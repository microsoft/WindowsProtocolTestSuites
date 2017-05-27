// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.Protocols.TestTools.StackSdk;
    using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa;
    using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
    using Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc;
    using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

    /// <summary>
    /// The implementation of INrpcRpcAdapter.
    /// </summary>
    internal class NrpcCustomRpcAdapter : INrpcCustomRpcAdapter, IDisposable
    {
        /// <summary>
        /// RPCE client transport.
        /// </summary>
        private RpceClientTransport rpceClientTransport;

        /// <summary>
        /// Timeout for RPC bind/call.
        /// </summary>
        private TimeSpan rpceTimeout;

        /// <summary>
        /// Finalizes an instance of the NrpcCustomRpcAdapter class.
        /// </summary>
        ~NrpcCustomRpcAdapter()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets rpceClientTransport
        /// </summary>
        public RpceClientTransport RpceClientTransport
        {
            get
            {
                return this.rpceClientTransport;
            }
        }

        /// <summary>
        /// Gets RPC handle.
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                return this.rpceClientTransport.Handle;
            }
        }

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
        /// used by underlayer transport (SMB/SMB2). 
        /// If connect by TCP, this parameter is ignored.
        /// </param>
        /// <param name="securityContext">
        /// RPC security provider.
        /// </param>
        /// <param name="authenticationLevel">
        /// RPC authentication level.
        /// </param>
        /// <param name="timeout">
        /// Time out.
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
            if (this.rpceClientTransport != null)
            {
                throw new InvalidOperationException("NRPC has already been bind.");
            }

            this.rpceTimeout = timeout;

            this.rpceClientTransport = new RpceClientTransport();

            this.rpceClientTransport.Bind(
                protocolSequence,
                networkAddress,
                endpoint,
                transportCredential,
                NrpcUtility.NETLOGON_RPC_INTERFACE_UUID,
                NrpcUtility.NETLOGON_RPC_INTERFACE_MAJOR_VERSION,
                NrpcUtility.NETLOGON_RPC_INTERFACE_MINOR_VERSION,
                securityContext,
                authenticationLevel,
                false,
                this.rpceTimeout);
        }

        /// <summary>
        /// RPC unbind.
        /// </summary>
        public void Unbind()
        {
            if (this.rpceClientTransport != null)
            {
                this.rpceClientTransport.Unbind(this.rpceTimeout);
                this.rpceClientTransport = null;
            }
        }

        /// <summary>
        ///  NetrLogonUasLogon IDL method. Opnum: 0 
        /// </summary>
        /// <param name="serverName">
        ///  serverName parameter.
        /// </param>
        /// <param name="userName">
        ///  userName parameter.
        /// </param>
        /// <param name="workstation">
        ///  workstation parameter.
        /// </param>
        /// <param name="validationInformation">
        ///  validationInformation parameter.
        /// </param>
        /// <returns>
        ///  Net API Status.
        /// </returns>
        public NetApiStatus NetrLogonUasLogon(
            string serverName,
            string userName,
            string workstation,
            out _NETLOGON_VALIDATION_UAS_INFO? validationInformation)
        {
            const ushort Opnum = 0;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrServerName = Marshal.StringToHGlobalUni(serverName);
            SafeIntPtr ptrUserName = Marshal.StringToHGlobalUni(userName);
            SafeIntPtr ptrWorkstation = Marshal.StringToHGlobalUni(workstation);

            paramList = new Int3264[] 
            {
                ptrServerName,
                ptrUserName,
                ptrWorkstation,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrValidationInformation = outParamList[3];
                ptrValidationInformation = Marshal.ReadIntPtr(ptrValidationInformation);
                validationInformation = TypeMarshal.ToNullableStruct<_NETLOGON_VALIDATION_UAS_INFO>(
                    ptrValidationInformation);

                retVal = outParamList[4].ToInt32();
            }

            ptrServerName.Dispose();
            ptrUserName.Dispose();
            ptrWorkstation.Dispose();

            return (NetApiStatus)retVal;
        }

        /// <summary>
        ///  NetrLogonUasLogoff IDL method. Opnum: 1 
        /// </summary>
        /// <param name="serverName">
        ///  serverName parameter.
        /// </param>
        /// <param name="userName">
        ///  userName parameter.
        /// </param>
        /// <param name="workstation">
        ///  workstation parameter.
        /// </param>
        /// <param name="logoffInformation">
        ///  logoffInformation parameter.
        /// </param>
        /// <returns>
        /// Net API status.
        /// </returns>
        public NetApiStatus NetrLogonUasLogoff(
            string serverName,
            string userName,
            string workstation,
            out _NETLOGON_LOGOFF_UAS_INFO? logoffInformation)
        {
            const ushort Opnum = 1;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrServerName = Marshal.StringToHGlobalUni(serverName);
            SafeIntPtr ptrUserName = Marshal.StringToHGlobalUni(userName);
            SafeIntPtr ptrWorkstation = Marshal.StringToHGlobalUni(workstation);

            paramList = new Int3264[] 
            {
                ptrServerName,
                ptrUserName,
                ptrWorkstation,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrValidationInformation = outParamList[3];
                logoffInformation = TypeMarshal.ToNullableStruct<_NETLOGON_LOGOFF_UAS_INFO>(ptrValidationInformation);

                retVal = outParamList[4].ToInt32();
            }

            ptrServerName.Dispose();
            ptrUserName.Dispose();
            ptrWorkstation.Dispose();

            return (NetApiStatus)retVal;
        }

        /// <summary>
        ///  The NetrLogonSamLogon methodThis method is used in
        ///  windows_nt_4_0. It was superseded by the NetrLogonSamLogonWithFlags
        ///  method in windows_2000_server, windows_xp,
        ///  windows_server_2003, windows_vista, and windows_server_2008,
        ///  windows_7, and windows_server_7. It is a predecessor to
        ///  the NetrLogonSamLogonWithFlags method. All
        ///  parameters of this method have the same meanings as
        ///  the identically named parameters of the NetrLogonSamLogonWithFlags
        ///  method. Opnum: 2 
        /// </summary>
        /// <param name="logonServer">
        ///  logonServer parameter.
        /// </param>
        /// <param name="computerName">
        ///  computerName parameter.
        /// </param>
        /// <param name="authenticator">
        ///  authenticator parameter.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  returnAuthenticator parameter.
        /// </param>
        /// <param name="logonLevel">
        ///  logonLevel parameter.
        /// </param>
        /// <param name="logonInformation">
        ///  logonInformation parameter.
        /// </param>
        /// <param name="validationLevel">
        ///  validationLevel parameter.
        /// </param>
        /// <param name="validationInformation">
        ///  validationInformation parameter.
        /// </param>
        /// <param name="authoritative">
        ///  authoritative parameter.
        /// </param>
        /// <returns>
        ///  the NTSTATUS.
        /// </returns>
        public NtStatus NetrLogonSamLogon(
            string logonServer,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            ref _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            _NETLOGON_LOGON_INFO_CLASS logonLevel,
            ////switch_is(LogonLevel)
            _NETLOGON_LEVEL? logonInformation,
            _NETLOGON_VALIDATION_INFO_CLASS validationLevel,
            ////switch_is(ValidationLevel)
            out _NETLOGON_VALIDATION? validationInformation,
            out byte? authoritative)
        {
            const ushort Opnum = 2;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrLogonServer = Marshal.StringToHGlobalUni(logonServer);
            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrAuthenticator = TypeMarshal.ToIntPtr(authenticator);
            SafeIntPtr ptrReturnAuthenticatorIn = TypeMarshal.ToIntPtr(returnAuthenticator);
            SafeIntPtr ptrLogonInformation = TypeMarshal.ToIntPtr(logonInformation, logonLevel, null, null);

            paramList = new Int3264[] 
            {
                ptrLogonServer,
                ptrComputerName,
                ptrAuthenticator,
                ptrReturnAuthenticatorIn,
                (uint)logonLevel,
                ptrLogonInformation,
                (uint)validationLevel,
                IntPtr.Zero,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrReturnAuthenticatorOut = outParamList[3];
                returnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(ptrReturnAuthenticatorOut);

                IntPtr ptrValidationInformation = outParamList[7];
                validationInformation = TypeMarshal.ToNullableStruct<_NETLOGON_VALIDATION>(
                    ptrValidationInformation,
                    validationLevel,
                    null,
                    null);

                authoritative = TypeMarshal.ToNullableStruct<byte>(outParamList[8]);

                retVal = outParamList[9].ToInt32();
            }

            ptrLogonServer.Dispose();
            ptrComputerName.Dispose();
            ptrAuthenticator.Dispose();
            ptrReturnAuthenticatorIn.Dispose();
            ptrLogonInformation.Dispose();

            return (NtStatus)retVal;
        }

        /// <summary>
        ///  The NetrLogonSamLogoff method handles logoff requests
        ///  for the SAM accounts. Opnum: 3 
        /// </summary>
        /// <param name="logonServer">
        ///  The custom RPC binding handle, as specified in the MS-NRPC.
        /// </param>
        /// <param name="computerName">
        ///  The Unicode string that contains the NetBIOS name of
        ///  the client computer calling this method.
        /// </param>
        /// <param name="authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in the MS-NRPC , that contains the client authenticator.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in the MS-NRPC, that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="logonLevel">
        ///  A NETLOGON_LOGON_INFO_CLASS structure, as specified
        ///  in the MS-NRPC, that identifies the type of logon information
        ///  in the logonInformation union.
        /// </param>
        /// <param name="logonInformation">
        ///  A pointer to a NETLOGON_LEVEL structure, as specified
        ///  in the MS-NRPC, that describes the logon information.
        /// </param>
        /// <param name="isLogonInfoNull">
        ///  Indicate if the logonInformation field is NULL.
        ///  </param>
        ///  <returns>
        ///  the NTSTATUS.
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
            const ushort Opnum = 3;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrLogonServer = Marshal.StringToHGlobalUni(logonServer);
            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrAuthenticator = TypeMarshal.ToIntPtr(authenticator);
            SafeIntPtr ptrReturnAuthenticatorIn = TypeMarshal.ToIntPtr(returnAuthenticator);
            SafeIntPtr ptrLogonInformation = TypeMarshal.ToIntPtr(logonInformation, logonLevel, null, null);

            paramList = new Int3264[] 
            {
                ptrLogonServer,
                ptrComputerName,
                ptrAuthenticator,
                ptrReturnAuthenticatorIn,
                (uint)logonLevel,
                ptrLogonInformation,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);
            if (isLogonInfoNull)
            {
                // 112 is the index where logonInformation starts.
                for (int i = 112; i < requestStub.Length; i++)
                {
                    requestStub[i] = 0;
                }
            }

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrReturnAuthenticatorOut = outParamList[3];
                returnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(ptrReturnAuthenticatorOut);

                retVal = outParamList[6].ToInt32();
            }

            ptrLogonServer.Dispose();
            ptrComputerName.Dispose();
            ptrAuthenticator.Dispose();
            ptrReturnAuthenticatorIn.Dispose();
            ptrLogonInformation.Dispose();

            return (NtStatus)retVal;
        }

        /// <summary>
        ///  The NetrLogonSamLogoff method handles logoff requests
        ///  for the SAM accounts. Opnum: 3 
        /// </summary>
        /// <param name="logonServer">
        ///  The custom RPC binding handle, as specified in the MS-NRPC.
        /// </param>
        /// <param name="computerName">
        ///  The Unicode string that contains the NetBIOS name of
        ///  the client computer calling this method.
        /// </param>
        /// <param name="authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in MS-NRPC, that contains the client authenticator.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in MS-NRPC, that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="logonLevel">
        ///  A NETLOGON_LOGON_INFO_CLASS structure, as specified
        ///  in MS-NRPC, that identifies the type of logon information
        ///  in the logonInformation union.
        /// </param>
        /// <param name="logonInformation">
        ///  A pointer to a NETLOGON_LEVEL structure, as specified
        ///  in MS-NRPC, that describes the logon information.
        /// </param>
        /// <returns>
        /// The NTSTATUS.
        /// </returns>
        public NtStatus NetrLogonSamLogoff(
            string logonServer,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            ref _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            _NETLOGON_LOGON_INFO_CLASS logonLevel,
            _NETLOGON_LEVEL? logonInformation)
        {
            const ushort Opnum = 3;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrLogonServer = Marshal.StringToHGlobalUni(logonServer);
            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrAuthenticator = TypeMarshal.ToIntPtr(authenticator);
            SafeIntPtr ptrReturnAuthenticatorIn = TypeMarshal.ToIntPtr(returnAuthenticator);
            SafeIntPtr ptrLogonInformation = TypeMarshal.ToIntPtr(logonInformation, logonLevel, null, null);

            paramList = new Int3264[] 
            {
                ptrLogonServer,
                ptrComputerName,
                ptrAuthenticator,
                ptrReturnAuthenticatorIn,
                (uint)logonLevel,
                ptrLogonInformation,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrReturnAuthenticatorOut = outParamList[3];
                returnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(ptrReturnAuthenticatorOut);

                retVal = outParamList[6].ToInt32();
            }

            ptrLogonServer.Dispose();
            ptrComputerName.Dispose();
            ptrAuthenticator.Dispose();
            ptrReturnAuthenticatorIn.Dispose();
            ptrLogonInformation.Dispose();

            return (NtStatus)retVal;
        }

        /// <summary>
        ///  The NetrServerReqChallenge method receives a client
        ///  challenge and returns a server challenge. Opnum: 4
        /// </summary>
        /// <param name="primaryName">
        ///  The custom RPC binding handle, as specified in MS-NRPC
        /// </param>
        /// <param name="computerName">
        ///  A Unicode string that contains the NetBIOS name of the
        ///  client computer calling this method.
        /// </param>
        /// <param name="clientChallenge">
        ///  A pointer to a NETLOGON_CREDENTIAL structure, as specified
        ///  in MS-NRPC, that contains the client challenge.
        /// </param>
        /// <param name="serverChallenge">
        ///  A pointer to a NETLOGON_CREDENTIAL structure, as specified
        ///  in MS-NRPC, that contains the server challenge response.
        /// </param>
        /// <returns>
        /// the NTSTATUS.
        /// </returns>
        public NtStatus NetrServerReqChallenge(
            string primaryName,
            string computerName,
            _NETLOGON_CREDENTIAL? clientChallenge,
            out _NETLOGON_CREDENTIAL? serverChallenge)
        {
            const ushort Opnum = 4;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrPrimaryName = Marshal.StringToHGlobalUni(primaryName);
            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrClientChallenge = TypeMarshal.ToIntPtr(clientChallenge);

            paramList = new Int3264[] 
            {
                ptrPrimaryName,
                ptrComputerName,
                ptrClientChallenge,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrServerChallenge = outParamList[3];
                serverChallenge = TypeMarshal.ToNullableStruct<_NETLOGON_CREDENTIAL>(ptrServerChallenge);

                retVal = outParamList[4].ToInt32();
            }

            ptrPrimaryName.Dispose();
            ptrComputerName.Dispose();
            ptrClientChallenge.Dispose();

            return (NtStatus)retVal;
        }

        /// <summary>
        ///  The NetrServerAuthenticate method is used
        ///  in windows_nt_server_3_1. In windows_nt_server_3_5,
        ///  it was superseded by the NetrServerAuthenticate2 method
        ///  . In windows_2000_server, windows_xp, windows_server_2003,
        ///  windows_vista, windows_server_2008, windows_7, and
        ///  windows_server_7, the NetrServerAuthenticate2 method
        ///  was superseded by the NetrServerAuthenticate3
        ///  method. is a predecessor to the NetrServerAuthenticate3
        ///  method. All parameters of this method have
        ///  the same meanings as the identically named parameters
        ///  of the NetrServerAuthenticate3 method. Opnum: 5 
        /// </summary>
        /// <param name="primaryName">
        ///  primaryName parameter.
        /// </param>
        /// <param name="accountName">
        ///  accountName parameter.
        /// </param>
        /// <param name="secureChannelType">
        ///  secureChannelType parameter.
        /// </param>
        /// <param name="computerName">
        ///  computerName parameter.
        /// </param>
        /// <param name="clientCredential">
        ///  clientCredential parameter.
        /// </param>
        /// <param name="serverCredential">
        ///  serverCredential parameter.
        /// </param>
        /// <returns>
        /// the NTSTATUS.
        /// </returns>
        public NtStatus NetrServerAuthenticate(
            string primaryName,
            string accountName,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType,
            string computerName,
            _NETLOGON_CREDENTIAL? clientCredential,
            out _NETLOGON_CREDENTIAL? serverCredential)
        {
            const ushort Opnum = 5;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrPrimaryName = Marshal.StringToHGlobalUni(primaryName);
            SafeIntPtr ptrAccountName = Marshal.StringToHGlobalUni(accountName);
            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrClientCredential = TypeMarshal.ToIntPtr(clientCredential);

            paramList = new Int3264[] 
            {
                ptrPrimaryName,
                ptrAccountName,
                (uint)secureChannelType,
                ptrComputerName,
                ptrClientCredential,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrServerCredential = outParamList[5];
                serverCredential = TypeMarshal.ToNullableStruct<_NETLOGON_CREDENTIAL>(ptrServerCredential);

                retVal = outParamList[6].ToInt32();
            }

            ptrPrimaryName.Dispose();
            ptrAccountName.Dispose();
            ptrComputerName.Dispose();
            ptrClientCredential.Dispose();

            return (NtStatus)retVal;
        }

        /// <summary>
        ///  The NetrServerPasswordSet method sets a new one-way
        ///  function (OWF) of a password for an account used by
        ///  the domain controller for
        ///  setting up the secure channel from the client. Opnum
        ///  : 6 
        /// </summary>
        /// <param name="primaryName">
        ///  The custom RPC binding handle, as specified in MS-NRPC.
        /// </param>
        /// <param name="accountName">
        ///  The null-terminated Unicode string that contains the
        ///  name of the account whose password is being changed. In
        ///  windows, all machine account names are the name of
        ///  the machine with a $ (dollar sign) appended.
        /// </param>
        /// <param name="secureChannelType">
        ///  An enumerated value that indicates
        ///  the type of secure channel used by the client.
        /// </param>
        /// <param name="computerName">
        ///  A null-terminated Unicode string that contains the NetBIOS
        ///  name of the client computer calling this method.
        /// </param>
        /// <param name="authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in MS-NRPC, that contains the client authenticator.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in MS-NRPC, that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="uasNewPassword">
        ///  A pointer to an ENCRYPTED_LM_OWF_PASSWORD structure,
        ///  as specified in MS-NRPC and encrypted by the algorithm
        ///  specified in MS-NRPC.
        /// </param>
        /// <returns>
        /// The NTSTATUS.
        /// </returns>
        public NtStatus NetrServerPasswordSet(
            string primaryName,
            string accountName,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            out _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            _LM_OWF_PASSWORD? uasNewPassword)
        {
            const ushort Opnum = 6;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrPrimaryName = Marshal.StringToHGlobalUni(primaryName);
            SafeIntPtr ptrAccountName = Marshal.StringToHGlobalUni(accountName);
            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrAuthenticator = TypeMarshal.ToIntPtr(authenticator);
            SafeIntPtr ptrUasNewPassword = TypeMarshal.ToIntPtr(uasNewPassword);

            paramList = new Int3264[] 
            {
                ptrPrimaryName,
                ptrAccountName,
                (uint)secureChannelType,
                ptrComputerName,
                ptrAuthenticator,
                IntPtr.Zero,
                ptrUasNewPassword,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrReturnAuthenticator = outParamList[5];
                returnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(ptrReturnAuthenticator);

                retVal = outParamList[7].ToInt32();
            }

            ptrPrimaryName.Dispose();
            ptrAccountName.Dispose();
            ptrComputerName.Dispose();
            ptrAuthenticator.Dispose();
            ptrUasNewPassword.Dispose();

            return (NtStatus)retVal;
        }

        /// <summary>
        /// The NetrDatabaseDeltas method returns a set of changes (or deltas) 
        /// performed to the SAM, SAM built-in, or LSA databases after a particular 
        /// value of the database serial number. It is used by BDCs to request 
        /// database changes from the PDC that are missing on the BDC. Opnum: 7
        /// </summary>
        /// <param name="primaryName">
        /// The custom RPC binding handle (as specified in section 3.5.5.1) that 
        /// represents the connection to the PDC.
        /// </param>
        /// <param name="computerName">
        /// The null-terminated Unicode string that contains the NetBIOS name of 
        /// the BDC calling this method.
        /// </param>
        /// <param name="authenticator">
        /// A pointer to a NETLOGON_AUTHENTICATOR structure that contains the 
        /// client authenticator.
        /// </param>
        /// <param name="returnAuthenticator">
        /// A pointer to a NETLOGON_AUTHENTICATOR structure that contains the 
        /// server return authenticator.
        /// </param>
        /// <param name="databaseID">
        /// The identifier for a specific account database set as follows: 
        /// 0x00000000
        /// Indicates the SAM database.
        /// 0x00000001
        /// Indicates the SAM built-in database.
        /// 0x00000002
        /// Indicates the LSA database.
        /// </param>
        /// <param name="domainModifiedCount">
        /// A pointer to an NLPR_MODIFIED_COUNT structure, as specified in section 
        /// 2.2.1.5.26, that contains the database serial number. On input, this is 
        /// the value of the database serial number on the client. On output, this is 
        /// the value of the database serial number corresponding to the last element 
        /// (delta) returned in the deltaArray parameter.
        /// </param>
        /// <param name="deltaArray">
        /// A pointer to a NETLOGON_DELTA_ENUM_ARRAY structure that contains an array 
        /// of enumerated changes (deltas) to the specified database with database 
        /// serial numbers larger than the database serial number value specified in 
        /// the input value of the domainModifiedCount parameter.
        /// </param>
        /// <param name="preferredMaximumLength">
        /// The value that specifies the preferred maximum size, in bytes, of data to 
        /// return in the deltaArray parameter. This is not a hard upper limit, but 
        /// serves as a guide to the server. The server SHOULD stop including 
        /// elements in the returned deltaArray after the size of the returned data 
        /// equals or exceeds the value of the preferredMaximumLength parameter. It is 
        /// up to the client implementation to choose the value for this parameter.
        /// </param>
        /// <returns>
        /// The NTSTATUS.
        /// </returns>
        public NtStatus NetrDatabaseDeltas(
            string primaryName,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            ref _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            DatabaseID_Values databaseID,
            ref _NLPR_MODIFIED_COUNT? domainModifiedCount,
            out _NETLOGON_DELTA_ENUM_ARRAY? deltaArray,
            uint preferredMaximumLength)
        {
            const ushort Opnum = 7;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrPrimaryName = Marshal.StringToHGlobalUni(primaryName);
            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrAuthenticator = TypeMarshal.ToIntPtr(authenticator);
            SafeIntPtr ptrReturnAuthenticatorIn = TypeMarshal.ToIntPtr(returnAuthenticator);
            SafeIntPtr ptrDomainModifiedCountIn = TypeMarshal.ToIntPtr(domainModifiedCount);

            paramList = new Int3264[] 
            {
                ptrPrimaryName,
                ptrComputerName,
                ptrAuthenticator,
                ptrReturnAuthenticatorIn,
                (uint)databaseID,
                ptrDomainModifiedCountIn,
                IntPtr.Zero,
                preferredMaximumLength,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrReturnAuthenticatorOut = outParamList[3];
                returnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(ptrReturnAuthenticatorOut);

                IntPtr ptrDomainModifiedCountOut = outParamList[5];
                domainModifiedCount = TypeMarshal.ToNullableStruct<_NLPR_MODIFIED_COUNT>(ptrDomainModifiedCountOut);

                IntPtr ptrDeltaArray = outParamList[6];
                ptrDeltaArray = Marshal.ReadIntPtr(ptrDeltaArray);
                deltaArray = TypeMarshal.ToNullableStruct<_NETLOGON_DELTA_ENUM_ARRAY>(ptrDeltaArray);

                retVal = outParamList[8].ToInt32();
            }

            ptrPrimaryName.Dispose();
            ptrComputerName.Dispose();
            ptrAuthenticator.Dispose();
            ptrReturnAuthenticatorIn.Dispose();
            ptrDomainModifiedCountIn.Dispose();

            return (NtStatus)retVal;
        }

        /// <summary>
        ///  The NetrDatabaseSync method is a predecessor to the NetrDatabaseSync2 method, 
        ///  as specified in section 3.5.5.5.2. All parameters of this method have the same 
        ///  meanings as the identically named parameters of the NetrDatabaseSync2 method. 
        ///  Opnum: 8
        /// </summary>
        /// <param name="primaryName">
        ///  The custom RPC binding handle, as specified in MS-NRPC
        ///  , representing the connection to the PDC.
        /// </param>
        /// <param name="computerName">
        ///  The null-terminated Unicode string that contains the
        ///  NetBIOS name of the BDC calling this method.
        /// </param>
        /// <param name="authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in MS-NRPC, that contains the client authenticator.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in MS-NRPC, that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="databaseID">
        ///  The identifier for a specific database for which the
        ///  changes are requested. It MUST be one of the following
        ///  values.
        /// </param>
        /// <param name="syncContext">
        ///  Specifies context needed to continue the operation.
        ///  The value MUST be set to zero on the first call. The
        ///  caller MUST treat this as an opaque value, unless this
        ///  call is a restart of the series of synchronization
        ///  calls. The value returned is to be used on input for
        ///  the next call in the series of synchronization calls. If
        ///  this call is the restart of the series, the values
        ///  of the RestartState and the syncContext parameters
        ///  are dependent on the DeltaType value received on the
        ///  last call before the restart and MUST be set as follows.
        ///  Find the last NETLOGON_DELTA_ENUM structure in the
        ///  deltaArray parameter of the call. The DeltaType field
        ///  of this NETLOGON_DELTA_ENUM structure, as specified
        ///  in MS-NRPC, is the DeltaType needed for the restart.
        ///  The values of RestartState and syncContext are then
        ///  determined from the following table. 
        ///  DeltaType | RestartState | SyncContext
        ///  ------------------------------------------
        ///  AddOrChangeGroup | GroupState | The value of the RID field of the last element
        ///  AddOrChangeUser | UserState | The value of the RID field of the last element
        ///  ChangeGroupMembership | GroupMemberState | The value of the RID field of the last element
        ///  AddOrChangeAlias | AliasState | 0x00000000
        ///  ChangeAliasMembership | AliasMemberState | 0x00000000
        ///  Any other value not previously listed | NormalState | 0x00000000
        /// </param>
        /// <param name="deltaArray">
        ///  A pointer to a NETLOGON_DELTA_ENUM_ARRAY structure,
        ///  as specified in MS-NRPC, that contains an array of
        ///  enumerated changes (deltas) to the specified database.
        /// </param>
        /// <param name="preferredMaximumLength">
        ///  The value that specifies the preferred maximum size,
        ///  in bytes, of data referenced in the deltaArray parameter.
        ///  This is not a hard upper limit, but serves as a guide
        /// to the server. The server SHOULD stop including
        ///  elements in the returned deltaArray once the size of
        ///  the returned data equals or exceeds the value of the
        ///  preferredMaximumLength parameter. The server SHOULD stop including elements
        ///  in the returned deltaArray once the size of the returned
        ///  data equals or exceeds the value of the preferredMaximumLength
        ///  parameter. It is up to the client implementation to
        ///  choose the value for this parameter.
        /// </param>
        /// <returns>
        /// The NTSTATUS.
        /// </returns>
        public NtStatus NetrDatabaseSync(
            string primaryName,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            ref _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            DatabaseID_Values databaseID,
            ref uint? syncContext,
            out _NETLOGON_DELTA_ENUM_ARRAY? deltaArray,
            uint preferredMaximumLength)
        {
            const ushort Opnum = 8;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrPrimaryName = Marshal.StringToHGlobalUni(primaryName);
            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrAuthenticator = TypeMarshal.ToIntPtr(authenticator);
            SafeIntPtr ptrReturnAuthenticatorIn = TypeMarshal.ToIntPtr(returnAuthenticator);
            SafeIntPtr ptrSyncContext = TypeMarshal.ToIntPtr(syncContext);

            paramList = new Int3264[] 
            {
                ptrPrimaryName,
                ptrComputerName,
                ptrAuthenticator,
                ptrReturnAuthenticatorIn,
                (uint)databaseID,
                ptrSyncContext,
                IntPtr.Zero,
                preferredMaximumLength,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrReturnAuthenticatorOut = outParamList[3];
                returnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(ptrReturnAuthenticatorOut);

                syncContext = TypeMarshal.ToNullableStruct<uint>(outParamList[5]);

                IntPtr ptrDeltaArray = outParamList[6];
                ptrDeltaArray = Marshal.ReadIntPtr(ptrDeltaArray);
                deltaArray = TypeMarshal.ToNullableStruct<_NETLOGON_DELTA_ENUM_ARRAY>(ptrDeltaArray);

                retVal = outParamList[8].ToInt32();
            }

            ptrPrimaryName.Dispose();
            ptrComputerName.Dispose();
            ptrAuthenticator.Dispose();
            ptrReturnAuthenticatorIn.Dispose();
            ptrSyncContext.Dispose();

            return (NtStatus)retVal;
        }

        /// <summary>
        ///  NetrAccountDeltas IDL method. Opnum: 9 
        /// </summary>
        /// <param name="primaryName">
        ///  primaryName parameter.
        /// </param>
        /// <param name="computerName">
        ///  computerName parameter.
        /// </param>
        /// <param name="authenticator">
        ///  authenticator parameter.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  returnAuthenticator parameter.
        /// </param>
        /// <param name="recordID">
        ///  recordID parameter.
        /// </param>
        /// <param name="count">
        ///  count parameter.
        /// </param>
        /// <param name="level">
        ///  level parameter.
        /// </param>
        /// <param name="buffer">
        ///  buffer parameter.
        /// </param>
        /// <param name="bufferSize">
        ///  bufferSize parameter.
        /// </param>
        /// <param name="countReturned">
        ///  CountReturned parameter.
        /// </param>
        /// <param name="totalEntries">
        ///  totalEntries parameter.
        /// </param>
        /// <param name="nextRecordId">
        ///  nextRecordId parameter.
        /// </param>
        /// <returns>
        ///  the NTSTATUS.
        /// </returns>
        public NtStatus NetrAccountDeltas(
            string primaryName,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            ref _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            _UAS_INFO_0? recordID,
            uint count,
            uint level,
            out byte[] buffer,
            uint bufferSize,
            out uint? countReturned,
            out uint? totalEntries,
            out _UAS_INFO_0? nextRecordId)
        {
            const ushort Opnum = 9;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrPrimaryName = Marshal.StringToHGlobalUni(primaryName);
            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrAuthenticator = TypeMarshal.ToIntPtr(authenticator);
            SafeIntPtr ptrReturnAuthenticatorIn = TypeMarshal.ToIntPtr(returnAuthenticator);
            SafeIntPtr ptrRecordID = TypeMarshal.ToIntPtr(recordID);

            paramList = new Int3264[] 
            {
                ptrPrimaryName,
                ptrComputerName,
                ptrAuthenticator,
                ptrReturnAuthenticatorIn,
                ptrRecordID,
                count,
                level,
                IntPtr.Zero,
                bufferSize,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrReturnAuthenticatorOut = outParamList[3];
                returnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(ptrReturnAuthenticatorOut);

                IntPtr ptrBuffer = outParamList[7];
                buffer = IntPtrUtility.PtrToArray<byte>(ptrBuffer, bufferSize);

                countReturned = TypeMarshal.ToNullableStruct<uint>(outParamList[9]);

                totalEntries = TypeMarshal.ToNullableStruct<uint>(outParamList[10]);

                IntPtr ptrNextRecordId = outParamList[11];
                nextRecordId = TypeMarshal.ToNullableStruct<_UAS_INFO_0>(ptrNextRecordId);

                retVal = outParamList[12].ToInt32();
            }

            ptrPrimaryName.Dispose();
            ptrComputerName.Dispose();
            ptrAuthenticator.Dispose();
            ptrReturnAuthenticatorIn.Dispose();
            ptrRecordID.Dispose();

            return (NtStatus)retVal;
        }

        /// <summary>
        ///  NetrAccountSync IDL method. Opnum: 10 
        /// </summary>
        /// <param name="primaryName">
        ///  primaryName parameter.
        /// </param>
        /// <param name="computerName">
        ///  computerName parameter.
        /// </param>
        /// <param name="authenticator">
        ///  authenticator parameter.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  returnAuthenticator parameter.
        /// </param>
        /// <param name="reference">
        ///  reference parameter.
        /// </param>
        /// <param name="level">
        ///  level parameter.
        /// </param>
        /// <param name="buffer">
        ///  buffer parameter.
        /// </param>
        /// <param name="bufferSize">
        ///  bufferSize parameter.
        /// </param>
        /// <param name="countReturned">
        ///  countReturned parameter.
        /// </param>
        /// <param name="totalEntries">
        ///  totalEntries parameter.
        /// </param>
        /// <param name="nextReference">
        ///  nextReference parameter.
        /// </param>
        /// <param name="lastRecordId">
        ///  lastRecordId parameter.
        /// </param>
        /// <returns>
        ///  the NTSTATUS.
        /// </returns>
        public NtStatus NetrAccountSync(
            string primaryName,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            ref _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            uint reference,
            uint level,
            out byte[] buffer,
            uint bufferSize,
            out uint? countReturned,
            out uint? totalEntries,
            out uint? nextReference,
            out _UAS_INFO_0? lastRecordId)
        {
            const ushort Opnum = 10;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrPrimaryName = Marshal.StringToHGlobalUni(primaryName);
            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrAuthenticator = TypeMarshal.ToIntPtr(authenticator);
            SafeIntPtr ptrReturnAuthenticatorIn = TypeMarshal.ToIntPtr(returnAuthenticator);

            paramList = new Int3264[] 
            {
                ptrPrimaryName,
                ptrComputerName,
                ptrAuthenticator,
                ptrReturnAuthenticatorIn,
                reference,
                level,
                IntPtr.Zero,
                bufferSize,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrReturnAuthenticatorOut = outParamList[3];
                returnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(ptrReturnAuthenticatorOut);

                IntPtr ptrBuffer = outParamList[6];
                buffer = IntPtrUtility.PtrToArray<byte>(ptrBuffer, bufferSize);

                countReturned = TypeMarshal.ToNullableStruct<uint>(outParamList[8]);

                totalEntries = TypeMarshal.ToNullableStruct<uint>(outParamList[9]);

                nextReference = TypeMarshal.ToNullableStruct<uint>(outParamList[10]);

                IntPtr ptrLastRecordId = outParamList[11];
                lastRecordId = TypeMarshal.ToNullableStruct<_UAS_INFO_0>(ptrLastRecordId);

                retVal = outParamList[12].ToInt32();
            }

            ptrPrimaryName.Dispose();
            ptrComputerName.Dispose();
            ptrAuthenticator.Dispose();
            ptrReturnAuthenticatorIn.Dispose();

            return (NtStatus)retVal;
        }

        /// <summary>
        ///  The NetrGetDCName method is used in windows_nt_server_3_1
        ///  and is supported in windows_nt_server_3_1 versions.
        ///  It was superseded by the DsrGetDcNameEx2 method
        ///  in windows_2000, windows_xp, windows_server_2003,
        ///  windows_vista,  windows_server_2008, windows_7, and
        ///  windows_server_7. It retrieves the NetBIOS name of the
        ///  PDC for the specified domain. Opnum: 11 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle, as specified in MS-NRPC
        ///  , that represents the connection to a domain controller.
        /// </param>
        /// <param name="domainName">
        ///  A null-terminated Unicode string that specifies the
        ///  NetBIOS name of the domain.
        /// </param>
        /// <param name="buffer">
        ///  A pointer to a null-terminated Unicode string that contains
        ///  the NetBIOS name of the PDC for the specified domain.
        ///  The server name returned by this method is prefixed
        ///  by two backslashes (\\).
        /// </param>
        /// <returns>
        ///  Net API status
        /// </returns>
        public NetApiStatus NetrGetDCName(
            string serverName,
            string domainName,
            out string buffer)
        {
            const ushort Opnum = 11;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrServerName = Marshal.StringToHGlobalUni(serverName);
            SafeIntPtr ptrDomainName = Marshal.StringToHGlobalUni(domainName);

            paramList = new Int3264[] 
            {
                ptrServerName,
                ptrDomainName,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrBuffer = outParamList[2];

                // wchar_t** Buffer
                ptrBuffer = Marshal.ReadIntPtr(ptrBuffer);
                buffer = Marshal.PtrToStringUni(ptrBuffer);

                retVal = outParamList[3].ToInt32();
            }

            ptrServerName.Dispose();
            ptrDomainName.Dispose();

            return (NetApiStatus)retVal;
        }

        /// <summary>
        ///  The NetrLogonControl method is a predecessor to the
        ///  NetrLogonControl2Ex method, as specified in MS-NRPC
        ///  All parameters of this method have the same meanings
        ///  as the identically named parameters of the NetrLogonControl2Ex
        ///  method. Opnum: 12 
        /// </summary>
        /// <param name="serverName">
        ///  serverName parameter.
        /// </param>
        /// <param name="functionCode">
        ///  functionCode parameter.
        /// </param>
        /// <param name="queryLevel">
        ///  queryLevel parameter.
        /// </param>
        /// <param name="buffer">
        ///  buffer parameter.
        /// </param>
        /// <returns>
        ///  Net API status
        /// </returns>
        public NetApiStatus NetrLogonControl(
            string serverName,
            uint functionCode,
            uint queryLevel,
            out _NETLOGON_CONTROL_QUERY_INFORMATION? buffer)
        {
            const ushort Opnum = 12;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrServerName = Marshal.StringToHGlobalUni(serverName);

            paramList = new Int3264[] 
            {
                ptrServerName,
                functionCode,
                queryLevel,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrBuffer = outParamList[3];
                buffer = TypeMarshal.ToNullableStruct<_NETLOGON_CONTROL_QUERY_INFORMATION>(
                    ptrBuffer,
                    queryLevel,
                    null,
                    null);

                retVal = outParamList[4].ToInt32();
            }

            ptrServerName.Dispose();

            return (NetApiStatus)retVal;
        }

        /// <summary>
        ///  The NetrGetAnyDCName method was introduced
        ///  in windows_nt_server_3_1 and is supported in windows_nt_server_3_1
        ///  versions. It was superseded by the DsrGetDcNameEx2
        ///  method in windows_2000, windows_xp, windows_server_2003,
        ///  windows_vista, and windows_server_2008, windows_7,
        ///  and windows_server_7. It retrieves the name of a domain
        ///  controller in the specified primary or directly trusted
        ///  domain. Only DCs can return the name of a DC in a specified
        ///  directly trusted domain. Opnum: 13 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle, as specified in MS-NRPC.
        /// </param>
        /// <param name="domainName">
        ///  A null-terminated Unicode string that contains the name
        ///  of the primary or directly trusted domain. If the string
        ///  is NULL or empty (that is, the first character in the
        ///  string is the null-terminator character), the primary
        ///  domain name (3) is assumed.
        /// </param>
        /// <param name="buffer">
        ///  A pointer to an allocated buffer that contains the null-terminated
        ///  Unicode string containing the NetBIOS name of a DC
        ///  in the specified domain. The DC name is prefixed by
        ///  two backslashes (\\).
        /// </param>
        /// <returns>
        ///  Net API status
        /// </returns>
        public NetApiStatus NetrGetAnyDCName(
            string serverName,
            string domainName,
            out string buffer)
        {
            const ushort Opnum = 13;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrServerName = Marshal.StringToHGlobalUni(serverName);
            SafeIntPtr ptrDomainName = Marshal.StringToHGlobalUni(domainName);

            paramList = new Int3264[] 
            {
                ptrServerName,
                ptrDomainName,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrBuffer = outParamList[2];

                // wchar_t** Buffer
                ptrBuffer = Marshal.ReadIntPtr(ptrBuffer);
                buffer = Marshal.PtrToStringUni(ptrBuffer);

                retVal = outParamList[3].ToInt32();
            }

            ptrServerName.Dispose();
            ptrDomainName.Dispose();

            return (NetApiStatus)retVal;
        }

        /// <summary>
        ///  The NetrLogonControl2 method is a predecessor to the
        ///  NetrLogonControl2Ex method, as specified in MS-NRPC
        ///  All parameters of this method have the same meanings
        ///  as the identically named parameters of the NetrLogonControl2Ex
        ///  method. Opnum: 14 
        /// </summary>
        /// <param name="serverName">
        ///  serverName parameter.
        /// </param>
        /// <param name="functionCode">
        ///  functionCode parameter.
        /// </param>
        /// <param name="queryLevel">
        ///  queryLevel parameter.
        /// </param>
        /// <param name="data">
        ///  data parameter.
        /// </param>
        /// <param name="buffer">
        ///  buffer parameter.
        /// </param>
        /// <returns>
        ///  Net API status
        /// </returns>
        public NetApiStatus NetrLogonControl2(
            string serverName,
            uint functionCode,
            uint queryLevel,
            _NETLOGON_CONTROL_DATA_INFORMATION? data,
            out _NETLOGON_CONTROL_QUERY_INFORMATION? buffer)
        {
            const ushort Opnum = 14;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrServerName = Marshal.StringToHGlobalUni(serverName);
            SafeIntPtr ptrData = TypeMarshal.ToIntPtr(data, functionCode, null, null);

            paramList = new Int3264[] 
            {
                ptrServerName,
                functionCode,
                queryLevel,
                ptrData,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrBuffer = outParamList[4];
                buffer = null;
                if (queryLevel > 0 && queryLevel < 5)
                {
                    buffer = TypeMarshal.ToNullableStruct<_NETLOGON_CONTROL_QUERY_INFORMATION>(
                        ptrBuffer,
                        queryLevel,
                        null,
                        null);
                }

                retVal = outParamList[5].ToInt32();
            }

            ptrServerName.Dispose();
            ptrData.Dispose();

            return (NetApiStatus)retVal;
        }

        /// <summary>
        ///The NetrServerAuthenticate2 method was used
        ///  in windows_nt_3_5 and windows_nt_4_0. In windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7, it was superseded
        ///  by the NetrServerAuthenticate3 method. It is
        ///  a predecessor to the NetrServerAuthenticate3 method,
        ///  as specified in MS-NRPC. All parameters of this method
        ///  have the same meanings as the identically named parameters
        ///  of the NetrServerAuthenticate3 method. Opnum: 15 
        /// </summary>
        /// <param name="primaryName">
        ///  primaryName parameter.
        /// </param>
        /// <param name="accountName">
        ///  accountName parameter.
        /// </param>
        /// <param name="secureChannelType">
        ///  secureChannelType parameter.
        /// </param>
        /// <param name="computerName">
        ///  computerName parameter.
        /// </param>
        /// <param name="clientCredential">
        ///  clientCredential parameter.
        /// </param>
        /// <param name="serverCredential">
        ///  serverCredential parameter.
        /// </param>
        /// <param name="negotiateFlags">
        ///  negotiateFlags parameter.
        /// </param>
        /// <returns>
        ///  the NTSTATUS.
        /// </returns>
        public NtStatus NetrServerAuthenticate2(
            string primaryName,
            string accountName,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType,
            string computerName,
            _NETLOGON_CREDENTIAL? clientCredential,
            out _NETLOGON_CREDENTIAL? serverCredential,
            ref uint? negotiateFlags)
        {
            const ushort Opnum = 15;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrPrimaryName = Marshal.StringToHGlobalUni(primaryName);
            SafeIntPtr ptrAccountName = Marshal.StringToHGlobalUni(accountName);
            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrClientCredential = TypeMarshal.ToIntPtr(clientCredential);
            SafeIntPtr ptrNegotiateFlags = TypeMarshal.ToIntPtr(negotiateFlags);

            paramList = new Int3264[] 
            {
                ptrPrimaryName,
                ptrAccountName,
                (uint)secureChannelType,
                ptrComputerName,
                ptrClientCredential,
                IntPtr.Zero,
                ptrNegotiateFlags,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrServerCredential = outParamList[5];
                serverCredential = TypeMarshal.ToNullableStruct<_NETLOGON_CREDENTIAL>(ptrServerCredential);

                negotiateFlags = TypeMarshal.ToNullableStruct<uint>(outParamList[6]);

                retVal = outParamList[7].ToInt32();
            }

            ptrPrimaryName.Dispose();
            ptrAccountName.Dispose();
            ptrComputerName.Dispose();
            ptrClientCredential.Dispose();
            ptrNegotiateFlags.Dispose();

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
        ///  during the series of calls as detailed in MS-NRPC.
        ///  Opnum: 16 
        /// </summary>
        /// <param name="primaryName">
        ///  The custom RPC binding handle, as specified in MS-NRPC
        ///  , that represents the connection to the PDC.
        /// </param>
        /// <param name="computerName">
        ///  The null-terminated Unicode string that contains the
        ///  NetBIOS name of the BDC calling this method.
        /// </param>
        /// <param name="authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in MS-NRPC, that contains the client authenticator.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in MS-NRPC, that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="databaseID">
        ///  The identifier for a specific database for which the
        ///  changes are requested. It MUST be one of the following
        ///  values.
        /// </param>
        /// <param name="restartState">
        ///  Specifies whether this is a restart of the series of
        ///  the synchronization calls and how to interpret SyncContext.
        ///  This value MUST be NormalState unless this is the restart,
        ///  in which case the value MUST be set as specified in
        ///  the description of the syncContext parameter.
        /// </param>
        /// <param name="syncContext">
        ///  Specifies context needed to continue the operation.
        ///  The value MUST be set to zero on the first call. The
        ///  caller MUST treat this as an opaque value, unless this
        ///  call is a restart of the series of synchronization
        ///  calls. The value returned is to be used on input for
        ///  the next call in the series of synchronization calls. If
        ///  this call is the restart of the series, the values
        ///  of the RestartState and the syncContext parameters
        ///  are dependent on the DeltaType value received on the
        ///  last call before the restart and MUST be set as follows.
        ///  Find the last NETLOGON_DELTA_ENUM structure in the
        ///  deltaArray parameter of the call. The DeltaType field
        ///  of this NETLOGON_DELTA_ENUM structure, as specified
        ///  in MS-NRPC, is the DeltaType needed for the restart.
        ///  The values of restartState and syncContext are then
        ///  determined from the following table. DeltaTypeRestartStateSyncContextAddOrChangeGroupGroupStateThe
        ///  value of the RID field of the last element AddOrChangeUserUserStateThe
        ///  value of the RID field of the last element ChangeGroupMembershipGroupMemberStateThe
        ///  value of the RID field of the last element 
        ///  AddOrChangeAliasAliasState0x00000000ChangeAliasMembershipAliasMemberState0x00000000Any
        ///  other value not previously listedNormalState0x00000000
        /// </param>
        /// <param name="deltaArray">
        ///  A pointer to a NETLOGON_DELTA_ENUM_ARRAY structure,
        ///  as specified in MS-NRPC, that contains an array of
        ///  enumerated changes (deltas) to the specified database.
        /// </param>
        /// <param name="preferredMaximumLength">
        ///  The value that specifies the preferred maximum size,
        ///  in bytes, of data referenced in the deltaArray parameter.
        ///  This is not a hard upper limit, but serves as a guide
        ///  to the server. The server SHOULDwindows stops including
        ///  elements in the returned deltaArray once the size of
        ///  the returned data equals or exceeds the value of the
        ///  preferredMaximumLength parameter. The server SHOULD stop including elements
        ///  in the returned deltaArray once the size of the returned
        ///  data equals or exceeds the value of the preferredMaximumLength
        ///  parameter. It is up to the client implementation to
        ///  choose the value for this parameter.
        /// </param>
        /// <returns>
        /// the NTSTATUS.
        /// </returns>
        public NtStatus NetrDatabaseSync2(
            string primaryName,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            ref _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            DatabaseID_Values databaseID,
            _SYNC_STATE restartState,
            ref uint? syncContext,
            out _NETLOGON_DELTA_ENUM_ARRAY? deltaArray,
            uint preferredMaximumLength)
        {
            const ushort Opnum = 16;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrPrimaryName = Marshal.StringToHGlobalUni(primaryName);
            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrAuthenticator = TypeMarshal.ToIntPtr(authenticator);
            SafeIntPtr ptrReturnAuthenticatorIn = TypeMarshal.ToIntPtr(returnAuthenticator);
            SafeIntPtr ptrSyncContext = TypeMarshal.ToIntPtr(syncContext);

            paramList = new Int3264[] 
            {
                ptrPrimaryName,
                ptrComputerName,
                ptrAuthenticator,
                ptrReturnAuthenticatorIn,
                (uint)databaseID,
                (uint)restartState,
                ptrSyncContext,
                IntPtr.Zero,
                preferredMaximumLength,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrReturnAuthenticatorOut = outParamList[3];
                returnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(ptrReturnAuthenticatorOut);

                syncContext = TypeMarshal.ToNullableStruct<uint>(outParamList[6]);

                IntPtr ptrDeltaArray = outParamList[7];
                ptrDeltaArray = Marshal.ReadIntPtr(ptrDeltaArray);
                deltaArray = TypeMarshal.ToNullableStruct<_NETLOGON_DELTA_ENUM_ARRAY>(ptrDeltaArray);

                retVal = outParamList[9].ToInt32();
            }

            ptrPrimaryName.Dispose();
            ptrComputerName.Dispose();
            ptrAuthenticator.Dispose();
            ptrReturnAuthenticatorIn.Dispose();
            ptrSyncContext.Dispose();

            return (NtStatus)retVal;
        }

        /// <summary>
        ///  The NetrDatabaseRedo method is used by a BDC to request
        ///  information about a single account from the PDC. Opnum
        ///  : 17 
        /// </summary>
        /// <param name="primaryName">
        ///  The custom RPC binding handle, as specified in MS-NRPC
        ///  , representing the connection to the PDC.
        /// </param>
        /// <param name="computerName">
        ///  The null-terminated Unicode string that contains the
        ///  NetBIOS name of the BDC calling this method.
        /// </param>
        /// <param name="authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in MS-NRPC, that contains the client authenticator.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in MS-NRPC, that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="changeLogEntry">
        ///  A pointer to a buffer that contains a CHANGELOG_ENTRY
        ///  structure, specified as follows, for the account being
        ///  queried.
        /// </param>
        /// <param name="changeLogEntrySize">
        ///  The size, in bytes, of the buffer pointed to by the
        ///  changeLogEntry parameter.
        /// </param>
        /// <param name="deltaArray">
        ///  A pointer to a NETLOGON_DELTA_ENUM_ARRAY structure,
        ///  as specified in MS-NRPC, that contains an array of
        ///  enumerated database changes for the account being queried.
        /// </param>
        /// <returns>
        ///The NTSTATUS.
        /// </returns>
        public NtStatus NetrDatabaseRedo(
            string primaryName,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            ref _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            byte[] changeLogEntry,
            uint changeLogEntrySize,
            out _NETLOGON_DELTA_ENUM_ARRAY? deltaArray)
        {
            const ushort Opnum = 17;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrPrimaryName = Marshal.StringToHGlobalUni(primaryName);
            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrAuthenticator = TypeMarshal.ToIntPtr(authenticator);
            SafeIntPtr ptrReturnAuthenticatorIn = TypeMarshal.ToIntPtr(returnAuthenticator);
            SafeIntPtr ptrChangeLogEntry = IntPtrUtility.ArrayToPtr(changeLogEntry);

            paramList = new Int3264[] 
            {
                ptrPrimaryName,
                ptrComputerName,
                ptrAuthenticator,
                ptrReturnAuthenticatorIn,
                ptrChangeLogEntry,
                changeLogEntrySize,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrReturnAuthenticatorOut = outParamList[3];
                returnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(ptrReturnAuthenticatorOut);

                IntPtr ptrDeltaArray = outParamList[6];
                ptrDeltaArray = Marshal.ReadIntPtr(ptrDeltaArray);
                deltaArray = TypeMarshal.ToNullableStruct<_NETLOGON_DELTA_ENUM_ARRAY>(ptrDeltaArray);

                retVal = outParamList[7].ToInt32();
            }

            ptrPrimaryName.Dispose();
            ptrComputerName.Dispose();
            ptrAuthenticator.Dispose();
            ptrReturnAuthenticatorIn.Dispose();
            ptrChangeLogEntry.Dispose();

            return (NtStatus)retVal;
        }

        /// <summary>
        ///  The NetrLogonControl2Ex method executes windows-specific
        ///  administrative actions that pertain to the Netlogon
        ///  server operation. It is used to query the status and
        ///  control the actions of the Netlogon server. Opnum :
        ///  18 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="functionCode">
        ///  The control operation to be performed; MUST be one of
        ///  the following values. The following restrictions apply
        ///  to the values of the functionCode parameter in windows_nt_4_0,
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
        ///  in MS-NRPC, that contains specific data required by
        ///  the query.
        /// </param>
        /// <param name="buffer">
        ///  NETLOGON_CONTROL_QUERY_INFORMATION structure, as specified
        ///  in MS-NRPC, that contains the specific query results,
        ///  with a level of verbosity as specified in queryLevel.
        /// </param>
        /// <returns>
        /// Net API status
        /// </returns>
        public NetApiStatus NetrLogonControl2Ex(
            string serverName,
            uint functionCode,
            uint queryLevel,
            _NETLOGON_CONTROL_DATA_INFORMATION? data,
            out _NETLOGON_CONTROL_QUERY_INFORMATION? buffer)
        {
            const ushort Opnum = 18;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrServerName = Marshal.StringToHGlobalUni(serverName);
            SafeIntPtr ptrData = TypeMarshal.ToIntPtr(data, functionCode, null, null);

            paramList = new Int3264[] 
            {
                ptrServerName,
                functionCode,
                queryLevel,
                ptrData,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrBuffer = outParamList[4];
                buffer = null;
                if (queryLevel > 0 && queryLevel < 5)
                {
                    buffer = TypeMarshal.ToNullableStruct<_NETLOGON_CONTROL_QUERY_INFORMATION>(
                        ptrBuffer,
                        queryLevel,
                        null,
                        null);
                }

                retVal = outParamList[5].ToInt32();
            }

            ptrServerName.Dispose();
            ptrData.Dispose();

            return (NetApiStatus)retVal;
        }

        /// <summary>
        ///  The NetrLogonControl2Ex method executes windows-specific
        ///  administrative actions that pertain to the Netlogon
        ///  server operation. It is used to query the status and
        ///  control the actions of the Netlogon server. Opnum :
        ///  18 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="functionCode">
        ///  The control operation to be performed; MUST be one of
        ///  the following values. The following restrictions apply
        ///  to the values of the functionCode parameter in windows_nt_4_0,
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
        ///  in MS-NRPC, that contains specific data required by
        ///  the query.
        /// </param>
        /// <param name="buffer">
        ///  NETLOGON_CONTROL_QUERY_INFORMATION structure, as specified
        ///  in MS-NRPC, that contains the specific query results,
        ///  with a level of verbosity as specified in queryLevel.
        /// </param>
        /// <returns>
        /// Net API status
        /// </returns>
        public NetApiStatus NetrLogonControl2Ex(
            string serverName,
            FunctionCode_Values functionCode,
            QueryLevel_Values queryLevel,
            _NETLOGON_CONTROL_DATA_INFORMATION? data,
            out _NETLOGON_CONTROL_QUERY_INFORMATION? buffer)
        {
            const ushort Opnum = 18;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrServerName = Marshal.StringToHGlobalUni(serverName);
            SafeIntPtr ptrData = TypeMarshal.ToIntPtr(data, functionCode, null, null);

            paramList = new Int3264[] 
            {
                ptrServerName,
                (uint)functionCode,
                (uint)queryLevel,
                ptrData,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrBuffer = outParamList[4];
                buffer = TypeMarshal.ToNullableStruct<_NETLOGON_CONTROL_QUERY_INFORMATION>(
                    ptrBuffer,
                    queryLevel,
                    null,
                    null);

                retVal = outParamList[5].ToInt32();
            }

            ptrServerName.Dispose();
            ptrData.Dispose();

            return (NetApiStatus)retVal;
        }

        /// <summary>
        ///  The NetrEnumerateTrustedDomains method returns a set
        ///  of trusted domain names. Opnum: 19 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle, as specified in MS-NRPC.
        /// </param>
        /// <param name="domainNameBuffer">
        ///  A pointer to a DOMAIN_NAME_BUFFER structure, as specified
        ///  in MS-NRPC, that contains a list of trusted domain
        ///  names. The format of domain names contained in the
        ///  buffer is specified in MS-NRPC.
        /// </param>
        /// <returns>
        ///  The NTSTATUS.
        /// </returns>
        public NtStatus NetrEnumerateTrustedDomains(
            string serverName,
            out _DOMAIN_NAME_BUFFER? domainNameBuffer)
        {
            const ushort Opnum = 19;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrServerName = Marshal.StringToHGlobalUni(serverName);

            paramList = new Int3264[] 
            {
                ptrServerName,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrDomainNameBuffer = outParamList[1];
                domainNameBuffer = TypeMarshal.ToNullableStruct<_DOMAIN_NAME_BUFFER>(ptrDomainNameBuffer);

                retVal = outParamList[2].ToInt32();
            }

            ptrServerName.Dispose();

            return (NtStatus)retVal;
        }

        /// <summary>
        ///  The DsrGetDcName methodSupported in windows_2000, windows_xp,
        ///  windows_server_2003, windows_vista, and windows_server_2008,
        ///  windows_7, and windows_server_7. It is a predecessor to
        ///  the DsrGetDcNameEx2 method. The method returns
        ///  information about a domain controller in the specified
        ///  domain. All parameters of this method have the same
        ///  meanings as the identically named parameters of the
        ///  DsrGetDcNameEx2 method, except for the siteGuid parameter,
        ///  detailed as follows. Opnum: 20 
        /// </summary>
        /// <param name="computerName">
        ///  computerName parameter.
        /// </param>
        /// <param name="domainName">
        ///  domainName parameter.
        /// </param>
        /// <param name="domainGuid">
        ///  domainGuid parameter.
        /// </param>
        /// <param name="siteGuid">
        ///  This parameter MUST be NULL and ignored upon receipt.
        /// </param>
        /// <param name="flags">
        ///  flags parameter.
        /// </param>
        /// <param name="domainControllerInfo">
        ///  domainControllerInfo parameter.
        /// </param>
        /// <returns>
        /// Net API status
        /// </returns>
        public NetApiStatus DsrGetDcName(
            string computerName,
            string domainName,
            Guid? domainGuid,
            Guid? siteGuid,
            uint flags,
            out _DOMAIN_CONTROLLER_INFOW? domainControllerInfo)
        {
            const ushort Opnum = 20;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrDomainName = Marshal.StringToHGlobalUni(domainName);
            SafeIntPtr ptrDomainGuid = TypeMarshal.ToIntPtr(domainGuid);
            SafeIntPtr ptrSiteGuid = TypeMarshal.ToIntPtr(siteGuid);

            paramList = new Int3264[] 
            {
                ptrComputerName,
                ptrDomainName,
                ptrDomainGuid,
                ptrSiteGuid,
                flags,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrDomainControllerInfo = outParamList[5];
                ptrDomainControllerInfo = Marshal.ReadIntPtr(ptrDomainControllerInfo);
                domainControllerInfo = TypeMarshal.ToNullableStruct<_DOMAIN_CONTROLLER_INFOW>(ptrDomainControllerInfo);

                retVal = outParamList[6].ToInt32();
            }

            ptrComputerName.Dispose();
            ptrDomainName.Dispose();
            ptrDomainGuid.Dispose();
            ptrSiteGuid.Dispose();

            return (NetApiStatus)retVal;
        }

        /// <summary>
        ///  The NetrLogonDummyRoutine1 method is no longer supported.
        ///  It serves as a placeholder in the IDL file for the
        ///  RPCOpnum value 21.The NetrLogonDummyRoutine1 method
        ///  is deprecated by NetrLogonGetCapabilities. It serves
        ///  as a placeholder in the IDL file for the RPCOpnum value
        ///  21.Supported in windows_nt, windows_2000, windows_xp,
        ///  windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. Opnum: 21 
        /// </summary>
        /// <param name="serverName">
        ///  A LOGONSRV_HANDLE Unicode string handle of the server
        ///  that is handling the request.
        /// </param>
        /// <param name="computerName">
        ///  A string that contains the name of the computer.
        /// </param>
        /// <param name="authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure that
        ///  contains the client authenticator.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure that
        ///  contains the server return authenticator.
        /// </param>
        /// <param name="queryLevel">
        ///  Specifies the level of information to return from the
        ///  domain controller being queried. A value of 0x00000001
        ///  causes a NETLOGON_DOMAIN_INFO structure that contains
        ///  information about the DC to be returned.
        /// </param>
        /// <param name="serverCapabilities">
        ///  A pointer to a 32-bit set of bit flags that identify 
        ///  the server's capabilities.
        /// </param>
        /// <returns>
        ///  the NtStatus.
        /// </returns>
        public NtStatus NetrLogonGetCapabilities(
            string serverName,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            ref _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            uint queryLevel,
            out _NETLOGON_CAPABILITIES? serverCapabilities)
        {
            const ushort Opnum = 21;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrServerName = Marshal.StringToHGlobalUni(serverName);
            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrAuthenticator = TypeMarshal.ToIntPtr(authenticator);
            SafeIntPtr ptrReturnAuthenticatorIn = TypeMarshal.ToIntPtr(returnAuthenticator);

            paramList = new Int3264[] 
            {
                ptrServerName,
                ptrComputerName,
                ptrAuthenticator,
                ptrReturnAuthenticatorIn,
                queryLevel,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrReturnAuthenticatorOut = outParamList[3];
                returnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(ptrReturnAuthenticatorOut);

                IntPtr ptrServerCapabilities = outParamList[5];
                serverCapabilities = TypeMarshal.ToNullableStruct<_NETLOGON_CAPABILITIES>(
                    ptrServerCapabilities,
                    queryLevel,
                    null,
                    null);

                retVal = outParamList[6].ToInt32();
            }

            ptrServerName.Dispose();
            ptrComputerName.Dispose();
            ptrAuthenticator.Dispose();
            ptrReturnAuthenticatorIn.Dispose();

            return (NtStatus)retVal;
        }

        /// <summary>
        ///  The NetrLogonSetServiceBitsSupported in windows_2000_server,
        ///  windows_xp, windows_server_2003. This method is used to
        ///  notify Netlogon whether a domain controller is running
        ///  specified services, as detailed in the following section.
        ///  Opnum: 22 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle, as specified in MS-NRPC.
        ///  , representing the connection to a DC.
        /// </param>
        /// <param name="serviceBitsOfInterest">
        ///  A set of bit flags used as a mask to indicate which
        ///  service's state (running or not running) is being set
        ///  by this call. The value is constructed from zero or
        ///  more bit flags from the following table.
        /// </param>
        /// <param name="serviceBits">
        ///  A set of bit flags used as a mask to indicate whether
        ///  the service indicated by serviceBitsOfInterest is running
        ///  or not. If the flag is set to 0, the corresponding
        ///  service indicated by serviceBitsOfInterest is not running.
        ///  Otherwise, if the flag is set to 1, the corresponding
        ///  service indicated by serviceBitsOfInterest is running.
        ///  The value is constructed from zero or more bit flags
        ///  from the following table.
        /// </param>
        /// <returns>
        ///  the NtStatus.
        /// </returns>
        public NtStatus NetrLogonSetServiceBits(
            string serverName,
            uint serviceBitsOfInterest,
            uint serviceBits)
        {
            const ushort Opnum = 22;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrServerName = Marshal.StringToHGlobalUni(serverName);

            paramList = new Int3264[] 
            {
                ptrServerName,
                serviceBitsOfInterest,
                serviceBits,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                retVal = outParamList[3].ToInt32();
            }

            ptrServerName.Dispose();

            return (NtStatus)retVal;
        }

        /// <summary>
        ///The NetrLogonGetTrustRid method is supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_2008_R2. It is used to obtain
        ///  the RID of the account whose password is used by domain
        ///  controllers in the specified domain for establishing
        ///  the secure channel from the server receiving this call.
        ///  Opnum: 23 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle, as specified in MS-NRPC
        ///  serverName SHOULD be NULL. In windows_server_2008
        ///  and windows_server_7, serverName is NULL because this
        ///  method is restricted to local callers.
        /// </param>
        /// <param name="domainName">
        ///  The null-terminated Unicode string that contains the
        ///  DNS or NetBIOS name of the primary or trusted domain.
        ///  If this parameter is NULL, this method uses the name
        ///  of the primary domain of the server.
        /// </param>
        /// <param name="rid">
        ///  A pointer to an unsigned long that receives the RID
        ///  of the account.
        /// </param>
        /// <returns>
        ///  Net API status.
        /// </returns>
        public NetApiStatus NetrLogonGetTrustRid(
            string serverName,
            string domainName,
            out uint? rid)
        {
            const ushort Opnum = 23;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrServerName = Marshal.StringToHGlobalUni(serverName);
            SafeIntPtr ptrDomainName = Marshal.StringToHGlobalUni(domainName);

            paramList = new Int3264[] 
            {
                ptrServerName,
                ptrDomainName,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                rid = TypeMarshal.ToNullableStruct<uint>(outParamList[2]);

                retVal = outParamList[3].ToInt32();
            }

            ptrServerName.Dispose();
            ptrDomainName.Dispose();

            return (NetApiStatus)retVal;
        }

        /// <summary>
        /// The NetrLogonComputeServerDigest method is supported in
        ///  windows_2000_server, windows_xp, windows_server_2003,
        ///  windows_vista, windows_server_2008, windows_7, and
        ///  windows_server_7. It computes a cryptographic digest of
        ///  a message by using the MD5 message-digest algorithm,
        ///  as specified in [RFC1321]. This method is called by
        ///  a client computer against a server and is used to compute
        ///  a message digest, as specified in this section. The
        ///  client MAY then call the NetrLogonComputeClientDigest
        ///  method (as specified in MS-NRPC) and compare the digests
        ///  to ensure that the server that it communicates with
        ///  knows the shared secret between the client machine
        ///  and the domain. Opnum: 24 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle, as specified in MS-NRPC.
        /// </param>
        /// <param name="rid">
        ///  The RID of the machine account for which the digest
        ///  is to be computed. The NetrLogonGetTrustRid method,
        ///  as specified in MS-NRPC, is used to obtain the RID.
        /// </param>
        /// <param name="message">
        ///  A pointer to buffer that contains the message to compute
        ///  the digest.
        /// </param>
        /// <param name="messageSize">
        ///  The length of the data referenced by the message parameter,
        ///  in bytes.
        /// </param>
        /// <param name="newMessageDigest">
        ///  A 128-bit MD5 digest of the current machine account
        ///  password and the message in the Message buffer. The
        ///  machine account is identified by the rid parameter.
        /// </param>
        /// <param name="oldMessageDigest">
        ///  A 128-bit MD5 digest of the previous machine account
        ///  password, if present, and the message in the Message
        ///  buffer. If no previous machine account password exists,
        ///  then the current password is used. The machine account
        ///  is identified by the rid parameter.
        /// </param>
        /// <returns>
        /// Net API status.
        /// </returns>
        public NetApiStatus NetrLogonComputeServerDigest(
            string serverName,
            uint rid,
            byte[] message,
            uint messageSize,
            out byte[] newMessageDigest,
            out byte[] oldMessageDigest)
        {
            const ushort Opnum = 24;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrServerName = Marshal.StringToHGlobalUni(serverName);
            SafeIntPtr ptrMessage = IntPtrUtility.ArrayToPtr(message);

            paramList = new Int3264[] 
            {
                ptrServerName,
                rid,
                ptrMessage,
                messageSize,
                IntPtr.Zero,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrNewMessageDigest = outParamList[4];
                newMessageDigest = IntPtrUtility.PtrToArray<byte>(ptrNewMessageDigest, 16); ////size_is(16)

                IntPtr ptrOldMessageDigest = outParamList[5];
                oldMessageDigest = IntPtrUtility.PtrToArray<byte>(ptrOldMessageDigest, 16); ////size_is(16)

                retVal = outParamList[6].ToInt32();
            }

            ptrServerName.Dispose();
            ptrMessage.Dispose();

            return (NetApiStatus)retVal;
        }

        /// <summary>
        ///The NetrLogonComputeClientDigest method is supported in
        ///  windows_2000_server, windows_xp, windows_server_2003,
        ///  windows_vista, windows_server_2008, windows_7, and
        ///  windows_server_7. It is used by a client to compute a
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
        /// <param name="serverName">
        ///  The custom RPC binding handle, as specified in MS-NRPC.
        /// </param>
        /// <param name="domainName">
        ///  A pointer to a null-terminated Unicode string that contains
        ///  the DNS or NetBIOS name of the trusted domain. If this
        ///  parameter is NULL, the domain of which the client computer
        ///  is a member is used.
        /// </param>
        /// <param name="message">
        ///  A pointer to a buffer that contains the message for
        ///  which the digest is to be computed.
        /// </param>
        /// <param name="messageSize">
        ///  The length, in bytes, of the message parameter.
        /// </param>
        /// <param name="newMessageDigest">
        ///  A 128-bit MD5 digest of the current computer account
        ///  password and the message in the Message buffer.
        /// </param>
        /// <param name="oldMessageDigest">
        ///  A 128-bit MD5 digest of the previous machine account
        ///  password and the message in the Message buffer. If
        ///  no previous computer account password exists, the current
        ///  password is used.
        /// </param>
        /// <returns>
        /// Net API status.
        /// </returns>
        public NetApiStatus NetrLogonComputeClientDigest(
            string serverName,
            string domainName,
            byte[] message,
            uint messageSize,
            out byte[] newMessageDigest,
            out byte[] oldMessageDigest)
        {
            const ushort Opnum = 25;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrServerName = Marshal.StringToHGlobalUni(serverName);
            SafeIntPtr ptrDomainName = Marshal.StringToHGlobalUni(domainName);
            SafeIntPtr ptrMessage = IntPtrUtility.ArrayToPtr(message);

            paramList = new Int3264[] 
            {
                ptrServerName,
                ptrDomainName,
                ptrMessage,
                messageSize,
                IntPtr.Zero,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrNewMessageDigest = outParamList[4];
                newMessageDigest = IntPtrUtility.PtrToArray<byte>(ptrNewMessageDigest, 16); ////size_is(16)

                IntPtr ptrOldMessageDigest = outParamList[5];
                oldMessageDigest = IntPtrUtility.PtrToArray<byte>(ptrOldMessageDigest, 16); ////size_is(16)

                retVal = outParamList[6].ToInt32();
            }

            ptrServerName.Dispose();
            ptrDomainName.Dispose();
            ptrMessage.Dispose();

            return (NetApiStatus)retVal;
        }

        /// <summary>
        ///  The NetrServerAuthenticate3 method mutually authenticates
        ///  the client and the server and establishes the session
        ///  key to be used for the secure channel message protection
        ///  between the client and the server, supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. It is called after
        ///  the NetrServerReqChallenge method, as specified in
        ///  MS-NRPC. Opnum: 26 
        /// </summary>
        /// <param name="primaryName">
        ///  The custom RPC binding handle, as specified in MS-NRPC.
        /// </param>
        /// <param name="accountName">
        ///  A null-terminated Unicode string that identifies the
        ///  name of the account that contains the secret key (password)
        ///  that is shared between the client and the server, as
        ///  specified in MS-NRPC. In windows, all machine account
        ///  names are the name of the machine with a $ (dollar
        ///  sign) appended. If there is a period at the end of
        ///  the account name, it is ignored during processing.
        /// </param>
        /// <param name="secureChannelType">
        ///  A NETLOGON_SECURE_CHANNEL_TYPE enumerated value, as
        ///  specified in MS-NRPC, that indicates the type of the
        ///  secure channel being established by this call.
        /// </param>
        /// <param name="computerName">
        ///  A null-terminated Unicode string that contains the NetBIOS
        ///  name of the client computer calling this method.
        /// </param>
        /// <param name="clientCredential">
        ///  A pointer to a NETLOGON_CREDENTIAL structure, as specified
        ///  in MS-NRPC, that contains the supplied client credentials,
        ///  as specified in MS-NRPC.
        /// </param>
        /// <param name="serverCredential">
        ///  A pointer to a NETLOGON_CREDENTIAL structure, as specified
        ///  in MS-NRPC, that contains the returned server credentials.
        /// </param>
        /// <param name="negotiateFlags">
        ///  A pointer to a 32-bit set of bit flags that indicate
        ///  features supported. As input, the set of flags are
        ///  those requested by the client and SHOULD be the same
        ///  as ClientCapabilities. As output, they are the bit-wise
        ///  AND of the client's requested capabilities and the
        ///  server's ServerCapabilities. For more details, see
        ///  MS-NRPC.
        /// </param>
        /// <param name="accountRid">
        ///  A pointer that receives the RID of the account specified
        ///  by the accountName parameter. ([MS-ADTS]
        ///  describes how this RID is assigned at account creation
        ///  time.) This value is stored in the AccountRid ADM element
        ///  within the ClientSessionInfo table.
        /// </param>
        /// <returns>
        /// the NtStatus.
        /// </returns>
        public NtStatus NetrServerAuthenticate3(
            string primaryName,
            string accountName,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType,
            string computerName,
            _NETLOGON_CREDENTIAL? clientCredential,
            out _NETLOGON_CREDENTIAL? serverCredential,
            ref uint? negotiateFlags,
            out uint? accountRid)
        {
            const ushort Opnum = 26;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrPrimaryName = Marshal.StringToHGlobalUni(primaryName);
            SafeIntPtr ptrAccountName = Marshal.StringToHGlobalUni(accountName);
            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrClientCredential = TypeMarshal.ToIntPtr(clientCredential);
            SafeIntPtr ptrNegotiateFlags = TypeMarshal.ToIntPtr(negotiateFlags);

            paramList = new Int3264[] 
            {
                ptrPrimaryName,
                ptrAccountName,
                (uint)secureChannelType,
                ptrComputerName,
                ptrClientCredential,
                IntPtr.Zero,
                ptrNegotiateFlags,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrServerCredential = outParamList[5];
                serverCredential = TypeMarshal.ToNullableStruct<_NETLOGON_CREDENTIAL>(ptrServerCredential);

                negotiateFlags = TypeMarshal.ToNullableStruct<uint>(outParamList[6]);

                accountRid = TypeMarshal.ToNullableStruct<uint>(outParamList[7]);

                retVal = outParamList[8].ToInt32();
            }

            ptrPrimaryName.Dispose();
            ptrAccountName.Dispose();
            ptrComputerName.Dispose();
            ptrClientCredential.Dispose();
            ptrNegotiateFlags.Dispose();

            return (NtStatus)retVal;
        }

        /// <summary>
        ///  The DsrGetDcNameEx method is supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. It is a predecessor to
        ///  the DsrGetDcNameEx2 method. The method returns information
        ///  about a domain controller in the specified domain and
        ///  site. All parameters of this method have the same meanings
        ///  as the identically named parameters of the DsrGetDcNameEx2
        ///  method. Opnum: 27 
        /// </summary>
        /// <param name="computerName">
        ///  computerName parameter.
        /// </param>
        /// <param name="domainName">
        ///  domainName parameter.
        /// </param>
        /// <param name="domainGuid">
        ///  domainGuid parameter.
        /// </param>
        /// <param name="siteName">
        ///  siteName parameter.
        /// </param>
        /// <param name="flags">
        ///  flags parameter.
        /// </param>
        /// <param name="domainControllerInfo">
        ///  domainControllerInfo parameter.
        /// </param>
        /// <returns>
        /// Net API status.
        /// </returns>
        public NetApiStatus DsrGetDcNameEx(
            string computerName,
            string domainName,
            Guid? domainGuid,
            string siteName,
            uint flags,
            out _DOMAIN_CONTROLLER_INFOW? domainControllerInfo)
        {
            const ushort Opnum = 27;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrDomainName = Marshal.StringToHGlobalUni(domainName);
            SafeIntPtr ptrDomainGuid = TypeMarshal.ToIntPtr(domainGuid);
            SafeIntPtr ptrSiteName = Marshal.StringToHGlobalUni(siteName);

            paramList = new Int3264[] 
            {
                ptrComputerName,
                ptrDomainName,
                ptrDomainGuid,
                ptrSiteName,
                flags,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
             RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                true,
                responseStub,
                rpceClientTransport.Context.PackedDataRepresentationFormat,
                paramList))
            {
                // domainControllerInfo is double pointer (**)
                // ptrDomainControllerInfo won't be null, read pointer inside directly.
                IntPtr ptrDomainControllerInfo;
                ptrDomainControllerInfo = outParamList[5];
                ptrDomainControllerInfo = Marshal.ReadIntPtr(ptrDomainControllerInfo);
                domainControllerInfo = TypeMarshal.ToNullableStruct<_DOMAIN_CONTROLLER_INFOW>(ptrDomainControllerInfo);

                retVal = outParamList[6].ToInt32();
            }

            ptrComputerName.Dispose();
            ptrDomainName.Dispose();
            ptrDomainGuid.Dispose();
            ptrSiteName.Dispose();

            return (NetApiStatus)retVal;
        }

        /// <summary>
        ///  The DsrGetSiteName method is supported in windows_2000,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. It returns the site name
        ///  for the specified computer that receives this call.
        ///  Opnum: 28 
        /// </summary>
        /// <param name="computerName">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="siteName">
        ///  A null-terminated Unicode string that contains the name
        ///  of the site in which the computer that receives this
        ///  call resides.
        /// </param>
        /// <returns>
        /// Net API status.
        /// </returns>
        public NetApiStatus DsrGetSiteName(
            string computerName,
            out string siteName)
        {
            const ushort Opnum = 28;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);

            paramList = new Int3264[] 
            {
                ptrComputerName,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrSiteName = outParamList[1];

                // wchar_t** SiteName
                ptrSiteName = Marshal.ReadIntPtr(ptrSiteName);
                siteName = Marshal.PtrToStringUni(ptrSiteName);

                retVal = outParamList[2].ToInt32();
            }

            ptrComputerName.Dispose();

            return (NetApiStatus)retVal;
        }

        /// <summary>
        ///  The NetrLogonGetDomainInfo method is supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, and
        ///  windows_server_2008, windows_7, and windows_server_7.
        ///It  returns information that describes the current domain
        ///  to which the specified client belongs. Opnum: 29 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle, as specified in MS-NRPC.
        /// </param>
        /// <param name="computerName">
        ///  The null-terminated Unicode string that contains the
        ///  name of the client computer issuing the request.
        /// </param>
        /// <param name="authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in MS-NRPC, that contains the client authenticator.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in MS-NRPC, that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="level">
        ///  The information level requested by the client. The buffer
        ///  contains one of the following structures, based on
        ///  the value of this field.
        /// </param>
        /// <param name="wkstaBuffer">
        ///  A pointer to a NETLOGON_WORKSTATION_INFORMATION structure,
        ///  as specified in MS-NRPC, that contains information
        ///  about the client workstation.
        /// </param>
        /// <param name="domBuffer">
        ///  A pointer to a NETLOGON_DOMAIN_INFORMATION structure,
        ///  as specified in MS-NRPC, that contains information
        ///  about the domain or policy information.
        /// </param>
        /// <returns>
        /// the NtStatus.
        /// </returns>
        public NtStatus NetrLogonGetDomainInfo(
            string serverName,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            ref _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            Level_Values level,
            _NETLOGON_WORKSTATION_INFORMATION? wkstaBuffer,
            out _NETLOGON_DOMAIN_INFORMATION? domBuffer)
        {
            const ushort Opnum = 29;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrServerName = Marshal.StringToHGlobalUni(serverName);
            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrAuthenticator = TypeMarshal.ToIntPtr(authenticator);
            SafeIntPtr ptrReturnAuthenticatorIn = TypeMarshal.ToIntPtr(returnAuthenticator);
            SafeIntPtr ptrWkstaBuffer = TypeMarshal.ToIntPtr(wkstaBuffer, level, null, null);

            paramList = new Int3264[] 
            {
                ptrServerName,
                ptrComputerName,
                ptrAuthenticator,
                ptrReturnAuthenticatorIn,
                (uint)level,
                ptrWkstaBuffer,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrReturnAuthenticatorOut = outParamList[3];
                returnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(ptrReturnAuthenticatorOut);

                IntPtr ptrDomBuffer = outParamList[6];
                domBuffer = TypeMarshal.ToNullableStruct<_NETLOGON_DOMAIN_INFORMATION>(
                    ptrDomBuffer,
                    level,
                    null,
                    null);

                retVal = outParamList[7].ToInt32();
            }

            ptrServerName.Dispose();
            ptrComputerName.Dispose();
            ptrAuthenticator.Dispose();
            ptrReturnAuthenticatorIn.Dispose();
            ptrWkstaBuffer.Dispose();

            return (NtStatus)retVal;
        }

        /// <summary>
        ///  The NetrServerPasswordSet2 method is supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. It allows the client
        ///  to set a new clear text password for an account used
        ///  by the domain controller (as specified in MS-NRPC)
        ///  for setting up the secure channel from the client. A
        ///  domain member uses this function to periodically change
        ///  its machine account password. A PDC uses this function
        ///  to periodically change the trust password for all directly
        ///  trusted domains. By default, the period is 30 days
        ///  in windows_2000_server, windows_xp, windows_server_2003,
        ///  windows_vista, windows_server_2008, windows_7, and
        ///  windows_server_7. Opnum: 30 
        /// </summary>
        /// <param name="primaryName">
        ///  The custom RPC binding handle, as specified in MS-NRPC.
        /// </param>
        /// <param name="accountName">
        ///  The null-terminated Unicode string that contains the
        ///  name of the account whose password is being changed. In
        ///  windows, all machine account names are the name of
        ///  the machine with a $ (dollar sign) appended.
        /// </param>
        /// <param name="secureChannelType">
        ///  An enumerated value that describes the secure channel
        ///  to be used for authentication, as specified in section
        /// </param>
        /// <param name="computerName">
        ///  The null-terminated Unicode string that contains the
        ///  NetBIOS name of the computer making the request.
        /// </param>
        /// <param name="authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the encrypted
        ///  logon credential and a time stamp.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="clearNewPassword">
        ///  A pointer to an NL_TRUST_PASSWORD structure, as specified
        ///  in section , that contains the new password encrypted
        ///  as specified in Calling NetrServerPasswordSet2.
        /// </param>
        /// <returns>
        /// the NtStatus.
        /// </returns>
        public NtStatus NetrServerPasswordSet2(
            string primaryName,
            string accountName,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            out _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            _NL_TRUST_PASSWORD? clearNewPassword)
        {
            const ushort Opnum = 30;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrPrimaryName = Marshal.StringToHGlobalUni(primaryName);
            SafeIntPtr ptrAccountName = Marshal.StringToHGlobalUni(accountName);
            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrAuthenticator = TypeMarshal.ToIntPtr(authenticator);
            SafeIntPtr ptrClearNewPassword = TypeMarshal.ToIntPtr(clearNewPassword);

            paramList = new Int3264[] 
            {
                ptrPrimaryName,
                ptrAccountName,
                (uint)secureChannelType,
                ptrComputerName,
                ptrAuthenticator,
                IntPtr.Zero,
                ptrClearNewPassword,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrReturnAuthenticator = outParamList[5];
                returnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(ptrReturnAuthenticator);

                retVal = outParamList[7].ToInt32();
            }

            ptrPrimaryName.Dispose();
            ptrAccountName.Dispose();
            ptrComputerName.Dispose();
            ptrAuthenticator.Dispose();
            ptrClearNewPassword.Dispose();

            return (NtStatus)retVal;
        }

        /// <summary>
        ///  The NetrServerPasswordGet method is supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. It allows a domain controller
        ///  to get a machine account password from the DC with
        ///  the PDC role in the domain. Opnum: 31 
        /// </summary>
        /// <param name="primaryName">
        ///  The custom RPC binding handle, as specified in MS-NRPC.
        /// </param>
        /// <param name="accountName">
        ///  A null-terminated Unicode string that contains the name
        ///  of the account to retrieve the password for. For machine
        ///  accounts, the account name is the machine name appended
        ///  with a "$" character.
        /// </param>
        /// <param name="accountType">
        ///  A NETLOGON_SECURE_CHANNEL_TYPE enumerated value, as
        ///  specified in section , that describes the secure channel
        ///  to be used for authentication.
        /// </param>
        /// <param name="computerName">
        ///  A null-terminated Unicode string that contains the NetBIOS
        ///  name of the BDC making the call.
        /// </param>
        /// <param name="authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in MS-NRPC, that contains the encrypted
        ///  logon credential and a time stamp.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="encryptedNtOwfPassword">
        ///  A pointer to an ENCRYPTED_NT_OWF_PASSWORD structure,
        ///  as specified in [MS-SAMR] section , that contains the
        ///  OWF password of the account.
        /// </param>
        /// <returns>
        /// the NtStatus.
        /// </returns>
        public NtStatus NetrServerPasswordGet(
            string primaryName,
            string accountName,
            _NETLOGON_SECURE_CHANNEL_TYPE accountType,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            out _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            out _NT_OWF_PASSWORD? encryptedNtOwfPassword)
        {
            const ushort Opnum = 31;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrPrimaryName = Marshal.StringToHGlobalUni(primaryName);
            SafeIntPtr ptrAccountName = Marshal.StringToHGlobalUni(accountName);
            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrAuthenticator = TypeMarshal.ToIntPtr(authenticator);

            paramList = new Int3264[] 
            {
                ptrPrimaryName,
                ptrAccountName,
                (uint)accountType,
                ptrComputerName,
                ptrAuthenticator,
                IntPtr.Zero,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrReturnAuthenticator = outParamList[5];
                returnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(ptrReturnAuthenticator);

                IntPtr ptrEncryptedNtOwfPassword = outParamList[6];
                encryptedNtOwfPassword = TypeMarshal.ToNullableStruct<_NT_OWF_PASSWORD>(ptrEncryptedNtOwfPassword);

                retVal = outParamList[7].ToInt32();
            }

            ptrPrimaryName.Dispose();
            ptrAccountName.Dispose();
            ptrComputerName.Dispose();
            ptrAuthenticator.Dispose();

            return (NtStatus)retVal;
        }

        /// <summary>
        ///  The NetrLogonSendToSam is supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, and windows_server_2008windows_server_2008,
        ///  windows_7, and windows_server_7. The method allows a BDC
        ///  to forward user account password changes to the PDC.
        ///  It is used by the client to deliver an opaque buffer
        ///  to the SAM database ([MS-SAMR]) on the server side.
        ///  Opnum: 32 
        /// </summary>
        /// <param name="primaryName">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="computerName">
        ///  A null-terminated Unicode string that contains the NetBIOS
        ///  name of the client computer making the call.
        /// </param>
        /// <param name="authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in MS-NRPC, that contains the client authenticator.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="opaqueBuffer">
        ///  A buffer to be passed to the Security Account Manager
        ///  (SAM) service on the PDC. The buffer is encrypted on
        ///  the wire.
        /// </param>
        /// <param name="opaqueBufferSize">
        ///  The size, in bytes, of the opaqueBuffer parameter.
        /// </param>
        /// <returns>
        /// the NtStatus.
        /// </returns>
        public NtStatus NetrLogonSendToSam(
            string primaryName,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            out _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            byte[] opaqueBuffer,
            uint opaqueBufferSize)
        {
            const ushort Opnum = 32;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrPrimaryName = Marshal.StringToHGlobalUni(primaryName);
            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrAuthenticator = TypeMarshal.ToIntPtr(authenticator);
            SafeIntPtr ptrOpaqueBuffer = IntPtrUtility.ArrayToPtr(opaqueBuffer);

            paramList = new Int3264[] 
            {
                ptrPrimaryName,
                ptrComputerName,
                ptrAuthenticator,
                IntPtr.Zero,
                ptrOpaqueBuffer,
                opaqueBufferSize,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
         RpceStubHelper.GetPlatform(),
            NrpcRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrReturnAuthenticator = outParamList[3];
                returnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(ptrReturnAuthenticator);

                retVal = outParamList[6].ToInt32();
            }

            ptrPrimaryName.Dispose();
            ptrComputerName.Dispose();
            ptrAuthenticator.Dispose();
            ptrOpaqueBuffer.Dispose();

            return (NtStatus)retVal;
        }

        /// <summary>
        ///  The DsrAddressToSiteNamesW method is supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista,  windows_server_2008,
        ///  windows_7, and windows_server_7. It translates a list
        ///  of socket addresses into their corresponding site names.
        ///  For information about the mapping from socket address
        ///  to subnet/site name, see [MS-ADTS].
        ///  Opnum: 33 
        /// </summary>
        /// <param name="computerName">
        ///  The custom RPC binding handle (section) that represents
        ///  the connection to a domain controller.
        /// </param>
        /// <param name="entryCount">
        ///  The number of socket addresses specified in socketAddresses.
        ///  The maximum value for entryCount is 32000.To avoid
        ///  large memory allocations, the number of 32,000 was
        ///  chosen as a reasonable limit for the maximum number
        ///  of socket addresses that this method accepts.
        /// </param>
        /// <param name="socketAddresses">
        ///  An array of NL_SOCKET_ADDRESS structures
        ///  that contains socket addresses to translate. The number
        ///  of addresses specified MUST be equal to entryCount.
        /// </param>
        /// <param name="siteNames">
        ///  A pointer to an NL_SITE_NAME_ARRAY structure
        ///  that contains a corresponding array of site names.
        ///  The number of entries returned is equal to entryCount.
        ///  An entry is returned as NULL if the corresponding socket
        ///  address does not map to any site, or if the address
        ///  family of the socket address is not IPV4 or IPV6. The
        ///  mapping of IP addresses to sites is specified in [MS-ADTS]
        ///  section.
        /// </param>
        /// <returns>
        /// Net API status.
        /// </returns>
        public NetApiStatus DsrAddressToSiteNamesW(
            string computerName,
            uint entryCount,
            _NL_SOCKET_ADDRESS[] socketAddresses,
            out _NL_SITE_NAME_ARRAY? siteNames)
        {
            const ushort Opnum = 33;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrSocketAddresses = TypeMarshal.ToIntPtr(socketAddresses);

            paramList = new Int3264[] 
            {
                ptrComputerName,
                entryCount,
                Marshal.ReadIntPtr(ptrSocketAddresses),
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrSiteNames = outParamList[3];
                ptrSiteNames = Marshal.ReadIntPtr(ptrSiteNames);
                siteNames = TypeMarshal.ToNullableStruct<_NL_SITE_NAME_ARRAY>(ptrSiteNames);

                retVal = outParamList[4].ToInt32();
            }

            ptrComputerName.Dispose();
            ptrSocketAddresses.Dispose();

            return (NetApiStatus)retVal;
        }

        /// <summary>
        ///  The DsrGetDcNameEx2 method returns information about
        ///  a domain controller in the specified domain and site.Supported
        ///  in windows_2000_server, windows_xp, windows_server_2003,
        ///  windows_vista, and windows_server_2008, windows_7,
        ///  and windows_server_7. The method will also verify that
        ///  the responding DCdatabase contains an account if accountName
        ///  is specified. The server that receives this call is
        ///  not required to be a DC. Opnum: 34 
        /// </summary>
        /// <param name="computerName">
        ///  The custom RPC binding handle.
        /// </param>
        /// <param name="accountName">
        ///  A null-terminated Unicode string that contains the name
        ///  of the account that MUST exist and be enabled on the
        ///  DC.
        /// </param>
        /// <param name="allowableAccountControlBits">
        ///  A set of bit flags that list properties of the accountName
        ///  account. A flag is TRUE (or set) if its value is equal
        ///  to 1. If the flag is set, then the account MUST have
        ///  that property; otherwise, the property is ignored.
        ///  The value is constructed from zero or more bit flags
        ///  from the following table.
        /// </param>
        /// <param name="domainName">
        ///  A null-terminated Unicode string that contains the domain
        ///  name (3).
        /// </param>
        /// <param name="domainGuid">
        ///  A pointer to a GUID structure that specifies the GUID
        ///  of the domain queried. If domainGuid is not NULL and
        ///  the domain specified by domainName cannot be found,
        ///  the DC locator attempts to locate a DC in the domain
        ///  that has the GUID specified by domainGuid. This allows
        ///  renamed domains to be found by their GUID.
        /// </param>
        /// <param name="siteName">
        ///  A null-terminated string that contains the name of the
        ///  site in which the DC MUST be located.
        /// </param>
        /// <param name="flags">
        ///  A set of bit flags that provide additional data that
        ///  is used to process the request. A flag is TRUE (or
        ///  set) if its value is equal to 1. The value is constructed
        ///  from zero or more bit flags from the following table,
        ///  with the exceptions that bits D, E, and H cannot be
        ///  combined; S and R cannot be combined; and N and O cannot
        ///  be combined.
        /// </param>
        /// <param name="domainControllerInfo">
        ///  A pointer to a DOMAIN_CONTROLLER_INFOW structure
        ///  containing data about the DC.
        /// </param>
        /// <returns>
        ///  Net API status.
        /// </returns>
        public NetApiStatus DsrGetDcNameEx2(
            string computerName,
            string accountName,
            uint allowableAccountControlBits,
            string domainName,
            Guid? domainGuid,
            string siteName,
            uint flags,
            out _DOMAIN_CONTROLLER_INFOW? domainControllerInfo)
        {
            const ushort Opnum = 34;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrAccountName = Marshal.StringToHGlobalUni(accountName);
            SafeIntPtr ptrDomainName = Marshal.StringToHGlobalUni(domainName);
            SafeIntPtr ptrDomainGuid = TypeMarshal.ToIntPtr(domainGuid);
            SafeIntPtr ptrSiteName = Marshal.StringToHGlobalUni(siteName);

            paramList = new Int3264[] 
            {
                ptrComputerName,
                ptrAccountName,
                allowableAccountControlBits,
                ptrDomainName,
                ptrDomainGuid,
                ptrSiteName,
                flags,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrDomainControllerInfo = outParamList[7];
                ptrDomainControllerInfo = Marshal.ReadIntPtr(ptrDomainControllerInfo);
                domainControllerInfo = TypeMarshal.ToNullableStruct<_DOMAIN_CONTROLLER_INFOW>(ptrDomainControllerInfo);

                retVal = outParamList[8].ToInt32();
            }

            ptrComputerName.Dispose();
            ptrAccountName.Dispose();
            ptrDomainName.Dispose();
            ptrDomainGuid.Dispose();
            ptrSiteName.Dispose();

            return (NetApiStatus)retVal;
        }

        /// <summary>
        ///  The NetrLogonGetTimeServiceParentDomain method is supported
        ///  in windows_2000_server, windows_xp and windows_server_2003.
        ///  It returns the name of the parent domain of the current
        ///  domain. The domain name returned by this method is
        ///  suitable for passing into the NetrLogonGetTrustRid
        ///  method and NetrLogonComputeClientDigest method. 
        ///  Opnum: 35
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle, as specified in section
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
        ///  is in the same site as the server specified by serverName.
        ///  The Netlogon client ignores this value
        ///  if serverName is not a domain controller.  
        /// </param>
        /// <returns>
        ///  Net API status.
        /// </returns>
        public NetApiStatus NetrLogonGetTimeServiceParentDomain(
            string serverName,
            out string domainName,
            out PdcSameSite_Values? pdcSameSite)
        {
            const ushort Opnum = 35;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrServerName = Marshal.StringToHGlobalUni(serverName);

            paramList = new Int3264[] 
            {
                ptrServerName,
                IntPtr.Zero,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrDomainName = outParamList[1];

                // wchar_t** DomainName
                ptrDomainName = Marshal.ReadIntPtr(ptrDomainName);
                domainName = Marshal.PtrToStringUni(ptrDomainName);

                pdcSameSite = TypeMarshal.ToNullableStruct<PdcSameSite_Values>(outParamList[2]);

                retVal = outParamList[3].ToInt32();
            }

            ptrServerName.Dispose();

            return (NetApiStatus)retVal;
        }

        /// <summary>
        ///  The NetrEnumerateTrustedDomainsEx method is supported in
        ///  windows_2000_server, windows_xp, windows_server_2003,
        ///  windows_vista, windows_server_2008, windows_7, and
        ///  windows_server_7. It returns a list of trusted domains
        ///  from a specified server. This method extends NetrEnumerateTrustedDomains
        ///  by returning an array of domains in a more flexible
        ///  DS_DOMAIN_TRUSTSW structure, as specified in MS-NRPC
        ///  , rather than the array of strings in DOMAIN_NAME_BUFFER
        ///  structure, as specified in section. The array is returned
        ///  as part of the NETLOGON_TRUSTED_DOMAIN_ARRAY structure,
        ///  as specified in MS-NRPC. Opnum: 36 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle, as specified in section.
        /// </param>
        /// <param name="domains">
        ///  A pointer to a NETLOGON_TRUSTED_DOMAIN_ARRAY structure,
        ///  as specified in section , that contains an array of
        ///  DS_DOMAIN_TRUSTSW structures, as specified in section
        ///  , one for each trusted domain.
        /// </param>
        /// <returns>
        /// Net API status.
        /// </returns>
        public NetApiStatus NetrEnumerateTrustedDomainsEx(
            string serverName,
            out _NETLOGON_TRUSTED_DOMAIN_ARRAY? domains)
        {
            const ushort Opnum = 36;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrServerName = Marshal.StringToHGlobalUni(serverName);

            paramList = new Int3264[] 
            {
                ptrServerName,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrDomains = outParamList[1];
                domains = TypeMarshal.ToNullableStruct<_NETLOGON_TRUSTED_DOMAIN_ARRAY>(ptrDomains);

                retVal = outParamList[2].ToInt32();
            }

            ptrServerName.Dispose();

            return (NetApiStatus)retVal;
        }

        /// <summary>
        ///  The DsrAddressToSiteNamesExW method is supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///windows_7, windows_server_7. It translates a list of socket
        ///  addresses into their corresponding site names and subnet
        ///  names. For information about the mapping from socket
        ///  address to subnet/site name, see [MS-ADTS].
        ///   Opnum: 37 
        /// </summary>
        /// <param name="computerName">
        ///  The custom RPC binding handle that represents
        ///  the connection to a domain controller.
        /// </param>
        /// <param name="entryCount">
        ///  The number of socket addresses specified in socketAddresses.
        ///  The maximum value for entryCount is 32000.To avoid
        ///  large memory allocations, the number of 32,000 was
        ///  chosen as a reasonable limit for the maximum number
        ///  of socket addresses that this method accepts.
        /// </param>
        /// <param name="socketAddresses">
        ///  An array of NL_SOCKET_ADDRESS structures
        ///  that contains socket addresses to translate. The number
        ///  of addresses specified MUST be equal to entryCount.
        /// </param>
        /// <param name="siteNames">
        ///  A pointer to an NL_SITE_NAME_EX_ARRAY structure
        ///  that contains an array of site names and an array
        ///  of subnet names that correspond to socket addresses
        ///  in socketAddresses. The number of entries returned
        ///  is equal to entryCount. An entry is returned as NULL
        ///  if the corresponding socket address does not map to
        ///  any site, or if the address family of the socket address
        ///  is not IPV4 or IPV6. The mapping of IP addresses to
        ///  sites is specified in [MS-ADTS].
        /// </param>
        /// <returns>
        /// Net API status.
        /// </returns>
        public NetApiStatus DsrAddressToSiteNamesExW(
            string computerName,
            uint entryCount,
            _NL_SOCKET_ADDRESS[] socketAddresses,
            out _NL_SITE_NAME_EX_ARRAY? siteNames)
        {
            const ushort Opnum = 37;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrSocketAddresses = TypeMarshal.ToIntPtr(socketAddresses);

            paramList = new Int3264[] 
            {
                ptrComputerName,
                entryCount,
                Marshal.ReadIntPtr(ptrSocketAddresses),
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrSiteNames;
                ptrSiteNames = (IntPtr)outParamList[3];
                ptrSiteNames = Marshal.ReadIntPtr(ptrSiteNames);
                siteNames = TypeMarshal.ToNullableStruct<_NL_SITE_NAME_EX_ARRAY>(ptrSiteNames);

                retVal = outParamList[4].ToInt32();
            }

            ptrComputerName.Dispose();
            ptrSocketAddresses.Dispose();

            return (NetApiStatus)retVal;
        }

        /// <summary>
        ///  The DsrGetDcSiteCoverageW method is supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. It returns a list of
        ///  sites covered by a domain controller. Site coverage
        ///  is detailed in [MS-ADTS]. Opnum: 38 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle that represents
        ///  the connection to a DC.
        /// </param>
        /// <param name="siteNames">
        ///  A pointer to an NL_SITE_NAME_ARRAY structure
        ///  that contains an array of site name strings.
        /// </param>
        /// <returns>
        /// Net API status.
        /// </returns>
        public NetApiStatus DsrGetDcSiteCoverageW(
            string serverName,
            out _NL_SITE_NAME_ARRAY? siteNames)
        {
            const ushort Opnum = 38;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrServerName = Marshal.StringToHGlobalUni(serverName);

            paramList = new Int3264[] 
            {
                ptrServerName,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrSiteNames = outParamList[1];
                ptrSiteNames = Marshal.ReadIntPtr(ptrSiteNames);
                siteNames = TypeMarshal.ToNullableStruct<_NL_SITE_NAME_ARRAY>(ptrSiteNames);

                retVal = outParamList[2].ToInt32();
            }

            ptrServerName.Dispose();

            return (NetApiStatus)retVal;
        }

        /// <summary>
        ///  The NetrLogonSamLogonEx method is supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. It provides an extension
        ///  to NetrLogonSamLogon that accepts an extra flags parameter
        ///  and uses Secure RPC ([MS-RPCE] section) instead of
        ///  Netlogon authenticators. This method handles logon
        ///  requests for the SAM accounts and allows for generic
        ///  pass-through authentication, as specified in section.
        ///   For more information about fields and structures
        ///  used by Netlogon pass-through methods, see section
        ///  Opnum: 39 
        /// </summary>
        /// <param name="contextHandle">
        ///  A primitive RPC handle that identifies a particular
        ///  client/server binding, as specified in section.
        /// </param>
        /// <param name="logonServer">
        ///  The null-terminated Unicode string that contains the
        ///  NetBIOS name of the server that will handle the logon
        ///  request.
        /// </param>
        /// <param name="computerName">
        ///  The null-terminated Unicode string that contains the
        ///  NetBIOS name of the client computer sending the logon
        ///  request.
        /// </param>
        /// <param name="logonLevel">
        ///  A NETLOGON_LOGON_INFO_CLASS structure, as specified
        ///  in section , that specifies the type of the logon information
        ///  passed in the logonInformation parameter.
        /// </param>
        /// <param name="logonInformation">
        ///  A pointer to a NETLOGON_LEVEL structure, as specified
        ///  in MS-NRPC, that describes the logon request information.
        /// </param>
        /// <param name="validationLevel">
        ///  A NETLOGON_VALIDATION_INFO_CLASS enumerated type, as
        ///  specified in MS-NRPC, that contains the validation
        ///  level requested by the client.
        /// </param>
        /// <param name="validationInformation">
        ///  A pointer to a NETLOGON_VALIDATION structure, as specified
        ///  in section , that describes the user validation information
        ///  returned to the client. The type of the NETLOGON_VALIDATION
        ///  used is determined by the value of the validationLevel
        ///  parameter.
        /// </param>
        /// <param name="authoritative">
        /// ///  A pointer to a char value representing a Boolean condition.
        ///  FALSE is indicated by the value 0x00; TRUE SHOULD be
        ///  indicated by the value 0x01 and MAY also be indicated
        ///  by any nonzero value. Windows uses the value of 0x01
        ///  as the representation of TRUE and 0x00 for FALSE. This
        ///  Boolean value indicates whether the validation information
        ///  is final. This field is necessary because the request
        ///  might be forwarded through multiple servers. A value
        ///  of TRUE indicates that the validation information is
        ///  final and MUST remain unchanged. The authoritative
        ///  parameter indicates whether the response to this call
        ///  is final or if the same request can be sent to another
        ///  server. The value SHOULD be set to FALSE if the server
        ///  encounters a transient error, and the client can resend
        ///  the request to another server. If the same request
        ///  is known to fail in all subsequent requests, the server
        ///  SHOULD return TRUE.
        /// </param>
        /// <param name="extraFlags">
        ///  A pointer to a set of bit flags that specify delivery
        ///  settings. A flag is TRUE (or set) if its value is equal
        ///  to 1. Output flags MUST be the same as input. The value
        ///  is constructed from zero or more bit flags from the
        ///  following table.
        /// </param>
        /// <returns>
        /// the NtStatus.
        /// </returns>
        public NtStatus NetrLogonSamLogonEx(
            IntPtr contextHandle,
            string logonServer,
            string computerName,
            _NETLOGON_LOGON_INFO_CLASS logonLevel,
            _NETLOGON_LEVEL? logonInformation,
            _NETLOGON_VALIDATION_INFO_CLASS validationLevel,
            out _NETLOGON_VALIDATION? validationInformation,
            out byte? authoritative,
            ref uint? extraFlags)
        {
            const ushort Opnum = 39;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrLogonServer = Marshal.StringToHGlobalUni(logonServer);
            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrLogonInformation = TypeMarshal.ToIntPtr(logonInformation, logonLevel, null, null);
            SafeIntPtr ptrExtraFlags = TypeMarshal.ToIntPtr(extraFlags);

            paramList = new Int3264[] 
            {
                ptrLogonServer,
                ptrComputerName,
                (uint)logonLevel,
                ptrLogonInformation,
                (uint)validationLevel,
                IntPtr.Zero,
                IntPtr.Zero,
                ptrExtraFlags,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrValidationInformation = outParamList[5];
                validationInformation = TypeMarshal.ToNullableStruct<_NETLOGON_VALIDATION>(
                    ptrValidationInformation,
                    validationLevel,
                    null,
                    null);

                authoritative = TypeMarshal.ToNullableStruct<byte>(outParamList[6]);

                extraFlags = TypeMarshal.ToNullableStruct<uint>(outParamList[7]);

                retVal = outParamList[8].ToInt32();
            }

            ptrLogonServer.Dispose();
            ptrComputerName.Dispose();
            ptrLogonInformation.Dispose();
            ptrExtraFlags.Dispose();

            return (NtStatus)retVal;
        }

        /// <summary>
        ///  The DsrEnumerateDomainTrusts method is supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. It returns an enumerated
        ///  list of domaintrusts, filtered by a set of flags, from
        ///  the specified server. Opnum: 40 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle, as specified in section.
        /// </param>
        /// <param name="flags">
        ///  A set of bit flags that specify properties that MUST
        ///  be true for a domain trust to be part of the returned
        ///  domain name list. A flag is TRUE (or set) if its value
        ///  is equal to 1. flags MUST contain one or more of the
        ///  following bits.
        /// </param>
        /// <param name="domains">
        ///  A pointer to a NETLOGON_TRUSTED_DOMAIN_ARRAY structure,
        ///  as specified in MS-NRPC, that contains a list of trusted
        ///  domains.
        /// </param>
        /// <returns>
        /// Net API status.
        /// </returns>
        public NetApiStatus DsrEnumerateDomainTrusts(
            string serverName,
            uint flags,
            out _NETLOGON_TRUSTED_DOMAIN_ARRAY? domains)
        {
            const ushort Opnum = 40;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrServerName = Marshal.StringToHGlobalUni(serverName);

            paramList = new Int3264[] 
            {
                ptrServerName,
                flags,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrDomains = outParamList[2];
                domains = TypeMarshal.ToNullableStruct<_NETLOGON_TRUSTED_DOMAIN_ARRAY>(ptrDomains);

                retVal = outParamList[3].ToInt32();
            }

            ptrServerName.Dispose();

            return (NetApiStatus)retVal;
        }

        /// <summary>
        ///  The DsrDeregisterDnsHostRecords method is supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, windows_server_2008,
        ///windows_7, and windows_server_7. It deletes all of the
        ///  DNS SRV records registered by a specified domain controller.
        ///  For the list of SRV records that a domain registers,
        ///  see [MS-ADTS] section , SRV Records Registered by DC.
        ///  Opnum: 41 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle, as specified in section
        ///  , that represents the connection to the DC.
        /// </param>
        /// <param name="dnsDomainName">
        ///  A null-terminated Unicode string that specifies the
        ///  fully qualified domain name (FQDN) (2).
        /// </param>
        /// <param name="domainGuid">
        ///  A pointer to the domainGUID. If the value is not NULL,
        ///  the DNS SRV record of type _ldap._tcp.domainGuid.domains._msdcs.DnsDomainName
        ///  is also deregistered.
        /// </param>
        /// <param name="dsaGuid">
        ///  A pointer to the objectGUID of the DC's TDSDSA object.
        ///  For information about the TDSDSA object, see [MS-ADTS]
        ///  section. If the value is not NULL, the CNAME [RFC1035]
        ///  record of the domain in the form of DsaGuid._msdcs.DnsDomainName
        ///  is also deregistered.
        /// </param>
        /// <param name="dnsHostName">
        ///  A null-terminated Unicode string that specifies the
        ///  fully qualified domain name (FQDN) (1) of the DC whose
        ///  records are being deregistered. If the value is NULL,
        ///  ERROR_INVALID_PARAMETER is returned.
        /// </param>
        /// <returns>
        /// Net API status.
        /// </returns>
        public NetApiStatus DsrDeregisterDnsHostRecords(
            string serverName,
            string dnsDomainName,
            Guid? domainGuid,
            Guid? dsaGuid,
            string dnsHostName)
        {
            const ushort Opnum = 41;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrServerName = Marshal.StringToHGlobalUni(serverName);
            SafeIntPtr ptrDnsDomainName = Marshal.StringToHGlobalUni(dnsDomainName);
            SafeIntPtr ptrDnsHostName = Marshal.StringToHGlobalUni(dnsHostName);
            SafeIntPtr ptrDomainGuid = TypeMarshal.ToIntPtr(domainGuid);
            SafeIntPtr ptrDsaGuid = TypeMarshal.ToIntPtr(dsaGuid);

            paramList = new Int3264[] 
            {
                ptrServerName,
                ptrDnsDomainName,
                ptrDomainGuid,
                ptrDsaGuid,
                ptrDnsHostName,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                retVal = outParamList[5].ToInt32();
            }

            ptrServerName.Dispose();
            ptrDnsDomainName.Dispose();
            ptrDnsHostName.Dispose();
            ptrDomainGuid.Dispose();
            ptrDsaGuid.Dispose();

            return (NetApiStatus)retVal;
        }

        /// <summary>
        ///  The NetrServerTrustPasswordsGet method is supported in windows_2000_server_sp4,
        ///  windows_xp, and windows_server_2003, windows_vista,
        ///  windows_server_2008, windows_7, and windows_server_7.
        ///It  returns the encrypted current and previous passwords
        ///  for an account in the domain. This method is called
        ///  by a client to retrieve the current and previous account
        ///  passwords from a domain controller. The account name
        ///  requested MUST be the name used when the secure channel
        ///  was created, unless the method is called on a PDC by
        ///  a DC, in which case it can be any valid account name.
        ///  Opnum: 42 
        /// </summary>
        /// <param name="trustedDcName">
        ///  The custom RPC binding handle, as specified in section
        /// </param>
        /// <param name="accountName">
        ///  The null-terminated Unicode string that contains the
        ///  name of the client account in the domain for which
        ///  the trust password MUST be returned. In windows, all
        ///  machine account names are the name of the machine with
        ///  a $ (dollar sign) appended.
        /// </param>
        /// <param name="secureChannelType">
        ///  A NETLOGON_SECURE_CHANNEL_TYPE enumerated value, as
        ///  specified in section , that indicates the type of the
        ///  secure channel being established by this call.
        /// </param>
        /// <param name="computerName">
        ///  The null-terminated Unicode string that contains the
        ///  NetBIOS name of the client computer.
        /// </param>
        /// <param name="authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the client authenticator.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="encryptedNewOwfPassword">
        ///  A pointer to an ENCRYPTED_NT_OWF_PASSWORD structure,
        ///  as specified in section , that contains the NTOWFv1
        ///  (as specified in NTLM v1 Authentication in [MS-NLMP]
        ///  section) of the current password, encrypted as specified
        ///  in [MS-SAMR], Encrypting an NT Hash or LM
        ///  Hash Value with a Specified Key. The session key is
        ///  the specified 16-byte key that is used to derive the
        ///  password's keys. The specified 16-byte key uses the
        ///  16-byte value process, as specified in [MS-SAMR] section
        /// </param>
        /// <param name="encryptedOldOwfPassword">
        ///  A pointer to an ENCRYPTED_NT_OWF_PASSWORD structure,
        ///  as specified in section , that contains the NTOWFv1
        ///  (as specified in NTLM v1 Authentication in [MS-NLMP]
        ///  section) of the previous password, encrypted as specified
        ///  in [MS-SAMR] section , Encrypting an NT Hash or LM
        ///  Hash Value with a Specified Key. The session key is
        ///  the specified 16-byte key that is used to derive the
        ///  password's keys. The specified 16-byte key uses the
        ///  16-byte value process, as specified in [MS-SAMR]
        /// </param>
        /// <returns>
        /// the NtStatus.
        /// </returns>
        public NtStatus NetrServerTrustPasswordsGet(
            string trustedDcName,
            string accountName,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            out _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            out _NT_OWF_PASSWORD? encryptedNewOwfPassword,
            out _NT_OWF_PASSWORD? encryptedOldOwfPassword)
        {
            const ushort Opnum = 42;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrTrustedDcName = Marshal.StringToHGlobalUni(trustedDcName);
            SafeIntPtr ptrAccountName = Marshal.StringToHGlobalUni(accountName);
            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrAuthenticator = TypeMarshal.ToIntPtr(authenticator);

            paramList = new Int3264[] 
            {
                ptrTrustedDcName,
                ptrAccountName,
                (uint)secureChannelType,
                ptrComputerName,
                ptrAuthenticator,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrReturnAuthenticator = outParamList[5];
                returnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(ptrReturnAuthenticator);

                IntPtr ptrEncryptedNewOwfPassword = outParamList[6];
                encryptedNewOwfPassword = TypeMarshal.ToNullableStruct<_NT_OWF_PASSWORD>(ptrEncryptedNewOwfPassword);

                IntPtr ptrEncryptedOldOwfPassword = outParamList[7];
                encryptedOldOwfPassword = TypeMarshal.ToNullableStruct<_NT_OWF_PASSWORD>(ptrEncryptedOldOwfPassword);

                retVal = outParamList[8].ToInt32();
            }

            ptrTrustedDcName.Dispose();
            ptrAccountName.Dispose();
            ptrComputerName.Dispose();
            ptrAuthenticator.Dispose();

            return (NtStatus)retVal;
        }

        /// <summary>
        ///  The DsrGetForestTrustInformation method is supported in
        ///  windows_xpwindows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. It retrieves the trust
        ///  information for the forest of the specified domain
        ///  controller, or for a forest trusted by the forest of
        ///  the specified DC. Opnum: 43 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle, as specified in section
        /// </param>
        /// <param name="trustedDomainName">
        ///  The optional null-terminated Unicode string that contains
        ///  the DNS or NetBIOS name of the trusted domain for which
        ///  the foresttrust information is to be gathered.
        /// </param>
        /// <param name="flags">
        ///  A set of bit flags that specify additional applications
        ///  for the foresttrust information. A flag is TRUE (or
        ///  set) if its value is equal to 1.
        /// </param>
        /// <param name="forestTrustInfo">
        ///  A pointer to an LSA_FOREST_TRUST_INFORMATION structure,
        ///  as specified in [MS-LSAD], that contains data
        ///  for each foresttrust.
        /// </param>
        /// <returns>
        /// Net API status
        /// </returns>
        public NetApiStatus DsrGetForestTrustInformation(
            string serverName,
            string trustedDomainName,
            uint flags,
            out _LSA_FOREST_TRUST_INFORMATION? forestTrustInfo)
        {
            const ushort Opnum = 43;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrServerName = Marshal.StringToHGlobalUni(serverName);
            SafeIntPtr ptrTrustedDomainName = Marshal.StringToHGlobalUni(trustedDomainName);

            paramList = new Int3264[] 
            {
                ptrServerName,
                ptrTrustedDomainName,
                flags,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrForestTrustInfo = outParamList[3];
                ptrForestTrustInfo = Marshal.ReadIntPtr(ptrForestTrustInfo);
                forestTrustInfo = TypeMarshal.ToNullableStruct<_LSA_FOREST_TRUST_INFORMATION>(ptrForestTrustInfo);

                retVal = outParamList[4].ToInt32();
            }

            ptrServerName.Dispose();
            ptrTrustedDomainName.Dispose();

            return (NetApiStatus)retVal;
        }

        /// <summary>
        ///  The NetrGetForestTrustInformation is supported in windows_2000_server,
        ///  windows_xp, windows_server_2003, windows_vista, and
        ///  windows_server_2008, windows_7, and windows_server_7.
        ///  The method retrieves the trust information for the forest
        ///  of which the member's domain is itself a member. Opnum
        ///  : 44 
        /// </summary>
        /// <param name="serverName">
        ///  The custom RPC binding handle, as specified in section.
        /// </param>
        /// <param name="computerName">
        ///  The null-terminated Unicode string that contains the
        ///  client computer NetBIOS name.
        /// </param>
        /// <param name="authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the client authenticator.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="flags">
        ///  MUST be set to zero and MUST be ignored on receipt.
        /// </param>
        /// <param name="forestTrustInfo">
        ///  A pointer to an LSA_FOREST_TRUST_INFORMATION structure,
        ///  as specified in [MS-LSAD] section , that contains data
        ///  for each foresttrust.
        /// </param>
        /// <returns>
        /// the NtStatus.
        /// </returns>
        public NtStatus NetrGetForestTrustInformation(
            string serverName,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            out _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            uint flags,
            out _LSA_FOREST_TRUST_INFORMATION? forestTrustInfo)
        {
            const ushort Opnum = 44;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrServerName = Marshal.StringToHGlobalUni(serverName);
            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrAuthenticator = TypeMarshal.ToIntPtr(authenticator);

            paramList = new Int3264[] 
            {
                ptrServerName,
                ptrComputerName,
                ptrAuthenticator,
                IntPtr.Zero,
                flags,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrReturnAuthenticator = outParamList[3];
                returnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(ptrReturnAuthenticator);

                IntPtr ptrForestTrustInfo = outParamList[5];
                ptrForestTrustInfo = Marshal.ReadIntPtr(ptrForestTrustInfo);
                forestTrustInfo = TypeMarshal.ToNullableStruct<_LSA_FOREST_TRUST_INFORMATION>(ptrForestTrustInfo);

                retVal = outParamList[6].ToInt32();
            }

            ptrServerName.Dispose();
            ptrComputerName.Dispose();
            ptrAuthenticator.Dispose();

            return (NtStatus)retVal;
        }

        /// <summary>
        ///  The NetrLogonSamLogonWithFlags method is supported in windows_xpwindows_server_2003,
        ///  windows_vista, windows_server_2008, windows_7, and
        ///  windows_server_7. It handles logon requests for the SAM
        ///  accounts. Opnum: 45 
        /// </summary>
        /// <param name="logonServer">
        ///  The custom RPC binding handle, as specified in section.
        /// </param>
        /// <param name="computerName">
        ///  The Unicode string that contains the NetBIOS name of
        ///  the client computer calling this method.
        /// </param>
        /// <param name="authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the client authenticator.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="logonLevel">
        ///  A NETLOGON_LOGON_INFO_CLASS structure, as specified
        ///  in section , that specifies the type of logon information
        ///  passed in the logonInformation parameter.
        /// </param>
        /// <param name="logonInformation">
        ///  A pointer to a NETLOGON_LEVEL structure, as specified
        ///  in section , that describes the logon request information.
        /// </param>
        /// <param name="validationLevel">
        ///  A NETLOGON_VALIDATION_INFO_CLASS enumerated type, as
        ///  specified in section , that contains the validation
        ///  level requested by the client.
        /// </param>
        /// <param name="validationInformation">
        ///  A pointer to a NETLOGON_VALIDATION structure, as specified
        ///  in section , that describes the user validation information
        ///  returned to the client. The type of the NETLOGON_VALIDATION
        ///  used is determined by the value of the validationLevel
        ///  parameter.
        /// </param>
        /// <param name="authoritative">
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
        /// <param name="extraFlags">
        ///  A pointer to a set of bit flags that specify delivery
        ///  settings. A flag is TRUE (or set) if its value is equal
        ///  to 1. The value is constructed from zero or more bit
        ///  flags from the following table.
        /// </param>
        /// <returns>
        /// the NtStatus.
        /// </returns>
        public NtStatus NetrLogonSamLogonWithFlags(
            string logonServer,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            ref _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            _NETLOGON_LOGON_INFO_CLASS logonLevel,
            _NETLOGON_LEVEL? logonInformation,
            _NETLOGON_VALIDATION_INFO_CLASS validationLevel,
            out _NETLOGON_VALIDATION? validationInformation,
            out byte? authoritative,
            ref uint? extraFlags)
        {
            const ushort Opnum = 45;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrLogonServer = Marshal.StringToHGlobalUni(logonServer);
            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrAuthenticator = TypeMarshal.ToIntPtr(authenticator);
            SafeIntPtr ptrReturnAuthenticatorIn = TypeMarshal.ToIntPtr(returnAuthenticator);
            SafeIntPtr ptrLogonInformation = TypeMarshal.ToIntPtr(logonInformation, logonLevel, null, null);
            SafeIntPtr ptrExtraFlags = TypeMarshal.ToIntPtr(extraFlags);

            paramList = new Int3264[] 
            {
                ptrLogonServer,
                ptrComputerName,
                ptrAuthenticator,
                ptrReturnAuthenticatorIn,
                (uint)logonLevel,
                ptrLogonInformation,
                (uint)validationLevel,
                IntPtr.Zero,
                IntPtr.Zero,
                ptrExtraFlags,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrReturnAuthenticatorOut = outParamList[3];
                returnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(ptrReturnAuthenticatorOut);

                IntPtr ptrValidationInformation = outParamList[7];
                validationInformation = TypeMarshal.ToNullableStruct<_NETLOGON_VALIDATION>(
                    ptrValidationInformation,
                    validationLevel,
                    null,
                    null);

                authoritative = TypeMarshal.ToNullableStruct<byte>(outParamList[8]);

                extraFlags = TypeMarshal.ToNullableStruct<uint>(outParamList[9]);

                retVal = outParamList[10].ToInt32();
            }

            ptrLogonServer.Dispose();
            ptrComputerName.Dispose();
            ptrAuthenticator.Dispose();
            ptrReturnAuthenticatorIn.Dispose();
            ptrLogonInformation.Dispose();
            ptrExtraFlags.Dispose();

            return (NtStatus)retVal;
        }

        /// <summary>
        ///The NetrServerGetTrustInfo method is supported in windows_xp
        ///  and windows_server_2003, windows_vista, windows_server_2008,
        ///windows_7, windows_server_7. It returns an information
        ///  block from a specified server. The information includes
        ///  encrypted current and previous passwords for a particular
        ///  account and additional trust data. The account name
        ///  requested MUST be the name used when the secure channel
        ///  was created, unless the method is called on a PDC by
        ///  a domain controller, in which case it can be any valid
        ///  account name. Opnum: 46 
        /// </summary>
        /// <param name="trustedDcName">
        ///  The custom RPC binding handle, as specified in section.
        /// </param>
        /// <param name="accountName">
        ///  The null-terminated Unicode string that contains the
        ///  name of the client account in the domain.
        /// </param>
        /// <param name="secureChannelType">
        ///  A NETLOGON_SECURE_CHANNEL_TYPE enumerated value, as
        ///  specified in section , that indicates the type of the
        ///  secure channel being established by this call.
        /// </param>
        /// <param name="computerName">
        ///  The null-terminated Unicode string that contains the
        ///  NetBIOS name of the client computer, for which the
        ///  trust information MUST be returned.
        /// </param>
        /// <param name="authenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the client authenticator.
        /// </param>
        /// <param name="returnAuthenticator">
        ///  A pointer to a NETLOGON_AUTHENTICATOR structure, as
        ///  specified in section , that contains the server return
        ///  authenticator.
        /// </param>
        /// <param name="encryptedNewOwfPassword">
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
        /// <param name="encryptedOldOwfPassword">
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
        /// <param name="trustInfo">
        ///  A pointer to an NL_GENERIC_RPC_DATA structure, as specified
        ///  in section , that contains a block of generic RPC data
        ///  with trust information for the specified server.
        /// </param>
        /// <returns>
        /// the NtStatus.
        /// </returns>
        public NtStatus NetrServerGetTrustInfo(
            string trustedDcName,
            string accountName,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            out _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            out _NT_OWF_PASSWORD? encryptedNewOwfPassword,
            out _NT_OWF_PASSWORD? encryptedOldOwfPassword,
            out _NL_GENERIC_RPC_DATA? trustInfo)
        {
            const ushort Opnum = 46;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrTrustedDcName = Marshal.StringToHGlobalUni(trustedDcName);
            SafeIntPtr ptrAccountName = Marshal.StringToHGlobalUni(accountName);
            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrAuthenticator = TypeMarshal.ToIntPtr(authenticator);

            paramList = new Int3264[] 
            {
                ptrTrustedDcName,
                ptrAccountName,
                (uint)secureChannelType,
                ptrComputerName,
                ptrAuthenticator,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrReturnAuthenticator = outParamList[5];
                returnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(ptrReturnAuthenticator);

                IntPtr ptrEncryptedNewOwfPassword = outParamList[6];
                encryptedNewOwfPassword = TypeMarshal.ToNullableStruct<_NT_OWF_PASSWORD>(ptrEncryptedNewOwfPassword);

                IntPtr ptrEncryptedOldOwfPassword = outParamList[7];
                encryptedOldOwfPassword = TypeMarshal.ToNullableStruct<_NT_OWF_PASSWORD>(ptrEncryptedOldOwfPassword);

                IntPtr ptrTrustInfo = outParamList[8];
                ptrTrustInfo = Marshal.ReadIntPtr(ptrTrustInfo);
                trustInfo = TypeMarshal.ToNullableStruct<_NL_GENERIC_RPC_DATA>(ptrTrustInfo);

                retVal = outParamList[9].ToInt32();
            }

            ptrTrustedDcName.Dispose();
            ptrAccountName.Dispose();
            ptrComputerName.Dispose();
            ptrAuthenticator.Dispose();

            return (NtStatus)retVal;
        }

        /// <summary>
        ///  OpnumUnused47 method. Opnum: 47 
        /// </summary>
        /// <param name="contextHandle">
        ///  A primitive RPC handle that identifies a particular
        ///  client/server binding.
        /// </param>
        /// <returns>
        /// the NtStatus.
        /// </returns>
        public NtStatus OpnumUnused47(IntPtr contextHandle)
        {
            const ushort Opnum = 47;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            paramList = new Int3264[] 
            {
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
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
        /// The DsrUpdateReadOnlyServerDnsRecords method allows an RODC to send a control 
        /// command to a normal (writable) DC for site-specific and CName types of DNS records 
        /// update. For registration, site-specific records should be for the site in which 
        /// RODC resides. For the types of DNS records, see [MS-ADTS] section 7.3.2. Opnum: 48
        /// </summary>
        /// <param name="serverName">
        /// The custom RPC binding handle (as specified in section 3.5.5.1) that represents 
        /// the connection to the normal (writable) DC.
        /// </param>
        /// <param name="computerName">
        /// A null-terminated Unicode string that contains the client computer NetBIOS name.
        /// </param>
        /// <param name="authenticator">
        /// A pointer to a NETLOGON_AUTHENTICATOR structure (as specified in section 2.2.1.1.5) 
        /// that contains the client authenticator that will be used to authenticate the client.
        /// </param>
        /// <param name="returnAuthenticator">
        /// A pointer to a NETLOGON_AUTHENTICATOR structure that contains the server return 
        /// authenticator.
        /// </param>
        /// <param name="siteName">
        /// A pointer to a null-terminated Unicode string that contains the site name where 
        /// the RODC resides.
        /// </param>
        /// <param name="dnsTtl">
        /// The Time To Live value, in seconds, for DNS records.
        /// </param>
        /// <param name="dnsNames">
        /// A pointer to an NL_DNS_NAME_INFO_ARRAY (section 2.2.1.2.6) structure that contains 
        /// an array of NL_DNS_NAME_INFO structures.
        /// </param>
        /// <returns>
        /// the NtStatus.
        /// </returns>
        public NtStatus DsrUpdateReadOnlyServerDnsRecords(
            string serverName,
            string computerName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            out _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            string siteName,
            uint dnsTtl,
            ref _NL_DNS_NAME_INFO_ARRAY? dnsNames)
        {
            const ushort Opnum = 48;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrServerName = Marshal.StringToHGlobalUni(serverName);
            SafeIntPtr ptrComputerName = Marshal.StringToHGlobalUni(computerName);
            SafeIntPtr ptrAuthenticator = TypeMarshal.ToIntPtr(authenticator);
            SafeIntPtr ptrSiteName = Marshal.StringToHGlobalUni(siteName);
            SafeIntPtr ptrDnsNamesIn = TypeMarshal.ToIntPtr(dnsNames);

            paramList = new Int3264[] 
            {
                ptrServerName,
                ptrComputerName,
                ptrAuthenticator,
                IntPtr.Zero,
                ptrSiteName,
                dnsTtl,
                ptrDnsNamesIn,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrReturnAuthenticator = outParamList[3];
                returnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(ptrReturnAuthenticator);

                IntPtr ptrDnsNamesOut = outParamList[6];
                dnsNames = TypeMarshal.ToNullableStruct<_NL_DNS_NAME_INFO_ARRAY>(ptrDnsNamesOut);

                retVal = outParamList[7].ToInt32();
            }

            ptrServerName.Dispose();
            ptrComputerName.Dispose();
            ptrAuthenticator.Dispose();
            ptrSiteName.Dispose();
            ptrDnsNamesIn.Dispose();

            return (NtStatus)retVal;
        }

        /// <summary>
        /// When an RODC receives either the NetrServerAuthenticate3 method or the 
        /// NetrLogonGetDomainInfo method with updates requested, it invokes this method 
        /// on a normal (writable) DC to update to a client's computer account object in 
        /// Active Directory.
        /// </summary>
        /// <param name="primaryName">
        /// The custom RPC binding handle, as specified in section 3.5.5.1.
        /// </param>
        /// <param name="chainedFromServerName">
        /// The null-terminated Unicode string that contains the name of the read-only 
        /// DC that issues the request.
        /// </param>
        /// <param name="chainedForClientName">
        /// The null-terminated Unicode string that contains the name of the client 
        /// computer that called NetrServerAuthenticate3 or NetrLogonGetDomainInfo on 
        /// the RODC.
        /// </param>
        /// <param name="authenticator">
        /// A pointer to a NETLOGON_AUTHENTICATOR structure that contains the client 
        /// authenticator.
        /// </param>
        /// <param name="returnAuthenticator">
        /// A pointer to a NETLOGON_AUTHENTICATOR structure that contains the server 
        /// return authenticator.
        /// </param>
        /// <param name="dwdwInVersion">
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
        /// <returns>
        /// The NtStatus.
        /// </returns>
        public NtStatus NetrChainSetClientAttributes(
            string primaryName,
            string chainedFromServerName,
            string chainedForClientName,
            _NETLOGON_AUTHENTICATOR? authenticator,
            ref _NETLOGON_AUTHENTICATOR? returnAuthenticator,
            uint dwdwInVersion,
            NL_IN_CHAIN_SET_CLIENT_ATTRIBUTES? pmsgIn,
            ref uint? pdwOutVersion,
            ref NL_OUT_CHAIN_SET_CLIENT_ATTRIBUTES? pmsgOut)
        {
            const ushort Opnum = 49;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;
            int retVal;

            SafeIntPtr ptrPrimaryName = Marshal.StringToHGlobalUni(primaryName);
            SafeIntPtr ptrChainedFromServerName = Marshal.StringToHGlobalUni(chainedFromServerName);
            SafeIntPtr ptrChainedForClientName = Marshal.StringToHGlobalUni(chainedForClientName);
            SafeIntPtr ptrAuthenticator = TypeMarshal.ToIntPtr(authenticator);
            SafeIntPtr ptrReturnAuthenticatorIn = TypeMarshal.ToIntPtr(returnAuthenticator);
            SafeIntPtr ptrptrMsgIn = TypeMarshal.ToIntPtr(pmsgIn, dwdwInVersion, null, null);
            SafeIntPtr ppdwOutVersion = TypeMarshal.ToIntPtr(pdwOutVersion);
            SafeIntPtr ptrptrMsgOutIn = TypeMarshal.ToIntPtr(pmsgOut, pdwOutVersion.Value, null, null);

            paramList = new Int3264[] 
            {
                ptrPrimaryName,
                ptrChainedFromServerName,
                ptrChainedForClientName,
                ptrAuthenticator,
                ptrReturnAuthenticatorIn,
                dwdwInVersion,
                ptrptrMsgIn,
                ppdwOutVersion,
                ptrptrMsgOutIn,
                0 // retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    paramList);

            this.rpceClientTransport.Call(Opnum, requestStub, this.rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                 RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { new RpceStubExprEval(Logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[Opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrReturnAuthenticatorOut = outParamList[4];
                returnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(ptrReturnAuthenticatorOut);

                pdwOutVersion = TypeMarshal.ToNullableStruct<uint>(outParamList[7]);

                IntPtr ptrptrMsgOutOut = outParamList[8];
                pmsgOut = TypeMarshal.ToNullableStruct<NL_OUT_CHAIN_SET_CLIENT_ATTRIBUTES>(
                    ptrptrMsgOutOut,
                    pdwOutVersion.Value,
                    null,
                    null);

                retVal = outParamList[9].ToInt32();
            }

            ptrPrimaryName.Dispose();
            ptrChainedFromServerName.Dispose();
            ptrChainedForClientName.Dispose();
            ptrAuthenticator.Dispose();
            ptrReturnAuthenticatorIn.Dispose();
            ptrptrMsgIn.Dispose();
            ppdwOutVersion.Dispose();
            ptrptrMsgOutIn.Dispose();

            return (NtStatus)retVal;
        }

        #endregion

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
        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Release managed resources.
                if (this.rpceClientTransport != null)
                {
                    this.rpceClientTransport.Dispose();
                    this.rpceClientTransport = null;
                }
            }

            // Release unmanaged resources.
        }

        #endregion

        /// <summary>
        /// Logon__NETLOGON_DELTA_USERExprEval_0000 defined by MIDL.
        /// </summary>
        /// <param name="rpcStub">RpceStub structure.</param>
        internal static void Logon__NETLOGON_DELTA_USERExprEval_0000(RpceStub rpcStub)
        {
            IntPtr ptrStackTop = rpcStub.GetStackTop();
            _NLPR_LOGON_HOURS nlprLogonHours = (_NLPR_LOGON_HOURS)Marshal.PtrToStructure(
                ptrStackTop,
                typeof(_NLPR_LOGON_HOURS));
            rpcStub.SetOffset(0);
            rpcStub.SetMaxCount((uint)((nlprLogonHours.UnitsPerWeek + 7) / 8));
        }
    }
}
