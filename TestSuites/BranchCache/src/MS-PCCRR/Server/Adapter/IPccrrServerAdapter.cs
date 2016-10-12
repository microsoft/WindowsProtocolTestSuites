// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pccrr
{
    using Microsoft.Protocols.TestTools;

    /// <summary>
    /// Receive message MSG_NEGO_RESP handler
    /// </summary>
    public delegate void ReceiveMsgNegoRespHandler();

    /// <summary>
    /// Receive message MSG_BLKLIST handler
    /// </summary>
    /// <param name="blockRangeCount">the element number of BlockRange</param>
    /// <param name="blockRanges">block ranges client received</param>
    /// <param name="nextBlockIndex">after the message sent, first Block Index  which is available in server</param>
    public delegate void ReceiveMsgBlkListHandler(uint blockRangeCount, BLOCKRANGE[] blockRanges, uint nextBlockIndex);

    /// <summary>
    /// Receive message MSG_BLK handler
    /// </summary>
    /// <param name="blockIndex">received block index</param>
    /// <param name="nextBlockIndex">The index of the first block after the received block which is available in server</param>
    /// <param name="isSizeOfBlockZero">The value of the SizeOfBlock field in this message is zero or not.</param>
    /// <param name="isBlockEmpty">The block in this message is empty or not.</param>
    /// <param name="cryptoAlgoId">Encryption algorithm</param>
    public delegate void ReceiveMsgBlkHandler(uint blockIndex, uint nextBlockIndex, bool isSizeOfBlockZero, bool isBlockEmpty, CryptoAlgoIdValues cryptoAlgoId);

    /// <summary>
    /// this class is used to provide server adapter interface to model
    /// </summary>
    public interface IPccrrServerAdapter : IAdapter
    {
        /// <summary>
        /// Receive message MSG_NEGO_RESP
        /// </summary>
        event ReceiveMsgNegoRespHandler ReceiveMsgNegoResp;

        /// <summary>
        /// Receive message MSG_BLKLIST
        /// </summary>
        event ReceiveMsgBlkListHandler ReceiveMsgBlkList;

        /// <summary>
        /// Receive message MSG_BLK
        /// </summary>
        event ReceiveMsgBlkHandler ReceiveMsgBlk;

        /// <summary>
        /// Send message MSG_NEGO_REQ
        /// </summary>
        void SendMsgNegoReq();

        /// <summary>
        /// Send message MSG_GETBLKLIST
        /// </summary>
        /// <param name="sid">The segment id.</param>
        /// <param name="blockRanges">Block ranges client wants to get.</param>
        /// <param name="isVersionSupported">The version in message is supported by server or not.</param>
        void SendMsgGetBlkList(byte[] sid, BLOCKRANGE[] blockRanges, bool isVersionSupported);

        /// <summary>
        /// Send message MSG_GETBLKS
        /// </summary>
        /// <param name="sid">The segment id.</param>
        /// <param name="blockRanges">Block ranges client wants to get.</param>
        /// <param name="isVersionSupported">The version in this message is supported in server.</param>
        void SendMsgGetBlks(byte[] sid, BLOCKRANGE[] blockRanges, bool isVersionSupported);
    }
}
