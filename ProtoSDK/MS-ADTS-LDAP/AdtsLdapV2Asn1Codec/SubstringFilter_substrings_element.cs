// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2
{
    /*
    SubstringFilter_substrings_element ::= CHOICE {
                        initial [0] LDAPString,
                        any     [1] LDAPString,
                        final   [2] LDAPString }
    */
    public class SubstringFilter_substrings_element : Asn1Choice
    {
        [Asn1ChoiceIndex]
        public const long initial = 0;
        [Asn1ChoiceElement(initial), Asn1Tag(Asn1TagType.Context, 0)]
        protected LDAPString field0 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long any = 1;
        [Asn1ChoiceElement(any), Asn1Tag(Asn1TagType.Context, 1)]
        protected LDAPString field1 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long final = 2;
        [Asn1ChoiceElement(final), Asn1Tag(Asn1TagType.Context, 2)]
        protected LDAPString field2 { get; set; }
        
        public SubstringFilter_substrings_element()
            : base()
        {
        }
        
        public SubstringFilter_substrings_element(long? choiceIndex, Asn1Object obj)
            : base(choiceIndex, obj)
        {
        }
    }
}

