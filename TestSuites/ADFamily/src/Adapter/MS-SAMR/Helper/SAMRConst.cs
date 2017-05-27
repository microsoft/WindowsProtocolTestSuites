// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Samr
{
    /// <summary>
    /// Utilities tool for SAMR protocol.
    /// </summary>
    public partial class Utilities
    {
        #region Constants

        /// <summary>
        /// Access Mask DELETE.
        /// </summary>
        public const UInt32 DELETE = 0x00010000;

        /// <summary>
        /// Access Mask READ_CONTROL.
        /// </summary>
        public const UInt32 READ_CONTROL = 0x00020000;

        /// <summary>
        /// Access Mask WRITE_DAC.
        /// </summary>
        public const UInt32 WRITE_DAC = 0x00040000;

        /// <summary>
        ///  Access Mask WRITE_OWNER.
        /// </summary>
        public const UInt32 WRITE_OWNER = 0x00080000;

        /// <summary>
        /// Access Mask ACCESS_SYSTEM_SECURITY.
        /// </summary>
        public const UInt32 ACCESS_SYSTEM_SECURITY = 0x01000000;

        /// <summary>
        /// Access Mask MAXIMUM_ALLOWED.
        /// </summary>
        public const UInt32 MAXIMUM_ALLOWED = 0x02000000;

        /// <summary>
        /// Access Mask GENERIC_READ.
        /// </summary>
        public const UInt32 GENERIC_READ = 0x80000000;

        /// <summary>
        /// Access Mask GENERIC_WRITE.
        /// </summary>
        public const UInt32 GENERIC_WRITE = 0x40000000;

        /// <summary>
        /// Access Mask GENERIC_EXECUTE.
        /// </summary>
        public const UInt32 GENERIC_EXECUTE = 0x20000000;

        /// <summary>
        /// Access Mask GENERIC_ALL.
        /// </summary>
        public const UInt32 GENERIC_ALL = 0x10000000;

        /// <summary>
        /// Access Mask SAM_SERVER_CONNECT.
        /// </summary>
        public const UInt32 SAM_SERVER_CONNECT = 0x00000001;

        /// <summary>
        /// Access Mask SAM_SERVER_SHUTDOWN.
        /// </summary>
        public const UInt32 SAM_SERVER_SHUTDOWN = 0x00000002;

        /// <summary>
        /// Access Mask SAM_SERVER_INITIALIZE.
        /// </summary>
        public const UInt32 SAM_SERVER_INITIALIZE = 0x00000004;

        /// <summary>
        /// Access Mask SAM_SERVER_CREATE_DOMAIN.
        /// </summary>
        public const UInt32 SAM_SERVER_CREATE_DOMAIN = 0x00000008;

        /// <summary>
        /// Access Mask SAM_SERVER_ENUMERATE_DOMAINS.
        /// </summary>
        public const UInt32 SAM_SERVER_ENUMERATE_DOMAINS = 0x00000010;

        /// <summary>
        /// Access Mask SAM_SERVER_LOOKUP_DOMAIN.
        /// </summary>
        public const UInt32 SAM_SERVER_LOOKUP_DOMAIN = 0x00000020;

        /// <summary>
        /// Access Mask SAM_SERVER_ALL_ACCESS.
        /// </summary>
        public const UInt32 SAM_SERVER_ALL_ACCESS = 0x000F003F;

        /// <summary>
        /// Access Mask SAM_SERVER_READ.
        /// </summary>
        public const UInt32 SAM_SERVER_READ = 0x00020010;

        /// <summary>
        /// Access Mask SAM_SERVER_WRITE.
        /// </summary>
        public const UInt32 SAM_SERVER_WRITE = 0x0002000E;

        /// <summary>
        /// Access Mask SAM_SERVER_EXECUTE.
        /// </summary>
        public const UInt32 SAM_SERVER_EXECUTE = 0x00020021;

        /// <summary>
        /// Access Mask DOMAIN_READ_PASSWORD_PARAMETERS.
        /// </summary>
        public const UInt32 DOMAIN_READ_PASSWORD_PARAMETERS = 0x00000001;

        /// <summary>
        /// Access Mask DOMAIN_WRITE_PASSWORD_PARAMS.
        /// </summary>
        public const UInt32 DOMAIN_WRITE_PASSWORD_PARAMS = 0x00000002;

        /// <summary>
        /// Access Mask DOMAIN_READ_OTHER_PARAMETERS.
        /// </summary>
        public const UInt32 DOMAIN_READ_OTHER_PARAMETERS = 0x00000004;

        /// <summary>
        /// Access Mask DOMAIN_WRITE_OTHER_PARAMETERS.
        /// </summary>
        public const UInt32 DOMAIN_WRITE_OTHER_PARAMETERS = 0x00000008;

        /// <summary>
        /// Access Mask DOMAIN_CREATE_USER.
        /// </summary>
        public const UInt32 DOMAIN_CREATE_USER = 0x00000010;

        /// <summary>
        /// Access Mask DOMAIN_CREATE_GROUP.
        /// </summary>
        public const UInt32 DOMAIN_CREATE_GROUP = 0x00000020;

        /// <summary>
        /// Access Mask DOMAIN_CREATE_ALIAS.
        /// </summary>
        public const UInt32 DOMAIN_CREATE_ALIAS = 0x00000040;

        /// <summary>
        /// Access Mask DOMAIN_GET_ALIAS_MEMBERSHIP.
        /// </summary>
        public const UInt32 DOMAIN_GET_ALIAS_MEMBERSHIP = 0x00000080;

        /// <summary>
        /// Access Mask DOMAIN_LIST_ACCOUNTS.
        /// </summary>
        public const UInt32 DOMAIN_LIST_ACCOUNTS = 0x00000100;

        /// <summary>
        /// Access Mask DOMAIN_LOOKUP.
        /// </summary>
        public const UInt32 DOMAIN_LOOKUP = 0x00000200;

        /// <summary>
        /// Access Mask DOMAIN_ADMINISTER_SERVER.
        /// </summary>
        public const UInt32 DOMAIN_ADMINISTER_SERVER = 0x00000400;

        /// <summary>
        /// Access Mask DOMAIN_ALL_ACCESS.
        /// </summary>
        public const UInt32 DOMAIN_ALL_ACCESS = 0x000F07FF;

        /// <summary>
        /// Access Mask DOMAIN_READ.
        /// </summary>
        public const UInt32 DOMAIN_READ = 0x00020084;

        /// <summary>
        /// Access Mask DOMAIN_ALL_WRITE.
        /// </summary>
        public const UInt32 DOMAIN_ALL_WRITE = 0x0002047A;

        /// <summary>
        /// Access Mask DOMAIN_ALL_EXECUTE.
        /// </summary>
        public const UInt32 DOMAIN_ALL_EXECUTE = 0x00020301;

        /// <summary>
        /// Access Mask GROUP_READ_INFORMATION.
        /// </summary>
        public const UInt32 GROUP_READ_INFORMATION = 0x00000001;

        /// <summary>
        /// Access Mask GROUP_WRITE_ACCOUNT.
        /// </summary>
        public const UInt32 GROUP_WRITE_ACCOUNT = 0x00000002;

        /// <summary>
        /// Access Mask GROUP_ADD_MEMBER.
        /// </summary>
        public const UInt32 GROUP_ADD_MEMBER = 0x00000004;

        /// <summary>
        /// Access Mask GROUP_REMOVE_MEMBER.
        /// </summary>
        public const UInt32 GROUP_REMOVE_MEMBER = 0x00000008;

        /// <summary>
        /// Access Mask GROUP_LIST_MEMBERS.
        /// </summary>
        public const UInt32 GROUP_LIST_MEMBERS = 0x00000010;

        /// <summary>
        /// Access Mask GROUP_ALL_ACCESS.
        /// </summary>
        public const UInt32 GROUP_ALL_ACCESS = 0x000F001F;

        /// <summary>
        /// Access Mask GROUP_ALL_READ.
        /// </summary>
        public const UInt32 GROUP_ALL_READ = 0x00020010;

        /// <summary>
        /// Access Mask GROUP_ALL_WRITE.
        /// </summary>
        public const UInt32 GROUP_ALL_WRITE = 0x0002000E;

        /// <summary>
        /// Access Mask GROUP_ALL_EXECUTE.
        /// </summary>
        public const UInt32 GROUP_ALL_EXECUTE = 0x00020001;

        /// <summary>
        /// Access Mask ALIAS_ADD_MEMBER.
        /// </summary>
        public const UInt32 ALIAS_ADD_MEMBER = 0x00000001;

        /// <summary>
        /// Access Mask ALIAS_REMOVE_MEMBER.
        /// </summary>
        public const UInt32 ALIAS_REMOVE_MEMBER = 0x00000002;

        /// <summary>
        /// Access Mask ALIAS_LIST_MEMBERS.
        /// </summary>
        public const UInt32 ALIAS_LIST_MEMBERS = 0x00000004;

        /// <summary>
        /// Access Mask ALIAS_READ_INFORMATION.
        /// </summary>
        public const UInt32 ALIAS_READ_INFORMATION = 0x00000008;

        /// <summary> 
        /// Access Mask ALIAS_WRITE_ACCOUNT.
        /// </summary>
        public const UInt32 ALIAS_WRITE_ACCOUNT = 0x00000010;

        /// <summary>
        /// Access Mask ALIAS_ALL_ACCESS.
        /// </summary>
        public const UInt32 ALIAS_ALL_ACCESS = 0x000F001F;

        /// <summary>
        /// Access Mask ALIAS_ALL_READ.
        /// </summary>
        public const UInt32 ALIAS_ALL_READ = 0x00020004;

        /// <summary>
        /// Access Mask ALIAS_ALL_WRITE.
        /// </summary>
        public const UInt32 ALIAS_ALL_WRITE = 0x00020013;

        /// <summary>
        /// Access Mask ALIAS_ALL_EXECUTE.
        /// </summary>
        public const UInt32 ALIAS_ALL_EXECUTE = 0x00020008;

        /// <summary>
        /// Access Mask USER_READ_GENERAL.
        /// </summary>
        public const UInt32 USER_READ_GENERAL = 0x00000001;

        /// <summary>
        /// Access Mask USER_READ_PREFERENCES.
        /// </summary>
        public const UInt32 USER_READ_PREFERENCES = 0x00000002;

        /// <summary>
        /// Access Mask USER_WRITE_PREFERENCES.
        /// </summary>
        public const UInt32 USER_WRITE_PREFERENCES = 0x00000004;

        /// <summary>
        /// Access Mask USER_READ_LOGON.
        /// </summary>
        public const UInt32 USER_READ_LOGON = 0x00000008;

        /// <summary>
        /// Access Mask USER_READ_ACCOUNT.
        /// </summary>
        public const UInt32 USER_READ_ACCOUNT = 0x00000010;

        /// <summary>
        /// Access Mask USER_WRITE_ACCOUNT.
        /// </summary>
        public const UInt32 USER_WRITE_ACCOUNT = 0x00000020;

        /// <summary>
        /// Access Mask USER_CHANGE_PASSWORD.
        /// </summary>
        public const UInt32 USER_CHANGE_PASSWORD = 0x00000040;

        /// <summary>
        /// Access Mask USER_FORCE_PASSWORD_CHANGE.
        /// </summary>
        public const UInt32 USER_FORCE_PASSWORD_CHANGE = 0x00000080;

        /// <summary>
        /// Access Mask USER_LIST_GROUPS.
        /// </summary>
        public const UInt32 USER_LIST_GROUPS = 0x00000100;

        /// <summary>
        /// Access Mask USER_READ_GROUP_INFORMATION.
        /// </summary>
        public const UInt32 USER_READ_GROUP_INFORMATION = 0x00000200;

        /// <summary>
        /// Access Mask USER_WRITE_GROUP_INFORMATION.
        /// </summary>
        public const UInt32 USER_WRITE_GROUP_INFORMATION = 0x00000400;

        /// <summary>
        /// Access Mask USER_ALL_ACCESS.
        /// </summary>
        public const UInt32 USER_ALL_ACCESS = 0x000F07FF;

        /// <summary>
        /// Access Mask USER_ALL_READ.
        /// </summary>
        public const UInt32 USER_ALL_READ = 0x0002031A;

        /// <summary>
        /// Access Mask USER_ALL_WRITE.
        /// </summary>
        public const UInt32 USER_ALL_WRITE = 0x00020044;

        /// <summary>
        /// Access Mask USER_ALL_EXECUTE.
        /// </summary>
        public const UInt32 USER_ALL_EXECUTE = 0x00020041;

        /// <summary>
        /// Access Mask ACTRL_DS_CONTROL_ACCESS.
        /// </summary>
        public const UInt32 ACTRL_DS_CONTROL_ACCESS = 0x00000100;

        /// <summary>
        /// Access Mask ACTRL_DS_DELETE_TREE.
        /// </summary>
        public const UInt32 ACTRL_DS_DELETE_TREE = 0x00000040;

        /// <summary>
        /// Attribute Values of a Group.
        /// </summary>
        public const UInt32 SE_GROUP_MANDATORY = 0x00000001;

        /// <summary>
        /// Attribute Values of a Group.
        /// </summary>
        public const UInt32 SE_GROUP_ENABLED_BY_DEFAULT = 0x00000002;

        /// <summary>
        /// Attribute Values of a Group.
        /// </summary>
        public const UInt32 SE_GROUP_ENABLED = 0x00000004;

        /// <summary>
        /// InValid Bits ServerInValidAccessMask.
        /// </summary>
        public const UInt32 ServerInValidAccessMask = 0x0CF0FFC0;

        /// <summary>
        /// InValid Bits DomainInValidAccessMask.
        /// </summary>
        public const UInt32 DomainInValidAccessMask = 0x0CF0F800;

        /// <summary>
        /// InValid Bits GroupInValidAccessMask.
        /// </summary>
        public const UInt32 GroupInValidAccessMask = 0x0CF0FFE0;

        /// <summary>
        /// InValid Bits AliasInValidAccessMask.
        /// </summary>
        public const UInt32 AliasInValidAccessMask = 0x0CF0FFE0;

        /// <summary>
        /// InValid Bits UserInValidAccessMask.
        /// </summary>
        public const UInt32 UserInValidAccessMask = 0x0CF0F800;

        /// <summary>
        /// InValid Bits CommonAccessMaskInValid.
        /// </summary>
        public const UInt32 CommonAccessMaskInValid = 0xFCF0FFFF;

        /// <summary>
        /// UserAccountControl Values USER_ACCOUNT_DISABLED.
        /// </summary>
        public const uint USER_ACCOUNT_DISABLED = 0x00000001;

        /// <summary>
        /// UserAccountControl Values .
        /// </summary>
        public const uint USER_HOME_DIRECTORY_REQUIRED = 0x00000002;

        /// <summary>
        /// UserAccountControl Values USER_PASSWORD_NOT_REQUIRED.
        /// </summary>
        public const uint USER_PASSWORD_NOT_REQUIRED = 0x00000004;

        /// <summary>
        /// UserAccountControl Values USER_TEMP_DUPLICATE_ACCOUNT.
        /// </summary>
        public const uint USER_TEMP_DUPLICATE_ACCOUNT = 0x00000008;

        /// <summary>
        /// UserAccountControl Values USER_NORMAL_ACCOUNT.
        /// </summary>
        public const uint USER_NORMAL_ACCOUNT = 0x00000010;

        /// <summary>
        /// UserAccountControl Values USER_MNS_LOGON_ACCOUNT.
        /// </summary>
        public const uint USER_MNS_LOGON_ACCOUNT = 0x00000020;

        /// <summary>
        /// UserAccountControl Values USER_INTERDOMAIN_TRUST_ACCOUNT.
        /// </summary>
        public const uint USER_INTERDOMAIN_TRUST_ACCOUNT = 0x00000040;

        /// <summary>
        /// UserAccountControl Values USER_WORKSTATION_TRUST_ACCOUNT.
        /// </summary>
        public const uint USER_WORKSTATION_TRUST_ACCOUNT = 0x00000080;

        /// <summary>
        /// UserAccountControl Values USER_SERVER_TRUST_ACCOUNT.
        /// </summary>
        public const uint USER_SERVER_TRUST_ACCOUNT = 0x00000100;

        /// <summary>
        /// UserAccountControl Values USER_DONT_EXPIRE_PASSWORD.
        /// </summary>
        public const uint USER_DONT_EXPIRE_PASSWORD = 0x00000200;

        /// <summary>
        /// UserAccountControl Values USER_ACCOUNT_AUTO_LOCKED.
        /// </summary>
        public const uint USER_ACCOUNT_AUTO_LOCKED = 0x00000400;

        /// <summary>
        /// UserAccountControl Values USER_ENCRYPTED_TEXT_PASSWORD_ALLOWED.
        /// </summary>
        public const uint USER_ENCRYPTED_TEXT_PASSWORD_ALLOWED = 0x00000800;

        /// <summary>
        /// UserAccountControl Values USER_SMARTCARD_REQUIRED.
        /// </summary>
        public const uint USER_SMARTCARD_REQUIRED = 0x00001000;

        /// <summary>
        /// UserAccountControl Values USER_TRUSTED_FOR_DELEGATION.
        /// </summary>
        public const uint USER_TRUSTED_FOR_DELEGATION = 0x00002000;

        /// <summary>
        /// UserAccountControl Values USER_NOT_DELEGATED.
        /// </summary>
        public const uint USER_NOT_DELEGATED = 0x00004000;

        /// <summary>
        /// UserAccountControl Values USER_USE_DES_KEY_ONLY.
        /// </summary>
        public const uint USER_USE_DES_KEY_ONLY = 0x00008000;

        /// <summary>
        /// UserAccountControl Values USER_DONT_REQUIRE_PREAUTH.
        /// </summary>
        public const uint USER_DONT_REQUIRE_PREAUTH = 0x00010000;

        /// <summary>
        /// UserAccountControl Values USER_PASSWORD_EXPIRED.
        /// </summary>
        public const uint USER_PASSWORD_EXPIRED = 0x00020000;

        /// <summary>
        /// UserAccountControl Values USER_TRUSTED_TO_AUTHENTICATE_FOR_DELEGATION.
        /// </summary>
        public const uint USER_TRUSTED_TO_AUTHENTICATE_FOR_DELEGATION = 0x00040000;

        /// <summary>
        /// UserAccountControl Values USER_NO_AUTH_DATA_REQUIRED.
        /// </summary>
        public const uint USER_NO_AUTH_DATA_REQUIRED = 0x00080000;

        /// <summary>
        /// HRESULT values STATUS_DEFAULT.
        /// </summary>
        public const uint STATUS_DEFAULT = 0x00000FFF;

        /// <summary>
        /// HRESULT values STATUS_SUCCESS.
        /// </summary>
        public const uint STATUS_SUCCESS = 0x00000000;

        /// <summary>
        /// HRESULT values STATUS_NO_SUCH_MEMBER.
        /// </summary>
        public const uint STATUS_NO_SUCH_MEMBER = 0xC000017A;

        /// <summary>
        /// HRESULT values STATUS_ERROR.
        /// </summary>
        public const uint STATUS_ERROR = 0xFFFFF;

        /// <summary>
        /// HRESULT values STATUS_ACCESS_DENIED.
        /// </summary>
        public const uint STATUS_ACCESS_DENIED = 0xC0000022;

        /// <summary>
        /// HRESULT values STATUS_MORE_ENTRIES.
        /// </summary>
        public const uint STATUS_MORE_ENTRIES = 0x00000105;

        /// <summary>
        /// HRESULT values STATUS_SOME_NOT_MAPPED.
        /// </summary>
        public const uint STATUS_SOME_NOT_MAPPED = 0x00000107;

        /// <summary>
        /// HRESULT values STATUS_NONE_MAPPED.
        /// </summary>
        public const uint STATUS_NONE_MAPPED = 0xC0000073;

        /// <summary>
        /// HRESULT values STATUS_WRONG_PASSWORD.
        /// </summary>
        public const uint STATUS_WRONG_PASSWORD = 0xC000006A;

        /// <summary>
        /// HRESULT values STATUS_ACCOUNT_LOCKED_OUT.
        /// </summary>
        public const uint STATUS_ACCOUNT_LOCKED_OUT = 0xC0000234;

        /// <summary>
        /// HRESULT values STATUS_GROUP_EXISTS.
        /// </summary>
        public const uint STATUS_GROUP_EXISTS = 0xC0000065;

        /// <summary>
        /// HRESULT values STATUS_ALIAS_EXISTS.
        /// </summary>
        public const uint STATUS_ALIAS_EXISTS = 0xC0000154;

        /// <summary>
        /// HRESULT values STATUS_USER_EXISTS.
        /// </summary>
        public const uint STATUS_USER_EXISTS = 0xC0000063;

        /// <summary>
        /// HRESULT values STATUS_NO_SUCH_DOMAIN.
        /// </summary>
        public const uint STATUS_NO_SUCH_DOMAIN = 0xC00000DF;

        /// <summary>
        /// HRESULT values STATUS_NO_SUCH_USER.
        /// </summary>
        public const uint STATUS_NO_SUCH_USER = 0xC0000064;

        /// <summary>
        /// HRESULT values STATUS_NO_SUCH_GROUP.
        /// </summary>
        public const uint STATUS_NO_SUCH_GROUP = 0xC0000066;

        /// <summary>
        /// HRESULT values STATUS_NO_SUCH_ALIAS.
        /// </summary>
        public const uint STATUS_NO_SUCH_ALIAS = 0xC0000151;

        /// <summary>
        /// HRESULT values STATUS_SPECIAL_ACCOUNT.
        /// </summary>
        public const uint STATUS_SPECIAL_ACCOUNT = 0xC0000124;

        /// <summary>
        /// HRESULT values STATUS_LM_CROSS_ENCRYPTION_REQUIRED.
        /// </summary>
        public const uint STATUS_LM_CROSS_ENCRYPTION_REQUIRED = 0xC000017F;

        /// <summary>
        /// HRESULT values STATUS_NT_CROSS_ENCRYPTION_REQUIRED.
        /// </summary>
        public const uint STATUS_NT_CROSS_ENCRYPTION_REQUIRED = 0xC000015D;

        /// <summary>
        /// HRESULT values STATUS_MEMBER_IN_GROUP.
        /// </summary>
        public const uint STATUS_MEMBER_IN_GROUP = 0x00000528;

        /// <summary>
        /// HRESULT values STATUS_MEMBER_IN_ALIAS.
        /// </summary>
        public const uint STATUS_MEMBER_IN_ALIAS = 0xC0000153;

        /// <summary>
        /// HRESULT values STATUS_MEMBER_NOT_IN_ALIAS.
        /// </summary>
        public const uint STATUS_MEMBER_NOT_IN_ALIAS = 0xC0000152;

        /// <summary>
        /// HRESULT values STATUS_MEMBER_NOT_IN_GROUP.
        /// </summary>
        public const uint STATUS_MEMBER_NOT_IN_GROUP = 0xC0000068;

        /// <summary>
        /// HRESULT values STATUS_OBJECT_NAME_NOT_FOUND.
        /// </summary>
        public const uint STATUS_OBJECT_NAME_NOT_FOUND = 0xC0000034;

        /// <summary>
        /// HRESULT values STATUS_INVALID_INFO_CLASS.
        /// </summary>
        public const uint STATUS_INVALID_INFO_CLASS = 0xC0000003;

        /// <summary>
        /// HRESULT values STATUS_NO_MORE_ENTRIES.
        /// </summary>
        public const uint STATUS_NO_MORE_ENTRIES = 0x8000001A;

        /// <summary>
        /// HRESULT values STATUS_INVALID_PARAMETER.
        /// </summary>
        public const uint STATUS_INVALID_PARAMETER = 0xc000000d;

        /// <summary>
        /// HRESULT values STATUS_INVALID_PARAMETER_MIX.
        /// </summary>
        public const uint STATUS_INVALID_PARAMETER_MIX = 0xC0000030;

        /// <summary>
        /// HRESULT values SAM_VALIDATE_PASSWORD_LAST_SET.
        /// </summary>
        public const uint SAM_VALIDATE_PASSWORD_LAST_SET = 0x00000001;

        /// <summary>
        /// HRESULT values SAM_VALIDATE_BAD_PASSWORD_TIME.
        /// </summary>
        public const uint SAM_VALIDATE_BAD_PASSWORD_TIME = 0x00000002;

        /// <summary>
        /// HRESULT values SAM_VALIDATE_LOCKOUT_TIME.
        /// </summary>
        public const uint SAM_VALIDATE_LOCKOUT_TIME = 0x00000004;

        /// <summary>
        /// HRESULT values SAM_VALIDATE_BAD_PASSWORD_COUNT.
        /// </summary>
        public const uint SAM_VALIDATE_BAD_PASSWORD_COUNT = 0x00000008;

        /// <summary>
        /// HRESULT values SAM_VALIDATE_PASSWORD_HISTORY_LENGTH.
        /// </summary>
        public const uint SAM_VALIDATE_PASSWORD_HISTORY_LENGTH = 0x00000010;

        /// <summary>
        /// HRESULT values SAM_VALIDATE_PASSWORD_HISTORY.
        /// </summary>
        public const uint SAM_VALIDATE_PASSWORD_HISTORY = 0x00000020;

        /// <summary>
        /// HRESULT values STATUS_BUFFER_NULL.
        /// </summary>
        public const uint STATUS_BUFFER_NULL = 0xC0000062;

        /// <summary>
        /// HRESULT values RPC_S_ACCESS_DENIED.
        /// </summary>
        public const uint RPC_S_ACCESS_DENIED = 0x00000005;

        /// <summary>
        /// User Account bits UF_PARTIAL_SECRETS_ACCOUNT.
        /// </summary>
        public const uint UF_PARTIAL_SECRETS_ACCOUNT = 0x04000000;

        /// <summary>
        /// User Account bits UF_NORMAL_ACCOUNT.
        /// </summary>
        public const uint UF_NORMAL_ACCOUNT = 0x00000200;

        /// <summary>
        /// User Account bits UF_ACCOUNTDISABLE.
        /// </summary>
        public const uint UF_ACCOUNTDISABLE = 0x00000002;

        /// <summary>
        /// User Account bits UF_WORKSTATION_TRUST_ACCOUNT.
        /// </summary>
        public const uint UF_WORKSTATION_TRUST_ACCOUNT = 0x00001000;

        /// <summary>
        /// User Account bits UF_SERVER_TRUST_ACCOUNT.
        /// </summary>
        public const uint UF_SERVER_TRUST_ACCOUNT = 0x00002000;

        /// <summary>
        /// User Account bits UF_INTERDOMAIN_TRUST_ACCOUNT.
        /// </summary>
        public const uint UF_INTERDOMAIN_TRUST_ACCOUNT = 0x00000800;

        /// <summary>
        /// User Account bits UF_PASSWD_NOTREQD.
        /// </summary>
        public const uint UF_PASSWD_NOTREQD = 0x00000020;

        /// <summary>
        /// User Account bits UF_DONT_EXPIRE_PASSWORD.
        /// </summary>
        public const uint UF_DONT_EXPIRE_PASSWORD = 0x00010000;

        /// <summary>
        /// User Account bits UF_SMARTCARD_REQUIRED.
        /// </summary>
        public const uint UF_SMARTCARD_REQUIRED = 0x00040000;

        /// <summary>
        /// User Account bits UF_LOCKOUT.
        /// </summary>
        public const uint UF_LOCKOUT = 0x00000010;


        /// <summary>
        /// Bits to represent Group Attributes DOMAIN_PASSWORD_COMPLEX.
        /// </summary>
        public const uint DOMAIN_PASSWORD_COMPLEX = 0x00000001;

        /// <summary>
        /// Bits to represent Group Attributes DOMAIN_PASSWORD_STORE_CLEARTEXT.
        /// </summary>
        public const uint DOMAIN_PASSWORD_STORE_CLEARTEXT = 0x00000010;

        /// <summary>
        /// Predefined User Accounts DOMAIN_USER_RID_ADMIN.
        /// </summary>
        public const uint DOMAIN_USER_RID_ADMIN = 0x000001F4;

        /// <summary>
        /// Predefined User Accounts DOMAIN_USER_RID_KRBTGT.
        /// </summary>
        public const uint DOMAIN_USER_RID_KRBTGT = 0x000001F6;

        /// <summary>
        /// Predefined User Accounts DOMAIN_GROUP_RID_CONTROLLERS.
        /// </summary>
        public const uint DOMAIN_GROUP_RID_CONTROLLERS = 0x00000204;

        /// <summary>
        /// Predefined User Accounts DOMAIN_GROUP_RID_READONLY_CONTROLLERS.
        /// </summary>
        public const uint DOMAIN_GROUP_RID_READONLY_CONTROLLERS = 0x00000209;

        /// <summary>
        /// Predefined User Accounts DOMAIN_GROUP_RID_COMPUTERS.
        /// </summary>
        public const uint DOMAIN_GROUP_RID_COMPUTERS = 0x00000203;

        /// <summary>
        /// Predefined User Accounts DOMAIN_GROUP_RID_USERS.
        /// </summary>
        public const uint DOMAIN_GROUP_RID_USERS = 0x00000201;

        /// <summary>
        /// Bits to represent Object Type SAM_GROUP_OBJECT.
        /// </summary>
        public const uint SAM_GROUP_OBJECT = 0x10000000;

        /// <summary>
        /// Bits to represent Object Type SAM_ALIAS_OBJECT.
        /// </summary>
        public const uint SAM_ALIAS_OBJECT = 0x20000000;

        /// <summary>
        /// Bits to represent Object Type SAM_USER_OBJECT.
        /// </summary>
        public const uint SAM_USER_OBJECT = 0x30000000;

        /// <summary>
        /// Bits to represent Object Type SAM_NON_SECURITY_GROUP_OBJECT.
        /// </summary>
        public const uint SAM_NON_SECURITY_GROUP_OBJECT = 0x10000001;

        /// <summary>
        /// Bits to represent Object Type SAM_NON_SECURITY_ALIAS_OBJECT.
        /// </summary>
        public const uint SAM_NON_SECURITY_ALIAS_OBJECT = 0x20000001;

        /// <summary>
        /// Bits to represent Object Type SAM_TRUST_ACCOUNT.
        /// </summary>
        public const uint SAM_TRUST_ACCOUNT = 0x30000002;

        /// <summary>
        /// Bits to represent Object Type SAM_MACHINE_ACCOUNT.
        /// </summary>
        public const uint SAM_MACHINE_ACCOUNT = 0x30000001;

        /// <summary>
        /// Bits to represent Group Type GROUP_TYPE_ACCOUNT_GROUP.
        /// </summary>
        public const uint GROUP_TYPE_ACCOUNT_GROUP = 0x00000002;

        /// <summary>
        /// Bits to represent Group Type GROUP_TYPE_RESOURCE_GROUP.
        /// </summary>
        public const uint GROUP_TYPE_RESOURCE_GROUP = 0x00000004;

        /// <summary>
        /// Bits to represent Group Type GROUP_TYPE_UNIVERSAL_GROUP.
        /// </summary>
        public const uint GROUP_TYPE_UNIVERSAL_GROUP = 0x00000008;

        /// <summary>
        /// Bits to represent Group Type GROUP_TYPE_SECURITY_ENABLED.
        /// </summary>
        public const uint GROUP_TYPE_SECURITY_ENABLED = 0x80000000;

        /// <summary>
        /// Bits to represent Group Type GROUP_TYPE_SECURITY_ACCOUNT.
        /// </summary>
        public const uint GROUP_TYPE_SECURITY_ACCOUNT = 0x80000002;

        /// <summary>
        /// Bits to represent Group Type GROUP_TYPE_SECURITY_RESOURCE.
        /// </summary>
        public const uint GROUP_TYPE_SECURITY_RESOURCE = 0x80000004;

        /// <summary>
        /// Bits to represent Group Type GROUP_TYPE_SECURITY_UNIVERSAL.
        /// </summary>
        public const uint GROUP_TYPE_SECURITY_UNIVERSAL = 0x80000008;

        /// <summary>
        /// Values used in QueryDisplayInformation3 Adapter method in QueryInformationGroup method.
        /// </summary>
        public const uint IndexQueryDisp = 0;

        /// <summary>
        /// Values used in QueryDisplayInformation3 Adapter method in QueryInformationGroup method.
        /// </summary>
        public const uint EntryCountQueryDisp = 20;

        /// <summary>
        /// Values used in QueryDisplayInformation3 Adapter method in QueryInformationGroup method.
        /// </summary>
        public const uint PrefMaxLengthQueryDisp = 2000;

        /// <summary>
        /// Values used in QueryDisplayInformation3 Adapter method in QueryInformationGroup method.
        /// </summary>
        public const uint MaxPasswordLength = 1024;

        /// <summary>
        /// Delta time for test in FILETIME.
        /// </summary>
        public const uint DeltaTime = 10000000;

        /// <summary>
        /// The time deviation in milliseconds.
        /// </summary>
        public const uint TimeDeviation = 5000;

        #endregion
    }
}