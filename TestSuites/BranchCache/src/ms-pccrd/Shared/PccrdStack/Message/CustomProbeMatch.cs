// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrd
{
    using System.Xml.Serialization;
    using Microsoft.Protocols.TestTools.StackSdk.WsDiscovery;

    /// <summary>
    /// Probematch strcture for PCCRD
    /// </summary>
    public partial class CustomProbeMatchType : ProbeMatchType
    {
        /// <summary>
        /// PeerDistData field
        /// </summary>
        private PeerDistData peerDistData;

        /// <summary>
        /// Gets or sets the PeerDistData property
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(
            Namespace = "http://schemas.microsoft.com/p2p/2007/09/PeerDistributionDiscovery",
            Order = 6)]
        public PeerDistData PeerDistData
        {
            get
            {
                return this.peerDistData;
            }

            set
            {
                this.peerDistData = value;
            }
        }
    }

    /// <summary>
    /// The PeerDistData
    /// </summary>
    [System.SerializableAttribute]
    [XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery")]
    public class PeerDistData
    {
        /// <summary>
        /// block count
        /// </summary>
        private string blockCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="PeerDistData"/> class
        /// </summary>
        /// <param name="blockCount">The block Count</param>
        public PeerDistData(string blockCount)
        {
            this.blockCount = blockCount;
        }

        /// <summary>
        /// Gets or sets the blockCount
        /// </summary>
        public string BlockCount
        {
            get { return this.blockCount; }
            set { this.blockCount = value; }
        }
    }
}
