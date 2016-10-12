// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.BranchCache
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The Probe message is part of the WS-Discovery standard.
    /// </summary>
    public class ProbeMsg
    {
        /// <summary>
        /// The id of the probe message 
        /// </summary>
        public string messageId;

        /// <summary>
        /// Represents the list of discovery provider types.
        /// </summary>
        public string types;

        /// <summary>
        /// Represents the list of discovery provider scopes
        /// </summary>
        public string scopes;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProbeMsg"/> class
        /// </summary>
        /// <param name="messageId">he id of the probe message</param>
        /// <param name="types">Represents the list of discovery provider types.</param>
        /// <param name="scopes">Represents the list of discovery provider scopes</param>
        public ProbeMsg(string messageId, string types, string scopes)
        {
            this.messageId = messageId;
            this.types = types;
            this.scopes = scopes;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProbeMsg"/> class without parameter
        /// </summary>
        public ProbeMsg()
        {
            this.messageId = string.Empty;
            this.types = string.Empty;
            this.scopes = string.Empty;
        }
    }

    /// <summary>
    /// The properties of service
    /// </summary>
    public class ServiceProperty
    {
        /// <summary>
        /// Identifies the responding device
        /// </summary>
        public string Address;

        /// <summary>
        /// Represents the list of discovery provider types.
        /// </summary>
        public string Types;

        /// <summary>
        /// Represents the list of discovery provider scopes
        /// </summary>
        public string Scopes;

        /// <summary>
        /// Contains the list of transport URIs supported by the peer responding
        /// to the Probe message (with a ProbeMatch message).
        /// </summary>
        public string XAddrs;

        /// <summary>
        /// The current metadata version
        /// </summary>
        public uint MetadataVersion;

        /// <summary>
        /// The block count
        /// </summary>
        public string BlockCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceProperty"/> class
        /// </summary>
        /// <param name="address">Identifies the responding device</param>
        /// <param name="type">Represents the list of discovery provider types.</param>
        /// <param name="scope">Represents the list of discovery provider scopes</param>
        /// <param name="xAddrs">Contains the list of transport URIs supported by the peer responding
        /// to the Probe message (with a ProbeMatch message).</param>
        /// <param name="metadataVersion">The current metadata version</param>
        /// <param name="blockCount">The block count</param>
        public ServiceProperty(
            string address,
            string type,
            string scope,
            string xAddrs,
            uint metadataVersion,
            string blockCount)
        {
            this.Address = address;
            this.Types = type;
            this.Scopes = scope;
            this.XAddrs = xAddrs;
            this.MetadataVersion = metadataVersion;
            this.BlockCount = blockCount;
        }
    }
}
