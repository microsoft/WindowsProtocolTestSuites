// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    AttributeDescriptionList ::= SEQUENCE OF AttributeDescription
    */
    public class AttributeDescriptionList : Asn1SequenceOf<AttributeDescription>
    {
        public AttributeDescriptionList()
            : base()
        {
            this.Elements = null;
        }
        
        public AttributeDescriptionList(AttributeDescription[] val)
            : base(val)
        {
        }
    }
}

