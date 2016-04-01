// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;


namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{
    public class RdprfxServerDecoder
    {
        public TS_RFX_CLNT_CAPS_CONTAINER Decode_TS_RFX_CLNT_CAPS_CONTAINER(byte[] data)
        {
            int curIdx = 0;
            TS_RFX_CLNT_CAPS_CONTAINER clientRFXCaps = new TS_RFX_CLNT_CAPS_CONTAINER();
            clientRFXCaps.length = ParseUInt32(data, ref curIdx, false);
            clientRFXCaps.captureFlags = ParseUInt32(data, ref curIdx, false);
            clientRFXCaps.capsLength = ParseUInt32(data, ref curIdx, false);

            byte[] capsData = GetBytes(data,ref curIdx,(int)clientRFXCaps.capsLength);
            TS_RFX_CAPS caps = new TS_RFX_CAPS();
            curIdx = 0;
            caps.blockType = (blockType_Value)ParseUInt16(capsData, ref curIdx, false);
            caps.blockLen = ParseUInt32(capsData, ref curIdx, false);
            caps.numCapsets = ParseUInt16(capsData, ref curIdx, false);

            List<TS_RFX_CAPSET> capSetList = new List<TS_RFX_CAPSET>(); 
            for (int i = 0; i < caps.numCapsets; i++)
            {
                TS_RFX_CAPSET capSet = new TS_RFX_CAPSET();
                capSet.blockType = (blockType_Value)(blockType_Value)ParseUInt16(capsData, ref curIdx, false);
                capSet.blockLen = ParseUInt32(capsData, ref curIdx, false);
                capSet.codecId = ParseByte(capsData, ref curIdx);
                capSet.capsetType = ParseUInt16(capsData, ref curIdx, false);
                capSet.numIcaps = ParseUInt16(capsData, ref curIdx, false);
                capSet.icapLen = ParseUInt16(capsData, ref curIdx, false);

                List<TS_RFX_ICAP> icapList = new List<TS_RFX_ICAP>();
                for (int j = 0; j < capSet.numIcaps; j++)
                {
                    TS_RFX_ICAP icap = new TS_RFX_ICAP();
                    icap.version = (version_Value)ParseUInt16(capsData, ref curIdx, false);
                    icap.tileSize = (short)ParseUInt16(capsData, ref curIdx, false);
                    icap.flags = ParseByte(capsData, ref curIdx);
                    icap.colConvBits = ParseByte(capsData, ref curIdx);
                    icap.transformBits = ParseByte(capsData, ref curIdx);
                    icap.entropyBits = (entropyBits_Value)ParseByte(capsData, ref curIdx);
                    icapList.Add(icap);
                }
                capSet.icapsData = icapList.ToArray();
                capSetList.Add(capSet);
            }
            caps.capsetsData = capSetList.ToArray();
            clientRFXCaps.capsData = caps;
            return clientRFXCaps;
        }

        #region Private Methods: Base Type Parsers
        /// <summary>
        /// Parse one byte
        /// (parser index will be updated by one)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="index">parser index</param>
        /// <returns>the parsed byte</returns>
        private byte ParseByte(byte[] data, ref int index)
        {
            // if input data is null
            if (null == data)
            {
                throw new FormatException();
            }

            // if index is out of range
            if (index < 0 || index >= data.Length)
            {
                throw new FormatException();
            }

            // get a byte
            byte b = data[index];

            // update parser index
            ++index;

            return b;
        }

        /// <summary>
        /// Parse UInt16
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="index">parser index</param>
        /// <param name="isBigEndian">big endian format flag</param>
        /// <returns>parsed UInt16 number</returns>
        private UInt16 ParseUInt16(byte[] data, ref int index, bool isBigEndian)
        {
            // Read 2 bytes
            byte[] bytes = GetBytes(data, ref index, sizeof(UInt16));

            // Big Endian format requires reversed byte order
            if (isBigEndian)
            {
                Array.Reverse(bytes, 0, sizeof(UInt16));
            }

            // Convert
            return BitConverter.ToUInt16(bytes, 0);
        }

        /// <summary>
        /// Parse UInt32
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="index">parser index</param>
        /// <param name="isBigEndian">big endian format flag</param>
        /// <returns>parsed UInt32 number</returns>
        private UInt32 ParseUInt32(byte[] data, ref int index, bool isBigEndian)
        {
            // Read 4 bytes
            byte[] bytes = GetBytes(data, ref index, sizeof(UInt32));

            // Big Endian format requires reversed byte order
            if (isBigEndian)
            {
                Array.Reverse(bytes, 0, sizeof(UInt32));
            }

            // Convert
            return BitConverter.ToUInt32(bytes, 0);
        }

        /// <summary>
        /// Get specified length of bytes from a byte array
        /// (start index is updated according to the specified length)
        /// </summary>
        /// <param name="data">data in byte array</param>
        /// <param name="startIndex">start index</param>
        /// <param name="bytesToRead">specified length</param>
        /// <returns>bytes of specified length</returns>
        private byte[] GetBytes(byte[] data, ref int startIndex, int bytesToRead)
        {
            // if input data is null
            if (null == data)
            {
                throw new FormatException();
            }

            // if index is out of range
            if ((startIndex < 0) || (startIndex + bytesToRead > data.Length))
            {
                throw new FormatException();
            }

            // read bytes of specific length
            byte[] dataRead = new byte[bytesToRead];
            Array.Copy(data, startIndex, dataRead, 0, bytesToRead);

            // update index
            startIndex += bytesToRead;
            return dataRead;
        }

        #endregion
    }
}
