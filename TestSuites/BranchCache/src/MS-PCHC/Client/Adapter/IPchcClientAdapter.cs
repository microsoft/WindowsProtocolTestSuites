// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pchc
{
    using System;
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc;

    /// <summary>
    /// PCHC adapter interface to test client behaviors.
    /// </summary>
    public interface IPchcClientAdapter : IAdapter
    {
        /// <summary>
        /// Send the corresponding response for the request message.
        /// </summary>
        /// <param name="responseCode">
        /// The ResponseCode specified the hosted cache server is INTERESTED or OK.
        /// 1 is for INTERESTED, 0 is for OK.
        /// </param>
        void SendResponseMessage(int responseCode);

        /// <summary>
        /// Send the Http Status Code 401.
        /// </summary>
        void SendHttpStatusCode401();

        /// <summary>
        /// Receive a InitialOfferMessage from the client.
        /// </summary>
        /// <param name="timeout">Waiting for specified timeout to receive the specified request.</param>
        /// <returns>The InitialOfferMessage request is sent from the client</returns>
        InitialOfferMessage ExpectInitialOfferMessage(TimeSpan timeout);

        /// <summary>
        /// Receive a SegmentInfoMessage from the client.
        /// </summary>
        /// <param name="timeout">Waiting for specified timeout to receive the specified request.</param>
        /// <returns>The SegmentInfoMessage request is sent from the client</returns>
        SegmentInfoMessage ExpectSegmentInfoMessage(TimeSpan timeout);
    }
}

