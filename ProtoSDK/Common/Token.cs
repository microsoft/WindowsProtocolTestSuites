// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;

namespace Microsoft.Protocols.TestTools.StackSdk.Dtyp
{
    /// <summary>
    /// The names of privileges.
    /// </summary>
    public enum PrivilegeName
    {
        /// <summary>
        /// None.
        /// </summary>
        NONE,

        /// <summary>
        ///  Replace a process-level token.
        /// </summary>
        SE_ASSIGNPRIMARYTOKEN_NAME,

        /// <summary>
        ///  Generate security audits. 
        /// </summary>
        SE_AUDIT_NAME,

        /// <summary>
        ///  Back up files and directories
        /// </summary> 
        SE_BACKUP_NAME,

        /// <summary>
        ///  Bypass traverse checking. 
        /// </summary> 
        SE_CHANGE_NOTIFY_NAME,

        /// <summary>
        ///  Create global objects.
        /// </summary> 
        SE_CREATE_GLOBAL_NAME,

        /// <summary>
        ///  Create a page file. 
        /// </summary>
        SE_CREATE_PAGEFILE_NAME,

        /// <summary>
        ///  Create permanent shared objects. 
        /// </summary>
        SE_CREATE_PERMANENT_NAME,

        /// <summary>
        ///  Create a token object.
        /// </summary>
        SE_CREATE_TOKEN_NAME,

        /// <summary>
        ///  Debug programs.
        /// </summary>
        SE_DEBUG_NAME,

        /// <summary>
        /// Enable computer and user accounts to be trusted for delegation.
        /// </summary> 
        SE_ENABLE_DELEGATION_NAME,

        /// <summary>
        ///  Impersonate a client after authentication.
        /// </summary>
        SE_IMPERSONATE_NAME,

        /// <summary>
        ///  Increase scheduling priority.
        /// </summary>
        SE_INC_BASE_PRIORITY_NAME,

        /// <summary>
        ///  Adjust memory quotas for a process. 
        /// </summary>
        SE_INCREASE_QUOTA_NAME,

        /// <summary>
        /// Load and unload device drivers.  
        /// </summary>
        SE_LOAD_DRIVER_NAME,

        /// <summary>
        ///  Lock pages in memory.
        /// </summary>
        SE_LOCK_MEMORY_NAME,

        /// <summary>
        ///  Add workstations to domain.
        /// </summary> 
        SE_MACHINE_ACCOUNT_NAME,

        /// <summary>
        ///  Manage the files on a volume.
        /// </summary>
        SE_MANAGE_VOLUME_NAME,

        /// <summary>
        ///  Profile single process. 
        /// </summary>
        SE_PROF_SINGLE_PROCESS_NAME,

        /// <summary>
        /// Force shutdown from a remote system. 
        /// </summary>
        SE_REMOTE_SHUTDOWN_NAME,

        /// <summary>
        /// Restore files and directories.
        /// </summary>
        SE_RESTORE_NAME,

        /// <summary>
        ///  Manage auditing and security log. 
        /// </summary>
        SE_SECURITY_NAME,

        /// <summary>
        ///  Shut down the system. 
        /// </summary>
        SE_SHUTDOWN_NAME,

        /// <summary>
        ///  Synchronize directory service data.
        /// </summary> 
        SE_SYNC_AGENT_NAME,

        /// <summary>
        /// Modify firmware environment values. 
        /// </summary>
        SE_SYSTEM_ENVIRONMENT_NAME,

        /// <summary>
        ///  Profile system performance. 
        /// </summary>
        SE_SYSTEM_PROFILE_NAME,

        /// <summary>
        /// Change system time.  
        /// </summary>
        SE_SYSTEMTIME_NAME,

        /// <summary>
        ///  Take ownership of files or other objects. 
        /// </summary>
        SE_TAKE_OWNERSHIP_NAME,

        /// <summary>
        ///  Act as part of the operating system.
        /// </summary>
        SE_TCB_NAME,

        /// <summary>
        ///  Remove computer from docking station.
        /// </summary>
        SE_UNDOCK_NAME,

