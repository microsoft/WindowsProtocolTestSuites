// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    internal interface IPacketBuffer
    {
        ushort BufferOffset { get; }
        uint BufferLength { get; }
    }
}
