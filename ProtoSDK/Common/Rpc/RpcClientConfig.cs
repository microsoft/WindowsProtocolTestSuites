// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// RpcClientConfig Contains the parameters used by WinAPI for RPC binding.
    /// </summary>
    public class RpcClientConfig
    {
        #region Variables

        private Guid? clientGuid;
        private String serverComputerName;
        private String protocolSequence;
        private String endPoint;
        private String networkOptions;
        private String servicePrincipalName;
        private RPC_C_AUTHN_LEVEL authenticationLevel;
        private RPC_C_AUTHN authenticationService;
        private SEC_WINNT_AUTH_IDENTITY authenticationIdentity;
        private RPC_C_AUTHN authorizationService;

        #endregion


        #region Properties

        /// <summary>
        /// an object UUID which represents the RPC caller. 
        /// It is used in RpcStringBindingCompose.
        /// </summary>
        public Guid? ClientGuid
        {
            get
            {
                return this.clientGuid;
            }
        }


        /// <summary>
        /// the NetworkingAddress of the RPC server.
        /// It is used in RpcStringBindingCompose.
        /// </summary>
        public String ServerComputerName
        {
            get
            {
                return this.serverComputerName;
            }
        }


        /// <summary>
        /// a string representation of a protocol sequence.
        /// It is used in RpcStringBindingCompose.
        /// </summary>
        public String ProtocolSequence
        {
            get
            {
                return this.protocolSequence;
            }
        }


        /// <summary>
        /// a string representation of an endpoint. The endpoint format and content are associated with
        /// the protocol sequence. For example, the endpoint associated with the protocol sequence 
        /// ncacn_np is a pipe name in the format \pipe\pipename. 
        /// It is used in RpcStringBindingCompose.
        /// </summary>
        public String EndPoint
        {
            get
            {
                return this.endPoint;
            }
        }


        /// <summary>
        /// a string representation of network options. The option string is associated with the protocol sequence.
        /// It is used in RpcStringBindingCompose.
        /// </summary>
        public String NetworkOptions
        {
            get
            {
                return this.networkOptions;
            }
        }


        /// <summary>
        /// the expected principal name of the server referenced by Binding. The content 
        /// of the name and its syntax are defined by the authentication service in use. 
        /// It is used in RpcBindingSetAuthInfo.
        /// </summary>
        public String ServicePrincipalName
        {
            get
            {
                return this.servicePrincipalName;
            }
        }


        /// <summary>
        /// Level of authentication to be performed on remote procedure calls made using
        /// Binding. For a list of the RPC-supported authentication levels.
        /// It is used in RpcBindingSetAuthInfo.
        /// </summary>
        public RPC_C_AUTHN_LEVEL AuthenticationLevel
        {
            get
            {
                return this.authenticationLevel;
            }
        }


        /// <summary>
        /// Authentication service to use.
        /// It is used in RpcBindingSetAuthInfo.
        /// </summary>
        public RPC_C_AUTHN AuthenticationService
        {
            get
            {
                return this.authenticationService;
            }
        }


        /// <summary>
        /// Handle to the structure containing the client's authentication and authorization 
        /// credentials appropriate for the selected authentication and authorization service.
        /// It is used in RpcBindingSetAuthInfo.
        /// </summary>
        public SEC_WINNT_AUTH_IDENTITY AuthenticationIdentity
        {
            get
            {
                return this.authenticationIdentity;
            }
        }


        /// <summary>
        /// Authorization service implemented by the server for the interface of interest.
        /// It is used in RpcBindingSetAuthInfo.
        /// </summary>
        public RPC_C_AUTHN AuthorizationService
        {
            get
            {
                return this.authorizationService;
            }
        }


        #endregion


        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="clientGuid">an object UUID which represents the RPC caller.</param>
        /// <param name="serverComputerName">the NetworkingAddress of the RPC server.</param>
        /// <param name="protocolSequence">a string representation of a protocol sequence.</param>
        /// <param name="endPoint">a string representation of an endpoint.</param>
        /// <param name="networkOptions">a string representation of network options. The option string is associated
        /// with the protocol sequence.</param>
        /// <param name="servicePrincipalName">the expected principal name of the server referenced by Binding.</param>
        /// <param name="authenticationLevel">Level of authentication to be performed on remote procedure calls
        /// made using Binding.</param>
        /// <param name="authenticationService">Authentication service to use.</param>
        /// <param name="authenticationIdentity">the structure containing the client's authentication and 
        /// authorization credentials appropriate for the selected authentication and authorization service.</param>
        /// <param name="authorizationService">Authorization service implemented by the server for the interface of
        /// interest.</param>
        public RpcClientConfig(
            Guid? clientGuid,
            String serverComputerName,
            String protocolSequence,
            String endPoint,
            String networkOptions,
            String servicePrincipalName,
            RPC_C_AUTHN_LEVEL authenticationLevel,
            RPC_C_AUTHN authenticationService,
            SEC_WINNT_AUTH_IDENTITY authenticationIdentity,
            RPC_C_AUTHN authorizationService)
        {
            this.clientGuid = clientGuid;
            this.serverComputerName = serverComputerName;
            this.protocolSequence = protocolSequence;
            this.endPoint = endPoint;
            this.networkOptions = networkOptions;
            this.servicePrincipalName = servicePrincipalName;
            this.authenticationLevel = authenticationLevel;
            this.authenticationService = authenticationService;
            this.authenticationIdentity = authenticationIdentity;
            this.authorizationService = authorizationService;
        }

        #endregion
    }
}
