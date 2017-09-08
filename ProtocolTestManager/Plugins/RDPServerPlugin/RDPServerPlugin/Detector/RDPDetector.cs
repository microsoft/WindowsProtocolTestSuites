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


namespace Microsoft.Protocols.TestManager.RDPServerPlugin
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
       
        public const string SystemManagementAutomationAssemblyNameV1 =
           "System.Management.Automation, Version=1.0.0.0, Culture=neutral, " +
           "PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL";
        public const string SystemManagementAutomationAssemblyNameV3 =
            "System.Management.Automation, Version=3.0.0.0, Culture=neutral, " +
            "PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL";
        public const string SUTControlScriptLocation = @"..\etc\RDP\SUTControlAdapter\";
        #endregion Variables
        #region Constructor

        public RDPDetector(DetectionInfo detectInfo)
        {
            this.detectInfo = detectInfo;
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
                StartRDPListening();
                triggerClientRDPConnect(detectInfo.TriggerMethod);
                EstablishRDPConnection();
            }
            catch (Exception e)
            {
                DetectorUtil.WriteLog("Exception occured when establishing RDP connection: " + e.Message);
                DetectorUtil.WriteLog(""+e.StackTrace);
                if (e.InnerException != null)
                {
                    DetectorUtil.WriteLog("**" + e.InnerException.Message);
                    DetectorUtil.WriteLog("**" + e.InnerException.StackTrace);
                }
                DetectorUtil.WriteLog("Failed", false, LogStyle.StepFailed);
                return false;
            }

            // Notify the UI for establishing RDP connection successfully.
            DetectorUtil.WriteLog("Passed", false, LogStyle.StepPassed);

            // Set result according to messages during connection
            if (mscConnectionInitialPDU.mcsCi.gccPdu.clientCoreData != null && mscConnectionInitialPDU.mcsCi.gccPdu.clientCoreData.earlyCapabilityFlags != null)
            {
                detectInfo.IsSupportNetcharAutoDetect = ((mscConnectionInitialPDU.mcsCi.gccPdu.clientCoreData.earlyCapabilityFlags.actualData & (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_NETWORK_AUTODETECT)
                    == (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_NETWORK_AUTODETECT);

                detectInfo.IsSupportHeartbeatPdu = ((mscConnectionInitialPDU.mcsCi.gccPdu.clientCoreData.earlyCapabilityFlags.actualData & (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_HEARTBEAT_PDU)
                    == (ushort)earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_HEARTBEAT_PDU);
            }
            else
            {
                detectInfo.IsSupportNetcharAutoDetect = false;
                detectInfo.IsSupportHeartbeatPdu = false;
            }
            
            // Trigger client to close the RDP connection
            TriggerClientDisconnectAll(detectInfo.TriggerMethod);

            if (detectInfo.IsSupportStaticVirtualChannel != null && detectInfo.IsSupportStaticVirtualChannel.Value)
            {
                detectInfo.IsSupportRDPEUDP = true;
            }
            else
            {
                detectInfo.IsSupportRDPEUDP = false;
            }
            // Notify the UI for detecting protocol supported finished
            DetectorUtil.WriteLog("Passed", false, LogStyle.StepPassed);

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

            return true;
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
            else if (triggerMethod == TriggerMethod.Manual)
            {
                MessageBox.Show("Please Trigger Client RDP Connetion Manually.");
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
            else if (triggerMethod == TriggerMethod.Manual)
            {
                MessageBox.Show("Please Close All RDP Connetion Manually on SUT.");
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
        private void StartRDPListening()
        {
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
            
            securityExchangePDU =  ExpectPacket<Client_Security_Exchange_Pdu>(sessionContext, timeout);

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
        private T ExpectPacket<T>(RdpbcgrServerSessionContext session, TimeSpan waitTimeSpan) where T: StackPacket
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
            throw new TimeoutException("Timeout when expecting "+typeof(T).Name +" message.");
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
            SUT_Control_Request_Message requestMessage = new SUT_Control_Request_Message(SUTControl_TestsuiteId.RDP_TESTSUITE, (ushort)RDPSUTControl_CommandId.START_RDP_CONNECTION,
                        reqId, helpMessage, payload);

            TCPSUTControlTransport transport = new TCPSUTControlTransport();
            IPEndPoint agentEndpoint = new IPEndPoint(IPAddress.Parse(detectInfo.SUTName), detectInfo.AgentListenPort);
            
            transport.Connect(timeout, agentEndpoint);
            transport.SendSUTControlRequestMessage(requestMessage);
            transport.Disconnect();
        }

        private void TriggerRDPDisconnectAllByProtocol()
        {
            string helpMessage = "Trigger RDP client to disconnect all connections.";
            byte[] payload = null;
            ushort reqId = ++requestId;
            SUT_Control_Request_Message requestMessage = new SUT_Control_Request_Message(SUTControl_TestsuiteId.RDP_TESTSUITE, (ushort)RDPSUTControl_CommandId.CLOSE_RDP_CONNECTION,
               reqId, helpMessage, payload);
            TCPSUTControlTransport transport = new TCPSUTControlTransport();
            IPEndPoint agentEndpoint = new IPEndPoint(IPAddress.Parse(detectInfo.SUTName), detectInfo.AgentListenPort);
            
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

        /// <summary>
        /// Execute a powershell script file
        /// </summary>
        /// <param name="scriptFile"></param>
        /// <returns></returns>
        private int ExecutePowerShellCommand(string scriptFile)
        {
            string scriptPath = SUTControlScriptLocation + scriptFile;

            Assembly sysMgmtAutoAssembly = null;
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod;
            try
            {
                sysMgmtAutoAssembly = Assembly.Load(SystemManagementAutomationAssemblyNameV3);
            }
            catch { }
            // If loading System.Management.Automation, Version=3.0.0.0 failed, try Version=1.0.0.0
            if (sysMgmtAutoAssembly == null)
            {
                try
                {
                    sysMgmtAutoAssembly = Assembly.Load(SystemManagementAutomationAssemblyNameV1);
                }
                catch
                {
                    throw new InvalidOperationException("Can not find system management automation assembly from GAC." +
                                                        "Please make sure your PowerShell installation is valid." +
                                                        "Or you need to reinstall PowerShell.");
                }
            }

            Type runspaceConfigurationType = sysMgmtAutoAssembly.GetType("System.Management.Automation.Runspaces.RunspaceConfiguration");
            object runspaceConfigurationInstance = runspaceConfigurationType.InvokeMember("Create", BindingFlags.InvokeMethod, null, null, null);

            Type runspaceFactoryType = sysMgmtAutoAssembly.GetType("System.Management.Automation.Runspaces.RunspaceFactory");
            Type runspaceType = sysMgmtAutoAssembly.GetType("System.Management.Automation.Runspaces.Runspace");

            object runspaceInstance = runspaceFactoryType.InvokeMember("CreateRunspace", BindingFlags.InvokeMethod, null, null, new object[] { runspaceConfigurationInstance });

            runspaceType.InvokeMember("Open", flag, null, runspaceInstance, null);

            Type sessionStateProxyType = sysMgmtAutoAssembly.GetType("System.Management.Automation.Runspaces.SessionStateProxy");
            object proxyInstance = runspaceType.InvokeMember(
                    "SessionStateProxy",
                    BindingFlags.GetProperty,
                    null,
                    runspaceInstance,
                    null);

            // Set Variables
            MethodInfo methodSetVariable = sessionStateProxyType.GetMethod(
                "SetVariable",
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
                null,
                new Type[] { typeof(string), typeof(object) },
                null
                );

            methodSetVariable.Invoke(proxyInstance, new object[] { "PtfPropSUTName", detectInfo.SUTName });
            methodSetVariable.Invoke(proxyInstance, new object[] { "PtfPropSUTUserName", detectInfo.UserNameInTC });
            methodSetVariable.Invoke(proxyInstance, new object[] { "PtfPropSUTUserPassword", detectInfo.UserPwdInTC });
            methodSetVariable.Invoke(proxyInstance, new object[] { "PtfPropRDPConnectWithNegotiationAppoachFullScreen_Task", DetectorUtil.GetPropertyValue("RDPConnectWithNegotiationAppoachFullScreen_Task") });
            methodSetVariable.Invoke(proxyInstance, new object[] { "PtfPropRDPConnectWithDrectCredSSPFullScreen_Task", DetectorUtil.GetPropertyValue("RDPConnectWithDrectCredSSPFullScreen_Task") }); methodSetVariable.Invoke(proxyInstance, new object[] { "PtfPropRDPConnectWithDrectCredSSP_Task", DetectorUtil.GetPropertyValue("RDPConnectWithDrectCredSSP_Task") });
            methodSetVariable.Invoke(proxyInstance, new object[] { "PtfPropRDPConnectWithDrectTLS_Task", DetectorUtil.GetPropertyValue("RDPConnectWithDrectTLS_Task") });
            methodSetVariable.Invoke(proxyInstance, new object[] { "PtfPropRDPConnectWithDrectTLSFullScreen_Task", DetectorUtil.GetPropertyValue("RDPConnectWithDrectTLSFullScreen_Task") });
            methodSetVariable.Invoke(proxyInstance, new object[] { "PtfPropRDPConnectWithNegotiationApproach_Task", DetectorUtil.GetPropertyValue("RDPConnectWithNegotiationApproach_Task") });
            methodSetVariable.Invoke(proxyInstance, new object[] { "PtfPropTriggerClientDisconnectAll_Task", DetectorUtil.GetPropertyValue("TriggerClientDisconnectAll_Task") });
            methodSetVariable.Invoke(proxyInstance, new object[] { "PtfPropTriggerClientAutoReconnect_Task", DetectorUtil.GetPropertyValue("TriggerClientAutoReconnect_Task") });
            methodSetVariable.Invoke(proxyInstance, new object[] { "PtfPropSUTSystemDrive", DetectorUtil.GetPropertyValue("SUTSystemDrive") });
            methodSetVariable.Invoke(proxyInstance, new object[] { "PtfPropTriggerInputEvents_Task", DetectorUtil.GetPropertyValue("TriggerInputEvents_Task") });

            Type pipelineType = sysMgmtAutoAssembly.GetType("System.Management.Automation.Runspaces.Pipeline");
            object pipelineInstance = runspaceType.InvokeMember(
                    "CreatePipeline", flag, null, runspaceInstance, null);

            Type RunspaceInvokeType = sysMgmtAutoAssembly.GetType("System.Management.Automation.RunspaceInvoke");
            object scriptInvoker = Activator.CreateInstance(RunspaceInvokeType);
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            RunspaceInvokeType.InvokeMember("Invoke", flag, null, scriptInvoker, new object[] { "Set-ExecutionPolicy Unrestricted" });

            object CommandCollectionInstance = pipelineType.InvokeMember("Commands", BindingFlags.GetProperty, null, pipelineInstance, null);
            Type CommandCollectionType = sysMgmtAutoAssembly.GetType("System.Management.Automation.Runspaces.CommandCollection");

            CommandCollectionType.InvokeMember(
                "AddScript", flag, null, CommandCollectionInstance, new object[] { "cd " + SUTControlScriptLocation });

            Type CommandType = sysMgmtAutoAssembly.GetType("System.Management.Automation.Runspaces.Command");
            object myCommand = Activator.CreateInstance(CommandType, scriptPath);
            CommandCollectionType.InvokeMember(
                "Add", flag, null, CommandCollectionInstance, new object[] { myCommand });
            
            pipelineType.InvokeMember("Invoke", flag, null, pipelineInstance, null);

            runspaceType.InvokeMember("Close", flag, null, runspaceInstance, null);


            return 0;
        }

        #endregion Methods
    }
}
