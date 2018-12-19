// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeusb;
using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.Protocols.TestSuites.Rdpbcgr;
using Microsoft.Protocols.TestSuites.Rdpeusb;

namespace Microsoft.Protocols.TestSuites.Rdpeusb
{
    [TestClass]
    public partial class RdpeusbTestSutie : RdpTestClassBase
    {
        private IRdpeusbAdapter rdpeusbAdapter;
        private EusbTestContext context = new EusbTestContext();
        private RdpedycServer rdpedycServer;

        #region Class Initialization and Cleanup
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            RdpTestClassBase.BaseInitialize(context);
            //Config.LoadConfig(TestClassBase.BaseTestSite);
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

            if (null == rdpeusbAdapter)
            {
                rdpeusbAdapter = Site.GetAdapter<IRdpeusbAdapter>();
                ReqCapturer.Site = Site;
                ReqCapturer.NeedVerify = true;
            }
            else
            {
                rdpeusbAdapter.Reset();
                context = new EusbTestContext();
            }

            this.rdpbcgrAdapter.TurnVerificationOff(true);

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            StartRDPConnection();
        }

        protected override void TestCleanup()
        {
            // TestCleanup() may be not main thread
            DynamicVCException.SetCleanUp(true);

            base.TestCleanup();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Dispose virtual channel manager.");
            if (rdpedycServer != null)
                rdpedycServer.Dispose();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Stop RDP listening.");
            this.rdpbcgrAdapter.StopRDPListening();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Trigger client to close all RDP connections for clean up.");
            int iResult = this.sutControlAdapter.TriggerClientDisconnectAll(this.TestContext.TestName);
            this.TestSite.Log.Add(LogEntryKind.Debug, "The result of TriggerClientDisconnectAll is {0}.", iResult);

            DynamicVCException.SetCleanUp(false);
        }
        
        #endregion

        #region Private Methods

