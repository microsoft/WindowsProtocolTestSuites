// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the interfaces for server.<para/>
    /// provides interfaces of startable, special target and target.<para/>
    /// such as UdpServer.
    /// </summary>
    internal interface IUdpServer : ITargetReceive, ITargetSend, ILocalTarget, IStartable
    {
    }
}
