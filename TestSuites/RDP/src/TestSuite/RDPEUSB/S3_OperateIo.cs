// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeusb;
using System.Threading;

namespace Microsoft.Protocols.TestSuites.Rdpeusb
{
    public partial class RdpeusbTestSutie
    {
        #region BVT Test Cases
                
        [TestMethod]
        [Priority(0)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPEUSB")]
        [Description("Verify behaviors of sending IO control request and related responses.")]
        public void BVT_EUSB_OperateIo_IoControl()
        {
            LogComment("BVT_EUSB_OperateIo_IoControl");

            LogComment("1. Creates the control virtual channel, exchanges capabilities then notifies that the channel is created.");
            context.ControlChannel = CreateVirtualChannel();

            LogComment("2. Receives an add virtual channel request.");
            rdpeusbAdapter.ExpectAddVirtualChannel(context.ControlChannel);

            LogComment("3. Creates a new virtual channel for the device.");
            DynamicVirtualChannel channel = CreateVirtualChannel();

            LogComment("4. Receives an add device request.");
            EusbDeviceContext device = rdpeusbAdapter.ExpectAddDevice(channel);

            LogComment("5. Registers a callback to provide the Request Completion Interface to the client.");
            uint interfaceId = IdGenerator.NewId();
            rdpeusbAdapter.RegisterCallback(device, 1, interfaceId);

            LogComment("6. Sends an IO request.");
            uint requestId = IdGenerator.NewId();
            const uint outputSize = 256;
            rdpeusbAdapter.IoControl(device, UsbIoControlCode.IOCTL_INTERNAL_USB_GET_HUB_NAME, null, outputSize, requestId);

            LogComment("7. Receives a completion message.");
            EusbPdu pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            // TODO: The result should be checked.
            Site.Assert.IsNotNull(pdu, "Must receive a completion message.");

            LogComment("8. Sends retract device request and the channel for the device is expected to be closed.");
            rdpeusbAdapter.RetractDevice(device, USB_RETRACT_REASON.UsbRetractReason_BlockedByPolicy);
        }
                
        [TestMethod]
        [Priority(0)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPEUSB")]
        [Description("Verify behaviors of sending internal IO control request and related responses.")]
        public void BVT_EUSB_OperateIo_InternalIoControl()
        {
            LogComment("BVT_EUSB_OperateIo_InternalIoControl");

            LogComment("1. Creates the control virtual channel, exchanges capabilities then notifies that the channel is created.");
            context.ControlChannel = CreateVirtualChannel();

            LogComment("2. Receives an add virtual channel request.");
            rdpeusbAdapter.ExpectAddVirtualChannel(context.ControlChannel);

            LogComment("3. Creates a new virtual channel for the device.");
            DynamicVirtualChannel channel = CreateVirtualChannel();

            LogComment("4. Receives an add device request.");
            EusbDeviceContext device = rdpeusbAdapter.ExpectAddDevice(channel);

            LogComment("5. Registers a callback to provide the Request Completion Interface to the client.");
            uint interfaceId = IdGenerator.NewId();
            rdpeusbAdapter.RegisterCallback(device, 1, interfaceId);

            LogComment("6. Sends the IOCTL_TSUSBGD_IOCTL_USBDI_QUERY_BUS_TIME internal IO request.");
            uint requestId = IdGenerator.NewId();
            const uint outputSize = 4;
            rdpeusbAdapter.InternalIoControl(
                device, 
                UsbInternalIoControlCode.IOCTL_TSUSBGD_IOCTL_USBDI_QUERY_BUS_TIME, 
                null, 
                outputSize, 
                requestId);

            LogComment("7. Receives a completion message.");
            EusbPdu pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            uint frameNum = ParseUInt32IoResult(pdu);
            
            LogComment("The current frame number is {0}", frameNum);

            LogComment("8. Sends retract device request and the channel for the device is expected to be closed.");
            rdpeusbAdapter.RetractDevice(device, USB_RETRACT_REASON.UsbRetractReason_BlockedByPolicy);
        }
                
        [TestMethod]
        [Priority(0)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPEUSB")]
        [Description("Verify behaviors of sending transfer in request and related responses.")]
        public void BVT_EUSB_OperateIo_TransferIn()
        {
            LogComment("BVT_EUSB_OperateIo_TransferIn");

            LogComment("1. Creates the control virtual channel, exchanges capabilities then notifies that the channel is created.");
            context.ControlChannel = CreateVirtualChannel();

            LogComment("2. Receives an add virtual channel request.");
            rdpeusbAdapter.ExpectAddVirtualChannel(context.ControlChannel);

            LogComment("3. Creates a new virtual channel for the device.");
            DynamicVirtualChannel channel = CreateVirtualChannel();

            LogComment("4. Receives an add device request.");
            EusbDeviceContext device = rdpeusbAdapter.ExpectAddDevice(channel);

            LogComment("5. Registers a callback to provide the Request Completion Interface to the client.");
            uint interfaceId = IdGenerator.NewId();
            rdpeusbAdapter.RegisterCallback(device, 1, interfaceId);

            LogComment("6. Sends TS_URB_CONTROL_DESCRIPTOR_REQUEST passing the buffer size as the size of " +
                "USB_STRING_DESCRIPTOR a transfer in request.");
            uint requestId = IdGenerator.NewId();
            TS_URB_CONTROL_DESCRIPTOR_REQUEST des = new UrbBuilder(
                URB_FUNCTIONID.URB_FUNCTION_GET_DESCRIPTOR_FROM_DEVICE,
                requestId,
                0).BuildStringDescriptorRequest(1);
            rdpeusbAdapter.TransferInRequest(device, des, USB_STRING_DESCRIPTOR.DefaultSize);

            LogComment("7. Receives a completion message to retrieve the descriptor size specified as the " +
                "member bLength in the USB_STRING_DESCRIPTOR.");
            EusbPdu pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            ReqCapturer.VerifyUrbCompletion((EusbUrbCompletionPdu)pdu, des, interfaceId);
            USB_STRING_DESCRIPTOR res = ParseStringDescriptor(pdu);

            LogComment("8. Sends TS_URB_CONTROL_DESCRIPTOR_REQUEST with the buffer size as bLength.");
            rdpeusbAdapter.TransferInRequest(device, des, res.bLength);

            LogComment("9. Receives and parses the descriptor result.");
            pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            ReqCapturer.VerifyUrbCompletion((EusbUrbCompletionPdu)pdu, des, interfaceId);
            res = ParseStringDescriptor(pdu);

            LogComment("The device string descriptor is '{0}'.", res.bString);
            
            LogComment("10. Sends retract device request and the channel for the device is expected to be closed.");
            rdpeusbAdapter.RetractDevice(device, USB_RETRACT_REASON.UsbRetractReason_BlockedByPolicy);
        }
                
        [TestMethod]
        [Priority(0)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPEUSB")]
        [Description("Verify behaviors of sending the query device text request and related responses.")]
        public void BVT_EUSB_OperateIo_QueryDeviceText()
        {
            LogComment("BVT_EUSB_OperateIo_QueryDeviceText");

            LogComment("1. Creates the control virtual channel, exchanges capabilities then notifies that the channel is created.");
            context.ControlChannel = CreateVirtualChannel();

            LogComment("2. Receives an add virtual channel request.");
            rdpeusbAdapter.ExpectAddVirtualChannel(context.ControlChannel);

            LogComment("3. Creates a new virtual channel for the device.");
            DynamicVirtualChannel channel = CreateVirtualChannel();

            LogComment("4. Receives an add device request.");
            EusbDeviceContext device = rdpeusbAdapter.ExpectAddDevice(channel);

            LogComment("5. Sends QUERY_DEVICE_TEXT request and receives corresponding response.");
            const uint englishLocaleId = 0x0409; // English (U.S.)
            rdpeusbAdapter.QueryDeviceText(device, 0, englishLocaleId);

            LogComment("6. Sends retract device request and the channel for the device is expected to be closed.");
            rdpeusbAdapter.RetractDevice(device, USB_RETRACT_REASON.UsbRetractReason_BlockedByPolicy);
        }

        #endregion

        #region URB Test Cases
                
        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPEUSB")]
        [Description("Verify behaviors of the select-configuration process.")]
        public void S3_EUSB_OperateIo_SelectConfiguration()
        {
            LogComment("S3_EUSB_OperateIo_SelectConfiguration");

            LogComment("1. Creates the control virtual channel, exchanges capabilities then notifies that the channel is created.");
            context.ControlChannel =  CreateVirtualChannel();

            LogComment("2. Receives an add virtual channel request.");
            rdpeusbAdapter.ExpectAddVirtualChannel(context.ControlChannel);

            LogComment("3. Creates a new virtual channel for the device.");
            uint channelId = context.NewChannelId();
            DynamicVirtualChannel channel =  CreateVirtualChannel();

            LogComment("4. Receives an add device request.");
            EusbDeviceContext device = rdpeusbAdapter.ExpectAddDevice(channel);

            LogComment("5. Registers a callback to provide the Request Completion Interface to the client.");
            uint interfaceId = IdGenerator.NewId();
            rdpeusbAdapter.RegisterCallback(device, 1, interfaceId);

            LogComment("6. Sends TS_URB_CONTROL_DESCRIPTOR_REQUEST with the descriptor type of USB_DEVICE_DESCRIPTOR_TYPE.");
            uint requestId = IdGenerator.NewId();
            TS_URB_CONTROL_DESCRIPTOR_REQUEST des = new UrbBuilder(
                URB_FUNCTIONID.URB_FUNCTION_GET_DESCRIPTOR_FROM_DEVICE,
                requestId,
                0).BuildDeviceDescriptorRequest();
            rdpeusbAdapter.TransferInRequest(device, des, USB_DEVICE_DESCRIPTOR.DefaultSize);

            LogComment("7. Receives a completion message with the result for USB_DEVICE_DESCRIPTOR.");
            EusbPdu pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            ReqCapturer.VerifyUrbCompletion((EusbUrbCompletionPdu)pdu, des, interfaceId);
            EusbUrbCompletionPdu completionPdu = (EusbUrbCompletionPdu)pdu;
            USB_DEVICE_DESCRIPTOR desDevice = UsbStructParser.Parse<USB_DEVICE_DESCRIPTOR>(completionPdu);
            
            // A device may support multiple configurations, numbered starting at zero. 
            // Test case will assume the device only have a single configuration.
            Site.Assume.AreEqual<byte>(1, desDevice.bNumConfigurations, "This test case only supports the device with the single configuration.");

            LogComment("Retrieved the device descriptor: {0}", desDevice.ToString());

            LogComment("8. Sends TS_URB_CONTROL_DESCRIPTOR_REQUEST with the descriptor type of USB_CONFIGURATION_DESCRIPTOR_TYPE to query " + 
                "the total length of the configuration.");
            requestId = IdGenerator.NewId();
            des = new UrbBuilder(
                URB_FUNCTIONID.URB_FUNCTION_GET_DESCRIPTOR_FROM_DEVICE, 
                requestId, 
                0).BuildConfigurationDescriptorRequest(0);
            rdpeusbAdapter.TransferInRequest(device, des, USB_CONFIGURATION_DESCRIPTOR.DefaultSize);

            LogComment("9. Receives a completion message with the result for USB_CONFIGURATION_DESCRIPTOR.");
            pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            ReqCapturer.VerifyUrbCompletion((EusbUrbCompletionPdu)pdu, des, interfaceId);
            completionPdu = (EusbUrbCompletionPdu)pdu;
            USB_CONFIGURATION_DESCRIPTOR desConfig = UsbStructParser.Parse<USB_CONFIGURATION_DESCRIPTOR>(completionPdu);

            LogComment("10. Sends TS_URB_CONTROL_DESCRIPTOR_REQUEST with the actual length of USB_CONFIGURATION_DESCRIPTOR result.");
            requestId = IdGenerator.NewId();
            des = new UrbBuilder(
                URB_FUNCTIONID.URB_FUNCTION_GET_DESCRIPTOR_FROM_DEVICE,
                requestId,
                0).BuildConfigurationDescriptorRequest(0);
            rdpeusbAdapter.TransferInRequest(device, des, desConfig.wTotalLength);

            LogComment("11. Receives a completion message with the complete result for USB_CONFIGURATION_DESCRIPTOR.");
            pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            ReqCapturer.VerifyUrbCompletion((EusbUrbCompletionPdu)pdu, des, interfaceId);
            completionPdu = (EusbUrbCompletionPdu)pdu;

            LogComment("12. Sends TS_URB_SELECT_CONFIGURATION URB request.");
            UsbConfigurationParser configParser = new UsbConfigurationParser();
            configParser.ParseAll(completionPdu);
            requestId = IdGenerator.NewId();
            TS_URB_SELECT_CONFIGURATION sel = new UrbBuilder(
                URB_FUNCTIONID.URB_FUNCTION_GET_DESCRIPTOR_FROM_DEVICE,
                requestId,
                0).BuildSelectConfigRequest(configParser.Interfaces, configParser.configDescriptor);
            rdpeusbAdapter.TransferInRequest(device, sel, 0);

            LogComment("13. Receives a completion message with the result for configuration selection.");
            pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            Site.Assert.IsInstanceOfType(
                pdu, 
                typeof(EusbUrbCompletionNoDataPdu), 
                "The result must be type of EusbUrbCompletionNoDataPdu.");
            EusbUrbCompletionNoDataPdu pduRes = (EusbUrbCompletionNoDataPdu)pdu;
            Site.Assert.IsSuccess((int)pduRes.HResult, "The EusbUrbCompletionNoDataPdu must indicate successful.");
            ReqCapturer.VerifyUrbCompletionNoData(pduRes, sel, true, interfaceId);

            TS_URB_SELECT_CONFIGURATION_RESULT urb = new TS_URB_SELECT_CONFIGURATION_RESULT();
            Site.Assert.IsTrue(
                PduMarshaler.Unmarshal(pduRes.TsUrbResult, urb), 
                "The completion PDU must contain the result.");

            LogComment("The configuration-selection result is {0}.", urb);

            LogComment("14. Sends retract device request and the channel for the device is expected to be closed.");
            rdpeusbAdapter.RetractDevice(device, USB_RETRACT_REASON.UsbRetractReason_BlockedByPolicy);
        }
                
        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPEUSB")]
        [TestCategory("DeviceNeeded")]
        [Description("Verify behaviors of selecting interface.")]
        public void S3_EUSB_OperateIo_SelectInterface()
        {
            LogComment("S3_EUSB_OperateIo_SelectInterface");

            LogComment("1. Creates the control virtual channel, exchanges capabilities then notifies that the channel is created.");
            context.ControlChannel =  CreateVirtualChannel();

            LogComment("2. Receives an add virtual channel request.");
            rdpeusbAdapter.ExpectAddVirtualChannel(context.ControlChannel);

            LogComment("3. Creates a new virtual channel for the device.");
            DynamicVirtualChannel channel =  CreateVirtualChannel();

            LogComment("4. Receives an add device request.");
            EusbDeviceContext device = rdpeusbAdapter.ExpectAddDevice(channel);

            LogComment("5. Registers a callback to provide the Request Completion Interface to the client.");
            uint interfaceId = IdGenerator.NewId();
            rdpeusbAdapter.RegisterCallback(device, 1, interfaceId);

            LogComment("6. Select the configuration with index 0.");
            SelectConfiguration(device, 0);

            Site.Assume.IsTrue(context.SelectedConfig.NumInterfaces > 0, "The configuration must contain at least 1 interface to be selected.");

            LogComment("7. Select the interface with index 0 by sending TS_URB_SELECT_INTERFACE request.");
            uint requestId = 0; // TDI, so set to 0
            TS_URB_SELECT_INTERFACE sel = new UrbBuilder(
                URB_FUNCTIONID.URB_FUNCTION_SELECT_INTERFACE,
                requestId,
                0).BuildSelectionInterfaceRequest(context.SelectedConfig, 0);
            rdpeusbAdapter.TransferInRequest(device, sel, 0);

            LogComment("8. Receives a completion message with the result for interface selection.");
            EusbPdu pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            Site.Assert.IsInstanceOfType(
                pdu,
                typeof(EusbUrbCompletionNoDataPdu),
                "The result must be type of EusbUrbCompletionNoDataPdu.");
            EusbUrbCompletionNoDataPdu pduRes = (EusbUrbCompletionNoDataPdu)pdu;
            Site.Assert.IsSuccess((int)pduRes.HResult, "The EusbUrbCompletionNoDataPdu must indicate successful.");
            ReqCapturer.VerifyUrbCompletionNoData(pduRes, sel, true, interfaceId);

            TS_USBD_INTERFACE_INFORMATION_RESULT urb = new TS_USBD_INTERFACE_INFORMATION_RESULT();
            Site.Assert.IsTrue(
                PduMarshaler.Unmarshal(pduRes.TsUrbResult, urb),
                "The completion PDU must contain the result.");

            LogComment("The interface-selection result is {0}.", urb);

            LogComment("9. Sends retract device request and the channel for the device is expected to be closed.");
            rdpeusbAdapter.RetractDevice(device, USB_RETRACT_REASON.UsbRetractReason_BlockedByPolicy);
        }
                
        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPEUSB")]
        [Description("Verify behaviors of clearing a stall condition on an endpoint.")]
        public void S3_EUSB_OperateIo_PipeRequest()
        {
            LogComment("S3_EUSB_OperateIo_PipeRequest");

            LogComment("1. Creates the control virtual channel, exchanges capabilities then notifies that the channel is created.");
            context.ControlChannel = CreateVirtualChannel();

            LogComment("2. Receives an add virtual channel request.");
            rdpeusbAdapter.ExpectAddVirtualChannel(context.ControlChannel);

            LogComment("3. Creates a new virtual channel for the device.");
            DynamicVirtualChannel channel =  CreateVirtualChannel();

            LogComment("4. Receives an add device request.");
            EusbDeviceContext device = rdpeusbAdapter.ExpectAddDevice(channel);

            LogComment("5. Registers a callback to provide the Request Completion Interface to the client.");
            uint interfaceId = IdGenerator.NewId();
            rdpeusbAdapter.RegisterCallback(device, 1, interfaceId);

            LogComment("6. Selects the configuration with index 0.");
            SelectConfiguration(device, 0);

            Site.Assume.IsTrue(
                null != context.SelectedConfig && context.SelectedConfig.NumInterfaces >= 1,
                "The configuration for the device must contain more than one single interface.");
            Site.Assert.IsNotNull(
                context.SelectedConfig.Interface,
                "The number of interfaces specifies {0}, but the interface member is null.", context.SelectedConfig.NumInterfaces);
            Site.Assume.IsTrue(
                context.SelectedConfig.Interface[0].NumberOfPipes >= 1,
                "The interface of the device must contain more than one endpoint.");
            Site.Assert.IsNotNull(
                context.SelectedConfig.Interface[0].Pipes,
                "The number of interfaces specifies {0}, but the interface member is null.", context.SelectedConfig.Interface[0].NumberOfPipes);

            LogComment("7. Searches the first bulk or interrupt pipe handle in the results of the configuration-selection.");
            uint pipeHandle;
            Site.Assume.IsTrue(
                TrySearchPipe(new USBD_PIPE_TYPE[] { USBD_PIPE_TYPE.UsbdPipeTypeBulk, USBD_PIPE_TYPE.UsbdPipeTypeInterrupt }, out pipeHandle),
                "The device must contain a bulk or interrupt pipe."
                );

            LogComment("8. Sends the TS_URB_PIPE_REQUEST with URB_FUNCTION_SYNC_RESET_PIPE and verifies the result.");
            uint requestId = IdGenerator.NewId();
            TS_URB_PIPE_REQUEST req = new UrbBuilder(
                URB_FUNCTIONID.URB_FUNCTION_SYNC_RESET_PIPE,
                requestId,
                0).BuildPipeRequest(pipeHandle);
            rdpeusbAdapter.TransferInRequest(device, req, 0);

            // Waits for the result.
            EusbPdu pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            Site.Assert.IsInstanceOfType(
                pdu,
                typeof(EusbUrbCompletionNoDataPdu),
                "The result must be type of EusbUrbCompletionNoDataPdu.");
            EusbUrbCompletionNoDataPdu pduRes = (EusbUrbCompletionNoDataPdu)pdu;
            Site.Assert.IsSuccess((int)pduRes.HResult, "The EusbUrbCompletionNoDataPdu must indicate successful.");
            ReqCapturer.VerifyUrbCompletionNoData(pduRes, req, true, interfaceId);

            LogComment("8. Sends the TS_URB_PIPE_REQUEST with URB_FUNCTION_ABORT_PIPE and verifies the result.");
            requestId = IdGenerator.NewId();
            req = new UrbBuilder(
                URB_FUNCTIONID.URB_FUNCTION_ABORT_PIPE,
                requestId,
                0).BuildPipeRequest(pipeHandle);
            rdpeusbAdapter.TransferInRequest(device, req, 0);

            // Waits for the result.
            pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            Site.Assert.IsInstanceOfType(
                pdu,
                typeof(EusbUrbCompletionNoDataPdu),
                "The result must be type of EusbUrbCompletionNoDataPdu.");
            pduRes = (EusbUrbCompletionNoDataPdu)pdu;
            Site.Assert.IsSuccess((int)pduRes.HResult, "The EusbUrbCompletionNoDataPdu must indicate successful.");
            ReqCapturer.VerifyUrbCompletionNoData(pduRes, req, true, interfaceId);

            LogComment("9. Sends retract device request and the channel for the device is expected to be closed.");
            rdpeusbAdapter.RetractDevice(device, USB_RETRACT_REASON.UsbRetractReason_BlockedByPolicy);
        }
                
        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPEUSB")]
        [TestCategory("DeviceNeeded")]
        [Description("Verify behaviors of retrieving the current frame number.")]
        public void S3_EUSB_OperateIo_GetCurrentFrameNumber()
        {
            LogComment("S3_EUSB_OperateIo_GetCurrentFrameNumber");

            LogComment("1. Creates the control virtual channel, exchanges capabilities then notifies that the channel is created.");
            context.ControlChannel =  CreateVirtualChannel();

            LogComment("2. Receives an add virtual channel request.");
            rdpeusbAdapter.ExpectAddVirtualChannel(context.ControlChannel);

            LogComment("3. Creates a new virtual channel for the device.");
            DynamicVirtualChannel channel = CreateVirtualChannel();

            LogComment("4. Receives an add device request.");
            EusbDeviceContext device = rdpeusbAdapter.ExpectAddDevice(channel);

            LogComment("5. Registers a callback to provide the Request Completion Interface to the client.");
            uint interfaceId = IdGenerator.NewId();
            rdpeusbAdapter.RegisterCallback(device, 1, interfaceId);

            LogComment("6. Sends TS_URB_GET_CURRENT_FRAME_NUMBER request.");
            const uint frameNumberSize = 0; 
            uint requestId = IdGenerator.NewId();
            TS_URB_GET_CURRENT_FRAME_NUMBER req = new TS_URB_GET_CURRENT_FRAME_NUMBER(
                requestId,
                0);
            rdpeusbAdapter.TransferInRequest(device, req, frameNumberSize);

            LogComment("7. Receives TS_URB_GET_CURRENT_FRAME_NUMBER_RESULT in the  URB_COMPLETION_NO_DATA message.");
            // Waits for the result.
            EusbPdu pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            Site.Assert.IsInstanceOfType(
                pdu,
                typeof(EusbUrbCompletionNoDataPdu),
                "The result must be type of EusbUrbCompletionNoDataPdu.");
            EusbUrbCompletionNoDataPdu pduRes = (EusbUrbCompletionNoDataPdu)pdu;
            Site.Assert.IsSuccess((int)pduRes.HResult, "The EusbUrbCompletionNoDataPdu must indicate successful.");
            ReqCapturer.VerifyUrbCompletionNoData(pduRes, req, true, interfaceId);
            TS_URB_GET_CURRENT_FRAME_NUMBER_RESULT res = new TS_URB_GET_CURRENT_FRAME_NUMBER_RESULT();
            Site.Assert.IsTrue(
                PduMarshaler.Unmarshal(pduRes.TsUrbResult, res), 
                "The TS_URB_GET_CURRENT_FRAME_NUMBER_RESULT structure is sent via the URB_COMPLETION_NO_DATA message"
                );
            
            LogComment("The current frame number is {0}(0x{0:x4}).", res.FrameNumber);

            LogComment("8. Sends retract device request and the channel for the device is expected to be closed.");
            rdpeusbAdapter.RetractDevice(device, USB_RETRACT_REASON.UsbRetractReason_BlockedByPolicy);
        }
               
        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPEUSB")]
        [TestCategory("DeviceNeeded")]
        [Description("Verify behaviors of retrieving status from a device, interface, endpoint.")]
        public void S3_EUSB_OperateIo_GetStatus()
        {
            LogComment("S3_EUSB_OperateIo_GetStatus");

            LogComment("1. Creates the control virtual channel, exchanges capabilities then notifies that the channel is created.");
            context.ControlChannel = CreateVirtualChannel();

            LogComment("2. Receives an add virtual channel request.");
            rdpeusbAdapter.ExpectAddVirtualChannel(context.ControlChannel);

            LogComment("3. Creates a new virtual channel for the device.");
            DynamicVirtualChannel channel = CreateVirtualChannel();

            LogComment("4. Receives an add device request.");
            EusbDeviceContext device = rdpeusbAdapter.ExpectAddDevice(channel);

            LogComment("5. Registers a callback to provide the Request Completion Interface to the client.");
            uint interfaceId = IdGenerator.NewId();
            rdpeusbAdapter.RegisterCallback(device, 1, interfaceId);

            LogComment("6. Selects the device configuration.");
            SelectConfiguration(device, 0);

            LogComment("7. Retrieves the status from the device by sending TS_URB_CONTROL_GET_STATUS_REQUEST and verifies the response.");  
            const uint transferBufferLength = 2;
            const byte low4BitsMask = 0x0F;
            uint requestId;
            EusbPdu pdu;
            EusbUrbCompletionPdu pduRes;
            TS_URB_CONTROL_GET_STATUS_REQUEST req = new TS_URB_CONTROL_GET_STATUS_REQUEST();

            requestId = IdGenerator.NewId();
            req.Header.RequestId = requestId;
            req.Header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_GET_STATUS_FROM_DEVICE;
            req.Index = 0;
            // Sends device status request.
            rdpeusbAdapter.TransferInRequest(device, req, transferBufferLength);

            // Waits for the result.
            pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            Site.Assert.IsInstanceOfType(
                pdu,
                typeof(EusbUrbCompletionPdu),
                "The result must be type of EusbUrbCompletionPdu.");
            pduRes = (EusbUrbCompletionPdu)pdu;
            Site.Assert.IsSuccess((int)pduRes.HResult, "The EusbUrbCompletionNoDataPdu must indicate successful.");
            ReqCapturer.VerifyUrbCompletion(pduRes, req, interfaceId);

            LogComment("The device status is 0x{1:x4}", req.Index, BitConverter.ToUInt16(pduRes.OutputBuffer, 0));

            // TODO: Not sure how to get the index of interfaces and devices. Using any index can result successful responses.
            LogComment("8. Enumerates all interfaces and endpoints of the device and retrieves their status.");
            foreach (TS_USBD_INTERFACE_INFORMATION_RESULT iinf in context.SelectedConfig.Interface)
	        {
                requestId = IdGenerator.NewId();
                req.Header.RequestId = requestId;
                req.Header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_GET_STATUS_FROM_INTERFACE;
                req.Index = iinf.InterfaceNumber;
                // Sends interface status request.
                rdpeusbAdapter.TransferInRequest(device, req, transferBufferLength);

                // Waits for the result.
                pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
                Site.Assert.IsInstanceOfType(
                    pdu,
                    typeof(EusbUrbCompletionPdu),
                    "The result must be type of EusbUrbCompletionPdu.");
                pduRes = (EusbUrbCompletionPdu)pdu;
                Site.Assert.IsSuccess((int)pduRes.HResult, "The EusbUrbCompletionNoDataPdu must indicate successful.");
                ReqCapturer.VerifyUrbCompletion(pduRes, req, interfaceId);

                LogComment("The interface (index: {0}) status is 0x{1:x4}", req.Index, BitConverter.ToUInt16(pduRes.OutputBuffer, 0));

                foreach (TS_USBD_PIPE_INFORMATION_RESULT pinf in iinf.Pipes)
	            {
                    requestId = IdGenerator.NewId();
                    req.Header.RequestId = requestId;
                    req.Header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_GET_STATUS_FROM_ENDPOINT;
                    req.Index = (ushort)(pinf.EndpointAddress & low4BitsMask);
                    // Sends endpoint status request.
                    rdpeusbAdapter.TransferInRequest(device, req, transferBufferLength);

                    // Waits for the result.
                    pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
                    Site.Assert.IsInstanceOfType(
                        pdu,
                        typeof(EusbUrbCompletionPdu),
                        "The result must be type of EusbUrbCompletionPdu.");
                    pduRes = (EusbUrbCompletionPdu)pdu;
                    Site.Assert.IsSuccess((int)pduRes.HResult, "The EusbUrbCompletionNoDataPdu must indicate successful.");
                    ReqCapturer.VerifyUrbCompletion(pduRes, req, interfaceId);

                    LogComment("The endpoint (index: {0}) status is 0x{1:x4}", req.Index, BitConverter.ToUInt16(pduRes.OutputBuffer, 0));
	            }
	        }

            LogComment("9. Sends retract device request and the channel for the device is expected to be closed.");
            rdpeusbAdapter.RetractDevice(device, USB_RETRACT_REASON.UsbRetractReason_BlockedByPolicy);
        }
                
        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPEUSB")]
        [Description("Verify behaviors of retrieving the current configuration for a device.")]
        public void S3_EUSB_OperateIo_GetConfiguration()
        {
            LogComment("S3_EUSB_OperateIo_GetConfiguration");

            LogComment("1. Creates the control virtual channel, exchanges capabilities then notifies that the channel is created.");
            context.ControlChannel = CreateVirtualChannel();

            LogComment("2. Receives an add virtual channel request.");
            rdpeusbAdapter.ExpectAddVirtualChannel(context.ControlChannel);

            LogComment("3. Creates a new virtual channel for the device.");
            DynamicVirtualChannel channel = CreateVirtualChannel();

            LogComment("4. Receives an add device request.");
            EusbDeviceContext device = rdpeusbAdapter.ExpectAddDevice(channel);

            LogComment("5. Registers a callback to provide the Request Completion Interface to the client.");
            uint interfaceId = IdGenerator.NewId();
            rdpeusbAdapter.RegisterCallback(device, 1, interfaceId);

            LogComment("6. Sends TS_URB_CONTROL_GET_CONFIGURATION_REQUEST and verifies the response.");
            const uint transferBufferLength = 1; //Must be 1.
            uint requestId = IdGenerator.NewId();
            TS_URB_CONTROL_GET_CONFIGURATION_REQUEST req = new TS_URB_CONTROL_GET_CONFIGURATION_REQUEST(
                requestId,
                0);
            rdpeusbAdapter.TransferInRequest(device, req, transferBufferLength);

            // Waits for the result.
            EusbPdu pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);

            Site.Assert.IsInstanceOfType(
                pdu,
                typeof(EusbUrbCompletionPdu),
                "The result must be type of EusbUrbCompletionPdu.");
            EusbUrbCompletionPdu pduRes = (EusbUrbCompletionPdu)pdu;
            Site.Assert.IsSuccess((int)pduRes.HResult, "The EusbUrbCompletionNoDataPdu must indicate successful.");
            ReqCapturer.VerifyUrbCompletion(pduRes, req, interfaceId);
            Site.Assert.IsNotNull(pduRes.OutputBuffer, "The result buffer must not be null.");
            Site.Assert.AreEqual(transferBufferLength, pduRes.OutputBufferSize, "The result must be a single byte.");

            LogComment("The index of the current configuration is {0}.", pduRes.OutputBuffer[0]);

            LogComment("7. Sends retract device request and the channel for the device is expected to be closed.");
            rdpeusbAdapter.RetractDevice(device, USB_RETRACT_REASON.UsbRetractReason_BlockedByPolicy);
        }
                
        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPEUSB")]
        [Description("Verify behaviors of retrieving the current alternate interface setting for an interface in the current configuration.")]
        public void S3_EUSB_OperateIo_GetInterface()
        {
            LogComment("S3_EUSB_OperateIo_GetInterface");

            LogComment("1. Creates the control virtual channel, exchanges capabilities then notifies that the channel is created.");
            context.ControlChannel = CreateVirtualChannel();

            LogComment("2. Receives an add virtual channel request.");
            rdpeusbAdapter.ExpectAddVirtualChannel(context.ControlChannel);

            LogComment("3. Creates a new virtual channel for the device.");
            DynamicVirtualChannel channel =  CreateVirtualChannel();

            LogComment("4. Receives an add device request.");
            EusbDeviceContext device = rdpeusbAdapter.ExpectAddDevice(channel);

            LogComment("5. Registers a callback to provide the Request Completion Interface to the client.");
            uint interfaceId = IdGenerator.NewId();
            rdpeusbAdapter.RegisterCallback(device, 1, interfaceId);

            LogComment("6. Sends TS_URB_CONTROL_GET_INTERFACE_REQUEST and verifies the response.");
            const uint transferBufferLength = 1; //Must be 1.
            // TODO: May not hard code the interface index.
            ushort interfaceIndex = 0;
            uint requestId = IdGenerator.NewId();
            TS_URB_CONTROL_GET_INTERFACE_REQUEST req = new UrbBuilder(
                URB_FUNCTIONID.URB_FUNCTION_GET_INTERFACE,
                requestId,
                0).BuildGetInterfaceRequest(interfaceIndex);
            rdpeusbAdapter.TransferInRequest(device, req, transferBufferLength);

            // Waits for the result.
            EusbPdu pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            Site.Assert.IsInstanceOfType(
                pdu,
                typeof(EusbUrbCompletionPdu),
                "The result must be type of EusbUrbCompletionPdu.");
            EusbUrbCompletionPdu pduRes = (EusbUrbCompletionPdu)pdu;
            Site.Assert.IsSuccess((int)pduRes.HResult, "The EusbUrbCompletionNoDataPdu must indicate successful.");
            ReqCapturer.VerifyUrbCompletion(pduRes, req, interfaceId);
            Site.Assert.IsNotNull(pduRes.OutputBuffer, "The result buffer must not be null.");
            Site.Assert.AreEqual(transferBufferLength, pduRes.OutputBufferSize, "The result must be a single byte.");

            LogComment("The index of the current alternate setting for the interface is {0}.", pduRes.OutputBuffer[0]);

            LogComment("7. Sends retract device request and the channel for the device is expected to be closed.");
            rdpeusbAdapter.RetractDevice(device, USB_RETRACT_REASON.UsbRetractReason_BlockedByPolicy);
        }
                
        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPEUSB")]
        [Description("Verify behaviors of retrieving Microsoft OS Feature Descriptors from a USB device or an interface on a USB device.")]
        public void S3_EUSB_OperateIo_OsFeatureDescriptor()
        {
            LogComment("S3_EUSB_OperateIo_OsFeatureDescriptor");

            LogComment("1. Creates the control virtual channel, exchanges capabilities then notifies that the channel is created.");
            context.ControlChannel = CreateVirtualChannel();

            LogComment("2. Receives an add virtual channel request.");
            rdpeusbAdapter.ExpectAddVirtualChannel(context.ControlChannel);

            LogComment("3. Creates a new virtual channel for the device.");
            DynamicVirtualChannel channel = CreateVirtualChannel();

            LogComment("4. Receives an add device request.");
            EusbDeviceContext device = rdpeusbAdapter.ExpectAddDevice(channel);

            LogComment("5. Registers a callback to provide the Request Completion Interface to the client.");
            uint interfaceId = IdGenerator.NewId();
            rdpeusbAdapter.RegisterCallback(device, 1, interfaceId);

            LogComment("6. Sends TS_URB_OS_FEATURE_DESCRIPTOR_REQUEST requests and receives the response.");
            const uint transferBufferLength = 1024; // the maximum MS OS Feature Descriptor size is 4 Kilobytes.

            uint requestId = IdGenerator.NewId();
            Recipient[] recipients = { Recipient.Device, Recipient.Interface, Recipient.Endpoint };
            byte interfaceIndex = 0;
            byte MS_PageIndex = 0;
            byte MS_FeatureDescriptorIndex = 0;

            foreach (Recipient r in recipients)
	        {
                // TODO: Need to test against OS feature supported devices.
                TS_URB_OS_FEATURE_DESCRIPTOR_REQUEST req = new UrbBuilder(
                       URB_FUNCTIONID.URB_FUNCTION_GET_MS_FEATURE_DESCRIPTOR,
                       requestId,
                       0).BuildOSFeatureDescriptor(r, interfaceIndex, MS_PageIndex, MS_FeatureDescriptorIndex);
                rdpeusbAdapter.TransferInRequest(device, req, transferBufferLength);

                // Waits for the result.
                EusbPdu pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
                LogComment("Received completion PDU: \r\n{0}", pdu.ToString());
	        }
            
            LogComment("7. Sends retract device request and the channel for the device is expected to be closed.");
            rdpeusbAdapter.RetractDevice(device, USB_RETRACT_REASON.UsbRetractReason_BlockedByPolicy);
        }

        #endregion

        #region IO Control Test Cases
                
        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPEUSB")]
        [Description("Verify behaviors of the process to reset the upstream port of the device it manages.")]
        public void S3_EUSB_OperateIo_ResetPort()
        {
            LogComment("S3_EUSB_OperateIo_ResetPort");

            LogComment("1. Creates the control virtual channel, exchanges capabilities then notifies that the channel is created.");
            context.ControlChannel = CreateVirtualChannel();

            LogComment("2. Receives an add virtual channel request.");
            rdpeusbAdapter.ExpectAddVirtualChannel(context.ControlChannel);

            LogComment("3. Creates a new virtual channel for the device.");
            DynamicVirtualChannel channel =  CreateVirtualChannel();

            LogComment("4. Receives an add device request.");
            EusbDeviceContext device = rdpeusbAdapter.ExpectAddDevice(channel);

            LogComment("5. Registers a callback to provide the Request Completion Interface to the client.");
            uint interfaceId = IdGenerator.NewId();
            rdpeusbAdapter.RegisterCallback(device, 1, interfaceId);

            LogComment("6. Sends an IOCTL_INTERNAL_USB_RESET_PORT request.");
            uint requestId = IdGenerator.NewId();
            const uint outputSize = 0;
            rdpeusbAdapter.IoControl(device, UsbIoControlCode.IOCTL_INTERNAL_USB_RESET_PORT, null, outputSize, requestId);

            LogComment("7. Receives a completion message.");
            EusbPdu pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            // TODO: The result should be checked.
            Site.Assert.IsNotNull(pdu, "Must receive a completion message.");

            LogComment("8. Sends retract device request and the channel for the device is expected to be closed.");
            rdpeusbAdapter.RetractDevice(device, USB_RETRACT_REASON.UsbRetractReason_BlockedByPolicy);
        }
                
        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPEUSB")]
        [Description("Verify behaviors of querying the status of the PDO.")]
        public void S3_EUSB_OperateIo_GetPortStatus()
        {
            LogComment("S3_EUSB_OperateIo_GetPortStatus");

            LogComment("1. Creates the control virtual channel, exchanges capabilities then notifies that the channel is created.");
            context.ControlChannel = CreateVirtualChannel();

            LogComment("2. Receives an add virtual channel request.");
            rdpeusbAdapter.ExpectAddVirtualChannel(context.ControlChannel);

            LogComment("3. Creates a new virtual channel for the device.");
            DynamicVirtualChannel channel = CreateVirtualChannel();

            LogComment("4. Receives an add device request.");
            EusbDeviceContext device = rdpeusbAdapter.ExpectAddDevice(channel);

            LogComment("5. Registers a callback to provide the Request Completion Interface to the client.");
            uint interfaceId = IdGenerator.NewId();
            rdpeusbAdapter.RegisterCallback(device, 1, interfaceId);

            LogComment("6. Sends an IOCTL_INTERNAL_USB_GET_PORT_STATUS request.");
            uint requestId = IdGenerator.NewId();
            const uint outputSize = 4;
            rdpeusbAdapter.IoControl(device, UsbIoControlCode.IOCTL_INTERNAL_USB_GET_PORT_STATUS, null, outputSize, requestId);

            LogComment("7. Receives a completion message.");
            EusbPdu pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            // TODO: The result should be checked.
            Site.Assert.IsNotNull(pdu, "Must receive a completion message.");

            LogComment("8. Sends retract device request and the channel for the device is expected to be closed.");
            rdpeusbAdapter.RetractDevice(device, USB_RETRACT_REASON.UsbRetractReason_BlockedByPolicy);
        }
                
        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPEUSB")]
        [Description("Verify behaviors of querying the count of USB hub.")]
        public void S3_EUSB_OperateIo_GetHubCount()
        {
            LogComment("S3_EUSB_OperateIo_GetHubCount");

            LogComment("1. Creates the control virtual channel, exchanges capabilities then notifies that the channel is created.");
            context.ControlChannel = CreateVirtualChannel();

            LogComment("2. Receives an add virtual channel request.");
            rdpeusbAdapter.ExpectAddVirtualChannel(context.ControlChannel);

            LogComment("3. Creates a new virtual channel for the device.");
            DynamicVirtualChannel channel = CreateVirtualChannel();

            LogComment("4. Receives an add device request.");
            EusbDeviceContext device = rdpeusbAdapter.ExpectAddDevice(channel);

            LogComment("5. Registers a callback to provide the Request Completion Interface to the client.");
            uint interfaceId = IdGenerator.NewId();
            rdpeusbAdapter.RegisterCallback(device, 1, interfaceId);

            LogComment("6. Sends an IOCTL_INTERNAL_USB_GET_HUB_COUNT request.");
            uint requestId = IdGenerator.NewId();
            const uint outputSize = 4;
            rdpeusbAdapter.IoControl(device, UsbIoControlCode.IOCTL_INTERNAL_USB_GET_HUB_COUNT, null, outputSize, requestId);

            LogComment("7. Receives a completion message.");
            EusbPdu pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            // TODO: The result should be checked.
            Site.Assert.IsNotNull(pdu, "Must receive a completion message.");

            LogComment("8. Sends retract device request and the channel for the device is expected to be closed.");
            rdpeusbAdapter.RetractDevice(device, USB_RETRACT_REASON.UsbRetractReason_BlockedByPolicy);
        }
                
        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPEUSB")]
        [TestCategory("DeviceNeeded")]
        [Description("Verify behaviors of simulating a device unplug and replug on the port associated with the PDO.")]
        public void S3_EUSB_OperateIo_CyclePort()
        {
            LogComment("S3_EUSB_OperateIo_CyclePort");

            LogComment("1. Creates the control virtual channel, exchanges capabilities then notifies that the channel is created.");
            context.ControlChannel = CreateVirtualChannel();

            LogComment("2. Receives an add virtual channel request.");
            rdpeusbAdapter.ExpectAddVirtualChannel(context.ControlChannel);

            LogComment("3. Creates a new virtual channel for the device.");
            DynamicVirtualChannel channel = CreateVirtualChannel();

            LogComment("4. Receives an add device request.");
            EusbDeviceContext device = rdpeusbAdapter.ExpectAddDevice(channel);

            LogComment("5. Registers a callback to provide the Request Completion Interface to the client.");
            uint interfaceId = IdGenerator.NewId();
            rdpeusbAdapter.RegisterCallback(device, 1, interfaceId);

            LogComment("6. Sends an IOCTL_INTERNAL_USB_CYCLE_PORT request.");
            uint requestId = IdGenerator.NewId();
            const uint outputSize = 0;
            rdpeusbAdapter.IoControl(device, UsbIoControlCode.IOCTL_INTERNAL_USB_CYCLE_PORT, null, outputSize, requestId);

            LogComment("7. Receives a completion message.");
            EusbPdu pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            // TODO: The result should be checked.
            Site.Assert.IsNotNull(pdu, "Must receive a completion message.");

            LogComment("8. Sends retract device request and the channel for the device is expected to be closed.");
            rdpeusbAdapter.RetractDevice(device, USB_RETRACT_REASON.UsbRetractReason_BlockedByPolicy);
        }

        [TestMethod]
        [Priority(1)]        
        [TestCategory("RDP7.1")]
        [TestCategory("RDPEUSB")]
        [Description("Verify behaviors of querying the bus driver for certain bus information.")]
        public void S3_EUSB_OperateIo_GetBusInfo()
        {
            LogComment("S3_EUSB_OperateIo_GetBusInfo");

            LogComment("1. Creates the control virtual channel, exchanges capabilities then notifies that the channel is created.");
            context.ControlChannel = CreateVirtualChannel();

            LogComment("2. Receives an add virtual channel request.");
            rdpeusbAdapter.ExpectAddVirtualChannel(context.ControlChannel);

            LogComment("3. Creates a new virtual channel for the device.");
            DynamicVirtualChannel channel = CreateVirtualChannel();

            LogComment("4. Receives an add device request.");
            EusbDeviceContext device = rdpeusbAdapter.ExpectAddDevice(channel);

            LogComment("5. Registers a callback to provide the Request Completion Interface to the client.");
            uint interfaceId = IdGenerator.NewId();
            rdpeusbAdapter.RegisterCallback(device, 1, interfaceId);

            LogComment("6. Sends an IOCTL_INTERNAL_USB_GET_BUS_INFO request.");
            uint requestId = IdGenerator.NewId();
            const uint outputSize = 16;
            rdpeusbAdapter.IoControl(device, UsbIoControlCode.IOCTL_INTERNAL_USB_GET_BUS_INFO, null, outputSize, requestId);

            LogComment("7. Receives a completion message.");
            EusbPdu pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            // TODO: The result should be checked.
            Site.Assert.IsNotNull(pdu, "Must receive a completion message.");

            LogComment("8. Sends retract device request and the channel for the device is expected to be closed.");
            rdpeusbAdapter.RetractDevice(device, USB_RETRACT_REASON.UsbRetractReason_BlockedByPolicy);
        }

        [TestMethod]
        [Priority(1)]        
        [TestCategory("RDP7.1")]
        [TestCategory("RDPEUSB")]
        [Description("Verify behaviors of querying the bus driver for the device name of the USB host controller.")]
        public void S3_EUSB_OperateIo_GetControllerName()
        {
            //The IOCTL_INTERNAL_USB_RESET_PORT I/O control request is used by a driver to reset the upstream port of the device it manages. After a successful reset, the bus driver reselects the configuration and any alternative interface settings that the device had before the reset occurred. All pipe handles, configuration handles and interface handles remain valid.
            LogComment("S3_EUSB_OperateIo_GetControllerName");

            LogComment("1. Creates the control virtual channel, exchanges capabilities then notifies that the channel is created.");
            context.ControlChannel = CreateVirtualChannel();

            LogComment("2. Receives an add virtual channel request.");
            rdpeusbAdapter.ExpectAddVirtualChannel(context.ControlChannel);

            LogComment("3. Creates a new virtual channel for the device.");
            DynamicVirtualChannel channel = CreateVirtualChannel();

            LogComment("4. Receives an add device request.");
            EusbDeviceContext device = rdpeusbAdapter.ExpectAddDevice(channel);

            LogComment("5. Registers a callback to provide the Request Completion Interface to the client.");
            uint interfaceId = IdGenerator.NewId();
            rdpeusbAdapter.RegisterCallback(device, 1, interfaceId);

            LogComment("6. Sends an IO request.");
            uint requestId = IdGenerator.NewId();
            const uint outputSize = 256;
            rdpeusbAdapter.IoControl(device, UsbIoControlCode.IOCTL_INTERNAL_USB_GET_CONTROLLER_NAME, null, outputSize, requestId);

            LogComment("7. Receives a completion message.");
            EusbPdu pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            // TODO: The result should be checked.
            Site.Assert.IsNotNull(pdu, "Must receive a completion message.");

            LogComment("8. Sends retract device request and the channel for the device is expected to be closed.");
            rdpeusbAdapter.RetractDevice(device, USB_RETRACT_REASON.UsbRetractReason_BlockedByPolicy);
        }

        #endregion

        #region OSR FX2 Specific Test Cases

        [TestMethod]
        [Priority(2)]        
        [TestCategory("RDP7.1")]
        [TestCategory("RDPEUSB")]
        [TestCategory("DeviceNeeded")]
        [Description("Verify behaviors of issuing OSR FX2 Device vendor command, SET/READ BARGRAPH DISPLAY.")]
        public void S3_EUSB_OperateIo_OSRFX2_BargraphDisplay()
        {
            LogComment("S3_EUSB_OperateIo_OSRFX2_BargraphDisplay");

            LogComment("1. Creates the control virtual channel, exchanges capabilities then notifies that the channel is created.");
            context.ControlChannel = CreateVirtualChannel();

            LogComment("2. Receives an add virtual channel request.");
            rdpeusbAdapter.ExpectAddVirtualChannel(context.ControlChannel);

            LogComment("3. Creates a new virtual channel for the device.");
            DynamicVirtualChannel channel = CreateVirtualChannel();

            LogComment("4. Receives an add device request.");
            EusbDeviceContext device = rdpeusbAdapter.ExpectAddDevice(channel);

            LogComment("5. Registers a callback to provide the Request Completion Interface to the client.");
            uint interfaceId = IdGenerator.NewId();
            rdpeusbAdapter.RegisterCallback(device, 1, interfaceId);

            LogComment("6. Sends several TS_URB_CONTROL_VENDOR_OR_CLASS_REQUESTs with the vendor command code SET_BARGRAPH_DISPLAY to change " +
                "state of the bar graph display on the device, then READ_BARGRAPH_DISPLAY to retrieve the state.");

            // Build the temp instance of TS_URB_CONTROL_VENDOR_OR_CLASS_REQUEST, which will be modified later.
            TS_URB_CONTROL_VENDOR_OR_CLASS_REQUEST req = new UrbBuilder(
                URB_FUNCTIONID.URB_FUNCTION_VENDOR_DEVICE,
                0,
                0).BuildVendorCommandRequest();

            EusbPdu pdu;
            EusbUrbCompletionNoDataPdu pduResNoData;
            EusbUrbCompletionPdu pduRes;
            uint requestId;
            byte[] data;

            // i is the iterator to set bit from 0 to 7.
            for (int i = 1; i < 0xFF; i = i << 1)
            {
                LogComment("The graph bar is setting to 0x{0:x2}.", i);

                requestId = IdGenerator.NewId();
                req.Request = (byte)OsrFx2VendorCommand.SET_BARGRAPH_DISPLAY;
                req.TransferFlags = 0;
                req.Header.RequestId = requestId;
                data = new byte[] { (byte)i };

                // Sends the SET_BARGRAPH_DISPLAY request.
                rdpeusbAdapter.TransferOutRequest(device, req, data);

                // Waits for the result.
                pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
                Site.Assert.IsInstanceOfType(
                    pdu,
                    typeof(EusbUrbCompletionNoDataPdu),
                    "The result must be type of EusbUrbCompletionNoDataPdu.");
                pduResNoData = (EusbUrbCompletionNoDataPdu)pdu;
                Site.Assert.IsSuccess((int)pduResNoData.HResult, "The EusbUrbCompletionNoDataPdu must indicate successful.");
                ReqCapturer.VerifyUrbCompletionNoData(pduResNoData, req, false, interfaceId);
                Site.Assert.AreEqual(
                    (uint)data.Length, 
                    pduResNoData.OutputBufferSize, 
                    "The size, in bytes, of data sent to the device of the RequestId that corresponds to a TRANSFER_OUT_REQUEST."
                    );

                requestId = IdGenerator.NewId();
                req.Request = (byte)OsrFx2VendorCommand.READ_BARGRAPH_DISPLAY;
                req.TransferFlags = (byte)(TransferFlags.USBD_TRANSFER_DIRECTION_IN | TransferFlags.USBD_SHORT_TRANSFER_OK);
                req.Header.RequestId = requestId;

                // Sends the READ_BARGRAPH_DISPLAY request.
                rdpeusbAdapter.TransferInRequest(device, req, 1);
                
                // Waits for the result.
                pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
                Site.Assert.IsInstanceOfType(
                    pdu,
                    typeof(EusbUrbCompletionPdu),
                    "The result must be type of EusbUrbCompletionNoDataPdu.");
                pduRes = (EusbUrbCompletionPdu)pdu;
                Site.Assert.IsSuccess((int)pduRes.HResult, "The EusbUrbCompletionNoDataPdu must indicate successful.");
                ReqCapturer.VerifyUrbCompletion(pduRes, req, interfaceId);
                Site.Assert.AreEqual(1u, pduRes.OutputBufferSize, "The result should be a UCHAR value.");
                Site.Assert.AreEqual(data[0], pduRes.OutputBuffer[0], "The graph bar state is expected to be same as the state to be set in the request.");

                // Short delay for checking device
                Thread.Sleep(1000);
            }

            LogComment("7. Sends retract device request and the channel for the device is expected to be closed.");
            rdpeusbAdapter.RetractDevice(device, USB_RETRACT_REASON.UsbRetractReason_BlockedByPolicy);
        }

        [TestMethod]
        [Priority(2)]        
        [TestCategory("RDP7.1")]
        [TestCategory("RDPEUSB")]
        [TestCategory("DeviceNeeded")]
        [Description("Verify behaviors of issuing OSR FX2 Device vendor command, SET/READ 7 SEGMENT DISPLAY.")]
        public void S3_EUSB_OperateIo_OSRFX2_SegmentDisplay()
        {
            LogComment("S3_EUSB_OperateIo_OSRFX2_SegmentDisplay");

            LogComment("1. Creates the control virtual channel, exchanges capabilities then notifies that the channel is created.");
            context.ControlChannel = CreateVirtualChannel();

            LogComment("2. Receives an add virtual channel request.");
            rdpeusbAdapter.ExpectAddVirtualChannel(context.ControlChannel);

            LogComment("3. Creates a new virtual channel for the device.");
            DynamicVirtualChannel channel = CreateVirtualChannel();

            LogComment("4. Receives an add device request.");
            EusbDeviceContext device = rdpeusbAdapter.ExpectAddDevice(channel);

            LogComment("5. Registers a callback to provide the Request Completion Interface to the client.");
            uint interfaceId = IdGenerator.NewId();
            rdpeusbAdapter.RegisterCallback(device, 1, interfaceId);

            LogComment("6. Sends several TS_URB_CONTROL_VENDOR_OR_CLASS_REQUESTs with the vendor command code SET_7_SEGMENT_DISPLAY to change " + 
                "state of the 7-segment display on the device, then READ_7_SEGMENT_DISPLAY to retrieve the state.");

            // Build the temp instance of TS_URB_CONTROL_VENDOR_OR_CLASS_REQUEST
            TS_URB_CONTROL_VENDOR_OR_CLASS_REQUEST req = new UrbBuilder(
                URB_FUNCTIONID.URB_FUNCTION_VENDOR_DEVICE,
                0,
                0).BuildVendorCommandRequest();

            EusbPdu pdu;
            EusbUrbCompletionNoDataPdu pduResNoData;
            EusbUrbCompletionPdu pduRes;
            uint requestId;
            byte[] data;

            // Set the state to test data stored in SegmentDisplayStates iteratively.
            for (int i = 0; i < SegmentDisplayStates.Length; i++)
            {
                LogComment("Setting 7 segment display state to 0x{0:x2}", SegmentDisplayStates[i]);

                requestId = IdGenerator.NewId();
                req.Request = (byte)OsrFx2VendorCommand.SET_7_SEGMENT_DISPLAY;
                req.TransferFlags = (byte)TransferFlags.USBD_TRANSFER_DIRECTION_OUT;
                req.Header.RequestId = requestId;
                data = new byte[] { SegmentDisplayStates[i] };

                // Sends the SET_7_SEGMENT_DISPLAY request.
                rdpeusbAdapter.TransferOutRequest(device, req, data);

                // Waits for the result.
                pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);

                Site.Assert.IsInstanceOfType(
                    pdu,
                    typeof(EusbUrbCompletionNoDataPdu),
                    "The result must be type of EusbUrbCompletionNoDataPdu.");
                pduResNoData = (EusbUrbCompletionNoDataPdu)pdu;
                Site.Assert.IsSuccess((int)pduResNoData.HResult, "The EusbUrbCompletionNoDataPdu must indicate successful.");
                ReqCapturer.VerifyUrbCompletionNoData(pduResNoData, req, false, interfaceId);
                Site.Assert.AreEqual(
                    (uint)data.Length,
                    pduResNoData.OutputBufferSize,
                    "The size, in bytes, of data sent to the device of the RequestId that corresponds to a TRANSFER_OUT_REQUEST."
                    );

                requestId = IdGenerator.NewId();
                req.Request = (byte)OsrFx2VendorCommand.READ_7_SEGMENT_DISPLAY;
                req.TransferFlags = (byte)(TransferFlags.USBD_TRANSFER_DIRECTION_IN | TransferFlags.USBD_SHORT_TRANSFER_OK);
                req.Header.RequestId = requestId;

                // Sends the READ_7_SEGMENT_DISPLAY request.
                rdpeusbAdapter.TransferInRequest(device, req, 1);

                // Waits for the result.
                pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
                Site.Assert.IsInstanceOfType(
                    pdu,
                    typeof(EusbUrbCompletionPdu),
                    "The result must be type of EusbUrbCompletionNoDataPdu.");
                pduRes = (EusbUrbCompletionPdu)pdu;
                Site.Assert.IsSuccess((int)pduRes.HResult, "The EusbUrbCompletionNoDataPdu must indicate successful.");
                ReqCapturer.VerifyUrbCompletion(pduRes, req, interfaceId);
                Site.Assert.AreEqual(1u, pduRes.OutputBufferSize, "The result should be a UCHAR value.");
                Site.Assert.AreEqual(data[0], pduRes.OutputBuffer[0], "The state is expected to be same as the state to be set in the request.");

                // Short delay for checking device
                Thread.Sleep(1000);
            }

            LogComment("7. Sends retract device request and the channel for the device is expected to be closed.");
            rdpeusbAdapter.RetractDevice(device, USB_RETRACT_REASON.UsbRetractReason_BlockedByPolicy);
        }

        [TestMethod]
        [Priority(2)]        
        [TestCategory("RDP7.1")]
        [TestCategory("RDPEUSB")]
        [TestCategory("DeviceNeeded")]
        [Description("Verify behaviors of issuing OSR FX2 Device vendor command, IS HIGH SPEED.")]
        public void S3_EUSB_OperateIo_OSRFX2_IsHighSpeed()
        {
            LogComment("S3_EUSB_OperateIo_OSRFX2_IsHighSpeed");

            LogComment("1. Creates the control virtual channel, exchanges capabilities then notifies that the channel is created.");
            context.ControlChannel = CreateVirtualChannel();

            LogComment("2. Receives an add virtual channel request.");
            rdpeusbAdapter.ExpectAddVirtualChannel(context.ControlChannel);

            LogComment("3. Creates a new virtual channel for the device.");
            DynamicVirtualChannel channel = CreateVirtualChannel();

            LogComment("4. Receives an add device request.");
            EusbDeviceContext device = rdpeusbAdapter.ExpectAddDevice(channel);

            LogComment("5. Registers a callback to provide the Request Completion Interface to the client.");
            uint interfaceId = IdGenerator.NewId();
            rdpeusbAdapter.RegisterCallback(device, 1, interfaceId);

            LogComment("6. Sends TS_URB_CONTROL_VENDOR_OR_CLASS_REQUEST with the vendor command code IS_HIGH_SPEED to check the state of the device.");
            uint requestId = IdGenerator.NewId();
            TS_URB_CONTROL_VENDOR_OR_CLASS_REQUEST req = new UrbBuilder(
                URB_FUNCTIONID.URB_FUNCTION_VENDOR_DEVICE,
                requestId,
                0).BuildVendorCommandRequest(
                (byte)OsrFx2VendorCommand.IS_HIGH_SPEED, 
                0,
                TransferFlags.USBD_TRANSFER_DIRECTION_IN | TransferFlags.USBD_SHORT_TRANSFER_OK
                );

            rdpeusbAdapter.TransferInRequest(device, req, 1);

            LogComment("7. Receives the EusbUrbCompletionPdu message and check the result.");
            EusbPdu pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            Site.Assert.IsInstanceOfType(
                pdu,
                typeof(EusbUrbCompletionPdu),
                "The result must be type of EusbUrbCompletionNoDataPdu.");
            EusbUrbCompletionPdu pduRes = (EusbUrbCompletionPdu)pdu;
            Site.Assert.IsSuccess((int)pduRes.HResult, "The EusbUrbCompletionNoDataPdu must indicate successful.");
            ReqCapturer.VerifyUrbCompletion(pduRes, req, interfaceId);
            Site.Assert.AreEqual(1u, pduRes.OutputBufferSize, "The result should be a UCHAR value.");
            byte state = pduRes.OutputBuffer[0];

            // TODO: The device spec says High Speed will return 1 in a UCHAR but acctaully the device will return 1 at the most significant bit.
            const byte highSpeedIndicator = 0x80;
            Site.Assert.IsTrue(state == 0 || state == highSpeedIndicator, "The state must be 1 = High Speed or 0 = Full Speed. The device returned {0}.", state);

            LogComment("The device state is in {0} Speed.", state == highSpeedIndicator ? "High" : "Full");

            LogComment("8. Sends retract device request and the channel for the device is expected to be closed.");
            rdpeusbAdapter.RetractDevice(device, USB_RETRACT_REASON.UsbRetractReason_BlockedByPolicy);
        }

        [TestMethod]
        [Priority(2)]        
        [TestCategory("RDP7.1")]
        [TestCategory("RDPEUSB")]
        [TestCategory("DeviceNeeded")]
        [Description("Verify behaviors of issuing OSR FX2 Device vendor command, READ SWITCHES.")]
        public void S3_EUSB_OperateIo_OSRFX2_ReadSwitches()
        {
            LogComment("S3_EUSB_OperateIo_OSRFX2_ReadSwitches");

            LogComment("1. Creates the control virtual channel, exchanges capabilities then notifies that the channel is created.");
            context.ControlChannel = CreateVirtualChannel();

            LogComment("2. Receives an add virtual channel request.");
            rdpeusbAdapter.ExpectAddVirtualChannel(context.ControlChannel);

            LogComment("3. Creates a new virtual channel for the device.");
            DynamicVirtualChannel channel = CreateVirtualChannel();

            LogComment("4. Receives an add device request.");
            EusbDeviceContext device = rdpeusbAdapter.ExpectAddDevice(channel);

            LogComment("5. Registers a callback to provide the Request Completion Interface to the client.");
            uint interfaceId = IdGenerator.NewId();
            rdpeusbAdapter.RegisterCallback(device, 1, interfaceId);

            LogComment("6. Sends TS_URB_CONTROL_VENDOR_OR_CLASS_REQUEST with the vendor command code READ_SWITCHES to check the switch state of the device.");
            uint requestId = IdGenerator.NewId();
            TS_URB_CONTROL_VENDOR_OR_CLASS_REQUEST req = new UrbBuilder(
                URB_FUNCTIONID.URB_FUNCTION_VENDOR_DEVICE,
                requestId,
                0).BuildVendorCommandRequest(
                (byte)OsrFx2VendorCommand.READ_SWITCHES,
                0,
                TransferFlags.USBD_TRANSFER_DIRECTION_IN | TransferFlags.USBD_SHORT_TRANSFER_OK
                );

            rdpeusbAdapter.TransferInRequest(device, req, 1);

            LogComment("7. Receives the EusbUrbCompletionPdu message and check the result.");
            EusbPdu pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            Site.Assert.IsInstanceOfType(
                pdu,
                typeof(EusbUrbCompletionPdu),
                "The result must be type of EusbUrbCompletionNoDataPdu.");
            EusbUrbCompletionPdu pduRes = (EusbUrbCompletionPdu)pdu;
            Site.Assert.IsSuccess((int)pduRes.HResult, "The EusbUrbCompletionNoDataPdu must indicate successful.");
            ReqCapturer.VerifyUrbCompletion(pduRes, req, interfaceId);
            Site.Assert.AreEqual(1u, pduRes.OutputBufferSize, "The result should be a UCHAR value.");
            byte state = pduRes.OutputBuffer[0];

            LogComment("The device switch state is 0x{0:x2}.", state);

            LogComment("8. Sends retract device request and the channel for the device is expected to be closed.");
            rdpeusbAdapter.RetractDevice(device, USB_RETRACT_REASON.UsbRetractReason_BlockedByPolicy);
        }

        [TestMethod]
        [Priority(2)]        
        [TestCategory("RDP7.1")]
        [TestCategory("RDPEUSB")]
        [TestCategory("DeviceNeeded")]
        [Description("Verify behaviors of using the interrupt endpoint to read the switch states.")]
        public void S3_EUSB_OperateIo_OSRFX2_InterruptIn()
        {
            LogComment("S3_EUSB_OperateIo_OSRFX2_InterruptIn");

            LogComment("1. Creates the control virtual channel, exchanges capabilities then notifies that the channel is created.");
            context.ControlChannel = CreateVirtualChannel();

            LogComment("2. Receives an add virtual channel request.");
            rdpeusbAdapter.ExpectAddVirtualChannel(context.ControlChannel);

            LogComment("3. Creates a new virtual channel for the device.");
            DynamicVirtualChannel channel = CreateVirtualChannel();

            LogComment("4. Receives an add device request.");
            EusbDeviceContext device = rdpeusbAdapter.ExpectAddDevice(channel);

            LogComment("5. Registers a callback to provide the Request Completion Interface to the client.");
            uint interfaceId = IdGenerator.NewId();
            rdpeusbAdapter.RegisterCallback(device, 1, interfaceId);

            LogComment("6. Select the configuration with index 0.");
            SelectConfiguration(device, 0);

            Site.Assume.IsTrue(
                null != context.SelectedConfig && context.SelectedConfig.NumInterfaces == 1,
                "The configuration for the device must contain one single interface.");
            Site.Assume.IsTrue(
                context.SelectedConfig.Interface[0].NumberOfPipes == 3,
                "The interface of the device must contain 3 endpoints.");

            LogComment("7. Query the switch states from the interrupt endpoint for several times.");
            int transferCount = InterruptEndpointReadCount;
            TS_USBD_PIPE_INFORMATION_RESULT[] endpoints = context.SelectedConfig.Interface[0].Pipes;
            byte endpointNum = (byte)EndpointNumber.InterruptIn; // Endpoint 1: Interrupt, IN from board to host

            LogComment("Query the switch states from endpoint {0}, total endpoints {1}.", endpointNum, endpoints.Length);
            for (int i = 0; i < transferCount; i++)
            {
                uint requestId = IdGenerator.NewId();
                TS_URB_BULK_OR_INTERRUPT_TRANSFER req = new UrbBuilder(
                    URB_FUNCTIONID.URB_FUNCTION_BULK_OR_INTERRUPT_TRANSFER,
                    requestId,
                    0).BuildInterruptTransferRequest(
                    endpoints, 
                    endpointNum, 
                    TransferFlags.USBD_TRANSFER_DIRECTION_IN, 
                    USBD_PIPE_TYPE.UsbdPipeTypeInterrupt
                    );
                rdpeusbAdapter.TransferInRequest(device, req, 1);

                EusbPdu pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
                EusbUrbCompletionPdu cpdu = pdu as EusbUrbCompletionPdu;
                if (null != cpdu)
                {
                    Site.Assert.AreEqual(1u, cpdu.OutputBufferSize, "The output buffer must be a single byte representing the switch state.");
                    Site.Assert.IsNotNull(cpdu.OutputBuffer, "The output buffer must not be empty");
                    LogComment("Round {0}: Current switch state is {1}.", i, Convert.ToString(cpdu.OutputBuffer[0], 2));
                }
                else
                {
                    LogComment("Round {0}: Time out.", i);
                }
            }

            LogComment("8. Sends retract device request and the channel for the device is expected to be closed.");
            rdpeusbAdapter.RetractDevice(device, USB_RETRACT_REASON.UsbRetractReason_BlockedByPolicy);
        }

        [TestMethod]
        [Priority(2)]        
        [TestCategory("RDP7.1")]
        [TestCategory("RDPEUSB")]
        [TestCategory("DeviceNeeded")]
        [Description("Verify behaviors of using the bulk read and write endpoints of the OSR FX2 Device, Bulk read / write.")]
        public void S3_EUSB_OperateIo_OSRFX2_BulkWriteRead()
        {
            LogComment("S3_EUSB_OperateIo_OSRFX2_BulkWriteRead");

            LogComment("1. Creates the control virtual channel, exchanges capabilities then notifies that the channel is created.");
            context.ControlChannel = CreateVirtualChannel();

            LogComment("2. Receives an add virtual channel request.");
            rdpeusbAdapter.ExpectAddVirtualChannel(context.ControlChannel);

            LogComment("3. Creates a new virtual channel for the device.");
            DynamicVirtualChannel channel = CreateVirtualChannel();

            LogComment("4. Receives an add device request.");
            EusbDeviceContext device = rdpeusbAdapter.ExpectAddDevice(channel);

            LogComment("5. Registers a callback to provide the Request Completion Interface to the client.");
            uint interfaceId = IdGenerator.NewId();
            rdpeusbAdapter.RegisterCallback(device, 1, interfaceId);

            LogComment("6. Selects the configuration with index 0.");
            SelectConfiguration(device, 0);

            Site.Assume.IsTrue(
                null != context.SelectedConfig && context.SelectedConfig.NumInterfaces == 1,
                "The configuration for the device must contain one single interface.");
            Site.Assume.IsTrue(
                context.SelectedConfig.Interface[0].NumberOfPipes == 3,
                "The interface of the device must contain 3 endpoints.");

            byte[] data = BulkTransferData;
            uint requestId = IdGenerator.NewId();
            TS_USBD_PIPE_INFORMATION_RESULT[] endpoints = context.SelectedConfig.Interface[0].Pipes;
            byte endpointNum = (byte)EndpointNumber.BulkOut; // Endpoint 6: Bulk, OUT from host to board

            LogComment("7. Writes test data to the bulk write endpoint");
            TS_URB_BULK_OR_INTERRUPT_TRANSFER req = new UrbBuilder(
                URB_FUNCTIONID.URB_FUNCTION_BULK_OR_INTERRUPT_TRANSFER,
                requestId,
                0).BuildInterruptTransferRequest(
                endpoints,
                endpointNum, 
                TransferFlags.USBD_TRANSFER_DIRECTION_OUT,
                USBD_PIPE_TYPE.UsbdPipeTypeBulk
                );
            rdpeusbAdapter.TransferOutRequest(device, req, data);

            EusbPdu pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            Site.Assert.IsInstanceOfType(
                pdu,
                typeof(EusbUrbCompletionNoDataPdu),
                "The result must be type of EusbUrbCompletionNoDataPdu.");
            EusbUrbCompletionNoDataPdu pduResNoData = (EusbUrbCompletionNoDataPdu)pdu;
            Site.Assert.IsSuccess((int)pduResNoData.HResult, "The EusbUrbCompletionNoDataPdu must indicate successful.");
            ReqCapturer.VerifyUrbCompletionNoData(pduResNoData, req, false, interfaceId);
            Site.Assert.AreEqual(
                (uint)data.Length,
                pduResNoData.OutputBufferSize,
                "The size, in bytes, of data sent to the device of the RequestId that corresponds to a TRANSFER_OUT_REQUEST."
                );

            requestId = IdGenerator.NewId();
            endpointNum = (byte)EndpointNumber.BulkIn; // Endpoint 8: Bulk, IN from board to host

            LogComment("8. Reads data from the bulk read endpoint and verifies they read are same as written data.");
            req = new UrbBuilder(
                URB_FUNCTIONID.URB_FUNCTION_BULK_OR_INTERRUPT_TRANSFER,
                requestId,
                0).BuildInterruptTransferRequest(
                endpoints,
                endpointNum, 
                TransferFlags.USBD_TRANSFER_DIRECTION_IN, 
                USBD_PIPE_TYPE.UsbdPipeTypeBulk);
            rdpeusbAdapter.TransferInRequest(device, req, (uint)data.Length);

            pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            Site.Assert.IsInstanceOfType(
                pdu,
                typeof(EusbUrbCompletionPdu),
                "The result must be type of EusbUrbCompletionPdu.");
            EusbUrbCompletionPdu pduRes = (EusbUrbCompletionPdu)pdu;
            Site.Assert.IsSuccess((int)pduRes.HResult, "The EusbUrbCompletionPdu must indicate successful.");
            ReqCapturer.VerifyUrbCompletion(pduRes, req, interfaceId);
            Site.Assert.AreEqual((uint)data.Length, pduRes.OutputBufferSize, "The size of data read must be same as written.");
            Site.Assert.IsTrue(data.SequenceEqual(pduRes.OutputBuffer), "The data read must be same as written.");

            LogComment("9. Sends retract device request and the channel for the device is expected to be closed.");
            rdpeusbAdapter.RetractDevice(device, USB_RETRACT_REASON.UsbRetractReason_BlockedByPolicy);
        }

        #endregion
    }
}
