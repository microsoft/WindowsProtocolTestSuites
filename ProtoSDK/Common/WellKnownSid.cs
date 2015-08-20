// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;


namespace Microsoft.Protocols.TestTools.StackSdk.Dtyp
{
    /// <summary>
    /// WellKnownSid has all Well-Known SIDs defined in MS-DTYP section 2.4.2.3.
    /// </summary>
    public enum WellKnownSid
    {
        /// <summary>
        /// No Security principal.
        /// </summary>
        NULL,

        /// <summary>
        /// A group that includes all users.
        /// </summary>
        EVERYONE,

        /// <summary>
        /// A group that includes all users who have logged on locally.
        /// </summary>
        LOCAL,

        /// <summary>
        /// A group that includes users who are logged on to the physical console.
        /// </summary>
        CONSOLE_LOGON,

        /// <summary>
        /// A placeholder in an inheritable access control entry (ACE).
        /// When the ACE is inherited, the system replaces this SID with 
        /// the SID for the object's creator.
        /// </summary>
        CREATOR_OWNER,

        /// <summary>
        /// A placeholder in an inheritable ACE. When the ACE is inherited,
        /// the system replaces this SID with the SID for the primary 
        /// group of the object's creator.
        /// </summary>
        CREATOR_GROUP,

        /// <summary>
        /// A placeholder in an inheritable ACE. When the ACE is inherited, 
        /// the system replaces this SID with the SID for the primary group
        /// of the object's creator.
        /// </summary>
        OWNER_SERVER,

        /// <summary>
        /// A placeholder in an inheritable ACE. When the ACE is inherited,
        /// the system replaces this SID with the SID for the primary group
        /// of the object's creator.
        /// </summary>
        GROUP_SERVER,

        /// <summary>
        /// A group that represents the current owner of the object.
        /// When an ACE that carries this SID is applied to an object, 
        /// the system ignores the implicit READ_CONTROL and WRITE_DAC 
        /// permissions for the object owner.
        /// </summary>
        OWNER_RIGHTS,

        /// <summary>
        /// An identifier authority.
        /// </summary>
        NT_AUTHORITY,

        /// <summary>
        /// A group that includes all users who have logged on through a dial-up connection
        /// </summary>
        DIALUP,

        /// <summary>
        /// A group that includes all users who have logged on through a network connection.
        /// </summary>
        NETWORK,

        /// <summary>
        /// A group that includes all users who have logged on through a batch queue facility.
        /// </summary>
        BATCH,

        /// <summary>
        /// A group that includes all users who have logged on interactively.
        /// </summary>
        INTERACTIVE,

        /// <summary>
        /// A group that includes all security principals that have logged on as a service
        /// </summary>
        SERVICE,

        /// <summary>
        /// A group that represents an anonymous logon.
        /// </summary>
        ANONYMOUS,

        /// <summary>
        /// Identifies a SECURITY_NT_AUTHORITY Proxy.
        /// </summary>
        PROXY,

        /// <summary>
        /// A group that includes all domain controllers in a forest
        /// that uses an Active Directory directory service.
        /// </summary>
        ENTERPRISE_DOMAIN_CONTROLLERS,

        /// <summary>
        /// A placeholder in an inheritable ACE on an account object or group object in
        /// Active Directory. When the ACE is inherited, the system replaces this SID 
        /// with the SID for the security principal that holds the account.
        /// </summary>
        PRINCIPAL_SELF,

        /// <summary>
        /// A group that includes all users whose identities were 
        /// authenticated when they logged on.
        /// </summary>
        AUTHENTICATED_USERS,

        /// <summary>
        /// This SID is used to control access by untrusted code. ACL validation 
        /// against tokens with RC consists of two checks, one against the token's 
        /// normal list of SIDs and one against a second list (typically containing 
        /// RC - the "RESTRICTED_CODE" token - and a subset of the original token SIDs).
        /// Access is granted only if a token passes both tests. Any ACL that 
        /// specifies RC must also specify WD - the "EVERYONE" token. When RC is
        /// paired with WD in an ACL, a superset of "EVERYONE", including untrusted code,
        /// is described.
        /// </summary>
        RESTRICTED_CODE,

        /// <summary>
        /// A group that includes all users who have logged on to a Terminal Services server.
        /// </summary>
        TERMINAL_SERVER_USER,

        /// <summary>
        /// A group that includes all users who have logged on through a terminal services logon.
        /// </summary>
        REMOTE_INTERACTIVE_LOGON,