        /// <summary>
        ///  Create symbolic links.
        /// </summary>
        SE_CREATE_SYMBOLIC_LINK_NAME,

        /// <summary>
        ///  Increase a process working set. 
        /// </summary>
        SE_INC_WORKING_SET_NAME,

        /// <summary>
        /// Modify an object label. 
        /// </summary>
        SE_RELABEL_NAME,

        /// <summary>
        ///  Change time zone.
        /// </summary>
        SE_TIME_ZONE_NAME,

        /// <summary>
        /// Access Credential Manager as a trusted caller. 
        /// </summary>
        SE_TRUSTED_CREDMAN_ACCESS_NAME,
    }


    /// <summary>
    ///  The LUID structure containing 64 bits that define a
    ///  locally unique identifier (LUID).
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\b9f30c12-96d8-459e-80c3-66c4ceabccb2.xml
    //  </remarks>
    public partial struct _LUID
    {
        /// <summary>
        ///  Low-order 32 bits.
        /// </summary>
        public uint LowPart;

        /// <summary>
        ///  High-order 32 bits.
        /// </summary>
        public int HighPart;
    }


    /// <summary>
    /// The mandatory integrity access policy for the associated token. This can be one of the following values.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [Flags]
    public enum TokenMandatoryPolicyValue : uint
    {
        /// <summary>
        ///  No mandatory integrity policy is enforced for the token.
        /// </summary>
        TOKEN_MANDATORY_POLICY_OFF = 0x0,

        /// <summary>
        /// A process associated with the token cannot write to objects that have a greater mandatory integrity level.
        /// </summary>
        TOKEN_MANDATORY_POLICY_NO_WRITE_UP = 0x1,

        /// <summary>
        /// A process created with the token has an integrity level that is the lesser of the parent-process integrity level and the executable-file integrity level.
        /// </summary>
        TOKEN_MANDATORY_POLICY_NEW_PROCESS_MIN = 0x2,

        /// <summary>
        /// A combination of TOKEN_MANDATORY_POLICY_NO_WRITE_UP and TOKEN_MANDATORY_POLICY_NEW_PROCESS_MIN.
        /// </summary>
        TOKEN_MANDATORY_POLICY_VALID_MASK = 0x3,
    }


    /// <summary>
    /// The authorization context, also referred to as a Token, is a collection of the groups associated
    /// with the client principal, as well as additional optional policy information.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// An array that contains the SID of the user account itself, and the SIDs of all groups to which 
        /// the user belongs. Ordering of the SIDs is not required.
        /// </summary>
        public _SID[] Sids;

        /// <summary>
        /// A set of administrative or security-relevant privileges associated with this authorization context.
        /// </summary>
        public _LUID[] Privileges;

        /// <summary>
        ///  An index into the Sids[] array that indicates which SID indicates ownership.
        /// </summary>
        public uint UserIndex;

        /// <summary>
        /// An index into the Sids[] array that indicates which SID should be assigned as the owner
        /// for new objects. This value is determined by local policy in an implementation-specific
        /// manner. Ownership is often used, by way of example, for accounting for file storage space
        /// on a file server. This value may be the same as the UserIndex attribute, but is not 
        /// required to be; this allows, for example, quota or ownership of objects to be assigned 
        /// to groups rather than individuals.
        /// </summary>
        public uint OwnerIndex;

        /// <summary>
        ///  An index into the Sids[] array that indicates which SID should be used as the primary group of the user.
        /// </summary>
        public uint PrimaryGroup;

        /// <summary>
        /// A DACL, as defined in section 2.4.5, that can be applied to new objects when there is no 
        /// parent security descriptor for inheritance and no explicit new security descriptor was 
        /// supplied by the client.
        /// </summary>
        public _ACL DefaultDACL;

        /// <summary>
        /// A separate SID, not used for general access decisions like the Sids[] array above, that
        /// indicates the mandatory integrity level of this principal.
        /// </summary>
        public _SID? IntegrityLevelSid;

        /// <summary>
        /// The TOKEN_MANDATORY_POLICY structure specifies the token's mandatory integrity policy.
        /// </summary>
        public TokenMandatoryPolicyValue MandatoryPolicy;
    }
}
