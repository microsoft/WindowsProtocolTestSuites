// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Gcc
{
    /*
    UserID ::= DynamicChannelID
    */
    public class UserID : DynamicChannelID
    {
        public UserID()
            : base()
        {
        }

        public UserID(long val)
            : base(val)
        {
        }
    }
}
