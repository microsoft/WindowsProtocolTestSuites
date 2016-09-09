// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc
{
    /// <summary>
    /// A struct contains client session information
    /// </summary>
    public struct NrpcClientSessionInfo
    {
        /// <summary>
        /// The ComputerName used by the DC during session-key negotiations
        /// </summary>
        public string ComputerName;

        /// <summary>
        /// A 64-bit integer value used for detecting out-of-order
        /// messages on the client side
        /// </summary>
        public ulong? ClientSequenceNumber;

        /// <summary>
        /// The RID of this client’s machine account
        /// </summary>
        public uint? AccountRid;

        /// <summary>
        /// A 64-bit integer value used for detecting out-of-order messages on the server side
        /// </summary>
        public ulong? ServerSequenceNumber;

        /// <summary>
        /// Session-key negotiation between a client and a server is performed over
        /// an unprotected RPC channel.
        /// </summary>
        public byte[] SessionKey;

        /// <summary>
        /// A 32-bit set of bit flags that identify the negotiated capabilities between the client and the server
        /// </summary>
        public NrpcNegotiateFlags NegotiateFlags;

        /// <summary>
        /// A NETLOGON_CREDENTIAL structure containing
        /// the credential that is created by the server and received by
        /// the client and that is used during computation and verification
        /// of the Netlogon authenticator.
        /// </summary>
        public byte[] ServerStoredCredential;

        /// <summary>
        /// A NETLOGON_SECURE_CHANNEL_TYPE enumerated value, which indicates the type of
        /// secure channel being established with this client
        /// </summary>
        public _NETLOGON_SECURE_CHANNEL_TYPE SecureChannelType;


        /// <summary>
        /// Client Machine Password
        /// </summary>
        public string SharedSecret;
    };

    #region NRPC Messages Sent and Received

    /// <summary>
    /// Opnums of Nrpc methods
    /// </summary>
    public enum NrpcMethodOpnums : int
    {
        /// <summary>
        /// Opnum of method NetrLogonUasLogon
        /// </summary>
        NetrLogonUasLogon = 0,

        /// <summary>
        /// Opnum of method NetrLogonUasLogoff
        /// </summary>
        NetrLogonUasLogoff = 1,

        /// <summary>
        /// Opnum of method NetrLogonSamLogon
        /// </summary>
        NetrLogonSamLogon = 2,

        /// <summary>
        /// Opnum of method NetrLogonSamLogoff
        /// </summary>
        NetrLogonSamLogoff = 3,

        /// <summary>
        /// Opnum of method NetrServerReqChallenge
        /// </summary>
        NetrServerReqChallenge = 4,

        /// <summary>
        /// Opnum of method NetrServerAuthenticate
        /// </summary>
        NetrServerAuthenticate = 5,

        /// <summary>
        /// Opnum of method NetrServerPasswordSet
        /// </summary>
        NetrServerPasswordSet = 6,

        /// <summary>
        /// Opnum of method NetrDatabaseDeltas
        /// </summary>
        NetrDatabaseDeltas = 7,

        /// <summary>
        /// Opnum of method NetrDatabaseSync
        /// </summary>
        NetrDatabaseSync = 8,

        /// <summary>
        /// Opnum of method NetrAccountDeltas
        /// </summary>
        NetrAccountDeltas = 9,

        /// <summary>
        /// Opnum of method NetrAccountSync
        /// </summary>
        NetrAccountSync = 10,

        /// <summary>
        /// Opnum of method NetrGetDCName
        /// </summary>
        NetrGetDCName = 11,

        /// <summary>
        /// Opnum of method NetrLogonControl
        /// </summary>
        NetrLogonControl = 12,

        /// <summary>
        /// Opnum of method NetrGetAnyDCName
        /// </summary>
        NetrGetAnyDCName = 13,

        /// <summary>
        /// Opnum of method NetrLogonControl2
        /// </summary>
        NetrLogonControl2 = 14,

        /// <summary>
        /// Opnum of method NetrServerAuthenticate2
        /// </summary>
        NetrServerAuthenticate2 = 15,

        /// <summary>
        /// Opnum of method NetrDatabaseSync2
        /// </summary>
        NetrDatabaseSync2 = 16,

        /// <summary>
        /// Opnum of method NetrDatabaseRedo
        /// </summary>
        NetrDatabaseRedo = 17,

        /// <summary>
        /// Opnum of method NetrLogonControl2Ex
        /// </summary>
        NetrLogonControl2Ex = 18,

        /// <summary>
        /// Opnum of method NetrEnumerateTrustedDomains
        /// </summary>
        NetrEnumerateTrustedDomains = 19,

        /// <summary>
        /// Opnum of method DsrGetDcName
        /// </summary>
        DsrGetDcName = 20,

        /// <summary>
        /// Opnum of method NetrLogonGetCapabilities
        /// </summary>
        NetrLogonGetCapabilities = 21,

        /// <summary>
        /// Opnum of method NetrLogonSetServiceBits
        /// </summary>
        NetrLogonSetServiceBits = 22,

        /// <summary>
        /// Opnum of method NetrLogonGetTrustRid
        /// </summary>
        NetrLogonGetTrustRid = 23,

        /// <summary>
        /// Opnum of method NetrLogonComputeServerDigest
        /// </summary>
        NetrLogonComputeServerDigest = 24,

        /// <summary>
        /// Opnum of method NetrLogonComputeClientDigest
        /// </summary>
        NetrLogonComputeClientDigest = 25,

        /// <summary>
        /// Opnum of method NetrServerAuthenticate3
        /// </summary>
        NetrServerAuthenticate3 = 26,

        /// <summary>
        /// Opnum of method DsrGetDcNameEx
        /// </summary>
        DsrGetDcNameEx = 27,

        /// <summary>
        /// Opnum of method DsrGetSiteName
        /// </summary>
        DsrGetSiteName = 28,

        /// <summary>
        /// Opnum of method NetrLogonGetDomainInfo
        /// </summary>
        NetrLogonGetDomainInfo = 29,

        /// <summary>
        /// Opnum of method NetrServerPasswordSet2
        /// </summary>
        NetrServerPasswordSet2 = 30,

        /// <summary>
        /// Opnum of method NetrServerPasswordGet
        /// </summary>
        NetrServerPasswordGet = 31,

        /// <summary>
        /// Opnum of method NetrLogonSendToSam
        /// </summary>
        NetrLogonSendToSam = 32,

        /// <summary>
        /// Opnum of method DsrAddressToSiteNamesW
        /// </summary>
        DsrAddressToSiteNamesW = 33,

        /// <summary>
        /// Opnum of method DsrGetDcNameEx2
        /// </summary>
        DsrGetDcNameEx2 = 34,

        /// <summary>
        /// Opnum of method NetrLogonGetTimeServiceParentDomain
        /// </summary>
        NetrLogonGetTimeServiceParentDomain = 35,

        /// <summary>
        /// Opnum of method NetrEnumerateTrustedDomainsEx
        /// </summary>
        NetrEnumerateTrustedDomainsEx = 36,

        /// <summary>
        /// Opnum of method DsrAddressToSiteNamesExW
        /// </summary>
        DsrAddressToSiteNamesExW = 37,

        /// <summary>
        /// Opnum of method DsrGetDcSiteCoverageW
        /// </summary>
        DsrGetDcSiteCoverageW = 38,

        /// <summary>
        /// Opnum of method NetrLogonSamLogonEx
        /// </summary>
        NetrLogonSamLogonEx = 39,

        /// <summary>
        /// Opnum of method DsrEnumerateDomainTrusts
        /// </summary>
        DsrEnumerateDomainTrusts = 40,

        /// <summary>
        /// Opnum of method DsrDeregisterDnsHostRecords
        /// </summary>
        DsrDeregisterDnsHostRecords = 41,

        /// <summary>
        /// Opnum of method NetrServerTrustPasswordsGet
        /// </summary>
        NetrServerTrustPasswordsGet = 42,

        /// <summary>
        /// Opnum of method DsrGetForestTrustInformation
        /// </summary>
        DsrGetForestTrustInformation = 43,

        /// <summary>
        /// Opnum of method NetrGetForestTrustInformation
        /// </summary>
        NetrGetForestTrustInformation = 44,

        /// <summary>
        /// Opnum of method NetrLogonSamLogonWithFlags
        /// </summary>
        NetrLogonSamLogonWithFlags = 45,

        /// <summary>
        /// Opnum of method NetrServerGetTrustInfo
        /// </summary>
        NetrServerGetTrustInfo = 46,

        /// <summary>
        /// Opnum of method OpnumUnused47
        /// </summary>
        OpnumUnused47 = 47,

        /// <summary>
        /// Opnum of method DsrUpdateReadOnlyServerDnsRecords
        /// </summary>
        DsrUpdateReadOnlyServerDnsRecords = 48,

        /// <summary>
        /// Opnum of method NetrChainSetClientAttributes
        /// </summary>
        NetrChainSetClientAttributes = 49
    };


    /// <summary>
    /// The base class of all Nrpc request
    /// </summary>
    public abstract class NrpcRequestStub
    {
        private NrpcMethodOpnums rpceLayerOpnum;
        /// <summary>
        ///  Opnum at RPC layer
        /// </summary>
        public NrpcMethodOpnums Opnum
        {
            get
            {
                return rpceLayerOpnum;
            }

            protected set
            {
                rpceLayerOpnum = value;
            }
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal abstract void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub);
    }


    /// <summary>
    /// The base class of all Nrpc response
    /// </summary>
    public abstract class NrpcResponseStub
    {
        /// <summary>
        ///  Opnum at RPC layer
        /// </summary>
        private NrpcMethodOpnums rpceLayerOpnum;
        /// <summary>
        ///  Opnum at RPC layer
        /// </summary>
        public NrpcMethodOpnums Opnum
        {
            get
            {
                return rpceLayerOpnum;
            }

            protected set
            {
                rpceLayerOpnum = value;
            }
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal abstract byte[] Encode(NrpcServerSessionContext sessionContext);


        /// <summary>
        /// encode param list to bytes
        /// </summary>
        /// <param name="paramList">param list</param>
        /// <param name="Opnum">rpc method opnum</param>
        /// <returns>bytes array</returns>
        internal protected byte[] NrpcStubEncodeToBytes(Int3264[] paramList, NrpcMethodOpnums Opnum)
        {
            return RpceStubEncoder.ToBytes(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] { 
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                paramList);
        }
    }

    #region Structures of input and ouput parameters of NRPC methods
    /// <summary>
    /// The NetrLogonUasLogonRequest class defines input parameters of method NetrLogonUasLogon.
    /// </summary>
    public class NrpcNetrLogonUasLogonRequest : NrpcRequestStub
    {
        /// <summary>
        ///  ServerName parameter.
        /// </summary>
        public string ServerName;

        /// <summary>
        ///  UserName parameter.
        /// </summary>
        public string UserName;

        /// <summary>
        ///  Workstation parameter.
        /// </summary>
        public string Workstation;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonUasLogonRequest()
        {
            Opnum = NrpcMethodOpnums.NetrLogonUasLogon;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ServerName = Marshal.PtrToStringUni(outParamList[0]);
                UserName = Marshal.PtrToStringUni(outParamList[1]);
                Workstation = Marshal.PtrToStringUni(outParamList[2]);
            }
        }
    };


    /// <summary>
    /// The NetrLogonUasLogonResponse class defines output parameters of method NetrLogonUasLogon.
    /// </summary>
    public class NrpcNetrLogonUasLogonResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NetApiStatus Status;

        /// <summary>
        ///  ValidationInformation parameter.
        /// </summary>
        public _NETLOGON_VALIDATION_UAS_INFO? ValidationInformation;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonUasLogonResponse()
        {
            Opnum = NrpcMethodOpnums.NetrLogonUasLogon;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrLogonUasLogonRequest request = sessionContext.LastRequestReceived as NrpcNetrLogonUasLogonRequest;

            if (request == null)
            {
                throw new InvalidOperationException(
                    "The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pValidationInformation = TypeMarshal.ToIntPtr(ValidationInformation);
            SafeIntPtr ppValidationInformation = TypeMarshal.ToIntPtr(pValidationInformation.Value);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ppValidationInformation,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pValidationInformation.Dispose();
                ppValidationInformation.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrLogonUasLogoffRequest class defines input parameters of method NetrLogonUasLogoff.
    /// </summary>
    public class NrpcNetrLogonUasLogoffRequest : NrpcRequestStub
    {
        /// <summary>
        ///  ServerName parameter.
        /// </summary>
        public string ServerName;

        /// <summary>
        ///  UserName parameter.
        /// </summary>
        public string UserName;

        /// <summary>
        ///  Workstation parameter.
        /// </summary>
        public string Workstation;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonUasLogoffRequest()
        {
            Opnum = NrpcMethodOpnums.NetrLogonUasLogoff;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000)},
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ServerName = Marshal.PtrToStringUni(outParamList[0]);
                UserName = Marshal.PtrToStringUni(outParamList[1]);
                Workstation = Marshal.PtrToStringUni(outParamList[2]);
            }
        }
    };


    /// <summary>
    /// The NetrLogonUasLogoffResponse class defines output parameters of method NetrLogonUasLogoff.
    /// </summary>
    public class NrpcNetrLogonUasLogoffResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NetApiStatus Status;

        /// <summary>
        ///  ValidationInformation parameter.
        /// </summary>
        public _NETLOGON_LOGOFF_UAS_INFO? LogoffInformation;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonUasLogoffResponse()
        {
            Opnum = NrpcMethodOpnums.NetrLogonUasLogoff;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrLogonUasLogoffRequest request = sessionContext.LastRequestReceived as NrpcNetrLogonUasLogoffRequest;

            if (request == null)
            {
                throw new InvalidOperationException(
                    "The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pLogoffInformation = TypeMarshal.ToIntPtr(LogoffInformation);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pLogoffInformation,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pLogoffInformation.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrLogonSamLogonRequest class defines input parameters of method NetrLogonSamLogon.
    /// </summary>
    public class NrpcNetrLogonSamLogonRequest : NrpcRequestStub
    {
        /// <summary>
        ///  LogonServer parameter.
        /// </summary>
        public string LogonServer;

        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        ///  Authenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? Authenticator;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        ///  LogonLevel parameter.
        /// </summary>
        public _NETLOGON_LOGON_INFO_CLASS LogonLevel;

        /// <summary>
        ///  LogonInformation parameter.
        /// </summary>
        public _NETLOGON_LEVEL? LogonInformation;

        /// <summary>
        ///  ValidationLevel parameter.
        /// </summary>
        public _NETLOGON_VALIDATION_INFO_CLASS ValidationLevel;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonSamLogonRequest()
        {
            Opnum = NrpcMethodOpnums.NetrLogonSamLogon;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                  RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] {
                        new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    requestStub,
                    sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                LogonServer = Marshal.PtrToStringUni(outParamList[0]);
                ComputerName = Marshal.PtrToStringUni(outParamList[1]);
                Authenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[2]);
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[3]);
                LogonLevel = (_NETLOGON_LOGON_INFO_CLASS)outParamList[4].ToInt32();
                LogonInformation = TypeMarshal.ToNullableStruct<_NETLOGON_LEVEL>(outParamList[5],
                    LogonLevel, null, null);
                ValidationLevel = (_NETLOGON_VALIDATION_INFO_CLASS)outParamList[6].ToInt32();
            }
        }
    };


    /// <summary>
    /// The NetrLogonSamLogonResponse class defines output parameters of method NetrLogonSamLogon.
    /// </summary>
    public class NrpcNetrLogonSamLogonResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NtStatus Status;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR ReturnAuthenticator;

        /// <summary>
        ///  ValidationInformation parameter.
        /// </summary>
        public _NETLOGON_VALIDATION ValidationInformation;

        /// <summary>
        ///  Authoritative parameter.
        /// </summary>
        public byte? Authoritative;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonSamLogonResponse()
        {
            Opnum = NrpcMethodOpnums.NetrLogonSamLogon;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrLogonSamLogonRequest request =
                sessionContext.LastRequestReceived as NrpcNetrLogonSamLogonRequest;

            if (request == null)
            {
                throw new InvalidOperationException(
                    "The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pReturnAuthenticator = TypeMarshal.ToIntPtr(ReturnAuthenticator);
            SafeIntPtr pValidationInformation = TypeMarshal.ToIntPtr(ValidationInformation,
                request.ValidationLevel, null, null);
            SafeIntPtr pAuthoritative = TypeMarshal.ToIntPtr(Authoritative);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pReturnAuthenticator,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    (uint)request.ValidationLevel,
                    pValidationInformation,
                    pAuthoritative,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pReturnAuthenticator.Dispose();
                pValidationInformation.Dispose();
                pAuthoritative.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrLogonSamLogoffRequest class defines input parameters of method NetrLogonSamLogoff.
    /// </summary>
    public class NrpcNetrLogonSamLogoffRequest : NrpcRequestStub
    {
        /// <summary>
        ///  LogonServer parameter.
        /// </summary>
        public string LogonServer;

        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        ///  Authenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? Authenticator;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        ///  LogonLevel parameter.
        /// </summary>
        public _NETLOGON_LOGON_INFO_CLASS LogonLevel;

        /// <summary>
        ///  LogonInformation parameter.
        /// </summary>
        public _NETLOGON_LEVEL? LogonInformation;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonSamLogoffRequest()
        {
            Opnum = NrpcMethodOpnums.NetrLogonSamLogoff;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                        new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                LogonServer = Marshal.PtrToStringUni(outParamList[0]);
                ComputerName = Marshal.PtrToStringUni(outParamList[1]);
                Authenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[2]);
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[3]);
                LogonLevel = (_NETLOGON_LOGON_INFO_CLASS)outParamList[4].ToInt32();
                LogonInformation = TypeMarshal.ToNullableStruct<_NETLOGON_LEVEL>(outParamList[5], LogonLevel, null, null);
            }
        }
    };


    /// <summary>
    /// The NetrLogonSamLogoffResponse class defines output parameters of method NetrLogonSamLogoff.
    /// </summary>
    public class NrpcNetrLogonSamLogoffResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NtStatus Status;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonSamLogoffResponse()
        {
            Opnum = NrpcMethodOpnums.NetrLogonSamLogoff;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrLogonSamLogoffRequest request = sessionContext.LastRequestReceived as NrpcNetrLogonSamLogoffRequest;

            if (request == null)
            {
                throw new InvalidOperationException(
                    "The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pReturnAuthenticator = TypeMarshal.ToIntPtr(ReturnAuthenticator);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pReturnAuthenticator,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pReturnAuthenticator.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrServerReqChallengeRequest class defines input parameters of method NetrServerReqChallenge.
    /// </summary>
    public class NrpcNetrServerReqChallengeRequest : NrpcRequestStub
    {
        /// <summary>
        ///  PrimaryName parameter.
        /// </summary>
        public string PrimaryName;

        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        ///  ClientChallenge parameter.
        /// </summary>
        public _NETLOGON_CREDENTIAL? ClientChallenge;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrServerReqChallengeRequest()
        {
            Opnum = NrpcMethodOpnums.NetrServerReqChallenge;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                  RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] {
                        new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    requestStub,
                    sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                PrimaryName = Marshal.PtrToStringUni(outParamList[0]);
                ComputerName = Marshal.PtrToStringUni(outParamList[1]);
                ClientChallenge = TypeMarshal.ToNullableStruct<_NETLOGON_CREDENTIAL>(outParamList[2]);
            }
        }
    };


    /// <summary>
    /// The NetrServerReqChallengeResponse class defines output parameters of method NetrServerReqChallenge.
    /// </summary>
    public class NrpcNetrServerReqChallengeResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NtStatus Status;

        /// <summary>
        ///  ServerChallenge parameter.
        /// </summary>
        public _NETLOGON_CREDENTIAL? ServerChallenge;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrServerReqChallengeResponse()
        {
            Opnum = NrpcMethodOpnums.NetrServerReqChallenge;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrServerReqChallengeRequest request =
                sessionContext.LastRequestReceived as NrpcNetrServerReqChallengeRequest;

            if (request == null)
            {
                throw new InvalidOperationException(
                    "The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pServerChallenge = TypeMarshal.ToIntPtr(ServerChallenge);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pServerChallenge,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pServerChallenge.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrServerAuthenticateRequest class defines input parameters of method NetrServerAuthenticate.
    /// </summary>
    public class NrpcNetrServerAuthenticateRequest : NrpcRequestStub
    {
        /// <summary>
        ///  PrimaryName parameter.
        /// </summary>
        public string PrimaryName;

        /// <summary>
        ///  AccountName parameter.
        /// </summary>
        public string AccountName;

        /// <summary>
        ///  SecureChannelType parameter.
        /// </summary>
        public _NETLOGON_SECURE_CHANNEL_TYPE SecureChannelType;

        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        ///  ClientCredential parameter.
        /// </summary>
        public _NETLOGON_CREDENTIAL? ClientCredential;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrServerAuthenticateRequest()
        {
            Opnum = NrpcMethodOpnums.NetrServerAuthenticate;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                  RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] {
                        new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    requestStub,
                    sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                PrimaryName = Marshal.PtrToStringUni(outParamList[0]);
                AccountName = Marshal.PtrToStringUni(outParamList[1]);
                SecureChannelType = (_NETLOGON_SECURE_CHANNEL_TYPE)(outParamList[2].ToInt32());
                ComputerName = Marshal.PtrToStringUni(outParamList[3]);
                ClientCredential = TypeMarshal.ToNullableStruct<_NETLOGON_CREDENTIAL>(outParamList[4]);
            }
        }
    };


    /// <summary>
    /// The NetrServerAuthenticateResponse class defines output parameters of method NetrServerAuthenticate.
    /// </summary>
    public class NrpcNetrServerAuthenticateResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NtStatus Status;

        /// <summary>
        ///  ServerCredential parameter.
        /// </summary>
        public _NETLOGON_CREDENTIAL? ServerCredential;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrServerAuthenticateResponse()
        {
            Opnum = NrpcMethodOpnums.NetrServerAuthenticate;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrServerAuthenticateRequest request =
                sessionContext.LastRequestReceived as NrpcNetrServerAuthenticateRequest;

            if (request == null)
            {
                throw new InvalidOperationException(
                    "The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pServerCredential = TypeMarshal.ToIntPtr(ServerCredential);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pServerCredential,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pServerCredential.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrServerPasswordSetRequest class defines input parameters of method NetrServerPasswordSet.
    /// </summary>
    public class NrpcNetrServerPasswordSetRequest : NrpcRequestStub
    {
        /// <summary>
        ///  PrimaryName parameter.
        /// </summary>
        public string PrimaryName;

        /// <summary>
        ///  AccountName parameter.
        /// </summary>
        public string AccountName;

        /// <summary>
        ///  SecureChannelType parameter.
        /// </summary>
        public _NETLOGON_SECURE_CHANNEL_TYPE SecureChannelType;

        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        ///  Authenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? Authenticator;

        /// <summary>
        ///  UasNewPassword parameter.
        /// </summary>
        public _LM_OWF_PASSWORD? UasNewPassword;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrServerPasswordSetRequest()
        {
            Opnum = NrpcMethodOpnums.NetrServerPasswordSet;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                  RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] {
                        new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    requestStub,
                    sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                PrimaryName = Marshal.PtrToStringUni(outParamList[0]);
                AccountName = Marshal.PtrToStringUni(outParamList[1]);
                SecureChannelType = (_NETLOGON_SECURE_CHANNEL_TYPE)outParamList[2].ToInt32();
                ComputerName = Marshal.PtrToStringUni(outParamList[3]);
                Authenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[4]);
                UasNewPassword = TypeMarshal.ToNullableStruct<_LM_OWF_PASSWORD>(outParamList[6]);
            }
        }
    };


    /// <summary>
    /// The NetrServerPasswordSetResponse class defines output parameters of method NetrServerPasswordSet.
    /// </summary>
    public class NrpcNetrServerPasswordSetResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NtStatus Status;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrServerPasswordSetResponse()
        {
            Opnum = NrpcMethodOpnums.NetrServerPasswordSet;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrServerPasswordSetRequest request = sessionContext.LastRequestReceived as NrpcNetrServerPasswordSetRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pReturnAuthenticator = TypeMarshal.ToIntPtr(ReturnAuthenticator);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pReturnAuthenticator,
                    IntPtr.Zero,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pReturnAuthenticator.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrDatabaseDeltasRequest class defines input parameters of method NetrDatabaseDeltas.
    /// </summary>
    public class NrpcNetrDatabaseDeltasRequest : NrpcRequestStub
    {
        /// <summary>
        ///  PrimaryName parameter.
        /// </summary>
        public string PrimaryName;

        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? Authenticator;

        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public DatabaseID_Values DatabaseID;

        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public _NLPR_MODIFIED_COUNT? DomainModifiedCount;

        /// <summary>
        ///  PreferredMaximumLength parameter.
        /// </summary>
        public uint PreferredMaximumLength;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrDatabaseDeltasRequest()
        {
            Opnum = NrpcMethodOpnums.NetrDatabaseDeltas;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                PrimaryName = Marshal.PtrToStringUni(outParamList[0]);
                ComputerName = Marshal.PtrToStringUni(outParamList[1]);
                Authenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[2]);
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[3]);
                DatabaseID = (DatabaseID_Values)outParamList[4].ToUInt32();
                DomainModifiedCount = TypeMarshal.ToNullableStruct<_NLPR_MODIFIED_COUNT>(outParamList[5]);
                PreferredMaximumLength = outParamList[7].ToUInt32();
            }
        }
    };


    /// <summary>
    /// The NetrDatabaseDeltasResponse class defines output parameters of method NetrDatabaseDeltas.
    /// </summary>
    public class NrpcNetrDatabaseDeltasResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NtStatus Status;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        ///  DomainModifiedCount parameter.
        /// </summary>
        public _NLPR_MODIFIED_COUNT? DomainModifiedCount;

        /// <summary>
        ///  DeltaArray parameter.
        /// </summary>
        public _NETLOGON_DELTA_ENUM_ARRAY? DeltaArray;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrDatabaseDeltasResponse()
        {
            Opnum = NrpcMethodOpnums.NetrDatabaseDeltas;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrDatabaseDeltasRequest request = sessionContext.LastRequestReceived as NrpcNetrDatabaseDeltasRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pReturnAuthenticator = TypeMarshal.ToIntPtr(ReturnAuthenticator);
            SafeIntPtr pDomainModifiedCount = TypeMarshal.ToIntPtr(DomainModifiedCount);
            SafeIntPtr pDeltaArray = TypeMarshal.ToIntPtr(DeltaArray);
            SafeIntPtr ppDeltaArray = TypeMarshal.ToIntPtr(pDeltaArray.Value);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pReturnAuthenticator,
                    IntPtr.Zero,
                    pDomainModifiedCount,
                    ppDeltaArray,
                    IntPtr.Zero,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pReturnAuthenticator.Dispose();
                pDomainModifiedCount.Dispose();
                pDeltaArray.Dispose();
                ppDeltaArray.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrDatabaseSyncRequest class defines input parameters of method NetrDatabaseSync
    /// </summary>
    public class NrpcNetrDatabaseSyncRequest : NrpcRequestStub
    {
        /// <summary>
        ///  PrimaryName parameter.
        /// </summary>
        public string PrimaryName;

        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        ///  Authenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? Authenticator;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        ///  DatabaseID parameter.
        /// </summary>
        public DatabaseID_Values DatabaseID;

        /// <summary>
        ///  SyncContext parameter.
        /// </summary>
        public uint? SyncContext;

        /// <summary>
        ///  PreferredMaximumLength parameter.
        /// </summary>
        public uint PreferredMaximumLength;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrDatabaseSyncRequest()
        {
            Opnum = NrpcMethodOpnums.NetrDatabaseSync;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                PrimaryName = Marshal.PtrToStringUni(outParamList[0]);
                ComputerName = Marshal.PtrToStringUni(outParamList[1]);
                Authenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[2]);
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[3]);
                DatabaseID = (DatabaseID_Values)outParamList[4].ToUInt32();
                SyncContext = TypeMarshal.ToNullableStruct<uint>(outParamList[5]);
                PreferredMaximumLength = outParamList[7].ToUInt32();
            }
        }
    };


    /// <summary>
    /// The NetrDatabaseSyncResponse class defines output parameters of method NetrDatabaseSync
    /// </summary>
    public class NrpcNetrDatabaseSyncResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NtStatus Status;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        ///  SyncContext parameter.
        /// </summary>
        public uint? SyncContext;

        /// <summary>
        ///  DeltaArray parameter.
        /// </summary>
        public _NETLOGON_DELTA_ENUM_ARRAY? DeltaArray;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrDatabaseSyncResponse()
        {
            Opnum = NrpcMethodOpnums.NetrDatabaseSync;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrDatabaseSyncRequest request = sessionContext.LastRequestReceived as NrpcNetrDatabaseSyncRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pReturnAuthenticator = TypeMarshal.ToIntPtr(ReturnAuthenticator);
            SafeIntPtr pSyncContext = TypeMarshal.ToIntPtr(SyncContext);
            SafeIntPtr pDeltaArray = TypeMarshal.ToIntPtr(DeltaArray);
            SafeIntPtr ppDeltaArray = TypeMarshal.ToIntPtr(pDeltaArray.Value);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pReturnAuthenticator,
                    IntPtr.Zero,
                    pSyncContext,
                    ppDeltaArray,
                    IntPtr.Zero,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pReturnAuthenticator.Dispose();
                pSyncContext.Dispose();
                pDeltaArray.Dispose();
                ppDeltaArray.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrAccountDeltasRequest class defines input parameters of method NetrAccountDeltas
    /// </summary>
    public class NrpcNetrAccountDeltasRequest : NrpcRequestStub
    {
        /// <summary>
        ///  PrimaryName parameter.
        /// </summary>
        public string PrimaryName;

        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        ///  Authenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? Authenticator;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        ///  RecordID parameter.
        /// </summary>
        public _UAS_INFO_0? RecordID;

        /// <summary>
        ///  Count parameter.
        /// </summary>
        public uint Count;

        /// <summary>
        ///  Level parameter.
        /// </summary>
        public uint Level;

        /// <summary>
        ///  BufferSize parameter.
        /// </summary>
        public uint BufferSize;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrAccountDeltasRequest()
        {
            Opnum = NrpcMethodOpnums.NetrAccountDeltas;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                PrimaryName = Marshal.PtrToStringUni(outParamList[0]);
                ComputerName = Marshal.PtrToStringUni(outParamList[1]);
                Authenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[2]);
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[3]);
                RecordID = TypeMarshal.ToNullableStruct<_UAS_INFO_0>(outParamList[4]);
                Count = outParamList[5].ToUInt32();
                Level = outParamList[6].ToUInt32();
                BufferSize = outParamList[8].ToUInt32();
            }
        }
    };


    /// <summary>
    /// The NetrAccountDeltasResponse class defines output parameters of method NetrAccountDeltas
    /// </summary>
    public class NrpcNetrAccountDeltasResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NtStatus Status;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        ///  Buffer parameter.
        /// </summary>
        public byte[] Buffer;

        /// <summary>
        ///  CountReturned parameter.
        /// </summary>
        public uint? CountReturned;

        /// <summary>
        ///  TotalEntries parameter.
        /// </summary>
        public uint? TotalEntries;

        /// <summary>
        ///  NextRecordId parameter.
        /// </summary>
        public _UAS_INFO_0? NextRecordId;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrAccountDeltasResponse()
        {
            Opnum = NrpcMethodOpnums.NetrAccountDeltas;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrAccountDeltasRequest request = sessionContext.LastRequestReceived as NrpcNetrAccountDeltasRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pReturnAuthenticator = TypeMarshal.ToIntPtr(ReturnAuthenticator);
            SafeIntPtr pBuffer = TypeMarshal.ToIntPtr(Buffer);
            SafeIntPtr pCountReturned = TypeMarshal.ToIntPtr(CountReturned);
            SafeIntPtr pTotalEntries = TypeMarshal.ToIntPtr(TotalEntries);
            SafeIntPtr pNextRecordId = TypeMarshal.ToIntPtr(NextRecordId);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pReturnAuthenticator,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pBuffer,
                    IntPtr.Zero,
                    pCountReturned,
                    pTotalEntries,
                    pNextRecordId,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pReturnAuthenticator.Dispose();
                pBuffer.Dispose();
                pCountReturned.Dispose();
                pTotalEntries.Dispose();
                pNextRecordId.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrAccountSyncRequest class defines input parameters of method NetrAccountSync
    /// </summary>
    public class NrpcNetrAccountSyncRequest : NrpcRequestStub
    {
        /// <summary>
        ///  PrimaryName parameter.
        /// </summary>
        public string PrimaryName;

        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        ///  Authenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? Authenticator;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        ///  Reference parameter.
        /// </summary>
        public uint Reference;

        /// <summary>
        ///  Level parameter.
        /// </summary>
        public uint Level;

        /// <summary>
        ///  BufferSize parameter.
        /// </summary>
        public uint BufferSize;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrAccountSyncRequest()
        {
            Opnum = NrpcMethodOpnums.NetrAccountSync;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                PrimaryName = Marshal.PtrToStringUni(outParamList[0]);
                ComputerName = Marshal.PtrToStringUni(outParamList[1]);
                Authenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[2]);
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[3]);
                Reference = outParamList[4].ToUInt32();
                Level = outParamList[5].ToUInt32();
                BufferSize = outParamList[7].ToUInt32();
            }
        }
    };


    /// <summary>
    /// The NetrAccountSyncResponse class defines output parameters of method NetrAccountSync
    /// </summary>
    public class NrpcNetrAccountSyncResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NtStatus Status;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        ///  Buffer parameter.
        /// </summary>
        public byte[] Buffer;

        /// <summary>
        ///  CountReturned parameter.
        /// </summary>
        public uint? CountReturned;

        /// <summary>
        ///  TotalEntries parameter.
        /// </summary>
        public uint? TotalEntries;

        /// <summary>
        ///  NextReference parameter.
        /// </summary>
        public uint? NextReference;

        /// <summary>
        ///  LastRecordId parameter.
        /// </summary>
        public _UAS_INFO_0? LastRecordId;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrAccountSyncResponse()
        {
            Opnum = NrpcMethodOpnums.NetrAccountSync;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrAccountSyncRequest request = sessionContext.LastRequestReceived as NrpcNetrAccountSyncRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pReturnAuthenticator = TypeMarshal.ToIntPtr(ReturnAuthenticator);
            SafeIntPtr pBuffer = IntPtrUtility.ArrayToPtr(Buffer);
            SafeIntPtr pCountReturned = TypeMarshal.ToIntPtr(CountReturned);
            SafeIntPtr pTotalEntries = TypeMarshal.ToIntPtr(TotalEntries);
            SafeIntPtr pNextReference = TypeMarshal.ToIntPtr(NextReference);
            SafeIntPtr pLastRecordId = TypeMarshal.ToIntPtr(LastRecordId);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pReturnAuthenticator,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pBuffer,
                    IntPtr.Zero,
                    pCountReturned,
                    pTotalEntries,
                    pNextReference,
                    pLastRecordId,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pReturnAuthenticator.Dispose();
                pBuffer.Dispose();
                pCountReturned.Dispose();
                pTotalEntries.Dispose();
                pNextReference.Dispose();
                pLastRecordId.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrGetDCNameRequest class defines input parameters of method NetrGetDCName
    /// </summary>
    public class NrpcNetrGetDCNameRequest : NrpcRequestStub
    {
        /// <summary>
        ///  ServerName parameter.
        /// </summary>
        public string ServerName;

        /// <summary>
        ///  DomainName parameter.
        /// </summary>
        public string DomainName;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrGetDCNameRequest()
        {
            Opnum = NrpcMethodOpnums.NetrGetDCName;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ServerName = Marshal.PtrToStringUni(outParamList[0]);
                DomainName = Marshal.PtrToStringUni(outParamList[1]);
            }
        }
    };


    /// <summary>
    /// The NetrGetDCNameResponse class defines output parameters of method NetrGetDCName
    /// </summary>
    public class NrpcNetrGetDCNameResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NetApiStatus Status;

        /// <summary>
        ///  Buffer parameter.
        /// </summary>
        public string Buffer;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrGetDCNameResponse()
        {
            Opnum = NrpcMethodOpnums.NetrGetDCName;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrGetDCNameRequest request = sessionContext.LastRequestReceived as NrpcNetrGetDCNameRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pBuffer = Marshal.StringToHGlobalUni(Buffer);
            SafeIntPtr ppBuffer = TypeMarshal.ToIntPtr(pBuffer.Value);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ppBuffer,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pBuffer.Dispose();
                ppBuffer.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrLogonControlRequest class defines input parameters of method NetrLogonControl
    /// </summary>
    public class NrpcNetrLogonControlRequest : NrpcRequestStub
    {
        /// <summary>
        ///  ServerName parameter.
        /// </summary>
        public string ServerName;

        /// <summary>
        ///  FunctionCode parameter.
        /// </summary>
        public FunctionCode_Values FunctionCode;

        /// <summary>
        ///  QueryLevel parameter.
        /// </summary>
        public QueryLevel_Values QueryLevel;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonControlRequest()
        {
            Opnum = NrpcMethodOpnums.NetrLogonControl;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ServerName = Marshal.PtrToStringUni(outParamList[0]);
                FunctionCode = (FunctionCode_Values)outParamList[1].ToUInt32();
                QueryLevel = (QueryLevel_Values)outParamList[2].ToUInt32();
            }
        }
    };


    /// <summary>
    /// The NetrLogonControlResponse class defines output parameters of method NetrLogonControl
    /// </summary>
    public class NrpcNetrLogonControlResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NetApiStatus Status;

        /// <summary>
        ///  Buffer parameter.
        /// </summary>
        public _NETLOGON_CONTROL_QUERY_INFORMATION? Buffer;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonControlResponse()
        {
            Opnum = NrpcMethodOpnums.NetrLogonControl;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrLogonControlRequest request = sessionContext.LastRequestReceived as NrpcNetrLogonControlRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pBuffer = TypeMarshal.ToIntPtr(Buffer, request.QueryLevel, null, null);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    (uint)request.QueryLevel,
                    pBuffer,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pBuffer.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrGetAnyDCNameRequest class defines input parameters of method NetrGetAnyDCName
    /// </summary>
    public class NrpcNetrGetAnyDCNameRequest : NrpcRequestStub
    {
        /// <summary>
        ///  ServerName parameter.
        /// </summary>
        public string ServerName;

        /// <summary>
        ///  DomainName parameter.
        /// </summary>
        public string DomainName;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrGetAnyDCNameRequest()
        {
            Opnum = NrpcMethodOpnums.NetrGetAnyDCName;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ServerName = Marshal.PtrToStringUni(outParamList[0]);
                DomainName = Marshal.PtrToStringUni(outParamList[1]);
            }
        }
    };


    /// <summary>
    /// The NetrGetAnyDCNameResponse class defines output parameters of method NetrGetAnyDCName
    /// </summary>
    public class NrpcNetrGetAnyDCNameResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NetApiStatus Status;

        /// <summary>
        ///  Buffer parameter.
        /// </summary>
        public string Buffer;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrGetAnyDCNameResponse()
        {
            Opnum = NrpcMethodOpnums.NetrGetAnyDCName;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrGetAnyDCNameRequest request = sessionContext.LastRequestReceived as NrpcNetrGetAnyDCNameRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pBuffer = Marshal.StringToHGlobalUni(Buffer);
            SafeIntPtr ppBuffer = TypeMarshal.ToIntPtr(pBuffer.Value);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ppBuffer,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pBuffer.Dispose();
                ppBuffer.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrLogonControl2Request class defines input parameters of method NetrLogonControl2
    /// </summary>
    public class NrpcNetrLogonControl2Request : NrpcRequestStub
    {
        /// <summary>
        ///  ServerName parameter.
        /// </summary>
        public string ServerName;

        /// <summary>
        ///  FunctionCode parameter.
        /// </summary>
        public FunctionCode_Values FunctionCode;

        /// <summary>
        ///  QueryLevel parameter.
        /// </summary>
        public QueryLevel_Values QueryLevel;

        /// <summary>
        ///  Data parameter.
        /// </summary>
        public _NETLOGON_CONTROL_DATA_INFORMATION? Data;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonControl2Request()
        {
            Opnum = NrpcMethodOpnums.NetrLogonControl2;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                                new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ServerName = Marshal.PtrToStringUni(outParamList[0]);
                FunctionCode = (FunctionCode_Values)outParamList[1].ToUInt32();
                QueryLevel = (QueryLevel_Values)outParamList[2].ToUInt32();
                Data = TypeMarshal.ToNullableStruct<_NETLOGON_CONTROL_DATA_INFORMATION>(outParamList[3], FunctionCode, null, null);
            }
        }
    };


    /// <summary>
    /// The NetrLogonControl2Response class defines output parameters of method NetrLogonControl2
    /// </summary>
    public class NrpcNetrLogonControl2Response : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NetApiStatus Status;

        /// <summary>
        ///  Buffer parameter.
        /// </summary>
        public _NETLOGON_CONTROL_QUERY_INFORMATION? Buffer;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonControl2Response()
        {
            Opnum = NrpcMethodOpnums.NetrLogonControl2;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrLogonControl2Request request = sessionContext.LastRequestReceived as NrpcNetrLogonControl2Request;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pBuffer = TypeMarshal.ToIntPtr(Buffer, request.QueryLevel, null, null);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    (uint)request.QueryLevel,
                    IntPtr.Zero,
                    pBuffer,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pBuffer.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrServerAuthenticate2Request class defines input parameters of method NetrServerAuthenticate2
    /// </summary>
    public class NrpcNetrServerAuthenticate2Request : NrpcRequestStub
    {
        /// <summary>
        ///  PrimaryName parameter.
        /// </summary>
        public string PrimaryName;

        /// <summary>
        ///  AccountName parameter.
        /// </summary>
        public string AccountName;

        /// <summary>
        ///  SecureChannelType parameter.
        /// </summary>
        public _NETLOGON_SECURE_CHANNEL_TYPE SecureChannelType;

        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        ///  ClientCredential parameter.
        /// </summary>
        public _NETLOGON_CREDENTIAL? ClientCredential;

        /// <summary>
        ///  NegotiateFlags parameter.
        /// </summary>
        public uint? NegotiateFlags;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrServerAuthenticate2Request()
        {
            Opnum = NrpcMethodOpnums.NetrServerAuthenticate2;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                  RpceStubHelper.GetPlatform(),
                    NrpcRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] {
                        new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                    NrpcRpcStubFormatString.ProcFormatString,
                    NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    requestStub,
                    sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                PrimaryName = Marshal.PtrToStringUni(outParamList[0]);
                AccountName = Marshal.PtrToStringUni(outParamList[1]);
                SecureChannelType = (_NETLOGON_SECURE_CHANNEL_TYPE)(outParamList[2].ToInt32());
                ComputerName = Marshal.PtrToStringUni(outParamList[3]);
                ClientCredential = TypeMarshal.ToNullableStruct<_NETLOGON_CREDENTIAL>(outParamList[4]);
                NegotiateFlags = (uint)Marshal.ReadInt32(outParamList[6]);
            }
        }
    };


    /// <summary>
    /// The NetrServerAuthenticate2Response class defines output parameters of method
    /// NetrServerAuthenticate2
    /// </summary>
    public class NrpcNetrServerAuthenticate2Response : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NtStatus Status;

        /// <summary>
        ///  ServerCredential parameter.
        /// </summary>
        public _NETLOGON_CREDENTIAL? ServerCredential;

        /// <summary>
        ///  NegotiateFlags parameter.
        /// </summary>
        public uint? NegotiateFlags;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrServerAuthenticate2Response()
        {
            Opnum = NrpcMethodOpnums.NetrServerAuthenticate2;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrServerAuthenticate2Request request =
                sessionContext.LastRequestReceived as NrpcNetrServerAuthenticate2Request;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pServerCredential = TypeMarshal.ToIntPtr(ServerCredential);
            SafeIntPtr pNegotiateFlags = TypeMarshal.ToIntPtr(NegotiateFlags);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pServerCredential,
                    pNegotiateFlags,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pServerCredential.Dispose();
                pNegotiateFlags.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrDatabaseSync2Request class defines input parameters of method NetrDatabaseSync2
    /// </summary>
    public class NrpcNetrDatabaseSync2Request : NrpcRequestStub
    {
        /// <summary>
        ///  PrimaryName parameter.
        /// </summary>
        public string PrimaryName;

        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        ///  Authenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? Authenticator;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        ///  DatabaseID parameter.
        /// </summary>
        public DatabaseID_Values DatabaseID;

        /// <summary>
        ///  RestartState parameter.
        /// </summary>
        public _SYNC_STATE RestartState;

        /// <summary>
        ///  SyncContext parameter.
        /// </summary>
        public uint? SyncContext;

        /// <summary>
        ///  PreferredMaximumLength parameter.
        /// </summary>
        public uint PreferredMaximumLength;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrDatabaseSync2Request()
        {
            Opnum = NrpcMethodOpnums.NetrDatabaseSync2;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                PrimaryName = Marshal.PtrToStringUni(outParamList[0]);
                ComputerName = Marshal.PtrToStringUni(outParamList[1]);
                Authenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[2]);
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[3]);
                DatabaseID = (DatabaseID_Values)outParamList[4].ToUInt32();
                RestartState = (_SYNC_STATE)outParamList[5].ToInt32();
                SyncContext = outParamList[6].ToUInt32();
                PreferredMaximumLength = outParamList[8].ToUInt32();
            }
        }
    };


    /// <summary>
    /// The NetrDatabaseSync2Response class defines output parameters of method NetrDatabaseSync2
    /// </summary>
    public class NrpcNetrDatabaseSync2Response : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NtStatus Status;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        ///  SyncContext parameter.
        /// </summary>
        public uint? SyncContext;

        /// <summary>
        ///  DeltaArray parameter.
        /// </summary>
        public _NETLOGON_DELTA_ENUM_ARRAY? DeltaArray;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrDatabaseSync2Response()
        {
            Opnum = NrpcMethodOpnums.NetrDatabaseSync2;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrDatabaseSync2Request request = sessionContext.LastRequestReceived as NrpcNetrDatabaseSync2Request;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pReturnAuthenticator = TypeMarshal.ToIntPtr(ReturnAuthenticator);
            SafeIntPtr pSyncContext = TypeMarshal.ToIntPtr(SyncContext);
            SafeIntPtr pDeltaArray = TypeMarshal.ToIntPtr(DeltaArray);
            SafeIntPtr ppDeltaArray = TypeMarshal.ToIntPtr(pDeltaArray.Value);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pReturnAuthenticator,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pSyncContext,
                    ppDeltaArray,
                    IntPtr.Zero,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pReturnAuthenticator.Dispose();
                pSyncContext.Dispose();
                pDeltaArray.Dispose();
                ppDeltaArray.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrDatabaseRedoRequest class defines input parameters of method NetrDatabaseRedo
    /// </summary>
    public class NrpcNetrDatabaseRedoRequest : NrpcRequestStub
    {
        /// <summary>
        ///  PrimaryName parameter.
        /// </summary>
        public string PrimaryName;

        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        ///  Authenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? Authenticator;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        ///  ChangeLogEntry parameter.
        /// </summary>
        public byte[] ChangeLogEntry;

        /// <summary>
        ///  ChangeLogEntrySize parameter.
        /// </summary>
        public uint ChangeLogEntrySize;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrDatabaseRedoRequest()
        {
            Opnum = NrpcMethodOpnums.NetrDatabaseRedo;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                PrimaryName = Marshal.PtrToStringUni(outParamList[0]);
                ComputerName = Marshal.PtrToStringUni(outParamList[1]);
                Authenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[2]);
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[3]);
                ChangeLogEntrySize = outParamList[5].ToUInt32();
                ChangeLogEntry = IntPtrUtility.PtrToArray<byte>(outParamList[4], ChangeLogEntrySize);
            }
        }
    };


    /// <summary>
    /// The NetrDatabaseRedoResponse class defines output parameters of method NetrDatabaseRedo
    /// </summary>
    public class NrpcNetrDatabaseRedoResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NtStatus Status;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        ///  DeltaArray parameter.
        /// </summary>
        public _NETLOGON_DELTA_ENUM_ARRAY? DeltaArray;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrDatabaseRedoResponse()
        {
            Opnum = NrpcMethodOpnums.NetrDatabaseRedo;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrDatabaseRedoRequest request = sessionContext.LastRequestReceived as NrpcNetrDatabaseRedoRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pReturnAuthenticator = TypeMarshal.ToIntPtr(ReturnAuthenticator);
            SafeIntPtr pDeltaArray = TypeMarshal.ToIntPtr(DeltaArray);
            SafeIntPtr ppDeltaArray = TypeMarshal.ToIntPtr(pDeltaArray.Value);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pReturnAuthenticator,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ppDeltaArray,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pReturnAuthenticator.Dispose();
                pDeltaArray.Dispose();
                ppDeltaArray.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrLogonControl2ExRequest class defines input parameters of method NetrLogonControl2Ex
    /// </summary>
    public class NrpcNetrLogonControl2ExRequest : NrpcRequestStub
    {
        /// <summary>
        ///  ServerName parameter.
        /// </summary>
        public string ServerName;

        /// <summary>
        ///  FunctionCode parameter.
        /// </summary>
        public FunctionCode_Values FunctionCode;

        /// <summary>
        ///  QueryLevel parameter.
        /// </summary>
        public QueryLevel_Values QueryLevel;

        /// <summary>
        ///  Data parameter.
        /// </summary>
        public _NETLOGON_CONTROL_DATA_INFORMATION? Data;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonControl2ExRequest()
        {
            Opnum = NrpcMethodOpnums.NetrLogonControl2Ex;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ServerName = Marshal.PtrToStringUni(outParamList[0]);
                FunctionCode = (FunctionCode_Values)outParamList[1].ToUInt32();
                QueryLevel = (QueryLevel_Values)outParamList[2].ToUInt32();
                Data = TypeMarshal.ToNullableStruct<_NETLOGON_CONTROL_DATA_INFORMATION>(outParamList[3], FunctionCode, null, null);
            }
        }
    };


    /// <summary>
    /// The NetrLogonControl2ExResponse class defines output parameters of method NetrLogonControl2Ex
    /// </summary>
    public class NrpcNetrLogonControl2ExResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NetApiStatus Status;

        /// <summary>
        ///  Buffer parameter.
        /// </summary>
        public _NETLOGON_CONTROL_QUERY_INFORMATION? Buffer;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonControl2ExResponse()
        {
            Opnum = NrpcMethodOpnums.NetrLogonControl2Ex;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrLogonControl2ExRequest request = sessionContext.LastRequestReceived as NrpcNetrLogonControl2ExRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pBuffer = TypeMarshal.ToIntPtr(Buffer, request.QueryLevel, null, null);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    (uint)request.QueryLevel,
                    IntPtr.Zero,
                    pBuffer,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pBuffer.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrEnumerateTrustedDomainsRequest class defines input parameters 
    /// of method NetrEnumerateTrustedDomains
    /// </summary>
    public class NrpcNetrEnumerateTrustedDomainsRequest : NrpcRequestStub
    {
        /// <summary>
        ///  ServerName parameter.
        /// </summary>
        public string ServerName;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrEnumerateTrustedDomainsRequest()
        {
            Opnum = NrpcMethodOpnums.NetrEnumerateTrustedDomains;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ServerName = Marshal.PtrToStringUni(outParamList[0]);
            }
        }
    };


    /// <summary>
    /// The NetrEnumerateTrustedDomainsResponse class defines output parameters of 
    /// method NetrEnumerateTrustedDomains
    /// </summary>
    public class NrpcNetrEnumerateTrustedDomainsResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NtStatus Status;

        /// <summary>
        ///  DomainNameBuffer parameter.
        /// </summary>
        public _DOMAIN_NAME_BUFFER? DomainNameBuffer;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrEnumerateTrustedDomainsResponse()
        {
            Opnum = NrpcMethodOpnums.NetrEnumerateTrustedDomains;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrEnumerateTrustedDomainsRequest request =
                sessionContext.LastRequestReceived as NrpcNetrEnumerateTrustedDomainsRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pDomainNameBuffer = TypeMarshal.ToIntPtr(DomainNameBuffer);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    pDomainNameBuffer,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pDomainNameBuffer.Dispose();
            }
        }
    };


    /// <summary>
    /// The DsrGetDcNameRequest class defines input parameters of method DsrGetDcName
    /// </summary>
    public class NrpcDsrGetDcNameRequest : NrpcRequestStub
    {
        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        ///  DomainName parameter.
        /// </summary>
        public string DomainName;

        /// <summary>
        ///  DomainGuid parameter.
        /// </summary>
        public Guid? DomainGuid;

        /// <summary>
        ///  SiteGuid parameter.
        /// </summary>
        public Guid? SiteGuid;

        /// <summary>
        ///  Flags parameter.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcDsrGetDcNameRequest()
        {
            Opnum = NrpcMethodOpnums.DsrGetDcName;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                                new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ComputerName = Marshal.PtrToStringUni(outParamList[0]);
                DomainName = Marshal.PtrToStringUni(outParamList[1]);
                DomainGuid = TypeMarshal.ToNullableStruct<Guid>(outParamList[2]);
                SiteGuid = TypeMarshal.ToNullableStruct<Guid>(outParamList[3]);
                Flags = outParamList[4].ToUInt32();
            }
        }
    };


    /// <summary>
    /// The DsrGetDcNameResponse class defines output parameters of method DsrGetDcName
    /// </summary>
    public class NrpcDsrGetDcNameResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NetApiStatus Status;

        /// <summary>
        ///  DomainControllerInfo parameter.
        /// </summary>
        public _DOMAIN_CONTROLLER_INFOW? DomainControllerInfo;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcDsrGetDcNameResponse()
        {
            Opnum = NrpcMethodOpnums.DsrGetDcName;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcDsrGetDcNameRequest request = sessionContext.LastRequestReceived as NrpcDsrGetDcNameRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pDomainControllerInfo = TypeMarshal.ToIntPtr(DomainControllerInfo);
            SafeIntPtr ppDomainControllerInfo = TypeMarshal.ToIntPtr(pDomainControllerInfo.Value);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ppDomainControllerInfo,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pDomainControllerInfo.Dispose();
                ppDomainControllerInfo.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrLogonGetCapabilitiesRequest class defines input parameters
    /// of method NetrLogonGetCapabilities
    /// </summary>
    public class NrpcNetrLogonGetCapabilitiesRequest : NrpcRequestStub
    {
        /// <summary>
        ///  ServerName parameter.
        /// </summary>
        public string ServerName;

        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        ///  Authenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? Authenticator;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        ///  QueryLevel parameter.
        /// </summary>
        public uint QueryLevel;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonGetCapabilitiesRequest()
        {
            Opnum = NrpcMethodOpnums.NetrLogonGetCapabilities;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ServerName = Marshal.PtrToStringUni(outParamList[0]);
                ComputerName = Marshal.PtrToStringUni(outParamList[1]);
                Authenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[2]);
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[3]);
                QueryLevel = outParamList[4].ToUInt32();
            }
        }
    };


    /// <summary>
    /// The NetrLogonGetCapabilitiesResponse class defines output parameters of method NetrLogonGetCapabilities
    /// </summary>
    public class NrpcNetrLogonGetCapabilitiesResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NtStatus Status;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        ///  serverCapabilities parameter.
        /// </summary>
        public _NETLOGON_CAPABILITIES? ServerCapabilities;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonGetCapabilitiesResponse()
        {
            Opnum = NrpcMethodOpnums.NetrLogonGetCapabilities;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrLogonGetCapabilitiesRequest request =
                sessionContext.LastRequestReceived as NrpcNetrLogonGetCapabilitiesRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pReturnAuthenticator = TypeMarshal.ToIntPtr(ReturnAuthenticator);
            SafeIntPtr pServerCapabilities = TypeMarshal.ToIntPtr(ServerCapabilities, request.QueryLevel, null, null);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pReturnAuthenticator,
                    (uint)request.QueryLevel,
                    pServerCapabilities,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pReturnAuthenticator.Dispose();
                pServerCapabilities.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrLogonSetServiceBitsRequest class defines input parameters of method NetrLogonSetServiceBits
    /// </summary>
    public class NrpcNetrLogonSetServiceBitsRequest : NrpcRequestStub
    {
        /// <summary>
        ///  PrimaryName parameter.
        /// </summary>
        public string ServerName;

        /// <summary>
        ///  ServiceBitsOfInterest parameter.
        /// </summary>
        public uint ServiceBitsOfInterest;


        /// <summary>
        ///  ServiceBits parameter.
        /// </summary>
        public uint ServiceBits;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonSetServiceBitsRequest()
        {
            Opnum = NrpcMethodOpnums.NetrLogonSetServiceBits;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ServerName = Marshal.PtrToStringUni(outParamList[0]);
                ServiceBitsOfInterest = outParamList[1].ToUInt32();
                ServiceBits = outParamList[2].ToUInt32();
            }
        }
    };


    /// <summary>
    /// The NetrLogonSetServiceBitsResponse class defines output parameters of method NetrLogonSetServiceBits
    /// </summary>
    public class NrpcNetrLogonSetServiceBitsResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NtStatus Status;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonSetServiceBitsResponse()
        {
            Opnum = NrpcMethodOpnums.NetrLogonSetServiceBits;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrLogonSetServiceBitsRequest request =
                sessionContext.LastRequestReceived as NrpcNetrLogonSetServiceBitsRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            Int3264[] paramList = new Int3264[]
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                (uint)Status
            };

            return NrpcStubEncodeToBytes(paramList, Opnum);
        }
    };


    /// <summary>
    /// The NetrLogonGetTrustRidRequest class defines input parameters of method NetrLogonGetTrustRid
    /// </summary>
    public class NrpcNetrLogonGetTrustRidRequest : NrpcRequestStub
    {
        /// <summary>
        ///  ServerName parameter.
        /// </summary>
        public string ServerName;

        /// <summary>
        ///  DomainName parameter.
        /// </summary>
        public string DomainName;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonGetTrustRidRequest()
        {
            Opnum = NrpcMethodOpnums.NetrLogonGetTrustRid;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ServerName = Marshal.PtrToStringUni(outParamList[0]);
                DomainName = Marshal.PtrToStringUni(outParamList[1]);
            }
        }
    };


    /// <summary>
    /// The NetrLogonGetTrustRidResponse class defines output parameters of method NetrLogonGetTrustRid
    /// </summary>
    public class NrpcNetrLogonGetTrustRidResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NetApiStatus Status;

        /// <summary>
        ///  Rid parameter.
        /// </summary>
        public uint? Rid;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonGetTrustRidResponse()
        {
            Opnum = NrpcMethodOpnums.NetrLogonGetTrustRid;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrLogonGetTrustRidRequest request = sessionContext.LastRequestReceived as NrpcNetrLogonGetTrustRidRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pRid = TypeMarshal.ToIntPtr(Rid);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pRid,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pRid.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrLogonComputeServerDigestRequest class defines input parameters of 
    /// method NetrLogonComputeServerDigest
    /// </summary>
    public class NrpcNetrLogonComputeServerDigestRequest : NrpcRequestStub
    {
        /// <summary>
        ///  ServerName parameter.
        /// </summary>
        public string ServerName;

        /// <summary>
        ///  Rid parameter.
        /// </summary>
        public uint Rid;

        /// <summary>
        ///  Message parameter.
        /// </summary>
        public byte[] Message;

        /// <summary>
        ///  MessageSize parameter.
        /// </summary>
        public uint MessageSize;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonComputeServerDigestRequest()
        {
            Opnum = NrpcMethodOpnums.NetrLogonComputeServerDigest;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ServerName = Marshal.PtrToStringUni(outParamList[0]);
                Rid = outParamList[1].ToUInt32();
                MessageSize = outParamList[3].ToUInt32();
                Message = IntPtrUtility.PtrToArray<byte>(outParamList[2], MessageSize);
            }
        }
    };


    /// <summary>
    /// The NetrLogonComputeServerDigestResponse class defines output parameters of 
    /// method NetrLogonComputeServerDigest
    /// </summary>
    public class NrpcNetrLogonComputeServerDigestResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NetApiStatus Status;

        /// <summary>
        ///  NewMessageDigest parameter.
        /// </summary>
        public byte[] NewMessageDigest;

        /// <summary>
        ///  OldMessageDigest parameter.
        /// </summary>    
        public byte[] OldMessageDigest;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonComputeServerDigestResponse()
        {
            Opnum = NrpcMethodOpnums.NetrLogonComputeServerDigest;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrLogonComputeServerDigestRequest request =
                sessionContext.LastRequestReceived as NrpcNetrLogonComputeServerDigestRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pNewMessageDigest = IntPtrUtility.ArrayToPtr<byte>(NewMessageDigest);
            SafeIntPtr pOldMessageDigest = IntPtrUtility.ArrayToPtr<byte>(OldMessageDigest);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pNewMessageDigest,
                    pOldMessageDigest,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pNewMessageDigest.Dispose();
                pOldMessageDigest.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrLogonComputeClientDigestRequest class defines input parameters of
    /// method NetrLogonComputeClientDigest
    /// </summary>
    public class NrpcNetrLogonComputeClientDigestRequest : NrpcRequestStub
    {
        /// <summary>
        ///  ServerName parameter.
        /// </summary>
        public string ServerName;

        /// <summary>
        ///  DomainName parameter.
        /// </summary>
        public string DomainName;

        /// <summary>
        ///  Message parameter.
        /// </summary>
        public byte[] Message;

        /// <summary>
        ///  MessageSize parameter.
        /// </summary>
        public uint MessageSize;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonComputeClientDigestRequest()
        {
            Opnum = NrpcMethodOpnums.NetrLogonComputeClientDigest;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ServerName = Marshal.PtrToStringUni(outParamList[0]);
                DomainName = Marshal.PtrToStringUni(outParamList[1]);
                MessageSize = outParamList[3].ToUInt32();
                Message = IntPtrUtility.PtrToArray<byte>(outParamList[2], MessageSize);
            }
        }
    };


    /// <summary>
    /// The NetrLogonComputeClientDigestResponse class defines output parameters of
    /// method NetrLogonComputeClientDigest
    /// </summary>
    public class NrpcNetrLogonComputeClientDigestResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NetApiStatus Status;

        /// <summary>
        ///  NewMessageDigest parameter.
        /// </summary>
        public byte[] NewMessageDigest;

        /// <summary>
        ///  OldMessageDigest parameter.
        /// </summary>
        public byte[] OldMessageDigest;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonComputeClientDigestResponse()
        {
            Opnum = NrpcMethodOpnums.NetrLogonComputeClientDigest;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrLogonComputeClientDigestRequest request =
                sessionContext.LastRequestReceived as NrpcNetrLogonComputeClientDigestRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pNewMessageDigest = IntPtrUtility.ArrayToPtr<byte>(NewMessageDigest);
            SafeIntPtr pOldMessageDigest = IntPtrUtility.ArrayToPtr<byte>(OldMessageDigest);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pNewMessageDigest,
                    pOldMessageDigest,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pNewMessageDigest.Dispose();
                pOldMessageDigest.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrServerAuthenticate3Request class defines input parameters of
    /// method NetrServerAuthenticate3
    /// </summary>
    public class NrpcNetrServerAuthenticate3Request : NrpcRequestStub
    {
        /// <summary>
        ///  PrimaryName parameter.
        /// </summary>
        public string PrimaryName;

        /// <summary>
        ///  AccountName parameter.
        /// </summary>
        public string AccountName;

        /// <summary>
        ///  SecureChannelType parameter.
        /// </summary>
        public _NETLOGON_SECURE_CHANNEL_TYPE SecureChannelType;

        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        ///  ClientCredential parameter.
        /// </summary>
        public _NETLOGON_CREDENTIAL? ClientCredential;

        /// <summary>
        ///  NegotiateFlags parameter.
        /// </summary>
        public uint? NegotiateFlags;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrServerAuthenticate3Request()
        {
            Opnum = NrpcMethodOpnums.NetrServerAuthenticate3;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                PrimaryName = Marshal.PtrToStringUni(outParamList[0]);
                AccountName = Marshal.PtrToStringUni(outParamList[1]);
                SecureChannelType = (_NETLOGON_SECURE_CHANNEL_TYPE)(outParamList[2].ToInt32());
                ComputerName = Marshal.PtrToStringUni(outParamList[3]);
                ClientCredential = TypeMarshal.ToNullableStruct<_NETLOGON_CREDENTIAL>(outParamList[4]);
                NegotiateFlags = TypeMarshal.ToNullableStruct<uint>(outParamList[6]);
            }
        }
    };


    /// <summary>
    /// The NetrServerAuthenticate3Response class defines output parameters of method NetrServerAuthenticate3
    /// </summary>
    public class NrpcNetrServerAuthenticate3Response : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NtStatus Status;

        /// <summary>
        ///  ServerCredential parameter.
        /// </summary>
        public _NETLOGON_CREDENTIAL? ServerCredential;

        /// <summary>
        ///  NegotiateFlags parameter.
        /// </summary>
        public uint? NegotiateFlags;

        /// <summary>
        ///  AccountRid parameter.
        /// </summary>
        public uint? AccountRid;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrServerAuthenticate3Response()
        {
            Opnum = NrpcMethodOpnums.NetrServerAuthenticate3;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrServerAuthenticate3Request request =
                sessionContext.LastRequestReceived as NrpcNetrServerAuthenticate3Request;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pServerCredential = TypeMarshal.ToIntPtr(ServerCredential);
            SafeIntPtr pAccountRid = TypeMarshal.ToIntPtr(AccountRid);
            SafeIntPtr pNegotiateFlags = TypeMarshal.ToIntPtr(NegotiateFlags);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pServerCredential,
                    pNegotiateFlags,
                    pAccountRid,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pServerCredential.Dispose();
                pAccountRid.Dispose();
                pNegotiateFlags.Dispose();
            }
        }
    };


    /// <summary>
    /// The DsrGetDcNameExRequest class defines input parameters of method DsrGetDcNameEx
    /// </summary>
    public class NrpcDsrGetDcNameExRequest : NrpcRequestStub
    {
        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        ///  DomainName parameter.
        /// </summary>
        public string DomainName;

        /// <summary>
        ///  DomainGuid parameter.
        /// </summary>
        public Guid? DomainGuid;

        /// <summary>
        ///  SiteName parameter.
        /// </summary>
        public string SiteName;

        /// <summary>
        ///  Flags parameter.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcDsrGetDcNameExRequest()
        {
            Opnum = NrpcMethodOpnums.DsrGetDcNameEx;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ComputerName = Marshal.PtrToStringUni(outParamList[0]);
                DomainName = Marshal.PtrToStringUni(outParamList[1]);
                DomainGuid = TypeMarshal.ToNullableStruct<Guid>(outParamList[2]);
                SiteName = Marshal.PtrToStringUni(outParamList[3]);
                Flags = outParamList[4].ToUInt32();
            }
        }
    };


    /// <summary>
    /// The DsrGetDcNameExResponse class defines output parameters of method DsrGetDcNameEx
    /// </summary>
    public class NrpcDsrGetDcNameExResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NetApiStatus Status;

        /// <summary>
        ///  DomainControllerInfo parameter.
        /// </summary>
        public _DOMAIN_CONTROLLER_INFOW? DomainControllerInfo;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcDsrGetDcNameExResponse()
        {
            Opnum = NrpcMethodOpnums.DsrGetDcNameEx;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcDsrGetDcNameExRequest request = sessionContext.LastRequestReceived as NrpcDsrGetDcNameExRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pDomainControllerInfo = TypeMarshal.ToIntPtr(DomainControllerInfo);
            SafeIntPtr ppDomainControllerInfo = TypeMarshal.ToIntPtr(pDomainControllerInfo.Value);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ppDomainControllerInfo,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pDomainControllerInfo.Dispose();
                ppDomainControllerInfo.Dispose();
            }
        }
    };

    /// <summary>
    /// The DsrGetSiteNameRequest class defines input parameters of method DsrGetSiteName
    /// </summary>
    public class NrpcDsrGetSiteNameRequest : NrpcRequestStub
    {
        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcDsrGetSiteNameRequest()
        {
            Opnum = NrpcMethodOpnums.DsrGetSiteName;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ComputerName = Marshal.PtrToStringUni(outParamList[0]);
            }
        }
    };


    /// <summary>
    /// The DsrGetSiteNameResponse class defines output parameters of method DsrGetSiteName
    /// </summary>
    public class NrpcDsrGetSiteNameResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NetApiStatus Status;

        /// <summary>
        ///  SiteName parameter.
        /// </summary>
        public string SiteName;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcDsrGetSiteNameResponse()
        {
            Opnum = NrpcMethodOpnums.DsrGetSiteName;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcDsrGetSiteNameRequest request = sessionContext.LastRequestReceived as NrpcDsrGetSiteNameRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pSiteName = Marshal.StringToHGlobalUni(SiteName);
            SafeIntPtr ppSiteName = TypeMarshal.ToIntPtr(pSiteName.Value);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    ppSiteName,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pSiteName.Dispose();
                ppSiteName.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrLogonGetDomainInfoRequest class defines input parameters of method NetrLogonGetDomainInfo
    /// </summary>
    public class NrpcNetrLogonGetDomainInfoRequest : NrpcRequestStub
    {
        /// <summary>
        ///  ServerName parameter.
        /// </summary>
        public string ServerName;

        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        ///  Authenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? Authenticator;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        ///  Level parameter.
        /// </summary>
        public Level_Values Level;

        /// <summary>
        ///  WkstaBuffer parameter.
        /// </summary>
        public _NETLOGON_WORKSTATION_INFORMATION? WkstaBuffer;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonGetDomainInfoRequest()
        {
            Opnum = NrpcMethodOpnums.NetrLogonGetDomainInfo;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ServerName = Marshal.PtrToStringUni(outParamList[0]);
                ComputerName = Marshal.PtrToStringUni(outParamList[1]);
                Authenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[2]);
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[3]);
                Level = (Level_Values)outParamList[4].ToUInt32();
                WkstaBuffer = TypeMarshal.ToNullableStruct<_NETLOGON_WORKSTATION_INFORMATION>(outParamList[5], Level, null, null);
            }
        }
    };


    /// <summary>
    /// The NetrLogonGetDomainInfoResponse class defines output parameters of method NetrLogonGetDomainInfo
    /// </summary>
    public class NrpcNetrLogonGetDomainInfoResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NtStatus Status;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        ///  DomBuffer parameter.
        /// </summary>
        public _NETLOGON_DOMAIN_INFORMATION? DomBuffer;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonGetDomainInfoResponse()
        {
            Opnum = NrpcMethodOpnums.NetrLogonGetDomainInfo;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrLogonGetDomainInfoRequest request =
                sessionContext.LastRequestReceived as NrpcNetrLogonGetDomainInfoRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pReturnAuthenticator = TypeMarshal.ToIntPtr(ReturnAuthenticator);
            SafeIntPtr pDomBuffer = TypeMarshal.ToIntPtr(DomBuffer, request.Level, null, null);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pReturnAuthenticator,
                    (uint)request.Level,
                    IntPtr.Zero,
                    pDomBuffer,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pReturnAuthenticator.Dispose();
                pDomBuffer.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrServerPasswordSet2Request class defines input parameters of method NetrServerPasswordSet2
    /// </summary>
    public class NrpcNetrServerPasswordSet2Request : NrpcRequestStub
    {
        /// <summary>
        ///  PrimaryName parameter.
        /// </summary>
        public string PrimaryName;

        /// <summary>
        ///  AccountName parameter.
        /// </summary>
        public string AccountName;

        /// <summary>
        ///  SecureChannelType parameter.
        /// </summary>
        public _NETLOGON_SECURE_CHANNEL_TYPE SecureChannelType;

        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        ///  Authenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? Authenticator;

        /// <summary>
        ///  ClearNewPassword parameter.
        /// </summary>
        public _NL_TRUST_PASSWORD? ClearNewPassword;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrServerPasswordSet2Request()
        {
            Opnum = NrpcMethodOpnums.NetrServerPasswordSet2;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                PrimaryName = Marshal.PtrToStringUni(outParamList[0]);
                AccountName = Marshal.PtrToStringUni(outParamList[1]);
                SecureChannelType = (_NETLOGON_SECURE_CHANNEL_TYPE)outParamList[2].ToInt32();
                ComputerName = Marshal.PtrToStringUni(outParamList[3]);
                Authenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[4]);
                ClearNewPassword = TypeMarshal.ToNullableStruct<_NL_TRUST_PASSWORD>(outParamList[6]);
            }
        }
    };


    /// <summary>
    /// The NetrServerPasswordSet2Response class defines output parameters of method NetrServerPasswordSet2
    /// </summary>
    public class NrpcNetrServerPasswordSet2Response : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NtStatus Status;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrServerPasswordSet2Response()
        {
            Opnum = NrpcMethodOpnums.NetrServerPasswordSet2;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrServerPasswordSet2Request request =
                sessionContext.LastRequestReceived as NrpcNetrServerPasswordSet2Request;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pReturnAuthenticator = TypeMarshal.ToIntPtr(ReturnAuthenticator);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pReturnAuthenticator,
                    IntPtr.Zero,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pReturnAuthenticator.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrServerPasswordGetRequest class defines input parameters of method NetrServerPasswordGet
    /// </summary>
    public class NrpcNetrServerPasswordGetRequest : NrpcRequestStub
    {
        /// <summary>
        ///  PrimaryName parameter.
        /// </summary>
        public string PrimaryName;

        /// <summary>
        ///  AccountName parameter.
        /// </summary>
        public string AccountName;

        /// <summary>
        ///  AccountType parameter.
        /// </summary>
        public _NETLOGON_SECURE_CHANNEL_TYPE AccountType;

        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        ///  Authenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? Authenticator;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrServerPasswordGetRequest()
        {
            Opnum = NrpcMethodOpnums.NetrServerPasswordGet;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                PrimaryName = Marshal.PtrToStringUni(outParamList[0]);
                AccountName = Marshal.PtrToStringUni(outParamList[1]);
                AccountType = (_NETLOGON_SECURE_CHANNEL_TYPE)outParamList[2].ToUInt32();
                ComputerName = Marshal.PtrToStringUni(outParamList[3]);
                Authenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[4]);
            }
        }
    };


    /// <summary>
    /// The NetrServerPasswordGetResponse class defines output parameters of method NetrServerPasswordGet
    /// </summary>
    public class NrpcNetrServerPasswordGetResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NtStatus Status;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        ///  EncryptedNtOwfPassword parameter.
        /// </summary>
        public _NT_OWF_PASSWORD? EncryptedNtOwfPassword;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrServerPasswordGetResponse()
        {
            Opnum = NrpcMethodOpnums.NetrServerPasswordGet;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrServerPasswordGetRequest request =
                sessionContext.LastRequestReceived as NrpcNetrServerPasswordGetRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pReturnAuthenticator = TypeMarshal.ToIntPtr(ReturnAuthenticator);
            SafeIntPtr pEncryptedNtOwfPassword = TypeMarshal.ToIntPtr(EncryptedNtOwfPassword);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pReturnAuthenticator,
                    pEncryptedNtOwfPassword,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pReturnAuthenticator.Dispose();
                pEncryptedNtOwfPassword.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrLogonSendToSamRequest class defines input parameters of method NetrLogonSendToSam
    /// </summary>
    public class NrpcNetrLogonSendToSamRequest : NrpcRequestStub
    {
        /// <summary>
        ///  PrimaryName parameter.
        /// </summary>
        public string PrimaryName;

        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        ///  Authenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? Authenticator;

        /// <summary>
        ///  OpaqueBuffer parameter.
        /// </summary>
        public byte[] OpaqueBuffer;

        /// <summary>
        ///  OpaqueBufferSize parameter.
        /// </summary>
        public uint OpaqueBufferSize;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonSendToSamRequest()
        {
            Opnum = NrpcMethodOpnums.NetrLogonSendToSam;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                PrimaryName = Marshal.PtrToStringUni(outParamList[0]);
                ComputerName = Marshal.PtrToStringUni(outParamList[1]);
                Authenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[2]);
                OpaqueBufferSize = outParamList[5].ToUInt32();
                OpaqueBuffer = IntPtrUtility.PtrToArray<byte>(outParamList[4], OpaqueBufferSize);
            }
        }
    };


    /// <summary>
    /// The NetrLogonSendToSamResponse class defines output parameters of method NetrLogonSendToSam
    /// </summary>
    public class NrpcNetrLogonSendToSamResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NtStatus Status;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonSendToSamResponse()
        {
            Opnum = NrpcMethodOpnums.NetrLogonSendToSam;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrLogonSendToSamRequest request = sessionContext.LastRequestReceived as NrpcNetrLogonSendToSamRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pReturnAuthenticator = TypeMarshal.ToIntPtr(ReturnAuthenticator);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pReturnAuthenticator,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pReturnAuthenticator.Dispose();
            }
        }
    };


    /// <summary>
    /// The DsrAddressToSiteNamesWRequest class defines input parameters of method DsrAddressToSiteNamesW
    /// </summary>
    public class NrpcDsrAddressToSiteNamesWRequest : NrpcRequestStub
    {
        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        ///  EntryCount parameter.
        /// </summary>
        public uint EntryCount;

        /// <summary>
        ///  SocketAddresses parameter.
        /// </summary>
        public _NL_SOCKET_ADDRESS[] SocketAddresses;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcDsrAddressToSiteNamesWRequest()
        {
            Opnum = NrpcMethodOpnums.DsrAddressToSiteNamesW;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ComputerName = Marshal.PtrToStringUni(outParamList[0]);
                EntryCount = outParamList[1].ToUInt32();
                IntPtr ppSocketAddresses = Marshal.AllocHGlobal(IntPtr.Size);
                Marshal.WriteIntPtr(ppSocketAddresses, outParamList[2]);
                SocketAddresses = TypeMarshal.ToArray<_NL_SOCKET_ADDRESS>(ppSocketAddresses, (int)EntryCount);
                Marshal.FreeHGlobal(ppSocketAddresses);
            }
        }
    };


    /// <summary>
    /// The DsrAddressToSiteNamesWResponse class defines output parameters of method DsrAddressToSiteNamesW
    /// </summary>
    public class NrpcDsrAddressToSiteNamesWResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NetApiStatus Status;

        /// <summary>
        ///  SiteNames parameter.
        /// </summary>
        public _NL_SITE_NAME_ARRAY? SiteNames;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcDsrAddressToSiteNamesWResponse()
        {
            Opnum = NrpcMethodOpnums.DsrAddressToSiteNamesW;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcDsrAddressToSiteNamesWRequest request =
                sessionContext.LastRequestReceived as NrpcDsrAddressToSiteNamesWRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pSiteNames = TypeMarshal.ToIntPtr(SiteNames);
            SafeIntPtr ppSiteNames = TypeMarshal.ToIntPtr(pSiteNames.Value);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ppSiteNames,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pSiteNames.Dispose();
                ppSiteNames.Dispose();
            }
        }
    };


    /// <summary>
    /// The DsrGetDcNameEx2Request class defines input parameters of method DsrGetDcNameEx2
    /// </summary>
    public class NrpcDsrGetDcNameEx2Request : NrpcRequestStub
    {
        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        ///  AccountName parameter.
        /// </summary>
        public string AccountName;

        /// <summary>
        ///  AllowableAccountControlBits parameter.
        /// </summary>
        public uint AllowableAccountControlBits;

        /// <summary>
        ///  DomainName parameter.
        /// </summary>
        public string DomainName;

        /// <summary>
        ///  DomainGuid parameter.
        /// </summary>
        public Guid? DomainGuid;

        /// <summary>
        ///  SiteName parameter.
        /// </summary>
        public string SiteName;

        /// <summary>
        ///  Flags parameter.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcDsrGetDcNameEx2Request()
        {
            Opnum = NrpcMethodOpnums.DsrGetDcNameEx2;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ComputerName = Marshal.PtrToStringUni(outParamList[0]);
                AccountName = Marshal.PtrToStringUni(outParamList[1]);
                AllowableAccountControlBits = outParamList[2].ToUInt32();
                DomainName = Marshal.PtrToStringUni(outParamList[3]);
                DomainGuid = TypeMarshal.ToNullableStruct<Guid>(outParamList[4]);
                SiteName = Marshal.PtrToStringUni(outParamList[5]);
                Flags = outParamList[6].ToUInt32();
            }
        }
    };


    /// <summary>
    /// The DsrGetDcNameEx2Response class defines output parameters of method DsrGetDcNameEx2
    /// </summary>
    public class NrpcDsrGetDcNameEx2Response : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NetApiStatus Status;

        /// <summary>
        ///  DomainControllerInfo parameter.
        /// </summary>
        public _DOMAIN_CONTROLLER_INFOW? DomainControllerInfo;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcDsrGetDcNameEx2Response()
        {
            Opnum = NrpcMethodOpnums.DsrGetDcNameEx2;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcDsrGetDcNameEx2Request request = sessionContext.LastRequestReceived as NrpcDsrGetDcNameEx2Request;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pDomainControllerInfo = TypeMarshal.ToIntPtr(DomainControllerInfo);
            SafeIntPtr ppDomainControllerInfo = TypeMarshal.ToIntPtr(pDomainControllerInfo.Value);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ppDomainControllerInfo,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pDomainControllerInfo.Dispose();
                ppDomainControllerInfo.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrLogonGetTimeServiceParentDomainRequest class defines input parameters of
    /// method NetrLogonGetTimeServiceParentDomain
    /// </summary>
    public class NrpcNetrLogonGetTimeServiceParentDomainRequest : NrpcRequestStub
    {
        /// <summary>
        ///  ServerName parameter.
        /// </summary>
        public string ServerName;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonGetTimeServiceParentDomainRequest()
        {
            Opnum = NrpcMethodOpnums.NetrLogonGetTimeServiceParentDomain;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ServerName = Marshal.PtrToStringUni(outParamList[0]);
            }
        }
    };


    /// <summary>
    /// The NetrLogonGetTimeServiceParentDomainResponse class defines output parameters of
    /// method NetrLogonGetTimeServiceParentDomain
    /// </summary>
    public class NrpcNetrLogonGetTimeServiceParentDomainResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NetApiStatus Status;

        /// <summary>
        ///  DomainName parameter.
        /// </summary>
        public string DomainName;

        /// <summary>
        ///  PdcSameSite parameter.
        /// </summary>
        public PdcSameSite_Values? PdcSameSite;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonGetTimeServiceParentDomainResponse()
        {
            Opnum = NrpcMethodOpnums.NetrLogonGetTimeServiceParentDomain;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrLogonGetTimeServiceParentDomainRequest request =
                sessionContext.LastRequestReceived as NrpcNetrLogonGetTimeServiceParentDomainRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pDomainName = Marshal.StringToHGlobalUni(DomainName);
            SafeIntPtr ppDomainName = TypeMarshal.ToIntPtr(pDomainName.Value);
            SafeIntPtr pPdcSameSite = TypeMarshal.ToIntPtr(PdcSameSite);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    ppDomainName,
                    pPdcSameSite,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pDomainName.Dispose();
                ppDomainName.Dispose();
                pPdcSameSite.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrEnumerateTrustedDomainsExRequest class defines input parameters of
    /// method NetrEnumerateTrustedDomainsEx
    /// </summary>
    public class NrpcNetrEnumerateTrustedDomainsExRequest : NrpcRequestStub
    {
        /// <summary>
        ///  ServerName parameter.
        /// </summary>
        public string ServerName;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrEnumerateTrustedDomainsExRequest()
        {
            Opnum = NrpcMethodOpnums.NetrEnumerateTrustedDomainsEx;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ServerName = Marshal.PtrToStringUni(outParamList[0]);
            }
        }
    };


    /// <summary>
    /// The NetrEnumerateTrustedDomainsExResponse class defines output parameters of
    /// method NetrEnumerateTrustedDomainsEx
    /// </summary>
    public class NrpcNetrEnumerateTrustedDomainsExResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NetApiStatus Status;

        /// <summary>
        ///  Domains parameter.
        /// </summary>
        public _NETLOGON_TRUSTED_DOMAIN_ARRAY? Domains;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrEnumerateTrustedDomainsExResponse()
        {
            Opnum = NrpcMethodOpnums.NetrEnumerateTrustedDomainsEx;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrEnumerateTrustedDomainsExRequest request =
                sessionContext.LastRequestReceived as NrpcNetrEnumerateTrustedDomainsExRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pDomains = TypeMarshal.ToIntPtr(Domains);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    pDomains,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pDomains.Dispose();
            }
        }
    };


    /// <summary>
    /// The DsrAddressToSiteNamesExWRequest class defines input parameters of method DsrAddressToSiteNamesExW
    /// </summary>
    public class NrpcDsrAddressToSiteNamesExWRequest : NrpcRequestStub
    {
        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        ///  EntryCount parameter.
        /// </summary>
        public uint EntryCount;

        /// <summary>
        ///  SocketAddresses parameter.
        /// </summary>
        public _NL_SOCKET_ADDRESS[] SocketAddresses;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcDsrAddressToSiteNamesExWRequest()
        {
            Opnum = NrpcMethodOpnums.DsrAddressToSiteNamesExW;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ComputerName = Marshal.PtrToStringUni(outParamList[0]);
                EntryCount = outParamList[1].ToUInt32();
                IntPtr ppSocketAddresses = Marshal.AllocHGlobal(IntPtr.Size);
                Marshal.WriteIntPtr(ppSocketAddresses, outParamList[2]);
                SocketAddresses = TypeMarshal.ToArray<_NL_SOCKET_ADDRESS>(ppSocketAddresses, (int)EntryCount);
                Marshal.FreeHGlobal(ppSocketAddresses);
            }
        }
    };


    /// <summary>
    /// The DsrAddressToSiteNamesExWResponse class defines output parameters of method DsrAddressToSiteNamesExW
    /// </summary>
    public class NrpcDsrAddressToSiteNamesExWResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NetApiStatus Status;

        /// <summary>
        ///  SiteNames parameter.
        /// </summary>
        public _NL_SITE_NAME_EX_ARRAY? SiteNames;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcDsrAddressToSiteNamesExWResponse()
        {
            Opnum = NrpcMethodOpnums.DsrAddressToSiteNamesExW;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcDsrAddressToSiteNamesExWRequest request =
                sessionContext.LastRequestReceived as NrpcDsrAddressToSiteNamesExWRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pSiteNames = TypeMarshal.ToIntPtr(SiteNames);
            SafeIntPtr ppSiteNames = TypeMarshal.ToIntPtr(pSiteNames.Value);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ppSiteNames,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pSiteNames.Dispose();
                ppSiteNames.Dispose();
            }
        }
    };


    /// <summary>
    /// The DsrGetDcSiteCoverageWRequest class defines input parameters of method DsrGetDcSiteCoverageW
    /// </summary>
    public class NrpcDsrGetDcSiteCoverageWRequest : NrpcRequestStub
    {
        /// <summary>
        ///  ServerName parameter.
        /// </summary>
        public string ServerName;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcDsrGetDcSiteCoverageWRequest()
        {
            Opnum = NrpcMethodOpnums.DsrGetDcSiteCoverageW;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ServerName = Marshal.PtrToStringUni(outParamList[0]);
            }
        }
    };


    /// <summary>
    /// The DsrGetDcSiteCoverageWResponse class defines output parameters of method DsrGetDcSiteCoverageW
    /// </summary>
    public class NrpcDsrGetDcSiteCoverageWResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NetApiStatus Status;

        /// <summary>
        ///  SiteNames parameter.
        /// </summary>
        public _NL_SITE_NAME_ARRAY? SiteNames;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcDsrGetDcSiteCoverageWResponse()
        {
            Opnum = NrpcMethodOpnums.DsrGetDcSiteCoverageW;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcDsrGetDcSiteCoverageWRequest request =
                sessionContext.LastRequestReceived as NrpcDsrGetDcSiteCoverageWRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pSiteNames = TypeMarshal.ToIntPtr(SiteNames);
            SafeIntPtr ppSiteNames = TypeMarshal.ToIntPtr(pSiteNames.Value);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    ppSiteNames,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pSiteNames.Dispose();
                ppSiteNames.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrLogonSamLogonExRequest class defines input parameters of method NetrLogonSamLogonEx
    /// </summary>
    public class NrpcNetrLogonSamLogonExRequest : NrpcRequestStub
    {
        /// <summary>
        ///  ContextHandle parameter.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr ContextHandle;

        /// <summary>
        ///  LogonServer parameter.
        /// </summary>
        public string LogonServer;

        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        ///  LogonLevel parameter.
        /// </summary>
        public _NETLOGON_LOGON_INFO_CLASS LogonLevel;

        /// <summary>
        ///  LogonInformation parameter.
        /// </summary>
        public _NETLOGON_LEVEL? LogonInformation;

        /// <summary>
        ///  ValidationLevel parameter.
        /// </summary>
        public _NETLOGON_VALIDATION_INFO_CLASS ValidationLevel;

        /// <summary>
        ///  ExtraFlags parameter.
        /// </summary>
        public uint? ExtraFlags;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonSamLogonExRequest()
        {
            Opnum = NrpcMethodOpnums.NetrLogonSamLogonEx;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ContextHandle = sessionContext.RpceLayerSessionContext.Handle;
                LogonServer = Marshal.PtrToStringUni(outParamList[0]);
                ComputerName = Marshal.PtrToStringUni(outParamList[1]);
                LogonLevel = (_NETLOGON_LOGON_INFO_CLASS)outParamList[2].ToInt32();
                LogonInformation = TypeMarshal.ToNullableStruct<_NETLOGON_LEVEL>(outParamList[3], LogonLevel, null, null);
                ValidationLevel = (_NETLOGON_VALIDATION_INFO_CLASS)outParamList[4].ToInt32();
                ExtraFlags = TypeMarshal.ToNullableStruct<uint>(outParamList[7]);
            }
        }
    };


    /// <summary>
    /// The NetrLogonSamLogonExResponse class defines output parameters of method NetrLogonSamLogonEx
    /// </summary>
    public class NrpcNetrLogonSamLogonExResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NtStatus Status;

        /// <summary>
        ///  ValidationInformation parameter.
        /// </summary>
        public _NETLOGON_VALIDATION ValidationInformation;

        /// <summary>
        ///  Authoritative parameter.
        /// </summary>
        public byte? Authoritative;

        /// <summary>
        ///  ExtraFlags parameter.
        /// </summary>
        public uint? ExtraFlags;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonSamLogonExResponse()
        {
            Opnum = NrpcMethodOpnums.NetrLogonSamLogonEx;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrLogonSamLogonExRequest request =
                sessionContext.LastRequestReceived as NrpcNetrLogonSamLogonExRequest;

            if (request == null)
            {
                throw new InvalidOperationException(
                    "The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pValidationInformation = TypeMarshal.ToIntPtr(ValidationInformation,
                request.ValidationLevel, null, null);
            SafeIntPtr pAuthoritative = TypeMarshal.ToIntPtr(Authoritative);
            SafeIntPtr pExtraFlags = TypeMarshal.ToIntPtr(ExtraFlags);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    (uint)request.ValidationLevel,
                    pValidationInformation,
                    pAuthoritative,
                    pExtraFlags,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pValidationInformation.Dispose();
                pExtraFlags.Dispose();
                pAuthoritative.Dispose();
            }
        }
    };


    /// <summary>
    /// The DsrEnumerateDomainTrustsRequest class defines input parameters of method DsrEnumerateDomainTrusts
    /// </summary>
    public class NrpcDsrEnumerateDomainTrustsRequest : NrpcRequestStub
    {
        /// <summary>
        ///  ServerName parameter.
        /// </summary>
        public string ServerName;

        /// <summary>
        ///  Flags parameter.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcDsrEnumerateDomainTrustsRequest()
        {
            Opnum = NrpcMethodOpnums.DsrEnumerateDomainTrusts;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ServerName = Marshal.PtrToStringUni(outParamList[0]);
                Flags = outParamList[1].ToUInt32();
            }
        }
    };


    /// <summary>
    /// The DsrEnumerateDomainTrustsResponse class defines output parameters of method DsrEnumerateDomainTrusts
    /// </summary>
    public class NrpcDsrEnumerateDomainTrustsResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NetApiStatus Status;

        /// <summary>
        ///  Domains parameter.
        /// </summary>
        public _NETLOGON_TRUSTED_DOMAIN_ARRAY? Domains;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcDsrEnumerateDomainTrustsResponse()
        {
            Opnum = NrpcMethodOpnums.DsrEnumerateDomainTrusts;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcDsrEnumerateDomainTrustsRequest request =
                sessionContext.LastRequestReceived as NrpcDsrEnumerateDomainTrustsRequest;

            if (request == null)
            {
                throw new InvalidOperationException(
                    "The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pDomains = TypeMarshal.ToIntPtr(Domains);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pDomains,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pDomains.Dispose();
            }
        }
    };


    /// <summary>
    /// The DsrDeregisterDnsHostRecordsRequest class defines input parameters of
    /// method DsrDeregisterDnsHostRecords
    /// </summary>
    public class NrpcDsrDeregisterDnsHostRecordsRequest : NrpcRequestStub
    {
        /// <summary>
        ///  ServerName parameter.
        /// </summary>
        public string ServerName;

        /// <summary>
        ///  DnsDomainName parameter.
        /// </summary>
        public string DnsDomainName;

        /// <summary>
        ///  DomainGuid parameter.
        /// </summary>
        public Guid? DomainGuid;

        /// <summary>
        ///  DsaGuid parameter.
        /// </summary>
        public Guid? DsaGuid;

        /// <summary>
        ///  DnsHostName parameter.
        /// </summary>
        public string DnsHostName;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcDsrDeregisterDnsHostRecordsRequest()
        {
            Opnum = NrpcMethodOpnums.DsrDeregisterDnsHostRecords;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ServerName = Marshal.PtrToStringUni(outParamList[0]);
                DnsDomainName = Marshal.PtrToStringUni(outParamList[1]);
                DomainGuid = TypeMarshal.ToNullableStruct<Guid>(outParamList[2]);
                DsaGuid = TypeMarshal.ToNullableStruct<Guid>(outParamList[3]);
                DnsHostName = Marshal.PtrToStringUni(outParamList[4]);
            }
        }
    };


    /// <summary>
    /// The DsrDeregisterDnsHostRecordsResponse class defines output parameters 
    /// of method DsrDeregisterDnsHostRecords
    /// </summary>
    public class NrpcDsrDeregisterDnsHostRecordsResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NetApiStatus Status;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcDsrDeregisterDnsHostRecordsResponse()
        {
            Opnum = NrpcMethodOpnums.DsrDeregisterDnsHostRecords;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcDsrDeregisterDnsHostRecordsRequest request =
                sessionContext.LastRequestReceived as NrpcDsrDeregisterDnsHostRecordsRequest;

            if (request == null)
            {
                throw new InvalidOperationException(
                    "The corresponding request isn't found, unable to create the response");
            }

            Int3264[] paramList = new Int3264[]
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                (uint)Status
            };

            return NrpcStubEncodeToBytes(paramList, Opnum);
        }
    };


    /// <summary>
    /// The NetrServerTrustPasswordsGetRequest class defines input parameters of method NetrServerTrustPasswordsGet
    /// </summary>
    public class NrpcNetrServerTrustPasswordsGetRequest : NrpcRequestStub
    {
        /// <summary>
        ///  TrustedDcName parameter.
        /// </summary>
        public string TrustedDcName;

        /// <summary>
        ///  AccountName parameter.
        /// </summary>
        public string AccountName;

        /// <summary>
        ///  SecureChannelType parameter.
        /// </summary>
        public _NETLOGON_SECURE_CHANNEL_TYPE SecureChannelType;

        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        ///  Authenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? Authenticator;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrServerTrustPasswordsGetRequest()
        {
            Opnum = NrpcMethodOpnums.NetrServerTrustPasswordsGet;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                TrustedDcName = Marshal.PtrToStringUni(outParamList[0]);
                AccountName = Marshal.PtrToStringUni(outParamList[1]);
                SecureChannelType = (_NETLOGON_SECURE_CHANNEL_TYPE)outParamList[2].ToUInt32();
                ComputerName = Marshal.PtrToStringUni(outParamList[3]);
                Authenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[4]);
            }
        }
    };


    /// <summary>
    /// The NetrServerTrustPasswordsGetResponse class defines output parameters of method NetrServerTrustPasswordsGet
    /// </summary>
    public class NrpcNetrServerTrustPasswordsGetResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NtStatus Status;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        ///  EncryptedNewOwfPassword parameter.
        /// </summary>
        public _NT_OWF_PASSWORD? EncryptedNewOwfPassword;

        /// <summary>
        ///  EncryptedOldOwfPassword parameter.
        /// </summary>
        public _NT_OWF_PASSWORD? EncryptedOldOwfPassword;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrServerTrustPasswordsGetResponse()
        {
            Opnum = NrpcMethodOpnums.NetrServerTrustPasswordsGet;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrServerTrustPasswordsGetRequest request =
                sessionContext.LastRequestReceived as NrpcNetrServerTrustPasswordsGetRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pReturnAuthenticator = TypeMarshal.ToIntPtr(ReturnAuthenticator);
            SafeIntPtr pEncryptedNewOwfPassword = TypeMarshal.ToIntPtr(EncryptedNewOwfPassword);
            SafeIntPtr pEncryptedOldOwfPassword = TypeMarshal.ToIntPtr(EncryptedOldOwfPassword);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pReturnAuthenticator,
                    pEncryptedNewOwfPassword,
                    pEncryptedOldOwfPassword,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pReturnAuthenticator.Dispose();
                pEncryptedNewOwfPassword.Dispose();
                pEncryptedOldOwfPassword.Dispose();
            }
        }
    };


    /// <summary>
    /// The DsrGetForestTrustInformationRequest class defines input parameters of
    /// method DsrGetForestTrustInformation
    /// </summary>
    public class NrpcDsrGetForestTrustInformationRequest : NrpcRequestStub
    {
        /// <summary>
        ///  ServerName parameter.
        /// </summary>
        public string ServerName;

        /// <summary>
        ///  TrustedDomainName parameter.
        /// </summary>
        public string TrustedDomainName;

        /// <summary>
        ///  Flags parameter.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcDsrGetForestTrustInformationRequest()
        {
            Opnum = NrpcMethodOpnums.DsrGetForestTrustInformation;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ServerName = Marshal.PtrToStringUni(outParamList[0]);
                TrustedDomainName = Marshal.PtrToStringUni(outParamList[1]);
                Flags = outParamList[2].ToUInt32();
            }
        }
    };


    /// <summary>
    /// The DsrGetForestTrustInformationResponse class defines output parameters of
    /// method DsrGetForestTrustInformation
    /// </summary>
    public class NrpcDsrGetForestTrustInformationResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NetApiStatus Status;

        /// <summary>
        ///  ForestTrustInfo parameter.
        /// </summary>
        public _LSA_FOREST_TRUST_INFORMATION? ForestTrustInfo;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcDsrGetForestTrustInformationResponse()
        {
            Opnum = NrpcMethodOpnums.DsrGetForestTrustInformation;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcDsrGetForestTrustInformationRequest request =
                sessionContext.LastRequestReceived as NrpcDsrGetForestTrustInformationRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pForestTrustInfo = TypeMarshal.ToIntPtr(ForestTrustInfo);
            SafeIntPtr ppForestTrustInfo = TypeMarshal.ToIntPtr(pForestTrustInfo.Value);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ppForestTrustInfo,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                ppForestTrustInfo.Dispose();
                pForestTrustInfo.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrGetForestTrustInformationRequest class defines input parameters of
    /// method NetrGetForestTrustInformation
    /// </summary>
    public class NrpcNetrGetForestTrustInformationRequest : NrpcRequestStub
    {
        /// <summary>
        ///  ServerName parameter.
        /// </summary>
        public string ServerName;

        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        ///  Authenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? Authenticator;

        /// <summary>
        ///  Flags parameter.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrGetForestTrustInformationRequest()
        {
            Opnum = NrpcMethodOpnums.NetrGetForestTrustInformation;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ServerName = Marshal.PtrToStringUni(outParamList[0]);
                ComputerName = Marshal.PtrToStringUni(outParamList[1]);
                Authenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[2]);
                Flags = outParamList[4].ToUInt32();
            }
        }
    };


    /// <summary>
    /// The NetrGetForestTrustInformationResponse class defines output parameters of
    /// method NetrGetForestTrustInformation
    /// </summary>
    public class NrpcNetrGetForestTrustInformationResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NtStatus Status;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        ///  ForestTrustInfo parameter.
        /// </summary>
        public _LSA_FOREST_TRUST_INFORMATION? ForestTrustInfo;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrGetForestTrustInformationResponse()
        {
            Opnum = NrpcMethodOpnums.NetrGetForestTrustInformation;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrGetForestTrustInformationRequest request = sessionContext.LastRequestReceived as NrpcNetrGetForestTrustInformationRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pReturnAuthenticator = TypeMarshal.ToIntPtr(ReturnAuthenticator);
            SafeIntPtr pForestTrustInfo = TypeMarshal.ToIntPtr(ForestTrustInfo);
            SafeIntPtr ppForestTrustInfo = TypeMarshal.ToIntPtr(pForestTrustInfo.Value);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pReturnAuthenticator,
                    IntPtr.Zero,
                    ppForestTrustInfo,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pReturnAuthenticator.Dispose();
                pForestTrustInfo.Dispose();
                ppForestTrustInfo.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrLogonSamLogonWithFlagsRequest class defines input parameters of method NetrLogonSamLogonWithFlags
    /// </summary>
    public class NrpcNetrLogonSamLogonWithFlagsRequest : NrpcRequestStub
    {
        /// <summary>
        ///  LogonServer parameter.
        /// </summary>
        public string LogonServer;

        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        ///  Authenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? Authenticator;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        ///  LogonLevel parameter.
        /// </summary>
        public _NETLOGON_LOGON_INFO_CLASS LogonLevel;

        /// <summary>
        ///  LogonInformation parameter.
        /// </summary>
        public _NETLOGON_LEVEL? LogonInformation;

        /// <summary>
        ///  ValidationLevel parameter.
        /// </summary>
        public _NETLOGON_VALIDATION_INFO_CLASS ValidationLevel;

        /// <summary>
        ///  ExtraFlags parameter.
        /// </summary>
        public uint? ExtraFlags;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonSamLogonWithFlagsRequest()
        {
            Opnum = NrpcMethodOpnums.NetrLogonSamLogonWithFlags;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                LogonServer = Marshal.PtrToStringUni(outParamList[0]);
                ComputerName = Marshal.PtrToStringUni(outParamList[1]);
                Authenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[2]);
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[3]);
                LogonLevel = (_NETLOGON_LOGON_INFO_CLASS)outParamList[4].ToInt32();
                LogonInformation = TypeMarshal.ToNullableStruct<_NETLOGON_LEVEL>(outParamList[5], LogonLevel, null, null);
                ValidationLevel = (_NETLOGON_VALIDATION_INFO_CLASS)outParamList[6].ToInt32();
                ExtraFlags = TypeMarshal.ToNullableStruct<uint>(outParamList[9]);
            }
        }
    };


    /// <summary>
    /// The NetrLogonSamLogonWithFlagsResponse class defines output parameters
    /// of method NetrLogonSamLogonWithFlags
    /// </summary>
    public class NrpcNetrLogonSamLogonWithFlagsResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NtStatus Status;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR ReturnAuthenticator;

        /// <summary>
        ///  ValidationInformation parameter.
        /// </summary>
        public _NETLOGON_VALIDATION ValidationInformation;

        /// <summary>
        ///  Authoritative parameter.
        /// </summary>
        public byte? Authoritative;

        /// <summary>
        ///  ExtraFlags parameter.
        /// </summary>
        public uint? ExtraFlags;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrLogonSamLogonWithFlagsResponse()
        {
            Opnum = NrpcMethodOpnums.NetrLogonSamLogonWithFlags;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrLogonSamLogonWithFlagsRequest request =
                sessionContext.LastRequestReceived as NrpcNetrLogonSamLogonWithFlagsRequest;

            if (request == null)
            {
                throw new InvalidOperationException(
                    "The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pReturnAuthenticator = TypeMarshal.ToIntPtr(ReturnAuthenticator);
            SafeIntPtr pValidationInformation = TypeMarshal.ToIntPtr(ValidationInformation,
                request.ValidationLevel, null, null);
            SafeIntPtr pAuthoritative = TypeMarshal.ToIntPtr(Authoritative);
            SafeIntPtr pExtraFlags = TypeMarshal.ToIntPtr(ExtraFlags);
            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pReturnAuthenticator,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    (uint)request.ValidationLevel,
                    pValidationInformation,
                    pAuthoritative,
                    pExtraFlags,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pReturnAuthenticator.Dispose();
                pValidationInformation.Dispose();
                pAuthoritative.Dispose();
                pExtraFlags.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrServerGetTrustInfoRequest class defines input parameters of method NetrServerGetTrustInfo
    /// </summary>
    public class NrpcNetrServerGetTrustInfoRequest : NrpcRequestStub
    {
        /// <summary>
        ///  TrustedDcName parameter.
        /// </summary>
        public string TrustedDcName;

        /// <summary>
        ///  AccountName parameter.
        /// </summary>
        public string AccountName;

        /// <summary>
        ///  SecureChannelType parameter.
        /// </summary>
        public _NETLOGON_SECURE_CHANNEL_TYPE SecureChannelType;

        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        ///  Authenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? Authenticator;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrServerGetTrustInfoRequest()
        {
            Opnum = NrpcMethodOpnums.NetrServerGetTrustInfo;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                TrustedDcName = Marshal.PtrToStringUni(outParamList[0]);
                AccountName = Marshal.PtrToStringUni(outParamList[1]);
                SecureChannelType = (_NETLOGON_SECURE_CHANNEL_TYPE)outParamList[2].ToUInt32();
                ComputerName = Marshal.PtrToStringUni(outParamList[3]);
                Authenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[4]);
            }
        }
    };


    /// <summary>
    /// The NetrServerGetTrustInfoResponse class defines output parameters of method NetrServerGetTrustInfo
    /// </summary>
    public class NrpcNetrServerGetTrustInfoResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NtStatus Status;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        ///  EncryptedNewOwfPassword parameter.
        /// </summary>
        public _NT_OWF_PASSWORD? EncryptedNewOwfPassword;

        /// <summary>
        ///  EncryptedOldOwfPassword parameter.
        /// </summary>
        public _NT_OWF_PASSWORD? EncryptedOldOwfPassword;

        /// <summary>
        ///  TrustInfo parameter.
        /// </summary>
        public _NL_GENERIC_RPC_DATA? TrustInfo;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrServerGetTrustInfoResponse()
        {
            Opnum = NrpcMethodOpnums.NetrServerGetTrustInfo;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrServerGetTrustInfoRequest request =
                sessionContext.LastRequestReceived as NrpcNetrServerGetTrustInfoRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pReturnAuthenticator = TypeMarshal.ToIntPtr(ReturnAuthenticator);
            SafeIntPtr pEncryptedNewOwfPassword = TypeMarshal.ToIntPtr(EncryptedNewOwfPassword);
            SafeIntPtr pEncryptedOldOwfPassword = TypeMarshal.ToIntPtr(EncryptedOldOwfPassword);
            SafeIntPtr pTrustInfo = TypeMarshal.ToIntPtr(TrustInfo);
            SafeIntPtr ppTrustInfo = TypeMarshal.ToIntPtr(pTrustInfo.Value);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pReturnAuthenticator,
                    pEncryptedNewOwfPassword,
                    pEncryptedOldOwfPassword,
                    ppTrustInfo,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pReturnAuthenticator.Dispose();
                pEncryptedNewOwfPassword.Dispose();
                pEncryptedOldOwfPassword.Dispose();
                ppTrustInfo.Dispose();
            }
        }
    };


    /// <summary>
    /// The OpnumUnused47Request class defines input parameters of method OpnumUnused47
    /// </summary>
    public class NrpcOpnumUnused47Request : NrpcRequestStub
    {
        /// <summary>
        ///  ContextHandle parameter.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr ContextHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcOpnumUnused47Request()
        {
            Opnum = NrpcMethodOpnums.OpnumUnused47;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    };


    /// <summary>
    /// The OpnumUnused47Response class defines output parameters of method OpnumUnused47
    /// </summary>
    public class NrpcOpnumUnused47Response : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NtStatus Status;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcOpnumUnused47Response()
        {
            Opnum = NrpcMethodOpnums.OpnumUnused47;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            return null;
        }
    };


    /// <summary>
    /// The DsrUpdateReadOnlyServerDnsRecordsRequest class defines input parameters of
    /// method DsrUpdateReadOnlyServerDnsRecords
    /// </summary>
    public class NrpcDsrUpdateReadOnlyServerDnsRecordsRequest : NrpcRequestStub
    {
        /// <summary>
        ///  ServerName parameter.
        /// </summary>
        public string ServerName;

        /// <summary>
        ///  ComputerName parameter.
        /// </summary>
        public string ComputerName;

        /// <summary>
        ///  Authenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? Authenticator;

        /// <summary>
        ///  SiteName parameter.
        /// </summary>
        public string SiteName;

        /// <summary>
        ///  DnsTtl parameter.
        /// </summary>
        public uint DnsTtl;

        /// <summary>
        ///  DnsNames parameter.
        /// </summary>
        public _NL_DNS_NAME_INFO_ARRAY? DnsNames;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcDsrUpdateReadOnlyServerDnsRecordsRequest()
        {
            Opnum = NrpcMethodOpnums.DsrUpdateReadOnlyServerDnsRecords;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ServerName = Marshal.PtrToStringUni(outParamList[0]);
                ComputerName = Marshal.PtrToStringUni(outParamList[1]);
                Authenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[2]);
                SiteName = Marshal.PtrToStringUni(outParamList[4]);
                DnsTtl = outParamList[5].ToUInt32();
                DnsNames = TypeMarshal.ToNullableStruct<_NL_DNS_NAME_INFO_ARRAY>(outParamList[6]);
            }
        }
    };


    /// <summary>
    /// The DsrUpdateReadOnlyServerDnsRecordsResponse class defines output parameters of
    /// method DsrUpdateReadOnlyServerDnsRecords
    /// </summary>
    public class NrpcDsrUpdateReadOnlyServerDnsRecordsResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NtStatus Status;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        ///  DnsNames parameter.
        /// </summary>
        public _NL_DNS_NAME_INFO_ARRAY? DnsNames;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcDsrUpdateReadOnlyServerDnsRecordsResponse()
        {
            Opnum = NrpcMethodOpnums.DsrUpdateReadOnlyServerDnsRecords;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcDsrUpdateReadOnlyServerDnsRecordsRequest request =
                sessionContext.LastRequestReceived as NrpcDsrUpdateReadOnlyServerDnsRecordsRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pReturnAuthenticator = TypeMarshal.ToIntPtr(ReturnAuthenticator);
            SafeIntPtr pDnsNames = TypeMarshal.ToIntPtr(DnsNames);

            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pReturnAuthenticator,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pDnsNames,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pReturnAuthenticator.Dispose();
                pDnsNames.Dispose();
            }
        }
    };


    /// <summary>
    /// The NetrChainSetClientAttributesRequest class defines input parameters of
    /// method NetrChainSetClientAttributes
    /// </summary>
    public class NrpcNetrChainSetClientAttributesRequest : NrpcRequestStub
    {
        /// <summary>
        ///  PrimaryName parameter.
        /// </summary>
        public string PrimaryName;

        /// <summary>
        ///  ChainedFromServerName parameter.
        /// </summary>
        public string ChainedFromServerName;

        /// <summary>
        ///  ChainedForClientName parameter.
        /// </summary>
        public string ChainedForClientName;

        /// <summary>
        ///  Authenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? Authenticator;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        ///  dwInVersion parameter.
        /// </summary>
        public uint dwInVersion;

        /// <summary>
        ///  pmsgIn parameter.
        /// </summary>
        public NL_IN_CHAIN_SET_CLIENT_ATTRIBUTES? pmsgIn;

        /// <summary>
        ///  pdwOutVersion parameter.
        /// </summary>
        public uint? pdwOutVersion;

        /// <summary>
        ///  pmsgOut parameter.
        /// </summary>
        public NL_OUT_CHAIN_SET_CLIENT_ATTRIBUTES? pmsgOut;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrChainSetClientAttributesRequest()
        {
            Opnum = NrpcMethodOpnums.NetrChainSetClientAttributes;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(NrpcServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                NrpcRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] {
                    new RpceStubExprEval(NrpcRpcAdapter.logon__NETLOGON_DELTA_USERExprEval_0000) },
                NrpcRpcStubFormatString.ProcFormatString,
                NrpcRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                PrimaryName = Marshal.PtrToStringUni(outParamList[0]);
                ChainedFromServerName = Marshal.PtrToStringUni(outParamList[1]);
                ChainedForClientName = Marshal.PtrToStringUni(outParamList[2]);
                Authenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[3]);
                ReturnAuthenticator = TypeMarshal.ToNullableStruct<_NETLOGON_AUTHENTICATOR>(outParamList[4]);
                dwInVersion = outParamList[5].ToUInt32();
                pmsgIn = TypeMarshal.ToNullableStruct<NL_IN_CHAIN_SET_CLIENT_ATTRIBUTES>(outParamList[6], dwInVersion, null, null);
                pdwOutVersion = TypeMarshal.ToNullableStruct<uint>(outParamList[7]);
                pmsgOut = TypeMarshal.ToNullableStruct<NL_OUT_CHAIN_SET_CLIENT_ATTRIBUTES>(outParamList[8], pdwOutVersion, null, null);
            }
        }
    };


    /// <summary>
    /// The NetrChainSetClientAttributesResponse class defines output parameters of
    /// method NetrChainSetClientAttributes
    /// </summary>
    public class NrpcNetrChainSetClientAttributesResponse : NrpcResponseStub
    {
        /// <summary>
        ///  Return value of the RPC method.
        /// </summary>
        public NtStatus Status;

        /// <summary>
        ///  ReturnAuthenticator parameter.
        /// </summary>
        public _NETLOGON_AUTHENTICATOR? ReturnAuthenticator;

        /// <summary>
        ///  PdwOutVersion parameter.
        /// </summary>
        public uint? PdwOutVersion;

        /// <summary>
        ///  PmsgOut parameter.
        /// </summary>
        public NL_OUT_CHAIN_SET_CLIENT_ATTRIBUTES? PmsgOut;

        /// <summary>
        /// Constructor method
        /// </summary>
        public NrpcNetrChainSetClientAttributesResponse()
        {
            Opnum = NrpcMethodOpnums.NetrChainSetClientAttributes;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when corresponding request isn't found
        /// </exception>
        internal override byte[] Encode(NrpcServerSessionContext sessionContext)
        {
            NrpcNetrChainSetClientAttributesRequest request =
                sessionContext.LastRequestReceived as NrpcNetrChainSetClientAttributesRequest;

            if (request == null)
            {
                throw new InvalidOperationException("The corresponding request isn't found, unable to create the response");
            }

            SafeIntPtr pReturnAuthenticator = TypeMarshal.ToIntPtr(ReturnAuthenticator);
            SafeIntPtr pPdwOutVersion = TypeMarshal.ToIntPtr(PdwOutVersion);
            SafeIntPtr pPmsgOut = TypeMarshal.ToIntPtr(PmsgOut, PdwOutVersion, null, null);
            try
            {
                Int3264[] paramList = new Int3264[]
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pReturnAuthenticator,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pPdwOutVersion,
                    pPmsgOut,
                    (uint)Status
                };

                return NrpcStubEncodeToBytes(paramList, Opnum);
            }
            finally
            {
                pReturnAuthenticator.Dispose();
                pPdwOutVersion.Dispose();
                pPmsgOut.Dispose();
            }
        }
    };
    #endregion

    #endregion
}
