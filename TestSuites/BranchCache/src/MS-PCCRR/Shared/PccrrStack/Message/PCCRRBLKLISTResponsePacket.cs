// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrr
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The PccrrBLKLISTResponsePacket is sent by the server.
    /// </summary>
    public class PccrrBLKLISTResponsePacket : PccrrPacket
    {
        /// <summary>
        /// The messageHeader
        /// </summary>
        private MESSAGE_HEADER messageHeader;

        /// <summary>
        /// The msgBLKLIST
        /// </summary>
        private MSG_BLKLIST msgBLKLIST;

        /// <summary>
        /// The transportResponseHeader
        /// </summary>
        private TRANSPORT_RESPONSE_HEADER transportResponseHeader;

        /// <summary>
        /// Initializes a new instance of the <see cref="PccrrBLKLISTResponsePacket"/> class.
        /// </summary>
        public PccrrBLKLISTResponsePacket()
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
                return MsgType_Values.MSG_BLKLIST;
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
        /// Gets or sets the MsgBLKLIST.
        /// </summary>
        public MSG_BLKLIST MsgBLKLIST
        {
            get
            {
                return this.msgBLKLIST;
            }

            set
            {
                this.msgBLKLIST = value;
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
        /// <returns>Encoded bytes.</returns>
        public override byte[] Encode()
        {
            RESPONSE_MESSAGE responseMessage = new RESPONSE_MESSAGE();
            responseMessage.MESSAGEBODY = this.msgBLKLIST;
            responseMessage.MESSAGEHEADER = this.MessageHeader;
            byte[] ret;

            List<byte> temp = new List<byte>();
            byte[] tempPayload;

            List<byte> listRet = new List<byte>();
            listRet.AddRange(MarshalHelper.GetBytes(((MSG_BLKLIST)responseMessage.MESSAGEBODY).SizeOfSegmentId, false));
            listRet.AddRange(((MSG_BLKLIST)responseMessage.MESSAGEBODY).SegmentId);
            while (listRet.Count % 4 != 0)
            {
                listRet.Add(byte.MinValue);
            }

            listRet.AddRange(MarshalHelper.GetBytes(((MSG_BLKLIST)responseMessage.MESSAGEBODY).BlockRangeCount, false));
            for (int i = 0; i < ((MSG_BLKLIST)responseMessage.MESSAGEBODY).BlockRangeCount; i++)
            {
                listRet.AddRange(MarshalHelper.GetBytes(((MSG_BLKLIST)responseMessage.MESSAGEBODY).BlockRanges[i].Index, false));
                listRet.AddRange(MarshalHelper.GetBytes(((MSG_BLKLIST)responseMessage.MESSAGEBODY).BlockRanges[i].Count, false));
            }

            listRet.AddRange(MarshalHelper.GetBytes(((MSG_BLKLIST)responseMessage.MESSAGEBODY).NextBlockIndex, false));
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
                throw new ArgumentException("The raw data should not be empty.");
            }

            PccrrBLKLISTResponsePacket packet = new PccrrBLKLISTResponsePacket();

            return packet;
        }
    }
}

