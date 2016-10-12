// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrr
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using Microsoft.Protocols.TestTools.StackSdk.CommonStack;
    
    /// <summary>
    /// Utitlity of pccrr.
    /// </summary>
    public class PccrrUtitlity
    {
        /// <summary>
        /// Creates Aes instance based on specified algorithm.
        /// </summary>
        /// <param name="cryptoAlgoId">Algorithm ID.</param>
        /// <returns>Aes object.</returns>
        public static Aes CreateAes(CryptoAlgoId_Values cryptoAlgoId)
        {
            if (cryptoAlgoId == CryptoAlgoId_Values.NoEncryption)
                return null;

            Aes aes = Aes.Create();
            aes.Mode = CipherMode.CBC;

            switch (cryptoAlgoId)
            {
                case CryptoAlgoId_Values.AES_128:
                    aes.KeySize = 128;
                    break;
                case CryptoAlgoId_Values.AES_192:
                    aes.KeySize = 192;
                    break;
                case CryptoAlgoId_Values.AES_256:
                    aes.KeySize = 256;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return aes;
        }

        /// <summary>
        /// Encrypts data.
        /// </summary>
        /// <param name="aes">Aes object.</param>
        /// <param name="content">Content to be encrypted.</param>
        /// <param name="key">Encryption key.</param>
        /// <param name="iv">Initial vector.</param>
        /// <returns>Encrypted data.</returns>
        public static byte[] Encrypt(Aes aes, byte[] content, byte[] key, byte[] iv)
        {
            var encryptor = aes.CreateEncryptor(key, iv);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(content, 0, content.Length);
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Decrypts data.
        /// </summary>
        /// <param name="aes">Aes object.</param>
        /// <param name="encrypted">Encrypted data to be decrypted.</param>
        /// <param name="key">Encryption key.</param>
        /// <param name="iv">Initial vector.</param>
        /// <returns>Decrypted data.</returns>
        public static byte[] Decrypt(Aes aes, byte[] encrypted, byte[] key, byte[] iv)
        {
            byte[] buffer = new byte[encrypted.Length];

            var decryptor = aes.CreateDecryptor(key, iv);

            using (MemoryStream ms = new MemoryStream(encrypted))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    int length = cs.Read(buffer, 0, encrypted.Length);
                    return buffer.Take(length).ToArray();
                }
            }
        }

        /// <summary>
        /// Merge bytes array to byte array
        /// </summary>
        /// <param name="buffer">The buffer of bytes array</param>
        /// <returns>Byte array</returns>
        public static byte[] MergeBytesArray(byte[][] buffer)
        {
            List<byte> ret = new List<byte>();

            for (int i = 0; i < buffer.Length; i++)
            {
                ret.AddRange(buffer[i]);
            }

            return ret.ToArray();
        }

        /// <summary>
        /// Convert to byte array.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>The byte array.</returns>
        public static byte[] ToByteArray(string str)
        {
            str = str.Replace(" ", string.Empty);
            if ((str.Length % 2) != 0)
            {
                str += " ";
            }

            byte[] ret = new byte[str.Length / 2];

            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = Convert.ToByte(str.Substring(i * 2, 2), 16);
            }

            return ret;
        }

        /// <summary>
        /// Create MessageHeader.
        /// </summary>
        /// <param name="cryptoAlgoIdValues">The cryptoAlgoIdValues.</param>
        /// <param name="msgTypeValues">The msgTypeValues.</param>
        /// <param name="protoVer">The protoVer.</param>
        /// <returns>The MESSAGE_HEADER struct.</returns>
        public static MESSAGE_HEADER CreateMessageHeader(CryptoAlgoId_Values cryptoAlgoIdValues, MsgType_Values msgTypeValues, ProtoVersion protoVer)
        {
            MESSAGE_HEADER messageHeader = new MESSAGE_HEADER();
            messageHeader.CryptoAlgoId = cryptoAlgoIdValues;
            messageHeader.MsgType = msgTypeValues;
            messageHeader.MsgSize = 16;
            messageHeader.ProtVer = protoVer;

            return messageHeader;
        }

        /// <summary>
        /// Decode request message.
        /// </summary>
        /// <param name="rawdata">The raw data.</param>
        /// <param name="uri">The request uri.</param>
        /// <param name="method">The request method.</param>
        /// <returns>The PccrrPacket.</returns>
        public PccrrPacket DecodeRequestMessage(byte[] rawdata, Uri uri, HttpMethod method)
        {
            if (rawdata == null)
            {
                throw new ArgumentNullException("rawdata");
            }

            if (rawdata.Length == 0)
            {
                throw new ArgumentException("The rawdata should not be empty.");
            }

            int messageLength = 0;

            messageLength = rawdata.Length;

            PccrrPacket packet = null;

            if (messageLength > 0)
            {
                int index = 0;

                REQUEST_MESSAGE ret = new REQUEST_MESSAGE();
                ret.MESSAGEHEADER = this.DecodeMessageHeader(rawdata, ref index);

                switch (ret.MESSAGEHEADER.MsgType)
                {
                    case MsgType_Values.MSG_NEGO_REQ:
                        PccrrNegoRequestPacket pccrrNegoRequestPacket = new PccrrNegoRequestPacket();

                        MSG_NEGO_REQ msgNEGOREQ = this.DecodeMSG_NEGO_REQ(rawdata, ref index);
                        pccrrNegoRequestPacket.MsgNegoReq = msgNEGOREQ;
                        pccrrNegoRequestPacket.MessageHeader = ret.MESSAGEHEADER;
                        pccrrNegoRequestPacket.Method = method;
                        pccrrNegoRequestPacket.Uri = uri;
                        packet = pccrrNegoRequestPacket;
                        break;
                    case MsgType_Values.MSG_GETBLKLIST:
                        PccrrGETBLKLISTRequestPacket pccrrGETBLKLISTRequestPacket = new PccrrGETBLKLISTRequestPacket();

                        MSG_GETBLKLIST msgGETBLKLIST = this.DecodeMSG_GETBLKLIST(rawdata, ref index);
                        pccrrGETBLKLISTRequestPacket.MsgGetBLKLIST = msgGETBLKLIST;
                        pccrrGETBLKLISTRequestPacket.MessageHeader = ret.MESSAGEHEADER;
                        pccrrGETBLKLISTRequestPacket.Method = method;
                        pccrrGETBLKLISTRequestPacket.Uri = uri;
                        packet = pccrrGETBLKLISTRequestPacket;
                        break;
                    case MsgType_Values.MSG_GETBLKS:
                        PccrrGETBLKSRequestPacket pccrrGETBLKSRequestPacket = new PccrrGETBLKSRequestPacket();

                        MSG_GETBLKS msgGETBLKS = this.DecodeMSG_GETBLKS(rawdata, ref index);
                        pccrrGETBLKSRequestPacket.MsgGetBLKS = msgGETBLKS;
                        pccrrGETBLKSRequestPacket.MessageHeader = ret.MESSAGEHEADER;
                        pccrrGETBLKSRequestPacket.Method = method;
                        pccrrGETBLKSRequestPacket.Uri = uri;
                        packet = pccrrGETBLKSRequestPacket;
                        break;

                    case MsgType_Values.MSG_GETSEGLIST:
                        PccrrGetSegListRequestPacket pccrrGetSegListRequestPacket = new PccrrGetSegListRequestPacket();
                        pccrrGetSegListRequestPacket.MsgGetSegList = TypeMarshal.ToStruct<MSG_GETSEGLIST>(rawdata, ref index);
                        pccrrGetSegListRequestPacket.MessageHeader = ret.MESSAGEHEADER;
                        pccrrGetSegListRequestPacket.Method = method;
                        pccrrGetSegListRequestPacket.Uri = uri;
                        packet = pccrrGetSegListRequestPacket;
                        break;
                }
            }

            return packet;
        }

        /// <summary>
        /// Decode response message.
        /// </summary>
        /// <param name="rawdata">The raw data.</param>
        /// <returns>The PccrrPacket.</returns>
        public PccrrPacket DecodeResponseMessage(byte[] rawdata)
        {
            if (rawdata == null)
            {
                throw new ArgumentNullException("rawdata");
            }

            if (rawdata.Length == 0)
            {
                throw new ArgumentException("The raw data should not be empty.");
            }

            int messageLength = 0;

            messageLength = rawdata.Length;

            PccrrPacket packet = null;

            if (messageLength > 0)
            {
                int index = 0;

                RESPONSE_MESSAGE ret = new RESPONSE_MESSAGE();
                ret.TRANSPORTRESPONSEHEADER.Size = MarshalHelper.GetUInt32(rawdata, ref index, false);
                ret.MESSAGEHEADER = this.DecodeMessageHeader(rawdata, ref index);

                switch (ret.MESSAGEHEADER.MsgType)
                {
                    case MsgType_Values.MSG_BLKLIST:
                        PccrrBLKLISTResponsePacket pccrrBLKLISTResponsePacket = new PccrrBLKLISTResponsePacket();

                        MSG_BLKLIST msgBLKLIST = this.DecodeMSG_BLKLIST(rawdata, ref index);
                        pccrrBLKLISTResponsePacket.TransportResponseHeader = ret.TRANSPORTRESPONSEHEADER;
                        pccrrBLKLISTResponsePacket.MsgBLKLIST = msgBLKLIST;
                        pccrrBLKLISTResponsePacket.MessageHeader = ret.MESSAGEHEADER;
                        packet = pccrrBLKLISTResponsePacket;
                        break;
                    case MsgType_Values.MSG_BLK:
                        PccrrBLKResponsePacket pccrrBLKResponsePacket = new PccrrBLKResponsePacket();

                        MSG_BLK msgBLK = this.DecodeMSG_BLK(rawdata, ref index);
                        pccrrBLKResponsePacket.TransportResponseHeader = ret.TRANSPORTRESPONSEHEADER;
                        pccrrBLKResponsePacket.MsgBLK = msgBLK;
                        pccrrBLKResponsePacket.MessageHeader = ret.MESSAGEHEADER;
                        packet = pccrrBLKResponsePacket;
                        break;
                    case MsgType_Values.MSG_NEGO_RESP:
                        PccrrNegoResponsePacket pccrrNegoResponsePacket = new PccrrNegoResponsePacket();

                        MSG_NEGO_RESP msgNEGORESP = this.DecodeMSG_NEGO_RESP(rawdata, ref index);
                        pccrrNegoResponsePacket.TransportResponseHeader = ret.TRANSPORTRESPONSEHEADER;
                        pccrrNegoResponsePacket.MsgNegoResp = msgNEGORESP;
                        pccrrNegoResponsePacket.MessageHeader = ret.MESSAGEHEADER;
                        packet = pccrrNegoResponsePacket;
                        break;

                    case MsgType_Values.MSG_SEGLIST:
                        PccrrSegListResponsePacket pccrrSegListResponsePacket = new PccrrSegListResponsePacket();
                        pccrrSegListResponsePacket.MsgSegList = TypeMarshal.ToStruct<MSG_SEGLIST>(rawdata, ref index);
                        pccrrSegListResponsePacket.TransportResponseHeader = ret.TRANSPORTRESPONSEHEADER;
                        pccrrSegListResponsePacket.MessageHeader = ret.MESSAGEHEADER;
                        packet = pccrrSegListResponsePacket;
                        break;
                }
            }

            return packet;
        }

        /// <summary>
        /// Decode MSG_BLKLIST message.
        /// </summary>
        /// <param name="data">The data to be decoded.</param>
        /// <param name="index">Started index.</param>
        /// <returns>The MSG_BLKLIST struct.</returns>
        private MSG_BLKLIST DecodeMSG_BLKLIST(byte[] data, ref int index)
        {
            MSG_BLKLIST ret = new MSG_BLKLIST();
            ret.SizeOfSegmentId = MarshalHelper.GetUInt32(data, ref index, false);
            ret.SegmentId = MarshalHelper.GetBytes(data, ref index, (int)ret.SizeOfSegmentId);
            ret.ZeroPad = MarshalHelper.GetBytes(data, ref index, (int)ret.SizeOfSegmentId % 4);
            if (ret.ZeroPad == null)
            {
                ret.ZeroPad = new byte[0];
            }

            ret.BlockRangeCount = MarshalHelper.GetUInt32(data, ref index, false);
            ret.BlockRanges = new BLOCK_RANGE[ret.BlockRangeCount];
            for (int i = 0; i < ret.BlockRangeCount; i++)
            {
                ret.BlockRanges[i].Index = MarshalHelper.GetUInt32(data, ref index, false);
                ret.BlockRanges[i].Count = MarshalHelper.GetUInt32(data, ref index, false);
            }

            ret.NextBlockIndex = MarshalHelper.GetUInt32(data, ref index, false);
            return ret;
        }

        /// <summary>
        /// Decode MSG_BLK message.
        /// </summary>
        /// <param name="data">Data to be decoded.</param>
        /// <param name="index">Started index.</param>
        /// <returns>The MSG_BLK struct.</returns>
        private MSG_BLK DecodeMSG_BLK(byte[] data, ref int index)
        {
            MSG_BLK ret = new MSG_BLK();
            ret.SizeOfSegmentId = MarshalHelper.GetUInt32(data, ref index, false);
            ret.SegmentId = MarshalHelper.GetBytes(data, ref index, (int)ret.SizeOfSegmentId);
            ret.ZeroPad = MarshalHelper.GetBytes(data, ref index, (int)ret.SizeOfSegmentId % 4);
            if (ret.ZeroPad == null)
            {
                ret.ZeroPad = new byte[0];
            }

            ret.BlockIndex = MarshalHelper.GetUInt32(data, ref index, false);
            ret.NextBlockIndex = MarshalHelper.GetUInt32(data, ref index, false);
            ret.SizeOfBlock = MarshalHelper.GetUInt32(data, ref index, false);
            ret.Block = MarshalHelper.GetBytes(data, ref index, (int)ret.SizeOfBlock);
            if (ret.Block == null)
            {
                ret.Block = new byte[0];
            }

            ret.ZeroPad2 = MarshalHelper.GetBytes(data, ref index, (int)ret.SizeOfBlock % 4);
            if (ret.ZeroPad2 == null)
            {
                ret.ZeroPad2 = new byte[0];
            }

            ret.SizeOfVrfBlock = MarshalHelper.GetUInt32(data, ref index, false);
            ret.VrfBlock = MarshalHelper.GetBytes(data, ref index, (int)ret.SizeOfVrfBlock);
            if (ret.VrfBlock == null)
            {
                ret.VrfBlock = new byte[0];
            }

            ret.ZeroPad3 = MarshalHelper.GetBytes(data, ref index, (int)ret.SizeOfVrfBlock % 4);
            if (ret.ZeroPad3 == null)
            {
                ret.ZeroPad3 = new byte[0];
            }

            ret.SizeOfIVBlock = MarshalHelper.GetUInt32(data, ref index, false);
            ret.IVBlock = MarshalHelper.GetBytes(data, ref index, (int)ret.SizeOfIVBlock);

            if (ret.IVBlock == null)
            {
                ret.IVBlock = new byte[0];
            }

            return ret;
        }

        /// <summary>
        /// Decode MSG_NEGO_RESP message.
        /// </summary>
        /// <param name="data">Data to be decoded.</param>
        /// <param name="index">Started index.</param>
        /// <returns>The MSG_NEGO_RESP struct.</returns>
        private MSG_NEGO_RESP DecodeMSG_NEGO_RESP(byte[] data, ref int index)
        {
            MSG_NEGO_RESP ret = new MSG_NEGO_RESP();
            ret.MinSupporteProtocolVersion.MinorVersion = MarshalHelper.GetUInt16(data, ref index, false);
            ret.MinSupporteProtocolVersion.MajorVersion = MarshalHelper.GetUInt16(data, ref index, false);
            ret.MaxSupporteProtocolVersion.MinorVersion = MarshalHelper.GetUInt16(data, ref index, false);
            ret.MaxSupporteProtocolVersion.MajorVersion = MarshalHelper.GetUInt16(data, ref index, false);
            return ret;
        }

        /// <summary>
        /// Decode message header.
        /// </summary>
        /// <param name="data">Data to be decoded.</param>
        /// <param name="index">Started index.</param>
        /// <returns>The MESSAGE_HEADER struct.</returns>
        private MESSAGE_HEADER DecodeMessageHeader(byte[] data, ref int index)
        {
            MESSAGE_HEADER ret = new MESSAGE_HEADER();
            ret.ProtVer.MinorVersion = MarshalHelper.GetUInt16(data, ref index, false);
            ret.ProtVer.MajorVersion = MarshalHelper.GetUInt16(data, ref index, false);
            ret.MsgType = (MsgType_Values)MarshalHelper.GetUInt32(data, ref index, false);
            ret.MsgSize = MarshalHelper.GetUInt32(data, ref index, false);
            ret.CryptoAlgoId = (CryptoAlgoId_Values)MarshalHelper.GetUInt32(data, ref index, false);
            return ret;
        }

        /// <summary>
        /// Decode MSG_GETBLKLIST message
        /// </summary>
        /// <param name="data">Data to be decoded.</param>
        /// <param name="index">Started index.</param>
        /// <returns>The MSG_GETBLKLIST struct.</returns>
        private MSG_GETBLKLIST DecodeMSG_GETBLKLIST(byte[] data, ref int index)
        {
            MSG_GETBLKLIST ret = new MSG_GETBLKLIST();

            ret.SizeOfSegmentID = MarshalHelper.GetUInt32(data, ref index, false);
            ret.SegmentID = MarshalHelper.GetBytes(data, ref index, (int)ret.SizeOfSegmentID);

            ret.ZeroPad = MarshalHelper.GetBytes(data, ref index, (int)ret.SizeOfSegmentID % 4);

            if (ret.ZeroPad == null)
            {
                ret.ZeroPad = new byte[0];
            }

            ret.NeededBlocksRangeCount = MarshalHelper.GetUInt32(data, ref index, false);
            ret.NeededBlockRanges = new BLOCK_RANGE[ret.NeededBlocksRangeCount];

            for (int i = 0; i < ret.NeededBlocksRangeCount; i++)
            {
                ret.NeededBlockRanges[i].Index = MarshalHelper.GetUInt32(data, ref index, false);
                ret.NeededBlockRanges[i].Count = MarshalHelper.GetUInt32(data, ref index, false);
            }

            return ret;
        }

        /// <summary>
        /// Decode MSG_GETBLKS message.
        /// </summary>
        /// <param name="data">Data to be decoded.</param>
        /// <param name="index">Started index.</param>
        /// <returns>The MSG_GETBLKS struct.</returns>
        private MSG_GETBLKS DecodeMSG_GETBLKS(byte[] data, ref int index)
        {
            MSG_GETBLKS ret = new MSG_GETBLKS();

            ret.SizeOfSegmentID = MarshalHelper.GetUInt32(data, ref index, false);
            ret.SegmentID = MarshalHelper.GetBytes(data, ref index, (int)ret.SizeOfSegmentID);
            ret.ZeroPad = MarshalHelper.GetBytes(data, ref index, (int)ret.SizeOfSegmentID % 4);

            if (ret.ZeroPad == null)
            {
                ret.ZeroPad = new byte[0];
            }

            ret.ReqBlockRangeCount = MarshalHelper.GetUInt32(data, ref index, false);
            ret.ReqBlockRanges = new BLOCK_RANGE[ret.ReqBlockRangeCount];

            for (int i = 0; i < ret.ReqBlockRangeCount; i++)
            {
                ret.ReqBlockRanges[i].Index = MarshalHelper.GetUInt32(data, ref index, false);
                ret.ReqBlockRanges[i].Count = MarshalHelper.GetUInt32(data, ref index, false);
            }

            ret.SizeOfDataForVrfBlock = MarshalHelper.GetUInt32(data, ref index, false);
            ret.DataForVrfBlock = MarshalHelper.GetBytes(data, ref index, (int)ret.SizeOfDataForVrfBlock);

            if (ret.DataForVrfBlock == null)
            {
                ret.DataForVrfBlock = new byte[0];
            }

            return ret;
        }

        /// <summary>
        /// Decode MSG_NEGO_REQ message.
        /// </summary>
        /// <param name="data">Data to be decoded.</param>
        /// <param name="index">Started index.</param>
        /// <returns>The MSG_NEGO_REQ struct.</returns>
        private MSG_NEGO_REQ DecodeMSG_NEGO_REQ(byte[] data, ref int index)
        {
            MSG_NEGO_REQ ret = new MSG_NEGO_REQ();
            ret.MinSupportedProtocolVersion.MinorVersion = MarshalHelper.GetUInt16(data, ref index, false);
            ret.MinSupportedProtocolVersion.MajorVersion = MarshalHelper.GetUInt16(data, ref index, false);
            ret.MaxSupportedProtocolVersion.MinorVersion = MarshalHelper.GetUInt16(data, ref index, false);
            ret.MaxSupportedProtocolVersion.MajorVersion = MarshalHelper.GetUInt16(data, ref index, false);

            return ret;
        }
    }
}

