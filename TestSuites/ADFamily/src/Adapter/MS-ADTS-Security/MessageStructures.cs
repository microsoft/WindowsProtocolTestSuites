// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Modeling;

namespace ProtocolMessageStructures
{

    /// <summary>
    /// Structure TRUSTED_DOMAIN_OBJECT stores the information
    /// pertaining to the attributes of a TDO object.
    /// It stores the following:
    /// FlatName: The flatName attribute contains the NetBIOS name of the 
    ///           trusted domain in String(Unicode) syntax
    /// TrustPartner: This String(Unicode) attribute contains the DNS domain name 
    ///               of the trusted domain          
    /// isCriticalSysObj: mandatory boolean attribute of a TDO
    /// TrustAttr: Trust Attributes
    /// TrustDir: Trust Direction
    /// TrustDomain_Sid: Security Identifier of the trust domain
    /// TrustType: Type of trust established
    /// SupportedEncryptionTypes: Encryption Types as supported by Windows 2008
    /// recType: recType is of type RecordType. Section 7.1.6.9.3.1 specifies the possible entries
    /// for record type field of Records
    /// </summary>
    public class TRUSTED_DOMAIN_OBJECT : CompoundValue
    {
        /// <summary>
        /// FlatName: The flatName attribute contains the NetBIOS name of the 
        ///           trusted domain in String(Unicode) syntax
        /// </summary>
        public string FlatName;

        /// <summary>
        /// TrustPartner: This String(Unicode) attribute contains the DNS domain name 
        ///               of the trusted domain
        /// </summary>
        public string TrustPartner;

        /// <summary>
        /// TrustAttr: Trust Attributes
        /// </summary>
        public TrustAttributesEnum TrustAttr;

        /// <summary>
        /// TrustDir: Trust Direction
        /// </summary>
        public TrustDirectionEnum TrustDir;

        /// <summary>
        /// TrustDomain_Sid: Security Identifier of the trust domain
        /// </summary>
        public string TrustDomain_Sid;

        /// <summary>
        /// TrustType: Type of trust established
        /// </summary>
        public TrustTypeEnum TrustType;

        /// <summary>
        /// SupportedEncryptionTypes: Encryption Types as supported by Windows 2008
        /// </summary>
        public SupportedEncryptionTypes EncryptionType;



        /// <summary>
        /// Parameterized constructor for TRUSTED_DOMAIN_OBJECT compound value
        /// </summary>
        /// <param name="flatName">TDO FlatName</param>
        /// <param name="trustPartner">This String(Unicode) attribute contains the DNS domain name 
        ///               of the trusted domain</param>
        /// <param name="trustAttr">Trust Attributes</param>
        /// <param name="trustDir">Trust Direction</param>
        /// <param name="domain_Sid">Security Identifier of the trust domain</param>
        /// <param name="trustType">Type of trust established</param>
        /// <param name="encryptionType">Encryption Types as supported by Windows 2008</param>
        public TRUSTED_DOMAIN_OBJECT(string flatName,
                                     string trustPartner,
                                     TrustAttributesEnum trustAttr,
                                     TrustDirectionEnum trustDir,
                                     string domain_Sid,
                                     TrustTypeEnum trustType,
                                     SupportedEncryptionTypes encryptionType)
        {
            this.FlatName = flatName;
            this.TrustPartner = trustPartner;
            this.TrustAttr = trustAttr;
            this.TrustDir = trustDir;
            this.TrustDomain_Sid = domain_Sid;
            this.TrustType = trustType;
            this.EncryptionType = encryptionType;
        }

        /// <summary>
        /// constructor
        /// </summary>
        public TRUSTED_DOMAIN_OBJECT() 
        {
        }
    }

