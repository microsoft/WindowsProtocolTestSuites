// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.Protocols.TestTools.StackSdk.Dtyp
{
    /// <summary>
    /// An utility class of MS-DTYP.
    /// </summary>
    public static class DtypUtility
    {
        /// <summary>
        /// The length of _ACL header, including _ACL.AclRevision, _ACL.Sbz1 ,_ACL.AclSize,
        /// _ACL.AceCount and _ACL.Sbz2, in bytes.
        /// </summary>
        private const ushort ACL_HEADER_LENGTH = 8;

        /// <summary>
        /// The length of _ACE_HEADER in bytes.
        /// </summary>
        private const ushort ACE_HEADER_LENGTH = 4;

        //The short fixed ace length in bytes contains the following parts:
        //_ACE_HEADER Header; (4 bytes)
        //uint Mask; (4 bytes)
        private const ushort SHORT_FIXED_ACE_LENGTH = 8;

        //The long fixed ace length in bytes contains the following parts:
        //_ACE_HEADER Header; (4 bytes)
        //ACCESS_OBJECT_ACE_Mask Mask; (4 bytes)
        //ACCESS_OBJECT_ACE_Flags Flags;(4 bytes)
        //Guid ObjectType;(16 bytes)
        //Guid InheritedObjectType;(16 bytes)
        private const ushort LONG_FIXED_ACE_LENGTH = 44;

        // Length of SID authority byte array.
        private const int SID_AUTHORITY_LENGTH = 6;

        /// <summary>
        /// Specifies access to delete an object.
        /// </summary>
        public const uint ACCESS_MASK_DELETE = 0x00010000;

        /// <summary>
        /// Specifies access to read the security descriptor of an object.
        /// </summary>
        public const uint ACCESS_MASK_READ_CONTROL = 0x00020000;

        /// <summary>
        /// Specifies access to change the discretionary access control list of the security descriptor 
        /// of an object.
        /// </summary>
        public const uint ACCESS_MASK_WRITE_DAC = 0x00040000;

        /// <summary>
        /// Specifies access to change the owner of the object as listed in the security descriptor.
        /// </summary>
        public const uint ACCESS_MASK_WRITE_OWNER = 0x00080000;

        /// <summary>
        /// Specifies access to the object sufficient to synchronize or wait on the object.
        /// </summary>
        public const uint ACCESS_MASK_SYNCHRONIZE = 0x00100000;

        /// <summary>
        ///  When used in an Access Request operation: When requested, this bit grants the requestor the
        ///  right to change the SACL of an object.
        /// </summary>
        public const uint ACCESS_MASK_ACCESS_SYSTEM_SECURITY = 0x01000000;

        /// <summary>
        /// When used in an Access Request operation: When requested, this bit grants the requestor the 
        /// maximum permissions allowed to the object through the Access Check Algorithm. 
        /// When used to set the Security Descriptor on an object: Specifying the Maximum Allowed bit in
        /// the SECURITY_DESCRIPTOR has no meaning. 
        /// </summary>
        public const uint ACCESS_MASK_MAXIMAL_ALLOWED = 0x02000000;

        /// <summary>
        ///  When used in an Access Request operation: When all access permissions to an object are 
        ///  requested, this bit is translated to a combination of bits, which are usually set in the 
        ///  lower 16 bits of the ACCESS_MASK_ 
        ///  When used to set the Security Descriptor on an object: When the GA bit is set in an ACE 
        ///  that is to be attached to an object, it is translated into a combination of bits, which 
        ///  are usually set in the lower 16 bits of the ACCESS_MASK_        
        /// </summary>
        public const uint ACCESS_MASK_GENERIC_ALL = 0x10000000;

        /// <summary>
        ///  When used in an Access Request operation: When execute access to an object is requested,
        ///  this bit is translated to a combination of bits, which are usually set in the lower 16 
        ///  bits of the ACCESS_MASK_
        ///  When used to set the Security Descriptor on an object: When the GX bit is set in an ACE 
        ///  that is to be attached to an object, it is translated into a combination of bits, which
        ///  are usually set in the lower 16 bits of the ACCESS_MASK_ 
        /// </summary>
        public const uint ACCESS_MASK_GENERIC_EXECUTE = 0x20000000;

        /// <summary>
        ///  When used in an Access Request operation: When write access to an object is requested, 
        ///  this bit is translated to a combination of bits, which are usually set in the lower 16 bits
        ///  of the ACCESS_MASK_
        ///  When used to set the Security Descriptor on an object: When the GW bit is set in an ACE
        ///  that is to be attached to an object, it is translated into a combination of bits, which are
        ///  usually set in the lower 16 bits of the ACCESS_MASK_
        /// </summary>
        public const uint ACCESS_MASK_GENERIC_WRITE = 0x40000000;

        /// <summary>
        ///  When used in an Access Request operation: When read access to an object is requested, this
        ///  bit is translated to a combination of bits.
        ///  When used to set the Security Descriptor on an object: When the GR bit is set in an ACE
        ///  that is to be attached to an object, it is translated into a combination of bits, which are 
        ///  usually set in the lower 16 bits of the ACCESS_MASK_
        /// </summary>
        public const uint ACCESS_MASK_GENERIC_READ = 0x80000000;

        /// <summary>
        /// All bits of the standard rights.
        /// </summary>
        public const uint ACCESS_MASK_STANDARD_RIGHTS_ALL = 0x001F0000;

        /// <summary>
        /// All bits of the protocol specific rights.
        /// </summary>
        public const uint ACCESS_MASK_SPECIFIC_RIGHTS_ALL = 0x0000FFFF;

        /// <summary>
        /// Initialize a new instance of FILETIME structure from the specified DateTime object.
        /// </summary>
        /// <param name="dateTime">
        /// The DateTime object used to initialize the value of the new instance.
        /// </param>
        /// <returns>A FILETIME.</returns>
        public static _FILETIME ToFileTime(DateTime dateTime)
        {
            _FILETIME retVal;

            if (dateTime == DateTime.MaxValue.ToUniversalTime())
            {
                retVal.dwLowDateTime = 0xFFFFFFFFu;
                // According to TD section 2.5 KERB_VALIDATION_INFO,
                // "(If the session should not expire,)
                // (If the client should not be logged off,)
                // (If the password will not expire,)
                // this structure SHOULD have the
                // dwHighDateTime member set to 0x7FFFFFFF and the dwLowDateTime member
                // set to 0xFFFFFFFF."
                retVal.dwHighDateTime = 0x7FFFFFFFu;
            }
            else
            {
                long fileTime = dateTime.ToFileTimeUtc();
                retVal.dwLowDateTime = (uint)(fileTime & 0xFFFFFFFF);
                retVal.dwHighDateTime = (uint)((fileTime >> 32) & 0xFFFFFFFF);
            }

            return retVal;
        }

        /// <summary>
        /// Converts the value of the current FILETIME object to an equivalent DateTime.
        /// </summary>
        /// <param name="fileTime">FILETIME to convert.</param>
        /// <returns>A DateTime object that represents a UTC time equivalent to the date
        /// and time represented by the fileTime parameter.</returns>
        public static DateTime ToDateTime(_FILETIME fileTime)
        {
            long fileTimeLong = (((long)fileTime.dwHighDateTime) << 32) | fileTime.dwLowDateTime;
            if (fileTimeLong > DateTime.MaxValue.ToFileTimeUtc())
            {
                return DateTime.MaxValue.ToUniversalTime();
            }

            return DateTime.FromFileTimeUtc(fileTimeLong);
        }


        /// <summary>
        /// Specifies the NULL SID authority. It defines only the 
        /// NULL well-known-SID: S-1-0-0.
        /// </summary>
        public static byte[] NULL_SID_AUTHORITY
        {
            get
            {
                return new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            }
        }

        /// <summary>
        /// Specifies the World SID authority. It only defines the 
        /// Everyone well-known-SID: S-1-1-0.
        /// </summary>
        public static byte[] WORLD_SID_AUTHORITY
        {
            get
            {
                return new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
            }
        }

        /// <summary>
        /// Specifies the Local SID authority. It defines only the 
        /// Local well-known-SID: S-1-2-0.
        /// </summary>
        public static byte[] LOCAL_SID_AUTHORITY
        {
            get
            {
                return new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x02 };
            }
        }

        /// <summary>
        /// Specifies the Creator SID authority. It defines the 
        /// Creator Owner, Creator Group, and Creator Owner Server 
        /// well-known-SIDs: S-1-3-0, S-1-3-1, and S-1-3-2. 
        /// These SIDs are used as placeholders in an access control 
        /// list (ACL) and are replaced by the user, group, and machine 
        /// SIDs of the security principal.
        /// </summary>
        public static byte[] CREATOR_SID_AUTHORITY
        {
            get
            {
                return new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x03 };
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static byte[] NON_UNIQUE_AUTHORITY
        {
            get
            {
                return new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x04 };
            }
        }

        /// <summary>
        /// Specifies the Windows NT security subsystem SID authority. 
        /// It defines all other SIDs in the forest.
        /// </summary>
        public static byte[] NT_AUTHORITY
        {
            get
            {
                return new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x05 };
            }
        }

        /// <summary>
        /// Specifies the Mandatory label authority. It defines the integrity level SIDs.
        /// </summary>
        public static byte[] SECURITY_MANDATORY_LABEL_AUTHORITY
        {
            get
            {
                return new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x10 };
            }
        }

        /// <summary>
        /// Retrieve Well-Known SID defined in MS-DTYP section 2.4.2.3.
        /// </summary>
        /// <param name="type">The SID type to retrieve.</param>
        /// <param name="domain">
        /// The domain or root-domain represents the three sub-authority values if required in SID. 
        /// It can be null for most SID.
        /// </param>
        /// <returns>Well-Known SID.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when type is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown when domain is null for DOMAIN_USERS, DOMAIN_GUESTS, DOMAIN_COMPUTERS, 
        /// DOMAIN_DOMAIN_CONTROLLERS, CERT_PUBLISHERS, SCHEMA_ADMINISTRATORS, 
        /// ENTERPRISE_ADMINS, RAS_SERVERS, GROUP_POLICY_CREATOR_OWNERS and 
        /// BUILTIN_ADMINISTRATORS.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Throw when domain is invalid.
        /// </exception>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public static _SID GetWellKnownSid(WellKnownSid type, uint[] domain)
        {
            switch (type)
            {
                case WellKnownSid.DOMAIN_USERS:
                case WellKnownSid.DOMAIN_GUESTS:
                case WellKnownSid.DOMAIN_COMPUTERS:
                case WellKnownSid.DOMAIN_DOMAIN_CONTROLLERS:
                case WellKnownSid.CERT_PUBLISHERS:
                case WellKnownSid.SCHEMA_ADMINISTRATORS:
                case WellKnownSid.ENTERPRISE_ADMINS:
                case WellKnownSid.RAS_SERVERS:
                case WellKnownSid.GROUP_POLICY_CREATOR_OWNERS:
                    if (domain == null)
                    {
                        throw new ArgumentNullException("domain");
                    }
                    if (domain.Length != 3)
                    {
                        throw new ArgumentException("The domain parameter is invalid", "domain");
                    }
                    break;
            }

            switch (type)
            {
                case WellKnownSid.NULL:
                    return CreateWellKnownSid(0, 0);
                case WellKnownSid.EVERYONE:
                    return CreateWellKnownSid(1, 0);
                case WellKnownSid.LOCAL:
                    return CreateWellKnownSid(2, 0);
                case WellKnownSid.CONSOLE_LOGON:
                    return CreateWellKnownSid(2, 1);
                case WellKnownSid.CREATOR_OWNER:
                    return CreateWellKnownSid(3, 0);
                case WellKnownSid.CREATOR_GROUP:
                    return CreateWellKnownSid(3, 1);
                case WellKnownSid.OWNER_SERVER:
                    return CreateWellKnownSid(3, 2);
                case WellKnownSid.GROUP_SERVER:
                    return CreateWellKnownSid(3, 3);
                case WellKnownSid.OWNER_RIGHTS:
                    return CreateWellKnownSid(3, 4);
                case WellKnownSid.NT_AUTHORITY:
                    return CreateWellKnownSid(5);
                case WellKnownSid.DIALUP:
                    return CreateWellKnownSid(5, 1);
                case WellKnownSid.NETWORK:
                    return CreateWellKnownSid(5, 2);
                case WellKnownSid.BATCH:
                    return CreateWellKnownSid(5, 3);
                case WellKnownSid.INTERACTIVE:
                    return CreateWellKnownSid(5, 4);
                case WellKnownSid.SERVICE:
                    return CreateWellKnownSid(5, 6);
                case WellKnownSid.ANONYMOUS:
                    return CreateWellKnownSid(5, 7);
                case WellKnownSid.PROXY:
                    return CreateWellKnownSid(5, 8);
                case WellKnownSid.ENTERPRISE_DOMAIN_CONTROLLERS:
                    return CreateWellKnownSid(5, 9);
                case WellKnownSid.PRINCIPAL_SELF:
                    return CreateWellKnownSid(5, 10);
                case WellKnownSid.AUTHENTICATED_USERS:
                    return CreateWellKnownSid(5, 11);
                case WellKnownSid.RESTRICTED_CODE:
                    return CreateWellKnownSid(5, 12);
                case WellKnownSid.TERMINAL_SERVER_USER:
                    return CreateWellKnownSid(5, 13);
                case WellKnownSid.REMOTE_INTERACTIVE_LOGON:
                    return CreateWellKnownSid(5, 14);
                case WellKnownSid.THIS_ORGANIZATION:
                    return CreateWellKnownSid(5, 15);
                case WellKnownSid.IUSR:
                    return CreateWellKnownSid(5, 17);
                case WellKnownSid.LOCAL_SYSTEM:
                    return CreateWellKnownSid(5, 18);
                case WellKnownSid.LOCAL_SERVICE:
                    return CreateWellKnownSid(5, 19);
                case WellKnownSid.NETWORK_SERVICE:
                    return CreateWellKnownSid(5, 20);
                case WellKnownSid.DOMAIN_USERS:
                    return CreateWellKnownSid(5, 21, domain[0], domain[1], domain[2], 513);
                case WellKnownSid.DOMAIN_GUESTS:
                    return CreateWellKnownSid(5, 21, domain[0], domain[1], domain[2], 514);
                case WellKnownSid.DOMAIN_COMPUTERS:
                    return CreateWellKnownSid(5, 21, domain[0], domain[1], domain[2], 515);
                case WellKnownSid.DOMAIN_DOMAIN_CONTROLLERS:
                    return CreateWellKnownSid(5, 21, domain[0], domain[1], domain[2], 516);
                case WellKnownSid.CERT_PUBLISHERS:
                    return CreateWellKnownSid(5, 21, domain[0], domain[1], domain[2], 517);
                case WellKnownSid.SCHEMA_ADMINISTRATORS:
                    return CreateWellKnownSid(5, 21, domain[0], domain[1], domain[2], 518);
                case WellKnownSid.ENTERPRISE_ADMINS:
                    return CreateWellKnownSid(5, 21, domain[0], domain[1], domain[2], 519);
                case WellKnownSid.RAS_SERVERS:
                    return CreateWellKnownSid(5, 21, domain[0], domain[1], domain[2], 553);
                case WellKnownSid.GROUP_POLICY_CREATOR_OWNERS:
                    return CreateWellKnownSid(5, 21, domain[0], domain[1], domain[2], 520);
                case WellKnownSid.BUILTIN_ADMINISTRATORS:
                    return CreateWellKnownSid(5, 32, 544);
                case WellKnownSid.BUILTIN_USERS:
                    return CreateWellKnownSid(5, 32, 545);
                case WellKnownSid.BUILTIN_GUESTS:
                    return CreateWellKnownSid(5, 32, 546);
                case WellKnownSid.POWER_USERS:
                    return CreateWellKnownSid(5, 32, 547);
                case WellKnownSid.ACCOUNT_OPERATORS:
                    return CreateWellKnownSid(5, 32, 548);
                case WellKnownSid.SERVER_OPERATORS:
                    return CreateWellKnownSid(5, 32, 549);
                case WellKnownSid.PRINTER_OPERATORS:
                    return CreateWellKnownSid(5, 32, 550);
                case WellKnownSid.BACKUP_OPERATORS:
                    return CreateWellKnownSid(5, 32, 551);
                case WellKnownSid.REPLICATOR:
                    return CreateWellKnownSid(5, 32, 552);
                case WellKnownSid.ALIAS_PREW2KCOMPACC:
                    return CreateWellKnownSid(5, 32, 554);
                case WellKnownSid.REMOTE_DESKTOP:
                    return CreateWellKnownSid(5, 32, 555);
                case WellKnownSid.NETWORK_CONFIGURATION_OPS:
                    return CreateWellKnownSid(5, 32, 556);
                case WellKnownSid.INCOMING_FOREST_TRUST_BUILDERS:
                    return CreateWellKnownSid(5, 32, 557);
                case WellKnownSid.PERFMON_USERS:
                    return CreateWellKnownSid(5, 32, 558);
                case WellKnownSid.PERFLOG_USERS:
                    return CreateWellKnownSid(5, 32, 559);
                case WellKnownSid.WINDOWS_AUTHORIZATION_ACCESS_GROUP:
                    return CreateWellKnownSid(5, 32, 560);
                case WellKnownSid.TERMINAL_SERVER_LICENSE_SERVERS:
                    return CreateWellKnownSid(5, 32, 561);
                case WellKnownSid.DISTRIBUTED_COM_USERS:
                    return CreateWellKnownSid(5, 32, 562);
                case WellKnownSid.IIS_IUSRS:
                    return CreateWellKnownSid(5, 32, 568);
                case WellKnownSid.CRYPTOGRAPHIC_OPERATORS:
                    return CreateWellKnownSid(5, 32, 569);
                case WellKnownSid.EVENT_LOG_READERS:
                    return CreateWellKnownSid(5, 32, 573);
                case WellKnownSid.CERTIFICATE_SERVICE_DCOM_ACCESS:
                    return CreateWellKnownSid(5, 32, 574);
                case WellKnownSid.WRITE_RESTRICTED:
                    return CreateWellKnownSid(5, 33);
                case WellKnownSid.NTLM_AUTHENTICATION:
                    return CreateWellKnownSid(5, 64, 10);
                case WellKnownSid.SCHANNEL_AUTHENTICATION:
                    return CreateWellKnownSid(5, 64, 14);
                case WellKnownSid.DIGEST_AUTHENTICATION:
                    return CreateWellKnownSid(5, 64, 21);
                case WellKnownSid.NT_SERVICE:
                    return CreateWellKnownSid(5, 80);
                case WellKnownSid.OTHER_ORGANIZATION:
                    return CreateWellKnownSid(5, 1000);
                case WellKnownSid.ML_UNTRUSTED:
                    return CreateWellKnownSid(16, 0);
                case WellKnownSid.ML_LOW:
                    return CreateWellKnownSid(16, 4096);
                case WellKnownSid.ML_MEDIUM:
                    return CreateWellKnownSid(16, 8192);
                case WellKnownSid.ML_MEDIUM_PLUS:
                    return CreateWellKnownSid(16, 8448);
                case WellKnownSid.ML_HIGH:
                    return CreateWellKnownSid(16, 12288);
                case WellKnownSid.ML_SYSTEM:
                    return CreateWellKnownSid(16, 16384);
                case WellKnownSid.ML_PROTECTED_PROCESS:
                    return CreateWellKnownSid(16, 20480);
                default:
                    throw new ArgumentException("Invalid Well-known SID type.", "type");
            }
        }

        /// <summary>
        /// Retrieve Well-Known RPC_SID defined in MS-DTYP section 2.4.2.3.
        /// </summary>
        /// <param name="type">The SID type to retrieve.</param>
        /// <param name="domain">
        /// The domain or root-domain represents the three sub-authority values if required in SID. 
        /// It can be null for most SID.
        /// </param>
        /// <returns>Well-Known RPC_SID.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when type is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown when domain is null for DOMAIN_USERS, DOMAIN_GUESTS, DOMAIN_COMPUTERS, 
        /// DOMAIN_DOMAIN_CONTROLLERS, CERT_PUBLISHERS, SCHEMA_ADMINISTRATORS, 
        /// ENTERPRISE_ADMINS, RAS_SERVERS, GROUP_POLICY_CREATOR_OWNERS and 
        /// BUILTIN_ADMINISTRATORS.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Throw when domain is invalid.
        /// </exception>
        public static _RPC_SID GetWellKnownRpcSid(WellKnownSid type, uint[] domain)
        {
            _SID sid = GetWellKnownSid(type, domain);
            return CreateRpcSid(sid.IdentifierAuthority, sid.SubAuthority);
        }

        /// <summary>
        /// Get Well Known SID Structures.
        /// </summary>
        /// <param name="identifierAuthorityType">
        /// The type of IdentifierAuthority
        /// </param>
        /// <param name="subAuthorities">
        /// The contents of SubAuthority.
        /// </param>
        /// <returns>Well-known SID</returns>
        ///<exception cref="ArgumentException">
        /// Throw when identifierAuthorityType is invalid.
        ///</exception>
        private static _SID CreateWellKnownSid(
            int identifierAuthorityType,
            params uint[] subAuthorities)
        {
            switch (identifierAuthorityType)
            {
                case 0:
                    return CreateSid(NULL_SID_AUTHORITY, subAuthorities);
                case 1:
                    return CreateSid(WORLD_SID_AUTHORITY, subAuthorities);
                case 2:
                    return CreateSid(LOCAL_SID_AUTHORITY, subAuthorities);
                case 3:
                    return CreateSid(CREATOR_SID_AUTHORITY, subAuthorities);
                case 4:
                    return CreateSid(NON_UNIQUE_AUTHORITY, subAuthorities);
                case 5:
                    return CreateSid(NT_AUTHORITY, subAuthorities);
                case 16:
                    return CreateSid(SECURITY_MANDATORY_LABEL_AUTHORITY, subAuthorities);
                default:
                    throw new ArgumentException("Invalid identifierAuthorityType.", "identifierAuthorityType");
            }
        }

        /// <summary>
        /// Create an instance of SID.
        /// </summary>
        /// <param name="identifierAuthority">
        /// Six element arrays of 8-bit unsigned integers that specify 
        /// the top-level authority of a RPC_SID.
        /// </param>
        /// <param name="subAuthorities">
        /// A variable length array of unsigned 32-bit integers that 
        /// uniquely identifies a principal relative to the IdentifierAuthority.
        /// </param>
        /// <returns>Created SID structure.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when identifierAuthority or subAuthorities is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when length of identifierAuthority is not 6 bytes.
        /// </exception>
        public static _SID CreateSid(
            byte[] identifierAuthority,
            params uint[] subAuthorities)
        {
            if (identifierAuthority == null)
            {
                throw new ArgumentNullException("identifierAuthority");
            }
            if (identifierAuthority.Length != SID_AUTHORITY_LENGTH)
            {
                throw new ArgumentException("Incorrect size of identifierAuthority.", "identifierAuthority");
            }
            if (subAuthorities == null)
            {
                throw new ArgumentNullException("subAuthorities");
            }

            const byte DEFAULT_REVISION = 1;

            _SID sid = new _SID();
            sid.Revision = DEFAULT_REVISION;
            sid.SubAuthorityCount = (byte)subAuthorities.Length;
            sid.IdentifierAuthority = identifierAuthority;
            sid.SubAuthority = subAuthorities;
            return sid;
        }

        /// <summary>
        /// Create an instance of RPC_SID.
        /// </summary>
        /// <param name="identifierAuthority">
        /// Six element arrays of 8-bit unsigned integers that specify 
        /// the top-level authority of a RPC_SID.
        /// </param>
        /// <param name="subAuthorities">
        /// A variable length array of unsigned 32-bit integers that 
        /// uniquely identifies a principal relative to the IdentifierAuthority.
        /// </param>
        /// <returns>Created RPC_SID structure.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when identifierAuthority or subAuthorities is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when length of identifierAuthority is not 6 bytes.
        /// </exception>
        public static _RPC_SID CreateRpcSid(
            byte[] identifierAuthority,
            params uint[] subAuthorities)
        {
            if (identifierAuthority == null)
            {
                throw new ArgumentNullException("identifierAuthority");
            }
            if (identifierAuthority.Length != SID_AUTHORITY_LENGTH)
            {
                throw new ArgumentException("Incorrect size of identifierAuthority.", "identifierAuthority");
            }
            if (subAuthorities == null)
            {
                throw new ArgumentNullException("subAuthorities");
            }

            const byte DEFAULT_REVISION = 1;

            _RPC_SID rpcSid = new _RPC_SID();
            rpcSid.Revision = DEFAULT_REVISION;
            rpcSid.SubAuthorityCount = (byte)subAuthorities.Length;
            rpcSid.IdentifierAuthority.Value = identifierAuthority;
            rpcSid.SubAuthority = subAuthorities;
            return rpcSid;
        }

        /// <summary>
        /// Create an instance of PRC_UNICODE_STRING.
        /// </summary>
        /// <param name="s">
        /// A string. 
        /// If it's null, Length and maximumLength is 0, Buffer is NULL.
        /// </param>
        /// <returns>Created RPC_UNICODE_STRING structure.</returns>
        public static _RPC_UNICODE_STRING ToRpcUnicodeString(string s)
        {
            _RPC_UNICODE_STRING rpcUnicodeString = new _RPC_UNICODE_STRING();

            if (s == null)
            {
                rpcUnicodeString.Length = 0;
                rpcUnicodeString.MaximumLength = 0;
                rpcUnicodeString.Buffer = null;
            }
            else
            {
                byte[] buf = Encoding.Unicode.GetBytes(s);
                rpcUnicodeString.Length = (ushort)buf.Length;
                rpcUnicodeString.MaximumLength = (ushort)buf.Length;
                rpcUnicodeString.Buffer = new ushort[buf.Length / sizeof(ushort)];
                Buffer.BlockCopy(buf, 0, rpcUnicodeString.Buffer, 0, buf.Length);
            }

            return rpcUnicodeString;
        }

        /// <summary>
        /// Read string from PRC_UNICODE_STRING, null terminating char is not included.
        /// </summary>
        /// <param name="s">
        /// A PRC_UNICODE_STRING structure. 
        /// Return value is null if Length and MaximumLength is 0 and Buffer is NULL.</param>
        /// <returns>The string in the structure.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when s is invalid.
        /// </exception>
        public static string ToString(_RPC_UNICODE_STRING s)
        {
            if (s.Length == 0 && s.MaximumLength == 0 && s.Buffer == null)
            {
                return null;
            }

            if (s.Buffer.Length * sizeof(ushort) < s.Length)
            {
                throw new ArgumentException("RPC_UNICODE_STRING is invalid.", "s");
            }

            byte[] buf = new byte[s.Length];
            Buffer.BlockCopy(s.Buffer, 0, buf, 0, buf.Length);
            return Encoding.Unicode.GetString(buf);
        }

        /// <summary>
        /// Create an instance of OLD_LARGE_INTEGER.
        /// </summary>
        /// <param name="value">A int64 value.</param>
        /// <returns>Created OLD_LARGE_INTEGER structure.</returns>
        public static _OLD_LARGE_INTEGER ToOldLargeInteger(long value)
        {
            _OLD_LARGE_INTEGER integer = new _OLD_LARGE_INTEGER();
            byte[] buf = BitConverter.GetBytes(value);
            integer.LowPart = BitConverter.ToUInt32(buf, 0);
            integer.HighPart = BitConverter.ToInt32(buf, 4);
            return integer;
        }

        /// <summary>
        /// Read int64 value from _OLD_LARGE_INTEGER.
        /// </summary>
        /// <param name="value">A _OLD_LARGE_INTEGER structure.</param>
        /// <returns>The value in the structure.</returns>
        public static long ToInt64(_OLD_LARGE_INTEGER value)
        {
            return ((long)value.HighPart << 32) + (long)value.LowPart;
        }

        /// <summary>
        /// Converts a string to an _RPC_STRING
        /// </summary>
        /// <param name="input">
        /// A string to be converted.
        /// If it's null, Length and maximumLength is 0, Buffer is NULL.
        /// </param>
        /// <returns>Converted _RPC_STRING</returns>    
        public static _RPC_STRING ToRpcString(string input)
        {
            _RPC_STRING rpcString = new _RPC_STRING();

            if (input == null)
            {
                rpcString.Length = 0;
                rpcString.MaximumLength = 0;
                rpcString.Buffer = null;
            }
            else
            {
                rpcString.Buffer = Encoding.ASCII.GetBytes(input);
                rpcString.Length = (ushort)rpcString.Buffer.Length;
                rpcString.MaximumLength = (ushort)(rpcString.Length + 1);
            }

            return rpcString;
        }

        /// <summary>
        /// Convert SID to SDDL string.
        /// </summary>
        /// <param name="sid">SID to convert.</param>
        /// <returns>SDDL string.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the Revision field of sid is invalid. 
        /// Thrown when the length of identifierAuthority is not 6 bytes.
        /// Thrown when the IdentifierAuthority field of sid is null.
        /// </exception>
        public static string ToSddlString(_SID sid)
        {
            //
            //SID String Format Syntax
            //
            //SID= "S-1-" IdentifierAuthority 1*SubAuthority
            //IdentifierAuthority= IdentifierAuthorityDec / IdentifierAuthorityHex
            //; If the identifier authority is < 2^32, the
            //; identifier authority is represented as a decimal
            //; number
            //; If the identifier authority is >= 2^32,
            //; the identifier authority is represented in
            //; hexadecimal
            //IdentifierAuthorityDec = 1*10DIGIT
            //; IdentifierAuthorityDec, top level authority of a
            //; security identifier is represented as a decimal number
            //IdentifierAuthorityHex = "0x" 12HEXDIG
            //; IdentifierAuthorityHex, the top-level authority of a
            //; security identifier is represented as a hexadecimal number
            //SubAuthority= "-" 1*10DIGIT
            //; Sub-Authority is always represented as a decimal number
            //; No leading "0" characters are allowed when IdentifierAuthority
            //; or SubAuthority is represented as a decimal number
            //; All hexadecimal digits must be output in string format,
            //; pre-pended by "0x"

            if (sid.Revision != 1)
            {
                throw new ArgumentException("The Revision field of sid is invalid.", "sid");
            }

            //S-1: Indicates a revision or version 1 SID.
            string sddlString = "S-1-";

            if (sid.IdentifierAuthority == null)
            {
                throw new ArgumentException("The IdentifierAuthority field of sid is null.", "sid");
            }
            if (sid.IdentifierAuthority.Length != SID_AUTHORITY_LENGTH)
            {
                throw new ArgumentException("Incorrect size of identifierAuthority field.", "sid");
            }

            ulong identifierAuthority = 0;
            for (int i = 0; i < sid.IdentifierAuthority.Length; i++)
            {
                identifierAuthority = (identifierAuthority << 8) + sid.IdentifierAuthority[i];
            }
            if ((sid.IdentifierAuthority[0] == 0) && (sid.IdentifierAuthority[1] == 0))
            {
                sddlString += string.Format(CultureInfo.InvariantCulture, "{0:D}", identifierAuthority);
            }
            else
            {
                sddlString += string.Format(CultureInfo.InvariantCulture, "0x{0:X12}", identifierAuthority);
            }

            for (int i = 0; i < sid.SubAuthorityCount; i++)
            {
                sddlString += string.Format(CultureInfo.InvariantCulture, "-{0:D}", sid.SubAuthority[i]);
            }

            return sddlString;
        }

        /// <summary>
        /// Convert RPC_SID to SDDL string.
        /// </summary>
        /// <param name="rpcSid">RPC_SID to convert.</param>
        /// <returns>SDDL string.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the Revision field of sid is invalid. 
        /// Thrown when length of identifierAuthority is not 6 bytes.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown when identifierAuthority is null.
        /// </exception>
        public static string ToSddlString(_RPC_SID rpcSid)
        {
            _SID sid = CreateSid(rpcSid.IdentifierAuthority.Value, rpcSid.SubAuthority);

            return ToSddlString(sid);
        }

        /// <summary>
        /// Convert _SECURITY_DESCRIPTOR to SDDL string.
        /// </summary>
        /// <param name="securityDescriptor">_SECURITY_DESCRIPTOR to convert.</param>
        /// <returns>SDDL string.</returns>
        public static string ToSddlString(_SECURITY_DESCRIPTOR securityDescriptor)
        {
            byte[] securityDescriptorBytes = DtypUtility.EncodeSecurityDescriptor(securityDescriptor);
            RawSecurityDescriptor rawSecurityDescriptor = new RawSecurityDescriptor(securityDescriptorBytes, 0);

            return rawSecurityDescriptor.GetSddlForm(AccessControlSections.All);
        }

        /// <summary>
        /// Covert SDDL string to SID.
        /// </summary>
        /// <param name="sddlString">SDDL string to convert.</param>
        /// <returns>Converted SID.</returns>
        ///<exception cref="ArgumentException">Throw when the given sddl string is invalid</exception>
        public static _SID ToSid(string sddlString)
        {
            byte[] identifierAuthority = new byte[6];
            List<uint> subAuthorities = new List<uint>();

            Regex regex = new Regex(@"S-1-((0x(?<identifierAuthorityHex>[0-9a-fA-F]{12}))|(?<identifierAuthorityDec>\d{1,10}))(?<subAuthority>(-\d{1,10})*)",
                RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

            Match match = regex.Match(sddlString);
            if (!match.Success)
            {
                throw new ArgumentException("The given sddl string is invalid.", "sddlString");
            }
            if (match.Groups["identifierAuthorityDec"].Success)
            {
                uint identifierAuthorityDec = UInt32.Parse(match.Groups["identifierAuthorityDec"].Value, CultureInfo.InvariantCulture);
                for (int i = identifierAuthority.Length - 1; i >= 0; i--)
                {
                    identifierAuthority[i] = (byte)(identifierAuthorityDec & 0xFF);
                    identifierAuthorityDec = identifierAuthorityDec >> 8;
                }
            }
            if (match.Groups["identifierAuthorityHex"].Success)
            {
                for (int i = 0; i < identifierAuthority.Length; i++)
                {
                    identifierAuthority[i] = Convert.ToByte(match.Groups["identifierAuthorityHex"].Value.Substring(i * 2, 2), 16);
                }
            }
            if (match.Groups["subAuthority"].Success)
            {
                string[] result = match.Groups["subAuthority"].Value.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < result.Length; i++)
                {
                    subAuthorities.Add(UInt32.Parse(result[i], CultureInfo.InvariantCulture));
                }
            }

            return DtypUtility.CreateSid(identifierAuthority, subAuthorities.ToArray());
        }

        /// <summary>
        /// Covert SDDL string to _SECURITY_DESCRIPTOR in self-relative format.
        /// </summary>
        /// <param name="sddlString">SDDL string to convert.</param>
        /// <returns>_SECURITY_DESCRIPTOR</returns>
        public static _SECURITY_DESCRIPTOR ToSecurityDescriptor(string sddlString)
        {
            RawSecurityDescriptor rawSecurityDescriptor = new RawSecurityDescriptor(sddlString);
            byte[] rawSecurityDescriptorBytes = new byte[rawSecurityDescriptor.BinaryLength];
            rawSecurityDescriptor.GetBinaryForm(rawSecurityDescriptorBytes, 0);

            return DtypUtility.DecodeSecurityDescriptor(rawSecurityDescriptorBytes);
        }

        /// <summary>
        /// This algorithm returns the size, in bytes, of SID. This is equal to the number of bytes occupied by the Revision, 
        /// SubAuthorityCount, and IdentifierAuthorityCount fields of a SID. Added to this is the size of a SubAuthority field 
        /// of a SID times SID.SubAuthorityCount.
        /// </summary>
        /// <param name="sid">The SID to be calculated.</param>
        /// <returns>The length of the specified SID.</returns>
        public static int SidLength(_SID sid)
        {
            return (8 + (4 * sid.SubAuthorityCount));
        }

        /// <summary>
        /// A support function, SidInToken, takes the authorization context, 
        /// a SID (referenced below as the SidToTest parameter), and an optional 
        /// PrincipalSelfSubstitute parameter, and returns TRUE if the SidToTest 
        /// is present in the authorization context; otherwise, it returns FALSE. 
        /// The well-known SID PRINCIPAL_SELF, if passed as SidToTest, is replaced 
        /// by the PrincipalSelfSubstitute SID prior to the examination of the 
        /// authorization context. MS-DTYP section 2.5.4.1
        /// </summary>
        /// <param name="token">
        /// Token is an authorization context containing all SIDs 
        /// that represent the security principal
        /// </param>
        /// <param name="sidToTest">
        /// The SID for which to search in Token
        /// </param>
        /// <param name="principalSelfSubstitute">
        /// a SID with which SidToTest may be replaced
        /// </param>
        /// <returns>
        /// Returns TRUE if the SidToTest is present in the authorization context; 
        /// otherwise, it returns FALSE. 
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the well-known SID PRINCIPAL_SELF, if passed as SidToTest, but 
        /// principalSelfSubstitute does't exist.
        /// </exception>
        public static bool SidInToken(
            Token token,
            _SID sidToTest,
            _SID? principalSelfSubstitute)
        {
            //
            //Pseudocode syntax
            //
            //--
            //-- On entry
            //-- Token is an authorization context containing all SIDs
            //-- that represent the security principal
            //-- SidToTest, the SID for which to search in Token
            //-- PrincipalSelfSubstitute, a SID with which SidToTest may be
            //-- replaced
            //IF SidToTest is the Well Known SID PRINCIPAL_SELF THEN
            //set SidToTest to be PrincipalSelfSubstitute
            //END IF
            //FOR EACH SID s in Token DO
            //IF s equals SidToTest THEN
            //return TRUE
            //END IF
            //END FOR
            //Return FALSE
            //END-SUBROUTINE

            if (ObjectUtility.DeepCompare(GetWellKnownSid(WellKnownSid.PRINCIPAL_SELF, null), sidToTest))
            {
                if (principalSelfSubstitute != null)
                {
                    sidToTest = (_SID)principalSelfSubstitute;
                }
                else
                {
                    throw new ArgumentException("The principalSelfSubstitute doesn't exist", "principalSelfSubstitute");
                }
            }
            if (token.Sids != null)
            {
                foreach (_SID sid in token.Sids)
                {
                    if (ObjectUtility.DeepCompare(sid, sidToTest))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// A support function, SidDominates, compares the mandatory integrity levels expressed in two SIDs.
        /// This function can be used only on SIDs that encode integrity levels (the SID_IDENTIFIER_AUTHORITY
        /// field is SECURITY_MANDATORY_LABEL_AUTHORITY); any other use is unsupported.
        /// </summary>
        /// <param name="sid1">first sid in dominance calculation</param>
        /// <param name="sid2">second sid in dominance calculation</param>
        /// <returns>
        /// The function returns TRUE if the first SID dominates the second SID or is equal to the second SID, 
        /// or FALSE if the first SID is subordinate to the second SID.
        /// </returns>
        public static bool SidDominates(_SID sid1, _SID sid2)
        {
            //Described in MS-DTYP Section 2.5.3.1.2 SidDominates:
            //
            //IF sid1 equals sid2 THEN
            //    Return TRUE
            //END IF
            //-- If Sid2 has more SubAuthorities than Sid1, Sid1 cannot dominate.
            //IF sid2.SubAuthorityCount GREATER THAN sid1.SubAuthorityCount THEN
            //    Return FALSE
            //END IF
            //--on entry, index is zero and is incremented for each iteration of the loop.
            //FOR each SubAuthority in sid1
            //    IF sid1.SubAuthority[ index ] GREATER THAN or EQUAL TO sid2.SubAuthority[ index ] THEN
            //        Return TRUE
            //    END IF
            //END FOR
            //Return FALSE

            if (ObjectUtility.DeepCompare(sid1, sid2) == true)
            {
                return true;
            }

            if (sid2.SubAuthorityCount > sid1.SubAuthorityCount)
            {
                return false;
            }

            for (int i = 0; i < sid1.SubAuthorityCount; i++)
            {
                if (i >= sid2.SubAuthorityCount)
                {
                    break;
                }
                if (sid1.SubAuthority[i] >= sid2.SubAuthority[i])
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Create an instance of _LUID.
        /// </summary>
        /// <param name="lowPart">Low-order 32 bits.</param>
        /// <param name="highPart">High-order 32 bits.</param>
        /// <returns>Created LUID structure.</returns>
        public static _LUID CreateLuid(uint lowPart, int highPart)
        {
            _LUID luid = new _LUID();
            luid.LowPart = lowPart;
            luid.HighPart = highPart;
            return luid;
        }

        /// <summary>
        /// Retrieve privilege LUID according to the given privilege name defined in MS-LSAD section 3.1.1.2.1.
        /// </summary>
        /// <param name="privilegeName"> The privilege name to retrieve. </param>
        /// <returns>Privilege LUID</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when privilegeName is invalid.
        /// </exception>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public static _LUID GetPrivilegeLuid(PrivilegeName privilegeName)
        {
            switch (privilegeName)
            {
                case PrivilegeName.SE_ASSIGNPRIMARYTOKEN_NAME:
                    return CreateLuid(0, 3);

                case PrivilegeName.SE_AUDIT_NAME:
                    return CreateLuid(0, 21);

                case PrivilegeName.SE_BACKUP_NAME:
                    return CreateLuid(0, 17);

                case PrivilegeName.SE_CHANGE_NOTIFY_NAME:
                    return CreateLuid(0, 23);

                case PrivilegeName.SE_CREATE_GLOBAL_NAME:
                    return CreateLuid(0, 30);

                case PrivilegeName.SE_CREATE_PAGEFILE_NAME:
                    return CreateLuid(0, 15);

                case PrivilegeName.SE_CREATE_PERMANENT_NAME:
                    return CreateLuid(0, 16);

                case PrivilegeName.SE_CREATE_TOKEN_NAME:
                    return CreateLuid(0, 2);

                case PrivilegeName.SE_DEBUG_NAME:
                    return CreateLuid(0, 20);

                case PrivilegeName.SE_ENABLE_DELEGATION_NAME:
                    return CreateLuid(0, 27);

                case PrivilegeName.SE_IMPERSONATE_NAME:
                    return CreateLuid(0, 29);

                case PrivilegeName.SE_INC_BASE_PRIORITY_NAME:
                    return CreateLuid(0, 14);

                case PrivilegeName.SE_INCREASE_QUOTA_NAME:
                    return CreateLuid(0, 5);

                case PrivilegeName.SE_LOAD_DRIVER_NAME:
                    return CreateLuid(0, 10);

                case PrivilegeName.SE_LOCK_MEMORY_NAME:
                    return CreateLuid(0, 4);

                case PrivilegeName.SE_MACHINE_ACCOUNT_NAME:
                    return CreateLuid(0, 6);

                case PrivilegeName.SE_MANAGE_VOLUME_NAME:
                    return CreateLuid(0, 28);

                case PrivilegeName.SE_PROF_SINGLE_PROCESS_NAME:
                    return CreateLuid(0, 13);

                case PrivilegeName.SE_REMOTE_SHUTDOWN_NAME:
                    return CreateLuid(0, 24);

                case PrivilegeName.SE_RESTORE_NAME:
                    return CreateLuid(0, 18);

                case PrivilegeName.SE_SECURITY_NAME:
                    return CreateLuid(0, 8);

                case PrivilegeName.SE_SHUTDOWN_NAME:
                    return CreateLuid(0, 19);

                case PrivilegeName.SE_SYNC_AGENT_NAME:
                    return CreateLuid(0, 26);

                case PrivilegeName.SE_SYSTEM_ENVIRONMENT_NAME:
                    return CreateLuid(0, 22);

                case PrivilegeName.SE_SYSTEM_PROFILE_NAME:
                    return CreateLuid(0, 11);

                case PrivilegeName.SE_SYSTEMTIME_NAME:
                    return CreateLuid(0, 12);

                case PrivilegeName.SE_TAKE_OWNERSHIP_NAME:
                    return CreateLuid(0, 9);

                case PrivilegeName.SE_TCB_NAME:
                    return CreateLuid(0, 7);

                case PrivilegeName.SE_UNDOCK_NAME:
                    return CreateLuid(0, 25);

                case PrivilegeName.SE_CREATE_SYMBOLIC_LINK_NAME:
                    return CreateLuid(0, 35);

                case PrivilegeName.SE_INC_WORKING_SET_NAME:
                    return CreateLuid(0, 33);

                case PrivilegeName.SE_RELABEL_NAME:
                    return CreateLuid(0, 32);

                case PrivilegeName.SE_TIME_ZONE_NAME:
                    return CreateLuid(0, 34);

                case PrivilegeName.SE_TRUSTED_CREDMAN_ACCESS_NAME:
                    return CreateLuid(0, 31);

                default:
                    throw new ArgumentException("Invalid privilege name.", "privilegeName");
            }
        }

        /// <summary>
        /// The sub function of the AccessCheck algorithm
        /// </summary>
        /// <param name="objectType">The current ace objectType to be dealt with.</param>
        /// <param name="localTree">
        /// The current tree representation of the hierarchy of objects for which to check access.
        /// </param>
        private static void AncestorNodesAccessCheck(
            Guid objectType,
            ref AccessCheckObjectTree localTree)
        {
            Guid[] ancestor = localTree.GetAncestorNodes(objectType);
            if (ancestor.Length > 0)
            {
                Guid currentNode = objectType;
                for (int i = 0; i < ancestor.Length; i++)
                {
                    Guid[] sibling = localTree.GetSiblingNodes(currentNode);
                    if (sibling.Length > 0)
                    {
                        uint currentRemaining = 0;
                        foreach (Guid node in sibling)
                        {
                            currentRemaining |= localTree.GetTreeNodeData(node);
                        }
                        if (currentRemaining != localTree.GetTreeNodeData(currentNode))
                        {
                            break;
                        }
                    }
                    localTree.SetTreeNodeData(ancestor[i], localTree.GetTreeNodeData(currentNode));
                    currentNode = ancestor[i];
                }
            }
        }

        /// <summary>
        /// The sub function of the AccessCheck algorithm
        /// </summary>
        /// <param name="ace">The current ace to be dealt with.</param>
        /// <param name="remainingAccess">The current requested permissions. </param>
        /// <param name="localTree">
        /// The current tree representation of the hierarchy of objects for which to check access.
        /// </param>
        /// <param name="allNodes">All the nodes in the current tree.</param>
        /// <returns>Return FALSE if access is denied. Otherwise, it returns TRUE.</returns>
        private static bool AceAccessCheck(
            object ace,
            ref uint remainingAccess,
            ref AccessCheckObjectTree localTree,
            Guid[] allNodes)
        {
            if (localTree != null && localTree.Root != null)
            {
                List<Guid> tmpList = new List<Guid>();
                tmpList.Add(localTree.Root.Value);
                tmpList.AddRange(localTree.GetDescendentNodes(localTree.Root.Value));
                allNodes = tmpList.ToArray();
            }

            _ACE_HEADER header = (_ACE_HEADER)ObjectUtility.GetFieldValue(ace, "Header");
            uint mask = (uint)ObjectUtility.GetFieldValue(ace, "Mask");
            ACCESS_OBJECT_ACE_Flags flags = ACCESS_OBJECT_ACE_Flags.None;
            ACE_TYPE aceType = header.AceType;

            if (header.AceType == ACE_TYPE.ACCESS_ALLOWED_OBJECT_ACE_TYPE
                || header.AceType == ACE_TYPE.ACCESS_DENIED_OBJECT_ACE_TYPE)
            {
                flags = (ACCESS_OBJECT_ACE_Flags)ObjectUtility.GetFieldValue(ace, "Flags");
                if ((flags & ACCESS_OBJECT_ACE_Flags.ACE_OBJECT_TYPE_PRESENT)
                                 != ACCESS_OBJECT_ACE_Flags.ACE_OBJECT_TYPE_PRESENT)
                {
                    aceType = (header.AceType == ACE_TYPE.ACCESS_ALLOWED_OBJECT_ACE_TYPE)
                        ? ACE_TYPE.ACCESS_ALLOWED_ACE_TYPE
                        : ACE_TYPE.ACCESS_DENIED_ACE_TYPE;
                }
            }

            switch (aceType)
            {
                //CASE ACE.Type OF
                //CASE Allow Access:
                //CALL SidInToken( Token, ACE.Sid, and PrincipalSelfSubst )
                //IF SidInToken returns True THEN
                //    Remove ACE.AccessMask from RemainingAccess
                //    FOR each node in LocalTree DO
                //    Remove ACE.AccessMask from node.Remaining
                //    END FOR
                //END IF
                //CASE Deny Access:
                //CALL SidInToken( Token, ACE.Sid, PrincipalSelfSubst )
                //IF SidInToken returns True THEN
                //    IF any bit of RemainingAccess is in ACE.AccessMask THEN
                //    Return access_denied
                //END IF
                //END IF

                case ACE_TYPE.ACCESS_ALLOWED_ACE_TYPE:
                    remainingAccess &= ~mask;
                    foreach (Guid node in allNodes)
                    {
                        uint newRemaining = localTree.GetTreeNodeData(node) & ~mask;
                        localTree.SetTreeNodeData(node, newRemaining);
                    }
                    break;

                case ACE_TYPE.ACCESS_DENIED_ACE_TYPE:
                    if ((remainingAccess & mask) != 0)
                    {
                        return false;
                    }
                    break;

                //CASE Object Allow Access:
                //CALL SidInToken( Token, ACE.Sid, PrincipalSelfSubst )
                //IF SidInToken returns True THEN
                //    IF ACE.Object is contained in LocalTree THEN
                //    Locate node n in LocalTree such that
                //    n.GUID is the same as ACE.Object
                //    Remove ACE.AccessMask from n.Remaining
                //    FOR each node ns such that ns is a descendent of n DO
                //    Remove ACE.AccessMask from ns.Remaining
                //    END FOR
                //    FOR each node np such that np is an ancestor of n DO
                //    Set np.Remaining = np.Remaining or np-1.Remaining   //Filed TDI 55801
                //END FOR
                //END IF
                //END IF

                case ACE_TYPE.ACCESS_ALLOWED_OBJECT_ACE_TYPE:
                    Guid objectType = (Guid)ObjectUtility.GetFieldValue(ace, "ObjectType");
                    if (localTree.ContainsTreeNode(objectType))
                    {
                        uint newRemaining = localTree.GetTreeNodeData(objectType) & ~mask;
                        localTree.SetTreeNodeData(objectType, newRemaining);

                        Guid[] descendent = localTree.GetDescendentNodes(objectType);
                        if (descendent.Length > 0)
                        {
                            foreach (Guid nodeGuid in descendent)
                            {
                                newRemaining = localTree.GetTreeNodeData(nodeGuid) & ~mask;
                                localTree.SetTreeNodeData(nodeGuid, newRemaining);
                            }
                        }

                        DtypUtility.AncestorNodesAccessCheck(objectType, ref localTree);
                    }
                    break;

                //CASE Object Deny Access:
                //CALL SidInToken( Token, ACE.Sid, PrincipalSelfSubst )
                //IF SidInToken returns True THEN
                //    Locate node n in LocalTree such that
                //    n.GUID is the same as ACE.Object
                //    IF n exists THEN
                //    If any bit of n.Remaining is in ACE.AccessMask THEN
                //    Return access_denied
                //END IF
                //END IF
                //END IF
                //END CASE

                case ACE_TYPE.ACCESS_DENIED_OBJECT_ACE_TYPE:
                    Guid objectType2 = (Guid)ObjectUtility.GetFieldValue(ace, "ObjectType");
                    foreach (Guid node in allNodes)
                    {
                        if (node == objectType2 && (localTree.GetTreeNodeData(node) & mask) != 0)
                        {
                            return false;
                        }
                    }
                    break;
            }
            return true;
        }

        /// <summary>
        /// Access Check Algorithm. MS-DTYP section 2.5.4.1
        /// </summary>
        /// <param name="securityDescriptor">
        /// SECURITY_DESCRIPTOR structure that is assigned to the object.
        /// </param>
        /// <param name="token">
        /// Token is an authorization context containing all SIDs 
        /// that represent the security principal
        /// </param>
        /// <param name="accessRequestMask">
        /// Set of permissions requested on the object.
        /// </param>
        /// <param name="objectTree">
        /// A tree representation of the hierarchy of objects for which 
        /// to check access. Each node represents an object with two values. 
        /// A GUID that represents the object itself and a value called 
        /// Remaining, which indicates the user rights request for that 
        /// node that have not yet been satisfied. It can be null.
        /// </param>
        /// <param name="principalSelfSubstitudeSid">
        /// A SID that logically replaces the SID in any ACE that contains 
        /// the well-known PRINCIPAL_SELF SID. It can be null.
        /// </param>
        /// <returns>
        /// Returns TRUE if access is allowed. Otherwise, it returns FALSE if access is denied. 
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the Dacl field of securityDescriptor doesn't exist, but the DACLPresent flag is set.
        /// Thrown when the Revision field of securityDescriptor is invalid
        /// </exception>
        public static bool AccessCheck(
            _SECURITY_DESCRIPTOR securityDescriptor,
            Token token,
            uint accessRequestMask,
            AccessCheckObjectTree objectTree,
            _SID? principalSelfSubstitudeSid)
        {

            //MS-ADTS Section 5.1.3.3.1 Null vs. Empty DACLs
            //The presence of a NULL DACL in the nTSecurityDescriptor attribute of an object grants full
            //access to the object to any principal that requests it; normal access checks are not performed 
            //with respect to the object.

            if (securityDescriptor.Revision != 0x1)
            {
                throw new ArgumentException("The Revision field is invalid.", "securityDescriptor");
            }

            if ((securityDescriptor.Control & SECURITY_DESCRIPTOR_Control.DACLPresent)
                == SECURITY_DESCRIPTOR_Control.None)
            {
                return true;
            }
            else if (securityDescriptor.Dacl == null)
            {
                throw new ArgumentException(
                    "The Dacl field of securityDescriptor doesn't exist, but the DACLPresent flag is set.", "securityDescriptor");
            }

            //Pscudocode
            //
            //Set DACL to SecurityDescriptor Dacl field
            //Set RemainingAccess to Access Request mask
            //
            //IF RemainingAccess contains ACCESS_SYSTEM_SECURITY access flag THEN
            //IF Token.Privileges contains SeSecurityPrivilege THEN
            //Remove ACCESS_SYSTEM_SECURITY access bit from RemainingAccess
            //END IF
            //END IF
            //
            //IF RemainingAccess contains WRITE_OWNER access bit THEN
            //IF Token.Privileges contains SeTakeOwnershipPrivilege THEN
            //Remove WRITE_OWNER access bit from RemainingAccess
            //END IF
            //END IF
            //-- the owner of an object is always granted READ_CONTROL and WRITE_DAC.
            //CALL SidInToken( Token, SecurityDescriptor.Owner, PrincipalSelfSubst)
            //IF SidInToken returns True THEN
            //Remove READ_CONTROL and WRITE_DAC from RemainingAccess
            //END IF

            _ACL? dacl = securityDescriptor.Dacl;
            uint remainingAccess = accessRequestMask;
            AccessCheckObjectTree localTree = new AccessCheckObjectTree();
            Guid[] allNodes = new Guid[] { };

            if ((remainingAccess & ACCESS_MASK_ACCESS_SYSTEM_SECURITY) != 0 && token.Privileges != null)
            {
                foreach (_LUID privilege in token.Privileges)
                {
                    if (ObjectUtility.DeepCompare(privilege, GetPrivilegeLuid(PrivilegeName.SE_SECURITY_NAME)))
                    {
                        remainingAccess &= ~ACCESS_MASK_ACCESS_SYSTEM_SECURITY;
                    }
                }
            }
            if ((remainingAccess & ACCESS_MASK_WRITE_OWNER) != 0 && token.Privileges != null)
            {
                foreach (_LUID privilege in token.Privileges)
                {
                    if (ObjectUtility.DeepCompare(privilege, GetPrivilegeLuid(PrivilegeName.SE_TAKE_OWNERSHIP_NAME)))
                    {
                        remainingAccess &= ~ACCESS_MASK_WRITE_OWNER;
                    }
                }
            }
            if (securityDescriptor.OwnerSid != null
                && SidInToken(token, securityDescriptor.OwnerSid.Value, principalSelfSubstitudeSid))
            {
                remainingAccess &= ~(uint)(ACCESS_MASK_READ_CONTROL | ACCESS_MASK_WRITE_DAC);
            }

            //IF Object Tree is not NULL THEN
            //Set LocalTree to Object Tree
            //FOR each node in LocalTree DO
            //Set node.Remaining to RemainingAccess
            //END FOR
            //END IF

            if (objectTree != null && objectTree.Root != null)
            {
                localTree = (AccessCheckObjectTree)ObjectUtility.DeepClone(objectTree);
                List<Guid> tmpList = new List<Guid>();
                tmpList.Add(localTree.Root.Value);
                tmpList.AddRange(localTree.GetDescendentNodes(localTree.Root.Value));
                allNodes = tmpList.ToArray();
                foreach (Guid node in allNodes)
                {
                    localTree.SetTreeNodeData(node, remainingAccess);
                }
            }

            //FOR each ACE in DACL DO
            //IF ACE.flag does not contain INHERIT_ONLY_ACE THEN
            //CASE ACE.Type OF
            //CASE Allow Access:
            //CALL SidInToken( Token, ACE.Sid, and PrincipalSelfSubst )
            //IF SidInToken returns True THEN
            //    Remove ACE.AccessMask from RemainingAccess
            //    FOR each node in LocalTree DO
            //    Remove ACE.AccessMask from node.Remaining
            //END FOR
            //END IF
            //CASE Deny Access:
            //CALL SidInToken( Token, ACE.Sid, PrincipalSelfSubst )
            //IF SidInToken returns True THEN
            //    IF any bit of RemainingAccess is in ACE.AccessMask THEN
            //    Return access_denied
            //END IF
            //END IF
            //CASE Object Allow Access:
            //CALL SidInToken( Token, ACE.Sid, PrincipalSelfSubst )
            //IF SidInToken returns True THEN
            //    IF ACE.Object is contained in LocalTree THEN
            //    Locate node n in LocalTree such that
            //    n.GUID is the same as ACE.Object
            //    Remove ACE.AccessMask from n.Remaining
            //    FOR each node ns such that ns is a descendent of n DO
            //    Remove ACE.AccessMask from ns.Remaining
            //    END FOR
            //    FOR each node np such that np is an ancestor of n DO
            //    Set np.Remaining = np.Remaining or np-1.Remaining
            //END FOR
            //END IF
            //END IF
            //CASE Object Deny Access:
            //CALL SidInToken( Token, ACE.Sid, PrincipalSelfSubst )
            //IF SidInToken returns True THEN
            //    Locate node n in LocalTree such that
            //    n.GUID is the same as ACE.Object
            //    IF n exists THEN
            //    If any bit of n.Remaining is in ACE.AccessMask THEN
            //    Return access_denied
            //END IF
            //END IF
            //END IF
            //END CASE
            //END IF
            //END FOR

            if (dacl.Value.Aces != null)
            {
                foreach (object ace in dacl.Value.Aces)
                {
                    _ACE_HEADER header = (_ACE_HEADER)ObjectUtility.GetFieldValue(ace, "Header");
                    _SID sid = (_SID)ObjectUtility.GetFieldValue(ace, "Sid");

                    if ((header.AceFlags & ACE_FLAGS.INHERIT_ONLY_ACE) == ACE_FLAGS.INHERIT_ONLY_ACE
                        || !SidInToken(token, sid, principalSelfSubstitudeSid))
                    {
                        continue;
                    }

                    bool ret = DtypUtility.AceAccessCheck(ace, ref remainingAccess, ref localTree, allNodes);
                    if (ret == false)
                    {
                        return false;
                    }
                }
            }

            //IF RemainingAccess = 0 THEN
            //Return success
            //Else
            //Return access_denied
            //END IF

            bool status;
            if (objectTree != null && objectTree.Root != null)
            {
                status = (localTree.GetTreeNodeData(localTree.Root.Value) == 0);
            }
            else
            {
                status = (remainingAccess == 0);
            }
            return status;
        }


        /// <summary>
        /// MandatoryIntegrityCheck. MS-DTYP section 2.5.4.1
        /// </summary>
        /// <param name="tokenIntegritySID">
        /// Mandatory Integrity SID in the Token
        /// </param>
        /// <param name="aceIntegritySID">
        /// Mandatory Integrity label SID in the Security descriptor of the object
        /// </param>
        /// <param name="mandatoryInformation">
        /// Output of the function describing the allowable bits for the caller
        /// </param>
        /// <param name="token">
        /// Authorization context
        /// </param>
        /// <param name="objectSecurityDescriptor">
        /// SECURITY_DESCRIPTOR structure that is assigned to the object
        /// </param>
        /// <returns>True if check is done successfully; otherwise, false</returns>
        public static bool MandatoryIntegrityCheck(
            _SID tokenIntegritySID,
            _SID aceIntegritySID,
            out uint mandatoryInformation,
            Token token,
            _SECURITY_DESCRIPTOR objectSecurityDescriptor)
        {
            //Set TokenPolicy to Token MandatoryPolicy field
            //Set ObjectIntegrityACE to SecurityDescriptor ObjectIntegrity ACE field
            //Set ObjectIntegrityAceMask to SecurityDescriptor ObjectIntegrity Accessmask field
            //
            //IF TokenPolicy.Policy EQUAL TOKEN_MANDATORY_POLICY_OFF OR
            //TokenPolicy.Policy EQUAL TOKEN_MANDATORY_POLICY_NEW_PROCESS_MIN THEN
            //Set MandatoryInformation.AllowedAccess to GENERIC_ALL
            //Return success
            //END IF
            //
            //IF ObjectIntegrityACE.AceFlag NOT EQUAL INHERIT_ONLY_ACE THEN
            //Set AceMask to ObjectIintegrityACE.AccessMask
            //Set AceIntegritysid to ObjectIntegrityACE.TrusteeSid
            //ELSE
            //Set AceMask to SYSTEM_MANDATORY_LABEL_NO_WRITE_UP
            //--The DefaultMandatorySid is derived from policy managed in an
            //--implementation specific manner. The SID for ML_MEDIUM is used by
            //--Windows.
            //Set AceIntegritysid to DefaultMandatorySid
            //END IF

            TokenMandatoryPolicyValue tokenPolicy = token.MandatoryPolicy;
            _SYSTEM_MANDATORY_LABEL_ACE? objectIntegrityACE = null;
            mandatoryInformation = 0;
            bool tokenDominates;

            if (tokenPolicy == TokenMandatoryPolicyValue.TOKEN_MANDATORY_POLICY_OFF
                || tokenPolicy == TokenMandatoryPolicyValue.TOKEN_MANDATORY_POLICY_NEW_PROCESS_MIN)
            {
                mandatoryInformation = ACCESS_MASK_GENERIC_ALL;
                return true;
            }

            if ((objectSecurityDescriptor.Control & SECURITY_DESCRIPTOR_Control.SACLPresent)
                    == SECURITY_DESCRIPTOR_Control.SACLPresent
                && objectSecurityDescriptor.Sacl != null)
            {
                if (objectSecurityDescriptor.Sacl.Value.Aces != null)
                {
                    foreach (object ace in objectSecurityDescriptor.Sacl.Value.Aces)
                    {
                        _SID sid = (_SID)ObjectUtility.GetFieldValue(ace, "Sid");
                        if (ObjectUtility.DeepCompare(sid, aceIntegritySID))
                        {
                            objectIntegrityACE = (_SYSTEM_MANDATORY_LABEL_ACE)ace;
                        }
                    }
                }
            }

            SYSTEM_MANDATORY_LABEL_ACE_Mask objectIntegrityAceMask;
            _SID aceIntegritySid;

            if (objectIntegrityACE != null && objectIntegrityACE.Value.Header.AceFlags != ACE_FLAGS.INHERIT_ONLY_ACE)
            {
                objectIntegrityAceMask = objectIntegrityACE.Value.Mask;
                aceIntegritySid = objectIntegrityACE.Value.Sid;
            }
            else
            {
                objectIntegrityAceMask = SYSTEM_MANDATORY_LABEL_ACE_Mask.SYSTEM_MANDATORY_LABEL_NO_WRITE_UP;
                //The DefaultMandatorySid is derived from policy managed in an
                //implementation specific manner. The SID for ML_MEDIUM is used by
                //Windows.
                aceIntegritySid = GetWellKnownSid(WellKnownSid.ML_MEDIUM, null);
            }
            //
            //Note:SidDominates is removed from the current function, because it's declared but not used.
            //
            //CALL sidEqual(AceIntegritysid, TokenIntegritysid)
            //IF AceIntegritySid EQUALS Token.MandatoryIntegritySid THEN
            //Set TokenDominates to TRUE
            //Set ObjectDominates to TRUE
            //ELSE
            //CALL SidDominates (Token.MandatoryIntegritySid, AceIntegritysid)
            //IF SidDominates returns TRUE THEN
            //Set TokenDominates to TRUE
            //ELSE
            //Set TokenDominates to FALSE
            //END IF
            //Set ObjectDominates to NOT(TokenDominates)
            //END IF

            tokenDominates = SidDominates((_SID)token.IntegrityLevelSid, aceIntegritySid);


            //IF TokenPolicy EQUAL TOKEN_MANDATORY_POLICY_NO_WRITE_UP THEN
            //Add READ to MandatoryInformation.AllowedAccess
            //Add EXECUTE to MandatoryInformation.AllowedAccess
            //IF TokenDominates is TRUE THEN
            //Add WRITE to MandatoryInformation.AllowedAccess
            //END IF
            //END IF
            //
            //IF TokenDominates is FALSE THEN
            //IF ObjectIntegrityAceMask & SYSTEM_MANDATORY_LABEL_NO_READ_UP THEN
            //Remove READ from MandatoryInformation.AllowedAccess
            //END IF
            //IF ObjectIntegrityAceMask & SYSTEM_MANDATORY_LABEL_NO_WRITE_UP THEN
            //Remove WRITE from MandatoryInformation.AllowedAccess
            //END IF
            //IF ObjectIntegrityAceMask & SYSTEM_MANDATORY_LABEL_NO_EXECUTE_UP THEN
            //Remove EXECUTE from MandatoryInformation.AllowedAccess
            //END IF
            //END IF
            //
            //IF Token.Privileges contains SeRelabelPrivilegeTHEN
            //Add WRITE_OWNER to MandatoryInformation.AllowedAccess
            //END IF


            if (tokenPolicy == TokenMandatoryPolicyValue.TOKEN_MANDATORY_POLICY_NO_WRITE_UP)
            {
                mandatoryInformation |= ACCESS_MASK_GENERIC_READ;
                mandatoryInformation |= ACCESS_MASK_GENERIC_EXECUTE;
                if (tokenDominates)
                {
                    mandatoryInformation |= ACCESS_MASK_GENERIC_WRITE;
                }
            }

            if (!tokenDominates)
            {
                if ((objectIntegrityAceMask
                    & SYSTEM_MANDATORY_LABEL_ACE_Mask.SYSTEM_MANDATORY_LABEL_NO_READ_UP)
                    == SYSTEM_MANDATORY_LABEL_ACE_Mask.SYSTEM_MANDATORY_LABEL_NO_READ_UP)
                {
                    mandatoryInformation &= ~ACCESS_MASK_GENERIC_READ;
                }
                if ((objectIntegrityAceMask
                    & SYSTEM_MANDATORY_LABEL_ACE_Mask.SYSTEM_MANDATORY_LABEL_NO_WRITE_UP)
                    == SYSTEM_MANDATORY_LABEL_ACE_Mask.SYSTEM_MANDATORY_LABEL_NO_WRITE_UP)
                {
                    mandatoryInformation &= ~ACCESS_MASK_GENERIC_WRITE;
                }
                if ((objectIntegrityAceMask
                    & SYSTEM_MANDATORY_LABEL_ACE_Mask.SYSTEM_MANDATORY_LABEL_NO_EXECUTE_UP)
                    == SYSTEM_MANDATORY_LABEL_ACE_Mask.SYSTEM_MANDATORY_LABEL_NO_EXECUTE_UP)
                {
                    mandatoryInformation &= ~ACCESS_MASK_GENERIC_EXECUTE;
                }
            }

            if (token.Privileges != null)
            {
                foreach (_LUID privilege in token.Privileges)
                {
                    if (ObjectUtility.DeepCompare(privilege, GetPrivilegeLuid(PrivilegeName.SE_RELABEL_NAME)))
                    {
                        mandatoryInformation |= ACCESS_MASK_WRITE_OWNER;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Create a _SECURITY_DESCRIPTOR structure in self-relative format.
        /// </summary>
        /// <param name="control">An unsigned 16-bit field that specifies control access bit flags.</param>
        /// <param name="ownerSid">The SID of the owner of the object</param>
        /// <param name="groupSid">The SID of the group of the object.</param>
        /// <param name="sacl">The SACL of the object.</param>
        /// <param name="dacl">The DACL of the object.</param>
        /// <returns> Output security descriptor for the object.</returns>
        public static _SECURITY_DESCRIPTOR CreateSecurityDescriptor(
            SECURITY_DESCRIPTOR_Control control,
            _SID? ownerSid,
            _SID? groupSid,
            _ACL? sacl,
            _ACL? dacl)
        {
            RawAcl sRawAcl = null;
            RawAcl dRawAcl = null;
            SecurityIdentifier rawOwnerSid = null;
            SecurityIdentifier rawGroupSid = null;
            if (ownerSid != null)
            {
                rawOwnerSid = new SecurityIdentifier(TypeMarshal.ToBytes(ownerSid.Value), 0);
            }
            if (groupSid != null)
            {
                rawGroupSid = new SecurityIdentifier(TypeMarshal.ToBytes(groupSid.Value), 0);
            }
            if (sacl != null)
            {
                sRawAcl = new RawAcl(DtypUtility.EncodeAcl(sacl.Value), 0);
            }
            if (dacl != null)
            {
                dRawAcl = new RawAcl(DtypUtility.EncodeAcl(dacl.Value), 0);
            }
            RawSecurityDescriptor rawSecurityDescriptor = new RawSecurityDescriptor(
                (ControlFlags)control,
                rawOwnerSid,
                rawGroupSid,
                sRawAcl,
                dRawAcl);
            byte[] rawSecurityDescriptorBytes = new byte[rawSecurityDescriptor.BinaryLength];
            rawSecurityDescriptor.GetBinaryForm(rawSecurityDescriptorBytes, 0);

            return DtypUtility.DecodeSecurityDescriptor(rawSecurityDescriptorBytes);
        }

        /// <summary>
        /// Create SecurityDescriptor in absolute format. MS-DTYP section 2.5.4.3
        /// </summary>
        /// <param name="parentDescriptor">
        /// Security descriptor for the parent (container) object of the new object. 
        /// If the object has no parent, this parameter is null.
        /// </param>
        /// <param name="creatorDescriptor">
        /// Security descriptor for the new object provided by the creator of the object. 
        /// Caller can pass null.
        /// </param>
        /// <param name="isContainerObject">
        /// TRUE when the object is a container; otherwise, FALSE.
        /// </param>
        /// <param name="objectTypes">
        /// An array of pointers to GUID structures that identify the object types or 
        /// classes of the object associated with NewDescriptor (the return value). 
        /// For Active Directory objects, this array contains pointers to the class 
        /// GUIDs of the object's structural class and all attached auxiliary classes. 
        /// If the object for which this descriptor is being created does not have a GUID, 
        /// this field MUST be set to null.
        /// </param>
        /// <param name="autoInheritFlags">
        /// A set of bit flags that control how access control entries (ACEs) are 
        /// inherited from ParentDescriptor.
        /// </param>
        /// <param name="token">
        /// Token supplied by the caller for default security information for the new object.
        /// </param>
        /// <param name="genericMapping">
        /// Mapping of generic permissions to resource manager-specific permissions 
        /// supplied by the caller.
        /// </param>
        /// <returns>
        /// Output security descriptor for the object computed by the algorithm.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the OwnerSid or GroupSid field of parentDescriptor is null, but
        /// autoInheritFlags is set to DEFAULT_OWNER_FROM_PARENT or DEFAULT_GROUP_FROM_PARENT.
        /// Thrown when the Dacl field of parentDescriptor doesn't exist, but the DACLPresent flag is set.
        /// Thrown when the Sacl field of parentDescriptor doesn't exist, but the SP flag is set.
        /// </exception>
        public static _SECURITY_DESCRIPTOR CreateSecurityDescriptor(
            _SECURITY_DESCRIPTOR? parentDescriptor,
            _SECURITY_DESCRIPTOR? creatorDescriptor,
            bool isContainerObject,
            Guid[] objectTypes,
            SecurityDescriptorAutoInheritFlags autoInheritFlags,
            Token token,
            GenericMapping genericMapping)
        {
            const byte DEFAULT_REVISION = 1;

            _SECURITY_DESCRIPTOR newDescriptor = new _SECURITY_DESCRIPTOR();
            newDescriptor.Revision = DEFAULT_REVISION;

            //Compute the Owner field (v20100908)

            //IF CreatorDescriptor.Owner is NULL THEN
            //IF AutoInheritFlags contains DEFAULT_OWNER_FROM_PARENT THEN
            //Set NewDescriptor.Owner to ParentDescriptor.Owner
            //ELSE
            //Set NewDescriptor.Owner to Token.SIDs[Token.OwnerIndex]
            //ENDIF
            //ELSE
            //Set NewDescriptor.Owner to CreatorDescriptor.Owner
            //ENDIF

            if (creatorDescriptor != null)
            {
                newDescriptor.OwnerSid = creatorDescriptor.Value.OwnerSid;
            }
            else if ((autoInheritFlags & SecurityDescriptorAutoInheritFlags.DEFAULT_OWNER_FROM_PARENT)
                == SecurityDescriptorAutoInheritFlags.DEFAULT_OWNER_FROM_PARENT)
            {
                if (parentDescriptor != null)
                {
                    newDescriptor.OwnerSid = parentDescriptor.Value.OwnerSid;
                }
                else
                {
                    throw new ArgumentException(
                        "The parentDescriptor doesn't exist, but DEFAULT_OWNER_FROM_PARENT flag is set.",
                        "parentDescriptor");
                }
            }
            else
            {
                newDescriptor.OwnerSid = token.Sids[token.OwnerIndex];
            }

            //Compute the Group field

            //IF CreatorDescriptor.Group is NULL THEN
            //IF AutoInheritFlags contains DEFAULT_GROUP_FROM_PARENT THEN
            //Set NewDescriptor.Group to ParentDescriptor.Group
            //ELSE
            //Set NewDescriptor.Group to Token.SIDs[Token.PrimaryGroup]
            //ENDIF
            //ELSE
            //Set NewDescriptor.Group to CreatorDescriptor.Group
            //ENDIF
            if (creatorDescriptor != null)
            {
                newDescriptor.GroupSid = creatorDescriptor.Value.GroupSid;
            }
            else if ((autoInheritFlags & SecurityDescriptorAutoInheritFlags.DEFAULT_GROUP_FROM_PARENT)
                == SecurityDescriptorAutoInheritFlags.DEFAULT_GROUP_FROM_PARENT)
            {
                if (parentDescriptor != null)
                {
                    newDescriptor.GroupSid = parentDescriptor.Value.GroupSid;
                }
                else
                {
                    throw new ArgumentException(
                        "The parentDescriptor doesn't exist, but DEFAULT_GROUP_FROM_PARENT flag is set.",
                        "parentDescriptor");
                }
            }
            else
            {
                newDescriptor.GroupSid = token.Sids[token.PrimaryGroup];
            }

            _ACL? parentDacl = null;
            _ACL? parentSacl = null;
            SECURITY_DESCRIPTOR_Control parentControl = SECURITY_DESCRIPTOR_Control.None;
            _ACL? creatorDacl = null;
            _ACL? creatorSacl = null;
            SECURITY_DESCRIPTOR_Control creatorControl = SECURITY_DESCRIPTOR_Control.None;
            #region Check Sacl or Dacl filed against SP or DACLPresent flag in the control field.

            if (parentDescriptor != null)
            {
                parentControl = parentDescriptor.Value.Control;
                if ((parentDescriptor.Value.Control & SECURITY_DESCRIPTOR_Control.DACLPresent)
                         == SECURITY_DESCRIPTOR_Control.DACLPresent)
                {
                    if (parentDescriptor.Value.Dacl == null)
                    {
                        throw new ArgumentException(
                            "The Dacl field of parentDescriptor doesn't exist, but the DACLPresent flag is set.",
                            "parentDescriptor");
                    }
                    else
                    {
                        parentDacl = parentDescriptor.Value.Dacl;
                    }
                }
                if ((parentDescriptor.Value.Control & SECURITY_DESCRIPTOR_Control.SACLPresent)
                        == SECURITY_DESCRIPTOR_Control.SACLPresent)
                {
                    if (parentDescriptor.Value.Sacl == null)
                    {
                        throw new ArgumentException(
                            "The Sacl field of parentDescriptor doesn't exist, but the SP flag is set.",
                            "parentDescriptor");
                    }
                    else
                    {
                        parentSacl = parentDescriptor.Value.Sacl;
                    }
                }
            }
            if (creatorDescriptor != null)
            {
                creatorControl = creatorDescriptor.Value.Control;
                if ((creatorDescriptor.Value.Control & SECURITY_DESCRIPTOR_Control.DACLPresent)
                        == SECURITY_DESCRIPTOR_Control.DACLPresent)
                {
                    if (creatorDescriptor.Value.Dacl == null)
                    {
                        throw new ArgumentException(
                            "The Dacl field of creatorDescriptor doesn't exist, but the DACLPresent flag is set.",
                            "creatorDescriptor");
                    }
                    else
                    {
                        creatorDacl = creatorDescriptor.Value.Dacl;
                    }
                }
                if ((creatorDescriptor.Value.Control & SECURITY_DESCRIPTOR_Control.SACLPresent)
                        == SECURITY_DESCRIPTOR_Control.SACLPresent)
                {
                    if (creatorDescriptor.Value.Sacl == null)
                    {
                        throw new ArgumentException(
                            "The Sacl field of creatorDescriptor doesn't exist, but the SP flag is set.",
                            "creatorDescriptor");
                    }
                    else
                    {
                        creatorSacl = creatorDescriptor.Value.Sacl;
                    }
                }
            }

            #endregion

            //Compute the DACL

            //CALL ComputeACL WITH
            //COMPUTE_DACL, ParentDescriptor.DACL, ParentDescriptor.Control,
            //CreatorDescriptor.DACL,CreatorDescriptor.Control       
            //IsContainerObject, ObjectTypes, GenericMapping,
            //NewDescriptor.Owner, NewDescriptor.Group, Token
            //RETURNING NewDACL, NewControl
            //Set NewDescriptor.DACL to NewDACL
            //Set NewDescriptor.Control to NewControl

            SECURITY_DESCRIPTOR_Control newControl = SECURITY_DESCRIPTOR_Control.None;
            _ACL? newDacl = ComputeACL(
               AclComputeType.COMPUTE_DACL,
               parentDacl,
               parentControl,
               creatorDacl,
               creatorControl,
               isContainerObject,
               objectTypes,
               autoInheritFlags,
               genericMapping,
               newDescriptor.OwnerSid.Value,
               newDescriptor.GroupSid.Value,
               token,
               out newControl);

            newDescriptor.Dacl = newDacl;
            newDescriptor.Control = newControl;

            //Compute the SACL

            //CALL ComputeACL WITH
            //COMPUTE_SACL, ParentDescriptor.SACL, ParentDescriptor.Control,
            //CreatorDescriptor.SACL,CreatorDescriptor.Control
            //IsContainerObject, ObjectTypes, GenericMapping,
            //NewDescriptor.Owner, NewDescriptor.Group, Token
            //RETURNING NewSACL, NewControl
            //Set NewDescriptor.SACLto NewSACL
            //Set NewDescriptor.Control to (NewDescriptor.Control OR NewControl)
            //RETURN NewDescriptor

            _ACL? newSacl = ComputeACL(
                    AclComputeType.COMPUTE_SACL,
                    parentSacl,
                    parentControl,
                    creatorSacl,
                    creatorControl,
                    isContainerObject,
                    objectTypes,
                    autoInheritFlags,
                    genericMapping,
                    newDescriptor.OwnerSid.Value,
                    newDescriptor.GroupSid.Value,
                    token,
                    out newControl);

            newDescriptor.Sacl = newSacl;
            newDescriptor.Control |= newControl;

            //New SecurityDescriptor is in absolute format.
            newDescriptor.Control = newDescriptor.Control & (~SECURITY_DESCRIPTOR_Control.SelfRelative);
            return newDescriptor;
        }


        /// <summary>
        /// ComputeACL. MS-DTYP section 2.5.4.4
        /// </summary>
        /// <param name="computeType">Enumeration of COMPUTE_DACL and COMPUTE_SACL.</param>
        /// <param name="parentAcl">ACL from the parent security descriptor.</param>
        /// <param name="parentControl">Control flags from the parent security descriptor.</param>
        /// <param name="creatorAcl">ACL supplied in the security descriptor by the creator.</param>
        /// <param name="creatorControl">Control flags supplied in the security descriptor by the creator.</param>
        /// <param name="isContainerObject">TRUE if the object is a container; otherwise, FALSE.</param>
        /// <param name="objectTypes">Array of GUIDs for the object type being created.</param>
        /// <param name="autoInheritFlags">
        /// A set of bit flags that control how access control entries (ACEs) are 
        /// inherited from ParentDescriptor.
        /// </param>
        /// <param name="genericMapping">
        /// Mapping of generic permissions to resource manager-specific permissions supplied by the caller.
        /// </param>
        /// <param name="owner">Owner to use in substituting the CreatorOwner SID.</param>
        /// <param name="group">Group to use in substituting the CreatorGroup SID.</param>
        /// <param name="token">Token for default values.</param>
        /// <param name="computedControl">ComputedControl</param>
        /// <returns>Computed ACL</returns>
        public static _ACL? ComputeACL(
            AclComputeType computeType,
            _ACL? parentAcl,
            SECURITY_DESCRIPTOR_Control parentControl,
            _ACL? creatorAcl,
            SECURITY_DESCRIPTOR_Control creatorControl,
            bool isContainerObject,
            Guid[] objectTypes,
            SecurityDescriptorAutoInheritFlags autoInheritFlags,
            GenericMapping genericMapping,
            _SID owner,
            _SID group,
            Token token,
            out SECURITY_DESCRIPTOR_Control computedControl)
        {
            #region Pseudocode  v20110329

            //// The details of the algorithm to merge the parent ACL and the supplied ACL.
            //// The Control flags computed are slightly different based on whether it is the 
            //// ACL in the DACL or the SACL field of the descriptor.
            //// The caller specifies whether it is a DACL or a SACL using the parameter,
            //// ComputeType.
            //Set ComputedACL to NULL
            //Set ComputedControl to NULL

            //CALL ContainsInheritableACEs WITH ParentACL RETURNING ParentHasInheritableACEs

            //IF ParentHasInheritableACEs = TRUE THEN

            //    // The Parent ACL has inheritable ACEs. The Parent ACL should be used if no Creator
            //    // ACL is supplied, or if the Creator ACL was supplied AND it is a default ACL based
            //    // on object type information

            //    IF(CreatorACL is not present) OR
            //      ((CreatorACL is present) AND
            //      (AutoInheritFlags contains DEFAULT_DESCRIPTOR_FOR_OBJECT))
            //    THEN
            //        // Use only the inherited ACEs from the parent.  First compute the ACL from the 
            //        // parent ACL, then clean it up by resolving the generic mappings etc.

            //        CALL ComputeInheritedACLFromParent WITH
            //          ACL set to ParentACL,
            //          IsContainerObject set to IsContainerObject,
            //          ObjectTypes set to ObjectTypes

            //        RETURNING NextACL
            //        CALL PostProcessACL WITH
            //          ACL set to NextACL,
            //          CopyFilter set to CopyInheritedAces,
            //          Owner set to Owner,
            //          Group set to Group,
            //          GenericMapping set to GenericMapping

            //        RETURNING FinalACL

            //        Set ComputedACL to FinalACL
            //        RETURN
            //    ENDIF

            //IF ((CreatorACL is present) AND
            //  (AutoInheritFlags does not contain DEFAULT_DESCRIPTOR_FOR_OBJECT))
            //THEN
            //    // Since a creator ACL is present, and we’re not defaulting the
            //    // descriptor, determine which ACEs are inherited and compute the new ACL
            //    CALL PreProcessACLFromCreator WITH 
            //       ACL set to CreatorACL
            //    RETURNING PreACL

            //    CALL ComputeInheritedACLFromCreator WITH
            //       ACL set to PreACL,
            //       IsContainerObject set to IsContainerObject,
            //       ObjectTypes set to ObjectTypes
            //    RETURNING TmpACL

            //    //  Special handling for DACL types of ACLs

            //    IF (ComputeType = DACL_COMPUTE) THEN

            //        // DACL-specific operations

            //        IF (CreatorControl does not have DACL_PROTECTED flag set) AND
            //           (AutoInheritFlags contains DACL_AUTO_INHERIT)
            //        THEN 

            //            //  We’re not working from a protected DACL, and we’re supposed to
            //            //  allow automatic inheritance.  Compute the inherited ACEs from
            //            //  Parent ACL this time, and append that to the ACL that we’re building

            //            CALL ComputeInheritedACLFromParent WITH
            //              ACL set to ParentACL,
            //              IsContainerObject set to IsContainerObject,
            //              ObjectTypes set to ObjectTypes
            //            RETURNING InheritedParentACL

            //            Append InheritedParentACL.ACEs to TmpACL.ACE
            //            Set DACL_AUTO_INHERITED flag in ComputedControl

            //        ENDIF

            //    ENDIF  // DACL-Specific behavior
            //    IF (ComputeType = SACL_COMPUTE) THEN

            //        // Similar to the above, perform SACL-specific operations

            //        IF (CreatorControl does not have SACL_PROTECTED flag set) AND
            //           (AutoInheritFlags contains SACL_AUTO_INHERIT flag)
            //        THEN

            //            //  We’re not working from a protected SACL, and we’re supposed to
            //            //  allow automatic inheritance.  Compute the inherited ACEs from
            //            //  Parent ACL this time, and append that to the ACL that we’re building

            //            CALL ComputeInheritedACLFromParent WITH
            //              ACL set to ParentACL,
            //              IsContainerObject set to IsContainerObject,
            //              ObjectTypes set to ObjectTypes
            //            RETURNING InheritedParentACL

            //            Append InheritedParentACL.ACEs to TmpACL.ACE
            //            Set SACL_AUTO_INHERITED flag in ComputedControl

            //        ENDIF

            //    ENDIF  // SACL-Specific behavior

            //    CALL PostProcessACL WITH
            //      ACL set to TmpACL,
            //      CopyFilter set to CopyInheritedAces,
            //      Owner set to Owner,
            //      Group set to Group,
            //      GenericMapping set to GenericMapping
            //    RETURNING ProcessedACL

            //    Set ComputedACL to ProcessedACL
            //    RETURN
            //ENDIF  // CreatorACL is present

            //ELSE // ParentACL does not contain inheritable ACEs

            //    IF CreatorACL = NULL THEN
            //        // No ACL supplied for the object
            //        IF (ComputeType = DACL_COMPUTE) THEN
            //            Set TmpACL to Token.DefaultDACL
            //        ELSE
            //            // No default for SACL; left as NULL
            //        ENDIF

            //    ELSE
            //        // Explicit ACL was supplied for the object - either default or not.
            //        // In either case, use it for the object, since there are no inherited ACEs.
            //        CALL PreProcessACLFromCreator WITH CreatorACL
            //        RETURNING TmpACL
            //    ENDIF

            //    CALL PostProcessACL WITH
            //      ACL set to TmpACL,
            //      CopyFilter set to CopyAllAces,
            //      Owner set to Owner,
            //      Group set to Group,
            //      GenericMapping set to GenericMapping

            //        RETURNING ProcessedACL
            //        Set ComputedACL to ProcessedACL

            //ENDIF
            //// END ComputeACL

            #endregion

            _ACL? computedACL = null;
            computedControl = SECURITY_DESCRIPTOR_Control.None;
            _ACL tmpAcl = new _ACL();
            _ACL preAcl = new _ACL();

            if (parentAcl != null && ContainsInheritableACEs(parentAcl.Value))
            {
                // ParentACL contains inheritable ACEs
                if ((creatorAcl == null) || ((creatorAcl != null) && (autoInheritFlags
                    & SecurityDescriptorAutoInheritFlags.DEFAULT_DESCRIPTOR_FOR_OBJECT)
                    == SecurityDescriptorAutoInheritFlags.DEFAULT_DESCRIPTOR_FOR_OBJECT))
                {
                    // Use only the inherited ACEs from the parent
                    _ACL nextAcl = ComputeInheritedACLfromParent(parentAcl.Value, isContainerObject, objectTypes);
                    _ACL finalAcl = PostProcessACL(nextAcl, CopyFilter.CopyInheritedAces, owner, group, genericMapping);
                    computedACL = finalAcl;
                }
                else
                {
                    preAcl = PreProcessACLfromCreator(creatorAcl.Value);
                    tmpAcl = ComputeInheritedACLfromCreator(preAcl, isContainerObject, objectTypes);

                    _ACL? inheritedAcl = null;
                    if ((computeType == AclComputeType.COMPUTE_DACL)
                        && (creatorControl & SECURITY_DESCRIPTOR_Control.DACLProtected)
                            == SECURITY_DESCRIPTOR_Control.None
                        && (autoInheritFlags & SecurityDescriptorAutoInheritFlags.DACL_AUTO_INHERIT)
                            == SecurityDescriptorAutoInheritFlags.DACL_AUTO_INHERIT)
                    {
                        // Compute the inherited ACEs from the parent
                        inheritedAcl = ComputeInheritedACLfromParent(
                            parentAcl.Value,
                            isContainerObject,
                            objectTypes);

                        computedControl |= SECURITY_DESCRIPTOR_Control.DACLAutoInherited;
                    }
                    else if ((computeType == AclComputeType.COMPUTE_SACL)
                        && (creatorControl & SECURITY_DESCRIPTOR_Control.SACLProtected)
                            == SECURITY_DESCRIPTOR_Control.None
                        && (autoInheritFlags & SecurityDescriptorAutoInheritFlags.SACL_AUTO_INHERIT)
                            == SecurityDescriptorAutoInheritFlags.SACL_AUTO_INHERIT)
                    {
                        // Compute the inherited ACEs from the parent
                        inheritedAcl = ComputeInheritedACLfromParent(
                            parentAcl.Value,
                            isContainerObject,
                            objectTypes);

                        computedControl |= SECURITY_DESCRIPTOR_Control.SACLAutoInherited;
                    }
                    if (inheritedAcl != null)
                    {
                        List<object> tmpAces = new List<object>();
                        tmpAces.AddRange(tmpAcl.Aces);
                        tmpAces.AddRange(inheritedAcl.Value.Aces);
                        tmpAcl.Aces = tmpAces.ToArray();
                        tmpAcl.AceCount = (ushort)tmpAcl.Aces.Length;
                        tmpAcl.AclSize = DtypUtility.CalculateAclSize(tmpAcl);
                    }

                    computedACL = PostProcessACL(tmpAcl, CopyFilter.CopyInheritedAces, owner, group, genericMapping);
                }
            }
            else
            {
                // ParentACL does not contain inheritable ACEs
                if (creatorAcl == null)
                {
                    if (computeType == AclComputeType.COMPUTE_DACL)
                    {
                        tmpAcl = token.DefaultDACL;
                    }
                    else
                    {
                        //No default for SACL;left as NULL
                        return null;
                    }
                }
                else
                {
                    // Explicit ACL was supplied for the object - either default or not.
                    // In either case, use it for the object, since there are no inherited ACEs.
                    tmpAcl = PreProcessACLfromCreator(creatorAcl.Value);
                }
                computedACL = PostProcessACL(tmpAcl, CopyFilter.CopyAllAces, owner, group, genericMapping);
            }

            if (computedACL != null)
            {
                computedControl |= ((computeType == AclComputeType.COMPUTE_DACL)
                    ? SECURITY_DESCRIPTOR_Control.DACLPresent
                    : SECURITY_DESCRIPTOR_Control.SACLPresent);
            }
            return computedACL;
        }

        /// <summary>
        /// ContainsInheritableACEs. MS-DTYP section 2.5.4.5
        /// </summary>
        /// <param name="acl">ACL</param>
        /// <returns>Return TRUE if contains inheritable ACE; otherwise, FALSE.</returns>
        public static bool ContainsInheritableACEs(_ACL acl)
        {
            //FOR each ACE in ACL DO
            //IF(ACE.Flags contains CONTAINER_INHERIT_ACE) OR
            //(ACE.Flags contains OBJECT_INHERIT_ACE)
            //THEN
            //RETURN TRUE
            //ENDIF
            //END FOR
            //RETURN FALSE

            if (acl.Aces != null)
            {
                foreach (object ace in acl.Aces)
                {
                    object newAce = ObjectUtility.DeepClone(ace);
                    _ACE_HEADER header = (_ACE_HEADER)ObjectUtility.GetFieldValue(newAce, "Header");

                    if ((header.AceFlags & ACE_FLAGS.CONTAINER_INHERIT_ACE) == ACE_FLAGS.CONTAINER_INHERIT_ACE
                        || (header.AceFlags & ACE_FLAGS.OBJECT_INHERIT_ACE) == ACE_FLAGS.OBJECT_INHERIT_ACE)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Compute inherited ACE from parent.
        /// </summary>
        /// <param name="newAce">
        /// The current ace to be computed.
        /// </param>
        /// <param name="objectTypes">
        /// An array of GUIDs for the object type being created.
        /// </param>
        /// <returns>
        /// True if the current ace is an inherited ace from parent ACL. Otherwise, false.
        /// </returns>
        private static bool ComputeInheritedAcefromParent(
           ref  object newAce,
            Guid[] objectTypes)
        {

            //CASE ACE.Type OF
            //ALLOW:
            //DENY:
            //Set NewACE to ACE
            //Set NewACE.Flags to INHERITED_ACE
            //Append NewACE to ExplicitACL
            //OBJECT_ALLOW:
            //OBJECT_DENY:
            //IF (ObjectTypes contains ACE.ObjectGUID) THEN
            //Set NewACE to ACE
            //Set NewACE.Flags to INHERITED_ACE
            //Append NewACE to ExplicitACL
            //ENDIF
            //ENDCASE

            _ACE_HEADER header = (_ACE_HEADER)ObjectUtility.GetFieldValue(newAce, "Header");
            ACCESS_OBJECT_ACE_Flags flags = ACCESS_OBJECT_ACE_Flags.None;
            ACE_TYPE aceType = header.AceType;

            if (header.AceType == ACE_TYPE.ACCESS_ALLOWED_OBJECT_ACE_TYPE
                || header.AceType == ACE_TYPE.ACCESS_DENIED_OBJECT_ACE_TYPE)
            {
                flags = (ACCESS_OBJECT_ACE_Flags)ObjectUtility.GetFieldValue(newAce, "Flags");
                if ((flags & ACCESS_OBJECT_ACE_Flags.ACE_OBJECT_TYPE_PRESENT)
                    != ACCESS_OBJECT_ACE_Flags.ACE_OBJECT_TYPE_PRESENT)
                {
                    aceType = (header.AceType == ACE_TYPE.ACCESS_ALLOWED_OBJECT_ACE_TYPE)
                        ? ACE_TYPE.ACCESS_ALLOWED_ACE_TYPE
                        : ACE_TYPE.ACCESS_DENIED_ACE_TYPE;
                }
            }

            switch (aceType)
            {
                case ACE_TYPE.ACCESS_ALLOWED_ACE_TYPE:
                case ACE_TYPE.ACCESS_DENIED_ACE_TYPE:
                    header.AceFlags = ACE_FLAGS.INHERITED_ACE;
                    ObjectUtility.SetFieldValue(newAce, "Header", header);
                    return true;

                case ACE_TYPE.ACCESS_ALLOWED_OBJECT_ACE_TYPE:
                case ACE_TYPE.ACCESS_DENIED_OBJECT_ACE_TYPE:
                    if (objectTypes != null)
                    {
                        Guid objectType = (Guid)ObjectUtility.GetFieldValue(newAce, "ObjectType");
                        foreach (Guid guid in objectTypes)
                        {
                            if (guid == objectType)
                            {
                                header.AceFlags = ACE_FLAGS.INHERITED_ACE;
                                ObjectUtility.SetFieldValue(newAce, "Header", header);
                                return true;
                            }
                        }
                    }
                    break;
            }

            return false;
        }

        /// <summary>
        /// ComputeInheritedACLfromParent. MS-DTYP section 2.5.4.6
        /// </summary>
        /// <param name="acl">
        /// An ACL that contains the parent's ACEs from which to compute the inherited ACL.
        /// </param>
        /// <param name="isContainerObject">
        /// TRUE if the object is a container; otherwise, FALSE.
        /// </param>
        /// <param name="objectTypes">
        /// An array of GUIDs for the object type being created.
        /// </param>
        /// <returns>
        /// The computed ACL that also includes the inherited ACEs.
        /// </returns>
        public static _ACL ComputeInheritedACLfromParent(
            _ACL acl,
            bool isContainerObject,
            Guid[] objectTypes)
        {


            //Initialize ExplicitACL to Empty ACL
            //FOR each ACE in ACL DO
            //IF ACE.Flags contains INHERIT_ONLY_ACE
            //THEN
            //CONTINUE
            //ENDIF
            //IF(((ACE.Flags contains CONTAINER_INHERIT_ACE) AND
            //(IsContainerObject = TRUE))OR
            //((ACE.Flags contains OBJECT_INHERIT_ACE) AND
            //(IsContainerObject = FALSE)))
            //THEN
            //    CASE ACE.Type OF
            //    ALLOW:
            //    DENY:
            //    Set NewACE to ACE
            //    Set NewACE.Flags to INHERITED_ACE
            //    Append NewACE to ExplicitACL
            //    OBJECT_ALLOW:
            //    OBJECT_DENY:
            //    IF (ObjectTypes contains ACE.ObjectGUID) THEN
            //    Set NewACE to ACE
            //    Set NewACE.Flags to INHERITED_ACE
            //    Append NewACE to ExplicitACL
            //    ENDIF
            //    ENDCASE
            //ENDIF
            //END FOR
            //Initialize InheritableACL to Empty ACL
            //IF (IsContainerObject = TRUE) THEN
            //FOR each ACE in ACL DO
            //IF ACE.Flags contains NO_PROPAGATE_INHERIT_ACE THEN
            //CONTINUE
            //ENDIF
            //IF((ACE.Flags contains CONTAINER_INHERIT_ACE) OR
            //(ACE.Flags contains OBJECT_INHERIT_ACE))
            //THEN
            //Set NewACE to ACE
            //Add INHERITED_ACE to NewACE.Flags
            //Add INHERIT_ONLY_ACE to NewACE.Flags
            //Append NewACE to InheritableACL
            //ENDIF
            //END FOR
            //ENDIF
            //RETURN concatenation of ExplicitACL and InheritableACL

            _ACL newAcl = new _ACL();
            newAcl.AclRevision = acl.AclRevision;
            List<object> newAces = new List<object>();

            #region Computes the explicit Acl
            if (acl.Aces != null)
            {
                foreach (object ace in acl.Aces)
                {
                    object newAce = ObjectUtility.DeepClone(ace);
                    _ACE_HEADER header = (_ACE_HEADER)ObjectUtility.GetFieldValue(newAce, "Header");

                    if ((header.AceFlags & ACE_FLAGS.INHERIT_ONLY_ACE) == ACE_FLAGS.INHERIT_ONLY_ACE)
                    {
                        continue;
                    }

                    if (((header.AceFlags & ACE_FLAGS.CONTAINER_INHERIT_ACE)
                            == ACE_FLAGS.CONTAINER_INHERIT_ACE && isContainerObject)
                        || ((header.AceFlags & ACE_FLAGS.OBJECT_INHERIT_ACE)
                            == ACE_FLAGS.OBJECT_INHERIT_ACE && (!isContainerObject)))
                    {
                        bool retVal = DtypUtility.ComputeInheritedAcefromParent(ref newAce, objectTypes);
                        if (retVal)
                        {
                            newAces.Add(newAce);
                        }
                    }
                }
            }
            #endregion

            #region Compute the inheritable ACL
            if (isContainerObject && acl.AceCount >= 0 && acl.Aces != null)
            {
                foreach (object ace in acl.Aces)
                {
                    object newAce = ObjectUtility.DeepClone(ace);
                    _ACE_HEADER header = (_ACE_HEADER)ObjectUtility.GetFieldValue(newAce, "Header");

                    if ((header.AceFlags & ACE_FLAGS.NO_PROPAGATE_INHERIT_ACE)
                            == ACE_FLAGS.NO_PROPAGATE_INHERIT_ACE)
                    {
                        continue;
                    }

                    if ((header.AceFlags & ACE_FLAGS.CONTAINER_INHERIT_ACE) == ACE_FLAGS.CONTAINER_INHERIT_ACE
                        || (header.AceFlags & ACE_FLAGS.OBJECT_INHERIT_ACE) == ACE_FLAGS.OBJECT_INHERIT_ACE)
                    {
                        header.AceFlags |= (ACE_FLAGS.INHERIT_ONLY_ACE | ACE_FLAGS.INHERITED_ACE);
                        ObjectUtility.SetFieldValue(newAce, "Header", header);
                        //Append NewACE to NewACL
                        newAces.Add(newAce);
                    }
                }
            }
            #endregion

            newAcl.Aces = newAces.ToArray();
            newAcl.AceCount = (ushort)newAcl.Aces.Length;
            newAcl.AclSize = DtypUtility.CalculateAclSize(newAcl);

            return newAcl;
        }


        /// <summary>
        /// Compute inherited ACE from creator.
        /// </summary>
        /// <param name="newAce">
        /// The current ace to be computed.
        /// </param>
        /// <param name="objectTypes">
        /// An array of GUIDs for the object type being created.
        /// </param>
        /// <returns>
        /// True if the current ace is an inherited ace from creator ACL. Otherwise, false.
        /// </returns>
        private static bool ComputeInheritedAcefromCreator(
           ref  object newAce,
            Guid[] objectTypes)
        {

            //CASE ACE.Type OF
            //ALLOW:
            //DENY:
            //Set NewACE to ACE
            //Set NewACE.Flags to NULL
            //Append NewACE to ExplicitACL
            //OBJECT_ALLOW
            //OBJECT_DENY:
            //IF (ObjectTypes contains ACE.ObjectGUID) THEN
            //Set NewACE to ACE
            //Set NewACE.Flags to NULL
            //Append NewACE to ExplicitACL
            //ENDIF
            //ENDCASE

            _ACE_HEADER header = (_ACE_HEADER)ObjectUtility.GetFieldValue(newAce, "Header");
            ACCESS_OBJECT_ACE_Flags flags = ACCESS_OBJECT_ACE_Flags.None;
            ACE_TYPE aceType = header.AceType;

            if (header.AceType == ACE_TYPE.ACCESS_ALLOWED_OBJECT_ACE_TYPE
                || header.AceType == ACE_TYPE.ACCESS_DENIED_OBJECT_ACE_TYPE)
            {
                flags = (ACCESS_OBJECT_ACE_Flags)ObjectUtility.GetFieldValue(newAce, "Flags");
                if ((flags & ACCESS_OBJECT_ACE_Flags.ACE_OBJECT_TYPE_PRESENT)
                    != ACCESS_OBJECT_ACE_Flags.ACE_OBJECT_TYPE_PRESENT)
                {
                    aceType = (header.AceType == ACE_TYPE.ACCESS_ALLOWED_OBJECT_ACE_TYPE)
                        ? ACE_TYPE.ACCESS_ALLOWED_ACE_TYPE
                        : ACE_TYPE.ACCESS_DENIED_ACE_TYPE;
                }
            }

            switch (aceType)
            {
                case ACE_TYPE.ACCESS_ALLOWED_ACE_TYPE:
                case ACE_TYPE.ACCESS_DENIED_ACE_TYPE:
                    header.AceFlags = ACE_FLAGS.None;
                    ObjectUtility.SetFieldValue(newAce, "Header", header);
                    return true;

                case ACE_TYPE.ACCESS_ALLOWED_OBJECT_ACE_TYPE:
                case ACE_TYPE.ACCESS_DENIED_OBJECT_ACE_TYPE:
                    if (objectTypes != null)
                    {
                        Guid objectType = (Guid)ObjectUtility.GetFieldValue(newAce, "ObjectType");
                        foreach (Guid guid in objectTypes)
                        {
                            if (guid == objectType)
                            {
                                header.AceFlags = ACE_FLAGS.None;
                                ObjectUtility.SetFieldValue(newAce, "Header", header);
                                return true;
                            }
                        }
                    }
                    break;
            }

            return false;
        }

        /// <summary>
        /// ComputeInheritedACLfromCreator. MS-DTYP section 2.5.4.7
        /// </summary>
        /// <param name="acl">
        /// An ACL supplied in the security descriptor by the caller.
        /// </param>
        /// <param name="isContainerObject">
        /// TRUE if the object is a container; otherwise, FALSE.
        /// </param>
        /// <param name="objectTypes">
        /// An array of GUIDs for the object type being created.
        /// </param>
        /// <returns>
        /// The computed ACL that also includes the inherited ACEs.
        /// </returns>
        public static _ACL ComputeInheritedACLfromCreator(
            _ACL acl,
            bool isContainerObject,
            Guid[] objectTypes)
        {

            //Initialize ExplicitACL to Empty ACL
            //FOR each ACE in ACL DO
            //IF((ACE.Flags contains CONTAINER_INHERIT_ACE) AND
            //(IsContainerObject = TRUE))OR
            //((ACE.Flags contains OBJECT_INHERIT_ACE) AND
            //(IsContainerObject = FALSE))
            //THEN
            //    CASE ACE.Type OF
            //    ALLOW:
            //    DENY:
            //    Set NewACE to ACE
            //    Set NewACE.Flags to NULL
            //    Append NewACE to ExplicitACL
            //    OBJECT_ALLOW
            //    OBJECT_DENY:
            //    IF (ObjectTypes contains ACE.ObjectGUID) THEN
            //    Set NewACE to ACE
            //    Set NewACE.Flags to NULL
            //    Append NewACE to ExplicitACL
            //    ENDIF
            //    ENDCASE
            //ENDIF
            //END FOR
            //Initialize InheritableACL to Empty ACL
            //IF (IsContainerObject = TRUE) THEN
            //    FOR each ACE in ACL DO
            //    IF((ACE.Flags contains CONTAINER_INHERIT_ACE) OR
            //        (ACE.Flags contains OBJECT_INHERIT_ACE))
            //    THEN
            //    Set NewACE to ACE
            //    Add INHERIT_ONLY_ACE to NewACE.Flags
            //    Append NewACE to InheritableACL
            //    ENDIF
            //    END FOR
            //ENDIF
            //RETURN concatenation of ExplicitACL and InheritableACL
            _ACL newAcl = new _ACL();
            newAcl.AclRevision = acl.AclRevision;
            List<object> newAces = new List<object>();

            #region Computes the explicit Acl
            if (acl.Aces != null)
            {
                foreach (object ace in acl.Aces)
                {
                    object newAce = ObjectUtility.DeepClone(ace);
                    _ACE_HEADER header = (_ACE_HEADER)ObjectUtility.GetFieldValue(newAce, "Header");
                    ACCESS_OBJECT_ACE_Flags flags = ACCESS_OBJECT_ACE_Flags.None;
                    ACE_TYPE aceType = header.AceType;

                    if (header.AceType == ACE_TYPE.ACCESS_ALLOWED_OBJECT_ACE_TYPE
                        || header.AceType == ACE_TYPE.ACCESS_DENIED_OBJECT_ACE_TYPE)
                    {
                        flags = (ACCESS_OBJECT_ACE_Flags)ObjectUtility.GetFieldValue(newAce, "Flags");
                        if ((flags & ACCESS_OBJECT_ACE_Flags.ACE_OBJECT_TYPE_PRESENT)
                                         != ACCESS_OBJECT_ACE_Flags.ACE_OBJECT_TYPE_PRESENT)
                        {
                            aceType = (header.AceType == ACE_TYPE.ACCESS_ALLOWED_OBJECT_ACE_TYPE)
                                ? ACE_TYPE.ACCESS_ALLOWED_ACE_TYPE
                                : ACE_TYPE.ACCESS_DENIED_ACE_TYPE;
                        }
                    }

                    if (((header.AceFlags & ACE_FLAGS.CONTAINER_INHERIT_ACE) != 0 && isContainerObject)
                        || ((header.AceFlags & ACE_FLAGS.OBJECT_INHERIT_ACE) != 0 && (!isContainerObject)))
                    {
                        bool retVal = DtypUtility.ComputeInheritedAcefromCreator(ref newAce, objectTypes);
                        if (retVal)
                        {
                            newAces.Add(newAce);
                        }
                    }
                }
            }
            #endregion

            #region Compute the inheritable ACL
            if (isContainerObject)
            {
                if (acl.Aces != null)
                {
                    foreach (object ace in acl.Aces)
                    {
                        object newAce = ObjectUtility.DeepClone(ace);
                        _ACE_HEADER header = (_ACE_HEADER)ObjectUtility.GetFieldValue(newAce, "Header");

                        if ((header.AceFlags & ACE_FLAGS.CONTAINER_INHERIT_ACE) == ACE_FLAGS.CONTAINER_INHERIT_ACE
                            || (header.AceFlags & ACE_FLAGS.OBJECT_INHERIT_ACE) == ACE_FLAGS.OBJECT_INHERIT_ACE)
                        {
                            header.AceFlags |= ACE_FLAGS.INHERIT_ONLY_ACE;
                            ObjectUtility.SetFieldValue(newAce, "Header", header);
                            //Append NewACE to NewACL
                            newAces.Add(newAce);
                        }
                    }
                }
            }
            #endregion

            newAcl.Aces = newAces.ToArray();
            newAcl.AceCount = (ushort)newAcl.Aces.Length;
            newAcl.AclSize = DtypUtility.CalculateAclSize(newAcl);

            return newAcl;
        }

        /// <summary>
        /// PreProcessACLfromCreator. MS-DTYP section 2.5.4.8
        /// </summary>
        /// <param name="acl">
        /// ACL to preprocess.
        /// </param>
        /// <returns>
        /// Processed ACL.
        /// </returns>
        public static _ACL PreProcessACLfromCreator(_ACL acl)
        {
            //Initialize NewACL to Empty ACL
            //FOR each ACE in ACL DO
            //IF ACE.Flags does not contain INHERITED_ACE THEN
            //Append ACE to NewACL
            //ENDIF
            //END FOR
            //RETURN NewACL
            _ACL newAcl = new _ACL();
            newAcl.AclRevision = acl.AclRevision;
            List<object> newAces = new List<object>();

            if (acl.Aces != null)
            {
                foreach (object ace in acl.Aces)
                {
                    _ACE_HEADER header = (_ACE_HEADER)ObjectUtility.GetFieldValue(ace, "Header");
                    if ((header.AceFlags & ACE_FLAGS.INHERITED_ACE) == ACE_FLAGS.None)
                    {
                        //Append ACE to NewACL
                        newAces.Add(ace);
                    }
                }
            }
            newAcl.Aces = newAces.ToArray();
            newAcl.AceCount = (ushort)newAcl.Aces.Length;
            newAcl.AclSize = DtypUtility.CalculateAclSize(newAcl);

            return newAcl;
        }

        /// <summary>
        /// PostProcessACL. MS-DTYP section 2.5.4.9
        /// </summary>
        /// <param name="acl">ACL on which to substitute SIDs.</param>
        /// <param name="copyFilter">The filter for post-processing the ACL.</param>
        /// <param name="owner">Owner to use in substituting the CreatorOwner SID.</param>
        /// <param name="group">Group to use in substituting the CreatorGroup SID.</param>
        /// <param name="genericMapping">
        /// Mapping of generic permissions to resource manager-specific 
        /// permissions supplied by the caller.
        /// </param>
        /// <returns>
        /// The computed ACL with the SID substitutions performed.
        /// </returns>
        public static _ACL PostProcessACL(
            _ACL acl,
            CopyFilter copyFilter,
            _SID owner,
            _SID group,
            GenericMapping genericMapping)
        {
            #region Pseudocode  v20110329

            //// Substitute CreatorOwner and CreatorGroup SIDs and do GenericMapping in ACL

            //Initialize NewACL to Empty ACL

            //FOR each ACE in ACL DO

            //    // Determine if this ACE passes the filter to be copied to the new ACL

            //    SET CopyThisAce = FALSE 

            //    CASE CopyFilter OF 

            //        CopyAllAces: 
            //            BEGIN 
            //                SET CopyThisAce = TRUE 
            //            END

            //        CopyInheritedAces: 
            //            BEGIN 
            //                IF (ACE.AceFlags contains INHERITED_ACE) THEN 
            //                    SET CopyThisAce = TRUE 
            //                ENDIF 
            //            END

            //        CopyExplicitAces: 
            //            BEGIN 
            //                IF (ACE.AceFlags does not contain INHERITED_ACE) THEN 
            //                   SET CopyThisAce = TRUE 
            //                ENDIF 
            //            END

            //    ENDCASE

            //    Set NewACE to ACE

            //    IF (CopyThisAce) THEN

            //        CASE ACE.Sid OF

            //            CREATOR_OWNER:
            //                NewACE.Sid = Owner

            //            CREATOR_GROUP:
            //                NewACE.Sid = Group
            //        ENDCASE

            //        IF (ACE.Mask contains GENERIC_READ) THEN
            //            Add GenericMapping.GenericRead to NewACE.Mask
            //        ENDIF

            //        IF (ACE.Mask contains GENERIC_WRITE) THEN
            //            Add GenericMapping.GenericWrite to NewACE.Mask
            //        ENDIF 

            //        IF (ACE.Mask contains GENERIC_EXECUTE) THEN
            //            Add GenericMapping.GenericExecute to NewACE.Mask
            //        ENDIF

            //        Append NewACE to NewACL
            //    ENDIF

            //END FOR

            //RETURN NewACL
            //// END PostProcessACL

            #endregion

            _ACL newAcl = new _ACL();
            newAcl.AclRevision = acl.AclRevision;
            newAcl.Aces = new object[] { };
            List<object> newAces = new List<object>();

            if (acl.Aces != null)
            {
                foreach (object ace in acl.Aces)
                {
                    object newAce = ObjectUtility.DeepClone(ace);
                    _ACE_HEADER header = (_ACE_HEADER)ObjectUtility.GetFieldValue(newAce, "Header");

                    bool copyThisAce = false;
                    switch (copyFilter)
                    {
                        case CopyFilter.CopyAllAces:
                            copyThisAce = true;
                            break;
                        case CopyFilter.CopyInheritedAces:
                            if ((header.AceFlags & ACE_FLAGS.INHERITED_ACE) == ACE_FLAGS.INHERITED_ACE)
                            {
                                copyThisAce = true;
                            }
                            break;
                        case CopyFilter.CopyExplicitAces:
                            if ((header.AceFlags & ACE_FLAGS.INHERITED_ACE) == ACE_FLAGS.None)
                            {
                                copyThisAce = true;
                            }
                            break;
                    }

                    if (copyThisAce)
                    {
                        #region Substitute CreatorOwner and CreatorGroup SIDs in ACL

                        _SID sid = (_SID)ObjectUtility.GetFieldValue(newAce, "Sid");
                        if (ObjectUtility.DeepCompare(sid, GetWellKnownSid(WellKnownSid.CREATOR_OWNER, null)))
                        {
                            ObjectUtility.SetFieldValue(newAce, "Sid", owner);
                            header.AceSize = (ushort)(header.AceSize
                                - (sid.SubAuthorityCount - owner.SubAuthorityCount) * sizeof(uint));
                            ObjectUtility.SetFieldValue(newAce, "Header", header);

                        }
                        else if (ObjectUtility.DeepCompare(sid, GetWellKnownSid(WellKnownSid.CREATOR_GROUP, null)))
                        {
                            ObjectUtility.SetFieldValue(newAce, "Sid", group);
                            header.AceSize = (ushort)(header.AceSize
                                - (sid.SubAuthorityCount - group.SubAuthorityCount) * sizeof(uint));
                            ObjectUtility.SetFieldValue(newAce, "Header", header);
                        }
                        #endregion

                        #region Do GenericMapping in ACL

                        uint mask = (uint)ObjectUtility.GetFieldValue(newAce, "Mask");

                        if ((mask & ACCESS_MASK_GENERIC_READ) == ACCESS_MASK_GENERIC_READ)
                        {
                            mask = mask | genericMapping.GenericRead;
                            ObjectUtility.SetFieldValue(newAce, "Mask", mask);
                        }
                        if ((mask & ACCESS_MASK_GENERIC_WRITE) == ACCESS_MASK_GENERIC_WRITE)
                        {
                            mask = mask | genericMapping.GenericWrite;
                            ObjectUtility.SetFieldValue(newAce, "Mask", mask);
                        }
                        if ((mask & ACCESS_MASK_GENERIC_EXECUTE) == ACCESS_MASK_GENERIC_EXECUTE)
                        {
                            mask = mask | genericMapping.GenericExecute;
                            ObjectUtility.SetFieldValue(newAce, "Mask", mask);
                        }
                        #endregion

                        //Append NewACE to NewACL
                        newAces.Add(newAce);
                    }
                }
            }
            newAcl.Aces = newAces.ToArray();
            newAcl.AceCount = (ushort)newAcl.Aces.Length;
            newAcl.AclSize = DtypUtility.CalculateAclSize(newAcl);

            return newAcl;
        }

        /// <summary>
        /// Calculate the length of the ApplicationData byte array according to the specified ace header.
        /// </summary>
        /// <param name="header">The specified ace header.</param>
        /// <param name="sid">The sid of the specified ace.</param>
        /// <returns>The calculated length.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throw when the ace type is invalid.</exception> 
        internal static int CalculateApplicationDataLength(_ACE_HEADER header, _SID sid)
        {
            int applicationDataLength = 0;
            switch (header.AceType)
            {
                case ACE_TYPE.ACCESS_ALLOWED_CALLBACK_ACE_TYPE:
                case ACE_TYPE.ACCESS_DENIED_CALLBACK_ACE_TYPE:
                case ACE_TYPE.SYSTEM_AUDIT_CALLBACK_ACE_TYPE:
                    //The AceSize contains the following parts:
                    //_ACE_HEADER Header,(4 bytes)
                    //uint Mask,(4 bytes)
                    //_SID Sid;(variable length)
                    //byte[] ApplicationData,(variable length)
                    applicationDataLength =
                        header.AceSize - SHORT_FIXED_ACE_LENGTH - TypeMarshal.ToBytes<_SID>(sid).GetLength(0);
                    break;

                case ACE_TYPE.ACCESS_ALLOWED_CALLBACK_OBJECT_ACE_TYPE:
                case ACE_TYPE.ACCESS_DENIED_CALLBACK_OBJECT_ACE_TYPE:
                case ACE_TYPE.SYSTEM_AUDIT_CALLBACK_OBJECT_ACE_TYPE:
                    //The AceSize contains the following parts:
                    //_ACE_HEADER Header,(4 bytes)
                    //uint Mask,(4 bytes)
                    //uint Flags,(4 bytes)
                    //Guid ObjectType,(16 bytes)
                    //Guid InheritedObjectType,(16 bytes)
                    //_SID Sid;(variable length)
                    //byte[] ApplicationData,(variable length)
                    applicationDataLength =
                        header.AceSize - LONG_FIXED_ACE_LENGTH - TypeMarshal.ToBytes<_SID>(sid).GetLength(0);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("header", "The ace type is invalid.");
            }
            return applicationDataLength > 0 ? applicationDataLength : 0;
        }

        /// <summary>
        /// Calculate AclSize of the given ACL, in bytes.
        /// </summary>
        /// <param name="acl">The acl to be retrieved.</param>
        /// <returns>The size of the given acl, in bytes.</returns>
        internal static ushort CalculateAclSize(_ACL acl)
        {
            //The AclSize contains the following parts:
            //byte AclRevision;(1 byte)
            //byte Sbz1;(1 byte)
            //ushort AclSize;(2 bytes)
            //ushort AceCount;(2 bytes)
            //ushort Sbz2;(2 bytes)
            //object[] Aces;(variable length)
            ushort sizeOfAcl = ACL_HEADER_LENGTH;
            for (int i = 0; i < acl.AceCount; i++)
            {
                _ACE_HEADER header = (_ACE_HEADER)ObjectUtility.GetFieldValue(acl.Aces[i], "Header");
                sizeOfAcl += header.AceSize;
            }
            return sizeOfAcl;
        }

        /// <summary>
        /// Calculate AceSize of the given ACE, in bytes.
        /// </summary>
        /// <param name="ace">The ace to be retrieved.</param>
        /// <returns>The size of the given ace, in bytes.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throw when the ace type is invalid.</exception> 
        internal static ushort CalculateAceSize(object ace)
        {
            BindingFlags InvokeFlattenHierarchyMemberFlags = BindingFlags.Instance
                | BindingFlags.Public | BindingFlags.FlattenHierarchy;
            FieldInfo headField = ace.GetType().GetField("Header", InvokeFlattenHierarchyMemberFlags);
            _ACE_HEADER header = (_ACE_HEADER)headField.GetValue(ace);

            FieldInfo sidField = ace.GetType().GetField("Sid", InvokeFlattenHierarchyMemberFlags);
            _SID sid = (_SID)sidField.GetValue(ace);
            int sizeOfSid = TypeMarshal.ToBytes<_SID>(sid).Length;

            FieldInfo applicationDataField;
            byte[] applicationData;

            int sizeOfAce = 0;
            switch (header.AceType)
            {
                case ACE_TYPE.ACCESS_ALLOWED_ACE_TYPE:
                case ACE_TYPE.ACCESS_DENIED_ACE_TYPE:
                case ACE_TYPE.SYSTEM_AUDIT_ACE_TYPE:
                case ACE_TYPE.SYSTEM_MANDATORY_LABLE_ACE_TYPE:
                    //The AceSize contains the following parts:
                    //_ACE_HEADER Header; (4 bytes)
                    //uint Mask; (4 bytes)
                    //_SID Sid; (variable length)
                    sizeOfAce = sizeOfSid + SHORT_FIXED_ACE_LENGTH;
                    break;

                case ACE_TYPE.ACCESS_ALLOWED_OBJECT_ACE_TYPE:
                case ACE_TYPE.ACCESS_DENIED_OBJECT_ACE_TYPE:
                case ACE_TYPE.SYSTEM_AUDIT_OBJECT_ACE_TYPE:
                    //The AceSize contains the following parts:
                    //_ACE_HEADER Header; (4 bytes)
                    //ACCESS_OBJECT_ACE_Mask Mask; (4 bytes)
                    //ACCESS_OBJECT_ACE_Flags Flags;(4 bytes)
                    //Guid ObjectType;(16 bytes)
                    //Guid InheritedObjectType;(16 bytes)
                    //_SID Sid; (variable length)
                    sizeOfAce = sizeOfSid + LONG_FIXED_ACE_LENGTH;
                    break;

                case ACE_TYPE.ACCESS_ALLOWED_CALLBACK_ACE_TYPE:
                case ACE_TYPE.ACCESS_DENIED_CALLBACK_ACE_TYPE:
                case ACE_TYPE.SYSTEM_AUDIT_CALLBACK_ACE_TYPE:
                    //The AceSize contains the following parts:
                    //_ACE_HEADER Header; (4 bytes)
                    //uint Mask; (4 bytes)
                    //_SID Sid; (variable length)
                    //byte[] ApplicationData;(variable length)
                    applicationDataField =
                        ace.GetType().GetField("ApplicationData", InvokeFlattenHierarchyMemberFlags);
                    applicationData = (byte[])applicationDataField.GetValue(ace);
                    sizeOfAce = applicationData.Length + sizeOfSid + SHORT_FIXED_ACE_LENGTH;
                    break;

                case ACE_TYPE.ACCESS_ALLOWED_CALLBACK_OBJECT_ACE_TYPE:
                case ACE_TYPE.ACCESS_DENIED_CALLBACK_OBJECT_ACE_TYPE:
                case ACE_TYPE.SYSTEM_AUDIT_CALLBACK_OBJECT_ACE_TYPE:
                    //The AceSize contains the following parts:
                    //_ACE_HEADER Header; (4 bytes)
                    //ACCESS_OBJECT_ACE_Mask Mask; (4 bytes)
                    //ACCESS_OBJECT_ACE_Flags Flags;(4 bytes)
                    //Guid ObjectType;(16 bytes)
                    //Guid InheritedObjectType;(16 bytes)
                    //_SID Sid; (variable length)
                    //byte[] ApplicationData;(variable length)
                    applicationDataField =
                        ace.GetType().GetField("ApplicationData", InvokeFlattenHierarchyMemberFlags);
                    applicationData = (byte[])applicationDataField.GetValue(ace);
                    sizeOfAce = applicationData.Length + sizeOfSid + LONG_FIXED_ACE_LENGTH;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("ace", "The ace type is invalid.");
            }
            return (ushort)sizeOfAce;
        }

        /// <summary>
        /// Encode the ACL structure into byte array, according to TD specification.
        /// </summary>
        /// <param name="acl">The acl to be retrieved.</param>
        /// <returns>The encoded byte array.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when The type of ace is invalid.
        /// </exception>
        public static byte[] EncodeAcl(_ACL acl)
        {
            List<byte> byteArray = new List<byte>();
            byteArray.Add(acl.AclRevision);
            byteArray.Add(acl.Sbz1);
            byteArray.AddRange(TypeMarshal.ToBytes<ushort>(acl.AclSize));
            byteArray.AddRange(TypeMarshal.ToBytes<ushort>(acl.AceCount));
            byteArray.AddRange(TypeMarshal.ToBytes<ushort>(acl.Sbz2));

            for (int i = 0; i < acl.AceCount; i++)
            {
                _ACE_HEADER header = (_ACE_HEADER)ObjectUtility.GetFieldValue(acl.Aces[i], "Header");
                byte[] tmpByteArray = new byte[] { };
                switch (header.AceType)
                {
                    case ACE_TYPE.ACCESS_ALLOWED_ACE_TYPE:
                        tmpByteArray = TypeMarshal.ToBytes<_ACCESS_ALLOWED_ACE>((_ACCESS_ALLOWED_ACE)acl.Aces[i]);
                        break;

                    case ACE_TYPE.ACCESS_DENIED_ACE_TYPE:
                        tmpByteArray = TypeMarshal.ToBytes<_ACCESS_DENIED_ACE>((_ACCESS_DENIED_ACE)acl.Aces[i]);
                        break;

                    case ACE_TYPE.SYSTEM_AUDIT_ACE_TYPE:
                        tmpByteArray = TypeMarshal.ToBytes<_SYSTEM_AUDIT_ACE>((_SYSTEM_AUDIT_ACE)acl.Aces[i]);
                        break;

                    case ACE_TYPE.ACCESS_ALLOWED_OBJECT_ACE_TYPE:
                        tmpByteArray = TypeMarshal.ToBytes<_ACCESS_ALLOWED_OBJECT_ACE>(
                            (_ACCESS_ALLOWED_OBJECT_ACE)acl.Aces[i]);
                        break;

                    case ACE_TYPE.ACCESS_DENIED_OBJECT_ACE_TYPE:
                        tmpByteArray = TypeMarshal.ToBytes<_ACCESS_DENIED_OBJECT_ACE>(
                            (_ACCESS_DENIED_OBJECT_ACE)acl.Aces[i]);
                        break;

                    case ACE_TYPE.SYSTEM_AUDIT_OBJECT_ACE_TYPE:
                        tmpByteArray = TypeMarshal.ToBytes<_SYSTEM_AUDIT_OBJECT_ACE>(
                            (_SYSTEM_AUDIT_OBJECT_ACE)acl.Aces[i]);
                        break;

                    case ACE_TYPE.ACCESS_ALLOWED_CALLBACK_ACE_TYPE:
                        tmpByteArray = TypeMarshal.ToBytes<_ACCESS_ALLOWED_CALLBACK_ACE>(
                            (_ACCESS_ALLOWED_CALLBACK_ACE)acl.Aces[i]);
                        break;

                    case ACE_TYPE.ACCESS_DENIED_CALLBACK_ACE_TYPE:
                        tmpByteArray = TypeMarshal.ToBytes<_ACCESS_DENIED_CALLBACK_ACE>(
                            (_ACCESS_DENIED_CALLBACK_ACE)acl.Aces[i]);
                        break;

                    case ACE_TYPE.ACCESS_ALLOWED_CALLBACK_OBJECT_ACE_TYPE:
                        tmpByteArray = TypeMarshal.ToBytes<_ACCESS_ALLOWED_CALLBACK_OBJECT_ACE>(
                            (_ACCESS_ALLOWED_CALLBACK_OBJECT_ACE)acl.Aces[i]);
                        break;

                    case ACE_TYPE.ACCESS_DENIED_CALLBACK_OBJECT_ACE_TYPE:
                        tmpByteArray = TypeMarshal.ToBytes<_ACCESS_DENIED_CALLBACK_OBJECT_ACE>(
                            (_ACCESS_DENIED_CALLBACK_OBJECT_ACE)acl.Aces[i]);
                        break;

                    case ACE_TYPE.SYSTEM_AUDIT_CALLBACK_ACE_TYPE:
                        tmpByteArray = TypeMarshal.ToBytes<_SYSTEM_AUDIT_CALLBACK_ACE>(
                            (_SYSTEM_AUDIT_CALLBACK_ACE)acl.Aces[i]);
                        break;

                    case ACE_TYPE.SYSTEM_AUDIT_CALLBACK_OBJECT_ACE_TYPE:
                        tmpByteArray = TypeMarshal.ToBytes<_SYSTEM_AUDIT_CALLBACK_OBJECT_ACE>(
                            (_SYSTEM_AUDIT_CALLBACK_OBJECT_ACE)acl.Aces[i]);
                        break;

                    case ACE_TYPE.SYSTEM_MANDATORY_LABLE_ACE_TYPE:
                        tmpByteArray = TypeMarshal.ToBytes<_SYSTEM_MANDATORY_LABEL_ACE>(
                            (_SYSTEM_MANDATORY_LABEL_ACE)acl.Aces[i]);
                        break;

                    case ACE_TYPE.SYSTEM_RESOURCE_ATTRIBUTE_ACE_TYPE:
                        tmpByteArray = ((SystemResourceAttributeAce)acl.Aces[i]).BinaryForm;
                        break;

                    case ACE_TYPE.SYSTEM_SCOPED_POLICY_ID_ACE_TYPE:
                        tmpByteArray = TypeMarshal.ToBytes<_SYSTEM_SCOPED_POLICY_ID_ACE>(
                            (_SYSTEM_SCOPED_POLICY_ID_ACE)acl.Aces[i]);
                        break;

                    default:
                        throw new ArgumentException("The type of ace is invalid", "acl");
                }
                byteArray.AddRange(tmpByteArray);
            }
            return byteArray.ToArray();
        }

        /// <summary>
        /// Decode the specified buffer into the ACL structure.
        /// </summary>
        /// <param name="buffer">The byte array to be decoded.</param>
        /// <returns>The ACL structure.</returns>
        /// <exception cref="FormatException">
        /// Thrown when the format of input parameter is not correct.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when The type of ace is invalid.
        /// </exception>
        public static _ACL DecodeAcl(byte[] buffer)
        {
            if (buffer == null || buffer.Length < ACL_HEADER_LENGTH)
            {
                throw new FormatException("The token body is incomplete.");
            }

            _ACL acl = new _ACL();
            List<object> aces = new List<object>();
            int index = 0;
            byte[] tokenBody = new byte[] { };

            acl.AclRevision = buffer[0];
            acl.Sbz1 = buffer[1];

            index += sizeof(byte) * 2;
            tokenBody = ArrayUtility.SubArray(buffer, index, sizeof(ushort));
            acl.AclSize = TypeMarshal.ToStruct<ushort>(tokenBody);

            index += sizeof(ushort);
            tokenBody = ArrayUtility.SubArray(buffer, index, sizeof(ushort));
            acl.AceCount = TypeMarshal.ToStruct<ushort>(tokenBody);

            index += sizeof(ushort);
            tokenBody = ArrayUtility.SubArray(buffer, index, sizeof(ushort));
            acl.Sbz2 = TypeMarshal.ToStruct<ushort>(tokenBody);

            index += sizeof(ushort);
            for (int i = 0; i < acl.AceCount; i++)
            {
                tokenBody = ArrayUtility.SubArray(buffer, index, ACE_HEADER_LENGTH);
                _ACE_HEADER header = TypeMarshal.ToStruct<_ACE_HEADER>(tokenBody);
                object ace = null;
                tokenBody = ArrayUtility.SubArray(buffer, index, header.AceSize);
                switch (header.AceType)
                {
                    case ACE_TYPE.ACCESS_ALLOWED_ACE_TYPE:
                        ace = TypeMarshal.ToStruct<_ACCESS_ALLOWED_ACE>(tokenBody);
                        break;

                    case ACE_TYPE.ACCESS_DENIED_ACE_TYPE:
                        ace = TypeMarshal.ToStruct<_ACCESS_DENIED_ACE>(tokenBody);
                        break;

                    case ACE_TYPE.SYSTEM_AUDIT_ACE_TYPE:
                        ace = TypeMarshal.ToStruct<_SYSTEM_AUDIT_ACE>(tokenBody);
                        break;

                    case ACE_TYPE.ACCESS_ALLOWED_OBJECT_ACE_TYPE:
                        ace = TypeMarshal.ToStruct<_ACCESS_ALLOWED_OBJECT_ACE>(tokenBody);
                        break;

                    case ACE_TYPE.ACCESS_DENIED_OBJECT_ACE_TYPE:
                        ace = TypeMarshal.ToStruct<_ACCESS_DENIED_OBJECT_ACE>(tokenBody);
                        break;

                    case ACE_TYPE.SYSTEM_AUDIT_OBJECT_ACE_TYPE:
                        ace = TypeMarshal.ToStruct<_SYSTEM_AUDIT_OBJECT_ACE>(tokenBody);
                        break;

                    case ACE_TYPE.ACCESS_ALLOWED_CALLBACK_ACE_TYPE:
                        ace = TypeMarshal.ToStruct<_ACCESS_ALLOWED_CALLBACK_ACE>(tokenBody);
                        break;

                    case ACE_TYPE.ACCESS_DENIED_CALLBACK_ACE_TYPE:
                        ace = TypeMarshal.ToStruct<_ACCESS_DENIED_CALLBACK_ACE>(tokenBody);
                        break;

                    case ACE_TYPE.ACCESS_ALLOWED_CALLBACK_OBJECT_ACE_TYPE:
                        ace = TypeMarshal.ToStruct<_ACCESS_ALLOWED_CALLBACK_OBJECT_ACE>(tokenBody);
                        break;

                    case ACE_TYPE.ACCESS_DENIED_CALLBACK_OBJECT_ACE_TYPE:
                        ace = TypeMarshal.ToStruct<_ACCESS_DENIED_CALLBACK_OBJECT_ACE>(tokenBody);
                        break;

                    case ACE_TYPE.SYSTEM_AUDIT_CALLBACK_ACE_TYPE:
                        ace = TypeMarshal.ToStruct<_SYSTEM_AUDIT_CALLBACK_ACE>(tokenBody);
                        break;

                    case ACE_TYPE.SYSTEM_AUDIT_CALLBACK_OBJECT_ACE_TYPE:
                        ace = TypeMarshal.ToStruct<_SYSTEM_AUDIT_CALLBACK_OBJECT_ACE>(tokenBody);
                        break;

                    case ACE_TYPE.SYSTEM_MANDATORY_LABLE_ACE_TYPE:
                        ace = TypeMarshal.ToStruct<_SYSTEM_MANDATORY_LABEL_ACE>(tokenBody);
                        break;

                    case ACE_TYPE.SYSTEM_RESOURCE_ATTRIBUTE_ACE_TYPE:
                        ace = SystemResourceAttributeAce.FromBytes(tokenBody);
                        break;

                    case ACE_TYPE.SYSTEM_SCOPED_POLICY_ID_ACE_TYPE:
                        ace = TypeMarshal.ToStruct<_SYSTEM_SCOPED_POLICY_ID_ACE>(tokenBody);
                        break;

                    default:
                        throw new ArgumentException("The type of ace is invalid.", "buffer");
                }
                index += header.AceSize;
                aces.Add(ace);
            }

            acl.Aces = aces.ToArray();
            return acl;
        }

        /// <summary>
        /// Update the Offset* fields of the input _SECURITY_DESCRIPTOR structure. 
        /// </summary>
        /// <param name="securityDescriptor">The security descriptor to be updated.</param>
        /// <returns>The encoded byte array.</returns>
        public static void UpdateSecurityDescriptor(ref _SECURITY_DESCRIPTOR sd)
        {
            // Revision (1 byte) + Sbz1 (1 byte) + Control (2 bytes) + OffsetOwner (4 bytes) + OffsetGroup (4 bytes) + 
            // OffsetSacl (4 bytes) + OffsetDacl (4 bytes)
            int offset = 20;
            if (sd.OwnerSid != null)
            {
                sd.OffsetOwner = (uint)offset;
                offset += TypeMarshal.GetBlockMemorySize<_SID>(sd.OwnerSid.Value);
            }
            else
            {
                sd.OffsetOwner = 0;
            }
            if (sd.GroupSid != null)
            {
                sd.OffsetGroup = (uint)offset;
                offset += TypeMarshal.GetBlockMemorySize<_SID>(sd.GroupSid.Value);
            }
            else
            {
                sd.OffsetGroup = 0;
            }
            if (sd.Sacl != null)
            {
                sd.OffsetSacl = (uint)offset;
                offset += DtypUtility.EncodeAcl(sd.Sacl.Value).Length;
            }
            else
            {
                sd.OffsetSacl = 0;
            }
            if (sd.Dacl != null)
            {
                sd.OffsetDacl = (uint)offset;
                offset += DtypUtility.EncodeAcl(sd.Dacl.Value).Length;
            }
            else
            {
                sd.OffsetDacl = 0;
            }
        }


        /// <summary>
        /// Encode the _SECURITY_DESCRIPTOR structure into byte array, according to TD specification.
        /// </summary>
        /// <param name="securityDescriptor">The security descriptor to be retrieved.</param>
        /// <returns>The encoded byte array.</returns>
        public static byte[] EncodeSecurityDescriptor(_SECURITY_DESCRIPTOR securityDescriptor)
        {
            if ((securityDescriptor.Control & SECURITY_DESCRIPTOR_Control.SelfRelative) == 0)
            {
                securityDescriptor = DtypUtility.CreateSecurityDescriptor(
                    securityDescriptor.Control,
                    securityDescriptor.OwnerSid,
                    securityDescriptor.GroupSid,
                    securityDescriptor.Sacl,
                    securityDescriptor.Dacl);
            }

            List<byte> byteArray = new List<byte>();
            byteArray.Add(securityDescriptor.Revision);
            byteArray.Add(securityDescriptor.Sbz1);
            byteArray.AddRange(TypeMarshal.ToBytes<SECURITY_DESCRIPTOR_Control>(securityDescriptor.Control));
            byteArray.AddRange(TypeMarshal.ToBytes<uint>(securityDescriptor.OffsetOwner));
            byteArray.AddRange(TypeMarshal.ToBytes<uint>(securityDescriptor.OffsetGroup));
            byteArray.AddRange(TypeMarshal.ToBytes<uint>(securityDescriptor.OffsetSacl));
            byteArray.AddRange(TypeMarshal.ToBytes<uint>(securityDescriptor.OffsetDacl));
            if (securityDescriptor.OwnerSid != null)
            {
                byteArray.AddRange(TypeMarshal.ToBytes<_SID>(securityDescriptor.OwnerSid.Value));
            }
            if (securityDescriptor.GroupSid != null)
            {
                byteArray.AddRange(TypeMarshal.ToBytes<_SID>(securityDescriptor.GroupSid.Value));
            }
            if (securityDescriptor.Sacl != null)
            {
                byteArray.AddRange(DtypUtility.EncodeAcl(securityDescriptor.Sacl.Value));
            }
            if (securityDescriptor.Dacl != null)
            {
                byteArray.AddRange(DtypUtility.EncodeAcl(securityDescriptor.Dacl.Value));
            }
            return byteArray.ToArray();
        }

        /// <summary>
        /// Decode the specified buffer into the _SECURITY_DESCRIPTOR structure.
        /// </summary>
        /// <param name="buffer">The byte array to be decoded.</param>
        /// <returns>The _SECURITY_DESCRIPTOR structure.</returns>
        public static _SECURITY_DESCRIPTOR DecodeSecurityDescriptor(byte[] buffer)
        {
            int offset = 0;

            _SECURITY_DESCRIPTOR securityDescriptor = new _SECURITY_DESCRIPTOR();
            securityDescriptor.Revision = buffer[offset];
            offset += sizeof(byte);
            securityDescriptor.Sbz1 = buffer[offset];
            offset += sizeof(byte);
            securityDescriptor.Control = TypeMarshal.ToStruct<SECURITY_DESCRIPTOR_Control>(
                ArrayUtility.SubArray(buffer, offset, sizeof(SECURITY_DESCRIPTOR_Control)));
            offset += sizeof(ushort);
            securityDescriptor.OffsetOwner = TypeMarshal.ToStruct<uint>(
                ArrayUtility.SubArray(buffer, offset, sizeof(uint)));
            offset += sizeof(uint);
            securityDescriptor.OffsetGroup = TypeMarshal.ToStruct<uint>(
                ArrayUtility.SubArray(buffer, offset, sizeof(uint)));
            offset += sizeof(uint);
            securityDescriptor.OffsetSacl = TypeMarshal.ToStruct<uint>(
                ArrayUtility.SubArray(buffer, offset, sizeof(uint)));
            offset += sizeof(uint);
            securityDescriptor.OffsetDacl = TypeMarshal.ToStruct<uint>(
                ArrayUtility.SubArray(buffer, offset, sizeof(uint)));
            offset += sizeof(uint);

            if (securityDescriptor.OffsetOwner != 0)
            {
                byte subAuthorityCount = buffer[securityDescriptor.OffsetOwner + sizeof(byte)];
                //Size of Revision,SubAuthorityCount, IdentifierAuthority and SubAuthority. 
                int sizeOfSid = sizeof(byte) + sizeof(byte) + sizeof(byte) * 6 + sizeof(uint) * subAuthorityCount;
                securityDescriptor.OwnerSid = TypeMarshal.ToStruct<_SID>(
                    ArrayUtility.SubArray(buffer, (int)securityDescriptor.OffsetOwner, sizeOfSid));
            }
            if (securityDescriptor.OffsetGroup != 0)
            {
                byte subAuthorityCount = buffer[securityDescriptor.OffsetGroup + sizeof(byte)];
                //Size of Revision,SubAuthorityCount, IdentifierAuthority and SubAuthority. 
                int sizeOfSid = sizeof(byte) + sizeof(byte) + sizeof(byte) * 6 + sizeof(uint) * subAuthorityCount;
                securityDescriptor.GroupSid = TypeMarshal.ToStruct<_SID>(
                    ArrayUtility.SubArray(buffer, (int)securityDescriptor.OffsetGroup, sizeOfSid));
            }
            if (securityDescriptor.OffsetSacl != 0)
            {
                //Add offset of AclRevision and Sbz1.
                ushort aclSize = TypeMarshal.ToStruct<ushort>(
                    ArrayUtility.SubArray(buffer, (int)(securityDescriptor.OffsetSacl + sizeof(byte) + sizeof(byte)), sizeof(ushort)));
                securityDescriptor.Sacl = DtypUtility.DecodeAcl(
                    ArrayUtility.SubArray(buffer, (int)securityDescriptor.OffsetSacl, aclSize));
            }
            if (securityDescriptor.OffsetDacl != 0)
            {
                //Add offset of AclRevision and Sbz1.
                ushort aclSize = TypeMarshal.ToStruct<ushort>(
                    ArrayUtility.SubArray(buffer, (int)(securityDescriptor.OffsetDacl + sizeof(byte) + sizeof(byte)), sizeof(ushort)));
                securityDescriptor.Dacl = DtypUtility.DecodeAcl(
                    ArrayUtility.SubArray(buffer, (int)securityDescriptor.OffsetDacl, aclSize));
            }

            return securityDescriptor;
        }

        /// <summary>
        /// Get the SID from the user's domain name and account name.
        /// </summary>
        /// <param name="domainName">The name of the domain. </param>
        /// <param name="accountName">The name of the account.</param>
        /// <returns>The SID of the user</returns>
        public static _SID GetSidFromAccount(string domainName, string accountName)
        {
            NTAccount account = new NTAccount(domainName, accountName);
            SecurityIdentifier securityId = (SecurityIdentifier)account.Translate(typeof(SecurityIdentifier));
            byte[] sidBinary = new byte[securityId.BinaryLength];
            securityId.GetBinaryForm(sidBinary, 0);
            _SID sid = TypeMarshal.ToStruct<_SID>(sidBinary);
            return sid;
        }

        /// <summary>
        /// Create an ACCESS_ALLOWED_ACE by using specific SID, access mask and optional ace flags.
        /// </summary>
        /// <param name="sid">The SID of the trustee.</param>
        /// <param name="mask">An ACCESS_MASK that specifies the user rights allowed by this ACE.</param>
        /// <param name="flags">ACE type-specific control flags in the ACE header.</param>
        /// <returns>The constructed ACCESS_ALLOWED_ACE structure</returns>
        public static _ACCESS_ALLOWED_ACE CreateAccessAllowedAce(_SID sid, uint mask, ACE_FLAGS flags = ACE_FLAGS.None)
        {
            _ACE_HEADER aceHeader = new _ACE_HEADER
            {
                AceFlags = flags,
                AceType = ACE_TYPE.ACCESS_ALLOWED_ACE_TYPE,
                // Header (4 bytes) + Mask (4 bytes) + SID length;
                // For details, please refer to MS-DTYP.
                AceSize = (ushort)(4 + 4 + DtypUtility.SidLength(sid)),
            };

            _ACCESS_ALLOWED_ACE ace = new _ACCESS_ALLOWED_ACE
            {
                Header = aceHeader,
                Mask = mask,
                Sid = sid,
            };
            return ace;
        }

        /// <summary>
        /// Create an ACCESS_DENIED_ACE by using specific SID, access mask and optional ace flags.
        /// </summary>
        /// <param name="sid">The SID of the trustee.</param>
        /// <param name="mask">An ACCESS_MASK that specifies the user rights denied by this ACE.</param>
        /// <param name="flags">ACE type-specific control flags in the ACE header.</param>
        /// <returns>The constructed ACCESS_DENIED_ACE structure</returns>
        public static _ACCESS_DENIED_ACE CreateAccessDeniedAce(_SID sid, uint mask, ACE_FLAGS flags = ACE_FLAGS.None)
        {
            _ACE_HEADER aceHeader = new _ACE_HEADER
            {
                AceFlags = flags,
                AceType = ACE_TYPE.ACCESS_DENIED_ACE_TYPE,
                // Header (4 bytes) + Mask (4 bytes) + SID length;
                // For details, please refer to MS-DTYP.
                AceSize = (ushort)(4 + 4 + DtypUtility.SidLength(sid)),
            };

            _ACCESS_DENIED_ACE ace = new _ACCESS_DENIED_ACE
            {
                Header = aceHeader,
                Mask = mask,
                Sid = sid,
            };
            return ace;
        }

        /// <summary>
        /// Create a SYSTEM_RESOURCE_ATTRIBUTE_ACE with specified Attribute Name and Values.
        /// </summary>
        /// <param name="attributeName">Name of the claim security attribute.</param>
        /// <param name="values">Claim security attribute values</param>
        /// <returns>A SYSTEM_RESOURCE_ATTRIBUTE_ACE with specified Attribute Name and Values</returns>
        public static SystemResourceAttributeAce CreateSystemResourceAttributeAce(string attributeName, long[] values)
        {
            return new SystemResourceAttributeAce(attributeName, values);
        }

        /// <summary>
        /// Create a SYSTEM_RESOURCE_ATTRIBUTE_ACE with specified Attribute Name and Values.
        /// </summary>
        /// <param name="attributeName">Name of the claim security attribute.</param>
        /// <param name="values">Claim security attribute values</param>
        /// <returns>A SYSTEM_RESOURCE_ATTRIBUTE_ACE with specified Attribute Name and Values</returns>
        public static SystemResourceAttributeAce CreateSystemResourceAttributeAce(string attributeName, ulong[] values)
        {
            return new SystemResourceAttributeAce(attributeName, values);
        }

        /// <summary>
        /// Create a SYSTEM_RESOURCE_ATTRIBUTE_ACE with specified Attribute Name and Values.
        /// </summary>
        /// <param name="attributeName">Name of the claim security attribute.</param>
        /// <param name="values">Claim security attribute values</param>
        /// <returns>A SYSTEM_RESOURCE_ATTRIBUTE_ACE with specified Attribute Name and Values</returns>
        public static SystemResourceAttributeAce CreateSystemResourceAttributeAce(string attributeName, string[] values)
        {
            return new SystemResourceAttributeAce(attributeName, values);
        }

        /// <summary>
        /// Create a SYSTEM_SCOPED_POLICY_ID_ACE with specified SID and optional ACE_FLAGS.
        /// </summary>
        /// <param name="sid">A SID that identifies a central access policy.</param>
        /// <param name="flags">An unsigned 8-bit integer that specifies a set of ACE type-specific control flags. </param>
        /// <returns>Return the ACE.</returns>
        public static _SYSTEM_SCOPED_POLICY_ID_ACE CreateSystemScopedPolicyIdAce(_SID sid,
            ACE_FLAGS flags = ACE_FLAGS.OBJECT_INHERIT_ACE | ACE_FLAGS.CONTAINER_INHERIT_ACE)
        {
            _ACE_HEADER aceHeader = new _ACE_HEADER
            {
                AceFlags = flags,
                AceType = ACE_TYPE.SYSTEM_SCOPED_POLICY_ID_ACE_TYPE,
                // Header (4 bytes) + Mask (4 bytes) + SID length;
                // For details, please refer to MS-DTYP.
                AceSize = (ushort)(4 + 4 + DtypUtility.SidLength(sid)),
            };

            _SYSTEM_SCOPED_POLICY_ID_ACE ace = new _SYSTEM_SCOPED_POLICY_ID_ACE
            {
                Header = aceHeader,
                Mask = 0, // An ACCESS_MASK that MUST be set to zero.
                Sid = sid,
            };
            return ace;
        }

        /// <summary>
        /// Add the specific ACE(s) to the provided ACL. 
        /// DACL would be reordered after being added, ACCESS_DENIED_ACE before ACCESS_ALLOWED_ACE.
        /// </summary>
        /// <param name="isDACL">Whether it is DACL or SACL</param>
        /// <param name="acl">The ACL to be added to.</param>
        /// <param name="aces">The ACE(s) to be added.</param>
        public static void AddAceToAcl(ref _ACL acl, bool isDACL, params object[] aces)
        {
            if (aces == null || aces.Length == 0)
            {
                return;
            }
            acl.AceCount += (ushort)aces.Length;
            object[] newAces = new object[acl.AceCount];
            if (isDACL)
            {
                int i = 0;
                int j = acl.AceCount - 1;
                foreach (object tmp in acl.Aces)
                {
                    if (tmp is _ACCESS_ALLOWED_ACE)
                    {
                        newAces[j--] = tmp;
                    }
                    else if (tmp is _ACCESS_DENIED_ACE)
                    {
                        newAces[i++] = tmp;
                    }
                    else
                    {
                        throw new ArgumentException("Unsupport type of ACE");
                    }
                }
                foreach (object tmp in aces)
                {
                    if (tmp is _ACCESS_ALLOWED_ACE)
                    {
                        newAces[j--] = tmp;
                    }
                    else if (tmp is _ACCESS_DENIED_ACE)
                    {
                        newAces[i++] = tmp;
                    }
                    else
                    {
                        throw new ArgumentException("Unsupport type of ACE");
                    }
                }
                Debug.Assert(i - j == 1, "New ACE array should be filled.");
            }
            else // SACL does not need to be reordered
            {
                acl.Aces.CopyTo(newAces, 0);
                aces.CopyTo(newAces, acl.Aces.Length);
            }
            acl.Aces = newAces;
            acl.AclSize = CalculateAclSize(acl);
        }

        /// <summary>
        /// Create an ACL with provided ACE(s). 
        /// </summary>
        /// <param name="isDACL">Whether it is DACL or SACL</param>
        /// <param name="aces">The ACE(s) to be put into the ACL.</param>
        /// <returns>The constructed ACL structure.</returns>
        public static _ACL CreateAcl(bool isDACL, params object[] aces)
        {
            _ACL acl = new _ACL
            {
                AceCount = 0,
                Aces = new object[0],
                AclRevision = 0x02,
                // AclRevision (1 byte) + Sbz1 (1 byte) + AclSize (2 bytes)
                // + AceCount (2 bytes) + Sbz2 (2 bytes)
                AclSize = 8,
                Sbz1 = 0,
                Sbz2 = 0
            };
            if (aces != null && aces.Length != 0)
            {
                AddAceToAcl(ref acl, isDACL, aces);
            }
            return acl;
        }
    }

    /// <summary>
    /// String that indicates the type of ACE that is being presented.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    internal enum AceTypeString : byte
    {
        /// <summary>
        /// Access-allowed ACE that uses the ACCESS_ALLOWED_ACE (section 2.4.4.2) structure.
        /// </summary>
        A = 0x00,

        /// <summary>
        /// Access-denied ACE that uses the ACCESS_DENIED_ACE (section 2.4.4.4) structure.
        /// </summary>
        D = 0x01,

        /// <summary>
        /// System-audit ACE that uses the SYSTEM_AUDIT_ACE (section 2.4.4.9) structure.
        /// </summary>
        AU = 0x02,

        /// <summary>
        /// Reserved for future use.
        /// SYSTEM_ALARM_ACE_TYPE
        /// </summary>
        AL = 0x03,

        /// <summary>
        /// Object-specific access-allowed ACE that uses the 
        /// ACCESS_ALLOWED_ACE (section 2.4.4.2) structure.
        /// </summary>
        OA = 0x05,

        /// <summary>
        /// Object-specific access-denied ACE that uses the 
        /// ACCESS_DENIED_ACE (section 2.4.4.4) structure.
        /// </summary>
        OD = 0x06,

        /// <summary>
        /// Object-specific system-audit ACE that uses the 
        /// SYSTEM_AUDIT_ACE (section 2.4.4.9) structure.
        /// </summary>
        OU = 0x07,

        /// <summary>
        /// Reserved for future use.
        /// SYSTEM_ALARM_OBJECT_ACE_TYPE
        /// </summary>
        OL = 0x08,

        /// <summary>
        /// Mandatory label ACE that uses the 
        /// SYSTEM_MANDATORY_LABEL_ACE (section 2.4.4.11) structure.
        /// </summary>
        ML = 0x11,
    }

    /// <summary>
    /// A set of ACE flags that define the behavior of the ACE.
    /// This field can be a combination of the following values.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    internal enum AceFlagsString : byte
    {
        /// <summary>
        /// CONTAINER_INHERIT_ACE
        /// </summary>
        CI = 0x02,

        /// <summary>
        /// FAILED_ACCESS_ACE_FLAG
        /// </summary>
        FA = 0x80,

        /// <summary>
        /// INHERIT_ONLY_ACE
        /// </summary>
        IO = 0x08,

        /// <summary>
        /// INHERITED_ACE
        /// </summary>
        ID = 0x10,

        /// <summary>
        /// NO_PROPAGATE_INHERIT_ACE
        /// </summary>
        NP = 0x04,

        /// <summary>
        /// OBJECT_INHERIT_ACE
        /// </summary>
        OI = 0x01,

        /// <summary>
        /// SUCCESSFUL_ACCESS_ACE_FALG
        /// </summary>
        SA = 0x40,
    }

    /// <summary>
    /// A set of bit flags that control how access control entries (ACEs) 
    /// are inherited from ParentDescriptor. This parameter can be a 
    /// combination of the following values:
    /// </summary>
    [Flags]
    public enum SecurityDescriptorAutoInheritFlags
    {
        /// <summary>
        /// No flag is set.
        /// </summary>
        None = 0,

        /// <summary>
        /// If set, inheritable ACEs from the parent security descriptor 
        /// DACL are merged with the explicit ACEs in the CreatorDescriptor.
        /// </summary>
        DACL_AUTO_INHERIT = 1,

        /// <summary>
        /// If set, inheritable ACEs from the parent security descriptor 
        /// SACL are merged with the explicit ACEs in the CreatorDescriptor.
        /// </summary>
        SACL_AUTO_INHERIT = 2,

        /// <summary>
        /// Selects the CreatorDescriptor as the default security descriptor 
        /// provided that no object type specific ACEs are inherited from the 
        /// parent. If such ACEs do get inherited, CreatorDescriptor is ignored.
        /// </summary>
        DEFAULT_DESCRIPTOR_FOR_OBJECT = 4,

        /// <summary>
        /// Relevant only when the owner field is not specified in 
        /// CreatorDescriptor. If this flag is set, the owner field in 
        /// NewDescriptor is set to the owner of ParentDescriptor. 
        /// If not set, the owner from the token is selected.
        /// </summary>
        DEFAULT_OWNER_FROM_PARENT = 8,

        /// <summary>
        /// Relevant only when the primary group field is not specified in 
        /// CreatorDescriptor. If this flag is set, the primary group of 
        /// NewDescriptor is set to the primary group of ParentDescriptor. 
        /// If not set, the default group from the token is selected.
        /// </summary>
        DEFAULT_GROUP_FROM_PARENT = 0x10
    }

    /// <summary>
    /// ACL Compute type, COMPUTE_DACL or COMPUTE_SACL.
    /// </summary>
    public enum AclComputeType
    {
        /// <summary>
        /// Compute DACL
        /// </summary>
        COMPUTE_DACL,

        /// <summary>
        /// Compute SACL
        /// </summary>
        COMPUTE_SACL
    }

    /// <summary>
    /// The GENERIC_MAPPING structure defines the mapping of generic access rights to specific and 
    /// standard access rights for an object. When a client application requests generic access to
    /// an object, that request is mapped to the access rights defined in this structure.
    /// </summary>
    public struct GenericMapping
    {
        /// <summary>
        /// Specifies an access mask defining read access to an object.
        /// </summary>
        public uint GenericRead;

        /// <summary>
        /// Specifies an access mask defining write access to an object.
        /// </summary>
        public uint GenericWrite;

        /// <summary>
        /// Specifies an access mask defining execute access to an object.
        /// </summary>
        public uint GenericExecute;

        /// <summary>
        /// Specifies an access mask defining all possible types of access to an object.
        /// </summary>
        public uint GenericAll;
    }

    /// <summary>
    /// CopyFilter enumeration for post-processing the ACL.
    /// </summary>
    public enum CopyFilter
    {
        /// <summary>
        /// Copy all ACEs.
        /// </summary>
        CopyAllAces,

        /// <summary>
        /// Copy inherited ACEs.
        /// </summary>
        CopyInheritedAces,

        /// <summary>
        /// Copy explicit ACEs.
        /// </summary>
        CopyExplicitAces
    }

    /// <summary>
    /// USER_ALL values are used in the WhichFields bit field in the SAMPR_USER_ALL_INFORMATION structure. 
    /// All bits can be combined with a logical OR in any combination that is in accordance with the 
    /// processing instructions specified in sections 3.1.5.6.5, 3.1.5.6.4, 3.1.5.5.6 and 3.1.5.5.5 of MS-SAMR.
    /// If a bit is set, the associated field of SAMPR_USER_ALL_INFORMATION MUST be processed by the server. 
    /// If a bit is not set, the server MUST ignore the associated field.
    /// </summary>
    [Flags]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags")]
    public enum USER_ALL : uint
    {
        /// <summary>
        /// UserName
        /// </summary>
        USER_ALL_USERNAME = 0x00000001,

        /// <summary>
        /// FullName
        /// </summary>
        USER_ALL_FULLNAME = 0x00000002,

        /// <summary>
        /// UserId
        /// </summary>
        USER_ALL_USERID = 0x00000004,

        /// <summary>
        /// PrimaryGroupId
        /// </summary>
        USER_ALL_PRIMARYGROUPID = 0x00000008,

        /// <summary>
        /// AdminComment
        /// </summary>
        USER_ALL_ADMINCOMMENT = 0x00000010,

        /// <summary>
        /// UserComment
        /// </summary>
        USER_ALL_USERCOMMENT = 0x00000020,

        /// <summary>
        /// HomeDirectory
        /// </summary>
        USER_ALL_HOMEDIRECTORY = 0x00000040,

        /// <summary>
        /// HomeDirectoryDrive
        /// </summary>
        USER_ALL_HOMEDIRECTORYDRIVE = 0x00000080,

        /// <summary>
        /// ScriptPath
        /// </summary>
        USER_ALL_SCRIPTPATH = 0x00000100,

        /// <summary>
        /// ProfilePath
        /// </summary>
        USER_ALL_PROFILEPATH = 0x00000200,

        /// <summary>
        /// WorkStations
        /// </summary>
        USER_ALL_WORKSTATIONS = 0x00000400,

        /// <summary>
        /// LastLogon
        /// </summary>
        USER_ALL_LASTLOGON = 0x00000800,

        /// <summary>
        /// LastLogoff
        /// </summary>
        USER_ALL_LASTLOGOFF = 0x00001000,

        /// <summary>
        /// LogonHours
        /// </summary>
        USER_ALL_LOGONHOURS = 0x00002000,

        /// <summary>
        /// BadPasswordCount
        /// </summary>
        USER_ALL_BADPASSWORDCOUNT = 0x00004000,

        /// <summary>
        /// LogonCount
        /// </summary>
        USER_ALL_LOGONCOUNT = 0x00008000,

        /// <summary>
        /// PasswordCanChange
        /// </summary>
        USER_ALL_PASSWORDCANCHANGE = 0x00010000,

        /// <summary>
        /// PasswordMustChange
        /// </summary>
        USER_ALL_PASSWORDMUSTCHANGE = 0x00020000,

        /// <summary>
        /// PasswordLastSet
        /// </summary>
        USER_ALL_PASSWORDLASTSET = 0x00040000,

        /// <summary>
        /// AccountExpires
        /// </summary>
        USER_ALL_ACCOUNTEXPIRES = 0x00080000,

        /// <summary>
        /// UserAccountControl
        /// </summary>
        USER_ALL_USERACCOUNTCONTROL = 0x00100000,

        /// <summary>
        /// Parameters
        /// </summary>
        USER_ALL_PARAMETERS = 0x00200000,

        /// <summary>
        /// CountryCode
        /// </summary>
        USER_ALL_COUNTRYCODE = 0x00400000,

        /// <summary>
        /// CodePage
        /// </summary>
        USER_ALL_CODEPAGE = 0x00800000,

        /// <summary>
        /// NtPasswordPresent
        /// </summary>
        USER_ALL_NTPASSWORDPRESENT = 0x01000000,

        /// <summary>
        /// LmPasswordPresent
        /// </summary>
        USER_ALL_LMPASSWORDPRESENT = 0x02000000,

        /// <summary>
        /// PrivateData
        /// </summary>
        USER_ALL_PRIVATEDATA = 0x04000000,

        /// <summary>
        /// PasswordExpired
        /// </summary>
        USER_ALL_PASSWORDEXPIRED = 0x08000000,

        /// <summary>
        /// SecurityDescriptor
        /// </summary>
        USER_ALL_SECURITYDESCRIPTOR = 0x10000000,

        /// <summary>
        /// Undefined mask.
        /// </summary>
        USER_ALL_UNDEFINED_MASK = 0xC0000000
    }

    /// <summary>
    /// Account type values are associated with accounts and indicate the type of account. 
    /// These values may not be combined through logical operations.
    /// </summary>
    public enum ACCOUNT_TYPE
    {
        /// <summary>
        /// Represents a domain object.
        /// </summary>
        SAM_DOMAIN_OBJECT = 0x00000000,

        /// <summary>
        /// Represents a group object.
        /// </summary>
        SAM_GROUP_OBJECT = 0x10000000,

        /// <summary>
        /// Represents a group object that is not used for authorization context generation.
        /// </summary>
        SAM_NON_SECURITY_GROUP_OBJECT = 0x10000001,

        /// <summary>
        /// Represents an alias object.
        /// </summary>
        SAM_ALIAS_OBJECT = 0x20000000,

        /// <summary>
        /// Represents an alias object that is not used for authorization context generation.
        /// </summary>
        SAM_NON_SECURITY_ALIAS_OBJECT = 0x20000001,

        /// <summary>
        /// Represents a user object.
        /// </summary>
        SAM_USER_OBJECT = 0x30000000,

        /// <summary>
        /// Represents a computer object.
        /// </summary>
        SAM_MACHINE_ACCOUNT = 0x30000001,

        /// <summary>
        /// Represents a user object that is used for domain trusts.
        /// </summary>
        SAM_TRUST_ACCOUNT = 0x30000002,

        /// <summary>
        /// Represents an application-defined group.
        /// </summary>
        SAM_APP_BASIC_GROUP = 0x40000000,

        /// <summary>
        /// Represents an application-defined group whose members are determined by the results of a query.
        /// </summary>
        SAM_APP_QUERY_GROUP = 0x40000001
    }

    /// <summary>
    /// GROUP_TYPE Codes: These values specify the type of a group object. 
    /// They are used in the groupType attribute. The values are mutually exclusive, 
    /// except for the GROUP_TYPE_SECURITY_ENABLED bit, which can be combined using
    /// a logical OR with any other value.
    /// </summary>
    [Flags]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum GROUP_TYPE : uint
    {
        /// <summary>
        /// Specifies that the group is an account group.
        /// </summary>
        GROUP_TYPE_ACCOUNT_GROUP = 0x00000002,

        /// <summary>
        /// Specifies that the group is a resource group.
        /// </summary>
        GROUP_TYPE_RESOURCE_GROUP = 0x00000004,

        /// <summary>
        /// Specifies that the group is a universal group.
        /// </summary>
        GROUP_TYPE_UNIVERSAL_GROUP = 0x00000008,

        /// <summary>
        /// Specifies that the group's membership is to be included in an authorization context.
        /// </summary>
        GROUP_TYPE_SECURITY_ENABLED = 0x80000000,

        /// <summary>
        /// A combination of two of the bits shown above for the purposes of this specification.
        /// </summary>
        GROUP_TYPE_SECURITY_ACCOUNT = 0x80000002,

        /// <summary>
        /// A combination of two of the bits shown above for the purposes of this specification.
        /// </summary>
        GROUP_TYPE_SECURITY_RESOURCE = 0x80000004,

        /// <summary>
        /// A combination of two of the bits shown above for the purposes of this specification.
        /// </summary>
        GROUP_TYPE_SECURITY_UNIVERSAL = 0x80000008
    }
}