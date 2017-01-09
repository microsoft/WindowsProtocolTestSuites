// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    SearchRequest_scope  ::=         ENUMERATED {
                        baseObject              (0),
                        singleLevel             (1),
                        wholeSubtree            (2) }
    */
    public class SearchRequest_scope : Asn1Enumerated
    {
        [Asn1EnumeratedElement]
        public const long baseObject = 0;
        
        [Asn1EnumeratedElement]
        public const long singleLevel = 1;
        
        [Asn1EnumeratedElement]
        public const long wholeSubtree = 2;
        
        public SearchRequest_scope()
            : base()
        {
        }
        
        public SearchRequest_scope(long val)
            : base(val)
        {
        }
    }
}

