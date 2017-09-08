// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeusb;

namespace Microsoft.Protocols.TestSuites.Rdpeusb
{
    /// <summary>
    /// The protocol adapter.
    /// </summary>
    public interface IRdpeusbAdapter : IAdapter
    {
        /// <summary>
        /// Initialize this protocol with create control and data channels.
        /// </summary>
        /// <param name="rdpedycServer">RDPEDYC Server instance</param>
        /// <param name="transportType">selected transport type for created channels</param>
        /// <returns>true if client supports this protocol; otherwise, return false.</returns>
        bool ProtocolInitialize(RdpedycServer rdpedycServer);

        /// <summary>
        /// Creates a dynamic virtual channel with specified channel ID.
        /// </summary>
        /// <param name="transportType">selected transport type for created channels</param>
        /// <returns>Instance of the DVC created</returns>
        DynamicVirtualChannel CreateVirtualChannel(DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP);

        /// <summary>
        /// Negotiates the capability through the specified channel with the connected RDP client.
        /// </summary>
        /// <param name="channel">The channel which the negotiation is sent in.</param>
        /// <param name="messageId">A unique ID for the request or response pair.</param>
        /// <param name="capability">The server's capability.</param>
        void NegotiateCapability(DynamicVirtualChannel channel, uint messageId, CapabilityValue_Values capability);

        /// <summary>
        /// Sends CHANNEL_CREATED to notify the client the channel has been created.
        /// </summary>
        /// <param name="channel">The channel to be sent in.</param>
        /// <param name="messageId">A unique ID for the request or response pair.</param>
        /// <param name="majorVersion">The major version of RDP USB redirection supported.</param>
        /// <param name="minorVersion">The minor version of RDP USB redirection supported.</param>
        /// <param name="capability">The capabilities of RDP USB redirection supported.</param>
        void ChannelCreated(DynamicVirtualChannel channel, uint messageId, uint majorVersion, uint minorVersion, uint capability);

        /// <summary>
        /// Receives the ADD_VIRTUAL_CHANNEL request from the client.
        /// </summary>
        /// <param name="channel">The channel received in.</param>
        void ExpectAddVirtualChannel(DynamicVirtualChannel channel);

        /// <summary>
        /// Receives the ADD_DEVICE request from the client.
        /// </summary>
        /// <param name="channel">The channel to be received from.</param>
        /// <returns>The context of the device which is being added.</returns>
        EusbDeviceContext ExpectAddDevice(DynamicVirtualChannel channel);

        /// <summary>
        /// Sends RETRACT_DEVICE request to the client to stop redirecting the USB device.
        /// </summary>
        /// <param name="device">The context of the device which is being operated.</param>
        /// <param name="reason">The reason to stop redirecting the USB device.</param>
        void RetractDevice(EusbDeviceContext device, USB_RETRACT_REASON reason);

        /// <summary>
        /// Queries the USB device's text from the client.
        /// </summary>
        /// <param name="device">The context of the device which is being operated.</param>
        /// <param name="textType">This value represents the type of text to query as described in [MSFT-W2KDDK], Volume 1, 
        /// Part 1, Chapter 2.</param>
        /// <param name="localeId">This value represents the locale of the text to query as described in [MSFT-W2KDDK], 
        /// Volume 1, Part 1, Chapter 2.</param>
        void QueryDeviceText(EusbDeviceContext device, uint textType, uint localeId);

        /// <summary>
        /// Registers a request completion interface on the client for a specific device.
        /// </summary>
        /// <param name="device">The information of the device which is being operated.</param>
        /// <param name="numRequestCompletion">If this field is set to 0x00000001 or greater, then the RequestCompletion field
        /// is also present. If this field is set to 0x0000000, the RequestCompletion field is not present.</param>
        /// <param name="requestCompletion">A unique InterfaceID to be used by all Request Completion messages defined in the
        /// Request Completion Interface.</param>
        void RegisterCallback(EusbDeviceContext device, uint numRequestCompletion, uint requestCompletion);

        /// <summary>
        /// Submits an I/O control request to the client USB device.
        /// </summary>
        /// <param name="channelId">The ID of the channel to be sent in.</param>
        /// <param name="ioControlCode">This I/O control code specifies what operation is requested in the I/O request.</param>
        /// <param name="input">This value represents the input buffer for the IO control request.</param>
        /// <param name="outputBufferSize">The maximum number of bytes the client can return to the server.</param>
        /// <param name="requestId">This ID uniquely identifies the I/O control request.</param>
        void IoControl(EusbDeviceContext device, UsbIoControlCode ioControlCode, byte[] input, uint outputBufferSize, uint requestId);

        /// <summary>
        /// Submits an internal I/O control request to the client USB device.
        /// </summary>
        /// <param name="channelId">The ID of the channel to be sent in.</param>
        /// <param name="ioControlCode">This internal I/O control code specifies what operation is requested in the internal
        /// I/O request.</param>
        /// <param name="input">This value represents the input buffer for the IO control request.</param>
        /// <param name="outputBufferSize">The maximum number of bytes the client can return to the server.</param>
        /// <param name="requestId">This ID uniquely identifies the internal I/O control request.</param>
        void InternalIoControl(EusbDeviceContext device, UsbInternalIoControlCode ioControlCode, byte[] input, uint outputBufferSize, uint requestId);

        /// <summary>
        /// Requests data from the client USB device.
        /// </summary>
        /// <param name="device">The context of the device which is being operated.</param>
        /// <param name="tsUrb">A TS_URB structure.</param>
        /// <param name="outputBufferSize">This value represents the maximum number of bytes of data that is requested from
        /// the USB device.</param>
        void TransferInRequest(EusbDeviceContext device, TS_URB tsUrb, uint outputBufferSize);

        /// <summary>
        /// Submits data to the client USB device.
        /// </summary>
        /// <param name="device">The context of the device which is being operated.</param>
        /// <param name="tsUrb">A TS_URB structure.</param>
        /// <param name="outputBuffer">The raw data to be sent to the device.</param>
        void TransferOutRequest(EusbDeviceContext device, TS_URB tsUrb, byte[] outputBuffer);

        /// <summary>
        /// Cancels an outstanding IO request.
        /// </summary>
        /// <param name="device">The context of the device which is being operated.</param>
        /// <param name="requestId">This value represents the ID of a request previously sent via IO_CONTROL, INTERNAL_IO_CONTROL,
        /// TRANSFER_IN_REQUEST, or TRANSFER_OUT_REQUEST message.</param>
        void CancelRequest(EusbDeviceContext device, uint requestId);

        /// <summary>
        /// Receives an IOCONTROL_COMPLETION, URB_COMPLETION or URB_COMPLETION_NO_DATA request from the client.
        /// </summary>
        /// <param name="channel">The channel to be received in.</param>
        /// <returns>The received completion PDU.</returns>
        EusbPdu ExpectCompletion(DynamicVirtualChannel channel);
    }
}
