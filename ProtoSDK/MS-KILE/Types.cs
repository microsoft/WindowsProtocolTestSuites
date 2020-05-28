// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib;
using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Kile
{
    #region PDU definition
    /// <summary>
    /// A type of PDU to indicate the PDU is a KILE PDU. It's a base class for all input/output PDUs.
    /// </summary> 
    public class KilePdu : StackPacket
    {
        /// <summary>
        /// The client context
        /// </summary>
        protected KileClientContext clientContext;

        /// <summary>
        /// The server context
        /// </summary>
        protected KileServerContext serverContext;

        /// <summary>
        /// Either client context or server context
        /// </summary>
        protected KileContext kileContext;

        /// <summary>
        /// This constructor is used to set context.
        /// </summary>
        /// <param name="clientContext">The client context.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the input parameter is null.</exception>
        protected KilePdu(KileClientContext clientContext)
        {
            if (clientContext == null)
            {
                throw new ArgumentNullException(nameof(clientContext));
            }

            this.clientContext = clientContext;
        }


        /// <summary>
        /// This constructor is used to set context.
        /// </summary>
        /// <param name="serverContext">The server context.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the input parameter is null.</exception>
        protected KilePdu(KileServerContext serverContext)
        {
            if (serverContext == null)
            {
                throw new ArgumentNullException(nameof(serverContext));
            }

            this.serverContext = serverContext;
        }


        /// <summary>
        /// constructor
        /// </summary>
        protected KilePdu()
        {
        }


        /// <summary>
        /// This constructor is used to set context.
        /// </summary>
        /// <param name="context">either client or server context .</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the input parameter is null.</exception>
        protected KilePdu(KileContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            kileContext = context;
        }


        /// <summary>
        /// This constructor is used to set field packetBytes in StackPacket.
        /// </summary>
        /// <param name="data">The data to be sent.</param>
        public KilePdu(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Encode this class into byte array.
        /// </summary>
        /// <returns>The byte array of the class.</returns>
        public override byte[] ToBytes()
        {
            return null;
        }


        /// <summary>
        /// Decode the KILE PDU from the message bytes.
        /// </summary>
        /// <param name="buffer">The byte array to be decoded.</param>
        public virtual void FromBytes(byte[] buffer)
        {
        }


        /// <summary>
        /// Create an instance of the class that is identical to the current PDU.
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            return null;
        }
    }

    /// <summary>
    /// AS request PDU.
    /// </summary>
    public class KileAsRequest : KilePdu
    {
        /// <summary>
        /// ASN.1 type of the request.
        /// </summary>
        private AS_REQ asRequest;

        /// <summary>
        /// ASN.1 type of the request.
        /// </summary>
        [CLSCompliant(false)]
        public AS_REQ Request
        {
            get
            {
                return asRequest;
            }
        }


        /// <summary>
        /// Create an instance.
        /// </summary>
        /// <param name="clientContext">The context of the client.</param>
        public KileAsRequest(KileClientContext clientContext)
            : base(clientContext)
        {
            asRequest = new AS_REQ();
        }


        /// <summary>
        /// Create an instance.
        /// </summary>
        /// <param name="clientContext">The context of the server.</param>
        public KileAsRequest(KileServerContext serverContext)
            : base(serverContext)
        {
            asRequest = new AS_REQ();
        }


        /// <summary>
        /// Encode this class into byte array.
        /// </summary>
        /// <returns>The byte array of the class.</returns>
        public override byte[] ToBytes()
        {
            var asBerBuffer = new Asn1BerEncodingBuffer();
            asRequest.BerEncode(asBerBuffer, true);
            if (clientContext.TransportType == KileConnectionType.TCP)
            {
                return KerberosUtility.WrapLength(asBerBuffer.Data, true);
            }
            else
            {
                return asBerBuffer.Data;
            }
        }


        /// <summary>
        /// Decode AS Request from bytes
        /// </summary>
        /// <param name="buffer">the byte array to be decoded</param>
        /// <exception cref="System.ArgumentNullException">thrown when input buffer is null</exception>
        public override void FromBytes(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            // Decode AS Request
            var decodeBuffer = new Asn1DecodingBuffer(buffer);
            asRequest.BerDecode(decodeBuffer);
        }
    }

    /// <summary>
    /// AS response PDU.
    /// </summary>
    public class KileAsResponse : KilePdu
    {
        /// <summary>
        /// ASN.1 type of the response.
        /// </summary>
        private AS_REP asResponse;

        /// <summary>
        /// The decrypted enc_part of KDC_REP.
        /// </summary>
        private EncASRepPart encPart;

        /// <summary>
        /// The decrypted enc_part of Ticket.
        /// </summary>
        private EncTicketPart ticketEncPart;

        /// <summary>
        /// The key used to encrypt enc_part.
        /// </summary>
        private EncryptionKey asReplyKey;

        /// <summary>
        /// ASN.1 type of the response.
        /// </summary>
        [CLSCompliant(false)]
        public AS_REP Response
        {
            get
            {
                return asResponse;
            }
        }

        /// <summary>
        /// The decrypted enc_part of KDC_REP.
        /// </summary>
        [CLSCompliant(false)]
        public EncASRepPart EncPart
        {
            get
            {
                return encPart;
            }
            set
            {
                encPart = value;
            }
        }

        /// <summary>
        /// The decrypted enc_part of Ticket.
        /// </summary>
        [CLSCompliant(false)]
        public EncTicketPart TicketEncPart
        {
            get
            {
                return ticketEncPart;
            }
            set
            {
                ticketEncPart = value;
            }
        }

        /// <summary>
        /// The key used to encrypt enc_part.
        /// </summary>
        [CLSCompliant(false)]
        public EncryptionKey AsReplyKey
        {
            get
            {
                return asReplyKey;
            }
            set
            {
                asReplyKey = value;
            }
        }

        /// <summary>
        /// Create an instance.
        /// </summary>
        /// <param name="clientContext">The context of the client.</param>
        public KileAsResponse(KileClientContext clientContext)
            : base(clientContext)
        {
            asResponse = new AS_REP();
        }


        /// <summary>
        /// Create an instance.
        /// </summary>
        /// <param name="serverContext">The context of the server.</param>
        public KileAsResponse(KileServerContext serverContext)
            : base(serverContext)
        {
            asResponse = new AS_REP();
        }


        /// <summary>
        /// Encode this class into byte array.
        /// </summary>
        /// <returns>The byte array of the class</returns>
        public override byte[] ToBytes()
        {
            var asBerBuffer = new Asn1BerEncodingBuffer();

            // Encode ticket enc_part
            ticketEncPart.BerEncode(asBerBuffer, true);

            // Encrypt ticket enc_part to cipher data
            byte[] ticketCipherData = KerberosUtility.Encrypt(
                   (EncryptionType)serverContext.TicketEncryptKey.keytype.Value,
                   serverContext.TicketEncryptKey.keyvalue.ByteArrayValue,
                   asBerBuffer.Data,
                   (int)KeyUsageNumber.AS_REP_TicketAndTGS_REP_Ticket);
            asResponse.ticket.enc_part = new EncryptedData(new KerbInt32(serverContext.TicketEncryptKey.keytype.Value), null,
                new Asn1OctetString(ticketCipherData));

            // Encode enc_part
            encPart.BerEncode(asBerBuffer, true);

            // Encrypt enc_part to cipher data
            if (asReplyKey == null)
            {
                byte[] sessionKey = KeyGenerator.MakeKey(
                    EncryptionType.RC4_HMAC,
                    serverContext.Password,
                    serverContext.Salt);
                asReplyKey = new EncryptionKey(new KerbInt32((int)EncryptionType.RC4_HMAC), new Asn1OctetString(sessionKey));
            }
            EncryptionType encryptType = (EncryptionType)asReplyKey.keytype.Value;
            int keyUsageNumber;

            if (encryptType == EncryptionType.RC4_HMAC)
            {
                keyUsageNumber = (int)KeyUsageNumber.TGS_REP_encrypted_part;
            }
            else
            {
                keyUsageNumber = (int)KeyUsageNumber.AS_REP_ENCRYPTEDPART;
            }

            byte[] cipherData = KerberosUtility.Encrypt(
                encryptType,
                asReplyKey.keyvalue.ByteArrayValue,
                asBerBuffer.Data,
                keyUsageNumber);
            asResponse.enc_part = new EncryptedData(new KerbInt32((int)encryptType), null, new Asn1OctetString(cipherData));

            // Encode AS Response
            asResponse.BerEncode(asBerBuffer, true);

            if (serverContext.TransportType == KileConnectionType.TCP)
            {
                return KerberosUtility.WrapLength(asBerBuffer.Data, true);
            }
            else
            {
                return asBerBuffer.Data;
            }
        }


        /// <summary>
        /// Decode AS Response from bytes
        /// </summary>
        /// <param name="buffer">the byte array to be decoded</param>
        /// <exception cref="System.ArgumentNullException">thrown when input buffer is null</exception>
        public override void FromBytes(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            // Decode AS Response
            var decodeBuffer = new Asn1DecodingBuffer(buffer);
            asResponse.BerDecode(decodeBuffer);

            // Get the current encryption type, cipher data, session key
            EncryptionType encryptType = (EncryptionType)asResponse.enc_part.etype.Value;

            // If not using PKCA to decrypt the encPart
            if (!(asResponse.padata != null && asResponse.padata.Elements != null
                && asResponse.padata.Elements.Length > 0
                && (asResponse.padata.Elements[0].padata_type.Value == (long)PaDataType.PA_PK_AS_REP
                || asResponse.padata.Elements[0].padata_type.Value == (long)PaDataType.PA_PK_AS_REP_WINDOWS_OLD)))
            {
                byte[] sessionKey = KeyGenerator.MakeKey(encryptType, clientContext.Password, clientContext.Salt);
                Parse(sessionKey);
            }
        }


        /// <summary>
        /// Decode AS Response with AsReplyKey got from PADATA.
        /// This method is used by testing MS-PKCA.
        /// </summary>
        /// <param name="asReplyKey">The key to decrypt As response encrypted part.</param>
        public void Parse(byte[] replyKey)
        {
            // Get key usage number
            int keyUsageNumber;
            EncryptionType encryptType = (EncryptionType)asResponse.enc_part.etype.Value;
            if (encryptType == EncryptionType.RC4_HMAC)
            {
                keyUsageNumber = (int)KeyUsageNumber.TGS_REP_encrypted_part;
            }
            else
            {
                keyUsageNumber = (int)KeyUsageNumber.AS_REP_ENCRYPTEDPART;
            }

            byte[] cipherData = asResponse.enc_part.cipher.ByteArrayValue;// asResponse.enc_part.cipher.Value;
            byte[] clearText = KerberosUtility.Decrypt(encryptType, replyKey, cipherData, keyUsageNumber);

            // Decode enc_part
            var decodeBuffer = new Asn1DecodingBuffer(clearText);
            encPart = new EncASRepPart();
            encPart.BerDecode(decodeBuffer);
            clientContext.UpdateContext(this);
        }
    }


    /// <summary>
    /// TGS request PDU.
    /// </summary>
    public class KileTgsRequest : KilePdu
    {
        /// <summary>
        /// ASN.1 type of the request.
        /// </summary>
        private TGS_REQ tgsRequest;

        /// <summary>
        /// The non-encrypt EncryptedData enc_authorization_data of KDC_REQ_BODY.
        /// </summary>
        private AuthorizationData encAuthorizationData;

        /// <summary>
        /// Authenticator portion of the authentication header.
        /// </summary>
        internal Authenticator authenticator;

        /// <summary>
        /// Accompanying ticket in the authentication header
        /// </summary>
        internal EncTicketPart tgtTicket;

        /// <summary>
        /// ASN.1 type of the request.
        /// </summary>
        [CLSCompliant(false)]
        public TGS_REQ Request
        {
            get
            {
                return tgsRequest;
            }
        }

        /// <summary>
        /// The non-encrypt EncryptedData enc_authorization_data of KDC_REQ_BODY.
        /// </summary>
        [CLSCompliant(false)]
        public AuthorizationData EncAuthorizationData
        {
            get
            {
                return encAuthorizationData;
            }
            set
            {
                encAuthorizationData = value;
            }
        }

        /// <summary>
        /// Create an instance.
        /// </summary>
        /// <param name="clientContext">The context of the client.</param>
        public KileTgsRequest(KileClientContext clientContext)
            : base(clientContext)
        {
            tgsRequest = new TGS_REQ();
        }


        /// <summary>
        /// Create an instance.
        /// </summary>
        /// <param name="serverContext">The context of the server.</param>
        public KileTgsRequest(KileServerContext serverContext)
            : base(serverContext)
        {
            tgsRequest = new TGS_REQ();
        }


        /// <summary>
        /// Encode this class into byte array.
        /// </summary>
        /// <returns>The byte array of the class.</returns>
        public override byte[] ToBytes()
        {
            var tgsBerBuffer = new Asn1BerEncodingBuffer();
            tgsRequest.BerEncode(tgsBerBuffer, true);

            if (clientContext.TransportType == KileConnectionType.TCP)
            {
                return KerberosUtility.WrapLength(tgsBerBuffer.Data, true);
            }
            else
            {
                return tgsBerBuffer.Data;
            }
        }


        /// <summary>
        /// Decode TGS Request from bytes
        /// </summary>
        /// <param name="buffer">the byte array to be decoded</param>
        /// <exception cref="System.ArgumentNullException">thrown when input buffer is null</exception>
        public override void FromBytes(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }
            var decodeBuffer = new Asn1DecodingBuffer(buffer);

            // Decode TGS Request
            tgsRequest.BerDecode(decodeBuffer);
            EncryptionType eType;
            byte[] cipherData;
            byte[] clearText;

            // Decode authentication header
            if (tgsRequest.padata != null && tgsRequest.padata.Elements != null)
            {
                for (int i = 0; i < tgsRequest.padata.Elements.Length; i++)
                {
                    if (tgsRequest.padata.Elements[i].padata_type.Value == (int)PaDataType.PA_TGS_REQ)
                    {
                        AP_REQ authenticationHeader = new AP_REQ();
                        decodeBuffer = new Asn1DecodingBuffer(tgsRequest.padata.Elements[i].padata_value.ByteArrayValue);
                        authenticationHeader.BerDecode(decodeBuffer);

                        // Decrypt Ticket from authentication header
                        if (serverContext.TicketEncryptKey != null)
                        {
                            eType = (EncryptionType)authenticationHeader.ticket.enc_part.etype.Value;
                            cipherData = authenticationHeader.ticket.enc_part.cipher.ByteArrayValue;
                            byte[] sessionKey = serverContext.TicketEncryptKey.keyvalue.ByteArrayValue;
                            clearText = KerberosUtility.Decrypt(
                                eType,
                                sessionKey,
                                cipherData,
                                (int)KeyUsageNumber.AS_REP_TicketAndTGS_REP_Ticket);

                            // Decode Ticket
                            decodeBuffer = new Asn1DecodingBuffer(clearText);
                            tgtTicket = new EncTicketPart();
                            tgtTicket.BerDecode(decodeBuffer);
                        }

                        // Decrypt Authenticator from authentication header
                        eType = (EncryptionType)authenticationHeader.authenticator.etype.Value;
                        cipherData = authenticationHeader.authenticator.cipher.ByteArrayValue;
                        clearText = KerberosUtility.Decrypt(
                            eType,
                            serverContext.TgsSessionKey.keyvalue.ByteArrayValue,
                            cipherData,
                            (int)KeyUsageNumber.TG_REQ_PA_TGS_REQ_padataOR_AP_REQ_Authenticator);

                        // Decode Authenticator
                        decodeBuffer = new Asn1DecodingBuffer(clearText);
                        authenticator = new Authenticator();
                        authenticator.BerDecode(decodeBuffer);
                    }
                }
            }

            // Decrypt EncAuthorizationData
            if (tgsRequest.req_body.enc_authorization_data != null
                && tgsRequest.req_body.enc_authorization_data.cipher != null
                && tgsRequest.req_body.enc_authorization_data.cipher.Value != null)
            {
                eType = (EncryptionType)tgsRequest.req_body.enc_authorization_data.etype.Value;
                cipherData = tgsRequest.req_body.enc_authorization_data.cipher.ByteArrayValue;

                if (authenticator != null && authenticator.subkey != null)
                {
                    clearText = KerberosUtility.Decrypt(
                        eType,
                        authenticator.subkey.keyvalue.ByteArrayValue,
                        cipherData,
                        (int)KeyUsageNumber.TGS_REQ_KDC_REQ_BODY_AuthorizationData);
                }
                else
                {
                    clearText = KerberosUtility.Decrypt(
                        eType,
                        serverContext.TgsSessionKey.keyvalue.ByteArrayValue,
                        cipherData,
                        (int)KeyUsageNumber.TGS_REQ_KDC_REQ_BODY_AuthorizationData);
                }

                // Decode EncAuthorizationData
                decodeBuffer = new Asn1DecodingBuffer(clearText);
                encAuthorizationData = new AuthorizationData();
                encAuthorizationData.BerDecode(decodeBuffer);
            }
        }
    }


    /// <summary>
    /// TGS response PDU.
    /// </summary>
    public class KileTgsResponse : KilePdu
    {
        /// <summary>
        /// ASN.1 type of the response.
        /// </summary>
        private TGS_REP tgsResponse;

        /// <summary>
        /// The decrypted enc_part of KDC_REP.
        /// </summary>
        private EncTGSRepPart encPart;

        /// <summary>
        /// The decrypted enc_part of Ticket.
        /// </summary>
        private EncTicketPart ticketEncPart;

        /// <summary>
        /// ASN.1 type of the response.
        /// </summary>
        [CLSCompliant(false)]
        public TGS_REP Response
        {
            get
            {
                return tgsResponse;
            }
        }

        /// <summary>
        /// The decrypted enc_part of KDC_REP.
        /// </summary>
        [CLSCompliant(false)]
        public EncTGSRepPart EncPart
        {
            get
            {
                return encPart;
            }
            set
            {
                encPart = value;
            }
        }

        /// <summary>
        /// The decrypted enc_part of Ticket.
        /// </summary>
        [CLSCompliant(false)]
        public EncTicketPart TicketEncPart
        {
            get
            {
                return ticketEncPart;
            }
            set
            {
                ticketEncPart = value;
            }
        }


        /// <summary>
        /// Create an instance.
        /// </summary>
        /// <param name="clientContext">The context of the client.</param>
        public KileTgsResponse(KileClientContext clientContext)
            : base(clientContext)
        {
            tgsResponse = new TGS_REP();
        }


        /// <summary>
        /// Create an instance.
        /// </summary>
        /// <param name="clientContext">The context of the client.</param>
        public KileTgsResponse(KileServerContext serverContext)
            : base(serverContext)
        {
            tgsResponse = new TGS_REP();
        }


        /// <summary>
        /// Encode this class into byte array.
        /// </summary>
        /// <returns>The byte array of the class</returns>
        public override byte[] ToBytes()
        {
            var tgsBerBuffer = new Asn1BerEncodingBuffer();

            // Encode ticket enc_part
            ticketEncPart.BerEncode(tgsBerBuffer, true);

            // Encrypt ticket enc_part to cipher data
            byte[] ticketCipherData = KerberosUtility.Encrypt(
                   (EncryptionType)serverContext.TicketEncryptKey.keytype.Value,
                   serverContext.TicketEncryptKey.keyvalue.ByteArrayValue,
                   tgsBerBuffer.Data,
                   (int)KeyUsageNumber.AS_REP_TicketAndTGS_REP_Ticket);
            tgsResponse.ticket.enc_part = new EncryptedData(new KerbInt32(serverContext.TicketEncryptKey.keytype.Value),null,
               new Asn1OctetString(ticketCipherData));

            // Encode enc_part
            encPart.BerEncode(tgsBerBuffer, true);

            // Encrypt enc_part to cipher data
            EncryptionKey eKey;
            byte[] cipherData;

            if (serverContext.TgsSubSessionKey != null)
            {
                eKey = serverContext.TgsSubSessionKey;
                cipherData = KerberosUtility.Encrypt((EncryptionType)eKey.keytype.Value,
                    eKey.keyvalue.ByteArrayValue,
                    tgsBerBuffer.Data,
                    (int)KeyUsageNumber.TGS_REP_encrypted_part_subkey);
            }
            else
            {
                eKey = serverContext.TgsSessionKey;
                cipherData = KerberosUtility.Encrypt((EncryptionType)eKey.keytype.Value,
                    eKey.keyvalue.ByteArrayValue,
                    tgsBerBuffer.Data,
                    (int)KeyUsageNumber.TGS_REP_encrypted_part);
            }
            tgsResponse.enc_part = new EncryptedData(new KerbInt32(eKey.keytype.Value),null, new Asn1OctetString(cipherData));

            // Encode TGS Response
            tgsResponse.BerEncode(tgsBerBuffer, true);

            if (serverContext.TransportType == KileConnectionType.TCP)
            {
                return KerberosUtility.WrapLength(tgsBerBuffer.Data, true);
            }
            else
            {
                return tgsBerBuffer.Data;
            }
        }


        /// <summary>
        /// Decode TGS Response from bytes
        /// </summary>
        /// <param name="buffer">byte array to be decoded</param>
        /// <exception cref="System.ArgumentNullException">thrown when input buffer is null</exception>
        public override void FromBytes(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            // Decode TGS Response
            var decodeBuffer = new Asn1DecodingBuffer(buffer);
            tgsResponse.BerDecode(decodeBuffer);

            // Get the current encryption type, cipher data
            EncryptionType encryptType = (EncryptionType)tgsResponse.enc_part.etype.Value;
            byte[] cipherData = tgsResponse.enc_part.cipher.ByteArrayValue;

            // Decrypt enc_part to clear text
            byte[] clearText = KerberosUtility.Decrypt(
                encryptType,
                clientContext.TgsSessionKey.keyvalue.ByteArrayValue,
                cipherData,
                (int)KeyUsageNumber.TGS_REP_encrypted_part);

            // Decode enc_part
            decodeBuffer = new Asn1DecodingBuffer(clearText);
            encPart = new EncTGSRepPart();
            encPart.BerDecode(decodeBuffer);
        }
    }


    /// <summary>
    /// AP request PDU.
    /// </summary>
    public class KileApRequest : KilePdu
    {
        /// <summary>
        /// ASN.1 type of the request.
        /// </summary>
        private AP_REQ apRequest;

        /// <summary>
        /// The non-encrypt EncryptedData authenticator of AP_REQ.
        /// </summary>
        private Authenticator authenticator;

        /// <summary>
        /// The non-encrypt EncryptedData ticket of AP_REQ.
        /// </summary>
        internal EncTicketPart ticket;

        /// <summary>
        /// ASN.1 type of the request.
        /// </summary>
        [CLSCompliant(false)]
        public AP_REQ Request
        {
            get
            {
                return apRequest;
            }
        }

        /// <summary>
        /// The non-encrypt EncryptedData authenticator of AP_REQ.
        /// </summary>
        [CLSCompliant(false)]
        public Authenticator Authenticator
        {
            get
            {
                return authenticator;
            }
            set
            {
                authenticator = value;
            }
        }


        /// <summary>
        /// Create an instance.
        /// </summary>
        /// <param name="kileContext">The context of the client or server.</param>
        public KileApRequest(KileContext kileContext)
            : base(kileContext)
        {
            apRequest = new AP_REQ();
        }


        /// <summary>
        /// Encode this class into byte array.
        /// </summary>
        /// <returns>The byte array of the class.</returns>
        public override byte[] ToBytes()
        {
            kileContext.UpdateContext(this);
            if (authenticator != null)
            {
                var asnBuffPlainAuthenticator = new Asn1BerEncodingBuffer();
                authenticator.BerEncode(asnBuffPlainAuthenticator, true);

                apRequest.authenticator = new EncryptedData();
                apRequest.authenticator.etype = new KerbInt32(kileContext.ApSessionKey.keytype.Value);
                byte[] encAsnEncodedAuth = asnBuffPlainAuthenticator.Data;
                if (kileContext.ApSessionKey != null && kileContext.ApSessionKey.keytype != null
                    && kileContext.ApSessionKey.keyvalue != null
                    && kileContext.ApSessionKey.keyvalue.Value != null)
                {
                    encAsnEncodedAuth = KerberosUtility.Encrypt((EncryptionType)kileContext.ApSessionKey.keytype.Value,
                                                            kileContext.ApSessionKey.keyvalue.ByteArrayValue,
                                                            asnBuffPlainAuthenticator.Data,
                                                            (int)KeyUsageNumber.AP_REQ_Authenticator);
                    apRequest.authenticator.etype = new KerbInt32(kileContext.ApSessionKey.keytype.Value);
                }

                apRequest.authenticator.cipher = new Asn1OctetString(encAsnEncodedAuth);
            }

            var apBerBuffer = new Asn1BerEncodingBuffer();
            apRequest.BerEncode(apBerBuffer, true);

            if ((kileContext.ChecksumFlag & ChecksumFlags.GSS_C_DCE_STYLE) == ChecksumFlags.GSS_C_DCE_STYLE)
            {
                // In DCE mode, the AP-REQ message MUST NOT have GSS-API wrapping. 
                // It is sent as is without encapsulating it in a header ([RFC2743] section 3.1).
                return apBerBuffer.Data;
            }
            else
            {
                return KerberosUtility.AddGssApiTokenHeader(ArrayUtility.ConcatenateArrays(
                    BitConverter.GetBytes(KerberosUtility.ConvertEndian((ushort)TOK_ID.KRB_AP_REQ)),
                    apBerBuffer.Data));
            }
        }


        /// <summary>
        /// Decode AP Request from bytes
        /// </summary>
        /// <param name="buffer">byte array to be decoded</param>
        /// <exception cref="System.ArgumentNullException">thrown when input buffer is null</exception>
        public override void FromBytes(byte[] buffer)
        {
            FromBytes(buffer, ((KileServerContext)kileContext).TicketEncryptKey);
        }


        /// <summary>
        /// Decode AP Request from bytes
        /// </summary>
        /// <param name="buffer">byte array to be decoded</param>
        /// <param name="ticketEncryptKey">
        /// The key associated with ticket.
        /// Server's secret key.
        /// Or session key from the server's TGT in USE-SESSION-KEY mode.
        /// </param>
        /// <exception cref="System.ArgumentNullException">thrown when input buffer is null</exception>
        [CLSCompliant(false)]
        public void FromBytes(byte[] buffer, EncryptionKey ticketEncryptKey)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            byte[] apBody = buffer;

            // If flag GSS_C_DCE_STYLE is not present, 
            // then token header exists and needs to be removed.
            if (apBody[0] == ConstValue.KERBEROS_TAG)
            {
                byte[] apData = KerberosUtility.VerifyGssApiTokenHeader(buffer);

                // Check if it has a two-byte tok_id
                if (apData == null || apData.Length <= sizeof(TOK_ID))
                {
                    throw new FormatException(
                        "Data length is shorter than a valid AP Response data length.");
                }

                // verify TOK_ID
                byte[] tokenID = ArrayUtility.SubArray<byte>(apData, 0, sizeof(TOK_ID));
                Array.Reverse(tokenID);
                TOK_ID id = (TOK_ID)BitConverter.ToUInt16(tokenID, 0);

                if (id != TOK_ID.KRB_AP_REQ)
                {
                    throw new FormatException("Invalid TOK_ID.");
                }

                // Get apBody
                apBody = ArrayUtility.SubArray(apData, sizeof(TOK_ID));
            }

            // Decode AP Request
            var decodeBuffer = new Asn1DecodingBuffer(apBody);
            apRequest.BerDecode(decodeBuffer);

            if (apRequest.ticket != null && ticketEncryptKey != null)
            {
                // Decrypt Ticket
                EncryptionType eType = (EncryptionType)apRequest.ticket.enc_part.etype.Value;
                byte[] cipherData = apRequest.ticket.enc_part.cipher.ByteArrayValue;
                byte[] clearText = KerberosUtility.Decrypt(eType, ticketEncryptKey.keyvalue.ByteArrayValue, cipherData,
                        (int)KeyUsageNumber.AS_REP_TicketAndTGS_REP_Ticket);

                // Decode Ticket
                decodeBuffer = new Asn1DecodingBuffer(clearText);
                ticket = new EncTicketPart();
                ticket.BerDecode(decodeBuffer);

                kileContext.ApSessionKey = ticket.key;
            }

            if (apRequest.authenticator != null)
            {
                // Decrypt Authenticator
                EncryptionType eType = (EncryptionType)apRequest.authenticator.etype.Value;
                byte[] cipherData = apRequest.authenticator.cipher.ByteArrayValue;
                byte[] clearText = KerberosUtility.Decrypt(eType, kileContext.ApSessionKey.keyvalue.ByteArrayValue, cipherData,
                    (int)KeyUsageNumber.AP_REQ_Authenticator);

                // Decode Authenticator
                decodeBuffer = new Asn1DecodingBuffer(clearText);
                authenticator = new Authenticator();
                authenticator.BerDecode(decodeBuffer);
            }

            kileContext.UpdateContext(this);
        }
    }


    /// <summary>
    /// AP response PDU.
    /// </summary>
    public class KileApResponse : KilePdu
    {
        /// <summary>
        /// ASN.1 type of the response.
        /// </summary>
        private AP_REP apResponse;

        /// <summary>
        /// The non-encrypt EncryptedData enc_part of AP_REP.
        /// </summary>
        private EncAPRepPart apEncPart;

        /// <summary>
        /// ASN.1 type of the response.
        /// </summary>
        [CLSCompliant(false)]
        public AP_REP Response
        {
            get
            {
                return apResponse;
            }
        }


        /// <summary>
        /// The non-encrypt EncryptedData enc_part of AP_REP.
        /// </summary>
        [CLSCompliant(false)]
        public EncAPRepPart ApEncPart
        {
            get
            {
                return apEncPart;
            }
            set
            {
                apEncPart = value;
            }
        }


        /// <summary>
        /// Create an instance.
        /// </summary>
        /// <param name="kileContext">The context of the client or server.</param>
        public KileApResponse(KileContext kileContext)
            : base(kileContext)
        {
            apResponse = new AP_REP();
        }


        /// <summary>
        /// Decode AP Response from bytes
        /// </summary>
        /// <param name="buffer">the byte array to be decoded</param>
        /// <exception cref="System.ArgumentNullException">thrown when input buffer is null</exception>
        /// <exception cref="System.FormatException">thrown when encounters decoding error</exception>
        public override void FromBytes(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            byte[] apBody = buffer;

            // If flag GSS_C_DCE_STYLE is not present, 
            // then token header exists and needs to be removed.
            if (apBody[0] == ConstValue.KERBEROS_TAG)
            {
                byte[] apData = KerberosUtility.VerifyGssApiTokenHeader(buffer);

                // Check if it has a two-byte tok_id
                if (apData == null || apData.Length <= sizeof(TOK_ID))
                {
                    throw new FormatException(
                        "Data length is shorter than a valid AP Response data length.");
                }

                // verify TOK_ID
                byte[] tokenID = ArrayUtility.SubArray<byte>(apData, 0, sizeof(TOK_ID));
                Array.Reverse(tokenID);
                TOK_ID id = (TOK_ID)BitConverter.ToUInt16(tokenID, 0);
                if (id != TOK_ID.KRB_AP_REP)
                {
                    throw new FormatException("Invalid TOK_ID.");
                }

                // Get apBody
                apBody = ArrayUtility.SubArray(apData, sizeof(TOK_ID));
            }

            // Decode AP response
            var decodeBuffer = new Asn1DecodingBuffer(apBody);
            apResponse.BerDecode(decodeBuffer);

            // Get the current encryption type, cipher data
            EncryptionType encryptType = (EncryptionType)apResponse.enc_part.etype.Value;
            byte[] cipherData = apResponse.enc_part.cipher.ByteArrayValue;
            byte[] sessionKey = kileContext.ApSessionKey.keyvalue.ByteArrayValue;

            // Decrypt enc_part to clear text
            byte[] clearText = KerberosUtility.Decrypt(
                encryptType,
                sessionKey,
                cipherData,
                (int)KeyUsageNumber.AP_REP_EncAPRepPart);

            // Decode enc_part
            decodeBuffer = new Asn1DecodingBuffer(clearText);
            apEncPart = new EncAPRepPart();
            apEncPart.BerDecode(decodeBuffer);

            if (kileContext.GetType() == typeof(KileClientContext))
            {
                // Verify enc_part fields
                if (apEncPart.ctime.Value != kileContext.Time.Value)
                {
                    throw new FormatException("Ctime is different from its context value.");
                }
                if (apEncPart.cusec.Value != kileContext.Cusec.Value)
                {
                    throw new FormatException("Cusec is different from its context value.");
                }
            }

            // Update context
            kileContext.UpdateContext(this);
        }


        /// <summary>
        /// Encode this class into byte array.
        /// </summary>
        /// <returns>The byte array of the class.</returns>
        public override byte[] ToBytes()
        {
            // Update server context
            kileContext.UpdateContext(this);

            var apBerBuffer = new Asn1BerEncodingBuffer();

            if (apEncPart != null)
            {
                // Encode enc_part
                apEncPart.BerEncode(apBerBuffer, true);
                byte[] encAsnEncoded = apBerBuffer.Data;
                EncryptionKey key = kileContext.ApSessionKey;

                if (key == null || key.keytype == null || key.keyvalue == null || key.keyvalue.Value == null)
                {
                    throw new ArgumentException("Ap session key is not valid");
                }

                // Encrypt enc_part
                EncryptionType eType = (EncryptionType)key.keytype.Value;
                byte[] cipherData = KerberosUtility.Encrypt(
                    eType,
                    key.keyvalue.ByteArrayValue,
                    apBerBuffer.Data,
                    (int)KeyUsageNumber.AP_REP_EncAPRepPart);
                apResponse.enc_part = new EncryptedData(new KerbInt32((int)eType),null,new Asn1OctetString(cipherData));
            }

            // Encode AP Response
            apResponse.BerEncode(apBerBuffer, true);

            if ((kileContext.ChecksumFlag & ChecksumFlags.GSS_C_DCE_STYLE) == ChecksumFlags.GSS_C_DCE_STYLE)
            {
                // In DCE mode, the AP-REP message MUST NOT have GSS-API wrapping. 
                // It is sent as is without encapsulating it in a header ([RFC2743] section 3.1).
                return apBerBuffer.Data;
            }
            else
            {
                return KerberosUtility.AddGssApiTokenHeader(ArrayUtility.ConcatenateArrays(
                    BitConverter.GetBytes(KerberosUtility.ConvertEndian((ushort)TOK_ID.KRB_AP_REP)),
                    apBerBuffer.Data));
            }
        }
    }


    /// <summary>
    /// KrbZero PDU
    /// ([MS-KILE] Section 3.4.5
    /// KILE will return a zero-length message whenever it receives 
    /// a message that is either not well-formed or not supported.)
    /// </summary>
    public class KrbZero : KilePdu
    {
        /// <summary>
        /// Override Constructor
        /// </summary>
        /// <param name="clientContext">client context</param>
        public KrbZero(KileClientContext clientContext)
            : base(clientContext)
        {
        }
    }


    /// <summary>
    /// KRB_CRED PDU.
    /// </summary>
    public class KrbCred : KilePdu
    {
        /// <summary>
        /// ASN.1 type of the PDU.
        /// </summary>
        private KRB_CRED krbCred;

        /// <summary>
        /// The non-encrypt EncryptedData enc_part of KRB_CRED.
        /// </summary>
        private EncKrbCredPart credEncPart;

        /// <summary>
        /// ASN.1 type of the PDU.
        /// </summary>
        [CLSCompliant(false)]
        public KRB_CRED KerberosCred
        {
            get
            {
                return krbCred;
            }
        }

        /// <summary>
        /// The non-encrypt EncryptedData enc_part of KRB_CRED.
        /// </summary>
        [CLSCompliant(false)]
        public EncKrbCredPart CredEncPart
        {
            get
            {
                return credEncPart;
            }
            set
            {
                credEncPart = value;
            }
        }

        /// <summary>
        /// Create an instance.
        /// </summary>
        /// <param name="clientContext">The context of the client.</param>
        public KrbCred(KileClientContext clientContext)
            : base(clientContext)
        {
            krbCred = new KRB_CRED();
        }


        /// <summary>
        /// Encode this class into byte array.
        /// </summary>
        /// <returns>The byte array of the class.</returns>
        public override byte[] ToBytes()
        {
            if (credEncPart != null)
            {
                var encKrbCredBuf = new Asn1BerEncodingBuffer();
                credEncPart.BerEncode(encKrbCredBuf);

                byte[] encryptedData = encKrbCredBuf.Data;
                if (clientContext.ContextKey != null && clientContext.ContextKey.keytype != null
                    && clientContext.ContextKey.keyvalue != null && clientContext.ContextKey.keyvalue.Value != null)
                {
                    encryptedData = KerberosUtility.Encrypt((EncryptionType)clientContext.ContextKey.keytype.Value,
                                                        clientContext.ContextKey.keyvalue.ByteArrayValue,
                                                        encKrbCredBuf.Data,
                                                        (int)KeyUsageNumber.KRB_CRED_EncPart);
                    krbCred.enc_part = new EncryptedData(new KerbInt32(clientContext.ContextKey.keytype.Value), null, new Asn1OctetString(encryptedData));
                }
                else
                {
                    krbCred.enc_part = new EncryptedData(new KerbInt32(0), null, new Asn1OctetString(encryptedData));
                }
            }

            var krbCredBuf = new Asn1BerEncodingBuffer();
            krbCred.BerEncode(krbCredBuf);
            return KerberosUtility.WrapLength(krbCredBuf.Data, true);
        }
    }

    /// <summary>
    /// KRB_PRIV PDU.
    /// </summary>
    public class KrbPriv : KilePdu
    {
        /// <summary>
        /// ASN.1 type of the PDU.
        /// </summary>
        private KRB_PRIV krbPriv;

        /// <summary>
        /// The non-encrypt EncryptedData enc_part of KRB_PRIV.
        /// </summary>
        private EncKrbPrivPart privEncPart;

        /// <summary>
        /// ASN.1 type of the PDU.
        /// </summary>
        [CLSCompliant(false)]
        public KRB_PRIV KerberosPriv
        {
            get
            {
                return krbPriv;
            }
        }

        /// <summary>
        /// The non-encrypt EncryptedData enc_part of KRB_PRIV.
        /// </summary>
        [CLSCompliant(false)]
        public EncKrbPrivPart PrivEncPart
        {
            get
            {
                return privEncPart;
            }
            set
            {
                privEncPart = value;
            }
        }

        /// <summary>
        /// Create an instance.
        /// </summary>
        /// <param name="clientContext">The context of the client.</param>
        public KrbPriv(KileClientContext clientContext)
            : base(clientContext)
        {
            krbPriv = new KRB_PRIV();
        }


        /// <summary>
        /// Encode this class into byte array.
        /// </summary>
        /// <returns>The byte array of the class.</returns>
        public override byte[] ToBytes()
        {
            #region update sequence number
            clientContext.IncreaseLocalSequenceNumber();
            #endregion update sequence number

            if (privEncPart != null)
            {
                var encKrbPrivBuf = new Asn1BerEncodingBuffer();
                privEncPart.BerEncode(encKrbPrivBuf);

                byte[] encryptedData = encKrbPrivBuf.Data;
                if (clientContext.ContextKey != null && clientContext.ContextKey.keytype != null
                    && clientContext.ContextKey.keyvalue != null && clientContext.ContextKey.keyvalue.Value != null)
                {
                    encryptedData = KerberosUtility.Encrypt((EncryptionType)clientContext.ContextKey.keytype.Value,
                                                        clientContext.ContextKey.keyvalue.ByteArrayValue,
                                                        encKrbPrivBuf.Data,
                                                        (int)KeyUsageNumber.KRB_PRIV_EncPart);
                    krbPriv.enc_part = new EncryptedData(new KerbInt32(clientContext.ContextKey.keytype.Value), null, new Asn1OctetString(encryptedData));
                }
                else
                {
                    krbPriv.enc_part = new EncryptedData(new KerbInt32(0), null, new Asn1OctetString(encryptedData));
                }
            }

            var krbPrivBuf = new Asn1BerEncodingBuffer();
            krbPriv.BerEncode(krbPrivBuf, true);
            return KerberosUtility.WrapLength(krbPrivBuf.Data, true);
        }
    }

    /// <summary>
    /// KRB_ERROR PDU.
    /// </summary>
    public class KileKrbError : KilePdu
    {
        /// <summary>
        /// ASN.1 type of the PDU.
        /// </summary>
        private KRB_ERROR krbError;

        /// <summary>
        /// ASN.1 type of the PDU.
        /// </summary>
        [CLSCompliant(false)]
        public KRB_ERROR KerberosError
        {
            get
            {
                return krbError;
            }
        }


        /// <summary>
        /// Create an instance.
        /// </summary>
        public KileKrbError()
        {
            krbError = new KRB_ERROR();
        }


        /// <summary>
        /// Create an instance.
        /// </summary>
        public KileKrbError(KileContext kileContext)
            : base(kileContext)
        {
            krbError = new KRB_ERROR();
        }


        /// <summary>
        /// Kerberos Error Code
        /// </summary>
        private KRB_ERROR_CODE krbErrorCode;

        /// <summary>
        /// Kerberos Error Code
        /// </summary>
        public KRB_ERROR_CODE ErrorCode
        {
            get { return krbErrorCode; }
        }

        /// <summary>
        /// Decode the Krb Error from bytes
        /// </summary>
        /// <param name="buffer">The byte array to be decoded.</param>
        /// <exception cref="System.ArgumentNullException">thrown when input buffer is null</exception>
        public override void FromBytes(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            // Decode Krb Error
            var decodeBuffer = new Asn1DecodingBuffer(buffer);
            krbError.BerDecode(decodeBuffer);

            // Get its error code
            krbErrorCode = (KRB_ERROR_CODE)krbError.error_code.Value;
        }


        /// <summary>
        /// Encode the Krb Error to bytes
        /// </summary>
        /// <param name="buffer">The byte array to be decoded.</param>
        public override byte[] ToBytes()
        {
            var errorBerBuffer = new Asn1BerEncodingBuffer();
            krbError.BerEncode(errorBerBuffer, true);

            if (kileContext.TransportType == KileConnectionType.TCP)
            {
                return KerberosUtility.WrapLength(errorBerBuffer.Data, true);
            }
            else
            {
                return errorBerBuffer.Data;
            }
        }
    }
    #endregion PDU definition

    /// <summary>
    /// Specify the account type of cName
    /// </summary>
    public enum KileAccountType : byte
    {
        /// <summary>
        /// User account
        /// </summary>
        User = 0,

        /// <summary>
        /// Computer account
        /// </summary>
        Computer = 1,
    }

    #region PaData
    /// <summary>
    /// Pre-authentication Data used in ConstructPaData.
    /// </summary>
    public abstract class PaData
    {
        /// <summary>
        /// The Pre-authentication type.
        /// </summary>
        private PaDataType paType;

        /// <summary>
        /// The Pre-authentication type.
        /// </summary>
        public PaDataType PaType
        {
            get
            {
                return paType;
            }
            set
            {
                paType = value;
            }
        }
    }

    /// <summary>
    /// Pre-authentication type is PA_ENC_TIMESTAMP.
    /// </summary>
    public class PaEncTimeStamp : PaData
    {
        /// <summary>
        /// The client's time.
        /// </summary>
        private string timeStamp;

        /// <summary>
        /// The microseconds of the client's time.
        /// </summary>
        private int usec;

        /// <summary>
        /// The encryption type selected to encrypt the timestamp.
        /// </summary>
        private EncryptionType type;

        /// <summary>
        /// The client's time.
        /// </summary>
        public string TimeStamp
        {
            get
            {
                return timeStamp;
            }
            set
            {
                timeStamp = value;
            }
        }

        /// <summary>
        /// The microseconds of the client's time.
        /// </summary>
        public int Usec
        {
            get
            {
                return usec;
            }
            set
            {
                usec = value;
            }
        }

        /// <summary>
        /// The encryption type selected to encrypt the timestamp.
        /// </summary>
        public EncryptionType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }
    }

    /// <summary>
    /// Pre-authentication type is PA_ETYPE_INFO.
    /// </summary>
    public class PaEtypeInfo : PaData
    {
        /// <summary>
        /// The encryption type supported.
        /// </summary>
        private Collection<EncryptionType> typeList;

        /// <summary>
        /// The salt used to do encryption.
        /// </summary>
        private string salt;

        /// <summary>
        /// The encryption type supported.
        /// </summary>
        public Collection<EncryptionType> TypeList
        {
            get
            {
                return typeList;
            }
            set
            {
                typeList = value;
            }
        }

        /// <summary>
        /// The salt used to do encryption.
        /// </summary>
        public string Salt
        {
            get
            {
                return salt;
            }
            set
            {
                salt = value;
            }
        }
    }

    /// <summary>
    /// Pre-authentication type is PA_ETYPE_INFO2.
    /// </summary>
    public class PaEtypeInfo2 : PaData
    {
        /// <summary>
        /// The encryption type supported.
        /// </summary>
        private Collection<EncryptionType> typeList;

        /// <summary>
        /// The salt used to do encryption.
        /// </summary>
        private string salt;

        /// <summary>
        /// The encryption type supported.
        /// </summary>
        public Collection<EncryptionType> TypeList
        {
            get
            {
                return typeList;
            }
            set
            {
                typeList = value;
            }
        }

        /// <summary>
        /// The salt used to do encryption.
        /// </summary>
        public string Salt
        {
            get
            {
                return salt;
            }
            set
            {
                salt = value;
            }
        }
    }

    /// <summary>
    /// Pre-authentication type is PA_PK_AS_REQ, PA_PK_AS_REP, PA_PK_AS_REQ_OLD or PA_PK_AS_REP_OLD.
    /// </summary>
    public class PaPkcaData : PaData
    {
        /// <summary>
        /// The PKCA data.
        /// </summary>
        private byte[] data;

        /// <summary>
        /// The PKCA data.
        /// </summary>
        public byte[] Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }
    }

    /// <summary>
    /// Pre-authentication type is PA_PAC_REQUEST.
    /// </summary>
    public class PaPacRequest : PaData
    {
        /// <summary>
        /// If include PAC.
        /// </summary>
        private bool includePac;

        /// <summary>
        /// If include PAC.
        /// </summary>
        public bool IncludePac
        {
            get
            {
                return includePac;
            }
            set
            {
                includePac = value;
            }
        }
    }

    /// <summary>
    /// Pre-authentication type is PA_SVR_REFERRAL_INFO.
    /// </summary>
    public class PaSvrReferralInfo : PaData
    {
        /// <summary>
        /// Specify PrincipalType.
        /// </summary>
        private PrincipalType principalType;

        /// <summary>
        /// Specify principal name.
        /// </summary>
        private string principalName;

        /// <summary>
        /// Specify realm.
        /// </summary>
        private string realm;

        /// <summary>
        /// Specify PrincipalType.
        /// </summary>
        public PrincipalType PrincipalType
        {
            get
            {
                return principalType;
            }
            set
            {
                principalType = value;
            }
        }

        /// <summary>
        /// Specify principal name.
        /// </summary>
        public string PrincipalName
        {
            get
            {
                return principalName;
            }
            set
            {
                principalName = value;
            }
        }

        /// <summary>
        /// Specify realm.
        /// </summary>
        public string Realm
        {
            get
            {
                return realm;
            }
            set
            {
                realm = value;
            }
        }
    }
    #endregion PaData

    #region AutherizationData
    /// <summary>
    /// AuthorizationData used in ConstructAuthorizationData.
    /// </summary>
    public abstract class AuthData
    {
        /// <summary>
        /// ad_type of AuthorizationData_element.
        /// </summary>
        private AuthorizationData_elementType adType;

        /// <summary>
        /// ad_type of AuthorizationData_element.
        /// </summary>
        public AuthorizationData_elementType AdType
        {
            get
            {
                return adType;
            }
            set
            {
                adType = value;
            }
        }
    }

    /// <summary>
    /// AuthorizationData_elementType is KERB_AUTH_DATA_TOKEN_RESTRICTIONS.
    /// </summary>
    public class KerbAuthDataTokenRestrictions : AuthData
    {
        /// <summary>
        /// restriction_type of KERB_AD_RESTRICTION_ENTRY.
        /// </summary>
        private int restriction_type;

        /// <summary>
        /// flags of LSAP_TOKEN_INFO_INTEGRITY.
        /// </summary>
        private uint flags;

        /// <summary>
        /// tokenIL of LSAP_TOKEN_INFO_INTEGRITY.
        /// </summary>
        private uint tokenIL;

        /// <summary>
        /// machineID of LSAP_TOKEN_INFO_INTEGRITY.
        /// </summary>
        private string machineId;

        /// <summary>
        /// restriction_type of KERB_AD_RESTRICTION_ENTRY.
        /// </summary>
        public int RestrictionType
        {
            get
            {
                return restriction_type;
            }
            set
            {
                restriction_type = value;
            }
        }

        /// <summary>
        /// flags of LSAP_TOKEN_INFO_INTEGRITY.
        /// </summary>
        [CLSCompliant(false)]
        public uint Flags
        {
            get
            {
                return flags;
            }
            set
            {
                flags = value;
            }
        }

        /// <summary>
        /// tokenIL of LSAP_TOKEN_INFO_INTEGRITY.
        /// </summary>
        [CLSCompliant(false)]
        public uint TokenIL
        {
            get
            {
                return tokenIL;
            }
            set
            {
                tokenIL = value;
            }
        }

        /// <summary>
        /// machineID of LSAP_TOKEN_INFO_INTEGRITY.
        /// </summary>
        public string MachineId
        {
            get
            {
                return machineId;
            }
            set
            {
                machineId = value;
            }
        }
    }

    /// <summary>
    /// AuthorizationData_elementType is AD_AUTH_DATA_AP_OPTIONS.
    /// </summary>
    public class AdAuthDataApOptions : AuthData
    {
        /// <summary>
        /// ad_data of KERB_AP_OPTIONS_CBT.
        /// </summary>
        private uint options;

        /// <summary>
        /// ad_data of KERB_AP_OPTIONS_CBT.
        /// </summary>
        [CLSCompliant(false)]
        public uint Options
        {
            get
            {
                return options;
            }
            set
            {
                options = value;
            }
        }
    }

    /// <summary>
    /// AuthorizationData_elementType is AD_IF_RELEVANT.
    /// </summary>
    public class PacAuthData : AuthData
    {
        /// <summary>
        /// The PAC data.
        /// </summary>
        private byte[] data;

        /// <summary>
        /// The PAC data.
        /// </summary>
        public byte[] Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }
    }
    #endregion AutherizationData

    /// <summary>
    /// The transport type.
    /// </summary>
    public enum KileConnectionType
    {
        TCP,
        UDP,
    }

    /// <summary>
    /// The ip type.
    /// </summary>
    public enum KileIpType
    {
        Ipv4,
        Ipv6,
    }

    #region Token
    /// <summary>
    /// Wrap token in rfc[4121].
    /// </summary>
    public class Token4121 : KilePdu
    {
        /// <summary>
        /// The token header.
        /// </summary>
        private TokenHeader4121 tokenHeader;

        /// <summary>
        /// The orignial data.
        /// </summary>
        private byte[] data;

        /// <summary>
        /// The token header.
        /// </summary>
        public TokenHeader4121 TokenHeader
        {
            get
            {
                return tokenHeader;
            }
            set
            {
                tokenHeader = value;
            }
        }

        /// <summary>
        /// The orignial data.
        /// </summary>
        public byte[] Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }

        /// <summary>
        /// Create an instance.
        /// </summary>
        /// <param name="context">The context of the client.</param>
        public Token4121(KileContext context)
            : base(context)
        {
        }


        /// <summary>
        /// Decode the KILE PDU from the message bytes.
        /// </summary>
        /// <param name="buffer">The byte array to be decoded.</param>
        /// <exception cref="System.FormatException">Thrown when the format of input parameter is not correct.
        /// </exception>
        public override void FromBytes(byte[] buffer)
        {
            if (buffer == null || buffer.Length < Marshal.SizeOf(typeof(TokenHeader4121)))
            {
                throw new FormatException("The token body is incomplete!");
            }

            byte[] tokenBody = buffer;
            if (buffer[0] == KerberosConstValue.KERBEROS_TAG)
            {
                tokenBody = KerberosUtility.VerifyGssApiTokenHeader(buffer);
            }

            tokenHeader = KerberosUtility.ToStruct<TokenHeader4121>(tokenBody);
            ushort ec = KerberosUtility.ConvertEndian(tokenHeader.ec);
            TOK_ID tokId = (TOK_ID)KerberosUtility.ConvertEndian((ushort)tokenHeader.tok_id);

            if (tokId == TOK_ID.Wrap4121)
            {
                int tokenSize = buffer.Length - tokenBody.Length;
                tokenSize += Marshal.SizeOf(typeof(TokenHeader4121)); //clearhdr
                if ((tokenHeader.flags & WrapFlag.Sealed) != 0)
                {
                    tokenSize += Marshal.SizeOf(typeof(TokenHeader4121)); //enchdr
                    tokenSize += Cryptographic.ConstValue.HMAC_HASH_OUTPUT_SIZE; //checksum
                    tokenSize += Cryptographic.ConstValue.AES_BLOCK_SIZE; // confounder
                    tokenSize += ec; //EC bytes of padding
                }
                else
                {
                    tokenSize += ec; //checksum
                }

                if (buffer.Length < tokenSize)
                {
                    throw new FormatException("The token body is incomplete!");
                }

                FromSecurityBuffers(
                    new SecurityBuffer(SecurityBufferType.Token, ArrayUtility.SubArray(buffer, 0, tokenSize)),
                    new SecurityBuffer(SecurityBufferType.Data, ArrayUtility.SubArray(buffer, tokenSize)));
            }
            else if (tokId == TOK_ID.Mic4121)
            {
                FromSecurityBuffers(
                    new SecurityBuffer(SecurityBufferType.Token, buffer),
                    new SecurityBuffer(SecurityBufferType.Data, data));
            }
            else
            {
                throw new FormatException("Other tok_id is not supported.");
            }
        }


        /// <summary>
        /// Decode the KILE PDU from security buffers.
        /// </summary>
        /// <param name="securityBuffers">Security buffers</param>
        public void FromSecurityBuffers(params SecurityBuffer[] securityBuffers)
        {
            byte[] tokenBody = SspiUtility.ConcatenateSecurityBuffers(securityBuffers, SecurityBufferType.Token);
            if (tokenBody[0] == KerberosConstValue.KERBEROS_TAG)
            {
                tokenBody = KerberosUtility.VerifyGssApiTokenHeader(tokenBody);
            }

            if (tokenBody.Length < Marshal.SizeOf(typeof(TokenHeader4121)))
            {
                throw new FormatException("The token body is incomplete!");
            }

            tokenHeader = KerberosUtility.ToStruct<TokenHeader4121>(tokenBody);
            ushort rrc = KerberosUtility.ConvertEndian(tokenHeader.rrc);
            ushort ec = KerberosUtility.ConvertEndian(tokenHeader.ec);
            TOK_ID tokId = (TOK_ID)KerberosUtility.ConvertEndian((ushort)tokenHeader.tok_id);

            byte[] cipher;
            if (tokId == TOK_ID.Wrap4121)
            {
                cipher = ArrayUtility.ConcatenateArrays(
                    ArrayUtility.SubArray(tokenBody, Marshal.SizeOf(typeof(TokenHeader4121))),
                    SspiUtility.ConcatenateReadWriteSecurityBuffers(securityBuffers, SecurityBufferType.Data, SecurityBufferType.Padding));
            }
            else
            {
                cipher = ArrayUtility.SubArray(tokenBody, Marshal.SizeOf(typeof(TokenHeader4121)));
            }

            #region set keyusage
            TokenKeyUsage keyUsage;
            if (tokId == TOK_ID.Wrap4121)  // wrap token
            {
                if ((tokenHeader.flags & WrapFlag.SentByAcceptor) == WrapFlag.SentByAcceptor)
                {
                    keyUsage = TokenKeyUsage.KG_USAGE_ACCEPTOR_SEAL;
                }
                else
                {
                    keyUsage = TokenKeyUsage.KG_USAGE_INITIATOR_SEAL;
                }

                if ((tokenHeader.flags & WrapFlag.Sealed) == WrapFlag.None)
                {
                    tokenHeader.ec = 0;
                }

                tokenHeader.rrc = 0;
            }
            else  // mic token
            {
                if ((tokenHeader.flags & WrapFlag.SentByAcceptor) == WrapFlag.SentByAcceptor)
                {
                    keyUsage = TokenKeyUsage.KG_USAGE_ACCEPTOR_SIGN;
                }
                else
                {
                    keyUsage = TokenKeyUsage.KG_USAGE_INITIATOR_SIGN;
                }
            }
            #endregion set keyusage

            byte[] header = KerberosUtility.StructToBytes(tokenHeader);

            #region convert big-endian
            tokenHeader.tok_id = (TOK_ID)KerberosUtility.ConvertEndian((ushort)tokenHeader.tok_id);
            tokenHeader.snd_seq = KerberosUtility.ConvertEndian(tokenHeader.snd_seq);
            tokenHeader.ec = ec;
            tokenHeader.rrc = rrc;
            #endregion convert big-endian

            #region verify header
            if (tokenHeader.tok_id != TOK_ID.Wrap4121 && tokenHeader.tok_id != TOK_ID.Mic4121)
            {
                throw new FormatException("The token ID is incorrect! tok_id = " + (ushort)tokenHeader.tok_id);
            }

            if (tokenHeader.filler != KerberosConstValue.TOKEN_FILLER_1_BYTE)
            {
                throw new FormatException("The token filler is incorrect! filler = " + tokenHeader.filler);
            }

            if (tokenHeader.snd_seq != kileContext.CurrentRemoteSequenceNumber)
            {
                throw new FormatException("The token sequence number is incorrect! snd_seq = " + tokenHeader.snd_seq);
            }

            if (tokId == TOK_ID.Mic4121)
            {
                if (tokenHeader.ec != KerberosConstValue.TOKEN_FILLER_2_BYTE)
                {
                    throw new FormatException("The token ec is incorrect! ec = " + (ushort)tokenHeader.ec);
                }

                if (tokenHeader.rrc != KerberosConstValue.TOKEN_FILLER_2_BYTE)
                {
                    throw new FormatException("The token rrc is incorrect! rrc = " + (ushort)tokenHeader.rrc);
                }
            }
            #endregion verify header

            #region set context key
            EncryptionKey key;
            if ((tokenHeader.flags & WrapFlag.AcceptorSubkey) == WrapFlag.AcceptorSubkey)
            {
                key = kileContext.AcceptorSubKey;
                if (key == null)
                {
                    throw new FormatException("Acceptor SubKey does not exist!");
                }
            }
            else
            {
                if (kileContext.ApSubKey != null)
                {
                    key = kileContext.ApSubKey;
                }
                else
                {
                    key = kileContext.ApSessionKey;
                }
            }
            #endregion

            if (tokId == TOK_ID.Wrap4121)
            {
                //The RRC field ([RFC4121] section 4.2.5) is 12 if no encryption is requested or 28 if encryption is requested. 
                //The RRC field is chosen such that all the data can be encrypted in place. 
                //The trailing meta-data H1 is rotated by RRC+EC bytes, 
                //which is different from RRC alone ([RFC4121] section 4.2.5).
                if ((tokenHeader.flags & WrapFlag.Sealed) != 0)
                {
                    rrc += ec;
                }
                KerberosUtility.RotateRight(cipher, cipher.Length - rrc);
            }

            if (tokId == TOK_ID.Wrap4121 && (tokenHeader.flags & WrapFlag.Sealed) == WrapFlag.Sealed)
            {
                GetToBeSignedDataFunc getToBeSignedDataCallback = delegate (byte[] decryptedData)
                {
                    //Coding according to MS-KILE section 4.3 GSS_WrapEx with AES128-CTS-HMAC-SHA1-96
                    //And Figure 4: Example of RRC with output message with 4 buffers

                    //cipher block size, c
                    //This is the block size of the block cipher underlying the
                    //encryption and decryption functions indicated above, used for key
                    //derivation and for the size of the message confounder and initial
                    //vector.  (If a block cipher is not in use, some comparable
                    //parameter should be determined.)  It must be at least 5 octets.
                    int cipherBlockSize;
                    EncryptionType eType = (EncryptionType)key.keytype.Value;
                    if (eType != EncryptionType.AES256_CTS_HMAC_SHA1_96 && eType != EncryptionType.AES128_CTS_HMAC_SHA1_96)
                    {
                        //etype other than AES is not supported.
                        return decryptedData;
                    }
                    cipherBlockSize = Cryptographic.ConstValue.AES_BLOCK_SIZE;
                    cipherBlockSize = Math.Max(cipherBlockSize, 5);
                    int dataLength = SspiUtility.ConcatenateReadWriteSecurityBuffers(securityBuffers, SecurityBufferType.Data, SecurityBufferType.Padding).Length;
                    int hdrSize = Marshal.SizeOf(typeof(TokenHeader4121));

                    byte[] confounder = ArrayUtility.SubArray(decryptedData, 0, cipherBlockSize);
                    byte[] plainText = ArrayUtility.SubArray(decryptedData, cipherBlockSize, dataLength);
                    byte[] filler = ArrayUtility.SubArray(decryptedData, confounder.Length + dataLength, ec);
                    byte[] enchdr = ArrayUtility.SubArray(decryptedData, decryptedData.Length - hdrSize);
                    SspiUtility.UpdateSecurityBuffers(
                        securityBuffers,
                        new SecurityBufferType[] { SecurityBufferType.Data, SecurityBufferType.Padding },
                        plainText);

                    byte[] toBeSignedData = KileUtility.GetToBeSignedDataFromSecurityBuffers(securityBuffers);
                    toBeSignedData = ArrayUtility.ConcatenateArrays(
                        confounder,
                        toBeSignedData,
                        filler,
                        enchdr);
                    return toBeSignedData;
                };

                // wrap & confidentiality is provided
                // {"header" | encrypt(plaintext-data | filler | "header")}
                byte[] plainBuffer = KerberosUtility.Decrypt((EncryptionType)key.keytype.Value,
                                                       key.keyvalue.ByteArrayValue,
                                                       cipher,
                                                       (int)keyUsage,
                                                       getToBeSignedDataCallback);
                if (plainBuffer.Length < Marshal.SizeOf(typeof(TokenHeader4121)) + ec)
                {
                    throw new FormatException("The encrypted data is incomplete!");
                }

                byte[] headerBuffer = ArrayUtility.SubArray(plainBuffer,
                                                            plainBuffer.Length - Marshal.SizeOf(typeof(TokenHeader4121)));
                if (!ArrayUtility.CompareArrays(header, headerBuffer))
                {
                    throw new FormatException("The encrypted data is incorrect!");
                }

                data = ArrayUtility.SubArray(plainBuffer,
                                             0,
                                             plainBuffer.Length - Marshal.SizeOf(typeof(TokenHeader4121)) - ec);
            }
            else   // no confidentiality is provided
            {
                byte[] check = cipher;
                if (tokId == TOK_ID.Wrap4121)
                {
                    // {"header" | plaintext-data | get_mic(plaintext-data | "header")}
                    data = ArrayUtility.SubArray(cipher, 0, cipher.Length - ec);
                    check = ArrayUtility.SubArray(cipher, cipher.Length - ec);
                }
                else
                {
                    data = KileUtility.GetToBeSignedDataFromSecurityBuffers(securityBuffers);
                }

                ChecksumType checksumType = ChecksumType.hmac_sha1_96_aes256;
                if ((EncryptionType)key.keytype.Value == EncryptionType.AES128_CTS_HMAC_SHA1_96)
                {
                    checksumType = ChecksumType.hmac_sha1_96_aes128;
                }

                byte[] checksum = KerberosUtility.GetChecksum(
                    key.keyvalue.ByteArrayValue,
                    ArrayUtility.ConcatenateArrays(data, header),
                    (int)keyUsage,
                    checksumType);

                if (!ArrayUtility.CompareArrays(check, checksum))
                {
                    throw new FormatException("The checksum is incorrect!");
                }
            }

            #region update sequence number
            kileContext.IncreaseRemoteSequenceNumber();
            #endregion
        }


        /// <summary>
        /// Encode this class into byte array.
        /// </summary>
        /// <returns>The byte array of the class.</returns>
        /// <exception cref="System.NotSupportedException">Thrown when the type of tok_id is not supported.</exception>
        public override byte[] ToBytes()
        {
            if (data == null)
            {
                return null;
            }

            if (tokenHeader.tok_id != TOK_ID.Wrap4121 && tokenHeader.tok_id != TOK_ID.Mic4121)
            {
                throw new NotSupportedException("tok_id = " + (ushort)tokenHeader.tok_id);
            }

            #region convert big-endian
            TokenHeader4121 header4121 = tokenHeader;
            header4121.tok_id = (TOK_ID)KerberosUtility.ConvertEndian((ushort)tokenHeader.tok_id);
            header4121.snd_seq = KerberosUtility.ConvertEndian(tokenHeader.snd_seq);
            header4121.ec = KerberosUtility.ConvertEndian(tokenHeader.ec);
            #endregion convert big-endian

            #region set keyusage
            TokenKeyUsage keyUsage;
            if (tokenHeader.tok_id == TOK_ID.Wrap4121)  // wrap token
            {
                if ((tokenHeader.flags & WrapFlag.SentByAcceptor) == WrapFlag.SentByAcceptor)
                {
                    keyUsage = TokenKeyUsage.KG_USAGE_ACCEPTOR_SEAL;
                }
                else
                {
                    keyUsage = TokenKeyUsage.KG_USAGE_INITIATOR_SEAL;
                }

                if ((tokenHeader.flags & WrapFlag.Sealed) == WrapFlag.None)
                {
                    header4121.ec = 0;
                }

                header4121.rrc = 0;
            }
            else  // mic token
            {
                if ((tokenHeader.flags & WrapFlag.SentByAcceptor) == WrapFlag.SentByAcceptor)
                {
                    keyUsage = TokenKeyUsage.KG_USAGE_ACCEPTOR_SIGN;
                }
                else
                {
                    keyUsage = TokenKeyUsage.KG_USAGE_INITIATOR_SIGN;
                }
            }
            #endregion set keyusage

            #region set context key
            EncryptionKey key;
            if ((tokenHeader.flags & WrapFlag.AcceptorSubkey) == WrapFlag.AcceptorSubkey)
            {
                key = kileContext.AcceptorSubKey;
                if (key == null)
                {
                    throw new FormatException("Acceptor SubKey does not exist!");
                }
            }
            else
            {
                if (kileContext.ApSubKey != null)
                {
                    key = kileContext.ApSubKey;
                }
                else
                {
                    key = kileContext.ApSessionKey;
                }
            }
            #endregion set context key

            // plainBuf = plaintext-data | "header"
            byte[] headerBuf = KerberosUtility.StructToBytes(header4121);
            byte[] plainBuf = ArrayUtility.ConcatenateArrays(data, headerBuf);

            byte[] cipher = null;
            if (tokenHeader.tok_id == TOK_ID.Wrap4121 && (tokenHeader.flags & WrapFlag.Sealed) == WrapFlag.Sealed)
            {
                // wrap & confidentiality is provided
                // {"header" | encrypt(plaintext-data | filler | "header")}
                if (tokenHeader.ec != 0)
                {
                    byte[] filler = new byte[tokenHeader.ec];
                    plainBuf = ArrayUtility.ConcatenateArrays(data, filler, headerBuf);
                }

                cipher = KerberosUtility.Encrypt((EncryptionType)key.keytype.Value,
                                             key.keyvalue.ByteArrayValue,
                                             plainBuf,
                                             (int)keyUsage);
            }
            else   // no confidentiality is provided or mic token
            {
                // {"header" | plaintext-data | get_mic(plaintext-data | "header")}
                ChecksumType checksumType = ChecksumType.hmac_sha1_96_aes256;
                if ((EncryptionType)key.keytype.Value == EncryptionType.AES128_CTS_HMAC_SHA1_96)
                {
                    checksumType = ChecksumType.hmac_sha1_96_aes128;
                }

                cipher = KerberosUtility.GetChecksum(key.keyvalue.ByteArrayValue, plainBuf, (int)keyUsage, checksumType);
                if (tokenHeader.tok_id == TOK_ID.Wrap4121)
                {
                    header4121.ec = KerberosUtility.ConvertEndian((ushort)cipher.Length);
                    cipher = ArrayUtility.ConcatenateArrays(data, cipher);
                }
            }

            #region set rrc
            if (tokenHeader.tok_id == TOK_ID.Wrap4121)
            {
                KerberosUtility.RotateRight(cipher, tokenHeader.rrc);
                header4121.rrc = KerberosUtility.ConvertEndian(tokenHeader.rrc);
                headerBuf = KerberosUtility.StructToBytes(header4121);
            }
            #endregion set rrc

            #region update sequence number
            kileContext.IncreaseLocalSequenceNumber();
            #endregion

            return ArrayUtility.ConcatenateArrays(headerBuf, cipher);
        }
    }

    /// <summary>
    /// Wrap token in rfc[1964].
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security.Cryptography", "CA5350:MD5CannotBeUsed")]
    public class Token1964_4757 : KilePdu
    {
        /// <summary>
        /// The token header.
        /// </summary>
        private TokenHeader1964_4757 tokenHeader;

        /// <summary>
        /// The orignial data.
        /// </summary>
        private byte[] data;

        /// <summary>
        /// Padding data
        /// </summary>
        internal byte[] paddingData;

        /// <summary>
        /// The token header.
        /// </summary>
        public TokenHeader1964_4757 TokenHeader
        {
            get
            {
                return tokenHeader;
            }
            set
            {
                tokenHeader = value;
            }
        }

        /// <summary>
        /// The orignial data.
        /// </summary>
        public byte[] Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }

        /// <summary>
        /// Create an instance.
        /// </summary>
        /// <param name="kileContext">The context of the client or server.</param>
        public Token1964_4757(KileContext context)
            : base(context)
        {
        }


        /// <summary>
        /// Decode the KILE PDU from the message bytes.
        /// </summary>
        /// <param name="buffer">The byte array to be decoded.</param>
        /// <exception cref="System.FormatException">thrown when the format of input buffer is not correct</exception>
        public override void FromBytes(byte[] buffer)
        {
            byte[] tokenBody = KerberosUtility.VerifyGssApiTokenHeader(buffer);
            if (tokenBody == null || tokenBody.Length < KerberosConstValue.HEADER_FIRST_8_BYTE_SIZE)
            {
                throw new FormatException("The token body is incomplete!");
            }

            TOK_ID tokId = (TOK_ID)KerberosUtility.ConvertEndian(BitConverter.ToUInt16(tokenBody, 0));

            if (tokId == TOK_ID.Wrap1964_4757)
            {
                int tokenSize = buffer.Length - tokenBody.Length;
                tokenSize += KerberosConstValue.HEADER_FIRST_8_BYTE_SIZE + KerberosConstValue.CHECKSUM_SIZE_RFC1964 + KerberosConstValue.SEQUENCE_NUMBER_SIZE;
                if (tokId == TOK_ID.Wrap1964_4757)
                {
                    tokenSize += KerberosConstValue.CONFOUNDER_SIZE;
                }

                if (buffer.Length < tokenSize)
                {
                    throw new FormatException("The token body is incomplete!");
                }

                FromSecurityBuffers(
                    new SecurityBuffer(SecurityBufferType.Token, ArrayUtility.SubArray(buffer, 0, tokenSize)),
                    new SecurityBuffer(SecurityBufferType.Data, ArrayUtility.SubArray(buffer, tokenSize)));
            }
            else if (tokId == TOK_ID.Mic1964_4757)
            {
                FromSecurityBuffers(
                    new SecurityBuffer(SecurityBufferType.Token, buffer),
                    new SecurityBuffer(SecurityBufferType.Data, data));
            }
            else
            {
                throw new FormatException("Other tok_id is not supported.");
            }
        }


        /// <summary>
        /// Decode the KILE PDU from the security buffers.
        /// </summary>
        /// <param name="securityBuffers">Security buffers</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security.Cryptography", "CA5350:MD5CannotBeUsed")]
        public void FromSecurityBuffers(params SecurityBuffer[] securityBuffers)
        {
            byte[] tokenBody = SspiUtility.ConcatenateSecurityBuffers(securityBuffers, SecurityBufferType.Token);
            tokenBody = KerberosUtility.VerifyGssApiTokenHeader(tokenBody);
            if (tokenBody == null
                || tokenBody.Length <
                (KerberosConstValue.HEADER_FIRST_8_BYTE_SIZE + KerberosConstValue.CHECKSUM_SIZE_RFC1964 + KerberosConstValue.SEQUENCE_NUMBER_SIZE))
            {
                throw new FormatException("The token body is incomplete!");
            }

            byte[] header = ArrayUtility.SubArray(tokenBody, 0, KerberosConstValue.HEADER_FIRST_8_BYTE_SIZE);
            byte[] sequence = ArrayUtility.SubArray(tokenBody,
                                                    KerberosConstValue.HEADER_FIRST_8_BYTE_SIZE,
                                                    KerberosConstValue.SEQUENCE_NUMBER_SIZE);
            byte[] checksum = ArrayUtility.SubArray(tokenBody,
                KerberosConstValue.HEADER_FIRST_8_BYTE_SIZE + KerberosConstValue.SEQUENCE_NUMBER_SIZE,
                KerberosConstValue.CHECKSUM_SIZE_RFC1964);
            byte[] confounder = new byte[0];

            // Get fields from byte array. The numbers below are the offset of these fields.
            tokenHeader = new TokenHeader1964_4757();
            tokenHeader.tok_id = (TOK_ID)KerberosUtility.ConvertEndian(BitConverter.ToUInt16(header, 0));
            tokenHeader.sng_alg = (SGN_ALG)KerberosUtility.ConvertEndian(BitConverter.ToUInt16(header, 2));
            tokenHeader.seal_alg = (SEAL_ALG)KerberosUtility.ConvertEndian(BitConverter.ToUInt16(header, 4));
            tokenHeader.filler = KerberosUtility.ConvertEndian(BitConverter.ToUInt16(header, 6));

            #region verify header
            if (tokenHeader.tok_id != TOK_ID.Wrap1964_4757 && tokenHeader.tok_id != TOK_ID.Mic1964_4757)
            {
                throw new FormatException("The token ID is incorrect! tok_id = " + (ushort)tokenHeader.tok_id);
            }

            if (tokenHeader.sng_alg != SGN_ALG.DES_MAC && tokenHeader.sng_alg != SGN_ALG.DES_MAC_MD5
                && tokenHeader.sng_alg != SGN_ALG.MD2_5 && tokenHeader.sng_alg != SGN_ALG.HMAC)
            {
                throw new FormatException("The token sng_alg is incorrect! sng_alg = " + (ushort)tokenHeader.sng_alg);
            }

            if (tokenHeader.seal_alg != SEAL_ALG.DES && tokenHeader.seal_alg != SEAL_ALG.RC4
                && tokenHeader.seal_alg != SEAL_ALG.NONE)
            {
                throw new FormatException("The token seal_alg is incorrect! seal_alg = "
                    + (ushort)tokenHeader.seal_alg);
            }

            if (tokenHeader.filler != KerberosConstValue.TOKEN_FILLER_2_BYTE)
            {
                throw new FormatException("The token filler is incorrect! filler = " + tokenHeader.filler);
            }
            #endregion verify header

            byte[] cipher = null;
            if (tokenHeader.tok_id == TOK_ID.Wrap1964_4757)   // wrap token
            {
                // the tokenBody must >= header + sequnce number + checksum + confounder
                if (tokenBody.Length !=
                (KerberosConstValue.HEADER_FIRST_8_BYTE_SIZE + KerberosConstValue.CHECKSUM_SIZE_RFC1964 + KerberosConstValue.SEQUENCE_NUMBER_SIZE
                 + KerberosConstValue.CONFOUNDER_SIZE))
                {
                    throw new FormatException("The token body is incomplete!");
                }
                confounder = ArrayUtility.SubArray(
                    tokenBody,
                    KerberosConstValue.HEADER_FIRST_8_BYTE_SIZE + KerberosConstValue.CHECKSUM_SIZE_RFC1964 + KerberosConstValue.SEQUENCE_NUMBER_SIZE);
                cipher = ArrayUtility.ConcatenateArrays(
                    confounder,
                    SspiUtility.ConcatenateReadWriteSecurityBuffers(securityBuffers, SecurityBufferType.Data, SecurityBufferType.Padding));
            }
            else  // mic token, the data is given by user
            {
                if (tokenBody.Length !=
                (KerberosConstValue.HEADER_FIRST_8_BYTE_SIZE + KerberosConstValue.CHECKSUM_SIZE_RFC1964 + KerberosConstValue.SEQUENCE_NUMBER_SIZE))
                {
                    throw new FormatException("The token body is incomplete!");
                }
            }

            #region decrypt data
            EncryptionKey key = kileContext.ContextKey;
            bool isExport = (EncryptionType)key.keytype.Value == EncryptionType.RC4_HMAC_EXP;
            byte[] plainText = data;
            if (tokenHeader.seal_alg == SEAL_ALG.DES)
            {
                // The key used is derived from the established context key by XOR-ing the context key 
                // with the hexadecimal constant f0f0f0f0f0f0f0f0.
                byte[] Klocal = new byte[8];
                for (int i = 0; i < 8; i++)
                {
                    Klocal[i] = (byte)(key.keyvalue.Value[i] ^ 0xF0);
                }
                // The data is encrypted using DES-CBC, with an IV of zero.
                plainText = KerberosUtility.DesCbcDecrypt(Klocal, new byte[8], cipher);
            }
            else if (tokenHeader.seal_alg == SEAL_ALG.RC4)
            {
                // for (i = 0; i < 16; i++) Klocal[i] = Kss[i] ^ 0xF0;
                byte[] Klocal = new byte[16];
                for (int i = 0; i < 16; i++)
                {
                    Klocal[i] = (byte)(key.keyvalue.Value[i] ^ 0xF0);
                }
                byte[] seqBuffer = BitConverter.GetBytes((uint)kileContext.CurrentRemoteSequenceNumber);
                Array.Reverse(seqBuffer);
                plainText = KerberosUtility.RC4HMAC(Klocal, seqBuffer, cipher, isExport);
            }
            else
            {
                if (tokenHeader.tok_id == TOK_ID.Wrap1964_4757)
                {
                    plainText = cipher;
                }
            }

            if (tokenHeader.tok_id == TOK_ID.Wrap1964_4757)
            {
                // The plaintext data is padded to the next highest multiple of 8 bytes, 
                // by appending between 1 and 8 bytes, the value of each such byte being
                // the total number of pad bytes.  For example, given data of length 20
                // bytes, four pad bytes will be appended, and each byte will contain
                // the hex value 04.  
                // no padding is possible.
                byte pad = plainText[plainText.Length - 1];
                if (pad > 8 || pad < 1)
                {
                    pad = 0;
                }
                for (int i = 1; i < pad; ++i)
                {
                    if (plainText[plainText.Length - i - 1] != pad)
                    {
                        pad = 0;
                    }
                }
                paddingData = new byte[pad];

                for (int i = 0; i < paddingData.Length; i++)
                {
                    paddingData[i] = pad;
                }

                confounder = ArrayUtility.SubArray(plainText, 0, KerberosConstValue.CONFOUNDER_SIZE);

                plainText = ArrayUtility.SubArray(plainText,
                                             KerberosConstValue.CONFOUNDER_SIZE,
                                             plainText.Length - KerberosConstValue.CONFOUNDER_SIZE);
                SspiUtility.UpdateSecurityBuffers(
                    securityBuffers,
                    new SecurityBufferType[] { SecurityBufferType.Data, SecurityBufferType.Padding },
                    plainText);

                data = ArrayUtility.SubArray(plainText, 0, plainText.Length - pad);
            }
            #endregion decrypt data

            #region verify checksum
            byte[] check = null;
            byte[] toChecksumText = KileUtility.GetToBeSignedDataFromSecurityBuffers(securityBuffers);
            toChecksumText = ArrayUtility.ConcatenateArrays(header, confounder, toChecksumText);
            if ((EncryptionType)key.keytype.Value == EncryptionType.RC4_HMAC
                || (EncryptionType)key.keytype.Value == EncryptionType.RC4_HMAC_EXP)
            {
                TokenKeyUsage usage = TokenKeyUsage.USAGE_WRAP;
                if (tokenHeader.tok_id == TOK_ID.Mic1964_4757)
                {
                    usage = TokenKeyUsage.USAGE_MIC;
                }
                toChecksumText = ArrayUtility.ConcatenateArrays(BitConverter.GetBytes((int)usage), toChecksumText);
            }

            var md5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] md5 = md5CryptoServiceProvider.ComputeHash(toChecksumText);

            switch (tokenHeader.sng_alg)
            {
                case SGN_ALG.DES_MAC_MD5:
                    check = KerberosUtility.DesCbcMac(key.keyvalue.ByteArrayValue, new byte[KerberosConstValue.DES_BLOCK_SIZE], md5);
                    break;

                case SGN_ALG.MD2_5:
                    check = KerberosUtility.MD2_5(key.keyvalue.ByteArrayValue, toChecksumText);
                    break;

                case SGN_ALG.DES_MAC:
                    throw new NotSupportedException("DES_MAC is not supported currently.");

                case SGN_ALG.HMAC:
                    byte[] keySign = KerberosUtility.HMAC(key.keyvalue.ByteArrayValue, KerberosConstValue.SIGNATURE_KEY);
                    check = KerberosUtility.HMAC(keySign, md5);
                    break;

                default:
                    break;
            }
            tokenHeader.sng_cksum = ArrayUtility.SubArray(check, 0, KerberosConstValue.CHECKSUM_SIZE_RFC1964);
            if (!ArrayUtility.CompareArrays(tokenHeader.sng_cksum, checksum))
            {
                throw new FormatException("The checksum is incorrect!");
            }
            #endregion verify checksum

            #region verify sequence number
            byte[] seqBuf = null;
            switch ((EncryptionType)key.keytype.Value)
            {
                case EncryptionType.DES_CBC_CRC:
                case EncryptionType.DES_CBC_MD5:
                    uint seqNumber = KerberosUtility.ConvertEndian((uint)kileContext.CurrentRemoteSequenceNumber);
                    seqBuf = SetSequenceBuffer(seqNumber, !kileContext.isInitiator);
                    tokenHeader.snd_seq = KerberosUtility.DesCbcDecrypt(key.keyvalue.ByteArrayValue,
                                                                    tokenHeader.sng_cksum,
                                                                    sequence);
                    break;

                case EncryptionType.RC4_HMAC:
                case EncryptionType.RC4_HMAC_EXP:
                    seqBuf = SetSequenceBuffer((uint)kileContext.CurrentRemoteSequenceNumber, !kileContext.isInitiator);
                    tokenHeader.snd_seq = KerberosUtility.RC4HMAC(key.keyvalue.ByteArrayValue,
                                                              tokenHeader.sng_cksum,
                                                              sequence,
                                                              isExport);
                    break;

                default:
                    throw new NotSupportedException("RFC 1964 and 4757 only support encryption algorithm " +
                        "DES_CBC_CRC, DES_CBC_MD5, RC4_HMAC and RC4_HMAC_EXP!");
            }

            if (!ArrayUtility.CompareArrays(tokenHeader.snd_seq, seqBuf))
            {
                throw new FormatException("The sequence number is incorrect!");
            }
            #endregion verify sequence number

            #region update sequence number
            kileContext.IncreaseRemoteSequenceNumber();
            #endregion update sequence number
        }


        /// <summary>
        /// Encode this class into byte array.
        /// </summary>
        /// <returns>The byte array of the class.</returns>
        /// <exception cref="System.NotSupportedException">thrown when any type of tok_id, sng_alg or seal_alg is
        /// not supported.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security.Cryptography", "CA5350:MD5CannotBeUsed")]
        public override byte[] ToBytes()
        {
            if (data == null)
            {
                return null;
            }

            TokenHeader1964_4757 header1964_4757 = tokenHeader;
            TOK_ID tokId = tokenHeader.tok_id;
            if (tokId != TOK_ID.Wrap1964_4757 && tokId != TOK_ID.Mic1964_4757)
            {
                throw new NotSupportedException("tok_id = " + (ushort)tokenHeader.tok_id);
            }

            SGN_ALG sngAlg = tokenHeader.sng_alg;
            if (sngAlg != SGN_ALG.DES_MAC && sngAlg != SGN_ALG.DES_MAC_MD5
                && sngAlg != SGN_ALG.MD2_5 && sngAlg != SGN_ALG.HMAC)
            {
                throw new NotSupportedException("sng_alg = " + tokenHeader.sng_alg);
            }

            SEAL_ALG sealAlg = tokenHeader.seal_alg;
            if (sealAlg != SEAL_ALG.DES && sealAlg != SEAL_ALG.RC4 && sealAlg != SEAL_ALG.NONE)
            {
                throw new NotSupportedException("seal_alg = " + tokenHeader.seal_alg);
            }

            #region convert big-endian
            header1964_4757.tok_id = (TOK_ID)KerberosUtility.ConvertEndian((ushort)tokenHeader.tok_id);
            header1964_4757.sng_alg = (SGN_ALG)KerberosUtility.ConvertEndian((ushort)tokenHeader.sng_alg);
            header1964_4757.seal_alg = (SEAL_ALG)KerberosUtility.ConvertEndian((ushort)tokenHeader.seal_alg);
            byte[] headercheck = ArrayUtility.SubArray(KerberosUtility.StructToBytes(header1964_4757),
                                                       0,
                                                       KerberosConstValue.HEADER_FIRST_8_BYTE_SIZE);
            #endregion convert big-endian

            #region create plainText
            EncryptionKey key = kileContext.ContextKey;
            byte[] plainText = data;
            byte paddingLength = 0;

            if (tokId == TOK_ID.Wrap1964_4757)  // wrap token
            {
                #region compute pad
                byte[] pad = null;
                switch ((EncryptionType)key.keytype.Value)
                {
                    case EncryptionType.DES_CBC_CRC:
                    case EncryptionType.DES_CBC_MD5:
                        // The plaintext data is padded to the next highest multiple of 8 bytes, 
                        // by appending between 1 and 8 bytes, the value of each such byte being
                        // the total number of pad bytes.  For example, given data of length 20
                        // bytes, four pad bytes will be appended, and each byte will contain
                        // the hex value 04.  
                        int padLength = 8 - (data.Length % 8);
                        pad = new byte[padLength];
                        for (int i = 0; i < padLength; ++i)
                        {
                            pad[i] = (byte)padLength;
                        }
                        break;

                    case EncryptionType.RC4_HMAC:
                    case EncryptionType.RC4_HMAC_EXP:
                        // All padding is rounded up to 1 byte.
                        pad = new byte[] { 1 };
                        break;

                    default:
                        throw new NotSupportedException("RFC 1964 and 4757 only support encryption algorithm " +
                        "DES_CBC_CRC, DES_CBC_MD5, RC4_HMAC and RC4_HMAC_EXP!");
                }
                paddingLength = (byte)pad.Length;
                #endregion compute pad

                byte[] confounder = KerberosUtility.GenerateRandomBytes(KerberosConstValue.CONFOUNDER_SIZE);
                plainText = ArrayUtility.ConcatenateArrays(confounder, data, pad);
            }
            #endregion create plainText

            #region compute checksum
            byte[] check = null;
            byte[] toChecksumText = ArrayUtility.ConcatenateArrays(headercheck,
                                                                  plainText);
            if ((EncryptionType)key.keytype.Value == EncryptionType.RC4_HMAC
                || (EncryptionType)key.keytype.Value == EncryptionType.RC4_HMAC_EXP)
            {
                TokenKeyUsage usage = TokenKeyUsage.USAGE_WRAP;
                if (tokId == TOK_ID.Mic1964_4757)
                {
                    usage = TokenKeyUsage.USAGE_MIC;
                }
                toChecksumText = ArrayUtility.ConcatenateArrays(BitConverter.GetBytes((int)usage), toChecksumText);
            }

            var md5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] md5 = md5CryptoServiceProvider.ComputeHash(toChecksumText);

            switch (sngAlg)
            {
                case SGN_ALG.DES_MAC_MD5:
                    check = KerberosUtility.DesCbcMac(key.keyvalue.ByteArrayValue, new byte[KerberosConstValue.DES_BLOCK_SIZE], md5);
                    break;

                case SGN_ALG.MD2_5:
                    check = KerberosUtility.MD2_5(key.keyvalue.ByteArrayValue, toChecksumText);
                    break;

                case SGN_ALG.DES_MAC:
                    throw new NotSupportedException("DES_MAC is not supported currently.");

                case SGN_ALG.HMAC:
                    byte[] keySign = KerberosUtility.HMAC(key.keyvalue.ByteArrayValue, KerberosConstValue.SIGNATURE_KEY);
                    check = KerberosUtility.HMAC(keySign, md5);
                    break;

                default:
                    break;
            }
            tokenHeader.sng_cksum = ArrayUtility.SubArray(check, 0, KerberosConstValue.CHECKSUM_SIZE_RFC1964);
            #endregion compute checksum

            #region compute sequence number
            byte[] seqBuf = null;
            bool isExport = (EncryptionType)key.keytype.Value == EncryptionType.RC4_HMAC_EXP;
            switch ((EncryptionType)key.keytype.Value)
            {
                case EncryptionType.DES_CBC_CRC:
                case EncryptionType.DES_CBC_MD5:
                    uint seqNumber = KerberosUtility.ConvertEndian((uint)kileContext.CurrentLocalSequenceNumber);
                    seqBuf = SetSequenceBuffer(seqNumber, kileContext.isInitiator);
                    tokenHeader.snd_seq = KerberosUtility.DesCbcEncrypt(key.keyvalue.ByteArrayValue,
                                                                    tokenHeader.sng_cksum,
                                                                    seqBuf);
                    break;

                case EncryptionType.RC4_HMAC:
                case EncryptionType.RC4_HMAC_EXP:
                    seqBuf = SetSequenceBuffer((uint)kileContext.CurrentLocalSequenceNumber, kileContext.isInitiator);
                    tokenHeader.snd_seq = KerberosUtility.RC4HMAC(key.keyvalue.ByteArrayValue,
                                                              tokenHeader.sng_cksum,
                                                              seqBuf,
                                                              isExport);
                    break;

                default:
                    tokenHeader.snd_seq = seqBuf;
                    break;
            }
            #endregion compute sequence number

            #region compute encrypted data
            byte[] encData = null;
            if (sealAlg == SEAL_ALG.DES)
            {
                // The key used is derived from the established context key by XOR-ing the context key 
                // with the hexadecimal constant f0f0f0f0f0f0f0f0.
                byte[] Klocal = new byte[8];
                for (int i = 0; i < 8; i++)
                {
                    Klocal[i] = (byte)(key.keyvalue.Value[i] ^ 0xF0);
                }
                // The data is encrypted using DES-CBC, with an IV of zero.
                encData = KerberosUtility.DesCbcEncrypt(Klocal, new byte[8], plainText);
            }
            else if (sealAlg == SEAL_ALG.RC4)
            {
                // for (i = 0; i < 16; i++) Klocal[i] = Kss[i] ^ 0xF0;
                byte[] Klocal = new byte[16];
                for (int i = 0; i < 16; i++)
                {
                    Klocal[i] = (byte)(key.keyvalue.Value[i] ^ 0xF0);
                }
                byte[] seqBuffer = BitConverter.GetBytes((uint)kileContext.CurrentLocalSequenceNumber);
                Array.Reverse(seqBuffer);
                encData = KerberosUtility.RC4HMAC(Klocal, seqBuffer, plainText, isExport);
            }
            else
            {
                if (tokId == TOK_ID.Wrap1964_4757)
                {
                    encData = plainText;
                }
            }
            #endregion compute encrypted data

            byte[] allData = ArrayUtility.ConcatenateArrays(headercheck,
                                                            tokenHeader.snd_seq,
                                                            tokenHeader.sng_cksum,
                                                            encData);

            #region update sequence number
            kileContext.IncreaseLocalSequenceNumber();
            #endregion

            paddingData = ArrayUtility.SubArray(allData, allData.Length - paddingLength);

            return KerberosUtility.AddGssApiTokenHeader(allData);
        }


        /// <summary>
        /// Set sequence buffer with the given sequence number.
        /// </summary>
        /// <param name="sequenceNumber">The specified sequence number.</param>
        /// <param name="isInitiator">If the sender is the initiator.</param>
        /// <returns>The sequence buffer.</returns>
        private byte[] SetSequenceBuffer(uint sequenceNumber, bool isInitiator)
        {
            /* From [RFC 4757]
             * if (direction == sender_is_initiator)
            {
                memset(&Token.SEND_SEQ[4], 0xff, 4)
            }
            else if (direction == sender_is_acceptor)
            {
                memset(&Token.SEND_SEQ[4], 0, 4)
            }
            Token.SEND_SEQ[0] = (seq_num & 0xff000000) >> 24;
            Token.SEND_SEQ[1] = (seq_num & 0x00ff0000) >> 16;
            Token.SEND_SEQ[2] = (seq_num & 0x0000ff00) >> 8;
            Token.SEND_SEQ[3] = (seq_num & 0x000000ff);
             * */

            byte[] seqBuffer = new byte[KerberosConstValue.SEQUENCE_NUMBER_SIZE];
            if (!isInitiator)  // sender_is_acceptor
            {
                seqBuffer[4] = KerberosConstValue.TOKEN_FILLER_1_BYTE;
                seqBuffer[5] = KerberosConstValue.TOKEN_FILLER_1_BYTE;
                seqBuffer[6] = KerberosConstValue.TOKEN_FILLER_1_BYTE;
                seqBuffer[7] = KerberosConstValue.TOKEN_FILLER_1_BYTE;
            }
            seqBuffer[0] = (byte)((sequenceNumber & 0xff000000) >> 24);
            seqBuffer[1] = (byte)((sequenceNumber & 0x00ff0000) >> 16);
            seqBuffer[2] = (byte)((sequenceNumber & 0x0000ff00) >> 8);
            seqBuffer[3] = (byte)(sequenceNumber & 0x000000ff);
            return seqBuffer;
        }
    }

    /// <summary>
    /// Token header of rfc[4121].
    /// </summary>
    public struct TokenHeader4121
    {
        /// <summary>
        /// Identification field.
        /// </summary>
        [CLSCompliant(false)]
        public TOK_ID tok_id;

        /// <summary>
        /// Attributes field.
        /// </summary>
        public WrapFlag flags;

        /// <summary>
        /// Contains the hex value FF.
        /// </summary>
        public byte filler;

        /// <summary>
        /// Contains the "extra count" field.
        /// </summary>
        [CLSCompliant(false)]
        public ushort ec;

        /// <summary>
        /// Contains the "right rotation count".
        /// </summary>
        [CLSCompliant(false)]
        public ushort rrc;

        /// <summary>
        /// Sequence number field in clear text.
        /// </summary>
        [CLSCompliant(false)]
        public UInt64 snd_seq;
    }

    /// <summary>
    /// The Attribute of flag.
    /// </summary>
    [Flags]
    public enum WrapFlag : byte
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicate the sender is the context acceptor.
        /// </summary>
        SentByAcceptor = 0x01,

        /// <summary>
        /// Indicate confidentiality is provided for. It SHALL NOT be set in MIC tokens.
        /// </summary>
        Sealed = 0x02,

        /// <summary>
        /// A subkey asserted by the context acceptor is used to protect the message.
        /// </summary>
        AcceptorSubkey = 0x04,
    }

    /// <summary>
    /// Token header of rfc[1964] and rfc[4757].
    /// </summary>
    public struct TokenHeader1964_4757
    {
        /// <summary>
        /// Identification field.
        /// </summary>
        [CLSCompliant(false)]
        public TOK_ID tok_id;

        /// <summary>
        /// Checksum algorithm indicator.
        ///  00 00 - DES MAC MD5
        ///  01 00 - MD2.5
        ///  02 00 - DES MAC
        /// </summary>
        [CLSCompliant(false)]
        public SGN_ALG sng_alg;

        /// <summary>
        /// ff ff - none
        /// 00 00 - DES
        /// </summary>
        [CLSCompliant(false)]
        public SEAL_ALG seal_alg;

        /// <summary>
        /// Contains ff ff.
        /// </summary>
        [CLSCompliant(false)]
        public ushort filler;

        /// <summary>
        /// Encrypted sequence number field. 8 bytes.
        /// </summary>
        public byte[] snd_seq;

        /// <summary>
        /// Checksum of plaintext padded data, calculated according to algorithm specified in SGN_ALG field.8 bytes.
        /// </summary>
        public byte[] sng_cksum;
    }
    #endregion Token
}
