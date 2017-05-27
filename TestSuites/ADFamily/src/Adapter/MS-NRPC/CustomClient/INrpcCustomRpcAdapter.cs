// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    using Microsoft.Protocols.TestTools.StackSdk;
    using Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc;

    /// <summary>
    /// NRPC RPC interface.
    /// </summary>
    public partial interface INrpcCustomRpcAdapter : INrpcRpcAdapter
    {
        /// <summary>
        ///  The NetrLogonControl2Ex method executes windows-specific
        ///  administrative actions that pertain to the Netlogon
        ///  server operation. It is used to query the status and
        ///  control the actions of the Netlogon server. Opnum :
        ///  18 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle, as specified in the MS-NRPC.
        /// </param>
        /// <param name="functionCode">
        ///  The control operation to be performed; MUST be one of
        ///  the following values. The following restrictions apply
        ///  to the values of the FunctionCode parameter in windows_nt_4_0,
        ///  windows_2000, windows_7, and windows_server_7. There
        ///  are no restrictions in windows_server_2003, windows_vista,
        ///  and windows_server_2008. The following values are not
        ///  supported on windows_nt_4_0:NETLOGON_CONTROL_CHANGE_PASSWORD
        ///  (0x00000009)NETLOGON_CONTROL_TC_VERIFY (0x0000000A)NETLOGON_CONTROL_FORCE_DNS_REG
        ///  (0x0000000B)NETLOGON_CONTROL_QUERY_DNS_REG (0x0000000C)NETLOGON_CONTROL_BACKUP_CHANGE_LOG
        ///  (0x0000FFFC)NETLOGON_CONTROL_TRUNCATE_LOG (0x0000FFFD)NETLOGON_CONTROL_SET_DBFLAG
        ///  (0x0000FFFE)NETLOGON_CONTROL_BREAKPOINT (0x0000FFFF)The
        ///  error ERROR_NOT_SUPPORTED is returned if one of these
        ///  values is used. The following values are not supported
        ///  on windows_2000_server:NETLOGON_CONTROL_TC_VERIFY (0x0000000A)NETLOGON_CONTROL_FORCE_DNS_REG
        ///  (0x0000000B)NETLOGON_CONTROL_QUERY_DNS_REG (0x0000000C)The
        ///  error ERROR_NOT_SUPPORTED is returned if one of these
        ///  values is used. The following values are not supported
        ///  on windows_7 or windows_server_7:NETLOGON_CONTROL_REPLICATE
        ///  (0x00000002)NETLOGON_CONTROL_SYNCHRONIZE (0x00000003)NETLOGON_CONTROL_PDC_REPLICATE
        ///  (0x00000004)NETLOGON_CONTROL_BACKUP_CHANGE_LOG (0x0000FFFC)The
        ///  error ERROR_NOT_SUPPORTED is returned if one of these
        ///  values is used.
        /// </param>
        /// <param name="queryLevel">
        ///  Information query level requested by the client. The
        ///  buffer returned in the buffer parameter contains one
        ///  of the following structures, based on the value of
        ///  this field.
        /// </param>
        /// <param name="data">
        ///  NETLOGON_CONTROL_DATA_INFORMATION structure, as specified
        ///  in the MS-NRPC, that contains specific data required by
        ///  the query.
        /// </param>
        /// <param name="buffer">
        ///  NETLOGON_CONTROL_QUERY_INFORMATION structure, as specified
        ///  in the MS-NRPC, that contains the specific query results,
        ///  with a level of verbosity as specified in queryLevel.
        /// </param>
        /// <returns> return type NetApiStatus
        /// </returns>
        NetApiStatus NetrLogonControl2Ex(
            string serverName,
            uint functionCode,
            uint queryLevel,
            ////switch_is(FunctionCode)
            _NETLOGON_CONTROL_DATA_INFORMATION? data,
            ////switch_is(QueryLevel)
            out _NETLOGON_CONTROL_QUERY_INFORMATION? buffer);

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
        NtStatus NetrLogonSamLogoff(
            string logonServer,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            ref _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            _NETLOGON_LOGON_INFO_CLASS logonLevel,
            _NETLOGON_LEVEL? logonInformation,
            bool isLogonInfoNull);
    }
}