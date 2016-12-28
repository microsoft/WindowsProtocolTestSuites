// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad
{
    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.Modeling;
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk;
    using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa;
    using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
    using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

    /// <summary>
    /// Implement methods of interface ILsadManagedAdapter.
    /// </summary>
    /// Disable warning CA1506 because it will affect the implementation of Adapter and Model codes 
    /// if do any changes about maintainability.
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling",
            Justification = "Disable warning CA1506 because it will affect the implementation of Adapter and Model")]
    public partial class LsadManagedAdapter
    {
        #region Accounts&Common Variables

        /// <summary>
        /// Variables used in Accounts and Common objects
        /// </summary>
        private Hashtable htAccHandle = new Hashtable();

        /// <summary>
        /// Variables used in Accounts and Common objects
        /// </summary>
        private Hashtable htAddAccRight = new Hashtable();

        /// <summary>
        /// Variables used in Accounts and Common objects
        /// </summary>
        private const uint uintHandle = 0;

        /// <summary>
        /// Variables used in Accounts and Common objects
        /// </summary>
        private const uint uintEntries = 0;

        /// <summary>
        /// Variables used in Accounts and Common objects
        /// </summary>
        private const uint uintIndex = 0;

        /// <summary>
        /// Variables used in Accounts and Common objects
        /// </summary>
        private NtStatus uintMethodStatus = 0;

        /// <summary>
        /// Variables used in Accounts and Common objects
        /// </summary>
        private ACCESS_MASK uintAccess = 0;

        /// <summary>
        /// Variables used in Accounts and Common objects
        /// </summary>
        private ACCESS_MASK uintOpenAccAccess = 0;

        /// <summary>
        /// Variables used in Accounts and Common objects
        /// </summary>
        private ACCESS_MASK uintDesrAccess = 0;

        /// <summary>
        /// Variables used in Accounts and Common objects
        /// </summary>
        private uint uintSecurityInfo;

        /// <summary>
        /// Variables used in Accounts and Common objects
        /// </summary>
        private ulong uintSystemAccess;

        /// <summary>
        /// Variables used in Accounts and Common objects
        /// </summary>
        private bool invalidParamCheck;

        /// <summary>
        /// Variables used in Accounts and Common objects
        /// </summary>
        private bool accessDeniedCheck;

        /// <summary>
        /// Variables used in Accounts and Common objects
        /// </summary>
        private bool invalidHandleCheck;

        /// <summary>
        /// Variables used in Accounts and Common objects
        /// </summary>
        private bool closeCheck;

        /// <summary>
        /// Variables used in Accounts and Common objects
        /// </summary>
        private bool checkTrustHandle;

        /// <summary>
        /// Variables used in Accounts and Common objects
        /// </summary>
        private IntPtr? closePolicyHandle = IntPtr.Zero;

        /// <summary>
        /// Variables used in Accounts and Common objects
        /// </summary>
        private IntPtr? objPolicyHandle = IntPtr.Zero;

        /// <summary>
        /// Variables used in Accounts and Common objects
        /// </summary>
        private IntPtr? objAccountHandle = IntPtr.Zero;

        /// <summary>
        /// Variables used in Accounts and Common objects
        /// </summary>
        private IntPtr? validAccountHandle = IntPtr.Zero;

        /// <summary>
        /// Variables used in Accounts and Common objects
        /// </summary>
        private _RPC_SID[] objAccountSid = new _RPC_SID[1];

        #endregion Accounts&Common Variables

        #region Account_Objects

        #region LsarCreateAccount

        /// <summary>
        /// The CreateAccount method is invoked to create a new account object in the server's database.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="desiredAccess">Contains the access to be given to the account Handle</param>
        /// <param name="sid">It is to check that passed in account sid is valid</param>
        /// <param name="accountSid">Contains the account sid to be created</param>
        /// <param name="accountHandle">Outparam which contains valid or invalid account handle</param>
        /// <returns>Result of create account</returns>
        public ErrorStatus CreateAccount(
            int handleInput,
            uint desiredAccess,
            AccountSid sid,
            string accountSid,
            out Handle accountHandle)
        {
            utilities.DeleteUnknownSID();

            this.closeCheck = true;
            this.invalidParamCheck = false;
            this.accessDeniedCheck = false;
            this.invalidHandleCheck = false;

            this.objAccountSid = utilities.SID(TypeOfSID.NewSID);
            utilities.AccountHandleCheck(ref this.objAccountSid);
            this.uintOpenAccAccess = (ACCESS_MASK)desiredAccess;
            this.objAccountSid[uintIndex].Revision = (sid == AccountSid.Valid) ? (byte)0x01 : (byte)0x00;

            if (stPolicyInformation.PHandle != handleInput)
            {
                this.objPolicyHandle = utilities.AccountObjInvalidHandle();
            }

            this.uintMethodStatus = lsadClientStack.LsarOpenAccount(
                this.objPolicyHandle.Value,
                this.objAccountSid[0],
                uintAccessMask,
                out this.objAccountHandle);

            if (this.uintMethodStatus == NtStatus.STATUS_OBJECT_NAME_COLLISION)
            {
                this.objAccountSid = utilities.SID(TypeOfSID.UnKnownSID);
            }

            this.uintMethodStatus = lsadClientStack.LsarCreateAccount(
                this.objPolicyHandle.Value,
                this.objAccountSid[0],
                this.uintOpenAccAccess,
                out this.objAccountHandle);

            if (this.objAccountHandle != IntPtr.Zero)
            {
                this.htAccHandle.Add("AccHandle", this.objAccountHandle);
                this.closePolicyHandle = this.objAccountHandle.Value;
                this.validAccountHandle = this.objAccountHandle;
            }

            accountHandle = (IntPtr.Zero == this.objAccountHandle) ? Handle.Invalid : Handle.Valid;

            if (sid == AccountSid.Invalid)
            {
                #region MS-LSAD_R435

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidParameter,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    435,
                    @"In Accountsid parameter  in LsarCreateAccount method , server MUST validate that AccountSid 
                    represents a valid SID, and fail the request with STATUS_INVALID_PARAMETER if it is not.");

                #endregion
            }
            else if (stPolicyInformation.PHandle != handleInput)
            {
                #region MS-LSAD_R869

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidHandle,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    869,
                    @"In LsarCreateAccount method ,if PolicyHandle is not a valid context handle to Policy Object,
                    server MUST return STATUS_INVALID_HANDLE.");

                #endregion
            }
            else if ((this.uintAccess & ACCESS_MASK.POLICY_CREATE_ACCOUNT) != ACCESS_MASK.POLICY_CREATE_ACCOUNT)
            {
                #region MS-LSAD_R434

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.AccessDenied,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    434,
                    @"In PolicyHandle parameter  in LsarCreateAccount method ,server MUST verify that the 
                    caller has POLICY_CREATE_ACCOUNT rights, and fail the request with STATUS_ACCESS_DENIED 
                    if it does not.");

                #endregion
            }
            else
            {
                #region MS-LSAD_R440

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    440,
                    @"In LsarCreateAccount method,if request is succesfully completed then server MUST 
                    return STATUS_SUCCESS.");

                #endregion

                if ((uint)ErrorStatus.Success == (uint)this.uintMethodStatus)
                {
                    #region MS-LSAD_R46

                    Site.CaptureRequirementIfIsNotNull(
                        this.objAccountSid,
                        "MS-LSAD",
                        46,
                        @"The Sid field of the LSAPR_ACCOUNT_INFORMATION structure in LSAD protocol MUST NOT be NULL.");

                    #endregion
                }

                #region MS-LSAD_R439

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    439,
                    @"In LsarCreateAccount method ,server MUST associate a security descriptor with a newly 
                    created account object.");

                #endregion

                if (isWindows)
                {
                    #region MS-LSAD_R436

                    if (sid == AccountSid.Valid)
                    {
                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.Success,
                            (uint)this.uintMethodStatus,
                            "MS-LSAD",
                            436,
                            @"[In  Accountsid parameter  in LsarCreateAccount method] <56> Section 3.1.4.5.1: 
                            Windows does not validate the structure of the SID beyond whether it is valid.");
                    }

                    #endregion
                }
            }

            return (ErrorStatus)this.uintMethodStatus;
        }

        #endregion LsarCreateAccount

        #region LsarOpenAccount

        /// <summary>
        /// The OpenAccount method is invoked to obtain a handle to an account object.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="securityDescrAllows">Specify if it allows the security Description</param>
        /// <param name="sid">It is to check that passed in account sid is valid</param>
        /// <param name="accountSid">Contains the account sid to be opened</param>
        /// <param name="accountHandle">Outparam which contains valid or invalid account handle</param>
        /// <returns>Result of open account</returns>
        public ErrorStatus OpenAccount(
            int handleInput,
            bool securityDescrAllows,
            AccountSid sid,
            string accountSid,
            out Handle accountHandle)
        {
            bool invalidcheck = false;
            bool unknownSidCheck = false;
            uintAccessMask = ACCESS_MASK.NONE;
            this.objAccountSid[uintIndex].Revision = (sid == AccountSid.Valid) ? (byte)0x01 : (byte)0x00;

            if ((stPolicyInformation.PHandle != handleInput
                     || this.htAccHandle.Count == uintHandle)
                 && stPolicyInformation.PHandle != handleInput)
            {
                this.objPolicyHandle = utilities.AccountObjInvalidHandle();

                this.uintMethodStatus = lsadClientStack.LsarOpenAccount(
                    this.objPolicyHandle.Value,
                    this.objAccountSid[0],
                    uintAccessMask,
                    out this.objAccountHandle);

                invalidcheck = true;
            }
            else if (!securityDescrAllows)
            {
                uintAccessMask = ACCESS_MASK.POLICY_LOOKUP_NAMES;
            }

            if (stPolicyInformation.PHandle == handleInput
                    && securityDescrAllows
                    && sid == AccountSid.Valid
                    && this.htAccHandle.Count == uintHandle)
            {
                ////Retrieves unknown UnKnownSID values from ptfconfig file.
                this.objAccountSid = utilities.SID(TypeOfSID.UnKnownSID);
                unknownSidCheck = true;
            }

            if (!invalidcheck || unknownSidCheck)
            {
                if (!securityDescrAllows)
                {
                    ////Retrieves exisiting SIDCount values from ptfconfig file.
                    this.objAccountSid = utilities.SID(TypeOfSID.ExistSID);
                    lsadClientStack.LsarOpenPolicy(
                        serverName, 
                        objectAttributes, 
                        ACCESS_MASK.MAXIMUM_ALLOWED,
                        out this.objPolicyHandle);

                    this.uintMethodStatus = lsadClientStack.LsarCreateAccount(
                            this.objPolicyHandle.Value,
                            this.objAccountSid[0],
                            this.uintOpenAccAccess,
                            out this.objAccountHandle);

                    lsadClientStack.LsarClose(ref this.objPolicyHandle);
                }

                lsadClientStack.LsarOpenPolicy(serverName, objectAttributes, uintAccessMask, out this.objPolicyHandle);

                this.uintMethodStatus = lsadClientStack.LsarOpenAccount(
                    this.objPolicyHandle.Value,
                    this.objAccountSid[0],
                    uintAccessMask,
                    out this.objAccountHandle);

                lsadClientStack.LsarClose(ref this.objPolicyHandle);
            }

            accountHandle = (IntPtr.Zero == this.objAccountHandle) ? Handle.Invalid : Handle.Valid;

            if (sid == AccountSid.Invalid)
            {
                #region MS-LSAD_R457

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidParameter,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    457,
                    @"In AccountSid parameter of LsarOpenAccount method ,the server MUST verify that the 
                    SID is valid or AccessMask is NULL and fail the request with STATUS_INVALID_PARAMETER otherwise.");

                #endregion
            }
            else if ((stPolicyInformation.PHandle != handleInput
                          || this.htAccHandle.Count == uintHandle)
                      && stPolicyInformation.PHandle != handleInput
                      && unknownSidCheck == false)
            {
                #region MS-LSAD_R454

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidHandle,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    454,
                    @"In  LsarOpenAccount method ,if PolicyHandle is not a valid context handle to Policy Object,
                    server MUST return STATUS_INVALID_HANDLE.");

                #endregion
            }
            else if (!securityDescrAllows)
            {
                #region MS-LSAD_R460

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.AccessDenied,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    460,
                    @"In DesiredAccess parameter of LsarOpenAccount method,the server MUST verify that the account 
                    object's security descriptor (LSAPR_SR_SECURITY_DESCRIPTOR) allows the requested access and, 
                    failing that, returns STATUS_ACCESS_DENIED.");

                #endregion
            }
            else if (stPolicyInformation.PHandle == handleInput
                         && securityDescrAllows
                         && sid == AccountSid.Valid
                         && this.htAccHandle.Count == uintHandle)
            {
                #region MS-LSAD_R458

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.ObjectNameNotFound,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    458,
                    @"In AccountSid parameter of LsarOpenAccount method ,the server MUST verify that the account 
                    object with specified AccountSid exists in its policy database and fail the request with 
                    STATUS_OBJECT_NAME_NOT_FOUND otherwise.");

                #endregion
            }
            else
            {
                this.validAccountHandle = this.objAccountHandle;

                #region MS-LSAD_R461

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    461,
                    @"In LsarOpenAccount method,if request is succesfully completed then server MUST 
                    return STATUS_SUCCESS.");

                #endregion
            }

            invalidcheck = false;
            unknownSidCheck = false;

            return (ErrorStatus)this.uintMethodStatus;
        }

        #endregion LsarOpenAccount

        #region LsarEnumeratePrivilegesAccount

        /// <summary>
        /// The EnumeratePrivilegesAccount method is invoked to retrieve a list of privileges 
        /// granted to an account on the server.
        /// </summary>
        /// <param name="handleInput">Contains account handle obtained from CreateAccount/OpenAccount </param>
        /// <param name="privileges">Outparam which contains valid or invalid 
        /// privileges enumerated from an account object</param>
        /// <returns>Returns Success if the method is successful;          
        /// Returns AccessDenied if the caller does not have the permissions to perform this operation;     
        /// Returns InvalidHandle if the passed in account handle is not valid.</returns>
        /// Disable warning CA1502 because it will affect the implementation of Adapter and Model codes 
        /// if do any changes about maintainability.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity",
            Justification = "Disable warning CA1502 because it will affect the implementation of Adapter and Model")]
        public ErrorStatus EnumeratePrivilegesAccount(int handleInput, out Set<AccountPrivilege> privileges)
        {
            this.objAccountHandle = this.validAccountHandle;
            _LSAPR_PRIVILEGE_SET? privilegeSet = new _LSAPR_PRIVILEGE_SET?();
            const uint Attribute_Disabled = 0x00000001;
            const uint Attribute_Enabled = 0x00000002;
            const uint Luid_HighPart = 0;
            const uint Luid_LowPart = 35;

            //if (Site.Properties.Get("Win2K8PrivCheck").ToLower() == Convert.ToString(Server.Windows2k8).ToLower())
            if (PDCOSVersion >= ServerVersion.Win2008)
            {
                #region Passing ValidHandle

                ACCESS_MASK checkAccess = ACCESS_MASK.MAXIMUM_ALLOWED;
                this.uintMethodStatus = lsadClientStack.LsarOpenPolicy(
                    serverName,
                    objectAttributes,
                    checkAccess,
                    out this.objPolicyHandle);

                this.uintMethodStatus = lsadClientStack.LsarCreateAccount(
                    this.objPolicyHandle.Value,
                    this.objAccountSid[0],
                    this.uintOpenAccAccess,
                    out this.objAccountHandle);

                this.uintMethodStatus = lsadClientStack.LsarOpenAccount(
                    this.objPolicyHandle.Value,
                    this.objAccountSid[0],
                    this.uintOpenAccAccess,
                    out this.objAccountHandle);

                #endregion Passing ValidHandle

                this.validAccountHandle = this.objAccountHandle;
            }

            if (stPolicyInformation.PHandle + 1 != handleInput || this.htAccHandle.Count == uintHandle)
            {
                this.objAccountHandle = utilities.AccountObjInvalidHandle();

                this.uintMethodStatus = lsadClientStack.LsarEnumeratePrivilegesAccount(
                    this.objAccountHandle.Value,
                    out privilegeSet);
            }
            else
            {
                this.uintMethodStatus = lsadClientStack.LsarEnumeratePrivilegesAccount(
                    this.objAccountHandle.Value,
                    out privilegeSet);
            }

            privileges = new Set<AccountPrivilege>();

            if (privilegeSet == null)
            {
                privileges = privileges.Add(AccountPrivilege.Invalid);
            }
            else
            {
                privileges = privileges.Add(AccountPrivilege.Valid);
            }

            if (stPolicyInformation.PHandle + 1 != handleInput || this.htAccHandle.Count == uintHandle)
            {
                #region MS-LSAD_R464

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidHandle,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    464,
                    @"In LsarEnumeratePrivilegesAccount method, If AccountHandle is not a valid context handle to 
                    an account object, the server MUST return STATUS_INVALID_HANDLE.");

                #endregion
            }
            else if ((uint)this.uintOpenAccAccess != LsadUtilities.OpenAccAccess)
            {
                #region MS-LSAD_R465

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.AccessDenied,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    465,
                    @"In AccountHandle parameter of LsarEnumeratePrivilegesAccount method ,the server MUST 
                    verify that the handle has been opened for ACCOUNT_VIEW access and fail the request with 
                    STATUS_ACCESS_DENIED otherwise.");

                #endregion
            }
            else
            {
                #region MS-LSAD_R467

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    467,
                    @"In LsarEnumeratePrivilegesAccount  method,if request is succesfully completed then 
                    server MUST return STATUS_SUCCESS.");

                #endregion

                if (privilegeSet.Value.PrivilegeCount != uintIndex && privilegeSet != null)
                {
                    #region MS-LSAD_R883

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.Success,
                        (uint)this.uintMethodStatus,
                        "MS-LSAD",
                        883,
                        @"For  LSAPR_PRIVILEGE_SET,If PrivilegeCount is not 0, Privilege MUST NOT be NULL.");

                    #endregion
                }

                // Windows 2k8 implementations of this protocol validates LSAPR_LUID_AND_ATTRIBUTES
                // Luid.HighPart MUST NOT be 0.
                // Luid.LowPart MUST be less than or equal to 35.
                // Attributes MUST have ONLY combinations of bits (0x00000001 & 0x00000002) set.
                if (privilegeSet.Value.PrivilegeCount != 0 
                        && privilegeSet.Value.Privilege[uintIndex].Luid.HighPart == Luid_HighPart
                        && privilegeSet.Value.Privilege[uintIndex].Luid.LowPart <= Luid_LowPart
                        && (privilegeSet.Value.Privilege[uintIndex].Attributes == Attribute_Disabled
                                || privilegeSet.Value.Privilege[uintIndex].Attributes == Attribute_Enabled))
                {
                    #region MS-LSAD_R946

                    //// Verify requirement MS-LSAD_R100946 and MS-LSAD_R946
                    string isR946Implementated = R946Implementation;
                    bool isR100946Satisfied = (uint)this.uintMethodStatus == (uint)ErrorStatus.Success;

                    if (isWindows && PDCOSVersion >= ServerVersion.Win2008R2)
                    {
                        //// Add the debug information
                        Site.Log.Add(LogEntryKind.Debug, "Verify MS-LSAD_R100946");

                        //// Verify MS-LSAD requirement: 100946
                        Site.CaptureRequirementIfIsTrue(
                            isR100946Satisfied,
                            "MS-LSAD",
                            100946,
                            @"For LSAPR_LUID_AND_ATTRIBUTES, Luid.HighPart is not 0 except in windows Windows 2000, 
                            Windows XP, Windows Server 2003 and Windows Server 2003 R2.");

                        if (null == isR946Implementated)
                        {
                            Site.Properties.Add("R946Implementation", bool.TrueString);
                            isR946Implementated = bool.TrueString;
                        }
                    }

                    if (null != isR946Implementated)
                    {
                        bool implement = bool.Parse(isR946Implementated);
                        bool isSatisfied = isR100946Satisfied;

                        //// Add the debug information
                        Site.Log.Add(LogEntryKind.Debug, "Verify MS-LSAD_R946");

                        //// Verify MS-LSAD requirement: 946
                        Site.CaptureRequirementIfAreEqual<bool>(
                            implement,
                            isSatisfied,
                            "MS-LSAD",
                            946,
                            string.Format(
                            @"For LSAPR_LUID_AND_ATTRIBUTES, Luid.HighPart SHOULD NOT be 0.<94> 
                            This requirement is {0} implement", implement ? string.Empty : "not"));
                    }

                    #endregion

                    #region MS-LSAD_R949

                    //// Verify requirement MS-LSAD_R100949 and MS-LSAD_R949
                    string isR949Implementated = R949Implementation;
                    bool isR100949Satisfied = (uint)this.uintMethodStatus == (uint)ErrorStatus.Success;

                    if (isWindows && PDCOSVersion >= ServerVersion.Win2008)
                    {
                        //// Add the debug information
                        Site.Log.Add(LogEntryKind.Debug, "Verify MS-LSAD_R100949");

                        //// Verify MS-LSAD requirement: 100949
                        Site.CaptureRequirementIfIsTrue(
                            isR100949Satisfied,
                            "MS-LSAD",
                            100949,
                            @"For LSAPR_LUID_AND_ATTRIBUTES, Luid.LowPart is less than or equal to 35 except in 
                            windows Windows 2000, Windows XP, Windows Server 2003 and Windows Server 2003 R2.");

                        if (null == isR949Implementated)
                        {
                            Site.Properties.Add("R949Implementation", bool.TrueString);
                            isR949Implementated = bool.TrueString;
                        }
                    }

                    if (null != isR949Implementated)
                    {
                        bool implement = bool.Parse(isR949Implementated);
                        bool isSatisfied = isR100949Satisfied;

                        //// Add the debug information
                        Site.Log.Add(LogEntryKind.Debug, "Verify MS-LSAD_R949");

                        //// Verify MS-LSAD requirement: 949
                        Site.CaptureRequirementIfAreEqual<bool>(
                            implement,
                            isSatisfied,
                            "MS-LSAD",
                            949,
                            string.Format(
                            @"For LSAPR_LUID_AND_ATTRIBUTES, Luid.LowPart SHOULD be less than or equal 
                            to 35.<95> This requirement is {0} implement", implement ? string.Empty : "not"));
                    }
                    #endregion

                    #region MS-LSAD_R952

                    //// Verify requirement MS-LSAD_R100952 and MS-LSAD_R952
                    string isR952Implementated = R952Implementation;
                    bool isR100952Satisfied = (uint)this.uintMethodStatus == (uint)ErrorStatus.Success;

                    if (isWindows && PDCOSVersion >= ServerVersion.Win2008)
                    {
                        //// Add the debug information
                        Site.Log.Add(LogEntryKind.Debug, "Verify MS-LSAD_R100952");

                        //// Verify MS-LSAD requirement: 100952
                        Site.CaptureRequirementIfIsTrue(
                            isR100952Satisfied,
                            "MS-LSAD",
                            100952,
                            @"For LSAPR_LUID_AND_ATTRIBUTE,S Attributes have only combinations of bits 
                            (0x00000001 & 0x00000002) set except in windows Windows 2000, Windows XP, 
                            Windows Server 2003 and Windows Server 2003 R2.");

                        if (null == isR952Implementated)
                        {
                            Site.Properties.Add("R952Implementation", bool.TrueString);
                            isR952Implementated = bool.TrueString;
                        }
                    }

                    if (null != isR952Implementated)
                    {
                        bool implement = bool.Parse(isR952Implementated);
                        bool isSatisfied = isR100952Satisfied;

                        //// Add the debug information
                        Site.Log.Add(LogEntryKind.Debug, "Verify MS-LSAD_R952");

                        //// Verify MS-LSAD requirement: 952
                        Site.CaptureRequirementIfAreEqual<bool>(
                            implement,
                            isSatisfied,
                            "MS-LSAD",
                            952,
                            string.Format(
                            @"For LSAPR_LUID_AND_ATTRIBUTE,S Attributes SHOULD have only combinations of 
                            bits (0x00000001 & 0x00000002) set.<96> 
                            This requirement is {0} implement", implement ? string.Empty : "not"));
                    }

                    #endregion
                }               

                #region MS-LSAD_R89

                if (privilegeSet.Value.PrivilegeCount != 0)
                {
                    Site.CaptureRequirementIfIsNotNull(
                        privilegeSet.Value.Privilege,
                        "MS-LSAD",
                        89,
                        @"In LSAPR_PRIVILEGE_SET structure under field 'Privilege' : If the PrivilegeCount field has a 
                        value different than 0, Privilege field MUST NOT be NULL.");
                }

                #endregion
            }

            return (ErrorStatus)this.uintMethodStatus;
        }

        #endregion LsarEnumeratePrivilegesAccount

        #region LsarAddPrivilegesToAccount

        /// <summary>
        /// The AddPrivilegesToAccount method is invoked to add new privileges to an existing account object.
        /// </summary>
        /// <param name="handleInput">Contains account handle obtained from CreateAccount/OpenAccount </param>
        /// <param name="privilege">Contains the set of privileges to be added to an account object</param>
        /// <returns>Returns Success if the method is successful;
        /// Returns InvalidParameter if the parameters passed to the method are not valid;
        /// Returns AccessDenied if the caller does not have the permissions to perform this operation;
        /// Returns InvalidHandle if the passed in account handle is not valid.</returns>
        public ErrorStatus AddPrivilegesToAccount(int handleInput, Set<string> privilege)
        {
            bool luidAttributeValidation = false;
            this.objAccountHandle = this.validAccountHandle;
            
            _LSAPR_PRIVILEGE_SET? privilegeSet = new _LSAPR_PRIVILEGE_SET?();
            privilegeSet = utilities.AddPrivilege(PrivilegeType.Valid);
            luidAttributeValidation = true;

            if (privilege.Contains(LsadUtilities.Priv2))
            {
                //if (GetProperty("Win2K8PrivCheck").ToLower() == Convert.ToString(Server.Windows2k8).ToLower())
                if(PDCOSVersion >= ServerVersion.Win2008)
                {
                    bool checkHandle = false;
                    this.objAccountHandle = utilities.win2k8AccHandle();
                    checkHandle = true;
                    privilegeSet = utilities.AddPrivilege(PrivilegeType.InValid);
                    luidAttributeValidation = false;

                    this.uintMethodStatus = lsadClientStack.LsarAddPrivilegesToAccount(
                        this.objAccountHandle.Value, 
                        privilegeSet.Value);

                    #region DeleteHandle

                    if (checkHandle == true)
                    {
                        lsadClientStack.LsarDeleteObject(ref this.objAccountHandle);
                    }

                    #endregion DeleteHandle
                }
                else
                {
                   this.uintMethodStatus = NtStatus.STATUS_INVALID_PARAMETER;
                }
            }
            else if (stPolicyInformation.PHandle + 1 != handleInput || this.htAccHandle.Count == uintHandle)
            {
                this.objAccountHandle = utilities.AccountObjInvalidHandle();
                this.uintMethodStatus = lsadClientStack.LsarAddPrivilegesToAccount(
                    this.objAccountHandle.Value, 
                    privilegeSet.Value);
            }
            else
            {
                this.uintMethodStatus = lsadClientStack.LsarAddPrivilegesToAccount(
                    this.objAccountHandle.Value, 
                    privilegeSet.Value);
            }

            if (privilege.Contains(LsadUtilities.Priv2))
            {
                string IsR471Implemented = R471Implementation;

                if (isWindows && PDCOSVersion >= ServerVersion.Win2008)
                {
                    #region MS-LSAD_R471

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.InvalidParameter,
                        (uint)this.uintMethodStatus,
                        "MS-LSAD",
                        471,
                        @"In Privileges parameter of LsarAddPrivilegesToAccount method , if LUID of Privileges 
                        is not recognized as valid by the server SHOULD cause the message to be rejected with 
                        STATUS_INVALID_PARAMETER.<58>");

                    #endregion

                    if (IsR471Implemented == null)
                    {
                        Site.Properties.Add("IsR471Implemented", bool.TrueString);
                        IsR471Implemented = bool.TrueString;
                    }
                }

                if (IsR471Implemented != null)
                {
                    bool implSigns = bool.Parse(IsR471Implemented);
                    bool isSatisfied = this.uintMethodStatus == NtStatus.STATUS_INVALID_PARAMETER;

                    Site.CaptureRequirementIfAreEqual<bool>(
                        implSigns,
                        isSatisfied,
                        "MS-LSAD",
                        471,
                        @"In Privileges parameter of LsarAddPrivilegesToAccount method , if LUID of Privileges 
                        is not recognized as valid by the server SHOULD cause the message to be rejected with 
                        STATUS_INVALID_PARAMETER.<58>");
                }
            }
            else if (stPolicyInformation.PHandle + 1 != handleInput || this.htAccHandle.Count == uintHandle)
            {
                #region MS-LSAD_R469

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidHandle,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    469,
                    @"In LsarAddPrivilegesToAccount method , If AccountHandle is not a valid context handle to 
                    an account object, the server MUST return STATUS_INVALID_HANDLE. ");

                #endregion
            }
            else if ((this.uintOpenAccAccess & ACCESS_MASK.ACCOUNT_ADJUST_PRIVILEGES) != 
                ACCESS_MASK.ACCOUNT_ADJUST_PRIVILEGES)
            {
                #region MS-LSAD_R470

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.AccessDenied,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    470,
                    @"In  AccountHandle parameter of LsarAddPrivilegesToAccount method, server MUST verify that
                    the handle has been opened for ACCOUNT_ADJUST_PRIVILEGES access and fail the request with 
                    STATUS_ACCESS_DENIED otherwise.");

                #endregion
            }
            else
            {
                #region MS-LSAD_R472

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    472,
                    @"In LsarAddPrivilegesToAccount method,if request is succesfully completed then server
                    MUST return STATUS_SUCCESS.");

                #endregion

                if (luidAttributeValidation == true)
                {
                    #region MS-LSAD_R884

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.Success,
                        (uint)this.uintMethodStatus,
                        "MS-LSAD",
                        884,
                        @"For LSAPR_PRIVILEGE_SET, Each Privilege MUST pass validation for LSAPR_LUID_AND_ATTRIBUTES.");

                    #endregion
                }
            }

            return (ErrorStatus)this.uintMethodStatus;
        }

        #endregion LsarAddPrivilegesToAccount

        #region LsarRemovePrivilegesFromAccount

        /// <summary>
        /// The RemovePrivilegesFromAccount method is invoked to remove privileges from an account object.
        /// </summary>
        /// <param name="handleInput">Contains account handle obtained from CreateAccount/OpenAccount </param>
        /// <param name="allPrivileges">If this parameter is not FALSE, all privileges 
        /// will be stripped from the account object.</param>
        /// <param name="privilege">Contains the set of privileges to be removed from an account object </param>
        /// <returns>Returns Success if the method is successful;
        /// Returns InvalidParameter if the parameters passed to the method are not valid;
        /// Returns AccessDenied if the caller does not have the permissions to perform this operation;
        /// Returns InvalidHandle if the passed in account handle is not valid.</returns>
        public ErrorStatus RemovePrivilegesFromAccount(int handleInput, bool allPrivileges, Set<string> privilege)
        {
            _LSAPR_PRIVILEGE_SET? privilegeSet = new _LSAPR_PRIVILEGE_SET?();
            privilegeSet = utilities.AddPrivilege(PrivilegeType.Valid);
            _LSAPR_PRIVILEGE_SET privilegeSetTemp = new _LSAPR_PRIVILEGE_SET();
            privilegeSetTemp = privilegeSet.Value;
            privilegeSetTemp.Privilege = new _LSAPR_LUID_AND_ATTRIBUTES[1];
            privilegeSet = privilegeSetTemp;
            bool invalidCheck = false;
            bool invalidParamcheck = false;
            bool checkHandle = false;
            byte AllPriv;

            if (privilege.Contains(LsadUtilities.Priv2))
            {
                AllPriv = 1;
                invalidParamcheck = true;

                //if (Site.Properties.Get("Win2K8PrivCheck").ToLower() == Convert.ToString(Server.Windows2k8).ToLower())
                if (PDCOSVersion >= ServerVersion.Win2008)
                {
                    this.validAccountHandle = utilities.AccountObjInvalidHandle();
                    checkHandle = true;
                }
            }
            else
            {
                AllPriv = 0;
            }

            if ((stPolicyInformation.PHandle + 1 != handleInput
                     || this.htAccHandle.Count == uintHandle)
                 && invalidParamcheck == false)
            {
                //if (Site.Properties.Get("Win2K8PrivCheck").ToLower() == Convert.ToString(Server.Windows2k8).ToLower())
                if (PDCOSVersion >= ServerVersion.Win2008)
                {
                    this.uintMethodStatus = NtStatus.STATUS_INVALID_HANDLE;
                    invalidCheck = true;
                }
                else
                {
                    this.objAccountHandle = utilities.AccountObjInvalidHandle();
                }
            }

            if (invalidCheck == false)
            {
                this.uintMethodStatus = lsadClientStack.LsarRemovePrivilegesFromAccount(
                    this.objAccountHandle.Value,
                    AllPriv,
                    privilegeSet);
            }

            if (checkHandle == true)
            {
                this.validAccountHandle = this.objAccountHandle;
            }

            if (privilege.Contains(LsadUtilities.Priv2))
            {
                #region MS-LSAD_R478

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidParameter,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    478,
                    @"In AllPrivileges parameter of LsarRemovePrivilegesFromAccount method , If AllPrivileges 
                    parameter is 0 then server MUST verify that Privileges is not NULL and fail the request with 
                    STATUS_INVALID_PARAMETER otherwise.<59>");

                #endregion

                if (invalidCheck == false)
                {
                    lsadClientStack.LsarEnumeratePrivilegesAccount(this.objAccountHandle.Value, out privilegeSet);

                    if (privilegeSet == null && AllPriv != 0)
                    {
                        #region MS-LSAD_R970

                        Site.CaptureRequirement(
                            "MS-LSAD",
                            970,
                            @"In Privileges parameter of LsarRemovePrivilegesFromAccount method, if AllPrivileges 
                            parameter is not FALSE (0), all privileges will be stripped from the account object.");

                        #endregion
                    }
                }
            }
            else if (stPolicyInformation.PHandle + 1 != handleInput || this.htAccHandle.Count == uintHandle)
            {
                #region MS-LSAD_R474

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidHandle,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    474,
                    @"In LsarRemovePrivilegesFromAccount method , If AccountHandle is not a valid context handle to 
                    an account object, the server MUST return STATUS_INVALID_HANDLE.");

                #endregion
            }
            else if ((uint)this.uintOpenAccAccess != LsadUtilities.OpenAccAccess)
            {
                #region MS-LSAD_R475

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.AccessDenied,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    475,
                    @"In AccountHandle parameter of LsarRemovePrivilegesFromAccount, server MUST verify that the 
                    handle has been opened for ACCOUNT_ADJUST_PRIVILEGES access and fail the request with 
                    STATUS_ACCESS_DENIED otherwise.");

                #endregion
            }
            else
            {
                lsadClientStack.LsarEnumeratePrivilegesAccount(this.objAccountHandle.Value, out privilegeSet);

                ////Checking whether privilegeSet is removed or not.
                if (this.uintMethodStatus == NtStatus.STATUS_SUCCESS)
                {
                    #region MS-LSAD_R481

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.Success,
                        (uint)this.uintMethodStatus,
                        "MS-LSAD",
                        481,
                        @"In LsarRemovePrivilegesFromAccount method,if request is succesfully completed then server 
                        MUST return STATUS_SUCCESS.");

                    #endregion
                }
            }

            return (ErrorStatus)this.uintMethodStatus;
        }

        #endregion LsarRemovePrivilegesFromAccount

        #region LsarGetSystemAccessAccount

        /// <summary>
        /// The GetSystemAccessAccount method is invoked to retrieve system access account flags 
        /// for an account object. System access account flags are described as part of the account object 
        /// data model, as specified in section.
        /// </summary>
        /// <param name="handleInput">Contains account handle obtained from CreateAccount/OpenAccount </param>
        /// <param name="accessAccount">Out param which contains the set of system access rights to be retrieved
        /// from an account object </param>
        /// <returns>Returns Success if the method is successful;
        /// Returns AccessDenied if the caller does not have the permissions to perform this operation;
        /// Returns InvalidHandle if the passed in account handle is not valid.</returns>
        public ErrorStatus GetSystemAccessAccount(int handleInput, out SystemAccessAccount accessAccount)
        {
            this.objAccountHandle = this.validAccountHandle;
            uint? SystemAccess;

            if (stPolicyInformation.PHandle + 1 != handleInput || this.htAccHandle.Count == uintHandle)
            {
                this.objAccountHandle = utilities.AccountObjInvalidHandle();
            }

            this.uintMethodStatus = lsadClientStack.LsarGetSystemAccessAccount(
                this.objAccountHandle.Value, 
                out SystemAccess);

            accessAccount = (LsadUtilities.ReturnSuccess == (int)this.uintMethodStatus) ?
                SystemAccessAccount.Valid : SystemAccessAccount.Invalid;

            if (stPolicyInformation.PHandle + 1 != handleInput || this.htAccHandle.Count == uintHandle)
            {
                #region MS-LSAD_R483

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidHandle,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    483,
                    @"In LsarGetSystemAccessAccount  method, If AccountHandle is not a valid context handle to 
                    an account object, the server MUST return STATUS_InvalidHandle.");

                #endregion
            }
            else if ((uint)this.uintOpenAccAccess != LsadUtilities.OpenAccAccess)
            {
                #region MS-LSAD_R484

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.AccessDenied,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    484,
                    @"In AccountHandle parameter of LsarGetSystemAccessAccount  method, server MUST verify 
                    that the handle has been opened for ACCOUNT_VIEW access and fail the request with
                    STATUS_ACCESS_DENIED otherwise.");

                #endregion
            }
            else
            {
                #region MS-LSAD_R485

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    485,
                    @"In LsarGetSystemAccessAccount method,if request is succesfully completed then server 
                    MUST return STATUS_SUCCESS.");

                #endregion
            }

            return (ErrorStatus)this.uintMethodStatus;
        }

        #endregion LsarGetSystemAccessAccount

        #region LsarSetSystemAccessAccount

        /// <summary>
        /// The SetSystemAccessAccount method is invoked to set system access account flags 
        /// for an account object.
        /// </summary>
        /// <param name="handleInput">Contains account handle obtained from CreateAccount/OpenAccount </param>
        /// <param name="systemAccess">Contains the set of system access rights to be 
        /// set to an account object </param>
        /// <returns>Returns Success if the method is successful;
        /// Returns InvalidParameter if the parameters passed to the method are not valid;
        /// Returns AccessDenied if the caller does not have the permissions to perform this operation;
        /// Returns InvalidHandle if the passed in account handle is not valid.</returns>
        public ErrorStatus SetSystemAccessAccount(int handleInput, uint systemAccess)
        {
            this.objAccountHandle = this.validAccountHandle;
            int accountAccessDenied = 8;
            int SetSystemAccess = 32;
            int SetSystemAccessInvalidParam = 4055;
            uintAccessMask = ACCESS_MASK.MAXIMUM_ALLOWED;

            if ((this.uintOpenAccAccess & ACCESS_MASK.TRUSTED_QUERY_POSIX) != ACCESS_MASK.TRUSTED_QUERY_POSIX)
            {
                uintAccessMask = ACCESS_MASK.NONE;
            }

            this.uintMethodStatus = lsadClientStack.LsarOpenAccount(
                this.objPolicyHandle.Value,
                this.objAccountSid[0],
                uintAccessMask,
                out this.objAccountHandle);

            this.uintSystemAccess = 0x00000000;

            if (systemAccess > SetSystemAccessInvalidParam
                    || (systemAccess & accountAccessDenied) == accountAccessDenied
                    || (systemAccess & SetSystemAccess) == SetSystemAccess)
            {
                this.uintSystemAccess = 0x00000009;
            }
            else if (stPolicyInformation.PHandle + 1 != handleInput || this.htAccHandle.Count == uintHandle)
            {
                this.objAccountHandle = utilities.AccountObjInvalidHandle();
            }

            this.uintMethodStatus = lsadClientStack.LsarSetSystemAccessAccount(
                this.objAccountHandle.Value,
                Convert.ToUInt32(this.uintSystemAccess));

            if (!(systemAccess > SetSystemAccessInvalidParam
                  || (systemAccess & accountAccessDenied) == accountAccessDenied
                  || (systemAccess & SetSystemAccess) == SetSystemAccess))
            {
                if (stPolicyInformation.PHandle + 1 != handleInput || this.htAccHandle.Count == uintHandle)
                {
                    #region MS-LSAD_R487

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.InvalidHandle,
                        (uint)this.uintMethodStatus,
                        "MS-LSAD",
                        487,
                        @"In LsarSetSystemAccessAccount method, If AccountHandle is not a valid context handle 
                        to an account object, the server MUST return STATUS_INVALID_HANDLE.");

                    #endregion
                }
                else if ((this.uintOpenAccAccess & ACCESS_MASK.ACCOUNT_ADJUST_SYSTEM_ACCESS) != 
                    ACCESS_MASK.ACCOUNT_ADJUST_SYSTEM_ACCESS)
                {
                    #region MS-LSAD_R488

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.AccessDenied,
                        (uint)this.uintMethodStatus,
                        "MS-LSAD",
                        488,
                        @"In AccountHandle parameter of LsarSetSystemAccessAccount method , server MUST verify 
                        that the handle has been opened for ACCOUNT_ADJUST_SYSTEM_ACCESS access and fail the request 
                        with ACCESS_DENIED otherwise.");

                    #endregion
                }
                else
                {
                    #region MS-LSAD_R491

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.Success,
                        (uint)this.uintMethodStatus,
                        "MS-LSAD",
                        491,
                        @"In LsarSetSystemAccessAccount method,if request is succesfully completed then server 
                        MUST return STATUS_SUCCESS.");

                    #endregion
                }
            }

            return (ErrorStatus)this.uintMethodStatus;
        }

        #endregion LsarSetSystemAccessAccount

        #region LsarEnumerateAccountsWithUserRight

        /// <summary>
        /// The EnumerateAccountsWithUserRight method is invoked to return a list of account objects
        /// that have the user right equal to the passed-in value. 
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="userRight">Contains the user right of account object that is enumerated</param>
        /// <param name="right">It is for validation of passed in user right whether it is valid or invalid</param>
        /// <param name="enumerationBuffer">Out param which contains the list of account objects that have
        /// the user right equal to the passed-in value</param>
        /// <returns>Returns Success if the method is successful;
        /// Returns InvalidParameter if the parameters passed to the method are not valid;
        /// Returns AccessDenied if the caller does not have the permissions to perform this operation;
        /// Returns InvalidHandle if the passed in policy handle is not valid;
        /// Returns NoSuchPrivilege if the supplied user right is not recognized by the server;
        /// Returns NoMoreEntries if no account was found with the specified user right.</returns>
        public ErrorStatus EnumerateAccountsWithUserRight(
            int handleInput,
            string userRight,
            ValidString right,
            out AccountSid enumerationBuffer)
        {
            this.objAccountHandle = this.validAccountHandle;
            _RPC_UNICODE_STRING[] objUserRight = new _RPC_UNICODE_STRING[1];
            _LSAPR_ACCOUNT_ENUM_BUFFER? EnumBuffer = new _LSAPR_ACCOUNT_ENUM_BUFFER();

            utilities.AccountsWithUserRight(Convert.ToString(TypeOfUserRight.ValidUserRight), ref objUserRight);
            lsadClientStack.LsarOpenPolicy(serverName, objectAttributes, this.uintDesrAccess, out this.objPolicyHandle);

            if (right == ValidString.Invalid)
            {
                objUserRight = utilities.InvalidParamUserRight();
                this.uintDesrAccess = ACCESS_MASK.MAXIMUM_ALLOWED;
                this.invalidParamCheck = true;
            }
            else if (stPolicyInformation.PHandle != handleInput)
            {
                this.objPolicyHandle = utilities.AccountObjInvalidHandle();
                this.invalidHandleCheck = true;
            }

            if ((userRight != LsadUtilities.Privilege1)
                     && (this.invalidParamCheck == false)
                     && (this.invalidHandleCheck == false))
            {
                utilities.AccountsWithUserRight(
                    Convert.ToString(TypeOfUserRight.InvalidUserRight),
                    ref objUserRight);
            }
            else if ((this.htAddAccRight.Count == uintHandle)
                          && (right == ValidString.Valid)
                          && (stPolicyInformation.PHandle == handleInput))
            {
                utilities.AccountsWithUserRight(
                    Convert.ToString(TypeOfUserRight.NoPrivilegeWithAccount),
                    ref objUserRight);
            }
            else if (((this.uintAccess & ACCESS_MASK.TRUSTED_QUERY_DOMAIN_NAME) != ACCESS_MASK.TRUSTED_QUERY_DOMAIN_NAME)
                           && (this.invalidParamCheck == false)
                           && (this.invalidHandleCheck == false))
            {
                this.uintMethodStatus = lsadClientStack.LsarOpenPolicy(
                    serverName,
                    objectAttributes,
                    uintAccessMask,
                    out this.objPolicyHandle);
            }

            if (right == ValidString.Valid)
            {
                objUserRight[uintIndex].Length = (ushort)(2 * objUserRight[uintIndex].Buffer.Length);
                objUserRight[uintIndex].MaximumLength = (ushort)(objUserRight[uintIndex].Length + 2);
            }

            this.uintMethodStatus = lsadClientStack.LsarEnumerateAccountsWithUserRight(
                this.objPolicyHandle.Value,
                objUserRight[0],
                out EnumBuffer);

            enumerationBuffer = 
                (LsadUtilities.ReturnSuccess == EnumBuffer.Value.EntriesRead) ? AccountSid.Invalid : AccountSid.Valid;

            if (right == ValidString.Invalid)
            {
                #region MS-LSAD_R870

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidParameter,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    870,
                    @"In LsarEnumerateAccountsWithUserRight  method, if one of the supplied arguments is invalid then 
                    server MUST return STATUS_INVALID_PARAMETER.");

                #endregion
            }
            else if (stPolicyInformation.PHandle != handleInput)
            {
                #region MS-LSAD_R493

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidHandle,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    493,
                    @"In LsarEnumerateAccountsWithUserRight method ,If Policyhandle is not a valid context handle 
                    to policy object, the server MUST return STATUS_INVALID_HANDLE.");

                #endregion

                #region MS-LSAD_R216

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidHandle,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    216,
                    @"Upon receipt of a handle parameter, the server MUST check to see that the handle is one of 
                    the valid handles of a type relevant for that operation, and fail the request by returning 
                    STATUS_INVALID_HANDLE otherwise");

                #endregion
            }
            else if ((userRight != LsadUtilities.Privilege1)
                          && (this.invalidParamCheck == false)
                          && (this.invalidHandleCheck == false))
            {
                #region MS-LSAD_R496

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.NoSuchPrivilege,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    496,
                    @"In UserRight parameter of LsarEnumerateAccountsWithUserRight method ,if the server does not 
                    recognize the account right, it MUST return STATUS_NO_SUCH_PRIVILEGE.");

                #endregion
            }
            else if ((this.htAddAccRight.Count == uintHandle)
                          && (right == ValidString.Valid)
                          && (stPolicyInformation.PHandle == handleInput))
            {
                #region MS-LSAD_R871

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.NoMoreEntries,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    871,
                    @"In LsarEnumerateAccountsWithUserRight  method,No account was found with the specified privilege.
                    then server MUST return STATUS_NO_MORE_ENTRIES.");

                #endregion
            }
            else if (((this.uintAccess & ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION) != 
                ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION)
                          && (this.invalidParamCheck == false)
                          && (this.invalidHandleCheck == false))
            {
                #region MS-LSAD_R494

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.AccessDenied,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    494,
                    @"In PolicyHandle parameter of LsarEnumerateAccountsWithUserRight method , server MUST verify 
                    that the handle has POLICY_VIEW_LOCAL_INFORMATION access, and fail the request with 
                    STATUS_ACCESS_DENIED otherwise.<60>");

                #endregion
            }
            else
            {
                #region MS-LSAD_R497

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    497,
                    @"In LsarEnumerateAccountsWithUserRight  method, if request is succesfully completed then server 
                    MUST return STATUS_SUCCESS.");

                #endregion
            }

            return (ErrorStatus)this.uintMethodStatus;
        }

        #endregion LsarEnumerateAccountsWithUserRight

        #region LsarEnumerateAccountRights

        /// <summary>
        /// The EnumerateAccountRights method is invoked to retrieve a list of rights 
        /// associated with an existing account.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="sid">It is for validation of passed in account sid whether it is valid or invalid</param>
        /// <param name="accountSid">Contains account sid of an account object whose rights are retrieved</param>
        /// <param name="userRights">Out param which contains the list of rights associated 
        /// with an existing account</param>
        /// <returns>Returns Success if the method is successful;
        /// Returns InvalidParameter if the parameters passed to the method are not valid;
        /// Returns AccessDenied if the caller does not have the permissions to perform this operation;
        /// Returns InvalidHandle if the passed in policy handle is not valid;
        /// Returns ObjectNameNotFound if the specified account object does not exist.</returns>
        public ErrorStatus EnumerateAccountRights(
            int handleInput,
            AccountSid sid,
            string accountSid,
            out userRight userRights)
        {
            this.objAccountHandle = this.validAccountHandle;

            _LSAPR_USER_RIGHT_SET? enumRights = new _LSAPR_USER_RIGHT_SET();

            utilities.DeleteUnknownSID();
            this.objAccountSid = utilities.SID(TypeOfSID.NewSID);
            this.objAccountSid[uintIndex].Revision = (sid == AccountSid.Valid) ? (byte)0x01 : (byte)0x00;

            if (((stPolicyInformation.PHandle != handleInput) || (this.htAccHandle.Count == uintHandle))
                      && (stPolicyInformation.PHandle != handleInput))
            {
                this.objPolicyHandle = utilities.AccountObjInvalidHandle();
            }

            if ((stPolicyInformation.PHandle == handleInput)
                      && (sid == AccountSid.Valid)
                      && (this.htAccHandle.Count == uintHandle)
                      && (this.htAddAccRight.Count == uintHandle))
            {
                this.objAccountSid = utilities.SID(TypeOfSID.UnKnownSID);
            }

            this.uintMethodStatus = lsadClientStack.LsarEnumerateAccountRights(
                this.objPolicyHandle.Value,
                this.objAccountSid[0],
                out enumRights);

            userRights = (null == enumRights.Value.UserRights) ? userRight.Invalid : userRight.Valid;

            if (sid == AccountSid.Invalid)
            {
                #region MS-LSAD_R501

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidParameter,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    501,
                    @"In AccountSid parameter of  LsarEnumerateAccountRights method,server MUST verify that the SID 
                    pointed to by AccountSid is valid, and fail the request with STATUS_INVALID_PARAMETER otherwise.");

                #endregion
            }
            else if (stPolicyInformation.PHandle != handleInput)
            {
                #region MS-LSAD_R500

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidHandle,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    500,
                    @"In PolicyHandle parameter of LsarEnumerateAccountRights method,If the handle is not a valid 
                    context handle to policy object, the server MUST return STATUS_INVALID_HANDLE.");

                #endregion
            }
            else if ((stPolicyInformation.PHandle == handleInput)
                          && (sid == AccountSid.Valid)
                          && (this.htAccHandle.Count == uintHandle)
                          || ((this.uintOpenAccAccess & ACCESS_MASK.TRUSTED_QUERY_DOMAIN_NAME) != 
                ACCESS_MASK.TRUSTED_QUERY_DOMAIN_NAME))
            {
                #region MS-LSAD_R503

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.ObjectNameNotFound,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    503,
                    @"In AccountSid parameter of LsarEnumerateAccountRights method, , server MUST verify that such an 
                    account exists in its database and fail the request with STATUS_OBJECT_NAME_NOT_FOUND otherwise.");

                #endregion
            }
            else
            {
                #region MS-LSAD_R506

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    506,
                    @"In LsarEnumerateAccountRights method,if request is succesfully completed then server MUST 
                    return STATUS_SUCCESS.");

                #endregion

                if ((enumRights.Value.Entries != uintEntries) && (enumRights.Value.UserRights != null))
                {
                    #region MS-LSAD_R100

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.Success,
                        (uint)this.uintMethodStatus,
                        "MS-LSAD",
                        100,
                        @"In LSAPR_USER_RIGHT_SET structure under field UserRights, 
                        If the Entries field has a value other than 0, UserRights field MUST NOT be NULL.");

                    #endregion

                    #region MS-LSAD_R944

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.Success,
                        (uint)this.uintMethodStatus,
                        "MS-LSAD",
                        944,
                        @"For LSAPR_USER_RIGHT_SET, if Entries is not 0, UserRights MUST NOT be NULL.");

                    #endregion
                }

                #region MS-LSAD_R505

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    505,
                    @"In AccountSid parameter of LsarEnumerateAccountRights method, ,server MUST return the string 
                    names of all the system access rights and privileges associated with the account.");

                #endregion

                // Length MUST be a multiple of 2.
                // Length MUST be less than or equal to MaximumLength.
                // If Length is not 0, Buffer MUST NOT be NULL.
                if ((enumRights.Value.UserRights[uintIndex].Length == 
                    2 * enumRights.Value.UserRights[uintIndex].Buffer.Length)
                         && (enumRights.Value.UserRights[uintIndex].Length <= 
                         enumRights.Value.UserRights[uintIndex].MaximumLength)
                         && ((enumRights.Value.UserRights[uintIndex].Length != 0)
                                  && (enumRights.Value.UserRights[uintIndex].Buffer != null)))
                {
                    #region MS-LSAD_R874

                    Site.CaptureRequirement(                        
                        "MS-LSAD",
                        874,
                        @"For RPC_UNICODE_STRING, Length MUST be a multiple of 2.");

                    #endregion

                    #region MS-LSAD_R875

                    Site.CaptureRequirement(                        
                        "MS-LSAD",
                        875,
                        @"For RPC_UNICODE_STRING, Length MUST be less than or equal to MaximumLength.");

                    #endregion

                    #region MS-LSAD_R876

                    Site.CaptureRequirement(
                        "MS-LSAD",
                        876,
                        @"For RPC_UNICODE_STRING,If Length is not 0, Buffer MUST NOT be NULL.");

                    #endregion

                    #region MS-LSAD_R945

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.Success,
                        (uint)this.uintMethodStatus,
                        "MS-LSAD",
                        945,
                        @"For LSAPR_USER_RIGHT_SET, Each element in UserRights MUST satisfy 
                        RPC_UNICODE_STRING validation.");

                    #endregion                   
                }
            }

            return (ErrorStatus)this.uintMethodStatus;
        }

        #endregion LsarEnumerateAccountRights

        #region LsarAddAccountRights

        /// <summary>
        /// The AddAccountRights method is invoked to add new rights to an account object.  
        /// If the account object does not exist, the system will attempt to create one.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="accountSid">Contains account sid of an account object</param>
        /// <param name="sid">It is for validation of passed in account sid whether it is valid or invalid</param>
        /// <param name="accountRights">Contains the list of user rights to be added to an account object</param>
        /// <returns>Returns Success if the method is successful;
        /// Returns InvalidParameter if the parameters passed to the method are not valid;
        /// Returns AccessDenied if the caller does not have the permissions to perform this operation;
        /// Returns InvalidHandle if the passed in policy handle is not valid;
        /// Returns NoSuchPrivilege if user rights passed were not recognized.</returns>
        public ErrorStatus AddAccountRights(
            int handleInput,
            string accountSid,
            AccountSid sid,
            Set<string> accountRights)
        {
            ACCESS_MASK accountAccess = ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION 
                                            | ACCESS_MASK.POLICY_VIEW_AUDIT_INFORMATION 
                                            | ACCESS_MASK.POLICY_TRUST_ADMIN;
            this.invalidParamCheck = false;
            this.accessDeniedCheck = false;
            this.invalidHandleCheck = false;
            _LSAPR_USER_RIGHT_SET[] addUserRight = new _LSAPR_USER_RIGHT_SET[1];

            this.uintDesrAccess = ACCESS_MASK.MAXIMUM_ALLOWED;
            lsadClientStack.LsarOpenPolicy(serverName, objectAttributes, this.uintDesrAccess, out this.objPolicyHandle);

            this.objAccountSid[uintIndex].Revision = 0x01;
            addUserRight[uintIndex].Entries = 1;
            string[] accountRightArray = new string[1];
            accountRightArray[0] = Convert.ToString(TypeOfUserRight.ValidUserRight);
            utilities.AddAccountright(accountRightArray, ref addUserRight, 1);

            if (sid == AccountSid.Invalid)
            {
                this.objAccountSid[uintIndex].Revision = 0x00;
                this.invalidParamCheck = true;
                this.uintDesrAccess = ACCESS_MASK.MAXIMUM_ALLOWED;
                lsadClientStack.LsarOpenPolicy(serverName, objectAttributes, this.uintDesrAccess, out this.objPolicyHandle);
                this.uintMethodStatus = lsadClientStack.LsarAddAccountRights(
                    this.objPolicyHandle.Value,
                    this.objAccountSid[0],
                    addUserRight[0]);
            }
            else if (stPolicyInformation.PHandle != handleInput)
            {
                this.objPolicyHandle = utilities.AccountObjInvalidHandle();
                this.invalidHandleCheck = true;
            }
            else if (this.htAccHandle.Count == uintHandle)
            {
                if ((this.uintAccess & ACCESS_MASK.TRUSTED_SET_POSIX) != ACCESS_MASK.TRUSTED_SET_POSIX)
                {
                    this.uintMethodStatus = NtStatus.STATUS_ACCESS_DENIED;
                    this.accessDeniedCheck = true;
                }
            }
            else
            {
                if ((this.uintOpenAccAccess & accountAccess) != accountAccess)
                {
                    this.uintMethodStatus = NtStatus.STATUS_ACCESS_DENIED;
                    this.accessDeniedCheck = true;
                }
            }

            if (!accountRights.Contains(LsadUtilities.Privilege1))
            {
                accountRightArray[0] = Convert.ToString(TypeOfUserRight.InvalidUserRight);
                utilities.AddAccountright(accountRightArray, ref addUserRight, 1);
            }

            if (this.objAccountSid[uintIndex].SubAuthorityCount == uintHandle)
            {
                this.objAccountSid = utilities.SID(TypeOfSID.ExistSID);
                this.objAccountSid[uintIndex].Revision = 0x01;
            }

            if (this.accessDeniedCheck == false)
            {
                if (!accountRights.Contains(LsadUtilities.Privilege1))
                {
                    if ((!accountRights.Contains(LsadUtilities.Privilege1)) && (this.invalidHandleCheck == false))
                    {
                        this.uintDesrAccess = ACCESS_MASK.MAXIMUM_ALLOWED;
                        lsadClientStack.LsarOpenPolicy(
                            serverName,
                            objectAttributes,
                            this.uintDesrAccess,
                            out this.objPolicyHandle);
                    }
                }

                this.uintMethodStatus = lsadClientStack.LsarAddAccountRights(
                    this.objPolicyHandle.Value,
                    this.objAccountSid[0],
                    addUserRight[0]);
            }

            if ((int)this.uintMethodStatus == LsadUtilities.ReturnSuccess)
            {
                this.htAddAccRight.Add("htAddAccRight", uintHandle);
            }

            if (sid == AccountSid.Invalid)
            {
                #region MS-LSAD_R513

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidParameter,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    513,
                    @"In LsarAddAccountRights  method,server MUST return STATUS_INVALID_PARAMETER if One or more of 
                    the supplied parameters was invalid.");

                #endregion
            }
            else if (stPolicyInformation.PHandle != handleInput)
            {
                #region MS-LSAD_R508

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidHandle,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    508,
                    @"In LsarAddAccountRights method,If the Policyhandle is not a valid context handle to the policy 
                    object, the server MUST return STATUS_INVALID_HANDLE.");

                #endregion
            }
            else if (((int)this.uintMethodStatus != LsadUtilities.ReturnSuccess)
                          && (this.accessDeniedCheck == true)
                          && (((this.uintOpenAccAccess & accountAccess) != accountAccess)
                                    || ((this.uintAccess & ACCESS_MASK.TRUSTED_SET_POSIX) != ACCESS_MASK.TRUSTED_SET_POSIX)))
            {
                #region MS-LSAD_R509

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.AccessDenied,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    509,
                    @"In PolicyHandle parameter of LsarAddAccountRights  method,For an account that exists, 
                    the server MUST verify that the caller has the following access rights to the account object: 
                    ACCOUNT_ADJUST_PRIVILEGES, ACCOUNT_ADJUST_SYSTEM_ACCESS, and ACCOUNT_VIEW.");

                #endregion

                #region MS-LSAD_R510

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.AccessDenied,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    510,
                    @"In PolicyHandle parameter of LsarAddAccountRights  method if  access check fails, 
                    then server MUST return STATUS_ACCESS_DENIED.");

                #endregion
            }
            else if (!accountRights.Contains(LsadUtilities.Privilege1))
            {
                #region MS-LSAD_R512

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.NoSuchPrivilege,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    512,
                    @"For UserRight parameter of LsarAddAccountRights  method,If the server does not recognize any of 
                    the rights, it MUST return STATUS_NO_SUCH_PRIVILEGE.");

                #endregion
            }
            else
            {
                #region MS-LSAD_R514

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    514,
                    @"In  LsarAddAccountRights    method,if request is succesfully completed then server MUST 
                    return STATUS_SUCCESS.");

                #endregion

                ////Querying the server to verify that the account object is added.
                NtStatus uintCheckHandle = lsadClientStack.LsarOpenAccount(
                    this.objPolicyHandle.Value,
                    this.objAccountSid[0],
                    uintAccessMask,
                    out this.objAccountHandle);

                if ((int)uintCheckHandle == LsadUtilities.ReturnSuccess)
                {
                    #region MS-LSAD_R511

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.Success,
                        (uint)this.uintMethodStatus,
                        "MS-LSAD",
                        511,
                        @"For AccountSid parameter of LsarAddAccountRights  method ,server MUST 
                        create the account object if one does not exist.");

                    #endregion
                }
            }

            return (ErrorStatus)this.uintMethodStatus;
        }

        #endregion LsarAddAccountRights

        #region LsarRemoveAccountRights

        /// <summary>
        /// The RemoveAccountRights method is invoked to remove rights from an account object.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="accountSid">Contains account sid of an account object</param>
        /// <param name="sid">It is for validation of passed in account sid whether it is valid or invalid</param>
        /// <param name="allRights">If this field is not set to 0, all rights will be removed.</param>
        /// <param name="accountRights">Contains the list of user rights to be added to an account object</param>
        /// <returns>Returns Success if the method is successful;
        /// Returns InvalidParameter if the parameters passed to the method are not valid;
        /// Returns AccessDenied if the caller does not have the permissions to perform this operation;
        /// Returns InvalidHandle if the passed in policy handle is not valid;
        /// Returns NoSuchPrivilege if user rights passed were not recognized;
        /// Returns ObjectNameNotFound if an account with passed in account sid does not exist;
        /// Returns NotSupported if the operation is not supported by server.</returns>
        /// Disable warning CA1502 because it will affect the implementation of Adapter and Model codes 
        /// if do any changes about maintainability.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity",
            Justification = "Disable warning CA1502 because it will affect the implementation of Adapter and Model")]
        public ErrorStatus RemoveAccountRights(
            int handleInput,
            string accountSid,
            AccountSid sid,
            int allRights,
            Set<string> accountRights)
        {
            utilities.DeleteUnknownSID();
            bool ObjNotfound = false;
            this.invalidParamCheck = false;
            this.accessDeniedCheck = false;
            this.invalidHandleCheck = false;
            ACCESS_MASK accountAccess = ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION
                                            | ACCESS_MASK.POLICY_VIEW_AUDIT_INFORMATION
                                            | ACCESS_MASK.POLICY_TRUST_ADMIN;

            _LSAPR_USER_RIGHT_SET[] addUserRight = new _LSAPR_USER_RIGHT_SET[1];
            addUserRight[uintIndex].Entries = (uint)accountRights.Count;

            //"S-1-5-19" and "S-1-5-20" are SIDs
            if (accountSid.CompareTo("S-1-5-19") == 0 || accountSid.CompareTo("S-1-5-20") == 0)
            {
                string[] accountRightArray = accountRights.ToArray();
                utilities.AddAccountright(accountRightArray, ref addUserRight, accountRights.Count);

                //Construct SID for "S-1-5-19" and "S-1-5-20"
                _RPC_SID objSid = new _RPC_SID();
                objSid.Revision = 0x1;
                objSid.IdentifierAuthority = new _RPC_SID_IDENTIFIER_AUTHORITY();
                objSid.IdentifierAuthority.Value = new byte[6];
                objSid.IdentifierAuthority.Value[0] = (byte)Value_Values.NULL_SID_AUTHORITY;
                objSid.IdentifierAuthority.Value[1] = (byte)Value_Values.NULL_SID_AUTHORITY;
                objSid.IdentifierAuthority.Value[2] = (byte)Value_Values.NULL_SID_AUTHORITY;
                objSid.IdentifierAuthority.Value[3] = (byte)Value_Values.NULL_SID_AUTHORITY;
                objSid.IdentifierAuthority.Value[4] = (byte)Value_Values.NULL_SID_AUTHORITY;
                objSid.IdentifierAuthority.Value[5] = (byte)Value_Values.NT_AUTHORITY;
                objSid.SubAuthorityCount = 1;
                objSid.SubAuthority = new uint[1];

                if (accountSid.CompareTo("S-1-5-19") == 0)
                {
                    objSid.SubAuthority[0] = 19;
                }
                else
                {
                    objSid.SubAuthority[0] = 20;
                }

                this.uintMethodStatus = lsadClientStack.LsarRemoveAccountRights(
                    this.objPolicyHandle.Value,
                    objSid,
                    (byte)allRights,
                    addUserRight[0]);

                if (accountRights.Contains(Convert.ToString(AccountRight.SeAuditPrivilege))
                        || accountRights.Contains(Convert.ToString(AccountRight.SeChangeNotifyPrivilege))
                        || accountRights.Contains(Convert.ToString(AccountRight.SeImpersonatePrivilege))
                        || accountRights.Contains(Convert.ToString(AccountRight.SeCreateGlobalPrivilege)))
                {
                    #region MS-LSAD_R520

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.NotSupported,
                        (uint)this.uintMethodStatus,
                        "MS-LSAD",
                        520,
                        @"For UserRight parameter of LsarRemoveAccountRights  method,if server allows removal 
                        of 'SeAuditPrivilege', 'SeChangeNotifyPrivilege', 'SeImpersonatePrivilege', and 
                        'SeCreateGlobalPrivilege' from accounts represented with SIDs 'S-1-5-19' and 'S-1-5-20'then  
                        request MUST be rejected with STATUS_NOT_SUPPORTED.<61>");

                    #endregion
                }

                if (PDCOSVersion >= ServerVersion.Win2008)
                {
                    if ((objSid.SubAuthorityCount == 1
                             && objSid.IdentifierAuthority.Value[5] == 0x05
                             && (objSid.SubAuthority[0] == 19 || objSid.SubAuthority[0] == 20))
                         && (accountRights.Contains(Convert.ToString(AccountRight.SeAuditPrivilege))
                             || accountRights.Contains(Convert.ToString(AccountRight.SeChangeNotifyPrivilege))
                             || accountRights.Contains(Convert.ToString(AccountRight.SeImpersonatePrivilege))
                             || accountRights.Contains(Convert.ToString(AccountRight.SeCreateGlobalPrivilege))))
                    {
                        #region MS-LSAD_R525

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.NotSupported,
                            (uint)this.uintMethodStatus,
                            "MS-LSAD",
                            525,
                            @"<61> Section 3.1.4.5.12: Windows Vista, Windows Server 2008, Windows 7, and 
                            Windows Server 2008 R2 does not allow removal of 'SeAuditPrivilege', 
                            'SeChangeNotifyPrivilege', 'SeImpersonatePrivilege,' and 'SeCreateGlobalPrivilege' 
                            from accounts represented with SIDs 'S-1-5-19' and'S-1-5-20'and the server MUST 
                            return STATUS_NOT_SUPPORTED");

                        #endregion
                    }
                }

                return (ErrorStatus)this.uintMethodStatus;
            }
            else
            {
                string[] accountRightArray = new string[1];
                accountRightArray[0] = Convert.ToString(TypeOfUserRight.ValidUserRight);
                utilities.AddAccountright(accountRightArray, ref addUserRight, 1);
            }

            this.objAccountSid[uintIndex].Revision = 0x01;

            if (sid == AccountSid.Invalid)
            {
                this.objAccountSid[uintIndex].Revision = 0x00;
                this.invalidParamCheck = true;
            }
            else if ((stPolicyInformation.PHandle != handleInput) && (this.invalidParamCheck == false))
            {
                this.objPolicyHandle = utilities.AccountObjInvalidHandle();
                this.invalidHandleCheck = true;
            }
            else if ((this.uintOpenAccAccess & accountAccess) != accountAccess)
            {
                this.uintMethodStatus = NtStatus.STATUS_ACCESS_DENIED;
                this.accessDeniedCheck = true;
            }

            if ((stPolicyInformation.PHandle == handleInput)
                     && (sid == AccountSid.Valid)
                     && (this.htAccHandle.Count == uintHandle)
                     && (this.htAddAccRight.Count == uintHandle))
            {
                this.objAccountSid = utilities.SID(TypeOfSID.UnKnownSID);
                ObjNotfound = true;
            }

            if ((!accountRights.Contains(LsadUtilities.Privilege1)) && (ObjNotfound == false))
            {
                string[] accountRightArray = new string[1];
                accountRightArray[0] = Convert.ToString(TypeOfUserRight.InvalidUserRight);
                utilities.AddAccountright(accountRightArray, ref addUserRight, 1);
            }

            if ((this.accessDeniedCheck == false) || (ObjNotfound == true))
            {
                if (!accountRights.Contains(LsadUtilities.Privilege1))
                {
                    if ((!accountRights.Contains(LsadUtilities.Privilege1)) && (this.invalidHandleCheck == false))
                    {
                        this.uintDesrAccess = ACCESS_MASK.MAXIMUM_ALLOWED;
                        lsadClientStack.LsarOpenPolicy(
                            serverName,
                            objectAttributes,
                            this.uintDesrAccess,
                            out this.objPolicyHandle);
                    }
                }

                if (this.htAccHandle.Count != uintHandle)
                {
                    this.uintMethodStatus = lsadClientStack.LsarOpenAccount(
                        this.objPolicyHandle.Value,
                        this.objAccountSid[0],
                        uintAccessMask,
                        out this.objAccountHandle);

                    if ((int)this.uintMethodStatus != LsadUtilities.ReturnSuccess)
                    {
                        this.uintMethodStatus = lsadClientStack.LsarCreateAccount(
                            this.objPolicyHandle.Value,
                            this.objAccountSid[0],
                            this.uintOpenAccAccess,
                            out this.objAccountHandle);
                    }
                }

                this.uintMethodStatus = lsadClientStack.LsarRemoveAccountRights(
                    this.objPolicyHandle.Value,
                    this.objAccountSid[0],
                    (byte)allRights,
                    addUserRight[0]);
            }

            if (sid == AccountSid.Invalid)
            {
                #region MS-LSAD_R522

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidParameter,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    522,
                    @"In LsarRemoveAccountRights  method,server MUST return STATUS_INVALID_PARAMETER if One or more of 
                    the supplied parameters was invalid.");

                #endregion
            }
            else if (stPolicyInformation.PHandle != handleInput)
            {
                #region MS-LSAD_R516

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidHandle,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    516,
                    @"In LsarRemoveAccountRights method,If the Policyhandle is not a valid context handle to the 
                    policy object, the server MUST return STATUS_INVALID_HANDLE.");

                #endregion
            }
            else if (((this.uintOpenAccAccess & accountAccess) != accountAccess) && (ObjNotfound == false))
            {
                #region MS-LSAD_R524

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.AccessDenied,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    524,
                    @"In LsarRemoveAccountRights method,if caller does not have the permissions to perform this 
                    operation server will return STATUS_ACCESS_DENIED.");

                #endregion
            }
            else if ((stPolicyInformation.PHandle == handleInput)
                          && (sid == AccountSid.Valid)
                          && (this.htAccHandle.Count == uintHandle))
            {
                #region MS-LSAD_R517

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.ObjectNameNotFound,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    517,
                    @"The server MUST verify that such an account exists in its database and fail the request 
                    with STATUS_OBJECT_NAME_NOT_FOUND otherwise.");

                #endregion
            }
            else if (!accountRights.Contains(LsadUtilities.Privilege1))
            {
                #region MS-LSAD_R518

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.NoSuchPrivilege,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    518,
                    @"For UserRights parameter of LsarRemoveAccountRights method ,If server does not recognize 
                    any of the rights, server MUST return STATUS_NO_SUCH_PRIVILEGE.");

                #endregion
            }
            else
            {
                #region MS-LSAD_R523

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    523,
                    @"In  LsarRemoveAccountRights    method,if request is succesfully completed then server MUST 
                    return STATUS_SUCCESS.");

                #endregion

                ////Query the account rights for checking whether the Account object is removed or not. 
                _LSAPR_USER_RIGHT_SET? enumRights = new _LSAPR_USER_RIGHT_SET();
                NtStatus checkRightsNHandle = lsadClientStack.LsarEnumerateAccountRights(
                    this.objPolicyHandle.Value,
                    this.objAccountSid[0],
                    out enumRights);

                if (enumRights.Value.UserRights == null)
                {
                    #region MS-LSAD_R521

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.ObjectNameNotFound,
                        (uint)checkRightsNHandle,
                        "MS-LSAD",
                        521,
                        @"In UserRight parameter of LsarRemoveAccountRights  method ,If the resulting set of access 
                        rights and privileges is empty, the server MUST delete the account object from its database.");

                    #endregion
                }
            }

            if (this.htAccHandle.Count != uintHandle)
            {
                utilities.DeleteExistAccount();
                this.htAccHandle.Remove("AccHandle");
            }

            utilities.CreateExistAccount(this.uintOpenAccAccess);

            if (this.htAddAccRight.Count != uintHandle)
            {
                this.htAddAccRight.Remove("htAddAccRight");
            }

            return (ErrorStatus)this.uintMethodStatus;
        }

        #endregion LsarRemoveAccountRights

        #region EnumerateAccounts

        /// <summary>
        /// The EnumerateAccountsRequest method is invoked to enumerate the account request.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="enumerationContext">A pointer to a context value that is used to resume enumeration, 
        /// if necessary</param>
        public void EnumerateAccountsRequest(int handleInput, int enumerationContext)
        {
            uint? enumConxt = (uint)enumerationContext;
            uint maxLength = 1000;
            IntPtr? tempPolicyHandle = IntPtr.Zero;
            _LSAPR_ACCOUNT_ENUM_BUFFER? enumBuffer = new _LSAPR_ACCOUNT_ENUM_BUFFER();
            uint noOfAccounts = 0;
            uint? enumerateContext = (uint)enumerationContext;

            if (stPolicyInformation.PHandle != handleInput)
            {
                this.objPolicyHandle = utilities.AccountObjInvalidHandle();
            }

            #region Getting No of AccountObjects on Server

            this.uintMethodStatus = lsadClientStack.LsarOpenPolicy(
                utilities.ConversionfromStringtoushortArray(string.Empty),
                objectAttributes,
                ACCESS_MASK.MAXIMUM_ALLOWED,
                out tempPolicyHandle);

            this.uintMethodStatus = lsadClientStack.LsarEnumerateAccounts(
                tempPolicyHandle.Value,
                ref enumConxt,
                out enumBuffer,
                maxLength);

            this.uintMethodStatus = lsadClientStack.LsarClose(ref tempPolicyHandle);
            noOfAccounts = enumBuffer.Value.EntriesRead;

            #endregion

            this.uintMethodStatus = lsadClientStack.LsarEnumerateAccounts(
                this.objPolicyHandle.Value,
                ref enumerateContext,
                out enumBuffer, 
                maxLength);

            if (stPolicyInformation.PHandle != handleInput)
            {
                EnumerateAccounts(handleInput, enumerateResponse.EnumerateNone);

                #region MS-LSAD_R442

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidHandle,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    442,
                    @"If the PolicyHandle parameter of LsarEnumerateAccounts method is not a valid context handle to 
                    policy object, the server MUST return STATUS_INVALID_HANDLE.");

                #endregion
            }
            else if ((uintAccessMask & ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION) != 
                ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION)
            {
                EnumerateAccounts(handleInput, enumerateResponse.EnumerateNone);

                #region MS-LSAD_R443

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.AccessDenied,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    443,
                    @"In LsarEnumerateAccounts method,server MUST verify that the caller has 
                    POLICY_VIEW_LOCAL_INFORMATION rights, and fail the request with
                    STATUS_ACCESS_DENIED if it does not.");

                #endregion
            }
            else if (enumerationContext > noOfAccounts)
            {
                EnumerateAccounts(handleInput, enumerateResponse.EnumerateNone);

                #region MS-LSAD_R447

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.NoMoreEntries,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    447,
                    @"In EnumerationContext parameter of LsarEnumerateAccounts  method ,If the enumeration is 
                    finished and there are no more entries to be returned, the server MUST return the status code 
                    STATUS_NO_MORE_ENTRIES and set EnumerationContext to a number such that enumeration would not 
                    continue if the method was called again with that value of EnumerationContext. ");

                #endregion

                #region MS-LSAD_R448

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.NoMoreEntries,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    448,
                    @"In EnumerationContext parameter of LsarEnumerateAccounts method,If EnumerationContext 
                    supplied by the caller is such that enumeration cannot continue, the server MUST return 
                    STATUS_NO_MORE_ENTRIES.");

                #endregion
            }
            else if (enumerationContext <= noOfAccounts)
            {
                EnumerateAccounts(handleInput, enumerateResponse.EnumerateAll);

                if ((enumBuffer.Value.EntriesRead != uintEntries) && (enumBuffer.Value.Information != null))
                {
                    #region MS-LSAD_R47

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.Success,
                        (uint)this.uintMethodStatus,
                        "MS-LSAD",
                        47,
                        @"In LSAPR_ACCOUNT_ENUM_BUFFER structure ,If the EntriesRead field has a value other than 0, 
                        the Information field MUST NOT be NULL.");

                    #endregion

                    #region MS-LSAD_R932

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.Success,
                        (uint)this.uintMethodStatus,
                        "MS-LSAD",
                        932,
                        @"For LSAPR_ACCOUNT_ENUM_BUFFER, if EntriesRead is not 0, Information MUST NOT be NULL.");

                    #endregion
                }

                #region MS-LSAD_R452
                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)this.uintMethodStatus,
                    "MS-LSAD",
                    452,
                    @"In LsarEnumerateAccounts  method,if request is succesfully completed then server MUST 
                    return STATUS_SUCCESS.");

                #endregion
            }
        }

        #endregion EnumerateAccounts
         
        #endregion Account_Objects
    }
}