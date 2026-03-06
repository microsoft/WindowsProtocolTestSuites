// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smbd
{
    public struct RdmaBufferDescriptor
    {
        public UInt64 Offset; // The RDMA provider-specific offset.
        public UInt32 Token; // An RDMA provider-assigned Steering Tag for accessing the registered buffer.
        public UInt32 Length;
    }
}