    /// <summary>
    /// Structure InterDomain_Trust_Info stores the inter domain trust information when
    /// when the trustDirection attribute of a TDO equals to TRUST_DIRECTION_INBOUND or 
    /// TRUST_DIRECTION_BIDIRECTIONAL
    /// 
    /// If trustDirection != TRUST_DIRECTION_INBOUND 
    ///     || 
    ///    trustDirection != TRUST_DIRECTION_BIDIRECTIONAL
    /// then 
    /// the attribute values are not set
    /// cnAttribute = null
    /// sAMAccName = null
    /// interDomainAccType = sAMAccountType.NotSet
    /// userAccountControl = userAccountControl.NotSet
    /// </summary>
    public class InterDomain_Trust_Info : CompoundValue
    {
        /// <summary>
        /// The RDN of an inter domain trust account attribute, contains the NetBIOS name 
        /// of the trusted domain account
        /// </summary>
        public string cnAttribute;

        /// <summary>
        /// The sAMAccountName attribute contains the NetBIOS name 
        /// of the trusted domain account 
        /// </summary>
        public string sAMAccName;

        /// <summary>
        /// In a domain trust account, the sAMAccountType attribute must have the value 
        /// SAM_TRUST_ACCOUNT, in the Enumeration syntax
        /// </summary>
        public sAMAccountType interDomainAccType;

        /// <summary>
        /// In a domain trust account, the userAccountControl attribute must have the flag 
        /// ADS_UF_INTERDOMAIN_TRUST_ACCOUNT set
        /// </summary>
        public userAccountControl accControl;

        /// <summary>
        /// Parameterized constructor for InterDomain_Trust_Info compound value
        /// </summary>
        /// <param name="cn">The RDN of an inter domain trust account attribute, contains the NetBIOS name 
        /// of the trusted domain account</param>
        /// <param name="accName">The sAMAccountName attribute contains the NetBIOS name 
        /// of the trusted domain account </param>
        /// <param name="accType">In a domain trust account, the sAMAccountType attribute must have the value 
        /// SAM_TRUST_ACCOUNT, in the Enumeration syntax</param>
        /// <param name="userAccCtrl">In a domain trust account, the userAccountControl attribute must have the flag 
        /// ADS_UF_INTERDOMAIN_TRUST_ACCOUNT set</param>
        public InterDomain_Trust_Info(string cn,
                                      string accName,
                                      sAMAccountType accType,
                                      userAccountControl userAccCtrl)
        {
            this.cnAttribute = cn;
            this.sAMAccName = accName;
            this.interDomainAccType = accType;
            this.accControl = userAccCtrl;
        }

        /// <summary>
        /// constructor
        /// </summary>
        public InterDomain_Trust_Info() { }
    }
    /// <summary>
    /// Signifies whether the SDC Flag is set
    /// </summary>
    public enum SDCFlag
    {
        /// <summary>
        /// set SDC flag
        /// </summary>
        Set,

        /// <summary>
        /// not set SDC flag
        /// </summary>
        NotSet
    }

    /// <summary>
    /// Signifies whether NDC Flag is set
    /// </summary>
    public enum NDCFlag
    {
        /// <summary>
        /// set NDC flag
        /// </summary>
        Set,

        /// <summary>
        /// not set NDC flag
        /// </summary>
        NotSet
    }

    /// <summary>
    /// define the authority
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue"), Flags()]
    public enum Value_Values : byte
    {

        /// <summary>
        ///  The authority is the NULL SID authority. It defines
        ///  only the NULL  well-known-SID: S-1-0-0.
        /// </summary>
        NULL_SID_AUTHORITY = 0x00,

        /// <summary>
        ///  The authority is the World  SID authority. It only defines
        ///   the Everyone well-known-SID: S-1-1-0.
        /// </summary>
        WORLD_SID_AUTHORITY = 0x01,

        /// <summary>
        ///  The authority is the Local  SID authority. It defines
        ///  only the Local  well-known-SID: S-1-2-0.
        /// </summary>
        LOCAL_SID_AUTHORITY = 0x02,

