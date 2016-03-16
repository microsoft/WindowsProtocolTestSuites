// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Mcs
{
    /*
    SendDataRequest ::= [APPLICATION 25] IMPLICIT SEQUENCE
    {
        initiator UserId,
        channelId ChannelId,
        dataPriority DataPriority,
        segmentation Segmentation,
        userData OCTET STRING
    }
    */
    [Asn1Tag(Asn1TagType.Application, 25)]
    public class SendDataRequest : Asn1Sequence
    {
        [Asn1Field(0)]
        public UserId initiator { get; set; }
        
        [Asn1Field(1)]
        public ChannelId channelId { get; set; }
        
        [Asn1Field(2)]
        public DataPriority dataPriority { get; set; }
        
        [Asn1Field(3)]
        public Segmentation segmentation { get; set; }
        
        [Asn1Field(4)]
        public Asn1OctetString userData { get; set; }
        
        public SendDataRequest()
        {
            this.initiator = null;
            this.channelId = null;
            this.dataPriority = null;
            this.segmentation = null;
            this.userData = null;
        }
        
        public SendDataRequest(
         UserId initiator,
         ChannelId channelId,
         DataPriority dataPriority,
         Segmentation segmentation,
         Asn1OctetString userData)
        {
            this.initiator = initiator;
            this.channelId = channelId;
            this.dataPriority = dataPriority;
            this.segmentation = segmentation;
            this.userData = userData;
        }
    }
}

