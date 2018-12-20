// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Common
{
    #region Common Enums

    /// <summary>
    /// Enum specifies the optional feature flags
    /// </summary>
    [Flags]
    public enum OptionalFeature : uint
    {
        FOREST_OPTIONAL_FEATURE = 0x00000001,
        DOMAIN_OPTIONAL_FEATURE = 0x00000002,
        DISABLABLE_OPTIONAL_FEATURE = 0x00000004,
        SERVER_OPTIONAL_FEATURE = 0x00000008,
    }

    /// <summary>
    /// Enumeration specifies the domain function level
    /// </summary>
    public enum DomainFunctionLevel : uint
    {
        /// <summary>
        /// Windows server 2000
        /// </summary>
        DS_BEHAVIOR_WIN2000 = 0,

        /// <summary>
        /// Windows server 2000 with mixed domains
        /// </summary>
        DS_BEHAVIOR_WIN2000_WITH_MIXED_DOMAINS = 1,

        ///<summary>
        /// Windows server 2003
        /// </summary>
        DS_BEHAVIOR_WIN2003 = 2,

        ///<summary>
        /// Windows server 2008
        /// </summary>
        DS_BEHAVIOR_WIN2008 = 3,

        ///<summary>
        /// Windows server 2008 R2
        /// </summary>
        DS_BEHAVIOR_WIN2008R2 = 4,

        ///<summary>
        /// Windows Server 2012
        /// </summary>
        DS_BEHAVIOR_WIN2012 = 5,

        /// <summary>
        /// Windows Server 2012 R2
        /// </summary>
        DS_BEHAVIOR_WIN2012R2 = 6,

        /// <summary>
        /// Windows Threshold
        /// </summary>
        DS_BEHAVIOR_WINTHRESHOLD = 7,
    }

    /// <summary>
    /// Defines the server version
    /// </summary>
    public enum ServerVersion
    {
        Invalid = 0,
        Win2003 = 5,
        Win2008 = 6,
        Win2008R2 = 7,
        Win2012 = 8,
        Win2012R2 = 9,
        Win2016 = 10,
        Winv1803 = 11,
        NonWin = 100, // suppose non-windows support all windows features
    }

    /// <summary>
    /// Defines the windows error code that are used in [MS-ADTS]
    /// </summary>
    public enum WindowsErrorCode
    {
        STATUS_QUOTA_EXCEEDED = 0x0044,
        ERROR_INVALID_PARAMETER = 0x0057,
        ERROR_NOT_FOUND = 0x0490,
        ERROR_DS_OBJ_CLASS_VIOLATION = 0x2014,
        ERROR_DS_REFERRAL = 0x202B,
        ERROR_DS_CONSTRAINT_VIOLATION = 0x202F,
        ERROR_DS_UNWILLING_TO_PERFORM = 0x2035,
        ERROR_DS_NOT_SUPPORTED = 0x2040,
        ERROR_DS_ADD_REPLICA_INHIBITED = 0x206E,
        ERROR_DS_ATT_NOT_DEF_IN_SCHEMA = 0x206F,
        ERROR_DS_RDN_DOESNT_MATCH_SCHEMA = 0x2073,
        ERROR_DS_ATT_IS_NOT_ON_OBJ = 0x2076,
        ERROR_DS_ILLEGAL_MOD_OPERATION = 0x2077,
        ERROR_DS_BAD_INSTANCE_TYPE = 0x2079,
        ERROR_DS_MASTERDSA_REQUIRED = 0x207A,
        ERROR_DS_OBJECT_CLASS_REQUIRED = 0x207B,
        ERROR_DS_ATT_VAL_ALREADY_EXISTS = 0x2083,
        ERROR_DS_CANT_REM_MISSING_ATT_VAL = 0x2085,
        ERROR_DS_NO_PARENT_OBJECT = 0x2089,
        ERROR_DS_CHILDREN_EXIST = 0x208C,
        ERROR_DS_OBJ_NOT_FOUND = 0x208D,
        ERROR_DS_BAD_NAME_SYNTAX = 0x208F,
        ERROR_DS_ILLEGAL_SUPERIOR = 0x2099,
        ERROR_DS_GENERIC_ERROR = 0x2095,
        ERROR_DS_ATTRIBUTE_OWNED_BY_SAM = 0x209A,
        ERROR_DS_NAME_UNPARSEABLE = 0x209E,
        ERROR_DS_CANT_ADD_SYSTEM_ONLY = 0x20A6,
        ERROR_DS_CLASS_MUST_BE_CONCRETE = 0x20A7,
        ERROR_DS_INVALID_ROLE_OWNER = 0x20AE,
        ERROR_DS_CANT_MOD_SYSTEM_ONLY = 0x20B1,
        ERROR_DS_OBJ_CLASS_NOT_SUBCLASS = 0x20B4,
        ERROR_DS_RANGE_CONSTRAINT = 0x20BF,
        ERROR_DS_CANT_DELETE = 0x20CE,
        ERROR_DS_CANT_FIND_EXPECTED_NC = 0x20E4,
        ERROR_DS_SECURITY_ILLEGAL_MODIFY = 0x20E7,
        ERROR_DS_CONSTRUCTED_ATT_MOD = 0x211B,
        ERROR_DS_SRC_AND_DST_NC_IDENTICAL = 0x2125,
        ERROR_DS_CANT_MOVE_DELETED_OBJECT = 0x2129,
        ERROR_DS_ILLEGAL_XDOM_MOVE_OPERATION = 0x212C,
        ERROR_DS_CANT_WITH_ACCT_GROUP_MEMBERSHPS = 0x212D,
        ERROR_DS_CANT_MOVE_ACCOUNT_GROUP = 0x2132,
        ERROR_DS_CANT_MOVE_RESOURCE_GROUP = 0x2133,
        ERROR_DS_ILLEGAL_BASE_SCHEMA_MOD = 0x213B,
        ERROR_DS_LOW_DSA_VERSION = 0x2178,
        ERROR_DS_NAME_NOT_UNIQUE = 0x217B,
        ERROR_DS_MODIFYDN_DISALLOWED_BY_FLAG = 0x2185,
        ERROR_DS_CANT_MOVE_APP_QUERY_GROUP = 0x21A1,
        ERROR_DS_WKO_CONTAINER_CANNOT_BE_SPECIAL = 0x21A3,
        ERROR_DS_DISALLOWED_IN_SYSTEM_CONTAINER = 0x21A7,
        ERROR_DS_HIGH_DSA_VERSION = 0x21C2,
        ERROR_DS_SPN_VALUE_NOT_UNIQUE_IN_FOREST = 0x21C7,
        ERROR_DS_UPN_VALUE_NOT_UNIQUE_IN_FOREST = 0x21C8,
    }

    /// <summary>
    /// Enum contains all the ldap policies supported
    /// </summary>
    public enum LdapPolicy
    {
        MaxActiveQueries,
        InitRecvTimeout,
        MaxConnections,
        MaxConnIdleTime,
        MaxDatagramRecv,
        MaxNotificationPerConn,
        MaxPoolThreads,
        MaxReceiveBuffer,
        MaxPageSize,
        MaxQueryDuration,
        MaxResultSetSize,
        MaxTempTableSize,
        MaxValRange,
        MaxResultSetsPerConn,
        MinResultSets,
        MaxBatchReturnMessages,
    }

    /// <summary>
    /// Enum contains all the the Active Directory services provided
    /// </summary>
    public enum ADImplementations
    {
        /// <summary>
        /// Represent AD/DS 
        /// </summary>
        AD_DS,

        /// <summary>
        /// Represents AD/LDS
        /// </summary>
        AD_LDS,
    }

    /// <summary>
    /// Defines all the server states
    /// </summary>
    public enum State
    {
        /// <summary>
        /// Represents an uninitialized state of AD
        /// </summary>
        UnInitialized,

        /// <summary>
        /// Initial state
        /// </summary>
        Init,

        /// <summary>
        /// state reached after the search request is sent
        /// </summary>
        RequestSent,

        /// <summary>
        /// State reached if the search operation is successful
        /// </summary>
        RetrievalSuccessful,

        /// <summary>
        /// State reached if the search operation is unsuccessful
        /// </summary>
        RetrievalUnSuccessful,

        /// <summary>
        /// State reached if the add operation is successful
        /// </summary>
        ADDOperationSuccessful,

        /// <summary>
        /// State reached if the add operation is unsuccessful
        /// </summary>
        ADDOperationUnSuccessful,

        /// <summary>
        /// State reached if the modify operation is successful
        /// </summary>
        ModifyOperationSuccessful,

        /// <summary>
        /// State reached if the modify operation is unsuccessful
        /// </summary>
        ModifyOperationUnSuccessful,

        /// <summary>
        /// State reached if the delete operation is successful
        /// </summary>
        DeleteOperationSuccessful,

        /// <summary>
        /// State reached if the delete operation is unsuccessful
        /// </summary>
        DeleteOperationUnSuccessful,

        /// <summary>
        /// State reached if Tree delete operation is successful
        /// </summary>
        TreeDeleteOperationSuccessful,

        /// <summary>
        /// State reached if Tree delete operation is unsuccessful
        /// </summary>
        TreeDeleteOperationUnSuccessful,

        /// <summary>
        /// Represents a successful ModifyDN operation 
        /// </summary>
        ModifyDNSuccessful,

        /// <summary>
        /// Represents an unSuccessful ModifyDN operation 
        /// </summary>
        ModifyDNUnSuccessful,

        /// <summary>
        /// Represents a successful extended operation
        /// </summary>
        ExtendedOperationSuccessful,

        /// <summary>
        /// Represents an unsuccessful extended operation
        /// </summary>
        ExtendedoperationUnSuccessful
    }

    /// <summary>
    /// Defines the Access rights on container objects.
    /// </summary>
    public enum ContainerAccessRights
    {
        /// <summary>
        /// Right that is needed for the visibility check of the object
        /// </summary>
        RIGHT_DS_LIST_CONTENTS,

        /// <summary>
        /// Invalid right which is not required.
        /// </summary>
        INVALID_RIGHT
    }

    /// <summary>
    /// Enum that specifies the Access Rights on parent objects
    /// </summary>
    public enum RightsOnParentObjects
    {
        /// <summary>
        /// Access right on the parent object for the ObjectClass of the object being added along with SE_ENABLE_DELEGATION_PRIVILEGE
        /// </summary>
        RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,

        /// <summary>
        /// Access right on the parent object for the ObjectClass of the object being added without SE_ENABLE_DELEGATION_PRIVILEGE
        /// </summary>
        RIGHT_DS_CREATE_CHILDwithOutSE_ENABLE_DELEGATION_PRIVILEGE,

        /// <summary>
        /// Right that is needed for the visibility check of the object
        /// </summary>
        RIGHT_DS_LIST_CONTENTS,

        /// <summary>
        /// Right that is needed for the visibility check of the object if isfDoListObject flag of dsHeuristics is set.
        /// </summary>
        RIGHT_DS_LIST_OBJECT,

        /// <summary>
        /// Right that must be present on the parent of the deleting object.
        /// </summary>
        RIGHT_DS_DELETE_CHILD,

        /// <summary>
        /// Access right on the parent object for the ObjectClass of the object being added
        /// </summary>
        RIGHT_DS_CREATE_CHILD,

        /// <summary>
        /// CREATE_CHILD
        /// </summary>
        CREATE_CHILD,

        /// <summary>
        /// Invalid right which is not required.
        /// </summary>
        INVALID_RIGHT
    }

    /// <summary>
    /// Enum that specifies the access rights on Objects
    /// </summary>
    public enum RightsOnObjects
    {
        /// <summary>
        /// Right that must be present in order to delete the object and its children.
        /// </summary>
        RIGHT_DS_DELETE_TREE,

        /// <summary>
        /// RIGHT_DELETE
        /// </summary>
        RIGHT_DELETE,

        /// <summary>
        /// Right that must be present in order to move or rename the object.RIGHT_DS_WRITE_PROPERTY on security context 
        /// And RIGHT_DELETE on requester
        /// </summary>
        RIGHT_DS_WRITE_PROPERTYWithRIGHT_DELETE,

        /// <summary>
        /// RIGHT_DS_WRITE_PROPERTY present on Security context and RIGHT_DELETE not present for the requester.
        /// </summary>
        RIGHT_DS_WRITE_PROPERTYWithOutRIGHT_DELETE,

        /// <summary>
        /// Invalid right which is not required.
        /// </summary>
        INVALID_RIGHT
    }

    #endregion

    #region Common Classes

    /// <summary>
    /// Class contains all the TD defined extended controls OID
    /// </summary>
    public class ExtendedControl
    {
        public const string OffsetRangeError = null;
        public const string LDAP_SERVER_GET_STATS_OIDWithSO_EXTENDED_FMT = "1.2.840.113556.1.4.970";
        public const string NotSpecifiedControl = null;

        /// <summary>
        /// LDAP_PAGED_RESULT_OID_STRING
        /// </summary>
        public const string LDAP_PAGED_RESULT_OID_STRING = "1.2.840.113556.1.4.319";

        /// <summary>
        /// LDAP_SERVER_CROSSDOM_MOVE_TARGET_OID: Control to move the object across the domains.
        /// </summary>
        public const string LDAP_SERVER_CROSSDOM_MOVE_TARGET_OID = "1.2.840.113556.1.4.521";

        /// <summary>
        /// LDAP_SERVER_DIRSYNC_OID
        /// </summary>
        public const string LDAP_SERVER_DIRSYNC_OID = "1.2.840.113556.1.4.841";

        /// <summary>
        /// LDAP_SERVER_DOMAIN_SCOPE_OID
        /// </summary>
        public const string LDAP_SERVER_DOMAIN_SCOPE_OID = "1.2.840.113556.1.4.1339";

        /// <summary>
        /// LDAP_SERVER_EXTENDED_DN_OID
        /// </summary>
        public const string LDAP_SERVER_EXTENDED_DN_OID = "1.2.840.113556.1.4.529";

        /// <summary>
        /// LDAP_SERVER_GET_STATS_OID
        /// </summary>
        public const string LDAP_SERVER_GET_STATS_OID = "1.2.840.113556.1.4.970";

        /// <summary>
        /// LDAP_SERVER_LAZY_COMMIT_OID
        /// </summary>
        public const string LDAP_SERVER_LAZY_COMMIT_OID = "1.2.840.113556.1.4.619";

        /// <summary>
        /// LDAP_SERVER_PERMISSIVE_MODIFY_OID
        /// </summary>
        public const string LDAP_SERVER_PERMISSIVE_MODIFY_OID = "1.2.840.113556.1.4.1413";

        /// <summary>
        /// LDAP_SERVER_NOTIFICATION_OID
        /// </summary>
        public const string LDAP_SERVER_NOTIFICATION_OID = "1.2.840.113556.1.4.528";

        /// <summary>
        /// LDAP_SERVER_RESP_SORT_OID
        /// </summary>
        public const string LDAP_SERVER_RESP_SORT_OID = "1.2.840.113556.1.4.474";

        /// <summary>
        /// LDAP_SERVER_SD_FLAGS_OID
        /// </summary>
        public const string LDAP_SERVER_SD_FLAGS_OID = "1.2.840.113556.1.4.801";

        /// <summary>
        /// LDAP_SERVER_SEARCH_OPTIONS_OID
        /// </summary>
        public const string LDAP_SERVER_SEARCH_OPTIONS_OID = "1.2.840.113556.1.4.1340";

        /// <summary>
        /// LDAP_SERVER_SORT_OID
        /// </summary>
        public const string LDAP_SERVER_SORT_OID = "1.2.840.113556.1.4.473";

        /// <summary>
        /// LDAP_SERVER_SHOW_DELETED_OID
        /// </summary>
        public const string LDAP_SERVER_SHOW_DELETED_OID = "1.2.840.113556.1.4.417";

        /// <summary>
        /// LDAP_SERVER_TREE_DELETE_OID
        /// </summary>
        public const string LDAP_SERVER_TREE_DELETE_OID = "1.2.840.113556.1.4.805";

        /// <summary>
        /// LDAP_SERVER_VERIFY_NAME_OID
        /// </summary>
        public const string LDAP_SERVER_VERIFY_NAME_OID = "1.2.840.113556.1.4.1338";

        /// <summary>
        /// LDAP_CONTROL_VLVREQUEST
        /// </summary>
        public const string LDAP_CONTROL_VLVREQUEST = "2.16.840.1.113730.3.4.9";

        /// <summary>
        /// LDAP_CONTROL_VLVRESPONSE
        /// </summary>
        public const string LDAP_CONTROL_VLVRESPONSE = "2.16.840.1.113730.3.4.10";

        /// <summary>
        /// LDAP_SERVER_ASQ_OID
        /// </summary>
        public const string LDAP_SERVER_ASQ_OID = "1.2.840.113556.1.4.1504";

        /// <summary>
        /// LDAP_SERVER_QUOTA_CONTROL_OID
        /// </summary>
        public const string LDAP_SERVER_QUOTA_CONTROL_OID = "1.2.840.113556.1.4.1852";

        /// <summary>
        /// LDAP_SERVER_RANGE_OPTION_OID
        /// </summary>
        public const string LDAP_SERVER_RANGE_OPTION_OID = "1.2.840.113556.1.4.802";

        /// <summary>
        /// LDAP_SERVER_SHUTDOWN_NOTIFY_OID
        /// </summary>
        public const string LDAP_SERVER_SHUTDOWN_NOTIFY_OID = "1.2.840.113556.1.4.1907";

        /// <summary>
        /// LDAP_SERVER_FORCE_UPDATE_OID 
        /// </summary>
        public const string LDAP_SERVER_FORCE_UPDATE_OID = "1.2.840.113556.1.4.1974";

        /// <summary>
        /// LDAP_SERVER_RANGE_RETRIEVAL_NOERR_OID
        /// </summary>
        public const string LDAP_SERVER_RANGE_RETRIEVAL_NOERR_OID = "1.2.840.113556.1.4.1948";

        /// <summary>
        /// LDAP_SERVER_RODC_DCPROMO_OID
        /// </summary>
        public const string LDAP_SERVER_RODC_DCPROMO_OID = "1.2.840.113556.1.4.1341";

        /// <summary>
        /// LDAP_SERVER_DN_INPUT_OID
        /// </summary>
        public const string LDAP_SERVER_DN_INPUT_OID = "1.2.840.113556.1.4.2026";

        /// <summary>
        /// LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID: Specify that link attributes that refer to deleted-objects are visible to the search operation
        /// </summary>
        public const string LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID = "1.2.840.113556.1.4.2065";

        /// <summary>
        /// LDAP_SERVER_SHOW_RECYCLED_OID
        /// </summary>
        public const string LDAP_SERVER_SHOW_RECYCLED_OID = "1.2.840.113556.1.4.2064";

        /// <summary>
        /// LDAP_SERVER_POLICY_HINTS_DEPRECATED_OID
        /// </summary>
        public const string LDAP_SERVER_POLICY_HINTS_DEPRECATED_OID = "1.2.840.113556.1.4.2066";

        /// <summary>
        /// The LDAP_SERVER_DIRSYNC_EX_OID: used with an LDAP search operation in exactly the same way as the LDAP_SERVER_DIRSYNC_OID control.
        /// </summary>
        public const string LDAP_SERVER_DIRSYNC_EX_OID = "1.2.840.113556.1.4.2090";

        /// <summary>
        /// The LDAP_SERVER_UPDATE_STATS_OID: can be used with any LDAP operation. When sending this control to the DC, the controlValue field of the Control structure is omitted.
        /// When the server receives a request with the LDAP_SERVER_UPDATE_STATS_OID control attached to it, the server includes a response control in the response that contains statistics.
        /// </summary>
        public const string LDAP_SERVER_UPDATE_STATS_OID = "1.2.840.113556.1.4.2205";

        /// <summary>
        /// The LDAP_SERVER_TREE_DELETE_EX_OID: used with an LDAP delete operation to cause the server to recursively delete the entire subtree of objects located underneath the object specified in the delete operation.
        /// </summary>
        public const string LDAP_SERVER_TREE_DELETE_EX_OID = "1.2.840.113556.1.4.2204";

        /// <summary>
        /// The LDAP_SERVER_SEARCH_HINTS_OID control is used with an LDAP search operation. This control supplies hints to the search operation on how to satisfy the search.
        /// </summary>
        public const string LDAP_SERVER_SEARCH_HINTS_OID = "1.2.840.113556.1.4.2206";

        /// <summary>
        /// The LDAP_SERVER_EXPECTED_ENTRY_COUNT_OID control is used with an LDAP search operation to potentially modify the return code of the operation.
        /// </summary>
        public const string LDAP_SERVER_EXPECTED_ENTRY_COUNT_OID = "1.2.840.113556.1.4.2211";

        /// <summary>
        /// LDAP_SERVER_POLICY_HINTS_OID
        /// </summary>
        public const string LDAP_SERVER_POLICY_HINTS_OID = "1.2.840.113556.1.4.2239";

        /// <summary>
        /// The LDAP_SERVER_SET_OWNER_OID control is used with an LDAP add operation to specify the owner of the object to be created.
        /// </summary>
        public const string LDAP_SERVER_SET_OWNER_OID = "1.2.840.113556.1.4.2255";

        /// <summary>
        /// LDAP_SERVER_BYPASS_QUOTA_OID
        /// </summary>
        public const string LDAP_SERVER_BYPASS_QUOTA_OID = "1.2.840.113556.1.4.2256";
    }

    /// <summary>
    /// Class contains all the matching rules supported by active directory
    /// </summary>
    public class MatchRule
    {
        /// <summary>
        /// LDAP_MATCHING_RULE_BIT_AND
        /// </summary>
        public const string LDAP_MATCHING_RULE_BIT_AND = "1.2.840.113556.1.4.803";

        /// <summary>
        /// LDAP_MATCHING_RULE_BIT_OR
        /// </summary>
        public const string LDAP_MATCHING_RULE_BIT_OR = "1.2.840.113556.1.4.804";

        /// <summary>
        /// LDAP_MATCHING_RULE_TRANSITIVE_EVAL
        /// </summary>
        public const string LDAP_MATCHING_RULE_TRANSITIVE_EVAL = "1.2.840.113556.1.4.1941";

        /// <summary>
        /// LDAP_MATCHING_RULE_DN_WITH_DATA
        /// </summary>
        public const string LDAP_MATCHING_RULE_DN_WITH_DATA = "1.2.840.113556.1.4.2253";
    }

    /// <summary>
    /// Class contains all the extended operations
    /// </summary>
    public class ExtendedOperation
    {
        /// <summary>
        /// LDAP_SERVER_FAST_BIND_OID, Extended operation exposed as OID 1.2.840.113556.1.4.1781
        /// </summary>
        public const string LDAP_SERVER_FAST_BIND_OID = "1.2.840.113556.1.4.1781";

        /// <summary>
        /// LDAP_SERVER_START_TLS_OID, Extended operation exposed as OID 1.3.6.1.4.1.1466.20037
        /// </summary>
        public const string LDAP_SERVER_START_TLS_OID = "1.3.6.1.4.1.1466.20037";

        /// <summary>
        /// LDAP_TTL_REFRESH_OID, Extended operation exposed as OID 1.3.6.1.4.1.1466.101.119.1
        /// </summary>
        public const string LDAP_TTL_REFRESH_OID = "1.3.6.1.4.1.1466.101.119.1";

        /// <summary>
        /// LDAP_SERVER_WHO_AM_I_OID, Extended operation exposed as OID 1.3.6.1.4.1.4203.1.11.3
        /// </summary>
        public const string LDAP_SERVER_WHO_AM_I_OID = "1.3.6.1.4.1.4203.1.11.3";

        /// <summary>
        /// LDAP_SERVER_BATCH_REQUEST_OID, Extended operation exposed as OID 1.2.840.113556.1.4.2212
        /// </summary>
        public const string LDAP_SERVER_BATCH_REQUEST_OID = "1.2.840.113556.1.4.2212";
    }

    /// <summary>
    /// Class contains all the RootDSE Attributes
    /// </summary>
    public class RootDSEAttribute
    {
        public const string configurationNamingContext = "configurationNamingContext";
        public const string currentTime = "currentTime";
        public const string defaultNamingContext = "defaultNamingContext";
        public const string dNSHostName = "dNSHostName";
        public const string dsSchemaAttrCount = "dsSchemaAttrCount";
        public const string dsSchemaClassCount = "dsSchemaClassCount";
        public const string dsSchemaPrefixCount = "dsSchemaPrefixCount";
        public const string dsServiceName = "dsServiceName";
        public const string highestCommittedUSN = "highestCommittedUSN";
        public const string isGlobalCatalogReady = "isGlobalCatalogReady";
        public const string isSynchronized = "isSynchronized";
        public const string ldapServiceName = "ldapServiceName";
        public const string namingContexts = "namingContexts";
        public const string netlogon = "netlogon";
        public const string pendingPropagations = "pendingPropagations";
        public const string rootDomainNamingContext = "rootDomainNamingContext";
        public const string schemaNamingContext = "schemaNamingContext";
        public const string serverName = "serverName";
        public const string subschemaSubentry = "subschemaSubentry";
        public const string supportedCapabilities = "supportedCapabilities";
        public const string supportedControl = "supportedControl";
        public const string supportedLDAPPolicies = "supportedLDAPPolicies";
        public const string supportedLDAPVersion = "supportedLDAPVersion";
        public const string supportedSASLMechanisms = "supportedSASLMechanisms";
        public const string domainControllerFunctionality = "domainControllerFunctionality";
        public const string domainFunctionality = "domainFunctionality";
        public const string forestFunctionality = "forestFunctionality";
        public const string msDS_ReplAllInboundNeighbors = "msDS-ReplAllInboundNeighbors";
        public const string msDS_ReplAllOutboundNeighbors = "msDS-ReplAllOutboundNeighbors";
        public const string msDS_ReplConnectionFailures = "msDS-ReplConnectionFailures";
        public const string msDS_ReplLinkFailures = "msDS-ReplLinkFailures";
        public const string msDS_ReplPendingOps = "msDS-ReplPendingOps";
        public const string msDS_ReplQueueStatistics = "msDS-ReplQueueStatistics";
        public const string msDS_TopQuotaUsage = "msDS-TopQuotaUsage";
        public const string supportedConfigurableSettings = "supportedConfigurableSettings";
        public const string supportedExtension = "supportedExtension";
        public const string validFSMOs = "validFSMOs";
        public const string dsaVersionString = "dsaVersionString";
        public const string msDS_PortLDAP = "msDS-PortLDAP";
        public const string msDS_PortSSL = "msDS-PortSSL";
        public const string msDS_PrincipalName = "msDS-PrincipalName";
        public const string serviceAccountInfo = "serviceAccountInfo";
        public const string spnRegistrationResult = "spnRegistrationResult";
        public const string tokenGroups = "tokenGroups";
        public const string usnAtRifm = "usnAtRifm";
    }

    /// <summary>
    /// Class contains all the Control Access Rights
    /// </summary>
    public class ControlAccessRight 
    {
        public static Guid Abandon_Replication = new Guid("{ee914b82-0a98-11d1-adbb-00c04fd8d5cd}");
        public static Guid Add_GUID = new Guid("{440820ad-65b4-11d1-a3da-0000f875ae0d}");
        public static Guid Allocate_Rids = new Guid("{1abd7cf8-0a99-11d1-adbb-00c04fd8d5cd}");
        public static Guid Allowed_To_Authenticate = new Guid("{68b1d179-0d15-4d4f-ab71-46152e79a7bc}"); 
        public static Guid Apply_Group_Policy = new Guid("{edacfd8f-ffb3-11d1-b41d-00a0c968f939}"); 
        public static Guid Certificate_Enrollment = new Guid("{0e10c968-78fb-11d2-90d4-00c04f79dc55}"); 
        public static Guid Certificate_AutoEnrollment = new Guid("{a05b8cc2-17bc-4802-a710-e7c15ab866a2}"); 
        public static Guid Change_Domain_Master = new Guid("{014bf69c-7b3b-11d1-85f6-08002be74fab}"); 
        public static Guid Change_Infrastructure_Master = new Guid("{cc17b1fb-33d9-11d2-97d4-00c04fd8d5cd}"); 
        public static Guid Change_PDC = new Guid("{bae50096-4752-11d1-9052-00c04fc2d4cf}"); 
        public static Guid Change_Rid_Master = new Guid("{d58d5f36-0a98-11d1-adbb-00c04fd8d5cd}"); 
        public static Guid Change_Schema_Master = new Guid("{e12b56b6-0a95-11d1-adbb-00c04fd8d5cd}"); 
        public static Guid Create_Inbound_Forest_Trust = new Guid("{e2a36dc9-ae17-47c3-b58b-be34c55ba633}"); 
        public static Guid Do_Garbage_Collection = new Guid("{fec364e0-0a98-11d1-adbb-00c04fd8d5cd}"); 
        public static Guid Domain_Administer_Server = new Guid("{ab721a52-1e2f-11d0-9819-00aa0040529b}"); 
        public static Guid DS_Check_Stale_Phantoms = new Guid("{69ae6200-7f46-11d2-b9ad-00c04f79f805}"); 
        public static Guid DS_Execute_Intentions_Script = new Guid("{2f16c4a5-b98e-432c-952a-cb388ba33f2e}"); 
        public static Guid DS_Install_Replica = new Guid("{9923a32a-3607-11d2-b9be-0000f87a36b2}"); 
        public static Guid DS_Query_Self_Quota = new Guid("{4ecc03fe-ffc0-4947-b630-eb672a8a9dbc}"); 
        public static Guid DS_Replication_Get_Changes = new Guid("{1131f6aa-9c07-11d1-f79f-00c04fc2dcd2}"); 
        public static Guid DS_Replication_Get_Changes_All = new Guid("{1131f6ad-9c07-11d1-f79f-00c04fc2dcd2}"); 
        public static Guid DS_Replication_Get_Changes_In_Filtered_Set = new Guid("{89e95b76-444d-4c62-991a-0facbeda640c}"); 
        public static Guid DS_Replication_Manage_Topology = new Guid("{1131f6ac-9c07-11d1-f79f-00c04fc2dcd2}"); 
        public static Guid DS_Replication_Monitor_Topology = new Guid("{f98340fb-7c5b-4cdb-a00b-2ebdfa115a96}"); 
        public static Guid DS_Replication_Synchronize = new Guid("{1131f6ab-9c07-11d1-f79f-00c04fc2dcd2}"); 
        public static Guid Enable_Per_User_Reversibly_Encrypted_Password = new Guid("{05c74c5e-4deb-43b4-bd9f-86664c2a7fd5}"); 
        public static Guid Generate_RSoP_Logging = new Guid("{b7b1b3de-ab09-4242-9e30-9980e5d322f7}"); 
        public static Guid Generate_RSoP_Planning = new Guid("{b7b1b3dd-ab09-4242-9e30-9980e5d322f7}"); 
        public static Guid Manage_Optional_Features = new Guid("{7c0e2a7c-a419-48e4-a995-10180aad54dd}"); 
        public static Guid Migrate_SID_History = new Guid("{ba33815a-4f93-4c76-87f3-57574bff8109}"); 
        public static Guid msmq_Open_Connector = new Guid("{b4e60130-df3f-11d1-9c86-006008764d0e}"); 
        public static Guid msmq_Peek = new Guid("{06bd3201-df3e-11d1-9c86-006008764d0e}"); 
        public static Guid msmq_Peek_computer_Journal = new Guid("{4b6e08c3-df3c-11d1-9c86-006008764d0e}"); 
        public static Guid msmq_Peek_Dead_Letter = new Guid("{4b6e08c1-df3c-11d1-9c86-006008764d0e}"); 
        public static Guid msmq_Receive = new Guid("{06bd3200-df3e-11d1-9c86-006008764d0e}"); 
        public static Guid msmq_Receive_computer_Journal = new Guid("{4b6e08c2-df3c-11d1-9c86-006008764d0e}"); 
        public static Guid msmq_Receive_Dead_Letter = new Guid("{4b6e08c0-df3c-11d1-9c86-006008764d0e}"); 
        public static Guid msmq_Receive_journal = new Guid("{06bd3203-df3e-11d1-9c86-006008764d0e}"); 
        public static Guid msmq_Send = new Guid("{06bd3202-df3e-11d1-9c86-006008764d0e}"); 
        public static Guid Open_Address_Book = new Guid("{a1990816-4298-11d1-ade2-00c04fd8d5cd}"); 
        public static Guid Read_Only_Replication_Secret_Synchronization = new Guid("{1131f6ae-9c07-11d1-f79f-00c04fc2dcd2}"); 
        public static Guid Reanimate_Tombstones = new Guid("{45ec5156-db7e-47bb-b53f-dbeb2d03c40f}"); 
        public static Guid Recalculate_Hierarchy = new Guid("{0bc1554e-0a99-11d1-adbb-00c04fd8d5cd}"); 
        public static Guid Recalculate_Security_Inheritance = new Guid("{62dd28a8-7f46-11d2-b9ad-00c04f79f805}"); 
        public static Guid Receive_As = new Guid("{ab721a56-1e2f-11d0-9819-00aa0040529b}"); 
        public static Guid Refresh_Group_Cache = new Guid("{9432c620-033c-4db7-8b58-14ef6d0bf477}"); 
        public static Guid Reload_SSL_Certificate = new Guid("{1a60ea8d-58a6-4b20-bcdc-fb71eb8a9ff8}"); 
        public static Guid Run_Protect_Admin_Groups_Task = new Guid("{7726b9d5-a4b4-4288-a6b2-dce952e80a7f}"); 
        public static Guid SAM_Enumerate_Entire_Domain = new Guid("{91d67418-0135-4acc-8d79-c08e857cfbec}"); 
        public static Guid Send_As = new Guid("{ab721a54-1e2f-11d0-9819-00aa0040529b}"); 
        public static Guid Send_To = new Guid("{ab721a55-1e2f-11d0-9819-00aa0040529b}"); 
        public static Guid Unexpire_Password = new Guid("{ccc2dc7d-a6ad-4a7a-8846-c04e3cc53501}"); 
        public static Guid Update_Password_Not_Required_Bit = new Guid("{280f369c-67c7-438e-ae98-1d46f3c6f541}"); 
        public static Guid Update_Schema_Cache = new Guid("{be2bb760-7f46-11d2-b9ad-00c04f79f805}"); 
        public static Guid User_Change_Password = new Guid("{ab721a53-1e2f-11d0-9819-00aa0040529b}"); 
        public static Guid User_Force_Change_Password = new Guid("{00299570-246d-11d0-a768-00aa006e0529}"); 
        public static Guid DS_Clone_Domain_Controller = new Guid("{3e0f7e18-2c7a-4c10-ba82-4d926db99a3e}"); 
        public static Guid DS_Read_Partition_Secrets = new Guid("{084c93a2-620d-4879-a836-f0ae47de0e89}"); 
        public static Guid DS_Write_Partition_Secrets = new Guid("{94825a8d-b171-4116-8146-1e34d8f54401}"); 
        public static Guid DS_Set_Owner = new Guid("{4125c71f-7fac-4ff0-bcb7-f09a41325286}"); 
        public static Guid DS_Bypass_Quota = new Guid("{88a9933e-e5c8-4f2a-9dd7-2527416b8092}"); 
    }

    /// <summary>
    /// Class contains all the Validated Writes
    /// </summary>
    public class ValidatedWrite
    {
        public static Guid Self_Membership = new Guid("{bf9679c0-0de6-11d0-a285-00aa003049e2}");
        public static Guid Validated_DNS_Host_Name = new Guid("{72e39547-7b18-11d1-adef-00c04fd8d5cd}");
        public static Guid Validated_MS_DS_Additional_DNS_Host_Name = new Guid("{80863791-dbe9-4eb8-837e-7f0ab55d9ac7}");
        public static Guid Validated_MS_DS_Behavior_Version = new Guid("{d31a8757-2447-4545-8081-3bb610cacbf2}");
        public static Guid Validated_SPN = new Guid("{f3a64788-5306-11d1-a9c5-0000f80367c1}");
    }

    /// <summary>
    /// Class contains all the LDAP Capabilities
    /// </summary>
    public class LdapCapability
    {
        public const string LDAP_CAP_ACTIVE_DIRECTORY_OID = "1.2.840.113556.1.4.800";
        public const string LDAP_CAP_ACTIVE_DIRECTORY_LDAP_INTEG_OID = "1.2.840.113556.1.4.1791";
        public const string LDAP_CAP_ACTIVE_DIRECTORY_V51_OID = "1.2.840.113556.1.4.1670";
        public const string LDAP_CAP_ACTIVE_DIRECTORY_ADAM_DIGEST_OID = "1.2.840.113556.1.4.1880";
        public const string LDAP_CAP_ACTIVE_DIRECTORY_ADAM_OID = "1.2.840.113556.1.4.1851";
        public const string LDAP_CAP_ACTIVE_DIRECTORY_PARTIAL_SECRETS_OID = "1.2.840.113556.1.4.1920";
        public const string LDAP_CAP_ACTIVE_DIRECTORY_V60_OID = "1.2.840.113556.1.4.1935";
        public const string LDAP_CAP_ACTIVE_DIRECTORY_V61_R2_OID = "1.2.840.113556.1.4.2080";
        public const string LDAP_CAP_ACTIVE_DIRECTORY_W8_OID = "1.2.840.113556.1.4.2237";
    }

    #endregion

    #region Extended Operations

    /// <summary>
    /// Error status returned when an extended operation is performed.
    /// </summary>
    public enum ExtendedOperationResponse
    {
        /// <summary>
        /// Valid response
        /// </summary>
        Valid,

        /// <summary>
        /// InValid response
        /// </summary>
        InValid,

        /// <summary>
        /// Represents an un-supported operation
        /// </summary>
        IsNotSupported
    }

    #endregion

    #region Enums For Search Operation

    /// <summary>
    /// Enum that defines the search scope
    /// </summary>
    public enum SearchScope
    {
        /// <summary>
        /// Represents the Scope of search as Base level.
        /// </summary>
        baseObject,

        /// <summary>
        /// Represents the scope of search as single level.
        /// </summary>
        SingleLevel,

        /// <summary>
        /// Represents the scope of the search as whole tree.
        /// </summary>
        WholeTree,

        /// <summary>
        /// Represents the scope of the search as subtree.
        /// </summary>
        Subtree
    }

    /// <summary>
    /// Enum that represents the search response.
    /// </summary>
    public enum SearchResp
    {
        /// <summary>
        /// Represents successful retrieval of search results.
        /// </summary>
        retrievalSuccessful,

        /// <summary>
        /// Represents unsuccessful retrieval of search results
        /// </summary>
        retreivalUnsuccessful
    }

    #endregion

    #region Enums For Add Operation

    /// <summary>
    /// Rights on NC
    /// </summary>
    public enum NCRight
    {
        /// <summary>
        /// Right to add GUID
        /// </summary>
        RIGHT_DS_ADD_GUID,

        /// <summary>
        /// not a valid right
        /// </summary>
        notAValidRight
    }

    /// <summary>
    /// Enum that specifies the AD privileges
    /// </summary>
    public enum Privileges
    {
        /// <summary>
        /// Privilege which is must to have if msDS-AllowedToDelegateTo attribute is to be specified as part of an add operation
        /// </summary>
        SE_ENABLE_DELGATION_PRIVELEGE
    }

    /// <summary>
    /// Enum that specifies the Constraints on Add Operation
    /// </summary>
    public enum ConstrOnAddOpErrs
    {
        /// <summary>
        /// Right specifying insufficient access rights (if security considerations are not met)
        /// </summary>
        insufficientAccessRights,

        #region Non-Windows

        /// <summary>
        /// Specifies Bad InstanceType
        /// </summary>
        UnwillingToPerform_UnKnownError,

        /// <summary>
        /// Represents the absence of parent object
        /// </summary>
        NoSuchObject_UnKnownError,

        /// <summary>
        /// if objectClass values has an unknown or defunct class
        /// </summary>
        NoSuchAttribute_UnKnownError,

        /// <summary>
        /// if objectClass is not present
        /// </summary>
        ObjectClassViolation_UnKnownError,

        /// <summary>
        /// Represents an error code returned if the object to be added already exists.
        /// </summary>
        EntryAlreadyExists_UnKnownError,

        /// <summary>
        /// naming violation
        /// </summary>
        NamingViolation_UnKnownError,

        /// <summary>
        /// invalid syntax for distinguishedname
        /// </summary>
        InvalidDNSyntax_UnKnownError,

        /// <summary>
        /// Represents an error code ConstraintViolation_ERROR_DS_ATTRIBUTE_OWNED_BY_SAM in Non-Windows.
        /// </summary>
        ConstraintViolation_UnKnownError,

        /// <summary>
        /// Represents an error code if the userPrincipalName to be added is not unique.
        /// </summary>
        AttributeOrValueExists_UnKnownError,

        #endregion

        #region Windows

        /// <summary>
        /// Specifies Bad InstanceType
        /// </summary>
        UnwillingToPerform_ERROR_DS_BAD_INSTANCE_TYPE,

        /// <summary>
        /// unwillingToPerform with error code ERROR_DS_NOT_SUPPORTED
        /// </summary>
        UnwillingToPerform_ERROR_DS_NOT_SUPPORTED,

        /// <summary>
        /// Specifies that the object DN does not match the rdnTypeId
        /// </summary>
        NamingViolation_ERROR_DS_RDN_DOESNT_MATCH_SCHEMA,

        /// <summary>
        /// specifies IT_WRITE is not set even if IT_NC_HEAD is set
        /// </summary>
        UnwillingToPerform_ERROR_DS_ADD_REPLICA_INHIBITED,

        /// <summary>
        /// If instanceType attribute value is specified with multiple integer values (more than one)
        /// </summary>
        UnwillingToPerform_ERROR_DS_BAD_NAME_SYNTAX,

        /// <summary>
        /// Represents the absence of parent object
        /// </summary>
        NoSuchObject_ERROR_DS_OBJ_NOT_FOUND,

        /// <summary>
        /// Denotes if objectClass attribute is not present
        /// </summary>
        UnwillingToPerform_ERROR_DS_OBJ_CLASS_NOT_DEFINED,

        /// <summary>
        /// Specifies if more than one ObjectClass attribute values are defined
        /// </summary>
        UnwillingToPerform_ERROR_DS_ILLEGAL_MOD_OPERATION,

        /// <summary>
        /// Specifies if an attribute with an inactive or defunct attribute schema is populated
        /// </summary>
        UnwillingToPerform_ERROR_DS_ATT_NOT_DEF_IN_SCHEMA,

        /// <summary>
        /// Specifies security illegal modification
        /// </summary>
        UnwillingToPerform_ERROR_DS_SECURITY_ILLEGAL_MODIFY,

        /// <summary>
        /// Specifies if an object is created for system only class.
        /// </summary>
        UnwillingToPerform_ERROR_DS_CANT_ADD_SYSTEM_ONLY,

        /// <summary>
        /// Specifies if an object is created for system only class.
        /// </summary>
        UnwillingToPerform_ERROR_DS_CLASS_MUST_BE_CONCRETE,

        /// <summary>
        /// NamingViolation / ERROR_DS_ILLEGAL_SUPERIOR.
        /// </summary>
        NamingViolation_ERROR_DS_ILLEGAL_SUPERIOR,

        /// <summary>
        /// ObjectClassViolation / ERROR_DS_ILLEGAL_MOD_OPERATION
        /// </summary>
        ObjectClassViolation_ERROR_DS_ILLEGAL_MOD_OPERATION,

        /// <summary>
        /// ObjectClassViolation / ERROR_DS_OBJ_CLASS_NOT_DEFINED
        /// </summary>
        ObjectClassViolation_ERROR_DS_OBJ_CLASS_NOT_DEFINED,

        /// <summary>
        /// ObjectClassViolation / ERROR_DS_OBJECT_CLASS_REQUIRED
        /// </summary>
        objectClassViolation_ERROR_DS_OBJECT_CLASS_REQUIRED,

        /// <summary>
        /// NoSuchAttribute / ERROR_INVALID_PARAMETER
        /// </summary>
        NoSuchAttribute_ERROR_INVALID_PARAMETER,

        /// <summary>
        /// UnwillingToPerform / ERROR_DS_RDN_DOESNT_MATCH_SCHEMA
        /// </summary>
        UnwillingToPerform_ERROR_DS_RDN_DOESNT_MATCH_SCHEMA,

        /// <summary>
        /// ObjectClassViolation / ERROR_DS_OBJ_CLASS_NOT_SUBCLASS
        /// </summary>
        ObjectClassViolation_ERROR_DS_OBJ_CLASS_NOT_SUBCLASS,

        /// <summary>
        /// UnwillingToPerform / ERROR_DS_UNWILLING_TO_PERFORM
        /// </summary>
        UnwillingToPerform_ERROR_DS_UNWILLING_TO_PERFORM,

        /// <summary>
        /// NamingViolation / ERROR_DS_NAME_UNPARSEABLE
        /// </summary>
        NamingViolation_ERROR_DS_NAME_UNPARSEABLE,

        /// <summary>
        /// Represents an error code if the userPrincipalName to be added is not unique.
        /// </summary>
        AttributeOrValueExists_ERROR_DS_NAME_NOT_UNIQUE,

        /// <summary>
        /// Constraint Violation
        /// </summary>
        ConstraintViolation_ERROR_DS_UPN_VALUE_NOT_UNIQUE_IN_FOREST,

        /// <summary>
        /// Constraint Violation
        /// </summary>
        ConstraintViolation_ERROR_DS_SPN_VALUE_NOT_UNIQUE_IN_FOREST,

        /// <summary>
        /// ConstraintViolation
        /// </summary>
        ConstraintViolation_ERROR_DS_ATTRIBUTE_OWNED_BY_SAM,

        /// <summary>
        /// Represents an error code if the RDN value is not valid in subnet object.
        /// </summary>
        InvalidDNSyntax_ERROR_DS_BAD_NAME_SYNTAX,

        /// <summary>
        /// Represents an error code if the objectClass attribute is specified more than once
        /// </summary>
        AttributeOrValueExists_ERROR_DS_ATT_ALREADY_EXISTS,

        #endregion

        /// <summary>
        /// Represents a success response
        /// </summary>
        success,

        /// <summary>
        /// UnSpecified error in the TD
        /// </summary>
        unSpecifiedError
    }

    /// <summary>
    /// Enum that specifies, on which Special Classes add operation needs to be performed
    /// </summary>
    public enum SpecialClasses
    {
        /// <summary>
        /// Represents LSA specific object classes
        /// </summary>
        LSASpecificObjectClasses,

        /// <summary>
        /// Represents SAM specific object classes
        /// </summary>
        SAMSpecificObjectClasses,

        /// <summary>
        /// Represents Schema object classes
        /// </summary>
        SchemaObjectClasses,

        /// <summary>
        /// Represents Not a special class
        /// </summary>
        NotASpecialClass
    }

    #endregion

    #region Enums for Modify Operation

    /// <summary>
    /// Enum containing the access rights on attributes
    /// </summary>
    public enum RightsOnAttributes
    {
        /// <summary>
        /// Represents access right on attribute to modify.along with SE_ENABLE_DELEGATION_PRIVILEGE
        /// </summary>
        RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,

        /// <summary>
        /// Represents access right on attribute to modify.without SE_ENABLE_DELEGATION_PRIVILEGE
        /// </summary>
        RIGHT_DS_WRITE_PROPERTYwithOutSE_ENABLE_DELEGATION_PRIVILEGE,

        /// <summary>
        /// Represents Extended rights
        /// </summary>
        RIGHT_DS_WRITE_PROPERTY_EXTENDED,

        /// <summary>
        /// Represents an arbitrary right that doesn't satisfy any of the required rights
        /// </summary>
        unSpecifiedRights
    }

    /// <summary>
    /// Enum containing the error codes returned according to the constraints on the Modify operations
    /// </summary>
    public enum ConstrOnModOpErrs
    {
        /// <summary>
        /// Right specifying insufficient access rights (if security considerations are not met)
        /// </summary>
        insufficientAccessRights,

        /// <summary>
        /// Represents a series of violated constraints .
        /// </summary>
        UnwillingToPerform_UnKnownError,

        /// <summary>
        /// UnwillingToPerform / ERROR_DS_ILLEGAL_MOD_OPERATION
        /// </summary>
        UnwillingToPerform_ERROR_DS_ILLEGAL_MOD_OPERATION,

        /// <summary>
        /// UnwillingToPerform / ERROR_DS_LOW_DSA_VERSION
        /// </summary>
        UnwillingToPerform_ERROR_DS_LOW_DSA_VERSION,

        /// <summary>
        /// UnwillingToPerform / ERROR_DS_NOT_SUPPORTED
        /// </summary>
        UnwillingToPerform_ERROR_DS_NOT_SUPPORTED,

        /// <summary>
        /// UnwillingToPerform / ERROR_DS_ILLEGAL_MOD_OPERATION
        /// </summary>
        UnwillingToPerform_ERROR_DS_SECURITY_ILLEGAL_MODIFY,

        /// <summary>
        /// UnwillingToPerform_ERROR_DS_CONSTRUCTED_ATT_MOD
        /// </summary>
        UnwillingToPerform_ERROR_DS_CONSTRUCTED_ATT_MOD,

        /// <summary>
        /// unwillingToPerform / ERROR_DS_INVALID_ROLE_OWNER
        /// </summary>
        UnwillingToPerform_ERROR_DS_INVALID_ROLE_OWNER,

        /// <summary>
        /// UnwillingToPerform / ERROR_DS_ILLEGAL_BASE_SCHEMA_MOD
        /// </summary>
        UnwillingToPerform_ERROR_DS_ILLEGAL_BASE_SCHEMA_MOD,

        /// <summary>
        /// UnwillingToPerform / ERROR_DS_ILLEGAL_SUPERIOR
        /// </summary>
        UnwillingToPerform_ERROR_DS_ILLEGAL_SUPERIOR,

        /// <summary>
        /// UnwillingToPerform / ERROR_DS_UNWILLING_TO_PERFORM
        /// </summary>
        UnwillingToPerform_ERROR_DS_UNWILLING_TO_PERFORM,

        /// <summary>
        /// UnwillingToPerform / ERROR_INVALID_PARAMETER
        /// </summary>
        UnwillingToPerform_ERROR_INVALID_PARAMETER,

        UnwillingToPerform_ERROR_DS_HIGH_DSA_VERSION,

        /// <summary>
        /// AttributeOrValueExists / ERROR_DS_ATT_VAL_ALREADY_EXISTS
        /// </summary>
        AttributeOrValueExists_ERROR_DS_ATT_VAL_ALREADY_EXISTS,

        /// <summary>
        /// AttributeOrValueExists_UnKnownError
        /// </summary>
        AttributeOrValueExists_UnKnownError,

        /// <summary>
        /// If not a writable NC replica
        /// </summary>
        referral_UnKnownError,

        /// <summary>
        /// If not a writable NC replica
        /// </summary>
        referral_ERROR_DS_REFERRAL,

        /// <summary>
        /// Undocumented Error
        /// </summary>
        UnspecifiedError,

        /// <summary>
        /// Represents a success response
        /// </summary>
        success,

        /// <summary>
        /// Returned if the modify operation is performed on a non-existing object
        /// </summary>
        NoSuchObject_UnKnownError,

        /// <summary>
        /// MultiValuedRDN error
        /// </summary>
        NotAllowedOnRdn_UnKnownError,

        /// <summary>
        /// NotAllowedOnRDN / ERROR_DS_CANT_MOD_SYSTEM_ONLY
        /// </summary>
        NotAllowedOnRDN_ERROR_DS_CANT_MOD_SYSTEM_ONLY,

        /// <summary>
        /// NoSuchAttribute / ERROR_DS_ATT_IS_NOT_ON_OBJ
        /// </summary>
        NoSuchAttribute_ERROR_DS_ATT_IS_NOT_ON_OBJ,

        /// <summary>
        /// NoSuchAttribute_UnKnownError
        /// </summary>
        NoSuchAttribute_UnKnownError,

        /// <summary>
        /// NoSuchAttribute / ERROR_DS_CANT_REM_MISSING_ATT_VAL
        /// </summary>
        NoSuchAttribute_ERROR_DS_CANT_REM_MISSING_ATT_VAL,

        /// <summary>
        /// NoSuchAttribute / ERROR_INVALID_PARAMETER
        /// </summary>
        NoSuchAttribute_ERROR_INVALID_PARAMETER,

        /// <summary>
        /// ConstraintViolation / ERROR_DS_OBJ_CLASS_VIOLATION
        /// </summary>
        ConstraintViolation_ERROR_DS_OBJ_CLASS_VIOLATION,

        /// <summary>
        /// Represents the error code when the modified userPrincipalName is not unique.
        /// </summary>
        ConstraintViolation_ERROR_DS_NAME_NOT_UNIQUE,

        /// <summary>
        /// Represents the error code when the modified userPrincipalName is not unique
        /// And the error code when pwdLastSet attribute is modified with invalid value.
        /// </summary>
        ConstraintViolation_UnKnownError,

        /// <summary>
        /// Represents an error code when pwdLastSet attribute is modified with invalid value.
        /// </summary>
        ConstraintViolation_ERROR_INVALID_PARAMETER,

        /// <summary>
        /// Constraint Violation
        /// </summary>
        ConstraintViolation_ERROR_DS_CONSTRAINT_VIOLATION,

        /// <summary>
        /// Constraint Violation
        /// </summary>
        ConstraintViolation_ERROR_DS_CANT_MOD_SYSTEM_ONLY,

        /// <summary>
        /// Constraint Violation
        /// </summary>
        ConstraintViolation_ERROR_DS_CONSTRUCTED_ATT_MOD,

        /// <summary>
        /// Constraint Violation
        /// </summary>
        ConstraintViolation_ERROR_DS_UPN_VALUE_NOT_UNIQUE_IN_FOREST,

        /// <summary>
        /// Constraint Violation
        /// </summary>
        ConstraintViolation_ERROR_DS_SPN_VALUE_NOT_UNIQUE_IN_FOREST,

        /// <summary>
        /// OperationsError
        /// </summary>
        OperationsError,

        /// <summary>
        /// OperationsError / ERROR_DS_GENERIC_ERROR
        /// </summary>
        OperationsError_ERROR_DS_GENERIC_ERROR,

        /// <summary>
        /// OperationsError / ERROR_DS_OBJ_NOT_FOUND
        /// </summary>
        OperationsError_ERROR_DS_OBJ_NOT_FOUND,

        /// <summary>
        /// Represents error objectClassViolation / ERROR_DS_OBJECT_CLASS_REQUIRED
        /// </summary>
        ObjectClassViolation_ERROR_DS_OBJECT_CLASS_REQUIRED,
        /// <summary>
        /// Represents error ObjectClassViolation / ERROR_DS_ILLEGAL_MOD_OPERATION
        /// </summary>
        ObjectClassViolation_ERROR_DS_ILLEGAL_MOD_OPERATION,

        /// <summary>
        /// Represents error objectClassViolation / ERROR_DS_OBJ_CLASS_NOT_SUBCLASS
        /// </summary>
        ObjectClassViolation_ERROR_DS_OBJ_CLASS_NOT_SUBCLASS,

        /// <summary>
        /// Represents error objectClassViolation / ERROR_DS_OBJECT_CLASS_REQUIRED in Non-Windows case
        /// </summary>
        ObjectClassViolation_UnKnownError,

        /// <summary>
        /// Represents undefinedAttributeType / ERROR_DS_ATT_NOT_DEF_IN_SCHEMA
        /// </summary>
        UndefinedAttributeType_ERROR_DS_ATT_NOT_DEF_IN_SCHEMA,

        /// <summary>
        /// Represents undefinedAttributeType / ERROR_DS_ATT_NOT_DEF_IN_SCHEMA in Non-Windows
        /// </summary>
        UndefinedAttributeType_UnKnownError
    }

    #endregion

    #region Enums For Delete Operation

    /// <summary>
    /// Enum that contains Errors that are supposed to be returned as part of constraints of Delete operation
    /// </summary>
    public enum ConstrOnDelOpErr
    {
        /// <summary>
        /// Common Error returned if any of the constraints are not met.
        /// </summary>
        UnwillingToPerform_UnKnownError,

        /// <summary>
        /// Common Error returned if any of the constraints are not met.(windows)
        /// </summary>
        UnwillingToPerform_ERROR_DS_CANT_DELETE,

        /// <summary>
        /// NotAllowedOnNonleaf / ERROR_DS_CHILDREN_EXIST
        /// </summary>
        NotAllowedOnNonLeaf_ERROR_DS_CHILDREN_EXIST,

        /// <summary>
        /// UnwillingToPerform / ERROR_DS_ILLEGAL_MOD_OPERATION
        /// </summary>
        UnwillingToPerform_ERROR_DS_ILLEGAL_MOD_OPERATION,

        /// <summary>
        /// Represents error returned if the object does not reside in a writable NC replica.
        /// </summary>
        Referral_UnKnownError,

        /// <summary>
        /// Represents error returned if the object does not reside in a writable NC replica.(windows)
        /// </summary>
        Referral_ERROR_DS_REFERRAL,

        /// <summary>
        /// Represents insufficient rights
        /// </summary>
        insufficientAccessRights,

        /// <summary>
        /// Represents an undocumented error
        /// </summary>
        UnSpecifiedError,

        /// <summary>
        /// NoSuchObject
        /// </summary>
        NoSuchObject,

        /// <summary>
        /// NotAllowedOnNonleaf / ERROR_DS_CHILDREN_EXIST (Non-Windows)
        /// </summary>
        NotAllowedOnNonLeaf_UnKnownError,

        /// <summary>
        /// Represents a successful operation
        /// </summary>
        success
    }

    #endregion

    #region Enums for ModifyDN Operation

    /// <summary>
    /// Enum bearing all the error status messages with respect to the ModifyDN operation.
    /// </summary>
    public enum ConstrOnModDNOpErrs
    {
        /// <summary>
        /// Represents a success response
        /// </summary>
        Success,

        /// <summary>
        /// Represents an undocumented error
        /// </summary>
        UnSpecifiedError,

        /// <summary>
        /// Represents the error returned if the modifying object is a schema object.
        /// </summary>
        UnwillingToPerform_ERROR_DS_ILLEGAL_MOD_OPERATION,

        /// <summary>
        /// Represents the error ProtocolError / ERROR_INVALID_PARAMETER.
        /// </summary>
        ProtocolError_ERROR_INVALID_PARAMETER,

        /// <summary>
        /// Represents the error ProtocolError / ERROR_INVALID_PARAMETER in case of Non-Windows.
        /// </summary>
        ProtocolError_UnKnownError,

        /// <summary>
        /// Represents the error returned if any of the security considerations are not met.
        /// </summary>
        InsufficientAccessRights_UnKnownError,

        /// <summary>
        /// Represents the error returned if Kerberos delegation is not enabled on the LDAP connection.
        /// </summary>
        InappropriateAuthentication_UnKnownError,

        /// <summary>
        /// Represents an error code returned if the object to be added already exists.
        /// </summary>
        EntryAlreadyExists_UnKnownError,

        /// <summary>
        /// Represents an error code returned if distinguishedname is of invalid syntax.
        /// </summary>
        InvalidDNSyntax_UnKnownError,

        /// <summary>
        /// Represents an error code returned if parent object does not exist.
        /// </summary>
        Other_ERROR_DS_NO_PARENT_OBJECT,

        /// <summary>
        /// Represents an error code returned if the object to be moved is not present.
        /// </summary>
        NoSuchObject_ERROR_DS_OBJ_NOT_FOUND,

        /// <summary>
        /// Represents an error code saying it is disallowed to move object from System Container.
        /// </summary>
        Other_ERROR_DS_DISALLOWED_IN_SYSTEM_CONTAINER,

        /// <summary>
        /// Represents an error code if the secret or trusted domain object is being moved.
        /// </summary>
        UnwillingToPerform_UnKnownError,

        /// <summary>
        /// Represents an error code returned if parent object does not exist in case of Non Windows.
        /// </summary>
        Other_UnKnownError,

        /// <summary>
        /// Represents an error code returned if the object to be moved is not present in case of Non Windows.
        /// </summary>
        NoSuchObject_UnKnownError,

        /// <summary>
        /// If systemOnly of objectClass value is not set to false on an object, then the server returns this error.
        /// </summary>
        UnwillingToPerform_ERROR_DS_CANT_DELETE,

        /// <summary>
        /// Represents error unwillingToPerform / ERROR_DS_ILLEGAL_XDOM_MOVE_OPERATION.
        /// </summary>
        UnwillingToPerform_ERROR_DS_ILLEGAL_XDOM_MOVE_OPERATION,

        /// <summary>
        /// Represents error unwillingToPerform / ERROR_DS_ILLEGAL_BASE_SCHEMA_MOD.
        /// </summary>
        UnwillingToPerform_ERROR_DS_ILLEGAL_BASE_SCHEMA_MOD,

        /// <summary>
        /// Represents error unwillingToPerform / ERROR_DS_CANT_MOVE_ACCOUNT_GROUP
        /// </summary>
        UnwillingToPerform_ERROR_DS_CANT_MOVE_ACCOUNT_GROUP,

        /// <summary>
        /// Represents error unwillingToPerform / ERROR_DS_MODIFYDN_DISALLOWED_BY_FLAG
        /// </summary>
        UnwillingToPerform_ERROR_DS_MODIFYDN_DISALLOWED_BY_FLAG,

        /// <summary>
        /// Represents error unwillingToPerform / ERROR_DS_CANT_MOVE_RESOURCE_GROUP
        /// </summary>
        UnwillingToPerform_ERROR_DS_CANT_MOVE_RESOURCE_GROUP,

        /// <summary>
        /// Represents error unwillingToPerform / ERROR_DS_CANT_MOVE_APP_QUERY_GROUP
        /// </summary>
        UnwillingToPerform_ERROR_DS_CANT_MOVE_APP_QUERY_GROUP,

        /// <summary>
        /// Represents error noSuchObject / ERROR_DS_CANT_FIND_EXPECTED_NC
        /// </summary>
        NoSuchObject_ERROR_DS_CANT_FIND_EXPECTED_NC,

        /// <summary>
        /// Represents error unwillingToPerform / ERROR_DS_CANT_WITH_ACCT_GROUP_MEMBERSHPS
        /// </summary>
        UnwillingToPerform_ERROR_DS_CANT_WITH_ACCT_GROUP_MEMBERSHPS,

        /// <summary>
        /// Represents error invalidDNSyntax / ERROR_DS_SRC_AND_DST_NC_IDENTICAL
        /// </summary>
        InvalidDNSyntax_ERROR_DS_SRC_AND_DST_NC_IDENTICAL,

        /// <summary>
        /// Represents error notAllowedOnNonLeaf / ERROR_DS_CHILDREN_EXIST
        /// </summary>
        NotAllowedOnNonLeaf_ERROR_DS_CHILDREN_EXIST,

        /// <summary>
        /// Represents error notAllowedOnNonLeaf / ERROR_DS_CHILDREN_EXIST in Non-Windows.
        /// </summary>
        NotAllowedOnNonLeaf_UnKnownError,

        /// <summary>
        /// Represents error unwillingToPerform / ERROR_DS_CANT_MOVE_DELETED_OBJECT
        /// </summary>
        UnwillingToPerform_ERROR_DS_CANT_MOVE_DELETED_OBJECT,

        /// <summary>
        /// Represents error unwillingToPerform / ERROR_INVALID_PARAMETER
        /// </summary>
        UnwillingToPerform_ERROR_INVALID_PARAMETER
    }

    /// <summary>
    /// Enum representing the rights on the existing parent of the modifying object
    /// </summary>
    public enum RightOnOldParentObject
    {
        /// <summary>
        /// Right expected to be granted for the requester on the new parent to perform a move operation.
        /// </summary>
        RIGHT_DS_DELETE_CHILD,

        /// <summary>
        /// Invalid right which is not required.
        /// </summary>
        INVALID_RIGHT
    }

    #endregion
}
