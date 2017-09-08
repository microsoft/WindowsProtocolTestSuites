// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeusb;

namespace Microsoft.Protocols.TestSuites.Rdpeusb
{
    public partial class RdpeusbTestSutie
    {        
        [TestMethod]
        [Priority(0)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPEUSB")]
        [Description("Verify behaviors of canceling an IO control request.")]
        public void BVT_EUSB_CancelRequest_IoControl()
        {
            LogComment("BVT_EUSB_CancelRequest_IoControl");

            LogComment("1. Creates the control virtual channel, exchanges capabilities then notifies that the channel is created.");
            context.ControlChannel =  CreateVirtualChannel();

            LogComment("2. Receives an add virtual channel request.");
            rdpeusbAdapter.ExpectAddVirtualChannel(context.ControlChannel);

            LogComment("3. Creates a new virtual channel for the device.");
            DynamicVirtualChannel channel = CreateVirtualChannel();

            LogComment("4. Receives an add device request.");
            EusbDeviceContext device = rdpeusbAdapter.ExpectAddDevice(channel);

            LogComment("5. Register a callback to provide the Request Completion Interface to the client.");
            uint interfaceId = IdGenerator.NewId();
            rdpeusbAdapter.RegisterCallback(device, 1, interfaceId);

            // TODO: Needs a long processing IO request so that the request cannot be completed before cancel is 
            // received.
            LogComment("6. Sends an IO request.");
            uint requestId = IdGenerator.NewId();
            const uint outputSize = 256;
            rdpeusbAdapter.IoControl(device, UsbIoControlCode.IOCTL_INTERNAL_USB_GET_HUB_NAME, null, outputSize, requestId);

            LogComment("7. Sends a cancel request with the request ID specified in the IO request.");
            rdpeusbAdapter.CancelRequest(device, requestId);

            LogComment("8. Expects not to receive a completion message.");
            EusbPdu pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            if (null != pdu)
            {
                Site.Log.Add(
                    LogEntryKind.Debug,
                    "The Completion message is received. Callback interface ID: {0}, Completion Interface ID: {1}",
                    interfaceId,
                    pdu.InterfaceId
                    );
            }

            LogComment("9. Sends retract device request and the channel for the device is expected to be closed.");
            rdpeusbAdapter.RetractDevice(device, USB_RETRACT_REASON.UsbRetractReason_BlockedByPolicy);
        }
                
        [TestMethod]
        [Priority(0)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPEUSB")]
        [Description("Verify behaviors of canceling an internal IO control request.")]
        public void BVT_EUSB_CancelRequest_InternalIoControl()
        {
            LogComment("BVT_EUSB_CancelRequest_InternalIoControl");

            LogComment("1. Creates the control virtual channel, exchanges capabilities then notifies that the channel is created.");
            context.ControlChannel = CreateVirtualChannel();

            LogComment("2. Receives an add virtual channel request.");
            rdpeusbAdapter.ExpectAddVirtualChannel(context.ControlChannel);

            LogComment("3. Creates a new virtual channel for the device.");
            DynamicVirtualChannel channel = CreateVirtualChannel();

            LogComment("4. Receives an add device request.");
            EusbDeviceContext device = rdpeusbAdapter.ExpectAddDevice(channel);

            LogComment("5. Register a callback to provide the Request Completion Interface to the client.");
            uint interfaceId = IdGenerator.NewId();
            rdpeusbAdapter.RegisterCallback(device, 1, interfaceId);

            // TODO: Needs a long processing internal IO request so that the request cannot be completed before cancel is 
            // received.
            LogComment("6. Sends an internal IO request.");
            uint requestId = IdGenerator.NewId();
            const uint outputSize = 4;
            rdpeusbAdapter.InternalIoControl(device, UsbInternalIoControlCode.IOCTL_TSUSBGD_IOCTL_USBDI_QUERY_BUS_TIME, null, outputSize, requestId);

            LogComment("7. Sends a cancel request with the request ID specified in the IO request.");
            rdpeusbAdapter.CancelRequest(device, requestId);

            LogComment("8. Expects not to receive a completion message.");
            EusbPdu pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            if (null != pdu)
            {
                Site.Log.Add(
                    LogEntryKind.Debug,
                    "The Completion message is received. Callback interface ID: {0}, Completion Interface ID: {1}",
                    interfaceId,
                    pdu.InterfaceId
                    );
            }

            LogComment("9. Sends retract device request and the channel for the device is expected to be closed.");
            rdpeusbAdapter.RetractDevice(device, USB_RETRACT_REASON.UsbRetractReason_BlockedByPolicy);
        }
                
        [TestMethod]
        [Priority(0)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPEUSB")]
        [Description("Verify behaviors of canceling a transfer in request.")]
        public void BVT_EUSB_CancelRequest_TransferInRequest()
        {
            LogComment("BVT_EUSB_CancelRequest_TransferInRequest");

            LogComment("1. Creates the control virtual channel, exchanges capabilities then notifies that the channel is created.");
            context.ControlChannel = CreateVirtualChannel();

            LogComment("2. Receives an add virtual channel request.");
            rdpeusbAdapter.ExpectAddVirtualChannel(context.ControlChannel);

            LogComment("3. Creates a new virtual channel for the device.");
            DynamicVirtualChannel channel =  CreateVirtualChannel();

            LogComment("4. Receives an add device request.");
            EusbDeviceContext device = rdpeusbAdapter.ExpectAddDevice(channel);

            LogComment("5. Register a callback to provide the Request Completion Interface to the client.");
            uint interfaceId = IdGenerator.NewId();
            rdpeusbAdapter.RegisterCallback(device, 1, interfaceId);

            // TODO: Needs a long processing transfer in request so that the request cannot be completed before cancel is 
            // received.
            LogComment("6. Sends TS_URB_CONTROL_DESCRIPTOR_REQUEST within a transfer in request.");
            uint requestId = IdGenerator.NewId();
            TS_URB_CONTROL_DESCRIPTOR_REQUEST des = new UrbBuilder(
                URB_FUNCTIONID.URB_FUNCTION_GET_DESCRIPTOR_FROM_DEVICE, 
                requestId, 
                0).BuildDeviceDescriptorRequest();
            rdpeusbAdapter.TransferInRequest(device, des, USB_DEVICE_DESCRIPTOR.DefaultSize);

            LogComment("7. Sends a cancel request with the request ID specified in the transfer in request.");
            rdpeusbAdapter.CancelRequest(device, requestId);

            LogComment("8. Expects not to receive a completion message.");
            EusbPdu pdu = rdpeusbAdapter.ExpectCompletion(device.VirtualChannel);
            if (null != pdu)
            {
                Site.Log.Add(
                    LogEntryKind.Debug,
                    "The Completion message is received. Callback interface ID: {0}, Completion Interface ID: {1}",
                    interfaceId,
                    pdu.InterfaceId
                    );
            }

            LogComment("9. Sends retract device request and the channel for the device is expected to be closed.");
            rdpeusbAdapter.RetractDevice(device, USB_RETRACT_REASON.UsbRetractReason_BlockedByPolicy);
        }
                
    }
}
