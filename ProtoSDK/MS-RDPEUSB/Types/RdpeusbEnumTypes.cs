// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeusb
{
    #region Basic Types

    /// <summary>
    /// The errors from Windows.
    /// </summary>
    public enum HRESULT_FROM_WIN32 : uint
    {
        /// <summary>
        /// The action completed successfully.
        /// </summary>
        ERROR_SUCCESS = 0x00000000,

        /// <summary>
        /// HRESULT_FROM_WIN32(ERROR_INSUFFICIENT_BUFFER)
        /// The buffer is not large enough.
        /// </summary>
        ERROR_INSUFFICIENT_BUFFER = 0x8007007A,
    }

    /// <summary>
    /// The types of MS-RDPEUSB PDUs.
    /// </summary>
    public enum EusbType : uint
    {
        UNKNOWN = 0,
        RIMCALL_RELEASE = 1,
        RIMCALL_QUERYINTERFACE,
        RIM_EXCHANGE_CAPABILITY_REQUEST,
        IOCONTROL_COMPLETION,
        URB_COMPLETION,
        URB_COMPLETION_NO_DATA,
        CANCEL_REQUEST,
        REGISTER_REQUEST_CALLBACK,
        IO_CONTROL,
        INTERNAL_IO_CONTROL,
        QUERY_DEVICE_TEXT,
        TRANSFER_IN_REQUEST,
        TRANSFER_OUT_REQUEST,
        RETRACT_DEVICE,
        ADD_VIRTUAL_CHANNEL,
        ADD_DEVICE,
        CHANNEL_CREATED,
    }

    /// <summary>
    /// The values of Mask.
    /// </summary>
    public enum Mask_Values : byte
    {
        /// <summary>
        /// Indicates that the SHARED_MSG_HEADER is being used for interface manipulation capabilities exchange
        /// as specified in section 2.2.3. This value MUST NOT be used for any other messages.
        /// </summary>
        STREAM_ID_NONE = 0x00,

        /// <summary>
        /// Indicates that the SHARED_MSG_HEADER is not being used in a response message.
        /// </summary>
        STREAM_ID_PROXY = 0x01,

        /// <summary>
        /// Indicates that the SHARED_MSG_HEADER is being used in a response message.
        /// </summary>
        STREAM_ID_STUB = 0x02,

        /// <summary>
        /// Indicates that the SHARED_MSG_HEADER is invalid.
        /// </summary>
        STREAM_ID_INVALID = 0x03,
    }

    /// <summary>
    /// This field MUST be present in all packets except response packets. Its value is either used in interface
    /// manipulation messages or defined for a specific interface. 
    /// </summary>
    public enum FunctionId_Values : uint
    {
        // Common IDs for all interfaces are as follows.

        /// <summary>
        /// Release the given interface ID.
        /// </summary>
        RIMCALL_RELEASE = 0x00000001,

        /// <summary>
        /// Query for a new interface.
        /// </summary>
        RIMCALL_QUERYINTERFACE = 0x00000002,

        // Capabilities Negotiator Interface IDs are as follows.

        /// <summary>
        /// The server sends the Interface Manipulation Exchange Capabilities Request message.
        /// </summary>
        RIM_EXCHANGE_CAPABILITY_REQUEST = 0x00000100,

        // Client Request Completion Interface IDs are as follows.

        /// <summary>
        /// The client sends the IO Control Completion message.
        /// </summary>
        IOCONTROL_COMPLETION = 0x00000100,

        /// <summary>
        /// The client sends the URB Completion message.
        /// </summary>
        URB_COMPLETION = 0x00000101,

        /// <summary>
        /// The client sends the URB Completion No Data message.
        /// </summary>
        URB_COMPLETION_NO_DATA = 0x00000102,

        // Server USB Device Interface IDs are as follows.

        /// <summary>
        /// The server sends the Cancel Request message.
        /// </summary>
        CANCEL_REQUEST = 0x00000100,

        /// <summary>
        /// The server sends the Register Request Callback message.
        /// </summary>
        REGISTER_REQUEST_CALLBACK = 0x00000101,

        /// <summary>
        /// The server sends the IO Control message.
        /// </summary>
        IO_CONTROL = 0x00000102,

        /// <summary>
        /// The server sends the Internal IO Control message.
        /// </summary>
        INTERNAL_IO_CONTROL = 0x00000103,

        /// <summary>
        /// The server sends the Query Device Text message.
        /// </summary>
        QUERY_DEVICE_TEXT = 0x00000104,

        /// <summary>
        /// The server sends the Transfer In Request message.
        /// </summary>
        TRANSFER_IN_REQUEST = 0x00000105,

        /// <summary>
        /// The server sends the Transfer Out Request message.
        /// </summary>
        TRANSFER_OUT_REQUEST = 0x00000106,

        /// <summary>
        /// The server sends the Retract Device message.
        /// </summary>
        RETRACT_DEVICE = 0x00000107,

        // Client Device Sink Interface IDs are as follows.

        /// <summary>
        /// The client sends the Add Virtual Channel message.
        /// </summary>
        ADD_VIRTUAL_CHANNEL = 0x00000100,

        /// <summary>
        /// The client sends the Add Device message.
        /// </summary>
        ADD_DEVICE = 0x00000101,

        // Channel Notification Interface IDs are as follows.

        /// <summary>
        /// The server and the client send the Channel Created message.
        /// </summary>
        CHANNEL_CREATED = 0x00000100,
    }

    /// <summary>
    /// Interface ID in the SHARED_MSG_HEADER
    /// </summary>
    public enum EusbInterfaceId_Values : uint
    {
        /// <summary>
        /// Interface ID of exchange capability request
        /// </summary>
        RIM_EXCHANGE_CAPABILITY_REQUEST = 0x00000000,

        /// <summary>
        /// Interface ID of add a virtual channel or add a device on server
        /// </summary>
        ADD_VIRTUAL_CHANNEL_OR_DEVICE = 0x00000001,

        /// <summary>
        /// Interface ID of channel created that is sent by server
        /// </summary>
        CHANNEL_CREATED_SERVER = 0x00000002,

        /// <summary>
        /// Interface ID of channel created that is sent by client
        /// </summary>
        CHANNEL_CREATED_CLIENT = 0x00000003,
    };

    /// <summary>
    /// A 32-bit unsigned integer that identifies the server's capability.
    /// </summary>
    public enum CapabilityValue_Values : uint
    {
        /// <summary>
        /// Invalid capability 0.
        /// </summary>
        RIM_CAPABILITY_VERSION_INVALID_ZERO = 0x00000000,

        /// <summary>
        /// This capability MUST be present in the message.
        /// </summary>
        RIM_CAPABILITY_VERSION_01 = 0x00000001,
    }

    /// <summary>
    /// The USB version.
    /// </summary>
    public enum UsbBusInterfaceVersion_Values : uint
    {
        /// <summary>
        /// The Version of USB's BUS is 0
        /// </summary>
        USB_BUS_VERSION_0 = 0x00000000,

        /// <summary>
        /// The Version of USB's BUS is 1.0
        /// </summary>
        USB_BUS_VERSION_1 = 0x00000001,

        /// <summary>
        /// The Version of USB's BUS is 2.0
        /// </summary>
        USB_BUS_VERSION_2 = 0x00000002
    }

    /// <summary>
    /// The version of USB.
    /// </summary>
    public enum Supported_USB_Version_Values : uint
    {
        /// <summary>
        /// USB 1.0
        /// </summary>
        USB_1_0 = 0x100,

        /// <summary>
        /// USB 1.1
        /// </summary>
        USB_1_1 = 0x110,

        /// <summary>
        /// USB 2.0
        /// </summary>
        USB_2_0 = 0x200,
    }

    /// <summary>
    /// The version of USB.
    /// </summary>
    public enum DeviceSpeed_Values : uint
    {
        /// <summary>
        /// The device is full speed.
        /// </summary>
        FULL_SPEED = 0x00000000,

        /// <summary>
        /// The device is high speed.
        /// </summary>
        HIGH_SPEED = 0x00000001,
    }

    public enum USB_RETRACT_REASON : uint
    {
        /// <summary>
        /// The USB device is to be stopped from being redirected because the device is blocked by the server's policy.
        /// </summary>
        UsbRetractReason_BlockedByPolicy = 0x00000001,
    }

    /// <summary>
    /// The ID of the URB function
    /// </summary>
    public enum URB_FUNCTIONID : ushort
    {
        /// <summary>
        /// A configuration is to be selected
        /// </summary>
        URB_FUNCTION_SELECT_CONFIGURATION = 0x0000,

        /// <summary>
        /// An alternate interface setting is being selected for an interface
        /// </summary>
        URB_FUNCTION_SELECT_INTERFACE = 0x0001,

        /// <summary>
        /// All outstanding requests for a pipe should be canceled
        /// </summary>
        URB_FUNCTION_ABORT_PIPE = 0x0002,

        /// <summary>
        /// Used with _URB_FRAME_LENGTH_CONTROL as the data structure.
        /// </summary>
        URB_FUNCTION_TAKE_FRAME_LENGTH_CONTROL = 0x0003,

        /// <summary>
        /// It was used with _URB_FRAME_LENGTH_CONTROL as the data structure
        /// </summary>
        URB_FUNCTION_RELEASE_FRAME_LENGTH_CONTROL = 0x0004,

        /// <summary>
        /// It was used with _URB_GET_FRAME_LENGTH as the data structure
        /// </summary>
        URB_FUNCTION_GET_FRAME_LENGTH = 0x0005,

        /// <summary>
        /// It was used with _URB_SET_FRAME_LENGTH as the data structure
        /// </summary>
        URB_FUNCTION_SET_FRAME_LENGTH = 0x0006,

        /// <summary>
        /// Requests the current frame number from the host controller driver
        /// </summary>
        URB_FUNCTION_GET_CURRENT_FRAME_NUMBER = 0x0007,

        /// <summary>
        /// Transfers data to or from a control pipe
        /// </summary>
        URB_FUNCTION_CONTROL_TRANSFER = 0x0008,

        /// <summary>
        /// Transfers data from a bulk pipe or interrupt pipe or to an bulk pipe
        /// </summary>
        URB_FUNCTION_BULK_OR_INTERRUPT_TRANSFER = 0x0009,

        /// <summary>
        /// Transfers data to or from an isochronous pipe
        /// </summary>
        URB_FUNCTION_ISOCH_TRANSFER = 0x000A,

        /// <summary>
        /// Retrieves the device descriptor from a specific USB device
        /// </summary>
        URB_FUNCTION_GET_DESCRIPTOR_FROM_DEVICE = 0x000B,

        /// <summary>
        /// Sets a device descriptor on a device
        /// </summary>
        URB_FUNCTION_SET_DESCRIPTOR_TO_DEVICE = 0x000C,

        /// <summary>
        /// Sets a USB-defined feature on a device
        /// </summary>
        URB_FUNCTION_SET_FEATURE_TO_DEVICE = 0x000D,

        /// <summary>
        /// Sets a USB-defined feature on an interface for a device
        /// </summary>
        URB_FUNCTION_SET_FEATURE_TO_INTERFACE = 0x000E,

        /// <summary>
        /// Sets a USB-defined feature on an endpoint for an interface on a USB device
        /// </summary>
        URB_FUNCTION_SET_FEATURE_TO_ENDPOINT = 0x000F,

        /// <summary>
        /// Clears a USB-defined feature on a device
        /// </summary>
        URB_FUNCTION_CLEAR_FEATURE_TO_DEVICE = 0x0010,

        /// <summary>
        /// Clears a USB-defined feature on an interface for a device
        /// </summary>
        URB_FUNCTION_CLEAR_FEATURE_TO_INTERFACE = 0x0011,

        /// <summary>
        /// Clears a USB-defined feature on an endpoint, for an interface, on a USB device
        /// </summary>
        URB_FUNCTION_CLEAR_FEATURE_TO_ENDPOINT = 0x0012,

        /// <summary>
        /// Retrieves status from a USB device
        /// </summary>
        URB_FUNCTION_GET_STATUS_FROM_DEVICE = 0x0013,

        /// <summary>
        /// Retrieves status from an interface on a USB device
        /// </summary>
        URB_FUNCTION_GET_STATUS_FROM_INTERFACE = 0x0014,

        /// <summary>
        /// Retrieves status from an endpoint for an interface on a USB device
        /// </summary>
        URB_FUNCTION_GET_STATUS_FROM_ENDPOINT = 0x0015,

        /// <summary>
        /// Reserve for code 0x0016
        /// </summary>
        URB_FUNCTION_RESERVED_0X0016 = 0x0016,

        /// <summary>
        /// Sends a vendor-specific command to a USB device
        /// </summary>
        URB_FUNCTION_VENDOR_DEVICE = 0x0017,

        /// <summary>
        /// Sends a vendor-specific command for an interface on a USB device
        /// </summary>
        URB_FUNCTION_VENDOR_INTERFACE = 0x0018,

        /// <summary>
        /// Sends a vendor-specific command for an endpoint on an interface on a USB device
        /// </summary>
        URB_FUNCTION_VENDOR_ENDPOINT = 0x0019,

        /// <summary>
        /// Sends a USB-defined class-specific command to a USB device
        /// </summary>
        URB_FUNCTION_CLASS_DEVICE = 0x001A,

        /// <summary>
        /// Sends a USB-defined class-specific command to an interface on a USB device
        /// </summary>
        URB_FUNCTION_CLASS_INTERFACE = 0x001B,

        /// <summary>
        /// Sends a USB-defined class-specific command to an endpoint, on an interface, on a USB device
        /// </summary>
        URB_FUNCTION_CLASS_ENDPOINT = 0x001C,

        /// <summary>
        /// Reserve for code 0x001d
        /// </summary>
        URB_FUNCTION_RESERVE_0X001D = 0x001D,

        /// <summary>
        /// Resets the indicated pipe
        /// </summary>
        URB_FUNCTION_SYNC_RESET_PIPE_AND_CLEAR_STALL = 0x001E,

        /// <summary>
        /// Sends a USB-defined class-specific command to a device defined target on a USB device
        /// </summary>
        URB_FUNCTION_CLASS_OTHER = 0x001F,

        /// <summary>
        /// Sends a vendor-specific command to a device-defined target on a USB device
        /// </summary>
        URB_FUNCTION_VENDOR_OTHER = 0x0020,

        /// <summary>
        /// Retrieves status from a device-defined target on a USB device
        /// </summary>
        URB_FUNCTION_GET_STATUS_FROM_OTHER = 0x0021,

        /// <summary>
        /// Clears a USB-defined feature on a device defined target on a USB device
        /// </summary>
        URB_FUNCTION_CLEAR_FEATURE_TO_OTHER = 0x0022,

        /// <summary>
        /// Sets a USB-defined feature on a device-defined target on a USB device
        /// </summary>
        URB_FUNCTION_SET_FEATURE_TO_OTHER = 0x0023,

        /// <summary>
        /// Retrieves the descriptor from an endpoint on an interface for a USB device
        /// </summary>
        URB_FUNCTION_GET_DESCRIPTOR_FROM_ENDPOINT = 0x0024,

        /// <summary>
        /// Sets an endpoint descriptor on an endpoint for an interface
        /// </summary>
        URB_FUNCTION_SET_DESCRIPTOR_TO_ENDPOINT = 0x0025,

        /// <summary>
        /// Retrieves the current configuration on a USB device
        /// </summary>
        URB_FUNCTION_GET_CONFIGURATION = 0x0026,

        /// <summary>
        /// Retrieves the current settings for an interface on a USB device
        /// </summary>
        URB_FUNCTION_GET_INTERFACE = 0x0027,

        /// <summary>
        /// Retrieves the descriptor from an interface for a USB device
        /// </summary>
        URB_FUNCTION_GET_DESCRIPTOR_FROM_INTERFACE = 0x0028,

        /// <summary>
        /// Sets a descriptor for an interface on a USB device
        /// </summary>
        URB_FUNCTION_SET_DESCRIPTOR_TO_INTERFACE = 0x0029,

        /// <summary>
        /// Retrieves a Microsoft OS feature descriptor from a USB device or an interface on a USB device
        /// </summary>
        URB_FUNCTION_GET_MS_FEATURE_DESCRIPTOR = 0x002A,

        /// <summary>
        /// Reserve for code 0x002B
        /// </summary>
        URB_FUNCTION_RESERVE_0X002B = 0x002B,

        /// <summary>
        /// Reserve for code 0x002C
        /// </summary>
        URB_FUNCTION_RESERVE_0X002C = 0x002C,

        /// <summary>
        /// Reserve for code 0x002D
        /// </summary>
        URB_FUNCTION_RESERVE_0X002D = 0x002D,

        /// <summary>
        /// Reserve for code 0x002E
        /// </summary>
        URB_FUNCTION_RESERVE_0X002E = 0x002E,

        /// <summary>
        /// Reserve for code 0x002F
        /// </summary>
        URB_FUNCTION_RESERVE_0X002F = 0x002F,

        /// <summary>
        /// Clears the halt condition on the host side of a pipe
        /// </summary>
        URB_FUNCTION_SYNC_RESET_PIPE = 0x0030,

        /// <summary>
        /// Clears the stall condition on the endpoint
        /// </summary>
        URB_FUNCTION_SYNC_CLEAR_STALL = 0x0031,

        /// <summary>
        /// Transfers data to or from a control pipe extension
        /// </summary>
        URB_FUNCTION_CONTROL_TRANSFER_EX = 0x0032,

        /// <summary>
        /// Unknown type
        /// </summary>
        UNKNOWN = 0xFFFF
    }

    /// <summary>
    /// The USBD_STATUS data type defines USB status values for USB requests
    /// </summary>
    public enum USBD_STATUS : uint
    {
        /// <summary>
        /// It verify that the Request was completed with success.
        /// </summary>
        USBD_STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// It verify that the Request is pending.
        /// </summary>
        USBD_STATUS_PENDING = 0x40000000,

        /// <summary>
        /// One of the values indicates the Request was completed with an error.
        /// </summary>
        USBD_STATUS_ERROR_10 = 10,

        /// <summary>
        /// One of the values indicates the Request was completed with an error.
        /// </summary>
        USBD_STATUS_ERROR_11 = 11,

        /// <summary>
        /// CRC error (defined for backward compatibility with the USB 1.0).
        /// </summary>
        USBD_STATUS_CRC = 0xC0000001,

        /// <summary>
        /// BTS error (defined for backward compatibility with the USB 1.0).
        /// </summary>
        USBD_STATUS_BTSTUFF = 0xC0000002,

        /// <summary>
        /// The device returned a stall packet identifier (defined for backward compatibility with the USB 1.0).
        /// </summary>
        USBD_STATUS_STALL_PID = 0xC0000004,

        /// <summary>
        /// The device is not responding (defined for backward compatibility with the USB 1.0).
        /// </summary>
        USBD_STATUS_DEV_NOT_RESPONDING = 0xC0000005,

        /// <summary>
        /// The device returned a packet identifier check failure (defined for backward compatibility with the USB 1.0).
        /// </summary>
        USBD_STATUS_PID_CHECK_FAILURE = 0xC0000006,

        /// <summary>
        /// The device returned an unexpected packet identifier error (defined for backward compatibility with the USB 1.0).
        /// </summary>
        USBD_STATUS_UNEXPECTED_PID = 0xC0000007,

        /// <summary>
        /// The device returned a data overrun error (defined for backward compatibility with the USB 1.0).
        /// </summary>
        USBD_STATUS_DATA_OVERRUN = 0xC0000008,

        /// <summary>
        /// The device returned a data underrun error (defined for backward compatibility with the USB 1.0).
        /// </summary>
        USBD_STATUS_DATA_UNDERRUN = 0xC0000009,

        /// <summary>
        /// The value is Reserved,it has not been used.
        /// </summary>
        USBD_STATUS_RESERVED1 = 0xC000000A,

        /// <summary>
        /// The value is Reserved,it has not been used.
        /// </summary>
        USBD_STATUS_RESERVED2 = 0xC000000B,

        /// <summary>
        /// The device returned a buffer overrun error (defined for backward compatibility with the USB 1.0).
        /// </summary>
        USBD_STATUS_BUFFER_OVERRUN = 0xC000000C,

        /// <summary>
        /// The device returned a buffer underrun error (defined for backward compatibility with the USB 1.0).
        /// </summary>
        USBD_STATUS_BUFFER_UNDERRUN = 0xC000000D,

        /// <summary>
        /// The USB stack could not access the device (defined for backward compatibility with the USB 1.0).
        /// </summary>
        USBD_STATUS_NOT_ACCESSED = 0xC000000F,

        /// <summary>
        /// The device returned a FIFO error (defined for backward compatibility with the USB 1.0).
        /// </summary>
        USBD_STATUS_FIFO = 0xC0000010,

        /// <summary>
        /// The device returned a transaction error (defined for backward compatibility with the USB 1.0).
        /// </summary>
        USBD_STATUS_XACT_ERROR = 0xC0000011,

        /// <summary>
        /// The device returned a babble detected error (defined for backward compatibility with the USB 1.0).
        /// </summary>
        USBD_STATUS_BABBLE_DETECTED = 0xC0000012,

        /// <summary>
        /// Hardware status codes that range from 0x00000001 to 0x000000FF (defined for backward compatibility with the USB 1.0 stack).
        /// </summary>
        USBD_STATUS_DATA_BUFFER_ERROR = 0xC0000013,

        /// <summary>
        /// A transfer was submitted to an endpoint that is stalled.
        /// </summary>
        USBD_STATUS_ENDPOINT_HALTED = 0xC0000030,

        /// <summary>
        /// It indicates an Invalid URB function.
        /// </summary>
        USBD_STATUS_INVALID_URB_FUNCTION = 0x80000200,

        /// <summary>
        /// It indicates an Invalid parameter.
        /// </summary>
        USBD_STATUS_INVALID_PARAMETER = 0x80000300,

        /// <summary>
        /// The client driver caused an error by attempting to close an endpoint, interface, or configuration handle with outstanding transfers.
        /// </summary>
        USBD_STATUS_ERROR_BUSY = 0x80000400,

        /// <summary>
        /// It indicates an Invalid pipe handle.
        /// </summary>
        USBD_STATUS_INVALID_PIPE_HANDLE = 0x80000600,

        /// <summary>
        /// There was not enough bandwidth to open a requested endpoint.
        /// </summary>
        USBD_STATUS_NO_BANDWIDTH = 0x80000700,

        /// <summary>
        /// Unspecified host controller error.
        /// </summary>
        USBD_STATUS_INTERNAL_HC_ERROR = 0x80000800,

        /// <summary>
        /// The transfer ended with a short packet, but the USBD_SHORT_TRANSFER_OK bit is not set for the pipe.
        /// </summary>
        USBD_STATUS_ERROR_SHORT_TRANSFER = 0x80000900,

        /// <summary>
        /// The requested start frame is not within a range of USBD_ISO_START_FRAME_RANGE frames of the current USB frame. Whenever this error occurs, the system sets the stall bit on the pipe.
        /// </summary>
        USBD_STATUS_BAD_START_FRAME = 0xC0000A00,

        /// <summary>
        /// The host controller returns this error whenever all packets in an isochronous transfer complete with an error.
        /// </summary>
        USBD_STATUS_ISOCH_REQUEST_FAILED = 0xC0000B00,

        /// <summary>
        /// The hub driver returns this error whenever the frame length control for the host controller is being used by a driver other than the host controller driver.
        /// </summary>
        USBD_STATUS_FRAME_CONTROL_OWNED = 0xC0000C00,

        /// <summary>
        /// The hub driver returns this error if the caller does not own frame length control and attempts to release or modify the host controller frame length.
        /// </summary>
        USBD_STATUS_FRAME_CONTROL_NOT_OWNED = 0xC0000D00,

        /// <summary>
        /// The request was not supported.
        /// </summary>
        USBD_STATUS_NOT_SUPPORTED = 0xC0000E00,

        /// <summary>
        /// Invalid configuration descriptor.
        /// </summary>
        USBD_STATUS_INAVLID_CONFIGURATION_DESCRIPTOR = 0xC0000F00,

        /// <summary>
        /// It verifies that it is Insufficient resources.
        /// </summary>
        USBD_STATUS_INSUFFICIENT_RESOURCES = 0xC0001000,

        /// <summary>
        /// An attempt to change the device configuration failed.
        /// </summary>
        USBD_STATUS_SET_CONFIG_FAILED = 0xC0002000,

        /// <summary>
        /// The buffer is too small.
        /// </summary>
        USBD_STATUS_BUFFER_TOO_SMALL = 0xC0003000,

        /// <summary>
        /// The interface was not found.
        /// </summary>
        USBD_STATUS_INTERFACE_NOT_FOUND = 0xC0004000,

        /// <summary>
        /// The interface was not found.
        /// </summary>
        USBD_STATUS_INAVLID_PIPE_FLAGS = 0xC0005000,

        /// <summary>
        /// The request timed out.
        /// </summary>
        USBD_STATUS_TIMEOUT = 0xC0006000,

        /// <summary>
        /// The device is no longer present in the system.
        /// </summary>
        USBD_STATUS_DEVICE_GONE = 0xC0007000,

        /// <summary>
        /// The device bus address is not mapped to system memory.
        /// </summary>
        USBD_STATUS_STATUS_NOT_MAPPED = 0xC0008000,

        /// <summary>
        /// The USB stack reports this error whenever it completed a transfer because of an AbortPipe request from the client driver.
        /// </summary>
        USBD_STATUS_CANCELED = 0xC0010000,

        /// <summary>
        /// The host controller did not access the transfer descriptor (TD) that is associated with this packet. The USB stack reports this error in the packet status field of an isochronous transfer packet.
        /// </summary>
        USBD_STATUS_ISO_NOT_ACCESSED_BY_HW = 0xC0020000,

        /// <summary>
        /// The host controller reported an error in the transfer descriptor (TD). The USB stack reports this error in the packet status field of an isochronous transfer packet.
        /// </summary>
        USBD_STATUS_ISO_TD_ERROR = 0xC0030000,

        /// <summary>
        /// The client driver submitted the packet on time, but the packet failed to reach the miniport driver on time. The USB stack reports this error in the packet status field of an isochronous transfer packet.
        /// </summary>
        USBD_STATUS_ISO_NA_LATE_USBPORT = 0xC0040000,

        /// <summary>
        /// The client driver did not submit the packet on time. The USB stack reports this error in the packet status field of an isochronous transfer packet.
        /// </summary>
        USBD_STATUS_ISO_NOT_ACCESSED_LATE = 0xC0050000,
    }

    /// <summary>
    /// The IO_CONTROL messages are sent in response to IOCTL requests that it receives from the system. 
    /// This section describes the IOCTL codes that the server supports.
    /// </summary>
    public enum UsbIoControlCode : uint
    {
        /// <summary>
        /// The IoControlCode indicates to reset the usb port
        /// </summary>
        IOCTL_INTERNAL_USB_RESET_PORT = 0x00220007,

        /// <summary>
        /// The IoControlCode indicates to get the port status
        /// </summary>
        IOCTL_INTERNAL_USB_GET_PORT_STATUS = 0x00220013,

        /// <summary>
        /// The IoControlCode indicates to get the count of the hub
        /// </summary>
        IOCTL_INTERNAL_USB_GET_HUB_COUNT = 0x0022001B,

        /// <summary>
        /// The IoControlCode indicates to cycle the usb port
        /// </summary>
        IOCTL_INTERNAL_USB_CYCLE_PORT = 0x0022001F,

        /// <summary>
        /// The IoControlCode indicates to get the name of the hub
        /// </summary>
        IOCTL_INTERNAL_USB_GET_HUB_NAME = 0x00220020,

        /// <summary>
        /// The IoControlCode indicates to get the information of the bus
        /// </summary>
        IOCTL_INTERNAL_USB_GET_BUS_INFO = 0x00220420,

        /// <summary>
        /// The IoControlCode indicates to get the name of the controller
        /// </summary>
        IOCTL_INTERNAL_USB_GET_CONTROLLER_NAME = 0x00220424
    }

    /// <summary>
    /// The Internal IO Control code
    /// </summary>
    public enum UsbInternalIoControlCode : uint
    {
        /// <summary>
        /// The Internal IoControlCode indicates query the device's current frame number and Microframe Generation is received.
        /// </summary>
        IOCTL_TSUSBGD_IOCTL_USBDI_QUERY_BUS_TIME = 0x00224000
    }

    /// <summary>
    /// The highest USBDI's version the device supports
    /// </summary>
    public enum USBDI_VER : uint
    {
        /// <summary>
        /// Version 5
        /// </summary>
        USBDI_VERSION_5 = 0x00000500,

        /// <summary>
        /// Version 6
        /// </summary>
        USBDI_VERSION_6 = 0x00000600
    }

    #endregion
 
    #region USB_DEVICE_CAPABILITIES
    /// <summary>
    /// The USB_DEVICE_CAPABILITIES structure defines the capabilities of a USB device.
    /// </summary>
    public class USB_DEVICE_CAPABILITIES : BasePDU
    {
        /// <summary>
        /// The byte size of the USB_DEVICE_CAPABILITIES structure.
        /// </summary>
        public static readonly int USB_DEVICE_CAPABILITIES_SIZE = 28;

        /// <summary>
        /// The byte size of this structure.
        /// </summary>
        public uint CbSize;

        /// <summary>
        /// The USB version the device supports.
        /// </summary>
        public uint UsbBusInterfaceVersion;

        /// <summary>
        /// The highest USBDI version the device supports. This value can be 0x00000500 or 0x00000600.
        /// </summary>
        public uint USBDI_Version;

        /// <summary>
        /// The version of USB the device supports.
        /// </summary>
        public uint Supported_USB_Version;

        /// <summary>
        /// The host capabilities supported. This value MUST always be zero.
        /// </summary>
        public uint HcdCapabilities = 0;

        /// <summary>
        /// This value represents the device speed. 0x00000000 if the device is full speed and 0x00000001
        /// if the device is high speed. If UsbBusInterfaceVersion is 0x00000000, DeviceIsHighSpeed MUST
        /// be 0x00000000. A high speed device operates as a USB 2.0 device while a full speed device 
        /// operates as a USB 1.1 device.
        /// </summary>
        public uint DeviceIsHighSpeed;

        /// <summary>
        /// If the value is nonzero, the client supports TS_URB_ISOCH_TRANSFER messages that do not expect
        /// URB_COMPLETION messages; otherwise, if the value is zero, the client does not support 
        /// TS_URB_ISOCH_TRANSFER messages. If the value is not zero, the value represents the amount of 
        /// outstanding isochronous data the client expects from the server. If this value is nonzero, it 
        /// MUST be greater than or equal to 10 and less than or equal to 512.
        /// </summary>
        public uint NoAckIsochWriteJitterBufferSizeInMs;

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteUInt32(CbSize);
            marshaler.WriteUInt32(UsbBusInterfaceVersion);
            marshaler.WriteUInt32(USBDI_Version);
            marshaler.WriteUInt32(Supported_USB_Version);
            marshaler.WriteUInt32(HcdCapabilities);
            marshaler.WriteUInt32(DeviceIsHighSpeed);
            marshaler.WriteUInt32(NoAckIsochWriteJitterBufferSizeInMs);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                CbSize = marshaler.ReadUInt32();
                UsbBusInterfaceVersion = marshaler.ReadUInt32();
                USBDI_Version = marshaler.ReadUInt32();
                Supported_USB_Version = marshaler.ReadUInt32();
                HcdCapabilities = marshaler.ReadUInt32();
                DeviceIsHighSpeed = marshaler.ReadUInt32();
                NoAckIsochWriteJitterBufferSizeInMs = marshaler.ReadUInt32();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Convert the fields of this PDU to a string.
        /// </summary>
        /// <returns>A string contains the fields of this PDU.</returns>
        public override string ToString()
        {
            string s = string.Format("CbSize: 0x{0:x8}\r\n", CbSize);
            s += string.Format("UsbBusInterfaceVersion: 0x{0:x8}\r\n", UsbBusInterfaceVersion);
            s += string.Format("USBDI_Version: 0x{0:x8}\r\n", USBDI_Version);
            s += string.Format("Supported_USB_Version: 0x{0:x8}\r\n", Supported_USB_Version);
            s += string.Format("HcdCapabilities: 0x{0:x8}\r\n", HcdCapabilities);
            s += string.Format("DeviceIsHighSpeed: 0x{0:x8}\r\n", DeviceIsHighSpeed);
            s += string.Format("NoAckIsochWriteJitterBufferSizeInMs: 0x{0:x8}\r\n", NoAckIsochWriteJitterBufferSizeInMs);

            return s;
        }
    }
    #endregion

}
