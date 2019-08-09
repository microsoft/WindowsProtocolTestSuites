// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP
{
    #region MS-WSP Enumerators

    /// <summary>
    /// Windows SKU version enum
    /// </summary>
    public enum SkuOsVersion
    {
        /// <summary>
        /// Non Windows OS
        /// </summary>
        NonWindows = 1,

        /// <summary>
        /// Windows NT
        /// </summary>
        WinNT,

        /// <summary>
        /// Windows 2000
        /// </summary>
        Win2K,

        /// <summary>
        /// Windows XP
        /// </summary>
        WinXP,

        /// <summary>
        /// Windows 2003 Server
        /// </summary>
        Win2K3,

        /// <summary>
        /// Windows Vista
        /// </summary>
        WinVista,

        /// <summary>
        /// Windows 2008 Server
        /// </summary>
        Win2K8,

        /// <summary>
        /// Windows 2008 Server R2
        /// </summary>
        Win2K8R2
    }
    /// <summary>
    /// This structure is used to keep server information, such as server platform.
    /// </summary>
    public class ServerInfo
    {
        /// <summary>
        /// Represent the server's platform type
        /// </summary>
        SkuOsVersion serverPlatForm;

        /// <summary>
        /// Constructor
        /// </summary>
        public ServerInfo()
        {
            serverPlatForm = SkuOsVersion.Win2K8;
        }

        /// <summary>
        /// ServerPlatForm property
        /// </summary>
        public SkuOsVersion ServerPlatForm
        {
            get
            {
                return serverPlatForm;
            }

            set
            {
                serverPlatForm = value;
            }
        }

    }
    /// <summary>
    /// Property class
    /// </summary>
    public class Properties
    {
        // OS sku version
        protected static SkuOsVersion _skuOsVersion;

        /// <summary>
        /// OS SKU version
        /// </summary>
        public static SkuOsVersion SkuOsVersion
        {
            set
            {
                _skuOsVersion = value;
            }

            get
            {
                return _skuOsVersion;
            }
        }
    }
    /// <summary>
    /// Update v-aliche
    /// Delta testing
    /// Specifies the WinVersion and NLSVersion.
    /// </summary>
    public enum dwWinNLSVersion
    {
        /// <summary>
        /// WINDOWS_MAJOR_VERSION_6
        /// </summary>
        WINDOWS_MAJOR_VERSION_6 = 0x0000006,
        /// <summary>
        /// WINDOWS_MINOR_VERSION_0
        /// </summary>
        WINDOWS_MINOR_VERSION_0 = 0x0000000,
        /// <summary>
        /// WINDOWS_MINOR_VERSION_1
        /// </summary>
        WINDOWS_MINOR_VERSION_1 = 0x0000001,
        /// <summary>
        /// NLS_VERSION_40500
        /// </summary>
        NLS_VERSION_40500 = 0x00040500,
        /// <summary>
        /// NLS_VERSION_60000
        /// </summary>
        NLS_VERSION_60000 = 0x00060000,
        /// <summary>
        /// NLS_VERSION_60101
        /// </summary>
        NLS_VERSION_60101 = 0x00060101
    }



    /// <summary>
    /// Specifies the query match
    /// </summary>
    public enum UlGenerateMethod
    {
        /// <summary>
        /// Value of ulGenerateMethod field of CContentRestriction structure
        /// </summary>
        GENERATE_METHOD_EXACT = 0x00000000,
        /// <summary>
        /// Value of ulGenerateMethod field of CContentRestriction structure
        /// </summary>
        GENERATE_METHOD_PREFIX = 0x00000001,
        /// <summary>
        /// Value of ulGenerateMethod field of CContentRestriction structure
        /// </summary>
        GENERATE_METHOD_INFLECT = 0x00000002
    }
    /// <summary>
    /// Specifies Kind of Catalog Properties
    /// </summary>
    public enum UlKind
    {
        /// <summary>
        /// Value of uLKind field of CFullPropSpec structure specifies 
        /// the number of non-NULL characters in the Property name field 
        /// </summary>
        PRSPEC_LPWSTR = 0x00000000,
        /// <summary>
        /// Value of uLKind field if CFullPropSpec structure specifies 
        /// The PrSpec field of CFullPropSpec specifies the property ID
        /// </summary>
        PRSPEC_PROPID = 0x00000001
    }
    /// <summary>
    /// Base Storage Variant Types
    /// </summary>
    public enum StorageType : ushort
    {
        /// <summary>
        /// Value is not present 
        /// </summary>
        VT_EMPTY = 0x0000,
        /// <summary>
        /// Value is not present 
        /// </summary>
        VT_NULL = 0x0001,
        /// <summary>
        /// A 1-byte signed integer
        /// </summary>
        VT_I1 = 0x0010,
        /// <summary>
        /// A 1-byte unsigned integer
        /// </summary>
        VT_UI1 = 0x0011,
        /// <summary>
        /// A 2-byte signed integer
        /// </summary>
        VT_I2 = 0x0002,
        /// <summary>
        /// A 2-byte unsigned integer
        /// </summary>
        VT_UI2 = 0x0012,
        /// <summary>
        /// A Boolean value
        /// </summary>
        VT_BOOL = 0x000B,
        /// <summary>
        /// A 4-byte signed integer
        /// </summary>
        VT_I4 = 0x0003,
        /// <summary>
        /// A 4-byte unsigned integer
        /// </summary>
        VT_UI4 = 0x0013,
        /// <summary>
        /// A 4-byte unsigned integer
        /// </summary>
        VT_R4 = 0x0004,
        /// <summary>
        /// A 4-byte signed integer
        /// </summary>
        VT_INT = 0x0016,
        /// <summary>
        /// A 4-byte unsigned intege
        /// </summary>
        VT_UINT = 0x0017,
        /// <summary>
        /// A 4-byte unsigned integer containing an HRESULT
        /// </summary>
        VT_ERROR = 0x000A,
        /// <summary>
        /// An 8-byte signed integer
        /// </summary>
        VT_I8 = 0x0014,
        /// <summary>
        /// An 8-byte unsigned integer
        /// </summary>
        VT_UI8 = 0x0015,
        /// <summary>
        /// An IEEE 64-bit floating point number
        /// </summary>
        VT_R8 = 0x0005,
        /// <summary>
        /// An 8-byte two's complement integer
        /// </summary>
        VT_CY = 0x0006,
        /// <summary>
        /// A 64-bit floating point number representing the number of days
        /// </summary>
        VT_DATE = 0x0007,
        /// <summary>
        /// A 64-bit integer representing the number of 100-nanosecond intervals
        /// </summary>
        VT_FILETIME = 0x0040,
        /// <summary>
        /// A DECIMAL structure
        /// </summary>
        VT_DECIMAL = 0x000E,
        /// <summary>
        /// A 16-byte binary value containing a GUID
        /// </summary>
        VT_CLSID = 0x0048,
        /// <summary>
        /// A 4-byte unsigned integer count of bytes in the blob
        /// </summary>
        VT_BLOB = 0x0041,
        /// <summary>
        /// A 4-byte unsigned integer count of bytes in the string
        /// </summary>
        VT_BSTR = 0x0008,
        /// <summary>
        /// A null-terminated ANSI string
        /// </summary>
        VT_LPSTR = 0x001E,
        /// <summary>
        /// A null-terminated Unicode string
        /// </summary>
        VT_LPWSTR = 0x001F,
        /// <summary>
        /// A compressed version of a null-terminated Unicode string
        /// </summary>
        VT_COMPRESSED_LPWSTR = 0x0023,
        /// <summary>
        /// Type of CBaseStorageVariant
        /// </summary>
        VT_VARIANT = 0x000C

    }

    /// <summary>
    /// priority of CPMSetScopePrioritizationIn
    /// </summary>
    public enum Priority : uint
    {
        /// <summary>
        /// Specifies Process items that may be relevant to the originating query before others as quickly as possible.
        /// </summary>
        PRIORITY_LEVEL_FOREGROUND = 0,

        /// <summary>
        /// Specifies Process items that may be relevant to the originating query before others at the normal rate.
        /// </summary>
        PRIORITY_LEVEL_HIGH = 1,

        /// <summary>
        /// Specifies Process items that may be relevant to the originating query before others, but after any other prioritization requests at the normal rate.
        /// </summary>
        PRIORITY_LEVEL_LOW = 2,

        /// <summary>
        /// Specifies Process items at the normal rate.
        /// </summary>
        PRIORITY_LEVEL_DEFAULT = 3
    }

    /// <summary>
    /// Event Type in CPMGetRowserNotifyIn
    /// </summary>
    public enum EventType : int
    {
        /// <summary>
        /// Specifies the response indicates that no available rowset events
        /// </summary>
        PROPAGATE_NONE = 0,

        /// <summary>
        /// Specifies the response indicates that an item was added
        /// </summary>
        PROPAGATE_ADD = 1,

        /// <summary>
        /// Specifies the response indicates that an item was deleted
        /// </summary>
        PROPAGATE_DELETE = 2,

        /// <summary>
        /// Specifies the response indicates that an item was re-indexed
        /// </summary>
        PROPAGATE_MODIFY = 3,

        /// <summary>
        /// Specifies the response is a rowset specific notification
        /// </summary>
        PROPAGATE_ROWSET = 4
    }

    /// <summary>
    /// Value of EventType
    /// </summary>
    public enum EventTypeValue : uint
    {
        /// <summary>
        /// Specifies the value when eventType is PROPAGATE_ADD
        /// </summary>
        PROPAGATE_ADD = 2,
        /// <summary>
        /// Specifies the value when eventType is PROPAGATE_DELETE
        /// </summary>
        PROPAGATE_DELETE = 4,
        /// <summary>
        /// Specifies the value when eventType is PROPAGATE_MODIFY
        /// </summary>
        PROPAGATE_MODIFY = 6
    }

    /// <summary>
    /// RowsetItemState in CPMGetRowsetNotifyOut
    /// </summary>
    public enum RowsetItemState
    {
        /// <summary>
        /// Specify that the document identifier specified by wid 
        /// MUST not have been contained within the originating rowset
        /// </summary>
        ROWSETEVENT_ITEMSTATE_NOTINROWSET = 0,
        /// <summary>
        /// Specify that the document identifier specified by wid 
        /// MUST be contained within the originating rowset
        /// </summary>
        ROWSETEVENT_ITEMSTATE_INROWSET = 1,
        /// <summary>
        /// Specify that the document identifier specified by wid;s containment
        /// within the originating rowset has not been specified
        /// </summary>
        ROWSETEVENT_ITEMSTATE_UNKNOWN = 2
    }

    /// <summary>
    /// RowsetEvent in CPMGetRowsetNotifyOut
    /// </summary>
    public enum RowsetEvent : int
    {
        /// <summary>
        /// The data backing the rowset is no longer valid.
        /// </summary>
        ROWSETEVENT_TYPE_DATAEXPIRED = 0,
        /// <summary>
        /// The rowset no longer has foreground priority and has been reverted to high priority.
        /// </summary>
        ROWSETEVENT_TYPE_FOREGROUNDLOST = 1,
        /// <summary>
        /// The number of indexed items, number of items that need to be indexed, 
        /// or number of items that need re-indexed has changed.
        /// </summary>
        ROWSETEVENT_TYPE_SCOPESTATISTICS = 2
    }

    /// <summary>
    /// Database Property Id
    /// </summary>
    public enum DbPropId
    {
        /// <summary>
        /// Specifies the name of the catalog or catalogs to query
        /// </summary>
        DBPROP_CI_CATALOG_NAME = 0x00000002,
        /// <summary>
        /// Specifies one or more paths to be included in the query
        /// </summary>
        DBPROP_CI_INCLUDE_SCOPES = 0x00000003,
        /// <summary>
        /// Specifies how the paths are to be treated 
        /// </summary>
        DBPROP_CI_SCOPE_FLAGS = 0x00000004,
        /// <summary>
        /// Specifies the type of query using a CDbColId structure
        /// </summary>
        DBPROP_CI_QUERY_TYPE = 0x00000007,
        /// <summary>
        /// Indicates that files in the scope directory
        /// </summary>
        QUERY_DEEP = 0x01,
        /// <summary>
        /// Indicates that the scope is a virtual path
        /// </summary>
        QUERY_VIRTUAL_Path = 0x02
    }
    /// <summary>
    /// Indexing Service Flag
    /// </summary>
    public enum DBPropId_CIFRMWRKCORE_EXT
    {
        /// <summary>
        /// Specifies the names of the computers on which a query is to be processed
        /// </summary>
        DBPROP_MACHINE = 0x00000002,
        /// <summary>
        /// Specifies a connection constant for the WSS
        /// </summary>
        DBPROP_CLIENT_CLSID = 0x00000003
    }
    /// <summary>
    /// RowsIn and Out EKind
    /// </summary>
    public enum Ekind
    {
        /// <summary>
        /// 4 byte unsigned integer 
        /// </summary>
        DBKIND_GUID_NAME = 0x00000000,
        /// <summary>
        /// 4 byte unsigned integer 
        /// </summary>
        DBKIND_GUID_PROPID = 0x00000001
    }
    /// <summary>
    /// ConnectIn Guid Property Set
    /// </summary>
    public enum GuidPropSet
    {
        /// <summary>
        /// File system content index framework property set
        /// </summary>
        DBPROPSET_FSCIFRMWRK_EXT,
        /// <summary>
        /// Content index framework core property set
        /// </summary>
        DBPROPSET_CIFRMWRKCORE_EXT,
        /// <summary>
        /// An OLE-DB property 
        /// </summary>
        DBPROPSET_MSIDXS_ROWSETEXT,
        /// <summary>
        /// Query extension property set
        /// </summary>
        DBPROPSET_QUERYEXT,
    }
    /// <summary>
    /// MS-WSP MessageType
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// MessageId of CPMConnectIn
        /// </summary>
        CPMConnectIn = 0x000000C8,
        /// <summary>
        ///  MessageId of CPMConnectOut
        /// </summary>
        CPMConnectOut = 0x000000C8,
        /// <summary>
        ///  MessageId of CPMDisconnect
        /// </summary>
        CPMDisconnect = 0x000000C9,
        /// <summary>
        ///  MessageId of CPMCreateQueryIn
        /// </summary>
        CPMCreateQueryIn = 0x000000CA,
        /// <summary>
        ///  MessageId of CPMCreateQueryOut
        /// </summary>
        CPMCreateQueryOut = 0x000000CA,
        /// <summary>
        ///  MessageId of CPMFreeCursorIn
        /// </summary>
        CPMFreeCursorIn = 0x000000CB,
        /// <summary>
        ///  MessageId of CPMFreeCursorOut
        /// </summary>
        CPMFreeCursorOut = 0x000000CB,
        /// <summary>
        ///  MessageId of CPMGetRowsIn
        /// </summary>
        CPMGetRowsIn = 0x000000CC,
        /// <summary>
        ///  MessageId of CPMGetRowsOut
        /// </summary>
        CPMGetRowsOut = 0x000000CC,
        /// <summary>
        ///  MessageId of CPMRatioFinishedIn
        /// </summary>
        CPMRatioFinishedIn = 0x000000CD,
        /// <summary>
        ///  MessageId of CPMRatioFinishedOut
        /// </summary>
        CPMRatioFinishedOut = 0x000000CD,
        /// <summary>
        ///  MessageId of CPMCompareBmkIn
        /// </summary>
        CPMCompareBmkIn = 0x000000CE,
        /// <summary>
        ///  MessageId of CPMCompareBmkOut 
        /// </summary>
        CPMCompareBmkOut = 0x000000CE,
        /// <summary>
        ///  MessageId of CPMGetApproximatePositionIn
        /// </summary>
        CPMGetApproximatePositionIn = 0x000000CF,
        /// <summary>
        ///  MessageId of CPMGetApproximatePositionOut
        /// </summary>
        CPMGetApproximatePositionOut = 0x000000CF,
        /// <summary>
        ///  MessageId of CPMSetBindingsIn
        /// </summary>
        CPMSetBindingsIn = 0x000000D0,
        /// <summary>
        ///  MessageId of CPMGetNotify
        /// </summary>
        CPMGetNotify = 0x000000D1,
        /// <summary>
        ///  MessageId of CPMSendNotifyOut
        /// </summary>
        CPMSendNotifyOut = 0x000000D2,
        /// <summary>
        ///  MessageId of CPMGetQueryStatusIn
        /// </summary>
        CPMGetQueryStatusIn = 0x000000D7,
        /// <summary>
        ///  MessageId of CPMGetQueryStatusOut
        /// </summary>
        CPMGetQueryStatusOut = 0x000000D7,
        /// <summary>
        ///  MessageId of CPMCiStateInOut
        /// </summary>
        CPMCiStateInOut = 0x000000D9,
        /// <summary>
        ///  MessageId of CPMForceMergeIn
        /// </summary>
        CPMForceMergeIn = 0x000000E1,
        /// <summary>
        ///  MessageId of CPMFetchValueIn
        /// </summary>
        CPMFetchValueIn = 0x000000E4,
        /// <summary>
        ///  MessageId of CPMFetchValueOut
        /// </summary>
        CPMFetchValueOut = 0x000000E4,
        /// <summary>
        ///  MessageId of CPMUpdateDocumentsIn
        /// </summary>
        CPMUpdateDocumentsIn = 0x000000E6,
        /// <summary>
        ///  MessageId of CPMGetQueryStatusExIn
        /// </summary>
        CPMGetQueryStatusExIn = 0x000000E7,
        /// <summary>
        ///  MessageId of CPMGetQueryStatusExOut
        /// </summary>
        CPMGetQueryStatusExOut = 0x000000E7,
        /// <summary>
        ///  MessageId of CPMRestartPositionIn
        /// </summary>
        CPMRestartPositionIn = 0x000000E8,
        //Update by v-aliche for delta testing
        ///// <summary>
        /////  MessageId of CPMStopAsynchIn
        ///// </summary>
        //CPMStopAsynchIn = 0x000000E9

        /// <summary>
        /// MessageID of CPMFindIndicesIn
        /// </summary>
        CPMFindIndicesIn = 0x000000F2,

        /// <summary>
        /// MessageID of CPMFindIndicesOut
        /// </summary>
        CPMFindIndicesOut = 0x000000F2,

        /// <summary>
        /// MessageID of CPMGetRowsetNotifyIn
        /// </summary>
        CPMGetRowsetNotifyIn = 0x000000F1,

        /// <summary>
        /// MessageID of CPMGetRowsetNotifyOut
        /// </summary>
        CPMGetRowsetNotifyOut = 0x000000F1,

        /// <summary>
        /// MessageID of CPMSetScopePrioritizationIn
        /// </summary>
        CPMSetScopePrioritizationIn = 0x000000F3,

        /// <summary>
        /// MessageID of CPMSetScopePrioritizationOut
        /// </summary>
        CPMSetScopePrioritizationOut = 0x000000F3,

        /// <summary>
        /// MessageID of CPMGetScopeStatisticsOut
        /// </summary>
        CPMGetScopeStatisticsOut = 0x000000F4,

        /// <summary>
        /// MessageID of CPMGetScopeStatisticsIn
        /// </summary>
        CPMGetScopeStatisticsIn = 0x000000F4
    }
    /// <summary>
    /// Restriction Type
    /// </summary>
    public enum UlType
    {
        /// <summary>
        /// Represents a noise word in a vector query
        /// </summary>
        RTNone = 0x00000000,
        /// <summary>
        /// The node contains a CNodeRestriction structure
        /// </summary>
        RTAnd = 0x00000001,
        /// <summary>
        /// The node contains a CNodeRestriction structure
        /// </summary>
        RTOr = 0x00000002,
        /// <summary>
        /// The node contains a CRestriction structure structure
        /// </summary>
        RTNot = 0x00000003,
        /// <summary>
        /// The node contains a CContentRestriction structure
        /// </summary>
        RTContent = 0x00000004,
        /// <summary>
        /// The node contains a CPropertyRestriction structure
        /// </summary>
        RTProperty = 0x00000005,
        /// <summary>
        /// The node contains a CNodeRestriction structure
        /// </summary>
        RTProximity = 0x00000006,
        /// <summary>
        /// The node contains a CVectorRestriction structure
        /// </summary>
        RTVector = 0x00000007,
        /// <summary>
        /// The node contains a CNatLanguageRestriction structure
        /// </summary>
        RTNatLanguage = 0x00000008,
        /// <summary>
        /// The node contains a CScopeRestriction structure
        /// </summary>
        RTScope = 0x00000009,
        /// <summary>
        /// The node contains a CReuseWhere restriction structure
        /// </summary>
        RTReuseWhere = 0x00000011,
        /// <summary>
        /// The node contains a CInternalPropertyRestriction structure
        /// </summary>
        RTInternalProp = 0x00FFFFFA,
        /// <summary>
        /// The node contains a CNodeRestriction structure
        /// </summary>
        RTPhrase = 0x00FFFFFD,
        /// <summary>
        /// The node contains a CCoercionRestriction structure
        /// </summary>
        RTCoerce_Add = 0x0000000A,
        /// <summary>
        /// The node contains a CCoercionRestriction structure
        /// </summary>
        RTCoerce_Multiply = 0x0000000B,
        /// <summary>
        /// The node contains a CCoercionRestriction structure
        /// </summary>
        RTCoerce_Absolute = 0x0000000C,
        /// <summary>
        /// The node contains a CProbRestriction structure
        /// </summary>
        RTProb = 0x0000000D,
        /// <summary>
        /// The node contains a CFeedbackRestriction structure
        /// </summary>
        RTFeedback = 0x0000000E,
        /// <summary>
        /// The node contains a CRelDocRestriction structure
        /// </summary>
        RTReldoc = 0x0000000F
    }
    /// <summary>
    /// Row Seek Type
    /// </summary>
    public enum RowSeekType
    {
        /// <summary>
        /// Indicates no SeekDescription field
        /// </summary>
        eRowSeekNone = 0x00000000,
        /// <summary>
        /// SeekDescription contains a CRowSeekNext structure
        /// </summary>
        eRowSeekNext = 0x00000001,
        /// <summary>
        /// SeekDescription contains a CRowSeekAt structure
        /// </summary>
        eRowSeekAt = 0x00000002,
        /// <summary>
        /// SeekDescription contains a CRowSeekAtRatio structure
        /// </summary>
        eRowSeekAtRatio = 0x00000003,
        /// <summary>
        /// SeekDescription contains a CRowSeekByBookmark structure
        /// </summary>
        eRowSeekByBookmark = 0x00000004

    }
    /// <summary>
    /// Query Aggregate Type
    /// </summary>
    public enum AggregateType
    {
        /// <summary>
        /// No Aggregation
        /// </summary>
        DBAGGTTYPE_BYNONE = 0x00,
        /// <summary>
        /// Sum of Selected Row Set
        /// </summary>
        DBAGGTTYPE_SUM = 0x01,
        /// <summary>
        /// Maximum of Selected Row Set
        /// </summary>
        DBAGGTTYPE_MAX = 0x02,
        /// <summary>
        /// Minimum of Selected Row Set
        /// </summary>
        DBAGGTTYPE_MIN = 0x03,
        /// <summary>
        /// Average of Selected Row Set
        /// </summary>
        DBAGGTTYPE_AVG = 0x04,
        /// <summary>
        /// Count of Selected Row Set
        /// </summary>
        DBAGGTTYPE_COUNT = 0x05,
        /// <summary>
        /// Child Count of Selected Row Set
        /// </summary>
        DBAGGTTYPE_CHILDCOUNT = 0x06
    }
    /// <summary>
    /// Catalog State
    /// </summary>
    public enum EState
    {
        /// <summary>
        /// The WSS is in the process of 
        /// optimizing some of the indexes to reduce memory usage and improve
        /// query performance 
        /// </summary>
        CI_STATE_SHADOW_MERGE = 0x00000001,
        /// <summary>
        /// The WSS is in the process of
        /// full optimization for all indexes 
        /// </summary>
        CI_STATE_MASTER_MERGE = 0x00000002,
        /// <summary>
        ///The WSS is in the process of 
        ///optimizing indexes to reduce memory usage and improve query performance 
        /// </summary>
        CI_STATE_ANNEALING_MERGE = 0x00000008,
        /// <summary>
        ///The WSS is checking a directory
        /// or a set of directories to determine if any files have been added, 
        /// deleted, or updated since the last time the directory was indexed 
        /// </summary>
        CI_STATE_SCANNING = 0x00000010,
        /// <summary>
        /// Most of the virtual memory of the 
        /// server is in use
        /// </summary>
        CI_STATE_LOW_MEMORY = 0x00000080,
        /// <summary>
        /// The level of input/output (I/O) activity on the server is relatively high
        /// </summary>
        CI_STATE_HIGH_IO = 0x00000100,
        /// <summary>
        /// The process of full optimization for all indexes that was in progress 
        /// has been paused
        /// </summary>
        CI_STATE_MASTER_MERGE_PAUSED = 0x00000200,
        /// <summary>
        /// The portion of the WSS that picks up new documents to index has been paused
        /// </summary>
        CI_STATE_READ_ONLY = 0x00000400,
        /// <summary>
        /// The portion of the WSS that picks up new documents to index has been paused
        /// to conserve battery lifetime but still replies to the queries
        /// </summary>
        CI_STATE_BATTERY_POWER = 0x00000800,
        /// <summary>
        /// The portion of the WSS that picks up new documents to index has been 
        /// paused due to high activity by the user 
        /// </summary>
        CI_STATE_USER_ACTIVE = 0x00001000,
        /// <summary>
        /// The service is paused due to low disk availability
        /// </summary>
        CI_STATE_LOW_DISK = 0x00010000,
        /// <summary>
        /// The service is paused due to high CPU usage
        /// </summary>
        CI_STATE_HIGH_CPU = 0x00020000
    }
    /// <summary>
    /// UpdateDocuments Flags
    /// </summary>
    public enum Flag
    {
        /// <summary>
        /// Indicates An incremental update is to be performed
        /// </summary>
        UPD_INCREM = 0x00000000,
        /// <summary>
        /// Indicates A full update is to be performed
        /// </summary>
        UPD_FULL = 0x00000001,
        /// <summary>
        /// Indicates A new initialization is to be performed
        /// </summary>
        UPD_INIT = 0x00000002
    }
    /// <summary>
    /// Query Operator
    /// </summary>
    public enum Relop : uint
    {
        /// <summary>
        /// Indicates A less-than comparison
        /// </summary>
        PRLT = 0x00000000,
        /// <summary>
        /// Indicates A less-than or equal-to comparison
        /// </summary>
        PRLE = 0x00000001,
        /// <summary>
        /// Indicates A greater-than comparison
        /// </summary>
        PRGT = 0x00000002,
        /// <summary>
        /// Indicates A greater-than or equal-to comparison
        /// </summary>
        PRGE = 0x00000003,
        /// <summary>
        /// Indicates An equality comparison
        /// </summary>
        PREQ = 0x00000004,
        /// <summary>
        /// Indicates A not-equal comparison
        /// </summary>
        PRNE = 0x00000005,
        /// <summary>
        /// Indicates A regular expression comparison
        /// </summary>
        PRRE = 0x00000006,
        /// <summary>
        /// Indicates A bitwise AND that returns the right operand
        /// </summary>
        PRAllBits = 0x00000007,
        /// <summary>
        /// Indicates A bitwise AND that returns a nonzero value
        /// </summary>
        PRSomeBits = 0x00000008,
        /// <summary>
        /// Indicates The operation is to be performed on a column of a rowset 
        /// and is only true if the operation is true for all rows
        /// </summary>
        PRAll = 0x00000100,
        /// <summary>
        /// Indicates The operation is to be performed on a column of a rowset 
        /// and is true if the operation is true for any row
        /// </summary>
        PRAny = 0x00000200
    }
    #endregion

    #region Model Data
    /// <summary>
    /// Enum containing all the possible error codes for WSP protocol
    /// </summary>
    public enum WspErrorCode : uint
    {
        /// <summary>
        /// Specifies Successful response
        /// </summary>
        SUCCESS = 0x00000000,
        /// <summary>
        /// Specifies Invalid Parameter
        /// </summary>
        STATUS_INVALID_PARAMETER = 0xC000000D,
        /// <summary>
        /// Specifies No Catalog associated
        /// </summary>
        CI_E_NO_CATALOG = 0x8004181D,
        /// <summary>
        /// Specifies No Query Associated
        /// </summary>
        QUERY_S_NO_QUERY = 0x8004160C,
        /// <summary>
        /// Unknown Errors
        /// </summary>
        ERROR_OTHER = 0xFFFFFFFF,
        /// <summary>
        /// Generic Access Denied Error
        /// </summary>
        STATUS_ACCESS_DENIED = 0xC0000022,
        /// <summary>
        /// Failure
        /// </summary>
        E_FAIL = 0x80004005,
        /// <summary>
        /// Invalid Binding Info
        /// </summary>
        DB_E_BADBINDINFO = 0x80040E08,

        /// <summary>
        /// Access Denied
        /// </summary>
        E_ACCESSDENIED = 0x80070005,

        /// <summary>
        /// Upexpected 
        /// </summary>
        E_UNEXPECTED = 0x8000FFFF,

        /// <summary>
        /// Nointerface
        /// </summary>
        E_NOINTERFACE = 0x80004002,

        /// <summary>
        /// Specifies invalid parameter
        /// </summary>
        STATUS_INVALID_PARAMETER_MIX = 0xc0000030,

        /// <summary>
        /// Specifies out of memory
        /// </summary>
        E_OUTOFMEMORY = 0x8007000e,

        /// <summary>
        /// Another invalid parameter error code
        /// </summary>
        STATUS_INVALID_PARAMETER2= 0xD000000D,

        /// <summary>
        /// Catalog name not found
        /// </summary>
        MSS_E_CATALOGNOTFOUND = 0x80042103
    }

    /// <summary>
    /// Enum containing all the WSP messages
    /// </summary>
    public enum WspMessage
    {
        ///<summary>
        ///When there are no messages
        ///</summary>
        None = 0,
        /// <summary>
        /// CPMConnectIn message of WSP protocol
        /// </summary>
        CPMConnectIn = 1,
        /// <summary>
        /// CPMCiStateInOut message of WSP protocol
        /// </summary>
        CPMCiStateInOut = 2,
        /// <summary>
        /// CPMUpdateDocumentsIn message of WSP protocol
        /// </summary>
        CPMUpdateDocumentsIn = 3,
        /// <summary>
        /// CPMForceMergeIn message of WSP protocol
        /// </summary>
        CPMForceMergeIn = 4,
        /// <summary>
        /// CPMCreateQueryIn message of WSP protocol
        /// </summary>
        CPMCreateQueryIn = 5,
        /// <summary>
        /// CPMGetQueryStatusIn message of WSP protocol
        /// </summary>
        CPMGetQueryStatusIn = 6,
        /// <summary>
        /// CPMGetQueryStatusExIn message of WSP protocol
        /// </summary>
        CPMGetQueryStatusExIn = 7,
        /// <summary>
        /// CPMSetBindingsIn message of WSP protocol
        /// </summary>
        CPMSetBindingsIn = 8,
        /// <summary>
        /// CPMGetRowsIn message of WSP protocol
        /// </summary>
        CPMGetRowsIn = 9,
        /// <summary>
        /// CPMRatioFinishedIn message of WSP protocol
        /// </summary>
        CPMRatioFinishedIn = 10,
        /// <summary>
        /// CPMFetchValueIn message of WSP protocol
        /// </summary>
        CPMFetchValueIn = 11,
        /// <summary>
        /// CPMGetNotify message of WSP protocol
        /// </summary>
        CPMGetNotify = 12,
        /// <summary>
        /// CPMGetApproximatePositionIn message of WSP protocol
        /// </summary>
        CPMGetApproximatePositionIn = 13,
        /// <summary>
        /// CPMCompareBmkIn message of WSP protocol
        /// </summary>
        CPMCompareBmkIn = 14,
        /// <summary>
        /// CPMRestartPositionIn message of WSP protocol
        /// </summary>
        CPMRestartPositionIn = 15,
        ///// <summary>
        ///// CPMStopAsyncIn message of WSP protocol
        ///// </summary>
        //CPMStopAsyncIn = 16,
        /// <summary>
        /// CPMFreeCursorIn message of WSP protocol
        /// </summary>
        CPMFreeCursorIn = 17,
        /// <summary>
        /// CPMDisconnect message of WSP protocol
        /// </summary>
        CPMDisconnect = 18,
        /// <summary>
        /// ErrorState value 
        /// </summary>
        ErrorState = 19,
        /// <summary>
        /// CPMConnectOut message of WSP protocol
        /// </summary>
        CPMConnectOut = 20,

        /// <summary>
        /// CPMFindIndicesIn message of WSP protocol
        /// </summary>
        CPMFindIndicesIn = 21,

        /// <summary>
        /// CPMGetRowsetNotifyIn message of WSP protocol
        /// </summary>
        CPMGetRowsetNotifyIn = 22,

        /// <summary>
        /// CPMSetScopePrioritizationIn message of WSP protocol
        /// </summary>
        CPMSetScopePrioritizationIn = 23,

        /// <summary>
        /// CPMGetScopeStatisticsIn message of WSP protocol
        /// </summary>
        CPMGetScopeStatisticsIn = 24
    }

    #endregion

    /// <summary>
    /// Constant class provides values of commonly used type and fields
    /// </summary>
    public class Constant
    {
        /// <summary>
        /// the value of eventFrequency
        /// </summary>
        public const uint eventFrequency = 0;
        /// <summary>
        /// Size of unsigned integer
        /// </summary>
        public const int SIZE_OF_UINT = 4;
        /// <summary>
        /// Size of a byte
        /// </summary>
        public const int SIZE_OF_BYTE = 1;
        /// <summary>
        /// Size of a GUID
        /// </summary>
        public const int SIZE_OF_GUID = 16;
        /// <summary>
        /// Size of unsigned short
        /// </summary>
        public const int SIZE_OF_USHORT = 2;
        /// <summary>
        /// Offset used by 32 bit machine
        /// </summary>
        public const uint OFFSET_32 = 32;
        /// <summary>
        /// Offset used by 64 bit machine
        /// </summary>
        public const uint OFFSET_64 = 64;

        /// <summary>
        /// Size of WSP Message Header
        /// </summary>
        public const int SIZE_OF_HEADER = 16;

        /// <summary>
        /// property of DBPROP_ENABLEROWSETEVENTS
        /// </summary>
        public static bool DBPROP_ENABLEROWSETEVENTS = true;

        /// <summary>
        /// Returns the value of Combination of all possible values of Message field
        /// </summary>
        /// <returns>LOGICAL OR of all message fields</returns>
        public static uint GetAllPossibleMessageTypeValue()
        {
            /* Gives the maximum possible value of all 
             * the messageType to check whether a number
             * X is a valid messageType 
             * it should be 'ANDed'(LOGICAL AND Operation) with this value 
             * the result must be EQUAL to the X */
            //Updated by:v-zhil
            //Delta testing
            return 0x000000C8
                | 0x000000C9
                | 0x000000CA
                | 0x000000CB
                | 0x000000CC
                | 0x000000CD
                | 0x000000CE
                | 0x000000CF
                | 0x000000D0
                | 0x000000D1
                | 0x000000D2
                | 0x000000D7
                | 0x000000D9
                | 0x000000E1
                | 0x000000E4
                | 0x000000E6
                | 0x000000E7
                | 0x000000E8
                | 0x000000E9
                | 0x000000EC
                | 0x000000F1
                | 0x000000F2
                | 0x000000F3
                | 0x000000F4;

        }
    }

}

