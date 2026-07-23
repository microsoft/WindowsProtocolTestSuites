// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

//
// Platform-neutral abstraction over the managed Network Direct (NDSPI) wrappers.
//
// The SMBD stack can talk to either of two C++/CLI wrappers that expose an
// identical public surface in the namespace
// Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rdma:
//   * NDSPI v1  -> ProtoSDK\RDMA           (assembly ...FileAccessService.Rdma)
//   * NDSPI v2  -> ProtoSDK\RDMANdv2       (assembly ...FileAccessService.RdmaNdv2)
//
// Because both wrappers use the same namespace and type names, they are
// referenced side by side via `extern alias` (see RdmaNativeAdapters.cs) and
// adapted to the version-neutral interfaces declared below. SmbdConnectionEndpoint
// chooses the implementation at runtime through RdmaServiceFactory.Create(NdspiVersion),
// so no rebuild is needed to switch versions.
//
// This file contains ONLY version-neutral types and therefore compiles on every
// platform (the concrete adapters and the factory are Windows-only).
//

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smbd
{
    /// <summary>
    /// Network Direct Service Provider Interface version used by the RDMA layer.
    /// </summary>
    public enum NdspiVersion
    {
        /// <summary>
        /// NDSPI v1 interfaces, wrapped by ProtoSDK\RDMA.
        /// </summary>
        NDv1,

        /// <summary>
        /// NDSPI v2 interfaces, wrapped by ProtoSDK\RDMANdv2.
        /// </summary>
        NDv2
    }

    /// <summary>
    /// Version-neutral read/write permission flag for a memory window bind. The
    /// concrete adapter maps these values onto the native RdmaOperationReadWriteFlag.
    /// </summary>
    public enum RdmaReadWriteFlag
    {
        Read = 0,
        Write = 1,
        ReadAndWrite = 2
    }

    /// <summary>
    /// Version-neutral transfer segment (mirrors the native RdmaSegment).
    /// </summary>
    public struct NdSegment
    {
        public ulong MemoryHandler;
        public uint Length;
    }

    /// <summary>
    /// Version-neutral request result (mirrors the native RdmaNetworkDirectResult).
    /// </summary>
    public struct NdResult
    {
        public uint BytesTransferred;
        public int Status;
    }

    /// <summary>
    /// Version-neutral local address (mirrors the native RdmaAddress).
    /// </summary>
    public class NdAddress
    {
        public byte[] Data;
        public int Family;
    }

    /// <summary>
    /// Version-neutral memory window handle.
    /// </summary>
    public interface INativeRdmaMemoryWindow
    {
        /// <summary>
        /// Remote token (R_Key) of the memory window.
        /// </summary>
        uint RemoteToken { get; }
    }

    /// <summary>
    /// Version-neutral provider information (mirrors the native RdmaProviderInfo).
    /// </summary>
    public interface INativeRdmaProviderInfo
    {
        string Path { get; }
        INativeRdmaProvider Provider { get; }
    }

    /// <summary>
    /// Version-neutral RDMA provider.
    /// </summary>
    public interface INativeRdmaProvider
    {
        int OpenAdapter(string ipAddress, short ipFamily, out INativeRdmaAdapter adapter);
        int QueryAddressList(out NdAddress[] addressList);
    }

    /// <summary>
    /// Version-neutral completion queue.
    /// </summary>
    public interface INativeRdmaCompletionQueue
    {
        int Notify();

        /// <summary>
        /// Retrieves the next completion result. Returns the size of the result
        /// (0 means there are no more results); this is NOT an HRESULT.
        /// </summary>
        ulong GetResult(out ulong resultHandler, out NdResult requestResult);
    }

    /// <summary>
    /// Version-neutral listener.
    /// </summary>
    public interface INativeRdmaListen
    {
        int GetConnectionRequest(INativeRdmaConnector connector);
    }

    /// <summary>
    /// Version-neutral endpoint (queue pair).
    /// </summary>
    public interface INativeRdmaEndpoint
    {
        int Send(NdSegment[] segments, out ulong resultHandler);
        int Receive(NdSegment[] segments, out ulong resultHandler);
        int Bind(
            ulong memoryHandler,
            INativeRdmaMemoryWindow memoryWindow,
            RdmaReadWriteFlag flag,
            bool reverseMemory,
            out RdmaBufferDescriptor bufferDescriptor,
            out ulong resultHandler);
        int Invalidate(INativeRdmaMemoryWindow memoryWindow, out ulong resultHandler);
    }

    /// <summary>
    /// Version-neutral connector.
    /// </summary>
    public interface INativeRdmaConnector
    {
        int CreateEndpoint(
            INativeRdmaCompletionQueue completionQueue,
            uint inboundEntries,
            uint outboundEntries,
            uint inboundSegment,
            uint outboundSegment,
            uint inboundReadLimit,
            uint outboundReadLimit,
            out uint maxInlineData,
            out INativeRdmaEndpoint endpoint);
        int Connect(INativeRdmaEndpoint endpoint, string ipAddress, int port, int protocol);
        int CompleteConnect();
        int Accept(INativeRdmaEndpoint endpoint);
        int NotifyDisconnect();
        void Disconnect();
    }

    /// <summary>
    /// Version-neutral adapter.
    /// </summary>
    public interface INativeRdmaAdapter
    {
        int CreateCompletionQueue(uint entrySize, out INativeRdmaCompletionQueue completionQueue);
        int CreateConnector(out INativeRdmaConnector connector);
        int RegisterMemory(uint bufferSize, out ulong memoryHandler);
        int DeregisterMemory(ulong memoryHandler);
        int CreateMemoryWindow(ulong invalidateResult, out INativeRdmaMemoryWindow memoryWindow);
        int Listen(int protocol, ushort port, out INativeRdmaListen listen);
    }

    /// <summary>
    /// Entry point for a selected NDSPI version. Hosts the operations that are
    /// static on the native wrapper (provider enumeration and the global
    /// registered-memory read/write helpers), so a single instance fully
    /// determines which version is in use.
    /// </summary>
    public interface INativeRdmaService
    {
        int LoadProviders(out INativeRdmaProviderInfo[] providers);
        int WriteToMemory(ulong memoryHandler, byte[] buffer);
        int ReadFromMemory(ulong memoryHandler, byte[] buffer);
    }
}
