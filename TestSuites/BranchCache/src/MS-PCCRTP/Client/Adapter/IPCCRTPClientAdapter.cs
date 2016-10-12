// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pccrtp
{
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp;

    /// <summary>
    /// Define all methods of the interface IPCCRTPClientAdapter which are used for testing client endpoint.
    /// </summary>
    public interface IPCCRTPClientAdapter : IAdapter
    {
        /// <summary>
        /// Receive a PCCRTP request message from the HTTP/1.1 client (SUT) when testing client endpoint.
        /// </summary>
        /// <returns>Return the HTTP request received.</returns>
        PccrtpRequest ReceivePccrtpRequestMessage();

        /// <summary>
        /// Generate and send a PCCRTP response message to the HTTP/1.1 client (SUT) when testing client endpoint.
        /// </summary>
        /// <param name="requestFileFullPath">Indicate that local full path of the requested file on the driver computer.
        /// </param>
        void SendPccrtpResponseMessage(string requestFileFullPath);
    }
}