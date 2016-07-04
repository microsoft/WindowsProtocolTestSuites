// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpemt
{
    public interface ISecureChannel : IDisposable
    {
        /// <summary>
        /// Send bytes through this security channel
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeout"></param>
        void Send(byte[] data);

        /// <summary>
        /// Event be called when received data
        /// </summary>
        event ReceiveData Received;
    }
}
