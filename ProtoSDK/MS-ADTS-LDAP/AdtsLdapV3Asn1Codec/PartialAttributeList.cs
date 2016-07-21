// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
     PartialAttributeList ::= SEQUENCE OF PartialAttributeListElement
    */
    public class PartialAttributeList : Asn1SequenceOf<PartialAttributeList_element>
    {
        public PartialAttributeList()
            : base()
        {
            this.Elements = null;
        }
        
        public PartialAttributeList(PartialAttributeList_element[] val)
            : base(val)
        {
        }
    }
}

