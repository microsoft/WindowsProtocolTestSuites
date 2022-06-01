// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pccrd
{
    /// <summary>
    /// The Probe message is part of the WS-Discovery standard.
    /// </summary>
     public class ProbeMsg
    {
        /// <summary>
        /// The id of the probe message 
        /// </summary>
        private string messageId;

        /// <summary>
        /// Represents the list of discovery provider types.
        /// </summary>
        private string types;

        /// <summary>
        /// Represents the list of discovery provider scopes
        /// </summary>
        private string scopes;

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

        /// <summary>
        /// Gets or sets the id of the probe message 
        /// </summary>
        public string MessageId
        {
            get
            {
                return this.messageId;
            }

            set
            {
                this.messageId = value;
            }
        }

        /// <summary>
        /// Gets or sets the list of discovery provider types.
        /// </summary>
        public string Types
        {
            get
            {
                return this.types;
            }

            set
            {
                this.types = value;
            }
        }

        /// <summary>
        /// Gets or sets the list of discovery provider scopes
        /// </summary>
        public string Scopes
        {
            get
            {
                return this.scopes;
            }

            set
            {
                this.scopes = value;
            }
        }
    }

    /// <summary>
    /// The properties of service
    /// </summary>
    public class ServiceProperty
    {
        /// <summary>
        /// The parameter identifies the responding device
        /// </summary>
        private string address;

        /// <summary>
        /// The discovery provider types.
        /// </summary>
        private string types;

        /// <summary>
        /// The discovery provider scopes
        /// </summary>
        private string scopes;

        /// <summary>
        /// The transport URIs supported by the peer responding to the
        /// Probe message (with a ProbeMatch message)
        /// </summary>
        private string xAddrs;

        /// <summary>
        /// The current metadata version and MUST be set to 1
        /// </summary>
        private uint metadataVersion;

        /// <summary>
        /// The block count
        /// </summary>
        private string blockCount;

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
            this.address = address;
            this.types = type;
            this.scopes = scope;
            this.xAddrs = xAddrs;
            this.metadataVersion = metadataVersion;
            this.blockCount = blockCount;
        }

        /// <summary>
        /// Gets or sets the parameter identifies the responding device
        /// </summary>
        public string Address
        {
            get
            {
                return this.address;
            }

            set
            {
                this.address = value;
            }
        }

        /// <summary>
        /// Gets or sets the discovery provider types.
        /// </summary>
        public string Types
        {
            get
            {
                return this.types;
            }

            set
            {
                this.types = value;
            }
        }

        /// <summary>
        /// Gets or sets the discovery provider scopes
        /// </summary>
        public string Scopes
        {
            get
            {
                return this.scopes;
            }

            set
            {
                this.scopes = value;
            }
        }

        /// <summary>
        /// Gets or sets the transport URIs supported by the peer responding to the
        /// Probe message (with a ProbeMatch message)
        /// </summary>
        public string XAddrs
        {
            get
            {
                return this.xAddrs;
            }

            set
            {
                this.xAddrs = value;
            }
        }

        /// <summary>
        /// Gets or sets the current metadata version and MUST be set to 1
        /// </summary>
        public uint MetadataVersion
        {
            get
            {
                return this.metadataVersion;
            }

            set
            {
                this.metadataVersion = value;
            }
        }

        /// <summary>
        /// Gets or sets the block count
        /// </summary>
        public string BlockCount
        {
            get
            {
                return this.blockCount;
            }

            set
            {
                this.blockCount = value;
            }
        }
    }
}
