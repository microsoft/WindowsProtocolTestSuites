// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    ModifyRequest_modification_element ::= SEQUENCE {
                        operation      ModifyRequest_modification_operation,
                        modification    AttributeTypeAndValues }
    */
    public class ModifyRequest_modification_element : Asn1Sequence
    {
        [Asn1Field(0)]
        public ModifyRequest_modification_element_operation operation { get; set; }
        
        [Asn1Field(1)]
        public AttributeTypeAndValues modification { get; set; }
        
        public ModifyRequest_modification_element()
        {
            this.operation = null;
            this.modification = null;
        }
        
        public ModifyRequest_modification_element(
         ModifyRequest_modification_element_operation operation,
         AttributeTypeAndValues modification)
        {
            this.operation = operation;
            this.modification = modification;
        }
    }
}

