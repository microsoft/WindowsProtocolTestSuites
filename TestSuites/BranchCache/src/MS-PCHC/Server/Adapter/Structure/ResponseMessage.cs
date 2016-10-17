// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pchc
{
    /// <summary>
    /// Response message
    /// </summary>
    public struct ResponseMessage
    {
        /// <summary>
        /// Transportheader (4 bytes): A transport header (section 2.2.2.1).
        /// </summary>
        public TransportHeader TransportHeader;

        /// <summary>
        /// ResponseCode (1 byte): A code that indicates the server response to the client request message.
        /// OK 0x00:The server has sufficient information to download content from the client.
        /// INTERESTED 0x01:The server needs the range of block hashes from the client before it can 
        /// download content from the client.
        /// </summary>
        public ResponseCode ResponseCode;
    }
}
