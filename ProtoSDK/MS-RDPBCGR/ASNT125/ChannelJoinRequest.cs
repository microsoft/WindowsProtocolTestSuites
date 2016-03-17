// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Mcs
{
    /*
    ChannelJoinRequest ::= [APPLICATION 14] IMPLICIT SEQUENCE
    {
        initiator UserId,
        channelId ChannelId
        -- may be zero
    }
    */
    [Asn1Tag(Asn1TagType.Application, 14)]
    public class ChannelJoinRequest : Asn1Sequence
    {
        [Asn1Field(0)]
        public UserId initiator { get; set; }
        
        [Asn1Field(1)]
        public ChannelId channelId { get; set; }
        
        public ChannelJoinRequest()
        {
            this.initiator = null;
            this.channelId = null;
        }
        
        public ChannelJoinRequest(
         UserId initiator,
         ChannelId channelId)
        {
            this.initiator = initiator;
            this.channelId = channelId;
        }
    }
}

