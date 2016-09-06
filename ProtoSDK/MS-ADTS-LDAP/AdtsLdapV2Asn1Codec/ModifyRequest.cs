// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2
{
    /*
    ModifyRequest ::=
    [APPLICATION 6] SEQUENCE {
         modifyobject         LDAPDN,
modifications  SEQUENCE OF ModifyRequest_modifications_element
}
    */
    [Asn1Tag(Asn1TagType.Application, 6)]
    public class ModifyRequest : Asn1Sequence
    {
        [Asn1Field(0)]
        public LDAPDN modifyobject { get; set; }
        
        [Asn1Field(1)]
        public Asn1SequenceOf<ModifyRequest_modifications_element> modifications { get; set; }
        
        public ModifyRequest()
        {
            this.modifyobject = null;
            this.modifications = null;
        }
        
        public ModifyRequest(
         LDAPDN modifyobject,
         Asn1SequenceOf<ModifyRequest_modifications_element> modifications)
        {
            this.modifyobject = modifyobject;
            this.modifications = modifications;
        }
    }
}

