// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk;
    using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa;
    using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
    using Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc;
    using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

    /// <summary>
    /// Implement methods of interface ILsadManagedAdapter.
    /// </summary>
    public partial class LsadManagedAdapter
    {
        #region Policy Variables

        //// Variables used in policy objects

        /// <summary>
        /// Check if the OS is windows
        /// </summary>
        private bool isWindows;

        /// <summary>
        /// check if the parameter is invalid 
        /// </summary>
        private bool invalidParam;

        /// <summary>
        /// check if the handle is invalid 
        /// </summary>
        private bool Invalidhandlle;

        /// <summary>
        /// check if the policyInfoquery is valid
        /// </summary>
        private bool validPolicyInfoquery;

        /// <summary>
        /// check if the domaininfoquery is valid
        /// </summary>
        private bool validdomaininfoquery;

        /// <summary>
        /// the lsadUUTD
        /// </summary>
        private string lsadUUID;

        /// <summary>
        /// the lsadendpoint
        /// </summary>
        private string lsadendPoint;

        /// <summary>
        /// the lsadProtocolSequence
        /// </summary>
        private string lsadProtocolSequence;

        /// <summary>
        /// set the InvalidPolicyHandle as readonly
        /// </summary>
        private IntPtr? InvalidPolicyHandle = IntPtr.Zero;

        /// <summary>
        /// set the validPolicyhandle as readOnly
        /// </summary>
        private IntPtr? ValidPolicyHandle = IntPtr.Zero;

        /// <summary>
        /// The access rights to grant an object.
        /// </summary>
        private ACCESS_MASK uintAccessMask;

        /// <summary>
        /// an _LSAPR_OBJECT_ATTRIBUTES structure variable
        /// </summary>
        private _LSAPR_OBJECT_ATTRIBUTES objectAttributes = new _LSAPR_OBJECT_ATTRIBUTES();

        /// <summary>
        /// set the server to windows2k8
        /// </summary>
        private Server serverPlatform = Server.Windows2k8;

        /// <summary>
        /// structure used to store the policy handle and the Access_Mask for Handle.
        /// </summary>
        internal struct stPolicyInformation
        {
            /// <summary>
            /// Variables to store the policy handle.
            /// </summary>
            internal static uint PHandle;

            /// <summary>
            /// Variables used to store the Access_Mask for Handle.
            /// </summary>
            internal static ACCESS_MASK AccessforHandle;
        };

        /// <summary>
        /// Variables used to store the policy Handle.
        /// </summary>
        internal static IntPtr? PolicyHandle = IntPtr.Zero;

        /// <summary>
        /// the const lsad UUTD
        /// </summary>
        private const string constLsadUUID = "12345778-1234-ABCD-EF00-0123456789AB";

        /// <summary>
        /// The const lsad endPoint
        /// </summary>
        private const string constLsadendPoint = "\\PIPE\\lsarpc";

        /// <summary>
        /// the const lsad protocolSequence
        /// </summary>
        private const string constLsadProtocolSequence = "ncacn_np";

        #endregion Policy Variables

        #region Policy_Objects

        #region OpenPolicy2

        /// <summary>
        /// This method is used to open a policy handle with required access.
        /// </summary>
        /// <param name="rootDirectory">Contains Null value or Non-Null Value </param>
        /// <param name="desiredAccess">Contains the access to be given to the policyHandle</param>
        /// <param name="policyHandle">Output Parameter which contains Valid or Invalid or Null value</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller is anonymous 
        /// and the Server is a non-DomainController</returns>        
        public ErrorStatus OpenPolicy2(
            RootDirectory rootDirectory,
            UInt32 desiredAccess,
            out Handle policyHandle)
        {
            _LSAPR_OBJECT_ATTRIBUTES objectAttributesForOpenPolicy2 = new _LSAPR_OBJECT_ATTRIBUTES();

            objectAttributesForOpenPolicy2.RootDirectory = new byte[1];
            uintAccess = ACCESS_MASK.MAXIMUM_ALLOWED;
            this.uintAccessMask = (ACCESS_MASK)desiredAccess;

            if (rootDirectory == RootDirectory.Null)
            {
                objectAttributesForOpenPolicy2.RootDirectory = null;
            }
            else
            {
                objectAttributesForOpenPolicy2.RootDirectory = 
                    new byte[1] { Convert.ToByte(LsadUtilities.INVALID_ROOTDIRECTORY) };
            }
                       
            uintMethodStatus = lsadClientStack.LsarOpenPolicy2(
                strServerName,
                objectAttributesForOpenPolicy2,
                this.uintAccessMask,
                out PolicyHandle);

            this.ValidPolicyHandle = PolicyHandle;
            objPolicyHandle = PolicyHandle;
            closePolicyHandle = PolicyHandle;

            if (objectAttributesForOpenPolicy2.RootDirectory != null)
            {
                stPolicyInformation.PHandle = LsadUtilities.INVALID_HANDLE;
                stPolicyInformation.AccessforHandle = this.uintAccessMask;

                #region MS-LSAD_R239

                Site.CaptureRequirementIfAreEqual<uint>(
                    ((uint)ErrorStatus.InvalidParameter),
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    239,
                    @"In LsarOpenPolicy2 ,One of the supplied parameters is incorrect. For instance, 
                    this can happen when either ObjectAttributes of DesiredAccess is NULL, or when the 
                    object SID in ObjectAttributes is NULL.then return values that an implementation
                    MUST return is STATUS_INVALID_PARAMETER.");

                #endregion

                if (this.isWindows)
                {
                    #region MS-LSAD_R231

                    Site.CaptureRequirementIfAreEqual<uint>(
                        ((uint)ErrorStatus.InvalidParameter),
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        231,
                        @"<50> Section 3.1.4.4.1: The Windows RPC server for LSAD protocol ignores 
                        ObjectAttributes parameter of  LsarOpenPolicy2 except for the RootDirectory field. 
                        It verifies whether the value is NULL, and returns STATUS_INVALID_PARAMETER if it is not. ");

                    #endregion
                }
            }
            else if ((desiredAccess & LsadUtilities.securityDsrcrAccessCheck) != 0)
            {
                stPolicyInformation.PHandle = LsadUtilities.INVALID_HANDLE;
                stPolicyInformation.AccessforHandle = this.uintAccessMask;

                #region MS-LSAD_R234

                Site.CaptureRequirementIfAreEqual<uint>(
                    ((uint)ErrorStatus.AccessDenied),
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    234,
                    @"If the requested access cannot be granted to the caller, the LsarOpenPolicy2 call MUST 
                    fail with STATUS_ACCESS_DENIED.");

                #endregion
            }
            else if (objectAttributesForOpenPolicy2.RootDirectory == null
                         && (desiredAccess & LsadUtilities.securityDsrcrAccessCheck) == 0)
            {
                stPolicyInformation.PHandle = LsadUtilities.INITIALISED_HANDLE;
                stPolicyInformation.AccessforHandle = this.uintAccessMask;
                validPolicyHandle = PolicyHandle;

                #region MS-LSAD_R238

                Site.CaptureRequirementIfAreEqual<uint>(
                    ((uint)ErrorStatus.Success),
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    238,
                    @"In LsarOpenPolicy2  interface of  LSAD Remote Protocol: On success it MUST return 
                    (0x00000000) STATUS_Success.");

                #endregion

                #region MS-LSAD_R237

                Site.CaptureRequirementIfIsTrue(
                    ((uint)uintMethodStatus == ((uint)ErrorStatus.Success)
                                                && Convert.ToBoolean(PolicyHandle != IntPtr.Zero)),
                    "MS-LSAD",
                    237,
                    @"PolicyHandle of LsarOpenPolicy2 MUST be used to return a handle to the policy 
                    object back to the caller if the request is successful. ");

                #endregion
            }

            policyHandle = (IntPtr.Zero == PolicyHandle) ? Handle.Invalid : Handle.Valid;

            if (stPolicyInformation.AccessforHandle == (ACCESS_MASK)desiredAccess && policyHandle == Handle.Valid)
            {
                #region MS-LSAD_R232

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    232,
                    @"DesiredAccess is an ACCESS_MASK value in LsarOpenPolicy2 that specifies the requested access 
                    rights that MUST be granted on the returned PolicyHandle, if the request is successful.");

                #endregion

                #region MS-LSAD_R236

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    236,
                    @"The context created by the implementation that is referenced by PolicyHandle on return MUST 
                    contain the access granted by the server implementation as a result of validating the DesiredAccess 
                    in Opnum LsarOpenPolicy2.");

                #endregion
            }

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion OpenPolicy2

        #region OpenPolicy

        /// <summary>
        /// This method is used to open a policy handle with required access.
        /// </summary>
        /// <param name="rootDirectory">Contains Null value or Non-Null Value </param>
        /// <param name="desiredAccess">Contains the access to be given to the policyHandle</param>
        /// <param name="policyHandle">Output Parameter which contains Valid or Invalid or Null value</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller is anonymous 
        /// and the Server is a non-DomainController</returns>      
        public ErrorStatus OpenPolicy(
            RootDirectory rootDirectory,
            UInt32 desiredAccess,
            out Handle policyHandle)
        {
            _LSAPR_OBJECT_ATTRIBUTES objectAttributesForOpenPolicy = new _LSAPR_OBJECT_ATTRIBUTES();

            PolicyHandle = IntPtr.Zero;
            objectAttributesForOpenPolicy.RootDirectory = new byte[1];
            uintAccess = (ACCESS_MASK)desiredAccess;
            this.uintAccessMask = (ACCESS_MASK)desiredAccess;

            if (rootDirectory == RootDirectory.Null)
            {
                objectAttributesForOpenPolicy.RootDirectory = null;
            }
            else
            {
                objectAttributesForOpenPolicy.RootDirectory = 
                    new byte[1] { Convert.ToByte(LsadUtilities.INVALID_ROOTDIRECTORY) };
            }
                        
            uintMethodStatus = lsadClientStack.LsarOpenPolicy(
                serverName,
                objectAttributesForOpenPolicy, 
                this.uintAccessMask, 
                out PolicyHandle);

            this.ValidPolicyHandle = PolicyHandle;
            objPolicyHandle = PolicyHandle;
            closePolicyHandle = PolicyHandle;

            if (objectAttributesForOpenPolicy.RootDirectory != null)
            {
                stPolicyInformation.PHandle = LsadUtilities.INVALID_HANDLE;
                stPolicyInformation.AccessforHandle = (ACCESS_MASK)this.uintAccessMask;

                if (this.isWindows)
                {
                    #region MS-LSAD_R243

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.InvalidParameter, 
                        (uint)uintMethodStatus, 
                        "MS-LSAD", 
                        243,
                        @"<51> Section 3.1.4.4.2: The Windows RPC server for LSAD protocol ignores 
                        ObjectAttributes parameter of LsarOpenPolicy except for the RootDirectory field.
                        It verifies whether the value is NULL, and returns STATUS_INVALID_PARAMETER if it is not.");
                    
                    #endregion
                }
            }
            else if (((uint)desiredAccess & LsadUtilities.securityDsrcrAccessCheck) != 0)
            {
                stPolicyInformation.PHandle = LsadUtilities.INVALID_HANDLE;
                stPolicyInformation.AccessforHandle = this.uintAccessMask;

                #region MS-LSAD_R246

                Site.CaptureRequirementIfAreEqual<uint>(
                    ((uint)ErrorStatus.AccessDenied), 
                    (uint)uintMethodStatus, 
                    "MS-LSAD", 
                    246, 
                    @"If the DesiredAccess of LsarOpenPolicy requested access cannot be granted to the caller, 
                    the call MUST fail with STATUS_ACCESS_DENIED.");
                
                #endregion
            }
            else if (objectAttributesForOpenPolicy.RootDirectory == null 
                         && (((uint)desiredAccess & LsadUtilities.securityDsrcrAccessCheck) == 0))
            {
                stPolicyInformation.PHandle = LsadUtilities.INITIALISED_HANDLE;
                stPolicyInformation.AccessforHandle = this.uintAccessMask;
                validPolicyHandle = PolicyHandle;

                if (this.lsadendPoint == constLsadendPoint)
                {
                    #region MS-LSAD_R2

                    Site.CaptureRequirementIfAreEqual<uint>(
                        ((uint)ErrorStatus.Success),
                        (uint)uintMethodStatus, 
                        "MS-LSAD", 
                        2, 
                        @"LSAD Remote Protocol MUST use '\\PIPE\\lsarpc' as the RPC endpoint when using RPC over SMB.");
                    
                    #endregion

                    #region MS-LSAD_R205

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.Success, 
                        (uint)uintMethodStatus, 
                        "MS-LSAD", 
                        205, 
                        @"The server MUST start listening on the well-known, named-pipe '\\PIPE\\lsarpc' 
                        for the RPC interface");
                    
                    #endregion
                }

                if (this.lsadUUID == constLsadUUID)
                {
                    #region MS-LSAD_R12

                    Site.CaptureRequirementIfAreEqual<uint>(
                        ((uint)ErrorStatus.Success), 
                        (uint)uintMethodStatus, 
                        "MS-LSAD",
                        12, 
                        @"LSAD Remote Protocol for the RPC interface MUST use the UUID as 
                        (12345778-1234-ABCD-EF00-0123456789AB). ");
                    
                    #endregion
                }

                if (this.lsadProtocolSequence == constLsadProtocolSequence)
                {
                    #region MS-LSAD_R1

                    Site.CaptureRequirementIfAreEqual<uint>(
                        ((uint)ErrorStatus.Success), 
                        (uint)uintMethodStatus, 
                        "MS-LSAD",
                        1,
                        @"Local Security Authority (Domain Policy) Remote Protocol MUST use Server Message Block (SMB) 
                        RPC protocol sequences.");
                    
                    #endregion
                }

                #region MS-LSAD_R886

                Site.CaptureRequirementIfIsTrue(
                    (((uint)uintMethodStatus == (uint)ErrorStatus.Success) 
                                                && (objectAttributesForOpenPolicy.RootDirectory == null)),
                    "MS-LSAD",
                    886,
                    @"For LSAPR_OBJECT_ATTRIBUTES, RootDirectory MUST be NULL.");
                
                #endregion

                #region MS-LSAD_R250

                Site.CaptureRequirementIfAreEqual<uint>(
                    ((uint)ErrorStatus.Success),
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    250, 
                    @"In LsarOpenPolicy  interface of  LSAD Remote Protocol: On success it MUST return 
                    (0x00000000) STATUS_Success.");
                
                #endregion

                #region MS-LSAD_R249

                Site.CaptureRequirementIfIsTrue(
                    ((uint)uintMethodStatus == ((uint)ErrorStatus.Success) 
                                            && Convert.ToBoolean(PolicyHandle != IntPtr.Zero)),
                    "MS-LSAD", 
                    249,
                    @"PolicyHandle of LsarOpenPolicy MUST be used to return a handle to the policy 
                    object back to the caller if the request is successful. ");
                
                #endregion
            }

            policyHandle = (IntPtr.Zero == PolicyHandle) ? Handle.Invalid : Handle.Valid;
            if (stPolicyInformation.AccessforHandle == (ACCESS_MASK)desiredAccess 
                    && policyHandle == Handle.Valid)
            {
                #region MS-LSAD_R244

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)uintMethodStatus, 
                    "MS-LSAD", 
                    244, 
                    @"DesiredAccess is an ACCESS_MASK value in LsarOpenPolicy that specifies the requested access 
                    rights that MUST be granted on the returned PolicyHandle, if the request is successful.");
                
                #endregion

                #region MS-LSAD_R248

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success, 
                    (uint)uintMethodStatus, 
                    "MS-LSAD", 
                    248, 
                    @"The context created by the implementation that is referenced by PolicyHandle on return MUST 
                    contain the access granted by the server implementation as a result of validating the DesiredAccess 
                    in Opnum LsarOpenPolicy.");
                
                #endregion
            }

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion OpenPolicy

        #region SetInformationPolicy2

        /// <summary>
        /// This method is used to set policy object information
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="informationType">Contains the type of policy object information to be set</param>
        /// <returns>Returns Success if the method is successful
        ///          Returns InvalidParameter if the parameters passed to the method are not valid
        ///          Returns AccessDenied if the caller does not have the permissions 
        ///          to perform the operation
        ///          Returns InvalidHandle if the policy handle passed is not valid
        ///          Returns NotImplemented if the passed in information type cannot be set</returns>
        /// Disable warning CA1502, CA1505 and CA1506 because it will affect the implementation of Adapter and Model 
        /// codes if do any changes about maintainability.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity",
            Justification = "affect the implementation of Adapter and Model codes")]
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode",
            Justification = "affect the implementation of Adapter and Model codes")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", 
            Justification = "affect the implementation of Adapter and Model codes")]
        public ErrorStatus SetInformationPolicy2(int handleInput, InformationClass informationType)
        {
            InformationClass policyInformationClass2 = new InformationClass();
            _LSAPR_POLICY_INFORMATION policyInfoinServer = new _LSAPR_POLICY_INFORMATION();
            this.Invalidhandlle = false;
            this.invalidParam = false;

            #region CheckforInvalidParameter

            // Setting flag true for Information class which return Invalid Parameter
            if ((informationType == InformationClass.PolicyAuditFullQueryInformation)
                     || (informationType == InformationClass.PolicyAuditFullSetInformation)
                     || ((informationType == InformationClass.PolicyAccountDomainInformation) && (isDC == true))
                     || (informationType == InformationClass.PolicyPdAccountInformation)
                     || (informationType == InformationClass.PolicyModificationInformation)
                     || (informationType == InformationClass.Invalid))
            {
                this.invalidParam = true;
            }

            #endregion CheckforInvalidParameter

            #region CheckforInvalidHandleCase

            // Only check InvalidHandle in case of NOT InvalidParam
            if (stPolicyInformation.PHandle != handleInput && !this.invalidParam)
            {
                // CreateAnInvalidHandle() will create invalid handle to check requirement of Invalid Handle
                objSecretHandle = utilities.CreateAnInvalidHandle(false);
                this.InvalidPolicyHandle = objSecretHandle;
                PolicyHandle = this.InvalidPolicyHandle;
                this.Invalidhandlle = true;
            }
            else
            {
                PolicyHandle = this.ValidPolicyHandle;
                this.Invalidhandlle = false;
            }

            #endregion CheckforInvalidHandleCase

            #region checkInformationClassvalues

            // Passing Input to set Information as per Information class and setting intCheckInfoClass to return status
            switch (informationType)
            {
                case InformationClass.PolicyAccountDomainInformation:
                    policyInformationClass2 = InformationClass.PolicyAccountDomainInformation;
                    if (isDC == true)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnInvalidParameter;
                    }
                    else
                    {
                        policyInfoinServer.PolicyAccountDomainInfo = new _LSAPR_POLICY_ACCOUNT_DOM_INFO();
                        policyInfoinServer.PolicyAccountDomainInfo.DomainName = new _RPC_UNICODE_STRING();
                        policyInfoinServer.PolicyAccountDomainInfo.DomainName.Buffer =
                            utilities.ConversionfromStringtoushortArray(domain);
                        policyInfoinServer.PolicyAccountDomainInfo.DomainName.Length =
                            (ushort)(2 * (policyInfoinServer.PolicyAccountDomainInfo.DomainName.Buffer.Length));
                        policyInfoinServer.PolicyAccountDomainInfo.DomainName.MaximumLength =
                            (ushort)((policyInfoinServer.PolicyAccountDomainInfo.DomainName.Length)+2);
                        policyInfoinServer.PolicyAccountDomainInfo.DomainSid =
                            utilities.GetSid(DomainSid.Valid, GetAccountDomainSidNonDC);
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;
                case InformationClass.PolicyPrimaryDomainInformation:

                    policyInformationClass2 = InformationClass.PolicyPrimaryDomainInformation;
                    policyInfoinServer.PolicyPrimaryDomainInfo = new _LSAPR_POLICY_PRIMARY_DOM_INFO();
                    policyInfoinServer.PolicyPrimaryDomainInfo.Name = new _RPC_UNICODE_STRING();
                    policyInfoinServer.PolicyPrimaryDomainInfo.Name.Buffer =
                        utilities.ConversionfromStringtoushortArray(domain);
                    policyInfoinServer.PolicyPrimaryDomainInfo.Name.Length =
                        (ushort)(2 * policyInfoinServer.PolicyPrimaryDomainInfo.Name.Buffer.Length);
                    policyInfoinServer.PolicyPrimaryDomainInfo.Name.MaximumLength =
                        (ushort)((policyInfoinServer.PolicyPrimaryDomainInfo.Name.Length)+2);
                    policyInfoinServer.PolicyPrimaryDomainInfo.Sid =
                        utilities.GetSid(DomainSid.Valid, this.PrimaryDomainSID);

                    if (policyInfoinServer.PolicyPrimaryDomainInfo.Name.Length <= 30)
                    {
                        #region MS-LSAD_R894

                        Site.CaptureRequirement(
                            "MS-LSAD",
                            894,
                            @"For LSAPR_POLICY_PRIMARY_DOM_INFO, Name.Length MUST be less than or equal 30.");                        

                        #endregion                        
                    }

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_TRUST_ADMIN) != ACCESS_MASK.POLICY_TRUST_ADMIN)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyAuditEventsInformation:

                    policyInformationClass2 = InformationClass.PolicyAuditEventsInformation;
                    policyInfoinServer.PolicyAuditEventsInfo = new _LSAPR_POLICY_AUDIT_EVENTS_INFO();
                    policyInfoinServer.PolicyAuditEventsInfo.AuditingMode = (byte)AuditingMode;
                    policyInfoinServer.PolicyAuditEventsInfo.EventAuditingOptions = new EventAuditingOptions_Values[1];
                    policyInfoinServer.PolicyAuditEventsInfo.EventAuditingOptions[0] =
                        EventAuditingOptions_Values.POLICY_AUDIT_EVENT_SUCCESS;
                    policyInfoinServer.PolicyAuditEventsInfo.MaximumAuditEventCount = MaximumAuditEventCount;                  

                    if (MaximumAuditEventCount != 0)
                    {
                        #region MS-LSAD_R60

                        Site.CaptureRequirementIfIsNotNull(
                            policyInfoinServer.PolicyAuditEventsInfo.EventAuditingOptions,
                            "MS-LSAD",
                            60,
                            @"If the ""MaximumAuditingEventCount"" field of the 
                            LSAPR_POLICY_AUDIT_EVENTS_INFO structure has a value other than 0, the 
                            ""EventAuditingOptions"" field MUST NOT be NULL.");

                        #endregion

                        #region MS-LSAD_R888

                        Site.CaptureRequirementIfAreNotEqual<uint>(
                            0,
                            policyInfoinServer.PolicyAuditEventsInfo.MaximumAuditEventCount,
                            "MS-LSAD",
                            888,
                            @"For LSAPR_POLICY_AUDIT_EVENTS_INFO, MaximumAuditEventCount MUST NOT be 0.");                        

                        #endregion

                        #region MS-LSAD_R890

                        Site.CaptureRequirementIfIsNotNull(
                            policyInfoinServer.PolicyAuditEventsInfo.EventAuditingOptions,
                            "MS-LSAD",
                            890,
                            @"For LSAPR_POLICY_AUDIT_EVENTS_INFO, EventAuditingOptions MUST NOT be NULL.");                        

                        #endregion                       
                    }   

                    if (policyInfoinServer.PolicyAuditEventsInfo.MaximumAuditEventCount <= 8)
                    {   
                        #region MS-LSAD_R889

                        Site.CaptureRequirement(
                            "MS-LSAD",
                            889,
                            @"For LSAPR_POLICY_AUDIT_EVENTS_INFO, MaximumAuditEventCount
                            MUST be less than or equal to 8.");

                        #endregion
                    }

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_SET_AUDIT_REQUIREMENTS) != 
                        ACCESS_MASK.POLICY_SET_AUDIT_REQUIREMENTS)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyAuditFullQueryInformation:

                    policyInformationClass2 = InformationClass.PolicyAuditFullQueryInformation;
                    policyInfoinServer.PolicyAuditFullQueryInfo = new _POLICY_AUDIT_FULL_QUERY_INFO();
                    intCheckInfoClass = LsadUtilities.ReturnInvalidParameter;

                    break;

                case InformationClass.PolicyAuditFullSetInformation:

                    policyInformationClass2 = InformationClass.PolicyAuditFullSetInformation;
                    policyInfoinServer.PolicyAuditFullSetInfo = new _POLICY_AUDIT_FULL_SET_INFO();

                    if (this.isWindows)
                    {
                        if (PDCOSVersion >= ServerVersion.Win2003)
                        {
                            intCheckInfoClass = LsadUtilities.ReturnInvalidParameter;
                        }
                    }
                    else if ((this.uintAccessMask & ACCESS_MASK.POLICY_AUDIT_LOG_ADMIN) != 
                        ACCESS_MASK.POLICY_AUDIT_LOG_ADMIN)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }

                    break;

                case InformationClass.PolicyAuditLogInformation:

                    policyInformationClass2 = InformationClass.PolicyAuditLogInformation;
                    policyInfoinServer.PolicyAuditLogInfo = new _POLICY_AUDIT_LOG_INFO();

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_AUDIT_LOG_ADMIN) != 
                        ACCESS_MASK.POLICY_AUDIT_LOG_ADMIN)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnNotSupported;
                    }

                    break;

                case InformationClass.PolicyDnsDomainInformation:

                    policyInformationClass2 = InformationClass.PolicyDnsDomainInformation;
                    policyInfoinServer.PolicyDnsDomainInfo = new _LSAPR_POLICY_DNS_DOMAIN_INFO();

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_TRUST_ADMIN) != ACCESS_MASK.POLICY_TRUST_ADMIN)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        policyInfoinServer.PolicyDnsDomainInfo.Name = new _RPC_UNICODE_STRING();
                        policyInfoinServer.PolicyDnsDomainInfo.Name.Buffer =
                            utilities.ConversionfromStringtoushortArray(domain);
                        policyInfoinServer.PolicyDnsDomainInfo.Name.Length =
                            (ushort)(2 * (policyInfoinServer.PolicyDnsDomainInfo.Name.Buffer.Length));
                        policyInfoinServer.PolicyDnsDomainInfo.Name.MaximumLength =
                            (ushort)((policyInfoinServer.PolicyDnsDomainInfo.Name.Length)+2);
                        policyInfoinServer.PolicyDnsDomainInfo.DnsDomainName = new _RPC_UNICODE_STRING();
                        policyInfoinServer.PolicyDnsDomainInfo.DnsDomainName.Buffer =
                            utilities.ConversionfromStringtoushortArray(fullDomain);
                        policyInfoinServer.PolicyDnsDomainInfo.DnsDomainName.Length =
                            (ushort)(2 * (policyInfoinServer.PolicyDnsDomainInfo.DnsDomainName.Buffer.Length));
                        policyInfoinServer.PolicyDnsDomainInfo.DnsDomainName.MaximumLength =
                            (ushort)((policyInfoinServer.PolicyDnsDomainInfo.DnsDomainName.Length)+2);
                        policyInfoinServer.PolicyDnsDomainInfo.DnsForestName = new _RPC_UNICODE_STRING();
                        policyInfoinServer.PolicyDnsDomainInfo.DnsForestName.Buffer =
                            utilities.ConversionfromStringtoushortArray(fullDomain);
                        policyInfoinServer.PolicyDnsDomainInfo.DnsForestName.Length =
                            (ushort)(2 * (policyInfoinServer.PolicyDnsDomainInfo.DnsForestName.Buffer.Length));
                        policyInfoinServer.PolicyDnsDomainInfo.DnsForestName.MaximumLength =
                            (ushort)((policyInfoinServer.PolicyDnsDomainInfo.DnsForestName.Length)+2);
                        policyInfoinServer.PolicyDnsDomainInfo.DomainGuid =
                            new Guid(DomainGUID);
                        policyInfoinServer.PolicyDnsDomainInfo.Sid =
                            utilities.GetSid(DomainSid.Valid, this.PrimaryDomainSID);

                        #region MS-LSAD_R897
                        Site.CaptureRequirementIfIsTrue(
                            (policyInfoinServer.PolicyDnsDomainInfo.Name.Length <= 30),
                            "MS-LSAD",
                            897,
                            @"For LSAPR_POLICY_DNS_DOMAIN_INFO, Name.Length MUST be less than or equal to 30.");

                        #endregion
                        
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyDnsDomainInformationInt:

                    policyInformationClass2 = InformationClass.PolicyDnsDomainInformationInt;

                    policyInfoinServer.PolicyDnsDomainInfoInt = new _LSAPR_POLICY_DNS_DOMAIN_INFO();

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_TRUST_ADMIN) != ACCESS_MASK.POLICY_TRUST_ADMIN)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        policyInfoinServer.PolicyDnsDomainInfoInt.Name = new _RPC_UNICODE_STRING();
                        policyInfoinServer.PolicyDnsDomainInfoInt.Name.Buffer =
                            utilities.ConversionfromStringtoushortArray(domain);
                        policyInfoinServer.PolicyDnsDomainInfoInt.Name.Length =
                            (ushort)(2 * (policyInfoinServer.PolicyDnsDomainInfoInt.Name.Buffer.Length));
                        policyInfoinServer.PolicyDnsDomainInfoInt.Name.MaximumLength =
                            (ushort)((policyInfoinServer.PolicyDnsDomainInfoInt.Name.Length)+2);
                        policyInfoinServer.PolicyDnsDomainInfoInt.DnsDomainName = new _RPC_UNICODE_STRING();
                        policyInfoinServer.PolicyDnsDomainInfoInt.DnsDomainName.Buffer =
                            utilities.ConversionfromStringtoushortArray(fullDomain);
                        policyInfoinServer.PolicyDnsDomainInfoInt.DnsDomainName.Length =
                            (ushort)(2 * (policyInfoinServer.PolicyDnsDomainInfoInt.DnsDomainName.Buffer.Length));
                        policyInfoinServer.PolicyDnsDomainInfoInt.DnsDomainName.MaximumLength =
                            (ushort)((policyInfoinServer.PolicyDnsDomainInfoInt.DnsDomainName.Length)+2);
                        policyInfoinServer.PolicyDnsDomainInfoInt.DnsForestName = new _RPC_UNICODE_STRING();
                        policyInfoinServer.PolicyDnsDomainInfoInt.DnsForestName.Buffer =
                            utilities.ConversionfromStringtoushortArray(fullDomain);
                        policyInfoinServer.PolicyDnsDomainInfoInt.DnsForestName.Length =
                            (ushort)(2 * (policyInfoinServer.PolicyDnsDomainInfoInt.DnsForestName.Buffer.Length));
                        policyInfoinServer.PolicyDnsDomainInfoInt.DnsForestName.MaximumLength =
                            (ushort)((policyInfoinServer.PolicyDnsDomainInfoInt.DnsForestName.Length)+2);
                        policyInfoinServer.PolicyDnsDomainInfoInt.DomainGuid =
                            new Guid(DomainGUID);

                        policyInfoinServer.PolicyDnsDomainInfoInt.Sid =
                            utilities.GetSid(DomainSid.Valid, this.PrimaryDomainSID);

                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyLocalAccountDomainInformation:

                    policyInformationClass2 = InformationClass.PolicyLocalAccountDomainInformation;
                    policyInfoinServer.PolicyLocalAccountDomainInfo = new _LSAPR_POLICY_ACCOUNT_DOM_INFO();
                    policyInfoinServer.PolicyLocalAccountDomainInfo.DomainName = new _RPC_UNICODE_STRING();
                    policyInfoinServer.PolicyLocalAccountDomainInfo.DomainName.Buffer =
                        utilities.ConversionfromStringtoushortArray(domain);
                    policyInfoinServer.PolicyLocalAccountDomainInfo.DomainName.Length =
                        (ushort)(2 * policyInfoinServer.PolicyLocalAccountDomainInfo.DomainName.Buffer.Length);
                    policyInfoinServer.PolicyLocalAccountDomainInfo.DomainName.MaximumLength =
                        (ushort)((policyInfoinServer.PolicyLocalAccountDomainInfo.DomainName.Length)+2);
                    policyInfoinServer.PolicyLocalAccountDomainInfo.DomainSid =
                        utilities.GetSid(DomainSid.Valid, GetLocalAccountDomainSid);

                    if (this.serverPlatform == Server.Windows2k8)
                    {
                        if ((this.uintAccessMask & ACCESS_MASK.POLICY_TRUST_ADMIN) != ACCESS_MASK.POLICY_TRUST_ADMIN)
                        {
                            intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                        }
                        else
                        {
                            intCheckInfoClass = LsadUtilities.ReturnSuccess;
                        }
                    }

                    break;

                case InformationClass.PolicyLsaServerRoleInformation:

                    policyInformationClass2 = InformationClass.PolicyLsaServerRoleInformation;
                    policyInfoinServer.PolicyServerRoleInfo = new _POLICY_LSA_SERVER_ROLE_INFO();

                    if (GetIntProperty(propertyGroup + "ServerRoleSelected") == 3)
                    {
                        policyInfoinServer.PolicyServerRoleInfo.LsaServerRole =
                            _POLICY_LSA_SERVER_ROLE.PolicyServerRolePrimary;
                    }

                    if (GetIntProperty(propertyGroup + "ServerRoleSelected") == 2)
                    {
                        policyInfoinServer.PolicyServerRoleInfo.LsaServerRole =
                            _POLICY_LSA_SERVER_ROLE.PolicyServerRoleBackup;
                    }

                    #region MS-LSAD_R902

                    Site.CaptureRequirementIfIsTrue(
                        (((uint)policyInfoinServer.PolicyServerRoleInfo.LsaServerRole == 2)
                            || ((uint)policyInfoinServer.PolicyServerRoleInfo.LsaServerRole == 3)),
                            "MS-LSAD",
                            902,
                            "For POLICY_LSA_SERVER_ROLE_INFO, LsaServerRole MUST be 2 OR 3.");

                    #endregion

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_SERVER_ADMIN) != ACCESS_MASK.POLICY_SERVER_ADMIN)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyModificationInformation:

                    policyInformationClass2 = InformationClass.PolicyModificationInformation;
                    policyInfoinServer.PolicyModificationInfo = new _POLICY_MODIFICATION_INFO();
                    intCheckInfoClass = LsadUtilities.ReturnInvalidParameter;

                    break;

                case InformationClass.PolicyPdAccountInformation:

                    policyInformationClass2 = InformationClass.PolicyPdAccountInformation;
                    policyInfoinServer.PolicyPdAccountInfo = new _LSAPR_POLICY_PD_ACCOUNT_INFO();
                    intCheckInfoClass = LsadUtilities.ReturnInvalidParameter;

                    break;

                case InformationClass.PolicyReplicaSourceInformation:

                    policyInformationClass2 = InformationClass.PolicyReplicaSourceInformation;
                    policyInfoinServer.PolicyReplicaSourceInfo = new _LSAPR_POLICY_REPLICA_SRCE_INFO();
                    policyInfoinServer.PolicyReplicaSourceInfo.ReplicaAccountName = new _RPC_UNICODE_STRING();
                    policyInfoinServer.PolicyReplicaSourceInfo.ReplicaAccountName.Buffer =
                        utilities.ConversionfromStringtoushortArray(ReplicasourceName);
                    policyInfoinServer.PolicyReplicaSourceInfo.ReplicaAccountName.Length =
                        (ushort)(2 * policyInfoinServer.PolicyReplicaSourceInfo.ReplicaAccountName.Buffer.Length);
                    policyInfoinServer.PolicyReplicaSourceInfo.ReplicaAccountName.MaximumLength =
                        (ushort)((policyInfoinServer.PolicyReplicaSourceInfo.ReplicaAccountName.Length)+2);
                    policyInfoinServer.PolicyReplicaSourceInfo.ReplicaSource = new _RPC_UNICODE_STRING();
                    policyInfoinServer.PolicyReplicaSourceInfo.ReplicaSource.Buffer =
                        utilities.ConversionfromStringtoushortArray(ReplicaAccountName);
                    policyInfoinServer.PolicyReplicaSourceInfo.ReplicaSource.Length =
                        (ushort)(2 * policyInfoinServer.PolicyReplicaSourceInfo.ReplicaSource.Buffer.Length);
                    policyInfoinServer.PolicyReplicaSourceInfo.ReplicaSource.MaximumLength =
                        (ushort)((policyInfoinServer.PolicyReplicaSourceInfo.ReplicaSource.Length)+2);

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_SERVER_ADMIN) != ACCESS_MASK.POLICY_SERVER_ADMIN)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyMachineAccountInformation:
                    policyInformationClass2 = InformationClass.PolicyMachineAccountInformation;
                    policyInfoinServer.PolicyMachineAccountInfo = new _LSAPR_POLICY_MACHINE_ACCT_INFO();
                    policyInfoinServer.PolicyMachineAccountInfo.Rid = 0;
                    policyInfoinServer.PolicyMachineAccountInfo.Sid = utilities.GetSid(DomainSid.Valid, GetLocalAccountDomainSid);
                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_SERVER_ADMIN) != ACCESS_MASK.POLICY_SERVER_ADMIN)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }
                    break;

                default:

                    #region invalidenumcheck

                    intCheckInfoClass = LsadUtilities.ReturnInvalidParameter;

                    #endregion

                    break;
            }

            #endregion checkInformationClassvalues

            uintMethodStatus = lsadClientStack.LsarSetInformationPolicy2(
                PolicyHandle.Value,
                (_POLICY_INFORMATION_CLASS)policyInformationClass2,
                policyInfoinServer);

            //Capturing Invalid Parameter Requirements based on different Information class
            if (this.invalidParam == true)
            {
                if (informationType == InformationClass.PolicyAccountDomainInformation)
                {
                    #region MS-LSAD_R336

                    Site.CaptureRequirementIfAreEqual<uint>(
                        ((uint)ErrorStatus.InvalidParameter),
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        336,
                        @"If InformationClass value of LsarSetInformationPolicy2  is PolicyAccountDomainInformation
                        then the request MUST fail with STATUS_INVALID_PARAMETER.");

                    #endregion

                    ////For domain Controller 
                    if (isDC == true)
                    {
                        #region MS-LSAD_R351

                        Site.CaptureRequirementIfAreEqual<uint>(
                            ((uint)ErrorStatus.InvalidParameter),
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            351,
                            @"If the Value of InformationClass parameter in LsarSetInformationPolicy2 is 
                            PolicyAccountDomainInformation then on domain controller, server MUST fail the 
                            request with STATUS_INVALID_PARAMETER.");

                        #endregion
                    }
                }

                if (informationType == InformationClass.PolicyAuditFullQueryInformation)
                {
                    #region MS-LSAD_R342

                    Site.CaptureRequirementIfAreEqual<uint>(
                        ((uint)ErrorStatus.InvalidParameter),
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        342,
                        @"If InformationClass value of LsarSetInformationPolicy2  is PolicyAuditFullQueryInformation 
                        then the request MUST fail with STATUS_INVALID_PARAMETER.");

                    #endregion

                    #region MS-LSAD_R358

                    Site.CaptureRequirementIfAreEqual<uint>(
                        ((uint)ErrorStatus.InvalidParameter),
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        358,
                        @"If the Value of InformationClass parameter in LsarSetInformationPolicy2 is 
                        PolicyAuditFullQueryInformation then Server MUST return STATUS_INVALID_PARAMETER 
                        because this is not a policy element that can be set.");

                    #endregion
                }

                if (informationType == InformationClass.PolicyModificationInformation)
                {
                    #region MS-LSAD_R340

                    Site.CaptureRequirementIfAreEqual<uint>(
                        ((uint)ErrorStatus.InvalidParameter),
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        340,
                        @"If InformationClass value of LsarSetInformationPolicy2  is PolicyModificationInformation 
                        then the request MUST fail with STATUS_INVALID_PARAMETER.");

                    #endregion

                    #region MS-LSAD_R356

                    Site.CaptureRequirementIfAreEqual<uint>(
                        ((uint)ErrorStatus.InvalidParameter),
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        356,
                        @"If the Value of InformationClass parameter in LsarSetInformationPolicy2 is 
                        PolicyModificationInformation then Server MUST return STATUS_INVALID_PARAMETER 
                        because this is not a policy element that can be set.");

                    #endregion
                }

                if (informationType == InformationClass.PolicyPdAccountInformation)
                {
                    #region MS-LSAD_R335

                    Site.CaptureRequirementIfAreEqual<uint>(
                        ((uint)ErrorStatus.InvalidParameter),
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        335,
                        @"If InformationClass value of LsarSetInformationPolicy2  is PolicyPdAccountInformation then
                        the request MUST fail with STATUS_InvalidParameter.");

                    #endregion

                    #region MS-LSAD_R350

                    Site.CaptureRequirementIfAreEqual<uint>(
                        ((uint)ErrorStatus.InvalidParameter),
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        350,
                        @"If the Value of InformationClass parameter in LsarSetInformationPolicy2 is
                        PolicyPdAccountInformation then Server MUST return STATUS_InvalidParameter 
                        because this is not a policy element that can be set.");

                    #endregion
                }

                ////Capturing Invalid Handle Requirements
                if (informationType == InformationClass.PolicyAuditFullSetInformation)
                {
                    #region MS-LSAD_R357

                    if (PDCOSVersion >= ServerVersion.Win2003)     
                    {
                        Site.CaptureRequirementIfAreEqual<uint>(
                            ((uint)ErrorStatus.InvalidParameter),
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            357,
                            @"[If InformationClass value of LsarSetInformationPolicy  is PolicyAuditFullSetInformation]
                            <54> Section 3.1.4.4.5: Windows XP, Windows Server 2003, Windows Vista, Windows Server 2008, 
                            Windows 7, and Windows Server 2008 R2 return STATUS_INVALID_PARAMETER for this information
                            class.");
                    }

                    #endregion

                    //// Add the comment information including the location information.
                    Site.Log.Add(
                        LogEntryKind.Comment,
                        @"Verify MS-LSAD_R1039: The Expect value is 0, The Actual value is :{0}.",
                        policyInfoinServer.PolicyAuditFullSetInfo.ShutDownOnFull);

                    #region MS-LSAD_R1039

                    // Verify requirement 1039
                    Site.CaptureRequirementIfAreEqual<byte>(
                        0,
                        policyInfoinServer.PolicyAuditFullSetInfo.ShutDownOnFull,
                        "MS-LSAD",
                        1039,
                        @"If the Value of InformationClass parameter in LsarSetInformationPolicy2 
                        is PolicyAuditFullSetInformation then server MUST update its abstract data model 
                        as ShutDownOnFull field of Audit Full Information.<54>");

                    #endregion                                      
                }
            }
            else if (this.Invalidhandlle == true)
            {
                utilities.CreateAnInvalidHandle(true);

                #region MS-LSAD_R330

                Site.CaptureRequirementIfAreEqual<uint>(
                    ((uint)ErrorStatus.InvalidHandle),
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    330,
                    @"If PolicyHandle of LsarSetInformationPolicy2 is not a valid context handle,
                    the server MUST return STATUS_INVALID_HANDLE.");

                #endregion
            }

            //Validating Requirement for Success, Not Implemented and Access Denied Status
            if (!this.Invalidhandlle)
            {
                #region Capturing_Requirement for Status other than Invalid Handle

                switch (intCheckInfoClass)
                {
                    ////Validating Success Requirements
                    case 0:

                        #region MS-LSAD_R326

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.Success,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            326,
                            @"The LsarSetInformationPolicy2  implementation MUST return STATUS_SUCCESS(0x00000000) 
                            when the request was successfully completed.");

                        #endregion

                        #region MS-LSAD_R329

                        Site.CaptureRequirementIfIsTrue(
                            ((PolicyHandle != IntPtr.Zero) && (uintMethodStatus != NtStatus.STATUS_ACCESS_DENIED)),
                            "MS-LSAD",
                            329,
                            @"PolicyHandle parameter of LsarSetInformationPolicy2 MUST reference a context that was 
                            granted an access commensurate with the InformationClass value requested.");

                        #endregion

                        break;

                    ////Capturing Requirement for general Invalid Parameter case
                    case 1:

                        #region MS-LSAD_R327

                        Site.CaptureRequirementIfAreEqual<uint>(
                            ((uint)ErrorStatus.InvalidParameter),
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            327,
                            @"The LsarSetInformationPolicy2   implementation MUST return 
                            STATUS_InvalidParameter(oxC000000D) when one of the parameters is incorrect.
                            For instance, this can happen if InformationClass is not supported or some of 
                            the supplied policy data is inValid.");

                        #endregion

                        break;

                    ////Capturing Requirement for Not_Implemented status
                    case 2:

                        #region MS-LSAD_R347

                        Site.CaptureRequirementIfAreEqual<uint>(
                            ((uint)ErrorStatus.NotImplemented),
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            347,
                            @"If the Value of InformationClass parameter in LsarSetInformationPolicy2 is 
                            PolicyAuditLogInformation then Server MUST return the STATUS_NOT_IMPLEMENTED error code
                            because this is not a policy element that can be set.");

                        #endregion

                        #region MS-LSAD_R328

                        Site.CaptureRequirementIfAreEqual<uint>(
                            ((uint)ErrorStatus.NotImplemented),
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            328,
                            @"The LsarSetInformationPolicy2  implementation MUST return  
                            STATUS_NOT_IMPLEMENTED(0xC0000002) when PolicyAuditLogInformation 
                            information class cannot be set.");

                        #endregion

                        break;

                    ////Capturing Requirement for Access denied status
                    case 3:
                        if (informationType == InformationClass.PolicyPrimaryDomainInformation)
                        {
                            #region MS-LSAD_R334

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                334,
                                @"If InformationClass value of LsarSetInformationPolicy2 is 
                                PolicyPrimaryDomainInformation then type of access required
                                MUST be POLICY_TRUST_ADMIN");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyAuditEventsInformation)
                        {
                            #region MS-LSAD_R333

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                333,
                                @"If InformationClass value of LsarSetInformationPolicy2 is 
                                PolicyAuditEventsInformation then type of access required MUST 
                                be POLICY_SET_AUDIT_REQUIREMENTS");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyAuditFullSetInformation)
                        {
                            #region MS-LSAD_R341

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                341,
                                @"If InformationClass value of LsarSetInformationPolicy2 is
                                PolicyAuditFullSetInformation then type of access required MUST 
                                be POLICY_AUDIT_LOG_ADMIN");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyAuditLogInformation)
                        {
                            #region MS-LSAD_R332

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                332,
                                @"If InformationClass value of LsarSetInformationPolicy2 is PolicyAuditLogInformation 
                                then type of access required MUST be POLICY_AUDIT_LOG_ADMIN");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyDnsDomainInformation)
                        {
                            #region MS-LSAD_R343

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                343,
                                @"If InformationClass value of LsarSetInformationPolicy2  is PolicyDnsDomainInformation
                                then type of access required MUST be POLICY_TRUST_ADMIN.");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyDnsDomainInformationInt)
                        {
                            #region MS-LSAD_R344

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                344,
                                @"If InformationClass value of LsarSetInformationPolicy2 is 
                                PolicyDnsDomainInformationInt then type of access required MUST 
                                be POLICY_TRUST_ADMIN.");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyLocalAccountDomainInformation)
                        {
                            if (this.serverPlatform == Server.Windows2k8)
                            {
                                #region MS-LSAD_R345

                                Site.CaptureRequirementIfAreEqual<uint>(
                                    ((uint)ErrorStatus.AccessDenied),
                                    (uint)uintMethodStatus,
                                    "MS-LSAD",
                                    345,
                                    @"If InformationClass value of LsarSetInformationPolicy2 is 
                                    PolicyLocalAccountDomainInformation then type of access required MUST
                                    be POLICY_TRUST_ADMIN.");

                                #endregion
                            }
                        }

                        if (informationType == InformationClass.PolicyLsaServerRoleInformation)
                        {
                            #region MS-LSAD_R337

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                337,
                                @"If InformationClass value of LsarSetInformationPolicy2 is 
                                PolicyLsaServerRoleInformation then type of access required MUST
                                be POLICY_SERVER_ADMIN");

                            #endregion
                        }

                        #region MS-LSAD_R331

                        Site.CaptureRequirementIfAreEqual<uint>(
                            ((uint)ErrorStatus.AccessDenied),
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            331,
                            @"If the context of LsarSetInformationPolicy2 does not have sufficient access, 
                            the server MUST return STATUS_AccessDenied.");

                        #endregion

                        break;

                    default:

                        break;
                }

                #endregion Capturing_Requirement for Status other than Invalid Handle
            }

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion SetInformationPolicy2

        #region SetInformationPolicy

        /// <summary>
        /// This method is used to set policy object information
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="informationType">Contains the type of policy object information to be set</param>
        /// <returns>Returns Success if the method is successful
        ///          Returns InvalidParameter if the parameters passed to the method are not valid
        ///          Returns AccessDenied if the caller does not have the permissions 
        ///          to perform the operation
        ///          Returns InvalidHandle if the policy handle passed is not valid
        ///          Returns NotImplemented if the passed in information type cannot be set</returns>
        /// Disable warning CA1502, CA1505 and CA1506 because it will affect the implementation of Adapter and Model 
        /// codes if do any changes about maintainability.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", 
            Justification = "affect the implementation of Adapter and Model codes")]
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", 
            Justification = "affect the implementation of Adapter and Model codes")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", 
            Justification = "affect the implementation of Adapter and Model codes")]
        public ErrorStatus SetInformationPolicy(int handleInput, InformationClass informationType)
        {
            InformationClass policyInformationClass2 = new InformationClass();
            _LSAPR_POLICY_INFORMATION policyInfoinServer = new _LSAPR_POLICY_INFORMATION();
            this.Invalidhandlle = false;
            this.invalidParam = false;

            #region CheckforInvalidParameter

            ////Setting invalidParam to true for Information Class which will return Invalid Parameter
            if ((informationType == InformationClass.PolicyAuditFullQueryInformation)
                     || (informationType == InformationClass.PolicyAuditFullSetInformation)
                     || ((informationType == InformationClass.PolicyAccountDomainInformation) && (isDC == true))
                     || (informationType == InformationClass.PolicyPdAccountInformation)
                     || (informationType == InformationClass.PolicyModificationInformation)
                     || (informationType == InformationClass.Invalid))
            {
                this.invalidParam = true;
            }

            #endregion CheckforInvalidParameter

            #region CheckforInvalidHandleCase

            ////Only check InvalidHandle in case of NOT InvalidParam
            if (stPolicyInformation.PHandle != handleInput && !this.invalidParam) 
            {
                ////CreateAnInvalidHandle() will create invalid handle to check requirement of Invalid Handle
                objSecretHandle = utilities.CreateAnInvalidHandle(false);
                this.InvalidPolicyHandle = objSecretHandle;
                PolicyHandle = this.InvalidPolicyHandle;
                this.Invalidhandlle = true;
            }
            else
            {
                PolicyHandle = this.ValidPolicyHandle;
                this.Invalidhandlle = false;
            }

            #endregion CheckforInvalidHandleCase

            #region checkInformationClassvalues

            ////Passing Input to set Information as per Information class and setting intCheckInfoClass to return status
            switch (informationType)
            {
                case InformationClass.PolicyAccountDomainInformation:

                    policyInformationClass2 = InformationClass.PolicyAccountDomainInformation;

                    if (isDC)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnInvalidParameter;
                    }
                    else
                    {
                        policyInfoinServer.PolicyAccountDomainInfo = new _LSAPR_POLICY_ACCOUNT_DOM_INFO();
                        policyInfoinServer.PolicyAccountDomainInfo.DomainName = new _RPC_UNICODE_STRING();
                        policyInfoinServer.PolicyAccountDomainInfo.DomainName.Buffer =
                            utilities.ConversionfromStringtoushortArray(domain);
                        policyInfoinServer.PolicyAccountDomainInfo.DomainName.Length =
                            (ushort)(2 * (policyInfoinServer.PolicyAccountDomainInfo.DomainName.Buffer.Length));
                        policyInfoinServer.PolicyAccountDomainInfo.DomainName.MaximumLength = 
                            (ushort)((policyInfoinServer.PolicyAccountDomainInfo.DomainName.Length)+2);
                        policyInfoinServer.PolicyAccountDomainInfo.DomainSid =
                            utilities.GetSid(DomainSid.Valid, GetAccountDomainSidNonDC);
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyPrimaryDomainInformation:

                    policyInformationClass2 = InformationClass.PolicyPrimaryDomainInformation;
                    policyInfoinServer.PolicyPrimaryDomainInfo = new _LSAPR_POLICY_PRIMARY_DOM_INFO();
                    policyInfoinServer.PolicyPrimaryDomainInfo.Name = new _RPC_UNICODE_STRING();
                    policyInfoinServer.PolicyPrimaryDomainInfo.Name.Buffer =
                        utilities.ConversionfromStringtoushortArray(domain);
                    policyInfoinServer.PolicyPrimaryDomainInfo.Name.Length = 
                        (ushort)(2 * policyInfoinServer.PolicyPrimaryDomainInfo.Name.Buffer.Length);
                    policyInfoinServer.PolicyPrimaryDomainInfo.Name.MaximumLength = 
                        (ushort)((policyInfoinServer.PolicyPrimaryDomainInfo.Name.Length)+2);
                    policyInfoinServer.PolicyPrimaryDomainInfo.Sid = 
                        utilities.GetSid(DomainSid.Valid, this.PrimaryDomainSID);

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_TRUST_ADMIN) != ACCESS_MASK.POLICY_TRUST_ADMIN)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyAuditEventsInformation:

                    policyInformationClass2 = InformationClass.PolicyAuditEventsInformation;
                    policyInfoinServer.PolicyAuditEventsInfo = new _LSAPR_POLICY_AUDIT_EVENTS_INFO();
                    policyInfoinServer.PolicyAuditEventsInfo.AuditingMode = (byte)AuditingMode;
                    policyInfoinServer.PolicyAuditEventsInfo.EventAuditingOptions = new EventAuditingOptions_Values[1];
                    policyInfoinServer.PolicyAuditEventsInfo.EventAuditingOptions[0] = 
                        EventAuditingOptions_Values.POLICY_AUDIT_EVENT_SUCCESS;
                    policyInfoinServer.PolicyAuditEventsInfo.MaximumAuditEventCount = MaximumAuditEventCount;

                    if (informationType == InformationClass.PolicyAuditEventsInformation 
                            && policyInfoinServer.PolicyAuditEventsInfo.MaximumAuditEventCount != 0)
                    {
                        #region MS-LSAD_R60

                        Site.CaptureRequirementIfIsNotNull(
                            policyInfoinServer.PolicyAuditEventsInfo.EventAuditingOptions, 
                            "MS-LSAD", 
                            60,
                            @"If the ""MaximumAuditingEventCount"" field of the 
                            LSAPR_POLICY_AUDIT_EVENTS_INFO structure has a value other than 0, the 
                            ""EventAuditingOptions"" field MUST NOT be NULL.");
                        
                        #endregion
                    }

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_SET_AUDIT_REQUIREMENTS) != 
                        ACCESS_MASK.POLICY_SET_AUDIT_REQUIREMENTS)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyAuditFullQueryInformation:

                    policyInformationClass2 = InformationClass.PolicyAuditFullQueryInformation;
                    policyInfoinServer.PolicyAuditFullQueryInfo = new _POLICY_AUDIT_FULL_QUERY_INFO();
                    intCheckInfoClass = LsadUtilities.ReturnInvalidParameter;

                    break;

                case InformationClass.PolicyAuditFullSetInformation:

                    policyInformationClass2 = InformationClass.PolicyAuditFullSetInformation;
                    policyInfoinServer.PolicyAuditFullSetInfo = new _POLICY_AUDIT_FULL_SET_INFO();

                    if (this.isWindows)
                    {
                        if (PDCOSVersion >= ServerVersion.Win2003)
                        {
                            intCheckInfoClass = LsadUtilities.ReturnInvalidParameter;
                        }
                    }
                    else if ((this.uintAccessMask & ACCESS_MASK.POLICY_AUDIT_LOG_ADMIN) != 
                        ACCESS_MASK.POLICY_AUDIT_LOG_ADMIN)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }

                    break;

                case InformationClass.PolicyAuditLogInformation:

                    policyInformationClass2 = InformationClass.PolicyAuditLogInformation;
                    policyInfoinServer.PolicyAuditLogInfo = new _POLICY_AUDIT_LOG_INFO();

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_AUDIT_LOG_ADMIN) != 
                        ACCESS_MASK.POLICY_AUDIT_LOG_ADMIN)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnNotSupported;
                    }

                    break;

                case InformationClass.PolicyDnsDomainInformation:

                    policyInformationClass2 = InformationClass.PolicyDnsDomainInformation;
                    policyInfoinServer.PolicyDnsDomainInfo = new _LSAPR_POLICY_DNS_DOMAIN_INFO();

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_TRUST_ADMIN) != ACCESS_MASK.POLICY_TRUST_ADMIN)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        policyInfoinServer.PolicyDnsDomainInfo.Name = new _RPC_UNICODE_STRING();
                        policyInfoinServer.PolicyDnsDomainInfo.Name.Buffer =
                            utilities.ConversionfromStringtoushortArray(domain);
                        policyInfoinServer.PolicyDnsDomainInfo.Name.Length = 
                            (ushort)(2 * (policyInfoinServer.PolicyDnsDomainInfo.Name.Buffer.Length));
                        policyInfoinServer.PolicyDnsDomainInfo.Name.MaximumLength =
                            (ushort)((policyInfoinServer.PolicyDnsDomainInfo.Name.Length)+2);
                        policyInfoinServer.PolicyDnsDomainInfo.DnsDomainName = new _RPC_UNICODE_STRING();
                        policyInfoinServer.PolicyDnsDomainInfo.DnsDomainName.Buffer =
                            utilities.ConversionfromStringtoushortArray(fullDomain);
                        policyInfoinServer.PolicyDnsDomainInfo.DnsDomainName.Length = 
                            (ushort)(2 * (policyInfoinServer.PolicyDnsDomainInfo.DnsDomainName.Buffer.Length));
                        policyInfoinServer.PolicyDnsDomainInfo.DnsDomainName.MaximumLength = 
                            (ushort)((policyInfoinServer.PolicyDnsDomainInfo.DnsDomainName.Length)+2);
                        policyInfoinServer.PolicyDnsDomainInfo.DnsForestName = new _RPC_UNICODE_STRING();
                        policyInfoinServer.PolicyDnsDomainInfo.DnsForestName.Buffer = 
                            utilities.ConversionfromStringtoushortArray(fullDomain);
                        policyInfoinServer.PolicyDnsDomainInfo.DnsForestName.Length = 
                            (ushort)(2 * (policyInfoinServer.PolicyDnsDomainInfo.DnsForestName.Buffer.Length));
                        policyInfoinServer.PolicyDnsDomainInfo.DnsForestName.MaximumLength =
                            (ushort)((policyInfoinServer.PolicyDnsDomainInfo.DnsForestName.Length)+2);
                        policyInfoinServer.PolicyDnsDomainInfo.DomainGuid = 
                            new Guid(DomainGUID);
                        policyInfoinServer.PolicyDnsDomainInfo.Sid = 
                            utilities.GetSid(DomainSid.Valid, this.PrimaryDomainSID);
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyDnsDomainInformationInt:

                    policyInformationClass2 = InformationClass.PolicyDnsDomainInformationInt;

                    policyInfoinServer.PolicyDnsDomainInfoInt = new _LSAPR_POLICY_DNS_DOMAIN_INFO();

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_TRUST_ADMIN) != ACCESS_MASK.POLICY_TRUST_ADMIN)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        policyInfoinServer.PolicyDnsDomainInfoInt.Name = new _RPC_UNICODE_STRING();
                        policyInfoinServer.PolicyDnsDomainInfoInt.Name.Buffer = 
                            utilities.ConversionfromStringtoushortArray(domain);
                        policyInfoinServer.PolicyDnsDomainInfoInt.Name.Length = 
                            (ushort)(2 * (policyInfoinServer.PolicyDnsDomainInfoInt.Name.Buffer.Length));
                        policyInfoinServer.PolicyDnsDomainInfoInt.Name.MaximumLength =
                            (ushort)((policyInfoinServer.PolicyDnsDomainInfoInt.Name.Length)+2);
                        policyInfoinServer.PolicyDnsDomainInfoInt.DnsDomainName = new _RPC_UNICODE_STRING();
                        policyInfoinServer.PolicyDnsDomainInfoInt.DnsDomainName.Buffer = 
                            utilities.ConversionfromStringtoushortArray(fullDomain);
                        policyInfoinServer.PolicyDnsDomainInfoInt.DnsDomainName.Length = 
                            (ushort)(2 * (policyInfoinServer.PolicyDnsDomainInfoInt.DnsDomainName.Buffer.Length));
                        policyInfoinServer.PolicyDnsDomainInfoInt.DnsDomainName.MaximumLength = 
                            (ushort)((policyInfoinServer.PolicyDnsDomainInfoInt.DnsDomainName.Length)+2);
                        policyInfoinServer.PolicyDnsDomainInfoInt.DnsForestName = new _RPC_UNICODE_STRING();
                        policyInfoinServer.PolicyDnsDomainInfoInt.DnsForestName.Buffer = 
                            utilities.ConversionfromStringtoushortArray(fullDomain);
                        policyInfoinServer.PolicyDnsDomainInfoInt.DnsForestName.Length = 
                            (ushort)(2 * (policyInfoinServer.PolicyDnsDomainInfoInt.DnsForestName.Buffer.Length));
                        policyInfoinServer.PolicyDnsDomainInfoInt.DnsForestName.MaximumLength = 
                            (ushort)((policyInfoinServer.PolicyDnsDomainInfoInt.DnsForestName.Length)+2);
                        policyInfoinServer.PolicyDnsDomainInfoInt.DomainGuid = 
                            new Guid(DomainGUID);

                        policyInfoinServer.PolicyDnsDomainInfoInt.Sid =
                            utilities.GetSid(DomainSid.Valid, this.PrimaryDomainSID);

                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyLocalAccountDomainInformation:

                    policyInformationClass2 = InformationClass.PolicyLocalAccountDomainInformation;
                    policyInfoinServer.PolicyLocalAccountDomainInfo = new _LSAPR_POLICY_ACCOUNT_DOM_INFO();
                    policyInfoinServer.PolicyLocalAccountDomainInfo.DomainName = new _RPC_UNICODE_STRING();
                    policyInfoinServer.PolicyLocalAccountDomainInfo.DomainName.Buffer = 
                        utilities.ConversionfromStringtoushortArray(domain);
                    policyInfoinServer.PolicyLocalAccountDomainInfo.DomainName.Length = 
                        (ushort)(2 * policyInfoinServer.PolicyLocalAccountDomainInfo.DomainName.Buffer.Length);
                    policyInfoinServer.PolicyLocalAccountDomainInfo.DomainName.MaximumLength = 
                        (ushort)((policyInfoinServer.PolicyLocalAccountDomainInfo.DomainName.Length)+2);
                    policyInfoinServer.PolicyLocalAccountDomainInfo.DomainSid = 
                        utilities.GetSid(DomainSid.Valid, GetLocalAccountDomainSid);

                    if (this.serverPlatform == Server.Windows2k8)
                    {
                        if ((this.uintAccessMask & ACCESS_MASK.POLICY_TRUST_ADMIN) != ACCESS_MASK.POLICY_TRUST_ADMIN)
                        {
                            intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                        }
                        else
                        {
                            intCheckInfoClass = LsadUtilities.ReturnSuccess;
                        }
                    }

                    break;

                case InformationClass.PolicyLsaServerRoleInformation:

                    policyInformationClass2 = InformationClass.PolicyLsaServerRoleInformation;
                    policyInfoinServer.PolicyServerRoleInfo = new _POLICY_LSA_SERVER_ROLE_INFO();

                    if (GetIntProperty(propertyGroup + "ServerRoleSelected") == 3)
                    {
                        policyInfoinServer.PolicyServerRoleInfo.LsaServerRole =
                            _POLICY_LSA_SERVER_ROLE.PolicyServerRolePrimary;
                    }

                    if (GetIntProperty(propertyGroup + "ServerRoleSelected") == 2)
                    {
                        policyInfoinServer.PolicyServerRoleInfo.LsaServerRole =
                            _POLICY_LSA_SERVER_ROLE.PolicyServerRoleBackup;
                    }

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_SERVER_ADMIN) != ACCESS_MASK.POLICY_SERVER_ADMIN)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyModificationInformation:

                    policyInformationClass2 = InformationClass.PolicyModificationInformation;
                    policyInfoinServer.PolicyModificationInfo = new _POLICY_MODIFICATION_INFO();
                    intCheckInfoClass = LsadUtilities.ReturnInvalidParameter;

                    break;

                case InformationClass.PolicyPdAccountInformation:

                    policyInformationClass2 = InformationClass.PolicyPdAccountInformation;
                    policyInfoinServer.PolicyPdAccountInfo = new _LSAPR_POLICY_PD_ACCOUNT_INFO();
                    intCheckInfoClass = LsadUtilities.ReturnInvalidParameter;

                    break;

                case InformationClass.PolicyReplicaSourceInformation:

                    policyInformationClass2 = InformationClass.PolicyReplicaSourceInformation;
                    policyInfoinServer.PolicyReplicaSourceInfo = new _LSAPR_POLICY_REPLICA_SRCE_INFO();
                    policyInfoinServer.PolicyReplicaSourceInfo.ReplicaAccountName = new _RPC_UNICODE_STRING();
                    policyInfoinServer.PolicyReplicaSourceInfo.ReplicaAccountName.Buffer = 
                        utilities.ConversionfromStringtoushortArray(ReplicasourceName);
                    policyInfoinServer.PolicyReplicaSourceInfo.ReplicaAccountName.Length = 
                        (ushort)(2 * policyInfoinServer.PolicyReplicaSourceInfo.ReplicaAccountName.Buffer.Length);
                    policyInfoinServer.PolicyReplicaSourceInfo.ReplicaAccountName.MaximumLength = 
                        (ushort)((policyInfoinServer.PolicyReplicaSourceInfo.ReplicaAccountName.Length)+2);
                    policyInfoinServer.PolicyReplicaSourceInfo.ReplicaSource = new _RPC_UNICODE_STRING();
                    policyInfoinServer.PolicyReplicaSourceInfo.ReplicaSource.Buffer = 
                        utilities.ConversionfromStringtoushortArray(ReplicaAccountName);
                    policyInfoinServer.PolicyReplicaSourceInfo.ReplicaSource.Length = 
                        (ushort)(2 * policyInfoinServer.PolicyReplicaSourceInfo.ReplicaSource.Buffer.Length);
                    policyInfoinServer.PolicyReplicaSourceInfo.ReplicaSource.MaximumLength = 
                        (ushort)((policyInfoinServer.PolicyReplicaSourceInfo.ReplicaSource.Length)+2);

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_SERVER_ADMIN) != ACCESS_MASK.POLICY_SERVER_ADMIN)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyMachineAccountInformation:
                    policyInformationClass2 = InformationClass.PolicyMachineAccountInformation;
                    policyInfoinServer.PolicyMachineAccountInfo = new _LSAPR_POLICY_MACHINE_ACCT_INFO();
                    policyInfoinServer.PolicyMachineAccountInfo.Rid = 0;
                    policyInfoinServer.PolicyMachineAccountInfo.Sid = utilities.GetSid(DomainSid.Valid, GetLocalAccountDomainSid);
                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_SERVER_ADMIN) != ACCESS_MASK.POLICY_SERVER_ADMIN)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }
                    break;

                default:

                    #region invalidenumcheck

                    intCheckInfoClass = LsadUtilities.ReturnInvalidParameter;

                    #endregion

                    break;
            }

            #endregion checkInformationClassvalues

            uintMethodStatus = lsadClientStack.LsarSetInformationPolicy(
                PolicyHandle.Value,
                (_POLICY_INFORMATION_CLASS)policyInformationClass2, 
                policyInfoinServer);

            if ((uint)uintMethodStatus == (uint)ErrorStatus.Success)
            {
                if (informationType == InformationClass.PolicyAuditEventsInformation 
                        && policyInfoinServer.PolicyAuditEventsInfo.MaximumAuditEventCount != 0)
                {
                    #region MS-LSAD_R60

                    Site.CaptureRequirementIfIsNotNull(
                        policyInfoinServer.PolicyAuditEventsInfo.EventAuditingOptions,
                        "MS-LSAD", 
                        60,
                        @"If the ""MaximumAuditingEventCount"" field of the 
                            LSAPR_POLICY_AUDIT_EVENTS_INFO structure has a value other than 0, the 
                            ""EventAuditingOptions"" field MUST NOT be NULL.");

                    #endregion
                }
            }

            ////Capturing Invalid Parameter Requirements based on Different Information classes
            if (this.invalidParam)
            {
                if (informationType == InformationClass.PolicyAccountDomainInformation && (isDC == true))
                {
                    #region MS-LSAD_R386

                    Site.CaptureRequirementIfAreEqual<uint>(
                        ((uint)ErrorStatus.InvalidParameter), 
                        (uint)uintMethodStatus,
                        "MS-LSAD", 
                        386, 
                        @"If the Value of InformationClass parameter in LsarSetInformationPolicy is 
                        PolicyAccountDomainInformation then on domain controller, server MUST fail 
                        the request with the STATUS_INVALID_PARAMETER.");

                    #endregion
                }

                if (informationType == InformationClass.PolicyAuditFullQueryInformation)
                {
                    #region MS-LSAD_R377

                    Site.CaptureRequirementIfAreEqual<uint>(
                        ((uint)ErrorStatus.InvalidParameter),
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        377, 
                        @"If InformationClass value of LsarSetInformationPolicy is 
                        PolicyAuditFullQueryInformation then type of access required is cannot be set; 
                        the request MUST fail with STATUS_InvalidParameter.");
                   
                    #endregion

                    #region MS-LSAD_R393

                    Site.CaptureRequirementIfAreEqual<uint>(
                        ((uint)ErrorStatus.InvalidParameter),
                        (uint)uintMethodStatus, 
                        "MS-LSAD",
                        393, 
                        @"If the Value of InformationClass parameter in LsarSetInformationPolicy is 
                        PolicyAuditFullQueryInformation then Server MUST return STATUS_InvalidParameter 
                        because this is not a policy element that can be set.");
                   
                    #endregion
                }

                if (informationType == InformationClass.PolicyModificationInformation)
                {
                    #region MS-LSAD_R375

                    Site.CaptureRequirementIfAreEqual<uint>(
                        ((uint)ErrorStatus.InvalidParameter), 
                        (uint)uintMethodStatus, 
                        "MS-LSAD", 
                        375, 
                        @"If InformationClass value of LsarSetInformationPolicy is 
                        PolicyModificationInformation then type of access required 
                        is cannot be set; the request MUST fail with STATUS_InvalidParameter.");
                    
                    #endregion

                    #region MS-LSAD_R391

                    Site.CaptureRequirementIfAreEqual<uint>(
                        ((uint)ErrorStatus.InvalidParameter),
                        (uint)uintMethodStatus, 
                        "MS-LSAD",
                        391, 
                        @"If the Value of InformationClass parameter in LsarSetInformationPolicy is 
                        PolicyModificationInformation then Server MUST return STATUS_InvalidParameter 
                        because this is not a policy element that can be set.");
                    
                    #endregion
                }

                if (informationType == InformationClass.PolicyPdAccountInformation)
                {
                    #region MS-LSAD_R385

                    Site.CaptureRequirementIfAreEqual<uint>(
                        ((uint)ErrorStatus.InvalidParameter), 
                        (uint)uintMethodStatus, 
                        "MS-LSAD",
                        385, 
                        @"If the Value of InformationClass parameter in LsarSetInformationPolicy is
                        PolicyPdAccountInformation then Server MUST return STATUS_InvalidParameter
                        because this is not a policy element that can be set.");
                    
                    #endregion
                }

                ////Capturing Invalid Handle case Requirement
                if (informationType == InformationClass.PolicyAuditFullSetInformation)
                {
                    #region MS-LSAD_R392

                    Site.CaptureRequirementIfAreEqual<uint>(
                        ((uint)ErrorStatus.InvalidParameter),
                        (uint)uintMethodStatus, 
                        "MS-LSAD",
                        392,
                        @"If InformationClass value of LsarSetInformationPolicy is PolicyAuditFullSetInformation 
                        then Win2k3,2k8 and Vista will return Invalid Parameter");
                    
                    #endregion
                }
            }
            else if (this.Invalidhandlle == true)
            {
                utilities.CreateAnInvalidHandle(true);

                #region MS-LSAD_R367

                Site.CaptureRequirementIfAreEqual<uint>(
                    ((uint)ErrorStatus.InvalidHandle),
                    (uint)uintMethodStatus, 
                    "MS-LSAD", 
                    367, 
                    @"If PolicyHandle of LsarSetInformationPolicy is not a Valid context handle,
                    the server MUST return STATUS_InvalidHandle.");
                
                #endregion
            }
            //Validating Requirement for Success, Not Implemented and Access Denied Status
            if (!this.Invalidhandlle)
            {
                #region Capturing_Requirement for Status other than Invalid Handle

                switch (intCheckInfoClass)
                {
                    ////Validating success requirements
                    case 0:

                        #region MS-LSAD_R363

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.Success, 
                            (uint)uintMethodStatus, 
                            "MS-LSAD",
                            363, 
                            @"The return Value  for Opnum LsarSetInformationPolicy that an implementation MUST 
                            return is STATUS_Success(0x00000000) when the request was successfully completed");
                        
                        #endregion

                        #region MS-LSAD_R366

                        Site.CaptureRequirementIfIsTrue(
                            ((PolicyHandle != IntPtr.Zero) && (uintMethodStatus != NtStatus.STATUS_ACCESS_DENIED)), 
                            "MS-LSAD", 
                            366,
                            @"PolicyHandle parameter of LsarSetInformationPolicy MUST reference a context that was 
                            granted an access commensurate with the InformationClass value requested.");
                        
                        #endregion

                        break;

                    ////Capturing Requirement for general Invalid Parameter case
                    case 1:

                        #region MS-LSAD_R364

                        Site.CaptureRequirementIfAreEqual<uint>(
                            ((uint)ErrorStatus.InvalidParameter),
                            (uint)uintMethodStatus, 
                            "MS-LSAD",
                            364, 
                            @"The LsarSetInformationPolicy implementation MUST return
                            STATUS_InvalidParameter(oxC000000D) when one of the parameters is incorrect. 
                            For instance, this can happen if InformationClass is not supported or some of the 
                            supplied policy data is inValid.");
                        
                        #endregion

                        break;

                    ////Capturing requirement for Not_Implemented status
                    case 2:

                        #region MS-LSAD_R365

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.NotImplemented,
                            (uint)uintMethodStatus,
                            "MS-LSAD", 
                            365,
                            @"The LsarSetInformationPolicy implementation MUST return is 
                            STATUS_NotImplemented(0xC0000002) when an information class cannot be set.");
                        
                        #endregion

                        #region MS-LSAD_R382

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.NotImplemented, 
                            (uint)uintMethodStatus,
                            "MS-LSAD", 
                            382,
                            @"If the Value of InformationClass parameter in LsarSetInformationPolicy is 
                            PolicyAuditLogInformation then Server MUST return the STATUS_NotImplemented 
                            error code because this is not a policy element that can be set.");
                        
                        #endregion

                        break;

                    ////capturing requirement for Access denied status
                    case 3:
                        if (informationType == InformationClass.PolicyPrimaryDomainInformation)
                        { 
                            #region MS-LSAD_R371

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied), 
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                371, 
                                @"If InformationClass value of LsarSetInformationPolicy is 
                                PolicyPrimaryDomainInformation then type of access required 
                                MUST be POLICY_TRUST_ADMIN");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyAuditEventsInformation)
                        {   
                            #region MS-LSAD_R370

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus, 
                                "MS-LSAD",
                                370, 
                                @"If InformationClass value of LsarSetInformationPolicy is PolicyAuditEventsInformation
                                then type of access required MUST be POLICY_SET_AUDIT_REQUIREMENTS");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyAuditFullSetInformation)
                        {  
                            #region MS-LSAD_R376

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus, 
                                "MS-LSAD",
                                376, 
                                @"If InformationClass value of LsarSetInformationPolicy is PolicyAuditFullSetInformation
                                then type of access required MUST be POLICY_AUDIT_LOG_ADMIN");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyAuditLogInformation)
                        {  
                            #region MS-LSAD_R369

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied), 
                                (uint)uintMethodStatus, 
                                "MS-LSAD", 
                                369, 
                                @"If InformationClass value of LsarSetInformationPolicy is PolicyAuditLogInformation 
                                then type of access required MUST be POLICY_AUDIT_LOG_ADMIN");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyDnsDomainInformation)
                        {   
                            #region MS-LSAD_R378

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                378, 
                                @"If InformationClass value of LsarSetInformationPolicy is PolicyDnsDomainInformation 
                                then type of access required MUST be POLICY_TRUST_ADMIN.");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyDnsDomainInformationInt)
                        {   
                            #region MS-LSAD_R379

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus, 
                                "MS-LSAD", 
                                379, 
                                @"If InformationClass value of LsarSetInformationPolicy is PolicyDnsDomainInformationInt
                                then type of access required MUST be POLICY_TRUST_ADMIN.");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyLocalAccountDomainInformation)
                        {
                            if (this.serverPlatform == Server.Windows2k8)
                            {
                                #region MS-LSAD_R380

                                Site.CaptureRequirementIfAreEqual<uint>(
                                    ((uint)ErrorStatus.AccessDenied), 
                                    (uint)uintMethodStatus,
                                    "MS-LSAD", 
                                    380, 
                                    @"If InformationClass value of LsarSetInformationPolicy is 
                                    PolicyLocalAccountDomainInformation then type of access required
                                    MUST be POLICY_TRUST_ADMIN.");

                                #endregion
                            }
                        }

                        if (informationType == InformationClass.PolicyLsaServerRoleInformation)
                        {
                            #region MS-LSAD_R372

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied), 
                                (uint)uintMethodStatus, 
                                "MS-LSAD",
                                372, 
                                @"If InformationClass value of LsarSetInformationPolicy is 
                                PolicyLsaServerRoleInformation then type of access required 
                                MUST be POLICY_SERVER_ADMIN");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyReplicaSourceInformation)
                        {   
                            #region MS-LSAD_R373

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied), 
                                (uint)uintMethodStatus, 
                                "MS-LSAD", 
                                373,
                                @"If InformationClass value of LsarSetInformationPolicy is 
                                PolicyReplicaSourceInformation then type of access required 
                                MUST be POLICY_SERVER_ADMIN");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyPdAccountInformation)
                        {   
                            #region MS-LSAD_R300

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied), 
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                300, 
                                @"If InformationClass value of LsarQueryInformationPolicy is PolicyPdAccountInformation
                                then Type of access required MUST be POLICY_GET_PRIVATE_INFORMATION(0x00000004)");

                            #endregion
                        }

                        #region MS-LSAD_R368

                        Site.CaptureRequirementIfAreEqual<uint>(
                            ((uint)ErrorStatus.AccessDenied),
                            (uint)uintMethodStatus, 
                            "MS-LSAD",
                            368, 
                            @"If the context of LsarSetInformationPolicy does not have sufficient access, 
                            the server MUST return STATUS_AccessDenied.");

                        #endregion

                        break;

                    default:

                        break;
                }

                #endregion Capturing_Requirement for Status other than Invalid Handle
            }

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion SetInformationPolicy

        #region QueryInformationPolicy2

        /// <summary>
        /// This method is used to query policy object information
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="informationType">Contains the type of policy object information to be queried</param>
        /// <param name="policyInformation">Output Parameter which contains policy object information 
        /// which has been queried</param>
        /// <returns>Returns Success if the method is successful
        ///          Returns InvalidParameter if the parameters passed to the method are not valid
        ///          Returns AccessDenied if the caller does not have the permissions 
        ///          to perform the operation
        ///          Returns InvalidHandle if the policy handle passed is not valid</returns>
        /// Disable warning CA1502 and CA1505 because it will affect the implementation of Adapter and Model 
        /// codes if do any changes about maintainability.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity",
            Justification = "affect the implementation of Adapter and Model codes")]
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", 
            Justification = "affect the implementation of Adapter and Model codes")]
        public ErrorStatus QueryInformationPolicy2(
            int handleInput,
            InformationClass informationType,
            out PolicyInformation policyInformation)
        {
            InformationClass policyInformationClass2 = new InformationClass();
            _LSAPR_POLICY_INFORMATION? policyInfoinServer = new _LSAPR_POLICY_INFORMATION();
            this.Invalidhandlle = false;
            this.invalidParam = false;

            #region CheckingInvalidParameter

            ////Setting invalidParam flag to true for Information class which will return Invalid Parameter
            if ((informationType == InformationClass.PolicyModificationInformation)
                     || (informationType == InformationClass.PolicyAuditFullQueryInformation)
                     || (informationType == InformationClass.PolicyAuditFullSetInformation)
                     || (informationType == InformationClass.Invalid))
            {
                this.invalidParam = true;
            }

            #endregion CheckingInvalidParameter

            #region CreatingInvalidHandle

            if (stPolicyInformation.PHandle != handleInput)
            {
                ////CreateAnInvalidHandle() will create invalid handle to check requirement of Invalid Handle
                objSecretHandle = utilities.CreateAnInvalidHandle(false);
                this.InvalidPolicyHandle = objSecretHandle;
                PolicyHandle = this.InvalidPolicyHandle;
                this.Invalidhandlle = true;
            }
            else
            {
                PolicyHandle = this.ValidPolicyHandle;
                this.Invalidhandlle = false;
            }

            #endregion CreatingInvalidHandle

            #region checkInfovalues

            ////Setting intCheckInfoClass with return status for capturing requirement
            switch (informationType)
            {
                case InformationClass.PolicyAccountDomainInformation:

                    policyInformationClass2 = InformationClass.PolicyAccountDomainInformation;

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION) != 
                        ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyAuditEventsInformation:

                    policyInformationClass2 = InformationClass.PolicyAuditEventsInformation;

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_VIEW_AUDIT_INFORMATION) != 
                        ACCESS_MASK.POLICY_VIEW_AUDIT_INFORMATION)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyAuditFullQueryInformation:

                    policyInformationClass2 = InformationClass.PolicyAuditFullQueryInformation;

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_VIEW_AUDIT_INFORMATION) != 
                        ACCESS_MASK.POLICY_VIEW_AUDIT_INFORMATION)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }

                    if (this.isWindows)
                    {
                        if (PDCOSVersion >= ServerVersion.Win2003)
                        {
                            intCheckInfoClass = LsadUtilities.ReturnInvalidParameter;
                        }
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyAuditFullSetInformation:

                    policyInformationClass2 = InformationClass.PolicyAuditFullSetInformation;
                    intCheckInfoClass = LsadUtilities.ReturnInvalidParameter;

                    break;

                case InformationClass.PolicyAuditLogInformation:

                    policyInformationClass2 = InformationClass.PolicyAuditLogInformation;

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_VIEW_AUDIT_INFORMATION) != 
                        ACCESS_MASK.POLICY_VIEW_AUDIT_INFORMATION)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyDefaultQuotaInformation:

                    policyInformationClass2 = InformationClass.PolicyDefaultQuotaInformation;

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION) != 
                        ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyDnsDomainInformation:

                    policyInformationClass2 = InformationClass.PolicyDnsDomainInformation;

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION) != 
                        ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyDnsDomainInformationInt:

                    policyInformationClass2 = InformationClass.PolicyDnsDomainInformationInt;

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION) != 
                        ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyLocalAccountDomainInformation:

                    policyInformationClass2 = InformationClass.PolicyLocalAccountDomainInformation;

                    if (this.serverPlatform == Server.Windows2k8)
                    {
                        if ((this.uintAccessMask & ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION) != 
                            ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION)
                        {
                            intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                        }
                        else
                        {
                            intCheckInfoClass = LsadUtilities.ReturnSuccess;
                        }
                    }

                    break;

                case InformationClass.PolicyLsaServerRoleInformation:

                    policyInformationClass2 = InformationClass.PolicyLsaServerRoleInformation;

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION) != 
                        ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyModificationInformation:

                    policyInformationClass2 = InformationClass.PolicyModificationInformation;
                    intCheckInfoClass = LsadUtilities.ReturnInvalidParameter;

                    break;

                case InformationClass.PolicyPdAccountInformation:

                    policyInformationClass2 = InformationClass.PolicyPdAccountInformation;

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_GET_PRIVATE_INFORMATION) != 
                        ACCESS_MASK.POLICY_GET_PRIVATE_INFORMATION)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyPrimaryDomainInformation:

                    policyInformationClass2 = InformationClass.PolicyPrimaryDomainInformation;

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION) != 
                        ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyReplicaSourceInformation:

                    policyInformationClass2 = InformationClass.PolicyReplicaSourceInformation;

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION) != 
                        ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyMachineAccountInformation:
                    policyInformationClass2 = InformationClass.PolicyMachineAccountInformation;
                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_VIEW_AUDIT_INFORMATION) != ACCESS_MASK.POLICY_VIEW_AUDIT_INFORMATION)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }
                    break;

                default:

                    intCheckInfoClass = LsadUtilities.ReturnInvalidParameter;

                    break;
            }

            #endregion

            uintMethodStatus = lsadClientStack.LsarQueryInformationPolicy2(
                PolicyHandle.Value,
                (_POLICY_INFORMATION_CLASS)policyInformationClass2,
                out policyInfoinServer);

            ////Capturing requirements for Invalid Parameter status
            if (this.invalidParam == true)
            {
                if (informationType == InformationClass.PolicyModificationInformation)
                {
                    #region MS-LSAD_R267

                    Site.CaptureRequirementIfAreEqual<uint>(
                        ((uint)ErrorStatus.InvalidParameter),
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        267,
                        @"If InformationClass value of LsarQueryInformationPolicy2 is PolicyModificationInformation 
                        then Type of access required cannot be queried.The request MUST fail with 
                        STATUS_InvalidParameter(0xC000000D)");

                    #endregion

                    #region MS-LSAD_R282

                    Site.CaptureRequirementIfAreEqual<uint>(
                        ((uint)ErrorStatus.InvalidParameter),
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        282,
                        @"If the value of InformationClass parameter in LsarQueryInformationPolicy2 is 
                        PolicyModificationInformation then Information returned to caller from abstract 
                        data model MUST return STATUS_InvalidParameter");

                    #endregion
                }

                if (informationType == InformationClass.PolicyAuditFullSetInformation)
                {
                    #region MS-LSAD_R872

                    Site.CaptureRequirementIfAreEqual<uint>(
                        ((uint)ErrorStatus.InvalidParameter),
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        872,
                        @"LsarQueryInformationPolicy2 MUST return STATUS_INVALID_PARAMETER 
                        for AuditFullSet Information class");

                    #endregion
                }

                ////Capturing requirement for Invalid Handle Case
                if ((informationType == InformationClass.PolicyAuditFullQueryInformation) 
                    && this.serverPlatform >= Server.Windows2k3)
                {
                    #region MS-LSAD_R287

                    Site.CaptureRequirementIfAreEqual<uint>(
                        ((uint)ErrorStatus.InvalidParameter),
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        287,
                        @"<52> Section 3.1.4.4.3: Windows XP, Windows Server 2003, Windows Vista, and 
                        Windows Server 2008, Windows 7, and Windows Server 2008 R2 return STATUS_INVALID_PARAMETER
                        for PolicyAuditFullQueryInformationinformation class in LsarQueryInformationPolicy2.");

                    #endregion
                }
            }
            else if (this.Invalidhandlle == true)
            {
                #region MS-LSAD_R255

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidHandle,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    255,
                    @"LsarQueryInformationPolicy2 returns STATUS_InvalidHandle(0xC0000008),
                    if the handle is not a Valid policy object handle");

                #endregion
            }

            ////Capturing requirements for other return status
            if (!this.Invalidhandlle)
            {
                #region checkinfoclasss

                switch (intCheckInfoClass)
                {
                    ////Capturing requirements for Success state
                    case 0:

                        if (informationType == InformationClass.PolicyAuditLogInformation)
                        {
                            this.validPolicyInfoquery = 
                                utilities.ifQueryauditLogInfo((_LSAPR_POLICY_INFORMATION)policyInfoinServer);

                            if (this.isWindows)
                            {
                                #region MS-LSAD_R273

                                Site.CaptureRequirementIfIsTrue(
                                    (((uint)uintMethodStatus ==
                                    (uint)ErrorStatus.Success) && this.validPolicyInfoquery == true),
                                    "MS-LSAD",
                                    273,
                                    @"If the value of InformationClass parameter in LsarQueryInformationPolicy2 is
                                    PolicyAuditLogInformation then Information returned to caller from abstract data
                                    model MUST be Auditing Log Information");

                                #endregion
                            }
                        }

                        if (informationType == InformationClass.PolicyAuditEventsInformation)
                        {
                            this.validPolicyInfoquery = 
                                utilities.ifQueryauditeventsInfo((_LSAPR_POLICY_INFORMATION)policyInfoinServer);

                            #region MS-LSAD_R274

                            Site.CaptureRequirementIfIsTrue(
                                (((uint)uintMethodStatus == (uint)ErrorStatus.Success) 
                                && this.validPolicyInfoquery == true),
                                "MS-LSAD",
                                274,
                                @"If the value of InformationClass parameter in LsarQueryInformationPolicy2 is
                                PolicyAuditEventsInformation then Information returned to caller from abstract 
                                data model MUST be Event Auditing Options");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyPrimaryDomainInformation)
                        {
                            this.validPolicyInfoquery = 
                                utilities.ifQueryPrimarydomainInfo((_LSAPR_POLICY_INFORMATION)policyInfoinServer);

                            #region MS-LSAD_R275

                            Site.CaptureRequirementIfIsTrue(
                                (((uint)uintMethodStatus == (uint)ErrorStatus.Success) 
                                && this.validPolicyInfoquery == true),
                                "MS-LSAD",
                                275,
                                @"If the value of InformationClass parameter in LsarQueryInformationPolicy2 is
                                PolicyPrimaryDomainInformation then Information returned to caller from abstract
                                data model MUST be Primary Domain Information");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyAccountDomainInformation)
                        {
                            if (isDC == false)
                            {
                                this.validPolicyInfoquery = utilities.ifQueryAccountdomInfofornondc(
                                    (_LSAPR_POLICY_INFORMATION)policyInfoinServer);

                                #region MS-LSAD_R277

                                Site.CaptureRequirementIfIsTrue(
                                    ((uint)uintMethodStatus ==
                                        (uint)ErrorStatus.Success && this.validPolicyInfoquery == true),
                                    "MS-LSAD",
                                    277,
                                    @"If the value of InformationClass parameter in LsarQueryInformationPolicy2
                                    is PolicyAccountDomainInformation then Information returned to caller from 
                                    abstract data model on non-domain controllers MUST be Account Domain");

                                #endregion
                            }
                            else
                            {
                                this.validPolicyInfoquery = 
                                    utilities.ifQueryAccountdomInfo((_LSAPR_POLICY_INFORMATION)policyInfoinServer);

                                #region MS-LSAD_R969

                                Site.CaptureRequirementIfIsTrue(
                                    ((uint)uintMethodStatus ==
                                        (uint)ErrorStatus.Success && this.validPolicyInfoquery == true),
                                    "MS-LSAD",
                                    969,
                                    @"If the value of InformationClass parameter in LsarQueryInformationPolicy2 is 
                                    PolicyAccountDomainInformation then Information returned to caller from abstract
                                    data model on domain controller MUST return Primary Domain Information");

                                #endregion
                            }
                        }

                        if (informationType == InformationClass.PolicyLsaServerRoleInformation)
                        {
                            this.validPolicyInfoquery = 
                                utilities.ifQueryLsaSrvrroleInfo((_LSAPR_POLICY_INFORMATION)policyInfoinServer);

                            if (isDC == true)
                            {
                                #region MS-LSAD_R278

                                Site.CaptureRequirementIfIsTrue(
                                    (((uint)uintMethodStatus ==
                                        (uint)ErrorStatus.Success) && this.validPolicyInfoquery == true),
                                    "MS-LSAD",
                                    278,
                                    @"If the value of InformationClass parameter in LsarQueryInformationPolicy2 
                                    is PolicyLsaServerRoleInformation then Information returned to caller from 
                                    abstract data model on domain controller MUST be Primary Domain Information");

                                #endregion
                            }

                            #region MS-LSAD_R279

                            Site.CaptureRequirementIfIsTrue(
                                (((uint)uintMethodStatus == (uint)ErrorStatus.Success) 
                                && this.validPolicyInfoquery == true),
                                "MS-LSAD",
                                279,
                                @"If the value of InformationClass parameter in LsarQueryInformationPolicy2 is 
                                PolicyLsaServerRoleInformation then Information returned to caller from abstract 
                                data model MUST be Server Role Information");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyReplicaSourceInformation)
                        {
                            this.validPolicyInfoquery = 
                                utilities.ifQueryReplicaSrcInfo((_LSAPR_POLICY_INFORMATION)policyInfoinServer);

                            #region MS-LSAD_R280

                            Site.CaptureRequirementIfIsTrue(
                                (((uint)uintMethodStatus == (uint)ErrorStatus.Success) 
                                && this.validPolicyInfoquery == true),
                                "MS-LSAD",
                                280,
                                @"If the value of InformationClass parameter in LsarQueryInformationPolicy2 is 
                                PolicyReplicaSourceInformation then Information returned to caller from abstract 
                                data model MUST be Replica Source Information");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyDnsDomainInformation)
                        {
                            this.validPolicyInfoquery = 
                                utilities.ifQueryDnsDomInfo((_LSAPR_POLICY_INFORMATION)policyInfoinServer);

                            #region MS-LSAD_R284

                            Site.CaptureRequirementIfIsTrue(
                                (((uint)uintMethodStatus == (uint)ErrorStatus.Success) 
                                && this.validPolicyInfoquery == true),
                                "MS-LSAD",
                                284,
                                @"If the value of InformationClass parameter in LsarQueryInformationPolicy2 is
                                PolicyDnsDomainInformation then Information returned to caller from abstract data
                                model MUST be DNS Domain Information");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyDnsDomainInformationInt)
                        {
                            this.validPolicyInfoquery = 
                                utilities.ifQueryDnsDomIntInfo((_LSAPR_POLICY_INFORMATION)policyInfoinServer);

                            #region MS-LSAD_R285

                            Site.CaptureRequirementIfIsTrue(
                                (((uint)uintMethodStatus == (uint)ErrorStatus.Success) 
                                && this.validPolicyInfoquery == true),
                                "MS-LSAD",
                                285,
                                @"If the value of InformationClass parameter in LsarQueryInformationPolicy2 is 
                                PolicyDnsDomainInformationInt then Information returned to caller from abstract 
                                data model MUST be DNS Domain Information");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyLocalAccountDomainInformation)
                        {
                            if (this.serverPlatform == Server.Windows2k8)
                            {
                                this.validPolicyInfoquery = 
                                    utilities.ifQueryLocalaccountInfo((_LSAPR_POLICY_INFORMATION)policyInfoinServer);

                                #region MS-LSAD_R286
                                Site.CaptureRequirementIfIsTrue(
                                    (((uint)uintMethodStatus ==
                                        (uint)ErrorStatus.Success) && this.validPolicyInfoquery == true),
                                    "MS-LSAD",
                                    286,
                                    @"If the value of InformationClass parameter in LsarQueryInformationPolicy2 
                                    is PolicyLocalAccountDomainInformation then Information returned to caller 
                                    from abstract data model MUST be AccountDomainInformation");

                                #endregion
                            }
                        }

                        if (informationType == InformationClass.PolicyPdAccountInformation)
                        {
                            this.validPolicyInfoquery = 
                                utilities.ifQueryPdaccountInfo((_LSAPR_POLICY_INFORMATION)policyInfoinServer);

                            #region MS-LSAD_R276

                            Site.CaptureRequirementIfIsTrue(
                                (((uint)uintMethodStatus == (uint)ErrorStatus.Success) 
                                && this.validPolicyInfoquery == true),
                                "MS-LSAD",
                                276,
                                @"If the value of InformationClass parameter in LsarQueryInformationPolicy2 is 
                                PolicyPdAccountInformation then Information returned to caller from abstract data
                                model MUST return an LSAPR_POLICY_PD_ACCOUNT_INFO information structure initialized
                                with zeros");

                            #endregion
                        }

                        #region MS-LSAD_R252

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.Success, 
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            252,
                            @"The return Values for Opnum LsarQueryInformationPolicy2 that an implementation MUST 
                            return when the request was successfully completed isSTATUS_Success(0x00000000)");

                        #endregion

                        #region MS-LSAD_R256

                        Site.CaptureRequirementIfIsTrue(
                            (((uint)uintMethodStatus == (uint)ErrorStatus.Success) && PolicyHandle != IntPtr.Zero),
                            "MS-LSAD",
                            256,
                            @"PolicyHandle parameter of LsarQueryInformationPolicy2 MUST be a handle to an open
                            policy object");

                        #endregion

                        #region MS-LSAD_R257

                        Site.CaptureRequirementIfIsTrue(
                            (((uint)uintMethodStatus == (uint)ErrorStatus.Success) && PolicyHandle != IntPtr.Zero),
                            "MS-LSAD",
                            257,
                            @"PolicyHandle parameter of LsarQueryInformationPolicy2 MUST reference a context that was
                            granted an access commensurate with the InformationClass value requested.");

                        #endregion

                        break;

                    ////Capturing Requirement for general Invalid Parameter status
                    case 1:

                        #region MS-LSAD_R254

                        Site.CaptureRequirementIfAreEqual<uint>(
                            ((uint)ErrorStatus.InvalidParameter),
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            254,
                            @"The return Values  of Opnum LsarQueryInformationPolicy2 that an implementation MUST 
                            return  when one of the parameters is incorrect is STATUS_InvalidParameter
                            (0xC000000D) . For instance, this can happen if InformationClass is out of range or if 
                            PolicyInformation is Null.");

                        #endregion

                        break;

                    ////Capturing Requirement for Access denied status
                    case 3:

                        if (informationType == InformationClass.PolicyAuditFullQueryInformation)
                        {
                            #region MS-LSAD_R268

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                268,
                                @"If InformationClass value of LsarQueryInformationPolicy2 is
                                PolicyAuditFullQueryInformation then Type of access required MUST
                                be POLICY_VIEW_AUDIT_INFORMATION");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyAuditLogInformation)
                        {
                            #region MS-LSAD_R259

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                259,
                                @"If InformationClass value of LsarQueryInformationPolicy2 is PolicyAuditLogInformation
                                then Type of access required MUST be POLICY_VIEW_AUDIT_INFORMATION(0x00000002)");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyAuditEventsInformation)
                        {
                            #region MS-LSAD_R260
                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                260,
                                @"If InformationClass value of LsarQueryInformationPolicy2 is 
                                PolicyAuditEventsInformation then Type of access required MUST 
                                be POLICY_VIEW_AUDIT_INFORMATION(0x00000002)");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyPrimaryDomainInformation)
                        {
                            #region MS-LSAD_R261

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                261,
                                @"If InformationClass value of LsarQueryInformationPolicy2 is 
                                PolicyPrimaryDomainInformation then Type of access required MUST 
                                be POLICY_VIEW_LOCAL_INFORMATION((0x00000001)");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyAccountDomainInformation)
                        {
                            #region MS-LSAD_R263

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                263,
                                @"If InformationClass value of LsarQueryInformationPolicy2 is 
                                PolicyAccountDomainInformation then Type of access required MUST
                                be POLICY_VIEW_LOCAL_INFORMATION");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyLsaServerRoleInformation)
                        {
                            #region MS-LSAD_R264

                            @Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                264,
                                @"If InformationClass value of LsarQueryInformationPolicy2 is 
                                PolicyLsaServerRoleInformation then Type of access required MUST
                                be POLICY_VIEW_LOCAL_INFORMATION(0x00000001)");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyReplicaSourceInformation)
                        {
                            #region MS-LSAD_R265

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                265,
                                @"If InformationClass value of LsarQueryInformationPolicy2 is 
                                PolicyReplicaSourceInformation then Type of access required MUST 
                                be POLICY_VIEW_LOCAL_INFORMATION(0x00000001)");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyDnsDomainInformation)
                        {
                            #region MS-LSAD_R269

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                269,
                                @"If InformationClass value of LsarQueryInformationPolicy2 is 
                                PolicyDnsDomainInformation then Type of access required MUST be
                                POLICY_VIEW_LOCAL_INFORMATION");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyDnsDomainInformationInt)
                        {
                            #region MS-LSAD_R270

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                270,
                                @"If InformationClass value of LsarQueryInformationPolicy2 is 
                                PolicyDnsDomainInformationInt then Type of access required MUST
                                be POLICY_VIEW_LOCAL_INFORMATION (0x00000001)");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyLocalAccountDomainInformation)
                        {
                            if (this.serverPlatform == Server.Windows2k8)
                            {
                                #region MS-LSAD_R271

                                Site.CaptureRequirementIfAreEqual<uint>(
                                    ((uint)ErrorStatus.AccessDenied),
                                    (uint)uintMethodStatus,
                                    "MS-LSAD",
                                    271,
                                    @"If InformationClass value of LsarQueryInformationPolicy2 is
                                    PolicyLocalAccountDomainInformation then Type of access required
                                    MUST be POLICY_VIEW_LOCAL_INFORMATION");

                                #endregion
                            }
                        }

                        if (informationType == InformationClass.PolicyPdAccountInformation)
                        {
                            #region MS-LSAD_R262

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                262,
                                @"If InformationClass value of LsarQueryInformationPolicy2 is 
                                PolicyPdAccountInformation then Type of access required MUST
                                be POLICY_GET_PRIVATE_INFORMATION(0x00000004)");

                            #endregion
                        }

                        #region MS-LSAD_R258

                        Site.CaptureRequirementIfAreEqual<uint>(
                            ((uint)ErrorStatus.AccessDenied),
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            258,
                            @"If the context PolicyHandle parameter of LsarQueryInformationPolicy2 does not have 
                            sufficient access, the server MUST return STATUS_ACCESS_DENIED(0xC0000022).");

                        #endregion

                        break;

                    default:

                        break;
                }

                #endregion
            }

            policyInformation = (((uint)uintMethodStatus == (uint)ErrorStatus.Success) 
                && this.validPolicyInfoquery == true)
                                     ? PolicyInformation.Valid : PolicyInformation.Invalid;

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion QueryInformationPolicy2

        #region QueryInformationPolicy

        /// <summary>
        /// This method is used to query policy object information
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="informationType">Contains the type of policy object information to be queried</param>
        /// <param name="policyInformation">Output Parameter which contains policy object information 
        /// which has been queried</param>
        /// <returns>Returns Success if the method is successful
        ///          Returns InvalidParameter if the parameters passed to the method are not valid
        ///          Returns AccessDenied if the caller does not have the permissions 
        ///          to perform the operation
        /// Returns InvalidHandle if the policy handle passed is not valid</returns>
        /// Disable warning CA1502 and CA1505 because it will affect the implementation of Adapter and Model 
        /// codes if do any changes about maintainability.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity",
            Justification = "affect the implementation of Adapter and Model codes")]
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode",
            Justification = "affect the implementation of Adapter and Model codes")]
        public ErrorStatus QueryInformationPolicy(
            int handleInput,
            InformationClass informationType,
            out PolicyInformation policyInformation)
        {
            InformationClass policyInformationClass2 = new InformationClass();
            _LSAPR_POLICY_INFORMATION? policyInfoinServer = new _LSAPR_POLICY_INFORMATION();
            this.Invalidhandlle = false;
            this.invalidParam = false;

            #region CheckingInvalidParameter

            ////Setting invalidParam flag for Information class which will return Invalid Parameter
            if ((informationType == InformationClass.PolicyModificationInformation)
                     || (informationType == InformationClass.PolicyAuditFullQueryInformation)
                     || (informationType == InformationClass.PolicyAuditFullSetInformation)
                     || (informationType == InformationClass.Invalid))
            {
                this.invalidParam = true;
            }

            #endregion CheckingInvalidParameter

            #region CreatingInvalidHandle

            if (stPolicyInformation.PHandle != handleInput)
            {
                ////CreateAnInvalidHandle() will create invalid handle to check requirement of Invalid Handle
                objSecretHandle = utilities.CreateAnInvalidHandle(false);
                this.InvalidPolicyHandle = objSecretHandle;
                PolicyHandle = this.InvalidPolicyHandle;
                this.Invalidhandlle = true;
            }
            else
            {
                PolicyHandle = this.ValidPolicyHandle;
                this.Invalidhandlle = false;
            }

            #endregion CreatingInvalidHandle

            #region checkInfovalues

            ////Setting intCheckInfoClass with return status for capturing requirement
            switch (informationType)
            {
                case InformationClass.PolicyAccountDomainInformation:
                    policyInformationClass2 = InformationClass.PolicyAccountDomainInformation;

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION) !=
                        ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;
                case InformationClass.PolicyAuditEventsInformation:
                    policyInformationClass2 = InformationClass.PolicyAuditEventsInformation;

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_VIEW_AUDIT_INFORMATION) != 
                        ACCESS_MASK.POLICY_VIEW_AUDIT_INFORMATION)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;
                case InformationClass.PolicyAuditFullQueryInformation:

                    policyInformationClass2 = InformationClass.PolicyAuditFullQueryInformation;

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_VIEW_AUDIT_INFORMATION) != 
                        ACCESS_MASK.POLICY_VIEW_AUDIT_INFORMATION)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }

                    if (this.isWindows)
                    {
                        if (PDCOSVersion >= ServerVersion.Win2003)
                        {
                            intCheckInfoClass = LsadUtilities.ReturnInvalidParameter;
                        }
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyAuditFullSetInformation:

                    policyInformationClass2 = InformationClass.PolicyAuditFullSetInformation;
                    intCheckInfoClass = LsadUtilities.ReturnInvalidParameter;

                    break;

                case InformationClass.PolicyAuditLogInformation:

                    policyInformationClass2 = InformationClass.PolicyAuditLogInformation;

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_VIEW_AUDIT_INFORMATION) != 
                        ACCESS_MASK.POLICY_VIEW_AUDIT_INFORMATION)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyDefaultQuotaInformation:

                    policyInformationClass2 = InformationClass.PolicyDefaultQuotaInformation;

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION) != 
                        ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyDnsDomainInformation:

                    policyInformationClass2 = InformationClass.PolicyDnsDomainInformation;

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION) != 
                        ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyDnsDomainInformationInt:

                    policyInformationClass2 = InformationClass.PolicyDnsDomainInformationInt;

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION) != 
                        ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyLocalAccountDomainInformation:

                    policyInformationClass2 = InformationClass.PolicyLocalAccountDomainInformation;

                    if (this.serverPlatform == Server.Windows2k8)
                    {
                        if ((this.uintAccessMask & ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION) != 
                            ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION)
                        {
                            intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                        }
                        else
                        {
                            intCheckInfoClass = LsadUtilities.ReturnSuccess;
                        }
                    }

                    break;

                case InformationClass.PolicyLsaServerRoleInformation:

                    policyInformationClass2 = InformationClass.PolicyLsaServerRoleInformation;

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION) != 
                        ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyModificationInformation:

                    policyInformationClass2 = InformationClass.PolicyModificationInformation;
                    intCheckInfoClass = LsadUtilities.ReturnInvalidParameter;

                    break;

                case InformationClass.PolicyPdAccountInformation:

                    policyInformationClass2 = InformationClass.PolicyPdAccountInformation;

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_GET_PRIVATE_INFORMATION) != 
                        ACCESS_MASK.POLICY_GET_PRIVATE_INFORMATION)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyPrimaryDomainInformation:

                    policyInformationClass2 = InformationClass.PolicyPrimaryDomainInformation;

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION) != 
                        ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;
                case InformationClass.PolicyReplicaSourceInformation:

                    policyInformationClass2 = InformationClass.PolicyReplicaSourceInformation;

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION) != 
                        ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case InformationClass.PolicyMachineAccountInformation:
                    policyInformationClass2 = InformationClass.PolicyMachineAccountInformation;
                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_VIEW_AUDIT_INFORMATION) != ACCESS_MASK.POLICY_VIEW_AUDIT_INFORMATION)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }
                    break;

                default:

                    intCheckInfoClass = LsadUtilities.ReturnInvalidParameter;

                    break;
            }

            #endregion

            uintMethodStatus = lsadClientStack.LsarQueryInformationPolicy(
                PolicyHandle.Value,
                (_POLICY_INFORMATION_CLASS)policyInformationClass2,
                out policyInfoinServer);

            ////Capturing Requirements for Invalid Parameter status
            if (this.invalidParam == true)
            {
                if (informationType == InformationClass.PolicyModificationInformation)
                {
                    #region MS-LSAD_R321

                    Site.CaptureRequirementIfAreEqual<uint>(
                        ((uint)ErrorStatus.InvalidParameter),
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        321,
                        @"If the value of InformationClass parameter in LsarQueryInformationPolicy is 
                        PolicyModificationInformation then Information returned to caller from abstract
                        data model MUST return STATUS_InvalidParameter");

                    #endregion

                    #region MS-LSAD_R305

                    Site.CaptureRequirementIfAreEqual<uint>(
                        ((uint)ErrorStatus.InvalidParameter),
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        305,
                        @"If InformationClass value of LsarQueryInformationPolicy is PolicyModificationInformation 
                        then Type of access required cannot be queried.The request MUST fail with
                        STATUS_INVALID_PARAMETER.");

                    #endregion
                }

                if (informationType == InformationClass.PolicyAuditFullSetInformation)
                {
                    #region MS-LSAD_R306

                    Site.CaptureRequirementIfAreEqual<uint>(
                        ((uint)ErrorStatus.InvalidParameter),
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        306,
                        @"If InformationClass value of LsarQueryInformationPolicy is PolicyAuditFullSetInformation
                        then Type of access required cannot be queried. The request MUST fail with 
                        STATUS_InvalidParameter");

                    #endregion
                }

                ////Capturing requirement for Invalid Handle status
                if (informationType == InformationClass.PolicyAuditFullQueryInformation)
                {
                    if (this.isWindows)
                    {
                        if (PDCOSVersion >= ServerVersion.Win2003)
                        {
                            #region MS-LSAD-322

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.InvalidParameter,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                322,
                                @"If the value of InformationClass parameter in LsarQueryInformationPolicy is 
                                PolicyAuditFullQueryInformation then Windows XP, Windows Server 2003,
                                Windows Vista, and Windows Server 2008 return STATUS_INVALID_PARAMETER for
                                this information class");

                            #endregion
                        }
                    }
                }
            }
            else if (this.Invalidhandlle == true)
            {
                utilities.CreateAnInvalidHandle(true);

                #region MS-LSAD_R294

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidHandle,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    294,
                    @"LsarQueryInformationPolicy returns STATUS_InvalidHandle(0xC0000008), 
                    if the handle is not a Valid policy object handle");

                #endregion
            }

            ////Capturing requirements for other return status
            if (!this.Invalidhandlle)
            {
                #region checkinfoclasss

                switch (intCheckInfoClass)
                {
                    ////Capturing requirements for Success status
                    case 0:
                        if (informationType == InformationClass.PolicyAuditLogInformation)
                        {
                            this.validPolicyInfoquery = utilities.ifQueryauditLogInfo(policyInfoinServer.Value);

                            if (this.isWindows)
                            {
                                #region MS-LSAD_R312

                                Site.CaptureRequirementIfIsTrue(
                                    (((uint)uintMethodStatus == (uint)ErrorStatus.Success)
                                        && this.validPolicyInfoquery == true),
                                    "MS-LSAD",
                                    312,
                                    @"If the value of InformationClass parameter in LsarQueryInformationPolicy is 
                                    PolicyAuditLogInformation then Information returned to caller from abstract data
                                    model MUST be Auditing Log Information");

                                #endregion
                            }
                        }

                        if (informationType == InformationClass.PolicyAuditEventsInformation)
                        {
                            this.validPolicyInfoquery = utilities.ifQueryauditeventsInfo(policyInfoinServer.Value);

                            #region MS-LSAD_R313

                            Site.CaptureRequirementIfIsTrue(
                                (((uint)uintMethodStatus == (uint)ErrorStatus.Success)
                                    && (this.validPolicyInfoquery == true)),
                                "MS-LSAD",
                                313,
                                @"If the value of InformationClass parameter in LsarQueryInformationPolicy is 
                                PolicyAuditEventsInformation then Information returned to caller from abstract 
                                data model MUST be Event Auditing Options");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyPrimaryDomainInformation)
                        {
                            this.validPolicyInfoquery = utilities.ifQueryPrimarydomainInfo(policyInfoinServer.Value);

                            #region MS-LSAD_R314

                            Site.CaptureRequirementIfIsTrue(
                                (((uint)uintMethodStatus == (uint)ErrorStatus.Success) 
                                && this.validPolicyInfoquery == true),
                                "MS-LSAD",
                                314,
                                @"If the value of InformationClass parameter in LsarQueryInformationPolicy2 
                                is PolicyPrimaryDomainInformation then Information returned to caller from 
                                abstract data model MUST be Primary Domain Information");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyAccountDomainInformation)
                        {
                            if (isDC == false)
                            {
                                this.validPolicyInfoquery = 
                                    utilities.ifQueryAccountdomInfofornondc(policyInfoinServer.Value);

                                #region MS-LSAD_R316

                                Site.CaptureRequirementIfIsTrue(
                                    ((uint)uintMethodStatus ==
                                        (uint)ErrorStatus.Success && this.validPolicyInfoquery == true),
                                    "MS-LSAD",
                                    316,
                                    @"If the value of InformationClass parameter in LsarQueryInformationPolicy is 
                                    PolicyAccountDomainInformation then Information returned to caller from abstract 
                                    data model on non-domain controllers MUST be Account Domain");

                                #endregion
                            }
                            else
                            {
                                this.validPolicyInfoquery = utilities.ifQueryAccountdomInfo(policyInfoinServer.Value);

                                #region MS-LSAD_R873

                                Site.CaptureRequirementIfIsTrue(
                                    ((uint)uintMethodStatus ==
                                        (uint)ErrorStatus.Success && this.validPolicyInfoquery == true),
                                    "MS-LSAD",
                                    873,
                                    @"If the value of InformationClass parameter in LsarQueryInformationPolicy is 
                                    PolicyAccountDomainInformation then Information returned to caller from abstract
                                    data model on domain controller MUST return Primary Domain Information");

                                #endregion
                            }
                        }

                        if (informationType == InformationClass.PolicyLsaServerRoleInformation)
                        {
                            this.validPolicyInfoquery = utilities.ifQueryLsaSrvrroleInfo(policyInfoinServer.Value);

                            if (isDC == true)
                            {
                                #region MS-LSAD_R317

                                Site.CaptureRequirementIfIsTrue(
                                    (((uint)uintMethodStatus ==
                                        (uint)ErrorStatus.Success) && this.validPolicyInfoquery == true),
                                    "MS-LSAD",
                                    317,
                                    @"If the value of InformationClass parameter in LsarQueryInformationPolicy is
                                    PolicyLsaServerRoleInformation then Information returned to caller from abstract 
                                    data model on domain controller MUST be Primary Domain Information");

                                #endregion
                            }

                            #region MS-LSAD_R318

                            Site.CaptureRequirementIfIsTrue(
                                (((uint)uintMethodStatus == (uint)ErrorStatus.Success) 
                                && this.validPolicyInfoquery == true),
                                "MS-LSAD",
                                318,
                                @"If the value of InformationClass parameter in LsarQueryInformationPolicy is
                                PolicyLsaServerRoleInformation then Information returned to caller from abstract
                                data model MUST be Server Role Information");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyReplicaSourceInformation)
                        {
                            this.validPolicyInfoquery = utilities.ifQueryReplicaSrcInfo(policyInfoinServer.Value);

                            #region MS-LSAD_R319

                            Site.CaptureRequirementIfIsTrue(
                                (((uint)uintMethodStatus == (uint)ErrorStatus.Success) 
                                && this.validPolicyInfoquery == true),
                                "MS-LSAD",
                                319,
                                @"If the value of InformationClass parameter in LsarQueryInformationPolicy is
                                PolicyReplicaSourceInformation then Information returned to caller from abstract
                                data model MUST be Replica Source Information");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyDnsDomainInformation)
                        {
                            this.validPolicyInfoquery = utilities.ifQueryDnsDomInfo(policyInfoinServer.Value);

                            #region MS-LSAD_R323

                            Site.CaptureRequirementIfIsTrue(
                                (((uint)uintMethodStatus == (uint)ErrorStatus.Success) 
                                && this.validPolicyInfoquery == true),
                                "MS-LSAD",
                                323,
                                @"If the value of InformationClass parameter in LsarQueryInformationPolicy is 
                                PolicyDnsDomainInformation then Information returned to caller from abstract data
                                model MUST be DNS Domain Information");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyDnsDomainInformationInt)
                        {
                            this.validPolicyInfoquery = utilities.ifQueryDnsDomIntInfo(policyInfoinServer.Value);

                            #region MS-LSAD_R324

                            Site.CaptureRequirementIfIsTrue(
                                (((uint)uintMethodStatus ==
                                    (uint)ErrorStatus.Success) && this.validPolicyInfoquery == true),
                                "MS-LSAD",
                                324,
                                @"If the value of InformationClass parameter in LsarQueryInformationPolicy is 
                                PolicyDnsDomainInformationInt then Information returned to caller from abstract
                                data model MUST be DNS Domain Information");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyLocalAccountDomainInformation)
                        {
                            if (this.serverPlatform == Server.Windows2k8)
                            {
                                this.validPolicyInfoquery = utilities.ifQueryLocalaccountInfo(policyInfoinServer.Value);

                                #region MS-LSAD_R325

                                Site.CaptureRequirementIfIsTrue(
                                    (((uint)uintMethodStatus == (uint)ErrorStatus.Success)
                                        && this.validPolicyInfoquery == true),
                                    "MS-LSAD",
                                    325,
                                    @"If the value of InformationClass parameter in LsarQueryInformationPolicy is
                                    PolicyLocalAccountDomainInformation then Information returned to caller from 
                                    abstract data model MUST be AccountDomainInformation");

                                #endregion
                            }
                        }

                        if (informationType == InformationClass.PolicyPdAccountInformation)
                        {
                            _LSAPR_POLICY_INFORMATION policyInfoinServer1 = policyInfoinServer.Value;
                            policyInfoinServer1.PolicyPdAccountInfo.Name = new _RPC_UNICODE_STRING();
                            this.validPolicyInfoquery = utilities.ifQueryPdaccountInfo(policyInfoinServer.Value);

                            #region MS-LSAD_R315

                            Site.CaptureRequirementIfIsTrue(
                                (((uint)uintMethodStatus == (uint)ErrorStatus.Success) 
                                && this.validPolicyInfoquery == true),
                                "MS-LSAD",
                                315,
                                @"If the value of InformationClass parameter in LsarQueryInformationPolicy is
                                PolicyPdAccountInformation then Information returned to caller from abstract data
                                model MUST return an LSAPR_POLICY_PD_ACCOUNT_INFO information structure initialized 
                                with zeros");

                            #endregion
                        }

                        #region MS-LSAD_R289

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.Success,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            289,
                            @"The return Values for Opnum LsarQueryInformationPolicy that an implementation MUST 
                            return when the request was successfully completed isSTATUS_Success(0x00000000)");

                        #endregion

                        #region MS-LSAD_R293

                        Site.CaptureRequirementIfIsTrue(
                            (((uint)uintMethodStatus == (uint)ErrorStatus.Success) && PolicyHandle != IntPtr.Zero),
                            "MS-LSAD",
                            293,
                            @"PolicyHandle parameter of LsarQueryInformationPolicy MUST be a handle to an open
                            policy object");

                        #endregion

                        #region MS-LSAD_R295

                        Site.CaptureRequirementIfIsTrue(
                            (((uint)uintMethodStatus == (uint)ErrorStatus.Success) && PolicyHandle != IntPtr.Zero),
                            "MS-LSAD",
                            295,
                            @"PolicyHandle parameter of LsarQueryInformationPolicy MUST reference a context that was 
                            granted an access commensurate with the InformationClass value requested.");

                        #endregion
                        break;
                    ////Capturing Requirements for general Invalid Parameter status
                    case 1:
                        #region MS-LSAD_R291

                        Site.CaptureRequirementIfAreEqual<uint>(
                            ((uint)ErrorStatus.InvalidParameter),
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            291,
                            @"The return Values  of Opnum LsarQueryInformationPolicy that an implementation MUST 
                            return  when one of the parameters is incorrect is STATUS_InvalidParameter(0xC000000D). 
                            For instance, this can happen if InformationClass is out of range or if PolicyInformation 
                            is Null.");

                        #endregion
                        break;

                    ////Capturing requirements for Access Denied status
                    case 3:
                        if (informationType == InformationClass.PolicyAuditFullQueryInformation)
                        {
                            #region MS-LSAD_R307

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                307,
                                @"If InformationClass value of LsarQueryInformationPolicy is 
                                PolicyAuditFullQueryInformation then Type of access required MUST
                                be POLICY_VIEW_AUDIT_INFORMATION");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyAuditLogInformation)
                        {
                            #region MS-LSAD_R297

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                297,
                                @"If InformationClass value of LsarQueryInformationPolicy is
                                PolicyAuditLogInformation then Type of access required MUST 
                                be POLICY_VIEW_AUDIT_INFORMATION(0x00000002)");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyAuditEventsInformation)
                        {
                            #region MS-LSAD_R298

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                298,
                                @"If InformationClass value of LsarQueryInformationPolicy is
                                PolicyAuditEventsInformation then Type of access required MUST
                                be POLICY_VIEW_AUDIT_INFORMATION(0x00000002)");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyPrimaryDomainInformation)
                        {
                            #region MS-LSAD_R299

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                299,
                                @"If InformationClass value of LsarQueryInformationPolicy2 is 
                                PolicyPrimaryDomainInformation then Type of access required MUST
                                be POLICY_VIEW_LOCAL_INFORMATION((0x00000001)");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyAccountDomainInformation)
                        {
                            #region MS-LSAD_R301

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                301,
                                @"If InformationClass value of LsarQueryInformationPolicy is 
                                PolicyAccountDomainInformation then Type of access required MUST
                                be POLICY_VIEW_LOCAL_INFORMATION((0x00000001)");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyLsaServerRoleInformation)
                        {
                            #region MS-LSAD_R302

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                302,
                                @"If InformationClass value of LsarQueryInformationPolicy is 
                                PolicyLsaServerRoleInformation then Type of access required MUST 
                                be POLICY_VIEW_LOCAL_INFORMATION(0x00000001)");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyReplicaSourceInformation)
                        {
                            #region MS-LSAD_R303

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                303,
                                @"If InformationClass value of LsarQueryInformationPolicy is
                                PolicyReplicaSourceInformation then Type of access required MUST 
                                be POLICY_VIEW_LOCAL_INFORMATION(0x00000001)");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyDnsDomainInformation)
                        {
                            #region MS-LSAD_R308

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                308,
                                @"If InformationClass value of LsarQueryInformationPolicy is
                                PolicyDnsDomainInformation then Type of access required MUST
                                be POLICY_VIEW_LOCAL_INFORMATION(0x00000001)");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyDnsDomainInformationInt)
                        {
                            #region MS-LSAD_R309

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                309,
                                @"If InformationClass value of LsarQueryInformationPolicy is
                                PolicyDnsDomainInformationInt then Type of access required MUST 
                                be POLICY_VIEW_LOCAL_INFORMATION (0x00000001)");

                            #endregion
                        }

                        if (informationType == InformationClass.PolicyLocalAccountDomainInformation)
                        {
                            if (this.serverPlatform == Server.Windows2k8)
                            {
                                #region MS-LSAD_R310

                                Site.CaptureRequirementIfAreEqual<uint>(
                                    ((uint)ErrorStatus.AccessDenied),
                                    (uint)uintMethodStatus,
                                    "MS-LSAD",
                                    310,
                                    @"If InformationClass value of LsarQueryInformationPolicy is
                                    PolicyLocalAccountDomainInformation then Type of access required MUST
                                    be POLICY_VIEW_LOCAL_INFORMATION (0x00000001)");

                                #endregion
                            }
                        }

                        if (informationType == InformationClass.PolicyPdAccountInformation)
                        {
                            #region MS-LSAD_R300

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                300,
                                @"If InformationClass value of LsarQueryInformationPolicy is PolicyPdAccountInformation
                                then Type of access required MUST be POLICY_GET_PRIVATE_INFORMATION(0x00000004)");

                            #endregion
                        }

                        #region MS-LSAD_R296

                        Site.CaptureRequirementIfAreEqual<uint>(
                            ((uint)ErrorStatus.AccessDenied),
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            296,
                            @"If the context PolicyHandle parameter of LsarQueryInformationPolicy does not have
                            sufficient access, the server MUST return STATUS_ACCESS_DENIED(0xC0000022).");

                        #endregion
                        break;
                    default:
                        break;
                }

                #endregion
            }

            policyInformation = (((uint)uintMethodStatus == (uint)ErrorStatus.Success) 
                && this.validPolicyInfoquery == true)
                                     ? PolicyInformation.Valid : PolicyInformation.Invalid;

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion QueryInformationPolicy

        #region SetDomainInformationPolicy

        /// <summary>
        /// This method is used to set policy object domain information
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="informationType">Contains the type of policy object domain information to be set</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions 
        /// to perform the operation
        /// Returns InvalidHandle if the policy handle passed is not valid</returns>
        /// Disable warning CA1502 because it will affect the implementation of Adapter and Model codes if 
        /// do any changes about maintainability.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity",
            Justification = "affect the implementation of Adapter and Model codes")]
        public ErrorStatus SetDomainInformationPolicy(int handleInput, DomainInformationClass informationType)
        {
            _POLICY_DOMAIN_INFORMATION_CLASS domainInformationClass = new _POLICY_DOMAIN_INFORMATION_CLASS();
            _LSAPR_POLICY_DOMAIN_INFORMATION domainInfoinServer = new _LSAPR_POLICY_DOMAIN_INFORMATION();
            this.Invalidhandlle = false;

            #region CheckInvalidHandle

            if (stPolicyInformation.PHandle != handleInput)
            {
                // CreateAnInvalidHandle() will create invalid handle to check requirement of Invalid Handle
                objSecretHandle = utilities.CreateAnInvalidHandle(false);
                this.InvalidPolicyHandle = objSecretHandle;
                PolicyHandle = this.InvalidPolicyHandle;
                this.Invalidhandlle = true;
            }
            else
            {
                PolicyHandle = this.ValidPolicyHandle;
                this.Invalidhandlle = false;
            }

            #endregion CheckInvalidHandle

            #region checkinfovalues

            switch (informationType)
            {
                // Passing Input to Set Domain Information class and setting intCheckInfoClass with return status
                case DomainInformationClass.PolicyDomainEfsInformation:
                    domainInformationClass = _POLICY_DOMAIN_INFORMATION_CLASS.PolicyDomainEfsInformation;
                    domainInfoinServer.PolicyDomainEfsInfo = new _LSAPR_POLICY_DOMAIN_EFS_INFO();
                    domainInfoinServer.PolicyDomainEfsInfo.EfsBlob = utilities.CreateEfsblob();
                    domainInfoinServer.PolicyDomainEfsInfo.InfoLength = InfoLength;

                    // Win2k8r2 unsupport PolicyDomainEfsInformation in informationType parameter 
                    // and will return STATUS_INVALID_PARAMETER.
                    if (PDCOSVersion >= ServerVersion.Win2008R2)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnInvalidParameter;
                    }
                    else if ((uintAccessMask & ACCESS_MASK.POLICY_SERVER_ADMIN) != ACCESS_MASK.POLICY_SERVER_ADMIN)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;
                case DomainInformationClass.PolicyDomainKerberosTicketInformation:
                    domainInformationClass = _POLICY_DOMAIN_INFORMATION_CLASS.PolicyDomainKerberosTicketInformation;
                    domainInfoinServer.PolicyDomainKerbTicketInfo = new _POLICY_DOMAIN_KERBEROS_TICKET_INFO();
                    domainInfoinServer.PolicyDomainKerbTicketInfo.AuthenticationOptions = Authenticationoption;
                    domainInfoinServer.PolicyDomainKerbTicketInfo.MaxServiceTicketAge.QuadPart = Maxservicequadpart;
                    domainInfoinServer.PolicyDomainKerbTicketInfo.MaxTicketAge.QuadPart = Maxticketquadpart;
                    domainInfoinServer.PolicyDomainKerbTicketInfo.MaxRenewAge.QuadPart = MaxRenewquadpart;
                    domainInfoinServer.PolicyDomainKerbTicketInfo.MaxClockSkew.QuadPart = MaxClockskewquad;

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_SERVER_ADMIN) != ACCESS_MASK.POLICY_SERVER_ADMIN)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;
                case DomainInformationClass.PolicyDomainQualityOfServiceInformation:
                    domainInformationClass = _POLICY_DOMAIN_INFORMATION_CLASS.PolicyDomainQualityOfServiceInformation;
                    domainInfoinServer.PolicyDomainQualityOfServiceInfo.QualityOfService = 0;

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_SERVER_ADMIN) != ACCESS_MASK.POLICY_SERVER_ADMIN)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;
                default:
                    intCheckInfoClass = LsadUtilities.ReturnInvalidParameter;
                    break;
            }

            #endregion
            
            uintMethodStatus = lsadClientStack.LsarSetDomainInformationPolicy(
                (IntPtr)PolicyHandle,
                domainInformationClass,
                domainInfoinServer);

            if (informationType == DomainInformationClass.PolicyDomainEfsInformation
                    && domainInfoinServer.PolicyDomainEfsInfo.InfoLength != 0)
            {
                #region MS-LSAD_R107

                Site.CaptureRequirementIfIsTrue(
                    (domainInfoinServer.PolicyDomainEfsInfo.EfsBlob != null),
                    "MS-LSAD",
                    107,
                    @"In LSAPR_POLICY_DOMAIN_EFS_INFO structure under field 'EfsBlob' : If the value of 
                    InfoLength is other than 0, EfsBlob field MUST NOT be NULL.");

                #endregion
            }

            ////Capturing requirement for Invalid Handle status
            if (this.Invalidhandlle == true && intCheckInfoClass != LsadUtilities.ReturnInvalidParameter)
            {
                utilities.CreateAnInvalidHandle(true);

                #region MS-LSAD_R418
                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidHandle,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    418,
                    @"If PolicyHandle in LsarSetDomainInformationPolicy is not a valid context handle, 
                    the server MUST return STATUS_INVALID_HANDLE(0xC0000008).");

                #endregion
            }

            ////Capturing requirements for other status
            if (!this.Invalidhandlle)
            {
                #region checkInfoclass

                switch (intCheckInfoClass)
                {
                    ////capturing requirements for Success status
                    case 0:
                        #region MS-LSAD_R414

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.Success,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            414,
                            @"The return Value of Opnum LsarSetDomainInformationPolicy that an implementation MUST
                            return is STATUS_SUCCESS(0x00000000) if the request was successfully completed.");

                        #endregion

                        #region MS-LSAD_R417

                        Site.CaptureRequirementIfIsTrue(
                            ((uintMethodStatus != NtStatus.STATUS_ACCESS_DENIED) && PolicyHandle != IntPtr.Zero),
                            "MS-LSAD",
                            417,
                            @"PolicyHandle  in LsarSetDomainInformationPolicy MUST reference a context that was granted 
                            an access commensurate with the InformationClass value requested.");

                        #endregion

                        if (informationType == DomainInformationClass.PolicyDomainEfsInformation
                                && domainInfoinServer.PolicyDomainEfsInfo.EfsBlob != null)
                        {
                            if (this.isWindows && (uint)ErrorStatus.Success == (uint)uintMethodStatus)
                            {
                                ////Windows do not enforce the EfsBlob, just captureRequirement directly
                                #region MS-LSAD_R20107

                                Site.CaptureRequirementIfAreEqual<uint>(
                                    (uint)ErrorStatus.Success,
                                    (uint)uintMethodStatus,
                                    "MS-LSAD",
                                    20107,
                                    @"<16> Section 2.2.4.18: Microsoft implementations of the Local Security 
                                    Authority (Domain Policy) Remote Protocol do not enforce data in EfsBlob to 
                                    conform to the layout specified in [MS-GPEF] section 2.2.1.2.1.");

                                #endregion

                                #region  MS-LSAD_R10107

                                Site.CaptureRequirementIfAreEqual<uint>(
                                    (uint)ErrorStatus.Success,
                                    (uint)uintMethodStatus,
                                    "MS-LSAD",
                                    10107,
                                    @"[In LSAPR_POLICY_DOMAIN_EFS_INFO structure] EfsBlob: The syntax of this
                                    blob SHOULD<16> conform to the layout specified in [MS-GPEF] section 2.2.1.2.1.");

                                #endregion
                            }

                            if ((uint)ErrorStatus.Success == (uint)uintMethodStatus)
                            {
                                #region MS-LSAD_R10107

                                //If the request is responsed normally by server, and the EfsBlob is valid in the response,requirement R10107 is captured
                                Site.CaptureRequirementIfIsNotNull(
                                    domainInfoinServer.PolicyDomainEfsInfo.EfsBlob,
                                    "MS-LSAD",
                                    10107,
                                    @"[In LSAPR_POLICY_DOMAIN_EFS_INFO structure] EfsBlob: The syntax of this blob 
                                    SHOULD<16> conform to the layout specified in [MS-GPEF] section 2.2.1.2.1.");

                                #endregion
                            }
                        }

                        break;

                    ////Capturing Requirements for Invalid Parameter status
                    case 1:
                        bool bEFSIfIsSuproted = EFSIfIsSuproted;

                        if (!bEFSIfIsSuproted && informationType == DomainInformationClass.PolicyDomainEfsInformation)
                        {
                            #region MS-LSAD_R415

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.InvalidParameter,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                415,
                                @"If the InformationClass parameter in LsarSetDomainInformationPolicy is 
                                PolicyDomainEfsInformation, and the responder implementation does not support
                                Encrypting File System (EFS) Policy Information, then request MUST fail with 
                                STATUS_InvalidParameter(0xC000000D).");

                            #endregion

                            #region MS-LSAD_R420

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.InvalidParameter,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                420,
                                @"If the InformationClass parameter of Opnum LsarSetDomainInformationPolicy is
                                PolicyDomainEfsInformation and the responder implementation does not support 
                                Encrypting File System (EFS) Policy Information, then the request MUST fail with 
                                STATUS_InvalidParameter.");

                            #endregion
                        }

                        #region MS-LSAD_R20107

                        Site.CaptureRequirementIfIsTrue(
                            (domainInfoinServer.PolicyDomainEfsInfo.EfsBlob != null),
                            "MS-LSAD",
                            20107,
                            @"<16> Section 2.2.4.18: Microsoft implementations of the Local Security Authority 
                            (Domain Policy) Remote Protocol do not enforce data in EfsBlob to conform to the layout
                            specified in [MS-GPEF] section 2.2.1.2.1.");

                        #endregion

                        #region  MS-LSAD_R10107

                        Site.CaptureRequirementIfIsTrue(
                            ((domainInfoinServer.PolicyDomainEfsInfo.InfoLength != 0)
                                 && (domainInfoinServer.PolicyDomainEfsInfo.EfsBlob != null)),
                            "MS-LSAD",
                            10107,
                            @"[In LSAPR_POLICY_DOMAIN_EFS_INFO structure] EfsBlob: The syntax of this blob SHOULD<16>
                            conform to the layout specified in [MS-GPEF] section 2.2.1.2.1.");

                        #endregion

                        break;

                    ////Capturing Requirements for Access denied status
                    case 3:
                        if (informationType == DomainInformationClass.PolicyDomainEfsInformation)
                        {
                            #region MS-LSAD_R423

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.AccessDenied,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                423,
                                @"If InformationClass value of LsarSetDomainInformationPolicy is 
                                PolicyDomainEfsInformation then type of access required MUST be
                                POLICY_SERVER_ADMIN");

                            #endregion
                        }

                        if (informationType == DomainInformationClass.PolicyDomainKerberosTicketInformation)
                        {
                            #region MS-LSAD_R424

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.AccessDenied,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                424,
                                @"If InformationClass value of LsarSetDomainInformationPolicy is 
                                PolicyDomainKerberosTicketInformation then type of access required
                                MUST be POLICY_SERVER_ADMIN");

                            #endregion
                        }

                        if (informationType == DomainInformationClass.PolicyDomainQualityOfServiceInformation)
                        {
                            #region MS-LSAD_R422

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.AccessDenied,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                422,
                                @"If InformationClass value of LsarSetDomainInformationPolicy is 
                                PolicyDomainQualityOfServiceInformation then type of access required
                                MUST be POLICY_SERVER_ADMIN");

                            #endregion
                        }

                        #region MS-LSAD_R419

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.AccessDenied,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            419,
                            @"If the context in LsarSetDomainInformationPolicy does not have sufficient access,
                            the server MUST return STATUS_AccessDenied(0xC0000022).");

                        #endregion

                        break;
                    default:
                        break;
                }

                #endregion
            }

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion SetDomainInformationPolicy

        #region QueryDomainInformationPolicy

        /// <summary>
        /// This method is used to query policy object domain information
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="informationType">Contains the type of policy object domain information to be queried</param>
        /// <param name="policyInformation">Output Parameter which contains policy object domain information 
        /// which has been queried</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions 
        /// to perform the operation
        /// Returns InvalidHandle if the policy handle passed is not valid
        /// Returns ObjectNameNotFound if no value has been set for policy object domain information.</returns>
        /// Disable warning CA1502 because it will affect the implementation of Adapter and Model codes if do any 
        /// changes about maintainability.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity",
            Justification = "affect the implementation of Adapter and Model codes")]
        public ErrorStatus QueryDomainInformationPolicy(
            int handleInput,
            DomainInformationClass informationType,
            out PolicyInformation policyInformation)
        {
            _POLICY_DOMAIN_INFORMATION_CLASS domainInformationClass = new _POLICY_DOMAIN_INFORMATION_CLASS();
            _LSAPR_POLICY_DOMAIN_INFORMATION? domainInfoinServer = new _LSAPR_POLICY_DOMAIN_INFORMATION();
            this.Invalidhandlle = false;
            this.invalidParam = false;

            #region CreateInvalidHandle

            if (stPolicyInformation.PHandle != handleInput)
            {
                // CreateAnInvalidHandle() will create invalid handle to check requirement of Invalid Handle
                objSecretHandle = utilities.CreateAnInvalidHandle(false);
                this.InvalidPolicyHandle = objSecretHandle;
                PolicyHandle = this.InvalidPolicyHandle;
                this.Invalidhandlle = true;
            }
            else
            {
                PolicyHandle = this.ValidPolicyHandle;
                this.Invalidhandlle = false;
            }

            #endregion CreateInvalidHandle

            #region checkinfovalues

            switch (informationType)
            {
                // Setting intCheckInfoClass with return status
                case DomainInformationClass.PolicyDomainEfsInformation:

                    domainInformationClass = _POLICY_DOMAIN_INFORMATION_CLASS.PolicyDomainEfsInformation;

                    // Win2k8r2 unsupport PolicyDomainEfsInformation in informationType parameter and
                    // will return STATUS_INVALID_PARAMETER.                    
                    if (PDCOSVersion >= ServerVersion.Win2008R2)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnInvalidParameter;
                    }
                    else if ((uintAccessMask & ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION) != 
                        ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case DomainInformationClass.PolicyDomainKerberosTicketInformation:

                    domainInformationClass = _POLICY_DOMAIN_INFORMATION_CLASS.PolicyDomainKerberosTicketInformation;

                    if ((this.uintAccessMask & ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION) != 
                        ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                case DomainInformationClass.PolicyDomainQualityOfServiceInformation:

                    domainInformationClass = _POLICY_DOMAIN_INFORMATION_CLASS.PolicyDomainQualityOfServiceInformation;

                    if (PDCOSVersion >= ServerVersion.Win2003)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnInvalidParameter;
                    }
                    else if ((uintAccessMask & ACCESS_MASK.POLICY_VIEW_AUDIT_INFORMATION) != 
                        ACCESS_MASK.POLICY_VIEW_AUDIT_INFORMATION)
                    {
                        intCheckInfoClass = LsadUtilities.ReturnAccessDenied;
                    }
                    else
                    {
                        intCheckInfoClass = LsadUtilities.ReturnSuccess;
                    }

                    break;

                default:

                    intCheckInfoClass = LsadUtilities.ReturnInvalidParameter;

                    break;
            }

            #endregion

            uintMethodStatus = lsadClientStack.LsarQueryDomainInformationPolicy(
                PolicyHandle.Value,
                domainInformationClass,
                out domainInfoinServer);

            ////Capturing Requirements for PolicyDomainQualityOfServiceInformation DomainInformationClass
            ////Capturing Requirements for Invalid Handle status
            if (informationType == DomainInformationClass.PolicyDomainQualityOfServiceInformation)
            {
                if (this.isWindows)
                {
                    if (PDCOSVersion >= ServerVersion.Win2003)
                    {
                        #region MS-LSAD_R104

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.InvalidParameter,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            104,
                            @"<15> Section 2.2.4.16: Windows NT 3.1, windows NT 3.5, windows NT 3.51, windows NT 4.0,
                            windows XP, windows server 2003, windows vista, and windows server 2008, Windows 7 and 
                            Windows Server 2008 R2 implementations do not contain 
                            PolicyDomainQualityOfServiceInformation enumeration value and corresponding structure in
                            LSAPR_POLICY_DOMAIN_INFORMATION.");

                        #endregion
                    }
                }

                #region MS-LSAD_R399

                Site.CaptureRequirementIfAreEqual<uint>(
                    ((uint)ErrorStatus.InvalidParameter),
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    399,
                    @"If the InformationClass parameter  in LsarQueryDomainInformationPolicy is
                    PolicyDomainQualityOfServiceInformation, and the responder implementation does 
                    not support Quality Of Service Information, then request MUST fail with 
                    STATUS_INVALID_PARAMETER(0xC000000D).");

                #endregion
            }           
            else if (this.Invalidhandlle == true)
            {
                utilities.CreateAnInvalidHandle(true);

                #region MS-LSAD_R401

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidHandle,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    401,
                    @"If PolicyHandle of LsarQueryDomainInformationPolicy is not a valid context handle, 
                    the server MUST return STATUS_INVALID_HANDLE(0xC0000008).");

                #endregion
            }

            ////Capturing Requirements for other status
            if (!this.Invalidhandlle)
            {
                #region checkinfoclass

                switch (intCheckInfoClass)
                {
                    ////Capturing requirements for Success status
                    case 0:
                        if (informationType == DomainInformationClass.PolicyDomainEfsInformation)
                        {
                            this.validdomaininfoquery = utilities.validateEfsBlobInfoquery(domainInfoinServer.Value);

                            #region MS-LSAD_R409

                            Site.CaptureRequirementIfIsTrue(
                                (((uint)uintMethodStatus == (uint)ErrorStatus.Success) 
                                && this.validdomaininfoquery == true),
                                "MS-LSAD",
                                409,
                                @"If the Value of InformationClass parameter in LsarQueryDomainInformationPolicy is
                                PolicyDomainEfsInformation then Server MUST return EFS Policy Information");

                            #endregion
                        }

                        if (informationType == DomainInformationClass.PolicyDomainKerberosTicketInformation)
                        {
                            this.validdomaininfoquery = 
                                utilities.validateKerberoseticketInfoquery(domainInfoinServer.Value);

                            #region MS-LSAD_R410

                            Site.CaptureRequirementIfIsTrue(
                                (((uint)uintMethodStatus == (uint)ErrorStatus.Success) 
                                && this.validdomaininfoquery == true),
                                "MS-LSAD",
                                410,
                                @"If the Value of InformationClass parameter in LsarQueryDomainInformationPolicy is 
                                PolicyDomainKerberosTicketInformation then Server MUST return Kerberos 
                                Policy Information");

                            #endregion
                        }

                        #region MS-LSAD_R397

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.Success,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            397,
                            @"The return Values of opnum LsarQueryDomainInformationPolicy that an implementation MUST 
                            return STATUS_SUCCESS(0x00000000) if the request was successfully completed.");

                        #endregion

                        #region MS-LSAD_R400

                        Site.CaptureRequirementIfIsTrue(
                            ((uintMethodStatus != NtStatus.STATUS_ACCESS_DENIED) && (PolicyHandle != IntPtr.Zero)),
                            "MS-LSAD",
                            400,
                            @"PolicyHandle parameter of LsarQueryDomainInformationPolicy MUST reference a context that
                            was granted an access commensurate with the InformationClass value requested.");

                        #endregion

                        break;

                    ////Capturing Requirements for Invalid Parameter status
                    case 1:

                        #region MS-LSAD_R407

                        Site.CaptureRequirementIfAreEqual<uint>(
                            ((uint)ErrorStatus.InvalidParameter),
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            407,
                            @"The InformationClass parameter of Opnum LsarQueryDomainInformationPolicy can take on any
                            value in the POLICY_DOMAIN_INFORMATION_CLASS enumeration range. For all values outside this
                            range, the server MUST return the STATUS_InvalidParameter error code.");

                        #endregion

                        break;

                    ////Capturing Requirements for Access Denied status
                    case 3:
                        if (informationType == DomainInformationClass.PolicyDomainEfsInformation)
                        {
                            #region MS-LSAD_R405

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                405,
                                @"If InformationClass value of LsarQueryDomainInformationPolicy is 
                                PolicyDomainEfsInformation then type of access required is 
                                POLICY_VIEW_LOCAL_INFORMATION");

                            #endregion
                        }

                        if (informationType == DomainInformationClass.PolicyDomainKerberosTicketInformation)
                        {
                            #region MS-LSAD_R406

                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                406,
                                @"If InformationClass value of LsarQueryDomainInformationPolicy is
                                PolicyDomainKerberosTicketInformation then type of access required 
                                MUST be POLICY_VIEW_LOCAL_INFORMATION");

                            #endregion
                        }

                        if (informationType == DomainInformationClass.PolicyDomainQualityOfServiceInformation)
                        {
                            #region MS-LSAD_R403
                            Site.CaptureRequirementIfAreEqual<uint>(
                                ((uint)ErrorStatus.AccessDenied),
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                403,
                                @"If InformationClass value of LsarQueryDomainInformationPolicy is 
                                PolicyDomainQualityOfServiceInformation then type of access required 
                                is POLICY_VIEW_AUDIT_INFORMATION");

                            #endregion
                        }

                        #region MS-LSAD_R402

                        Site.CaptureRequirementIfAreEqual<uint>(
                            ((uint)ErrorStatus.AccessDenied),
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            402,
                            @"If the context of LsarQueryDomainInformationPolicy does not have sufficient access, 
                            the server MUST return STATUS_AccessDenied(0xC0000022)");
                        #endregion

                        break;
                    default:
                        break;
                }
                #endregion
            }

            policyInformation = (((uint)uintMethodStatus == (uint)ErrorStatus.Success) 
                && this.validdomaininfoquery == true)
                                      ? PolicyInformation.Valid : PolicyInformation.Invalid;
            return (ErrorStatus)uintMethodStatus;
        }

        #endregion QueryDomainInformationPolicy2

        #endregion Policy_Objects
    }
}