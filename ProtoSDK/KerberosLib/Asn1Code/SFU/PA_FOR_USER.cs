using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    public class PA_FOR_USER : PA_DATA
    {
        public PA_FOR_USER(PA_FOR_USER_ENC value)
        {
            padata_type = new KerbInt32(129);

            var buffer = new Asn1BerEncodingBuffer();
            value.BerEncode(buffer, true);

            padata_value = new Asn1OctetString(buffer.Data);
        }
    }

    public class PA_FOR_USER_ENC : Asn1Sequence
    {
        public PA_FOR_USER_ENC(PrincipalName name, Realm realm)
        {
            userName = name;
            userRealm = realm;
            auth_package = new KerberosString("Kerberos");

        }

        public void UpdateChecksum(byte[] key)
        {
            var ASREQRawPAForUser = new List<byte>();
            ASREQRawPAForUser.AddRange(BitConverter.GetBytes((int)userName.name_type.Value));

            foreach (var str in userName.name_string.Elements)
            {
                ASREQRawPAForUser.AddRange(str.ByteArrayValue);
            }

            ASREQRawPAForUser.AddRange(userRealm.ByteArrayValue);

            ASREQRawPAForUser.AddRange(auth_package.ByteArrayValue);

            var data = ASREQRawPAForUser.ToArray();

            cksum = new Checksum(new KerbInt32(-138), new Asn1OctetString(CHKSUM(key, 17, data)));
        }

        private byte[] CHKSUM(byte[] K, int T, byte[] data)
        {
            var converter = new ASCIIEncoding();
            HMACMD5 hmacmd5 = new HMACMD5(K);
            byte[] tempdata = converter.GetBytes("signaturekey\0");
            byte[] Ksign = hmacmd5.ComputeHash(tempdata);

            var buf = new List<byte>();
            buf.AddRange(BitConverter.GetBytes(T));
            buf.AddRange(data);
            byte[] finalData = buf.ToArray();

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] tmp = md5.ComputeHash(finalData);

            hmacmd5 = new HMACMD5(Ksign);
            byte[] ret = hmacmd5.ComputeHash(tmp);

            return ret;
        }


        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public PrincipalName userName;

        [Asn1Field(1), Asn1Tag(Asn1TagType.Context, 1)]
        public Realm userRealm;

        [Asn1Field(2), Asn1Tag(Asn1TagType.Context, 2)]
        public Checksum cksum;

        [Asn1Field(3), Asn1Tag(Asn1TagType.Context, 3)]
        public KerberosString auth_package;
    }
}
