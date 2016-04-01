// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Mcs
{
    /*
    DomainParameters ::= SEQUENCE
    {
        maxChannelIds INTEGER (0..MAX),
        -- a limit on channel ids in use,
        -- static + user id + private + assigned
        maxUserIds INTEGER (0..MAX),
        -- a sublimit on user id channels alone
        maxTokenIds INTEGER (0..MAX),
        -- a limit on token ids in use
        -- grabbed + inhibited + giving + ungivable + given
        numPriorities INTEGER (0..MAX),
        -- the number of TCs in an MCS connection
        minThroughput INTEGER (0..MAX),
        -- the enforced number of octets per second
        maxHeight INTEGER (0..MAX),
        -- a limit on the height of a provider
        maxMCSPDUsize INTEGER (0..MAX),
        -- an octet limit on domain MCSPDUs
        protocolVersion INTEGER (0..MAX)
    }
    */
    public class DomainParameters : Asn1Sequence
    {
        [Asn1Field(0)/*, Asn1IntegerBound(Min = 0)*/]
        public Asn1Integer maxChannelIds { get; set; }
        
        [Asn1Field(1)/*, Asn1IntegerBound(Min = 0)*/]
        public Asn1Integer maxUserIds { get; set; }
        
        [Asn1Field(2)/*, Asn1IntegerBound(Min = 0)*/]
        public Asn1Integer maxTokenIds { get; set; }
        
        [Asn1Field(3), Asn1IntegerBound(Min = 0)]
        public Asn1Integer numPriorities { get; set; }
        
        [Asn1Field(4), Asn1IntegerBound(Min = 0)]
        public Asn1Integer minThroughput { get; set; }
        
        [Asn1Field(5), Asn1IntegerBound(Min = 0)]
        public Asn1Integer maxHeight { get; set; }
        
        [Asn1Field(6)/*, Asn1IntegerBound(Min = 0)*/]
        public Asn1Integer maxMCSPDUsize { get; set; }
        
        [Asn1Field(7), Asn1IntegerBound(Min = 0)]
        public Asn1Integer protocolVersion { get; set; }
        
        public DomainParameters()
        {
            this.maxChannelIds = null;
            this.maxUserIds = null;
            this.maxTokenIds = null;
            this.numPriorities = null;
            this.minThroughput = null;
            this.maxHeight = null;
            this.maxMCSPDUsize = null;
            this.protocolVersion = null;
        }
        
        public DomainParameters(
         Asn1Integer maxChannelIds,
         Asn1Integer maxUserIds,
         Asn1Integer maxTokenIds,
         Asn1Integer numPriorities,
         Asn1Integer minThroughput,
         Asn1Integer maxHeight,
         Asn1Integer maxMCSPDUsize,
         Asn1Integer protocolVersion)
        {
            this.maxChannelIds = maxChannelIds;
            this.maxUserIds = maxUserIds;
            this.maxTokenIds = maxTokenIds;
            this.numPriorities = numPriorities;
            this.minThroughput = minThroughput;
            this.maxHeight = maxHeight;
            this.maxMCSPDUsize = maxMCSPDUsize;
            this.protocolVersion = protocolVersion;
        }

        public override int BerEncode(IAsn1BerEncodingBuffer buffer, bool explicitTag = true)
        {
            return base.BerEncode(buffer, explicitTag);
        }

        public override int BerDecode(IAsn1DecodingBuffer buffer, bool explicitTag = true)
        {
            return base.BerDecode(buffer, explicitTag);
        }

    }
}

