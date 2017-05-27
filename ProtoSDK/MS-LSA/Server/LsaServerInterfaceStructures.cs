// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa
{
    /// <summary>
    /// Opnums of Lsa methods
    /// </summary>
    public enum LsaMethodOpnums : int
    {
        /// <summary>
        /// Opnum of method LsarClose
        /// </summary>
        LsarClose = 0,

        /// <summary>
        /// Opnum of method Opnum1NotUsedOnWire
        /// </summary>
        Opnum1NotUsedOnWire = 1,

        /// <summary>
        /// Opnum of method LsarEnumeratePrivileges
        /// </summary>
        LsarEnumeratePrivileges = 2,

        /// <summary>
        /// Opnum of method LsarQuerySecurityObject
        /// </summary>
        LsarQuerySecurityObject = 3,

        /// <summary>
        /// Opnum of method LsarSetSecurityObject
        /// </summary>
        LsarSetSecurityObject = 4,

        /// <summary>
        /// Opnum of method Opnum5NotUsedOnWire
        /// </summary>
        Opnum5NotUsedOnWire = 5,

        /// <summary>
        /// Opnum of method LsarOpenPolicy
        /// </summary>
        LsarOpenPolicy = 6,

        /// <summary>
        /// Opnum of method LsarQueryInformationPolicy
        /// </summary>
        LsarQueryInformationPolicy = 7,

        /// <summary>
        /// Opnum of method LsarSetInformationPolicy
        /// </summary>
        LsarSetInformationPolicy = 8,

        /// <summary>
        /// Opnum of method Opnum9NotUsedOnWire
        /// </summary>
        Opnum9NotUsedOnWire = 9,

        /// <summary>
        /// Opnum of method LsarCreateAccount
        /// </summary>
        LsarCreateAccount = 10,

        /// <summary>
        /// Opnum of method LsarEnumerateAccounts
        /// </summary>
        LsarEnumerateAccounts = 11,

        /// <summary>
        /// Opnum of method LsarCreateTrustedDomain
        /// </summary>
        LsarCreateTrustedDomain = 12,

        /// <summary>
        /// Opnum of method LsarEnumerateTrustedDomains
        /// </summary>
        LsarEnumerateTrustedDomains = 13,

        /// <summary>
        /// Opnum of method LsarLookupNames
        /// </summary>
        LsarLookupNames = 14,

        /// <summary>
        /// Opnum of method LsarLookupSids
        /// </summary>
        LsarLookupSids = 15,

        /// <summary>
        /// Opnum of method LsarCreateSecret
        /// </summary>
        LsarCreateSecret = 16,

        /// <summary>
        /// Opnum of method LsarOpenAccount
        /// </summary>
        LsarOpenAccount = 17,

        /// <summary>
        /// Opnum of method LsarEnumeratePrivilegesAccount
        /// </summary>
        LsarEnumeratePrivilegesAccount = 18,

        /// <summary>
        /// Opnum of method LsarAddPrivilegesToAccount
        /// </summary>
        LsarAddPrivilegesToAccount = 19,

        /// <summary>
        /// Opnum of method LsarRemovePrivilegesFromAccount
        /// </summary>
        LsarRemovePrivilegesFromAccount = 20,

        /// <summary>
        /// Opnum of method Opnum21NotUsedOnWire
        /// </summary>
        Opnum21NotUsedOnWire = 21,

        /// <summary>
        /// Opnum of method Opnum22NotUsedOnWire
        /// </summary>
        Opnum22NotUsedOnWire = 22,

        /// <summary>
        /// Opnum of method LsarGetSystemAccessAccount
        /// </summary>
        LsarGetSystemAccessAccount = 23,

        /// <summary>
        /// Opnum of method LsarSetSystemAccessAccount
        /// </summary>
        LsarSetSystemAccessAccount = 24,

        /// <summary>
        /// Opnum of method LsarOpenTrustedDomain
        /// </summary>
        LsarOpenTrustedDomain = 25,

        /// <summary>
        /// Opnum of method LsarQueryInfoTrustedDomain
        /// </summary>
        LsarQueryInfoTrustedDomain = 26,

        /// <summary>
        /// Opnum of method LsarSetInformationTrustedDomain
        /// </summary>
        LsarSetInformationTrustedDomain = 27,

        /// <summary>
        /// Opnum of method LsarOpenSecret
        /// </summary>
        LsarOpenSecret = 28,

        /// <summary>
        /// Opnum of method LsarSetSecret
        /// </summary>
        LsarSetSecret = 29,

        /// <summary>
        /// Opnum of method LsarQuerySecret
        /// </summary>
        LsarQuerySecret = 30,

        /// <summary>
        /// Opnum of method LsarLookupPrivilegeValue
        /// </summary>
        LsarLookupPrivilegeValue = 31,

        /// <summary>
        /// Opnum of method LsarLookupPrivilegeName
        /// </summary>
        LsarLookupPrivilegeName = 32,

        /// <summary>
        /// Opnum of method LsarLookupPrivilegeDisplayName
        /// </summary>
        LsarLookupPrivilegeDisplayName = 33,

        /// <summary>
        /// Opnum of method LsarDeleteObject
        /// </summary>
        LsarDeleteObject = 34,

        /// <summary>
        /// Opnum of method LsarEnumerateAccountsWithUserRight
        /// </summary>
        LsarEnumerateAccountsWithUserRight = 35,

        /// <summary>
        /// Opnum of method LsarEnumerateAccountRights
        /// </summary>
        LsarEnumerateAccountRights = 36,

        /// <summary>
        /// Opnum of method LsarAddAccountRights
        /// </summary>
        LsarAddAccountRights = 37,

        /// <summary>
        /// Opnum of method LsarRemoveAccountRights
        /// </summary>
        LsarRemoveAccountRights = 38,

        /// <summary>
        /// Opnum of method LsarQueryTrustedDomainInfo
        /// </summary>
        LsarQueryTrustedDomainInfo = 39,

        /// <summary>
        /// Opnum of method LsarSetTrustedDomainInfo
        /// </summary>
        LsarSetTrustedDomainInfo = 40,

        /// <summary>
        /// Opnum of method LsarDeleteTrustedDomain
        /// </summary>
        LsarDeleteTrustedDomain = 41,

        /// <summary>
        /// Opnum of method LsarStorePrivateData
        /// </summary>
        LsarStorePrivateData = 42,

        /// <summary>
        /// Opnum of method LsarRetrievePrivateData
        /// </summary>
        LsarRetrievePrivateData = 43,

        /// <summary>
        /// Opnum of method LsarOpenPolicy2
        /// </summary>
        LsarOpenPolicy2 = 44,

        /// <summary>
        /// Opnum of method LsarGetUserName
        /// </summary>
        LsarGetUserName = 45,

        /// <summary>
        /// Opnum of method LsarQueryInformationPolicy2
        /// </summary>
        LsarQueryInformationPolicy2 = 46,

        /// <summary>
        /// Opnum of method LsarSetInformationPolicy2
        /// </summary>
        LsarSetInformationPolicy2 = 47,

        /// <summary>
        /// Opnum of method LsarQueryTrustedDomainInfoByName
        /// </summary>
        LsarQueryTrustedDomainInfoByName = 48,

        /// <summary>
        /// Opnum of method LsarSetTrustedDomainInfoByName
        /// </summary>
        LsarSetTrustedDomainInfoByName = 49,

        /// <summary>
        /// Opnum of method LsarEnumerateTrustedDomainsEx
        /// </summary>
        LsarEnumerateTrustedDomainsEx = 50,

        /// <summary>
        /// Opnum of method LsarCreateTrustedDomainEx
        /// </summary>
        LsarCreateTrustedDomainEx = 51,

        /// <summary>
        /// Opnum of method Opnum52NotUsedOnWire
        /// </summary>
        Opnum52NotUsedOnWire = 52,

        /// <summary>
        /// Opnum of method LsarQueryDomainInformationPolicy
        /// </summary>
        LsarQueryDomainInformationPolicy = 53,

        /// <summary>
        /// Opnum of method LsarSetDomainInformationPolicy
        /// </summary>
        LsarSetDomainInformationPolicy = 54,

        /// <summary>
        /// Opnum of method LsarOpenTrustedDomainByName
        /// </summary>
        LsarOpenTrustedDomainByName = 55,

        /// <summary>
        /// Opnum of method Opnum56NotUsedOnWire
        /// </summary>
        Opnum56NotUsedOnWire = 56,

        /// <summary>
        /// Opnum of method LsarLookupSids2
        /// </summary>
        LsarLookupSids2 = 57,

        /// <summary>
        /// Opnum of method LsarLookupNames2
        /// </summary>
        LsarLookupNames2 = 58,

        /// <summary>
        /// Opnum of method LsarCreateTrustedDomainEx2
        /// </summary>
        LsarCreateTrustedDomainEx2 = 59,

        /// <summary>
        /// Opnum of method Opnum60NotUsedOnWire
        /// </summary>
        Opnum60NotUsedOnWire = 60,

        /// <summary>
        /// Opnum of method Opnum61NotUsedOnWire
        /// </summary>
        Opnum61NotUsedOnWire = 61,

        /// <summary>
        /// Opnum of method Opnum62NotUsedOnWire
        /// </summary>
        Opnum62NotUsedOnWire = 62,

        /// <summary>
        /// Opnum of method Opnum63NotUsedOnWire
        /// </summary>
        Opnum63NotUsedOnWire = 63,

        /// <summary>
        /// Opnum of method Opnum64NotUsedOnWire
        /// </summary>
        Opnum64NotUsedOnWire = 64,

        /// <summary>
        /// Opnum of method Opnum65NotUsedOnWire
        /// </summary>
        Opnum65NotUsedOnWire = 65,

        /// <summary>
        /// Opnum of method Opnum66NotUsedOnWire
        /// </summary>
        Opnum66NotUsedOnWire = 66,

        /// <summary>
        /// Opnum of method Opnum67NotUsedOnWire
        /// </summary>
        Opnum67NotUsedOnWire = 67,

        /// <summary>
        /// Opnum of method LsarLookupNames3
        /// </summary>
        LsarLookupNames3 = 68,

        /// <summary>
        /// Opnum of method Opnum69NotUsedOnWire
        /// </summary>
        Opnum69NotUsedOnWire = 69,

        /// <summary>
        /// Opnum of method Opnum70NotUsedOnWire
        /// </summary>
        Opnum70NotUsedOnWire = 70,

        /// <summary>
        /// Opnum of method Opnum71NotUsedOnWire
        /// </summary>
        Opnum71NotUsedOnWire = 71,

        /// <summary>
        /// Opnum of method Opnum72NotUsedOnWire
        /// </summary>
        Opnum72NotUsedOnWire = 72,

        /// <summary>
        /// Opnum of method LsarQueryForestTrustInformation
        /// </summary>
        LsarQueryForestTrustInformation = 73,

        /// <summary>
        /// Opnum of method LsarSetForestTrustInformation
        /// </summary>
        LsarSetForestTrustInformation = 74,

        /// <summary>
        /// Opnum of method Opnum75NotUsedOnWire
        /// </summary>
        Opnum75NotUsedOnWire = 75,

        /// <summary>
        /// Opnum of method LsarLookupSids3
        /// </summary>
        LsarLookupSids3 = 76,

        /// <summary>
        /// Opnum of method LsarLookupNames4
        /// </summary>
        LsarLookupNames4 = 77
    }


    /// <summary>
    /// The base class of all Lsa request
    /// </summary>
    public abstract class LsaRequestStub
    {
        private LsaMethodOpnums rpceLayerOpnum;

        /// <summary>
        ///  Opnum at RPC layer
        /// </summary>
        public LsaMethodOpnums Opnum
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
        internal abstract void Decode(LsaServerSessionContext sessionContext, byte[] requestStub);


        /// <summary>
        /// decode request stub to param list.
        /// </summary>
        /// <param name="requestStub">request stub</param>
        /// <returns>param list</returns>
        [CLSCompliant(false)]
        internal protected RpceInt3264Collection LsaStubDecodeToParamList(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            return RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                LsaRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] { new RpceStubExprEval(LsaRpcAdapter.lsarpc__LSAPR_ACLExprEval_0000) },
                LsaRpcStubFormatString.ProcFormatString,
                LsaRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat);
        }
    }


    /// <summary>
    /// The base class of all Lsa response
    /// </summary>
    public abstract class LsaResponseStub
    {
        /// <summary>
        /// Return value of the RPC method.
        /// </summary>
        [CLSCompliant(false)]
        public NtStatus Status;

        /// <summary>
        ///  Opnum at RPC layer
        /// </summary>
        private LsaMethodOpnums rpceLayerOpnum;

        /// <summary>
        ///  Opnum at RPC layer
        /// </summary>
        public LsaMethodOpnums Opnum
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
        internal abstract byte[] Encode(LsaServerSessionContext sessionContext);


        /// <summary>
        /// encode param list to bytes
        /// </summary>
        /// <param name="paramList">param list</param>
        /// <param name="Opnum">rpc method opnum</param>
        /// <returns>bytes array</returns>
        internal protected byte[] LsaStubEncodeToBytes(Int3264[] paramList, LsaMethodOpnums Opnum)
        {
            return RpceStubEncoder.ToBytes(
              RpceStubHelper.GetPlatform(),
                LsaRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] { new RpceStubExprEval(LsaRpcAdapter.lsarpc__LSAPR_ACLExprEval_0000) },
                LsaRpcStubFormatString.ProcFormatString,
                LsaRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                paramList);
        }
    }


    #region Structures of input and output parameters of LSA methods
    /// <summary>
    /// The LsarCloseRequest class defines input parameters of method LsarClose.
    /// </summary>
    public class LsarCloseRequest : LsaRequestStub
    {
        /// <summary>
        ///  ObjectHandle parameter.
        /// </summary>
        public IntPtr? ObjectHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarCloseRequest()
        {
            Opnum = LsaMethodOpnums.LsarClose;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                LsaRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] { 
                    new RpceStubExprEval(LsaRpcAdapter.lsarpc__LSAPR_ACLExprEval_0000) },
                    LsaRpcStubFormatString.ProcFormatString,
                    LsaRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    requestStub,
                    sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ObjectHandle = TypeMarshal.ToNullableStruct<IntPtr>(outParamList[0]);
            }
        }
    }


    /// <summary>
    /// The LsarCloseResponse class defines output parameters of method LsarClose.
    /// </summary>
    public class LsarCloseResponse : LsaResponseStub
    {
        /// <summary>
        ///  ObjectHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr ObjectHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarCloseResponse()
        {
            Opnum = LsaMethodOpnums.LsarClose;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            SafeIntPtr pObjectHandle = TypeMarshal.ToIntPtr(ObjectHandle);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    pObjectHandle,
                    (uint)Status
                };

                return RpceStubEncoder.ToBytes(
                  RpceStubHelper.GetPlatform(),
                    LsaRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { 
                        new RpceStubExprEval(LsaRpcAdapter.lsarpc__LSAPR_ACLExprEval_0000) },
                    LsaRpcStubFormatString.ProcFormatString,
                    LsaRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pObjectHandle.Dispose();
            }
        }
    }


    /// <summary>
    /// The Opnum1NotUsedOnWireRequest class defines input parameters 
    /// of method Opnum1NotUsedOnWire.
    /// </summary>
    public class Opnum1NotUsedOnWireRequest : LsaRequestStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum1NotUsedOnWireRequest()
        {
            Opnum = LsaMethodOpnums.Opnum1NotUsedOnWire;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    }


    /// <summary>
    /// The Opnum1NotUsedOnWireResponse class defines output parameters 
    /// of method Opnum1NotUsedOnWire.
    /// </summary>
    public class Opnum1NotUsedOnWireResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum1NotUsedOnWireResponse()
        {
            Opnum = LsaMethodOpnums.Opnum1NotUsedOnWire;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            return null;
        }
    }


    /// <summary>
    /// The Opnum5NotUsedOnWireRequest class defines input parameters 
    /// of method Opnum5NotUsedOnWire.
    /// </summary>
    public class Opnum5NotUsedOnWireRequest : LsaRequestStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum5NotUsedOnWireRequest()
        {
            Opnum = LsaMethodOpnums.Opnum5NotUsedOnWire;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    }


    /// <summary>
    /// The Opnum5NotUsedOnWireResponse class defines output parameters 
    /// of method Opnum5NotUsedOnWire.
    /// </summary>
    public class Opnum5NotUsedOnWireResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum5NotUsedOnWireResponse()
        {
            Opnum = LsaMethodOpnums.Opnum5NotUsedOnWire;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            return null;
        }
    }


    /// <summary>
    /// The LsarOpenPolicyRequest class defines input parameters of method LsarOpenPolicy.
    /// </summary>
    public class LsarOpenPolicyRequest : LsaRequestStub
    {
        /// <summary>
        ///  SystemName parameter.
        /// </summary>
        [CLSCompliant(false)]
        public ushort[] SystemName;

        /// <summary>
        ///  ObjectAttributes parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_OBJECT_ATTRIBUTES? ObjectAttributes;

        /// <summary>
        ///  DesiredAccess parameter.
        /// </summary>
        [CLSCompliant(false)]
        public ACCESS_MASK DesiredAccess;

        /// <summary>
        ///  Constructor method
        /// </summary>
        public LsarOpenPolicyRequest()
        {
            Opnum = LsaMethodOpnums.LsarOpenPolicy;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                LsaRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] { 
                    new RpceStubExprEval(LsaRpcAdapter.lsarpc__LSAPR_ACLExprEval_0000) },
                    LsaRpcStubFormatString.ProcFormatString,
                    LsaRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    requestStub,
                    sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                SystemName = IntPtrUtility.PtrToArray<ushort>(outParamList[0], 1);
                ObjectAttributes = TypeMarshal.ToNullableStruct<_LSAPR_OBJECT_ATTRIBUTES>(outParamList[1]);
                DesiredAccess = (ACCESS_MASK)(outParamList[2].ToInt32());
            }
        }
    }


    /// <summary>
    /// The LsarOpenPolicyResponse class defines output parameters of method LsarOpenPolicy.
    /// </summary>
    public class LsarOpenPolicyResponse : LsaResponseStub
    {
        /// <summary>
        ///  PolicyHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        ///  Constructor method
        /// </summary>
        public LsarOpenPolicyResponse()
        {
            Opnum = LsaMethodOpnums.LsarOpenPolicy;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            SafeIntPtr pPolicyHandle = TypeMarshal.ToIntPtr(PolicyHandle);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pPolicyHandle,
                    (uint)Status
                };

                return RpceStubEncoder.ToBytes(
                  RpceStubHelper.GetPlatform(),
                    LsaRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { 
                        new RpceStubExprEval(LsaRpcAdapter.lsarpc__LSAPR_ACLExprEval_0000) },
                    LsaRpcStubFormatString.ProcFormatString,
                    LsaRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pPolicyHandle.Dispose();
            }
        }
    }


    /// <summary>
    /// The Opnum9NotUsedOnWireRequest class defines input parameters 
    /// of method Opnum9NotUsedOnWire.
    /// </summary>
    public class Opnum9NotUsedOnWireRequest : LsaRequestStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum9NotUsedOnWireRequest()
        {
            Opnum = LsaMethodOpnums.Opnum9NotUsedOnWire;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    }


    /// <summary>
    /// The Opnum9NotUsedOnWireResponse class defines output parameters 
    /// of method Opnum9NotUsedOnWire.
    /// </summary>
    public class Opnum9NotUsedOnWireResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum9NotUsedOnWireResponse()
        {
            Opnum = LsaMethodOpnums.Opnum9NotUsedOnWire;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            return null;
        }
    }


    /// <summary>
    /// The LsarLookupNamesRequest class defines input parameters of method LsarLookupNames.
    /// </summary>
    public class LsarLookupNamesRequest : LsaRequestStub
    {
        /// <summary>
        ///  PolicyHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        ///  Count parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint Count;

        /// <summary>
        ///  Names parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_UNICODE_STRING? Names;

        /// <summary>
        ///  TranslatedSids parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_TRANSLATED_SIDS? TranslatedSids;

        /// <summary>
        ///  LookupLevel parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _LSAP_LOOKUP_LEVEL LookupLevel;

        /// <summary>
        ///  MappedCount parameter.
        /// </summary>
        [CLSCompliant(false)]
        public UInt32? MappedCount;

        /// <summary>
        ///  Constructor method
        /// </summary>
        public LsarLookupNamesRequest()
        {
            Opnum = LsaMethodOpnums.LsarLookupNames;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                LsaRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] { 
                    new RpceStubExprEval(LsaRpcAdapter.lsarpc__LSAPR_ACLExprEval_0000) },
                    LsaRpcStubFormatString.ProcFormatString,
                    LsaRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    requestStub,
                    sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                Count = outParamList[1].ToUInt32();
                Names = TypeMarshal.ToNullableStruct<_RPC_UNICODE_STRING>(outParamList[2], null, Count, null);
                TranslatedSids = TypeMarshal.ToNullableStruct<_LSAPR_TRANSLATED_SIDS>(outParamList[4]);
                LookupLevel = (_LSAP_LOOKUP_LEVEL)(outParamList[5].ToUInt32());
                MappedCount = TypeMarshal.ToNullableStruct<uint>(outParamList[6]);
            }
        }
    }


    /// <summary>
    /// The LsarLookupNamesResponse class defines output parameters of method LsarLookupNames.
    /// </summary>
    public class LsarLookupNamesResponse : LsaResponseStub
    {
        /// <summary>
        ///  ReferencedDomains parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_REFERENCED_DOMAIN_LIST ReferencedDomains;

        /// <summary>
        ///  TranslatedSids parameter.
        /// </summary>        
        [CLSCompliant(false)]
        public _LSAPR_TRANSLATED_SIDS TranslatedSids;

        /// <summary>
        ///  MappedCount parameter.
        /// </summary>
        [CLSCompliant(false)]
        public UInt32? MappedCount;

        /// <summary>
        ///  Constructor method
        /// </summary>
        public LsarLookupNamesResponse()
        {
            Opnum = LsaMethodOpnums.LsarLookupNames;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            SafeIntPtr pReferencedDomains = TypeMarshal.ToIntPtr(ReferencedDomains);
            SafeIntPtr ppReferencedDomains = TypeMarshal.ToIntPtr(pReferencedDomains.Value);
            SafeIntPtr pTranslatedSids = TypeMarshal.ToIntPtr(TranslatedSids);
            SafeIntPtr pMappedCount = TypeMarshal.ToIntPtr(MappedCount);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ppReferencedDomains,
                    pTranslatedSids,
                    IntPtr.Zero,
                    pMappedCount,
                    (uint)Status
                };

                return RpceStubEncoder.ToBytes(
                  RpceStubHelper.GetPlatform(),
                    LsaRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { 
                        new RpceStubExprEval(LsaRpcAdapter.lsarpc__LSAPR_ACLExprEval_0000) },
                    LsaRpcStubFormatString.ProcFormatString,
                    LsaRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pReferencedDomains.Dispose();
                ppReferencedDomains.Dispose();
                pTranslatedSids.Dispose();
                pMappedCount.Dispose();
            }
        }
    }


    /// <summary>
    /// The LsarLookupSidsRequest class defines input parameters of method LsarLookupSids.
    /// </summary>
    public class LsarLookupSidsRequest : LsaRequestStub
    {
        /// <summary>
        ///  PolicyHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        ///  SidEnumBuffer parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_SID_ENUM_BUFFER? SidEnumBuffer;

        /// <summary>
        ///  TranslatedNames parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_TRANSLATED_NAMES? TranslatedNames;

        /// <summary>
        ///  LookupLevel parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _LSAP_LOOKUP_LEVEL LookupLevel;

        /// <summary>
        ///  MappedCount parameter.
        /// </summary>
        [CLSCompliant(false)]
        public UInt32? MappedCount;

        /// <summary>
        ///  Constructor method
        /// </summary>
        public LsarLookupSidsRequest()
        {
            Opnum = LsaMethodOpnums.LsarLookupSids;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                LsaRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] { 
                    new RpceStubExprEval(LsaRpcAdapter.lsarpc__LSAPR_ACLExprEval_0000) },
                    LsaRpcStubFormatString.ProcFormatString,
                    LsaRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    requestStub,
                    sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                SidEnumBuffer = TypeMarshal.ToNullableStruct<_LSAPR_SID_ENUM_BUFFER>(outParamList[1]);
                TranslatedNames = TypeMarshal.ToNullableStruct<_LSAPR_TRANSLATED_NAMES>(outParamList[3]);
                LookupLevel = (_LSAP_LOOKUP_LEVEL)(outParamList[4].ToUInt32());
                MappedCount = TypeMarshal.ToNullableStruct<uint>(outParamList[5]);
            }
        }
    }


    /// <summary>
    /// The LsarLookupSidsResponse class defines output parameters of method LsarLookupSids.
    /// </summary>
    public class LsarLookupSidsResponse : LsaResponseStub
    {
        /// <summary>
        ///  ReferencedDomains parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_REFERENCED_DOMAIN_LIST ReferencedDomains;

        /// <summary>
        ///  TranslatedNames parameter.
        /// </summary>        
        [CLSCompliant(false)]
        public _LSAPR_TRANSLATED_NAMES TranslatedNames;

        /// <summary>
        ///  MappedCount parameter.
        /// </summary>
        [CLSCompliant(false)]
        public UInt32? MappedCount;

        /// <summary>
        ///  Constructor method
        /// </summary>
        public LsarLookupSidsResponse()
        {
            Opnum = LsaMethodOpnums.LsarLookupSids;
        }

        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            SafeIntPtr pReferencedDomains = TypeMarshal.ToIntPtr(ReferencedDomains);
            SafeIntPtr ppReferencedDomains = TypeMarshal.ToIntPtr(pReferencedDomains.Value);
            SafeIntPtr pTranslatedNames = TypeMarshal.ToIntPtr(TranslatedNames);
            SafeIntPtr pMappedCount = TypeMarshal.ToIntPtr(MappedCount);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ppReferencedDomains,
                    pTranslatedNames,
                    IntPtr.Zero,
                    pMappedCount,
                    (uint)Status
                };

                return RpceStubEncoder.ToBytes(
                  RpceStubHelper.GetPlatform(),
                    LsaRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { 
                        new RpceStubExprEval(LsaRpcAdapter.lsarpc__LSAPR_ACLExprEval_0000) },
                    LsaRpcStubFormatString.ProcFormatString,
                    LsaRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pReferencedDomains.Dispose();
                ppReferencedDomains.Dispose();
                pTranslatedNames.Dispose();
                pMappedCount.Dispose();
            }
        }
    }


    /// <summary>
    /// The Opnum21NotUsedOnWireRequest class defines input parameters 
    /// of method Opnum21NotUsedOnWire.
    /// </summary>
    public class Opnum21NotUsedOnWireRequest : LsaRequestStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum21NotUsedOnWireRequest()
        {
            Opnum = LsaMethodOpnums.Opnum21NotUsedOnWire;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    }


    /// <summary>
    /// The Opnum21NotUsedOnWire Response class defines output parameters 
    /// of method Opnum21NotUsedOnWire .
    /// </summary>
    public class Opnum21NotUsedOnWireResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum21NotUsedOnWireResponse()
        {
            Opnum = LsaMethodOpnums.Opnum21NotUsedOnWire;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            return null;
        }
    }


    /// <summary>
    /// The Opnum22NotUsedOnWireRequest class defines input parameters 
    /// of method Opnum22NotUsedOnWire.
    /// </summary>
    public class Opnum22NotUsedOnWireRequest : LsaRequestStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum22NotUsedOnWireRequest()
        {
            Opnum = LsaMethodOpnums.Opnum22NotUsedOnWire;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    }


    /// <summary>
    /// The Opnum22NotUsedOnWireResponse class defines output parameters 
    /// of method Opnum22NotUsedOnWire.
    /// </summary>
    public class Opnum22NotUsedOnWireResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum22NotUsedOnWireResponse()
        {
            Opnum = LsaMethodOpnums.Opnum22NotUsedOnWire;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            return null;
        }
    }


    /// <summary>
    /// The LsarOpenPolicy2Request class defines input parameters of method LsarOpenPolicy2.
    /// </summary>
    public class LsarOpenPolicy2Request : LsaRequestStub
    {
        /// <summary>
        ///  SystemName parameter.
        /// </summary>
        public string SystemName;

        /// <summary>
        ///  ObjectAttributes parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_OBJECT_ATTRIBUTES? ObjectAttributes;

        /// <summary>
        ///  DesiredAccess parameter.
        /// </summary>
        [CLSCompliant(false)]
        public ACCESS_MASK DesiredAccess;

        /// <summary>
        ///  Constructor method
        /// </summary>
        public LsarOpenPolicy2Request()
        {
            Opnum = LsaMethodOpnums.LsarOpenPolicy2;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                LsaRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] { 
                    new RpceStubExprEval(LsaRpcAdapter.lsarpc__LSAPR_ACLExprEval_0000) },
                    LsaRpcStubFormatString.ProcFormatString,
                    LsaRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    requestStub,
                    sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                SystemName = Marshal.PtrToStringUni(outParamList[0]);
                ObjectAttributes = TypeMarshal.ToNullableStruct<_LSAPR_OBJECT_ATTRIBUTES>(outParamList[1]);
                DesiredAccess = (ACCESS_MASK)(outParamList[2].ToInt32());
            }
        }
    }


    /// <summary>
    /// The LsarOpenPolicy2Response class defines output parameters of method LsarOpenPolicy2.
    /// </summary>
    public class LsarOpenPolicy2Response : LsaResponseStub
    {
        /// <summary>
        ///  PolicyHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        ///  Constructor method
        /// </summary>
        public LsarOpenPolicy2Response()
        {
            Opnum = LsaMethodOpnums.LsarOpenPolicy2;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            SafeIntPtr pPolicyHandle = TypeMarshal.ToIntPtr(PolicyHandle);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pPolicyHandle,
                    (uint)Status
                };

                return RpceStubEncoder.ToBytes(
                  RpceStubHelper.GetPlatform(),
                    LsaRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { 
                        new RpceStubExprEval(LsaRpcAdapter.lsarpc__LSAPR_ACLExprEval_0000) },
                    LsaRpcStubFormatString.ProcFormatString,
                    LsaRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pPolicyHandle.Dispose();
            }
        }
    }


    /// <summary>
    /// The LsarGetUserNameRequest class defines input parameters of method LsarGetUserName.
    /// </summary>
    public class LsarGetUserNameRequest : LsaRequestStub
    {
        /// <summary>
        ///  SystemName parameter.
        /// </summary>
        public string SystemName;

        /// <summary>
        ///  UserName parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_UNICODE_STRING? UserName;

        /// <summary>
        ///  DomainName parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_UNICODE_STRING? DomainName;

        /// <summary>
        ///  Constructor method
        /// </summary>
        public LsarGetUserNameRequest()
        {
            Opnum = LsaMethodOpnums.LsarGetUserName;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                LsaRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] { 
                    new RpceStubExprEval(LsaRpcAdapter.lsarpc__LSAPR_ACLExprEval_0000) },
                    LsaRpcStubFormatString.ProcFormatString,
                    LsaRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    requestStub,
                    sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                SystemName = Marshal.PtrToStringUni(outParamList[0]);
                UserName = TypeMarshal.ToNullableStruct<_RPC_UNICODE_STRING>(Marshal.ReadIntPtr(outParamList[1]));
                DomainName = TypeMarshal.ToNullableStruct<_RPC_UNICODE_STRING>(Marshal.ReadIntPtr(outParamList[2]));
            }
        }
    }


    /// <summary>
    /// The LsarGetUserNameResponse class defines output parameters of method LsarGetUserName.
    /// </summary>
    public class LsarGetUserNameResponse : LsaResponseStub
    {
        /// <summary>
        ///  UserName parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_UNICODE_STRING UserName;

        /// <summary>
        ///  DomainName parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_UNICODE_STRING DomainName;

        /// <summary>
        ///  Constructor method
        /// </summary>
        public LsarGetUserNameResponse()
        {
            Opnum = LsaMethodOpnums.LsarGetUserName;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            SafeIntPtr pUserName = TypeMarshal.ToIntPtr(UserName);
            SafeIntPtr ppUserName = TypeMarshal.ToIntPtr(pUserName.Value);
            SafeIntPtr pDomainName = TypeMarshal.ToIntPtr(DomainName);
            SafeIntPtr ppDomainName = TypeMarshal.ToIntPtr(pDomainName.Value);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    ppUserName,
                    ppDomainName,
                    (uint)Status
                };

                return RpceStubEncoder.ToBytes(
                      RpceStubHelper.GetPlatform(),
                        LsaRpcStubFormatString.TypeFormatString,
                        new RpceStubExprEval[] { 
                        new RpceStubExprEval(LsaRpcAdapter.lsarpc__LSAPR_ACLExprEval_0000) },
                        LsaRpcStubFormatString.ProcFormatString,
                        LsaRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                        false,
                        paramList);
            }
            finally
            {
                pUserName.Dispose();
                ppUserName.Dispose();
                pDomainName.Dispose();
                ppDomainName.Dispose();
            }
        }
    }


    /// <summary>
    /// The Opnum52NotUsedOnWireRequest class defines input parameters
    /// of method Opnum52NotUsedOnWire.
    /// </summary>
    public class Opnum52NotUsedOnWireRequest : LsaRequestStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum52NotUsedOnWireRequest()
        {
            Opnum = LsaMethodOpnums.Opnum52NotUsedOnWire;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    }


    /// <summary>
    /// The Opnum52NotUsedOnWireResponse class defines output parameters
    /// of method Opnum52NotUsedOnWire.
    /// </summary>
    public class Opnum52NotUsedOnWireResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum52NotUsedOnWireResponse()
        {
            Opnum = LsaMethodOpnums.Opnum52NotUsedOnWire;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            return null;
        }
    }


    /// <summary>
    /// The Opnum56NotUsedOnWireRequest class defines input parameters
    /// of method Opnum56NotUsedOnWire.
    /// </summary>
    public class Opnum56NotUsedOnWireRequest : LsaRequestStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum56NotUsedOnWireRequest()
        {
            Opnum = LsaMethodOpnums.Opnum56NotUsedOnWire;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    }


    /// <summary>
    /// The Opnum56NotUsedOnWireResponse class defines output parameters
    /// of method Opnum56NotUsedOnWire.
    /// </summary>
    public class Opnum56NotUsedOnWireResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum56NotUsedOnWireResponse()
        {
            Opnum = LsaMethodOpnums.Opnum56NotUsedOnWire;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            return null;
        }
    }


    /// <summary>
    /// The LsarLookupSids2Request class defines input parameters of method LsarLookupSids2.
    /// </summary>
    public class LsarLookupSids2Request : LsaRequestStub
    {
        /// <summary>
        ///  PolicyHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        ///  SidEnumBuffer parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_SID_ENUM_BUFFER? SidEnumBuffer;

        /// <summary>
        ///  TranslatedNames parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_TRANSLATED_NAMES_EX? TranslatedNames;

        /// <summary>
        ///  LookupLevel parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _LSAP_LOOKUP_LEVEL LookupLevel;

        /// <summary>
        ///  MappedCount parameter.
        /// </summary>
        [CLSCompliant(false)]
        public UInt32? MappedCount;

        /// <summary>
        ///  LookupOptions parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint LookupOptions;

        /// <summary>
        ///  ClientRevision parameter.
        /// </summary>
        [CLSCompliant(false)]
        public ClientRevision_Values ClientRevision;

        /// <summary>
        ///  Constructor method
        /// </summary>
        public LsarLookupSids2Request()
        {
            Opnum = LsaMethodOpnums.LsarLookupSids2;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                LsaRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] { 
                    new RpceStubExprEval(LsaRpcAdapter.lsarpc__LSAPR_ACLExprEval_0000) },
                    LsaRpcStubFormatString.ProcFormatString,
                    LsaRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    requestStub,
                    sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                SidEnumBuffer = TypeMarshal.ToNullableStruct<_LSAPR_SID_ENUM_BUFFER>(outParamList[1]);
                TranslatedNames = TypeMarshal.ToNullableStruct<_LSAPR_TRANSLATED_NAMES_EX>(outParamList[3]);
                LookupLevel = (_LSAP_LOOKUP_LEVEL)(outParamList[4].ToUInt32());
                MappedCount = TypeMarshal.ToNullableStruct<uint>(outParamList[5]);
                LookupOptions = outParamList[6].ToUInt32();
                ClientRevision = (ClientRevision_Values)(outParamList[7].ToUInt32());
            }
        }
    }


    /// <summary>
    /// The LsarLookupSids2Response class defines output parameters of method LsarLookupSids2.
    /// </summary>
    public class LsarLookupSids2Response : LsaResponseStub
    {
        /// <summary>
        ///  ReferencedDomains parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_REFERENCED_DOMAIN_LIST ReferencedDomains;

        /// <summary>
        ///  TranslatedNames parameter.
        /// </summary>        
        [CLSCompliant(false)]
        public _LSAPR_TRANSLATED_NAMES_EX TranslatedNames;

        /// <summary>
        ///  MappedCount parameter.
        /// </summary>
        [CLSCompliant(false)]
        public UInt32? MappedCount;

        /// <summary>
        ///  Constructor method
        /// </summary>
        public LsarLookupSids2Response()
        {
            Opnum = LsaMethodOpnums.LsarLookupSids2;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            SafeIntPtr pReferencedDomains = TypeMarshal.ToIntPtr(ReferencedDomains);
            SafeIntPtr ppReferencedDomains = TypeMarshal.ToIntPtr(pReferencedDomains.Value);
            SafeIntPtr pTranslatedNames = TypeMarshal.ToIntPtr(TranslatedNames);
            SafeIntPtr pMappedCount = TypeMarshal.ToIntPtr(MappedCount);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ppReferencedDomains,
                    pTranslatedNames,
                    IntPtr.Zero,
                    pMappedCount,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    (uint)Status
                };

                return RpceStubEncoder.ToBytes(
                  RpceStubHelper.GetPlatform(),
                    LsaRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { 
                        new RpceStubExprEval(LsaRpcAdapter.lsarpc__LSAPR_ACLExprEval_0000) },
                    LsaRpcStubFormatString.ProcFormatString,
                    LsaRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pReferencedDomains.Dispose();
                ppReferencedDomains.Dispose();
                pTranslatedNames.Dispose();
                pMappedCount.Dispose();
            }
        }
    }


    /// <summary>
    /// The LsarLookupNames2Request class defines input parameters of method LsarLookupNames2.
    /// </summary>
    public class LsarLookupNames2Request : LsaRequestStub
    {
        /// <summary>
        ///  PolicyHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        ///  Count parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint Count;

        /// <summary>
        ///  Names parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_UNICODE_STRING? Names;

        /// <summary>
        ///  TranslatedSids parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_TRANSLATED_SIDS_EX? TranslatedSids;

        /// <summary>
        ///  LookupLevel parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _LSAP_LOOKUP_LEVEL LookupLevel;

        /// <summary>
        ///  MappedCount parameter.
        /// </summary>
        [CLSCompliant(false)]
        public UInt32? MappedCount;

        /// <summary>
        ///  LookupOptions parameter.
        /// </summary>
        [CLSCompliant(false)]
        public LookupOptions_Values LookupOptions;

        /// <summary>
        ///  ClientRevision parameter.
        /// </summary>
        [CLSCompliant(false)]
        public ClientRevision_Values ClientRevision;

        /// <summary>
        ///  Constructor method
        /// </summary>
        public LsarLookupNames2Request()
        {
            Opnum = LsaMethodOpnums.LsarLookupNames2;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                LsaRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] { 
                    new RpceStubExprEval(LsaRpcAdapter.lsarpc__LSAPR_ACLExprEval_0000) },
                    LsaRpcStubFormatString.ProcFormatString,
                    LsaRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    requestStub,
                    sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                Count = outParamList[1].ToUInt32();
                Names = TypeMarshal.ToNullableStruct<_RPC_UNICODE_STRING>(outParamList[2], null, Count, null);
                TranslatedSids = TypeMarshal.ToNullableStruct<_LSAPR_TRANSLATED_SIDS_EX>(outParamList[4]);
                LookupLevel = (_LSAP_LOOKUP_LEVEL)(outParamList[5].ToUInt32());
                MappedCount = TypeMarshal.ToNullableStruct<uint>(outParamList[6]);
                LookupOptions = (LookupOptions_Values)(outParamList[7].ToUInt32());
                ClientRevision = (ClientRevision_Values)(outParamList[8].ToUInt32());
            }
        }
    }


    /// <summary>
    /// The LsarLookupNames2Response class defines output parameters of method LsarLookupNames2.
    /// </summary>
    public class LsarLookupNames2Response : LsaResponseStub
    {
        /// <summary>
        ///  ReferencedDomains parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_REFERENCED_DOMAIN_LIST ReferencedDomains;

        /// <summary>
        ///  TranslatedSids parameter.
        /// </summary>        
        [CLSCompliant(false)]
        public _LSAPR_TRANSLATED_SIDS_EX TranslatedSids;

        /// <summary>
        ///  MappedCount parameter.
        /// </summary>
        [CLSCompliant(false)]
        public UInt32? MappedCount;

        /// <summary>
        ///  Constructor method
        /// </summary>
        public LsarLookupNames2Response()
        {
            Opnum = LsaMethodOpnums.LsarLookupNames2;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            SafeIntPtr pReferencedDomains = TypeMarshal.ToIntPtr(ReferencedDomains);
            SafeIntPtr ppReferencedDomains = TypeMarshal.ToIntPtr(pReferencedDomains.Value);
            SafeIntPtr pTranslatedSids = TypeMarshal.ToIntPtr(TranslatedSids);
            SafeIntPtr pMappedCount = TypeMarshal.ToIntPtr(MappedCount);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ppReferencedDomains,
                    pTranslatedSids,
                    IntPtr.Zero,
                    pMappedCount,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    (uint)Status
                };

                return RpceStubEncoder.ToBytes(
                  RpceStubHelper.GetPlatform(),
                    LsaRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { 
                        new RpceStubExprEval(LsaRpcAdapter.lsarpc__LSAPR_ACLExprEval_0000) },
                    LsaRpcStubFormatString.ProcFormatString,
                    LsaRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pReferencedDomains.Dispose();
                ppReferencedDomains.Dispose();
                pTranslatedSids.Dispose();
                pMappedCount.Dispose();
            }
        }
    }


    /// <summary>
    /// The Opnum60NotUsedOnWireRequest class defines input parameters 
    /// of method Opnum60NotUsedOnWire.
    /// </summary>
    public class Opnum60NotUsedOnWireRequest : LsaRequestStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum60NotUsedOnWireRequest()
        {
            Opnum = LsaMethodOpnums.Opnum60NotUsedOnWire;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    }


    /// <summary>
    /// The Opnum60NotUsedOnWireResponse class defines output parameters 
    /// of method Opnum60NotUsedOnWire.
    /// </summary>
    public class Opnum60NotUsedOnWireResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum60NotUsedOnWireResponse()
        {
            Opnum = LsaMethodOpnums.Opnum60NotUsedOnWire;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            return null;
        }
    }


    /// <summary>
    /// The Opnum61NotUsedOnWireRequest class defines input parameters 
    /// of method Opnum61NotUsedOnWire.
    /// </summary>
    public class Opnum61NotUsedOnWireRequest : LsaRequestStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum61NotUsedOnWireRequest()
        {
            Opnum = LsaMethodOpnums.Opnum61NotUsedOnWire;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    }


    /// <summary>
    /// The Opnum61NotUsedOnWireResponse class defines output parameters 
    /// of method Opnum61NotUsedOnWire.
    /// </summary>
    public class Opnum61NotUsedOnWireResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum61NotUsedOnWireResponse()
        {
            Opnum = LsaMethodOpnums.Opnum61NotUsedOnWire;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            return null;
        }
    }


    /// <summary>
    /// The Opnum62NotUsedOnWireRequest class defines input parameters 
    /// of method Opnum62NotUsedOnWire.
    /// </summary>
    public class Opnum62NotUsedOnWireRequest : LsaRequestStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum62NotUsedOnWireRequest()
        {
            Opnum = LsaMethodOpnums.Opnum62NotUsedOnWire;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    }


    /// <summary>
    /// The Opnum62NotUsedOnWireResponse class defines output parameters 
    /// of method Opnum62NotUsedOnWire.
    /// </summary>
    public class Opnum62NotUsedOnWireResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum62NotUsedOnWireResponse()
        {
            Opnum = LsaMethodOpnums.Opnum62NotUsedOnWire;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            return null;
        }
    }


    /// <summary>
    /// The Opnum63NotUsedOnWireRequest class defines input parameters 
    /// of method Opnum63NotUsedOnWire.
    /// </summary>
    public class Opnum63NotUsedOnWireRequest : LsaRequestStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum63NotUsedOnWireRequest()
        {
            Opnum = LsaMethodOpnums.Opnum63NotUsedOnWire;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    }


    /// <summary>
    /// The Opnum63NotUsedOnWireResponse class defines output parameters 
    /// of method Opnum63NotUsedOnWire.
    /// </summary>
    public class Opnum63NotUsedOnWireResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum63NotUsedOnWireResponse()
        {
            Opnum = LsaMethodOpnums.Opnum63NotUsedOnWire;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            return null;
        }
    }


    /// <summary>
    /// The Opnum64NotUsedOnWireRequest class defines input parameters 
    /// of method Opnum64NotUsedOnWire.
    /// </summary>
    public class Opnum64NotUsedOnWireRequest : LsaRequestStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum64NotUsedOnWireRequest()
        {
            Opnum = LsaMethodOpnums.Opnum64NotUsedOnWire;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    }


    /// <summary>
    /// The Opnum64NotUsedOnWireResponse class defines output parameters 
    /// of method Opnum64NotUsedOnWire.
    /// </summary>
    public class Opnum64NotUsedOnWireResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum64NotUsedOnWireResponse()
        {
            Opnum = LsaMethodOpnums.Opnum64NotUsedOnWire;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            return null;
        }
    }


    /// <summary>
    /// The Opnum65NotUsedOnWireRequest class defines input parameters 
    /// of method Opnum65NotUsedOnWire.
    /// </summary>
    public class Opnum65NotUsedOnWireRequest : LsaRequestStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum65NotUsedOnWireRequest()
        {
            Opnum = LsaMethodOpnums.Opnum65NotUsedOnWire;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    }


    /// <summary>
    /// The Opnum65NotUsedOnWireResponse class defines output parameters 
    /// of method Opnum65NotUsedOnWire.
    /// </summary>
    public class Opnum65NotUsedOnWireResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum65NotUsedOnWireResponse()
        {
            Opnum = LsaMethodOpnums.Opnum65NotUsedOnWire;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            return null;
        }
    }


    /// <summary>
    /// The Opnum66NotUsedOnWireRequest class defines input parameters
    /// of method Opnum66NotUsedOnWire.
    /// </summary>
    public class Opnum66NotUsedOnWireRequest : LsaRequestStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum66NotUsedOnWireRequest()
        {
            Opnum = LsaMethodOpnums.Opnum66NotUsedOnWire;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    }


    /// <summary>
    /// The Opnum66NotUsedOnWireResponse class defines output parameters 
    /// of method Opnum66NotUsedOnWire.
    /// </summary>
    public class Opnum66NotUsedOnWireResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum66NotUsedOnWireResponse()
        {
            Opnum = LsaMethodOpnums.Opnum66NotUsedOnWire;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            return null;
        }
    }


    /// <summary>
    /// The Opnum67NotUsedOnWireRequest class defines input parameters 
    /// of method Opnum67NotUsedOnWire.
    /// </summary>
    public class Opnum67NotUsedOnWireRequest : LsaRequestStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum67NotUsedOnWireRequest()
        {
            Opnum = LsaMethodOpnums.Opnum67NotUsedOnWire;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    }


    /// <summary>
    /// The Opnum67NotUsedOnWireResponse class defines output parameters
    /// of method Opnum67NotUsedOnWire.
    /// </summary>
    public class Opnum67NotUsedOnWireResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum67NotUsedOnWireResponse()
        {
            Opnum = LsaMethodOpnums.Opnum67NotUsedOnWire;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            return null;
        }
    }


    /// <summary>
    /// The LsarLookupNames3Request class defines input parameters 
    /// of method LsarLookupNames3.
    /// </summary>
    public class LsarLookupNames3Request : LsaRequestStub
    {
        /// <summary>
        ///  PolicyHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        ///  Count parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint Count;

        /// <summary>
        ///  Names parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_UNICODE_STRING? Names;

        /// <summary>
        ///  TranslatedSids parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_TRANSLATED_SIDS_EX2? TranslatedSids;

        /// <summary>
        ///  LookupLevel parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _LSAP_LOOKUP_LEVEL LookupLevel;

        /// <summary>
        ///  MappedCount parameter.
        /// </summary>
        [CLSCompliant(false)]
        public UInt32? MappedCount;

        /// <summary>
        ///  LookupOptions parameter.
        /// </summary>
        [CLSCompliant(false)]
        public LookupOptions_Values LookupOptions;

        /// <summary>
        ///  ClientRevision parameter.
        /// </summary>
        [CLSCompliant(false)]
        public ClientRevision_Values ClientRevision;

        /// <summary>
        ///  Constructor method
        /// </summary>
        public LsarLookupNames3Request()
        {
            Opnum = LsaMethodOpnums.LsarLookupNames3;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                LsaRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] { 
                    new RpceStubExprEval(LsaRpcAdapter.lsarpc__LSAPR_ACLExprEval_0000) },
                    LsaRpcStubFormatString.ProcFormatString,
                    LsaRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    requestStub,
                    sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                Count = outParamList[1].ToUInt32();
                Names = TypeMarshal.ToNullableStruct<_RPC_UNICODE_STRING>(outParamList[2], null, Count, null);
                TranslatedSids = TypeMarshal.ToNullableStruct<_LSAPR_TRANSLATED_SIDS_EX2>(outParamList[4]);
                LookupLevel = (_LSAP_LOOKUP_LEVEL)(outParamList[5].ToUInt32());
                MappedCount = TypeMarshal.ToNullableStruct<uint>(outParamList[6]);
                LookupOptions = (LookupOptions_Values)(outParamList[7].ToUInt32());
                ClientRevision = (ClientRevision_Values)(outParamList[8].ToUInt32());
            }
        }
    }


    /// <summary>
    /// The LsarLookupNames3Response class defines output parameters 
    /// of method LsarLookupNames3.
    /// </summary>
    public class LsarLookupNames3Response : LsaResponseStub
    {
        /// <summary>
        ///  ReferencedDomains parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_REFERENCED_DOMAIN_LIST ReferencedDomains;

        /// <summary>
        ///  TranslatedSids parameter.
        /// </summary>        
        [CLSCompliant(false)]
        public _LSAPR_TRANSLATED_SIDS_EX2 TranslatedSids;

        /// <summary>
        ///  MappedCount parameter.
        /// </summary>
        [CLSCompliant(false)]
        public UInt32? MappedCount;

        /// <summary>
        ///  Constructor method
        /// </summary>
        public LsarLookupNames3Response()
        {
            Opnum = LsaMethodOpnums.LsarLookupNames3;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            SafeIntPtr pReferencedDomains = TypeMarshal.ToIntPtr(ReferencedDomains);
            SafeIntPtr ppReferencedDomains = TypeMarshal.ToIntPtr(pReferencedDomains.Value);
            SafeIntPtr pTranslatedSids = TypeMarshal.ToIntPtr(TranslatedSids);
            SafeIntPtr pMappedCount = TypeMarshal.ToIntPtr(MappedCount);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ppReferencedDomains,
                    pTranslatedSids,
                    IntPtr.Zero,
                    pMappedCount,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    (uint)Status
                };

                return RpceStubEncoder.ToBytes(
                  RpceStubHelper.GetPlatform(),
                    LsaRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { 
                        new RpceStubExprEval(LsaRpcAdapter.lsarpc__LSAPR_ACLExprEval_0000) },
                    LsaRpcStubFormatString.ProcFormatString,
                    LsaRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pReferencedDomains.Dispose();
                ppReferencedDomains.Dispose();
                pTranslatedSids.Dispose();
                pMappedCount.Dispose();
            }
        }
    }


    /// <summary>
    /// The Opnum69NotUsedOnWireRequest class defines input parameters 
    /// of method Opnum69NotUsedOnWire.
    /// </summary>
    public class Opnum69NotUsedOnWireRequest : LsaRequestStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum69NotUsedOnWireRequest()
        {
            Opnum = LsaMethodOpnums.Opnum69NotUsedOnWire;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    }


    /// <summary>
    /// The Opnum69NotUsedOnWireResponse class defines output parameters
    /// of method Opnum69NotUsedOnWire.
    /// </summary>
    public class Opnum69NotUsedOnWireResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum69NotUsedOnWireResponse()
        {
            Opnum = LsaMethodOpnums.Opnum69NotUsedOnWire;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            return null;
        }
    }


    /// <summary>
    /// The Opnum70NotUsedOnWireRequest class defines input parameters
    /// of method Opnum70NotUsedOnWire.
    /// </summary>
    public class Opnum70NotUsedOnWireRequest : LsaRequestStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum70NotUsedOnWireRequest()
        {
            Opnum = LsaMethodOpnums.Opnum70NotUsedOnWire;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    }


    /// <summary>
    /// The Opnum70NotUsedOnWireResponse class defines output parameters
    /// of method Opnum70NotUsedOnWire.
    /// </summary>
    public class Opnum70NotUsedOnWireResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum70NotUsedOnWireResponse()
        {
            Opnum = LsaMethodOpnums.Opnum70NotUsedOnWire;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            return null;
        }
    }


    /// <summary>
    /// The Opnum71NotUsedOnWireRequest class defines input parameters
    /// of method Opnum71NotUsedOnWire.
    /// </summary>
    public class Opnum71NotUsedOnWireRequest : LsaRequestStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum71NotUsedOnWireRequest()
        {
            Opnum = LsaMethodOpnums.Opnum71NotUsedOnWire;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    }


    /// <summary>
    /// The Opnum71NotUsedOnWireResponse class defines output parameters 
    /// of method Opnum71NotUsedOnWire.
    /// </summary>
    public class Opnum71NotUsedOnWireResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum71NotUsedOnWireResponse()
        {
            Opnum = LsaMethodOpnums.Opnum71NotUsedOnWire;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            return null;
        }
    }


    /// <summary>
    /// The Opnum72NotUsedOnWireRequest class defines input parameters 
    /// of method Opnum72NotUsedOnWire.
    /// </summary>
    public class Opnum72NotUsedOnWireRequest : LsaRequestStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum72NotUsedOnWireRequest()
        {
            Opnum = LsaMethodOpnums.Opnum72NotUsedOnWire;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    }


    /// <summary>
    /// The Opnum72NotUsedOnWireResponse class defines output parameters
    /// of method Opnum72NotUsedOnWire.
    /// </summary>
    public class Opnum72NotUsedOnWireResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum72NotUsedOnWireResponse()
        {
            Opnum = LsaMethodOpnums.Opnum72NotUsedOnWire;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            return null;
        }
    }


    /// <summary>
    /// The Opnum75NotUsedOnWireRequest class defines input parameters
    /// of method Opnum75NotUsedOnWire.
    /// </summary>
    public class Opnum75NotUsedOnWireRequest : LsaRequestStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum75NotUsedOnWireRequest()
        {
            Opnum = LsaMethodOpnums.Opnum75NotUsedOnWire;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    }


    /// <summary>
    /// The Opnum75NotUsedOnWireResponse class defines output parameters
    /// of method Opnum75NotUsedOnWire.
    /// </summary>
    public class Opnum75NotUsedOnWireResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum75NotUsedOnWireResponse()
        {
            Opnum = LsaMethodOpnums.Opnum75NotUsedOnWire;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            return null;
        }
    }


    /// <summary>
    /// The LsarLookupSids3Request class defines input parameters
    /// of method LsarLookupSids3.
    /// </summary>
    public class LsarLookupSids3Request : LsaRequestStub
    {
        /// <summary>
        /// RPC handle.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr RpcHandle;

        /// <summary>
        ///  SidEnumBuffer parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_SID_ENUM_BUFFER? SidEnumBuffer;

        /// <summary>
        ///  TranslatedNames parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_TRANSLATED_NAMES_EX? TranslatedNames;

        /// <summary>
        ///  LookupLevel parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _LSAP_LOOKUP_LEVEL LookupLevel;

        /// <summary>
        ///  MappedCount parameter.
        /// </summary>
        [CLSCompliant(false)]
        public UInt32? MappedCount;

        /// <summary>
        ///  LookupOptions parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint LookupOptions;

        /// <summary>
        ///  ClientRevision parameter.
        /// </summary>
        [CLSCompliant(false)]
        public ClientRevision_Values ClientRevision;

        /// <summary>
        ///  Constructor method
        /// </summary>
        public LsarLookupSids3Request()
        {
            Opnum = LsaMethodOpnums.LsarLookupSids3;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                LsaRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] { 
                    new RpceStubExprEval(LsaRpcAdapter.lsarpc__LSAPR_ACLExprEval_0000) },
                    LsaRpcStubFormatString.ProcFormatString,
                    LsaRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    requestStub,
                    sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                RpcHandle = sessionContext.RpceLayerSessionContext.Handle;
                SidEnumBuffer = TypeMarshal.ToNullableStruct<_LSAPR_SID_ENUM_BUFFER>(outParamList[0]);
                TranslatedNames = TypeMarshal.ToNullableStruct<_LSAPR_TRANSLATED_NAMES_EX>(outParamList[2]);
                LookupLevel = (_LSAP_LOOKUP_LEVEL)(outParamList[3].ToUInt32());
                MappedCount = TypeMarshal.ToNullableStruct<uint>(outParamList[4]);
                LookupOptions = outParamList[5].ToUInt32();
                ClientRevision = (ClientRevision_Values)(outParamList[6].ToUInt32());
            }
        }
    }


    /// <summary>
    /// The LsarLookupSids3Response class defines output parameters 
    /// of method LsarLookupSids3.
    /// </summary>
    public class LsarLookupSids3Response : LsaResponseStub
    {
        /// <summary>
        ///  ReferencedDomains parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_REFERENCED_DOMAIN_LIST ReferencedDomains;

        /// <summary>
        ///  TranslatedNames parameter.
        /// </summary>        
        [CLSCompliant(false)]
        public _LSAPR_TRANSLATED_NAMES_EX TranslatedNames;

        /// <summary>
        ///  MappedCount parameter.
        /// </summary>
        [CLSCompliant(false)]
        public UInt32? MappedCount;

        /// <summary>
        ///  Constructor method
        /// </summary>
        public LsarLookupSids3Response()
        {
            Opnum = LsaMethodOpnums.LsarLookupSids3;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            SafeIntPtr pReferencedDomains = TypeMarshal.ToIntPtr(ReferencedDomains);
            SafeIntPtr ppReferencedDomains = TypeMarshal.ToIntPtr(pReferencedDomains.Value);
            SafeIntPtr pTranslatedNames = TypeMarshal.ToIntPtr(TranslatedNames);
            SafeIntPtr pMappedCount = TypeMarshal.ToIntPtr(MappedCount);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    ppReferencedDomains,
                    pTranslatedNames,
                    IntPtr.Zero,
                    pMappedCount,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    (uint)Status
                };

                return RpceStubEncoder.ToBytes(
                  RpceStubHelper.GetPlatform(),
                    LsaRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { 
                        new RpceStubExprEval(LsaRpcAdapter.lsarpc__LSAPR_ACLExprEval_0000) },
                    LsaRpcStubFormatString.ProcFormatString,
                    LsaRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pReferencedDomains.Dispose();
                ppReferencedDomains.Dispose();
                pTranslatedNames.Dispose();
                pMappedCount.Dispose();
            }
        }
    }


    /// <summary>
    /// The LsarLookupNames4Request class defines input parameters 
    /// of method LsarLookupNames4.
    /// </summary>
    public class LsarLookupNames4Request : LsaRequestStub
    {
        /// <summary>
        /// RPC handle.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr RpcHandle;

        /// <summary>
        ///  Count parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint Count;

        /// <summary>
        ///  Names parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_UNICODE_STRING? Names;

        /// <summary>
        ///  TranslatedSids parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_TRANSLATED_SIDS_EX2? TranslatedSids;

        /// <summary>
        ///  LookupLevel parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _LSAP_LOOKUP_LEVEL LookupLevel;

        /// <summary>
        ///  MappedCount parameter.
        /// </summary>
        [CLSCompliant(false)]
        public UInt32? MappedCount;

        /// <summary>
        ///  LookupOptions parameter.
        /// </summary>
        [CLSCompliant(false)]
        public LookupOptions_Values LookupOptions;

        /// <summary>
        ///  ClientRevision parameter.
        /// </summary>
        [CLSCompliant(false)]
        public ClientRevision_Values ClientRevision;

        /// <summary>
        ///  Constructor method
        /// </summary>
        public LsarLookupNames4Request()
        {
            Opnum = LsaMethodOpnums.LsarLookupNames4;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
              RpceStubHelper.GetPlatform(),
                LsaRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[] { 
                    new RpceStubExprEval(LsaRpcAdapter.lsarpc__LSAPR_ACLExprEval_0000) },
                    LsaRpcStubFormatString.ProcFormatString,
                    LsaRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    requestStub,
                    sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                RpcHandle = sessionContext.RpceLayerSessionContext.Handle;
                Count = outParamList[0].ToUInt32();
                Names = TypeMarshal.ToNullableStruct<_RPC_UNICODE_STRING>(outParamList[1], null, Count, null);
                TranslatedSids = TypeMarshal.ToNullableStruct<_LSAPR_TRANSLATED_SIDS_EX2>(outParamList[3]);
                LookupLevel = (_LSAP_LOOKUP_LEVEL)(outParamList[4].ToInt32());
                MappedCount = TypeMarshal.ToNullableStruct<uint>(outParamList[5]);
                LookupOptions = (LookupOptions_Values)(outParamList[6].ToUInt32());
                ClientRevision = (ClientRevision_Values)(outParamList[7].ToUInt32());
            }
        }
    }


    /// <summary>
    /// The LsarLookupNames4Response class defines output parameters
    /// of method LsarLookupNames4.
    /// </summary>
    public class LsarLookupNames4Response : LsaResponseStub
    {
        /// <summary>
        ///  ReferencedDomains parameter.
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_REFERENCED_DOMAIN_LIST ReferencedDomains;

        /// <summary>
        ///  TranslatedSids parameter.
        /// </summary>        
        [CLSCompliant(false)]
        public _LSAPR_TRANSLATED_SIDS_EX2 TranslatedSids;

        /// <summary>
        ///  MappedCount parameter.
        /// </summary>
        [CLSCompliant(false)]
        public UInt32? MappedCount;

        /// <summary>
        ///  Constructor method
        /// </summary>
        public LsarLookupNames4Response()
        {
            Opnum = LsaMethodOpnums.LsarLookupNames4;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            SafeIntPtr pReferencedDomains = TypeMarshal.ToIntPtr(ReferencedDomains);
            SafeIntPtr ppReferencedDomains = TypeMarshal.ToIntPtr(pReferencedDomains.Value);
            SafeIntPtr pTranslatedSids = TypeMarshal.ToIntPtr(TranslatedSids);
            SafeIntPtr pMappedCount = TypeMarshal.ToIntPtr(MappedCount);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ppReferencedDomains,
                    pTranslatedSids,
                    IntPtr.Zero,
                    pMappedCount,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    (uint)Status
                };

                return RpceStubEncoder.ToBytes(
                  RpceStubHelper.GetPlatform(),
                    LsaRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { 
                        new RpceStubExprEval(LsaRpcAdapter.lsarpc__LSAPR_ACLExprEval_0000) },
                    LsaRpcStubFormatString.ProcFormatString,
                    LsaRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pReferencedDomains.Dispose();
                ppReferencedDomains.Dispose();
                pTranslatedSids.Dispose();
                pMappedCount.Dispose();
            }
        }
    }

    #region LSAD
    /// <summary>
    /// The LsarEnumeratePrivilegesRequest class defines input parameters
    /// of method LsarEnumeratePrivileges.
    /// </summary>
    public class LsarEnumeratePrivilegesRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// EnumerationContext parameter
        /// </summary>
        [CLSCompliant(false)]
        public UInt32? EnumerationContext;

        /// <summary>
        /// PreferedMaximumLength parameter
        /// </summary>
        [CLSCompliant(false)]
        public uint PreferedMaximumLength;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarEnumeratePrivilegesRequest()
        {
            Opnum = LsaMethodOpnums.LsarEnumeratePrivileges;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                EnumerationContext = outParamList[1].ToUInt32();
                PreferedMaximumLength = outParamList[3].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The LsarEnumeratePrivilegesResponse class defines output parameters
    /// of method LsarEnumeratePrivileges.
    /// </summary>
    public class LsarEnumeratePrivilegesResponse : LsaResponseStub
    {
        /// <summary>
        /// EnumerationContext parameter
        /// </summary>
        [CLSCompliant(false)]
        public UInt32? EnumerationContext;

        /// <summary>
        /// EnumerationBuffer parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_PRIVILEGE_ENUM_BUFFER? EnumerationBuffer;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarEnumeratePrivilegesResponse()
        {
            Opnum = LsaMethodOpnums.LsarEnumeratePrivileges;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            using (SafeIntPtr pEnumerationContext = TypeMarshal.ToIntPtr(EnumerationContext),
                pEnumerationBuffer = TypeMarshal.ToIntPtr(EnumerationBuffer))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    pEnumerationContext,
                    pEnumerationBuffer,
                    IntPtr.Zero,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarQuerySecurityObjectRequest class defines input parameters
    /// of method LsarQuerySecurityObject.
    /// </summary>
    public class LsarQuerySecurityObjectRequest : LsaRequestStub
    {
        /// <summary>
        /// ObjectHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr ObjectHandle;

        /// <summary>
        /// SecurityInformation parameter
        /// </summary>
        [CLSCompliant(false)]
        public SECURITY_INFORMATION SecurityInformation;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarQuerySecurityObjectRequest()
        {
            Opnum = LsaMethodOpnums.LsarQuerySecurityObject;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                ObjectHandle = outParamList[0].ToIntPtr();
                SecurityInformation = (SECURITY_INFORMATION)outParamList[1].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The LsarQuerySecurityObjectResponse class defines output parameters
    /// of method LsarQuerySecurityObject.
    /// </summary>
    public class LsarQuerySecurityObjectResponse : LsaResponseStub
    {
        /// <summary>
        /// SecurityDescriptor parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_SR_SECURITY_DESCRIPTOR? SecurityDescriptor;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarQuerySecurityObjectResponse()
        {
            Opnum = LsaMethodOpnums.LsarQuerySecurityObject;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            using (SafeIntPtr pSecurityDescriptor = TypeMarshal.ToIntPtr(SecurityDescriptor),
                ppSecurityDescriptor = TypeMarshal.ToIntPtr(pSecurityDescriptor.Value))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ppSecurityDescriptor,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarSetSecurityObjectRequest class defines input parameters
    /// of method LsarSetSecurityObject.
    /// </summary>
    public class LsarSetSecurityObjectRequest : LsaRequestStub
    {
        /// <summary>
        /// ObjectHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr ObjectHandle;

        /// <summary>
        /// SecurityInformation parameter
        /// </summary>
        [CLSCompliant(false)]
        public SECURITY_INFORMATION SecurityInformation;

        /// <summary>
        /// SecurityDescriptor parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_SR_SECURITY_DESCRIPTOR? SecurityDescriptor;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarSetSecurityObjectRequest()
        {
            Opnum = LsaMethodOpnums.LsarSetSecurityObject;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                ObjectHandle = outParamList[0].ToIntPtr();
                SecurityInformation = (SECURITY_INFORMATION)outParamList[1].ToUInt32();
                SecurityDescriptor = TypeMarshal.ToNullableStruct<_LSAPR_SR_SECURITY_DESCRIPTOR>(outParamList[2]);
            }
        }
    }


    /// <summary>
    /// The LsarSetSecurityObjectResponse class defines output parameters
    /// of method LsarSetSecurityObject.
    /// </summary>
    public class LsarSetSecurityObjectResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarSetSecurityObjectResponse()
        {
            Opnum = LsaMethodOpnums.LsarSetSecurityObject;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            Int3264[] paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                (uint)Status
            };

            return LsaStubEncodeToBytes(paramList, Opnum);
        }
    }


    /// <summary>
    /// The LsarQueryInformationPolicyRequest class defines input parameters
    /// of method LsarQueryInformationPolicy.
    /// </summary>
    public class LsarQueryInformationPolicyRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// InformationClass parameter
        /// </summary>
        [CLSCompliant(false)]
        public _POLICY_INFORMATION_CLASS InformationClass;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarQueryInformationPolicyRequest()
        {
            Opnum = LsaMethodOpnums.LsarQueryInformationPolicy;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                InformationClass = (_POLICY_INFORMATION_CLASS)outParamList[1].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The LsarQueryInformationPolicyResponse class defines output parameters
    /// of method LsarQueryInformationPolicy.
    /// </summary>
    public class LsarQueryInformationPolicyResponse : LsaResponseStub
    {
        /// <summary>
        /// PolicyInformation parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_POLICY_INFORMATION? PolicyInformation;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarQueryInformationPolicyResponse()
        {
            Opnum = LsaMethodOpnums.LsarQueryInformationPolicy;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidCastException">thrown when last request is not right</exception>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            LsarQueryInformationPolicyRequest request =
                sessionContext.RequestReceived as LsarQueryInformationPolicyRequest;

            if (request == null)
            {
                throw new InvalidCastException("Last received request should be LsarQueryInformationPolicy.");
            }

            using (SafeIntPtr pPolicyInformation = TypeMarshal.ToIntPtr(PolicyInformation,
                request.InformationClass, null, null),
                ppPolicyInformation = TypeMarshal.ToIntPtr(pPolicyInformation.Value))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    (uint)request.InformationClass,
                    ppPolicyInformation,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarSetInformationPolicyRequest class defines input parameters
    /// of method LsarSetInformationPolicy.
    /// </summary>
    public class LsarSetInformationPolicyRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// InformationClass parameter
        /// </summary>
        [CLSCompliant(false)]
        public _POLICY_INFORMATION_CLASS InformationClass;

        /// <summary>
        /// PolicyInformation parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_POLICY_INFORMATION? PolicyInformation;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarSetInformationPolicyRequest()
        {
            Opnum = LsaMethodOpnums.LsarSetInformationPolicy;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                InformationClass = (_POLICY_INFORMATION_CLASS)outParamList[1].ToUInt32();
                PolicyInformation = TypeMarshal.ToNullableStruct<_LSAPR_POLICY_INFORMATION>(
                    outParamList[2], InformationClass, null, null);
            }
        }
    }


    /// <summary>
    /// The LsarSetInformationPolicyResponse class defines output parameters
    /// of method LsarSetInformationPolicy.
    /// </summary>
    public class LsarSetInformationPolicyResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarSetInformationPolicyResponse()
        {
            Opnum = LsaMethodOpnums.LsarSetInformationPolicy;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            Int3264[] paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                (uint)Status
            };

            return LsaStubEncodeToBytes(paramList, Opnum);
        }
    }


    /// <summary>
    /// The LsarCreateAccountRequest class defines input parameters
    /// of method LsarCreateAccount.
    /// </summary>
    public class LsarCreateAccountRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// AccountSid parameter
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_SID? AccountSid;

        /// <summary>
        /// DesiredAccess parameter
        /// </summary>
        [CLSCompliant(false)]
        public ACCESS_MASK DesiredAccess;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarCreateAccountRequest()
        {
            Opnum = LsaMethodOpnums.LsarCreateAccount;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                AccountSid = TypeMarshal.ToNullableStruct<_RPC_SID>(outParamList[1]);
                DesiredAccess = (ACCESS_MASK)outParamList[2].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The LsarCreateAccountResponse class defines output parameters
    /// of method LsarCreateAccount.
    /// </summary>
    public class LsarCreateAccountResponse : LsaResponseStub
    {
        /// <summary>
        /// AccountHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr AccountHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarCreateAccountResponse()
        {
            Opnum = LsaMethodOpnums.LsarCreateAccount;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            using (SafeIntPtr pAccountHandle = TypeMarshal.ToIntPtr(AccountHandle))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pAccountHandle,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarEnumerateAccountsRequest class defines input parameters
    /// of method LsarEnumerateAccounts.
    /// </summary>
    public class LsarEnumerateAccountsRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// EnumerationContext parameter
        /// </summary>
        [CLSCompliant(false)]
        public UInt32? EnumerationContext;

        /// <summary>
        /// PreferedMaximumLength parameter
        /// </summary>
        [CLSCompliant(false)]
        public uint PreferedMaximumLength;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarEnumerateAccountsRequest()
        {
            Opnum = LsaMethodOpnums.LsarEnumerateAccounts;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                EnumerationContext = outParamList[1].ToUInt32();
                PreferedMaximumLength = outParamList[3].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The LsarEnumerateAccountsResponse class defines output parameters
    /// of method LsarEnumerateAccounts.
    /// </summary>
    public class LsarEnumerateAccountsResponse : LsaResponseStub
    {
        /// <summary>
        /// EnumerationContext parameter
        /// </summary>
        [CLSCompliant(false)]
        public UInt32? EnumerationContext;

        /// <summary>
        /// EnumerationBuffer parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_ACCOUNT_ENUM_BUFFER? EnumerationBuffer;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarEnumerateAccountsResponse()
        {
            Opnum = LsaMethodOpnums.LsarEnumerateAccounts;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            using (SafeIntPtr pEnumerationContext = TypeMarshal.ToIntPtr(EnumerationContext),
                pEnumerationBuffer = TypeMarshal.ToIntPtr(EnumerationBuffer))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    pEnumerationContext,
                    pEnumerationBuffer,
                    IntPtr.Zero,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarCreateTrustedDomainRequest class defines input parameters
    /// of method LsarCreateTrustedDomain.
    /// </summary>
    public class LsarCreateTrustedDomainRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// TrustedDomainInformation parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_TRUST_INFORMATION? TrustedDomainInformation;

        /// <summary>
        /// DesiredAccess parameter
        /// </summary>
        [CLSCompliant(false)]
        public ACCESS_MASK DesiredAccess;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarCreateTrustedDomainRequest()
        {
            Opnum = LsaMethodOpnums.LsarCreateTrustedDomain;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                TrustedDomainInformation = TypeMarshal.ToNullableStruct<_LSAPR_TRUST_INFORMATION>(outParamList[1]);
                DesiredAccess = (ACCESS_MASK)outParamList[2].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The LsarCreateTrustedDomainResponse class defines output parameters
    /// of method LsarCreateTrustedDomain.
    /// </summary>
    public class LsarCreateTrustedDomainResponse : LsaResponseStub
    {
        /// <summary>
        /// TrustedDomainHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr TrustedDomainHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarCreateTrustedDomainResponse()
        {
            Opnum = LsaMethodOpnums.LsarCreateTrustedDomain;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            using (SafeIntPtr pTrustedDomainHandle = TypeMarshal.ToIntPtr(TrustedDomainHandle))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pTrustedDomainHandle,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarEnumerateTrustedDomainsRequest class defines input parameters
    /// of method LsarEnumerateTrustedDomains.
    /// </summary>
    public class LsarEnumerateTrustedDomainsRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// EnumerationContext parameter
        /// </summary>
        [CLSCompliant(false)]
        public UInt32? EnumerationContext;

        /// <summary>
        /// PreferedMaximumLength parameter
        /// </summary>
        [CLSCompliant(false)]
        public uint PreferedMaximumLength;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarEnumerateTrustedDomainsRequest()
        {
            Opnum = LsaMethodOpnums.LsarEnumerateTrustedDomains;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                EnumerationContext = outParamList[1].ToUInt32();
                PreferedMaximumLength = outParamList[3].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The LsarEnumerateTrustedDomainsResponse class defines output parameters
    /// of method LsarEnumerateTrustedDomains.
    /// </summary>
    public class LsarEnumerateTrustedDomainsResponse : LsaResponseStub
    {
        /// <summary>
        /// EnumerationContext parameter
        /// </summary>
        [CLSCompliant(false)]
        public UInt32? EnumerationContext;

        /// <summary>
        /// EnumerationBuffer parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_TRUSTED_ENUM_BUFFER? EnumerationBuffer;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarEnumerateTrustedDomainsResponse()
        {
            Opnum = LsaMethodOpnums.LsarEnumerateTrustedDomains;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            using (SafeIntPtr pEnumerationContext = TypeMarshal.ToIntPtr(EnumerationContext),
                pEnumerationBuffer = TypeMarshal.ToIntPtr(EnumerationBuffer))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    pEnumerationContext,
                    pEnumerationBuffer,
                    IntPtr.Zero,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarCreateSecretRequest class defines input parameters
    /// of method LsarCreateSecret.
    /// </summary>
    public class LsarCreateSecretRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// SecretName parameter
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_UNICODE_STRING? SecretName;

        /// <summary>
        /// DesiredAccess parameter
        /// </summary>
        [CLSCompliant(false)]
        public ACCESS_MASK DesiredAccess;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarCreateSecretRequest()
        {
            Opnum = LsaMethodOpnums.LsarCreateSecret;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                SecretName = TypeMarshal.ToNullableStruct<_RPC_UNICODE_STRING>(outParamList[1]);
                DesiredAccess = (ACCESS_MASK)outParamList[3].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The LsarCreateSecretResponse class defines output parameters
    /// of method LsarCreateSecret.
    /// </summary>
    public class LsarCreateSecretResponse : LsaResponseStub
    {
        /// <summary>
        /// SecretHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr SecretHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarCreateSecretResponse()
        {
            Opnum = LsaMethodOpnums.LsarCreateSecret;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            using (SafeIntPtr pSecretHandle = TypeMarshal.ToIntPtr(SecretHandle))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pSecretHandle,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarOpenAccountRequest class defines input parameters
    /// of method LsarOpenAccount.
    /// </summary>
    public class LsarOpenAccountRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// AccountSid parameter
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_SID? AccountSid;

        /// <summary>
        /// DesiredAccess parameter
        /// </summary>
        [CLSCompliant(false)]
        public ACCESS_MASK DesiredAccess;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarOpenAccountRequest()
        {
            Opnum = LsaMethodOpnums.LsarOpenAccount;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                AccountSid = TypeMarshal.ToNullableStruct<_RPC_SID>(outParamList[1]);
                DesiredAccess = (ACCESS_MASK)outParamList[3].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The LsarOpenAccountResponse class defines output parameters
    /// of method LsarOpenAccount.
    /// </summary>
    public class LsarOpenAccountResponse : LsaResponseStub
    {
        /// <summary>
        /// AccountHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr AccountHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarOpenAccountResponse()
        {
            Opnum = LsaMethodOpnums.LsarOpenAccount;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            using (SafeIntPtr pAccountHandle = TypeMarshal.ToIntPtr(AccountHandle))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pAccountHandle,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarEnumeratePrivilegesAccountRequest class defines input parameters
    /// of method LsarEnumeratePrivilegesAccount.
    /// </summary>
    public class LsarEnumeratePrivilegesAccountRequest : LsaRequestStub
    {
        /// <summary>
        /// AccountHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr AccountHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarEnumeratePrivilegesAccountRequest()
        {
            Opnum = LsaMethodOpnums.LsarEnumeratePrivilegesAccount;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                AccountHandle = outParamList[0].ToIntPtr();
            }
        }
    }


    /// <summary>
    /// The LsarEnumeratePrivilegesAccountResponse class defines output parameters
    /// of method LsarEnumeratePrivilegesAccount.
    /// </summary>
    public class LsarEnumeratePrivilegesAccountResponse : LsaResponseStub
    {
        /// <summary>
        /// Privileges parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_PRIVILEGE_SET? Privileges;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarEnumeratePrivilegesAccountResponse()
        {
            Opnum = LsaMethodOpnums.LsarEnumeratePrivilegesAccount;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            using (SafeIntPtr pPrivileges = TypeMarshal.ToIntPtr(Privileges),
                ppPrivileges = TypeMarshal.ToIntPtr(pPrivileges.Value))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    ppPrivileges,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarAddPrivilegesToAccountRequest class defines input parameters
    /// of method LsarAddPrivilegesToAccount.
    /// </summary>
    public class LsarAddPrivilegesToAccountRequest : LsaRequestStub
    {
        /// <summary>
        /// AccountHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr AccountHandle;

        /// <summary>
        /// Privileges parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_PRIVILEGE_SET? Privileges;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarAddPrivilegesToAccountRequest()
        {
            Opnum = LsaMethodOpnums.LsarAddPrivilegesToAccount;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                AccountHandle = outParamList[0].ToIntPtr();
                Privileges = TypeMarshal.ToNullableStruct<_LSAPR_PRIVILEGE_SET>(outParamList[1]);
            }
        }
    }


    /// <summary>
    /// The LsarAddPrivilegesToAccountResponse class defines output parameters
    /// of method LsarAddPrivilegesToAccount.
    /// </summary>
    public class LsarAddPrivilegesToAccountResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarAddPrivilegesToAccountResponse()
        {
            Opnum = LsaMethodOpnums.LsarAddPrivilegesToAccount;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            Int3264[] paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                (uint)Status
            };

            return LsaStubEncodeToBytes(paramList, Opnum);
        }
    }


    /// <summary>
    /// The LsarRemovePrivilegesFromAccountRequest class defines input parameters
    /// of method LsarRemovePrivilegesFromAccount.
    /// </summary>
    public class LsarRemovePrivilegesFromAccountRequest : LsaRequestStub
    {
        /// <summary>
        /// AccountHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr AccountHandle;

        /// <summary>
        /// AllPrivileges parameter
        /// </summary>
        public byte AllPrivileges;

        /// <summary>
        /// Privileges parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_PRIVILEGE_SET? Privileges;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarRemovePrivilegesFromAccountRequest()
        {
            Opnum = LsaMethodOpnums.LsarRemovePrivilegesFromAccount;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                AccountHandle = outParamList[0].ToIntPtr();
                AllPrivileges = (byte)outParamList[1].ToUInt32();
                Privileges = TypeMarshal.ToNullableStruct<_LSAPR_PRIVILEGE_SET>(outParamList[2]);
            }
        }
    }


    /// <summary>
    /// The LsarRemovePrivilegesFromAccountResponse class defines output parameters
    /// of method LsarRemovePrivilegesFromAccount.
    /// </summary>
    public class LsarRemovePrivilegesFromAccountResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarRemovePrivilegesFromAccountResponse()
        {
            Opnum = LsaMethodOpnums.LsarRemovePrivilegesFromAccount;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            Int3264[] paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                (uint)Status
            };

            return LsaStubEncodeToBytes(paramList, Opnum);
        }
    }


    /// <summary>
    /// The LsarGetSystemAccessAccountRequest class defines input parameters
    /// of method LsarGetSystemAccessAccount.
    /// </summary>
    public class LsarGetSystemAccessAccountRequest : LsaRequestStub
    {
        /// <summary>
        /// AccountHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr AccountHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarGetSystemAccessAccountRequest()
        {
            Opnum = LsaMethodOpnums.LsarGetSystemAccessAccount;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                AccountHandle = outParamList[0].ToIntPtr();
            }
        }
    }


    /// <summary>
    /// The LsarGetSystemAccessAccountResponse class defines output parameters
    /// of method LsarGetSystemAccessAccount.
    /// </summary>
    public class LsarGetSystemAccessAccountResponse : LsaResponseStub
    {
        /// <summary>
        /// SystemAccess parameter
        /// </summary>
        [CLSCompliant(false)]
        public UInt32? SystemAccess;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarGetSystemAccessAccountResponse()
        {
            Opnum = LsaMethodOpnums.LsarGetSystemAccessAccount;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            using (SafeIntPtr pSystemAccess = TypeMarshal.ToIntPtr(SystemAccess))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    pSystemAccess,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarSetSystemAccessAccountRequest class defines input parameters
    /// of method LsarSetSystemAccessAccount.
    /// </summary>
    public class LsarSetSystemAccessAccountRequest : LsaRequestStub
    {
        /// <summary>
        /// AccountHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr AccountHandle;

        /// <summary>
        /// SystemAccess parameter
        /// </summary>
        [CLSCompliant(false)]
        public UInt32? SystemAccess;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarSetSystemAccessAccountRequest()
        {
            Opnum = LsaMethodOpnums.LsarSetSystemAccessAccount;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                AccountHandle = outParamList[0].ToIntPtr();
                SystemAccess = TypeMarshal.ToNullableStruct<uint>(outParamList[1]);
            }
        }
    }


    /// <summary>
    /// The LsarSetSystemAccessAccountResponse class defines output parameters
    /// of method LsarSetSystemAccessAccount.
    /// </summary>
    public class LsarSetSystemAccessAccountResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarSetSystemAccessAccountResponse()
        {
            Opnum = LsaMethodOpnums.LsarSetSystemAccessAccount;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            Int3264[] paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                (uint)Status
            };

            return LsaStubEncodeToBytes(paramList, Opnum);
        }
    }


    /// <summary>
    /// The LsarOpenTrustedDomainRequest class defines input parameters
    /// of method LsarOpenTrustedDomain.
    /// </summary>
    public class LsarOpenTrustedDomainRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// TrustedDomainSid parameter
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_SID? TrustedDomainSid;

        /// <summary>
        /// DesiredAccess parameter
        /// </summary>
        [CLSCompliant(false)]
        public ACCESS_MASK DesiredAccess;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarOpenTrustedDomainRequest()
        {
            Opnum = LsaMethodOpnums.LsarOpenTrustedDomain;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                TrustedDomainSid = TypeMarshal.ToNullableStruct<_RPC_SID>(outParamList[1]);
                DesiredAccess = (ACCESS_MASK)outParamList[2].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The LsarOpenTrustedDomainResponse class defines output parameters
    /// of method LsarOpenTrustedDomain.
    /// </summary>
    public class LsarOpenTrustedDomainResponse : LsaResponseStub
    {
        /// <summary>
        /// TrustedDomainHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr TrustedDomainHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarOpenTrustedDomainResponse()
        {
            Opnum = LsaMethodOpnums.LsarOpenTrustedDomain;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            using (SafeIntPtr pTrustedDomainHandle = TypeMarshal.ToIntPtr(TrustedDomainHandle))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pTrustedDomainHandle,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarQueryInfoTrustedDomainRequest class defines input parameters
    /// of method LsarQueryInfoTrustedDomain.
    /// </summary>
    public class LsarQueryInfoTrustedDomainRequest : LsaRequestStub
    {
        /// <summary>
        /// TrustedDomainHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr TrustedDomainHandle;

        /// <summary>
        /// InformationClass parameter
        /// </summary>
        [CLSCompliant(false)]
        public _TRUSTED_INFORMATION_CLASS InformationClass;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarQueryInfoTrustedDomainRequest()
        {
            Opnum = LsaMethodOpnums.LsarQueryInfoTrustedDomain;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                TrustedDomainHandle = outParamList[0].ToIntPtr();
                InformationClass = (_TRUSTED_INFORMATION_CLASS)outParamList[1].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The LsarQueryInfoTrustedDomainResponse class defines output parameters
    /// of method LsarQueryInfoTrustedDomain.
    /// </summary>
    public class LsarQueryInfoTrustedDomainResponse : LsaResponseStub
    {
        /// <summary>
        /// TrustedDomainInformation parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_TRUSTED_DOMAIN_INFO? TrustedDomainInformation;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarQueryInfoTrustedDomainResponse()
        {
            Opnum = LsaMethodOpnums.LsarQueryInfoTrustedDomain;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidCastException">thrown when last request is not right</exception>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            LsarQueryInfoTrustedDomainRequest request =
                sessionContext.RequestReceived as LsarQueryInfoTrustedDomainRequest;

            if (request == null)
            {
                throw new InvalidCastException("Last received request should be LsarQueryInfoTrustedDomain.");
            }

            using (SafeIntPtr pTrustedDomainInformation = TypeMarshal.ToIntPtr(TrustedDomainInformation,
                request.InformationClass, null, null),
                ppTrustedDomainInformation = TypeMarshal.ToIntPtr(pTrustedDomainInformation.Value))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    (uint)request.InformationClass,
                    ppTrustedDomainInformation,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarSetInformationTrustedDomainRequest class defines input parameters
    /// of method LsarSetInformationTrustedDomain.
    /// </summary>
    public class LsarSetInformationTrustedDomainRequest : LsaRequestStub
    {
        /// <summary>
        /// TrustedDomainHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr TrustedDomainHandle;

        /// <summary>
        /// InformationClass parameter
        /// </summary>
        [CLSCompliant(false)]
        public _TRUSTED_INFORMATION_CLASS InformationClass;

        /// <summary>
        /// TrustedDomainInformation parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_TRUSTED_DOMAIN_INFO? TrustedDomainInformation;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarSetInformationTrustedDomainRequest()
        {
            Opnum = LsaMethodOpnums.LsarSetInformationTrustedDomain;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                TrustedDomainHandle = outParamList[0].ToIntPtr();
                InformationClass = (_TRUSTED_INFORMATION_CLASS)outParamList[1].ToUInt32();
                TrustedDomainInformation = TypeMarshal.ToNullableStruct<_LSAPR_TRUSTED_DOMAIN_INFO>(outParamList[2],
                    InformationClass, null, null);
            }
        }
    }


    /// <summary>
    /// The LsarSetInformationTrustedDomainResponse class defines output parameters
    /// of method LsarSetInformationTrustedDomain.
    /// </summary>
    public class LsarSetInformationTrustedDomainResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarSetInformationTrustedDomainResponse()
        {
            Opnum = LsaMethodOpnums.LsarSetInformationTrustedDomain;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            Int3264[] paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                (uint)Status
            };

            return LsaStubEncodeToBytes(paramList, Opnum);
        }
    }


    /// <summary>
    /// The LsarOpenSecretRequest class defines input parameters
    /// of method LsarOpenSecret.
    /// </summary>
    public class LsarOpenSecretRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// SecretName parameter
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_UNICODE_STRING? SecretName;

        /// <summary>
        /// DesiredAccess parameter
        /// </summary>
        [CLSCompliant(false)]
        public ACCESS_MASK DesiredAccess;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarOpenSecretRequest()
        {
            Opnum = LsaMethodOpnums.LsarOpenSecret;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                SecretName = TypeMarshal.ToNullableStruct<_RPC_UNICODE_STRING>(outParamList[1]);
                DesiredAccess = (ACCESS_MASK)outParamList[2].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The LsarOpenSecretResponse class defines output parameters
    /// of method LsarOpenSecret.
    /// </summary>
    public class LsarOpenSecretResponse : LsaResponseStub
    {
        /// <summary>
        /// SecretHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr SecretHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarOpenSecretResponse()
        {
            Opnum = LsaMethodOpnums.LsarOpenSecret;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            using (SafeIntPtr pSecretHandle = TypeMarshal.ToIntPtr(SecretHandle))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pSecretHandle,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarSetSecretRequest class defines input parameters
    /// of method LsarSetSecret.
    /// </summary>
    public class LsarSetSecretRequest : LsaRequestStub
    {
        /// <summary>
        /// SecretHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr SecretHandle;

        /// <summary>
        /// EncryptedCurrentValue parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_CR_CIPHER_VALUE? EncryptedCurrentValue;

        /// <summary>
        /// EncryptedOldValue parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_CR_CIPHER_VALUE? EncryptedOldValue;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarSetSecretRequest()
        {
            Opnum = LsaMethodOpnums.LsarSetSecret;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                SecretHandle = outParamList[0].ToIntPtr();
                EncryptedCurrentValue = TypeMarshal.ToNullableStruct<_LSAPR_CR_CIPHER_VALUE>(outParamList[1]);
                EncryptedOldValue = TypeMarshal.ToNullableStruct<_LSAPR_CR_CIPHER_VALUE>(outParamList[2]);
            }
        }
    }


    /// <summary>
    /// The LsarSetSecretResponse class defines output parameters
    /// of method LsarSetSecret.
    /// </summary>
    public class LsarSetSecretResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarSetSecretResponse()
        {
            Opnum = LsaMethodOpnums.LsarSetSecret;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            Int3264[] paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                (uint)Status
            };

            return LsaStubEncodeToBytes(paramList, Opnum);
        }
    }


    /// <summary>
    /// The LsarQuerySecretRequest class defines input parameters
    /// of method LsarQuerySecret.
    /// </summary>
    public class LsarQuerySecretRequest : LsaRequestStub
    {
        /// <summary>
        /// SecretHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr SecretHandle;

        /// <summary>
        /// EncryptedCurrentValue parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_CR_CIPHER_VALUE? EncryptedCurrentValue;

        /// <summary>
        /// CurrentValueSetTime parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LARGE_INTEGER? CurrentValueSetTime;

        /// <summary>
        /// EncryptedOldValue parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_CR_CIPHER_VALUE? EncryptedOldValue;

        /// <summary>
        /// OldValueSetTime parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LARGE_INTEGER? OldValueSetTime;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarQuerySecretRequest()
        {
            Opnum = LsaMethodOpnums.LsarQuerySecret;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                SecretHandle = outParamList[0].ToIntPtr();
                EncryptedCurrentValue = TypeMarshal.ToNullableStruct<_LSAPR_CR_CIPHER_VALUE>(Marshal.ReadIntPtr(outParamList[1]));
                CurrentValueSetTime = TypeMarshal.ToNullableStruct<_LARGE_INTEGER>(outParamList[2]);
                EncryptedOldValue = TypeMarshal.ToNullableStruct<_LSAPR_CR_CIPHER_VALUE>(Marshal.ReadIntPtr(outParamList[3]));
                OldValueSetTime = TypeMarshal.ToNullableStruct<_LARGE_INTEGER>(outParamList[4]);
            }
        }
    }


    /// <summary>
    /// The LsarQuerySecretResponse class defines output parameters
    /// of method LsarQuerySecret.
    /// </summary>
    public class LsarQuerySecretResponse : LsaResponseStub
    {
        /// <summary>
        /// EncryptedCurrentValue parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_CR_CIPHER_VALUE? EncryptedCurrentValue;

        /// <summary>
        /// CurrentValueSetTime parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LARGE_INTEGER? CurrentValueSetTime;

        /// <summary>
        /// EncryptedOldValue parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_CR_CIPHER_VALUE? EncryptedOldValue;

        /// <summary>
        /// OldValueSetTime parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LARGE_INTEGER? OldValueSetTime;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarQuerySecretResponse()
        {
            Opnum = LsaMethodOpnums.LsarQuerySecret;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            using (SafeIntPtr pEncryptedCurrentValue = TypeMarshal.ToIntPtr(EncryptedCurrentValue),
                pCurrentValueSetTime = TypeMarshal.ToIntPtr(CurrentValueSetTime),
                pEncryptedOldValue = TypeMarshal.ToIntPtr(EncryptedOldValue),
                pOldValueSetTime = TypeMarshal.ToIntPtr(OldValueSetTime),
                ppEncryptedCurrentValue = TypeMarshal.ToIntPtr(pEncryptedCurrentValue.Value),
                ppEncryptedOldValue = TypeMarshal.ToIntPtr(pEncryptedOldValue.Value))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    ppEncryptedCurrentValue,
                    pCurrentValueSetTime,
                    ppEncryptedOldValue,
                    pOldValueSetTime,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarLookupPrivilegeValueRequest class defines input parameters
    /// of method LsarLookupPrivilegeValue.
    /// </summary>
    public class LsarLookupPrivilegeValueRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// Name parameter
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_UNICODE_STRING? Name;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarLookupPrivilegeValueRequest()
        {
            Opnum = LsaMethodOpnums.LsarLookupPrivilegeValue;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                Name = TypeMarshal.ToNullableStruct<_RPC_UNICODE_STRING>(outParamList[1]);
            }
        }
    }


    /// <summary>
    /// The LsarLookupPrivilegeValueResponse class defines output parameters
    /// of method LsarLookupPrivilegeValue.
    /// </summary>
    public class LsarLookupPrivilegeValueResponse : LsaResponseStub
    {
        /// <summary>
        /// Value parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LUID? Value;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarLookupPrivilegeValueResponse()
        {
            Opnum = LsaMethodOpnums.LsarLookupPrivilegeValue;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            using (SafeIntPtr pValue = TypeMarshal.ToIntPtr(Value))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pValue,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarLookupPrivilegeNameRequest class defines input parameters
    /// of method LsarLookupPrivilegeName.
    /// </summary>
    public class LsarLookupPrivilegeNameRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// Value parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LUID? Value;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarLookupPrivilegeNameRequest()
        {
            Opnum = LsaMethodOpnums.LsarLookupPrivilegeName;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                Value = TypeMarshal.ToNullableStruct<_LUID>(outParamList[1]);
            }
        }
    }


    /// <summary>
    /// The LsarLookupPrivilegeNameResponse class defines output parameters
    /// of method LsarLookupPrivilegeName.
    /// </summary>
    public class LsarLookupPrivilegeNameResponse : LsaResponseStub
    {
        /// <summary>
        /// Name parameter
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_UNICODE_STRING? Name;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarLookupPrivilegeNameResponse()
        {
            Opnum = LsaMethodOpnums.LsarLookupPrivilegeName;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            using (SafeIntPtr pName = TypeMarshal.ToIntPtr(Name),
                ppName = TypeMarshal.ToIntPtr(pName.Value))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ppName,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarLookupPrivilegeDisplayNameRequest class defines input parameters
    /// of method LsarLookupPrivilegeDisplayName.
    /// </summary>
    public class LsarLookupPrivilegeDisplayNameRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// Name parameter
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_UNICODE_STRING? Name;

        /// <summary>
        /// ClientLanguage parameter
        /// </summary>
        public short ClientLanguage;

        /// <summary>
        /// ClientSystemDefaultLanguage parameter
        /// </summary>
        public short ClientSystemDefaultLanguage;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarLookupPrivilegeDisplayNameRequest()
        {
            Opnum = LsaMethodOpnums.LsarLookupPrivilegeDisplayName;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                Name = TypeMarshal.ToNullableStruct<_RPC_UNICODE_STRING>(outParamList[1]);
                ClientLanguage = (short)outParamList[2].ToUInt32();
                ClientSystemDefaultLanguage = (short)outParamList[3].ToUInt32(); ;
            }
        }
    }


    /// <summary>
    /// The LsarLookupPrivilegeDisplayNameResponse class defines output parameters
    /// of method LsarLookupPrivilegeDisplayName.
    /// </summary>
    public class LsarLookupPrivilegeDisplayNameResponse : LsaResponseStub
    {
        /// <summary>
        /// DisplayName parameter
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_UNICODE_STRING? DisplayName;

        /// <summary>
        /// LanguageReturned parameter
        /// </summary>
        [CLSCompliant(false)]
        public UInt16? LanguageReturned;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarLookupPrivilegeDisplayNameResponse()
        {
            Opnum = LsaMethodOpnums.LsarLookupPrivilegeDisplayName;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            using (SafeIntPtr pDisplayName = TypeMarshal.ToIntPtr(DisplayName),
                ppDisplayName = TypeMarshal.ToIntPtr(pDisplayName.Value),
                pLanguageReturned = TypeMarshal.ToIntPtr(LanguageReturned))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ppDisplayName,
                    pLanguageReturned,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarDeleteObjectRequest class defines input parameters
    /// of method LsarDeleteObject.
    /// </summary>
    public class LsarDeleteObjectRequest : LsaRequestStub
    {
        /// <summary>
        /// ObjectHandle parameter
        /// </summary>
        public IntPtr? ObjectHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarDeleteObjectRequest()
        {
            Opnum = LsaMethodOpnums.LsarDeleteObject;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                ObjectHandle = TypeMarshal.ToNullableStruct<IntPtr>(outParamList[0]);
            }
        }
    }


    /// <summary>
    /// The LsarDeleteObjectResponse class defines output parameters
    /// of method LsarDeleteObject.
    /// </summary>
    public class LsarDeleteObjectResponse : LsaResponseStub
    {
        /// <summary>
        /// ObjectHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr ObjectHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarDeleteObjectResponse()
        {
            Opnum = LsaMethodOpnums.LsarDeleteObject;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            using (SafeIntPtr pObjectHandle = TypeMarshal.ToIntPtr(ObjectHandle))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    pObjectHandle,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarEnumerateAccountsWithUserRightRequest class defines input parameters
    /// of method LsarEnumerateAccountsWithUserRight.
    /// </summary>
    public class LsarEnumerateAccountsWithUserRightRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// UserRight parameter
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_UNICODE_STRING? UserRight;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarEnumerateAccountsWithUserRightRequest()
        {
            Opnum = LsaMethodOpnums.LsarEnumerateAccountsWithUserRight;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                UserRight = TypeMarshal.ToNullableStruct<_RPC_UNICODE_STRING>(outParamList[1]);
            }
        }
    }


    /// <summary>
    /// The LsarEnumerateAccountsWithUserRightResponse class defines output parameters
    /// of method LsarEnumerateAccountsWithUserRight.
    /// </summary>
    public class LsarEnumerateAccountsWithUserRightResponse : LsaResponseStub
    {
        /// <summary>
        /// EnumerationBuffer parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_ACCOUNT_ENUM_BUFFER? EnumerationBuffer;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarEnumerateAccountsWithUserRightResponse()
        {
            Opnum = LsaMethodOpnums.LsarEnumerateAccountsWithUserRight;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            using (SafeIntPtr pEnumerationBuffer = TypeMarshal.ToIntPtr(EnumerationBuffer))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pEnumerationBuffer,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarEnumerateAccountRightsRequest class defines input parameters
    /// of method LsarEnumerateAccountRights.
    /// </summary>
    public class LsarEnumerateAccountRightsRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// AccountSid parameter
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_SID? AccountSid;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarEnumerateAccountRightsRequest()
        {
            Opnum = LsaMethodOpnums.LsarEnumerateAccountRights;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                AccountSid = TypeMarshal.ToNullableStruct<_RPC_SID>(outParamList[1]);
            }
        }
    }


    /// <summary>
    /// The LsarEnumerateAccountRightsResponse class defines output parameters
    /// of method LsarEnumerateAccountRights.
    /// </summary>
    public class LsarEnumerateAccountRightsResponse : LsaResponseStub
    {
        /// <summary>
        /// UserRights parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_USER_RIGHT_SET? UserRights;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarEnumerateAccountRightsResponse()
        {
            Opnum = LsaMethodOpnums.LsarEnumerateAccountRights;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            using (SafeIntPtr pUserRights = TypeMarshal.ToIntPtr(UserRights))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pUserRights,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarAddAccountRightsRequest class defines input parameters
    /// of method LsarAddAccountRights.
    /// </summary>
    public class LsarAddAccountRightsRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// AccountSid parameter
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_SID? AccountSid;

        /// <summary>
        /// UserRights parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_USER_RIGHT_SET? UserRights;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarAddAccountRightsRequest()
        {
            Opnum = LsaMethodOpnums.LsarAddAccountRights;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                AccountSid = TypeMarshal.ToNullableStruct<_RPC_SID>(outParamList[1]);
                UserRights = TypeMarshal.ToNullableStruct<_LSAPR_USER_RIGHT_SET>(outParamList[2]);
            }
        }
    }


    /// <summary>
    /// The LsarAddAccountRightsResponse class defines output parameters
    /// of method LsarAddAccountRights.
    /// </summary>
    public class LsarAddAccountRightsResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarAddAccountRightsResponse()
        {
            Opnum = LsaMethodOpnums.LsarAddAccountRights;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            Int3264[] paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                (uint)Status
            };

            return LsaStubEncodeToBytes(paramList, Opnum);
        }
    }


    /// <summary>
    /// The LsarRemoveAccountRightsRequest class defines input parameters
    /// of method LsarRemoveAccountRights.
    /// </summary>
    public class LsarRemoveAccountRightsRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// AccountSid parameter
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_SID? AccountSid;

        /// <summary>
        /// AllRights parameter
        /// </summary>
        public byte AllRights;

        /// <summary>
        /// UserRights parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_USER_RIGHT_SET? UserRights;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarRemoveAccountRightsRequest()
        {
            Opnum = LsaMethodOpnums.LsarRemoveAccountRights;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                AccountSid = TypeMarshal.ToNullableStruct<_RPC_SID>(outParamList[1]);
                AllRights = (byte)outParamList[2].ToUInt32();
                UserRights = TypeMarshal.ToNullableStruct<_LSAPR_USER_RIGHT_SET>(outParamList[3]);
            }
        }
    }


    /// <summary>
    /// The LsarRemoveAccountRightsResponse class defines output parameters
    /// of method LsarRemoveAccountRights.
    /// </summary>
    public class LsarRemoveAccountRightsResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarRemoveAccountRightsResponse()
        {
            Opnum = LsaMethodOpnums.LsarRemoveAccountRights;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            Int3264[] paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                (uint)Status
            };

            return LsaStubEncodeToBytes(paramList, Opnum);
        }
    }


    /// <summary>
    /// The LsarQueryTrustedDomainInfoRequest class defines input parameters
    /// of method LsarQueryTrustedDomainInfo.
    /// </summary>
    public class LsarQueryTrustedDomainInfoRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// TrustedDomainSid parameter
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_SID? TrustedDomainSid;

        /// <summary>
        /// InformationClass parameter
        /// </summary>
        [CLSCompliant(false)]
        public _TRUSTED_INFORMATION_CLASS InformationClass;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarQueryTrustedDomainInfoRequest()
        {
            Opnum = LsaMethodOpnums.LsarQueryTrustedDomainInfo;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                TrustedDomainSid = TypeMarshal.ToNullableStruct<_RPC_SID>(outParamList[1]);
                InformationClass = (_TRUSTED_INFORMATION_CLASS)outParamList[2].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The LsarQueryTrustedDomainInfoResponse class defines output parameters
    /// of method LsarQueryTrustedDomainInfo.
    /// </summary>
    public class LsarQueryTrustedDomainInfoResponse : LsaResponseStub
    {
        /// <summary>
        /// TrustedDomainInformation parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_TRUSTED_DOMAIN_INFO? TrustedDomainInformation;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarQueryTrustedDomainInfoResponse()
        {
            Opnum = LsaMethodOpnums.LsarQueryTrustedDomainInfo;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidCastException">thrown when last request is not right</exception>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            LsarQueryTrustedDomainInfoRequest request =
                sessionContext.RequestReceived as LsarQueryTrustedDomainInfoRequest;

            if (request == null)
            {
                throw new InvalidCastException("Last received request should be LsarQueryTrustedDomainInfo.");
            }

            using (SafeIntPtr pTrustedDomainInformation = TypeMarshal.ToIntPtr(TrustedDomainInformation,
                request.InformationClass, null, null),
                ppTrustedDomainInformation = TypeMarshal.ToIntPtr(pTrustedDomainInformation.Value))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    (uint)request.InformationClass,
                    ppTrustedDomainInformation,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarSetTrustedDomainInfoRequest class defines input parameters
    /// of method LsarSetTrustedDomainInfo.
    /// </summary>
    public class LsarSetTrustedDomainInfoRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// TrustedDomainSid parameter
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_SID? TrustedDomainSid;

        /// <summary>
        /// InformationClass parameter
        /// </summary>
        [CLSCompliant(false)]
        public _TRUSTED_INFORMATION_CLASS InformationClass;

        /// <summary>
        /// TrustedDomainInformation parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_TRUSTED_DOMAIN_INFO? TrustedDomainInformation;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarSetTrustedDomainInfoRequest()
        {
            Opnum = LsaMethodOpnums.LsarSetTrustedDomainInfo;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                TrustedDomainSid = TypeMarshal.ToNullableStruct<_RPC_SID>(outParamList[1]);
                InformationClass = (_TRUSTED_INFORMATION_CLASS)outParamList[2].ToUInt32();
                TrustedDomainInformation = TypeMarshal.ToNullableStruct<_LSAPR_TRUSTED_DOMAIN_INFO>(outParamList[3],
                    InformationClass, null, null);
            }
        }
    }


    /// <summary>
    /// The LsarSetTrustedDomainInfoResponse class defines output parameters
    /// of method LsarSetTrustedDomainInfo.
    /// </summary>
    public class LsarSetTrustedDomainInfoResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarSetTrustedDomainInfoResponse()
        {
            Opnum = LsaMethodOpnums.LsarSetTrustedDomainInfo;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            Int3264[] paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                (uint)Status
            };

            return LsaStubEncodeToBytes(paramList, Opnum);
        }
    }


    /// <summary>
    /// The LsarDeleteTrustedDomainRequest class defines input parameters
    /// of method LsarDeleteTrustedDomain.
    /// </summary>
    public class LsarDeleteTrustedDomainRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// TrustedDomainSid parameter
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_SID? TrustedDomainSid;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarDeleteTrustedDomainRequest()
        {
            Opnum = LsaMethodOpnums.LsarDeleteTrustedDomain;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                TrustedDomainSid = TypeMarshal.ToNullableStruct<_RPC_SID>(outParamList[1]);
            }
        }
    }


    /// <summary>
    /// The LsarDeleteTrustedDomainResponse class defines output parameters
    /// of method LsarDeleteTrustedDomain.
    /// </summary>
    public class LsarDeleteTrustedDomainResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarDeleteTrustedDomainResponse()
        {
            Opnum = LsaMethodOpnums.LsarDeleteTrustedDomain;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            Int3264[] paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                (uint)Status
            };

            return LsaStubEncodeToBytes(paramList, Opnum);
        }
    }


    /// <summary>
    /// The LsarStorePrivateDataRequest class defines input parameters
    /// of method LsarStorePrivateData.
    /// </summary>
    public class LsarStorePrivateDataRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// KeyName parameter
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_UNICODE_STRING? KeyName;

        /// <summary>
        /// EncryptedData parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_CR_CIPHER_VALUE? EncryptedData;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarStorePrivateDataRequest()
        {
            Opnum = LsaMethodOpnums.LsarStorePrivateData;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                KeyName = TypeMarshal.ToNullableStruct<_RPC_UNICODE_STRING>(outParamList[1]);
                EncryptedData = TypeMarshal.ToNullableStruct<_LSAPR_CR_CIPHER_VALUE>(outParamList[2]);
            }
        }
    }


    /// <summary>
    /// The LsarStorePrivateDataResponse class defines output parameters
    /// of method LsarStorePrivateData.
    /// </summary>
    public class LsarStorePrivateDataResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarStorePrivateDataResponse()
        {
            Opnum = LsaMethodOpnums.LsarStorePrivateData;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            Int3264[] paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                (uint)Status
            };

            return LsaStubEncodeToBytes(paramList, Opnum);
        }
    }


    /// <summary>
    /// The LsarRetrievePrivateDataRequest class defines input parameters
    /// of method LsarRetrievePrivateData.
    /// </summary>
    public class LsarRetrievePrivateDataRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// KeyName parameter
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_UNICODE_STRING? KeyName;

        /// <summary>
        /// EncryptedData parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_CR_CIPHER_VALUE? EncryptedData;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarRetrievePrivateDataRequest()
        {
            Opnum = LsaMethodOpnums.LsarRetrievePrivateData;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                KeyName = TypeMarshal.ToNullableStruct<_RPC_UNICODE_STRING>(outParamList[1]);
                EncryptedData = TypeMarshal.ToNullableStruct<_LSAPR_CR_CIPHER_VALUE>(Marshal.ReadIntPtr(outParamList[2]));
            }
        }
    }


    /// <summary>
    /// The LsarRetrievePrivateDataResponse class defines output parameters
    /// of method LsarRetrievePrivateData.
    /// </summary>
    public class LsarRetrievePrivateDataResponse : LsaResponseStub
    {
        /// <summary>
        /// EncryptedData parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_CR_CIPHER_VALUE? EncryptedData;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarRetrievePrivateDataResponse()
        {
            Opnum = LsaMethodOpnums.LsarRetrievePrivateData;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            using (SafeIntPtr pEncryptedData = TypeMarshal.ToIntPtr(EncryptedData),
                ppEncryptedData = TypeMarshal.ToIntPtr(pEncryptedData.Value))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ppEncryptedData,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarQueryInformationPolicy2Request class defines input parameters
    /// of method LsarQueryInformationPolicy2.
    /// </summary>
    public class LsarQueryInformationPolicy2Request : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// InformationClass parameter
        /// </summary>
        [CLSCompliant(false)]
        public _POLICY_INFORMATION_CLASS InformationClass;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarQueryInformationPolicy2Request()
        {
            Opnum = LsaMethodOpnums.LsarQueryInformationPolicy2;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                InformationClass = (_POLICY_INFORMATION_CLASS)outParamList[1].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The LsarQueryInformationPolicy2Response class defines output parameters
    /// of method LsarQueryInformationPolicy2.
    /// </summary>
    public class LsarQueryInformationPolicy2Response : LsaResponseStub
    {
        /// <summary>
        /// PolicyInformation parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_POLICY_INFORMATION? PolicyInformation;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarQueryInformationPolicy2Response()
        {
            Opnum = LsaMethodOpnums.LsarQueryInformationPolicy2;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidCastException">thrown when last request is not right</exception>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            LsarQueryInformationPolicy2Request request =
                sessionContext.RequestReceived as LsarQueryInformationPolicy2Request;

            if (request == null)
            {
                throw new InvalidCastException("Last received request should be LsarQueryInformationPolicy2.");
            }

            using (SafeIntPtr pPolicyInformation = TypeMarshal.ToIntPtr(PolicyInformation, request.InformationClass,
                null, null),
                ppPolicyInformation = TypeMarshal.ToIntPtr(pPolicyInformation.Value))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    (uint)request.InformationClass,
                    ppPolicyInformation,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarSetInformationPolicy2Request class defines input parameters
    /// of method LsarSetInformationPolicy2.
    /// </summary>
    public class LsarSetInformationPolicy2Request : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// InformationClass parameter
        /// </summary>
        [CLSCompliant(false)]
        public _POLICY_INFORMATION_CLASS InformationClass;

        /// <summary>
        /// PolicyInformation parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_POLICY_INFORMATION? PolicyInformation;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarSetInformationPolicy2Request()
        {
            Opnum = LsaMethodOpnums.LsarSetInformationPolicy2;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                InformationClass = (_POLICY_INFORMATION_CLASS)outParamList[1].ToUInt32();
                PolicyInformation = TypeMarshal.ToNullableStruct<_LSAPR_POLICY_INFORMATION>(outParamList[2],
                    InformationClass, null, null);
            }
        }
    }


    /// <summary>
    /// The LsarSetInformationPolicy2Response class defines output parameters
    /// of method LsarSetInformationPolicy2.
    /// </summary>
    public class LsarSetInformationPolicy2Response : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarSetInformationPolicy2Response()
        {
            Opnum = LsaMethodOpnums.LsarSetInformationPolicy2;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            Int3264[] paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                (uint)Status
            };

            return LsaStubEncodeToBytes(paramList, Opnum);
        }
    }


    /// <summary>
    /// The LsarQueryTrustedDomainInfoByNameRequest class defines input parameters
    /// of method LsarQueryTrustedDomainInfoByName.
    /// </summary>
    public class LsarQueryTrustedDomainInfoByNameRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// TrustedDomainName parameter
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_UNICODE_STRING? TrustedDomainName;

        /// <summary>
        /// InformationClass parameter
        /// </summary>
        [CLSCompliant(false)]
        public _TRUSTED_INFORMATION_CLASS InformationClass;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarQueryTrustedDomainInfoByNameRequest()
        {
            Opnum = LsaMethodOpnums.LsarQueryTrustedDomainInfoByName;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                TrustedDomainName = TypeMarshal.ToNullableStruct<_RPC_UNICODE_STRING>(outParamList[1]);
                InformationClass = (_TRUSTED_INFORMATION_CLASS)outParamList[2].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The LsarQueryTrustedDomainInfoByNameResponse class defines output parameters
    /// of method LsarQueryTrustedDomainInfoByName.
    /// </summary>
    public class LsarQueryTrustedDomainInfoByNameResponse : LsaResponseStub
    {
        /// <summary>
        /// TrustedDomainInformation parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_TRUSTED_DOMAIN_INFO? TrustedDomainInformation;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarQueryTrustedDomainInfoByNameResponse()
        {
            Opnum = LsaMethodOpnums.LsarQueryTrustedDomainInfoByName;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidCastException">thrown when last request is not right</exception>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            LsarQueryTrustedDomainInfoByNameRequest request =
                sessionContext.RequestReceived as LsarQueryTrustedDomainInfoByNameRequest;

            if (request == null)
            {
                throw new InvalidCastException("Last received request should be LsarQueryTrustedDomainInfoByName.");
            }

            using (SafeIntPtr pTrustedDomainInformation = TypeMarshal.ToIntPtr(TrustedDomainInformation,
                request.InformationClass, null, null),
                ppTrustedDomainInformation = TypeMarshal.ToIntPtr(pTrustedDomainInformation.Value))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    (uint)request.InformationClass,
                    ppTrustedDomainInformation,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarSetTrustedDomainInfoByNameRequest class defines input parameters
    /// of method LsarSetTrustedDomainInfoByName.
    /// </summary>
    public class LsarSetTrustedDomainInfoByNameRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// TrustedDomainName parameter
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_UNICODE_STRING? TrustedDomainName;

        /// <summary>
        /// InformationClass parameter
        /// </summary>
        [CLSCompliant(false)]
        public _TRUSTED_INFORMATION_CLASS InformationClass;

        /// <summary>
        /// TrustedDomainInformation parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_TRUSTED_DOMAIN_INFO? TrustedDomainInformation;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarSetTrustedDomainInfoByNameRequest()
        {
            Opnum = LsaMethodOpnums.LsarSetTrustedDomainInfoByName;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                TrustedDomainName = TypeMarshal.ToNullableStruct<_RPC_UNICODE_STRING>(outParamList[1]);
                InformationClass = (_TRUSTED_INFORMATION_CLASS)outParamList[2].ToUInt32();
                TrustedDomainInformation = TypeMarshal.ToNullableStruct<_LSAPR_TRUSTED_DOMAIN_INFO>(outParamList[3],
                    InformationClass, null, null);
            }
        }
    }


    /// <summary>
    /// The LsarSetTrustedDomainInfoByNameResponse class defines output parameters
    /// of method LsarSetTrustedDomainInfoByName.
    /// </summary>
    public class LsarSetTrustedDomainInfoByNameResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarSetTrustedDomainInfoByNameResponse()
        {
            Opnum = LsaMethodOpnums.LsarSetTrustedDomainInfoByName;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            Int3264[] paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                (uint)Status
            };

            return LsaStubEncodeToBytes(paramList, Opnum);
        }
    }


    /// <summary>
    /// The LsarEnumerateTrustedDomainsExRequest class defines input parameters
    /// of method LsarEnumerateTrustedDomainsEx.
    /// </summary>
    public class LsarEnumerateTrustedDomainsExRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// EnumerationContext parameter
        /// </summary>
        [CLSCompliant(false)]
        public UInt32? EnumerationContext;

        /// <summary>
        /// PreferedMaximumLength parameter
        /// </summary>
        [CLSCompliant(false)]
        public uint PreferedMaximumLength;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarEnumerateTrustedDomainsExRequest()
        {
            Opnum = LsaMethodOpnums.LsarEnumerateTrustedDomainsEx;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                EnumerationContext = outParamList[1].ToUInt32();
                PreferedMaximumLength = outParamList[3].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The LsarEnumerateTrustedDomainsExResponse class defines output parameters
    /// of method LsarEnumerateTrustedDomainsEx.
    /// </summary>
    public class LsarEnumerateTrustedDomainsExResponse : LsaResponseStub
    {
        /// <summary>
        /// EnumerationContext parameter
        /// </summary>
        [CLSCompliant(false)]
        public UInt32? EnumerationContext;

        /// <summary>
        /// EnumerationBuffer parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_TRUSTED_ENUM_BUFFER_EX? EnumerationBuffer;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarEnumerateTrustedDomainsExResponse()
        {
            Opnum = LsaMethodOpnums.LsarEnumerateTrustedDomainsEx;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            using (SafeIntPtr pEnumerationContext = TypeMarshal.ToIntPtr(EnumerationContext),
                pEnumerationBuffer = TypeMarshal.ToIntPtr(EnumerationBuffer))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    pEnumerationContext,
                    pEnumerationBuffer,
                    IntPtr.Zero,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarCreateTrustedDomainExRequest class defines input parameters
    /// of method LsarCreateTrustedDomainEx.
    /// </summary>
    public class LsarCreateTrustedDomainExRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// TrustedDomainInformation parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_TRUSTED_DOMAIN_INFORMATION_EX? TrustedDomainInformation;

        /// <summary>
        /// AuthenticationInformation parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION? AuthenticationInformation;

        /// <summary>
        /// DesiredAccess parameter
        /// </summary>
        [CLSCompliant(false)]
        public ACCESS_MASK DesiredAccess;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarCreateTrustedDomainExRequest()
        {
            Opnum = LsaMethodOpnums.LsarCreateTrustedDomainEx;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                TrustedDomainInformation =
                    TypeMarshal.ToNullableStruct<_LSAPR_TRUSTED_DOMAIN_INFORMATION_EX>(outParamList[1]);
                AuthenticationInformation =
                    TypeMarshal.ToNullableStruct<_LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION>(outParamList[2]);

                DesiredAccess = (ACCESS_MASK)outParamList[3].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The LsarCreateTrustedDomainExResponse class defines output parameters
    /// of method LsarCreateTrustedDomainEx.
    /// </summary>
    public class LsarCreateTrustedDomainExResponse : LsaResponseStub
    {
        /// <summary>
        /// TrustedDomainHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr TrustedDomainHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarCreateTrustedDomainExResponse()
        {
            Opnum = LsaMethodOpnums.LsarCreateTrustedDomainEx;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            using (SafeIntPtr pTrustedDomainHandle = TypeMarshal.ToIntPtr(TrustedDomainHandle))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pTrustedDomainHandle,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarQueryDomainInformationPolicyRequest class defines input parameters
    /// of method LsarQueryDomainInformationPolicy.
    /// </summary>
    public class LsarQueryDomainInformationPolicyRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// InformationClass parameter
        /// </summary>
        [CLSCompliant(false)]
        public _POLICY_DOMAIN_INFORMATION_CLASS InformationClass;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarQueryDomainInformationPolicyRequest()
        {
            Opnum = LsaMethodOpnums.LsarQueryDomainInformationPolicy;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                InformationClass = (_POLICY_DOMAIN_INFORMATION_CLASS)outParamList[1].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The LsarQueryDomainInformationPolicyResponse class defines output parameters
    /// of method LsarQueryDomainInformationPolicy.
    /// </summary>
    public class LsarQueryDomainInformationPolicyResponse : LsaResponseStub
    {
        /// <summary>
        /// PolicyDomainInformation parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_POLICY_DOMAIN_INFORMATION? PolicyDomainInformation;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarQueryDomainInformationPolicyResponse()
        {
            Opnum = LsaMethodOpnums.LsarQueryDomainInformationPolicy;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        /// <exception cref="InvalidCastException">thrown when last request is not right</exception>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            LsarQueryDomainInformationPolicyRequest request =
                sessionContext.RequestReceived as LsarQueryDomainInformationPolicyRequest;

            if (request == null)
            {
                throw new InvalidCastException("Last received request should be LsarQueryDomainInformationPolicy.");
            }

            using (SafeIntPtr pPolicyDomainInformation = TypeMarshal.ToIntPtr(PolicyDomainInformation,
                request.InformationClass, null, null),
                ppPolicyDomainInformation = TypeMarshal.ToIntPtr(pPolicyDomainInformation.Value))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    (uint)request.InformationClass,
                    ppPolicyDomainInformation,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarSetDomainInformationPolicyRequest class defines input parameters
    /// of method LsarSetDomainInformationPolicy.
    /// </summary>
    public class LsarSetDomainInformationPolicyRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// InformationClass parameter
        /// </summary>
        [CLSCompliant(false)]
        public _POLICY_DOMAIN_INFORMATION_CLASS InformationClass;

        /// <summary>
        /// PolicyDomainInformation parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_POLICY_DOMAIN_INFORMATION? PolicyDomainInformation;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarSetDomainInformationPolicyRequest()
        {
            Opnum = LsaMethodOpnums.LsarSetDomainInformationPolicy;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                InformationClass = (_POLICY_DOMAIN_INFORMATION_CLASS)outParamList[1].ToUInt32();
                PolicyDomainInformation = TypeMarshal.ToNullableStruct<_LSAPR_POLICY_DOMAIN_INFORMATION>(outParamList[2],
                    InformationClass, null, null);
            }
        }
    }


    /// <summary>
    /// The LsarSetDomainInformationPolicyResponse class defines output parameters
    /// of method LsarSetDomainInformationPolicy.
    /// </summary>
    public class LsarSetDomainInformationPolicyResponse : LsaResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarSetDomainInformationPolicyResponse()
        {
            Opnum = LsaMethodOpnums.LsarSetDomainInformationPolicy;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            Int3264[] paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                (uint)Status
            };

            return LsaStubEncodeToBytes(paramList, Opnum);
        }
    }


    /// <summary>
    /// The LsarOpenTrustedDomainByNameRequest class defines input parameters
    /// of method LsarOpenTrustedDomainByName.
    /// </summary>
    public class LsarOpenTrustedDomainByNameRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// TrustedDomainName parameter
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_UNICODE_STRING? TrustedDomainName;

        /// <summary>
        /// DesiredAccess parameter
        /// </summary>
        [CLSCompliant(false)]
        public ACCESS_MASK DesiredAccess;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarOpenTrustedDomainByNameRequest()
        {
            Opnum = LsaMethodOpnums.LsarOpenTrustedDomainByName;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                TrustedDomainName = TypeMarshal.ToNullableStruct<_RPC_UNICODE_STRING>(outParamList[1]);
                DesiredAccess = (ACCESS_MASK)outParamList[2].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The LsarOpenTrustedDomainByNameResponse class defines output parameters
    /// of method LsarOpenTrustedDomainByName.
    /// </summary>
    public class LsarOpenTrustedDomainByNameResponse : LsaResponseStub
    {
        /// <summary>
        /// TrustedDomainHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr TrustedDomainHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarOpenTrustedDomainByNameResponse()
        {
            Opnum = LsaMethodOpnums.LsarOpenTrustedDomainByName;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            using (SafeIntPtr pTrustedDomainHandle = TypeMarshal.ToIntPtr(TrustedDomainHandle))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pTrustedDomainHandle,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarCreateTrustedDomainEx2Request class defines input parameters
    /// of method LsarCreateTrustedDomainEx2.
    /// </summary>
    public class LsarCreateTrustedDomainEx2Request : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// TrustedDomainInformation parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_TRUSTED_DOMAIN_INFORMATION_EX? TrustedDomainInformation;

        /// <summary>
        /// AuthenticationInformation parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION_INTERNAL? AuthenticationInformation;

        /// <summary>
        /// DesiredAccess parameter
        /// </summary>
        [CLSCompliant(false)]
        public ACCESS_MASK DesiredAccess;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarCreateTrustedDomainEx2Request()
        {
            Opnum = LsaMethodOpnums.LsarCreateTrustedDomainEx2;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                TrustedDomainInformation =
                    TypeMarshal.ToNullableStruct<_LSAPR_TRUSTED_DOMAIN_INFORMATION_EX>(outParamList[1]);
                AuthenticationInformation =
                    TypeMarshal.ToNullableStruct<_LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION_INTERNAL>(outParamList[2]);
                DesiredAccess = (ACCESS_MASK)outParamList[3].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The LsarCreateTrustedDomainEx2Response class defines output parameters
    /// of method LsarCreateTrustedDomainEx2.
    /// </summary>
    public class LsarCreateTrustedDomainEx2Response : LsaResponseStub
    {
        /// <summary>
        /// TrustedDomainHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr TrustedDomainHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarCreateTrustedDomainEx2Response()
        {
            Opnum = LsaMethodOpnums.LsarCreateTrustedDomainEx2;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            using (SafeIntPtr pTrustedDomainHandle = TypeMarshal.ToIntPtr(TrustedDomainHandle))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pTrustedDomainHandle,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarQueryForestTrustInformationRequest class defines input parameters
    /// of method LsarQueryForestTrustInformation.
    /// </summary>
    public class LsarQueryForestTrustInformationRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// TrustedDomainName parameter
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_UNICODE_STRING? TrustedDomainName;

        /// <summary>
        /// HighestRecordType parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSA_FOREST_TRUST_RECORD_TYPE HighestRecordType;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarQueryForestTrustInformationRequest()
        {
            Opnum = LsaMethodOpnums.LsarQueryForestTrustInformation;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                TrustedDomainName = TypeMarshal.ToNullableStruct<_RPC_UNICODE_STRING>(outParamList[1]);
                HighestRecordType = (_LSA_FOREST_TRUST_RECORD_TYPE)outParamList[2].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The LsarQueryForestTrustInformationResponse class defines output parameters
    /// of method LsarQueryForestTrustInformation.
    /// </summary>
    public class LsarQueryForestTrustInformationResponse : LsaResponseStub
    {
        /// <summary>
        /// ForestTrustInfo parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSA_FOREST_TRUST_INFORMATION? ForestTrustInfo;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarQueryForestTrustInformationResponse()
        {
            Opnum = LsaMethodOpnums.LsarQueryForestTrustInformation;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            using (SafeIntPtr pForestTrustInfo = TypeMarshal.ToIntPtr(ForestTrustInfo),
                ppForestTrustInfo = TypeMarshal.ToIntPtr(pForestTrustInfo.Value))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ppForestTrustInfo,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }


    /// <summary>
    /// The LsarSetForestTrustInformationRequest class defines input parameters
    /// of method LsarSetForestTrustInformation.
    /// </summary>
    public class LsarSetForestTrustInformationRequest : LsaRequestStub
    {
        /// <summary>
        /// PolicyHandle parameter
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr PolicyHandle;

        /// <summary>
        /// TrustedDomainName parameter
        /// </summary>
        [CLSCompliant(false)]
        public _RPC_UNICODE_STRING? TrustedDomainName;

        /// <summary>
        /// HighestRecordType parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSA_FOREST_TRUST_RECORD_TYPE HighestRecordType;

        /// <summary>
        /// ForestTrustInfo parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSA_FOREST_TRUST_INFORMATION? ForestTrustInfo;

        /// <summary>
        /// CheckOnly parameter
        /// </summary>
        public byte CheckOnly;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarSetForestTrustInformationRequest()
        {
            Opnum = LsaMethodOpnums.LsarSetForestTrustInformation;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(LsaServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = LsaStubDecodeToParamList(sessionContext, requestStub))
            {
                PolicyHandle = outParamList[0].ToIntPtr();
                TrustedDomainName = TypeMarshal.ToNullableStruct<_RPC_UNICODE_STRING>(outParamList[1]);
                HighestRecordType = (_LSA_FOREST_TRUST_RECORD_TYPE)outParamList[2].ToUInt32();
                ForestTrustInfo = TypeMarshal.ToNullableStruct<_LSA_FOREST_TRUST_INFORMATION>(outParamList[3]);
                CheckOnly = (byte)outParamList[4].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The LsarSetForestTrustInformationResponse class defines output parameters
    /// of method LsarSetForestTrustInformation.
    /// </summary>
    public class LsarSetForestTrustInformationResponse : LsaResponseStub
    {
        /// <summary>
        /// CollisionInfo parameter
        /// </summary>
        [CLSCompliant(false)]
        public _LSA_FOREST_TRUST_COLLISION_INFORMATION? CollisionInfo;

        /// <summary>
        /// Constructor method
        /// </summary>
        public LsarSetForestTrustInformationResponse()
        {
            Opnum = LsaMethodOpnums.LsarSetForestTrustInformation;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(LsaServerSessionContext sessionContext)
        {
            using (SafeIntPtr pCollisionInfo = TypeMarshal.ToIntPtr(CollisionInfo),
                ppCollisionInfo = TypeMarshal.ToIntPtr(pCollisionInfo.Value))
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ppCollisionInfo,
                    (uint)Status
                };

                return LsaStubEncodeToBytes(paramList, Opnum);
            }
        }
    }
    #endregion
    #endregion
}
