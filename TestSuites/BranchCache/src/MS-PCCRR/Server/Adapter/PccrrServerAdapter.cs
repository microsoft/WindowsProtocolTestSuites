// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pccrr
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.Protocol.TestSuites;
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrr;
    using Microsoft.Protocols.TestTools.StackSdk.CommonStack;

    /// <summary>
    /// This class is used to provide internal function of sending or receiving message on client.
    /// </summary>
    public partial class PccrrServerAdapter : ManagedAdapterBase, IPccrrServerAdapter
    {
        #region Variables

        /// <summary>
        /// The pccrr uri.
        /// </summary>
        public const string PccrrUri = "116B50EB-ECE2-41ac-8429-9F9E963361B7/";

        /// <summary>
        /// operation system is windows or not
        /// </summary>
        private bool isWindows;

        /// <summary>
        /// The variable is set in HttpTransport_Receive and HttpTransport_ReceiveFrom methods when
        /// receive a http response. It's the length of http content body. 
        /// </summary>
        private int uiPayload;

        /// <summary>
        /// Is distributed mode.
        /// </summary>
        private bool isDistributedMode;

        /// <summary>
        /// Indicates the stack of Pccrr.
        /// </summary>
        private PccrrClient pccrrStack;

        /// <summary>
        /// server name
        /// </summary>
        private string serverName;

        /// <summary>
        /// The port of pccrr
        /// </summary>
        private int port;

        /// <summary>
        /// The timeout
        /// </summary>
        private int timeout;

        /// <summary>
        /// The protoVer
        /// </summary>
        private ProtoVersion protoVer;

        /// <summary>
        /// The error protoVer
        /// </summary>
        private ProtoVersion protoErrorVer;

        /// <summary>
        /// The minSupV
        /// </summary>
        private ProtoVersion minSupV;

        /// <summary>
        /// The maxSupV
        /// </summary>
        private ProtoVersion maxSupV;

        /// <summary>
        /// cryptoAlgorithm used in sending or receiving message
        /// </summary>
        private CryptoAlgoId_Values cryptoAlgo = CryptoAlgoId_Values.V1;

        #endregion

        #region Implementations for methods defined in interface

        /// <summary>
        /// Receive message MSG_NEGO_RESP
        /// </summary>
        public event ReceiveMsgNegoRespHandler ReceiveMsgNegoResp;

        /// <summary>
        /// Receive message MSG_BLKLIST
        /// </summary>
        public event ReceiveMsgBlkListHandler ReceiveMsgBlkList;

        /// <summary>
        /// Receive message MSG_BLK
        /// </summary>
        public event ReceiveMsgBlkHandler ReceiveMsgBlk;

        /// <summary>
        /// Send message MSG_NEGO_REQ
        /// </summary>
        public void SendMsgNegoReq()
        {
            PccrrNegoRequestPacket packet = this.pccrrStack.CreateMsgNegoRequest(this.minSupV, this.maxSupV, this.cryptoAlgo, MsgType_Values.MSG_NEGO_REQ, this.protoVer);

            this.pccrrStack.SendPacket(packet, new TimeSpan(0, 0, this.timeout));

            PccrrPacket respMSG = this.pccrrStack.ExpectPacket();

            PccrrNegoResponsePacket pccrrNegoResponsePacket = respMSG as PccrrNegoResponsePacket;

            MSG_NEGO_RESP msgNegoResp = pccrrNegoResponsePacket.MsgNegoResp;
            MESSAGE_HEADER messageHeader = pccrrNegoResponsePacket.MessageHeader;
            TRANSPORT_RESPONSE_HEADER transportRespH = pccrrNegoResponsePacket.TransportResponseHeader;
            RESPONSE_MESSAGE respMessage = new RESPONSE_MESSAGE();
            respMessage.MESSAGEBODY = msgNegoResp;
            respMessage.MESSAGEHEADER = messageHeader;
            respMessage.TRANSPORTRESPONSEHEADER = transportRespH;
            this.uiPayload = Marshal.SizeOf(messageHeader) + Marshal.SizeOf(msgNegoResp);

            this.VerifyHttpResponse();
            PccrrBothRoleCaptureCode.CaptureHttpRequirements();
            this.VerifyMsgNegoResp(msgNegoResp);
            this.VerifyMessageHeader(messageHeader);
            PccrrBothRoleCaptureCode.CaptureMessageHeaderRequirements(messageHeader, this.uiPayload);
            this.VerifyTransportResponseHeader(transportRespH);
            PccrrBothRoleCaptureCode.CaptureMessageRequirements();
            this.VerifyResponseMessage(respMessage);

            this.ReceiveMsgNegoResp();
        }

        /// <summary>
        /// Send message MSG_GETBLKLIST.
        /// </summary>
        /// <param name="sid">segment id.</param>
        /// <param name="blockRang">Block ranges client wants to get.</param>
        /// <param name="isVersionSupported">The version in message is supported by server or not.</param>
        public void SendMsgGetBlkList(byte[] sid, BLOCKRANGE[] blockRang, bool isVersionSupported)
        {
            PccrrGETBLKLISTRequestPacket packet;

            BLOCK_RANGE[] blockRanges = Helper.ConvertToStackBLOCKRANGEArray(blockRang);

            if (!isVersionSupported)
            {
                packet = this.pccrrStack.CreateMsgGetBlkListRequest(sid, blockRanges, this.cryptoAlgo, MsgType_Values.MSG_GETBLKLIST, this.protoErrorVer);
            }
            else
            {
                packet = this.pccrrStack.CreateMsgGetBlkListRequest(sid, blockRanges, this.cryptoAlgo, MsgType_Values.MSG_GETBLKLIST, this.protoVer);
            }

            this.pccrrStack.SendPacket(packet, new TimeSpan(0, 0, this.timeout));

            PccrrPacket respMSG = this.pccrrStack.ExpectPacket();

            if (!isVersionSupported)
            {
                PccrrNegoResponsePacket pccrrNegoResponsePacket = respMSG as PccrrNegoResponsePacket;

                if (pccrrNegoResponsePacket != null)
                {
                    this.ReceiveMsgNegoResp();
                }
            }
            else
            {
                PccrrBLKLISTResponsePacket pccrrBLKLISTResponsePacket = respMSG as PccrrBLKLISTResponsePacket;

                MSG_BLKLIST msgBLKLIST = pccrrBLKLISTResponsePacket.MsgBLKLIST;
                MESSAGE_HEADER messageHeader = pccrrBLKLISTResponsePacket.MessageHeader;
                TRANSPORT_RESPONSE_HEADER transportRespH = pccrrBLKLISTResponsePacket.TransportResponseHeader;
                RESPONSE_MESSAGE respMessage = new RESPONSE_MESSAGE();
                respMessage.MESSAGEBODY = msgBLKLIST;
                respMessage.MESSAGEHEADER = messageHeader;
                respMessage.TRANSPORTRESPONSEHEADER = transportRespH;

                if (msgBLKLIST.BlockRanges.Length != 0)
                {
                    BLOCK_RANGE blockRange = pccrrBLKLISTResponsePacket.MsgBLKLIST.BlockRanges[0];
                    PccrrBothRoleCaptureCode.CaptureBlockRangeRequirements(blockRange);
                }

                this.VerifyMsgBlkList(msgBLKLIST);
                PccrrBothRoleCaptureCode.CaptureCommonDataTypesRequirements(msgBLKLIST);
                this.VerifyResponseMessage(respMessage);

                BLOCKRANGE[] blkRanges = Helper.ConvertFromStackBLOCKRANGEArray(msgBLKLIST.BlockRanges);
                this.ReceiveMsgBlkList(msgBLKLIST.BlockRangeCount, blkRanges, msgBLKLIST.NextBlockIndex);
            }
        }

        /// <summary>
        /// Send message MSG_GETBLKS
        /// </summary>
        /// <param name="sid">segment id.</param>
        /// <param name="blockRang">Block ranges client wants to get</param>
        /// <param name="isVersionSupported">The version in this message is supported in server.</param>
        public void SendMsgGetBlks(byte[] sid, BLOCKRANGE[] blockRang, bool isVersionSupported)
        {
            PccrrGETBLKSRequestPacket packet;

            BLOCK_RANGE[] blockRanges = Helper.ConvertToStackBLOCKRANGEArray(blockRang);

            if (!isVersionSupported)
            {
                packet = this.pccrrStack.CreateMsgGetBlksRequest(sid, this.cryptoAlgo, MsgType_Values.MSG_GETBLKS, this.protoErrorVer);
            }
            else
            {
                packet = this.pccrrStack.CreateMsgGetBlksRequest(sid, this.cryptoAlgo, MsgType_Values.MSG_GETBLKS, this.protoVer);
            }

            for (int i = 0; i < blockRanges.Length; i++)
            {
                if (blockRanges.Length == 1)
                {
                    packet.MsgGetBLKS.ReqBlockRanges[i].Index = blockRanges[i].Index;
                    packet.MsgGetBLKS.ReqBlockRanges[i].Count = blockRanges[i].Count;
                }
                else
                {
                    MSG_GETBLKS msgGETBLKS = packet.MsgGetBLKS;
                    msgGETBLKS.ReqBlockRanges = new BLOCK_RANGE[blockRanges.Length];
                    msgGETBLKS.ReqBlockRangeCount = (uint)blockRanges.Length;
                    packet.MsgGetBLKS = msgGETBLKS;

                    packet.MsgGetBLKS.ReqBlockRanges[i].Index = blockRanges[i].Index;
                    packet.MsgGetBLKS.ReqBlockRanges[i].Count = blockRanges[i].Count;
                }
            }

            this.pccrrStack.SendPacket(packet, new TimeSpan(0, 0, this.timeout));

            PccrrPacket respMSG = this.pccrrStack.ExpectPacket();

            if (!isVersionSupported)
            {
                PccrrNegoResponsePacket pccrrNegoResponsePacket = respMSG as PccrrNegoResponsePacket;

                if (pccrrNegoResponsePacket != null)
                {
                    this.ReceiveMsgNegoResp();
                }
            }
            else
            {
                PccrrBLKResponsePacket pccrrBLKResponsePacket = respMSG as PccrrBLKResponsePacket;

                MSG_BLK msgBLK = pccrrBLKResponsePacket.MsgBLK;
                MESSAGE_HEADER messageHeader = pccrrBLKResponsePacket.MessageHeader;
                TRANSPORT_RESPONSE_HEADER transportRespH = pccrrBLKResponsePacket.TransportResponseHeader;
                RESPONSE_MESSAGE respMessage = new RESPONSE_MESSAGE();
                respMessage.MESSAGEBODY = msgBLK;
                respMessage.MESSAGEHEADER = messageHeader;
                respMessage.TRANSPORTRESPONSEHEADER = transportRespH;

                this.VerifyMsgBlk(msgBLK);
                PccrrBothRoleCaptureCode.CaptureSegmentIdRequirements(msgBLK.SegmentId);
                this.VerifyResponseMessage(respMessage);

                bool isSizeOfBlockZero = false;

                if (msgBLK.SizeOfBlock == 0)
                {
                    isSizeOfBlockZero = true;
                }
                else
                {
                    isSizeOfBlockZero = false;
                }

                bool isBlockEmpty = false;

                if (msgBLK.Block.Length == 0)
                {
                    isBlockEmpty = true;
                }
                else
                {
                    isBlockEmpty = false;
                }

                CryptoAlgoIdValues crypAlgoId = Helper.ConvertFromStackCryptoAlgoIdValues(pccrrBLKResponsePacket.MessageHeader.CryptoAlgoId);
                this.ReceiveMsgBlk(msgBLK.BlockIndex, msgBLK.NextBlockIndex, isSizeOfBlockZero, isBlockEmpty, crypAlgoId);
            }
        }

        #endregion

        #region ManagedAdapterBase

        /// <summary>
        /// Initialize adapter data and create connection.
        /// </summary>
        /// <param name="testSite">the test site</param>
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(ReqConfigurableSite.GetReqConfigurableSite(testSite));
            this.Site.DefaultProtocolDocShortName = "MS-PCCRR";
            PccrrBothRoleCaptureCode.Initialize(this.Site);

            this.isDistributedMode = bool.Parse(this.GetProperty("Environment.IsDistributedMode"));

            if (this.isDistributedMode)
            {
                this.isWindows = string.Equals(
                        this.GetProperty("Environment.DistributedSUT.OSVersion"),
                        "win2k8r2",
                        StringComparison.OrdinalIgnoreCase);
                this.serverName = this.GetProperty("Environment.DistributedSUT.MachineName");
            }
            else
            {
                this.isWindows = string.Equals(
                        this.GetProperty("Environment.HostedCacheServer.OSVersion"),
                        "win2k8r2",
                        StringComparison.OrdinalIgnoreCase);
                this.serverName = this.GetProperty("Environment.HostedCacheServer.MachineName");
            }

            this.cryptoAlgo = (CryptoAlgoId_Values)Enum.Parse(typeof(CryptoAlgoId_Values), this.GetProperty("PCCRR.Protocol.CryptoAlgoId_Value"));
            this.timeout = int.Parse(this.GetProperty("PCCRR.Protocol.timeout"));
            this.port = int.Parse(this.GetProperty("PCCRR.Protocol.Http.Port"));

            this.protoErrorVer.MinorVersion = ushort.Parse(this.GetProperty("PCCRR.Protocol.MSG_NEGO_REQ.ErrorProtocolVersion"));
            this.protoErrorVer.MajorVersion = ushort.Parse(this.GetProperty("PCCRR.Protocol.MSG_NEGO_REQ.ErrorProtocolVersion"));
            this.minSupV.MinorVersion = ushort.Parse(this.GetProperty("PCCRR.Protocol.MSG_NEGO_REQ.MinSupportedProtocolVersion.MinorVer"));
            this.minSupV.MajorVersion = ushort.Parse(this.GetProperty("PCCRR.Protocol.MSG_NEGO_REQ.MinSupportedProtocolVersion.MajorVer"));
            this.maxSupV.MinorVersion = ushort.Parse(this.GetProperty("PCCRR.Protocol.MSG_NEGO_REQ.MaxSupportedProtocolVersion.MinorVer"));
            this.maxSupV.MajorVersion = ushort.Parse(this.GetProperty("PCCRR.Protocol.MSG_NEGO_REQ.MaxSupportedProtocolVersion.MajorVer"));
            this.protoVer.MinorVersion = ushort.Parse(this.GetProperty("PCCRR.Protocol.MSG_NEGO_REQ.MinSupportedProtocolVersion.MinorVer"));
            this.protoVer.MajorVersion = ushort.Parse(this.GetProperty("PCCRR.Protocol.MSG_NEGO_REQ.MinSupportedProtocolVersion.MajorVer"));

            this.pccrrStack = new PccrrClient(this.serverName, this.port, PccrrUri, HttpMethod.POST);
        }

        /// <summary>
        /// Reset the adapter
        /// </summary>
        public override void Reset()
        {
            base.Reset();
        }

        /// <summary>
        /// Dispose(bool disposing) executes in two distinct scenarios.
        /// If disposing equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.
        /// </summary>
        /// <param name="disposing">
        /// If disposing equals false, the method has been called by the 
        /// runtime from inside the finalizer and you should not reference 
        /// other objects. Only unmanaged resources can be disposed.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (this.pccrrStack != null)
            {
                this.pccrrStack.Dispose();
            }

            base.Dispose(disposing);
            this.pccrrStack = null;
        }

        #endregion

        #region Private help methods

        /// <summary>
        /// Get the value of property in ptfconfig file.
        /// </summary>
        /// <param name="propName">The property name in ptfconfig file.</param>
        /// <returns>Return the value of property.</returns>
        private string GetProperty(string propName)
        {
            if (string.IsNullOrEmpty(propName))
            {
                throw new ArgumentNullException(propName, "The value of propName can't be null or empty.");
            }

            string propValue = string.Empty;
            propValue = this.Site.Properties[propName];
            return propValue;
        }

        #endregion
    }
}
