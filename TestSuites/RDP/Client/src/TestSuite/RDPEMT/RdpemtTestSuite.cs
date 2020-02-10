// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.Protocols.TestSuites.Rdpbcgr;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpemt;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpefs;
using Microsoft.Protocols.TestSuites.Rdp.Rdpefs;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;

namespace Microsoft.Protocols.TestSuites.Rdpemt
{
    [TestClass]
    public partial class RdpemtTestSuite : RdpTestClassBase
    {
        struct RTTMeasureDataStore
        {
            public DateTime requestTime;
            public DateTime responseTime;
        }

        struct BandwidthDataStore
        {
            public uint timeDelta;
            public uint byteCount;
        }

        #region Variables

        private RdpeudpServer rdpeudpServer;
        private RdpeudpSocket rdpeudpSocketR;
        private RdpeudpSocket rdpeudpSocketL;

        private RdpemtServer rdpemtServerR;
        private RdpemtServer rdpemtServerL;

        private uint multitransportRequestId = 0;
        private TimeSpan timeout = new TimeSpan(0, 0, 30);

        RTTMeasureDataStore rttDataStore;
        BandwidthDataStore bwDataStore;
        
        uint autoDetectedBaseRTT;
        uint autoDetectedBandwidth;
        uint autoDetectedAverageRTT;

        private List<uint> requestIdList = new List<uint>();
        private List<byte[]> securityCookieList = new List<byte[]>();

        RdpbcgrServer rdpbcgrServer;
        RdpedycServer rdpedycServer;
        IRdpefsAdapter rdpefsAdapter;

        #endregion

        #region Class Initialization and Cleanup
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            RdpTestClassBase.BaseInitialize(context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            RdpTestClassBase.BaseCleanup();
        }
        #endregion

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            base.TestInitialize();

            CheckSecurityProtocolForMultitransport();
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();

            rdpbcgrServer.Dispose();

            if (rdpedycServer != null)
                rdpedycServer.Dispose();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Trigger client to close all RDP connections for clean up.");
            int iResult = this.sutControlAdapter.TriggerClientDisconnectAll(this.TestContext.TestName);
            this.TestSite.Log.Add(LogEntryKind.Debug, "The result of TriggerClientDisconnectAll is {0}.", iResult);

            if (rdpemtServerL != null)
                rdpemtServerL.Dispose();

            if (rdpemtServerR != null)
                rdpemtServerR.Dispose();
            if (rdpeudpServer != null && rdpeudpServer.Running)
                rdpeudpServer.Stop();
            if (rdpeudpSocketR != null && rdpeudpSocketR.Connected)
                rdpeudpSocketR.Close();
            if (rdpeudpSocketL != null && rdpeudpSocketL.Connected)
                rdpeudpSocketL.Close();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Stop RDP listening.");
            this.rdpbcgrAdapter.StopRDPListening();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Start RDP connection.
        /// </summary>
        private void StartRDPConnection(bool useRDPEncryption = false, bool isSoftSync = false, bool isUDPPreferred = false)
        {

            if (useRDPEncryption)
            {
                //if force RDP encryption
                selectedProtocol = selectedProtocols_Values.PROTOCOL_RDP_FLAG;
                enMethod = EncryptionMethods.ENCRYPTION_METHOD_128BIT;
                enLevel = EncryptionLevel.ENCRYPTION_LEVEL_LOW;
                transportProtocol = EncryptedProtocol.Rdp;
            }

            int port = ConstValue.TEST_PORT;
            rdpbcgrServer = new RdpbcgrServer(port, transportProtocol);

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);


            #region Trigger client to connect
            //Trigger client to connect. 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            #region RDPBCGR Connection

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);


            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            MULTITRANSPORT_TYPE_FLAGS flags = MULTITRANSPORT_TYPE_FLAGS.TRANSPORTTYPE_UDPFECL | MULTITRANSPORT_TYPE_FLAGS.TRANSPORTTYPE_UDPFECR;

            if (isUDPPreferred)
            {
                flags |= MULTITRANSPORT_TYPE_FLAGS.TRANSPORTTYPE_UDP_PREFERRED;
            }

            if (isSoftSync)
            {
                flags |= MULTITRANSPORT_TYPE_FLAGS.SOFTSYNC_TCP_TO_UDP;
            }