        /// <summary>
        ///  The authority is the Creator SID authority. It defines
        ///  the Creator Owner, Creator Group, and Creator  Owner
        ///  Server well-known-SIDs: S-1-3-0, S-1-3-1, and S-1-3-2.
        ///  These SIDs are used as placeholders in an access control
        ///  list (ACL) and are replaced by the  user, group, and
        ///  machine SIDs of the security principal.
        /// </summary>
        CREATOR_SID_AUTHORITY = 0x03,

        /// <summary>
        ///  Not used.
        /// </summary>
        NON_UNIQUE_AUTHORITY = 0x04,

        /// <summary>
        ///  The authority is the security subsystem SID authority.
        ///  It defines all other SIDs in the forest.
        /// </summary>
        NT_AUTHORITY = 0x05,
    }

    /// <summary>
    /// Flags_Values
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue"), Flags()]
    public enum Flags_Values : uint
    {

        /// <summary>
        ///  The top-level name trust is disabled due to a conflict. This
        ///  flag MUST be used only with forest trust record types
        ///  of ForestTrustTopLevelName and ForestTrustTopLevelNameEx.
        /// </summary>
        LSA_TLN_DISABLED_CONFLICT = 0x00000004,

        /// <summary>
        ///  The domain information trust record's trust is disabled
        ///  by the domain administrator. This flag MUST be used
        ///  only with a forest trust record type of ForestTrustDomainInfo.
        /// </summary>
        LSA_SID_DISABLED_ADMIN = 0x00000001,

        /// <summary>
        ///  The domain information trust record's trust is disabled
        ///  due to a conflict. This flag MUST be used only with
        ///  a forest trust record type of ForestTrustDomainInfo.
        /// </summary>
        LSA_SID_DISABLED_CONFLICT = 0x00000002,

        /// <summary>
        ///  The domain information trust record's trust is disabled
        ///  by the domain administrator. This flag MUST be used
        ///  only with a forest trust record type of ForestTrustDomainInfo.
        /// </summary>
        LSA_NB_DISABLED_ADMIN = 0x00000004,

        /// <summary>
        ///  The domain information trust record's trust is disabled
        ///  due to a conflict. This flag MUST be used only with
        ///  a forest trust record type of ForestTrustDomainInfo.
        /// </summary>
        LSA_NB_DISABLED_CONFLICT = 0x00000008,

        /// <summary>
        ///  The domain information trust record's trust is disabled. This
        ///  set of flags is reserved; for current and future reasons,
        ///  the trust is disabled.
        /// </summary>
        LSA_FTRECORD_DISABLED_REASONS = 0x0000ffff,

        /// <summary>
        /// None
        /// </summary>
        LSA_NULL = 0x00000000,
    }


    /// <summary>
    /// sAMAccountType is of type enumerator which stores the probable values for attribute sAMAccountType
    /// with respect to the Test Suite
    /// In a domain trust account, the sAMAccountType attribute must have the value SAM_TRUST_ACCOUNT 
    /// (0x30000002), in the Enumeration syntax
    /// </summary>
    public enum sAMAccountType
    {
        /// <summary>
        /// trust account
        /// </summary>
        SAM_TRUST_ACCOUNT,

        /// <summary>
        /// not set
        /// </summary>
        NotSet
    }

    /// <summary>
    /// userAccountControl is of type enumerator which stores the probable values for attribute 
    /// userAccountControl with respect to the Test Suite.
    /// In a domain trust account, the userAccountControl attribute must have the flag 
    /// ADS_UF_INTERDOMAIN_TRUST_ACCOUNT (0x00000800) set
    /// </summary>
    public enum userAccountControl
    {
        /// <summary>
        /// ADS_UF_INTERDOMAIN_TRUST_ACCOUNT
        /// </summary>
        ADS_UF_INTERDOMAIN_TRUST_ACCOUNT,

        /// <summary>
        /// not set
        /// </summary>
        NotSet
    }

