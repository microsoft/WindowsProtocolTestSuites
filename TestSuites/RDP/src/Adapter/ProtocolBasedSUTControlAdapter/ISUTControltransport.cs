// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.Protocols.TestSuites;

namespace Microsoft.Protocols.TestSuites.Rdp
{
    /// <summary>
    /// Interface for transport of SUT control adapter
    /// </summary>
    public interface ISUTControltransport
    {
        /// <summary>
        /// Connect to the server
        /// </summary>
        /// <param name="timeout">Time out</param>
        /// <param name="remoteEP">RemoteEP</param>
        /// <returns></returns>
        bool Connect(TimeSpan timeout, IPEndPoint remoteEP);

        /// <summary>
        /// Disconnect
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Send SUT Control Request Message
        /// </summary>
        /// <param name="sutControlRequest">Request message</param>
        /// <returns>True if successful, otherwise false</returns>
        bool SendSUTControlRequestMessage(SUT_Control_Request_Message sutControlRequest);

        /// <summary>
        /// Expect a SUT control response message
        /// </summary>
        /// <param name="timeout">Time out</param>
        /// <param name="requestId">RequestId should be in the response message</param>
        /// <returns>SUT Control response message</returns>
        SUT_Control_Response_Message ExpectSUTControlResponseMessage(TimeSpan timeout, ushort requestId);
    }
}
