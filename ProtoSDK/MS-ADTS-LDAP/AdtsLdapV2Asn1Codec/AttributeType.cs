// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2
{
    /*
    AttributeType ::= LDAPString
                -- text name of the attribute, or dotted
                -- OID representation
    */
    public class AttributeType : LDAPString
    {
        public AttributeType()
            : base()
        {
        }

        public AttributeType(string val)
            : base(val)
        {
        }
    }
}

