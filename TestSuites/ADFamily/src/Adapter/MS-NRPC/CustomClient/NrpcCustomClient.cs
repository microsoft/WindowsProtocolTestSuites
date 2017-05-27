// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using Microsoft.Protocols.TestTools.StackSdk;
    using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
    using Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc;
    using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

    /// <summary>
    /// Nrpc Client.
    /// </summary>
    public class NrpcCustomClient : IDisposable
    {
        /// <summary>
        /// Client context.
        /// </summary>
        private NrpcCustomClientContext context;

        /// <summary>
        /// Actual rpc adapter.
        /// </summary>
        private INrpcCustomRpcAdapter customRpc;

        /// <summary>
        /// Client security context.
        /// </summary>
        private NrpcCustomClientSecurityContext nrpcSecurityContext;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the NrpcCustomClient class.
        /// </summary>
        /// <param name="domainName">Domain name.</param>
        public NrpcCustomClient(string domainName)
        {
            this.context = new NrpcCustomClientContext();
            this.customRpc = new NrpcCustomRpcAdapter();
            this.context.DomainName = domainName;
        }

        #endregion

        /// <summary>
        /// Finalizes an instance of the NrpcCustomClient class.
        /// </summary>
        ~NrpcCustomClient()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets Client context.
        /// </summary>
        public NrpcCustomClientContext Context
        {
            get
            {
                return this.context;
            }
        }

        /// <summary>
        /// Gets the RPC handle.
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                return this.customRpc.Handle;
            }
        }

        /// <summary>
        /// Dispose function.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #region RPC bind methods

        /// <summary>
        /// RPC bind over named pipe, using well-known endpoint "\PIPE\NETLOGON".
        /// </summary>
        /// <param name="serverName">NRPC server machine name.</param>
        /// <param name="transportCredential">
        /// If connect by SMB/SMB2, it's the security credential 
        /// used by underlayer transport (SMB/SMB2). 
        /// If connect by TCP, this parameter is ignored.
        /// </param>
        /// <param name="securityContext">
        /// Security provider for RPC. 
        /// Set the value to null to disable authentication.
        /// </param>
        /// <param name="timeout">Timeout for bind and all future requests.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when serverName is null.
        /// </exception>
        public void BindOverNamedPipe(
            string serverName,
            AccountCredential transportCredential,
            ClientSecurityContext securityContext,
            TimeSpan timeout)
        {
            if (serverName == null)
            {
                throw new ArgumentNullException("serverName");
            }

            this.nrpcSecurityContext = securityContext as NrpcCustomClientSecurityContext;
            if (this.nrpcSecurityContext != null)
            {
                this.context = new NrpcCustomClientContext(this.nrpcSecurityContext.Context);
            }

            RpceAuthenticationLevel level = securityContext == null ? RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_NONE
                    : (this.context.SealSecureChannel
                        ? RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_PKT_PRIVACY
                        : RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_PKT_INTEGRITY);
            this.customRpc.Bind(
                RpceUtility.RPC_OVER_NAMED_PIPE_PROTOCOL_SEQUENCE,
                serverName,
                NrpcUtility.NETLOGON_RPC_OVER_NP_WELLKNOWN_ENDPOINT,
                transportCredential,
                securityContext,
                level,
                timeout);

            this.context.PrimaryName = serverName;
            NrpcCustomRpcAdapter nrpcRpcAdapter = this.customRpc as NrpcCustomRpcAdapter;
            if (nrpcRpcAdapter != null)
            {
                this.context.RpceTransportContext = nrpcRpcAdapter.RpceClientTransport.Context;
            }
        }

        /// <summary>
        /// RPC bind over TCP/IP, using specified endpoint and authenticate provider.
        /// </summary>
        /// <param name="serverName">NRPC server machine name.</param>
        /// <param name="endpoint">RPC endpoints, it's the port on TCP/IP.</param>
        /// <param name="securityContext">
        /// Security provider for RPC. 
        /// Set the value to null to disable authentication.
        /// </param>
        /// <param name="timeout">Timeout for bind and all future requests.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when serverName is null.
        /// </exception>
        public void BindOverTcp(
            string serverName,
            ushort endpoint,
            ClientSecurityContext securityContext,
            TimeSpan timeout)
        {
            if (serverName == null)
            {
                throw new ArgumentNullException("serverName");
            }

            this.nrpcSecurityContext = securityContext as NrpcCustomClientSecurityContext;
            if (this.nrpcSecurityContext != null)
            {
                this.context = new NrpcCustomClientContext(this.nrpcSecurityContext.Context);
            }

            RpceAuthenticationLevel level = securityContext == null
                    ? RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_NONE
                    : (this.context.SealSecureChannel
                        ? RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_PKT_PRIVACY
                        : RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_PKT_INTEGRITY);
            this.customRpc.Bind(
                RpceUtility.RPC_OVER_TCPIP_PROTOCOL_SEQUENCE,
                serverName,
                endpoint.ToString(CultureInfo.InvariantCulture),
                null,
                securityContext,
                level,
                timeout);

            this.context.PrimaryName = serverName;
            NrpcCustomRpcAdapter nrpcRpcAdapter = this.customRpc as NrpcCustomRpcAdapter;
            if (nrpcRpcAdapter != null)
            {
                this.context.RpceTransportContext = nrpcRpcAdapter.RpceClientTransport.Context;
            }
        }

        #endregion

        /// <summary>
        ///  The NetrLogonControl2 method is a predecessor to the
        ///  NetrLogonControl2Ex method. 
        ///  All parameters of this method have the same meanings
        ///  as the identically named parameters of the NetrLogonControl2Ex
        ///  method. Opnum: 14 
        /// </summary>
        /// <param name="serverName">
        ///  ServerName parameter.
        /// </param>
        /// <param name="functionCode">
        ///  FunctionCode parameter.
        /// </param>
        /// <param name="queryLevel">
        ///  QueryLevel parameter.
        /// </param>
        /// <param name="data">
        ///  Data parameter.
        /// </param>
        /// <param name="buffer">
        ///  Buffer parameter.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NetApiStatus CustomNetrLogonControl2(
                        string serverName,
                        uint functionCode,
                        uint queryLevel,
                        _NETLOGON_CONTROL_DATA_INFORMATION? data,
                        out _NETLOGON_CONTROL_QUERY_INFORMATION? buffer)
        {
            return this.customRpc.NetrLogonControl2(
                serverName,
                functionCode,
                queryLevel,
                data,
                out buffer);
        }

        /// <summary>
        ///  The NetrLogonControl2Ex method executes windows-specific
        ///  administrative actions that pertain to the Netlogon
        ///  server operation. It is used to query the status and
        ///  control the actions of the Netlogon server. Opnum: 18 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="functionCode">
        ///  The control operation to be performed; MUST be one of
        ///  the following values. The following restrictions apply
        ///  to the values of the FunctionCode parameter in windows_nt_4_0,
        ///  windows_2000, windows_7, and windows_server_7. There
        ///  are no restrictions in windows_server_2003, windows_vista,
        ///  and windows_server_2008. The following values are not
        ///  supported on windows_nt_4_0: NETLOGON_CONTROL_CHANGE_PASSWORD
        ///  (0x00000009) NETLOGON_CONTROL_TC_VERIFY (0x0000000A) NETLOGON_CONTROL_FORCE_DNS_REG
        ///  (0x0000000B) NETLOGON_CONTROL_QUERY_DNS_REG (0x0000000C) NETLOGON_CONTROL_BACKUP_CHANGE_LOG
        ///  (0x0000FFFC) NETLOGON_CONTROL_TRUNCATE_LOG (0x0000FFFD) NETLOGON_CONTROL_SET_DBFLAG
        ///  (0x0000FFFE) NETLOGON_CONTROL_BREAKPOINT (0x0000FFFF).<para/>
        ///  The error ERROR_NOT_SUPPORTED is returned if one of these
        ///  values is used.<para/>
        ///  The following values are not supported
        ///  on windows_2000_server: NETLOGON_CONTROL_TC_VERIFY (0x0000000A) NETLOGON_CONTROL_FORCE_DNS_REG
        ///  (0x0000000B) NETLOGON_CONTROL_QUERY_DNS_REG (0x0000000C).<para/>
        ///  The error ERROR_NOT_SUPPORTED is returned if one of these
        ///  values is used.<para/>
        ///  The following values are not supported
        ///  on windows_7 or windows_server_7: NETLOGON_CONTROL_REPLICATE
        ///  (0x00000002) NETLOGON_CONTROL_SYNCHRONIZE (0x00000003) NETLOGON_CONTROL_PDC_REPLICATE
        ///  (0x00000004) NETLOGON_CONTROL_BACKUP_CHANGE_LOG (0x0000FFFC).<para/>
        ///  The error ERROR_NOT_SUPPORTED is returned if one of these
        ///  values is used.
        /// </param>
        /// <param name="queryLevel">
        ///  Information query level requested by the client. The
        ///  buffer returned in the Buffer parameter contains one
        ///  of the following structures, based on the value of
        ///  this field.
        /// </param>
        /// <param name="data">
        ///  NETLOGON_CONTROL_DATA_INFORMATION structure, 
        ///  that contains specific data required by
        ///  the query.
        /// </param>
        /// <param name="buffer">
        ///  NETLOGON_CONTROL_QUERY_INFORMATION structure, 
        ///  that contains the specific query results,
        ///  with a level of verbosity as specified in QueryLevel.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NetApiStatus CustomNetrLogonControl2Ex(
                        string serverName,
                        uint functionCode,
                        uint queryLevel,
                        _NETLOGON_CONTROL_DATA_INFORMATION? data,
                        out _NETLOGON_CONTROL_QUERY_INFORMATION? buffer)
        {
            return this.customRpc.NetrLogonControl2Ex(
                                serverName,
                                functionCode,
                                queryLevel,
                                data,
                                out buffer);
        }

        /// <summary>
        ///  The NetrLogonSamLogoff method handles logoff requests
        ///  for the SAM accounts. Opnum: 3 
        /// </summary>
        /// <param name="logonServer">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="computerName">
        ///  The Unicode string that contains the NetBIOS name of
        ///  the client computer calling this method.
        /// </param>
        /// <param name="authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, 
        ///  that contains the client authenticator.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, 
        ///  that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="logonLevel">
        ///  A NETLOGON_LOGON_INFO_CLASS structure, 
        ///  that identifies the type of logon information
        ///  in the LogonInformation union.
        /// </param>
        /// <param name="logonInformation">
        ///  A pointer to a NETLOGON_LEVEL structure, 
        ///  that describes the logon information.
        /// </param>
        /// <param name="isLogonInfoNull">
        ///  Indicate if the logonInformation field is NULL.
        ///  </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NtStatus NetrLogonSamLogoff(
            string logonServer,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            ref _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            _NETLOGON_LOGON_INFO_CLASS logonLevel,
            _NETLOGON_LEVEL? logonInformation,
            bool isLogonInfoNull)
        {
            NtStatus status = this.customRpc.NetrLogonSamLogoff(
                logonServer,
                computerName,
                authenticator,
                ref returnAuthenticator,
                logonLevel,
                logonInformation,
                isLogonInfoNull);

            this.context.ConnectionStatus = status;
            return status;
        }

        /// <summary>
        /// The NetrLogonGetTimeServiceParentDomain method is supported
        ///  in windows_2000_server, windows_xp and windows_server_2003.
        ///  returns the name of the parent domain of the current
        ///  domain. The domain name returned by this method is
        ///  suitable for passing into the NetrLogonGetTrustRid
        ///  method and NetrLogonComputeClientDigest method. Opnum: 35 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="domainName">
        ///  A pointer to the buffer that receives the null-terminated
        ///  Unicode string that contains the name of the parent
        ///  domain. If the DNSdomain name is available, it is returned
        ///  through this parameter; otherwise, the NetBIOS domain
        ///  name is returned.
        /// </param>
        /// <param name="pdcSameSite">
        ///  A pointer to the buffer that receives the value that
        ///  indicates whether the PDC for the domainDomainName
        ///  is in the same site as the server specified by ServerName.
        ///  This value SHOULD be ignored
        ///  if ServerName is not a domain controller.
        /// </param>
        /// <returns>
        /// The method returns 0x00000000 on success; 
        /// otherwise, it returns a nonzero error code.
        /// </returns>
        public NetApiStatus NetrLogonGetTimeServiceParentDomain(
                 string serverName,
                 out string domainName,
                 out PdcSameSite_Values? pdcSameSite)
        {
            return this.customRpc.NetrLogonGetTimeServiceParentDomain(
                serverName,
                out domainName,
                out pdcSameSite);
        }

        /// <summary>
        /// Dispose function
        /// </summary>
        /// <param name="disposing">
        /// True to release both managed and unmanaged resources.
        /// False to release unmanaged resources only.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "DoNotCatchGeneralExceptionTypes")]
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // release managed resources.
                if (null != this.customRpc)
                {
                    try
                    {
                        this.customRpc.Unbind();
                    }
                    catch
                    {
                        // ignore the exception thrown by Unbind method.
                    }

                    this.customRpc.Dispose();
                    this.customRpc = null;
                }
            }

            // release unmanaged resources.
        }
    }
}
