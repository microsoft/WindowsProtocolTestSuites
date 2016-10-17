// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc
{
    /// <summary>
    /// Response message are sent in response to the request message: INITIAL_OFFER_MESSAGE, SEGMENT_INFO_MESSAGE.
    /// </summary>
    public struct RESPONSE_MESSAGE
    {
        /// <summary>
        /// The transport adds the TransportHeader in front of any response protocol message.
        /// </summary>
        public TRANSPORT_HEADER TransportHeader;

        /// <summary>
        /// Each response message contains a response code.
        /// </summary>
        public RESPONSE_CODE ResponseCode;
    }
}
