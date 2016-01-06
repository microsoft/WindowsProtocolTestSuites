// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2
{
    /*
    SearchRequest_derefAliases ::= ENUMERATED {
                             neverDerefAliases     (0),
                             derefInSearching      (1),
                             derefFindingBaseObj   (2),
                             alwaysDerefAliases    (3)
                        }
    */
    public class SearchRequest_derefAliases : Asn1Enumerated
    {
        [Asn1EnumeratedElement]
        public const long neverDerefAliases = 0;
        
        [Asn1EnumeratedElement]
        public const long derefInSearching = 1;
        
        [Asn1EnumeratedElement]
        public const long derefFindingBaseObj = 2;
        
        [Asn1EnumeratedElement]
        public const long alwaysDerefAliases = 3;
        
        public SearchRequest_derefAliases()
            : base()
        {
        }
        
        public SearchRequest_derefAliases(long val)
            : base()
        {
        }
    }
}

