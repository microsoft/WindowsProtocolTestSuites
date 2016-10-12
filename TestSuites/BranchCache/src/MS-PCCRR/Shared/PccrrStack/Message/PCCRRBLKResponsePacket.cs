// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrr
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The PccrrBLKResponsePacket is sent by the server.
    /// </summary>
    public class PccrrBLKResponsePacket : PccrrPacket
    {
        /// <summary>
        /// The messageHeader
        /// </summary>
        private MESSAGE_HEADER messageHeader;

        /// <summary>
        /// The msgBLK
        /// </summary>
        private MSG_BLK msgBLK;

        /// <summary>
        /// The transportResponseHeader
        /// </summary>
        private TRANSPORT_RESPONSE_HEADER transportResponseHeader;

        /// <summary>
        /// Initializes a new instance of the <see cref="PccrrBLKResponsePacket"/> class.
        /// </summary>
        public PccrrBLKResponsePacket()
        {
        }

        /// <summary>
        /// Get the type of the packet.
        /// </summary>
        /// <returns>The type of the packet.</returns>
        public override MsgType_Values PacketType
        {
            get
            {
                return MsgType_Values.MSG_BLK;
            }
        }

        /// <summary>
        /// Gets or sets the MessageHeader.
        /// </summary>
        public MESSAGE_HEADER MessageHeader
        {
            get
            {
                return this.messageHeader;
            }

            set
            {
                this.messageHeader = value;
            }
        }

        /// <summary>
        /// Gets or sets the MsgBLK.
        /// </summary>
        public MSG_BLK MsgBLK
        {
            get
            {
                return this.msgBLK;
            }

            set
            {
                this.msgBLK = value;
            }
        }

        /// <summary>
        /// Gets or sets the TransportResponseHeader.
        /// </summary>
        public TRANSPORT_RESPONSE_HEADER TransportResponseHeader
        {
            get
            {
                return this.transportResponseHeader;
            }

            set
            {
                this.transportResponseHeader = value;
            }
        }

        /// <summary>
        /// Encode pack.
        /// </summary>
        /// <returns>Encode bytes.</returns>
        public override byte[] Encode()
        {
            RESPONSE_MESSAGE responseMessage = new RESPONSE_MESSAGE();
            responseMessage.MESSAGEBODY = this.msgBLK;
            responseMessage.MESSAGEHEADER = this.MessageHeader;
            byte[] ret;

            List<byte> temp = new List<byte>();
            byte[] tempPayload;

            List<byte> listRet = new List<byte>();
            listRet.AddRange(MarshalHelper.GetBytes(((MSG_BLK)responseMessage.MESSAGEBODY).SizeOfSegmentId, false));
            listRet.AddRange(((MSG_BLK)responseMessage.MESSAGEBODY).SegmentId);
            while (listRet.Count % 4 != 0)
            {
                listRet.Add(byte.MinValue);
            }

            listRet.AddRange(MarshalHelper.GetBytes(((MSG_BLK)responseMessage.MESSAGEBODY).BlockIndex, false));
            listRet.AddRange(MarshalHelper.GetBytes(((MSG_BLK)responseMessage.MESSAGEBODY).NextBlockIndex, false));
            listRet.AddRange(MarshalHelper.GetBytes(((MSG_BLK)responseMessage.MESSAGEBODY).SizeOfBlock, false));
            if (((MSG_BLK)responseMessage.MESSAGEBODY).Block != null)
            {
                listRet.AddRange(((MSG_BLK)responseMessage.MESSAGEBODY).Block);
            }

            while (listRet.Count % 4 != 0)
            {
                listRet.Add(byte.MinValue);
            }

            listRet.AddRange(MarshalHelper.GetBytes(((MSG_BLK)responseMessage.MESSAGEBODY).SizeOfVrfBlock, false));
            while (listRet.Count % 4 != 0)
            {
                listRet.Add(byte.MinValue);
            }

            listRet.AddRange(MarshalHelper.GetBytes(((MSG_BLK)responseMessage.MESSAGEBODY).SizeOfIVBlock, false));
            if (((MSG_BLK)responseMessage.MESSAGEBODY).IVBlock != null)
            {
                listRet.AddRange(((MSG_BLK)responseMessage.MESSAGEBODY).IVBlock);
            }

            tempPayload = listRet.ToArray();

            responseMessage.MESSAGEHEADER.MsgSize = (uint)tempPayload.Length + 16;

            List<byte> listRet1 = new List<byte>();
            listRet1.AddRange(MarshalHelper.GetBytes((ushort)responseMessage.MESSAGEHEADER.ProtVer.MinorVersion, false));
            listRet1.AddRange(MarshalHelper.GetBytes((ushort)responseMessage.MESSAGEHEADER.ProtVer.MajorVersion, false));
            listRet1.AddRange(MarshalHelper.GetBytes((uint)responseMessage.MESSAGEHEADER.MsgType, false));
            listRet1.AddRange(MarshalHelper.GetBytes(responseMessage.MESSAGEHEADER.MsgSize, false));
            listRet1.AddRange(MarshalHelper.GetBytes((uint)responseMessage.MESSAGEHEADER.CryptoAlgoId, false));

            temp.AddRange(listRet1.ToArray());
            temp.AddRange(tempPayload);
            temp.InsertRange(0, MarshalHelper.GetBytes((uint)temp.Count, false));
            ret = (byte[])temp.ToArray();

            return ret;
        }

        /// <summary>
        /// Decode pack.
        /// </summary>
        /// <param name="rawdata">The rawdata.</param>
        /// <returns>The PccrrPacket.</returns>
        public override PccrrPacket Decode(byte[] rawdata)
        {
            if (rawdata == null)
            {
                throw new ArgumentNullException("rawdata");
            }

            if (rawdata.Length == 0)
            {
                throw new ArgumentException("The rawdata should not be empty.");
            }

            PccrrBLKResponsePacket packet = new PccrrBLKResponsePacket();

            return packet;
        }
    }
}
