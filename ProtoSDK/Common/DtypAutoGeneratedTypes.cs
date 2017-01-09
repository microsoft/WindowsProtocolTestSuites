// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Dtyp
{
    /// <summary>
    /// The OLD_LARGE_INTEGER structure is used to represent 
    /// a 64-bit signed integer value as two 32-bit integers.
    /// </summary>
    public partial struct _OLD_LARGE_INTEGER
    {
        /// <summary>
        /// Low-order 32 bits.
        /// </summary>
        public uint LowPart;

        /// <summary>
        /// High-order 32 bits. The sign of this member determines the sign of the 64-bit integer.
        /// </summary>
        public int HighPart;
    }

    /// <summary>
    ///  The LARGE_INTEGER structure is used to represent a 64-bit
    ///  signed integer value.
    /// </summary>
    //  <remarks>
    //   MS-DTYP\e904b1ba-f774-4203-ba1b-66485165ab1a.xml
    //  </remarks>
    public partial struct _LARGE_INTEGER
    {

        /// <summary>
        ///  QuadPart member.
        /// </summary>
        public long QuadPart;
    }

    /// <summary>
    /// The MULTI_SZ structure defines an implementation-specific type 
    /// that contains a sequence of null-terminated strings, terminated by an 
    /// empty string (\0) so that the last two characters are both null terminators.
    /// </summary>
    public partial struct _MULTI_SZ
    {
        /// <summary>
        /// A data buffer, which is a string literal containing
        /// multiple null-terminated strings serially.
        /// </summary>
        public ushort[] Value;

        /// <summary>
        /// The length, in characters, including the two terminating nulls.
        /// </summary>
        public uint nChar;

    }

    /// <summary>
    ///  The RPC_UNICODE_STRING structure specifies a Unicode
    ///  string. This structure is defined in IDL as follows:
    /// </summary>
    //  <remarks>
    //   MS-DTYP\94a16bb6-c610-4cb9-8db6-26f15f560061.xml
    //  </remarks>
    public partial struct _RPC_UNICODE_STRING
    {

        /// <summary>
        ///  The length, in bytes, of the string pointed to by the
        ///  Buffer member, not including the terminating null character
        ///  if any. The length MUST be a multiple of 2. The length
        ///  SHOULD equal the entire size of the Buffer, in which
        ///  case there is no terminating null character. Any method
        ///  that accesses this structure MUST use the Length specified
        ///  instead of relying on the presence or absence of a
        ///  null character.
        /// </summary>
        public ushort Length;

        /// <summary>
        ///  The maximum size, in bytes, of the string pointed to
        ///  by Buffer. The size MUST be a multiple of 2. If not,
        ///  the size MUST be decremented by 1 prior to use. This
        ///  value MUST not be less than Length.
        /// </summary>
        public ushort MaximumLength;

        /// <summary>
        ///  A pointer to a string buffer. If MaximumLength is greater
        ///  than zero, the buffer MUST contain a non-null value.
        /// </summary>
        [Length("Length / 2")]
        [Size("MaximumLength / 2")]
        public ushort[] Buffer;
    }

    /// <summary>
    /// The FILETIME structure is a 61-bit value that represents
    /// the number of 100-nanosecond intervals that have elapsed 
    /// since January 1,1601,Coordinated Universal Time(UTC).
    /// </summary>
    public partial struct _FILETIME
    {
        /// <summary>
        /// A 32-bit unsigned integer that contains the low-order bits of the file time.
        /// </summary>
        public uint dwLowDateTime;

        /// <summary>
        /// A 32-bit unsigned integer that contains the high-order bits of the file time.
        /// </summary>
        public uint dwHighDateTime;
    }

    /// <summary>
    /// The SYSTEMTIME structure is a date and time, in Coordinated 
    /// Universal Time(CUT), represented by using individual WORD-sized
    /// structure members for the month,day,year,day of week,hour,
    /// minute,second and millisecond.
    /// </summary>
    public partial struct _SYSTEMTIME
    {
        /// <summary>
        /// wYear member
        /// </summary>
        public ushort wYear;

        /// <summary>
        /// wMonth member
        /// </summary>
        public ushort wMonth;

        /// <summary>
        /// wDayOfWeek member
        /// </summary>
        public ushort wDayOfWeek;

        /// <summary>
        /// wDay member
        /// </summary>
        public ushort wDay;

        /// <summary>
        /// wHour member
        /// </summary>
        public ushort wHour;

        /// <summary>
        /// wMinute member
        /// </summary>
        public ushort wMinute;

        /// <summary>
        /// wSecond member
        /// </summary>
        public ushort wSecond;

        /// <summary>
        /// wMilliseconds member
        /// </summary>
        public ushort wMilliseconds;
    }

    /// <summary>
    /// The UINT128 structure is intended to hold 128-bit unsigned 
    /// intergers, such as an IPv6 destination address.
    /// </summary>
    public partial struct _UINT128
    {
        /// <summary>
        /// lower member
        /// </summary>
        public ulong lower;

        /// <summary>
        /// upper member
        /// </summary>
        public ulong upper;
    }

    /// <summary>
    /// The ULARGE_INTEGER structure is used to represent
    /// a 64-bit unsigned integer value.
    /// </summary>
    public partial struct _ULARGE_INTEGER
    {
        /// <summary>
        /// QuadPart member
        /// </summary>
        public ulong QuadPart;
    }


    /// <summary>
    /// The SID_IDENTIFIER_AUTHORITY structure represents the top-level 
    /// authority of a security identifier (SID).
    /// </summary>
    public struct _SID_IDENTIFIER_AUTHORITY
    {
        /// <summary>
        /// Six element arrays of 8-bit unsigned integers that specify 
        /// the top-level authority of a SID, RPC_SID, and LSAPR_SID_INFORMATION.
        /// </summary>
        [StaticSize(6, StaticSizeMode.Elements)]
        public byte[] Value;
    }


    /// <summary>
    ///  The RPC_SID_IDENTIFIER_AUTHORITY structure is a representation
    ///  of a security identifier (SID), as specified by the
    ///  SID_IDENTIFIER_AUTHORITY structure. This structure
    ///  is defined in IDL as follows.
    /// </summary>
    //  <remarks>
    //   MS-DTYP\d7e6e5a5-437c-41e5-8ba1-bdfd43e96cbc.xml
    //  </remarks>
    public partial struct _RPC_SID_IDENTIFIER_AUTHORITY
    {

        /// <summary>
        ///  Value member.
        /// </summary>
        [Inline()]
        [StaticSize(6, StaticSizeMode.Elements)]
        public byte[] Value;
    }


    /// <summary>
    /// The SID structure defines a security identifier (SID), which is a 
    /// variable-length byte array that uniquely identifies a security principal. 
    /// Each security principal has a unique SID that is issued by a security agent. 
    /// The agent can be a Windows local system or domain. The agent generates 
    /// the SID when the security principal is created.
    /// </summary>
    public struct _SID
    {
        /// <summary>
        /// An 8-bit unsigned integer that specifies the revision level of 
        /// the SID structure. This value MUST be set to 0x01.
        /// </summary>
        public byte Revision;

        /// <summary>
        /// An 8-bit unsigned integer that specifies the number of elements 
        /// in the SubAuthority array. The maximum number of elements allowed is 15.
        /// </summary>
        public byte SubAuthorityCount;

        /// <summary>
        /// A SID_IDENTIFIER_AUTHORITY structure that contains information, which 
        /// indicates the authority under which the SID was created. It describes 
        /// the entity that created the SID.
        /// </summary>
        [StaticSize(6, StaticSizeMode.Elements)]
        public byte[] IdentifierAuthority;

        /// <summary>
        /// A variable length array of unsigned 32-bit integers that uniquely 
        /// identifies a principal relative to the IdentifierAuthority. 
        /// Its length is determined by SubAuthorityCount.
        /// </summary>
        [Size("SubAuthorityCount")]
        public uint[] SubAuthority;
    }


    /// <summary>
    ///  The RPC_SID structure is a representation of a security
    ///  identifier (SID), as specified by the SID structure.
    ///  This structure is defined in IDL as follows.
    /// </summary>
    //  <remarks>
    //   MS-DTYP\5cb97814-a1c2-4215-b7dc-76d1f4bfad01.xml
    //  </remarks>
    public partial struct _RPC_SID
    {

        /// <summary>
        ///  Revision member.
        /// </summary>
        public byte Revision;

        /// <summary>
        ///  SubAuthorityCount member.
        /// </summary>
        public byte SubAuthorityCount;

        /// <summary>
        ///  IdentifierAuthority member.
        /// </summary>
        public _RPC_SID_IDENTIFIER_AUTHORITY IdentifierAuthority;

        /// <summary>
        ///  SubAuthority member.
        /// </summary>
        [Inline()]
        [Size("SubAuthorityCount")]
        public uint[] SubAuthority;
    }


    /// <summary>
    /// An unsigned 16-bit field that specifies control access bit flags. 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum SECURITY_DESCRIPTOR_Control : ushort
    {
        /// <summary>
        /// No access.
        /// </summary>
        None = 0x0000,

        /// <summary>
        /// SR, Set when the security descriptor is in self-relative format. 
        /// Cleared when the security descriptor is in absolute format.
        /// </summary>
        SelfRelative = 0x8000,

        /// <summary>
        /// RM, Set to 0x1 when the Sbz1 field is to be interpreted as 
        /// resource manager control bits.
        /// </summary>
        RMControlValid = 0x4000,

        /// <summary>
        /// PS, Set when the SACL should be protected from inherit operations.
        /// </summary>
        SACLProtected = 0x2000,

        /// <summary>
        /// PD, Set when the DACL should be protected from inherit operations.
        /// </summary>
        DACLProtected = 0x1000,

        /// <summary>
        /// SI, Set when the SACL was created through inheritance.
        /// </summary>
        SACLAutoInherited = 0x0800,

        /// <summary>
        /// DI, Set when the DACL was created through inheritance.
        /// </summary>
        DACLAutoInherited = 0x0400,

        /// <summary>
        /// SR, Set when the SACL is to be computed through inheritance. 
        /// When both SR and SI are set, the resulting security descriptor 
        /// should set SI; the SR setting is not preserved.
        /// </summary>
        SACLInheritanceRequired = 0x0200,

        /// <summary>
        /// DR, Set when the DACL is to be computed through inheritance. 
        /// When both DR and DI are set, the resulting security descriptor 
        /// should set DI; the DR setting is not preserved.
        /// </summary>
        DACLInheritanceRequired = 0x0100,

        /// <summary>
        /// DT, Set when the ACL that is pointed to by the DACL field was 
        /// provided by a trusted source and does not require any editing of compound ACEs.
        /// </summary>
        DACLTrusted = 0x0080,

        /// <summary>
        /// SS, Set when the caller wants the system to create a Server ACL based on 
        /// the input ACL, regardless of its source (explicit or defaulting).
        /// </summary>
        ServerSecurity = 0x0040,

        /// <summary>
        /// SD, Set when the SACL was established by default means.
        /// </summary>
        SACLDefaulted = 0x0020,

        /// <summary>
        /// SP, Set when the SACL is present on the object.
        /// </summary>
        SACLPresent = 0x0010,

        /// <summary>
        /// DD, Set when the DACL was established by default means.
        /// </summary>
        DACLDefaulted = 0x0008,

        /// <summary>
        /// Set when the DACL is present on the object.
        /// </summary>
        DACLPresent = 0x0004,

        /// <summary>
        /// GD, Set when the group was established by default means.
        /// </summary>
        GroupDefaulted = 0x0002,

        /// <summary>
        /// OD, Set when the owner was established by default means.
        /// </summary>
        OwnerDefaulted = 0x0001,
    }


    /// <summary>
    /// The SECURITY_DESCRIPTOR structure defines the security attributes
    /// of an object. These attributes specify who owns the object; who can 
    /// access the object and what they can do with it; what level of audit
    /// logging should be applied to the object; and what kind of restrictions
    /// apply to the use of the security descriptor.
    /// </summary>
    public partial struct _SECURITY_DESCRIPTOR
    {
        /// <summary>
        /// An unsigned 8-bit value that specifies the revision of the 
        /// SECURITY_DESCRIPTOR structure. This field MUST be set to one.
        /// </summary>
        public byte Revision;

        /// <summary>
        /// An unsigned 8-bit value with no meaning unless the Control RM bit 
        /// is set to 0x1. If the RM bit is set to 0x1, Sbz1 is interpreted as 
        /// the resource manager control bits that contain specific information 
        /// for the specific resource manager that is accessing the structure. 
        /// The permissible values and meanings of these bits are determined by 
        /// the implementation of the resource manager.
        /// </summary>
        public byte Sbz1;

        /// <summary>
        /// An unsigned 16-bit field that specifies control access bit flags. 
        /// The Self Relative (SR) bit MUST be set when the security descriptor 
        /// is in self-relative format.
        /// </summary>
        public SECURITY_DESCRIPTOR_Control Control;

        /// <summary>
        /// An unsigned 32-bit integer that specifies the offset to the SID. 
        /// This SID specifies the owner of the object to which the security 
        /// descriptor is associated. This must be a valid offset if the OD 
        /// flag is not set. If this field is set to zero, the OwnerSid field 
        /// MUST not be present.
        /// </summary>
        public uint OffsetOwner;

        /// <summary>
        /// An unsigned 32-bit integer that specifies the offset to the SID. 
        /// This SID specifies the group of the object to which the security 
        /// descriptor is associated. This must be a valid offset if the GD 
        /// flag is not set. If this field is set to zero, the GroupSid field 
        /// MUST not be present.
        /// </summary>
        public uint OffsetGroup;

        /// <summary>
        /// An unsigned 32-bit integer that specifies the offset to the ACL 
        /// that contains system ACEs. Typically, the system ACL contains 
        /// auditing ACEs (such as SYSTEM_AUDIT_ACE, SYSTEM_AUDIT_CALLBACK_ACE, 
        /// or SYSTEM_AUDIT_CALLBACK_OBJECT_ACE), and at most one Label ACE 
        /// (as specified in section 2.4.4.11). This must be a valid offset 
        /// if the SP flag is set; if the SP flag is not set, this field MUST 
        /// be set to zero. If this field is set to zero, the Sacl field MUST not be present.
        /// </summary>
        public uint OffsetSacl;

        /// <summary>
        /// An unsigned 32-bit integer that specifies the offset to the ACL 
        /// that contains ACEs that control access. Typically, the DACL contains 
        /// ACEs that grant or deny access to principals or groups. This must be 
        /// a valid offset if the DACLPresent flag is set; if the DACLPresent flag is not set, 
        /// this field MUST be set to zero. If this field is set to zero, 
        /// the Dacl field MUST not be present.
        /// </summary>
        public uint OffsetDacl;

        /// <summary>
        /// The SID of the owner of the object. The length of the SID MUST be a multiple of 4. 
        /// This field MUST be present if the OffsetOwner field is not zero.
        /// </summary>
        public _SID? OwnerSid;

        /// <summary>
        /// The SID of the group of the object. The length of the SID MUST be a multiple of 4. 
        /// This field MUST be present if the GroupOwner field is not zero.
        /// </summary>
        public _SID? GroupSid;

        /// <summary>
        /// The SACL of the object. The length of the SID MUST be a multiple of 4. 
        /// This field MUST be present if the SP flag is set.
        /// </summary>
        public _ACL? Sacl;

        /// <summary>
        /// The DACL of the object. The length of the SID MUST be a multiple of 4. 
        /// This field MUST be present if the DACLPresent flag is set.
        /// </summary>
        public _ACL? Dacl;

    }

    /// <summary>
    /// The SECURITY_DESCRIPTOR structure defines the security attributes
    /// of an object. These attributes specify who owns the object; who can 
    /// access the object and what they can do with it; what level of audit
    /// logging should be applied to the object; and what kind of restrictions
    /// apply to the use of the security descriptor.
    /// </summary>
    public partial struct _RPC_SECURITY_DESCRIPTOR
    {
        /// <summary>
        /// An unsigned 8-bit value that specifies the revision of the 
        /// SECURITY_DESCRIPTOR structure. This field MUST be set to one.
        /// </summary>
        public byte Revision;

        /// <summary>
        /// An unsigned 8-bit value with no meaning unless the Control RM bit 
        /// is set to 0x1. If the RM bit is set to 0x1, Sbz1 is interpreted as 
        /// the resource manager control bits that contain specific information 
        /// for the specific resource manager that is accessing the structure. 
        /// The permissible values and meanings of these bits are determined by 
        /// the implementation of the resource manager.
        /// </summary>
        public byte Sbz1;

        /// <summary>
        /// An unsigned 16-bit field that specifies control access bit flags. 
        /// The Self Relative (SR) bit MUST be set.
        /// </summary>
        public SECURITY_DESCRIPTOR_Control Control;

        /// <summary>
        /// The SID of the owner of the object. The length of the SID MUST be a multiple of 4. 
        /// This field MUST be present if the OffsetOwner field is not zero.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _RPC_SID[] Sid;

        /// <summary>
        /// The SID of the group of the object. The length of the SID MUST be a multiple of 4. 
        /// This field MUST be present if the GroupOwner field is not zero.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _RPC_SID[] Group;

        /// <summary>
        /// The SACL of the object. The length of the SID MUST be a multiple of 4. 
        /// This field MUST be present if the SP flag is set.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _RPC_ACL[] Sacl;

        /// <summary>
        /// The DACL of the object. The length of the SID MUST be a multiple of 4. 
        /// This field MUST be present if the DACLPresent flag is set.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _RPC_ACL[] Dacl;
    }

    /// <summary>
    ///  The RPC_STRING structure holds a counted string encoded
    ///  in the OEM code page.
    /// </summary>
    public partial struct _RPC_STRING
    {
        /// <summary>
        ///  The size, in bytes, not including a NULL terminator,
        ///  of the string contained in Buffer.
        /// </summary>
        public ushort Length;

        /// <summary>
        ///  The size, in bytes, of the Buffer member.
        /// </summary>
        public ushort MaximumLength;

        /// <summary>
        ///  A buffer containing a string encoded in the OEM code
        ///  page. The string is counted (by the Length member),
        ///  and therefore is not NULL-terminated.
        /// </summary>
        [Length("Length")]
        [Size("MaximumLength")]
        public byte[] Buffer;
    }

    /// <summary>
    /// The SECURITY_INFORMATION data type identifies the object-related security information being set or queried.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    public enum SECURITY_INFORMATION : uint
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
    /// An unsigned 8-bit integer that specifies the ACE types. 
    /// This field MUST be one of the following values.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum ACE_TYPE : byte
    {
        /// <summary>
        /// Access-allowed ACE that uses the ACCESS_ALLOWED_ACE (section 2.4.4.2) structure.
        /// </summary>
        ACCESS_ALLOWED_ACE_TYPE = 0x00,

        /// <summary>
        /// Access-denied ACE that uses the ACCESS_DENIED_ACE (section 2.4.4.4) structure.
        /// </summary>
        ACCESS_DENIED_ACE_TYPE = 0x01,

        /// <summary>
        /// System-audit ACE that uses the SYSTEM_AUDIT_ACE (section 2.4.4.9) structure.
        /// </summary>
        SYSTEM_AUDIT_ACE_TYPE = 0x02,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        SYSTEM_ALARM_ACE_TYPE = 0x03,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        ACCESS_ALLOWED_COMPOUND_ACE_TYPE = 0x04,

        /// <summary>
        /// Object-specific access-allowed ACE that uses the 
        /// ACCESS_ALLOWED_ACE (section 2.4.4.2) structure.
        /// </summary>
        ACCESS_ALLOWED_OBJECT_ACE_TYPE = 0x05,

        /// <summary>
        /// Object-specific access-denied ACE that uses the 
        /// ACCESS_DENIED_ACE (section 2.4.4.4) structure.
        /// </summary>
        ACCESS_DENIED_OBJECT_ACE_TYPE = 0x06,

        /// <summary>
        /// Object-specific system-audit ACE that uses the 
        /// SYSTEM_AUDIT_ACE (section 2.4.4.9) structure.
        /// </summary>
        SYSTEM_AUDIT_OBJECT_ACE_TYPE = 0x07,
        /// <summary>
        /// Reserved for future use.
        /// </summary>
        SYSTEM_ALARM_OBJECT_ACE_TYPE = 0x08,

        /// <summary>
        /// Access-allowed callback ACE that uses the 
        /// ACCESS_ALLOWED_CALLBACK_ACE (section 2.4.4.5) structure.
        /// </summary>
        ACCESS_ALLOWED_CALLBACK_ACE_TYPE = 0x09,

        /// <summary>
        /// Access-denied callback ACE that uses the
        /// ACCESS_DENIED_CALLBACK_ACE (section 2.4.4.6) structure.
        /// </summary>
        ACCESS_DENIED_CALLBACK_ACE_TYPE = 0x0A,

        /// <summary>
        /// Object-specific access-allowed callback ACE that uses the
        /// ACCESS_ALLOWED_CALLBACK_OBJECT_ACE (section 2.4.4.7) structure.
        /// </summary>
        ACCESS_ALLOWED_CALLBACK_OBJECT_ACE_TYPE = 0x0B,

        /// <summary>
        /// Object-specific access-denied callback ACE that uses the 
        /// ACCESS_DENIED_CALLBACK_OBJECT_ACE (section 2.4.4.8) structure.
        /// </summary>
        ACCESS_DENIED_CALLBACK_OBJECT_ACE_TYPE = 0x0C,

        /// <summary>
        /// System-audit callback ACE that uses the 
        /// SYSTEM_AUDIT_CALLBACK_ACE (section 2.4.4.10) structure.
        /// </summary>
        SYSTEM_AUDIT_CALLBACK_ACE_TYPE = 0x0D,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        SYSTEM_ALARM_CALLBACK_ACE_TYPE = 0x0E,

        /// <summary>
        /// Object-specific system-audit callback ACE that uses the 
        /// SYSTEM_AUDIT_CALLBACK_OBJECT_ACE (section 2.4.4.12) structure.
        /// </summary>
        SYSTEM_AUDIT_CALLBACK_OBJECT_ACE_TYPE = 0x0F,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        SYSTEM_ALARM_CALLBACK_OBJECT_ACE_TYPE = 0x10,

        /// <summary>
        /// Mandatory label ACE that uses the 
        /// SYSTEM_MANDATORY_LABEL_ACE (section 2.4.4.11) structure.
        /// </summary>
        SYSTEM_MANDATORY_LABLE_ACE_TYPE = 0x11,

        /// <summary>
        /// Resource attribute ACE that uses the 
        /// SYSTEM_RESOURCE_ATTRIBUTE_ACE (section 2.4.4.15)
        /// </summary>
        SYSTEM_RESOURCE_ATTRIBUTE_ACE_TYPE = 0x12,

        /// <summary>
        /// A central policy ID ACE that uses the 
        /// SYSTEM_SCOPED_POLICY_ID_ACE (section 2.4.4.16)
        /// </summary>
        SYSTEM_SCOPED_POLICY_ID_ACE_TYPE = 0x13
    }


    /// <summary>
    /// An unsigned 8-bit integer that specifies a set of ACE type-specific control flags. 
    /// This field can be a combination of the following values.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum ACE_FLAGS : byte
    {
        /// <summary>
        /// No flag is set.
        /// </summary>
        None = 0,

        /// <summary>
        /// Child objects that are containers, such as directories, 
        /// inherit the ACE as an effective ACE. The inherited ACE 
        /// is inheritable unless the NO_PROPAGATE_INHERIT_ACE bit flag is also set.
        /// </summary>
        CONTAINER_INHERIT_ACE = 0x02,

        /// <summary>
        /// Used with system-audit ACEs in a system access control list (SACL) to 
        /// generate audit messages for failed access attempts
        /// </summary>
        FAILED_ACCESS_ACE_FLAG = 0x80,

        /// <summary>
        /// Indicates an inherit-only ACE, which does not control access to the 
        /// object to which it is attached. If this flag is not set, the ACE is
        /// an effective ACE that controls access to the object to which it is attached.
        /// Both effective and inherit-only ACEs can be inherited depending 
        /// on the state of the other inheritance flags.
        /// </summary>
        INHERIT_ONLY_ACE = 0x08,

        /// <summary>
        /// Indicates that the ACE was inherited. The system sets this bit when 
        /// it propagates an inherited ACE to a child object.
        /// </summary>
        INHERITED_ACE = 0x10,

        /// <summary>
        /// If the ACE is inherited by a child object, the system clears the 
        /// OBJECT_INHERIT_ACE and CONTAINER_INHERIT_ACE flags in the inherited ACE.
        /// This prevents the ACE from being inherited by subsequent generations of objects.
        /// </summary>
        NO_PROPAGATE_INHERIT_ACE = 0x04,

        /// <summary>
        /// Noncontainer child objects inherit the ACE as an effective ACE.
        /// For child objects that are containers, the ACE is inherited as an
        /// inherit-only ACE unless the NO_PROPAGATE_INHERIT_ACE bit flag is also set.
        /// </summary>
        OBJECT_INHERIT_ACE = 0x01,

        /// <summary>
        /// Used with system-audit ACEs in a SACL to generate audit messages 
        /// for successful access attempts.
        /// </summary>
        SUCCESSFUL_ACCESS_ACE_FALG = 0x40,
    }


    /// <summary>
    /// The ACE_HEADER structure defines the type and size of an access control entry (ACE).
    /// </summary>
    public struct _ACE_HEADER
    {
        /// <summary>
        /// An unsigned 8-bit integer that specifies the ACE types. 
        /// </summary>
        public ACE_TYPE AceType;

        /// <summary>
        /// An unsigned 8-bit integer that specifies a set of ACE 
        /// type-specific control flags.
        /// </summary>
        public ACE_FLAGS AceFlags;

        /// <summary>
        /// An unsigned 16-bit integer that specifies the size, in bytes, 
        /// of the ACE. The AceSize field can be greater than the sum of 
        /// the individual fields. In cases where the AceSize field encompasses 
        /// additional data for the callback ACEs types, that data is 
        /// implementation-specific. Otherwise, this additional data is not 
        /// interpreted and MUST be ignored.
        /// </summary>
        public ushort AceSize;
    }


    /// <summary>
    /// The ACCESS_ALLOWED_ACE structure defines an ACE for the discretionary 
    /// access control list (DACL) that controls access to an object. 
    /// An access-allowed ACE allows access to an object for a specific trustee 
    /// identified by a security identifier (SID).
    /// </summary>
    public struct _ACCESS_ALLOWED_ACE
    {
        /// <summary>
        /// An ACE_HEADER structure that specifies the size and type of ACE. 
        /// It also contains flags that control inheritance of the ACE by child objects.
        /// </summary>
        public _ACE_HEADER Header;

        /// <summary>
        /// An ACCESS_MASK that specifies the user rights allowed by this ACE.
        /// </summary>
        public uint Mask;

        /// <summary>
        /// The SID of a trustee. The length of the SID MUST be a multiple of 4.
        /// </summary>
        public _SID Sid;
    }


    /// <summary>
    /// An ACCESS_MASK that specifies the user rights allowed/denied by this ACE.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum ACCESS_OBJECT_ACE_Mask : uint
    {
        /// <summary>
        /// No bit is set.
        /// </summary>
        None = 0,

        /// <summary>
        /// The ObjectType GUID identifies an extended access right.
        /// </summary>
        ADS_RIGHT_DS_CONTROL_ACCESS = 0x00000100,

        /// <summary>
        /// The ObjectType GUID identifies a type of child object.
        /// The ACE controls the trustee's right to create this type of child object.
        /// </summary>
        ADS_RIGHT_DS_CREATE_CHILD = 0x00000001,

        /// <summary>
        /// The ObjectType GUID identifies a type of child object. 
        /// The ACE controls the trustee's right to delete this type of child object.
        /// </summary>
        ADS_RIGHT_DS_DELETE_CHILD = 0x00000002,

        /// <summary>
        /// The ObjectType GUID identifies a property set or property of the object. 
        /// The ACE controls the trustee's right to read the property or property set.
        /// </summary>
        ADS_RIGHT_DS_READ_PROP = 0x00000010,

        /// <summary>
        /// The ObjectType GUID identifies a property set or property of the object. 
        /// The ACE controls the trustee's right to write the property or property set
        /// </summary>
        ADS_RIGHT_DS_WRITE_PROP = 0x00000020,

        /// <summary>
        /// The ObjectType GUID identifies a validated write.
        /// </summary>
        ADS_RIGHT_DS_SELF = 0x00000008,
    }


    /// <summary>
    /// A 32-bit unsigned integer that specifies a set of bit flags that 
    /// indicate whether the ObjectType and InheritedObjectType fields 
    /// contain valid data. This parameter can be one or more of the following values.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum ACCESS_OBJECT_ACE_Flags : uint
    {
        /// <summary>
        /// Neither ObjectType nor InheritedObjectType are valid.
        /// </summary>
        None = 0,

        /// <summary>
        /// ObjectType is valid.
        /// </summary>
        ACE_OBJECT_TYPE_PRESENT = 0x00000001,

        /// <summary>
        /// InheritedObjectType is valid. 
        /// If this value is not specified, all types of child objects can inherit the ACE.
        /// </summary>
        ACE_INHERITED_OBJECT_TYPE_PRESENT = 0x00000002,
    }


    /// <summary>
    /// The ACCESS_ALLOWED_OBJECT_ACE structure defines an ACE that controls 
    /// allowed access to an object, a property set, or property. The ACE 
    /// contains a set of access rights, a GUID that identifies the type of object, 
    /// and a SID that identifies the trustee to whom the system will grant access. 
    /// The ACE also contains a GUID and a set of flags that control inheritance of 
    /// the ACE by child objects.
    /// </summary>
    public partial struct _ACCESS_ALLOWED_OBJECT_ACE
    {
        /// <summary>
        /// An ACE_HEADER structure that specifies the size and type of ACE. 
        /// It also contains flags that control inheritance of the ACE by child objects.
        /// </summary>
        public _ACE_HEADER Header;

        /// <summary>
        /// An ACCESS_MASK that specifies the user rights allowed by this ACE.
        /// </summary>
        public ACCESS_OBJECT_ACE_Mask Mask;

        /// <summary>
        /// A 32-bit unsigned integer that specifies a set of bit flags that 
        /// indicate whether the ObjectType and InheritedObjectType fields 
        /// contain valid data.
        /// </summary>
        public ACCESS_OBJECT_ACE_Flags Flags;

        /// <summary>
        /// A GUID that identifies a property set, property, extended right, 
        /// or type of child object. The purpose of this GUID depends on the 
        /// user rights specified in the Mask field. This field is valid only 
        /// if the ACE _OBJECT_TYPE_PRESENT bit is set in the Flags field. 
        /// Otherwise, the ObjectType field is ignored. For information on 
        /// access rights and for a mapping of the control access rights to 
        /// the corresponding GUID value that identifies each right, 
        /// see [MS-ADTS] sections 5.1.3.2 and 5.1.3.2.1.
        /// ACCESS_MASK bits are not mutually exclusive. Therefore, the ObjectType 
        /// field can be set in an ACE with any DtypUtility.ACCESS_MASK_ If the AccessCheck 
        /// algorithm calls this ACE and does not find an appropriate GUID, 
        /// then that ACE will be ignored. For more information on access checks 
        /// and object access, see [MS-ADTS] section 5.1.3.3.3.
        /// </summary>
        [MarshalingCondition("IsObjectTypePresent")]
        public Guid ObjectType;

        /// <summary>
        /// A GUID that identifies the type of child object that can inherit 
        /// the ACE. Inheritance is also controlled by the inheritance flags 
        /// in the ACE_HEADER, as well as by any protection against inheritance 
        /// placed on the child objects. This field is valid only if the 
        /// ACE_INHERITED_OBJECT_TYPE_PRESENT bit is set in the Flags member. 
        /// Otherwise, the InheritedObjectType field is ignored.
        /// </summary>
        [MarshalingCondition("IsInheritedObjectTypePresent")]
        public Guid InheritedObjectType;

        /// <summary>
        /// The SID of a trustee. The length of the SID MUST be a multiple of 4.
        /// </summary>
        public _SID Sid;
    }


    /// <summary>
    /// The _ACCESS_DENIED_OBJECT_ACE structure defines an ACE that controls 
    /// denied access to an object, a property set, or property. The ACE 
    /// contains a set of access rights, a GUID that identifies the type of object, 
    /// and a SID that identifies the trustee to whom the system will grant access. 
    /// The ACE also contains a GUID and a set of flags that control inheritance of 
    /// the ACE by child objects.
    /// </summary>
    public partial struct _ACCESS_DENIED_OBJECT_ACE
    {
        /// <summary>
        /// An ACE_HEADER structure that specifies the size and type of ACE. 
        /// It also contains flags that control inheritance of the ACE by child objects.
        /// </summary>
        public _ACE_HEADER Header;

        /// <summary>
        /// An ACCESS_MASK that specifies the user rights allowed by this ACE.
        /// </summary>
        public ACCESS_OBJECT_ACE_Mask Mask;

        /// <summary>
        /// A 32-bit unsigned integer that specifies a set of bit flags that 
        /// indicate whether the ObjectType and InheritedObjectType fields 
        /// contain valid data.
        /// </summary>
        public ACCESS_OBJECT_ACE_Flags Flags;

        /// <summary>
        /// A GUID that identifies a property set, property, extended right, 
        /// or type of child object. The purpose of this GUID depends on the 
        /// user rights specified in the Mask field. This field is valid only 
        /// if the ACE _OBJECT_TYPE_PRESENT bit is set in the Flags field. 
        /// Otherwise, the ObjectType field is ignored. For information on 
        /// access rights and for a mapping of the control access rights to 
        /// the corresponding GUID value that identifies each right, 
        /// see [MS-ADTS] sections 5.1.3.2 and 5.1.3.2.1.
        /// ACCESS_MASK bits are not mutually exclusive. Therefore, the ObjectType 
        /// field can be set in an ACE with any DtypUtility.ACCESS_MASK_ If the AccessCheck 
        /// algorithm calls this ACE and does not find an appropriate GUID, 
        /// then that ACE will be ignored. For more information on access checks 
        /// and object access, see [MS-ADTS] section 5.1.3.3.3.
        /// </summary>
        [MarshalingCondition("IsObjectTypePresent")]
        public Guid ObjectType;

        /// <summary>
        /// A GUID that identifies the type of child object that can inherit 
        /// the ACE. Inheritance is also controlled by the inheritance flags 
        /// in the ACE_HEADER, as well as by any protection against inheritance 
        /// placed on the child objects. This field is valid only if the 
        /// ACE_INHERITED_OBJECT_TYPE_PRESENT bit is set in the Flags member. 
        /// Otherwise, the InheritedObjectType field is ignored.
        /// </summary>
        [MarshalingCondition("IsInheritedObjectTypePresent")]
        public Guid InheritedObjectType;

        /// <summary>
        /// The SID of a trustee. The length of the SID MUST be a multiple of 4.
        /// </summary>
        public _SID Sid;
    }


    /// <summary>
    /// The ACCESS_DENIED_ACE structure defines an ACE for the DACL that 
    /// controls access to an object. An access-denied ACE denies access 
    /// to an object for a specific trustee identified by a SID.
    /// </summary>
    public struct _ACCESS_DENIED_ACE
    {
        /// <summary>
        /// An ACE_HEADER structure that specifies the size and type of ACE. 
        /// It also contains flags that control inheritance of the ACE by child objects.
        /// </summary>
        public _ACE_HEADER Header;

        /// <summary>
        /// An ACCESS_MASK that specifies the user rights denied by this ACE.
        /// </summary>
        public uint Mask;

        /// <summary>
        /// The SID of a trustee. The length of the SID MUST be a multiple of 4.
        /// </summary>
        public _SID Sid;
    }


    /// <summary>
    /// The ACCESS_ALLOWED_CALLBACK_ACE structure defines an ACE for the DACL 
    /// that controls access to an object. An access-allowed ACE allows access 
    /// to an object for a specific trustee identified by a SID.
    /// </summary>
    public partial struct _ACCESS_ALLOWED_CALLBACK_ACE
    {
        /// <summary>
        /// An ACE_HEADER structure that specifies the size and type of ACE. 
        /// It also contains flags that control inheritance of the ACE by child objects.
        /// </summary>
        public _ACE_HEADER Header;

        /// <summary>
        /// An ACCESS_MASK that specifies the user rights allowed by this ACE.
        /// </summary>
        public uint Mask;

        /// <summary>
        /// The SID of a trustee. The length of the SID MUST be a multiple of 4.
        /// </summary>
        public _SID Sid;

        /// <summary>
        /// Optional application data. The size of the application data is 
        /// determined by the AceSize field of the ACE_HEADER.
        /// </summary>
        [Size("@CalculateApplicationDataLength")]
        public byte[] ApplicationData;
    }


    /// <summary>
    /// The ACCESS_DENIED_CALLBACK_ACE structure defines an ACE for the DACL 
    /// that controls access to an object. An access-denied ACE denies access 
    /// to an object for a specific trustee identified by a SID.
    /// </summary>
    public partial struct _ACCESS_DENIED_CALLBACK_ACE
    {
        /// <summary>
        /// An ACE_HEADER structure that specifies the size and type of ACE. 
        /// It also contains flags that control inheritance of the ACE by child objects.
        /// </summary>
        public _ACE_HEADER Header;

        /// <summary>
        /// An ACCESS_MASK that specifies the user rights denied by this ACE.
        /// </summary>
        public uint Mask;

        /// <summary>
        /// The SID of a trustee. The length of the SID MUST be a multiple of 4.
        /// </summary>
        public _SID Sid;

        /// <summary>
        /// Optional application data. The size of the application data is determined 
        /// by the AceSize field of the ACE_HEADER.
        /// </summary>
        [Size("@CalculateApplicationDataLength")]
        public byte[] ApplicationData;
    }


    /// <summary>
    /// The ACCESS_ALLOWED_CALLBACK_OBJECT_ACE structure defines an ACE that 
    /// controls allowed access to an object, property set, or property. 
    /// The ACE contains a set of user rights, a GUID that identifies the type 
    /// of object, and a SID that identifies the trustee to whom the system 
    /// will grant access. The ACE also contains a GUID and a set of flags that 
    /// control inheritance of the ACE by child objects.
    /// </summary>
    public partial struct _ACCESS_ALLOWED_CALLBACK_OBJECT_ACE
    {
        /// <summary>
        /// An ACE_HEADER structure that specifies the size and type of ACE. 
        /// It also contains flags that control inheritance of the ACE by child objects.
        /// </summary>
        public _ACE_HEADER Header;

        /// <summary>
        /// An ACCESS_MASK structure that specifies the user rights allowed by this ACE.
        /// </summary>
        public ACCESS_OBJECT_ACE_Mask Mask;

        /// <summary>
        /// A 32-bit unsigned integer that specifies a set of bit flags 
        /// that indicate whether the ObjectType and InheritedObjectType 
        /// fields contain valid data.
        /// </summary>
        public ACCESS_OBJECT_ACE_Flags Flags;

        /// <summary>
        /// A GUID that identifies a property set, property, extended right, 
        /// or type of child object. The purpose of this GUID depends on 
        /// the user rights specified in the Mask field. This field is 
        /// valid only if the ACE _OBJECT_TYPE_PRESENT bit is set in the 
        /// Flags field. Otherwise, the ObjectType field is ignored.
        /// </summary>
        [MarshalingCondition("IsObjectTypePresent")]
        public Guid ObjectType;

        /// <summary>
        /// A GUID that identifies the type of child object that can inherit 
        /// the ACE. Inheritance is also controlled by the inheritance flags 
        /// in the ACE_HEADER, as well as by any protection against inheritance 
        /// placed on the child objects. This field is valid only if the 
        /// ACE_INHERITED_OBJECT_TYPE_PRESENT bit is set in the Flags member. 
        /// Otherwise, the InheritedObjectType field is ignored.
        /// </summary>
        [MarshalingCondition("IsInheritedObjectTypePresent")]
        public Guid InheritedObjectType;

        /// <summary>
        /// The SID of a trustee. The length of the SID MUST be a multiple of 4.
        /// </summary>
        public _SID Sid;

        /// <summary>
        /// Optional application data. The size of the application data 
        /// is determined by the AceSize field of the ACE_HEADER.
        /// </summary>
        [Size("@CalculateApplicationDataLength")]
        public byte[] ApplicationData;
    }


    /// <summary>
    /// The ACCESS_DENIED_CALLBACK_OBJECT_ACE structure defines an ACE 
    /// that controls denied access to an object, a property set, or property. 
    /// The ACE contains a set of user rights, a GUID that identifies the 
    /// type of object, and a SID that identifies the trustee to whom the 
    /// system will deny access. The ACE also contains a GUID and a set of 
    /// flags that control inheritance of the ACE by child objects.
    /// </summary>
    public partial struct _ACCESS_DENIED_CALLBACK_OBJECT_ACE
    {
        /// <summary>
        /// An ACE_HEADER structure that specifies the size and type of ACE. 
        /// It also contains flags that control inheritance of the ACE by child objects.
        /// </summary>
        public _ACE_HEADER Header;

        /// <summary>
        /// An ACCESS_MASK structure that specifies the user rights denied by this ACE.
        /// </summary>
        public ACCESS_OBJECT_ACE_Mask Mask;

        /// <summary>
        /// A 32-bit unsigned integer that specifies a set of bit flags 
        /// that indicate whether the ObjectType and InheritedObjectType 
        /// fields contain valid data.
        /// </summary>
        public ACCESS_OBJECT_ACE_Flags Flags;

        /// <summary>
        /// A GUID that identifies a property set, property, extended right, 
        /// or type of child object. The purpose of this GUID depends on 
        /// the user rights specified in the Mask field. This field is 
        /// valid only if the ACE _OBJECT_TYPE_PRESENT bit is set in the 
        /// Flags field. Otherwise, the ObjectType field is ignored.
        /// </summary>
        [MarshalingCondition("IsObjectTypePresent")]
        public Guid ObjectType;

        /// <summary>
        /// A GUID that identifies the type of child object that can inherit 
        /// the ACE. Inheritance is also controlled by the inheritance flags 
        /// in the ACE_HEADER, as well as by any protection against inheritance 
        /// placed on the child objects. This field is valid only if the 
        /// ACE_INHERITED_OBJECT_TYPE_PRESENT bit is set in the Flags member. 
        /// Otherwise, the InheritedObjectType field is ignored.
        /// </summary>
        [MarshalingCondition("IsInheritedObjectTypePresent")]
        public Guid InheritedObjectType;

        /// <summary>
        /// The SID of a trustee. The length of the SID MUST be a multiple of 4.
        /// </summary>
        public _SID Sid;

        /// <summary>
        /// Optional application data. The size of the application data 
        /// is determined by the AceSize field of the ACE_HEADER.
        /// </summary>
        [Size("@CalculateApplicationDataLength")]
        public byte[] ApplicationData;
    }


    /// <summary>
    /// The SYSTEM_AUDIT_ACE structure defines an access ACE for the system 
    /// access control list (SACL) that specifies what types of access cause 
    /// system-level notifications. A system-audit ACE causes an audit message 
    /// to be logged when a specified trustee attempts to gain access to an object. 
    /// The trustee is identified by a SID.
    /// </summary>
    public struct _SYSTEM_AUDIT_ACE
    {
        /// <summary>
        /// An ACE_HEADER structure that specifies the size and type of ACE. 
        /// It also contains flags that control inheritance of the ACE by child objects.
        /// </summary>
        public _ACE_HEADER Header;

        /// <summary>
        /// An ACCESS_MASK structure that specifies the user rights that 
        /// cause audit messages to be generated.
        /// </summary>
        public uint Mask;

        /// <summary>
        /// The SID of a trustee. The length of the SID MUST be a multiple of 4. 
        /// An access attempt of a kind specified by the Mask field by any trustee 
        /// whose SID matches the Sid field causes the system to generate an audit 
        /// message. If an application does not specify a SID for this field, audit 
        /// messages are generated for the specified access rights for all trustees.
        /// </summary>
        public _SID Sid;
    }


    /// <summary>
    /// The SYSTEM_AUDIT_OBJECT_ACE structure defines an access control entry (ACE)
    /// for a system access control list (SACL). The ACE can audit access to an object
    /// or subobjects such as property sets or properties. The ACE contains a set of 
    /// access rights, a GUID that identifies the type of object or subobject, and a 
    /// security identifier (SID) that identifies the trustee for whom the system will 
    /// audit access. The ACE also contains a GUID and a set of flags that control 
    /// inheritance of the ACE by child objects.
    /// </summary>
    public partial struct _SYSTEM_AUDIT_OBJECT_ACE
    {
        /// <summary>
        /// An ACE_HEADER structure that specifies the size and type of ACE. 
        /// It also contains flags that control inheritance of the ACE by child objects.
        /// </summary>
        public _ACE_HEADER Header;

        /// <summary>
        /// An ACCESS_MASK structure that specifies the user rights that 
        /// cause audit messages to be generated.
        /// </summary>
        public ACCESS_OBJECT_ACE_Mask Mask;

        /// <summary>
        /// A 32-bit unsigned integer that specifies a set of bit flags 
        /// that indicate whether the ObjectType and InheritedObjectType 
        /// fields contain valid data.
        /// </summary>
        public ACCESS_OBJECT_ACE_Flags Flags;

        /// <summary>
        /// A GUID that identifies a property set, property, extended right, 
        /// or type of child object. The purpose of this GUID depends on 
        /// the user rights specified in the Mask field. This field is 
        /// valid only if the ACE _OBJECT_TYPE_PRESENT bit is set in the 
        /// Flags field. Otherwise, the ObjectType field is ignored.
        /// </summary>
        [MarshalingCondition("IsObjectTypePresent")]
        public Guid ObjectType;

        /// <summary>
        /// A GUID that identifies the type of child object that can inherit 
        /// the ACE. Inheritance is also controlled by the inheritance flags 
        /// in the ACE_HEADER, as well as by any protection against inheritance 
        /// placed on the child objects. This field is valid only if the 
        /// ACE_INHERITED_OBJECT_TYPE_PRESENT bit is set in the Flags member. 
        /// Otherwise, the InheritedObjectType field is ignored.
        /// </summary>
        [MarshalingCondition("IsInheritedObjectTypePresent")]
        public Guid InheritedObjectType;

        /// <summary>
        /// The SID of a trustee. The length of the SID MUST be a multiple of 4. 
        /// An access attempt of a kind specified by the Mask field by any trustee 
        /// whose SID matches the Sid field causes the system to generate an audit 
        /// message. If an application does not specify a SID for this field, audit 
        /// messages are generated for the specified access rights for all trustees.
        /// </summary>
        public _SID Sid;
    }


    /// <summary>
    /// The SYSTEM_AUDIT_CALLBACK_ACE structure defines an ACE for the SACL that 
    /// specifies what types of access cause system-level notifications. 
    /// A system-audit ACE causes an audit message to be logged when a specified 
    /// trustee attempts to gain access to an object. The trustee is identified by a SID.
    /// </summary>
    public partial struct _SYSTEM_AUDIT_CALLBACK_ACE
    {
        /// <summary>
        /// An ACE_HEADER structure that specifies the size and type of ACE. 
        /// It also contains flags that control inheritance of the ACE by child objects.
        /// </summary>
        public _ACE_HEADER Header;

        /// <summary>
        /// An ACCESS_MASK structure that specifies the user rights that cause 
        /// audit messages to be generated.
        /// </summary>
        public uint Mask;

        /// <summary>
        /// The SID of a trustee. The length of the SID MUST be a multiple of 4. 
        /// An access attempt of a kind specified by the Mask field by any trustee 
        /// whose SID matches the Sid field causes the system to generate an 
        /// audit message. If an application does not specify a SID for this field, 
        /// audit messages are generated for the specified access rights for all trustees.
        /// </summary>
        public _SID Sid;

        /// <summary>
        /// Optional application data. The size of the application data is determined 
        /// by the AceSize field of the ACE_HEADER.
        /// </summary>
        [Size("@CalculateApplicationDataLength")]
        public byte[] ApplicationData;
    }


    /// <summary>
    /// An ACCESS_MASK structure that specifies the access policy for principals 
    /// with a mandatory integrity level lower than the object associated with 
    /// the SACL that contains this ACE.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum SYSTEM_MANDATORY_LABEL_ACE_Mask : uint
    {
        /// <summary>
        /// A principal with a lower mandatory level than the object cannot write to the object.
        /// </summary>
        SYSTEM_MANDATORY_LABEL_NO_WRITE_UP = 0x00000001,

        /// <summary>
        /// A principal with a lower mandatory level than the object cannot read the object.
        /// </summary>
        SYSTEM_MANDATORY_LABEL_NO_READ_UP = 0x00000002,

        /// <summary>
        /// A principal with a lower mandatory level than the object cannot execute the object.
        /// </summary>
        SYSTEM_MANDATORY_LABEL_NO_EXECUTE_UP = 0x00000004,
    }


    /// <summary>
    /// The SYSTEM_MANDATORY_LABEL_ACE structure defines an ACE for the SACL 
    /// that specifies the mandatory access level and policy for a securable object.
    /// </summary>
    public struct _SYSTEM_MANDATORY_LABEL_ACE
    {
        /// <summary>
        /// An ACE_HEADER structure that specifies the size and type of ACE. 
        /// It also contains flags that control inheritance of the ACE by child objects.
        /// </summary>
        public _ACE_HEADER Header;

        /// <summary>
        /// An ACCESS_MASK structure that specifies the access policy for principals 
        /// with a mandatory integrity level lower than the object associated with 
        /// the SACL that contains this ACE.
        /// </summary>
        public SYSTEM_MANDATORY_LABEL_ACE_Mask Mask;

        /// <summary>
        /// The SID of a trustee. The length of the SID MUST be a multiple of 4. 
        /// The identifier authority of the SID must be SECURITY_MANDATORY_LABEL_AUTHORITY. 
        /// The RID of the SID specifies the mandatory integrity level of the 
        /// object associated with the SACL that contains this ACE.
        /// The RID must be one of the following values.<para/>
        /// 0x00000000 Untrusted integrity level.<para/>
        /// 0x00001000 Low integrity level.<para/>
        /// 0x00002000 Medium integrity level.<para/>
        /// 0x00003000 High integrity level.<para/>
        /// 0x00004000 System integrity level.<para/>
        /// 0x00005000 Protected process integrity level.<para/>
        /// </summary>
        public _SID Sid;
    }


    /// <summary>
    /// The SYSTEM_AUDIT_CALLBACK_OBJECT_ACE structure defines an ACE for a SACL. 
    /// The ACE can audit access to an object or subobjects, such as property 
    /// sets or properties. The ACE contains a set of user rights, a GUID that 
    /// identifies the type of object or subobject, and a SID that identifies the 
    /// trustee for whom the system will audit access. The ACE also contains a 
    /// GUID and a set of flags that control inheritance of the ACE by child objects.
    /// </summary>
    public partial struct _SYSTEM_AUDIT_CALLBACK_OBJECT_ACE
    {
        /// <summary>
        /// An ACE_HEADER structure that specifies the size and type of ACE. 
        /// It contains flags that control inheritance of the ACE by child objects.
        /// </summary>
        public _ACE_HEADER Header;

        /// <summary>
        /// An ACCESS_MASK structure that specifies the user rights that cause 
        /// audit messages to be generated.
        /// </summary>
        public ACCESS_OBJECT_ACE_Mask Mask;

        /// <summary>
        /// A 32-bit unsigned integer that specifies a set of bit flags that 
        /// indicate whether the ObjectType and InheritedObjectType fields contain 
        /// valid data. This parameter can be one or more of the following values.
        /// </summary>
        public ACCESS_OBJECT_ACE_Flags Flags;

        /// <summary>
        /// A GUID that identifies a property set, property, extended right, 
        /// or type of child object. The purpose of this GUID depends on the 
        /// user rights specified in the Mask field. This field is valid only 
        /// if the ACE_OBJECT_TYPE_PRESENT bit is set in the Flags field. 
        /// Otherwise, the ObjectType field is ignored.
        /// </summary>
        [MarshalingCondition("IsObjectTypePresent")]
        public Guid ObjectType;

        /// <summary>
        /// A GUID that identifies the type of child object that can inherit the ACE. 
        /// Inheritance is also controlled by the inheritance flags in the ACE_HEADER, 
        /// as well as by any protection against inheritance placed on the child objects. 
        /// This field is valid only if the ACE_INHERITED_OBJECT_TYPE_PRESENT bit is 
        /// set in the Flags member. Otherwise, the InheritedObjectType field is ignored.
        /// </summary>
        [MarshalingCondition("IsInheritedObjectTypePresent")]
        public Guid InheritedObjectType;

        /// <summary>
        /// The SID of a trustee. The length of the SID MUST be a multiple of 4.
        /// </summary>
        public _SID Sid;

        /// <summary>
        /// Optional application data. The size of the application data is determined 
        /// by the AceSize field of the ACE_HEADER.
        /// </summary>
        [Size("@CalculateApplicationDataLength")]
        public byte[] ApplicationData;
    }

    /// <summary>
    /// The CLAIM_SECURITY_ATTRIBUTE_RELATIVE_V1 structure defines a resource attribute 
    /// that is defined in contiguous memory for persistence within a serialized Security Descriptor.
    /// </summary>
    public struct CLAIM_SECURITY_ATTRIBUTE_RELATIVE_V1
    {
        /// <summary>
        /// A DWORD value indicating an offset from the beginning of the CLAIM_SECURITY_ATTRIBUTE_RELATIVE_V1 
        /// structure to a string of Unicode characters containing the name of the claim security attribute. 
        /// The string MUST be at least 4 bytes in length.
        /// </summary>
        public uint Name;

        /// <summary>
        /// A union tag value indicating the type of information referred to by the Values member. The Values member 
        /// MUST be an array of offsets from the beginning of the CLAIM_SECURITY_ATTRIBUTE_RELATIVE_V1 structure to 
        /// the specified ValueType. 
        /// </summary>
        public ValueTypeEnum ValueType;

        /// <summary>
        /// Reserved. This member MUST be set to zero when sent and MUST be ignored when received.
        /// </summary>
        public ushort Reserved;

        /// <summary>
        ///  Flags
        /// </summary>
        public FlagEnum Flags;

        /// <summary>
        /// The number of values contained in the Values member.
        /// </summary>
        public uint ValueCount;

        /// <summary>
        /// An array of offsets from the beginning of the CLAIM_SECURITY_ATTRIBUTE_RELATIVE_V1 structure. Each offset indicates
        /// the location of a claim security attribute value of type specified in the ValueType member.
        /// </summary>
        [Size("ValueCount")]
        public uint[] Values;

        [Flags]
        public enum FlagEnum : uint
        {
            /// <summary>
            /// The CLAIM_SECURITY_ATTRIBUTE has been manually assigned.
            /// </summary>
            FCI_CLAIM_SECURITY_ATTRIBUTE_MANUAL = 0x00010000,

            /// <summary>
            /// The CLAIM_SECURITY_ATTRIBUTE has been determined by a central policy.
            /// </summary>
            FCI_CLAIM_SECURITY_ATTRIBUTE_POLICY_DERIVED = 0x00020000,

            /// <summary>
            /// This claim security attribute is not inherited across processes.
            /// </summary>
            CLAIM_SECURITY_ATTRIBUTE_NON_INHERITABLE = 0x00000001,

            /// <summary>
            /// The value of the claim security attribute is case sensitive. 
            /// This flag is valid for values that contain string types.
            /// </summary>
            CLAIM_SECURITY_ATTRIBUTE_VALUE_CASE_SENSITIVE = 0x00000002,

            /// <summary>
            /// Reserved for future use.
            /// </summary>
            CLAIM_SECURITY_ATTRIBUTE_USE_FOR_DENY_ONLY = 0x00000004,

            /// <summary>
            /// The claim security attribute is disabled by default.
            /// </summary>
            CLAIM_SECURITY_ATTRIBUTE_DISABLED_BY_DEFAULT = 0x00000008,

            /// <summary>
            /// Reserved for future use.
            /// </summary>
            CLAIM_SECURITY_ATTRIBUTE_DISABLED = 0x00000010,

            /// <summary>
            /// The claim security attribute is mandatory.
            /// </summary>
            CLAIM_SECURITY_ATTRIBUTE_MANDATORY = 0x00000020
        }

        public enum ValueTypeEnum : ushort
        {
            /// <summary>
            /// Values member refers to an array of offsets to LONG64 value(s).
            /// </summary>
            CLAIM_SECURITY_ATTRIBUTE_TYPE_INT64 = 0x0001,

            /// <summary>
            /// Values member refers to an array of offsets to ULONG64 value(s).
            /// </summary>
            CLAIM_SECURITY_ATTRIBUTE_TYPE_UINT64 = 0x0002,

            /// <summary>
            /// Values member refers to an array of offsets to Unicode character string value(s).
            /// </summary>
            CLAIM_SECURITY_ATTRIBUTE_TYPE_STRING = 0x0003,

            /// <summary>
            /// The Values member refers to an array of offsets to CLAIM_SECURITY_ATTRIBUTE_OCTET_STRING_RELATIVE
            /// value(s) where the OctetString value is a SID string.
            /// </summary>
            CLAIM_SECURITY_ATTRIBUTE_TYPE_SID = 0x0005,

            /// <summary>
            /// The Values member refers to an array of offsets to ULONG64 values where each element indicates a 
            /// Boolean value. The value 1 indicates TRUE, and the value 0 indicates FALSE.
            /// </summary>
            CLAIM_SECURITY_ATTRIBUTE_TYPE_BOOLEAN = 0x0006,

            /// <summary>
            /// Values member contains an array of CLAIM_SECURITY_ATTRIBUTE_OCTET_STRING_RELATIVE value(s) 
            /// as specified in section 2.4.10.2.
            /// </summary>
            CLAIM_SECURITY_ATTRIBUTE_TYPE_OCTET_STRING = 0x0010
        }
    }

    /// <summary>
    /// The SYSTEM_RESOURCE_ATTRIBUTE_ACE structure defines an ACE for the specification of a resource attribute associated with an object. 
    /// A SYSTEM_RESOURCE_ATTRIBUTE_ACE is used in conditional ACEs in specifying access or audit policy for the resource.
    /// </summary>
    public class SystemResourceAttributeAce
    {
        /// <summary>
        /// An ACE_HEADER structure that specifies the size and type of ACE. 
        /// It contains flags that control inheritance of the ACE by child objects.
        /// </summary>
        public _ACE_HEADER Header;

        /// <summary>
        /// An ACCESS_MASK that MUST be set to zero.
        /// </summary>
        public uint Mask;

        /// <summary>
        /// The SID corresponding to the Everyone SID (S-1-1-0) in binary form.
        /// </summary>
        public _SID Sid;

        /// <summary>
        /// Data describing a resource attribute type, name, and value(s). This data MUST be encoded in 
        /// CLAIM_SECURITY_ATTRIBUTE_RELATIVE_V1 format as described in section 2.4.10.1
        /// </summary>
        public CLAIM_SECURITY_ATTRIBUTE_RELATIVE_V1 AttributeData;

        /// <summary>
        /// a string of Unicode characters containing the name of the claim security attribute. 
        /// The string MUST be at least 4 bytes in length.
        /// </summary>
        public string AttributeName;

        /// <summary>
        /// Binary form of this structure.
        /// </summary>
        public byte[] BinaryForm;


        // The following values are mutual exclusive:
        private long[] Int64Values;
        private ulong[] Uint64Values;
        private string[] StringValues;

        // public _SID[] SidValues;
        // public bool[] BooleanValues;
        // Octet String

        public SystemResourceAttributeAce(string attributeName, long[] values)
            : this(attributeName)
        {
            if (values == null)
            {
                throw new ArgumentException("values cannot be null.");
            }
            AttributeData.ValueCount = (uint)values.Length;
            AttributeData.Values = new uint[values.Length];
            Int64Values = values;
            AttributeData.ValueType = CLAIM_SECURITY_ATTRIBUTE_RELATIVE_V1.ValueTypeEnum.CLAIM_SECURITY_ATTRIBUTE_TYPE_INT64;
            BinaryForm = this.ToBytes();
        }

        public SystemResourceAttributeAce(string attributeName, ulong[] values)
            : this(attributeName)
        {
            if (values == null)
            {
                throw new ArgumentException("values cannot be null.");
            }
            AttributeData.ValueCount = (uint)values.Length;
            AttributeData.Values = new uint[values.Length];
            Uint64Values = values;
            AttributeData.ValueType = CLAIM_SECURITY_ATTRIBUTE_RELATIVE_V1.ValueTypeEnum.CLAIM_SECURITY_ATTRIBUTE_TYPE_UINT64;
            BinaryForm = this.ToBytes();
        }

        public SystemResourceAttributeAce(string attributeName, string[] values)
            : this(attributeName)
        {
            if (values == null)
            {
                throw new ArgumentException("values cannot be null.");
            }
            AttributeData.ValueCount = (uint)values.Length;
            AttributeData.Values = new uint[values.Length];
            StringValues = values;
            AttributeData.ValueType = CLAIM_SECURITY_ATTRIBUTE_RELATIVE_V1.ValueTypeEnum.CLAIM_SECURITY_ATTRIBUTE_TYPE_STRING;
            BinaryForm = this.ToBytes();
        }

        private SystemResourceAttributeAce(string attributeName)
        {
            if (attributeName.Length < 2)
            {
                throw new ArgumentException("attributeName must be at least 4 bytes in length.");
            }

            AttributeName = attributeName;
            Sid = DtypUtility.GetWellKnownSid(WellKnownSid.EVERYONE, null);
            AttributeData.Flags = CLAIM_SECURITY_ATTRIBUTE_RELATIVE_V1.FlagEnum.CLAIM_SECURITY_ATTRIBUTE_MANDATORY 
                | CLAIM_SECURITY_ATTRIBUTE_RELATIVE_V1.FlagEnum.FCI_CLAIM_SECURITY_ATTRIBUTE_MANUAL;
            Mask = 0;
            Header = new _ACE_HEADER
            {
                AceFlags = ACE_FLAGS.CONTAINER_INHERIT_ACE | ACE_FLAGS.OBJECT_INHERIT_ACE,
                AceType = ACE_TYPE.SYSTEM_RESOURCE_ATTRIBUTE_ACE_TYPE,
                // Size would be filled later
            };
        }

        private SystemResourceAttributeAce()
        {

        }

        private byte[] ToBytes()
        {
            List<byte> result = new List<byte>();

            int curPos = TypeMarshal.GetBlockMemorySize<CLAIM_SECURITY_ATTRIBUTE_RELATIVE_V1>(AttributeData);
            AttributeData.Name = (uint)curPos;

            byte[] attributeNameBytes = Encoding.Unicode.GetBytes(AttributeName + "\0");
            curPos += attributeNameBytes.Length;

            byte[] valueBytes = null;
            if (Int64Values != null)
            {
                Debug.Assert(AttributeData.ValueType == CLAIM_SECURITY_ATTRIBUTE_RELATIVE_V1.ValueTypeEnum.CLAIM_SECURITY_ATTRIBUTE_TYPE_INT64, "ValueType should be equal to the input");
                Debug.Assert(AttributeData.ValueCount == Int64Values.Length, "ValueCount should be equal to the input length");
                Debug.Assert(AttributeData.ValueCount == AttributeData.Values.Length, "ValueCount should be equal to the value length");
                for (int i = 0; i < AttributeData.ValueCount; ++i)
                {
                    AttributeData.Values[i] = (uint)(curPos + (uint)(8 * i));
                }
                valueBytes = new byte[AttributeData.ValueCount * 8];
                Buffer.BlockCopy(Int64Values, 0, valueBytes, 0, valueBytes.Length);
            }
            else if (Uint64Values != null)
            {
                Debug.Assert(AttributeData.ValueType == CLAIM_SECURITY_ATTRIBUTE_RELATIVE_V1.ValueTypeEnum.CLAIM_SECURITY_ATTRIBUTE_TYPE_UINT64, "ValueType should be equal to the input");
                Debug.Assert(AttributeData.ValueCount == Uint64Values.Length, "ValueCount should be equal to the input length");
                Debug.Assert(AttributeData.ValueCount == AttributeData.Values.Length, "ValueCount should be equal to the value length");
                for (int i = 0; i < AttributeData.ValueCount; ++i)
                {
                    AttributeData.Values[i] = (uint)(curPos + (uint)(8 * i));
                }
                valueBytes = new byte[AttributeData.ValueCount * 8];
                Buffer.BlockCopy(Uint64Values, 0, valueBytes, 0, valueBytes.Length);
            }
            else if (StringValues != null)
            {
                Debug.Assert(AttributeData.ValueType == CLAIM_SECURITY_ATTRIBUTE_RELATIVE_V1.ValueTypeEnum.CLAIM_SECURITY_ATTRIBUTE_TYPE_STRING, "ValueType should be equal to the input");
                Debug.Assert(AttributeData.ValueCount == StringValues.Length, "ValueCount should be equal to the input length");
                Debug.Assert(AttributeData.ValueCount == AttributeData.Values.Length, "ValueCount should be equal to the value length");

                string tmp = string.Empty;
                for (int i = 0; i < AttributeData.ValueCount; ++i)
                {
                    AttributeData.Values[i] = (uint)curPos;
                    curPos += StringValues[i].Length * 2 + 2; // Null-terminated Unicode
                    tmp += (StringValues[i] + "\0");
                }
                valueBytes = Encoding.Unicode.GetBytes(tmp);
            }
            else
            {
                Debug.Fail("Should not reach here.");
            }

            result.AddRange(TypeMarshal.ToBytes<CLAIM_SECURITY_ATTRIBUTE_RELATIVE_V1>(AttributeData));
            result.AddRange(attributeNameBytes);
            result.AddRange(valueBytes);

            int dataLen = result.Count;

            // This padding seems required. But it's not documented. Will check windows code and file TDI later.
            int paddingLen = BlockAlign(dataLen, 4) - dataLen;
            result.AddRange(new byte[paddingLen]);

            //  Header (4 bytes) + Mask (4 bytes) + Sid (12 bytes) + AttributeData + Padding
            Header.AceSize = (ushort)(4 + 4 + 12 + dataLen + paddingLen);

            // Reverse order
            result.InsertRange(0, TypeMarshal.ToBytes<_SID>(Sid));
            result.InsertRange(0, TypeMarshal.ToBytes<uint>(Mask));
            result.InsertRange(0, TypeMarshal.ToBytes<_ACE_HEADER>(Header));

            return result.ToArray();
        }

        public static SystemResourceAttributeAce FromBytes(byte[] data)
        {
            SystemResourceAttributeAce retVal = new SystemResourceAttributeAce();
            retVal.BinaryForm = data;

            int offset = 0;
            retVal.Header = TypeMarshal.ToStruct<_ACE_HEADER>(data, ref offset);
            retVal.Mask = TypeMarshal.ToStruct<uint>(data, ref offset);
            retVal.Sid = TypeMarshal.ToStruct<_SID>(data, ref offset);
            int attributeDataOffset = offset;
            retVal.AttributeData = TypeMarshal.ToStruct<CLAIM_SECURITY_ATTRIBUTE_RELATIVE_V1>(data, ref offset);
            Debug.Assert(retVal.AttributeData.Name == (offset - attributeDataOffset));

            int strLen = 0;
            for (int i = offset; i < data.Length - 2; ++i)
            {
                if (data[i] == 0 && data[i + 1] == 0)
                {
                    strLen = i - offset;
                    break;
                }
            }
            retVal.AttributeName = Encoding.Unicode.GetString(data, offset, strLen);
            Debug.Assert(retVal.AttributeName.Length >= 2, "The string MUST be at least 4 bytes in length.");
            offset += strLen + 2; // Null-terminator

            switch (retVal.AttributeData.ValueType)
            {
                case CLAIM_SECURITY_ATTRIBUTE_RELATIVE_V1.ValueTypeEnum.CLAIM_SECURITY_ATTRIBUTE_TYPE_UINT64:
                    retVal.Uint64Values = new ulong[retVal.AttributeData.ValueCount];
                    for (int i = 0; i < retVal.AttributeData.ValueCount; ++i)
                    {
                        Debug.Assert(retVal.AttributeData.Values[i] == offset - attributeDataOffset);
                        retVal.Uint64Values[i] = TypeMarshal.ToStruct<ulong>(data, ref offset);
                    }
                    break;
                case CLAIM_SECURITY_ATTRIBUTE_RELATIVE_V1.ValueTypeEnum.CLAIM_SECURITY_ATTRIBUTE_TYPE_INT64:
                    retVal.Int64Values = new long[retVal.AttributeData.ValueCount];
                    for (int i = 0; i < retVal.AttributeData.ValueCount; ++i)
                    {
                        Debug.Assert(retVal.AttributeData.Values[i] == offset - attributeDataOffset);
                        retVal.Int64Values[i] = TypeMarshal.ToStruct<long>(data, ref offset);
                    }
                    break;
                case CLAIM_SECURITY_ATTRIBUTE_RELATIVE_V1.ValueTypeEnum.CLAIM_SECURITY_ATTRIBUTE_TYPE_STRING:
                    retVal.StringValues = new string[retVal.AttributeData.ValueCount];
                    for (int i = 0; i < retVal.AttributeData.ValueCount; ++i)
                    {
                        Debug.Assert(retVal.AttributeData.Values[i] == offset - attributeDataOffset);
                        for (int j = offset; j < data.Length - 2; ++j)
                        {
                            if (data[j] == 0 && data[j + 1] == 0)
                            {
                                strLen = j - offset;
                                break;
                            }
                        }
                        retVal.StringValues[i] = Encoding.Unicode.GetString(data, offset, strLen);
                        offset += strLen + 2;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Attribute data value type is not implemented");
            }
            Debug.Assert(BlockAlign(offset, 4) == data.Length - 1, "All bytes should be consumed.");
            return retVal;
        }

        private static int BlockAlign(int value, int boundary)
        {
            return (value + (boundary - 1)) & -(boundary);
        }
    }

    /// <summary>
    /// The SYSTEM_SCOPED_POLICY_ID_ACE structure defines an ACE for the purpose of applying a 
    /// central access policy to the resource.
    /// </summary>
    public struct _SYSTEM_SCOPED_POLICY_ID_ACE
    {
        /// <summary>
        /// An ACE_HEADER structure that specifies the size and type of ACE. 
        /// It contains flags that control inheritance of the ACE by child objects.
        /// </summary>
        public _ACE_HEADER Header;

        /// <summary>
        /// An ACCESS_MASK that MUST be set to zero.
        /// </summary>
        public uint Mask;

        /// <summary>
        /// A SID that identifies a central access policy.
        /// </summary>
        public _SID Sid;
    }

    /// <summary>
    /// The access control list (ACL), is used to specify a list of 
    /// individual access control entries (ACEs).
    /// </summary>
    public struct _ACL
    {
        /// <summary>
        /// An unsigned 8-bit value that specifies the revision of the ACL structure. 
        /// This field MUST be set to one of the following values.<para/>
        /// When set to 0x02, only ACE types 0x00, 0x01, 0x02, and 0x03 should 
        /// be present in the ACL. An AceType of 0x11 is used for SACLs but 
        /// not for DACLs. For more information about ACE types, see section 2.4.4.1.<para/>
        /// When set to 0x04, ACE types 0x05, 0x06, 0x07, and 0x08 are allowed. 
        /// ACLs of revision 0x04 should be applied only to directory service 
        /// objects. An AceType of 0x11 is used for SACLs but not for DACLs.<para/>
        /// </summary>
        public byte AclRevision;

        /// <summary>
        /// An unsigned 8-bit value. This field is reserved and MUST be set to zero.
        /// </summary>
        public byte Sbz1;

        /// <summary>
        /// An unsigned 16-bit integer that specifies the size, in bytes, 
        /// of the complete ACL, including all ACEs.
        /// </summary>
        public ushort AclSize;

        /// <summary>
        /// An unsigned 16-bit integer that specifies the count of the number 
        /// of ACE records in the ACL.
        /// </summary>
        public ushort AceCount;

        /// <summary>
        /// An unsigned 16-bit integer. This field is reserved and MUST be set to zero.
        /// </summary>
        public ushort Sbz2;

        /// <summary>
        /// An ordered list of ACEs.
        /// </summary>
        [Size("AceCount")]
        public object[] Aces;
    }

    /// <summary>
    /// The access control list (ACL), is used to specify a list of 
    /// individual access control entries (ACEs).
    /// </summary>
    public struct _RPC_ACL
    {
        /// <summary>
        /// An unsigned 8-bit value that specifies the revision of the ACL structure. 
        /// This field MUST be set to one of the following values.<para/>
        /// When set to 0x02, only ACE types 0x00, 0x01, 0x02, and 0x03 should 
        /// be present in the ACL. An AceType of 0x11 is used for SACLs but 
        /// not for DACLs. For more information about ACE types, see section 2.4.4.1.<para/>
        /// When set to 0x04, ACE types 0x05, 0x06, 0x07, and 0x08 are allowed. 
        /// ACLs of revision 0x04 should be applied only to directory service 
        /// objects. An AceType of 0x11 is used for SACLs but not for DACLs.<para/>
        /// </summary>
        public byte AclRevision;

        /// <summary>
        /// An unsigned 8-bit value. This field is reserved and MUST be set to zero.
        /// </summary>
        public byte Sbz1;

        /// <summary>
        /// An unsigned 16-bit integer that specifies the size, in bytes, 
        /// of the complete ACL, including all ACEs.
        /// </summary>
        public ushort AclSize;

        /// <summary>
        /// An unsigned 16-bit integer that specifies the count of the number 
        /// of ACE records in the ACL.
        /// </summary>
        public ushort AceCount;

        /// <summary>
        /// An unsigned 16-bit integer. This field is reserved and MUST be set to zero.
        /// </summary>
        public ushort Sbz2;

        /// <summary>
        /// An ordered list of ACEs.
        /// </summary>
        [Size("AceCount")]
        public object[] Aces;
    }
}
