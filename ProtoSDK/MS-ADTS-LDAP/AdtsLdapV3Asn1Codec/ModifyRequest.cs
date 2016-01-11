// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    ModifyRequest ::= [APPLICATION 6] SEQUENCE {
                object          LDAPDN,
                modification    SEQUENCE OF ModifyRequest_modificationElement }
    */
    [Asn1Tag(Asn1TagType.Application, 6)]
    public class ModifyRequest : Asn1Sequence
    {
        [Asn1Field(0)]
        public LDAPDN object1 { get; set; }
        
        [Asn1Field(1)]
        public Asn1SequenceOf<ModifyRequest_modification_element> modification { get; set; }
        
        public ModifyRequest()
        {
            this.object1 = null;
            this.modification = null;
        }
        
        public ModifyRequest(
         LDAPDN object1,
         Asn1SequenceOf<ModifyRequest_modification_element> modification)
        {
            this.object1 = object1;
            this.modification = modification;
        }
    }
}

