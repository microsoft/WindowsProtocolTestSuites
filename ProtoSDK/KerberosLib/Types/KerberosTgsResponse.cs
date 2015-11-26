// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    public class KerberosTgsResponse : KerberosPdu
    {
        ///<summary>
        /// ASN.1 type of the response.
        /// </summary>
        public TGS_REP Response
        {
            get;
            private set;
        }

        /// <summary>
        /// The decrypted enc_part of KDC_REP.
        /// </summary>
        public EncTGSRepPart EncPart
        {
            get;
            private set;
        }

        /// <summary>
        /// The decrypted enc_part of Ticket.
        /// </summary>
        public EncTicketPart TicketEncPart
        {
            get;
            private set;
        }

        /// <summary>
        /// Create an instance.
        /// </summary>
        public KerberosTgsResponse()
        {
            Response = new TGS_REP();
        }

        /// <summary>
        /// Encode this class into byte array.
        /// </summary>
        /// <returns>The byte array of the class</returns>
        public override byte[] ToBytes()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Decode TGS Response from bytes
        /// </summary>
        /// <param name="buffer">byte array to be decoded</param>
        /// <exception cref="System.ArgumentNullException">thrown when input buffer is null</exception>
        public override void FromBytes(byte[] buffer)
        {
            if (null == buffer)
            {
                throw new ArgumentNullException("buffer");
            }
            KerberosUtility.OnDumpMessage("KRB5:KrbMessage",
                "Kerberos Message",
                KerberosUtility.DumpLevel.WholeMessage,
                buffer);
            // Decode TGS Response
            Asn1DecodingBuffer decodeBuffer = new Asn1DecodingBuffer(buffer);
            Response.BerDecode(decodeBuffer);
        }

        public void Decrypt(byte[] key, string serverPassword, string serverSalt)
        {
            DecryptTgsResponse(key);
            DecryptTicket(serverPassword, serverSalt);
        }

        public void DecryptTicket(string serverPassword, string serverSalt)
        {
            var encryptType = (EncryptionType)Response.ticket.enc_part.etype.Value;
            var key = KeyGenerator.MakeKey(encryptType, serverPassword, serverSalt);
            DecryptTicket(encryptType, key);
        }

        public void DecryptTicket(EncryptionKey key)
        {
            DecryptTicket((EncryptionType)key.keytype.Value, key.keyvalue.ByteArrayValue);
        }

        private void DecryptTicket(EncryptionType type, byte[] sessionKey)
        {
            var ticketEncPartRawData = KerberosUtility.Decrypt(
                type,
                sessionKey,
                Response.ticket.enc_part.cipher.ByteArrayValue,
                (int)KeyUsageNumber.AS_REP_TicketAndTGS_REP_Ticket);
            TicketEncPart = new EncTicketPart();
            TicketEncPart.BerDecode(new Asn1DecodingBuffer(ticketEncPartRawData));
            KerberosUtility.OnDumpMessage("KRB5:TicketEncPart",
                "Encrypted Ticket in TGS-REP",
                KerberosUtility.DumpLevel.PartialMessage,
                ticketEncPartRawData);
        }

        public void DecryptTgsResponse(byte[] key, KeyUsageNumber usage = KeyUsageNumber.TGS_REP_encrypted_part)
        {
            var encryptType = (EncryptionType)Response.enc_part.etype.Value;
            var encPartRawData = KerberosUtility.Decrypt(
                encryptType,
                key,
                Response.enc_part.cipher.ByteArrayValue,
                (int)usage);
            EncPart = new EncTGSRepPart();
            EncPart.BerDecode(new Asn1DecodingBuffer(encPartRawData));
            KerberosUtility.OnDumpMessage("KRB5:TGS-REP(enc-part)",
                "Encrypted part of TGS-REP",
                KerberosUtility.DumpLevel.PartialMessage,
                encPartRawData);
        }
    }
}
