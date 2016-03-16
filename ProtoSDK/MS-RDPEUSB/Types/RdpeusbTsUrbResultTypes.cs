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
    /// The base class of TS_URB_RESULT structures.
    /// Sending in response to the TRANSFER_IN_REQUEST and TRANSFER_OUT_REQUEST messages.
    /// Be sent via the URB_COMPLETION or URB_COMPLETION_NO_DATA messages.
    /// These messages contain the TS_URB_RESULT field.
    /// </summary>
    public abstract class TS_URB_RESULT : BasePDU
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected TS_URB_RESULT()
        {
            header = new TS_URB_RESULT_HEADER();
        }

        /// <summary>
        /// Common header of TS_URB_RESULT
        /// </summary>
        protected TS_URB_RESULT_HEADER header;

        /// <summary>
        /// Common header of TS_URB_RESULT
        /// </summary>
        public TS_URB_RESULT_HEADER Header
        {
            set { header = value; }
            get { return header; }
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
    /// The TS_USBD_INTERFACE_INFORMATION_RESULT structure
    /// Based on the USBD_INTERFACE_INFORMATION structure
    /// </summary>
    public class TS_USBD_INTERFACE_INFORMATION : BasePDU
    {
        /// <summary>
        /// A 16-bit unsigned integer.
        /// The size in bytes of the TS_USBD_INTERFACE_INFORMATION structure
        /// </summary>
        private ushort len;

        /// <summary>
        /// A 16-bit unsigned integer.
        /// The size in bytes of the TS_USBD_INTERFACE_INFORMATION structure
        /// </summary>
        public ushort Length
        {
            set { len = value; }
            get { return len; }
        }

        /// <summary>
        /// A 32-bit unsigned integer
        /// The number of USBD_PIPE_INFORMATION structures found in the USBD_INTERFACE_INFORMATION.
        /// Also indicates the number of Pipes array elements that are to follow.
        /// </summary>
        private ushort numberOfPipesExpected;

        /// <summary>
        /// A 32-bit unsigned integer.
        /// The number of USBD_PIPE_INFORMATION structures found in the USBD_INTERFACE_INFORMATION.
        /// Also indicates the number of Pipes array elements that are to follow.
        /// </summary>
        public ushort NumberOfPipesExpected
        {
            set { numberOfPipesExpected = value; }
            get { return numberOfPipesExpected; }
        }

        /// <summary>
        /// A 8-bit unsigned integer. This value is from the InterfaceNumber field in USBD_INTERFACE_INFORMATION.
        /// </summary>
        private byte interfaceNumber;

        /// <summary>
        /// A 8-bit unsigned integer. This value is from the InterfaceNumber field in USBD_INTERFACE_INFORMATION.
        /// </summary>
        public byte InterfaceNumber
        {
            set { interfaceNumber = value; }
            get { return interfaceNumber; }
        }

        /// <summary>
        /// A 8-bit unsigned integer.
        /// Represents the AlternateSetting field in USBD_INTERFACE_INFORMATION.
        /// </summary>
        private byte alternateSetting;

        /// <summary>
        /// A 8-bit unsigned integer.
        /// Represents the AlternateSetting field in USBD_INTERFACE_INFORMATION.
        /// </summary>
        public byte AlternateSetting
        {
            set { alternateSetting = value; }
            get { return alternateSetting; }
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
        /// A 32-bit unsigned integer
        /// Represents the NumberOfPipes field in USBD_INTERFACE_INFORMATION
        /// Also indicates the number of Pipes array elements that are to follow.
        /// </summary>
        private uint numberOfPipes;

        /// <summary>
        /// A 32-bit unsigned integer
        /// Represents the NumberOfPipes field in USBD_INTERFACE_INFORMATION
        /// Also indicates the number of Pipes array elements that are to follow.
        /// </summary>
        public uint NumberOfPipes
        {
            set { numberOfPipes = value; }
            get { return numberOfPipes; }
        }

        /// <summary>
        /// An array of TS_USBD_PIPE_INFORMATION structures
        /// </summary>
        private TS_USBD_PIPE_INFORMATION[] informations;

        /// <summary>
        /// An array of TS_USBD_PIPE_INFORMATION structures
        /// </summary>
        public TS_USBD_PIPE_INFORMATION[] Infomations
        {
            set { informations = value; }
            get { return informations; }
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteUInt16(len);
            marshaler.WriteUInt16(numberOfPipesExpected);
            marshaler.WriteByte(interfaceNumber);
            marshaler.WriteByte(alternateSetting);
            marshaler.WriteUInt16(padding);
            marshaler.WriteUInt32(numberOfPipes);

            for (int i = 0; i < informations.Length; i++)
            {
                informations[i].Encode(marshaler);
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
                len = marshaler.ReadUInt16();
                numberOfPipes = marshaler.ReadUInt16();
                interfaceNumber = marshaler.ReadByte();
                alternateSetting = marshaler.ReadByte();
                padding = marshaler.ReadUInt16();
                numberOfPipes = marshaler.ReadUInt32();

                informations = new TS_USBD_PIPE_INFORMATION[numberOfPipes];
                for (int i = 0; i < numberOfPipes; i++)
                {
                    informations[i].Decode(marshaler);
                }
            }
            catch (Exception)
            {
                // TODO: All generic exceptions should be got rid of.
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
            string s = string.Format("len: 0x{0:x4}\r\n", len);
            s += string.Format("numberOfPipes: 0x{0:x4}\r\n", numberOfPipes);
            s += string.Format("interfaceNumber: 0x{0:x2}\r\n", interfaceNumber);
            s += string.Format("alternateSetting: 0x{0:x2}\r\n", alternateSetting);
            s += string.Format("padding: 0x{0:x4}\r\n", padding);
            s += string.Format("numberOfPipes: 0x{0:x8}\r\n", numberOfPipes);
            s += string.Format("informations: \r\n");

            for (int i = 0; i < numberOfPipes; i++)
            {
                s += string.Format("informations[{0}]: \r\n", i);
                s += informations[i].ToString();
            }

            return s;
        }
    };

    /// <summary>
    /// Common header of TS_URB_RESULT
    /// </summary>
    public class TS_URB_RESULT_HEADER : BasePDU
    {
        /// <summary>
        /// The size in bytes of the TS_URB_RESULT structure
        /// </summary>
        private ushort size;

        /// <summary>
        /// The size in bytes of the TS_URB_RESULT structure
        /// </summary>
        public ushort Size
        {
            set { size = value; }
            get { return size; }
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
        /// This value represents the USBD Status
        /// </summary>
        private uint usbdStatus;

        /// <summary>
        /// This value represents the USBD Status
        /// </summary>
        public uint UsbdStatus
        {
            set { usbdStatus = value; }
            get { return usbdStatus; }
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteUInt16(size);
            marshaler.WriteUInt16(padding);
            marshaler.WriteUInt32(usbdStatus);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                size = marshaler.ReadUInt16();
                padding = marshaler.ReadUInt16();
                usbdStatus = marshaler.ReadUInt32();
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
            string s = string.Format("size: 0x{0:x4}\r\n", size);
            s += string.Format("padding: 0x{0:x4}\r\n", padding);
            s += string.Format("usbdStatus: 0x{0:x8}\r\n", usbdStatus);

            return s;
        }
    };

    /// <summary>
    /// Based on the USBD_INTERFACE_INFORMATION structure 
    /// </summary>
    public class TS_USBD_INTERFACE_INFORMATION_RESULT : BasePDU
    {
        /// <summary>
        /// The size in bytes of the TS_USBD_INTERFACE_INFORMATION_RESULT structure
        /// </summary>
        private ushort length;

        /// <summary>
        /// The size in bytes of the TS_USBD_INTERFACE_INFORMATION_RESULT structure
        /// </summary>
        public ushort Length
        {
            set { length = value; }
            get { return length; }
        }

        /// <summary>
        /// This value represents the InterfaceNumber field in USBD_INTERFACE_INFORMATION
        /// </summary>
        private byte interfaceNumber;

        /// <summary>
        /// This value represents the InterfaceNumber field in USBD_INTERFACE_INFORMATION
        /// </summary>
        public byte InterfaceNumber
        {
            set { interfaceNumber = value; }
            get { return interfaceNumber; }
        }

        /// <summary>
        /// This value represents the AlternateSetting field in USBD_INTERFACE_INFORMATION
        /// </summary>
        private byte alternateSetting;

        /// <summary>
        /// This value represents the AlternateSetting field in USBD_INTERFACE_INFORMATION
        /// </summary>
        public byte AlternateSetting
        {
            set { alternateSetting = value; }
            get { return alternateSetting; }
        }

        /// <summary>
        /// This value represents the Class field in USBD_INTERFACE_INFORMATION
        /// </summary>
        private byte class_field;

        /// <summary>
        /// This value represents the Class field in USBD_INTERFACE_INFORMATION
        /// </summary>
        public byte Class
        {
            set { class_field = value; }
            get { return class_field; }
        }

        /// <summary>
        /// This value represents the SubClass field in USBD_INTERFACE_INFORMATION
        /// </summary>
        private byte subClass;

        /// <summary>
        /// This value represents the SubClass field in USBD_INTERFACE_INFORMATION
        /// </summary>
        public byte SubClass
        {
            set { subClass = value; }
            get { return subClass; }
        }

        /// <summary>
        /// This value represents the Protocol field in USBD_INTERFACE_INFORMATION
        /// </summary>
        private byte protocol;

        /// <summary>
        /// This value represents the Protocol field in USBD_INTERFACE_INFORMATION
        /// </summary>
        public byte Protocol
        {
            set { protocol = value; }
            get { return protocol; }
        }

        /// <summary>
        /// A 8-bit unsigned integer for padding
        /// </summary>
        private byte padding;

        /// <summary>
        /// A 8-bit unsigned integer for padding
        /// </summary>
        public byte Padding
        {
            set { padding = value; }
            get { return padding; }
        }

        /// <summary>
        /// This value represents the InterfaceHandle field in USBD_INTERFACE_INFORMATION
        /// </summary>
        private uint interfaceHandle;

        /// <summary>
        /// This value represents the InterfaceHandle field in USBD_INTERFACE_INFORMATION
        /// </summary>
        public uint InterfaceHandle
        {
            set { interfaceHandle = value; }
            get { return interfaceHandle; }
        }

        /// <summary>
        /// This value represents the NumberOfPipes field in USBD_INTERFACE_INFORMATION
        /// </summary>
        private uint numberOfPipes;

        /// <summary>
        /// This value represents the NumberOfPipes field in USBD_INTERFACE_INFORMATION
        /// </summary>
        public uint NumberOfPipes
        {
            set { numberOfPipes = value; }
            get { return numberOfPipes; }
        }

        /// <summary>
        /// An array of TS_USBD_PIPE_INFORMATION_RESULT structures
        /// </summary>
        private TS_USBD_PIPE_INFORMATION_RESULT[] pipes;

        /// <summary>
        /// An array of TS_USBD_PIPE_INFORMATION_RESULT structures
        /// </summary>
        public TS_USBD_PIPE_INFORMATION_RESULT[] Pipes
        {
            set { pipes = value; }
            get { return pipes; }
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteUInt16(length);
            marshaler.WriteByte(interfaceNumber);
            marshaler.WriteByte(alternateSetting);
            marshaler.WriteByte(class_field);
            marshaler.WriteByte(subClass);
            marshaler.WriteByte(protocol);
            marshaler.WriteByte(padding);
            marshaler.WriteUInt32(interfaceHandle);
            marshaler.WriteUInt32(numberOfPipes);

            if (null != pipes)
            {
                for (int i = 0; i < pipes.Length; i++)
                {
                    pipes[i].Encode(marshaler);
                }
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
                length = marshaler.ReadUInt16();
                interfaceNumber = marshaler.ReadByte();
                alternateSetting = marshaler.ReadByte();
                class_field = marshaler.ReadByte();
                subClass = marshaler.ReadByte();
                protocol = marshaler.ReadByte();
                padding = marshaler.ReadByte();
                interfaceHandle = marshaler.ReadUInt32();
                numberOfPipes = marshaler.ReadUInt32();

                if (numberOfPipes > 0)
                {
                    pipes = new TS_USBD_PIPE_INFORMATION_RESULT[numberOfPipes];

                    for (int i = 0; i < pipes.Length; i++)
                    {
                        pipes[i] = new TS_USBD_PIPE_INFORMATION_RESULT();
                        pipes[i].Decode(marshaler);
                    }
                }
                else
                {
                    pipes = null;
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
            sb.AppendFormat("length: 0x{0:x4}\r\n", length);
            sb.AppendFormat("interfaceNumber: 0x{0:x2}\r\n", interfaceNumber);
            sb.AppendFormat("alternateSetting: 0x{0:x2}\r\n", alternateSetting);
            sb.AppendFormat("class_field: 0x{0:x2}\r\n", class_field);
            sb.AppendFormat("subClass: 0x{0:x2}\r\n", subClass);
            sb.AppendFormat("protocol: 0x{0:x2}\r\n", protocol);
            sb.AppendFormat("padding: 0x{0:x2}\r\n", padding);
            sb.AppendFormat("interfaceHandle: 0x{0:x8}\r\n", interfaceHandle);
            sb.AppendFormat("numberOfPipes: 0x{0:x8}\r\n", numberOfPipes);

            if (null != pipes)
            {
                for (int i = 0; i < pipes.Length; i++)
                {
                    sb.AppendFormat("pipes[{0}]: \r\n{1}", i, pipes[i]);
                }
            }

            return sb.ToString();
        }
    }

    /// <summary>
    /// Based on the USBD_PIPE_INFORMATION structure 
    /// </summary>
    public class TS_USBD_PIPE_INFORMATION : BasePDU
    {
        /// <summary>
        /// A 16-bit unsigned integer.
        /// This value is from the MaximumPacketSize field in USBD_PIPE_INFORMATION
        /// </summary>
        private ushort maximumPacketSize;

        /// <summary>
        /// A 16-bit unsigned integer.
        /// This value is from the MaximumPacketSize field in USBD_PIPE_INFORMATION
        /// </summary>
        public ushort MaximumPacketSize
        {
            set { maximumPacketSize = value; }
            get { return maximumPacketSize; }
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
        /// A 32-bit unsigned integer. 
        /// This value is from the MaximumTransferSize field in USBD_PIPE_INFORMATION
        /// </summary>
        private uint maximumTransferSize;

        /// <summary>
        /// A 32-bit unsigned integer. 
        /// This value is from the MaximumTransferSize field in USBD_PIPE_INFORMATION
        /// </summary>
        public uint MaximumTransferSize
        {
            set { maximumTransferSize = value; }
            get { return maximumTransferSize; }
        }

        /// <summary>
        /// A 32-bit unsigned integer. 
        /// This value is from the PipeFlags field in USBD_PIPE_INFORMATION
        /// </summary>
        private uint pipeFlags;

        /// <summary>
        /// A 32-bit unsigned integer. 
        /// This value is from the PipeFlags field in USBD_PIPE_INFORMATION
        /// </summary>
        public uint PipeFlags
        {
            set { pipeFlags = value; }
            get { return pipeFlags; }
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteUInt16(maximumPacketSize);
            marshaler.WriteUInt16(padding);
            marshaler.WriteUInt32(maximumTransferSize);
            marshaler.WriteUInt32(pipeFlags);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                maximumPacketSize = marshaler.ReadUInt16();
                padding = marshaler.ReadUInt16();
                maximumTransferSize = marshaler.ReadUInt32();
                pipeFlags = marshaler.ReadUInt32();
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
            string s = string.Format("maximumPacketSize: 0x{0:x4}\r\n", maximumPacketSize);
            s += string.Format("padding: 0x{0:x4}\r\n", padding);
            s += string.Format("maximumTransferSize: 0x{0:x8}\r\n", maximumTransferSize);
            s += string.Format("pipeFlags: 0x{0:x8}\r\n", pipeFlags);

            return s;
        }
    };

    /// <summary>
    /// Based on the USBD_PIPE_INFORMATION structure
    /// </summary>
    public class TS_USBD_PIPE_INFORMATION_RESULT : BasePDU
    {
        /// <summary>
        /// A 16-bit unsigned integer.
        /// This value represents the MaximumPacketSize field in USBD_PIPE_INFORMATION.
        /// </summary>
        private ushort maximumPacketSize;

        /// <summary>
        /// A 16-bit unsigned integer. 
        /// This value represents the MaximumPacketSize field in USBD_PIPE_INFORMATION.
        /// </summary>
        public ushort MaximumPacketSize
        {
            set { maximumPacketSize = value; }
            get { return maximumPacketSize; }
        }

        /// <summary>
        /// A 8-bit unsigned integer
        /// This value represents the EndpointAddress field in USBD_PIPE_INFORMATION
        /// </summary>
        private byte endpointAddress;

        /// <summary>
        /// A 8-bit unsigned integer
        /// This value represents the EndpointAddress field in USBD_PIPE_INFORMATION
        /// </summary>
        public byte EndpointAddress
        {
            set { endpointAddress = value; }
            get { return endpointAddress; }
        }

        /// <summary>
        /// A 8-bit unsigned integer.
        /// This value represents the Interval field in USBD_PIPE_INFORMATION
        /// </summary>
        private byte interval;

        /// <summary>
        /// A 8-bit unsigned integer.
        /// This value represents the Interval field in USBD_PIPE_INFORMATION
        /// </summary>
        public byte Interval
        {
            set { interval = value; }
            get { return interval; }
        }

        /// <summary>
        /// A 32-bit unsigned integer.
        /// This value represents the PipeType field in USBD_PIPE_INFORMATION
        /// </summary>
        private uint pipeType;

        /// <summary>
        /// A 32-bit unsigned integer.
        /// This value represents the PipeType field in USBD_PIPE_INFORMATION
        /// </summary>
        public uint PipeType
        {
            set { pipeType = value; }
            get { return pipeType; }
        }

        /// <summary>
        /// A 32-bit unsigned integer
        /// An opaque handle to the pipe
        /// </summary>
        private uint pipeHandle;

        /// <summary>
        /// A 32-bit unsigned integer
        /// An opaque handle to the pipe
        /// </summary>
        public uint PipeHandle
        {
            set { pipeHandle = value; }
            get { return pipeHandle; }
        }

        /// <summary>
        /// A 32-bit unsigned integer
        /// This value represents the MaximumTransferSize field in USBD_PIPE_INFORMATION
        /// </summary>
        private uint maximumTransferSize;

        /// <summary>
        /// A 32-bit unsigned integer
        /// This value represents the MaximumTransferSize field in USBD_PIPE_INFORMATION
        /// </summary>
        public uint MaximumTransferSize
        {
            set { maximumTransferSize = value; }
            get { return maximumTransferSize; }
        }

        /// <summary>
        /// A 32-bit unsigned integer
        /// This value represents the PipeFlags field in USBD_PIPE_INFORMATION
        /// </summary>
        private uint pipeFlags;

        /// <summary>
        /// A 32-bit unsigned integer
        /// This value represents the PipeFlags field in USBD_PIPE_INFORMATION
        /// </summary>
        public uint PipeFlags
        {
            set { pipeFlags = value; }
            get { return pipeFlags; }
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteUInt16(maximumPacketSize);
            marshaler.WriteByte(endpointAddress);
            marshaler.WriteByte(interval);
            marshaler.WriteUInt32(pipeType);
            marshaler.WriteUInt32(pipeHandle);
            marshaler.WriteUInt32(maximumTransferSize);
            marshaler.WriteUInt32(pipeFlags);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                maximumPacketSize = marshaler.ReadUInt16();
                endpointAddress = marshaler.ReadByte();
                interval = marshaler.ReadByte();
                pipeType = marshaler.ReadUInt32();
                pipeHandle = marshaler.ReadUInt32();
                maximumTransferSize = marshaler.ReadUInt32();
                pipeFlags = marshaler.ReadUInt32();
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
            string s = string.Format("maximumPacketSize: 0x{0:x4}\r\n", maximumPacketSize);
            s += string.Format("endpointAddress: 0x{0:x2}\r\n", endpointAddress);
            s += string.Format("interval: 0x{0:x2}\r\n", interval);
            s += string.Format("pipeType: 0x{0:x8}\r\n", pipeType);
            s += string.Format("pipeHandle: 0x{0:x8}\r\n", pipeHandle);
            s += string.Format("maximumTransferSize: 0x{0:x8}\r\n", maximumTransferSize);
            s += string.Format("pipeFlags: 0x{0:x8}\r\n", pipeFlags);

            return s;
        }
    }

    /// <summary>
    /// Represents the result of the TRANSFER_IN_REQUEST with TS_URB_SELECT_CONFIGURATION. The TS_URB_SELECT_CONFIGURATION_RESULT.
    /// Be sent via the URB_COMPLETION_NO_DATA message.
    /// </summary>
    public class TS_URB_SELECT_CONFIGURATION_RESULT : TS_URB_RESULT
    {
        /// <summary>
        /// A 32-bit unsigned integer.
        /// An opaque handle that identifies the configuration described by TS_URB_SELECT_CONFIGURATION
        /// </summary>
        private uint configurationHandle;

        /// <summary>
        /// A 32-bit unsigned integer.
        /// An opaque handle that identifies the configuration described by TS_URB_SELECT_CONFIGURATION
        /// </summary>
        public uint ConfigurationHandle
        {
            set { configurationHandle = value; }
            get { return configurationHandle; }
        }

        /// <summary>
        /// A 32-bit unsigned integer.
        /// The number of Interface fields that are to follow
        /// </summary>
        private uint numInterfaces;

        /// <summary>
        /// A 32-bit unsigned integer.
        /// The number of Interface fields that are to follow
        /// </summary>
        public uint NumInterfaces
        {
            set { numInterfaces = value; }
            get { return numInterfaces; }
        }

        /// <summary>
        /// TS_USBD_INTERFACE_INFORMATION_RESULT structures 
        /// </summary>
        private TS_USBD_INTERFACE_INFORMATION_RESULT[] interfaces;

        /// <summary>
        /// TS_USBD_INTERFACE_INFORMATION_RESULT structures 
        /// </summary>
        public TS_USBD_INTERFACE_INFORMATION_RESULT[] Interface
        {
            set { interfaces = value; }
            get { return interfaces; }
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(configurationHandle);
            marshaler.WriteUInt32(numInterfaces);

            if (null != Interface)
            {
                for (int i = 0; i < Interface.Length; i++)
                {
                    interfaces[i].Encode(marshaler);
                }
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
                if (!base.Decode(marshaler))
                {
                    return false;
                }
                configurationHandle = marshaler.ReadUInt32();
                numInterfaces = marshaler.ReadUInt32();

                if (numInterfaces == 0)
                {
                    interfaces = null;
                }
                else
                {
                    interfaces = new TS_USBD_INTERFACE_INFORMATION_RESULT[numInterfaces];
                    for (int i = 0; i < numInterfaces; i++)
                    {
                        interfaces[i] = new TS_USBD_INTERFACE_INFORMATION_RESULT();
                        if (!interfaces[i].Decode(marshaler))
                        {
                            return false;
                        }
                    }
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
            sb.AppendFormat("configurationHandle: 0x{0:x8}\r\n", configurationHandle);
            sb.AppendFormat("numInterfaces: 0x{0:x8}\r\n", numInterfaces);

            if (numInterfaces > 0)
            {
                sb.AppendFormat("Interface: \r\n");
                for (int i = 0; i < interfaces.Length; i++)
                {
                    sb.AppendFormat("interface [{0}]:\r\n{1}", i, interfaces[i]);
                }
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// Class include result of the TRANSFER_IN_REQUEST with TS_URB_SELECT_INTERFACE
    /// </summary>
    public class TS_URB_SELECT_INTERFACE_RESULT : TS_URB_RESULT
    {
        /// <summary>
        /// A TS_USBD_INTERFACE_INFORMATION_RESULT structure 
        /// </summary>
        private TS_USBD_INTERFACE_INFORMATION_RESULT interface_field;

        /// <summary>
        /// A TS_USBD_INTERFACE_INFORMATION_RESULT structure 
        /// </summary>
        public TS_USBD_INTERFACE_INFORMATION_RESULT Interface
        {
            set { interface_field = value; }
            get { return interface_field; }
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            interface_field.Encode(marshaler);
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
                interface_field.Decode(marshaler);
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
            s += interface_field.ToString();

            return s;
        }
    }

    /// <summary>
    /// Class include result of the TRANSFER_IN_REQUEST with TS_URB_GET_CURRENT_FRAME_NUMBER
    /// </summary>
    public class TS_URB_GET_CURRENT_FRAME_NUMBER_RESULT : TS_URB_RESULT
    {
        /// <summary>
        /// A 32-bit unsigned integer.
        /// The current frame number
        /// </summary>
        private uint frameNumber;

        /// <summary>
        /// A 32-bit unsigned integer.
        /// The current frame number
        /// </summary>
        public uint FrameNumber
        {
            set { frameNumber = value; }
            get { return frameNumber; }
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(frameNumber);
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
                frameNumber = marshaler.ReadUInt32();
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
            s += string.Format("frameNumber: 0x{0:x8}\r\n", frameNumber);

            return s;
        }
    }

    /// <summary>
    /// Class include result of TRANSFER_IN_REQUEST or TRANSFER_OUT_REQUEST with TS_URB_ISOCH_TRANSFER
    /// </summary>
    public class TS_URB_ISOCH_TRANSFER_RESULT : TS_URB_RESULT
    {
        /// <summary>
        /// The resulting StartFrame value as specified in URB_ISOCH_TRANSFER
        /// </summary>
        private uint startFrame;

        /// <summary>
        /// The resulting StartFrame value as specified in URB_ISOCH_TRANSFER
        /// </summary>
        public uint StartFrame
        {
            set { startFrame = value; }
            get { return startFrame; }
        }

        /// <summary>
        /// This value is the number of URB_ISOCH_TRANSFER following the IsoPacket field
        /// </summary>
        private uint numberOfPackets;

        /// <summary>
        /// This value is the number of URB_ISOCH_TRANSFER following the IsoPacket field
        /// </summary>
        public uint NumberOfPackets
        {
            set { numberOfPackets = value; }
            get { return numberOfPackets; }
        }

        /// <summary>
        /// The resulting ErrorCount value as described in URB_ISOCH_TRANSFER
        /// </summary>
        private uint errorCount;

        /// <summary>
        /// The resulting ErrorCount value as described in URB_ISOCH_TRANSFER
        /// </summary>
        public uint ErrorCount
        {
            set { errorCount = value; }
            get { return errorCount; }
        }

        /// <summary>
        /// The resulting array of USBD_ISO_PACKET_DESCRIPTOR structures 
        /// </summary>
        private USBD_ISO_PACKET_DESCRIPTOR[] isoPacket;

        /// <summary>
        /// The resulting array of USBD_ISO_PACKET_DESCRIPTOR structures 
        /// </summary>
        public USBD_ISO_PACKET_DESCRIPTOR[] IsoPacket
        {
            set { isoPacket = value; }
            get { return isoPacket; }
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
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
}