        //Set default server capabilities
        private void setServerCapabilitiesWithRemoteFxSupported()
        {
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true,
               true, maxRequestSize,
               true,
               true, 1,
               true, CmdFlags_Values.SURFCMDS_FRAMEMARKER | CmdFlags_Values.SURFCMDS_SETSURFACEBITS | CmdFlags_Values.SURFCMDS_STREAMSURFACEBITS,
               true, true, true);
        }

        //Start RDP connection.
        private void StartRDPConnection()
        {

            #region Trigger client to connect
            //Trigger client to connect. 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            #region RDPBCGR Connection

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with RomoteFX codec supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            setServerCapabilitiesWithRemoteFxSupported();

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            #endregion

            rdpedycServer = new RdpedycServer(this.rdpbcgrAdapter.ServerStack, this.rdpbcgrAdapter.SessionContext);
            rdpedycServer.ExchangeCapabilities(waitTime);
            this.rdpeusbAdapter.ProtocolInitialize(rdpedycServer);
        }

        //Stop RDP connection.
        private void StopRDPConnection()
        {
            int iResult = this.sutControlAdapter.TriggerClientDisconnectAll(this.TestContext.TestName);
            this.TestSite.Log.Add(LogEntryKind.Debug, "The result of TriggerClientDisconnectAll is {0}.", iResult);
            this.rdpbcgrAdapter.Reset();
            this.rdpeusbAdapter.Reset();
        }

        #endregion

        #region Test Helper Methods

        private DynamicVirtualChannel CreateVirtualChannel()
        {
            // Creates the required dynamic virtual channel.
            DynamicVirtualChannel channel = rdpeusbAdapter.CreateVirtualChannel();

            // Exchanges capabilities.
            rdpeusbAdapter.NegotiateCapability(channel, IdGenerator.NewId(), CapabilityValue_Values.RIM_CAPABILITY_VERSION_01);

            // Notifies the channel has been created.
            rdpeusbAdapter.ChannelCreated(channel, IdGenerator.NewId(), 1, 0, 0);

            return channel;
        }

        private USB_STRING_DESCRIPTOR ParseStringDescriptor(EusbPdu pdu)
        {
            if (pdu is EusbUrbCompletionNoDataPdu)
            {
                Site.Log.Add(
                    LogEntryKind.Debug, 
                    "Unexpectedly received an EusbUrbCompletionNoDataPdu message: {0}", 
                    pdu
                    );
                return null;
            }

            Site.Assert.IsInstanceOfType(
                pdu,
                typeof(EusbUrbCompletionPdu),
                "Must receive an EusbUrbCompletionPdu message."
                );

            EusbUrbCompletionPdu completionPdu = (EusbUrbCompletionPdu)pdu;

            Site.Assert.IsSuccess(
                (int)completionPdu.HResult,
                "the HResult member of the EusbUrbCompletionPdu must be a successful code."
                );

            USB_STRING_DESCRIPTOR res = UsbStructParser.Parse<USB_STRING_DESCRIPTOR>(completionPdu);

            Site.Assert.IsNotNull(res, "USB_STRING_DESCRIPTOR cannot be parsed from EusbUrbCompletionPdu");
            return res;
        }

        private uint ParseUInt32IoResult(EusbPdu pdu)
        {
            Site.Assert.IsInstanceOfType(
                pdu,
                typeof(EusbIoControlCompletionPdu),
                "Must receive an EusbIoControlCompletionPdu message."
                );

            EusbIoControlCompletionPdu completionPdu = (EusbIoControlCompletionPdu)pdu;

            Site.Assert.IsSuccess(
                (int)completionPdu.HResult,
                "the HResult member of the EusbIoControlCompletionPdu must be a successful code."
                );

            Site.Assert.AreEqual<uint>(
                4,
                completionPdu.OutputBufferSize,
                "If the operation is successful, the client MUST set the OutputBufferSize field to 0x4."
                );

            uint res = BitConverter.ToUInt32(completionPdu.OutputBuffer, 0);

            return res;
        }

        private void LogComment(string comment, params object[] args)
        {
            Site.Log.Add(LogEntryKind.Comment, comment, args);
        }

        private bool SelectConfiguration(EusbDeviceContext device, byte configIndex)
        {
            // 6. Sends TS_URB_CONTROL_DESCRIPTOR_REQUEST with the descriptor type of USB_DEVICE_DESCRIPTOR_TYPE.
            uint requestId = IdGenerator.NewId();
            TS_URB_CONTROL_DESCRIPTOR_REQUEST des = new UrbBuilder(
                URB_FUNCTIONID.URB_FUNCTION_GET_DESCRIPTOR_FROM_DEVICE, 
                requestId,
                0).BuildDeviceDescriptorRequest();
            rdpeusbAdapter.TransferInRequest(device, des, USB_DEVICE_DESCRIPTOR.DefaultSize);

            // 7. Receives a completion message with the result for USB_DEVICE_DESCRIPTOR.");
            EusbUrbCompletionPdu pdu = (EusbUrbCompletionPdu)rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            if (null == pdu || pdu.HResult != (uint)HRESULT_FROM_WIN32.ERROR_SUCCESS)
            {
                return false;
            }
            USB_DEVICE_DESCRIPTOR desDevice = UsbStructParser.Parse<USB_DEVICE_DESCRIPTOR>(pdu);

            // 8. Sends TS_URB_CONTROL_DESCRIPTOR_REQUEST to retrieve the total length of the configuration.
            requestId = IdGenerator.NewId();
            des = new UrbBuilder(
                URB_FUNCTIONID.URB_FUNCTION_GET_DESCRIPTOR_FROM_DEVICE,
                requestId,
                0).BuildConfigurationDescriptorRequest(configIndex);
            rdpeusbAdapter.TransferInRequest(device, des, USB_CONFIGURATION_DESCRIPTOR.DefaultSize);

            // 9. Receives a completion message with the result for USB_CONFIGURATION_DESCRIPTOR.");
            pdu = (EusbUrbCompletionPdu)rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            if (null == pdu || pdu.HResult != (uint)HRESULT_FROM_WIN32.ERROR_SUCCESS)
            {
                return false;
            }
            USB_CONFIGURATION_DESCRIPTOR desConfig = UsbStructParser.Parse<USB_CONFIGURATION_DESCRIPTOR>(pdu);

            // 10. Sends TS_URB_CONTROL_DESCRIPTOR_REQUEST with the actual length of USB_CONFIGURATION_DESCRIPTOR result.
            requestId = IdGenerator.NewId();
            des = new UrbBuilder(
                URB_FUNCTIONID.URB_FUNCTION_GET_DESCRIPTOR_FROM_DEVICE,
                requestId,
                0).BuildConfigurationDescriptorRequest(configIndex);
            rdpeusbAdapter.TransferInRequest(device, des, desConfig.wTotalLength);

            // 11. Receives a completion message with the complete result for USB_CONFIGURATION_DESCRIPTOR.");
            pdu = (EusbUrbCompletionPdu)rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);

            // 12. Sends TS_URB_SELECT_CONFIGURATION URB request.
            UsbConfigurationParser configParser = new UsbConfigurationParser();
            configParser.ParseAll(pdu);
            requestId = IdGenerator.NewId();
            TS_URB_SELECT_CONFIGURATION sel = new UrbBuilder(
                URB_FUNCTIONID.URB_FUNCTION_GET_DESCRIPTOR_FROM_DEVICE,
                requestId,
                0).BuildSelectConfigRequest(configParser.Interfaces, configParser.configDescriptor);
            rdpeusbAdapter.TransferInRequest(device, sel, 0);

            // 13. Receives a completion message with the result for configuration selection.");
            EusbUrbCompletionNoDataPdu pduRes = (EusbUrbCompletionNoDataPdu)rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            if (null == pduRes || pduRes.HResult != (uint)HRESULT_FROM_WIN32.ERROR_SUCCESS)
            {
                return false;
            }

            TS_URB_SELECT_CONFIGURATION_RESULT urb = new TS_URB_SELECT_CONFIGURATION_RESULT();
            if (!PduMarshaler.Unmarshal(pduRes.TsUrbResult, urb))
            {
                return false;
            }

            context.SelectedConfig = urb;
            return true;
        }

        private bool TrySearchPipe(USBD_PIPE_TYPE[] acceptablePipeTypes, out uint pipeHandle)
        {
            pipeHandle = 0;
            if (null == acceptablePipeTypes
                || null == context.SelectedConfig
                || null == context.SelectedConfig.Interface
                )
            {
                return false;
            }

            List<USBD_PIPE_TYPE> types = new List<USBD_PIPE_TYPE>(acceptablePipeTypes);
            foreach (TS_USBD_INTERFACE_INFORMATION_RESULT inf in context.SelectedConfig.Interface)
	        {
                if (null == inf.Pipes)
                {
                    continue;
                }
                foreach (TS_USBD_PIPE_INFORMATION_RESULT p in inf.Pipes)
                {
                    if (types.Contains((USBD_PIPE_TYPE)p.PipeType))
                    {
                        pipeHandle = p.PipeHandle;
                        return true;
                    }
                }
	        }
            return false;
        }

        #region OSR FX2 Test Data

        private enum OsrFx2VendorCommand : byte
        {
            READ_7_SEGMENT_DISPLAY = 0xD4,
            READ_SWITCHES = 0xD6,
            READ_BARGRAPH_DISPLAY = 0xD7,
            SET_BARGRAPH_DISPLAY = 0xD8,
            IS_HIGH_SPEED = 0xD9,
            SET_7_SEGMENT_DISPLAY = 0xDB
        }

        private byte[] SegmentDisplayStates = { 
                                0x02 | 0x04, // 1 on 7-segment display
                                0x01 | 0x02 | 0x20 | 0x10 | 0x80, // 2 on 7-segment display
                                0x01 | 0x02 | 0x20 | 0x04 | 0x80, // 3 on 7-segment display
                                0x40 | 0x20 | 0x02 | 0x04, // 4 on 7-segment display
                                0x01 | 0x40 | 0x20 | 0x04 | 0x80 // 5 on 7-segment dis
                            };
        
        private byte[] BulkTransferData = { 1, 2, 3, 4, 5 };

        private int InterruptEndpointReadCount = 2;

        private enum EndpointNumber : byte
        {
            InterruptIn = 1,
            BulkOut = 6,
            BulkIn = 8
        }

        #endregion

        #endregion
    }
}
