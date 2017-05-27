// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Policy;
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
        #region Secret&Privilege Variables      
        
        /// <summary>
        /// a variable used to indicate the name of the secret object
        /// </summary>
        private string secretNameOfSecretObject;

        /// <summary>
        /// A variable used to indicate that the secret object name is either local or global.
        /// </summary>
        private string localname = string.Empty;

        /// <summary>
        /// A bool variable used to indicate that the object is system object or local object.
        /// </summary>
        private bool strSystemOrLocal;

        /// <summary>
        /// A variable used to indicate the object that is being deleted is secret object
        /// this is true when the object is a secret object and false for all other objects.
        /// </summary>
        private bool secretFlag;
 
        /// <summary>
        /// This is a temporary variable used to store the secret handle created.
        /// </summary>
        private IntPtr? objQuerySecretHandle = IntPtr.Zero;

        /// <summary>
        /// A temporary access variable used to check the access for the secret handle created.
        /// </summary>
        private uint uintAccessToCreateSecretObject;

        /// <summary>
        /// structure used to store the secret name and the a count variable to increase 
        /// whenever a secret handle is created.
        /// </summary>       
        internal struct stSecretInformation
        {
            /// <summary>
            /// Variable used to store the count variable to increase whenever a secret handle is created.
            /// </summary> 
            internal static uint UIntSecretHandleAccessCount = 1;

            /// <summary>
            /// Variable used to store the secret name.
            /// </summary> 
            internal static string strNameOfSecretObject = string.Empty;
        };

        /// <summary>
        /// This variable is used to store the secret handle created.
        /// </summary>  
        internal static IntPtr? objSecretHandle = IntPtr.Zero;        

        /// <summary>
        /// This is the instance variable of the Helper class which contains the private methods.
        /// </summary>
        LsadUtilities utilities = new LsadUtilities();

        /// <summary>
        /// This variable is used to get the status of Directory Service.
        /// </summary>  
        private static ILsadSutControlAdapter ISUTControlAdapterInstance;

        #endregion Secret&Privilege Variables

        #region Secret_Objects

        #region LsarCreateSecret

        /// <summary>
        ///  The CreateSecret method is invoked to create a new secret object in the server's database.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="secretName">Contains the secret name of an secret object to be created</param>
        /// <param name="desiredAccess">Contains the access to be given to the secretHandle</param>
        /// <param name="secretHandle">Outparam which contains valid or invalid secret handle</param>
        /// <returns>Returns Success if the method is successful
        ///          Returns InvalidParameter if the parameters passed to the method are not valid
        ///          Returns AccessDenied if the caller does not have the permissions to 
        ///          perform this operation
        ///          Returns InvalidHandle if the passed in policy handle is not valid
        ///          Returns ObjectNameCollision if passed in account sid already exists
        ///          Returns NameTooLong if the length of specified secret name exceeds
        ///          the maximum set by the server</returns>
        /// Disable warning CA1502 and CA1505 because it will affect the implementation of Adapter and Model 
        /// codes if do any changes about maintainability.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity",
            Justification = "affect the implementation of Adapter and Model codes")]
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", 
            Justification = "affect the implementation of Adapter and Model codes")]
        public ErrorStatus CreateSecret(
            int handleInput,
            string secretName,
            UInt32 desiredAccess,
            out SecretHandle secretHandle)
        {
            ////Variable to store the secret name.
            _RPC_UNICODE_STRING[] SecretName = new _RPC_UNICODE_STRING[1];
            const uint SECRET_SET_QUERY_VALUE = 0x00000003;
            const uint SECRET_SET_VALUE = 0x00000001;
            const uint NAME_TOO_LONG = 128;

            ////constant variable to check for the access.
            ACCESS_MASK intTempAccess = ACCESS_MASK.NONE;

            this.secretFlag = true;
            this.strSystemOrLocal = false;
            this.localname = string.Empty;

            bool ifServiceStopped = ISUTControlAdapterInstance.IsDirectoryServiceStopped();

            if (string.IsNullOrEmpty(secretName) || secretName.StartsWith("G$$"))
            {
                this.utilities.nameOfSecretObject(ResOfNameChecked.InValid, ref SecretName);
            }
            else if (stPolicyInformation.PHandle != handleInput)
            {
                this.utilities.nameOfSecretObject(ResOfNameChecked.Valid, ref SecretName);
                this.utilities.inValidHandle();
            }
            else if (this.secretNameOfSecretObject == secretName)
            {
                this.utilities.nameOfSecretObject(ResOfNameChecked.Valid, ref SecretName);               

                uintMethodStatus = lsadClientStack.LsarCreateSecret(
                PolicyHandle.Value,
                SecretName[0],
                ACCESS_MASK.MAXIMUM_ALLOWED,
                out objSecretHandle);
            }
            else if ((uintAccessMask & ACCESS_MASK.POLICY_CREATE_SECRET) != ACCESS_MASK.POLICY_CREATE_SECRET)
            {
                this.utilities.nameOfSecretObject(ResOfNameChecked.Valid, ref SecretName);
            }
            else if (secretName.Length > NAME_TOO_LONG)
            {
                this.utilities.nameOfSecretObject(ResOfNameChecked.TooLong, ref SecretName);
            }
            else
            {
                if (ifServiceStopped)
                {
                    this.utilities.nameOfSecretObject(ResOfNameChecked.GlobalSecretName, ref SecretName);
                }
                else
                {
                    this.utilities.nameOfSecretObject(ResOfNameChecked.Valid, ref SecretName);
                }

                lsadClientStack.LsarOpenSecret(
                    PolicyHandle.Value,
                    SecretName[0],
                    ACCESS_MASK.DELETE,
                    out objSecretHandle);

                if (objSecretHandle != IntPtr.Zero)
                {
                    lsadClientStack.LsarDeleteObject(ref objSecretHandle);
                }
            }

            intTempAccess = uintAccessMask;

            if ((desiredAccess & SECRET_SET_QUERY_VALUE) == SECRET_SET_QUERY_VALUE)
            {
                uintAccessMask = ACCESS_MASK.SECRET_SET_VALUE | ACCESS_MASK.SECRET_QUERY_VALUE | ACCESS_MASK.DELETE;
            }
            else if ((desiredAccess & SECRET_SET_VALUE) == SECRET_SET_VALUE)
            {
                uintAccessMask = ACCESS_MASK.SECRET_SET_VALUE;
            }
            else
            {
                uintAccessMask = ACCESS_MASK.SECRET_QUERY_VALUE;
            }

            this.uintAccessToCreateSecretObject = desiredAccess;

            uintMethodStatus = lsadClientStack.LsarCreateSecret(
                PolicyHandle.Value,
                SecretName[0],
                uintAccessMask,
                out objSecretHandle);

            secretHandle = (objSecretHandle == IntPtr.Zero) ? SecretHandle.Invalid : SecretHandle.Valid;
            bool SecretNameIsNotSatisfied = true;

            if (ifServiceStopped)
            {
                if (isDC && this.utilities.strCheckSecretName.StartsWith("G$"))
                {
                    #region MS-LSAD_R545

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.DirectoryServiceRequired,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        545,
                        @"A LsarCreateSecret request to create a global secret on a Windows server that is not
                        a domain controller fails with status code STATUS_DIRECTORY_SERVICE_REQUIRED.");

                    #endregion
                }
            }
            else
            {
                for (int index = 0; index < SecretName.Length; index++)
                {
                    if (SecretName[index].Length % 2 != 0
                            || SecretName[index].Length > SecretName[index].MaximumLength
                            || (SecretName[index].Length != 0 && SecretName[index].Buffer == null)
                            || (Convert.ToString(SecretName[index].Buffer).Length < (SecretName[index].Length / 2)
                                    && PDCOSVersion != ServerVersion.Win2003))
                    {
                        SecretNameIsNotSatisfied = false;
                        break;
                    }
                }

                if (!SecretNameIsNotSatisfied)
                {
                    #region MS-LSAD_R1046

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.InvalidParameter,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        1046,
                        @"[In LsarCreateSecret method] SecretName: The server MUST verify that the name syntax
                    restrictions on secrets specified in section 3.1.4.10 are satisfied, and fail the request
                    with STATUS_INVALID_PARAMETER otherwise.");

                    #endregion
                }

                if (string.IsNullOrEmpty(secretName)
                        || this.utilities.strCheckSecretName.StartsWith("G$$")
                        || (this.utilities.strCheckSecretName.Equals("G$")
                        || this.utilities.strCheckSecretName.Equals("M$")
                        || this.utilities.strCheckSecretName.Equals("L$")
                        || this.utilities.strCheckSecretName.Equals("_sc_")
                        || this.utilities.strCheckSecretName.Equals("NL$")
                        || this.utilities.strCheckSecretName.Equals("RasDialParams")
                        || this.utilities.strCheckSecretName.StartsWith("RasCredentials")))
                {
                    if (isWindows)
                    {
                        if (string.IsNullOrEmpty(this.utilities.strCheckSecretName)
                            || this.utilities.strCheckSecretName.Contains("\\"))
                        {
                            #region MS-LSAD_R537

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.InvalidParameter,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                537,
                                @" In SecretName parameter of LsarCreateSecret method, server MUST also check that the 
                            following constraints are satisfied by SecretName and fail the request with 
                            STATUS_INVALID_PARAMETER if the name does not check out: Must not be empty. 
                            Must not contain the \ character.<63><64><65>");

                            #endregion
                        }

                        if (this.utilities.strCheckSecretName.StartsWith("G$$")
                                && PDCOSVersion >= ServerVersion.Win2003)
                        {
                            #region MS-LSAD_R541

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.InvalidParameter,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                541,
                                @"<64> Section 3.1.4.6.1: when implementing LsarCreateSecret,Windows 2000, Windows XP, 
                            Windows Server 2003, Windows Vista, Windows Server 2008, Windows 7, and 
                            Windows Server 2008 R2 do not allow a secret whose name is prefixed by G$$ to be created,
                            and return STATUS_INVALID_PARAMETER to indicate a failure to the caller.");

                            #endregion
                        }

                        if ((PDCOSVersion >= ServerVersion.Win2003)
                                 && (this.utilities.strCheckSecretName.StartsWith("G$$")
                                         || this.utilities.strCheckSecretName.StartsWith("G$")
                                         || this.utilities.strCheckSecretName.StartsWith("L$")
                                         || this.utilities.strCheckSecretName.StartsWith("M$")
                                         || this.utilities.strCheckSecretName.StartsWith("_sc_")
                                         || this.utilities.strCheckSecretName.StartsWith("NL$")
                                         || this.utilities.strCheckSecretName.Equals("RasDialParams")
                                         || this.utilities.strCheckSecretName.StartsWith("RasCredentials")))
                        {
                            #region MS-LSAD_R542

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.InvalidParameter,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                542,
                                @"<65> Section 3.1.4.6.1: when implementing LsarCreateSecret, Windows Server 2003 R2,
                            Windows Server 2003 SP1, Windows Vista, Windows Server 2008, Windows 7, and 
                            Windows Server 2008 R2 do not allow the secret name to be G$$, G$, L$, M$, _sc_, NL$, 
                            RasDialParams or RasCredentials. They return STATUS_INVALID_PARAMETER to indicate 
                            a failure to the caller.");

                            #endregion
                        }
                    }
                }
                else if (stPolicyInformation.PHandle != handleInput)
                {
                    lsadClientStack.LsarDeleteObject(ref PolicyHandle);
                    PolicyHandle = this.utilities.tempPolicyHandle;
                    objSecretHandle = this.utilities.tempSecretHandle;

                    #region MS-LSAD_R535

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.InvalidHandle,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        535,
                        @"In LsarCreateSecret method,If the Policyhandle is not a valid context handle 
                        to the policy object, the server MUST return STATUS_INVALID_HANDLE.");

                    #endregion
                }
                else if (this.secretNameOfSecretObject == secretName)
                {
                    #region MS-LSAD_R538

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.ObjectNameCollision,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        538,
                        @"In  LsarCreateSecret,server MUST check that the secret by the name SecretName does not
                    already exist and fail the request with STATUS_OBJECT_NAME_COLLISION otherwise.<66>");

                    #endregion
                }
                else if ((intTempAccess & ACCESS_MASK.POLICY_CREATE_SECRET) != ACCESS_MASK.POLICY_CREATE_SECRET)
                {
                    #region MS-LSAD_R536

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.AccessDenied,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        536,
                        @"For PolicyHandle parameter of LsarCreateSecret method, server MUST verify 
                        that the handle has POLICY_CREATE_SECRET access and fail the request with 
                        STATUS_ACCESS_DENIED if this is not the case.");

                    #endregion
                }
                else if (secretName.Length > NAME_TOO_LONG)
                {
                    if (isWindows)
                    {
                        if (PDCOSVersion == ServerVersion.Win2003)
                        {
                            #region MS-LSAD_R539

                            if (this.utilities.strCheckSecretName.Length > NAME_TOO_LONG)
                            {
                                Site.CaptureRequirementIfAreEqual<uint>(
                                    (uint)ErrorStatus.NameTooLong,
                                    (uint)uintMethodStatus,
                                    "MS-LSAD",
                                    539,
                                    @"<63> Section 3.1.4.6.1: Windows NT 4.0, Windows 2000, Windows XP, 
                                    Windows Server 2003,limit the secret name length to 128 characters. They return
                                    STATUS_NAME_TOO_LONG for lengths that are greater than 128 characters while 
                                    implementing LsarCreateSecret method.");
                            }

                            #endregion
                        }
                        else if (PDCOSVersion >= ServerVersion.Win2008)
                        {
                            #region MS-LSAD_R1047

                            if (this.utilities.strCheckSecretName.Length > NAME_TOO_LONG)
                            {
                                Site.CaptureRequirementIfAreEqual<uint>(
                                    (uint)ErrorStatus.InvalidParameter,
                                    (uint)uintMethodStatus,
                                    "MS-LSAD",
                                    1047,
                                    @"<63> Section 3.1.4.6.1: Windows Vista, Windows Server 2008, Windows 7, and 
                                Windows Server 2008 R2 return STATUS_INVALID_PARAMETER for lengths that are
                                greater than 128 characters.");
                            }

                            #endregion
                        }
                    }
                }
                else
                {
                    ////to check whether the object is a system object or local object.
                    if (isWindows)
                    {
                        if (secretName.StartsWith("L$")
                                || secretName.StartsWith("M$")
                                || secretName.StartsWith("_sc_")
                                || secretName.Contains("NL$")
                                || secretName.StartsWith("RasDialParams")
                                || secretName.StartsWith("RasCredentials")
                                || secretName.StartsWith("$MACHINE.ACC")
                                || (secretName == "SAC")
                                || (secretName == "SAI")
                                || (secretName == "SANSC"))
                        {
                            this.utilities.nameOfSecretObject(ResOfNameChecked.LocalSystem, ref SecretName);
                            this.localname = this.utilities.strCheckSecretName;
                            this.strSystemOrLocal = true;
                            lsadClientStack.LsarOpenSecret(
                                PolicyHandle.Value,
                                SecretName[0],
                                ACCESS_MASK.SECRET_QUERY_VALUE,
                                out this.objQuerySecretHandle);
                        }
                    }

                    stSecretInformation.strNameOfSecretObject = secretName;
                    stSecretInformation.UIntSecretHandleAccessCount = 
                        stSecretInformation.UIntSecretHandleAccessCount + 1;

                    #region MS-LSAD_R544

                    if (this.utilities.strCheckSecretName.Length != 0
                            || !this.utilities.strCheckSecretName.Contains("\\")
                            || !this.utilities.strCheckSecretName.StartsWith("G$$"))
                    {
                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.Success,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            544,
                            @"In LsarCreateSecret method,if request is succesfully completed 
                            then server MUST return STATUS_SUCCESS.");
                    }

                    #endregion
                }
            }

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion LsarCreateSecret

        #region LsarOpenSecret

        /// <summary>
        ///  The OpenSecret method is invoked to obtain a handle to an existing secret object.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="daclAllows">It is to check whether DACL allows the passed in desired access</param>
        /// <param name="secretName">Contains the secret name of an secret object to be opened</param>
        /// <param name="secretHandle">Outparam which contains valid or invalid secret handle</param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidParameter if the parameters passed to the method are not valid
        /// Returns AccessDenied if the caller does not have the permissions to 
        /// perform this operation
        /// Returns InvalidHandle if the passed in policy handle is not valid
        /// Returns ObjectNameCollision if secret with the specified name was not found</returns>
        public ErrorStatus OpenSecret(
            int handleInput,
            bool daclAllows,
            string secretName,
            out SecretHandle secretHandle)
        {
            ////Variable used to store the secret name.
            _RPC_UNICODE_STRING[] SecretName = new _RPC_UNICODE_STRING[1];

            ////A temporary variable used to
            _RPC_UNICODE_STRING[] openSecretName = new _RPC_UNICODE_STRING[1];

            ////Temporary secret handle used for the OpenSecretOpnum.
            IntPtr? objTempSecretHandle = IntPtr.Zero;
            IntPtr? TempPolicyHandle = IntPtr.Zero;
            IntPtr? TempSecretHandle = IntPtr.Zero;

            ////A temporary character array to convert the string into a character array.
            char[] tempName = new char[20];
            this.secretFlag = true;
            ACCESS_MASK tempAcces = ACCESS_MASK.SECRET_QUERY_VALUE | ACCESS_MASK.TRUSTED_SET_CONTROLLERS;
            ACCESS_MASK Access = ACCESS_MASK.NONE;

            const ACCESS_MASK SECRET_SET_AND_QUERY_VALUE = ACCESS_MASK.SECRET_SET_VALUE;

            if (string.IsNullOrEmpty(secretName) || secretName.StartsWith("G$$"))
            {
                this.utilities.nameOfSecretObject(ResOfNameChecked.Valid, ref SecretName);
                SecretName[0].Length = (ushort)((2 * SecretName[0].Buffer.Length) + 1);
                SecretName[0].MaximumLength = (ushort)((2 * SecretName[0].Length) + 2);
            }
            else if (stPolicyInformation.PHandle != handleInput)
            {
                this.utilities.nameOfSecretObject(ResOfNameChecked.Valid, ref SecretName);
                this.utilities.inValidHandle();
            }
            else if (!daclAllows)
            {
                this.utilities.nameOfSecretObject(ResOfNameChecked.Valid, ref SecretName);
                lsadClientStack.LsarOpenPolicy2(
                    "name",
                    objectAttributes,
                    ACCESS_MASK.POLICY_CREATE_SECRET,
                    out TempPolicyHandle);

                lsadClientStack.LsarCreateSecret(
                    TempPolicyHandle.Value,
                    SecretName[0],
                    ACCESS_MASK.DELETE,
                    out TempSecretHandle);

                Access = uintAccessMask;
                uintAccessMask = tempAcces;
            }
            else if (stSecretInformation.strNameOfSecretObject != secretName)
            {
                tempName = secretName.ToCharArray();
                openSecretName[0].Buffer = new ushort[tempName.Length];
                Array.Copy(tempName, openSecretName[0].Buffer, tempName.Length);
                openSecretName[0].Length = (ushort)(2 * openSecretName[0].Buffer.Length);
                openSecretName[0].MaximumLength = (ushort)(openSecretName[0].Length + 2);
            }
            else
            {
                this.utilities.nameOfSecretObject(ResOfNameChecked.Valid, ref SecretName);
            }

            if ((stSecretInformation.strNameOfSecretObject != secretName) && (openSecretName[0].Length != 0))
            {
                uintMethodStatus = lsadClientStack.LsarOpenSecret(
                    PolicyHandle.Value,
                    openSecretName[0],
                    uintAccessMask,
                    out objTempSecretHandle);

                openSecretName = new _RPC_UNICODE_STRING[1];

                secretHandle = (objTempSecretHandle == IntPtr.Zero) ? SecretHandle.Invalid : SecretHandle.Valid;
            }
            else
            {
                uintMethodStatus = lsadClientStack.LsarOpenSecret(
                    PolicyHandle.Value,
                    SecretName[0],
                    uintAccessMask,
                    out objTempSecretHandle);

                secretHandle = (objTempSecretHandle == IntPtr.Zero) ? SecretHandle.Invalid : SecretHandle.Valid;
            }

            if (string.IsNullOrEmpty(secretName) || secretName.StartsWith("G$$") || secretName.Contains("\\"))
            {
                #region MS-LSAD_R549

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidParameter,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    549,
                    @" In SecretName parameter of LsarOpenSecret method,The server MUST verify that the name syntax 
                    restrictions on secrets are satisfied, and fail the request with 
                    STATUS_INVALID_PARAMETER otherwise.");

                #endregion
            }
            else if (stPolicyInformation.PHandle != handleInput)
            {
                #region MS-LSAD_R548

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidHandle,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    548,
                    @"In LsarOpenSecret ,If Policyhandle is not a valid context handle to the policy 
                    object, the server MUST return STATUS_INVALID_HANDLE");

                #endregion

                lsadClientStack.LsarDeleteObject(ref PolicyHandle);
                PolicyHandle = this.utilities.tempPolicyHandle;
                objSecretHandle = this.utilities.tempSecretHandle;
            }
            else if (!daclAllows)
            {
                if (uintAccessMask != ACCESS_MASK.SECRET_SET_VALUE
                        || uintAccessMask != ACCESS_MASK.SECRET_QUERY_VALUE
                        || uintAccessMask != SECRET_SET_AND_QUERY_VALUE)
                {
                    #region MS-LSAD_R553

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.AccessDenied,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        553,
                        @" In DesiredAccess parameter of LsarOpenSecret method,server MUST verify that the secret 
                        object's discretionary access control list (DACL) allows the requested access and, 
                        failing that, return STATUS_ACCESS_DENIED.");

                    #endregion
                }

                if (TempSecretHandle != IntPtr.Zero)
                {
                    lsadClientStack.LsarDeleteObject(ref TempSecretHandle);
                }

                uintAccessMask = Access;
            }
            else if (stSecretInformation.strNameOfSecretObject != secretName)
            {
                #region MS-LSAD_R552

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.ObjectNameNotFound,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    552,
                    @" In DesiredAccess parameter of LsarOpenSecret method ,If the requestor is anonymous 
                    and the setting (Boolean setting that affects the processing of certain messages in LSAD 
                    protocol when the requester is anonymous) is set to true, the call MUST fail with 
                    STATUS_OBJECT_NAME_NOT_FOUND. ");

                #endregion

                #region MS-LSAD_R550

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.ObjectNameNotFound,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    550,
                    @"In SecretName parameter of LsarOpenSecret method,server MUST verify that the secret object 
                    with specified SecretName exists in its policy database and fail the request with 
                    STATUS_OBJECT_NAME_NOT_FOUND otherwise.<67>");

                #endregion
            }
            else
            {
                #region MS-LSAD_R554

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    554,
                    @"In LsarOpenSecret method,if request is succesfully completed then server 
                    MUST return STATUS_SUCCESS.");

                #endregion
            }

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion LsarOpenSecret

        #region LsarSetSecret

        /// <summary>
        ///  The SetSecret method is invoked to set the current and old values of the secret object.
        /// </summary>
        /// <param name="handleInput">Contains secret handle obtained from CreateSecret/OpenSecret </param>
        /// <param name="currentValue">Contains current value that is to be set to an secret object</param>
        /// <param name="oldValue">Contains old value that is to be set to an secret object</param>
        /// <param name="isValueNull">If isValueNull is 1: parameter of currentvalue and old value are not null;
        /// if isValueNull is 2: parameter of currentvalue is null;
        /// if isValueNull is 3: parameter of oldvalue is null;</param>
        /// <returns>Returns Success if the method is successful
        ///          Returns InvalidParameter if the parameters passed to the method are not valid
        ///          Returns AccessDenied if the caller does not have the permissions to 
        ///          perform this operation
        ///          Returns InvalidHandle if the passed in secret handle is not valid</returns>
        /// Disable warning CA1502 because it will affect the implementation of Adapter and Model codes
        /// if do any changes about maintainability.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", 
            Justification = "affect the implementation of Adapter and Model codes")]
        public ErrorStatus SetSecret(
            int handleInput,
            CipherValue currentValue,
            CipherValue oldValue,
            int isValueNull)
        {
            ////Structure used to pass the Current value.
            _LSAPR_CR_CIPHER_VALUE[] EncryptedCurrentValue = new _LSAPR_CR_CIPHER_VALUE[1];

            ////Structure used to pass the Old value.
            _LSAPR_CR_CIPHER_VALUE[] EncryptedOldValue = new _LSAPR_CR_CIPHER_VALUE[1];
            _LSAPR_OBJECT_ATTRIBUTES[] objectAttributesForOpenPolicy2 = new _LSAPR_OBJECT_ATTRIBUTES[1];

            ////Variable that is used to store the secret object name.
            _RPC_UNICODE_STRING[] SecretName = new _RPC_UNICODE_STRING[1];

            ////variable that contain the Current Set Time.
            _LARGE_INTEGER? queryCurrentSetTime = new _LARGE_INTEGER();

            ////variable that contain the Old Set Time.
            _LARGE_INTEGER? queryOldSetTime = new _LARGE_INTEGER();
            IntPtr? querySecretHandle = IntPtr.Zero;
            IntPtr? queryPolicyHandle = IntPtr.Zero;
            this.secretFlag = true;

            ////Variable used to store the Session key.
            byte[] byteKey = new byte[16];

            ////Constant Variable that are used to check for access.
            const ACCESS_MASK tempAccess = ACCESS_MASK.MAXIMUM_ALLOWED;

            ////Variable used to retrieve the current value from the ptf config file and pass 
            ////that to the EncryptedCurrentValue structure.                         
            string strSetCurrentValue = string.Empty;

            ////Variable used to retrieve the current value from the ptf config file and pass
            ////that to the EncryptedCurrentValue structure.
            string strSetOldValue = string.Empty;

            ////Varible used to check whether the programmer is used RPC Security or not. 
            ////This is False if not using else it is True.
            string isUsingRPCSecurity = "flase";

            ////Variable used to retrieve the hardcoded key from the ptf config file.   
            string hardCodedKey = string.Empty;

            ////Variable used to compare the stored value.
            string strQueryCurrentValue = string.Empty;

            ////Variable used to compare the stored value.      
            string strQueryOldValue = string.Empty;

            _RPC_UNICODE_STRING[] input = new _RPC_UNICODE_STRING[1];
            _RPC_UNICODE_STRING output = new _RPC_UNICODE_STRING();

            ////Structure to store the Current value to pass to the encrypt_secret funtion.
            _RPC_UNICODE_STRING[] inputCurrent = new _RPC_UNICODE_STRING[1];

            ////Structure to store the Old value to pass to the encrypt_secret funtion.
            _RPC_UNICODE_STRING[] inputOld = new _RPC_UNICODE_STRING[1];

            ////Structure to retrieve the Current value from the encrypt_secret funtion.
            _RPC_UNICODE_STRING outputCurrent = new _RPC_UNICODE_STRING();

            ////Structure to retrieve the Old value from the encrypt_secret funtion.
            _RPC_UNICODE_STRING outputOld = new _RPC_UNICODE_STRING();

            ////To check whether using RPC security is used or not.
            ////if it is true then hardcoded key is used to encrypt and decrypt the data.
            if (isUsingRPCSecurity == "true")
            {
                #region MS-LSAD_R9

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidParameter1,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    9,
                    @"Windows XP and Windows Vista MUST NOT support security-support providers
                    RPC_C_AUTHN_GSS_KERBEROS RPC_C_AUTHN_GSS_NEGOTIATE");

                #endregion

                hardCodedKey = HardCodedKey;
                byteKey = Encoding.ASCII.GetBytes(hardCodedKey);
            }
            else
            {
                byteKey = lsadAdapter.SessionKey;
            }

            strSetCurrentValue = InputCurrentMessage;
            strSetOldValue = InputOldMessage;

            inputCurrent[0] = DtypUtility.ToRpcUnicodeString(strSetCurrentValue);

            if (strSetCurrentValue == "null")
            {
                inputCurrent[0].Length = (ushort)0;
                inputCurrent[0].MaximumLength = inputCurrent[0].Length;
                inputCurrent[0].Buffer = this.utilities.ConversionfromStringtoushortArray(string.Empty);
            }

            LsaUtility.EncryptSecret(inputCurrent[0], byteKey, out outputCurrent);

            EncryptedCurrentValue[0].Buffer = new byte[outputCurrent.Length];

            Buffer.BlockCopy(outputCurrent.Buffer, 0, EncryptedCurrentValue[0].Buffer, 0, outputCurrent.Length);

            EncryptedCurrentValue[0].Length = (uint)EncryptedCurrentValue[0].Buffer.Length;
            EncryptedCurrentValue[0].MaximumLength = EncryptedCurrentValue[0].Length;

            inputOld[0] = DtypUtility.ToRpcUnicodeString(strSetOldValue);

            if (strSetOldValue == "null")
            {
                inputOld[0].Length = (ushort)0;
                inputOld[0].MaximumLength = inputOld[0].Length;
                inputOld[0].Buffer = this.utilities.ConversionfromStringtoushortArray(string.Empty);
            }

            LsaUtility.EncryptSecret(inputOld[0], byteKey, out outputOld);

            EncryptedOldValue[0].Buffer = new byte[outputOld.Length];

            Buffer.BlockCopy(outputOld.Buffer, 0, EncryptedOldValue[0].Buffer, 0, outputOld.Length);

            EncryptedOldValue[0].Length = (uint)EncryptedOldValue[0].Buffer.Length;
            EncryptedOldValue[0].MaximumLength = EncryptedOldValue[0].Length;

            if (currentValue == CipherValue.Invalid || oldValue == CipherValue.Invalid)
            {
                EncryptedCurrentValue[0].Length = (uint)EncryptedCurrentValue[0].Buffer.Length;
                EncryptedOldValue[0].Length = (uint)EncryptedOldValue[0].Buffer.Length;
                if (currentValue == CipherValue.Invalid)
                {
                    EncryptedCurrentValue[0].MaximumLength = (uint)(EncryptedCurrentValue[0].Length - 1);
                    EncryptedCurrentValue[0].Buffer = null;
                }
                else
                {
                    EncryptedCurrentValue[0].MaximumLength = (uint)EncryptedCurrentValue[0].Length;
                }

                if (oldValue == CipherValue.Invalid)
                {
                    EncryptedOldValue[0].MaximumLength = (uint)(EncryptedOldValue[0].Length - 1);
                    EncryptedOldValue[0].Buffer = null;
                }
                else
                {
                    EncryptedOldValue[0].MaximumLength = (uint)EncryptedOldValue[0].Length;
                }
            }
            else if (stSecretInformation.UIntSecretHandleAccessCount != handleInput)
            {
                this.utilities.inValidHandle();
            }

            if (isValueNull == 1)
            {
                uintMethodStatus = lsadClientStack.LsarSetSecret(
                    objSecretHandle.Value,
                    EncryptedCurrentValue[0],
                    EncryptedOldValue[0]);
            }
            else if (isValueNull == 2)
            {
                uintMethodStatus = lsadClientStack.LsarSetSecret(
                objSecretHandle.Value,
                null,
                EncryptedOldValue[0]);
            }
            else if (isValueNull == 3)
            {
                uintMethodStatus = lsadClientStack.LsarSetSecret(
                objSecretHandle.Value,
                EncryptedCurrentValue[0],
                null);
            }            

            inputCurrent = new _RPC_UNICODE_STRING[1];
            inputOld = new _RPC_UNICODE_STRING[1];
            outputCurrent = new _RPC_UNICODE_STRING();
            outputOld = new _RPC_UNICODE_STRING();

            if (currentValue == CipherValue.Invalid || oldValue == CipherValue.Invalid)
            {
                #region MS-LSAD_R561

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidParameter,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    561,
                    @"In  LsarSetSecret , if One or more of the supplied parameters was invalid, server MUST 
                    return STATUS_INVALID_PARAMETER");

                #endregion
            }
            else if (stSecretInformation.UIntSecretHandleAccessCount != handleInput)
            {
                #region MS-LSAD_R556

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidHandle,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    556,
                    @"In LsarSetSecret method,If Secrethandle is not a valid context handle to a secret 
                    object, the server MUST return STATUS_INVALID_HANDLE.");

                #endregion

                lsadClientStack.LsarDeleteObject(ref objSecretHandle);
                PolicyHandle = this.utilities.tempPolicyHandle;
                objSecretHandle = this.utilities.tempSecretHandle;
            }
            else if ((uintAccessMask & ACCESS_MASK.SECRET_SET_VALUE) != ACCESS_MASK.SECRET_SET_VALUE)
            {
                if (uintAccessMask != ACCESS_MASK.SECRET_SET_VALUE)
                {
                    #region MS-LSAD_R557

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.AccessDenied,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        557,
                        @"In SecretHandle parameter of LsarSetSecret method ,the handle must have been opened for 
                        SECRET_SET_VALUE access. The server MUST perform this access check and fail the request with 
                        STATUS_ACCESS_DENIED if the access check is not successful.<68>");

                    #endregion
                }
            }
            else
            {
                this.utilities.nameOfSecretObject(ResOfNameChecked.Valid, ref SecretName);

                lsadClientStack.LsarOpenPolicy2(
                    "name",
                    objectAttributesForOpenPolicy2[0],
                    tempAccess,
                    out queryPolicyHandle);

                lsadClientStack.LsarOpenSecret(
                    queryPolicyHandle.Value,
                    SecretName[0],
                    tempAccess,
                    out querySecretHandle);

                _LSAPR_CR_CIPHER_VALUE? EncryptedCurrentValue1 = EncryptedCurrentValue[0];
                _LSAPR_CR_CIPHER_VALUE? EncryptedOldValue1 = EncryptedOldValue[0];

                lsadClientStack.LsarQuerySecret(
                    querySecretHandle.Value,
                    ref EncryptedCurrentValue1,
                    ref queryCurrentSetTime,
                    ref EncryptedOldValue1,
                    ref queryOldSetTime);

                if ((queryCurrentSetTime.Value.QuadPart != 0) || (queryOldSetTime.Value.QuadPart != 0))
                {
                    #region MS-LSAD_R560

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.Success,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        560,
                        @"In  LsarSetSecret , server MUST also maintain time stamp values 
                        for current and old values of the secret object.");

                    #endregion
                }

                string IsR101042Implemented = R101042Implementation;
                bool bLsarSetSecretIsSupported = LsarSetSecretIsSupported;

                if (isWindows)
                {
                    if (PDCOSVersion == ServerVersion.Win2008 
                            || PDCOSVersion == ServerVersion.Win2003)
                    {
                        #region MS-LSAD_R111043

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.Success,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            111043,
                            @"[<62> Section 3.1.4.6:] Windows 2000 Server, Windows XP, Windows Server 2003, 
                            Windows Vista, and Windows Server 2008 support method LsarSetSecret.");

                        #endregion
                    }

                    if (PDCOSVersion >= ServerVersion.Win2008R2)
                    {
                        if (bLsarSetSecretIsSupported)
                        {
                            #region MS-LSAD_R151043

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.Success,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                151043,
                                @"[<62> Section 3.1.4.6:] Windows 7 and Windows Server 2008 R2 support
                                method LsarSetSecret by default.");

                            #endregion
                        }
                    }                 

                    if (IsR101042Implemented == null)
                    {
                        Site.Properties.Add("IsR101042Implemented", bool.TrueString);
                        IsR101042Implemented = bool.TrueString;
                    }
                }
                else
                {
                    if (IsR101042Implemented != null)
                    {
                        bool implSigns = bool.Parse(IsR101042Implemented);

                        if (implSigns)
                        {
                            bool isSatisfied = ((uint)ErrorStatus.Success == (uint)uintMethodStatus);

                            #region MS-LSAD_R101042

                            Site.CaptureRequirementIfAreEqual<bool>(
                                implSigns,
                                isSatisfied,
                                "MS-LSAD",
                                101042,
                                @"The server MAY NOT support the LsarSetSecret method.<62> ");

                            #endregion
                        }
                    }
                }

                #region MS-LSAD_R563

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    563,
                    @"In  LsarSetSecret , if request is  successfully completed then server MUST 
                    return STATUS_SUCCESS.");

                #endregion

                if (isValueNull == 2)
                {
                    if (EncryptedCurrentValue1 == null)
                    {
                        #region MS-LSAD_R558

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.Success,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            558,
                            @"In LsarSetSecret method ,server MUST delete the current value in its database 
                            if EncryptedCurrentValue parameter is NULL");

                        #endregion
                    }
                }

                if (isValueNull == 3)
                {
                    input[0] = DtypUtility.ToRpcUnicodeString(queryPolicyHandle.ToString());
                    input[0].Buffer = new ushort[(int)EncryptedCurrentValue1.Value.Length];

                    Buffer.BlockCopy(
                        EncryptedCurrentValue1.Value.Buffer, 
                        0, 
                        input[0].Buffer, 
                        0, 
                        (int)EncryptedCurrentValue1.Value.Length);

                    input[0].Length = (ushort)EncryptedCurrentValue1.Value.Length;
                    input[0].MaximumLength = input[0].Length;

                    LsaUtility.DecryptSecret(input[0], byteKey, out output);

                    strQueryCurrentValue = DtypUtility.ToString(output);

                    Buffer.BlockCopy(
                        EncryptedCurrentValue1.Value.Buffer,
                        0,
                        input[0].Buffer,
                        0,
                        (int)EncryptedCurrentValue1.Value.Length);

                    input[0].Length = (ushort)EncryptedCurrentValue1.Value.Length;
                    input[0].MaximumLength = input[0].Length;

                    LsaUtility.DecryptSecret(input[0], byteKey, out output);

                    strQueryOldValue = DtypUtility.ToString(output);

                    if (strQueryCurrentValue == strQueryOldValue && EncryptedOldValue1 == null)
                    {
                        #region MS-LSAD_R559

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.Success,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            559,
                            @"In LsarSetSecret , server MUST delete the old value in its database and replace 
                            it with the previous version of CurrentValue if EncryptedOldValue parameter is NULL");

                        #endregion
                    }
                }
            }

            bool IsLsarSetSecretSupported = LsarSetSecretIsSupported;

            if (IsLsarSetSecretSupported && (uint)uintMethodStatus == (uint)ErrorStatus.Success)
            {
                ////if LsarSetSecretIfIsSupported is true, it means the server performs the operations in the message
                ////processing section for this method.
                #region MS-LSAD_R101045

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    101045,
                    @"If the server supports method LsarSetSecret, the server MUST perform the operations
                    in the message processing section for this method.");

                #endregion
            }

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion LsarSetSecret

        #region LsarQuerySecret

        /// <summary>
        ///  The QuerySecret method is invoked to retrieve the current and old (or previous) value of
        ///  the secret object.
        /// </summary>
        /// <param name="handleInput">Contains secret handle obtained from CreateSecret/OpenSecret </param>
        /// <param name="encryptedCurrentValue">Out param which contains current value that 
        /// is to be retrieved from a secret object</param>
        /// <param name="currentValueSetTime">Out param which contains current value set time that 
        /// is to be retrieved from a secret object</param>
        /// <param name="encryptedOldValue">Out param which contains old value that 
        /// is to be retrieved from a secret object</param>
        /// <param name="oldValueSetTime">Out param which contains old value set time that 
        /// is to be retrieved from a secret object</param>
        /// <returns>Returns Success if the method is successful
        ///          Returns AccessDenied if the caller does not have the permissions to 
        ///          perform this operation
        ///          Returns InvalidHandle if the passed in secret handle is not valid</returns>
        /// Disable warning CA1502 because it will affect the implementation of Adapter and Model codes if do
        /// any changes about maintainability.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity",
            Justification = "affect the implementation of Adapter and Model codes")]
        public ErrorStatus QuerySecret(
            int handleInput,
            out EncryptedValue encryptedCurrentValue,
            out ValueSetTime currentValueSetTime,
            out EncryptedValue encryptedOldValue,
            out ValueSetTime oldValueSetTime)
        {
            ////Variabel used to store the Current Set time retrieved using LsarQuerySecret
            _LARGE_INTEGER? CurrentValueSetTime = new _LARGE_INTEGER();

            ////Variabel used to store the Old Set time retrieved using LsarQuerySecret
            _LARGE_INTEGER? OldValueSetTime = new _LARGE_INTEGER();
            IntPtr? PolicyHandle2 = IntPtr.Zero;
            IntPtr? queryTempSecretHandle = IntPtr.Zero;
            string strSetCurrentValue = string.Empty;
            string strSetOldValue = string.Empty;
            string isUsingRPCSecurity = string.Empty;
            string hardCodedKey = string.Empty;

            ////Variables used to store the Encrypted Current value Retrieved using LsarQuerySecret.
            _LSAPR_CR_CIPHER_VALUE[] EncryptedCurrentValue = new _LSAPR_CR_CIPHER_VALUE[1];

            ////Variables used to store the Encrypted Old value Retrieved using LsarQuerySecret.
            _LSAPR_CR_CIPHER_VALUE[] EncryptedOldValue = new _LSAPR_CR_CIPHER_VALUE[1];
            _LSAPR_OBJECT_ATTRIBUTES[] objectAttributesForOpenPolicy2 = new _LSAPR_OBJECT_ATTRIBUTES[1];

            this.secretFlag = true;
            byte[] byteKey = new byte[16];
            string strQueryCurrentValue = string.Empty;
            string setQueryOldValue = string.Empty;
            _RPC_UNICODE_STRING[] input = new _RPC_UNICODE_STRING[1];
            _RPC_UNICODE_STRING output = new _RPC_UNICODE_STRING();

            if (isUsingRPCSecurity == "true")
            {
                hardCodedKey = HardCodedKey;
                byteKey = Encoding.ASCII.GetBytes(hardCodedKey);
            }
            else
            {
                byteKey = lsadAdapter.SessionKey;
            }

            strSetCurrentValue = InputCurrentMessage;
            strSetOldValue = InputOldMessage;

            if (stSecretInformation.UIntSecretHandleAccessCount != handleInput)
            {
                this.utilities.inValidHandle();
            }
            else if (this.strSystemOrLocal)
            {
                queryTempSecretHandle = objSecretHandle;
                objSecretHandle = this.objQuerySecretHandle;
            }

            _LSAPR_CR_CIPHER_VALUE? EncryptedCurrentValue1 = EncryptedCurrentValue[0];
            _LSAPR_CR_CIPHER_VALUE? EncryptedOldValue1 = EncryptedOldValue[0];
            uintMethodStatus = lsadClientStack.LsarQuerySecret(
                objSecretHandle.Value,
                ref EncryptedCurrentValue1,
                ref CurrentValueSetTime,
                ref EncryptedOldValue1,
                ref OldValueSetTime);

            encryptedCurrentValue = ((EncryptedCurrentValue1 == null || EncryptedCurrentValue1.Value.Buffer == null)
                                          && CurrentValueSetTime.Value.QuadPart == 0)
                                             ? EncryptedValue.Invalid : EncryptedValue.Valid;

            currentValueSetTime = (CurrentValueSetTime.Value.QuadPart == 0) ? ValueSetTime.Invalid : ValueSetTime.Valid;
            encryptedOldValue = ((EncryptedOldValue1 == null || EncryptedOldValue1.Value.Buffer == null)
                                      && OldValueSetTime.Value.QuadPart == 0)
                                         ? EncryptedValue.Invalid : EncryptedValue.Valid;

            oldValueSetTime = (OldValueSetTime.Value.QuadPart == 0) ? ValueSetTime.Invalid : ValueSetTime.Valid;

            if (stSecretInformation.UIntSecretHandleAccessCount != handleInput)
            {
                #region MS-LSAD_R565

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidHandle,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    565,
                    @"In  LsarQuerySecret if secrethandle is not a valid context handle to a secret 
                    object, the server MUST return STATUS_INVALID_HANDLE.");

                #endregion

                lsadClientStack.LsarDeleteObject(ref objSecretHandle);
                PolicyHandle = this.utilities.tempPolicyHandle;
                objSecretHandle = this.utilities.tempSecretHandle;
            }
            else if ((uintAccessMask & ACCESS_MASK.SECRET_QUERY_VALUE) != ACCESS_MASK.SECRET_QUERY_VALUE)
            {
                #region MS-LSAD_R566

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.AccessDenied,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    566,
                    @"In SecretHandle parameter of LsarQuerySecret method, server MUST verify that the handle 
                    has been opened with SECRET_QUERY_VALUE bit and reject the request with 
                    STATUS_ACCESS_DENIED if it has not.<70>");

                #endregion
            }
            else
            {
                lsadClientStack.LsarOpenPolicy2(
                    "name",
                    objectAttributesForOpenPolicy2[0],
                    ACCESS_MASK.DELETE,
                    out PolicyHandle2);

                input[0] = new _RPC_UNICODE_STRING();

                if (this.localname.Contains("L$")
                         || this.localname.Contains("M$")
                         || this.localname.Contains("_sc_")
                         || this.localname.Contains("NL$")
                         || this.localname.Contains("RasDialParams")
                         || this.localname.Contains("RasCredentials")
                         || this.localname.Contains("$MACHINE.ACC")
                         || (this.localname == "SAC")
                         || (this.localname == "SAI")
                         || (this.localname == "SANSC")
                         && isWindows)
                {
                    if (this.localname.Contains("L$")
                             || this.localname.Contains("RasDialParams")
                             || this.localname.Contains("RasCredentials")
                             || this.localname.Contains("$MACHINE.ACC")
                             || (this.localname == "SAI")
                             || (this.localname == "SANSC"))
                    {
                        #region MS-LSAD_R567

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.AccessDenied,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            567,
                            @"<71> Section 3.1.4.6.4: Windows rejects the LsarQuerySecret requests of type
                            system by returning STATUS_ACCESS_DENIED.");

                        #endregion

                        #region MS-LSAD_R568

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.AccessDenied,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            568,
                            @"<71> Section 3.1.4.6.4: Windows  rejects the secret LsarQuerySecret requests of type
                            local from network clients with STATUS_ACCESS_DENIED.");

                        #endregion
                    }

                    objSecretHandle = queryTempSecretHandle;
                }
                else
                {
                    if (EncryptedCurrentValue1 != null)
                    {
                        input[0].Buffer = new ushort[(int)EncryptedCurrentValue1.Value.Length];
                        Buffer.BlockCopy(
                            EncryptedCurrentValue1.Value.Buffer, 
                            0, 
                            input[0].Buffer, 
                            0, 
                            (int)EncryptedCurrentValue1.Value.Length);

                        input[0].Length = (ushort)EncryptedCurrentValue1.Value.Length;
                        input[0].MaximumLength = input[0].Length;

                        LsaUtility.DecryptSecret(input[0], byteKey, out output);

                        strQueryCurrentValue = DtypUtility.ToString(output);

                        if (output.Length == 0)
                        {
                            strQueryCurrentValue = "null";
                        }

                        if (EncryptedCurrentValue[0].MaximumLength > 0)
                        {
                            #region MS-LSAD_R97

                            Site.CaptureRequirementIfIsNotNull(
                                EncryptedCurrentValue[0].Buffer,
                                "MS-LSAD",
                                97,
                                @"In LSAPR_CR_CIPHER_VALUE structure under filed 'Buffer' : If the value of the 
                                MaximumLength field is greater than 0, Buffer field MUST contain a non-NULL value");

                            #endregion
                        }
                    }

                    input[0] = DtypUtility.ToRpcUnicodeString(PolicyHandle2.ToString());

                    if (EncryptedOldValue1 != null)
                    {
                        input[0].Buffer = new ushort[(int)EncryptedOldValue1.Value.Length];
                        Buffer.BlockCopy(
                            EncryptedOldValue1.Value.Buffer, 
                            0, 
                            input[0].Buffer, 
                            0, 
                            (int)EncryptedOldValue1.Value.Length);

                        input[0].Length = (ushort)EncryptedOldValue1.Value.Length;
                        input[0].MaximumLength = input[0].Length;

                        LsaUtility.DecryptSecret(input[0], byteKey, out output);

                        setQueryOldValue = DtypUtility.ToString(output);

                        if (EncryptedOldValue1.Value.MaximumLength > 0)
                        {
                            #region MS-LSAD_R97

                            Site.CaptureRequirementIfIsNotNull(
                                EncryptedOldValue1.Value.Buffer,
                                "MS-LSAD",
                                97,
                                @"In LSAPR_CR_CIPHER_VALUE structure under filed 'Buffer' : If the value of the 
                                MaximumLength field is greater than 0, Buffer field MUST contain a non-NULL value");

                            #endregion
                        }
                    }
                }

                if (strSetCurrentValue == strQueryCurrentValue && strSetOldValue == setQueryOldValue)
                {
                    #region MS-LSAD_R569

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.Success,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        569,
                        @"In  LsarQuerySecret , if request is  successfully completed then server 
                        MUST return STATUS_SUCCESS.");

                    #endregion

                    string IsR201042Implemented = R201042Implementation;
                    bool bLsarQuerySecretIsSupported = LsarQuerySecretIsSupported;

                    if (isWindows)
                    {
                        if (PDCOSVersion == ServerVersion.Win2008
                                || PDCOSVersion == ServerVersion.Win2003)
                        {
                            #region MS-LSAD_R121043

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.Success,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                121043,
                                @"[<62> Section 3.1.4.6:] Windows 2000 Server, Windows XP, Windows Server 2003,
                                Windows Vista, and Windows Server 2008 support method LsarQuerySecret.");

                            #endregion
                        }

                        if (PDCOSVersion >= ServerVersion.Win2008R2)
                        {
                            if (bLsarQuerySecretIsSupported)
                            {
                                #region MS-LSAD_R161043

                                Site.CaptureRequirementIfAreEqual<uint>(
                                    (uint)ErrorStatus.Success,
                                    (uint)uintMethodStatus,
                                    "MS-LSAD",
                                    161043,
                                    @"[<62> Section 3.1.4.6:] Windows 7 and Windows Server 2008 R2 
                                    support method LsarQuerySecret by default.");

                                #endregion
                            }
                        }

                        if (IsR201042Implemented == null)
                        {
                            Site.Properties.Add("IsR201042Implemented", bool.TrueString);
                            IsR201042Implemented = bool.TrueString;
                        }
                    }
                    else
                    {
                        if (IsR201042Implemented != null)
                        {
                            bool implSigns = Boolean.Parse(IsR201042Implemented);

                            if (implSigns)
                            {
                                bool isSatisfied = ((uint)ErrorStatus.Success == (uint)uintMethodStatus);

                                #region MS-LSAD_R201042

                                Site.CaptureRequirementIfAreEqual<bool>(
                                    implSigns,
                                    isSatisfied,
                                    "MS-LSAD",
                                    201042,
                                    @"The server MAY NOT support the LsarQuerySecret method.<62> ");

                                #endregion
                            }
                        }
                    }

                    if (LsarQuerySecretIsSupported)
                    {
                        ////if LsarQuerySecretIfIsSupported is true, it means the server performs the operations 
                        ////in the message processing section for this method.
                        #region MS-LSAD_R111045

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.Success,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            111045,
                            @"If the server supports method LsarQuerySecret, the server MUST perform the operations
                            in the message processing section for this method.");

                        #endregion
                    }
                }
            }

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion LsarQuerySecret

        #region LsarStorePrivateData

        /// <summary>
        ///  The StorePrivateData method is invoked to store a secret value.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="secretName">Contains Secret name of an secret object</param>
        /// <param name="encryptedData">Contains data that is to be stored in a secret object</param>
        /// <returns>Returns Success if the method is successful
        ///          Returns InvalidParameter if the parameters passed to the method are not valid
        ///          Returns AccessDenied if the caller does not have the permissions to 
        ///          perform this operation
        ///          Returns InvalidHandle if the passed in policy handle is not valid</returns>
        /// Disable warning CA1502 because it will affect the implementation of Adapter and Model codes if do
        /// any changes about maintainability.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", 
            Justification = "affect the implementation of Adapter and Model codes")]
        public ErrorStatus StorePrivateData(
            int handleInput,
            string secretName,
            EncryptedValue encryptedData)
        {
            _RPC_UNICODE_STRING[] SecretName = new _RPC_UNICODE_STRING[1];
            _LSAPR_CR_CIPHER_VALUE[] EncryptedData = new _LSAPR_CR_CIPHER_VALUE[1];
            IntPtr? tempPolicyHandle = IntPtr.Zero;
            IntPtr? tempSecretHandle = IntPtr.Zero;
            string strEcryptedData = string.Empty;
            string hardCodedKey = string.Empty;
            string isUsingRPCSecurity = string.Empty;
            ACCESS_MASK temporaryAccess = ACCESS_MASK.NONE;
            byte[] byteKey = new byte[16];
            _RPC_UNICODE_STRING[] input = new _RPC_UNICODE_STRING[1];
            _RPC_UNICODE_STRING output = new _RPC_UNICODE_STRING();
            const uint POLICY_CREATE_SECRET = 32;

            this.secretFlag = true;
            temporaryAccess = uintAccessMask;

            if (isUsingRPCSecurity == "true")
            {
                hardCodedKey = HardCodedKey;
                byteKey = Encoding.ASCII.GetBytes(hardCodedKey);
            }
            else
            {
                byteKey = lsadAdapter.SessionKey;
            }

            strEcryptedData = InputEncryptedMessage;

            if (encryptedData == EncryptedValue.Null)
            {
                strEcryptedData = null;
            }

            if (encryptedData == EncryptedValue.Invalid)
            {
                this.utilities.nameOfSecretObject(ResOfNameChecked.Valid, ref SecretName);
                SecretName[0].Length = (ushort)((2 * SecretName[0].Buffer.Length) + 1);
                SecretName[0].MaximumLength = (ushort)(SecretName[0].Length + 2);
            }
            else if (stPolicyInformation.PHandle != handleInput)
            {
                this.utilities.nameOfSecretObject(ResOfNameChecked.Valid, ref SecretName);
                this.utilities.inValidHandle();
            }
            else if (stSecretInformation.strNameOfSecretObject == secretName)
            {
                if ((uintAccessMask & ACCESS_MASK.TRUSTED_QUERY_DOMAIN_NAME) == ACCESS_MASK.TRUSTED_QUERY_DOMAIN_NAME 
                         && strEcryptedData != null)
                {
                    this.utilities.nameOfSecretObject(ResOfNameChecked.Valid, ref SecretName);
                }
                else
                {
                    if (objSecretHandle == IntPtr.Zero)
                    {
                        this.utilities.nameOfSecretObject(ResOfNameChecked.Valid, ref SecretName);
                    }
                    else
                    {
                        temporaryAccess = uintAccessMask;
                        uintAccessMask = ACCESS_MASK.SECRET_QUERY_VALUE;
                    }
                }

                this.utilities.nameOfSecretObject(ResOfNameChecked.Valid, ref SecretName);
            }
            else
            {
                if (strEcryptedData != null)
                {
                    if ((this.uintAccessToCreateSecretObject & POLICY_CREATE_SECRET) == POLICY_CREATE_SECRET)
                    {
                        this.utilities.nameOfSecretObject(ResOfNameChecked.Valid, ref SecretName);
                        if (objSecretHandle == IntPtr.Zero)
                        {
                            lsadClientStack.LsarOpenPolicy2(
                                "name",
                                objectAttributes,
                                ACCESS_MASK.POLICY_CREATE_SECRET,
                                out tempPolicyHandle);

                            lsadClientStack.LsarCreateSecret(
                                tempPolicyHandle.Value,
                                SecretName[0],
                                ACCESS_MASK.SECRET_SET_VALUE,
                                out tempSecretHandle);
                        }

                        stSecretInformation.strNameOfSecretObject = secretName;
                    }
                }

                this.utilities.nameOfSecretObject(ResOfNameChecked.Valid, ref SecretName);
            }

            input[0] = DtypUtility.ToRpcUnicodeString(strEcryptedData);

            if (strEcryptedData == null)
            {
                input[0].Length = 0;
                input[0].Buffer = this.utilities.ConversionfromStringtoushortArray(string.Empty);
            }

            LsaUtility.EncryptSecret(input[0], byteKey, out output);

            EncryptedData[0].Buffer = new byte[output.Length];

            Buffer.BlockCopy(output.Buffer, 0, EncryptedData[0].Buffer, 0, output.Length);

            EncryptedData[0].Length = output.Length;
            EncryptedData[0].MaximumLength = output.MaximumLength;

            uintMethodStatus = lsadClientStack.LsarStorePrivateData(
                PolicyHandle.Value,
                SecretName[0],
                EncryptedData[0]);

            if (encryptedData == EncryptedValue.Invalid)
            {
                if (string.IsNullOrEmpty(secretName) 
                        || secretName.StartsWith("G$$") 
                        || ((SecretName[0].Length - 2) != SecretName[0].MaximumLength))
                {
                    #region MS-LSAD_R575

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.InvalidParameter,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        575,
                        @"The server MUST verify that KeyName is syntactically Valid and reject the 
                        request with STATUS_InvalidParameter otherwise.");

                    #endregion
                }
            }
            else if (stPolicyInformation.PHandle != handleInput)
            {
                #region MS-LSAD_R571

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.InvalidHandle,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    571,
                    @"In LsarStorePrivateData ,If Policyhandle is not a valid context handle to policy 
                    object, the server MUST return STATUS_INVALID_HANDLE.");

                #endregion

                lsadClientStack.LsarDeleteObject(ref PolicyHandle);
                PolicyHandle = this.utilities.tempPolicyHandle;
                objSecretHandle = this.utilities.tempSecretHandle;
            }
            else if (stSecretInformation.strNameOfSecretObject == secretName)
            {
                string IsR301042Implemented = R301042Implementation;
                bool bLsarStorePrivateDataIsSupported = LsarStorePrivateDataIsSupported;
                if (isWindows)
                {
                    if (PDCOSVersion == ServerVersion.Win2008
                            || PDCOSVersion == ServerVersion.Win2003)
                    {
                        #region MS-LSAD_R131043

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.Success,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            131043,
                            @"[<62> Section 3.1.4.6:] Windows 2000 Server, Windows XP, Windows Server 2003, 
                            Windows Vista, and Windows Server 2008 support method LsarStorePrivateData.");

                        #endregion
                    }

                    if (PDCOSVersion >= ServerVersion.Win2008R2)
                    {
                        if (bLsarStorePrivateDataIsSupported)
                        {
                            #region MS-LSAD_R171043

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.Success,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                171043,
                                @"[<62> Section 3.1.4.6:] Windows 7 and Windows Server 2008 R2 support method 
                                LsarStorePrivateData by default.");

                            #endregion
                        }
                    }

                    if (IsR301042Implemented == null)
                    {
                        Site.Properties.Add("IsR301042Implemented", bool.TrueString);
                        IsR301042Implemented = bool.TrueString;
                    }
                }
                else
                {
                    if (IsR301042Implemented != null)
                    {
                        bool implSigns = bool.Parse(IsR301042Implemented);

                        if (implSigns)
                        {
                            bool isSatisfied = ((uint)ErrorStatus.Success == (uint)uintMethodStatus);

                            #region MS-LSAD_R301042

                            Site.CaptureRequirementIfAreEqual<bool>(
                                implSigns,
                                isSatisfied,
                                "MS-LSAD",
                                301042,
                                @"The server MAY NOT support the LsarStorePrivateData method.<62> ");

                            #endregion
                        }
                    }
                }

                if ((uintAccessMask & ACCESS_MASK.SECRET_SET_VALUE) == 
                    ACCESS_MASK.SECRET_SET_VALUE && strEcryptedData != null)
                {
                    #region MS-LSAD_R573

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.Success,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        573,
                        @"In KeyName parameter of  LsarStorePrivateData method If access check 
                        fails, the server MUST return STATUS_ACCESS_DENIED. ");

                    #endregion
                }
                else
                {
                    uintAccessMask = temporaryAccess;
                    return ErrorStatus.AccessDenied;
                }
            }

            bool IsLsarStorePrivateDateSupported = LsarStorePrivateDataIsSupported;

            if (IsLsarStorePrivateDateSupported && (uint)ErrorStatus.Success == (uint)uintMethodStatus)
            {
                ////if LsarStorePrivateDataIfIsSupported is true, it means the server performs the operations
                ////in the message processing section for this method.
                #region MS-LSAD_R121045

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    121045,
                    @"If the server supports method LsarStorePrivateData, the server MUST perform the operations
                    in the message processing section for this method.");

                #endregion
            }

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion LsarStorePrivateData

        #region LsarRetrievePrivateData

        /// <summary>
        ///  The RetrievePrivateData method is invoked to retrieve a secret value.
        /// </summary>
        /// <param name="handleInput">Contains policy handle obtained from OpenPolicy/OpenPolicy2 </param>
        /// <param name="name">It is for validating the secret name passed whether it is valid or invalid</param>
        /// <param name="secretName">Contains Secret name of an secret object</param>
        /// <param name="encryptedData">Out param which contains data that is to be 
        /// retrieved from a secret object</param>
        /// <returns>Returns Success if the method is successful
        ///          Returns InvalidParameter if the parameters passed to the method are not valid
        ///          Returns AccessDenied if the caller does not have the permissions to 
        ///          perform this operation
        ///          Returns InvalidHandle if the passed in policy handle is not valid
        ///          returns ObjectNameNotFound if the specified secret name was not found</returns>
        /// Disable warning CA1502 because it will affect the implementation of Adapter and Model codes 
        /// if do any changes about maintainability.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", 
            Justification = "affect the implementation of Adapter and Model codes")]
        public ErrorStatus RetrievePrivateData(
            int handleInput,
            ValidString name,
            string secretName,
            out EncryptedValue encryptedData)
        {
            _RPC_UNICODE_STRING[] SecretName = new _RPC_UNICODE_STRING[1];
            _RPC_UNICODE_STRING[] openSecretName = new _RPC_UNICODE_STRING[1];
            _RPC_UNICODE_STRING[] tempSecretName = new _RPC_UNICODE_STRING[1];
            _LSAPR_CR_CIPHER_VALUE?[] EncryptedData = new _LSAPR_CR_CIPHER_VALUE?[1];
            bool anonymousaccess = false;
            string strEncryptedMessage = string.Empty;
            string strEcryptedData = string.Empty;
            string isUsingRPCSecurity = string.Empty;
            string hardCodedKey = string.Empty;
            char[] tempName = new char[20];

            this.secretFlag = true;
            byte[] byteKey = new byte[16];

            _RPC_UNICODE_STRING[] input = new _RPC_UNICODE_STRING[1];
            _RPC_UNICODE_STRING output = new _RPC_UNICODE_STRING();
            IntPtr? PolicyHandle2 = IntPtr.Zero;

            if (isUsingRPCSecurity == "true")
            {
                hardCodedKey = HardCodedKey;
                byteKey = Encoding.ASCII.GetBytes(hardCodedKey);
            }
            else
            {
                byteKey = lsadAdapter.SessionKey;
            }

            strEcryptedData = InputEncryptedMessage;

            bool bLsarRetrievePrivateDataIsSupported = LsarRetrievePrivateDataIsSupported;

            if (name == ValidString.Invalid)
            {
                this.utilities.nameOfSecretObject(ResOfNameChecked.Valid, ref SecretName);
                SecretName[0].Length = (ushort)((2 * SecretName[0].Buffer.Length) + 1);
                SecretName[0].MaximumLength = (ushort)(SecretName[0].Length + 2);
            }
            else if (stPolicyInformation.PHandle != handleInput)
            {
                this.utilities.nameOfSecretObject(ResOfNameChecked.Valid, ref SecretName);
                this.utilities.inValidHandle();
            }
            else if (stSecretInformation.strNameOfSecretObject != secretName)
            {
                tempName = secretName.ToCharArray();
                openSecretName[0].Buffer = new ushort[tempName.Length];
                Array.Copy(tempName, openSecretName[0].Buffer, tempName.Length);
                openSecretName[0].Length = (ushort)(2 * openSecretName[0].Buffer.Length);
                openSecretName[0].MaximumLength = (ushort)(openSecretName[0].Length + 2);
                this.utilities.nameOfSecretObject(ResOfNameChecked.Valid, ref SecretName);

                tempSecretName = SecretName;
                SecretName = openSecretName;
            }
            else if ((uintAccessMask & ACCESS_MASK.SECRET_QUERY_VALUE) != ACCESS_MASK.SECRET_QUERY_VALUE)
            {
                this.utilities.nameOfSecretObject(ResOfNameChecked.Valid, ref SecretName);
                this.utilities.tempPolicyHandle = PolicyHandle;
                lsadClientStack.LsarOpenPolicy(
                    serverName,
                    objectAttributes,
                    uintAccessMask,
                    out PolicyHandle);
            }
            else
            {
                this.utilities.nameOfSecretObject(ResOfNameChecked.Valid, ref SecretName);
            }

            uintMethodStatus = lsadClientStack.LsarRetrievePrivateData(
                PolicyHandle.Value,
                SecretName[0],
                ref EncryptedData[0]);

            if (uintMethodStatus == 0 && (EncryptedData == null))
            {
                encryptedData = EncryptedValue.Valid;
            }
            else
            {
                encryptedData = (EncryptedData[0] == null)
                    ? EncryptedValue.Invalid : EncryptedValue.Valid;
            }

            if (!(name == ValidString.Invalid))
            {
                if (stPolicyInformation.PHandle != handleInput)
                {
                    #region MS-LSAD_R578

                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)ErrorStatus.InvalidHandle,
                        (uint)uintMethodStatus,
                        "MS-LSAD",
                        578,
                        @"In LsarRetrievePrivateData ,If PolicyHandle is not a valid context handle to policy object, 
                        then the server MUST return STATUS_INVALID_HANDL");

                    #endregion

                    lsadClientStack.LsarDeleteObject(ref PolicyHandle);
                    PolicyHandle = this.utilities.tempPolicyHandle;
                    objSecretHandle = this.utilities.tempSecretHandle;
                }
                else if (stSecretInformation.strNameOfSecretObject != secretName)
                {
                    if ((stSecretInformation.strNameOfSecretObject != secretName) || anonymousaccess == true)
                    {
                        #region MS-LSAD_R579

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.ObjectNameNotFound,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            579,
                            @"In KeyName parameter of LsarRetrievePrivateData method ,If the requester is 
                            anonymous and the setting (Boolean setting that affects the processing of certain 
                            messages in LSAD protocol when the requester is anonymous) is set to true, the call 
                            MUST fail with STATUS_OBJECT_NAME_NOT_FOUND.");

                        #endregion

                        #region MS-LSAD_R582

                        Site.CaptureRequirementIfAreEqual<uint>(
                            (uint)ErrorStatus.ObjectNameNotFound,
                            (uint)uintMethodStatus,
                            "MS-LSAD",
                            582,
                            @"In KeyName parameter of LsarRetrievePrivateData method,If a secret object by this name 
                            does not exist, the server MUST return STATUS_OBJECT_NAME_NOT_FOUND.");

                        #endregion
                    }

                    SecretName = tempSecretName;
                }
                else if ((uintAccessMask & ACCESS_MASK.SECRET_QUERY_VALUE) != ACCESS_MASK.SECRET_QUERY_VALUE)
                {
                    encryptedData = EncryptedValue.Invalid;
                    uintMethodStatus = (NtStatus)ErrorStatus.AccessDenied;
                }
                else
                {
                    lsadClientStack.LsarOpenPolicy2(
                        "name",
                        objectAttributes,
                        ACCESS_MASK.DELETE,
                        out PolicyHandle2);

                    input[0] = new _RPC_UNICODE_STRING();
                    input[0].Buffer = new ushort[(int)EncryptedData[0].Value.Length];
                    if (EncryptedData[0] != null)
                    {
                        Buffer.BlockCopy(
                            EncryptedData[0].Value.Buffer, 
                            0, 
                            input[0].Buffer, 
                            0, 
                            (int)EncryptedData[0].Value.Length);
                        input[0].Length = (ushort)EncryptedData[0].Value.Length;
                        input[0].MaximumLength = (ushort)EncryptedData[0].Value.MaximumLength;

                        LsaUtility.DecryptSecret(input[0], byteKey, out output);

                        strEncryptedMessage = DtypUtility.ToString(output);

                        if (strEncryptedMessage.Contains(strEcryptedData))
                        {
                            #region MS-LSAD_R581

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.Success,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                581,
                                @"In LsarRetrievePrivateData method, if request is  successfully completed then 
                                server MUST return STATUS_SUCCESS.");

                            #endregion
                        }

                        string IsR401042Implemented = R401042Implementation;

                        if (isWindows)
                        {
                            if (PDCOSVersion == ServerVersion.Win2008
                                    || PDCOSVersion == ServerVersion.Win2003)
                            {
                                #region MS-LSAD_R141043

                                Site.CaptureRequirementIfAreEqual<uint>(
                                    (uint)ErrorStatus.Success,
                                    (uint)uintMethodStatus,
                                    "MS-LSAD",
                                    141043,
                                    @"[<62> Section 3.1.4.6:] Windows 2000 Server, Windows XP, Windows Server 2003, 
                                    Windows Vista, and Windows Server 2008 support method LsarRetrievePrivateData.");

                                #endregion
                            }

                            if (PDCOSVersion >= ServerVersion.Win2008R2)
                            {
                                if (bLsarRetrievePrivateDataIsSupported)
                                {
                                    #region MS-LSAD_R181043

                                    Site.CaptureRequirementIfAreEqual<uint>(
                                        (uint)ErrorStatus.Success,
                                        (uint)uintMethodStatus,
                                        "MS-LSAD",
                                        181043,
                                        @"[<62> Section 3.1.4.6:] Windows 7 and Windows Server 2008 R2 support method
                                        LsarRetrievePrivateData by default.");

                                    #endregion
                                }
                            }

                            if (IsR401042Implemented == null)
                            {
                                Site.Properties.Add("IsR401042Implemented", bool.TrueString);
                                IsR401042Implemented = bool.TrueString;
                            }
                        }
                        else
                        {
                            if (IsR401042Implemented != null)
                            {
                                bool implSigns = bool.Parse(IsR401042Implemented);
                                if (implSigns)
                                {
                                    bool isSatisfied = ((uint)ErrorStatus.Success == (uint)uintMethodStatus);

                                    #region MS-LSAD_R401042

                                    Site.CaptureRequirementIfAreEqual<bool>(
                                        implSigns,
                                        isSatisfied,
                                        "MS-LSAD",
                                        401042,
                                        @"The server MAY NOT support the LsarRetrievePrivateData method.<62> ");

                                    #endregion
                                }
                            }
                        }

                        if (bLsarRetrievePrivateDataIsSupported)
                        {
                            #region MS-LSAD_R581

                            Site.CaptureRequirementIfAreEqual<uint>(
                                (uint)ErrorStatus.Success,
                                (uint)uintMethodStatus,
                                "MS-LSAD",
                                581,
                                @"In LsarRetrievePrivateData method, if request is  successfully completed then 
                                server MUST return STATUS_SUCCESS.");

                            #endregion
                        }
                    }
                }
            }

            if (bLsarRetrievePrivateDataIsSupported && (uint)ErrorStatus.Success == (uint)uintMethodStatus)
            {
                ////if LsarRetrievePrivateDataIfIsSupported is true, it means the server performs the operations 
                ////in the message processing section for this method.
                #region MS-LSAD_R131045

                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)ErrorStatus.Success,
                    (uint)uintMethodStatus,
                    "MS-LSAD",
                    131045,
                    @"If the server supports method LsarRetrievePrivateData, the server MUST perform the operations 
                    in the message processing section for this method.");

                #endregion
            }

            return (ErrorStatus)uintMethodStatus;
        }

        #endregion LsarRetrievePrivateData

        #endregion SecretObect
    }
}
