// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeusb
{
    /// <summary>
    /// A base class of TS_URB structures
    /// Be sent in response to a URB request it receives from the system.
    /// </summary>
    public class TS_URB : BasePDU
    {
        /// <summary>
        /// Common header of TS_URB
        /// </summary>
        protected TS_URB_HEADER header;

        /// <summary>
        /// Common header of TS_URB
        /// </summary>
        public TS_URB_HEADER Header
        {
            set { header = value; }
            get { return header; }
        }

        public TS_URB()
        {
            header = new TS_URB_HEADER();
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            header.Encode(marshaler);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                header.Decode(marshaler);
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
            return header.ToString();
        }
    }

    /// <summary>
    /// Common header of TS_URB Structures
    /// </summary>
    public class TS_URB_HEADER : BasePDU
    {
        /// <summary>
        /// The size of the TS_URB structure
        /// The TS_URB structure is in bytes.
        /// </summary>
        //private ushort size;

        /// <summary>
        /// The size of the TS_URB structure
        /// The TS_URB structure is in bytes.
        /// </summary>
        public ushort Size { get; set; }

        /// <summary>
        /// The max value of the RequestID.
        /// </summary>
        const int MAX_REQUEST_ID = 0x7FFFFFFF;

        /// <summary>
        /// The offset of the RequestID.
        /// </summary>
        //const int OFF_SET_REQUEST_ID = 1;

        /// <summary>
        /// The max value of the NoAck.
        /// </summary>
        const byte MAX_VALUE_NO_ACK = 1;

        /// <summary>
        /// A 16-bit unsigned integer.
        /// Represents The function of URB.
        /// </summary>
        private URB_FUNCTIONID URBFunction;

        /// <summary>
        /// A 16-bit unsigned integer.
        /// Represents The function of URB.
        /// </summary>
        public URB_FUNCTIONID URB_Function
        {
            set { URBFunction = value; }
            get { return URBFunction; }
        }
        
        private uint requestId;
        /// <summary>
        /// An ID that uniquely similar to GUID.
        /// Identifies the TRANSFER_IN_REQUEST or TRANSFER_OUT_REQUEST
        /// </summary>
        public uint RequestId
        {
            set
            {
                // RequestId is 31 bits, so max value is 0x7FFFFFFF;
                const uint maxValue = (uint)1 << 30;
                if (value > maxValue)
                {
                    throw new ArgumentException(String.Format("RequestId cannot exceed the max value ({0}}.", maxValue));
                }
                requestId = value;
            }
            get
            {
                return requestId;
            }
        }

        private byte noAck;
        /// <summary>
        /// This bit indicates if the client should send a URB_COMPLETION message
        /// </summary>
        public byte NoAck
        {
            set
            {
                // NoAck is 1 bit, so must be 0 or 1
                if (NoAck > 1)
                {
                    throw new ArgumentException("NoAck must be 0 or 1");
                }
                noAck = value;
            }

            get
            {
                return noAck;
            }
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteUInt16(Size);
            marshaler.WriteUInt16((ushort)URBFunction);
            marshaler.WriteUInt32(((uint)noAck << 31) | RequestId);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                Size = marshaler.ReadUInt16();
                URBFunction = (URB_FUNCTIONID)marshaler.ReadUInt16();
                uint temp = marshaler.ReadUInt32();
                noAck = (byte)(temp >> 31);
                requestId = temp & 0x7FFFFFFF;
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
            sb.AppendFormat("size: 0x{0:x4} ({0})\r\n", Size);
            sb.AppendFormat("URBFunction: 0x{0:x4}\r\n", (ushort)URBFunction);
            sb.AppendFormat("RequestId: {0} NoAck: {1} (0x{2:x8})\r\n", requestId, noAck, ((uint)noAck << 31) | RequestId);
            sb.AppendFormat("", noAck);
            return sb.ToString();
        }
    }

    /// <summary>
    /// Represents URB_SELECT_CONFIGURATION
    /// Be sent using TRANSFER_IN_REQUEST
    /// </summary>
    public class TS_URB_SELECT_CONFIGURATION : TS_URB
    {
        /// <summary>
        /// The size of the padding field in this class.
        /// </summary>
        const int PADDING_SIZE = 3;

        /// <summary>
        /// A 8-bit unsigned integer
        /// Indicates that the TS_URB_SELECT_CONFIGURATION.
        /// Contains the USB_CONFIGURATION_DESCRIPTOR field.
        /// </summary>
        private byte configurationDescriptorIsValid;

        /// <summary>
        /// A 8-bit unsigned integer
        /// Indicates that the TS_URB_SELECT_CONFIGURATION.
        /// Contains the USB_CONFIGURATION_DESCRIPTOR field.
        /// </summary>
        public byte ConfigurationDescriptorIsValid
        {
            set { configurationDescriptorIsValid = value; }
            get { return configurationDescriptorIsValid; }
        }

        /// <summary>
        /// A 24-bit unsigned integer for padding.
        /// </summary>
        private byte[] padding;

        /// <summary>
        /// A 24-bit unsigned integer for padding.
        /// </summary>
        public byte[] Padding
        {
            get { return padding; }
        }

        /// <summary>
        /// A 32-bit unsigned integer.
        /// The number of TS_USBD_INTERFACE_INFORMATION structures
        /// They are in the TS_URB_SELECT_CONFIGURATION.
        /// </summary>
        private uint numInterfaces;

        /// <summary>
        /// A 32-bit unsigned integer.
        /// The number of TS_USBD_INTERFACE_INFORMATION structures
        /// They are in the TS_URB_SELECT_CONFIGURATION.
        /// </summary>
        public uint NumInterfaces
        {
            set { numInterfaces = value; }
            get { return numInterfaces; }
        }

        /// <summary>
        /// A TS_USBD_INTERFACE_INFORMATION structure.
        /// Based on the USBD_INTERFACE_INFORMATION 
        /// </summary>
        private TS_USBD_INTERFACE_INFORMATION[] intefaces;

        /// <summary>
        /// A TS_USBD_INTERFACE_INFORMATION structure.
        /// Based on the USBD_INTERFACE_INFORMATION 
        /// </summary>
        public TS_USBD_INTERFACE_INFORMATION[] Interfaces
        {
            set { intefaces = value; }
            get { return intefaces; }
        }

        /// <summary>
        /// A USB_CONFIGURATION_DESCRIPTOR structure
        /// describes information about a specific USB device configuration.
        /// USB devices can support several different configurations in which different interfaces on the device might behave in different ways.
        /// </summary>
        private byte[] descriptor;

        /// <summary>
        /// A USB_CONFIGURATION_DESCRIPTOR structure
        /// describes information about a specific USB device configuration.
        /// USB devices can support several different configurations in which different interfaces on the device might behave in different ways.
        /// </summary>
        public byte[] UsbConfDesc
        {
            set { descriptor = value; }
            get { return descriptor; }
        }

        /// <summary>
        /// The Constructor function of TS_URB_SELECT_CONFIGURATION
        /// </summary>
        public TS_URB_SELECT_CONFIGURATION()
        {
            header = new TS_URB_HEADER();
            header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_SELECT_CONFIGURATION;
            header.RequestId = 0;
            header.NoAck = (byte)0;

            padding = new byte[PADDING_SIZE];
            for (int i = 0; i < PADDING_SIZE; i++)
            {
                padding[i] = 0;
            }
        }

        public TS_URB_SELECT_CONFIGURATION(uint requestId, byte noAck)
        {
            header = new TS_URB_HEADER();
            header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_SELECT_CONFIGURATION;
            header.RequestId = requestId;
            header.NoAck = noAck;

            padding = new byte[PADDING_SIZE];
            for (int i = 0; i < PADDING_SIZE; i++)
            {
                padding[i] = 0;
            }
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteByte(configurationDescriptorIsValid);
            marshaler.WriteBytes(padding);
            marshaler.WriteUInt32(numInterfaces);
            for (int i = 0; i < intefaces.Length; i++)
            {
                intefaces[i].Encode(marshaler);
            }
            marshaler.WriteBytes(descriptor);
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
                configurationDescriptorIsValid = marshaler.ReadByte();
                padding = marshaler.ReadBytes(PADDING_SIZE);
                numInterfaces = marshaler.ReadUInt32();
                for (int i = 0; i < intefaces.Length; i++)
                {
                    intefaces[i].Decode(marshaler);
                }
                descriptor = marshaler.ReadToEnd();
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
            s += string.Format("configurationDescriptorIsValid: 0x{0:x2}\r\n", configurationDescriptorIsValid);
            s += string.Format("padding: 0x{0}\r\n", BitConverter.ToString(padding));
            s += string.Format("numInterfaces: 0x{0:x8}\r\n", numInterfaces);
            s += intefaces.ToString();
            s += descriptor.ToString();

            return s;
        }
    }

    /// <summary>
    /// Represents an URB_SELECT_INTERFACE.
    /// Be sent using the TRANSFER_IN_REQUEST message with OutputBufferSize set to zero.
    /// </summary>
    public class TS_URB_SELECT_INTERFACE : TS_URB
    {
        /// <summary>
        /// A 32-bit unsigned integer.
        /// Returned from the client after it successfully completes a TS_URB_SELECT_CONFIGURATION request.
        /// </summary>
        private uint configurationHandle;

        /// <summary>
        /// A 32-bit unsigned integer.
        /// Returned from the client after it successfully completes a TS_URB_SELECT_CONFIGURATION request.
        /// </summary>
        public uint ConfigurationHandle
        {
            set { configurationHandle = value; }
            get { return configurationHandle; }
        }

        /// <summary>
        /// A TS_USBD_INTERFACE_INFORMATION structure
        /// Based on the USBD_INTERFACE_INFORMATION structure
        /// </summary>
        private TS_USBD_INTERFACE_INFORMATION information;

        /// <summary>
        /// A TS_USBD_INTERFACE_INFORMATION structure
        /// Based on the USBD_INTERFACE_INFORMATION structure
        /// </summary>
        public TS_USBD_INTERFACE_INFORMATION TsUsbdIInfo
        {
            set { information = value; }
            get { return information; }
        }

        /// <summary>
        /// The Constructor function of TS_URB_SELECT_INTERFACE
        /// </summary>
        public TS_URB_SELECT_INTERFACE()
        {
            header = new TS_URB_HEADER();
            header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_SELECT_INTERFACE;
            header.RequestId = 0;
            header.NoAck = (byte)0;
        }

        public TS_URB_SELECT_INTERFACE(uint requestId, byte noAck)
        {
            header = new TS_URB_HEADER();
            header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_SELECT_INTERFACE;
            header.RequestId = requestId;
            header.NoAck = noAck;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(configurationHandle);
            information.Encode(marshaler);
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
                configurationHandle = marshaler.ReadUInt32();
                information.Decode(marshaler);
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
            s += string.Format("configurationHandle: 0x{0:x8}\r\n", configurationHandle);
            s += information.ToString();

            return s;
        }
    }

    /// <summary>
    /// Represents URB_PIPE_REQUEST.
    /// Be sent using the TRANSFER_IN_REQUEST message.
    /// </summary>
    public class TS_URB_PIPE_REQUEST : TS_URB
    {
        /// <summary>
        /// A 32-bit unsigned integer.
        /// The handle returned from the client after it successfully completes.
        /// </summary>
        private uint pipeHandle;

        /// <summary>
        /// A 32-bit unsigned integer.
        /// The handle returned from the client after it successfully completes.
        /// </summary>
        public uint PipeHandle
        {
            set { pipeHandle = value; }
            get { return pipeHandle; }
        }

        /// <summary>
        /// The Constructor function of TS_URB_PIPE_REQUEST
        /// </summary>
        public TS_URB_PIPE_REQUEST()
        {
            header = new TS_URB_HEADER();
            header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_SYNC_RESET_PIPE;
            header.RequestId = 0;
            header.NoAck = (byte)0;
        }

        public TS_URB_PIPE_REQUEST(uint requestId, byte noAck, URB_FUNCTIONID functionId)
        {
            header = new TS_URB_HEADER();
            header.URB_Function = functionId;
            header.RequestId = requestId;
            header.NoAck = noAck;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(pipeHandle);
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
                pipeHandle = marshaler.ReadUInt32();
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
            s += string.Format("pipeHandle: 0x{0:x8}\r\n", pipeHandle);

            return s;
        }
    }

    /// <summary>
    /// Represents URB_GET_CURRENT_FRAME_NUMBER
    /// </summary>
    public class TS_URB_GET_CURRENT_FRAME_NUMBER : TS_URB
    {
        /// <summary>
        /// The Constructor function of TS_URB_GET_CURRENT_FRAME_NUMBER
        /// </summary>
        public TS_URB_GET_CURRENT_FRAME_NUMBER()
        {
            header = new TS_URB_HEADER();
            header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_GET_CURRENT_FRAME_NUMBER;
            header.RequestId = 0;
            header.NoAck = (byte)0;
            header.Size = 8;
        }

        public TS_URB_GET_CURRENT_FRAME_NUMBER(uint requestId, byte noAck)
        {
            header = new TS_URB_HEADER();
            header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_GET_CURRENT_FRAME_NUMBER;
            header.RequestId = requestId;
            header.NoAck = noAck;
            header.Size = 8;
        }
    }

    /// <summary>
    /// Represents an URB_CONTROL
    /// The server converts URB_CONTROL_TRANSFER into the TS_URB_CONTROL_TRANSFER structure.
    /// </summary>
    public class TS_URB_CONTROL_TRANSFER : TS_URB
    {
        /// <summary>
        /// The Size of the setup packets.
        /// </summary>
        const int PACKET_SIZE = 8;

        /// <summary>
        /// A 32-bit unsigned integer.
        /// The handle returned from the client after it successfully completes
        /// </summary>
        private uint pipeHandle;

        /// <summary>
        /// A 32-bit unsigned integer.
        /// The handle returned from the client after it successfully completes
        /// </summary>
        public uint PipeHandle
        {
            set { pipeHandle = value; }
            get { return pipeHandle; }
        }

        /// <summary>
        /// A 32-bit unsigned integer. 
        /// This value is from the TransferFlags field in URB_CONTROL_TRANSFER
        /// </summary>
        private uint transferFlags;

        /// <summary>
        /// A 32-bit unsigned integer. 
        /// This value is from the TransferFlags field in URB_CONTROL_TRANSFER
        /// </summary>
        public uint TransferFlags
        {
            set { transferFlags = value; }
            get { return transferFlags; }
        }

        /// <summary>
        /// An 8-byte array
        /// This value is from the SetupPacket field in URB_CONTROL_TRANSFER
        /// </summary>
        private byte[] setupPacket;

        /// <summary>
        /// An 8-byte array
        /// This value is from the SetupPacket field in URB_CONTROL_TRANSFER
        /// </summary>
        public byte[] SetupPacket
        {
            set { setupPacket = value; }
            get { return setupPacket; }
        }

        /// <summary>
        /// The Constructor function of TS_URB_CONTROL_TRANSFER
        /// </summary>
        public TS_URB_CONTROL_TRANSFER()
        {
            header = new TS_URB_HEADER();
            header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_CONTROL_TRANSFER;
            header.RequestId = 0;
            header.NoAck = (byte)0;

            setupPacket = new byte[PACKET_SIZE];
        }

        public TS_URB_CONTROL_TRANSFER(uint requestId, bool noAck)
        {
            header = new TS_URB_HEADER();
            header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_CONTROL_TRANSFER;
            header.RequestId = requestId;
            header.NoAck = (byte)(noAck ? 1 : 0);

            setupPacket = new byte[PACKET_SIZE];
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(pipeHandle);
            marshaler.WriteUInt32(transferFlags);
            marshaler.WriteBytes(setupPacket);
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
                pipeHandle = marshaler.ReadUInt32();
                transferFlags = marshaler.ReadUInt32();
                setupPacket = marshaler.ReadBytes(PACKET_SIZE);
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
            s += string.Format("pipeHandle: 0x{0:x8}\r\n", pipeHandle);
            s += string.Format("transferFlags: 0x{0:x8}\r\n", transferFlags);
            s += string.Format("setupPacket: 0x{0}\r\n", BitConverter.ToString(setupPacket));

            return s;
        }
    }

    /// <summary>
    /// Represents an URB_BULK_OR_INTERRUPT_TRANSFER
    /// </summary>
    public class TS_URB_BULK_OR_INTERRUPT_TRANSFER : TS_URB
    {
        /// <summary>
        /// A 32-bit unsigned integer. 
        /// The handle returned from the client after it successfully completes 
        /// </summary>
        private uint pipeHandle;

        /// <summary>
        /// A 32-bit unsigned integer. 
        /// The handle returned from the client after it successfully completes 
        /// </summary>
        public uint PipeHandle
        {
            set { pipeHandle = value; }
            get { return pipeHandle; }
        }

        /// <summary>
        /// A 32-bit unsigned integer. 
        /// This value is from the TransferFlags field in URB_BULK_OR_INTERRUPT_TRANSFER
        /// </summary>
        private uint transferFlags;

        /// <summary>
        /// A 32-bit unsigned integer. 
        /// This value is from the TransferFlags field in URB_BULK_OR_INTERRUPT_TRANSFER
        /// </summary>
        public uint TransferFlags
        {
            set { transferFlags = value; }
            get { return transferFlags; }
        }

        /// <summary>
        /// The Constructor function of TS_URB_BULK_OR_INTERRUPT_TRANSFER
        /// </summary>
        public TS_URB_BULK_OR_INTERRUPT_TRANSFER()
        {
            header = new TS_URB_HEADER();
            header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_BULK_OR_INTERRUPT_TRANSFER;
            header.RequestId = 0;
            header.NoAck = (byte)0;
        }

        public TS_URB_BULK_OR_INTERRUPT_TRANSFER(uint requestId, byte noAck)
        {
            header = new TS_URB_HEADER();
            header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_BULK_OR_INTERRUPT_TRANSFER;
            header.RequestId = requestId;
            header.NoAck = noAck;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(pipeHandle);
            marshaler.WriteUInt32(transferFlags);
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
                pipeHandle = marshaler.ReadUInt32();
                transferFlags = marshaler.ReadUInt32();
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
            s += string.Format("pipeHandle: 0x{0:x8}\r\n", pipeHandle);
            s += string.Format("transferFlags: 0x{0:x8}\r\n", transferFlags);

            return s;
        }
    }

    /// <summary>
    /// Represents an URB_ISOCH_TRANSFER
    /// </summary>
    public class TS_URB_ISOCH_TRANSFER : TS_URB
    {
        /// <summary>
        /// A 32-bit unsigned integer.
        /// The handle returned from the client after it successfully completes
        /// </summary>
        private uint pipeHandle;

        /// <summary>
        /// A 32-bit unsigned integer.
        /// The handle returned from the client after it successfully completes
        /// </summary>
        public uint PipeHandle
        {
            set { pipeHandle = value; }
            get { return pipeHandle; }
        }

        /// <summary>
        /// A 32-bit unsigned integer
        /// This value is from the TransferFlags field in URB_ISOCH_TRANSFER
        /// </summary>
        private uint transferFlags;

        /// <summary>
        /// A 32-bit unsigned integer
        /// This value is from the TransferFlags field in URB_ISOCH_TRANSFER
        /// </summary>
        public uint TransferFlags
        {
            set { transferFlags = value; }
            get { return transferFlags; }
        }

        /// <summary>
        /// This value is from the StartFrame field in URB_ISOCH_TRANSFER
        /// </summary>
        private uint startFrame;

        /// <summary>
        /// This value is from the StartFrame field in URB_ISOCH_TRANSFER
        /// </summary>
        public uint StartFrame
        {
            set { startFrame = value; }
            get { return startFrame; }
        }

        /// <summary>
        /// A 32-bit unsigned integer
        /// This value is from the NumberOfPackets field in URB_ISOCH_TRANSFER
        /// </summary>
        private uint numberOfPackets;

        /// <summary>
        /// A 32-bit unsigned integer
        /// This value is from the NumberOfPackets field in URB_ISOCH_TRANSFER
        /// </summary>
        public uint NumberOfPackets
        {
            set { numberOfPackets = value; }
            get { return numberOfPackets; }
        }

        /// <summary>
        /// A 32-bit unsigned integer
        /// This value is from the ErrorCount field in URB_ISOCH_TRANSFER
        /// </summary>
        private uint errorCount;

        /// <summary>
        /// A 32-bit unsigned integer
        /// This value is from the ErrorCount field in URB_ISOCH_TRANSFER
        /// </summary>
        public uint ErrorCount
        {
            set { errorCount = value; }
            get { return errorCount; }
        }

        /// <summary>
        /// An array of USBD_ISO_PACKET_DESCRIPTOR structures
        /// From the IsoPacket field in URB_ISOCH_TRANSFER.
        /// </summary>
        private USBD_ISO_PACKET_DESCRIPTOR[] isoPacket;

        /// <summary>
        /// An array of USBD_ISO_PACKET_DESCRIPTOR structures
        /// From the IsoPacket field in URB_ISOCH_TRANSFER.
        /// </summary>
        public USBD_ISO_PACKET_DESCRIPTOR[] IsoPacket
        {
            set { isoPacket = value; }
            get { return isoPacket; }
        }

        /// <summary>
        /// The Constructor function of TS_URB_ISOCH_TRANSFER
        /// </summary>
        public TS_URB_ISOCH_TRANSFER()
        {
            header = new TS_URB_HEADER();
            header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_ISOCH_TRANSFER;
            header.RequestId = 0;
            header.NoAck = (byte)0;
        }

        public TS_URB_ISOCH_TRANSFER(uint requestId, bool noAck)
        {
            header = new TS_URB_HEADER();
            header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_ISOCH_TRANSFER;
            header.RequestId = requestId;
            header.NoAck = (byte)(noAck ? 1 : 0);
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(pipeHandle);
            marshaler.WriteUInt32(transferFlags);
            marshaler.WriteUInt32(startFrame);
            marshaler.WriteUInt32(numberOfPackets);
            marshaler.WriteUInt32(errorCount);

            for (int i = 0; i < isoPacket.Length; i++)
            {
                isoPacket[i].Encode(marshaler);
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
                pipeHandle = marshaler.ReadUInt32();
                transferFlags = marshaler.ReadUInt32();
                startFrame = marshaler.ReadUInt32();
                numberOfPackets = marshaler.ReadUInt32();
                errorCount = marshaler.ReadUInt32();

                isoPacket = new USBD_ISO_PACKET_DESCRIPTOR[numberOfPackets];
                for (int i = 0; i < isoPacket.Length; i++)
                {
                    isoPacket[i].Decode(marshaler);
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
            s += string.Format("pipeHandle: 0x{0:x8}\r\n", pipeHandle);
            s += string.Format("transferFlags: 0x{0:x8}\r\n", transferFlags);
            s += string.Format("startFrame: 0x{0:x8}\r\n", startFrame);
            s += string.Format("numberOfPackets: 0x{0:x8}\r\n", numberOfPackets);
            s += string.Format("errorCount: 0x{0:x8}\r\n", errorCount);

            for (int i = 0; i < isoPacket.Length; i++)
            {
                s += string.Format("isoPacket[{0}]: \r\n", i);
                s += isoPacket[i].ToString();
            }

            return s;
        }
    }

    /// <summary>
    /// Represents an URB_CONTROL_DESCRIPTOR_REQUEST
    /// </summary>
    public class TS_URB_CONTROL_DESCRIPTOR_REQUEST : TS_URB
    {

        /// <summary>
        /// A 8-bit unsigned integer.
        /// This value is from the Index field in URB_CONTROL_DESCRIPTOR_REQUEST
        /// </summary>
        private byte index;

        /// <summary>
        /// A 8-bit unsigned integer.
        /// This value is from the Index field in URB_CONTROL_DESCRIPTOR_REQUEST
        /// </summary>
        public byte Index
        {
            set { index = value; }
            get { return index; }
        }

        /// <summary>
        /// A 8-bit unsigned integer
        /// This value is from the DescriptorType field in URB_CONTROL_DESCRIPTOR_REQUEST
        /// </summary>
        private byte descriptorType;

        /// <summary>
        /// A 8-bit unsigned integer
        /// This value is from the DescriptorType field in URB_CONTROL_DESCRIPTOR_REQUEST
        /// </summary>
        public byte DescriptorType
        {
            set { descriptorType = value; }
            get { return descriptorType; }
        }

        /// <summary>
        /// A 16-bit unsigned integer.
        /// This value is from the LanguageId field in URB_CONTROL_DESCRIPTOR_REQUEST
        /// </summary>
        private ushort languageId;

        /// <summary>
        /// A 16-bit unsigned integer.
        /// This value is from the LanguageId field in URB_CONTROL_DESCRIPTOR_REQUEST
        /// </summary>
        public ushort LanguageId
        {
            set { languageId = value; }
            get { return languageId; }
        }

        public TS_URB_CONTROL_DESCRIPTOR_REQUEST()
        {
            header = new TS_URB_HEADER();
            header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_GET_DESCRIPTOR_FROM_DEVICE;
            header.RequestId = 0;
            header.NoAck = (byte)0;
        }

        public TS_URB_CONTROL_DESCRIPTOR_REQUEST(uint requestId, bool noAck, URB_FUNCTIONID functionId)
        {
            header = new TS_URB_HEADER();
            header.URB_Function = functionId;
            header.RequestId = requestId;
            header.NoAck = (byte)(noAck ? 1 : 0);
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteByte(index);
            marshaler.WriteByte(descriptorType);
            marshaler.WriteUInt16(languageId);
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
                index = marshaler.ReadByte();
                descriptorType = marshaler.ReadByte();
                languageId = marshaler.ReadUInt16();
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
            s += string.Format("index: 0x{0:x2}\r\n", index);
            s += string.Format("descriptorType: 0x{0:x2}\r\n", descriptorType);
            s += string.Format("languageId: 0x{0:x4}\r\n", languageId);

            return s;
        }
    }


    /// <summary>
    /// Represents a URB_CONTROL_FEATURE_REQUEST
    /// Be sent using the TRANSFER_IN_REQUEST message
    /// </summary>
    public class TS_URB_CONTROL_FEATURE_REQUEST : TS_URB
    {
        /// <summary>
        /// A 16-bit unsigned integer
        /// This value is from the Index field in URB_CONTROL_FEATURE_REQUEST
        /// </summary>
        private byte featureSelector;

        /// <summary>
        /// A 16-bit unsigned integer
        /// This value is from the Index field in URB_CONTROL_FEATURE_REQUEST
        /// </summary>
        public byte FeatureSelector
        {
            set { featureSelector = value; }
            get { return featureSelector; }
        }

        /// <summary>
        /// A 16-bit unsigned integer
        /// This value is from the Index field in URB_CONTROL_FEATURE_REQUEST
        /// </summary>
        private ushort index;

        /// <summary>
        /// A 16-bit unsigned integer
        /// This value is from the Index field in URB_CONTROL_FEATURE_REQUEST
        /// </summary>
        public ushort Index
        {
            set { index = value; }
            get { return index; }
        }

        /// <summary>
        /// The Constructor function of TS_URB_CONTROL_FEATURE_REQUEST
        /// </summary>
        public TS_URB_CONTROL_FEATURE_REQUEST()
        {
            header = new TS_URB_HEADER();
            header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_GET_MS_FEATURE_DESCRIPTOR;
            header.RequestId = 0;
            header.NoAck = (byte)0;
        }

        public TS_URB_CONTROL_FEATURE_REQUEST(uint requestId, bool noAck, URB_FUNCTIONID functionId)
        {
            header = new TS_URB_HEADER();
            header.URB_Function = functionId;
            header.RequestId = requestId;
            header.NoAck = (byte)(noAck ? 1 : 0);
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteByte(featureSelector);
            marshaler.WriteUInt16(index);
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
                featureSelector = marshaler.ReadByte();
                index = marshaler.ReadUInt16();
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
            s += string.Format("featureSelector: 0x{0:x2}\r\n", featureSelector);
            s += string.Format("index: 0x{0:x4}\r\n", index);

            return s;
        }
    }

    /// <summary>
    /// Represents an URB_CONTROL_GET_STATUS_REQUEST
    /// </summary>
    public class TS_URB_CONTROL_GET_STATUS_REQUEST : TS_URB
    {
        /// <summary>
        /// A 16-bit unsigned integer
        /// This value is from the Index field in URB_CONTROL_GET_STATUS_REQUEST
        /// </summary>
        private ushort index;

        /// <summary>
        /// A 16-bit unsigned integer
        /// This value is from the Index field in URB_CONTROL_GET_STATUS_REQUEST
        /// </summary>
        public ushort Index
        {
            set { index = value; }
            get { return index; }
        }

        /// <summary>
        /// A 16-bit unsigned integer for padding
        /// </summary>
        private ushort padding;

        /// <summary>
        /// A 16-bit unsigned integer for padding
        /// </summary>
        public ushort Padding
        {
            set { padding = value; }
            get { return padding; }
        }

        /// <summary>
        /// The Constructor function of TS_URB_CONTROL_GET_STATUS_REQUEST
        /// </summary>
        public TS_URB_CONTROL_GET_STATUS_REQUEST()
        {
            header = new TS_URB_HEADER();
            header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_GET_STATUS_FROM_DEVICE;
            header.RequestId = 0;
            header.NoAck = (byte)0;
            header.Size = 12;
        }

        public TS_URB_CONTROL_GET_STATUS_REQUEST(uint requestId, byte noAck, URB_FUNCTIONID functionId)
        {
            header = new TS_URB_HEADER();
            header.URB_Function = functionId;
            header.RequestId = requestId;
            header.NoAck = noAck;
            header.Size = 12;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt16(index);
            marshaler.WriteUInt16(padding);
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
                index = marshaler.ReadUInt16();
                padding = marshaler.ReadUInt16();
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
            s += string.Format("index: 0x{0:x4}\r\n", index);
            s += string.Format("padding: 0x{0:x4}\r\n", padding);

            return s;
        }
    }

    /// <summary>
    /// Represents an URB_CONTROL_VENDOR_OR_CLASS_REQUEST
    /// </summary>
    public class TS_URB_CONTROL_VENDOR_OR_CLASS_REQUEST : TS_URB
    {

        /// <summary>
        /// A 32-bit unsigned integer.
        /// This value is from the TransferFlags field in URB_CONTROL_VENDOR_OR_CLASS_REQUEST
        /// </summary>
        private uint transferFlags;

        /// <summary>
        /// A 32-bit unsigned integer.
        /// This value is from the TransferFlags field in URB_CONTROL_VENDOR_OR_CLASS_REQUEST
        /// </summary>
        public uint TransferFlags
        {
            set { transferFlags = value; }
            get { return transferFlags; }
        }

        /// <summary>
        /// This value is from the RequestTypeReservedBits field in URB_CONTROL_VENDOR_OR_CLASS_REQUEST
        /// </summary>
        private byte requestTypeReservedBits;

        /// <summary>
        /// A 8-bit unsigned integer.
        /// This value is from the RequestTypeReservedBits field in URB_CONTROL_VENDOR_OR_CLASS_REQUEST
        /// </summary>
        public byte RequestTypeReservedBits
        {
            set { requestTypeReservedBits = value; }
            get { return requestTypeReservedBits; }
        }

        /// <summary>
        /// A 8-bit unsigned integer.
        /// This value is from the Request field in URB_CONTROL_VENDOR_OR_CLASS_REQUEST
        /// </summary>
        private byte request;

        /// <summary>
        /// A 8-bit unsigned integer.
        /// This value is from the Request field in URB_CONTROL_VENDOR_OR_CLASS_REQUEST
        /// </summary>
        public byte Request
        {
            set { request = value; }
            get { return request; }
        }

        /// <summary>
        /// A 16-bit unsigned integer. 
        /// This value is from the Value field in URB_CONTROL_VENDOR_OR_CLASS_REQUEST
        /// </summary>
        private ushort value_field;

        /// <summary>
        /// A 16-bit unsigned integer. 
        /// This value is from the Value field in URB_CONTROL_VENDOR_OR_CLASS_REQUEST
        /// </summary>
        public ushort Value
        {
            set { value_field = value; }
            get { return value_field; }
        }

        /// <summary>
        /// A 16-bit unsigned integer.
        /// This value is from the Index field in URB_CONTROL_VENDOR_OR_CLASS_REQUEST
        /// </summary>
        private ushort index;

        /// <summary>
        /// A 16-bit unsigned integer.
        /// This value is from the Index field in URB_CONTROL_VENDOR_OR_CLASS_REQUEST
        /// </summary>
        public ushort Index
        {
            set { index = value; }
            get { return index; }
        }

        /// <summary>
        /// A 16-bit unsigned integer for padding
        /// </summary>
        private ushort padding;

        /// <summary>
        /// A 16-bit unsigned integer for padding
        /// </summary>
        public ushort Padding
        {
            set { padding = value; }
            get { return padding; }
        }

        /// <summary>
        /// The Constructor function of TS_URB_CONTROL_VENDOR_OR_CLASS_REQUEST
        /// </summary>
        public TS_URB_CONTROL_VENDOR_OR_CLASS_REQUEST()
        {
            header = new TS_URB_HEADER();
            header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_VENDOR_DEVICE;
            header.RequestId = 0;
            header.NoAck = (byte)0;
        }

        public TS_URB_CONTROL_VENDOR_OR_CLASS_REQUEST(uint requestId, byte noAck, URB_FUNCTIONID functionId)
        {
            header = new TS_URB_HEADER();
            header.URB_Function = functionId;
            header.RequestId = requestId;
            header.NoAck = noAck;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(transferFlags);
            marshaler.WriteByte(requestTypeReservedBits);
            marshaler.WriteByte(request);
            marshaler.WriteUInt16(value_field);
            marshaler.WriteUInt16(index);
            marshaler.WriteUInt16(padding);
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
                transferFlags = marshaler.ReadUInt32();
                requestTypeReservedBits = marshaler.ReadByte();
                request = marshaler.ReadByte();
                value_field = marshaler.ReadUInt16();
                index = marshaler.ReadUInt16();
                padding = marshaler.ReadUInt16();
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
            s += string.Format("transferFlags: 0x{0:x8}\r\n", transferFlags);
            s += string.Format("requestTypeReservedBits: 0x{0:x2}\r\n", requestTypeReservedBits);
            s += string.Format("request: 0x{0:x2}\r\n", request);
            s += string.Format("value_field: 0x{0:x4}\r\n", value_field);
            s += string.Format("index: 0x{0:x4}\r\n", index);
            s += string.Format("padding: 0x{0:x4}\r\n", padding);

            return s;
        }
    }

    /// <summary>
    /// Represents an URB_CONTROL_GET_CONFIGURATION_REQUEST 
    /// </summary>
    public class TS_URB_CONTROL_GET_CONFIGURATION_REQUEST : TS_URB
    {
        /// <summary>
        /// The Constructor function of TS_URB_CONTROL_GET_CONFIGURATION_REQUEST
        /// </summary>
        public TS_URB_CONTROL_GET_CONFIGURATION_REQUEST()
        {
            header = new TS_URB_HEADER();
            header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_GET_CONFIGURATION;
            header.RequestId = 0;
            header.NoAck = (byte)0;
            header.Size = 8;
        }

        public TS_URB_CONTROL_GET_CONFIGURATION_REQUEST(uint requestId, byte noAck)
        {
            header = new TS_URB_HEADER();
            header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_GET_CONFIGURATION;
            header.RequestId = requestId;
            header.NoAck = noAck;
            header.Size = 8;
        }
    }

    /// <summary>
    /// Represents an URB_CONTROL_GET_INTERFACE_REQUEST
    /// </summary>
    public class TS_URB_CONTROL_GET_INTERFACE_REQUEST : TS_URB
    {
        /// <summary>
        /// A 16-bit unsigned integer.
        /// This value is from the Interface field in URB_CONTROL_GET_INTERFACE_REQUEST
        /// </summary>
        private ushort interface_field;

        /// <summary>
        /// A 16-bit unsigned integer.
        /// This value is from the Interface field in URB_CONTROL_GET_INTERFACE_REQUEST
        /// </summary>
        public ushort Interface
        {
            set { interface_field = value; }
            get { return interface_field; }
        }

        /// <summary>
        /// A 16-bit unsigned integer for padding
        /// </summary>
        private ushort padding;

        /// <summary>
        /// A 16-bit unsigned integer for padding
        /// </summary>
        public ushort Padding
        {
            set { padding = value; }
            get { return padding; }
        }

        /// <summary>
        /// The Constructor function of TS_URB_CONTROL_GET_INTERFACE_REQUEST
        /// </summary>
        public TS_URB_CONTROL_GET_INTERFACE_REQUEST()
        {
            header = new TS_URB_HEADER();
            header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_GET_INTERFACE;
            header.RequestId = 0;
            header.NoAck = (byte)0;
            header.Size = 12;
        }

        public TS_URB_CONTROL_GET_INTERFACE_REQUEST(uint requestId, byte noAck)
        {
            header = new TS_URB_HEADER();
            header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_GET_INTERFACE;
            header.RequestId = requestId;
            header.NoAck = noAck;
            header.Size = 12;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt16(interface_field);
            marshaler.WriteUInt16(padding);
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
                interface_field = marshaler.ReadUInt16();
                padding = marshaler.ReadUInt16();
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
            s += string.Format("interface_field: 0x{0:x4}\r\n", interface_field);
            s += string.Format("padding: 0x{0:x4}\r\n", padding);

            return s;
        }
    }

    /// <summary>
    /// Represents an URB_OS_FEATURE_DESCRIPTOR_REQUEST
    /// </summary>
    public class TS_URB_OS_FEATURE_DESCRIPTOR_REQUEST : TS_URB
    {
        /// <summary>
        /// The Size of padding2
        /// Used as Offset
        /// </summary>
        const int PADDING_SIZE = 3;

        /// <summary>
        /// The max value of the Recipient
        /// </summary>
        const int MAX_VALUE_RECIPIENT = 0x1f;

        const int HIGH_3_BIT_MASK = 0xE0;
        const int LOW_5_BIT_MASK = 0x1F;

        /// <summary>
        /// The max value of the Padding
        /// </summary>
        const int MAX_VALUE_PADDING = 0x7;

        /// <summary>
        /// A 8 bits data contents recipient and padding1 value
        /// </summary>
        private byte recipientAndPadding1;

        /// <summary>
        /// This value is from the Interface field in URB_OS_FEATURE_DESCRIPTOR_REQUES
        /// </summary>
        public byte Recipient
        {
            set
            {
                if (value > MAX_VALUE_RECIPIENT)
                {
                    throw new ArgumentException(String.Format("Recipient is a 5-bit field, must less than {0}.", MAX_VALUE_RECIPIENT));
                }
                else
                {
                    // TODO: Should always resets high bits to zero?
                    recipientAndPadding1 = (byte)(value & LOW_5_BIT_MASK);
                }
            }
            get
            {
                return (byte)(recipientAndPadding1 & LOW_5_BIT_MASK);
            }
        }

        /// <summary>
        /// A 3-bit unsigned integer for padding
        /// </summary>
        public byte Padding
        {
            // 0xE0 is 3 high-bits mask.
            get { return (byte)((recipientAndPadding1 & HIGH_3_BIT_MASK) >> 5); }
        }

        /// <summary>
        /// This value is from the InterfaceNumber field in URB_OS_FEATURE_DESCRIPTOR_REQUEST
        /// </summary>
        private byte interfaceNumber;

        /// <summary>
        /// This value is from the InterfaceNumber field in URB_OS_FEATURE_DESCRIPTOR_REQUEST
        /// </summary>
        public byte InterfaceNumber
        {
            set { interfaceNumber = value; }
            get { return interfaceNumber; }
        }

        /// <summary>
        /// This value is from the MS_PageIndex field in URB_OS_FEATURE_DESCRIPTOR_REQUEST
        /// </summary>
        private byte msPageIndex;

        /// <summary>
        /// This value is from the MS_PageIndex field in URB_OS_FEATURE_DESCRIPTOR_REQUEST
        /// </summary>
        public byte MS_PageIndex
        {
            set { msPageIndex = value; }
            get { return msPageIndex; }
        }

        /// <summary>
        /// This value is from the MS_FeatureDescriptorIndex field in URB_OS_FEATURE_DESCRIPTOR_REQUEST
        /// </summary>
        private ushort msFeatureDescriptorIndex;

        /// <summary>
        /// This value is from the MS_FeatureDescriptorIndex field in URB_OS_FEATURE_DESCRIPTOR_REQUEST
        /// </summary>
        public ushort MS_FeatureDescriptorIndex
        {
            set { msFeatureDescriptorIndex = value; }
            get { return msFeatureDescriptorIndex; }
        }

        /// <summary>
        /// A 24-bit unsigned integer for padding
        /// </summary>
        private byte[] padding2;

        /// <summary>
        /// A 24-bit unsigned integer for padding
        /// </summary>
        public byte[] Padding2
        {
            set { padding2 = value; }
            get { return padding2; }
        }

        /// <summary>
        /// The Constructor function of TS_URB_OS_FEATURE_DESCRIPTOR_REQUEST
        /// </summary>
        public TS_URB_OS_FEATURE_DESCRIPTOR_REQUEST()
        {
            header = new TS_URB_HEADER();
            header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_GET_MS_FEATURE_DESCRIPTOR;
            header.RequestId = 0;
            header.NoAck = (byte)0;
            header.Size = 16;
            padding2 = new byte[PADDING_SIZE];
        }

        public TS_URB_OS_FEATURE_DESCRIPTOR_REQUEST(uint requestId, byte noAck)
        {
            header = new TS_URB_HEADER();
            header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_GET_MS_FEATURE_DESCRIPTOR;
            header.RequestId = requestId;
            header.NoAck = noAck;
            header.Size = 16;
            padding2 = new byte[PADDING_SIZE];
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteByte(recipientAndPadding1);
            marshaler.WriteByte(interfaceNumber);
            marshaler.WriteByte(msPageIndex);
            marshaler.WriteUInt16(msFeatureDescriptorIndex);
            marshaler.WriteBytes(padding2);
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
                recipientAndPadding1 = marshaler.ReadByte();
                interfaceNumber = marshaler.ReadByte();
                msPageIndex = marshaler.ReadByte();
                msFeatureDescriptorIndex = marshaler.ReadUInt16();
                padding2 = marshaler.ReadBytes(PADDING_SIZE);
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
            s += string.Format("recipientAndPadding1: 0x{0:x2}\r\n", recipientAndPadding1);
            s += string.Format("interfaceNumber: 0x{0:x2}\r\n", interfaceNumber);
            s += string.Format("msPageIndex: 0x{0:x2}\r\n", msPageIndex);
            s += string.Format("msFeatureDescriptorIndex: 0x{0:x4}\r\n", msFeatureDescriptorIndex);
            s += string.Format("padding2: 0x{0}\r\n", BitConverter.ToString(padding2));

            return s;
        }
    }

    /// <summary>
    /// Represents an URB_CONTROL_TRANSFER_EX
    /// </summary>
    public class TS_URB_CONTROL_TRANSFER_EX : TS_URB
    {
        /// <summary>
        /// Size of the setup packets
        /// </summary>
        const int PACKET_SIZE = 8;

        /// <summary>
        /// The handle returned from the client after it successfully completes
        /// </summary>
        private uint pipeHandle;

        /// <summary>
        /// The handle returned from the client after it successfully completes
        /// </summary>
        public uint PipeHandle
        {
            set { pipeHandle = value; }
            get { return pipeHandle; }
        }

        /// <summary>
        /// This value is from the TransferFlags field in URB_CONTROL_TRANSFER_EX
        /// </summary>
        private uint transferFlags;

        /// <summary>
        /// This value is from the TransferFlags field in URB_CONTROL_TRANSFER_EX
        /// </summary>
        public uint TransferFlags
        {
            set { transferFlags = value; }
            get { return transferFlags; }
        }

        /// <summary>
        /// This value is from the Timeout field in URB_CONTROL_TRANSFER_EX
        /// </summary>
        private uint timeout;

        /// <summary>
        /// This value is from the Timeout field in URB_CONTROL_TRANSFER_EX
        /// </summary>
        public uint Timeout
        {
            set { timeout = value; }
            get { return timeout; }
        }

        /// <summary>
        /// This value is from the SetupPacket field in URB_CONTROL_TRANSFER_EX
        /// </summary>
        private byte[] setupPacket;

        /// <summary>
        /// This value is from the SetupPacket field in URB_CONTROL_TRANSFER_EX
        /// </summary>
        public byte[] SetupPacket
        {
            set { setupPacket = value; }
            get { return setupPacket; }
        }

        /// <summary>
        /// The Constructor function of TS_URB_CONTROL_TRANSFER_EX
        /// </summary>
        public TS_URB_CONTROL_TRANSFER_EX()
        {
            header = new TS_URB_HEADER();
            header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_CONTROL_TRANSFER_EX;
            header.RequestId = 0;
            header.NoAck = (byte)0;

            setupPacket = new byte[PACKET_SIZE];
        }

        public TS_URB_CONTROL_TRANSFER_EX(uint requestId, bool noAck)
        {
            header = new TS_URB_HEADER();
            header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_CONTROL_TRANSFER_EX;
            header.RequestId = requestId;
            header.NoAck = (byte)(noAck ? 1 : 0);

            setupPacket = new byte[PACKET_SIZE];
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(pipeHandle);
            marshaler.WriteUInt32(transferFlags);
            marshaler.WriteUInt32(timeout);
            marshaler.WriteBytes(setupPacket);
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
                pipeHandle = marshaler.ReadUInt32();
                transferFlags = marshaler.ReadUInt32();
                timeout = marshaler.ReadUInt32();
                setupPacket = marshaler.ReadBytes(PACKET_SIZE);
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
            s += string.Format("pipeHandle: 0x{0:x8}\r\n", pipeHandle);
            s += string.Format("transferFlags: 0x{0:x8}\r\n", transferFlags);
            s += string.Format("timeout: 0x{0:x8}\r\n", timeout);
            s += string.Format("setupPacket: 0x{0}\r\n", BitConverter.ToString(setupPacket));

            return s;
        }
    }

    /// <summary>
    /// Represents an unknown structure
    /// </summary>
    public class TS_URB_UNKNOWN : TS_URB
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
}
