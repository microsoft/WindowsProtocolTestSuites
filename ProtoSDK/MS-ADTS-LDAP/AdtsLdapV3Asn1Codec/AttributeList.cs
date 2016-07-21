// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    AttributeList ::= SEQUENCE OF Attribute
    */
    public class AttributeList : Asn1SequenceOf<AttributeList_element>
    {
        public AttributeList()
            : base()
        {
            this.Elements = null;
        }
        
        public AttributeList(AttributeList_element[] val)
            : base(val)
        {
        }
    }
}

