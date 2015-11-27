// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth 
{
    /*
     KrbFastArmor ::= SEQUENCE {
          armor-type   [0] Int32,
              -- Type of the armor.
          armor-value  [1] OCTET STRING,
              -- Value of the armor.
          ...
      }
    */
    public class KrbFastArmor : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public KerbInt32 armor_type { get; set; }
        
        [Asn1Field(1), Asn1Tag(Asn1TagType.Context, 1)]
        public Asn1OctetString armor_value { get; set; }
        
        public KrbFastArmor()
        {
            this.armor_type = null;
            this.armor_value = null;
        }
        
        public KrbFastArmor(
         KerbInt32 param0,
         Asn1OctetString param1)
        {
            this.armor_type = param0;
            this.armor_value = param1;
        }
    }
}

