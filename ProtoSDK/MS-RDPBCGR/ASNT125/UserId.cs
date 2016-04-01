// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Mcs
{
    /*
    UserId ::= DynamicChannelId -- created by Attach-User
    */
    public class UserId : DynamicChannelId
    {
        public UserId() : base()
        {

        }

        public UserId(long? val) : base(val)
        {

        }
    }
}
