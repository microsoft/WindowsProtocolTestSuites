// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the interfaces for server<para/>
    /// provides interface of startable, target and special target.<para/>
    /// such as TcpServer.
    /// </summary>
    internal interface ITcpServer : INetbiosServer, ISslServer
    {
    }
}
