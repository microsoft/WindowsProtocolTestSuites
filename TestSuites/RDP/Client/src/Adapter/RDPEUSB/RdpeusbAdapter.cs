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
using Microsoft.Protocols.TestSuites.Rdpbcgr;
using System.Threading;

namespace Microsoft.Protocols.TestSuites.Rdpeusb
{
    /// <summary>
    /// The protocol adapter implementation.
    /// </summary>
    public partial class RdpeusbAdapter : ManagedAdapterBase, IRdpeusbAdapter
    {
        #region Nested Type
           

        #endregion

        #region Private Members

        private RdpedycServer rdpedycServer;
        private RdpeusbServer rdpeusbServer;
        private TimeSpan waitTime;

        #endregion

        #region Constructor
        
        /// <summary>
        /// Constructor
        /// </summary>
        public RdpeusbAdapter()
        {
        }

        #endregion
        
        #region Helper Methods

        private void SendPdu(EusbPdu pdu, DynamicVirtualChannel channel)
        {
            Site.Assume.IsNotNull(channel, "DynamicVirtualChannel must be initialized.");

            channel.Send(PduMarshaler.Marshal(pdu));

            Site.Log.Add(LogEntryKind.Debug, "Sending {0}: \r\n{1}\r\n", pdu.GetType().ToString(), pdu.ToString());
        }

        private bool IsChannelClosed(DynamicVirtualChannel channel)
        {
            double timeout = Config.Timeout.TotalMilliseconds;
            const int wait = 100;

            Thread.Sleep(wait);
            timeout -= wait;
            while (timeout >= 0 && channel.IsActive)
            {
                Thread.Sleep(wait);
                timeout -= wait;
            }


            if (channel.IsActive)
            {
                Site.Log.Add(LogEntryKind.Debug, "The channel {0} is not closed within {1}", channel.ChannelId, Config.Timeout.ToString("c"));
            }

            return true;
        }

        private void LogPdu(EusbPdu pdu)
        {
            Site.Log.Add(LogEntryKind.Debug, "{0}\r\n{1}\r\n", pdu.GetType().ToString(), pdu.ToString());
        }

        #endregion

        #region IAdapter Members

        /// <summary>
        /// Inherited from ManagedAdapterBase.
        /// </summary>
        /// <param name="testSite"></param>
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
            Config.LoadConfig(testSite);

            #region WaitTime
            string strWaitTime = Site.Properties["WaitTime"];
            if (strWaitTime != null)
            {
                int waitSeconds = Int32.Parse(strWaitTime);
                waitTime = new TimeSpan(0, 0, waitSeconds);
            }
            else
            {
                waitTime = new TimeSpan(0, 0, 20);
            }
            #endregion
        }

        /// <summary>
        /// Inherited from ManagedAdapterBase.
        /// </summary>
        public override void Reset()
        {
            rdpedycServer = null;

            base.Reset();
        }

