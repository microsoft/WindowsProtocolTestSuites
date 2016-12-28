// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad
{
    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk;
    using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa;
    using Microsoft.Protocols.TestTools.StackSdk.Dtyp;

    /// <summary>
    /// Implement methods of interface ILsadRpcAdapter.
    /// </summary>
    public partial class LsadManagedAdapter
    {
        #region Common_Objects

        #region LsarQuerySecurityObject

        /// <summary>
        ///  The QuerySecurityObject method is invoked to query security information that is assigned
        ///  to a database object. It returns the security descriptor of the object.
        /// </summary>
        /// <param name="handleInput">Contains any object handle </param>
        /// <param name="securityInfo">Contains security descriptor information </param>
        /// <param name="securityDescriptor">Out param contains valid or invalid 
        /// security descriptor </param>
        /// <returns>Returns Success if the method is successful
        /// Returns AccessDenied if the caller does not have the permissions to perform this operation
        /// Returns InvalidHandle if the passed in is a valid object handle
        /// Returns NotSupported if the request is not supported</returns>
        public ErrorStatus QuerySecurityObject(
            int handleInput,
            SecurityInfo securityInfo,
            out SecurityDescriptor securityDescriptor)
        {
            System.IntPtr? objInvalidAccountHandle = IntPtr.Zero;
            uintSecurityInfo = Convert.ToByte(LsadUtilities.QuerySecurityInfo);
            objAccountSid[0].Revision = 0x01;
            _LSAPR_SR_SECURITY_DESCRIPTOR? secDescriptor = new _LSAPR_SR_SECURITY_DESCRIPTOR();

            ////Passing Trusteddomain object for STATUS_NOT_SUPPORTED status.
            if ((htAccHandle.Count == 0) && (checkTrustHandle == true))
            {
                objAccountHandle = validTrustHandle;
            }
            else if ((htAccHandle.Count == 0) && (checkTrustHandle == false))
            {
                #region Passing InvalidHandle

                uintDesrAccess = ACCESS_MASK.MAXIMUM_ALLOWED;
                uintMethodStatus = lsadClientStack.LsarOpenAccount(
                    objPolicyHandle.Value,
                    objAccountSid[0],
                    uintDesrAccess,
                    out objInvalidAccountHandle);

                if (uintMethodStatus != 0)
                {
                    uintMethodStatus = lsadClientStack.LsarCreateAccount(
                        objPolicyHandle.Value,
                        objAccountSid[0],
                        uintDesrAccess,
                        out objAccountHandle);
                }

                uintMethodStatus = lsadClientStack.LsarOpenAccount(
                    objPolicyHandle.Value,
                    objAccountSid[0],
                    uintDesrAccess,
                    out objInvalidAccountHandle);

                uintMethodStatus = lsadClientStack.LsarDeleteObject(ref objAccountHandle);

                #endregion Passing InvalidHandle

                objAccountHandle = objInvalidAccountHandle;
            }

            uintMethodStatus = lsadClientStack.LsarQuerySecurityObject(
                objAccountHandle.Value,
                (SECURITY_INFORMATION)uintSecurityInfo,
                out secDescriptor);

            securityDescriptor = (null == secDescriptor) ? SecurityDescriptor.Invalid : SecurityDescriptor.Valid;

            if ((htAccHandle.Count == 0) && (checkTrustHandle == true))
            {
                #region MS-LSAD_R829

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.NotSupported,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    829,
                    @"It is valid for the server to not support LsarQuerySecurityObject method for all 
                    object types. If an object does not support the LsarQuerySecurityObject method, the 
                    server MUST return STATUS_NOT_SUPPORTED.");

                #endregion

                #region MS-LSAD_R830

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.NotSupported,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    830,
                    @"The server will not return the security descriptor of objects that it stores in Active Directory. 
                    It will return the security descriptor of objects in its local policy only. The objects stored in 
                    Active Directory include Global Secrets and trusted domain objects in Windows 2000 and Windows 
                    Server 2003 R2. For objects that fall into this category, the server will return the 
                    STATUS_NOT_SUPPORTED status code on receipt of LsarQuerySecurityObject method.");

                #endregion
            }
            else if ((htAccHandle.Count == 0) && (checkTrustHandle == false))
            {
                #region MS-LSAD_R826

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidHandle,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    826,
                    @"If the handle of Opnum LsarQuerySecurityObject is not a valid context handle 
                    to an object, the server MUST return STATUS_INVALID_HANDLE. ");

                #endregion
            }
            else if ((securityInfo != SecurityInfo.SACLSecurityInformation)
                          && ((uintOpenAccAccess & ACCESS_MASK.READ_CONTROL) != ACCESS_MASK.READ_CONTROL))
            {
                #region MS-LSAD_R825

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.AccessDenied,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    825,
                    @"SecurityDescriptor of LsarQuerySecurityObject  MUST return  STATUS_ACCESS_DENIED 
                    if the caller does not have the permissions to perform the operation.");

                #endregion
            }
            else
            {
                #region MS-LSAD_R824

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    824,
                    @"SecurityDescriptor of LsarQuerySecurityObject return Values   that an implementation 
                    MUST return  STATUS_SUCCESS if the request was successfully completed.");

                #endregion
            }

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion LsarQuerySecurityObject

        #region LsarSetSecurityObject

        /// <summary>
        ///  The SetSecurityObject method is invoked to set a security descriptor on an object.
        /// </summary>
        /// <param name="handleInput">Contains any object handle </param>
        /// <param name="securityInfo">Contains security descriptor information type</param>
        /// <param name="securityDescriptor">Contains security descriptor to be set</param>
        /// <returns>Returns Success if the method is successful
        /// Returns AccessDenied if the caller does not have the permissions to perform this operation
        /// Returns InvalidHandle if the passed in is a valid object handle
        /// Returns NotSupported if the request is not supported for this object
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns InvalidSecurityDescr if the supplied security descriptor is invalid</returns>
        public ErrorStatus SetSecurityObject(
            int handleInput,
            SecurityInfo securityInfo,
            SecurityDescriptor securityDescriptor)
        {
            System.IntPtr? objInvalidAccountHandle = IntPtr.Zero;
            uint writeDacl = 0x000F0FFF;
            uintMethodStatus = lsadClientStack.LsarOpenPolicy(
                serverName,
                objectAttributes,
                uintAccess,
                out objPolicyHandle);

            _LSAPR_SR_SECURITY_DESCRIPTOR secDescriptor = new _LSAPR_SR_SECURITY_DESCRIPTOR();

            secDescriptor.SecurityDescriptor = utilities.SecurityDescriptor();
            secDescriptor.Length = (uint)secDescriptor.SecurityDescriptor.Length;

            ////Passing Trusteddomain object for STATUS_NOT_SUPPORTED status.
            if ((htAccHandle.Count == 0) && (checkTrustHandle == true))
            {
                objAccountHandle = validTrustHandle;
            }
            else
            {
                if ((htAccHandle.Count == 0) && (checkTrustHandle == false))
                {
                    #region Passing InvalidHandle

                    uintDesrAccess = ACCESS_MASK.MAXIMUM_ALLOWED;
                    uintMethodStatus = lsadClientStack.LsarOpenAccount(
                        objPolicyHandle.Value,
                        objAccountSid[0],
                        uintDesrAccess,
                        out objInvalidAccountHandle);

                    if (uintMethodStatus != 0)
                    {
                        uintMethodStatus = lsadClientStack.LsarCreateAccount(
                            objPolicyHandle.Value,
                            objAccountSid[0],
                            uintDesrAccess,
                            out objInvalidAccountHandle);
                    }

                    uintMethodStatus = lsadClientStack.LsarOpenAccount(
                        objPolicyHandle.Value,
                        objAccountSid[0],
                        uintDesrAccess,
                        out objAccountHandle);

                    uintMethodStatus = lsadClientStack.LsarDeleteObject(ref objInvalidAccountHandle);

                    #endregion Passing InvalidHandle
                }

                if (securityInfo == SecurityInfo.OwnerSecurityInformation)
                {
                    if ((uintOpenAccAccess & ACCESS_MASK.WRITE_OWNER) == ACCESS_MASK.WRITE_OWNER)
                    {
                        uintSecurityInfo = 0x00080000;
                    }
                    else
                    {
                        uintSecurityInfo = 0x99999999;
                    }
                }
                else if (securityInfo == SecurityInfo.DACLSecurityInformation)
                {
                    if (((uint)uintOpenAccAccess & writeDacl) == writeDacl)
                    {
                        uintSecurityInfo = 0x00080000;
                    }
                    else
                    {
                        uintSecurityInfo = 0x99999999;
                    }
                }

                if (securityDescriptor == SecurityDescriptor.Null)
                {
                    secDescriptor.SecurityDescriptor = null;
                    secDescriptor.Length = (uint)0;
                }
                else if (securityDescriptor == SecurityDescriptor.Invalid)
                {
                    secDescriptor.SecurityDescriptor[0] = 0x00000000;
                    secDescriptor.Length = (uint)secDescriptor.SecurityDescriptor.Length;
                }
            }

            uintMethodStatus = lsadClientStack.LsarSetSecurityObject(
                objAccountHandle.Value,
                (SECURITY_INFORMATION)uintSecurityInfo,
                secDescriptor);

            if ((htAccHandle.Count == 0) && (checkTrustHandle == true))
            {
                #region MS-LSAD_R833

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.NotSupported,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    833,
                    @"LsarSetSecurityObject  MUST return  STATUS_NOT_SUPPORTED 
                    if the operation is not supported for this object.");

                #endregion

                //Here we are passing TrustedDomainHandle(checkTrustHandle == true) for LsarSetSecurityObject
                #region MS-LSAD_R838

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.NotSupported,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    838,
                    @"The Windows server will not return the security descriptor of objects it stores in Active 
                    Directory. It will return the security descriptor of objects in its local policy only. 
                    The objects stored in Active Directory include Global Secrets and trusted domain objects. 
                    For objects that fall into this category, a Windows server returns the STATUS_NOT_SUPPORTED status
                    code when it receives a LsarSetSecurityObject request.");

                #endregion
            }
            else if (securityDescriptor == SecurityDescriptor.Null)
            {
                #region MS-LSAD_R837

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidParameter,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    837,
                    @"The server MUST return STATUS_InvalidParameter when one of the parameter of LsarSetSecurityObject 
                    supplied is inValid(for instance security descriptor)");

                #endregion
            }
            else if (securityDescriptor == SecurityDescriptor.Invalid)
            {
                #region MS-LSAD_R836

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidSecurityDescr,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    836,
                    @"If the security descriptor in LsarSetSecurityObject is invalid, the server 
                    MUST return the STATUS_INVALID_SECURITY_DESCR status code.");

                #endregion
            }
            else if ((htAccHandle.Count == 0) && (checkTrustHandle == false))
            {
                #region MS-LSAD_R834

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidHandle,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    834,
                    @"If the ObjectHandle in LsarSetSecurityObject is not a valid context handle to 
                    an object, the server MUST return STATUS_INVALID_HANDLE. ");

                #endregion
            }
            else if ((securityInfo == SecurityInfo.OwnerSecurityInformation)
                          && ((uintOpenAccAccess & ACCESS_MASK.WRITE_OWNER) != ACCESS_MASK.WRITE_OWNER))
            {
                #region MS-LSAD_R832

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.AccessDenied,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    832,
                    @"LsarSetSecurityObject  MUST return  STATUS_ACCESS_DENIED if the caller does not 
                    have the permissions to perform this operation");

                #endregion
            }
            else if ((securityInfo == SecurityInfo.DACLSecurityInformation)
                          && (((uint)uintOpenAccAccess & writeDacl) != writeDacl))
            {
                #region MS-LSAD_R832

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.AccessDenied,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    832,
                    @"LsarSetSecurityObject  MUST return  STATUS_ACCESS_DENIED if the caller does not 
                    have the permissions to perform this operation");

                #endregion
            }
            else
            {
                #region MS-LSAD_R51

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                    secDescriptor.Length,
                    (uint)secDescriptor.SecurityDescriptor.Length,
                    "MS-LSAD",
                    51,
                    @"The SecurityDescriptor field of the LSAPR_SR_SECURITY_DESCRIPTOR structure of the LSAD protocol
                    MUST contain the number of bytes that are specified in the Length field.");

                #endregion

                #region MS-LSAD_R52

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                    secDescriptor.Length,
                    (uint)secDescriptor.SecurityDescriptor.Length,
                    "MS-LSAD",
                    52,
                    @"If the Length field of the LSAPR_SR_SECURITY_DESCRIPTOR structure of the LSAD protocol 
                    has a value other than 0, the SecurityDescriptor field MUST NOT be NULL.");

                #endregion

                #region MS-LSAD_R831

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    831,
                    @"LsarSetSecurityObject returns  MUST return  STATUS_SUCCESS 
                    if the request was successfully completed.");

                #endregion
            }

            invalidHandleCheck = false;

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion LsarSetSecurityObject

        #region LsarDeleteObject

        /// <summary>
        ///  The DeleteObject method is invoked to delete an open account object, 
        ///  secret object, or trusted domain object.
        /// </summary>
        /// <param name="handleInput">Contains any object handle </param>
        /// <param name="usedObject">Type of object which is be deleted</param>
        /// <param name="handleOutput">Out param which contains null handle value</param>
        /// <returns>Returns Success if the method is successful
        ///          Returns AccessDenied if the caller does not have the permissions to 
        ///          perform this operation
        ///          Returns InvalidHandle if the passed in is a valid object handle
        ///          Returns InvalidParameter if the parameters passed to the method are not valid</returns>
        /// Disable warning CA1502 because it will affect the implementation of Adapter and Model codes 
        /// if do any changes about maintainability.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity",
            Justification = "Disable warning CA1502 because it will affect the implementation of Adapter and Model")]
        public ErrorStatus DeleteObject(
            int handleInput,
            ObjectEnum usedObject,
            out Handle handleOutput)
        {
            _LSAPR_OBJECT_ATTRIBUTES[] objectAttributesForDelete = new _LSAPR_OBJECT_ATTRIBUTES[1];
            _RPC_UNICODE_STRING[] SecretName = new _RPC_UNICODE_STRING[1];
            IntPtr? deleteHandle = IntPtr.Zero;
            IntPtr? PolicyHandle2 = IntPtr.Zero;
            IntPtr? deletedHandle = IntPtr.Zero;
            Hashtable htCheckHandle = new Hashtable();

            if (TurstDomainFlag && usedObject == ObjectEnum.TrustDomainObject)
            {
                ////Check if Interdomain Trust Account exist and is valid
                bool isInterdomainTrustAccountExist = !CheckInterdomainTrustAccount();

                uintMethodStatus = lsadClientStack.LsarDeleteObject(ref validTrustHandle);
                handleOutput = (IntPtr.Zero == validTrustHandle) ? Handle.Null : Handle.Invalid;
                TurstDomainFlag = false;

                ////Check if interdomain trust account is deleted along with the trusted domain
                if (isInterdomainTrustAccountExist)
                {
                    #region MS-LSAD_R1062

                    // Add the comment information including the location information.
                    TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, "Verify MS-LSAD_R1062");

                    // Verify requirement 1062
                    TestClassBase.BaseTestSite.CaptureRequirement(
                        "MS-LSAD",
                        1062,
                        @"If the object being deleted is a trusted domain, then the server MUST also check whether
                          an interdomain trust account with name ""<Trusted Domain NetBIOS Name>$"" exists.");

                    #endregion

                    //// The server must delete that account along with the trusted domain, so check the accounts exists 
                    //// or not We expect the trusted domain is not existed
                    #region MS-LSAD_R1063

                    // Add the comment information including the location information.
                    TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, "Verify MS-LSAD_R1063");

                    // Verify requirement 1063
                    TestClassBase.BaseTestSite.CaptureRequirement(
                        "MS-LSAD",
                        1063,
                        @"If an interdomain trust account with name ""<Trusted Domain NetBIOS Name>$""] exists, 
                        the server MUST delete that account along with the trusted domain.");

                    #endregion
                }
            }
            else
            {
                if (secretFlag && usedObject == ObjectEnum.SecretObject)
                {
                    objectAttributesForDelete[0].RootDirectory = null;
                    IntPtr? tempSecretHandle = IntPtr.Zero;
                    IntPtr? tempSecretHandle2 = IntPtr.Zero;
                    IntPtr? objDeletHandle = IntPtr.Zero;

                    if (stSecretInformation.UIntSecretHandleAccessCount != handleInput)
                    {
                        utilities.nameOfSecretObject(ResOfNameChecked.Valid, ref SecretName);
                        lsadClientStack.LsarOpenPolicy(
                            utilities.ConversionfromStringtoushortArray("name"),
                            objectAttributesForDelete[0],
                            ACCESS_MASK.POLICY_CREATE_SECRET,
                            out PolicyHandle2);

                        lsadClientStack.LsarOpenSecret(
                            PolicyHandle2.Value,
                            SecretName[0],
                            ACCESS_MASK.DELETE,
                            out tempSecretHandle2);

                        if (tempSecretHandle2 != IntPtr.Zero)
                        {
                            lsadClientStack.LsarDeleteObject(ref tempSecretHandle2);
                        }

                        lsadClientStack.LsarCreateSecret(
                            PolicyHandle2.Value,
                            SecretName[0],
                            ACCESS_MASK.DELETE,
                            out tempSecretHandle);

                        lsadClientStack.LsarOpenSecret(
                            PolicyHandle2.Value,
                            SecretName[0],
                            ACCESS_MASK.DELETE,
                            out tempSecretHandle2);

                        lsadClientStack.LsarDeleteObject(ref tempSecretHandle);
                        deletedHandle = tempSecretHandle;
                        objDeletHandle = tempSecretHandle2;
                    }
                    else if ((uintAccessMask & ACCESS_MASK.DELETE) != ACCESS_MASK.DELETE)
                    {
                        objDeletHandle = objSecretHandle;
                    }
                    else if (stPolicyInformation.PHandle == handleInput)
                    {
                        objDeletHandle = PolicyHandle;
                    }
                    else
                    {
                        objDeletHandle = objSecretHandle;
                    }

                    uintMethodStatus = lsadClientStack.LsarDeleteObject(ref objDeletHandle);
                    deletedHandle = objDeletHandle;
                    handleOutput = (0 == uintMethodStatus) ? Handle.Null : Handle.Invalid;
                }
                else
                {
                    uintDesrAccess = ACCESS_MASK.MAXIMUM_ALLOWED;
                    objAccountSid[0].Revision = 0x01;
                    uintMethodStatus = lsadClientStack.LsarOpenPolicy(
                        serverName,
                        objectAttributesForDelete[0],
                        uintDesrAccess,
                        out objPolicyHandle);

                    if (stPolicyInformation.PHandle == handleInput)
                    {
                        uintMethodStatus = lsadClientStack.LsarOpenPolicy(
                            serverName,
                            objectAttributesForDelete[0],
                            uintDesrAccess,
                            out objPolicyHandle);

                        deleteHandle = objPolicyHandle;
                        uintMethodStatus = lsadClientStack.LsarDeleteObject(ref objPolicyHandle);
                        deletedHandle = objPolicyHandle;
                    }
                    else if ((stPolicyInformation.PHandle + 1 != handleInput)
                                   || ((htAccHandle.Count == 0) && (checkTrustHandle == false)))
                    {
                        #region Passing InvalidHandle

                        uintMethodStatus = lsadClientStack.LsarOpenAccount(
                            objPolicyHandle.Value,
                            objAccountSid[0],
                            uintDesrAccess,
                            out deleteHandle);

                        if (uintMethodStatus != 0)
                        {
                            uintMethodStatus = lsadClientStack.LsarCreateAccount(
                                objPolicyHandle.Value,
                                objAccountSid[0],
                                uintDesrAccess,
                                out deleteHandle);
                        }

                        uintMethodStatus = lsadClientStack.LsarOpenAccount(
                            objPolicyHandle.Value,
                            objAccountSid[0],
                            uintDesrAccess,
                            out objAccountHandle);

                        uintMethodStatus = lsadClientStack.LsarDeleteObject(ref deleteHandle);

                        #endregion Passing InvalidHandle

                        deleteHandle = objAccountHandle;
                        uintMethodStatus = lsadClientStack.LsarDeleteObject(ref objAccountHandle);
                        deletedHandle = objAccountHandle;
                    }
                    else
                    {
                        if (uintOpenAccAccess != 0)
                        {
                            objAccountSid[0].Revision = 0x01;
                            uintDesrAccess = ACCESS_MASK.MAXIMUM_ALLOWED;
                        }
                        else
                        {
                            uintDesrAccess = ACCESS_MASK.NONE;
                            htCheckHandle.Add("DeleteHandle", 0);
                        }

                        lsadClientStack.LsarOpenAccount(
                            objPolicyHandle.Value,
                            objAccountSid[0],
                            uintDesrAccess,
                            out objAccountHandle);

                        deleteHandle = validAccountHandle;

                        if (uintOpenAccAccess != 0)
                        {
                            uintMethodStatus = lsadClientStack.LsarDeleteObject(ref deleteHandle);
                        }
                        else
                        {
                            if ((checkTrustHandle == true) && (htAccHandle.Count == 0))
                            {
                                deleteHandle = validTrustHandle;
                                uintMethodStatus = lsadClientStack.LsarDeleteObject(ref deleteHandle);
                            }
                            else
                            {
                                uintMethodStatus = lsadClientStack.LsarDeleteObject(ref objAccountHandle);
                                deletedHandle = objAccountHandle;
                            }
                        }
                    }

                    handleOutput = (IntPtr.Zero == deleteHandle) ? Handle.Null : Handle.Invalid;
                }

                if (secretFlag && usedObject == ObjectEnum.SecretObject)
                {
                    if (stSecretInformation.UIntSecretHandleAccessCount != handleInput)
                    {
                        #region MS-LSAD_R844

                        TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.InvalidHandle,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            844,
                            @"The server MUST make all subsequent  requests to the deleted object fail with 
                            STATUS_INVALID_HANDLE, even if the requests come in through other open handles.");

                        #endregion
                    }
                    else if ((uintAccessMask & ACCESS_MASK.DELETE) != ACCESS_MASK.DELETE)
                    {
                        #region MS-LSAD_R843

                        TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.AccessDenied,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            843,
                            @"The handle in LsarDeleteObject MUST have been opened for DELETE access, 
                            and the server MUST fail the request with STATUS_ACCESS_DENIED otherwise.");

                        #endregion
                    }
                    else if (stPolicyInformation.PHandle == handleInput)
                    {
                        #region MS-LSAD_R842

                        TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.InvalidParameter,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            842,
                            @"Policy objects cannot be deleted. Attempts to delete policy objects 
                            MUST fail with STATUS_INVALID_PARAMETER.");

                        #endregion
                    }
                    else
                    {
                        #region MS-LSAD_R840

                        TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.Success,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            840,
                            @"LsarDeleteObject MUST return  STATUS_SUCCESS if the request was 
                                successfully completed.");

                        #endregion

                        #region MS-LSAD_R845

                        TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                            (((uint)uintMethodStatus == (uint)ErrorStatus.Success) && deletedHandle == IntPtr.Zero),
                            "MS-LSAD",
                            845,
                            @"The deleted ObjectHandle of LsarDeleteObject  MUST be automatically closed by 
                                the server; the caller need not close it.");

                        #endregion
                    }

                    if (objSecretHandle != IntPtr.Zero)
                    {
                        objectAttributesForDelete[0].RootDirectory = null;
                        utilities.nameOfSecretObject(ResOfNameChecked.Valid, ref SecretName);
                        lsadClientStack.LsarOpenPolicy2(
                            "name",
                            objectAttributesForDelete[0], 
                            ACCESS_MASK.DELETE | ACCESS_MASK.POLICY_CREATE_SECRET,
                            out PolicyHandle2);

                        lsadClientStack.LsarCreateSecret(
                            PolicyHandle2.Value,
                            SecretName[0],
                            ACCESS_MASK.DELETE,
                            out objSecretHandle);

                        lsadClientStack.LsarOpenSecret(
                            PolicyHandle2.Value,
                            SecretName[0],
                            ACCESS_MASK.DELETE,
                            out objSecretHandle);

                        lsadClientStack.LsarDeleteObject(ref objSecretHandle);
                    }

                    secretFlag = false;
                }
                else
                {
                    if (stPolicyInformation.PHandle == handleInput)
                    {
                        #region MS-LSAD_R842

                        TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.InvalidParameter,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            842,
                            @"Policy objects cannot be deleted. Attempts to delete policy objects 
                            MUST fail with STATUS_INVALID_PARAMETER.");

                        #endregion
                    }
                    else if ((stPolicyInformation.PHandle + 1 != handleInput)
                                   || ((htAccHandle.Count == 0) && (checkTrustHandle == false)))
                    {
                        #region MS-LSAD_R841

                        TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.InvalidHandle,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            841,
                            @"If the ObjectHandle of LsarDeleteObject  is not a valid context handle to an object, 
                            the server MUST return STATUS_INVALID_HANDLE.");

                        #endregion

                        #region MS-LSAD_R844

                        TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.InvalidHandle,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            844,
                            @"The server MUST make all subsequent  requests to the deleted object fail with 
                            STATUS_INVALID_HANDLE, even if the requests come in through other open handles.");

                        #endregion
                    }
                    else if (((uintAccess & ACCESS_MASK.DELETE) != ACCESS_MASK.DELETE)
                                    && ((uintOpenAccAccess & ACCESS_MASK.DELETE) != ACCESS_MASK.DELETE)
                                    && (htAccHandle.Count != 0))
                    {
                        #region MS-LSAD_R843

                        TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.AccessDenied,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            843,
                            @"The handle in LsarDeleteObject MUST have been opened for DELETE access, 
                            and the server MUST fail the request with STATUS_ACCESS_DENIED otherwise.");

                        #endregion
                    }
                    else
                    {
                        NtStatus checkDeleteHandle = lsadClientStack.LsarOpenAccount(
                            objPolicyHandle.Value,
                            objAccountSid[0],
                            uintAccessMask,
                            out objAccountHandle);

                        if (checkDeleteHandle == (NtStatus)0xC0000034)
                        {
                            #region MS-LSAD_R840

                            TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.Success,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                840,
                                @"LsarDeleteObject MUST return  STATUS_SUCCESS if the request was 
                                successfully completed.");

                            #endregion

                            #region MS-LSAD_R845

                            TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.Success,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                845,
                                @"The deleted ObjectHandle of LsarDeleteObject  MUST be automatically closed by 
                                the server; the caller need not close it.");

                            #endregion

                            #region MS-LSAD_R224

                            TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.Success,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                224,
                                @"Deleting an object to which the caller has an open handle (by calling 
                                LsarDeleteObject), if successful, MUST also close the handle.");

                            #endregion
                        }
                    }

                    if (htAccHandle.Count != 0)
                    {
                        utilities.DeleteExistAccount();
                        htAccHandle.Remove("AccHandle");
                    }

                    utilities.CreateExistAccount(uintOpenAccAccess);

                    if (htAddAccRight.Count != 0)
                    {
                        htAddAccRight.Remove("htAddAccRight");
                    }
                }
            }

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion LsarDeleteObject

        #region LsarClose

        /// <summary>
        ///  The Close method frees the resources held by a context handle 
        ///  that was opened earlier. After response, the context handle will no longer be usable
        ///  and any subsequent uses of this handle will fail.
        /// </summary>
        /// <param name="handleToBeClosed">Contains any object handle to be closed </param>
        /// <param name="handleAfterClose">Out param which contains null handle value</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidHandle if the passed in is a valid object handle</returns>
        public ErrorStatus Close(int handleToBeClosed, out Handle handleAfterClose)
        {
            IntPtr? closingHandle = IntPtr.Zero;

            if (closeCheck == true)
            {
                ////Creating one handle from LsarOpenPolicy before closing the PolicyHandle from LsarClose.
                uintDesrAccess = ACCESS_MASK.MAXIMUM_ALLOWED;
                uintMethodStatus = lsadClientStack.LsarOpenPolicy(
                    serverName,
                    objectAttributes,
                    uintDesrAccess,
                    out closingHandle);

                objAccountSid = utilities.SID(TypeOfSID.NewSID);
                uintMethodStatus = lsadClientStack.LsarCreateAccount(
                    closingHandle.Value,
                    objAccountSid[0],
                    uintDesrAccess,
                    out objAccountHandle);

                uintMethodStatus = lsadClientStack.LsarOpenAccount(
                    closingHandle.Value,
                    objAccountSid[0],
                    uintDesrAccess,
                    out objAccountHandle);
            }

            if (stPolicyInformation.PHandle != handleToBeClosed)
            {
                handleAfterClose = Handle.Invalid;
                return ErrorStatus.InvalidHandle;
            }
            else
            {
                uintMethodStatus = lsadClientStack.LsarClose(ref validPolicyHandle);
                handleAfterClose = (IntPtr.Zero == validPolicyHandle) ? Handle.Null : Handle.Invalid;

                #region MS-LSAD_R847

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                    0,
                    (uint)validPolicyHandle,
                    "MS-LSAD",
                    847,
                    @"ObjectHandle of LsarClose MUST return  STATUS_SUCCESS 
                    if the request was successfully completed.");

                #endregion
            }

            if (closeCheck == true)
            {
                ////Closing one handle from LsarClose MUST NOT affect any other handle on the server.
                ////ie. Handles obtained using a policy handle MUST continue to be Valid 
                ////after that policy handle is closed.
                uintMethodStatus = lsadClientStack.LsarDeleteObject(ref objAccountHandle);

                #region MS-LSAD_R849

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                    0, 
                    (uint)validPolicyHandle, 
                    "MS-LSAD",
                    849,
                    @"Closing one handle from LsarClose MUST NOT affect any other handle on the server that is, handles  
                    obtained using a policy handle MUST continue to be valid after that policy handle is closed.");
                
                #endregion

                #region MS-LSAD_R225

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success, 
                    (uint)uintMethodStatus, 
                    "MS-LSAD", 
                    225,
                    @"Closing one handle MUST NOT affect any other handle on the server; that is, handles obtained 
                    using a policy handle MUST continue to be valid after that policy handle is closed.");
                
                #endregion
            }

            closeCheck = false;
            return (ErrorStatus)uintMethodStatus;
        }

        #endregion LsarClose

        #endregion Common_Objects
    }
}