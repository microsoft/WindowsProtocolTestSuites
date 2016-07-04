// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeusb
{
    #region Basic PDUs
    /// <summary>
    /// This class is used as the base class of all MS-RDPEUSB PDUs.
    /// </summary>
    public class EusbPdu : BasePDU
    {
        /// <summary>
        /// The bitmask of InterfaceId.
        /// </summary>
        protected readonly uint InterfaceIdBitmask = 0x3FFFFFFF;

        /// <summary>
        /// The bitmask of Mask.
        /// </summary>
        protected readonly uint MaskBitmask = 0xC0000000;

        /// <summary>
        ///  A 30-bit field that represents the common identifier for the interface. The default value is 0.
        ///  If the message uses this default interface ID, the message is interpreted for the default interface
        ///  for which this channel has been instantiated. All other values MUST be retrieved either from a
        ///  Query Interface response (QI_RSP) ([MS-RDPEXPS] section 2.2.2.1.2) or from responses that contain
        ///  interface IDs. The highest two bits of NetInterfaceId field in QI_RSP MUST be ignored.
        /// </summary>
        public uint InterfaceId { get; set; } // TODO: change to EusbInterfaceId_Values
        /// <summary>
        ///  The 2 bits of the Mask field MUST be set to one of the following values.
        /// </summary>
        public Mask_Values Mask { get; set; }
        /// <summary>
        ///  A 32-bit unsigned integer. A unique ID for the request or response pair. Requests and responses
        ///  are matched based on this ID coupled with the InterfaceId.
        /// </summary>
        public uint MessageId { get; set; }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            uint id = (((uint)InterfaceId) & InterfaceIdBitmask) | ((((uint)Mask) << 30) & MaskBitmask);
            marshaler.WriteUInt32(id);
            marshaler.WriteUInt32(MessageId);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                uint id = marshaler.ReadUInt32();
                InterfaceId = id & InterfaceIdBitmask;
                Mask = (Mask_Values)((id & MaskBitmask) >> 30);
                MessageId = marshaler.ReadUInt32();
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
            string s = string.Format("InterfaceId: 0x{0:x8}\r\n", InterfaceId);
            s += string.Format("Mask: 0x{0:x2}\r\n", (ushort)Mask);
            s += string.Format("MessageId: 0x{0:x8}\r\n", MessageId);

            return s;
        }
    }

    public class EusbRequestPdu : EusbPdu
    {
        /// <summary>
        ///  A 32-bit unsigned integer. This field MUST be present in all packets except response packets.
        ///  Its value is either used in interface manipulation messages or defined for a specific interface.
        ///  The following values are categorized by the interface for which they are defined.
        /// </summary>
        public uint FunctionId { get; set; }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(FunctionId);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                FunctionId = marshaler.ReadUInt32();
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
            string s = string.Format("InterfaceId: 0x{0:x8}\r\n", InterfaceId);
            s += string.Format("Mask: 0x{0:x2}\r\n", (ushort)Mask);
            s += string.Format("MessageId: 0x{0:x8}\r\n", MessageId);
            s += string.Format("FunctionId: 0x{0:x8}\r\n", FunctionId);

            return s;
        }
    }
    #endregion

    #region Interface Manipulation Exchange Capabilities Interface

    /// <summary>
    /// This message is used by the server to request interface manipulation capabilities from the client.
    /// </summary>
    public class EusbRimExchangeCapRequestPdu : EusbRequestPdu
    {
        /// <summary>
        /// A 32-bit unsigned integer that identifies the server's capability.
        /// </summary>
        public uint CapabilityValue { get; set; }

        public EusbRimExchangeCapRequestPdu()
        {
            base.InterfaceId = 0x00000000;
            base.Mask = Mask_Values.STREAM_ID_NONE;
            base.FunctionId = (uint)FunctionId_Values.RIM_EXCHANGE_CAPABILITY_REQUEST;
            CapabilityValue = (uint)CapabilityValue_Values.RIM_CAPABILITY_VERSION_01;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(CapabilityValue);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                CapabilityValue = marshaler.ReadUInt32();
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
            string s = base.ToString();
            s += string.Format("CapabilityValue: 0x{0:x8}\r\n", CapabilityValue);

            return s;
        }
    }

    /// <summary>
    /// This message is sent by the client in response to RIM_EXCHANGE_CAPABILITY_REQUEST.
    /// </summary>
    public class EusbRimExchangeCapResponsePdu : EusbPdu
    {
        /// <summary>
        /// A 32-bit unsigned integer that identifies the server's capability.
        /// </summary>
        public uint CapabilityValue { get; set; }

        /// <summary>
        /// A 32-bit unsigned integer that indicates the HRESULT of the operation.
        /// </summary>
        public uint Result { get; set; }

        public EusbRimExchangeCapResponsePdu()
        {
            base.InterfaceId = 0x00000000;
            base.Mask = Mask_Values.STREAM_ID_NONE;
            CapabilityValue = (uint)CapabilityValue_Values.RIM_CAPABILITY_VERSION_01;
            Result = 0;
        }

        public EusbRimExchangeCapResponsePdu(uint interfaceId, uint messageId)
        {
            base.InterfaceId = interfaceId;
            base.Mask = Mask_Values.STREAM_ID_NONE;
            base.MessageId = messageId;
            CapabilityValue = (uint)CapabilityValue_Values.RIM_CAPABILITY_VERSION_01;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(CapabilityValue);
            marshaler.WriteUInt32(Result);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                CapabilityValue = marshaler.ReadUInt32();
                Result = marshaler.ReadUInt32();
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
            string s = base.ToString();
            s += string.Format("CapabilityValue: 0x{0:x8}\r\n", CapabilityValue);
            s += string.Format("Result: 0x{0:x8}\r\n", Result);

            return s;
        }
    }

    #endregion

    #region Device Sink Interface

    /// <summary>
    /// The ADD_VIRTUAL_CHANNEL message is sent from the client to the server to create a new instance
    /// of dynamic virtual channel.
    /// </summary>
    public class EusbAddVirtualChannelPdu : EusbRequestPdu
    {
        public EusbAddVirtualChannelPdu()
        {
            base.InterfaceId = 0x00000001;
            base.Mask = Mask_Values.STREAM_ID_PROXY;
            base.FunctionId = (uint)FunctionId_Values.ADD_VIRTUAL_CHANNEL;
        }
    }

    /// <summary>
    /// The ADD_DEVICE message is sent from the client to the server in order to create a redirected
    /// USB device on the server.
    /// </summary>
    public class EusbAddDevicePdu : EusbRequestPdu
    {
        /// <summary>
        /// A 32-bit unsigned integer. MUST be set to 0x00000001.
        /// </summary>
        public uint NumUsbDevice { get; set; }

        /// <summary>
        /// A 32-bit unsigned integer. A unique interface ID to be used by request messages defined 
        /// in USB device interface.
        /// </summary>
        public uint UsbDevice { get; set; }

        /// <summary>
        /// A 32-bit unsigned integer. This field MUST contain the number of Unicode characters in 
        /// the DeviceInstanceId field.
        /// </summary>
        public uint cchDeviceInstanceId { get; set; }

        /// <summary>
        /// An array of bytes. A variable-length field that contains a null-terminated Unicode string
        /// that identifies an instance of a USB device.
        /// </summary>
        public string DeviceInstanceId { get; set; }

        /// <summary>
        /// A 32-bit unsigned integer. This field MUST contain the number of Unicode characters in the
        /// HardwareIds field. This field MAY be 0x00000000.
        /// </summary>
        public uint cchHwIds { get; set; }

        /// <summary>
        /// An array of bytes. A variable-length field that specifies a multisz string representing the
        /// hardware IDs of the client-side device. If the value in the cchHwIds field is 0x00000000, 
        /// the HardwareIds buffer MUST NOT be present.
        /// </summary>
        public string HardwareIds { get; set; }

        /// <summary>
        /// 32-bit unsigned integer. This field MUST contain the number of Unicode characters in the 
        /// CompatibilityIds field.
        /// </summary>
        public uint cchCompatIds { get; set; }

        /// <summary>
        /// An array of bytes. A variable-length field that specifies a multisz string representing 
        /// the compatibility IDs of the client-side device. If the value in the cchCompatIds field 
        /// is 0x00000000, the CompatibilityIds buffer MUST NOT be present.
        /// </summary>
        public string CompatibilityIds { get; set; }

        /// <summary>
        /// A 32-bit unsigned integer. This field MUST contain the number of Unicode characters in 
        /// the ContainerId field.
        /// </summary>
        public uint cchContainerId { get; set; }

        /// <summary>
        /// An array of bytes. A variable-length field that contains a null-terminated Unicode string
        /// that contains the container ID in GUID, as specified in [MS-DTYP] section 2.3.2.2, format
        /// of the USB device.
        /// </summary>
        public string ContainerId { get; set; }

        /// <summary>
        /// A 28-byte structure as specified in section 2.2.11.
        /// </summary>
        public byte[] UsbDeviceCapabilities { get; set; }

        public EusbAddDevicePdu()
        {
            base.InterfaceId = 0x00000001;
            base.Mask = Mask_Values.STREAM_ID_PROXY;
            base.FunctionId = (uint)FunctionId_Values.ADD_DEVICE;
            NumUsbDevice = 0x00000001;
            cchHwIds = 0x00000000;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(NumUsbDevice);
            marshaler.WriteUInt32(UsbDevice);
            marshaler.WriteUInt32(cchDeviceInstanceId);
            marshaler.WriteUnicodeString(DeviceInstanceId);
            marshaler.WriteUInt32(cchHwIds);
            if (cchHwIds != 0)
            {
                marshaler.WriteUnicodeString(HardwareIds);
            }
            marshaler.WriteUInt32(cchCompatIds);
            if (cchCompatIds != 0)
            {
                marshaler.WriteUnicodeString(CompatibilityIds);
            }
            marshaler.WriteUInt32(cchContainerId);
            marshaler.WriteUnicodeString(ContainerId);
            marshaler.WriteBytes(UsbDeviceCapabilities);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                NumUsbDevice = marshaler.ReadUInt32();
                UsbDevice = marshaler.ReadUInt32();
                cchDeviceInstanceId = marshaler.ReadUInt32();
                DeviceInstanceId = marshaler.ReadUnicodeString((int)cchDeviceInstanceId);
                cchHwIds = marshaler.ReadUInt32();
                if (cchHwIds == 0)
                {
                    HardwareIds = null;
                }
                else
                {
                    HardwareIds = marshaler.ReadUnicodeString((int)cchHwIds);
                }
                cchCompatIds = marshaler.ReadUInt32();
                if (cchCompatIds == 0)
                {
                    CompatibilityIds = null;
                }
                else
                {
                    CompatibilityIds = marshaler.ReadUnicodeString((int)cchCompatIds);
                }
                cchContainerId = marshaler.ReadUInt32();
                ContainerId = marshaler.ReadUnicodeString((int)cchContainerId);
                UsbDeviceCapabilities = marshaler.ReadBytes(USB_DEVICE_CAPABILITIES.USB_DEVICE_CAPABILITIES_SIZE);
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
            string s = base.ToString();
            s += string.Format("NumUsbDevice: 0x{0:x8}\r\n", NumUsbDevice);
            s += string.Format("UsbDevice: 0x{0:x8}\r\n", UsbDevice);
            s += string.Format("cchDeviceInstanceId: 0x{0:x8}\r\n", cchDeviceInstanceId);
            s += string.Format("DeviceInstanceId: {0}\r\n", DeviceInstanceId);
            s += string.Format("cchHwIds: 0x{0:x8}\r\n", cchHwIds);
            if (cchHwIds != 0)
            {
                s += string.Format("HardwareIds: {0}\r\n", ConvertMutiszString(HardwareIds));
            }
            s += string.Format("cchCompatIds: 0x{0:x8}\r\n", cchCompatIds);
            if (cchCompatIds != 0)
            {
                s += string.Format("CompatibilityIds: {0}\r\n", ConvertMutiszString(CompatibilityIds));
            }
            s += string.Format("cchContainerId: 0x{0:x8}\r\n", cchContainerId);
            s += string.Format("ContainerId: {0}\r\n", ContainerId);
            s += string.Format("UsbDeviceCapabilities: {0}\r\n", BitConverter.ToString(UsbDeviceCapabilities));

            return s;
        }

        /// <summary>
        /// Convert mutisz string to the normal string which can be written to the XML file correctly.
        /// </summary>
        /// <param name="mutiszStr">null-terminated Unicode string composed of other null-terminated strings appended together.
        /// For example, a multisz string that contains "one", "brown", and "cow" would be represented as three null-terminated
        /// strings "one\0", "brown\0", "cow\0" appended together with an additional null appended, as follows: 
        /// "one\0brown\0cow\0\0".</param>
        /// <returns>Return a normal string if mutiszStr is not null, otherwise return null.</returns>
        private string ConvertMutiszString(string mutiszStr)
        {
            if (mutiszStr == null) return null;

            StringBuilder str = new StringBuilder();
            char ch;

            for (int i = 0; i < mutiszStr.Length; i++)
            {
                ch = mutiszStr[i];
                // remove any characters outside the valid UTF-8 range as well as all control characters 
                // except tabs and new lines 
                if ((ch < 0x00FD && ch > 0x001F) || ch == '\t' || ch == '\n' || ch == '\r')
                {
                    str.Append(ch);
                }
                else
                {
                    str.Append(" ");
                }
            }
            return str.ToString();
        } 
    }

    #endregion

    #region Channel Notification Interface
    /// <summary>
    /// The CHANNEL_CREATED message is sent from both the client and the server to inform the
    /// other side of the RDP USB device redirection version supported.
    /// </summary>
    public class EusbChannelCreatedPdu : EusbRequestPdu
    {
        /// <summary>
        /// The major version of RDP USB redirection supported. This value MUST be set to one.
        /// </summary>
        public uint MajorVersion { get; set; }

        /// <summary>
        /// The minor version of RDP USB redirection supported. This value MUST be set to zero.
        /// </summary>
        public uint MinorVersion { get; set; }

        /// <summary>
        /// The capabilities of RDP USB redirection supported. This value MUST be set to zero.
        /// </summary>
        public uint Capabilities { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="isServer">Indicates that this message is sent by server or by client.</param>
        public EusbChannelCreatedPdu(bool isServer)
        {
            base.InterfaceId = (uint)(isServer ? 0x00000002 : 0x000000003);
            base.Mask = Mask_Values.STREAM_ID_PROXY;
            base.FunctionId = (uint)FunctionId_Values.CHANNEL_CREATED;
            MajorVersion = 1;
            MinorVersion = 0;
            Capabilities = 0;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(MajorVersion);
            marshaler.WriteUInt32(MinorVersion);
            marshaler.WriteUInt32(Capabilities);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                MajorVersion = marshaler.ReadUInt32();
                MinorVersion = marshaler.ReadUInt32();
                Capabilities = marshaler.ReadUInt32();
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
            string s = base.ToString();
            s += string.Format("MajorVersion: 0x{0:x8}\r\n", MajorVersion);
            s += string.Format("MinorVersion: 0x{0:x8}\r\n", MinorVersion);
            s += string.Format("Capabilities: 0x{0:x8}\r\n", Capabilities);

            return s;
        }
    }
    #endregion

    #region USB Device Interface
    /// <summary>
    /// The CANCEL_REQUEST message is sent from the server to the client to cancel an outstanding IO request.
    /// </summary>
    public class EusbCancelRequestPdu : EusbRequestPdu
    {
        /// <summary>
        /// This value represents the ID of a request previously sent via IO_CONTROL, INTERNAL_IO_CONTROL,
        /// TRANSFER_IN_REQUEST, or TRANSFER_OUT_REQUEST message.
        /// </summary>
        public uint RequestId { get; set; }

        public EusbCancelRequestPdu()
        {
            base.Mask = Mask_Values.STREAM_ID_PROXY;
            base.FunctionId = (uint)FunctionId_Values.CANCEL_REQUEST;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="interfaceId">The InterfaceId field MUST match the value sent previously in the 
        /// UsbDevice field of the ADD_DEVICE message.</param>
        public EusbCancelRequestPdu(uint interfaceId)
        {
            base.InterfaceId = interfaceId;
            base.Mask = Mask_Values.STREAM_ID_PROXY;
            base.FunctionId = (uint)FunctionId_Values.CANCEL_REQUEST;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(RequestId);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                RequestId = marshaler.ReadUInt32();
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
            string s = base.ToString();
            s += string.Format("RequestId: 0x{0:x8}\r\n", RequestId);

            return s;
        }
    }

    /// <summary>
    /// The REGISTER_REQUEST_CALLBACK message is sent from the server to the client in order to provide
    /// a Request Completion Interface to the client.
    /// </summary>
    public class EusbRegisterRequestCallbackPdu : EusbRequestPdu
    {
        /// <summary>
        /// If this field is set to 0x00000001 or greater, then the RequestCompletion field is also present.
        /// If this field is set to 0x0000000, the RequestCompletion field is not present.
        /// </summary>
        public uint NumRequestCompletion { get; set; }

        /// <summary>
        /// A unique InterfaceID to be used by all Request Completion messages defined in the Request 
        /// Completion Interface (section 2.2.7).
        /// </summary>
        public uint RequestCompletion { get; set; }

        public EusbRegisterRequestCallbackPdu()
        {
            base.Mask = Mask_Values.STREAM_ID_PROXY;
            base.FunctionId = (uint)FunctionId_Values.REGISTER_REQUEST_CALLBACK;
        }

        public EusbRegisterRequestCallbackPdu(uint interfaceId, uint numRequestCompletion, uint requestCompletion)
        {
            base.InterfaceId = interfaceId;
            base.Mask = Mask_Values.STREAM_ID_PROXY;
            base.FunctionId = (uint)FunctionId_Values.REGISTER_REQUEST_CALLBACK;
            NumRequestCompletion = numRequestCompletion;
            RequestCompletion = requestCompletion;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(NumRequestCompletion);
            if (NumRequestCompletion != 0x00000000)
            {
                marshaler.WriteUInt32(RequestCompletion);
            }
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                NumRequestCompletion = marshaler.ReadUInt32();
                if (NumRequestCompletion != 0x00000000)
                {
                    RequestCompletion = marshaler.ReadUInt32();
                }
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
            string s = base.ToString();
            s += string.Format("NumRequestCompletion: 0x{0:x8}\r\n", NumRequestCompletion);
            if (NumRequestCompletion != 0x00000000)
            {
                s += string.Format("RequestCompletion: 0x{0:x8}\r\n", RequestCompletion);
            }

            return s;
        }
    }

    /// <summary>
    /// The IO_CONTROL message is sent from the server to the client in order to submit an IO control 
    /// request to the USB device.
    /// </summary>
    public class EusbIoControlPdu : EusbRequestPdu
    {
        /// <summary>
        /// An IO control code as specified in section 2.2.12.
        /// </summary>
        public UsbIoControlCode IoControlCode { get; set; }

        /// <summary>
        /// The size, in bytes, of the InputBuffer field.
        /// </summary>
        public uint InputBufferSize { get; set; }

        /// <summary>
        /// This value represents the input buffer for the IO control request.
        /// </summary>
        public byte[] InputBuffer { get; set; }

        /// <summary>
        /// The maximum number of bytes the client can return to the server.
        /// </summary>
        public uint OutputBufferSize { get; set; }

        /// <summary>
        /// This ID uniquely identifies the I/O control request.
        /// </summary>
        public uint RequestId { get; set; }

        public EusbIoControlPdu()
        {
            base.Mask = Mask_Values.STREAM_ID_PROXY;
            base.FunctionId = (uint)FunctionId_Values.IO_CONTROL;
        }

        public EusbIoControlPdu(uint interfaceId)
        {
            base.InterfaceId = interfaceId;
            base.Mask = Mask_Values.STREAM_ID_PROXY;
            base.FunctionId = (uint)FunctionId_Values.IO_CONTROL;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32((uint)IoControlCode);
            marshaler.WriteUInt32(InputBufferSize);
            if (InputBufferSize != 0)
            {
                marshaler.WriteBytes(InputBuffer);
            }
            marshaler.WriteUInt32(OutputBufferSize);
            marshaler.WriteUInt32(RequestId);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                IoControlCode = (UsbIoControlCode)marshaler.ReadUInt32();
                InputBufferSize = marshaler.ReadUInt32();
                InputBuffer = marshaler.ReadBytes((int)InputBufferSize);
                OutputBufferSize = marshaler.ReadUInt32();
                RequestId = marshaler.ReadUInt32();
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
            string s = base.ToString();
            s += string.Format("IoControlCode: 0x{0:x8}\r\n", (uint)IoControlCode);
            s += string.Format("InputBufferSize: {0}\r\n", InputBufferSize);
            if (InputBufferSize != 0)
            {
                s += string.Format("InputBuffer: {0}\r\n", BitConverter.ToString(InputBuffer));
            }
            s += string.Format("OutputBufferSize: {0}\r\n", OutputBufferSize);
            s += string.Format("RequestId: 0x{0:x8}\r\n", RequestId);

            return s;
        }
    }

    /// <summary>
    /// The INTERNAL_IO_CONTROL message is sent from the server to the client in order to submit an internal
    /// IO control request to the USB device.
    /// </summary>
    public class EusbInternalIoControlPdu : EusbRequestPdu
    {
        /// <summary>
        /// An internal IO control code as specified in section 2.2.12.
        /// </summary>
        public UsbInternalIoControlCode IoControlCode { get; set; }

        /// <summary>
        /// The size, in bytes, of the InputBuffer field.
        /// </summary>
        public uint InputBufferSize { get; set; }

        /// <summary>
        /// This value represents the input buffer for the internal IO control request.
        /// </summary>
        public byte[] InputBuffer { get; set; }

        /// <summary>
        /// The maximum number of bytes the internal IO control request can return.
        /// </summary>
        public uint OutputBufferSize { get; set; }

        /// <summary>
        /// This value represents an ID that uniquely identifies this internal IO control request.
        /// </summary>
        public uint RequestId { get; set; }

        public EusbInternalIoControlPdu()
        {
            base.Mask = Mask_Values.STREAM_ID_PROXY;
            base.FunctionId = (uint)FunctionId_Values.INTERNAL_IO_CONTROL;
        }

        public EusbInternalIoControlPdu(uint interfaceId)
        {
            base.InterfaceId = interfaceId;
            base.Mask = Mask_Values.STREAM_ID_PROXY;
            base.FunctionId = (uint)FunctionId_Values.INTERNAL_IO_CONTROL;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32((uint)IoControlCode);
            marshaler.WriteUInt32(InputBufferSize);
            if (InputBufferSize != 0)
            {
                marshaler.WriteBytes(InputBuffer);
            }
            marshaler.WriteUInt32(OutputBufferSize);
            marshaler.WriteUInt32(RequestId);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                IoControlCode = (UsbInternalIoControlCode)marshaler.ReadUInt32();
                InputBufferSize = marshaler.ReadUInt32();
                InputBuffer = marshaler.ReadBytes((int)InputBufferSize);
                OutputBufferSize = marshaler.ReadUInt32();
                RequestId = marshaler.ReadUInt32();
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
            string s = base.ToString();
            s += string.Format("IoControlCode: 0x{0:x8}\r\n", (uint)IoControlCode);
            s += string.Format("InputBufferSize: {0}\r\n", InputBufferSize);
            if (InputBufferSize != 0)
            {
                s += string.Format("InputBuffer: {0}\r\n", BitConverter.ToString(InputBuffer));
            }
            s += string.Format("OutputBufferSize: {0}\r\n", OutputBufferSize);
            s += string.Format("RequestId: 0x{0:x8}\r\n", RequestId);

            return s;
        }
    }

    /// <summary>
    /// The QUERY_DEVICE_TEXT message is sent from the server to the client in order to query the USB device's
    /// text when the server receives a query device test request (IRP_MN_QUERY_DEVICE_TEXT) from the system as
    /// described in [MSFT-W2KDDK], Volume 1, Part 1, Chapter 2.
    /// </summary>
    public class EusbQueryDeviceTextRequestPdu : EusbRequestPdu
    {
        /// <summary>
        /// This value represents the type of text to query as described in [MSFT-W2KDDK], Volume 1, Part 1, Chapter 2.
        /// </summary>
        public uint TextType { get; set; }

        /// <summary>
        /// This value represents the locale of the text to query as described in [MSFT-W2KDDK], Volume 1, Part 1, Chapter 2.
        /// </summary>
        public uint LocaleId { get; set; }

        public EusbQueryDeviceTextRequestPdu()
        {
            base.Mask = Mask_Values.STREAM_ID_PROXY;
            base.FunctionId = (uint)FunctionId_Values.QUERY_DEVICE_TEXT;
        }

        public EusbQueryDeviceTextRequestPdu(uint interfaceId)
        {
            base.InterfaceId = interfaceId;
            base.Mask = Mask_Values.STREAM_ID_PROXY;
            base.FunctionId = (uint)FunctionId_Values.QUERY_DEVICE_TEXT;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(TextType);
            marshaler.WriteUInt32(LocaleId);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                TextType = marshaler.ReadUInt32();
                LocaleId = marshaler.ReadUInt32();
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
            string s = base.ToString();
            s += string.Format("TextType: {0}\r\n", TextType);
            s += string.Format("LocaleId: {0}\r\n", LocaleId);

            return s;
        }
    }

    /// <summary>
    /// The QUERY_DEVICE_TEXT_RSP message is sent from the client in response to a QUERY_DEVICE_TEXT message sent by the server.
    /// </summary>
    public class EusbQueryDeviceTextResponsePdu : EusbPdu
    {
        /// <summary>
        /// This field MUST contain the number of Unicode characters in the DeviceDescription field.
        /// </summary>
        public uint cchDeviceDescription { get; set; }

        /// <summary>
        /// A variable-length field that contains a null-terminated Unicode string that contains the requested device text.
        /// </summary>
        public string DeviceDescription { get; set; }

        /// <summary>
        /// A 32-bit unsigned integer that indicates the HRESULT of the operation.
        /// </summary>
        public uint HResult { get; set; }

        public EusbQueryDeviceTextResponsePdu()
        {
            base.Mask = Mask_Values.STREAM_ID_STUB;
        }

        public EusbQueryDeviceTextResponsePdu(uint interfaceId, uint messageId)
        {
            base.InterfaceId = interfaceId;
            base.Mask = Mask_Values.STREAM_ID_STUB;
            base.MessageId = messageId;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(cchDeviceDescription);
            marshaler.WriteUnicodeString(DeviceDescription);
            marshaler.WriteUInt32(HResult);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                cchDeviceDescription = marshaler.ReadUInt32();
                DeviceDescription = marshaler.ReadUnicodeString((int)cchDeviceDescription);
                HResult = marshaler.ReadUInt32();
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
            string s = base.ToString();
            s += string.Format("cchDeviceDescription: {0}\r\n", cchDeviceDescription);
            s += string.Format("DeviceDescription: {0}\r\n", DeviceDescription);
            s += string.Format("HResult: 0x{0:x8}\r\n", HResult);

            return s;
        }
    }

    /// <summary>
    /// The TRANSFER_IN_REQUEST message is sent from the server to the client in order to request data from the USB device.
    /// </summary>
    public class EusbTransferInRequestPdu : EusbRequestPdu
    {
        /// <summary>
        /// The size, in bytes, of the TsUrb field.
        /// </summary>
        public uint CbTsUrb { get; set; }

        /// <summary>
        /// A TS_URB structure as defined in section 2.2.9.
        /// </summary>
        public byte[] TsUrb { get; set; }

        /// <summary>
        /// This value represents the maximum number of bytes of data that is requested from the USB device.
        /// </summary>
        public uint OutputBufferSize { get; set; }

        public EusbTransferInRequestPdu()
        {
            base.Mask = Mask_Values.STREAM_ID_PROXY;
            base.FunctionId = (uint)FunctionId_Values.TRANSFER_IN_REQUEST;
        }

        public EusbTransferInRequestPdu(uint interfaceId)
        {
            base.InterfaceId = interfaceId;
            base.Mask = Mask_Values.STREAM_ID_PROXY;
            base.FunctionId = (uint)FunctionId_Values.TRANSFER_IN_REQUEST;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(CbTsUrb);
            marshaler.WriteBytes(TsUrb);
            marshaler.WriteUInt32(OutputBufferSize);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                CbTsUrb = marshaler.ReadUInt32();
                TsUrb = marshaler.ReadBytes((int)CbTsUrb);
                OutputBufferSize = marshaler.ReadUInt32();
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
            string s = base.ToString();
            s += string.Format("CbTsUrb: {0}\r\n", CbTsUrb);
            s += string.Format("TsUrb: 0x {0}\r\n", BitConverter.ToString(TsUrb));
            s += string.Format("OutputBufferSize: {0}\r\n", OutputBufferSize);

            return s;
        }
    }

    /// <summary>
    /// The TRANSFER_OUT_REQUEST message is sent from the server to the client in order to submit data to the USB device.
    /// </summary>
    public class EusbTransferOutRequestPdu : EusbRequestPdu
    {
        /// <summary>
        /// The size, in bytes, of the TsUrb field.
        /// </summary>
        public uint CbTsUrb { get; set; }

        /// <summary>
        /// A TS_URB structure as defined in section 2.2.9.
        /// </summary>
        public byte[] TsUrb { get; set; }

        /// <summary>
        /// The size in bytes of the OutputBuffer field.
        /// </summary>
        public uint OutputBufferSize { get; set; }

        /// <summary>
        /// The raw data to be sent to the device.
        /// </summary>
        public byte[] OutputBuffer { get; set; }

        public EusbTransferOutRequestPdu()
        {
            base.Mask = Mask_Values.STREAM_ID_PROXY;
            base.FunctionId = (uint)FunctionId_Values.TRANSFER_OUT_REQUEST;
        }

        public EusbTransferOutRequestPdu(uint interfaceId)
        {
            base.InterfaceId = interfaceId;
            base.Mask = Mask_Values.STREAM_ID_PROXY;
            base.FunctionId = (uint)FunctionId_Values.TRANSFER_OUT_REQUEST;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(CbTsUrb);
            marshaler.WriteBytes(TsUrb);
            marshaler.WriteUInt32(OutputBufferSize);
            if (null != OutputBuffer && OutputBuffer.Length > 0)
            {
                marshaler.WriteBytes(OutputBuffer);
            }
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                CbTsUrb = marshaler.ReadUInt32();
                TsUrb = marshaler.ReadBytes((int)CbTsUrb);
                OutputBufferSize = marshaler.ReadUInt32();
                // TODO: should fix ReadBytes method in the library
                if (OutputBufferSize > 0)
                {
                    OutputBuffer = marshaler.ReadBytes((int)OutputBufferSize);
                }
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
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.AppendFormat("CbTsUrb: {0}\r\n", CbTsUrb);
            sb.AppendFormat("TsUrb: 0x {0}\r\n", BitConverter.ToString(TsUrb));
            sb.AppendFormat("OutputBufferSize: {0}\r\n", OutputBufferSize);
            if (null != OutputBuffer)
            {
                sb.AppendFormat("OutputBuffer: 0x {0}\r\n", BitConverter.ToString(OutputBuffer));
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// The RETRACT_DEVICE message is sent from the server to the client in order to stop redirecting the USB device.
    /// </summary>
    public class EusbRetractDevicePdu : EusbRequestPdu
    {
        /// <summary>
        /// The reason code, as specified in section 2.2.8, to stop redirecting the USB device.
        /// </summary>
        public USB_RETRACT_REASON Reason { get; set; }

        public EusbRetractDevicePdu()
        {
            base.Mask = Mask_Values.STREAM_ID_PROXY;
            base.FunctionId = (uint)FunctionId_Values.RETRACT_DEVICE;
        }

        public EusbRetractDevicePdu(uint interfaceId)
        {
            base.InterfaceId = interfaceId;
            base.Mask = Mask_Values.STREAM_ID_PROXY;
            base.FunctionId = (uint)FunctionId_Values.RETRACT_DEVICE;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32((uint)Reason);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                Reason = (USB_RETRACT_REASON)marshaler.ReadUInt32();
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
            string s = base.ToString();
            s += string.Format("Reason: {0}\r\n", Reason);

            return s;
        }
    }
    #endregion

    #region Request Completion Interface
    /// <summary>
    /// The IOCONTROL_COMPLETION request is sent from the client to the server as the final result of an IO Control
    /// request or internal IO Control request.
    /// </summary>
    public class EusbIoControlCompletionPdu : EusbRequestPdu
    {
        /// <summary>
        /// This field MUST match the value sent previously in the RequestId field of the IO_CONTROL message, as
        /// specified in section 2.2.6.3.
        /// </summary>
        public uint RequestId { get; set; }

        /// <summary>
        /// A 32-bit unsigned integer that indicates the HRESULT of the operation.
        /// </summary>
        public uint HResult { get; set; }

        /// <summary>
        /// The number of bytes of data to be transferred by the request.
        /// </summary>
        public uint Information { get; set; }

        /// <summary>
        /// The size, in bytes, of the OutputBuffer field.
        /// </summary>
        public uint OutputBufferSize { get; set; }

        /// <summary>
        /// A data buffer that results from processing the request.
        /// </summary>
        public byte[] OutputBuffer { get; set; }

        public EusbIoControlCompletionPdu()
        {
            base.Mask = Mask_Values.STREAM_ID_PROXY;
            base.FunctionId = (uint)FunctionId_Values.IOCONTROL_COMPLETION;
        }

        public EusbIoControlCompletionPdu(uint interfaceId)
        {
            base.InterfaceId = interfaceId;
            base.Mask = Mask_Values.STREAM_ID_PROXY;
            base.FunctionId = (uint)FunctionId_Values.IOCONTROL_COMPLETION;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(RequestId);
            marshaler.WriteUInt32(HResult);
            marshaler.WriteUInt32(Information);
            marshaler.WriteUInt32(OutputBufferSize);
            marshaler.WriteBytes(OutputBuffer);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                RequestId = marshaler.ReadUInt32();
                HResult = marshaler.ReadUInt32();
                if (HResult == (uint)HRESULT_FROM_WIN32.ERROR_SUCCESS)
                {
                    Information = marshaler.ReadUInt32();
                    OutputBufferSize = marshaler.ReadUInt32();
                    OutputBuffer = marshaler.ReadBytes((int)OutputBufferSize);
                }
                else if (HResult == (uint)HRESULT_FROM_WIN32.ERROR_INSUFFICIENT_BUFFER)
                {
                    Information = marshaler.ReadUInt32();
                    OutputBufferSize = marshaler.ReadUInt32();
                    OutputBuffer = marshaler.ReadBytes((int)Information);
                }
                else
                {
                    Information = marshaler.ReadUInt32();
                    OutputBufferSize = marshaler.ReadUInt32();
                    OutputBuffer = marshaler.ReadBytes((int)OutputBufferSize);
                }
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
            string s = base.ToString();
            s += string.Format("RequestId: 0x{0:x8}\r\n", RequestId);
            s += string.Format("HResult: 0x{0:x8}\r\n", HResult);
            s += string.Format("Information: {0}\r\n", Information);
            s += string.Format("OutputBufferSize: {0}\r\n", OutputBufferSize);
            s += string.Format("OutputBuffer: {0}\r\n", BitConverter.ToString(OutputBuffer));

            return s;
        }
    }

    /// <summary>
    /// The URB_COMPLETION request is sent from the client to the server as the final result of a 
    /// TRANSFER_IN_REQUEST that contains output data.
    /// </summary>
    public class EusbUrbCompletionPdu : EusbRequestPdu
    {
        /// <summary>
        /// This field MUST match the value sent previously in the RequestId field of TsUrb structure
        /// in the TRANSFER_IN_REQUEST message.
        /// </summary>
        public uint RequestId { get; set; }

        /// <summary>
        /// The size, in bytes, of the TsUrbResult field.
        /// </summary>
        public uint CbTsUrbResult { get; set; }

        /// <summary>
        /// A TS_URB_RESULT structure as defined in 2.2.10.
        /// </summary>
        public byte[] TsUrbResult { get; set; }

        /// <summary>
        /// 32-bit unsigned integer that indicates the HRESULT of the operation.
        /// </summary>
        public uint HResult { get; set; }

        /// <summary>
        /// The size, in bytes, of the OutputBuffer field.
        /// </summary>
        public uint OutputBufferSize { get; set; }

        /// <summary>
        /// A data buffer that results from processing the request.
        /// </summary>
        public byte[] OutputBuffer { get; set; }

        public EusbUrbCompletionPdu()
        {
            base.Mask = Mask_Values.STREAM_ID_PROXY;
            base.FunctionId = (uint)FunctionId_Values.URB_COMPLETION;
        }

        public EusbUrbCompletionPdu(uint interfaceId)
        {
            base.InterfaceId = interfaceId;
            base.Mask = Mask_Values.STREAM_ID_PROXY;
            base.FunctionId = (uint)FunctionId_Values.URB_COMPLETION;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(RequestId);
            marshaler.WriteUInt32(CbTsUrbResult);
            marshaler.WriteBytes(TsUrbResult);
            marshaler.WriteUInt32(HResult);
            marshaler.WriteUInt32(OutputBufferSize);
            marshaler.WriteBytes(OutputBuffer);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                RequestId = marshaler.ReadUInt32();
                CbTsUrbResult = marshaler.ReadUInt32();
                TsUrbResult = marshaler.ReadBytes((int)CbTsUrbResult);
                HResult = marshaler.ReadUInt32();
                OutputBufferSize = marshaler.ReadUInt32();
                OutputBuffer = marshaler.ReadBytes((int)OutputBufferSize);
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
            string s = base.ToString();
            s += string.Format("RequestId: 0x{0:x8}\r\n", RequestId);
            s += string.Format("CbTsUrbResult: {0}\r\n", CbTsUrbResult);
            s += string.Format("TsUrbResult: {0}\r\n", BitConverter.ToString(TsUrbResult));
            s += string.Format("HResult: 0x{0:x8}\r\n", HResult);
            s += string.Format("OutputBufferSize: {0}\r\n", OutputBufferSize);
            s += string.Format("OutputBuffer: {0}\r\n", BitConverter.ToString(OutputBuffer));

            return s;
        }
    }

    /// <summary>
    /// The URB_COMPLETION_NO_DATA request is sent from the client to the server as the final result of a 
    /// TRANSFER_IN_REQUEST that contains no output data or a TRANSFER_OUT_REQUEST.
    /// </summary>
    public class EusbUrbCompletionNoDataPdu : EusbRequestPdu
    {
        /// <summary>
        /// This field MUST match the value sent previously in the RequestId field of TsUrb structure in
        /// the TRANSFER_IN_REQUEST or TRANSFER_OUT_REQUEST message.
        /// </summary>
        public uint RequestId { get; set; }

        /// <summary>
        /// The size, in bytes, of the TsUrbResult field.
        /// </summary>
        public uint CbTsUrbResult { get; set; }

        /// <summary>
        /// A TS_URB_RESULT structure as defined in 2.2.10.
        /// </summary>
        public byte[] TsUrbResult { get; set; }

        /// <summary>
        /// 32-bit unsigned integer that indicates the HRESULT of the operation.
        /// </summary>
        public uint HResult { get; set; }

        /// <summary>
        /// The size, in bytes, of data sent to the device of the RequestId that corresponds to a 
        /// TRANSFER_OUT_REQUEST. This field MUST be zero if the RequestId corresponds to a TRANSFER_IN_REQUEST.
        /// </summary>
        public uint OutputBufferSize { get; set; }

        public EusbUrbCompletionNoDataPdu()
        {
            base.Mask = Mask_Values.STREAM_ID_PROXY;
            base.FunctionId = (uint)FunctionId_Values.URB_COMPLETION;
        }

        public EusbUrbCompletionNoDataPdu(uint interfaceId)
        {
            base.InterfaceId = interfaceId;
            base.Mask = Mask_Values.STREAM_ID_PROXY;
            base.FunctionId = (uint)FunctionId_Values.URB_COMPLETION;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(RequestId);
            marshaler.WriteUInt32(CbTsUrbResult);
            marshaler.WriteBytes(TsUrbResult);
            marshaler.WriteUInt32(HResult);
            marshaler.WriteUInt32(OutputBufferSize);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                RequestId = marshaler.ReadUInt32();
                CbTsUrbResult = marshaler.ReadUInt32();
                TsUrbResult = marshaler.ReadBytes((int)CbTsUrbResult);
                HResult = marshaler.ReadUInt32();
                OutputBufferSize = marshaler.ReadUInt32();
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
            string s = base.ToString();
            s += string.Format("RequestId: 0x{0:x8}\r\n", RequestId);
            s += string.Format("CbTsUrbResult: {0}\r\n", CbTsUrbResult);
            s += string.Format("TsUrbResult: {0}\r\n", BitConverter.ToString(TsUrbResult));
            s += string.Format("HResult: 0x{0:x8}\r\n", HResult);
            s += string.Format("OutputBufferSize: {0}\r\n", OutputBufferSize);

            return s;
        }
    }
    #endregion

    #region Unknown Eusb PDUs
    /// <summary>
    /// This class is used when parsing MS-RDPEUSB PDUs fails.
    /// </summary>
    public class EusbUnknownPdu : EusbPdu
    {
        public byte[] Data { get; set; }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteBytes(Data);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                Data = marshaler.ReadToEnd();
            }
            catch (Exception)
            {
                Data = null;
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
            string s = "null";
            if (null != Data)
            {
                s = string.Format("Data: {0}\r\n", BitConverter.ToString(Data));
            }
            return s;
        }
    }
    #endregion
}