    /// <summary>
    /// The RecordType field of Record specifies the type of record contained in 
    /// msDS-TrustForestTrustInfo
    /// </summary>
    public enum RecordType
    {
        /// <summary>
        /// ForestTrustTopLevelName
        /// </summary>
        ForestTrustTopLevelName = 0,

        /// <summary>
        /// ForestTrustTopLevelNameEx
        /// </summary>
        ForestTrustTopLevelNameEx = 1,

        /// <summary>
        /// ForestTrustDomainInfo
        /// </summary>
        ForestTrustDomainInfo = 2,

        /// <summary>
        /// an invalid record type
        /// </summary>
        Invalid
    }

    /// <summary>
    /// TD Section: 7.1.6.7.3: msDs-supportedEncryptionTypes
    /// Supported Encryption types
    /// These Encryption types are only supported on Windows 2008
    /// </summary>
    public enum SupportedEncryptionTypes
    {
        /// <summary>
        /// CRC (KERB_ENCTYPE_DES_CBC_CRC)
        /// </summary>
        KERB_ENCTYPE_DES_CBC_CRC,

        /// <summary>
        /// MD5 (KERB_ENCTYPE_DES_CBC_MD5)
        /// </summary>
        KERB_ENCTYPE_DES_CBC_MD5,
        
        /// <summary>
        /// RC4 (KERB_ENCTYPE_RC4_HMAC_MD5)
        /// </summary>
        KERB_ENCTYPE_RC4_HMAC_MD5,

        /// <summary>
        /// A128 (KERB_ENCTYPE_AES128_CTS_HMAC_SHA1_96)
        /// </summary>
        KERB_ENCTYPE_AES128_CTS_HMAC_SHA1_96,

        /// <summary>
        /// A256 (KERB_ENCTYPE_AES256_CTS_HMAC_SHA1_96)
        /// </summary>
        KERB_ENCTYPE_AES256_CTS_HMAC_SHA1_96,

        /// <summary>
        /// All encryption types bit set
        /// </summary>
        AllBitsSet,

        /// <summary>
        /// Encryption Bit not set
        /// </summary>
        NotSet
    }
    /// <summary>
    /// TD Section: 7.1.6.7.9 trustAttributes
    /// TrustAttributesEnum is an enumerator which signifies the
    /// possible values of trust relationship
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum TrustAttributesEnum
    {
        /// <summary>
        /// TRUST_ATTRIBUTE_NON_TRANSITIVE
        /// </summary>
        TRUST_ATTRIBUTE_NON_TRANSITIVE = 0x00000001,

        /// <summary>
        /// TRUST_ATTRIBUTE_UPLEVEL_ONLY
        /// </summary>
        TRUST_ATTRIBUTE_UPLEVEL_ONLY = 0x00000002,

        /// <summary>
        /// TRUST_ATTRIBUTE_QUARANTINED_DOMAIN
        /// </summary>
        TRUST_ATTRIBUTE_QUARANTINED_DOMAIN = 0x00000004,

        /// <summary>
        /// TRUST_ATTRIBUTE_FOREST_TRANSITIVE
        /// </summary>
        TRUST_ATTRIBUTE_FOREST_TRANSITIVE = 0x00000008,

        /// <summary>
        /// TRUST_ATTRIBUTE_CROSS_ORGANIZATION
        /// </summary>
        TRUST_ATTRIBUTE_CROSS_ORGANIZATION = 0x00000010,

        /// <summary>
        /// TRUST_ATTRIBUTE_WITHIN_FOREST
        /// </summary>
        TRUST_ATTRIBUTE_WITHIN_FOREST = 0x00000020,

        /// <summary>
        /// TRUST_ATTRIBUTE_TREAT_AS_EXTERNAL
        /// </summary>
        TRUST_ATTRIBUTE_TREAT_AS_EXTERNAL = 0x00000040,

        /// <summary>
        /// TRUST_ATTRIBUTE_USES_RC4_ENCRYPTION
        /// </summary>
        TRUST_ATTRIBUTE_USES_RC4_ENCRYPTION = 0x00000080,

        /// <summary>
        /// TRUST_ATTRIBUTE_FOREST_TRANSITIVE bits compatible with
        /// TRUST_ATTRIBUTE_WITHIN_FOREST
        /// </summary>
        TAFTAndTAWF,

        /// <summary>
        ///  TRUST_ATTRIBUTE_CROSS_ORGANIZATION bits compatible with
        ///  TRUST_ATTRIBUTE_WITHIN_FOREST
        /// </summary>
        TACOAndTAWF,

        /// <summary>
        /// R bit is set
        /// </summary>
        R,

        /// <summary>
        /// O bit is set
        /// </summary>
        O,

        /// <summary>
        /// not set
        /// </summary>
        NotSet
    }
    /// <summary>
    /// TD Section:7.1.6.7.15 trustType
    /// TrustTypeEnum is an enumerator which signifies the
    /// possible values of type of trust that can be established
    /// The trustType attribute is an integer value that dictates what type of 
    /// trust has been designated for the trusted domain
    /// </summary>
    public enum TrustTypeEnum
    {
        /// <summary>
        /// TRUST_TYPE_DOWNLEVEL
        /// </summary>
        TRUST_TYPE_DOWNLEVEL,