        /// <summary>
        /// A group that includes all users from the same organization. 
        /// If this SID is present, the OTHER_ORGANIZATION SID MUST NOT be present.
        /// </summary>
        THIS_ORGANIZATION,

        /// <summary>
        /// An account that is used by the default Internet Information Services (IIS) user.
        /// </summary>
        IUSR,

        /// <summary>
        /// An account that is used by the operating system.
        /// </summary>
        LOCAL_SYSTEM,

        /// <summary>
        /// A local service account.
        /// </summary>
        LOCAL_SERVICE,

        /// <summary>
        /// A network service account.
        /// </summary>
        NETWORK_SERVICE,

        /// <summary>
        /// A global group that includes all user accounts in a domain.
        /// </summary>
        DOMAIN_USERS,

        /// <summary>
        /// built-in Guest account of the domain.
        /// </summary>
        DOMAIN_GUESTS,

        /// <summary>
        /// A global group that includes all clients and servers that have joined the domain.
        /// </summary>
        DOMAIN_COMPUTERS,

        /// <summary>
        /// A global group that includes all domain controllers in the domain.
        /// </summary>
        DOMAIN_DOMAIN_CONTROLLERS,

        /// <summary>
        /// A global group that includes all computers that are running an enterprise certification 
        /// authority. Cert Publishers are authorized to publish certificates for User objects in
        /// Active Directory.
        /// </summary>
        CERT_PUBLISHERS,

        /// <summary>
        /// A universal group in a native-mode domain, or a global group in a mixed-mode domain.
        /// The group is authorized to make schema changes in Active Directory
        /// </summary>
        SCHEMA_ADMINISTRATORS,

        /// <summary>
        /// A universal group in a native-mode domain, or a global group in a mixed-mode domain.
        /// The group is authorized to make forestwide changes in Active Directory, 
        /// such as adding child domains.
        /// </summary>
        ENTERPRISE_ADMINS,

        /// <summary>
        /// A domain local group for Remote Access Services (RAS) servers. Servers in this
        /// group have Read Account Restrictions and Read Logon Information access to User
        /// objects in the Active Directory domain local group.
        /// </summary>
        RAS_SERVERS,

        /// <summary>
        /// A global group that is authorized to create new Group Policy objects in Active Directory.
        /// </summary>
        GROUP_POLICY_CREATOR_OWNERS,

        /// <summary>
        /// A built-in group. After the initial installation of the operating system,
        /// the only member of the group is the Administrator account. When a computer 
        /// joins a domain, the Domain Administrators group is added to the 
        /// Administrators group. When a server becomes a domain controller,
        /// the Enterprise Administrators group also is added to the Administrators group.
        /// </summary>
        BUILTIN_ADMINISTRATORS,

        /// <summary>
        /// A built-in group. After the initial installation of the operating system,
        /// the only member is the Authenticated Users group. When a computer joins a 
        /// domain, the Domain Users group is added to the Users group on the computer.
        /// </summary>
        BUILTIN_USERS,

        /// <summary>
        /// A built-in group. The Guests group allows users to log on
        /// with limited privileges to a computer's built-in Guest account.
        /// </summary>
        BUILTIN_GUESTS,

        /// <summary>
        /// A built-in group. Power users can perform the following actions:
        /// Create local users and groups.
        /// Modify and delete accounts that they have created.
        /// Remove users from the Power Users, Users, and Guests groups.
        /// Install programs.
        /// Create, manage, and delete local printers.
        /// Create and delete file shares
        /// </summary>
        POWER_USERS,

        /// <summary>
        /// A built-in group that exists only on domain controllers. 
        /// Account Operators have permission to create, modify, and delete 
        /// accounts for users, groups, and computers in all containers and 
        /// organizational units of Active Directory except the Built-in container
        /// and the Domain Controllers OU. Account Operators do not have permission
        /// to modify the Administrators and Domain Administrators groups, nor do 
        /// they have permission to modify the accounts for members of those groups.
        /// </summary>
        ACCOUNT_OPERATORS,

        /// <summary>
        /// A built-in group that exists only on domain controllers.
        /// Server Operators can perform the following actions:
        /// Log on to a server interactively.
        /// Create and delete network shares.
        /// Start and stop services.
        /// Back up and restore files.
        /// Format the hard disk of a computer.
        /// Shut down the computer.
        /// </summary>
        SERVER_OPERATORS,

