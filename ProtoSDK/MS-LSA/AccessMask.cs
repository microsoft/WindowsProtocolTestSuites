// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa
{
    /// <summary>
    ///  The ACCESS_MASK data type is a bitmask that defines the access rights
    ///  to grant an object. Access types are reconciled with the discretionary
    ///  access control list (DACL) of the object to determine whether the 
    ///  requested access is granted or denied.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    [CLSCompliant(false)]
    public enum ACCESS_MASK : uint
    {
        /// <summary>
        /// Delete object.
        /// </summary>
        DELETE = 0x00010000,

        /// <summary>
        /// The read value of a DACL and owner in a security descriptor.
        /// </summary>
        READ_CONTROL = 0x00020000,

        /// <summary>
        /// The write value of a DACL in a security descriptor.
        /// </summary>
        WRITE_DAC = 0x00040000,

        /// <summary>
        /// The write value of the owner in a security descriptor.
        /// </summary>
        WRITE_OWNER = 0x00080000,

        /// <summary>
        /// Used in requesting access; get as much access as the server will allow.
        /// </summary>
        MAXIMUM_ALLOWED = 0x02000000,

        /// <summary>
        /// <para>
        /// When used with policy object, translated to (0x00020006):
        /// POLICY_VIEW_AUDIT_INFORMATION | POLICY_GET_PRIVATE_INFORMATION | READ_CONTROL
        /// </para>
        /// <para>
        /// When used with account object, translated to (0x00020001):
        /// ACCOUNT_VIEW | READ_CONTROL
        /// </para>
        /// <para>
        /// When used with secret object, translated to (0x00020002):
        /// SECRET_QUERY_VALUE | READ_CONTROL
        /// </para>
        /// <para>
        /// When used with trusted domain object, translated to (0x00020001):
        /// TRUSTED_QUERY_DOMAIN_NAME | READ_CONTROL
        /// </para>
        /// </summary>
        ValueToBeTranslated0x80000000 = 0x80000000,

        /// <summary>
        /// <para>
        /// When used with policy object, translated to (0x000207F8):
        /// POLICY_TRUST_ADMIN | POLICY_CREATE_ACCOUNT | POLICY_CREATE_SECRET | POLICY_CREATE_PRIVILEGE |
        /// POLICY_SET_DEFAULT_QUOTA_LIMITS | POLICY_SET_AUDIT_REQUIREMENTS | POLICY_AUDIT_LOG_ADMIN |
        /// POLICY_SERVER_ADMIN | READ_CONTROL
        /// </para>
        /// <para>
        /// When used with account object, translated to (0x0002000E):
        /// ACCOUNT_ADJUST_PRIVILEGES | ACCOUNT_ADJUST_QUOTAS | ACCOUNT_ADJUST_SYSTEM_ACCESS | READ_CONTROL
        /// </para>
        /// <para>
        /// When used with secret object, translated to (0x00020001):
        /// SECRET_SET_VALUE | READ_CONTROL
        /// </para>
        /// <para>
        /// When used with trusted domain object, translated to (0x00020034):
        /// TRUSTED_SET_CONTROLLERS | TRUSTED_SET_POSIX | TRUSTED_SET_AUTH | READ_CONTROL
        /// </para>
        /// </summary>
        ValueToBeTranslated0x40000000 = 0x40000000,

        /// <summary>
        /// <para>
        /// When used with policy object, translated to (0x00020801):
        /// POLICY_VIEW_LOCAL_INFORMATION | POLICY_LOOKUP_NAMES | READ_CONTROL
        /// </para>
        /// <para>
        /// When used with account object, translated to (0x00020000):
        /// READ_CONTROL
        /// </para>
        /// <para>
        /// When used with account object, translated to (0x00020000):
        /// READ_CONTROL
        /// </para>
        /// <para>
        /// When used with trusted domain object, translated to (0x0002000C):
        /// TRUSTED_QUERY_DOMAIN_NAME | TRUSTED_QUERY_POSIX | READ_CONTROL
        /// </para>
        /// </summary>
        ValueToBeTranslated0x20000000 = 0x20000000,

        /// <summary>
        /// <para>
        /// When used with policy object, translated to (0x000F0FFF):
        /// POLICY_VIEW_LOCAL_INFORMATION | POLICY_VIEW_AUDIT_INFORMATION | POLICY_GET_PRIVATE_INFORMATION |
        /// POLICY_TRUST_ADMIN | POLICY_CREATE_ACCOUNT | POLICY_CREATE_SECRET | POLICY_CREATE_PRIVILEGE |
        /// POLICY_SET_DEFAULT_QUOTA_LIMITS | POLICY_SET_AUDIT_REQUIREMENTS | POLICY_AUDIT_LOG_ADMIN | 
        /// POLICY_SERVER_ADMIN | POLICY_LOOKUP_NAMES | DELETE | READ_CONTROL | WRITE_DAC | WRITE_OWNER
        /// </para>
        /// <para>
        /// When used with account object, translated to (0x000F000F):
        /// ACCOUNT_VIEW | ACCOUNT_ADJUST_PRIVILEGES | ACCOUNT_ADJUST_QUOTAS | ACCOUNT_ADJUST_SYSTEM_ACCESS | DELETE |
        /// READ_CONTROL | WRITE_DAC | WRITE_OWNER
        /// </para>
        /// <para>
        /// When used with secret object, translated to (0x000F0003):
        /// SECRET_QUERY_VALUE | SECRET_SET_VALUE | DELETE | READ_CONTROL | WRITE_DAC | WRITE_OWNER
        /// </para>
        /// <para>
        /// When used with trusted domain object, translated to (0x000F007F):
        /// TRUSTED_QUERY_DOMAIN_NAME | TRUSTED_QUERY_CONTROLLERS | TRUSTED_SET_CONTROLLERS | TRUSTED_QUERY_POSIX |
        /// TRUSTED_SET_POSIX | TRUSTED_SET_AUTH | TRUSTED_QUERY_AUTH | DELETE | READ_CONTROL | WRITE_DAC | WRITE_OWNER
        /// </para>
        /// </summary>
        ValueToBeTranslated0x10000000 = 0x10000000,

        /// <summary>
        /// No access.
        /// </summary>
        NONE = 0x00000000,

        /// <summary>
        /// Access to view local information.
        /// </summary>
        POLICY_VIEW_LOCAL_INFORMATION = 0x00000001,

        /// <summary>
        /// Access to view audit information.
        /// </summary>
        POLICY_VIEW_AUDIT_INFORMATION = 0x00000002,

        /// <summary>
        /// Access to view private information.
        /// </summary>
        POLICY_GET_PRIVATE_INFORMATION = 0x00000004,

        /// <summary>
        /// Access to administer trust relationships.
        /// </summary>
        POLICY_TRUST_ADMIN = 0x00000008,

        /// <summary>
        /// POLICY_CREATE_SECRET
        /// </summary>
        POLICY_CREATE_ACCOUNT = 0x00000010,

        /// <summary>
        /// Access to create secret objects.
        /// </summary>
        POLICY_CREATE_SECRET = 0x00000020,

        /// <summary>
        /// Access to create privileges.
        /// Note New privilege creation is not currently a part of the protocol, so this flag is not actively used.
        /// </summary>
        POLICY_CREATE_PRIVILEGE = 0x00000040,

        /// <summary>
        /// Access to set default quota limits.
        /// </summary>
        POLICY_SET_DEFAULT_QUOTA_LIMITS = 0x00000080,

        /// <summary>
        /// Access to set audit requirements.
        /// </summary>
        POLICY_SET_AUDIT_REQUIREMENTS = 0x00000100,

        /// <summary>
        /// Access to administer the audit log.
        /// </summary>
        POLICY_AUDIT_LOG_ADMIN = 0x00000200,

        /// <summary>
        /// Access to administer policy on the server.
        /// </summary>
        POLICY_SERVER_ADMIN = 0x00000400,

        /// <summary>
        /// Access to translate names and security identifiers (SIDs).
        /// </summary>
        POLICY_LOOKUP_NAMES = 0x00000800,

        /// <summary>
        /// Access to be notified of policy changes.
        /// </summary>
        POLICY_NOTIFICATION = 0x00001000,

        /// <summary>
        /// View account information.
        /// </summary>
        ACCOUNT_VIEW = 0x00000001,

        /// <summary>
        /// Change privileges on an account.
        /// </summary>
        ACCOUNT_ADJUST_PRIVILEGES = 0x00000002,

        /// <summary>
        /// Change quotas on an account.
        /// </summary>
        ACCOUNT_ADJUST_QUOTAS = 0x00000004,

        /// <summary>
        /// Change system access.
        /// </summary>
        ACCOUNT_ADJUST_SYSTEM_ACCESS = 0x00000008,

        /// <summary>
        /// Set secret value.
        /// </summary>
        SECRET_SET_VALUE = 0x00000001,

        /// <summary>
        /// Query secret value.
        /// </summary>
        SECRET_QUERY_VALUE = 0x00000002,

        /// <summary>
        /// View domain name information.
        /// </summary>
        TRUSTED_QUERY_DOMAIN_NAME = 0x00000001,

        /// <summary>
        /// View "Domain Controllers" information.
        /// </summary>
        TRUSTED_QUERY_CONTROLLERS = 0x00000002,

        /// <summary>
        /// Change "Domain Controllers" information.
        /// </summary>
        TRUSTED_SET_CONTROLLERS = 0x00000004,

        /// <summary>
        /// View POSIX information.
        /// </summary>
        TRUSTED_QUERY_POSIX = 0x00000008,

        /// <summary>
        /// Change POSIX information.
        /// </summary>
        TRUSTED_SET_POSIX = 0x00000010,

        /// <summary>
        /// Change authentication information.
        /// </summary>
        TRUSTED_SET_AUTH = 0x00000020,

        /// <summary>
        /// View authentication information.
        /// </summary>
        TRUSTED_QUERY_AUTH = 0x00000040,
    }
}
