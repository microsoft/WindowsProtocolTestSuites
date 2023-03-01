// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Reflection;
using System.Windows;
using System.Net;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using System.IO;
using System.Diagnostics;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp;
using System.Security.Policy;

namespace Microsoft.Protocols.TestManager.RDPClientPlugin
{
    public class RDPDetector : IDisposable
    {
        #region Variables

        private DetectionInfo detectInfo;
        private RdpbcgrServer rdpbcgrServerStack;
        private RdpbcgrServerSessionContext sessionContext;
        private RdpedycServer rdpedycServer;

        private int port = 3389;
        private EncryptedProtocol encryptedProtocol = EncryptedProtocol.Rdp;
        private TimeSpan timeout = new TimeSpan(0, 0, 20);
        private DetectLogger logWriter = null;

        #region Received Packets

        Client_X_224_Connection_Request_Pdu x224ConnectionRequest = null;
        Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request mscConnectionInitialPDU = null;
        Client_Security_Exchange_Pdu securityExchangePDU = null;
        Client_Info_Pdu clientInfoPDU = null;
        Client_Confirm_Active_Pdu confirmActivePDU = null;
        RdpbcgrCapSet clientCapSet = null;
        ushort requestId = 0;

        #endregion Received Packets

        private const string RdpedispChannelName = "Microsoft::Windows::RDS::DisplayControl";
        private const string RdpegfxChannelName = "Microsoft::Windows::RDS::Graphics";
        private const string rdpeiChannelName = "Microsoft::Windows::RDS::Input";
        private const string RdpeusbChannelName = "URBDRC";
        private const string RdpegtChannelName = "Microsoft::Windows::RDS::Geometry::v08.01";
        private const string RdpevorControlChannelName = "Microsoft::Windows::RDS::Video::Control::v08.01";
        private const string RdpevorDataChannelName = "Microsoft::Windows::RDS::Video::Data::v08.01";


        public const string SystemManagementAutomationAssemblyNameV1 =
           "System.Management.Automation, Version=1.0.0.0, Culture=neutral, " +
           "PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL";
        public const string SystemManagementAutomationAssemblyNameV3 =
            "System.Management.Automation, Version=3.0.0.0, Culture=neutral, " +
            "PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL";
        public readonly string SUTControlScriptLocation;
        public readonly string SUTShellControlScriptLocation;

        #endregion Variables
        
        #region Constructor

        public RDPDetector(DetectionInfo detectInfo, DetectLogger logger, string testSuitePath)
        {
            this.detectInfo = detectInfo;
            this.logWriter = logger;

            string rdpClientRoute = Path.Combine(testSuitePath, "Plugin");

            SUTControlScriptLocation = Path.Combine(rdpClientRoute, "SUTControlAdapter");

            SUTShellControlScriptLocation = Path.Combine(rdpClientRoute, "ShellSUTControlAdapter");
        }
        #endregion Constructor

        #region Methods

