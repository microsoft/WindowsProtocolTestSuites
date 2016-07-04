// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Gcc
{
    /*
    TextString ::= BMPString (SIZE (0..255))
    */
    [Asn1StringConstraint(MinSize = 0, MaxSize = 255)]
    public class TextString : Asn1BmpString
    {
        public TextString()
        {

        }

        public TextString(string val) 
            : base(val)
        {

        }
    }
}