            this.rdpbcgrAdapter.EstablishRDPConnection(
                selectedProtocol, 
                enMethod, 
                enLevel, 
                true, 
                false, 
                rdpServerVersion, 
                flags,
                false,
                false);

            if (isSoftSync)
            {
                Site.Assert.IsTrue(this.rdpbcgrAdapter.SessionContext.MultitransportTypeFlagsInMCSConnectIntialPdu.HasFlag(MULTITRANSPORT_TYPE_FLAGS.SOFTSYNC_TCP_TO_UDP),
                   "Client Should support Soft-Sync, flags: {0}",
                   this.rdpbcgrAdapter.SessionContext.MultitransportTypeFlagsInMCSConnectIntialPdu);
            }

            if (isUDPPreferred)
            {
                Site.Assert.IsTrue(this.rdpbcgrAdapter.SessionContext.MultitransportTypeFlagsInMCSConnectIntialPdu.HasFlag(MULTITRANSPORT_TYPE_FLAGS.SOFTSYNC_TCP_TO_UDP)
                    && this.rdpbcgrAdapter.SessionContext.MultitransportTypeFlagsInMCSConnectIntialPdu.HasFlag(MULTITRANSPORT_TYPE_FLAGS.TRANSPORTTYPE_UDP_PREFERRED),
                   "Client Should support SOFTSYNC_TCP_TO_UDP and TRANSPORTTYPE_UDP_PREFERRED, flags: {0}",
                   this.rdpbcgrAdapter.SessionContext.MultitransportTypeFlagsInMCSConnectIntialPdu);
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            #endregion
        }

        /// <summary>
        /// Establish a UDP connection
        /// </summary>
        /// <param name="udpTransportMode">Transport mode: Reliable or Lossy</param>
        /// <param name="timeout">wait time</param>
        /// <returns>The accepted socket</returns>
        private void EstablishUDPConnection(TransportMode udpTransportMode, TimeSpan timeout)
        {
            //Start UDP listening
            if (rdpeudpServer == null)
            { 
                rdpeudpServer = new RdpeudpServer((IPEndPoint)this.rdpbcgrAdapter.SessionContext.LocalIdentity, true);

                rdpeudpServer.UnhandledExceptionReceived += (ex) =>
                {
                    Site.Log.Add(LogEntryKind.Debug, $"Unhandled exception from RdpeudpServer: {ex}");
                };
            }
            if (!rdpeudpServer.Running)
                rdpeudpServer.Start();

            //Send a Server Initiate Multitransport Request PDU
            byte[] securityCookie = new byte[16];
            Random rnd = new Random();
            rnd.NextBytes(securityCookie);
            Multitransport_Protocol_value requestedProtocol = Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECR;
            if (udpTransportMode == TransportMode.Lossy)
            {
                requestedProtocol = Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECL;
            }
            this.rdpbcgrAdapter.SendServerInitiateMultitransportRequestPDU(++this.multitransportRequestId, requestedProtocol, securityCookie);
            this.requestIdList.Add(this.multitransportRequestId);
            this.securityCookieList.Add(securityCookie);

            //create a UDP socket
            RdpeudpSocket rdpudpSocket = rdpeudpServer.Accept(((IPEndPoint)this.rdpbcgrAdapter.SessionContext.Identity).Address, udpTransportMode, timeout);

            if (udpTransportMode == TransportMode.Reliable)
            {
                this.rdpeudpSocketR = rdpudpSocket;
            }
            else
            {
                this.rdpeudpSocketL = rdpudpSocket;
            }
        }

        /// <summary>
        /// Used to establish a RDPEMT connection
        /// </summary>
        /// <param name="udpTransportMode">Transport Mode: Reliable or Lossy</param>
        /// <param name="timeout">wait time</param>
        private void EstablishRdpemtConnection(TransportMode udpTransportMode, TimeSpan timeout, bool verifyPacket = false)
        {
            RdpeudpSocket rdpeudpSocket = rdpeudpSocketR;
            if (udpTransportMode == TransportMode.Lossy)
            {
                rdpeudpSocket = rdpeudpSocketL;
            }


            String certFile = this.Site.Properties["CertificatePath"];
            String certPwd = this.Site.Properties["CertificatePassword"];
            X509Certificate2 cert = new X509Certificate2(certFile, certPwd);
            RdpemtServer rdpemtServer = new RdpemtServer(rdpeudpSocket, cert, false);
                        
            uint receivedRequestId;
            byte[] receivedSecurityCookie;
            if (!rdpemtServer.ExpectConnect(waitTime, out receivedRequestId, out receivedSecurityCookie))
            {
                Site.Assert.Fail("RDPEMT tunnel creation failed");
            }

            if (verifyPacket)
            {
                VerifyTunnelCreateRequestPacket(receivedRequestId, receivedSecurityCookie);
            }
            
            if (udpTransportMode == TransportMode.Reliable)
            {
                rdpemtServerR = rdpemtServer;
            }
            else
            {
                rdpemtServerL = rdpemtServer;
            }
        }

