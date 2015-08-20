// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the interfaces for client transport device<para/>
    /// provides interfaces of connectable and source.<para/>
    /// such as TcpClient.
    /// </summary>
    internal interface ITcpClient: INetbiosClient, ISslClient
    {
    }
}
