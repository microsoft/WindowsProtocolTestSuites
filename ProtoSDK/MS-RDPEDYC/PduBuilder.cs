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

        public CloseDvcPdu CreateCloseDvcPdu(uint channelId)
        {
            return new CloseDvcPdu(channelId);
        }

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
            // The DYNVC_CREATE_REQ (section 2.2.2.1) PDU is sent by the DVC server manager 
            // to the DVC client manager to request that a channel be opened.
            regsiteredPDUs.Add(new CreateReqDvcPdu());
            regsiteredPDUs.Add(new CloseDvcPdu());
            regsiteredPDUs.Add(new UnknownDynamicVCPDU());
            regsiteredPDUs.Add(new SoftSyncReqDvcPDU());
        }
    }
}
