// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;

//Disable this warning for the public interface check-in.
#pragma warning disable 169

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Samr
{
    #region Samr Messages sent and received

    /// <summary>
    /// Opnums of Samr methods
    /// </summary>
    public enum SamrMethodOpnums : int
    {
        /// <summary>
        /// Opnum of method SamrConnect
        /// </summary>
        SamrConnect = 0,

        /// <summary>
        /// Opnum of method SamrCloseHandle
        /// </summary>
        SamrCloseHandle = 1,

        /// <summary>
        /// Opnum of method SamrSetSecurityObject
        /// </summary>
        SamrSetSecurityObject = 2,

        /// <summary>
        /// Opnum of method SamrQuerySecurityObject
        /// </summary>
        SamrQuerySecurityObject = 3,

        /// <summary>
        /// Opnum of method Opnum4NotUsedOnWire
        /// </summary>
        Opnum4NotUsedOnWire = 4,

        /// <summary>
        /// Opnum of method SamrLookupDomainInSamServer
        /// </summary>
        SamrLookupDomainInSamServer = 5,

        /// <summary>
        /// Opnum of method SamrEnumerateDomainsInSamServer
        /// </summary>
        SamrEnumerateDomainsInSamServer = 6,

        /// <summary>
        /// Opnum of method SamrOpenDomain
        /// </summary>
        SamrOpenDomain = 7,

        /// <summary>
        /// Opnum of method SamrQueryInformationDomain
        /// </summary>
        SamrQueryInformationDomain = 8,

        /// <summary>
        /// Opnum of method SamrSetInformationDomain
        /// </summary>
        SamrSetInformationDomain = 9,

        /// <summary>
        /// Opnum of method SamrCreateGroupInDomain
        /// </summary>
        SamrCreateGroupInDomain = 10,

        /// <summary>
        /// Opnum of method SamrEnumerateGroupsInDomain
        /// </summary>
        SamrEnumerateGroupsInDomain = 11,

        /// <summary>
        /// Opnum of method SamrCreateUserInDomain
        /// </summary>
        SamrCreateUserInDomain = 12,

        /// <summary>
        /// Opnum of method SamrEnumerateUsersInDomain
        /// </summary>
        SamrEnumerateUsersInDomain = 13,

        /// <summary>
        /// Opnum of method SamrCreateAliasInDomain
        /// </summary>
        SamrCreateAliasInDomain = 14,

        /// <summary>
        /// Opnum of method SamrEnumerateAliasesInDomain
        /// </summary>
        SamrEnumerateAliasesInDomain = 15,

        /// <summary>
        /// Opnum of method SamrGetAliasMembership
        /// </summary>
        SamrGetAliasMembership = 16,

        /// <summary>
        /// Opnum of method SamrLookupNamesInDomain
        /// </summary>
        SamrLookupNamesInDomain = 17,

        /// <summary>
        /// Opnum of method SamrLookupIdsInDomain
        /// </summary>
        SamrLookupIdsInDomain = 18,

        /// <summary>
        /// Opnum of method SamrOpenGroup
        /// </summary>
        SamrOpenGroup = 19,

        /// <summary>
        /// Opnum of method SamrQueryInformationGroup
        /// </summary>
        SamrQueryInformationGroup = 20,

        /// <summary>
        /// Opnum of method SamrSetInformationGroup
        /// </summary>
        SamrSetInformationGroup = 21,

        /// <summary>
        /// Opnum of method SamrAddMemberToGroup
        /// </summary>
        SamrAddMemberToGroup = 22,

        /// <summary>
        /// Opnum of method SamrDeleteGroup
        /// </summary>
        SamrDeleteGroup = 23,

        /// <summary>
        /// Opnum of method SamrRemoveMemberFromGroup
        /// </summary>
        SamrRemoveMemberFromGroup = 24,

        /// <summary>
        /// Opnum of method SamrGetMembersInGroup
        /// </summary>
        SamrGetMembersInGroup = 25,

        /// <summary>
        /// Opnum of method SamrSetMemberAttributesOfGroup
        /// </summary>
        SamrSetMemberAttributesOfGroup = 26,

        /// <summary>
        /// Opnum of method SamrOpenAlias
        /// </summary>
        SamrOpenAlias = 27,

        /// <summary>
        /// Opnum of method SamrQueryInformationAlias
        /// </summary>
        SamrQueryInformationAlias = 28,

        /// <summary>
        /// Opnum of method SamrSetInformationAlias
        /// </summary>
        SamrSetInformationAlias = 29,

        /// <summary>
        /// Opnum of method SamrDeleteAlias
        /// </summary>
        SamrDeleteAlias = 30,

        /// <summary>
        /// Opnum of method SamrAddMemberToAlias
        /// </summary>
        SamrAddMemberToAlias = 31,

        /// <summary>
        /// Opnum of method SamrRemoveMemberFromAlias
        /// </summary>
        SamrRemoveMemberFromAlias = 32,

        /// <summary>
        /// Opnum of method SamrGetMembersInAlias
        /// </summary>
        SamrGetMembersInAlias = 33,

        /// <summary>
        /// Opnum of method SamrOpenUser
        /// </summary>
        SamrOpenUser = 34,

        /// <summary>
        /// Opnum of method SamrDeleteUser
        /// </summary>
        SamrDeleteUser = 35,

        /// <summary>
        /// Opnum of method SamrQueryInformationUser
        /// </summary>
        SamrQueryInformationUser = 36,

        /// <summary>
        /// Opnum of method SamrSetInformationUser
        /// </summary>
        SamrSetInformationUser = 37,

        /// <summary>
        /// Opnum of method SamrChangePasswordUser
        /// </summary>
        SamrChangePasswordUser = 38,

        /// <summary>
        /// Opnum of method SamrGetGroupsForUser
        /// </summary>
        SamrGetGroupsForUser = 39,

        /// <summary>
        /// Opnum of method SamrQueryDisplayInformation
        /// </summary>
        SamrQueryDisplayInformation = 40,

        /// <summary>
        /// Opnum of method SamrGetDisplayEnumerationIndex
        /// </summary>
        SamrGetDisplayEnumerationIndex = 41,

        /// <summary>
        /// Opnum of method Opnum42NotUsedOnWire
        /// </summary>
        Opnum42NotUsedOnWire = 42,

        /// <summary>
        /// Opnum of method Opnum43NotUsedOnWire
        /// </summary>
        Opnum43NotUsedOnWire = 43,

        /// <summary>
        /// Opnum of method SamrGetUserDomainPasswordInformation
        /// </summary>
        SamrGetUserDomainPasswordInformation = 44,

        /// <summary>
        /// Opnum of method SamrRemoveMemberFromForeignDomain
        /// </summary>
        SamrRemoveMemberFromForeignDomain = 45,

        /// <summary>
        /// Opnum of method SamrQueryInformationDomain2
        /// </summary>
        SamrQueryInformationDomain2 = 46,

        /// <summary>
        /// Opnum of method SamrQueryInformationUser2
        /// </summary>
        SamrQueryInformationUser2 = 47,

        /// <summary>
        /// Opnum of method SamrQueryDisplayInformation2
        /// </summary>
        SamrQueryDisplayInformation2 = 48,

        /// <summary>
        /// Opnum of method SamrGetDisplayEnumerationIndex2
        /// </summary>
        SamrGetDisplayEnumerationIndex2 = 49,

        /// <summary>
        /// Opnum of method SamrCreateUser2InDomain
        /// </summary>
        SamrCreateUser2InDomain = 50,

        /// <summary>
        /// Opnum of method SamrQueryDisplayInformation3
        /// </summary>
        SamrQueryDisplayInformation3 = 51,

        /// <summary>
        /// Opnum of method SamrAddMultipleMembersToAlias
        /// </summary>
        SamrAddMultipleMembersToAlias = 52,

        /// <summary>
        /// Opnum of method SamrRemoveMultipleMembersFromAlias
        /// </summary>
        SamrRemoveMultipleMembersFromAlias = 53,

        /// <summary>
        /// Opnum of method SamrOemChangePasswordUser2
        /// </summary>
        SamrOemChangePasswordUser2 = 54,

        /// <summary>
        /// Opnum of method SamrUnicodeChangePasswordUser2
        /// </summary>
        SamrUnicodeChangePasswordUser2 = 55,

        /// <summary>
        /// Opnum of method SamrGetDomainPasswordInformation
        /// </summary>
        SamrGetDomainPasswordInformation = 56,

        /// <summary>
        /// Opnum of method SamrConnect2
        /// </summary>
        SamrConnect2 = 57,

        /// <summary>
        /// Opnum of method SamrSetInformationUser2
        /// </summary>
        SamrSetInformationUser2 = 58,

        /// <summary>
        /// Opnum of method Opnum59NotUsedOnWire
        /// </summary>
        Opnum59NotUsedOnWire = 59,

        /// <summary>
        /// Opnum of method Opnum60NotUsedOnWire
        /// </summary>
        Opnum60NotUsedOnWire = 60,

        /// <summary>
        /// Opnum of method Opnum61NotUsedOnWire
        /// </summary>
        Opnum61NotUsedOnWire = 61,

        /// <summary>
        /// Opnum of method SamrConnect4
        /// </summary>
        SamrConnect4 = 62,

        /// <summary>
        /// Opnum of method Opnum63NotUsedOnWire
        /// </summary>
        Opnum63NotUsedOnWire = 63,

        /// <summary>
        /// Opnum of method SamrConnect5
        /// </summary>
        SamrConnect5 = 64,

        /// <summary>
        /// Opnum of method SamrRidToSid
        /// </summary>
        SamrRidToSid = 65,

        /// <summary>
        /// Opnum of method SamrSetDSRMPassword
        /// </summary>
        SamrSetDSRMPassword = 66,

        /// <summary>
        /// Opnum of method SamrValidatePassword
        /// </summary>
        SamrValidatePassword = 67,

        /// <summary>
        /// Opnum of method Opnum68NotUsedOnWire
        /// </summary>
        Opnum68NotUsedOnWire = 68,

        /// <summary>
        /// Opnum of method Opnum69NotUsedOnWire
        /// </summary>
        Opnum69NotUsedOnWire = 69
    }


    /// <summary>
    /// The base class of all Samr request
    /// </summary>
    public abstract class SamrRequestStub
    {
        /// <summary>
        ///  Opnum at RPC layer
        /// </summary>
        private SamrMethodOpnums rpceLayerOpnum;

        /// <summary>
        ///  Opnum at RPC layer
        /// </summary>
        public SamrMethodOpnums Opnum
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
        internal abstract void Decode(SamrServerSessionContext sessionContext, byte[] requestStub);
    }


    /// <summary>
    /// The base class of all Samr response
    /// </summary>
    public abstract class SamrResponseStub
    {
        /// <summary>
        ///  Opnum at RPC layer
        /// </summary>
        private SamrMethodOpnums rpceLayerOpnum;

        /// <summary>
        ///  Opnum at RPC layer
        /// </summary>
        public SamrMethodOpnums Opnum
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
        internal abstract byte[] Encode(SamrServerSessionContext sessionContext);
    }


    #region Structures of input and output parameters of SAMR methods
    /// <summary>
    /// The SamrConnectRequest class defines input parameters of method SamrConnect.
    /// </summary>
    public class SamrConnectRequest : SamrRequestStub
    {
        /// <summary>
        /// ServerName parameter
        /// </summary>
        public string ServerName;

        /// <summary>
        /// DesiredAccess parameter
        /// </summary>
        public uint DesiredAccess;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrConnectRequest()
        {
            Opnum = SamrMethodOpnums.SamrConnect;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
               RpceStubHelper.GetPlatform(),
                SamrRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[]{
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                SamrRpcStubFormatString.ProcFormatString,
                SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                IntPtr pServerName = inParams[0].ToIntPtr();
                ServerName = Marshal.PtrToStringUni(pServerName);
                DesiredAccess = inParams[2].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The SamrConnectResponse class defines output parameters of method SamrConnect.
    /// </summary>
    public class SamrConnectResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the ServerHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr ServerHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrConnectResponse()
        {
            Opnum = SamrMethodOpnums.SamrConnect;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pServerHandle = TypeMarshal.ToIntPtr(ServerHandle);
            paramList = new Int3264[]
            {
                IntPtr.Zero,
                pServerHandle,
                IntPtr.Zero,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pServerHandle.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrCloseHandleRequest class defines input parameters of method SamrCloseHandle.
    /// </summary>
    /// CA1049:TypesThatOwnNativeResourcesShouldBeDisposable suppressed because that the pointer SamHandle should be disposed by user
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1049:TypesThatOwnNativeResourcesShouldBeDisposable")]
    public class SamrCloseHandleRequest : SamrRequestStub
    {
        /// <summary>
        /// the SamHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr SamHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrCloseHandleRequest()
        {
            Opnum = SamrMethodOpnums.SamrCloseHandle;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
               RpceStubHelper.GetPlatform(),
                SamrRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[]{
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                SamrRpcStubFormatString.ProcFormatString,
                SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                IntPtr pSamHandle = inParams[0].ToIntPtr();
                SamHandle = Marshal.ReadIntPtr(pSamHandle);
            }
        }
    }


    /// <summary>
    /// The SamrCloseHandleResponse class defines output parameters of method SamrCloseHandle.
    /// </summary>
    public class SamrCloseHandleResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the SamHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr SamHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrCloseHandleResponse()
        {
            Opnum = SamrMethodOpnums.SamrCloseHandle;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pSamHandle = TypeMarshal.ToIntPtr(SamHandle);
            paramList = new Int3264[]
            {
                pSamHandle,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pSamHandle.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrSetSecurityObjectRequest class defines input parameters of method SamrSetSecurityObject.
    /// </summary>
    public class SamrSetSecurityObjectRequest : SamrRequestStub
    {
        /// <summary>
        /// the ObjectHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr ObjectHandle;

        /// <summary>
        /// the SecurityInformation parameter
        /// </summary>
        public SecurityInformation_Values SecurityInformation;

        /// <summary>
        /// the SecurityDescriptor parameter
        /// </summary>
        public _SAMPR_SR_SECURITY_DESCRIPTOR? SecurityDescriptor;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrSetSecurityObjectRequest()
        {
            Opnum = SamrMethodOpnums.SamrSetSecurityObject;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
               RpceStubHelper.GetPlatform(),
                SamrRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[]{
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                SamrRpcStubFormatString.ProcFormatString,
                SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ObjectHandle = inParams[0].ToIntPtr();
                SecurityInformation = (SecurityInformation_Values)inParams[1].ToUInt32();
                SecurityDescriptor = TypeMarshal.ToNullableStruct<_SAMPR_SR_SECURITY_DESCRIPTOR>(inParams[2]);
            }
        }
    }


    /// <summary>
    /// The SamrSetSecurityObjectResponse class defines output parameters of method SamrSetSecurityObject.
    /// </summary>
    public class SamrSetSecurityObjectResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrSetSecurityObjectResponse()
        {
            Opnum = SamrMethodOpnums.SamrSetSecurityObject;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                Status
            };
            byte[] responseStub = RpceStubEncoder.ToBytes(
               RpceStubHelper.GetPlatform(),
                SamrRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[]{
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                SamrRpcStubFormatString.ProcFormatString,
                SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                paramList);
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrQuerySecurityObjectRequest class defines input parameters of method SamrQuerySecurityObject.
    /// </summary>
    public class SamrQuerySecurityObjectRequest : SamrRequestStub
    {
        /// <summary>
        /// the ObjectHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr ObjectHandle;

        /// <summary>
        /// the SecurityInformation parameter
        /// </summary>
        public SamrQuerySecurityObject_SecurityInformation_Values SecurityInformation;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrQuerySecurityObjectRequest()
        {
            Opnum = SamrMethodOpnums.SamrQuerySecurityObject;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ObjectHandle = inParams[0].ToIntPtr();
                SecurityInformation = (SamrQuerySecurityObject_SecurityInformation_Values)(inParams[1].ToInt32());
            }
        }
    }


    /// <summary>
    /// The SamrQuerySecurityObjectResponse class defines output parameters of method SamrQuerySecurityObject.
    /// </summary>
    public class SamrQuerySecurityObjectResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the SecurityDescriptor parameter
        /// </summary>
        public _SAMPR_SR_SECURITY_DESCRIPTOR? SecurityDescriptor;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrQuerySecurityObjectResponse()
        {
            Opnum = SamrMethodOpnums.SamrQuerySecurityObject;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pSecurityDescriptor = TypeMarshal.ToIntPtr(SecurityDescriptor);
            IntPtr ppSecurityDescriptor = Marshal.AllocHGlobal(IntPtr.Size);
            Marshal.WriteIntPtr(ppSecurityDescriptor, pSecurityDescriptor);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                ppSecurityDescriptor,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pSecurityDescriptor.Dispose();
                Marshal.FreeHGlobal(ppSecurityDescriptor);
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The Opnum4NotUsedOnWireRequest class defines input parameters of method Opnum4NotUsedOnWire.
    /// </summary>
    public class Opnum4NotUsedOnWireRequest : SamrRequestStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum4NotUsedOnWireRequest()
        {
            Opnum = SamrMethodOpnums.Opnum4NotUsedOnWire;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    }


    /// <summary>
    /// The Opnum4NotUsedOnWireResponse class defines output parameters of method Opnum4NotUsedOnWire.
    /// </summary>
    public class Opnum4NotUsedOnWireResponse : SamrResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum4NotUsedOnWireResponse()
        {
            Opnum = SamrMethodOpnums.Opnum4NotUsedOnWire;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            return null;
        }
    }


    /// <summary>
    /// The SamrLookupDomainInSamServerRequest class defines input parameters of method SamrLookupDomainInSamServer.
    /// </summary>
    public class SamrLookupDomainInSamServerRequest : SamrRequestStub
    {
        /// <summary>
        /// the ServerHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr ServerHandle;

        /// <summary>
        /// the Name parameter
        /// </summary>
        public _RPC_UNICODE_STRING Name;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrLookupDomainInSamServerRequest()
        {
            Opnum = SamrMethodOpnums.SamrLookupDomainInSamServer;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ServerHandle = inParams[0].ToIntPtr();
                Name = TypeMarshal.ToStruct<_RPC_UNICODE_STRING>(inParams[1]);
            }
        }
    }


    /// <summary>
    /// The SamrLookupDomainInSamServerResponse class defines output parameters of method SamrLookupDomainInSamServer.
    /// </summary>
    public class SamrLookupDomainInSamServerResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the DomainId parameter
        /// </summary>
        public _RPC_SID? DomainId;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrLookupDomainInSamServerResponse()
        {
            Opnum = SamrMethodOpnums.SamrLookupDomainInSamServer;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pDomainId = TypeMarshal.ToIntPtr(DomainId);
            IntPtr ppDomainId = Marshal.AllocHGlobal(Marshal.SizeOf(IntPtr.Zero));
            Marshal.WriteIntPtr(ppDomainId, pDomainId);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                ppDomainId,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                Marshal.FreeHGlobal(ppDomainId);
                pDomainId.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrEnumerateDomainsInSamServerRequest class defines input parameters of method SamrEnumerateDomainsInSamServer.
    /// </summary>
    public class SamrEnumerateDomainsInSamServerRequest : SamrRequestStub
    {
        /// <summary>
        /// the xxx parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr ServerHandle;

        /// <summary>
        /// the EnumerationContext parameter
        /// </summary>
        public UInt32 EnumerationContext;

        /// <summary>
        /// the PreferedMaximumLength parameter
        /// </summary>
        public uint PreferedMaximumLength;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrEnumerateDomainsInSamServerRequest()
        {
            Opnum = SamrMethodOpnums.SamrEnumerateDomainsInSamServer;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ServerHandle = inParams[0].ToIntPtr();
                EnumerationContext = TypeMarshal.ToStruct<UInt32>(inParams[1]);
                PreferedMaximumLength = inParams[3].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The SamrEnumerateDomainsInSamServerResponse class defines output parameters of method SamrEnumerateDomainsInSamServer.
    /// </summary>
    public class SamrEnumerateDomainsInSamServerResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the EnumerationContext parameter
        /// </summary>
        public UInt32 EnumerationContext;

        /// <summary>
        /// the Buffer parameter
        /// </summary>
        public _SAMPR_ENUMERATION_BUFFER? Buffer;

        /// <summary>
        /// the CountReturned parameter
        /// </summary>
        public UInt32 CountReturned;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrEnumerateDomainsInSamServerResponse()
        {
            Opnum = SamrMethodOpnums.SamrEnumerateDomainsInSamServer;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pEnumerationContext = TypeMarshal.ToIntPtr(EnumerationContext);
            SafeIntPtr pBuffer = TypeMarshal.ToIntPtr(Buffer);
            IntPtr ppBuffer = Marshal.AllocHGlobal(Marshal.SizeOf(IntPtr.Zero));
            Marshal.WriteIntPtr(ppBuffer, pBuffer);
            SafeIntPtr pCountReturned = TypeMarshal.ToIntPtr(CountReturned);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                pEnumerationContext,
                ppBuffer,
                IntPtr.Zero,
                pCountReturned,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pEnumerationContext.Dispose();
                pCountReturned.Dispose();
                pBuffer.Dispose();
                Marshal.FreeHGlobal(ppBuffer);
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrOpenDomainRequest class defines input parameters of method SamrOpenDomain.
    /// </summary>
    public class SamrOpenDomainRequest : SamrRequestStub
    {
        /// <summary>
        /// the ServerHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr ServerHandle;

        /// <summary>
        /// the DesiredAccess parameter
        /// </summary>
        public uint DesiredAccess;

        /// <summary>
        /// the DomainId parameter
        /// </summary>
        public _RPC_SID? DomainId;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrOpenDomainRequest()
        {
            Opnum = SamrMethodOpnums.SamrOpenDomain;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ServerHandle = inParams[0].ToIntPtr();
                DesiredAccess = inParams[1].ToUInt32();
                DomainId = TypeMarshal.ToNullableStruct<_RPC_SID>(inParams[2]);
            }
        }
    }


    /// <summary>
    /// The SamrOpenDomainResponse class defines output parameters of method SamrOpenDomain.
    /// </summary>
    public class SamrOpenDomainResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the DomainHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr DomainHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrOpenDomainResponse()
        {
            Opnum = SamrMethodOpnums.SamrOpenDomain;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pDomainHandle = TypeMarshal.ToIntPtr(DomainHandle);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                pDomainHandle,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pDomainHandle.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrQueryInformationDomainRequest class defines input parameters of method SamrQueryInformationDomain.
    /// </summary>
    public class SamrQueryInformationDomainRequest : SamrRequestStub
    {
        /// <summary>
        /// the DomainHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr DomainHandle;

        /// <summary>
        /// the DomainInformationClass parameter
        /// </summary>
        public _DOMAIN_INFORMATION_CLASS DomainInformationClass;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrQueryInformationDomainRequest()
        {
            Opnum = SamrMethodOpnums.SamrQueryInformationDomain;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                DomainHandle = inParams[0].ToIntPtr();
                DomainInformationClass = (_DOMAIN_INFORMATION_CLASS)(inParams[1].ToInt32());
            }
        }
    }


    /// <summary>
    /// The SamrQueryInformationDomainResponse class defines output parameters of method SamrQueryInformationDomain.
    /// </summary>
    public class SamrQueryInformationDomainResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the DomainInformationClass parameter
        /// </summary>
        public _DOMAIN_INFORMATION_CLASS DomainInformationClass;

        /// <summary>
        /// the Buffer parameter, [Switch("DomainInformationClass")]
        /// </summary>
        public _SAMPR_DOMAIN_INFO_BUFFER? Buffer;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrQueryInformationDomainResponse()
        {
            Opnum = SamrMethodOpnums.SamrQueryInformationDomain;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pBuffer = TypeMarshal.ToIntPtr(Buffer, DomainInformationClass, null, null);
            IntPtr ppBuffer = Marshal.AllocHGlobal(IntPtr.Size);
            Marshal.WriteIntPtr(ppBuffer, pBuffer);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                (int)DomainInformationClass,
                ppBuffer,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pBuffer.Dispose();
                Marshal.FreeHGlobal(ppBuffer);
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrSetInformationDomainRequest class defines input parameters of method SamrSetInformationDomain.
    /// </summary>
    public class SamrSetInformationDomainRequest : SamrRequestStub
    {
        /// <summary>
        /// the DomainHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr DomainHandle;

        /// <summary>
        /// the DomainInformationClass parameter
        /// </summary>
        public _DOMAIN_INFORMATION_CLASS DomainInformationClass;

        /// <summary>
        /// the DomainInformation parameter, [Switch("DomainInformationClass")]
        /// </summary>
        public _SAMPR_DOMAIN_INFO_BUFFER DomainInformation;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrSetInformationDomainRequest()
        {
            Opnum = SamrMethodOpnums.SamrSetInformationDomain;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                DomainHandle = inParams[0].ToIntPtr();
                DomainInformationClass = (_DOMAIN_INFORMATION_CLASS)inParams[1].ToInt32();
                DomainInformation = TypeMarshal.ToStruct<_SAMPR_DOMAIN_INFO_BUFFER>(
                    inParams[2].ToIntPtr(),
                    DomainInformationClass, null, null);
            }
        }
    }


    /// <summary>
    /// The SamrSetInformationDomainResponse class defines output parameters of method SamrSetInformationDomain.
    /// </summary>
    public class SamrSetInformationDomainResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrSetInformationDomainResponse()
        {
            Opnum = SamrMethodOpnums.SamrSetInformationDomain;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;

            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                Status
            };
            byte[] responseStub = RpceStubEncoder.ToBytes(
               RpceStubHelper.GetPlatform(),
                SamrRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[]{
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                SamrRpcStubFormatString.ProcFormatString,
                SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                paramList);
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrCreateGroupInDomainRequest class defines input parameters of method SamrCreateGroupInDomain.
    /// </summary>
    public class SamrCreateGroupInDomainRequest : SamrRequestStub
    {
        /// <summary>
        /// the DomainHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr DomainHandle;

        /// <summary>
        /// the Name parameter
        /// </summary>
        public _RPC_UNICODE_STRING Name;

        /// <summary>
        /// the DesiredAccess parameter
        /// </summary>
        public uint DesiredAccess;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrCreateGroupInDomainRequest()
        {
            Opnum = SamrMethodOpnums.SamrCreateGroupInDomain;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                DomainHandle = inParams[0].ToIntPtr();
                Name = TypeMarshal.ToStruct<_RPC_UNICODE_STRING>(inParams[1]);
                DesiredAccess = inParams[2].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The SamrCreateGroupInDomainResponse class defines output parameters of method SamrCreateGroupInDomain.
    /// </summary>
    public class SamrCreateGroupInDomainResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the GroupHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr GroupHandle;

        /// <summary>
        /// the RelativeId parameter
        /// </summary>
        public UInt32 RelativeId;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrCreateGroupInDomainResponse()
        {
            Opnum = SamrMethodOpnums.SamrCreateGroupInDomain;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pGroupHandle = TypeMarshal.ToIntPtr(GroupHandle);
            SafeIntPtr pRelativeId = TypeMarshal.ToIntPtr(RelativeId);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                pGroupHandle,
                pRelativeId,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pGroupHandle.Dispose();
                pRelativeId.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrEnumerateGroupsInDomainRequest class defines input parameters of method SamrEnumerateGroupsInDomain.
    /// </summary>
    public class SamrEnumerateGroupsInDomainRequest : SamrRequestStub
    {
        /// <summary>
        /// the DomainHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr DomainHandle;

        /// <summary>
        /// the EnumerationContext parameter
        /// </summary>
        public UInt32 EnumerationContext;

        /// <summary>
        /// the PreferedMaximumLength parameter
        /// </summary>
        public uint PreferedMaximumLength;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrEnumerateGroupsInDomainRequest()
        {
            Opnum = SamrMethodOpnums.SamrEnumerateGroupsInDomain;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                DomainHandle = inParams[0].ToIntPtr();
                EnumerationContext = TypeMarshal.ToStruct<uint>(inParams[1]);
                PreferedMaximumLength = inParams[3].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The SamrEnumerateGroupsInDomainResponse class defines output parameters of method SamrEnumerateGroupsInDomain.
    /// </summary>
    public class SamrEnumerateGroupsInDomainResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the EnumerationContext parameter
        /// </summary>
        public UInt32 EnumerationContext;

        /// <summary>
        /// the Buffer parameter
        /// </summary>
        public _SAMPR_ENUMERATION_BUFFER? Buffer;

        /// <summary>
        /// the CountReturned parameter
        /// </summary>
        public UInt32 CountReturned;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrEnumerateGroupsInDomainResponse()
        {
            Opnum = SamrMethodOpnums.SamrEnumerateGroupsInDomain;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pEnumerationContext = TypeMarshal.ToIntPtr<uint>(EnumerationContext);
            SafeIntPtr pBuffer = TypeMarshal.ToIntPtr<_SAMPR_ENUMERATION_BUFFER>(Buffer);
            IntPtr ppBuffer = Marshal.AllocHGlobal(IntPtr.Size);
            Marshal.WriteIntPtr(ppBuffer, pBuffer);
            SafeIntPtr pCountReturned = TypeMarshal.ToIntPtr<uint>(CountReturned);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                pEnumerationContext,
                ppBuffer,
                IntPtr.Zero,
                pCountReturned,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pEnumerationContext.Dispose();
                pCountReturned.Dispose();
                pBuffer.Dispose();
                Marshal.FreeHGlobal(ppBuffer);
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrCreateUserInDomainRequest class defines input parameters of method SamrCreateUserInDomain.
    /// </summary>
    public class SamrCreateUserInDomainRequest : SamrRequestStub
    {
        /// <summary>
        /// the DomainHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr DomainHandle;

        /// <summary>
        /// the Name parameter
        /// </summary>
        public _RPC_UNICODE_STRING Name;

        /// <summary>
        /// the DesiredAccess parameter
        /// </summary>
        public uint DesiredAccess;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrCreateUserInDomainRequest()
        {
            Opnum = SamrMethodOpnums.SamrCreateUserInDomain;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                DomainHandle = inParams[0].ToIntPtr();
                Name = TypeMarshal.ToStruct<_RPC_UNICODE_STRING>(inParams[1]);
                DesiredAccess = inParams[2].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The SamrCreateUserInDomainResponse class defines output parameters of method SamrCreateUserInDomain.
    /// </summary>
    public class SamrCreateUserInDomainResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the UserHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr UserHandle;

        /// <summary>
        /// the RelativeId parameter
        /// </summary>
        public UInt32 RelativeId;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrCreateUserInDomainResponse()
        {
            Opnum = SamrMethodOpnums.SamrCreateUserInDomain;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pUserHandle = TypeMarshal.ToIntPtr(UserHandle);
            SafeIntPtr pRelativeId = TypeMarshal.ToIntPtr(RelativeId);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                pUserHandle,
                pRelativeId,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pUserHandle.Dispose();
                pRelativeId.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrEnumerateUsersInDomainRequest class defines input parameters of method SamrEnumerateUsersInDomain.
    /// </summary>
    public class SamrEnumerateUsersInDomainRequest : SamrRequestStub
    {
        /// <summary>
        /// the DomainHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr DomainHandle;

        /// <summary>
        /// the EnumerationContext parameter
        /// </summary>
        public UInt32 EnumerationContext;

        /// <summary>
        /// the UserAccountControl parameter
        /// </summary>
        public uint UserAccountControl;

        /// <summary>
        /// the PreferedMaximumLength parameter
        /// </summary>
        public uint PreferedMaximumLength;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrEnumerateUsersInDomainRequest()
        {
            Opnum = SamrMethodOpnums.SamrEnumerateUsersInDomain;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                DomainHandle = inParams[0].ToIntPtr();
                EnumerationContext = TypeMarshal.ToStruct<uint>(inParams[1]);
                UserAccountControl = inParams[2].ToUInt32();
                PreferedMaximumLength = inParams[4].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The SamrEnumerateUsersInDomainResponse class defines output parameters of method SamrEnumerateUsersInDomain.
    /// </summary>
    public class SamrEnumerateUsersInDomainResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the EnumerationContext parameter
        /// </summary>
        public UInt32? EnumerationContext;

        /// <summary>
        /// the Buffer parameter
        /// </summary>
        public _SAMPR_ENUMERATION_BUFFER? Buffer;

        /// <summary>
        /// the CountReturned parameter
        /// </summary>
        public UInt32 CountReturned;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrEnumerateUsersInDomainResponse()
        {
            Opnum = SamrMethodOpnums.SamrEnumerateUsersInDomain;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pEnumerationContext = TypeMarshal.ToIntPtr<UInt32>(EnumerationContext);
            SafeIntPtr pBuffer = TypeMarshal.ToIntPtr<_SAMPR_ENUMERATION_BUFFER>(Buffer);
            IntPtr ppBuffer = Marshal.AllocHGlobal(IntPtr.Size);
            Marshal.WriteIntPtr(ppBuffer, pBuffer);
            SafeIntPtr pCountReturned = TypeMarshal.ToIntPtr<uint>(CountReturned);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                pEnumerationContext,
                IntPtr.Zero,
                ppBuffer,
                IntPtr.Zero,
                pCountReturned,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pEnumerationContext.Dispose();
                pBuffer.Dispose();
                Marshal.FreeHGlobal(ppBuffer);
                pCountReturned.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrCreateAliasInDomainRequest class defines input parameters of method SamrCreateAliasInDomain.
    /// </summary>
    public class SamrCreateAliasInDomainRequest : SamrRequestStub
    {
        /// <summary>
        /// the DomainHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr DomainHandle;

        /// <summary>
        /// the AccountName parameter
        /// </summary>
        public _RPC_UNICODE_STRING AccountName;

        /// <summary>
        /// the DesiredAccess parameter
        /// </summary>
        public uint DesiredAccess;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrCreateAliasInDomainRequest()
        {
            Opnum = SamrMethodOpnums.SamrCreateAliasInDomain;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                DomainHandle = inParams[0].ToIntPtr();
                AccountName = TypeMarshal.ToStruct<_RPC_UNICODE_STRING>(inParams[1]);
                DesiredAccess = inParams[2].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The SamrCreateAliasInDomainResponse class defines output parameters of method SamrCreateAliasInDomain.
    /// </summary>
    public class SamrCreateAliasInDomainResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the AliasHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr AliasHandle;

        /// <summary>
        /// the RelativeId parameter
        /// </summary>
        public UInt32 RelativeId;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrCreateAliasInDomainResponse()
        {
            Opnum = SamrMethodOpnums.SamrCreateAliasInDomain;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pAliasHandle = TypeMarshal.ToIntPtr(AliasHandle);
            SafeIntPtr pRelativeId = TypeMarshal.ToIntPtr(RelativeId);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                pAliasHandle,
                pRelativeId,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pAliasHandle.Dispose();
                pRelativeId.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrEnumerateAliasesInDomainRequest class defines input parameters of method SamrEnumerateAliasesInDomain.
    /// </summary>
    public class SamrEnumerateAliasesInDomainRequest : SamrRequestStub
    {
        /// <summary>
        /// the DomainHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr DomainHandle;

        /// <summary>
        /// the EnumerationContext parameter
        /// </summary>
        public UInt32? EnumerationContext;

        /// <summary>
        /// the PreferedMaximumLength parameter
        /// </summary>
        public uint PreferedMaximumLength;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrEnumerateAliasesInDomainRequest()
        {
            Opnum = SamrMethodOpnums.SamrEnumerateAliasesInDomain;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                DomainHandle = inParams[0].ToIntPtr();
                EnumerationContext = TypeMarshal.ToNullableStruct<uint>(inParams[1]);
                PreferedMaximumLength = inParams[3].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The SamrEnumerateAliasesInDomainResponse class defines output parameters of method SamrEnumerateAliasesInDomain.
    /// </summary>
    public class SamrEnumerateAliasesInDomainResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the EnumerationContext parameter
        /// </summary>
        public UInt32? EnumerationContext;

        /// <summary>
        /// the Buffer parameter
        /// </summary>
        public _SAMPR_ENUMERATION_BUFFER? Buffer;

        /// <summary>
        /// the CountReturned parameter
        /// </summary>
        public UInt32 CountReturned;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrEnumerateAliasesInDomainResponse()
        {
            Opnum = SamrMethodOpnums.SamrEnumerateAliasesInDomain;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pEnumerationContext = TypeMarshal.ToIntPtr(EnumerationContext);
            SafeIntPtr pBuffer = TypeMarshal.ToIntPtr(Buffer);
            IntPtr ppBuffer = Marshal.AllocHGlobal(IntPtr.Size);
            Marshal.WriteIntPtr(ppBuffer, pBuffer);
            SafeIntPtr pCountReturned = TypeMarshal.ToIntPtr(CountReturned);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                pEnumerationContext,
                ppBuffer,
                IntPtr.Zero,
                pCountReturned,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pEnumerationContext.Dispose();
                pBuffer.Dispose();
                Marshal.FreeHGlobal(ppBuffer);
                pCountReturned.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrGetAliasMembershipRequest class defines input parameters of method SamrGetAliasMembership.
    /// </summary>
    public class SamrGetAliasMembershipRequest : SamrRequestStub
    {
        /// <summary>
        /// the DomainHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr DomainHandle;

        /// <summary>
        /// the SidArray parameter
        /// </summary>
        public _SAMPR_PSID_ARRAY SidArray;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrGetAliasMembershipRequest()
        {
            Opnum = SamrMethodOpnums.SamrGetAliasMembership;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                DomainHandle = inParams[0].ToIntPtr();
                SidArray = TypeMarshal.ToStruct<_SAMPR_PSID_ARRAY>(inParams[1].ToIntPtr());
            }
        }
    }


    /// <summary>
    /// The SamrGetAliasMembershipResponse class defines output parameters of method SamrGetAliasMembership.
    /// </summary>
    public class SamrGetAliasMembershipResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the Membership parameter
        /// </summary>
        public _SAMPR_ULONG_ARRAY? Membership;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrGetAliasMembershipResponse()
        {
            Opnum = SamrMethodOpnums.SamrGetAliasMembership;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pMembership = TypeMarshal.ToIntPtr(Membership);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                pMembership,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pMembership.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrLookupNamesInDomainRequest class defines input parameters of method SamrLookupNamesInDomain.
    /// </summary>
    public class SamrLookupNamesInDomainRequest : SamrRequestStub
    {
        /// <summary>
        /// the DomainHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr DomainHandle;

        /// <summary>
        /// the Count parameter
        /// </summary>
        public uint Count;

        /// <summary>
        /// the Names parameter [Length("Count")] [Size("1000")]
        /// </summary>
        public _RPC_UNICODE_STRING[] Names;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrLookupNamesInDomainRequest()
        {
            Opnum = SamrMethodOpnums.SamrLookupNamesInDomain;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                DomainHandle = inParams[0].ToIntPtr();
                Count = inParams[1].ToUInt32();
                Names = IntPtrUtility.PtrToArray<_RPC_UNICODE_STRING>(inParams[2], Count);
            }
        }
    }


    /// <summary>
    /// The SamrLookupNamesInDomainResponse class defines output parameters of method SamrLookupNamesInDomain.
    /// </summary>
    public class SamrLookupNamesInDomainResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the RelativeIds parameter
        /// </summary>
        public _SAMPR_ULONG_ARRAY RelativeIds;

        /// <summary>
        /// the Use parameter
        /// </summary>
        public _SAMPR_ULONG_ARRAY Use;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrLookupNamesInDomainResponse()
        {
            Opnum = SamrMethodOpnums.SamrLookupNamesInDomain;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pRelativeIds = TypeMarshal.ToIntPtr(RelativeIds);
            SafeIntPtr pUse = TypeMarshal.ToIntPtr(Use);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                pRelativeIds,
                pUse,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pRelativeIds.Dispose();
                pUse.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrLookupIdsInDomainRequest class defines input parameters of method SamrLookupIdsInDomain.
    /// </summary>
    public class SamrLookupIdsInDomainRequest : SamrRequestStub
    {
        /// <summary>
        /// the DomainHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr DomainHandle;

        /// <summary>
        /// the Count parameter
        /// </summary>
        public uint Count;

        /// <summary>
        /// the RelativeIds parameter [Length("Count")] [Size("1000")]
        /// </summary>
        public uint[] RelativeIds;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrLookupIdsInDomainRequest()
        {
            Opnum = SamrMethodOpnums.SamrLookupIdsInDomain;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                DomainHandle = inParams[0].ToIntPtr();
                Count = inParams[1].ToUInt32();
                RelativeIds = IntPtrUtility.PtrToArray<uint>(inParams[2], Count);
            }
        }
    }


    /// <summary>
    /// The SamrLookupIdsInDomainResponse class defines output parameters of method SamrLookupIdsInDomain.
    /// </summary>
    public class SamrLookupIdsInDomainResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the Names parameter
        /// </summary>
        public _SAMPR_RETURNED_USTRING_ARRAY Names;

        /// <summary>
        /// the Use parameter
        /// </summary>
        public _SAMPR_ULONG_ARRAY Use;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrLookupIdsInDomainResponse()
        {
            Opnum = SamrMethodOpnums.SamrLookupIdsInDomain;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pNames = TypeMarshal.ToIntPtr(Names);
            SafeIntPtr pUse = TypeMarshal.ToIntPtr(Use);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                pNames,
                pUse,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pNames.Dispose();
                pUse.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrOpenGroupRequest class defines input parameters of method SamrOpenGroup.
    /// </summary>
    public class SamrOpenGroupRequest : SamrRequestStub
    {
        /// <summary>
        /// the DomainHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr DomainHandle;

        /// <summary>
        /// the DesiredAccess parameter
        /// </summary>
        public uint DesiredAccess;

        /// <summary>
        /// the GroupId parameter
        /// </summary>
        public uint GroupId;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrOpenGroupRequest()
        {
            Opnum = SamrMethodOpnums.SamrOpenGroup;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                DomainHandle = inParams[0].ToIntPtr();
                DesiredAccess = inParams[1].ToUInt32();
                GroupId = inParams[2].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The SamrOpenGroupResponse class defines output parameters of method SamrOpenGroup.
    /// </summary>
    public class SamrOpenGroupResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the GroupHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr GroupHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrOpenGroupResponse()
        {
            Opnum = SamrMethodOpnums.SamrOpenGroup;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pGroupHandle = TypeMarshal.ToIntPtr(GroupHandle);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                pGroupHandle,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pGroupHandle.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrQueryInformationGroupRequest class defines input parameters of method SamrQueryInformationGroup.
    /// </summary>
    public class SamrQueryInformationGroupRequest : SamrRequestStub
    {
        /// <summary>
        /// the GroupHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr GroupHandle;

        /// <summary>
        /// the GroupInformationClass parameter
        /// </summary>
        public _GROUP_INFORMATION_CLASS GroupInformationClass;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrQueryInformationGroupRequest()
        {
            Opnum = SamrMethodOpnums.SamrQueryInformationGroup;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                GroupHandle = inParams[0].ToIntPtr();
                GroupInformationClass = (_GROUP_INFORMATION_CLASS)inParams[1].ToInt32();
            }
        }
    }


    /// <summary>
    /// The SamrQueryInformationGroupResponse class defines output parameters of method SamrQueryInformationGroup.
    /// </summary>
    public class SamrQueryInformationGroupResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the GroupInformationClass parameter
        /// </summary>
        public _GROUP_INFORMATION_CLASS GroupInformationClass;

        /// <summary>
        /// the Buffer parameter [Switch("GroupInformationClass")]
        /// </summary>
        public _SAMPR_GROUP_INFO_BUFFER? Buffer;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrQueryInformationGroupResponse()
        {
            Opnum = SamrMethodOpnums.SamrQueryInformationGroup;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pBuffer = TypeMarshal.ToIntPtr(Buffer, GroupInformationClass, null, null);
            IntPtr ppBuffer = Marshal.AllocHGlobal(IntPtr.Size);
            Marshal.WriteIntPtr(ppBuffer, pBuffer);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                (int)GroupInformationClass,
                ppBuffer,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pBuffer.Dispose();
                Marshal.FreeHGlobal(ppBuffer);
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrSetInformationGroupRequest class defines input parameters of method SamrSetInformationGroup.
    /// </summary>
    public class SamrSetInformationGroupRequest : SamrRequestStub
    {
        /// <summary>
        /// the GroupHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr GroupHandle;

        /// <summary>
        /// the GroupInformationClass parameter
        /// </summary>
        public _GROUP_INFORMATION_CLASS GroupInformationClass;

        /// <summary>
        /// the Buffer parameter [Switch("GroupInformationClass")]
        /// </summary>
        public _SAMPR_GROUP_INFO_BUFFER? Buffer;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrSetInformationGroupRequest()
        {
            Opnum = SamrMethodOpnums.SamrSetInformationGroup;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                GroupHandle = inParams[0].ToIntPtr();
                GroupInformationClass = (_GROUP_INFORMATION_CLASS)inParams[1].ToInt32();
                Buffer = TypeMarshal.ToStruct<_SAMPR_GROUP_INFO_BUFFER>(
                    inParams[2].ToIntPtr(),
                    GroupInformationClass, null, null);
            }
        }
    }


    /// <summary>
    /// The SamrSetInformationGroupResponse class defines output parameters of method SamrSetInformationGroup.
    /// </summary>
    public class SamrSetInformationGroupResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrSetInformationGroupResponse()
        {
            Opnum = SamrMethodOpnums.SamrSetInformationGroup;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                Status
            };
            byte[] responseStub = RpceStubEncoder.ToBytes(
               RpceStubHelper.GetPlatform(),
                SamrRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[]{
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                SamrRpcStubFormatString.ProcFormatString,
                SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                paramList);
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrAddMemberToGroupRequest class defines input parameters of method SamrAddMemberToGroup.
    /// </summary>
    public class SamrAddMemberToGroupRequest : SamrRequestStub
    {
        /// <summary>
        /// the GroupHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr GroupHandle;

        /// <summary>
        /// the MemberId parameter
        /// </summary>
        public uint MemberId;

        /// <summary>
        /// the Attributes parameter
        /// </summary>
        public uint Attributes;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrAddMemberToGroupRequest()
        {
            Opnum = SamrMethodOpnums.SamrAddMemberToGroup;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                GroupHandle = inParams[0].ToIntPtr();
                MemberId = inParams[1].ToUInt32();
                Attributes = inParams[2].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The SamrAddMemberToGroupResponse class defines output parameters of method SamrAddMemberToGroup.
    /// </summary>
    public class SamrAddMemberToGroupResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrAddMemberToGroupResponse()
        {
            Opnum = SamrMethodOpnums.SamrAddMemberToGroup;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;

            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                Status
            };
            byte[] responseStub = RpceStubEncoder.ToBytes(
               RpceStubHelper.GetPlatform(),
                SamrRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[]{
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                SamrRpcStubFormatString.ProcFormatString,
                SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                paramList);
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrDeleteGroupRequest class defines input parameters of method SamrDeleteGroup.
    /// </summary>
    public class SamrDeleteGroupRequest : SamrRequestStub
    {
        /// <summary>
        /// the GroupHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr GroupHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrDeleteGroupRequest()
        {
            Opnum = SamrMethodOpnums.SamrDeleteGroup;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                GroupHandle = TypeMarshal.ToStruct<IntPtr>(inParams[0]);
            }
        }
    }


    /// <summary>
    /// The SamrDeleteGroupResponse class defines output parameters of method SamrDeleteGroup.
    /// </summary>
    public class SamrDeleteGroupResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the GroupHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr GroupHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrDeleteGroupResponse()
        {
            Opnum = SamrMethodOpnums.SamrDeleteGroup;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pGroupHandle = TypeMarshal.ToIntPtr(GroupHandle);
            paramList = new Int3264[] 
            {
                pGroupHandle,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pGroupHandle.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrRemoveMemberFromGroupRequest class defines input parameters of method SamrRemoveMemberFromGroup.
    /// </summary>
    public class SamrRemoveMemberFromGroupRequest : SamrRequestStub
    {
        /// <summary>
        /// the GroupHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr GroupHandle;

        /// <summary>
        /// the MemberId parameter
        /// </summary>
        public uint MemberId;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrRemoveMemberFromGroupRequest()
        {
            Opnum = SamrMethodOpnums.SamrRemoveMemberFromGroup;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                GroupHandle = inParams[0].ToIntPtr();
                MemberId = inParams[1].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The SamrRemoveMemberFromGroupResponse class defines output parameters of method SamrRemoveMemberFromGroup.
    /// </summary>
    public class SamrRemoveMemberFromGroupResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrRemoveMemberFromGroupResponse()
        {
            Opnum = SamrMethodOpnums.SamrRemoveMemberFromGroup;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                Status
            };
            byte[] responseStub = RpceStubEncoder.ToBytes(
               RpceStubHelper.GetPlatform(),
                SamrRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[]{
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                SamrRpcStubFormatString.ProcFormatString,
                SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                paramList);
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrGetMembersInGroupRequest class defines input parameters of method SamrGetMembersInGroup.
    /// </summary>
    public class SamrGetMembersInGroupRequest : SamrRequestStub
    {
        /// <summary>
        /// the GroupHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr GroupHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrGetMembersInGroupRequest()
        {
            Opnum = SamrMethodOpnums.SamrGetMembersInGroup;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                GroupHandle = inParams[0].ToIntPtr();
            }
        }
    }


    /// <summary>
    /// The SamrGetMembersInGroupResponse class defines output parameters of method SamrGetMembersInGroup.
    /// </summary>
    public class SamrGetMembersInGroupResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the Members parameter
        /// </summary>
        public _SAMPR_GET_MEMBERS_BUFFER? Members;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrGetMembersInGroupResponse()
        {
            Opnum = SamrMethodOpnums.SamrGetMembersInGroup;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pMembers = TypeMarshal.ToIntPtr(Members);
            IntPtr ppMembers = Marshal.AllocHGlobal(IntPtr.Size);
            Marshal.WriteIntPtr(ppMembers, pMembers);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                ppMembers,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pMembers.Dispose();
                Marshal.FreeHGlobal(ppMembers);
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrSetMemberAttributesOfGroupRequest class defines input parameters of method SamrSetMemberAttributesOfGroup.
    /// </summary>
    public class SamrSetMemberAttributesOfGroupRequest : SamrRequestStub
    {
        /// <summary>
        /// the GroupHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr GroupHandle;

        /// <summary>
        /// the MemberId parameter
        /// </summary>
        public uint MemberId;

        /// <summary>
        /// the Attributes parameter
        /// </summary>
        public uint Attributes;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrSetMemberAttributesOfGroupRequest()
        {
            Opnum = SamrMethodOpnums.SamrSetMemberAttributesOfGroup;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                GroupHandle = inParams[0].ToIntPtr();
                MemberId = inParams[1].ToUInt32();
                Attributes = inParams[2].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The SamrSetMemberAttributesOfGroupResponse class defines output parameters of method SamrSetMemberAttributesOfGroup.
    /// </summary>
    public class SamrSetMemberAttributesOfGroupResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrSetMemberAttributesOfGroupResponse()
        {
            Opnum = SamrMethodOpnums.SamrSetMemberAttributesOfGroup;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                Status
            };
            byte[] responseStub = RpceStubEncoder.ToBytes(
               RpceStubHelper.GetPlatform(),
                SamrRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[]{
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                SamrRpcStubFormatString.ProcFormatString,
                SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                paramList);
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrOpenAliasRequest class defines input parameters of method SamrOpenAlias.
    /// </summary>
    public class SamrOpenAliasRequest : SamrRequestStub
    {
        /// <summary>
        /// the DomainHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr DomainHandle;

        /// <summary>
        /// the DesiredAccess parameter
        /// </summary>
        public uint DesiredAccess;

        /// <summary>
        /// the AliasId parameter
        /// </summary>
        public uint AliasId;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrOpenAliasRequest()
        {
            Opnum = SamrMethodOpnums.SamrOpenAlias;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                DomainHandle = inParams[0].ToIntPtr();
                DesiredAccess = inParams[1].ToUInt32();
                AliasId = inParams[2].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The SamrOpenAliasResponse class defines output parameters of method SamrOpenAlias.
    /// </summary>
    public class SamrOpenAliasResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the AliasHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr AliasHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrOpenAliasResponse()
        {
            Opnum = SamrMethodOpnums.SamrOpenAlias;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pAliasHandle = TypeMarshal.ToIntPtr(AliasHandle);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                pAliasHandle,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pAliasHandle.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrQueryInformationAliasRequest class defines input parameters of method SamrQueryInformationAlias.
    /// </summary>
    public class SamrQueryInformationAliasRequest : SamrRequestStub
    {
        /// <summary>
        /// the AliasHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr AliasHandle;

        /// <summary>
        /// the AliasInformationClass parameter
        /// </summary>
        public _ALIAS_INFORMATION_CLASS AliasInformationClass;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrQueryInformationAliasRequest()
        {
            Opnum = SamrMethodOpnums.SamrQueryInformationAlias;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                AliasHandle = inParams[0].ToIntPtr();
                AliasInformationClass = (_ALIAS_INFORMATION_CLASS)inParams[1].ToInt32();
            }
        }
    }


    /// <summary>
    /// The SamrQueryInformationAliasResponse class defines output parameters of method SamrQueryInformationAlias.
    /// </summary>
    public class SamrQueryInformationAliasResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the AliasInformationClass parameter
        /// </summary>
        public _ALIAS_INFORMATION_CLASS AliasInformationClass;

        /// <summary>
        /// the Buffer parameter [Switch("AliasInformationClass")]
        /// </summary>
        public _SAMPR_ALIAS_INFO_BUFFER? Buffer;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrQueryInformationAliasResponse()
        {
            Opnum = SamrMethodOpnums.SamrQueryInformationAlias;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pBuffer = TypeMarshal.ToIntPtr<_SAMPR_ALIAS_INFO_BUFFER>(
                Buffer, AliasInformationClass, null, null);
            IntPtr ppBuffer = Marshal.AllocHGlobal(IntPtr.Size);
            Marshal.WriteIntPtr(ppBuffer, pBuffer);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                (int)AliasInformationClass,
                ppBuffer,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pBuffer.Dispose();
                Marshal.FreeHGlobal(ppBuffer);
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrSetInformationAliasRequest class defines input parameters of method SamrSetInformationAlias.
    /// </summary>
    public class SamrSetInformationAliasRequest : SamrRequestStub
    {
        /// <summary>
        /// the AliasHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr AliasHandle;

        /// <summary>
        /// the AliasInformationClass parameter
        /// </summary>
        public _ALIAS_INFORMATION_CLASS AliasInformationClass;

        /// <summary>
        /// the Buffer parameter [Switch("AliasInformationClass")]
        /// </summary>
        public _SAMPR_ALIAS_INFO_BUFFER Buffer;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrSetInformationAliasRequest()
        {
            Opnum = SamrMethodOpnums.SamrSetInformationAlias;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                AliasHandle = inParams[0].ToIntPtr();
                AliasInformationClass = (_ALIAS_INFORMATION_CLASS)inParams[1].ToInt32();
                Buffer = TypeMarshal.ToStruct<_SAMPR_ALIAS_INFO_BUFFER>(
                    inParams[2].ToIntPtr(), AliasInformationClass, null, null);
            }
        }
    }


    /// <summary>
    /// The SamrSetInformationAliasResponse class defines output parameters of method SamrSetInformationAlias.
    /// </summary>
    public class SamrSetInformationAliasResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrSetInformationAliasResponse()
        {
            Opnum = SamrMethodOpnums.SamrSetInformationAlias;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;

            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                Status
            };
            byte[] responseStub = RpceStubEncoder.ToBytes(
               RpceStubHelper.GetPlatform(),
                SamrRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[]{
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                SamrRpcStubFormatString.ProcFormatString,
                SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                paramList);
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrDeleteAliasRequest class defines input parameters of method SamrDeleteAlias.
    /// </summary>
    public class SamrDeleteAliasRequest : SamrRequestStub
    {
        /// <summary>
        /// the AliasHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr AliasHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrDeleteAliasRequest()
        {
            Opnum = SamrMethodOpnums.SamrDeleteAlias;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                AliasHandle = TypeMarshal.ToStruct<IntPtr>(inParams[0]);
            }
        }
    }


    /// <summary>
    /// The SamrDeleteAliasResponse class defines output parameters of method SamrDeleteAlias.
    /// </summary>
    public class SamrDeleteAliasResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the AliasHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr AliasHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrDeleteAliasResponse()
        {
            Opnum = SamrMethodOpnums.SamrDeleteAlias;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr ppAliasHandle = TypeMarshal.ToIntPtr(AliasHandle);
            paramList = new Int3264[] 
            {
                ppAliasHandle,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                ppAliasHandle.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrAddMemberToAliasRequest class defines input parameters of method SamrAddMemberToAlias.
    /// </summary>
    public class SamrAddMemberToAliasRequest : SamrRequestStub
    {
        /// <summary>
        /// the AliasHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr AliasHandle;

        /// <summary>
        /// the MemberId parameter
        /// </summary>
        public _RPC_SID MemberId;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrAddMemberToAliasRequest()
        {
            Opnum = SamrMethodOpnums.SamrAddMemberToAlias;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                AliasHandle = inParams[0].ToIntPtr();
                MemberId = TypeMarshal.ToStruct<_RPC_SID>(inParams[1]);
            }
        }
    }


    /// <summary>
    /// The SamrAddMemberToAliasResponse class defines output parameters of method SamrAddMemberToAlias.
    /// </summary>
    public class SamrAddMemberToAliasResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrAddMemberToAliasResponse()
        {
            Opnum = SamrMethodOpnums.SamrAddMemberToAlias;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;

            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                Status
            };
            byte[] responseStub = RpceStubEncoder.ToBytes(
               RpceStubHelper.GetPlatform(),
                SamrRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[]{
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                SamrRpcStubFormatString.ProcFormatString,
                SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                paramList);
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrRemoveMemberFromAliasRequest class defines input parameters of method SamrRemoveMemberFromAlias.
    /// </summary>
    public class SamrRemoveMemberFromAliasRequest : SamrRequestStub
    {
        /// <summary>
        /// the AliasHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr AliasHandle;

        /// <summary>
        /// the MemberId parameter
        /// </summary>
        public _RPC_SID MemberId;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrRemoveMemberFromAliasRequest()
        {
            Opnum = SamrMethodOpnums.SamrRemoveMemberFromAlias;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                AliasHandle = inParams[0].ToIntPtr();
                MemberId = TypeMarshal.ToStruct<_RPC_SID>(inParams[1]);
            }
        }
    }


    /// <summary>
    /// The SamrRemoveMemberFromAliasResponse class defines output parameters of method SamrRemoveMemberFromAlias.
    /// </summary>
    public class SamrRemoveMemberFromAliasResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrRemoveMemberFromAliasResponse()
        {
            Opnum = SamrMethodOpnums.SamrRemoveMemberFromAlias;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;

            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                Status
            };
            byte[] responseStub = RpceStubEncoder.ToBytes(
               RpceStubHelper.GetPlatform(),
                SamrRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[]{
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                SamrRpcStubFormatString.ProcFormatString,
                SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                paramList);
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrGetMembersInAliasRequest class defines input parameters of method SamrGetMembersInAlias.
    /// </summary>
    public class SamrGetMembersInAliasRequest : SamrRequestStub
    {

        /// <summary>
        /// the AliasHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr AliasHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrGetMembersInAliasRequest()
        {
            Opnum = SamrMethodOpnums.SamrGetMembersInAlias;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                AliasHandle = inParams[0].ToIntPtr();
            }
        }
    }


    /// <summary>
    /// The SamrGetMembersInAliasResponse class defines output parameters of method SamrGetMembersInAlias.
    /// </summary>
    public class SamrGetMembersInAliasResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the Members parameter
        /// </summary>
        public _SAMPR_PSID_ARRAY? Members;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrGetMembersInAliasResponse()
        {
            Opnum = SamrMethodOpnums.SamrGetMembersInAlias;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pMembers = TypeMarshal.ToIntPtr(Members);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                pMembers,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pMembers.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrOpenUserRequest class defines input parameters of method SamrOpenUser.
    /// </summary>
    public class SamrOpenUserRequest : SamrRequestStub
    {
        /// <summary>
        /// the DomainHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr DomainHandle;

        /// <summary>
        /// the DesiredAccess parameter
        /// </summary>
        public uint DesiredAccess;

        /// <summary>
        /// the UserId parameter
        /// </summary>
        public uint UserId;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrOpenUserRequest()
        {
            Opnum = SamrMethodOpnums.SamrOpenUser;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                DomainHandle = inParams[0].ToIntPtr();
                DesiredAccess = inParams[1].ToUInt32();
                UserId = inParams[2].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The SamrOpenUserResponse class defines output parameters of method SamrOpenUser.
    /// </summary>
    public class SamrOpenUserResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the UserHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr UserHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrOpenUserResponse()
        {
            Opnum = SamrMethodOpnums.SamrOpenUser;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pUserHandle = TypeMarshal.ToIntPtr(UserHandle);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                pUserHandle,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pUserHandle.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrDeleteUserRequest class defines input parameters of method SamrDeleteUser.
    /// </summary>
    public class SamrDeleteUserRequest : SamrRequestStub
    {

        /// <summary>
        /// the UserHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr UserHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrDeleteUserRequest()
        {
            Opnum = SamrMethodOpnums.SamrDeleteUser;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                UserHandle = TypeMarshal.ToStruct<IntPtr>(inParams[0]);
            }
        }
    }


    /// <summary>
    /// The SamrDeleteUserResponse class defines output parameters of method SamrDeleteUser.
    /// </summary>
    public class SamrDeleteUserResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the UserHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr UserHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrDeleteUserResponse()
        {
            Opnum = SamrMethodOpnums.SamrDeleteUser;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr ppUserHandle = TypeMarshal.ToIntPtr(UserHandle);
            paramList = new Int3264[] 
            {
                ppUserHandle,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                ppUserHandle.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrQueryInformationUserRequest class defines input parameters of method SamrQueryInformationUser.
    /// </summary>
    public class SamrQueryInformationUserRequest : SamrRequestStub
    {
        /// <summary>
        /// the UserHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr UserHandle;

        /// <summary>
        /// the UserInformationClass parameter
        /// </summary>
        public _USER_INFORMATION_CLASS UserInformationClass;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrQueryInformationUserRequest()
        {
            Opnum = SamrMethodOpnums.SamrQueryInformationUser;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                UserHandle = inParams[0].ToIntPtr();
                UserInformationClass = (_USER_INFORMATION_CLASS)inParams[1].ToInt32();
            }
        }
    }


    /// <summary>
    /// The SamrQueryInformationUserResponse class defines output parameters of method SamrQueryInformationUser.
    /// </summary>
    public class SamrQueryInformationUserResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the UserInformationClass parameter
        /// </summary>
        public _USER_INFORMATION_CLASS UserInformationClass;

        /// <summary>
        /// the Buffer parameter [Switch("UserInformationClass")]
        /// </summary>
        public _SAMPR_USER_INFO_BUFFER? Buffer;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrQueryInformationUserResponse()
        {
            Opnum = SamrMethodOpnums.SamrQueryInformationUser;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pBuffer = TypeMarshal.ToIntPtr<_SAMPR_USER_INFO_BUFFER>(
                Buffer, UserInformationClass, null, null);
            IntPtr ppBuffer = Marshal.AllocHGlobal(IntPtr.Size);
            Marshal.WriteIntPtr(ppBuffer, pBuffer);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                (int)UserInformationClass,
                ppBuffer,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pBuffer.Dispose();
                Marshal.FreeHGlobal(ppBuffer);
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrSetInformationUserRequest class defines input parameters of method SamrSetInformationUser.
    /// </summary>
    public class SamrSetInformationUserRequest : SamrRequestStub
    {
        /// <summary>
        /// the UserHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr UserHandle;

        /// <summary>
        /// the UserInformationClass parameter
        /// </summary>
        public _USER_INFORMATION_CLASS UserInformationClass;

        /// <summary>
        /// the Buffer parameter [Switch("UserInformationClass")]
        /// </summary>
        public _SAMPR_USER_INFO_BUFFER Buffer;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrSetInformationUserRequest()
        {
            Opnum = SamrMethodOpnums.SamrSetInformationUser;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                UserHandle = inParams[0].ToIntPtr();
                UserInformationClass = (_USER_INFORMATION_CLASS)inParams[1].ToInt32();
                Buffer = TypeMarshal.ToStruct<_SAMPR_USER_INFO_BUFFER>
                    (inParams[2].ToIntPtr(), UserInformationClass, null, null);
            }
        }
    }


    /// <summary>
    /// The SamrSetInformationUserResponse class defines output parameters of method SamrSetInformationUser.
    /// </summary>
    public class SamrSetInformationUserResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrSetInformationUserResponse()
        {
            Opnum = SamrMethodOpnums.SamrSetInformationUser;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;

            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                Status
            };
            byte[] responseStub = RpceStubEncoder.ToBytes(
               RpceStubHelper.GetPlatform(),
                SamrRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[]{
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                SamrRpcStubFormatString.ProcFormatString,
                SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                paramList);
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrChangePasswordUserRequest class defines input parameters of method SamrChangePasswordUser.
    /// </summary>
    public class SamrChangePasswordUserRequest : SamrRequestStub
    {
        /// <summary>
        /// the UserHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr UserHandle;

        /// <summary>
        /// the LmPresent parameter
        /// </summary>
        public byte LmPresent;

        /// <summary>
        /// the OldLmEncryptedWithNewLm parameter
        /// </summary>
        public _ENCRYPTED_LM_OWF_PASSWORD? OldLmEncryptedWithNewLm;

        /// <summary>
        /// the NewLmEncryptedWithOldLm parameter
        /// </summary>
        public _ENCRYPTED_LM_OWF_PASSWORD? NewLmEncryptedWithOldLm;

        /// <summary>
        /// the NtPresent parameter
        /// </summary>
        public byte NtPresent;

        /// <summary>
        /// the OldNtEncryptedWithNewNt parameter
        /// </summary>
        public _ENCRYPTED_LM_OWF_PASSWORD? OldNtEncryptedWithNewNt;

        /// <summary>
        /// the NewNtEncryptedWithOldNt parameter
        /// </summary>
        public _ENCRYPTED_LM_OWF_PASSWORD? NewNtEncryptedWithOldNt;

        /// <summary>
        /// the NtCrossEncryptionPresent parameter
        /// </summary>
        public byte NtCrossEncryptionPresent;

        /// <summary>
        /// the NewNtEncryptedWithNewLm parameter
        /// </summary>
        public _ENCRYPTED_LM_OWF_PASSWORD? NewNtEncryptedWithNewLm;

        /// <summary>
        /// the LmCrossEncryptionPresent parameter
        /// </summary>
        public byte LmCrossEncryptionPresent;

        /// <summary>
        /// the NewLmEncryptedWithNewNt parameter
        /// </summary>
        public _ENCRYPTED_LM_OWF_PASSWORD? NewLmEncryptedWithNewNt;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrChangePasswordUserRequest()
        {
            Opnum = SamrMethodOpnums.SamrChangePasswordUser;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                UserHandle = inParams[0].ToIntPtr();
                LmPresent = (byte)inParams[1].ToInt32();
                OldLmEncryptedWithNewLm = TypeMarshal.ToNullableStruct<_ENCRYPTED_LM_OWF_PASSWORD>(inParams[2]);
                NewLmEncryptedWithOldLm = TypeMarshal.ToNullableStruct<_ENCRYPTED_LM_OWF_PASSWORD>(inParams[3]);
                NtPresent = (byte)inParams[4].ToInt32();
                OldNtEncryptedWithNewNt = TypeMarshal.ToNullableStruct<_ENCRYPTED_LM_OWF_PASSWORD>(inParams[5]);
                NewNtEncryptedWithOldNt = TypeMarshal.ToNullableStruct<_ENCRYPTED_LM_OWF_PASSWORD>(inParams[6]);
                NtCrossEncryptionPresent = (byte)inParams[7].ToInt32();
                NewNtEncryptedWithNewLm = TypeMarshal.ToNullableStruct<_ENCRYPTED_LM_OWF_PASSWORD>(inParams[8]);
                LmCrossEncryptionPresent = (byte)inParams[9].ToInt32();
                NewLmEncryptedWithNewNt = TypeMarshal.ToNullableStruct<_ENCRYPTED_LM_OWF_PASSWORD>(inParams[10]);
            }
        }
    }


    /// <summary>
    /// The SamrChangePasswordUserResponse class defines output parameters of method SamrChangePasswordUser.
    /// </summary>
    public class SamrChangePasswordUserResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrChangePasswordUserResponse()
        {
            Opnum = SamrMethodOpnums.SamrChangePasswordUser;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;

            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                Status
            };
            byte[] responseStub = RpceStubEncoder.ToBytes(
               RpceStubHelper.GetPlatform(),
                SamrRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[]{
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                SamrRpcStubFormatString.ProcFormatString,
                SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                paramList);
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrGetGroupsForUserRequest class defines input parameters of method SamrGetGroupsForUser.
    /// </summary>
    public class SamrGetGroupsForUserRequest : SamrRequestStub
    {
        /// <summary>
        /// the UserHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr UserHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrGetGroupsForUserRequest()
        {
            Opnum = SamrMethodOpnums.SamrGetGroupsForUser;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                UserHandle = inParams[0].ToIntPtr();
            }
        }
    }


    /// <summary>
    /// The SamrGetGroupsForUserResponse class defines output parameters of method SamrGetGroupsForUser.
    /// </summary>
    public class SamrGetGroupsForUserResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the Groups parameter
        /// </summary>
        public _SAMPR_GET_GROUPS_BUFFER? Groups;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrGetGroupsForUserResponse()
        {
            Opnum = SamrMethodOpnums.SamrGetGroupsForUser;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pGroups = TypeMarshal.ToIntPtr(Groups, null, null, null);
            IntPtr ppGroups = Marshal.AllocHGlobal(IntPtr.Size);
            Marshal.WriteIntPtr(ppGroups, pGroups);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                ppGroups,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pGroups.Dispose();
                Marshal.FreeHGlobal(ppGroups);
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrQueryDisplayInformationRequest class defines input parameters of method SamrQueryDisplayInformation.
    /// </summary>
    public class SamrQueryDisplayInformationRequest : SamrRequestStub
    {
        /// <summary>
        /// the DomainHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr DomainHandle;

        /// <summary>
        /// the DisplayInformationClass parameter
        /// </summary>
        public _DOMAIN_DISPLAY_INFORMATION DisplayInformationClass;

        /// <summary>
        /// the Index parameter
        /// </summary>
        public uint Index;

        /// <summary>
        /// the EntryCount parameter
        /// </summary>
        public uint EntryCount;

        /// <summary>
        /// the PreferredMaximumLength parameter
        /// </summary>
        public uint PreferredMaximumLength;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrQueryDisplayInformationRequest()
        {
            Opnum = SamrMethodOpnums.SamrQueryDisplayInformation;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                DomainHandle = inParams[0].ToIntPtr();
                DisplayInformationClass = (_DOMAIN_DISPLAY_INFORMATION)inParams[1].ToInt32();
                Index = inParams[2].ToUInt32();
                EntryCount = inParams[3].ToUInt32();
                PreferredMaximumLength = inParams[4].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The SamrQueryDisplayInformationResponse class defines output parameters of method SamrQueryDisplayInformation.
    /// </summary>
    public class SamrQueryDisplayInformationResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the TotalAvailable parameter
        /// </summary>
        public UInt32 TotalAvailable;

        /// <summary>
        /// the TotalReturned parameter
        /// </summary>
        public UInt32 TotalReturned;

        /// <summary>
        /// the DisplayInformationClass parameter
        /// </summary>
        public _DOMAIN_DISPLAY_INFORMATION DisplayInformationClass;

        /// <summary>
        /// the Buffer parameter
        /// </summary>
        //[Switch("DisplayInformationClass")]
        public _SAMPR_DISPLAY_INFO_BUFFER? Buffer;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrQueryDisplayInformationResponse()
        {
            Opnum = SamrMethodOpnums.SamrQueryDisplayInformation;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pTotalAvailable = TypeMarshal.ToIntPtr(TotalAvailable);
            SafeIntPtr pTotalReturned = TypeMarshal.ToIntPtr(TotalReturned);
            SafeIntPtr pBuffer = TypeMarshal.ToIntPtr(Buffer, DisplayInformationClass, null, null);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                (int)DisplayInformationClass,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                pTotalAvailable,
                pTotalReturned,
                pBuffer,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pTotalAvailable.Dispose();
                pTotalReturned.Dispose();
                pBuffer.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrGetDisplayEnumerationIndexRequest class defines input parameters of method SamrGetDisplayEnumerationIndex.
    /// </summary>
    public class SamrGetDisplayEnumerationIndexRequest : SamrRequestStub
    {
        /// <summary>
        /// the DomainHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr DomainHandle;

        /// <summary>
        /// the DisplayInformationClass parameter
        /// </summary>
        public _DOMAIN_DISPLAY_INFORMATION DisplayInformationClass;

        /// <summary>
        /// the Prefix parameter
        /// </summary>
        public _RPC_UNICODE_STRING Prefix;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrGetDisplayEnumerationIndexRequest()
        {
            Opnum = SamrMethodOpnums.SamrGetDisplayEnumerationIndex;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                DomainHandle = inParams[0].ToIntPtr();
                DisplayInformationClass = (_DOMAIN_DISPLAY_INFORMATION)(inParams[1].ToInt32());
                Prefix = TypeMarshal.ToStruct<_RPC_UNICODE_STRING>(inParams[2]);
            }
        }
    }


    /// <summary>
    /// The SamrGetDisplayEnumerationIndexResponse class defines output parameters of method SamrGetDisplayEnumerationIndex.
    /// </summary>
    public class SamrGetDisplayEnumerationIndexResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the Index parameter
        /// </summary>
        public UInt32 Index;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrGetDisplayEnumerationIndexResponse()
        {
            Opnum = SamrMethodOpnums.SamrGetDisplayEnumerationIndex;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pIndex = TypeMarshal.ToIntPtr(Index);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                pIndex,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pIndex.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The Opnum42NotUsedOnWireRequest class defines input parameters of method Opnum42NotUsedOnWire.
    /// </summary>
    public class Opnum42NotUsedOnWireRequest : SamrRequestStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum42NotUsedOnWireRequest()
        {
            Opnum = SamrMethodOpnums.Opnum42NotUsedOnWire;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    }


    /// <summary>
    /// The Opnum42NotUsedOnWireResponse class defines output parameters of method Opnum42NotUsedOnWire.
    /// </summary>
    public class Opnum42NotUsedOnWireResponse : SamrResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum42NotUsedOnWireResponse()
        {
            Opnum = SamrMethodOpnums.Opnum42NotUsedOnWire;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            return null;
        }
    }


    /// <summary>
    /// The Opnum43NotUsedOnWireRequest class defines input parameters of method Opnum43NotUsedOnWire.
    /// </summary>
    public class Opnum43NotUsedOnWireRequest : SamrRequestStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum43NotUsedOnWireRequest()
        {
            Opnum = SamrMethodOpnums.Opnum43NotUsedOnWire;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    }


    /// <summary>
    /// The Opnum43NotUsedOnWireResponse class defines output parameters of method Opnum43NotUsedOnWire.
    /// </summary>
    public class Opnum43NotUsedOnWireResponse : SamrResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum43NotUsedOnWireResponse()
        {
            Opnum = SamrMethodOpnums.Opnum43NotUsedOnWire;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            return null;
        }
    }


    /// <summary>
    /// The SamrGetUserDomainPasswordInformationRequest class defines input parameters of method SamrGetUserDomainPasswordInformation.
    /// </summary>
    public class SamrGetUserDomainPasswordInformationRequest : SamrRequestStub
    {
        /// <summary>
        /// the UserHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr UserHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrGetUserDomainPasswordInformationRequest()
        {
            Opnum = SamrMethodOpnums.SamrGetUserDomainPasswordInformation;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                UserHandle = inParams[0].ToIntPtr();
            }
        }
    }


    /// <summary>
    /// The SamrGetUserDomainPasswordInformationResponse class defines output parameters of method SamrGetUserDomainPasswordInformation.
    /// </summary>
    public class SamrGetUserDomainPasswordInformationResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the PasswordInformation parameter
        /// </summary>
        public _USER_DOMAIN_PASSWORD_INFORMATION PasswordInformation;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrGetUserDomainPasswordInformationResponse()
        {
            Opnum = SamrMethodOpnums.SamrGetUserDomainPasswordInformation;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pPasswordInformation = TypeMarshal.ToIntPtr(PasswordInformation);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                pPasswordInformation,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pPasswordInformation.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrRemoveMemberFromForeignDomainRequest class defines input parameters of method SamrRemoveMemberFromForeignDomain.
    /// </summary>
    public class SamrRemoveMemberFromForeignDomainRequest : SamrRequestStub
    {
        /// <summary>
        /// the DomainHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr DomainHandle;

        /// <summary>
        /// the MemberSid parameter
        /// </summary>
        public _RPC_SID? MemberSid;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrRemoveMemberFromForeignDomainRequest()
        {
            Opnum = SamrMethodOpnums.SamrRemoveMemberFromForeignDomain;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                DomainHandle = inParams[0].ToIntPtr();
                MemberSid = TypeMarshal.ToStruct<_RPC_SID>(inParams[1]);
            }
        }
    }


    /// <summary>
    /// The SamrRemoveMemberFromForeignDomainResponse class defines output parameters of method SamrRemoveMemberFromForeignDomain.
    /// </summary>
    public class SamrRemoveMemberFromForeignDomainResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrRemoveMemberFromForeignDomainResponse()
        {
            Opnum = SamrMethodOpnums.SamrRemoveMemberFromForeignDomain;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;

            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                Status
            };
            byte[] responseStub = RpceStubEncoder.ToBytes(
               RpceStubHelper.GetPlatform(),
                SamrRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[]{
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                SamrRpcStubFormatString.ProcFormatString,
                SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                paramList);
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrQueryInformationDomain2Request class defines input parameters of method SamrQueryInformationDomain2.
    /// </summary>
    public class SamrQueryInformationDomain2Request : SamrRequestStub
    {
        /// <summary>
        /// the DomainHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr DomainHandle;

        /// <summary>
        /// the DomainInformationClass parameter
        /// </summary>
        public _DOMAIN_INFORMATION_CLASS DomainInformationClass;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrQueryInformationDomain2Request()
        {
            Opnum = SamrMethodOpnums.SamrQueryInformationDomain2;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                DomainHandle = inParams[0].ToIntPtr();
                DomainInformationClass = (_DOMAIN_INFORMATION_CLASS)(inParams[1].ToInt32());
            }
        }
    }


    /// <summary>
    /// The SamrQueryInformationDomain2Response class defines output parameters of method SamrQueryInformationDomain2.
    /// </summary>
    public class SamrQueryInformationDomain2Response : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the DomainInformationClass parameter
        /// </summary>
        public _DOMAIN_INFORMATION_CLASS DomainInformationClass;

        /// <summary>
        /// the Buffer parameter
        /// </summary>
        //[Switch("DomainInformationClass")]
        public _SAMPR_DOMAIN_INFO_BUFFER? Buffer;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrQueryInformationDomain2Response()
        {
            Opnum = SamrMethodOpnums.SamrQueryInformationDomain2;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pBuffer = TypeMarshal.ToIntPtr(Buffer, DomainInformationClass, null, null);
            IntPtr ppBuffer = Marshal.AllocHGlobal(IntPtr.Size);
            Marshal.WriteIntPtr(ppBuffer, pBuffer);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                (int)DomainInformationClass,
                ppBuffer,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pBuffer.Dispose();
                Marshal.FreeHGlobal(ppBuffer);
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrQueryInformationUser2Request class defines input parameters of method SamrQueryInformationUser2.
    /// </summary>
    public class SamrQueryInformationUser2Request : SamrRequestStub
    {
        /// <summary>
        /// the UserHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr UserHandle;

        /// <summary>
        /// the UserInformationClass parameter
        /// </summary>
        public _USER_INFORMATION_CLASS UserInformationClass;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrQueryInformationUser2Request()
        {
            Opnum = SamrMethodOpnums.SamrQueryInformationUser2;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                UserHandle = inParams[0].ToIntPtr();
                UserInformationClass = (_USER_INFORMATION_CLASS)(inParams[1].ToInt32());
            }
        }
    }


    /// <summary>
    /// The SamrQueryInformationUser2Response class defines output parameters of method SamrQueryInformationUser2.
    /// </summary>
    public class SamrQueryInformationUser2Response : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the UserInformationClass parameter
        /// </summary>
        public _USER_INFORMATION_CLASS UserInformationClass;

        /// <summary>
        /// the Buffer parameter [Switch("UserInformationClass")]
        /// </summary>
        public _SAMPR_USER_INFO_BUFFER? Buffer;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrQueryInformationUser2Response()
        {
            Opnum = SamrMethodOpnums.SamrQueryInformationUser2;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pBuffer = TypeMarshal.ToIntPtr(Buffer, UserInformationClass, null, null);
            IntPtr ppBuffer = Marshal.AllocHGlobal(IntPtr.Size);
            Marshal.WriteIntPtr(ppBuffer, pBuffer);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                (int) UserInformationClass,
                ppBuffer,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pBuffer.Dispose();
                Marshal.FreeHGlobal(ppBuffer);
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrQueryDisplayInformation2Request class defines input parameters of method SamrQueryDisplayInformation2.
    /// </summary>
    public class SamrQueryDisplayInformation2Request : SamrRequestStub
    {
        /// <summary>
        /// the DomainHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr DomainHandle;

        /// <summary>
        /// the DisplayInformationClass parameter
        /// </summary>
        public _DOMAIN_DISPLAY_INFORMATION DisplayInformationClass;

        /// <summary>
        /// the Index parameter
        /// </summary>
        public uint Index;

        /// <summary>
        /// the EntryCount parameter
        /// </summary>
        public uint EntryCount;

        /// <summary>
        /// the PreferredMaximumLength parameter
        /// </summary>
        public uint PreferredMaximumLength;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrQueryDisplayInformation2Request()
        {
            Opnum = SamrMethodOpnums.SamrQueryDisplayInformation2;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                DomainHandle = inParams[0].ToIntPtr();
                DisplayInformationClass = (_DOMAIN_DISPLAY_INFORMATION)(inParams[1].ToInt32());
                Index = inParams[2].ToUInt32();
                EntryCount = inParams[3].ToUInt32();
                PreferredMaximumLength = inParams[4].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The SamrQueryDisplayInformation2Response class defines output parameters of method SamrQueryDisplayInformation2.
    /// </summary>
    public class SamrQueryDisplayInformation2Response : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the TotalAvailable parameter
        /// </summary>
        public UInt32 TotalAvailable;

        /// <summary>
        /// the TotalReturned parameter
        /// </summary>
        public UInt32 TotalReturned;

        /// <summary>
        /// the DisplayInformationClass parameter
        /// </summary>
        public _DOMAIN_DISPLAY_INFORMATION DisplayInformationClass;

        /// <summary>
        /// the Buffer parameter [Switch("DisplayInformationClass")]
        /// </summary>
        public _SAMPR_DISPLAY_INFO_BUFFER? Buffer;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrQueryDisplayInformation2Response()
        {
            Opnum = SamrMethodOpnums.SamrQueryDisplayInformation2;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pTotalAvailable = TypeMarshal.ToIntPtr(TotalAvailable);
            SafeIntPtr pTotalReturned = TypeMarshal.ToIntPtr(TotalReturned);
            SafeIntPtr pBuffer = TypeMarshal.ToIntPtr(Buffer, DisplayInformationClass, null, null);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                (int)DisplayInformationClass,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                pTotalAvailable,
                pTotalReturned,
                pBuffer,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pTotalAvailable.Dispose();
                pTotalReturned.Dispose();
                pBuffer.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrGetDisplayEnumerationIndex2Request class defines input parameters of method SamrGetDisplayEnumerationIndex2.
    /// </summary>
    public class SamrGetDisplayEnumerationIndex2Request : SamrRequestStub
    {
        /// <summary>
        /// the DomainHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr DomainHandle;

        /// <summary>
        /// the DisplayInformationClass parameter
        /// </summary>
        public _DOMAIN_DISPLAY_INFORMATION DisplayInformationClass;

        /// <summary>
        /// the Prefix parameter
        /// </summary>
        public _RPC_UNICODE_STRING Prefix;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrGetDisplayEnumerationIndex2Request()
        {
            Opnum = SamrMethodOpnums.SamrGetDisplayEnumerationIndex2;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                DomainHandle = inParams[0].ToIntPtr();
                DisplayInformationClass = (_DOMAIN_DISPLAY_INFORMATION)(inParams[1].ToInt32());
                Prefix = TypeMarshal.ToStruct<_RPC_UNICODE_STRING>(inParams[2]);
            }
        }
    }


    /// <summary>
    /// The SamrGetDisplayEnumerationIndex2Response class defines output parameters of method SamrGetDisplayEnumerationIndex2.
    /// </summary>
    public class SamrGetDisplayEnumerationIndex2Response : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the Index parameter
        /// </summary>
        public UInt32 Index;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrGetDisplayEnumerationIndex2Response()
        {
            Opnum = SamrMethodOpnums.SamrGetDisplayEnumerationIndex2;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pIndex = TypeMarshal.ToIntPtr(Index);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                pIndex,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pIndex.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrCreateUser2InDomainRequest class defines input parameters of method SamrCreateUser2InDomain.
    /// </summary>
    public class SamrCreateUser2InDomainRequest : SamrRequestStub
    {
        /// <summary>
        /// the DomainHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr DomainHandle;

        /// <summary>
        /// the Name parameter
        /// </summary>
        public _RPC_UNICODE_STRING? Name;

        /// <summary>
        /// the AccountType parameter
        /// </summary>
        public uint AccountType;

        /// <summary>
        /// the DesiredAccess parameter
        /// </summary>
        public uint DesiredAccess;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrCreateUser2InDomainRequest()
        {
            Opnum = SamrMethodOpnums.SamrCreateUser2InDomain;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                DomainHandle = inParams[0].ToIntPtr();
                Name = TypeMarshal.ToStruct<_RPC_UNICODE_STRING>(inParams[1]);
                AccountType = inParams[2].ToUInt32();
                DesiredAccess = inParams[3].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The SamrCreateUser2InDomainResponse class defines output parameters of method SamrCreateUser2InDomain.
    /// </summary>
    public class SamrCreateUser2InDomainResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the UserHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr UserHandle;

        /// <summary>
        /// the GrantedAccess parameter
        /// </summary>
        public UInt32 GrantedAccess;

        /// <summary>
        /// the RelativeId parameter
        /// </summary>
        public UInt32 RelativeId;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrCreateUser2InDomainResponse()
        {
            Opnum = SamrMethodOpnums.SamrCreateUser2InDomain;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pUserHandle = TypeMarshal.ToIntPtr(UserHandle);
            SafeIntPtr pGrantedAccess = TypeMarshal.ToIntPtr(GrantedAccess);
            SafeIntPtr pRelativeId = TypeMarshal.ToIntPtr(RelativeId);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                pUserHandle,
                pGrantedAccess,
                pRelativeId,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pUserHandle.Dispose();
                pGrantedAccess.Dispose();
                pRelativeId.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrQueryDisplayInformation3Request class defines input parameters of method SamrQueryDisplayInformation3.
    /// </summary>
    public class SamrQueryDisplayInformation3Request : SamrRequestStub
    {
        /// <summary>
        /// the DomainHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr DomainHandle;

        /// <summary>
        /// the DisplayInformationClass parameter
        /// </summary>
        public _DOMAIN_DISPLAY_INFORMATION DisplayInformationClass;

        /// <summary>
        /// the Index parameter
        /// </summary>
        public uint Index;

        /// <summary>
        /// the EntryCount parameter
        /// </summary>
        public uint EntryCount;

        /// <summary>
        /// the PreferredMaximumLength parameter
        /// </summary>
        public uint PreferredMaximumLength;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrQueryDisplayInformation3Request()
        {
            Opnum = SamrMethodOpnums.SamrQueryDisplayInformation3;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                DomainHandle = inParams[0].ToIntPtr();
                DisplayInformationClass = (_DOMAIN_DISPLAY_INFORMATION)(inParams[1].ToInt32());
                Index = inParams[2].ToUInt32();
                EntryCount = inParams[3].ToUInt32();
                PreferredMaximumLength = inParams[4].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The SamrQueryDisplayInformation3Response class defines output parameters of method SamrQueryDisplayInformation3.
    /// </summary>
    public class SamrQueryDisplayInformation3Response : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the TotalAvailable parameter
        /// </summary>
        public UInt32 TotalAvailable;

        /// <summary>
        /// the TotalReturned parameter
        /// </summary>
        public UInt32 TotalReturned;

        /// <summary>
        /// the DisplayInformationClass parameter
        /// </summary>
        public _DOMAIN_DISPLAY_INFORMATION DisplayInformationClass;

        /// <summary>
        /// the Buffer parameter [Switch("DisplayInformationClass")]
        /// </summary>
        public _SAMPR_DISPLAY_INFO_BUFFER? Buffer;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrQueryDisplayInformation3Response()
        {
            Opnum = SamrMethodOpnums.SamrQueryDisplayInformation3;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pTotalAvailable = TypeMarshal.ToIntPtr(TotalAvailable);
            SafeIntPtr pTotalReturned = TypeMarshal.ToIntPtr(TotalReturned);
            SafeIntPtr pBuffer = TypeMarshal.ToIntPtr(Buffer, DisplayInformationClass, null, null);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                (int)DisplayInformationClass,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                pTotalAvailable,
                pTotalReturned,
                pBuffer,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pTotalAvailable.Dispose();
                pTotalReturned.Dispose();
                pBuffer.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrAddMultipleMembersToAliasRequest class defines input parameters of method SamrAddMultipleMembersToAlias.
    /// </summary>
    public class SamrAddMultipleMembersToAliasRequest : SamrRequestStub
    {

        /// <summary>
        /// the AliasHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr AliasHandle;

        /// <summary>
        /// the MembersBuffer parameter
        /// </summary>
        public _SAMPR_PSID_ARRAY? MembersBuffer;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrAddMultipleMembersToAliasRequest()
        {
            Opnum = SamrMethodOpnums.SamrAddMultipleMembersToAlias;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                AliasHandle = inParams[0].ToIntPtr();
                MembersBuffer = TypeMarshal.ToNullableStruct<_SAMPR_PSID_ARRAY>(inParams[1]);
            }
        }
    }


    /// <summary>
    /// The SamrAddMultipleMembersToAliasResponse class defines output parameters of method SamrAddMultipleMembersToAlias.
    /// </summary>
    public class SamrAddMultipleMembersToAliasResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrAddMultipleMembersToAliasResponse()
        {
            Opnum = SamrMethodOpnums.SamrAddMultipleMembersToAlias;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;

            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                Status
            };
            byte[] responseStub = RpceStubEncoder.ToBytes(
               RpceStubHelper.GetPlatform(),
                SamrRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[]{
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                SamrRpcStubFormatString.ProcFormatString,
                SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                paramList);
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrRemoveMultipleMembersFromAliasRequest class defines input parameters of method SamrRemoveMultipleMembersFromAlias.
    /// </summary>
    public class SamrRemoveMultipleMembersFromAliasRequest : SamrRequestStub
    {
        /// <summary>
        /// the AliasHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr AliasHandle;

        /// <summary>
        /// the MembersBuffer parameter
        /// </summary>
        public _SAMPR_PSID_ARRAY? MembersBuffer;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrRemoveMultipleMembersFromAliasRequest()
        {
            Opnum = SamrMethodOpnums.SamrRemoveMultipleMembersFromAlias;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                AliasHandle = inParams[0].ToIntPtr();
                MembersBuffer = TypeMarshal.ToNullableStruct<_SAMPR_PSID_ARRAY>(inParams[1]);
            }
        }
    }


    /// <summary>
    /// The SamrRemoveMultipleMembersFromAliasResponse class defines output parameters of method SamrRemoveMultipleMembersFromAlias.
    /// </summary>
    public class SamrRemoveMultipleMembersFromAliasResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrRemoveMultipleMembersFromAliasResponse()
        {
            Opnum = SamrMethodOpnums.SamrRemoveMultipleMembersFromAlias;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;

            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                Status
            };
            byte[] responseStub = RpceStubEncoder.ToBytes(
               RpceStubHelper.GetPlatform(),
                SamrRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[]{
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                SamrRpcStubFormatString.ProcFormatString,
                SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                paramList);
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrOemChangePasswordUser2Request class defines input parameters of method SamrOemChangePasswordUser2.
    /// </summary>
    public class SamrOemChangePasswordUser2Request : SamrRequestStub
    {
        /// <summary>
        /// the ServerName parameter
        /// </summary>
        public _RPC_STRING ServerName;

        /// <summary>
        /// the UserName parameter
        /// </summary>
        public _RPC_STRING UserName;

        /// <summary>
        /// the NewPasswordEncryptedWithOldLm parameter
        /// </summary>
        public _SAMPR_ENCRYPTED_USER_PASSWORD NewPasswordEncryptedWithOldLm;

        /// <summary>
        /// the OldLmOwfPasswordEncryptedWithNewLm parameter
        /// </summary>
        public _ENCRYPTED_LM_OWF_PASSWORD OldLmOwfPasswordEncryptedWithNewLm;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrOemChangePasswordUser2Request()
        {
            Opnum = SamrMethodOpnums.SamrOemChangePasswordUser2;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                //BindingHandle = inParams[0].ToIntPtr();
                ServerName = TypeMarshal.ToStruct<_RPC_STRING>(inParams[0]);
                UserName = TypeMarshal.ToStruct<_RPC_STRING>(inParams[1]);
                NewPasswordEncryptedWithOldLm = TypeMarshal.ToStruct<_SAMPR_ENCRYPTED_USER_PASSWORD>(inParams[2]);
                OldLmOwfPasswordEncryptedWithNewLm = TypeMarshal.ToStruct<_ENCRYPTED_LM_OWF_PASSWORD>(inParams[3]);
            }
        }
    }


    /// <summary>
    /// The SamrOemChangePasswordUser2Response class defines output parameters of method SamrOemChangePasswordUser2.
    /// </summary>
    public class SamrOemChangePasswordUser2Response : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrOemChangePasswordUser2Response()
        {
            Opnum = SamrMethodOpnums.SamrOemChangePasswordUser2;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;

            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                Status
            };
            byte[] responseStub = RpceStubEncoder.ToBytes(
               RpceStubHelper.GetPlatform(),
                SamrRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[]{
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                SamrRpcStubFormatString.ProcFormatString,
                SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                paramList);
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrUnicodeChangePasswordUser2Request class defines input parameters of method SamrUnicodeChangePasswordUser2.
    /// </summary>
    public class SamrUnicodeChangePasswordUser2Request : SamrRequestStub
    {
        /// <summary>
        /// the ServerName parameter
        /// </summary>
        public _RPC_UNICODE_STRING ServerName;

        /// <summary>
        /// the UserName parameter
        /// </summary>
        public _RPC_UNICODE_STRING UserName;

        /// <summary>
        /// the NewPasswordEncryptedWithOldNt parameter
        /// </summary>
        public _SAMPR_ENCRYPTED_USER_PASSWORD NewPasswordEncryptedWithOldNt;

        /// <summary>
        /// the OldNtOwfPasswordEncryptedWithNewNt parameter
        /// </summary>
        public _ENCRYPTED_LM_OWF_PASSWORD OldNtOwfPasswordEncryptedWithNewNt;

        /// <summary>
        /// the LmPresent parameter
        /// </summary>
        public byte LmPresent;

        /// <summary>
        /// the NewPasswordEncryptedWithOldLm parameter
        /// </summary>
        public _SAMPR_ENCRYPTED_USER_PASSWORD NewPasswordEncryptedWithOldLm;

        /// <summary>
        /// the OldLmOwfPasswordEncryptedWithNewNt parameter
        /// </summary>
        public _ENCRYPTED_LM_OWF_PASSWORD OldLmOwfPasswordEncryptedWithNewNt;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrUnicodeChangePasswordUser2Request()
        {
            Opnum = SamrMethodOpnums.SamrUnicodeChangePasswordUser2;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ServerName = TypeMarshal.ToStruct<_RPC_UNICODE_STRING>(inParams[0]);
                UserName = TypeMarshal.ToStruct<_RPC_UNICODE_STRING>(inParams[1]);
                NewPasswordEncryptedWithOldNt = TypeMarshal.ToStruct<_SAMPR_ENCRYPTED_USER_PASSWORD>(inParams[2]);
                OldNtOwfPasswordEncryptedWithNewNt = TypeMarshal.ToStruct<_ENCRYPTED_LM_OWF_PASSWORD>(inParams[3]);
                LmPresent = (byte)inParams[4].ToInt32();
                NewPasswordEncryptedWithOldLm = TypeMarshal.ToStruct<_SAMPR_ENCRYPTED_USER_PASSWORD>(inParams[5]);
                OldLmOwfPasswordEncryptedWithNewNt = TypeMarshal.ToStruct<_ENCRYPTED_LM_OWF_PASSWORD>(inParams[6]);
            }
        }
    }


    /// <summary>
    /// The SamrUnicodeChangePasswordUser2Response class defines output parameters of method SamrUnicodeChangePasswordUser2.
    /// </summary>
    public class SamrUnicodeChangePasswordUser2Response : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrUnicodeChangePasswordUser2Response()
        {
            Opnum = SamrMethodOpnums.SamrUnicodeChangePasswordUser2;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;

            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                Status
            };
            byte[] responseStub = RpceStubEncoder.ToBytes(
               RpceStubHelper.GetPlatform(),
                SamrRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[]{
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                SamrRpcStubFormatString.ProcFormatString,
                SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                paramList);
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrGetDomainPasswordInformationRequest class defines input parameters of method SamrGetDomainPasswordInformation.
    /// </summary>
    public class SamrGetDomainPasswordInformationRequest : SamrRequestStub
    {
        /// <summary>
        /// the Unused parameter
        /// </summary>
        public _RPC_UNICODE_STRING Unused;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrGetDomainPasswordInformationRequest()
        {
            Opnum = SamrMethodOpnums.SamrGetDomainPasswordInformation;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                Unused = TypeMarshal.ToStruct<_RPC_UNICODE_STRING>(inParams[0]);
            }
        }
    }


    /// <summary>
    /// The SamrGetDomainPasswordInformationResponse class defines output parameters of method SamrGetDomainPasswordInformation.
    /// </summary>
    public class SamrGetDomainPasswordInformationResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the PasswordInformation parameter
        /// </summary>
        public _USER_DOMAIN_PASSWORD_INFORMATION PasswordInformation;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrGetDomainPasswordInformationResponse()
        {
            Opnum = SamrMethodOpnums.SamrGetDomainPasswordInformation;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pPasswordInformation = TypeMarshal.ToIntPtr(PasswordInformation);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                pPasswordInformation,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pPasswordInformation.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrConnect2Request class defines input parameters of method SamrConnect2.
    /// </summary>
    public class SamrConnect2Request : SamrRequestStub
    {
        /// <summary>
        /// the ServerName parameter
        /// </summary>
        public string ServerName;

        /// <summary>
        /// the DesiredAccess parameter
        /// </summary>
        public uint DesiredAccess;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrConnect2Request()
        {
            Opnum = SamrMethodOpnums.SamrConnect2;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ServerName = Marshal.PtrToStringAuto(inParams[0]);
                DesiredAccess = inParams[1].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The SamrConnect2Response class defines output parameters of method SamrConnect2.
    /// </summary>
    public class SamrConnect2Response : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the ServerHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr ServerHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrConnect2Response()
        {
            Opnum = SamrMethodOpnums.SamrConnect2;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pServerHandle = TypeMarshal.ToIntPtr(ServerHandle);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                pServerHandle,
                IntPtr.Zero,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pServerHandle.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrSetInformationUser2Request class defines input parameters of method SamrSetInformationUser2.
    /// </summary>
    public class SamrSetInformationUser2Request : SamrRequestStub
    {
        /// <summary>
        /// the UserHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr UserHandle;

        /// <summary>
        /// the UserInformationClass parameter
        /// </summary>
        public _USER_INFORMATION_CLASS UserInformationClass;

        /// <summary>
        /// the Buffer parameter [Switch("UserInformationClass")] 
        /// </summary>
        public _SAMPR_USER_INFO_BUFFER Buffer;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrSetInformationUser2Request()
        {
            Opnum = SamrMethodOpnums.SamrSetInformationUser2;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                UserHandle = inParams[0].ToIntPtr();
                UserInformationClass = (_USER_INFORMATION_CLASS)(inParams[1].ToInt32());
                Buffer = TypeMarshal.ToStruct<_SAMPR_USER_INFO_BUFFER>(inParams[2].ToIntPtr(), UserInformationClass, null, null);
            }
        }
    }


    /// <summary>
    /// The SamrSetInformationUser2Response class defines output parameters of method SamrSetInformationUser2.
    /// </summary>
    public class SamrSetInformationUser2Response : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrSetInformationUser2Response()
        {
            Opnum = SamrMethodOpnums.SamrSetInformationUser2;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;

            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                Status
            };
            byte[] responseStub = RpceStubEncoder.ToBytes(
               RpceStubHelper.GetPlatform(),
                SamrRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[]{
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                SamrRpcStubFormatString.ProcFormatString,
                SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                paramList);
            return responseStub;
        }
    }


    /// <summary>
    /// The Opnum59NotUsedOnWireRequest class defines input parameters of method Opnum59NotUsedOnWire.
    /// </summary>
    public class Opnum59NotUsedOnWireRequest : SamrRequestStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum59NotUsedOnWireRequest()
        {
            Opnum = SamrMethodOpnums.Opnum59NotUsedOnWire;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    }


    /// <summary>
    /// The Opnum59NotUsedOnWireResponse class defines output parameters of method Opnum59NotUsedOnWire.
    /// </summary>
    public class Opnum59NotUsedOnWireResponse : SamrResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum59NotUsedOnWireResponse()
        {
            Opnum = SamrMethodOpnums.Opnum59NotUsedOnWire;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            return null;
        }
    }


    /// <summary>
    /// The Opnum60NotUsedOnWireRequest class defines input parameters of method Opnum60NotUsedOnWire.
    /// </summary>
    public class Opnum60NotUsedOnWireRequest : SamrRequestStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum60NotUsedOnWireRequest()
        {
            Opnum = SamrMethodOpnums.Opnum60NotUsedOnWire;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    }


    /// <summary>
    /// The Opnum60NotUsedOnWireResponse class defines output parameters of method Opnum60NotUsedOnWire.
    /// </summary>
    public class Opnum60NotUsedOnWireResponse : SamrResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum60NotUsedOnWireResponse()
        {
            Opnum = SamrMethodOpnums.Opnum60NotUsedOnWire;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            return null;
        }
    }


    /// <summary>
    /// The Opnum61NotUsedOnWireRequest class defines input parameters of method Opnum61NotUsedOnWire.
    /// </summary>
    public class Opnum61NotUsedOnWireRequest : SamrRequestStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum61NotUsedOnWireRequest()
        {
            Opnum = SamrMethodOpnums.Opnum61NotUsedOnWire;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    }


    /// <summary>
    /// The Opnum61NotUsedOnWireResponse class defines output parameters of method Opnum61NotUsedOnWire.
    /// </summary>
    public class Opnum61NotUsedOnWireResponse : SamrResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum61NotUsedOnWireResponse()
        {
            Opnum = SamrMethodOpnums.Opnum61NotUsedOnWire;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            return null;
        }
    }


    /// <summary>
    /// The SamrConnect4Request class defines input parameters of method SamrConnect4.
    /// </summary>
    public class SamrConnect4Request : SamrRequestStub
    {
        /// <summary>
        /// the ServerName parameter
        /// </summary>
        public string ServerName;

        /// <summary>
        /// the ClientRevision parameter
        /// </summary>
        public uint ClientRevision;

        /// <summary>
        /// the DesiredAccess parameter
        /// </summary>
        public uint DesiredAccess;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrConnect4Request()
        {
            Opnum = SamrMethodOpnums.SamrConnect4;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ServerName = Marshal.PtrToStringAuto(inParams[0]);
                ClientRevision = inParams[2].ToUInt32();
                DesiredAccess = inParams[3].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The SamrConnect4Response class defines output parameters of method SamrConnect4.
    /// </summary>
    public class SamrConnect4Response : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the ServerHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr ServerHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrConnect4Response()
        {
            Opnum = SamrMethodOpnums.SamrConnect4;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pServerHandle = TypeMarshal.ToIntPtr(ServerHandle);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                pServerHandle,
                IntPtr.Zero,
                IntPtr.Zero,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pServerHandle.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The Opnum63NotUsedOnWireRequest class defines input parameters of method Opnum63NotUsedOnWire.
    /// </summary>
    public class Opnum63NotUsedOnWireRequest : SamrRequestStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum63NotUsedOnWireRequest()
        {
            Opnum = SamrMethodOpnums.Opnum63NotUsedOnWire;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    }


    /// <summary>
    /// The Opnum63NotUsedOnWireResponse class defines output parameters of method Opnum63NotUsedOnWire.
    /// </summary>
    public class Opnum63NotUsedOnWireResponse : SamrResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum63NotUsedOnWireResponse()
        {
            Opnum = SamrMethodOpnums.Opnum63NotUsedOnWire;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            return null;
        }
    }


    /// <summary>
    /// The SamrConnect5Request class defines input parameters of method SamrConnect5.
    /// </summary>
    public class SamrConnect5Request : SamrRequestStub
    {
        /// <summary>
        /// the ServerName parameter
        /// </summary>
        public string ServerName;

        /// <summary>
        /// the DesiredAccess parameter
        /// </summary>
        public uint DesiredAccess;

        /// <summary>
        /// the InVersion parameter
        /// </summary>
        public uint InVersion;

        /// <summary>
        /// the InRevisionInfo parameter [Switch("InVersion")]
        /// </summary>
        public SAMPR_REVISION_INFO InRevisionInfo;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrConnect5Request()
        {
            Opnum = SamrMethodOpnums.SamrConnect5;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ServerName = Marshal.PtrToStringAuto(inParams[0]);
                DesiredAccess = inParams[1].ToUInt32();
                InVersion = inParams[2].ToUInt32();
                InRevisionInfo = TypeMarshal.ToStruct<SAMPR_REVISION_INFO>(inParams[3], InVersion, null, null);
            }
        }
    }


    /// <summary>
    /// The SamrConnect5Response class defines output parameters of method SamrConnect5.
    /// </summary>
    public class SamrConnect5Response : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the OutVersion parameter
        /// </summary>
        public UInt32 OutVersion;

        /// <summary>
        /// the OutRevisionInfo parameter [Switch("*OutVersion")]
        /// </summary>
        public SAMPR_REVISION_INFO OutRevisionInfo;

        /// <summary>
        /// the ServerHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr ServerHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrConnect5Response()
        {
            Opnum = SamrMethodOpnums.SamrConnect5;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pOutVersion = TypeMarshal.ToIntPtr(OutVersion);
            SafeIntPtr pOutRevisionInfo = TypeMarshal.ToIntPtr(OutRevisionInfo, OutVersion, null, null);
            SafeIntPtr pServerHandle = TypeMarshal.ToIntPtr(ServerHandle);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                pOutVersion,
                pOutRevisionInfo,
                pServerHandle,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pOutVersion.Dispose();
                pOutRevisionInfo.Dispose();
                pServerHandle.Dispose();
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrRidToSidRequest class defines input parameters of method SamrRidToSid.
    /// </summary>
    public class SamrRidToSidRequest : SamrRequestStub
    {
        /// <summary>
        /// the ObjectHandle parameter
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr ObjectHandle;

        /// <summary>
        /// the Rid parameter
        /// </summary>
        public uint Rid;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrRidToSidRequest()
        {
            Opnum = SamrMethodOpnums.SamrRidToSid;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ObjectHandle = inParams[0].ToIntPtr();
                Rid = inParams[1].ToUInt32();
            }
        }
    }


    /// <summary>
    /// The SamrRidToSidResponse class defines output parameters of method SamrRidToSid.
    /// </summary>
    public class SamrRidToSidResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the Sid parameter
        /// </summary>
        public _RPC_SID? Sid;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrRidToSidResponse()
        {
            Opnum = SamrMethodOpnums.SamrRidToSid;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pSid = TypeMarshal.ToIntPtr(Sid);
            IntPtr ppSid = Marshal.AllocHGlobal(IntPtr.Size);
            Marshal.WriteIntPtr(ppSid, pSid);
            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                ppSid,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pSid.Dispose();
                Marshal.FreeHGlobal(ppSid);
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrSetDSRMPasswordRequest class defines input parameters of method SamrSetDSRMPassword.
    /// </summary>
    public class SamrSetDSRMPasswordRequest : SamrRequestStub
    {
        /// <summary>
        /// the Unused parameter
        /// </summary>
        public _RPC_UNICODE_STRING Unused;

        /// <summary>
        /// the UserId parameter
        /// </summary>
        public uint UserId;

        /// <summary>
        /// the EncryptedNtOwfPassword parameter
        /// </summary>
        public _ENCRYPTED_LM_OWF_PASSWORD EncryptedNtOwfPassword;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrSetDSRMPasswordRequest()
        {
            Opnum = SamrMethodOpnums.SamrSetDSRMPassword;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                Unused = TypeMarshal.ToStruct<_RPC_UNICODE_STRING>(inParams[0]);
                UserId = inParams[1].ToUInt32();
                EncryptedNtOwfPassword = TypeMarshal.ToStruct<_ENCRYPTED_LM_OWF_PASSWORD>(inParams[2]);
            }
        }
    }


    /// <summary>
    /// The SamrSetDSRMPasswordResponse class defines output parameters of method SamrSetDSRMPassword.
    /// </summary>
    public class SamrSetDSRMPasswordResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrSetDSRMPasswordResponse()
        {
            Opnum = SamrMethodOpnums.SamrSetDSRMPassword;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;

            paramList = new Int3264[] 
            {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                Status
            };
            byte[] responseStub = RpceStubEncoder.ToBytes(
               RpceStubHelper.GetPlatform(),
                SamrRpcStubFormatString.TypeFormatString,
                new RpceStubExprEval[]{
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                    new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                SamrRpcStubFormatString.ProcFormatString,
                SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                false,
                paramList);
            return responseStub;
        }
    }


    /// <summary>
    /// The SamrValidatePasswordRequest class defines input parameters of method SamrValidatePassword.
    /// </summary>
    public class SamrValidatePasswordRequest : SamrRequestStub
    {
        /// <summary>
        /// the ValidationType parameter
        /// </summary>
        public _PASSWORD_POLICY_VALIDATION_TYPE ValidationType;

        /// <summary>
        /// the InputArg parameter [Switch("ValidationType")]
        /// </summary>
        public _SAM_VALIDATE_INPUT_ARG InputArg;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrValidatePasswordRequest()
        {
            Opnum = SamrMethodOpnums.SamrValidatePassword;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection inParams = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
            SamrRpcStubFormatString.TypeFormatString,
            new RpceStubExprEval[]{
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
            SamrRpcStubFormatString.ProcFormatString,
            SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
            false,
            requestStub,
            sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                ValidationType = (_PASSWORD_POLICY_VALIDATION_TYPE)inParams[0].ToInt32();
                InputArg = TypeMarshal.ToStruct<_SAM_VALIDATE_INPUT_ARG>(inParams[1], ValidationType, null, null);
            }
        }
    }


    /// <summary>
    /// The SamrValidatePasswordResponse class defines output parameters of method SamrValidatePassword.
    /// </summary>
    public class SamrValidatePasswordResponse : SamrResponseStub
    {
        /// <summary>
        /// return value of Rpc method
        /// </summary>
        public int Status;

        /// <summary>
        /// the ValidationType parameter
        /// </summary>
        public _PASSWORD_POLICY_VALIDATION_TYPE ValidationType;

        /// <summary>
        /// the OutputArg parameter [Switch("ValidationType")]
        /// </summary>
        public _SAM_VALIDATE_OUTPUT_ARG? OutputArg;

        /// <summary>
        /// Constructor method
        /// </summary>
        public SamrValidatePasswordResponse()
        {
            Opnum = SamrMethodOpnums.SamrValidatePassword;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            Int3264[] paramList;
            SafeIntPtr pOutputArg = TypeMarshal.ToIntPtr(OutputArg, ValidationType, null, null);
            IntPtr ppOutputArg = Marshal.AllocHGlobal(IntPtr.Size);
            Marshal.WriteIntPtr(ppOutputArg, pOutputArg);
            paramList = new Int3264[] 
            {
                (int)ValidationType,
                IntPtr.Zero,
                ppOutputArg,
                Status
            };
            byte[] responseStub;
            try
            {
                responseStub = RpceStubEncoder.ToBytes(
                   RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[]{
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(SamrRpcAdapter.samr_SAMPR_LOGON_HOURSExprEval_0003)},
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                pOutputArg.Dispose();
                Marshal.FreeHGlobal(ppOutputArg);
            }
            return responseStub;
        }
    }


    /// <summary>
    /// The Opnum68NotUsedOnWireRequest class defines input parameters of method Opnum68NotUsedOnWire.
    /// </summary>
    public class Opnum68NotUsedOnWireRequest : SamrRequestStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum68NotUsedOnWireRequest()
        {
            Opnum = SamrMethodOpnums.Opnum68NotUsedOnWire;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    }


    /// <summary>
    /// The Opnum68NotUsedOnWireResponse class defines output parameters of method Opnum68NotUsedOnWire.
    /// </summary>
    public class Opnum68NotUsedOnWireResponse : SamrResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum68NotUsedOnWireResponse()
        {
            Opnum = SamrMethodOpnums.Opnum68NotUsedOnWire;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            return null;
        }
    }


    /// <summary>
    /// The Opnum69NotUsedOnWireRequest class defines input parameters of method Opnum69NotUsedOnWire.
    /// </summary>
    public class Opnum69NotUsedOnWireRequest : SamrRequestStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum69NotUsedOnWireRequest()
        {
            Opnum = SamrMethodOpnums.Opnum69NotUsedOnWire;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(SamrServerSessionContext sessionContext, byte[] requestStub)
        {
            return;
        }
    }


    /// <summary>
    /// The Opnum69NotUsedOnWireResponse class defines output parameters of method Opnum69NotUsedOnWire.
    /// </summary>
    public class Opnum69NotUsedOnWireResponse : SamrResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public Opnum69NotUsedOnWireResponse()
        {
            Opnum = SamrMethodOpnums.Opnum69NotUsedOnWire;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(SamrServerSessionContext sessionContext)
        {
            return null;
        }
    }
    #endregion

    #endregion
}
