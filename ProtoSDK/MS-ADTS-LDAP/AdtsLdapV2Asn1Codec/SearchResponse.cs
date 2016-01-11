// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2
{
    /*
    SearchResponse ::= CHOICE {
         entry          SearchResponse_entry,
         resultCode     SearchResponse_resultCode
    }
    */
    public class SearchResponse : Asn1Choice
    {
        [Asn1ChoiceIndex]
        public const long entry = 0;
        [Asn1ChoiceElement(entry)]
        protected SearchResponse_entry field0 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long resultCode = 1;
        [Asn1ChoiceElement(resultCode)]
        protected SearchResponse_resultCode field1 { get; set; }
        
        public SearchResponse()
            : base()
        {
        }
        
        public SearchResponse(long? choiceIndex, Asn1Object obj)
            : base(choiceIndex, obj)
        {
        }
    }
}

