// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc
{
    public class PduBuilder
    {
        public DynamicVCPDU ToPdu(byte[] data)
        {
            if (null == regsiteredPDUs)
            {
                regsiteredPDUs = new List<DynamicVCPDU>();
                RegisterDefaultPdus();
            }

            DynamicVCPDU res = null;
            foreach (DynamicVCPDU pdu in regsiteredPDUs)
            {
                if (PduMarshaler.Unmarshal(data, pdu))
                {
                    res = pdu;
                    break;
                }
            }

            if (res == null)
            {
                DynamicVCException.Throw("UnknownDynamicVCPDU was not registered.");
            }
            return res;
        }

        protected List<DynamicVCPDU> regsiteredPDUs = null;
        protected virtual void RegisterDefaultPdus()
        {
        }

        public void RegisterPdu(DynamicVCPDU pdu)
        {
            if (null == regsiteredPDUs)
            {
                regsiteredPDUs = new List<DynamicVCPDU>();
            }

            regsiteredPDUs.Add(pdu);
        }

        public byte[] ToRawData(DynamicVCPDU pdu)
        {
            return PduMarshaler.Marshal(pdu);
        }

        public UnknownDynamicVCPDU CreateUnknownPdu(byte[] rawData)
        {
            return new UnknownDynamicVCPDU(rawData);
        }

        public CapsVer1ReqDvcPdu CreateCapsV1ReqPdu()
        {
            return new CapsVer1ReqDvcPdu();
        }

        public CapsVer2ReqDvcPdu CreateCapsV2ReqPdu(
            ushort priorityCharge0,
            ushort priorityCharge1,
            ushort priorityCharge2,
            ushort priorityCharge3
            )
        {
            return new CapsVer2ReqDvcPdu(
                priorityCharge0,
                priorityCharge1,
                priorityCharge2,
                priorityCharge3
                );
        }

        public CapsVer2ReqDvcPdu CreateCapsV2ReqPdu()
        {
            return new CapsVer2ReqDvcPdu(
                936, // 70%
                3276, // 20%
                9362, // 7%
                21845 // 3%
                );
        }

        public CapsVer3ReqDvcPdu CreateCapsV3ReqPdu(
            ushort priorityCharge0,
            ushort priorityCharge1,
            ushort priorityCharge2,
            ushort priorityCharge3
            )
        {
            return new CapsVer3ReqDvcPdu(
                priorityCharge0,
                priorityCharge1,
                priorityCharge2,
                priorityCharge3
                );
        }

        public CapsVer3ReqDvcPdu CreateCapsV3ReqPdu()
        {
            return new CapsVer3ReqDvcPdu(
                936, // 70%
                3276, // 20%
                9362, // 7%
                21845 // 3%
                );
        }

        public CapsRespDvcPdu CreateCapsRespPdu(ushort version)
        {
            return new CapsRespDvcPdu(version);
        }

        public CreateReqDvcPdu CreateCreateReqDvcPdu(ushort priority, uint channelId, string channelName)
        {
            return new CreateReqDvcPdu(priority, channelId, channelName);
        }

        public CreateRespDvcPdu CreateCreateRespDvcPdu(uint channelId, int creationStatus)
        {
            return new CreateRespDvcPdu(channelId, creationStatus);
        }

        /// <summary>
        ///  Create DYNVC_SOFT_SYNC_REQUEST PDU. 
        /// </summary>
        public SoftSyncReqDvcPDU CreateSoftSyncReqPdu(SoftSyncReqFlags_Value flags, ushort numberOfTunnels, SoftSyncChannelList[] channelList = null)
        {
            return new SoftSyncReqDvcPDU(flags, numberOfTunnels, channelList);
        }

        public byte[] CompressDataToRdp8BulkEncodedData(byte[] data, PACKET_COMPR_FLAG compressedFlag)
        {
            //When the length of the original uncompressed message data being sent exceeds 1,590 bytes, 
            //and bulk data compression of the channel data is desired, the DYNVC_DATA_FIRST_COMPRESSED (section 2.2.3.3) PDU is sent as the first data PDU.             
            if (compressedFlag == (PACKET_COMPR_FLAG.PACKET_COMPR_TYPE_LITE | PACKET_COMPR_FLAG.PACKET_COMPRESSED))
            {
                CompressFactory cpf = new CompressFactory(
                   (int)SEGMENT_PART_SISE.MAX_PACKET_COMPR_TYPE_RDP8_LITE_MATCH_DISTANCE,
                   (int)SEGMENT_PART_SISE.MAX_PACKET_COMPR_TYPE_RDP8_LITE_SEGMENT_PART_SIZE);
                return cpf.Compress(data);

            }
            if (compressedFlag == (PACKET_COMPR_FLAG.PACKET_COMPR_TYPE_RDP8 | PACKET_COMPR_FLAG.PACKET_COMPRESSED)
                )
            {
                CompressFactory cpf = new CompressFactory(
                   (int)SEGMENT_PART_SISE.MAX_PACKET_COMPR_TYPE_RDP8_MATCH_DISTANCE,
                   (int)SEGMENT_PART_SISE.MAX_PACKET_COMPR_TYPE_RDP8_SEGMENT_PART_SIZE);
                return cpf.Compress(data);
            }
            //Otherwise, no compress
            return data;
        }

        /// <summary>
        ///  Create DYNVC_DATA_FIRST_COMPRESSED 
        /// </summary>        
        public DataFirstCompressedDvcPdu CreateDataFristCompressedReqPdu(uint channelId, byte[] data)
        {
            DataFirstCompressedDvcPdu firstCompressedPdu = null;
           
            if (data.Length <= ConstLength.MAX_FIRST_COMPRESSED_DATA_LENGTH)
            {
                firstCompressedPdu = new DataFirstCompressedDvcPdu(channelId, (uint)data.Length, data);
            }
            else
            {
                byte[] firstBlockData = new byte[(int)SEGMENT_PART_SISE.MAX_PACKET_COMPR_TYPE_RDP8_LITE_SEGMENT_PART_SIZE];
                Array.Copy(data, firstBlockData, (long)SEGMENT_PART_SISE.MAX_PACKET_COMPR_TYPE_RDP8_LITE_SEGMENT_PART_SIZE);

                firstCompressedPdu = new DataFirstCompressedDvcPdu(channelId, (uint)data.Length, firstBlockData);
            }
            firstCompressedPdu.GetNonDataSize();


            return firstCompressedPdu;
        }

        /// <summary>
        ///  Create DYNVC_DATA_COMPRESSED 
        /// </summary>
        public DataCompressedDvcPdu CreateDataCompressedReqPdu(uint channelId, byte[] data)
        {
            DataCompressedDvcPdu compressedPdu = new DataCompressedDvcPdu(channelId, data);
            compressedPdu.GetNonDataSize();
            return compressedPdu;
        }

        public CloseDvcPdu CreateCloseDvcPdu(uint channelId)
        {
            return new CloseDvcPdu(channelId);
        }
        /// <summary>
        /// Create the DYNVC_DATA PDU
        /// </summary>
        /// <param name="channelId">The channelId</param>
        /// <param name="data">The uncompressed data</param>
        /// <param name="channelChunkLength">The maximum number of uncompressed bytes in a single segment </param>        
        /// <returns>Return the first and second PDUs in an array</returns>
        public DataDvcBasePdu[] CreateDataPdu(uint channelId, byte[] data, int channelChunkLength)
        {
            DataFirstDvcPdu first = new DataFirstDvcPdu(channelId, (uint)data.Length, null);
            DataDvcPdu other = new DataDvcPdu(channelId, null);

            int firstNonDataSize = first.GetNonDataSize();
            int otherNonDataSize = other.GetNonDataSize();

            if (data.Length <= channelChunkLength - otherNonDataSize)
            {
                other.Data = data;
                return new DataDvcBasePdu[] { other };
            }

            // TODO: need to test for the following code
            byte[] buf = new byte[channelChunkLength - firstNonDataSize];
            MemoryStream ms = new MemoryStream(data);
            List<DataDvcBasePdu> pdus = new List<DataDvcBasePdu>();

            if (channelChunkLength - firstNonDataSize != ms.Read(buf, 0, channelChunkLength - firstNonDataSize))
            {
                DynamicVCException.Throw("Cannot create correct data PDUs.");
            }
            first.Data = buf;
            pdus.Add(first);

            buf = new byte[channelChunkLength - otherNonDataSize];
            // TODO: Check this logic
            int readLen = 0;
            readLen = ms.Read(buf, 0, channelChunkLength - otherNonDataSize);
            while (readLen == channelChunkLength - otherNonDataSize)
            {
                pdus.Add(new DataDvcPdu(channelId, buf));
                buf = new byte[channelChunkLength - otherNonDataSize];
                readLen = ms.Read(buf, 0, channelChunkLength - otherNonDataSize);
            }

            if (readLen > 0)
            {
                byte[] newBuf = new byte[readLen];
                Array.Copy(buf, newBuf, readLen);
                pdus.Add(new DataDvcPdu(channelId, newBuf));
            }

            return pdus.ToArray();
        }

        /// <summary>
        /// Create a DataCompressedDvcPdu
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="data"></param>       
        /// <returns></returns>
        public DataCompressedDvcPdu[] CreateCompressedDataPdu(uint channelId, byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            List<DataCompressedDvcPdu> pdus = new List<DataCompressedDvcPdu>();

            byte[] compressed = CompressDataToRdp8BulkEncodedData(data, PACKET_COMPR_FLAG.PACKET_COMPR_TYPE_LITE | PACKET_COMPR_FLAG.PACKET_COMPRESSED);
            
            DataCompressedDvcPdu pdu = new DataCompressedDvcPdu();
            pdu.HeaderBits.Cmd = Cmd_Values.DataCompressed;
            pdu.HeaderBits.Sp = 0x0;
            pdu.HeaderBits.CbChannelId = cbChId_Values.OneByte;
            pdu.ChannelId = channelId;

            RDP_SEGMENTED_DATA rdpSegmentedData = new RDP_SEGMENTED_DATA();
            rdpSegmentedData.descriptor = DescriptorTypes.SINGLE;

            RDP8_BULK_ENCODED_DATA rdp8BulkEncodedData = new RDP8_BULK_ENCODED_DATA();
            rdp8BulkEncodedData.header = (byte) (PACKET_COMPR_FLAG.PACKET_COMPR_TYPE_LITE | PACKET_COMPR_FLAG.PACKET_COMPRESSED);
            rdp8BulkEncodedData.data = compressed;

            pdu.Data = PduMarshaler.Marshal(rdpSegmentedData);
            pdus.Add(pdu);
            return pdus.ToArray();
        }
        
    }


    /// <summary>
    /// This builder is used to decode all PDU types possibly received by the server role.
    /// </summary>
    public class ServerDecodingPduBuilder : PduBuilder
    {
        protected override void RegisterDefaultPdus()
        {
            regsiteredPDUs.Add(new DataFirstDvcPdu());
            regsiteredPDUs.Add(new DataDvcPdu());
            // The DYNVC_CAPS_RSP (section 2.2.1.2) PDU is sent by the DVC client manager to 
            // indicate the protocol version level it supports.
            regsiteredPDUs.Add(new CapsRespDvcPdu());
            // The DYNVC_CREATE_RSP (section 2.2.2.2) PDU is sent by the DVC client manager to 
            // indicate the status of the client dynamic virtual channel create operation.
            regsiteredPDUs.Add(new CreateRespDvcPdu());
            regsiteredPDUs.Add(new SoftSyncResDvcPdu());
            regsiteredPDUs.Add(new DataFirstCompressedDvcPdu());
            regsiteredPDUs.Add(new DataCompressedDvcPdu());
            regsiteredPDUs.Add(new CloseDvcPdu());
            regsiteredPDUs.Add(new UnknownDynamicVCPDU());
        }
    }

    /// <summary>
    /// This builder is used to decode all PDU types possibly received by the client role.
    /// </summary>
    public class ClientDecodingPduBuilder : PduBuilder
    {
        protected override void RegisterDefaultPdus()
        {
            regsiteredPDUs.Add(new DataFirstDvcPdu());
            regsiteredPDUs.Add(new DataDvcPdu());
            // The DYNVC_CAPS_VERSION1 (section 2.2.1.1.1) PDU is sent by the DVC server 
            // manager to indicate that it supports version 1 of the Remote Desktop Protocol: 
            // Dynamic Channel Virtual Channel Extension.<1>
            regsiteredPDUs.Add(new CapsVer1ReqDvcPdu());
            // The DYNVC_CAPS_VERSION2 (section 2.2.1.1.2) PDU is sent by the DVC server manager 
            // to indicate that it supports version 2 of the Remote Desktop Protocol: Dynamic 
            // Channel Virtual Channel Extension.<3>
            regsiteredPDUs.Add(new CapsVer2ReqDvcPdu());
            // The DYNVC_CAPS_VERSION3(section 2.2.1.1.3) PDU is sent by the DVC server manager 
            // to indicate that it supports version 3 of the Remote Desktop Protocol: Dynamic 
            // Channel Virtual Channel Extension.<5>
            regsiteredPDUs.Add(new CapsVer3ReqDvcPdu());
            // The DYNVC_CREATE_REQ (section 2.2.2.1) PDU is sent by the DVC server manager 
            // to the DVC client manager to request that a channel be opened.
            regsiteredPDUs.Add(new CreateReqDvcPdu());
            regsiteredPDUs.Add(new CloseDvcPdu());
            regsiteredPDUs.Add(new UnknownDynamicVCPDU());
            regsiteredPDUs.Add(new DataFirstCompressedDvcPdu());
            regsiteredPDUs.Add(new DataCompressedDvcPdu());
            regsiteredPDUs.Add(new SoftSyncReqDvcPDU());
        }
    }
}
