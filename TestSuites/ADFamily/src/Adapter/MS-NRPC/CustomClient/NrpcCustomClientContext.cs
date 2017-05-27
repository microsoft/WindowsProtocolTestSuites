// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    using Microsoft.Protocols.TestTools.StackSdk;
    using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
    using Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc;

    /// <summary>
    /// NRPC client context.
    /// </summary>
    public class NrpcCustomClientContext : NrpcContext
    {
        /// <summary>
        /// RPC transport context.
        /// </summary>
        private RpceClientContext rpceTransportContext;

        /// <summary>
        /// Domain name.
        /// </summary>
        private string domainName;

        /// <summary>
        /// Primary name, usually equals to server name.
        /// </summary>
        private string primaryName;

        /// <summary>
        /// Connection Status.
        /// </summary>
        private NtStatus connectionStatus;

        /// <summary>
        /// Initializes a new instance of the NrpcCustomClientContext class.
        /// </summary>
        internal NrpcCustomClientContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the NrpcCustomClientContext class.
        /// </summary>
        /// <param name="context">Nrpc client context.</param>
        internal NrpcCustomClientContext(NrpcClientContext context)
        {
            this.domainName = context.DomainName;
            this.connectionStatus = context.ConnectionStatus;
            this.primaryName = context.PrimaryName;
            this.rpceTransportContext = context.RpceTransportContext;
        }

        /// <summary>
        /// Gets or sets the NetBIOS name of the domain 
        /// to which the machine has been joined For client machines,
        /// </summary>
        public string DomainName
        {
            get
            {
                return this.domainName;
            }

            set
            {
                this.domainName = value;
            }
        }

        /// <summary>
        /// Gets or sets the PrimaryName (section 3.5.5.3.1) used by the client 
        /// during session-key negotiations (section 3.1.4.1).
        /// </summary>
        public string PrimaryName
        {
            get
            {
                return this.primaryName;
            }

            set
            {
                this.primaryName = value;
            }
        }

        /// <summary>
        /// Gets or sets a 4-byte value that contains the most recent connection status 
        /// return value (section 3.4.5.3.1) last returned during secure 
        /// channel establishment or by a method requiring session key 
        /// establishment (section 3.1.4.6).
        /// </summary>
        public NtStatus ConnectionStatus
        {
            get
            {
                return this.connectionStatus;
            }

            set
            {
                this.connectionStatus = value;
            }
        }

        /// <summary>
        /// Gets or sets RPC transport context.
        /// </summary>
        public RpceClientContext RpceTransportContext
        {
            get
            {
                return this.rpceTransportContext;
            }

            set
            {
                this.rpceTransportContext = value;
            }
        }
    }
}