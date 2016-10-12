// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrr
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The PccrrNegoResponsePacket is sent by the server.
    /// </summary>
    public class PccrrNegoResponsePacket : PccrrPacket
    {
        /// <summary>
        /// The messageHeader
        /// </summary>
        private MESSAGE_HEADER messageHeader;

        /// <summary>
        /// The msgNegoResp
        /// </summary>
        private MSG_NEGO_RESP msgNegoResp;

        /// <summary>
        /// The transportResponseHeader
        /// </summary>
        private TRANSPORT_RESPONSE_HEADER transportResponseHeader;

        /// <summary>
        /// Initializes a new instance of the <see cref="PccrrNegoResponsePacket"/> class.
        /// </summary>
        public PccrrNegoResponsePacket()
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
                return MsgType_Values.MSG_NEGO_RESP;
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
        /// Gets or sets the MsgNegoResp.
        /// </summary>
        public MSG_NEGO_RESP MsgNegoResp
        {
            get
            {
                return this.msgNegoResp;
            }

            set
            {
                this.msgNegoResp = value;
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
            responseMessage.MESSAGEBODY = this.msgNegoResp;
            responseMessage.MESSAGEHEADER = this.MessageHeader;
            byte[] ret;

            List<byte> temp = new List<byte>();
            byte[] tempPayload;

            List<byte> listRet = new List<byte>();
            listRet.AddRange(MarshalHelper.GetBytes(((MSG_NEGO_RESP)responseMessage.MESSAGEBODY).MinSupporteProtocolVersion.MinorVersion, false));
            listRet.AddRange(MarshalHelper.GetBytes(((MSG_NEGO_RESP)responseMessage.MESSAGEBODY).MinSupporteProtocolVersion.MajorVersion, false));
            listRet.AddRange(MarshalHelper.GetBytes(((MSG_NEGO_RESP)responseMessage.MESSAGEBODY).MaxSupporteProtocolVersion.MinorVersion, false));
            listRet.AddRange(MarshalHelper.GetBytes(((MSG_NEGO_RESP)responseMessage.MESSAGEBODY).MaxSupporteProtocolVersion.MajorVersion, false));
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

            PccrrNegoResponsePacket packet = new PccrrNegoResponsePacket();

            int messageLength = 0;

            messageLength = rawdata.Length;

            if (messageLength > 0)
            {
                int index = 0;

                byte[] data = rawdata;

                RESPONSE_MESSAGE ret = new RESPONSE_MESSAGE();
                ret.TRANSPORTRESPONSEHEADER.Size = MarshalHelper.GetUInt32(data, ref index, false);

                MESSAGE_HEADER ret1 = new MESSAGE_HEADER();
                ret1.ProtVer.MinorVersion = MarshalHelper.GetUInt16(data, ref index, false);
                ret1.ProtVer.MajorVersion = MarshalHelper.GetUInt16(data, ref index, false);
                ret1.MsgType = (MsgType_Values)MarshalHelper.GetUInt32(data, ref index, false);
                ret1.MsgSize = MarshalHelper.GetUInt32(data, ref index, false);
                ret1.CryptoAlgoId = (CryptoAlgoId_Values)MarshalHelper.GetUInt32(data, ref index, false);

                MSG_NEGO_RESP ret2 = new MSG_NEGO_RESP();
                ret2.MinSupporteProtocolVersion.MinorVersion = MarshalHelper.GetUInt16(data, ref index, false);
                ret2.MinSupporteProtocolVersion.MajorVersion = MarshalHelper.GetUInt16(data, ref index, false);
                ret2.MaxSupporteProtocolVersion.MinorVersion = MarshalHelper.GetUInt16(data, ref index, false);
                ret2.MaxSupporteProtocolVersion.MajorVersion = MarshalHelper.GetUInt16(data, ref index, false);

                packet.TransportResponseHeader = ret.TRANSPORTRESPONSEHEADER;
                packet.MsgNegoResp = ret2;
                packet.MessageHeader = ret1;
            }

            return packet;
        }
    }
}

