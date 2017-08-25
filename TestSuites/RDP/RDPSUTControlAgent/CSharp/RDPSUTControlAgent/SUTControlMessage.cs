// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDPSUTControlAgent
{
    #region Types

    public enum SUTControl_MessageType : ushort
    {
        SUT_CONTROL_REQUEST = 0x0000,
        SUT_CONTROL_RESPONSE = 0x0001
    }

    public enum SUTControl_TestsuiteId : ushort
    {
        RDP_TESTSUITE = 0x0001
    }

    public enum SUTControl_ResultCode : uint
    {
        SUCCESS = 0x00000000,
    }

    #endregion Types

    #region Message

    /// <summary>
    /// SUT Control Request message to SUT
    /// </summary>
    public class SUT_Control_Request_Message
    {
        #region Fields
        public SUTControl_MessageType messageType;
        public SUTControl_TestsuiteId testsuiteId;
        public ushort commandId;
        public uint caseNameLength;
        public string caseName;
        public ushort requestId;

        public uint helpMessageLength;
        public string helpMessage;

        public uint payloadLength;
        public byte[] payload;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public SUT_Control_Request_Message()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="testsuiteId"></param>
        /// <param name="commandId"></param>
        /// <param name="requestId"></param>
        /// <param name="helpMessage"></param>
        /// <param name="payload"></param>
        public SUT_Control_Request_Message(SUTControl_TestsuiteId testsuiteId, ushort commandId, string caseName, ushort requestId, string helpMessage, byte[] payload)
        {
            this.messageType = SUTControl_MessageType.SUT_CONTROL_REQUEST;
            this.testsuiteId = testsuiteId;
            this.commandId = commandId;

            this.caseNameLength = 0;
            this.caseName = caseName;
            if (!string.IsNullOrEmpty(this.caseName))
            {
                this.caseNameLength = (uint)Encoding.UTF8.GetByteCount(this.caseName);
            }

            this.requestId = requestId;

            this.helpMessageLength = 0;
            this.helpMessage = helpMessage;
            if (helpMessage != null)
            {
                this.helpMessageLength = (uint)Encoding.UTF8.GetByteCount(helpMessage);
            }

            this.payloadLength = 0;
            this.payload = payload;
            if (payload != null)
            {
                this.payloadLength = (uint)payload.Length;
            }
        }

        /// <summary>
        /// Encode method
        /// </summary>
        /// <param name="marshaler"></param>
        public byte[] Encode()
        {
            List<byte> bufferList = new List<byte>();
            bufferList.AddRange(Utility.ChangeBytesOrderForNumeric(BitConverter.GetBytes((ushort)this.messageType)));
            bufferList.AddRange(Utility.ChangeBytesOrderForNumeric(BitConverter.GetBytes((ushort)this.testsuiteId)));
            bufferList.AddRange(Utility.ChangeBytesOrderForNumeric(BitConverter.GetBytes((ushort)this.commandId)));

            bufferList.AddRange(Utility.ChangeBytesOrderForNumeric(BitConverter.GetBytes(this.caseNameLength)));
            if (!string.IsNullOrEmpty(this.caseName) && this.caseNameLength > 0)
            {
                bufferList.AddRange(Encoding.UTF8.GetBytes(this.caseName));
            }

            bufferList.AddRange(Utility.ChangeBytesOrderForNumeric(BitConverter.GetBytes((ushort)this.requestId)));
            
            bufferList.AddRange(Utility.ChangeBytesOrderForNumeric(BitConverter.GetBytes(this.helpMessageLength)));
            if (helpMessage != null && helpMessage.Length > 0)
            {
                bufferList.AddRange(Encoding.UTF8.GetBytes(helpMessage));
            }

            bufferList.AddRange(Utility.ChangeBytesOrderForNumeric(BitConverter.GetBytes(this.payloadLength)));
            if(payload != null)
            {
                bufferList.AddRange(this.payload);
            }

            return bufferList.ToArray();
        }

        /// <summary>
        /// Decode method
        /// </summary>
        /// <param name="marshaler"></param>
        /// <returns></returns>
        public bool Decode(byte[] rawData, ref int index)
        {
            try
            {
                this.messageType = (SUTControl_MessageType)BitConverter.ToUInt16(Utility.GetNumericBytes(rawData, index, 2), 0);
                index += 2;

                this.testsuiteId = (SUTControl_TestsuiteId)BitConverter.ToUInt16(Utility.GetNumericBytes(rawData, index, 2), 0);
                index += 2;

                this.commandId = BitConverter.ToUInt16(Utility.GetNumericBytes(rawData, index, 2), 0);
                index += 2;

                this.caseNameLength = BitConverter.ToUInt32(Utility.GetNumericBytes(rawData, index, 4), 0);
                index += 4;

                if (this.caseNameLength > 0)
                {
                    int size = (int)caseNameLength;
                    this.caseName = Encoding.UTF8.GetString(rawData, index, size);
                    index += size;
                }

                this.requestId = BitConverter.ToUInt16(Utility.GetNumericBytes(rawData, index, 2), 0);
                index += 2;

                this.helpMessageLength = BitConverter.ToUInt32(Utility.GetNumericBytes(rawData, index, 4), 0);
                index += 4;

                if (helpMessageLength > 0)
                {
                    int size = (int)helpMessageLength;
                    this.helpMessage = Encoding.UTF8.GetString(rawData, index, size);
                    index += size;
                }
                this.payloadLength = BitConverter.ToUInt32(Utility.GetNumericBytes(rawData, index, 4), 0);
                index += 4;
                if (payloadLength > 0)
                {
                    this.payload = new byte[payloadLength];
                    Array.Copy(rawData, index, payload, 0, payloadLength);
                    index += (int)payloadLength;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion Methods
    }

    /// <summary>
    /// SUT Control Response message from SUT
    /// </summary>
    public class SUT_Control_Response_Message
    {
        #region Fields

        public SUTControl_MessageType messageType;
        public SUTControl_TestsuiteId testsuiteId;
        public ushort commandId;

        public uint caseNameLength;
        public string caseName;

        public ushort requestId;

        public uint resultCode;

        public uint errorMessageLength;
        public string errorMessage;

        public uint payloadLength;
        public byte[] payload;

        #endregion Fields

        #region Methods
        /// <summary>
        /// Constuctor
        /// </summary>
        public SUT_Control_Response_Message()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="testsuiteId"></param>
        /// <param name="commandId"></param>
        /// <param name="requestId"></param>
        /// <param name="errorMessage"></param>
        /// <param name="payload"></param>
        public SUT_Control_Response_Message(SUTControl_TestsuiteId testsuiteId, ushort commandId, string caseName, ushort requestId, uint resultCode, string errorMessage, byte[] payload)
        {
            this.messageType = SUTControl_MessageType.SUT_CONTROL_RESPONSE;
            this.testsuiteId = testsuiteId;
            this.commandId = commandId;

            this.caseNameLength = 0;
            this.caseName = caseName;
            if (!string.IsNullOrEmpty(this.caseName))
            {
                this.caseNameLength = (uint)Encoding.UTF8.GetByteCount(this.caseName);
            }

            this.requestId = requestId;
            this.resultCode = resultCode;

            this.errorMessageLength = 0;
            this.errorMessage = errorMessage;
            if (errorMessage != null)
            {
                this.errorMessageLength = (uint)Encoding.UTF8.GetByteCount(errorMessage);
            }

            this.payloadLength = 0;
            this.payload = payload;
            if (payload != null)
            {
                this.payloadLength = (uint)payload.Length;
            }
        }

        /// <summary>
        /// Encode method
        /// </summary>
        /// <param name="marshaler"></param>
        public byte[] Encode()
        {
            List<byte> bufferList = new List<byte>();
            bufferList.AddRange(Utility.ChangeBytesOrderForNumeric(BitConverter.GetBytes((ushort)this.messageType)));
            bufferList.AddRange(Utility.ChangeBytesOrderForNumeric(BitConverter.GetBytes((ushort)this.testsuiteId)));
            bufferList.AddRange(Utility.ChangeBytesOrderForNumeric(BitConverter.GetBytes((ushort)this.commandId)));

            bufferList.AddRange(Utility.ChangeBytesOrderForNumeric(BitConverter.GetBytes(this.caseNameLength)));
            if (!string.IsNullOrEmpty(this.caseName) && this.caseNameLength > 0)
            {
                bufferList.AddRange(Encoding.UTF8.GetBytes(this.caseName));
            }

            bufferList.AddRange(Utility.ChangeBytesOrderForNumeric(BitConverter.GetBytes((ushort)this.requestId)));
            bufferList.AddRange(Utility.ChangeBytesOrderForNumeric(BitConverter.GetBytes(this.resultCode)));

            bufferList.AddRange(Utility.ChangeBytesOrderForNumeric(BitConverter.GetBytes(this.errorMessageLength)));
            if (errorMessage != null && errorMessage.Length > 0)
            {
                bufferList.AddRange(Encoding.UTF8.GetBytes(errorMessage));
            }

            bufferList.AddRange(Utility.ChangeBytesOrderForNumeric(BitConverter.GetBytes(this.payloadLength)));
            if (payload != null)
            {
                bufferList.AddRange(this.payload);
            }

            return bufferList.ToArray();
        }

        /// <summary>
        /// Decode method
        /// </summary>
        /// <param name="marshaler"></param>
        /// <returns></returns>
        public bool Decode(byte[] rawData, ref int index)
        {
            try
            {
                this.messageType = (SUTControl_MessageType)BitConverter.ToUInt16(Utility.GetNumericBytes(rawData, index, 2), 0);
                index += 2;

                this.testsuiteId = (SUTControl_TestsuiteId)BitConverter.ToUInt16(Utility.GetNumericBytes(rawData, index, 2), 0);
                index += 2;

                this.commandId = BitConverter.ToUInt16(Utility.GetNumericBytes(rawData, index, 2), 0);
                index += 2;

                this.caseNameLength = BitConverter.ToUInt32(Utility.GetNumericBytes(rawData, index, 4), 0);
                index += 4;

                if (this.caseNameLength > 0)
                {
                    int size = (int)caseNameLength;
                    this.caseName = Encoding.UTF8.GetString(rawData, index, size);
                    index += size;
                }

                this.requestId = BitConverter.ToUInt16(Utility.GetNumericBytes(rawData, index, 2), 0);
                index += 2;

                this.resultCode = BitConverter.ToUInt32(Utility.GetNumericBytes(rawData, index, 4), 0);
                index += 4;

                this.errorMessageLength = BitConverter.ToUInt32(Utility.GetNumericBytes(rawData, index, 4), 0);
                index += 4;

                if (errorMessageLength > 0)
                {
                    int size = (int)errorMessageLength;
                    this.errorMessage = Encoding.UTF8.GetString(rawData, index, size);
                    index += size;
                }
                this.payloadLength = BitConverter.ToUInt32(Utility.GetNumericBytes(rawData, index, 4), 0);
                index += 4;

                if (payloadLength > 0)
                {
                    this.payload = new byte[payloadLength];
                    Array.Copy(rawData, index, payload, 0, payloadLength);
                    index += (int)payloadLength;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion Methods
    }

    public class Utility
    {
        public static byte[] ChangeBytesOrderForNumeric(byte[] originalData, bool isLittleEndian = true)
        {
            if (BitConverter.IsLittleEndian != isLittleEndian)
            {
                Array.Reverse(originalData);
            }
            return originalData;
        }

        public static byte[] GetNumericBytes(byte[] rawData, int index, int length, bool isLittleEndian = true)
        {
            byte[] buffer = new byte[length];
            Array.Copy(rawData, index, buffer, 0, length);
            if (BitConverter.IsLittleEndian != isLittleEndian)
            {
                Array.Reverse(buffer);
            }
            return buffer;
        }
    }
    #endregion Message
}