// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    public interface IFastArmor
    {
        KrbFastArmor Armor { get; }
  
    }

    public class FastArmorApRequest : IFastArmor
    {
        AP_REQ ApReq { get; set; }
        public KrbFastArmorType armorType { get; set;}
        public FastArmorApRequest(AP_REQ apReq)
        {
            ApReq = apReq;
            armorType = KrbFastArmorType.FX_FAST_ARMOR_AP_REQUEST;
        }

        public KrbFastArmor Armor
        {
            get 
            {
                Asn1BerEncodingBuffer buffer = new Asn1BerEncodingBuffer();
                ApReq.BerEncode(buffer);
                return new KrbFastArmor(new KerbInt32((long)armorType), new Asn1OctetString(buffer.Data));
            }
        }

        public static FastArmorApRequest Parse(KrbFastArmor armor)
        {
            if (armor.armor_type.Value != (long)KrbFastArmorType.FX_FAST_ARMOR_AP_REQUEST)
                throw new Exception();
            var apReq = new AP_REQ();
            apReq.BerDecode(new Asn1DecodingBuffer(armor.armor_value.ByteArrayValue));
            return new FastArmorApRequest(apReq);
        }
    }
    public static class FastArmorApRequestParser
    {
        public static IFastArmor Parse(KrbFastArmor armor)
        {
            switch ((KrbFastArmorType)armor.armor_type.Value)
            {
                case KrbFastArmorType.FX_FAST_ARMOR_AP_REQUEST:
                    return FastArmorApRequest.Parse(armor);
            }
            throw new NotImplementedException();
        }
    }
}
