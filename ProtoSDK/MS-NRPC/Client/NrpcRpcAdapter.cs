// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using System.Security;

using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc
{
    /// <summary>
    /// The implmentation of INrpcRpcAdapter
    /// </summary>
    internal class NrpcRpcAdapter : INrpcRpcAdapter, IDisposable
    {
        // RPCE client transport
        internal RpceClientTransport rpceClientTransport;

        // Timeout for RPC bind/call
        private TimeSpan rpceTimeout;


        #region INrpcRpcAdapter Members

        /// <summary>
        /// Bind to NRPC RPC server.
        /// </summary>
        /// <param name="protocolSequence">
        /// RPC protocol sequence.
        /// </param>
        /// <param name="networkAddress">
        /// RPC network address.
        /// </param>
        /// <param name="endpoint">
        /// RPC endpoint.
        /// </param>
        /// <param name="transportCredential">
        /// If connect by SMB/SMB2, it's the security credential 
        /// used by under layer transport (SMB/SMB2). 
        /// If connect by TCP, this parameter is ignored.
        /// </param>
        /// <param name="securityContext">
        /// RPC security provider.
        /// </param>
        /// <param name="authenticationLevel">
        /// RPC authentication level.
        /// </param>
        /// <param name="timeout">
        /// Timeout
        /// </param>
        public void Bind(
            string protocolSequence,
            string networkAddress,
            string endpoint,
            AccountCredential transportCredential,
            ClientSecurityContext securityContext,
            RpceAuthenticationLevel authenticationLevel,
            TimeSpan timeout)
        {
            if (rpceClientTransport != null)
            {
                throw new InvalidOperationException("NRPC has already been bind.");
            }

            rpceTimeout = timeout;

            rpceClientTransport = new RpceClientTransport();

            rpceClientTransport.Bind(
                protocolSequence,
                networkAddress,
                endpoint,
                transportCredential,
                NrpcUtility.NETLOGON_RPC_INTERFACE_UUID,
                NrpcUtility.NETLOGON_RPC_INTERFACE_MAJOR_VERSION,
                NrpcUtility.NETLOGON_RPC_INTERFACE_MINOR_VERSION,
                securityContext,
                null,
                authenticationLevel,
                false,
                rpceTimeout);
        }


        /// <summary>
        /// RPC unbind.
        /// </summary>
        public void Unbind()
        {
            if (rpceClientTransport != null)
            {
                rpceClientTransport.Unbind(rpceTimeout);
                rpceClientTransport = null;
            }
        }


        /// <summary>
        /// RPC handle.
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                return rpceClientTransport.Handle;
            }
        }


        /// <summary>
        ///  NetrLogonUasLogon IDL method. Opnum: 0 
        /// </summary>
        /// <param name="ServerName">
        ///  ServerName parameter.
        /// </param>
        /// <param name="UserName">
        ///  UserName parameter.
        /// </param>
        /// <param name="Workstation">
        ///  Workstation parameter.
        /// </param>
        /// <param name="ValidationInformation">
        ///  ValidationInformation parameter.
        /// </param>
        public NetApiStatus NetrLogonUasLogon(
            string ServerName,
            string UserName,
            string Workstation,
            out _NETLOGON_VALIDATION_UAS_INFO? ValidationInformation)
        {
            const ushort opnum = 0;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pServerName = Marshal.StringToHGlobalUni(ServerName);
            SafeIntPtr pUserName = Marshal.StringToHGlobalUni(UserName);
            SafeIntPtr pWorkstation = Marshal.StringToHGlobalUni(Workstation);

            paramList = new Int3264[] {
                pServerName,
                pUserName,
                pWorkstation,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pValidationInformation = outParamList[3];
                pValidationInformation = Marshal.ReadIntPtr(pValidationInformation);
                ValidationInformation = TypeMarshal.ToNullableStruct<_NETLOGON_VALIDATION_UAS_INFO>(pValidationInformation);

                retVal = outParamList[4].ToInt32();
            }

            pServerName.Dispose();
            pUserName.Dispose();
            pWorkstation.Dispose();

            return (NetApiStatus)retVal;
        }


        /// <summary>
        ///  NetrLogonUasLogoff IDL method. Opnum: 1 
        /// </summary>
        /// <param name="ServerName">
        ///  ServerName parameter.
        /// </param>
        /// <param name="UserName">
        ///  UserName parameter.
        /// </param>
        /// <param name="Workstation">
        ///  Workstation parameter.
        /// </param>
        /// <param name="LogoffInformation">
        ///  LogoffInformation parameter.
        /// </param>
        public NetApiStatus NetrLogonUasLogoff(
            string ServerName,
            string UserName,
            string Workstation,
            out _NETLOGON_LOGOFF_UAS_INFO? LogoffInformation)
        {
            const ushort opnum = 1;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pServerName = Marshal.StringToHGlobalUni(ServerName);
            SafeIntPtr pUserName = Marshal.StringToHGlobalUni(UserName);
            SafeIntPtr pWorkstation = Marshal.StringToHGlobalUni(Workstation);

            paramList = new Int3264[] {
                pServerName,
                pUserName,
                pWorkstation,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pValidationInformation = outParamList[3];
                LogoffInformation = TypeMarshal.ToNullableStruct<_NETLOGON_LOGOFF_UAS_INFO>(pValidationInformation);

                retVal = outParamList[4].ToInt32();
            }

            pServerName.Dispose();
            pUserName.Dispose();
            pWorkstation.Dispose();

            return (NetApiStatus)retVal;
        }


        /// <summary>
        ///  The NetrLogonSamLogon method This method was used in
        ///  windows_nt_4_0. It was superseded by the NetrLogonSamLogonWithFlags
        ///  method (section) in windows_2000_server, windows_xp,
        ///  windows_server_2003, windows_vista, and windows_server_2008,
        ///  windows_7, and windows_server_7. is a predecessor to
        ///  the NetrLogonSamLogonWithFlags method (section ). All
        ///  parameters of this method have the same meanings as
        ///  the identically named parameters of the NetrLogonSamLogonWithFlags
        ///  method. Opnum: 2 
        /// </summary>
        /// <param name="LogonServer">
        ///  LogonServer parameter.
        /// </param>
        /// <param name="ComputerName">
        ///  ComputerName parameter.
        /// </param>
        /// <param name="Authenticator">
        ///  Authenticator parameter.
        /// </param>
        /// <param name="ReturnAuthenticator">
        ///  ReturnAuthenticator parameter.
        /// </param>
        /// <param name="LogonLevel">
        ///  LogonLevel parameter.
        /// </param>
        /// <param name="LogonInformation">
        ///  LogonInformation parameter.
        /// </param>
        /// <param name="ValidationLevel">
        ///  ValidationLevel parameter.
        /// </param>
        /// <param name="ValidationInformation">
        ///  ValidationInformation parameter.
        /// </param>
        /// <param name="Authoritative">
        ///  Authoritative parameter.
        /// </param>
        public NtStatus NetrLogonSamLogon(
            string LogonServer,
            string ComputerName,
            _NETLOGON_AUTHENTICATOR? Authenticator,
            ref _NETLOGON_AUTHENTICATOR? ReturnAuthenticator,
            _NETLOGON_LOGON_INFO_CLASS LogonLevel,
            //switch_is(LogonLevel)
            _NETLOGON_LEVEL? LogonInformation,
            _NETLOGON_VALIDATION_INFO_CLASS ValidationLevel,
            //switch_is(ValidationLevel)
            out _NETLOGON_VALIDATION? ValidationInformation,
            out byte? Authoritative)
        {
            const ushort opnum = 2;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pLogonServer = Marshal.StringToHGlobalUni(LogonServer);
            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);
            SafeIntPtr pAuthenticator = TypeMarshal.ToIntPtr(Authenticator);
            SafeIntPtr pReturnAuthenticatorIn = TypeMarshal.ToIntPtr(ReturnAuthenticator);
            SafeIntPtr pLogonInformation = TypeMarshal.ToIntPtr(LogonInformation, LogonLevel, null, null);

            paramList = new Int3264[] {
                pLogonServer,
                pComputerName,
                pAuthenticator,
                pReturnAuthenticatorIn,
                (uint)LogonLevel,
                pLogonInformation,
                (uint)ValidationLevel,
                IntPtr.Zero,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pReturnAuthenticatorOut = outParamList[3];
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(pReturnAuthenticatorOut);

                IntPtr pValidationInformation = outParamList[7];
                ValidationInformation = TypeMarshal.ToNullableStruct<_NETLOGON_VALIDATION>(
                    pValidationInformation,
                    ValidationLevel,
                    null,
                    null);

                Authoritative = TypeMarshal.ToNullableStruct<byte>(outParamList[8]);

                retVal = outParamList[9].ToInt32();
            }

            pLogonServer.Dispose();
            pComputerName.Dispose();
            pAuthenticator.Dispose();
            pReturnAuthenticatorIn.Dispose();
            pLogonInformation.Dispose();

            return (NtStatus)retVal;
        }


        /// <summary>
        ///  The NetrLogonSamLogoff method handles logoff requests
        ///  for the SAM accounts. Opnum: 3 
        /// </summary>
        /// <param name="LogonServer">
        ///  The custom RPC binding handle, as specified in section
        ///  .
        /// </param>
        /// <param name="ComputerName">
        ///  The Unicode string that contains the NetBIOS name of
        ///  the client computer calling this method.
        /// </param>
        /// <param name="Authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the client authenticator.
        /// </param>
        /// <param name="ReturnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="LogonLevel">
        ///  A NETLOGON_LOGON_INFO_CLASS structure, as specified
        ///  in section , that identifies the type of logon information
        ///  in the LogonInformation union.
        /// </param>
        /// <param name="LogonInformation">
        ///  A pointer to a NETLOGON_LEVEL structure, as specified
        ///  in section , that describes the logon information.
        /// </param>
        public NtStatus NetrLogonSamLogoff(
            string LogonServer,
            string ComputerName,
            _NETLOGON_AUTHENTICATOR? Authenticator,
            ref _NETLOGON_AUTHENTICATOR? ReturnAuthenticator,
            _NETLOGON_LOGON_INFO_CLASS LogonLevel,
            _NETLOGON_LEVEL? LogonInformation)
        {
            const ushort opnum = 3;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pLogonServer = Marshal.StringToHGlobalUni(LogonServer);
            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);
            SafeIntPtr pAuthenticator = TypeMarshal.ToIntPtr(Authenticator);
            SafeIntPtr pReturnAuthenticatorIn = TypeMarshal.ToIntPtr(ReturnAuthenticator);
            SafeIntPtr pLogonInformation = TypeMarshal.ToIntPtr(LogonInformation, LogonLevel, null, null);

            paramList = new Int3264[] {
                pLogonServer,
                pComputerName,
                pAuthenticator,
                pReturnAuthenticatorIn,
                (uint)LogonLevel,
                pLogonInformation,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pReturnAuthenticatorOut = outParamList[3];
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(pReturnAuthenticatorOut);

                retVal = outParamList[6].ToInt32();
            }

            pLogonServer.Dispose();
            pComputerName.Dispose();
            pAuthenticator.Dispose();
            pReturnAuthenticatorIn.Dispose();
            pLogonInformation.Dispose();

            return (NtStatus)retVal;
        }


        /// <summary>
        ///  The NetrServerReqChallenge method receives a client
        ///  challenge and returns a server challenge. Opnum: 4
        ///  
        /// </summary>
        /// <param name="PrimaryName">
        ///  The custom RPC binding handle, as specified in section
        ///  .
        /// </param>
        /// <param name="ComputerName">
        ///  A Unicode string that contains the NetBIOS name of the
        ///  client computer calling this method.
        /// </param>
        /// <param name="ClientChallenge">
        ///  A pointer to a NETLOGON_CREDENTIAL structure, as specified
        ///  in section , that contains the client challenge.
        /// </param>
        /// <param name="ServerChallenge">
        ///  A pointer to a NETLOGON_CREDENTIAL structure, as specified
        ///  in section , that contains the server challenge response.
        /// </param>
        public NtStatus NetrServerReqChallenge(
            string PrimaryName,
            string ComputerName,
            _NETLOGON_CREDENTIAL? ClientChallenge,
            out _NETLOGON_CREDENTIAL? ServerChallenge)
        {
            const ushort opnum = 4;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pPrimaryName = Marshal.StringToHGlobalUni(PrimaryName);
            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);
            SafeIntPtr pClientChallenge = TypeMarshal.ToIntPtr(ClientChallenge);

            paramList = new Int3264[] {
                pPrimaryName,
                pComputerName,
                pClientChallenge,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pServerChallenge = outParamList[3];
                ServerChallenge = TypeMarshal.ToNullableStruct<_NETLOGON_CREDENTIAL>(pServerChallenge);

                retVal = outParamList[4].ToInt32();
            }

            pPrimaryName.Dispose();
            pComputerName.Dispose();
            pClientChallenge.Dispose();

            return (NtStatus)retVal;
        }


        /// <summary>
        ///  The NetrServerAuthenticate method This method was used
        ///  in windows_nt_server_3_1. In windows_nt_server_3_5,
        ///  it was superseded by the NetrServerAuthenticate2 method
        ///  (section ). In windows_2000_server, windows_xp, windows_server_2003,
        ///  windows_vista, windows_server_2008, windows_7, and
        ///  windows_server_7, the NetrServerAuthenticate2 method
        ///  (section) was superseded by the NetrServerAuthenticate3
        ///  method (section ). is a predecessor to the NetrServerAuthenticate3
        ///  method (section ). All parameters of this method have
        ///  the same meanings as the identically named parameters
        ///  of the NetrServerAuthenticate3 method. Opnum: 5 
        /// </summary>
        /// <param name="PrimaryName">
        ///  PrimaryName parameter.
        /// </param>
        /// <param name="AccountName">
        ///  AccountName parameter.
        /// </param>
        /// <param name="SecureChannelType">
        ///  SecureChannelType parameter.
        /// </param>
        /// <param name="ComputerName">
        ///  ComputerName parameter.
        /// </param>
        /// <param name="ClientCredential">
        ///  ClientCredential parameter.
        /// </param>
        /// <param name="ServerCredential">
        ///  ServerCredential parameter.
        /// </param>
        public NtStatus NetrServerAuthenticate(
            string PrimaryName,
            string AccountName,
            _NETLOGON_SECURE_CHANNEL_TYPE SecureChannelType,
            string ComputerName,
            _NETLOGON_CREDENTIAL? ClientCredential,
            out _NETLOGON_CREDENTIAL? ServerCredential)
        {
            const ushort opnum = 5;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pPrimaryName = Marshal.StringToHGlobalUni(PrimaryName);
            SafeIntPtr pAccountName = Marshal.StringToHGlobalUni(AccountName);
            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);
            SafeIntPtr pClientCredential = TypeMarshal.ToIntPtr(ClientCredential);

            paramList = new Int3264[] {
                pPrimaryName,
                pAccountName,
                (uint)SecureChannelType,
                pComputerName,
                pClientCredential,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pServerCredential = outParamList[5];
                ServerCredential = TypeMarshal.ToNullableStruct<_NETLOGON_CREDENTIAL>(pServerCredential);

                retVal = outParamList[6].ToInt32();
            }

            pPrimaryName.Dispose();
            pAccountName.Dispose();
            pComputerName.Dispose();
            pClientCredential.Dispose();

            return (NtStatus)retVal;
        }


        /// <summary>
        ///  The NetrServerPasswordSet method sets a new one-way
        ///  function (OWF) of a password for an account used by
        ///  the domain controller (as detailed in section) for
        ///  setting up the secure channel from the client. Opnum
        ///  : 6 
        /// </summary>
        /// <param name="PrimaryName">
        ///  The custom RPC binding handle, as specified in section
        ///  .
        /// </param>
        /// <param name="AccountName">
        ///  The null-terminated Unicode string that contains the
        ///  name of the account whose password is being changed. In
        ///  windows, all machine account names are the name of
        ///  the machine with a $ (dollar sign) appended.
        /// </param>
        /// <param name="SecureChannelType">
        ///  An enumerated value (specified in section) that indicates
        ///  the type of secure channel used by the client.
        /// </param>
        /// <param name="ComputerName">
        ///  A null-terminated Unicode string that contains the NetBIOS
        ///  name of the client computer calling this method.
        /// </param>
        /// <param name="Authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the client authenticator.
        /// </param>
        /// <param name="ReturnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="UasNewPassword">
        ///  A pointer to an ENCRYPTED_LM_OWF_PASSWORD structure,
        ///  as specified in section  and encrypted by the algorithm
        ///  specified in section.
        /// </param>
        public NtStatus NetrServerPasswordSet(
            string PrimaryName,
            string AccountName,
            _NETLOGON_SECURE_CHANNEL_TYPE SecureChannelType,
            string ComputerName,
            _NETLOGON_AUTHENTICATOR? Authenticator,
            out _NETLOGON_AUTHENTICATOR? ReturnAuthenticator,
            _LM_OWF_PASSWORD? UasNewPassword)
        {
            const ushort opnum = 6;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pPrimaryName = Marshal.StringToHGlobalUni(PrimaryName);
            SafeIntPtr pAccountName = Marshal.StringToHGlobalUni(AccountName);
            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);
            SafeIntPtr pAuthenticator = TypeMarshal.ToIntPtr(Authenticator);
            SafeIntPtr pUasNewPassword = TypeMarshal.ToIntPtr(UasNewPassword);

            paramList = new Int3264[] {
                pPrimaryName,
                pAccountName,
                (uint)SecureChannelType,
                pComputerName,
                pAuthenticator,
                IntPtr.Zero,
                pUasNewPassword,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pReturnAuthenticator = outParamList[5];
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(pReturnAuthenticator);

                retVal = outParamList[7].ToInt32();
            }

            pPrimaryName.Dispose();
            pAccountName.Dispose();
            pComputerName.Dispose();
            pAuthenticator.Dispose();
            pUasNewPassword.Dispose();

            return (NtStatus)retVal;
        }


        /// <summary>
        /// The NetrDatabaseDeltas method returns a set of changes (or deltas) 
        /// performed to the SAM, SAM built-in, or LSA databases after a particular 
        /// value of the database serial number. It is used by BDCs to request 
        /// database changes from the PDC that are missing on the BDC. Opnum: 7
        /// </summary>
        /// <param name="PrimaryName">
        /// The custom RPC binding handle (as specified in section 3.5.5.1) that 
        /// represents the connection to the PDC.
        /// </param>
        /// <param name="ComputerName">
        /// The null-terminated Unicode string that contains the NetBIOS name of 
        /// the BDC calling this method.
        /// </param>
        /// <param name="Authenticator">
        /// A pointer to a NETLOGON_AUTHENTICATOR structure that contains the 
        /// client authenticator.
        /// </param>
        /// <param name="ReturnAuthenticator">
        /// A pointer to a NETLOGON_AUTHENTICATOR structure that contains the 
        /// server return authenticator.
        /// </param>
        /// <param name="DatabaseID">
        /// The identifier for a specific account database set as follows: 
        /// 0x00000000
        /// Indicates the SAM database.
        /// 0x00000001
        /// Indicates the SAM built-in database.
        /// 0x00000002
        /// Indicates the LSA database.
        /// </param>
        /// <param name="DomainModifiedCount">
        /// A pointer to an NLPR_MODIFIED_COUNT structure, as specified in section 
        /// 2.2.1.5.26, that contains the database serial number. On input, this is 
        /// the value of the database serial number on the client. On output, this is 
        /// the value of the database serial number corresponding to the last element 
        /// (delta) returned in the DeltaArray parameter.
        /// </param>
        /// <param name="DeltaArray">
        /// A pointer to a NETLOGON_DELTA_ENUM_ARRAY structure that contains an array 
        /// of enumerated changes (deltas) to the specified database with database 
        /// serial numbers larger than the database serial number value specified in 
        /// the input value of the DomainModifiedCount parameter.
        /// </param>
        /// <param name="PreferredMaximumLength">
        /// The value that specifies the preferred maximum size, in bytes, of data to 
        /// return in the DeltaArray parameter. This is not a hard upper limit, but 
        /// serves as a guide to the server. The server SHOULD stop including 
        /// elements in the returned DeltaArray after the size of the returned data 
        /// equals or exceeds the value of the PreferredMaximumLength parameter. It is 
        /// up to the client implementation to choose the value for this parameter.
        /// </param>
        public NtStatus NetrDatabaseDeltas(
            string PrimaryName,
            string ComputerName,
            _NETLOGON_AUTHENTICATOR? Authenticator,
            ref _NETLOGON_AUTHENTICATOR? ReturnAuthenticator,
            DatabaseID_Values DatabaseID,
            ref _NLPR_MODIFIED_COUNT? DomainModifiedCount,
            out _NETLOGON_DELTA_ENUM_ARRAY? DeltaArray,
            uint PreferredMaximumLength)
        {
            const ushort opnum = 7;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pPrimaryName = Marshal.StringToHGlobalUni(PrimaryName);
            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);
            SafeIntPtr pAuthenticator = TypeMarshal.ToIntPtr(Authenticator);
            SafeIntPtr pReturnAuthenticatorIn = TypeMarshal.ToIntPtr(ReturnAuthenticator);
            SafeIntPtr pDomainModifiedCountIn = TypeMarshal.ToIntPtr(DomainModifiedCount);

            paramList = new Int3264[] {
                pPrimaryName,
                pComputerName,
                pAuthenticator,
                pReturnAuthenticatorIn,
                (uint)DatabaseID,
                pDomainModifiedCountIn,
                IntPtr.Zero,
                PreferredMaximumLength,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pReturnAuthenticatorOut = outParamList[3];
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(pReturnAuthenticatorOut);

                IntPtr pDomainModifiedCountOut = outParamList[5];
                DomainModifiedCount = TypeMarshal.ToNullableStruct<_NLPR_MODIFIED_COUNT>(pDomainModifiedCountOut);

                IntPtr pDeltaArray = outParamList[6];
                pDeltaArray = Marshal.ReadIntPtr(pDeltaArray);
                DeltaArray = TypeMarshal.ToNullableStruct<_NETLOGON_DELTA_ENUM_ARRAY>(pDeltaArray);

                retVal = outParamList[8].ToInt32();
            }

            pPrimaryName.Dispose();
            pComputerName.Dispose();
            pAuthenticator.Dispose();
            pReturnAuthenticatorIn.Dispose();
            pDomainModifiedCountIn.Dispose();

            return (NtStatus)retVal;
        }


        /// <summary>
        ///  The NetrDatabaseSync method is a predecessor to the NetrDatabaseSync2 method, 
        ///  as specified in section 3.5.5.5.2. All parameters of this method have the same 
        ///  meanings as the identically named parameters of the NetrDatabaseSync2 method. 
        ///  Opnum: 8
        /// </summary>
        /// <param name="PrimaryName">
        ///  The custom RPC binding handle, as specified in section
        ///  , representing the connection to the PDC.
        /// </param>
        /// <param name="ComputerName">
        ///  The null-terminated Unicode string that contains the
        ///  NetBIOS name of the BDC calling this method.
        /// </param>
        /// <param name="Authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the client authenticator.
        /// </param>
        /// <param name="ReturnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="DatabaseID">
        ///  The identifier for a specific database for which the
        ///  changes are requested. It MUST be one of the following
        ///  values.
        /// </param>
        /// <param name="SyncContext">
        ///  Specifies context needed to continue the operation.
        ///  The value MUST be set to zero on the first call. The
        ///  caller MUST treat this as an opaque value, unless this
        ///  call is a restart of the series of synchronization
        ///  calls. The value returned is to be used on input for
        ///  the next call in the series of synchronization calls. If
        ///  this call is the restart of the series, the values
        ///  of the RestartState and the SyncContext parameters
        ///  are dependent on the DeltaType value received on the
        ///  last call before the restart and MUST be set as follows.
        ///  Find the last NETLOGON_DELTA_ENUM structure in the
        ///  DeltaArray parameter of the call. The DeltaType field
        ///  of this NETLOGON_DELTA_ENUM structure, as specified
        ///  in section , is the DeltaType needed for the restart.
        ///  The values of RestartState and SyncContext are then
        ///  determined from the following table. DeltaTypeRestartStateSyncContextAddOrChangeGroupGroupStateThe
        ///  value of the RID field of the last element AddOrChangeUserUserStateThe
        ///  value of the RID field of the last element ChangeGroupMembershipGroupMemberStateThe
        ///  value of the RID field of the last element AddOrChangeAliasAliasState0x00000000ChangeAliasMembershipAliasMemberState0x00000000Any
        ///  other value not previously listedNormalState0x00000000
        /// </param>
        /// <param name="DeltaArray">
        ///  A pointer to a NETLOGON_DELTA_ENUM_ARRAY structure,
        ///  as specified in section , that contains an array of
        ///  enumerated changes (deltas) to the specified database.
        /// </param>
        /// <param name="PreferredMaximumLength">
        ///  The value that specifies the preferred maximum size,
        ///  in bytes, of data referenced in the DeltaArray parameter.
        ///  This is not a hard upper limit, but serves as a guide
        ///  to the server. The server SHOULDwindows stops including
        ///  elements in the returned DeltaArray once the size of
        ///  the returned data equals or exceeds the value of the
        ///  PreferredMaximumLength parameter. The server SHOULD stop including elements
        ///  in the returned DeltaArray once the size of the returned
        ///  data equals or exceeds the value of the PreferredMaximumLength
        ///  parameter. It is up to the client implementation to
        ///  choose the value for this parameter.
        /// </param>
        public NtStatus NetrDatabaseSync(
            string PrimaryName,
            string ComputerName,
            _NETLOGON_AUTHENTICATOR? Authenticator,
            ref _NETLOGON_AUTHENTICATOR? ReturnAuthenticator,
            DatabaseID_Values DatabaseID,
            ref uint? SyncContext,
            out _NETLOGON_DELTA_ENUM_ARRAY? DeltaArray,
            uint PreferredMaximumLength)
        {
            const ushort opnum = 8;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pPrimaryName = Marshal.StringToHGlobalUni(PrimaryName);
            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);
            SafeIntPtr pAuthenticator = TypeMarshal.ToIntPtr(Authenticator);
            SafeIntPtr pReturnAuthenticatorIn = TypeMarshal.ToIntPtr(ReturnAuthenticator);
            SafeIntPtr pSyncContext = TypeMarshal.ToIntPtr(SyncContext);

            paramList = new Int3264[] {
                pPrimaryName,
                pComputerName,
                pAuthenticator,
                pReturnAuthenticatorIn,
                (uint)DatabaseID,
                pSyncContext,
                IntPtr.Zero,
                PreferredMaximumLength,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pReturnAuthenticatorOut = outParamList[3];
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(pReturnAuthenticatorOut);

                SyncContext = TypeMarshal.ToNullableStruct<uint>(outParamList[5]);

                IntPtr pDeltaArray = outParamList[6];
                pDeltaArray = Marshal.ReadIntPtr(pDeltaArray);
                DeltaArray = TypeMarshal.ToNullableStruct<_NETLOGON_DELTA_ENUM_ARRAY>(pDeltaArray);

                retVal = outParamList[8].ToInt32();
            }

            pPrimaryName.Dispose();
            pComputerName.Dispose();
            pAuthenticator.Dispose();
            pReturnAuthenticatorIn.Dispose();
            pSyncContext.Dispose();

            return (NtStatus)retVal;
        }


        /// <summary>
        ///  NetrAccountDeltas IDL method. Opnum: 9 
        /// </summary>
        /// <param name="PrimaryName">
        ///  PrimaryName parameter.
        /// </param>
        /// <param name="ComputerName">
        ///  ComputerName parameter.
        /// </param>
        /// <param name="Authenticator">
        ///  Authenticator parameter.
        /// </param>
        /// <param name="ReturnAuthenticator">
        ///  ReturnAuthenticator parameter.
        /// </param>
        /// <param name="RecordID">
        ///  RecordID parameter.
        /// </param>
        /// <param name="Count">
        ///  Count parameter.
        /// </param>
        /// <param name="Level">
        ///  Level parameter.
        /// </param>
        /// <param name="Buffer">
        ///  Buffer parameter.
        /// </param>
        /// <param name="BufferSize">
        ///  BufferSize parameter.
        /// </param>
        /// <param name="CountReturned">
        ///  CountReturned parameter.
        /// </param>
        /// <param name="TotalEntries">
        ///  TotalEntries parameter.
        /// </param>
        /// <param name="NextRecordId">
        ///  NextRecordId parameter.
        /// </param>
        public NtStatus NetrAccountDeltas(
            string PrimaryName,
            string ComputerName,
            _NETLOGON_AUTHENTICATOR? Authenticator,
            ref _NETLOGON_AUTHENTICATOR? ReturnAuthenticator,
            _UAS_INFO_0? RecordID,
            uint Count,
            uint Level,
            out byte[] Buffer,
            uint BufferSize,
            out uint? CountReturned,
            out uint? TotalEntries,
            out _UAS_INFO_0? NextRecordId)
        {
            const ushort opnum = 9;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pPrimaryName = Marshal.StringToHGlobalUni(PrimaryName);
            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);
            SafeIntPtr pAuthenticator = TypeMarshal.ToIntPtr(Authenticator);
            SafeIntPtr pReturnAuthenticatorIn = TypeMarshal.ToIntPtr(ReturnAuthenticator);
            SafeIntPtr pRecordID = TypeMarshal.ToIntPtr(RecordID);

            paramList = new Int3264[] {
                pPrimaryName,
                pComputerName,
                pAuthenticator,
                pReturnAuthenticatorIn,
                pRecordID,
                Count,
                Level,
                IntPtr.Zero,
                BufferSize,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pReturnAuthenticatorOut = outParamList[3];
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(pReturnAuthenticatorOut);

                IntPtr pBuffer = outParamList[7];
                Buffer = IntPtrUtility.PtrToArray<byte>(pBuffer, BufferSize);

                CountReturned = TypeMarshal.ToNullableStruct<uint>(outParamList[9]);

                TotalEntries = TypeMarshal.ToNullableStruct<uint>(outParamList[10]);

                IntPtr pNextRecordId = outParamList[11];
                NextRecordId = TypeMarshal.ToNullableStruct<_UAS_INFO_0>(pNextRecordId);

                retVal = outParamList[12].ToInt32();
            }

            pPrimaryName.Dispose();
            pComputerName.Dispose();
            pAuthenticator.Dispose();
            pReturnAuthenticatorIn.Dispose();
            pRecordID.Dispose();

            return (NtStatus)retVal;
        }


        /// <summary>
        ///  NetrAccountSync IDL method. Opnum: 10 
        /// </summary>
        /// <param name="PrimaryName">
        ///  PrimaryName parameter.
        /// </param>
        /// <param name="ComputerName">
        ///  ComputerName parameter.
        /// </param>
        /// <param name="Authenticator">
        ///  Authenticator parameter.
        /// </param>
        /// <param name="ReturnAuthenticator">
        ///  ReturnAuthenticator parameter.
        /// </param>
        /// <param name="Reference">
        ///  Reference parameter.
        /// </param>
        /// <param name="Level">
        ///  Level parameter.
        /// </param>
        /// <param name="Buffer">
        ///  Buffer parameter.
        /// </param>
        /// <param name="BufferSize">
        ///  BufferSize parameter.
        /// </param>
        /// <param name="CountReturned">
        ///  CountReturned parameter.
        /// </param>
        /// <param name="TotalEntries">
        ///  TotalEntries parameter.
        /// </param>
        /// <param name="NextReference">
        ///  NextReference parameter.
        /// </param>
        /// <param name="LastRecordId">
        ///  LastRecordId parameter.
        /// </param>
        public NtStatus NetrAccountSync(
            string PrimaryName,
            string ComputerName,
            _NETLOGON_AUTHENTICATOR? Authenticator,
            ref _NETLOGON_AUTHENTICATOR? ReturnAuthenticator,
            uint Reference,
            uint Level,
            out byte[] Buffer,
            uint BufferSize,
            out uint? CountReturned,
            out uint? TotalEntries,
            out uint? NextReference,
            out _UAS_INFO_0? LastRecordId)
        {
            const ushort opnum = 10;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pPrimaryName = Marshal.StringToHGlobalUni(PrimaryName);
            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);
            SafeIntPtr pAuthenticator = TypeMarshal.ToIntPtr(Authenticator);
            SafeIntPtr pReturnAuthenticatorIn = TypeMarshal.ToIntPtr(ReturnAuthenticator);

            paramList = new Int3264[] {
                pPrimaryName,
                pComputerName,
                pAuthenticator,
                pReturnAuthenticatorIn,
                Reference,
                Level,
                IntPtr.Zero,
                BufferSize,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pReturnAuthenticatorOut = outParamList[3];
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(pReturnAuthenticatorOut);

                IntPtr pBuffer = outParamList[6];
                Buffer = IntPtrUtility.PtrToArray<byte>(pBuffer, BufferSize);

                CountReturned = TypeMarshal.ToNullableStruct<uint>(outParamList[8]);

                TotalEntries = TypeMarshal.ToNullableStruct<uint>(outParamList[9]);

                NextReference = TypeMarshal.ToNullableStruct<uint>(outParamList[10]);

                IntPtr pLastRecordId = outParamList[11];
                LastRecordId = TypeMarshal.ToNullableStruct<_UAS_INFO_0>(pLastRecordId);

                retVal = outParamList[12].ToInt32();
            }

            pPrimaryName.Dispose();
            pComputerName.Dispose();
            pAuthenticator.Dispose();
            pReturnAuthenticatorIn.Dispose();

            return (NtStatus)retVal;
        }


        /// <summary>
        ///  The NetrGetDCName method This method was used in windows_nt_server_3_1
        ///  and is supported in windows_nt_server_3_1 versions.
        ///  It was superseded by the DsrGetDcNameEx2 method (section
        ///  ) in windows_2000, windows_xp, windows_server_2003,
        ///  windows_vista,  and windows_server_2008windows_7, and
        ///  windows_server_7. retrieves the NetBIOS name of the
        ///  PDC for the specified domain. Opnum: 11 
        /// </summary>
        /// <param name="ServerName">
        ///  The custom RPC binding handle, as specified in section
        ///  , that represents the connection to a domain controller.
        /// </param>
        /// <param name="DomainName">
        ///  A null-terminated Unicode string that specifies the
        ///  NetBIOS name of the domain.
        /// </param>
        /// <param name="Buffer">
        ///  A pointer to a null-terminated Unicode string that contains
        ///  the NetBIOS name of the PDC for the specified domain.
        ///  The server name returned by this method is prefixed
        ///  by two backslashes (\\).
        /// </param>
        public NetApiStatus NetrGetDCName(
            string ServerName,
            string DomainName,
            out string Buffer)
        {
            const ushort opnum = 11;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pServerName = Marshal.StringToHGlobalUni(ServerName);
            SafeIntPtr pDomainName = Marshal.StringToHGlobalUni(DomainName);

            paramList = new Int3264[] {
                pServerName,
                pDomainName,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pBuffer = outParamList[2];
                //wchar_t** Buffer
                pBuffer = Marshal.ReadIntPtr(pBuffer);
                Buffer = Marshal.PtrToStringUni(pBuffer);

                retVal = outParamList[3].ToInt32();
            }

            pServerName.Dispose();
            pDomainName.Dispose();

            return (NetApiStatus)retVal;
        }


        /// <summary>
        ///  The NetrLogonControl method is a predecessor to the
        ///  NetrLogonControl2Ex method, as specified in section
        ///  . All parameters of this method have the same meanings
        ///  as the identically named parameters of the NetrLogonControl2Ex
        ///  method. Opnum: 12 
        /// </summary>
        /// <param name="ServerName">
        ///  ServerName parameter.
        /// </param>
        /// <param name="FunctionCode">
        ///  FunctionCode parameter.
        /// </param>
        /// <param name="QueryLevel">
        ///  QueryLevel parameter.
        /// </param>
        /// <param name="Buffer">
        ///  Buffer parameter.
        /// </param>
        public NetApiStatus NetrLogonControl(
            string ServerName,
            uint FunctionCode,
            uint QueryLevel,
            out _NETLOGON_CONTROL_QUERY_INFORMATION? Buffer)
        {
            const ushort opnum = 12;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pServerName = Marshal.StringToHGlobalUni(ServerName);

            paramList = new Int3264[] {
                pServerName,
                FunctionCode,
                QueryLevel,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pBuffer = outParamList[3];
                Buffer = TypeMarshal.ToNullableStruct<_NETLOGON_CONTROL_QUERY_INFORMATION>(
                    pBuffer,
                    QueryLevel,
                    null,
                    null);

                retVal = outParamList[4].ToInt32();
            }

            pServerName.Dispose();

            return (NetApiStatus)retVal;
        }


        /// <summary>
        ///  The NetrGetAnyDCName method This method was introduced
        ///  in windows_nt_server_3_1 and is supported in windows_nt_server_3_1
        ///  versions. It was superseded by the DsrGetDcNameEx2
        ///  method (section) in windows_2000, windows_xp, windows_server_2003,
        ///  windows_vista, and windows_server_2008, windows_7,
        ///  and windows_server_7. retrieves the name of a domain
        ///  controller in the specified primary or directly trusted
        ///  domain. Only DCs can return the name of a DC in a specified
        ///  directly trusted domain. Opnum: 13 
        /// </summary>
        /// <param name="ServerName">
        ///  The custom RPC binding handle, as specified in section
        ///  .
        /// </param>
        /// <param name="DomainName">
        ///  A null-terminated Unicode string that contains the name
        ///  of the primary or directly trusted domain. If the string
        ///  is NULL or empty (that is, the first character in the
        ///  string is the null-terminator character), the primary
        ///  domain name (3) is assumed.
        /// </param>
        /// <param name="Buffer">
        ///  A pointer to an allocated buffer that contains the null-terminated
        ///  Unicode string containing the NetBIOS name of a DC
        ///  in the specified domain. The DC name is prefixed by
        ///  two backslashes (\\).
        /// </param>
        public NetApiStatus NetrGetAnyDCName(
            string ServerName,
            string DomainName,
            out string Buffer)
        {
            const ushort opnum = 13;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pServerName = Marshal.StringToHGlobalUni(ServerName);
            SafeIntPtr pDomainName = Marshal.StringToHGlobalUni(DomainName);

            paramList = new Int3264[] {
                pServerName,
                pDomainName,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pBuffer = outParamList[2];
                //wchar_t** Buffer
                pBuffer = Marshal.ReadIntPtr(pBuffer);
                Buffer = Marshal.PtrToStringUni(pBuffer);

                retVal = outParamList[3].ToInt32();
            }

            pServerName.Dispose();
            pDomainName.Dispose();

            return (NetApiStatus)retVal;
        }


        /// <summary>
        ///  The NetrLogonControl2 method is a predecessor to the
        ///  NetrLogonControl2Ex method, as specified in section
        ///  . All parameters of this method have the same meanings
        ///  as the identically named parameters of the NetrLogonControl2Ex
        ///  method. Opnum: 14 
        /// </summary>
        /// <param name="ServerName">
        ///  ServerName parameter.
        /// </param>
        /// <param name="FunctionCode">
        ///  FunctionCode parameter.
        /// </param>
        /// <param name="QueryLevel">
        ///  QueryLevel parameter.
        /// </param>
        /// <param name="Data">
        ///  Data parameter.
        /// </param>
        /// <param name="Buffer">
        ///  Buffer parameter.
        /// </param>
        public NetApiStatus NetrLogonControl2(
            string ServerName,
            uint FunctionCode,
            uint QueryLevel,
            _NETLOGON_CONTROL_DATA_INFORMATION? Data,
            out _NETLOGON_CONTROL_QUERY_INFORMATION? Buffer)
        {
            const ushort opnum = 14;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pServerName = Marshal.StringToHGlobalUni(ServerName);
            SafeIntPtr pData = TypeMarshal.ToIntPtr(Data, FunctionCode, null, null);

            paramList = new Int3264[] {
                pServerName,
                FunctionCode,
                QueryLevel,
                pData,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pBuffer = outParamList[4];
                Buffer = TypeMarshal.ToNullableStruct<_NETLOGON_CONTROL_QUERY_INFORMATION>(
                    pBuffer,
                    QueryLevel,
                    null,
                    null);

                retVal = outParamList[5].ToInt32();
            }

            pServerName.Dispose();
            pData.Dispose();

            return (NetApiStatus)retVal;
        }


        /// <summary>
        ///  The NetrServerAuthenticate2 method This method was used
        ///  in windows_nt_3_5 and windows_nt_4_0. In windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7, it was superseded
        ///  by the NetrServerAuthenticate3 method (section ). is
        ///  a predecessor to the NetrServerAuthenticate3 method,
        ///  as specified in section. All parameters of this method
        ///  have the same meanings as the identically named parameters
        ///  of the NetrServerAuthenticate3 method. Opnum: 15 
        /// </summary>
        /// <param name="PrimaryName">
        ///  PrimaryName parameter.
        /// </param>
        /// <param name="AccountName">
        ///  AccountName parameter.
        /// </param>
        /// <param name="SecureChannelType">
        ///  SecureChannelType parameter.
        /// </param>
        /// <param name="ComputerName">
        ///  ComputerName parameter.
        /// </param>
        /// <param name="ClientCredential">
        ///  ClientCredential parameter.
        /// </param>
        /// <param name="ServerCredential">
        ///  ServerCredential parameter.
        /// </param>
        /// <param name="NegotiateFlags">
        ///  NegotiateFlags parameter.
        /// </param>
        public NtStatus NetrServerAuthenticate2(
            string PrimaryName,
            string AccountName,
            _NETLOGON_SECURE_CHANNEL_TYPE SecureChannelType,
            string ComputerName,
            _NETLOGON_CREDENTIAL? ClientCredential,
            out _NETLOGON_CREDENTIAL? ServerCredential,
            ref uint? NegotiateFlags)
        {
            const ushort opnum = 15;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pPrimaryName = Marshal.StringToHGlobalUni(PrimaryName);
            SafeIntPtr pAccountName = Marshal.StringToHGlobalUni(AccountName);
            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);
            SafeIntPtr pClientCredential = TypeMarshal.ToIntPtr(ClientCredential);
            SafeIntPtr pNegotiateFlags = TypeMarshal.ToIntPtr(NegotiateFlags);

            paramList = new Int3264[] {
                pPrimaryName,
                pAccountName,
                (uint)SecureChannelType,
                pComputerName,
                pClientCredential,
                IntPtr.Zero,
                pNegotiateFlags,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pServerCredential = outParamList[5];
                ServerCredential = TypeMarshal.ToNullableStruct<_NETLOGON_CREDENTIAL>(pServerCredential);

                NegotiateFlags = TypeMarshal.ToNullableStruct<uint>(outParamList[6]);

                retVal = outParamList[7].ToInt32();
            }

            pPrimaryName.Dispose();
            pAccountName.Dispose();
            pComputerName.Dispose();
            pClientCredential.Dispose();
            pNegotiateFlags.Dispose();

            return (NtStatus)retVal;
        }


        /// <summary>
        ///  The NetrDatabaseSync2 method returns a set of all changes
        ///  applied to the specified database since its creation.
        ///  It provides an interface for a BDC to fully synchronize
        ///  its databases to those of the PDC. Because returning
        ///  all changes in one call might be prohibitively expensive
        ///  due to a large amount of data being returned, this
        ///  method supports retrieving portions of the database
        ///  changes in a series of calls using a continuation context
        ///  until all changes are received. It is possible for
        ///  the series of calls to be terminated prematurely due
        ///  to external events, such as system restarts. For that
        ///  reason, the method also supports restarting the series
        ///  of calls at a particular point specified by the caller.
        ///  The caller MUST keep track of synchronization progress
        ///  during the series of calls as detailed in this section.
        ///  Opnum: 16 
        /// </summary>
        /// <param name="PrimaryName">
        ///  The custom RPC binding handle, as specified in section
        ///  , that represents the connection to the PDC.
        /// </param>
        /// <param name="ComputerName">
        ///  The null-terminated Unicode string that contains the
        ///  NetBIOS name of the BDC calling this method.
        /// </param>
        /// <param name="Authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the client authenticator.
        /// </param>
        /// <param name="ReturnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="DatabaseID">
        ///  The identifier for a specific database for which the
        ///  changes are requested. It MUST be one of the following
        ///  values.
        /// </param>
        /// <param name="RestartState">
        ///  Specifies whether this is a restart of the series of
        ///  the synchronization calls and how to interpret SyncContext.
        ///  This value MUST be NormalState unless this is the restart,
        ///  in which case the value MUST be set as specified in
        ///  the description of the SyncContext parameter.
        /// </param>
        /// <param name="SyncContext">
        ///  Specifies context needed to continue the operation.
        ///  The value MUST be set to zero on the first call. The
        ///  caller MUST treat this as an opaque value, unless this
        ///  call is a restart of the series of synchronization
        ///  calls. The value returned is to be used on input for
        ///  the next call in the series of synchronization calls. If
        ///  this call is the restart of the series, the values
        ///  of the RestartState and the SyncContext parameters
        ///  are dependent on the DeltaType value received on the
        ///  last call before the restart and MUST be set as follows.
        ///  Find the last NETLOGON_DELTA_ENUM structure in the
        ///  DeltaArray parameter of the call. The DeltaType field
        ///  of this NETLOGON_DELTA_ENUM structure, as specified
        ///  in section , is the DeltaType needed for the restart.
        ///  The values of RestartState and SyncContext are then
        ///  determined from the following table. DeltaTypeRestartStateSyncContextAddOrChangeGroupGroupStateThe
        ///  value of the RID field of the last element AddOrChangeUserUserStateThe
        ///  value of the RID field of the last element ChangeGroupMembershipGroupMemberStateThe
        ///  value of the RID field of the last element AddOrChangeAliasAliasState0x00000000ChangeAliasMembershipAliasMemberState0x00000000Any
        ///  other value not previously listedNormalState0x00000000
        /// </param>
        /// <param name="DeltaArray">
        ///  A pointer to a NETLOGON_DELTA_ENUM_ARRAY structure,
        ///  as specified in section , that contains an array of
        ///  enumerated changes (deltas) to the specified database.
        /// </param>
        /// <param name="PreferredMaximumLength">
        ///  The value that specifies the preferred maximum size,
        ///  in bytes, of data referenced in the DeltaArray parameter.
        ///  This is not a hard upper limit, but serves as a guide
        ///  to the server. The server SHOULDwindows stops including
        ///  elements in the returned DeltaArray once the size of
        ///  the returned data equals or exceeds the value of the
        ///  PreferredMaximumLength parameter. The server SHOULD stop including elements
        ///  in the returned DeltaArray once the size of the returned
        ///  data equals or exceeds the value of the PreferredMaximumLength
        ///  parameter. It is up to the client implementation to
        ///  choose the value for this parameter.
        /// </param>
        public NtStatus NetrDatabaseSync2(
            string PrimaryName,
            string ComputerName,
            _NETLOGON_AUTHENTICATOR? Authenticator,
            ref _NETLOGON_AUTHENTICATOR? ReturnAuthenticator,
            DatabaseID_Values DatabaseID,
            _SYNC_STATE RestartState,
            ref uint? SyncContext,
            out _NETLOGON_DELTA_ENUM_ARRAY? DeltaArray,
            uint PreferredMaximumLength)
        {
            const ushort opnum = 16;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pPrimaryName = Marshal.StringToHGlobalUni(PrimaryName);
            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);
            SafeIntPtr pAuthenticator = TypeMarshal.ToIntPtr(Authenticator);
            SafeIntPtr pReturnAuthenticatorIn = TypeMarshal.ToIntPtr(ReturnAuthenticator);
            SafeIntPtr pSyncContext = TypeMarshal.ToIntPtr(SyncContext);


            paramList = new Int3264[] {
                pPrimaryName,
                pComputerName,
                pAuthenticator,
                pReturnAuthenticatorIn,
                (uint)DatabaseID,
                (uint)RestartState,
                pSyncContext,
                IntPtr.Zero,
                PreferredMaximumLength,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pReturnAuthenticatorOut = outParamList[3];
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(pReturnAuthenticatorOut);

                SyncContext = TypeMarshal.ToNullableStruct<uint>(outParamList[6]);

                IntPtr pDeltaArray = outParamList[7];
                pDeltaArray = Marshal.ReadIntPtr(pDeltaArray);
                DeltaArray = TypeMarshal.ToNullableStruct<_NETLOGON_DELTA_ENUM_ARRAY>(pDeltaArray);

                retVal = outParamList[9].ToInt32();
            }

            pPrimaryName.Dispose();
            pComputerName.Dispose();
            pAuthenticator.Dispose();
            pReturnAuthenticatorIn.Dispose();
            pSyncContext.Dispose();

            return (NtStatus)retVal;
        }


        /// <summary>
        ///  The NetrDatabaseRedo method is used by a BDC to request
        ///  information about a single account from the PDC. Opnum
        ///  : 17 
        /// </summary>
        /// <param name="PrimaryName">
        ///  The custom RPC binding handle, as specified in section
        ///  , representing the connection to the PDC.
        /// </param>
        /// <param name="ComputerName">
        ///  The null-terminated Unicode string that contains the
        ///  NetBIOS name of the BDC calling this method.
        /// </param>
        /// <param name="Authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the client authenticator.
        /// </param>
        /// <param name="ReturnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="ChangeLogEntry">
        ///  A pointer to a buffer that contains a CHANGELOG_ENTRY
        ///  structure, specified as follows, for the account being
        ///  queried.
        /// </param>
        /// <param name="ChangeLogEntrySize">
        ///  The size, in bytes, of the buffer pointed to by the
        ///  ChangeLogEntry parameter.
        /// </param>
        /// <param name="DeltaArray">
        ///  A pointer to a NETLOGON_DELTA_ENUM_ARRAY structure,
        ///  as specified in section , that contains an array of
        ///  enumerated database changes for the account being queried.
        /// </param>
        public NtStatus NetrDatabaseRedo(
            string PrimaryName,
            string ComputerName,
            _NETLOGON_AUTHENTICATOR? Authenticator,
            ref _NETLOGON_AUTHENTICATOR? ReturnAuthenticator,
            byte[] ChangeLogEntry,
            uint ChangeLogEntrySize,
            out _NETLOGON_DELTA_ENUM_ARRAY? DeltaArray)
        {
            const ushort opnum = 17;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pPrimaryName = Marshal.StringToHGlobalUni(PrimaryName);
            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);
            SafeIntPtr pAuthenticator = TypeMarshal.ToIntPtr(Authenticator);
            SafeIntPtr pReturnAuthenticatorIn = TypeMarshal.ToIntPtr(ReturnAuthenticator);
            SafeIntPtr pChangeLogEntry = IntPtrUtility.ArrayToPtr(ChangeLogEntry);

            paramList = new Int3264[] {
                pPrimaryName,
                pComputerName,
                pAuthenticator,
                pReturnAuthenticatorIn,
                pChangeLogEntry,
                ChangeLogEntrySize,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pReturnAuthenticatorOut = outParamList[3];
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(pReturnAuthenticatorOut);

                IntPtr pDeltaArray = outParamList[6];
                pDeltaArray = Marshal.ReadIntPtr(pDeltaArray);
                DeltaArray = TypeMarshal.ToNullableStruct<_NETLOGON_DELTA_ENUM_ARRAY>(pDeltaArray);

                retVal = outParamList[7].ToInt32();
            }

            pPrimaryName.Dispose();
            pComputerName.Dispose();
            pAuthenticator.Dispose();
            pReturnAuthenticatorIn.Dispose();
            pChangeLogEntry.Dispose();

            return (NtStatus)retVal;
        }


        /// <summary>
        ///  The NetrLogonControl2Ex method executes windows-specific
        ///  administrative actions that pertain to the Netlogon
        ///  server operation. It is used to query the status and
        ///  control the actions of the Netlogon server. Opnum :
        ///  18 
        /// </summary>
        /// <param name="ServerName">
        ///  The custom RPC binding handle, as specified in section
        ///  .
        /// </param>
        /// <param name="FunctionCode">
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
        /// <param name="QueryLevel">
        ///  Information query level requested by the client. The
        ///  buffer returned in the Buffer parameter contains one
        ///  of the following structures, based on the value of
        ///  this field.
        /// </param>
        /// <param name="Data">
        ///  NETLOGON_CONTROL_DATA_INFORMATION structure, as specified
        ///  in section , that contains specific data required by
        ///  the query.
        /// </param>
        /// <param name="Buffer">
        ///  NETLOGON_CONTROL_QUERY_INFORMATION structure, as specified
        ///  in section , that contains the specific query results,
        ///  with a level of verbosity as specified in QueryLevel.
        /// </param>
        public NetApiStatus NetrLogonControl2Ex(
            string ServerName,
            FunctionCode_Values FunctionCode,
            QueryLevel_Values QueryLevel,
            _NETLOGON_CONTROL_DATA_INFORMATION? Data,
            out _NETLOGON_CONTROL_QUERY_INFORMATION? Buffer)
        {
            const ushort opnum = 18;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pServerName = Marshal.StringToHGlobalUni(ServerName);
            SafeIntPtr pData = TypeMarshal.ToIntPtr(Data, FunctionCode, null, null);

            paramList = new Int3264[] {
                pServerName,
                (uint)FunctionCode,
                (uint)QueryLevel,
                pData,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pBuffer = outParamList[4];
                Buffer = TypeMarshal.ToNullableStruct<_NETLOGON_CONTROL_QUERY_INFORMATION>(
                    pBuffer,
                    QueryLevel,
                    null,
                    null);

                retVal = outParamList[5].ToInt32();
            }

            pServerName.Dispose();
            pData.Dispose();

            return (NetApiStatus)retVal;
        }


        /// <summary>
        ///  The NetrEnumerateTrustedDomains method returns a set
        ///  of trusted domain names. Opnum: 19 
        /// </summary>
        /// <param name="ServerName">
        ///  The custom RPC binding handle, as specified in section
        ///  .
        /// </param>
        /// <param name="DomainNameBuffer">
        ///  A pointer to a DOMAIN_NAME_BUFFER structure, as specified
        ///  in section , that contains a list of trusted domain
        ///  names. The format of domain names contained in the
        ///  buffer is specified in section.
        /// </param>
        public NtStatus NetrEnumerateTrustedDomains(
            string ServerName,
            out _DOMAIN_NAME_BUFFER? DomainNameBuffer)
        {
            const ushort opnum = 19;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pServerName = Marshal.StringToHGlobalUni(ServerName);

            paramList = new Int3264[] {
                pServerName,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pDomainNameBuffer = outParamList[1];
                DomainNameBuffer = TypeMarshal.ToNullableStruct<_DOMAIN_NAME_BUFFER>(pDomainNameBuffer);

                retVal = outParamList[2].ToInt32();
            }

            pServerName.Dispose();

            return (NtStatus)retVal;
        }


        /// <summary>
        ///  The DsrGetDcName method Supported in windows_2000, windows_xp,
        ///  windows_server_2003, windows_vista, and windows_server_2008,
        ///  windows_7, and windows_server_7. is a predecessor to
        ///  the DsrGetDcNameEx2 method (section ). The method returns
        ///  information about a domain controller in the specified
        ///  domain. All parameters of this method have the same
        ///  meanings as the identically named parameters of the
        ///  DsrGetDcNameEx2 method, except for the SiteGuid parameter,
        ///  detailed as follows. Opnum: 20 
        /// </summary>
        /// <param name="ComputerName">
        ///  ComputerName parameter.
        /// </param>
        /// <param name="DomainName">
        ///  DomainName parameter.
        /// </param>
        /// <param name="DomainGuid">
        ///  DomainGuid parameter.
        /// </param>
        /// <param name="SiteGuid">
        ///  This parameter MUST be NULL and ignored upon receipt.
        /// </param>
        /// <param name="Flags">
        ///  Flags parameter.
        /// </param>
        /// <param name="DomainControllerInfo">
        ///  DomainControllerInfo parameter.
        /// </param>
        public NetApiStatus DsrGetDcName(
            string ComputerName,
            string DomainName,
            Guid? DomainGuid,
            Guid? SiteGuid,
            uint Flags,
            out _DOMAIN_CONTROLLER_INFOW? DomainControllerInfo)
        {
            const ushort opnum = 20;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);
            SafeIntPtr pDomainName = Marshal.StringToHGlobalUni(DomainName);
            SafeIntPtr pDomainGuid = TypeMarshal.ToIntPtr(DomainGuid);
            SafeIntPtr pSiteGuid = TypeMarshal.ToIntPtr(SiteGuid);

            paramList = new Int3264[] {
                pComputerName,
                pDomainName,
                pDomainGuid,
                pSiteGuid,
                Flags,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pDomainControllerInfo = outParamList[5];
                pDomainControllerInfo = Marshal.ReadIntPtr(pDomainControllerInfo);
                DomainControllerInfo = TypeMarshal.ToNullableStruct<_DOMAIN_CONTROLLER_INFOW>(pDomainControllerInfo);

                retVal = outParamList[6].ToInt32();
            }

            pComputerName.Dispose();
            pDomainName.Dispose();
            pDomainGuid.Dispose();
            pSiteGuid.Dispose();

            return (NetApiStatus)retVal;
        }


        /// <summary>
        ///  The NetrLogonDummyRoutine1 method is no longer supported.
        ///  It serves as a placeholder in the IDL file for the
        ///  RPC opnum value 21.The NetrLogonDummyRoutine1 method
        ///  is deprecated by NetrLogonGetCapabilities. It serves
        ///  as a placeholder in the IDL file for the RPC opnum value
        ///  21.Supported in windows_nt, windows_2000, windows_xp,
        ///  windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. Opnum: 21 
        /// </summary>
        /// <param name="ServerName">
        ///  A LOGONSRV_HANDLE Unicode string handle of the server
        ///  that is handling the request.
        /// </param>
        /// <param name="ComputerName">
        ///  A string that contains the name of the computer.
        /// </param>
        /// <param name="Authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure that
        ///  contains the client authenticator.
        /// </param>
        /// <param name="ReturnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure that
        ///  contains the server return authenticator.
        /// </param>
        /// <param name="QueryLevel">
        ///  Specifies the level of information to return from the
        ///  domain controller being queried. A value of 0x00000001
        ///  causes a NETLOGON_DOMAIN_INFO structure that contains
        ///  information about the DC to be returned.
        /// </param>
        /// <param name="serverCapabilities">
        ///  A pointer to a 32-bit set of bit flags that identify 
        ///  the server's capabilities.
        /// </param>
        public NtStatus NetrLogonGetCapabilities(
            string ServerName,
            string ComputerName,
            _NETLOGON_AUTHENTICATOR? Authenticator,
            ref _NETLOGON_AUTHENTICATOR? ReturnAuthenticator,
            uint QueryLevel,
            out _NETLOGON_CAPABILITIES? serverCapabilities)
        {
            const ushort opnum = 21;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pServerName = Marshal.StringToHGlobalUni(ServerName);
            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);
            SafeIntPtr pAuthenticator = TypeMarshal.ToIntPtr(Authenticator);
            SafeIntPtr pReturnAuthenticatorIn = TypeMarshal.ToIntPtr(ReturnAuthenticator);

            paramList = new Int3264[] {
                pServerName,
                pComputerName,
                pAuthenticator,
                pReturnAuthenticatorIn,
                QueryLevel,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pReturnAuthenticatorOut = outParamList[3];
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(pReturnAuthenticatorOut);

                IntPtr pServerCapabilities = outParamList[5];
                serverCapabilities = TypeMarshal.ToNullableStruct<_NETLOGON_CAPABILITIES>(
                    pServerCapabilities,
                    QueryLevel,
                    null,
                    null);

                retVal = outParamList[6].ToInt32();
            }

            pServerName.Dispose();
            pComputerName.Dispose();
            pAuthenticator.Dispose();
            pReturnAuthenticatorIn.Dispose();

            return (NtStatus)retVal;
        }


        /// <summary>
        ///  The NetrLogonSetServiceBitsSupported in windows_2000_server,
        ///  windows_xp, windows_server_2003. This method is used to
        ///  notify Netlogon whether a domain controller is running
        ///  specified services, as detailed in the following section.
        ///  Opnum: 22 
        /// </summary>
        /// <param name="ServerName">
        ///  The custom RPC binding handle, as specified in section
        ///  , representing the connection to a DC.
        /// </param>
        /// <param name="ServiceBitsOfInterest">
        ///  A set of bit flags used as a mask to indicate which
        ///  service's state (running or not running) is being set
        ///  by this call. The value is constructed from zero or
        ///  more bit flags from the following table.
        /// </param>
        /// <param name="ServiceBits">
        ///  A set of bit flags used as a mask to indicate whether
        ///  the service indicated by ServiceBitsOfInterest is running
        ///  or not. If the flag is set to 0, the corresponding
        ///  service indicated by ServiceBitsOfInterest is not running.
        ///  Otherwise, if the flag is set to 1, the corresponding
        ///  service indicated by ServiceBitsOfInterest is running.
        ///  The value is constructed from zero or more bit flags
        ///  from the following table.
        /// </param>
        public NtStatus NetrLogonSetServiceBits(
            string ServerName,
            uint ServiceBitsOfInterest,
            uint ServiceBits)
        {
            const ushort opnum = 22;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pServerName = Marshal.StringToHGlobalUni(ServerName);

            paramList = new Int3264[] {
                pServerName,
                ServiceBitsOfInterest,
                ServiceBits,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                retVal = outParamList[3].ToInt32();
            }

            pServerName.Dispose();

            return (NtStatus)retVal;
        }


        /// <summary>
        ///  The NetrLogonGetTrustRid method Supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7.windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. is used to obtain
        ///  the RID of the account whose password is used by domain
        ///  controllers in the specified domain for establishing
        ///  the secure channel from the server receiving this call.
        ///  Opnum: 23 
        /// </summary>
        /// <param name="ServerName">
        ///  The custom RPC binding handle, as specified in section
        ///  . ServerName SHOULD be NULL. In windows_server_2008
        ///  and windows_server_7, ServerName is NULL because this
        ///  method is restricted to local callers.
        /// </param>
        /// <param name="DomainName">
        ///  The null-terminated Unicode string that contains the
        ///  DNS or NetBIOS name of the primary or trusted domain.
        ///  If this parameter is NULL, this method uses the name
        ///  of the primary domain of the server.
        /// </param>
        /// <param name="Rid">
        ///  A pointer to an unsigned long that receives the RID
        ///  of the account.
        /// </param>
        public NetApiStatus NetrLogonGetTrustRid(
            string ServerName,
            string DomainName,
            out uint? Rid)
        {
            const ushort opnum = 23;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pServerName = Marshal.StringToHGlobalUni(ServerName);
            SafeIntPtr pDomainName = Marshal.StringToHGlobalUni(DomainName);

            paramList = new Int3264[] {
                pServerName,
                pDomainName,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                Rid = TypeMarshal.ToNullableStruct<uint>(outParamList[2]);

                retVal = outParamList[3].ToInt32();
            }

            pServerName.Dispose();
            pDomainName.Dispose();

            return (NetApiStatus)retVal;
        }


        /// <summary>
        ///  The NetrLogonComputeServerDigest methodSupported in
        ///  windows_2000_server, windows_xp, windows_server_2003,
        ///  windows_vista, windows_server_2008, windows_7, and
        ///  windows_server_7. computes a cryptographic digest of
        ///  a message by using the MD5 message-digest algorithm,
        ///  as specified in [RFC1321]. This method is called by
        ///  a client computer against a server and is used to compute
        ///  a message digest, as specified in this section. The
        ///  client MAY then call the NetrLogonComputeClientDigest
        ///  method (as specified in section) and compare the digests
        ///  to ensure that the server that it communicates with
        ///  knows the shared secret between the client machine
        ///  and the domain. Opnum: 24 
        /// </summary>
        /// <param name="ServerName">
        ///  The custom RPC binding handle, as specified in section
        ///  .
        /// </param>
        /// <param name="Rid">
        ///  The RID of the machine account for which the digest
        ///  is to be computed. The NetrLogonGetTrustRid method,
        ///  as specified in section , is used to obtain the RID.
        /// </param>
        /// <param name="Message">
        ///  A pointer to buffer that contains the message to compute
        ///  the digest.
        /// </param>
        /// <param name="MessageSize">
        ///  The length of the data referenced by the Message parameter,
        ///  in bytes.
        /// </param>
        /// <param name="NewMessageDigest">
        ///  A 128-bit MD5 digest of the current machine account
        ///  password and the message in the Message buffer. The
        ///  machine account is identified by the Rid parameter.
        /// </param>
        /// <param name="OldMessageDigest">
        ///  A 128-bit MD5 digest of the previous machine account
        ///  password, if present, and the message in the Message
        ///  buffer. If no previous machine account password exists,
        ///  then the current password is used. The machine account
        ///  is identified by the Rid parameter.
        /// </param>
        public NetApiStatus NetrLogonComputeServerDigest(
            string ServerName,
            uint Rid,
            byte[] Message,
            uint MessageSize,
            out byte[] NewMessageDigest,
            out byte[] OldMessageDigest)
        {
            const ushort opnum = 24;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pServerName = Marshal.StringToHGlobalUni(ServerName);
            SafeIntPtr pMessage = IntPtrUtility.ArrayToPtr(Message);

            paramList = new Int3264[] {
                pServerName,
                Rid,
                pMessage,
                MessageSize,
                IntPtr.Zero,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pNewMessageDigest = outParamList[4];
                NewMessageDigest = IntPtrUtility.PtrToArray<byte>(pNewMessageDigest, 16); //size_is(16)

                IntPtr pOldMessageDigest = outParamList[5];
                OldMessageDigest = IntPtrUtility.PtrToArray<byte>(pOldMessageDigest, 16); //size_is(16)

                retVal = outParamList[6].ToInt32();
            }

            pServerName.Dispose();
            pMessage.Dispose();

            return (NetApiStatus)retVal;
        }


        /// <summary>
        ///  The NetrLogonComputeClientDigest methodSupported in
        ///  windows_2000_server, windows_xp, windows_server_2003,
        ///  windows_vista, windows_server_2008, windows_7, and
        ///  windows_server_7. is used by a client to compute a
        ///  cryptographic digest of a message by using the MD5
        ///  message-digest algorithm, as specified in [RFC1321].
        ///  This method is called by a client to compute a message
        ///  digest, as specified in this section. The client SHOULD
        ///  use this digest to compare against one that is returned
        ///  by a call to NetrLogonComputeServerDigest. This comparison
        ///  allows the client to ensure that the server that it
        ///  communicates with knows the shared secret between the
        ///  client machine and the domain. Opnum: 25 
        /// </summary>
        /// <param name="ServerName">
        ///  The custom RPC binding handle, as specified in section
        ///  .
        /// </param>
        /// <param name="DomainName">
        ///  A pointer to a null-terminated Unicode string that contains
        ///  the DNS or NetBIOS name of the trusted domain. If this
        ///  parameter is NULL, the domain of which the client computer
        ///  is a member is used.
        /// </param>
        /// <param name="Message">
        ///  A pointer to a buffer that contains the message for
        ///  which the digest is to be computed.
        /// </param>
        /// <param name="MessageSize">
        ///  The length, in bytes, of the Message parameter.
        /// </param>
        /// <param name="NewMessageDigest">
        ///  A 128-bit MD5 digest of the current computer account
        ///  password and the message in the Message buffer.
        /// </param>
        /// <param name="OldMessageDigest">
        ///  A 128-bit MD5 digest of the previous machine account
        ///  password and the message in the Message buffer. If
        ///  no previous computer account password exists, the current
        ///  password is used.
        /// </param>
        public NetApiStatus NetrLogonComputeClientDigest(
            string ServerName,
            string DomainName,
            byte[] Message,
            uint MessageSize,
            out byte[] NewMessageDigest,
            out byte[] OldMessageDigest)
        {
            const ushort opnum = 25;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pServerName = Marshal.StringToHGlobalUni(ServerName);
            SafeIntPtr pDomainName = Marshal.StringToHGlobalUni(DomainName);
            SafeIntPtr pMessage = IntPtrUtility.ArrayToPtr(Message);

            paramList = new Int3264[] {
                pServerName,
                pDomainName,
                pMessage,
                MessageSize,
                IntPtr.Zero,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pNewMessageDigest = outParamList[4];
                NewMessageDigest = IntPtrUtility.PtrToArray<byte>(pNewMessageDigest, 16); //size_is(16)

                IntPtr pOldMessageDigest = outParamList[5];
                OldMessageDigest = IntPtrUtility.PtrToArray<byte>(pOldMessageDigest, 16); //size_is(16)

                retVal = outParamList[6].ToInt32();
            }

            pServerName.Dispose();
            pDomainName.Dispose();
            pMessage.Dispose();

            return (NetApiStatus)retVal;
        }


        /// <summary>
        ///  The NetrServerAuthenticate3 method mutually authenticates
        ///  the client and the server and establishes the session
        ///  key to be used for the secure channel message protection
        ///  between the client and the server. Supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. It is called after
        ///  the NetrServerReqChallenge method, as specified in
        ///  section. Opnum: 26 
        /// </summary>
        /// <param name="PrimaryName">
        ///  The custom RPC binding handle, as specified in section
        ///  .
        /// </param>
        /// <param name="AccountName">
        ///  A null-terminated Unicode string that identifies the
        ///  name of the account that contains the secret key (password)
        ///  that is shared between the client and the server, as
        ///  specified in section.In windows, all machine account
        ///  names are the name of the machine with a $ (dollar
        ///  sign) appended. If there is a period at the end of
        ///  the account name, it is ignored during processing.
        /// </param>
        /// <param name="SecureChannelType">
        ///  A NETLOGON_SECURE_CHANNEL_TYPE enumerated value, as
        ///  specified in section , that indicates the type of the
        ///  secure channel being established by this call.
        /// </param>
        /// <param name="ComputerName">
        ///  A null-terminated Unicode string that contains the NetBIOS
        ///  name of the client computer calling this method.
        /// </param>
        /// <param name="ClientCredential">
        ///  A pointer to a NETLOGON_CREDENTIAL structure, as specified
        ///  in section , that contains the supplied client credentials,
        ///  as specified in section.
        /// </param>
        /// <param name="ServerCredential">
        ///  A pointer to a NETLOGON_CREDENTIAL structure, as specified
        ///  in section , that contains the returned server credentials.
        /// </param>
        /// <param name="NegotiateFlags">
        ///  A pointer to a 32-bit set of bit flags that indicate
        ///  features supported. As input, the set of flags are
        ///  those requested by the client and SHOULD be the same
        ///  as ClientCapabilities. As output, they are the bit-wise
        ///  AND of the client's requested capabilities and the
        ///  server's ServerCapabilities. For more details, see
        ///  section.
        /// </param>
        /// <param name="AccountRid">
        ///  A pointer that receives the RID of the account specified
        ///  by the AccountName parameter. (Section  of [MS-ADTS]
        ///  describes how this RID is assigned at account creation
        ///  time.) This value is stored in the AccountRid ADM element
        ///  within the ClientSessionInfo table.
        /// </param>
        public NtStatus NetrServerAuthenticate3(
            string PrimaryName,
            string AccountName,
            _NETLOGON_SECURE_CHANNEL_TYPE SecureChannelType,
            string ComputerName,
            _NETLOGON_CREDENTIAL? ClientCredential,
            out _NETLOGON_CREDENTIAL? ServerCredential,
            ref uint? NegotiateFlags,
            out uint? AccountRid)
        {
            const ushort opnum = 26;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pPrimaryName = Marshal.StringToHGlobalUni(PrimaryName);
            SafeIntPtr pAccountName = Marshal.StringToHGlobalUni(AccountName);
            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);
            SafeIntPtr pClientCredential = TypeMarshal.ToIntPtr(ClientCredential);
            SafeIntPtr pNegotiateFlags = TypeMarshal.ToIntPtr(NegotiateFlags);

            paramList = new Int3264[] {
                pPrimaryName,
                pAccountName,
                (uint)SecureChannelType,
                pComputerName,
                pClientCredential,
                IntPtr.Zero,
                pNegotiateFlags,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pServerCredential = outParamList[5];
                ServerCredential = TypeMarshal.ToNullableStruct<_NETLOGON_CREDENTIAL>(pServerCredential);

                NegotiateFlags = TypeMarshal.ToNullableStruct<uint>(outParamList[6]);

                AccountRid = TypeMarshal.ToNullableStruct<uint>(outParamList[7]);

                retVal = outParamList[8].ToInt32();
            }

            pPrimaryName.Dispose();
            pAccountName.Dispose();
            pComputerName.Dispose();
            pClientCredential.Dispose();
            pNegotiateFlags.Dispose();

            return (NtStatus)retVal;
        }


        /// <summary>
        ///  The DsrGetDcNameEx method Supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. is a predecessor to
        ///  the DsrGetDcNameEx2 method. The method returns information
        ///  about a domain controller in the specified domain and
        ///  site. All parameters of this method have the same meanings
        ///  as the identically named parameters of the DsrGetDcNameEx2
        ///  method. Opnum: 27 
        /// </summary>
        /// <param name="ComputerName">
        ///  ComputerName parameter.
        /// </param>
        /// <param name="DomainName">
        ///  DomainName parameter.
        /// </param>
        /// <param name="DomainGuid">
        ///  DomainGuid parameter.
        /// </param>
        /// <param name="SiteName">
        ///  SiteName parameter.
        /// </param>
        /// <param name="Flags">
        ///  Flags parameter.
        /// </param>
        /// <param name="DomainControllerInfo">
        ///  DomainControllerInfo parameter.
        /// </param>
        public NetApiStatus DsrGetDcNameEx(
            string ComputerName,
            string DomainName,
            Guid? DomainGuid,
            string SiteName,
            uint Flags,
            out _DOMAIN_CONTROLLER_INFOW? DomainControllerInfo)
        {
            const ushort opnum = 27;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            //CLIENT_CALL_RETURN _RetVal;
            //_RetVal = NdrClientCall2(
            //return ( NET_API_STATUS  )_RetVal.Simple;

            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);
            SafeIntPtr pDomainName = Marshal.StringToHGlobalUni(DomainName);
            SafeIntPtr pDomainGuid = TypeMarshal.ToIntPtr(DomainGuid);
            SafeIntPtr pSiteName = Marshal.StringToHGlobalUni(SiteName);

            paramList = new Int3264[] {
                pComputerName,
                pDomainName,
                pDomainGuid,
                pSiteName,
                Flags,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                true,
                responseStub,
                rpceClientTransport.Context.PackedDataRepresentationFormat,
                paramList))
            {
                //DomainControllerInfo is double pointer (**)
                //pDomainControllerInfo won't be null, read pointer inside directly.
                IntPtr pDomainControllerInfo;
                pDomainControllerInfo = outParamList[5];
                pDomainControllerInfo = Marshal.ReadIntPtr(pDomainControllerInfo);
                DomainControllerInfo = TypeMarshal.ToNullableStruct<_DOMAIN_CONTROLLER_INFOW>(pDomainControllerInfo);

                retVal = outParamList[6].ToInt32();
            }

            pComputerName.Dispose();
            pDomainName.Dispose();
            pDomainGuid.Dispose();
            pSiteName.Dispose();

            return (NetApiStatus)retVal;
        }


        /// <summary>
        ///  The DsrGetSiteName method Supported in windows_2000,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. returns the site name
        ///  for the specified computer that receives this call.
        ///  Opnum: 28 
        /// </summary>
        /// <param name="ComputerName">
        ///  The custom RPC binding handle (section ).
        /// </param>
        /// <param name="SiteName">
        ///  A null-terminated Unicode string that contains the name
        ///  of the site in which the computer that receives this
        ///  call resides.
        /// </param>
        public NetApiStatus DsrGetSiteName(
            string ComputerName,
            out string SiteName)
        {
            const ushort opnum = 28;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);

            paramList = new Int3264[] {
                pComputerName,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pSiteName = outParamList[1];
                //wchar_t** SiteName
                pSiteName = Marshal.ReadIntPtr(pSiteName);
                SiteName = Marshal.PtrToStringUni(pSiteName);

                retVal = outParamList[2].ToInt32();
            }

            pComputerName.Dispose();

            return (NetApiStatus)retVal;
        }


        /// <summary>
        ///  The NetrLogonGetDomainInfo method Supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, and
        ///  windows_server_2008, windows_7, and windows_server_7.
        ///  returns information that describes the current domain
        ///  to which the specified client belongs. Opnum: 29 
        /// </summary>
        /// <param name="ServerName">
        ///  The custom RPC binding handle, as specified in section
        ///  .
        /// </param>
        /// <param name="ComputerName">
        ///  The null-terminated Unicode string that contains the
        ///  name of the client computer issuing the request.
        /// </param>
        /// <param name="Authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the client authenticator.
        /// </param>
        /// <param name="ReturnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="Level">
        ///  The information level requested by the client. The buffer
        ///  contains one of the following structures, based on
        ///  the value of this field.
        /// </param>
        /// <param name="WkstaBuffer">
        ///  A pointer to a NETLOGON_WORKSTATION_INFORMATION structure,
        ///  as specified in section , that contains information
        ///  about the client workstation.
        /// </param>
        /// <param name="DomBuffer">
        ///  A pointer to a NETLOGON_DOMAIN_INFORMATION structure,
        ///  as specified in section , that contains information
        ///  about the domain or policy information.
        /// </param>
        public NtStatus NetrLogonGetDomainInfo(
            string ServerName,
            string ComputerName,
            _NETLOGON_AUTHENTICATOR? Authenticator,
            ref _NETLOGON_AUTHENTICATOR? ReturnAuthenticator,
            Level_Values Level,
            _NETLOGON_WORKSTATION_INFORMATION? WkstaBuffer,
            out _NETLOGON_DOMAIN_INFORMATION? DomBuffer)
        {
            const ushort opnum = 29;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pServerName = Marshal.StringToHGlobalUni(ServerName);
            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);
            SafeIntPtr pAuthenticator = TypeMarshal.ToIntPtr(Authenticator);
            SafeIntPtr pReturnAuthenticatorIn = TypeMarshal.ToIntPtr(ReturnAuthenticator);
            SafeIntPtr pWkstaBuffer = TypeMarshal.ToIntPtr(WkstaBuffer, Level, null, null);

            paramList = new Int3264[] {
                pServerName,
                pComputerName,
                pAuthenticator,
                pReturnAuthenticatorIn,
                (uint)Level,
                pWkstaBuffer,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pReturnAuthenticatorOut = outParamList[3];
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(pReturnAuthenticatorOut);

                IntPtr pDomBuffer = outParamList[6];
                DomBuffer = TypeMarshal.ToNullableStruct<_NETLOGON_DOMAIN_INFORMATION>(
                    pDomBuffer,
                    Level,
                    null,
                    null);

                retVal = outParamList[7].ToInt32();
            }

            pServerName.Dispose();
            pComputerName.Dispose();
            pAuthenticator.Dispose();
            pReturnAuthenticatorIn.Dispose();
            pWkstaBuffer.Dispose();

            return (NtStatus)retVal;
        }


        /// <summary>
        ///  The NetrServerPasswordSet2 method Supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. allows the client
        ///  to set a new clear text password for an account used
        ///  by the domain controller (as specified in section )
        ///  for setting up the secure channel from the client. A
        ///  domain member uses this function to periodically change
        ///  its machine account password. A PDC uses this function
        ///  to periodically change the trust password for all directly
        ///  trusted domains. By default, the period is 30 days
        ///  in windows_2000_server, windows_xp, windows_server_2003,
        ///  windows_vista, windows_server_2008, windows_7, and
        ///  windows_server_7. Opnum: 30 
        /// </summary>
        /// <param name="PrimaryName">
        ///  The custom RPC binding handle, as specified in section
        ///  .
        /// </param>
        /// <param name="AccountName">
        ///  The null-terminated Unicode string that contains the
        ///  name of the account whose password is being changed. In
        ///  windows, all machine account names are the name of
        ///  the machine with a $ (dollar sign) appended.
        /// </param>
        /// <param name="SecureChannelType">
        ///  An enumerated value that describes the secure channel
        ///  to be used for authentication, as specified in section
        ///  .
        /// </param>
        /// <param name="ComputerName">
        ///  The null-terminated Unicode string that contains the
        ///  NetBIOS name of the computer making the request.
        /// </param>
        /// <param name="Authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the encrypted
        ///  logon credential and a time stamp.
        /// </param>
        /// <param name="ReturnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="ClearNewPassword">
        ///  A pointer to an NL_TRUST_PASSWORD structure, as specified
        ///  in section , that contains the new password encrypted
        ///  as specified in Calling NetrServerPasswordSet2.
        /// </param>
        public NtStatus NetrServerPasswordSet2(
            string PrimaryName,
            string AccountName,
            _NETLOGON_SECURE_CHANNEL_TYPE SecureChannelType,
            string ComputerName,
            _NETLOGON_AUTHENTICATOR? Authenticator,
            out _NETLOGON_AUTHENTICATOR? ReturnAuthenticator,
            _NL_TRUST_PASSWORD? ClearNewPassword)
        {
            const ushort opnum = 30;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pPrimaryName = Marshal.StringToHGlobalUni(PrimaryName);
            SafeIntPtr pAccountName = Marshal.StringToHGlobalUni(AccountName);
            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);
            SafeIntPtr pAuthenticator = TypeMarshal.ToIntPtr(Authenticator);
            SafeIntPtr pClearNewPassword = TypeMarshal.ToIntPtr(ClearNewPassword);

            paramList = new Int3264[] {
                pPrimaryName,
                pAccountName,
                (uint)SecureChannelType,
                pComputerName,
                pAuthenticator,
                IntPtr.Zero,
                pClearNewPassword,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pReturnAuthenticator = outParamList[5];
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(pReturnAuthenticator);

                retVal = outParamList[7].ToInt32();
            }

            pPrimaryName.Dispose();
            pAccountName.Dispose();
            pComputerName.Dispose();
            pAuthenticator.Dispose();
            pClearNewPassword.Dispose();

            return (NtStatus)retVal;
        }


        /// <summary>
        ///  The NetrServerPasswordGet method Supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. allows a domain controller
        ///  to get a machine account password from the DC with
        ///  the PDC role in the domain. Opnum: 31 
        /// </summary>
        /// <param name="PrimaryName">
        ///  The custom RPC binding handle, as specified in section
        ///  .
        /// </param>
        /// <param name="AccountName">
        ///  A null-terminated Unicode string that contains the name
        ///  of the account to retrieve the password for. For machine
        ///  accounts, the account name is the machine name appended
        ///  with a "$" character.
        /// </param>
        /// <param name="AccountType">
        ///  A NETLOGON_SECURE_CHANNEL_TYPE enumerated value, as
        ///  specified in section , that describes the secure channel
        ///  to be used for authentication.
        /// </param>
        /// <param name="ComputerName">
        ///  A null-terminated Unicode string that contains the NetBIOS
        ///  name of the BDC making the call.
        /// </param>
        /// <param name="Authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the encrypted
        ///  logon credential and a time stamp.
        /// </param>
        /// <param name="ReturnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="EncryptedNtOwfPassword">
        ///  A pointer to an ENCRYPTED_NT_OWF_PASSWORD structure,
        ///  as specified in [MS-SAMR] section , that contains the
        ///  OWF password of the account.
        /// </param>
        public NtStatus NetrServerPasswordGet(
            string PrimaryName,
            string AccountName,
            _NETLOGON_SECURE_CHANNEL_TYPE AccountType,
            string ComputerName,
            _NETLOGON_AUTHENTICATOR? Authenticator,
            out _NETLOGON_AUTHENTICATOR? ReturnAuthenticator,
            out _NT_OWF_PASSWORD? EncryptedNtOwfPassword)
        {
            const ushort opnum = 31;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pPrimaryName = Marshal.StringToHGlobalUni(PrimaryName);
            SafeIntPtr pAccountName = Marshal.StringToHGlobalUni(AccountName);
            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);
            SafeIntPtr pAuthenticator = TypeMarshal.ToIntPtr(Authenticator);

            paramList = new Int3264[] {
                pPrimaryName,
                pAccountName,
                (uint)AccountType,
                pComputerName,
                pAuthenticator,
                IntPtr.Zero,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pReturnAuthenticator = outParamList[5];
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(pReturnAuthenticator);

                IntPtr pEncryptedNtOwfPassword = outParamList[6];
                EncryptedNtOwfPassword = TypeMarshal.ToNullableStruct<_NT_OWF_PASSWORD>(pEncryptedNtOwfPassword);

                retVal = outParamList[7].ToInt32();
            }

            pPrimaryName.Dispose();
            pAccountName.Dispose();
            pComputerName.Dispose();
            pAuthenticator.Dispose();

            return (NtStatus)retVal;
        }


        /// <summary>
        ///  The NetrLogonSendToSamSupported in windows_2000_server,
        ///  windows_xp, windows_server_2003, and windows_server_2008windows_server_2008,
        ///  windows_7, and windows_server_7. method allows a BDC
        ///  to forward user account password changes to the PDC.
        ///  It is used by the client to deliver an opaque buffer
        ///  to the SAM database ([MS-SAMR]) on the server side.
        ///  Opnum: 32 
        /// </summary>
        /// <param name="PrimaryName">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="ComputerName">
        ///  A null-terminated Unicode string that contains the NetBIOS
        ///  name of the client computer making the call.
        /// </param>
        /// <param name="Authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the client authenticator.
        /// </param>
        /// <param name="ReturnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="OpaqueBuffer">
        ///  A buffer to be passed to the Security Account Manager
        ///  (SAM) service on the PDC. The buffer is encrypted on
        ///  the wire.
        /// </param>
        /// <param name="OpaqueBufferSize">
        ///  The size, in bytes, of the OpaqueBuffer parameter.
        /// </param>
        public NtStatus NetrLogonSendToSam(
            string PrimaryName,
            string ComputerName,
            _NETLOGON_AUTHENTICATOR? Authenticator,
            out _NETLOGON_AUTHENTICATOR? ReturnAuthenticator,
            byte[] OpaqueBuffer,
            uint OpaqueBufferSize)
        {
            const ushort opnum = 32;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pPrimaryName = Marshal.StringToHGlobalUni(PrimaryName);
            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);
            SafeIntPtr pAuthenticator = TypeMarshal.ToIntPtr(Authenticator);
            SafeIntPtr pOpaqueBuffer = IntPtrUtility.ArrayToPtr(OpaqueBuffer);

            paramList = new Int3264[] {
                pPrimaryName,
                pComputerName,
                pAuthenticator,
                IntPtr.Zero,
                pOpaqueBuffer,
                OpaqueBufferSize,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
             RpceStubHelper.GetPlatform(),
            NrpcRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pReturnAuthenticator = outParamList[3];
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(pReturnAuthenticator);

                retVal = outParamList[6].ToInt32();
            }

            pPrimaryName.Dispose();
            pComputerName.Dispose();
            pAuthenticator.Dispose();
            pOpaqueBuffer.Dispose();

            return (NtStatus)retVal;
        }


        /// <summary>
        ///  The DsrAddressToSiteNamesW method Supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista,  windows_server_2008,
        ///  windows_7, and windows_server_7. translates a list
        ///  of socket addresses into their corresponding site names.
        ///  For information about the mapping from socket address
        ///  to subnet/site name, see [MS-ADTS] sections  and .
        ///  Opnum: 33 
        /// </summary>
        /// <param name="ComputerName">
        ///  The custom RPC binding handle (section) that represents
        ///  the connection to a domain controller.
        /// </param>
        /// <param name="EntryCount">
        ///  The number of socket addresses specified in SocketAddresses.
        ///  The maximum value for EntryCount is 32000.To avoid
        ///  large memory allocations, the number of 32,000 was
        ///  chosen as a reasonable limit for the maximum number
        ///  of socket addresses that this method accepts.
        /// </param>
        /// <param name="SocketAddresses">
        ///  An array of NL_SOCKET_ADDRESS structures (section )
        ///  that contains socket addresses to translate. The number
        ///  of addresses specified MUST be equal to EntryCount.
        /// </param>
        /// <param name="SiteNames">
        ///  A pointer to an NL_SITE_NAME_ARRAY structure (section
        ///  ) that contains a corresponding array of site names.
        ///  The number of entries returned is equal to EntryCount.
        ///  An entry is returned as NULL if the corresponding socket
        ///  address does not map to any site, or if the address
        ///  family of the socket address is not IPV4 or IPV6. The
        ///  mapping of IP addresses to sites is specified in [MS-ADTS]
        ///  section.
        /// </param>
        public NetApiStatus DsrAddressToSiteNamesW(
            string ComputerName,
            uint EntryCount,
            _NL_SOCKET_ADDRESS[] SocketAddresses,
            out _NL_SITE_NAME_ARRAY? SiteNames)
        {
            const ushort opnum = 33;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            //_RetVal = NdrClientCall2(
            //  ( PMIDL_STUB_DESC  )&logon_StubDesc,
            //  (PFORMAT_STRING) &ms2Dnrpc__MIDL_ProcFormatString.Format[724],
            //  ( unsigned char * )&ComputerName);
            //return ( NET_API_STATUS  )_RetVal.Simple;

            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);
            SafeIntPtr pSocketAddresses = TypeMarshal.ToIntPtr(SocketAddresses);

            paramList = new Int3264[] {
                pComputerName,
                EntryCount,
                Marshal.ReadIntPtr(pSocketAddresses),
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pSiteNames = outParamList[3];
                pSiteNames = Marshal.ReadIntPtr(pSiteNames);
                SiteNames = TypeMarshal.ToNullableStruct<_NL_SITE_NAME_ARRAY>(pSiteNames);

                retVal = outParamList[4].ToInt32();
            }

            pComputerName.Dispose();
            pSocketAddresses.Dispose();

            return (NetApiStatus)retVal;
        }


        /// <summary>
        ///  The DsrGetDcNameEx2 method returns information about
        ///  a domain controller in the specified domain and site. Supported
        ///  in windows_2000_server, windows_xp, windows_server_2003,
        ///  windows_vista, and windows_server_2008, windows_7,
        ///  and windows_server_7. The method will also verify that
        ///  the responding DC database contains an account if AccountName
        ///  is specified. The server that receives this call is
        ///  not required to be a DC. Opnum: 34 
        /// </summary>
        /// <param name="ComputerName">
        ///  The custom RPC binding handle (section ).
        /// </param>
        /// <param name="AccountName">
        ///  A null-terminated Unicode string that contains the name
        ///  of the account that MUST exist and be enabled on the
        ///  DC.
        /// </param>
        /// <param name="AllowableAccountControlBits">
        ///  A set of bit flags that list properties of the AccountName
        ///  account. A flag is TRUE (or set) if its value is equal
        ///  to 1. If the flag is set, then the account MUST have
        ///  that property; otherwise, the property is ignored.
        ///  The value is constructed from zero or more bit flags
        ///  from the following table.
        /// </param>
        /// <param name="DomainName">
        ///  A null-terminated Unicode string that contains the domain
        ///  name (3).
        /// </param>
        /// <param name="DomainGuid">
        ///  A pointer to a GUID structure that specifies the GUID
        ///  of the domain queried. If DomainGuid is not NULL and
        ///  the domain specified by DomainName cannot be found,
        ///  the DC locator attempts to locate a DC in the domain
        ///  that has the GUID specified by DomainGuid. This allows
        ///  renamed domains to be found by their GUID.
        /// </param>
        /// <param name="SiteName">
        ///  A null-terminated string that contains the name of the
        ///  site in which the DC MUST be located.
        /// </param>
        /// <param name="Flags">
        ///  A set of bit flags that provide additional data that
        ///  is used to process the request. A flag is TRUE (or
        ///  set) if its value is equal to 1. The value is constructed
        ///  from zero or more bit flags from the following table,
        ///  with the exceptions that bits D, E, and H cannot be
        ///  combined; S and R cannot be combined; and N and O cannot
        ///  be combined.
        /// </param>
        /// <param name="DomainControllerInfo">
        ///  A pointer to a DOMAIN_CONTROLLER_INFOW structure (section
        ///  ) containing data about the DC.
        /// </param>
        public NetApiStatus DsrGetDcNameEx2(
            string ComputerName,
            string AccountName,
            uint AllowableAccountControlBits,
            string DomainName,
            Guid? DomainGuid,
            string SiteName,
            uint Flags,
            out _DOMAIN_CONTROLLER_INFOW? DomainControllerInfo)
        {
            const ushort opnum = 34;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);
            SafeIntPtr pAccountName = Marshal.StringToHGlobalUni(AccountName);
            SafeIntPtr pDomainName = Marshal.StringToHGlobalUni(DomainName);
            SafeIntPtr pDomainGuid = TypeMarshal.ToIntPtr(DomainGuid);
            SafeIntPtr pSiteName = Marshal.StringToHGlobalUni(SiteName);

            paramList = new Int3264[] {
                pComputerName,
                pAccountName,
                AllowableAccountControlBits,
                pDomainName,
                pDomainGuid,
                pSiteName,
                Flags,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pDomainControllerInfo = outParamList[7];
                pDomainControllerInfo = Marshal.ReadIntPtr(pDomainControllerInfo);
                DomainControllerInfo = TypeMarshal.ToNullableStruct<_DOMAIN_CONTROLLER_INFOW>(pDomainControllerInfo);

                retVal = outParamList[8].ToInt32();
            }

            pComputerName.Dispose();
            pAccountName.Dispose();
            pDomainName.Dispose();
            pDomainGuid.Dispose();
            pSiteName.Dispose();

            return (NetApiStatus)retVal;
        }


        /// <summary>
        ///  The NetrLogonGetTimeServiceParentDomain methodSupported
        ///  in windows_2000_server, windows_xp and windows_server_2003.
        ///  returns the name of the parent domain of the current
        ///  domain. The domain name returned by this method is
        ///  suitable for passing into the NetrLogonGetTrustRid
        ///  method and NetrLogonComputeClientDigest method. Opnum
        ///  : 35 
        /// </summary>
        /// <param name="ServerName">
        ///  The custom RPC binding handle, as specified in section
        ///  .
        /// </param>
        /// <param name="DomainName">
        ///  A pointer to the buffer that receives the null-terminated
        ///  Unicode string that contains the name of the parent
        ///  domain. If the DNSdomain name is available, it is returned
        ///  through this parameter; otherwise, the NetBIOS domain
        ///  name is returned.
        /// </param>
        /// <param name="PdcSameSite">
        ///  A pointer to the buffer that receives the value that
        ///  indicates whether the PDC for the domainDomainName
        ///  is in the same site as the server specified by ServerName.
        ///  This value SHOULD The Netlogon client ignores this value
        ///  if ServerName is not a domain controller.  be ignored
        ///  if ServerName is not a domain controller.
        /// </param>
        public NetApiStatus NetrLogonGetTimeServiceParentDomain(
            string ServerName,
            out string DomainName,
            out PdcSameSite_Values? PdcSameSite)
        {
            const ushort opnum = 35;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pServerName = Marshal.StringToHGlobalUni(ServerName);

            paramList = new Int3264[] {
                pServerName,
                IntPtr.Zero,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pDomainName = outParamList[1];
                //wchar_t** DomainName
                pDomainName = Marshal.ReadIntPtr(pDomainName);
                DomainName = Marshal.PtrToStringUni(pDomainName);

                PdcSameSite = TypeMarshal.ToNullableStruct<PdcSameSite_Values>(outParamList[2]);

                retVal = outParamList[3].ToInt32();
            }

            pServerName.Dispose();

            return (NetApiStatus)retVal;
        }


        /// <summary>
        ///  The NetrEnumerateTrustedDomainsEx methodSupported in
        ///  windows_2000_server, windows_xp, windows_server_2003,
        ///  windows_vista, windows_server_2008, windows_7, and
        ///  windows_server_7. returns a list of trusted domains
        ///  from a specified server. This method extends NetrEnumerateTrustedDomains
        ///  by returning an array of domains in a more flexible
        ///  DS_DOMAIN_TRUSTSW structure, as specified in section
        ///  , rather than the array of strings in DOMAIN_NAME_BUFFER
        ///  structure, as specified in section. The array is returned
        ///  as part of the NETLOGON_TRUSTED_DOMAIN_ARRAY structure,
        ///  as specified in section. Opnum: 36 
        /// </summary>
        /// <param name="ServerName">
        ///  The custom RPC binding handle, as specified in section
        ///  .
        /// </param>
        /// <param name="Domains">
        ///  A pointer to a NETLOGON_TRUSTED_DOMAIN_ARRAY structure,
        ///  as specified in section , that contains an array of
        ///  DS_DOMAIN_TRUSTSW structures, as specified in section
        ///  , one for each trusted domain.
        /// </param>
        public NetApiStatus NetrEnumerateTrustedDomainsEx(
            string ServerName,
            out _NETLOGON_TRUSTED_DOMAIN_ARRAY? Domains)
        {
            const ushort opnum = 36;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pServerName = Marshal.StringToHGlobalUni(ServerName);

            paramList = new Int3264[] {
                pServerName,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pDomains = outParamList[1];
                Domains = TypeMarshal.ToNullableStruct<_NETLOGON_TRUSTED_DOMAIN_ARRAY>(pDomains);

                retVal = outParamList[2].ToInt32();
            }

            pServerName.Dispose();

            return (NetApiStatus)retVal;
        }


        /// <summary>
        ///  The DsrAddressToSiteNamesExW method Supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, windows_server_7. translates a list of socket
        ///  addresses into their corresponding site names and subnet
        ///  names. For information about the mapping from socket
        ///  address to subnet/site name, see [MS-ADTS] sections
        ///   and . Opnum: 37 
        /// </summary>
        /// <param name="ComputerName">
        ///  The custom RPC binding handle (section) that represents
        ///  the connection to a domain controller.
        /// </param>
        /// <param name="EntryCount">
        ///  The number of socket addresses specified in SocketAddresses.
        ///  The maximum value for EntryCount is 32000.To avoid
        ///  large memory allocations, the number of 32,000 was
        ///  chosen as a reasonable limit for the maximum number
        ///  of socket addresses that this method accepts.
        /// </param>
        /// <param name="SocketAddresses">
        ///  An array of NL_SOCKET_ADDRESS structures (section )
        ///  that contains socket addresses to translate. The number
        ///  of addresses specified MUST be equal to EntryCount.
        /// </param>
        /// <param name="SiteNames">
        ///  A pointer to an NL_SITE_NAME_EX_ARRAY structure (section
        ///  ) that contains an array of site names and an array
        ///  of subnet names that correspond to socket addresses
        ///  in SocketAddresses. The number of entries returned
        ///  is equal to EntryCount. An entry is returned as NULL
        ///  if the corresponding socket address does not map to
        ///  any site, or if the address family of the socket address
        ///  is not IPV4 or IPV6. The mapping of IP addresses to
        ///  sites is specified in [MS-ADTS] section.
        /// </param>
        public NetApiStatus DsrAddressToSiteNamesExW(
            string ComputerName,
            uint EntryCount,
            _NL_SOCKET_ADDRESS[] SocketAddresses,
            out _NL_SITE_NAME_EX_ARRAY? SiteNames)
        {
            const ushort opnum = 37;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            //CLIENT_CALL_RETURN _RetVal;
            //_RetVal = NdrClientCall2(
            //return ( NET_API_STATUS  )_RetVal.Simple;

            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);
            SafeIntPtr pSocketAddresses = TypeMarshal.ToIntPtr(SocketAddresses);

            paramList = new Int3264[] {
                pComputerName,
                EntryCount,
                Marshal.ReadIntPtr(pSocketAddresses),
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pSiteNames;
                pSiteNames = (IntPtr)outParamList[3];
                pSiteNames = Marshal.ReadIntPtr(pSiteNames);
                SiteNames = TypeMarshal.ToNullableStruct<_NL_SITE_NAME_EX_ARRAY>(pSiteNames);

                retVal = outParamList[4].ToInt32();
            }

            pComputerName.Dispose();
            pSocketAddresses.Dispose();

            return (NetApiStatus)retVal;
        }


        /// <summary>
        ///  The DsrGetDcSiteCoverageW method Supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. returns a list of
        ///  sites covered by a domain controller. Site coverage
        ///  is detailed in [MS-ADTS] section. Opnum: 38 
        /// </summary>
        /// <param name="ServerName">
        ///  The custom RPC binding handle (section) that represents
        ///  the connection to a DC.
        /// </param>
        /// <param name="SiteNames">
        ///  A pointer to an NL_SITE_NAME_ARRAY structure (section
        ///  ) that contains an array of site name strings.
        /// </param>
        public NetApiStatus DsrGetDcSiteCoverageW(
            string ServerName,
            out _NL_SITE_NAME_ARRAY? SiteNames)
        {
            const ushort opnum = 38;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pServerName = Marshal.StringToHGlobalUni(ServerName);

            paramList = new Int3264[] {
                pServerName,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pSiteNames = outParamList[1];
                pSiteNames = Marshal.ReadIntPtr(pSiteNames);
                SiteNames = TypeMarshal.ToNullableStruct<_NL_SITE_NAME_ARRAY>(pSiteNames);

                retVal = outParamList[2].ToInt32();
            }

            pServerName.Dispose();

            return (NetApiStatus)retVal;
        }


        /// <summary>
        ///  The NetrLogonSamLogonEx method Supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. provides an extension
        ///  to NetrLogonSamLogon that accepts an extra flags parameter
        ///  and uses Secure RPC ([MS-RPCE] section) instead of
        ///  Netlogon authenticators. This method handles logon
        ///  requests for the SAM accounts and allows for generic
        ///  pass-through authentication, as specified in section
        ///  . For more information about fields and structures
        ///  used by Netlogon pass-through methods, see section
        ///  . Opnum: 39 
        /// </summary>
        /// <param name="ContextHandle">
        ///  A primitive RPC handle that identifies a particular
        ///  client/server binding, as specified in section.
        /// </param>
        /// <param name="LogonServer">
        ///  The null-terminated Unicode string that contains the
        ///  NetBIOS name of the server that will handle the logon
        ///  request.
        /// </param>
        /// <param name="ComputerName">
        ///  The null-terminated Unicode string that contains the
        ///  NetBIOS name of the client computer sending the logon
        ///  request.
        /// </param>
        /// <param name="LogonLevel">
        ///  A NETLOGON_LOGON_INFO_CLASS structure, as specified
        ///  in section , that specifies the type of the logon information
        ///  passed in the LogonInformation parameter.
        /// </param>
        /// <param name="LogonInformation">
        ///  A pointer to a NETLOGON_LEVEL structure, as specified
        ///  in section , that describes the logon request information.
        /// </param>
        /// <param name="ValidationLevel">
        ///  A NETLOGON_VALIDATION_INFO_CLASS enumerated type, as
        ///  specified in section , that contains the validation
        ///  level requested by the client.
        /// </param>
        /// <param name="ValidationInformation">
        ///  A pointer to a NETLOGON_VALIDATION structure, as specified
        ///  in section , that describes the user validation information
        ///  returned to the client. The type of the NETLOGON_VALIDATION
        ///  used is determined by the value of the ValidationLevel
        ///  parameter.
        /// </param>
        /// <param name="Authoritative">
        ///  A pointer to a char value that represents a Boolean
        ///  condition. FALSE is indicated by the value 0x00, and
        ///  TRUE SHOULDwindows uses the value 0x01 as the representation
        ///  of TRUE and 0x00 for FALSE. be indicated by the value
        ///  0x01 and MAY also be indicated by any nonzero value.
        ///  This Boolean value indicates whether the validation
        ///  information is final. This field is necessary because
        ///  the request might be forwarded through multiple servers.
        ///  The value TRUE indicates that the validation information
        ///  is final and MUST remain unchanged. The Authoritative
        ///  parameter indicates whether the response to this call
        ///  is final or if the same request can be sent to another
        ///  server. The value SHOULD be set to FALSE if the server
        ///  encounters a transient error, and the client can resend
        ///  the request to another server. If the same request
        ///  is known to fail in all subsequent requests, the server
        ///  SHOULD return TRUE.
        /// </param>
        /// <param name="ExtraFlags">
        ///  A pointer to a set of bit flags that specify delivery
        ///  settings. A flag is TRUE (or set) if its value is equal
        ///  to 1. Output flags MUST be the same as input. The value
        ///  is constructed from zero or more bit flags from the
        ///  following table.
        /// </param>
        public NtStatus NetrLogonSamLogonEx(
            IntPtr ContextHandle,
            string LogonServer,
            string ComputerName,
            _NETLOGON_LOGON_INFO_CLASS LogonLevel,
            _NETLOGON_LEVEL? LogonInformation,
            _NETLOGON_VALIDATION_INFO_CLASS ValidationLevel,
            out _NETLOGON_VALIDATION? ValidationInformation,
            out byte? Authoritative,
            ref uint? ExtraFlags)
        {
            const ushort opnum = 39;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pLogonServer = Marshal.StringToHGlobalUni(LogonServer);
            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);
            SafeIntPtr pLogonInformation = TypeMarshal.ToIntPtr(LogonInformation, LogonLevel, null, null);
            SafeIntPtr pExtraFlags = TypeMarshal.ToIntPtr(ExtraFlags);

            paramList = new Int3264[] {
                pLogonServer,
                pComputerName,
                (uint)LogonLevel,
                pLogonInformation,
                (uint)ValidationLevel,
                IntPtr.Zero,
                IntPtr.Zero,
                pExtraFlags,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pValidationInformation = outParamList[5];
                ValidationInformation = TypeMarshal.ToNullableStruct<_NETLOGON_VALIDATION>(
                    pValidationInformation,
                    ValidationLevel,
                    null,
                    null);

                Authoritative = TypeMarshal.ToNullableStruct<byte>(outParamList[6]);

                ExtraFlags = TypeMarshal.ToNullableStruct<uint>(outParamList[7]);

                retVal = outParamList[8].ToInt32();
            }

            pLogonServer.Dispose();
            pComputerName.Dispose();
            pLogonInformation.Dispose();
            pExtraFlags.Dispose();

            return (NtStatus)retVal;
        }


        /// <summary>
        ///  The DsrEnumerateDomainTrusts method Supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. returns an enumerated
        ///  list of domain trusts, filtered by a set of flags, from
        ///  the specified server. Opnum: 40 
        /// </summary>
        /// <param name="ServerName">
        ///  The custom RPC binding handle, as specified in section
        ///  .
        /// </param>
        /// <param name="Flags">
        ///  A set of bit flags that specify properties that MUST
        ///  be true for a domain trust to be part of the returned
        ///  domain name list. A flag is TRUE (or set) if its value
        ///  is equal to 1. Flags MUST contain one or more of the
        ///  following bits.
        /// </param>
        /// <param name="Domains">
        ///  A pointer to a NETLOGON_TRUSTED_DOMAIN_ARRAY structure,
        ///  as specified in section , that contains a list of trusted
        ///  domains.
        /// </param>
        public NetApiStatus DsrEnumerateDomainTrusts(
            string ServerName,
            uint Flags,
            out _NETLOGON_TRUSTED_DOMAIN_ARRAY? Domains)
        {
            const ushort opnum = 40;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pServerName = Marshal.StringToHGlobalUni(ServerName);

            paramList = new Int3264[] {
                pServerName,
                Flags,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pDomains = outParamList[2];
                Domains = TypeMarshal.ToNullableStruct<_NETLOGON_TRUSTED_DOMAIN_ARRAY>(pDomains);

                retVal = outParamList[3].ToInt32();
            }

            pServerName.Dispose();

            return (NetApiStatus)retVal;
        }


        /// <summary>
        ///  The DsrDeregisterDnsHostRecords method Supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. deletes all of the
        ///  DNS SRV records registered by a specified domain controller.
        ///  For the list of SRV records that a domain registers,
        ///  see [MS-ADTS] section , SRV Records Registered by DC.
        ///  Opnum: 41 
        /// </summary>
        /// <param name="ServerName">
        ///  The custom RPC binding handle, as specified in section
        ///  , that represents the connection to the DC.
        /// </param>
        /// <param name="DnsDomainName">
        ///  A null-terminated Unicode string that specifies the
        ///  fully qualified domain name (FQDN) (2).
        /// </param>
        /// <param name="DomainGuid">
        ///  A pointer to the domainGUID. If the value is not NULL,
        ///  the DNS SRV record of type _ldap._tcp.DomainGuid.domains._msdcs.DnsDomainName
        ///  is also deregistered.
        /// </param>
        /// <param name="DsaGuid">
        ///  A pointer to the objectGUID of the DC's TDSDSA object.
        ///  For information about the TDSDSA object, see [MS-ADTS]
        ///  section. If the value is not NULL, the CNAME [RFC1035]
        ///  record of the domain in the form of DsaGuid._msdcs.DnsDomainName
        ///  is also deregistered.
        /// </param>
        /// <param name="DnsHostName">
        ///  A null-terminated Unicode string that specifies the
        ///  fully qualified domain name (FQDN) (1) of the DC whose
        ///  records are being deregistered. If the value is NULL,
        ///  ERROR_INVALID_PARAMETER is returned.
        /// </param>
        public NetApiStatus DsrDeregisterDnsHostRecords(
            string ServerName,
            string DnsDomainName,
            Guid? DomainGuid,
            Guid? DsaGuid,
            string DnsHostName)
        {
            const ushort opnum = 41;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pServerName = Marshal.StringToHGlobalUni(ServerName);
            SafeIntPtr pDnsDomainName = Marshal.StringToHGlobalUni(DnsDomainName);
            SafeIntPtr pDnsHostName = Marshal.StringToHGlobalUni(DnsHostName);
            SafeIntPtr pDomainGuid = TypeMarshal.ToIntPtr(DomainGuid);
            SafeIntPtr pDsaGuid = TypeMarshal.ToIntPtr(DsaGuid);

            paramList = new Int3264[] {
                pServerName,
                pDnsDomainName,
                pDomainGuid,
                pDsaGuid,
                pDnsHostName,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                retVal = outParamList[5].ToInt32();
            }

            pServerName.Dispose();
            pDnsDomainName.Dispose();
            pDnsHostName.Dispose();
            pDomainGuid.Dispose();
            pDsaGuid.Dispose();

            return (NetApiStatus)retVal;
        }


        /// <summary>
        ///  The NetrServerTrustPasswordsGet method Supported in windows_2000_server_sp4,
        ///  windows_xp, and windows_server_2003, windows_vista,
        ///  windows_server_2008, windows_7, and windows_server_7.
        ///  returns the encrypted current and previous passwords
        ///  for an account in the domain. This method is called
        ///  by a client to retrieve the current and previous account
        ///  passwords from a domain controller. The account name
        ///  requested MUST be the name used when the secure channel
        ///  was created, unless the method is called on a PDC by
        ///  a DC, in which case it can be any valid account name.
        ///  Opnum: 42 
        /// </summary>
        /// <param name="TrustedDcName">
        ///  The custom RPC binding handle, as specified in section
        ///  .
        /// </param>
        /// <param name="AccountName">
        ///  The null-terminated Unicode string that contains the
        ///  name of the client account in the domain for which
        ///  the trust password MUST be returned. In windows, all
        ///  machine account names are the name of the machine with
        ///  a $ (dollar sign) appended.
        /// </param>
        /// <param name="SecureChannelType">
        ///  A NETLOGON_SECURE_CHANNEL_TYPE enumerated value, as
        ///  specified in section , that indicates the type of the
        ///  secure channel being established by this call.
        /// </param>
        /// <param name="ComputerName">
        ///  The null-terminated Unicode string that contains the
        ///  NetBIOS name of the client computer.
        /// </param>
        /// <param name="Authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the client authenticator.
        /// </param>
        /// <param name="ReturnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="EncryptedNewOwfPassword">
        ///  A pointer to an ENCRYPTED_NT_OWF_PASSWORD structure,
        ///  as specified in section , that contains the NTOWFv1
        ///  (as specified in NTLM v1 Authentication in [MS-NLMP]
        ///  section) of the current password, encrypted as specified
        ///  in [MS-SAMR] section , Encrypting an NT Hash or LM
        ///  Hash Value with a Specified Key. The session key is
        ///  the specified 16-byte key that is used to derive the
        ///  password's keys. The specified 16-byte key uses the
        ///  16-byte value process, as specified in [MS-SAMR] section
        ///  .
        /// </param>
        /// <param name="EncryptedOldOwfPassword">
        ///  A pointer to an ENCRYPTED_NT_OWF_PASSWORD structure,
        ///  as specified in section , that contains the NTOWFv1
        ///  (as specified in NTLM v1 Authentication in [MS-NLMP]
        ///  section) of the previous password, encrypted as specified
        ///  in [MS-SAMR] section , Encrypting an NT Hash or LM
        ///  Hash Value with a Specified Key. The session key is
        ///  the specified 16-byte key that is used to derive the
        ///  password's keys. The specified 16-byte key uses the
        ///  16-byte value process, as specified in [MS-SAMR] section
        ///  .
        /// </param>
        public NtStatus NetrServerTrustPasswordsGet(
            string TrustedDcName,
            string AccountName,
            _NETLOGON_SECURE_CHANNEL_TYPE SecureChannelType,
            string ComputerName,
            _NETLOGON_AUTHENTICATOR? Authenticator,
            out _NETLOGON_AUTHENTICATOR? ReturnAuthenticator,
            out _NT_OWF_PASSWORD? EncryptedNewOwfPassword,
            out _NT_OWF_PASSWORD? EncryptedOldOwfPassword)
        {
            const ushort opnum = 42;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pTrustedDcName = Marshal.StringToHGlobalUni(TrustedDcName);
            SafeIntPtr pAccountName = Marshal.StringToHGlobalUni(AccountName);
            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);
            SafeIntPtr pAuthenticator = TypeMarshal.ToIntPtr(Authenticator);

            paramList = new Int3264[] {
                pTrustedDcName,
                pAccountName,
                (uint)SecureChannelType,
                pComputerName,
                pAuthenticator,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pReturnAuthenticator = outParamList[5];
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(pReturnAuthenticator);

                IntPtr pEncryptedNewOwfPassword = outParamList[6];
                EncryptedNewOwfPassword = TypeMarshal.ToNullableStruct<_NT_OWF_PASSWORD>(pEncryptedNewOwfPassword);

                IntPtr pEncryptedOldOwfPassword = outParamList[7];
                EncryptedOldOwfPassword = TypeMarshal.ToNullableStruct<_NT_OWF_PASSWORD>(pEncryptedOldOwfPassword);

                retVal = outParamList[8].ToInt32();
            }

            pTrustedDcName.Dispose();
            pAccountName.Dispose();
            pComputerName.Dispose();
            pAuthenticator.Dispose();

            return (NtStatus)retVal;
        }


        /// <summary>
        ///  The DsrGetForestTrustInformation methodSupported in
        ///  windows_xpwindows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. retrieves the trust
        ///  information for the forest of the specified domain
        ///  controller, or for a forest trusted by the forest of
        ///  the specified DC. Opnum: 43 
        /// </summary>
        /// <param name="ServerName">
        ///  The custom RPC binding handle, as specified in section
        ///  .
        /// </param>
        /// <param name="TrustedDomainName">
        ///  The optional null-terminated Unicode string that contains
        ///  the DNS or NetBIOS name of the trusted domain for which
        ///  the forest trust information is to be gathered.
        /// </param>
        /// <param name="Flags">
        ///  A set of bit flags that specify additional applications
        ///  for the forest trust information. A flag is TRUE (or
        ///  set) if its value is equal to 1.
        /// </param>
        /// <param name="ForestTrustInfo">
        ///  A pointer to an LSA_FOREST_TRUST_INFORMATION structure,
        ///  as specified in [MS-LSAD] section , that contains data
        ///  for each foresttrust.
        /// </param>
        public NetApiStatus DsrGetForestTrustInformation(
            string ServerName,
            string TrustedDomainName,
            uint Flags,
            out _LSA_FOREST_TRUST_INFORMATION? ForestTrustInfo)
        {
            const ushort opnum = 43;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pServerName = Marshal.StringToHGlobalUni(ServerName);
            SafeIntPtr pTrustedDomainName = Marshal.StringToHGlobalUni(TrustedDomainName);

            paramList = new Int3264[] {
                pServerName,
                pTrustedDomainName,
                Flags,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pForestTrustInfo = outParamList[3];
                pForestTrustInfo = Marshal.ReadIntPtr(pForestTrustInfo);
                ForestTrustInfo = TypeMarshal.ToNullableStruct<_LSA_FOREST_TRUST_INFORMATION>(pForestTrustInfo);

                retVal = outParamList[4].ToInt32();
            }

            pServerName.Dispose();
            pTrustedDomainName.Dispose();

            return (NetApiStatus)retVal;
        }


        /// <summary>
        ///  The NetrGetForestTrustInformationSupported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, and
        ///  windows_server_2008, windows_7, and windows_server_7.
        ///  method retrieves the trust information for the forest
        ///  of which the member's domain is itself a member. Opnum
        ///  : 44 
        /// </summary>
        /// <param name="ServerName">
        ///  The custom RPC binding handle, as specified in section
        ///  .
        /// </param>
        /// <param name="ComputerName">
        ///  The null-terminated Unicode string that contains the
        ///  client computer NetBIOS name.
        /// </param>
        /// <param name="Authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the client authenticator.
        /// </param>
        /// <param name="ReturnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="Flags">
        ///  MUST be set to zero and MUST be ignored on receipt.
        /// </param>
        /// <param name="ForestTrustInfo">
        ///  A pointer to an LSA_FOREST_TRUST_INFORMATION structure,
        ///  as specified in [MS-LSAD] section , that contains data
        ///  for each foresttrust.
        /// </param>
        public NtStatus NetrGetForestTrustInformation(
            string ServerName,
            string ComputerName,
            _NETLOGON_AUTHENTICATOR? Authenticator,
            out _NETLOGON_AUTHENTICATOR? ReturnAuthenticator,
            uint Flags,
            out _LSA_FOREST_TRUST_INFORMATION? ForestTrustInfo)
        {
            const ushort opnum = 44;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pServerName = Marshal.StringToHGlobalUni(ServerName);
            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);
            SafeIntPtr pAuthenticator = TypeMarshal.ToIntPtr(Authenticator);

            paramList = new Int3264[] {
                pServerName,
                pComputerName,
                pAuthenticator,
                IntPtr.Zero,
                Flags,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pReturnAuthenticator = outParamList[3];
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(pReturnAuthenticator);

                IntPtr pForestTrustInfo = outParamList[5];
                pForestTrustInfo = Marshal.ReadIntPtr(pForestTrustInfo);
                ForestTrustInfo = TypeMarshal.ToNullableStruct<_LSA_FOREST_TRUST_INFORMATION>(pForestTrustInfo);

                retVal = outParamList[6].ToInt32();
            }

            pServerName.Dispose();
            pComputerName.Dispose();
            pAuthenticator.Dispose();

            return (NtStatus)retVal;
        }


        /// <summary>
        ///  The NetrLogonSamLogonWithFlags method Supported in windows_xpwindows_server_2003,
        ///  windows_vista, windows_server_2008, windows_7, and
        ///  windows_server_7. handles logon requests for the SAM
        ///  accounts. Opnum: 45 
        /// </summary>
        /// <param name="LogonServer">
        ///  The custom RPC binding handle, as specified in section
        ///  .
        /// </param>
        /// <param name="ComputerName">
        ///  The Unicode string that contains the NetBIOS name of
        ///  the client computer calling this method.
        /// </param>
        /// <param name="Authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the client authenticator.
        /// </param>
        /// <param name="ReturnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="LogonLevel">
        ///  A NETLOGON_LOGON_INFO_CLASS structure, as specified
        ///  in section , that specifies the type of logon information
        ///  passed in the LogonInformation parameter.
        /// </param>
        /// <param name="LogonInformation">
        ///  A pointer to a NETLOGON_LEVEL structure, as specified
        ///  in section , that describes the logon request information.
        /// </param>
        /// <param name="ValidationLevel">
        ///  A NETLOGON_VALIDATION_INFO_CLASS enumerated type, as
        ///  specified in section , that contains the validation
        ///  level requested by the client.
        /// </param>
        /// <param name="ValidationInformation">
        ///  A pointer to a NETLOGON_VALIDATION structure, as specified
        ///  in section , that describes the user validation information
        ///  returned to the client. The type of the NETLOGON_VALIDATION
        ///  used is determined by the value of the ValidationLevel
        ///  parameter.
        /// </param>
        /// <param name="Authoritative">
        ///  A pointer to a char value representing a Boolean condition.
        ///  FALSE is indicated by the value 0x00; TRUE SHOULD be
        ///  indicated by the value 0x01 and MAY also be indicated
        ///  by any nonzero value. Windows uses the value of 0x01
        ///  as the representation of TRUE and 0x00 for FALSE. This
        ///  Boolean value indicates whether the validation information
        ///  is final. This field is necessary because the request
        ///  might be forwarded through multiple servers. A value
        ///  of TRUE indicates that the validation information is
        ///  final and MUST remain unchanged.
        /// </param>
        /// <param name="ExtraFlags">
        ///  A pointer to a set of bit flags that specify delivery
        ///  settings. A flag is TRUE (or set) if its value is equal
        ///  to 1. The value is constructed from zero or more bit
        ///  flags from the following table.
        /// </param>
        public NtStatus NetrLogonSamLogonWithFlags(
            string LogonServer,
            string ComputerName,
            _NETLOGON_AUTHENTICATOR? Authenticator,
            ref _NETLOGON_AUTHENTICATOR? ReturnAuthenticator,
            _NETLOGON_LOGON_INFO_CLASS LogonLevel,
            _NETLOGON_LEVEL? LogonInformation,
            _NETLOGON_VALIDATION_INFO_CLASS ValidationLevel,
            out _NETLOGON_VALIDATION? ValidationInformation,
            out byte? Authoritative,
            ref uint? ExtraFlags)
        {
            const ushort opnum = 45;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pLogonServer = Marshal.StringToHGlobalUni(LogonServer);
            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);
            SafeIntPtr pAuthenticator = TypeMarshal.ToIntPtr(Authenticator);
            SafeIntPtr pReturnAuthenticatorIn = TypeMarshal.ToIntPtr(ReturnAuthenticator);
            SafeIntPtr pLogonInformation = TypeMarshal.ToIntPtr(LogonInformation, LogonLevel, null, null);
            SafeIntPtr pExtraFlags = TypeMarshal.ToIntPtr(ExtraFlags);

            paramList = new Int3264[] {
                pLogonServer,
                pComputerName,
                pAuthenticator,
                pReturnAuthenticatorIn,
                (uint)LogonLevel,
                pLogonInformation,
                (uint)ValidationLevel,
                IntPtr.Zero,
                IntPtr.Zero,
                pExtraFlags,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pReturnAuthenticatorOut = outParamList[3];
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(pReturnAuthenticatorOut);

                IntPtr pValidationInformation = outParamList[7];
                ValidationInformation = TypeMarshal.ToNullableStruct<_NETLOGON_VALIDATION>(
                    pValidationInformation,
                    ValidationLevel,
                    null,
                    null);

                Authoritative = TypeMarshal.ToNullableStruct<byte>(outParamList[8]);

                ExtraFlags = TypeMarshal.ToNullableStruct<uint>(outParamList[9]);

                retVal = outParamList[10].ToInt32();
            }

            pLogonServer.Dispose();
            pComputerName.Dispose();
            pAuthenticator.Dispose();
            pReturnAuthenticatorIn.Dispose();
            pLogonInformation.Dispose();
            pExtraFlags.Dispose();

            return (NtStatus)retVal;
        }


        /// <summary>
        ///  The NetrServerGetTrustInfo method Supported in windows_xp
        ///  and windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, windows_server_7. returns an information
        ///  block from a specified server. The information includes
        ///  encrypted current and previous passwords for a particular
        ///  account and additional trust data. The account name
        ///  requested MUST be the name used when the secure channel
        ///  was created, unless the method is called on a PDC by
        ///  a domain controller, in which case it can be any valid
        ///  account name. Opnum: 46 
        /// </summary>
        /// <param name="TrustedDcName">
        ///  The custom RPC binding handle, as specified in section
        ///  .
        /// </param>
        /// <param name="AccountName">
        ///  The null-terminated Unicode string that contains the
        ///  name of the client account in the domain.
        /// </param>
        /// <param name="SecureChannelType">
        ///  A NETLOGON_SECURE_CHANNEL_TYPE enumerated value, as
        ///  specified in section , that indicates the type of the
        ///  secure channel being established by this call.
        /// </param>
        /// <param name="ComputerName">
        ///  The null-terminated Unicode string that contains the
        ///  NetBIOS name of the client computer, for which the
        ///  trust information MUST be returned.
        /// </param>
        /// <param name="Authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the client authenticator.
        /// </param>
        /// <param name="ReturnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="EncryptedNewOwfPassword">
        ///  A pointer to an ENCRYPTED_NT_OWF_PASSWORD structure,
        ///  as specified in section , that contains the NTOWFv1
        ///  (as specified in NTLM v1 Authentication in [MS-NLMP]
        ///  section) of the current password, encrypted as specified
        ///  in [MS-SAMR] section , Encrypting an NT Hash or LM
        ///  Hash Value with a Specified Key. The session key is
        ///  the specified 16-byte key that is used to derive its
        ///  keys via the 16-byte value process, as specified in
        ///  [MS-SAMR] section.
        /// </param>
        /// <param name="EncryptedOldOwfPassword">
        ///  A pointer to an ENCRYPTED_NT_OWF_PASSWORD structure,
        ///  as specified in section , that contains the NTOWFv1
        ///  (as specified in NTLM v1 Authentication in [MS-NLMP]
        ///  section) of the old password, encrypted as specified
        ///  in [MS-SAMR] section , Encrypting an NT Hash or LM
        ///  Hash Value with a Specified Key. The session key is
        ///  the specified 16-byte key that is used to derive its
        ///  keys via the 16-byte value process, as specified in
        ///  [MS-SAMR] section.
        /// </param>
        /// <param name="TrustInfo">
        ///  A pointer to an NL_GENERIC_RPC_DATA structure, as specified
        ///  in section , that contains a block of generic RPC data
        ///  with trust information for the specified server.
        /// </param>
        public NtStatus NetrServerGetTrustInfo(
            string TrustedDcName,
            string AccountName,
            _NETLOGON_SECURE_CHANNEL_TYPE SecureChannelType,
            string ComputerName,
            _NETLOGON_AUTHENTICATOR? Authenticator,
            out _NETLOGON_AUTHENTICATOR? ReturnAuthenticator,
            out _NT_OWF_PASSWORD? EncryptedNewOwfPassword,
            out _NT_OWF_PASSWORD? EncryptedOldOwfPassword,
            out _NL_GENERIC_RPC_DATA? TrustInfo)
        {
            const ushort opnum = 46;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pTrustedDcName = Marshal.StringToHGlobalUni(TrustedDcName);
            SafeIntPtr pAccountName = Marshal.StringToHGlobalUni(AccountName);
            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);
            SafeIntPtr pAuthenticator = TypeMarshal.ToIntPtr(Authenticator);

            paramList = new Int3264[] {
                pTrustedDcName,
                pAccountName,
                (uint)SecureChannelType,
                pComputerName,
                pAuthenticator,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pReturnAuthenticator = outParamList[5];
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(pReturnAuthenticator);

                IntPtr pEncryptedNewOwfPassword = outParamList[6];
                EncryptedNewOwfPassword = TypeMarshal.ToNullableStruct<_NT_OWF_PASSWORD>(pEncryptedNewOwfPassword);

                IntPtr pEncryptedOldOwfPassword = outParamList[7];
                EncryptedOldOwfPassword = TypeMarshal.ToNullableStruct<_NT_OWF_PASSWORD>(pEncryptedOldOwfPassword);

                IntPtr pTrustInfo = outParamList[8];
                pTrustInfo = Marshal.ReadIntPtr(pTrustInfo);
                TrustInfo = TypeMarshal.ToNullableStruct<_NL_GENERIC_RPC_DATA>(pTrustInfo);

                retVal = outParamList[9].ToInt32();
            }

            pTrustedDcName.Dispose();
            pAccountName.Dispose();
            pComputerName.Dispose();
            pAuthenticator.Dispose();

            return (NtStatus)retVal;
        }


        /// <summary>
        ///  OpnumUnused47 method. Opnum: 47 
        /// </summary>
        /// <param name="ContextHandle">
        ///  A primitive RPC handle that identifies a particular
        ///  client/server binding.
        /// </param>
        public NtStatus OpnumUnused47(IntPtr ContextHandle)
        {
            const ushort opnum = 47;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            paramList = new Int3264[] {
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                retVal = outParamList[0].ToInt32();
            }

            return (NtStatus)retVal;
        }


        /// <summary>
        /// The DsrUpdateReadOnlyServerDnsRecords method will allow an RODC to send a control 
        /// command to a normal (writable) DC for site-specific and CName types of DNS records 
        /// update. For registration, site-specific records should be for the site in which 
        /// RODC resides. For the types of DNS records, see [MS-ADTS] section 7.3.2. Opnum: 48
        /// </summary>
        /// <param name="ServerName">
        /// The custom RPC binding handle (as specified in section 3.5.5.1) that represents 
        /// the connection to the normal (writable) DC.
        /// </param>
        /// <param name="ComputerName">
        /// A null-terminated Unicode string that contains the client computer NetBIOS name.
        /// </param>
        /// <param name="Authenticator">
        /// A pointer to a NETLOGON_AUTHENTICATOR structure (as specified in section 2.2.1.1.5) 
        /// that contains the client authenticator that will be used to authenticate the client.
        /// </param>
        /// <param name="ReturnAuthenticator">
        /// A pointer to a NETLOGON_AUTHENTICATOR structure that contains the server return 
        /// authenticator.
        /// </param>
        /// <param name="SiteName">
        /// A pointer to a null-terminated Unicode string that contains the site name where 
        /// the RODC resides.
        /// </param>
        /// <param name="DnsTtl">
        /// The Time To Live value, in seconds, for DNS records.
        /// </param>
        /// <param name="DnsNames">
        /// A pointer to an NL_DNS_NAME_INFO_ARRAY (section 2.2.1.2.6) structure that contains 
        /// an array of NL_DNS_NAME_INFO structures.
        /// </param>
        public NtStatus DsrUpdateReadOnlyServerDnsRecords(
            string ServerName,
            string ComputerName,
            _NETLOGON_AUTHENTICATOR? Authenticator,
            out _NETLOGON_AUTHENTICATOR? ReturnAuthenticator,
            string SiteName,
            uint DnsTtl,
            ref _NL_DNS_NAME_INFO_ARRAY? DnsNames)
        {
            const ushort opnum = 48;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pServerName = Marshal.StringToHGlobalUni(ServerName);
            SafeIntPtr pComputerName = Marshal.StringToHGlobalUni(ComputerName);
            SafeIntPtr pAuthenticator = TypeMarshal.ToIntPtr(Authenticator);
            SafeIntPtr pSiteName = Marshal.StringToHGlobalUni(SiteName);
            SafeIntPtr pDnsNamesIn = TypeMarshal.ToIntPtr(DnsNames);

            paramList = new Int3264[] {
                pServerName,
                pComputerName,
                pAuthenticator,
                IntPtr.Zero,
                pSiteName,
                DnsTtl,
                pDnsNamesIn,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pReturnAuthenticator = outParamList[3];
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(pReturnAuthenticator);

                IntPtr pDnsNamesOut = outParamList[6];
                DnsNames = TypeMarshal.ToNullableStruct<_NL_DNS_NAME_INFO_ARRAY>(pDnsNamesOut);

                retVal = outParamList[7].ToInt32();
            }

            pServerName.Dispose();
            pComputerName.Dispose();
            pAuthenticator.Dispose();
            pSiteName.Dispose();
            pDnsNamesIn.Dispose();

            return (NtStatus)retVal;
        }


        /// <summary>
        /// When an RODC receives either the NetrServerAuthenticate3 method or the 
        /// NetrLogonGetDomainInfo method with updates requested, it invokes this method 
        /// on a normal (writable) DC to update to a client's computer account object in 
        /// Active Directory.
        /// </summary>
        /// <param name="PrimaryName">
        /// The custom RPC binding handle, as specified in section 3.5.5.1.
        /// </param>
        /// <param name="ChainedFromServerName">
        /// The null-terminated Unicode string that contains the name of the read-only 
        /// DC that issues the request.
        /// </param>
        /// <param name="ChainedForClientName">
        /// The null-terminated Unicode string that contains the name of the client 
        /// computer that called NetrServerAuthenticate3 or NetrLogonGetDomainInfo on 
        /// the RODC.
        /// </param>
        /// <param name="Authenticator">
        /// A pointer to a NETLOGON_AUTHENTICATOR structure that contains the client 
        /// authenticator.
        /// </param>
        /// <param name="ReturnAuthenticator">
        /// A pointer to a NETLOGON_AUTHENTICATOR structure that contains the server 
        /// return authenticator.
        /// </param>
        /// <param name="dwInVersion">
        /// One of the NL_IN_CHAIN_SET_CLIENT_ATTRIBUTES union types selected based on 
        /// the value of the pmsgIn field. The value MUST be 1.
        /// </param>
        /// <param name="pmsgIn">
        /// A pointer to an NL_IN_CHAIN_SET_CLIENT_ATTRIBUTES_V1 structure that contains 
        /// the values to update on the client's computer account object in Active 
        /// Directory on the normal (writable) DC.
        /// </param>
        /// <param name="pdwOutVersion">
        /// A pointer to one of the NL_OUT_CHAIN_SET_CLIENT_ATTRIBUTES union types selected 
        /// based on the value of the pmsgIn field. The value MUST be 1.
        /// </param>
        /// <param name="pmsgOut">
        /// A pointer to an NL_OUT_CHAIN_SET_CLIENT_ATTRIBUTES_V1 structure that contains 
        /// information on the client workstation and the writable domain controller. For 
        /// how it is populated by the server, see below.
        /// </param>
        public NtStatus NetrChainSetClientAttributes(
            string PrimaryName,
            string ChainedFromServerName,
            string ChainedForClientName,
            _NETLOGON_AUTHENTICATOR? Authenticator,
            ref _NETLOGON_AUTHENTICATOR? ReturnAuthenticator,
            uint dwInVersion,
            NL_IN_CHAIN_SET_CLIENT_ATTRIBUTES? pmsgIn,
            ref uint? pdwOutVersion,
            ref NL_OUT_CHAIN_SET_CLIENT_ATTRIBUTES? pmsgOut)
        {
            const ushort opnum = 49;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr pPrimaryName = Marshal.StringToHGlobalUni(PrimaryName);
            SafeIntPtr pChainedFromServerName = Marshal.StringToHGlobalUni(ChainedFromServerName);
            SafeIntPtr pChainedForClientName = Marshal.StringToHGlobalUni(ChainedForClientName);
            SafeIntPtr pAuthenticator = TypeMarshal.ToIntPtr(Authenticator);
            SafeIntPtr pReturnAuthenticatorIn = TypeMarshal.ToIntPtr(ReturnAuthenticator);
            SafeIntPtr ppMsgIn = TypeMarshal.ToIntPtr(pmsgIn, dwInVersion, null, null);
            SafeIntPtr ppdwOutVersion = TypeMarshal.ToIntPtr(pdwOutVersion);
            SafeIntPtr ppMsgOutIn = TypeMarshal.ToIntPtr(pmsgOut, pdwOutVersion.Value, null, null);

            paramList = new Int3264[] {
                pPrimaryName,
                pChainedFromServerName,
                pChainedForClientName,
                pAuthenticator,
                pReturnAuthenticatorIn,
                dwInVersion,
                ppMsgIn,
                ppdwOutVersion,
                ppMsgOutIn,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                     RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr pReturnAuthenticatorOut = outParamList[4];
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(pReturnAuthenticatorOut);

                pdwOutVersion = TypeMarshal.ToNullableStruct<uint>(outParamList[7]);

                IntPtr ppMsgOutOut = outParamList[8];
                pmsgOut = TypeMarshal.ToNullableStruct<NL_OUT_CHAIN_SET_CLIENT_ATTRIBUTES>(
                    ppMsgOutOut,
                    pdwOutVersion.Value,
                    null,
                    null);

                retVal = outParamList[9].ToInt32();
            }

            pPrimaryName.Dispose();
            pChainedFromServerName.Dispose();
            pChainedForClientName.Dispose();
            pAuthenticator.Dispose();
            pReturnAuthenticatorIn.Dispose();
            ppMsgIn.Dispose();
            ppdwOutVersion.Dispose();
            ppMsgOutIn.Dispose();

            return (NtStatus)retVal;
        }

        #endregion


        /// <summary>
        /// logon__NETLOGON_DELTA_USERExprEval_0000 defined by MIDL.
        /// </summary>
        /// <param name="rpcStub">RpceStub structure.</param>
        internal static void logon__NETLOGON_DELTA_USERExprEval_0000(RpceStub rpcStub)
        {
            IntPtr pStackTop = rpcStub.GetStackTop();
            _NLPR_LOGON_HOURS nlprLogonHours = TypeMarshal.ToStruct<_NLPR_LOGON_HOURS>(pStackTop);
            rpcStub.SetOffset(0);
            rpcStub.SetMaxCount((uint)((nlprLogonHours.UnitsPerWeek + 7) / 8));
        }


        #region IDisposable Members

        /// <summary>
        /// Dispose method.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Dispose method.
        /// </summary>
        /// <param name="disposing">
        /// True to release both managed and unmanaged resources.<para/>
        /// False to release unmanaged resources only.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Release managed resources.
                if (rpceClientTransport != null)
                {
                    try
                    {
                        rpceClientTransport.Unbind(new TimeSpan(0, 0, 3));
                    }
                    catch
                    {
                    }
                    rpceClientTransport.Dispose();

                    rpceClientTransport = null;
                }
            }

            // Release unmanaged resources.
        }


        /// <summary>
        /// finalizer
        /// </summary>
        ~NrpcRpcAdapter()
        {
            Dispose(false);
        }

        #endregion
    }
}
