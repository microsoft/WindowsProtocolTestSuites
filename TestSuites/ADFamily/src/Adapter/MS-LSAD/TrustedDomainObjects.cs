// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.DirectoryServices.Protocols;
    using System.Text;

    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk;
    using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa;
    using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
    using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

    /// <summary>
    /// Implement methods of interface ILsadManagedAdapter.
    /// </summary>
    public partial class LsadManagedAdapter
    {
        #region Trusted Variables

        /// <summary>
        /// Variables used in trusted objects
        /// </summary>    
        private bool isDC;

        /// <summary>
        /// check if is current domain
        /// </summary>
        private bool isCurrentDomain;

        /// <summary>
        /// check if is domain present
        /// </summary>
        private bool isDomainPresent;

        /// <summary>
        /// check if is invalid handle
        /// </summary>
        private bool isInvalidHandle;

        /// <summary>
        /// check if set is success
        /// </summary>
        private bool isSetSuccess;

        /// <summary>
        /// check if it is set trusted domain information
        /// </summary>
        private bool isitSetTrustedDomainInfo;

        /// <summary>
        /// check if is in domain administrator group
        /// </summary>
        private bool IsInDomainAdminsGroup;

        /// <summary>
        /// A variable used to indicate the object that is being deleted is Trust Domain object
        /// this is true when the object is a Trust Domain object and false for other objects.
        /// </summary>
        private bool TurstDomainFlag;
                
        /// <summary>
        /// set and initialize the SET_OPERATION,QUERY_OPERATION
        /// </summary>
        private const bool SET_OPERATION = true, QUERY_OPERATION = false;
        
        /// <summary>
        /// set and initialize the const SUCCESS,INVALIDPARAM,INVALIDINFOCLASS
        /// </summary>
        private const int SUCCESS = 0, INVALIDPARAM = 1, INVALIDINFOCLASS = 2;

        /// <summary>
        /// set and initialize the NoAttributes
        /// </summary>
        private const uint NoAttributes = 0x00000000;

        /// <summary>
        /// set and initialize the const OutboundTrust 
        /// </summary>
        private const UInt32 OutboundTrust = 0x00000002;           
 
        /// <summary>
        /// the values for server configuration
        /// </summary>
        private ProtocolServerConfig domainState;

        /// <summary>
        /// set trustedDomainHandle as read only
        /// </summary>
        private System.IntPtr? trustedDomainHandle = IntPtr.Zero;

        /// <summary>
        /// set the validTrustHandle as read only
        /// </summary>
        private System.IntPtr? validTrustHandle = IntPtr.Zero;

        /// <summary>
        /// set the statue to success
        /// </summary>
        private NtStatus uintStatus = NtStatus.STATUS_SUCCESS;

        /// <summary>
        /// the access rights to grant an object
        /// </summary>
        private ACCESS_MASK uintTrustDesiredAccess;

        /// <summary>
        /// Variables used in trusted objects
        /// </summary>
        internal static int intCheckInfoClass;

        /// <summary>
        /// Variables used to specify if Access is Denied.
        /// </summary>
        internal static bool isAccessDenied;

        /// <summary>
        /// Variables used to store trusted domain info.
        /// </summary>
        internal static _LSAPR_TRUSTED_DOMAIN_INFO domainInformationThatWasSet = new _LSAPR_TRUSTED_DOMAIN_INFO();

        /// <summary>
        /// Variables used to store TDO Information.
        /// </summary>
        internal static TDOInformation trustObjectCreateinformation;

        /// <summary>
        /// Variables used to store ForestInformation.
        /// </summary>
        internal static _LSA_FOREST_TRUST_INFORMATION forestInformationThatWasSet = new _LSA_FOREST_TRUST_INFORMATION();

        /// <summary>
        /// Variables used to store a valid policy handle.
        /// </summary>
        internal static System.IntPtr? validPolicyHandle = IntPtr.Zero;

        /// <summary>
        /// Variables used to store the count of invalid parameter.
        /// </summary>
        internal static int invalidParamterCount;
        
        #endregion Trusted Variables

        #region Trusted_Domain_Objects

        #region LsarOpenTrustedDomain

        /// <summary>
        ///  The OpenTrustedDomain method is invoked to
        ///  open a trusted domain object handle by supplying the
        ///  SID of the trusted domain.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="trustedDomainSid">Contains the sid of trusted domain object</param>
        /// <param name="sid">It is for validating the passed in trusted domain sid 
        /// whether it is valid or invalid</param>
        /// <param name="daclAllows">It is for checking whether DACL allows the requested access</param>
        /// <param name="trustHandle">Outparam which contains valid or invalid trust handle</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in policy handle is not valid
        /// Returns NoSuchDomain if the specified trusted domain does not exist</returns>
        public ErrorStatus OpenTrustedDomain(
            int handleInput,
            string trustedDomainSid,
            DomainSid sid,
            bool daclAllows,
            out Handle trustHandle)
        {
            _RPC_SID[] trustSid = new _RPC_SID[1];
            _RPC_UNICODE_STRING tempRPCName = new _RPC_UNICODE_STRING();

            invalidParamterCount = 0;
            PolicyHandle = validPolicyHandle;

            //// Checking if the Handle Passed to model is valid or not.
            //// If it is invalid, an invalid handle will be generated and passed
            if (stPolicyInformation.PHandle != handleInput)
            {
                PolicyHandle = utilities.CreateAnInvalidHandle(false);
                invalidParamterCount++;
            }

            // Checking if The SID passed to the model is already present
            if (trustObjectCreateinformation.strDomainSid.Equals(trustedDomainSid))
            {
                this.isDomainPresent = true;
                trustSid = utilities.GetSid(sid, LsadManagedAdapter.ValidSid);
            }
            else
            {
                this.isDomainPresent = false;
                trustSid = utilities.GetSid(sid, LsadManagedAdapter.NoSid);
                invalidParamterCount++;
            }           

            if (sid == DomainSid.Invalid)
            {
                invalidParamterCount++;
            }

            if (daclAllows)
            {
                this.uintTrustDesiredAccess = ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION;
            }
            else
            {
                this.uintTrustDesiredAccess = (ACCESS_MASK)LsadUtilities.invalidDesiredAccess;
                invalidParamterCount++;
            }

            if (!this.isDC)
            {
                invalidParamterCount++;
            }

            uintMethodStatus = lsadClientStack.LsarOpenTrustedDomain(
                PolicyHandle.Value,
                trustSid[0],
                this.uintTrustDesiredAccess,
                out this.trustedDomainHandle);

            // The TD doesn't specify the behavior, when more than one Invalid Input is passed to the Opnum, 
            // Hence we dont know what the server might return. So we return ErrorStatus.ErrorUnKnown when we 
            // have 2 or more invalid inputs.
            if (invalidParamterCount > 1)
            {
                if (!this.isDC)
                {
                    // Windows specific Requirement
                    if (isWindows && this.PDCOSVersion >= ServerVersion.Win2003)
                    {
                        #region MS-LSAD_R596

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.DirectoryServiceRequired,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            596,
                            @"<77> Section 3.1.4.7.1: On Windows 2000, Windows Server 2003, Windows Server 2008, 
                            and Windows Server 2008 R2, Active Directory MUST be running on the server in order 
                            for the LsarOpenTrustedDomain  request to succeed. Failing that, 
                            STATUS_DIRECTORY_SERVICE_REQUIRED status code is returned.");

                        #endregion
                    }
                }

                trustHandle = Handle.Invalid;
                return ErrorStatus.ErrorUnKnown;
            }

            //// First checking for the Validity of the Data. Returns INVALID_PARAMETER if we give an invalid SID.
            if (sid == DomainSid.Invalid)
            {
                #region MS-LSAD_R591

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidParameter,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    591,
                    @"In LsarOpenTrustedDomain  the server MUST return STATUS_INVALID_PARAMETER
                    when one of the supplied parameters is invalid.");

                #endregion
            }
            else
            {
                if (daclAllows)
                {
                    //// If the passed Handle is not a valid one, we get INVALID_HANDLE
                    if (stPolicyInformation.PHandle != handleInput)
                    {
                        #region MS-LSAD_R593

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.InvalidHandle,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            593,
                            @"In LsarOpenTrustedDomain, If the PolicyHandle given as input is not a valid handle
                            then the Server MUST return STATUS_INVALID_HANDLE.");

                        #endregion
                    }

                    //// Checking if the domain is present or not. Returns NO_SUCH_DOMAIN if Domain is not present.
                    if (!this.isDomainPresent)
                    {
                        #region MS-LSAD_R592

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.NoSuchDomain,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            592,
                            @"In LsarOpenTrustedDomain  the server MUST return STATUS_NO_SUCH_DOMAIN 
                            when the specified trusted domain object does not exist.");

                        #endregion
                    }

                    if (this.isDomainPresent && daclAllows && stPolicyInformation.PHandle == handleInput)
                    {
                        ////Checking to be sure if the Domain is really present or not by enumerating all the
                        //// Trusted Domain Objects present on the Server.
                        if (utilities.CheckTheDomain(trustSid[0], tempRPCName, validPolicyHandle.Value))
                        {
                            #region MS-LSAD_R589

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.Success,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                589,
                                @"The server MUST return STATUS_SUCCESS if LsarOpenTrustedDomain  
                                request was succesfully completed");

                            #endregion
                        }

                        trustHandle = Handle.Valid;
                        return (ErrorStatus)uintMethodStatus;
                    }
                }
                else
                {
                    #region MS-LSAD_R590

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.AccessDenied,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        590,
                        @"In LsarOpenTrustedDomain  The server MUST verify that the trusted domain object's
                        DACL allows the requested access and, failing that, return STATUS_ACCESS_DENIED.");

                    #endregion
                }
            }

            trustHandle = Handle.Invalid;
            return (ErrorStatus)uintMethodStatus;
        }

        #endregion

        #region LsarCreateTrustedDomainEx2
        /// <summary>
        ///  The CreateTrustedDomainEx2 method is invoked to
        ///  create a trusted domain object by supplying the
        ///  name,SID and authentication information for the trusted domain.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="trustedDomainInfo">Contains the information of trusted domain object</param>
        /// <param name="name">It is for validating the passed in trusted domain name
        /// whether it is valid or invalid</param>
        /// <param name="sid">It is for validating the passed in trusted domain sid 
        /// whether it is valid or invalid</param>
        /// <param name="forestFuncLevel">Contains forest functional levels</param>
        /// <param name="isRootDomain">It is to check whether the current domain is a root domain</param>
        /// <param name="desiredAccess">Contains access that is required for the trust handle </param>
        /// <param name="trustHandle">Outparam which contains valid or invalid trust handle</param>
        /// <returns>Returns Success if the method is successful
        ///          Returns InvalidParameter if the parameters passed to the method are not valid
        ///          Returns AccessDenied if the caller does not have the permissions to 
        ///          perform this operation
        ///          Returns InvalidHandle if the passed in policy handle is not valid
        ///          Returns NotSupportedOnSBS if operation is not supported on small business server 2k3
        ///          Returns InvalidDomainState if the operation cannot complete
        ///          in current state of the domain
        ///          Returns DirectoryServiceRequired if the active directory is not available on the server
        ///          Returns InvalidSid if the security identifier of the trusted domain is not valid
        ///          Returns CurrentDomainNotAllowed if the trust cannot be established with the current domain
        ///          Returns ObjectNameCollision if another trusted domain object already exists
        ///          that matches some of the identifying information of the supplied information.</returns>
        /// Disable warning CA1502 and CA1506 because it will affect the implementation of Adapter and Model 
        /// codes if do any changes about maintainability.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", 
            Justification = "affect the implementation of Adapter and Model codes")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", 
            Justification = "affect the implementation of Adapter and Model codes")]
        public ErrorStatus CreateTrustedDomainEx2(
            int handleInput,
            TRUSTED_DOMAIN_INFORMATION_EX trustedDomainInfo,
            ValidString name,
            DomainSid sid,
            ForestFunctionalLevel forestFuncLevel,
            bool isRootDomain,
            UInt32 desiredAccess,
            out Handle trustHandle)
        {
            _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION_INTERNAL[] AuthenticationInformation =
                new _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION_INTERNAL[1];
            _LSAPR_TRUSTED_DOMAIN_INFORMATION_EX[] domainInformation = new _LSAPR_TRUSTED_DOMAIN_INFORMATION_EX[1];
            _LSAPR_TRUSTED_DOMAIN_INFO? domainInfo = new _LSAPR_TRUSTED_DOMAIN_INFO();
            _RPC_UNICODE_STRING[] domainName = new _RPC_UNICODE_STRING[1];
            PolicyHandle = validPolicyHandle;
            invalidParamterCount = 0;
            this.isDomainPresent = false;
            DomainType checkDomainName = DomainType.ValidDomainName;

            //// Checking if Invalid Handle is input to model
            if (stPolicyInformation.PHandle != handleInput)
            {
                PolicyHandle = utilities.CreateAnInvalidHandle(false);
                invalidParamterCount++;
            }

            //// Checking if the Domain Name provided by implementer clashes with the Current Domain
            ////checking if the Name specified is already present.
            if (trustedDomainInfo.TrustDomainName.Equals(Convert.ToString(DomainType.CurrentDomain)))
            {
                checkDomainName = DomainType.CurrentDomain;
                this.isCurrentDomain = true;
                invalidParamterCount++;
            }
            else if (trustedDomainInfo.TrustDomainName.Equals(trustObjectCreateinformation.strTdoDnsName))
            {
                checkDomainName = DomainType.ValidDomainName;
                this.isDomainPresent = true;
                invalidParamterCount++;
            }
            else
            {
                checkDomainName = DomainType.ValidDomainName;
                this.isCurrentDomain = false;
                _RPC_SID[] sidTemp = utilities.GetSid(DomainSid.Valid, LsadManagedAdapter.ValidSid);
                this.uintStatus = lsadClientStack.LsarDeleteTrustedDomain(
                    validPolicyHandle.Value,
                    sidTemp[0]);
            }

            domainInformation[0].Name = new _RPC_UNICODE_STRING();

            //// Get the Domain Name.
            utilities.GetTheDomainName(checkDomainName, ref domainName);

            if (name == ValidString.Valid)
            {
                domainName[0].Length = (ushort)(2 * domainName[0].Buffer.Length);
            }
            else
            {
                domainName[0].Length = (ushort)((2 * domainName[0].Buffer.Length) + 1);
                invalidParamterCount++;
            }

            domainName[0].MaximumLength = (ushort)(2 + domainName[0].Length);
            domainInformation[0].Name = domainName[0];
            domainInformation[0].FlatName = domainName[0];

            if (sid == DomainSid.Valid)
            {
                ////Get the SID
                domainInformation[0].Sid = utilities.GetSid(sid, LsadManagedAdapter.ValidSid);
            }
            else
            {
                invalidParamterCount++;
            }

            domainInformation[0].TrustAttributes = trustedDomainInfo.TrustAttr;
            domainInformation[0].TrustDirection = trustedDomainInfo.TrustDir;
            domainInformation[0].TrustType = (TrustType_Values)trustedDomainInfo.TrustType;

            _LSAPR_AUTH_INFORMATION[] authInfos = new _LSAPR_AUTH_INFORMATION[1];
            authInfos[0].AuthInfo = new byte[3] { 1, 2, 3 };
            authInfos[0].AuthInfoLength = (uint)authInfos[0].AuthInfo.Length;
            authInfos[0].AuthType = AuthType_Values.V1;
            authInfos[0].LastUpdateTime = new _LARGE_INTEGER();
            authInfos[0].LastUpdateTime.QuadPart = 0xcafebeef;
            AuthenticationInformation[0].AuthBlob = LsaUtility.CreateTrustedDomainAuthorizedBlob(
                authInfos,
                null,
                authInfos,
                null,
                lsadAdapter.SessionKey);

            //// Checking if the Domain is in the correct state to perfrom the operation. This is a windows check
            if ((forestFuncLevel == ForestFunctionalLevel.DS_BEHAVIOR_WIN2000
                         && (trustedDomainInfo.TrustAttr == LsadUtilities.TRUST_ATTRIBUTE_FOREST_TRANSITIVE
                                 || trustedDomainInfo.TrustAttr == LsadUtilities.TRUST_ATTRIBUTE_CROSS_ORGANIZATION))
                 || (trustedDomainInfo.TrustAttr == LsadUtilities.TRUST_ATTRIBUTE_FOREST_TRANSITIVE
                         && !isRootDomain)
                 && isWindows)
            {
                invalidParamterCount++;
            }

            if (this.domainState == ProtocolServerConfig.ReadOnlyDomainController)
            {
                invalidParamterCount++;
            }

            if (!this.isDC)
            {
                invalidParamterCount++;
            }

            ////Check if Interdomain Trust Account exist and is valid
            uintMethodStatus = lsadClientStack.LsarCreateTrustedDomainEx2(
                PolicyHandle.Value,
                domainInformation[0],
                AuthenticationInformation[0],
                ACCESS_MASK.MAXIMUM_ALLOWED,
                out this.trustedDomainHandle);

            if (!this.IsInDomainAdminsGroup)
            {
                #region MS-LSAD_R1054

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.AccessDenied,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    1054,
                    @"If the caller is not a member of the Domain Admins group, the server MUST return 
                    STATUS_ACCESS_DENIED for policy handle access checking.");

                #endregion

                trustHandle = Handle.Invalid;
                return (ErrorStatus)uintMethodStatus;
            }

            this.TurstDomainFlag = true;
            trustHandle = Handle.Invalid;

            //// The TD doesn't describe about the protocol Behavior when the opnum has 2 or
            //// more Invalid inputs.So we return ErrorStatus.ErrorUnknown when there are
            //// 2 or more Invalid inputs to the Opnum.
            if (invalidParamterCount > 1)
            {
                if (!this.isDC)
                {
                    #region Non-DC Requirements
                    // Windows Specific Requirement. Windows behaves differently when we 
                    // are trying to create a TDO with either TAFT or TACO set.
                    if (isWindows
                            && (this.PDCOSVersion >= ServerVersion.Win2003 && this.PDCOSVersion <= ServerVersion.Win2008R2)
                            && (trustedDomainInfo.TrustAttr == LsadUtilities.TRUST_ATTRIBUTE_FOREST_TRANSITIVE
                                    || trustedDomainInfo.TrustAttr == LsadUtilities.TRUST_ATTRIBUTE_CROSS_ORGANIZATION))
                    {
                        #region MS-LSAD_R1031

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.InvalidDomainState,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            1031,
                            @"In LsarCreateTrustedDomainEx, Servers running Windows Server 2003, Windows Server 2008 and
                            Windows Server 2008 R2 return the STATUS_INVALID_DOMAIN_STATE error when the 
                            TRUST_ATTRIBUTE_FOREST_TRANSITIVE or the TRUST_ATTRIBUTE_CROSS_ORGANIZATION bit is set in 
                            the TrustAttributes field of the TrustedDomainInformation input parameter when queried on 
                            a Non-Domain Controller.");

                        #endregion MS-LSAD_R1031
                    }
                    else
                    {
                        #region MS-LSAD_686

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.DirectoryServiceRequired,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            686,
                            @"In LsarCreateTrustedDomainEx2, The server MUST return STATUS_DIRECTORY_SERVICE_REQUIRED 
                            when the  Active Directory Service was not available on the server.");

                        #endregion
                    }

                    #endregion Non-DC Requirements
                }
                else
                {
                    #region ReadOnly-DC Requirements

                    if (this.domainState == ProtocolServerConfig.ReadOnlyDomainController)
                    {
                        #region MS-LSAD_R691

                        Site.CaptureRequirementIfAreNotEqual<uint>(
                            (uint)ErrorStatus.Success,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            691,
                            @"In LsarCreateTrustedDomainEx2 , If the server is a read-only domain controller,
                            it MUST return an error.");

                        #endregion MS-LSAD_R691

                        // Windows 2K8 specific Requirement.
                        #region MS-LSAD_R692

                        if (isWindows && PDCOSVersion >= ServerVersion.Win2008)
                        {
                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.ObjectNameNotFound,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                692,
                                @"<85> Section 3.1.4.7.10:In LsarCreateTrustedDomainEx2, If the server is running
                                windows server 2008 or Windows Server 2008 R2 and  is a read-only domain controller, 
                                it MUST return  STATUS_OBJECT_NAME_NOT_FOUND .");
                        }

                        #endregion
                    }

                    #endregion ReadOnly-DC Requirements
                }

                return ErrorStatus.ErrorUnKnown;
            }

            //// Checking for the validity of the RPC_UNICODE_STRING Domain Name
            if (name == ValidString.Invalid)
            {
                #region MS-LSAD_684

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidParameter,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    684,
                    @"In LsarCreateTrustedDomainEx2, The data provided in TrustedDomainInformation parameter MUST 
                    be checked for validity in accordance with rules for trusted domain object consistency specified 
                    in Trust Objects in [MS-ADTS]7.1.6. If the consistency was not accordingly, the server MUST reject
                    invalid input with STATUS_INVALID_PARAMETER.");

                #endregion
            }
            else
            {
                //// Checking if the Domain is in correct state to perform this Operation. Windows Specific Requirment.
                if (!this.isDC)
                {
                    //// Protocol behavior is different when executed on Non-DC windows servers.
                    if (isWindows && (PDCOSVersion == ServerVersion.Win2003 || PDCOSVersion == ServerVersion.Win2008)
                            && (trustedDomainInfo.TrustAttr == LsadUtilities.TRUST_ATTRIBUTE_FOREST_TRANSITIVE
                                    || trustedDomainInfo.TrustAttr == LsadUtilities.TRUST_ATTRIBUTE_CROSS_ORGANIZATION))
                    {
                        #region MS-LSAD_R1031

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.InvalidDomainState,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            1031,
                            @"In LsarCreateTrustedDomainEx, Servers running Windows Server 2003 and Windows Server 2008
                            return the STATUS_INVALID_DOMAIN_STATE error when the TRUST_ATTRIBUTE_FOREST_TRANSITIVE or
                            the TRUST_ATTRIBUTE_CROSS_ORGANIZATION bit is set in the TrustAttributes field of the 
                            TrustedDomainInformation input parameter when queried on a Non-Domain Controller.");

                        #endregion MS-LSAD_R1031
                    }
                    else
                    {
                        #region MS-LSAD_R686

                        Site.CaptureRequirement(
                            "MS-LSAD",
                            686,
                            @"In LsarCreateTrustedDomainEx2, The server MUST return STATUS_DIRECTORY_SERVICE_REQUIRED
                            when the  Active Directory Service was not available on the server.");

                        #endregion
                    }
                }
                else if (this.domainState == ProtocolServerConfig.ReadOnlyDomainController)
                {
                    #region MS-LSAD_R691

                    Site.CaptureRequirementIfAreNotEqual<uint>(
                        (uint)ErrorStatus.Success,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        691,
                        @"In LsarCreateTrustedDomainEx2 , If the server is a read-only domain controller,
                        it MUST return an error.");

                    #endregion MS-LSAD_R691

                    //// Windows Specific Requirement for Read-Only Domain Controllers.
                    #region MS-LSAD_R692

                    if (isWindows && PDCOSVersion >= ServerVersion.Win2008)
                    {
                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.ObjectNameNotFound,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            692,
                            @"In LsarCreateTrustedDomainEx2 , If the server is running windows server 2008 and
                            is a read-only domain controller, it MUST return  STATUS_OBJECT_NAME_NOT_FOUND .");
                    }

                    #endregion
                }
                else if ((forestFuncLevel == ForestFunctionalLevel.DS_BEHAVIOR_WIN2000
                               && (trustedDomainInfo.TrustAttr == LsadUtilities.TRUST_ATTRIBUTE_FOREST_TRANSITIVE
                                       || trustedDomainInfo.TrustAttr == 
                                            LsadUtilities.TRUST_ATTRIBUTE_CROSS_ORGANIZATION))
                          || ((!isRootDomain)
                               && (trustedDomainInfo.TrustAttr == LsadUtilities.TRUST_ATTRIBUTE_FOREST_TRANSITIVE))
                          && isWindows)
                {
                    #region MS-LSAD_R685

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.InvalidDomainState,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        685,
                        @"In LsarCreateTrustedDomainEx2, The server MUST return STATUS_INVALID_DOMAIN_STATE 
                        when the operation cannot complete in the current state of the domain.");

                    #endregion
                }
                else
                {
                    //// If the SID is sent as NULL, INVALID_SID is returned
                    if (sid == DomainSid.Invalid)
                    {
                        #region MS-LSAD_687

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.InvalidSid,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            687,
                            @"In LsarCreateTrustedDomainEx2, The server MUST return STATUS_INVALID_SID when the  
                            security identifier of the trusted domain is not valid.");

                        #endregion
                    }
                    else
                    {
                        //// If we send any other handle than policy handle, Server returns INVALID_HANDLE
                        if (stPolicyInformation.PHandle != handleInput)
                        {
                            #region MS-LSAD_690

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.InvalidHandle,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                690,
                                @"In LsarCreateTrustedDomainEx2, The server MUST return STATUS_INVALID_HANDLE 
                                when PolicyHandle is not a valid handle.");

                            #endregion
                        }
                        else
                        {
                            if (this.isCurrentDomain)
                            {
                                #region MS-LSAD_688

                                Site.CaptureRequirementIfAreEqual<uint>(
                                    (uint)ErrorStatus.CurrentDomainNotAllowed,
                                    (uint)uintMethodStatus,
                                    "MS-LSAD",
                                    688,
                                    @"In LsarCreateTrustedDomainEx2, The server MUST return 
                                    STATUS_CURRENT_DOMAIN_NOT_ALLOWED when trust cannot be
                                    established with the current domain.");

                                #endregion
                            }
                            else
                            {
                                if (this.isDomainPresent)
                                {
                                    #region MS-LSAD_R689

                                    Site.CaptureRequirementIfAreEqual<uint>(
                                        (uint)ErrorStatus.ObjectNameCollision,
                                        (uint)uintMethodStatus,
                                        "MS-LSAD",
                                        689,
                                        @"In LsarCreateTrustedDomainEx2, The server MUST return 
                                        STATUS_OBJECT_NAME_COLLISION when another trusted domain object 
                                        already exists that matches some of the identifying information 
                                        of the supplied information.");

                                    #endregion MS-LSAD_R689
                                }
                                else
                                {
                                    //// Checking to be sure if the TDO is created or not
                                    if (utilities.CheckTheDomain(
                                            domainInformation[0].Sid[0],
                                            domainInformation[0].Name,
                                            validPolicyHandle.Value))
                                    {
                                        #region MS-LSAD_R682

                                        Site.CaptureRequirementIfAreEqual<uint>(
                                            (uint)ErrorStatus.Success,
                                            (uint)uintMethodStatus,
                                            "MS-LSAD",
                                            682,
                                            @"In LsarCreateTrustedDomainEx2, The server MUST return STATUS_SUCCESS when
                                            the request was successfully completed.");

                                        #endregion

                                        #region MS-LSAD_R698

                                        Site.CaptureRequirementIfIsTrue(
                                            (this.trustedDomainHandle != IntPtr.Zero),
                                            "MS-LSAD",
                                            698,
                                            @"In LsarCreateTrustedDomainEx2 in the processing of TrustedDomainHandle, 
                                            TrustedDomainHandle is used to return an open handle to the newly created 
                                            trusted domain object.");

                                        #endregion MS-LSAD_R698

                                        #region MS-LSAD_R693

                                        Site.CaptureRequirementIfIsTrue(
                                            (utilities.IsValidSid(domainInformation[0].Sid[0])
                                                 && utilities.IsRPCStringValid(domainInformation[0].Name)
                                                 && utilities.IsRPCStringValid(domainInformation[0].FlatName)),
                                            "MS-LSAD",
                                            693,
                                            @"In LsarCreateTrustedDomainEx2  in the processing of 
                                            TrustedDomainInformation, The data provided in TrustedDomainInformation
                                            parameter MUST be checked for validity in accordance with rules for 
                                            trusted domain object consistency.");

                                        #endregion MS-LSAD_R693
                                    }

                                    this.uintStatus = lsadClientStack.LsarQueryInfoTrustedDomain(
                                        this.trustedDomainHandle.Value,
                                        _TRUSTED_INFORMATION_CLASS.TrustedDomainFullInformation2Internal,
                                        out domainInfo);

                                    #region MS-LSAD_699

                                    Site.CaptureRequirementIfIsTrue(
                                        (domainInfo.Value.TrustedFullInfo2.Information.ForestTrustInfo == null
                                             && domainInfo.Value.TrustedFullInfo2.Information.ForestTrustLength == 0
                                             && (uint)uintMethodStatus == (uint)ErrorStatus.Success),
                                        "MS-LSAD",
                                        699,
                                        @"New trusted domain objects are always created without forest trust 
                                        information. Both ForestTrustInfo and ForestTrustLength fields of the 
                                        trusted domain object are thus set to NULL and 0, respectively.");

                                    #endregion

                                    trustObjectCreateinformation.doesTdoSupportForestInformation = true;
                                    trustObjectCreateinformation.intTdoHandleNumber = handleInput++;
                                    trustObjectCreateinformation.isForestInformationPresent = false;
                                    trustObjectCreateinformation.strDomainSid = trustedDomainInfo.TrustDomain_Sid;
                                    trustObjectCreateinformation.strTdoDnsName = trustedDomainInfo.TrustDomainName;
                                    trustObjectCreateinformation.strTdoNetBiosName =
                                        trustedDomainInfo.TrustDomain_NetBiosName;

                                    trustObjectCreateinformation.uintTdoDesiredAccess = desiredAccess;
                                    trustObjectCreateinformation.uintTrustAttr = trustedDomainInfo.TrustAttr;
                                    trustObjectCreateinformation.uintTrustDir = trustedDomainInfo.TrustDir;
                                    trustObjectCreateinformation.uintTrustType = trustedDomainInfo.TrustType;
                                    this.validTrustHandle = this.trustedDomainHandle;
                                    trustHandle = Handle.Valid;
                                    this.TurstDomainFlag = true;
                                }
                            }
                        }
                    }
                }
            }

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion

        #region LsarCreateTrustedDomainEx
        /// <summary>
        ///  The CreateTrustedDomainEx method is invoked to
        ///  create a new trusted domain object by supplying
        ///  the Name and SID of the domain.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="trustedDomaininfo">Contains the information of trusted domain object</param>
        /// <param name="name">It is for validating the passed in trusted domain name
        /// whether it is valid or invalid</param>
        /// <param name="sid">It is for validating the passed in trusted domain sid 
        /// whether it is valid or invalid</param>
        /// <param name="forestFuncLevel">Contains forest functional levels</param>
        /// <param name="isRootDomain">It is to check whether the current domain is a root domain</param>
        /// <param name="desiredAccess">Contains access that is required for the trust handle </param>
        /// <param name="authInfo">Contains the authentication information of trusted domain object</param>
        /// <param name="trustHandle">Outparam which contains valid or invalid trust handle</param>
        /// <returns>Returns Success if the method is successful
        ///          Returns InvalidParameter if the parameters passed to the method are not valid
        ///          Returns AccessDenied if the caller does not have the permissions to 
        ///          perform this operation
        ///          Returns InvalidHandle if the passed in policy handle is not valid
        ///          Returns NotSupportedOnSBS if operation is not supported on small business server 2k3
        ///          Returns InvalidDomainState if the operation cannot complete
        ///          in current state of the domain
        ///          Returns DirectoryServiceRequired if the active directory is not available on the server
        ///          Returns InvalidSid if the security identifier of the trusted domain is not valid
        ///          Returns CurrentDomainNotAllowed if the trust cannot be established with the current domain
        ///          Returns ObjectNameCollision if another trusted domain object already exists
        ///          that matches some of the identifying information of the supplied information.</returns>
        /// Disable warning CA1502 and CA1505 because it will affect the implementation of Adapter and Model 
        /// codes if do any changes about maintainability.
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", 
            Justification = "affect the implementation of Adapter and Model codes")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", 
            Justification = "affect the implementation of Adapter and Model codes")]
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", 
            Justification = "affect the implementation of Adapter and Model codes")]
        public ErrorStatus CreateTrustedDomainEx(
            int handleInput,
            TRUSTED_DOMAIN_INFORMATION_EX trustedDomaininfo,
            ValidString name,
            DomainSid sid,
            ForestFunctionalLevel forestFuncLevel,
            bool isRootDomain,
            UInt32 desiredAccess,
            TRUSTED_DOMAIN_AUTH_INFORMATION authInfo,
            out Handle trustHandle)
        {
            _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION[] authInformation = new _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION[1];
            _LSAPR_TRUSTED_DOMAIN_INFORMATION_EX[] trustedDomainInformation = 
                new _LSAPR_TRUSTED_DOMAIN_INFORMATION_EX[1];
            _LSAPR_TRUSTED_DOMAIN_INFO? domainInfo = new _LSAPR_TRUSTED_DOMAIN_INFO();
            _RPC_UNICODE_STRING[] domainName = new _RPC_UNICODE_STRING[1];
            this.isDomainPresent = false;
            invalidParamterCount = 0;
            string strDomainNameCheck = string.Empty;
            PolicyHandle = validPolicyHandle;

            //// Checking if the Model received an Invalid Handle input
            if (stPolicyInformation.PHandle != handleInput)
            {
                PolicyHandle = utilities.CreateAnInvalidHandle(false);
                invalidParamterCount++;
            }

            //// Checking if the Model received a Domain Name that clashes with the Current Domain Name
            //// Checking if Model received a Domain Name that is already present.
            if (trustedDomaininfo.TrustDomainName.Equals(Convert.ToString(DomainType.CurrentDomain)))
            {
                strDomainNameCheck = trustedDomaininfo.TrustDomainName;
                this.isCurrentDomain = true;
                invalidParamterCount++;
                utilities.GetTheDomainName(
                    (DomainType)Enum.Parse(typeof(DomainType), strDomainNameCheck, true),
                    ref domainName);
            }
            else if (trustedDomaininfo.TrustDomainName.Equals(trustObjectCreateinformation.strTdoDnsName))
            {
                this.isDomainPresent = true;
                invalidParamterCount++;
                utilities.GetTheDomainName(DomainType.ValidDomainName, ref domainName);
            }
            else
            {
                this.isCurrentDomain = false;
                _RPC_SID[] sidTemp = utilities.GetSid(DomainSid.Valid, LsadManagedAdapter.ValidSid);
                this.uintStatus = lsadClientStack.LsarDeleteTrustedDomain(
                    validPolicyHandle.Value,
                    sidTemp[0]);

                utilities.GetTheDomainName(DomainType.ValidDomainName, ref domainName);
            }

            //// Checking for the Validity of the Data
            if (name == ValidString.Valid)
            {
                domainName[0].Length = (ushort)(2 * domainName[0].Buffer.Length);
            }
            else
            {
                domainName[0].Length = (ushort)((2 * domainName[0].Buffer.Length) + 1);
            }

            domainName[0].MaximumLength = (ushort)(domainName[0].Length + 2);

            trustedDomainInformation[0].FlatName = domainName[0];
            trustedDomainInformation[0].Name = domainName[0];

            if (sid == DomainSid.Valid)
            {
                trustedDomainInformation[0].Sid = utilities.GetSid(DomainSid.Valid, LsadManagedAdapter.ValidSid);
            }
            else
            {
                invalidParamterCount++;
            }

            trustedDomainInformation[0].TrustAttributes = trustedDomaininfo.TrustAttr;
            trustedDomainInformation[0].TrustDirection = trustedDomaininfo.TrustDir;
            trustedDomainInformation[0].TrustType = (TrustType_Values)trustedDomaininfo.TrustType;

            authInformation[0].IncomingAuthInfos = authInfo.IncomingAuthInfos;
            authInformation[0].OutgoingAuthInfos = authInfo.OutgoingAuthInfos;

            if (desiredAccess <= LsadUtilities.MaxTrustHandleAccess)
            {
                this.uintTrustDesiredAccess = (ACCESS_MASK)desiredAccess;
            }
            else
            {
                this.uintTrustDesiredAccess = (ACCESS_MASK)LsadUtilities.invalidDesiredAccess;
                invalidParamterCount++;
            }

            if ((authInfo.IncomingAuthInfos > 1) || (authInfo.OutgoingAuthInfos > 1) || name == ValidString.Invalid)
            {
                invalidParamterCount++;
            }

            if (!this.isDC)
            {
                invalidParamterCount++;
            }

            //// Checking if the Domain is in the correct state to perform the Operation.
            if ((trustedDomaininfo.TrustAttr == LsadUtilities.TRUST_ATTRIBUTE_FOREST_TRANSITIVE
                     && forestFuncLevel == ForestFunctionalLevel.DS_BEHAVIOR_WIN2000)
                 || (trustedDomaininfo.TrustAttr == LsadUtilities.TRUST_ATTRIBUTE_CROSS_ORGANIZATION
                     && forestFuncLevel == ForestFunctionalLevel.DS_BEHAVIOR_WIN2000)
                 || (trustedDomaininfo.TrustAttr == LsadUtilities.TRUST_ATTRIBUTE_FOREST_TRANSITIVE && !isRootDomain)
                 && isWindows)
            {
                invalidParamterCount++;
            }

            if (this.domainState == ProtocolServerConfig.ReadOnlyDomainController)
            {
                invalidParamterCount++;
            }

            uintMethodStatus = lsadClientStack.LsarCreateTrustedDomainEx(
                PolicyHandle.Value,
                trustedDomainInformation[0],
                authInformation[0],
                this.uintTrustDesiredAccess,
                out this.trustedDomainHandle);

            //// The TD doesn't describe the protocol behavior when the opnum receives 2 or more invalid inputs
            //// To handle the condition, we return ErrorStatus.ErrorUnknown when there are 2 or more Invalid Inputs.
            if (invalidParamterCount > 1)
            {
                if (!this.isDC)
                {
                    #region NON-DC Requirements
                    // Windows Specific Requirement. Windows servers behavior deviates when executed
                    // on NON-DC servers with TAFT or TACO flag set.
                    if (isWindows && (this.PDCOSVersion >= ServerVersion.Win2003 && this.PDCOSVersion <= ServerVersion.Win2008R2)
                            && (trustedDomaininfo.TrustAttr == LsadUtilities.TRUST_ATTRIBUTE_FOREST_TRANSITIVE
                                    || trustedDomaininfo.TrustAttr == LsadUtilities.TRUST_ATTRIBUTE_CROSS_ORGANIZATION))
                    {
                        #region MS-LSAD_R1030

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.InvalidDomainState,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            1030,
                            @"<84> Section 3.1.4.7.10: In LsarCreateTrustedDomainEx2, Servers running
                            Windows Server 2003 and Windows Server 2008 or Windows Server 2008 R2 
                            return the STATUS_INVALID_DOMAIN_STATE error when the 
                            TRUST_ATTRIBUTE_FOREST_TRANSITIVE or the TRUST_ATTRIBUTE_CROSS_ORGANIZATION
                            bit is set in the TrustAttributes field of the TrustedDomainInformation 
                            input parameter when queried on a Non-Domain Controller.");

                        #endregion MS-LSAD_R1030

                        if (!isRootDomain)
                        {
                            #region MS-LSAD_R706

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.InvalidDomainState,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                706,
                                @"In LsarCreateTrustedDomainEx, The server MUST return STATUS_INVALID_DOMAIN_STATE 
                                if the trusted domain information provided in TrustedDomainInformation parameter 
                                cannot be satisfied in the current domain state.");

                            #endregion

                            trustHandle = Handle.Invalid;
                            return (ErrorStatus)uintMethodStatus;
                        }
                    }
                    else
                    {
                        #region MS-LSAD_R707

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.DirectoryServiceRequired,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            707,
                            @"In LsarCreateTrustedDomainEx, The server MUST return STATUS_DIRECTORY_SERVICE_REQUIRED"
                            +" when the Active Directory Service was not available on the server.");
                        #endregion

                        trustHandle = Handle.Invalid;
                        return (ErrorStatus)uintMethodStatus;                       
                    }

                    #endregion NON-DC Requirements
                }
                else
                {
                    #region ReadOnly-DC Requirements

                    if (this.domainState == ProtocolServerConfig.ReadOnlyDomainController)
                    {
                        #region MS-LSAD_R1023

                        Site.CaptureRequirementIfAreNotEqual<uint>(
                            (uint)ErrorStatus.Success,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            1023,
                            @"In LsarCreateTrustedDomainEx, the server MUST return an error if the server is 
                            a Read Only Domain Controller");

                        #endregion MS-LSAD_R1038

                        #region MS-LSAD_R1024

                        if (isWindows && this.PDCOSVersion == ServerVersion.Win2008)
                        {
                            // Windows 2K8 Specific requirement
                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.ObjectNameNotFound,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                1024,
                                @"In LsarCreateTrustedDomainEx, windows2008 server MUST return 
                                STATUS_OBJECT_NAME_NOT_FOUND if it is a Read Only Domain Controller.");
                        }

                        #endregion MS-LSAD_R1024
                    }

                    #endregion ReadOnly-DC Requirements
                }

                trustHandle = Handle.Invalid;
                return ErrorStatus.ErrorUnKnown;
            }

            if (isWindows && this.PDCOSVersion >= ServerVersion.Win2008R2)
            {
                #region MS-LSAD_R11124

                Site.CaptureRequirementIfIsTrue(
                    authInfo.IncomingAuthInfos == 1 || authInfo.IncomingAuthInfos == 0,
                    "MS-LSAD",
                    11124,
                    @"<24> Section 2.2.7.11: The Windows RPC server and client limit the IncomingAuthInfos field 
                    of this structure to be 0 or 1 (using the range primitive defined in [MS-RPCE]) in Windows 7 
                    and Windows Server 2008 R2.");

                #endregion

                #region MS-LSAD_R14124

                Site.CaptureRequirementIfIsTrue(
                    authInfo.OutgoingAuthInfos == 1 || authInfo.OutgoingAuthInfos == 0,
                    "MS-LSAD",
                    14124,
                    @"<25> Section 2.2.7.11: The Windows RPC server and client limit the OutgoingAuthInfos field 
                    of this structure to be 0 or 1 (using the range primitive defined in [MS-RPCE]) in Windows 7 
                    and Windows Server 2008 R2.");

                #endregion
            }
            else
            {
                //// whatever is the value of IncomingAuthInfos, it is acceptable.
                #region MS-LSAD_R12124

                Site.CaptureRequirementIfIsNotNull(
                    authInfo.IncomingAuthInfos,
                    "MS-LSAD",
                    12124,
                    @"<24> Section 2.2.7.11: Other versions[other than Windows 7 and Windows Server 2008 R2] do not 
                    enforce this restriction[the Windows RPC server and client limit the IncomingAuthInfos field of 
                    this structure to be 0 or 1].");

                #endregion

                ////whatever is the value of OutgoingAuthInfos, it is acceptable.
                #region MS-LSAD_R15124

                Site.CaptureRequirementIfIsNotNull(
                    authInfo.OutgoingAuthInfos,
                    "MS-LSAD",
                    15124,
                    @"<25> Section 2.2.7.11: Other versions[other than Windows 7 and Windows Server 2008 R2] do not
                    enforce this restriction[the Windows RPC server and client limit the OutgoingAuthInfos field of
                    this structure to be 0 or 1 ].");

                #endregion
            }

            if (!((authInfo.IncomingAuthInfos > 1) || (authInfo.OutgoingAuthInfos > 1)))
            {
                if (name == ValidString.Invalid)
                {
                    #region MS-LSAD_R703

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidParameter,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    703,
                    @"In LsarCreateTrustedDomainEx, The server MUST reject invalid input
                    with STATUS_INVALID_PARAMETER.");

                #endregion
                }
                else
                {
                    //// Checking if the Domain is in correct state for the successful execution of the Opnum
                    //// This is a windows specific Requirement.
                    if (!this.isDC)
                    {
                        //// Execution of this Opnum on Non-DC windows servers is a bit different when we set the 
                        //// TAFT or the TACO flag.
                        if (isWindows && this.PDCOSVersion >= ServerVersion.Win2003
                                && (trustedDomaininfo.TrustAttr == LsadUtilities.TRUST_ATTRIBUTE_FOREST_TRANSITIVE
                                        || trustedDomaininfo.TrustAttr == 
                                            LsadUtilities.TRUST_ATTRIBUTE_CROSS_ORGANIZATION))
                        {
                            #region MS-LSAD_R1030

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.InvalidDomainState,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                1030,
                                @"<84> Section 3.1.4.7.10: In LsarCreateTrustedDomainEx2, Servers running
                                Windows Server 2003 and Windows Server 2008 or Windows Server 2008 R2 
                                return the STATUS_INVALID_DOMAIN_STATE error when the 
                                TRUST_ATTRIBUTE_FOREST_TRANSITIVE or the TRUST_ATTRIBUTE_CROSS_ORGANIZATION
                                bit is set in the TrustAttributes field of the TrustedDomainInformation 
                                input parameter when queried on a Non-Domain Controller.");

                            #endregion MS-LSAD_R1030
                        }
                        else
                        {
                            #region MS-LSAD_R707

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.DirectoryServiceRequired,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                707,
                                @"In LsarCreateTrustedDomainEx, The server MUST return STATUS_DIRECTORY_SERVICE_REQUIRED"
                                +" when the Active Directory Service was not available on the server.");
                            #endregion
                        }
                    }
                    else if (this.domainState == ProtocolServerConfig.ReadOnlyDomainController)
                    {
                        #region MS-LSAD_R1023

                        Site.CaptureRequirementIfIsTrue(
                            ((uint)uintMethodStatus != (uint)ErrorStatus.Success),
                            "MS-LSAD",
                            1023,
                            @"In LsarCreateTrustedDomainEx, the server MUST return an error if the server 
                            is a Read Only Domain Controller");

                        #endregion MS-LSAD_R1023

                        #region MS-LSAD_R1024

                        if (isWindows && this.PDCOSVersion == ServerVersion.Win2008)
                        {
                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.ObjectNameNotFound,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                1024,
                                @"In LsarCreateTrustedDomainEx, windows2008 server MUST return 
                                STATUS_OBJECT_NAME_NOT_FOUND if it is a Read Only Domain Controller.");
                        }

                        #endregion MS-LSAD_R1024
                    }
                    else if ((forestFuncLevel == ForestFunctionalLevel.DS_BEHAVIOR_WIN2000
                                  && trustedDomaininfo.TrustAttr == LsadUtilities.TRUST_ATTRIBUTE_FOREST_TRANSITIVE)
                              || (forestFuncLevel == ForestFunctionalLevel.DS_BEHAVIOR_WIN2000
                                  && trustedDomaininfo.TrustAttr == LsadUtilities.TRUST_ATTRIBUTE_CROSS_ORGANIZATION)
                              || ((!isRootDomain)
                                  && trustedDomaininfo.TrustAttr == LsadUtilities.TRUST_ATTRIBUTE_FOREST_TRANSITIVE)
                              && isWindows)
                    {
                        #region MS-LSAD_R706

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.InvalidDomainState,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                           706,
                           @"In LsarCreateTrustedDomainEx, The server MUST return STATUS_INVALID_DOMAIN_STATE 
                           if the trusted domain information provided in TrustedDomainInformation parameter 
                           cannot be satisfied in the current domain state.");

                        #endregion
                    }
                    else
                    {
                        //// Server returns INVALID_SID when a NULL sid is passed to Create a TDO
                        if (sid == DomainSid.Invalid)
                        {
                            #region MS-LSAD_R708

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.InvalidSid,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                708,
                                @"In LsarCreateTrustedDomainEx, The server MUST return (uint)ErrorStatus.INVALID_SID
                                when the security identifier of the trusted domain is not valid.");

                            #endregion
                        }
                        else
                        {
                            //// Checking for the Invalid Handle Condition
                            if (stPolicyInformation.PHandle != handleInput)
                            {
                                #region MS-LSAD_R711

                                Site.CaptureRequirementIfAreEqual<uint>(
                                    (uint)ErrorStatus.InvalidHandle,
                                    (uint)uintMethodStatus,
                                    "MS-LSAD",
                                    711,
                                    @"In LsarCreateTrustedDomainEx, If the handle is not a valid context handle to the
                                    policy object, the server MUST return STATUS_INVALID_HANDLE.");

                                #endregion
                            }
                            else
                            {
                                if (this.isDC)
                                {
                                    //// If the Name provided to Create a TDO clashes with the name of the Domain
                                    // the computer is part of CURRENT_DOMAIN_NOT_ALLOWED is returned.
                                    if (this.isCurrentDomain)
                                    {
                                        #region MS-LSAD_R709

                                        Site.CaptureRequirementIfAreEqual<uint>(
                                            (uint)ErrorStatus.CurrentDomainNotAllowed,
                                            (uint)uintMethodStatus,
                                            "MS-LSAD",
                                            709,
                                            @"In LsarCreateTrustedDomainEx, If one or more properties in 
                                            TrustedDomainInformation points to the current domain (such as the domain
                                            that the server is a part of), the server MUST return
                                            STATUS_CURRENT_DOMAIN_NOT_ALLOWED.");

                                        #endregion
                                    }
                                    else if (this.isDomainPresent)
                                    {
                                        #region MS-LSAD_R710

                                        Site.CaptureRequirementIfAreEqual<uint>(
                                            (uint)ErrorStatus.ObjectNameCollision,
                                            (uint)uintMethodStatus,
                                            "MS-LSAD",
                                            710,
                                            @"In LsarCreateTrustedDomainEx, If there is another domain that claims the
                                            same properties, the server MUST return STATUS_OBJECT_NAME_COLLISION. ");

                                        #endregion MS-LSAD_R710
                                    }
                                    else
                                    {
                                        //// Checking to be sure if the TDO is created or not by enumerating all the TDOs
                                        if (utilities.CheckTheDomain(
                                            trustedDomainInformation[0].Sid[0],
                                            trustedDomainInformation[0].Name,
                                            validPolicyHandle.Value))
                                        {
                                            #region MS-LSAD_R701
                                            Site.CaptureRequirementIfAreEqual<uint>(
                                                (uint)ErrorStatus.Success,
                                                (uint)uintMethodStatus,
                                                "MS-LSAD",
                                                701,
                                                @"In LsarCreateTrustedDomainEx, The server MUST return STATUS_SUCCESS
                                                when the request was successfully completed. ");

                                            #endregion
                                        }

                                        #region MS-LSAD_R722

                                        this.uintStatus = lsadClientStack.LsarQueryInfoTrustedDomain(
                                            this.trustedDomainHandle.Value,
                                            _TRUSTED_INFORMATION_CLASS.TrustedDomainFullInformation2Internal,
                                            out domainInfo);

                                        Site.CaptureRequirementIfIsTrue(
                                            (domainInfo.Value.TrustedFullInfo2.Information.ForestTrustLength == 0
                                                && domainInfo.Value.TrustedFullInfo2.Information.ForestTrustInfo == null
                                                && (uint)uintMethodStatus == (uint)ErrorStatus.Success),
                                            "MS-LSAD",
                                            722,
                                            @"In LsarCreateTrustedDomainEx, TrustedDomainHandle Used to return an open 
                                            handle to the newly created trusted domain object. Both ForestTrustInfo and 
                                            ForestTrustLength fields of the trusted domain object are thus set to 
                                            NULL and 0 respectively.");

                                        #endregion MS-LSAD_R722

                                        #region MS-LSAD_R704

                                        Site.CaptureRequirementIfIsTrue(
                                            (utilities.IsValidSid(trustedDomainInformation[0].Sid[0])
                                                 && utilities.IsRPCStringValid(trustedDomainInformation[0].FlatName)
                                                 && utilities.IsRPCStringValid(trustedDomainInformation[0].Name)),
                                            "MS-LSAD",
                                            704,
                                            @"In LsarCreateTrustedDomainEx, The data provided in 
                                            TrustedDomainInformation parameter MUST be checked for validity in 
                                            accordance with rules for trusted domain object consistency.");

                                        #endregion MS-LSAD_R704

                                        trustObjectCreateinformation.intTdoHandleNumber = 
                                            (int)stPolicyInformation.PHandle + 1;
                                        trustObjectCreateinformation.strTdoDnsName = trustedDomaininfo.TrustDomainName;
                                        trustObjectCreateinformation.strTdoNetBiosName = 
                                            trustedDomaininfo.TrustDomain_NetBiosName;
                                        trustObjectCreateinformation.strDomainSid = trustedDomaininfo.TrustDomain_Sid;
                                        trustObjectCreateinformation.uintTdoDesiredAccess =
                                            (desiredAccess | (uint)ACCESS_MASK.TRUSTED_SET_AUTH);

                                        trustObjectCreateinformation.uintTrustAttr = trustedDomaininfo.TrustAttr;
                                        trustObjectCreateinformation.uintTrustDir = trustedDomaininfo.TrustDir;
                                        trustObjectCreateinformation.uintTrustType = trustedDomaininfo.TrustType;
                                        trustObjectCreateinformation.doesTdoSupportForestInformation = true;
                                        trustObjectCreateinformation.isForestInformationPresent = false;

                                        this.TurstDomainFlag = true;
                                        this.validTrustHandle = this.trustedDomainHandle;
                                        trustHandle = Handle.Valid;
                                        return (ErrorStatus)uintMethodStatus;
                                    }
                                }
                                else
                                {
                                    if (isWindows && this.PDCOSVersion >= ServerVersion.Win2003
                                            && trustedDomaininfo.TrustAttr == 
                                                LsadUtilities.TRUST_ATTRIBUTE_FOREST_TRANSITIVE
                                            || trustedDomaininfo.TrustAttr == 
                                                LsadUtilities.TRUST_ATTRIBUTE_CROSS_ORGANIZATION)
                                    {
                                        #region MS-LSAD_R1030

                                        Site.CaptureRequirementIfAreEqual<uint>(
                                            (uint)ErrorStatus.InvalidDomainState,
                                            (uint)uintMethodStatus,
                                            "MS-LSAD",
                                            1030,
                                            @"<84> Section 3.1.4.7.10: In LsarCreateTrustedDomainEx2, Servers running
                                            Windows Server 2003 and Windows Server 2008 or Windows Server 2008 R2 
                                            return the STATUS_INVALID_DOMAIN_STATE error when the 
                                            TRUST_ATTRIBUTE_FOREST_TRANSITIVE or the TRUST_ATTRIBUTE_CROSS_ORGANIZATION
                                            bit is set in the TrustAttributes field of the TrustedDomainInformation 
                                            input parameter when queried on a Non-Domain Controller.");

                                        #endregion MS-LSAD_R1030
                                    }
                                    else
                                    {
                                        #region MS-LSAD_R707

                                        Site.CaptureRequirementIfAreEqual<uint>(
                                            (uint)ErrorStatus.DirectoryServiceRequired,
                                            (uint)uintMethodStatus,
                                            "MS-LSAD",
                                            707,
                                            @"In LsarCreateTrustedDomainEx, The server MUST return STATUS_DIRECTORY_SERVICE_REQUIRED"
                                            +" when the Active Directory Service was not available on the server.");
                                        #endregion
                                    }
                                }
                            }
                        }
                    }
                }
            }

            trustHandle = Handle.Invalid;

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion

        #region LsarCreateTrustedDomain

        /// <summary>
        ///  The CreateTrustedDomain method is invoked to create
        ///  an object of type trusted domain in the server's database.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="trustedDomainInfo">Contains the information of trusted domain object</param>
        /// <param name="sid">It is for validating the passed in trusted domain sid 
        /// whether it is valid or invalid</param>
        /// <param name="isRootDomain">It is to check whether the current domain is a root domain</param>
        /// <param name="name">It is for validating the passed in trusted domain name
        /// whether it is valid or invalid</param>
        /// <param name="desiredAccess">Contains access that is required for the trust handle </param>
        /// <param name="trustHandle">Outparam which contains valid or invalid trust handle</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in policy handle is not valid
        /// Returns NotSupportedOnSBS if operation is not supported on small business server 2k3
        /// Returns InvalidDomainState if the operation cannot complete
        /// in current state of the domain
        /// Returns DirectoryServiceRequired if the active directory is not available on the server
        /// Returns InvalidSid if the security identifier of the trusted domain is not valid
        /// Returns CurrentDomainNotAllowed if the trust cannot be established with the current domain
        /// Returns ObjectNameCollision if another trusted domain object already exists
        /// that matches some of the identifying information of the supplied information.</returns>
        public ErrorStatus CreateTrustedDomain(
            int handleInput,
            TRUSTED_DOMAIN_INFORMATION_EX trustedDomainInfo,
            DomainSid sid,
            bool isRootDomain,
            ValidString name,
            UInt32 desiredAccess,
            out Handle trustHandle)
        {
            _LSAPR_TRUST_INFORMATION[] trustInfo = new _LSAPR_TRUST_INFORMATION[1];
            _LSAPR_TRUSTED_DOMAIN_INFO? domainInfo = new _LSAPR_TRUSTED_DOMAIN_INFO();

            _RPC_UNICODE_STRING[] domainName = new _RPC_UNICODE_STRING[1];
            _RPC_SID[] Tsid = new _RPC_SID[1];
            string strTrustDomainName = string.Empty;
            invalidParamterCount = 0;
            PolicyHandle = validPolicyHandle;
            this.isDomainPresent = false;

            //// Checking if the model received invalid handle as input
            if (stPolicyInformation.PHandle != handleInput)
            {
                PolicyHandle = utilities.CreateAnInvalidHandle(false);
                invalidParamterCount++;
            }

            //// Checking if the model recieved the TDO name as the Current Domain Name
            //// Checking to see that the TDO name input to model is unique (for avoiding object name collision)
            if (trustedDomainInfo.TrustDomainName.Equals(Convert.ToString(DomainType.CurrentDomain)))
            {
                strTrustDomainName = trustedDomainInfo.TrustDomainName;
                this.isCurrentDomain = true;
                invalidParamterCount++;
                utilities.GetTheDomainName(
                    (DomainType)Enum.Parse(typeof(DomainType), strTrustDomainName, true),
                    ref domainName);
            }
            else if (trustedDomainInfo.TrustDomainName.Equals(trustObjectCreateinformation.strTdoDnsName))
            {
                this.isDomainPresent = true;
                invalidParamterCount++;
                utilities.GetTheDomainName(DomainType.ValidDomainName, ref domainName);
            }
            else
            {
                Tsid = utilities.GetSid(DomainSid.Valid, LsadManagedAdapter.ValidSid);
                this.isCurrentDomain = false;
                this.uintStatus = lsadClientStack.LsarDeleteTrustedDomain(
                    validPolicyHandle.Value,
                    Tsid[0]);
                utilities.GetTheDomainName(DomainType.ValidDomainName, ref domainName);
            }

            trustInfo[0].Name = domainName[0];

            if (name == ValidString.Valid)
            {
                domainName[0].Length = (ushort)(2 * domainName[0].Buffer.Length);
            }
            else
            {
                domainName[0].Length = (ushort)((2 * domainName[0].Buffer.Length) + 1);
                invalidParamterCount++;
            }

            domainName[0].MaximumLength = (ushort)(domainName[0].Length + 2);
            trustInfo[0].Name = domainName[0];

            if (sid == DomainSid.Valid)
            {
                trustInfo[0].Sid = utilities.GetSid(DomainSid.Valid, LsadManagedAdapter.ValidSid);
            }
            else
            {
                invalidParamterCount++;
            }

            // Checking to see the access requested has valid bits set.
            if (desiredAccess <= LsadUtilities.MaxTrustHandleAccess)
            {
                this.uintTrustDesiredAccess = ACCESS_MASK.TRUSTED_SET_AUTH;
                this.uintTrustDesiredAccess = ACCESS_MASK.MAXIMUM_ALLOWED;
            }
            else
            {
                this.uintTrustDesiredAccess = (ACCESS_MASK)LsadUtilities.invalidDesiredAccess;
                invalidParamterCount++;
            }

            if (this.domainState == ProtocolServerConfig.ReadOnlyDomainController
                    || this.domainState == ProtocolServerConfig.NonDomainController)
            {
                invalidParamterCount++;
            }

            uintMethodStatus = lsadClientStack.LsarCreateTrustedDomain(
                PolicyHandle.Value,
                trustInfo[0],
                this.uintTrustDesiredAccess,
                out this.trustedDomainHandle);

            // The TD doesn't describe the protocol behavior when the opnum receives 2 or more invalid inputs
            // To handle the condition, we return ErrorStatus.ErrorUnknown when there are 2 or more Invalid Inputs.
            if (invalidParamterCount > 1)
            {
                if (!this.isDC)
                {
                    #region MS-LSAD_R729

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.DirectoryServiceRequired,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        729,
                        @"In LsarCreateTrustedDomain, The server MUST return STATUS_DIRECTORY_SERVICE_REQUIRED
                        when the Active Directory Service was not available on the server.");

                    #endregion
                }

                trustHandle = Handle.Invalid;
                return ErrorStatus.ErrorUnKnown;
            }

            if (name == ValidString.Invalid)
            {
                #region MS-LSAD_R726

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidParameter,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    726,
                    @"In LsarCreateTrustedDomain, The server MUST return STATUS_INVALID_PARAMETER when the caller 
                    does not have the permissions to perform this operation.");

                #endregion
            }
            else if (sid == DomainSid.Invalid)
            {
                #region MS-LSAD_R730

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidSid,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    730,
                    @"In LsarCreateTrustedDomain, The server MUST turn STATUS_INVALID_SID when the security 
                    identifier of the trusted domain is not valid.");

                #endregion
            }
            else if (!this.isDC)
            {
                #region MS-LSAD_R729

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.DirectoryServiceRequired,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    729,
                    @"In LsarCreateTrustedDomain, The server MUST return STATUS_DIRECTORY_SERVICE_REQUIRED 
                    when the Active Directory Service was not available on the server.");

                #endregion
            }
            else if (this.domainState == ProtocolServerConfig.ReadOnlyDomainController)
            {
                #region MS-LSAD_R1025

                Site.CaptureRequirementIfIsTrue(
                    (uint)uintMethodStatus != (uint)ErrorStatus.Success,
                    "MS-LSAD",
                    1025,
                    @"In LsarCreateTrustedDomain, the server MUST return an error if the server is
                    a Read Only Domain Controller");

                #endregion MS-LSAD_R1025

                #region MS-LSAD_R1026

                if (isWindows && this.PDCOSVersion == ServerVersion.Win2008)
                {
                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.ObjectNameNotFound,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        1026,
                        @"In LsarCreateTrustedDomain, windows2008 server MUST return STATUS_OBJECT_NAME_NOT_FOUND 
                        if it is a Read Only Domain Controller");
                }

                #endregion MS-LSAD_R1026
            }
            else if (desiredAccess > LsadUtilities.MaxTrustHandleAccess)
            {
                #region MS-LSAD_R725

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.AccessDenied,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    725,
                    @"In LsarCreateTrustedDomain, The server MUST return STATUS_ACCESS_DENIED when the caller 
                    does not have the permissions to perform this operation.");

                #endregion
            }
            else if (this.isCurrentDomain)
            {
                #region MS-LSAD_R731

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.CurrentDomainNotAllowed,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    731,
                    @"In LsarCreateTrustedDomain, The server MUST return STATUS_CURRENT_DOMAIN_NOT_ALLOWED
                    when trust cannot be established with the current domain.");

                #endregion
            }
            else if (this.isDomainPresent)
            {
                #region MS-LSAD_R732

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.ObjectNameCollision,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    732,
                    @"In LsarCreateTrustedDomain, The server MUST return STATUS_OBJECT_NAME_COLLISION when 
                    another trusted domain object already exists that matches some of the identifying information 
                    of the supplied information like Trusted Domain Name or Sid.");

                #endregion MS-LSAD_R732
            }
            else if (stPolicyInformation.PHandle != handleInput)
            {
                #region MS-LSAD_R733

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidHandle,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    733,
                    @"In LsarCreateTrustedDomain, If the handle is not a valid context handle to the policy object,
                    the server MUST return STATUS_INVALID_HANDLE.");

                #endregion
            }
            else if (trustObjectCreateinformation.strTdoDnsName.Equals(trustedDomainInfo.TrustDomainName))
            {
                #region MS-LSAD_R732

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.ObjectNameCollision,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    732,
                    @"In LsarCreateTrustedDomain, The server MUST return STATUS_OBJECT_NAME_COLLISION when another
                    trusted domain object already exists that matches some of the identifying information of the
                    supplied information like Trusted Domain Name or Sid.");

                #endregion
            }
            else
            {
                // Checking to be sure that the TDO got created by Enumerating all the TDOs
                if (utilities.CheckTheDomain(trustInfo[0].Sid[0], trustInfo[0].Name, validPolicyHandle.Value))
                {
                    #region MS-LSAD_R724

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.Success,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        724,
                        @"In LsarCreateTrustedDomain, The server MUST return STATUS_SUCCESS when the
                        request was successfully completed");

                    #endregion

                    #region MS-LSAD_R736

                    this.uintStatus = lsadClientStack.LsarQueryInfoTrustedDomain(
                        this.trustedDomainHandle.Value,
                        _TRUSTED_INFORMATION_CLASS.TrustedDomainFullInformation2Internal,
                        out domainInfo);

                    Site.CaptureRequirementIfIsTrue(
                        (domainInfo.Value.TrustedFullInfo2.Information.ForestTrustInfo == null
                             && domainInfo.Value.TrustedFullInfo2.Information.ForestTrustLength == 0),
                        "MS-LSAD",
                        736,
                        @"In LsarCreateTrustedDomain in the TrustedDomainHandle, TrustedDomainHandle is used to
                        return the newly created trusted domain object in which Both ForestTrustInfo and 
                        ForestTrustLength fields of the trusted domain object are thus set to NULL and 0
                        respectively.");

                    #endregion
                }

                this.validTrustHandle = this.trustedDomainHandle;
                checkTrustHandle = true;
                trustObjectCreateinformation.intTdoHandleNumber = (int)LsadUtilities.INITIALISED_HANDLE + 1;
                trustObjectCreateinformation.strTdoDnsName = trustedDomainInfo.TrustDomainName;
                trustObjectCreateinformation.strTdoNetBiosName = trustedDomainInfo.TrustDomain_NetBiosName;
                trustObjectCreateinformation.strDomainSid = trustedDomainInfo.TrustDomain_Sid;
                trustObjectCreateinformation.uintTdoDesiredAccess = 
                    (desiredAccess | (uint)ACCESS_MASK.TRUSTED_SET_AUTH);
                trustObjectCreateinformation.uintTrustAttr = NoAttributes;
                trustObjectCreateinformation.uintTrustDir = OutboundTrust;
                trustObjectCreateinformation.uintTrustType = (uint)TrustType_Values.NoneActiveDirectory;
                trustObjectCreateinformation.isForestInformationPresent = false;
                trustObjectCreateinformation.doesTdoSupportForestInformation = false;

                if (this.trustedDomainHandle.Value != IntPtr.Zero)
                {
                    trustHandle = Handle.Valid;
                    this.TurstDomainFlag = true;
                }
                else
                {
                    trustHandle = Handle.Invalid;
                }

                return (ErrorStatus)uintMethodStatus;
            }

            trustHandle = Handle.Invalid;

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion

        #region LsarOpenTrustedDomainByName

        /// <summary>
        ///  The OpenTrustedDomainByName method is invoked to
        ///  obtain a trusted domain object handle by supplying the
        ///  name of the trusted domain.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="trustedDomainName">Contains the name of trusted domain object</param>
        /// <param name="name">It is for validating the passed in trusted domain name
        /// whether it is valid or invalid</param>
        /// <param name="desiredAccess">Contains access that is required for the trust handle </param>
        /// <param name="trustHandle">Outparam which contains valid or invalid trust handle</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in policy handle is not valid
        /// Returns ObjectNameNotFound if trusted domain object by passed in name was not found</returns>
        public ErrorStatus OpenTrustedDomainByName(
            int handleInput,
            string trustedDomainName,
            ValidString name,
            UInt32 desiredAccess,
            out Handle trustHandle)
        {
            _RPC_UNICODE_STRING[] domainName = new _RPC_UNICODE_STRING[1];
            _RPC_SID[] tempRPCSid = new _RPC_SID[1];
            DomainType checkDomainName = DomainType.ValidDomainName;
            PolicyHandle = validPolicyHandle;
            invalidParamterCount = 0;

            //// Checking if the Model received and invalid handle
            if (stPolicyInformation.PHandle != handleInput)
            {
                PolicyHandle = utilities.CreateAnInvalidHandle(false);
                invalidParamterCount++;
            }

            //// Checking if the Name of the TDO received by the model is already present
            if (trustObjectCreateinformation.strTdoDnsName.Equals(trustedDomainName))
            {
                checkDomainName = DomainType.ValidDomainName;
                this.isDomainPresent = true;
            }
            else
            {
                checkDomainName = DomainType.NoDomainName;
                this.isDomainPresent = false;
                invalidParamterCount++;
            }

            // Initialize the TDO name
            utilities.GetTheDomainName(checkDomainName, ref domainName);

            if (name == ValidString.Valid)
            {
                domainName[0].Length = (ushort)(domainName[0].Buffer.Length * 2);
            }
            else
            {
                domainName[0].Length = (ushort)((domainName[0].Buffer.Length * 2) + 1);
                invalidParamterCount++;
            }

            domainName[0].MaximumLength = (ushort)(domainName[0].Length + 2);

            if ((desiredAccess & LsadUtilities.invalidDesiredAccess) != 0)
            {
                uintAccessMask = (ACCESS_MASK)LsadUtilities.invalidDesiredAccess;
                invalidParamterCount++;
            }
            else
            {
                uintAccessMask = ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION;
            }

            uintMethodStatus = lsadClientStack.LsarOpenTrustedDomainByName(
                PolicyHandle.Value,
                domainName[0],
                uintAccessMask,
                out this.trustedDomainHandle);

            // The TD doesn't describe the protocol behavior when the opnum receives 2 or more invalid inputs
            // To handle the condition, we return ErrorStatus.ErrorUnknown when there are 2 or more Invalid Inputs.
            if (invalidParamterCount > 1)
            {
                trustHandle = Handle.Invalid;
                return ErrorStatus.ErrorUnKnown;
            }

            // Checking to see if Data Validation fails
            if (name == ValidString.Invalid)
            {
                #region MS-LSAD_R677
                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidParameter,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    677,
                    @"In LsarOpenTrustedDomainByName, The server MUST return STATUS_INVALID_PARAMETER
                    when one of the supplied arguments was invalid.");

                #endregion
            }
            else
            {
                if ((desiredAccess & LsadUtilities.invalidDesiredAccess) != 0)
                {
                    #region MS-LSAD_R679

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.AccessDenied,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        679,
                        @"In LsarOpenTrustedDomainByName  in the processing of DesiredAccess, The server MUST 
                        check this against the security descriptor of the trusted domain object, and fail the request
                        with STATUS_ACCESS_DENIED if the access check fails");

                    #endregion
                }
                else
                {
                    // If an invalid Handle is passed, it returns INVALID_HANDLE
                    if (stPolicyInformation.PHandle != handleInput)
                    {
                        #region MS-LSAD_R678

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.InvalidHandle,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            678,
                            @"In LsarOpenTrustedDomainByName, If the handle is not a valid context handle to the
                            policy object, the server MUST return STATUS_INVALID_HANDLE.");

                        #endregion
                    }
                    else
                    {
                        // When no TDO is found with the specified TDO name, server returns OBJECT_NAME_NOT_FOUND
                        if (!this.isDomainPresent)
                        {
                            #region MS-LSAD_R1001

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.ObjectNameNotFound,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                1001,
                                @"In LsarOpenTrustedDomainByName, The server MUST return STATUS_OBJECT_NAME_NOT_FOUND
                                if a trusted Domain Object by the specified name could not be found.");

                            #endregion
                        }
                        else
                        {
                            // Checking if the Domain really exists or not
                            if (utilities.CheckTheDomain(tempRPCSid[0], domainName[0], validPolicyHandle.Value))
                            {
                                #region MS-LSAD_R675

                                Site.CaptureRequirementIfAreEqual<uint>(
                                    (uint)ErrorStatus.Success,
                                    (uint)uintMethodStatus,
                                    "MS-LSAD",
                                    675,
                                    @"In LsarOpenTrustedDomainByName, The server MUST return STATUS_SUCCESS 
                                    when the request was successfully completed.");

                                #endregion

                                #region MS-LSAD_R680

                                Site.CaptureRequirementIfIsTrue(
                                    this.trustedDomainHandle != IntPtr.Zero,
                                    "MS-LSAD",
                                    680,
                                    @"In LsarOpenTrustedDomainByName  in the processing of TrustedDomainHandle,
                                    TrustedDomainHandle is used to return the open handle to the caller if the
                                    request is successful");

                                #endregion
                            }

                            trustHandle = Handle.Valid;
                            return (ErrorStatus)uintMethodStatus;
                        }
                    }
                }
            }

            trustHandle = Handle.Invalid;

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion

        #region LsarQueryInfoTrustedDomain

        /// <summary>
        ///  The QueryInfoTrustedDomain method is invoked to
        ///  retrieve information about the trusted domain object.
        ///  Identifies the Trusted Domain Object by an Open Trusted Domain Handle
        /// </summary>
        /// <param name="handleInput">Contains trust handle obtained from 
        /// CreateTrustedDomainEx2/CreateTrustedDomainEx/CreateTrustedDomain </param>
        /// <param name="trustedInformation">Contains the type of trusted domain object information</param>
        /// <param name="forestFunctionLevel">Contains forest functional levels</param>
        /// <param name="trustDomainInfo">Outparam which contains valid or invalid trusted 
        /// domain object information</param>
        /// <returns>Returns Success if the method is successful
        ///          Returns InvalidParameter if the parameters passed to the method are not valid
        ///          Returns AccessDenied if the caller does not have the permissions to 
        ///          perform this operation
        ///          Returns InvalidHandle if the passed in policy handle is not valid
        ///          Returns InvalidInfoClass if the InformationClass argument is outside
        ///          the allowed range</returns>
        /// Disable warning CA1502 because it will affect the implementation of Adapter and Model codes 
        /// if do any changes about maintainability.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", 
            Justification = "affect the implementation of Adapter and Model codes")]
        public ErrorStatus QueryInfoTrustedDomain(
            int handleInput,
            TrustedInformationClass trustedInformation,
            ForestFunctionalLevel forestFunctionLevel,
            out TrustedDomainInformation trustDomainInfo)
        {
            _LSAPR_TRUSTED_DOMAIN_INFO domainInformation = new _LSAPR_TRUSTED_DOMAIN_INFO();
            _LSAPR_TRUSTED_DOMAIN_INFO? queryInformation = new _LSAPR_TRUSTED_DOMAIN_INFO();
            TRUSTED_DOMAIN_INFORMATION_EX trustedDomainInfo = new TRUSTED_DOMAIN_INFORMATION_EX();
            _TRUSTED_INFORMATION_CLASS trustedInfoClass = _TRUSTED_INFORMATION_CLASS.TrustedDomainInformationEx;
            trustDomainInfo = TrustedDomainInformation.Invalid;
            invalidParamterCount = 0;
            this.trustedDomainHandle = this.validTrustHandle;

            // Checking if the Model received and invalid handle
            if (trustObjectCreateinformation.intTdoHandleNumber != handleInput)
            {
                this.trustedDomainHandle = PolicyHandle;
                this.isInvalidHandle = true;
                invalidParamterCount++;
            }

            // Initializing the Information Class that we are requesting 
            // to query and check required access.
            // Checking if it is a valid information class to be queried
            utilities.InitializeInformationClass(
                trustedInformation,
                trustedDomainInfo,
                QUERY_OPERATION,
                ref domainInformation);
            queryInformation = domainInformation;

            if (trustedInformation == TrustedInformationClass.Invalid)
            {
                trustedInfoClass = _TRUSTED_INFORMATION_CLASS.TrustedControllersInformation;
            }
            else
            {
                trustedInfoClass = (_TRUSTED_INFORMATION_CLASS)trustedInformation;
            }

            //// This is a Windows Check.
            //// This information class was introduced only in Windows Vista. So it won't work on windows 2k3
            if (isWindows
                    && this.PDCOSVersion == ServerVersion.Win2003
                    && trustedInformation == TrustedInformationClass.TrustedDomainSupportedEncryptionTypes)
            {
                if (trustObjectCreateinformation.intTdoHandleNumber != handleInput)
                {
                    return ErrorStatus.InvalidHandle;
                }
                else
                {
                    trustDomainInfo = TrustedDomainInformation.Valid;
                    return ErrorStatus.Success;
                }
            }

            if (intCheckInfoClass == INVALIDPARAM)
            {
                invalidParamterCount++;
            }

            uintMethodStatus = lsadClientStack.LsarQueryInfoTrustedDomain(
                this.trustedDomainHandle.Value,
                trustedInfoClass,
                out queryInformation);
            if (queryInformation == null)
            {
                domainInformation = new _LSAPR_TRUSTED_DOMAIN_INFO();
            }
            else
            {
                domainInformation = queryInformation.Value;
            }

            //// The TD doesn't describe the protocol behavior when the opnum receives 2 or more invalid inputs
            //// To handle the condition, we return ErrorStatus.ErrorUnknown when there are 2 or more Invalid Inputs.
            if (invalidParamterCount > 1)
            {
                trustDomainInfo = TrustedDomainInformation.Invalid;
                return ErrorStatus.ErrorUnKnown;
            }

            //// Checking if the handle provided for the opnum is valid.
            //// If an Invalid handle is sent, the server responds with INVALID_HANDLE
            if (trustObjectCreateinformation.intTdoHandleNumber == handleInput)
            {
                if (isAccessDenied)
                {
                    #region MS-LSAD_R739

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.AccessDenied,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        739,
                        @"In LsarQueryInfoTrustedDomain, The server MUST return STATUS_ACCESS_DENIED 
                        when the caller does not have the permissions to perform this operation.");

                    #endregion MS-LSAD_R739
                }
                else
                {
                    #region MS-LSAD_R744

                    Site.CaptureRequirementIfIsFalse(
                        isAccessDenied,
                        "MS-LSAD",
                        744,
                        @"In LsarQueryInfoTrustedDomain in the processing of TrustedDomainHandle, 
                        The handle MUST have been opened with a set of access rights that depends on the 
                        InformationClass parameter provided by the caller.");

                    #endregion MS-LSAD_R744
                }

                //// Case SUCCESS is for Information Classes that are valid.
                //// Case INVALIDPARAM is for Information Classes that return INVALID_PARAMETER
                //// Case INVALIDINFOCLASS is for Information Classes that return INVALID_INFO_CLASS
                ////The switch is decided based on the value set in InitializeInformationClass Method.
                switch (intCheckInfoClass)
                {
                    case SUCCESS:
                        if ((trustedInfoClass == _TRUSTED_INFORMATION_CLASS.TrustedDomainNameInformation)
                                || (trustedInfoClass == 
                                    _TRUSTED_INFORMATION_CLASS.TrustedDomainFullInformation2Internal))
                        {
                            //// Checking if the Data that we set is equal to the Data that we are querying
                            if (utilities.CheckForValidityOfData(domainInformation, trustedInfoClass))
                            {
                                #region MS-LSAD_R738

                                Site.CaptureRequirementIfAreEqual<uint>(
                                    (uint)ErrorStatus.Success,
                                    (uint)uintMethodStatus,
                                    "MS-LSAD",
                                    738,
                                    @"In LsarQueryInfoTrustedDomain, The server MUST return STATUS_SUCCESS 
                                    when the request was successfully completed.");

                                #endregion

                                #region MS-LSAD_R749

                                Site.CaptureRequirementIfAreEqual<uint>(
                                    (uint)ErrorStatus.Success,
                                    (uint)uintMethodStatus,
                                    "MS-LSAD",
                                    749,
                                    @"In LsarQueryInfoTrustedDomain, Buffer is used to return
                                    the data requested by caller.");

                                #endregion

                                trustDomainInfo = TrustedDomainInformation.Valid;
                            }
                        }

                        ////When the server is not at DS_BEHAVIOR_WIN2003 forest functional level, 
                        ////the server should capture these RS
                        if (ForestFunctionalLevel.DS_BEHAVIOR_WIN2003 != forestFunctionLevel)
                        {
                            string IsR1056Implemented = R1056Implementation;
                            bool TRUST_ATTRIBUTE_FOREST_TRANSITIVEIsHide = false;
                            TRUST_ATTRIBUTE_FOREST_TRANSITIVEIsHide =
                                ((trustedInfoClass == _TRUSTED_INFORMATION_CLASS.TrustedDomainInformationEx
                                              && LsadUtilities.TRUST_ATTRIBUTE_FOREST_TRANSITIVE !=
                                                     domainInformation.TrustedDomainInfoEx.TrustAttributes)
                                      || (trustedInfoClass == _TRUSTED_INFORMATION_CLASS.TrustedDomainFullInformation
                                              && LsadUtilities.TRUST_ATTRIBUTE_FOREST_TRANSITIVE !=
                                                     domainInformation.TrustedFullInfo.Information.TrustAttributes)
                                      || (trustedInfoClass == 
                                            _TRUSTED_INFORMATION_CLASS.TrustedDomainFullInformation2Internal
                                              && LsadUtilities.TRUST_ATTRIBUTE_FOREST_TRANSITIVE !=
                                                     domainInformation.TrustedFullInfo2.Information.TrustAttributes));

                            if (isWindows && this.PDCOSVersion >= ServerVersion.Win2003)
                            {
                                #region MS-LSAD_R1057

                                Site.CaptureRequirementIfIsTrue(
                                    TRUST_ATTRIBUTE_FOREST_TRANSITIVEIsHide,
                                    "MS-LSAD",
                                    1057,
                                    @"<88> Section 3.1.4.7.13: When not at DS_BEHAVIOR_WIN2003 forest functional level,
                                    Windows Server 2003, Windows Server 2008, and Windows Server 2008 R2 hide the 
                                    presence of the TRUST_ATTRIBUTE_FOREST_TRANSITIVE bit in the Trust Attributes 
                                    field of a trusted domain object.");

                                #endregion

                                if (IsR1056Implemented == null)
                                {
                                    Site.Properties.Add("IsR1056Implemented", bool.TrueString);
                                    IsR1056Implemented = bool.TrueString;
                                }
                            }

                            if (IsR1056Implemented != null)
                            {
                                bool implSigns = bool.Parse(IsR1056Implemented);
                                bool isSatisfied = (true == TRUST_ATTRIBUTE_FOREST_TRANSITIVEIsHide);

                                #region MS-LSAD_R1056

                                Site.CaptureRequirementIfAreEqual<bool>(
                                    implSigns,
                                    isSatisfied,
                                    "MS-LSAD",
                                    1056,
                                    @"If the server is not at DS_BEHAVIOR_WIN2003 forest functional level, 
                                    the presence of the TRUST_ATTRIBUTE_FOREST_TRANSITIVE bit in the Trust Attributes
                                    field of a trusted domain object MUST NOT be returned by the server.<88>");

                                #endregion
                            }
                        }

                        trustDomainInfo = TrustedDomainInformation.Valid;

                        break;

                    case INVALIDPARAM:

                        #region MS-LSAD_R740

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.InvalidParameter,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            740,
                            @"In LsarQueryInfoTrustedDomain, The server MUST return STATUS_INVALID_PARAMETER when 
                            one of the arguments supplied to the function was invalid.");

                        #endregion

                        if (trustedInformation == TrustedInformationClass.Invalid)
                        {
                            #region MS-LSAD_R746

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.InvalidParameter,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                746,
                                @"In LsarQueryInfoTrustedDomain in the processing of InformationClass, For values 
                                outside the TRUSTED_INFORMATION_CLASS range, the server MUST reject the request
                                with STATUS_INVALID_PARAMETER.");

                            #endregion
                        }

                        break;
                    case INVALIDINFOCLASS:

                        #region MS-LSAD_R741

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.InvalidInfoClass,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            741,
                            @"In LsarQueryInfoTrustedDomain, The server MUST return STATUS_INVALID_INFO_CLASS when 
                            informationClass argument is outside the allowed range.");

                        #endregion

                        break;
                }

                if (trustedInformation == TrustedInformationClass.TrustedDomainAuthInformationInternal
                        || trustedInformation == TrustedInformationClass.TrustedDomainFullInformationInternal)
                {
                    #region MS-LSAD_R742

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.InvalidInfoClass,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        742,
                        @"In LsarQueryInfoTrustedDomain, Information class values TrustedDomainAuthInformationInternal
                        and TrustedDomainFullInformationInternal MUST be rejected with STATUS_INVALID_INFO_CLASS.");

                    #endregion
                }

                if (trustedInformation == TrustedInformationClass.TrustedDomainAuthInformation)
                {
                    #region MS-LSAD_R750

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.InvalidInfoClass,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        750,
                        @"In LsarQueryInfoTrustedDomain, in the information class, when the parameter
                        TrustedDomainAuthInformation is queried the server MUST return STATUS_INVALID_INFO_CLASS.");

                    #endregion
                }

                if (trustedInformation == TrustedInformationClass.TrustedDomainFullInformation)
                {
                    if ((uint)uintMethodStatus == (uint)ErrorStatus.Success)
                    {
                        #region MS-LSAD_R745

                        Site.CaptureRequirementIfIsTrue(
                            ((domainInformation.TrustedFullInfo.AuthInformation.IncomingAuthInfos == 0)
                                 && (domainInformation.TrustedFullInfo.AuthInformation.OutgoingAuthInfos == 0)),
                            "MS-LSAD",
                            745,
                            @"A LsarQueryInfoTrustedDomain request with the  InformationClass set to
                            TrustedDomainFullInformation, if successful,then the trust Incoming and Outgoing 
                             Password values MUST be set to 0 by the server in the returned Information.");

                        #endregion
                    }
                }

                if (trustedInformation == TrustedInformationClass.TrustedDomainFullInformation2Internal)
                {
                    if ((uint)uintMethodStatus == (uint)ErrorStatus.Success)
                    {
                        #region MS-LSAD_R747

                        Site.CaptureRequirementIfIsTrue(
                            ((domainInformation.TrustedFullInfo2.AuthInformation.IncomingAuthInfos == 0)
                                 && (domainInformation.TrustedFullInfo2.AuthInformation.OutgoingAuthInfos == 0)),
                            "MS-LSAD",
                            747,
                            @"In LsarQueryInfoTrustedDomain if  InformationClass parameter is set to 
                            TrustedDomainFullInformation2Internal,if the call is successful, trust Incoming and 
                            Outgoing Password values MUST be set to 0 by the server.");

                        #endregion
                    }
                }
            }
            else
            {
                #region MS-LSAD_R743

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidHandle,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    743,
                    @"In LsarQueryInfoTrustedDomain, If the handle is  not a valid context handle to a trusted domain  
                    object, the server MUST return STATUS_INVALID_HANDLE.");

                #endregion
            }

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion

        #region LsarSetInformationTrustedDomain

        /// <summary>
        ///  The SetInformationTrustedDomain method is invoked
        ///  to set information on a trusted domain object. Identifies
        ///  the Trusted Domain Object by an open Trusted Domain Handle.
        /// </summary>
        /// <param name="handleInput">Contains trust handle obtained from 
        /// CreateTrustedDomainEx2/CreateTrustedDomainEx/CreateTrustedDomain </param>
        /// <param name="trustedDomainInfo">Contains the information of trusted domain object</param>
        /// <param name="forestFuncLevel">Contains forest functional levels</param>
        /// <param name="trustedInformation">Contains the type of trusted domain object information</param>
        /// <param name="isRootDomain">It is to check whether the current domain is a root domain</param>
        /// <returns>Returns Success if the method is successful
        ///          Returns InvalidParameter if the parameters passed to the method are not valid
        ///          Returns AccessDenied if the caller does not have the permissions to 
        ///          perform this operation
        ///          Returns InvalidHandle if the passed in policy handle is not valid
        ///          Returns InvalidDomainState if the domain is in the wrong state
        ///          to do the stated operation</returns>
        /// Disable warning CA1502 because it will affect the implementation of Adapter and Model codes
        /// if do any changes about maintainability.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", 
            Justification = "affect the implementation of Adapter and Model codes")]
        public ErrorStatus SetInformationTrustedDomain(
            int handleInput,
            TRUSTED_DOMAIN_INFORMATION_EX trustedDomainInfo,
            ForestFunctionalLevel forestFuncLevel,
            TrustedInformationClass trustedInformation,
            bool isRootDomain)
        {
            _LSAPR_TRUSTED_DOMAIN_INFO domainInformation = new _LSAPR_TRUSTED_DOMAIN_INFO();
            _LSAPR_TRUSTED_DOMAIN_INFO? queryInformation = new _LSAPR_TRUSTED_DOMAIN_INFO();
            _TRUSTED_INFORMATION_CLASS trustedInfoClass = _TRUSTED_INFORMATION_CLASS.TrustedDomainInformationEx;
            this.trustedDomainHandle = this.validTrustHandle;
            invalidParamterCount = 0;

            // Checking if the Model received and invalid handle
            if (trustObjectCreateinformation.intTdoHandleNumber != handleInput)
            {
                this.trustedDomainHandle = PolicyHandle;
                invalidParamterCount++;
            }

            // Initializing the Information Class that we are requesting 
            // to query and check required access.
            // Checking if it is a valid information class to be queried
            utilities.InitializeInformationClass(
                trustedInformation,
                trustedDomainInfo,
                SET_OPERATION,
                ref domainInformation);

            if (trustedInformation == TrustedInformationClass.Invalid)
            {
                trustedInfoClass = _TRUSTED_INFORMATION_CLASS.TrustedControllersInformation;
            }
            else
            {
                trustedInfoClass = (_TRUSTED_INFORMATION_CLASS)trustedInformation;
            }

            if (intCheckInfoClass == INVALIDPARAM)
            {
                invalidParamterCount++;
            }

            if (trustedInformation == TrustedInformationClass.TrustedDomainInformationEx
                    || trustedInformation == TrustedInformationClass.TrustedDomainFullInformation
                    || trustedInformation == TrustedInformationClass.TrustedDomainFullInformationInternal)
            {
                //// Checking if the Domain is in the correct state for this operation
                if ((trustedDomainInfo.TrustAttr == LsadUtilities.TRUST_ATTRIBUTE_FOREST_TRANSITIVE
                         && forestFuncLevel == ForestFunctionalLevel.DS_BEHAVIOR_WIN2000)
                     || (trustedDomainInfo.TrustAttr == LsadUtilities.TRUST_ATTRIBUTE_CROSS_ORGANIZATION
                         && forestFuncLevel == ForestFunctionalLevel.DS_BEHAVIOR_WIN2000)
                     || (trustedDomainInfo.TrustAttr == LsadUtilities.TRUST_ATTRIBUTE_FOREST_TRANSITIVE
                         && !isRootDomain)
                     && isWindows)
                {
                    invalidParamterCount++;
                }
            }

            if (this.domainState == ProtocolServerConfig.ReadOnlyDomainController)
            {
                invalidParamterCount++;
            }

            //// TrustedDomainSupportedEncryptionTypes was introduced in windows Vista. So it won't work on 2k3
            if (this.PDCOSVersion == ServerVersion.Win2003
                    && trustedInformation == TrustedInformationClass.TrustedDomainSupportedEncryptionTypes)
            {
                if (invalidParamterCount > 1)
                {
                    return ErrorStatus.ErrorUnKnown;
                }
                else if (trustObjectCreateinformation.intTdoHandleNumber != handleInput)
                {
                    return ErrorStatus.InvalidHandle;
                }
                else
                {
                    return ErrorStatus.Success;
                }
            }

            ////Check if Interdomain Trust Account exist and is valid
            bool isInterdomainTrustAccountExist = false;

            if (invalidParamterCount == 0)
            {
                isInterdomainTrustAccountExist = !this.CheckInterdomainTrustAccount();
            }

            uintMethodStatus = lsadClientStack.LsarSetInformationTrustedDomain(
                this.trustedDomainHandle.Value,
                trustedInfoClass,
                domainInformation);

            ////Check if Interdomain Trust Account exist and is valid
            bool serverCreateInterdomain = invalidParamterCount == 0
                                               && isInterdomainTrustAccountExist
                                               && !this.CheckInterdomainTrustAccount();

            if ((uint)TrustDirection.INBOUND == trustedDomainInfo.TrustDir && serverCreateInterdomain)
            {
                #region MS-LSAD_R1059

                // Add the comment information including the location information.
                Site.Log.Add(
                    LogEntryKind.Comment,
                    "Verify MS-LSAD_R1059:");

                // Verify requirement 1059
                Site.CaptureRequirementIfIsTrue(
                    serverCreateInterdomain,
                    "MS-LSAD",
                    1059,
                    @"If the trust direction is being set to incoming, then the server MUST create an interdomain
                    trust account for this trust, if such an account does not yet exist, and populate it as 
                    specified in [MS-ADTS] section 7.1.6.8.");

                #endregion
            }

            if ((uint)TrustDirection.BIDIRECTIONAL == trustedDomainInfo.TrustDir && serverCreateInterdomain)
            {
                #region MS-LSAD_R1060

                // Verify requirement 1060
                Site.CaptureRequirementIfIsTrue(
                    serverCreateInterdomain,
                    "MS-LSAD",
                    1060,
                    @"If the trust direction is being set to bidirectional, then the server MUST create an interdomain 
                    trust account for this trust, if such an account does not yet exist, and populate it as specified 
                    in [MS-ADTS] section 7.1.6.8.");

                #endregion
            }

            //// The TD doesn't describe the protocol behavior when the opnum receives 2 or more invalid inputs
            //// To handle the condition, we return ErrorStatus.ErrorUnknown when there are 2 or more Invalid Inputs.
            if (invalidParamterCount > 1)
            {
                #region ReadOnly-DC Requirements

                if (this.domainState == ProtocolServerConfig.ReadOnlyDomainController)
                {
                    #region MS-LSAD_R762

                    Site.CaptureRequirementIfAreNotEqual<uint>(
                        (uint)ErrorStatus.Success,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        762,
                        @"If the server is a read-only domain controller, the server MUST return an error on receiving
                        an LsarSetInformationTrustedDomain request.");

                    #endregion MS-LSAD_R762

                    #region MS-LSAD_R763

                    // Windows Specific Requirement.
                    if (isWindows && PDCOSVersion >= ServerVersion.Win2008)
                    {
                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.ObjectNameNotFound,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            763,
                            @"If the server is running windows server 2008 ans is a read-only domain controller, 
                            the server MUST return STATUS_OBJECT_NAME_NOT_FOUND error on receiving an 
                            LsarSetInformationTrustedDomain request.");
                    }

                    #endregion MS-LSAD_R763
                }

                #endregion ReadOnly-DC Requirements

                return ErrorStatus.ErrorUnKnown;
            }

            if (this.domainState == ProtocolServerConfig.ReadOnlyDomainController)
            {
                #region MS-LSAD_R762

                Site.CaptureRequirementIfAreNotEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    762,
                    @"If the server is a read-only domain controller, the server MUST return an error on receiving an
                    LsarSetInformationTrustedDomain request.");

                #endregion MS-LSAD_R762

                #region MS-LSAD_R763

                // Windows Specific Requirement
                if (isWindows && PDCOSVersion >= ServerVersion.Win2008)
                {
                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.ObjectNameNotFound,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        763,
                        @"If the server is running windows server 2008 ans is a read-only domain controller, 
                        the server MUST return STATUS_OBJECT_NAME_NOT_FOUND error on receiving an 
                        LsarSetInformationTrustedDomain request.");
                }

                #endregion MS-LSAD_R763
            }

            if (forestFuncLevel != ForestFunctionalLevel.DS_BEHAVIOR_WIN2003
                    && (trustedInformation == TrustedInformationClass.TrustedDomainInformationEx
                            || trustedInformation == TrustedInformationClass.TrustedDomainFullInformation
                            || trustedInformation == TrustedInformationClass.TrustedDomainFullInformationInternal))
            {
                if (isWindows
                        && (trustedDomainInfo.TrustAttr == LsadUtilities.TRUST_ATTRIBUTE_FOREST_TRANSITIVE
                                && forestFuncLevel == ForestFunctionalLevel.DS_BEHAVIOR_WIN2000))
                {
                    #region MS-LSAD_R1027

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.InvalidDomainState,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        1027,
                        @"In LsarSetInformationTrustedDomain, the server MUST return STATUS_INVALID_DOMAIN_STATE 
                        when the TRUST_ATTRIBUTE_FOREST_TRANSITIVE flag is set prior to reaching DS_BEHAVIOR_WIN2003
                        forest functional level.");

                    #endregion MS-LSAD_R1027

                    #region MS-LSAD_R761

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.InvalidDomainState,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        761,
                        @"In LsarSetInformationTrustedDomain in the proccessing of TrustedDomainInformation,
                        The server MUST return STATUS_INVALID_DOMAIN_STATE if the trusted domain information
                        provided in TrustedDomainInformation parameter cannot be satisfied in the current 
                        domain state.");

                    #endregion MS-LSAD_R761
                }
                else if (isWindows
                             && (trustedDomainInfo.TrustAttr == LsadUtilities.TRUST_ATTRIBUTE_CROSS_ORGANIZATION
                                     && forestFuncLevel == ForestFunctionalLevel.DS_BEHAVIOR_WIN2000))
                {
                    #region MS-LSAD_R1029

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.InvalidDomainState,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        1029,
                        @"In LsarSetInformationTrustedDomain, the server MUST return STATUS_INVALID_DOMAIN_STATE 
                        when the TRUST_ATTRIBUTE_CROSS_ORGANIZATION flag is set for a trust in a domain where the
                        forest is not at DS_BEHAVIOR_WIN2003 forest functional level.");

                    #endregion MS-LSAD_R1029
                }
                else if (isWindows
                             && (trustedDomainInfo.TrustAttr == LsadUtilities.TRUST_ATTRIBUTE_FOREST_TRANSITIVE
                                     && !isRootDomain))
                {
                    #region MS-LSAD_R1028

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.InvalidDomainState,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        1028,
                        @"In LsarSetInformationTrustedDomain, the server MUST return STATUS_INVALID_DOMAIN_STATE
                        when the TRUST_ATTRIBUTE_FOREST_TRANSITIVE flag is set for a trust in a domain that is 
                        not the root domain of the forest.");

                    #endregion MS-LSAD_R1028
                }
            }
            else
            {
                if (trustObjectCreateinformation.intTdoHandleNumber != handleInput)
                {
                    #region MS-LSAD_R751

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.InvalidHandle,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        751,
                        @"In LsarSetInformationTrustedDomain, If the TrustedDomainHandle is not a valid context handle 
                        to a trusted domain object, the server MUST return STATUS_INVALID_HANDLE.");

                    #endregion
                }
                else
                {
                    if (isAccessDenied)
                    {
                        #region MS-LSAD_R757

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.AccessDenied,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            757,
                            @"In LsarSetInformationTrustedDomain in the proccessing of TrustedDomainInformation,
                            The server MUST return STATUS_ACCESS_DENIED when the caller does not have the permissions
                            to perform this operation.");

                        #endregion MS-LSAD_R757
                    }
                    else
                    {
                        #region MS-LSAD_R754

                        Site.CaptureRequirementIfIsFalse(
                            isAccessDenied,
                            "MS-LSAD",
                            754,
                            @"In LsarSetInformationTrustedDomain in the proccessing of InformationClass, 
                            The handle MUST have been opened with a set of access rights that depends on
                            the InformationClass required.");

                        #endregion MS-LSAD_R754
                    }

                    //// If the InformationClass is an invalid one, we set intCheckInfoClass to INVALIDPARAM 
                    //// in IntializeInformationClass method.
                    if (intCheckInfoClass == SUCCESS)
                    {
                        //// Querying the Same Data that we set to see if the set was a successful Operation
                        this.uintStatus = lsadClientStack.LsarQueryInfoTrustedDomain(
                            this.trustedDomainHandle.Value,
                            trustedInfoClass,
                            out queryInformation);

                        if (utilities.CheckIfDataIsSame(
                            trustedInfoClass, 
                            domainInformation,
                            queryInformation.HasValue ? queryInformation.Value : new _LSAPR_TRUSTED_DOMAIN_INFO()))
                        {
                            #region MS-LSAD_R756

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.Success,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                756,
                                @"In LsarSetInformationTrustedDomain in the proccessing of TrustedDomainInformation, 
                            The server MUST return STATUS_SUCCESS when the request was successfully completed.");

                            #endregion

                            #region MS-LSAD_R738

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.Success,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                738,
                                @"In LsarQueryInfoTrustedDomain, The server MUST return STATUS_SUCCESS 
                                    when the request was successfully completed.");

                            #endregion

                            #region MS-LSAD_R749

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.Success,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                749,
                                @"In LsarQueryInfoTrustedDomain, Buffer is used to return
                                    the data requested by caller.");

                            #endregion
                        }

                        if ((trustedInfoClass == _TRUSTED_INFORMATION_CLASS.TrustedDomainInformationEx
                                 || trustedInfoClass == _TRUSTED_INFORMATION_CLASS.TrustedDomainFullInformationInternal)
                             && domainInformation.TrustedDomainInfoEx.TrustAttributes !=
                                    LsadUtilities.TRUST_ATTRIBUTE_CROSS_ORGANIZATION)
                        {
                            this.uintStatus = lsadClientStack.LsarQueryInfoTrustedDomain(
                                this.trustedDomainHandle.Value,
                                _TRUSTED_INFORMATION_CLASS.TrustedDomainFullInformation2Internal,
                                out queryInformation);

                            if (trustedInfoClass == _TRUSTED_INFORMATION_CLASS.TrustedDomainInformationEx)
                            {
                                #region MS-LSAD_R759

                                Site.CaptureRequirementIfIsTrue(
                                    (queryInformation.Value.TrustedFullInfo2.Information.ForestTrustLength == 0
                                        && queryInformation.Value.TrustedFullInfo2.Information.ForestTrustInfo == null),
                                    "MS-LSAD",
                                    759,
                                    @"In LsarSetInformationTrustedDomain, Value of InformationClass parameter 
                                    TrustedDomainInformationEx Forest trust attributes MUST be set to 0 if new trust
                                    attributes do not contain TRUST_ATTRIBUTE_FOREST_TRANSITIVE flag.");

                                #endregion MS-LSAD_R759
                            }

                            if (trustedInfoClass == _TRUSTED_INFORMATION_CLASS.TrustedDomainFullInformationInternal)
                            {
                                #region MS-LSAD_R760

                                Site.CaptureRequirementIfIsTrue(
                                    (queryInformation.Value.TrustedFullInfo2.Information.ForestTrustLength == 0
                                        && queryInformation.Value.TrustedFullInfo2.Information.ForestTrustInfo == null),
                                    "MS-LSAD",
                                    760,
                                    @"In LsarSetInformationTrustedDomain, Value of InformationClass parameter 
                                    TrustedDomainFullInformationInternal Forest trust attributes MUST be set to
                                    0 if new trust attributes do not contain TRUST_ATTRIBUTE_FOREST_TRANSITIVE flag.");

                                #endregion MS-LSAD_R760
                            }
                        }
                    }
                    else if (intCheckInfoClass == INVALIDPARAM)
                    {
                        string IsR753Implemented = R753Implementation;
                        if (isWindows)
                        {
                            #region MS-LSAD_R10753

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.InvalidParameter,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                10753,
                                @"In LsarSetInformationTrustedDomain in the proccessing of InformationClass,
                                Information class values other than the TrustedPosixOffsetInformation,  
                                TrustedDomainInformationEx, TrustedDomainAuthInformation, TrustedDomainFullInformation,
                                TrustedDomainAuthInformationInternal, TrustedDomainFullInformationInternal, 
                                TrustedDomainSupportedEncryptionTypes <89> is rejected with
                                STATUS_INVALID_PARAMETER in Windows.");

                            #endregion

                            if (IsR753Implemented == null)
                            {
                                Site.Properties.Add("IsR753Implemented", bool.TrueString);
                                IsR753Implemented = bool.TrueString;
                            }
                        }

                        if (IsR753Implemented != null)
                        {
                            bool implSigns = bool.Parse(IsR753Implemented);
                            bool isSatisfied = ((uint)ErrorStatus.InvalidParameter == (uint)uintMethodStatus);

                            Site.CaptureRequirementIfAreEqual<bool>(
                                implSigns,
                                isSatisfied,
                                "MS-LSAD",
                                753,
                                @"In LsarSetInformationTrustedDomain in the proccessing of InformationClass, 
                                Information class values other than the TrustedPosixOffsetInformation,
                                TrustedDomainInformationEx, TrustedDomainAuthInformation, TrustedDomainFullInformation,
                                TrustedDomainAuthInformationInternal, TrustedDomainFullInformationInternal, 
                                TrustedDomainSupportedEncryptionTypes SHOULD<89> be rejected with 
                                STATUS_INVALID_PARAMETER.");
                        }

                        #region MS-LSAD_R752

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.InvalidParameter,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            752,
                            @"In LsarSetInformationTrustedDomain in the proccessing of InformationClass,
                            For values outside the TRUSTED_INFORMATION_CLASS range, the server MUST reject the 
                            request with STATUS_INVALID_PARAMETER.");

                        #endregion
                    }
                    else
                    {
                        if (!(trustedInformation == TrustedInformationClass.TrustedDomainInformationBasic))
                        {
                            return ErrorStatus.InvalidInfoClass;
                        }
                    }
                }
            }

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion

        #region LsarQueryTrustedDomainInfoByName

        /// <summary>
        ///  The QueryTrustedDomainInfoByName method is invoked
        ///  to retrieve information about a trusted domain object.
        ///  Object is identified by its Domain Name.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="trustedDomainName">Contains the trusted domain object name</param>
        /// <param name="name">It is for validating the trusted domain name whether it is valid or invalid</param>
        /// <param name="trustedInformation">Contains the type of trusted domain object information</param>
        /// <param name="trustDomainInfo">Out param which contains valid or invalid
        /// trusted domain information</param>
        /// <returns>Returns Success if the method is successful
        ///          Returns InvalidParameter if the parameters passed to the method are not valid
        ///          Returns AccessDenied if the caller does not have the permissions to 
        ///          perform this operation
        ///          Returns InvalidHandle if the passed in policy handle is not valid
        ///          Returns ObjectNameNotFound if the trusted domain
        ///          with specified name could not be found</returns>
        /// Disable warning CA1502 because it will affect the implementation of Adapter and Model codes 
        /// if do any changes about maintainability.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", 
            Justification = "affect the implementation of Adapter and Model codes")]
        public ErrorStatus QueryTrustedDomainInfoByName(
            int handleInput,
            string trustedDomainName,
            ValidString name,
            TrustedInformationClass trustedInformation,
            out TrustedDomainInformation trustDomainInfo)
        {
            _LSAPR_TRUSTED_DOMAIN_INFO domainInformation = new _LSAPR_TRUSTED_DOMAIN_INFO();
            _LSAPR_TRUSTED_DOMAIN_INFO? domainInformationForQuery = new _LSAPR_TRUSTED_DOMAIN_INFO();
            _RPC_UNICODE_STRING[] domainName = new _RPC_UNICODE_STRING[1];
            TRUSTED_DOMAIN_INFORMATION_EX trustedDomainInfo = new TRUSTED_DOMAIN_INFORMATION_EX();
            _TRUSTED_INFORMATION_CLASS trustedInfoClass = _TRUSTED_INFORMATION_CLASS.TrustedDomainInformationEx;
            DomainType checkDomainName = DomainType.ValidDomainName;
            invalidParamterCount = 0;
            trustDomainInfo = TrustedDomainInformation.Invalid;
            PolicyHandle = validPolicyHandle;

            // Checking if the Model received and invalid handle
            if (stPolicyInformation.PHandle != handleInput)
            {
                PolicyHandle = utilities.CreateAnInvalidHandle(false);
                invalidParamterCount++;
            }

            // Checking if the Name of the TDO received by the model is present
            if (trustObjectCreateinformation.strTdoDnsName.Equals(trustedDomainName))
            {
                checkDomainName = DomainType.ValidDomainName;
                this.isDomainPresent = true;
            }
            else
            {
                checkDomainName = DomainType.NoDomainName;
                this.isDomainPresent = false;
                invalidParamterCount++;
            }

            // Initialize the TDO name
            utilities.GetTheDomainName(checkDomainName, ref domainName);

            if (name == ValidString.Valid)
            {
                domainName[0].Length = (ushort)(domainName[0].Buffer.Length * 2);
            }
            else
            {
                domainName[0].Length = (ushort)((domainName[0].Buffer.Length * 2) + 1);
                invalidParamterCount++;
            }

            domainName[0].MaximumLength = (ushort)(domainName[0].Length + 2);

            // Initializing the Information Class that we are requesting 
            // to query and check required access.
            // Checking if it is a valid information class to be queried.
            utilities.InitializeInformationClass(
                trustedInformation,
                trustedDomainInfo,
                QUERY_OPERATION,
                ref domainInformation);

            if (trustedInformation == TrustedInformationClass.Invalid)
            {
                trustedInfoClass = _TRUSTED_INFORMATION_CLASS.TrustedControllersInformation;
            }
            else
            {
                trustedInfoClass = (_TRUSTED_INFORMATION_CLASS)trustedInformation;
            }

            if (intCheckInfoClass == INVALIDPARAM)
            {
                if (name != ValidString.Invalid)
                {
                    invalidParamterCount++;
                }
            }

            //// TrustedDomainSupportedEncryptionTypes was introduced in Windows Vista, so it won't work on 2k3
            if (isWindows
                    && this.PDCOSVersion == ServerVersion.Win2003
                    && trustedInformation == TrustedInformationClass.TrustedDomainSupportedEncryptionTypes)
            {
                if (invalidParamterCount > 1)
                {
                    return ErrorStatus.ErrorUnKnown;
                }
                else if (stPolicyInformation.PHandle != handleInput)
                {
                    return ErrorStatus.InvalidHandle;
                }
                else if (name == ValidString.Invalid)
                {
                    return ErrorStatus.InvalidParameter;
                }
                else if (!this.isDomainPresent)
                {
                    return ErrorStatus.ObjectNameNotFound;
                }
                else
                {
                    trustDomainInfo = TrustedDomainInformation.Valid;
                    return ErrorStatus.Success;
                }
            }

            domainInformationForQuery = domainInformation;
            uintMethodStatus = lsadClientStack.LsarQueryTrustedDomainInfoByName(
                PolicyHandle.Value,
                domainName[0],
                trustedInfoClass,
                out domainInformationForQuery);

            if (domainInformationForQuery == null)
            {
                domainInformation = new _LSAPR_TRUSTED_DOMAIN_INFO();
            }
            else
            {
                domainInformation = domainInformationForQuery.Value;
            }

            // The TD doesn't describe the protocol behavior when the opnum receives 2 or more invalid inputs
            // To handle the condition, we return ErrorStatus.ErrorUnknown when there are 2 or more Invalid Inputs.
            if (invalidParamterCount > 1)
            {
                trustDomainInfo = TrustedDomainInformation.Invalid;
                return ErrorStatus.ErrorUnKnown;
            }

            // Checking for the Validity of the data.
            if (name == ValidString.Invalid)
            {
                #region MS-LSAD_R640

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidParameter,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    640,
                    @"In LsarQueryTrustedDomainInfoByName, The server MUST return 
                                    STATUS_INVALID_PARAMETER when One of the supplied parameters was invalid.");

                #endregion
            }
            else
            {
                // Checking if the handle provided is valid or not.returns INVALID_HANDLE if not a valid one.
                if (stPolicyInformation.PHandle == handleInput)
                {
                    if (!this.isDomainPresent)
                    {
                        #region MS-LSAD_R641

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.ObjectNameNotFound,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            641,
                            @"In LsarQueryTrustedDomainInfoByName, The server MUST return STATUS_OBJECT_NAME_NOT_FOUND 
                            when the trusted domain object by the specified name could not be found.");

                        #endregion
                    }
                    else
                    {
                        switch (intCheckInfoClass)
                        {
                            case SUCCESS:
                                if ((trustedInfoClass == _TRUSTED_INFORMATION_CLASS.TrustedDomainNameInformation)
                                        || (trustedInfoClass ==
                                                _TRUSTED_INFORMATION_CLASS.TrustedDomainFullInformation2Internal))
                                {
                                    if (utilities.CheckForValidityOfData(domainInformation, trustedInfoClass))
                                    {
                                        #region MS-LSAD_R638

                                        Site.CaptureRequirementIfAreEqual<uint>(
                                            (uint)ErrorStatus.Success,
                                            (uint)uintMethodStatus,
                                            "MS-LSAD",
                                            638,
                                            @"In LsarQueryTrustedDomainInfoByName, The server MUST return 
                                                STATUS_SUCCESS when the request was successfully completed.");

                                        #endregion

                                        trustDomainInfo = TrustedDomainInformation.Valid;
                                        return ErrorStatus.Success;
                                    }
                                }

                                break;
                            case INVALIDPARAM:

                                #region MS-LSAD_R640

                                Site.CaptureRequirementIfAreEqual<uint>(
                                    (uint)ErrorStatus.InvalidParameter,
                                    (uint)uintMethodStatus,
                                    "MS-LSAD",
                                    640,
                                    @"In LsarQueryTrustedDomainInfoByName, The server MUST return 
                                    STATUS_INVALID_PARAMETER when One of the supplied parameters was invalid.");

                                #endregion

                                break;
                            case INVALIDINFOCLASS:

                                #region MS-LSAD_R1002

                                Site.CaptureRequirementIfAreEqual<uint>(
                                    (uint)ErrorStatus.InvalidInfoClass,
                                    (uint)uintMethodStatus,
                                    "MS-LSAD",
                                    1002,
                                    @"In LsarQueryInfoTrustedDomain, The server MUST return STATUS_INVALID_INFO_CLASS 
                                    when informationClass argument is outside the allowed range.");

                                #endregion

                                break;
                        }
                    }
                }
                else
                {
                    #region MS-LSAD_R642

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.InvalidHandle,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        642,
                        @"In LsarQueryTrustedDomainInfoByName, The server MUST return STATUS_INVALID_HANDLE 
                        when the PolicyHandle is not a valid handle.");

                    #endregion
                }
            }

            if ((uint)uintMethodStatus == (uint)ErrorStatus.Success)
            {
                trustDomainInfo = TrustedDomainInformation.Valid;
            }
            else
            {
                trustDomainInfo = TrustedDomainInformation.Invalid;
            }

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion

        #region LsarSetTrustedDomainInfoByName

        /// <summary>
        ///  The SetTrustedDomainInfoByName method is invoked to set information 
        ///  about a trusted domain object.
        ///  The Object is identified by its Domain name.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="trustedDomainInfo">Contains the information of trusted domain object</param>
        /// <param name="name">It is for validating the trusted domain name whether it is valid or invalid</param>
        /// <param name="forestFuncLevel">Contains forest functional levels</param>
        /// <param name="trustedInformation">Contains the type of trusted domain object information</param>
        /// <param name="isRootDomain">It is to check whether the current domain is a root domain</param>
        /// <returns>Returns Success if the method is successful
        ///          Returns InvalidParameter if the parameters passed to the method are not valid
        ///          Returns AccessDenied if the caller does not have the permissions to 
        ///          perform this operation
        ///          Returns InvalidHandle if the passed in policy handle is not valid
        ///          Returns ObjectNameNotFound if the trusted domain
        ///          with specified name could not be found</returns>
        /// Disable warning CA1502 because it will affect the implementation of Adapter and Model codes 
        /// if do any changes about maintainability.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", 
            Justification = "affect the implementation of Adapter and Model codes")]
        public ErrorStatus SetTrustedDomainInfoByName(
            int handleInput,
            TRUSTED_DOMAIN_INFORMATION_EX trustedDomainInfo,
            ValidString name,
            ForestFunctionalLevel forestFuncLevel,
            TrustedInformationClass trustedInformation,
            bool isRootDomain)
        {
            _LSAPR_TRUSTED_DOMAIN_INFO domainInformation = new _LSAPR_TRUSTED_DOMAIN_INFO();
            _LSAPR_TRUSTED_DOMAIN_INFO? queryInformation = new _LSAPR_TRUSTED_DOMAIN_INFO();
            _RPC_UNICODE_STRING[] domainName = new _RPC_UNICODE_STRING[1];
            _TRUSTED_INFORMATION_CLASS trustedInfoClass = _TRUSTED_INFORMATION_CLASS.TrustedDomainInformationEx;
            _RPC_UNICODE_STRING[] domainDnsName = new _RPC_UNICODE_STRING[1];

            DomainType checkDomainName = DomainType.ValidDomainName;
            invalidParamterCount = 0;
            PolicyHandle = validPolicyHandle;

            //// Checking if the Model received and invalid handle
            if (stPolicyInformation.PHandle != handleInput)
            {
                PolicyHandle = utilities.CreateAnInvalidHandle(false);
                invalidParamterCount++;
            }

            //// Checking if the Name of the TDO received by the model is present
            if (trustObjectCreateinformation.strTdoDnsName.Equals(trustedDomainInfo.TrustDomainName))
            {
                checkDomainName = DomainType.ValidDomainName;
                this.isDomainPresent = true;
            }
            else
            {
                checkDomainName = DomainType.NoDomainName;
                this.isDomainPresent = false;
                invalidParamterCount++;
            }

            //// Initialize the TDO name
            utilities.GetTheDomainName(checkDomainName, ref domainName);

            if (name == ValidString.Valid)
            {
                domainName[0].Length = (ushort)(2 * domainName[0].Buffer.Length);
            }
            else
            {
                domainName[0].Length = (ushort)((2 * domainName[0].Buffer.Length) + 1);
            }

            domainName[0].MaximumLength = (ushort)(2 + domainName[0].Length);

            domainDnsName[0] = domainName[0];

            // Initializing the Information Class that we are requesting 
            // to query and check required access.
            // Checking if it is a valid information class to be queried
            utilities.InitializeInformationClass(
                trustedInformation,
                trustedDomainInfo,
                SET_OPERATION,
                ref domainInformation);

            trustedInfoClass = (_TRUSTED_INFORMATION_CLASS)trustedInformation;

            if (trustedInformation == TrustedInformationClass.Invalid)
            {
                trustedInfoClass = _TRUSTED_INFORMATION_CLASS.TrustedControllersInformation;
            }

            if (intCheckInfoClass == INVALIDPARAM || name == ValidString.Invalid)
            {
                invalidParamterCount++;
            }

            //// TrustedDomainSupportedEncryptionTypes was introduced in windows Vista. So it won't work on 2k3
            if (isWindows
                    && this.PDCOSVersion == ServerVersion.Win2003
                    && trustedInformation == TrustedInformationClass.TrustedDomainSupportedEncryptionTypes)
            {
                if (invalidParamterCount > 1)
                {
                    return ErrorStatus.ErrorUnKnown;
                }

                if (stPolicyInformation.PHandle != handleInput)
                {
                    return ErrorStatus.InvalidHandle;
                }

                if (name == ValidString.Invalid)
                {
                    return ErrorStatus.InvalidParameter;
                }

                if (!this.isDomainPresent)
                {
                    return ErrorStatus.ObjectNameNotFound;
                }

                return ErrorStatus.Success;
            }

            //// Checking if the Domain is in the correct state to perform the operation
            if (trustedInformation == TrustedInformationClass.TrustedDomainInformationEx
                    || trustedInformation == TrustedInformationClass.TrustedDomainFullInformation
                    || trustedInformation == TrustedInformationClass.TrustedDomainFullInformationInternal
                    && isWindows)
            {
                if ((trustedDomainInfo.TrustAttr == LsadUtilities.TRUST_ATTRIBUTE_FOREST_TRANSITIVE
                         && forestFuncLevel == ForestFunctionalLevel.DS_BEHAVIOR_WIN2000)
                     || (trustedDomainInfo.TrustAttr == LsadUtilities.TRUST_ATTRIBUTE_CROSS_ORGANIZATION
                         && forestFuncLevel == ForestFunctionalLevel.DS_BEHAVIOR_WIN2000)
                     || (trustedDomainInfo.TrustAttr == LsadUtilities.TRUST_ATTRIBUTE_FOREST_TRANSITIVE
                         && !isRootDomain))
                {
                    invalidParamterCount++;
                }
            }

            if (this.domainState == ProtocolServerConfig.ReadOnlyDomainController && this.isDomainPresent)
            {
                invalidParamterCount++;
            }

            uintMethodStatus = lsadClientStack.LsarSetTrustedDomainInfoByName(
                PolicyHandle.Value,
                domainDnsName[0],
                trustedInfoClass,
                domainInformation);

            //// The TD doesn't describe the protocol behavior when the opnum receives 2 or more invalid inputs
            //// To handle the condition, we return ErrorStatus.ErrorUnknown when there are 2 or more Invalid Inputs.
            if (invalidParamterCount > 1)
            {
                if (this.domainState == ProtocolServerConfig.ReadOnlyDomainController)
                {
                    #region ReadOnly-DC Requirements

                    #region MS-LSAD_R1016

                    Site.CaptureRequirementIfIsTrue(
                        ((uint)uintMethodStatus != (uint)ErrorStatus.Success),
                        "MS-LSAD",
                        1016,
                        @"In LsarSetTrustedDomainInfoByName, If the server is a read-only domain controller, 
                        it MUST return an error.");

                    #endregion

                    #region MS-LSAD_R1017

                    // Windows 2k8 specific requirement
                    if (isWindows && this.PDCOSVersion == ServerVersion.Win2008)
                    {
                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.ObjectNameNotFound,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            1017,
                            @"In LsarSetTrustedDomainInfoByName, If the server is running Windows 2008 and 
                            is a read-only domain controller, it MUST return STATUS_OBJECT_NAME_NOT_FOUND.");
                    }

                    #endregion

                    #endregion ReadOnly-DC Requirements
                }

                return ErrorStatus.ErrorUnKnown;
            }

            //// Data Validation check
            if (name == ValidString.Valid)
            {
                if (this.domainState == ProtocolServerConfig.ReadOnlyDomainController)
                {
                    #region ReadOnly-DC Requirements

                    #region MS-LSAD_R1016

                    Site.CaptureRequirementIfIsTrue(
                        ((uint)uintMethodStatus != (uint)ErrorStatus.Success),
                        "MS-LSAD",
                        1016,
                        @"In LsarSetTrustedDomainInfoByName, If the server is a read-only domain controller,
                        it MUST return an error.");

                    #endregion

                    #region MS-LSAD_R1017

                    // Windows 2k8 specific requirement.
                    if (isWindows && this.PDCOSVersion == ServerVersion.Win2008)
                    {
                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.ObjectNameNotFound,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            1017,
                            @"In LsarSetTrustedDomainInfoByName, If the server is running Windows 2008 and
                            is a read-only domain controller, it MUST return STATUS_OBJECT_NAME_NOT_FOUND.");
                    }

                    #endregion

                    #endregion ReadOnly-DC Requirements
                }
                else if (trustedInfoClass == _TRUSTED_INFORMATION_CLASS.TrustedPasswordInformation
                             || trustedInfoClass == _TRUSTED_INFORMATION_CLASS.TrustedDomainInformationBasic)
                {
                    if (stPolicyInformation.PHandle != handleInput)
                    {
                        #region MS-LSAD_R648

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.InvalidHandle,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            648,
                            @"In LsarSetTrustedDomainInfoByName, The server MUST return STATUS_INVALID_HANDLE 
                            when the PolicyHandle is not a valid handle.");

                        #endregion
                    }
                    else
                    {
                        if (!this.isDomainPresent)
                        {
                            #region MS-LSAD_R647

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.ObjectNameNotFound,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                647,
                                @"In LsarSetTrustedDomainInfoByName, The server MUST return STATUS_OBJECT_NAME_NOT_FOUND
                                when the trusted domain object by the specified name could not be found.");

                            #endregion
                        }
                        else
                        {
                            if (intCheckInfoClass == INVALIDPARAM)
                            {
                                #region MS-LSAD_R646

                                Site.CaptureRequirementIfAreEqual<uint>(
                                    (uint)ErrorStatus.InvalidParameter,
                                    (uint)uintMethodStatus,
                                    "MS-LSAD",
                                    646,
                                    @"In LsarSetTrustedDomainInfoByName, The server MUST return STATUS_INVALID_PARAMETER
                                    when one of the supplied arguments is invalid.");

                                #endregion
                            }
                            else
                            {
                                if (intCheckInfoClass == SUCCESS)
                                {
                                    this.uintStatus = lsadClientStack.LsarQueryTrustedDomainInfoByName(
                                        PolicyHandle.Value,
                                        domainName[0],
                                        trustedInfoClass,
                                        out queryInformation);

                                    if (utilities.CheckIfDataIsSame(
                                        trustedInfoClass, 
                                        domainInformation, 
                                        queryInformation.Value))
                                    {
                                        #region MS-LSAD_R644

                                        Site.CaptureRequirementIfAreEqual<uint>(
                                            (uint)ErrorStatus.Success,
                                            (uint)uintMethodStatus,
                                            "MS-LSAD",
                                            644,
                                            @"In LsarSetTrustedDomainInfoByName, The server MUST return STATUS_SUCCESS 
                                            when the request was successfully completed.");

                                        #endregion

                                        #region MS-LSAD_R638

                                        Site.CaptureRequirementIfAreEqual<uint>(
                                            (uint)ErrorStatus.Success,
                                            (uint)uintMethodStatus,
                                            "MS-LSAD",
                                            638,
                                            @"In LsarQueryTrustedDomainInfoByName, The server MUST return 
                                                STATUS_SUCCESS when the request was successfully completed.");

                                        #endregion
                                    }
                                }
                                else
                                {
                                    if (intCheckInfoClass == INVALIDINFOCLASS
                                            && trustedInfoClass == 
                                                    _TRUSTED_INFORMATION_CLASS.TrustedDomainInformationBasic)
                                    {
                                        return ErrorStatus.InvalidParameter;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    //// If the InformationClass being set is not a valid one, server returns INVALID_PARAMETER
                    if (intCheckInfoClass == INVALIDPARAM)
                    {
                        #region MS-LSAD_R646

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.InvalidParameter,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            646,
                            @"In LsarSetTrustedDomainInfoByName, The server MUST return STATUS_INVALID_PARAMETER
                                    when one of the supplied arguments is invalid.");

                        #endregion
                    }
                    else
                    {
                        //// checking if handle is invalid.Server returns INVALID_HANDLE if it is an invalid handle.
                        if (stPolicyInformation.PHandle != handleInput)
                        {
                            #region MS-LSAD_R648

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.InvalidHandle,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                648,
                                @"In LsarSetTrustedDomainInfoByName, The server MUST return STATUS_INVALID_HANDLE
                                when the PolicyHandle is not a valid handle.");

                            #endregion
                        }
                        else
                        {
                            // Checking if the domain is present or not
                            if (!this.isDomainPresent)
                            {
                                #region MS-LSAD_R647

                                Site.CaptureRequirementIfAreEqual<uint>(
                                    (uint)ErrorStatus.ObjectNameNotFound,
                                    (uint)uintMethodStatus,
                                    "MS-LSAD",
                                    647,
                                    @"In LsarSetTrustedDomainInfoByName, The server MUST return 
                                    STATUS_OBJECT_NAME_NOT_FOUND when the trusted domain object by
                                    the specified name could not be found.");

                                #endregion
                            }
                            else
                            {
                                if (intCheckInfoClass == SUCCESS)
                                {
                                    //// Checking if the Domain is in right state to perform this operation.
                                    if (isWindows
                                            && (trustedInformation == TrustedInformationClass.TrustedDomainInformationEx
                                                    || trustedInformation ==
                                                          TrustedInformationClass.TrustedDomainFullInformation
                                                    || trustedInformation ==
                                                          TrustedInformationClass.TrustedDomainFullInformationInternal))
                                    {
                                        if (trustedDomainInfo.TrustAttr == 
                                                LsadUtilities.TRUST_ATTRIBUTE_FOREST_TRANSITIVE
                                                && forestFuncLevel == ForestFunctionalLevel.DS_BEHAVIOR_WIN2000)
                                        {
                                            #region MS-LSAD_R1034
                                            Site.CaptureRequirementIfAreEqual<uint>(
                                                (uint)ErrorStatus.InvalidDomainState,
                                                (uint)uintMethodStatus,
                                                "MS-LSAD",
                                                1034,
                                                @"In LsarSetTrustedDomainInfoByName, the server MUST return 
                                                STATUS_INVALID_DOMAIN_STATE when the TRUST_ATTRIBUTE_FOREST_TRANSITIVE
                                                flag is set prior to reaching DS_BEHAVIOR_WIN2003 forest
                                                functional level.");

                                            #endregion MS-LSAD_R1034
                                        }
                                        else if (trustedDomainInfo.TrustAttr == 
                                                    LsadUtilities.TRUST_ATTRIBUTE_CROSS_ORGANIZATION
                                                     && forestFuncLevel == ForestFunctionalLevel.DS_BEHAVIOR_WIN2000)
                                        {
                                            #region MS-LSAD_R1036

                                            Site.CaptureRequirementIfAreEqual<uint>(
                                                (uint)ErrorStatus.InvalidDomainState,
                                                (uint)uintMethodStatus,
                                                "MS-LSAD",
                                                1036,
                                                @"In LsarSetTrustedDomainInfoByName, the server MUST return 
                                                STATUS_INVALID_DOMAIN_STATE when the TRUST_ATTRIBUTE_CROSS_ORGANIZATION
                                                flag is set for a trust in a domain where the forest is not at 
                                                DS_BEHAVIOR_WIN2003 forest functional level.");

                                            #endregion MS-LSAD_R1036

                                            #region MS-LSAD_R1003

                                            Site.CaptureRequirementIfAreEqual<uint>(
                                                (uint)ErrorStatus.InvalidDomainState,
                                                (uint)uintMethodStatus,
                                                "MS-LSAD",
                                                1003,
                                                @"In LsarSetTrustedDomainInfoByName, The server MUST return 
                                                STATUS_InvalidDomainState when the The domain is in the wrong 
                                                state to perform the operation");

                                            #endregion
                                        }
                                        else if (trustedDomainInfo.TrustAttr == 
                                                    LsadUtilities.TRUST_ATTRIBUTE_FOREST_TRANSITIVE
                                                     && !isRootDomain)
                                        {
                                            #region MS-LSAD_R1035

                                            Site.CaptureRequirementIfAreEqual<uint>(
                                                (uint)ErrorStatus.InvalidDomainState,
                                                (uint)uintMethodStatus,
                                                "MS-LSAD",
                                                1035,
                                                @"In LsarSetTrustedDomainInfoByName, the server MUST return 
                                                STATUS_INVALID_DOMAIN_STATE when the TRUST_ATTRIBUTE_FOREST_TRANSITIVE 
                                                flag is set for a trust in a domain that is not the root domain 
                                                of the forest.");

                                            #endregion MS-LSAD_R1035
                                        }
                                    }
                                    else
                                    {
                                        //// Checking if the Data is really set by querying the data and 
                                        //// checking if they are same
                                        this.uintStatus = lsadClientStack.LsarQueryTrustedDomainInfoByName(
                                            PolicyHandle.Value,
                                            domainName[0],
                                            trustedInfoClass,
                                            out queryInformation);

                                        if (utilities.CheckIfDataIsSame(
                                            trustedInfoClass,
                                            domainInformation,
                                            queryInformation.HasValue ? queryInformation.Value : 
                                                new _LSAPR_TRUSTED_DOMAIN_INFO()))
                                        {
                                            #region MS-LSAD_R644

                                            Site.CaptureRequirementIfAreEqual<uint>(
                                                (uint)ErrorStatus.Success,
                                                (uint)uintMethodStatus,
                                                "MS-LSAD",
                                                644,
                                                @"In LsarSetTrustedDomainInfoByName, The server MUST return 
                                            STATUS_SUCCESS when the request was successfully completed.");

                                            #endregion

                                            #region MS-LSAD_R638

                                            Site.CaptureRequirementIfAreEqual<uint>(
                                                (uint)ErrorStatus.Success,
                                                (uint)uintMethodStatus,
                                                "MS-LSAD",
                                                638,
                                                @"In LsarQueryTrustedDomainInfoByName, The server MUST return 
                                                STATUS_SUCCESS when the request was successfully completed.");

                                            #endregion
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                #region MS-LSAD_R646

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidParameter,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    646,
                    @"In LsarSetTrustedDomainInfoByName, The server MUST return STATUS_INVALID_PARAMETER
                                    when one of the supplied arguments is invalid.");

                #endregion
            }

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion

        #region LsarQueryTrustedDomainInfo

        /// <summary>
        ///  The QueryTrustedDomainInfo method is invoked to retrieve information 
        ///  on a trusted domain object. The Object is identified via the SID 
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="sid">It is for validating the trusted domain accountSid whether it is valid or invalid</param>
        /// <param name="trustedDomainSid">Contains the trusted domain object accountSid</param>
        /// <param name="trustedInformation">Contains the type of trusted domain object information</param>
        /// <param name="forestFunctionalLevel">Contains forest functional levels</param>
        /// <param name="trustDomainInfo">Out param which contains valid or invalid
        /// trusted domain information</param>
        /// <returns>Returns Success if the method is successful
        ///          Returns InvalidParameter if the parameters passed to the method are not valid
        ///          Returns AccessDenied if the caller does not have the permissions to 
        ///          perform this operation
        ///          Returns InvalidHandle if the passed in policy handle is not valid
        ///          Returns NotSupported if the specified information class is not supported
        ///          Returns NoSuchDomain if specified trusted domain object does not exist</returns>
        /// Disable warning CA1502 because it will affect the implementation of Adapter and Model codes 
        /// if do any changes about maintainability.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", 
            Justification = "affect the implementation of Adapter and Model codes")]
        public ErrorStatus QueryTrustedDomainInfo(
            int handleInput,
            DomainSid sid,
            string trustedDomainSid,
            TrustedInformationClass trustedInformation,
            ForestFunctionalLevel forestFunctionalLevel,
            out TrustedDomainInformation trustDomainInfo)
        {
            _LSAPR_TRUSTED_DOMAIN_INFO domainInformation = new _LSAPR_TRUSTED_DOMAIN_INFO();
            _LSAPR_TRUSTED_DOMAIN_INFO? domainInformation1 = new _LSAPR_TRUSTED_DOMAIN_INFO();
            _RPC_SID[] trustSid = new _RPC_SID[1];
            TRUSTED_DOMAIN_INFORMATION_EX trustedDomainInfo = new TRUSTED_DOMAIN_INFORMATION_EX();
            _TRUSTED_INFORMATION_CLASS trustedInfoClass = _TRUSTED_INFORMATION_CLASS.TrustedDomainInformationEx;
            trustDomainInfo = TrustedDomainInformation.Invalid;
            invalidParamterCount = 0;
            PolicyHandle = validPolicyHandle;
            this.isInvalidHandle = false;

            // Checking if the Model received and invalid handle
            if (stPolicyInformation.PHandle != handleInput)
            {
                PolicyHandle = utilities.CreateAnInvalidHandle(false);
                this.isInvalidHandle = true;
                invalidParamterCount++;
            }

            // Checking if the Name of the TDO received by the model is present
            if (trustObjectCreateinformation.strDomainSid.Equals(trustedDomainSid))
            {
                trustSid = utilities.GetSid(sid, LsadManagedAdapter.ValidSid);
                this.isDomainPresent = true;
            }
            else
            {
                trustSid = utilities.GetSid(sid, LsadManagedAdapter.NoSid);
                this.isDomainPresent = false;
                invalidParamterCount++;
            }
            
            if (sid == DomainSid.Invalid || trustedInformation == TrustedInformationClass.Invalid)
            {
                invalidParamterCount++;
            }

            // Initializing the Information Class that we are requesting 
            // to query and check required access.
            // Checking if it is a valid information class to be queried
            utilities.InitializeInformationClass(
                trustedInformation,
                trustedDomainInfo,
                QUERY_OPERATION,
                ref domainInformation);
            
            trustedInfoClass = (_TRUSTED_INFORMATION_CLASS)trustedInformation;            

            if (isAccessDenied && this.isDomainPresent)
            {
                invalidParamterCount++;
            }

            // TrustedDomainSupportedEncryptionTypes doesn't work on 2k3 as it was introduced in windows Vista
            if (isWindows
                    && this.PDCOSVersion == ServerVersion.Win2003
                    && trustedInformation == TrustedInformationClass.TrustedDomainSupportedEncryptionTypes)
            {
                if (invalidParamterCount > 1)
                {
                    trustDomainInfo = TrustedDomainInformation.Invalid;
                    return ErrorStatus.ErrorUnKnown;
                }
                else if (sid == DomainSid.Invalid)
                {
                    trustDomainInfo = TrustedDomainInformation.Invalid;
                    return ErrorStatus.InvalidParameter;
                }
                else if (this.isInvalidHandle)
                {
                    trustDomainInfo = TrustedDomainInformation.Invalid;
                    return ErrorStatus.InvalidHandle;
                }
                else if (!this.isDomainPresent)
                {
                    return ErrorStatus.NoSuchDomain;
                }
                else if (isAccessDenied & this.isDomainPresent)
                {
                    // Need to remove it.
                    trustDomainInfo = TrustedDomainInformation.Invalid;
                    return ErrorStatus.AccessDenied;
                }
                else
                {
                    trustDomainInfo = TrustedDomainInformation.Valid;
                    return ErrorStatus.Success;
                }
            }

            if (intCheckInfoClass == INVALIDPARAM && trustedInformation != TrustedInformationClass.Invalid)
            {
                invalidParamterCount++;
            }

            domainInformation1 = domainInformation;
            uintMethodStatus = lsadClientStack.LsarQueryTrustedDomainInfo(
                PolicyHandle.Value,
                trustSid[0],
                trustedInfoClass,
                out domainInformation1);

            // The TD doesn't describe the protocol behavior when the opnum receives 2 or more invalid inputs
            // To handle the condition, we return ErrorStatus.ErrorUnknown when there are 2 or more Invalid Inputs.
            if (invalidParamterCount > 1)
            {
                trustDomainInfo = TrustedDomainInformation.Invalid;
                return ErrorStatus.ErrorUnKnown;
            }

            //// Data Validation Check
            if (sid == DomainSid.Valid)
            {
                //// Checking if the Class specified is out of enum range
                if ((intCheckInfoClass != 0) && (trustedInformation != TrustedInformationClass.Invalid))
                {
                    #region MS-LSAD_R1018

                    Site.CaptureRequirementIfIsTrue(
                        ((uint)uintMethodStatus != (uint)ErrorStatus.Success),
                        "MS-LSAD",
                        1018,
                        @"In LsarQueryTrustedDomainInfo, for InformationClass values TrustedControllersInformation, 
                        TrustedDomainAuthInformationInternal, TrustedDomainFullInformationInternal, and for any 
                        values that would be rejected by an LsarQueryInfoTrustedDomain call, the server MUST 
                        reject the request with an implementation-specific error.");

                    #endregion

                    trustDomainInfo = TrustedDomainInformation.Invalid;

                    return ErrorStatus.ImplementationSpecific;
                }
                else if (trustedInformation == TrustedInformationClass.Invalid)
                {
                    #region MS-LSAD_R600

                    if ((uint)ErrorStatus.InvalidParameter == (uint)uintMethodStatus)
                    {
                        Site.CaptureRequirement(
                            "MS-LSAD",
                            600,
                            @"In LsarQueryTrustedDomainInfo, Not all values are valid. For values outside the 
                            TRUSTED_INFORMATION_CLASS enumeration range, the server MUST reject the request 
                            with STATUS_INVALID_PARAMETER.");
                    }                    

                    #endregion MS-LSAD_R600
                }
                else
                {
                    //// Server returns INVALID_HANDLE if the handle is not a valid handle
                    if (!this.isInvalidHandle)
                    {
                        if (!this.isDomainPresent)
                        {
                            #region MS-LSAD_R605

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.NoSuchDomain,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                605,
                                @"In LsarQueryTrustedDomainInfo, Processing of TrustedDomainSid, The server MUST 
                                verify that a trusted domain object with TrustedDomainSid exists in its policy database
                                and fail the request with STATUS_NO_SUCH_DOMAIN otherwise.");

                            #endregion
                        }
                        else
                        {
                            #region SwitchClass

                            switch (intCheckInfoClass)
                            {
                                case SUCCESS:

                                    #region MS-LSAD_R598

                                    Site.CaptureRequirementIfAreEqual<uint>(
                                        (uint)ErrorStatus.Success,
                                        (uint)uintMethodStatus,
                                        "MS-LSAD",
                                        598,
                                        @"LsarQueryTrustedDomainInfo MUST return STATUS_SUCCESS when 
                                        the request was succesfully completed");

                                    #endregion

                                    trustDomainInfo = TrustedDomainInformation.Valid;

                                    break;

                                case INVALIDPARAM:

                                    #region MS-LSAD_R600

                                    if ((uint)ErrorStatus.InvalidParameter == (uint)uintMethodStatus)
                                    {
                                        Site.CaptureRequirement(
                                            "MS-LSAD",
                                            600,
                                            @"In LsarQueryTrustedDomainInfo, Not all values are valid. For values 
                                            outside the TRUSTED_INFORMATION_CLASS enumeration range, the server MUST 
                                            reject the request with STATUS_INVALID_PARAMETER.");
                                    }

                                    #endregion

                                    trustDomainInfo = TrustedDomainInformation.Invalid;

                                    break;

                                case INVALIDINFOCLASS:
                                    trustDomainInfo = TrustedDomainInformation.Invalid;

                                    break;
                            }

                            #endregion
                        }
                    }
                    else
                    {
                        #region MS-LSAD_R604

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.InvalidHandle,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            604,
                            @"In LsarQueryTrustedDomainInfo, If the handle is not a valid context handle to the policy
                            object, the server MUST return STATUS_INVALID_HANDLE.");

                        #endregion
                    }
                }
            }
            else
            {
                #region MS-LSAD_R1005

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidParameter,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    1005,
                    @"In LsarQueryTrustedDomainInfo, If One or more of the supplied parameters was invalid, 
                    the server MUST return STATUS_INVALID_PARAMETER.");

                #endregion
            }

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion

        #region LsarSetTrustedDomainInfo

        /// <summary>
        ///  The SetTrustedDomainInfo method is invoked to set information on a trusted domain object. 
        ///  The SID of the object is used to identify it.
        ///  In some cases, if the trusted domain object does not exist, it will be created. 
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="trustedDomainInfo">Contains the information of trusted domain object</param>
        /// <param name="sid">It is for validating the trusted domain accountSid whether it is valid or invalid</param>        
        /// <param name="forestFuncLevel">Contains forest functional levels</param>
        /// <param name="trustedInformation">Contains the type of trusted domain object information</param>
        /// <param name="isRootDomain">It is to check whether the current domain is a root domain</param>
        /// <param name="authInfo">Contains the authentication information of
        /// trusted domain object information</param>
        /// <param name="desiredAccess">Contains the access required for trust handle</param>
        /// <param name="daclAllows">It is to check whether DACL allows the requested access</param>
        /// <returns>Returns Success if the method is successful
        ///          Returns InvalidParameter if the parameters passed to the method are not valid
        ///          Returns AccessDenied if the caller does not have the permissions to 
        ///          perform this operation
        ///          Returns InvalidHandle if the passed in policy handle is not valid
        ///          Returns NoSuchDomain if the specified trusted domain object
        ///          does not exist
        ///          Returns NotSupportedOnSBS if operation is not supported on small business server 2k3
        ///          Returns InvalidDomainState if the operation cannot complete
        ///          in current state of the domain
        ///          Returns DirectoryServiceRequired if the active directory is not available on the server
        ///          Returns InvalidSid if the security identifier of the trusted domain is not valid
        ///          Returns CurrentDomainNotAllowed if the trust cannot be established with the current domain
        ///          Returns ObjectNameCollision if another trusted domain object already exists
        ///          that matches some of the identifying information of the supplied information</returns>
        /// Disable warning CA1502 and CA1505 because it will affect the implementation of Adapter and Model codes if do
        /// any changes about maintainability.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", 
            Justification = "affect the implementation of Adapter and Model codes")]
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", 
            Justification = "affect the implementation of Adapter and Model codes")]
        public ErrorStatus SetTrustedDomainInfo(
            int handleInput,
            TRUSTED_DOMAIN_INFORMATION_EX trustedDomainInfo,
            DomainSid sid,
            ForestFunctionalLevel forestFuncLevel,
            TrustedInformationClass trustedInformation,
            bool isRootDomain,
            TRUSTED_DOMAIN_AUTH_INFORMATION authInfo,
            UInt32 desiredAccess,
            bool daclAllows)
        {
            _LSAPR_TRUSTED_DOMAIN_INFO domainInformation = new _LSAPR_TRUSTED_DOMAIN_INFO();
            _LSAPR_TRUSTED_DOMAIN_INFO? queryInformation = new _LSAPR_TRUSTED_DOMAIN_INFO();
            _RPC_SID[] trustSid = new _RPC_SID[1];
            _RPC_UNICODE_STRING[] domainName = new _RPC_UNICODE_STRING[1];
            _TRUSTED_INFORMATION_CLASS trustedInfoClass = _TRUSTED_INFORMATION_CLASS.TrustedDomainInformationEx;
            DomainType checkDomainName = DomainType.ValidDomainName;

            PolicyHandle = validPolicyHandle;

            // Checking if the Model received and invalid handle
            if (stPolicyInformation.PHandle != handleInput)
            {
                PolicyHandle = utilities.CreateAnInvalidHandle(false);
            }

            // Checking if the SID of the TDO received by the model is already present
            if (trustObjectCreateinformation.strDomainSid.Equals(trustedDomainInfo.TrustDomain_Sid))
            {
                trustSid = utilities.GetSid(sid, LsadManagedAdapter.ValidSid);
                this.isDomainPresent = true;
            }
            else
            {
                trustSid = utilities.GetSid(sid, LsadManagedAdapter.NoSid);
                this.isDomainPresent = false;
            }           

            // TrustedDomainNameInformation Creates an TDO if TDO with the specified SID is not present.
            // So processing rules are similar to LsarCreateTrustedDomain
            if (trustedInformation == TrustedInformationClass.TrustedDomainNameInformation)
            {
                // Checking if Domain Name received by model matches Current Domain
                if (trustedDomainInfo.TrustDomainName.Equals(Convert.ToString(DomainType.CurrentDomain)))
                {
                    checkDomainName = DomainType.CurrentDomain;
                    this.isCurrentDomain = true;
                }
                else
                {
                    checkDomainName = DomainType.ValidDomainName;
                }

                // Checking if the Name of the TDO received by the model is already present
                if (trustObjectCreateinformation.strTdoDnsName.Equals(trustedDomainInfo.TrustDomainName))
                {
                    this.isDomainPresent = true;
                }
                else
                {
                    _RPC_SID[] tempSid = new _RPC_SID[1];
                    tempSid = utilities.GetSid(DomainSid.Valid, LsadManagedAdapter.ValidSid);
                    this.uintStatus = lsadClientStack.LsarDeleteTrustedDomain(
                        validPolicyHandle.Value,
                        tempSid[0]);
                }

                utilities.GetTheDomainName(checkDomainName, ref domainName);
                domainInformation.TrustedDomainNameInfo.Name = domainName[0];
                domainInformation.TrustedDomainNameInfo.Name.Length =
                    (ushort)(domainInformation.TrustedDomainNameInfo.Name.Buffer.Length * 2);
                domainInformation.TrustedDomainNameInfo.Name.MaximumLength =
                    (ushort)((domainInformation.TrustedDomainNameInfo.Name.Length)+2);
                trustSid = utilities.GetSid(sid, LsadManagedAdapter.ValidSid);

                //// Initialize the SID
                if (trustedDomainInfo.TrustDomain_Sid.Equals(trustObjectCreateinformation.strDomainSid))
                {
                    this.isDomainPresent = true;
                }

                intCheckInfoClass = SUCCESS;
            }
            else if (trustedInformation == TrustedInformationClass.TrustedDomainInformationEx)
            {
                utilities.InitializeInformationClass(
                    trustedInformation,
                    trustedDomainInfo,
                    SET_OPERATION,
                    ref domainInformation);

                intCheckInfoClass = INVALIDPARAM;
            }
            else if (trustedInformation == TrustedInformationClass.TrustedPasswordInformation)
            {
                utilities.InitializeInformationClass(
                    trustedInformation,
                    trustedDomainInfo,
                    SET_OPERATION,
                    ref domainInformation);

                // Win2k8r2 unsupport TrustedDomainInformationEx & TrustedPasswordInformation in informationType 
                // trustedInformation and will return STATUS_INVALID_PARAMETER.
                if (this.PDCOSVersion >= ServerVersion.Win2008R2)
                {
                    intCheckInfoClass = INVALIDPARAM;
                }
                else
                {
                    intCheckInfoClass = SUCCESS;
                }
            }
            else
            {
                utilities.InitializeInformationClass(
                    trustedInformation,
                    trustedDomainInfo,
                    SET_OPERATION,
                    ref domainInformation);
            }

            if (trustedInformation == TrustedInformationClass.Invalid)
            {
                trustedInfoClass = _TRUSTED_INFORMATION_CLASS.TrustedControllersInformation;
            }
            else
            {
                trustedInfoClass = (_TRUSTED_INFORMATION_CLASS)trustedInformation;
            }

            // TrustedDomainSupportedEncryptionTypes was introduced in windows vista, so it won't work on windows 2k3
            if (isWindows
                    && this.PDCOSVersion == ServerVersion.Win2003
                    && trustedInformation == TrustedInformationClass.TrustedDomainSupportedEncryptionTypes)
            {
                return ErrorStatus.InvalidParameter;
            }

            uintMethodStatus = lsadClientStack.LsarSetTrustedDomainInfo(
                PolicyHandle.Value,
                trustSid[0],
                trustedInfoClass,
                domainInformation);

            ////check if the caller has the permissions
            if (!this.IsInDomainAdminsGroup)
            {
                #region MS-LSAD_R608

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.AccessDenied,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    608,
                    @"In LsarSetTrustedDomainInfo, The server MUST return STATUS_ACCESS_DENIED when the
                    caller does not have the permissions to perform the operation.");

                #endregion
            }

            //// Checking Data Validity
            if (sid == DomainSid.Valid
                    && (trustedInformation == TrustedInformationClass.TrustedDomainInformationEx
                            || trustedInformation == TrustedInformationClass.TrustedPasswordInformation))
            {
                string IsR1049Implemented = R1049Implementation;

                if (isWindows)
                {
                    if (intCheckInfoClass == SUCCESS
                            && (this.PDCOSVersion == ServerVersion.Win2003
                                    || this.PDCOSVersion == ServerVersion.Win2008))
                    {
                        #region MS-LSAD_R1050

                        Site.CaptureRequirementIfAreNotEqual<uint>(
                            (uint)ErrorStatus.InvalidParameter,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            1050,
                            @"<79> Section 3.1.4.7.3: Windows 2000 Server, Windows Server 2003, and Windows Server
                            2008 support these InformationClass values[TrustedDomainInformationEx,
                            TrustedPasswordInformation].");

                        #endregion
                    }
                    else
                    {
                        if (this.PDCOSVersion >= ServerVersion.Win2008R2)
                        {
                            #region MS-LSAD_R1049

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.InvalidParameter,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                1049,
                                @"The server MAY support the following InformationClass values
                                [TrustedDomainInformationEx, TrustedPasswordInformation].<79>");

                            #endregion

                            #region MS-LSAD_R1051

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.InvalidParameter,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                1051,
                                @"If the server does not support these values[TrustedDomainInformationEx,
                                TrustedPasswordInformation], it MUST return STATUS_INVALID_PARAMETER.");

                            #endregion
                        }
                    }

                    if (IsR1049Implemented == null)
                    {
                        Site.Properties.Add("IsR1049Implemented", bool.FalseString);
                        IsR1049Implemented = bool.FalseString;
                    }
                }
                else
                {
                    if (IsR1049Implemented != null)
                    {
                        bool implSigns = bool.Parse(IsR1049Implemented);
                        bool isSatisfied = ((NtStatus)ErrorStatus.InvalidParameter != uintMethodStatus);

                        #region MS-LSAD_R1049

                        Site.CaptureRequirementIfAreEqual<bool>(
                            implSigns,
                            isSatisfied,
                            "MS-LSAD",
                            1049,
                            @"The server MAY support the following InformationClass values
                            [TrustedDomainInformationEx, TrustedPasswordInformation].<79>");

                        #endregion

                        if (!implSigns)
                        {
                            #region MS-LSAD_R1051

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.InvalidParameter,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                1051,
                                @"If the server does not support these values[TrustedDomainInformationEx,
                                TrustedPasswordInformation], it MUST return STATUS_INVALID_PARAMETER.");

                            #endregion
                        }
                    }
                }
            }

            if (sid == DomainSid.Invalid)
            {
                #region MS-LSAD_R609

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidParameter,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    609,
                    @"In LsarSetTrustedDomainInfo, the server MUST return STATUS_INVALID_PARAMETER when
                    one or more of the supplied parameters was invalid.");

                #endregion
            }
            else
            {
                switch (intCheckInfoClass)
                {
                    case SUCCESS:
                        if (trustedInformation == TrustedInformationClass.TrustedDomainNameInformation)
                        {
                            #region ValidationForTrustedDomainNameInfo

                            if (!this.isDC)
                            {
                                #region MS-LSAD_R1014

                                Site.CaptureRequirementIfAreEqual<uint>(
                                    (uint)ErrorStatus.DirectoryServiceRequired,
                                    (uint)uintMethodStatus,
                                    1014,
                                    @"In LsarSetTrustedDomainInfo, the server MUST be in a Domain Controller 
                                    and return DIRECTORY_SERVICE_REQUIRED if there is no active directory 
                                    installed on it.");

                                #endregion
                            }
                            else if (this.isDC && this.domainState == ProtocolServerConfig.ReadOnlyDomainController)
                            {
                                #region MS-LSAD_R612

                                Site.CaptureRequirementIfAreNotEqual<uint>(
                                    (uint)ErrorStatus.Success,
                                    (uint)uintMethodStatus,
                                    "MS-LSAD",
                                    612,
                                    @"In LsarSetTrustedDomainInfo,  In the processing of PolicyHandle,  
                                    If the server is a read-only domain controller, it MUST return an error.");

                                #endregion MS-LSAD_R612

                                #region MS-LSAD_R613

                                // Windows 2k8 specific requirement.
                                if (isWindows && this.PDCOSVersion >= ServerVersion.Win2008)
                                {
                                    Site.CaptureRequirementIfAreEqual<uint>(
                                        (uint)ErrorStatus.ObjectNameNotFound,
                                        (uint)uintMethodStatus,
                                        "MS-LSAD",
                                        613,
                                        @"<78> Section 3.1.4.7.3: In LsarSetTrustedDomainInfo, In the processing 
                                        of PolicyHandle, If the server is windows 2008 or Windows Server 2008 R2 and 
                                        if it is a read-only domain controller, it MUST return 
                                        STATUS_OBJECT_NAME_NOT_FOUND error.");
                                }

                                #endregion MS-LSAD_R613
                            }
                            else
                            {
                                // Since this opnum creates an object if one is not present, it checks if the
                                // name clashes with the name of the current domain.
                                if (this.isCurrentDomain)
                                {
                                    #region MS-LSAD_R1019

                                    Site.CaptureRequirementIfAreEqual<uint>(
                                        (uint)ErrorStatus.CurrentDomainNotAllowed,
                                        (uint)uintMethodStatus,
                                        "MS-LSAD",
                                        1019,
                                        @"In LsarSetTrustedDomainInfo, in the processing of 
                                        TrustedDomainNameInformation,  the server MUST return 
                                        STATUS_CURRENT_DOMAIN_NOT_ALLOWED if the name or SID 
                                        refers to the Current Domain of which the server is part of.");

                                    #endregion MS-LSAD_R1019
                                }
                                else
                                {
                                    // Checking if the handle is invalid
                                    if (stPolicyInformation.PHandle != handleInput)
                                    {
                                        #region MS-LSAD_R610

                                        Site.CaptureRequirementIfAreEqual<uint>(
                                            (uint)ErrorStatus.InvalidHandle,
                                            (uint)uintMethodStatus,
                                            "MS-LSAD",
                                            610,
                                            @"In LsarSetTrustedDomainInfo, the server MUST return STATUS_INVALID_HANDLE
                                            when PolicyHandle is not a valid handle.");

                                        #endregion
                                    }
                                    else
                                    {
                                        if (this.isDomainPresent)
                                        {
                                            #region MS-LSAD_R1013

                                            Site.CaptureRequirementIfAreEqual<uint>(
                                                (uint)ErrorStatus.ObjectNameCollision,
                                                (uint)uintMethodStatus,
                                                "MS-LSAD",
                                                1013,
                                                @"In LsarSetTrustedDomainInfo, while processing 
                                                TrustedDomainInformation, the server MUST check for the existence of 
                                                a Trusted Domain Object with the given SID and fail the request with 
                                                OBJECT_NAME_COLLISION if the Object already exists.");

                                            #endregion
                                        }
                                        else
                                        {
                                            //// Checking to be sure if the TDO is created or not by 
                                            //// enumerating all the objects.
                                            if (utilities.CheckTheDomain(
                                                trustSid[0], 
                                                domainName[0], 
                                                PolicyHandle.Value))
                                            {
                                                #region MS-LSAD_R607

                                                Site.CaptureRequirementIfAreEqual<uint>(
                                                    (uint)ErrorStatus.Success,
                                                    (uint)uintMethodStatus,
                                                    "MS-LSAD",
                                                    607,
                                                    @"In LsarSetTrustedDomainInfo  the server MUST return STATUS_SUCCESS
                                                    when the request was succesfully completed");

                                                #endregion

                                                #region MS-LSAD_R614

                                                Site.CaptureRequirementIfAreEqual<uint>(
                                                    (uint)ErrorStatus.Success,
                                                    (uint)uintMethodStatus,
                                                    "MS-LSAD",
                                                    614,
                                                    @"In LsarSetTrustedDomainInfo  in the processing of InformationClass
                                                    (TrustedDomainNameInformation), The server MUST act as if an 
                                                    LsarCreateTrustedDomain message came in with Trusted Domain Name as
                                                    the name passed in TrustedDomainInformation and Trusted Domain SID 
                                                    as the SID passed in TrustedDomainSid parameter is being
                                                    processed.");

                                                #endregion MS-LSAD_R614
                                            }

                                            trustObjectCreateinformation.strTdoDnsName = 
                                                trustedDomainInfo.TrustDomainName;
                                            trustObjectCreateinformation.strTdoNetBiosName =
                                                trustedDomainInfo.TrustDomain_NetBiosName;
                                            trustObjectCreateinformation.intTdoHandleNumber = 2;
                                            trustObjectCreateinformation.strDomainSid = 
                                                trustedDomainInfo.TrustDomain_Sid;
                                            trustObjectCreateinformation.isForestInformationPresent = false;
                                            trustObjectCreateinformation.doesTdoSupportForestInformation = false;
                                            trustObjectCreateinformation.uintTdoDesiredAccess = 0;
                                            trustObjectCreateinformation.uintTrustAttr = 0;
                                            trustObjectCreateinformation.uintTrustDir = 2;
                                            trustObjectCreateinformation.uintTrustType = 1;
                                            this.isitSetTrustedDomainInfo = true;
                                        }
                                    }
                                }
                            }

                            return (ErrorStatus)uintMethodStatus;

                            #endregion ValidationForTrustedDomainNameInfo
                        }
                        else if (trustedInformation == TrustedInformationClass.TrustedPasswordInformation)
                        {
                            #region ValidationForTrustedPasswordInformation

                            // Checking if the handle is invalid.
                            if (stPolicyInformation.PHandle != handleInput)
                            {
                                #region MS-LSAD_R610

                                Site.CaptureRequirementIfAreEqual<uint>(
                                    (uint)ErrorStatus.InvalidHandle,
                                    (uint)uintMethodStatus,
                                    "MS-LSAD",
                                    610,
                                    @"In LsarSetTrustedDomainInfo, the server MUST return STATUS_INVALID_HANDLE
                                            when PolicyHandle is not a valid handle.");

                                #endregion
                            }
                            else
                            {
                                // server returns NO_SUCH_DOMAIN when there is no domain present.
                                if (!this.isDomainPresent)
                                {
                                    #region MS-LSAD_R623
                                    Site.CaptureRequirementIfAreEqual<uint>(
                                        (uint)ErrorStatus.NoSuchDomain,
                                        (uint)uintMethodStatus,
                                        "MS-LSAD",
                                        623,
                                        @"In LsarSetTrustedDomainInfo  in the processing of InformationClass 
                                        (TrustedPasswordInformation), The server MUST verify that a trusted 
                                        domain object with the specified TrustedDomainSid exists in its policy database.
                                        If it does not exists, it MUST fail with STATUS_NO_SUCH_DOMAIN.");

                                    #endregion

                                    #region MS-LSAD_R622

                                    Site.CaptureRequirementIfAreEqual<uint>(
                                        (uint)ErrorStatus.NoSuchDomain,
                                        (uint)uintMethodStatus,
                                        "MS-LSAD",
                                        622,
                                        @"In LsarSetTrustedDomainInfo  in the processing of InformationClass
                                        (TrustedPasswordInformation), The server MUST verify that a trusted domain 
                                        object with TrustedDomainSid exists in its policy database.");

                                    #endregion
                                }
                                else
                                {
                                    #region MS-LSAD_R607

                                    Site.CaptureRequirementIfAreEqual<uint>(
                                        (uint)ErrorStatus.Success,
                                        (uint)uintMethodStatus,
                                        "MS-LSAD",
                                        607,
                                        @"In LsarSetTrustedDomainInfo  the server MUST return STATUS_SUCCESS
                                                    when the request was succesfully completed");

                                    #endregion
                                }
                            }

                            return (ErrorStatus)uintMethodStatus;

                            #endregion
                        }
                        else if (trustedInformation == TrustedInformationClass.TrustedPosixOffsetInformation)
                        {
                            #region ValidationForPosixOffset

                            // Checking if the handle provided is invalid.
                            if (stPolicyInformation.PHandle != handleInput)
                            {
                                #region MS-LSAD_R610

                                Site.CaptureRequirementIfAreEqual<uint>(
                                    (uint)ErrorStatus.InvalidHandle,
                                    (uint)uintMethodStatus,
                                    "MS-LSAD",
                                    610,
                                    @"In LsarSetTrustedDomainInfo, the server MUST return STATUS_INVALID_HANDLE
                                            when PolicyHandle is not a valid handle.");

                                #endregion
                            }
                            else
                            {
                                // returns NO_SUCH_DOMAIN if there is no domain present with the specified name
                                if (!this.isDomainPresent)
                                {
                                    #region MS-LSAD_R618

                                    Site.CaptureRequirementIfAreEqual<uint>(
                                        (uint)ErrorStatus.NoSuchDomain,
                                        (uint)uintMethodStatus,
                                        "MS-LSAD",
                                        618,
                                        @"In LsarSetTrustedDomainInfo  in the processing of InformationClass 
                                        (TrustedPosixOffsetInformation), The server MUST verify that a trusted domain
                                        object with TrustedDomainSid exists in its policy database.");

                                    #endregion MS-LSAD_R618

                                    #region MS-LSAD_R619

                                    Site.CaptureRequirementIfAreEqual<uint>
                                        ((uint)ErrorStatus.NoSuchDomain,
                                        (uint)uintMethodStatus,
                                        "MS-LSAD",
                                        619,
                                        @"In LsarSetTrustedDomainInfo  in the processing of InformationClass
                                        (TrustedPosixOffsetInformation), The server MUST verify that a trusted domain
                                        object with the specified TrustedDomainSid exists in its policy database. 
                                        If it does exists, it MUST fail with STATUS_NO_SUCH_DOMAIN");

                                    #endregion

                                    #region MS-LSAD_R611

                                    Site.CaptureRequirementIfAreEqual<uint>(
                                        (uint)ErrorStatus.NoSuchDomain,
                                        (uint)uintMethodStatus,
                                        "MS-LSAD",
                                        611,
                                        @"In LsarSetTrustedDomainInfo, The server MUST return STATUS_NO_SUCH_DOMAIN 
                                        when The specified trusted domain object does not exist.");

                                    #endregion
                                }
                                else
                                {
                                    this.uintStatus = lsadClientStack.LsarQueryTrustedDomainInfo(
                                        PolicyHandle.Value,
                                        trustSid[0],
                                        trustedInfoClass,
                                        out queryInformation);

                                    if (utilities.CheckIfDataIsSame(
                                            trustedInfoClass,
                                            domainInformation,
                                            queryInformation.Value))
                                    {
                                        #region MS-LSAD_R607

                                        Site.CaptureRequirementIfAreEqual<uint>(
                                            (uint)ErrorStatus.Success,
                                            (uint)uintMethodStatus,
                                            "MS-LSAD",
                                            607,
                                            @"In LsarSetTrustedDomainInfo  the server MUST return STATUS_SUCCESS
                                                    when the request was succesfully completed");

                                        #endregion

                                        #region MS-LSAD_R621

                                        Site.CaptureRequirementIfAreEqual<uint>(
                                            (uint)ErrorStatus.Success,
                                            (uint)uintMethodStatus,
                                            "MS-LSAD",
                                            621,
                                            @"In LsarSetTrustedDomainInfo  in the processing of InformationClass 
                                            (TrustedPosixOffsetInformation),  Then the server MUST act as if an 
                                            LsarSetInformationTrustedDomain message is being processed.");

                                        #endregion MS-LSAD_R621
                                    }
                                }
                            }

                            return (ErrorStatus)uintMethodStatus;

                            #endregion ValidationForPosixOffset
                        }

                        break;

                    case INVALIDPARAM:

                        //// Win2k8r2 unsupport TrustedDomainInformationEx & TrustedPasswordInformation in 
                        //// informationType trustedInformation 
                        //// and will return STATUS_INVALID_PARAMETER.
                        if (trustedInformation != TrustedInformationClass.TrustedDomainInformationEx
                                && trustedInformation != TrustedInformationClass.TrustedDomainNameInformation
                                && trustedInformation != TrustedInformationClass.TrustedPasswordInformation
                                && trustedInformation != TrustedInformationClass.TrustedPosixOffsetInformation)
                        {
                            #region MS-LSAD_R625

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.InvalidParameter,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                625,
                                @"In LsarSetTrustedDomainInfo, the valid InformationClass values are
                                TrustedDomainNameInformation, TrustedDomainInformationEx, TrustedPosixOffsetInformation,
                                TrustedPasswordInformation. The server MUST return STATUS_INVALID_PARAMETER for all 
                                other InformationClass arguments.");

                            #endregion
                        }
                        else if (trustedInformation == TrustedInformationClass.TrustedDomainInformationEx && PDCOSVersion < ServerVersion.Win2008R2)
                        {
                            //// There is a product bug in the processing of TrustedDomainInformationEx,
                            //// The opnum will return STATUS_NO_SUCH_DOMAIN if no TDO with the specified SID exists,
                            //// The Opnum will return STATUS_ACCESS_DENIED if TDO with the specified SID exists.
                            #region ValidationForTrustedDomainInformationEx

                            if (!this.isDC)
                            {
                                #region MS-LSAD_R1014

                                Site.CaptureRequirementIfAreEqual<uint>(
                                    (uint)ErrorStatus.DirectoryServiceRequired,
                                    (uint)uintMethodStatus,
                                    1014,
                                    @"In LsarSetTrustedDomainInfo, the server MUST be in a Domain Controller 
                                    and return DIRECTORY_SERVICE_REQUIRED if there is no active directory 
                                    installed on it.");

                                #endregion
                            }
                            else
                            {
                                if (stPolicyInformation.PHandle != handleInput)
                                {
                                    #region MS-LSAD_R610

                                    Site.CaptureRequirementIfAreEqual<uint>(
                                        (uint)ErrorStatus.InvalidHandle,
                                        (uint)uintMethodStatus,
                                        "MS-LSAD",
                                        610,
                                        @"In LsarSetTrustedDomainInfo, the server MUST return STATUS_INVALID_HANDLE
                                            when PolicyHandle is not a valid handle.");

                                    #endregion
                                }
                                else
                                {
                                    if (this.isDomainPresent)
                                    {
                                        #region MS-LSAD_R1033

                                        Site.CaptureRequirementIfAreEqual<uint>(
                                            (uint)ErrorStatus.AccessDenied,
                                            (uint)uintMethodStatus,
                                            "MS-LSAD",
                                            1033,
                                            @"In LsarSetTrustedDomainInfo, in the processing of 
                                            TrustedDomainInformationEx, The server MUST check that a trusted
                                            domain object with the specified SID exists in its policy database.
                                            If the object  exists, the server MUST fail the request with the 
                                            STATUS_ACCESS_DENIED error code.");

                                        #endregion MS-LSAD_R1033
                                    }
                                    else
                                    {
                                        #region MS-LSAD_R615

                                        Site.CaptureRequirementIfAreEqual<uint>(
                                            (uint)ErrorStatus.NoSuchDomain,
                                            (uint)uintMethodStatus,
                                            "MS-LSAD",
                                            615,
                                            @"In LsarSetTrustedDomainInfo in the processing of InformationClass 
                                            (TrustedDomainInformationEx), The server MUST check that a trusted domain
                                            object with TrustedDomainSid exists in its policy database.");

                                        #endregion

                                        #region MS-LSAD_R1032

                                        Site.CaptureRequirementIfAreEqual<uint>(
                                            (uint)ErrorStatus.NoSuchDomain,
                                            (uint)uintMethodStatus,
                                            "MS-LSAD",
                                            1032,
                                            @"In LsarSetTrustedDomainInfo, in the processing of 
                                            TrustedDomainInformationEx, The server MUST check that a trusted
                                            domain object with the specified SID exists in its policy database. 
                                            If the object does not exist, the server MUST fail the request with 
                                            the STATUS_NO_SUCH_DOMAIN error code.");

                                        #endregion MS-LSAD_R1032
                                    }
                                }
                            }

                            return (ErrorStatus)uintMethodStatus;

                            #endregion
                        }

                        break;
                }
            }

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion

        #region LsarSetForestTrustInformation

        /// <summary>
        ///  The SetForestTrustInformation method is invoked to establish a trust 
        ///  relationship with another forest by attaching a set of records called the 
        ///  forest trust information to the trusted domain object.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="trustedDomainName">Contains the trusted domain name</param>
        /// <param name="name">It is for validating the trusted domain name whether it is valid or invalid</param>
        /// <param name="highestRecordType">Contains highest record type</param>
        /// <param name="recordCount">Contains record count</param>
        /// <param name="forestFuncLevel">Contains forest functional levels</param>
        /// <param name="isRootDomain">It is to check whether the current domain is a root domain</param>        
        /// <param name="collisionInfo">Out param which contains the collision information</param>
        /// <returns>Returns Success if the method is successful
        ///          Returns InvalidParameter if the parameters passed to the method are not valid
        ///          Returns AccessDenied if the caller does not have the permissions to 
        ///          perform this operation
        ///          Returns InvalidHandle if the passed in policy handle is not valid
        ///          Returns NoSuchDomain if the specified trusted domain object
        ///          does not exist
        ///          Returns InvalidDomainState if the operation cannot complete
        ///          in current state of the domain
        ///          Returns InvalidDomainRole if the server is not primary domain controller</returns>
        /// Disable warning CA1502 because it will affect the implementation of Adapter and Model codes 
        /// if do any changes about maintainability.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", 
            Justification = "affect the implementation of Adapter and Model codes")]
        public ErrorStatus SetForestTrustInformation(
            int handleInput,
            string trustedDomainName,
            ValidString name,
            int highestRecordType,
            int recordCount,
            ForestFunctionalLevel forestFuncLevel,
            bool isRootDomain,
            out CollisionInfo collisionInfo)
        {
            _RPC_UNICODE_STRING[] domainName = new _RPC_UNICODE_STRING[1];
            _LSA_FOREST_TRUST_RECORD_TYPE recordType = _LSA_FOREST_TRUST_RECORD_TYPE.ForestTrustDomainInfo;
            _LSA_FOREST_TRUST_COLLISION_INFORMATION? collisionInformation =
                new _LSA_FOREST_TRUST_COLLISION_INFORMATION();
            _LSA_FOREST_TRUST_INFORMATION? forestTrustInfo = new _LSA_FOREST_TRUST_INFORMATION();
            _LSA_FOREST_TRUST_INFORMATION forestTrustInfoValue = new _LSA_FOREST_TRUST_INFORMATION();

            forestTrustInfoValue.Entries = new _LSA_FOREST_TRUST_RECORD[1][];
            forestTrustInfoValue.Entries[0] = new _LSA_FOREST_TRUST_RECORD[1];

            const int WRITE_MODE = 0;
            trustObjectCreateinformation.isForestInformationPresent = false;
            invalidParamterCount = 0;
            byte checkOnly = WRITE_MODE;
            DomainType checkDomainName = DomainType.ValidDomainName;
            PolicyHandle = validPolicyHandle;

            //// Checking if the Model received and invalid handle
            if (stPolicyInformation.PHandle != handleInput)
            {
                PolicyHandle = utilities.CreateAnInvalidHandle(false);
                invalidParamterCount++;
            }

            //// Checking if the Name of the TDO received by the model is already present
            if (trustObjectCreateinformation.strTdoDnsName.Equals(trustedDomainName))
            {
                checkDomainName = DomainType.ValidDomainName;
                this.isDomainPresent = true;
            }
            else
            {
                checkDomainName = DomainType.NoDomainName;
                this.isDomainPresent = false;
                invalidParamterCount++;
            }

            //// Initialize the TDO Name
            utilities.GetTheDomainName(checkDomainName, ref domainName);

            if (name == ValidString.Valid)
            {
                domainName[0].Length = (ushort)(2 * domainName[0].Buffer.Length);
            }
            else
            {
                domainName[0].Length = (ushort)((2 * domainName[0].Buffer.Length) + 1);
            }

            domainName[0].MaximumLength = (ushort)(2 + domainName[0].Length);

            forestTrustInfoValue.Entries[0][0] = new _LSA_FOREST_TRUST_RECORD();
            forestTrustInfoValue.Entries[0][0].Flags = Flags_Values.NameNotMatchingSecurityPrincipal;
            forestTrustInfoValue.Entries[0][0].ForestTrustType = (_LSA_FOREST_TRUST_RECORD_TYPE)highestRecordType;

            if (forestTrustInfoValue.Entries[0][0].ForestTrustType == 
                _LSA_FOREST_TRUST_RECORD_TYPE.ForestTrustDomainInfo)
            {
                forestTrustInfoValue.Entries[0][0].ForestTrustData.DomainInfo.DnsName = domainName[0];
                forestTrustInfoValue.Entries[0][0].ForestTrustData.DomainInfo.NetbiosName = domainName[0];
                forestTrustInfoValue.Entries[0][0].ForestTrustData.DomainInfo.Sid =
                    utilities.GetSid(DomainSid.Valid, LsadManagedAdapter.ValidSid);
            }
            else if (forestTrustInfoValue.Entries[0][0].ForestTrustType == 
                _LSA_FOREST_TRUST_RECORD_TYPE.ForestTrustTopLevelName)
            {
                forestTrustInfoValue.Entries[0][0].ForestTrustData.TopLevelName = domainName[0];
                invalidParamterCount++;
            }
            else if (forestTrustInfoValue.Entries[0][0].ForestTrustType == 
                _LSA_FOREST_TRUST_RECORD_TYPE.ForestTrustTopLevelNameEx)
            {
                forestTrustInfoValue.Entries[0][0].ForestTrustData.TopLevelName = domainName[0];
                invalidParamterCount++;
            }
            else
            {
                forestTrustInfoValue.Entries[0][0].ForestTrustData.Data.Buffer =
                    Encoding.Default.GetBytes(LsadManagedAdapter.ForestDefaultData);

                forestTrustInfoValue.Entries[0][0].ForestTrustData.Data.Length = 
                    (uint)forestTrustInfoValue.Entries[0][0].ForestTrustData.Data.Buffer.Length;
                invalidParamterCount++;
            }

            if (name == ValidString.Invalid
                    || (trustObjectCreateinformation.uintTrustAttr != LsadUtilities.TRUST_ATTRIBUTE_FOREST_TRANSITIVE
                            && this.isDomainPresent))
            {
                invalidParamterCount++;
            }

            if (this.isDC && this.domainState == ProtocolServerConfig.DomainController)
            {
                invalidParamterCount++;
            }

            if ((this.isDC && !isRootDomain)
                     || !this.isDC
                     || (isWindows && forestFuncLevel != ForestFunctionalLevel.DS_BEHAVIOR_WIN2003))
            {
                invalidParamterCount++;
            }

            isAccessDenied = false;

            if (this.isDomainPresent)
            {
                if ((trustObjectCreateinformation.uintTdoDesiredAccess & (uint)ACCESS_MASK.TRUSTED_SET_AUTH) ==
                         (uint)ACCESS_MASK.TRUSTED_SET_AUTH)
                {
                    isAccessDenied = false;
                }
                else
                {
                    isAccessDenied = true;
                    invalidParamterCount++;
                }
            }

            forestTrustInfoValue.RecordCount = 0x1;
            forestTrustInfo = forestTrustInfoValue;

            uintMethodStatus = lsadClientStack.LsarSetForestTrustInformation(
                PolicyHandle.Value,
                domainName[0],
                recordType,
                forestTrustInfo,
                checkOnly,
                out collisionInformation);

            if (highestRecordType > (int)recordType)
            {
                #region MS-LSAD_R778

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidParameter,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    778,
                    @"In LsarSetForestTrustInformation, If the value in HighestRecordType argument is greater 
                    in value than the highest record type recognized by the server, the server MUST return 
                    STATUS_INVALID_PARAMETER.");

                #endregion MS-LSAD_R778

                collisionInfo = CollisionInfo.Invalid;
                return (ErrorStatus)uintMethodStatus;
            }

            //// The TD doesn't describe the protocol behavior when the opnum receives 2 or more invalid inputs
            //// To handle the condition, we return ErrorStatus.ErrorUnknown when there are 2 or more Invalid Inputs.
            if (invalidParamterCount > 1)
            {
                //// Checking if the Domain and the Domain Controller are in the correct state for this operation.
                if (!this.isDC)
                {
                }
                else if (this.isDC && !isRootDomain)
                {
                    #region MS-LSAD_R772

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.InvalidDomainState,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        772,
                        @"While implementing LsarSetForestTrustInformation: The server MUST refuse to service this
                        request if the server is not a trusted domain object in the root domain of the forest.The 
                        status code in this case is STATUS_INVALID_DOMAIN_STATE.");

                    #endregion
                }
                else if (this.isDC && this.domainState == ProtocolServerConfig.DomainController)
                {
                    #region MS-LSAD_R773

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.InvalidDomainRole,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        773,
                        @"In LsarSetForestTrustInformation, LsarSetForestTrustInformation request is valid 
                        only if the server is a primary domain controller. If the server is not a domain controller, 
                        the server MUST return STATUS_INVALID_DOMAIN_ROLE.");

                    #endregion
                }
                else if (isWindows
                             && this.isDC
                             && forestFuncLevel != ForestFunctionalLevel.DS_BEHAVIOR_WIN2003)
                {
                    #region MS-LSAD_R1037

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.InvalidDomainState,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        1037,
                        @"In LsarSetForestTrustInformation, The server MUST refuse to service this request
                        and STATUS_INVALID_DOMAIN_STATE MUST be returned by the server if the server is not 
                        at DS_BEHAVIOR_WIN2003 forest functional level.");

                    #endregion MS-LSAD_R1037
                }

                collisionInfo = CollisionInfo.Invalid;
                return ErrorStatus.ErrorUnKnown;
            }

            //// Data validation Check
            if (name == ValidString.Invalid)
            {
                #region MS-LSAD_R1008

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidParameter,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    1008,
                    @"In LsarSetForestTrustInformation, The server MUST return STATUS_INVALID_PARAMETER 
                    if Some of the parameters supplied were invalid.");

                #endregion
            }
            else
            {
                //// Server checks for the presence of TAFT flag on the TDO and returns INVALID_PARAMETER 
                //// if not present.
                if (!isRootDomain && this.isDC)
                {
                    #region MS-LSAD_R772

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.InvalidDomainState,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        772,
                        @"While implementing LsarSetForestTrustInformation: The server MUST refuse to service
                        this request if the server is not a trusted domain object in the root domain of the forest.
                        The status code in this case is STATUS_INVALID_DOMAIN_STATE.");

                    #endregion
                }
                else if (this.isDC && (this.domainState == ProtocolServerConfig.DomainController))
                {
                    #region MS-LSAD_R773

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.InvalidDomainRole,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        773,
                        @"In LsarSetForestTrustInformation, LsarSetForestTrustInformation request is valid only if 
                        the server is a primary domain controller. If the server is not a domain controller, 
                        the server MUST return STATUS_INVALID_DOMAIN_ROLE.");

                    #endregion
                }
                else if (stPolicyInformation.PHandle != handleInput)
                {
                    #region MS-LSAD_R771

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.InvalidHandle,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        771,
                        @"While implementing LsarSetForestTrustInformation: If the handle is not a valid context handle
                        to the policy object, the server MUST return STATUS_INVALID_HANDLE.");

                    #endregion
                }
                else if (isAccessDenied)
                {
                    #region MS-LSAD_R775

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.AccessDenied,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        775,
                        @"In LsarSetForestTrustInformation, The server MUST verify that the caller has 
                        TRUSTED_SET_AUTH access to the trusted domain object, and fail the request with 
                        STATUS_ACCESS_DENIED otherwise.");

                    #endregion

                    collisionInfo = CollisionInfo.Invalid;
                    return ErrorStatus.AccessDenied;
                }
                else if (trustObjectCreateinformation.uintTrustAttr != LsadUtilities.TRUST_ATTRIBUTE_FOREST_TRANSITIVE
                             && this.isDomainPresent)
                {
                    #region MS-LSAD_R776

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.InvalidParameter,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        776,
                        @"In LsarSetForestTrustInformation, The server MUST also make sure that the trust attributes
                        associated with the trusted domain object referenced by the TrustedDomainName parameter has
                        the TRUST_ATTRIBUTE_FOREST_TRANSITIVE set. If the attribute is not present, the server MUST
                        return STATUS_INVALID_PARAMETER.");

                    #endregion
                }
                else if (!this.isDomainPresent)
                {
                    #region MS-LSAD_R774

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.NoSuchDomain,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        774,
                        @"InLsarSetForestTrustInformation, If a trusted domain object by the name that is contained 
                        in the TrustedDomainName parameter does not exist, the server MUST return 
                        STATUS_NO_SUCH_DOMAIN.");

                    #endregion
                }
                else
                {
                    forestInformationThatWasSet.RecordCount = (uint)recordCount;
                    forestInformationThatWasSet.Entries = forestTrustInfo.Value.Entries;
                    _LSA_FOREST_TRUST_RECORD[][] entries = new _LSA_FOREST_TRUST_RECORD[1][];
                    entries[0] = new _LSA_FOREST_TRUST_RECORD[1];
                    _LSA_FOREST_TRUST_INFORMATION forestInformation_Query = new _LSA_FOREST_TRUST_INFORMATION();

                    this.uintStatus = lsadClientStack.LsarQueryForestTrustInformation(
                        PolicyHandle.Value,
                        domainName[0],
                        recordType,
                        out forestTrustInfo);

                    forestInformation_Query.RecordCount = 0x1;
                    forestInformation_Query.Entries = forestTrustInfo.Value.Entries;

                    //// Querying the information that we set to see if both are same, to be sure that set is success.
                    if (utilities.CheckForestInformation(forestInformationThatWasSet, forestInformation_Query))
                    {
                        #region MS-LSAD_R1009

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.Success,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            1009,
                            @"In LsarSetForestTrustInformation, The server MUST return STATUS_SUCCESS 
                            when the operation was completed successfully.");

                        #endregion

                        #region MS-LSAD_R1012

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.Success,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            1012,
                            @"In LsarQueryForestTrustInformation, The server MUST return STATUS_SUCCESS
                                        when the operation was completed successfully");

                        #endregion
                    }

                    this.isSetSuccess = true;
                    trustObjectCreateinformation.isForestInformationPresent = true;
                    collisionInfo = CollisionInfo.Valid;
                    return ErrorStatus.Success;
                }
            }

            collisionInfo = CollisionInfo.Invalid;
            return (ErrorStatus)uintMethodStatus;
        }

        #endregion

        #region LsarQueryForestTrustInformation

        /// <summary>
        ///  The QueryForestTrustInformation method is invoked to retrieve
        ///  forest information on a Trusted Domain Object.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="trustedDomainName">Contains the trusted domain name</param>
        /// <param name="name">It is for validating the trusted domain name whether it is valid or invalid</param>
        /// <param name="highestRecordType">Contains highest record type</param>
        /// <param name="recordCount">Contains record count</param>
        /// <param name="forestFuncLevel">Contains forest functional levels</param>
        /// <param name="isRootDomain">It is to check whether the current domain is a root domain</param>
        /// <param name="trustInfo">Out param which contains the trust information</param>
        /// <returns>Returns Success if the method is successful
        ///          Returns InvalidParameter if the parameters passed to the method are not valid
        ///          Returns AccessDenied if the caller does not have the permissions to 
        ///          perform this operation
        ///          Returns InvalidHandle if the passed in policy handle is not valid
        ///          Returns NoSuchDomain if the specified trusted domain object
        ///          does not exist
        ///          Returns InvalidDomainState if the operation cannot complete
        ///          in current state of the domain</returns>
        /// Disable warning CA1502 because it will affect the implementation of Adapter and Model codes 
        /// if do any changes about maintainability.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", 
            Justification = "affect the implementation of Adapter and Model codes")]
        public ErrorStatus QueryForestTrustInformation(
            int handleInput,
            string trustedDomainName,
            ValidString name,
            int highestRecordType,
            int recordCount,
            ForestFunctionalLevel forestFuncLevel,
            bool isRootDomain,
            out ForestTrustInfo trustInfo)
        {
            _RPC_UNICODE_STRING[] domainName = new _RPC_UNICODE_STRING[1];
            _LSA_FOREST_TRUST_INFORMATION[] forestTrustInformation = new _LSA_FOREST_TRUST_INFORMATION[1];
            _LSA_FOREST_TRUST_INFORMATION? forestTrustInfo = new _LSA_FOREST_TRUST_INFORMATION();
            _LSA_FOREST_TRUST_RECORD_TYPE recordType = _LSA_FOREST_TRUST_RECORD_TYPE.ForestTrustDomainInfo;
            invalidParamterCount = 0;
            PolicyHandle = validPolicyHandle;
            DomainType checkDomainName = DomainType.ValidDomainName;

            //// Checking if the Model received and invalid handle
            if (stPolicyInformation.PHandle != handleInput)
            {
                PolicyHandle = utilities.CreateAnInvalidHandle(false);
                invalidParamterCount++;
            }

            //// Checking if the Name of the TDO received by the model is already present
            if (trustObjectCreateinformation.strTdoDnsName.Equals(trustedDomainName))
            {
                checkDomainName = DomainType.ValidDomainName;
                this.isDomainPresent = true;
            }
            else
            {
                checkDomainName = DomainType.NoDomainName;
                this.isDomainPresent = false;
                invalidParamterCount++;
            }

            //// Initializing the Domain Name.
            utilities.GetTheDomainName(checkDomainName, ref domainName);

            if (name == ValidString.Valid)
            {
                domainName[0].Length = (ushort)(2 * domainName[0].Buffer.Length);
            }
            else
            {
                domainName[0].Length = (ushort)((2 * domainName[0].Buffer.Length) + 1);
                invalidParamterCount++;
            }

            domainName[0].MaximumLength = (ushort)(2 + domainName[0].Length);

            recordType = (_LSA_FOREST_TRUST_RECORD_TYPE)highestRecordType;

            if (this.isDomainPresent)
            {
                if (trustObjectCreateinformation.uintTrustAttr == LsadUtilities.TRUST_ATTRIBUTE_FOREST_TRANSITIVE)
                {
                    if (!trustObjectCreateinformation.isForestInformationPresent)
                    {
                        invalidParamterCount++;
                    }
                }
                else
                {
                    if (name == ValidString.Valid)
                    {
                        invalidParamterCount++;
                    }
                }
            }

            if ((this.isDC && !isRootDomain)
                     || (!this.isDC)
                     || (isWindows && forestFuncLevel != ForestFunctionalLevel.DS_BEHAVIOR_WIN2003))
            {
                invalidParamterCount++;
            }

            uintMethodStatus = lsadClientStack.LsarQueryForestTrustInformation(
                PolicyHandle.Value,
                domainName[0],
                recordType,
                out forestTrustInfo);

            //// The TD doesn't describe the protocol behavior when the opnum receives 2 or more invalid inputs
            //// To handle the condition, we return ErrorStatus.ErrorUnknown when there are 2 or more Invalid Inputs.
            if (invalidParamterCount > 1)
            {
                if (isWindows
                        && this.isDC
                        && forestFuncLevel != ForestFunctionalLevel.DS_BEHAVIOR_WIN2003)
                {
                    #region MS-LSAD_R1011

                    Site.CaptureRequirementIfIsTrue(
                        ((uint)ErrorStatus.InvalidDomainState == (uint)uintMethodStatus)
                              && (forestFuncLevel != ForestFunctionalLevel.DS_BEHAVIOR_WIN2003),
                        "MS-LSAD",
                        1011,
                        @"In LsarQueryForestTrustInformation, The server MUST refuse to service this request
                                    and STATUS_INVALID_DOMAIN_STATE MUST be returned by the server if the server is not 
                                    at DS_BEHAVIOR_WIN2003 forest functional level.");

                    #endregion
                }

                trustInfo = ForestTrustInfo.Invalid;

                return ErrorStatus.ErrorUnKnown;
            }

            forestTrustInformation[0].Entries = forestTrustInfo.HasValue ? forestTrustInfo.Value.Entries : null;
            forestTrustInformation[0].RecordCount = (uint)(forestTrustInfo.HasValue ? 0x1 : 0x0);

            if (name == ValidString.Invalid)
            {
                #region MS-LSAD_1010

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidParameter,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    1010,
                    @"In LsarQueryForestTrustInformation, The server MUST return
                    STATUS_INVALID_PARAMETER if any argument is invalid");

                #endregion
            }

            if (this.isDC)
            {
                if (stPolicyInformation.PHandle != handleInput)
                {
                    #region MS-LSAD_769

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.InvalidHandle,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        769,
                        @"In LsarQueryForestTrustInformation, If the handle is not a valid context handle to the 
                        policy object, the server MUST return STATUS_INVALID_HANDLE.");

                    #endregion
                }
                else
                {
                    if (!trustObjectCreateinformation.isForestInformationPresent && this.isDomainPresent)
                    {
                        if (trustObjectCreateinformation.uintTrustAttr != 
                            LsadUtilities.TRUST_ATTRIBUTE_FOREST_TRANSITIVE)
                        {
                            #region MS-LSAD_770

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.InvalidParameter,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                770,
                                @"In LsarQueryForestTrustInformation the out parameter ForestTrustInfo returns the 
                                foresttrust information associated with the trusted domain object. If the trusted 
                                domain object is not of the type that supports a forest trust (as determined by the 
                                presence or absenceof TRUST_ATTRIBUTE_FOREST_TRANSITIVE attribute), the server MUST 
                                return STATUS_INVALID_PARAMETER.");

                            #endregion

                            #region MS-LSAD_766

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.InvalidParameter,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                766,
                                @"In LsarQueryForestTrustInformation, If the trusted domain object is not of the 
                                type that supports forest trust, the server MUST return STATUS_INVALID_PARAMETER.");

                            #endregion
                        }
                        else
                        {
                            #region MS-LSAD_767

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.NotFound,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                767,
                                @"In LsarQueryForestTrustInformation, If the forest trust information does not 
                                exist on a trusted domain object which otherwise can support forest trust, the 
                                server MUST return STATUS_NOT_FOUND.");

                            #endregion
                        }
                    }
                    else
                    {
                        if ((!(forestFuncLevel == ForestFunctionalLevel.DS_BEHAVIOR_WIN2003) 
                            && isWindows) || !isRootDomain)
                        {
                            if (isWindows)
                            {
                                #region MS-LSAD_R1011

                                Site.CaptureRequirementIfIsTrue(
                                    ((uint)ErrorStatus.InvalidDomainState == (uint)uintMethodStatus)
                                         && (!(forestFuncLevel == ForestFunctionalLevel.DS_BEHAVIOR_WIN2003)),
                                    "MS-LSAD",
                                    1011,
                                    @"In LsarQueryForestTrustInformation, The server MUST refuse to service this request
                                    and STATUS_INVALID_DOMAIN_STATE MUST be returned by the server if the server is not 
                                    at DS_BEHAVIOR_WIN2003 forest functional level.");

                                #endregion
                            }
                        }
                        else if (!this.isDomainPresent)
                        {
                            #region MS-LSAD_765

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.NoSuchDomain,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                765,
                                @"In LsarQueryForestTrustInformation, If a trusted domain object by the name 
                                TrustedDomainName does not exist, the server MUST return STATUS_NO_SUCH_DOMAIN.");

                            #endregion
                        }
                        else
                        {
                            if (this.isSetSuccess)
                            {
                                if (utilities.CheckForestInformation(
                                    forestInformationThatWasSet,
                                    forestTrustInformation[0]))
                                {
                                    #region MS-LSAD_R1012

                                    Site.CaptureRequirementIfAreEqual<uint>(
                                        (uint)ErrorStatus.Success,
                                        (uint)uintMethodStatus,
                                        "MS-LSAD",
                                        1012,
                                        @"In LsarQueryForestTrustInformation, The server MUST return STATUS_SUCCESS
                                        when the operation was completed successfully");

                                    #endregion
                                    trustInfo = ForestTrustInfo.Valid;
                                    return ErrorStatus.Success;
                                }
                            }
                        }
                    }
                }
            }

            trustInfo = ForestTrustInfo.Invalid;
            return (ErrorStatus)uintMethodStatus;
        }

        #endregion

        #region LsarDeleteTrustedDomain

        /// <summary>
        ///  The DeleteTrustedDomain method is invoked to delete a trusted domain object.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2</param>
        /// <param name="trustedDomainSid">Contains the sid of trusted domain object</param>
        /// <param name="sid">It is for validating the passed in trusted domain sid 
        /// whether it is valid or invalid</param>
        /// <returns>Returns Success if the method is successful
        ///          Returns InvalidParameter if the parameters passed to the method are not valid
        ///          Returns AccessDenied if the caller does not have the permissions to 
        ///          perform this operation
        ///          Returns InvalidHandle if the passed in policy handle is not valid
        ///          Returns NoSuchDomain if the specified trusted domain does not exist</returns>
        /// Disable warning CA1502 because it will affect the implementation of Adapter and Model codes
        /// if do any changes about maintainability.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", 
            Justification = "affect the implementation of Adapter and Model codes")]
        public ErrorStatus DeleteTrustedDomain(
            int handleInput,
            string trustedDomainSid,
            DomainSid sid)
        {
            _RPC_SID[] trustSid = new _RPC_SID[1];
            _RPC_UNICODE_STRING tempRpcName = new _RPC_UNICODE_STRING();
            _RPC_UNICODE_STRING[] secretObjectNameToCheck = new _RPC_UNICODE_STRING[1];
            IntPtr? secretHandleToCheck = IntPtr.Zero;
            invalidParamterCount = 0;
            PolicyHandle = validPolicyHandle;

            //// Checking if the Model received and invalid handle
            if (stPolicyInformation.PHandle != handleInput)
            {
                PolicyHandle = utilities.CreateAnInvalidHandle(false);
                invalidParamterCount++;
            }

            //// Checking if the SID of the TDO received by the model is already present                      
            if (trustObjectCreateinformation.strDomainSid.Equals(trustedDomainSid))
            {
                trustSid = utilities.GetSid(sid, LsadManagedAdapter.ValidSid);
                this.isDomainPresent = true;
            }
            else
            {
                trustSid = utilities.GetSid(sid, LsadManagedAdapter.NoSid);
                this.isDomainPresent = false;
                invalidParamterCount++;
            }           

            if (sid == DomainSid.Invalid)
            {
                invalidParamterCount++;
            }

            if (!this.isDC)
            {
                invalidParamterCount++;
            }

            if (((trustObjectCreateinformation.uintTdoDesiredAccess & (uint)ACCESS_MASK.TRUSTED_QUERY_DOMAIN_NAME) ==
                               (uint)ACCESS_MASK.TRUSTED_QUERY_DOMAIN_NAME)
                      && ((trustObjectCreateinformation.uintTdoDesiredAccess & (uint)ACCESS_MASK.DELETE) 
                            == (uint)ACCESS_MASK.DELETE))
            {
                isAccessDenied = false;
            }
            else
            {
                isAccessDenied = true;
            }

            if (this.domainState == ProtocolServerConfig.ReadOnlyDomainController)
            {
                invalidParamterCount++;
            }

            utilities.GetTheDomainName(DomainType.SecretNameToCheck, ref secretObjectNameToCheck);
            secretObjectNameToCheck[0].Length = (ushort)(2 * secretObjectNameToCheck[0].Buffer.Length);
            secretObjectNameToCheck[0].MaximumLength = (ushort)(2 + secretObjectNameToCheck[0].Length);
            this.uintStatus = lsadClientStack.LsarOpenSecret(
                validPolicyHandle.Value,
                secretObjectNameToCheck[0],
                ACCESS_MASK.MAXIMUM_ALLOWED,
                out secretHandleToCheck);

            ////Check if Interdomain Trust Account exist and is valid
            bool isInterdomainTrustAccountExist = false;

            if (invalidParamterCount == 0)
            {
                isInterdomainTrustAccountExist = !this.CheckInterdomainTrustAccount();
            }

            Site.Log.Add(LogEntryKind.Comment, "-------Begin LsarDeletetrustDomain method-----");

            uintMethodStatus = lsadClientStack.LsarDeleteTrustedDomain(PolicyHandle.Value, trustSid[0]);

            ////Check if interdomain trust account is deleted along with the trusted domain
            if (invalidParamterCount == 0 && isInterdomainTrustAccountExist && !this.CheckInterdomainTrustAccount())
            {
                #region MS-LSAD_R1052

                // Verify requirement 1052
                Site.CaptureRequirement(
                    "MS-LSAD",
                    1052,
                    @"The server MUST also check whether an interdomain trust account with name 
                    ""<Trusted Domain NetBIOS Name>$"" exists.");

                #endregion

                #region MS-LSAD_R1053

                // Verify requirement 1053
                Site.CaptureRequirement(
                    "MS-LSAD",
                    1053,
                    @"If it[an interdomain trust account with name ""<Trusted Domain NetBIOS Name>$""] exists,
                    the server MUST delete that account along with the trusted domain.");

                #endregion
            }

            if (invalidParamterCount > 1 && this.domainState != ProtocolServerConfig.ReadOnlyDomainController)
            {
                if (!this.isDC)
                {
                    #region MS-LSAD_R1014

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.DirectoryServiceRequired,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        1014,
                        @"In LsarSetTrustedDomainInfo, the server MUST be in a Domain Controller 
                        and return DIRECTORY_SERVICE_REQUIRED if there is no active directory 
                        installed on it.");

                    #endregion
                }

                return ErrorStatus.ErrorUnKnown;
            }

            if (sid == DomainSid.Invalid)
            {
                #region MS-LSAD_R630

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidParameter,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    630,
                    @"In LsarDeleteTrustedDomain, The server MUST verify that the caller has supplied a valid domain
                    SID for TrustedDomainSid parameter, and fail the request with STATUS_INVALID_PARAMETER
                    if the check fails.");

                #endregion
            }
            else
            {
                if (!this.isDC)
                {
                    #region MS-LSAD_R1014

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.DirectoryServiceRequired,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        1014,
                        @"In LsarSetTrustedDomainInfo, the server MUST be in a Domain Controller 
                        and return DIRECTORY_SERVICE_REQUIRED if there is no active directory 
                        installed on it.");

                    #endregion
                }
                else if (this.domainState == ProtocolServerConfig.ReadOnlyDomainController)
                {
                    #region MS-LSAD_R632

                    Site.CaptureRequirementIfIsTrue(
                        ((uint)uintMethodStatus != (uint)ErrorStatus.Success),
                        "MS-LSAD",
                        632,
                        @"In LsarDeleteTrustedDomain, If the server is a read-only domain controller, 
                        then the server MUST return an error.<78>");

                    #endregion

                    if (isWindows && PDCOSVersion >= ServerVersion.Win2008)
                    {
                        return ErrorStatus.NotSupported;
                    }
                }
                else
                {
                    if (stPolicyInformation.PHandle != handleInput)
                    {
                        #region MS-LSAD_R631

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.InvalidHandle,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            631,
                            @"In LsarDeleteTrustedDomain, The server MUST return STATUS_INVALID_HANDLE 
                            when PolicyHandle is not a valid handle.");

                        #endregion
                    }
                    else
                    {
                        if (isAccessDenied && this.isitSetTrustedDomainInfo && this.isDomainPresent)
                        {
                            return ErrorStatus.AccessDenied;
                        }

                        if (!this.isDomainPresent)
                        {
                            #region MS-LSAD_R629

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.NoSuchDomain,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                629,
                                @"In LsarDeleteTrustedDomain,The server MUST verify that a trusted domain object with
                                TrustedDomainSid exists in its policy database and fail the request with 
                                STATUS_NO_SUCH_DOMAIN otherwise.");

                            #endregion

                            #region MS-LSAD_R634

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.NoSuchDomain,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                634,
                                @"In LsarDeleteTrustedDomain  in the processing of TrustedDomainSid, for a trusted
                                domain object to be deleted,the server MUST verify that a trusted domain object with
                                the specified TrustedDomainSid exists in its policy database and fail the request
                                with STATUS_NO_SUCH_DOMAIN otherwise.");

                            #endregion
                        }
                        else
                        {
                            if (!utilities.CheckTheDomain(trustSid[0], tempRpcName, validPolicyHandle.Value))
                            {
                                #region MS-LSAD_R627

                                Site.CaptureRequirementIfAreEqual<uint>(
                                    (uint)ErrorStatus.Success,
                                    (uint)uintMethodStatus,
                                    "MS-LSAD",
                                    627,
                                    @"In LsarDeleteTrustedDomain , The server MUST return STATUS_SUCCESS 
                                    when The request was successfully completed.");

                                #endregion
                            }                           

                            if ((uint)this.uintStatus == (uint)ErrorStatus.ObjectNameNotFound)
                            {
                                #region MS-LSAD_R637

                                Site.CaptureRequirementIfAreEqual<uint>(
                                    (uint)ErrorStatus.ObjectNameNotFound,
                                    (uint)this.uintStatus,
                                    "MS-LSAD",
                                    637,
                                    @"In LsarDeleteTrustedDomain in the processing of TrustedDomainSid, 
                                    If a secret with name G$$<Trusted Domain Name> exists, the server MUST 
                                    delete that secret along with the trusted domain.");

                                #endregion
                            }

                            this.TurstDomainFlag = false;
                        }
                    }
                }
            }

            if (((uint)uintMethodStatus != (uint)ErrorStatus.Success) 
                && (sid == DomainSid.Valid) && this.isDomainPresent)
            {
                this.uintStatus = lsadClientStack.LsarClose(ref this.validTrustHandle);
                trustSid = utilities.GetSid(DomainSid.Valid, LsadManagedAdapter.ValidSid);
                this.uintStatus = lsadClientStack.LsarDeleteTrustedDomain(validPolicyHandle.Value, trustSid[0]);
            }

            if (PolicyHandle != IntPtr.Zero)
            {
                this.uintStatus = lsadClientStack.LsarClose(ref PolicyHandle);
            }

            isAccessDenied = false;

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion

        #region LsarEnumerateTrustedDomainEx

        /// <summary>
        /// The EnumerateTrustedDomainsExRequest method is invoked to enumerate the request from trust domain.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2</param>
        /// <param name="enumerationContext"> A pointer to a context value that is used to resume
        /// enumeration, if necessary.</param>
        public void EnumerateTrustedDomainsExRequest(
            int handleInput,
            int enumerationContext)
        {
            _LSAPR_TRUSTED_ENUM_BUFFER_EX? EnumerationBuf = new _LSAPR_TRUSTED_ENUM_BUFFER_EX();
            IntPtr objInvalidHandle = IntPtr.Zero;
            PolicyHandle = closePolicyHandle;
            IntPtr? tempPolicyHandle = IntPtr.Zero;
            uint? enumContext = 0;
            uint? tempEnumContext = 0;
            uint preferredLength = 1;
            int numOfTDOs = 0;
            const bool DELETE_THE_TDOS = true, CREATE_SOME_TDOS_FOR_ENUMERATE = false;

            utilities.CreateTrustedDomainsForEnumerate(CREATE_SOME_TDOS_FOR_ENUMERATE);

            // We Enumerate first to get the Total Number of Trusted Domain Objects on the server.
            // Since we dont know the number of objects on the server, we first enumerate to get the number.
            // We do it in the following way : When there are no more objects on the server to enumerate, the server
            // sets the EnumerationContext value so that the next enumeration is not valid. 
            // So we keep calling LsarEnumerateTrustedDomainsEx until the EnumerationContext value in successive calls
            // is same. Then we get the Number of Trusted Domain Objects on the server.
            #region Getting the No.Of Trusted Domain Objects on the Server

            ushort[] strServerNameTemp = new ushort[] { 'n', 'a', 'm', 'e' };
            uintMethodStatus = lsadClientStack.LsarOpenPolicy(
                strServerNameTemp,
                objectAttributes,
                ACCESS_MASK.MAXIMUM_ALLOWED,
                out tempPolicyHandle);

            for (numOfTDOs = 0; ;numOfTDOs++)
            {
                enumContext = tempEnumContext.Value;
                uintMethodStatus = lsadClientStack.LsarEnumerateTrustedDomainsEx(
                    tempPolicyHandle.Value,
                    ref tempEnumContext,
                    out EnumerationBuf,
                    preferredLength);

                if (tempEnumContext == enumContext)
                {
                    break;
                }
            }

            uintMethodStatus = lsadClientStack.LsarClose(ref tempPolicyHandle);

            #endregion Getting the No.Of Trusted Domain Objects on the Server

            if (stPolicyInformation.PHandle != handleInput)
            {
                objInvalidHandle = utilities.AccountObjInvalidHandle().Value;
                PolicyHandle = objInvalidHandle;
            }

            enumContext = 0;
            int i = 0;

            // We cannot control the program to simulate the return values STATUS_MORE_ENTRIES, STATUS_SUCCESS, 
            // STATUS_NO_MORE_ENTRIES Since in Trusted Domain Objects, the server seems to ignore the 
            // EnumerationContext value and it is  enumerating from the starting object and not from the 
            // EnumerationContext offset. Hence what we are now doing is enumerating enumerationContext 
            // times(value comes from the cord) and checking the valuewith the number of Trusted Domain Objects 
            // on the server. If the EnumerationContext passed through cord is less thanthe number of Objects, 
            // server MUST return STATUS_MORE_ENTRIES, if it is more than the Number Of Objects, server MUST 
            // return  STATUS_NO_MORE_ENTRIES, and if it equal to the number of TDOs, server returns STATUS_SUCCESS.
            do
            {
                uintMethodStatus = lsadClientStack.LsarEnumerateTrustedDomainsEx(
                    PolicyHandle.Value,
                    ref enumContext,
                    out EnumerationBuf,
                    preferredLength);
                
                i++;
            } 
            while (i < enumerationContext);

            utilities.CreateTrustedDomainsForEnumerate(DELETE_THE_TDOS);

            if (stPolicyInformation.PHandle != handleInput)
            {
                EnumerateTrustedDomainsEx(handleInput, enumerateResponse.EnumerateNone);

                #region MS-LSAD_R654

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidHandle,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    654,
                    @"In LsarEnumerateTrustedDomainsEx, The server MUST return STATUS_INVALID_HANDLE 
                    when the PolicyHandle is not a valid handle.");

                #endregion
            }
            else if ((uintAccessMask & ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION) != 
                        ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION)
            {
                EnumerateTrustedDomainsEx(handleInput, enumerateResponse.EnumerateNone);

                #region MS-LSAD_R655

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.AccessDenied,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    655,
                    @"In LsarEnumerateTrustedDomainsEx in the processing of PolicyHandle, The server MUST
                    verify that the handle has POLICY_VIEW_LOCAL_INFORMATION access and fail the request 
                    with STATUS_ACCESS_DENIED if this is not the case.");

                #endregion
            }
            else if (enumerationContext > numOfTDOs)
            {
                EnumerateTrustedDomainsEx(handleInput, enumerateResponse.EnumerateNone);

                #region MS-LSAD_R653

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.NoMoreEntries,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    653,
                    @"In LsarEnumerateTrustedDomainsEx, If the enumeration is finished and there are no more 
                    entries to be returned, the server MUST return the status code STATUS_NO_MORE_ENTRIES and 
                    set EnumerationContext to a value that indicates that the enumeration has been finished");               

                #endregion
            }
            else if (enumerationContext == numOfTDOs)
            {
                EnumerateTrustedDomainsEx(handleInput, enumerateResponse.EnumerateAll);

                #region MS-LSAD_R661

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    661,
                    @"<81> Section 3.1.4.7.8:The value returned in the PreferedMaximumLength field of the Windows 
                    implementation of LsarEnumerateTrustedDomains method might exceed the maximum desired length 
                    specified by the caller.");                
                
                #endregion

                #region MS-LSAD_R659

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    659,
                    @"In LsarEnumerateTrustedDomainsEx  after the processing of the method by the server, 
                    EnumerationBuffer is used to return the results of enumeration. It will contain only 
                    as many entries as were enumerated on this call.");               

                #endregion

                #region MS-LSAD_R650

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    650,
                    @"In LsarEnumerateTrustedDomainsEx, The server MUST return STATUS_SUCCESS when 
                    the request was successfully completed.");              

                #endregion

                #region MS-LSAD_R84
                if (EnumerationBuf.Value.EntriesRead != 0)
                {
                    Site.CaptureRequirementIfIsNotNull(
                        EnumerationBuf.Value.EnumerationBuffer,
                        "MS-LSAD",
                        84,
                        @"In LSAPR_TRUSTED_ENUM_BUFFER_EX structure under field 'EnumerationBuffer' : If the
                        EntriesRead field has a value other than 0, EnumerationBuffer field MUST NOT be NULL.");
                 }
                #endregion MS-LSAD_R84
            }
            else if (enumerationContext < numOfTDOs)
            {
                EnumerateTrustedDomainsEx(handleInput, enumerateResponse.EnumerateSome);

                #region MS-LSAD_R652

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.MoreEntries,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    652,
                    @"In LsarEnumerateTrustedDomainsEx, If the server decides not to return an entire set of trusted
                    domain objects known to it when this method is invoked, it MUST set the EnumerationContext value 
                    to a value that it will later use to resume enumeration, and return the status code 
                    STATUS_MORE_ENTRIES.");

                #endregion
            }
        }

        #endregion

        #region LsarEnumerateTrustedDomains

        /// <summary>
        /// The EnumerateTrustedDomainsRequest method is invoked to enumerate the request from trust domain.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2</param>
        /// <param name="enumerationContext"> A pointer to a context value that is used to resume
        /// enumeration, if necessary.</param>
        public void EnumerateTrustedDomainsRequest(int handleInput, int enumerationContext)
        {
            _LSAPR_TRUSTED_ENUM_BUFFER? EnumerationBuf = new _LSAPR_TRUSTED_ENUM_BUFFER();
            IntPtr objInvalidHandle = IntPtr.Zero;
            PolicyHandle = closePolicyHandle;
            IntPtr? tempPolicyHandle = IntPtr.Zero;
            uint? enumContext = 0;
            uint? tempEnumContext = 0;
            uint preferredLength = 0;
            int numOfTDOs = 0;
            const bool DELETE_THE_TDOS = true;
            const bool CREATE_SOME_TDOS_FOR_ENUMERATE = false;

            utilities.CreateTrustedDomainsForEnumerate(CREATE_SOME_TDOS_FOR_ENUMERATE);

            if (stPolicyInformation.PHandle != handleInput)
            {
                objInvalidHandle = utilities.AccountObjInvalidHandle().Value;
                PolicyHandle = objInvalidHandle;
            }

            // We Enumerate first to get the Total Number of Trusted Domain Objects on the server.
            // Since we dont know the number of objects on the server, we first enumerate to get the number.
            // We do it in the following way : When there are no more objects on the server to enumerate, the server
            // sets the EnumerationContext value so that the next enumeration is not valid. 
            // So we keep calling LsarEnumerateTrustedDomainsEx until the EnumerationContext value in successive calls
            // is same. Then we get the Number of Trusted Domain Objects on the server.
            #region Getting the No.Of Trusted Domain Objects on the Server

            ushort[] strServerNameTemp = new ushort[] { 'n', 'a', 'm', 'e' };
            uintMethodStatus = lsadClientStack.LsarOpenPolicy(
                strServerNameTemp,
                objectAttributes,
                ACCESS_MASK.MAXIMUM_ALLOWED,
                out tempPolicyHandle);

            for (numOfTDOs = 0; ;numOfTDOs++)
            {
                enumContext = tempEnumContext;
                uintMethodStatus = lsadClientStack.LsarEnumerateTrustedDomains(
                    tempPolicyHandle.Value,
                    ref tempEnumContext,
                    out EnumerationBuf,
                    preferredLength);

                if (tempEnumContext == enumContext)
                {
                    break;
                }
            }

            #endregion Getting the No.Of Trusted Domain Objects on the Server

            enumContext = 0;
            int i = 0;

            // We cannot control the program to simulate the return values STATUS_MORE_ENTRIES, STATUS_SUCCESS, 
            // STATUS_NO_MORE_ENTRIES Since in Trusted Domain Objects, the server seems to ignore the
            // EnumerationContext value and it is enumerating from the starting object and not from the 
            // EnumerationContext offset. Hence what we are now doing is enumerating enumerationContext 
            // times(value comes from the cord) and checking the value with the number of Trusted Domain Objects
            // on the server. If the EnumerationContext passed through cord is less than the number of Objects,
            // server MUST return STATUS_MORE_ENTRIES, if it is more than the Number Of Objects, server MUST 
            // return STATUS_NO_MORE_ENTRIES, and if it equal to the number of TDOs, server returns STATUS_SUCCESS.
            do
            {
                uintMethodStatus = lsadClientStack.LsarEnumerateTrustedDomains(
                    PolicyHandle.Value,
                    ref enumContext,
                    out EnumerationBuf,
                    preferredLength);
                
                i++;
            } 
            while (i < enumerationContext);

            utilities.CreateTrustedDomainsForEnumerate(DELETE_THE_TDOS);

            if (stPolicyInformation.PHandle != handleInput)
            {
                EnumerateTrustedDomains(handleInput, enumerateResponse.EnumerateNone);

                #region MS-LSAD_R665

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidHandle,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    665,
                    @"In LsarEnumerateTrustedDomains, The server MUST return STATUS_INVALID_HANDLE
                    when PolicyHandle in not a valid handle");

                #endregion
            }
            else if ((uintAccessMask & ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION) != 
                        ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION)
            {
                EnumerateTrustedDomains(handleInput, enumerateResponse.EnumerateNone);

                #region MS-LSAD_R666

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.AccessDenied,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    666,
                    @"In LsarEnumerateTrustedDomains  in the processing of PolicyHandle, The server MUST verify
                    that the handle has POLICY_VIEW_LOCAL_INFORMATION access and fail the request with 
                    STATUS_ACCESS_DENIED if this is not the case.");

                #endregion
            }
            else if (enumerationContext > numOfTDOs)
            {
                EnumerateTrustedDomains(handleInput, enumerateResponse.EnumerateNone);

                #region MS-LSAD_R669

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.NoMoreEntries,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    669,
                    @"In LsarEnumerateTrustedDomains  in the processing of EnumerationContext, If the enumeration 
                    is finished and there are no more entries to be returned, the server MUST return the status
                    code STATUS_NO_MORE_ENTRIES, and set EnumerationContext to a number such that enumeration would
                    not continue if the method was called again with that value of EnumerationContext.");
               
                #endregion

                #region MS-LSAD_R670

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.NoMoreEntries,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    670,
                    @"In LsarEnumerateTrustedDomains  in the processing of EnumerationContext, If the EnumerationContext
                    supplied by the caller is such that enumeration cannot continue, the server MUST return 
                    STATUS_NO_MORE_ENTRIES.");
                
                #endregion

                #region MS-LSAD_R664

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.NoMoreEntries,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    664,
                    @"In LsarEnumerateTrustedDomains, The server MUST return STATUS_NO_MORE_ENTRIES when no more entries
                    are available from the enumeration.");
               
                #endregion
            }
            else if (enumerationContext == numOfTDOs)
            {
                EnumerateTrustedDomains(handleInput, enumerateResponse.EnumerateAll);

                #region MS-LSAD_R662

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    662,
                    @"In LsarEnumerateTrustedDomainsEx, The server MUST return STATUS_SUCCESS when 
                    the request was successfully completed.");                

                #endregion
            }
            else if (enumerationContext < numOfTDOs)
            {
                EnumerateTrustedDomains(handleInput, enumerateResponse.EnumerateSome);

                #region MS-LSAD_R663

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.MoreEntries,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    663,
                    @"In LsarEnumerateTrustedDomains, The server MUST return STATUS_MORE_ENTRIES when more information
                    is available to successive calls.");

                #endregion
            }
        }

        #endregion

        #endregion

        #region Check Interdomain Trust Account

        /// <summary>
        /// Check if the interdomain trust account exists
        /// </summary>
        /// <returns>if exist, return true; otherwise, return false.</returns>
        bool CheckInterdomainTrustAccount()
        {
            ////Connect to SUT through LDAP
            LdapConnection connection = new LdapConnection(fullDomain);
            string domainNC = "DC=" + fullDomain.Replace(".", ",DC=");
            string distinguishedName = "CN=" + ValidDomainName +
                "$,CN=Users," + domainNC;

            SearchRequest searchRequest = new SearchRequest(
                distinguishedName,
                "objectclass=user",
                System.DirectoryServices.Protocols.SearchScope.Base,
                new string[] { "cn", "sAMAccountName", "sAMAccountType", "userAccountControl", "unicodePwd" });

            try
            {
                ////Search if the specified Interdomain Trust Account exists
                SearchResponse searchResponse = (SearchResponse)connection.SendRequest(searchRequest);

                ////Get some attributes of Interdomain Trust Account and check if the values are set as specified
                ////in [MS-ADTS] section 7.1.6.8 
                string strsAMAccountName = searchResponse.Entries[0].Attributes["sAMAccountName"][0].ToString();

                if (strsAMAccountName.CompareTo(ValidDomainName + "$") != 0)
                {
                    return false;
                }

                string strsAMAccountType = searchResponse.Entries[0].Attributes["sAMAccountType"][0].ToString();

                if ((Convert.ToInt32(strsAMAccountType) & 0x30000002) != 0x30000002)
                {
                    return false;
                }

                string userAccountControl = searchResponse.Entries[0].Attributes["userAccountControl"][0].ToString();

                if ((Convert.ToInt32(userAccountControl) & 0x00000800) != 0x00000800)
                {
                    return false;
                }

                string cn = searchResponse.Entries[0].Attributes["cn"][0].ToString();

                if (cn.CompareTo(ValidDomainName + "$") != 0)
                {
                    return false;
                }
            }
            catch (DirectoryOperationException exp)
            {
                Site.Log.Add(LogEntryKind.Comment, "Throw exception: {0}", exp.ToString());
                return false;
            }

            return true;
        }

        #endregion
    }
}