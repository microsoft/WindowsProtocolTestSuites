// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.Messages;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrd;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrc;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrr;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc;
using Microsoft.Protocols.TestTools.StackSdk.CommonStack;
using Microsoft.Protocols.TestTools.StackSdk.CommonStack.Enum;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestSuites.BranchCache.TestSuite;

namespace Microsoft.Protocols.TestSuites.BranchCache.HostedCacheServer
{
    #region Abstract PCCRR Test Server

    abstract class PccrrTestServer : IDisposable
    {
        protected PccrrServer pccrrServer;
        protected CryptoAlgoId_Values cryptoAlgoId;
        protected ProtoVersion protoVersion;
        protected byte[] content;

        protected EventQueue eventQueue;

        protected bool disposed;

        protected Aes aes;
        protected byte[] iv;

        protected abstract ProtoVersion MaxSupportedVersion
        {
            get;
        }

        public void Start(int port, CryptoAlgoId_Values cryptoAlgoId, byte[] content, EventQueue eventQueue)
        {
            this.cryptoAlgoId = cryptoAlgoId;
            this.content = content;
            this.eventQueue = eventQueue;
            this.aes = PccrrUtitlity.CreateAes(cryptoAlgoId);
            this.iv = new byte[16];
            for (int i = 0; i < iv.Length; i++)
            {
                this.iv[i] = (byte)i;
            }

            pccrrServer = new PccrrServer(port);

            pccrrServer.MessageArrived += new MessageArrivedEventArgs(pccrrServer_MessageArrived);

            pccrrServer.StartListening();
        }

        public void Start(int port, CryptoAlgoId_Values cryptoAlgoId, ProtoVersion protoVersion, byte[] content, EventQueue eventQueue)
        {
            this.cryptoAlgoId = cryptoAlgoId;
            this.protoVersion = protoVersion;
            this.content = content;
            this.eventQueue = eventQueue;
            this.aes = PccrrUtitlity.CreateAes(cryptoAlgoId);
            this.iv = new byte[16];
            for (int i = 0; i < iv.Length; i++)
            {
                this.iv[i] = (byte)i;
            }

            pccrrServer = new PccrrServer(port);

            pccrrServer.MessageArrived += new MessageArrivedEventArgs(pccrrServer_MessageArrived);

            pccrrServer.StartListening();
        }

        void pccrrServer_MessageArrived(System.Net.IPEndPoint sender, PccrrPacket pccrrPacket)
        {
            var pccrrNegoRequest = pccrrPacket as PccrrNegoRequestPacket;

            if (pccrrNegoRequest != null)
            {
                HandlePccrrNegoRequestPacket(pccrrNegoRequest);

                goto logevent;
            }

            var pccrrGetBlkListRequest = pccrrPacket as PccrrGETBLKLISTRequestPacket;

            if (pccrrGetBlkListRequest != null)
            {
                HandlePccrrGETBLKLISTRequestPacket(pccrrGetBlkListRequest);

                goto logevent;
            }

            var pccrrGetBlkRequest = pccrrPacket as PccrrGETBLKSRequestPacket;

            if (pccrrGetBlkRequest != null)
            {
                HandlePccrrGETBLKSRequestPacket(pccrrGetBlkRequest);

                goto logevent;
            }

            var pccrrGetSegListRequest = pccrrPacket as PccrrGetSegListRequestPacket;

            if (pccrrGetSegListRequest != null)
            {
                HandlePccrrGetSegListRequestPacket(pccrrGetSegListRequest);

                goto logevent;
            }

            // Unknown packet
            throw new NotImplementedException("Unknown PCCRR message type " + pccrrPacket.PacketType);

        logevent:
            eventQueue.LogEvent(typeof(PccrrServer).GetEvent("MessageArrived"), sender, pccrrPacket);
        }

        protected virtual void HandlePccrrNegoRequestPacket(PccrrNegoRequestPacket pccrrNegoRequest)
        {
            var pccrrNegoResponse = pccrrServer.CreateMsgNegoResponse(
                    protoVersion,
                    protoVersion,
                    cryptoAlgoId,
                    MsgType_Values.MSG_NEGO_RESP);
            pccrrServer.SendPacket(pccrrNegoResponse);
        }

