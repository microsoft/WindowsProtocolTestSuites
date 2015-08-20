// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the interfaces for stream.<para/>
    /// provides interfaces of connectable and target.<para/>
    /// such as StreamTransport.
    /// </summary>
    internal interface IStream : ISource, IConnectable, IDisconnectable
    {
    }
}
