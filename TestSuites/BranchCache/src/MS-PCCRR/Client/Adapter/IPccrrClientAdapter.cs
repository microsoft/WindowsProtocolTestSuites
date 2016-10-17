// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pccrr
{
    using Microsoft.Protocols.TestTools;

    /// <summary>
    /// Receiving time out handler.
    /// </summary>
    public delegate void ReceivingTimeOutHandler();

    /// <summary>
    /// Receive message MSG_NEGO_REQ handler.
    /// </summary>
    public delegate void ReceiveMsgNegoReqHandler();

    /// <summary>
    /// Receive message MSG_GETBLKLIST handler.
    /// </summary>
    /// <param name="blockRanges">The block ranges.</param>
    public delegate void ReceiveMsgGetBlkListHandler(_BLOCKRANGE[] blockRanges);

    /// <summary>
    /// Receive message MSG_GETBLK handler.
    /// </summary>
    /// <param name="index">Block index.</param>
    public delegate void ReceiveMsgGetBlkHandler(uint index);

    /// <summary>
    /// This class is used to provide client adapter interface.
    /// </summary>
    public interface IPccrrClientAdapter : IAdapter
    {
        /// <summary>
        /// Receiving time out.
        /// </summary>
        event ReceivingTimeOutHandler ReceivingTimeOut;

        /// <summary>
        /// Receive message MSG_NEGO_REQ.
        /// </summary>
        event ReceiveMsgNegoReqHandler ReceiveMsgNegoReq;

        /// <summary>
        /// Receive message MSG_GETBLKLIST.
        /// </summary>
        event ReceiveMsgGetBlkListHandler ReceiveMsgGetBlkList;

        /// <summary>
        /// Receive message MSG_GETBLK.
        /// </summary>
        event ReceiveMsgGetBlkHandler ReceiveMsgGetBlk;

        /// <summary>
        /// Send message MSG_NEGO_RESP.
        /// </summary>
        /// <param name="isSupportVersion">If it is true, it is support version, if it is false, it is not support version.</param>
        /// <param name="isWellFormed">If it is true, it is well formed, if it is false, it is not well formed.</param>
        void SendMsgNegoResp(bool isSupportVersion, bool isWellFormed);

        /// <summary>
        /// Send message MSG_BLKLIST.
        /// </summary>
        /// <param name="isTimerExpire">The timer for SendMsgBlkList from client will expire or not.</param>
        /// <param name="isSameSegment">The SegmentID is same as the request from client.</param>
        /// <param name="dwHashAlgoValues">The hash algorithm to use.</param>
        /// <param name="isOverlap">The block ranges overlap with the ranges specified in any request 
        /// with a matching Segment ID in the outstanding request list.</param>
        void SendMsgBlkList(bool isTimerExpire, bool isSameSegment, DWHashAlgValues dwHashAlgoValues, bool isOverlap);

        /// <summary>
        /// Send message MSG_BLK.
        /// </summary>
        /// <param name="isTimerExpire">The timer for SendMsgBlk from client will expire or not.</param>
        /// <param name="isSameSegment">The message is for the segment that client request.</param>
        /// <param name="dwHashAlgoValues">The hash algorithm to use.</param>
        /// <param name="index">Block index.</param>
        /// <param name="isLastBLK">The block is the last available.</param>
        void SendMsgBlk(bool isTimerExpire, bool isSameSegment, DWHashAlgValues dwHashAlgoValues, uint index, bool isLastBLK);
    }
}
