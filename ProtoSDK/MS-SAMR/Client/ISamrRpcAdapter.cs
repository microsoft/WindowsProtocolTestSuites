// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Samr
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
    using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;
    using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
    using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

    /// <summary>
    /// An enumeration to specify security group membership attributes
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum SE_GROUP : uint
    {
        /// <summary>
        /// The SID cannot have the SE_GROUP_ENABLED attribute removed
        /// </summary>
        SE_GROUP_MANDATORY = 0x00000001,
        /// <summary>
        /// The SID is enabled by default (rather than being added by an application)
        /// </summary>
        SE_GROUP_ENABLED_BY_DEFAULT = 0x00000002,
        /// <summary>
        /// The SID is enabled for access checks
        /// </summary>
        SE_GROUP_ENABLED = 0x00000004
    }

    /// <summary>
    /// An enumeration to specify user account control bits
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    public enum USER_ACCOUNT_CONTROL : uint
    {
        /// <summary>
        /// Specifies that the account is not enabled for authentication. 
        /// </summary>
        USER_ACCOUNT_DISABLED = 0x00000001,

        /// <summary>
        /// Specifies that the homeDirectory attribute is required.
        /// </summary>
        USER_HOME_DIRECTORY_REQUIRED = 0x00000002,

        /// <summary>
        /// Specifies that the password-length policy does not apply to this user. 
        /// </summary>
        USER_PASSWORD_NOT_REQUIRED = 0x00000004,

        /// <summary>
        /// This bit is ignored by clients and servers. 
        /// </summary>
        USER_TEMP_DUPLICATE_ACCOUNT = 0x00000008,

        /// <summary>
        /// Specifies that the user is not a computer object. 
        /// </summary>
        USER_NORMAL_ACCOUNT = 0x00000010,

        /// <summary>
        /// This bit is ignored by clients and servers. 
        /// </summary>
        USER_MNS_LOGON_ACCOUNT = 0x00000020,

        /// <summary>
        /// Specifies that the object represents a trust object. 
        /// For more information about trust objects, see [MS-LSAD]. 
        /// </summary>
        USER_INTERDOMAIN_TRUST_ACCOUNT = 0x00000040,

        /// <summary>
        /// Specifies that the object is a member workstation or server. 
        /// </summary>
        USER_WORKSTATION_TRUST_ACCOUNT = 0x00000080,

        /// <summary>
        /// Specifies that the object is a DC. 
        /// </summary>
        USER_SERVER_TRUST_ACCOUNT = 0x00000100,

        /// <summary>
        /// Specifies that the maximum-password-age policy does not apply to this user. 
        /// </summary>
        USER_DONT_EXPIRE_PASSWORD = 0x00000200,

        /// <summary>
        /// Specifies that the account has been locked out. 
        /// </summary>
        USER_ACCOUNT_AUTO_LOCKED = 0x00000400,

        /// <summary>
        /// Specifies that the clear text password is to be persisted. 
        /// </summary>
        USER_ENCRYPTED_TEXT_PASSWORD_ALLOWED = 0x00000800,

        /// <summary>
        /// Specifies that the user can authenticate only with a smart card. 
        /// </summary>
        USER_SMARTCARD_REQUIRED = 0x00001000,

        /// <summary>
        /// This bit is used by the Kerberos protocol. It indicates that the "OK as Delegate" ticket flag 
        /// (described in [RFC4120] section 2.8) MUST be set. 
        /// </summary>
        USER_TRUSTED_FOR_DELEGATION = 0x00002000,

        /// <summary>
        /// This bit is used by the Kerberos protocol. It indicates that the ticket-granting tickets (TGTs)
        /// of this account and the service tickets obtained by this account are not marked as forwardable 
        /// or proxiable when the forwardable or proxiable ticket flags are requested. 
        /// For more information, see [RFC4120].
        /// </summary>
        USER_NOT_DELEGATED = 0x00004000,

        /// <summary>
        /// This bit is used by the Kerberos protocol. It indicates that only des-cbc-md5 or des-cbc-crc 
        /// keys (as defined in [RFC3961]) are used in the Kerberos protocols for this account. 
        /// </summary>
        USER_USE_DES_KEY_ONLY = 0x00008000,

        /// <summary>
        /// This bit is used by the Kerberos protocol. It indicates that the account is not required to 
        /// present valid pre-authentication data, as described in [RFC4120] section 7.5.2. 
        /// </summary>
        USER_DONT_REQUIRE_PREAUTH = 0x00010000,

        /// <summary>
        /// Specifies that the password age on the user has exceeded the maximum password age policy. 
        /// </summary>
        USER_PASSWORD_EXPIRED = 0x00020000,

        /// <summary>
        /// This bit is used by the Kerberos protocol. When set, it indicates that the account (when running 
        /// as a service) obtains an S4U2self service ticket (as specified in [MS-SFU]) with the forwardable
        /// flag set. If this bit is cleared, the forwardable flag is not set in the S4U2self service ticket. 
        /// </summary>
        USER_TRUSTED_TO_AUTHENTICATE_FOR_DELEGATION = 0x00040000,

        /// <summary>
        /// This bit is used by the Kerberos protocol. It indicates that when the key distribution center 
        /// (KDC) is issuing a service ticket for this account, the privilege attribute certificate 
        /// (PAC) MUST NOT be included. For more information, see [RFC4120]. 
        /// </summary>
        USER_NO_AUTH_DATA_REQUIRED = 0x00080000,

        /// <summary>
        /// Specifies that the object is a read-only domain controller (RODC).
        /// </summary>
        USER_PARTIAL_SECRETS_ACCOUNT = 0x00100000,

        /// <summary>
        /// This bit is ignored by clients and servers.
        /// </summary>
        USER_USE_AES_KEYS = 0x00200000
    }

    /// <summary>
    /// An enumeration to specify common access masks
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    public enum COMMON_ACCESS_MASK : uint
    {
        /// <summary>
        /// Specifies the ability to delete the object
        /// </summary>
        DELETE = 0x00010000,
        /// <summary>
        /// Specifies the ability to read the security descriptor
        /// </summary>
        READ_CONTROL = 0x00020000,
        /// <summary>
        /// Specifies the ability to update the discretionary access control list (DACL) 
        /// of the security descriptor. 
        /// </summary>
        WRITE_DAC = 0x00040000,
        /// <summary>
        /// Specifies the ability to update the Owner field of the security descriptor
        /// </summary>
        WRITE_OWNER = 0x00080000,
        /// <summary>
        /// Specifies access to the system security portion of the security descriptor
        /// </summary>
        ACCESS_SYSTEM_SECURITY = 0x01000000,
        /// <summary>
        /// Indicates that the caller is requesting the most access possible to the object
        /// </summary>
        MAXIMUM_ALLOWED = 0x02000000
    }

    /// <summary>
    /// An enumeration to specify generic access masks
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    public enum GENERIC_ACCESS_MASK : uint
    {
        /// <summary>
        /// Specifies access control suitable for reading the object
        /// </summary>
        GENERIC_READ = 0x80000000,
        /// <summary>
        /// Specifies access control suitable for updating attributes on the object
        /// </summary>
        GENERIC_WRITE = 0x40000000,
        /// <summary>
        /// Specifies access control suitable for executing an action on the object
        /// </summary>
        GENERIC_EXECUTE = 0x20000000,
        /// <summary>
        /// Specifies all defined access control on the object
        /// </summary>
        GENERIC_ALL = 0x10000000
    }

    /// <summary>
    /// An enumeration to specify server access masks
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags")]
    [Flags()]
    public enum SERVER_ACCESS_MASK : uint
    {
        /// <summary>
        /// Specifies access control to obtain a server handle
        /// </summary>
        SAM_SERVER_CONNECT = 0x00000001,
        /// <summary>
        /// Does not specify any access control
        /// </summary>
        SAM_SERVER_SHUTDOWN = 0x00000002,
        /// <summary>
        /// Does not specify any access control
        /// </summary>
        SAM_SERVER_INITIALIZE = 0x00000004,
        /// <summary>
        /// Does not specify any access control
        /// </summary>
        SAM_SERVER_CREATE_DOMAIN = 0x00000008,
        /// <summary>
        /// Specifies access control to view domain objects
        /// </summary>
        SAM_SERVER_ENUMERATE_DOMAINS = 0x00000010,
        /// <summary>
        /// Specifies access control to perform SID-to-name translation
        /// </summary>
        SAM_SERVER_LOOKUP_DOMAIN = 0x00000020,
        /// <summary>
        /// The specified accesses for a GENERIC_ALL request
        /// </summary>
        SAM_SERVER_ALL_ACCESS = 0x000F003F,
        /// <summary>
        /// The specified accesses for a GENERIC_READ request
        /// </summary>
        SAM_SERVER_READ = 0x00020010,
        /// <summary>
        /// The specified accesses for a GENERIC_WRITE request
        /// </summary>
        SAM_SERVER_WRITE = 0x0002000E,
        /// <summary>
        /// The specified accesses for a GENERIC_EXECUTE request
        /// </summary>
        SAM_SERVER_EXECUTE = 0x00020021
    }

    /// <summary>
    /// An enumeration to specify domain access masks
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags")]
    [Flags()]
    public enum DOMAIN_ACCESS_MASK : uint
    {
        /// <summary>
        /// Specifies access control to read password policy
        /// </summary>
        DOMAIN_READ_PASSWORD_PARAMETERS = 0x00000001,
        /// <summary>
        /// Specifies access control to write password policy
        /// </summary>
        DOMAIN_WRITE_PASSWORD_PARAMS = 0x00000002,
        /// <summary>
        /// Specifies access control to read attributes not related to password policy
        /// </summary>
        DOMAIN_READ_OTHER_PARAMETERS = 0x00000004,
        /// <summary>
        /// Specifies access control to write attributes not related to password policy
        /// </summary>
        DOMAIN_WRITE_OTHER_PARAMETERS = 0x00000008,
        /// <summary>
        /// Specifies access control to create a user object
        /// </summary>
        DOMAIN_CREATE_USER = 0x00000010,
        /// <summary>
        /// Specifies access control to create a group object
        /// </summary>
        DOMAIN_CREATE_GROUP = 0x00000020,
        /// <summary>
        /// Specifies access control to create an alias object
        /// </summary>
        DOMAIN_CREATE_ALIAS = 0x00000040,
        /// <summary>
        /// Specifies access control to read the alias membership of a set of SIDs
        /// </summary>
        DOMAIN_GET_ALIAS_MEMBERSHIP = 0x00000080,
        /// <summary>
        /// Specifies access control to enumerate objects
        /// </summary>
        DOMAIN_LIST_ACCOUNTS = 0x00000100,
        /// <summary>
        /// Specifies access control to look up objects by name and SID
        /// </summary>
        DOMAIN_LOOKUP = 0x00000200,
        /// <summary>
        /// Specifies access control to various administrative operations on the server
        /// </summary>
        DOMAIN_ADMINISTER_SERVER = 0x00000400,
        /// <summary>
        /// The specified accesses for a GENERIC_ALL request
        /// </summary>
        DOMAIN_ALL_ACCESS = 0x000F07FF,
        /// <summary>
        /// The specified accesses for a GENERIC_READ request
        /// </summary>
        DOMAIN_READ = 0x00020084,
        /// <summary>
        /// The specified accesses for a GENERIC_WRITE request
        /// </summary>
        DOMAIN_ALL_WRITE = 0x0002047A,
        /// <summary>
        /// The specified accesses for a GENERIC_EXECUTE request
        /// </summary>
        DOMAIN_ALL_EXECUTE = 0x00020301
    }

    /// <summary>
    /// An enumeration to specify group access masks
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags")]
    [Flags()]
    public enum GROUP_ACCESS_MASK : uint
    {
        /// <summary>
        /// Specifies the ability to read various attributes
        /// </summary>
        GROUP_READ_INFORMATION = 0x00000001,
        /// <summary>
        /// Specifies the ability to write various attributes, not including the member attribute
        /// </summary>
        GROUP_WRITE_ACCOUNT = 0x00000002,
        /// <summary>
        /// Specifies the ability to add a value to the member attribute
        /// </summary>
        GROUP_ADD_MEMBER = 0x00000004,
        /// <summary>
        /// Specifies the ability to remove a value from the member attribute
        /// </summary>
        GROUP_REMOVE_MEMBER = 0x00000008,
        /// <summary>
        /// Specifies the ability to read the values of the member attribute
        /// </summary>
        GROUP_LIST_MEMBERS = 0x00000010,
        /// <summary>
        /// The specified accesses for a GENERIC_ALL request
        /// </summary>
        GROUP_ALL_ACCESS = 0x000F001F,
        /// <summary>
        /// The specified accesses for a GENERIC_READ request
        /// </summary>
        GROUP_READ = 0x00020010,
        /// <summary>
        /// The specified accesses for a GENERIC_WRITE request
        /// </summary>
        GROUP_WRITE = 0x0002000E,
        /// <summary>
        /// The specified accesses for a GENERIC_EXECUTE request
        /// </summary>
        GROUP_EXECUTE = 0x00020001
    }

    /// <summary>
    /// An enumeration to specify alias access masks
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags")]
    [Flags()]
    public enum ALIAS_ACCESS_MASK : uint
    {
        /// <summary>
        /// Specifies the ability to add a value to the member attribute
        /// </summary>
        ALIAS_ADD_MEMBER = 0x00000001,
        /// <summary>
        /// Specifies the ability to remove a value from the member attribute
        /// </summary>
        ALIAS_REMOVE_MEMBER = 0x00000002,
        /// <summary>
        /// Specifies the ability to read the member attribute
        /// </summary>
        ALIAS_LIST_MEMBERS = 0x00000004,
        /// <summary>
        /// Specifies the ability to read various attributes, not including the member attribute
        /// </summary>
        ALIAS_READ_INFORMATION = 0x00000008,
        /// <summary>
        /// Specifies the ability to write various attributes, not including the member attribute
        /// </summary>
        ALIAS_WRITE_ACCOUNT = 0x00000010,
        /// <summary>
        /// The specified accesses for a GENERIC_ALL request
        /// </summary>
        ALIAS_ALL_ACCESS = 0x000F001F,
        /// <summary>
        /// The specified accesses for a GENERIC_READ request
        /// </summary>
        ALIAS_READ = 0x00020004,
        /// <summary>
        /// The specified accesses for a GENERIC_WRITE request
        /// </summary>
        ALIAS_WRITE = 0x00020013,
        /// <summary>
        /// The specified accesses for a GENERIC_EXECUTE request
        /// </summary>
        ALIAS_EXECUTE = 0x00020008
    }

    /// <summary>
    /// An enumeration to specify user access masks
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags")]
    [Flags()]
    public enum USER_ACCESS_MASK : uint
    {
        /// <summary>
        /// Specifies the ability to read sundry attributes
        /// </summary>
        USER_READ_GENERAL = 0x00000001,
        /// <summary>
        /// Specifies the ability to read general information attributes
        /// </summary>
        USER_READ_PREFERENCES = 0x00000002,
        /// <summary>
        /// Specifies the ability to write general information attributes
        /// </summary>
        USER_WRITE_PREFERENCES = 0x00000004,
        /// <summary>
        /// Specifies the ability to read attributes related to logon statistics
        /// </summary>
        USER_READ_LOGON = 0x00000008,
        /// <summary>
        /// Specifies the ability to read attributes related to the administration of the user object
        /// </summary>
        USER_READ_ACCOUNT = 0x00000010,
        /// <summary>
        /// Specifies the ability to write attributes related to the administration of the user object
        /// </summary>
        USER_WRITE_ACCOUNT = 0x00000020,
        /// <summary>
        /// Specifies the ability to change the user's password
        /// </summary>
        USER_CHANGE_PASSWORD = 0x00000040,
        /// <summary>
        /// Specifies the ability to set the user's password
        /// </summary>
        USER_FORCE_PASSWORD_CHANGE = 0x00000080,
        /// <summary>
        /// Specifies the ability to query the membership of the user object
        /// </summary>
        USER_LIST_GROUPS = 0x00000100,
        /// <summary>
        /// Does not specify any access control
        /// </summary>
        USER_READ_GROUP_INFORMATION = 0x00000200,
        /// <summary>
        /// Does not specify any access control
        /// </summary>
        USER_WRITE_GROUP_INFORMATION = 0x00000400,
        /// <summary>
        /// The specified accesses for a GENERIC_ALL request
        /// </summary>
        USER_ALL_ACCESS = 0x000F07FF,
        /// <summary>
        /// The specified accesses for a GENERIC_READ request
        /// </summary>
        USER_READ = 0x0002031A,
        /// <summary>
        /// The specified accesses for a GENERIC_WRITE request
        /// </summary>
        USER_WRITE = 0x00020044,
        /// <summary>
        /// The specified accesses for a GENERIC_EXECUTE request
        /// </summary>
        USER_EXECUTE = 0x00020041
    }

    /// <summary>
    /// An enumeration to specify active directory access masks
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    public enum AD_ACCESS_MASK : uint
    {
        /// <summary>
        /// Indicates the ability to read the children of an object in Active Directory
        /// </summary>
        ACTRL_DS_LIST = 0x00000004,
        /// <summary>
        /// Indicates the access control to read a property in Active Directory
        /// </summary>
        ACTRL_DS_READ_PROP = 0x00000010,
        /// <summary>
        /// Indicates the access control to write a property in Active Directory
        /// </summary>
        ACTRL_DS_WRITE_PROP = 0x00000020,
        /// <summary>
        /// Indicates the ability to delete a tree of objects
        /// </summary>
        ACTRL_DS_DELETE_TREE = 0x00000040,
        /// <summary>
        /// Indicates the ability to perform an operation on an object as indicated by
        /// the ObjectGuid field in the ACE
        /// </summary>
        ACTRL_DS_CONTROL_ACCESS = 0x00000100
    }

    /// <summary>
    ///  An enumeration to specify the type of account that a
    ///  SID references.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_2_8.xml
    //  </remarks>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum _SID_NAME_USE
    {

        /// <summary>
        ///  Indicates a user object.
        /// </summary>
        SidTypeUser = 1,

        /// <summary>
        ///  Indicates a group object.
        /// </summary>
        SidTypeGroup,

        /// <summary>
        ///  Indicates a domain object.
        /// </summary>
        SidTypeDomain,

        /// <summary>
        ///  Indicates an alias object.
        /// </summary>
        SidTypeAlias,

        /// <summary>
        ///  Indicates an object whose SID is invariant.
        /// </summary>
        SidTypeWellKnownGroup,

        /// <summary>
        ///  Indicates an object that has been deleted.
        /// </summary>
        SidTypeDeletedAccount,

        /// <summary>
        ///  This member is unused.
        /// </summary>
        SidTypeInvalid,

        /// <summary>
        ///  Indicates that the type of object could not be determined.
        ///  For example, no object with that SID exists.
        /// </summary>
        SidTypeUnknown,
    }

    /// <summary>
    ///  The DOMAIN_INFORMATION_CLASS enumeration indicates how
    ///  to interpret the Buffer parameter for SamrSetInformationDomain
    ///  and SamrQueryInformationDomain. For a list of associated
    ///  structures, see section.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_4_16.xml
    //  </remarks>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum _DOMAIN_INFORMATION_CLASS
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
        ///  DomainUasInformation constant.
        /// </summary>
        DomainUasInformation = 10,

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
    }

    /// <summary>
    ///  The DOMAIN_SERVER_ENABLE_STATE enumeration describes
    ///  the enabled or disabled state of a server.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_4_2.xml
    //  </remarks>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum _DOMAIN_SERVER_ENABLE_STATE
    {

        /// <summary>
        ///  The server is considered "enabled" to the client.
        /// </summary>
        DomainServerEnabled = 1,

        /// <summary>
        ///  This field is not used.
        /// </summary>
        DomainServerDisabled,
    }

    /// <summary>
    ///  The DOMAIN_SERVER_ROLE enumeration indicates whether
    ///  a server is a PDC.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_4_4.xml
    //  </remarks>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum _DOMAIN_SERVER_ROLE
    {

        /// <summary>
        ///  The DC is not the PDC.
        /// </summary>
        DomainServerRoleBackup = 2,

        /// <summary>
        ///  The DC is the PDC.
        /// </summary>
        DomainServerRolePrimary = 3,
    }

    /// <summary>
    ///  The GROUP_INFORMATION_CLASS enumeration indicates how
    ///  to interpret the Buffer parameter for SamrSetInformationGroup
    ///  and SamrQueryInformationGroup. For a list of associated
    ///  structures, see section.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_5_6.xml
    //  </remarks>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum _GROUP_INFORMATION_CLASS
    {

        /// <summary>
        ///  GroupGeneralInformation constant.
        /// </summary>
        GroupGeneralInformation = 1,

        /// <summary>
        ///  GroupNameInformation constant.
        /// </summary>
        GroupNameInformation,

        /// <summary>
        ///  GroupAttributeInformation constant.
        /// </summary>
        GroupAttributeInformation,

        /// <summary>
        ///  GroupAdminCommentInformation constant.
        /// </summary>
        GroupAdminCommentInformation,

        /// <summary>
        ///  GroupReplicationInformation constant.
        /// </summary>
        GroupReplicationInformation,
    }

    /// <summary>
    ///  The ALIAS_INFORMATION_CLASS enumeration indicates how
    ///  to interpret the Buffer parameter for SamrQueryInformationAlias
    ///  and SamrSetInformationAlias. For a list of  the structures
    ///  associated with each enumeration, see section.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_6_5.xml
    //  </remarks>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum _ALIAS_INFORMATION_CLASS
    {

        /// <summary>
        ///  AliasGeneralInformation constant.
        /// </summary>
        AliasGeneralInformation = 1,

        /// <summary>
        ///  AliasNameInformation constant.
        /// </summary>
        AliasNameInformation,

        /// <summary>
        ///  AliasAdminCommentInformation constant.
        /// </summary>
        AliasAdminCommentInformation,
    }

    /// <summary>
    ///  The USER_INFORMATION_CLASS enumeration indicates how
    ///  to interpret the Buffer parameter for SamrSetInformationUser
    ///  and SamrQueryInformationUser. For a list of associated
    ///  structures, see section.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_7_29.xml
    //  </remarks>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags")]
    public enum _USER_INFORMATION_CLASS
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

        /// <summary>
        /// Invalid information level.
        /// </summary>
        UserInvalidInformation = 88,
    }

    /// <summary>
    ///  The DOMAIN_DISPLAY_INFORMATION enumeration indicates
    ///  how to interpret the Buffer parameter for SamrQueryDisplayInformation,
    ///  SamrQueryDisplayInformation2, SamrQueryDisplayInformation3,
    ///  SamrGetDisplayEnumerationIndex, and SamrGetDisplayEnumerationIndex2.
    ///  See section  for the list of the structures that are
    ///  associated with each enumeration.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_8_12.xml
    //  </remarks>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum _DOMAIN_DISPLAY_INFORMATION
    {

        /// <summary>
        ///  DomainDisplayUser constant.
        /// </summary>
        DomainDisplayUser = 1,

        /// <summary>
        ///  DomainDisplayMachine constant.
        /// </summary>
        DomainDisplayMachine,

        /// <summary>
        ///  DomainDisplayGroup constant.
        /// </summary>
        DomainDisplayGroup,

        /// <summary>
        ///  DomainDisplayOemUser constant.
        /// </summary>
        DomainDisplayOemUser,

        /// <summary>
        ///  DomainDisplayOemGroup constant.
        /// </summary>
        DomainDisplayOemGroup,
    }

    /// <summary>
    ///  The SAM_VALIDATE_VALIDATION_STATUS enumeration defines
    ///  policy evaluation outcomes.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_9_3.xml
    //  </remarks>
    public enum _SAM_VALIDATE_VALIDATION_STATUS
    {

        /// <summary>
        ///  SamValidateSuccess constant.
        /// </summary>
        SamValidateSuccess = 0,

        /// <summary>
        ///  SamValidatePasswordMustChange constant.
        /// </summary>
        SamValidatePasswordMustChange,

        /// <summary>
        ///  SamValidateAccountLockedOut constant.
        /// </summary>
        SamValidateAccountLockedOut,

        /// <summary>
        ///  SamValidatePasswordExpired constant.
        /// </summary>
        SamValidatePasswordExpired,

        /// <summary>
        ///  SamValidatePasswordIncorrect constant.
        /// </summary>
        SamValidatePasswordIncorrect,

        /// <summary>
        ///  SamValidatePasswordIsInHistory constant.
        /// </summary>
        SamValidatePasswordIsInHistory,

        /// <summary>
        ///  SamValidatePasswordTooShort constant.
        /// </summary>
        SamValidatePasswordTooShort,

        /// <summary>
        ///  SamValidatePasswordTooLong constant.
        /// </summary>
        SamValidatePasswordTooLong,

        /// <summary>
        ///  SamValidatePasswordNotComplexEnough constant.
        /// </summary>
        SamValidatePasswordNotComplexEnough,

        /// <summary>
        ///  SamValidatePasswordTooRecent constant.
        /// </summary>
        SamValidatePasswordTooRecent,

        /// <summary>
        ///  SamValidatePasswordFilterError constant.
        /// </summary>
        SamValidatePasswordFilterError,
    }

    /// <summary>
    ///  The PASSWORD_POLICY_VALIDATION_TYPE enumeration indicates
    ///  the type of policy validation that is being requested.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_9_8.xml
    //  </remarks>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum _PASSWORD_POLICY_VALIDATION_TYPE
    {

        /// <summary>
        ///  Indicates a request to execute the password policy validation
        ///  performed at logon.
        /// </summary>
        SamValidateAuthentication = 1,

        /// <summary>
        ///  Indicates a request to execute the password policy validation
        ///  performed during a password change request.
        /// </summary>
        SamValidatePasswordChange,

        /// <summary>
        ///  Indicates a request to execute the password policy validation
        ///  performed during a password reset.
        /// </summary>
        SamValidatePasswordReset,
    }

    /// <summary>
    ///  The LARGE_INTEGER structure is used to represent a 64-bit
    ///  signed integer value.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/DTYP/_rfc_ms-dtyp_large_integer_union.xml
    //  </remarks>
    public partial struct _LARGE_INTEGER
    {

        /// <summary>
        ///  QuadPart member.
        /// </summary>
        public long QuadPart;
    }

    /// <summary>
    ///  The USER_PROPERTIES structure defines the format of
    ///  the supplementalCredentials attribute.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_10_1.xml
    //  </remarks>
    public partial struct _USER_PROPERTIES
    {

        /// <summary>
        ///  This value MAY be ignored by the recipient and MAY contain
        ///  arbitrary values. Sets this buffer to the repeating
        ///  pattern 0x20 0x00 on update.
        /// </summary>
        [Inline()]
        [StaticSize(48, StaticSizeMode.Elements)]
        public ushort[] Reserved;

        /// <summary>
        ///  This field MUST be the value 0x50, in little-endian
        ///  byte order. This is an arbitrary value used to indicate
        ///  whether the structure is corrupt. That is, if this
        ///  value is not 0x50 on read, the structure is considered
        ///  corrupt, processing MUST be aborted, and an error code
        ///  MUST be returned.
        /// </summary>
        public ushort PropertySignature;

        /// <summary>
        ///  The number of USER_PROPERTY elements (which are variable-length
        ///  themselves) that follow the USER_PROPERTIES structure.
        /// </summary>
        public ushort PropertyCount;
    }

    /// <summary>
    ///  The USER_PROPERTY structure defines an array element
    ///  that contains a single property name and value for
    ///  the supplementalCredentials attribute.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_10_2.xml
    //  </remarks>
    public partial struct _USER_PROPERTY
    {

        /// <summary>
        ///  The number of bytes, in little-endian byte order, of
        ///  the property name. The property name is located at
        ///  an offset of zero bytes just following the Reserved
        ///  field. For more information, see the message processing
        ///  section for supplementalCredentials.
        /// </summary>
        public ushort PropertyLength;

        /// <summary>
        ///  The number of bytes of the property value. The property
        ///  value is located at an offset of PropertyLength bytes
        ///  following the Reserved field. The value MUST be hexadecimal-encoded
        ///  using an 8-bit character size, and the values '0' through
        ///  '9' inclusive and 'a' through 'f' inclusive (the specification
        ///  of 'a' through 'f' is case-sensitive).
        /// </summary>
        public ushort ValueLength;

        /// <summary>
        ///  This value is ignored on read and MAY sets this value
        ///  to 1 or 2, but does not use the value.  be set to arbitrary
        ///  values on update.
        /// </summary>
        public ushort Reserved;
    }

    /// <summary>
    ///  The WDIGEST_CREDENTIALS structure defines the format
    ///  of the Primary:WDigest property within the supplementalCredentials
    ///  attribute. This structure is stored as a property value
    ///  in a USER_PROPERTY structure.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_10_3.xml
    //  </remarks>
    public partial struct _WDIGEST_CREDENTIALS
    {

        /// <summary>
        ///  This value is ignored on read and MAY sets this value
        ///  to 0x31 and ignores it on read.  be set to arbitrary
        ///  values upon an update to the supplementalCredentials
        ///  attribute.
        /// </summary>
        public byte Reserved1;

        /// <summary>
        ///  This value MUST be set to 1.
        /// </summary>
        public Version_Values Version;

        /// <summary>
        ///  This value MUST be set to 29 because there are 29 hashes
        ///  in the array.
        /// </summary>
        public NumberOfHashes_Values NumberOfHashes;

        /// <summary>
        ///  This value is ignored on read and SHOULD  set to be
        ///  zero.
        /// </summary>
        [Inline()]
        [StaticSize(13, StaticSizeMode.Elements)]
        public byte[] Reserved2;

        /// <summary>
        ///  Hash1 member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Hash1;

        /// <summary>
        ///  Hash2 member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Hash2;

        /// <summary>
        ///  Hash3 member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Hash3;

        /// <summary>
        ///  Hash4 member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Hash4;

        /// <summary>
        ///  Hash5 member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Hash5;

        /// <summary>
        ///  Hash6 member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Hash6;

        /// <summary>
        ///  Hash7 member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Hash7;

        /// <summary>
        ///  Hash8 member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Hash8;

        /// <summary>
        ///  Hash9 member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Hash9;

        /// <summary>
        ///  Hash10 member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Hash10;

        /// <summary>
        ///  Hash11 member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Hash11;

        /// <summary>
        ///  Hash12 member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Hash12;

        /// <summary>
        ///  Hash13 member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Hash13;

        /// <summary>
        ///  Hash14 member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Hash14;

        /// <summary>
        ///  Hash15 member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Hash15;

        /// <summary>
        ///  Hash16 member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Hash16;

        /// <summary>
        ///  Hash17 member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Hash17;

        /// <summary>
        ///  Hash18 member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Hash18;

        /// <summary>
        ///  Hash19 member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Hash19;

        /// <summary>
        ///  Hash20 member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Hash20;

        /// <summary>
        ///  Hash21 member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Hash21;

        /// <summary>
        ///  Hash22 member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Hash22;

        /// <summary>
        ///  Hash23 member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Hash23;

        /// <summary>
        ///  Hash24 member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Hash24;

        /// <summary>
        ///  Hash25 member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Hash25;

        /// <summary>
        ///  Hash26 member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Hash26;

        /// <summary>
        ///  Hash27 member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Hash27;

        /// <summary>
        ///  Hash28 member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Hash28;

        /// <summary>
        ///  Hash29 member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Hash29;
    }

    /// <summary>
    /// Version values for _WDIGEST_CREDENTIALS structure.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Version_Values : byte
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 1,
    }

    /// <summary>
    /// Value of number of hashes for _WDIGEST_CREDENTIALS structure.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum NumberOfHashes_Values : byte
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 29,
    }

    /// <summary>
    ///  The KERB_STORED_CREDENTIAL structure is a variable-length
    ///  structure that defines the format of the Primary:Kerberos
    ///  property within the supplementalCredentials attribute.
    ///  For information on how this structure is created, see
    ///  section.This structure is stored as a property value
    ///  in a USER_PROPERTY structure.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_10_4.xml
    //  </remarks>
    public partial struct _KERB_STORED_CREDENTIAL
    {

        /// <summary>
        ///  This value MUST be set to 3.
        /// </summary>
        public Revision_Values Revision;

        /// <summary>
        ///  This value MUST be zero and ignored on read.
        /// </summary>
        public Flags_Values Flags;

        /// <summary>
        ///  This is the count of elements in an array that follows
        ///  the KERB_STORED_CREDENTIAL structure that contains
        ///  the keys for the current password. This value MUST
        ///  be set to 2.
        /// </summary>
        public CredentialCount_Values CredentialCount;

        /// <summary>
        ///  This is the count of elements in an array that follows
        ///  the KERB_STORED_CREDENTIAL structure that contains
        ///  the keys for the previous password. This value MUST
        ///  be set to 0 or 2.
        /// </summary>
        public ushort OldCredentialCount;

        /// <summary>
        ///  The length, in bytes, of a salt value.This value is
        ///  in little-endian byte order. This value SHOULD be ignored
        ///  on read.
        /// </summary>
        public ushort DefaultSaltLength;

        /// <summary>
        ///  The length, in bytes, of the buffer containing the salt
        ///  value.This value is in little-endian byte order. This
        ///  value SHOULD be ignored on read.
        /// </summary>
        public ushort DefaultSaltMaximumLength;

        /// <summary>
        ///  An offset, in little-endian byte order, from the beginning
        ///  of the attribute value (that is, from the beginning
        ///  of the Revision field of KERB_STORED_CREDENTIAL) to
        ///  where the salt value starts. This value SHOULD be ignored
        ///  on read.
        /// </summary>
        public uint DefaultSaltOffset;

        /// <summary>
        ///  This value MUST be set to zero.
        /// </summary>
        public Padding_Values Padding;
    }

    /// <summary>
    /// Revision values for _KERB_STORED_CREDENTIAL structure.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Revision_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 3,
    }

    /// <summary>
    /// Flag values for _KERB_STORED_CREDENTIAL structure.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Flags_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// Credential count possible values for _KERB_STORED_CREDENTIAL structure.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum CredentialCount_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 2,
    }

    /// <summary>
    /// Padding values for _KERB_STORED_CREDENTIAL structure.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Padding_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The KERB_KEY_DATA structure holds a cryptographic key.
    ///  This structure is used in conjunction with KERB_STORED_CREDENTIAL.
    ///  For more information, see section.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_10_5.xml
    //  </remarks>
    public partial struct _KERB_KEY_DATA
    {

        /// <summary>
        ///  This value MUST be set to zero and ignored on receipt.
        /// </summary>
        public Reserved1_Values Reserved1;

        /// <summary>
        ///  This value MUST be set to zero and ignored on receipt.
        /// </summary>
        public Reserved2_Values Reserved2;

        /// <summary>
        ///  This value MUST be set to zero and ignored on receipt.
        /// </summary>
        public Reserved3_Values Reserved3;

        /// <summary>
        ///  Indicates the type of key, stored as a 32-bit, unsigned
        ///  integer in little-endian byte order. Legal values for
        ///  this field are in the right-hand column of the following
        ///  table, along with the associated semantic.
        /// </summary>
        public KeyType_Values KeyType;

        /// <summary>
        ///  The length, in bytes, of the key value. The value of
        ///  this member is stored in little-endian byte order.
        /// </summary>
        public uint KeyLength;

        /// <summary>
        ///  An offset, in little-endian byte order, from the beginning
        ///  of the property value (that is, from the beginning
        ///  of the Revision field of KERB_STORED_CREDENTIAL) to
        ///  where the key value starts. The key value is the hash
        ///  value specified according to the KeyType.
        /// </summary>
        public uint KeyOffset;
    }

    /// <summary>
    /// Values of reserved1 for _KERB_KEY_DATA structure.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Reserved1_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// Values of reserved2 for _KERB_KEY_DATA structure.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Reserved2_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// Values of reserved3 for _KERB_KEY_DATA structure.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Reserved3_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// Values of key types for _KERB_KEY_DATA structure.
    /// </summary>
    [Flags()]
    public enum KeyType_Values : int
    {

        /// <summary>
        ///  des-cbc-md5 ([RFC3961] section 6.2.1)
        /// </summary>
        V1 = 1,

        /// <summary>
        ///  dec-cbc-crc ([RFC3961] section 6.2.3)
        /// </summary>
        V2 = 2,
    }

    /// <summary>
    ///  The SID_IDENTIFIER_AUTHORITY structure defines a substructure
    ///  of RPC_SID.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_2_3.xml
    //  </remarks>
    public partial struct _SID_IDENTIFIER_AUTHORITY
    {

        /// <summary>
        ///  An array of 6 bytes that is the IdentifierAuthority
        ///  member of an RPC_SID structure.
        /// </summary>
        [Inline()]
        [StaticSize(6, StaticSizeMode.Elements)]
        public byte[] Value;
    }

    /// <summary>
    ///  The SAMPR_SR_SECURITY_DESCRIPTOR structure holds a formatted
    ///  security descriptor.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_3_10.xml
    //  </remarks>
    public partial struct _SAMPR_SR_SECURITY_DESCRIPTOR
    {

        /// <summary>
        ///  The size, in bytes, of SecurityDescriptor. If zero,
        ///  SecurityDescriptor MUST be ignored. The maximum size
        ///  of 256 * 1024 is an arbitrary value chosen to limit
        ///  the amount of memory a client can force the server
        ///  to allocate.
        /// </summary>
        public uint Length;

        /// <summary>
        ///  A binary format per the SECURITY_DESCRIPTOR format in
        ///  [MS-DTYP] section.
        /// </summary>
        [Size("Length")]
        public byte[] SecurityDescriptor;
    }

    /// <summary>
    ///  The GROUP_MEMBERSHIP structure holds information on
    ///  a group membership.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_3_11.xml
    //  </remarks>
    public partial struct _GROUP_MEMBERSHIP
    {

        /// <summary>
        ///  A RID that represents one membership value.
        /// </summary>
        public uint RelativeId;

        /// <summary>
        ///  Characteristics about the membership represented as
        ///  a bitmask. Values are defined in section 2.2.1.10.
        /// </summary>
        public uint Attributes;
    }

    /// <summary>
    ///  The SAMPR_GET_MEMBERS_BUFFER structure represents the
    ///  membership of a group.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_3_13.xml
    //  </remarks>
    public partial struct _SAMPR_GET_MEMBERS_BUFFER
    {

        /// <summary>
        ///  The number of elements in Members and Attributes. If
        ///  zero, Members and Attributes MUST be ignored. If nonzero,
        ///  Members and Attributes MUST point to at least MemberCount
        ///  * sizeof(unsigned long) bytes of memory.
        /// </summary>
        public uint MemberCount;

        /// <summary>
        ///  An array of RIDs.
        /// </summary>
        [Size("MemberCount")]
        public uint[] Members;

        /// <summary>
        ///  Characteristics about the membership, represented as
        ///  a bitmask. Values are defined in section.
        /// </summary>
        [Size("MemberCount")]
        public uint[] Attributes;
    }

    /// <summary>
    ///  The SAMPR_REVISION_INFO_V1 structure is used to communicate
    ///  the revision and capabilities of client and server.
    ///  For more information, see SamrConnect5.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_3_14.xml
    //  </remarks>
    public partial struct _SAMPR_REVISION_INFO_V1
    {

        /// <summary>
        ///  The revision of the client or server side of this protocol
        ///  (depending on which side sends the structure). The
        ///  value MUST be one of the following.
        /// </summary>
        public _SAMPR_REVISION_INFO_V1_Revision_Values Revision;

        /// <summary>
        ///  A bit field. When sent from the client, this field MUST
        ///  be zero and ignored on receipt. When returned from
        ///  the server, the following fields are handled by the
        ///  client; all other bits are ignored and MUST be zero
        ///  when returned from the client.
        /// </summary>
        public SupportedFeatures_Values SupportedFeatures;
    }

    /// <summary>
    /// Revision values for _SAMPR_REVISION_INFO_V1 structure.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    public enum _SAMPR_REVISION_INFO_V1_Revision_Values : uint
    {

        /// <summary>
        ///  Pre
        /// </summary>
        V1 = 1,

        /// <summary>
        /// </summary>
        V2 = 2,

        /// <summary>
        ///  , , and
        /// </summary>
        V3 = 3,
    }

    /// <summary>
    /// Supported feature values for _SAMPR_REVISION_INFO_V1 structure.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    public enum SupportedFeatures_Values : uint
    {

        /// <summary>
        ///  On receipt by the client, this value, when set, indicates
        ///  that RID values returned from the server MUST NOT be
        ///  concatenated with the domain SID to create the SID
        ///  for the account referenced by the RID. Instead, the
        ///  client MUST call SamrRidToSid to obtain the SID. This
        ///  field can be combined with other bits using a logical
        ///  OR.See the Windows Behavior citation at the end of
        ///  this section for more information (about  implementations).
        /// </summary>
        V1 = 0x00000001,

        /// <summary>
        ///  Reserved. See the Windows Behavior citation at the end
        ///  of this section for additional details.
        /// </summary>
        V2 = 0x00000002,

        /// <summary>
        ///  Reserved. See the Windows Behavior citation at the end
        ///  of this section for additional details.
        /// </summary>
        V3 = 0x00000004,
    }

    /// <summary>
    ///  The USER_DOMAIN_PASSWORD_INFORMATION structure contains
    ///  domain fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_3_16.xml
    //  </remarks>
    public partial struct _USER_DOMAIN_PASSWORD_INFORMATION
    {

        /// <summary>
        ///  MinPasswordLength member.
        /// </summary>
        public ushort MinPasswordLength;

        /// <summary>
        ///  PasswordProperties member.
        /// </summary>
        public uint PasswordProperties;
    }

    /// <summary>
    ///  The ENCRYPTED_LM_OWF_PASSWORD structure defines a block
    ///  of encrypted data used in various methods to communicate
    ///  sensitive information.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_3_3.xml
    //  </remarks>
    public partial struct _ENCRYPTED_LM_OWF_PASSWORD
    {

        /// <summary>
        ///  16 bytes of unstructured data used to hold an encrypted
        ///  16-byte hash (either an LM hash or an NT hash). The
        ///  encryption algorithm is specified in section. The
        ///  methods specified in sections  and  use this structure
        ///  and specify the type of hash and  the encryption key.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] data;
    }

    /// <summary>
    ///  The SAMPR_ULONG_ARRAY structure holds a counted array
    ///  of unsigned long values.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_3_4.xml
    //  </remarks>
    public partial struct _SAMPR_ULONG_ARRAY
    {

        /// <summary>
        ///  The number of elements in Element. If zero, Element
        ///  MUST be ignored. If nonzero, Element MUST point to
        ///  at least Count * sizeof(unsigned long) bytes of memory.
        /// </summary>
        public uint Count;

        /// <summary>
        ///  A pointer to an array of unsigned integers with Count
        ///  elements. The semantic meaning is dependent on the
        ///  method in which the structure is being used.
        /// </summary>
        [Size("Count")]
        public uint[] Element;
    }

    /// <summary>
    ///  The DOMAIN_STATE_INFORMATION structure holds the enabled/disabled
    ///  state of the server.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_4_3.xml
    //  </remarks>
    public partial struct _DOMAIN_STATE_INFORMATION
    {

        /// <summary>
        ///  DomainServerState member.
        /// </summary>
        public _DOMAIN_SERVER_ENABLE_STATE DomainServerState;
    }

    /// <summary>
    ///  The DOMAIN_SERVER_ROLE_INFORMATION structure contains
    ///  domain fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_4_7.xml
    //  </remarks>
    public partial struct _DOMAIN_SERVER_ROLE_INFORMATION
    {

        /// <summary>
        ///  DomainServerRole member.
        /// </summary>
        public _DOMAIN_SERVER_ROLE DomainServerRole;
    }

    /// <summary>
    ///  The GROUP_ATTRIBUTE_INFORMATION structure contains group
    ///  fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_5_2.xml
    //  </remarks>
    public partial struct _GROUP_ATTRIBUTE_INFORMATION
    {

        /// <summary>
        ///  Attributes member.
        /// </summary>
        public uint Attributes;
    }

    /// <summary>
    ///  The USER_PRIMARY_GROUP_INFORMATION structure contains
    ///  user fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_7_2.xml
    //  </remarks>
    public partial struct _USER_PRIMARY_GROUP_INFORMATION
    {

        /// <summary>
        ///  PrimaryGroupId member.
        /// </summary>
        public uint PrimaryGroupId;
    }

    /// <summary>
    ///  The SAMPR_ENCRYPTED_USER_PASSWORD structure carries
    ///  an encrypted string.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_7_21.xml
    //  </remarks>
    public partial struct _SAMPR_ENCRYPTED_USER_PASSWORD
    {

        /// <summary>
        ///  An array to carry encrypted clear text password data.
        ///  The encryption key is method-specific, while the algorithm
        ///  specified in section  is common for all methods that
        ///  use this structure. See the message syntax for SamrOemChangePasswordUser2
        ///  and SamrUnicodeChangePasswordUser2, and the message
        ///  processing for SamrSetInformationUser2, for details
        ///  on the encryption key selection. The size of (256 *
        ///  2) + 4 for Buffer is determined by the size of the
        ///  structure that is encrypted, SAMPR_USER_PASSWORD; see
        ///  below for more details.For all protocol uses, the decrypted
        ///  format of Buffer is the following structure.typedef
        ///  struct _SAMPR_USER_PASSWORD {     wchar_t       Buffer[256];
        ///      unsigned long Length; } SAMPR_USER_PASSWORD, *PSAMPR_USER_PASSWORD;Buffer:
        ///  This array contains the clear text value at the end
        ///  of the buffer. The start of the string is Length number
        ///  of bytes from the end of the buffer. The clear text
        ///  value can be no more than 512 bytes. The unused portions
        ///  of SAMPR_USER_PASSWORD.Buffer SHOULD be filled with
        ///  random bytes by the client. The value 512 is chosen
        ///  because that is the longest password allowed by this
        ///  protocol (and enforced by the server).Length: An unsigned
        ///  integer, in little-endian byte order, that indicates
        ///  the number of bytes of the clear text value located
        ///  in SAMPR_USER_PASSWORD.Buffer.
        /// </summary>
        [Inline()]
        [StaticSize(516, StaticSizeMode.Elements)]
        public byte[] Buffer;
    }

    /// <summary>
    ///  The SAMPR_ENCRYPTED_USER_PASSWORD_NEW structure carries
    ///  an encrypted string.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_7_22.xml
    //  </remarks>
    public partial struct _SAMPR_ENCRYPTED_USER_PASSWORD_NEW
    {

        /// <summary>
        ///  An array to carry encrypted clear text password data.For
        ///  all protocol uses, the decrypted format of Buffer is
        ///  the following structure.typedef struct _SAMPR_USER_PASSWORD_NEW
        ///  {     WCHAR Buffer[256];     ULONG Length;     UCHAR
        ///  ClearSalt[16]; } SAMPR_USER_PASSWORD_NEW, *PSAMPR_USER_PASSWORD_NEW;Buffer:
        ///  This array contains the clear text value at the end
        ///  of the buffer. The clear text value can be no more than
        ///  512 bytes. The start of the string is Length number
        ///  of bytes from the end of the buffer. The unused portions
        ///  of SAMPR_USER_PASSWORD_NEW.Buffer SHOULD be filled
        ///  with random bytes by the client.Length: An unsigned
        ///  integer, in little-endian byte order, that indicates
        ///  the number of bytes of the clear text value (located
        ///  in SAMPR_USER_PASSWORD_NEW.Buffer).ClearSalt: This
        ///  value (a salt) MUST be filled with random bytes by
        ///  the client and MUST NOT be encrypted. The length of
        ///  16 was chosen in particular because 128 bits of randomness
        ///  was deemed sufficiently secure when this protocol was
        ///  introduced (circa 1998).
        /// </summary>
        [Inline()]
        [StaticSize(532, StaticSizeMode.Elements)]
        public byte[] Buffer;
    }

    /// <summary>
    ///  The USER_CONTROL_INFORMATION structure contains user
    ///  fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_7_3.xml
    //  </remarks>
    public partial struct _USER_CONTROL_INFORMATION
    {

        /// <summary>
        ///  UserAccountControl member.
        /// </summary>
        public uint UserAccountControl;
    }

    /// <summary>
    ///  The SAMPR_LOGON_HOURS structure contains logon policy
    ///  information that describes when a user account is permitted
    ///  to authenticate.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_7_5.xml
    //  </remarks>
    public partial struct _SAMPR_LOGON_HOURS
    {

        /// <summary>
        ///  A division of the week (7 days). For example, the value
        ///  7 means that each unit is a day; a value of (7*24)
        ///  means that the units are hours. The minimum granularity
        ///  of time is one minute, where the UnitsPerWeek would
        ///  be 10080; therefore, the maximum size of LogonHours
        ///  is 10080/8, or 1,260 bytes.
        /// </summary>
        public ushort UnitsPerWeek;

        /// <summary>
        ///  A pointer to a bit field containing at least UnitsPerWeek
        ///  number of bits. The leftmost bit represents the first
        ///  unit, starting at Sunday, 12 A.M. If a bit is set,
        ///  authentication is allowed to occur; otherwise, authentication
        ///  is not allowed to occur.For example, if the UnitsPerWeek
        ///  value is 168 (that is, the units per week is hours,
        ///  resulting in a 21-byte bit field), and if the leftmost
        ///  bit is set and the rightmost bit is set, the user is
        ///  able to log on for two consecutive hours between Saturday,
        ///  11 P.M. and Sunday, 1 A.M.
        /// </summary>
        [Length("(UnitsPerWeek+7)/8")]
        [Size("1260")]
        public byte[] LogonHours;
    }

    /// <summary>
    ///  The SAM_VALIDATE_PASSWORD_HASH structure holds a binary
    ///  value that represents a cryptographic hash.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_9_1.xml
    //  </remarks>
    public partial struct _SAM_VALIDATE_PASSWORD_HASH
    {

        /// <summary>
        ///  The size, in bytes, of Hash. If zero, Hash MUST be ignored.
        /// </summary>
        public uint Length;

        /// <summary>
        ///  A binary value.
        /// </summary>
        [Size("Length")]
        public byte[] Hash;
    }

    /// <summary>
    ///  The SAMPR_GET_GROUPS_BUFFER structure represents the
    ///  members of a group.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_3_12.xml
    //  </remarks>
    public partial struct _SAMPR_GET_GROUPS_BUFFER
    {

        /// <summary>
        ///  The number of elements in Groups. If zero, Groups MUST
        ///  be ignored. If nonzero, Groups MUST point to at least
        ///  MembershipCount * sizeof(GROUP_MEMBERSHIP) bytes of
        ///  memory.
        /// </summary>
        public uint MembershipCount;

        /// <summary>
        ///  An array to hold information about the members of the
        ///  group.
        /// </summary>
        [Size("MembershipCount")]
        public _GROUP_MEMBERSHIP[] Groups;
    }

    /// <summary>
    ///  The SAMPR_REVISION_INFO union holds revision information
    ///  structures that are used in the SamrConnect5 method.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_3_15.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct SAMPR_REVISION_INFO
    {

        /// <summary>
        ///  Version 1 revision information, as described in SAMPR_REVISION_INFO_V1.
        /// </summary>
        [Case("1")]
        public _SAMPR_REVISION_INFO_V1 V1;
    }

    /// <summary>
    ///  The SAMPR_SID_INFORMATION structure holds a SID pointer.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_3_5.xml
    //  </remarks>
    public partial struct _SAMPR_SID_INFORMATION
    {

        /// <summary>
        ///  A pointer to a SID value, as described in section.
        /// </summary>
        [Indirect]
        public _RPC_SID SidPointer;
    }

    /// <summary>
    ///  The SAMPR_PSID_ARRAY structure holds an array of SID
    ///  values.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_3_6.xml
    //  </remarks>
    public partial struct _SAMPR_PSID_ARRAY
    {

        /// <summary>
        ///  The number of elements in Sids. If zero, Sids MUST be
        ///  ignored. If nonzero, Sids MUST point to at least Count
        ///  * sizeof(SAMPR_SID_INFORMATION) bytes of memory.
        /// </summary>
        public uint Count;

        /// <summary>
        ///  An array of pointers to SID values. For more information,
        ///  see section.
        /// </summary>
        [Size("Count")]
        public _SAMPR_SID_INFORMATION[] Sids;
    }

    /// <summary>
    ///  The SAMPR_RETURNED_USTRING_ARRAY structure holds an
    ///  array of counted UTF-16 encoded strings.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_3_7.xml
    //  </remarks>
    public partial struct _SAMPR_RETURNED_USTRING_ARRAY
    {

        /// <summary>
        ///  The number of elements in Element. If zero, Element
        ///  MUST be ignored. If nonzero, Element MUST point to
        ///  at least Count * sizeof(RPC_UNICODE_STRING) bytes of
        ///  memory.
        /// </summary>
        public uint Count;

        /// <summary>
        ///  Array of counted strings (see section ). The semantic
        ///  meaning is method-dependent.
        /// </summary>
        [Size("Count")]
        public _RPC_UNICODE_STRING[] Element;
    }

    /// <summary>
    ///  The SAMPR_RID_ENUMERATION structure holds the name and
    ///  RID information about an account.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_3_8.xml
    //  </remarks>
    public partial struct _SAMPR_RID_ENUMERATION
    {

        /// <summary>
        ///  A RID.
        /// </summary>
        public uint RelativeId;

        /// <summary>
        ///  The UTF-16 encoded name of the account that is associated
        ///  with RelativeId.
        /// </summary>
        public _RPC_UNICODE_STRING Name;
    }

    /// <summary>
    ///  The SAMPR_ENUMERATION_BUFFER structure holds an array
    ///  of SAMPR_RID_ENUMERATION elements.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_3_9.xml
    //  </remarks>
    public partial struct _SAMPR_ENUMERATION_BUFFER
    {

        /// <summary>
        ///  The number of elements in Buffer. If zero, Buffer MUST
        ///  be ignored. If nonzero, Buffer MUST point to at least
        ///  EntriesRead * sizeof(SAMPR_RID_ENUMERATION) bytes of
        ///  memory.
        /// </summary>
        public uint EntriesRead;

        /// <summary>
        ///  An array of SAMPR_RID_ENUMERATION elements.
        /// </summary>
        [Size("EntriesRead")]
        public _SAMPR_RID_ENUMERATION[] Buffer;
    }

    /// <summary>
    ///  The SAMPR_DOMAIN_GENERAL_INFORMATION structure contains
    ///  domain fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_4_10.xml
    //  </remarks>
    public partial struct _SAMPR_DOMAIN_GENERAL_INFORMATION
    {

        /// <summary>
        ///  ForceLogoff member.
        /// </summary>
        public _OLD_LARGE_INTEGER ForceLogoff;

        /// <summary>
        ///  OemInformation member.
        /// </summary>
        public _RPC_UNICODE_STRING OemInformation;

        /// <summary>
        ///  DomainName member.
        /// </summary>
        public _RPC_UNICODE_STRING DomainName;

        /// <summary>
        ///  ReplicaSourceNodeName member.
        /// </summary>
        public _RPC_UNICODE_STRING ReplicaSourceNodeName;

        /// <summary>
        ///  DomainModifiedCount member.
        /// </summary>
        public _OLD_LARGE_INTEGER DomainModifiedCount;

        /// <summary>
        ///  DomainServerState member.
        /// </summary>
        public _DOMAIN_SERVER_ENABLE_STATE DomainServerState;

        /// <summary>
        ///  DomainServerRole member.
        /// </summary>
        public _DOMAIN_SERVER_ROLE DomainServerRole;

        /// <summary>
        ///  UasCompatibilityRequired member.
        /// </summary>
        public byte UasCompatibilityRequired;

        /// <summary>
        ///  UserCount member.
        /// </summary>
        public uint UserCount;

        /// <summary>
        ///  GroupCount member.
        /// </summary>
        public uint GroupCount;

        /// <summary>
        ///  AliasCount member.
        /// </summary>
        public uint AliasCount;
    }

    /// <summary>
    ///  The SAMPR_DOMAIN_GENERAL_INFORMATION2 structure contains
    ///  domain fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_4_11.xml
    //  </remarks>
    public partial struct _SAMPR_DOMAIN_GENERAL_INFORMATION2
    {

        /// <summary>
        ///  I1 member.
        /// </summary>
        public _SAMPR_DOMAIN_GENERAL_INFORMATION I1;

        /// <summary>
        ///  LockoutDuration member.
        /// </summary>
        public _OLD_LARGE_INTEGER LockoutDuration;

        /// <summary>
        ///  LockoutObservationWindow member.
        /// </summary>
        public _OLD_LARGE_INTEGER LockoutObservationWindow;

        /// <summary>
        ///  LockoutThreshold member.
        /// </summary>
        public ushort LockoutThreshold;
    }

    /// <summary>
    ///  The SAMPR_DOMAIN_OEM_INFORMATION structure contains
    ///  domain fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_4_12.xml
    //  </remarks>
    public partial struct _SAMPR_DOMAIN_OEM_INFORMATION
    {

        /// <summary>
        ///  OemInformation member.
        /// </summary>
        public _RPC_UNICODE_STRING OemInformation;
    }

    /// <summary>
    ///  The SAMPR_DOMAIN_NAME_INFORMATION structure contains
    ///  domain fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_4_13.xml
    //  </remarks>
    public partial struct _SAMPR_DOMAIN_NAME_INFORMATION
    {

        /// <summary>
        ///  DomainName member.
        /// </summary>
        public _RPC_UNICODE_STRING DomainName;
    }

    /// <summary>
    ///  The SAMPR_DOMAIN_REPLICATION_INFORMATION structure contains
    ///  domain fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_4_14.xml
    //  </remarks>
    public partial struct SAMPR_DOMAIN_REPLICATION_INFORMATION
    {

        /// <summary>
        ///  ReplicaSourceNodeName member.
        /// </summary>
        public _RPC_UNICODE_STRING ReplicaSourceNodeName;
    }

    /// <summary>
    ///  The SAMPR_DOMAIN_LOCKOUT_INFORMATION structure contains
    ///  domain fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_4_15.xml
    //  </remarks>
    public partial struct _SAMPR_DOMAIN_LOCKOUT_INFORMATION
    {

        /// <summary>
        ///  LockoutDuration member.
        /// </summary>
        public _OLD_LARGE_INTEGER LockoutDuration;

        /// <summary>
        ///  LockoutObservationWindow member.
        /// </summary>
        public _OLD_LARGE_INTEGER LockoutObservationWindow;

        /// <summary>
        ///  LockoutThreshold member.
        /// </summary>
        public ushort LockoutThreshold;
    }

    /// <summary>
    ///  The DOMAIN_PASSWORD_INFORMATION structure contains domain
    ///  fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_4_5.xml
    //  </remarks>
    public partial struct _DOMAIN_PASSWORD_INFORMATION
    {

        /// <summary>
        ///  MinPasswordLength member.
        /// </summary>
        public ushort MinPasswordLength;

        /// <summary>
        ///  PasswordHistoryLength member.
        /// </summary>
        public ushort PasswordHistoryLength;

        /// <summary>
        ///  PasswordProperties member.
        /// </summary>
        public uint PasswordProperties;

        /// <summary>
        ///  MaxPasswordAge member.
        /// </summary>
        public _OLD_LARGE_INTEGER MaxPasswordAge;

        /// <summary>
        ///  MinPasswordAge member.
        /// </summary>
        public _OLD_LARGE_INTEGER MinPasswordAge;
    }

    /// <summary>
    ///  The DOMAIN_LOGOFF_INFORMATION structure contains domain
    ///  fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_4_6.xml
    //  </remarks>
    public partial struct _DOMAIN_LOGOFF_INFORMATION
    {

        /// <summary>
        ///  ForceLogoff member.
        /// </summary>
        public _OLD_LARGE_INTEGER ForceLogoff;
    }

    /// <summary>
    ///  The DOMAIN_MODIFIED_INFORMATION structure contains domain
    ///  fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_4_8.xml
    //  </remarks>
    public partial struct _DOMAIN_MODIFIED_INFORMATION
    {

        /// <summary>
        ///  DomainModifiedCount member.
        /// </summary>
        public _OLD_LARGE_INTEGER DomainModifiedCount;

        /// <summary>
        ///  CreationTime member.
        /// </summary>
        public _OLD_LARGE_INTEGER CreationTime;
    }

    /// <summary>
    ///  The DOMAIN_MODIFIED_INFORMATION2 structure contains
    ///  domain fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_4_9.xml
    //  </remarks>
    public partial struct _DOMAIN_MODIFIED_INFORMATION2
    {

        /// <summary>
        ///  DomainModifiedCount member.
        /// </summary>
        public _OLD_LARGE_INTEGER DomainModifiedCount;

        /// <summary>
        ///  CreationTime member.
        /// </summary>
        public _OLD_LARGE_INTEGER CreationTime;

        /// <summary>
        ///  ModifiedCountAtLastPromotion member.
        /// </summary>
        public _OLD_LARGE_INTEGER ModifiedCountAtLastPromotion;
    }

    /// <summary>
    ///  The SAMPR_GROUP_GENERAL_INFORMATION structure contains
    ///  group fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_5_3.xml
    //  </remarks>
    public partial struct _SAMPR_GROUP_GENERAL_INFORMATION
    {

        /// <summary>
        ///  Name member.
        /// </summary>
        public _RPC_UNICODE_STRING Name;

        /// <summary>
        ///  Attributes member.
        /// </summary>
        public uint Attributes;

        /// <summary>
        ///  MemberCount member.
        /// </summary>
        public uint MemberCount;

        /// <summary>
        ///  AdminComment member.
        /// </summary>
        public _RPC_UNICODE_STRING AdminComment;
    }

    /// <summary>
    ///  The SAMPR_GROUP_NAME_INFORMATION structure contains
    ///  group fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_5_4.xml
    //  </remarks>
    public partial struct _SAMPR_GROUP_NAME_INFORMATION
    {

        /// <summary>
        ///  Name member.
        /// </summary>
        public _RPC_UNICODE_STRING Name;
    }

    /// <summary>
    ///  The SAMPR_GROUP_ADM_COMMENT_INFORMATION structure contains
    ///  group fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_5_5.xml
    //  </remarks>
    public partial struct _SAMPR_GROUP_ADM_COMMENT_INFORMATION
    {

        /// <summary>
        ///  AdminComment member.
        /// </summary>
        public _RPC_UNICODE_STRING AdminComment;
    }

    /// <summary>
    ///  The SAMPR_GROUP_INFO_BUFFER union combines all possible
    ///  structures used in the SamrSetInformationGroup and
    ///  SamrQueryInformationGroup methods. For information
    ///  on each field, with the exception of the DoNotUse field,
    ///  see the associated section for the field structure.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_5_7.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct _SAMPR_GROUP_INFO_BUFFER
    {

        /// <summary>
        ///  General member.
        /// </summary>
        [Case("1")]
        public _SAMPR_GROUP_GENERAL_INFORMATION General;

        /// <summary>
        ///  Name member.
        /// </summary>
        [Case("2")]
        public _SAMPR_GROUP_NAME_INFORMATION Name;

        /// <summary>
        ///  Attribute member.
        /// </summary>
        [Case("3")]
        public _GROUP_ATTRIBUTE_INFORMATION Attribute;

        /// <summary>
        ///  AdminComment member.
        /// </summary>
        [Case("4")]
        public _SAMPR_GROUP_ADM_COMMENT_INFORMATION AdminComment;

        /// <summary>
        ///  This field exists to allow the GroupReplicationInformation
        ///  enumeration to be specified by the client.As specified
        ///  in section , the General field (instead of DoNotUse)
        ///  MUST be used by the server when GroupReplicationInformation
        ///  is received. GroupReplicationInformation is not valid
        ///  for a set operation.
        /// </summary>
        [Case("5")]
        public _SAMPR_GROUP_GENERAL_INFORMATION DoNotUse;
    }

    /// <summary>
    ///  The SAMPR_ALIAS_GENERAL_INFORMATION structure contains
    ///  alias fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_6_2.xml
    //  </remarks>
    public partial struct _SAMPR_ALIAS_GENERAL_INFORMATION
    {

        /// <summary>
        ///  Name member.
        /// </summary>
        public _RPC_UNICODE_STRING Name;

        /// <summary>
        ///  MemberCount member.
        /// </summary>
        public uint MemberCount;

        /// <summary>
        ///  AdminComment member.
        /// </summary>
        public _RPC_UNICODE_STRING AdminComment;
    }

    /// <summary>
    ///  The SAMPR_ALIAS_NAME_INFORMATION structure contains
    ///  alias fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_6_3.xml
    //  </remarks>
    public partial struct _SAMPR_ALIAS_NAME_INFORMATION
    {

        /// <summary>
        ///  Name member.
        /// </summary>
        public _RPC_UNICODE_STRING Name;
    }

    /// <summary>
    ///  The SAMPR_ALIAS_ADM_COMMENT_INFORMATION structure contains
    ///  alias fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_6_4.xml
    //  </remarks>
    public partial struct _SAMPR_ALIAS_ADM_COMMENT_INFORMATION
    {

        /// <summary>
        ///  AdminComment member.
        /// </summary>
        public _RPC_UNICODE_STRING AdminComment;
    }

    /// <summary>
    ///  The SAMPR_ALIAS_INFO_BUFFER union combines all possible
    ///  structures used in the SamrSetInformationAlias and
    ///  SamrQueryInformationAlias methods. For information
    ///  on each field, see the associated section for the field
    ///  structure.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_6_6.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct _SAMPR_ALIAS_INFO_BUFFER
    {

        /// <summary>
        ///  General member.
        /// </summary>
        [Case("1")]
        public _SAMPR_ALIAS_GENERAL_INFORMATION General;

        /// <summary>
        ///  Name member.
        /// </summary>
        [Case("2")]
        public _SAMPR_ALIAS_NAME_INFORMATION Name;

        /// <summary>
        ///  AdminComment member.
        /// </summary>
        [Case("3")]
        public _SAMPR_ALIAS_ADM_COMMENT_INFORMATION AdminComment;
    }

    /// <summary>
    ///  The SAMPR_USER_LOGON_INFORMATION structure contains
    ///  user fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_7_10.xml
    //  </remarks>
    public partial struct _SAMPR_USER_LOGON_INFORMATION
    {

        /// <summary>
        ///  UserName member.
        /// </summary>
        public _RPC_UNICODE_STRING UserName;

        /// <summary>
        ///  FullName member.
        /// </summary>
        public _RPC_UNICODE_STRING FullName;

        /// <summary>
        ///  UserId member.
        /// </summary>
        public uint UserId;

        /// <summary>
        ///  PrimaryGroupId member.
        /// </summary>
        public uint PrimaryGroupId;

        /// <summary>
        ///  HomeDirectory member.
        /// </summary>
        public _RPC_UNICODE_STRING HomeDirectory;

        /// <summary>
        ///  HomeDirectoryDrive member.
        /// </summary>
        public _RPC_UNICODE_STRING HomeDirectoryDrive;

        /// <summary>
        ///  ScriptPath member.
        /// </summary>
        public _RPC_UNICODE_STRING ScriptPath;

        /// <summary>
        ///  ProfilePath member.
        /// </summary>
        public _RPC_UNICODE_STRING ProfilePath;

        /// <summary>
        ///  WorkStations member.
        /// </summary>
        public _RPC_UNICODE_STRING WorkStations;

        /// <summary>
        ///  LastLogon member.
        /// </summary>
        public _OLD_LARGE_INTEGER LastLogon;

        /// <summary>
        ///  LastLogoff member.
        /// </summary>
        public _OLD_LARGE_INTEGER LastLogoff;

        /// <summary>
        ///  PasswordLastSet member.
        /// </summary>
        public _OLD_LARGE_INTEGER PasswordLastSet;

        /// <summary>
        ///  PasswordCanChange member.
        /// </summary>
        public _OLD_LARGE_INTEGER PasswordCanChange;

        /// <summary>
        ///  PasswordMustChange member.
        /// </summary>
        public _OLD_LARGE_INTEGER PasswordMustChange;

        /// <summary>
        ///  LogonHours member.
        /// </summary>
        public _SAMPR_LOGON_HOURS LogonHours;

        /// <summary>
        ///  BadPasswordCount member.
        /// </summary>
        public ushort BadPasswordCount;

        /// <summary>
        ///  LogonCount member.
        /// </summary>
        public ushort LogonCount;

        /// <summary>
        ///  UserAccountControl member.
        /// </summary>
        public uint UserAccountControl;
    }

    /// <summary>
    ///  The SAMPR_USER_ACCOUNT_INFORMATION structure contains
    ///  user fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_7_11.xml
    //  </remarks>
    public partial struct _SAMPR_USER_ACCOUNT_INFORMATION
    {

        /// <summary>
        ///  UserName member.
        /// </summary>
        public _RPC_UNICODE_STRING UserName;

        /// <summary>
        ///  FullName member.
        /// </summary>
        public _RPC_UNICODE_STRING FullName;

        /// <summary>
        ///  UserId member.
        /// </summary>
        public uint UserId;

        /// <summary>
        ///  PrimaryGroupId member.
        /// </summary>
        public uint PrimaryGroupId;

        /// <summary>
        ///  HomeDirectory member.
        /// </summary>
        public _RPC_UNICODE_STRING HomeDirectory;

        /// <summary>
        ///  HomeDirectoryDrive member.
        /// </summary>
        public _RPC_UNICODE_STRING HomeDirectoryDrive;

        /// <summary>
        ///  ScriptPath member.
        /// </summary>
        public _RPC_UNICODE_STRING ScriptPath;

        /// <summary>
        ///  ProfilePath member.
        /// </summary>
        public _RPC_UNICODE_STRING ProfilePath;

        /// <summary>
        ///  AdminComment member.
        /// </summary>
        public _RPC_UNICODE_STRING AdminComment;

        /// <summary>
        ///  WorkStations member.
        /// </summary>
        public _RPC_UNICODE_STRING WorkStations;

        /// <summary>
        ///  LastLogon member.
        /// </summary>
        public _OLD_LARGE_INTEGER LastLogon;

        /// <summary>
        ///  LastLogoff member.
        /// </summary>
        public _OLD_LARGE_INTEGER LastLogoff;

        /// <summary>
        ///  LogonHours member.
        /// </summary>
        public _SAMPR_LOGON_HOURS LogonHours;

        /// <summary>
        ///  BadPasswordCount member.
        /// </summary>
        public ushort BadPasswordCount;

        /// <summary>
        ///  LogonCount member.
        /// </summary>
        public ushort LogonCount;

        /// <summary>
        ///  PasswordLastSet member.
        /// </summary>
        public _OLD_LARGE_INTEGER PasswordLastSet;

        /// <summary>
        ///  AccountExpires member.
        /// </summary>
        public _OLD_LARGE_INTEGER AccountExpires;

        /// <summary>
        ///  UserAccountControl member.
        /// </summary>
        public uint UserAccountControl;
    }

    /// <summary>
    ///  The SAMPR_USER_A_NAME_INFORMATION structure contains
    ///  user fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_7_12.xml
    //  </remarks>
    public partial struct _SAMPR_USER_A_NAME_INFORMATION
    {

        /// <summary>
        ///  UserName member.
        /// </summary>
        public _RPC_UNICODE_STRING UserName;
    }

    /// <summary>
    ///  The SAMPR_USER_F_NAME_INFORMATION structure contains
    ///  user fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_7_13.xml
    //  </remarks>
    public partial struct _SAMPR_USER_F_NAME_INFORMATION
    {

        /// <summary>
        ///  FullName member.
        /// </summary>
        public _RPC_UNICODE_STRING FullName;
    }

    /// <summary>
    ///  The SAMPR_USER_NAME_INFORMATION structure contains user
    ///  fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_7_14.xml
    //  </remarks>
    public partial struct _SAMPR_USER_NAME_INFORMATION
    {

        /// <summary>
        ///  UserName member.
        /// </summary>
        public _RPC_UNICODE_STRING UserName;

        /// <summary>
        ///  FullName member.
        /// </summary>
        public _RPC_UNICODE_STRING FullName;
    }

    /// <summary>
    ///  The SAMPR_USER_HOME_INFORMATION structure contains user
    ///  fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_7_15.xml
    //  </remarks>
    public partial struct _SAMPR_USER_HOME_INFORMATION
    {

        /// <summary>
        ///  HomeDirectory member.
        /// </summary>
        public _RPC_UNICODE_STRING HomeDirectory;

        /// <summary>
        ///  HomeDirectoryDrive member.
        /// </summary>
        public _RPC_UNICODE_STRING HomeDirectoryDrive;
    }

    /// <summary>
    ///  The SAMPR_USER_SCRIPT_INFORMATION structure contains
    ///  user fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_7_16.xml
    //  </remarks>
    public partial struct _SAMPR_USER_SCRIPT_INFORMATION
    {

        /// <summary>
        ///  ScriptPath member.
        /// </summary>
        public _RPC_UNICODE_STRING ScriptPath;
    }

    /// <summary>
    ///  The SAMPR_USER_PROFILE_INFORMATION structure contains
    ///  user fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_7_17.xml
    //  </remarks>
    public partial struct _SAMPR_USER_PROFILE_INFORMATION
    {

        /// <summary>
        ///  ProfilePath member.
        /// </summary>
        public _RPC_UNICODE_STRING ProfilePath;
    }

    /// <summary>
    ///  The SAMPR_USER_ADMIN_COMMENT_INFORMATION structure contains
    ///  user fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_7_18.xml
    //  </remarks>
    public partial struct _SAMPR_USER_ADMIN_COMMENT_INFORMATION
    {

        /// <summary>
        ///  AdminComment member.
        /// </summary>
        public _RPC_UNICODE_STRING AdminComment;
    }

    /// <summary>
    ///  The SAMPR_USER_WORKSTATIONS_INFORMATION structure contains
    ///  user fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_7_19.xml
    //  </remarks>
    public partial struct _SAMPR_USER_WORKSTATIONS_INFORMATION
    {

        /// <summary>
        ///  WorkStations member.
        /// </summary>
        public _RPC_UNICODE_STRING WorkStations;
    }

    /// <summary>
    ///  The SAMPR_USER_LOGON_HOURS_INFORMATION structure contains
    ///  user fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_7_20.xml
    //  </remarks>
    public partial struct _SAMPR_USER_LOGON_HOURS_INFORMATION
    {

        /// <summary>
        ///  LogonHours member.
        /// </summary>
        public _SAMPR_LOGON_HOURS LogonHours;
    }

    /// <summary>
    ///  The USER_PWD_CHANGE_FAILURE_INFORMATION structure stores
    ///  information regarding failure to change a user's password.
    ///  This structure is reserved.This structure is used only
    ///  for the SamrUnicodeChangePasswordUser3 method. There
    ///  are no Windows clients that call this method.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_7_23_a.xml
    //  </remarks>
    public partial struct _USER_PWD_CHANGE_FAILURE_INFORMATION
    {

        /// <summary>
        ///  Value that indicates the reason that the new password
        ///  was not accepted. The following values are possible.
        /// </summary>
        public ExtendedFailureReason_Values ExtendedFailureReason;

        /// <summary>
        ///  Name of the filter DLL, returned as a UNICODE_STRING.
        ///  This member is valid only if SAM_PWD_CHANGE_FAILED_BY_FILTER
        ///  is returned in the ExtendedFailureReason member.
        /// </summary>
        public _RPC_UNICODE_STRING FilterModuleName;
    }

    /// <summary>
    /// Extended failure reason values for _USER_PWD_CHANGE_FAILURE_INFORMATION structure.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    public enum ExtendedFailureReason_Values : uint
    {

        /// <summary>
        ///  No error.
        /// </summary>
        SAM_PWD_CHANGE_NO_ERROR = 0,

        /// <summary>
        ///  New password does not meet the password minimum length
        ///  policy.
        /// </summary>
        SAM_PWD_CHANGE_PASSWORD_TOO_SHORT = 1,

        /// <summary>
        ///  New password is in the history.
        /// </summary>
        SAM_PWD_CHANGE_PWD_IN_HISTORY = 2,

        /// <summary>
        ///  Complexity check failed because the new password includes
        ///  the user's account name.
        /// </summary>
        SAM_PWD_CHANGE_USERNAME_IN_PASSWORD = 3,

        /// <summary>
        ///  Complexity check failed because the new password includes
        ///  the user's full name.
        /// </summary>
        SAM_PWD_CHANGE_FULLNAME_IN_PASSWORD = 4,

        /// <summary>
        ///  Complexity check failed because the new password is
        ///  not complex enough.
        /// </summary>
        SAM_PWD_CHANGE_NOT_COMPLEX = 5,

        /// <summary>
        ///  Only the default password is allowed. This restriction
        ///  is set by the domain using the DOMAIN_REFUSE_PASSWORD_CHANGE
        ///  flag in the DOMAIN_PASSWORD_INFORMATION structure.
        /// </summary>
        SAM_PWD_CHANGE_MACHINE_PASSWORD_NOT_DEFAULT = 6,

        /// <summary>
        ///  New password failed to pass through the filter. The
        ///  FilterModuleName member provides the name of the filter
        ///  dynamic-link library (DLL).
        /// </summary>
        SAM_PWD_CHANGE_FAILED_BY_FILTER = 7,

        /// <summary>
        ///  New password does not meet the password maximum length
        ///  policy.
        /// </summary>
        SAM_PWD_CHANGE_PASSWORD_TOO_LONG = 8,
    }

    /// <summary>
    ///  The SAMPR_USER_INTERNAL1_INFORMATION structure holds
    ///  the hashed form of a clear text password.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_7_24.xml
    //  </remarks>
    public partial struct _SAMPR_USER_INTERNAL1_INFORMATION
    {

        /// <summary>
        ///  An NT hash encrypted with the 16-byte SMB [MS-SMB] session
        ///  key for the connection established by the underlying
        ///  authentication protocol (either Kerberos [MS-KILE]
        ///  or NTLM [MS-NLMP]).
        /// </summary>
        public _ENCRYPTED_LM_OWF_PASSWORD EncryptedNtOwfPassword;

        /// <summary>
        ///  An LM hash encrypted with the 16-byte SMB [MS-SMB] session
        ///  key for the connection established by the underlying
        ///  authentication protocol (either Kerberos [MS-KILE]
        ///  or NTLM [MS-NLMP]).
        /// </summary>
        public _ENCRYPTED_LM_OWF_PASSWORD EncryptedLmOwfPassword;

        /// <summary>
        ///  If nonzero, indicates that the EncryptedNtOwfPassword
        ///  value is valid; otherwise, EncryptedNtOwfPassword MUST
        ///  be ignored.
        /// </summary>
        public byte NtPasswordPresent;

        /// <summary>
        ///  If nonzero, indicates that the EncryptedLmOwfPassword
        ///  value is valid; otherwise, EncryptedLmOwfPassword MUST
        ///  be ignored.
        /// </summary>
        public byte LmPasswordPresent;

        /// <summary>
        ///  See section.
        /// </summary>
        public byte PasswordExpired;
    }

    /// <summary>
    ///  The SAMPR_USER_INTERNAL5_INFORMATION structure holds
    ///  an encrypted password.This structure is used to carry
    ///  a new password for a particular account from the client
    ///  to the server, encrypted in a way that protects it
    ///  from disclosure or tampering while in transit. The
    ///  encryption method that is used requires the client
    ///  to encrypt the clear text password and the server to
    ///  decrypt the password, allowing the server to verify
    ///  that the password meets implementation-specific policies
    ///  such as complexity requirements and excluded words.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_7_27.xml
    //  </remarks>
    public partial struct _SAMPR_USER_INTERNAL5_INFORMATION
    {

        /// <summary>
        ///  A clear text password, encrypted according to the specification
        ///  for SAMPR_ENCRYPTED_USER_PASSWORD, with the encryption
        ///  key being the 16-byte SMB [MS-SMB] session key established
        ///  by the underlying authentication protocol (either Kerberos
        ///  [MS-KILE] or NTLM [MS-NLMP]).
        /// </summary>
        public _SAMPR_ENCRYPTED_USER_PASSWORD UserPassword;

        /// <summary>
        ///  See section.
        /// </summary>
        public byte PasswordExpired;
    }

    /// <summary>
    ///  The SAMPR_USER_INTERNAL5_INFORMATION_NEW structure communicates
    ///  an encrypted password. The encrypted password uses
    ///  a salt to improve the encryption algorithm. See the
    ///  specification for SAMPR_ENCRYPTED_USER_PASSWORD_NEW
    ///  for details on salt value selection.This structure
    ///  is used to carry a new password for a particular account
    ///  from the client to the server, encrypted in a way that
    ///  protects it from disclosure or tampering while in transit.
    ///  A random value, a salt, is used by the client to seed
    ///  the encryption routine; see section  for details. The
    ///  encryption method that is used requires the client
    ///  to encrypt the clear text password and the server to
    ///  decrypt the password, allowing the server to verify
    ///  that the password meets implementation-specific policies
    ///  such as complexity requirements and excluded words.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_7_28.xml
    //  </remarks>
    public partial struct _SAMPR_USER_INTERNAL5_INFORMATION_NEW
    {

        /// <summary>
        ///  A password, encrypted according to the specification
        ///  for SAMPR_ENCRYPTED_USER_PASSWORD_NEW, with the encryption
        ///  key being the 16-byte SMB [MS-SMB] session key established
        ///  by the underlying authentication protocol (either Kerberos
        ///  [MS-KILE] or NTLM [MS-NLMP]).
        /// </summary>
        public _SAMPR_ENCRYPTED_USER_PASSWORD_NEW UserPassword;

        /// <summary>
        ///  See section.
        /// </summary>
        public byte PasswordExpired;
    }

    /// <summary>
    ///  The USER_EXPIRES_INFORMATION structure contains user
    ///  fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_7_4.xml
    //  </remarks>
    public partial struct _USER_EXPIRES_INFORMATION
    {

        /// <summary>
        ///  AccountExpires member.
        /// </summary>
        public _OLD_LARGE_INTEGER AccountExpires;
    }

    /// <summary>
    ///  The SAMPR_USER_ALL_INFORMATION structure contains user
    ///  attribute information. Most fields are described in
    ///  section. The exceptions are described as follows.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_7_6.xml
    //  </remarks>
    public partial struct _SAMPR_USER_ALL_INFORMATION
    {

        /// <summary>
        ///  LastLogon member.
        /// </summary>
        public _OLD_LARGE_INTEGER LastLogon;

        /// <summary>
        ///  LastLogoff member.
        /// </summary>
        public _OLD_LARGE_INTEGER LastLogoff;

        /// <summary>
        ///  PasswordLastSet member.
        /// </summary>
        public _OLD_LARGE_INTEGER PasswordLastSet;

        /// <summary>
        ///  AccountExpires member.
        /// </summary>
        public _OLD_LARGE_INTEGER AccountExpires;

        /// <summary>
        ///  PasswordCanChange member.
        /// </summary>
        public _OLD_LARGE_INTEGER PasswordCanChange;

        /// <summary>
        ///  PasswordMustChange member.
        /// </summary>
        public _OLD_LARGE_INTEGER PasswordMustChange;

        /// <summary>
        ///  UserName member.
        /// </summary>
        public _RPC_UNICODE_STRING UserName;

        /// <summary>
        ///  FullName member.
        /// </summary>
        public _RPC_UNICODE_STRING FullName;

        /// <summary>
        ///  HomeDirectory member.
        /// </summary>
        public _RPC_UNICODE_STRING HomeDirectory;

        /// <summary>
        ///  HomeDirectoryDrive member.
        /// </summary>
        public _RPC_UNICODE_STRING HomeDirectoryDrive;

        /// <summary>
        ///  ScriptPath member.
        /// </summary>
        public _RPC_UNICODE_STRING ScriptPath;

        /// <summary>
        ///  ProfilePath member.
        /// </summary>
        public _RPC_UNICODE_STRING ProfilePath;

        /// <summary>
        ///  AdminComment member.
        /// </summary>
        public _RPC_UNICODE_STRING AdminComment;

        /// <summary>
        ///  WorkStations member.
        /// </summary>
        public _RPC_UNICODE_STRING WorkStations;

        /// <summary>
        ///  UserComment member.
        /// </summary>
        public _RPC_UNICODE_STRING UserComment;

        /// <summary>
        ///  Parameters member.
        /// </summary>
        public _RPC_UNICODE_STRING Parameters;

        /// <summary>
        ///  An RPC_UNICODE_STRING structure where the Length and
        ///  MaximumLength MUST be 16 bytes, and the Buffer MUST
        ///  be formatted with an ENCRYPTED_LM_OWF_PASSWORD structure
        ///  with the clear text value being an LM hash, and the
        ///  encryption key being the 16-byte SMB [MS-SMB] session
        ///  key established by the underlying authentication protocol
        ///  (either Kerberos [MS-KILE] or NTLM [MS-NLMP]).
        /// </summary>
        public _RPC_UNICODE_STRING LmOwfPassword;

        /// <summary>
        ///  An RPC_UNICODE_STRING structure where the Length and
        ///  MaximumLength MUST be 16 bytes, and the Buffer MUST
        ///  be formatted with an ENCRYPTED_NT_OWF_PASSWORD structure
        ///  with the clear text value being an NT hash, and the
        ///  encryption key being the 16-byte SMB [MS-SMB] session
        ///  key established by the underlying authentication protocol
        ///  (either Kerberos [MS-KILE] or NTLM [MS-NLMP]).
        /// </summary>
        public _RPC_UNICODE_STRING NtOwfPassword;

        /// <summary>
        ///  Not used. Ignored on receipt at the server and client.
        ///  Clients MUST set to zero on send and servers MUST set
        ///  to zero on return.
        /// </summary>
        public _RPC_UNICODE_STRING PrivateData;

        /// <summary>
        ///  Not used. Ignored on receipt at the server and client.
        ///  Clients MUST set to zero on send and servers MUST set
        ///  to zero on return.
        /// </summary>
        public _SAMPR_SR_SECURITY_DESCRIPTOR SecurityDescriptor;

        /// <summary>
        ///  UserId member.
        /// </summary>
        public uint UserId;

        /// <summary>
        ///  PrimaryGroupId member.
        /// </summary>
        public uint PrimaryGroupId;

        /// <summary>
        ///  UserAccountControl member.
        /// </summary>
        public uint UserAccountControl;

        /// <summary>
        ///  A 32-bit bit field indicating which fields within the
        ///  SAMPR_USER_ALL_INFORMATION structure must be processed
        ///  by the server and which fields must be ignored by the
        ///  server. Section  specifies the valid bits and also
        ///  specifies the structure field to which each bit corresponds.If
        ///  a given bit is set, the associated field MUST be processed;
        ///  if a given bit is not set, then the associated field
        ///  MUST be ignored.
        /// </summary>
        public uint WhichFields;

        /// <summary>
        ///  LogonHours member.
        /// </summary>
        public _SAMPR_LOGON_HOURS LogonHours;

        /// <summary>
        ///  BadPasswordCount member.
        /// </summary>
        public ushort BadPasswordCount;

        /// <summary>
        ///  LogonCount member.
        /// </summary>
        public ushort LogonCount;

        /// <summary>
        ///  CountryCode member.
        /// </summary>
        public ushort CountryCode;

        /// <summary>
        ///  CodePage member.
        /// </summary>
        public ushort CodePage;

        /// <summary>
        ///  If zero, LmOwfPassword MUST be ignored; otherwise, LmOwfPassword
        ///  MUST be processed.
        /// </summary>
        public byte LmPasswordPresent;

        /// <summary>
        ///  If zero, NtOwfPassword MUST be ignored; otherwise, NtOwfPassword
        ///  MUST be processed.
        /// </summary>
        public byte NtPasswordPresent;

        /// <summary>
        ///  PasswordExpired member.
        /// </summary>
        public byte PasswordExpired;

        /// <summary>
        ///  Not used. Ignored on receipt at the server and client.
        /// </summary>
        public byte PrivateDataSensitive;
    }

    /// <summary>
    ///  The SAMPR_USER_GENERAL_INFORMATION structure contains
    ///  user fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_7_7.xml
    //  </remarks>
    public partial struct _SAMPR_USER_GENERAL_INFORMATION
    {

        /// <summary>
        ///  UserName member.
        /// </summary>
        public _RPC_UNICODE_STRING UserName;

        /// <summary>
        ///  FullName member.
        /// </summary>
        public _RPC_UNICODE_STRING FullName;

        /// <summary>
        ///  PrimaryGroupId member.
        /// </summary>
        public uint PrimaryGroupId;

        /// <summary>
        ///  AdminComment member.
        /// </summary>
        public _RPC_UNICODE_STRING AdminComment;

        /// <summary>
        ///  UserComment member.
        /// </summary>
        public _RPC_UNICODE_STRING UserComment;
    }

    /// <summary>
    ///  The SAMPR_USER_PREFERENCES_INFORMATION structure contains
    ///  user fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_7_8.xml
    //  </remarks>
    public partial struct _SAMPR_USER_PREFERENCES_INFORMATION
    {

        /// <summary>
        ///  UserComment member.
        /// </summary>
        public _RPC_UNICODE_STRING UserComment;

        /// <summary>
        ///  Ignored by the client and server and MUST be a zero-length
        ///  string on send and return.
        /// </summary>
        public _RPC_UNICODE_STRING Reserved1;

        /// <summary>
        ///  CountryCode member.
        /// </summary>
        public ushort CountryCode;

        /// <summary>
        ///  CodePage member.
        /// </summary>
        public ushort CodePage;
    }

    /// <summary>
    ///  The SAMPR_USER_PARAMETERS_INFORMATION structure contains
    ///  user fields.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_7_9.xml
    //  </remarks>
    public partial struct _SAMPR_USER_PARAMETERS_INFORMATION
    {

        /// <summary>
        ///  Parameters member.
        /// </summary>
        public _RPC_UNICODE_STRING Parameters;
    }

    /// <summary>
    ///  The SAMPR_DOMAIN_DISPLAY_USER structure contains a subset
    ///  of user account information sufficient to show a summary
    ///  of the account for an account management application.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_8_2.xml
    //  </remarks>
    public partial struct _SAMPR_DOMAIN_DISPLAY_USER
    {

        /// <summary>
        ///  Index member.
        /// </summary>
        public uint Index;

        /// <summary>
        ///  Rid member.
        /// </summary>
        public uint Rid;

        /// <summary>
        ///  AccountControl member.
        /// </summary>
        public uint AccountControl;

        /// <summary>
        ///  AccountName member.
        /// </summary>
        public _RPC_UNICODE_STRING AccountName;

        /// <summary>
        ///  AdminComment member.
        /// </summary>
        public _RPC_UNICODE_STRING AdminComment;

        /// <summary>
        ///  FullName member.
        /// </summary>
        public _RPC_UNICODE_STRING FullName;
    }

    /// <summary>
    ///  The SAMPR_DOMAIN_DISPLAY_MACHINE structure contains
    ///  a subset of machine account information sufficient
    ///  to show a summary of the account for an account management
    ///  application.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_8_3.xml
    //  </remarks>
    public partial struct _SAMPR_DOMAIN_DISPLAY_MACHINE
    {

        /// <summary>
        ///  Index member.
        /// </summary>
        public uint Index;

        /// <summary>
        ///  Rid member.
        /// </summary>
        public uint Rid;

        /// <summary>
        ///  AccountControl member.
        /// </summary>
        public uint AccountControl;

        /// <summary>
        ///  AccountName member.
        /// </summary>
        public _RPC_UNICODE_STRING AccountName;

        /// <summary>
        ///  AdminComment member.
        /// </summary>
        public _RPC_UNICODE_STRING AdminComment;
    }

    /// <summary>
    ///  The SAMPR_DOMAIN_DISPLAY_GROUP structure contains a
    ///  subset of group information sufficient to show a summary
    ///  of the account for an account management application.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_8_4.xml
    //  </remarks>
    public partial struct _SAMPR_DOMAIN_DISPLAY_GROUP
    {

        /// <summary>
        ///  Index member.
        /// </summary>
        public uint Index;

        /// <summary>
        ///  Rid member.
        /// </summary>
        public uint Rid;

        /// <summary>
        ///  Attributes member.
        /// </summary>
        public uint Attributes;

        /// <summary>
        ///  AccountName member.
        /// </summary>
        public _RPC_UNICODE_STRING AccountName;

        /// <summary>
        ///  AdminComment member.
        /// </summary>
        public _RPC_UNICODE_STRING AdminComment;
    }

    /// <summary>
    ///  The SAMPR_DOMAIN_DISPLAY_OEM_USER structure contains
    ///  a subset of user account information sufficient to
    ///  show a summary of the account for an account management
    ///  application. This structure exists to support nonUnicode-based
    ///  systems.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_8_5.xml
    //  </remarks>
    public partial struct _SAMPR_DOMAIN_DISPLAY_OEM_USER
    {

        /// <summary>
        ///  Index member.
        /// </summary>
        public uint Index;

        /// <summary>
        ///  OemAccountName member.
        /// </summary>
        public _RPC_STRING OemAccountName;
    }

    /// <summary>
    ///  The SAMPR_DOMAIN_DISPLAY_OEM_GROUP structure contains
    ///  a subset of group information sufficient to show a
    ///  summary of the account for an account management application.
    ///  This structure exists to support nonUnicode-based systems.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_8_6.xml
    //  </remarks>
    public partial struct _SAMPR_DOMAIN_DISPLAY_OEM_GROUP
    {

        /// <summary>
        ///  Index member.
        /// </summary>
        public uint Index;

        /// <summary>
        ///  OemAccountName member.
        /// </summary>
        public _RPC_STRING OemAccountName;
    }

    /// <summary>
    ///  The SAMPR_DOMAIN_DISPLAY_USER_BUFFER structure holds
    ///  an array of SAMPR_DOMAIN_DISPLAY_USER elements used
    ///  to return a list of users through the SamrQueryDisplayInformation
    ///  family of methods (see section ).
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_8_7.xml
    //  </remarks>
    public partial struct _SAMPR_DOMAIN_DISPLAY_USER_BUFFER
    {

        /// <summary>
        ///  The number of elements in Buffer. If zero, Buffer MUST
        ///  be ignored. If nonzero, Buffer MUST point to at least
        ///  EntriesRead number of elements.
        /// </summary>
        public uint EntriesRead;

        /// <summary>
        ///  An array of SAMPR_DOMAIN_DISPLAY_USER elements.
        /// </summary>
        [Size("EntriesRead")]
        public _SAMPR_DOMAIN_DISPLAY_USER[] Buffer;
    }

    /// <summary>
    ///  The SAMPR_DOMAIN_DISPLAY_MACHINE_BUFFER structure holds
    ///  an array of SAMPR_DOMAIN_DISPLAY_MACHINE elements used
    ///  to return a list of machine accounts through the SamrQueryDisplayInformation
    ///  family of methods (see section ).
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_8_8.xml
    //  </remarks>
    public partial struct _SAMPR_DOMAIN_DISPLAY_MACHINE_BUFFER
    {

        /// <summary>
        ///  The number of elements in Buffer. If zero, Buffer MUST
        ///  be ignored. If nonzero, Buffer MUST point to at least
        ///  EntriesRead number of elements.
        /// </summary>
        public uint EntriesRead;

        /// <summary>
        ///  An array of SAMPR_DOMAIN_DISPLAY_MACHINE elements.
        /// </summary>
        [Size("EntriesRead")]
        public _SAMPR_DOMAIN_DISPLAY_MACHINE[] Buffer;
    }

    /// <summary>
    ///  The SAMPR_DOMAIN_DISPLAY_GROUP_BUFFER structure holds
    ///  an array of SAMPR_DOMAIN_DISPLAY_GROUP elements used
    ///  to return a list of groups through the SamrQueryDisplayInformation
    ///  family of methods (see section ).
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_8_9.xml
    //  </remarks>
    public partial struct _SAMPR_DOMAIN_DISPLAY_GROUP_BUFFER
    {

        /// <summary>
        ///  The number of elements in Buffer. If zero, Buffer MUST
        ///  be ignored. If nonzero, Buffer MUST point to at least
        ///  EntriesRead number of elements.
        /// </summary>
        public uint EntriesRead;

        /// <summary>
        ///  An array of SAMPR_DOMAIN_DISPLAY_GROUP elements.
        /// </summary>
        [Size("EntriesRead")]
        public _SAMPR_DOMAIN_DISPLAY_GROUP[] Buffer;
    }

    /// <summary>
    ///  The SAM_VALIDATE_PERSISTED_FIELDS structure holds various
    ///  characteristics about password state.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_9_2.xml
    //  </remarks>
    public partial struct _SAM_VALIDATE_PERSISTED_FIELDS
    {

        /// <summary>
        ///  A bitmask to indicate which of the fields are valid.
        ///  The following table shows the defined values. If a
        ///  bit is set, the corresponding field is valid; if a
        ///  bit is not set, the field is not valid.
        /// </summary>
        public PresentFields_Values PresentFields;

        /// <summary>
        ///  This field represents the time at which the password
        ///  was last reset or changed. It uses FILETIME syntax.
        /// </summary>
        public _LARGE_INTEGER PasswordLastSet;

        /// <summary>
        ///  This field represents the time at which an invalid password
        ///  was presented to either a password change request or
        ///  an authentication request. It uses FILETIME syntax.
        /// </summary>
        public _LARGE_INTEGER BadPasswordTime;

        /// <summary>
        ///  This field represents the time at which the owner of
        ///  the password data was locked out. It uses FILETIME
        ///  syntax.
        /// </summary>
        public _LARGE_INTEGER LockoutTime;

        /// <summary>
        ///  Indicates how many invalid passwords have accumulated
        ///  (see message processing for details).
        /// </summary>
        public uint BadPasswordCount;

        /// <summary>
        ///  Indicates how many previous passwords are in the PasswordHistory
        ///  field.
        /// </summary>
        public uint PasswordHistoryLength;

        /// <summary>
        ///  An array of hash values representing the previous PasswordHistoryLength
        ///  passwords.
        /// </summary>
        [Size("PasswordHistoryLength")]
        public _SAM_VALIDATE_PASSWORD_HASH[] PasswordHistory;
    }

    /// <summary>
    /// Present field values for _SAM_VALIDATE_PERSISTED_FIELDS structure.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    public enum PresentFields_Values : uint
    {

        /// <summary>
        ///  PasswordLastSet
        /// </summary>
        SAM_VALIDATE_PASSWORD_LAST_SET = 0x00000001,

        /// <summary>
        ///  BadPasswordTime
        /// </summary>
        SAM_VALIDATE_BAD_PASSWORD_TIME = 0x00000002,

        /// <summary>
        ///  LockoutTime
        /// </summary>
        SAM_VALIDATE_LOCKOUT_TIME = 0x00000004,

        /// <summary>
        ///  BadPasswordCount
        /// </summary>
        SAM_VALIDATE_BAD_PASSWORD_COUNT = 0x00000008,

        /// <summary>
        ///  PasswordHistoryLength
        /// </summary>
        SAM_VALIDATE_PASSWORD_HISTORY_LENGTH = 0x00000010,

        /// <summary>
        ///  PasswordHistory
        /// </summary>
        SAM_VALIDATE_PASSWORD_HISTORY = 0x00000020,
    }

    /// <summary>
    ///  The SAM_VALIDATE_STANDARD_OUTPUT_ARG structure holds
    ///  the output of SamrValidatePassword.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_9_4.xml
    //  </remarks>
    public partial struct _SAM_VALIDATE_STANDARD_OUTPUT_ARG
    {

        /// <summary>
        ///  The password state that has changed. See section.
        /// </summary>
        public _SAM_VALIDATE_PERSISTED_FIELDS ChangedPersistedFields;

        /// <summary>
        ///  The result of the policy evaluation. See section.
        /// </summary>
        public _SAM_VALIDATE_VALIDATION_STATUS ValidationStatus;
    }

    /// <summary>
    ///  The SAM_VALIDATE_AUTHENTICATION_INPUT_ARG structure
    ///  holds information about an authentication request.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_9_5.xml
    //  </remarks>
    public partial struct _SAM_VALIDATE_AUTHENTICATION_INPUT_ARG
    {

        /// <summary>
        ///  Password state.
        /// </summary>
        public _SAM_VALIDATE_PERSISTED_FIELDS InputPersistedFields;

        /// <summary>
        ///  A nonzero value indicates that a valid password was
        ///  presented to the change-password request.
        /// </summary>
        public byte PasswordMatched;

    }

    /// <summary>
    ///  The SAM_VALIDATE_PASSWORD_CHANGE_INPUT_ARG structure
    ///  holds information about a password change request.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_9_6.xml
    //  </remarks>
    public partial struct _SAM_VALIDATE_PASSWORD_CHANGE_INPUT_ARG
    {

        /// <summary>
        ///  Password state. See section.
        /// </summary>
        public _SAM_VALIDATE_PERSISTED_FIELDS InputPersistedFields;

        /// <summary>
        ///  The clear text password of the change-password operation.
        /// </summary>
        public _RPC_UNICODE_STRING ClearPassword;

        /// <summary>
        ///  The application-specific logon name of an account performing
        ///  the change-password operation.
        /// </summary>
        public _RPC_UNICODE_STRING UserAccountName;

        /// <summary>
        ///  A binary value containing a hashed form of the value
        ///  contained in the ClearPassword field. The structure
        ///  of this binary value is specified in section. The
        ///  hash function used to generate this value is chosen
        ///  by the client. An example hash function might be MD5
        ///  (as specified in [RFC1321]). The server implementation
        ///  is independent of that choice; that is, through this
        ///  protocol, the server is exposed to a sequence of bytes
        ///  formatted per section  and is, therefore, not exposed
        ///  to the hash function chosen by the client. Furthermore,
        ///  there is no processing by the server that requires
        ///  knowledge of the specific hash function chosen. Section
        ///   contains more information about a scenario in which
        ///  this field is used.
        /// </summary>
        public _SAM_VALIDATE_PASSWORD_HASH HashedPassword;

        /// <summary>
        ///  A nonzero value indicates that a valid password was
        ///  presented to the change-password request.
        /// </summary>
        public byte PasswordMatch;

    }

    /// <summary>
    ///  The SAM_VALIDATE_PASSWORD_RESET_INPUT_ARG structure
    ///  holds various information about a password reset request.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_9_7.xml
    //  </remarks>
    public partial struct _SAM_VALIDATE_PASSWORD_RESET_INPUT_ARG
    {

        /// <summary>
        ///  Password state. See section.
        /// </summary>
        public _SAM_VALIDATE_PERSISTED_FIELDS InputPersistedFields;

        /// <summary>
        ///  The clear text password of the reset-password operation.
        /// </summary>
        public _RPC_UNICODE_STRING ClearPassword;

        /// <summary>
        ///  The application-specific logon name of the account performing
        ///  the reset-password operation.
        /// </summary>
        public _RPC_UNICODE_STRING UserAccountName;

        /// <summary>
        ///  See the specification for SAM_VALIDATE_PASSWORD_CHANGE_INPUT_ARG
        ///  for the field with the same name.
        /// </summary>
        public _SAM_VALIDATE_PASSWORD_HASH HashedPassword;

        /// <summary>
        ///  Nonzero indicates that a password change MUST occur
        ///  before an authentication request can succeed.
        /// </summary>
        public byte PasswordMustChangeAtNextLogon;

        /// <summary>
        ///  Nonzero indicates that the lockout state should be reset.
        /// </summary>
        public byte ClearLockout;
    }

    /// <summary>
    ///  The SAM_VALIDATE_INPUT_ARG union holds the various input
    ///  types to SamrValidatePassword.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_9_9.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct _SAM_VALIDATE_INPUT_ARG
    {

        /// <summary>
        ///   </summary>
        [Case("1")]
        public _SAM_VALIDATE_AUTHENTICATION_INPUT_ARG ValidateAuthenticationInput;

        /// <summary>
        ///   </summary>
        [Case("2")]
        public _SAM_VALIDATE_PASSWORD_CHANGE_INPUT_ARG ValidatePasswordChangeInput;

        /// <summary>
        ///   </summary>
        [Case("3")]
        public _SAM_VALIDATE_PASSWORD_RESET_INPUT_ARG ValidatePasswordResetInput;
    }

    /// <summary>
    ///  The SAMPR_DOMAIN_INFO_BUFFER union combines all possible
    ///  structures used in the SamrSetInformationDomain and
    ///  SamrQueryInformationDomain methods. For details on
    ///  each field, see the associated section for each field
    ///  structure.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_4_17.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct _SAMPR_DOMAIN_INFO_BUFFER
    {

        /// <summary>
        ///  Password member.
        /// </summary>
        [Case("1")]
        public _DOMAIN_PASSWORD_INFORMATION Password;

        /// <summary>
        ///  General member.
        /// </summary>
        [Case("2")]
        public _SAMPR_DOMAIN_GENERAL_INFORMATION General;

        /// <summary>
        ///  Logoff member.
        /// </summary>
        [Case("3")]
        public _DOMAIN_LOGOFF_INFORMATION Logoff;

        /// <summary>
        ///  Oem member.
        /// </summary>
        [Case("4")]
        public _SAMPR_DOMAIN_OEM_INFORMATION Oem;

        /// <summary>
        ///  Name member.
        /// </summary>
        [Case("5")]
        public _SAMPR_DOMAIN_NAME_INFORMATION Name;

        /// <summary>
        ///  Role member.
        /// </summary>
        [Case("7")]
        public _DOMAIN_SERVER_ROLE_INFORMATION Role;

        /// <summary>
        ///  Replication member.
        /// </summary>
        [Case("6")]
        public SAMPR_DOMAIN_REPLICATION_INFORMATION Replication;

        /// <summary>
        ///  Modified member.
        /// </summary>
        [Case("8")]
        public _DOMAIN_MODIFIED_INFORMATION Modified;

        /// <summary>
        ///  State member.
        /// </summary>
        [Case("9")]
        public _DOMAIN_STATE_INFORMATION State;

        /// <summary>
        ///  General2 member.
        /// </summary>
        [Case("11")]
        public _SAMPR_DOMAIN_GENERAL_INFORMATION2 General2;

        /// <summary>
        ///  Lockout member.
        /// </summary>
        [Case("12")]
        public _SAMPR_DOMAIN_LOCKOUT_INFORMATION Lockout;

        /// <summary>
        ///  Modified2 member.
        /// </summary>
        [Case("13")]
        public _DOMAIN_MODIFIED_INFORMATION2 Modified2;
    }

    /// <summary>
    ///  The SAMPR_USER_INTERNAL4_INFORMATION structure holds
    ///  all attributes of a user, along with an encrypted password.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_7_25.xml
    //  </remarks>
    public partial struct _SAMPR_USER_INTERNAL4_INFORMATION
    {

        /// <summary>
        ///  See section.
        /// </summary>
        public _SAMPR_USER_ALL_INFORMATION I1;

        /// <summary>
        ///  See section.
        /// </summary>
        public _SAMPR_ENCRYPTED_USER_PASSWORD UserPassword;
    }

    /// <summary>
    ///  The SAMPR_USER_INTERNAL4_INFORMATION_NEW structure holds
    ///  all attributes of a user, along with an encrypted password.
    ///  The encrypted password uses a salt to improve the encryption
    ///  algorithm. See the specification for SAMPR_ENCRYPTED_USER_PASSWORD_NEW
    ///  for details on salt value selection.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_7_26.xml
    //  </remarks>
    public partial struct _SAMPR_USER_INTERNAL4_INFORMATION_NEW
    {

        /// <summary>
        ///  See section.
        /// </summary>
        public _SAMPR_USER_ALL_INFORMATION I1;

        /// <summary>
        ///  See section.
        /// </summary>
        public _SAMPR_ENCRYPTED_USER_PASSWORD_NEW UserPassword;
    }

    /// <summary>
    ///  The SAMPR_USER_INFO_BUFFER union combines all possible
    ///  structures used in the SamrSetInformationUser and SamrQueryInformationUser
    ///  methods (see sections , ,  and ). For details on each
    ///  field, see the associated section for the field structure.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_7_30.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct _SAMPR_USER_INFO_BUFFER
    {

        /// <summary>
        ///  General member.
        /// </summary>
        [Case("1")]
        public _SAMPR_USER_GENERAL_INFORMATION General;

        /// <summary>
        ///  Preferences member.
        /// </summary>
        [Case("2")]
        public _SAMPR_USER_PREFERENCES_INFORMATION Preferences;

        /// <summary>
        ///  Logon member.
        /// </summary>
        [Case("3")]
        public _SAMPR_USER_LOGON_INFORMATION Logon;

        /// <summary>
        ///  LogonHours member.
        /// </summary>
        [Case("4")]
        public _SAMPR_USER_LOGON_HOURS_INFORMATION LogonHours;

        /// <summary>
        ///  Account member.
        /// </summary>
        [Case("5")]
        public _SAMPR_USER_ACCOUNT_INFORMATION Account;

        /// <summary>
        ///  Name member.
        /// </summary>
        [Case("6")]
        public _SAMPR_USER_NAME_INFORMATION Name;

        /// <summary>
        ///  AccountName member.
        /// </summary>
        [Case("7")]
        public _SAMPR_USER_A_NAME_INFORMATION AccountName;

        /// <summary>
        ///  FullName member.
        /// </summary>
        [Case("8")]
        public _SAMPR_USER_F_NAME_INFORMATION FullName;

        /// <summary>
        ///  PrimaryGroup member.
        /// </summary>
        [Case("9")]
        public _USER_PRIMARY_GROUP_INFORMATION PrimaryGroup;

        /// <summary>
        ///  Home member.
        /// </summary>
        [Case("10")]
        public _SAMPR_USER_HOME_INFORMATION Home;

        /// <summary>
        ///  Script member.
        /// </summary>
        [Case("11")]
        public _SAMPR_USER_SCRIPT_INFORMATION Script;

        /// <summary>
        ///  Profile member.
        /// </summary>
        [Case("12")]
        public _SAMPR_USER_PROFILE_INFORMATION Profile;

        /// <summary>
        ///  AdminComment member.
        /// </summary>
        [Case("13")]
        public _SAMPR_USER_ADMIN_COMMENT_INFORMATION AdminComment;

        /// <summary>
        ///  WorkStations member.
        /// </summary>
        [Case("14")]
        public _SAMPR_USER_WORKSTATIONS_INFORMATION WorkStations;

        /// <summary>
        ///  Control member.
        /// </summary>
        [Case("16")]
        public _USER_CONTROL_INFORMATION Control;

        /// <summary>
        ///  Expires member.
        /// </summary>
        [Case("17")]
        public _USER_EXPIRES_INFORMATION Expires;

        /// <summary>
        ///  Internal1 member.
        /// </summary>
        [Case("18")]
        public _SAMPR_USER_INTERNAL1_INFORMATION Internal1;

        /// <summary>
        ///  Parameters member.
        /// </summary>
        [Case("20")]
        public _SAMPR_USER_PARAMETERS_INFORMATION Parameters;

        /// <summary>
        ///  All member.
        /// </summary>
        [Case("21")]
        public _SAMPR_USER_ALL_INFORMATION All;

        /// <summary>
        ///  Internal4 member.
        /// </summary>
        [Case("23")]
        public _SAMPR_USER_INTERNAL4_INFORMATION Internal4;

        /// <summary>
        ///  Internal5 member.
        /// </summary>
        [Case("24")]
        public _SAMPR_USER_INTERNAL5_INFORMATION Internal5;

        /// <summary>
        ///  Internal4New member.
        /// </summary>
        [Case("25")]
        public _SAMPR_USER_INTERNAL4_INFORMATION_NEW Internal4New;

        /// <summary>
        ///  Internal5New member.
        /// </summary>
        [Case("26")]
        public _SAMPR_USER_INTERNAL5_INFORMATION_NEW Internal5New;
    }

    /// <summary>
    ///  The SAMPR_DOMAIN_DISPLAY_OEM_USER_BUFFER structure holds
    ///  an array of SAMPR_DOMAIN_DISPLAY_OEM_USER elements
    ///  used to return a list of users through the SamrQueryDisplayInformation
    ///  family of methods (see section ).
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_8_10.xml
    //  </remarks>
    public partial struct _SAMPR_DOMAIN_DISPLAY_OEM_USER_BUFFER
    {

        /// <summary>
        ///  The number of elements in Buffer. If zero, Buffer MUST
        ///  be ignored. If nonzero, Buffer MUST point to at least
        ///  EntriesRead number of elements.
        /// </summary>
        public uint EntriesRead;

        /// <summary>
        ///  An array of SAMPR_DOMAIN_DISPLAY_OEM_USER elements.
        /// </summary>
        [Size("EntriesRead")]
        public _SAMPR_DOMAIN_DISPLAY_OEM_USER[] Buffer;
    }

    /// <summary>
    ///  The SAMPR_DOMAIN_DISPLAY_OEM_GROUP_BUFFER structure
    ///  holds an array of SAMPR_DOMAIN_DISPLAY_OEM_GROUP elements
    ///  used to return a list of user accounts through the
    ///  SamrQueryDisplayInformation family of methods (see
    ///  section ).
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_8_11.xml
    //  </remarks>
    public partial struct _SAMPR_DOMAIN_DISPLAY_OEM_GROUP_BUFFER
    {

        /// <summary>
        ///  The number of elements in Buffer. If zero, Buffer MUST
        ///  be ignored. If nonzero, Buffer MUST point to at least
        ///  EntriesRead number of elements.
        /// </summary>
        public uint EntriesRead;

        /// <summary>
        ///  An array of SAMPR_DOMAIN_DISPLAY_OEM_GROUP elements.
        /// </summary>
        [Size("EntriesRead")]
        public _SAMPR_DOMAIN_DISPLAY_OEM_GROUP[] Buffer;
    }

    /// <summary>
    ///  The SAMPR_DISPLAY_INFO_BUFFER union is a union of display
    ///  structures returned by the SamrQueryDisplayInformation
    ///  family of methods (see section ). For details on each
    ///  field, see the associated section for the field structure.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_8_13.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct _SAMPR_DISPLAY_INFO_BUFFER
    {

        /// <summary>
        ///   </summary>
        [Case("1")]
        public _SAMPR_DOMAIN_DISPLAY_USER_BUFFER UserInformation;

        /// <summary>
        ///   </summary>
        [Case("2")]
        public _SAMPR_DOMAIN_DISPLAY_MACHINE_BUFFER MachineInformation;

        /// <summary>
        ///   </summary>
        [Case("3")]
        public _SAMPR_DOMAIN_DISPLAY_GROUP_BUFFER GroupInformation;

        /// <summary>
        ///   </summary>
        [Case("4")]
        public _SAMPR_DOMAIN_DISPLAY_OEM_USER_BUFFER OemUserInformation;

        /// <summary>
        ///   </summary>
        [Case("5")]
        public _SAMPR_DOMAIN_DISPLAY_OEM_GROUP_BUFFER OemGroupInformation;
    }

    /// <summary>
    ///  The SAM_VALIDATE_OUTPUT_ARG union holds the output of
    ///  SamrValidatePassword.
    /// </summary>
    //  <remarks>
    //   file:///E:/Working_Dir/ts_dev/TestSuites/MS-SAMR/Docs/TD-XML/SAMR/WSPP/_rfc_ms-samr2_2_9_10.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct _SAM_VALIDATE_OUTPUT_ARG
    {

        /// <summary>
        ///   </summary>
        [Case("1")]
        public _SAM_VALIDATE_STANDARD_OUTPUT_ARG ValidateAuthenticationOutput;

        /// <summary>
        ///   </summary>
        [Case("2")]
        public _SAM_VALIDATE_STANDARD_OUTPUT_ARG ValidatePasswordChangeOutput;

        /// <summary>
        ///   </summary>
        [Case("3")]
        public _SAM_VALIDATE_STANDARD_OUTPUT_ARG ValidatePasswordResetOutput;
    }

    /// <summary>
    /// Possible security information values.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    public enum SecurityInformation_Values : uint
    {

        /// <summary>
        ///  Refers to the Owner member of the security descriptor.
        /// </summary>
        OWNER_SECURITY_INFORMATION = 0x00000001,

        /// <summary>
        ///  Refers to the Group member of the security descriptor.
        /// </summary>
        GROUP_SECURITY_INFORMATION = 0x00000002,

        /// <summary>
        ///  Refers to the DACL of the security descriptor.
        /// </summary>
        DACL_SECURITY_INFORMATION = 0x00000004,

        /// <summary>
        ///  Refers to the SACL of the security descriptor.
        /// </summary>
        SACL_SECURITY_INFORMATION = 0x00000008,

        /// <summary>
        /// The mandatory integrity label is being referenced. 
        /// </summary>
        LABEL_SECURITY_INFORMATION = 0X00000010,

        /// <summary>
        /// The SACL inherits access control entries (ACEs) from the parent object. 
        /// </summary>
        UNPROTECTED_SACL_SECURITY_INFORMATION = 0x10000000,

        /// <summary>
        /// The DACL inherits ACEs from the parent object. 
        /// </summary>
        UNPROTECTED_DACL_SECURITY_INFORMATION = 0x20000000,

        /// <summary>
        /// The SACL cannot inherit ACEs. 
        /// </summary>
        PROTECTED_SACL_SECURITY_INFORMATION = 0x40000000,

        /// <summary>
        /// The DACL cannot inherit ACEs. 
        /// </summary>
        PROTECTED_DACL_SECURITY_INFORMATION = 0x80000000
    }

    /// <summary>
    /// Possible security information values for SamrQuerySecurityObject method.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    public enum SamrQuerySecurityObject_SecurityInformation_Values : uint
    {

        /// <summary>
        ///  If this bit is set, the client requests that the Owner
        ///  member be returned. If this bit is not set, the client
        ///  requests that the Owner member not be returned.
        /// </summary>
        OWNER_SECURITY_INFORMATION = 0x00000001,

        /// <summary>
        ///  If this bit is set, the client requests that the Group
        ///  member be returned. If this bit is not set, the client
        ///  requests that the Group member not be returned.
        /// </summary>
        GROUP_SECURITY_INFORMATION = 0x00000002,

        /// <summary>
        ///  If this bit is set, the client requests that the DACL
        ///  be returned. If this bit is not set, the client requests
        ///  that the DACL not be returned.
        /// </summary>
        DACL_SECURITY_INFORMATION = 0x00000004,

        /// <summary>
        ///  If this bit is set, the client requests that the SACL
        ///  be returned. If this bit is not set, the client requests
        ///  that the SACL not be returned.
        /// </summary>
        SACL_SECURITY_INFORMATION = 0x00000008,

        /// <summary>
        /// The mandatory integrity label is being referenced. 
        /// </summary>
        LABEL_SECURITY_INFORMATION = 0X00000010,

        /// <summary>
        /// The SACL inherits access control entries (ACEs) from the parent object. 
        /// </summary>
        UNPROTECTED_SACL_SECURITY_INFORMATION = 0x10000000,

        /// <summary>
        /// The DACL inherits ACEs from the parent object. 
        /// </summary>
        UNPROTECTED_DACL_SECURITY_INFORMATION = 0x20000000,

        /// <summary>
        /// The SACL cannot inherit ACEs. 
        /// </summary>
        PROTECTED_SACL_SECURITY_INFORMATION = 0x40000000,

        /// <summary>
        /// The DACL cannot inherit ACEs. 
        /// </summary>
        PROTECTED_DACL_SECURITY_INFORMATION = 0x80000000
    }

    /// <summary>
    /// ISamrRpcAdapter, provides managed RPC interfaces of MS-SAMR.
    /// </summary>
    public partial interface ISamrRpcAdapter : IDisposable
    {
        /// <summary>
        /// RPC handle.
        /// </summary>
        IntPtr Handle
        {
            get;
        }

        /// <summary>
        /// Gets session ke
        /// </summary>
        byte[] SessionKey
        {
            get;
        }

        /// <summary>
        /// Bind to SAMR RPC server.
        /// </summary>
        /// <param name="protocolSequence">
        /// RPC protocol sequence.
        /// </param>
        /// <param name="networkAddress">
        /// RPC network address.
        /// </param>
        /// <param name="endpoint">
        /// RPC endpoint.
        /// </param>
        /// <param name="transportCredential">
        /// If connect by SMB/SMB2, it's the security credential 
        /// used by under layer transport (SMB/SMB2). 
        /// If connect by TCP, this parameter is ignored.
        /// </param>
        /// <param name="securityContext">
        /// RPC security provider.
        /// </param>
        /// <param name="authenticationLevel">
        /// RPC authentication level.
        /// </param>
        /// <param name="timeout">
        /// Timeout
        /// </param>
        void Bind(
            string protocolSequence,
            string networkAddress,
            string endpoint,
            AccountCredential transportCredential,
            ClientSecurityContext securityContext,
            RpceAuthenticationLevel authenticationLevel,
            TimeSpan timeout);

        /// <summary>
        /// RPC unbind.
        /// </summary>
        void Unbind();

        /// <summary>
        ///  The SamrConnect method returns a handle to a server
        ///  object. Opnum: 0 
        /// </summary>
        /// <param name="ServerName">
        ///  The first character of the NETBIOS name of the responder;
        ///  this parameter MAY ServerName is ignored on receipt.
        ///   be ignored on receipt.
        /// </param>
        /// <param name="ServerHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <param name="DesiredAccess">
        ///  An ACCESS_MASK that indicates the access requested for
        ///  ServerHandle upon output. See section  for a listing
        ///  of possible values.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrConnect(string ServerName, out System.IntPtr ServerHandle, uint DesiredAccess);

        /// <summary>
        ///  The SamrCloseHandle method closes (that is, releases
        ///  server-side resources used by) any context handle obtained
        ///  from this RPC interface. Opnum: 1 
        /// </summary>
        /// <param name="SamHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  any context handle returned from this interface.
        /// </param>
        ///<returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrCloseHandle(ref IntPtr SamHandle);

        /// <summary>
        ///  The SamrSetSecurityObject method sets the access control
        ///  on a server, domain, user, group, or alias object.
        ///  Opnum: 2 
        /// </summary>
        /// <param name="ObjectHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a server, domain, user, group, or alias object.
        /// </param>
        /// <param name="SecurityInformation">
        ///  A bit field that indicates the fields of SecurityDescriptor
        ///  that are requested to be set. The SECURITY_INFORMATION
        ///  type is defined in [MS-DTYP] section. The following
        ///  bits are valid; all other bits MUST be zero on send
        ///  and ignored on receipt.
        /// </param>
        /// <param name="SecurityDescriptor">
        ///  A security descriptor expressing access that is specific
        ///  to the ObjectHandle.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrSetSecurityObject(System.IntPtr ObjectHandle, SecurityInformation_Values SecurityInformation, _SAMPR_SR_SECURITY_DESCRIPTOR? SecurityDescriptor);

        /// <summary>
        ///  The SamrQuerySecurityObject method queries the access
        ///  control on a server, domain, user, group, or alias
        ///  object. Opnum: 3 
        /// </summary>
        /// <param name="ObjectHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a server, domain, user, group, or alias object.
        /// </param>
        /// <param name="SecurityInformation">
        ///  A bit field that specifies which fields of SecurityDescriptor
        ///  the client is requesting to be returned. The SECURITY_INFORMATION
        ///  type is defined in [MS-DTYP] section. The following
        ///  bits are valid; all other bits MUST be zero on send
        ///  and ignored on receipt.
        /// </param>
        /// <param name="SecurityDescriptor">
        ///  A security descriptor expressing accesses that are specific
        ///  to the ObjectHandle and the owner and group of the
        ///  object. [MS-DTYP] section  contains the specification
        ///  for a valid security descriptor.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrQuerySecurityObject(System.IntPtr ObjectHandle, SamrQuerySecurityObject_SecurityInformation_Values SecurityInformation, out _SAMPR_SR_SECURITY_DESCRIPTOR? SecurityDescriptor);

        /// <summary>
        ///  Reserved for local use. Opnum: 4 
        /// </summary>
        void Opnum4NotUsedOnWire();

        /// <summary>
        ///  The SamrLookupDomainInSamServer method obtains the SID
        ///  of a domain object, given the object's name. Opnum
        ///  : 5 
        /// </summary>
        /// <param name="ServerHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a server object.
        /// </param>
        /// <param name="Name">
        ///  A UTF-16 encoded string.
        /// </param>
        /// <param name="DomainId">
        ///  A SID value of a domain that matches the Name passed
        ///  in. The match must be exact (no wildcards are permitted).
        ///  See message processing later in this section for more
        ///  details.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrLookupDomainInSamServer(System.IntPtr ServerHandle, _RPC_UNICODE_STRING Name, out _RPC_SID? DomainId);

        /// <summary>
        ///  The SamrEnumerateDomainsInSamServer method obtains a
        ///  listing of all domains hosted by the server side of
        ///  this protocol. Opnum: 6 
        /// </summary>
        /// <param name="ServerHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a server object.
        /// </param>
        /// <param name="EnumerationContext">
        ///  This value is a cookie that the server can use to continue
        ///  an enumeration on a subsequent call. It is an opaque
        ///  value to the client.
        /// </param>
        /// <param name="Buffer">
        ///  A listing of domain information, as described in section
        ///  .
        /// </param>
        /// <param name="PreferedMaximumLength">
        ///  The requested maximum number of bytes to return in Buffer.
        /// </param>
        /// <param name="CountReturned">
        ///  The count of domain elements returned in Buffer.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrEnumerateDomainsInSamServer(System.IntPtr ServerHandle, ref System.UInt32? EnumerationContext, out _SAMPR_ENUMERATION_BUFFER? Buffer, uint PreferedMaximumLength, out System.UInt32 CountReturned);

        /// <summary>
        ///  The SamrOpenDomain method obtains a handle to a domain
        ///  object, given a SID. Opnum: 7 
        /// </summary>
        /// <param name="ServerHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a server object.
        /// </param>
        /// <param name="DesiredAccess">
        ///  An ACCESS_MASK. See section  for a list of domain access
        ///  values.
        /// </param>
        /// <param name="DomainId">
        ///  A SID value of a domain hosted by the server side of
        ///  this protocol.
        /// </param>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrOpenDomain(System.IntPtr ServerHandle, uint DesiredAccess, _RPC_SID? DomainId, out System.IntPtr DomainHandle);

        /// <summary>
        ///  The SamrQueryInformationDomain method obtains attributes
        ///  from a domain object. Opnum: 8 
        /// </summary>
        /// <param name="DomainHandle">
        ///  DomainHandle parameter.
        /// </param>
        /// <param name="DomainInformationClass">
        ///  DomainInformationClass parameter.
        /// </param>
        /// <param name="Buffer">
        ///  Buffer parameter.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrQueryInformationDomain(System.IntPtr DomainHandle, _DOMAIN_INFORMATION_CLASS DomainInformationClass, [Switch("DomainInformationClass")] out _SAMPR_DOMAIN_INFO_BUFFER? Buffer);

        /// <summary>
        ///  The SamrSetInformationDomain method updates attributes
        ///  on a domain object. Opnum: 9 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="DomainInformationClass">
        ///  An enumeration indicating which attributes to update.
        ///  See section  for a list of possible values.
        /// </param>
        /// <param name="DomainInformation">
        ///  The requested attributes and values to update. See section
        ///   for structure details.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrSetInformationDomain(System.IntPtr DomainHandle, _DOMAIN_INFORMATION_CLASS DomainInformationClass, [Switch("DomainInformationClass")] _SAMPR_DOMAIN_INFO_BUFFER DomainInformation);

        /// <summary>
        ///  The SamrCreateGroupInDomain method creates a group object
        ///  within a domain. Opnum: 10 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="Name">
        ///  The value to use as the name of the group. Details on
        ///  how this value maps to the data model are provided
        ///  later in this section.
        /// </param>
        /// <param name="DesiredAccess">
        ///  The access requested on the GroupHandle on output. See
        ///  section  for a listing of possible values.
        /// </param>
        /// <param name="GroupHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <param name="RelativeId">
        ///  The RID of the newly created group.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrCreateGroupInDomain(System.IntPtr DomainHandle, _RPC_UNICODE_STRING Name, uint DesiredAccess, out System.IntPtr GroupHandle, out System.UInt32 RelativeId);

        /// <summary>
        ///  The SamrEnumerateGroupsInDomain method enumerates all
        ///  groups. Opnum: 11 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="EnumerationContext">
        ///  This value is a cookie that the server can use to continue
        ///  an enumeration on a subsequent call. It is an opaque
        ///  value to the client.
        /// </param>
        /// <param name="Buffer">
        ///  A list of group information, as specified in section
        ///  .
        /// </param>
        /// <param name="PreferedMaximumLength">
        ///  The requested maximum number of bytes to return in Buffer.
        /// </param>
        /// <param name="CountReturned">
        ///  The count of domain elements returned in Buffer.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrEnumerateGroupsInDomain(System.IntPtr DomainHandle, ref System.UInt32? EnumerationContext, out _SAMPR_ENUMERATION_BUFFER? Buffer, uint PreferedMaximumLength, out System.UInt32 CountReturned);

        /// <summary>
        ///  The SamrCreateUserInDomain method creates a user. Opnum
        ///  : 12 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="Name">
        ///  The value to use as the name of the user. See the message
        ///  processing shown later in this section for details
        ///  on how this value maps to the data model.
        /// </param>
        /// <param name="DesiredAccess">
        ///  The access requested on the UserHandle on output. See
        ///  section  for a listing of possible values.
        /// </param>
        /// <param name="UserHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <param name="RelativeId">
        ///  The RID of the newly created user.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrCreateUserInDomain(System.IntPtr DomainHandle, _RPC_UNICODE_STRING Name, uint DesiredAccess, out System.IntPtr UserHandle, out System.UInt32 RelativeId);

        /// <summary>
        ///  The SamrEnumerateUsersInDomain method enumerates all
        ///  users. Opnum: 13 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="EnumerationContext">
        ///  This value is a cookie that the server can use to continue
        ///  an enumeration on a subsequent call. It is an opaque
        ///  value to the client.
        /// </param>
        /// <param name="UserAccountControl">
        ///  A filter value to be used on the userAccountControl
        ///  attribute.
        /// </param>
        /// <param name="Buffer">
        ///  A list of user information, as specified in section
        ///  .
        /// </param>
        /// <param name="PreferedMaximumLength">
        ///  The requested maximum number of bytes to return in Buffer.
        /// </param>
        /// <param name="CountReturned">
        ///  The count of domain elements returned in Buffer.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrEnumerateUsersInDomain(System.IntPtr DomainHandle, ref System.UInt32? EnumerationContext, uint UserAccountControl, out _SAMPR_ENUMERATION_BUFFER? Buffer, uint PreferedMaximumLength, out System.UInt32 CountReturned);

        /// <summary>
        ///  The SamrCreateAliasInDomain method creates an alias.
        ///  Opnum: 14 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="AccountName">
        ///  The value to use as the name of the alias. Details on
        ///  how this value maps to the data model are provided
        ///  later in this section.
        /// </param>
        /// <param name="DesiredAccess">
        ///  The access requested on the AliasHandle on output. See
        ///  section  for a listing of possible values.
        /// </param>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <param name="RelativeId">
        ///  The RID of the newly created alias.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrCreateAliasInDomain(System.IntPtr DomainHandle, _RPC_UNICODE_STRING AccountName, uint DesiredAccess, out System.IntPtr AliasHandle, out System.UInt32 RelativeId);

        /// <summary>
        ///  The SamrEnumerateAliasesInDomain method enumerates all
        ///  aliases. Opnum: 15 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="EnumerationContext">
        ///  This value is a cookie that the server can use to continue
        ///  an enumeration on a subsequent call. It is an opaque
        ///  value to the client.
        /// </param>
        /// <param name="Buffer">
        ///  A list of alias information, as specified in section
        ///  .
        /// </param>
        /// <param name="PreferedMaximumLength">
        ///  The requested maximum number of bytes to return in Buffer.
        /// </param>
        /// <param name="CountReturned">
        ///  The count of domain elements returned in Buffer.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrEnumerateAliasesInDomain(System.IntPtr DomainHandle, ref System.UInt32? EnumerationContext, out _SAMPR_ENUMERATION_BUFFER? Buffer, uint PreferedMaximumLength, out System.UInt32 CountReturned);

        /// <summary>
        ///  The SamrGetAliasMembership method obtains the union
        ///  of all aliases that a given set of SIDs is a member
        ///  of. Opnum: 16 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="SidArray">
        ///  A list of SIDs.
        /// </param>
        /// <param name="Membership">
        ///  The union of all aliases (represented by RIDs) that
        ///  all SIDs in SidArray are a member of.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrGetAliasMembership(System.IntPtr DomainHandle, _SAMPR_PSID_ARRAY SidArray, out _SAMPR_ULONG_ARRAY Membership);

        /// <summary>
        ///  The SamrLookupNamesInDomain method translates a set
        ///  of account names into a set of RIDs. Opnum: 17 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="Count">
        ///  The number of elements in Names. The maximum value of
        ///  1,000 is chosen to limit the amount of memory that
        ///  the client can force the server to allocate.
        /// </param>
        /// <param name="Names">
        ///  An array of strings that are to be mapped to RIDs.
        /// </param>
        /// <param name="RelativeIds">
        ///  An array of RIDs of accounts that correspond to the
        ///  elements in Names.
        /// </param>
        /// <param name="Use">
        ///  An array of SID_NAME_USE enumeration values that describe
        ///  the type of account for each entry in RelativeIds.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrLookupNamesInDomain(System.IntPtr DomainHandle, uint Count, [Length("Count")] [Size("1000")] _RPC_UNICODE_STRING[] Names, out _SAMPR_ULONG_ARRAY RelativeIds, out _SAMPR_ULONG_ARRAY Use);

        /// <summary>
        ///  The SamrLookupIdsInDomain method translates a set of
        ///  RIDs into account names. Opnum: 18 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="Count">
        ///  The number of elements in RelativeIds. The maximum value
        ///  of 1,000 is chosen to limit the amount of memory that
        ///  the client can force the server to allocate.
        /// </param>
        /// <param name="RelativeIds">
        ///  An array of RIDs that are to be mapped to account names.
        /// </param>
        /// <param name="Names">
        ///  A structure containing an array of account names that
        ///  correspond to the elements in RelativeIds.
        /// </param>
        /// <param name="Use">
        ///  A structure containing an array of SID_NAME_USE enumeration
        ///  values that describe the type of account for each entry
        ///  in RelativeIds.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrLookupIdsInDomain(System.IntPtr DomainHandle, uint Count, [Length("Count")] [Size("1000")] uint[] RelativeIds, out _SAMPR_RETURNED_USTRING_ARRAY Names, out _SAMPR_ULONG_ARRAY Use);

        /// <summary>
        ///  The SamrOpenGroup method obtains a handle to a group,
        ///  given a RID. Opnum: 19 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="DesiredAccess">
        ///  An ACCESS_MASK that indicates the requested access for
        ///  the returned handle. See section  for a list of group
        ///  access values.
        /// </param>
        /// <param name="GroupId">
        ///  A RID of a group.
        /// </param>
        /// <param name="GroupHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrOpenGroup(System.IntPtr DomainHandle, uint DesiredAccess, uint GroupId, out System.IntPtr GroupHandle);

        /// <summary>
        ///  The SamrQueryInformationGroup method obtains attributes
        ///  from a group object. Opnum: 20 
        /// </summary>
        /// <param name="GroupHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a group object.
        /// </param>
        /// <param name="GroupInformationClass">
        ///  An enumeration indicating which attributes to return.
        ///  See section  for a listing of possible values.
        /// </param>
        /// <param name="Buffer">
        ///  The requested attributes on output. See section  for
        ///  structure details.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrQueryInformationGroup(System.IntPtr GroupHandle, _GROUP_INFORMATION_CLASS GroupInformationClass, [Switch("GroupInformationClass")] out _SAMPR_GROUP_INFO_BUFFER? Buffer);

        /// <summary>
        ///  The SamrSetInformationGroup method updates attributes
        ///  on a group object. Opnum: 21 
        /// </summary>
        /// <param name="GroupHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a group object.
        /// </param>
        /// <param name="GroupInformationClass">
        ///  An enumeration indicating which attributes to update.
        ///  See section  for a listing of possible values.
        /// </param>
        /// <param name="Buffer">
        ///  The requested attributes and values to update. See section
        ///   for structure details.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrSetInformationGroup(System.IntPtr GroupHandle, _GROUP_INFORMATION_CLASS GroupInformationClass, [Switch("GroupInformationClass")] _SAMPR_GROUP_INFO_BUFFER Buffer);

        /// <summary>
        ///  The SamrAddMemberToGroup method adds a member to a group.
        ///  Opnum: 22 
        /// </summary>
        /// <param name="GroupHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a group object.
        /// </param>
        /// <param name="MemberId">
        ///  A RID representing an account to add to the group's
        ///  membership list.
        /// </param>
        /// <param name="Attributes">
        ///  The characteristics of the membership relationship.
        ///  See section  for legal values and semantics.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrAddMemberToGroup(System.IntPtr GroupHandle, uint MemberId, uint Attributes);

        /// <summary>
        ///  The SamrDeleteGroup method removes a group object. Opnum
        ///  : 23 
        /// </summary>
        /// <param name="GroupHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a group object.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrDeleteGroup(ref System.IntPtr GroupHandle);

        /// <summary>
        ///  The SamrRemoveMemberFromGroup method removes a member
        ///  from a group. Opnum: 24 
        /// </summary>
        /// <param name="GroupHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a group object.
        /// </param>
        /// <param name="MemberId">
        ///  A RID representing an account to remove from the group's
        ///  membership list.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrRemoveMemberFromGroup(System.IntPtr GroupHandle, uint MemberId);

        /// <summary>
        ///  The SamrGetMembersInGroup method reads the members of
        ///  a group. Opnum: 25 
        /// </summary>
        /// <param name="GroupHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a group object.
        /// </param>
        /// <param name="Members">
        ///  A structure containing an array of RIDs, as well as
        ///  an array of attribute values.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrGetMembersInGroup(System.IntPtr GroupHandle, out _SAMPR_GET_MEMBERS_BUFFER? Members);

        /// <summary>
        ///  The SamrSetMemberAttributesOfGroup method sets the attributes
        ///  of a member relationship. Opnum: 26 
        /// </summary>
        /// <param name="GroupHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a group object.
        /// </param>
        /// <param name="MemberId">
        ///  A RID that represents a member of a group (which is
        ///  a user or machine account).
        /// </param>
        /// <param name="Attributes">
        ///  The characteristics of the membership relationship.
        ///  For legal values, see section.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrSetMemberAttributesOfGroup(System.IntPtr GroupHandle, uint MemberId, uint Attributes);

        /// <summary>
        ///  The SamrOpenAlias method obtains a handle to an alias,
        ///  given a RID. Opnum: 27 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="DesiredAccess">
        ///  An ACCESS_MASK that indicates the requested access for
        ///  the returned handle. See section  for a list of alias
        ///  access values.
        /// </param>
        /// <param name="AliasId">
        ///  A RID of an alias.
        /// </param>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrOpenAlias(System.IntPtr DomainHandle, uint DesiredAccess, uint AliasId, out System.IntPtr AliasHandle);

        /// <summary>
        ///  The SamrQueryInformationAlias method obtains attributes
        ///  from an alias object. Opnum: 28 
        /// </summary>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  an alias object.
        /// </param>
        /// <param name="AliasInformationClass">
        ///  An enumeration indicating which attributes to return.
        ///  See section  for a listing of possible values.
        /// </param>
        /// <param name="Buffer">
        ///  The requested attributes on output. See section  for
        ///  structure details.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrQueryInformationAlias(System.IntPtr AliasHandle, _ALIAS_INFORMATION_CLASS AliasInformationClass, [Switch("AliasInformationClass")] out _SAMPR_ALIAS_INFO_BUFFER? Buffer);

        /// <summary>
        ///  The SamrSetInformationAlias method  updates attributes
        ///  on an alias object. Opnum: 29 
        /// </summary>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  an alias object.
        /// </param>
        /// <param name="AliasInformationClass">
        ///  An enumeration indicating which attributes to update.
        ///  See section  for a listing of possible values.
        /// </param>
        /// <param name="Buffer">
        ///  The requested attributes and values to update. See section
        ///   for structure details.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrSetInformationAlias(System.IntPtr AliasHandle, _ALIAS_INFORMATION_CLASS AliasInformationClass, [Switch("AliasInformationClass")] _SAMPR_ALIAS_INFO_BUFFER Buffer);

        /// <summary>
        ///  The SamrDeleteAlias method removes an alias object.
        ///  Opnum: 30 
        /// </summary>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  an alias object.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrDeleteAlias(ref System.IntPtr AliasHandle);

        /// <summary>
        ///  The SamrAddMemberToAlias method adds a member to an
        ///  alias. Opnum: 31 
        /// </summary>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  an alias object.
        /// </param>
        /// <param name="MemberId">
        ///  The SID of an account to add to the alias.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrAddMemberToAlias(System.IntPtr AliasHandle, _RPC_SID MemberId);

        /// <summary>
        ///  The SamrRemoveMemberFromAlias method removes a member
        ///  from an alias. Opnum: 32 
        /// </summary>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  an alias object.
        /// </param>
        /// <param name="MemberId">
        ///  The SID of an account to remove from the alias.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrRemoveMemberFromAlias(System.IntPtr AliasHandle, _RPC_SID MemberId);

        /// <summary>
        ///  The SamrGetMembersInAlias method obtains the membership
        ///  list of an alias. Opnum: 33 
        /// </summary>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  an alias object.
        /// </param>
        /// <param name="Members">
        ///  A structure containing an array of SIDs that represent
        ///  the membership list of the alias referenced by AliasHandle.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrGetMembersInAlias(System.IntPtr AliasHandle, out _SAMPR_PSID_ARRAY Members);

        /// <summary>
        ///  The SamrOpenUser method obtains a handle to a user,
        ///  given a RID. Opnum: 34 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="DesiredAccess">
        ///  An ACCESS_MASK that indicates the requested access for
        ///  the returned handle. See section  for a list of user
        ///  access values.
        /// </param>
        /// <param name="UserId">
        ///  A RID of a user account.
        /// </param>
        /// <param name="UserHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrOpenUser(System.IntPtr DomainHandle, uint DesiredAccess, uint UserId, out System.IntPtr UserHandle);

        /// <summary>
        ///  The SamrDeleteUser method removes a user object. Opnum
        ///  : 35 
        /// </summary>
        /// <param name="UserHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a user object.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrDeleteUser(ref System.IntPtr UserHandle);

        /// <summary>
        ///  The SamrQueryInformationUser method obtains attributes
        ///  from a user object. Opnum: 36 
        /// </summary>
        /// <param name="UserHandle">
        ///  UserHandle parameter.
        /// </param>
        /// <param name="UserInformationClass">
        ///  UserInformationClass parameter.
        /// </param>
        /// <param name="Buffer">
        ///  Buffer parameter.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrQueryInformationUser(System.IntPtr UserHandle, _USER_INFORMATION_CLASS UserInformationClass, [Switch("UserInformationClass")] out _SAMPR_USER_INFO_BUFFER? Buffer);

        /// <summary>
        ///  The SamrSetInformationUser method updates attributes
        ///  on a user object. Opnum: 37 
        /// </summary>
        /// <param name="UserHandle">
        ///  UserHandle parameter.
        /// </param>
        /// <param name="UserInformationClass">
        ///  UserInformationClass parameter.
        /// </param>
        /// <param name="Buffer">
        ///  Buffer parameter.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrSetInformationUser(System.IntPtr UserHandle, _USER_INFORMATION_CLASS UserInformationClass, [Switch("UserInformationClass")] _SAMPR_USER_INFO_BUFFER Buffer);

        /// <summary>
        ///  The SamrChangePasswordUser method changes the password
        ///  of a user object. Opnum: 38 
        /// </summary>
        /// <param name="UserHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a user object.
        /// </param>
        /// <param name="LmPresent">
        ///  If this parameter is zero, the LmOldEncryptedWithLmNew
        ///  and LmNewEncryptedWithLmOld fields MUST be ignored
        ///  by the server; otherwise these fields MUST be processed.
        /// </param>
        /// <param name="OldLmEncryptedWithNewLm">
        ///  The LM hash of the target user's existing password (as
        ///  presented by the client) encrypted according to the
        ///  specification of ENCRYPTED_LM_OWF_PASSWORD, where the
        ///  key is the LM hash of the new password for the target
        ///  user (as presented by the client in the LmNewEncryptedWithLmOld
        ///  parameter).
        /// </param>
        /// <param name="NewLmEncryptedWithOldLm">
        ///  The LM hash of the target user's new password (as presented
        ///  by the client) encrypted according to the specification
        ///  of ENCRYPTED_LM_OWF_PASSWORD, where the key is the
        ///  LM hash of the existing password for the target user
        ///  (as presented by the client in the LmOldEncryptedWithLmNew
        ///  parameter).
        /// </param>
        /// <param name="NtPresent">
        ///  If this parameter is zero, NtOldEncryptedWithNtNew and
        ///  NtNewEncryptedWithNtOld MUST be ignored by the server;
        ///  otherwise these fields MUST be processed. 
        /// </param>
        /// <param name="OldNtEncryptedWithNewNt">
        ///  The NT hash of the target user's existing password (as
        ///  presented by the client) encrypted according to the
        ///  specification of  ENCRYPTED_NT_OWF_PASSWORD, where
        ///  the key is the NT hash of the new password for the
        ///  target user (as presented by the client).
        /// </param>
        /// <param name="NewNtEncryptedWithOldNt">
        ///  The NT hash of the target user's new password (as presented
        ///  by the client) encrypted according to the specification
        ///  of ENCRYPTED_NT_OWF_PASSWORD, where the key is the
        ///  NT hash of the existing password for the target user
        ///  (as presented by the client).
        /// </param>
        /// <param name="NtCrossEncryptionPresent">
        ///  If this parameter is zero, NtNewEncryptedWithLmNew MUST
        ///  be ignored; otherwise, this field MUST be processed.
        /// </param>
        /// <param name="NewNtEncryptedWithNewLm">
        ///  The NT hash of the target user's new password (as presented
        ///  by the client) encrypted according to the specification
        ///  of ENCRYPTED_NT_OWF_PASSWORD, where the key is the
        ///  LM hash of the new password for the target user (as
        ///  presented by the client).
        /// </param>
        /// <param name="LmCrossEncryptionPresent">
        ///  If this parameter is zero, LmNewEncryptedWithNtNew MUST
        ///  be ignored; otherwise, this field MUST be processed.
        /// </param>
        /// <param name="NewLmEncryptedWithNewNt">
        ///  The LM hash of the target user's new password (as presented
        ///  by the client) encrypted according to the specification
        ///  of ENCRYPTED_LM_OWF_PASSWORD, where the key is the
        ///  NT hash of the new password for the target user (as
        ///  presented by the client).
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrChangePasswordUser(System.IntPtr UserHandle, byte LmPresent, _ENCRYPTED_LM_OWF_PASSWORD? OldLmEncryptedWithNewLm, _ENCRYPTED_LM_OWF_PASSWORD? NewLmEncryptedWithOldLm, byte NtPresent, _ENCRYPTED_LM_OWF_PASSWORD? OldNtEncryptedWithNewNt, _ENCRYPTED_LM_OWF_PASSWORD? NewNtEncryptedWithOldNt, byte NtCrossEncryptionPresent, _ENCRYPTED_LM_OWF_PASSWORD? NewNtEncryptedWithNewLm, byte LmCrossEncryptionPresent, _ENCRYPTED_LM_OWF_PASSWORD? NewLmEncryptedWithNewNt);

        /// <summary>
        ///  The SamrGetGroupsForUser method obtains a listing of
        ///  groups that a user is a member of. Opnum: 39 
        /// </summary>
        /// <param name="UserHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a user object.
        /// </param>
        /// <param name="Groups">
        ///  An array of RIDs of the groups that the user referenced
        ///  by UserHandle is a member of.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrGetGroupsForUser(System.IntPtr UserHandle, out _SAMPR_GET_GROUPS_BUFFER? Groups);

        /// <summary>
        ///  The SamrQueryDisplayInformation method obtains a list
        ///  of accounts in name-sorted order, starting at a specified
        ///  index. Opnum: 40 
        /// </summary>
        /// <param name="DomainHandle">
        ///  DomainHandle parameter.
        /// </param>
        /// <param name="DisplayInformationClass">
        ///  DisplayInformationClass parameter.
        /// </param>
        /// <param name="Index">
        ///  Index parameter.
        /// </param>
        /// <param name="EntryCount">
        ///  EntryCount parameter.
        /// </param>
        /// <param name="PreferredMaximumLength">
        ///  PreferredMaximumLength parameter.
        /// </param>
        /// <param name="TotalAvailable">
        ///  TotalAvailable parameter.
        /// </param>
        /// <param name="TotalReturned">
        ///  TotalReturned parameter.
        /// </param>
        /// <param name="Buffer">
        ///  Buffer parameter.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrQueryDisplayInformation(System.IntPtr DomainHandle, _DOMAIN_DISPLAY_INFORMATION DisplayInformationClass, uint Index, uint EntryCount, uint PreferredMaximumLength, out System.UInt32 TotalAvailable, out System.UInt32 TotalReturned, [Switch("DisplayInformationClass")] out _SAMPR_DISPLAY_INFO_BUFFER Buffer);

        /// <summary>
        ///  The SamrGetDisplayEnumerationIndex method obtains an
        ///  index into an account-namesorted list of accounts.
        ///  Opnum: 41 
        /// </summary>
        /// <param name="DomainHandle">
        ///  DomainHandle parameter.
        /// </param>
        /// <param name="DisplayInformationClass">
        ///  DisplayInformationClass parameter.
        /// </param>
        /// <param name="Prefix">
        ///  Prefix parameter.
        /// </param>
        /// <param name="Index">
        ///  Index parameter.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrGetDisplayEnumerationIndex(System.IntPtr DomainHandle, _DOMAIN_DISPLAY_INFORMATION DisplayInformationClass, _RPC_UNICODE_STRING Prefix, out System.UInt32 Index);

        /// <summary>
        ///  Reserved for local use. Opnum: 42 
        /// </summary>
        void Opnum42NotUsedOnWire();

        /// <summary>
        ///  Reserved for local use. Opnum: 43 
        /// </summary>
        void Opnum43NotUsedOnWire();

        /// <summary>
        ///  The SamrGetUserDomainPasswordInformation method obtains
        ///  select password policy information (without requiring
        ///  a domain handle). Opnum: 44 
        /// </summary>
        /// <param name="UserHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a user object.
        /// </param>
        /// <param name="PasswordInformation">
        ///  Password policy information from the user's domain.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrGetUserDomainPasswordInformation(System.IntPtr UserHandle, out _USER_DOMAIN_PASSWORD_INFORMATION PasswordInformation);

        /// <summary>
        ///  The SamrRemoveMemberFromForeignDomain method removes
        ///  a member from all aliases. Opnum: 45 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="MemberSid">
        ///  The SID to remove from the membership.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrRemoveMemberFromForeignDomain(System.IntPtr DomainHandle, _RPC_SID? MemberSid);

        /// <summary>
        ///  The SamrQueryInformationDomain2 method obtains attributes
        ///  from a domain object. Opnum: 46 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="DomainInformationClass">
        ///  An enumeration indicating which attributes to return.
        ///  See section  for a listing of possible values.
        /// </param>
        /// <param name="Buffer">
        ///  The requested attributes on output. See section  for
        ///  structure details.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrQueryInformationDomain2(System.IntPtr DomainHandle, _DOMAIN_INFORMATION_CLASS DomainInformationClass, [Switch("DomainInformationClass")] out _SAMPR_DOMAIN_INFO_BUFFER? Buffer);

        /// <summary>
        ///  The SamrQueryInformationUser2 method obtains attributes
        ///  from a user object. Opnum: 47 
        /// </summary>
        /// <param name="UserHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a user object.
        /// </param>
        /// <param name="UserInformationClass">
        ///  An enumeration indicating which attributes to return.
        ///  See section  for a list of possible values.
        /// </param>
        /// <param name="Buffer">
        ///  The requested attributes on output. See section  for
        ///  structure details.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrQueryInformationUser2(System.IntPtr UserHandle, _USER_INFORMATION_CLASS UserInformationClass, [Switch("UserInformationClass")] out _SAMPR_USER_INFO_BUFFER? Buffer);

        /// <summary>
        ///  The SamrQueryDisplayInformation2 method obtains a list
        ///  of accounts in name-sorted order, starting at a specified
        ///  index. Opnum: 48 
        /// </summary>
        /// <param name="DomainHandle">
        ///  DomainHandle parameter.
        /// </param>
        /// <param name="DisplayInformationClass">
        ///  DisplayInformationClass parameter.
        /// </param>
        /// <param name="Index">
        ///  Index parameter.
        /// </param>
        /// <param name="EntryCount">
        ///  EntryCount parameter.
        /// </param>
        /// <param name="PreferredMaximumLength">
        ///  PreferredMaximumLength parameter.
        /// </param>
        /// <param name="TotalAvailable">
        ///  TotalAvailable parameter.
        /// </param>
        /// <param name="TotalReturned">
        ///  TotalReturned parameter.
        /// </param>
        /// <param name="Buffer">
        ///  Buffer parameter.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrQueryDisplayInformation2(System.IntPtr DomainHandle, _DOMAIN_DISPLAY_INFORMATION DisplayInformationClass, uint Index, uint EntryCount, uint PreferredMaximumLength, out System.UInt32 TotalAvailable, out System.UInt32 TotalReturned, [Switch("DisplayInformationClass")] out _SAMPR_DISPLAY_INFO_BUFFER Buffer);

        /// <summary>
        ///  The SamrGetDisplayEnumerationIndex2 method obtains an
        ///  index into an account-namesorted list of accounts,
        ///  such that the index is the position in the list of
        ///  the accounts whose account name best matches a client-provided
        ///  string. Opnum: 49 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="DisplayInformationClass">
        ///  An enumeration indicating which set of objects to return
        ///  an index into (for a subsequent SamrQueryDisplayInformation3
        ///  method call).
        /// </param>
        /// <param name="Prefix">
        ///  A string matched against the account name to find a
        ///  starting point for an enumeration. The Prefix parameter
        ///  enables the client to obtain a listing of an account
        ///  from SamrQueryDisplayInformation3  such that the accounts
        ///  are returned in alphabetical order with respect to
        ///  their account name, starting with the account name
        ///  that most closely matches Prefix. See details later
        ///  in this section.
        /// </param>
        /// <param name="Index">
        ///  A value to use as input to SamrQueryDisplayInformation3
        ///   in order to control the accounts that are returned
        ///  from that method.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrGetDisplayEnumerationIndex2(System.IntPtr DomainHandle, _DOMAIN_DISPLAY_INFORMATION DisplayInformationClass, _RPC_UNICODE_STRING Prefix, out System.UInt32 Index);

        /// <summary>
        ///  The SamrCreateUser2InDomain method creates a user. Opnum
        ///  : 50 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="Name">
        ///  The value to use as the name of the user. See the message
        ///  processing shown later in this section for details
        ///  on how this value maps to the data model.
        /// </param>
        /// <param name="AccountType">
        ///  A 32-bit value indicating the type of account to create.
        ///  See the message processing shown later in this section
        ///  for possible values.
        /// </param>
        /// <param name="DesiredAccess">
        ///  The access requested on the UserHandle on output. See
        ///  section  for a listing of possible values.
        /// </param>
        /// <param name="UserHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <param name="GrantedAccess">
        ///  The access granted on UserHandle.
        /// </param>
        /// <param name="RelativeId">
        ///  The RID of the newly created user.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrCreateUser2InDomain(System.IntPtr DomainHandle, _RPC_UNICODE_STRING? Name, uint AccountType, uint DesiredAccess, out System.IntPtr UserHandle, out System.UInt32 GrantedAccess, out System.UInt32 RelativeId);

        /// <summary>
        ///  The SamrQueryDisplayInformation3 method obtains a listing
        ///  of accounts in name-sorted order, starting at a specified
        ///  index. Opnum: 51 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="DisplayInformationClass">
        ///  An enumeration (see section) that indicates the type
        ///  of accounts, as well as the type of attributes on the
        ///  accounts, to return via the Buffer parameter.
        /// </param>
        /// <param name="Index">
        ///  A cursor into an account-namesorted list of accounts.
        /// </param>
        /// <param name="EntryCount">
        ///  The number of accounts that the client is requesting
        ///  on output.
        /// </param>
        /// <param name="PreferredMaximumLength">
        ///  The requested maximum number of bytes to return in Buffer;
        ///  this value overrides EntryCount if this value is reached
        ///  before EntryCount is reached.
        /// </param>
        /// <param name="TotalAvailable">
        ///  The number of bytes required to see a complete listing
        ///  of accounts specified by the DisplayInformationClass
        ///  parameter.
        /// </param>
        /// <param name="TotalReturned">
        ///  The number of bytes returned. This value is estimated
        ///  and is not accurate.  clients do not rely on the accuracy
        ///  of this value.
        /// </param>
        /// <param name="Buffer">
        ///  The accounts that are returned.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrQueryDisplayInformation3(System.IntPtr DomainHandle, _DOMAIN_DISPLAY_INFORMATION DisplayInformationClass, uint Index, uint EntryCount, uint PreferredMaximumLength, out System.UInt32 TotalAvailable, out System.UInt32 TotalReturned, [Switch("DisplayInformationClass")] out _SAMPR_DISPLAY_INFO_BUFFER Buffer);

        /// <summary>
        ///  The SamrAddMultipleMembersToAlias method adds multiple
        ///  members to an alias. Opnum: 52 
        /// </summary>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  an alias object.
        /// </param>
        /// <param name="MembersBuffer">
        ///  A structure containing a list of SIDs to add as members
        ///  to the alias.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrAddMultipleMembersToAlias(System.IntPtr AliasHandle, _SAMPR_PSID_ARRAY? MembersBuffer);

        /// <summary>
        ///  The SamrRemoveMultipleMembersFromAlias method removes
        ///  multiple members from an alias. Opnum: 53 
        /// </summary>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  an alias object.
        /// </param>
        /// <param name="MembersBuffer">
        ///  A structure containing a list of SIDs to remove from
        ///  the alias's membership list.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrRemoveMultipleMembersFromAlias(System.IntPtr AliasHandle, _SAMPR_PSID_ARRAY? MembersBuffer);

        /// <summary>
        ///  The SamrOemChangePasswordUser2 method changes a user's
        ///  password.  Opnum: 54 
        /// </summary>
        /// <param name="BindingHandle">
        ///  An RPC binding handle parameter as specified in [C706-Ch2Intro].
        /// </param>
        /// <param name="ServerName">
        ///  A counted string, encoded in the OEM character set,
        ///  containing the NETBIOS name of the server; this parameter
        ///  MAY servers ignore the ServerName parameter.  be ignored
        ///  by the server.
        /// </param>
        /// <param name="UserName">
        ///  A counted string, encoded in the OEM character set,
        ///  containing the name of the user whose password is to
        ///  be changed; see message processing later in this section
        ///  for details on how this value is used as a database
        ///  key to locate the account that is the target of this
        ///  password change operation.
        /// </param>
        /// <param name="NewPasswordEncryptedWithOldLm">
        ///  A clear text password encrypted according to the specification
        ///  of SAMPR_ENCRYPTED_USER_PASSWORD, where the key is
        ///  the LM hash of the existing password for the target
        ///  user (as presented by the client). The clear text password
        ///  MUST be encoded in an OEM code page character set (as
        ///  opposed to UTF-16).
        /// </param>
        /// <param name="OldLmOwfPasswordEncryptedWithNewLm">
        ///  The LM hash of the target user's existing password (as
        ///  presented by the client) encrypted according to the
        ///  specification of ENCRYPTED_LM_OWF_PASSWORD, where the
        ///  key is the LM hash of the clear text password obtained
        ///  from decrypting NewPasswordEncryptedWithOldLm (see
        ///  the preceding description for decryption details).
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrOemChangePasswordUser2(System.IntPtr BindingHandle, _RPC_STRING ServerName, _RPC_STRING UserName, _SAMPR_ENCRYPTED_USER_PASSWORD NewPasswordEncryptedWithOldLm, _ENCRYPTED_LM_OWF_PASSWORD OldLmOwfPasswordEncryptedWithNewLm);

        /// <summary>
        ///  The SamrUnicodeChangePasswordUser2 method changes a
        ///  user account's password. Opnum: 55 
        /// </summary>
        /// <param name="BindingHandle">
        ///  An RPC binding handle parameter as specified in [C706-Ch2Intro].
        /// </param>
        /// <param name="ServerName">
        ///  A null-terminated string containing the NETBIOS name
        ///  of the server; this parameter MAY servers ignore the
        ///  ServerName parameter.  be ignored by the server.
        /// </param>
        /// <param name="UserName">
        ///  The name of the user. See the message processing later
        ///  in this section for details on how this value is used
        ///  as a database key to locate the account that is the
        ///  target of this password change operation.
        /// </param>
        /// <param name="NewPasswordEncryptedWithOldNt">
        ///  A clear text password encrypted according to the specification
        ///  of SAMPR_ENCRYPTED_USER_PASSWORD, where the key is
        ///  the NT hash of the existing password for the target
        ///  user (as presented by the client in the OldNtOwfPasswordEncryptedWithNewNt
        ///  parameter). 
        /// </param>
        /// <param name="OldNtOwfPasswordEncryptedWithNewNt">
        ///  The NT hash of the target user's existing password (as
        ///  presented by the client) encrypted according to the
        ///  specification of ENCRYPTED_LM_OWF_PASSWORD, where the
        ///  key is the NT hash of the clear text password obtained
        ///  from decrypting NewPasswordEncryptedWithOldNt.
        /// </param>
        /// <param name="LmPresent">
        ///  If this parameter is zero, NewPasswordEncryptedWithOldLm
        ///  and OldLmOwfPasswordEncryptedWithOldLm MUST be ignored;
        ///  otherwise these fields MUST be processed.
        /// </param>
        /// <param name="NewPasswordEncryptedWithOldLm">
        ///  A clear text password encrypted according to the specification
        ///  of SAMPR_ENCRYPTED_USER_PASSWORD, where the key is
        ///  the LM hash of the existing password for the target
        ///  user (as presented by the client).
        /// </param>
        /// <param name="OldLmOwfPasswordEncryptedWithNewNt">
        ///  The LM hash the target user's existing password (as
        ///  presented by the client) encrypted according to the
        ///  specification of ENCRYPTED_LM_OWF_PASSWORD, where the
        ///  key is the NT hash of the clear text password obtained
        ///  from decrypting NewPasswordEncryptedWithOldNt.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrUnicodeChangePasswordUser2(System.IntPtr BindingHandle, _RPC_UNICODE_STRING ServerName, _RPC_UNICODE_STRING UserName, _SAMPR_ENCRYPTED_USER_PASSWORD NewPasswordEncryptedWithOldNt, _ENCRYPTED_LM_OWF_PASSWORD OldNtOwfPasswordEncryptedWithNewNt, byte LmPresent, _SAMPR_ENCRYPTED_USER_PASSWORD NewPasswordEncryptedWithOldLm, _ENCRYPTED_LM_OWF_PASSWORD OldLmOwfPasswordEncryptedWithNewNt);

        /// <summary>
        ///  The SamrGetDomainPasswordInformation method obtains
        ///  select password policy information (without authenticating
        ///  to the server). Opnum: 56 
        /// </summary>
        /// <param name="BindingHandle">
        ///  An RPC binding handle parameter, as specified in [C706-Ch2Intro].
        /// </param>
        /// <param name="Unused">
        ///  A string value that is unused by the protocol. It is
        ///  ignored by the server. The client MAY clients set this
        ///  value to be the NULL-terminated NETBIOS name of the
        ///  server. set any value.
        /// </param>
        /// <param name="PasswordInformation">
        ///  Password policy information from the account domain.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrGetDomainPasswordInformation(System.IntPtr BindingHandle, _RPC_UNICODE_STRING Unused, out _USER_DOMAIN_PASSWORD_INFORMATION PasswordInformation);

        /// <summary>
        ///  The SamrConnect2 method returns a handle to a server
        ///  object. Opnum: 57 
        /// </summary>
        /// <param name="ServerName">
        ///  The null-terminated NETBIOS name of the server; this
        ///  parameter MAYServerName is ignored on receipt.  be
        ///  ignored on receipt.
        /// </param>
        /// <param name="ServerHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <param name="DesiredAccess">
        ///  An ACCESS_MASK that indicates the access requested for
        ///  ServerHandle on output. See section  for a listing
        ///  of possible values.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrConnect2(string ServerName, out System.IntPtr ServerHandle, uint DesiredAccess);

        /// <summary>
        ///  The SamrSetInformationUser2 method updates attributes
        ///  on a user object. Opnum: 58 
        /// </summary>
        /// <param name="UserHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a user object.
        /// </param>
        /// <param name="UserInformationClass">
        ///  An enumeration indicating which attributes to update.
        ///  See section  for a listing of possible values.
        /// </param>
        /// <param name="Buffer">
        ///  The requested attributes and values to update. See section
        ///   for structure details.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrSetInformationUser2(System.IntPtr UserHandle, _USER_INFORMATION_CLASS UserInformationClass, [Switch("UserInformationClass")] _SAMPR_USER_INFO_BUFFER Buffer);

        /// <summary>
        ///  Reserved for local use. Opnum: 59 
        /// </summary>
        void Opnum59NotUsedOnWire();

        /// <summary>
        ///  Reserved for local use. Opnum: 60 
        /// </summary>
        void Opnum60NotUsedOnWire();

        /// <summary>
        ///  Reserved for local use. Opnum: 61 
        /// </summary>
        void Opnum61NotUsedOnWire();

        /// <summary>
        ///  The SamrConnect4 method obtains a handle to a server
        ///  object. Opnum: 62 
        /// </summary>
        /// <param name="ServerName">
        ///  The null-terminated NETBIOS name of the server; this
        ///  parameter MAYServerName is ignored on receipt.  be
        ///  ignored on receipt.
        /// </param>
        /// <param name="ServerHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <param name="ClientRevision">
        ///  Indicates the revision (for this protocol) of the client.
        ///  See the Revision field of SAMPR_REVISION_INFO_V1 for
        ///  possible values.
        /// </param>
        /// <param name="DesiredAccess">
        ///  An ACCESS_MASK that indicates the access requested for
        ///  ServerHandle on output. See section  for a listing
        ///  of possible values.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrConnect4(string ServerName, out System.IntPtr ServerHandle, uint ClientRevision, uint DesiredAccess);

        /// <summary>
        ///  Reserved for local use. Opnum: 63 
        /// </summary>
        void Opnum63NotUsedOnWire();

        /// <summary>
        ///  The SamrConnect5 method obtains a handle to a server
        ///  object. Opnum: 64 
        /// </summary>
        /// <param name="ServerName">
        ///  The null-terminated NETBIOS name of the server; this
        ///  parameter MAYServerName is ignored on receipt.  be
        ///  ignored on receipt.
        /// </param>
        /// <param name="DesiredAccess">
        ///  An ACCESS_MASK that indicates the access requested for
        ///  ServerHandle on output. For a listing of possible values,
        ///  see section.
        /// </param>
        /// <param name="InVersion">
        ///  Indicates which field of the InRevisionInfo union is
        ///  used.
        /// </param>
        /// <param name="InRevisionInfo">
        ///  Revision information. For details, see the definition
        ///  of the SAMPR_REVISION_INFO_V1 structure, which is contained
        ///  in the SAMPR_REVISION_INFO union.
        /// </param>
        /// <param name="OutVersion">
        ///  Indicates which field of the OutRevisionInfo union is
        ///  used.
        /// </param>
        /// <param name="OutRevisionInfo">
        ///  Revision information. For details, see the definition
        ///  of the SAMPR_REVISION_INFO_V1 structure, which is contained
        ///  in the SAMPR_REVISION_INFO union.
        /// </param>
        /// <param name="ServerHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrConnect5(string ServerName, uint DesiredAccess, uint InVersion, [Switch("InVersion")] SAMPR_REVISION_INFO InRevisionInfo, out System.UInt32 OutVersion, [Switch("*OutVersion")] out SAMPR_REVISION_INFO OutRevisionInfo, out System.IntPtr ServerHandle);

        /// <summary>
        ///  The SamrRidToSid method obtains the SID of an account,
        ///  given a RID. Opnum: 65 
        /// </summary>
        /// <param name="ObjectHandle">
        ///  An RPC context handle, as specified in section. The
        ///  message processing shown later in this section contains
        ///  details on which types of ObjectHandle are accepted
        ///  by the server.
        /// </param>
        /// <param name="Rid">
        ///  A RID of an account.
        /// </param>
        /// <param name="Sid">
        ///  The SID of the account referenced by Rid.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrRidToSid(System.IntPtr ObjectHandle, uint Rid, out _RPC_SID? Sid);

        /// <summary>
        ///  The SamrSetDSRMPassword method sets a local recovery
        ///  password. Opnum: 66 
        /// </summary>
        /// <param name="BindingHandle">
        ///  An RPC binding handle parameter, as specified in [C706-Ch2Intro].
        /// </param>
        /// <param name="Unused">
        ///  A string value. This value is not used in the protocol
        ///  and is ignored by the server.
        /// </param>
        /// <param name="UserId">
        ///  A RID of a user account. See the message processing
        ///  later in this section for details on restrictions on
        ///  this value.
        /// </param>
        /// <param name="EncryptedNtOwfPassword">
        ///  The NT hash of the new password (as presented by the
        ///  client) encrypted according to the specification of
        ///  ENCRYPTED_NT_OWF_PASSWORD, where the key is the User ID.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrSetDSRMPassword(System.IntPtr BindingHandle, _RPC_UNICODE_STRING Unused, uint UserId, _ENCRYPTED_LM_OWF_PASSWORD EncryptedNtOwfPassword);

        /// <summary>
        ///  The SamrValidatePassword method validates an application
        ///  password against the locally stored policy. Opnum :
        ///  67 
        /// </summary>
        /// <param name="Handle">
        ///  An RPC binding handle parameter, as specified in [C706-Ch2Intro].
        /// </param>
        /// <param name="ValidationType">
        ///  The password policy validation requested.
        /// </param>
        /// <param name="InputArg">
        ///  The password-related material to validate.
        /// </param>
        /// <param name="OutputArg">
        ///  The result of the validation.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        int SamrValidatePassword(System.IntPtr Handle, _PASSWORD_POLICY_VALIDATION_TYPE ValidationType, [Switch("ValidationType")] _SAM_VALIDATE_INPUT_ARG InputArg, [Switch("ValidationType")] out _SAM_VALIDATE_OUTPUT_ARG? OutputArg);

        /// <summary>
        ///  Reserved for local use. Opnum: 68 
        /// </summary>
        void Opnum68NotUsedOnWire();

        /// <summary>
        ///  Reserved for local use. Opnum: 69 
        /// </summary>
        void Opnum69NotUsedOnWire();
    }
}
