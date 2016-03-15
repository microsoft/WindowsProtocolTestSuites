// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpemt
{
    /// <summary>
    /// Class for decoding MS-RDPEMT packets
    /// </summary>
    public class RdpemtDecoder
    {
        /// <summary>
        /// Buffer used to save un-decoded data
        /// </summary>
        private List<byte> buffer;

        /// <summary>
        /// Constructor
        /// </summary>
        public RdpemtDecoder()
        {
            buffer = new List<byte>();
        }

        /// <summary>
        /// Decode bytes data
        /// This method will buffer data cannot decoded this time, and merged it with data from next call
        /// </summary>
        /// <param name="newData"></param>
        /// <returns></returns>
        public RdpemtBasePDU[] Decode(byte[] newData)
        {            
            int index = 0;            
            buffer.AddRange(newData);
            byte[]  data = buffer.ToArray();

            List<RdpemtBasePDU> pduList = new List<RdpemtBasePDU>();
            RdpemtBasePDU emtPacket = DecodeRdpemtPacket(data, ref index);
            while (emtPacket != null)
            {
                pduList.Add(emtPacket);
                emtPacket = DecodeRdpemtPacket(data, ref index);
            }
            buffer.RemoveRange(0, index);

            return pduList.ToArray();
        }
        /// <summary>
        /// Call this method to decode byte array to MS-RDPEMT packets
        /// </summary>
        /// <param name="action">The action value in header</param>
        /// <param name="data">Data in bytes to be decoded</param>
        /// <returns>The decoded packet</returns>
        public static RdpemtBasePDU DecodeRdpemtPacket(byte[] data, ref int index)
        {
            if (data == null || data.Length - index < 4)
            {
                return null;
            }

            byte action = (byte)(0xF & data[index]);
            byte[] payloadLenData = new byte[2];
            Array.Copy(data, index + 1, payloadLenData, 0, 2);
            if (!BitConverter.IsLittleEndian)
            {
                // Reverse the sequence if it is not little endian
                Array.Reverse(payloadLenData);
            }
            ushort payloadLength = BitConverter.ToUInt16(payloadLenData, 0);
            byte headerLength = data[index + 3];

            int expectLen = payloadLength + headerLength;

            if (expectLen > data.Length)
            {
                return null;
            }

            
            byte[] toDecodeData = new byte[expectLen];
            Array.Copy(data, index, toDecodeData, 0, expectLen);
            index += expectLen;

            RdpemtBasePDU rePdu = null;
            if (action == (byte)RDP_TUNNEL_ACTION_Values.RDPTUNNEL_ACTION_CREATEREQUEST)
            {
                RDP_TUNNEL_CREATEREQUEST createReq = new RDP_TUNNEL_CREATEREQUEST();
                bool bResult = PduMarshaler.Unmarshal(toDecodeData, createReq);
                if (bResult)
                {
                    rePdu = createReq;
                }
            }
            else if (action == (byte)RDP_TUNNEL_ACTION_Values.RDPTUNNEL_ACTION_CREATERESPONSE)
            {
                RDP_TUNNEL_CREATERESPONSE createRes = new RDP_TUNNEL_CREATERESPONSE();
                bool bResult = PduMarshaler.Unmarshal(toDecodeData, createRes);
                if (bResult)
                {
                    rePdu = createRes;
                }

            }
            else if (action == (byte)RDP_TUNNEL_ACTION_Values.RDPTUNNEL_ACTION_DATA)
            {
                RDP_TUNNEL_DATA tunnelData = new RDP_TUNNEL_DATA();
                bool bResult = PduMarshaler.Unmarshal(toDecodeData, tunnelData);
                if (bResult)
                {
                    rePdu = tunnelData;
                }               

            }
            else
            {
                throw new NotSupportedException("Unknow action in RDP_TUNNEL_HEADER:" + action);
            }

            if (rePdu == null)
            {
                throw new NotSupportedException("Decode for RDPEMT PDU failed, Action:" + action);
            }

            return rePdu;
        }
    }
}