        #region Soft-Sync connection
        /// <summary>
        /// Establish EMT connection and soft sync.
        /// </summary>
        private void StartSoftSyncConnection(TransportMode mode)
        {
            StartRDPConnection(false, true);
            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} UDP connection.", mode);
            this.EstablishUDPConnection(mode, waitTime);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} RDPEMT connection.", mode);
            this.EstablishRdpemtConnection(mode, waitTime, true);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expect for Client Initiate Multitransport PDU to indicate that the client was able to successfully complete the multitransport initiation request.");
            this.rdpbcgrAdapter.WaitForPacket<Client_Initiate_Multitransport_Response_PDU>(waitTime);

            // This response code MUST only be sent to a server that advertises the SOFTSYNC_TCP_TO_UDP (0x200) flag in the Server Multitransport Channel Data.
            // Indicates that the client was able to successfully complete the multitransport initiation request.
            if (requestIdList.Count == 1)
                VerifyClientInitiateMultitransportResponsePDU(rdpbcgrAdapter.SessionContext.ClientInitiateMultitransportResponsePDU, requestIdList[0], HrResponse_Value.S_OK);

            #region Start EDYC soft sync
            if(rdpedycServer == null)
            {
                rdpedycServer = new Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc.RdpedycServer(this.rdpbcgrAdapter.ServerStack, this.rdpbcgrAdapter.SessionContext);
            }
   
            this.TestSite.Log.Add(LogEntryKind.Comment, "Start Dynamic VC Version Negotiation");
            rdpedycServer.ExchangeCapabilities(waitTime);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Start Soft-Sync");
            rdpedycServer.SoftSyncNegotiate(waitTime);
            #endregion
        }
        #endregion 

        #region Tunneling Static VC Traffic
        // Default is using RDPEFS static virtual channel. Now is using DVC named "rdpdr".
        private void EstablishTunnelingStaticVCTrafficConnection(string staticChannelName = "RDPDR")
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Start RDP connection, support soft sync and UDP perferred.");
            StartRDPConnection(false, true, true);

            // Check whether 'rdpdr' channel has been created
            if (this.rdpbcgrAdapter.GetStaticVirtualChannelId(staticChannelName) == 0)
            {
                this.TestSite.Assume.Fail("The necessary channel {0} has not been created, so stop running this test case.", staticChannelName);
            }

            if (rdpefsAdapter == null)
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "Create rdpefs adapter.");
                this.rdpefsAdapter = (IRdpefsAdapter)this.TestSite.GetAdapter(typeof(IRdpefsAdapter));
                this.rdpefsAdapter.Reset();
                this.rdpefsAdapter.AttachRdpbcgrAdapter(this.rdpbcgrAdapter);
            }

            if (rdpedycServer == null)
            {
                rdpedycServer = new RdpedycServer(this.rdpbcgrAdapter.ServerStack, this.rdpbcgrAdapter.SessionContext);
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Start Dynamic VC Version Negotiation");
            ushort version = rdpedycServer.ExchangeCapabilities(waitTime);
            if(version < 0x0003)
            {
                this.TestSite.Log.Add(LogEntryKind.TestError, "Client doesn's support Version 3 DYNVC.");
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a dynamic virtual channel for MS-RDPEFS");
            rdpefsAdapter.ProtocolInitialize(rdpedycServer);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Send and receive efs data over DVC");
            rdpefsAdapter.EfsInitializationSequenceOverDVC();
        }
        #endregion 

        #region RTT Measure
        /// <summary>
        /// Send a Tunnel Data PDU with RTT Measure Request in its subheader
        /// </summary>
        /// <param name="udpTransportMode">Transport Mode: Reliable or Lossy</param>
        private void SendTunnelDataPdu_RTTMeasureRequest(Multitransport_Protocol_value requestedProtocol, ushort sequenceNumber)
        {
            AUTO_DETECT_REQUEST_TYPE requestType = AUTO_DETECT_REQUEST_TYPE.RDP_RTT_REQUEST_AFTER_CONNECTTIME;
            RdpemtServer rdpemtServer = rdpemtServerR;
            if (requestedProtocol == Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECL)
            {
                rdpemtServer = rdpemtServerL;
            }
            RDP_RTT_REQUEST RTTRequest = RdpbcgrUtility.GenerateRTTMeasureRequest(requestType, sequenceNumber);
            byte[] reqData = rdpbcgrServer.EncodeNetworkDetectionRequest(RTTRequest, true);
            List<byte[]> subHdDataList = new List<byte[]>();
            subHdDataList.Add(reqData);
            RDP_TUNNEL_DATA tunnelData = rdpemtServer.CreateTunnelDataPdu(null, subHdDataList);
            rttDataStore.requestTime = DateTime.Now;
            rdpemtServer.SendRdpemtPacket(tunnelData);
        }

        /// <summary>
        /// Wait for a Tunnel Data PDU with RDP_RTT_RESPONSE and check its sequenceNumber.
        /// </summary>
        /// <param name="requestedProtocol">Which tunnel to be used, reliable or lossy</param>
        /// <param name="sequenceNumber"></param>
        /// <param name="timeout"></param>
        private void WaitForAndCheckTunnelDataPdu_RTTResponse(Multitransport_Protocol_value requestedProtocol, ushort sequenceNumber, TimeSpan timeout)
        {

            bool isReceived = false;
            TimeSpan leftTime = timeout;
            DateTime expiratedTime = DateTime.Now + timeout;
            RDP_RTT_RESPONSE rttResponse = null;

            RdpemtServer rdpemtServer = rdpemtServerR;
            if (requestedProtocol == Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECL)
            {
                rdpemtServer = rdpemtServerL;
            }

            while (!isReceived && leftTime.CompareTo(new TimeSpan(0)) > 0)
            {
                try
                {
                    RDP_TUNNEL_DATA tunnelData = rdpemtServer.ExpectTunnelData(leftTime);

                    if (tunnelData != null)
                    {
                        RDP_TUNNEL_SUBHEADER[] SubHeaders = tunnelData.TunnelHeader.SubHeaders;
                        if (SubHeaders != null)
                        {
                            foreach (RDP_TUNNEL_SUBHEADER subHeader in SubHeaders)
                            {
                                if (subHeader.SubHeaderType == RDP_TUNNEL_SUBHEADER_TYPE_Values.TYPE_ID_AUTODETECT_RESPONSE)
                                {
                                    NETWORK_DETECTION_RESPONSE detectRsp = rdpbcgrServer.ParseNetworkDetectionResponse(subHeader.SubHeaderData, true);
                                    {
                                        if (detectRsp.responseType == AUTO_DETECT_RESPONSE_TYPE.RDP_RTT_RESPONSE)
                                        {
                                            rttResponse = (RDP_RTT_RESPONSE)detectRsp;
                                            isReceived = true;
                                            rttDataStore.responseTime = DateTime.Now;
                                            Site.Log.Add(LogEntryKind.Comment, "RequestTime: {0}\tResponseTime: {1}", rttDataStore.responseTime.Ticks, rttDataStore.responseTime.Ticks);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (TimeoutException)
                {
                    Site.Assert.Fail("Timeout when expecting a Tunnel Data PDU with RDP_RTT_RESULTS");
                }
                catch (InvalidOperationException ex)
                {
                    //break;
                    Site.Log.Add(LogEntryKind.Warning, "Exception thrown out when receiving client PDUs {0}.", ex.Message);
                }
                finally
                {
                    System.Threading.Thread.Sleep(100);//Wait some time for next packet.
                    leftTime = expiratedTime - DateTime.Now;
                }
            }
            if (isReceived)
            {
                VerifyTunnelDataPdu_RTTResponse(rttResponse, sequenceNumber);
            }
            else
            {
                Site.Assert.Fail("Timeout when expecting a Tunnel Data PDU with RDP_RTT_RESULTS");
            }
        }
        #endregion

        #region Bandwidth Measure
        /// <summary>
        /// Send a Tunnel Data PDU with RDP_BW_START in its subheader
        /// </summary>
        /// <param name="requestedProtocol"></param>
        /// <param name="sequenceNumber"></param>
        private void SendTunnelDataPdu_BWStart(Multitransport_Protocol_value requestedProtocol, ushort sequenceNumber)
        {
            AUTO_DETECT_REQUEST_TYPE requestType = AUTO_DETECT_REQUEST_TYPE.RDP_BW_START_AFTER_CONNECTTIME_OR_RELIABLEUDP;
            RdpemtServer rdpemtServer = rdpemtServerR;
            if (requestedProtocol == Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECL)
            {
                requestType = AUTO_DETECT_REQUEST_TYPE.RDP_BW_START_AFTER_CONNECTTIME_OR_LOSSYUDP;
                rdpemtServer = rdpemtServerL;
            }
            RDP_BW_START bwStart = RdpbcgrUtility.GenerateBandwidthMeasureStart(requestType, sequenceNumber);
            byte[] reqData = rdpbcgrServer.EncodeNetworkDetectionRequest(bwStart, true);
            List<byte[]> subHdDataList = new List<byte[]>();
            subHdDataList.Add(reqData);
            RDP_TUNNEL_DATA tunnelData = rdpemtServer.CreateTunnelDataPdu(null, subHdDataList);
            rdpemtServer.SendRdpemtPacket(tunnelData);
        }

        /// <summary>
        /// Send a Tunnel Data PDU with RDP_BW_STOP in its subheader
        /// </summary>
        /// <param name="requestedProtocol"></param>
        /// <param name="sequenceNumber"></param>
        private void SendTunnelDataPdu_BWStop(Multitransport_Protocol_value requestedProtocol, ushort sequenceNumber)
        {
            AUTO_DETECT_REQUEST_TYPE requestType = AUTO_DETECT_REQUEST_TYPE.RDP_BW_STOP_AFTER_CONNECTTIME_OR_RELIABLEUDP;
            RdpemtServer rdpemtServer = rdpemtServerR;
            if (requestedProtocol == Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECL)
            {
                requestType = AUTO_DETECT_REQUEST_TYPE.RDP_BW_STOP_AFTER_CONNECTTIME_OR_LOSSYUDP;
                rdpemtServer = rdpemtServerL;
            }
            RDP_BW_STOP bwStop = RdpbcgrUtility.GenerateBandwidthMeasureStop(requestType, sequenceNumber);
            byte[] reqData = rdpbcgrServer.EncodeNetworkDetectionRequest(bwStop, true);
            List<byte[]> subHdDataList = new List<byte[]>();
            subHdDataList.Add(reqData);
            RDP_TUNNEL_DATA tunnelData = rdpemtServer.CreateTunnelDataPdu(null, subHdDataList);
            rdpemtServer.SendRdpemtPacket(tunnelData);

        }


        /// <summary>
        /// Send a Tunnel Data PDU with RDP_NETCHAR_RESULT in its subheader
        /// </summary>
        /// <param name="requestedProtocol"></param>
        /// <param name="sequenceNumber"></param>
        private void SendTunnelDataPdu_NetcharResult(Multitransport_Protocol_value requestedProtocol, ushort sequenceNumber)
        {
            autoDetectedBaseRTT = (uint) (rttDataStore.responseTime - rttDataStore.requestTime).Milliseconds;
            autoDetectedAverageRTT = (uint)(rttDataStore.responseTime - rttDataStore.requestTime).Milliseconds;
            if (bwDataStore.timeDelta != 0)
            {
                autoDetectedBandwidth = (bwDataStore.byteCount * 8) / bwDataStore.timeDelta;
            }

            RdpemtServer rdpemtServer = rdpemtServerR;
            if (requestedProtocol == Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECL)
            {
                rdpemtServer = rdpemtServerL;
            }
            RDP_NETCHAR_RESULT netResult = RdpbcgrUtility.GenerateNetworkCharacteristicsResult(AUTO_DETECT_REQUEST_TYPE.RDP_NETCHAR_RESULT_BASERTT_BANDWIDTH_AVERAGERTT, sequenceNumber, autoDetectedBaseRTT, autoDetectedBandwidth, autoDetectedAverageRTT);
            byte[] reqData = rdpbcgrServer.EncodeNetworkDetectionRequest(netResult, true);
            List<byte[]> subHdDataList = new List<byte[]>();
            subHdDataList.Add(reqData);
            RDP_TUNNEL_DATA tunnelData = rdpemtServer.CreateTunnelDataPdu(null, subHdDataList);
            rdpemtServer.SendRdpemtPacket(tunnelData);

        }

        /// <summary>
        /// Wait for a Tunnel Data PDU with RDP_BW_RESULTS and check its sequenceNumber.
        /// </summary>
        /// <param name="requestedProtocol">Which tunnel to be used, reliable or lossy</param>
        /// <param name="sequenceNumber"></param>
        /// <param name="timeout"></param>
        private void WaitForAndCheckTunnelDataPdu_BWResult(Multitransport_Protocol_value requestedProtocol, ushort sequenceNumber, TimeSpan timeout, bool NegiveLossy = false)
        {

            bool isReceived = false;
            TimeSpan leftTime = timeout;
            DateTime expiratedTime = DateTime.Now + timeout;
            RDP_BW_RESULTS bwResult = null;

            RdpemtServer rdpemtServer = rdpemtServerR;
            if (requestedProtocol == Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECL)
            {
                rdpemtServer = rdpemtServerL;
            }

            while (!isReceived && leftTime.CompareTo(new TimeSpan(0)) > 0)
            {
                try
                {
                    RDP_TUNNEL_DATA tunnelData = rdpemtServer.ExpectTunnelData(leftTime);

                    if (tunnelData != null)
                    {
                        RDP_TUNNEL_SUBHEADER[] SubHeaders = tunnelData.TunnelHeader.SubHeaders;
                        if (SubHeaders != null)
                        {
                            foreach (RDP_TUNNEL_SUBHEADER subHeader in SubHeaders)
                            {
                                if (subHeader.SubHeaderType == RDP_TUNNEL_SUBHEADER_TYPE_Values.TYPE_ID_AUTODETECT_RESPONSE)
                                {
                                    NETWORK_DETECTION_RESPONSE detectRsp = rdpbcgrServer.ParseNetworkDetectionResponse(subHeader.SubHeaderData, true);
                                    {
                                        if (detectRsp.responseType == AUTO_DETECT_RESPONSE_TYPE.RDP_BW_RESULTS_AFTER_CONNECT)
                                        {
                                            bwResult = (RDP_BW_RESULTS)detectRsp;
                                            isReceived = true;
                                            bwDataStore.byteCount = bwResult.byteCount;
                                            bwDataStore.timeDelta = bwResult.timeDelta;
                                            Site.Log.Add(LogEntryKind.Comment, "ByteCount: {0} Bytes\tTimeDelta: {1} Milliseconds", bwDataStore.byteCount, bwDataStore.timeDelta);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (TimeoutException)
                {
                    if (NegiveLossy)
                    {
                        Site.Log.Add(LogEntryKind.Comment, "If the sequenceNumber of RDP_BW_STOP is different from that in RDP_BW_START, Client should not send RDP_BW_RESULTS");
                    }
                    else
                    {
                        Site.Assert.Fail("Timeout when expecting a Tunnel Data PDU with RDP_BW_RESULTS");
                    }
                }
                catch (InvalidOperationException ex)
                {
                    //break;
                    Site.Log.Add(LogEntryKind.Warning, "Exception thrown out when receiving client PDUs {0}.", ex.Message);
                }
                finally
                {
                    System.Threading.Thread.Sleep(100);//Wait some time for next packet.
                    leftTime = expiratedTime - DateTime.Now;
                }
            }
            if (isReceived)
            {
                VerifyTunnelDataPdu_BWResult(bwResult, sequenceNumber);
                if (NegiveLossy)
                {
                    Site.Assert.Fail("If the sequenceNumber of RDP_BW_STOP is different from that in RDP_BW_START, Client should not send RDP_BW_RESULTS");
                }
            }
            else
            {
                if (NegiveLossy)
                {
                    Site.Log.Add(LogEntryKind.Comment, "If the sequenceNumber of RDP_BW_STOP is different from that in RDP_BW_START, Client should not send RDP_BW_RESULTS");
                }
                else
                {
                    Site.Assert.Fail("Timeout when expecting a Tunnel Data PDU with RDP_BW_RESULTS");
                }
            }
        }

        /// <summary>
        /// Send some random tunnel data
        /// </summary>
        /// <param name="requestedProtocol">Which tunnel to be used, reliable or lossy</param>
        public void SendRandomTunnelData(TransportMode udpTransportMode)
        {
            RdpemtServer rdpemtServer = rdpemtServerR;
            if (udpTransportMode == TransportMode.Lossy)
            {
                rdpemtServer = rdpemtServerL;
            }
            Random rnd = new Random();
            for (int i = 0; i < 3; i++)
            {
                int len = rnd.Next(100, 500);
                byte[] randomData = new byte[len];
                rnd.NextBytes(randomData);
                rdpemtServer.Send(randomData);
            }
        }
        #endregion

        #endregion

        #region Packet Verification
        /// <summary>
        /// Verify Tunnel Create Request packet
        /// </summary>
        /// <param name="receivedRequestId">The received RequestId</param>
        /// <param name="receivedSecurityCookie">The received Security Cookie</param>
        private void VerifyTunnelCreateRequestPacket(uint receivedRequestId, byte[] receivedSecurityCookie)
        {
            if (!requestIdList.Contains(receivedRequestId))
            {
                Site.Assert.Fail("The RequestID field MUST contain the request ID included in the Initiate Multitransport Request PDU that was sent over the main RDP connection");
            }

            bool flagContain = false;
            foreach (byte[] cookie in securityCookieList)
            {
                if (isSameArray(cookie, receivedSecurityCookie)) flagContain = true;
            }
            if (!flagContain)
            {
                Site.Assert.Fail("The SecurityCookie field MUST contain the security cookie included in the Initiate Multitransport Request PDU that was sent over the main RDP connection");
            }
        }

        /// <summary>
        /// Verify RDP Bandwidth Measure Result packet
        /// </summary>
        /// <param name="BwResult">RDP Bandwidth Result packet</param>
        /// <param name="sequenceNumber">The sequence Number</param>
        private void VerifyTunnelDataPdu_BWResult(RDP_BW_RESULTS BwResult, ushort sequenceNumber)
        {
            if (BwResult == null)
            {
                Site.Assert.Fail("Not get Bandwidth Measure Result");
            }
            if (BwResult.sequenceNumber != sequenceNumber)
            {
                Site.Assert.Fail("Expect sequence Number is {0}, but receive sequence Number: {1}", sequenceNumber, BwResult.sequenceNumber);
            }
        }

        /// <summary>
        /// Verify RDP RTT Measure Response packet
        /// </summary>
        /// <param name="rttResponse">RDP RTT Measure Response packet</param>
        /// <param name="sequenceNumber">The sequence Number</param>
        private void VerifyTunnelDataPdu_RTTResponse(RDP_RTT_RESPONSE rttResponse, ushort sequenceNumber)
        {
            if (rttResponse == null)
            {
                Site.Assert.Fail("Not get Bandwidth Measure Result");
            }
            if (rttResponse.sequenceNumber != sequenceNumber)
            {
                Site.Assert.Fail("Expect sequence Number is {0}, but receive sequence Number: {1}", sequenceNumber, rttResponse.sequenceNumber);
            }
        }

        /// <summary>
        /// Verify Client Initiate Multitransport Response PDU
        /// </summary>
        /// <param name="multitransportResponsePdu">Client Initiate Multitransport Response PDU</param>
        /// <param name="expectedRequestId">Expected requestId</param>
        /// <param name="value">Expected response value</param>
        private void VerifyClientInitiateMultitransportResponsePDU(Client_Initiate_Multitransport_Response_PDU multitransportResponsePdu, uint expectedRequestId, HrResponse_Value value = HrResponse_Value.E_ABORT)
        {
            if (multitransportResponsePdu == null)
            {
                Site.Assert.Fail("Not get Client Initiate Multitransport Error PDU");
            }
            Site.Assert.AreEqual(expectedRequestId, multitransportResponsePdu.requestId, "Expected request id is {0}, but request id in received Multitransport Error Pdu is {1}.", expectedRequestId, multitransportResponsePdu.requestId);
            Site.Assert.AreEqual(value, multitransportResponsePdu.hrResponse, "hrResponse field must be {0}", value);
        }

        /// <summary>
        /// Judge whether two arrays are the same
        /// </summary>
        /// <param name="array1">one array</param>
        /// <param name="array2">another array</param>
        private bool isSameArray(byte[] array1, byte[] array2)
        {
            if (array1 == null && array2 == null) return true;
            if (array1 == null && array2 != null) return false;
            if (array1 != null && array2 == null) return false;
            if (array1.Length != array2.Length) return false;
            else
            {
                for (int i = 0; i < array1.Length; i++)
                {
                    if (array1[i] != array2[i]) return false;
                }
            }
            return true;
        }

        #endregion
    }
}
