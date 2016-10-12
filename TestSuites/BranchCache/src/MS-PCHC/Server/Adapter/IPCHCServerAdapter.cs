// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pchc
{
    using Microsoft.Protocols.TestTools;

    /// <summary>
    /// PCHC adapter interface to test server behaviors.
    /// </summary>
    public interface IPchcServerAdapter : IAdapter
    {
        /// <summary>
        /// This action is used to send InitialOfferMessage request to and receive the
        /// correspondent Response Message from the hosted cache server.
        /// </summary>
        /// <param name="paddingInMessageHeader">An array formed by bytes for message header padding</param>
        /// <param name="pccrrPort">
        /// The port on which MS-PCCRR server-role will be listening if the hosted cache server initiates the 
        /// Peer Content Caching and Retrieval: Retrieval Protocol (PCCRR) framework [MS-PCCRR] as a client-role peer to
        /// retrieve the missing blocks from the test suite.
        /// </param>
        /// <param name="paddingInConnectionInformation">
        /// An array formed by bytes for connection information padding
        /// </param>
        /// <param name="hash">Include segment id</param>
        /// <returns>Return the response message of the InitialOfferMessage</returns>
        ResponseMessage SendInitialOfferMessage(
            byte[] paddingInMessageHeader,
            int pccrrPort,
            byte[] paddingInConnectionInformation,
            byte[] hash);

        /// <summary>
        /// This action is used to send SEGMENT_INFO_MESSAGE request to and receive the
        /// correspondent Response Message from the hosted cache server.
        /// </summary>
        /// <param name="paddingInMessageHeader">An array formed by bytes for message header padding</param>
        /// <param name="pccrrPort">The port on which MS-PCCRR server-role will be listening. </param>
        /// <param name="paddingInConnectionInformation">An array formed by bytes for connection information padding</param>
        /// <param name="segmentInformation">The segment information.</param>
        /// <returns>Return the response message of the SegmentInfoMessage</returns>
        ResponseMessage SendSegmentInfoMessage(
            byte[] paddingInMessageHeader,
            int pccrrPort,
            byte[] paddingInConnectionInformation,
            SegmentInformation segmentInformation);
    }
}
