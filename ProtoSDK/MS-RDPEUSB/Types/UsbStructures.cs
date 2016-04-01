// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Microsoft.Protocols.TestTools.StackSdk;
using System.Text;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeusb
{
    #region Base Type

    /// <summary>
    /// The abstracted base class for all struct types.
    /// </summary>
    public abstract class UsbStructure : BasePDU
    {
    }

    /// <summary>
    /// The base type of the all descriptor structures.
    /// </summary>
    public class UsbDescriptorStructure : UsbStructure
    {
        /// <summary>
        /// Specifies the length, in bytes, of this descriptor.
        /// </summary>
        public byte bLength { get; set; }

        /// <summary>
        /// Specifies the descriptor type.
        /// </summary>
        public UsbDescriptorTypes bDescriptorType { get; set; }

        /// <summary>
        /// Unmarshal and validate the descriptor type from the marshaler buffer.
        /// </summary>
        /// <param name="marshaler">The marshaler containing raw data.</param>
        /// <returns>true indicates that the field is decoded successfully and valid.</returns>
        protected bool ReadDescriptorType(PduMarshaler marshaler)
        {
            bDescriptorType = UsbDescriptorTypes.UNKNOWN;
            byte b = marshaler.ReadByte();
            // TODO: don't hard-code.
            if (b > 8 || b < 1)
            {
                return false;
            }

            bDescriptorType = (UsbDescriptorTypes)b;
            return true;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteByte(bLength);
            marshaler.WriteByte((byte)bDescriptorType);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            bLength = marshaler.ReadByte();
            if (!ReadDescriptorType(marshaler))
            {
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// USB Descriptor Types
    /// </summary>
    public enum UsbDescriptorTypes : byte
	{
        UNKNOWN = 0,
        USB_DEVICE_DESCRIPTOR_TYPE = 1,
        USB_CONFIGURATION_DESCRIPTOR_TYPE = 2,
        USB_STRING_DESCRIPTOR_TYPE = 3,
        USB_INTERFACE_DESCRIPTOR_TYPE = 4,
        USB_ENDPOINT_DESCRIPTOR_TYPE = 5,
        USB_DEVICE_QUALIFIER_DESCRIPTOR_TYPE = 6,
        USB_OTHER_SPEED_CONFIG_DESCRIPTOR_TYPE = 7,
        USB_INTERFACE_POWER_DESCRIPTOR_TYPE = 8
	}

    #endregion

    #region URB_CONTROL_DESCRIPTOR_REQUEST structures

    /// <summary>
    /// The USB_STRING_DESCRIPTOR structure is used by USB client drivers to hold a USB-defined string descriptor.
    /// </summary>
    public class USB_STRING_DESCRIPTOR : UsbDescriptorStructure
    {
        /// <summary>
        /// Minimum default size of the structure in bytes.
        /// </summary>
        public const byte DefaultSize = 4;
        
        /// <summary>
        /// Pointer to a client-allocated buffer that contains, on return from the host controller driver, 
        /// a Unicode string with the requested string descriptor.
        /// </summary>
        public string bString { get; set; }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUnicodeString(bString);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            base.Decode(marshaler);

            // TODO: Need to refine marshaler interfaces.
            if (marshaler.RawData.Length > 2)
            {
                bString = marshaler.ReadUnicodeString();
            }
            return true;
        }

        /// <summary>
        /// Convert the fields of this PDU to a string.
        /// </summary>
        /// <returns>A string contains the fields of this PDU.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("bLength: 0x{0:x2}\r\n", bLength);
            sb.AppendFormat("bDescriptorType: {0}\r\n", bDescriptorType);
            sb.AppendFormat("bString: {0}\r\n", bString);

            return sb.ToString();
        }
    }

    /// <summary>
    /// The USB_DEVICE_DESCRIPTOR structure is used by USB client drivers to retrieve a USB-defined device descriptor.
    /// </summary>
    public class USB_DEVICE_DESCRIPTOR : UsbDescriptorStructure
    {
        /// <summary>
        /// Minimum default size of the structure in bytes.
        /// </summary>
        public const byte DefaultSize = 18;

        /// <summary>
        /// Identifies the version of the USB specification that this descriptor structure 
        /// complies with. This value is a binary-coded decimal number.
        /// </summary>
        public ushort bcdUSB { get; set; }

        /// <summary>
        /// Specifies the class code of the device as assigned by the USB specification group.
        /// </summary>
        public byte bDeviceClass { get; set; }

        /// <summary>
        /// Specifies the subclass code of the device as assigned by the USB specification group.
        /// </summary>
        public byte bDeviceSubClass { get; set; }

        /// <summary>
        /// Specifies the protocol code of the device as assigned by the USB specification group.
        /// </summary>
        public byte bDeviceProtocol { get; set; }

        /// <summary>
        /// Specifies the maximum packet size, in bytes, for endpoint zero of the device. 
        /// The value must be set to 8, 16, 32, or 64.
        /// </summary>
        public byte bMaxPacketSize0 { get; set; }

        /// <summary>
        /// Specifies the vendor identifier for the device as assigned by the USB specification committee.
        /// </summary>
        public ushort idVendor { get; set; }

        /// <summary>
        /// Specifies the product identifier. This value is assigned by the manufacturer and is device-specific.
        /// </summary>
        public ushort idProduct { get; set; }

        /// <summary>
        /// Identifies the version of the device. This value is a binary-coded decimal number.
        /// </summary>
        public ushort bcdDevice { get; set; }

        /// <summary>
        /// Specifies a device-defined index of the string descriptor that provides a string containing the name 
        /// of the manufacturer of this device.
        /// </summary>
        public byte iManufacturer { get; set; }

        /// <summary>
        /// Specifies a device-defined index of the string descriptor that provides a string that contains a 
        /// description of the device.
        /// </summary>
        public byte iProduct { get; set; }

        /// <summary>
        /// Specifies a device-defined index of the string descriptor that provides a string that contains a 
        /// manufacturer-determined serial number for the device.
        /// </summary>
        public byte iSerialNumber { get; set; }

        /// <summary>
        /// Specifies the total number of possible configurations for the device.
        /// </summary>
        public byte bNumConfigurations { get; set; }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            base.Decode(marshaler);
            bcdUSB = marshaler.ReadUInt16();
            bDeviceClass = marshaler.ReadByte();
            bDeviceSubClass = marshaler.ReadByte();
            bDeviceProtocol = marshaler.ReadByte();
            bMaxPacketSize0 = marshaler.ReadByte();
            idVendor = marshaler.ReadUInt16();
            idProduct = marshaler.ReadUInt16();
            bcdDevice = marshaler.ReadUInt16();
            iManufacturer = marshaler.ReadByte();
            iProduct = marshaler.ReadByte();
            iSerialNumber = marshaler.ReadByte();
            bNumConfigurations = marshaler.ReadByte();
            return true;
        }

        /// <summary>
        /// Convert the fields of this PDU to a string.
        /// </summary>
        /// <returns>A string contains the fields of this PDU.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("bLength: 0x{0:x2}\r\n", bLength);
            sb.AppendFormat("bDescriptorType: {0}\r\n", bDescriptorType);
            sb.AppendFormat("bcdUSB: 0x{0:x4}\r\n", bcdUSB);
            sb.AppendFormat("bDeviceClass: 0x{0:x2}\r\n", bDeviceClass);
            sb.AppendFormat("bDeviceSubClass: 0x{0:x2}\r\n", bDeviceSubClass);
            sb.AppendFormat("bDeviceProtocol: 0x{0:x2}\r\n", bDeviceProtocol);
            sb.AppendFormat("bMaxPacketSize0: 0x{0:x2}\r\n", bMaxPacketSize0);
            sb.AppendFormat("idVendor: 0x{0:x4}\r\n", idVendor);
            sb.AppendFormat("idProduct: 0x{0:x4}\r\n", idProduct);
            sb.AppendFormat("bcdDevice: 0x{0:x4}\r\n", bcdDevice);
            sb.AppendFormat("iManufacturer: 0x{0:x2}\r\n", iManufacturer);
            sb.AppendFormat("iProduct: 0x{0:x2}\r\n", iProduct);
            sb.AppendFormat("iSerialNumber: 0x{0:x2}\r\n", iSerialNumber);
            sb.AppendFormat("bNumConfigurations: 0x{0:x2}\r\n", bNumConfigurations);

            return sb.ToString();
        }
    }

    /// <summary>
    /// The USB_CONFIGURATION_DESCRIPTOR structure is used by USB client drivers to hold a USB-defined configuration descriptor.
    /// </summary>
    public class USB_CONFIGURATION_DESCRIPTOR : UsbDescriptorStructure
    {
        /// <summary>
        /// Minimum default size of the structure in bytes.
        /// </summary>
        public const byte DefaultSize = 9;

        /// <summary>
        /// Specifies the total length, in bytes, of all data for the configuration. The length includes all 
        /// interface, endpoint, class, or vendor-specific descriptors that are returned with the configuration 
        /// descriptor.
        /// </summary>
        public ushort wTotalLength { get; set; }

        /// <summary>
        /// Specifies the total number of interfaces supported by this configuration.
        /// </summary>
        public byte bNumInterfaces { get; set; }

        /// <summary>
        /// Contains the value that is used to select a configuration. This value is passed to the USB 
        /// SetConfiguration request , as described in version 1.1 of the Universal Serial Bus Specification. 
        /// The port driver does not currently expose a service that allows higher-level drivers to set 
        /// the configuration. 
        /// </summary>
        public byte bConfigurationValue { get; set; }

        /// <summary>
        /// Specifies the device-defined index of the string descriptor for this configuration.
        /// </summary>
        public byte iConfiguration { get; set; }

        /// <summary>
        /// Specifies a bitmap to describe behavior of this configuration. The bits are described and 
        /// set in little-endian order.
        /// </summary>
        public byte bmAttributes { get; set; }

        /// <summary>
        /// Specifies the power requirements of this device in two-milliampere units. This member is valid 
        /// only if bit seven is set in bmAttributes.
        /// </summary>
        public byte MaxPower { get; set; }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt16(wTotalLength);
            marshaler.WriteByte(bNumInterfaces);
            marshaler.WriteByte(bConfigurationValue);
            marshaler.WriteByte(iConfiguration);
            marshaler.WriteByte(bmAttributes);
            marshaler.WriteByte(MaxPower);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            base.Decode(marshaler);
            wTotalLength = marshaler.ReadUInt16();
            bNumInterfaces = marshaler.ReadByte();
            bConfigurationValue = marshaler.ReadByte();
            iConfiguration = marshaler.ReadByte();
            bmAttributes = marshaler.ReadByte();
            MaxPower = marshaler.ReadByte();
            return true;
        }

        /// <summary>
        /// Convert the fields of this PDU to a string.
        /// </summary>
        /// <returns>A string contains the fields of this PDU.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("bLength: 0x{0:x2}\r\n", bLength);
            sb.AppendFormat("bDescriptorType: {0}\r\n", bDescriptorType);
            sb.AppendFormat("wTotalLength: 0x{0:x4}\r\n", wTotalLength);
            sb.AppendFormat("bNumInterfaces: 0x{0:x2}\r\n", bNumInterfaces);
            sb.AppendFormat("bConfigurationValue: 0x{0:x2}\r\n", bConfigurationValue);
            sb.AppendFormat("iConfiguration: 0x{0:x2}\r\n", iConfiguration);
            sb.AppendFormat("bmAttributes: 0x{0:x2}\r\n", bmAttributes);
            sb.AppendFormat("MaxPower: 0x{0:x2}\r\n", MaxPower);

            return sb.ToString();
        }
    }

    /// <summary>
    /// The USB_INTERFACE_DESCRIPTOR structure is used by USB client drivers to retrieve a USB-defined interface descriptor.
    /// </summary>
    public class USB_INTERFACE_DESCRIPTOR : UsbDescriptorStructure
    {
        /// <summary>
        /// Minimum default size of the structure in bytes.
        /// </summary>
        public const byte DefaultSize = 9;

        /// <summary>
        /// The index number of the interface.
        /// </summary>
        public byte bInterfaceNumber { get; set; }

        /// <summary>
        /// The index number of the alternate setting of the interface.
        /// </summary>
        public byte bAlternateSetting { get; set; }

        /// <summary>
        /// The number of endpoints that are used by the interface, excluding the default status endpoint.
        /// </summary>
        public byte bNumEndpoints { get; set; }

        /// <summary>
        /// The class code of the device that the USB specification group assigned.
        /// </summary>
        public byte bInterfaceClass { get; set; }

        /// <summary>
        /// The subclass code of the device that the USB specification group assigned.
        /// </summary>
        public byte bInterfaceSubClass { get; set; }

        /// <summary>
        /// The protocol code of the device that the USB specification group assigned.
        /// </summary>
        public byte bInterfaceProtocol { get; set; }

        /// <summary>
        /// The index of a string descriptor that describes the interface. 
        /// </summary>
        public byte iInterface { get; set; }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteByte(bInterfaceNumber);
            marshaler.WriteByte(bAlternateSetting);
            marshaler.WriteByte(bNumEndpoints);
            marshaler.WriteByte(bInterfaceClass);
            marshaler.WriteByte(bInterfaceSubClass);
            marshaler.WriteByte(bInterfaceProtocol);
            marshaler.WriteByte(iInterface);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            base.Decode(marshaler);
            bInterfaceNumber = marshaler.ReadByte();
            bAlternateSetting = marshaler.ReadByte();
            bNumEndpoints = marshaler.ReadByte();
            bInterfaceClass = marshaler.ReadByte();
            bInterfaceSubClass = marshaler.ReadByte();
            bInterfaceProtocol = marshaler.ReadByte();
            iInterface = marshaler.ReadByte();
            return true;
        }

        /// <summary>
        /// Convert the fields of this PDU to a string.
        /// </summary>
        /// <returns>A string contains the fields of this PDU.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("bLength: 0x{0:x2}\r\n", bLength);
            sb.AppendFormat("bDescriptorType: {0}\r\n", bDescriptorType);
            sb.AppendFormat("bInterfaceNumber: 0x{0:x2}\r\n", bInterfaceNumber);
            sb.AppendFormat("bAlternateSetting: 0x{0:x2}\r\n", bAlternateSetting);
            sb.AppendFormat("bNumEndpoints: 0x{0:x2}\r\n", bNumEndpoints);
            sb.AppendFormat("bInterfaceClass: 0x{0:x2}\r\n", bInterfaceClass);
            sb.AppendFormat("bInterfaceSubClass: 0x{0:x2}\r\n", bInterfaceSubClass);
            sb.AppendFormat("bInterfaceProtocol: 0x{0:x2}\r\n", bInterfaceProtocol);
            sb.AppendFormat("iInterface: 0x{0:x2}\r\n", iInterface);

            return sb.ToString();
        }
    }

    /// <summary>
    /// The USB_ENDPOINT_DESCRIPTOR structure is used by USB client drivers to retrieve a USB-defined endpoint descriptor.
    /// </summary>
    public class USB_ENDPOINT_DESCRIPTOR  : UsbDescriptorStructure
    {
        /// <summary>
        /// Minimum default size of the structure in bytes.
        /// </summary>
        public const byte DefaultSize = 7;

        /// <summary>
        /// Specifies the USB-defined endpoint address. The four low-order bits specify the endpoint number. 
        /// The high-order bit specifies the direction of data flow on this endpoint: 1 for in, 0 for out.
        /// </summary>
        byte  bEndpointAddress { get; set; }

        /// <summary>
        /// The two low-order bits specify the endpoint type, one of USB_ENDPOINT_TYPE_CONTROL, USB_ENDPOINT_TYPE_ISOCHRONOUS, 
        /// USB_ENDPOINT_TYPE_BULK, or USB_ENDPOINT_TYPE_INTERRUPT.
        /// </summary>
        byte  bmAttributes { get; set; }

        /// <summary>
        /// Specifies the maximum packet size that can be sent from or to this endpoint.
        /// </summary>
        ushort wMaxPacketSize { get; set; }

        /// <summary>
        /// The bInterval value contains the polling interval for interrupt and isochronous endpoints. For other types of endpoint, this value should be ignored.
        /// </summary>
        byte  bInterval { get; set; }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteByte(bEndpointAddress);
            marshaler.WriteByte(bmAttributes);
            marshaler.WriteUInt16(wMaxPacketSize);
            marshaler.WriteByte(bInterval);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            base.Decode(marshaler);
            bEndpointAddress = marshaler.ReadByte();
            bmAttributes = marshaler.ReadByte();
            wMaxPacketSize = marshaler.ReadByte();
            bInterval = marshaler.ReadByte();
            return true;
        }

        /// <summary>
        /// Convert the fields of this PDU to a string.
        /// </summary>
        /// <returns>A string contains the fields of this PDU.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("bLength: 0x{0:x2}\r\n", bLength);
            sb.AppendFormat("bDescriptorType: {0}\r\n", bDescriptorType);
            sb.AppendFormat("bInterfaceNumber: 0x{0:x2}\r\n", bEndpointAddress);
            sb.AppendFormat("bAlternateSetting: 0x{0:x4}\r\n", wMaxPacketSize);
            sb.AppendFormat("bNumEndpoints: 0x{0:x2}\r\n", bInterval);

            return sb.ToString();
        }
    }

    #endregion

    #region Internal IO Control Result

    /// <summary>
    /// Type of notification 
    /// </summary>
    public enum USB_NOTIFICATION_TYPE
    {
        // the following return a USB_CONNECTION_NOTIFICATION structure
        EnumerationFailure = 0,
        InsufficentBandwidth,
        InsufficentPower,
        OverCurrent,
        ResetOvercurrent,

        // the following return a USB_BUS_NOTIFICATION structure
        AcquireBusInfo,

        // the following return a USB_ACQUIRE_INFO structure
        AcquireHubName,
        AcquireControllerName,

        // the following return a USB_HUB_NOTIFICATION structure
        HubOvercurrent,
        HubPowerChange,

        HubNestedTooDeeply,
        ModernDeviceInLegacyHub
    }

    /// <summary>
    /// The result type for IOCTL_INTERNAL_USB_GET_BUS_INFO
    /// </summary>
    public class USB_BUS_NOTIFICATION : UsbStructure
    {
                /// <summary>
        /// Minimum default size of the structure in bytes.
        /// </summary>
        public const byte DefaultSize = 16;

        /// <summary>
        /// Indicates type of notification.
        /// </summary>
        public USB_NOTIFICATION_TYPE NotificationType { get; set; }

        public uint TotalBandwidth { get; set; }
        public uint ConsumedBandwidth { get; set; }

        /// <summary>
        /// length of the UNICODE symbolic name (in bytes) for the controller
        /// that this device is attached to. Not including NULL */
        /// </summary>
        public uint ControllerNameLength { get; set; }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// This method was not implemented yet.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            return true;
        }

        /// <summary>
        /// Convert the fields of this PDU to a string.
        /// </summary>
        /// <returns>A string contains the fields of this PDU.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("NotificationType: {0}\r\n", NotificationType);
            sb.AppendFormat("TotalBandwidth: 0x{0:x8}\r\n", TotalBandwidth);
            sb.AppendFormat("ConsumedBandwidth: 0x{0:x8}\r\n", ConsumedBandwidth);
            sb.AppendFormat("ControllerNameLength: 0x{0:x8}\r\n", ControllerNameLength);

            return sb.ToString();
        }
    }

    [Flags]
    public enum TransferFlags : uint
    {
        USBD_TRANSFER_DIRECTION_OUT = 0,
        USBD_TRANSFER_DIRECTION_IN = 1,
        USBD_SHORT_TRANSFER_OK = 2               
    }

    public enum USBD_PIPE_TYPE : uint
    {
        UsbdPipeTypeControl       = 0,
        UsbdPipeTypeIsochronous   = 1,
        UsbdPipeTypeBulk          = 2,
        UsbdPipeTypeInterrupt     = 3 
    }

    /// <summary>
    /// Specifies whether the recipient is the USB device or an interface on the USB device.
    /// </summary>
    public enum Recipient : byte
    {
        /// <summary>
        /// 0 indicates that the USB device is the recipient of the request. 
        /// </summary>
        Device = 0,
        /// <summary>
        /// 1 indicates that a USB interface is the recipient of the request. 
        /// </summary>
        Interface = 1,
        /// <summary>
        /// 2 indicates that a USB endpoint is the recipient of the request. 
        /// </summary>
        Endpoint = 2
    }

    #endregion
}
