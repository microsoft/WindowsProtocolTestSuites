// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc
{
    /// <summary>
    /// NRPC client context.
    /// </summary>
    public class NrpcClientContext : NrpcContext
    {
        // Domain name
        private string domainName;

        // Primary name, usually equals to server name.
        private string primaryName;

        // Connection Status
        private NtStatus connectionStatus;

        // RPC transport context
        internal RpceClientContext rpceTransportContext;


        #region UTest support property
//Disable CS0649 because utcNow is never assigned a value in NRPC project.
//But we need this field when doing utest.
#pragma warning disable 649

        private DateTime? utcNow;
        internal DateTime UtcNow
        {
            get
            {
                return utcNow.GetValueOrDefault(DateTime.UtcNow);
            }
        }

#pragma warning restore 649
        #endregion


        /// <summary>
        /// Initialize a NRPC client context class.
        /// </summary>
        internal NrpcClientContext()
        {
        }


        /// <summary>
        /// For client machines, the NetBIOS name of the domain 
        /// to which the machine has been joined.
        /// </summary>
        public string DomainName
        {
            get
            {
                return domainName;
            }
            set
            {
                domainName = value;
            }
        }


        /// <summary>
        /// The PrimaryName (section 3.5.5.3.1) used by the client 
        /// during session-key negotiations (section 3.1.4.1).
        /// </summary>
        public string PrimaryName
        {
            get
            {
                return primaryName;
            }
            set
            {
                primaryName = value;
            }
        }


        /// <summary>
        /// A 4-byte value that contains the most recent connection status 
        /// return value (section 3.4.5.3.1) last returned during secure 
        /// channel establishment or by a method requiring session key 
        /// establishment (section 3.1.4.6).
        /// </summary>
        public NtStatus ConnectionStatus
        {
            get
            {
                return connectionStatus;
            }
            set
            {
                connectionStatus = value;
            }
        }


        /// <summary>
        /// Gets RPC transport context.
        /// </summary>
        public RpceClientContext RpceTransportContext
        {
            get
            {
                return rpceTransportContext;
            }
        }
    }
}
