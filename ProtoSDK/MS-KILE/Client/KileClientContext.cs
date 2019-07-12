// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Kile
{
    /// <summary>
    /// Maintain the important parameters during KILE transport, including the main sent or received PDUs, 
    /// TGS session key, application session key, checksum algorithm etc. It is called by KileClient,
    /// KilePdu and KileDecoder.
    /// </summary>
    public class KileClientContext : KileContext
    {
        #region private members
        /// <summary>
        /// The lock of the context.
        /// </summary>
        private object contextLock;

        /// <summary>
        /// The key of the ticket returned by AS response.
        /// </summary>
        private byte[] asTgtKey;

        /// <summary>
        /// The encryption types supported by client. Get from AS request
        /// </summary>
        private Asn1SequenceOf<KerbInt32> eType;

        /// <summary>
        /// The ticket returned from AS response.
        /// </summary>
        private Ticket tgsTicket;

        /// <summary>
        /// The ticket returned by TGS response.
        /// </summary>
        private Ticket apTicket;

        #endregion private members


        #region properties

        /// <summary>
        /// The TGT Key used to decrypt AS response Ticket.
        /// </summary>
        public byte[] AsTgtKey
        {
            get
            {
                lock (contextLock)
                {
                    return (asTgtKey == null) ? null : (byte[])asTgtKey.Clone();
                }
            }
            set
            {
                lock (contextLock)
                {
                    asTgtKey = (value == null) ? null : (byte[])value.Clone();
                }
            }
        }


        /// <summary>
        /// The encryption types supported by client.
        /// </summary>
        [CLSCompliant(false)]
        public Asn1SequenceOf<KerbInt32> ClientEncryptionTypes
        {
            get
            {
                lock (contextLock)
                {
                    return eType;
                }
            }
        }


        /// <summary>
        /// TGS ticket received from AS response
        /// </summary>
        [CLSCompliant(false)]
        public Ticket TgsTicket
        {
            get
            {
                return tgsTicket;
            }
            set
            {
                tgsTicket = value;
            }
        }


        /// <summary>
        /// AP Ticket received from TGS response
        /// </summary>
        [CLSCompliant(false)]
        public Ticket ApTicket
        {
            get
            {
                lock (contextLock)
                {
                    return apTicket;
                }
            }
        }

        #endregion properties


        #region constructor

        /// <summary>
        /// Create a KileClientContext instance.
        /// </summary>
        internal KileClientContext()
        {
            contextLock = new object();
            isInitiator = true;
        }
        #endregion constructor


        /// <summary>
        /// Update the context.
        /// </summary>
        /// <param name="pdu">The PDU to update the context.</param>
        internal override void UpdateContext(KilePdu pdu)
        {
            if (pdu == null)
            {
                return;
            }

            lock (contextLock)
            {
                Type pduType = pdu.GetType();

                if (pduType == typeof(KileAsRequest))
                {
                    KileAsRequest request = (KileAsRequest)pdu;
                    if (request.Request != null && request.Request.req_body != null)
                    {
                        cName = request.Request.req_body.cname;
                        cRealm = request.Request.req_body.realm;
                        eType = request.Request.req_body.etype;
                    }
                }
                else if (pduType == typeof(KileAsResponse))
                {
                    KileAsResponse response = (KileAsResponse)pdu;
                    if (response.EncPart != null)
                    {
                        tgsSessionKey = response.EncPart.key;
                    }

                    if (response.Response != null)
                    {
                        tgsTicket = response.Response.ticket;
                        if (tgsTicket != null && tgsTicket.sname != null
                            && tgsTicket.sname.name_string != null && tgsTicket.sname.name_string.Elements != null
                            && tgsTicket.sname.name_string.Elements.Length > 1)
                        {
                            int count = tgsTicket.sname.name_string.Elements.Length;
                            cRealm = new Realm(tgsTicket.sname.name_string.Elements[count - 1].Value);
                        }

                        if (response.Response.padata != null && response.Response.padata.Elements != null)
                        {
                            foreach (PA_DATA paData in response.Response.padata.Elements)
                            {
                                if (paData.padata_type != null
                                    && paData.padata_type.Value == (long)PaDataType.PA_ETYPE_INFO2)
                                {
                                    var buffer = new Asn1DecodingBuffer(paData.padata_value.ByteArrayValue);
                                    var eTypeInfo2 = new ETYPE_INFO2();
                                    eTypeInfo2.BerDecode(buffer);
                                    if (eTypeInfo2.Elements != null && eTypeInfo2.Elements.Length > 0)
                                    {
                                        // the salt is received from KDC
                                        salt = eTypeInfo2.Elements[0].salt.Value;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (pduType == typeof(KileTgsResponse))
                {
                    KileTgsResponse response = (KileTgsResponse)pdu;
                    if (response.Response != null)
                    {
                        apTicket = response.Response.ticket;
                        if (apTicket != null && apTicket.sname != null
                            && apTicket.sname.name_string != null && apTicket.sname.name_string.Elements != null
                            && apTicket.sname.name_string.Elements.Length > 1)
                        {
                            int count = apTicket.sname.name_string.Elements.Length;
                            cRealm = new Realm(apTicket.sname.name_string.Elements[count - 1].Value);
                        }
                    }

                    if (response.EncPart != null)
                    {
                        apSessionKey = response.EncPart.key;
                    }
                }
                else if (pduType == typeof(KileApRequest))
                {
                    KileApRequest request = (KileApRequest)pdu;
                    if (request.Authenticator != null)
                    {
                        apSubKey = request.Authenticator.subkey;
                        apRequestCtime = request.Authenticator.ctime;
                        apRequestCusec = request.Authenticator.cusec;

                        if (request.Authenticator.cksum != null
                            && request.Authenticator.cksum.cksumtype.Value == (int)ChecksumType.ap_authenticator_8003
                            && request.Authenticator.cksum.checksum != null
                            && request.Authenticator.cksum.checksum.Value != null
                            && request.Authenticator.cksum.checksum.Value.Length == ConstValue.AUTH_CHECKSUM_SIZE)
                        {
                            int flag = BitConverter.ToInt32(request.Authenticator.cksum.checksum.ByteArrayValue,
                                ConstValue.AUTHENTICATOR_CHECKSUM_LENGTH + sizeof(int));
                            checksumFlag = (ChecksumFlags)flag;
                        }

                        if (request.Authenticator.seq_number != null)
                        {
                            currentLocalSequenceNumber = (ulong)request.Authenticator.seq_number.Value;
                            currentRemoteSequenceNumber = currentLocalSequenceNumber;
                        }
                    }
                }
                else if (pduType == typeof(KileApResponse))
                {
                    KileApResponse response = (KileApResponse)pdu;
                    if (response.ApEncPart != null)
                    {
                        if (response.ApEncPart.seq_number != null)
                        {
                            currentRemoteSequenceNumber = (ulong)response.ApEncPart.seq_number.Value;
                        }

                        if (response.ApEncPart.subkey != null)
                        {
                            acceptorSubKey = response.ApEncPart.subkey;
                        }
                    }
                }
                // else do nothing
            }
        }
    }
}
