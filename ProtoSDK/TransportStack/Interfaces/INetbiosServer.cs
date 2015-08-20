// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the interfaces for netbios server transport device<para/>
    /// such as NetbiosServer and TcpServer.
    /// </summary>
    internal interface INetbiosServer : ISpecialTarget, ITargetReceive, IStartable, IAcceptable, IDisconnectable
    {
    }
}
