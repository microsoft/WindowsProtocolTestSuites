// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Gcc
{
    /*
    SimpleNumericString ::= NumericString (SIZE (1..255)) (FROM ("0123456789"))
    */
    [Asn1StringConstraint(MinSize = 1, MaxSize = 255, PermittedCharSet = "0123456789")]
    public class SimpleNumericString : Asn1NumericString 
    {
        public SimpleNumericString()
        {

        }

        public SimpleNumericString(string val) 
            : base(val)
        {

        }
    }
}