        /// <summary>
        /// Establish a RDP connection to detect RDP feature
        /// </summary>
        /// <returns></returns>
        public bool DetectRDPFeature()
        {
            // Establish a RDP connection with RDP client
            try
            {
                logWriter.AddLog(DetectLogLevel.Information, "Establish RDP connection with SUT...");

                StartRDPListening(detectInfo.RDPServerPort);
                triggerClientRDPConnect(detectInfo.TriggerMethod);
                EstablishRDPConnection();
                // Set RDP Version
                SetRdpVersion();

                CheckSupportedFeatures();

                CheckSupportedProtocols();

            }
            catch (Exception e)
            {
                logWriter.AddLog(DetectLogLevel.Information, "Exception occured when establishing RDP connection: " + e.Message);
                logWriter.AddLog(DetectLogLevel.Information, "" + e.StackTrace);
                
                if (e.InnerException != null)
                {
                    logWriter.AddLog(DetectLogLevel.Information, "**" + e.InnerException.Message);
                    logWriter.AddLog(DetectLogLevel.Information, "**" + e.InnerException.StackTrace);
                }

                logWriter.AddLog(DetectLogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                return false;
            }
            finally
            {
                // Trigger client to close the RDP connection
                TriggerClientDisconnectAll(detectInfo.TriggerMethod);

                if (this.rdpedycServer != null)
                {
                    this.rdpedycServer.Dispose();
                    this.rdpedycServer = null;
                }
                if (this.rdpbcgrServerStack != null)
                {
                    this.rdpbcgrServerStack.Dispose();
                    this.rdpbcgrServerStack = null;
                }
            }

            // Notify the UI for establishing RDP connection successfully.
            logWriter.AddLog(DetectLogLevel.Warning, "Finished", false, LogStyle.StepPassed);

            return true;
        }

        private void CheckSupportedFeatures()
        {
            logWriter.AddLog(DetectLogLevel.Information, "Check specified features support...");
            // Set result according to messages during connection
            if (mscConnectionInitialPDU.mcsCi.gccPdu.clientCoreData != null && mscConnectionInitialPDU.mcsCi.gccPdu.clientCoreData.earlyCapabilityFlags != null)
            {
                detectInfo.IsSupportNetcharAutoDetect = ((mscConnectionInitialPDU.mcsCi.gccPdu.clientCoreData.earlyCapabilityFlags.actualData & (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_NETWORK_AUTODETECT)
                    == (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_NETWORK_AUTODETECT);

                detectInfo.IsSupportRDPEGFX = ((mscConnectionInitialPDU.mcsCi.gccPdu.clientCoreData.earlyCapabilityFlags.actualData & (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_DYNVC_GFX_PROTOCOL)
                    == (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_DYNVC_GFX_PROTOCOL);

                detectInfo.IsSupportHeartbeatPdu = ((mscConnectionInitialPDU.mcsCi.gccPdu.clientCoreData.earlyCapabilityFlags.actualData & (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_HEARTBEAT_PDU)
                    == (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_HEARTBEAT_PDU);
            }
            else
            {
                detectInfo.IsSupportNetcharAutoDetect = false;
                detectInfo.IsSupportRDPEGFX = false;
                detectInfo.IsSupportHeartbeatPdu = false;
            }

            if (mscConnectionInitialPDU.mcsCi.gccPdu.clientMultitransportChannelData != null)
            {
                detectInfo.IsSupportTransportTypeUdpFECR = (mscConnectionInitialPDU.mcsCi.gccPdu.clientMultitransportChannelData.flags.HasFlag(MULTITRANSPORT_TYPE_FLAGS.TRANSPORTTYPE_UDPFECR));
                detectInfo.IsSupportTransportTypeUdpFECL = (mscConnectionInitialPDU.mcsCi.gccPdu.clientMultitransportChannelData.flags.HasFlag(MULTITRANSPORT_TYPE_FLAGS.TRANSPORTTYPE_UDPFECL));
            }
            else
            {
                detectInfo.IsSupportTransportTypeUdpFECR = false;
                detectInfo.IsSupportTransportTypeUdpFECL = false;
            }

            if (mscConnectionInitialPDU.mcsCi.gccPdu.clientClusterData != null)
            {
                detectInfo.IsSupportServerRedirection = (mscConnectionInitialPDU.mcsCi.gccPdu.clientClusterData.Flags.HasFlag(Flags_Values.REDIRECTION_SUPPORTED));
            }
            else
            {
                detectInfo.IsSupportServerRedirection = false;
            }

            if (mscConnectionInitialPDU.mcsCi.gccPdu.clientNetworkData != null && mscConnectionInitialPDU.mcsCi.gccPdu.clientNetworkData.channelCount > 0)
            {
                detectInfo.IsSupportStaticVirtualChannel = true;
            }
            else
            {
                detectInfo.IsSupportStaticVirtualChannel = false;
            }

            TS_GENERAL_CAPABILITYSET generalCapSet = (TS_GENERAL_CAPABILITYSET)this.clientCapSet.FindCapSet(capabilitySetType_Values.CAPSTYPE_GENERAL);
            if (generalCapSet.extraFlags.HasFlag(extraFlags_Values.AUTORECONNECT_SUPPORTED))
            {
                detectInfo.IsSupportAutoReconnect = true;
            }
            else
            {
                detectInfo.IsSupportAutoReconnect = false;
            }

            detectInfo.IsSupportRDPRFX = false;
            ITsCapsSet codecCapSet = this.clientCapSet.FindCapSet(capabilitySetType_Values.CAPSETTYPE_BITMAP_CODECS);
            if (codecCapSet != null)
            {
                foreach (TS_BITMAPCODEC codec in ((TS_BITMAPCODECS_CAPABILITYSET)codecCapSet).supportedBitmapCodecs.bitmapCodecArray)
                {
                    if (is_REMOTEFX_CODEC_GUID(codec.codecGUID))
                    {
                        detectInfo.IsSupportRDPRFX = true;
                        break;
                    }
                }
            }
            if (detectInfo.IsSupportRDPRFX.Value)
            {
                ITsCapsSet surfcmdCapSet = this.clientCapSet.FindCapSet(capabilitySetType_Values.CAPSETTYPE_SURFACE_COMMANDS);
                if (!((TS_SURFCMDS_CAPABILITYSET)surfcmdCapSet).cmdFlags.HasFlag(CmdFlags_Values.SURFCMDS_STREAMSURFACEBITS))
                {
                    detectInfo.IsSupportRDPRFX = false;
                }
            }
            // Notify the UI for detecting feature finished
            logWriter.AddLog(DetectLogLevel.Warning, "Finished", false, LogStyle.StepPassed);
        }

        private void CheckSupportedProtocols()
        {
            logWriter.AddLog(DetectLogLevel.Information, "Check specified protocols support...");

            detectInfo.IsSupportRDPEFS = false;
            if (mscConnectionInitialPDU.mcsCi.gccPdu.clientNetworkData != null && mscConnectionInitialPDU.mcsCi.gccPdu.clientNetworkData.channelCount > 0)
            {
                List<CHANNEL_DEF> channels = mscConnectionInitialPDU.mcsCi.gccPdu.clientNetworkData.channelDefArray;
                foreach (CHANNEL_DEF channel in channels)
                {
                    if (channel.name.ToUpper().Contains("RDPDR"))
                    {
                        detectInfo.IsSupportRDPEFS = true;
                        break;
                    }
                }
            }

            // Create Dynamic Virtual Channels to detect protocols supported
            detectInfo.IsSupportRDPEDISP = (CreateEDYCChannel(RDPDetector.RdpedispChannelName));

            if (detectInfo.IsSupportRDPEGFX != null && detectInfo.IsSupportRDPEGFX.Value)
            {
                detectInfo.IsSupportRDPEGFX = CreateEDYCChannel(RDPDetector.RdpegfxChannelName);
            }

            detectInfo.IsSupportRDPEI = (CreateEDYCChannel(RDPDetector.rdpeiChannelName));

            detectInfo.IsSupportRDPEUSB = (CreateEDYCChannel(RDPDetector.RdpeusbChannelName));

            detectInfo.IsSupportRDPEVOR = (CreateEDYCChannel(RDPDetector.RdpegtChannelName)
                && CreateEDYCChannel(RDPDetector.RdpevorControlChannelName)
                && CreateEDYCChannel(RDPDetector.RdpevorDataChannelName));

            detectInfo.IsSupportRDPEMT = false;
            detectInfo.IsSupportRDPEUDP = false;
            detectInfo.IsSupportRDPEUDP2 = false;
            if (detectInfo.IsSupportStaticVirtualChannel != null && detectInfo.IsSupportStaticVirtualChannel.Value
                && ((detectInfo.IsSupportTransportTypeUdpFECR != null && detectInfo.IsSupportTransportTypeUdpFECR.Value)
                || (detectInfo.IsSupportTransportTypeUdpFECL != null && detectInfo.IsSupportTransportTypeUdpFECL.Value)))
            {
                detectInfo.IsSupportRDPEMT = true;
                detectInfo.IsSupportRDPEUDP = true;
                detectInfo.IsSupportRDPEUDP2 = CheckSupportForRDPEUDP2(detectInfo);
            }

            // Notify the UI for detecting protocol supported finished
            logWriter.AddLog(DetectLogLevel.Warning, "Finished", false, LogStyle.StepPassed);
            logWriter.AddLog(DetectLogLevel.Information, "Check specified protocols support finished.");
        }

        private bool CheckSupportForRDPEUDP2(DetectionInfo detectInfo)
        {
            var result = false;

            logWriter.AddLog(DetectLogLevel.Information, "Checking for RDPEUDP2 support");

            // Start UDP listening.
            var autoHandle = false;
            var endPoint = new IPEndPoint(IPAddress.Parse(LocalIPAddress()), detectInfo.RDPServerPort);
            logWriter.AddLog(DetectLogLevel.Information, $"IP, Port -> {endPoint.ToString()}, {detectInfo.RDPServerPort}");

            var rdpeudpServer = new RdpeudpServer(endPoint, autoHandle);
            rdpeudpServer.UnhandledExceptionReceived += (ex) =>
            {
                logWriter.AddLog(DetectLogLevel.Warning, $"Unhandled exception from RdpeudpServer: {ex}");
            }; 
            rdpeudpServer.Start();

            // Create a UDP socket.
            uint requestId = 1;
            var udpTransportMode = TransportMode.Reliable;

            // Send a Server Initiate Multitransport Request PDU.
            byte[] securityCookie = new byte[16];
            Random rnd = new Random();
            rnd.NextBytes(securityCookie);
            Multitransport_Protocol_value requestedProtocol = Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECR;
            if (udpTransportMode == TransportMode.Lossy)
            {
                requestedProtocol = Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECL;
            }

            Server_Initiate_Multitransport_Request_PDU requestPDU = rdpbcgrServerStack.CreateServerInitiateMultitransportRequestPDU(sessionContext, requestId, requestedProtocol, securityCookie);
            SendPdu(requestPDU);

            var sutIP = GetHostIP(detectInfo.SUTName);
            logWriter.AddLog(DetectLogLevel.Information, $"IP 2 is {sutIP.ToString()}");
            logWriter.AddLog(DetectLogLevel.Information, $"UDP transport mode is {udpTransportMode}");
            RdpeudpServerSocket rdpeudpSocket = rdpeudpServer.CreateSocket(sutIP, udpTransportMode, timeout);
            if (rdpeudpSocket == null)
            {
                logWriter.AddLog(DetectLogLevel.Warning, $"Failed to create a UDP socket for the Client : {endPoint.Address}"); 
                
                result = false;
            }             
            else
            {
                // Expect a SYN packet.
                RdpeudpPacket synPacket = rdpeudpSocket.ExpectSynPacket(timeout);
                if (synPacket == null)
                {
                    logWriter.AddLog(DetectLogLevel.Warning, "Timeout when waiting for the SYN packet");

                    result = false;
                }
                else
                {
                    // Verify the SYN packet.
                    //Section 3.1.5.1.1: Not appending RDPUDP_SYNDATAEX_PAYLOAD structure implies that RDPUDP_PROTOCOL_VERSION_1 is the highest protocol version supported. 
                    if (synPacket.SynDataEx == null)
                    {
                        logWriter.AddLog(DetectLogLevel.Information, "SynDataEx is null. RDPEUDP2 not supported");

                        result = false;
                    }
                    else
                    {
                        //Supported.
                        logWriter.AddLog(DetectLogLevel.Information, "SynDataEx available. RDPEUDP2 supported");

                        result = true;
                    }
                }
            }

            rdpeudpServer?.Stop();
            rdpeudpSocket?.Close();

            return result;
        }

        private void SetRdpVersion()
        {
            detectInfo.RdpVersion = DetectorUtil.GetPropertyValue("Version");
            if (mscConnectionInitialPDU.mcsCi.gccPdu.clientCoreData != null)
            {
                var rdpVersion = mscConnectionInitialPDU.mcsCi.gccPdu.clientCoreData.version;
                if (rdpVersion == TS_UD_CS_CORE_version_Values.V1)
                {
                    detectInfo.RdpVersion = "4.0";
                }
                else if (rdpVersion == TS_UD_CS_CORE_version_Values.V2)
                {
                    detectInfo.RdpVersion = "8.1";
                }
                else if (rdpVersion == TS_UD_CS_CORE_version_Values.V3)
                {
                    detectInfo.RdpVersion = "10.0";
                }
                else if (rdpVersion == TS_UD_CS_CORE_version_Values.V4)
                {
                    detectInfo.RdpVersion = "10.1";
                }
                else if (rdpVersion == TS_UD_CS_CORE_version_Values.V5)
                {
                    detectInfo.RdpVersion = "10.2";
                }
                else if (rdpVersion == TS_UD_CS_CORE_version_Values.V6)
                {
                    detectInfo.RdpVersion = "10.3";
                }
                else if (rdpVersion == TS_UD_CS_CORE_version_Values.V7)
                {
                    detectInfo.RdpVersion = "10.4";
                }
                else if (rdpVersion == TS_UD_CS_CORE_version_Values.V8)
                {
                    detectInfo.RdpVersion = "10.5";
                }
                else if (rdpVersion == TS_UD_CS_CORE_version_Values.V9)
                {
                    detectInfo.RdpVersion = "10.6";
                }
                else if (rdpVersion == TS_UD_CS_CORE_version_Values.V10)
                {
                    detectInfo.RdpVersion = "10.7";
                }
                else if (rdpVersion == TS_UD_CS_CORE_version_Values.V11)
                {
                    detectInfo.RdpVersion = "10.8";
                }
            }
        }

        /// <summary>
        /// Trigger the client to init a RDP connection
        /// </summary>
        /// <param name="triggerMethod"></param>
        /// <returns></returns>
        private bool triggerClientRDPConnect(TriggerMethod triggerMethod)
        {
            if (triggerMethod == TriggerMethod.Powershell)
            {
                ExecutePowerShellCommand("RDPConnectWithNegotiationApproach");
            }
            else if (triggerMethod == TriggerMethod.Shell)
            {
                ExecuteShellCommand("RDPConnectWithNegotiationApproach");
            }
            else
            {
                TriggerRDPConnectionByProtocol();
            }

            return true;
        }

        /// <summary>
        /// Trigger the client to disconnect all RDP connection
        /// </summary>
        /// <param name="triggerMethod"></param>
        /// <returns></returns>
        private bool TriggerClientDisconnectAll(TriggerMethod triggerMethod)
        {
            if (triggerMethod == TriggerMethod.Powershell)
            {
                int iResult = 0;
                iResult = ExecutePowerShellCommand("TriggerClientDisconnectAll");
                if (iResult <= 0) return false;
            }
            else if (triggerMethod == TriggerMethod.Shell)
            {
                int iResult = 0;
                iResult = ExecuteShellCommand("TriggerClientDisconnectAll");
                if (iResult <= 0) return false;
            }
            else
            {
                TriggerRDPDisconnectAllByProtocol();
            }
            return true;
        }

        /// <summary>
        /// Start RDP listening.
        /// </summary>
        /// <param name="rdpServerPort">RDP server port</param>
        private void StartRDPListening(int rdpServerPort)
        {
            port = rdpServerPort;
            rdpbcgrServerStack = new RdpbcgrServer(port, encryptedProtocol, null);
            rdpbcgrServerStack.Start(IPAddress.Any);
        }

        /// <summary>
        /// Establish RDP Connection
        /// </summary>
        private void EstablishRDPConnection()
        {
            sessionContext = rdpbcgrServerStack.ExpectConnect(timeout);

            #region Connection Initial
            x224ConnectionRequest = ExpectPacket<Client_X_224_Connection_Request_Pdu>(sessionContext, timeout);
            Server_X_224_Connection_Confirm_Pdu confirmPdu
               = rdpbcgrServerStack.CreateX224ConnectionConfirmPdu(sessionContext, selectedProtocols_Values.PROTOCOL_RDP_FLAG, RDP_NEG_RSP_flags_Values.DYNVC_GFX_PROTOCOL_SUPPORTED | RDP_NEG_RSP_flags_Values.EXTENDED_CLIENT_DATA_SUPPORTED);

            SendPdu(confirmPdu);

            if (bool.Parse(detectInfo.IsWindowsImplementation))
            {
                RdpbcgrServerSessionContext orgSession = sessionContext;
                sessionContext = rdpbcgrServerStack.ExpectConnect(timeout);
                if (sessionContext.Identity == orgSession.Identity)
                {
                    sessionContext = rdpbcgrServerStack.ExpectConnect(timeout);
                }
                x224ConnectionRequest = ExpectPacket<Client_X_224_Connection_Request_Pdu>(sessionContext, timeout);
                confirmPdu = rdpbcgrServerStack.CreateX224ConnectionConfirmPdu(sessionContext, selectedProtocols_Values.PROTOCOL_RDP_FLAG, RDP_NEG_RSP_flags_Values.DYNVC_GFX_PROTOCOL_SUPPORTED | RDP_NEG_RSP_flags_Values.EXTENDED_CLIENT_DATA_SUPPORTED);

                SendPdu(confirmPdu);
            }
            #endregion Connection Initial

            #region Basic Setting Exchange

            mscConnectionInitialPDU = ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(sessionContext, timeout);

            SERVER_CERTIFICATE cert = null;
            int certLen = 0;
            int dwKeysize = 2048;

            byte[] privateExp, publicExp, modulus;
            cert = rdpbcgrServerStack.GenerateCertificate(dwKeysize, out privateExp, out publicExp, out modulus);
            certLen = 120 + dwKeysize / 8;

            Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response connectRespPdu = rdpbcgrServerStack.CreateMCSConnectResponsePduWithGCCConferenceCreateResponsePdu(
                    sessionContext,
                    EncryptionMethods.ENCRYPTION_METHOD_128BIT,
                    EncryptionLevel.ENCRYPTION_LEVEL_LOW,
                    cert,
                    certLen,
                    MULTITRANSPORT_TYPE_FLAGS.TRANSPORTTYPE_UDPFECL | MULTITRANSPORT_TYPE_FLAGS.TRANSPORTTYPE_UDPFECR);
            SendPdu(connectRespPdu);

            sessionContext.ServerPrivateExponent = new byte[privateExp.Length];
            Array.Copy(privateExp, sessionContext.ServerPrivateExponent, privateExp.Length);

            #endregion Basic Setting Exchange

            #region Channel Connection

            ExpectPacket<Client_MCS_Erect_Domain_Request>(sessionContext, timeout);
            ExpectPacket<Client_MCS_Attach_User_Request>(sessionContext, timeout);

            Server_MCS_Attach_User_Confirm_Pdu attachUserConfirmPdu = rdpbcgrServerStack.CreateMCSAttachUserConfirmPdu(sessionContext);
            SendPdu(attachUserConfirmPdu);

            //Join Channel
            int channelNum = 2;
            if (sessionContext.VirtualChannelIdStore != null)
            {
                channelNum += sessionContext.VirtualChannelIdStore.Length;
            }
            if (sessionContext.IsServerMessageChannelDataSend)
                channelNum++;
            for (int i = 0; i < channelNum; i++)
            {
                Client_MCS_Channel_Join_Request channelJoinPdu = ExpectPacket<Client_MCS_Channel_Join_Request>(sessionContext, timeout);
                Server_MCS_Channel_Join_Confirm_Pdu channelJoinResponse = rdpbcgrServerStack.CreateMCSChannelJoinConfirmPdu(
                        sessionContext,
                        channelJoinPdu.mcsChannelId);
                SendPdu(channelJoinResponse);
            }
            #endregion Channel Connection

            #region RDP Security Commencement

            securityExchangePDU = ExpectPacket<Client_Security_Exchange_Pdu>(sessionContext, timeout);

            #endregion RDP Security Commencement

            #region Secure Setting Exchange
            clientInfoPDU = ExpectPacket<Client_Info_Pdu>(sessionContext, timeout);
            #endregion Secure Setting Exchange

            #region Licensing
            Server_License_Error_Pdu_Valid_Client licensePdu = rdpbcgrServerStack.CreateLicenseErrorMessage(sessionContext);
            SendPdu(licensePdu);
            #endregion Licensing

            #region Capabilities Exchange

            RdpbcgrCapSet capSet = new RdpbcgrCapSet();
            capSet.GenerateCapabilitySets();
            Server_Demand_Active_Pdu demandActivePdu = rdpbcgrServerStack.CreateDemandActivePdu(sessionContext, capSet.CapabilitySets);
            SendPdu(demandActivePdu);

            confirmActivePDU = ExpectPacket<Client_Confirm_Active_Pdu>(sessionContext, timeout);
            clientCapSet = new RdpbcgrCapSet();
            clientCapSet.CapabilitySets = confirmActivePDU.confirmActivePduData.capabilitySets;
            #endregion Capabilities Exchange

            #region Connection Finalization
            ExpectPacket<Client_Synchronize_Pdu>(sessionContext, timeout);

            Server_Synchronize_Pdu synchronizePdu = rdpbcgrServerStack.CreateSynchronizePdu(sessionContext);
            SendPdu(synchronizePdu);

            Server_Control_Pdu controlCooperatePdu = rdpbcgrServerStack.CreateControlCooperatePdu(sessionContext);
            SendPdu(controlCooperatePdu);

            ExpectPacket<Client_Control_Pdu_Cooperate>(sessionContext, timeout);

            ExpectPacket<Client_Control_Pdu_Request_Control>(sessionContext, timeout);

            Server_Control_Pdu controlGrantedPdu = rdpbcgrServerStack.CreateControlGrantedPdu(sessionContext);
            SendPdu(controlGrantedPdu);


            ITsCapsSet cap = this.clientCapSet.FindCapSet(capabilitySetType_Values.CAPSTYPE_BITMAPCACHE_REV2);
            if (cap != null)
            {
                TS_BITMAPCACHE_CAPABILITYSET_REV2 bitmapCacheV2 = (TS_BITMAPCACHE_CAPABILITYSET_REV2)cap;
                if ((bitmapCacheV2.CacheFlags & CacheFlags_Values.PERSISTENT_KEYS_EXPECTED_FLAG) != 0)
                {
                    ExpectPacket<Client_Persistent_Key_List_Pdu>(sessionContext, timeout);
                }
            }

            ExpectPacket<Client_Font_List_Pdu>(sessionContext, timeout);

            Server_Font_Map_Pdu fontMapPdu = rdpbcgrServerStack.CreateFontMapPdu(sessionContext);
            SendPdu(fontMapPdu);

            #endregion Connection Finalization

            // Init for RDPEDYC
            try
            {
                rdpedycServer = new RdpedycServer(rdpbcgrServerStack, sessionContext);
                rdpedycServer.ExchangeCapabilities(timeout);
            }
            catch (Exception)
            {
                rdpedycServer = null;
            }

        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (this.rdpedycServer != null)
            {
                this.rdpedycServer.Dispose();
                this.rdpedycServer = null;
            }
            if (this.rdpbcgrServerStack != null)
            {
                this.rdpbcgrServerStack.Dispose();
                this.rdpbcgrServerStack = null;
            }
        }

        /// <summary>
        /// Create RDPEDYC channel
        /// </summary>
        /// <param name="channelName"></param>
        /// <returns></returns>
        private bool CreateEDYCChannel(string channelName)
        {
            try
            {
                if (rdpedycServer == null)
                {
                    return false;
                }
                rdpedycServer.CreateChannel(timeout, 0, channelName, DynamicVC_TransportType.RDP_TCP);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Expect a packet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="waitTimeSpan"></param>
        /// <returns></returns>
        private T ExpectPacket<T>(RdpbcgrServerSessionContext session, TimeSpan waitTimeSpan) where T : StackPacket
        {
            DateTime endTime = DateTime.Now + waitTimeSpan;
            object receivedPdu = null;
            while (waitTimeSpan.TotalMilliseconds > 0)
            {
                try
                {
                    receivedPdu = this.rdpbcgrServerStack.ExpectPdu(session, waitTimeSpan);
                }
                catch (TimeoutException)
                {
                }
                if (receivedPdu is T)
                {
                    return (T)receivedPdu;
                }
                waitTimeSpan = endTime - DateTime.Now;
            }
            throw new TimeoutException("Timeout when expecting " + typeof(T).Name + " message.");
        }

        /// <summary>
        /// Send a packet
        /// </summary>
        /// <param name="packet"></param>
        private void SendPdu(RdpbcgrServerPdu packet)
        {
            rdpbcgrServerStack.SendPdu(sessionContext, packet);
        }

        /// <summary>
        /// Construct a Parameter structure for Start_RDP_Connection command
        /// </summary>
        /// <param name="localAddress">Local address</param>
        /// <param name="RDPPort">Port for RDP test suite listening</param>
        /// <param name="DirectApproach">true for direct, false for negotiate</param>
        /// <param name="fullScreen">true for full screen, otherwise false</param>
        /// <returns>RDP_Connection_Configure_Parameters structure</returns>
        private RDP_Connection_Configure_Parameters GenerateRDPConnectionConfigParameters(string localAddress, int RDPPort, bool DirectApproach, bool fullScreen)
        {
            RDP_Connection_Configure_Parameters config = new RDP_Connection_Configure_Parameters();
            config.port = (ushort)RDPPort;
            config.address = localAddress;

            config.screenType = RDP_Screen_Type.NORMAL;
            if (fullScreen)
            {
                config.screenType = RDP_Screen_Type.FULL_SCREEN;
            }

            config.connectApproach = RDP_Connect_Approach.Negotiate;
            if (DirectApproach)
            {
                config.connectApproach = RDP_Connect_Approach.Direct;
            }

            config.desktopWidth = 1024;
            config.desktopHeight = 768;

            return config;

        }

        private string LocalIPAddress()
        {
            IPHostEntry host;
            string localIp = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    localIp = ip.ToString();
                    break;
                }
            }

            return localIp;
        }

        private void TriggerRDPConnectionByProtocol()
        {
            RDP_Connection_Payload payLoad = new RDP_Connection_Payload();
            payLoad.type = RDP_Connect_Payload_Type.PARAMETERS_STRUCT;
            payLoad.configureParameters = GenerateRDPConnectionConfigParameters(LocalIPAddress(), port, false, false);
            byte[] payload = payLoad.Encode();

            ushort reqId = ++requestId;
            string helpMessage = "Trigger RDP client to start an RDP connection by SUT Remote Control Protocol.";
            SUT_Control_Request_Message requestMessage = new SUT_Control_Request_Message(SUTControl_TestsuiteId.RDP_TESTSUITE, (ushort)RDPSUTControl_CommandId.START_RDP_CONNECTION, "RDPClientPlugin",
                        reqId, helpMessage, payload);

            TCPSUTControlTransport transport = new TCPSUTControlTransport();

            IPAddress sutIP = GetHostIP(detectInfo.SUTName);

            IPEndPoint agentEndpoint = new IPEndPoint(sutIP, detectInfo.AgentListenPort);

            transport.Connect(timeout, agentEndpoint);
            transport.SendSUTControlRequestMessage(requestMessage);
            transport.Disconnect();
        }

        private void TriggerRDPDisconnectAllByProtocol()
        {
            string helpMessage = "Trigger RDP client to disconnect all connections.";
            byte[] payload = null;
            ushort reqId = ++requestId;
            SUT_Control_Request_Message requestMessage = new SUT_Control_Request_Message(SUTControl_TestsuiteId.RDP_TESTSUITE, (ushort)RDPSUTControl_CommandId.CLOSE_RDP_CONNECTION, "RDPClientPlugin",
               reqId, helpMessage, payload);
            TCPSUTControlTransport transport = new TCPSUTControlTransport();
            IPAddress sutIP = GetHostIP(detectInfo.SUTName);
            IPEndPoint agentEndpoint = new IPEndPoint(sutIP, detectInfo.AgentListenPort);

            transport.Connect(timeout, agentEndpoint);
            transport.SendSUTControlRequestMessage(requestMessage);
            transport.Disconnect();
        }

        private bool is_REMOTEFX_CODEC_GUID(TS_BITMAPCODEC_GUID guidObj)
        {
            //CODEC_GUID_REMOTEFX
            //0x76772F12 BD72 4463 AF B3 B7 3C 9C 6F 78 86
            bool rtnValue;
            rtnValue = (guidObj.codecGUID1 == 0x76772F12) &&
                (guidObj.codecGUID2 == 0xBD72) &&
                (guidObj.codecGUID3 == 0x4463) &&
                (guidObj.codecGUID4 == 0xAF) &&
                (guidObj.codecGUID5 == 0xB3) &&
                (guidObj.codecGUID6 == 0xB7) &&
                (guidObj.codecGUID7 == 0x3C) &&
                (guidObj.codecGUID8 == 0x9C) &&
                (guidObj.codecGUID9 == 0x6F) &&
                (guidObj.codecGUID10 == 0x78) &&
                (guidObj.codecGUID11 == 0x86);

            return rtnValue;
        }

        private void SetPTFVariables(
            SessionStateProxy proxy)
        {
            //set all properties as variables
            var testsiteConfigureValues = GetTestSiteConfigureValues();
            foreach (KeyValuePair<string, string> kvp in testsiteConfigureValues)
            {
                proxy.SetVariable(kvp.Key, kvp.Value);
            }

        }

        /// <summary>
        /// Execute a powershell script file
        /// </summary>
        /// <param name="scriptFile"></param>
        /// <returns></returns>
        private int ExecutePowerShellCommand(string scriptFile)
        {
            string scriptPath = Path.Combine(SUTControlScriptLocation, scriptFile);
            logWriter.AddLog(DetectLogLevel.Information, scriptPath);
            string scriptContent = string.Format(". \"{0}\"", Path.GetFullPath(scriptPath + ".ps1"));
            logWriter.AddLog(DetectLogLevel.Information, scriptContent);

            Runspace runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();

            Pipeline pipeline = runspace.CreatePipeline();

            pipeline.Commands.AddScript("Set-ExecutionPolicy -Scope Process RemoteSigned");
            pipeline.Commands.AddScript("cd " + SUTControlScriptLocation);
            pipeline.Commands.AddScript(scriptContent);

            SessionStateProxy sessionStateProxy = runspace.SessionStateProxy;
            sessionStateProxy.Path.SetLocation(Path.GetDirectoryName(scriptPath));
            SetPTFVariables(sessionStateProxy);
            logWriter.AddLog(DetectLogLevel.Information, sessionStateProxy.GetVariable("PtfProp_SUTName").ToString());
            
            try
            {
               pipeline.Invoke();
            }
            catch (RuntimeException ex)
            {
                string errorMessage = ex.Message;
                string traceInfo = ex.ErrorRecord.InvocationInfo.PositionMessage;
                string ptfAdFailureMessage = string.Format(
                    "Exception thrown in PowerShell Adapter: {0} {1}", errorMessage, traceInfo);
                logWriter.AddLog(DetectLogLevel.Error, ptfAdFailureMessage);
            }

            return 0;
        }

        private int ExecuteShellCommand(string scriptFile)
        {
            string lastOutput = string.Empty;
            StringBuilder errorMsg = new StringBuilder();

            int exitCode = 0;
            try
            {
                string path = Path.Combine(SUTShellControlScriptLocation, scriptFile + ".sh");
                using (Process proc = new Process())
                {
                    string wslPath = "/bin/bash";
                    string winDir = Environment.GetEnvironmentVariable("WINDIR");
                    if (!string.IsNullOrEmpty(winDir))
                    {
                        if (Environment.Is64BitProcess)
                        {
                            wslPath = string.Format(@"{0}\System32\bash.exe", winDir);
                        }
                        else
                        {
                            wslPath = string.Format(@"{0}\Sysnative\bash.exe", winDir);
                        }

                        if (!File.Exists(wslPath))
                        {
                            throw new Exception("Windows Subsystem for Linux (WSL) is not installed.");
                        }
                    }
                    var shellPath = Path.Combine(Environment.CurrentDirectory, path);
                    var shellScriptDirectory = (new FileInfo(shellPath)).Directory.FullName;

                    proc.StartInfo.FileName = wslPath;
                    proc.StartInfo.WorkingDirectory = shellScriptDirectory;
                    proc.StartInfo.Arguments = $"./{scriptFile}.sh";

                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.CreateNoWindow = true;
                    proc.StartInfo.RedirectStandardError = true;
                    proc.StartInfo.RedirectStandardOutput = true;

                    Dictionary<string, string> testSiteParas = GetTestSiteConfigureValues();

                    List<string> wslEnvs = new List<string>();

                    // set ptfconfig properties as environment variables
                    foreach (var kvp in testSiteParas)
                    {
                        if (proc.StartInfo.EnvironmentVariables.ContainsKey(kvp.Key))
                        {
                            proc.StartInfo.EnvironmentVariables.Remove(kvp.Key);
                        }
                        proc.StartInfo.EnvironmentVariables.Add(kvp.Key, kvp.Value);

                        wslEnvs.Add(kvp.Key + "/u");
                    }

                    // Set WSLENV to pass those environment variables into WSL
                    proc.StartInfo.EnvironmentVariables.Add("WSLENV", String.Join(":", wslEnvs));

                    proc.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                    {
                        if (e.Data != null && !String.IsNullOrEmpty(e.Data.Trim()))
                        {
                            if (!String.IsNullOrEmpty(e.Data.Trim()))
                            {
                                lastOutput = e.Data.Trim();
                            }
                        }
                    };
                    proc.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                    {
                        if (e.Data != null)
                        {
                            errorMsg.Append(e.Data);
                        }
                    };
                    proc.Start();
                    proc.BeginOutputReadLine();
                    proc.BeginErrorReadLine();
                    proc.WaitForExit();
                    exitCode = proc.ExitCode;
                    proc.Close();
                }
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                throw ex;
            }
            return exitCode;
        }
        
        private IPAddress GetHostIP(string hostname)
        {
            try
            {
                IPHostEntry host = Dns.GetHostEntry(hostname);
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        logWriter.AddLog(DetectLogLevel.Information, "Parse the host name or the ip address as: " + ip.ToString());
                        return ip;
                    }
                }
            }
            catch (Exception e)
            {
                logWriter.AddLog(DetectLogLevel.Information, "Exception occured when parsing the host name or the ip address: " + e.Message);
                logWriter.AddLog(DetectLogLevel.Information, "" + e.StackTrace);
                if (e.InnerException != null)
                {
                    logWriter.AddLog(DetectLogLevel.Information, "**" + e.InnerException.Message);
                    logWriter.AddLog(DetectLogLevel.Information, "**" + e.InnerException.StackTrace);
                }
                logWriter.AddLog(DetectLogLevel.Warning, "Failed", false, LogStyle.StepFailed);
            }
            return null;

        }

        private Dictionary<string, string> GetTestSiteConfigureValues()
        {
            WriteConfigValuesToLog();
            return new Dictionary<string, string>()
                    {
                        { "PtfProp_SUTName",detectInfo.SUTName},
                        { "PtfProp_SUTUserName",detectInfo.UserNameInTC},
                        { "PtfProp_SUTUserPassword",detectInfo.UserPwdInTC},
                        { "PtfProp_RDPConnectWithNegotiationApproachFullScreen_Task",DetectorUtil.GetPropertyValue("RDPConnectWithNegotiationApproachFullScreen_Task")},
                        { "PtfProp_RDPConnectWithDirectCredSSPFullScreen_Task",DetectorUtil.GetPropertyValue("RDPConnectWithDirectCredSSPFullScreen_Task")},
                        { "PtfProp_RDPConnectWithDirectCredSSP_Task",DetectorUtil.GetPropertyValue("RDPConnectWithDirectCredSSP_Task")},
                        { "PtfProp_RDPConnectWithDirectCredSSPInvalidAccount_Task",DetectorUtil.GetPropertyValue("RDPConnectWithDirectCredSSPInvalidAccount_Task")},
                        { "PtfProp_RDPConnectWithNegotiationApproach_Task",DetectorUtil.GetPropertyValue("RDPConnectWithNegotiationApproach_Task")},
                        { "PtfProp_RDPConnectWithNegotiationApproachInvalidAccount_Task",DetectorUtil.GetPropertyValue("RDPConnectWithNegotiationApproachInvalidAccount_Task")},
                        { "PtfProp_TriggerClientDisconnectAll_Task",DetectorUtil.GetPropertyValue("TriggerClientDisconnectAll_Task")},
                        { "PtfProp_TriggerClientAutoReconnect_Task",DetectorUtil.GetPropertyValue("TriggerClientAutoReconnect_Task")},
                        { "PtfProp_SUTSystemDrive",DetectorUtil.GetPropertyValue("SUTSystemDrive")},
                        { "PtfProp_TriggerInputEvents_Task",DetectorUtil.GetPropertyValue("TriggerInputEvents_Task")},

                    };
        }

        private void WriteConfigValuesToLog()
        {
            logWriter.AddLog(DetectLogLevel.Information, "SUT Name => " +  detectInfo.SUTName);
            logWriter.AddLog(DetectLogLevel.Information, "PtfProp_SUTName => " + detectInfo.SUTName);
            logWriter.AddLog(DetectLogLevel.Information, "PtfProp_SUTUserName => " + detectInfo.UserNameInTC);
            logWriter.AddLog(DetectLogLevel.Information, "PtfProp_SUTUserPassword => " + detectInfo.UserPwdInTC);
            logWriter.AddLog(DetectLogLevel.Information, "PtfProp_RDPConnectWithNegotiationApproachFullScreen_Task => " + DetectorUtil.GetPropertyValue("RDPConnectWithNegotiationApproachFullScreen_Task"));
            logWriter.AddLog(DetectLogLevel.Information, "PtfProp_RDPConnectWithDirectCredSSPFullScreen_Task => " + DetectorUtil.GetPropertyValue("RDPConnectWithDirectCredSSPFullScreen_Task"));
            logWriter.AddLog(DetectLogLevel.Information, "PtfProp_RDPConnectWithDirectCredSSP_Task => " + DetectorUtil.GetPropertyValue("RDPConnectWithDirectCredSSP_Task"));
            logWriter.AddLog(DetectLogLevel.Information, "PtfProp_RDPConnectWithNegotiationApproach_Task => " + DetectorUtil.GetPropertyValue("RDPConnectWithNegotiationApproach_Task"));
            logWriter.AddLog(DetectLogLevel.Information, "PtfProp_TriggerClientDisconnectAll_Task => " + DetectorUtil.GetPropertyValue("TriggerClientDisconnectAll_Task"));
            logWriter.AddLog(DetectLogLevel.Information, "PtfProp_TriggerClientAutoReconnect_Task => " + DetectorUtil.GetPropertyValue("TriggerClientAutoReconnect_Task"));
            logWriter.AddLog(DetectLogLevel.Information, "PtfProp_SUTSystemDrive => " + DetectorUtil.GetPropertyValue("SUTSystemDrive"));
            logWriter.AddLog(DetectLogLevel.Information, "PtfProp_TriggerInputEvents_Task => " + DetectorUtil.GetPropertyValue("TriggerInputEvents_Task"));
        }

        #endregion Methods
    }
}
