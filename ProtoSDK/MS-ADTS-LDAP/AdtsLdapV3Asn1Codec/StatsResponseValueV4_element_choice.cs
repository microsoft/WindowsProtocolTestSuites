// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
     StatsResponseValueV4_element_choice ::= CHOICE {
         intStatistic  [0]     INTEGER
         stringStatistic [1]    OCTET STRING
     }

    */
    public class StatsResponseValueV4_element_choice : Asn1Choice
    {
        [Asn1ChoiceIndex]
        public const long intStatistic = 0;
        [Asn1ChoiceElement(intStatistic), Asn1Tag(Asn1TagType.Context, 0)]
        protected Asn1Integer field0 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long stringStatistic = 1;
        [Asn1ChoiceElement(stringStatistic), Asn1Tag(Asn1TagType.Context, 1)]
        protected Asn1OctetString field1 { get; set; }
        
        public StatsResponseValueV4_element_choice()
            : base()
        {
        }
        
        public StatsResponseValueV4_element_choice(long? choiceIndex, Asn1Object obj)
            : base(choiceIndex, obj)
        {
        }
    }
}

