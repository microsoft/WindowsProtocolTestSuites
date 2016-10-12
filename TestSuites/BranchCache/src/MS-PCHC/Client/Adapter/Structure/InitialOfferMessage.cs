// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pchc
{
    /// <summary>
    /// An INITIAL_OFFER_MESSAGE is the first message a client sends to the hosted cache. 
    /// The INITIAL_OFFER_MESSAGE is a request message that notifies the hosted cache 
    /// of new content available on the client.
    /// </summary>
    public struct InitialOfferMessage
    {
        /// <summary>
        /// MessageHeader (8 bytes): A MESSAGE_HEADER structure (section 2.2.1.1), 
        /// with the Type field set to 0x0001.
        /// </summary>
        public MessageHeader MsgHeader;

        /// <summary>
        /// ConnectionInformation (8 bytes): A CONNECTION_INFORMATION structure (section 2.2.1.2).
        /// </summary>
        public ConnectionInformation ConnectionInfo;

        /// <summary>
        /// Hash (variable): The Hash field is a binary byte array that contains the HoHoDk of the 
        /// segment that was partly or fully downloaded by the client.
        /// </summary>
        public byte[] Hash;
    }
}
