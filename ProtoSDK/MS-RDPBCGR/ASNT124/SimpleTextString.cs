// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Gcc
{
    /*
    SimpleTextString ::= BMPString (SIZE (0..255)) (FROM
    (simpleTextFirstCharacter..simpleTextLastCharacter))
    simpleTextFirstCharacter UniversalString ::= {0, 0, 0, 0}
    simpleTextLastCharacter UniversalString ::= {0, 0, 0, 255}
    */
    [Asn1StringConstraint(MinSize = 0, MaxSize = 255)]
    public class SimpleTextString : Asn1OctetString 
    {
        public SimpleTextString()
        {

        }

        public SimpleTextString(string val) 
            : base(val)
        {

        }
    }
}