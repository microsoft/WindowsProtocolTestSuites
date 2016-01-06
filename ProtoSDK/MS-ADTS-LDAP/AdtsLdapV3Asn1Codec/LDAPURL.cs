// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    LDAPURL ::= LDAPString -- limited to characters permitted in URLs
    */
    public class LDAPURL : LDAPString
    {
        public LDAPURL()
            : base()
        {
        }

        public LDAPURL(string val)
            : base(val)
        {
        }
    }
}