        /// <summary>
        /// A built-in group that exists only on domain controllers.
        /// Print Operators can manage printers and document queues.
        /// </summary>
        PRINTER_OPERATORS,

        /// <summary>
        /// A built-in group. Backup Operators can back up and restore all files on a computer, 
        /// regardless of the permissions that protect those files.
        /// </summary>
        BACKUP_OPERATORS,

        /// <summary>
        /// A built-in group that is used by the File Replication Service (FRS) on domain controllers..
        /// </summary>
        REPLICATOR,

        /// <summary>
        /// A backward compatibility group that allows read access on all
        /// users and groups in the domain.
        /// </summary>
        ALIAS_PREW2KCOMPACC,

        /// <summary>
        /// An alias. Members of this group are granted the right to log on remotely.
        /// </summary>
        REMOTE_DESKTOP,

        /// <summary>
        /// An alias. Members of this group can have some administrative
        /// privileges to manage configuration of networking features.
        /// </summary>
        NETWORK_CONFIGURATION_OPS,

        /// <summary>
        /// An alias. Members of this group can create incoming, one-way trusts to this forest.
        /// </summary>
        INCOMING_FOREST_TRUST_BUILDERS,

        /// <summary>
        /// An alias. Members of this group have remote access to monitor this computer.
        /// </summary>
        PERFMON_USERS,

        /// <summary>
        /// An alias. Members of this group have remote access to schedule the
        /// logging of performance counters on this computer.
        /// </summary>
        PERFLOG_USERS,

        /// <summary>
        /// An alias. Members of this group have access to the computed 
        /// tokenGroupsGlobalAndUniversal attribute on User objects.
        /// </summary>
        WINDOWS_AUTHORIZATION_ACCESS_GROUP,

        /// <summary>
        /// An alias. A group for Terminal Server License Servers.
        /// </summary>
        TERMINAL_SERVER_LICENSE_SERVERS,

        /// <summary>
        /// An alias. A group for COM to provide computer-wide access controls that govern 
        /// access to all call, activation, or launch requests on the computer.
        /// </summary>
        DISTRIBUTED_COM_USERS,

        /// <summary>
        /// A built-in group account for IIS users.
        /// </summary>
        IIS_IUSRS,

        /// <summary>
        /// A built-in group account for cryptographic operators.
        /// </summary>
        CRYPTOGRAPHIC_OPERATORS,

        /// <summary>
        /// A built-in local group. Members of this group can read event 
        /// logs from the local machine.
        /// </summary>
        EVENT_LOG_READERS,

        /// <summary>
        /// A built-in local group. Members of this group are allowed to
        /// connect to Certification Authorities in the enterprise.
        /// </summary>
        CERTIFICATE_SERVICE_DCOM_ACCESS,

        /// <summary>
        /// A SID that allows objects to have an ACL that lets any service 
        /// process with a write-restricted token to write to the object.
        /// </summary>
        WRITE_RESTRICTED,

        /// <summary>
        /// A SID that is used when the NTLM authentication package authenticated the client.
        /// </summary>
        NTLM_AUTHENTICATION,

        /// <summary>
        /// A SID that is used when the SChannel authentication package authenticated the client.
        /// </summary>
        SCHANNEL_AUTHENTICATION,

        /// <summary>
        /// A SID that is used when the Digest authentication package authenticated the client.
        /// </summary>
        DIGEST_AUTHENTICATION,

        /// <summary>
        /// An NT Service account prefix.
        /// </summary>
        NT_SERVICE,

        /// <summary>
        /// A group that includes all users and computers from another organization. 
        /// If this SID is present, THIS_ORGANIZATION SID MUST NOT be present.
        /// </summary>
        OTHER_ORGANIZATION,

        /// <summary>
        /// An untrusted integrity level.
        /// </summary>
        ML_UNTRUSTED,

        /// <summary>
        /// A low integrity level.
        /// </summary>
        ML_LOW,

        /// <summary>
        /// A medium integrity level.
        /// </summary>
        ML_MEDIUM,

        /// <summary>
        /// A medium-plus integrity level.
        /// </summary>
        ML_MEDIUM_PLUS,

        /// <summary>
        /// A high integrity level.
        /// </summary>
        ML_HIGH,

        /// <summary>
        /// A system integrity level.
        /// </summary>
        ML_SYSTEM,

        /// <summary>
        /// A protected-process integrity level.
        /// </summary>
        ML_PROTECTED_PROCESS,
    }
}