// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// RPCE context shared between client and server session.
    /// </summary>
    public abstract class RpceContext
    {
        // protocol sequence
        private string protocolSequence;

        // RPC version major, minor
        private byte rpcVersionMajor = RpceUtility.DEFAULT_RPC_VERSION_MAJOR;
        private byte rpcVersionMinor = RpceUtility.DEFAULT_RPC_VERSION_MINOR;

        // Supports header sign
        private bool supportsHeaderSign;

        // Supports concurrent multiplexing
        private bool supportsConcurrentMultiplexing;

        // NDR data representation format
        private RpceDataRepresentationFormat packedDataRepresentationFormat;

        // Current call id
        private uint currentCallId = 1;

        // Max transmit/receive fragment size, in bytes.
        private ushort maxTransmitFragmentSize = RpceUtility.DEFAULT_MAX_TRANSMIT_FRAGMENT_SIZE;
        private ushort maxReceiveFragmentSize = RpceUtility.DEFAULT_MAX_RECEIVE_FRAGMENT_SIZE;

        // Associate group id.
        private uint associateGroupId;

        // Interface identifier UUID, version major and version minor.
        private Guid interfaceId;
        private ushort interfaceMajorVersion;
        private ushort interfaceMinorVersion;

        // NDR version.
        private RpceNdrVersion ndrVersion = RpceUtility.DEFAULT_NDR_VERSION;

        // Bind time feature negotiation bitmask
        private RpceBindTimeFeatureNegotiationBitmask bindTimeFeatureNegotiationBitmask;

        // The p_cont_id field holds a presentation context identifier 
        // that identifies the data representation.
        private ushort contextIdentifier;

        // Secondary address
        private string secondaryAddress;

        // Table of Presentation Contexts. 
        // This is a table indexed by the presentation context ID 
        // (which is the same as the value of the p_cont_id field 
        // in the request PDU header).
        private SortedList<ushort, RpceNdrVersion> presentationContextsTable = 
            new SortedList<ushort, RpceNdrVersion>();

        // Authentication type, level, context id and security context.
        private RpceAuthenticationType authenticationType;
        private RpceAuthenticationLevel authenticationLevel;
        private uint authenticationContextId;
        private SecurityContext securityContext;
        private bool securityContextNeedContinueProcessing;


        /// <summary>
        /// constructor
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1805:DoNotInitializeUnnecessarily")]
        protected RpceContext()
        {
            // rpcVersionMinor will throw fxcop warning, 
            // we wrote this empty ctor and added attribute to suppress the rule.
        }


        /// <summary>
        /// Protocol sequence.
        /// </summary>
        public string ProtocolSequence
        {
            get
            {
                return protocolSequence;
            }
            internal set
            {
                protocolSequence = value;
            }
        }


        /// <summary>
        /// PRC version major
        /// </summary>
        public byte RpcVersionMajor
        {
            get
            {
                return rpcVersionMajor;
            }
            set
            {
                rpcVersionMajor = value;
            }
        }


        /// <summary>
        /// RPC version minor
        /// </summary>
        public byte RpcVersionMinor
        {
            get
            {
                return rpcVersionMinor;
            }
            set
            {
                rpcVersionMinor = value;
            }
        }


        /// <summary>
        /// Supports header sign
        /// </summary>
        public bool SupportsHeaderSign
        {
            get
            {
                return supportsHeaderSign;
            }
            set
            {
                supportsHeaderSign = value;
            }
        }


        /// <summary>
        /// Supports concurrent multiplexing
        /// </summary>
        public bool SupportsConcurrentMultiplexing
        {
            get
            {
                return supportsConcurrentMultiplexing;
            }
            set
            {
                supportsConcurrentMultiplexing = value;
            }
        }

        
        /// <summary>
        /// NDR data representation format.
        /// </summary>
        public RpceDataRepresentationFormat PackedDataRepresentationFormat
        {
            get
            {
                return packedDataRepresentationFormat;
            }
            set
            {
                packedDataRepresentationFormat = value;
            }
        }


        /// <summary>
        /// Current call id.<para/>
        /// To get a valid call id for next call, you can look at outstanding calls table, 
        /// if current call id is not present in outstanding calls table and greater than 
        /// any call id in outstanding calls table, you can use current call id.
        /// Otherwise, you should find a call id being greater than current call id and 
        /// any call id in outstanding calls table.
        /// </summary>
        public uint CurrentCallId
        {
            get
            {
                return currentCallId;
            }
            set
            {
                currentCallId = value;
            }
        }


        /// <summary>
        /// Max transmit fragment size, in bytes.
        /// </summary>
        public ushort MaxTransmitFragmentSize
        {
            get
            {
                return maxTransmitFragmentSize;
            }
            set
            {
                maxTransmitFragmentSize = value;
            }
        }


        /// <summary>
        /// Max receive fragment size, in bytes.
        /// </summary>
        public ushort MaxReceiveFragmentSize
        {
            get
            {
                return maxReceiveFragmentSize;
            }
            set
            {
                maxReceiveFragmentSize = value;
            }
        }


        /// <summary>
        /// Associate group id.
        /// </summary>
        public uint AssociateGroupId
        {
            get
            {
                return associateGroupId;
            }
            set
            {
                associateGroupId = value;
            }
        }


        /// <summary>
        /// Interface identifier UUID.
        /// </summary>
        public Guid InterfaceId
        {
            get
            {
                return interfaceId;
            }
            set
            {
                interfaceId = value;
            }
        }


        /// <summary>
        /// Interface identifier version major.
        /// </summary>
        public ushort InterfaceMajorVersion
        {
            get
            {
                return interfaceMajorVersion;
            }
            set
            {
                interfaceMajorVersion = value;
            }
        }


        /// <summary>
        /// Interface identifier version minor.
        /// </summary>
        public ushort InterfaceMinorVersion
        {
            get
            {
                return interfaceMinorVersion;
            }
            set
            {
                interfaceMinorVersion = value;
            }
        }


        /// <summary>
        /// NDR version.
        /// </summary>
        public RpceNdrVersion NdrVersion
        {
            get
            {
                return ndrVersion;
            }
            set
            {
                ndrVersion = value;
            }
        }


        /// <summary>
        /// Bind time feature negotiation bitmask
        /// </summary>
        public RpceBindTimeFeatureNegotiationBitmask BindTimeFeatureNegotiationBitmask
        {
            get
            {
                return bindTimeFeatureNegotiationBitmask;
            }
            set
            {
                bindTimeFeatureNegotiationBitmask = value;
            }
        }


        /// <summary>
        /// The p_cont_id field holds a presentation context identifier 
        /// that identifies the data representation.
        /// </summary>
        public ushort ContextIdentifier
        {
            get
            {
                return contextIdentifier;
            }
            set
            {
                contextIdentifier = value;
            }
        }


        /// <summary>
        /// Secondary address
        /// </summary>
        public string SecondaryAddress
        {
            get
            {
                return secondaryAddress;
            }
            set
            {
                secondaryAddress = value;
            }
        }


        /// <summary>
        /// Table of Presentation Contexts. 
        /// This is a table indexed by the presentation context ID 
        /// (which is the same as the value of the p_cont_id field 
        /// in the request PDU header).
        /// </summary>
        public SortedList<ushort, RpceNdrVersion> PresentationContextsTable
        {
            get
            {
                return presentationContextsTable;
            }
        }


        /// <summary>
        /// Authentication type.
        /// </summary>
        public RpceAuthenticationType AuthenticationType
        {
            get
            {
                return authenticationType;
            }
            set
            {
                authenticationType = value;
            }
        }


        /// <summary>
        /// Authentication level.
        /// </summary>
        public RpceAuthenticationLevel AuthenticationLevel
        {
            get
            {
                return authenticationLevel;
            }
            set
            {
                authenticationLevel = value;
            }
        }


        /// <summary>
        /// Authentication context id
        /// </summary>
        public uint AuthenticationContextId
        {
            get
            {
                return authenticationContextId;
            }
            set
            {
                authenticationContextId = value;
            }
        }


        /// <summary>
        /// Auentication security context.
        /// </summary>
        public SecurityContext SecurityContext
        {
            get
            {
                return securityContext;
            }
            set
            {
                securityContext = value;
            }
        }


        /// <summary>
        /// Everytime initialized or accepted a security token, 
        /// this will indicates whether need continue processing or not.
        /// It is usually equals to SecurityContext.NeedContinueProcessing, 
        /// but in some situations (like server-side), this value is more 
        /// reliable because it will not be overridden by multi-thread.
        /// </summary>
        public bool SecurityContextNeedContinueProcessing
        {
            get
            {
                return securityContextNeedContinueProcessing;
            }
            internal set
            {
                securityContextNeedContinueProcessing = value;
            }
        }
    }
}
