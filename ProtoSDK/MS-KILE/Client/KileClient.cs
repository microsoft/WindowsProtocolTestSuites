// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;

using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Transport;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Kile
{
    /// <summary>
    /// The KILE client, receives server PDUs and sends client PDUs.
    /// It is called by test cases to create, send or receive PDUs.
    /// </summary>
    public class KileClient : KileRole
    {
        #region members
        /// <summary>
        /// Represents whether this object has been disposed.
        /// </summary>
        private bool disposed;
        /// <summary>
        /// Contains all the important state variables in the context.
        /// </summary>
        private KileClientContext context;

        /// <summary>
        /// A class to decode received PDUs.
        /// </summary>
        private KileDecoder decoder;

        /// <summary>
        /// A TCP/UDP transport instance, sending and receiving PDUs with the KDC.
        /// </summary>
        private TransportStack kdcTransport;

        /// <summary>
        /// The buffer size of transport stack.
        /// </summary>
        private int transportBufferSize;

        /// <summary>
        /// The realm part of the client's principal identifier.
        /// </summary>
        private string domain;

        /// <summary>
        /// The user name to logon the remote machine.
        /// </summary>
        private string userName;

        /// <summary>
        /// The password of the user.
        /// </summary>
        private string password;
        #endregion members


        #region properties

        /// <summary>
        /// Contains all the important state variables in the context.
        /// </summary>
        public KileClientContext ClientContext
        {
            get
            {
                return context;
            }
        }

        /// <summary>
        /// Contains all the important state variables in the context.
        /// </summary>
        internal override KileContext Context
        {
            get
            {
                return context;
            }
        }


        /// <summary>
        /// The buffer size of transport stack. The default value is 1500.
        /// Set this value before call method Connect.
        /// </summary>
        public int TransportStackBufferSize
        {
            get
            {
                return transportBufferSize;
            }
            set
            {
                transportBufferSize = value;
            }
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
        /// <exception cref="System.ArgumentNullException">Thrown when the input parameter is null.</exception>
        public KileClient(string domain, string cName, string password, KileAccountType accountType)
        {
            if (domain == null)
            {
                throw new ArgumentNullException(nameof(domain));
            }
            if (cName == null)
            {
                throw new ArgumentNullException(nameof(cName));
            }
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            context = new KileClientContext();
            decoder = new KileDecoder(context);
            transportBufferSize = ConstValue.TRANSPORT_BUFFER_SIZE;
            this.domain = domain;
            this.userName = cName;
            this.password = password;

            context.Password = password;
            context.Salt = GenerateSalt(domain, cName, accountType);
        }
        #endregion constructor


        /// <summary>
        /// Set up the TCP/UDP transport connection with KDC.
        /// </summary>
        /// <param name="kdcAddress">The IP address of the KDC.</param>
        /// <param name="kdcPort">The port of the KDC.</param>
        /// <param name="transportType">Whether the transport is TCP or UDP transport.</param>
        /// <exception cref="System.ArgumentException">Thrown when the connection type is neither TCP nor UDP</exception>
        public void Connect(string kdcAddress, int kdcPort, KileConnectionType transportType)
        {
            var transportConfig = new SocketTransportConfig();
            transportConfig.Role = Role.Client;
            transportConfig.MaxConnections = 1;
            transportConfig.BufferSize = transportBufferSize;
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

            if (transportType == KileConnectionType.TCP)
            {
                transportConfig.Type = StackTransportType.Tcp;
            }
            else if (transportType == KileConnectionType.UDP)
            {
                transportConfig.Type = StackTransportType.Udp;
            }
            else
            {
                throw new ArgumentException("ConnectionType can only be TCP or UDP.");
            }

            kdcTransport = new TransportStack(transportConfig, decoder.DecodePacketCallback);
            if (transportType == KileConnectionType.TCP)
            {
                kdcTransport.Connect();
            }
            else
            {
                kdcTransport.Start();
            }

            context.TransportType = transportType;
        }


        #region packet api
        /// <summary>
        /// Create AS request.
        /// </summary>
        /// <param name="sName">The server principle name. This argument can be null.</param>
        /// <param name="kdcOptions">The options of request body.</param>
        /// <param name="paData">The pre-authentication data in AS request. 
        /// This argument can be generated by method ConstructPaData. This argument can be null.</param> 
        /// <param name="encryptionTypes">The encryption types client supported. This argument cannot be null.</param>
        /// <returns>The created AS request.</returns>
        [CLSCompliant(false)]
        public KileAsRequest CreateAsRequest(string sName,
                                         KRBFlags kdcOptions,
                                         Asn1SequenceOf<PA_DATA> paData,
                                         params EncryptionType[] encryptionTypes)
        {
            var request = new KileAsRequest(context);
            request.Request.msg_type = new Asn1Integer((int)MsgType.KRB_AS_REQ);
            request.Request.pvno = new Asn1Integer(ConstValue.KERBEROSV5);
            request.Request.padata = paData;

            request.Request.req_body = new KDC_REQ_BODY();
            request.Request.req_body.kdc_options = new KDCOptions(KileUtility.ConvertInt2Flags((int)kdcOptions));
            request.Request.req_body.nonce = new KerbUInt32((uint)Math.Abs((int)DateTime.Now.Ticks));
            request.Request.req_body.till = new KerberosTime(ConstValue.TGT_TILL_TIME);
            request.Request.req_body.rtime = new KerberosTime(ConstValue.TGT_RTIME);
            request.Request.req_body.addresses =
                new HostAddresses(new HostAddress[1] { new HostAddress(new KerbInt32((int)AddressType.NetBios),
                    new Asn1OctetString(Encoding.ASCII.GetBytes(System.Net.Dns.GetHostName()))) });

            if (userName != null)
            {
                request.Request.req_body.cname =
                    new PrincipalName(new KerbInt32((int)PrincipalType.NT_PRINCIPAL), KerberosUtility.String2SeqKerbString(userName));
            }

            if (domain != null)
            {
                request.Request.req_body.realm = new Realm(domain);
            }

            if (sName != null)
            {
                request.Request.req_body.sname =
                    new PrincipalName(new KerbInt32((int)PrincipalType.NT_SRV_INST), KileUtility.String2SeqKerbString(sName, domain));
            }

            if (encryptionTypes != null)
            {
                var etypes = new KerbInt32[encryptionTypes.Length];
                for (int i = 0; i < encryptionTypes.Length; i++)
                {
                    etypes[i] = new KerbInt32((int)encryptionTypes[i]);
                }

                request.Request.req_body.etype = new Asn1SequenceOf<KerbInt32>(etypes);
            }

            return request;
        }


        /// <summary>
        /// Create TGS request.
        /// </summary>
        /// <param name="sName">The server principle name. This argument cannot be null.</param>
        /// <param name="kdcOptions">The options of request body.</param>
        /// <param name="nonce">This parameter is intended to hold a random number generated by the client.</param>
        /// <param name="paData">The pre-authentication data in TGS request. This argument does not include 
        /// PaDataType PA_TGS_REQ. Type PA_TGS_REQ will be created in this method. 
        /// This argument can be generated by method ConstructPaData. This argument can be null.</param>
        /// <param name="checksumType">The checksum type in Authenticator of PaData.</param>
        /// <param name="additionalTicket">An additional ticket of request body. This field is optional.</param>
        /// <param name="authorizationData">The authentication data of request body. This field is optional.
        /// This argument can be generated by method ConstructAuthorizationData.</param>
        /// <returns>The created TGS request.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the input parameter is null.</exception>
        [CLSCompliant(false)]
        public KileTgsRequest CreateTgsRequest(string sName,
                                           KRBFlags kdcOptions,
                                           KerbUInt32 nonce,
                                           Asn1SequenceOf<PA_DATA> paData,
                                           ChecksumType checksumType,
                                           Ticket additionalTicket,
                                           AuthorizationData authorizationData)
        {
            if (sName == null)
            {
                throw new ArgumentNullException(nameof(sName));
            }
            var sname = new PrincipalName(new KerbInt32((int)PrincipalType.NT_SRV_INST),
                KileUtility.String2SeqKerbString(sName.Split('/')));
            return CreateTgsRequest(context.UserRealm, context.UserName, sname, kdcOptions, nonce, context.UserRealm, paData,
                checksumType, additionalTicket, authorizationData);
        }


        /// <summary>
        /// Create TGS request.
        /// </summary>
        /// <param name="cRealm">This field contains the name of the realm in which the client is registered and in 
        /// which initial authentication took place. This argument cannot be null.</param>
        /// <param name="cName">This field contains the name part of the client's principal identifier.
        /// This argument cannot be null.</param> 
        /// <param name="sName">The server principle name. This argument cannot be null.</param>
        /// <param name="kdcOptions">The options of request body.</param>
        /// <param name="nonce">This parameter is intended to hold a random number generated by the client.</param>
        /// <param name="realm">Server's realm. This argument cannot be null.</param>
        /// <param name="paData">The pre-authentication data in TGS request. This argument does not include 
        /// PaDataType PA_TGS_REQ. Type PA_TGS_REQ will be created in this method. 
        /// This argument can be generated by method ConstructPaData. This argument can be null.</param>
        /// <param name="checksumType">The checksum type in Authenticator of PaData.</param>
        /// <param name="additionalTicket">An additional ticket of request body. This field is optional.</param>
        /// <param name="authorizationData">The authentication data of request body. This field is optional.
        /// This argument can be generated by method ConstructAuthorizationData.</param>
        /// <returns>The created TGS request.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the input parameter is null.</exception>
        [CLSCompliant(false)]
        public KileTgsRequest CreateTgsRequest(
            Realm cRealm,
            PrincipalName cName,
            PrincipalName sName,
            KRBFlags kdcOptions,
            KerbUInt32 nonce,
            Realm realm,
            Asn1SequenceOf<PA_DATA> paData,
            ChecksumType checksumType,
            Ticket additionalTicket,
            AuthorizationData authorizationData)
        {
            if (cRealm == null)
            {
                throw new ArgumentNullException(nameof(cRealm));
            }
            if (cName == null)
            {
                throw new ArgumentNullException(nameof(cName));
            }
            if (sName == null)
            {
                throw new ArgumentNullException(nameof(sName));
            }
            if (realm == null)
            {
                throw new ArgumentNullException(nameof(realm));
            }

            var request = new KileTgsRequest(context);
            request.Request.msg_type = new Asn1Integer((int)MsgType.KRB_TGS_REQ);
            request.Request.pvno = new Asn1Integer(ConstValue.KERBEROSV5);

            #region construct req_body
            request.Request.req_body = new KDC_REQ_BODY();
            request.Request.req_body.kdc_options = new KDCOptions(KileUtility.ConvertInt2Flags((int)kdcOptions));
            request.Request.req_body.nonce = nonce;
            request.Request.req_body.till = new KerberosTime(ConstValue.TGT_TILL_TIME);
            request.Request.req_body.etype = context.ClientEncryptionTypes;
            request.Request.req_body.realm = realm;

            if (additionalTicket != null)
            {
                request.Request.req_body.additional_tickets = new Asn1SequenceOf<Ticket>(new Ticket[] { additionalTicket });
            }
            request.Request.req_body.sname = sName;
            request.EncAuthorizationData = authorizationData;

            if (authorizationData != null)
            {
                var asnBuffer = new Asn1BerEncodingBuffer();
                authorizationData.BerEncode(asnBuffer, true);

                request.Request.req_body.enc_authorization_data = new EncryptedData();
                request.Request.req_body.enc_authorization_data.etype = new KerbInt32(0);
                byte[] encAsnEncoded = asnBuffer.Data;
                if (context.TgsSessionKey != null && context.TgsSessionKey.keytype != null
                    && context.TgsSessionKey.keyvalue != null && context.TgsSessionKey.keyvalue.Value != null)
                {
                    encAsnEncoded = KileUtility.Encrypt((EncryptionType)context.TgsSessionKey.keytype.Value,
                                                        context.TgsSessionKey.keyvalue.ByteArrayValue,
                                                        asnBuffer.Data,
                                                        (int)KeyUsageNumber.TGS_REQ_KDC_REQ_BODY_AuthorizationData);
                    request.Request.req_body.enc_authorization_data.etype =
                        new KerbInt32(context.TgsSessionKey.keytype.Value);
                }

                request.Request.req_body.enc_authorization_data.cipher = new Asn1OctetString(encAsnEncoded);
            }
            #endregion construct req_body

            #region construct PA_DATA
            var bodyBuffer = new Asn1BerEncodingBuffer();
            request.Request.req_body.BerEncode(bodyBuffer);
            PA_DATA tgsPaData = ConstructTgsPaData(cRealm, cName, checksumType, bodyBuffer.Data);

            request.Request.padata = new Asn1SequenceOf<PA_DATA>();
            if (paData == null || paData.Elements == null || paData.Elements.Length == 0)
            {
                request.Request.padata.Elements = new PA_DATA[] { tgsPaData };
            }
            else
            {
                request.Request.padata.Elements = new PA_DATA[paData.Elements.Length + 1];
                Array.Copy(paData.Elements, request.Request.padata.Elements, paData.Elements.Length);
                request.Request.padata.Elements[paData.Elements.Length] = tgsPaData;
            }
            #endregion construct PA_DATA

            return request;
        }


        /// <summary>
        /// Create AP request. Then use KilePdu.ToBytes() to get the byte array.
        /// </summary>
        /// <param name="apOptions">The AP options in AP request.</param>
        /// <param name="checksumType">The checksum type selected.
        /// It should be ChecksumType.ap_authenticator_8003.</param>
        /// <param name="seqNumber">The current local sequence number.</param>
        /// <param name="flag">The flag set in checksum field of Authenticator.</param>
        /// <param name="subkey">Specify the new subkey used in the following exchange.
        /// This argument can be got with method GenerateKey(ApSessionKey).
        /// If this argument is null, no subkey will be sent.</param>
        /// <param name="authorizationData">The authentication data of authenticator.
        /// This argument can be generated by method ConstructAuthorizationData.
        /// If this argument is null, no Authorization Data will be sent.</param>
        /// <returns>The created AP request.</returns>
        [CLSCompliant(false)]
        public KileApRequest CreateApRequest(ApOptions apOptions,
                                         ChecksumType checksumType,
                                         int seqNumber,
                                         ChecksumFlags flag,
                                         EncryptionKey subkey,
                                         AuthorizationData authorizationData)
        {
            return CreateApRequest(context.UserRealm, context.UserName, apOptions, checksumType, seqNumber, flag,
                subkey, authorizationData);
        }


        /// <summary>
        /// Create AP request. Then use KilePdu.ToBytes() to get the byte array.
        /// </summary>
        /// <param name="cRealm">This field contains the name of the realm in which the client is registered and in 
        /// which initial authentication took place. This argument cannot be null.</param>
        /// <param name="cName">This field contains the name part of the client's principal identifier.
        /// This argument cannot be null.</param>
        /// <param name="apOptions">The AP options in AP request.</param>
        /// <param name="checksumType">The checksum type selected.
        /// It should be ChecksumType.ap_authenticator_8003.</param>
        /// <param name="seqNumber">The current local sequence number.</param>
        /// <param name="flag">The flag set in checksum field of Authenticator.</param>
        /// <param name="subkey">Specify the new subkey used in the following exchange.
        /// This argument can be got with method GenerateKey(ApSessionKey).
        /// If this argument is null, no subkey will be sent.</param>
        /// <param name="authorizationData">The authentication data of authenticator.
        /// This argument can be generated by method ConstructAuthorizationData.
        /// If this argument is null, no Authorization Data will be sent.</param>
        /// <returns>The created AP request.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the input parameter is null.</exception>
        [CLSCompliant(false)]
        public KileApRequest CreateApRequest(
            Realm cRealm,
            PrincipalName cName,
            ApOptions apOptions,
            ChecksumType checksumType,
            int seqNumber,
            ChecksumFlags flag,
            EncryptionKey subkey,
            AuthorizationData authorizationData)
        {
            if (cRealm == null)
            {
                throw new ArgumentNullException(nameof(cRealm));
            }
            if (cName == null)
            {
                throw new ArgumentNullException(nameof(cName));
            }
            var request = new KileApRequest(context);
            request.Authenticator = CreateAuthenticator(cRealm,
                                                        cName,
                                                        checksumType,
                                                        seqNumber,
                                                        flag,
                                                        subkey,
                                                        authorizationData,
                                                        context.ApSessionKey,
                                                        null);

            request.Request.ap_options = new APOptions(KileUtility.ConvertInt2Flags((int)apOptions));
            request.Request.msg_type = new Asn1Integer((int)MsgType.KRB_AP_REQ);
            request.Request.pvno = new Asn1Integer(ConstValue.KERBEROSV5);
            request.Request.ticket = context.ApTicket;

            return request;
        }


        /// <summary>
        /// Decode the AP response token. This method is used for mutual authentication.
        /// This method should be used after sending an application message with AP request token.
        /// </summary>
        /// <param name="apResponseToken">The token got from an application message.
        /// This argument cannot be null.</param>
        /// <returns>The decoded AP response.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the input parameter is null.</exception>
        public KileApResponse ParseApResponse(byte[] apResponseToken)
        {
            if (apResponseToken == null)
            {
                throw new ArgumentNullException(nameof(apResponseToken));
            }

            var response = new KileApResponse(context);
            response.FromBytes(apResponseToken);
            return response;
        }


        /// <summary>
        /// Create KRB_CRED. This PDU should be sent after AP exchange successfully.
        /// </summary>
        /// <returns>The created KRB_CRED.</returns>
        public KrbCred CreateKrbCredRequest()
        {
            var cred = new KrbCred(context);
            cred.KerberosCred.msg_type = new Asn1Integer((int)MsgType.KRB_CRED);
            cred.KerberosCred.pvno = new Asn1Integer(ConstValue.KERBEROSV5);

            var ticket = new Ticket[] { context.ApTicket };
            cred.KerberosCred.tickets = new Asn1SequenceOf<Ticket>(ticket);

            EncryptionKey key = context.ContextKey;
            cred.CredEncPart = ConstrutCredEncryptedData(key);

            return cred;
        }


        /// <summary>
        /// Create KRB_PRIV. This PDU should be sent after AP exchange successfully.
        /// </summary>
        /// <param name="krbPrivRequest">Specify if sequence number or timestamp will be used.</param>
        /// <param name="userData">The user data want to send.</param>
        /// <returns>The created KRB_PRIV.</returns>
        public KrbPriv CreateKrbPrivRequest(KRB_PRIV_REQUEST krbPrivRequest, byte[] userData)
        {
            var priv = new KrbPriv(context);
            priv.KerberosPriv.msg_type = new Asn1Integer((int)MsgType.KRB_PRIV);
            priv.KerberosPriv.pvno = new Asn1Integer(ConstValue.KERBEROSV5);

            priv.PrivEncPart = ConstructEncKrbPrivPart(krbPrivRequest, userData);
            return priv;
        }


        /// <summary>
        /// Decode the KRB_ERROR token got from application.
        /// </summary>
        /// <param name="errorToken">The token got from an application message. This argument cannot be null.</param>
        /// <returns>The decoded AP response.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the input parameter is null.</exception>
        /// <exception cref="System.FormatException">Thrown when the errorToken is not valid.</exception>
        public KileKrbError ParseKrbError(byte[] errorToken)
        {
            if (errorToken == null)
            {
                throw new ArgumentNullException(nameof(errorToken));
            }

            byte[] errorBody = KileUtility.VerifyGssApiTokenHeader(errorToken);

            // Check if it has a two-byte tok_id
            if (errorBody == null || errorBody.Length <= sizeof(TOK_ID))
            {
                throw new FormatException("Not a valid KRB_ERROR token!");
            }

            TOK_ID id = (TOK_ID)KileUtility.ConvertEndian(BitConverter.ToUInt16(errorBody, 0));
            if (id != TOK_ID.KRB_ERROR)
            {
                throw new FormatException("Not a valid KRB_ERROR token!");
            }

            errorBody = ArrayUtility.SubArray(errorBody, sizeof(TOK_ID));
            var error = new KileKrbError();
            error.FromBytes(errorBody);
            return error;
        }

        #endregion


        #region helper methods

        #region Pa Data

        /// <summary>
        /// Construct Pre-authentication Data. User can add the PaData they are interested in.
        /// All the types of PaData can be created by the user themselves. 
        /// For example, to create a PKCA type PaData, user could use PKCA data to new a PaPkcaData type.
        /// This method is commonly used by client side.
        /// </summary>
        /// <param name="paData">The PaData user wants to construct. This argument can be null.</param>
        /// <returns>The constructed PaData.</returns>
        [CLSCompliant(false)]
        public Asn1SequenceOf<PA_DATA> ConstructPaData(params PaData[] paData)
        {
            return ConstructPaData(context.Password, context.Salt, paData);
        }

        #endregion Pa Data


        /// <summary>
        /// Generate a new key based on the given key for encryption.
        /// </summary>
        /// <param name="baseKey">The base key. This argument can be null. 
        /// If it is null, then null will be returned.</param>
        /// <returns>The new key.</returns>
        [CLSCompliant(false)]
        public EncryptionKey GenerateKey(EncryptionKey baseKey)
        {
            if (baseKey == null || baseKey.keytype == null
                || baseKey.keyvalue == null || baseKey.keyvalue.Value == null)
            {
                return null;
            }

            byte[] keyBuffer = KileUtility.GenerateRandomBytes((uint)baseKey.keyvalue.ByteArrayValue.Length);
            var newKey = new EncryptionKey(new KerbInt32(baseKey.keytype.Value), new Asn1OctetString(keyBuffer));
            return newKey;
        }
        #endregion helper methods


        #region transport methods
        /// <summary>
        /// Encode a PDU to a binary stream. Then send the stream.
        /// The pdu could be got by calling method Create***Request or Create***Token.
        /// </summary>
        /// <param name="pdu">A specified type of a PDU. This argument cannot be null.
        /// If it is null, ArgumentNullException will be thrown.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the input parameter is null.</exception>
        public void SendPdu(KilePdu pdu)
        {
            if (pdu == null)
            {
                throw new ArgumentNullException(nameof(pdu));
            }

            context.UpdateContext(pdu);
            kdcTransport.SendPacket(pdu);
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
                throw new ArgumentNullException(nameof(packetBuffer));
            }

            kdcTransport.SendBytes(packetBuffer);
        }


        /// <summary>
        /// Expect to receive a PDU of any type from the remote host.
        /// </summary>
        /// <param name="timeout">Timeout of receiving PDU.</param>
        /// <returns>The expected PDU.</returns>
        /// <exception cref="System.TimeoutException">Thrown when the timeout parameter is negative.</exception>
        public KilePdu ExpectPdu(TimeSpan timeout)
        {
            if (timeout.TotalMilliseconds < 0)
            {
                throw new TimeoutException(ConstValue.TIMEOUT_EXCEPTION);
            }

            TransportEvent eventPacket = kdcTransport.ExpectTransportEvent(timeout);
            KilePdu packet = (KilePdu)eventPacket.EventObject;

            return packet;
        }
        #endregion transport methods


        #region private methods
        /// <summary>
        /// Create authenticator for AP request or part of PA-DATA for TGS request.
        /// </summary>
        /// <param name="cRealm">This field contains the name of the realm in which the client is registered and in 
        /// which initial authentication took place.</param>
        /// <param name="cName">This field contains the name part of the client's principal identifier.</param>
        /// <param name="checksumType">The checksum type selected.</param>
        /// <param name="seqNumber">The current local sequence number.</param>
        /// <param name="flag">The flag set in checksum field of Authenticator.</param>
        /// <param name="subkey">Specify the new subkey used in the following exchange. This field is optional.
        /// This argument can be got with method GenerateKey(ApSessionKey).
        /// This argument can be null. If this argument is null, no subkey will be sent.</param>
        /// <param name="authorizationData">The authentication data of authenticator. This field is optional.
        /// This argument can be generated by method ConstructAuthorizationData. This argument can be null.
        /// If this argument is null, no Authorization Data will be sent.</param>
        /// <param name="key">The key to do checksum.</param>
        /// <param name="checksumBody">The data to compute checksum.</param>
        /// <returns>The created authenticator.</returns>
        private Authenticator CreateAuthenticator(Realm cRealm,
                                                  PrincipalName cName,
                                                  ChecksumType checksumType,
                                                  int seqNumber,
                                                  ChecksumFlags flag,
                                                  EncryptionKey subkey,
                                                  AuthorizationData authorizationData,
                                                  EncryptionKey key,
                                                  byte[] checksumBody)
        {
            var plaintextAuthenticator = new Authenticator();
            plaintextAuthenticator.authenticator_vno = new Asn1Integer(ConstValue.KERBEROSV5);
            plaintextAuthenticator.crealm = cRealm;
            plaintextAuthenticator.cname = cName;
            plaintextAuthenticator.cusec = new Microseconds(0);
            plaintextAuthenticator.ctime = KileUtility.CurrentKerberosTime;
            plaintextAuthenticator.seq_number = new KerbUInt32(seqNumber);
            plaintextAuthenticator.subkey = subkey;
            plaintextAuthenticator.authorization_data = authorizationData;

            if (checksumType == ChecksumType.ap_authenticator_8003)
            {
                // compute the checksum
                var checksum = new AuthCheckSum();
                checksum.Lgth = ConstValue.AUTHENTICATOR_CHECKSUM_LENGTH;
                checksum.Bnd = new byte[checksum.Lgth];
                checksum.Flags = (int)flag;
                byte[] checkData = ArrayUtility.ConcatenateArrays(BitConverter.GetBytes(checksum.Lgth),
                                                                  checksum.Bnd,
                                                                  BitConverter.GetBytes(checksum.Flags));
                // in AP request
                plaintextAuthenticator.cksum = new Checksum(new KerbInt32((int)checksumType), new Asn1OctetString(checkData));
            }
            else
            {
                // in TGS PA data
                byte[] checkData = KileUtility.GetChecksum(
                    key.keyvalue.ByteArrayValue,
                    checksumBody,
                    (int)KeyUsageNumber.TGS_REQ_PA_TGS_REQ_adataOR_AP_REQ_Authenticator_cksum,
                    checksumType);

                plaintextAuthenticator.cksum = new Checksum(new KerbInt32((int)checksumType), new Asn1OctetString(checkData));

            }

            return plaintextAuthenticator;
        }


        /// <summary>
        /// Construct PA_TGS_REQ for TGS request.
        /// </summary>
        /// <param name="cRealm">This field contains the name of the realm in which the client is registered and in 
        /// which initial authentication took place.</param>
        /// <param name="cName">This field contains the name part of the client's principal identifier.</param>
        /// <param name="checksumType">The checksum type in Authenticator.</param>
        /// <param name="checksumBody">The data to compute checksum.</param>
        /// <returns>The constructed PaData.</returns>
        private PA_DATA ConstructTgsPaData(Realm cRealm, PrincipalName cName, ChecksumType checksumType, byte[] checksumBody)
        {
            var request = new AP_REQ();

            KerbAuthDataTokenRestrictions adRestriction =
                    ConstructKerbAuthDataTokenRestrictions(0,
                    (uint)LSAP_TOKEN_INFO_INTEGRITY_Flags.FULL_TOKEN,
                    (uint)LSAP_TOKEN_INFO_INTEGRITY_TokenIL.Medium,
                    new Guid().ToString());
            AuthorizationData authData = ConstructAuthorizationData(adRestriction);

            // create and encrypt authenticator
            Authenticator authenticator = CreateAuthenticator(cRealm,
                                                              cName,
                                                              checksumType,
                                                              0,
                                                              0,
                                                              null,
                                                              authData,
                                                              context.TgsSessionKey,
                                                              checksumBody);
            var asnBuffPlainAuthenticator = new Asn1BerEncodingBuffer();
            authenticator.BerEncode(asnBuffPlainAuthenticator, true);
            byte[] encAsnEncodedAuth =
                KileUtility.Encrypt((EncryptionType)context.TgsSessionKey.keytype.Value,
                                    context.TgsSessionKey.keyvalue.ByteArrayValue,
                                    asnBuffPlainAuthenticator.Data,
                                    (int)KeyUsageNumber.TG_REQ_PA_TGS_REQ_padataOR_AP_REQ_Authenticator);
            request.authenticator = new EncryptedData();
            request.authenticator.etype = new Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32(context.TgsSessionKey.keytype.Value);
            request.authenticator.cipher = new Asn1OctetString(encAsnEncodedAuth);

            // create AP request
            request.ap_options = new APOptions(KileUtility.ConvertInt2Flags((int)ApOptions.None));
            request.msg_type = new Asn1Integer((int)MsgType.KRB_AP_REQ);
            request.pvno = new Asn1Integer(ConstValue.KERBEROSV5);
            request.ticket = context.TgsTicket;
            var apBerBuffer = new Asn1BerEncodingBuffer();
            request.BerEncode(apBerBuffer, true);

            return new PA_DATA(new KerbInt32((int)PaDataType.PA_TGS_REQ), new Asn1OctetString(apBerBuffer.Data));
        }


        /// <summary>
        /// Construct an kerbcred EncryptedData.
        /// </summary>
        /// <param name="key">The key to do encryption.</param>
        /// <returns>The EncKrbCredPart.</returns>
        private EncKrbCredPart ConstrutCredEncryptedData(EncryptionKey key)
        {
            var encKrbCred = new EncKrbCredPart();
            encKrbCred.nonce = new KerbUInt32((uint)Math.Abs((int)DateTime.Now.Ticks));
            encKrbCred.timestamp = new KerberosTime(KileUtility.GetCurrentUTCTime());
            encKrbCred.usec = new Microseconds(0);
            encKrbCred.s_address = new HostAddress(new KerbInt32((int)AddressType.NetBios),
                                                   new Asn1OctetString(Dns.GetHostName()));

            var krbCredInfo = new KrbCredInfo[1];
            krbCredInfo[0] = new KrbCredInfo();
            krbCredInfo[0].key = key;
            encKrbCred.ticket_info = new Asn1SequenceOf<KrbCredInfo>(krbCredInfo);

            return encKrbCred;
        }


        /// <summary>
        /// Construct EncKrbPrivPartof of KrbPrivRequest
        /// </summary>
        /// <param name="krbPrivRequest">to decide whether an seq_number in EncKrbPrivPart or not</param>
        /// <param name="userData">The user data want to send.</param>
        /// <returns>The EncKrbPrivPart.</returns>
        private EncKrbPrivPart ConstructEncKrbPrivPart(KRB_PRIV_REQUEST krbPrivRequest, byte[] userData)
        {
            var encKrbPriv = new EncKrbPrivPart();
            encKrbPriv.s_address = new HostAddress(new KerbInt32((int)AddressType.NetBios),
                                                   new Asn1OctetString(Dns.GetHostName()));
            encKrbPriv.usec = new Microseconds(0);
            encKrbPriv.user_data = new Asn1OctetString(userData);

            if (krbPrivRequest == KRB_PRIV_REQUEST.KrbPrivWithSequenceNumber)
            {
                encKrbPriv.seq_number = new KerbUInt32((long)context.CurrentLocalSequenceNumber);
                encKrbPriv.timestamp = null;
            }
            else
            {
                encKrbPriv.seq_number = null;
                encKrbPriv.timestamp = new KerberosTime(KileUtility.GetCurrentUTCTime());
            }

            return encKrbPriv;
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
