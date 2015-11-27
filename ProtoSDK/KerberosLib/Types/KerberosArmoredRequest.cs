// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    public class KerberosArmoredRequest
    {
        public KrbFastArmoredReq FastArmoredReq
        {
            get
            {
                Asn1BerEncodingBuffer fastReqBuffer = new Asn1BerEncodingBuffer();
                EncryptedData encData;
                if (ArmorKey != null)
                {
                    FastReq.FastReq.BerEncode(fastReqBuffer);
                    KerberosUtility.OnDumpMessage("KRB5:KrbFastArmoredReq(enc-fast-req)",
                                                "An encrypted KrbFastReq in PA_FX_FAST_REQUEST",
                        KerberosUtility.DumpLevel.PartialMessage,
                        fastReqBuffer.Data);
                    var cipher = KerberosUtility.Encrypt((Cryptographic.EncryptionType)this.EType,
                                                              ArmorKey,
                                                              fastReqBuffer.Data,
                                                              (int)KeyUsageNumber.FAST_ENC);
                    encData = new EncryptedData(new KerbInt32(EType), null, new Asn1OctetString(cipher));
                }
                else
                {
                    encData = EncFastReq;
                }
                if (FastArmor == null)
                    return new KrbFastArmoredReq(null, CheckSum, encData);
                return new KrbFastArmoredReq(FastArmor.Armor, CheckSum, encData);

            }
        }

        public IFastArmor FastArmor
        {
            get;
            set;
        }

        public Checksum CheckSum
        {
            get;
            set;
        }

        public byte[] ArmorKey { get; set; }


        public long EType { get; set; }

        private EncryptedData EncFastReq;

        public KerberosFastRequest FastReq
        {
            get;
            set;
        }



        public KerberosArmoredRequest(IFastArmor fastArmor, Checksum checkSum, long etype, byte[] armorKey, KerberosFastRequest fastReq)
        {
            
            FastArmor = fastArmor;
            CheckSum = checkSum;
            ArmorKey = armorKey;
            EType = etype;
            FastReq = fastReq;
        }
        public KerberosArmoredRequest(IFastArmor fastArmor, Checksum checkSum, EncryptedData encFastReq)
        {
            FastArmor = fastArmor;
            CheckSum = checkSum;
            ArmorKey = null;
            EType = (long) encFastReq.etype.Value;
            EncFastReq = encFastReq;
        }

        public void Decrypt(byte[] armorKey)
        {
            ArmorKey = armorKey;
            var decrypted = KerberosUtility.Decrypt(
                (Cryptographic.EncryptionType)EncFastReq.etype.Value,
                armorKey,
                EncFastReq.cipher.ByteArrayValue,
                (int)KeyUsageNumber.FAST_ENC);
            KrbFastReq krbFastReq = new KrbFastReq();
            krbFastReq.BerDecode(new Asn1DecodingBuffer(decrypted));
            FastReq = new KerberosFastRequest(krbFastReq);
        }

        public static KerberosArmoredRequest FromBytes(byte[] msgdata)
        {
            var armoredReq = new KrbFastArmoredReq();
            armoredReq.BerDecode(new Asn1DecodingBuffer(msgdata));
            var kerberosArmoredReq = new KerberosArmoredRequest(
                FastArmorApRequestParser.Parse(armoredReq.armor),
                armoredReq.req_checksum,
                armoredReq.enc_fast_req
                );
            kerberosArmoredReq.ArmorKey = null;
            return kerberosArmoredReq;
        }

    }
}
