// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpemt
{
    /// <summary>
    /// This class provide static method for Decode/Encode of some auto-detect structure
    /// </summary>
    public class RdpemtUtility
    {
        /// <summary>
        /// Parse RDP_BW_START structure from RDPEMT subheader
        /// </summary>
        /// <param name="data">Data of subheader, not include first two bytes of Parse RDP_BW_START</param>
        /// <returns></returns>
        public static RDP_BW_START ParseRdpBWStart(byte[] data)
        {
            RDP_BW_START bwStart = new RDP_BW_START();
            bwStart.sequenceNumber = ParseUInt16(data, 0);
            bwStart.requestType = (AUTO_DETECT_REQUEST_TYPE)ParseUInt16(data, 2);

            return bwStart;
        }

        /// <summary>
        /// Encode a RDP_BW_START structure for subheader, don't encode the first two field 
        /// </summary>
        /// <param name="bwStart"></param>
        /// <returns></returns>
        public static byte[] EncodeRdpBWStart(RDP_BW_START bwStart)
        {
            List<byte> bufferList = new List<byte>();
            bufferList.AddRange(ToBytes(bwStart.sequenceNumber));
            bufferList.AddRange(ToBytes((ushort)bwStart.requestType));

            return bufferList.ToArray();
        }

        /// <summary>
        /// Parse RDP_BW_STOP structure from RDPEMT subheader
        /// </summary>
        /// <param name="data">Data of subheader, not include first two bytes of Parse RDP_BW_STOP</param>
        /// <returns></returns>
        public static RDP_BW_STOP ParseRdpBWStop(byte[] data)
        {
            RDP_BW_STOP bwStop = new RDP_BW_STOP();
            bwStop.sequenceNumber = ParseUInt16(data, 0);
            bwStop.requestType = (AUTO_DETECT_REQUEST_TYPE)ParseUInt16(data, 2);
            // payloadLength and payload Must not be present when the structure is in SubHeaderData of RDP_TUNNEL_SUBHEADER 

            return bwStop;
        }

        /// <summary>
        /// Encode a RDP_BW_STOP structure for subheader, don't encode the first two field 
        /// </summary>
        /// <param name="bwStop"></param>
        /// <returns></returns>
        public static byte[] EncodeRdpBWStop(RDP_BW_STOP bwStop)
        {
            List<byte> bufferList = new List<byte>();
            bufferList.AddRange(ToBytes(bwStop.sequenceNumber));
            bufferList.AddRange(ToBytes((ushort)bwStop.requestType));

            // payloadLength and payload Must not be present when the structure is in SubHeaderData of RDP_TUNNEL_SUBHEADER 
            return bufferList.ToArray();
        }

        /// <summary>
        /// Parse RDP_NETCHAR_RESULT structure from RDPEMT subheader
        /// </summary>
        /// <param name="data">Data of subheader, not include first two bytes of Parse RDP_NETCHAR_RESULT</param>
        /// <returns></returns>
        public static RDP_NETCHAR_RESULT ParseRdpNetCharResult(byte[] data)
        {
            RDP_NETCHAR_RESULT ncRes = new RDP_NETCHAR_RESULT();
            ncRes.sequenceNumber = ParseUInt16(data, 0);
            ncRes.requestType = (AUTO_DETECT_REQUEST_TYPE)ParseUInt16(data, 2);

            int index = 4;
            if (ncRes.requestType == AUTO_DETECT_REQUEST_TYPE.RDP_NETCHAR_RESULT_BANDWIDTH_AVERAGERTT)
            {
                ncRes.bandwidth = ParseUInt32(data, index);
                index += 4;
                ncRes.averageRTT = ParseUInt32(data, index);
                index += 4;
            }
            else if (ncRes.requestType == AUTO_DETECT_REQUEST_TYPE.RDP_NETCHAR_RESULT_BASERTT_AVERAGERTT)
            {
                ncRes.baseRTT = ParseUInt32(data, index);
                index += 4;
                ncRes.averageRTT = ParseUInt32(data, index);
                index += 4;
            }
            else if (ncRes.requestType == AUTO_DETECT_REQUEST_TYPE.RDP_NETCHAR_RESULT_BASERTT_BANDWIDTH_AVERAGERTT)
            {
                ncRes.baseRTT = ParseUInt32(data, index);
                index += 4;
                ncRes.bandwidth = ParseUInt32(data, index);
                index += 4;
                ncRes.averageRTT = ParseUInt32(data, index);
                index += 4;
            }

            return ncRes;
        }

        /// <summary>
        /// Encode a RDP_NETCHAR_RESULT structure for subheader, don't encode the first two field 
        /// </summary>
        /// <param name="bwStop"></param>
        /// <returns></returns>
        public static byte[] EncodeNetCharResult(RDP_NETCHAR_RESULT ncRes)
        {
            List<byte> bufferList = new List<byte>();
            bufferList.AddRange(ToBytes(ncRes.sequenceNumber));
            bufferList.AddRange(ToBytes((ushort)ncRes.requestType));

            if (ncRes.requestType == AUTO_DETECT_REQUEST_TYPE.RDP_NETCHAR_RESULT_BANDWIDTH_AVERAGERTT)
            {
                bufferList.AddRange(ToBytes(ncRes.bandwidth));
                bufferList.AddRange(ToBytes(ncRes.averageRTT));
            }
            else if (ncRes.requestType == AUTO_DETECT_REQUEST_TYPE.RDP_NETCHAR_RESULT_BASERTT_AVERAGERTT)
            {
                bufferList.AddRange(ToBytes(ncRes.baseRTT));
                bufferList.AddRange(ToBytes(ncRes.averageRTT));
            }
            else if (ncRes.requestType == AUTO_DETECT_REQUEST_TYPE.RDP_NETCHAR_RESULT_BASERTT_BANDWIDTH_AVERAGERTT)
            {
                bufferList.AddRange(ToBytes(ncRes.baseRTT));
                bufferList.AddRange(ToBytes(ncRes.bandwidth));
                bufferList.AddRange(ToBytes(ncRes.averageRTT));
            }

            return bufferList.ToArray();
        }


        /// <summary>
        /// Parse RDP_BW_RESULTS structure from RDPEMT subheader
        /// </summary>
        /// <param name="data">Data of subheader, not include first two bytes of Parse RDP_BW_RESULTS</param>
        /// <returns></returns>
        public static RDP_BW_RESULTS ParseRdpBWResults(byte[] data)
        {
            RDP_BW_RESULTS bwRes = new RDP_BW_RESULTS();
            bwRes.sequenceNumber = ParseUInt16(data, 0);
            bwRes.responseType = (AUTO_DETECT_RESPONSE_TYPE)ParseUInt16(data, 2);
            int index = 4;
            bwRes.timeDelta = ParseUInt32(data, index);
            index += 4;
            bwRes.byteCount = ParseUInt32(data, index);
            index += 4;

            return bwRes;
        }

        /// <summary>
        /// Encode a RDP_BW_RESULTS structure for subheader, don't encode the first two field 
        /// </summary>
        /// <param name="bwStop"></param>
        /// <returns></returns>
        public static byte[] EncodeRdpBWResults(RDP_BW_RESULTS bwRes)
        {
            List<byte> bufferList = new List<byte>();
            bufferList.AddRange(ToBytes(bwRes.sequenceNumber));
            bufferList.AddRange(ToBytes((ushort)bwRes.responseType));
            bufferList.AddRange(ToBytes(bwRes.timeDelta));
            bufferList.AddRange(ToBytes(bwRes.byteCount));

            return bufferList.ToArray();
        }
        #region Private Methods

        /// <summary>
        /// Parse UInt32
        /// </summary>
        /// <param name="data"></param>
        /// <param name="startPos"></param>
        /// <returns></returns>
        private static uint ParseUInt32(byte[] data, int startPos)
        {
            byte[] buffer = new byte[4];
            Array.Copy(data, startPos, buffer, 0, 4);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(buffer);
            }
            return BitConverter.ToUInt32(buffer, 0);
        }

        /// <summary>
        /// Parse UInt16
        /// </summary>
        /// <param name="data"></param>
        /// <param name="startPos"></param>
        /// <returns></returns>
        private static ushort ParseUInt16(byte[] data, int startPos)
        {
            byte[] buffer = new byte[2];
            Array.Copy(data, startPos, buffer, 0, 2);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(buffer);
            }
            return BitConverter.ToUInt16(buffer, 0);
        }

        /// <summary>
        /// Encode UInt32
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static byte[] ToBytes(uint value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(buffer);
            }
            return buffer;
        }

        /// <summary>
        /// Encode UInt16
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static byte[] ToBytes(ushort value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(buffer);
            }
            return buffer;
        }
        #endregion Private Methos
    }
}
