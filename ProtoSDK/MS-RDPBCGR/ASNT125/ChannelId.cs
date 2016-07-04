// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Mcs
{
    /*
    ChannelId ::= INTEGER (0..65535) -- range is 16 bits
    */
    [Asn1IntegerBound(Min = 0, Max = 65535)]
    public class ChannelId : Asn1Integer 
    {
        public ChannelId() : base()
        {

        }

        public ChannelId(long? val) : base(val)
        {

        }
    }
}
