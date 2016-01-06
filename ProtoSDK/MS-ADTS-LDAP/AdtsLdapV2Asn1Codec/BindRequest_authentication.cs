// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2
{
    /*
    BindRequest_authentication ::= CHOICE {
                             simple        [0] OCTET STRING,
                                       -- a zero length octet string
                                       -- implies an unauthenticated
                                       -- bind.
                             krbv42LDAP    [1] OCTET STRING,
                             krbv42DSA     [2] OCTET STRING,
                                       -- values as returned by
                                       -- krb_mk_req()
                                       -- Other values in later versions
                                       -- of this protocol.
                             sicilyPackageDiscovery [9]    OCTET STRING,
                             sicilyNegotiate        [10]   OCTET STRING,
                             sicilyResponse         [11]   OCTET STRING,  }
    */
    public class BindRequest_authentication : Asn1Choice
    {
        [Asn1ChoiceIndex]
        public const long simple = 0;
        [Asn1ChoiceElement(simple), Asn1Tag(Asn1TagType.Context, 0)]
        protected Asn1OctetString field0 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long krbv42LDAP = 1;
        [Asn1ChoiceElement(krbv42LDAP), Asn1Tag(Asn1TagType.Context, 1)]
        protected Asn1OctetString field1 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long krbv42DSA = 2;
        [Asn1ChoiceElement(krbv42DSA), Asn1Tag(Asn1TagType.Context, 2)]
        protected Asn1OctetString field2 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long sicilyPackageDiscovery = 3;
        [Asn1ChoiceElement(sicilyPackageDiscovery), Asn1Tag(Asn1TagType.Context, 9)]
        protected Asn1OctetString field3 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long sicilyNegotiate = 4;
        [Asn1ChoiceElement(sicilyNegotiate), Asn1Tag(Asn1TagType.Context, 10)]
        protected Asn1OctetString field4 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long sicilyResponse = 5;
        [Asn1ChoiceElement(sicilyResponse), Asn1Tag(Asn1TagType.Context, 11)]
        protected Asn1OctetString field5 { get; set; }
        
        public BindRequest_authentication()
            : base()
        {
        }
        
        public BindRequest_authentication(long? choiceIndex, Asn1Object obj)
            : base(choiceIndex, obj)
        {
        }
    }
}