        /// <summary>
        /// Inherited from ManagedAdapterBase.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                rdpedycServer = null;
            }

            base.Dispose(disposing);
        }

        #endregion

        #region IRdpeusbAdapter Members

        /// <summary>
        /// Initialize this protocol with create control and data channels.
        /// </summary>
        /// <param name="rdpedycServer">RDPEDYC Server instance</param>
        /// <param name="transportType">selected transport type for created channels</param>
        /// <returns>true if client supports this protocol; otherwise, return false.</returns>
        public bool ProtocolInitialize(RdpedycServer rdpedycServer)
        {
            this.rdpedycServer = rdpedycServer;
            this.rdpeusbServer = new RdpeusbServer(rdpedycServer);
            return true;
        }

        /// <summary>
        /// Creates a dynamic virtual channel with specified channel ID.
        /// </summary>
        /// <param name="transportType">selected transport type for created channels</param>
        /// <returns>Instance of the DVC created</returns>
        public DynamicVirtualChannel CreateVirtualChannel(DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP)
        {
            if (!this.rdpedycServer.IsMultipleTransportCreated(transportType))
            {
                this.rdpedycServer.CreateMultipleTransport(transportType);
            }
            
            DynamicVirtualChannel rdpeusbDVC = null;
            try
            {
                rdpeusbDVC = this.rdpeusbServer.CreateRdpeusbDvc(waitTime, transportType);
                
            }
            catch (Exception e)
            {
                Site.Assert.Fail("Exception occurred when creating RDPEUSB channel: {0}.", e.Message);
            }
            return rdpeusbDVC;

           
        }
        
        /// <summary>
        /// Negotiates the capability through the specified channel with the connected RDP client.
        /// </summary>
        /// <param name="channel">The channel which the negotiation is sent in.</param>
        /// <param name="messageId">A unique ID for the request or response pair.</param>
        /// <param name="capability">The server's capability.</param>
        public void NegotiateCapability(DynamicVirtualChannel channel, uint messageId, CapabilityValue_Values capability)
        {
            Site.Log.Add(LogEntryKind.Debug, "Sending RIM_EXCHANGE_CAPABILITY_REQUEST: Channel ID {0}, Message ID {1}, Capability {2}.", channel.ChannelId, messageId, capability.ToString());

            EusbRimExchangeCapRequestPdu requestPdu = new EusbRimExchangeCapRequestPdu();
            requestPdu.MessageId = messageId;
            SendPdu(requestPdu, channel);

            if (capability == CapabilityValue_Values.RIM_CAPABILITY_VERSION_01)
            {
                // Positive tests so expect a valid response.
                Site.Log.Add(LogEntryKind.Debug, "Receiving EusbRimExchangeCapResponsePdu.");
                EusbRimExchangeCapResponsePdu responsePdu = this.rdpeusbServer.ExpectRdpeusbPdu<EusbRimExchangeCapResponsePdu>(channel.ChannelId, waitTime);

                #region Verify RIM_EXCHANGE_CAPABILITY_RESPONSE

                Site.Assert.IsNotNull(
                    responsePdu,
                    "Expect that the response from the client is EusbRimExchangeCapResponsePdu.");

                Site.Assert.AreEqual<uint>(
                    requestPdu.InterfaceId,
                    responsePdu.InterfaceId,
                    "Expect that the InterfaceId in the response PDU equals the InterfaceId in the request PDU. The actual value is 0x{0:x8}.",
                    responsePdu.InterfaceId);

                Site.Assert.AreEqual<uint>(
                    requestPdu.MessageId,
                    responsePdu.MessageId,
                    "Expect that the MessageId in the response PDU equals the MessageId in the request PDU. The actual value is 0x{0:x8}.",
                    responsePdu.MessageId);

                Site.Assert.AreEqual<Mask_Values>(
                    Mask_Values.STREAM_ID_NONE,
                    responsePdu.Mask,
                    "Expect that the Mask in the response PDU is STREAM_ID_NONE.");

                Site.Assert.AreEqual<uint>(
                    (uint)CapabilityValue_Values.RIM_CAPABILITY_VERSION_01,
                    responsePdu.CapabilityValue,
                    "Expect that the CapabilityValue in the response PDU is 0x00000001.");

                Site.Assert.AreEqual<uint>(
                    0,
                    responsePdu.Result,
                    "Expect that the Result in the response PDU is 0x00000000. The actual value is 0x{0:x8}.",
                    responsePdu.MessageId);

                #endregion
            }
            else
            {
                // Negative tests so expect the channel to be closed.
                bool channelClosed = IsChannelClosed(channel);
                Site.Assert.IsTrue(channelClosed, "Expect the channel {0} to be closed.", channel.ChannelId);
            }
        }

        /// <summary>
        /// Sends CHANNEL_CREATED to notify the client the channel has been created.
        /// </summary>
        /// <param name="channel">The channel to be sent in.</param>
        /// <param name="messageId">A unique ID for the request or response pair.</param>
        /// <param name="majorVersion">The major version of RDP USB redirection supported.</param>
        /// <param name="minorVersion">The minor version of RDP USB redirection supported.</param>
        /// <param name="capability">The capabilities of RDP USB redirection supported.</param>
        public void ChannelCreated(DynamicVirtualChannel channel, uint messageId, uint majorVersion, uint minorVersion, uint capability)
        {
            Site.Log.Add(
                LogEntryKind.Debug, 
                "Sending CHANNEL_CREATED: Channel ID {0}, Message ID {1}, Version major {2} minor {3}, Capability {4}.",
                channel.ChannelId,
                messageId,
                majorVersion,
                minorVersion,
                capability
            );

            EusbChannelCreatedPdu requestPdu = new EusbChannelCreatedPdu(true);
            requestPdu.MessageId = messageId;
            requestPdu.MajorVersion = majorVersion;
            requestPdu.MinorVersion = minorVersion;
            requestPdu.Capabilities = capability;

            SendPdu(requestPdu, channel);

            if (majorVersion == 1 && minorVersion == 0 && capability == 0)
            {
                // Positive tests so expect a valid response.
                Site.Log.Add(LogEntryKind.Debug, "Receiving CHANNEL_CREATED.");
                EusbChannelCreatedPdu responsePdu = this.rdpeusbServer.ExpectRdpeusbPdu<EusbChannelCreatedPdu>(channel.ChannelId, waitTime);


                #region Verify CHANNEL_CREATED Response

                Site.Assert.IsNotNull(
                    responsePdu,
                    "Expect that the response from the client is EusbChannelCreatedPdu.");
                
                // Log this PDU if responsePdu is not null
                LogPdu(responsePdu);

                Site.Assert.AreEqual<uint>(
                    0x000000003,
                    responsePdu.InterfaceId,
                    "Expect that the InterfaceId in the response PDU equals 0x000000003. The actual value is 0x{0:x8}.",
                    responsePdu.InterfaceId);

                Site.Assert.AreEqual<FunctionId_Values>(
                    FunctionId_Values.CHANNEL_CREATED,
                    (FunctionId_Values)responsePdu.FunctionId,
                    "Expect that the FunctionId in the response PDU is CHANNEL_CREATED. The actual value is 0x{0:x8}.",
                    responsePdu.FunctionId);

                Site.Assert.AreEqual<uint>(responsePdu.MajorVersion,
                    1,
                    "Expect that the MajorVersion in the response PDU is 1. The actual value is {0}.",
                    responsePdu.MajorVersion);

                Site.Assert.AreEqual<uint>(responsePdu.MinorVersion,
                    0,
                    "Expect that the MinorVersion in the response PDU is 0. The actual value is {0}.",
                    responsePdu.MinorVersion);

                Site.Assert.AreEqual<uint>(responsePdu.Capabilities,
                    0,
                    "Expect that the Capabilities in the response PDU is 0. The actual value is {0}.",
                    responsePdu.Capabilities);
                
                #endregion
            }
            else
            {
                // Negative tests so expect the channel to be closed.
                bool channelClosed = IsChannelClosed(channel);
                Site.Assert.IsTrue(channelClosed, "Expect the channel {0} to be closed.", channel.ChannelId);
            }
        }

        /// <summary>
        /// Receives the ADD_VIRTUAL_CHANNEL request from the client.
        /// </summary>
        /// <param name="channelId">The channel received in.</param>
        public void ExpectAddVirtualChannel(DynamicVirtualChannel channel)
        {
            Site.Log.Add(LogEntryKind.Debug, "Receiving ADD_VIRTUAL_CHANNEL: Channel ID {0}", channel.ChannelId);

            EusbAddVirtualChannelPdu responsePdu = this.rdpeusbServer.ExpectRdpeusbPdu<EusbAddVirtualChannelPdu>(channel.ChannelId, waitTime);
            
            #region Verify ADD_VIRTUAL_CHANNEL

            Site.Assert.IsNotNull(
                responsePdu,
                "Expect that the response from the client is EusbAddVirtualChannelPdu.");
            
            // Log this PDU if responsePdu is not null
            LogPdu(responsePdu);

            Site.Assert.AreEqual<uint>(
                0x00000001,
                responsePdu.InterfaceId,
                "Expect that the InterfaceId in the response PDU equals 0x00000001. The actual value is 0x{0:x8}.",
                responsePdu.InterfaceId);

            Site.Assert.AreEqual<Mask_Values>(
                Mask_Values.STREAM_ID_PROXY,
                responsePdu.Mask,
                "Expect that the Mask in the response PDU is STREAM_ID_PROXY.");

            Site.Assert.AreEqual<FunctionId_Values>(
                FunctionId_Values.ADD_VIRTUAL_CHANNEL,
                (FunctionId_Values)responsePdu.FunctionId,
                "Expect that the FunctionId in the response PDU is ADD_VIRTUAL_CHANNEL. The actual value is 0x{0:x8}.",
                responsePdu.FunctionId);

            #endregion
        }

        /// <summary>
        /// Receives the ADD_DEVICE request from the client.
        /// </summary>
        /// <param name="channel">The channel to be received from.</param>
        /// <returns>The context of the device which is being added.</returns>
        public EusbDeviceContext ExpectAddDevice(DynamicVirtualChannel channel)
        {
            Site.Log.Add(LogEntryKind.Debug, "Receiving ADD_DEVICE, Channel ID {0}.", channel.ChannelId);

            EusbAddDevicePdu pdu = this.rdpeusbServer.ExpectRdpeusbPdu<EusbAddDevicePdu>(channel.ChannelId, waitTime);
            USB_DEVICE_CAPABILITIES cap = new USB_DEVICE_CAPABILITIES();

            if (!PduMarshaler.Unmarshal(pdu.UsbDeviceCapabilities, cap))
            {
                Site.Assert.Fail(
                    "UsbDeviceCapabilities (size in bytes: {0}) in ADD_DEVICE cannot be decoded.",
                    pdu.UsbDeviceCapabilities.Length
                    );
            }

            #region Verify ADD_DEVICE

            Site.Assert.IsNotNull(
                pdu,
                "Expect that the response from the client is EusbAddDevicePdu."
                );

            Site.Assert.AreEqual<uint>(
                0x00000001,
                pdu.InterfaceId,
                "Expect that the InterfaceId in the response PDU equals 0x00000001. The actual value is 0x{0:x8}.",
                pdu.InterfaceId);

            Site.Assert.AreEqual<Mask_Values>(
                Mask_Values.STREAM_ID_PROXY,
                pdu.Mask,
                "Expect that the Mask in the response PDU is STREAM_ID_PROXY.");

            Site.Assert.AreEqual<FunctionId_Values>(
                FunctionId_Values.ADD_DEVICE,
                (FunctionId_Values)pdu.FunctionId,
                "Expect that the FunctionId in the response PDU is ADD_DEVICE. The actual value is 0x{0:x8}.",
                pdu.FunctionId);

            Site.Assert.AreEqual<uint>(
                0x00000001,
                pdu.NumUsbDevice,
                "Expect that the NumUsbDevice in the response PDU equals 0x00000001. The actual value is 0x{0:x8}.",
                pdu.NumUsbDevice);

            Site.Assert.AreNotEqual<uint>(
                0x00000000,
                pdu.cchDeviceInstanceId,
                "Expect that the cchDeviceInstanceId in the response PDU is zero. The actual value is 0x{0:x8}.",
                pdu.cchDeviceInstanceId);

            if (Config.IsWindowsImplementation)
            {
                Site.Assert.AreNotEqual<uint>(
                    0x00000000,
                    pdu.cchHwIds,
                    "Expect that the cchHwIds in the response PDU is not zero in an implementation of Windows. The actual value is 0x{0:x8}.",
                    pdu.cchHwIds);
            }

            #region Verify UsbDeviceCapabilities

            Site.Assert.AreEqual<uint>(
                (uint)USB_DEVICE_CAPABILITIES.USB_DEVICE_CAPABILITIES_SIZE,
                cap.CbSize,
                "Expect that the CbSize of the USB_DEVICE_CAPABILITIES in the response PDU equals 28. The actual value is {0}.",
                cap.CbSize);

            Site.Assert.IsTrue(
                cap.UsbBusInterfaceVersion == (uint)UsbBusInterfaceVersion_Values.USB_BUS_VERSION_0
                || cap.UsbBusInterfaceVersion == (uint)UsbBusInterfaceVersion_Values.USB_BUS_VERSION_1
                || cap.UsbBusInterfaceVersion == (uint)UsbBusInterfaceVersion_Values.USB_BUS_VERSION_2,
                "Expect that the UsbBusInterfaceVersion of the USB_DEVICE_CAPABILITIES in the response PDU equals 0x00000000 or 0x00000001 or 0x00000002. The actual value is {0}.",
                cap.UsbBusInterfaceVersion);

            Site.Assert.IsTrue(
                cap.USBDI_Version == (uint)USBDI_VER.USBDI_VERSION_5 || cap.USBDI_Version == (uint)USBDI_VER.USBDI_VERSION_6,
                "Expect that the USBDI_Version of the USB_DEVICE_CAPABILITIES in the response PDU equals 0x00000500 or 0x00000600. The actual value is {0}.",
                cap.USBDI_Version);

            Site.Assert.IsTrue(
                cap.Supported_USB_Version == (uint)Supported_USB_Version_Values.USB_1_0
                || cap.Supported_USB_Version == (uint)Supported_USB_Version_Values.USB_1_1
                || cap.Supported_USB_Version == (uint)Supported_USB_Version_Values.USB_2_0,
                "Expect that the Supported_USB_Version of the USB_DEVICE_CAPABILITIES in the response PDU equals 0x100 or 0x110 or 0x200. The actual value is {0}.",
                cap.Supported_USB_Version);

            Site.Assert.AreEqual<uint>(
                0,
                cap.HcdCapabilities,
                "Expect that the HcdCapabilities of the USB_DEVICE_CAPABILITIES in the response PDU is zero. The actual value is {0}.",
                cap.HcdCapabilities);

            if (cap.UsbBusInterfaceVersion == (uint)UsbBusInterfaceVersion_Values.USB_BUS_VERSION_0)
            {
                Site.Assert.AreEqual<uint>(
                    (uint)DeviceSpeed_Values.FULL_SPEED,
                    cap.DeviceIsHighSpeed,
                    "Expect that the DeviceIsHighSpeed of the USB_DEVICE_CAPABILITIES in the response PDU is zero when UsbBusInterfaceVersion is 0x00000000. The actual value is {0}.",
                    cap.HcdCapabilities);
            }
            else
            {
                Site.Assert.IsTrue(
                    cap.DeviceIsHighSpeed == (uint)DeviceSpeed_Values.FULL_SPEED
                    || cap.DeviceIsHighSpeed == (uint)DeviceSpeed_Values.HIGH_SPEED,
                    "Expect that the DeviceIsHighSpeed of the USB_DEVICE_CAPABILITIES in the response PDU equals 0x100 or 0x110 or 0x200. The actual value is {0}.",
                    cap.DeviceIsHighSpeed);
            }

            if (cap.NoAckIsochWriteJitterBufferSizeInMs != 0)
            {
                Site.Assert.IsTrue(
                    cap.NoAckIsochWriteJitterBufferSizeInMs >= 10
                    && cap.NoAckIsochWriteJitterBufferSizeInMs <= 512,
                    "Expect that the NoAckIsochWriteJitterBufferSizeInMs of the USB_DEVICE_CAPABILITIES in the response PDU is greater than or equal to 10 and less than or equal to 512. The actual value is {0}.",
                    cap.NoAckIsochWriteJitterBufferSizeInMs);
            }

            #endregion

            #endregion

            EusbDeviceContext device = new EusbDeviceContext();
            device.VirtualChannel = channel;
            device.NoAckIsochWriteJitterBufferSizeInMs = (cap.NoAckIsochWriteJitterBufferSizeInMs != 0);
            device.UsbDeviceInterfaceId = pdu.UsbDevice;
            device.DeviceInstanceId = pdu.DeviceInstanceId;

            Site.Log.Add(LogEntryKind.Debug, "Received device request. Device: {0}", device);
            return device;
        }

        /// <summary>
        /// Sends RETRACT_DEVICE request to the client to stop redirecting the USB device.
        /// </summary>
        /// <param name="device">The context of the device which is being operated.</param>
        /// <param name="reason">The reason to stop redirecting the USB device.</param>
        public void RetractDevice(EusbDeviceContext device, USB_RETRACT_REASON reason)
        {
            Site.Log.Add(LogEntryKind.Debug, "Sending RETRACT_DEVICE. Device: {0}", device);

            EusbRetractDevicePdu requestPdu = new EusbRetractDevicePdu(device.UsbDeviceInterfaceId);
            requestPdu.Reason = reason;
            SendPdu(requestPdu, device.VirtualChannel);

            bool channelClosed = IsChannelClosed(device.VirtualChannel);
            Site.Assert.IsTrue(channelClosed, "Expect the channel {0} to be closed.", device.VirtualChannel.ChannelId);
        }
        
        /// <summary>
        /// Queries the USB device's text from the client.
        /// </summary>
        /// <param name="device">The context of the device which is being operated.</param>
        /// <param name="textType">This value represents the type of text to query as described in [MSFT-W2KDDK], Volume 1, 
        /// Part 1, Chapter 2.</param>
        /// <param name="localeId">This value represents the locale of the text to query as described in [MSFT-W2KDDK], 
        /// Volume 1, Part 1, Chapter 2.</param>
        public void QueryDeviceText(EusbDeviceContext device, uint textType, uint localeId)
        {
            Site.Log.Add(LogEntryKind.Debug, "Sending QUERY_DEVICE_TEXT. Device: {0}, Text type: {1}, Locale ID: {2}.", device, textType, localeId);

            EusbQueryDeviceTextRequestPdu requestPdu = new EusbQueryDeviceTextRequestPdu(device.UsbDeviceInterfaceId);
            requestPdu.TextType = textType;
            requestPdu.LocaleId = localeId;
            SendPdu(requestPdu, device.VirtualChannel);

            Site.Log.Add(LogEntryKind.Debug, "Receiving EusbQueryDeviceTextResponsePdu.");
            EusbQueryDeviceTextResponsePdu responsePdu = this.rdpeusbServer.ExpectRdpeusbPdu<EusbQueryDeviceTextResponsePdu>(device.VirtualChannel.ChannelId, waitTime);

            #region Verify QUERY_DEVICE_TEXT_RSP

            Site.Assert.IsNotNull(
                responsePdu,
                "Expect that the response from the client is EusbQueryDeviceTextResponsePdu.");

            Site.Assert.AreEqual<uint>(
                requestPdu.InterfaceId,
                responsePdu.InterfaceId,
                "Expect that the InterfaceId in the response PDU equals the InterfaceId in the request. The actual value is 0x{0:x8}.",
                responsePdu.InterfaceId);

            Site.Assert.AreEqual<uint>(
                requestPdu.MessageId,
                responsePdu.MessageId,
                "Expect that the MessageId in the response PDU equals the MessageId in the request. The actual value is 0x{0:x8}.",
                responsePdu.MessageId);

            Site.Assert.AreEqual<Mask_Values>(
                Mask_Values.STREAM_ID_STUB,
                responsePdu.Mask,
                "Expect that the Mask in the response PDU is STREAM_ID_STUB.");

            Site.Assert.AreNotEqual<uint>(
                0x00000000,
                responsePdu.cchDeviceDescription,
                "Expect that the cchDeviceDescription in the response PDU is not zero. The actual value is 0x{0:x8}.",
                responsePdu.cchDeviceDescription);

            Site.Assert.AreEqual<uint>(
                0x00000000,
                responsePdu.HResult,
                "Expect that the HResult in the response PDU is zero. The actual value is 0x{0:x8}.",
                responsePdu.HResult);

            #endregion
        }

        /// <summary>
        /// Registers a request completion interface on the client for a specific device.
        /// </summary>
        /// <param name="device">The information of the device which is being operated.</param>
        /// <param name="numRequestCompletion">If this field is set to 0x00000001 or greater, then the RequestCompletion field
        /// is also present. If this field is set to 0x0000000, the RequestCompletion field is not present.</param>
        /// <param name="requestCompletion">A unique InterfaceID to be used by all Request Completion messages defined in the
        /// Request Completion Interface.</param>
        public void RegisterCallback(EusbDeviceContext device, uint numRequestCompletion, uint requestCompletion)
        {
            Site.Log.Add(
                LogEntryKind.Debug, 
                "Sending REGISTER_REQUEST_CALLBACK. Device: {0}, Num request Completion {1}, Request completion {2}.", 
                device,
                numRequestCompletion,
                requestCompletion
                );

            EusbRegisterRequestCallbackPdu requestPdu = new EusbRegisterRequestCallbackPdu(
                device.UsbDeviceInterfaceId, 
                numRequestCompletion, 
                requestCompletion
                );
            SendPdu(requestPdu, device.VirtualChannel);

            // Record the interface ID for parsing Completion UDPs.
            rdpeusbServer.RequestCompletionInterfaceId = requestCompletion;
        }

        /// <summary>
        /// Submits an I/O control request to the client USB device.
        /// </summary>
        /// <param name="device">Device instance for this operation</param>
        /// <param name="input">This value represents the input buffer for the IO control request.</param>
        /// <param name="outputBufferSize">The maximum number of bytes the client can return to the server.</param>
        /// <param name="requestId">This ID uniquely identifies the I/O control request.</param>
        public void IoControl(EusbDeviceContext device, UsbIoControlCode ioControlCode, byte[] input, uint outputBufferSize, uint requestId)
        {
            Site.Log.Add(
                LogEntryKind.Debug, 
                "Sending IO_CONTROL. Device: {0}, IO control code: {1}, Input buffer size {2}, Output buffer size {3}, Request ID {4}.", 
                device, 
                ioControlCode,
                null == input ? 0 : input.Length, 
                outputBufferSize, 
                requestId
                );

            EusbIoControlPdu requestPdu = new EusbIoControlPdu(device.UsbDeviceInterfaceId);
            requestPdu.IoControlCode = ioControlCode;
            if (input == null)
            {
                requestPdu.InputBuffer = null;
                requestPdu.InputBufferSize = 0;
            }
            else
            {
                requestPdu.InputBuffer = (byte[])input;
                requestPdu.InputBufferSize = (uint)input.Length;
            }
            requestPdu.OutputBufferSize = outputBufferSize;
            requestPdu.RequestId = requestId;

            SendPdu(requestPdu, device.VirtualChannel);
        }

        /// <summary>
        /// Submits an internal I/O control request to the client USB device.
        /// </summary>
        /// <param name="device">Device instance for this operation</param>
        /// <param name="input">This value represents the input buffer for the IO control request.</param>
        /// <param name="outputBufferSize">The maximum number of bytes the client can return to the server.</param>
        /// <param name="requestId">This ID uniquely identifies the internal I/O control request.</param>
        public void InternalIoControl(EusbDeviceContext device, UsbInternalIoControlCode ioControlCode, byte[] input, uint outputBufferSize, uint requestId)
        {
            Site.Log.Add(
                LogEntryKind.Debug,
                "Sending INTERNAL_IO_CONTROL. Device: {0}, IO control code: {1}, Input buffer size {2}, Output buffer size {3}, Request ID {4}.",
                device, 
                ioControlCode,
                null == input ? 0 : input.Length,
                outputBufferSize,
                requestId
                );

            EusbInternalIoControlPdu requestPdu = new EusbInternalIoControlPdu(device.UsbDeviceInterfaceId);
            requestPdu.IoControlCode = ioControlCode;
            if (input == null)
            {
                requestPdu.InputBuffer = null;
                requestPdu.InputBufferSize = 0;
            }
            else
            {
                requestPdu.InputBuffer = (byte[])input.Clone();
                requestPdu.InputBufferSize = (uint)input.Length;
            }
            requestPdu.OutputBufferSize = outputBufferSize;
            requestPdu.RequestId = requestId;

            SendPdu(requestPdu, device.VirtualChannel);
        }

        /// <summary>
        /// Requests data from the client USB device.
        /// </summary>
        /// <param name="device">The context of the device which is being operated.</param>
        /// <param name="tsUrb">A TS_URB structure.</param>
        /// <param name="outputBufferSize">This value represents the maximum number of bytes of data that is requested from
        /// the USB device.</param>
        public void TransferInRequest(EusbDeviceContext device, TS_URB tsUrb, uint outputBufferSize)
        {
            // TODO: Check this verification.
            //Site.Assume.IsFalse(
            //    tsUrb is TS_URB_ISOCH_TRANSFER && device.NoAckIsochWriteJitterBufferSizeInMs,
            //    "The client does not support TS_URB_ISOCH_TRANSFER messages."
            //    );

            Site.Log.Add(
                LogEntryKind.Debug, 
                "Sending TRANSFER_IN_REQUEST. Device: {0}, URB: {1}, Output buffer size: {2}", 
                device, 
                tsUrb, 
                outputBufferSize);

            EusbTransferInRequestPdu requestPdu = new EusbTransferInRequestPdu(device.UsbDeviceInterfaceId);
            byte[] buf = PduMarshaler.Marshal(tsUrb);
            requestPdu.CbTsUrb = (uint)buf.Length;

            // TODO: Need verify for negtive test cases?
            if (tsUrb.Header.Size != (ushort)requestPdu.CbTsUrb)
            {
                throw new ArgumentException(String.Format(
                    "Header.Size ({0}) doesn't match the actual size ({1}).", 
                    tsUrb.Header.Size, 
                    requestPdu.CbTsUrb
                    ));
            }
            
            requestPdu.TsUrb = buf;
            requestPdu.OutputBufferSize = outputBufferSize;
            SendPdu(requestPdu, device.VirtualChannel);
        }

        /// <summary>
        /// Submits data to the client USB device.
        /// </summary>
        /// <param name="device">The context of the device which is being operated.</param>
        /// <param name="tsUrb">A TS_URB structure.</param>
        /// <param name="outputBuffer">The raw data to be sent to the device.</param>
        public void TransferOutRequest(EusbDeviceContext device, TS_URB tsUrb, byte[] outputBuffer)
        {
            uint size = null == outputBuffer ? 0 : (uint)outputBuffer.Length;
            Site.Log.Add(
                LogEntryKind.Debug,
                "Sending TRANSFER_OUT_REQUEST. Device: {0}, URB: {1}, Output buffer size: {2}",
                device,
                tsUrb,
                size);

            EusbTransferOutRequestPdu requestPdu = new EusbTransferOutRequestPdu(device.UsbDeviceInterfaceId);
            byte[] buf = PduMarshaler.Marshal(tsUrb);
            requestPdu.CbTsUrb = (uint)buf.Length;

            // TODO: Need verify for negtive test cases?
            if (tsUrb.Header.Size != (ushort)requestPdu.CbTsUrb)
            {
                throw new ArgumentException(String.Format(
                    "Header.Size ({0}) doesn't match the actual size ({1}).",
                    tsUrb.Header.Size,
                    requestPdu.CbTsUrb
                    ));
            }

            requestPdu.TsUrb = buf;
            requestPdu.OutputBuffer = outputBuffer;
            requestPdu.OutputBufferSize = size;
            SendPdu(requestPdu, device.VirtualChannel);
        }

        /// <summary>
        /// Cancels an outstanding IO request.
        /// </summary>
        /// <param name="device">The context of the device which is being operated.</param>
        /// <param name="requestId">This value represents the ID of a request previously sent via IO_CONTROL, INTERNAL_IO_CONTROL,
        /// TRANSFER_IN_REQUEST, or TRANSFER_OUT_REQUEST message.</param>
        public void CancelRequest(EusbDeviceContext device, uint requestId)
        {
            Site.Log.Add(LogEntryKind.Debug, "Sending CANCEL_REQUEST. Device: {0}, Request ID {1}.", device, requestId);

            EusbCancelRequestPdu requestPdu = new EusbCancelRequestPdu();
            requestPdu.RequestId = requestId;

            SendPdu(requestPdu, device.VirtualChannel);
        }

        /// <summary>
        /// Receives an IOCONTROL_COMPLETION, URB_COMPLETION or URB_COMPLETION_NO_DATA request from the client.
        /// </summary>
        /// <param name="channel">The channel to be received in.</param>
        /// <returns>The received completion PDU. Returns null, if timeout or required .</returns>
        public EusbPdu ExpectCompletion(DynamicVirtualChannel channel)
        {
            EusbPdu pdu = this.rdpeusbServer.ExpectRdpeusbPdu<EusbPdu>(channel.ChannelId, waitTime);

            if (null == pdu)
            {
                return null;
            }

            bool isCompletionPdu = (
                (pdu is EusbIoControlCompletionPdu) || 
                (pdu is EusbUrbCompletionPdu) || 
                (pdu is EusbUrbCompletionNoDataPdu)
                );

            Site.Assert.IsTrue(isCompletionPdu, "Expect a completion PDU, current pdu is {0}.", pdu.ToString());

            return pdu;
        }
        
        #endregion
    }
}
