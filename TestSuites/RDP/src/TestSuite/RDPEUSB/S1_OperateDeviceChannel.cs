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
        [Description("Verify creating device channels and responding to the retract request.")]
        public void BVT_EUSB_OperateDeviceChannel()
        {
            LogComment("BVT_EUSB_OperateDeviceChannel");

            LogComment("1. Creates the control virtual channel, exchanges capabilities then notifies that the channel is created.");
            context.ControlChannel = CreateVirtualChannel();

            LogComment("2. Receives an add virtual channel request.");
            rdpeusbAdapter.ExpectAddVirtualChannel(context.ControlChannel);

            LogComment("3. Creates a new virtual channel for the device.");
            
            DynamicVirtualChannel channel =  CreateVirtualChannel();

            LogComment("4. Receives an add device request.");
            EusbDeviceContext device = rdpeusbAdapter.ExpectAddDevice(channel);

            LogComment("5. Sends retract device request and the channel for the device is expected to be closed.");
            rdpeusbAdapter.RetractDevice(device, USB_RETRACT_REASON.UsbRetractReason_BlockedByPolicy);
        }
 
        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPEUSB")]
        [Description("Verify responding to the invalid CHANNEL_CREATED message.")]
        public void S1_EUSB_RdpeusbOperateDeviceChannel_Invalid_MajorVersion()
        {
            LogComment("S1_EUSB_RdpeusbOperateDeviceChannel_Invalid_MajorVersion");

            LogComment("1. Creates the control channel.");
            context.ControlChannel = rdpeusbAdapter.CreateVirtualChannel();

            LogComment("2. Exchanges capabilities.");
            rdpeusbAdapter.NegotiateCapability(context.ControlChannel, IdGenerator.NewId(), CapabilityValue_Values.RIM_CAPABILITY_VERSION_01);
            
            LogComment("3. Sends a CHANNEL_CREATED message with an invalid major version. The virtual channel will be closed.");
            rdpeusbAdapter.ChannelCreated(context.ControlChannel, IdGenerator.NewId(), 0, 0, 0);
        }
    }
}
