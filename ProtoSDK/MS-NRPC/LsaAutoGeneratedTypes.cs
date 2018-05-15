// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;


namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa
{
    /// <summary>
    ///  The SID_NAME_USE enumeration contains values that specify
    ///  the type of an account.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAT\171e9a87-8e01-4bd8-a35e-3468128c8fc4.xml
    //  </remarks>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum _SID_NAME_USE
    {

        /// <summary>
        ///  SidTypeUser constant.
        /// </summary>
        SidTypeUser = 1,

        /// <summary>
        ///  SidTypeGroup constant.
        /// </summary>
        SidTypeGroup,

        /// <summary>
        ///  SidTypeDomain constant.
        /// </summary>
        SidTypeDomain,

        /// <summary>
        ///  SidTypeAlias constant.
        /// </summary>
        SidTypeAlias,

        /// <summary>
        ///  SidTypeWellKnownGroup constant.
        /// </summary>
        SidTypeWellKnownGroup,

        /// <summary>
        ///  SidTypeDeletedAccount constant.
        /// </summary>
        SidTypeDeletedAccount,

        /// <summary>
        ///  SidTypeInvalid constant.
        /// </summary>
        SidTypeInvalid,

        /// <summary>
        ///  SidTypeUnknown constant.
        /// </summary>
        SidTypeUnknown,

        /// <summary>
        ///  SidTypeComputer constant.
        /// </summary>
        SidTypeComputer,

        /// <summary>
        ///  SidTypeLabel constant.
        /// </summary>
        SidTypeLabel,
    }

    /// <summary>
    ///  The SECURITY_IMPERSONATION_LEVEL enumeration defines
    ///  a set of values that specify security impersonation
    ///  levels as specified in [MS-LSAD] section 2.2.3.5.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAT\4797d0a1-7b71-4a22-a5a0-8e2059aec239.xml
    //  </remarks>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum _SECURITY_IMPERSONATION_LEVEL
    {

        /// <summary>
        ///  SecurityAnonymous constant.
        /// </summary>
        SecurityAnonymous = 0,

        /// <summary>
        ///  SecurityIdentification constant.
        /// </summary>
        SecurityIdentification = 1,

        /// <summary>
        ///  SecurityImpersonation constant.
        /// </summary>
        SecurityImpersonation = 2,

        /// <summary>
        ///  SecurityDelegation constant.
        /// </summary>
        SecurityDelegation = 3,
    }

    /// <summary>
    ///  The LSAP_LOOKUP_LEVEL enumeration defines different
    ///  scopes for searches during translation.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAT\9d1166cc-bcfd-4e22-a8ac-f55eae57c99f.xml
    //  </remarks>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum _LSAP_LOOKUP_LEVEL
    {

        /// <summary>
        ///  LsapLookupWksta constant.
        /// </summary>
        LsapLookupWksta = 1,

        /// <summary>
        ///  LsapLookupPDC constant.
        /// </summary>
        LsapLookupPDC,

        /// <summary>
        ///  LsapLookupTDL constant.
        /// </summary>
        LsapLookupTDL,

        /// <summary>
        ///  LsapLookupGC constant.
        /// </summary>
        LsapLookupGC,

        /// <summary>
        ///  LsapLookupXForestReferral constant.
        /// </summary>
        LsapLookupXForestReferral,

        /// <summary>
        ///  LsapLookupXForestResolve constant.
        /// </summary>
        LsapLookupXForestResolve,

        /// <summary>
        ///  LsapLookupRODCReferralToFullDC constant.
        /// </summary>
        LsapLookupRODCReferralToFullDC,
    }

    /// <summary>
    ///  The LSAPR_TRANSLATED_SID_EX structure contains information
    ///  about a security principal after it has been translated
    ///  into a SID. This structure MUST always be accompanied
    ///  by an LSAPR_REFERENCED_DOMAIN_LIST structure that contains
    ///  the domain information for the security principal.This
    ///  structure differs from LSA_TRANSLATED_SID only in that
    ///  a new Flags field is added.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAT\274ee4e3-86b5-4450-99e9-f1678b9e96f5.xml
    //  </remarks>
    public partial struct _LSAPR_TRANSLATED_SID_EX
    {

        /// <summary>
        ///  Defines the type of the security principal, as specified
        ///  in section 2.2.13.
        /// </summary>
        public _SID_NAME_USE Use;

        /// <summary>
        ///  Contains the relative identifier of the security principal
        ///  with respect to its domain.
        /// </summary>
        public uint RelativeId;

        /// <summary>
        ///  Contains the index into the corresponding LSAPR_REFERENCED_DOMAIN_LIST
        ///  structure that specifies the domain that the security
        ///  principal is in. A DomainIndex value of -1 MUST be
        ///  used to specify that there are no corresponding domains.
        ///  Other negative values MUST NOT be used.
        /// </summary>
        public int DomainIndex;

        /// <summary>
        ///  Contains bitmapped values that define the properties
        ///  of this translation. The value MUST be the logical
        ///  OR of zero or more of the following flags. These flags
        ///  communicate additional information on how the name
        ///  was resolved.
        /// </summary>
        public Flags_Values Flags;
    }

    /// <summary>
    /// _LSAPR_TRANSLATED_SID_EX flags
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum Flags_Values : uint
    {

        /// <summary>
        ///  The name was not found by matching against the Security
        ///  Principal Name property.
        /// </summary>
        NameNotMatchingSecurityPrincipal = 0x00000001,

        /// <summary>
        ///  The name might be found by traversing a forest trust.
        /// </summary>
        NameTraversingForestTrust = 0x00000002,

        /// <summary>
        ///  The name was found by matching against the last database
        ///  view, as defined in section 3.1.1.1.1.
        /// </summary>
        NameMatchingLastDatabaseView = 0x00000004,
    }

    /// <summary>
    ///  The LSA_TRANSLATED_SID structure contains information
    ///  about a security principal after translation from a
    ///  name to a SID. This structure MUST always be accompanied
    ///  by an LSAPR_REFERENCED_DOMAIN_LIST structure.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAT\d067a76e-fd92-4e36-91e7-8605814414cc.xml
    //  </remarks>
    public partial struct _LSA_TRANSLATED_SID
    {

        /// <summary>
        ///  Defines the type of the security principal, as specified
        ///  in section 2.2.13.
        /// </summary>
        public _SID_NAME_USE Use;

        /// <summary>
        ///  Contains the relative identifier of the security principal
        ///  with respect to its domain.
        /// </summary>
        public uint RelativeId;

        /// <summary>
        ///  Contains the index into an LSAPR_REFERENCED_DOMAIN_LIST
        ///  structure that specifies the domain that the security
        ///  principal is in. A DomainIndex value of -1 MUST be
        ///  used to specify that there are no corresponding domains.
        ///  Other negative values MUST NOT be returned.
        /// </summary>
        public int DomainIndex;
    }

    /// <summary>
    ///  The STRING structure defines an ANSI string along with
    ///  the number of characters in the string, as specified
    ///  in [MS-LSAD] section 2.2.3.1.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAT\d876ef82-25c1-423f-93cb-9d97547f2a4b.xml
    //  </remarks>
    public partial struct _STRING
    {

        /// <summary>
        ///  Length member.
        /// </summary>
        public ushort Length;

        /// <summary>
        ///  MaximumLength member.
        /// </summary>
        public ushort MaximumLength;

        /// <summary>
        ///  Buffer member.
        /// </summary>
        [Length("Length")]
        [Size("MaximumLength")]
        public byte[] Buffer;
    }

    /// <summary>
    ///  The LSAPR_ACL structure defines the header of an access
    ///  control list (ACL) as specified in [MS-LSAD] section
    ///  .
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAT\e1a27353-1eb5-4cab-b88e-43d18558daf2.xml
    //  </remarks>
    public partial struct _LSAPR_ACL
    {

        /// <summary>
        ///  AclRevision member.
        /// </summary>
        public byte AclRevision;

        /// <summary>
        ///  Sbz1 member.
        /// </summary>
        public byte Sbz1;

        /// <summary>
        ///  AclSize member.
        /// </summary>
        public ushort AclSize;

        /// <summary>
        ///  Dummy1 member.
        /// </summary>
        [Inline()]
        [Size("AclSize - 4")]
        public byte[] Dummy1;
    }

    /// <summary>
    ///  The SECURITY_QUALITY_OF_SERVICE structure defines information
    ///  used to support client impersonation as specified in
    ///  [MS-LSAD] section 2.2.3.7.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAT\ee3cadcd-ff91-4f45-b80c-ed820db9c1dd.xml
    //  </remarks>
    public partial struct _SECURITY_QUALITY_OF_SERVICE
    {

        /// <summary>
        ///  Length member.
        /// </summary>
        public uint Length;

        /// <summary>
        ///  ImpersonationLevel member.
        /// </summary>
        public _SECURITY_IMPERSONATION_LEVEL ImpersonationLevel;

        /// <summary>
        ///  ContextTrackingMode member.
        /// </summary>
        public byte ContextTrackingMode;

        /// <summary>
        ///  EffectiveOnly member.
        /// </summary>
        public byte EffectiveOnly;
    }

    /// <summary>
    ///  The LSAPR_TRANSLATED_SIDS_EX structure contains a set
    ///  of translated SIDs.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAT\19250ed6-4470-4325-9f85-adb49db720fd.xml
    //  </remarks>
    public partial struct _LSAPR_TRANSLATED_SIDS_EX
    {

        /// <summary>
        ///  Contains the number of translated SIDs.The windowsRPC
        ///  server and RPC client limit the Entries field of this
        ///  structure to 1,000 (using the range primitive defined
        ///  in [MS-RPCE]) in windows_xp_sp2, windows_server_2003,
        ///  windows_vista, and windows_server_2008, windows_vista,
        ///  windows_server_2008, windows_7, and windows_server_7.
        ///  windows_nt_3_1, windows_nt_3_5, windows_nt_3_51, windows_nt_4_0,
        ///  windows_2000, and windows_xp do not enforce this restriction.
        /// </summary>
        public uint Entries;

        /// <summary>
        ///  Contains a set of structures that contain translated
        ///  SIDs, as specified in section 2.2.23. If the Entries field
        ///  in this structure is not 0, this field MUST be non-NULL.
        ///  If Entries is 0, this field MUST be NULL.
        /// </summary>
        [Size("Entries")]
        public _LSAPR_TRANSLATED_SID_EX[] Sids;
    }

    /// <summary>
    ///  The LSAPR_TRANSLATED_NAME structure contains information
    ///  about a security principal, along with the human-readable
    ///  identifier for that security principal. This structure
    ///  MUST always be accompanied by an LSAPR_REFERENCED_DOMAIN_LIST
    ///  structure that contains the domain information for
    ///  the security principals.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAT\52e1ccc1-b57b-4c02-b35f-bd64913ce99b.xml
    //  </remarks>
    public partial struct _LSAPR_TRANSLATED_NAME
    {

        /// <summary>
        ///  Defines the type of the security principal, as specified
        ///  in section 2.2.13.
        /// </summary>
        public _SID_NAME_USE Use;

        /// <summary>
        ///  Contains the name of the security principal. The RPC_UNICODE_STRING
        ///  structure is defined in [MS-DTYP] section 2.3.6.
        /// </summary>
        public _RPC_UNICODE_STRING Name;

        /// <summary>
        ///  Contains the index into the corresponding LSAPR_REFERENCED_DOMAIN_LIST
        ///  structure that specifies the domain that the security
        ///  principal is in. A DomainIndex value of -1 MUST be
        ///  used to specify that there are no corresponding domains.
        ///  Other negative values MUST NOT be used.
        /// </summary>
        public int DomainIndex;
    }

    /// <summary>
    ///  The LSAPR_TRANSLATED_NAME_EX structure contains information
    ///  about a security principal, along with the human-readable
    ///  identifier for that security principal. This structure
    ///  MUST always be accompanied by an LSAPR_REFERENCED_DOMAIN_LIST
    ///  structure that contains the domain information for
    ///  the security principals.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAT\6a9131ac-f1a7-4b96-8917-a289a2c48de1.xml
    //  </remarks>
    public partial struct _LSAPR_TRANSLATED_NAME_EX
    {

        /// <summary>
        ///  Defines the type of the security principal, as specified
        ///  in section 2.2.13.
        /// </summary>
        public _SID_NAME_USE Use;

        /// <summary>
        ///  Contains the name of the security principal. The RPC_UNICODE_STRING
        ///  structure is defined in [MS-DTYP] section 2.3.6.
        /// </summary>
        public _RPC_UNICODE_STRING Name;

        /// <summary>
        ///  Contains the index into the corresponding LSAPR_REFERENCED_DOMAIN_LIST
        ///  structure that specifies the domain that the security
        ///  principal is in. A DomainIndex value of -1 MUST be
        ///  used to specify that there are no corresponding domains.
        ///  Other negative values MUST NOT be used.
        /// </summary>
        public int DomainIndex;

        /// <summary>
        ///  Contains bitmapped values that define the properties
        ///  of this translation. The value MUST be the logical
        ///  OR of zero or more of the following flags. These flags
        ///  communicate the following additional information about
        ///  how the SID was resolved.
        /// </summary>
        public _LSAPR_TRANSLATED_NAME_EX_Flags_Values Flags;
    }

    /// <summary>
    /// LSAPR_TRANSLATED_NAME_EX flags
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum _LSAPR_TRANSLATED_NAME_EX_Flags_Values : uint
    {

        /// <summary>
        ///  The SID was not found by matching against the security
        ///  principalSID property.
        /// </summary>
        SidNotMatchingSecurityPrincipal = 0x00000001,

        /// <summary>
        ///  The SID might be found by traversing a forest trust.
        /// </summary>
        SidTraversingForestTrust = 0x00000002,

        /// <summary>
        ///  The SID was found by matching against the last database
        ///  view, defined in section 3.1.1.1.1.
        /// </summary>
        SidMatchingLastDatabaseView = 0x00000004,
    }

    /// <summary>
    ///  The LSAPR_SECURITY_DESCRIPTOR structure defines an object's
    ///  security descriptor as specified in [MS-LSAD] section
    ///  .
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAT\8494008f-0bfb-45b8-bb6c-e32dd7f18e3d.xml
    //  </remarks>
    public partial struct _LSAPR_SECURITY_DESCRIPTOR
    {

        /// <summary>
        ///  Revision member.
        /// </summary>
        public byte Revision;

        /// <summary>
        ///  Sbz1 member.
        /// </summary>
        public byte Sbz1;

        /// <summary>
        ///  Control member.
        /// </summary>
        public ushort Control;

        /// <summary>
        ///  Owner member.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _RPC_SID[] Owner;

        /// <summary>
        ///  Group member.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _RPC_SID[] Group;

        /// <summary>
        ///  Sacl member.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _LSAPR_ACL[] Sacl;

        /// <summary>
        ///  Dacl member.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _LSAPR_ACL[] Dacl;
    }

    /// <summary>
    ///  The LSAPR_TRANSLATED_SIDS structure defines a set of
    ///  translated SIDs.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAT\8c3f0345-3cdd-4ccb-ba6f-8aac5e0da896.xml
    //  </remarks>
    public partial struct _LSAPR_TRANSLATED_SIDS
    {

        /// <summary>
        ///  Contains the number of translated SIDs.The windowsRPC
        ///  server and RPC client limit the Entries field of this
        ///  structure to 1,000 (using the range primitive defined
        ///  in [MS-RPCE]) in windows_xp_sp2, windows_server_2003,
        ///  windows_vista, and windows_server_2008, windows_vista,
        ///  windows_server_2008, windows_7, and windows_server_7.
        ///  windows_nt_3_1, windows_nt_3_5, windows_nt_3_51, windows_nt_4_0,
        ///  windows_2000, and windows_xp do not have this restriction.
        /// </summary>
        public uint Entries;

        /// <summary>
        ///  Contains a set of structures that contain translated
        ///  SIDs. If the Entries field in this structure is not
        ///  0, this field MUST be non-NULL. If Entries is 0, this
        ///  field MUST be NULL.
        /// </summary>
        [Size("Entries")]
        public _LSA_TRANSLATED_SID[] Sids;
    }

    /// <summary>
    ///  The LSAPR_TRUST_INFORMATION structure contains information
    ///  about a domain.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAT\b0f34b28-b5da-44aa-a607-99c09e6526e1.xml
    //  </remarks>
    public partial struct _LSAPR_TRUST_INFORMATION
    {

        /// <summary>
        ///  Name member.
        /// </summary>
        public _RPC_UNICODE_STRING Name;

        /// <summary>
        ///  Sid member.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _RPC_SID[] Sid;
    }

    /// <summary>
    ///  The LSAPR_TRANSLATED_SID_EX2 structure contains information
    ///  about a security principal after it has been translated
    ///  into a SID. This structure MUST always be accompanied
    ///  by an LSAPR_REFERENCED_DOMAIN_LIST structure.This structure
    ///  differs from LSAPR_TRANSLATED_SID_EX only in that a
    ///  SID is returned instead of a RID.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAT\e397a7ac-842a-4d44-87ef-170392dee522.xml
    //  </remarks>
    public partial struct _LSAPR_TRANSLATED_SID_EX2
    {

        /// <summary>
        ///  Defines the type of the security principal, as specified
        ///  in section 2.2.13.
        /// </summary>
        public _SID_NAME_USE Use;

        /// <summary>
        ///  Contains the SID ([MS-DTYP] section) of the security
        ///  principal. This field MUST be treated as a [ref] pointer
        ///  and hence MUST be non-NULL.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _RPC_SID[] Sid;

        /// <summary>
        ///  Contains the index into an LSAPR_REFERENCED_DOMAIN_LIST
        ///  structure that specifies the domain that the security
        ///  principal is in. A DomainIndex value of -1 MUST be
        ///  used to specify that there are no corresponding domains.
        ///  Other negative values MUST NOT be used.
        /// </summary>
        public int DomainIndex;

        /// <summary>
        ///  Contains bitmapped values that define the properties
        ///  of this translation. The value MUST be the logical
        ///  OR of zero or more of the following flags. These flags
        ///  communicate additional information on how the name
        ///  was resolved.
        /// </summary>
        public _LSAPR_TRANSLATED_SID_EX2_Flags_Values Flags;
    }

    /// <summary>
    /// LSAPR_TRANSLATED_SID_EX2 flags
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum _LSAPR_TRANSLATED_SID_EX2_Flags_Values : uint
    {

        /// <summary>
        ///  The name was not found by matching against the Security
        ///  Principal Name property.
        /// </summary>
        NameNotMatchingSecurityPrincipal = 0x00000001,

        /// <summary>
        ///  The name might be found by traversing a forest trust.
        /// </summary>
        NameTraversingForestTrust = 0x00000002,

        /// <summary>
        ///  The name was found by matching against the last database
        ///  view (see section 3.1.1.1.1).
        /// </summary>
        NameMatchingLastDatabaseView = 0x00000004,
    }

    /// <summary>
    ///  The LSAPR_SID_INFORMATION structure contains a PRPC_SID
    ///  value.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAT\f04a771b-c018-4098-81b5-2a819f9b5db8.xml
    //  </remarks>
    public partial struct _LSAPR_SID_INFORMATION
    {

        /// <summary>
        ///  Contains the PRPC_SID value, as specified in [MS-DTYP]
        ///  section 2.4.2.2. This field MUST be non-NULL.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _RPC_SID[] Sid;
    }

    /// <summary>
    ///  The LSAPR_TRANSLATED_SIDS_EX2 structure contains a set
    ///  of translated SIDs.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAT\f55a6251-6071-4600-89e7-7a7692e6aff4.xml
    //  </remarks>
    public partial struct _LSAPR_TRANSLATED_SIDS_EX2
    {

        /// <summary>
        ///  Contains the number of translated SIDs.The windowsRPC
        ///  server and RPC client limit the Entries field of this
        ///  structure to 1,000 (using the range primitive defined
        ///  in [MS-RPCE]) in windows_xp_sp2, windows_server_2003,
        ///  windows_vista, and windows_server_2008, windows_vista,
        ///  windows_server_2008, windows_7, and windows_server_7.
        ///  windows_nt_3_1, windows_nt_3_5, windows_nt_3_51, windows_nt_4_0,
        ///  windows_2000, and windows_xp do not enforce this restriction.
        /// </summary>
        public uint Entries;

        /// <summary>
        ///  Contains a set of structures that define translated
        ///  SIDs, as specified in section 2.2.25. If the Entries field
        ///  in this structure is not 0, this field MUST be non-NULL.
        ///  If Entries is 0, this field MUST be NULL.
        /// </summary>
        [Size("Entries")]
        public _LSAPR_TRANSLATED_SID_EX2[] Sids;
    }

    /// <summary>
    ///  The LSAPR_TRANSLATED_NAMES structure defines a set of
    ///  translated names. This is used in the response to a
    ///  translation request from SIDs to names.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAT\ff977eb9-563a-4353-a95f-640e7ee16356.xml
    //  </remarks>
    public partial struct _LSAPR_TRANSLATED_NAMES
    {

        /// <summary>
        ///  Contains the number of translated names.The windowsRPC
        ///  server and RPC client limit the Entries field of this
        ///  structure to 0x5000 (using the range primitive defined
        ///  in [MS-RPCE]) in windows_xp_sp2, windows_server_2003,
        ///  windows_vista, and windows_server_2008, windows_vista,
        ///  windows_server_2008, windows_7, and windows_server_7.
        ///  windows_nt_3_1, windows_nt_3_5, windows_nt_3_51, windows_nt_4_0,
        ///  windows_2000, and windows_xp do not enforce this restriction.
        /// </summary>
        public uint Entries;

        /// <summary>
        ///  Contains a set of translated names, as specified in
        ///  section 2.2.19. If the Entries field in this structure is
        ///  not 0, this field MUST be non-NULL. If Entries is 0,
        ///  this field MUST be ignored.
        /// </summary>
        [Size("Entries")]
        public _LSAPR_TRANSLATED_NAME[] Names;
    }

    /// <summary>
    ///  The LSAPR_OBJECT_ATTRIBUTES structure specifies an object
    ///  and its properties as specified in [MS-LSAD] section
    ///  .
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAT\11b18f08-8f19-40bd-9794-fc6bae3d7b05.xml
    //  </remarks>
    public partial struct _LSAPR_OBJECT_ATTRIBUTES
    {

        /// <summary>
        ///  Length member.
        /// </summary>
        public uint Length;

        /// <summary>
        ///  RootDirectory member.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] RootDirectory;

        /// <summary>
        ///  ObjectName member.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _STRING[] ObjectName;

        /// <summary>
        ///  Attributes member.
        /// </summary>
        public uint Attributes;

        /// <summary>
        ///  SecurityDescriptor member.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _LSAPR_SECURITY_DESCRIPTOR[] SecurityDescriptor;

        /// <summary>
        ///  SecurityQualityOfService member.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _SECURITY_QUALITY_OF_SERVICE[] SecurityQualityOfService;
    }

    /// <summary>
    ///  The LSAPR_SID_ENUM_BUFFER structure defines a set of
    ///  SIDs. This structure is used during a translation request
    ///  for a batch of SIDs to names.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAT\1ffb61f0-a4fe-4487-858d-fb709d605855.xml
    //  </remarks>
    public partial struct _LSAPR_SID_ENUM_BUFFER
    {

        /// <summary>
        ///  Contains the number of translated SIDs.The windows implementation
        ///  of the RPC server and RPC client limits the Entries
        ///  field of this structure to 0x5000 (using the range
        ///  primitive defined in [MS-RPCE]) in windows_xp_sp2,
        ///  windows_server_2003, windows_vista, and windows_server_2008,
        ///  windows_vista, windows_server_2008, windows_7, and
        ///  windows_server_7. windows_nt_3_1, windows_nt_3_5, windows_nt_3_51,
        ///  windows_nt_4_0, windows_2000, and windows_xp do not
        ///  enforce this restriction.
        /// </summary>
        public uint Entries;

        /// <summary>
        ///  Contains a set of structures that contain SIDs, as specified
        ///  in section 2.2.17. If the Entries field in this structure
        ///  is not 0, this field MUST be non-NULL. If Entries is
        ///  0, this field MUST be ignored.
        /// </summary>
        [Size("Entries")]
        public _LSAPR_SID_INFORMATION[] SidInfo;
    }

    /// <summary>
    ///  The LSAPR_REFERENCED_DOMAIN_LIST structure contains
    ///  information about the domains referenced in a lookup
    ///  operation.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAT\3a52af31-247a-4b08-91a0-1d46b2cc49b2.xml
    //  </remarks>
    public partial struct _LSAPR_REFERENCED_DOMAIN_LIST
    {

        /// <summary>
        ///  Contains the number of domains referenced in the lookup
        ///  operation.
        /// </summary>
        public uint Entries;

        /// <summary>
        ///  Contains a set of structures that identify domains.
        ///  If the Entries field in this structure is not 0, this
        ///  field MUST be non-NULL. If Entries is 0, this field
        ///  MUST be ignored.
        /// </summary>
        [Size("Entries")]
        public _LSAPR_TRUST_INFORMATION[] Domains;

        /// <summary>
        ///  This field MUST be ignored. The content is unspecified,
        ///  and no requirements are placed on its value since it
        ///  is never used.
        /// </summary>
        public uint MaxEntries;
    }

    /// <summary>
    ///  The LSAPR_TRANSLATED_NAMES_EX structure contains a set
    ///  of translated names.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAT\5f2bd56b-a162-4033-9c2f-7323fcf09e8c.xml
    //  </remarks>
    public partial struct _LSAPR_TRANSLATED_NAMES_EX
    {

        /// <summary>
        ///  Contains the number of translated names.The windowsRPC
        ///  server and RPC client limit the Entries field of this
        ///  structure to 0x5000 (using the range primitive defined
        ///  in [MS-RPCE]) in windows_xp_sp2, windows_server_2003,
        ///  windows_vista, and windows_server_2008, windows_vista,
        ///  windows_server_2008, windows_7, and windows_server_7.
        ///  windows_nt_3_1, windows_nt_3_5, windows_nt_3_51, windows_nt_4_0,
        ///  windows_2000, and windows_xp do not enforce this restriction.
        /// </summary>
        public uint Entries;

        /// <summary>
        ///  Contains a set of structures that contain translated
        ///  names, as specified in section 2.2.21. If the Entries field
        ///  in this structure is not 0, this field MUST be non-NULL.
        ///  If Entries is 0, this field MUST be ignored.
        /// </summary>
        [Size("Entries")]
        public _LSAPR_TRANSLATED_NAME_EX[] Names;
    }

    /// <summary>
    /// Flags specified by the caller that control the lookup operation.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    public enum LookupOptions_Values : uint
    {

        /// <summary>
        ///  Isolated names are searched for even when they are not
        ///  on the local computer.
        /// </summary>
        NamesBesidesLocalComputer = 0x00000000,

        /// <summary>
        ///  Isolated names (except for user principal names (UPNs))
        ///  are searched for only on the local account database.
        ///  UPNs are not searched for in any of the views.
        /// </summary>
        NamesOnlyLocalAccountDatabaseExceptUPNs = 0x80000000,
    }

    /// <summary>
    /// Version of the client, which implies the client's capabilities.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum ClientRevision_Values : uint
    {

        /// <summary>
        ///  The client does not understand DNS domain names and
        ///  is not aware that it may be part of a forest.
        /// </summary>
        Unknown = 0x00000001,

        /// <summary>
        ///  The client understands DNS domain names and is aware
        ///  that it may be part of a forest.Error codes returned
        ///  can include STATUS_TRUSTED_DOMAIN_FAILURE and STATUS_TRUSTED_RELATIONSHIP_FAILURE,
        ///  which are not returned for ClientRevision of 0x00000001.
        ///  For more information on error codes, see [MS-ERREF].
        /// </summary>
        Known = 0x00000002,
    }

    /// <summary>
    ///  The TRUSTED_INFORMATION_CLASS enumeration type contains
    ///  values that specify the type of trusted domain information
    ///  queried or set by the client.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\36069113-6c38-45e8-920e-17f8ef36f578.xml
    //  </remarks>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum _TRUSTED_INFORMATION_CLASS
    {

        /// <summary>
        ///  TrustedDomainNameInformation constant.
        /// </summary>
        TrustedDomainNameInformation = 1,

        /// <summary>
        ///  TrustedControllersInformation constant.
        /// </summary>
        TrustedControllersInformation,

        /// <summary>
        ///  TrustedPosixOffsetInformation constant.
        /// </summary>
        TrustedPosixOffsetInformation,

        /// <summary>
        ///  TrustedPasswordInformation constant.
        /// </summary>
        TrustedPasswordInformation,

        /// <summary>
        ///  TrustedDomainInformationBasic constant.
        /// </summary>
        TrustedDomainInformationBasic,

        /// <summary>
        ///  TrustedDomainInformationEx constant.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
        TrustedDomainInformationEx,

        /// <summary>
        ///  TrustedDomainAuthInformation constant.
        /// </summary>
        TrustedDomainAuthInformation,

        /// <summary>
        ///  TrustedDomainFullInformation constant.
        /// </summary>
        TrustedDomainFullInformation,

        /// <summary>
        ///  TrustedDomainAuthInformationInternal constant.
        /// </summary>
        TrustedDomainAuthInformationInternal,

        /// <summary>
        ///  TrustedDomainFullInformationInternal constant.
        /// </summary>
        TrustedDomainFullInformationInternal,

        /// <summary>
        ///  TrustedDomainInformationEx2Internal constant.
        /// </summary>
        TrustedDomainInformationEx2Internal,

        /// <summary>
        ///  TrustedDomainFullInformation2Internal constant.
        /// </summary>
        TrustedDomainFullInformation2Internal,

        /// <summary>
        ///  TrustedDomainSupportedEncryptionTypes constant.
        /// </summary>
        TrustedDomainSupportedEncryptionTypes,
    }

    /// <summary>
    ///  The POLICY_DOMAIN_INFORMATION_CLASS enumeration type
    ///  contains values that specify the type of policy being
    ///  queried or set by the client.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\566a61fc-2e99-47c8-99ca-62f7e22cb15d.xml
    //  </remarks>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum _POLICY_DOMAIN_INFORMATION_CLASS
    {

        /// <summary>
        ///  PolicyDomainQualityOfServiceInformation constant.
        /// </summary>
        PolicyDomainQualityOfServiceInformation = 1,

        /// <summary>
        ///  PolicyDomainEfsInformation constant.
        /// </summary>
        PolicyDomainEfsInformation = 2,

        /// <summary>
        ///  PolicyDomainKerberosTicketInformation constant.
        /// </summary>
        PolicyDomainKerberosTicketInformation = 3,
    }

    /// <summary>
    ///  The POLICY_LSA_SERVER_ROLE enumeration takes one of
    ///  two possible values, depending on which capacity the
    ///  account domain database is in: primary or backup. Certain
    ///  operations of the protocol are allowed only against
    ///  a primary account database. On non–domain controller
    ///  machines, the account domain database is in primary
    ///  state.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\620010b4-b439-4d46-893a-cb67246de5fc.xml
    //  </remarks>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum _POLICY_LSA_SERVER_ROLE
    {

        /// <summary>
        ///  A backup account database.
        /// </summary>
        PolicyServerRoleBackup = 2,

        /// <summary>
        ///  A primary account database.
        /// </summary>
        PolicyServerRolePrimary,
    }

    /// <summary>
    ///  The LSA_FOREST_TRUST_RECORD_TYPE enumeration specifies
    ///  a type of foresttrust record.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\700a91e8-a1a4-4e1b-9ad6-096b3cf0bef0.xml
    //  </remarks>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum _LSA_FOREST_TRUST_RECORD_TYPE
    {

        /// <summary>
        ///  The DNS name of the trusted forest. The structure used
        ///  for this record type is equivalent to LSA_UNICODE_STRING.
        /// </summary>
        ForestTrustTopLevelName = 0,

        /// <summary>
        ///  The DNS name of the trusted forest. This is the same
        ///  as ForestTrustTopLevelName. The structure used for
        ///  this record type is equivalent to LSA_UNICODE_STRING.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
        ForestTrustTopLevelNameEx = 1,

        /// <summary>
        ///  This field specifies a record containing identification
        ///  and name information.
        /// </summary>
        ForestTrustDomainInfo = 2,
    }

    /// <summary>
    ///  The POLICY_INFORMATION_CLASS enumeration type contains
    ///  values that specify the type of policy being queried
    ///  or set by the client.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\9ce0bb37-fc6c-4230-b109-7e1881660b83.xml
    //  </remarks>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum _POLICY_INFORMATION_CLASS
    {

        /// <summary>
        ///  Information about audit log.
        /// </summary>
        PolicyAuditLogInformation = 1,

        /// <summary>
        ///  Auditing options.
        /// </summary>
        PolicyAuditEventsInformation,

        /// <summary>
        ///  Primary domain information.
        /// </summary>
        PolicyPrimaryDomainInformation,

        /// <summary>
        ///  Obsolete information class.
        /// </summary>
        PolicyPdAccountInformation,

        /// <summary>
        ///  Account domain information.
        /// </summary>
        PolicyAccountDomainInformation,

        /// <summary>
        ///  Server role information.
        /// </summary>
        PolicyLsaServerRoleInformation,

        /// <summary>
        ///  Replica source information.
        /// </summary>
        PolicyReplicaSourceInformation,

        /// <summary>
        ///  Obsolete information class.
        /// </summary>
        PolicyDefaultQuotaInformation,

        /// <summary>
        ///  Obsolete information class.
        /// </summary>
        PolicyModificationInformation,

        /// <summary>
        ///  Audit log behavior.
        /// </summary>
        PolicyAuditFullSetInformation,

        /// <summary>
        ///  Audit log state.
        /// </summary>
        PolicyAuditFullQueryInformation,

        /// <summary>
        ///  DNS domain information.
        /// </summary>
        PolicyDnsDomainInformation,

        /// <summary>
        ///  DNS domain information.
        /// </summary>
        PolicyDnsDomainInformationInt,

        /// <summary>
        ///  Local account domain information.
        /// </summary>
        PolicyLocalAccountDomainInformation,

        /// <summary>
        /// Machine account information.
        /// </summary>
        PolicyMachineAccountInformation,

        /// <summary>
        ///  Not used in this protocol. Present to mark the end of
        ///  the enumeration.
        /// </summary>
        PolicyLastEntry,
    }

    /// <summary>
    ///  The LSA_FOREST_TRUST_COLLISION_RECORD_TYPE type specifies
    ///  the type of a collision record in the message.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\afc7d769-a317-4805-9f45-85d5393b57af.xml
    //  </remarks>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum _LSA_FOREST_TRUST_COLLISION_RECORD_TYPE
    {

        /// <summary>
        ///  A foresttrust record that a caller attempted to set
        ///  on a trusted domain object has suffered a collision
        ///  with another trusted domain object in Active Directory,
        ///  as specified in [MS-ADTS].
        /// </summary>
        CollisionTdo = 0,

        /// <summary>
        ///  A foresttrust record that a caller attempted to set
        ///  on a trusted domain object has suffered a collision
        ///  with a cross-reference object belonging to the forest
        ///  to which the server belongs, as specified in [MS-ADTS].
        /// </summary>
        CollisionXref,

        /// <summary>
        ///  A foresttrust record that a caller attempted to set
        ///  on a trusted domain object has suffered a collision
        ///  for an unknown reason.
        /// </summary>
        CollisionOther,
    }


    /// <summary>
    ///  The POLICY_AUDIT_FULL_QUERY_INFO structure is used to
    ///  query information about the state of the audit log
    ///  on the server. The following structure corresponds
    ///  to the PolicyAuditFullQueryInformation information
    ///  class. This information class is obsolete and exists
    ///  for backward compatibility purposes only.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\0ef0845f-f20e-4897-ad29-88c0c07be0f4.xml
    //  </remarks>
    public partial struct _POLICY_AUDIT_FULL_QUERY_INFO
    {

        /// <summary>
        ///  This field indicates whether the system MUST shut down
        ///  when the event log is full.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "ShutDown")]
        public byte ShutDownOnFull;

        /// <summary>
        ///  This field indicates whether the event log is full or
        ///  not.
        /// </summary>
        public byte LogIsFull;
    }

    /// <summary>
    ///  The POLICY_AUDIT_FULL_SET_INFO structure contains information
    ///  to set on the server that is controlling audit log
    ///  behavior. The following structure corresponds to the
    ///  PolicyAuditFullSetInformation information class. This
    ///  information class is not supported.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\3224400e-3c40-4e64-810a-8b11341ba4c6.xml
    //  </remarks>
    public partial struct _POLICY_AUDIT_FULL_SET_INFO
    {

        /// <summary>
        ///  A nonzero value means that the system MUST shut down
        ///  when the event log is full, while zero means that the
        ///  system MUST NOT shut down when the event log is full.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "ShutDown")]
        public byte ShutDownOnFull;
    }

    /// <summary>
    ///  The POLICY_DOMAIN_QUALITY_OF_SERVICE_INFO structure
    ///  is obsolete and exists for backward compatibility purposes
    ///  only.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\38bd52a0-4514-468f-b342-d7421a51a316.xml
    //  </remarks>
    public partial struct _POLICY_DOMAIN_QUALITY_OF_SERVICE_INFO
    {

        /// <summary>
        ///  Quality of service of the responder. MUST be set to
        ///  zero when sent and MUST be ignored on receipt.
        /// </summary>
        public QualityOfService_Values QualityOfService;
    }

    /// <summary>
    /// Quality of service of the responder. MUST be set to
    ///  zero when sent and MUST be ignored on receipt.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Naming", "CA1717:OnlyFlagsEnumsShouldHavePluralNames")]
    public enum QualityOfService_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The LSAPR_POLICY_DOMAIN_EFS_INFO structure communicates
    ///  a counted binary byte array.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\3ba6e751-cf91-4d87-a74c-488bb927a54c.xml
    //  </remarks>
    public partial struct _LSAPR_POLICY_DOMAIN_EFS_INFO
    {

        /// <summary>
        ///  The count of bytes in the EfsBlob.
        /// </summary>
        public uint InfoLength;

        /// <summary>
        ///  An array of bytes, as specified in [MS-EFSR].An array
        ///  of bytes, of size InfoLength bytes. If the value of
        ///  InfoLength is other than 0, this field MUST NOT be
        ///  NULL. The syntax of this blob SHOULDMicrosoft implementations
        ///  of the Local Security Authority (Domain Policy) Remote
        ///  Protocol do not enforce data in EfsBlob to conform
        ///  to the layout specified in [MS-GPEF] section. conform
        ///  to the layout specified in [MS-GPEF] section.
        /// </summary>
        [Size("InfoLength")]
        public byte[] EfsBlob;
    }

    /// <summary>
    ///  The LSAPR_SR_SECURITY_DESCRIPTOR structure is used to
    ///  communicate a self-relative security descriptor, as
    ///  specified in [MS-DTYP].
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\5564065e-3f3d-4481-a385-367cc9b042c4.xml
    //  </remarks>
    public partial struct _LSAPR_SR_SECURITY_DESCRIPTOR
    {

        /// <summary>
        ///  The count of bytes in SecurityDescriptor.The windowsRPC
        ///  server and client limit the Length field of this structure
        ///  to 262144 (using the range primitive specified in [MS-RPCE])
        ///  in windows_xp_sp2, windows_server_2003, windows_vista,
        ///  and windows_server_2008, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7.
        /// </summary>
        public uint Length;

        /// <summary>
        ///  The contiguous buffer containing the self-relative security
        ///  descriptor. This field MUST contain the Length number
        ///  of bytes. If the Length field has a value other than
        ///  0, this field MUST NOT be NULL.
        /// </summary>
        [Size("Length")]
        public byte[] SecurityDescriptor;
    }

    /// <summary>
    ///  The LSAPR_CR_CIPHER_VALUE structure is a counted buffer
    ///  of bytes containing a secret object.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\782eda77-b82e-4134-87c9-eb5e67f18f06.xml
    //  </remarks>
    public partial struct _LSAPR_CR_CIPHER_VALUE
    {

        /// <summary>
        ///  This field contains the number of valid bytes in the
        ///  Buffer field.The windowsRPC server and client limit
        ///  the Length field of this structure to 131088 (using
        ///  the range primitive as specified in [MS-RPCE]) in windows_xp_sp2,
        ///  windows_server_2003, windows_vista, and windows_server_2008,
        ///  windows_vista, windows_server_2008, windows_7, and
        ///  windows_server_7.
        /// </summary>
        public uint Length;

        /// <summary>
        ///  This field contains the number of allocated bytes in
        ///  the Buffer field.The windowsRPC server and client limit
        ///  the MaximumLength field of this structure to 131088
        ///  (using the range primitive defined in [MS-RPCE]) in
        ///  windows_xp_sp2, windows_server_2003, windows_vista,
        ///  and windows_server_2008, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7.
        /// </summary>
        public uint MaximumLength;

        /// <summary>
        ///  This field contains the actual secret data. If the value
        ///  of the MaximumLength field is greater than 0, this
        ///  field MUST contain a non-NULL value. This field is
        ///  always encrypted using algorithms as specified in section
        ///  .
        /// </summary>
        [Length("Length")]
        [Size("MaximumLength")]
        public byte[] Buffer;
    }

    /// <summary>
    ///  The TRUSTED_DOMAIN_SUPPORTED_ENCRYPTION_TYPES structure
    ///  is used to present the encryption types that are allowed
    ///  through a trust.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\7c519a64-3dc1-4be6-a17d-76817cff6e39.xml
    //  </remarks>
    public partial struct _TRUSTED_DOMAIN_SUPPORTED_ENCRYPTION_TYPES
    {

        /// <summary>
        ///  This field contains bitmapped values that define the
        ///  encryption types supported by this trust relationship.
        ///  The flags can be set in any combination.01234567891
        ///  01234567892 01234567893 01000000000000000000000000000SARMCC:
        ///  Supports CRC32, as specified in [RFC3961] page 31.M:
        ///  Supports RSA-MD5, as specified in [RFC3961] page 31.R:
        ///  Supports RC4-HMAC-MD5, as specified in [RFC4757].A:
        ///  Supports HMAC-SHA1-96-AES128, as specified in [RFC3961]
        ///  page 31.S: Supports HMAC-SHA1-96-AES256, as specified
        ///  in [RFC3961] page 31.All other bits SHOULD be 0 and
        ///  ignored upon receipt.
        /// </summary>
        public uint SupportedEncryptionTypes;
    }


    /// <summary>
    ///  The TRUSTED_POSIX_OFFSET_INFO structure communicates
    ///  any offset necessary for POSIX compliance. The following
    ///  structure corresponds to the TrustedPosixOffsetInformation
    ///  information class.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\b091ee7e-f5c3-4b48-8567-1b08ea002221.xml
    //  </remarks>
    public partial struct _TRUSTED_POSIX_OFFSET_INFO
    {

        /// <summary>
        ///  The offset to use for the generation of POSIX IDs for
        ///  users and groups, as specified in "trustPosixOffset"
        ///  in [MS-ADTS] section.
        /// </summary>
        public uint Offset;
    }

    /// <summary>
    ///  The LSAPR_POLICY_AUDIT_EVENTS_INFO structure contains
    ///  auditing options on the server.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\d00fc364-577d-4ed0-b3a5-952d78b67695.xml
    //  </remarks>
    public partial struct _LSAPR_POLICY_AUDIT_EVENTS_INFO
    {

        /// <summary>
        ///  0 indicates that auditing is disabled. All other values
        ///  indicate that auditing is enabled.
        /// </summary>
        public byte AuditingMode;

        /// <summary>
        ///  For every auditing category ID (which maps into the
        ///  ordinal index into this array), this field contains
        ///  an options flag that takes exactly one of the following
        ///  values.If the MaximumAuditingEventCount field has a
        ///  value other than 0, this field MUST NOT be NULL.
        /// </summary>
        [Size("MaximumAuditEventCount")]
        public EventAuditingOptions_Values[] EventAuditingOptions;

        /// <summary>
        ///  The number of entries in the EventAuditingOptions array.The
        ///  windowsRPC server and RPC client limit the MaximumAuditEventCount
        ///  field of this structure to 1000 (using the range primitive,
        ///  as specified in [MS-RPCE]) in windows_xp_sp2, windows_server_2003,
        ///  windows_vista, and windows_server_2008, windows_vista,
        ///  windows_server_2008, windows_7, and windows_server_7.
        /// </summary>
        public uint MaximumAuditEventCount;
    }

    /// <summary>
    /// For every auditing category ID (which maps into the ordinal index into this array),
    /// this field contains an options flag that takes exactly one of the following values.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [Flags()]
    public enum EventAuditingOptions_Values : uint
    {

        /// <summary>
        ///  Leave existing auditing options unchanged for events
        ///  of this type; used only for set operations.
        /// </summary>
        POLICY_AUDIT_EVENT_UNCHANGED = 0x00000000,

        /// <summary>
        ///  Cancel all auditing options for events of this type.
        ///  If set, the success/failure flags are ignored.
        /// </summary>
        POLICY_AUDIT_EVENT_NONE = 0x00000004,

        /// <summary>
        ///  When auditing is enabled, audit all successful occurrences
        ///  of events of the given type.
        /// </summary>
        POLICY_AUDIT_EVENT_SUCCESS = 0x00000001,

        /// <summary>
        ///  When auditing is enabled, audit all unsuccessful occurrences
        ///  of events of the given type.
        /// </summary>
        POLICY_AUDIT_EVENT_FAILURE = 0x00000002,
    }

    /// <summary>
    ///  The POLICY_LSA_SERVER_ROLE_INFO structure is used to
    ///  allow callers to query and set whether the account
    ///  domain database acts as the primary copy or backup
    ///  copy. The following structure corresponds to the PolicyLsaServerRoleInformation
    ///  information class.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\d37dbc65-04f3-4db8-b40a-4e9dd6c12520.xml
    //  </remarks>
    public partial struct _POLICY_LSA_SERVER_ROLE_INFO
    {

        /// <summary>
        ///  One of the values of the POLICY_LSA_SERVER_ROLE enumeration
        ///  on return.
        /// </summary>
        public _POLICY_LSA_SERVER_ROLE LsaServerRole;
    }

    /// <summary>
    ///  The LSA_FOREST_TRUST_BINARY_DATA structure is used to
    ///  communicate a foresttrust record. This structure is
    ///  not used in the current version of the protocol.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\d4859b44-b776-4237-baa1-12dc28c19634.xml
    //  </remarks>
    public partial struct _LSA_FOREST_TRUST_BINARY_DATA
    {

        /// <summary>
        ///  The count of bytes in Buffer.The windowsRPC server and
        ///  client limit the Length field of this structure to
        ///  131072 (using the range primitive defined in [MS-RPCE])
        ///  in windows_xp_sp2, windows_server_2003, windows_vista,
        ///  and windows_server_2008, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. Other versions do
        ///  not enforce this restriction.
        /// </summary>
        public uint Length;

        /// <summary>
        ///  The trust record. If the Length field has a value other
        ///  than 0, this field MUST NOT be NULL.
        /// </summary>
        [Size("Length")]
        public byte[] Buffer;
    }

    /// <summary>
    ///  The LSAPR_TRUSTED_DOMAIN_AUTH_BLOB structure contains
    ///  a counted buffer of authentication material. Domaintrust
    ///  authentication is specified in [MS-ADTS] section.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\da8f32a1-0a16-4194-810d-06cc0698595a.xml
    //  </remarks>
    public partial struct _LSAPR_TRUSTED_DOMAIN_AUTH_BLOB
    {

        /// <summary>
        ///  The count of bytes in AuthBlob.The windowsRPC server
        ///  and client limit the Entries field of this structure
        ///  to 63336 (using the range primitive defined in [MS-RPCE])
        ///  in windows_xp_sp2, windows_server_2003, windows_vista,
        ///  and windows_server_2008, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. Other versions do
        ///  not enforce this restriction.
        /// </summary>
        public uint AuthSize;

        /// <summary>
        ///  An array of bytes containing the authentication material.
        ///  If the AuthSize field has a value other than 0, this
        ///  field MUST NOT be NULL. Always encrypted using algorithms,
        ///  as specified in section. The plaintext layout is in
        ///  the following format.The incoming and outgoing authentication
        ///  information buffer size included at the end of the
        ///  LSAPR_TRUSTED_DOMAIN_AUTH_BLOB can be used to extract
        ///  the incoming and outgoing authentication information
        ///  buffers from the LSAPR_TRUSTED_DOMAIN_AUTH_BLOB. Each
        ///  of these buffers contains the byte offset to both the
        ///  current and the previous authentication information.
        ///  This information can be used to extract current and
        ///  (if any) previous authentication information.01234567891012345678920123456789301512
        ///  bytes of random data ...CountOutgoingAuthInfosByteOffsetCurrentOutgoingAuthInfoByteOffsetPreviousOutgoingAuthInfoCurrentOutgoingAuthInfos...PreviousOutgoingAuthInfos
        ///  (optional)...CountIncomingAuthInfosByteOffsetCurrentIncomingAuthInfoByteOffsetPreviousIncomingAuthInfoCurrentIncomingAuthInfos...PreviousIncomingAuthInfos
        ///  (optional)...OutgoingAuthInfoSizeIncomingAuthInfoSizeCountOutgoingAuthInfos
        ///  (4 bytes): Specifies the count of entries present in
        ///  the CurrentOutgoingAuthInfos field.ByteOffsetCurrentOutgoingAuthInfo
        ///  (4 bytes): Specifies the byte offset from the beginning
        ///  of CountOutgoingAuthInfos to the start of the CurrentOutgoingAuthInfos
        ///  field.ByteOffsetPreviousOutgoingAuthInfo (4 bytes):
        ///  Specifies the byte offset from the beginning of CountOutgoingAuthInfos
        ///  to the start of the PreviousOutgoingAuthInfos field.
        ///  If the difference between ByteOffsetPreviousOutgoingAuthInfo
        ///  and OutgoingAuthInfoSize is 0, this field MUST be ignored;
        ///  this also means that the PreviousOutgoingAuthInfos
        ///  field has zero entries.CurrentOutgoingAuthInfos: Contains
        ///  an array of CountOutgoingAuthInfos of LSAPR_AUTH_INFORMATION
        ///  entries in self-relative format. Each LSAPR_AUTH_INFORMATION
        ///  entry in the array MUST be 4-byte aligned. When it
        ///  is necessary to insert unused padding bytes into a
        ///  buffer for data alignment, such bytes MUST be set to
        ///  0.PreviousOutgoingAuthInfos: Contains an array of CountOutgoingAuthInfosLSAPR_AUTH_INFORMATION
        ///  entries in self-relative format. See the comments for
        ///  the ByteOffsetPreviousOutgoingAuthInfo field to determine
        ///  when this field is present. Each LSAPR_AUTH_INFORMATION
        ///  entry in the array MUST be 4-byte aligned. When it
        ///  is necessary to insert unused padding bytes into a
        ///  buffer for data alignment, such bytes MUST be set to
        ///  0.CountIncomingAuthInfos (4 bytes): Specifies the count
        ///  of entries present in the CountIncomingAuthInfos field.ByteOffsetCurrentIncomingAuthInfo
        ///  (4 bytes): Specifies the byte offset from the beginning
        ///  of CountIncomingAuthInfos to the start of the CurrentIncomingAuthInfos
        ///  field.ByteOffsetPreviousIncomingAuthInfo (4 bytes):
        ///  Specifies the byte offset from the beginning of CountIncomingAuthInfos
        ///  to the start of the PreviousIncomingAuthInfos field.
        ///  If the difference between ByteOffsetPreviousIncomingAuthInfo
        ///  and IncomingAuthInfoSize is 0, this field MUST be ignored;
        ///  this also means that the PreviousIncomingAuthInfos
        ///  field has zero entries.CurrentIncomingAuthInfos: Contains
        ///  an array of CountIncomingAuthInfosLSAPR_AUTH_INFORMATION
        ///  entries in self-relative format. Each LSAPR_AUTH_INFORMATION
        ///  entry in the array MUST be 4-byte aligned. When it
        ///  is necessary to insert unused padding bytes into a
        ///  buffer for data alignment, such bytes MUST be set to
        ///  0.PreviousIncomingAuthInfos: Contains an array of CountIncomingAuthInfosLSAPR_AUTH_INFORMATION
        ///  entries in self-relative format. See the comments for
        ///  the ByteOffsetPreviousIncomingAuthInfo field to determine
        ///  when this field is present. Each LSAPR_AUTH_INFORMATION
        ///  entry in the array MUST be 4-byte aligned. When it
        ///  is necessary to insert unused padding bytes into a
        ///  buffer for data alignment, such bytes MUST be set to
        ///  0.OutgoingAuthInfoSize (4 bytes): Specifies the size,
        ///  in bytes, of the subportion of the structure from the
        ///  beginning of the CountOutgoingAuthInfos field through
        ///  the end of the of the PreviousOutgoingAuthInfos field.IncomingAuthInfoSize
        ///  (4 bytes): Specifies the size, in bytes, of the sub-portion
        ///  of the structure from the beginning of the CountIncomingAuthInfos
        ///  field through the end of the of the PreviousIncomingAuthInfos
        ///  field.
        /// </summary>
        [Size("AuthSize")]
        public byte[] AuthBlob;
    }


    /// <summary>
    ///  The LSAPR_LUID_AND_ATTRIBUTES structure is a tuple defining
    ///  a locally unique identifier (LUID) and a field defining
    ///  the attributes of the LUID.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\03c834c0-f310-4e0c-832e-b6e7688364d1.xml
    //  </remarks>
    public partial struct _LSAPR_LUID_AND_ATTRIBUTES
    {

        /// <summary>
        ///  The locally unique identifier.
        /// </summary>
        public _LUID Luid;

        /// <summary>
        ///  This field contains bitmapped values that define the
        ///  properties of the privilege set. One or more of the
        ///  following flags can be set.01234567891 01234567892
        ///  01234567893 01000000000000000000000000000000 EDD: The
        ///  privilege is enabled by default.E: The privilege is
        ///  enabled.All other bits SHOULD be 0 and ignored upon
        ///  receipt.
        /// </summary>
        public uint Attributes;
    }

    /// <summary>
    ///  The LSAPR_POLICY_PRIMARY_DOM_INFO structure defines
    ///  the server's primary domain. The following structure
    ///  corresponds to the PolicyPrimaryDomainInformation information
    ///  class.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\0f3f5d3f-66d2-45a0-8c28-ede86f4cd4a8.xml
    //  </remarks>
    public partial struct _LSAPR_POLICY_PRIMARY_DOM_INFO
    {

        /// <summary>
        ///  This field contains a name for the primary domain that
        ///  is subject to the restrictions of a NetBIOS name, as
        ///  specified in [RFC1088]. The value SHOULD be used (by
        ///  implementations external to this protocol) to identify
        ///  the domain via the NetBIOS API, as specified in [RFC1088].
        /// </summary>
        public _RPC_UNICODE_STRING Name;

        /// <summary>
        ///  The SID of the primary domain.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _RPC_SID[] Sid;
    }

    /// <summary>
    ///  The LSA_FOREST_TRUST_COLLISION_RECORD structure is used
    ///  to communicate foresttrust collision information. For
    ///  more information about trusted domain objects, see
    ///  [MS-ADTS] section.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\32178d2c-ca74-4f53-8264-af1906f95011.xml
    //  </remarks>
    public partial struct _LSA_FOREST_TRUST_COLLISION_RECORD
    {

        /// <summary>
        ///  An ordinal number of a foresttrust record in the forest
        ///  trust information supplied by the caller that suffered
        ///  a collision. For rules about collisions, see sections
        ///   and .
        /// </summary>
        public uint Index;

        /// <summary>
        ///  The type of collision record, as specified in section
        ///  .
        /// </summary>
        public _LSA_FOREST_TRUST_COLLISION_RECORD_TYPE Type;

        /// <summary>
        ///  A set of bits specifying the nature of the collision.
        ///  These flags and the rules for generating them are specified
        ///  in sections  and .
        /// </summary>
        public uint Flags;

        /// <summary>
        ///  The name of the existing entity (a top-level name entry,
        ///  a domain information entry, or a top-level name exclusion
        ///  entry) that caused the collision.
        /// </summary>
        public _RPC_UNICODE_STRING Name;
    }

    /// <summary>
    ///  The LSAPR_TRUSTED_PASSWORD_INFO structure is used to
    ///  communicate trust-authentication material. The following
    ///  structure corresponds to the TrustedPasswordInformation
    ///  information class.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\33d7a9e4-c9ca-4021-9627-337d89e656a3.xml
    //  </remarks>
    public partial struct _LSAPR_TRUSTED_PASSWORD_INFO
    {

        /// <summary>
        ///  The current authentication material. See section.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _LSAPR_CR_CIPHER_VALUE[] Password;

        /// <summary>
        ///  The version prior to the current version of the authentication
        ///  material. See section.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _LSAPR_CR_CIPHER_VALUE[] OldPassword;
    }

    /// <summary>
    ///  The LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION_INTERNAL structure
    ///  communicates authentication material. The following
    ///  structure corresponds to the TrustedDomainAuthInformationInternal
    ///  information class. For more information about domaintrust
    ///  authentication material, see [MS-ADTS] section.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\3b1c61fe-6f07-4d83-af54-3a381de5c5d1.xml
    //  </remarks>
    public partial struct _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION_INTERNAL
    {

        /// <summary>
        ///  An LSAPR_TRUSTED_DOMAIN_AUTH_BLOB.
        /// </summary>
        public _LSAPR_TRUSTED_DOMAIN_AUTH_BLOB AuthBlob;
    }

    /// <summary>
    ///  The LSAPR_POLICY_DNS_DOMAIN_INFO structure is used to
    ///  allow callers to query and set the server's primary
    ///  domain.The following applies to windows_2000, windows_xp,
    ///  windows_server_2003_administration_tools_pack, windows_vista,
    ///  and windows_server_2008, windows_vista, windows_server_2008,
    ///  windows_7, and windows_server_7.The windowsRPC server
    ///  always throws an RPC_S_PROCNUM_OUT_OF_RANGE exception
    ///  for the message processing of LsarQueryInformationPolicy,
    ///  LsarQueryInformationPolicy, LsarSetInformationPolicy,
    ///  and LsarSetInformationPolicy2, if the server is configured
    ///  to emulate windows_nt_4_0 for PolicyDnsDomainInformation
    ///  information level.The following structure corresponds
    ///  to the PolicyDnsDomainInformation and PolicyDnsDomainInformationInt
    ///  information classes.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\3e15a02e-25d3-46aa-9c60-8def03c824d2.xml
    //  </remarks>
    public partial struct _LSAPR_POLICY_DNS_DOMAIN_INFO
    {

        /// <summary>
        ///  This field contains a name for the domain that is subject
        ///  to the restrictions of a NetBIOS name, as specified
        ///  in [RFC1088]. This value SHOULD be used (by implementations
        ///  external to this protocol) to identify the domain via
        ///  the NetBIOS API, as specified in [RFC1088].
        /// </summary>
        public _RPC_UNICODE_STRING Name;

        /// <summary>
        ///  The fully qualified DNS name of the domain.
        /// </summary>
        public _RPC_UNICODE_STRING DnsDomainName;

        /// <summary>
        ///  The fully qualified DNS name of the forest containing
        ///  this domain.
        /// </summary>
        public _RPC_UNICODE_STRING DnsForestName;

        /// <summary>
        ///  The globally unique identifier (GUID) of the domain.
        /// </summary>
        public System.Guid DomainGuid;

        /// <summary>
        ///  The SID of the domain.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _RPC_SID[] Sid;
    }

    /// <summary>
    ///  The POLICY_AUDIT_LOG_INFO structure contains information
    ///  about the state of the audit log. The following structure
    ///  corresponds to the PolicyAuditLogInformation information
    ///  class.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\3fff1c62-e8b1-4bc8-b18a-3ba6458ec622.xml
    //  </remarks>
    public partial struct _POLICY_AUDIT_LOG_INFO
    {

        /// <summary>
        ///  A measure of how full the audit log is, as a percentage.
        /// </summary>
        public uint AuditLogPercentFull;

        /// <summary>
        ///  The maximum size of the auditing log, in kilobytes (KB).
        /// </summary>
        public uint MaximumLogSize;

        /// <summary>
        ///  The auditing log retention period (64-bit signed integer),
        ///  a 64-bit value that represents the number of 100-nanosecond
        ///  intervals since January 1, 1601, UTC. An audit record
        ///  can be discarded if its time stamp predates the current
        ///  time minus the retention period.
        /// </summary>
        public _LARGE_INTEGER AuditRetentionPeriod;

        /// <summary>
        ///  A Boolean flag; indicates whether or not a system shutdown
        ///  is being initiated due to the security audit log becoming
        ///  full. This condition occurs only if the system is configured
        ///  to shut down when the log becomes full. After a shutdown
        ///  has been initiated, this flag MUST be set to TRUE (nonzero).
        ///  If an administrator can correct the situation before
        ///  the shutdown becomes irreversible, this flag MUST be
        ///  reset to FALSE (0). This field MUST be ignored for
        ///  set operations.
        /// </summary>
        public byte AuditLogFullShutdownInProgress;

        /// <summary>
        ///  A 64-bit value that represents the number of 100-nanosecond
        ///  intervals since January 1, 1601, UTC. If the AuditLogFullShutdownInProgress
        ///  flag is set, this field MUST contain the time left
        ///  before the shutdown becomes irreversible.
        /// </summary>
        public _LARGE_INTEGER TimeToShutdown;

        /// <summary>
        ///  Not in use. This field SHOULD be set to zero when sent,
        ///  and MUST be ignored on receipt.
        /// </summary>
        public uint NextAuditRecordId;
    }

    /// <summary>
    ///  The LSA_FOREST_TRUST_DOMAIN_INFO structure is used to
    ///  communicate a foresttrust record corresponding to the
    ///  LSA_FOREST_TRUST_DOMAIN_INFO value of ForestTrustDomainInfo.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\451ac72f-e9ad-4a4f-961f-d04a2a5b1515.xml
    //  </remarks>
    public partial struct _LSA_FOREST_TRUST_DOMAIN_INFO
    {

        /// <summary>
        ///  DomainSID for the trusted domain.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _RPC_SID[] Sid;

        /// <summary>
        ///  The DNS name of the trusted domain.
        /// </summary>
        public _RPC_UNICODE_STRING DnsName;

        /// <summary>
        ///  The NetBIOS name of the trusted domain, as specified
        ///  in [RFC1088].
        /// </summary>
        /// 
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Netbios")]
        public _RPC_UNICODE_STRING NetbiosName;
    }

    /// <summary>
    ///  The LSAPR_TRUSTED_CONTROLLERS_INFO structure is used
    ///  to communicate a set of names of domain controllers
    ///  (DCs) in a trusted domain. The following structure
    ///  corresponds to the TrustedControllersInformation information
    ///  class.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\5382bd89-69c6-46f2-beb1-7b70e5befbc5.xml
    //  </remarks>
    public partial struct _LSAPR_TRUSTED_CONTROLLERS_INFO
    {

        /// <summary>
        ///  The count of names.The windowsRPC server and client
        ///  limit the Entries field of this structure to 256 (using
        ///  the range primitive defined in [MS-RPCE]) in windows_xp_sp2,
        ///  windows_server_2003, windows_vista, and windows_server_2008,
        ///  windows_vista, windows_server_2008, windows_7, and
        ///  windows_server_7. Other versions do not enforce this
        ///  restriction.
        /// </summary>
        public uint Entries;

        /// <summary>
        ///  This field contains an array of DC names that are subject
        ///  to the restrictions of a NetBIOS name, as specified
        ///  in [RFC1088]. This field SHOULD be used (by implementations
        ///  external to this protocol) to identify the DCs via
        ///  the NetBIOS API, as specified in [RFC1088]. If the
        ///  Entries field has a value other than 0, this field
        ///  MUST NOT be NULL.
        /// </summary>
        [Size("Entries")]
        public _RPC_UNICODE_STRING[] Names;
    }

    /// <summary>
    ///  The LSAPR_TRUSTED_DOMAIN_NAME_INFO structure is used
    ///  to communicate the name of a trusted domain. The following
    ///  structure corresponds to the TrustedDomainNameInformation
    ///  information class.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\71c5724f-447e-452c-9cb9-a0fd90d88594.xml
    //  </remarks>
    public partial struct _LSAPR_TRUSTED_DOMAIN_NAME_INFO
    {

        /// <summary>
        ///  This field contains a name for the domain that is subject
        ///  to the restrictions of a NetBIOS name, as specified
        ///  in [RFC1088]. This field SHOULD be used (by implementations
        ///  external to this protocol) to identify the domain via
        ///  the NetBIOS API, as specified in [RFC1088].
        /// </summary>
        public _RPC_UNICODE_STRING Name;
    }


    /// <summary>
    ///  The LSAPR_TRUSTED_ENUM_BUFFER structure specifies a
    ///  collection of trust information structures of type
    ///  LSAPR_TRUST_INFORMATION.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\78f8a2e4-4f3d-40f5-bdd1-9dacdf1c832c.xml
    //  </remarks>
    public partial struct _LSAPR_TRUSTED_ENUM_BUFFER
    {

        /// <summary>
        ///  This field contains the number of trust information
        ///  structures.
        /// </summary>
        public uint EntriesRead;

        /// <summary>
        ///  This field contains a set of structures that define
        ///  the trust information, as specified in section. If
        ///  the EntriesRead field has a value other than 0, this
        ///  field MUST NOT be NULL.
        /// </summary>
        [Size("EntriesRead")]
        public _LSAPR_TRUST_INFORMATION[] Information;
    }

    /// <summary>
    ///  The LSAPR_ACCOUNT_INFORMATION structure specifies a
    ///  security principalsecurity identifier (SID).
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\98540c1c-09cc-4ee2-934a-cdde3de0c77f.xml
    //  </remarks>
    public partial struct _LSAPR_ACCOUNT_INFORMATION
    {

        /// <summary>
        ///  This field contains the SID of the security principal.
        ///  This field MUST NOT be NULL.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _RPC_SID[] Sid;
    }

    /// <summary>
    ///  The LSAPR_PRIVILEGE_SET structure defines a set of privileges
    ///  that belong to an account.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\a30a5720-7784-42f4-b03a-b14f4e486bae.xml
    //  </remarks>
    public partial struct _LSAPR_PRIVILEGE_SET
    {

        /// <summary>
        ///  This field contains the number of privileges.The windowsRPC
        ///  server and client limit the PrivilegeCount field of
        ///  this structure to 1000 (using the range primitive specified
        ///  in [MS-RPCE]) in windows_xp_sp2, windows_server_2003,
        ///  windows_vista, and windows_server_2008, windows_vista,
        ///  windows_server_2008, windows_7, and windows_server_7.
        /// </summary>
        public uint PrivilegeCount;

        /// <summary>
        ///  This field contains bitmapped values that define the
        ///  properties of the privilege set. 01234567891 01234567892
        ///  01234567893 010000000000000000000000000000000OO: Valid
        ///  for a set operation indicating that all specified privileges
        ///  that are not already assigned should be assigned.All
        ///  other bits SHOULD be set to zero when sent, and ignored
        ///  on receipt.
        /// </summary>
        public uint Control;

        /// <summary>
        ///  An array of LSAPR_LUID_AND_ATTRIBUTES structures. If
        ///  the PrivilegeCount field has a value different than
        ///  0, this field MUST NOT be NULL.
        /// </summary>
        [Inline()]
        [Size("PrivilegeCount")]
        public _LSAPR_LUID_AND_ATTRIBUTES[] Privilege;
    }

    /// <summary>
    ///  The POLICY_DOMAIN_KERBEROS_TICKET_INFO structure communicates
    ///  policy information about the Kerberos security provider.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\afcc4920-12d3-48e0-ab95-a8989ebbd41d.xml
    //  </remarks>
    public partial struct _POLICY_DOMAIN_KERBEROS_TICKET_INFO
    {

        /// <summary>
        ///  Optional flags that affect validations performed during
        ///  authentication.
        /// </summary>
        public uint AuthenticationOptions;

        /// <summary>
        ///  This is in units of 10^(-7) seconds. It corresponds
        ///  to Maximum ticket lifetime (as specified in [RFC4120]
        ///  section 8.2) for service tickets only. The default
        ///  value of this setting is 10 hours.
        /// </summary>
        public _LARGE_INTEGER MaxServiceTicketAge;

        /// <summary>
        ///  This is in units of 10^(-7) seconds. It corresponds
        ///  to the Maximum ticket lifetime (as specified in [RFC4120]
        ///  section 8.2) for ticket-granting ticket (TGT) only.
        ///  The default value of this setting is 10 hours.
        /// </summary>
        public _LARGE_INTEGER MaxTicketAge;

        /// <summary>
        ///  This is in units of 10^(-7) seconds. It corresponds
        ///  to the Maximum renewable lifetime, as specified in
        ///  [RFC4120] section 8.2. The default value of this setting
        ///  is one week.
        /// </summary>
        public _LARGE_INTEGER MaxRenewAge;

        /// <summary>
        ///  This is in units of 10^(-7) seconds. It corresponds
        ///  to the Acceptable clock skew, as specified in [RFC4120]
        ///  section 8.2. The default value of this setting is five
        ///  minutes.
        /// </summary>
        public _LARGE_INTEGER MaxClockSkew;

        /// <summary>
        ///  The value of this field SHOULD be set to zero when sent
        ///  or on receipt.
        /// </summary>
        public _LARGE_INTEGER Reserved;
    }

    /// <summary>
    ///  The LSAPR_POLICY_PD_ACCOUNT_INFO structure is obsolete
    ///  and exists for backward compatibility purposes only.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\b04175b3-fedf-4dda-9034-f754a10fe64e.xml
    //  </remarks>
    public partial struct _LSAPR_POLICY_PD_ACCOUNT_INFO
    {

        /// <summary>
        ///  Name member.
        /// </summary>
        public _RPC_UNICODE_STRING Name;
    }

    /// <summary>
    ///  The LSAPR_POLICY_ACCOUNT_DOM_INFO structure contains
    ///  information about the server's account domain. The
    ///  following structure corresponds to the PolicyAccountDomainInformation
    ///  and PolicyLocalAccountDomainInformation information
    ///  classes.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\bfad5424-3e20-43bd-87f6-d35b4253792e.xml
    //  </remarks>
    public partial struct _LSAPR_POLICY_ACCOUNT_DOM_INFO
    {

        /// <summary>
        ///  This field contains a name for the account domain that
        ///  is subjected to the restrictions of a NetBIOS name,
        ///  as specified in [RFC1088]. This value SHOULD be used
        ///  (by implementations external to this protocol) to identify
        ///  the domain via the NetBIOS API, as specified in [RFC1088].
        /// </summary>
        public _RPC_UNICODE_STRING DomainName;

        /// <summary>
        ///  The SID of the account domain. This field MUST NOT be
        ///  NULL.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _RPC_SID[] DomainSid;
    }

    /// <summary>
    /// The LSAPR_POLICY_MACHINE_ACCT_INFO structure is used to identify
    /// the machine account whose security policy is to be queried or set.
    /// </summary>
    public partial struct _LSAPR_POLICY_MACHINE_ACCT_INFO
    {

        /// <summary>
        ///  The RID of the machine account.
        /// </summary>
        public uint Rid;

        /// <summary>
        ///  The SID of the machine account.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _RPC_SID[] Sid;
    }

    /// <summary>
    ///  The POLICY_MODIFICATION_INFO structure is obsolete and
    ///  exists for backward compatibility purposes only. Callers
    ///  of this protocol MUST NOT be able to set or retrieve
    ///  this structure.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\c80ae9d5-d0c1-4d5c-a0ae-77eae7bfac25.xml
    //  </remarks>
    public partial struct _POLICY_MODIFICATION_INFO
    {

        /// <summary>
        ///  A 64-bit unsigned integer that is incremented each time
        ///  anything in the Local Security Authority (LSA) database
        ///  is modified.
        /// </summary>
        public _LARGE_INTEGER ModifiedId;

        /// <summary>
        ///  The date and time when the LSA database was created.
        ///  It is a 64-bit value that represents the number of
        ///  100-nanosecond intervals since January 1, 1601, UTC.
        /// </summary>
        public _LARGE_INTEGER DatabaseCreationTime;
    }

    /// <summary>
    ///  The LSAPR_AUTH_INFORMATION structure communicates information
    ///  about authentication between trusted domains. Domaintrust
    ///  authentication is specified in [MS-ADTS] section.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\cedb0d1b-c7c0-4480-99fc-279b06f22a0c.xml
    //  </remarks>
    public partial struct _LSAPR_AUTH_INFORMATION
    {

        /// <summary>
        ///  The date and time when this authentication information
        ///  was last updated. It is a 64-bit value that represents
        ///  the number of 100-nanosecond intervals since January
        ///  1, 1601, UTC.
        /// </summary>
        public _LARGE_INTEGER LastUpdateTime;

        /// <summary>
        ///  A type for the AuthInfo, as specified in the following
        ///  table.
        /// </summary>
        public AuthType_Values AuthType;

        /// <summary>
        ///  The count of bytes in AuthInfo buffer.The windowsRPC
        ///  server and client limit the AuthInfoLength field of
        ///  this structure to 65536 (using the range primitive
        ///  defined in [MS-RPCE]) in windows_xp_sp2, windows_server_2003,
        ///  windows_vista, and windows_server_2008, windows_vista,
        ///  windows_server_2008, windows_7, and windows_server_7.
        ///  Other versions do not enforce this restriction.
        /// </summary>
        public uint AuthInfoLength;

        /// <summary>
        ///  Authentication data that depends on the AuthType.The
        ///  self-relative form of the LSAPR_AUTH_INFORMATION structure
        ///  is used in LSAPR_TRUSTED_DOMAIN_AUTH_BLOB; in that
        ///  case, the structure memory layout looks like the following.01234567891
        ///  01234567892 01234567893 01LastUpdateTime LastUpdateTime
        ///  (continued)AuthTypeAuthInfoLengthAuthInfo [1 ... AuthInfoLength]
        /// </summary>
        [Size("AuthInfoLength")]
        public byte[] AuthInfo;
    }

    /// <summary>
    /// Possible type value for the AuthInfo
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum AuthType_Values : uint
    {

        /// <summary>
        ///  This type MUST be ignored.
        /// </summary>
        V1 = 0x00000000,

        /// <summary>
        ///  Derived RC4HMAC key. For more information, see [RFC4757].
        /// </summary>
        V2 = 0x00000001,

        /// <summary>
        ///  A plaintext password. Indicates that the information
        ///  stored in the attribute is a Unicode plaintext password.
        ///  If this AuthType is present, Kerberos can then use
        ///  this password to derive additional key types that are
        ///  needed to encrypt and decrypt cross-realm TGTs.
        /// </summary>
        V3 = 0x00000002,

        /// <summary>
        ///  A plaintext password version number that is a single,
        ///  unsigned long integer consisting of 32 bits.
        /// </summary>
        V4 = 0x00000003,
    }


    /// <summary>
    ///  The LSA_FOREST_TRUST_COLLISION_INFORMATION structure
    ///  is used to communicate a set of LSA_FOREST_TRUST_COLLISION_RECORD
    ///  structures.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\db0e9131-9a19-4cb5-937a-0e3a5767a0b2.xml
    //  </remarks>
    public partial struct _LSA_FOREST_TRUST_COLLISION_INFORMATION
    {

        /// <summary>
        ///  The count of elements in the Entries array.
        /// </summary>
        public uint RecordCount;

        /// <summary>
        ///  An array of LSA_FOREST_TRUST_COLLISION_RECORD structures.
        ///  If the RecordCount field has a value other than zero,
        ///  this field MUST NOT be NULL.
        /// </summary>
        [Size("RecordCount, 1")]
        public _LSA_FOREST_TRUST_COLLISION_RECORD[][] Entries;
    }

    /// <summary>
    ///  The LSAPR_USER_RIGHT_SET structure specifies a collection
    ///  of user rights.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\dcaca8ef-34a3-42dd-85b6-98363eb108ff.xml
    //  </remarks>
    public partial struct _LSAPR_USER_RIGHT_SET
    {

        /// <summary>
        ///  This field contains the number of rights.The windowsRPC
        ///  server and client limit the Entries field of this structure
        ///  to 256 (using the range primitive defined in [MS-RPCE])
        ///  in windows_xp_sp2, windows_server_2003, windows_vista,
        ///  and windows_server_2008, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7. Other versions do
        ///  not enforce this restriction.
        /// </summary>
        public uint Entries;

        /// <summary>
        ///  An array of strings specifying the rights. These may
        ///  be string names corresponding to either privilege names
        ///  or system access names, as specified in section. If
        ///  the Entries field has a value other than 0, this field
        ///  MUST NOT be NULL.
        /// </summary>
        [Size("Entries")]
        public _RPC_UNICODE_STRING[] UserRights;
    }

    /// <summary>
    ///  The LSAPR_TRUSTED_DOMAIN_INFORMATION_EX2 structure communicates
    ///  properties of a trusted domain. The following structure
    ///  corresponds to the TrustedDomainInformationEx2Internal
    ///  information class. Domaintrusts are specified in [MS-ADTS]
    ///  section.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\dd92d4d9-227f-4ef1-b42b-ef3f056f8aaa.xml
    //  </remarks>
    public partial struct _LSAPR_TRUSTED_DOMAIN_INFORMATION_EX2
    {

        /// <summary>
        ///  The DNS name of the domain. Maps to the Name field,
        ///  as specified in section.
        /// </summary>
        public _RPC_UNICODE_STRING Name;

        /// <summary>
        ///  The NetBIOS name of the trusted domain, as specified
        ///  in [RFC1088]. Maps to the Flat Name field, as specified
        ///  in section.
        /// </summary>
        public _RPC_UNICODE_STRING FlatName;

        /// <summary>
        ///  The domainSID. Maps to the Security Identifier field,
        ///  as specified in section.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _RPC_SID[] Sid;

        /// <summary>
        ///  This field contains bitmapped values that define the
        ///  properties of the direction of trust between the local
        ///  domain and the named domain. See section  for valid
        ///  values and a description of each flag. Maps to the
        ///  Trusted Direction field, as specified in section.
        /// </summary>
        public uint TrustDirection;

        /// <summary>
        ///  This field specifies the type of trust between the local
        ///  domain and the named domain. See section  for valid
        ///  values and a description of each value. Maps to the
        ///  Trusted Type field, as specified in section.
        /// </summary>
        public uint TrustType;

        /// <summary>
        ///  This field contains bitmapped values that define the
        ///  attributes of the trust. See section  for valid values
        ///  and a description of each flag. Maps to the Trusted
        ///  Attributes field, as specified in section.
        /// </summary>
        public uint TrustAttributes;

        /// <summary>
        ///  The count of bytes in ForestTrustInfo.
        /// </summary>
        public uint ForestTrustLength;

        /// <summary>
        ///  Binary data for the foresttrust. For more information,
        ///  see "Trust Objects" in [MS-ADTS] section. Maps to
        ///  the Forest Trust Information field, as specified in
        ///  section. Conversion from this binary format to the
        ///  LSA_FOREST_TRUST_INFORMATION format is specified in
        ///  [MS-ADTS] section. If the ForestTrustLength field
        ///  has a value other than 0, this field MUST NOT be NULL.
        /// </summary>
        [Size("ForestTrustLength")]
        public byte[] ForestTrustInfo;
    }

    /// <summary>
    ///  The LSAPR_TRUSTED_DOMAIN_INFORMATION_EX structure communicates
    ///  properties of a trusted domain. The following structure
    ///  corresponds to the TrustedDomainInformationEx information
    ///  class. Domaintrusts are specified in [MS-ADTS] section
    ///  .
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\f28f42b7-173c-4cda-9809-3fe4a5213ab3.xml
    //  </remarks>
    public partial struct _LSAPR_TRUSTED_DOMAIN_INFORMATION_EX
    {

        /// <summary>
        ///  The DNS name of the domain. Maps to the Name field,
        ///  as specified in section.
        /// </summary>
        public _RPC_UNICODE_STRING Name;

        /// <summary>
        ///  The NetBIOS name of the trusted domain, as specified
        ///  in [RFC1088]. Maps to the Flat Name field, as specified
        ///  in section.
        /// </summary>
        public _RPC_UNICODE_STRING FlatName;

        /// <summary>
        ///  The domainSID. Maps to the Security Identifier field,
        ///  as specified in section.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _RPC_SID[] Sid;

        /// <summary>
        ///  This field contains bitmapped values that define the
        ///  properties of the direction of trust between the local
        ///  domain and the named domain. One or more of the valid
        ///  flags can be set. If all bits are 0, the trust is said
        ///  to be disabled.01234567891012345678920123456789301000000000000000000000000000000OII:
        ///  The trust is inbound.O: The trust is outbound.All other
        ///  bits SHOULD be 0 and ignored upon receipt.Maps to the
        ///  Trust Direction field, as specified in section.
        /// </summary>
        public uint TrustDirection;

        /// <summary>
        ///  This field specifies the type of trust between the local
        ///  domain and the named domain.
        /// </summary>
        public TrustType_Values TrustType;

        /// <summary>
        ///  This field contains bitmapped values that define the
        ///  attributes of the trust.The following is a timeline
        ///  of when each flag value was introduced. Unless otherwise
        ///  specified, all flag values continue to be available
        ///  in subsequent versions of windows according to the
        ///  applicability list at the beginning of this section.Possible
        ///  valueValueProductTANT (TRUST_ATTRIBUTE_NON_TRANSITIVE)0x00000001windows_2000.TAUO
        ///  (TRUST_ATTRIBUTE_UPLEVEL_ONLY)0x00000002windows_2000.TAQD
        ///  (TRUST_ATTRIBUTE_QUARANTINED_DOMAIN)0x00000004windows_2000_sp2
        ///  and windows_xp.TAFT (TRUST_ATTRIBUTE_FOREST_TRANSITIVE)0x00000008windows_server_2003.TACO
        ///  (TRUST_ATTRIBUTE_CROSS_ORGANIZATION)0x00000010windows_server_2003.TAWF
        ///  (TRUST_ATTRIBUTE_WITHIN_FOREST)0x00000020windows_server_2003.TATE
        ///  (TRUST_ATTRIBUTE_TREAT_AS_EXTERNAL)0x00000040windows_server_2003.Obsolete0x00400000Introduced
        ///  in windows_2000 RTM. Became obsolete in windows_2000_sp4.Obsolete0x00800000Introduced
        ///  in windows_2000 RTM. Became obsolete in windows_2000_sp4.01234567891012345678920123456789301RRRRRRRROORRRRRRRRRRRRRRTARCTATETAWFTACOTAFTTAQDTAUOTANTTrustAttribute
        ///  values are described in section. The following table
        ///  shows how these values map to the Trust Attributes
        ///  field in section.ValueMappingTANT (TRUST_ATTRIBUTE_NON_TRANSITIVE)Trust
        ///  Attributes: Non-transitiveTAUO (TRUST_ATTRIBUTE_UPLEVEL_ONLY)Trust
        ///  Attributes: Uplevel onlyTAQD (TRUST_ATTRIBUTE_QUARANTINED_DOMAIN)Trust
        ///  Attributes: QuarantinedTAFT (TRUST_ATTRIBUTE_FOREST_TRANSITIVE)Trust
        ///  Attributes: Forest trustTACO (TRUST_ATTRIBUTE_CROSS_ORGANIZATION)Trust
        ///  Attributes: Cross organizationTAWF (TRUST_ATTRIBUTE_WITHIN_FOREST)Trust
        ///  Attributes: Within forestTATE (TRUST_ATTRIBUTE_TREAT_AS_EXTERNAL)Trust
        ///  Attributes: Treat as externalTARC (TRUST_ATTRIBUTE_USES_RC4_ENCRYPTION)Trust
        ///  Attributes: Use RC4 EncryptionOObsolete. SHOULD be
        ///  set to 0.RReserved for future use. SHOULD be set to
        ///  zero.
        /// </summary>
        public uint TrustAttributes;
    }

    /// <summary>
    /// Possible values for TrustType
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum TrustType_Values : uint
    {

        /// <summary>
        ///  Trust with a windowsdomain that is not running Active
        ///  Directory.
        /// </summary>
        NoneActiveDirectory = 0x00000001,

        /// <summary>
        ///  Trust with a windowsdomain that is running Active Directory.
        /// </summary>
        ActiveDirectory = 0x00000002,

        /// <summary>
        ///  Trust with a non–windows-compliant Kerberos distribution,
        ///  as specified in [RFC4120].
        /// </summary>
        NonWindowsCompiantKerberos = 0x00000003,

        /// <summary>
        ///  Trust with a distributed computing environment (DCE)
        ///  realm. This is a historical reference and is not used.
        /// </summary>
        DCERealm = 0x00000004,
    }

    /// <summary>
    ///  The LSAPR_POLICY_PRIVILEGE_DEF structure specifies a
    ///  privilege definition, which consists of a pairing of
    ///  a human-readable name with a locally unique identifier
    ///  (LUID).
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\f36d4737-5b2f-4bc0-8f29-e7b4c71b7401.xml
    //  </remarks>
    public partial struct _LSAPR_POLICY_PRIVILEGE_DEF
    {

        /// <summary>
        ///  An RPC_UNICODE_STRING that contains the privilege name.
        /// </summary>
        public _RPC_UNICODE_STRING Name;

        /// <summary>
        ///  This field contains the LUID value assigned locally
        ///  for efficient representation of the privilege. This
        ///  value is meaningful only on the system where it was
        ///  assigned.
        /// </summary>
        public _LUID LocalValue;
    }

    /// <summary>
    ///  The LSAPR_POLICY_REPLICA_SRCE_INFO structure is obsolete
    ///  and exists for backward compatibility purposes only.
    ///  This structure corresponds to the PolicyReplicaSourceInformation
    ///  information class.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\fb7df2bb-99e7-402f-8334-24d47e23ec00.xml
    //  </remarks>
    public partial struct _LSAPR_POLICY_REPLICA_SRCE_INFO
    {

        /// <summary>
        ///  Not used.
        /// </summary>
        public _RPC_UNICODE_STRING ReplicaSource;

        /// <summary>
        ///  Not used.
        /// </summary>
        public _RPC_UNICODE_STRING ReplicaAccountName;
    }

    /// <summary>
    ///  The LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION structure
    ///  communicates authentication material. The following
    ///  structure corresponds to the TrustedDomainAuthInformation
    ///  information class. Domaintrust authentication is specified
    ///  in [MS-ADTS] section. This structure maps to the Incoming
    ///  and Outgoing Trust Password fields, as specified in
    ///  section.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\084fdb6b-5bc3-4912-9aed-0257159996dd.xml
    //  </remarks>
    public partial struct _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION
    {

        /// <summary>
        ///  The count of LSAPR_AUTH_INFORMATION entries (section
        ///  ) in the IncomingAuthenticationInformation field.The
        ///  windows RPC server and client limit the IncomingAuthInfos
        ///  field of this structure to be 0 or 1 (using the range
        ///  primitive defined in [MS-RPCE]) in windows_7 and windows_server_7.
        ///  Other versions do not enforce this restriction.
        /// </summary>
        public uint IncomingAuthInfos;

        /// <summary>
        ///  An array of LSAPR_AUTH_INFORMATION structures. The values
        ///  are used to compute keys used in inbound trust validation,
        ///  as specified in [MS-ADTS] section.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _LSAPR_AUTH_INFORMATION[] IncomingAuthenticationInformation;

        /// <summary>
        ///  Same as IncomingAuthenticationInformation, but the data
        ///  is the previous version of the authentication information.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _LSAPR_AUTH_INFORMATION[] IncomingPreviousAuthenticationInformation;

        /// <summary>
        ///  The count of LSAPR_AUTH_INFORMATION entries in the OutgoingAuthenticationInformation
        ///  field. The windows RPC server and client limit the
        ///  OutgoingAuthInfos field of this structure to be 0 or
        ///  1 (using the range primitive defined in [MS-RPCE])
        ///  in windows_7 and windows_server_7. Other versions do
        ///  not enforce this restriction.
        /// </summary>
        public uint OutgoingAuthInfos;

        /// <summary>
        ///  An array of LSAPR_AUTH_INFORMATION structures. The values
        ///  are used to compute keys used in outbound trust validation,
        ///  as specified in [MS-ADTS] section.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _LSAPR_AUTH_INFORMATION[] OutgoingAuthenticationInformation;

        /// <summary>
        ///  Same as OutgoingAuthenticationInformation, but the data
        ///  is the previous version of the authentication information.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _LSAPR_AUTH_INFORMATION[] OutgoingPreviousAuthenticationInformation;
    }

    /// <summary>
    ///  _nested_ForestTrustData__LSA_FOREST_TRUST_RECORD union.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\08cf1a65-ef7c-46d3-aa4d-558f5135df3d.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct _nested_ForestTrustData__LSA_FOREST_TRUST_RECORD
    {

        /// <summary>
        ///  TopLevelName member.
        /// </summary>
        [Case("0,1")]
        public _RPC_UNICODE_STRING TopLevelName;

        /// <summary>
        ///  DomainInfo member.
        /// </summary>
        [Case("2")]
        public _LSA_FOREST_TRUST_DOMAIN_INFO DomainInfo;

        /// <summary>
        ///  Data member.
        /// </summary>
        [CaseDefault()]
        public _LSA_FOREST_TRUST_BINARY_DATA Data;
    }

    /// <summary>
    ///  The LSA_FOREST_TRUST_RECORD structure is used to communicate
    ///  the type, creation time, and data for a foresttrust
    ///  record. The data is determined by the trust type as
    ///  follows in the definition of the contained union.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\08cf1a65-ef7c-46d3-aa4d-558f5135df3d.xml
    //  </remarks>
    public partial struct _LSA_FOREST_TRUST_RECORD
    {

        /// <summary>
        ///  The following table specifies the possible flags.Some
        ///  flag values are reused for different forest record
        ///  types. See the Meaning column for more information.
        /// </summary>
        public Flags_Values Flags;

        /// <summary>
        ///  This value is one of LSA_FOREST_TRUST_RECORD_TYPE.
        /// </summary>
        public _LSA_FOREST_TRUST_RECORD_TYPE ForestTrustType;

        /// <summary>
        ///  The date and time when this entry was created. It is
        ///  a 64-bit value that represents the number of 100-nanosecond
        ///  intervals since January 1, 1601, UTC.
        /// </summary>
        public _LARGE_INTEGER Time;

        /// <summary>
        ///  An LSA_UNICODE_STRING or LSA_FOREST_TRUST_DOMAIN_INFO
        ///  structure, depending on the value ForestTrustType as
        ///  specified in the structure definition for LSA_FOREST_TRUST_RECORD.
        /// </summary>
        [Switch("ForestTrustType")]
        public _nested_ForestTrustData__LSA_FOREST_TRUST_RECORD ForestTrustData;
    }


    /// <summary>
    ///  The LSAPR_POLICY_DOMAIN_INFORMATION union is defined
    ///  as follows, where the structure depends on the POLICY_DOMAIN_INFORMATION_CLASS
    ///  that is specified in the message.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\1a9c523b-a67a-485f-8f8b-8fca05ca9334.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct _LSAPR_POLICY_DOMAIN_INFORMATION
    {

        /// <summary>
        ///  The complete description is as specified in section
        ///  .The PolicyDomainQualityOfServiceInformation enumeration
        ///  value and corresponding POLICY_DOMAIN_QUALITY_OF_SERVICE_INFO
        ///  structure are parts of LSAPR_POLICY_DOMAIN_INFORMATION
        ///  only in the windows_2000_server implementation of this
        ///  protocol. windows_nt_3_1, windows_nt_3_5, windows_nt_3_51,
        ///  windows_nt_4_0, windows_xp, windows_server_2003, windows_vista,
        ///  and windows_server_2008, windows_vista, windows_server_2008,
        ///  windows_7, and windows_server_7 implementations do
        ///  not contain this enumeration value and corresponding
        ///  structure in LSAPR_POLICY_DOMAIN_INFORMATION.
        /// </summary>
        [Case("1")]
        public _POLICY_DOMAIN_QUALITY_OF_SERVICE_INFO PolicyDomainQualityOfServiceInfo;

        /// <summary>
        ///  The complete description is as specified in section
        ///  .
        /// </summary>
        [Case("2")]
        public _LSAPR_POLICY_DOMAIN_EFS_INFO PolicyDomainEfsInfo;

        /// <summary>
        ///  The complete description is as specified in section
        ///  .
        /// </summary>
        [Case("3")]
        public _POLICY_DOMAIN_KERBEROS_TICKET_INFO PolicyDomainKerbTicketInfo;
    }

    /// <summary>
    ///  The LSA_FOREST_TRUST_INFORMATION structure is a collection
    ///  of LSA_FOREST_TRUST_RECORD structures.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\2993ffab-c0c8-4643-9a79-4ff7d31922dc.xml
    //  </remarks>
    public partial struct _LSA_FOREST_TRUST_INFORMATION
    {

        /// <summary>
        ///  A count of elements in the Entries array.The windowsRPC
        ///  server and client limit the RecordCount field of this
        ///  structure to 4000 (using the range primitive defined
        ///  in [MS-RPCE]) in windows_xp_sp2, windows_server_2003,
        ///  windows_vista, and windows_server_2008, windows_vista,
        ///  windows_server_2008, windows_7, and windows_server_7.
        ///  Other versions do not enforce this restriction.
        /// </summary>
        public uint RecordCount;

        /// <summary>
        ///  An array of LSA_FOREST_TRUST_RECORD structures. If the
        ///  RecordCount field has a value other than 0, this field
        ///  MUST NOT be NULL.
        /// </summary>
        [Size("RecordCount, 1")]
        public _LSA_FOREST_TRUST_RECORD[][] Entries;
    }

    /// <summary>
    ///  The LSAPR_TRUSTED_DOMAIN_FULL_INFORMATION_INTERNAL structure
    ///  communicates identification and authentication information
    ///  for a trusted domain. The following structure corresponds
    ///  to the TrustedDomainFullInformationInternal information
    ///  class.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\2e9e2c84-7b00-4fb1-8de5-88d4cfedd2b3.xml
    //  </remarks>
    public partial struct _LSAPR_TRUSTED_DOMAIN_FULL_INFORMATION_INTERNAL
    {

        /// <summary>
        ///  A structure containing name, SID, and trust attributes,
        ///  as specified in section.
        /// </summary>
        public _LSAPR_TRUSTED_DOMAIN_INFORMATION_EX Information;

        /// <summary>
        ///  Any offset required for POSIX compliance, as specified
        ///  in section.
        /// </summary>
        public _TRUSTED_POSIX_OFFSET_INFO PosixOffset;

        /// <summary>
        ///  Authentication material, as specified in section.
        /// </summary>
        public _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION_INTERNAL AuthInformation;
    }

    /// <summary>
    ///  The LSAPR_POLICY_INFORMATION union is defined as follows,
    ///  where the structure depends on the POLICY_INFORMATION_CLASS
    ///  specified in this message.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\6e63a2c8-5ddb-411a-a253-9c55afc49834.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct _LSAPR_POLICY_INFORMATION
    {

        /// <summary>
        ///  PolicyAuditLogInfo member.
        /// </summary>
        [Case("1")]
        public _POLICY_AUDIT_LOG_INFO PolicyAuditLogInfo;

        /// <summary>
        ///  PolicyAuditEventsInfo member.
        /// </summary>
        [Case("2")]
        public _LSAPR_POLICY_AUDIT_EVENTS_INFO PolicyAuditEventsInfo;

        /// <summary>
        ///  PolicyPrimaryDomainInfo member.
        /// </summary>
        [Case("3")]
        public _LSAPR_POLICY_PRIMARY_DOM_INFO PolicyPrimaryDomainInfo;

        /// <summary>
        ///  PolicyAccountDomainInfo member.
        /// </summary>
        [Case("5")]
        public _LSAPR_POLICY_ACCOUNT_DOM_INFO PolicyAccountDomainInfo;

        /// <summary>
        ///  PolicyPdAccountInfo member.
        /// </summary>
        [Case("4")]
        public _LSAPR_POLICY_PD_ACCOUNT_INFO PolicyPdAccountInfo;

        /// <summary>
        ///  PolicyServerRoleInfo member.
        /// </summary>
        [Case("6")]
        public _POLICY_LSA_SERVER_ROLE_INFO PolicyServerRoleInfo;

        /// <summary>
        ///  PolicyReplicaSourceInfo member.
        /// </summary>
        [Case("7")]
        public _LSAPR_POLICY_REPLICA_SRCE_INFO PolicyReplicaSourceInfo;

        /// <summary>
        ///  PolicyModificationInfo member.
        /// </summary>
        [Case("9")]
        public _POLICY_MODIFICATION_INFO PolicyModificationInfo;

        /// <summary>
        ///  PolicyAuditFullSetInfo member.
        /// </summary>
        [Case("10")]
        public _POLICY_AUDIT_FULL_SET_INFO PolicyAuditFullSetInfo;

        /// <summary>
        ///  PolicyAuditFullQueryInfo member.
        /// </summary>
        [Case("11")]
        public _POLICY_AUDIT_FULL_QUERY_INFO PolicyAuditFullQueryInfo;

        /// <summary>
        ///  PolicyDnsDomainInfo member.
        /// </summary>
        [Case("12")]
        public _LSAPR_POLICY_DNS_DOMAIN_INFO PolicyDnsDomainInfo;

        /// <summary>
        ///  PolicyDnsDomainInfoInt member.
        /// </summary>
        [Case("13")]
        public _LSAPR_POLICY_DNS_DOMAIN_INFO PolicyDnsDomainInfoInt;

        /// <summary>
        ///  PolicyLocalAccountDomainInfo member.
        /// </summary>
        [Case("14")]
        public _LSAPR_POLICY_ACCOUNT_DOM_INFO PolicyLocalAccountDomainInfo;

        /// <summary>
        ///  PolicyMachineAccountInfo member.
        /// </summary>
        [Case("15")]
        public _LSAPR_POLICY_MACHINE_ACCT_INFO PolicyMachineAccountInfo;
    }

    /// <summary>
    ///  The LSAPR_ACCOUNT_ENUM_BUFFER structure specifies a
    ///  collection of security principalSIDs represented in
    ///  an array of structures of type LSAPR_ACCOUNT_INFORMATION.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\727c2d44-8794-4896-9fba-5e1725bc288e.xml
    //  </remarks>
    public partial struct _LSAPR_ACCOUNT_ENUM_BUFFER
    {

        /// <summary>
        ///  This field contains the number of security principals.
        /// </summary>
        public uint EntriesRead;

        /// <summary>
        ///  This field contains a set of structures that define
        ///  the security principalSID, as specified in section
        ///  . If the EntriesRead field has a value other than 0,
        ///  this field MUST NOT be NULL.
        /// </summary>
        [Size("EntriesRead")]
        public _LSAPR_ACCOUNT_INFORMATION[] Information;
    }

    /// <summary>
    ///  The LSAPR_TRUSTED_DOMAIN_FULL_INFORMATION structure
    ///  communicates identification, POSIX compatibility, and
    ///  authentication information for a trusted domain. The
    ///  following structure corresponds to the TrustedDomainFullInformation
    ///  information class.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\9f9feebc-e9e1-41c1-8c48-02f83a227a14.xml
    //  </remarks>
    public partial struct _LSAPR_TRUSTED_DOMAIN_FULL_INFORMATION
    {

        /// <summary>
        ///  A structure containing name, SID, and trust attributes,
        ///  as specified in section.
        /// </summary>
        public _LSAPR_TRUSTED_DOMAIN_INFORMATION_EX Information;

        /// <summary>
        ///  Any offset required for POSIX compliance, as specified
        ///  in section.
        /// </summary>
        public _TRUSTED_POSIX_OFFSET_INFO PosixOffset;

        /// <summary>
        ///  Authentication material, as specified in section.
        /// </summary>
        public _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION AuthInformation;
    }

    /// <summary>
    ///  The LSAPR_TRUSTED_ENUM_BUFFER_EX structure specifies
    ///  a collection of trust information structures of type
    ///  LSAPR_TRUSTED_DOMAIN_INFORMATION_EX.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\b9b29ed6-786e-483f-9e3b-776eb014086b.xml
    //  </remarks>
    public partial struct _LSAPR_TRUSTED_ENUM_BUFFER_EX
    {

        /// <summary>
        ///  This field contains the number of trust information
        ///  structures.
        /// </summary>
        public uint EntriesRead;

        /// <summary>
        ///  This field contains a set of structures that define
        ///  the trust information, as specified in section. If
        ///  the EntriesRead field has a value other than 0, this
        ///  field MUST NOT be NULL.
        /// </summary>
        [Size("EntriesRead")]
        public _LSAPR_TRUSTED_DOMAIN_INFORMATION_EX[] EnumerationBuffer;
    }


    /// <summary>
    ///  The LSAPR_PRIVILEGE_ENUM_BUFFER structure specifies
    ///  a collection of privilege definitions of type LSAPR_POLICY_PRIVILEGE_DEF.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\c0278280-b4b6-4538-b3aa-eb40f64f42fb.xml
    //  </remarks>
    public partial struct _LSAPR_PRIVILEGE_ENUM_BUFFER
    {

        /// <summary>
        ///  This field contains the number of privileges in the
        ///  structure.
        /// </summary>
        public uint Entries;

        /// <summary>
        ///  This field contains a set of structures that define
        ///  the privileges, as specified in section. If the Entries
        ///  field has a value other than 0, this field MUST NOT
        ///  be NULL.
        /// </summary>
        [Size("Entries")]
        public _LSAPR_POLICY_PRIVILEGE_DEF[] Privileges;
    }

    /// <summary>
    ///  The LSAPR_TRUSTED_DOMAIN_FULL_INFORMATION2 structure
    ///  is used to communicate identification, POSIX compatibility,
    ///  and authentication information for a trusted domain.
    ///  The following structure corresponds to the TrustedDomainFullInformation2Internal
    ///  information class.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\e529d094-5de4-4738-adc4-efa1a7d1106f.xml
    //  </remarks>
    public partial struct _LSAPR_TRUSTED_DOMAIN_FULL_INFORMATION2
    {

        /// <summary>
        ///  A structure containing name, SID, and trust attributes,
        ///  as specified in section.
        /// </summary>
        public _LSAPR_TRUSTED_DOMAIN_INFORMATION_EX2 Information;

        /// <summary>
        ///  Any offset required for POSIX compliance, as specified
        ///  in section.
        /// </summary>
        public _TRUSTED_POSIX_OFFSET_INFO PosixOffset;

        /// <summary>
        ///  Authentication material, as specified in section.
        /// </summary>
        public _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION AuthInformation;
    }

    /// <summary>
    ///  The LSAPR_TRUSTED_DOMAIN_INFO union is defined as follows,
    ///  where the structure depends on the TRUSTED_INFORMATION_CLASS
    ///  that is specified in the message.
    /// </summary>
    //  <remarks>
    //   .\TD\MS-LSAD\65564571-dd0d-49a9-8a2a-6dba8ab57091.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct _LSAPR_TRUSTED_DOMAIN_INFO
    {

        /// <summary>
        ///  TrustedDomainNameInfo member.
        /// </summary>
        [Case("1")]
        public _LSAPR_TRUSTED_DOMAIN_NAME_INFO TrustedDomainNameInfo;

        /// <summary>
        ///  TrustedControllersInfo member.
        /// </summary>
        [Case("2")]
        public _LSAPR_TRUSTED_CONTROLLERS_INFO TrustedControllersInfo;

        /// <summary>
        ///  TrustedPosixOffsetInfo member.
        /// </summary>
        [Case("3")]
        public _TRUSTED_POSIX_OFFSET_INFO TrustedPosixOffsetInfo;

        /// <summary>
        ///  TrustedPasswordInfo member.
        /// </summary>
        [Case("4")]
        public _LSAPR_TRUSTED_PASSWORD_INFO TrustedPasswordInfo;

        /// <summary>
        ///  TrustedDomainInfoBasic member.
        /// </summary>
        [Case("5")]
        public _LSAPR_TRUST_INFORMATION TrustedDomainInfoBasic;

        /// <summary>
        ///  TrustedDomainInfoEx member.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
        [Case("6")]
        public _LSAPR_TRUSTED_DOMAIN_INFORMATION_EX TrustedDomainInfoEx;

        /// <summary>
        ///  TrustedAuthInfo member.
        /// </summary>
        [Case("7")]
        public _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION TrustedAuthInfo;

        /// <summary>
        ///  TrustedFullInfo member.
        /// </summary>
        [Case("8")]
        public _LSAPR_TRUSTED_DOMAIN_FULL_INFORMATION TrustedFullInfo;

        /// <summary>
        ///  TrustedAuthInfoInternal member.
        /// </summary>
        [Case("9")]
        public _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION_INTERNAL TrustedAuthInfoInternal;

        /// <summary>
        ///  TrustedFullInfoInternal member.
        /// </summary>
        [Case("10")]
        public _LSAPR_TRUSTED_DOMAIN_FULL_INFORMATION_INTERNAL TrustedFullInfoInternal;

        /// <summary>
        ///  TrustedDomainInfoEx2 member.
        /// </summary>
        [Case("11")]
        public _LSAPR_TRUSTED_DOMAIN_INFORMATION_EX2 TrustedDomainInfoEx2;

        /// <summary>
        ///  TrustedFullInfo2 member.
        /// </summary>
        [Case("12")]
        public _LSAPR_TRUSTED_DOMAIN_FULL_INFORMATION2 TrustedFullInfo2;

        /// <summary>
        ///  TrustedDomainSETs member.
        /// </summary>
        [Case("13")]
        public _TRUSTED_DOMAIN_SUPPORTED_ENCRYPTION_TYPES TrustedDomainSETs;
    }
}
