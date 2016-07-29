// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rdma;
using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smbd
{
    public delegate void SmbdLogEvent(string log);
        

    /// <summary>
    /// Read and Write flag for RDMA registered memory
    /// </summary>
    public enum SmbdBufferReadWrite
    {
        RDMA_READ_PERMISSION_FOR_WRITE_FILE, // remote server only can read registered memory
        RDMA_WRITE_PERMISSION_FOR_READ_FILE, // remote server only can write registered memory
        RDMA_READ_WRITE_PERMISSION_FOR_WRITE_READ_FILE // remote server can read and write registered memory
    }
    /// <summary>
    /// Each operation in RDMA will get the notification to notify the 
    /// result of operation execution
    /// </summary>
    public enum RequestType
    {
        Send, // send operation
        Receive, // Receive operation
        Bind, // bind memory window for RDMA Read/Write
        Read, // RDMA read
        Write, // RDMA Write
        Invalid, // Invalid registed memory window
        None // Not the Type
    }

    /// <summary>
    /// SMBD outstanding request, for example, a Send or Receive request.
    /// </summary>
    public struct SmbdRequest
    {
        public int EntryIndex;
        public UInt64 ResultId;
        public RequestType Type;
    }
    /// <summary>
    /// request result
    /// </summary>
    public struct SmbdRequestResult
    {
        public int EntryIndex;
        public RdmaNetworkDirectResult ResultInfo;
        public UInt64 ResultId;
    }
    /// <summary>
    /// Memory windows structure
    /// </summary>
    public class SmbdMemoryWindow
    {
        public UInt64 MemoryHandlerId;
        public RdmaMemoryWindow RdmaMW;
        public RdmaBufferDescriptorV1 BufferDescriptor;
        public UInt64 InvalidResultId;
        public bool IsValid;
    }

    public struct ReceiveEntry
    {
        public RdmaSegment Segment;
        public bool IsOccupied;
    }
}
