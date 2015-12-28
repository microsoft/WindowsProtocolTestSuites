// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    public class KerberosApResponse : KerberosPdu
    {
        /// <summary>
        /// ASN.1 type of the response.
        /// </summary>
        public AP_REP Response
        {
            get;
            set;
        }


        /// <summary>
        /// The non-encrypt EncryptedData enc_part of AP_REP.
        /// </summary>
        public EncAPRepPart ApEncPart
        {
            get;
            set;
        }


        /// <summary>
        /// Create an instance.
        /// </summary>
        /// <param name="kileContext">The context of the client or server.</param>
        public KerberosApResponse()
        {
            Response = new AP_REP();
        }


        /// <summary>
        /// Decode AP Response from bytes
        /// </summary>
        /// <param name="buffer">the byte array to be decoded</param>
        /// <exception cref="System.ArgumentNullException">thrown when input buffer is null</exception>
        /// <exception cref="System.FormatException">thrown when encounters decoding error</exception>
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
            this.Response = new AP_REP();
            Asn1DecodingBuffer decodeBuffer = new Asn1DecodingBuffer(buffer);
            this.Response.BerDecode(decodeBuffer);

        }


        /// <summary>
        /// Encode this class into byte array.
        /// </summary>
        /// <returns>The byte array of the class.</returns>
        public override byte[] ToBytes()
        {
            throw new NotImplementedException();
        }

        public void Decrypt(byte[] key)
        {
            var encryptType = (EncryptionType)Response.enc_part.etype.Value;
            var decoded = KerberosUtility.Decrypt(
                encryptType,
                key,
                Response.enc_part.cipher.ByteArrayValue,
                (int)KeyUsageNumber.AP_REP_EncAPRepPart);
            KerberosUtility.OnDumpMessage("KRB5:PA-ENC-TS-ENC",
                "Encrypted Timestamp Pre-authentication",
                KerberosUtility.DumpLevel.PartialMessage,
                decoded);
            ApEncPart = new EncAPRepPart();
            ApEncPart.BerDecode(new Asn1DecodingBuffer(decoded));
            KerberosUtility.OnDumpMessage("KRB5:AP-REP(enc-part)",
                "Encrypted part of AS-REP",
                KerberosUtility.DumpLevel.PartialMessage,
                decoded);
        }

    }
}
