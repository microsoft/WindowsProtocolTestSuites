// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    LDAPOID ::= OCTET STRING
    */
    public class LDAPOID : Asn1OctetString
    {
        public LDAPOID()
            : base()
        {
        }
        
        public LDAPOID(string val)
            : base(val)
        {
        }
    }
}

