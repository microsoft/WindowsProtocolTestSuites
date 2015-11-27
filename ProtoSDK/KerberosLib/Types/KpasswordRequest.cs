// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    public class KpasswordRequest : KerberosPdu
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
        /// Length of AP-REQ data, in bytes.
        /// </summary>
        public ushort ap_req_length
        {
            get;
            set;
        }

        /// <summary>
        /// KerberosApRequest
        /// </summary>
        public KerberosApRequest ap_req
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

        public Asn1BerEncodingBuffer privBuffer = new Asn1BerEncodingBuffer();
        public Asn1BerEncodingBuffer apBuffer = new Asn1BerEncodingBuffer();

        public KpasswordRequest()
            : base()
        {
            ap_req = new KerberosApRequest();
            krb_priv = new KRB_PRIV();
        }

        /// <summary>
        /// Create an instance.
        /// </summary>
        public KpasswordRequest(KerberosTicket ticket, Authenticator authenticator, string newPwd, bool isAuthErrorRequired = false)
        {
            //Create KerberosApRequest
            long pvno = KerberosConstValue.KERBEROSV5;
            APOptions option = new APOptions(KerberosUtility.ConvertInt2Flags((int)ApOptions.None));
            KerberosApRequest ap_req = new KerberosApRequest(pvno, option, ticket, authenticator, KeyUsageNumber.AP_REQ_Authenticator);
            //Create KRB_PRIV
            ChangePasswdData pwd_data = new ChangePasswdData(new Asn1OctetString(newPwd), null, null);
            priv_enc_part = new EncKrbPrivPart();
            priv_enc_part.user_data = pwd_data.newpasswd;
            priv_enc_part.usec = authenticator.cusec;
            priv_enc_part.seq_number = authenticator.seq_number;
            priv_enc_part.s_address = new HostAddress(new KerbInt32((int)AddressType.NetBios), new Asn1OctetString(Encoding.ASCII.GetBytes(System.Net.Dns.GetHostName())));
            Asn1BerEncodingBuffer asnBuffPriv = new Asn1BerEncodingBuffer();
            priv_enc_part.BerEncode(asnBuffPriv, true);
            byte[] encAsnEncodedPriv = null;

            if (!isAuthErrorRequired)
            {
                encAsnEncodedPriv = KerberosUtility.Encrypt((EncryptionType)authenticator.subkey.keytype.Value,
                            authenticator.subkey.keyvalue.ByteArrayValue,
                            asnBuffPriv.Data,
                            (int)KeyUsageNumber.KRB_PRIV_EncPart);
            }
            else
            {
                encAsnEncodedPriv = KerberosUtility.Encrypt((EncryptionType)authenticator.subkey.keytype.Value,
                            authenticator.subkey.keyvalue.ByteArrayValue,
                            asnBuffPriv.Data,
                            (int)KeyUsageNumber.None);
            }

            var encrypted = new EncryptedData();
            encrypted.etype = new KerbInt32(authenticator.subkey.keytype.Value);
            encrypted.cipher = new Asn1OctetString(encAsnEncodedPriv);
            KRB_PRIV krb_priv = new KRB_PRIV(new Asn1Integer(pvno), new Asn1Integer((long)MsgType.KRB_PRIV), encrypted);
            //Calculate the msg_length and ap_req_length
            krb_priv.BerEncode(privBuffer, true);
            ap_req.Request.BerEncode(apBuffer, true);
            version = 0x0001;
            ap_req_length = (ushort)apBuffer.Data.Length;
            msg_length = (ushort)(ap_req_length + privBuffer.Data.Length + 3 * sizeof(ushort));
            //Convert Endian
            version = KerberosUtility.ConvertEndian(version);
            ap_req_length = KerberosUtility.ConvertEndian(ap_req_length);
            msg_length = KerberosUtility.ConvertEndian(msg_length);
        }

        /// <summary>
        /// Encode this class into byte array.
        /// </summary>
        /// <returns>The byte array of the class.</returns>
        public override byte[] ToBytes()
        {
            byte[] ret = ArrayUtility.ConcatenateArrays<byte>(BitConverter.GetBytes(msg_length), BitConverter.GetBytes(version),
                BitConverter.GetBytes(ap_req_length), apBuffer.Data, privBuffer.Data);
            return KerberosUtility.WrapLength(ret, true);
        }


        /// <summary>
        /// Decode KpasswordRequest Request from bytes
        /// </summary>
        /// <param name="buffer">byte array to be decoded</param>
        /// <exception cref="System.ArgumentNullException">thrown when input buffer is null</exception>
        public override void FromBytes(byte[] buffer)
        {
            throw new NotImplementedException();
        }
    }
}
