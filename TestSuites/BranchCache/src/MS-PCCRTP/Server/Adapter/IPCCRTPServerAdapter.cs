// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pccrtp
{
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp;

    /// <summary>
    /// Define all methods of the interface IPCCRTPServerAdapter which are used for testing server endpoint.
    /// </summary>>
    public interface IPCCRTPServerAdapter : IAdapter
    {
        /// <summary>
        /// Send a PCCRTP request message to the HTTP/1.1 server (SUT ) and receive a PCCRTP response message from 
        /// the HTTP/1.1 server (SUT ) when testing server endpoint.
        /// </summary>
        /// <param name="httpVersion">Indicates the HTTP version type used.</param>
        /// <param name="isRequestPartialContent">Indicates it is requesting partical content or not.</param>
        /// <param name="fileName">Indicates the URI on the SUT requested by the client.</param>
        /// <returns>Return the PCCRTP response message received.</returns>
        PccrtpResponse SendPccrtpRequestMessage(HttpVersionType httpVersion, bool isRequestPartialContent, string fileName);
    }
}