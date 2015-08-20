// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the interfaces for client.<para/>
    /// provides interfaces of target.<para/>
    /// such as UdpClient.
    /// </summary>
    internal interface IUdpClient : ITarget, IStartable, ISourceSend
    {
    }
}