        /// <summary>
        /// TRUST_TYPE_UPLEVEL
        /// </summary>
        TRUST_TYPE_UPLEVEL,

        /// <summary>
        /// TRUST_TYPE_MIT
        /// </summary>
        TRUST_TYPE_MIT,

        /// <summary>
        /// TRUST_TYPE_DCE
        /// </summary>
        TRUST_TYPE_DCE,

        /// <summary>
        /// not set
        /// </summary>
        NotSet
    }

    /// <summary>
    /// TD Section: 7.1.6.7.12 trustDirection
    /// TrustDirectionEnum is an enumerator which signifies the
    /// possible values of type of direction of trust flows
    /// The trustDirection attribute dictates in which direction the trust flows
    /// </summary>
    public enum TrustDirectionEnum
    {
        /// <summary>
        /// TRUST_DIRECTION_DISABLED
        /// </summary>
        TRUST_DIRECTION_DISABLED = 0x00000000,

        /// <summary>
        /// TRUST_DIRECTION_INBOUND
        /// </summary>
        TRUST_DIRECTION_INBOUND = 0x00000001,

        /// <summary>
        /// TRUST_DIRECTION_OUTBOUND
        /// </summary>
        TRUST_DIRECTION_OUTBOUND = 0x00000002,

        /// <summary>
        /// TRUST_DIRECTION_BIDIRECTIONAL
        /// </summary>
        TRUST_DIRECTION_BIDIRECTIONAL = 0x00000003
    }
    
    /// <summary>
    /// This enumeration defines the TCP ports
    /// Used in Authentication mechanisms.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum Port
    {
        /// <summary>
        /// LDAP Port: 369
        /// </summary>
        LDAP_PORT = 389,
        
        /// <summary>
        /// LDAP Port for SSL authentication: 636
        /// </summary>
        LDAP_SSL_PORT = 636,
        
        /// <summary>
        /// LDAP GC Port: 3268
        /// </summary>
        LDAP_GC_PORT = 3268,
        
        /// <summary>
        /// LDAP SSL GC Port: 3269
        /// </summary>
        LDAP_SSL_GC_PORT = 3269
    };

    /// <summary>
    /// Active Directory Types. Related to different Test pass details
    /// </summary>
    public enum ADTypes
    {
        /// <summary>
        /// AD DS
        /// </summary>
        AD_DS,

        /// <summary>
        /// AD LDS
        /// </summary>
        AD_LDS
    }

