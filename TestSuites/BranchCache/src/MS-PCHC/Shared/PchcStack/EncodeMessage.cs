// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrc;

    /// <summary>
    /// Marshal the message into byte array.
    /// </summary>
    internal static class EncodeMessage
    {
        #region public method

        /// <summary>
        /// Transform the SEGMENT_INFO_MESSAGE to a byte array
        /// </summary>
        /// <param name="segmentInfoMsg">The SEGMENT_INFO_MESSAGE message</param>
        /// <returns>The result array</returns>
        public static byte[] EncodeSegmentInfoMessage(SEGMENT_INFO_MESSAGE segmentInfoMsg)
        {
            List<byte> list = new List<byte>();

            // Encode MESSAGE_HEADER
            list.Add(segmentInfoMsg.MsgHeader.MinorVersion);
            list.Add(segmentInfoMsg.MsgHeader.MajorVersion);
            list.AddRange(GetBytesFromUshort((ushort)segmentInfoMsg.MsgHeader.MsgType, false));
            if (segmentInfoMsg.MsgHeader.Padding == null)
            {
                segmentInfoMsg.MsgHeader.Padding = new byte[4];
            }

            list.AddRange(segmentInfoMsg.MsgHeader.Padding);

            // Encode CONNECTION_INFORMATION
            list.AddRange(GetBytesFromUint16(segmentInfoMsg.ConnectionInfo.Port, false));
            if (segmentInfoMsg.ConnectionInfo.Padding == null)
            {
                segmentInfoMsg.ConnectionInfo.Padding = new byte[6];
            }

            list.AddRange(segmentInfoMsg.ConnectionInfo.Padding);

            // Encode CONTENT_TAG
            list.AddRange(segmentInfoMsg.ContentTag);

            // Encode segment infomation.
            list.AddRange(GetBytesFromUshort(segmentInfoMsg.SegmentInfo.Version, true));
            list.AddRange(GetBytesFromUint((uint)segmentInfoMsg.SegmentInfo.dwHashAlgo, true));
            list.AddRange(GetBytesFromUint(segmentInfoMsg.SegmentInfo.dwOffsetInFirstSegment, true));
            list.AddRange(GetBytesFromUint(segmentInfoMsg.SegmentInfo.dwOffsetInFirstSegment, true));
            list.AddRange(GetBytesFromUint(segmentInfoMsg.SegmentInfo.cSegments, true));

            for (int i = 0; i < segmentInfoMsg.SegmentInfo.segments.Length; i++)
            {
                list.AddRange(GetBytesFromUlong(segmentInfoMsg.SegmentInfo.segments[i].ullOffsetInContent, true));
                list.AddRange(GetBytesFromUint(segmentInfoMsg.SegmentInfo.segments[i].cbSegment, true));
                list.AddRange(GetBytesFromUint(segmentInfoMsg.SegmentInfo.segments[i].cbBlockSize, true));
                list.AddRange(segmentInfoMsg.SegmentInfo.segments[i].SegmentHashOfData);
                list.AddRange(segmentInfoMsg.SegmentInfo.segments[i].SegmentSecret);
            }

            for (int j = 0; j < segmentInfoMsg.SegmentInfo.blocks.Length; j++)
            {
                list.AddRange(GetBytesFromUint(segmentInfoMsg.SegmentInfo.blocks[j].cBlocks, true));
                list.AddRange(segmentInfoMsg.SegmentInfo.blocks[j].BlockHashes);
            }

            return list.ToArray();
        }

        /// <summary>
        /// Transform the INITIAL_OFFER_MESSAGE to a byte array
        /// </summary>
        /// <param name="initOfferMsg">The INITIAL_OFFER_MESSAGE message</param>
        /// <returns>The result array</returns>
        public static byte[] EncodeInitialOfferMessage(INITIAL_OFFER_MESSAGE initOfferMsg)
        {
            List<byte> list = new List<byte>();
            list.Add(initOfferMsg.MsgHeader.MinorVersion);
            list.Add(initOfferMsg.MsgHeader.MajorVersion);
            list.AddRange(GetBytesFromUshort((ushort)initOfferMsg.MsgHeader.MsgType, false));
            if (initOfferMsg.MsgHeader.Padding != null)
            {
                list.AddRange(initOfferMsg.MsgHeader.Padding);
            }

            list.AddRange(GetBytesFromUint16(initOfferMsg.ConnectionInfo.Port, false));
            if (initOfferMsg.ConnectionInfo.Padding != null)
            {
                list.AddRange(initOfferMsg.ConnectionInfo.Padding);
            }

            list.AddRange(initOfferMsg.Hash);

            return list.ToArray();
        }

        /// <summary>
        /// Transform the RESPONSE_MESSAGE to a byte array
        /// </summary>
        /// <param name="responseMessage">The RESPONSE_MESSAGE message</param>
        /// <returns>The result array</returns>
        public static byte[] EncodeResponseMessage(RESPONSE_MESSAGE responseMessage)
        {
            List<byte> list = new List<byte>();
            list.AddRange(GetBytesFromUint32(responseMessage.TransportHeader.Size, false));
            list.Add((byte)responseMessage.ResponseCode);
            return list.ToArray();
        }

        #endregion

        #region private method
        /// <summary>
        /// Convert the ulong type value to byte array.
        /// </summary>
        /// <param name="value">An ulong value</param>
        /// <param name="isBigEndian">order of bytes. If it's big endian, set it to true, else set it to false.</param>
        /// <returns>The result byte array</returns>
        private static byte[] GetBytesFromUlong(ulong value, bool isBigEndian)
        {
            byte[] resultData = BitConverter.GetBytes(value);
            ReverseByFlag(resultData, isBigEndian);
            return resultData;
        }

        /// <summary>
        /// Convert the uint16 type value to byte array.
        /// </summary>
        /// <param name="value">An uint16 value</param>
        /// <param name="isBigEndian">order of bytes. If it's big endian, set it to true, else set it to false.</param>
        /// <returns>The result byte array</returns>
        private static byte[] GetBytesFromUint16(ushort value, bool isBigEndian)
        {
            byte[] resultData = BitConverter.GetBytes(value);
            ReverseByFlag(resultData, isBigEndian);
            return resultData;
        }

        /// <summary>
        /// Convert the uint16 type value to byte array.
        /// </summary>
        /// <param name="value">An uint16 value</param>
        /// <param name="isBigEndian">order of bytes. If it's big endian, set it to true, else set it to false.</param>
        /// <returns>The result byte array</returns>
        private static byte[] GetBytesFromUshort(ushort value, bool isBigEndian)
        {
            byte[] resultData = BitConverter.GetBytes(value);
            ReverseByFlag(resultData, isBigEndian);
            return resultData;
        }

        /// <summary>
        /// Convert the uint type value to byte array.
        /// </summary>
        /// <param name="value">An uint value</param>
        /// <param name="isBigEndian">order of bytes. If it's big endian, set it to true, else set it to false.</param>
        /// <returns>The result byte array</returns>
        private static byte[] GetBytesFromUint32(uint value, bool isBigEndian)
        {
            byte[] resultData = BitConverter.GetBytes(value);
            ReverseByFlag(resultData, isBigEndian);
            return resultData;
        }

        /// <summary>
        /// Convert the uint type value to byte array.
        /// </summary>
        /// <param name="value">An uint value</param>
        /// <param name="isBigEndian">order of bytes. If it's big endian, set it to true, else set it to false.</param>
        /// <returns>The result byte array</returns>
        private static byte[] GetBytesFromUint(uint value, bool isBigEndian)
        {
            byte[] resultData = BitConverter.GetBytes(value);
            ReverseByFlag(resultData, isBigEndian);
            return resultData;
        }

        /// <summary>
        /// reverse the bytes if it's not big endian.
        /// </summary>
        /// <param name="data">the bytes will be re-order</param>
        /// <param name="isBigEndian">order of bytes. If it's big endian, set it to true, else set it to false.</param>
        private static void ReverseByFlag(byte[] data, bool isBigEndian)
        {
            if (!isBigEndian)
            {
                Array.Reverse(data);
            }
        }

        /// <summary>
        /// Reverse the content of an array.
        /// </summary>
        /// <param name="data">The array to be reversed.</param>
        /// <returns>The reversed array.</returns>
        private static byte[] Reverse(byte[] data)
        {
            byte[] reverse = new byte[data.Length];
            Array.Copy(data, reverse, data.Length);
            Array.Reverse(reverse);
            return reverse;
        }
        #endregion
    }
}
