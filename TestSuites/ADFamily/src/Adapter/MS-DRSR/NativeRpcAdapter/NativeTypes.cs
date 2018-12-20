// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.Messages;
using Microsoft.Protocols.TestTools.Messages.Marshaling;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr.NativeTypes
{
    /// <summary>
    ///  PAS_DATA is a concrete type for a list of attributes
    ///  in a partial attribute set.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\14e0e482-8f1d-4fa5-89de-d9fd7f98b10d.xml
    //  </remarks>
    public partial struct PAS_DATA
    {

        /// <summary>
        ///  The version of the structure; MUST be 1.
        /// </summary>
        [CLSCompliant(false)]
        public version_Values version;

        /// <summary>
        ///  The size of the entire structure.
        /// </summary>
        [CLSCompliant(false)]
        public uint size;

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public flag_Values flag;

        /// <summary>
        ///  A PARTIAL_ATTR_VECTOR_V1_EXT structure, which specifies
        ///  the additional attributes being requested as part of
        ///  a PAS cycle.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] pas;
    }

    /// <summary>
    /// The version of PAS_DATA.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum version_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 1,
    }

    /// <summary>
    /// The flag of PAS_DATA.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum flag_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The INT64 type is an 8-byte integer in little-endian
    ///  form. See [MS-DTYP] section for its definition.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\1c3855ef-b058-4248-866f-70aa740b5a7b.xml
    //  </remarks>
    public partial struct INT64
    {

        /// <summary>
        ///  A 64-bit signed number in little-endian byte order.
        /// </summary>
        public long int64Value;
    }

    /// <summary>
    ///  Each Record is represented in the following manner.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\2a16b808-322f-433a-b5a6-71eefba82b5a.xml
    //  </remarks>
    public partial struct Record
    {

        /// <summary>
        ///  The length, in bytes, of the entire record.
        /// </summary>
        [CLSCompliant(false)]
        public uint RecordLen;

        /// <summary>
        ///  Individual bit flags that control how the forest trust
        ///  information in this record can be used.
        /// </summary>
        [CLSCompliant(false)]
        public uint Flags;

        /// <summary>
        ///  A 64-bit time-stamp value that indicates when this entry
        ///  was created.
        /// </summary>
        public INT64 Timestamp;

        /// <summary>
        ///  An 8-bit value that specifies the type of record contained
        ///  in this specific entry. The allowed values are specified
        ///  in section.
        /// </summary>
        public byte RecordType;

        /// <summary>
        ///  A variable length, type-specific record, depending on
        ///  the RecordType value, that contains the specific type
        ///  of data about the forest trust relationship.IMPORTANT
        ///  NOTE: The type-specific ForestTrustData record is not
        ///  necessarily aligned to a 32-bit boundary. Each record
        ///  starts at the byte following the RecordType field.
        /// </summary>
        public object ForestTrustData;
    }

    /// <summary>
    ///  The DRS_EXTENSIONS_INT structure is a concrete type
    ///  for structured capabilities information used in version
    ///  negotiation.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\3ee529b1-23db-4996-948a-042f04998e91.xml
    //  </remarks>
    public partial struct DRS_EXTENSIONS_INT
    {

        /// <summary>
        ///  The count of bytes in the fields dwFlags through dwReplEpoch,
        ///  inclusive. This field allows extended versions of this
        ///  structure to carry more information in future product
        ///  versions.
        /// </summary>
        [CLSCompliant(false)]
        public uint cb;

        /// <summary>
        ///  The dwFlags field contains individual bit flags that
        ///  describe the capabilities of the DC that produced the
        ///  DRS_EXTENSIONS_INT structure.Client callers set dwFlags
        ///  to zero.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwFlags;

        /// <summary>
        ///  A GUID. The objectGUID of the siteobject of which the
        ///  DC'sDSA object is a descendant. For non-DC client callers,
        ///  this field SHOULD be set to zero.
        /// </summary>
        public Guid SiteObjGuid;

        /// <summary>
        ///  A 32-bit, signed integer value that specifies the process
        ///  identifier of the client. This is for informational
        ///  and debugging purposes only. The assignment of this
        ///  field is implementation-specific. This field contains
        ///  the process ID of the client.
        /// </summary>
        [CLSCompliant(false)]
        public int Pid;

        /// <summary>
        ///  A 32-bit, unsigned integer value that specifies the
        ///  replication epoch.. This value is set to zero by all
        ///  client callers. The server sets this value by assigning
        ///  the value of msDS-ReplicationEpoch from its nTDSDSAobject.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwReplEpoch;

        /// <summary>
        ///  An extension of the dwFlags field that contains individual
        ///  bit flags that describe the capabilities of the DC
        ///  that produced the DRS_EXTENSIONS_INT structure. For
        ///  non-DC client callers, no bits SHOULD be set.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwFlagsExt;

        /// <summary>
        ///  A GUID. The objectGUID of the config NC object.
        /// </summary>
        public Guid ConfigObjGUID;

        /// <summary>
        /// A mask for the dwFlagsExt field that contains individual bit flags. 
        /// </summary>
        public uint dwExtCaps;

    }

    /// <summary>
    ///  The INT32 type is a 4-byte integer in little-endian
    ///  form. See [MS-DTYP] section for its definition.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\41605573-51bc-49c2-abcd-d8d4c31ab1f5.xml
    //  </remarks>
    public partial struct INT32
    {

        /// <summary>
        ///  A 32-bit signed number in little-endian byte order.
        /// </summary>
        [CLSCompliant(false)]
        public int intValue;
    }

    /// <summary>
    ///  FOREST_TRUST_RECORD_TYPE is a concrete type for specifying
    ///  the type of record contained in a forest trust information
    ///  (FOREST_TRUST_INFORMATION) entry. The allowed values
    ///  are specified by the following enumerated list.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\5514b72b-8452-446a-aa64-abb35536baca.xml
    //  </remarks>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    //[Flags()]
    public enum FOREST_TRUST_RECORD_TYPE
    {

        /// <summary>
        ///  ForestTrustTopLevelName constant.
        /// </summary>
        ForestTrustTopLevelName = 0,

        /// <summary>
        ///  ForestTrustTopLevelNameEx constant.
        /// </summary>
        ForestTrustTopLevelNameEx = 1,

        /// <summary>
        ///  ForestTrustDomainInfo constant.
        /// </summary>
        ForestTrustDomainInfo = 2,
    }

    /// <summary>
    ///  FOREST_TRUST_INFORMATION is a concrete type for state
    ///  information about trust relationships with other forests.
    ///  This data is stored in objects of classtrustedDomain
    ///  in the domain NCreplica of the forestroot domain. Specifically,
    ///  the msDS-TrustForestTrustInfoattribute on such objects
    ///  contains information about the trusted forest or realm.
    ///  The structure of the information contained in this
    ///  attribute is represented in the following manner.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\642c0d17-4f3b-4752-a9a3-464b080bf0b6.xml
    //  </remarks>
    public partial struct FOREST_TRUST_INFORMATION
    {

        /// <summary>
        ///  The version of the data structure. The only supported
        ///  version of the data structure is 1.
        /// </summary>
        [CLSCompliant(false)]
        public uint Version;

        /// <summary>
        ///  The number of records present in the data structure.
        /// </summary>
        [CLSCompliant(false)]
        public uint RecordCount;

        /// <summary>
        ///  Variable-length records that each contain a specific
        ///  type of data about the forest trust relationship. Records
        ///  are not necessarily aligned to 32-bit boundaries. Each
        ///  record starts at the next byte after the previous record
        ///  ends.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public Record[] Records;
    }

    /// <summary>
    ///  The REVERSE_MEMBERSHIP_OPERATION_TYPE enumeration defines
    ///  the type of reverse membership evaluation.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\66e09464-e143-47e4-8626-f10772ee6882.xml
    //  </remarks>
    [Flags()]
    public enum REVERSE_MEMBERSHIP_OPERATION_TYPE
    {

        /// <summary>
        ///  Non-transitive membership in groups that are confined
        ///  to a given domain, excluding built-in groups and domain-local
        ///  groups.
        /// </summary>
        RevMembGetGroupsForUser = 1,

        /// <summary>
        ///  Non-transitive membership in domain-local groups that
        ///  are confined to a given domain.
        /// </summary>
        RevMembGetAliasMembership,

        /// <summary>
        ///  Transitive membership in all account groups in a given
        ///  domain, excluding built-in groups.
        /// </summary>
        RevMembGetAccountGroups,

        /// <summary>
        ///  Transitive membership in all domain-local groups in
        ///  a given domain, excluding built-in groups.
        /// </summary>
        RevMembGetResourceGroups,

        /// <summary>
        ///  Transitive membership in all universal groups, excluding
        ///  built-in groups.
        /// </summary>
        RevMembGetUniversalGroups,

        /// <summary>
        ///  Transitive closure of members of a group based on the
        ///  information present in the server's NC replicas, including
        ///  the primary group.
        /// </summary>
        GroupMembersTransitive,

        /// <summary>
        ///  Non-transitive membership in global groups, excluding
        ///  built-in groups.
        /// </summary>
        RevMembGlobalGroupsNonTransitive,
    }

    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum DS_NAME_ERROR : uint
    {
        DS_NAME_NO_ERROR = 0,
        DS_NAME_ERROR_RESOLVING,
        DS_NAME_ERROR_NOT_FOUND,
        DS_NAME_ERROR_NOT_UNIQUE,
        DS_NAME_ERROR_NO_MAPPING,
        DS_NAME_ERROR_DOMAIN_ONLY,
        DS_NAME_ERROR_TRUST_REFERRAL,
        DS_NAME_ERROR_IS_SID_HISTORY_UNKNOWN = 0xfffffff2,
        DS_NAME_ERROR_IS_SID_HISTORY_ALIAS,
        DS_NAME_ERROR_IS_SID_HISTORY_GROUP,
        DS_NAME_ERROR_IS_SID_HISTORY_USER,
        DS_NAME_ERROR_IS_SID_UNKNOWN,
        DS_NAME_ERROR_IS_SID_ALIAS,
        DS_NAME_ERROR_IS_SID_GROUP,
        DS_NAME_ERROR_IS_SID_USER,
        DS_NAME_ERROR_SCHEMA_GUID_CONTROL_RIGHT,
        DS_NAME_ERROR_SCHEMA_GUID_CLASS,
        DS_NAME_ERROR_SCHEMA_GUID_ATTR_SET,
        DS_NAME_ERROR_SCHEMA_GUID_ATTR,
        DS_NAME_ERROR_SCHEMA_GUID_NOT_FOUND,
        DS_NAME_ERROR_IS_FPO
    }

    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum SAM_ACCOUNT_TYPE : uint
    {
        SAM_DOMAIN_OBJECT = 0x0,
        SAM_GROUP_OBJECT = 0x10000000,
        SAM_NON_SECURITY_GROUP_OBJECT = 0x10000001,
        SAM_ALIAS_OBJECT = 0x20000000,
        SAM_NON_SECURITY_ALIAS_OBJECT = 0x20000001,
        SAM_USER_OBJECT = 0x30000000,
        SAM_NORMAL_USER_ACCOUNT = 0x30000000,
        SAM_MACHINE_ACCOUNT = 0x30000001,
        SAM_TRUST_ACCOUNT = 0x30000002,
        SAM_APP_BASIC_GROUP = 0x40000000,
        SAM_APP_QUERY_GROUP = 0x40000001,
        SAM_ACCOUNT_TYPE_MAX = 0x7fffffff
    }

    /// <summary>
    ///  The DS_NAME_FORMAT enumeration describes the format
    ///  of a name sent to or received from the IDL_DRSCrackNames
    ///  method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\73c73cf2-0824-4d65-97f4-f56244f3e8a6.xml
    //  </remarks>
    //[Flags()]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum DS_NAME_FORMAT
    {

        /// <summary>
        ///  The server looks up the name by using the algorithm
        ///  specified in the LookupUnknownName procedure.
        /// </summary>
        DS_UNKNOWN_NAME = 0,

        /// <summary>
        ///  A distinguished name.
        /// </summary>
        DS_FQDN_1779_NAME = 1,

        /// <summary>
        ///  windows_nt_4_0 (and prior) name format. The account
        ///  name is in the format domain\user and the domain-only
        ///  name is in the format domain\.
        /// </summary>
        DS_NT4_ACCOUNT_NAME = 2,

        /// <summary>
        ///  A user-friendly display name.
        /// </summary>
        DS_DISPLAY_NAME = 3,

        /// <summary>
        ///  Curly braced string representation of an objectGUID.
        ///  The format of the string representation is specified
        ///  in [MS-DTYP] section.
        /// </summary>
        DS_UNIQUE_ID_NAME = 6,

        /// <summary>
        ///  A canonical name.
        /// </summary>
        DS_CANONICAL_NAME = 7,

        /// <summary>
        ///  User principal name.
        /// </summary>
        DS_USER_PRINCIPAL_NAME = 8,

        /// <summary>
        ///  Same as DS_CANONICAL_NAME except that the rightmost
        ///  forward slash (/) is replaced with a newline character
        ///  (\n).
        /// </summary>
        DS_CANONICAL_NAME_EX = 9,

        /// <summary>
        ///  Service principal name (SPN).
        /// </summary>
        DS_SERVICE_PRINCIPAL_NAME = 10,

        /// <summary>
        ///  String representation of a SID (as specified in [MS-DTYP]
        ///  section ).
        /// </summary>
        DS_SID_OR_SID_HISTORY_NAME = 11,

        /// <summary>
        ///  Not supported.
        /// </summary>
        DS_DNS_DOMAIN_NAME = 12,

        /// <summary>
        /// Invalid name. Added to support features that doesn't require a name.
        /// </summary>
        DS_INVALID_NAME
    }

    /// <summary>
    ///  The ENCRYPTED_PAYLOAD packet is the concrete type for
    ///  a value of an encrypted attribute.typedef struct {
    ///     UCHAR Salt[16];    ULONG CheckSum;    UCHAR EncryptedData[];
    ///  } ENCRYPTED_PAYLOAD;
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\7b60d2b3-5bb1-49aa-aefc-fa887e683977.xml
    //  </remarks>
    public partial struct ENCRYPTED_PAYLOAD
    {

        /// <summary>
        ///  A 128-bit randomly generated value.
        /// </summary>
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Salt;

        /// <summary>
        ///  A 32-bit CRC32 checksum of the data that is encrypted
        ///  along with the data.
        /// </summary>
        [CLSCompliant(false)]
        public uint CheckSum;

        /// <summary>
        ///  A variable-length byte array that represents the encrypted
        ///  value.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] EncryptedData;
    }

    /// <summary>
    ///  The SYNTAX_ADDRESS packet is the concrete type for a
    ///  sequence of bytes or Unicode characters.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\7df24a29-d2e4-4f9e-b55c-abbd72131422.xml
    //  </remarks>
    public partial struct SYNTAX_ADDRESS
    {

        /// <summary>
        ///  The size of the entire structure (including this field),
        ///  in bytes.
        /// </summary>
        [CLSCompliant(false)]
        public uint dataLen;

        /// <summary>
        ///  The byte or character data.
        /// </summary>
        [Size("dataLen-4")]
        public byte[] byteVal;
    }

    /// <summary>
    ///  The SYNTAX_DISTNAME_BINARY packet is the concrete type
    ///  for a combination of a DSNAME and a binary or character
    ///  data buffer.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\8eefc5ab-6d22-48b4-bea1-63b53a81a3a9.xml
    //  </remarks>
    public partial struct SYNTAX_DISTNAME_BINARY
    {

        /// <summary>
        ///  The length of the structure, in bytes, up to and including
        ///  the field StringName.
        /// </summary>
        [CLSCompliant(false)]
        public uint structLen;

        /// <summary>
        ///  The number of bytes in the Sid field used to represent
        ///  the object's objectSidattribute value. Zero indicates
        ///  that the SYNTAX_DISTNAME_BINARY does not identify the
        ///  objectSid value of the directory object.
        /// </summary>
        [CLSCompliant(false)]
        public uint SidLen;

        /// <summary>
        ///  The value of the object'sobjectGUIDattribute specified
        ///  as a GUID structure, which is defined in [MS-DTYP]
        ///  section. If the values for all fields in the GUID
        ///  structure are zero, this indicates that the SYNTAX_DISTNAME_BINARY
        ///  does not identify the objectGUID value of the directory
        ///  object.
        /// </summary>
        public Guid Guid;

        /// <summary>
        ///  The value of the object'sobjectSidattribute, its security
        ///  identifier (see [MS-SECO] section ), specified as a
        ///  SID structure, which is defined in [MS-DTYP] section
        ///  . The size of this field is exactly 28 bytes, regardless
        ///  of the value of SidLen, which specifies how many bytes
        ///  in this field are used.
        /// </summary>
        public NT4SID Sid;

        /// <summary>
        ///  The number of characters in the StringName field, not
        ///  including the terminating null character, used to represent
        ///  the object's distinguishedNameattribute value. Zero
        ///  indicates that the SYNTAX_DISTNAME_BINARY does not
        ///  identify the distinguishedName value of the directory
        ///  object.
        /// </summary>
        [CLSCompliant(false)]
        public uint NameLen;

        /// <summary>
        ///  The null-terminated Unicode value of the object'sdistinguishedNameattribute,
        ///  as specified in [MS-ADTS] section. This field always
        ///  contains at least one character: the terminating null
        ///  character. Each Unicode value is encoded as 2 bytes.
        ///  The byte ordering is little-endian.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        [CLSCompliant(false)]
        public ushort[] StringName;

        /// <summary>
        ///  The padding (bytes with value zero) to align the field
        ///  dataLen at a double word boundary.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] Padding;

        /// <summary>
        ///  The length of the remaining structure, including this
        ///  field, in bytes.
        /// </summary>
        [CLSCompliant(false)]
        public uint dataLen;

        /// <summary>
        ///  An array of bytes.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] byteVal;
    }

    /// <summary>
    ///  The nonreplicated, multivalued attributerepsTo is an
    ///  optional attribute on the root object of every NC replica.
    ///  It is stored with the structure of the REPS_TOconcrete
    ///  type, which is represented by the following diagram.
    ///  This attribute exposes a structure that controls the
    ///  server-to-server replication implementation—specifically,
    ///  the options for outbound replication of changes.This
    ///  structure is used for both repsTo values and repsFrom
    ///  values. Many of the fields are unused in repsTo values,
    ///  and some of the field names are misleading (for example,
    ///  timeLastSuccess).
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\b422aa87-7d07-4527-b070-c5d719696c43.xml
    //  </remarks>
    public partial struct REPS_TO
    {

        /// <summary>
        ///  The version of this structure. The value must be 1 or
        ///  2.windows_2000_server and windows_server_2003DCs have
        ///  value 1 in the dwVersion field. windows_server_2008
        ///  and windows_server_2008_r2DCs have value 2 in the dwVersion
        ///  field.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwVersion;

        /// <summary>
        ///  The total number of bytes in the REPS_TO structure.
        /// </summary>
        [CLSCompliant(false)]
        public uint cb;

        /// <summary>
        ///  An unsigned long that contains the number of unsuccessful
        ///  consecutive attempts to send a replication notification
        ///  to the DC identified by uuidDsaObj.
        /// </summary>
        [CLSCompliant(false)]
        public uint cConsecutiveFailures;

        /// <summary>
        ///  A DSTIME structure that contains the time when the last
        ///  successful replication notification to the DC identified
        ///  by uuidDsaObj was sent, or 0 if no replication notification
        ///  has been sent successfully.
        /// </summary>
        public long timeLastSuccess;

        /// <summary>
        ///  A DSTIME structure that contains the last time when
        ///  an attempt was made to send a replication notification
        ///  to the DC identified by uuidDsaObj, or 0 if no attempt
        ///  has been made.
        /// </summary>
        public long timeLastAttempt;

        /// <summary>
        ///  An unsigned long that contains the result of the last
        ///  attempt to send a replication notification to the DC
        ///  identified by uuidDsaObj. It has a value 0 if the last
        ///  notification was sent successfully, or a Windows error
        ///  code otherwise.
        /// </summary>
        [CLSCompliant(false)]
        public uint ulResultLastAttempt;

        /// <summary>
        ///  The offset from the start of the structure to a location
        ///  in the data field, specifying the start of a structure
        ///  that contains a NetworkAddress. If dwVersion is 1,
        ///  it is an MTX_ADDR structure. If dwVersion is 2, it
        ///  is a DSA_RPC_INST structure.
        /// </summary>
        [CLSCompliant(false)]
        public uint cbOtherDraOffset;

        /// <summary>
        ///  The size of the structure pointed to by cbOtherDraOffset.
        /// </summary>
        [CLSCompliant(false)]
        public uint cbOtherDra;

        /// <summary>
        ///  A ULONG. This set contains DRS_WRIT_REP (section) if
        ///  this replica is writable. This set never contains any
        ///  other elements.
        /// </summary>
        [CLSCompliant(false)]
        public uint ulReplicaFlags;

        /// <summary>
        ///  A REPLTIMES structure. Not used.
        /// </summary>
        public REPLTIMES rtSchedule;

        /// <summary>
        ///  A USN_VECTOR structure. Not used.
        /// </summary>
        public USN_VECTOR usnVec;

        /// <summary>
        ///  A GUID. A DSA GUID that identifies a DC.
        /// </summary>
        public Guid uuidDsaObj;

        /// <summary>
        ///  A GUID. Not used.
        /// </summary>
        public Guid uuidInvocId;

        /// <summary>
        ///  A GUID. Not used.
        /// </summary>
        public Guid uuidTransportObj;

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public dwReserved_Values dwReserved;

        /// <summary>
        ///  Not used.
        /// </summary>
        [CLSCompliant(false)]
        public uint cbPasDataOffset;

        /// <summary>
        ///  The storage for the rest of the structure. The structure
        ///  pointed to by cbOtherDraOffset is packed into this
        ///  field and can be referenced using the offset.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] data;
    }

    /// <summary>
    /// The reserved of REPS_TO.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum dwReserved_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The DRS_COMP_ALG_TYPE enumeration is a concrete type
    ///  for identifying a compression algorithm.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\bb303730-0667-49f0-b117-288404c4b4cb.xml
    //  </remarks>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [Flags()]
    public enum DRS_COMP_ALG_TYPE
    {

        /// <summary>
        ///  No compression.
        /// </summary>
        DRS_COMP_ALG_NONE = 0,

        /// <summary>
        ///  Unused. MUST not be used.
        /// </summary>
        DRS_COMP_ALG_UNUSED = 1,

        /// <summary>
        ///  MSZIP algorithm.
        /// </summary>
        DRS_COMP_ALG_MSZIP = 2,

        /// <summary>
        ///  windows_server_2003 compression.
        /// </summary>
        DRS_COMP_ALG_WIN2K3 = 3,
    }

    /// <summary>
    ///  DS_REPL_OP_TYPE is a concrete type for the replication
    ///  operation type.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\bf047cfe-32bd-43f6-93d3-b67b05eaac66.xml
    //  </remarks>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [Flags()]
    public enum DS_REPL_OP_TYPE
    {

        /// <summary>
        ///  Sync NC replica from server DC.
        /// </summary>
        DS_REPL_OP_TYPE_SYNC = 0,

        /// <summary>
        ///  Add NC replica server DC.
        /// </summary>
        DS_REPL_OP_TYPE_ADD = 1,

        /// <summary>
        ///  Remove NC replica server DC.
        /// </summary>
        DS_REPL_OP_TYPE_DELETE = 2,

        /// <summary>
        ///  Modify NC replica server DC.
        /// </summary>
        DS_REPL_OP_TYPE_MODIFY = 3,

        /// <summary>
        ///  Update NC replica client DC.
        /// </summary>
        DS_REPL_OP_TYPE_UPDATE_REFS = 4,
    }

    /// <summary>
    ///  RevealSecretsPolicy enumeration.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\dff72b44-5a85-45b8-bc9b-a6faab723610.xml
    //  </remarks>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [Flags()]
    public enum RevealSecretsPolicy
    {

        /// <summary>
        ///  RevealSecretsDeny constant.
        /// </summary>
        RevealSecretsDeny = 0,

        /// <summary>
        ///  RevealSecretsAllow constant.
        /// </summary>
        RevealSecretsAllow = 1,

        /// <summary>
        ///  RevealSecretsNoPolicy constant.
        /// </summary>
        RevealSecretsNoPolicy = 2,
    }

    /// <summary>
    ///  The nonreplicated, multivalued attributerepsFrom is
    ///  an optional attribute on the root object of every NC
    ///  replica. It is stored with the structure of the REPS_FROMconcrete
    ///  type, which is represented by the following diagram.
    ///  This attribute exposes a structure that controls the
    ///  server-to-server replication implementation—specifically,
    ///  the options for inbound replication of changes. In
    ///  the following field descriptions, the source DC refers
    ///  to the DC identified by the uuidDsaObj.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\f8e930ea-d847-4585-8d58-993e05f55e45.xml
    //  </remarks>
    public partial struct REPS_FROM
    {

        /// <summary>
        ///  The version of this structure. The value must be 1 or
        ///  2.windows_2000_server and windows_server_2003DCs have
        ///  value 1 in the dwVersion field. windows_server_2008
        ///  and windows_server_2008_r2DCs have value 2 in the dwVersion
        ///  field.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwVersion;

        /// <summary>
        /// TDI 66753
        /// </summary>
        public uint Reserved1;

        /// <summary>
        ///  The total number of bytes in the REPS_FROM structure.
        /// </summary>
        [CLSCompliant(false)]
        public uint cb;

        /// <summary>
        ///  An unsigned long that contains the number of consecutive
        ///  failures that have occurred while replicating from
        ///  the source DC.
        /// </summary>
        [CLSCompliant(false)]
        public uint cConsecutiveFailures;

        /// <summary>
        ///  A DSTIME that contains the time of the last successful
        ///  replication cycle with the source DC.
        /// </summary>
        public long timeLastSuccess;

        /// <summary>
        ///  A DSTIME that contains the time of the last replication
        ///  attempt with the source DC.
        /// </summary>
        public long timeLastAttempt;

        /// <summary>
        ///  An unsigned long that contains the result of the last
        ///  replication attempt with the source DC.
        /// </summary>
        [CLSCompliant(false)]
        public uint ulResultLastAttempt;

        /// <summary>
        ///  The offset from the start of the structure to a location
        ///  in the data field, specifying the start of a structure
        ///  that contains a NetworkAddress for the source DC. If
        ///  dwVersion is 1, it is an MTX_ADDR structure. If dwVersion
        ///  is 2, it is a DSA_RPC_INST structure.
        /// </summary>
        [CLSCompliant(false)]
        public uint cbOtherDraOffset;

        /// <summary>
        ///  The size of the structure pointed to by cbOtherDraOffset.
        /// </summary>
        [CLSCompliant(false)]
        public uint cbOtherDra;

        /// <summary>
        ///  A ULONG. This field contains a set of DRS_OPTIONS that
        ///  are applicable when replicating from the source DC.
        /// </summary>
        [CLSCompliant(false)]
        public uint ulReplicaFlags;

        /// <summary>
        ///  A REPLTIMES structure. If periodic replication is enabled
        ///  (ulReplicaFlags contains DRS_PER_SYNC), this field
        ///  identifies the 15-minute intervals within each week
        ///  when a replication cycle is initiated with the source
        ///  DC.
        /// </summary>
        public REPLTIMES rtSchedule;

        /// <summary>
        ///  A USN_VECTOR structure. This holds 0 or the usnvecTo
        ///  field from the response to the last IDL_DRSGetNCChanges
        ///  replication request sent to the source DC.
        /// </summary>
        public USN_VECTOR usnVec;

        /// <summary>
        /// TDI 66753
        /// </summary>
        public uint Reserved2;

        /// <summary>
        ///  A GUID that is the DSA GUID of the source DC.
        /// </summary>
        public Guid uuidDsaObj;

        /// <summary>
        ///  A GUID that contains the invocation ID of the source
        ///  DC.
        /// </summary>
        public Guid uuidInvocId;

        /// <summary>
        ///  A GUID that contains the objectGUID of the interSiteTransportobject
        ///  that corresponds to the transport used for communication
        ///  with the source DC.
        /// </summary>
        public Guid uuidTransportObj;

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public REPS_FROM_dwReserved_Values dwReserved;

        /// <summary>
        ///  The offset from the start of the structure to a location
        ///  in the data field, specifying the start of a PAS_DATA
        ///  structure.
        /// </summary>
        [CLSCompliant(false)]
        public uint cbPasDataOffset;

        /// <summary>
        ///  The storage for the rest of the structure. The structures
        ///  pointed to by cbOtherDraOffset and cbPasDataOffset
        ///  are packed into this field and can be referenced using
        ///  the offsets.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] data;
    }


    /// <summary>
    /// The reserved of REPS_FROM.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum REPS_FROM_dwReserved_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }


    /// <summary>
    ///  The DRS_MSG_RMSVRREQ_V1 structure defines a request
    ///  message sent to the IDL_DRSRemoveDsServer method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\05843a96-8bf7-4c08-b52d-5c472ed04a1a.xml
    //  </remarks>
    public partial struct DRS_MSG_RMSVRREQ_V1
    {

        /// <summary>
        ///  The DN of the serverobject of the DC to remove.
        /// </summary>
        [String()]
        public string ServerDN;

        /// <summary>
        ///  The DN of the NC root of the domain that the DC to be
        ///  removed belongs to. May be set to null if the client
        ///  does not want the server to compute the value of pmsgOut^.V1.fLastDCInDomain.
        /// </summary>
        [String()]
        public string DomainDN;

        /// <summary>
        ///  True if the DC's metadata should actually be removed
        ///  from the directory. False if the metadata should not
        ///  be removed. (This is used by a client that wants to
        ///  determine the value of pmsgOut^.V1.fLastDcInDomain
        ///  without altering the directory.)
        /// </summary>
        [CLSCompliant(false)]
        public int fCommit;
    }

    /// <summary>
    ///  The MTX_ADDR structure defines a concrete type for the
    ///  network name of a DC.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\107b7c0e-0f0d-4fe2-8232-14ec3b78f40d.xml
    //  </remarks>
    public partial struct MTX_ADDR
    {

        /// <summary>
        ///  A 32-bit, unsigned integer that specifies the number
        ///  of bytes in mtx_name, including a terminating null
        ///  character.
        /// </summary>
        [PossibleValueRange("1", "256")]
        [CLSCompliant(false)]
        public uint mtx_namelen;

        /// <summary>
        ///  The UTF-8 encoding of a NetworkAddress.
        /// </summary>
        [Inline()]
        [Size("mtx_namelen")]
        public byte[] mtx_name;
    }

    /// <summary>
    ///  The DRS_MSG_DCINFOREQ_V1 structure defines the request
    ///  message sent to the IDL_DRSDomainControllerInfo method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\18b23122-a1c2-4367-a677-592e0d4eef18.xml
    //  </remarks>
    public partial struct DRS_MSG_DCINFOREQ_V1
    {

        /// <summary>
        ///  DNS domain name for which the client requests DC information.
        /// </summary>
        [String()]
        public string Domain;

        /// <summary>
        ///  The response version requested by the client: 1, 2,
        ///  3, or 0xFFFFFFFF. The responses at InfoLevel 1, 2,
        ///  and 3 all contain information about DCs in the given
        ///  domain. The information at InfoLevel 1 is a subset
        ///  of the information at InfoLevel 2, which is a subset
        ///  of the information at InfoLevel 3. InfoLevel 3 includes
        ///  information about the RODCs in the given domain. InfoLevel
        ///  0xFFFFFFFF server returns information about the active
        ///  LDAP connections.
        /// </summary>
        [CLSCompliant(false)]
        public uint InfoLevel;
    }

    /// <summary>
    ///  The DRS_MSG_QUERYSITESREPLYELEMENT_V1 structure defines
    ///  the computed cost of communication between two sites.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\1a57b4d2-004d-4843-962a-0884abfb00c6.xml
    //  </remarks>
    public partial struct DRS_MSG_QUERYSITESREPLYELEMENT_V1
    {

        /// <summary>
        ///  0 if this "from-to" computation was successful, or ERROR_DS_OBJ_NOT_FOUND
        ///  if the "to" site does not exist.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwErrorCode;

        /// <summary>
        ///  The communication cost between the "from" site and this
        ///  "to" site, or 0xFFFFFFFF if the sites are not connected.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwCost;
    }

    /// <summary>
    ///  The DSA_MSG_PREPARE_SCRIPT_REPLY_V1 structure defines
    ///  a response message received from the IDL_DSAPrepareScript
    ///  method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\1c046d54-6167-4da8-b028-63ec9a235b6f.xml
    //  </remarks>
    public partial struct DSA_MSG_PREPARE_SCRIPT_REPLY_V1
    {

        /// <summary>
        ///  0 if successful, or a Windows error code if a fatal
        ///  error occurred.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwOperationStatus;

        /// <summary>
        ///  Null if successful, or a description of an error if
        ///  a fatal error occurred.
        /// </summary>
        [String()]
        public string pwErrMessage;

        /// <summary>
        ///  The count, in bytes, of the pbPassword array.
        /// </summary>
        [PossibleValueRange("0", "1024")]
        [CLSCompliant(false)]
        public uint cbPassword;

        /// <summary>
        ///  The password.
        /// </summary>
        [Size("cbPassword")]
        public byte[] pbPassword;

        /// <summary>
        ///  The count, in bytes, of the pbHashBody array.
        /// </summary>
        [PossibleValueRange("0", "10485760")]
        [CLSCompliant(false)]
        public uint cbHashBody;

        /// <summary>
        ///  The hash of the script value.
        /// </summary>
        [Size("cbHashBody")]
        public byte[] pbHashBody;

        /// <summary>
        ///  The count, in bytes, of the pbHashSignature array.
        /// </summary>
        [PossibleValueRange("0", "10485760")]
        [CLSCompliant(false)]
        public uint cbHashSignature;

        /// <summary>
        ///  The script signature.
        /// </summary>
        [Size("cbHashSignature")]
        public byte[] pbHashSignature;
    }

    /// <summary>
    ///  The PARTIAL_ATTR_VECTOR_V1_EXT structure defines a concrete
    ///  type for a set of attributes to be replicated to a
    ///  given partial replica.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\1d5c1b34-daa4-4761-a8b5-d3c0146a0e30.xml
    //  </remarks>
    public partial struct PARTIAL_ATTR_VECTOR_V1_EXT
    {

        /// <summary>
        ///  The version of this structure; MUST be 1.
        /// </summary>
        [CLSCompliant(false)]
        public dwVersion_Values dwVersion;

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public dwReserved1_Values dwReserved1;

        /// <summary>
        ///  The number of attributes in the rgPartialAttr array.
        /// </summary>
        [CLSCompliant(false)]
        public uint cAttrs;

        /// <summary>
        ///  The attributes in the set.
        /// </summary>
        [Inline()]
        [Size("cAttrs")]
        [CLSCompliant(false)]
        public uint[] rgPartialAttr;
    }

    /// <summary>
    /// The version of PARTIAL_ATTR_VECTOR_V1_EXT.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum dwVersion_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 1,
    }

    /// <summary>
    /// The reserved of PARTIAL_ATTR_VECTOR_V1_EXT.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum dwReserved1_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The DRS_MSG_SPNREPLY_V1 structure defines a response
    ///  message received from the IDL_DRSWriteSPN method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\23f5b130-02ba-48c2-a2b5-cafff417aafc.xml
    //  </remarks>
    public partial struct DRS_MSG_SPNREPLY_V1
    {

        /// <summary>
        ///  0, or a Windows error code.
        /// </summary>
        [CLSCompliant(false)]
        public uint retVal;
    }

    /// <summary>
    ///  The DRS_MSG_NT4_CHGLOG_REQ_V1 structure defines the
    ///  request message sent to the IDL_DRSGetNT4ChangeLog
    ///  method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\27207d2f-7de4-4658-82dd-228bfea28c49.xml
    //  </remarks>
    public partial struct DRS_MSG_NT4_CHGLOG_REQ_V1
    {

        /// <summary>
        ///  Zero or more of the following bit flags:01234567891
        ///  01234567892 01234567893 01CLSNXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX:
        ///  Unused. MUST be zero and ignored.CL (DRS_NT4_CHGLOG_GET_CHANGE_LOG):
        ///  If set, the server returns the PDC change log.SN (DRS_NT4_CHGLOG_GET_SERIAL_NUMBERS):
        ///  If set, the server returns the NT4 replication state.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwFlags;

        /// <summary>
        ///  The maximum size, in bytes, of the change log data that
        ///  should be retrieved in a single operation.
        /// </summary>
        [CLSCompliant(false)]
        public uint PreferredMaximumLength;

        /// <summary>
        ///  Zero if pRestart = null. Otherwise, the size, in bytes,
        ///  of pRestart^.
        /// </summary>
        [CLSCompliant(false)]
        public uint cbRestart;

        /// <summary>
        ///  Null to request the change log from the beginning. Otherwise,
        ///  a pointer to an opaque value, returned in some previous
        ///  call to IDL_DRSGetNT4ChangeLog, identifying the last
        ///  change log entry returned in that previous call.
        /// </summary>
        [Size("cbRestart")]
        public byte[] pRestart;
    }

    /// <summary>
    ///  The DRS_MSG_REPLICA_DEMOTIONREPLY_V1 structure defines
    ///  a response message received from the IDL_DRSReplicaDemotion
    ///  method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\33b62695-8d9d-4a25-8d37-320131470cca.xml
    //  </remarks>
    public partial struct DRS_MSG_REPLICA_DEMOTIONREPLY_V1
    {

        /// <summary>
        ///  The Win32 error code, as specified in [MS-ERREF] section
        ///  .
        /// </summary>
        [CLSCompliant(false)]
        public uint dwOpError;
    }

    /// <summary>
    ///  The DS_DOMAIN_CONTROLLER_INFO_FFFFFFFFW structure defines
    ///  DC information that is returned as a part of the response
    ///  to an InfoLevel = 0xFFFFFFFF request. The struct contains
    ///  information about a single LDAP connection to the current
    ///  server.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\38259d46-11e6-4e74-8e0c-0b0f9ce2dab4.xml
    //  </remarks>
    public partial struct DS_DOMAIN_CONTROLLER_INFO_FFFFFFFFW
    {

        /// <summary>
        ///  The IPv4 address of the client that established the
        ///  LDAP connection to the server. If the client connected
        ///  with IPv6, this field contains zero.
        /// </summary>
        [CLSCompliant(false)]
        public uint IPAddress;

        /// <summary>
        ///  Number of LDAP notifications enabled on the server.
        /// </summary>
        [CLSCompliant(false)]
        public uint NotificationCount;

        /// <summary>
        ///  Total time in number of seconds that the connection
        ///  is established.
        /// </summary>
        [CLSCompliant(false)]
        public uint secTimeConnected;

        /// <summary>
        ///  Zero or more of the bit flags from LDAP_CONN_PROPERTIES
        ///  indicating the properties of this connection.
        /// </summary>
        [CLSCompliant(false)]
        public uint Flags;

        /// <summary>
        ///  Total number of LDAP requests made on this LDAP connection.
        /// </summary>
        [CLSCompliant(false)]
        public uint TotalRequests;

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public Reserved1_Values Reserved1;

        /// <summary>
        ///  Name of the security principal that established the
        ///  LDAP connection.
        /// </summary>
        [String()]
        public string UserName;
    }

    /// <summary>
    /// The reserved1 of DS_DOMAIN_CONTROLLER_INFO_FFFFFFFFW.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum Reserved1_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The SECERR_DRS_WIRE_V1 structure defines a security
    ///  error.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\3f4e4f69-5652-4318-b898-a22d5907403c.xml
    //  </remarks>
    public partial struct SECERR_DRS_WIRE_V1
    {

        /// <summary>
        ///  The implementation-specific diagnostic code.
        /// </summary>
        [CLSCompliant(false)]
        public uint dsid;

        /// <summary>
        ///  0, STATUS code, or Windows error code.
        /// </summary>
        [CLSCompliant(false)]
        public uint extendedErr;

        /// <summary>
        ///  The implementation-specific diagnostic code.
        /// </summary>
        [CLSCompliant(false)]
        public uint extendedData;

        /// <summary>
        ///  0 or PROBLEM error code (section ).
        /// </summary>
        [CLSCompliant(false)]
        public ushort problem;
    }

    /// <summary>
    ///  The REPLTIMES structure defines a concrete type for
    ///  times at which periodic replication occurs.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\42d7d8e8-794e-4279-9802-8b5916e8b099.xml
    //  </remarks>
    public partial struct REPLTIMES
    {

        /// <summary>
        ///  A byte array of length 84 that is used to set periodic
        ///  replication times. Each bit in this byte array represents
        ///  a 15-minute period for which replication can be scheduled
        ///  within a one-week period. The replication schedule
        ///  begins on Sunday 12:00:00 AM UTC. Each byte in the
        ///  array represents a two-hour period of a week in ascending
        ///  order, starting Sunday 12:00:00 AM UTC. The most significant
        ///  bit of a byte represents the earliest 15-minute period
        ///  in the two-hour period, and the rest of the bits in
        ///  the byte represent their respective 15-minute periods
        ///  in this order.
        /// </summary>
        [Inline()]
        [StaticSize(84, StaticSizeMode.Elements)]
        public byte[] rgTimes;
    }

    /// <summary>
    ///  The DRS_MSG_SPNREQ_V1 structure defines a request message
    ///  sent to the IDL_DRSWriteSPN method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\463869d7-28a5-4e15-8c9b-0e3376e9acff.xml
    //  </remarks>
    public partial struct DRS_MSG_SPNREQ_V1
    {

        /// <summary>
        ///  The SPN operation to perform. MUST be one of the DS_SPN_OPERATION
        ///  values.
        /// </summary>
        [CLSCompliant(false)]
        public uint operation;

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public flags_Values flags;

        /// <summary>
        ///  The DN of the object to modify.
        /// </summary>
        [String()]
        public string pwszAccount;

        /// <summary>
        ///  The number of items in the rpwszSPN array.
        /// </summary>
        [PossibleValueRange("0", "10000")]
        [CLSCompliant(false)]
        public uint cSPN;

        /// <summary>
        ///  The SPN values.
        /// </summary>
        [String()]
        [Size("cSPN")]
        public string[] rpwszSPN;
    }

    /// <summary>
    /// The flags of DRS_MSG_SPNREQ_V1.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum flags_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The DSA_MSG_EXECUTE_SCRIPT_REQ_V1 structure defines
    ///  a request message sent to the IDL_DSAExecuteScript
    ///  method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\46742a7c-b859-4e4e-a64f-58d572b3bde8.xml
    //  </remarks>
    public partial struct DSA_MSG_EXECUTE_SCRIPT_REQ_V1
    {

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public Flags_Values Flags;

        /// <summary>
        ///  The count, in bytes, of the pbPassword array.
        /// </summary>
        [PossibleValueRange("1", "1024")]
        [CLSCompliant(false)]
        public uint cbPassword;

        /// <summary>
        ///  The password.
        /// </summary>
        [Size("cbPassword")]
        public byte[] pbPassword;
    }

    /// <summary>
    /// The flags values of DSA_MSG_EXECUTE_SCRIPT_REQ_V1.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum Flags_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The DRS_MSG_ADDSIDREQ_V1 structure defines the request
    ///  message sent to the IDL_DRSAddSidHistory method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\50b7cc92-608c-44ac-9d3e-48e2112c9bc0.xml
    //  </remarks>
    public partial struct DRS_MSG_ADDSIDREQ_V1
    {

        /// <summary>
        ///  A set of zero or more DRS_ADDSID_FLAGS bit flags.
        /// </summary>
        [CLSCompliant(false)]
        public uint Flags;

        /// <summary>
        ///  Name of the domain to query for the SID of SrcPrincipal.
        ///  The domain name can be an FQDN or a NetBIOS name.
        /// </summary>
        [String()]
        public string SrcDomain;

        /// <summary>
        ///  Name of a security principal (user, computer, or group)
        ///  in the source domain. This is the source principal,
        ///  whose SIDs will be added to the destination principal.
        ///  If Flags does not contain DS_ADDSID_FLAG_PRIVATE_DEL_SRC_OBJ,
        ///  this name is a domain-relative Security Accounts Manager
        ///  (SAM) name. Otherwise, it is a DN.
        /// </summary>
        [String()]
        public string SrcPrincipal;

        /// <summary>
        ///  Name of the primary domain controller (PDC) (or PDC
        ///  role owner) in the source domain. The DC name can be
        ///  an Internet host name or a NetBIOS name. If null, the
        ///  implementation of IDL_DRSAddSidHistory will locate
        ///  such a DC.
        /// </summary>
        [String()]
        public string SrcDomainController;

        /// <summary>
        ///  Count of characters in the SrcCredsUser array.
        /// </summary>
        [PossibleValueRange("0", "256")]
        [CLSCompliant(false)]
        public uint SrcCredsUserLength;

        /// <summary>
        ///  User name for the credentials to be used in the source
        ///  domain.
        /// </summary>
        [Size("SrcCredsUserLength")]
        [CLSCompliant(false)]
        public ushort[] SrcCredsUser;

        /// <summary>
        ///  Count of characters in the SrcCredsDomain array.
        /// </summary>
        [PossibleValueRange("0", "256")]
        [CLSCompliant(false)]
        public uint SrcCredsDomainLength;

        /// <summary>
        ///  Domain name for the credentials to be used in the source
        ///  domain.
        /// </summary>
        [Size("SrcCredsDomainLength")]
        [CLSCompliant(false)]
        public ushort[] SrcCredsDomain;

        /// <summary>
        ///  Count of characters in the SrcCredsPassword array.
        /// </summary>
        [PossibleValueRange("0", "256")]
        [CLSCompliant(false)]
        public uint SrcCredsPasswordLength;

        /// <summary>
        ///  Password for the credentials to be used in the source
        ///  domain.
        /// </summary>
        [Size("SrcCredsPasswordLength")]
        [CLSCompliant(false)]
        public ushort[] SrcCredsPassword;

        /// <summary>
        ///  Name of the destination domain in which DstPrincipal
        ///  resides. The domain name can be an FQDN or a NetBIOS
        ///  name.
        /// </summary>
        [String()]
        public string DstDomain;

        /// <summary>
        ///  Name of a security principal (user, computer, or group)
        ///  in the destination domain. This is the destination
        ///  principal, to which the source principal'sSIDs will
        ///  be added. If Flags does not contain DS_ADDSID_FLAG_PRIVATE_DEL_SRC_OBJ,
        ///  this name is a domain-relative SAM name. Otherwise,
        ///  it is a DN.
        /// </summary>
        [String()]
        public string DstPrincipal;
    }

    /// <summary>
    ///  The NAMERESOP_DRS_WIRE_V1 structure defines the state
    ///  of name resolution.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\5426b9b7-ba82-4f1b-a58a-bfafb8c12ba6.xml
    //  </remarks>
    public partial struct NAMERESOP_DRS_WIRE_V1
    {

        /// <summary>
        ///  MUST be the uppercase ASCII character "S".
        /// </summary>
        public byte nameRes;

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public unusedPad_Values unusedPad;

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public nextRDN_Values nextRDN;
    }

    /// <summary>
    /// The unused values of NAMERESOP_DRS_WIRE_V1.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum unusedPad_Values : byte
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The next RDN values of NAMERESOP_DRS_WIRE_V1.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum nextRDN_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The SYSERR_DRS_WIRE_V1 structure defines a system error.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\5528c5ea-4f69-4061-ae72-6cea2f7276c3.xml
    //  </remarks>
    public partial struct SYSERR_DRS_WIRE_V1
    {

        /// <summary>
        ///  The implementation-specific diagnostic code.
        /// </summary>
        [CLSCompliant(false)]
        public uint dsid;

        /// <summary>
        ///  0, STATUS code, or Windows error code.
        /// </summary>
        [CLSCompliant(false)]
        public uint extendedErr;

        /// <summary>
        ///  The implementation-specific diagnostic code.
        /// </summary>
        [CLSCompliant(false)]
        public uint extendedData;

        /// <summary>
        ///  0 or PROBLEM error code (section ).
        /// </summary>
        [CLSCompliant(false)]
        public ushort problem;
    }

    /// <summary>
    ///  The USN_VECTOR structure defines a concrete type for
    ///  the cookie (section) used to pass state between calls
    ///  to IDL_DRSGetNCChanges to pass implementation-specific
    ///  state between calls to server-to-server replication.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\595d11b8-6ca7-4a61-bd56-3e6a2b99b76b.xml
    //  </remarks>
    public partial struct USN_VECTOR
    {

        /// <summary>
        ///  A USN.
        /// </summary>
        public ulong usnHighObjUpdate;

        /// <summary>
        ///  A USN.
        /// </summary>
        public ulong usnReserved;

        /// <summary>
        ///  A USN.
        /// </summary>
        public ulong usnHighPropUpdate;
    }

    /// <summary>
    ///  The DRS_MSG_INIT_DEMOTIONREPLY_V1 structure defines
    ///  a response message received from the IDL_DRSInitDemotion
    ///  method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\5c5340e1-e984-4d3e-84ec-3e2efa555366.xml
    //  </remarks>
    public partial struct DRS_MSG_INIT_DEMOTIONREPLY_V1
    {

        /// <summary>
        ///  A Win32 error code, as specified in [MS-ERREF] section
        ///  .
        /// </summary>
        [CLSCompliant(false)]
        public uint dwOpError;
    }

    /// <summary>
    ///  ULARGE_INTEGER is a concrete type for a 64-bit, unsigned
    ///  integer.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\686ea1a0-42d0-4ff1-95ef-52d0148fe842.xml
    //  </remarks>
    public partial struct ULARGE_INTEGER
    {

        /// <summary>
        ///  QuadPart member.
        /// </summary>
        [CLSCompliant(false)]
        public ulong QuadPart;
    }

    /// <summary>
    ///  The DRS_COMPRESSED_BLOB structure defines a concrete
    ///  type that results from marshaling a data structure
    ///  into a byte stream by using RPC and compressing that
    ///  byte stream.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\6d3e7f57-3ef8-46e0-a6ad-e9331f297957.xml
    //  </remarks>
    public partial struct DRS_COMPRESSED_BLOB
    {

        /// <summary>
        ///  Size in bytes of the uncompressed byte stream.
        /// </summary>
        [CLSCompliant(false)]
        public uint cbUncompressedSize;

        /// <summary>
        ///  Size in bytes of the pbCompressedData array.
        /// </summary>
        [PossibleValueRange("1", "10485760")]
        [CLSCompliant(false)]
        public uint cbCompressedSize;

        /// <summary>
        ///  Compressed byte stream.
        /// </summary>
        [Size("cbCompressedSize")]
        public byte[] pbCompressedData;
    }

    /// <summary>
    ///  The DRS_MSG_KCC_EXECUTE_V1 structure defines the request
    ///  message sent to the IDL_DRSExecuteKCC method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\8111f933-07f4-4c59-8497-50c0f8970eb5.xml
    //  </remarks>
    public partial struct DRS_MSG_KCC_EXECUTE_V1
    {

        /// <summary>
        ///  MUST be 0.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwTaskID;

        /// <summary>
        ///  Zero or more of the following bit flags. 0 1 2 3 4 5
        ///  6 7 8 9 1 0 1 2 3 4 5 6 7 8 9 2 0 1 2 3 4 5 6 7 8 9
        ///  3 0 1 A S D P X X X X X X X X X X X X X X X X X X X
        ///  X X X X X X X X X X XX: Unused. MUST be zero and ignored.AS
        ///  (DS_KCC_FLAG_ASYNC_OP): Request the KCC to run, then
        ///  return immediately.DS_KCC_FLAG_DAMPED: Request
        ///  the KCC to run unless there is already such a request
        ///  pending.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwFlags;
    }

    /// <summary>
    /// The TaskId of DRS_MSG_KCC_EXECUTE_V1.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum dwTaskID_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The DSA_MSG_EXECUTE_SCRIPT_REPLY_V1 structure defines
    ///  a response message received from the IDL_DSAExecuteScript
    ///  method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\84fef2b2-02bb-4832-b0d1-b7eca9689e76.xml
    //  </remarks>
    public partial struct DSA_MSG_EXECUTE_SCRIPT_REPLY_V1
    {

        /// <summary>
        ///  0 if successful, or a Windows error code if a fatal
        ///  error occurred.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwOperationStatus;

        /// <summary>
        ///  Null if successful, or a description of the error if
        ///  a fatal error occurred.
        /// </summary>
        [String()]
        public string pwErrMessage;
    }

    /// <summary>
    ///  The DSA_MSG_PREPARE_SCRIPT_REQ_V1 structure defines
    ///  a request message sent to the IDL_DSAPrepareScript
    ///  method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\8664f791-fd1e-4f6c-aa2a-b0703d0786a0.xml
    //  </remarks>
    public partial struct DSA_MSG_PREPARE_SCRIPT_REQ_V1
    {

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public Reserved_Values Reserved;
    }

    /// <summary>
    /// The reserved of DRS_MSG_RMDMNREPLY_V1.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum Reserved_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The DSA_RPC_INST structure is a concrete type that represents
    ///  a DC.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\88a39619-6dbe-4ba1-8435-5966c1a490a7.xml
    //  </remarks>
    [CLSCompliant(false)]
    public partial struct _DSA_RPC_INST
    {

        /// <summary>
        ///  The total number of bytes in the DSA_RPC_INST structure.
        /// </summary>
        [CLSCompliant(false)]
        public uint cb;

        /// <summary>
        ///  The offset from the start of the DSA_RPC_INST structure
        ///  to a location that specifies the start of the server
        ///  name of this instance.
        /// </summary>
        [CLSCompliant(false)]
        public uint cbpszServerOffset;

        /// <summary>
        ///  The offset from the start of the DSA_RPC_INST structure
        ///  to a location that specifies the start of the annotation
        ///  of this instance.
        /// </summary>
        [CLSCompliant(false)]
        public uint cbpszAnnotationOffset;

        /// <summary>
        ///  The offset from the start of the DSA_RPC_INST structure
        ///  to a location that specifies the start of the NetworkAddress
        ///  of this instance.
        /// </summary>
        [CLSCompliant(false)]
        public uint cbpszInstanceOffset;

        /// <summary>
        ///  The offset from the start of the DSA_RPC_INST structure
        ///  to a location that specifies the start of the GUID
        ///  for the instance.
        /// </summary>
        [CLSCompliant(false)]
        public uint cbpguidInstanceOffset;
    }

    /// <summary>
    ///  The DS_REPL_SERVER_OUTGOING_CALL structure defines an
    ///  outstanding request from this DC to another DC. This
    ///  structure is a concrete representation of a tuple from
    ///  an RPCOutgoingContexts sequence.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\8e769e6a-a8b2-4291-aeff-1ec521c96852.xml
    //  </remarks>
    public partial struct DS_REPL_SERVER_OUTGOING_CALL
    {

        /// <summary>
        ///  The NetworkAddress of the server.
        /// </summary>
        [String()]
        public string pszServerName;

        /// <summary>
        ///  True if and only if the IDL_DRSBind method has completed
        ///  and the IDL_DRSUnbind method has not yet been called.
        /// </summary>
        [CLSCompliant(false)]
        public int fIsHandleBound;

        /// <summary>
        ///  True if and only if the context handle used was retrieved
        ///  from the cache an implemented cache.
        /// </summary>
        [CLSCompliant(false)]
        public int fIsHandleFromCache;

        /// <summary>
        ///  True if and only if the context handle is still in the
        ///  implemented cache.
        /// </summary>
        [CLSCompliant(false)]
        public int fIsHandleInCache;

        /// <summary>
        ///  The implementation-specific thread ID of the
        ///  thread that is using the context.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwThreadId;

        /// <summary>
        ///  If the context is set to be canceled, the time-out in
        ///  minutes.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwBindingTimeoutMins;

        /// <summary>
        ///  The date and time when the context was created.
        /// </summary>
        public long dstimeCreated;

        /// <summary>
        ///  The call that the client is waiting on. MUST be one
        ///  of the values in the following table.
        /// </summary>
        [CLSCompliant(false)]
        public dwCallType_Values dwCallType;
    }

    /// <summary>
    /// The call type of DS_REPL_SERVER_OUTGOING_CALL.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    //[Flags()]
    [CLSCompliant(false)]
    public enum dwCallType_Values : uint
    {

        /// <summary>
        ///  IDL_DRSBind
        /// </summary>
        V1 = 2,

        /// <summary>
        ///  IDL_DRSUnbind
        /// </summary>
        V2 = 3,

        /// <summary>
        ///  IDL_DRSReplicaSync
        /// </summary>
        V3 = 4,

        /// <summary>
        ///  IDL_DRSGetNCChangesImplemented server-to-server replication
        ///  method
        /// </summary>
        V4 = 5,

        /// <summary>
        ///  IDL_DRSUpdateRefs
        /// </summary>
        V5 = 6,

        /// <summary>
        ///  IDL_DRSReplicaAdd
        /// </summary>
        V6 = 7,

        /// <summary>
        ///  IDL_DRSReplicaDel
        /// </summary>
        V7 = 8,

        /// <summary>
        ///  IDL_DRSVerifyNamesImplemented server-to-server object
        ///  verification method
        /// </summary>
        V8 = 9,

        /// <summary>
        ///  IDL_DRSGetMembershipsImplemented server-to-server group
        ///  expansion method
        /// </summary>
        V9 = 10,

        /// <summary>
        ///  IDL_DRSInterDomainMoveImplemented server-to-server cross
        ///  NC move method
        /// </summary>
        V10 = 11,

        /// <summary>
        ///  IDL_DRSGetNT4ChangeLogImplemented server-to-server replication
        ///  method
        /// </summary>
        V11 = 12,

        /// <summary>
        ///  IDL_DRSCrackNames
        /// </summary>
        V12 = 13,

        /// <summary>
        ///  IDL_DRSAddEntryImplemented server-to-server object creation
        ///  method
        /// </summary>
        V13 = 14,

        /// <summary>
        ///  IDL_DRSGetMemberships2Implemented server-to-server object
        ///  lookup method
        /// </summary>
        V14 = 15,

        /// <summary>
        ///  IDL_DRSGetObjectExistenceImplemented server-to-server
        ///  replication method
        /// </summary>
        V15 = 16,

        /// <summary>
        ///  IDL_DRSGetReplInfo
        /// </summary>
        V16 = 17,
    }

    /// <summary>
    ///  The NT4SID structure defines a concrete type for a SID.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\8fb66015-d049-47b9-804d-1372b1afc9fc.xml
    //  </remarks>
    public partial struct NT4SID
    {

        /// <summary>
        ///  Bytes that make up a SID structure, as specified in
        ///  [MS-DTYP] section, in little-endian byte order.
        /// </summary>
        [Inline()]
        [StaticSize(28, StaticSizeMode.Elements)]
        public byte[] Data;
    }

    /// <summary>
    ///  The DRS_MSG_ADDSIDREPLY_V1 structure defines the response
    ///  message received from the IDL_DRSAddSidHistory method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\9f752d9d-236d-4370-aadf-03f2f1ecbee4.xml
    //  </remarks>
    public partial struct DRS_MSG_ADDSIDREPLY_V1
    {

        /// <summary>
        ///  Windows error code.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwWin32Error;
    }

    /// <summary>
    ///  DRS_SecBuffer is a concrete type for a buffer that contains
    ///  authentication data.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\a5ad6879-cf0c-411c-96c3-2eb7ea37149c.xml
    //  </remarks>
    public partial struct DRS_SecBuffer
    {

        /// <summary>
        ///  The size, in bytes, of the pvBuffer array.
        /// </summary>
        [CLSCompliant(false)]
        public uint cbBuffer;

        /// <summary>
        ///  A bit field that contains the following values:01234567891
        ///  01234567892 01234567893 01TYPXXXXXXXXXXXXXXXXXXXXXXXXXXXXROX:
        ///  Unused. MUST be zero and ignored.TYP: May be one of
        ///  the following values:
        /// </summary>
        [CLSCompliant(false)]
        public BufferType_Values BufferType;

        /// <summary>
        ///  Authentication data.
        /// </summary>
        [Size("cbBuffer")]
        public byte[] pvBuffer;
    }

    /// <summary>
    ///  The buffer type values of DRS_SecBuffer.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [Flags()]
    [CLSCompliant(false)]
    public enum BufferType_Values : uint
    {

        /// <summary>
        ///  A placeholder in the buffer array. The caller can supply
        ///  several such entries in the array, and the security
        ///  package can return data in them.
        /// </summary>
        SECBUFFER_EMPTY = 0x00000000,

        /// <summary>
        ///  Used for common data. The security package can read
        ///  this data and write it, for example, to encrypt some
        ///  or all of it.
        /// </summary>
        SECBUFFER_DATA = 0x00000001,

        /// <summary>
        ///  This buffer is used to indicate the security token portion
        ///  of the message. This is read-only for input parameters
        ///  or read/write for output parameters.
        /// </summary>
        SECBUFFER_TOKEN = 0x00000002,

        /// <summary>
        ///  These are transport-to-package–specific parameters.
        ///  For example, the Netware redirector may supply the
        ///  server object identifier, while DCE RPC can supply
        ///  an association UUID, and so on.
        /// </summary>
        SECBUFFER_PKG_PARAMS = 0x00000003,

        /// <summary>
        ///  The security package uses this value to indicate the
        ///  number of missing bytes in a particular message. The
        ///  pvBuffer member is ignored in this type.
        /// </summary>
        SECBUFFER_MISSING = 0x00000004,

        /// <summary>
        ///  The security package uses this value to indicate the
        ///  number of extra or unprocessed bytes in a message.
        /// </summary>
        SECBUFFER_EXTRA = 0x00000005,

        /// <summary>
        ///  Indicates a protocol-specific trailer for a particular
        ///  record. This is not usually of interest to callers.
        /// </summary>
        SECBUFFER_STREAM_TRAILER = 0x00000006,

        /// <summary>
        ///  Indicates a protocol-specific header for a particular
        ///  record. This is not usually of interest to callers.
        /// </summary>
        SECBUFFER_STREAM_HEADER = 0x00000007,
    }

    /// <summary>
    ///  The DRS_MSG_QUERYSITESREQ_V1 structure defines a request
    ///  message sent to the IDL_DRSQuerySitesByCost method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\a5e57fa8-9441-44c6-af98-6437db28d6d6.xml
    //  </remarks>
    public partial struct DRS_MSG_QUERYSITESREQ_V1
    {

        /// <summary>
        ///  The RDN of the siteobject of the "from" site.
        /// </summary>
        [String()]
        public string pwszFromSite;

        /// <summary>
        ///  The number of items in the rgszToSites array (the count
        ///  of "to" sites).
        /// </summary>
        [PossibleValueRange("1", "10000")]
        [CLSCompliant(false)]
        public uint cToSites;

        /// <summary>
        ///  The RDNs of the siteobjects of the "to" sites.
        /// </summary>
        [String()]
        [Size("cToSites")]
        public string[] rgszToSites;

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public dwFlags_Values dwFlags;
    }

    /// <summary>
    /// The flags of DRS_MSG_QUERYSITESREQ_V1.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum dwFlags_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The DS_DOMAIN_CONTROLLER_INFO_1W structure defines DC
    ///  information that is returned as a part of the response
    ///  to an InfoLevel = 1 request. The struct contains information
    ///  about a single DC in the domain.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\b30c5951-ccb1-4fb6-ba9a-5699d5d78759.xml
    //  </remarks>
    public partial struct DS_DOMAIN_CONTROLLER_INFO_1W
    {

        /// <summary>
        ///  NetBIOS name of the DC.
        /// </summary>
        [String()]
        public string NetbiosName;

        /// <summary>
        ///  DNS host name of the DC.
        /// </summary>
        [String()]
        public string DnsHostName;

        /// <summary>
        ///  RDN of the siteobject.
        /// </summary>
        [String()]
        public string SiteName;

        /// <summary>
        ///  DN of the computerobject that corresponds to the DC.
        /// </summary>
        [String()]
        public string ComputerObjectName;

        /// <summary>
        ///  DN of the serverobject that corresponds to the DC.
        /// </summary>
        [String()]
        public string ServerObjectName;

        /// <summary>
        ///  True if and only if the DC is the PDCFSMO role owner.
        /// </summary>
        [CLSCompliant(false)]
        public int fIsPdc;

        /// <summary>
        ///  MUST be true.
        /// </summary>
        [CLSCompliant(false)]
        public int fDsEnabled;
    }

    /// <summary>
    ///  The DRS_MSG_CRACKREQ_V1 structure defines the request
    ///  message sent to the IDL_DRSCrackNames method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\b47debc0-59ee-40e4-ad0f-4bc9f96043b2.xml
    //  </remarks>
    public partial struct DRS_MSG_CRACKREQ_V1
    {

        /// <summary>
        ///  The character set used by the client. This field SHOULD
        ///  be ignored by the server.
        /// </summary>
        [CLSCompliant(false)]
        public uint CodePage;

        /// <summary>
        ///  The locale used by the client. This field SHOULD be
        ///  ignored by the server.
        /// </summary>
        [CLSCompliant(false)]
        public uint LocaleId;

        /// <summary>
        ///  Zero or more of the following bit flags. 0 1 2 3 4 5
        ///  6 7 8 9 1 0 1 2 3 4 5 6 7 8 9 2 0 1 2 3 4 5 6 7 8 9
        ///  3 0 1 X X G C T R X X X X X X X X X X X X X X X X X
        ///  X X X X X X X X X X F P OX: Unused. MUST be zero and
        ///  ignored.GC (DS_NAME_FLAG_GC_VERIFY): If set, the call
        ///  fails if the server is not a GC server.TR (DS_NAME_FLAG_TRUST_REFERRAL):
        ///  If set and the lookup fails on the server, referrals
        ///  are returned to trusted forests where the lookup might
        ///  succeed.FPO (DS_NAME_FLAG_PRIVATE_RESOLVE_FPOS): If
        ///  set and the named object is a foreign security principal,
        ///  indicate this by using the status of the lookup operation.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwFlags;

        /// <summary>
        ///  The format of the names in rpNames. This may be one
        ///  of the values from DS_NAME_FORMAT or one of the following.
        /// </summary>
        [CLSCompliant(false)]
        public uint formatOffered;

        /// <summary>
        ///  Format of the names in the rItems field of the DS_NAME_RESULTW
        ///  structure, which is returned inside the DRS_MSG_CRACKREPLY
        ///  message. This may be one of the values from DS_NAME_FORMAT
        ///  or one of the following.
        /// </summary>
        [CLSCompliant(false)]
        public uint formatDesired;

        /// <summary>
        ///  Count of items in the rpNames array.
        /// </summary>
        [PossibleValueRange("1", "10000")]
        [CLSCompliant(false)]
        public uint cNames;

        /// <summary>
        ///  Input names to translate.
        /// </summary>
        [String()]
        [Size("cNames")]
        public string[] rpNames;
    }

    /// <summary>
    /// The formatOffered of DRS_MSG_CRACKREQ_V1.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    //[Flags()]
    [CLSCompliant(false)]
    public enum formatOffered_Values : uint
    {

        /// <summary>
        ///  Get all sites in the forest.
        /// </summary>
        DS_LIST_SITES = 0xFFFFFFFF,

        /// <summary>
        ///  Get all servers in a given site.
        /// </summary>
        DS_LIST_SERVERS_IN_SITE = 0xFFFFFFFE,

        /// <summary>
        ///  Get all domains in a given site.
        /// </summary>
        DS_LIST_DOMAINS_IN_SITE = 0xFFFFFFFD,

        /// <summary>
        ///  Get all DCs of a specified domain in a given site.
        /// </summary>
        DS_LIST_SERVERS_FOR_DOMAIN_IN_SITE = 0xFFFFFFFC,

        /// <summary>
        ///  Get DNS host name and server reference for a given DC.
        /// </summary>
        DS_LIST_INFO_FOR_SERVER = 0xFFFFFFFB,

        /// <summary>
        ///  Get FSMO role owners.
        /// </summary>
        DS_LIST_ROLES = 0xFFFFFFFA,

        /// <summary>
        ///  Get value of sAMAccountNameattribute.
        /// </summary>
        DS_NT4_ACCOUNT_NAME_SANS_DOMAIN = 0xFFFFFFF9,

        /// <summary>
        ///  Get LDAP display name from schemaGUID. The given schemaGUID
        ///  should be in the curly braced GUID string format as
        ///  specified in [MS-DTYP] section.
        /// </summary>
        DS_MAP_SCHEMA_GUID = 0xFFFFFFF8,

        /// <summary>
        ///  Get all domains in the forest.
        /// </summary>
        DS_LIST_DOMAINS = 0xFFFFFFF7,

        /// <summary>
        ///  Get all NCs in the forest.
        /// </summary>
        DS_LIST_NCS = 0xFFFFFFF6,

        /// <summary>
        ///  Alternate security identifier.
        /// </summary>
        DS_ALT_SECURITY_IDENTITIES_NAME = 0xFFFFFFF5,

        /// <summary>
        ///  String form of SID.
        /// </summary>
        DS_STRING_SID_NAME = 0xFFFFFFF4,

        /// <summary>
        ///  Get all DCs in a given site.
        /// </summary>
        DS_LIST_SERVERS_WITH_DCS_IN_SITE = 0xFFFFFFF3,

        /// <summary>
        ///  Get all GCs.
        /// </summary>
        DS_LIST_GLOBAL_CATALOG_SERVERS = 0xFFFFFFF1,

        /// <summary>
        ///  Get value of sAMAccountNameattribute; return status
        ///  DS_NAME_ERROR_NOT_FOUND if account is invalid.
        /// </summary>
        DS_NT4_ACCOUNT_NAME_SANS_DOMAIN_EX = 0xFFFFFFF0,

        /// <summary>
        ///  The user principal name and alternate security identifier.
        /// </summary>
        DS_USER_PRINCIPAL_NAME_AND_ALTSECID = 0xFFFFFFEF,
    }

    /// <summary>
    /// The formatDesired of DRS_MSG_CRACKREQ_V1.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    //[Flags()]
    [CLSCompliant(false)]
    public enum formatDesired_Values : uint
    {

        /// <summary>
        ///  String form of a SID.
        /// </summary>
        DS_STRING_SID_NAME = 0xFFFFFFF4,

        /// <summary>
        ///  User principal name.
        /// </summary>
        DS_USER_PRINCIPAL_NAME_FOR_LOGON = 0xFFFFFFF2,
    }

    /// <summary>
    ///  The UPDERR_DRS_WIRE_V1 structure defines an update error.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\c7e91a8c-1c26-4a60-b3f6-4a0d0be368b2.xml
    //  </remarks>
    public partial struct UPDERR_DRS_WIRE_V1
    {

        /// <summary>
        ///  The implementation-specific diagnostic code.
        /// </summary>
        [CLSCompliant(false)]
        public uint dsid;

        /// <summary>
        ///  0, STATUS code, or Windows error code.
        /// </summary>
        [CLSCompliant(false)]
        public uint extendedErr;

        /// <summary>
        ///  The implementation-specific diagnostic code.
        /// </summary>
        [CLSCompliant(false)]
        public uint extendedData;

        /// <summary>
        ///  0 or PROBLEM error code (section ).
        /// </summary>
        [CLSCompliant(false)]
        public ushort problem;
    }

    /// <summary>
    ///  The OID_t structure defines a concrete type for an OID
    ///  or a prefix of an OID; it is a component of type PrefixTableEntry.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\cbc2b761-8938-4591-a9f7-2d1512ed7f05.xml
    //  </remarks>
    public partial struct OID_t
    {

        /// <summary>
        ///  The size, in bytes, of the elements array.
        /// </summary>
        [PossibleValueRange("0", "10000")]
        [CLSCompliant(false)]
        public uint length;

        /// <summary>
        ///  An array of bytes that constitute an OID or a prefix
        ///  of an OID.
        /// </summary>
        [Size("length")]
        public byte[] elements;
    }

    /// <summary>
    ///  The ATTRVAL structure defines a concrete type for the
    ///  value of a single attribute.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\cc002cbf-efe0-42f8-9295-a5a6577263d4.xml
    //  </remarks>
    public partial struct ATTRVAL
    {
        /// <summary>
        ///  The size, in bytes, of the pVal array.
        /// </summary>
        [PossibleValueRange("0", "26214400")]
        [CLSCompliant(false)]
        public uint valLen;

        /// <summary>
        ///  The value of the attribute. The encoding of the attribute
        ///  varies by syntax, as described in the following sections.
        /// </summary>
        [Size("valLen")]
        public byte[] pVal;
    }

    /// <summary>
    ///  The SVCERR_DRS_WIRE_V1 structure defines a service error.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\d05a3b5d-4944-49a9-93cf-95e9f56e20e7.xml
    //  </remarks>
    public partial struct SVCERR_DRS_WIRE_V1
    {

        /// <summary>
        ///  The implementation-specific diagnostic code.
        /// </summary>
        [CLSCompliant(false)]
        public uint dsid;

        /// <summary>
        ///  0, STATUS code, or Windows error code.
        /// </summary>
        [CLSCompliant(false)]
        public uint extendedErr;

        /// <summary>
        ///  The implementation-specific diagnostic code.
        /// </summary>
        [CLSCompliant(false)]
        public uint extendedData;

        /// <summary>
        ///  0 or PROBLEM error code (section ).
        /// </summary>
        [CLSCompliant(false)]
        public ushort problem;
    }

    /// <summary>
    ///  The DRS_MSG_INIT_DEMOTIONREQ_V1 structure defines a
    ///  request message sent to the IDL_DRSInitDemotion method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\d71cd783-4d7b-443c-adfe-6b3a77a19671.xml
    //  </remarks>
    public partial struct DRS_MSG_INIT_DEMOTIONREQ_V1
    {

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public DRS_MSG_INIT_DEMOTIONREQ_V1_dwReserved_Values dwReserved;
    }

    /// <summary>
    /// The reserved of DRS_MSG_INIT_DEMOTIONREQ_V1.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum DRS_MSG_INIT_DEMOTIONREQ_V1_dwReserved_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The DS_NAME_RESULT_ITEMW structure defines the translated
    ///  name returned by the IDL_DRSCrackNames method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\e174fead-5a37-4a11-a0f6-69086e8dd4e9.xml
    //  </remarks>
    public partial struct DS_NAME_RESULT_ITEMW
    {

        /// <summary>
        ///  Status of the crack name operation for the corresponding
        ///  element of the rpNames field in the request. The status
        ///  is one of the values from the enumeration DS_NAME_ERROR.
        /// </summary>
        [CLSCompliant(false)]
        public DS_NAME_ERROR status;

        /// <summary>
        ///  DNS domain name of the domain in which the named object
        ///  resides.
        /// </summary>
        [String()]
        public string pDomain;

        /// <summary>
        ///  Object name in the requested format.
        /// </summary>
        [String()]
        public string pName;
    }

    /// <summary>
    ///  The COMPRESSED_DATA structure defines a sequence of
    ///  compressed (if cbDecompressedSize ≠ cbCompressedSize)
    ///  or uncompressed (if cbDecompressedSize = cbCompressedSize)
    ///  bytes.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\e4380043-1647-4869-9d68-3c0fefa5edd7.xml
    //  </remarks>
    public partial struct COMPRESSED_DATA
    {

        /// <summary>
        ///  The size of decompressed data.
        /// </summary>
        [CLSCompliant(false)]
        public uint cbDecompressedSize;

        /// <summary>
        ///  The size of compressed data.
        /// </summary>
        [CLSCompliant(false)]
        public uint cbCompressedSize;

        /// <summary>
        ///  Data stream. The data is padded with zeros, if necessary,
        ///  so that the block ends on a double word boundary.
        /// </summary>
        [Inline()]
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] data;
    }

    /// <summary>
    ///  The DRS_MSG_FINISH_DEMOTIONREPLY_V1 structure defines
    ///  the response message received from the IDL_DRSFinishDemotion
    ///  method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\e7fee5ef-4d7f-459e-b7c2-4e3b709a2f31.xml
    //  </remarks>
    public partial struct DRS_MSG_FINISH_DEMOTIONREPLY_V1
    {

        /// <summary>
        ///  The set of operations that were successfully performed.
        ///  This may include the following values: DS_DEMOTE_ROLLBACK_DEMOTE,
        ///  DS_DEMOTE_COMMIT_DEMOTE, DS_DEMOTE_DELETE_CSMETA, DS_DEMOTE_UNREGISTER_SCPS,
        ///  DS_DEMOTE_UNREGISTER_SPNS.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwOperationsDone;

        /// <summary>
        ///  The set of operations that failed during demotion. This
        ///  may include the same values as the dwOperationsDone
        ///  field.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwOpFailed;

        /// <summary>
        ///  The Win32 error code (as specified in [MS-ERREF] section
        ///  ) of the first failed operation (if any).
        /// </summary>
        [CLSCompliant(false)]
        public uint dwOpError;
    }

    /// <summary>
    ///  The DRS_EXTENSIONS structure defines a concrete type
    ///  for capabilities information used in version negotiation.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\ed0c5dc1-7566-48b3-be08-4c5e26ba60c4.xml
    //  </remarks>
    public partial struct DRS_EXTENSIONS
    {

        /// <summary>
        ///  The size, in bytes, of the rgb array.
        /// </summary>
        [CLSCompliant(false)]
        public uint cb;

        /// <summary>
        ///  To RPC, this field is a string of cb bytes. It is interpreted
        ///  by the client and the server as the first cb bytes
        ///  of a DRS_EXTENSIONS_INT structure that follow the cb
        ///  field of that structure. The fields of the DRS_EXTENSIONS_INT
        ///  structure are in little-endian byte order. Since both
        ///  DRS_EXTENSIONS and DRS_EXTENSIONS_INT begin with a
        ///  DWORDcb, a field in DRS_EXTENSIONS_INT is at the same
        ///  offset in DRS_EXTENSIONS as it is in DRS_EXTENSIONS_INT.
        /// </summary>
        [Size("cb")]
        [Inline()]
        public byte[] rgb;
    }

    /// <summary>
    ///  The DRS_MSG_RMDMNREPLY_V1 structure defines a response
    ///  message received from the IDL_DRSRemoveDsDomain method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\ed5d05b9-7264-49fa-8cc4-d393791eec86.xml
    //  </remarks>
    public partial struct DRS_MSG_RMDMNREPLY_V1
    {

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public DRS_MSG_RMDMNREPLY_V1_Reserved_Values Reserved;
    }

    /// <summary>
    ///  The reserved values of DRS_MSG_RMDMNREPLY_V1.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum DRS_MSG_RMDMNREPLY_V1_Reserved_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The DRS_MSG_RMSVRREPLY_V1 structure defines a response
    ///  message received from the IDL_DRSRemoveDsServer method.
    ///  Only one version, identified by pdwOutVersion^ = 1,
    ///  is currently defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\f567b19b-5349-4322-8e74-0925203ee682.xml
    //  </remarks>
    public partial struct DRS_MSG_RMSVRREPLY_V1
    {

        /// <summary>
        ///  True if the DC is the last DC in its domain, and pmsgIn^.V1.DomainDN
        ///  was set to the DN of the NC root of the domain to which
        ///  the DC belongs. Otherwise, false.
        /// </summary>
        [CLSCompliant(false)]
        public int fLastDcInDomain;
    }

    /// <summary>
    ///  The DRS_MSG_RMDMNREQ_V1 structure defines a request
    ///  message sent to the IDL_DRSRemoveDsDomain method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\f7f04091-302e-470f-ae47-7244192fe599.xml
    //  </remarks>
    public partial struct DRS_MSG_RMDMNREQ_V1
    {

        /// <summary>
        ///  The DN of the NC root of the domain NC to remove.
        /// </summary>
        [String()]
        public string DomainDN;
    }

    /// <summary>
    ///  The DS_NAME_RESULTW structure defines the translated
    ///  names returned by the IDL_DRSCrackNames method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\0076d241-3f79-4b0b-8e07-8ccfaff8bd4c.xml
    //  </remarks>
    public partial struct DS_NAME_RESULTW
    {

        /// <summary>
        ///  The count of items in the rItems array.
        /// </summary>
        [CLSCompliant(false)]
        public uint cItems;

        /// <summary>
        ///  Translated names that correspond one-to-one with the
        ///  elements in the rpNames field of the request.
        /// </summary>
        [Size("cItems")]
        public DS_NAME_RESULT_ITEMW[] rItems;
    }

    /// <summary>
    ///  The DS_DOMAIN_CONTROLLER_INFO_3W structure defines DC
    ///  information that is returned as a part of the response
    ///  to an InfoLevel = 3 request. The struct contains information
    ///  about a single DC in the domain.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\08f99ee7-8235-482b-bfe5-c6170f133cd4.xml
    //  </remarks>
    public partial struct DS_DOMAIN_CONTROLLER_INFO_3W
    {

        /// <summary>
        ///  NetBIOS name of the DC.
        /// </summary>
        [String()]
        public string NetbiosName;

        /// <summary>
        ///  DNS host name of the DC.
        /// </summary>
        [String()]
        public string DnsHostName;

        /// <summary>
        ///  RDN of the siteobject.
        /// </summary>
        [String()]
        public string SiteName;

        /// <summary>
        ///  DN of the siteobject.
        /// </summary>
        [String()]
        public string SiteObjectName;

        /// <summary>
        ///  DN of the computerobject that corresponds to the DC.
        /// </summary>
        [String()]
        public string ComputerObjectName;

        /// <summary>
        ///  DN of the serverobject that corresponds to the DC.
        /// </summary>
        [String()]
        public string ServerObjectName;

        /// <summary>
        ///  DN of the nTDSDSAobject that corresponds to the DC.
        /// </summary>
        [String()]
        public string NtdsDsaObjectName;

        /// <summary>
        ///  True if and only if the DC is the PDCFSMO role owner.
        /// </summary>
        [CLSCompliant(false)]
        public int fIsPdc;

        /// <summary>
        ///  MUST be true.
        /// </summary>
        [CLSCompliant(false)]
        public int fDsEnabled;

        /// <summary>
        ///  True if and only if the DC is also a GC.
        /// </summary>
        [CLSCompliant(false)]
        public int fIsGc;

        /// <summary>
        ///  True if and only if the DC is an RODC.
        /// </summary>
        [CLSCompliant(false)]
        public int fIsRodc;

        /// <summary>
        ///  objectGUID of the siteobject.
        /// </summary>
        public Guid SiteObjectGuid;

        /// <summary>
        ///  objectGUID of the computerobject that corresponds to
        ///  the DC.
        /// </summary>
        public Guid ComputerObjectGuid;

        /// <summary>
        ///  objectGUID of the serverobject that corresponds to the
        ///  DC.
        /// </summary>
        public Guid ServerObjectGuid;

        /// <summary>
        ///  objectGUID of the nTDSDSAobject that corresponds to
        ///  the DC.
        /// </summary>
        public Guid NtdsDsaObjectGuid;
    }

    /// <summary>
    ///  The DRS_MSG_QUERYSITESREQ union defines the request
    ///  message versions sent to the IDL_DRSQuerySitesByCost
    ///  method. Only one version, identified by dwVersion =
    ///  1, is currently defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\196d891f-2c52-418c-9cf7-8b679d66e457.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_QUERYSITESREQ
    {

        /// <summary>
        ///  The version 1 request.
        /// </summary>
        [Case("1")]
        public DRS_MSG_QUERYSITESREQ_V1 V1;
    }

    /// <summary>
    ///  The DRS_MSG_QUERYSITESREPLY_V1 structure defines a response
    ///  message received from the IDL_DRSQuerySitesByCost method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\1fdb5988-9d5c-47fd-a917-c579360f6188.xml
    //  </remarks>
    public partial struct DRS_MSG_QUERYSITESREPLY_V1
    {

        /// <summary>
        ///  The number of items in the rgCostInfo array.
        /// </summary>
        [PossibleValueRange("0", "10000")]
        [CLSCompliant(false)]
        public uint cToSites;

        /// <summary>
        ///  The sequence of computed site costs, in the same order
        ///  as the rgszToSites field in the request message.
        /// </summary>
        [Size("cToSites")]
        public DRS_MSG_QUERYSITESREPLYELEMENT_V1[] rgCostInfo;

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public DRS_MSG_QUERYSITESREPLY_V1_dwFlags_Values dwFlags;
    }

    /// <summary>
    /// The flags of DRS_MSG_QUERYSITESREPLY_V1.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum DRS_MSG_QUERYSITESREPLY_V1_dwFlags_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The DRS_MSG_INIT_DEMOTIONREPLY union defines the response
    ///  messages received from the IDL_DRSInitDemotion method.
    ///  Only one version, identified by pdwOutVersion^ = 1,
    ///  is currently defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\216ff6cb-3785-40f5-bbb0-7c3076724105.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_INIT_DEMOTIONREPLY
    {

        /// <summary>
        ///  Version 1 reply.
        /// </summary>
        [Case("1")]
        public DRS_MSG_INIT_DEMOTIONREPLY_V1 V1;
    }

    /// <summary>
    ///  The Cookie structure is a concrete type that contains
    ///  information about the cookie in the LDAP_SERVER_DIRSYNC_OID
    ///  control value.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\2313edaf-4e34-4b55-8acc-f8ad1593478f.xml
    //  </remarks>
    public partial struct Cookie
    {

        /// <summary>
        ///  Cookie signature.
        /// </summary>
        [Inline()]
        [StaticSize(4, StaticSizeMode.Elements)]
        public byte[] signature;

        /// <summary>
        ///  The version number.
        /// </summary>
        [CLSCompliant(false)]
        public uint version;

        /// <summary>
        ///  The creation time.
        /// </summary>
        [CLSCompliant(false)]
        public _FILETIME creationTime;

        /// <summary>
        ///  Unused.
        /// </summary>
        [CLSCompliant(false)]
        public ulong reserved;

        /// <summary>
        ///  The up-to-date vector size.
        /// </summary>
        [CLSCompliant(false)]
        public uint utdVectorSize;

        /// <summary>
        ///  The USN vector.
        /// </summary>
        public USN_VECTOR usnVector;

        /// <summary>
        ///  The invocation ID (a UUID) of the source DSA.
        /// </summary>
        public Guid uuidSourceDsaInvocationID;

        /// <summary>
        ///  The up-to-date vector.
        /// </summary>
        [Inline()]
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] utdVector;
    }

    /// <summary>
    ///  The DRS_MSG_SPNREPLY union defines the response messages
    ///  received from the IDL_DRSWriteSPN method. Only one
    ///  version, identified by pdwOutVersion^ = 1, is currently
    ///  defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\25b79282-8bcf-4366-b4ac-576d93132f31.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_SPNREPLY
    {

        /// <summary>
        ///  The version 1 response.
        /// </summary>
        [Case("1")]
        public DRS_MSG_SPNREPLY_V1 V1;
    }

    /// <summary>
    ///  The DRS_MSG_GETCHGREPLY_V7 structure defines a compressed
    ///  DRS_MSG_GETCHGREPLY_V6 message received from the IDL_DRSGetNCChanges
    ///  method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\26eaca61-0f19-47e7-b304-2580e9870aa8.xml
    //  </remarks>
    public partial struct DRS_MSG_GETCHGREPLY_V7
    {

        /// <summary>
        ///  Version of the response in CompressedAny; MUST be set
        ///  to 6.
        /// </summary>
        [CLSCompliant(false)]
        public dwCompressedVersion_Values dwCompressedVersion;

        /// <summary>
        ///  Algorithm used to compress the response.
        /// </summary>
        public DRS_COMP_ALG_TYPE CompressionAlg;

        /// <summary>
        ///  Compressed DRS_MSG_GETCHGREPLY_V6 response.
        /// </summary>
        public DRS_COMPRESSED_BLOB CompressedAny;
    }



    /// <summary>
    ///  The dwCompressedVersion values of DRS_MSG_GETCHGREPLY_V7.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [CLSCompliant(false)]
    public enum dwCompressedVersion_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 6,
    }

    /// <summary>
    ///  The DSA_ADDRESS_LIST_DRS_WIRE_V1 structure defines a
    ///  linked list entry for a referral network name.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\2f6ca421-320d-4fee-96b6-56315c446236.xml
    //  </remarks>
    [CLSCompliant(false)]
    public partial struct _DSA_ADDRESS_LIST_DRS_WIRE_V1
    {

        /// <summary>
        ///  Null, or the next element in the linked list.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _DSA_ADDRESS_LIST_DRS_WIRE_V1[] pNextAddress;

        /// <summary>
        ///  Network name of the DC to which the referral is directed.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _RPC_UNICODE_STRING[] pAddress;
    }

    /// <summary>
    ///  The DRS_MSG_EXISTREPLY_V1 structure defines the response
    ///  message received from the IDL_DRSGetObjectExistence
    ///  method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\324d0510-5947-4c86-89e8-38b6628bca2e.xml
    //  </remarks>
    public partial struct DRS_MSG_EXISTREPLY_V1
    {

        /// <summary>
        ///  1 if the digests of the object sequences on the client
        ///  and server are the same, 0 if they are different.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwStatusFlags;

        /// <summary>
        ///  The number of items in the rgGuids array. Zero if dwStatusFlags
        ///  = 0.
        /// </summary>
        [PossibleValueRange("0", "10485760")]
        [CLSCompliant(false)]
        public uint cNumGuids;

        /// <summary>
        ///  The objectGUIDs of the objects in the server's object
        ///  sequence.
        /// </summary>
        [Size("cNumGuids")]
        public Guid[] rgGuids;
    }

    /// <summary>
    ///  The DRS_MSG_CRACKREPLY_V1 structure defines the response
    ///  message received from the IDL_DRSCrackNames method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\3419de89-0d54-462e-98ac-fb77292c91e7.xml
    //  </remarks>
    public partial struct DRS_MSG_CRACKREPLY_V1
    {

        /// <summary>
        ///  Translated form of the names.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public DS_NAME_RESULTW[] pResult;
    }

    /// <summary>
    ///  DSNAME is a concrete type for representing a DSName,
    ///  identifying a directory object using the values of
    ///  one or more of its LDAPattributes: objectGUID, objectSid,
    ///  or distinguishedName.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\385d478f-3eb6-4d2c-ac58-f25c4debdd86.xml
    //  </remarks>
    public partial struct DSNAME
    {

        /// <summary>
        ///  The length, in bytes, of the entire data structure.
        /// </summary>
        [CLSCompliant(false)]
        public uint structLen;

        /// <summary>
        ///  The number of bytes in the Sid field used to represent
        ///  the object'sobjectSidattribute value. Zero indicates
        ///  that the DSNAME does not identify the objectSid value
        ///  of the directory object.
        /// </summary>
        [CLSCompliant(false)]
        public uint SidLen;

        /// <summary>
        ///  The value of the object's objectGUIDattribute specified
        ///  as a GUID structure, which is defined in [MS-DTYP]
        ///  section. If the values for all fields in the GUID
        ///  structure are zero, this indicates that the DSNAME
        ///  does not identify the objectGUID value of the directory
        ///  object.
        /// </summary>
        public Guid Guid;

        /// <summary>
        ///  The value of the object'sobjectSidattribute, its security
        ///  identifier (see [MS-SECO] section ), specified as a
        ///  SID structure, which is defined in [MS-DTYP] section
        ///  . The size of this field is exactly 28 bytes, regardless
        ///  of the value of SidLen, which specifies how many bytes
        ///  in this field are used. Note that this is smaller than
        ///  the theoretical size limit of a SID, which is 68 bytes.
        ///  While windows publishes a general SID format, windows
        ///  never uses that format in its full generality. 28 bytes
        ///  is sufficient for a windowsSID.
        /// </summary>
        public NT4SID Sid;

        /// <summary>
        ///  The number of characters in the StringName field, not
        ///  including the terminating null character, used to represent
        ///  the object's distinguishedNameattribute value. Zero
        ///  indicates that the DSNAME does not identify the distinguishedName
        ///  value of the directory object.
        /// </summary>
        [CLSCompliant(false)]
        public uint NameLen;

        /// <summary>
        ///  A null-terminated Unicode value of the object'sdistinguishedNameattribute,
        ///  as specified in [MS-ADTS] section. This field always
        ///  contains at least one character: the terminating null
        ///  character. Each Unicode value is encoded as 2 bytes.
        ///  The byte ordering is little-endian. No range is supported
        ///  on any member of DSNAME in windows_2000_server. A range
        ///  of 0 to 10485760 is supported on the NameLen member
        ///  of DSNAME in windows_server_2003. A range of 0 to 10485761
        ///  is supported on the StringName member of DSNAME in
        ///  windows_server_2008 and windows_server_7.
        /// </summary>
        [Inline()]
        [Size("NameLen + 1")]
        [CLSCompliant(false)]
        public ushort[] StringName;
    }

    /// <summary>
    ///  The DS_REPL_CURSOR_2 structure defines a replication
    ///  cursor for a given NC replica. This structure is a
    ///  concrete representation of a ReplUpToDateVector value;
    ///  it is a superset of DS_REPL_CURSOR.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\40366a5b-9a48-465f-b7ac-03f56334f76d.xml
    //  </remarks>
    public partial struct DS_REPL_CURSOR_2
    {

        /// <summary>
        ///  The invocation ID of a DC.
        /// </summary>
        public Guid uuidSourceDsaInvocationID;

        /// <summary>
        ///  The USN at which an update was applied on the DC.
        /// </summary>
        public long usnAttributeFilter;

        /// <summary>
        ///  The time at which the last successful replication occurred
        ///  from the DC identified by uuidDsa. Used for replication
        ///  latency reporting only.
        /// </summary>
        [CLSCompliant(false)]
        public _FILETIME ftimeLastSyncSuccess;
    }

    /// <summary>
    ///  The DRS_MSG_SPNREQ union defines the request messages
    ///  sent to the IDL_DRSWriteSPN method. Only one version,
    ///  identified by dwInVersion = 1, is currently defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\429b5b4d-95b4-4d52-87e7-5315d978c426.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_SPNREQ
    {

        /// <summary>
        ///  The version 1 request.
        /// </summary>
        [Case("1")]
        public DRS_MSG_SPNREQ_V1 V1;
    }

    /// <summary>
    ///  The DS_REPL_SERVER_OUTGOING_CALLS structure defines
    ///  a set of outstanding requests from this DC to other
    ///  DCs. This structure is a concrete representation of
    ///  RPCOutgoingContexts.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\4a8c3581-1f94-48f5-8c02-f2d2ec6cef46.xml
    //  </remarks>
    public partial struct DS_REPL_SERVER_OUTGOING_CALLS
    {

        /// <summary>
        ///  The number of items in the rgCall array.
        /// </summary>
        [PossibleValueRange("0", "256")]
        [CLSCompliant(false)]
        public uint cNumCalls;

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public DS_REPL_SERVER_OUTGOING_CALLS_dwReserved_Values dwReserved;

        /// <summary>
        ///  A set of outstanding requests from this DC to other
        ///  DCs.
        /// </summary>
        [Inline()]
        [Size("cNumCalls")]
        public DS_REPL_SERVER_OUTGOING_CALL[] rgCall;
    }

    /// <summary>
    /// The reserved of DS_REPL_SERVER_OUTGOING_CALLS.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum DS_REPL_SERVER_OUTGOING_CALLS_dwReserved_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The DRS_MSG_QUERYSITESREPLY union defines the response
    ///  messages received from the IDL_DRSQuerySitesByCost
    ///  method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\558cedbb-70fc-4249-934d-159a170cc519.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_QUERYSITESREPLY
    {

        /// <summary>
        ///  The version 1 response.
        /// </summary>
        [Case("1")]
        public DRS_MSG_QUERYSITESREPLY_V1 V1;
    }

    /// <summary>
    ///  The DRS_MSG_MOVEREPLY_V2 structure defines a response
    ///  message received from the IDL_DRSInterDomainMove method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\59b23876-f18c-44bd-b88f-3848d256c61e.xml
    //  </remarks>
    public partial struct DRS_MSG_MOVEREPLY_V2
    {

        /// <summary>
        ///  0 if successful, or non-zero if a fatal error occurred.
        /// </summary>
        [CLSCompliant(false)]
        public uint win32Error;

        /// <summary>
        ///  The name of the object in its new domain.
        /// </summary>
        [Indirect()]
        public DSNAME? pAddedName;
    }

    /// <summary>
    ///  The DSA_MSG_EXECUTE_SCRIPT_REQ union defines the request
    ///  messages sent to the IDL_DSAExecuteScript method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\5c4028fa-d46f-4e36-bc38-1f4f2378de53.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DSA_MSG_EXECUTE_SCRIPT_REQ
    {

        /// <summary>
        ///  The version 1 request.
        /// </summary>
        [Case("1")]
        public DSA_MSG_EXECUTE_SCRIPT_REQ_V1 V1;
    }

    /// <summary>
    ///  The DS_REPL_KCC_DSA_FAILUREW structure defines a DC
    ///  that is in a replication error state. This structure
    ///  is a concrete representation of a tuple in a KCCFailedConnections
    ///  or KCCFailedLinks sequence.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\5d5ac3d8-dc80-401b-9ca8-1e7a385024a4.xml
    //  </remarks>
    public partial struct DS_REPL_KCC_DSA_FAILUREW
    {

        /// <summary>
        ///  The DN of the nTDSDSAobject corresponding to the DC.
        /// </summary>
        [String()]
        public string pszDsaDN;

        /// <summary>
        ///  The DSA GUID of the DC.
        /// </summary>
        public Guid uuidDsaObjGuid;

        /// <summary>
        ///  The date and time at which the DC entered an error state.
        /// </summary>
        [CLSCompliant(false)]
        public _FILETIME ftimeFirstFailure;

        /// <summary>
        ///  The number of errors that have occurred.
        /// </summary>
        [CLSCompliant(false)]
        public uint cNumFailures;

        /// <summary>
        ///  The Windows error code for the last error.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwLastResult;
    }

    /// <summary>
    ///  The DRS_MSG_KCC_EXECUTE union defines the request messages
    ///  sent to the IDL_DRSExecuteKCC method. Only one version,
    ///  identified by dwInVersion = 1, is currently defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\5e5f7a58-a501-43de-a8e1-db8de9960c54.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_KCC_EXECUTE
    {

        /// <summary>
        ///  Version 1 request.
        /// </summary>
        [Case("1")]
        public DRS_MSG_KCC_EXECUTE_V1 V1;
    }

    /// <summary>
    ///  The DRS_MSG_DCINFOREPLY_VFFFFFFFF structure defines
    ///  the response message received from the IDL_DRSDomainControllerInfo
    ///  method, when the client has requested InfoLevel = 0xFFFFFFFF.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\625c5133-cb5b-440a-9f53-232ae1b2dc3f.xml
    //  </remarks>
    public partial struct DRS_MSG_DCINFOREPLY_VFFFFFFFF
    {

        /// <summary>
        ///  The count of items in the rItems array.
        /// </summary>
        [PossibleValueRange("0", "10000")]
        [CLSCompliant(false)]
        public uint cItems;

        /// <summary>
        ///  DC information.
        /// </summary>
        [Size("cItems")]
        public DS_DOMAIN_CONTROLLER_INFO_FFFFFFFFW[] rItems;
    }

    /// <summary>
    ///  The DRS_MSG_GETCHGREPLY_V2 structure defines the compressed
    ///  DRS_MSG_GETCHGREPLY_V1 message received from the IDL_DRSGetNCChanges
    ///  method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\677d8fab-6aa1-4327-9b6f-62a6ad7fcfa3.xml
    //  </remarks>
    public partial struct DRS_MSG_GETCHGREPLY_V2
    {

        /// <summary>
        ///  Compressed DRS_MSG_GETCHGREPLY_V1 response.
        /// </summary>
        public DRS_COMPRESSED_BLOB CompressedV1;
    }

    /// <summary>
    ///  The DRS_MSG_FINISH_DEMOTIONREQ_V1 structure defines
    ///  the request message sent to the IDL_DRSFinishDemotion
    ///  method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\687fcbf2-2e1f-467a-9a1c-e2c34b3f7ce1.xml
    //  </remarks>
    public partial struct DRS_MSG_FINISH_DEMOTIONREQ_V1
    {

        /// <summary>
        ///  Zero or more of the following bit flags. 0 1 2 3 4 5
        ///  6 7 8 9 10 1 2 3 4 5 6 7 8 9 20 1 2 3 4 5 6 7 8 9 30
        ///  1 R C D U 1 U 2 X X X X X X X X X X X X X X X X X X
        ///  X X X X X X X X FX: Unused. MUST be zero and ignored.R
        ///  (DS_DEMOTE_ROLLBACK_DEMOTE): Undo the effects of IDL_DRSInitDemotion.
        ///  If present, any other flags present are ignored.C (DS_DEMOTE_COMMIT_DEMOTE):
        ///  Mark the DC's database "demotion-complete" (this effect
        ///  is outside the state model).D (DS_DEMOTE_DELETE_CSMETA):
        ///  Delete the nTDSDSAobject for this DC; see RemoveADLDSServer.U1
        ///  (DS_DEMOTE_UNREGISTER_SCPS): Delete any serviceConnectionPointobjects
        ///  for this DC from AD DS; see RemoveADLDSSCP.U2 (DS_DEMOTE_UNREGISTER_SPNS):
        ///  Delete any AD LDSSPNs from the service account object
        ///  in AD DS; see RemoveADLDSSPNs.F (DS_DEMOTE_OPT_FAIL_ON_UNKNOWN_OP):
        ///  Fail the request if the dwOperations field contains
        ///  an unknown flag.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwOperations;

        /// <summary>
        ///  Unused. Must be NULL GUID and ignored.
        /// </summary>
        public Guid uuidHelperDest;

        /// <summary>
        ///  The path name of the folder in which to store SPN unregistration
        ///  scripts. Required when DS_DEMOTE_UNREGISTER_SPNS is
        ///  specified in dwOperations.
        /// </summary>
        [String()]
        public string szScriptBase;
    }

    /// <summary>
    ///  The DRS_MSG_RMSVRREPLY union defines the response messages
    ///  received from the IDL_DRSRemoveDsServer method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\6a28e865-3002-468e-860d-0f3cd251ceb5.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_RMSVRREPLY
    {

        /// <summary>
        ///  The version 1 response.
        /// </summary>
        [Case("1")]
        public DRS_MSG_RMSVRREPLY_V1 V1;
    }

    /// <summary>
    ///  The DRS_MSG_DCINFOREQ union defines the request messages
    ///  sent to the IDL_DRSDomainControllerInfo method. Only
    ///  one version, identified by dwInVersion = 1, is currently
    ///  defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\6ac9ec30-5bfb-4970-860c-3971eb815930.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_DCINFOREQ
    {

        /// <summary>
        ///  Version 1 request.
        /// </summary>
        [Case("1")]
        public DRS_MSG_DCINFOREQ_V1 V1;
    }

    /// <summary>
    ///  The DS_REPL_CURSOR_3W structure defines a replication
    ///  cursor for a given NC replica. This structure is a
    ///  concrete representation of a ReplUpToDateVector value;
    ///  it is a superset of DS_REPL_CURSOR_2.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\6c7bd13e-06b8-459a-87fc-e67c7954290d.xml
    //  </remarks>
    public partial struct DS_REPL_CURSOR_3W
    {

        /// <summary>
        ///  The invocation ID of a DC.
        /// </summary>
        public Guid uuidSourceDsaInvocationID;

        /// <summary>
        ///  The USN at which an update was applied on the DC.
        /// </summary>
        public long usnAttributeFilter;

        /// <summary>
        ///  The time at which the last successful replication occurred
        ///  from the DC identified by uuidDsa. Used for replication
        ///  latency reporting only.
        /// </summary>
        [CLSCompliant(false)]
        public _FILETIME ftimeLastSyncSuccess;

        /// <summary>
        ///  The DN of the nTDSDSAobject with an invocationId of
        ///  uuidSourceDsaInvocationID.
        /// </summary>
        [String()]
        public string pszSourceDsaDN;
    }

    /// <summary>
    ///  The DRS_MSG_REPADD_V1 structure defines a request message
    ///  sent to the IDL_DRSReplicaAdd method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\7dc022f4-41a5-4d78-82f4-9db6e60454b9.xml
    //  </remarks>
    public partial struct DRS_MSG_REPADD_V1
    {

        /// <summary>
        ///  The NC root of the NC to replicate.
        /// </summary>
        [Indirect()]
        public DSNAME? pNC;

        /// <summary>
        ///  The transport-specific NetworkAddress of the DC from
        ///  which to replicate updates.
        /// </summary>
        [String(StringEncoding.ASCII)]
        public string pszDsaSrc;

        /// <summary>
        ///  The schedule used to perform periodic replication.
        /// </summary>
        public REPLTIMES rtSchedule;

        /// <summary>
        ///  Zero or more DRS_OPTIONS flags.
        /// </summary>
        [CLSCompliant(false)]
        public uint ulOptions;
    }

    /// <summary>
    ///  The DRS_MSG_REPLICA_DEMOTIONREPLY union defines the
    ///  response messages received from the IDL_DRSReplicaDemotion
    ///  method. Only one version, identified by pdwOutVersion^
    ///  = 1, is currently defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\7ecf3724-4dff-431b-bfcb-327a62a11b26.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_REPLICA_DEMOTIONREPLY
    {

        /// <summary>
        ///  The version 1 reply.
        /// </summary>
        [Case("1")]
        public DRS_MSG_REPLICA_DEMOTIONREPLY_V1 V1;
    }

    /// <summary>
    ///  The DSA_MSG_EXECUTE_SCRIPT_REPLY union defines the response
    ///  messages received from the IDL_DSAExecuteScript method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\7f37deb3-fd0d-4d4f-a195-f2dbb74220c5.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DSA_MSG_EXECUTE_SCRIPT_REPLY
    {

        /// <summary>
        ///  The version 1 request.
        /// </summary>
        [Case("1")]
        public DSA_MSG_EXECUTE_SCRIPT_REPLY_V1 V1;
    }

    /// <summary>
    ///  The DS_REPL_ATTR_META_DATA_2 structure defines an attributestamp
    ///  for a given object. This structure is a concrete representation
    ///  of an AttributeStamp; it is a superset of DS_REPL_ATTR_META_DATA.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\8036e127-f8ae-4341-b218-86db427d5791.xml
    //  </remarks>
    public partial struct DS_REPL_ATTR_META_DATA_2
    {

        /// <summary>
        ///  The lDAPDisplayName of the attribute to which the stamp
        ///  corresponds.
        /// </summary>
        [String()]
        public string pszAttributeName;

        /// <summary>
        ///  The stamp version.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwVersion;

        /// <summary>
        ///  The date and time at which the last originating update
        ///  was made.
        /// </summary>
        [CLSCompliant(false)]
        public _FILETIME ftimeLastOriginatingChange;

        /// <summary>
        ///  The invocation ID of the DC that performed the last
        ///  originating update.
        /// </summary>
        public Guid uuidLastOriginatingDsaInvocationID;

        /// <summary>
        ///  The USN assigned to the last originating update by the
        ///  DC that performed it.
        /// </summary>
        public long usnOriginatingChange;

        /// <summary>
        ///  An implementation-specific value.
        /// </summary>
        public long usnLocalChange;

        /// <summary>
        ///  The DN of the nTDSDSAobject with an invocationId of
        ///  uuidLastOriginatingDsaInvocationID.
        /// </summary>
        [String()]
        public string pszLastOriginatingDsaDN;
    }

    /// <summary>
    ///  The DRS_MSG_GETREPLINFO_REQ_V1 structure defines a version
    ///  1 request message sent to the IDL_DRSGetReplInfo method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\8437a79c-4d00-46a1-b13e-a7f97d2f6608.xml
    //  </remarks>
    public partial struct DRS_MSG_GETREPLINFO_REQ_V1
    {

        /// <summary>
        ///  MUST be a DS_REPL_INFO code.
        /// </summary>
        [CLSCompliant(false)]
        public uint InfoType;

        /// <summary>
        ///  DN of the object on which the operation should be performed.
        ///  The meaning of this parameter depends on the value
        ///  of the InfoType parameter.
        /// </summary>
        [String()]
        public string pszObjectDN;

        /// <summary>
        ///  NULL GUID or the DSA GUID of a DC.
        /// </summary>
        public Guid uuidSourceDsaObjGuid;
    }

    /// <summary>
    ///  The DRS_MSG_RMDMNREPLY union defines the response messages
    ///  received from the IDL_DRSRemoveDsDomain method. Only
    ///  one version, identified by pdwOutVersion^ = 1, is currently
    ///  defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\88a1c1ea-a79a-46c8-9e0c-ea94a45605cb.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_RMDMNREPLY
    {

        /// <summary>
        ///  The version 1 response.
        /// </summary>
        [Case("1")]
        public DRS_MSG_RMDMNREPLY_V1 V1;
    }

    /// <summary>
    ///  The DRS_MSG_REPADD_V2 structure defines a request message
    ///  sent to the IDL_DRSReplicaAdd method. This request
    ///  version is a superset of V1.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\892d8577-6e15-45ba-8e7a-022807ed8649.xml
    //  </remarks>
    public partial struct DRS_MSG_REPADD_V2
    {

        /// <summary>
        ///  The NC root of the NC to replicate.
        /// </summary>
        [Indirect()]
        public DSNAME? pNC;

        /// <summary>
        ///  The nTDSDSAobject for the DC from which to replicate
        ///  changes.
        /// </summary>
        [Indirect()]
        public DSNAME? pSourceDsaDN;

        /// <summary>
        ///  The interSiteTransportobject that identifies the network
        ///  transport over which replication should be performed
        ///   to be used in the server-to-server replication implementation
        ///  with the specified DC .
        /// </summary>
        [Indirect()]
        public DSNAME? pTransportDN;

        /// <summary>
        ///  The transport-specific NetworkAddress of the DC from
        ///  which to replicate updates.
        /// </summary>
        [String(StringEncoding.ASCII)]
        public string pszSourceDsaAddress;

        /// <summary>
        ///  The schedule used to perform periodic replication.
        /// </summary>
        public REPLTIMES rtSchedule;

        /// <summary>
        ///  Zero or more DRS_OPTIONS flags.
        /// </summary>
        [CLSCompliant(false)]
        public uint ulOptions;
    }

    /// <summary>
    ///  The DRS_MSG_REPADD_V3 structure defines a request message
    ///  sent to the IDL_DRSReplicaAdd method. This request
    ///  version is a superset of V2.
    /// </summary>
    public partial struct DRS_MSG_REPADD_V3
    {

        /// <summary>
        ///  The NC root of the NC to replicate.
        /// </summary>
        [Indirect()]
        public DSNAME? pNC;

        /// <summary>
        ///  The nTDSDSAobject for the DC from which to replicate
        ///  changes.
        /// </summary>
        [Indirect()]
        public DSNAME? pSourceDsaDN;

        /// <summary>
        ///  The interSiteTransportobject that identifies the network
        ///  transport over which replication should be performed
        ///   to be used in the server-to-server replication implementation
        ///  with the specified DC .
        /// </summary>
        [Indirect()]
        public DSNAME? pTransportDN;

        /// <summary>
        ///  The transport-specific NetworkAddress of the DC from
        ///  which to replicate updates.
        /// </summary>
        [String(StringEncoding.ASCII)]
        public string pszSourceDsaAddress;

        /// <summary>
        ///  The schedule used to perform periodic replication.
        /// </summary>
        public REPLTIMES rtSchedule;

        /// <summary>
        ///  Zero or more DRS_OPTIONS flags.
        /// </summary>
        [CLSCompliant(false)]
        public uint ulOptions;

        /// <summary>
        /// An identifier for the operation that the DC can use for implementation-defined troubleshooting.
        /// There are no normative constraints on this value,
        /// nor does the value figure in any normative processing rules.
        /// </summary>
        public Guid correlationID;

        /// <summary>
        /// A pointer to a VAR_SIZE_BUFFER_WITH_VERSION structure (section 5.219).
        /// MUST be a null pointer.
        /// </summary>
        public VAR_SIZE_BUFFER_WITH_VERSION pReservedBuffer;
    }

    /// <summary>
    ///  The DRS_MSG_ADDSIDREPLY union defines the response messages
    ///  received from the IDL_DRSAddSidHistory method. Only
    ///  one version, identified by pdwOutVersion^ = 1, is currently
    ///  defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\8c600ba9-ada0-4771-932f-e4f68516d9bb.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_ADDSIDREPLY
    {

        /// <summary>
        ///  Version 1 of the reply packet structure.
        /// </summary>
        [Case("1")]
        public DRS_MSG_ADDSIDREPLY_V1 V1;
    }

    /// <summary>
    ///  The DRS_MSG_REPLICA_DEMOTIONREQ_V1 structure defines
    ///  a request message sent to the IDL_DRSReplicaDemotion
    ///  method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\8e459d5d-129e-40be-8bc9-0872f763c61d.xml
    //  </remarks>
    public partial struct DRS_MSG_REPLICA_DEMOTIONREQ_V1
    {

        /// <summary>
        ///  Zero or more of the following bit flags. 0 1 2 3 4 5
        ///  6 7 8 9 10 1 2 3 4 5 6 7 8 9 20 1 2 3 4 5 6 7 8 9 30
        ///  1 T X X X X X X X X X X X X X X X X X X X X X X X X
        ///  X X X X X X XX: Unused. MUST be zero and ignored.T
        ///  (DS_REPLICA_DEMOTE_TRY_ALL_SRCS): MUST be set.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwFlags;

        /// <summary>
        ///  Unused. Must be NULL GUID and ignored.
        /// </summary>
        public Guid uuidHelperDest;

        /// <summary>
        ///  The DSNAME of the NC to replicate off.
        /// </summary>
        [Indirect()]
        public DSNAME? pNC;
    }

    /// <summary>
    ///  The DS_REPL_VALUE_META_DATA structure defines a link
    ///  value stamp. This structure is a concrete representation
    ///  of a LinkValueStamp.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\8e53006b-9e1d-48e6-ba5f-c675c0a98b3a.xml
    //  </remarks>
    public partial struct DS_REPL_VALUE_META_DATA
    {

        /// <summary>
        ///  The lDAPDisplayName of the attribute.
        /// </summary>
        [String()]
        public string pszAttributeName;

        /// <summary>
        ///  The DN of the object.
        /// </summary>
        [String()]
        public string pszObjectDn;

        /// <summary>
        ///  The size, in bytes, of the pbData array.
        /// </summary>
        [CLSCompliant(false)]
        public uint cbData;

        /// <summary>
        ///  Null, or data associated with the attribute value if
        ///  the attribute is of syntax Object(DN-Binary) or Object(DN-String).
        /// </summary>
        [Size("cbData")]
        public byte[] pbData;

        /// <summary>
        ///  The date and time at which the last replicated update
        ///  was made that deleted the value, or 0 if the value
        ///  is not currently deleted.
        /// </summary>
        [CLSCompliant(false)]
        public _FILETIME ftimeDeleted;

        /// <summary>
        ///  The date and time at which the first originating update
        ///  was made.
        /// </summary>
        [CLSCompliant(false)]
        public _FILETIME ftimeCreated;

        /// <summary>
        ///  The stamp version.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwVersion;

        /// <summary>
        ///  The date and time at which the last originating update
        ///  was made.
        /// </summary>
        [CLSCompliant(false)]
        public _FILETIME ftimeLastOriginatingChange;

        /// <summary>
        ///  The invocation ID of the DC that performed the last
        ///  originating update.
        /// </summary>
        public Guid uuidLastOriginatingDsaInvocationID;

        /// <summary>
        ///  The USN assigned to the last originating update by the
        ///  DC that performed the update.
        /// </summary>
        public long usnOriginatingChange;

        /// <summary>
        ///  An implementation-specific value.
        /// </summary>
        public long usnLocalChange;
    }

    /// <summary>
    ///  The DSA_MSG_PREPARE_SCRIPT_REPLY union defines the response
    ///  messages received from the IDL_DSAPrepareScript method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\95850175-8d67-4d37-afc9-43346dc64a7b.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DSA_MSG_PREPARE_SCRIPT_REPLY
    {

        /// <summary>
        ///  The version 1 response.
        /// </summary>
        [Case("1")]
        public DSA_MSG_PREPARE_SCRIPT_REPLY_V1 V1;
    }

    /// <summary>
    ///  The CONTREF_DRS_WIRE_V1 structure defines a linked list
    ///  entry for a continuation referral.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\9b54d7aa-96a3-49cb-8a62-9d326f359b83.xml
    //  </remarks>
    public partial struct CONTREF_DRS_WIRE_V1
    {

        /// <summary>
        ///  The object to which the referral is directed.
        /// </summary>
        [Indirect()]
        public DSNAME? pTarget;

        /// <summary>
        ///  The operation state.
        /// </summary>
        public NAMERESOP_DRS_WIRE_V1 OpState;

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public aliasRDN_Values aliasRDN;

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public RDNsInternal_Values RDNsInternal;

        /// <summary>
        ///  The type of referral. This field MUST be one of the
        ///  following values.
        /// </summary>
        [CLSCompliant(false)]
        public refType_Values refType;

        /// <summary>
        ///  The count of items in the pDAL linked list.
        /// </summary>
        [CLSCompliant(false)]
        public ushort count;

        /// <summary>
        ///  A list of network names of the DCs to which the referral
        ///  is directed.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        [CLSCompliant(false)]
        public _DSA_ADDRESS_LIST_DRS_WIRE_V1[] pDAL;

        /// <summary>
        ///  Null, or the next item in the linked list.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public CONTREF_DRS_WIRE_V1[] pNextContRef;

        /// <summary>
        ///  True if and only if a new choice is specified.
        /// </summary>
        [CLSCompliant(false)]
        public int bNewChoice;

        /// <summary>
        ///  The choice to use in the continuation referral. This
        ///  field MUST be one of the following values:
        /// </summary>
        [CLSCompliant(false)]
        public choice_Values choice;
    }

    /// <summary>
    ///  The alias RDN values of CONTREF_DRS_WIRE_V1.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum aliasRDN_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The RDNsInternal values of CONTREF_DRS_WIRE_V1.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum RDNsInternal_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The refType values of CONTREF_DRS_WIRE_V1.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [Flags()]
    [CLSCompliant(false)]
    public enum refType_Values : ushort
    {

        /// <summary>
        ///  A referral to a superior DC.
        /// </summary>
        CH_REFTYPE_SUPERIOR = 0x0000,

        /// <summary>
        ///  A referral to a subordinate DC (for example, to a child
        ///  domain).
        /// </summary>
        CH_REFTYPE_SUBORDINATE = 0x0001,

        /// <summary>
        ///  Not in use.
        /// </summary>
        CH_REFTYPE_NSSR = 0x0002,

        /// <summary>
        ///  A referral to an external crossRef object. See [MS-ADTS]
        ///  section.
        /// </summary>
        CH_REFTYPE_CROSS = 0x0003,
    }

    /// <summary>
    ///  The choice values of CONTREF_DRS_WIRE_V1.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [Flags()]
    public enum choice_Values : byte
    {

        /// <summary>
        ///  A base search should be performed.
        /// </summary>
        SE_CHOICE_BASE_ONLY = 0x00,

        /// <summary>
        ///  A one-level search should be performed.
        /// </summary>
        SE_CHOICE_IMMED_CHLDRN = 0x01,

        /// <summary>
        ///  A subtree search should be performed.
        /// </summary>
        SE_CHOICE_WHOLE_SUBTREE = 0x02,
    }

    /// <summary>
    ///  The DS_REPL_OPW structure defines a replication operation
    ///  to be processed by a DC. This structure is a concrete
    ///  representation of a tuple in a ReplicationQueue sequence.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\9c0d0bd1-27bb-44a3-8317-969fe45e87e1.xml
    //  </remarks>
    public partial struct DS_REPL_OPW
    {

        /// <summary>
        ///  The date and time at which the operation was requested.
        /// </summary>
        [CLSCompliant(false)]
        public _FILETIME ftimeEnqueued;

        /// <summary>
        ///  The unique ID associated with the operation.
        /// </summary>
        [CLSCompliant(false)]
        public uint ulSerialNumber;

        /// <summary>
        ///  The priority of the operation.
        /// </summary>
        [CLSCompliant(false)]
        public uint ulPriority;

        /// <summary>
        ///  The type of operation.
        /// </summary>
        public DS_REPL_OP_TYPE OpType;

        /// <summary>
        ///  The DRS_OPTIONS flags.
        /// </summary>
        [CLSCompliant(false)]
        public uint ulOptions;

        /// <summary>
        ///  The NC root of the relevant NC replica.
        /// </summary>
        [String()]
        public string pszNamingContext;

        /// <summary>
        ///  The DN of the relevant DC'snTDSDSAobject.
        /// </summary>
        [String()]
        public string pszDsaDN;

        /// <summary>
        ///  The NetworkAddress of the relevant DC.
        /// </summary>
        [String()]
        public string pszDsaAddress;

        /// <summary>
        ///  The objectGUID of the NC root of the relevant NC replica.
        /// </summary>
        public Guid uuidNamingContextObjGuid;

        /// <summary>
        ///  The DSA GUID of the DC.
        /// </summary>
        public Guid uuidDsaObjGuid;
    }

    /// <summary>
    ///  The DRS_MSG_FINISH_DEMOTIONREQ union defines the request
    ///  messages sent to the IDL_DRSFinishDemotion method.
    ///  Only one version, identified by dwInVersion = 1, is
    ///  currently defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\a3f623a7-1a4b-432b-9ece-5b5eca0c5beb.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_FINISH_DEMOTIONREQ
    {

        /// <summary>
        ///  Version 1 request. Currently, only one version is defined.
        /// </summary>
        [Case("1")]
        public DRS_MSG_FINISH_DEMOTIONREQ_V1 V1;
    }

    /// <summary>
    ///  The DS_REPL_CLIENT_CONTEXT structure defines an active
    ///  RPC client connection. This structure is a concrete
    ///  representation of a tuple in an RPCClientContexts sequence.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\a540a2eb-228b-447a-ba6f-6868d7793c53.xml
    //  </remarks>
    public partial struct DS_REPL_CLIENT_CONTEXT
    {

        /// <summary>
        ///  The unique ID of the client context.
        /// </summary>
        [CLSCompliant(false)]
        public ulong hCtx;

        /// <summary>
        ///  The number of references to the context.
        /// </summary>
        [CLSCompliant(false)]
        public int lReferenceCount;

        /// <summary>
        ///  True if and only if the context has not yet been closed
        ///  by the IDL_DRSUnbind method.
        /// </summary>
        [CLSCompliant(false)]
        public int fIsBound;

        /// <summary>
        ///  Zeros, or the value pointed to by the puuidClientDsa
        ///  parameter to IDL_DRSBind.
        /// </summary>
        public Guid uuidClient;

        /// <summary>
        ///  The date and time at which this context was last used
        ///  in an RPC method call.
        /// </summary>
        public long timeLastUsed;

        /// <summary>
        ///  The IPv4 address of the client. If the client is connected
        ///  with IPv6, this field contains zero.
        /// </summary>
        [CLSCompliant(false)]
        public uint IPAddr;

        /// <summary>
        ///  0, or the process ID specified by the client in the
        ///  pextClient parameter to IDL_DRSBind.
        /// </summary>
        [CLSCompliant(false)]
        public int pid;
    }

    /// <summary>
    ///  The DRS_MSG_GETREPLINFO_REQ_V2 structure defines a version
    ///  2 request message sent to the IDL_DRSGetReplInfo method.
    ///  The V2 request structure is a superset of the V1 request
    ///  structure.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\a5f01efd-ecd9-42c6-a0c7-8cf67d028589.xml
    //  </remarks>
    public partial struct DRS_MSG_GETREPLINFO_REQ_V2
    {

        /// <summary>
        ///  MUST be a DS_REPL_INFO code.
        /// </summary>
        [CLSCompliant(false)]
        public uint InfoType;

        /// <summary>
        ///  DN of the object on which the operation should be performed.
        ///  The meaning of this parameter depends on the value
        ///  of the InfoType parameter.
        /// </summary>
        [String()]
        public string pszObjectDN;

        /// <summary>
        ///  NULL GUID or the DSA GUID of a DC.
        /// </summary>
        public Guid uuidSourceDsaObjGuid;

        /// <summary>
        ///  Zero or more of the following bit flags. 0 1 2 3 4 5
        ///  6 7 8 9 1 0 1 2 3 4 5 6 7 8 9 2 0 1 2 3 4 5 6 7 8 9
        ///  3 0 1 M T X X X X X X X X X X X X X X X X X X X X X
        ///  X X X X X X X X X XX: Unused. MUST be zero and ignored.MT
        ///  (DS_REPL_INFO_FLAG_IMPROVE_LINKED_ATTRS): Return attributestamps
        ///  for linked values.
        /// </summary>
        [CLSCompliant(false)]
        public uint ulFlags;

        /// <summary>
        ///  Null, or the lDAPDisplayName of a link attribute.
        /// </summary>
        [String()]
        public string pszAttributeName;

        /// <summary>
        ///  Null, or the DN of the link value for which to retrieve
        ///  a stamp.
        /// </summary>
        [String()]
        public string pszValueDN;

        /// <summary>
        ///  The range bound of values to be returned by the server.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwEnumerationContext;
    }

    /// <summary>
    ///  The NT4_REPLICATION_STATE structure defines the replication
    ///  state for windows_nt_4_0DCs, whose interpretation is
    ///  specified in [MS-ADTS] section.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\a7d8f924-3fc0-4dc1-89f3-59f4521cf0e7.xml
    //  </remarks>
    public partial struct NT4_REPLICATION_STATE
    {

        /// <summary>
        ///  The windows_nt_4_0replicationupdate sequence number
        ///  for the SAM database.
        /// </summary>
        [CLSCompliant(false)]
        public _LARGE_INTEGER SamSerialNumber;

        /// <summary>
        ///  The time at which the windows_nt_4_0replicationupdate
        ///  sequence number for the SAM database was set to 1.
        /// </summary>
        [CLSCompliant(false)]
        public _LARGE_INTEGER SamCreationTime;

        /// <summary>
        ///  The windows_nt_4_0replicationupdate sequence number
        ///  for the built-in database.
        /// </summary>
        [CLSCompliant(false)]
        public _LARGE_INTEGER BuiltinSerialNumber;

        /// <summary>
        ///  The time at which the windows_nt_4_0replicationupdate
        ///  sequence number for the built-in database was set to
        ///  1.
        /// </summary>
        [CLSCompliant(false)]
        public _LARGE_INTEGER BuiltinCreationTime;

        /// <summary>
        ///  The windows_nt_4_0replicationupdate sequence number
        ///  for the local security authority (LSA) database.
        /// </summary>
        [CLSCompliant(false)]
        public _LARGE_INTEGER LsaSerialNumber;

        /// <summary>
        ///  The time at which the windows_nt_4_0replicationupdate
        ///  sequence number for the LSA database was set to 1.
        /// </summary>
        [CLSCompliant(false)]
        public _LARGE_INTEGER LsaCreationTime;
    }

    /// <summary>
    ///  The DRS_MSG_REVMEMB_REPLY_V1 structure defines the response
    ///  message received from the IDL_DRSGetMemberships method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\a8a13298-8e3b-408d-bad7-6710138c54cd.xml
    //  </remarks>
    public partial struct DRS_MSG_REVMEMB_REPLY_V1
    {

        /// <summary>
        ///  0 on success. On failure, this can be one of the following.
        /// </summary>
        [CLSCompliant(false)]
        public errCode_Values errCode;

        /// <summary>
        ///  Count of items in the ppDsNames array.
        /// </summary>
        [PossibleValueRange("0", "10000")]
        [CLSCompliant(false)]
        public uint cDsNames;

        /// <summary>
        ///  Count of items in the ppSidHistory array.
        /// </summary>
        [PossibleValueRange("0", "10000")]
        [CLSCompliant(false)]
        public uint cSidHistory;

        /// <summary>
        ///  The filtered group membership. This list contains the
        ///  combined membership for all the names specified in
        ///  ppDsNames field of the input DRS_MSG_REVMEMB_REQ_V1
        ///  structure.
        /// </summary>
        [Size("cDsNames,1")]
        public DSNAME[][] ppDsNames;

        /// <summary>
        ///  Properties of the returned groups. Values are chosen
        ///  from SE_GROUP values.
        /// </summary>
        [Size("cDsNames")]
        [CLSCompliant(false)]
        public uint[] pAttributes;

        /// <summary>
        ///  SID history of the returned groups.
        /// </summary>
        [Size("cSidHistory,1")]
        public NT4SID[][] ppSidHistory;
    }

    /// <summary>
    ///  The errCode values of DRS_MSG_REVMEMB_REPLY_V1.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    //[Flags()]
    [CLSCompliant(false)]
    public enum errCode_Values : uint
    {

        /// <summary>
        ///  Insufficient system resources exist to complete the
        ///  request.
        /// </summary>
        STATUS_INSUFFICIENT_RESOURCES = 0xC000009A,

        /// <summary>
        ///  The number of groups is greater than the number that
        ///  can be returned to the caller.
        /// </summary>
        STATUS_TOO_MANY_CONTEXT_IDS = 0xC000015A,
    }

    /// <summary>
    ///  The DS_DOMAIN_CONTROLLER_INFO_2W structure defines DC
    ///  information that is returned as a part of the response
    ///  to an InfoLevel = 2 request. The struct contains information
    ///  about a single DC in the domain.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\a9c9fd50-24b5-4ff7-b336-8e23ac0622de.xml
    //  </remarks>
    public partial struct DS_DOMAIN_CONTROLLER_INFO_2W
    {

        /// <summary>
        ///  NetBIOS name of the DC.
        /// </summary>
        [String()]
        public string NetbiosName;

        /// <summary>
        ///  DNS host name of the DC.
        /// </summary>
        [String()]
        public string DnsHostName;

        /// <summary>
        ///  RDN of the siteobject.
        /// </summary>
        [String()]
        public string SiteName;

        /// <summary>
        ///  DN of the siteobject.
        /// </summary>
        [String()]
        public string SiteObjectName;

        /// <summary>
        ///  DN of the computerobject that corresponds to the DC.
        /// </summary>
        [String()]
        public string ComputerObjectName;

        /// <summary>
        ///  DN of the serverobject that corresponds to the DC.
        /// </summary>
        [String()]
        public string ServerObjectName;

        /// <summary>
        ///  DN of the nTDSDSAobject that corresponds to the DC.
        /// </summary>
        [String()]
        public string NtdsDsaObjectName;

        /// <summary>
        ///  True if and only if the DC is the PDCFSMO role owner.
        /// </summary>
        [CLSCompliant(false)]
        public int fIsPdc;

        /// <summary>
        ///  MUST be true.
        /// </summary>
        [CLSCompliant(false)]
        public int fDsEnabled;

        /// <summary>
        ///  True if and only if the DC is also a GC.
        /// </summary>
        [CLSCompliant(false)]
        public int fIsGc;

        /// <summary>
        ///  The objectGUID attribute of the siteobject.
        /// </summary>
        public Guid SiteObjectGuid;

        /// <summary>
        ///  The objectGUID attribute of the computerobject that
        ///  corresponds to the DC.
        /// </summary>
        public Guid ComputerObjectGuid;

        /// <summary>
        ///  The objectGUID attribute of the serverobject that corresponds
        ///  to the DC.
        /// </summary>
        public Guid ServerObjectGuid;

        /// <summary>
        ///  The objectGUID attribute of the nTDSDSAobject that corresponds
        ///  to the DC.
        /// </summary>
        public Guid NtdsDsaObjectGuid;
    }

    /// <summary>
    ///  DRS_SecBufferDesc is a Generic Security Service (GSS)
    ///  Kerberos authentication token, as specified in [RFC1964].
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\aa6f5b36-cf1e-49c9-b5fd-4cd5c9de7448.xml
    //  </remarks>
    public partial struct DRS_SecBufferDesc
    {

        /// <summary>
        ///  MUST be 0.
        /// </summary>
        [CLSCompliant(false)]
        public ulVersion_Values ulVersion;

        /// <summary>
        ///  The number of items in the Buffers array.
        /// </summary>
        [CLSCompliant(false)]
        public uint cBuffers;

        /// <summary>
        ///  Buffers that contain authentication data.
        /// </summary>
        [Size("cBuffers")]
        public DRS_SecBuffer[] Buffers;
    }

    /// <summary>
    ///  The ulVersion values of DRS_SecBufferDesc.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum ulVersion_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The DS_REPL_NEIGHBORW structure defines a replication
    ///  neighbor. This structure is a concrete representation
    ///  of a RepsFrom or RepsTo value.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\ab63bcfb-34cf-4d4c-b201-f43380c3d745.xml
    //  </remarks>
    public partial struct DS_REPL_NEIGHBORW
    {

        /// <summary>
        ///  The NC root of the NC replica.
        /// </summary>
        [String()]
        public string pszNamingContext;

        /// <summary>
        ///  The DN of the server DCnTDSDSAobject.
        /// </summary>
        [String()]
        public string pszSourceDsaDN;

        /// <summary>
        ///  The NetworkAddress of the server DC.
        /// </summary>
        [String()]
        public string pszSourceDsaAddress;

        /// <summary>
        ///  The DN of the interSiteTransportobject corresponding
        ///  to the transport used to communicate with the server
        ///  DC.
        /// </summary>
        [String()]
        public string pszAsyncIntersiteTransportDN;

        /// <summary>
        ///  The DRS_OPTIONS flags.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwReplicaFlags;

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public DS_REPL_NEIGHBORW_dwReserved_Values dwReserved;

        /// <summary>
        ///  The objectGUID of the NC root.
        /// </summary>
        public Guid uuidNamingContextObjGuid;

        /// <summary>
        ///  The DSA GUID of the server DC.
        /// </summary>
        public Guid uuidSourceDsaObjGuid;

        /// <summary>
        ///  The invocation ID associated with the server DC.
        /// </summary>
        public Guid uuidSourceDsaInvocationID;

        /// <summary>
        ///  The objectGUID of the interSiteTransportobject corresponding
        ///  to the transport used to communicate with the server
        ///  DC.
        /// </summary>
        public Guid uuidAsyncIntersiteTransportObjGuid;

        /// <summary>
        ///  An implementation-specific value.
        /// </summary>
        public long usnLastObjChangeSynced;

        /// <summary>
        ///  An implementation-specific value.
        /// </summary>
        public long usnAttributeFilter;

        /// <summary>
        ///  The time of the last successful replication from the
        ///  server DC.
        /// </summary>
        [CLSCompliant(false)]
        public _FILETIME ftimeLastSyncSuccess;

        /// <summary>
        ///  The time of the last attempt to replicate from the server
        ///  DC.
        /// </summary>
        [CLSCompliant(false)]
        public _FILETIME ftimeLastSyncAttempt;

        /// <summary>
        ///  0, or the Windows error code resulting from the last
        ///  sync attempt.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwLastSyncResult;

        /// <summary>
        ///  The number of consecutive failures to replicate from
        ///  the server DC.
        /// </summary>
        [CLSCompliant(false)]
        public uint cNumConsecutiveSyncFailures;
    }

    /// <summary>
    ///  The dwReserved values of DS_REPL_NEIGHBORW.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum DS_REPL_NEIGHBORW_dwReserved_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The DSA_MSG_PREPARE_SCRIPT_REQ union defines the request
    ///  messages sent to the IDL_DSAPrepareScript method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\ad644c4d-16dc-403e-9726-865677b5c221.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DSA_MSG_PREPARE_SCRIPT_REQ
    {

        /// <summary>
        ///  The version 1 request.
        /// </summary>
        [Case("1")]
        public DSA_MSG_PREPARE_SCRIPT_REQ_V1 V1;
    }

    /// <summary>
    ///  The DRS_MSG_FINISH_DEMOTIONREPLY union defines the response
    ///  messages received from the IDL_DRSFinishDemotion method.
    ///  Only one version, identified by pdwOutVersion^ = 1,
    ///  is currently defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\ae098000-268c-4894-87e8-4c6cacb4e610.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_FINISH_DEMOTIONREPLY
    {

        /// <summary>
        ///  Version 1 reply.
        /// </summary>
        [Case("1")]
        public DRS_MSG_FINISH_DEMOTIONREPLY_V1 V1;
    }

    /// <summary>
    ///  The PROPERTY_META_DATA_EXT structure defines a concrete
    ///  type for the stamp of the last originating update to
    ///  an attribute.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\aef7ebde-c305-4224-95fd-585c86b19c38.xml
    //  </remarks>
    public partial struct PROPERTY_META_DATA_EXT
    {

        /// <summary>
        ///  The version of the attribute values, starting at 1 and
        ///  increasing by one with each originating update.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwVersion;

        /// <summary>
        ///  The time at which the originating update was performed.
        /// </summary>
        public long timeChanged;

        /// <summary>
        ///  The invocationId of the DC that performed the originating
        ///  update.
        /// </summary>
        public Guid uuidDsaOriginating;

        /// <summary>
        ///  The USN of the DC assigned to the originating update.
        /// </summary>
        public long usnOriginating;
    }

    /// <summary>
    ///  The NAMERR_DRS_WIRE_V1 structure defines a name resolution
    ///  error.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\b1d8c71e-f368-4394-a356-12e6f23a5eca.xml
    //  </remarks>
    public partial struct NAMERR_DRS_WIRE_V1
    {

        /// <summary>
        ///  The implementation-specific diagnostic code.
        /// </summary>
        [CLSCompliant(false)]
        public uint dsid;

        /// <summary>
        ///  0, STATUS code, or Windows error code.
        /// </summary>
        [CLSCompliant(false)]
        public uint extendedErr;

        /// <summary>
        ///  The implementation-specific diagnostic code.
        /// </summary>
        [CLSCompliant(false)]
        public uint extendedData;

        /// <summary>
        ///  0 or PROBLEM error code (section ).
        /// </summary>
        [CLSCompliant(false)]
        public ushort problem;

        /// <summary>
        ///  The best match for the supplied object identity.
        /// </summary>
        [Indirect()]
        public DSNAME? pMatched;
    }

    /// <summary>
    ///  The ATTRVALBLOCK structure defines a concrete type for
    ///  a sequence of attribute values.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\b526370f-dfe5-4e85-9041-90d07bc16ff5.xml
    //  </remarks>
    public partial struct ATTRVALBLOCK
    {

        /// <summary>
        ///  The number of items in the pAVal array.
        /// </summary>
        [CLSCompliant(false)]
        public uint valCount;

        /// <summary>
        ///  The sequence of attribute values.
        /// </summary>
        [Size("valCount")]
        public ATTRVAL[] pAVal;
    }

    /// <summary>
    ///  The DRS_MSG_REPVERIFYOBJ_V1 structure defines a request
    ///  message sent to the IDL_DRSReplicaVerifyObjects method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\b81dc433-007c-4a0e-ae27-a0469cc25c58.xml
    //  </remarks>
    public partial struct DRS_MSG_REPVERIFYOBJ_V1
    {

        /// <summary>
        ///  The NC to verify.
        /// </summary>
        [Indirect()]
        public DSNAME? pNC;

        /// <summary>
        ///  The objectGUID of the nTDSDSAobject for the reference
        ///  DC.
        /// </summary>
        public Guid uuidDsaSrc;

        /// <summary>
        ///  0 to expunge each object that is not verified, or 1
        ///  to log an event that identifies each such object.
        /// </summary>
        [CLSCompliant(false)]
        public uint ulOptions;
    }

    /// <summary>
    ///  The DRS_MSG_REPVERIFYOBJ union defines the request messages
    ///  sent to the IDL_DRSReplicaVerifyObjects method. Only
    ///  one version, identified by dwVersion = 1, is currently
    ///  defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\b8cc0c8a-7119-4406-8f3a-4172dc512735.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_REPVERIFYOBJ
    {

        /// <summary>
        ///  The version 1 request.
        /// </summary>
        [Case("1")]
        public DRS_MSG_REPVERIFYOBJ_V1 V1;
    }

    /// <summary>
    ///  The DRS_MSG_REVMEMB_REQ_V1 structure defines the request
    ///  message sent to the IDL_DRSGetMemberships method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\bc96a03b-579e-4454-8db4-12067b6ca985.xml
    //  </remarks>
    public partial struct DRS_MSG_REVMEMB_REQ_V1
    {

        /// <summary>
        ///  The count of items in the ppDsNames array.
        /// </summary>
        [PossibleValueRange("1", "10000")]
        [CLSCompliant(false)]
        public uint cDsNames;

        /// <summary>
        ///  The DSName of the object whose reverse membership is
        ///  being requested, plus the DSNames of groups of the
        ///  appropriate type(s) of which it is already known to
        ///  be a member.
        /// </summary>
        [Size("cDsNames,1")]
        public DSNAME[][] ppDsNames;

        /// <summary>
        ///  Zero or more of the following bit flags.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwFlags;

        /// <summary>
        ///  The type of group membership evaluation to be performed.
        /// </summary>
        [PossibleValueRange("1", "7")]
        public REVERSE_MEMBERSHIP_OPERATION_TYPE OperationType;

        /// <summary>
        ///  Domain filter; resulting objects that are not from this
        ///  domain are neither returned nor followed transitively.
        /// </summary>
        [Indirect()]
        public DSNAME? pLimitingDomain;
    }

    /// <summary>
    ///  The DRS_MSG_RMSVRREQ union defines the request messages
    ///  sent to the IDL_DRSRemoveDsServer method. Only one
    ///  version, identified by dwInVersion = 1, is currently
    ///  defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\bd5413ad-a2f5-4976-b4aa-e4082183824c.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_RMSVRREQ
    {

        /// <summary>
        ///  The version 1 request.
        /// </summary>
        [Case("1")]
        public DRS_MSG_RMSVRREQ_V1 V1;
    }

    /// <summary>
    ///  The INTFORMPROB_DRS_WIRE_V1 structure defines an attribute
    ///  error.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\bdfbc428-fa77-4767-88bd-ca75750b03bf.xml
    //  </remarks>
    public partial struct INTFORMPROB_DRS_WIRE_V1
    {

        /// <summary>
        ///  The implementation-specific diagnostic code.
        /// </summary>
        [CLSCompliant(false)]
        public uint dsid;

        /// <summary>
        ///  0, STATUS code, or Windows error code.
        /// </summary>
        [CLSCompliant(false)]
        public uint extendedErr;

        /// <summary>
        ///  The implementation-specific diagnostic code.
        /// </summary>
        [CLSCompliant(false)]
        public uint extendedData;

        /// <summary>
        ///  0 or PROBLEM error code.
        /// </summary>
        [CLSCompliant(false)]
        public ushort problem;

        /// <summary>
        ///  The attribute that was being processed when the error
        ///  occurred.
        /// </summary>
        [CLSCompliant(false)]
        public uint type;

        /// <summary>
        ///  If true, the offending value is returned in the Val
        ///  member.
        /// </summary>
        [CLSCompliant(false)]
        public int valReturned;

        /// <summary>
        ///  The offending value.
        /// </summary>
        public ATTRVAL Val;
    }

    /// <summary>
    ///  The DS_REPL_NEIGHBORSW structure defines a set of replication
    ///  neighbors. This structure is a concrete representation
    ///  of a sequence of RepsFrom or RepsTo values.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\c0f48bab-491a-4d65-85b0-4045fc721bb4.xml
    //  </remarks>
    public partial struct DS_REPL_NEIGHBORSW
    {

        /// <summary>
        ///  The count of items in the rgNeighbor array.
        /// </summary>
        [CLSCompliant(false)]
        public uint cNumNeighbors;

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public DS_REPL_NEIGHBORSW_dwReserved_Values dwReserved;

        /// <summary>
        ///  A set of replication neighbors.
        /// </summary>
        [Inline()]
        [Size("cNumNeighbors")]
        public DS_REPL_NEIGHBORW[] rgNeighbor;
    }

    /// <summary>
    /// The reserved of DS_REPL_NEIGHBORW.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum DS_REPL_NEIGHBORSW_dwReserved_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The ADDENTRY_REPLY_INFO structure defines the identity
    ///  of an object added by the IDL_DRSAddEntry method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\c4dc8d64-90c0-4b96-aa23-01addd96728d.xml
    //  </remarks>
    public partial struct ADDENTRY_REPLY_INFO
    {

        /// <summary>
        ///  The objectGUID of the added object.
        /// </summary>
        public Guid objGuid;

        /// <summary>
        ///  The objectSid of the added object.
        /// </summary>
        public NT4SID objSid;
    }

    /// <summary>
    ///  The DRS_MSG_ADDENTRYREPLY_V1 structure defines the response
    ///  message received from the IDL_DRSAddEntry method. This
    ///  response version is obsolete.Though this response version
    ///  appears in the IDL, windowsDCs do not support it.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\c934ca07-465d-40e9-a89f-68f183667f1a.xml
    //  </remarks>
    public partial struct DRS_MSG_ADDENTRYREPLY_V1
    {

        /// <summary>
        ///  The objectGUID of the added object.
        /// </summary>
        public Guid Guid;

        /// <summary>
        ///  The objectSid of the added object.
        /// </summary>
        public NT4SID Sid;

        /// <summary>
        ///  0 if successful or a DIRERR error code (section) if
        ///  a fatal error occurred.
        /// </summary>
        [CLSCompliant(false)]
        public uint errCode;

        /// <summary>
        ///  The implementation-specific diagnostic code.
        /// </summary>
        [CLSCompliant(false)]
        public uint dsid;

        /// <summary>
        ///  0, STATUS code, or Windows error code.
        /// </summary>
        [CLSCompliant(false)]
        public uint extendedErr;

        /// <summary>
        ///  The implementation-specific diagnostic code.
        /// </summary>
        [CLSCompliant(false)]
        public uint extendedData;

        /// <summary>
        ///  0 or PROBLEM error code (section ).
        /// </summary>
        [CLSCompliant(false)]
        public ushort problem;
    }

    /// <summary>
    ///  The DRS_MSG_DCINFOREPLY_V3 structure defines the response
    ///  message received from the IDL_DRSDomainControllerInfo
    ///  method when the client has requested InfoLevel = 3.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\cafc7232-c6da-4784-84d7-e5d8c804c2d9.xml
    //  </remarks>
    public partial struct DRS_MSG_DCINFOREPLY_V3
    {

        /// <summary>
        ///  Count of items in the rItems array.
        /// </summary>
        [PossibleValueRange("0", "10000")]
        [CLSCompliant(false)]
        public uint cItems;

        /// <summary>
        ///  DC information.
        /// </summary>
        [Size("cItems")]
        public DS_DOMAIN_CONTROLLER_INFO_3W[] rItems;
    }

    /// <summary>
    ///  The DS_REPL_PENDING_OPSW structure defines a sequence
    ///  of replication operations to be processed by a DC.
    ///  This structure is a concrete representation of ReplicationQueue.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\cce01b0a-e66f-4ed2-992d-39a69e480926.xml
    //  </remarks>
    public partial struct DS_REPL_PENDING_OPSW
    {

        /// <summary>
        ///  The time when the current operation started.
        /// </summary>
        [CLSCompliant(false)]
        public _FILETIME ftimeCurrentOpStarted;

        /// <summary>
        ///  The number of items in the rgPendingOp array.
        /// </summary>
        [CLSCompliant(false)]
        public uint cNumPendingOps;

        /// <summary>
        ///  The sequence of replication operations to be performed.
        /// </summary>
        [Inline()]
        [Size("cNumPendingOps")]
        public DS_REPL_OPW[] rgPendingOp;
    }

    /// <summary>
    ///  The UPTODATE_CURSOR_V1 structure is a concrete type
    ///  for the replication state relative to a given DC.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\cf88f341-fb49-4cd5-b7e2-6920cbd91f1b.xml
    //  </remarks>
    public partial struct UPTODATE_CURSOR_V1
    {

        /// <summary>
        ///  The invocationId of the DC performing the update.
        /// </summary>
        public Guid uuidDsa;

        /// <summary>
        ///  The USN of the update on the updating DC.
        /// </summary>
        public long usnHighPropUpdate;
    }

    /// <summary>
    ///  The DS_REPL_CURSOR structure defines a replication cursor
    ///  for a given NC replica. This structure is a concrete
    ///  representation of a ReplUpToDateVector value.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\cf960f2f-c8fa-4dfa-9252-f70164c14039.xml
    //  </remarks>
    public partial struct DS_REPL_CURSOR
    {

        /// <summary>
        ///  The invocation ID of a DC.
        /// </summary>
        public Guid uuidSourceDsaInvocationID;

        /// <summary>
        ///  The update sequence number (USN) at which an update
        ///  was applied on the DC.
        /// </summary>
        public long usnAttributeFilter;
    }

    /// <summary>
    ///  The DRS_MSG_NT4_CHGLOG_REQ union defines the request
    ///  messages sent to the IDL_DRSGetNT4ChangeLog method.
    ///  Only one version, identified by dwInVersion = 1, is
    ///  currently defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\cfab5cb7-2dfc-4dcc-8f9d-472f75c3cc27.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_NT4_CHGLOG_REQ
    {

        /// <summary>
        ///  The version 1 request.
        /// </summary>
        [Case("1")]
        public DRS_MSG_NT4_CHGLOG_REQ_V1 V1;
    }

    /// <summary>
    ///  The DRS_MSG_RMDMNREQ union defines the request messages
    ///  sent to the IDL_DRSRemoveDsDomain method. Only one
    ///  version, identified by dwInVersion = 1, is currently
    ///  defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\d17fb26a-c320-42e1-8576-55be8a68b915.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_RMDMNREQ
    {

        /// <summary>
        ///  The version 1 request.
        /// </summary>
        [Case("1")]
        public DRS_MSG_RMDMNREQ_V1 V1;
    }

    /// <summary>
    ///  The PrefixTableEntry structure defines a concrete type
    ///  for mapping a range of ATTRTYP values to and from OIDs.
    ///  It is a component of the type SCHEMA_PREFIX_TABLE.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\d26d36cd-10c4-4b27-a84e-98336abf357a.xml
    //  </remarks>
    public partial struct PrefixTableEntry
    {

        /// <summary>
        ///  The index assigned to the prefix.
        /// </summary>
        [CLSCompliant(false)]
        public uint ndx;

        /// <summary>
        ///  An OID or a prefix of an OID.
        /// </summary>
        public OID_t prefix;
    }

    /// <summary>
    ///  The DRS_MSG_REPSYNC_V1 structure defines a request message
    ///  sent to the IDL_DRSReplicaSync method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\d29519a5-f85e-4bd5-907a-0777ce0be29f.xml
    //  </remarks>
    public partial struct DRS_MSG_REPSYNC_V1
    {

        /// <summary>
        ///  A pointer to DSName of the root of an NC replica on
        ///  the server.
        /// </summary>
        [Indirect()]
        public DSNAME? pNC;

        /// <summary>
        ///  The DSA GUID.
        /// </summary>
        public Guid uuidDsaSrc;

        /// <summary>
        ///  The transport-specific NetworkAddress of a DC.
        /// </summary>
        [String(StringEncoding.ASCII)]
        public string pszDsaSrc;

        /// <summary>
        ///  The DRS_OPTIONS flags.
        /// </summary>
        [CLSCompliant(false)]
        public uint ulOptions;
    }

    /// <summary>
    ///  The DRS_MSG_REPSYNC_V2 structure defines a request message
    ///  sent to the IDL_DRSReplicaSync method.
    /// </summary>
    public partial struct DRS_MSG_REPSYNC_V2
    {

        /// <summary>
        ///  A pointer to DSName of the root of an NC replica on
        ///  the server.
        /// </summary>
        [Indirect()]
        public DSNAME? pNC;

        /// <summary>
        ///  The DSA GUID.
        /// </summary>
        public Guid uuidDsaSrc;

        /// <summary>
        ///  The transport-specific NetworkAddress of a DC.
        /// </summary>
        [String(StringEncoding.ASCII)]
        public string pszDsaSrc;

        /// <summary>
        ///  The DRS_OPTIONS flags.
        /// </summary>
        [CLSCompliant(false)]
        public uint ulOptions;

        /// <summary>
        /// An identifier for the operation that the DC can use for implementation-defined troubleshooting.
        /// There are no normative constraints on this value,
        /// nor does the value figure in any normative processing rules.
        /// </summary>
        public Guid correlationID;

        /// <summary>
        /// A pointer to a VAR_SIZE_BUFFER_WITH_VERSION structure (section 5.219).
        /// MUST be a null pointer.
        /// </summary>
        public VAR_SIZE_BUFFER_WITH_VERSION pReservedBuffer;
    }

    /// <summary>
    ///  The UPTODATE_CURSOR_V2 structure defines a concrete
    ///  type for the replication state relative to a given
    ///  DC.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\d3e30021-b6ac-413e-b08a-b69b9b0c6592.xml
    //  </remarks>
    public partial struct UPTODATE_CURSOR_V2
    {

        /// <summary>
        ///  The invocationId of the DC performing the update.
        /// </summary>
        public Guid uuidDsa;

        /// <summary>
        ///  The USN of the update on the updating DC.
        /// </summary>
        public long usnHighPropUpdate;

        /// <summary>
        ///  The time at which the last successful replication occurred
        ///  from the DC identified by uuidDsa; for replication
        ///  latency reporting only.
        /// </summary>
        public long timeLastSyncSuccess;
    }

    /// <summary>
    ///  The DRS_MSG_REPADD union defines request messages that
    ///  are sent to the IDL_DRSReplicaAdd method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\d73cb42a-312c-49ae-82d1-db93c044011e.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_REPADD
    {

        /// <summary>
        ///  The version 1 request.
        /// </summary>
        [Case("1")]
        public DRS_MSG_REPADD_V1 V1;

        /// <summary>
        ///  The version 2 request (a superset of V1).
        /// </summary>
        [Case("2")]
        public DRS_MSG_REPADD_V2 V2;

        /// <summary>
        ///  The version 3 request (a superset of V2).
        /// </summary>
        [Case("3")]
        public DRS_MSG_REPADD_V3 V3;
    }

    /// <summary>
    ///  The DRS_MSG_INIT_DEMOTIONREQ union defines request messages
    ///  sent to the IDL_DRSInitDemotion method. Only one version,
    ///  identified by dwInVersion = 1, is currently defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\d7c5c65c-cfd5-4647-8314-77f32058e03f.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_INIT_DEMOTIONREQ
    {

        /// <summary>
        ///  Version 1 request. Currently, only one version is defined.
        /// </summary>
        [Case("1")]
        public DRS_MSG_INIT_DEMOTIONREQ_V1 V1;
    }

    /// <summary>
    ///  The DRS_MSG_ADDSIDREQ union defines the request messages
    ///  that are sent to the IDL_DRSAddSidHistory method. Only
    ///  one version, identified by dwInVersion = 1, is currently
    ///  defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\dc0b3735-881d-4ead-91e7-4e315821f0c4.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_ADDSIDREQ
    {

        /// <summary>
        ///  Version 1 request.
        /// </summary>
        [Case("1")]
        public DRS_MSG_ADDSIDREQ_V1 V1;
    }

    /// <summary>
    ///  The DS_REPL_VALUE_META_DATA_2 structure defines a link
    ///  value stamp. This structure is a concrete representation
    ///  of LinkValueStamp; it is a superset of DS_REPL_VALUE_META_DATA.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\ddf21c4e-616b-4679-8822-e1879ee05f6e.xml
    //  </remarks>
    public partial struct DS_REPL_VALUE_META_DATA_2
    {

        /// <summary>
        ///  The lDAPDisplayName of the attribute.
        /// </summary>
        [String()]
        public string pszAttributeName;

        /// <summary>
        ///  The DN of the object.
        /// </summary>
        [String()]
        public string pszObjectDn;

        /// <summary>
        ///  The size, in bytes, of the pbData array.
        /// </summary>
        [CLSCompliant(false)]
        public uint cbData;

        /// <summary>
        ///  Null, or data associated with the attribute value if
        ///  the attribute is of syntax Object(DN-Binary) or Object(DN-String).
        /// </summary>
        [Size("cbData")]
        public byte[] pbData;

        /// <summary>
        ///  The date and time at which the last replicated update
        ///  was made that deleted the value, or 0 if the value
        ///  is not currently deleted.
        /// </summary>
        [CLSCompliant(false)]
        public _FILETIME ftimeDeleted;

        /// <summary>
        ///  The date and time at which the first originating update
        ///  was made.
        /// </summary>
        [CLSCompliant(false)]
        public _FILETIME ftimeCreated;

        /// <summary>
        ///  The stamp version.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwVersion;

        /// <summary>
        ///  The date and time at which the last originating update
        ///  was made.
        /// </summary>
        [CLSCompliant(false)]
        public _FILETIME ftimeLastOriginatingChange;

        /// <summary>
        ///  The invocation ID of the DC that performed the last
        ///  originating update.
        /// </summary>
        public Guid uuidLastOriginatingDsaInvocationID;

        /// <summary>
        ///  The USN assigned to the last originating update by the
        ///  DC that performed the update.
        /// </summary>
        public long usnOriginatingChange;

        /// <summary>
        ///  An implementation-specific value.
        /// </summary>
        public long usnLocalChange;

        /// <summary>
        ///  The DN of the nTDSDSAobject with an invocationId of
        ///  uuidLastOriginatingDsaInvocationID.
        /// </summary>
        [String()]
        public string pszLastOriginatingDsaDN;
    }

    /// <summary>
    ///  The DS_REPL_CURSORS_2 structure defines a set of replication
    ///  cursors for a given NC replica. This structure is a
    ///  concrete representation of a sequence of ReplUpToDateVector
    ///  values; it is a superset of DS_REPL_CURSORS.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\e246043a-f208-4058-a00f-a8b19e479dc3.xml
    //  </remarks>
    public partial struct DS_REPL_CURSORS_2
    {

        /// <summary>
        ///  The count of items in the rgCursor array.
        /// </summary>
        [CLSCompliant(false)]
        public uint cNumCursors;

        /// <summary>
        ///  0xFFFFFFFF, or the range bound of the results.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwEnumerationContext;

        /// <summary>
        ///  A set of replication cursors.
        /// </summary>
        [Inline()]
        [Size("cNumCursors")]
        public DS_REPL_CURSOR_2[] rgCursor;
    }

    /// <summary>
    ///  The DS_REPL_KCC_DSA_FAILURESW structure defines a set
    ///  of DCs that are in an error state with respect to replication.
    ///  This structure is a concrete representation of KCCFailedConnections
    ///  and KCCFailedLinks.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\ecdca8ce-c07f-4bd5-8397-70b610bed8fc.xml
    //  </remarks>
    public partial struct DS_REPL_KCC_DSA_FAILURESW
    {

        /// <summary>
        ///  The count of items in the rgDsaFailure array.
        /// </summary>
        [CLSCompliant(false)]
        public uint cNumEntries;

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public DS_REPL_KCC_DSA_FAILURESW_dwReserved_Values dwReserved;

        /// <summary>
        ///  An array of DS_REPL_KCC_DSA_FAILUREW structures.
        /// </summary>
        [Inline()]
        [Size("cNumEntries")]
        public DS_REPL_KCC_DSA_FAILUREW[] rgDsaFailure;
    }

    /// <summary>
    /// The reserved of DS_REPL_KCC_DSA_FAILURESW.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum DS_REPL_KCC_DSA_FAILURESW_dwReserved_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The DRS_MSG_UPDREFS_V1 structure defines a request message
    ///  sent to the IDL_DRSUpdateRefs method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\ee70be33-08dc-48b2-a599-43a50943c0e1.xml
    //  </remarks>
    public partial struct DRS_MSG_UPDREFS_V1
    {

        /// <summary>
        ///  A pointer to the DSName of the root of an NC replica
        ///  on the server.
        /// </summary>
        [Indirect()]
        public DSNAME? pNC;

        /// <summary>
        ///  The transport-specific NetworkAddress of a DC.
        /// </summary>
        [String(StringEncoding.ASCII)]
        public string pszDsaDest;

        /// <summary>
        ///  The DSA GUID.
        /// </summary>
        public Guid uuidDsaObjDest;

        /// <summary>
        ///  The DRS_OPTIONS that control the update.
        /// </summary>
        [CLSCompliant(false)]
        public uint ulOptions;
    }

    /// <summary>
    ///  The DRS_MSG_UPDREFS_V2 structure defines a request message
    ///  sent to the IDL_DRSUpdateRefs method.
    /// </summary>
    public partial struct DRS_MSG_UPDREFS_V2
    {

        /// <summary>
        ///  A pointer to the DSName of the root of an NC replica
        ///  on the server.
        /// </summary>
        [Indirect()]
        public DSNAME? pNC;

        /// <summary>
        ///  The transport-specific NetworkAddress of a DC.
        /// </summary>
        [String(StringEncoding.ASCII)]
        public string pszDsaDest;

        /// <summary>
        ///  The DSA GUID.
        /// </summary>
        public Guid uuidDsaObjDest;

        /// <summary>
        ///  The DRS_OPTIONS that control the update.
        /// </summary>
        [CLSCompliant(false)]
        public uint ulOptions;

        /// <summary>
        /// An identifier for the operation that the DC can use for implementation-defined troubleshooting.
        /// There are no normative constraints on this value,
        /// nor does the value figure in any normative processing rules.
        /// </summary>
        public Guid correlationID;

        /// <summary>
        /// A pointer to a VAR_SIZE_BUFFER_WITH_VERSION structure (section 5.219).
        /// MUST be a null pointer.
        /// </summary>
        public VAR_SIZE_BUFFER_WITH_VERSION pReservedBuffer;
    }

    /// <summary>
    ///  The DRS_MSG_CRACKREQ union defines the request messages
    ///  sent to the IDL_DRSCrackNames method. Only one version,
    ///  identified by dwInVersion = 1, is currently defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\f2d5166e-09f6-4788-a391-66471b2f7d6d.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_CRACKREQ
    {

        /// <summary>
        ///  Version 1 request.
        /// </summary>
        [Case("1")]
        public DRS_MSG_CRACKREQ_V1 V1;
    }

    /// <summary>
    ///  The DRS_MSG_DCINFOREPLY_V2 structure defines the response
    ///  message received from the IDL_DRSDomainControllerInfo
    ///  method, when the client has requested InfoLevel = 2.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\f567e605-01fe-4228-960e-14647c29f668.xml
    //  </remarks>
    public partial struct DRS_MSG_DCINFOREPLY_V2
    {

        /// <summary>
        ///  Count of items in the rItems array.
        /// </summary>
        [PossibleValueRange("0", "10000")]
        [CLSCompliant(false)]
        public uint cItems;

        /// <summary>
        ///  DC information.
        /// </summary>
        [Size("cItems")]
        public DS_DOMAIN_CONTROLLER_INFO_2W[] rItems;
    }

    /// <summary>
    ///  The DRS_MSG_DCINFOREPLY_V1 structure defines the response
    ///  message received from the IDL_DRSDomainControllerInfo
    ///  method, when the client has requested InfoLevel = 1.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\f71a8f6c-5426-4628-aa91-aeabef2c086f.xml
    //  </remarks>
    public partial struct DRS_MSG_DCINFOREPLY_V1
    {

        /// <summary>
        ///  Count of items in the rItems array.
        /// </summary>
        [PossibleValueRange("0", "10000")]
        [CLSCompliant(false)]
        public uint cItems;

        /// <summary>
        ///  DC information.
        /// </summary>
        [Size("cItems")]
        public DS_DOMAIN_CONTROLLER_INFO_1W[] rItems;
    }

    /// <summary>
    ///  The DRS_MSG_EXISTREPLY union defines the response message
    ///  versions received from the IDL_DRSGetObjectExistence
    ///  method. Only one version, identified by pdwOutVersion^
    ///  = 1, is currently defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\f762aebb-97f9-4583-8d50-aa7dee70dc98.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_EXISTREPLY
    {

        /// <summary>
        ///  The version 1 response.
        /// </summary>
        [Case("1")]
        public DRS_MSG_EXISTREPLY_V1 V1;
    }

    /// <summary>
    ///  The DS_REPL_ATTR_META_DATA structure defines an attributestamp
    ///  for a given object. This structure is a concrete representation
    ///  of an AttributeStamp.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\f7a10e53-9c45-4719-a641-4db15e385297.xml
    //  </remarks>
    public partial struct DS_REPL_ATTR_META_DATA
    {

        /// <summary>
        ///  The lDAPDisplayName of the attribute to which the stamp
        ///  corresponds.
        /// </summary>
        [String()]
        public string pszAttributeName;

        /// <summary>
        ///  The stamp version.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwVersion;

        /// <summary>
        ///  The date and time at which the last originating update
        ///  was made.
        /// </summary>
        [CLSCompliant(false)]
        public _FILETIME ftimeLastOriginatingChange;

        /// <summary>
        ///  The invocation ID of the DC that performed the last
        ///  originating update.
        /// </summary>
        public Guid uuidLastOriginatingDsaInvocationID;

        /// <summary>
        ///  The USN assigned to the last originating update by the
        ///  DC that performed it.
        /// </summary>
        public long usnOriginatingChange;

        /// <summary>
        ///  An implementation-specific value.
        /// </summary>
        public long usnLocalChange;
    }

    /// <summary>
    ///  The DRS_MSG_REPDEL_V1 structure defines a request message
    ///  sent to the IDL_DRSReplicaDel method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\fe3b5d94-ff38-4481-8219-3d1e38d9fc40.xml
    //  </remarks>
    public partial struct DRS_MSG_REPDEL_V1
    {

        /// <summary>
        ///  A pointer to DSName of the root of an NC replica on
        ///  the server.
        /// </summary>
        [Indirect()]
        public DSNAME? pNC;

        /// <summary>
        ///  The transport-specific NetworkAddress of a DC.
        /// </summary>
        [String(StringEncoding.ASCII)]
        public string pszDsaSrc;

        /// <summary>
        ///  The DRS_OPTIONS flags.
        /// </summary>
        [CLSCompliant(false)]
        public uint ulOptions;
    }

    /// <summary>
    ///  The DRS_MSG_DCINFOREPLY union defines the response messages
    ///  received from the IDL_DRSDomainControllerInfo method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\034282e5-7828-4353-ad6e-2688c65ab9fb.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_DCINFOREPLY
    {

        /// <summary>
        ///  Version 1 response.
        /// </summary>
        [Case("1")]
        public DRS_MSG_DCINFOREPLY_V1 V1;

        /// <summary>
        ///  Version 2 response.
        /// </summary>
        [Case("2")]
        public DRS_MSG_DCINFOREPLY_V2 V2;

        /// <summary>
        ///  Version 3 response.
        /// </summary>
        [Case("3")]
        public DRS_MSG_DCINFOREPLY_V3 V3;

        /// <summary>
        ///  Version 0xFFFFFFFF response.
        /// </summary>
        [Case("-1")]
        public DRS_MSG_DCINFOREPLY_VFFFFFFFF VFFFFFFFF;
    }

    /// <summary>
    ///  The DRS_MSG_NT4_CHGLOG_REPLY_V1 structure defines the
    ///  response message received from the IDL_DRSGetNT4ChangeLog
    ///  method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\1bc2090e-d3ec-4435-88b9-02437207ea22.xml
    //  </remarks>
    public partial struct DRS_MSG_NT4_CHGLOG_REPLY_V1
    {

        /// <summary>
        ///  Zero if pRestart = null. Otherwise, the size, in bytes,
        ///  of pRestart^.
        /// </summary>
        [PossibleValueRange("0", "10485760")]
        [CLSCompliant(false)]
        public uint cbRestart;

        /// <summary>
        ///  Zero if pRestart = null. Otherwise, the size, in bytes,
        ///  of pLog^.
        /// </summary>
        [CLSCompliant(false)]
        public uint cbLog;

        /// <summary>
        ///  The replication state for windows_nt_4_0DCs.
        /// </summary>
        public NT4_REPLICATION_STATE ReplicationState;

        /// <summary>
        ///  A STATUS code. See the pseudo-code for interpretation.
        /// </summary>
        [CLSCompliant(false)]
        public uint ActualNtStatus;

        /// <summary>
        ///  Null if no entries were returned. Otherwise, a pointer
        ///  to an opaque value identifying the last entry returned
        ///  in pLog.
        /// </summary>
        [Size("cbRestart")]
        public byte[] pRestart;

        /// <summary>
        ///  The buffer containing the next entries from the change
        ///  log.
        /// </summary>
        [Size("cbLog")]
        public byte[] pLog;
    }

    /// <summary>
    ///  The DRS_MSG_CRACKREPLY union defines the response messages
    ///  received from the IDL_DRSCrackNames method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\1dc605fe-dd85-481d-84a4-f4c5da812d57.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_CRACKREPLY
    {

        /// <summary>
        ///  Version 1 reply.
        /// </summary>
        [Case("1")]
        public DRS_MSG_CRACKREPLY_V1 V1;
    }

    /// <summary>
    ///  The PROPERTY_META_DATA_EXT_VECTOR structure defines
    ///  a concrete type for a sequence of attributestamps.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\22bccd51-1e7d-4502-aef8-b84da983f94f.xml
    //  </remarks>
    public partial struct PROPERTY_META_DATA_EXT_VECTOR
    {

        /// <summary>
        ///  The number of items in the rgMetaData array.
        /// </summary>
        [CLSCompliant(false)]
        public uint cNumProps;

        /// <summary>
        ///  An array of attributestamps.
        /// </summary>
        [Inline()]
        [Size("cNumProps")]
        public PROPERTY_META_DATA_EXT[] rgMetaData;
    }

    /// <summary>
    ///  The DRS_MSG_UPDREFS union defines the request message
    ///  versions sent to the IDL_DRSUpdateRefs method. Only
    ///  one version, identified by dwVersion = 1, is currently
    ///  defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\2ad0252a-d028-412b-89c9-2fcb7123817e.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_UPDREFS
    {

        /// <summary>
        ///  The version 1 request.
        /// </summary>
        [Case("1")]
        public DRS_MSG_UPDREFS_V1 V1;

        /// <summary>
        ///  The version 2 request.
        /// </summary>
        [Case("2")]
        public DRS_MSG_UPDREFS_V2 V2;
    }

    /// <summary>
    ///  The DRS_MSG_REPLICA_DEMOTIONREQ union defines the request
    ///  messages sent to the IDL_DRSReplicaDemotion method.
    ///  Only one version, identified by dwInVersion = 1, is
    ///  currently defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\31bd794b-3e88-410b-b6e6-56e07cb34eed.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_REPLICA_DEMOTIONREQ
    {

        /// <summary>
        ///  The version 1 request. Only one version is defined.
        /// </summary>
        [Case("1")]
        public DRS_MSG_REPLICA_DEMOTIONREQ_V1 V1;
    }

    /// <summary>
    ///  The DRS_MSG_REPMOD_V1 structure defines a request message
    ///  for the IDL_DRSReplicaModify method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\34626189-925f-4786-9308-3d65ae3ce2ac.xml
    //  </remarks>
    public partial struct DRS_MSG_REPMOD_V1
    {

        /// <summary>
        ///  A pointer to the DSName of the root of an NC replica
        ///  on the server.
        /// </summary>
        [Indirect()]
        public DSNAME? pNC;

        /// <summary>
        ///  The DSA GUID.
        /// </summary>
        public Guid uuidSourceDRA;

        /// <summary>
        ///  The transport-specific NetworkAddress of a DC.
        /// </summary>
        [String(StringEncoding.ASCII)]
        public string pszSourceDRA;

        /// <summary>
        ///  The periodic replication schedule.
        /// </summary>
        public REPLTIMES rtSchedule;

        /// <summary>
        ///  The DRS_OPTIONS flags for the repsFrom value.
        /// </summary>
        [CLSCompliant(false)]
        public uint ulReplicaFlags;

        /// <summary>
        ///  The fields to update. 0 1 2 3 4 5 6 7 8 9 1 0 1 2 3
        ///  4 5 6 7 8 9 2 0 1 2 3 4 5 6 7 8 9 3 0 1 U F U A U S
        ///  X X X X X X X X X X X X X X X X X X X X X X X X X X
        ///  X X XX: Unused. MUST be zero and ignored.UF (DRS_UPDATE_FLAGS):
        ///  Updates the flags associated with the server.UA (DRS_UPDATE_ADDRESS):
        ///  Updates the transport-specific address associated with
        ///  the server.US (DRS_UPDATE_SCHEDULE): Updates the replication
        ///  schedule associated with the server.
        /// </summary>
        [CLSCompliant(false)]
        public uint ulModifyFields;

        /// <summary>
        ///  The DRS_OPTIONS flags for execution of this method.
        /// </summary>
        [CLSCompliant(false)]
        public uint ulOptions;
    }

    /// <summary>
    ///  The DRS_MSG_REPDEL union defines the request messages
    ///  sent to the IDL_DRSReplicaDel method. Only one version,
    ///  identified by dwVersion = 1, is currently defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\36c23abb-f2d1-4266-a1f4-6f167e72d3af.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_REPDEL
    {

        /// <summary>
        ///  The version 1 request.
        /// </summary>
        [Case("1")]
        public DRS_MSG_REPDEL_V1 V1;
    }

    /// <summary>
    ///  The DS_REPL_OBJ_META_DATA_2 structure defines a set
    ///  of attributestamps for a given object. This structure
    ///  is a concrete representation of the sequence of AttributeStamp
    ///  values for all attributes of a given object; it is
    ///  a superset of DS_REPL_OBJ_META_DATA.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\37c78898-e01f-4e27-9e18-6f357bba744f.xml
    //  </remarks>
    public partial struct DS_REPL_OBJ_META_DATA_2
    {

        /// <summary>
        ///  The count of items in the rgMetaData array.
        /// </summary>
        [CLSCompliant(false)]
        public uint cNumEntries;

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public DS_REPL_OBJ_META_DATA_2_dwReserved_Values dwReserved;

        /// <summary>
        ///  A set of attributestamps.
        /// </summary>
        [Inline()]
        [Size("cNumEntries")]
        public DS_REPL_ATTR_META_DATA_2[] rgMetaData;
    }

    /// <summary>
    /// The reserved of DS_REPL_OBJ_META_DATA_2.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum DS_REPL_OBJ_META_DATA_2_dwReserved_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The UPTODATE_VECTOR_V1_EXT structure defines a concrete
    ///  type for the replication state relative to a set of
    ///  DCs.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\462b424a-b50a-4c4a-a81f-48d0f4cf40fe.xml
    //  </remarks>
    public partial struct UPTODATE_VECTOR_V1_EXT
    {

        /// <summary>
        ///  The version of this structure; MUST be 1.
        /// </summary>
        [CLSCompliant(false)]
        public UPTODATE_VECTOR_V1_EXT_dwVersion_Values dwVersion;

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public UPTODATE_VECTOR_V1_EXT_dwReserved1_Values dwReserved1;

        /// <summary>
        ///  The number of items in the rgCursors array.
        /// </summary>
        [PossibleValueRange("0", "1048576")]
        [CLSCompliant(false)]
        public uint cNumCursors;

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public dwReserved2_Values dwReserved2;

        /// <summary>
        ///  An array of UPTODATE_CURSOR_V1. The items in this field
        ///  MUST be sorted in increasing order of the uuidDsa field.
        /// </summary>
        [Inline()]
        [Size("cNumCursors")]
        public UPTODATE_CURSOR_V1[] rgCursors;
    }

    /// <summary>
    /// The reserved of UPTODATE_VECTOR_V1_EXT.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum UPTODATE_VECTOR_V1_EXT_dwVersion_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 1,
    }

    /// <summary>
    /// The reserved1 of UPTODATE_VECTOR_V1_EXT.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum UPTODATE_VECTOR_V1_EXT_dwReserved1_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// The reserved2 of UPTODATE_VECTOR_V2_EXT.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum dwReserved2_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The DRS_MSG_EXISTREQ_V1 structure defines the request
    ///  message sent to the IDL_DRSGetObjectExistence method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\4e7acc51-329a-4771-a6d2-4b490efbeb97.xml
    //  </remarks>
    public partial struct DRS_MSG_EXISTREQ_V1
    {

        /// <summary>
        ///  The objectGUID of the first object in the client's object
        ///  sequence.
        /// </summary>
        public Guid guidStart;

        /// <summary>
        ///  The number of objects in the client's object sequence.
        /// </summary>
        [CLSCompliant(false)]
        public uint cGuids;

        /// <summary>
        ///  The NC containing the objects in the sequence.
        /// </summary>
        [Indirect()]
        public DSNAME? pNC;

        /// <summary>
        ///  The filter excluding objects from the client's object
        ///  sequence.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public UPTODATE_VECTOR_V1_EXT[] pUpToDateVecCommonV1;

        /// <summary>
        ///  The digest of the objectGUID values of the objects in
        ///  the client's object sequence.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Md5Digest;
    }

    /// <summary>
    ///  The REFERR_DRS_WIRE_V1 structure defines a referral
    ///  to other DCs.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\5d52fff9-85c5-4407-955d-dbf5630ec37b.xml
    //  </remarks>
    public partial struct REFERR_DRS_WIRE_V1
    {

        /// <summary>
        ///  The implementation-specific diagnostic code.
        /// </summary>
        [CLSCompliant(false)]
        public uint dsid;

        /// <summary>
        ///  0, STATUS code, or Windows error code.
        /// </summary>
        [CLSCompliant(false)]
        public uint extendedErr;

        /// <summary>
        ///  The implementation-specific diagnostic code.
        /// </summary>
        [CLSCompliant(false)]
        public uint extendedData;

        /// <summary>
        ///  The DCs to contact to chase the referral.
        /// </summary>
        public CONTREF_DRS_WIRE_V1 Refer;
    }

    /// <summary>
    ///  The DRS_MSG_EXISTREQ union defines request messages
    ///  sent to the IDL_DRSGetObjectExistence method. Only
    ///  one version, identified by dwVersion = 1, is currently
    ///  defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\633f7365-229b-490c-9dd3-3bcd4df67b2b.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_EXISTREQ
    {

        /// <summary>
        ///  The version 1 request.
        /// </summary>
        [Case("1")]
        public DRS_MSG_EXISTREQ_V1 V1;
    }

    /// <summary>
    ///  The DS_REPL_CURSORS_3W structure defines a replication
    ///  cursor for a given NC replica. This structure is a
    ///  concrete representation of a sequence of ReplUpToDateVector
    ///  values; it is a superset of DS_REPL_CURSORS_2.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\63ecf5a1-9042-4f64-b2c4-ba94f9b13064.xml
    //  </remarks>
    public partial struct DS_REPL_CURSORS_3W
    {

        /// <summary>
        ///  The count of items in the rgCursor array.
        /// </summary>
        [CLSCompliant(false)]
        public uint cNumCursors;

        /// <summary>
        ///  0xFFFFFFFF, or the range bound of the results.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwEnumerationContext;

        /// <summary>
        ///  A set of replication cursors.
        /// </summary>
        [Inline()]
        [Size("cNumCursors")]
        public DS_REPL_CURSOR_3W[] rgCursor;
    }

    /// <summary>
    ///  The DRS_MSG_GETREPLINFO_REQ union defines the request
    ///  message versions sent to the IDL_DRSGetReplInfo method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\6b238f73-8e76-4431-b8fc-4f9445c511a5.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_GETREPLINFO_REQ
    {

        /// <summary>
        ///  Version 1 request.
        /// </summary>
        [Case("1")]
        public DRS_MSG_GETREPLINFO_REQ_V1 V1;

        /// <summary>
        ///  Version 2 request. The V2 request structure is a superset
        ///  of the V1 request structure.
        /// </summary>
        [Case("2")]
        public DRS_MSG_GETREPLINFO_REQ_V2 V2;
    }

    /// <summary>
    ///  The DRS_MSG_REVMEMB_REPLY union defines the response
    ///  messages received from the IDL_DRSGetMemberships method.
    ///  Only one version, identified by pdwOutVersion^ = 1,
    ///  is currently defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\71c72f3c-0da6-440f-b679-c2a24b21a34b.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_REVMEMB_REPLY
    {

        /// <summary>
        ///  Version 1 reply.
        /// </summary>
        [Case("1")]
        public DRS_MSG_REVMEMB_REPLY_V1 V1;
    }

    /// <summary>
    ///  The VALUE_META_DATA_EXT_V1 structure defines a concrete
    ///  type for the stamp of a link value.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\7530cf2e-a2ad-4716-a570-8383f8b1846f.xml
    //  </remarks>
    public partial struct VALUE_META_DATA_EXT_V1
    {

        /// <summary>
        ///  The date and time at which the first originating update
        ///  was made.
        /// </summary>
        public long timeCreated;

        /// <summary>
        ///  The remainder of the stamp; has the same PROPERTY_META_DATA_EXT
        ///  type as used for the stamp of an attribute.
        /// </summary>
        public PROPERTY_META_DATA_EXT MetaData;
    }

    public partial struct VALUE_META_DATA_EXT_V3
    {

        /// <summary>
        ///  The date and time at which the first originating update
        ///  was made.
        /// </summary>
        public long timeCreated;

        /// <summary>
        ///  The remainder of the stamp; has the same PROPERTY_META_DATA_EXT
        ///  type as used for the stamp of an attribute.
        /// </summary>
        public PROPERTY_META_DATA_EXT MetaData;

        public uint unused1;
        public uint unused2;
        public uint unused3;
        public long timeExpired;
    }

    /// <summary>
    ///  The DRS_MSG_ADDENTRYREPLY_V2 structure defines the response
    ///  message received from the IDL_DRSAddEntry method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\77568caa-02be-49f4-b36a-758776133416.xml
    //  </remarks>
    public partial struct DRS_MSG_ADDENTRYREPLY_V2
    {

        /// <summary>
        ///  Null, or the identity of the object that was being added
        ///  when an error occurred.
        /// </summary>
        [Indirect()]
        public DSNAME? pErrorObject;

        /// <summary>
        ///  0 if successful, otherwise a DIRERR error code (section
        ///  ).
        /// </summary>
        [CLSCompliant(false)]
        public uint errCode;

        /// <summary>
        ///  The implementation-specific diagnostic code.
        /// </summary>
        [CLSCompliant(false)]
        public uint dsid;

        /// <summary>
        ///  0, STATUS code, or Windows error code.
        /// </summary>
        [CLSCompliant(false)]
        public uint extendedErr;

        /// <summary>
        ///  The implementation-specific diagnostic code.
        /// </summary>
        [CLSCompliant(false)]
        public uint extendedData;

        /// <summary>
        ///  0 or PROBLEM error code (section ).
        /// </summary>
        [CLSCompliant(false)]
        public ushort problem;

        /// <summary>
        ///  The count of items in the infoList array.
        /// </summary>
        [PossibleValueRange("0", "10000")]
        [CLSCompliant(false)]
        public uint cObjectsAdded;

        /// <summary>
        ///  The identities of the added objects. The item order
        ///  matches the item order of values in the EntInfList
        ///  field in the request structure.
        /// </summary>
        [Size("cObjectsAdded")]
        public ADDENTRY_REPLY_INFO[] infoList;
    }

    /// <summary>
    ///  The DS_REPL_ATTR_VALUE_META_DATA_2 structure defines
    ///  a sequence of link value stamps. This structure is
    ///  a concrete representation of a sequence of LinkValueStamp
    ///  values; it is a superset of DS_REPL_ATTR_VALUE_META_DATA.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\78196358-ad2a-4c6b-83ff-cef40577ba85.xml
    //  </remarks>
    public partial struct DS_REPL_ATTR_VALUE_META_DATA_2
    {

        /// <summary>
        ///  The number of items in the rgMetaData array.
        /// </summary>
        [CLSCompliant(false)]
        public uint cNumEntries;

        /// <summary>
        ///  0xFFFFFFFF, or the range bound of the results.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwEnumerationContext;

        /// <summary>
        ///  The sequence of link value stamps.
        /// </summary>
        [Inline()]
        [Size("cNumEntries")]
        public DS_REPL_VALUE_META_DATA_2[] rgMetaData;
    }

    /// <summary>
    ///  The DRS_MSG_REPMOD union defines the request messages
    ///  for the IDL_DRSReplicaModify method. Only one version,
    ///  identified by dwVersion = 1, is currently defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\7f70db41-ce74-467d-adf1-b5df6ced12f9.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_REPMOD
    {

        /// <summary>
        ///  The version 1 request.
        /// </summary>
        [Case("1")]
        public DRS_MSG_REPMOD_V1 V1;
    }

    /// <summary>
    ///  The DS_REPL_CLIENT_CONTEXTS structure defines a set
    ///  of active RPC client connections. This structure is
    ///  a concrete representation of RPCClientContexts.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\7fe11aa4-54ca-4d61-8ab6-bd0f51e8c9e9.xml
    //  </remarks>
    public partial struct DS_REPL_CLIENT_CONTEXTS
    {

        /// <summary>
        ///  The number of items in the rgContext array.
        /// </summary>
        [PossibleValueRange("0", "10000")]
        [CLSCompliant(false)]
        public uint cNumContexts;

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public DS_REPL_CLIENT_CONTEXTS_dwReserved_Values dwReserved;

        /// <summary>
        ///  A set of active RPC client connections.
        /// </summary>
        [Inline()]
        [Size("cNumContexts")]
        public DS_REPL_CLIENT_CONTEXT[] rgContext;
    }

    /// <summary>
    /// The reserved of DS_REPL_CLIENT_CONTEXTS.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum DS_REPL_CLIENT_CONTEXTS_dwReserved_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The DRS_MSG_REVMEMB_REQ union defines the request messages
    ///  sent to the IDL_DRSGetMemberships method. Only one
    ///  version, identified by dwInVersion = 1, is currently
    ///  defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\8172ef3e-9ffc-406e-a028-448328de5f76.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_REVMEMB_REQ
    {

        /// <summary>
        ///  Version 1 request. Currently only one version is defined.
        /// </summary>
        [Case("1")]
        public DRS_MSG_REVMEMB_REQ_V1 V1;
    }

    /// <summary>
    ///  The DS_REPL_ATTR_VALUE_META_DATA structure defines a
    ///  sequence of link value stamps. This structure is a
    ///  concrete representation of a sequence of LinkValueStamp
    ///  values.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\8e3dbb53-7f7f-4c37-9095-921fa9c0a4df.xml
    //  </remarks>
    public partial struct DS_REPL_ATTR_VALUE_META_DATA
    {

        /// <summary>
        ///  The number of items in rgMetaData array.
        /// </summary>
        [CLSCompliant(false)]
        public uint cNumEntries;

        /// <summary>
        ///  0xFFFFFFFF, or the range bound of the results.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwEnumerationContext;

        /// <summary>
        ///  The sequence of link value stamps.
        /// </summary>
        [Inline()]
        [Size("cNumEntries")]
        public DS_REPL_VALUE_META_DATA[] rgMetaData;
    }

    /// <summary>
    ///  The DRS_MSG_GETMEMBERSHIPS2_REQ_V1 structure defines
    ///  the request message sent to the IDL_DRSGetMemberships2
    ///  method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\91151fe9-19eb-4011-8d49-ae38e4215c3e.xml
    //  </remarks>
    public partial struct DRS_MSG_GETMEMBERSHIPS2_REQ_V1
    {

        /// <summary>
        ///  Count of items in the Requests array.
        /// </summary>
        [PossibleValueRange("1", "10000")]
        [CLSCompliant(false)]
        public uint Count;

        /// <summary>
        ///  Sequence of reverse membership requests.
        /// </summary>
        [Size("Count")]
        public DRS_MSG_REVMEMB_REQ_V1[] Requests;
    }

    /// <summary>
    ///  The DRS_MSG_GETMEMBERSHIPS2_REPLY_V1 structure defines
    ///  the response message received from the IDL_DRSGetMemberships2
    ///  method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\92cb5c90-905a-4142-acd5-c79ba4ac6872.xml
    //  </remarks>
    public partial struct DRS_MSG_GETMEMBERSHIPS2_REPLY_V1
    {

        /// <summary>
        ///  Count of items in the Replies array.
        /// </summary>
        [PossibleValueRange("0", "10000")]
        [CLSCompliant(false)]
        public uint Count;

        /// <summary>
        ///  Sequence of reverse membership replies, in the same
        ///  order as the Requests field of the request message.
        /// </summary>
        [Size("Count")]
        public DRS_MSG_REVMEMB_REPLY_V1[] Replies;
    }

    /// <summary>
    ///  The PROBLEMLIST_DRS_WIRE_V1 structure defines an attribute
    ///  error link entry.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\96ee7875-263b-4fc7-9fa3-1dc7d19c8d9d.xml
    //  </remarks>
    [CLSCompliant(false)]
    public partial struct _PROBLEMLIST_DRS_WIRE_V1
    {

        /// <summary>
        ///  Null, or a pointer to the next item in the list.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _PROBLEMLIST_DRS_WIRE_V1[] pNextProblem;

        /// <summary>
        ///  Attribute error description.
        /// </summary>
        public INTFORMPROB_DRS_WIRE_V1 intprob;
    }

    /// <summary>
    ///  The SCHEMA_PREFIX_TABLE structure defines the concrete
    ///  type for a table to map ATTRTYP values to and from
    ///  OIDs.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\9b371267-e8b8-4c69-9979-02dae02e5e38.xml
    //  </remarks>
    public partial struct SCHEMA_PREFIX_TABLE
    {

        /// <summary>
        ///  The number of items in the pPrefixEntry array.
        /// </summary>
        [PossibleValueRange("0", "1048576")]
        [CLSCompliant(false)]
        public uint PrefixCount;

        /// <summary>
        ///  An array of PrefixTableEntry items in the table.
        /// </summary>
        [Size("PrefixCount")]
        public PrefixTableEntry[] pPrefixEntry;
    }

    /// <summary>
    ///  The DS_REPL_OBJ_META_DATA structure defines a set of
    ///  attributestamps for a given object. This structure
    ///  is a concrete representation of the sequence of AttributeStamp
    ///  values for all attributes of a given object.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\a2ce73fa-aacd-49d7-9111-10ef08001963.xml
    //  </remarks>
    public partial struct DS_REPL_OBJ_META_DATA
    {

        /// <summary>
        ///  The count of items in the rgMetaData array.
        /// </summary>
        [CLSCompliant(false)]
        public uint cNumEntries;

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public DS_REPL_OBJ_META_DATA_dwReserved_Values dwReserved;

        /// <summary>
        ///  A set of attributestamps.
        /// </summary>
        [Inline()]
        [Size("cNumEntries")]
        public DS_REPL_ATTR_META_DATA[] rgMetaData;
    }

    /// <summary>
    /// The reserved of DS_REPL_OBJ_META_DATA.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum DS_REPL_OBJ_META_DATA_dwReserved_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The ATTR structure defines a concrete type for the identity
    ///  and values of an attribute.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\a2db41e2-7803-4d3c-a499-0fee92b1c149.xml
    //  </remarks>
    public partial struct ATTR
    {

        /// <summary>
        ///  An attribute.
        /// </summary>
        [CLSCompliant(false)]
        public uint attrTyp;

        /// <summary>
        ///  The sequence of values for this attribute.
        /// </summary>
        public ATTRVALBLOCK AttrVal;
    }

    /// <summary>
    ///  The PROPERTY_META_DATA structure contains attribute
    ///  and stamp information. For more information, see section
    ///  .The binary portion of the DNBinary value of the msDS-RevealedUsersattribute
    ///  contains this structure.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\ab1ad920-3538-4d64-9197-6e700ef1f222.xml
    //  </remarks>
    public partial struct PROPERTY_META_DATA
    {

        /// <summary>
        ///  Attribute whose value was revealed.
        /// </summary>
        [CLSCompliant(false)]
        public uint attrType;

        /// <summary>
        ///  Stamp of revealed attribute value.
        /// </summary>
        public PROPERTY_META_DATA_EXT propMetadataExt;
    }

    public struct PROPERTY_META_DATA_VECTOR
    {
        public ulong dwVersion;
        public PROPERTY_META_DATA_VECTOR_V1 V1;
    }

    public struct PROPERTY_META_DATA_VECTOR_V1
    {
        public ulong cNumProps;

        [Inline()]
        [Size("cNumProps")]
        public PROPERTY_META_DATA[] rgMetaData;
    }

    /// <summary>
    ///  The DS_REPL_CURSORS structure defines a set of replication
    ///  cursors for a given NC replica. This structure is a
    ///  concrete representation of a sequence of ReplUpToDateVector
    ///  values.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\bfab2029-039c-442e-8a92-4378d3a27473.xml
    //  </remarks>
    public partial struct DS_REPL_CURSORS
    {

        /// <summary>
        ///  The count of items in the rgCursor array.
        /// </summary>
        [CLSCompliant(false)]
        public uint cNumCursors;

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public DS_REPL_CURSORS_dwReserved_Values dwReserved;

        /// <summary>
        ///  A set of replication cursors.
        /// </summary>
        [Inline()]
        [Size("cNumCursors")]
        public DS_REPL_CURSOR[] rgCursor;
    }

    /// <summary>
    /// The reserved of DS_REPL_CURSORS.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum DS_REPL_CURSORS_dwReserved_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The UPTODATE_VECTOR_V2_EXT structure defines a concrete
    ///  type for the replication state relative to a set of
    ///  DCs.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\cebd1ccb-891b-4268-b056-4b714cdf981e.xml
    //  </remarks>
    public partial struct UPTODATE_VECTOR_V2_EXT
    {

        /// <summary>
        ///  The version of this structure; MUST be 2.
        /// </summary>
        [CLSCompliant(false)]
        public UPTODATE_VECTOR_V2_EXT_dwVersion_Values dwVersion;

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public UPTODATE_VECTOR_V2_EXT_dwReserved1_Values dwReserved1;

        /// <summary>
        ///  The number of items in the rgCursors array.
        /// </summary>
        [PossibleValueRange("0", "1048576")]
        [CLSCompliant(false)]
        public uint cNumCursors;

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public UPTODATE_VECTOR_V2_EXT_dwReserved2_Values dwReserved2;

        /// <summary>
        ///  An array of UPTODATE_CURSOR_V2. The items in this field
        ///  MUST be sorted in increasing order of the uuidDsa field.
        /// </summary>
        [Inline()]
        [Size("cNumCursors")]
        public UPTODATE_CURSOR_V2[] rgCursors;
    }

    /// <summary>
    /// The version of UPTODATE_VECTOR_V2_EXT.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum UPTODATE_VECTOR_V2_EXT_dwVersion_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 2,
    }

    /// <summary>
    /// The reserved1 of UPTODATE_VECTOR_V2_EXT.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum UPTODATE_VECTOR_V2_EXT_dwReserved1_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// The reserved2 of UPTODATE_VECTOR_V2_EXT.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum UPTODATE_VECTOR_V2_EXT_dwReserved2_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The DRS_MSG_REPSYNC union defines the request messages
    ///  sent to the IDL_DRSReplicaSync method. Only one version,
    ///  identified by dwVersion = 1, is currently defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\d1ed3b4e-6964-468e-9ef1-f9a6f25cde0e.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_REPSYNC
    {

        /// <summary>
        ///  The version 1 request.
        /// </summary>
        [Case("1")]
        public DRS_MSG_REPSYNC_V1 V1;

        /// <summary>
        ///  The version 2 request.
        /// </summary>
        [Case("2")]
        public DRS_MSG_REPSYNC_V2 V2;
    }

    /// <summary>
    ///  The DRS_MSG_GETMEMBERSHIPS2_REPLY union defines response
    ///  messages received from the IDL_DRSGetMemberships2 method.
    ///  Only one version, identified by pdwOutVersion^ = 1,
    ///  is currently defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\e5219fc0-1cfb-4ea4-9a53-9c50ddd4c4ef.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_GETMEMBERSHIPS2_REPLY
    {

        /// <summary>
        ///  Version 1 response.
        /// </summary>
        [Case("1")]
        public DRS_MSG_GETMEMBERSHIPS2_REPLY_V1 V1;
    }

    /// <summary>
    ///  The ATTRBLOCK structure defines a concrete type for
    ///  a set of attributes and their values.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\f81324b8-6400-41b5-bc25-5117589c602a.xml
    //  </remarks>
    public partial struct ATTRBLOCK
    {

        /// <summary>
        ///  The number of items in the pAttr array.
        /// </summary>
        [CLSCompliant(false)]
        public uint attrCount;

        /// <summary>
        ///  An array of attributes and their values.
        /// </summary>
        [Size("attrCount")]
        public ATTR[] pAttr;
    }

    /// <summary>
    ///  The DRS_MSG_GETCHGREQ_V5 structure defines the request
    ///  message sent to the IDL_DRSGetNCChanges method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\fd24b73c-7b81-43af-8c77-65bc2e3181b7.xml
    //  </remarks>
    public partial struct DRS_MSG_GETCHGREQ_V5
    {

        /// <summary>
        ///  DSA GUID of the client DC.
        /// </summary>
        public Guid uuidDsaObjDest;

        /// <summary>
        ///  Invocation ID of the server DC.
        /// </summary>
        public Guid uuidInvocIdSrc;

        /// <summary>
        ///  NC root of the replica to replicate or the FSMO role
        ///  object for an extended operation.
        /// </summary>
        [Indirect()]
        public DSNAME? pNC;

        /// <summary>
        ///  Data used to correlate calls to IDL_DRSGetNCChanges.
        /// </summary>
        public USN_VECTOR usnvecFrom;

        /// <summary>
        ///  Stamp filter that describes updates the client has already
        ///  applied.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public UPTODATE_VECTOR_V1_EXT[] pUpToDateVecDestV1;

        /// <summary>
        ///  DRS_OPTIONS bit field.
        /// </summary>
        [CLSCompliant(false)]
        public uint ulFlags;

        /// <summary>
        ///  Approximate cap on the number of objects to include
        ///  in the reply.
        /// </summary>
        [CLSCompliant(false)]
        public uint cMaxObjects;

        /// <summary>
        ///  Approximate cap on the number of bytes to include in
        ///  the reply.
        /// </summary>
        [CLSCompliant(false)]
        public uint cMaxBytes;

        /// <summary>
        ///  0 or an extended operation request code (section ).
        /// </summary>
        [CLSCompliant(false)]
        public uint ulExtendedOp;

        /// <summary>
        ///  0 or a value specific to the requested extended operation.
        /// </summary>
        public ULARGE_INTEGER liFsmoInfo;
    }

    /// <summary>
    ///  The DRS_MSG_NT4_CHGLOG_REPLY union defines the response
    ///  messages received from the IDL_DRSGetNT4ChangeLog method.
    ///  Only one version, identified by dwVersion = 1, is currently
    ///  defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\14ba8fee-3bba-4793-b781-18f328bbd767.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_NT4_CHGLOG_REPLY
    {

        /// <summary>
        ///  The version 1 reply.
        /// </summary>
        [Case("1")]
        public DRS_MSG_NT4_CHGLOG_REPLY_V1 V1;
    }

    /// <summary>
    ///  The REPLVALINF structure defines a concrete type for
    ///  the identity and stamp of a link value.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\22946fbf-170e-4ab4-82c7-dabdfd97bf5a.xml
    //  </remarks>
    public partial struct REPLVALINF_V1
    {

        /// <summary>
        ///  Identifies the object with the attribute that contains
        ///  the link value.
        /// </summary>
        [Indirect()]
        public DSNAME? pObject;

        /// <summary>
        ///  An attribute that contains the link value.
        /// </summary>
        [CLSCompliant(false)]
        public uint attrTyp;

        /// <summary>
        ///  The link value.
        /// </summary>
        [CLSCompliant(false)]
        public ATTRVAL Aval;

        /// <summary>
        ///  FALSE if and only if the link value has been removed
        ///  from the attribute.
        /// </summary>
        [CLSCompliant(false)]
        public int fIsPresent;

        /// <summary>
        ///  The stamp associated with the link value.
        /// </summary>
        public VALUE_META_DATA_EXT_V1 MetaData;
    }

    public partial struct REPLVALINF_V3
    {

        /// <summary>
        ///  Identifies the object with the attribute that contains
        ///  the link value.
        /// </summary>
        [Indirect()]
        public DSNAME? pObject;

        /// <summary>
        ///  An attribute that contains the link value.
        /// </summary>
        [CLSCompliant(false)]
        public uint attrTyp;

        /// <summary>
        ///  The link value.
        /// </summary>
        public ATTRVAL Aval;

        /// <summary>
        ///  FALSE if and only if the link value has been removed
        ///  from the attribute.
        /// </summary>
        [CLSCompliant(false)]
        public int fIsPresent;

        /// <summary>
        ///  The stamp associated with the link value.
        /// </summary>
        public VALUE_META_DATA_EXT_V3 MetaData;
    }

    /// <summary>
    ///  The DRS_MSG_GETREPLINFO_REPLY union defines response
    ///  messages received from the IDL_DRSGetReplInfo method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\374665d4-0c2f-4f0c-9866-d6b0855ede60.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_GETREPLINFO_REPLY
    {

        /// <summary>
        ///  Neighbor information.
        /// </summary>
        [Case("0")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public DS_REPL_NEIGHBORSW[] pNeighbors;

        /// <summary>
        ///  Cursors for an NC replica.
        /// </summary>
        [Case("1")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public DS_REPL_CURSORS[] pCursors;

        /// <summary>
        ///  Attributestamps.
        /// </summary>
        [Case("2")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public DS_REPL_OBJ_META_DATA[] pObjMetaData;

        /// <summary>
        ///  Connection failure data.
        /// </summary>
        [Case("3")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public DS_REPL_KCC_DSA_FAILURESW[] pConnectFailures;

        /// <summary>
        ///  Link failure data.
        /// </summary>
        [Case("4")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public DS_REPL_KCC_DSA_FAILURESW[] pLinkFailures;

        /// <summary>
        ///  Pending operations in the replication queue.
        /// </summary>
        [Case("5")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public DS_REPL_PENDING_OPSW[] pPendingOps;

        /// <summary>
        ///  Link value stamps.
        /// </summary>
        [Case("6")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public DS_REPL_ATTR_VALUE_META_DATA[] pAttrValueMetaData;

        /// <summary>
        ///  Cursors for an NC replica.
        /// </summary>
        [Case("7")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public DS_REPL_CURSORS_2[] pCursors2;

        /// <summary>
        ///  Cursors for an NC replica.
        /// </summary>
        [Case("8")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public DS_REPL_CURSORS_3W[] pCursors3;

        /// <summary>
        ///  Attribute stamps.
        /// </summary>
        [Case("9")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public DS_REPL_OBJ_META_DATA_2[] pObjMetaData2;

        /// <summary>
        ///  Link value stamps.
        /// </summary>
        [Case("10")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public DS_REPL_ATTR_VALUE_META_DATA_2[] pAttrValueMetaData2;

        /// <summary>
        ///  Outstanding requests from this DC to other DCs.
        /// </summary>
        [Case("-6")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public DS_REPL_SERVER_OUTGOING_CALLS[] pServerOutgoingCalls;

        /// <summary>
        ///  Cursors for an NC replica.
        /// </summary>
        [Case("-5")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public UPTODATE_VECTOR_V1_EXT[] pUpToDateVec;

        /// <summary>
        ///  Active RPC client connections.
        /// </summary>
        [Case("-4")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public DS_REPL_CLIENT_CONTEXTS[] pClientContexts;

        /// <summary>
        ///  Neighbor information.
        /// </summary>
        [Case("-2")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public DS_REPL_NEIGHBORSW[] pRepsTo;
    }

    /// <summary>
    ///  The DRS_MSG_GETMEMBERSHIPS2_REQ union defines request
    ///  messages sent to the IDL_DRSGetMemberships2 method.
    ///  Only one version, identified by dwInVersion = 1, is
    ///  currently defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\37f3ba0f-c5c5-48c8-888a-a28a082a61bf.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_GETMEMBERSHIPS2_REQ
    {

        /// <summary>
        ///  Version 1 request.
        /// </summary>
        [Case("1")]
        public DRS_MSG_GETMEMBERSHIPS2_REQ_V1 V1;
    }

    /// <summary>
    ///  The DRS_MSG_GETCHGREQ_V8 structure defines the request
    ///  message sent to the IDL_DRSGetNCChanges method. This
    ///  message version is a superset of DRS_MSG_GETCHGREQ_V5.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\4304bb4a-e9b5-4c8a-8731-df4d6f9ab567.xml
    //  </remarks>
    public partial struct DRS_MSG_GETCHGREQ_V8
    {

        /// <summary>
        ///  DSA GUID of the client DC.
        /// </summary>
        public Guid uuidDsaObjDest;

        /// <summary>
        ///  Invocation ID of the server DC.
        /// </summary>
        public Guid uuidInvocIdSrc;

        /// <summary>
        ///  NC root of the replica to replicate or the FSMO role
        ///  object for an extended operation.
        /// </summary>
        [Indirect()]
        public DSNAME? pNC;

        /// <summary>
        ///  Data used to correlate calls to IDL_DRSGetNCChanges.
        /// </summary>
        public USN_VECTOR usnvecFrom;

        /// <summary>
        ///  Stamp filter describing updates the client has already
        ///  applied.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public UPTODATE_VECTOR_V1_EXT[] pUpToDateVecDest;

        /// <summary>
        ///  A DRS_OPTIONS bit field.
        /// </summary>
        [CLSCompliant(false)]
        public uint ulFlags;

        /// <summary>
        ///  Approximate cap on the number of objects to include
        ///  in the reply.
        /// </summary>
        [CLSCompliant(false)]
        public uint cMaxObjects;

        /// <summary>
        ///  Approximate cap on the number of bytes to include in
        ///  the reply.
        /// </summary>
        [CLSCompliant(false)]
        public uint cMaxBytes;

        /// <summary>
        ///  0 or an extended operation request code (section ).
        /// </summary>
        [CLSCompliant(false)]
        public uint ulExtendedOp;

        /// <summary>
        ///  0 or a value specific to the requested extended operation.
        /// </summary>
        public ULARGE_INTEGER liFsmoInfo;

        /// <summary>
        ///  A set of one or more attributes whose values are to
        ///  be replicated to the client's partial replica, or null
        ///  if the client has a full replica.
        /// </summary>
        [Indirect()]
        public PARTIAL_ATTR_VECTOR_V1_EXT? pPartialAttrSet;

        /// <summary>
        ///  A set of one or more attributes whose values are to
        ///  be added to the client's existing partial replica,
        ///  or null.
        /// </summary>
        [Indirect()]
        public PARTIAL_ATTR_VECTOR_V1_EXT? pPartialAttrSetEx;

        /// <summary>
        ///  Prefix table with which to convert the ATTRTYP values
        ///  in pPartialAttrSet and pPartialAttrSetEx to OIDs.
        /// </summary>
        public SCHEMA_PREFIX_TABLE PrefixTableDest;
    }

    /// <summary>
    ///  The DRS_MSG_VERIFYREQ_V1 structure defines a request
    ///  message sent to the IDL_DRSVerifyNames method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\4593a76b-71f1-4e4c-b5e1-4d2e27afb3cb.xml
    //  </remarks>
    public partial struct DRS_MSG_VERIFYREQ_V1
    {

        /// <summary>
        ///  The type of name to be verified; MUST have one of the
        ///  following values:
        /// </summary>
        [CLSCompliant(false)]
        public uint dwFlags;

        /// <summary>
        ///  The number of items in the rpNames array.
        /// </summary>
        [PossibleValueRange("1", "10000")]
        [CLSCompliant(false)]
        public uint cNames;

        /// <summary>
        ///  An array of DSNames that need to be verified.
        /// </summary>
        [Size("cNames, 1")]
        public DSNAME[][] rpNames;

        /// <summary>
        ///  The list of attributes to be retrieved for each name
        ///  that is verified.
        /// </summary>
        public ATTRBLOCK RequiredAttrs;

        /// <summary>
        ///  The prefix table used to translate ATTRTYP values in
        ///  RequiredAttrs to OID values.
        /// </summary>
        public SCHEMA_PREFIX_TABLE PrefixTable;
    }

    /// <summary>
    ///  The dwFlags values of DRS_MSG_VERIFYREQ_V1.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [Flags()]
    [CLSCompliant(false)]
    public enum DRS_MSG_VERIFYREQ_V1_dwFlags_Values : uint
    {

        /// <summary>
        ///  Verify DSName values.
        /// </summary>
        DRS_VERIFY_DSNAMES = 0x00000000,

        /// <summary>
        ///  Verify objectSid values.
        /// </summary>
        DRS_VERIFY_SIDS = 0x00000001,

        /// <summary>
        ///  Verify sAMAccountName values.
        /// </summary>
        DRS_VERIFY_SAM_ACCOUNT_NAMES = 0x00000002,

        /// <summary>
        ///  Verify foreign principal object names.
        /// </summary>
        DRS_VERIFY_FPOS = 0x00000003,
    }

    /// <summary>
    ///  The DRS_MSG_GETCHGREQ_V3 structure defines a portion
    ///  of the request message that is sent to the IDL_DRSGetNCChanges
    ///  method as part of SMTP replication ([MS-SRPL]). This
    ///  is not a complete request message; it is embedded in
    ///  DRS_MSG_GETCHGREQ_V4 and DRS_MSG_GETCHGREQ_V7.Though
    ///  this request version appears in the IDL, Windows DCs
    ///  never send this request version by means of RPC. It
    ///  exists solely to support SMTP replication (see [MS-SRPL]).
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\6a2a056c-ac7f-47d0-9e6d-9023a4e5947c.xml
    //  </remarks>
    public partial struct DRS_MSG_GETCHGREQ_V3
    {

        /// <summary>
        ///  DSA GUID of the client DC.
        /// </summary>
        public Guid uuidDsaObjDest;

        /// <summary>
        ///  Invocation ID of the server DC.
        /// </summary>
        public Guid uuidInvocIdSrc;

        /// <summary>
        ///  NC root of the replica to replicate or the FSMO role
        ///  object for an extended operation.
        /// </summary>
        [Indirect()]
        public DSNAME? pNC;

        /// <summary>
        ///  Data that is used to correlate calls to IDL_DRSGetNCChanges.
        /// </summary>
        public USN_VECTOR usnvecFrom;

        /// <summary>
        ///  Stamp filter describing updates that the client has
        ///  already applied.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public UPTODATE_VECTOR_V1_EXT[] pUpToDateVecDestV1;

        /// <summary>
        ///  A set of one or more attributes whose values are to
        ///  be replicated to the client's partial replica.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public PARTIAL_ATTR_VECTOR_V1_EXT[] pPartialAttrVecDestV1;

        /// <summary>
        ///  Prefix table with which to convert the ATTRTYP values
        ///  in pPartialAttrVecDestV1 to OIDs.
        /// </summary>
        public SCHEMA_PREFIX_TABLE PrefixTableDest;

        /// <summary>
        ///  A DRS_OPTIONS bit field.
        /// </summary>
        [CLSCompliant(false)]
        public uint ulFlags;

        /// <summary>
        ///  An approximate cap on the number of objects to include
        ///  in the reply.
        /// </summary>
        [CLSCompliant(false)]
        public uint cMaxObjects;

        /// <summary>
        ///  An approximate cap on the number of bytes to include
        ///  in the reply.
        /// </summary>
        [CLSCompliant(false)]
        public uint cMaxBytes;

        /// <summary>
        ///  0 or an EXOP_REQ code (section ).
        /// </summary>
        [CLSCompliant(false)]
        public uint ulExtendedOp;
    }

    /// <summary>
    ///  ENTINF is a concrete type for the identity and attributes
    ///  (some or all) of a given object.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\6d69822e-adb6-4977-8553-c2d529c17e5b.xml
    //  </remarks>
    public partial struct ENTINF
    {

        /// <summary>
        ///  The identity of the object.
        /// </summary>
        [Indirect()]
        public DSNAME? pName;

        /// <summary>
        ///  A flags field that supports the following flags.01234567891
        ///  01234567892 01234567893 01MDOXXXXXXXXXXXXXXRMXXXXXXXXXXXXXXXX:
        ///  Unused. MUST be zero and ignored.M (ENTINF_FROM_MASTER):
        ///  Retrieved from a full replica.DO (ENTINF_DYNAMIC_OBJECT):
        ///  A dynamic object.RM (ENTINF_REMOTE_MODIFY): A remote
        ///  modify request to IDL_DRSAddEntry (section ).
        /// </summary>
        [CLSCompliant(false)]
        public uint ulFlags;

        /// <summary>
        ///  Some of all of the attributes for this object, as determined
        ///  by the particular method. See section for an overview
        ///  of methods using type ENTINF.
        /// </summary>
        public ATTRBLOCK AttrBlock;
    }

    /// <summary>
    ///  The ATRERR_DRS_WIRE_V1 structure defines attribute errors.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\77ad67f4-4670-45a0-88e7-aad7a920b915.xml
    //  </remarks>
    public partial struct ATRERR_DRS_WIRE_V1
    {

        /// <summary>
        ///  The identity of the object being processed when the
        ///  error occurred.
        /// </summary>
        [Indirect()]
        public DSNAME? pObject;

        /// <summary>
        ///  The count of items in the FirstProblem linked list.
        /// </summary>
        [CLSCompliant(false)]
        public uint count;

        /// <summary>
        ///  The first element in the linked list of attribute errors.
        /// </summary>
        [CLSCompliant(false)]
        public _PROBLEMLIST_DRS_WIRE_V1 FirstProblem;
    }

    /// <summary>
    ///  The DRS_MSG_MOVEREPLY_V1 structure defines a response
    ///  message received from the IDL_DRSInterDomainMove method.
    ///  This response version is obsolete.Although this response
    ///  version appears in the IDL, windowsDCs do not support
    ///  it.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\7a469f42-9d32-4cac-b6d7-29c1ad175ab0.xml
    //  </remarks>
    public partial struct DRS_MSG_MOVEREPLY_V1
    {

        /// <summary>
        ///  The object as it appears following the move operation.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ENTINF[][] ppResult;

        /// <summary>
        ///  The prefix table with which to translate the ATTRTYP
        ///  values in ppResult to OIDs.
        /// </summary>
        public SCHEMA_PREFIX_TABLE PrefixTable;

        /// <summary>
        ///  0 if successful, or non-zero if a fatal error occurred.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        [CLSCompliant(false)]
        public uint[] pError;
    }

    /// <summary>
    ///  The DRS_MSG_GETCHGREQ_V10 structure defines the request
    ///  message sent to the IDL_DRSGetNCChanges method. This
    ///  message version is a superset of DRS_MSG_GETCHGREQ_V8.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\92b1b77d-2058-46e0-9e8c-6664b96a0cf9.xml
    //  </remarks>
    public partial struct DRS_MSG_GETCHGREQ_V10
    {

        /// <summary>
        ///  DSA GUID of the client DC.
        /// </summary>
        public Guid uuidDsaObjDest;

        /// <summary>
        ///  Invocation ID of the server DC.
        /// </summary>
        public Guid uuidInvocIdSrc;

        /// <summary>
        ///  NC root of the replica to replicate or the FSMO role
        ///  object for an extended operation.
        /// </summary>
        [Indirect()]
        public DSNAME? pNC;

        /// <summary>
        ///  Data used to correlate calls to IDL_DRSGetNCChanges.
        /// </summary>
        public USN_VECTOR usnvecFrom;

        /// <summary>
        ///  Stamp filter describing updates the client has already
        ///  applied.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public UPTODATE_VECTOR_V1_EXT[] pUpToDateVecDest;

        /// <summary>
        ///  A DRS_OPTIONS bit field.
        /// </summary>
        [CLSCompliant(false)]
        public uint ulFlags;

        /// <summary>
        ///  Approximate cap on the number of objects to include
        ///  in the reply.
        /// </summary>
        [CLSCompliant(false)]
        public uint cMaxObjects;

        /// <summary>
        ///  Approximate cap on the number of bytes to include in
        ///  the reply.
        /// </summary>
        [CLSCompliant(false)]
        public uint cMaxBytes;

        /// <summary>
        ///  0 or an extended operation request code (section ).
        /// </summary>
        [CLSCompliant(false)]
        public uint ulExtendedOp;

        /// <summary>
        ///  0 or a value specific to the requested extended operation.
        /// </summary>
        public ULARGE_INTEGER liFsmoInfo;

        /// <summary>
        ///  A set of one or more attributes whose values are to
        ///  be replicated to the client's partial replica, or null
        ///  if the client has a full replica.
        /// </summary>
        [Indirect()]
        public PARTIAL_ATTR_VECTOR_V1_EXT? pPartialAttrSet;

        /// <summary>
        ///  A set of one or more attributes whose values are to
        ///  be added to the client's existing partial replica,
        ///  or null.
        /// </summary>
        [Indirect()]
        public PARTIAL_ATTR_VECTOR_V1_EXT? pPartialAttrSetEx;

        /// <summary>
        ///  Prefix table with which to convert the ATTRTYP values
        ///  in pPartialAttrSet and pPartialAttrSetEx to OIDs.
        /// </summary>
        public SCHEMA_PREFIX_TABLE PrefixTableDest;

        /// <summary>
        ///  A DRS_MORE_GETCHGREQ_OPTIONS bit field.
        /// </summary>
        [CLSCompliant(false)]
        public uint ulMoreFlags;
    }

    /// <summary>
    ///  The DRS_MSG_GETCHGREQ_V11 structure defines the request
    ///  message sent to the IDL_DRSGetNCChanges method.
    ///  This message version is a superset of DRS_MSG_GETCHGREQ_V10.
    /// </summary>
    public partial struct DRS_MSG_GETCHGREQ_V11
    {

        /// <summary>
        ///  DSA GUID of the client DC.
        /// </summary>
        public Guid uuidDsaObjDest;

        /// <summary>
        ///  Invocation ID of the server DC.
        /// </summary>
        public Guid uuidInvocIdSrc;

        /// <summary>
        ///  NC root of the replica to replicate or the FSMO role
        ///  object for an extended operation.
        /// </summary>
        [Indirect()]
        public DSNAME? pNC;

        /// <summary>
        ///  Data used to correlate calls to IDL_DRSGetNCChanges.
        /// </summary>
        public USN_VECTOR usnvecFrom;

        /// <summary>
        ///  Stamp filter describing updates the client has already
        ///  applied.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public UPTODATE_VECTOR_V1_EXT[] pUpToDateVecDest;

        /// <summary>
        ///  A DRS_OPTIONS bit field.
        /// </summary>
        [CLSCompliant(false)]
        public uint ulFlags;

        /// <summary>
        ///  Approximate cap on the number of objects to include
        ///  in the reply.
        /// </summary>
        [CLSCompliant(false)]
        public uint cMaxObjects;

        /// <summary>
        ///  Approximate cap on the number of bytes to include in
        ///  the reply.
        /// </summary>
        [CLSCompliant(false)]
        public uint cMaxBytes;

        /// <summary>
        ///  0 or an extended operation request code (section ).
        /// </summary>
        [CLSCompliant(false)]
        public uint ulExtendedOp;

        /// <summary>
        ///  0 or a value specific to the requested extended operation.
        /// </summary>
        public ULARGE_INTEGER liFsmoInfo;

        /// <summary>
        ///  A set of one or more attributes whose values are to
        ///  be replicated to the client's partial replica, or null
        ///  if the client has a full replica.
        /// </summary>
        [Indirect()]
        public PARTIAL_ATTR_VECTOR_V1_EXT? pPartialAttrSet;

        /// <summary>
        ///  A set of one or more attributes whose values are to
        ///  be added to the client's existing partial replica,
        ///  or null.
        /// </summary>
        [Indirect()]
        public PARTIAL_ATTR_VECTOR_V1_EXT? pPartialAttrSetEx;

        /// <summary>
        ///  Prefix table with which to convert the ATTRTYP values
        ///  in pPartialAttrSet and pPartialAttrSetEx to OIDs.
        /// </summary>
        public SCHEMA_PREFIX_TABLE PrefixTableDest;

        /// <summary>
        ///  A DRS_MORE_GETCHGREQ_OPTIONS bit field.
        /// </summary>
        [CLSCompliant(false)]
        public uint ulMoreFlags;

        /// <summary>
        /// An identifier for the operation that the DC can use for implementation-defined troubleshooting.
        /// There are no normative constraints on this value,
        /// nor does the value figure in any normative processing rules.
        /// </summary>
        public Guid correlationID;

        /// <summary>
        /// A pointer to a VAR_SIZE_BUFFER_WITH_VERSION structure (section 5.219).
        /// MUST be a null pointer.
        /// </summary>
        public VAR_SIZE_BUFFER_WITH_VERSION pReservedBuffer;
    }

    /// <summary>
    ///  The DRS_MSG_GETCHGREQ_V4 structure defines the request
    ///  message sent to the IDL_DRSGetNCChanges method. This
    ///  message version is a superset of DRS_MSG_GETCHGREQ_V3.Although
    ///  this request version appears in the IDL, Windows DCs
    ///  never send this request version using RPC. It exists
    ///  solely to support SMTP replication ([MS-SRPL]).
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\9db4db21-8ccd-4c81-8662-6e2baff8426c.xml
    //  </remarks>
    public partial struct DRS_MSG_GETCHGREQ_V4
    {

        /// <summary>
        ///  The objectGUID of the interSiteTransportobject that
        ///  identifies the transport by which to send the reply.
        /// </summary>
        public Guid uuidTransportObj;

        /// <summary>
        ///  The transport-specific address to which to send the
        ///  reply.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public MTX_ADDR[] pmtxReturnAddress;

        /// <summary>
        ///  Version 3 request.
        /// </summary>
        public DRS_MSG_GETCHGREQ_V3 V3;
    }

    /// <summary>
    ///  The DRS_MSG_VERIFYREQ union defines the request messages
    ///  sent to the IDL_DRSVerifyNames method. Only one version,
    ///  identified by dwVersion = 1, is currently defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\a2681aea-058b-47c2-961d-58cc973fa2f9.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_VERIFYREQ
    {

        /// <summary>
        ///  The version 1 request.
        /// </summary>
        [Case("1")]
        public DRS_MSG_VERIFYREQ_V1 V1;
    }

    /// <summary>
    ///  The DRS_MSG_ADDENTRYREQ_V1 structure defines the request
    ///  message sent to the IDL_DRSAddEntry method. This request
    ///  version is obsolete.Though this request version appears
    ///  in the IDL, windowsDCs do not support it. It was never
    ///  supported in a released version of windows_server.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\c02d658c-9c68-4df5-bcd5-b554d7ebc873.xml
    //  </remarks>
    public partial struct DRS_MSG_ADDENTRYREQ_V1
    {

        /// <summary>
        ///  The identity of the object to add.
        /// </summary>
        [Indirect()]
        public DSNAME? pObject;

        /// <summary>
        ///  The attributes of the object to add.
        /// </summary>
        public ATTRBLOCK AttrBlock;
    }

    /// <summary>
    ///  The REPLENTINFLIST structure defines a concrete type
    ///  for updates to one or more attributes of a given object.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\c38b0412-cf00-4b0c-b4f4-4662a4484a00.xml
    //  </remarks>
    public partial struct REPLENTINFLIST
    {

        /// <summary>
        ///  The next REPLENTINFLIST in the sequence, or null.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public REPLENTINFLIST[] pNextEntInf;

        /// <summary>
        ///  The object and its updatedattributes.
        /// </summary>
        public ENTINF Entinf;

        /// <summary>
        ///  TRUE only if the object is an NC root.
        /// </summary>
        [CLSCompliant(false)]
        public int fIsNCPrefix;

        /// <summary>
        ///  The value of the objectGUIDattribute of the parent of
        ///  the object, or null if not known or not specified.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public Guid[] pParentGuid;

        /// <summary>
        ///  The stamps for the attributes specified in Entinf.AttrBlock.
        ///  Entinf.AttrBlock and pMetaDataExt.rgMetaData are parallel
        ///  arrays. For a given integer i in [0 .. Entinf.AttrBlock.attrCount],
        ///  the stamp for the attribute described by Entinf.AttrBlock.pAttr^[i]
        ///  is pMetaDataExt^.rgMetaData[i].
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public PROPERTY_META_DATA_EXT_VECTOR[] pMetaDataExt;
    }

    /// <summary>
    ///  The DRS_MSG_VERIFYREPLY_V1 structure defines a response
    ///  message received from the IDL_DRSVerifyNames method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\d675aba6-c952-4e10-8869-3243408756f4.xml
    //  </remarks>
    public partial struct DRS_MSG_VERIFYREPLY_V1
    {

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public error_Values error;

        /// <summary>
        ///  The number of items in the rpEntInf array.
        /// </summary>
        [PossibleValueRange("0", "10000")]
        [CLSCompliant(false)]
        public uint cNames;

        /// <summary>
        ///  An array of ENTINF structures that contain the attributes
        ///  requested in the RequestAttrs field of the input DRS_MSG_VERIFYREQ_V1
        ///  structure if the corresponding name is verified.
        /// </summary>
        [Size("cNames")]
        public ENTINF[] rpEntInf;

        /// <summary>
        ///  The prefix table used to translate ATTRTYP values in
        ///  the response to OIDs.
        /// </summary>
        public SCHEMA_PREFIX_TABLE PrefixTable;
    }

    /// <summary>
    ///  The error values of DRS_MSG_MOVEREQ_V1.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum error_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The DRS_MSG_MOVEREQ_V2 structure defines a request message
    ///  sent to the IDL_DRSInterDomainMove method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\d77d5670-4a35-49d2-b6c8-e314a7f76ff3.xml
    //  </remarks>
    public partial struct DRS_MSG_MOVEREQ_V2
    {

        /// <summary>
        ///  The client DCnTDSDSAobject.
        /// </summary>
        [Indirect()]
        public DSNAME? pSrcDSA;

        /// <summary>
        ///  The object to be moved.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ENTINF[] pSrcObject;

        /// <summary>
        ///  The name the object will have in the destination domain.
        /// </summary>
        [Indirect()]
        public DSNAME? pDstName;

        /// <summary>
        ///  The NC to which pSrcObject is being moved.
        /// </summary>
        [Indirect()]
        public DSNAME? pExpectedTargetNC;

        /// <summary>
        ///  The credentials of the user initiating the call.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public DRS_SecBufferDesc[] pClientCreds;

        /// <summary>
        ///  The prefix table with which to translate the ATTRTYP
        ///  values in pSrcObject to OIDs.
        /// </summary>
        public SCHEMA_PREFIX_TABLE PrefixTable;

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public ulFlags_Values ulFlags;
    }

    /// <summary>
    ///  The ulFlags values of DRS_MSG_MOVEREQ_V2.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum ulFlags_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The DRS_MSG_MOVEREQ_V1 structure defines a request message
    ///  sent to the IDL_DRSInterDomainMove method. This request
    ///  version is obsolete.Although this request version appears
    ///  in the IDL, windowsDCs do not support it. It was never
    ///  supported in a released version of windows_server.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\ea758196-1fbe-48d6-91e7-f1a3105fff9a.xml
    //  </remarks>
    public partial struct DRS_MSG_MOVEREQ_V1
    {

        /// <summary>
        ///  The NetworkAddress of the client DC.
        /// </summary>
        [String(StringEncoding.ASCII)]
        public string pSourceDSA;

        /// <summary>
        ///  The object to be moved.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ENTINF[] pObject;

        /// <summary>
        ///  The objectGUID of the new parent object.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public Guid[] pParentUUID;

        /// <summary>
        ///  The prefix table with which to translate the ATTRTYP
        ///  values in pObject to OIDs.
        /// </summary>
        public SCHEMA_PREFIX_TABLE PrefixTable;

        /// <summary>
        ///  Unused. MUST be 0 and ignored.
        /// </summary>
        [CLSCompliant(false)]
        public DRS_MSG_MOVEREQ_V1_ulFlags_Values ulFlags;
    }

    /// <summary>
    ///  The ulFlags values of DRS_MSG_MOVEREQ_V1.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum DRS_MSG_MOVEREQ_V1_ulFlags_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The DRS_MSG_GETCHGREPLY_V6 structure defines the response
    ///  message received from the IDL_DRSGetNCChanges method.
    ///  This message version is a superset of DRS_MSG_GETCHGREPLY_V1.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\1317a654-5dd6-45ff-af73-919cbc7fbb45.xml
    //  </remarks>
    public partial struct DRS_MSG_GETCHGREPLY_V6
    {

        /// <summary>
        ///  DSA GUID of the server DC.
        /// </summary>
        public Guid uuidDsaObjSrc;

        /// <summary>
        ///  Invocation ID of the server DC.
        /// </summary>
        public Guid uuidInvocIdSrc;

        /// <summary>
        ///  The NC root of the replica to replicate or the FSMO
        ///  role object for an extended operation.
        /// </summary>
        [Indirect()]
        public DSNAME? pNC;

        /// <summary>
        ///  Data used to correlate calls to IDL_DRSGetNCChanges.
        /// </summary>
        public USN_VECTOR usnvecFrom;

        /// <summary>
        ///  Data used to correlate calls to IDL_DRSGetNCChanges.
        /// </summary>
        public USN_VECTOR usnvecTo;

        /// <summary>
        ///  Stamp filter that describes updates the server has already
        ///  applied.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public UPTODATE_VECTOR_V2_EXT[] pUpToDateVecSrc;

        /// <summary>
        ///  Table for translating ATTRTYP values in the response
        ///  to OIDs.
        /// </summary>
        public SCHEMA_PREFIX_TABLE PrefixTableSrc;

        /// <summary>
        ///  0 or an extended operation error code (section ).
        /// </summary>
        [CLSCompliant(false)]
        public uint ulExtendedRet;

        /// <summary>
        ///  Count of items in the pObjects linked list.
        /// </summary>
        [CLSCompliant(false)]
        public uint cNumObjects;

        /// <summary>
        ///  Size in bytes of items in or referenced by elements
        ///  in the pObjects linked list.
        /// </summary>
        [CLSCompliant(false)]
        public uint cNumBytes;

        /// <summary>
        ///  Linked list of objectupdates that the client applies
        ///  to its NC replica.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public REPLENTINFLIST[] pObjects;

        /// <summary>
        ///  False if and only if this is the last response in a
        ///  replication cycle.
        /// </summary>
        [CLSCompliant(false)]
        public int fMoreData;

        /// <summary>
        ///  Estimated number of objects in the server's NC replica.
        /// </summary>
        [CLSCompliant(false)]
        public uint cNumNcSizeObjects;

        /// <summary>
        ///  Estimated number of link values in the server's NC replica.
        /// </summary>
        [CLSCompliant(false)]
        public uint cNumNcSizeValues;

        /// <summary>
        ///  Count of items in the rgValues array.
        /// </summary>
        [CLSCompliant(false)]
        [PossibleValueRange("0", "1048576")]
        public uint cNumValues;

        /// <summary>
        ///  Link valueupdates for the client to apply to its NC
        ///  replica.
        /// </summary>
        [Size("cNumValues")]
        public REPLVALINF_V1[] rgValues;

        /// <summary>
        ///  0 if successful, otherwise a Windows error code.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwDRSError;
    }

    public partial struct DRS_MSG_GETCHGREPLY_V9
    {

        /// <summary>
        ///  DSA GUID of the server DC.
        /// </summary>
        public Guid uuidDsaObjSrc;

        /// <summary>
        ///  Invocation ID of the server DC.
        /// </summary>
        public Guid uuidInvocIdSrc;

        /// <summary>
        ///  The NC root of the replica to replicate or the FSMO
        ///  role object for an extended operation.
        /// </summary>
        [Indirect()]
        public DSNAME? pNC;

        /// <summary>
        ///  Data used to correlate calls to IDL_DRSGetNCChanges.
        /// </summary>
        public USN_VECTOR usnvecFrom;

        /// <summary>
        ///  Data used to correlate calls to IDL_DRSGetNCChanges.
        /// </summary>
        public USN_VECTOR usnvecTo;

        /// <summary>
        ///  Stamp filter that describes updates the server has already
        ///  applied.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public UPTODATE_VECTOR_V2_EXT[] pUpToDateVecSrc;

        /// <summary>
        ///  Table for translating ATTRTYP values in the response
        ///  to OIDs.
        /// </summary>
        public SCHEMA_PREFIX_TABLE PrefixTableSrc;

        /// <summary>
        ///  0 or an extended operation error code (section ).
        /// </summary>
        [CLSCompliant(false)]
        public uint ulExtendedRet;

        /// <summary>
        ///  Count of items in the pObjects linked list.
        /// </summary>
        [CLSCompliant(false)]
        public uint cNumObjects;

        /// <summary>
        ///  Size in bytes of items in or referenced by elements
        ///  in the pObjects linked list.
        /// </summary>
        [CLSCompliant(false)]
        public uint cNumBytes;

        /// <summary>
        ///  Linked list of objectupdates that the client applies
        ///  to its NC replica.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public REPLENTINFLIST[] pObjects;

        /// <summary>
        ///  False if and only if this is the last response in a
        ///  replication cycle.
        /// </summary>
        [CLSCompliant(false)]
        public int fMoreData;

        /// <summary>
        ///  Estimated number of objects in the server's NC replica.
        /// </summary>
        [CLSCompliant(false)]
        public uint cNumNcSizeObjects;

        /// <summary>
        ///  Estimated number of link values in the server's NC replica.
        /// </summary>
        [CLSCompliant(false)]
        public uint cNumNcSizeValues;

        /// <summary>
        ///  Count of items in the rgValues array.
        /// </summary>
        [CLSCompliant(false)]
        public uint cNumValues;

        /// <summary>
        ///  Link valueupdates for the client to apply to its NC
        ///  replica.
        /// </summary>
        [Size("cNumValues")]
        public REPLVALINF_V3[] rgValues;

        /// <summary>
        ///  0 if successful, otherwise a Windows error code.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwDRSError;
    }

    /// <summary>
    ///  The DRS_MSG_MOVEREQ union defines the request messages
    ///  sent to the IDL_DRSInterDomainMove method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\221c9cf9-0d0e-4ed5-8d51-80c8b3254a36.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_MOVEREQ
    {

        /// <summary>
        ///  The version 1 request (obsolete).
        /// </summary>
        [Case("1")]
        public DRS_MSG_MOVEREQ_V1 V1;

        /// <summary>
        ///  The version 2 request.
        /// </summary>
        [Case("2")]
        public DRS_MSG_MOVEREQ_V2 V2;
    }

    /// <summary>
    ///  The DRS_MSG_VERIFYREPLY union defines the response messages
    ///  received from the IDL_DRSVerifyNames method. Only one
    ///  version, identified by dwVersion = 1, is currently
    ///  defined.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\3eb00260-fe1d-40ad-99a2-3a19adb23b0a.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_VERIFYREPLY
    {

        /// <summary>
        ///  The version 1 reply.
        /// </summary>
        [Case("1")]
        public DRS_MSG_VERIFYREPLY_V1 V1;
    }

    /// <summary>
    ///  ENTINFLIST is a concrete type for a list of ENTINFentries.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\59cad1ff-e499-477c-8c9e-59939a71aff5.xml
    //  </remarks>
    public partial struct ENTINFLIST
    {

        /// <summary>
        ///  The next ENTINFLIST in the sequence, or null.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ENTINFLIST[] pNextEntInf;

        /// <summary>
        ///  An ENTINFentry.
        /// </summary>
        public ENTINF Entinf;
    }

    /// <summary>
    ///  The DRS_MSG_GETCHGREQ_V7 structure defines the request
    ///  message sent to the IDL_DRSGetNCChanges method. This
    ///  message version is a superset of DRS_MSG_GETCHGREQ_V4.Though
    ///  this request version appears in the IDL, windowsDCs
    ///  never send this request version using RPC. It exists
    ///  solely to support SMTP replication ([MS-SRPL]).
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\5ef4f597-a397-4f6f-a98b-7a034247d886.xml
    //  </remarks>
    public partial struct DRS_MSG_GETCHGREQ_V7
    {

        /// <summary>
        ///  The objectGUID of the interSiteTransportobject that
        ///  identifies the transport by which to send the reply.
        /// </summary>
        public Guid uuidTransportObj;

        /// <summary>
        ///  Transport-specific address to which to send the reply.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public MTX_ADDR[] pmtxReturnAddress;

        /// <summary>
        ///  Version 3 request.
        /// </summary>
        public DRS_MSG_GETCHGREQ_V3 V3;

        /// <summary>
        ///  A set of one or more attributes whose values are to
        ///  be replicated to the client's partial replica, or null
        ///  if the client has a full replica.
        /// </summary>
        [Indirect()]
        public PARTIAL_ATTR_VECTOR_V1_EXT? pPartialAttrSet;

        /// <summary>
        ///  A set of one or more attributes whose values are to
        ///  be added to the client's existing partial replica,
        ///  or null.
        /// </summary>
        [Indirect()]
        public PARTIAL_ATTR_VECTOR_V1_EXT? pPartialAttrSetEx;

        /// <summary>
        ///  Prefix table with which to convert the ATTRTYP values
        ///  in pPartialAttrSet and pPartialAttrSetEx to OIDs.
        /// </summary>
        public SCHEMA_PREFIX_TABLE PrefixTableDest;
    }

    /// <summary>
    ///  The DRS_MSG_MOVEREPLY union defines the response messages
    ///  received from the IDL_DRSInterDomainMove method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\63149a88-730f-4cb8-aa92-919d1ff5f658.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_MOVEREPLY
    {

        /// <summary>
        ///  The version 1 response (obsolete).
        /// </summary>
        [Case("1")]
        public DRS_MSG_MOVEREPLY_V1 V1;

        /// <summary>
        ///  The version 2 response.
        /// </summary>
        [Case("2")]
        public DRS_MSG_MOVEREPLY_V2 V2;
    }

    /// <summary>
    ///  The DIRERR_DRS_WIRE_V1 union defines the error that
    ///  occurred during processing of a request sent to the
    ///  IDL_DRSAddEntry method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\74534b5f-f066-4a7f-ba39-caa941088bed.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DIRERR_DRS_WIRE_V1
    {

        /// <summary>
        ///  Attribute errors.
        /// </summary>
        [Case("1")]
        public ATRERR_DRS_WIRE_V1 AtrErr;

        /// <summary>
        ///  Name resolution error.
        /// </summary>
        [Case("2")]
        public NAMERR_DRS_WIRE_V1 NamErr;

        /// <summary>
        ///  Referral.
        /// </summary>
        [Case("3")]
        public REFERR_DRS_WIRE_V1 RefErr;

        /// <summary>
        ///  Security error.
        /// </summary>
        [Case("4")]
        public SECERR_DRS_WIRE_V1 SecErr;

        /// <summary>
        ///  Service error.
        /// </summary>
        [Case("5")]
        public SVCERR_DRS_WIRE_V1 SvcErr;

        /// <summary>
        ///  Update error.
        /// </summary>
        [Case("6")]
        public UPDERR_DRS_WIRE_V1 UpdErr;

        /// <summary>
        ///  System error.
        /// </summary>
        [Case("7")]
        public SYSERR_DRS_WIRE_V1 SysErr;
    }

    /// <summary>
    ///  The DRS_MSG_ADDENTRYREQ_V2 structure defines the request
    ///  message sent to the IDL_DRSAddEntry method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\895157d5-24e2-4eaf-9060-148d67669e27.xml
    //  </remarks>
    public partial struct DRS_MSG_ADDENTRYREQ_V2
    {

        /// <summary>
        ///  The objects to be added.
        /// </summary>
        public ENTINFLIST EntInfList;
    }

    /// <summary>
    /// Used to pass byte buffers to certain messages
    /// </summary>
    public partial struct VAR_SIZE_BUFFER_WITH_VERSION
    {
        /// <summary>
        /// The version of the buffer that is being sent.
        /// Handling of this field is performed by the specific message that is using this structure.
        /// </summary>
        [StaticSize(4)]
        uint ulVersion;

        /// <summary>
        /// The size, in bytes, of the data in the rgbBuffer field.
        /// </summary>
        [StaticSize(4)]
        uint cbByteBuffer;

        /// <summary>
        /// Used to align the array of bytes in the rgbBuffer field to an 8-byte boundary.
        /// </summary>
        [StaticSize(8)]
        ulong ullPadding;

        /// <summary>
        /// An array of bytes.
        /// The content of the array depends on the specific message that is using this structure.
        /// Starts on an 8-byte boundary.
        /// </summary>
        [Size("cbByteBuffer")]
        byte[] rgbBuffer;
    }

    /// <summary>
    ///  The DRS_MSG_GETCHGREQ union defines request messages
    ///  that are sent to the IDL_DRSGetNCChanges method. There
    ///  are no V1, V2, V3, or V6V6, or V9 messages.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\96affbe1-7d93-453e-ac75-9f41c0c94b3b.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_GETCHGREQ
    {

        /// <summary>
        ///  Version 4 request (windows_2000 SMTP replication[MS-SRPL]).
        /// </summary>
        [Case("4")]
        public DRS_MSG_GETCHGREQ_V4 V4;

        /// <summary>
        ///  Version 5 request (windows_2000RPCreplication).
        /// </summary>
        [Case("5")]
        public DRS_MSG_GETCHGREQ_V5 V5;

        /// <summary>
        ///  Version 7 request (windows_server_2003 SMTP replication[MS-SRPL]).
        /// </summary>
        [Case("7")]
        public DRS_MSG_GETCHGREQ_V7 V7;

        /// <summary>
        ///  Version 8 request (windows_server_2003RPCreplication).
        /// </summary>
        [Case("8")]
        public DRS_MSG_GETCHGREQ_V8 V8;

        /// <summary>
        ///  Version 10 request (windows_server_2008R2RPCreplication).
        /// </summary>
        [Case("10")]
        public DRS_MSG_GETCHGREQ_V10 V10;

        /// <summary>
        /// Version 11 request (Windows Server v1803 operating system RPC replication).
        /// </summary>
        [Case("11")]
        public DRS_MSG_GETCHGREQ_V11 V11;
    }

    /// <summary>
    ///  The DRS_MSG_GETCHGREPLY_V1 structure defines the response
    ///  message received from the IDL_DRSGetNCChanges method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\bd70a9c3-c1d3-48cf-9c24-503a5567d09c.xml
    //  </remarks>
    public partial struct DRS_MSG_GETCHGREPLY_V1
    {

        /// <summary>
        ///  DSA GUID of the server DC.
        /// </summary>
        public Guid uuidDsaObjSrc;

        /// <summary>
        ///  Invocation ID of the server DC.
        /// </summary>
        public Guid uuidInvocIdSrc;

        /// <summary>
        ///  The NC root of the replica to replicate or the FSMO
        ///  role object for an extended operation.
        /// </summary>
        [Indirect()]
        public DSNAME? pNC;

        /// <summary>
        ///  Data used to correlate calls to IDL_DRSGetNCChanges.
        /// </summary>
        public USN_VECTOR usnvecFrom;

        /// <summary>
        ///  Data used to correlate calls to IDL_DRSGetNCChanges.
        /// </summary>
        public USN_VECTOR usnvecTo;

        /// <summary>
        ///  Stamp filter that describes updates the server has already
        ///  applied.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public UPTODATE_VECTOR_V1_EXT[] pUpToDateVecSrcV1;

        /// <summary>
        ///  Table for translating ATTRTYP values in the response
        ///  to OIDs.
        /// </summary>
        public SCHEMA_PREFIX_TABLE PrefixTableSrc;

        /// <summary>
        ///  0 or an EXOP_ERR code (section ).
        /// </summary>
        [CLSCompliant(false)]
        public uint ulExtendedRet;

        /// <summary>
        ///  Count of items in the pObjects linked list.
        /// </summary>
        [CLSCompliant(false)]
        public uint cNumObjects;

        /// <summary>
        ///  Size in bytes of items in or referenced by elements
        ///  in the pObjects linked list.
        /// </summary>
        [CLSCompliant(false)]
        public uint cNumBytes;

        /// <summary>
        ///  Linked list of objectupdates that the client applies
        ///  to its NC replica.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public REPLENTINFLIST[] pObjects;

        /// <summary>
        ///  False if and only if this is the last response in a
        ///  replication cycle.
        /// </summary>
        [CLSCompliant(false)]
        public int fMoreData;
    }

    /// <summary>
    ///  The DRS_ERROR_DATA_V1 structure defines the error response
    ///  received from the IDL_DRSAddEntry method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\cde11e7e-4d01-479c-8d25-400db549804e.xml
    //  </remarks>
    public partial struct DRS_ERROR_DATA_V1
    {

        /// <summary>
        ///  0 or a Windows error code.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwRepError;

        /// <summary>
        ///  A DIRERR code (section) that specifies the error category.
        /// </summary>
        [CLSCompliant(false)]
        public uint errCode;

        /// <summary>
        ///  Category-specific error information.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        [Switch("errCode")]
        public DIRERR_DRS_WIRE_V1[] pErrInfo;
    }

    /// <summary>
    ///  The DRS_MSG_ADDENTRYREQ_V3 structure defines the request
    ///  message sent to the IDL_DRSAddEntry method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\d04ae3cc-4c31-4c9e-bb24-5f718a6ee646.xml
    //  </remarks>
    public partial struct DRS_MSG_ADDENTRYREQ_V3
    {

        /// <summary>
        ///  The objects to be added.
        /// </summary>
        public ENTINFLIST EntInfList;

        /// <summary>
        ///  The user credentials to authorize the operation.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public DRS_SecBufferDesc[] pClientCreds;
    }

    /// <summary>
    ///  The DRS_ERROR_DATA union defines the error responses
    ///  that are received from the IDL_DRSAddEntry method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\5d98d99b-872e-4cff-9758-36b1792fd55e.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_ERROR_DATA
    {

        /// <summary>
        ///  Version 1 response.
        /// </summary>
        [Case("1")]
        public DRS_ERROR_DATA_V1 V1;
    }

    /// <summary>
    ///  The DRS_MSG_ADDENTRYREQ union defines the request messages
    ///  that are sent to the IDL_DRSAddEntry method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\6104d1c7-a2a6-47a8-b7a7-2ea655574df0.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_ADDENTRYREQ
    {

        /// <summary>
        ///  Version 1 request (obsolete).
        /// </summary>
        [Case("1")]
        public DRS_MSG_ADDENTRYREQ_V1 V1;

        /// <summary>
        ///  Version 2 request.
        /// </summary>
        [Case("2")]
        public DRS_MSG_ADDENTRYREQ_V2 V2;

        /// <summary>
        ///  Version 3 request.
        /// </summary>
        [Case("3")]
        public DRS_MSG_ADDENTRYREQ_V3 V3;
    }

    /// <summary>
    ///  The DRS_MSG_GETCHGREPLY union defines the response messages
    ///  received from the IDL_DRSGetNCChanges method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\65a5cb42-c25f-4378-b06e-f87341b21f93.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_GETCHGREPLY
    {

        /// <summary>
        ///  Version 1 response (windows_2000).
        /// </summary>
        [Case("1")]
        public DRS_MSG_GETCHGREPLY_V1 V1;

        /// <summary>
        ///  Version 2 response (compressed V1).
        /// </summary>
        [Case("2")]
        public DRS_MSG_GETCHGREPLY_V2 V2;

        /// <summary>
        ///  Version 6 response (windows_server_2003).
        /// </summary>
        [Case("6")]
        public DRS_MSG_GETCHGREPLY_V6 V6;

        /// <summary>
        ///  Version 7 response (compressed V6).
        /// </summary>
        [Case("7")]
        public DRS_MSG_GETCHGREPLY_V7 V7;

        [Case("9")]
        public DRS_MSG_GETCHGREPLY_V9 V9;
    }


    /// <summary>
    ///  The DRS_MSG_ADDENTRYREPLY_V3 structure defines the response
    ///  message received from the IDL_DRSAddEntry method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\1eeb493e-93f1-424c-b8eb-ca74e6f051a0.xml
    //  </remarks>
    public partial struct DRS_MSG_ADDENTRYREPLY_V3
    {

        /// <summary>
        ///  Null, or the identity of the object that was being added
        ///  when an error occurred.
        /// </summary>
        [Indirect()]
        public DSNAME? pdsErrObject;

        /// <summary>
        ///  MUST be set to 1.
        /// </summary>
        [CLSCompliant(false)]
        public dwErrVer_Values dwErrVer;

        /// <summary>
        ///  Null, or an error that occurred while processing the
        ///  request.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        [Switch("dwErrVer")]
        public DRS_ERROR_DATA[] pErrData;

        /// <summary>
        ///  The count of items in the infoList array.
        /// </summary>
        [PossibleValueRange("0", "10000")]
        [CLSCompliant(false)]
        public uint cObjectsAdded;

        /// <summary>
        ///  The identities of the added objects. The item order
        ///  matches the item order of values in the EntInfList
        ///  field in the request structure.
        /// </summary>
        [Size("cObjectsAdded")]
        public ADDENTRY_REPLY_INFO[] infoList;
    }

    /// <summary>
    ///  The dwErrVer values of DRS_MSG_ADDENTRYREPLY_V3.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [CLSCompliant(false)]
    public enum dwErrVer_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 1,
    }

    /// <summary>
    ///  The DRS_MSG_ADDENTRYREPLY union defines the response
    ///  messages received from the IDL_DRSAddEntry method.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-DRSR\33b6a012-3f08-4d09-8c06-65581b1ff1ed.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct DRS_MSG_ADDENTRYREPLY
    {

        /// <summary>
        ///  Version 1 response (obsolete).
        /// </summary>
        [Case("1")]
        public DRS_MSG_ADDENTRYREPLY_V1 V1;

        /// <summary>
        ///  Version 2 response.
        /// </summary>
        [Case("2")]
        public DRS_MSG_ADDENTRYREPLY_V2 V2;

        /// <summary>
        ///  Version 3 response.
        /// </summary>
        [Case("3")]
        public DRS_MSG_ADDENTRYREPLY_V3 V3;
    }

    /// <summary>
    /// The inVersion of DRS_MSG_GETREPLINFO_REPLY.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum dwInVersion_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 1,
    }

    /// <summary>
    /// The outVersion of DRS_MSG_GETREPLINFO_REPLY.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum pdwOutVersion_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 1,
    }

    /// <summary>
    /// The inVersion of DRSRemoveDsServer.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum IDL_DRSRemoveDsServer_dwInVersion_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 1,
    }

    /// <summary>
    /// The outVersion of DRSRemoveDsServer.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum IDL_DRSRemoveDsServer_pdwOutVersion_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 1,
    }

    /// <summary>
    /// The inVersion of DRSRemoveDsDomain.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum IDL_DRSRemoveDsDomain_dwInVersion_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 1,
    }

    /// <summary>
    /// The outVersion of DRSRemoveDsDomain.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum IDL_DRSRemoveDsDomain_pdwOutVersion_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 1,
    }

    /// <summary>
    /// The inVersion of DRSAddSidHistory.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum IDL_DRSAddSidHistory_dwInVersion_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 1,
    }

    /// <summary>
    /// The outVersion of DRSAddSidHistory.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum IDL_DRSAddSidHistory_pdwOutVersion_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 1,
    }


    /// <summary>
    /// The DRS_MSG_ADDCLONEDCREQ_V1 structure defines a request message
    /// sent to the IDL_DRSAddCloneDC method.
    /// </summary>
    public partial struct DRS_MSG_ADDCLONEDCREQ_V1
    {
        /// <summary>
        /// The new DC name
        /// </summary>
        [String()]
        public string pwszCloneDCName;

        /// <summary>
        /// The RDN of the site the new DC will be placed into.
        /// </summary>
        [String()]
        public string pwszSite;
    }


    /// <summary>
    /// The DRS_MSG_ADDCLONEDCREQ structure defines a request message
    /// sent to the IDL_DRSAddCloneDC method.
    /// </summary>
    [Union("System.Int32")]
    public partial struct DRS_MSG_ADDCLONEDCREQ
    {
        /// <summary>
        /// The version 1 request
        /// </summary>
        [Case("1")]
        public DRS_MSG_ADDCLONEDCREQ_V1 V1;
    }

    /// <summary>
    /// The DRS_MSG_ADDCLONEDCREPLY_V1 structure defines a response message
    /// received from the IDL_DRSAddCloneDC method.
    /// </summary>
    public partial struct DRS_MSG_ADDCLONEDCREPLY_V1
    {
        /// <summary>
        /// The new DC's name
        /// </summary>
        [String()]
        public string pwszCloneDCName;

        /// <summary>
        /// The site containing the new DC
        /// </summary>
        [String()]
        public string pwszSite;

        /// <summary>
        /// The length of the pwsNewDCAccountPassword member
        /// </summary>
        [PossibleValueRange("0", "1024")]
        [CLSCompliant(false)]
        public uint cPasswordLength;

        /// <summary>
        /// The password of the new DC account
        /// </summary>
        [Size("cPasswordLength")]
        [String()]
        public string pwsNewDCAccountPassword;
    }

    /// <summary>
    /// The DRS_MSG_ADDCLONEDCREPLY structure defines a response message
    /// received from the IDL_DRSAddCloneDC method.
    /// </summary>
    [Union("System.Int32")]
    public partial struct DRS_MSG_ADDCLONEDCREPLY
    {
        /// <summary>
        /// The version 1 response
        /// </summary>
        [Case("1")]
        public DRS_MSG_ADDCLONEDCREPLY_V1 V1;
    }

    public partial struct RecordTopLevelName
    {
        [String()]
        public string TopLevelName;
    }

    public partial struct RecordDomainInfo
    {
        public NT4SID Sid;
        [String()]
        public string DnsName;

        [String()]
        public string NetbiosName;
    }

    /// <summary>
    ///  The DRS_MSG_WRITENGCKEYREQ union defines the request messages
    ///  sent to the IDL_DRSWriteNgcKey method. Only one version,
    ///  identified by dwInVersion = 1, is currently defined.
    /// </summary>
    [Union("System.Int32")]
    public partial struct DRS_MSG_WRITENGCKEYREQ
    {

        /// <summary>
        ///  The version 1 response.
        /// </summary>
        [Case("1")]
        public DRS_MSG_WRITENGCKEYREQ_V1 V1;
    }

    /// <summary>
    ///  The DRS_MSG_WRITENGCKEYREQ_V1 structure defines a request
    ///  message sent to the IDL_DRSWriteNgcKey method.
    /// </summary>
    public partial struct DRS_MSG_WRITENGCKEYREQ_V1
    {

        /// <summary>
        ///  The DN of the object to modify.
        /// </summary>
        [String()]
        public string pwszAccount;

        /// <summary>
        ///  The number of bytes in the pNgcKey array.
        /// </summary>
        [PossibleValueRange("0", "0xFFFF")]
        [CLSCompliant(false)]
        public uint cNgcKey;

        /// <summary>
        ///  The NGC key value.
        /// </summary>
        [Size("cNgcKey")]
        public byte[] pNgcKey;
    }

    /// <summary>
    ///  The DRS_MSG_WRITENGCKEYREPLY union defines the response messages
    ///  received from the IDL_DRSWriteNgcKey method. Only one version,
    ///  identified by pdwOutVersion^ = 1, is currently defined.
    /// </summary>
    [Union("System.Int32")]
    public partial struct DRS_MSG_WRITENGCKEYREPLY
    {

        /// <summary>
        ///  The version 1 response.
        /// </summary>
        [Case("1")]
        public DRS_MSG_WRITENGCKEYREPLY_V1 V1;
    }

    /// <summary>
    ///  The DRS_MSG_WRITENGCKEYREPLY_V1 structure defines a response
    ///  message received from the IDL_DRSWriteNgcKey method.
    /// </summary>
    public partial struct DRS_MSG_WRITENGCKEYREPLY_V1
    {

        /// <summary>
        ///  0, or a Windows error code.
        /// </summary>
        [CLSCompliant(false)]
        public uint retVal;
    }

    /// <summary>
    ///  The DRS_MSG_READNGCKEYREQ union defines the request messages
    ///  sent to the IDL_DRSReadNgcKey method. Only one version,
    ///  identified by dwInVersion = 1, is currently defined.
    /// </summary>
    [Union("System.Int32")]
    public partial struct DRS_MSG_READNGCKEYREQ
    {

        /// <summary>
        ///  The version 1 response.
        /// </summary>
        [Case("1")]
        public DRS_MSG_READNGCKEYREQ_V1 V1;
    }

    /// <summary>
    ///  The DRS_MSG_READNGCKEYREQ_V1 structure defines a request
    ///  message sent to the IDL_DRSReadNgcKey method.
    /// </summary>
    public partial struct DRS_MSG_READNGCKEYREQ_V1
    {

        /// <summary>
        ///  The DN of the object to modify.
        /// </summary>
        [String()]
        public string pwszAccount;
    }

    /// <summary>
    ///  The DRS_MSG_READNGCKEYREPLY union defines the response messages
    ///  received from the IDL_DRSReadNgcKey method. Only one version,
    ///  identified by pdwOutVersion^ = 1, is currently defined.
    /// </summary>
    [Union("System.Int32")]
    public partial struct DRS_MSG_READNGCKEYREPLY
    {

        /// <summary>
        ///  The version 1 response.
        /// </summary>
        [Case("1")]
        public DRS_MSG_READNGCKEYREPLY_V1 V1;
    }

    /// <summary>
    ///  The DRS_MSG_READNGCKEYREPLY_V1 structure defines a response
    ///  message received from the IDL_DRSReadNgcKey method.
    /// </summary>
    public partial struct DRS_MSG_READNGCKEYREPLY_V1
    {

        /// <summary>
        ///  0, or a Windows error code.
        /// </summary>
        [CLSCompliant(false)]
        public uint retVal;

        /// <summary>
        ///  The number of bytes in the pNgcKey array.
        /// </summary>
        [PossibleValueRange("0", "0xFFFF")]
        [CLSCompliant(false)]
        public uint cNgcKey;

        /// <summary>
        ///  The NGC key value.
        /// </summary>
        [Size("cNgcKey")]
        public byte[] pNgcKey;
    }

}
