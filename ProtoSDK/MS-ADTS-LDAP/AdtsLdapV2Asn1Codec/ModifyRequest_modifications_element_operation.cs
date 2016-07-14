// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2
{
    /*
    ModifyRequest_modifications_element_operation ::=     ENUMERATED {
                                             add      (0),
                                             delete   (1),
                                             replace  (2)
                                           }
    */
    public class ModifyRequest_modifications_element_operation : Asn1Enumerated
    {
        [Asn1EnumeratedElement]
        public const long add = 0;
        
        [Asn1EnumeratedElement]
        public const long delete = 1;
        
        [Asn1EnumeratedElement]
        public const long replace = 2;
        
        public ModifyRequest_modifications_element_operation()
            : base()
        {
        }
        
        public ModifyRequest_modifications_element_operation(long val)
            : base()
        {
        }
    }
}

