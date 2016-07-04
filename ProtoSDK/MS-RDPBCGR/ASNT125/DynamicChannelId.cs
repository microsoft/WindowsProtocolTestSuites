// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Mcs
{
    /*
    DynamicChannelId ::= ChannelId (1001..65535) -- those created and deleted
    */
    [Asn1IntegerBound(Min = 1001, Max = 65535)]
    public class DynamicChannelId : ChannelId 
    {
        public DynamicChannelId() : base()
        {

        }

        public DynamicChannelId(long? val) : base(val)
        {

        }
    }
}
