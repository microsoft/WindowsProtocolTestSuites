// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2
{
    /*
    ModifyRequest_modifications_element ::= SEQUENCE {
                             operation     ModifyRequest_modifications_element_operation,
                             modification  ModifyRequest_modifications_element_modification
                        }
    */
    public class ModifyRequest_modifications_element : Asn1Sequence
    {
        [Asn1Field(0)]
        public ModifyRequest_modifications_element_operation operation { get; set; }
        
        [Asn1Field(1)]
        public ModifyRequest_modifications_element_modification modification { get; set; }
        
        public ModifyRequest_modifications_element()
        {
            this.operation = null;
            this.modification = null;
        }
        
        public ModifyRequest_modifications_element(
         ModifyRequest_modifications_element_operation operation,
         ModifyRequest_modifications_element_modification modification)
        {
            this.operation = operation;
            this.modification = modification;
        }
    }
}

