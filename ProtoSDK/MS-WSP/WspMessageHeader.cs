// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    public enum WspMessageHeader_msg_Values : uint
    {
        /// <summary>
        /// CPMConnectIn
        /// </summary>
        CPMConnectIn = 0x000000C8,

        /// <summary>
        /// CPMConnectOut
        /// </summary>
        CPMConnectOut = 0x000000C8,

        /// <summary>
        /// CPMDisconnect
        /// </summary>
        CPMDisconnect = 0x000000C9,

        /// <summary>
        /// CPMCreateQueryIn
        /// </summary>
        CPMCreateQueryIn = 0x000000CA,

        /// <summary>
        /// CPMCreateQueryOut
        /// </summary>
        CPMCreateQueryOut = 0x000000CA,

        /// <summary>
        /// CPMFreeCursorIn
        /// </summary>
        CPMFreeCursorIn = 0x000000CB,

        /// <summary>
        /// CPMFreeCursorOut
        /// </summary>
        CPMFreeCursorOut = 0x000000CB,

        /// <summary>
        /// CPMGetRowsIn
        /// </summary>
        CPMGetRowsIn = 0x000000CC,

        /// <summary>
        /// CPMGetRowsOut
        /// </summary>
        CPMGetRowsOut = 0x000000CC,

        /// <summary>
        /// CPMRatioFinishedIn
        /// </summary>
        CPMRatioFinishedIn = 0x000000CD,

        /// <summary>
        /// CPMRatioFinishedOut
        /// </summary>
        CPMRatioFinishedOut = 0x000000CD,

        /// <summary>
        /// CPMCompareBmkIn
        /// </summary>
        CPMCompareBmkIn = 0x000000CE,

        /// <summary>
        /// CPMCompareBmkOut
        /// </summary>
        CPMCompareBmkOut = 0x000000CE,

        /// <summary>
        /// CPMGetApproximatePositionIn
        /// </summary>
        CPMGetApproximatePositionIn = 0x000000CF,

        /// <summary>
        /// CPMGetApproximatePositionOut
        /// </summary>
        CPMGetApproximatePositionOut = 0x000000CF,

        /// <summary>
        /// CPMSetBindingsIn
        /// </summary>
        CPMSetBindingsIn = 0x000000D0,

        /// <summary>
        /// CPMGetNotify
        /// </summary>
        CPMGetNotify = 0x000000D1,

        /// <summary>
        /// CPMSendNotifyOut
        /// </summary>
        CPMSendNotifyOut = 0x000000D2,

        /// <summary>
        /// CPMGetQueryStatusIn
        /// </summary>
        CPMGetQueryStatusIn = 0x000000D7,

        /// <summary>
        /// CPMGetQueryStatusOut
        /// </summary>
        CPMGetQueryStatusOut = 0x000000D7,

        /// <summary>
        /// CPMCiStateInOut
        /// </summary>
        CPMCiStateInOut = 0x000000D9,

        /// <summary>
        /// CPMFetchValueIn
        /// </summary>
        CPMFetchValueIn = 0x000000E4,

        /// <summary>
        /// CPMFetchValueOut
        /// </summary>
        CPMFetchValueOut = 0x000000E4,

        /// <summary>
        /// CPMGetQueryStatusExIn
        /// </summary>
        CPMGetQueryStatusExIn = 0x000000E7,

        /// <summary>
        /// CPMGetQueryStatusExOut
        /// </summary>
        CPMGetQueryStatusExOut = 0x000000E7,

        /// <summary>
        /// CPMRestartPositionIn
        /// </summary>
        CPMRestartPositionIn = 0x000000E8,

        /// <summary>
        /// CPMSetCatStateIn (not supported)
        /// </summary>
        CPMSetCatStateIn = 0x000000EC,

        /// <summary>
        /// CPMGetRowsetNotifyIn
        /// </summary>
        CPMGetRowsetNotifyIn = 0x000000F1,

        /// <summary>
        /// CPMGetRowsetNotifyOut
        /// </summary>
        CPMGetRowsetNotifyOut = 0x000000F1,

        /// <summary>
        /// CPMFindIndicesIn
        /// </summary>
        CPMFindIndicesIn = 0x000000F2,

        /// <summary>
        /// CPMFindIndicesOut
        /// </summary>
        CPMFindIndicesOut = 0x000000F2,

        /// <summary>
        /// CPMSetScopePrioritizationIn
        /// </summary>
        CPMSetScopePrioritizationIn = 0x000000F3,

        /// <summary>
        /// CPMSetScopePrioritizationOut
        /// </summary>
        CPMSetScopePrioritizationOut = 0x000000F3,

        /// <summary>
        /// CPMGetScopeStatisticsIn
        /// </summary>
        CPMGetScopeStatisticsIn = 0x000000F4,

        /// <summary>
        /// CPMGetScopeStatisticsOut
        /// </summary>
        CPMGetScopeStatisticsOut = 0x000000F4,

        /// <summary>
        /// Invalid
        /// </summary>
        Invalid = 0xFFFFFFFF
    }

    /// <summary>
    /// All Windows Search Protocol messages have a 16-byte header.
    /// </summary>
    public struct WspMessageHeader : IWspStructure
    {
        /// <summary>
        /// A 32-bit integer that identifies the type of message following the header.
        /// </summary>
        public WspMessageHeader_msg_Values _msg;

        /// <summary>
        /// An HRESULT, indicating the status of the requested operation.
        /// The client MUST initialize this value to 0x00000000.
        /// The server then changes it as the status of the requested operation changes.
        /// </summary>
        public uint _status;

        /// <summary>
        /// The _ulChecksum MUST be calculated as specified in section 3.2.4 for the following messages:
        /// CPMConnectIn
        /// CPMCreateQueryIn
        /// CPMSetBindingsIn
        /// CPMGetRowsIn
        /// CPMFetchValueIn
        /// Note For all other messages, _ulChecksum MUST be set to 0x00000000. A client MUST ignore the _ulChecksum field.
        /// </summary>
        public uint _ulChecksum;

        /// <summary>
        /// MUST be ignored by the receiver.
        /// Note This field MUST be set to 0x00000000 except for the CPMGetRowsIn message, where it MUST hold the high 32-bits part of a 64-bit offset if 64-bit offsets are being used(see section 2.2.3.12 for details).
        /// </summary>
        public uint _ulReserved2;

        public void FromBytes(WspBuffer buffer)
        {
            this = buffer.ToStruct<WspMessageHeader>();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(this);
        }
    }
}
