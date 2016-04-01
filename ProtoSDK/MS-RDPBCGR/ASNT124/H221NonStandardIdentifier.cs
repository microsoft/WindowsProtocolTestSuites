// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Gcc
{
    /*
    H221NonStandardIdentifier ::= OCTET STRING (SIZE (4..255))
    */
    [Asn1StringConstraint(MinSize = 4, MaxSize = 255)]
    public class H221NonStandardIdentifier : Asn1OctetString 
    {
        public H221NonStandardIdentifier()
        {

        }

        public H221NonStandardIdentifier(string val) 
            : base(val)
        {

        }
    }
}
