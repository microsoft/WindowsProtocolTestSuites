// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Gcc
{
    /*
    DynamicChannelID ::= INTEGER (1001..65535)
    -- Those created and deleted by MCS
    */
    [Asn1IntegerBound(Min = 1001, Max = 65535)]
    public class DynamicChannelID : Asn1Integer
    {
        public DynamicChannelID()
            : base()
        {
        }

        public DynamicChannelID(long val)
            : base(val)
        {
        }
    }
}
