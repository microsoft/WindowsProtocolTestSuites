// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    LDAPString ::= OCTET STRING
    */
    public class LDAPString : Asn1OctetString
    {
        public LDAPString()
            : base()
        {
        }
        
        public LDAPString(string val)
            : base(val)
        {
        }

        public LDAPString(byte[] bytes)
            : base(bytes)
        {
        }
    }
}

