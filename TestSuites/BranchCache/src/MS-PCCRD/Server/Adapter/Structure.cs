// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pccrd
{
    /// <summary>
    /// Enum to record the status type
    /// </summary>
    public enum FaultType
    {
        /// <summary>
        /// The ok status of rececive pccrd message
        /// </summary>
        OK,

        /// <summary>
        /// The fault status of receive pccrd message
        /// </summary>
        SoapFault,
    }

    /// <summary>
    /// The structure represents the status of the message received.
    /// </summary>
    public struct Status
    {
        /// <summary>
        /// Record the types
        /// </summary>
        public FaultType FaultType;

        /// <summary>
        /// The return error code
        /// </summary>
        public string ErrorCode;
    }

    /// <summary>
    /// The response message match the ProbeMessage
    /// </summary>
    public class ProbeMatchMsg
    {
        /// <summary>
        /// The instance id of ProbeMessage
        /// </summary>
        private string instanceId;

        /// <summary>
        /// The message number of ProbeMessage
        /// </summary>
        private uint messageNumber;

        /// <summary>
        /// The peer content properties.
        /// </summary>
        private PeerProperties[] matches;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProbeMatchMsg"/> class
        /// </summary>
        /// <param name="instanceId">The instance id of the probe message</param>
        /// <param name="messageNumber">The number of the probe message</param>
        /// <param name="matches">The properties </param>
        public ProbeMatchMsg(string instanceId, uint messageNumber, PeerProperties[] matches)
        {
            this.instanceId = instanceId;
            this.messageNumber = messageNumber;
            this.matches = matches;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProbeMatchMsg"/> class without parameter
        /// </summary>
        public ProbeMatchMsg()
        {
            this.instanceId = string.Empty;
            this.messageNumber = 0;
            this.matches = null;
        }

        /// <summary>
        /// Gets or sets the instance id of ProbeMessage
        /// </summary>
        public string InstanceId
        {
            get
            {
                return this.instanceId;
            }

            set
            {
                this.instanceId = value;
            }
        }

        /// <summary>
        /// Gets or sets message number of ProbeMessage
        /// </summary>
        public uint MessageNumber
        {
            get
            {
                return this.messageNumber;
            }

            set
            {
                this.messageNumber = value;
            }
        }

        /// <summary>
        /// Gets or sets peer content properties.
        /// </summary>
        public PeerProperties[] Matches
        {
            get
            {
                return this.matches;
            }

            set
            {
                this.matches = value;
            }
        }
    }

    /// <summary>
    /// The Property of the discovery provider
    /// </summary>
    public class PeerProperties
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
        /// Initializes a new instance of the <see cref="PeerProperties"/> class
        /// </summary>
        /// <param name="address">The parameter identifies the responding device</param>
        /// <param name="types">The discovery provider types.</param>
        /// <param name="scopes">The discovery provider scopes</param>
        /// <param name="xAddrs">The transport URIs supported by the peer responding to the
        /// Probe message (with a ProbeMatch message)</param>
        /// <param name="metadataVersion">The current metadata version and MUST be set to 1</param>
        /// <param name="blockCount">The block count</param>
        public PeerProperties(
            string address,
            string types,
            string scopes,
            string xAddrs,
            uint metadataVersion,
            string blockCount)
        {
            this.address = address;
            this.types = types;
            this.scopes = scopes;
            this.xAddrs = xAddrs;
            this.metadataVersion = metadataVersion;
            this.blockCount = blockCount;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PeerProperties"/> class without parameter
        /// </summary>
        public PeerProperties()
        {
            this.address = null;
            this.types = null;
            this.scopes = null;
            this.xAddrs = null;
            this.metadataVersion = 0;
            this.blockCount = null;
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
