// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc
{
    using System;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrc;

    /// <summary>
    /// Unaarshal the message from byte array.
    /// </summary>
    internal static class DecodeMessage
    {
        #region public methods

        /// <summary>
        /// Get bytes from bytes.
        /// </summary>
        /// <param name="buffer">the buffer stores the value.</param>
        /// <param name="index">the index of start to parse</param>
        /// <param name="count">count of bytes</param>
        /// <returns>Parsed UInt16 value</returns>
        public static byte[] GetBytes(byte[] buffer, ref int index, int count)
        {
            byte[] byteReturn = null;

            if (count > 0)
            {
                byteReturn = new byte[count];

                Array.Copy(buffer, index, byteReturn, 0, byteReturn.Length);
                index += byteReturn.Length;
            }

            return byteReturn;
        }

        /// <summary>
        /// Unmarshal response message.
        /// </summary>
        /// <param name="byteArr">The payload.</param>
        /// <returns>The unmarshal response message.</returns>
        public static RESPONSE_MESSAGE DecodeResponseMessage(byte[] byteArr)
        {
            RESPONSE_MESSAGE responseMessage;
            TRANSPORT_HEADER header;
            RESPONSE_CODE responseCode;

            if (byteArr == null)
            {
                throw new ArgumentNullException("byteArr");
            }

            int index = 0;
            byte[] informationData = GetBytes(byteArr, ref index, byteArr.Length - index);
            int tempIndex = 0;

            header.Size = GetUInt32(informationData, ref tempIndex, false);

            if ((informationData[tempIndex] ^ 0x00) == 0)
            {
                responseCode = RESPONSE_CODE.OK;
            }
            else if ((informationData[tempIndex] ^ 0x01) == 0)
            {
                responseCode = RESPONSE_CODE.INTERESTED;
            }
            else
            {
                responseCode = (RESPONSE_CODE)informationData[tempIndex];
            }

            responseMessage.TransportHeader = header;
            responseMessage.ResponseCode = responseCode;

            return responseMessage;
        }

        /// <summary>
        /// Unmarshal the incoming pchc message to get message type.
        /// </summary>
        /// <param name="byteArr">The payload.</param>
        /// <returns>The message type.</returns>
        public static PCHC_MESSAGE_TYPE GetMessageType(byte[] byteArr)
        {
            int index = 0;
            byte[] informationData = GetBytes(byteArr, ref index, byteArr.Length - index);
            int tempIndex = 0;
            PCHC_MESSAGE_TYPE msgType;

            tempIndex = tempIndex + 2;
            msgType = (PCHC_MESSAGE_TYPE)GetUInt16(informationData, ref tempIndex, false);

            return msgType;
        }

        /// <summary>
        /// Unmarshal the initial offer message.
        /// </summary>
        /// <param name="byteArr">The payload.</param>
        /// <returns>The unmarshaled initial offer message.</returns>
        public static INITIAL_OFFER_MESSAGE DecodeInitialOfferMessage(byte[] byteArr)
        {
            int index = 0;
            byte[] informationData = GetBytes(byteArr, ref index, byteArr.Length - index);
            int tempIndex = 0;

            INITIAL_OFFER_MESSAGE initialOfferMessage = default(INITIAL_OFFER_MESSAGE);

            MESSAGE_HEADER msgHeader;
            msgHeader.MinorVersion = informationData[tempIndex++];
            msgHeader.MajorVersion = informationData[tempIndex++];
            msgHeader.MsgType = (PCHC_MESSAGE_TYPE)GetUInt16(informationData, ref tempIndex, false);
            msgHeader.Padding = GetBytes(informationData, ref tempIndex, 4);
            initialOfferMessage.MsgHeader = msgHeader;

            CONNECTION_INFORMATION connectionInfo;
            connectionInfo.Port = GetUInt16(informationData, ref tempIndex, false);
            connectionInfo.Padding = GetBytes(informationData, ref tempIndex, 6);
            initialOfferMessage.ConnectionInfo = connectionInfo;

            initialOfferMessage.Hash = GetBytes(informationData, ref tempIndex, informationData.Length - tempIndex);

            return initialOfferMessage;
        }

        /// <summary>
        /// Unmarshal the segment info message.
        /// </summary>
        /// <param name="byteArr">The payload.</param>
        /// <returns>The unmarshaled segment info message.</returns>
        public static SEGMENT_INFO_MESSAGE DecodeSegmentInfoMessage(byte[] byteArr)
        {
            int index = 0;
            byte[] informationData = GetBytes(byteArr, ref index, byteArr.Length - index);
            int tempIndex = 0;
            SEGMENT_INFO_MESSAGE segmentInfoMessage;

            MESSAGE_HEADER msgHeader;
            msgHeader.MinorVersion = informationData[tempIndex++];
            msgHeader.MajorVersion = informationData[tempIndex++];
            msgHeader.MsgType = (PCHC_MESSAGE_TYPE)GetUInt16(informationData, ref tempIndex, false);
            msgHeader.Padding = GetBytes(informationData, ref tempIndex, 4);
            segmentInfoMessage.MsgHeader = msgHeader;

            CONNECTION_INFORMATION connectionInfo;
            connectionInfo.Port = GetUInt16(informationData, ref tempIndex, false);
            connectionInfo.Padding = GetBytes(informationData, ref tempIndex, 6);
            segmentInfoMessage.ConnectionInfo = connectionInfo;

            segmentInfoMessage.ContentTag = GetBytes(informationData, ref tempIndex, 16);
            segmentInfoMessage.SegmentInfo = PccrcUtility.ParseContentInformation(informationData, ref tempIndex);

            return segmentInfoMessage;
        }

        #endregion

        #region private methods

        /// <summary>
        /// Parse ushort from bytes
        /// </summary>
        /// <param name="buffer">The buffer stores the value.</param>
        /// <param name="index">The index of start to parse</param>
        /// <param name="isBigEndian">Order of bytes. If it's big endian, set it to true, else set it to false.</param>
        /// <returns>Parsed ushort value</returns>
        private static ushort GetUInt16(byte[] buffer, ref int index, bool isBigEndian)
        {
            ushort ushortReturn = 0;
            byte[] byteTemp = BitConverter.GetBytes(ushortReturn);

            Array.Copy(buffer, index, byteTemp, 0, byteTemp.Length);
            ReverseByFlag(isBigEndian, byteTemp);
            ushortReturn = BitConverter.ToUInt16(byteTemp, 0);
            index += byteTemp.Length;

            return ushortReturn;
        }

        /// <summary>
        /// Parse uint from bytes
        /// </summary>
        /// <param name="buffer">The buffer stores the value.</param>
        /// <param name="index">The index of start to parse</param>
        /// <param name="isBigEndian">Order of bytes. If it's big endian, set it to true, else set it to false.</param>
        /// <returns>Parsed uint value</returns>
        private static uint GetUInt32(byte[] buffer, ref int index, bool isBigEndian)
        {
            uint uintReturn = 0;
            byte[] byteTemp = BitConverter.GetBytes(uintReturn);

            Array.Copy(buffer, index, byteTemp, 0, byteTemp.Length);
            ReverseByFlag(isBigEndian, byteTemp);
            uintReturn = BitConverter.ToUInt32(byteTemp, 0);
            index += byteTemp.Length;

            return uintReturn;
        }

        /// <summary>
        /// reverse the bytes if it's not big endian.
        /// </summary>
        /// <param name="isBigEndian">order of bytes. If it's big endian, set it to true, else set it to false.</param>
        /// <param name="byteTemp">the bytes will be re-order</param>
        private static void ReverseByFlag(bool isBigEndian, byte[] byteTemp)
        {
            if (!isBigEndian)
            {
                Array.Reverse(byteTemp);
            }
        }

        #endregion
    }
}
