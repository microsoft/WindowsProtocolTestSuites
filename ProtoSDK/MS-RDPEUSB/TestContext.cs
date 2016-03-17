// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeusb
{
    /// <summary>
    /// Stores the properties of a device.
    /// </summary>
    public class EusbDeviceContext
    {
        /// <summary>
        /// The channel which the current device is used.
        /// </summary>
        public DynamicVirtualChannel VirtualChannel { get; set; }

        /// <summary>
        /// The interface ID of the current device.
        /// </summary>
        public uint UsbDeviceInterfaceId { get; set; }

        /// <summary>
        /// The instance ID of the current device.
        /// </summary>
        public string DeviceInstanceId { get; set; }

        /// <summary>
        /// This field specifies if the client supports TS_URB_ISOCH_TRANSFER messages that do not expect URB_COMPLETION messages.
        /// If the value is nonzero, the client supports TS_URB_ISOCH_TRANSFER messages that do not expect URB_COMPLETION messages;
        /// otherwise, if the value is zero, the client does not support TS_URB_ISOCH_TRANSFER messages. 
        /// If the value is not zero, the value represents the amount of outstanding isochronous data the client expects from the 
        /// server. If this value is nonzero, it MUST be greater than or equal to 10 and less than or equal to 512.
        /// </summary>
        public bool NoAckIsochWriteJitterBufferSizeInMs { get; set; }

        /// <summary>
        /// Inherited from Object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format(
                "Channel ID {0}, Interface ID {1}, Instance ID {2}, Support TS_URB_ISOCH_TRANSFER {3}.",
                VirtualChannel.ChannelId,
                UsbDeviceInterfaceId,
                DeviceInstanceId,
                NoAckIsochWriteJitterBufferSizeInMs
                );
        }
    }

    /// <summary>
    /// Stores states and contexts of a test case.
    /// </summary>
    public class EusbTestContext
    {
        /// <summary>
        /// controling channel.
        /// </summary>
        public DynamicVirtualChannel ControlChannel { get; set; }

        /// <summary>
        /// The device context container, which is indexed by the channel IDs.
        /// </summary>
        public Dictionary<uint, EusbDeviceContext> DeviceContexts = new Dictionary<uint, EusbDeviceContext>();

        private uint nextChannelId = 2;

        /// <summary>
        /// Generate the next unused channel ID.
        /// </summary>
        /// <returns>Next avaliable channel ID.</returns>
        public uint NewChannelId()
        {
            if (nextChannelId == uint.MaxValue)
            {
                nextChannelId = 0;
                return uint.MaxValue;
            }
            return nextChannelId++;
        }

        /// <summary>
        /// Stores the result of configuration-selection.
        /// </summary>
        public TS_URB_SELECT_CONFIGURATION_RESULT SelectedConfig { get; set; }
    }
}
