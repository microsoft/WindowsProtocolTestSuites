// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrr
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using Microsoft.Protocols.TestTools.StackSdk.CommonStack;

    /// <summary>
    /// Receive pccrr message handler.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="pccrrPacket">The PccrrPacket.</param>
    public delegate void MessageArrivedEventArgs(IPEndPoint sender, PccrrPacket pccrrPacket);

    /// <summary>
    /// Pccrr server.
    /// </summary>
    public class PccrrServer : IDisposable
    {
        /// <summary>
        /// Identify the instance whether is disposed.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// The server port.
        /// </summary>
        private int serverPort;

        /// <summary>
        /// The pccrr path.
        /// </summary>
        private string url;

        /// <summary>
        /// The IP address type.
        /// </summary>
        private IPAddressType addressType;

        /// <summary>
        /// The lock obj.
        /// </summary>
        private object obj = new object();

        /// <summary>
        /// The http listener context.
        /// </summary>
        private HttpListenerContext httpListenerContext;

        /// <summary>
        /// The http server transport used for Pccrr.
        /// </summary>
        private HttpServerTransport httpServerTransport;

        /// <summary>
        /// Indicates an instance of ILogger.
        /// </summary>
        private ILogPrinter logger;

        /// <summary>
        /// The max size of the buffer.
        /// </summary>
        private int bufferSize = 1024 * 10;

        /// <summary>
        /// Initializes a new instance of the <see cref="PccrrServer"/> class with default settings.
        /// </summary>
        /// <param name="serverPort">The server port.</param>
        // TODO: Address type should be removed
        public PccrrServer(int serverPort)
            : this(serverPort, PccrrConsts.Url, IPAddressType.IPv4)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PccrrServer"/> class.
        /// </summary>
        /// <param name="serverPort">The server port.</param>
        /// <param name="pccrrPath">The pccrr path.</param>
        /// <param name="addressType">The IP address type.</param>
        public PccrrServer(
            int serverPort,
            string pccrrPath,
            IPAddressType addressType)
        {
            if (pccrrPath == null)
            {
                throw new ArgumentNullException("pccrrPath");
            }

            this.serverPort = serverPort;
            this.url = pccrrPath;
            this.addressType = addressType;

            this.httpServerTransport = new HttpServerTransport(TransferProtocol.HTTP, serverPort, addressType, pccrrPath);
            this.httpServerTransport.HttpRequestEventHandle += new EventHandler<HttpRequestEventArg>(this.HttpServerTransport_ReceiveFrom);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PccrrServer"/> class.
        /// </summary>
        /// <param name="serverPort">The server port.</param>
        /// <param name="pccrrPath">The pccrr path.</param>
        /// <param name="addressType">The IP address type.</param>
        /// <param name="logger">The specified logger.</param>
        public PccrrServer(
            int serverPort,
            string pccrrPath,
            IPAddressType addressType,
            ILogPrinter logger)
        {
            if (pccrrPath == null)
            {
                throw new ArgumentNullException("pccrrPath");
            }

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            this.serverPort = serverPort;
            this.url = pccrrPath;
            this.addressType = addressType;
            this.logger = logger;

            this.httpServerTransport = new HttpServerTransport(TransferProtocol.HTTP, serverPort, addressType, pccrrPath, logger);
            this.httpServerTransport.HttpRequestEventHandle += new EventHandler<HttpRequestEventArg>(this.HttpServerTransport_ReceiveFrom);
        }

        /// <summary>
        ///  Finalizes an instance of the <see cref="PccrrServer"/> class
        /// </summary>
        ~PccrrServer()
        {
            Dispose(false);
        }

        /// <summary>
        /// Receive pccrr message.
        /// </summary>
        public event MessageArrivedEventArgs MessageArrived;

        /// <summary>
        /// Gets or sets the pccrr server port.
        /// </summary>
        public int ServerPort
        {
            get
            {
                return this.serverPort;
            }

            set
            {
                this.serverPort = value;
            }
        }

        /// <summary>
        /// Gets or sets the pccrr server url.
        /// </summary>
        public string PccrrPath
        {
            get
            {
                return this.url;
            }

            set
            {
                this.url = value;
            }
        }

        /// <summary>
        /// Gets or sets the pccrr server ip address type.
        /// </summary>
        public IPAddressType IPAddressType
        {
            get
            {
                return this.addressType;
            }

            set
            {
                this.addressType = value;
            }
        }

        /// <summary>
        /// Start listening.
        /// </summary>
        public void StartListening()
        {
            if (this.httpServerTransport.RunState != State.Started)
            {
                this.httpServerTransport.StartHttpServer();

                if (this.logger != null)
                {
                    this.logger.AddInfo("The web service is started.");
                }

                return;
            }

            if (this.logger != null)
            {
                this.logger.AddInfo("The web service is already started.");
            }
        }

        /// <summary>
        /// Close connections.
        /// </summary>
        public void CloseConnections()
        {
            if (this.httpServerTransport.RunState != State.Stopped)
            {
                this.httpServerTransport.StopHttpServer();
            }
        }

        /// <summary>
        /// Create a MsgNego response.
        /// </summary>
        /// <param name="minSupportedProtocolVer">The minSupportedProtocolVersion.</param>
        /// <param name="maxSupportedProtocolVer">The maxSupportedProtocolVersion.</param>
        /// <param name="cryptoAlgoIdValues">The cryptoAlgoId.</param>
        /// <param name="msgTypeValues">The msgType.</param>
        /// <returns>The MsgNego response.</returns>
        public PccrrNegoResponsePacket CreateMsgNegoResponse(
            ProtoVersion minSupportedProtocolVer,
            ProtoVersion maxSupportedProtocolVer,
            CryptoAlgoId_Values cryptoAlgoIdValues,
            MsgType_Values msgTypeValues)
        {
            PccrrNegoResponsePacket packet = new PccrrNegoResponsePacket();

            MSG_NEGO_RESP msgNegoResp = new MSG_NEGO_RESP();
            msgNegoResp.MaxSupporteProtocolVersion = maxSupportedProtocolVer;
            msgNegoResp.MinSupporteProtocolVersion = minSupportedProtocolVer;
            MESSAGE_HEADER messageHeader = PccrrUtitlity.CreateMessageHeader(cryptoAlgoIdValues, msgTypeValues, new ProtoVersion { MajorVersion = 1, MinorVersion = 0 });
            packet.MsgNegoResp = msgNegoResp;
            packet.MessageHeader = messageHeader;

            return packet;
        }

        /// <summary>
        /// Create a MsgBlkList response.
        /// </summary>
        /// <param name="segmentId">The segmentId.</param>
        /// <param name="blockRanges">The blockRanges.</param>
        /// <param name="nextBlockIndex">The nextBlockIndex.</param>
        /// <param name="cryptoAlgoIdValues">The cryptoAlgoId.</param>
        /// <param name="msgTypeValues">The msgType.</param>
        /// <returns>The MsgBlkList response.</returns>
        public PccrrBLKLISTResponsePacket CreateMsgBlkListResponse(
            byte[] segmentId,
            BLOCK_RANGE[] blockRanges,
            uint nextBlockIndex,
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

            PccrrBLKLISTResponsePacket packet = new PccrrBLKLISTResponsePacket();

            MSG_BLKLIST msgBlkListResp = new MSG_BLKLIST();
            byte[] zeroPad = new byte[0] { };
            msgBlkListResp.ZeroPad = zeroPad;
            msgBlkListResp.SizeOfSegmentId = (uint)segmentId.Length;
            msgBlkListResp.SegmentId = segmentId;
            msgBlkListResp.BlockRanges = blockRanges;
            msgBlkListResp.BlockRangeCount = (uint)blockRanges.Length;
            msgBlkListResp.NextBlockIndex = nextBlockIndex;
            MESSAGE_HEADER messageHeader = PccrrUtitlity.CreateMessageHeader(cryptoAlgoIdValues, msgTypeValues, new ProtoVersion { MajorVersion = 1, MinorVersion = 0 });
            packet.MsgBLKLIST = msgBlkListResp;
            packet.MessageHeader = messageHeader;

            return packet;
        }

        /// <summary>
        /// Create a MsgBlk response.
        /// </summary>
        /// <param name="segmentId">The segmentId.</param>
        /// <param name="block">The block.</param>
        /// <param name="cryptoAlgoIdValues">The cryptoAlgoId.</param>
        /// <param name="msgTypeValues">The msgType.</param>
        /// <param name="iv">The initial vector.</param>
        /// <param name="blockIndex">Block index.</param>
        /// <param name="nextBlockIndex">Next available block index.</param>
        /// <returns>The MsgBlk response.</returns>
        public PccrrBLKResponsePacket CreateMsgBlkResponse(
            byte[] segmentId,
            byte[] block,
            CryptoAlgoId_Values cryptoAlgoIdValues,
            MsgType_Values msgTypeValues,
            byte[] iv,
            uint blockIndex,
            uint nextBlockIndex)
        {
            if (segmentId == null)
            {
                segmentId = new byte[0];
            }

            if (block == null)
            {
                block = new byte[0];
            }

            PccrrBLKResponsePacket packet = new PccrrBLKResponsePacket();

            MSG_BLK msgBlkResp = new MSG_BLK();
            byte[] zeroPad = new byte[0] { };
            byte[] zeroPad_2 = new byte[0] { };
            byte[] zeroPad_3 = new byte[0] { };
            msgBlkResp.ZeroPad3 = zeroPad_3;
            msgBlkResp.ZeroPad2 = zeroPad_2;
            msgBlkResp.ZeroPad = zeroPad;
            msgBlkResp.VrfBlock = null;
            msgBlkResp.SizeOfVrfBlock = 0;
            msgBlkResp.SizeOfSegmentId = (uint)segmentId.Length;
            msgBlkResp.SizeOfIVBlock = (uint)iv.Length;
            msgBlkResp.SizeOfBlock = (uint)block.Length;
            msgBlkResp.SegmentId = segmentId;
            msgBlkResp.IVBlock = iv;
            msgBlkResp.Block = block;
            msgBlkResp.BlockIndex = blockIndex;
            msgBlkResp.NextBlockIndex = nextBlockIndex;

            MESSAGE_HEADER messageHeader = PccrrUtitlity.CreateMessageHeader(cryptoAlgoIdValues, msgTypeValues, new ProtoVersion { MajorVersion = 1, MinorVersion = 0 });
            packet.MsgBLK = msgBlkResp;
            packet.MessageHeader = messageHeader;

            return packet;
        }

        /// <summary>
        /// Create a MsgBlk response.
        /// </summary>
        /// <param name="segmentId">The segmentId.</param>
        /// <param name="block">The block.</param>
        /// <param name="cryptoAlgoIdValues">The cryptoAlgoId.</param>
        /// <param name="msgTypeValues">The msgType.</param>
        /// <param name="isLastBLKAvail">If it is true, the block is the last available block.</param>
        /// <returns>The MsgBlk response.</returns>
        public PccrrBLKResponsePacket CreateMsgBlkResponse(
            byte[] segmentId,
            byte[] block,
            CryptoAlgoId_Values cryptoAlgoIdValues,
            MsgType_Values msgTypeValues,
            bool isLastBLKAvail)
        {
            if (segmentId == null)
            {
                segmentId = new byte[0];
            }

            if (block == null)
            {
                block = new byte[0];
            }

            PccrrBLKResponsePacket packet = new PccrrBLKResponsePacket();

            MSG_BLK msgBlkResp = new MSG_BLK();
            byte[] zeroPad = new byte[0] { };
            byte[] zeroPad_2 = new byte[0] { };
            byte[] zeroPad_3 = new byte[0] { };
            byte[] iVBlock = new byte[0] { };
            msgBlkResp.ZeroPad3 = zeroPad_3;
            msgBlkResp.ZeroPad2 = zeroPad_2;
            msgBlkResp.ZeroPad = zeroPad;
            msgBlkResp.VrfBlock = null;
            msgBlkResp.SizeOfVrfBlock = 0;
            msgBlkResp.SizeOfSegmentId = (uint)segmentId.Length;
            msgBlkResp.SizeOfIVBlock = (uint)iVBlock.Length;
            msgBlkResp.SizeOfBlock = (uint)block.Length;
            msgBlkResp.SegmentId = segmentId;
            msgBlkResp.IVBlock = iVBlock;
            msgBlkResp.Block = block;
            msgBlkResp.BlockIndex = 0;

            if (!isLastBLKAvail)
            {
                msgBlkResp.NextBlockIndex = 1;
            }
            else
            {
                msgBlkResp.NextBlockIndex = 0;
            }

            MESSAGE_HEADER messageHeader = PccrrUtitlity.CreateMessageHeader(cryptoAlgoIdValues, msgTypeValues, new ProtoVersion { MajorVersion = 1, MinorVersion = 0 });
            packet.MsgBLK = msgBlkResp;
            packet.MessageHeader = messageHeader;

            return packet;
        }

        /// <summary>
        /// Create a MsgSegList response.
        /// </summary>
        /// <param name="cryptoAlgoIdValues">The cryptoAlgoId.</param>
        /// <param name="requestID">Request ID.</param>
        /// <param name="segmentRanges">Segment ranges.</param>
        /// <returns>The MsgSegList response.</returns>
        public PccrrSegListResponsePacket CreateSegListResponse(
            CryptoAlgoId_Values cryptoAlgoIdValues,
            Guid requestID,
            BLOCK_RANGE[] segmentRanges)
        {
            var packet = new PccrrSegListResponsePacket();

            var msgSegList = new MSG_SEGLIST();
            msgSegList.RequestID = requestID;
            msgSegList.SegmentRangeCount = (uint)segmentRanges.Length;
            msgSegList.SegmentRanges = segmentRanges;

            MESSAGE_HEADER messageHeader = PccrrUtitlity.CreateMessageHeader(cryptoAlgoIdValues, MsgType_Values.MSG_SEGLIST, new ProtoVersion { MajorVersion = 2, MinorVersion = 0 });
            packet.MsgSegList = msgSegList;
            packet.MessageHeader = messageHeader;

            return packet;
        }


        /// <summary>
        /// Send a packet.
        /// </summary>
        /// <param name="packet">The packet need to be sent.</param>
        public void SendPacket(PccrrPacket packet)
        {
            if (packet == null)
            {
                throw new ArgumentNullException("packet");
            }

            if (this.httpServerTransport == null)
            {
                throw new InvalidOperationException("The transport is null for not connected.");
            }

            byte[] bytes = new byte[] { };
            switch (packet.PacketType)
            {
                case MsgType_Values.MSG_NEGO_RESP:
                    bytes = ((PccrrNegoResponsePacket)packet).Encode();
                    break;
                case MsgType_Values.MSG_BLKLIST:
                    bytes = ((PccrrBLKLISTResponsePacket)packet).Encode();
                    break;
                case MsgType_Values.MSG_BLK:
                    bytes = ((PccrrBLKResponsePacket)packet).Encode();
                    break;
            }

            //try
            //{
                this.httpServerTransport.Send(null, bytes, this.httpListenerContext);
            //}
            //catch (HttpListenerException ex)
            //{
            //    if (this.logger != null)
            //    {
            //        this.logger.AddDebug(
            //            string.Format(
            //            "Unexpected exception send packet failed: {0}",
            //            ex.Message));
            //    }

            //    throw;
            //}
        }

        /// <summary>
        /// Send bytes.
        /// </summary>
        /// <param name="bytes">The bytes need to be sent.</param>
        public void SendBytes(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }

            try
            {
                this.httpServerTransport.Send(null, bytes, this.httpListenerContext);
            }
            catch (HttpListenerException ex)
            {
                if (this.logger != null)
                {
                    this.logger.AddDebug(
                        string.Format(
                        "Unexpected exception send bytes failed: {0}",
                        ex.Message));
                }

                throw;
            }
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
                    if (this.httpServerTransport != null)
                    {
                        this.httpServerTransport.Dispose();
                        this.httpServerTransport = null;
                    }
                }

                this.disposed = true;
            }
        }

        #endregion

        /// <summary>
        /// Receive pccrr message.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e argu.</param>
        private void HttpServerTransport_ReceiveFrom(object sender, HttpRequestEventArg e)
        {
            lock (this)
            {
                this.httpListenerContext = e.ListenerContext;
                this.MessageArrived(
                    e.ListenerContext.Request.RemoteEndPoint,
                    this.DecodePacket(
                    e.ListenerContext.Request.Url,
                    (HttpMethod)Enum.Parse(
                    typeof(HttpMethod),
                    e.ListenerContext.Request.HttpMethod),
                    this.DecomposeHttpRequest(
                    e.ListenerContext.Request)));
            }
        }

        /// <summary>
        /// Decode a packet.
        /// </summary>
        /// <param name="uri">The uri of the request.</param>
        /// <param name="httpMethod">The http method.</param>
        /// <param name="rawdata">The rawdata.</param>
        /// <returns>The PccrrPacket.</returns>
        private PccrrPacket DecodePacket(Uri uri, HttpMethod httpMethod, byte[] rawdata)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            if (this.httpServerTransport == null)
            {
                throw new InvalidOperationException("The transport is null for not connected.");
            }

            PccrrPacket req = null;

            lock (this.obj)
            {
                req = new PccrrUtitlity().DecodeRequestMessage(rawdata, uri, httpMethod);

                return req;
            }
        }

        /// <summary>
        /// Gets the request body byte array from the http request.
        /// </summary>
        /// <param name="httpListenerRequest">The http request.</param>
        /// <returns>The array of byte.</returns>
        private byte[] DecomposeHttpRequest(HttpListenerRequest httpListenerRequest)
        {
            if (httpListenerRequest == null)
            {
                throw new ArgumentNullException("httpListenerRequest");
            }

            lock (this.obj)
            {
                List<byte[]> payloadList = new List<byte[]>();
                List<byte> tempBuffer = new List<byte>();
                Stream requestStream = null;
                try
                {
                    requestStream = httpListenerRequest.InputStream;

                    int readSize = 0;
                    byte[] payloadBuffer = null;
                    while (true)
                    {
                        int tempIndex = 0;
                        payloadBuffer = new byte[this.bufferSize];
                        readSize = requestStream.Read(payloadBuffer, 0, payloadBuffer.Length);

                        if (readSize == 0)
                        {
                            break;
                        }
                        else
                        {
                            tempBuffer.AddRange(MarshalHelper.GetBytes(payloadBuffer, ref tempIndex, readSize));
                        }
                    }
                }
                catch (IOException ex)
                {
                    if (this.logger != null)
                    {
                        this.logger.AddDebug(
                            string.Format(
                            "Unexpected exception receiving failed: {0}",
                            ex.Message));
                    }
                }
                finally
                {
                    if (requestStream != null)
                    {
                        requestStream.Close();
                    }
                }

                return tempBuffer.ToArray();
            }
        }
    }
}
