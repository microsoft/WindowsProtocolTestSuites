// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    KERB-AD-RESTRICTION-ENTRY ::= SEQUENCE {
    restriction-type [0] Int32,
    restriction [1] OCTET STRING
    }
    */
    public class KERB_AD_RESTRICTION_ENTRY : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public KerbInt32 restriction_type { get; set; }

        /// <summary>
        /// If the LSAP_TOKEN_INFO_INTEGRITY is needed, use following code to get the object.
        /// LSAP_TOKEN_INFO_INTEGRITY ltii = new LSAP_TOKEN_INFO_INTEGRITY();
        /// ltii.GetElements(SomeObj.restriction);
        /// </summary>
        [Asn1Field(1), Asn1Tag(Asn1TagType.Context, 1)]
        public Asn1OctetString restriction { get; set; }

        public KERB_AD_RESTRICTION_ENTRY()
        {
            this.restriction_type = null;
            this.restriction = null;
        }

        public KERB_AD_RESTRICTION_ENTRY(
         KerbInt32 param0,
         Asn1OctetString param1)
        {
            this.restriction_type = param0;
            this.restriction = param1;
        }

        //Added manually
        public KERB_AD_RESTRICTION_ENTRY(
         KerbInt32 param0,
         LSAP_TOKEN_INFO_INTEGRITY param1)
        {
            this.restriction_type = param0;
            this.restriction = param1.SetElements();
        }

        public override int BerEncode(IAsn1BerEncodingBuffer buffer, bool explicitTag)
        {
            int len = base.BerEncode(buffer);
            len += LengthBerEncode(buffer, len);
            len += TagBerEncode(buffer, new Asn1Tag(Asn1TagType.Universal,
                Asn1TagValue.Sequence) { EncodingWay = EncodingWay.Constructed });
            return len;
        }

        public override int BerDecode(IAsn1DecodingBuffer buffer, bool explicitTag = true)
        {
            int consumed = 0;
            int length;
            Asn1Tag tag;
            consumed += TagBerDecode(buffer, out tag);
            consumed += LengthBerDecode(buffer, out length);
            consumed += base.BerDecode(buffer);
            return consumed;

        }

    }
}

