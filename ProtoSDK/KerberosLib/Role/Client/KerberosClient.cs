// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Net;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.Types;
using Microsoft.Protocols.TestTools.StackSdk.Transport;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /// <summary>
    /// The KILE client, receives server PDUs and sends client PDUs.
    /// It is called by test cases to create, send or receive PDUs.
    /// </summary>
    public class KerberosClient : KerberosRole
    {
        #region members
        /// <summary>
        /// Represents whether this object has been disposed.
        /// </summary>
        private bool disposed;
        /// <summary>
        /// A TCP/UDP transport instance, sending and receiving PDUs with the KDC.
        /// </summary>
        private TransportStack kdcTransport;

        private Type expectedPduType;

        protected string kdcAddress;
        protected int kdcPort;
        protected TransportType transportType;
        protected KerberosConstValue.OidPkt oidPkt;

        #endregion members

        #region properties

        /// <summary>
        /// Contains all the important state variables in the context.
        /// </summary>
        public override KerberosContext Context
        {
            get;
            set;
        }

        /// <summary>
        /// The buffer size of transport stack. The default value is 1500.
        /// Set this value before call method Connect.
        /// </summary>
        public int TransportBufferSize
        {
            get;
            set;
        }

        /// <summary>
        /// A boolean value indicating whether KDC Proxy is used.
        /// </summary>
        public bool UseProxy
        {
            get;
            set;
        }

        /// <summary>
        /// KDC Proxy client used to send and receive proxy messages.
        /// It is used when UseProxy is TRUE.
        /// </summary>
        public KKDCPClient ProxyClient
        {
            get;
            set;
        }
        #endregion properties

        #region constructor
        /// <summary>
        /// Create a KileClient instance.
        /// </summary>
        /// <param name="domain">The realm part of the client's principal identifier.
        /// This argument cannot be null.</param>
        /// <param name="cName">The account to logon the remote machine. Either user account or computer account
        /// This argument cannot be null.</param>
        /// <param name="password">The password of the user. This argument cannot be null.</param>
        /// <param name="accountType">The type of the logon account. User or Computer</param>
        /// <param name="kdcAddress">The IP address of the KDC.</param>
        /// <param name="kdcPort">The port of the KDC.</param>
        /// <param name="transportType">Whether the transport is TCP or UDP transport.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the input parameter is null.</exception>
        public KerberosClient(string domain, string cName, string password, KerberosAccountType accountType, string kdcAddress, int kdcPort, TransportType transportType, KerberosConstValue.OidPkt oidPkt = KerberosConstValue.OidPkt.KerberosToken, string salt = null)
        {
            TransportBufferSize = KerberosConstValue.TRANSPORT_BUFFER_SIZE;
            this.Context = new KerberosContext(domain, cName, password, accountType, salt);
            this.kdcAddress = kdcAddress;
            this.kdcPort = kdcPort;
            this.transportType = transportType;
            this.oidPkt = oidPkt;
            this.Context.TransportType = transportType;
        }

        /// <summary>
        /// Create a KileClient instance.
        /// </summary>
        /// <param name="domain">The realm part of the client's principal identifier.
        /// This argument cannot be null.</param>
        /// <param name="cName">The account to logon the remote machine. Either user account or computer account
        /// This argument cannot be null.</param>
        /// <param name="password">The password of the user. This argument cannot be null.</param>
        /// <param name="accountType">The type of the logon account. User or Computer</param>
        /// <param name="kdcAddress">The IP address of the KDC.</param>
        /// <param name="kdcPort">The port of the KDC.</param>
        /// <param name="transportType">Whether the transport is TCP or UDP transport.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the input parameter is null.</exception>
        public KerberosClient(string domain, string cName, string password, KerberosAccountType accountType, KerberosTicket armorTicket, EncryptionKey armorSessionKey, string kdcAddress, int kdcPort, TransportType transportType, KerberosConstValue.OidPkt oidPkt = KerberosConstValue.OidPkt.KerberosToken, string salt = null)
        {
            TransportBufferSize = KerberosConstValue.TRANSPORT_BUFFER_SIZE;
            this.Context = new KerberosContext(domain, cName, password, accountType, salt, armorTicket, armorSessionKey);
            this.kdcAddress = kdcAddress;
            this.kdcPort = kdcPort;
            this.transportType = transportType;
            this.oidPkt = oidPkt;
            this.Context.TransportType = transportType;
        }
        #endregion constructor

        #region Connection with KDC
        /// <summary>
        /// Set up the TCP/UDP transport connection with KDC.
        /// </summary>
        /// <exception cref="System.ArgumentException">Thrown when the connection type is neither TCP nor UDP</exception>
        public virtual void Connect()
        {
            SocketTransportConfig transportConfig = new SocketTransportConfig();
            transportConfig.Role = Role.Client;
            transportConfig.MaxConnections = 1;
            transportConfig.BufferSize = TransportBufferSize;
            transportConfig.RemoteIpPort = kdcPort;
            transportConfig.RemoteIpAddress = IPAddress.Parse(kdcAddress);

            // For UDP bind
            if (transportConfig.RemoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                transportConfig.LocalIpAddress = IPAddress.Any;
            }
            else if (transportConfig.RemoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            {
                transportConfig.LocalIpAddress = IPAddress.IPv6Any;
            }

            if (transportType == TransportType.TCP)
            {
                transportConfig.Type = StackTransportType.Tcp;
            }
            else if (transportType == TransportType.UDP)
            {
                transportConfig.Type = StackTransportType.Udp;
            }
            else
            {
                throw new ArgumentException("ConnectionType can only be TCP or UDP.");
            }

            kdcTransport = new TransportStack(transportConfig, DecodePacketCallback);
            if (transportType == TransportType.TCP)
            {
                kdcTransport.Connect();
            }
            else
            {
                kdcTransport.Start();
            }
        }

        public virtual void DisConnect()
        {
            if (this.transportType == TransportType.TCP)
                kdcTransport.Disconnect();
        }
        #endregion

        #region Transport methods
        /// <summary>
        /// Encode a PDU to a binary stream. Then send the stream.
        /// The pdu could be got by calling method Create***Request or Create***Token.
        /// </summary>
        /// <param name="pdu">A specified type of a PDU. This argument cannot be null.
        /// If it is null, ArgumentNullException will be thrown.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the input parameter is null.</exception>
        public void SendPdu(KerberosPdu pdu)
        {
            if (pdu == null)
            {
                throw new ArgumentNullException("pdu");
            }
            this.UpdateContext(pdu);

            if (UseProxy)
            {
                Debug.Assert(ProxyClient != null, "Proxy Client should be set when using proxy.");

                //temporarily change the tranpsort type to TCP, since all proxy messages are TCP format.
                var oriTransportType = Context.TransportType;
                Context.TransportType = TransportType.TCP;
                KDCProxyMessage message = ProxyClient.MakeProxyMessage(pdu);
                //send proxy message
                ProxyClient.SendProxyRequest(message);
                //restore the original transport type
                Context.TransportType = oriTransportType;
            }
            else
            {
                this.Connect();
                kdcTransport.SendPacket(pdu);
            }
        }

        /// <summary>
        /// Send a byte array to KDC. This method is especially used for negative test.
        /// </summary>
        /// <param name="packetBuffer">The bytes to be sent. This argument cannot be null.
        /// If it is null, ArgumentNullException will be thrown.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the input parameter is null.</exception>
        public void SendBytes(byte[] packetBuffer)
        {
            if (packetBuffer == null)
            {
                throw new ArgumentNullException("packetBuffer");
            }
            this.Connect();
            kdcTransport.SendBytes(packetBuffer);
        }
        
        /// <summary>
        /// Expect to receive a PDU of any type from the remote host.
        /// </summary>
        /// <param name="timeout">Timeout of receiving PDU.</param>
        /// <returns>The expected PDU.</returns>
        /// <exception cref="System.TimeoutException">Thrown when the timeout parameter is negative.</exception>
        public KerberosPdu ExpectPdu(TimeSpan timeout, Type pduType = null)
        {
            this.expectedPduType = pduType;
            KerberosPdu pdu = null;
            if (UseProxy)
            {
                Debug.Assert(ProxyClient != null, "Proxy Client should be set when using proxy.");
                int consumedLength = 0;
                int expectedLength = 0;
                if (ProxyClient.Error == KKDCPError.STATUS_SUCCESS)
                {
                    KDCProxyMessage message = ProxyClient.GetProxyResponse();
                    //temporarily change the tranpsort type to TCP, since all proxy messages are TCP format.
                    var oriTransportType = Context.TransportType;
                    Context.TransportType = TransportType.TCP;
                    //get proxy message
                    pdu = getExpectedPduFromBytes(message.Message.kerb_message.ByteArrayValue, out consumedLength, out expectedLength);
                    //restore the original transport type
                    Context.TransportType = oriTransportType;
                }
            }
            else
            {
                if (timeout.TotalMilliseconds < 0)
                {
                    throw new TimeoutException(KerberosConstValue.TIMEOUT_EXCEPTION);
                }
                TransportEvent eventPacket = kdcTransport.ExpectTransportEvent(timeout);
                pdu = (KerberosPdu)eventPacket.EventObject;
                this.DisConnect();
            }
            return pdu;
        }
        #endregion

        #region Decoder
        /// <summary>
        /// Decode KILE PDUs from received message bytes
        /// </summary>
        /// <param name="endPoint">An endpoint from which the message bytes are received</param>
        /// <param name="receivedBytes">The received bytes to be decoded</param>
        /// <param name="consumedLength">Length of message bytes consumed by decoder</param>
        /// <param name="expectedLength">Length of message bytes the decoder expects to receive</param>
        /// <returns>The decoded KILE PDUs</returns>
        /// <exception cref="System.FormatException">thrown when a kile message type is unsupported</exception>
        public override KerberosPdu[] DecodePacketCallback(object endPoint,
                                                byte[] receivedBytes,
                                                out int consumedLength,
                                                out int expectedLength)
        {
            KerberosPdu pdu = getExpectedPduFromBytes(receivedBytes, out consumedLength, out expectedLength);
            // insufficient data, need to receive more
            if (pdu == null)
            {
                return null;
            }
            return new KerberosPdu[] { pdu };
        }

        //Get the expected Kerberos PDU from byte array
        private KerberosPdu getExpectedPduFromBytes(
            byte[] receivedBytes,
            out int consumedLength,
            out int expectedLength)
        {
            // initialize lengths
            consumedLength = 0;
            expectedLength = 0;
            if (null == receivedBytes || 0 == receivedBytes.Length)
            {
                return null;
            }

            // TCP has a 4 bytes length header, while UDP has not
            byte[] pduBytes = receivedBytes;

            if ((this.Context.TransportType == TransportType.TCP))
            {
                // insufficient data, needs to receive more
                if (receivedBytes.Length < sizeof(int))
                {
                    return null;
                }

                // get pdu data length
                byte[] lengthBytes = ArrayUtility.SubArray(receivedBytes, 0, sizeof(int));
                Array.Reverse(lengthBytes);
                int pduLength = BitConverter.ToInt32(lengthBytes, 0);

                // insufficient data, needs to receive more
                expectedLength = sizeof(int) + pduLength;
                if (receivedBytes.Length < expectedLength)
                {
                    return null;
                }

                // remove length header from pdu bytes
                pduBytes = ArrayUtility.SubArray<byte>(receivedBytes, sizeof(int), pduLength);
            }
            else
            {
                // UDP has no length header
                expectedLength = pduBytes.Length;
            }

            // decode according to message type
            consumedLength = expectedLength;

            // get message type
            // (the lower 5 bits indicates its kile message type)
            MsgType kileMessageType = (MsgType)(pduBytes[0] & 0x1f);
            KerberosPdu pdu = null;

            switch (kileMessageType)
            {
                case MsgType.KRB_AS_REQ:
                    pdu = new KerberosAsRequest();
                    break;

                case MsgType.KRB_AS_RESP:
                    pdu = new KerberosAsResponse();
                    break;

                case MsgType.KRB_TGS_REQ:
                    pdu = new KerberosTgsRequest();
                    break;

                case MsgType.KRB_TGS_RESP:
                    pdu = new KerberosTgsResponse();
                    break;

                case MsgType.KRB_ERROR:
                    pdu = new KerberosKrbError();
                    break;

                default:
                    throw new FormatException(
                        "Unsupported Message Type: " + kileMessageType.ToString());
            }
            pdu.FromBytes(pduBytes);
            // update context
            if (this.expectedPduType == null || this.expectedPduType == pdu.GetType())
                this.UpdateContext(pdu);
            return pdu;
        }
        #endregion

        #region Context
        /// <summary>
        /// Updated context based on Kerberos pdu
        /// </summary>
        /// <param name="pdu">Kerberos pdu</param>
        public override void UpdateContext(KerberosPdu pdu)
        {
            if (pdu is KerberosAsRequest)
            {
                KerberosAsRequest request = (KerberosAsRequest)pdu;
                UpdateContext(request);
            }
            if (pdu is KerberosKrbError)
            {
                KerberosKrbError error = (KerberosKrbError)pdu;
                UpdateContext(error);
            }
            if (pdu is KerberosAsResponse)
            {
                KerberosAsResponse response = (KerberosAsResponse)pdu;
                UpdateContext(response);
            }

            if (pdu is KerberosTgsRequest)
            {
                KerberosTgsRequest request = pdu as KerberosTgsRequest;
                UpdateContext(request);
            }

            if (pdu is KerberosTgsResponse)
            {
                KerberosTgsResponse response = pdu as KerberosTgsResponse;
                UpdateContext(response);
            }

            this.expectedPduType = null;
        }

        private void UpdateContext(KerberosAsRequest request)
        {
            if (request.Request != null && request.Request.req_body != null)
            {
                Context.Addresses = request.Request.req_body.addresses;
                Context.Nonce = request.Request.req_body.nonce;
            }
            if (request.Request.padata != null)
            {
                var padataList = request.Request.padata.Elements;
                foreach (var padata in padataList)
                {
                    var parsed = PaDataParser.ParseReqPaData(padata);
                    if (parsed is PaFxFastReq)
                    {
                        Context.ReplyKey = Context.FastArmorkey;
                    }
                }
            }
        }

        private void UpdateContext(KerberosAsResponse response)
        {
            KerberosFastResponse kerbFastRep = null;
            if (response.Response.padata != null && response.Response.padata.Elements != null)
            {
                foreach (PA_DATA paData in response.Response.padata.Elements)
                {
                    var parsedPaData = PaDataParser.ParseRepPaData(paData);
                    if (parsedPaData is PaETypeInfo2)
                    {
                        Asn1DecodingBuffer buffer = new Asn1DecodingBuffer(paData.padata_value.ByteArrayValue);
                        ETYPE_INFO2 eTypeInfo2 = new ETYPE_INFO2();
                        eTypeInfo2.BerDecode(buffer);
                        if (eTypeInfo2.Elements != null && eTypeInfo2.Elements.Length > 0)
                        {
                            // the salt is received from KDC
                            if (eTypeInfo2.Elements[0].salt != null)
                                Context.CName.Salt = eTypeInfo2.Elements[0].salt.Value;
                            continue;
                        }
                    }
                    if (parsedPaData is PaFxFastRep)
                    {
                        var armoredRep = ((PaFxFastRep)parsedPaData).GetArmoredRep();
                        kerbFastRep = ((PaFxFastRep)parsedPaData).GetKerberosFastRep(Context.FastArmorkey);
                        var strKey = kerbFastRep.FastResponse.strengthen_key;
                        Context.ReplyKey = KerberosUtility.KrbFxCf2(
                            strKey,
                            //Fix me: should be Context.ReplyKey
                            KerberosUtility.MakeKey(Context.SelectedEType, Context.CName.Password, Context.CName.Salt),
                            "strengthenkey",
                            "replykey");
                    }
                }
            }

            if (Context.ReplyKey != null)
            {
                response.Decrypt(Context.ReplyKey.keyvalue.ByteArrayValue);
            }
            else
            {
                var encryptType = (EncryptionType)response.Response.enc_part.etype.Value;
                var key = KeyGenerator.MakeKey(encryptType, Context.CName.Password, Context.CName.Salt);
                Context.ReplyKey = new EncryptionKey(new KerbInt32((long)encryptType), new Asn1OctetString(key));
                response.Decrypt(key);
            }

            if (response.EncPart != null)
            {
                Context.SessionKey = response.EncPart.key;
            }

            if (response.Response != null)
            {
                //Response.Response.cname is not the real CName of the ticket when hide-client-names=1 
                if (kerbFastRep != null && kerbFastRep.FastResponse != null && kerbFastRep.FastResponse.finished != null)
                {
                    // Windows DC is case insensitive. It may change the cname in the response, e.g. administrator -> Administrator
                    Context.CName.Name = kerbFastRep.FastResponse.finished.cname;
                    Context.Ticket = new KerberosTicket(response.Response.ticket, kerbFastRep.FastResponse.finished.cname, response.EncPart.key);
                }
                else
                {
                    // Windows DC is case insensitive. It may change the cname in the response, e.g. administrator -> Administrator
                    Context.CName.Name = response.Response.cname;
                    Context.Ticket = new KerberosTicket(response.Response.ticket, response.Response.cname, response.EncPart.key);
                }
                Context.SelectedEType = (EncryptionType)Context.Ticket.SessionKey.keytype.Value;
                if (Context.Ticket != null && Context.Ticket.Ticket.sname != null
                    && Context.Ticket.Ticket.sname.name_string != null
                    && Context.Ticket.Ticket.sname.name_string.Elements != null
                    && Context.Ticket.Ticket.sname.name_string.Elements.Length > 1)
                {
                    int count = Context.Ticket.Ticket.sname.name_string.Elements.Length;
                    Context.Realm = new Realm(Context.Ticket.Ticket.sname.name_string.Elements[count - 1].Value);
                }
            }
        }

        private void UpdateContext(KerberosKrbError error)
        {
            switch (error.ErrorCode)
            {
                case KRB_ERROR_CODE.KDC_ERR_PREAUTH_REQUIRED:
                    {
                        var seqOfPadata = new Asn1SequenceOf<PA_DATA>();
                        seqOfPadata.BerDecode(new Asn1DecodingBuffer(error.KrbError.e_data.ByteArrayValue));
                        var padataCount = seqOfPadata.Elements.Length;

                        for (int i = 0; i < padataCount; i++)
                        {
                            var padata = PaDataParser.ParseRepPaData(seqOfPadata.Elements[i]);
                            //Fix me: PaETypeInfo is also possible
                            if (padata is PaETypeInfo2)
                            {
                                var etypeinfo = padata as PaETypeInfo2;
                                string salt;
                                var etype = negotiateEtype(etypeinfo.ETypeInfo2.Elements, Context.SupportedEType.Elements, out salt);
                                Context.SelectedEType = (EncryptionType)etype;
                                if (!string.IsNullOrEmpty(salt)) Context.CName.Salt = salt;
                                Context.ReplyKey = KerberosUtility.MakeKey(Context.SelectedEType, Context.CName.Password, Context.CName.Salt);
                            }
                            if (padata is PaFxFastRep)
                            {
                                //Fix me: Do something.
                            }

                        }
                        break;
                    }
                default:
                    break;
            }
        }

        private long negotiateEtype(ETYPE_INFO2_ENTRY[] eTypeInfoEntries, KerbInt32[] eTypes, out string salt)
        {
            foreach (var entry in eTypeInfoEntries)
            {
                foreach (var etype in eTypes)
                {
                    if (entry.etype.Value == etype.Value)
                    {
                        if (entry.salt != null) salt = entry.salt.ToString();
                        else salt = null;
                        return (long) etype.Value;
                    }
                }
            }
            throw new Exception("Negotiage e-type failed.");
        }

        private void UpdateContext(KerberosTgsRequest request)
        {
            Context.Nonce = request.Request.req_body.nonce;
            if (Context.Subkey == null)
            {
                Context.ReplyKey = Context.SessionKey;
            }
            else
            {
                Context.ReplyKey = Context.Subkey;
            }
        }

        private void UpdateContext(KerberosTgsResponse response)
        {
            if (response.Response != null)
            {
                if (response.Response.padata != null && response.Response.padata.Elements != null)
                {
                    foreach (PA_DATA paData in response.Response.padata.Elements)
                    {
                        var parsedPaData = PaDataParser.ParseRepPaData(paData);
                        if (parsedPaData is PaFxFastRep)
                        {
                            var armoredRep = ((PaFxFastRep)parsedPaData).GetArmoredRep();
                            var kerbRep = ((PaFxFastRep)parsedPaData).GetKerberosFastRep(Context.FastArmorkey);
                            var strKey = kerbRep.FastResponse.strengthen_key;
                            Context.ReplyKey = KerberosUtility.KrbFxCf2(strKey, Context.ReplyKey, "strengthenkey", "replykey");
                        }
                    }
                }
                KeyUsageNumber usage =
                    Context.Subkey == null ? KeyUsageNumber.TGS_REP_encrypted_part : KeyUsageNumber.TGS_REP_encrypted_part_subkey;
                response.DecryptTgsResponse(Context.ReplyKey.keyvalue.ByteArrayValue, usage);
                Context.SessionKey = response.EncPart.key;
                //Fix me: when hide-client-names is set to true, response.Response.cname is not the real CName.
                Context.Ticket = new KerberosTicket(response.Response.ticket, response.Response.cname, response.EncPart.key);
                Context.SelectedEType = (EncryptionType)Context.Ticket.Ticket.enc_part.etype.Value;
            }
        }

        public void ChangeRealm(string realm, string kdcAddress, int kdcPort, TransportType transportType)
        {
            this.Context.Realm = new Realm(realm);
            this.kdcAddress = kdcAddress;
            this.kdcPort = kdcPort;
            this.transportType = transportType;
            if (ProxyClient != null)
            {
                ProxyClient.TargetDomain = realm;
            }
        }
        #endregion

        #region IDisposable

        /// <summary>
        /// Release resources.
        /// </summary>
        /// <param name="disposing">If disposing equals true, Managed and unmanaged resources are disposed.
        /// if false, Only unmanaged resources can be disposed.</param>
        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //Release managed resource.
                    if (kdcTransport != null)
                    {
                        kdcTransport.Dispose();
                        kdcTransport = null;
                    }
                }

                //Note disposing has been done.
                disposed = true;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
