// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Mcs
{
    /*
    ChannelJoinConfirm ::= [APPLICATION 15] IMPLICIT SEQUENCE
    {
        result Result,
        initiator UserId,
        requested ChannelId,
        -- may be zero
        channelId ChannelId OPTIONAL
    }
    */
    [Asn1Tag(Asn1TagType.Application, 15)]
    public class ChannelJoinConfirm : Asn1Sequence
    {
        [Asn1Field(0)]
        public Result result { get; set; }
        
        [Asn1Field(1)]
        public UserId initiator { get; set; }
        
        [Asn1Field(2)]
        public ChannelId requested { get; set; }
        
        [Asn1Field(3, Optional = true)]
        public ChannelId channelId { get; set; }
        
        public ChannelJoinConfirm()
        {
            this.result = null;
            this.initiator = null;
            this.requested = null;
            this.channelId = null;
        }
        
        public ChannelJoinConfirm(
         Result result,
         UserId initiator,
         ChannelId requested,
         ChannelId channelId)
        {
            this.result = result;
            this.initiator = initiator;
            this.requested = requested;
            this.channelId = channelId;
        }
    }
}

