// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad
{
    using System;
    using System.Collections;

    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk;
    using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa;
    using Microsoft.Protocols.TestTools.StackSdk.Dtyp;

    /// <summary>
    /// Implement methods of interface ILsadManagedAdapter.
    /// </summary>
    public partial class LsadManagedAdapter
    {
        #region PrivilegeObjects

        #region LsarLookupPrivilegeValue

        /// <summary>
        ///  The LookupPrivilegeValue method is invoked to map the name of a privilege 
        ///  into a locally unique identifier (luid) by which it is known on the server.
        ///  The locally unique value of the privilege can then be used in subsequent calls 
        ///  to other methods, such as LsarAddPrivilegesToAccount.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="name">It is for validating the privilege name passed in </param>
        /// <param name="privilegeName">Contains privilege name </param>
        /// <param name="luid">Out param contains valid or invalid 
        /// luid of the passed in privilege name </param>
        /// <returns>Returns Success if the method is successful
        ///          Returns AccessDenied if the caller does not have the permissions to 
        ///          perform this operation
        ///          Returns InvalidHandle if the passed in is a valid object handle
        ///          Returns InvalidParameter if one or more of the supplied parameters was invalid
        /// Returns NoSuchPrivilege if the privilege name is not recognized by the server</returns>
        public ErrorStatus LookupPrivilegeValue(
            int handleInput,
            ValidString name,
            string privilegeName,
            out PrivilegeLUID luid)
        {
            _RPC_UNICODE_STRING inputPrivilegeName = new _RPC_UNICODE_STRING();
            Hashtable htPrivilegeNameNLUID = new Hashtable();
            _LUID? outputPrivilegeLUID = new _LUID();
            utilities.privilegeInformation(ref htPrivilegeNameNLUID);

            if (name == ValidString.Invalid)
            {
                utilities.nameOfPrivilege(PrivilegeType.InValid, ref inputPrivilegeName);
                inputPrivilegeName.Length = (ushort)((2 * inputPrivilegeName.Buffer.Length) + 1);
                inputPrivilegeName.MaximumLength = (ushort)(inputPrivilegeName.Length + 2);
            }
            else if (stPolicyInformation.PHandle != handleInput)
            {
                utilities.nameOfPrivilege(PrivilegeType.Valid, ref inputPrivilegeName);
                utilities.inValidHandle();
            }
            else if ((uintAccessMask & ACCESS_MASK.POLICY_LOOKUP_NAMES) != ACCESS_MASK.POLICY_LOOKUP_NAMES)
            {
                utilities.nameOfPrivilege(PrivilegeType.Valid, ref inputPrivilegeName);
            }
            else if (!htPrivilegeNameNLUID.ContainsValue(privilegeName))
            {
                utilities.nameOfPrivilege(PrivilegeType.NoSuchPrivilege, ref inputPrivilegeName);
            }
            else
            {
                utilities.nameOfPrivilege(PrivilegeType.Valid, ref inputPrivilegeName);
            }

            uintMethodStatus = lsadClientStack.LsarLookupPrivilegeValue(
                PolicyHandle.Value, 
                inputPrivilegeName, 
                out outputPrivilegeLUID);

            luid = (outputPrivilegeLUID.Value.HighPart == 0 & outputPrivilegeLUID.Value.LowPart == 0) 
                ? PrivilegeLUID.Invalid : PrivilegeLUID.Valid;

            if (name == ValidString.Invalid)
            {
                #region MS-LSAD_R807

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidParameter, 
                    (uint)uintMethodStatus, 
                    "MS-LSAD",
                    807,
                    @"In LsarLookupPrivilegeValue method server MUST  return STATUS_INVALID_PARAMETER 
                    if One or more of the supplied parameters was invalid.");

                #endregion
            }
            else if (stPolicyInformation.PHandle != handleInput)
            {
                #region MS-LSAD_R804

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidHandle, 
                    (uint)uintMethodStatus, 
                    "MS-LSAD", 
                    804,
                    @"In LsarLookupPrivilegeValue method,If PolicyHandle is not a valid context handle 
                    to the policy object, the server MUST return STATUS_INVALID_HANDLE.");

                #endregion
                lsadClientStack.LsarDeleteObject(ref PolicyHandle);
                PolicyHandle = utilities.tempPolicyHandle;
            }
            else if ((uintAccessMask & ACCESS_MASK.POLICY_LOOKUP_NAMES) != ACCESS_MASK.POLICY_LOOKUP_NAMES)
            {
                #region MS-LSAD_R805

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.AccessDenied,
                    (uint)uintMethodStatus, 
                    "MS-LSAD", 
                    805,
                    @"In PolicyHandle parameter of LsarLookupPrivilegeValue method, server MUST verify 
                    that the caller has POLICY_LOOKUP_NAMES access and return STATUS_ACCESS_DENIED otherwise");

                #endregion
            }
            else if (!htPrivilegeNameNLUID.ContainsValue(privilegeName))
            {
                #region MS-LSAD_R806

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.NoSuchPrivilege,
                    (uint)uintMethodStatus,
                    "MS-LSAD", 
                    806,
                    @"In LsarLookupPrivilegeValue method , If the value in the Name argument is not 
                    recognized by the server, the server MUST fail the request with STATUS_NO_SUCH_PRIVILEGE");

                #endregion
            }
            else
            {
                htPrivilegeNameNLUID.Clear();

                #region MS-LSAD_R808

                if ((uint)ErrorStatus.Success == (uint)uintMethodStatus)
                {
                    Site.CaptureRequirement(
                        "MS-LSAD", 
                        808,
                        @"In LsarLookupPrivilegeValue method server MUST return STATUS_SUCCESS 
                        if  request is successfully completed.");
                }

                #endregion
            }

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion LsarLookupPrivilegeValue

        #region LsarLookupPrivilegeName

        /// <summary>
        ///  The LookupPrivilegeName method is invoked to map the luid of a privilege 
        ///  into a string name by which it is known on the server.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="luid">It is for validating the privilege luid passed in </param>
        /// <param name="privilegeLuid">Contains privilege luid </param>
        /// <param name="privilegeName">Out param contains valid or invalid 
        /// privilegeName of the passed in privilege luid </param>
        /// <returns>Returns Success if the method is successful
        ///          Returns AccessDenied if the caller does not have the permissions to 
        ///          perform this operation
        ///          Returns InvalidHandle if the passed in is a valid object handle
        ///          Returns InvalidParameter if one or more of the supplied parameters was invalid
        /// Returns NoSuchPrivilege if the privilege luid is not recognized by the server</returns>
        public ErrorStatus LookupPrivilegeName(
            int handleInput,
            PrivilegeLUID luid,
            string privilegeLuid,
            out ValidString privilegeName)
        {
            _RPC_UNICODE_STRING? outputPrivilegeName = new _RPC_UNICODE_STRING();
            _RPC_UNICODE_STRING outputComparePrivilegeName = new _RPC_UNICODE_STRING();
            _LUID[] inputPrivilegeLUID = new _LUID[1];
            Hashtable htPrivilegeNameNLUID = new Hashtable();
            string nameOfPrivilege = string.Empty;
            char[] charNameOfPrivilege = new char[25];
            bool isEqual = false;

            nameOfPrivilege = PrivilegeName;
            charNameOfPrivilege = nameOfPrivilege.ToCharArray();
            outputComparePrivilegeName.Buffer = new ushort[nameOfPrivilege.Length];
            Array.Copy(charNameOfPrivilege, outputComparePrivilegeName.Buffer, charNameOfPrivilege.Length);
            outputComparePrivilegeName.Length = (ushort)(2 * outputComparePrivilegeName.Buffer.Length);
            outputComparePrivilegeName.MaximumLength = (ushort)(outputComparePrivilegeName.Length + 2);

            utilities.privilegeInformation(ref htPrivilegeNameNLUID);

            if (luid == PrivilegeLUID.Invalid)
            {
                utilities.valuesOfLUID(PrivilegeLUID.Invalid, ref inputPrivilegeLUID);
                inputPrivilegeLUID[0].LowPart = 56;
                inputPrivilegeLUID[0].HighPart = 0;
            }
            else if (stPolicyInformation.PHandle != handleInput)
            {
                utilities.valuesOfLUID(PrivilegeLUID.Valid, ref inputPrivilegeLUID);
                utilities.inValidHandle();
            }
            else if ((uintAccessMask & ACCESS_MASK.POLICY_LOOKUP_NAMES) != ACCESS_MASK.POLICY_LOOKUP_NAMES)
            {
                utilities.valuesOfLUID(PrivilegeLUID.Valid, ref inputPrivilegeLUID);
            }
            else if (!htPrivilegeNameNLUID.ContainsKey(privilegeLuid))
            {
                utilities.valuesOfLUID(PrivilegeLUID.NotPresentLuid, ref inputPrivilegeLUID);
            }
            else
            {
                utilities.valuesOfLUID(PrivilegeLUID.Valid, ref inputPrivilegeLUID);
            }
                        
            uintMethodStatus = lsadClientStack.LsarLookupPrivilegeName(
                PolicyHandle.Value,
                inputPrivilegeLUID[0],
                out outputPrivilegeName);

            if (outputPrivilegeName != null)
            {
                isEqual = utilities.CheckTheRpcStrings(outputPrivilegeName.Value, outputComparePrivilegeName);

                if (isEqual)
                {
                    privilegeName = ValidString.Valid;
                }
                else
                {
                    privilegeName = ValidString.Invalid;
                }
            }
            else
            {
                privilegeName = (outputPrivilegeName == null) ? ValidString.Invalid : ValidString.Valid;
            }

            if (!(luid == PrivilegeLUID.Invalid))
            {
                if (stPolicyInformation.PHandle != handleInput)
                {
                    #region MS-LSAD_R809

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.InvalidHandle,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        809,
                        @"In LsarLookupPrivilegeName method ,If PolicyHandle is not a valid context handle to 
                        the policy object, the server MUST return STATUS_INVALID_HANDLE");

                    #endregion

                    lsadClientStack.LsarDeleteObject(ref PolicyHandle);
                    PolicyHandle = utilities.tempPolicyHandle;
                }
                else if ((uintAccessMask & ACCESS_MASK.POLICY_LOOKUP_NAMES) != ACCESS_MASK.POLICY_LOOKUP_NAMES)
                {
                    #region MS-LSAD_R810

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.AccessDenied,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        810,
                        @"In  PolicyHandle parameter of LsarLookupPrivilegeName method, server MUST verify 
                        that the caller has POLICY_LOOKUP_NAMES access and return STATUS_ACCESS_DENIED otherwise.");

                    #endregion
                }
                else if (!htPrivilegeNameNLUID.ContainsKey(privilegeLuid))
                {
                    #region MS-LSAD_R811

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.NoSuchPrivilege,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        811,
                        @"In LsarLookupPrivilegeName method,If the LUID in the Value argument is not recognized 
                        by the server, the server MUST fail the request with STATUS_NO_SUCH_PRIVILEGE");

                    #endregion
                }
                else
                {
                    htPrivilegeNameNLUID.Clear();

                    #region MS-LSAD_R813

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.Success,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        813,
                        @"In LsarLookupPrivilegeName method server MUST  return STATUS_SUCCESS 
                        if request is successfully completed.");

                    #endregion
                }
            }

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion LsarLookupPrivilegeName

        #region LsarLookupPrivilegeDisplayName

        /// <summary>
        ///  The LookupPrivilegeDisplayName method is invoked to map the name 
        ///  of a privilege into a display text string in the caller's language.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="name">It is for validating the privilege name passed in </param>
        /// <param name="privilegeName">Contains privilege name </param>
        /// <param name="displayName">Out param contains valid or invalid 
        /// display text of the passed in privilegename </param>
        /// <returns>Returns Success if the method is successful
        ///          Returns AccessDenied if the caller does not have the permissions to 
        ///          perform this operation
        ///          Returns InvalidHandle if the passed in is a valid object handle
        ///          Returns InvalidParameter if one or more of the supplied parameters was invalid
        /// Returns NoSuchPrivilege if the privilege luid is not recognized by the server</returns>
        public ErrorStatus LookupPrivilegeDisplayName(
            int handleInput,
            ValidString name,
            string privilegeName,
            out ValidString displayName)
        {
            _RPC_UNICODE_STRING? inputPrivilegeName = new _RPC_UNICODE_STRING();
            _RPC_UNICODE_STRING inputPrivilegeName1 = new _RPC_UNICODE_STRING();
            _RPC_UNICODE_STRING? outputPrivilegeName = new _RPC_UNICODE_STRING();
            Hashtable htPrivilegeNameNLUID = new Hashtable();

            short shortClientLanguage = 0, shortClientSystemDefaultLanguage = 0;
            ushort? ushortLanguageReturned = 0;
            utilities.privilegeInformation(ref htPrivilegeNameNLUID);

            inputPrivilegeName1 = (_RPC_UNICODE_STRING)inputPrivilegeName;

            if (name == ValidString.Invalid)
            {                
                shortClientLanguage = 9;
                utilities.nameOfPrivilege(PrivilegeType.InValid, ref inputPrivilegeName1);
                inputPrivilegeName1.Length = (ushort)((2 * inputPrivilegeName1.Buffer.Length) + 1);
                inputPrivilegeName1.MaximumLength = (ushort)(inputPrivilegeName1.Length + 2);
            }
            else if (stPolicyInformation.PHandle != handleInput)
            {
                shortClientLanguage = 9;
                utilities.nameOfPrivilege(PrivilegeType.Valid, ref inputPrivilegeName1);
                utilities.inValidHandle();
            }
            else if (!htPrivilegeNameNLUID.ContainsValue(privilegeName))
            {
                shortClientLanguage = 9;
                utilities.nameOfPrivilege(PrivilegeType.NoSuchPrivilege, ref inputPrivilegeName1);
            }
            else if ((uintAccessMask & ACCESS_MASK.POLICY_LOOKUP_NAMES) != ACCESS_MASK.POLICY_LOOKUP_NAMES)
            {
                utilities.nameOfPrivilege(PrivilegeType.Valid, ref inputPrivilegeName1);
                shortClientLanguage = 9;
            }
            else
            {
                shortClientLanguage = 9;
                utilities.nameOfPrivilege(PrivilegeType.Valid, ref inputPrivilegeName1);
            }

            inputPrivilegeName = inputPrivilegeName1;

            uintMethodStatus = lsadClientStack.LsarLookupPrivilegeDisplayName(
                PolicyHandle.Value,
                inputPrivilegeName,
                shortClientLanguage,
                shortClientSystemDefaultLanguage,
                out outputPrivilegeName,
                out ushortLanguageReturned);

            displayName = (outputPrivilegeName == null) ? ValidString.Invalid : ValidString.Valid;

            if (name == ValidString.Invalid)
            {
                #region MS-LSAD_R823

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidParameter,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    823,
                    @"In LsarLookupPrivilegeDisplayName method  server MUST return STATUS_INVALID_PARAMETER 
                    if One or more of the supplied parameters was invalid.");

                #endregion
            }
            else if (stPolicyInformation.PHandle != handleInput)
            {
                lsadClientStack.LsarDeleteObject(ref PolicyHandle);
                PolicyHandle = utilities.tempPolicyHandle;

                #region MS-LSAD_R814

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidHandle,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    814,
                    @"In LsarLookupPrivilegeDisplayName method,If PolicyHandle is not a valid 
                    context handle to the policy object, the server MUST return STATUS_INVALID_HANDLE.");

                #endregion
            }
            else if ((uintAccessMask & ACCESS_MASK.POLICY_LOOKUP_NAMES) != ACCESS_MASK.POLICY_LOOKUP_NAMES)
            {
                #region MS-LSAD_R815

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.AccessDenied,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    815,
                    @"In PolicyHandle parameter of LsarLookupPrivilegeDisplayName method ,server MUST verify that the 
                    caller has POLICY_LOOKUP_NAMES access and return STATUS_ACCESS_DENIED otherwise");

                #endregion
            }
            else if (!htPrivilegeNameNLUID.ContainsValue(privilegeName))
            {
                #region MS-LSAD_R817

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.NoSuchPrivilege,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    817,
                    @"In LsarLookupPrivilegeDisplayName method  , If the entry cannot be located, 
                    the server MUST return STATUS_NO_SUCH_PRIVILEGE.");

                #endregion
            }
            else
            {
                htPrivilegeNameNLUID.Clear();

                #region MS-LSAD_R822

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    822,
                    @"In LsarLookupPrivilegeDisplayName method  server MUST return STATUS_SUCCESS 
                    if  request is successfully completed.");

                #endregion

                #region MS-LSAD_R816

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    816,
                    @"In LsarLookupPrivilegeDisplayName method  server MUST locate the entry with the same 
                    name in the data store (RPC_UNICODE_STRING).");

                #endregion

                #region MS-LSAD_R521

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    821,
                    @"In LsarLookupPrivilegeDisplayName method , If neither ClientLanguage nor 
                    ClientSystemDefaultLanguage can be found, the server MUST return the description 
                    in the server's own language.");

                #endregion
            }

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion LsarLookupPrivilegeDisplayName

        #region LsarEnumeratePrivileges

        /// <summary>
        /// The EnumeratePrivilegesRequest method is invoked to enumerate the privilege request.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="enumerationContext"> A pointer to a context value that is used to resume
        /// enumeration, if necessary.</param>
        public void EnumeratePrivilegesRequest(int handleInput, int enumerationContext)
        {
            _LSAPR_PRIVILEGE_ENUM_BUFFER? EnumerationBuf = new _LSAPR_PRIVILEGE_ENUM_BUFFER();
            IntPtr? objInvalidHandle = IntPtr.Zero;
            objPolicyHandle = closePolicyHandle;
            uint? enumContext = 0;
            uint preferredMaximumLength = 10;
            uint noOfPrivileges = 0;
            IntPtr? tempPolicyHandle = IntPtr.Zero;
            uint? enumerateContext = (uint)enumerationContext;

            Invalidhandlle = false;

            if (stPolicyInformation.PHandle != handleInput)
            {
                objInvalidHandle = utilities.AccountObjInvalidHandle();
                objPolicyHandle = objInvalidHandle;
            }

            #region Getting No of Privileges on Server

            uintMethodStatus = lsadClientStack.LsarOpenPolicy(
                utilities.ConversionfromStringtoushortArray("name"),
                objectAttributes,
                ACCESS_MASK.MAXIMUM_ALLOWED,
                out tempPolicyHandle);

            uintMethodStatus = lsadClientStack.LsarEnumeratePrivileges(
                tempPolicyHandle.Value,
                ref enumContext,
                out EnumerationBuf,
                preferredMaximumLength);

            uintMethodStatus = lsadClientStack.LsarClose(ref tempPolicyHandle);
            noOfPrivileges = EnumerationBuf.Value.Entries;

            #endregion

            uintMethodStatus = lsadClientStack.LsarEnumeratePrivileges(
                objPolicyHandle.Value,
                ref enumerateContext,
                out EnumerationBuf,
                preferredMaximumLength);

            #region DeleteInvalidHandle

            if (objInvalidHandle != IntPtr.Zero)
            {
                lsadClientStack.LsarDeleteObject(ref objInvalidHandle);
            }

            #endregion DeleteInvalidHandle

            if (stPolicyInformation.PHandle != handleInput)
            {
                EnumeratePrivileges(handleInput, enumerateResponse.EnumerateNone);

                #region MS-LSAD_R792

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidHandle,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    792,
                    @"In LsarEnumeratePrivileges method,If PolicyHandle  is not a valid context handle 
                    to the policy object, the server MUST return STATUS_INVALID_HANDLE.");

                #endregion
            }
            else if ((uintAccessMask & ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION) != 
                ACCESS_MASK.POLICY_VIEW_LOCAL_INFORMATION)
            {
                EnumeratePrivileges(handleInput, enumerateResponse.EnumerateNone);

                #region MS-LSAD_R793

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.AccessDenied,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    793,
                    @"In PolicyHandle parameter of LsarEnumeratePrivileges method,when the policy handle 
                    lacks the required POLICY_VIEW_LOCAL_INFORMATION flag, the server MUST return 
                    STATUS_ACCESS_DENIED error code.");

                #endregion
            }
            else if (enumerationContext >= noOfPrivileges)
            {
                EnumeratePrivileges(handleInput, enumerateResponse.EnumerateNone);

                #region MS-LSAD_R798

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.NoMoreEntries,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    798,
                    @"In EnumerationContext parameter of LsarEnumeratePrivileges method , If the EnumerationContext 
                    supplied by the caller is set to such a number that would not allow further enumeration of known
                    privileges, the server MUST return STATUS_NO_MORE_ENTRIES");

                #endregion
            }
            else if (enumerationContext < noOfPrivileges)
            {
                EnumeratePrivileges(handleInput, enumerateResponse.EnumerateAll);

                #region MS-LSAD_R801

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    801,
                    @"In LsarEnumeratePrivileges method, server MUST  retrurn STATUS_SUCCESS 
                    if request is successfully completed.");

                #endregion

                #region MS-LSAD_R45

                if (EnumerationBuf.Value.Entries != 0)
                {
                    Site.CaptureRequirementIfIsNotNull(
                        EnumerationBuf.Value.Privileges,
                        "MS-LSAD",
                        45,
                        @"If the ""Entries"" field of the LSAPR_PRIVILEGE_ENUM_BUFFER structure in LSAD protocol 
                        has a value other than 0, the ""Privileges"" field MUST NOT be NULL.");
                }

                #endregion
            }
        }

        #endregion LsarEnumeratePrivileges

        #endregion PrivilegeObjects
    }
}