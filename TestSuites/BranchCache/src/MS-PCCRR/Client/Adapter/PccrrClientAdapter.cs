// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pccrr
{
    using System;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Text;
    using Microsoft.Protocol.TestSuites;
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrr;
    using Microsoft.Protocols.TestTools.StackSdk.CommonStack;

    /// <summary>
    /// This class is used to provide internal function of sending or receiving message on server.
    /// </summary>
    public partial class PccrrClientAdapter : ManagedAdapterBase, IPccrrClientAdapter
    {
        #region variables

        /// <summary>
        /// Operation system is windows or not.
        /// </summary>
        private bool isWindows;

        /// <summary>
        /// The length of http content body.
        /// </summary>
        private int uiPayload;

        /// <summary>
        /// Indicates the stack of Pccrr.
        /// </summary>
        private PccrrServer pccrrStackSer;

        /// <summary>
        /// Is distributed mode.
        /// </summary>
        private bool isDistributedMode;

        /// <summary>
        /// The port of pccrr.
        /// </summary>
        private int port;

        /// <summary>
        /// The min support version.
        /// </summary>
        private ProtoVersion minSupV;

        /// <summary>
        /// The max support version.
        /// </summary>
        private ProtoVersion maxSupV;

        /// <summary>
        /// cryptoAlgorithm used in sending or receiving message
        /// </summary>
        private CryptoAlgoId_Values cryptoAlgo = CryptoAlgoId_Values.V1;

        /// <summary>
        /// The segment id.
        /// </summary>
        private byte[] sid;

        /// <summary>
        /// The error min support version.
        /// </summary>
        private ProtoVersion minErrorSupV;

        /// <summary>
        /// The error max support version.
        /// </summary>
        private ProtoVersion maxErrorSupV;

        /// <summary>
        /// The proto version.
        /// </summary>
        private ProtoVersion protoVer;

        /// <summary>
        /// The error proto version.
        /// </summary>
        private ProtoVersion protoErrorVer;

        #endregion

        #region events

        /// <summary>
        /// Receiving time out.
        /// </summary>
        public event ReceivingTimeOutHandler ReceivingTimeOut;

        /// <summary>
        /// Receive message MSG_NEGO_REQ.
        /// </summary>
        public event ReceiveMsgNegoReqHandler ReceiveMsgNegoReq;

        /// <summary>
        /// Receive message MSG_GETBLKLIST.
        /// </summary>
        public event ReceiveMsgGetBlkListHandler ReceiveMsgGetBlkList;

        /// <summary>
        /// Receive message MSG_GETBLK.
        /// </summary>
        public event ReceiveMsgGetBlkHandler ReceiveMsgGetBlk;

        #endregion

        #region implementations for interface

        /// <summary>
        /// Send message MSG_NEGO_RESP.
        /// </summary>
        /// <param name="isSupportVersion">If it is true, it is support version, if it is false, it is not support version.</param>
        /// <param name="isWellFormed">If it is true, it is well formed, if it is false, it is not well formed.</param>
        public void SendMsgNegoResp(bool isSupportVersion, bool isWellFormed)
        {
            PccrrNegoResponsePacket pccrrNegoResponsePacket;
            if (!isSupportVersion && isWellFormed)
            {
                pccrrNegoResponsePacket = this.pccrrStackSer.CreateMsgNegoResponse(this.minErrorSupV, this.maxErrorSupV, CryptoAlgoId_Values.AES_128, MsgType_Values.MSG_NEGO_RESP, this.protoErrorVer);
            }
            else if (isSupportVersion && isWellFormed)
            {
                pccrrNegoResponsePacket = this.pccrrStackSer.CreateMsgNegoResponse(this.minSupV, this.maxSupV, CryptoAlgoId_Values.AES_128, MsgType_Values.MSG_NEGO_RESP, this.protoVer);
            }
            else
            {
                pccrrNegoResponsePacket = this.pccrrStackSer.CreateMsgNegoResponse(this.minErrorSupV, this.maxErrorSupV, CryptoAlgoId_Values.AES_128, MsgType_Values.MSG_NEGO_RESP, this.protoErrorVer);
                MSG_NEGO_RESP msgNegoResp = new MSG_NEGO_RESP();
                MESSAGE_HEADER messageHeader = new MESSAGE_HEADER();
                messageHeader = pccrrNegoResponsePacket.MessageHeader;
                msgNegoResp.MinSupporteProtocolVersion.MajorVersion = 0;
                msgNegoResp.MinSupporteProtocolVersion.MinorVersion = 0;
                messageHeader.ProtVer.MajorVersion = 1;
                messageHeader.ProtVer.MinorVersion = 0;
                msgNegoResp.MaxSupporteProtocolVersion.MajorVersion = 0;
                msgNegoResp.MaxSupporteProtocolVersion.MinorVersion = 0;

                pccrrNegoResponsePacket.MsgNegoResp = msgNegoResp;
                pccrrNegoResponsePacket.MessageHeader = messageHeader;
            }

            this.pccrrStackSer.SendPacket(pccrrNegoResponsePacket);
        }

        /// <summary>
        /// Send message MSG_BLKLIST.
        /// </summary>
        /// <param name="isTimerExpire">The timer for SendMsgBlkList from client will expire or not.</param>
        /// <param name="isSameSegment">The SegmentID is same as the request from client.</param>
        /// <param name="dwHashAlgoValues">The dwHashAlgo value.</param>
        /// <param name="isOverlap">The block ranges overlap with the ranges specified in any request 
        /// with a matching Segment ID in the outstanding request list.</param>
        public void SendMsgBlkList(bool isTimerExpire, bool isSameSegment, DWHashAlgValues dwHashAlgoValues, bool isOverlap)
        {
            BLOCK_RANGE[] blockRange;
            if (!isOverlap)
            {
                blockRange = new BLOCK_RANGE[1];
                blockRange[0].Count = 0;
                blockRange[0].Index = 0;
            }
            else
            {
                blockRange = new BLOCK_RANGE[int.Parse(this.GetProperty("PCCRR.Protocol.MSG_BLKLIST.BlockRangeCount"))];
                for (int i = 0; i < blockRange.Length; i++)
                {
                    blockRange[i].Count = uint.Parse(this.GetProperty("PCCRR.Protocol.MSG_BLKLIST.BlockRanges.0.Count"));
                    blockRange[i].Index = uint.Parse(this.GetProperty("PCCRR.Protocol.MSG_BLKLIST.BlockRanges.0.Index")) + (uint)i;
                }
            }

            PccrrBLKLISTResponsePacket pccrrBLKLISTResponsePacket;
            if (!isSameSegment && dwHashAlgoValues == DWHashAlgValues.V1)
            {
                this.sid = Encoding.UTF8.GetBytes(this.GetProperty("PCCRR.Protocol.ErrorSegmentId"));
                pccrrBLKLISTResponsePacket = this.pccrrStackSer.CreateMsgBlkListResponse(this.sid, blockRange, 0, this.cryptoAlgo, MsgType_Values.MSG_BLKLIST, this.protoVer);
            }
            else if (isSameSegment && dwHashAlgoValues == DWHashAlgValues.V1)
            {
                pccrrBLKLISTResponsePacket = this.pccrrStackSer.CreateMsgBlkListResponse(this.sid, blockRange, 0, this.cryptoAlgo, MsgType_Values.MSG_BLKLIST, this.protoVer);
            }
            else
            {
                if (dwHashAlgoValues == DWHashAlgValues.V3)
                {
                    this.sid = PccrrUtitlity.ToByteArray(this.GetProperty("PCCRR.Protocol.SHA512.SegmentId"));
                }
                else
                {
                    this.sid = PccrrUtitlity.ToByteArray(this.GetProperty("PCCRR.Protocol.SHA384.SegmentId"));
                }

                pccrrBLKLISTResponsePacket = this.pccrrStackSer.CreateMsgBlkListResponse(this.sid, blockRange, 0, this.cryptoAlgo, MsgType_Values.MSG_BLKLIST, this.protoVer);
            }

            try
            {
                this.pccrrStackSer.SendPacket(pccrrBLKLISTResponsePacket);
            }
            catch (HttpListenerException ex)
            {
                if (ex.ErrorCode == 1229 && isTimerExpire)
                {
                    this.ReceivingTimeOut();
                }
            }
        }

        /// <summary>
        /// Send message MSG_BLK.
        /// </summary>
        /// <param name="isTimerExpire">The timer for SendMsgBlk from client will expire or not.</param>
        /// <param name="isSameSegment">The message is for the segment that client request.</param>
        /// <param name="dwHashAlgoValues">The dwHashAlgo value.</param>
        /// <param name="index">Block index</param>
        /// <param name="isLastBLK">If it is true, the block is the last BLK.</param>
        public void SendMsgBlk(bool isTimerExpire, bool isSameSegment, DWHashAlgValues dwHashAlgoValues, uint index, bool isLastBLK)
        {
            PccrrBLKResponsePacket pccrrBLKResponsePacket;
            if (!isSameSegment && dwHashAlgoValues == DWHashAlgValues.V1)
            {
                this.sid = Encoding.UTF8.GetBytes(this.GetProperty("PCCRR.Protocol.ErrorSegmentId"));
                pccrrBLKResponsePacket = this.pccrrStackSer.CreateMsgBlkResponse(this.sid, new byte[] { }, this.cryptoAlgo, MsgType_Values.MSG_BLK, this.protoVer, isLastBLK);
            }
            else
            {
                if (dwHashAlgoValues == DWHashAlgValues.V3)
                {
                    this.sid = PccrrUtitlity.ToByteArray(this.GetProperty("PCCRR.Protocol.SHA512.SegmentId"));
                }
                else
                {
                    this.sid = PccrrUtitlity.ToByteArray(this.GetProperty("PCCRR.Protocol.SHA384.SegmentId"));
                }

                pccrrBLKResponsePacket = this.pccrrStackSer.CreateMsgBlkResponse(this.sid, new byte[] { }, this.cryptoAlgo, MsgType_Values.MSG_BLK, this.protoVer, isLastBLK);
            }

            MSG_BLK msgBLK = new MSG_BLK();
            msgBLK = pccrrBLKResponsePacket.MsgBLK;
            msgBLK.BlockIndex = index;

            if (!isLastBLK)
            {
                if (index == uint.MaxValue)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                else
                {
                    msgBLK.NextBlockIndex = index + 1;
                }
            }

            pccrrBLKResponsePacket.MsgBLK = msgBLK;

            try
            {
                this.pccrrStackSer.SendPacket(pccrrBLKResponsePacket);
            }
            catch (HttpListenerException ex)
            {
                if (ex.ErrorCode == 1229 && isTimerExpire)
                {
                    this.ReceivingTimeOut();
                }
            }
        }

        #endregion

        #region ManagedAdapterBase methods

        /// <summary>
        /// Initialize adapter data and create connection.
        /// </summary>
        /// <param name="testSite">the test site</param>
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(ReqConfigurableSite.GetReqConfigurableSite(testSite));
            this.Site.DefaultProtocolDocShortName = "MS-PCCRR";
            this.isDistributedMode = bool.Parse(this.GetProperty("Environment.IsDistributedMode"));

            if (!this.isDistributedMode)
            {
                this.isWindows = string.Equals(
                    testSite.Properties["Environment.SecondContentClient.OSVersion"],
                    "win2k8r2",
                    StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                this.isWindows = string.Equals(
                    testSite.Properties["Environment.DistributedSUT.OSVersion"],
                    "win2k8r2",
                    StringComparison.OrdinalIgnoreCase);
            }

            this.cryptoAlgo = (CryptoAlgoId_Values)Enum.Parse(typeof(CryptoAlgoId_Values), this.GetProperty("PCCRR.Protocol.CryptoAlgoId_Value"));
            this.port = int.Parse(this.GetProperty("PCCRR.Protocol.HttpPort"));
            PccrrBothRoleCaptureCode.Initialize(this.Site);

            this.minSupV.MinorVersion = ushort.Parse(this.GetProperty("PCCRR.Protocol.MSG_NEGO_RESP.MinSupportedProtocolVersion.MinorVer"));
            this.minSupV.MajorVersion = ushort.Parse(this.GetProperty("PCCRR.Protocol.MSG_NEGO_RESP.MinSupportedProtocolVersion.MajorVer"));
            this.maxSupV.MinorVersion = ushort.Parse(this.GetProperty("PCCRR.Protocol.MSG_NEGO_RESP.MaxSupportedProtocolVersion.MinorVer"));
            this.maxSupV.MajorVersion = ushort.Parse(this.GetProperty("PCCRR.Protocol.MSG_NEGO_RESP.MaxSupportedProtocolVersion.MajorVer"));
            this.minErrorSupV.MinorVersion = ushort.Parse(this.GetProperty("PCCRR.Protocol.MSG_NEGO_RESP.Error.MinSupportedProtocolVersion"));
            this.minErrorSupV.MajorVersion = ushort.Parse(this.GetProperty("PCCRR.Protocol.MSG_NEGO_RESP.MaxSupportedProtocolVersion.MajorVer"));
            this.maxErrorSupV.MinorVersion = ushort.Parse(this.GetProperty("PCCRR.Protocol.MSG_NEGO_RESP.Error.MaxSupportedProtocolVersion"));
            this.maxErrorSupV.MajorVersion = ushort.Parse(this.GetProperty("PCCRR.Protocol.MSG_NEGO_RESP.MaxSupportedProtocolVersion.MajorVer"));
            this.protoVer.MinorVersion = ushort.Parse(this.GetProperty("PCCRR.Protocol.MSG_NEGO_REQ.MinSupportedProtocolVersion.MinorVer"));
            this.protoVer.MajorVersion = ushort.Parse(this.GetProperty("PCCRR.Protocol.MSG_NEGO_REQ.MinSupportedProtocolVersion.MajorVer"));
            this.protoErrorVer.MinorVersion = ushort.Parse(this.GetProperty("PCCRR.Protocol.MSG_NEGO_REQ.ErrorProtocolVersion"));
            this.protoErrorVer.MajorVersion = ushort.Parse(this.GetProperty("PCCRR.Protocol.MSG_NEGO_RESP.MaxSupportedProtocolVersion.MajorVer"));

            this.pccrrStackSer = new PccrrServer(this.port, string.Empty, IPAddressType.IPv4);
            this.pccrrStackSer.MessageArrived += new MessageArrivedEventArgs(this.RetrievalTransport_Receive);

            this.pccrrStackSer.StartListening();
        }

        /// <summary>
        /// Reset the adapter
        /// </summary>
        public override void Reset()
        {
            this.pccrrStackSer.CloseConnections();
            base.Reset();
            this.pccrrStackSer.StartListening();
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
            if (this.pccrrStackSer != null)
            {
                this.pccrrStackSer.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Receive method

        /// <summary>
        /// Verify the received message from client.
        /// </summary>
        /// <param name="remoteAddr">The remote address.</param>
        /// <param name="pccrrPacket">The pccrrPacket.</param>
        private void RetrievalTransport_Receive(IPEndPoint remoteAddr, PccrrPacket pccrrPacket)
        {
            PccrrGETBLKSRequestPacket pccrrGETBLKSRequestPacket = pccrrPacket as PccrrGETBLKSRequestPacket;

            if (pccrrGETBLKSRequestPacket != null)
            {
                MSG_GETBLKS msgGETBLKS = pccrrGETBLKSRequestPacket.MsgGetBLKS;
                MESSAGE_HEADER messageHEADER = pccrrGETBLKSRequestPacket.MessageHeader;
                this.sid = msgGETBLKS.SegmentID;
                REQUEST_MESSAGE requestMESSAGE = new REQUEST_MESSAGE();
                requestMESSAGE.MESSAGEBODY = msgGETBLKS;
                requestMESSAGE.MESSAGEHEADER = messageHEADER;
                this.uiPayload = Marshal.SizeOf(messageHEADER) + Marshal.SizeOf(msgGETBLKS) + 24;

                PccrrBothRoleCaptureCode.CaptureBlockRangeRequirements(msgGETBLKS.ReqBlockRanges[0]);
                PccrrBothRoleCaptureCode.CaptureMessageHeaderRequirements(messageHEADER, this.uiPayload);
                this.VerifyGetBlocks(msgGETBLKS);
                this.VerifyRequestMessage(requestMESSAGE);
                PccrrBothRoleCaptureCode.CaptureSegmentIdRequirements(msgGETBLKS.SegmentID);
                PccrrBothRoleCaptureCode.CaptureHttpRequirements();
                PccrrBothRoleCaptureCode.CaptureMessageRequirements();

                this.ReceiveMsgGetBlk(msgGETBLKS.ReqBlockRanges[0].Index);
            }
            else
            {
                PccrrGETBLKLISTRequestPacket pccrrGETBLKLISTRequestPacket = pccrrPacket as PccrrGETBLKLISTRequestPacket;

                if (pccrrGETBLKLISTRequestPacket != null)
                {
                    MSG_GETBLKLIST msgGETBLKLIST = pccrrGETBLKLISTRequestPacket.MsgGetBLKLIST;
                    this.sid = msgGETBLKLIST.SegmentID;

                    this.VerifyGetBlkList(msgGETBLKLIST);
                    PccrrBothRoleCaptureCode.CaptureCommonDataTypesRequirements(msgGETBLKLIST);
                    _BLOCKRANGE[] blockRanges = ClientHelper.ConvertFromStackBLOCKRANGEArray(msgGETBLKLIST.NeededBlockRanges);

                    this.ReceiveMsgGetBlkList(blockRanges);
                }
                else
                {
                    this.ReceiveMsgNegoReq();
                }
            }
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
