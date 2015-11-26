// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    public class KpasswordResponse : KerberosPdu
    {
        /// <summary>
        /// Contains the number of bytes in the message including this field
        /// </summary>
        public ushort msg_length
        {
            get;
            set;
        }

        /// <summary>
        /// Protocol version number: contains the hex constant 0x0001
        /// </summary>
        public ushort version
        {
            get;
            set;
        }

        /// <summary>
        /// Length of AP-REP data, in bytes. 
        /// </summary>
        public ushort ap_rep_length
        {
            get;
            set;
        }

        /// <summary>
        /// KerberosApResponse
        /// </summary>
        public KerberosApResponse ap_rep
        {
            get;
            set;
        }

        /// <summary>
        /// The KRB_PRIV message contains encrypted user data.
        /// </summary>
        public KRB_PRIV krb_priv
        {
            get;
            set;
        }

        /// <summary>
        /// The decrypted enc_part of krb_priv 
        /// </summary>
        public EncKrbPrivPart priv_enc_part
        {
            get;
            set;
        }

        /// <summary>
        /// Create an instance.
        /// </summary>
        public KpasswordResponse()
            : base()
        {
            ap_rep = new KerberosApResponse();
            krb_priv = new KRB_PRIV();
            priv_enc_part = new EncKrbPrivPart();
        }

        /// <summary>
        /// Decode Kpassword Response from bytes
        /// </summary>
        /// <param name="buffer">the byte array to be decoded</param>
        /// <exception cref="System.ArgumentNullException">thrown when input buffer is null</exception>
        public override void FromBytes(byte[] buffer)
        {
            if (null == buffer)
            {
                throw new ArgumentNullException("buffer");
            }

            //Calculate the length of the message and the length of AP response data
            byte[] msgLengthBytes = ArrayUtility.SubArray<byte>(buffer, 0, sizeof(ushort));
            Array.Reverse(msgLengthBytes);
            ushort msgLength = BitConverter.ToUInt16(msgLengthBytes, 0);

            byte[] apLengthBytes = ArrayUtility.SubArray<byte>(buffer, 2 * sizeof(ushort), sizeof(ushort));
            Array.Reverse(apLengthBytes);
            ushort apLength = BitConverter.ToUInt16(apLengthBytes, 0);

            //Decode the ap response and the krb-priv message
            byte[] apBytes = ArrayUtility.SubArray<byte>(buffer, 3 * sizeof(ushort), apLength);
            Asn1DecodingBuffer apBuffer = new Asn1DecodingBuffer(apBytes);
            this.ap_rep.Response.BerDecode(apBuffer);

            byte[] privBytes = ArrayUtility.SubArray<byte>(buffer, 3 * sizeof(ushort) + apLength);
            Asn1DecodingBuffer privBuffer = new Asn1DecodingBuffer(privBytes);
            this.krb_priv.BerDecode(privBuffer);
        }

        /// <summary>
        /// Decrypt the KRB-PRIV
        /// </summary>
        /// <param name="subkey">the subkey used to decrypt</param>
        public void DecryptKrbPriv(EncryptionKey subkey)
        {
            byte[] priv = KerberosUtility.Decrypt(
                      (EncryptionType)subkey.keytype.Value,
                      subkey.keyvalue.ByteArrayValue,
                      krb_priv.enc_part.cipher.ByteArrayValue,
                      (int)KeyUsageNumber.KRB_PRIV_EncPart);

            Asn1DecodingBuffer decodeBuffer = new Asn1DecodingBuffer(priv);
            priv_enc_part.BerDecode(decodeBuffer);
        }
    }
}
