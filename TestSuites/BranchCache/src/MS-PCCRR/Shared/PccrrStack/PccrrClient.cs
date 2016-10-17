// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrr
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Microsoft.Protocols.TestTools.StackSdk.CommonStack;

    /// <summary>
    /// Pccrr client.
    /// </summary>
    public class PccrrClient : IDisposable
    {
        /// <summary>
        /// If disposed.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// The server name.
        /// </summary>
        private string serverName;

        /// <summary>
        /// The serverPort.
        /// </summary>
        private int serverPort;

        /// <summary>
        /// The request url.
        /// </summary>
        private string url;

        /// <summary>
        /// The request method.
        /// </summary>
        private HttpMethod method;

        /// <summary>
        /// The http transport used for sending HttpRequest and receiving HttpReponse for Pccrr.
        /// </summary>
        private HttpClientTransport httpClientTransport;

        /// <summary>
        /// Indicates an instance of ILogger.
        /// </summary>
        private ILogPrinter logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PccrrClient"/> class with default settings.
        /// </summary>
        /// <param name="server">The server name.</param>
        public PccrrClient(
            string server,
            int serverPort)
            : this(server, serverPort, PccrrConsts.Url, HttpMethod.POST)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PccrrClient"/> class.
        /// </summary>
        /// <param name="server">The server name.</param>
        /// <param name="serverPort">The serverPort.</param>
        /// <param name="pccrrPath">The HTTP request uri.</param>
        /// <param name="method">The HTTP request method.</param>
        public PccrrClient(
            string server,
            int serverPort,
            string pccrrPath,
            HttpMethod method)
        {
            if (server == null)
            {
                throw new ArgumentNullException("server");
            }

            if (pccrrPath == null)
            {
                throw new ArgumentNullException("pccrrPath");
            }

            this.serverName = server;
            this.serverPort = serverPort;
            this.url = pccrrPath;
            this.method = method;

            this.httpClientTransport = new HttpClientTransport(TransferProtocol.HTTP, server, serverPort, pccrrPath);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PccrrClient"/> class.
        /// </summary>
        /// <param name="server">The server name.</param>
        /// <param name="serverPort">The server port.</param>
        /// <param name="pccrrPath">The HTTP request uri.</param>
        /// <param name="method">The HTTP request method.</param>
        /// <param name="logger">The specified logger.</param>
        public PccrrClient(
            string server,
            int serverPort,
            string pccrrPath,
            HttpMethod method,
            ILogPrinter logger)
        {
            if (server == null)
            {
                throw new ArgumentNullException("server");
            }

            if (pccrrPath == null)
            {
                throw new ArgumentNullException("pccrrPath");
            }

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            this.serverName = server;
            this.serverPort = serverPort;
            this.url = pccrrPath;
            this.method = method;
            this.logger = logger;

            this.httpClientTransport = new HttpClientTransport(TransferProtocol.HTTP, server, serverPort, pccrrPath, logger);
        }

        /// <summary>
        ///  Finalizes an instance of the <see cref="PccrrClient"/> class
        /// </summary>
        ~PccrrClient()
        {
            Dispose(false);
        }

        /// <summary>
        /// Create a MsgNego request.
        /// </summary>
        /// <param name="minSupportedProtocolVer">The minSupportedProtocolVersion.</param>
        /// <param name="maxSupportedProtocolVer">The maxSupportedProtocolVersion.</param>
        /// <param name="cryptoAlgoIdValues">The cryptoAlgoId.</param>
        /// <param name="msgTypeValues">The msgType.</param>
        /// <returns>MsgNego request.</returns>
        public PccrrNegoRequestPacket CreateMsgNegoRequest(
            ProtoVersion minSupportedProtocolVer,
            ProtoVersion maxSupportedProtocolVer,
            CryptoAlgoId_Values cryptoAlgoIdValues,
            MsgType_Values msgTypeValues)
        {
            PccrrNegoRequestPacket packet = new PccrrNegoRequestPacket();

            MSG_NEGO_REQ msgNegoReq = new MSG_NEGO_REQ();
            msgNegoReq.MinSupportedProtocolVersion = minSupportedProtocolVer;
            msgNegoReq.MaxSupportedProtocolVersion = maxSupportedProtocolVer;
            MESSAGE_HEADER messageHeader = PccrrUtitlity.CreateMessageHeader(cryptoAlgoIdValues, msgTypeValues, new ProtoVersion { MajorVersion = 1, MinorVersion = 0 });
            packet.MsgNegoReq = msgNegoReq;
            packet.MessageHeader = messageHeader;

            return packet;
        }

        /// <summary>
        /// Create a MsgGetBlkList request.
        /// </summary>
        /// <param name="segmentId">The segmentId.</param>
        /// <param name="blockRanges">The needed BlockRanges.</param>
        /// <param name="cryptoAlgoIdValues">The cryptoAlgoId.</param>
        /// <param name="msgTypeValues">The msgType.</param>
        /// <returns>MsgGetBlkList request.</returns>
        public PccrrGETBLKLISTRequestPacket CreateMsgGetBlkListRequest(
            byte[] segmentId,
            BLOCK_RANGE[] blockRanges,
            CryptoAlgoId_Values cryptoAlgoIdValues,
            MsgType_Values msgTypeValues)
        {
            if (segmentId == null)
            {
                segmentId = new byte[0];
            }

            if (blockRanges == null)
            {
                blockRanges = new BLOCK_RANGE[0];
            }

            PccrrGETBLKLISTRequestPacket packet = new PccrrGETBLKLISTRequestPacket();

            MSG_GETBLKLIST msgGetBlkListReq = new MSG_GETBLKLIST();
            byte[] zeroPad = new byte[0] { };
            msgGetBlkListReq.SegmentID = segmentId;
            msgGetBlkListReq.SizeOfSegmentID = (uint)segmentId.Length;
            msgGetBlkListReq.NeededBlockRanges = blockRanges;
            msgGetBlkListReq.NeededBlocksRangeCount = (uint)blockRanges.Length;
            msgGetBlkListReq.ZeroPad = zeroPad;
            MESSAGE_HEADER messageHeader = PccrrUtitlity.CreateMessageHeader(cryptoAlgoIdValues, msgTypeValues, new ProtoVersion { MajorVersion = 1, MinorVersion = 0 });
            packet.MsgGetBLKLIST = msgGetBlkListReq;
            packet.MessageHeader = messageHeader;

            return packet;
        }

        /// <summary>
        /// Create a MsgGetBlks request.
        /// </summary>
        /// <param name="segmentId">The segmentId.</param>
        /// <param name="cryptoAlgoIdValues">The cryptoAlgoId.</param>
        /// <param name="msgTypeValues">The msgType.</param>
        /// <param name="blockIndex">The block index.</param>
        /// <param name="blockCount">The block count.</param>
        /// <returns>MsgGetBlks request.</returns>
        public PccrrGETBLKSRequestPacket CreateMsgGetBlksRequest(
            byte[] segmentId,
            CryptoAlgoId_Values cryptoAlgoIdValues,
            MsgType_Values msgTypeValues,
            uint blockIndex,
            uint blockCount)
        {
            if (segmentId == null)
            {
                segmentId = new byte[0];
            }

            PccrrGETBLKSRequestPacket packet = new PccrrGETBLKSRequestPacket();

            MSG_GETBLKS msgGetBlksReq = new MSG_GETBLKS();
            msgGetBlksReq.DataForVrfBlock = null;
            byte[] zeroPad = new byte[0] { };
            BLOCK_RANGE[] reqBlockRanges = new BLOCK_RANGE[1];
            reqBlockRanges[0].Index = blockIndex;
            reqBlockRanges[0].Count = blockCount;
            msgGetBlksReq.ReqBlockRanges = reqBlockRanges;
            msgGetBlksReq.ReqBlockRangeCount = (uint)reqBlockRanges.Length;
            msgGetBlksReq.ZeroPad = zeroPad;
            msgGetBlksReq.SizeOfSegmentID = (uint)segmentId.Length;
            msgGetBlksReq.SizeOfDataForVrfBlock = 0;
            msgGetBlksReq.SegmentID = segmentId;
            MESSAGE_HEADER messageHeader = PccrrUtitlity.CreateMessageHeader(cryptoAlgoIdValues, msgTypeValues, new ProtoVersion { MajorVersion = 1, MinorVersion = 0 });
            packet.MsgGetBLKS = msgGetBlksReq;
            packet.MessageHeader = messageHeader;

            return packet;
        }

        /// <summary>
        /// Create a MsgGetBlks request.
        /// </summary>
        /// <param name="segmentId">The segmentId.</param>
        /// <param name="cryptoAlgoIdValues">The cryptoAlgoId.</param>
        /// <param name="msgTypeValues">The msgType.</param>
        /// <returns>MsgGetBlks request.</returns>
        public PccrrGETBLKSRequestPacket CreateMsgGetBlksRequest(
            byte[] segmentId,
            CryptoAlgoId_Values cryptoAlgoIdValues,
            MsgType_Values msgTypeValues)
        {
            if (segmentId == null)
            {
                segmentId = new byte[0];
            }

            PccrrGETBLKSRequestPacket packet = new PccrrGETBLKSRequestPacket();

            MSG_GETBLKS msgGetBlksReq = new MSG_GETBLKS();
            msgGetBlksReq.DataForVrfBlock = null;
            byte[] zeroPad = new byte[0] { };
            BLOCK_RANGE[] reqBlockRanges = new BLOCK_RANGE[1];
            reqBlockRanges[0].Index = 0;
            reqBlockRanges[0].Count = 1;
            msgGetBlksReq.ReqBlockRanges = reqBlockRanges;
            msgGetBlksReq.ReqBlockRangeCount = (uint)reqBlockRanges.Length;
            msgGetBlksReq.ZeroPad = zeroPad;
            msgGetBlksReq.SizeOfSegmentID = (uint)segmentId.Length;
            msgGetBlksReq.SizeOfDataForVrfBlock = 0;
            msgGetBlksReq.SegmentID = segmentId;
            MESSAGE_HEADER messageHeader = PccrrUtitlity.CreateMessageHeader(cryptoAlgoIdValues, msgTypeValues, new ProtoVersion { MajorVersion = 1, MinorVersion = 0 });
            packet.MsgGetBLKS = msgGetBlksReq;
            packet.MessageHeader = messageHeader;

            return packet;
        }

        /// <summary>
        /// Create a MsgGetSegList request.
        /// </summary>
        /// <param name="cryptoAlgoIdValues">The cryptoAlgoId.</param>
        /// <param name="requestID">Request ID.</param>
        /// <param name="segmentIDs">Array of segment IDs.</param>
        /// <returns>MsgGetSegList request.</returns>
        public PccrrGetSegListRequestPacket CreateMsgGetSegListRequest(
            CryptoAlgoId_Values cryptoAlgoIdValues,
            Guid requestID,
            byte[][] segmentIDs)
        {
            return CreateMsgGetSegListRequest(cryptoAlgoIdValues, requestID, segmentIDs, new byte[0]);
        }

        /// <summary>
        /// Create a MsgGetSegList request.
        /// </summary>
        /// <param name="cryptoAlgoIdValues">The cryptoAlgoId.</param>
        /// <param name="requestID">Request ID.</param>
        /// <param name="segmentIDs">Array of segment IDs.</param>
        /// <param name="extensibleBlob">Extensible blob.</param>
        /// <returns>MsgGetSegList request.</returns>
        public PccrrGetSegListRequestPacket CreateMsgGetSegListRequest(
            CryptoAlgoId_Values cryptoAlgoIdValues,
            Guid requestID,
            byte[][] segmentIDs,
            byte[] extensibleBlob)
        {
            var packet = new PccrrGetSegListRequestPacket();

            var msgGetSegList = new MSG_GETSEGLIST();
            msgGetSegList.RequestID = requestID;
            msgGetSegList.CountOfSegmentIDs = (uint)segmentIDs.Length;
            msgGetSegList.SegmentIDs = new SegmentIDStructure[segmentIDs.Length];
            for (int i = 0; i < segmentIDs.Length; i++)
            {
                msgGetSegList.SegmentIDs[i] = new SegmentIDStructure(segmentIDs[i]);
            }
            ///[MS-PCCRR]Section 2.2.4.4 SizeOfExtensibleBlob: Size, in bytes, of the ExtensibleBlob field. Implementations MAY support extensible blobs in MSG_GETSEGLIST
            ///message.Implementations that do not support extensible blobs in MSG_GETSEGLIST messages MUST set SizeOfExtensibleBlob to zero and omit the ExtensibleBlob field.
            msgGetSegList.SizeOfExtensibleBlob = (uint)extensibleBlob.Length;
            msgGetSegList.ExtensibleBlob = extensibleBlob;

            packet.MsgGetSegList = msgGetSegList;
            MESSAGE_HEADER messageHeader = PccrrUtitlity.CreateMessageHeader(cryptoAlgoIdValues, MsgType_Values.MSG_GETSEGLIST, new ProtoVersion { MajorVersion = 2, MinorVersion = 0 });
            messageHeader.MsgSize += (uint)TypeMarshal.GetBlockMemorySize(msgGetSegList);
            packet.MessageHeader = messageHeader;

            return packet;
        }

        /// <summary>
        /// Send a packet.
        /// </summary>
        /// <param name="packet">The packet need to be sent.</param>
        /// <param name="timeout">The timeout.</param>
        public void SendPacket(PccrrPacket packet, TimeSpan timeout)
        {
            if (packet == null)
            {
                throw new ArgumentNullException("packet");
            }

            if (timeout == null)
            {
                throw new ArgumentNullException("timeout");
            }

            if (this.httpClientTransport == null)
            {
                throw new InvalidOperationException("The transport is null for not connected.");
            }

            byte[] bytes = new byte[] { };
            switch (packet.PacketType)
            {
                case MsgType_Values.MSG_NEGO_REQ:
                    bytes = ((PccrrNegoRequestPacket)packet).Encode();
                    break;
                case MsgType_Values.MSG_GETBLKLIST:
                    bytes = ((PccrrGETBLKLISTRequestPacket)packet).Encode();
                    break;
                case MsgType_Values.MSG_GETBLKS:
                    bytes = ((PccrrGETBLKSRequestPacket)packet).Encode();
                    break;
                case MsgType_Values.MSG_GETSEGLIST:
                    bytes = ((PccrrGetSegListRequestPacket)packet).Encode();
                    break;
                default:
                    throw new InvalidOperationException(
                        string.Format(
                        "The packet type {0} is not supported on client side.",
                        packet.PacketType));
            }

            try
            {
                this.httpClientTransport.Send(HttpVersion.Version11, null, bytes, CommonStack.HttpMethod.POST, (int)timeout.TotalMilliseconds);
            }
            catch (WebException ex)
            {
                if (this.logger != null)
                {
                    this.logger.AddDebug(
                        string.Format(
                        "Unexpected exception send packet failed: {0}",
                        ex.Message));
                }
            }
            catch (ObjectDisposedException ex)
            {
                if (this.logger != null)
                {
                    this.logger.AddDebug(
                        string.Format(
                        "Unexpected exception send packet failed: {0}",
                        ex.Message));
                }
            }
        }

        /// <summary>
        /// Send bytes.
        /// </summary>
        /// <param name="bytes">The bytes need to be sent.</param>
        /// <param name="timeout">The timeout.</param>
        public void SendBytes(byte[] bytes, TimeSpan timeout)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }

            if (timeout == null)
            {
                throw new ArgumentNullException("timeout");
            }

            try
            {
                this.httpClientTransport.Send(HttpVersion.Version11, null, bytes, CommonStack.HttpMethod.POST, (int)timeout.TotalMilliseconds);
            }
            catch (WebException ex)
            {
                if (this.logger != null)
                {
                    this.logger.AddDebug(
                        string.Format(
                        "Unexpected exception send bytes failed: {0}",
                        ex.Message));
                }
            }
            catch (ObjectDisposedException ex)
            {
                if (this.logger != null)
                {
                    this.logger.AddDebug(
                        string.Format(
                        "Unexpected exception send bytes failed: {0}",
                        ex.Message));
                }
            }
        }

        /// <summary>
        /// Expect a packet.
        /// </summary>
        /// <returns>The PccrrPacket.</returns>
        public PccrrPacket ExpectPacket()
        {
            if (this.httpClientTransport == null)
            {
                throw new InvalidOperationException("The transport is null for not connected.");
            }

            byte[] payloadBuffer = null;

            this.httpClientTransport.Receive(ref payloadBuffer);

            PccrrPacket resp = null;

            resp = new PccrrUtitlity().DecodeResponseMessage(payloadBuffer);

            return resp;
        }

        /// <summary>
        /// Close connections.
        /// </summary>
        public void CloseConnections()
        {
            this.httpClientTransport.Dispose();
        }

        #region IDisposable

        /// <summary>
        /// Release resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Release resources.
        /// </summary>
        /// <param name="disposing">If disposing equals true, Managed and unmanaged resources are disposed.
        /// if false, Only unmanaged resources can be disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.httpClientTransport != null)
                    {
                        this.httpClientTransport.Dispose();
                        this.httpClientTransport = null;
                    }
                }

                this.disposed = true;
            }
        }

        #endregion
    }
}