    /// <summary>
    /// This enumeration defines the possible authentication mechanisms
    /// as stated in MS-ADTS.pdf
    /// </summary>
    public enum authenticationMech
    {
        /// <summary>
        /// Simple Bind
        /// </summary>
        simple,
        
        /// <summary>
        /// SASL Bind
        /// </summary>
        SASL,
        
        /// <summary>
        /// Mutual Authentication
        /// </summary>
        mutualauth,
        
        /// <summary>
        /// Sicily Bind
        /// </summary>
        sicily
    }

    /// <summary>
    /// define SASL choice
    /// </summary>
    public enum SASLChoice
    {
        /// <summary>
        /// SASL - GSSAPI Bind
        /// </summary>
        saslgssapi,
        
        /// <summary>
        /// SASL - SPNEGO Bind
        /// </summary>
        saslspnego,
        
        /// <summary>
        /// SASL - External Bind
        /// </summary>
        saslexternal,
        
        /// <summary>
        /// SASL - Digest MD5 Bind
        /// </summary>
        sasldigestMD5,
    }

    /// <summary>
    /// This enumeration defines the possible error status
    /// </summary>
    public enum errorstatus
    {
        /// <summary>
        /// success
        /// </summary>
        success = 0,

        /// <summary>
        /// protocolError
        /// </summary>
        protocolError = 2,

        /// <summary>
        /// adminLimitExceeded
        /// </summary>
        adminLimitExceeded = 11,

        /// <summary>
        /// inappropriateAuthentication
        /// </summary>
        inappropriateAuthentication = 48,

        /// <summary>
        /// invalidCredentials
        /// </summary>
        invalidCredentials = 49,

        /// <summary>
        /// busy
        /// </summary>
        busy = 51,

        /// <summary>
        /// unavailable
        /// </summary>
        unavailable = 52,

        /// <summary>
        /// unwillingToPerform
        /// </summary>
        unwillingToPerform = 53,

        /// <summary>
        /// other
        /// </summary>
        other = 80,

        /// <summary>
        /// failure
        /// </summary>
        failure = 81,

        /// <summary>
        /// unknown spn
        /// </summary>
        spnUnknown = 82,

        /// <summary>
        /// unknown OS
        /// </summary>
        unknownOS
    }

    /// <summary>
    /// This enumeration defines the possible values for user name
    /// </summary>
    public enum name
    {
        /// <summary>
        /// the user name is invalid
        /// </summary>
        nonexistUserName,

        /// <summary>
        /// the use name is valid
        /// </summary>
        validUserName,

        /// <summary>
        /// the user is anonymous
        /// </summary>
        anonymousUser,

        /// <summary>
        /// the user name maps to more than one object on the server
        /// </summary>
        nameMapsMoreThanOneObject
    }

    /// <summary>
    /// This enumeration defines the possible values for password
    /// </summary>
    public enum Password
    {
        /// <summary>
        /// the password is invalid
        /// </summary>
        invalidPassword,

        /// <summary>
        /// the password is valid
        /// </summary>
        validPassword
    }

