// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2
{
    /*
    RelativeLDAPDN ::= LDAPString
    */
    public class RelativeLDAPDN : LDAPString
    {
        public RelativeLDAPDN()
            : base()
        {
        }

        public RelativeLDAPDN(string val)
            : base(val)
        {
        }
    }
}

