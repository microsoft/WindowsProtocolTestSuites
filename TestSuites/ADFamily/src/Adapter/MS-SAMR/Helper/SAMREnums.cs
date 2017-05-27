// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Samr
{
    /// <summary>
    /// Access control that can appear in the Mask field of an ACE or in methods to obtain a handle.
    /// </summary>
    public enum Common_ACCESS_MASK: uint
    {
        /// <summary>
        /// Specifies the ability to delete the object.
        /// </summary>
        DELETE = 0x00010000,

        /// <summary>
        /// Specifies the ability to read the security descriptor.
        /// </summary>
        READ_CONTROL = 0x00020000,

        /// <summary>
        /// Specifies the ability to update the discretionary access control list (DACL) of the security descriptor.
        /// </summary>
        WRITE_DAC = 0x00040000,

        /// <summary>
        /// Specifies the ability to update the Owner field of the security descriptor.
        /// </summary>
        WRITE_OWNER = 0x00080000,

        /// <summary>
        /// Specifies access to the system security portion of the security descriptor.
        /// </summary>
        ACCESS_SYSTEM_SECURITY = 0x01000000,

        /// <summary>
        /// Indicates that the caller is requesting the most access possible to the object.
        /// </summary>
        MAXIMUM_ALLOWED = 0x02000000
    }

    /// <summary>
    /// These values appear in methods that are used to obtain a handle. 
    /// They are translated by the server into specific ACCESS_MASK values. 
    /// </summary>
    public enum Generic_ACCESS_MASK : uint
    {
        /// <summary>
        /// Specifies access control suitable for reading the object.
        /// </summary>
        GENERIC_READ = 0x80000000,

        /// <summary>
        /// Specifies access control suitable for updating attributes on the object.
        /// </summary>
        GENERIC_WRITE = 0x40000000,

        /// <summary>
        /// Specifies access control suitable for executing an action on the object.
        /// </summary>
        GENERIC_EXECUTE = 0x20000000,

        /// <summary>
        /// Specifies all defined access control on the object.
        /// </summary>
        GENERIC_ALL = 0x10000000
    }
    /// <summary>
    /// These are the specific values available to describe the access control on a user object. 
    /// A bitwise OR operation can be performed on these values, along with values in Common_ACCESS_MASK. 
    /// </summary>
    public enum User_ACCESS_MASK : uint
    {
        /// <summary>
        /// Specifies an invalid user access mask
        /// </summary>
        USER_INVALID_ACCESS = 0x00000000,

        /// <summary>
        /// Specifies the ability to read sundry attributes.
        /// </summary>
        USER_READ_GENERAL = 0x00000001,

        /// <summary>
        /// Specifies the ability to read general information attributes.
        /// </summary>
        USER_READ_PREFERENCES = 0x00000002,

        /// <summary>
        /// Specifies the ability to write general information attributes.
        /// </summary>
        USER_WRITE_PREFERENCES = 0x00000004,

        /// <summary>
        /// Specifies the ability to read attributes related to logon statistics.
        /// </summary>
        USER_READ_LOGON = 0x00000008,

        /// <summary>
        /// Specifies the ability to read attributes related to the administration of the user object.
        /// </summary>
        USER_READ_ACCOUNT = 0x00000010,

        /// <summary>
        /// Specifies the ability to write attributes related to the administration of the user object.
        /// </summary>
        USER_WRITE_ACCOUNT = 0x00000020,

        /// <summary>
        /// Specifies the ability to change the user's password.
        /// </summary>
        USER_CHANGE_PASSWORD = 0x00000040,

        /// <summary>
        /// Specifies the ability to set the user's password.
        /// </summary>
        USER_FORCE_PASSWORD_CHANGE = 0x00000080,

        /// <summary>
        /// Specifies the ability to query the membership of the user object.
        /// </summary>
        USER_LIST_GROUPS = 0x00000100,

        /// <summary>
        /// Does not specify any access control.
        /// </summary>
        USER_READ_GROUP_INFORMATION = 0x00000200,

        /// <summary>
        /// Does not specify any access control.
        /// </summary>
        USER_WRITE_GROUP_INFORMATION = 0x00000400,

        /// <summary>
        /// The specified accesses for a GENERIC_ALL request.
        /// </summary>
        USER_ALL_ACCESS = 0x000F07FF,

        /// <summary>
        /// The specified accesses for a GENERIC_READ request.
        /// </summary>
        USER_READ = 0x0002031A,

        /// <summary>
        /// The specified accesses for a GENERIC_WRITE request.
        /// </summary>
        USER_WRITE = 0x00020044,

        /// <summary>
        /// The specified accesses for a GENERIC_EXECUTE request.
        /// </summary>
        USER_EXECUTE = 0x00020041
    }

    /// <summary>
    /// These are the specific values available to describe the access control on a group object.
    /// </summary>
    public enum Group_ACCESS_MASK : uint
    {
        /// <summary>
        /// Specifies the ability to read various attributes.
        /// </summary>
        GROUP_READ_INFORMATION = 0x00000001,

        /// <summary>
        /// Specifies the ability to write various attributes, not including the member attribute.
        /// </summary>
        GROUP_WRITE_ACCOUNT = 0x00000002,

        /// <summary>
        /// Specifies the ability to add a value to the member attribute.
        /// </summary>
        GROUP_ADD_MEMBER = 0X00000004,

        /// <summary>
        /// Specifies the ability to remove a value from the member attribute.
        /// </summary>
        GROUP_REMOVE_MEMBER = 0x00000008,

        /// <summary>
        /// Specifies the ability to read the values of the member attribute.
        /// </summary>
        GROUP_LIST_MEMBERS = 0x00000010,

        /// <summary>
        /// The specified accesses for a GENERIC_ALL request.
        /// </summary>
        GROUP_ALL_ACCESS = 0x000F001F,

        /// <summary>
        /// The specified accesses for a GENERIC_READ request.
        /// </summary>
        GROUP_READ = 0x00020010,

        /// <summary>
        /// The specified accesses for a GENERIC_WRITE request.
        /// </summary>
        GROUP_WRITE = 0x0002000E,

        /// <summary>
        /// The specified accesses for a GENERIC_EXECUTE request.
        /// </summary>
        GROUP_EXECUTE = 0x00020001
    }
    
    /// <summary>
    /// This enum represent status of the Entries returned from the server.
    /// </summary>
    public enum ENTRIES
    {
        /// <summary>
        /// Specify that few more entries are present in the server, which are not returned in the present response.
        /// </summary>
        MORE,

        /// <summary>
        /// Specify the new added object which satisfy Enumerate-Filter is returned.
        /// </summary>
        NewAddedObjectReturned,

        /// <summary>
        /// Specify the deleted object which satisfy Enumerate-Filter is not returned.
        /// </summary>
        DeletedObjectNotReturned,

        /// <summary>
        /// Specify that all the entries present in the server have been returned in the present response.
        /// </summary>
        ALL
    }

    /// <summary>
    /// This enum represent several error codes that are returned.
    /// </summary>
    public enum HRESULT : uint
    {
        /// <summary>
        /// Specify that requested operation completed successfully.
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// Specify that requested operation doesn't complete successfully.
        /// </summary>
        STATUS_ERROR = 0xFFFFF, 

        /// <summary>
        /// Specify that requester has requested access to an object, but has not 
        /// been granted those access rights.
        /// </summary>
        STATUS_ACCESS_DENIED = 0xC0000022,

        /// <summary>
        /// Specify that more information is available.
        /// </summary>
        STATUS_MORE_ENTRIES = 0x00000105,

        /// <summary>
        /// Specify that some of the information need to be translated but has not
        /// been translated.
        /// </summary>
        STATUS_SOME_NOT_MAPPED = 0x00000107,

        /// <summary>
        /// Specify that none of the information to be translated has been 
        /// translated.
        /// </summary>
        STATUS_NONE_MAPPED = 0xC0000073,

        /// <summary>
        /// Specify that the value provided as the current password is not correct
        /// when trying to update the password.
        /// </summary>
        STATUS_WRONG_PASSWORD = 0xC000006A,

        /// <summary>
        /// Specify that the user account has been automatically locked because too
        /// many invalid logon attempts or password change attempts have been 
        /// requested.
        /// </summary>
        STATUS_ACCOUNT_LOCKED_OUT = 0xC0000234,

        /// <summary>
        /// Specify that group already exists.
        /// </summary>
        STATUS_GROUP_EXISTS = 0xC0000065,

        /// <summary>
        /// Specify that user already exists.
        /// </summary>
        STATUS_USER_EXISTS = 0xC0000063,

        /// <summary>
        /// Specify that alias already exists.
        /// </summary>
        STATUS_ALIAS_EXISTS = 0xC0000154,

        /// <summary>
        /// Specify that the operation is not supported.
        /// </summary>
        STATUS_NOT_SUPPORTED = 0xC00000BB,

        /// <summary>
        /// Specify that specified account does not exist.
        /// </summary>
        STATUS_NO_SUCH_USER = 0xC0000064,

        /// <summary>
        /// Specify that specified domain does not exist.
        /// </summary>
        STATUS_NO_SUCH_DOMAIN = 0xC00000DF,

        /// <summary>
        /// Specify that specified group does not exist.
        /// </summary>
        STATUS_NO_SUCH_GROUP = 0xC0000066,

        /// <summary>
        /// Specify that specified alias does not exist.
        /// </summary>
        STATUS_NO_SUCH_ALIAS = 0xC0000151,

        /// <summary>
        /// Returned by enumeration methods to indicate that no more information is available.
        /// </summary>
        STATUS_NO_MORE_ENTRIES = 0x8000001A,

        /// <summary>
        /// Specify that requester should retry the request using the current 
        /// password LM hash as an encryption key.
        /// </summary>
        STATUS_LM_CROSS_ENCRYPTION_REQUIRED = 0xC000017F,

        /// <summary>
        /// Specify that the domain prefix of the objectSid attribute of any object in set M is 
        /// different from the domain prefix of G's objectSid.
        /// </summary>
        STATUS_DS_GLOBAL_CANT_HAVE_CROSSDOMAIN_MEMBER = 0xC00002DA,

        /// <summary>
        /// Specify that requester should retry the request using the current 
        /// password NT hash as an encryption key.
        /// </summary>
        STATUS_NT_CROSS_ENCRYPTION_REQUIRED = 0xC000015D,

        /// <summary>
        /// Specify that requester should retry the request using the current 
        /// password NT hash as an encryption key.
        /// </summary>
        RPC_S_PROTSEQ_NOT_SUPPORTED = 0x000006A7,

        /// <summary>
        /// Specify that the request is rejected.
        /// </summary>
        RPC_S_ACCESS_DENIED = 0x00000005,
    }

    /// <summary>
    /// This enum represent USER_ALL values.
    /// </summary>
    public enum USER_ALLValue : uint
    {
        /// <summary>
        /// Specify that the bit is Zero.
        /// </summary>
        Zero,

        /// <summary>
        /// Specify the name of the user account.
        /// </summary>
        USER_ALL_USERNAME = 0x00000001,

        /// <summary>
        /// Specify the full name of the user account.
        /// </summary>
        USER_ALL_FULLNAME = 0x00000002,

        /// <summary>
        /// Specify the ID of the user account.
        /// </summary>
        USER_ALL_USERID = 0x00000004,

        /// <summary>
        /// Specify the PrimaryGroupId of the user account.
        /// </summary>
        USER_ALL_PRIMARYGROUPID = 0x00000008,

        /// <summary>
        /// Specify the AdminComment of the user account.
        /// </summary>
        USER_ALL_ADMINCOMMENT = 0x00000010,

        /// <summary>
        /// Specify the UserComment of the user account.
        /// </summary>
        USER_ALL_USERCOMMENT = 0x00000020,

        /// <summary>
        /// Specify the HomeDirectory of the user account.
        /// </summary>
        USER_ALL_HOMEDIRECTORY = 0x00000040,

        /// <summary>
        /// Specify the HomeDirectoryDrive of the user account.
        /// </summary>
        USER_ALL_HOMEDIRECTORYDRIVE = 0x00000080,

        /// <summary>
        /// Specify the ScriptPath of the user account.
        /// </summary>
        USER_ALL_SCRIPTPATH = 0x00000100,

        /// <summary>
        /// Specify the ProfilePath of the user account.
        /// </summary>
        USER_ALL_PROFILEPATH = 0x00000200,

        /// <summary>
        /// Specify the WorkStations of the user account.
        /// </summary>
        USER_ALL_WORKSTATIONS = 0x00000400,

        /// <summary>
        /// Specify the LastLogon of the user account.
        /// </summary>
        USER_ALL_LASTLOGON = 0x00000800,

        /// <summary>
        /// Specify the LastLogoff of the user account.
        /// </summary>
        USER_ALL_LASTLOGOFF = 0x00001000,

        /// <summary>
        /// Specify the LogonHours of the user account.
        /// </summary>
        USER_ALL_LOGONHOURS = 0x00002000,

        /// <summary>
        /// Specify the BadPasswordCount of the user account.
        /// </summary>
        USER_ALL_BADPASSWORDCOUNT = 0x00004000,

        /// <summary>
        /// Specify the LogonCount of the user account.
        /// </summary>
        USER_ALL_LOGONCOUNT = 0x00008000,

        /// <summary>
        /// Specify the PasswordCanChange for the user account.
        /// </summary>
        USER_ALL_PASSWORDCANCHANGE = 0x00010000,

        /// <summary>
        /// Specify the PasswordMustChange for the user account.
        /// </summary>
        USER_ALL_PASSWORDMUSTCHANGE = 0x00020000,

        /// <summary>
        /// Specify the PasswordLastSet for the user account.
        /// </summary>
        USER_ALL_PASSWORDLASTSET = 0x00040000,

        /// <summary>
        /// Specify the AccountExpires of the user account.
        /// </summary>
        USER_ALL_ACCOUNTEXPIRES = 0x00080000,

        /// <summary>
        /// Specify the UserAccountControl of the user account.
        /// </summary>
        USER_ALL_USERACCOUNTCONTROL = 0x00100000,

        /// <summary>
        /// Specify the Parameters of the user account.
        /// </summary>
        USER_ALL_PARAMETERS = 0x00200000,

        /// <summary>
        /// Specify the CountryCode of the user account.
        /// </summary>
        USER_ALL_COUNTRYCODE = 0x00400000,

        /// <summary>
        /// Specify the CodePage of the user account.
        /// </summary>
        USER_ALL_CODEPAGE = 0x00800000,

        /// <summary>
        /// Specify the NtPasswordPresent of the user account.
        /// </summary>
        USER_ALL_NTPASSWORDPRESENT = 0x01000000,

        /// <summary>
        /// Specify the LmPasswordPresent of the user account.
        /// </summary>
        USER_ALL_LMPASSWORDPRESENT = 0x02000000,

        /// <summary>
        /// Specify the PasswordExpired for the user account.
        /// </summary>
        USER_ALL_PASSWORDEXPIRED = 0x08000000,

        /// <summary>
        /// Specify that the bit represent Undefined mask value.
        /// </summary>
        USER_ALL_UNDEFINED_MASK,

        /// <summary>
        /// Specify the SecurityDescriptor.
        /// </summary>
        USER_ALL_SECURITYDESCRIPTOR = 0x10000000,

        /// <summary>
        /// Specify that the bit is PrivateData.
        /// </summary>
        USER_ALL_PRIVATEDATA=0x04000000,

        /// <summary>
        /// Specify the user read preferences.
        /// </summary>
        USER_READ_PREFERENCES = 0x00000002
    }

    /// <summary>
    /// This enum represent the type of the user account.
    /// </summary>
    public enum ACCOUNT_TYPE : uint
    {
        /// <summary>
        /// Specify that the user is not a valid account type.
        /// </summary>
        USER_INVALID = 0x00000000,

        /// <summary>
        /// Specify that the user is not a computer object.
        /// </summary>
        USER_NORMAL_ACCOUNT = 0x00000010,

        /// <summary>
        /// Specify that the object is a member workstation or server.
        /// </summary>
        USER_WORKSTATION_TRUST_ACCOUNT = 0x00000080,

        /// <summary>
        /// Specify that the object is a Domain Controller.
        /// </summary>
        USER_SERVER_TRUST_ACCOUNT = 0x00000100
    }

    /// <summary>
    /// This enum indicate the possible information levels of the Group object that 
    /// are require to update or read.
    /// </summary>
    public enum GROUP_INFORMATION_CLASS
    {
        /// <summary>
        /// Specify the general information about the group like account info.
        /// </summary>
        GroupGeneralInformation = 1,

        /// <summary>
        /// Specify the name information of the group.
        /// </summary>
        GroupNameInformation = 2,

        /// <summary>
        /// Specify the attribute information of the group.
        /// </summary>
        GroupAttributeInformation = 3,

        /// <summary>
        /// Specify the admin comment information of the group.
        /// </summary>
        GroupAdminCommentInformation = 4,

        /// <summary>
        /// Specify the replication information of the group.
        /// </summary>
        GroupReplicationInformation = 5
    }

    /// <summary>
    /// This enum indicate the possible information levels of the Alias object that 
    /// are require to update or read.
    /// </summary>
    public enum ALIAS_INFORMATION_CLASS
    {

        /// <summary>
        /// Specify the general information about the alias like account information.
        /// </summary>
        AliasGeneralInformation = 1,

        /// <summary>
        /// Specify the name information of the alias.
        /// </summary>
        AliasNameInformation,

        /// <summary>
        /// Specify the admin comment information of the alias.
        /// </summary>
        AliasAdminCommentInformation
    }

    /// <summary>
    /// This enum indicate the possible information levels of the Domain object 
    /// that are require to update or read.
    /// </summary>
    public enum DOMAIN_INFORMATION_CLASS
    {
        /// <summary>
        ///  DomainPasswordInformation constant.
        /// </summary>
        DomainPasswordInformation = 1,

        /// <summary>
        ///  DomainGeneralInformation constant.
        /// </summary>
        DomainGeneralInformation = 2,

        /// <summary>
        ///  DomainLogoffInformation constant.
        /// </summary>
        DomainLogoffInformation = 3,

        /// <summary>
        ///  DomainOemInformation constant.
        /// </summary>
        DomainOemInformation = 4,

        /// <summary>
        ///  DomainNameInformation constant.
        /// </summary>
        DomainNameInformation = 5,

        /// <summary>
        ///  DomainReplicationInformation constant.
        /// </summary>
        DomainReplicationInformation = 6,

        /// <summary>
        ///  DomainServerRoleInformation constant.
        /// </summary>
        DomainServerRoleInformation = 7,

        /// <summary>
        ///  DomainModifiedInformation constant.
        /// </summary>
        DomainModifiedInformation = 8,

        /// <summary>
        ///  DomainStateInformation constant.
        /// </summary>
        DomainStateInformation = 9,

        /// <summary>
        ///  DomainGeneralInformation2 constant.
        /// </summary>
        DomainGeneralInformation2 = 11,

        /// <summary>
        ///  DomainLockoutInformation constant.
        /// </summary>
        DomainLockoutInformation = 12,

        /// <summary>
        ///  DomainModifiedInformation2 constant.
        /// </summary>
        DomainModifiedInformation2 = 13,

        /// <summary>
        /// DomainInformation.Password.MaxPasswordAge is not a valid delta time.
        /// </summary>
        MaxPasswordAgeInvalid = 14,

        /// <summary>
        /// DomainInformation.Password.MinPasswordAge is not a valid delta time.
        /// </summary>
        MinPasswordAgeInvalid = 15,

        /// <summary>
        /// DomainInformation.Password.MaxPasswordAge is less than or equal to 
        /// DomainInformation.Password.MinPasswordAge.
        /// </summary>
        MaxPasswordAgeLessThanMinPasswordAge =16,

        /// <summary>
        /// DomainInformation.Password.MinPasswordLength is greater than 1024.
        /// </summary>
        MinPasswordLengthGreaterThan1024 = 17,

        /// <summary>
        /// Set DomainServerState to DomainServerEnabled.
        /// </summary>
        DomainServerEnabled =18,

        /// <summary>
        /// Set DomainServerState to DomainServerDisabled.
        /// </summary>
        DomainServerDisabled = 19,

        /// <summary>
        /// pwdHistoryLength is not less than 1024.
        /// </summary>
        PwdHistoryLengthNotLessThan1024 =20
    }

    /// <summary>
    /// This enum indicate the possible information levels of user object that are 
    /// require to update or read.
    /// </summary>
    public enum USER_INFORMATION_CLASS
    {
        /// <summary>
        ///  UserGeneralInformation constant.
        /// </summary>
        UserGeneralInformation = 1,

        /// <summary>
        ///  UserPreferencesInformation constant.
        /// </summary>
        UserPreferencesInformation = 2,

        /// <summary>
        ///  UserLogonInformation constant.
        /// </summary>
        UserLogonInformation = 3,

        /// <summary>
        ///  UserLogonHoursInformation constant.
        /// </summary>
        UserLogonHoursInformation = 4,

        /// <summary>
        ///  UserAccountInformation constant.
        /// </summary>
        UserAccountInformation = 5,

        /// <summary>
        ///  UserNameInformation constant.
        /// </summary>
        UserNameInformation = 6,

        /// <summary>
        ///  UserAccountNameInformation constant.
        /// </summary>
        UserAccountNameInformation = 7,

        /// <summary>
        ///  UserFullNameInformation constant.
        /// </summary>
        UserFullNameInformation = 8,

        /// <summary>
        ///  UserPrimaryGroupInformation constant.
        /// </summary>
        UserPrimaryGroupInformation = 9,

        /// <summary>
        ///  UserHomeInformation constant.
        /// </summary>
        UserHomeInformation = 10,

        /// <summary>
        ///  UserScriptInformation constant.
        /// </summary>
        UserScriptInformation = 11,

        /// <summary>
        ///  UserProfileInformation constant.
        /// </summary>
        UserProfileInformation = 12,

        /// <summary>
        ///  UserAdminCommentInformation constant.
        /// </summary>
        UserAdminCommentInformation = 13,

        /// <summary>
        ///  UserWorkStationsInformation constant.
        /// </summary>
        UserWorkStationsInformation = 14,

        /// <summary>
        ///  UserControlInformation constant.
        /// </summary>
        UserControlInformation = 16,

        /// <summary>
        ///  UserExpiresInformation constant.
        /// </summary>
        UserExpiresInformation = 17,

        /// <summary>
        ///  UserInternal1Information constant.
        /// </summary>
        UserInternal1Information = 18,

        /// <summary>
        ///  UserParametersInformation constant.
        /// </summary>
        UserParametersInformation = 20,

        /// <summary>
        ///  UserAllInformation constant.
        /// </summary>
        UserAllInformation = 21,

        /// <summary>
        ///  UserInternal4Information constant.
        /// </summary>
        UserInternal4Information = 23,

        /// <summary>
        ///  UserInternal5Information constant.
        /// </summary>
        UserInternal5Information = 24,

        /// <summary>
        ///  UserInternal4InformationNew constant.
        /// </summary>
        UserInternal4InformationNew = 25,

        /// <summary>
        ///  UserInternal5InformationNew constant.
        /// </summary>
        UserInternal5InformationNew = 26,
    }

    /// <summary>
    /// This enum Indicate the fields of SecurityDescriptor that are request to be set.
    /// </summary>
    public enum SecurityInformation
    {
        /// <summary>
        /// Refer to invalid information level.
        /// </summary>
        INVALID_SECURITY_INFORMATION = 0x00000010,

        /// <summary>
        ///  Refer to the Owner member of the security descriptor.
        /// </summary>
        OWNER_SECURITY_INFORMATION = 0x00000001,

        /// <summary>
        ///  Refer to the Group member of the security descriptor.
        /// </summary>
        GROUP_SECURITY_INFORMATION = 0x00000002,

        /// <summary>
        ///  Refer to the DACL of the security descriptor.
        /// </summary>
        DACL_SECURITY_INFORMATION = 0x00000004,

        /// <summary>
        ///  Refer to the SACL of the security descriptor.
        /// </summary>
        SACL_SECURITY_INFORMATION = 0x00000008,
    }

    /// <summary>
    /// This enum represent Indicate the type of accounts, as well as the type of 
    /// attributes on the accounts.
    /// </summary>
    public enum DOMAIN_DISPLAY_INFORMATION
    {
        /// <summary>
        /// Refer to the user object.
        /// </summary>
        DomainDisplayUser = 1,

        /// <summary>
        /// Refer to the member workstation or server.
        /// </summary>
        DomainDisplayMachine,

        /// <summary>
        /// Refer to the group object.
        /// </summary>
        DomainDisplayGroup,

        /// <summary>
        /// Refer to the OemUser object.
        /// </summary>
        DomainDisplayOemUser,

        /// <summary>
        /// Refer to the OemGroup object.
        /// </summary>
        DomainDisplayOemGroup
    }

    /// <summary>
    /// This enum represent the validation type for SamrValidatePassword.
    /// </summary>
    public enum VALIDATION_TYPE
    {

        /// <summary>
        /// Refer to the password authentication Request.
        /// </summary>
        SamValidateAuthentication = 1,

        /// <summary>
        /// Refer to the password Change Request.
        /// </summary>
        SamValidatePasswordChange,

        /// <summary>
        /// Refer to the password Reset Request.
        /// </summary>
        SamValidatePasswordReset,
    }

    /// <summary>
    /// This enum represent the type of the attribute.
    /// </summary>
    public enum AttributeType
    {
        /// <summary>
        /// Refer to Lmowf type.
        /// </summary>
        Lmowf,

        /// <summary>
        /// Refer to Ntowf type.
        /// </summary>
        Ntowf,

        /// <summary>
        /// Refer to Lmowf and Ntowf types.
        /// </summary>
        Both
    }

    /// <summary>
    /// This represent the password last set value.
    /// </summary>
    public enum PasswordLastSet
    {
        /// <summary>
        /// Zero.
        /// </summary>
        ZERO,

        /// <summary>
        /// Add to be lesser than current time.
        /// </summary>
        TOLESSERTHANCURRENTTIME,

        /// <summary>
        /// Add to be greater than current time.
        /// </summary>
        TOGREATERTHANCURRENTTIME
    }

    /// <summary>
    /// Represent the time at which the owner of the password data was locked out.
    /// </summary>
    public enum LockOutTime
    {
        /// <summary>
        /// Add to be lesser than current time.
        /// </summary>
        TOLESSERTHANCURRENTTIME,

        /// <summary>
        /// Add to be greater than current time.
        /// </summary>
        TOGREATERTHANCURRENTTIME
    }

    /// <summary>
    /// Represent the time at which an invalid password was presented to either a password
    /// change request or an authentication request.
    /// </summary>
    public enum BadPasswordTime
    {
        /// <summary>
        /// Add to be lesser than current time.
        /// </summary>
        TOLESSERTHANCURRENTTIME,

        /// <summary>
        /// Add to be greater than current time.
        /// </summary>
        TOGREATERTHANCURRENTTIME
    }

    /// <summary>
    /// Protocol use Rpc over named pipe or TCP/IP protocol sequence.
    /// </summary>
    public enum ProtocolSequence
    {
        /// <summary>
        /// Protocol use Rpc over named pipe sequence.
        /// RPC over SMB, as specified in [MS-RPCE] section 2.1.1.2.
        /// This protocol uses the pipe name "\PIPE\samr" for the endpoint name.
        /// </summary>
        RpcOverNamedPipe,

        /// <summary>
        /// Protocol use Rpc over TCP/IP sequence.
        /// RPC over TCP
        /// This protocol uses RPC dynamic endpoints, as specified in [C706] section 6.
        /// </summary>
        RpcOverTcp
    };
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
