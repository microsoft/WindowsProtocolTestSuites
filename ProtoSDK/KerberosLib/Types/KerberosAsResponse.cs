// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    public class KerberosAsResponse : KerberosPdu
    {
        /// <summary>
        /// ASN.1 type of the response.
        /// </summary>
        public AS_REP Response
        {
            get;
            private set;
        }

        /// <summary>
        /// The decrypted enc_part of KDC_REP.
        /// </summary>
        public EncKDCRepPart EncPart
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
        /// The key used to encrypt enc_part.
        /// </summary>
        public EncryptionKey AsReplyKey
        {
            get;
            set;
        }

        /// <summary>
        /// Create an instance.
        /// </summary>
        /// <param name="clientContext">The context of the client.</param>
        public KerberosAsResponse()
        {
            this.Response = new AS_REP();
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
        /// Decode AS Response from bytes
        /// </summary>
        /// <param name="buffer">the byte array to be decoded</param>
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
            // Decode AS Response
            Asn1DecodingBuffer decodeBuffer = new Asn1DecodingBuffer(buffer);
            this.Response.BerDecode(decodeBuffer);
            // Get the current encryption type, cipher data, session key
            EncryptionType encryptType = (EncryptionType)this.Response.enc_part.etype.Value;
        }

        /// <summary>
        /// Decode AS Response with AsReplyKey got from PADATA.
        /// </summary>
        public void Decrypt(string password, string salt)
        {
            var encryptType = (EncryptionType)Response.enc_part.etype.Value;
            var key = KeyGenerator.MakeKey(encryptType, password, salt);
            DecryptAsResponse(key);
        }

        public void Decrypt(byte[] key)
        {
            DecryptAsResponse(key);
        }

        private void DecryptTicket()
        {

        }

        private void DecryptAsResponse(byte[] key)
        {
            var encryptType = (EncryptionType)Response.enc_part.etype.Value;
            int keyUsage  = (int)KeyUsageNumber.AS_REP_ENCRYPTEDPART;
            if (encryptType == EncryptionType.RC4_HMAC)
                keyUsage = (int)KeyUsageNumber.TGS_REP_encrypted_part;

            var encPartRawData = KerberosUtility.Decrypt(
                encryptType,
                key,
                Response.enc_part.cipher.ByteArrayValue,
                keyUsage);
            Asn1DecodingBuffer buf = new Asn1DecodingBuffer(encPartRawData);
            Asn1Tag tag = null;
            Asn1StandardProcedure.TagBerDecode(buf, out tag);
            //Some implementations unconditionally send an encrypted EncTGSRepPart in the field 
            //regardless of whether the reply is an AS-REP or a TGS-REP.([RFC4120] Section 5.4.2)
            if (tag.TagValue == 25)  //EncAsRepPart
            {
                EncPart = new EncASRepPart();
            }
            else if (tag.TagValue == 26) //EncTgsRepPart
            {
                EncPart = new EncTGSRepPart();
            }
            else
            {
                throw new Exception("Unknown tag number");
            }
            EncPart.BerDecode(new Asn1DecodingBuffer(encPartRawData));
            KerberosUtility.OnDumpMessage("KRB5:AS-REP(enc-part)",
                "Encrypted part of AS-REP",
                KerberosUtility.DumpLevel.PartialMessage,
                encPartRawData);
        }
    }
}
