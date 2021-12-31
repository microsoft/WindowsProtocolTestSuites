// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CPMConnectOut message contains a response to a CPMConnectIn message.
    /// </summary>
    public class CPMConnectOut : IWspOutMessage
    {
        #region Fields
        /// <summary>
        /// A 32-bit integer that indicates whether the server can support 64-bit offsets.
        /// Values greater than or equal to 0x00010000 indicate 64-bit support.
        /// Values less than 0x00010000 indicate 32-bit support.
        /// </summary>
        public uint _serverVersion;

        /// <summary>
        /// The server can send an arbitrary number of arbitrary values, and the client MUST ignore these values if present.
        /// If the server supports version reporting, the size MUST be 4 bytes.
        /// </summary>
        public byte[] _reserved;

        /// <summary>
        /// 32-bit unsigned integer that contains the major version number of the Windows operating system on a server.
        /// If server doesn't supports version reporting then this field MUST be omitted.
        /// </summary>
        public uint? dwWinVerMajor;

        /// <summary>
        /// 32-bit unsigned integer that contains the minor version number of the Windows operating system on a server.
        /// If server doesn't supports version reporting then this field MUST be omitted.
        /// </summary>
        public uint? dwWinVerMinor;

        /// <summary>
        /// 32-bit unsigned integer that contains the National Language Support (NLS) version number of the Windows operating system on a server.
        /// If server doesn't supports version reporting then this field MUST be omitted.
        /// </summary>
        public uint? dwNLSVerMajor;

        /// <summary>
        /// 32-bit unsigned integer that contains the defined National Language Support (NLS) version number of the Windows operating system on a server.
        /// If server doesn't supports version reporting then this field MUST be omitted.
        /// </summary>
        public uint? dwNLSVerMinor;
        #endregion

        public IWspInMessage Request { get; set; }

        public WspMessageHeader Header { get; set; }

        public void FromBytes(WspBuffer buffer)
        {
            var header = new WspMessageHeader();

            header.FromBytes(buffer);

            Header = header;

            var request = (CPMConnectIn)Request;

            var tempBuffer = new WspBuffer();

            request.ToBytes(tempBuffer);

            var requestBytes = tempBuffer.GetBytes();

            var responseBytes = buffer.GetBytes().Skip(buffer.ReadOffset).ToArray();

            bool supportVersion = false;

            if (responseBytes.Length >= 20)
            {
                if (!Enumerable.SequenceEqual(requestBytes.Skip(4).Take(16), responseBytes.Skip(4).Take(16)))
                {
                    supportVersion = true;
                }
            }

            _serverVersion = buffer.ToStruct<uint>();

            if (supportVersion)
            {
                _reserved = new byte[4];

                for (int i = 0; i < 4; i++)
                {
                    _reserved[i] = buffer.ToStruct<byte>();
                }

                dwWinVerMajor = buffer.ToStruct<uint>();

                dwWinVerMinor = buffer.ToStruct<uint>();

                dwNLSVerMajor = buffer.ToStruct<uint>();

                dwNLSVerMinor = buffer.ToStruct<uint>();
            }
            else
            {
                _reserved = buffer.GetBytes().Skip(buffer.ReadOffset).ToArray();

                dwWinVerMajor = null;

                dwWinVerMinor = null;

                dwNLSVerMajor = null;

                dwNLSVerMinor = null;
            }
        }

        public void ToBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
