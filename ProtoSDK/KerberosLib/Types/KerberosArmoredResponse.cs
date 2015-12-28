// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    public class KerberosArmoredResponse
    {
        public KrbFastArmoredRep FastArmoredRep
        {
            get
            {
                EncryptedData encData;
                if (EncFastRep == null)
                {
                    Asn1BerEncodingBuffer fastRepBuffer = new Asn1BerEncodingBuffer();
                    FastRep.FastResponse.BerEncode(fastRepBuffer);

                    var cipher = KerberosUtility.Encrypt((Cryptographic.EncryptionType)this.EType,
                                                              ArmorKey,
                                                              fastRepBuffer.Data,
                                                              (int)KeyUsageNumber.FAST_REP);
                    encData = new EncryptedData(new KerbInt32(EType), null, new Asn1OctetString(cipher));
                }
                else
                {
                    encData = EncFastRep;
                }
                return new KrbFastArmoredRep(encData);

            }
        }

        public long EType { get; set; }
        public byte[] ArmorKey { get; set; }
        public KerberosFastResponse FastRep { get; set; }
        private EncryptedData EncFastRep;
        public KerberosArmoredResponse(EncryptedData encFastRep)
        {
            EType = (long)encFastRep.etype.Value;
            ArmorKey = null;
            EncFastRep = encFastRep;
        }

        public KerberosArmoredResponse(byte[] armorKey, long eType, KerberosFastResponse fastRep)
        {
            ArmorKey = armorKey;
            EType = eType;
            FastRep = fastRep;
        }

        public void Decrypt(byte[] armorKey)
        {
            ArmorKey = armorKey;
            var decrypted = KerberosUtility.Decrypt(
                (Cryptographic.EncryptionType)EncFastRep.etype.Value,
                armorKey,
                EncFastRep.cipher.ByteArrayValue,
                (int)KeyUsageNumber.FAST_ENC);
            KrbFastResponse krbFastRep = new KrbFastResponse();
            krbFastRep.BerDecode(new Asn1DecodingBuffer(decrypted));
            FastRep = new KerberosFastResponse(krbFastRep);
        }

        public static KerberosArmoredResponse FromBytes(byte[] msgdata)
        {
            var armoredRep = new KrbFastArmoredRep();
            armoredRep.BerDecode(new Asn1DecodingBuffer(msgdata));
            var kerberosArmoredReq = new KerberosArmoredResponse(
                armoredRep.enc_fast_rep
                );
            return kerberosArmoredReq;
        }
    }
}
