// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
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
    /// Property class
    /// </summary>
    public class Properties
    {
        // OS SKU version
        protected static SkuOsVersion skuOsVersion;

        /// <summary>
        /// OS SKU version
        /// </summary>
        public static SkuOsVersion SkuOsVersion
        {
            set
            {
                skuOsVersion = value;
            }

            get
            {
                return skuOsVersion;
            }
        }
    }


    /// <summary>
    /// Priority of CPMSetScopePrioritizationIn
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
    /// EventType in CPMGetRowsetNotifyOut
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
    public enum RowsetEventType : int
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
        ///  MessageId of CPMFetchValueIn
        /// </summary>
        CPMFetchValueIn = 0x000000E4,

        /// <summary>
        ///  MessageId of CPMFetchValueOut
        /// </summary>
        CPMFetchValueOut = 0x000000E4,

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
    /// Row Seek Type
    /// Used in CPMGetRowsOut
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
        /// Specifies Invalid Parameter
        /// </summary>
        ERROR_INVALID_PARAMETER = 0x80070057,

        /// <summary>
        /// Specifies invalid parameter
        /// </summary>
        STATUS_INVALID_PARAMETER_MIX = 0xc0000030,

        /// <summary>
        /// Specifies out of memory
        /// </summary>
        E_OUTOFMEMORY = 0x8007000e,

        /// <summary>
        /// Catalog name not found
        /// </summary>
        MSS_E_CATALOGNOTFOUND = 0x80042103,

        /// <summary>
        /// Start or end of rowset or chapter was reached
        /// </summary>
        DB_S_ENDOFROWSET = 0x00040ec6,

        /// <summary>
        /// Operation aborted
        /// </summary>
        E_ABORT = 0x80004004,

        /// <summary>
        /// Column ID is invalid.
        /// </summary>
        DB_E_BADCOLUMNID = 0x80040E11,

        /// <summary>
        /// The scopes specified for the query were incorrectly formatted.
        /// </summary>
        QRY_E_INVALIDSCOPES = 0x80040718,

        /// <summary>
        /// Input dialect was ignored and command was processed using default dialect.
        /// </summary>
        DB_S_DIALECTIGNORED = 0x00040ECD,

        /// <summary>
        /// The data area passed to a system call is too small.
        /// </summary>
        STATUS_BUFFER_TOO_SMALL = 0xC0000023,

        /// <summary>
        /// Numerator was greater than denominator. Values must express ratio between zero and 1.
        /// </summary>
        DB_E_BADRATIO = 0x80040E12,
    }

    /// <summary>
    /// A 32-bit unsigned integer indicating the order in which to fetch the rows that MUST be set to one of the following values.
    /// Used in CPMGetRowsIn
    /// </summary>
    public enum FetchType : uint
    {
        ForwardOrder = 0x00000000,
        ReverseOrder = 0x00000001
    }

    #endregion

    /// <summary>
    /// Constants class provides values of commonly used type and fields
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// The value of eventFrequency
        /// </summary>
        public const uint EventFrequency = 0;

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
        /// Property of DBPROP_ENABLEROWSETEVENTS
        /// </summary>
        public static bool DBPROP_ENABLEROWSETEVENTS = true;

        /// <summary>
        /// The value of combination of all possible values of message field.
        /// </summary>
        /* Gives the maximum possible value of all 
         * the messageType to check whether a number
         * X is a valid messageType 
         * it should be 'ANDed'(LOGICAL AND Operation) with this value 
         * the result must be EQUAL to the X */
        // Updated by:v-zhil
        // Delta testing
        public const uint AllPossibleMessageTypeValues = 0x000000C8
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
            | 0x000000E7
            | 0x000000E8
            | 0x000000EC
            | 0x000000F1
            | 0x000000F2
            | 0x000000F3
            | 0x000000F4;
    }
}