        protected virtual void HandlePccrrGETBLKLISTRequestPacket(PccrrGETBLKLISTRequestPacket pccrrGetBlkListRequest)
        {
            var pccrrBLKLISTResponsePacket = pccrrServer.CreateMsgBlkListResponse(
                    pccrrGetBlkListRequest.MsgGetBLKLIST.SegmentID,
                    pccrrGetBlkListRequest.MsgGetBLKLIST.NeededBlockRanges,
                    0,
                    cryptoAlgoId,
                    MsgType_Values.MSG_BLKLIST);
            pccrrServer.SendPacket(pccrrBLKLISTResponsePacket);
        }

        protected abstract void HandlePccrrGETBLKSRequestPacket(PccrrGETBLKSRequestPacket pccrrGetBlkRequest);

        protected abstract void HandlePccrrGetSegListRequestPacket(PccrrGetSegListRequestPacket pccrrGetSegListRequest);

        #region Dispose

        ~PccrrTestServer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (pccrrServer != null)
                    {
                        pccrrServer.Dispose();
                    }
                }
            }

            disposed = true;
        }

        #endregion
    }

    #endregion

    #region PCCRR Test Server V1

    class PccrrTestServerV1 : PccrrTestServer
    {
        private Content_Information_Data_Structure contentInformation;

        protected override ProtoVersion MaxSupportedVersion
        {
            get { return new ProtoVersion { MajorVersion = 1, MinorVersion = ushort.MaxValue }; }
        }

        public void Start(int port, CryptoAlgoId_Values cryptoAlgoId, Content_Information_Data_Structure contentInformation, byte[] content, EventQueue eventQueue)
        {
            this.contentInformation = contentInformation;

            base.Start(port, cryptoAlgoId, content, eventQueue);
        }

        public void Start(int port, CryptoAlgoId_Values cryptoAlgoId, ProtoVersion protoVersion, Content_Information_Data_Structure contentInformation, byte[] content, EventQueue eventQueue)
        {
            this.contentInformation = contentInformation;

            base.Start(port, cryptoAlgoId, protoVersion, content, eventQueue);
        }

        protected override void HandlePccrrGETBLKSRequestPacket(PccrrGETBLKSRequestPacket pccrrGetBlkRequest)
        {
            int segmentIndex = -1;
            for (int i = 0; i < contentInformation.cSegments; i++)
            {
                if (Enumerable.SequenceEqual(contentInformation.GetSegmentId(i), pccrrGetBlkRequest.MsgGetBLKS.SegmentID))
                {
                    segmentIndex = i;
                    break;
                }
            }

            PccrrBLKResponsePacket pccrrBlocksResponse;
            if (segmentIndex == -1) // Mached segement not found
            {
                pccrrBlocksResponse = pccrrServer.CreateMsgBlkResponse(
                    pccrrGetBlkRequest.MsgGetBLKS.SegmentID,
                    new byte[0],
                    cryptoAlgoId,
                    MsgType_Values.MSG_BLK,
                    this.iv,
                    0,
                    0);
                pccrrBlocksResponse = pccrrServer.CreateMsgBlkResponse(
                    pccrrGetBlkRequest.MsgGetBLKS.SegmentID,
                    new byte[0],
                    cryptoAlgoId,
                    MsgType_Values.MSG_BLK,
                    iv,
                    0,
                    0);
            }
            else
            {
                ulong blockSize = PccrcConsts.V1BlockSize;
                byte[] block = content.Skip((int)(contentInformation.segments[segmentIndex].ullOffsetInContent + pccrrGetBlkRequest.MsgGetBLKS.ReqBlockRanges[0].Index * blockSize)).Take((int)blockSize).ToArray();

                if (cryptoAlgoId != CryptoAlgoId_Values.NoEncryption)
                    block = PccrrUtitlity.Encrypt(aes, block, contentInformation.segments[segmentIndex].SegmentSecret, iv);

                pccrrBlocksResponse = pccrrServer.CreateMsgBlkResponse(
                    pccrrGetBlkRequest.MsgGetBLKS.SegmentID,
                    block,
                    cryptoAlgoId,
                    MsgType_Values.MSG_BLK,
                    this.iv,
                    pccrrGetBlkRequest.MsgGetBLKS.ReqBlockRanges[0].Index,
                    pccrrGetBlkRequest.MsgGetBLKS.ReqBlockRanges[0].Index == contentInformation.cSegments - 1 ? 0 : pccrrGetBlkRequest.MsgGetBLKS.ReqBlockRanges[0].Index + 1);
            }
            pccrrServer.SendPacket(pccrrBlocksResponse);
        }

        protected override void HandlePccrrGetSegListRequestPacket(PccrrGetSegListRequestPacket pccrrGetSegListRequest)
        {
            // Not supported in v1
            throw new NotImplementedException();
        }
    }

    #endregion

    #region PCCRR Test Server V2

    class PccrrTestServerV2 : PccrrTestServer
    {
        private Content_Information_Data_Structure_V2 contentInformationV2;

        protected override ProtoVersion MaxSupportedVersion
        {
            get { return new ProtoVersion { MajorVersion = 2, MinorVersion = ushort.MaxValue }; }
        }

        public void Start(int port, CryptoAlgoId_Values cryptoAlgoId, Content_Information_Data_Structure_V2 contentInformationV2, byte[] content, EventQueue eventQueue)
        {
            this.contentInformationV2 = contentInformationV2;

            base.Start(port, cryptoAlgoId, content, eventQueue);
        }

        protected override void HandlePccrrGETBLKSRequestPacket(PccrrGETBLKSRequestPacket pccrrGetBlkRequest)
        {
            uint offset = 0;
            int chunkIndex = -1;
            int segmentIndex = -1;
            for (int i = 0; i < contentInformationV2.chunks.Length; i++)
            {
                var chunk = contentInformationV2.chunks[i];
                for (int j = 0; j < chunk.chunkData.Length; j++)
                {
                    if (Enumerable.SequenceEqual(contentInformationV2.GetSegmentId(i, j), pccrrGetBlkRequest.MsgGetBLKS.SegmentID))
                    {
                        chunkIndex = i;
                        segmentIndex = j;
                        break;
                    }
                    else
                    {
                        offset += chunk.chunkData[j].cbSegment;
                    }
                }
            }
            PccrrBLKResponsePacket pccrrBlocksResponse;
            if (segmentIndex == -1) // Mached segement not found
            {
                pccrrBlocksResponse = pccrrServer.CreateMsgBlkResponse(
                    pccrrGetBlkRequest.MsgGetBLKS.SegmentID,
                    new byte[0],
                    cryptoAlgoId,
                    MsgType_Values.MSG_BLK,
                    iv,
                    0,
                    0);
            }
            else
            {
                var segment = contentInformationV2.chunks[chunkIndex].chunkData[segmentIndex];

                byte[] block = content.Skip((int)offset).Take((int)segment.cbSegment).ToArray();

                if (cryptoAlgoId != CryptoAlgoId_Values.NoEncryption)
                    block = PccrrUtitlity.Encrypt(aes, block, contentInformationV2.chunks[chunkIndex].chunkData[segmentIndex].SegmentSecret, iv);

                pccrrBlocksResponse = pccrrServer.CreateMsgBlkResponse(
                    pccrrGetBlkRequest.MsgGetBLKS.SegmentID,
                    block,
                    cryptoAlgoId,
                    MsgType_Values.MSG_BLK,
                    iv,
                    0,
                    0);
            }
            pccrrServer.SendPacket(pccrrBlocksResponse);
        }

        protected override void HandlePccrrGetSegListRequestPacket(PccrrGetSegListRequestPacket pccrrGetSegListRequest)
        {
            var pccrrSegListResponsePacket = pccrrServer.CreateSegListResponse(
                    cryptoAlgoId,
                    pccrrGetSegListRequest.MsgGetSegList.RequestID,
                    new BLOCK_RANGE[] { new BLOCK_RANGE { Index = 0, Count = pccrrGetSegListRequest.MsgGetSegList.CountOfSegmentIDs } });
            pccrrServer.SendPacket(pccrrSegListResponsePacket);
        }
    }

    #endregion
}