    /// <summary>
    /// AccessRights is an enumerator which signifies the
    /// possible AccessRights that can be allotted to a user.
    /// User for validating Authorization feature.
    /// </summary>
    public enum AccessRights
    {
        /// <summary>
        /// none
        /// </summary>
        NotSet,
        /// <summary>
        /// RIGHT_DS_READ_PROPERTY on the Quotas container
        /// </summary>
        RIGHT_DS_READ_PROPERTY,
        /// <summary>
        /// RIGHT_DS_READ_PROPERTY on the user object
        /// </summary>
        RIGHT_DS_READ_PROPERTY_USEROBJ,
        /// <summary>
        /// RIGHT_DS_READ_PROPERTY on the replPropertyMetaData attribute
        /// </summary>
        DS_READ_METADATA,
        /// <summary>
        /// RIGHT_DS_READ_PROPERTY on repsfrom attribute
        /// </summary>
        DS_READ_REPSFORM,
        /// <summary>
        /// RIGHT_DS_READ_PROPERTY on repsto attribute 
        /// </summary>
        DS_READ_REPSTO,
        /// <summary>
        /// RIGHT_DS_READ_PROPERTY on replUpToDateVector 
        /// </summary>
        DS_READ_REPLVECTOR,
        /// <summary>
        /// (ACCESS_SYSTEM_SECURITY) and (READ_CONTROL)
        /// </summary>
        ACC_SYS_SEC_READ_CONTROL,
        /// <summary>
        /// RIGHT_DS_WRITE_PROPERTY 
        /// </summary>
        RIGHT_DS_WRITE_PROPERTY,
        /// <summary>
        /// RIGHT_DS_WRITE_PROPERTY_EXTENDED Granted
        /// </summary>
        RIGHT_DS_WRITE_EXTENDED,
        /// <summary>
        /// RIGHT_WRITE_DAC to do a write DACL Operation
        /// </summary>
        RIGHT_WRITE_DAC,
        /// <summary>
        /// RIGHT_DS_CREATE_CHILD property
        /// </summary>
        RIGHT_DS_CREATE_CHILD
    }
    /// <summary>
    /// ControlAccessRights is an enumerator which
    /// stores all the probable values of ControlAccessRights
    /// </summary>
    public enum ControlAccessRights
    {
        /// <summary>
        /// none
        /// </summary>
        NotSet,

        /// <summary>
        /// User_Change_Password
        /// </summary>
        User_Change_Password,

        /// <summary>
        /// User_Force_Change_Password
        /// </summary>
        User_Force_Change_Password,

        /// <summary>
        /// DS_Query_Self_Quota
        /// </summary>
        DS_Query_Self_Quota,

        /// <summary>
        /// RIGHT_DS_REPL_MANAGE_TOPOLOGY on the 
        /// replPropertyMetaData attribute
        /// </summary>
        DS_Replication_Manage_Topology,

        /// <summary>
        /// DS_Replication_Secrets_Synchronize
        /// </summary>
        DS_Replication_Secrets_Synchronize,

        /// <summary>
        /// DS_Replication_Monitor_Topology
        /// </summary>
        DS_Replication_Monitor_Topology,

        /// <summary>
        /// RIGHT_DS_REPL_MANAGE_TOPOLOGY, RIGHT_DS_REPL_MONITOR_TOPOLOGY 
        /// on repsfrom attribute
        /// </summary>
        Repl_Man_Topo_REPSFROM,

        /// <summary>
        /// Repl_Mon_Topo_REPSFROM
        /// </summary>
        Repl_Mon_Topo_REPSFROM,

        /// <summary>
        /// RIGHT_DS_REPL_MANAGE_TOPOLOGY, RIGHT_DS_REPL_MONITOR_TOPOLOGY 
        /// on repsto attribute
        /// </summary>
        Repl_Man_Topo_REPSTO,

        /// <summary>
        /// Repl_Mon_Topo_REPSTO
        /// </summary>
        Repl_Mon_Topo_REPSTO,

        /// <summary>
        /// RIGHT_DS_REPL_MANAGE_TOPOLOGY , RIGHT_DS_REPL_MONITOR_TOPOLOGY 
        /// on replUpToDateVector
        /// </summary>
        Repl_Man_Topo_REPLVECTOR,

        /// <summary>
        /// Repl_Mon_Topo_REPLVECTOR
        /// </summary>
        Repl_Mon_Topo_REPLVECTOR

    }

    /// <summary>
    /// Enumerates all the attributes of an object which needs to be checked depending
    /// their access levels (i.e AccessRights) as part of Authorization
    /// </summary>
    public enum AttribsToCheck
    {
        /// <summary>
        /// nTSecurityDescriptor
        /// </summary>
        nTSecurityDescriptor,

        /// <summary>
        /// msDS_QuotaEffective
        /// </summary>
        msDS_QuotaEffective,

