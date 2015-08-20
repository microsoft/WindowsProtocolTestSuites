// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the interfaces for source transport device.<para/>
    /// provides methods to send to target that is stored and no need to specify.<para/>
    /// such as TcpClient, Stream and NetbiosClient.
    /// </summary>
    internal interface ISource : ISourceSend, ISourceReceive
    {
    }
}
