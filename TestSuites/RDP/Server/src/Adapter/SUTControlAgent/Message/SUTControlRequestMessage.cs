// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Protocols.TestSuites.Rdp.SUTControlAgent.Message
{
    /// <summary>
    /// SUT Control Request message to SUT
    /// </summary>
    public class SUTControlRequestMessage
    {
        #region Fields
        public SUTControl_MessageType messageType;
        public SUTControl_TestsuiteId testsuiteId;
        public uint caseNameLength;
        public string caseName;
        public ushort commandId;
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
        public SUTControlRequestMessage()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="testsuiteId"></param>
        /// <param name="commandId"></param>
        /// <param name="caseName"></param>
        /// <param name="requestId"></param>
        /// <param name="helpMessage"></param>
        /// <param name="payload"></param>
        public SUTControlRequestMessage(SUTControl_TestsuiteId testsuiteId, ushort commandId, string caseName, ushort requestId, string helpMessage, byte[] payload)
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
        public byte[] Encode()
        {
            List<byte> bufferList = new List<byte>();
            bufferList.AddRange(SUTControlUtility.ChangeBytesOrderForNumeric(BitConverter.GetBytes((ushort)this.messageType)));
            bufferList.AddRange(SUTControlUtility.ChangeBytesOrderForNumeric(BitConverter.GetBytes((ushort)this.testsuiteId)));
            bufferList.AddRange(SUTControlUtility.ChangeBytesOrderForNumeric(BitConverter.GetBytes((ushort)this.commandId)));

            bufferList.AddRange(SUTControlUtility.ChangeBytesOrderForNumeric(BitConverter.GetBytes(this.caseNameLength)));
            if (!string.IsNullOrEmpty(this.caseName) && this.caseNameLength > 0)
            {
                bufferList.AddRange(Encoding.UTF8.GetBytes(this.caseName));
            }

            bufferList.AddRange(SUTControlUtility.ChangeBytesOrderForNumeric(BitConverter.GetBytes((ushort)this.requestId)));

            bufferList.AddRange(SUTControlUtility.ChangeBytesOrderForNumeric(BitConverter.GetBytes(this.helpMessageLength)));
            if (helpMessage != null && helpMessage.Length > 0)
            {
                bufferList.AddRange(Encoding.UTF8.GetBytes(helpMessage));
            }

            bufferList.AddRange(SUTControlUtility.ChangeBytesOrderForNumeric(BitConverter.GetBytes(this.payloadLength)));
            if (payload != null)
            {
                bufferList.AddRange(this.payload);
            }

            return bufferList.ToArray();
        }

        /// <summary>
        /// Decode method
        /// </summary>
        /// <param name="rawData">Decode source binary data</param>
		/// <param name="index">Decode position</param>
        /// <returns>Is decode success</returns>
        public bool Decode(byte[] rawData, ref int index)
        {
            try
            {
                this.messageType = (SUTControl_MessageType)BitConverter.ToUInt16(SUTControlUtility.GetNumericBytes(rawData, index, 2), 0);
                index += 2;

                this.testsuiteId = (SUTControl_TestsuiteId)BitConverter.ToUInt16(SUTControlUtility.GetNumericBytes(rawData, index, 2), 0);
                index += 2;

                this.commandId = BitConverter.ToUInt16(SUTControlUtility.GetNumericBytes(rawData, index, 2), 0);
                index += 2;

                this.caseNameLength = BitConverter.ToUInt32(SUTControlUtility.GetNumericBytes(rawData, index, 4), 0);
                index += 4;

                if (this.caseNameLength > 0)
                {
                    int size = (int)caseNameLength;
                    this.caseName = Encoding.UTF8.GetString(rawData, index, size);
                    index += size;
                }

                this.requestId = BitConverter.ToUInt16(SUTControlUtility.GetNumericBytes(rawData, index, 2), 0);
                index += 2;

                this.helpMessageLength = BitConverter.ToUInt32(SUTControlUtility.GetNumericBytes(rawData, index, 4), 0);
                index += 4;

                if (helpMessageLength > 0)
                {
                    int size = (int)helpMessageLength;
                    this.helpMessage = Encoding.UTF8.GetString(rawData, index, size);
                    index += size;
                }
                this.payloadLength = BitConverter.ToUInt32(SUTControlUtility.GetNumericBytes(rawData, index, 4), 0);
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
}