        /// <summary>
        /// msDS_QuotaUsed
        /// </summary>
        msDS_QuotaUsed,

        /// <summary>
        /// msDS_ReplAttributeMetaData
        /// </summary>
        msDS_ReplAttributeMetaData,

        /// <summary>
        /// msDS_ReplValueMetaData
        /// </summary>
        msDS_ReplValueMetaData,

        /// <summary>
        /// msDS_NCReplInboundNeighbors
        /// </summary>
        msDS_NCReplInboundNeighbors,

        /// <summary>
        /// msDS_NCReplOutboundNeighbors
        /// </summary>
        msDS_NCReplOutboundNeighbors,

        /// <summary>
        /// msDS_NCReplCursors
        /// </summary>
        msDS_NCReplCursors,

        /// <summary>
        /// msDS_IsUserCachableAtRodc
        /// </summary>
        msDS_IsUserCachableAtRodc,

        /// <summary>
        /// userPassword
        /// </summary>
        userPassword,

        /// <summary>
        /// dnsHostName
        /// </summary>
        dnsHostName,

        /// <summary>
        /// servicePrincipleName
        /// </summary>
        servicePrincipleName,

        /// <summary>
        /// writeDACLOperation
        /// </summary>
        writeDACLOperation,

        /// <summary>
        /// moveOperation
        /// </summary>
        moveOperation
    }
    /// <summary>
    /// Forest Functional Level
    /// </summary>
    public enum ForestFunctionalLevel
    {
        /// <summary>
        /// Forest Functional Level lesser than DS_BEHAVIOR_WIN2003
        /// </summary>
        LESS_DS_BEHAVIOR_WIN2003,

        /// <summary>
        /// Forest Functional Level DS_BEHAVIOR_WIN2003
        /// </summary>
        DS_BEHAVIOR_WIN2003,

        /// <summary>
        /// Forest Functional Level higher than DS_BEHAVIOR_WIN2003
        /// </summary>
        HIGHER_DS_BEHAVIOR_WIN2003,
        /// <summary>
        /// Used for setting the default value in Adapter
        /// </summary>
        NOT_SET

    }

    /// <summary>
    /// Trusted_Forest_Information stores the Trusted Forest Record Information
    /// for TDOs whose Trust Attribute is TRUST_ATTRIBUTE_FOREST_TRANSITIVE
    /// </summary>
    public class Trusted_Forest_Information : CompoundValue
    {

        /// <summary>
        /// Net-Bios Name
        /// </summary>
        public string NetBiosName;
        /// <summary>
        /// SID Value
        /// </summary>
        public string ValueSID;
        /// <summary>
        /// SDC Flag status
        /// </summary>
        public SDCFlag sdcBit;
        /// <summary>
        /// NDC Flag Status
        /// </summary>
        public NDCFlag ndcBit;
        /// <summary>
        /// recType is of type RecordType. Section 7.1.6.9.3.1 specifies the possible entries
        /// for record type field of Records
        /// </summary>
        public RecordType recType;
        /// <summary>
        /// Parameterized Constructor for Trusted_Forest_Information
        /// </summary>
        /// <param name="netBiosName">Net-Bios Name</param>
        /// <param name="sidValue">SID Value</param>
        /// <param name="sdc">SDC Flag status</param>
        /// <param name="ndc">NDC Flag Status</param>
        /// <param name="typeRec">Record Type</param>
        public Trusted_Forest_Information(string netBiosName,
                                          string sidValue,
                                          SDCFlag sdc,
                                          NDCFlag ndc,
                                          RecordType typeRec)
        {
            this.NetBiosName = netBiosName;
            this.ValueSID = sidValue;
            this.sdcBit = sdc;
            this.ndcBit = ndc;
            this.recType = typeRec;
        }

        /// <summary>
        /// initialize an instance
        /// </summary>
        public Trusted_Forest_Information() { }
    }
}
