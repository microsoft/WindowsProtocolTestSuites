// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

//
// Windows-only concrete adapters that bridge the version-neutral interfaces in
// RdmaAbstraction.cs to the two native NDSPI wrappers. Both wrappers expose the
// identical public surface in namespace
// Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rdma, so they are
// referenced side by side with `extern alias` (RdmaNdv1 -> ProtoSDK\RDMA,
// RdmaNdv2 -> ProtoSDK\RDMANdv2; see Smbd.csproj) and aliased below as V1 / V2.
//
// The whole file (including the extern alias directives) is compiled only on
// Windows, because the native references exist only on Windows.
//

#if WINDOWS
extern alias RdmaNdv1;
extern alias RdmaNdv2;
using V1 = RdmaNdv1::Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rdma;
using V2 = RdmaNdv2::Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rdma;
using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smbd
{
    /// <summary>
    /// Creates the RDMA service for the requested NDSPI version. The returned
    /// service, and every object reachable from it, wraps exactly one native
    /// version, so the choice made here flows through the whole connection.
    /// </summary>
    public static class RdmaServiceFactory
    {
        public static INativeRdmaService Create(NdspiVersion version)
        {
            switch (version)
            {
                case NdspiVersion.NDv1:
                    return new Ndv1RdmaService();
                case NdspiVersion.NDv2:
                    return new Ndv2RdmaService();
                default:
                    throw new NotSupportedException(string.Format("Unsupported NDSPI version: {0}", version));
            }
        }
    }

    // ===========================================================================
    // NDSPI v1 adapters (ProtoSDK\RDMA)
    // ===========================================================================

    internal sealed class Ndv1RdmaService : INativeRdmaService
    {
        public int LoadProviders(out INativeRdmaProviderInfo[] providers)
        {
            V1.RdmaProviderInfo[] list;
            int hr = V1.RdmaProvider.LoadRdmaProviders(out list);
            if (list == null)
            {
                providers = null;
                return hr;
            }
            providers = new INativeRdmaProviderInfo[list.Length];
            for (int i = 0; i < list.Length; i++)
            {
                providers[i] = (list[i] == null) ? null : new Ndv1ProviderInfo(list[i]);
            }
            return hr;
        }

        public int WriteToMemory(ulong memoryHandler, byte[] buffer)
        {
            return V1.RdmaEndpoint.WriteToMemory(memoryHandler, buffer);
        }

        public int ReadFromMemory(ulong memoryHandler, byte[] buffer)
        {
            return V1.RdmaEndpoint.ReadFromMemory(memoryHandler, buffer);
        }
    }

    internal sealed class Ndv1ProviderInfo : INativeRdmaProviderInfo
    {
        private readonly V1.RdmaProviderInfo info;
        public Ndv1ProviderInfo(V1.RdmaProviderInfo info) { this.info = info; }
        public string Path { get { return info.Path; } }
        public INativeRdmaProvider Provider
        {
            get { return info.Provider == null ? null : new Ndv1Provider(info.Provider); }
        }
    }

    internal sealed class Ndv1Provider : INativeRdmaProvider
    {
        private readonly V1.RdmaProvider provider;
        public Ndv1Provider(V1.RdmaProvider provider) { this.provider = provider; }

        public int OpenAdapter(string ipAddress, short ipFamily, out INativeRdmaAdapter adapter)
        {
            V1.RdmaAdapter a;
            int hr = provider.OpenAdapter(ipAddress, ipFamily, out a);
            adapter = (a == null) ? null : new Ndv1Adapter(a);
            return hr;
        }

        public int QueryAddressList(out NdAddress[] addressList)
        {
            V1.RdmaAddress[] list;
            int hr = provider.QueryAddressList(out list);
            if (list == null)
            {
                addressList = null;
                return hr;
            }
            addressList = new NdAddress[list.Length];
            for (int i = 0; i < list.Length; i++)
            {
                addressList[i] = (list[i] == null)
                    ? null
                    : new NdAddress { Data = list[i].Data, Family = list[i].Family };
            }
            return hr;
        }
    }

    internal sealed class Ndv1Adapter : INativeRdmaAdapter
    {
        private readonly V1.RdmaAdapter adapter;
        public Ndv1Adapter(V1.RdmaAdapter adapter) { this.adapter = adapter; }

        public int CreateCompletionQueue(uint entrySize, out INativeRdmaCompletionQueue completionQueue)
        {
            V1.RdmaCompletionQueue q;
            int hr = adapter.CreateCompletionQueue(entrySize, out q);
            completionQueue = (q == null) ? null : new Ndv1CompletionQueue(q);
            return hr;
        }

        public int CreateConnector(out INativeRdmaConnector connector)
        {
            V1.RdmaConnector c;
            int hr = adapter.CreateConnector(out c);
            connector = (c == null) ? null : new Ndv1Connector(c);
            return hr;
        }

        public int RegisterMemory(uint bufferSize, out ulong memoryHandler)
        {
            return adapter.RegisterMemory(bufferSize, out memoryHandler);
        }

        public int DeregisterMemory(ulong memoryHandler)
        {
            return adapter.DeregisterMemory(memoryHandler);
        }

        public int CreateMemoryWindow(ulong invalidateResult, out INativeRdmaMemoryWindow memoryWindow)
        {
            V1.RdmaMemoryWindow w;
            int hr = adapter.CreateMemoryWindow(invalidateResult, out w);
            memoryWindow = (w == null) ? null : new Ndv1MemoryWindow(w);
            return hr;
        }

        public int Listen(int protocol, ushort port, out INativeRdmaListen listen)
        {
            V1.RdmaListen l;
            int hr = adapter.Listen(protocol, port, out l);
            listen = (l == null) ? null : new Ndv1Listen(l);
            return hr;
        }
    }

    internal sealed class Ndv1Connector : INativeRdmaConnector
    {
        internal readonly V1.RdmaConnector Inner;
        public Ndv1Connector(V1.RdmaConnector connector) { Inner = connector; }

        public int CreateEndpoint(
            INativeRdmaCompletionQueue completionQueue,
            uint inboundEntries,
            uint outboundEntries,
            uint inboundSegment,
            uint outboundSegment,
            uint inboundReadLimit,
            uint outboundReadLimit,
            out uint maxInlineData,
            out INativeRdmaEndpoint endpoint)
        {
            V1.RdmaEndpoint ep;
            int hr = Inner.CreateEndpoint(
                ((Ndv1CompletionQueue)completionQueue).Inner,
                inboundEntries, outboundEntries, inboundSegment, outboundSegment,
                inboundReadLimit, outboundReadLimit, out maxInlineData, out ep);
            endpoint = (ep == null) ? null : new Ndv1Endpoint(ep);
            return hr;
        }

        public int Connect(INativeRdmaEndpoint endpoint, string ipAddress, int port, int protocol)
        {
            return Inner.Connect(((Ndv1Endpoint)endpoint).Inner, ipAddress, port, protocol);
        }

        public int CompleteConnect() { return Inner.CompleteConnect(); }

        public int Accept(INativeRdmaEndpoint endpoint)
        {
            return Inner.Accept(((Ndv1Endpoint)endpoint).Inner);
        }

        public int NotifyDisconnect() { return Inner.NotifyDisconnect(); }

        public void Disconnect() { Inner.Disconnect(); }
    }

    internal sealed class Ndv1Endpoint : INativeRdmaEndpoint
    {
        internal readonly V1.RdmaEndpoint Inner;
        public Ndv1Endpoint(V1.RdmaEndpoint endpoint) { Inner = endpoint; }

        public int Send(NdSegment[] segments, out ulong resultHandler)
        {
            return Inner.Send(ToNative(segments), out resultHandler);
        }

        public int Receive(NdSegment[] segments, out ulong resultHandler)
        {
            return Inner.Receive(ToNative(segments), out resultHandler);
        }

        public int Bind(
            ulong memoryHandler,
            INativeRdmaMemoryWindow memoryWindow,
            RdmaReadWriteFlag flag,
            bool reverseMemory,
            out RdmaBufferDescriptor bufferDescriptor,
            out ulong resultHandler)
        {
            V1.RdmaBufferDescriptorV1 nativeDescriptor;
            int hr = Inner.Bind(
                memoryHandler,
                ((Ndv1MemoryWindow)memoryWindow).Inner,
                ToNativeFlag(flag),
                reverseMemory,
                out nativeDescriptor,
                out resultHandler);
            bufferDescriptor = new RdmaBufferDescriptor();
            if (nativeDescriptor != null)
            {
                bufferDescriptor.Offset = nativeDescriptor.Offset;
                bufferDescriptor.Token = nativeDescriptor.Token;
                bufferDescriptor.Length = nativeDescriptor.Length;
            }
            return hr;
        }

        public int Invalidate(INativeRdmaMemoryWindow memoryWindow, out ulong resultHandler)
        {
            return Inner.Invalidate(((Ndv1MemoryWindow)memoryWindow).Inner, out resultHandler);
        }

        private static V1.RdmaSegment[] ToNative(NdSegment[] segments)
        {
            if (segments == null)
            {
                return null;
            }
            var native = new V1.RdmaSegment[segments.Length];
            for (int i = 0; i < segments.Length; i++)
            {
                native[i] = new V1.RdmaSegment();
                native[i].MemoryHandler = segments[i].MemoryHandler;
                native[i].Length = segments[i].Length;
            }
            return native;
        }

        private static V1.RdmaOperationReadWriteFlag ToNativeFlag(RdmaReadWriteFlag flag)
        {
            switch (flag)
            {
                case RdmaReadWriteFlag.Read: return V1.RdmaOperationReadWriteFlag.Read;
                case RdmaReadWriteFlag.Write: return V1.RdmaOperationReadWriteFlag.Write;
                default: return V1.RdmaOperationReadWriteFlag.ReadAndWrite;
            }
        }
    }

    internal sealed class Ndv1CompletionQueue : INativeRdmaCompletionQueue
    {
        internal readonly V1.RdmaCompletionQueue Inner;
        public Ndv1CompletionQueue(V1.RdmaCompletionQueue completionQueue) { Inner = completionQueue; }

        public int Notify() { return Inner.Notify(); }

        public ulong GetResult(out ulong resultHandler, out NdResult requestResult)
        {
            V1.RdmaNetworkDirectResult r;
            ulong size = Inner.GetResult(out resultHandler, out r);
            requestResult = (r == null)
                ? default(NdResult)
                : new NdResult { BytesTransferred = r.BytesTransferred, Status = r.Status };
            return size;
        }
    }

    internal sealed class Ndv1Listen : INativeRdmaListen
    {
        private readonly V1.RdmaListen listen;
        public Ndv1Listen(V1.RdmaListen listen) { this.listen = listen; }

        public int GetConnectionRequest(INativeRdmaConnector connector)
        {
            return listen.GetConnectionRequest(((Ndv1Connector)connector).Inner);
        }
    }

    internal sealed class Ndv1MemoryWindow : INativeRdmaMemoryWindow
    {
        internal readonly V1.RdmaMemoryWindow Inner;
        public Ndv1MemoryWindow(V1.RdmaMemoryWindow memoryWindow) { Inner = memoryWindow; }
        public uint RemoteToken { get { return Inner.RemoteToken; } }
    }

    // ===========================================================================
    // NDSPI v2 adapters (ProtoSDK\RDMANdv2)
    // ===========================================================================

    internal sealed class Ndv2RdmaService : INativeRdmaService
    {
        public int LoadProviders(out INativeRdmaProviderInfo[] providers)
        {
            V2.RdmaProviderInfo[] list;
            int hr = V2.RdmaProvider.LoadRdmaProviders(out list);
            if (list == null)
            {
                providers = null;
                return hr;
            }
            providers = new INativeRdmaProviderInfo[list.Length];
            for (int i = 0; i < list.Length; i++)
            {
                providers[i] = (list[i] == null) ? null : new Ndv2ProviderInfo(list[i]);
            }
            return hr;
        }

        public int WriteToMemory(ulong memoryHandler, byte[] buffer)
        {
            return V2.RdmaEndpoint.WriteToMemory(memoryHandler, buffer);
        }

        public int ReadFromMemory(ulong memoryHandler, byte[] buffer)
        {
            return V2.RdmaEndpoint.ReadFromMemory(memoryHandler, buffer);
        }
    }

    internal sealed class Ndv2ProviderInfo : INativeRdmaProviderInfo
    {
        private readonly V2.RdmaProviderInfo info;
        public Ndv2ProviderInfo(V2.RdmaProviderInfo info) { this.info = info; }
        public string Path { get { return info.Path; } }
        public INativeRdmaProvider Provider
        {
            get { return info.Provider == null ? null : new Ndv2Provider(info.Provider); }
        }
    }

    internal sealed class Ndv2Provider : INativeRdmaProvider
    {
        private readonly V2.RdmaProvider provider;
        public Ndv2Provider(V2.RdmaProvider provider) { this.provider = provider; }

        public int OpenAdapter(string ipAddress, short ipFamily, out INativeRdmaAdapter adapter)
        {
            V2.RdmaAdapter a;
            int hr = provider.OpenAdapter(ipAddress, ipFamily, out a);
            adapter = (a == null) ? null : new Ndv2Adapter(a);
            return hr;
        }

        public int QueryAddressList(out NdAddress[] addressList)
        {
            V2.RdmaAddress[] list;
            int hr = provider.QueryAddressList(out list);
            if (list == null)
            {
                addressList = null;
                return hr;
            }
            addressList = new NdAddress[list.Length];
            for (int i = 0; i < list.Length; i++)
            {
                addressList[i] = (list[i] == null)
                    ? null
                    : new NdAddress { Data = list[i].Data, Family = list[i].Family };
            }
            return hr;
        }
    }

    internal sealed class Ndv2Adapter : INativeRdmaAdapter
    {
        private readonly V2.RdmaAdapter adapter;
        public Ndv2Adapter(V2.RdmaAdapter adapter) { this.adapter = adapter; }

        public int CreateCompletionQueue(uint entrySize, out INativeRdmaCompletionQueue completionQueue)
        {
            V2.RdmaCompletionQueue q;
            int hr = adapter.CreateCompletionQueue(entrySize, out q);
            completionQueue = (q == null) ? null : new Ndv2CompletionQueue(q);
            return hr;
        }

        public int CreateConnector(out INativeRdmaConnector connector)
        {
            V2.RdmaConnector c;
            int hr = adapter.CreateConnector(out c);
            connector = (c == null) ? null : new Ndv2Connector(c);
            return hr;
        }

        public int RegisterMemory(uint bufferSize, out ulong memoryHandler)
        {
            return adapter.RegisterMemory(bufferSize, out memoryHandler);
        }

        public int DeregisterMemory(ulong memoryHandler)
        {
            return adapter.DeregisterMemory(memoryHandler);
        }

        public int CreateMemoryWindow(ulong invalidateResult, out INativeRdmaMemoryWindow memoryWindow)
        {
            V2.RdmaMemoryWindow w;
            int hr = adapter.CreateMemoryWindow(invalidateResult, out w);
            memoryWindow = (w == null) ? null : new Ndv2MemoryWindow(w);
            return hr;
        }

        public int Listen(int protocol, ushort port, out INativeRdmaListen listen)
        {
            V2.RdmaListen l;
            int hr = adapter.Listen(protocol, port, out l);
            listen = (l == null) ? null : new Ndv2Listen(l);
            return hr;
        }
    }

    internal sealed class Ndv2Connector : INativeRdmaConnector
    {
        internal readonly V2.RdmaConnector Inner;
        public Ndv2Connector(V2.RdmaConnector connector) { Inner = connector; }

        public int CreateEndpoint(
            INativeRdmaCompletionQueue completionQueue,
            uint inboundEntries,
            uint outboundEntries,
            uint inboundSegment,
            uint outboundSegment,
            uint inboundReadLimit,
            uint outboundReadLimit,
            out uint maxInlineData,
            out INativeRdmaEndpoint endpoint)
        {
            V2.RdmaEndpoint ep;
            int hr = Inner.CreateEndpoint(
                ((Ndv2CompletionQueue)completionQueue).Inner,
                inboundEntries, outboundEntries, inboundSegment, outboundSegment,
                inboundReadLimit, outboundReadLimit, out maxInlineData, out ep);
            endpoint = (ep == null) ? null : new Ndv2Endpoint(ep);
            return hr;
        }

        public int Connect(INativeRdmaEndpoint endpoint, string ipAddress, int port, int protocol)
        {
            return Inner.Connect(((Ndv2Endpoint)endpoint).Inner, ipAddress, port, protocol);
        }

        public int CompleteConnect() { return Inner.CompleteConnect(); }

        public int Accept(INativeRdmaEndpoint endpoint)
        {
            return Inner.Accept(((Ndv2Endpoint)endpoint).Inner);
        }

        public int NotifyDisconnect() { return Inner.NotifyDisconnect(); }

        public void Disconnect() { Inner.Disconnect(); }
    }

    internal sealed class Ndv2Endpoint : INativeRdmaEndpoint
    {
        internal readonly V2.RdmaEndpoint Inner;
        public Ndv2Endpoint(V2.RdmaEndpoint endpoint) { Inner = endpoint; }

        public int Send(NdSegment[] segments, out ulong resultHandler)
        {
            return Inner.Send(ToNative(segments), out resultHandler);
        }

        public int Receive(NdSegment[] segments, out ulong resultHandler)
        {
            return Inner.Receive(ToNative(segments), out resultHandler);
        }

        public int Bind(
            ulong memoryHandler,
            INativeRdmaMemoryWindow memoryWindow,
            RdmaReadWriteFlag flag,
            bool reverseMemory,
            out RdmaBufferDescriptor bufferDescriptor,
            out ulong resultHandler)
        {
            V2.RdmaBufferDescriptorV1 nativeDescriptor;
            int hr = Inner.Bind(
                memoryHandler,
                ((Ndv2MemoryWindow)memoryWindow).Inner,
                ToNativeFlag(flag),
                reverseMemory,
                out nativeDescriptor,
                out resultHandler);
            bufferDescriptor = new RdmaBufferDescriptor();
            if (nativeDescriptor != null)
            {
                bufferDescriptor.Offset = nativeDescriptor.Offset;
                bufferDescriptor.Token = nativeDescriptor.Token;
                bufferDescriptor.Length = nativeDescriptor.Length;
            }
            return hr;
        }

        public int Invalidate(INativeRdmaMemoryWindow memoryWindow, out ulong resultHandler)
        {
            return Inner.Invalidate(((Ndv2MemoryWindow)memoryWindow).Inner, out resultHandler);
        }

        private static V2.RdmaSegment[] ToNative(NdSegment[] segments)
        {
            if (segments == null)
            {
                return null;
            }
            var native = new V2.RdmaSegment[segments.Length];
            for (int i = 0; i < segments.Length; i++)
            {
                native[i] = new V2.RdmaSegment();
                native[i].MemoryHandler = segments[i].MemoryHandler;
                native[i].Length = segments[i].Length;
            }
            return native;
        }

        private static V2.RdmaOperationReadWriteFlag ToNativeFlag(RdmaReadWriteFlag flag)
        {
            switch (flag)
            {
                case RdmaReadWriteFlag.Read: return V2.RdmaOperationReadWriteFlag.Read;
                case RdmaReadWriteFlag.Write: return V2.RdmaOperationReadWriteFlag.Write;
                default: return V2.RdmaOperationReadWriteFlag.ReadAndWrite;
            }
        }
    }

    internal sealed class Ndv2CompletionQueue : INativeRdmaCompletionQueue
    {
        internal readonly V2.RdmaCompletionQueue Inner;
        public Ndv2CompletionQueue(V2.RdmaCompletionQueue completionQueue) { Inner = completionQueue; }

        public int Notify() { return Inner.Notify(); }

        public ulong GetResult(out ulong resultHandler, out NdResult requestResult)
        {
            V2.RdmaNetworkDirectResult r;
            ulong size = Inner.GetResult(out resultHandler, out r);
            requestResult = (r == null)
                ? default(NdResult)
                : new NdResult { BytesTransferred = r.BytesTransferred, Status = r.Status };
            return size;
        }
    }

    internal sealed class Ndv2Listen : INativeRdmaListen
    {
        private readonly V2.RdmaListen listen;
        public Ndv2Listen(V2.RdmaListen listen) { this.listen = listen; }

        public int GetConnectionRequest(INativeRdmaConnector connector)
        {
            return listen.GetConnectionRequest(((Ndv2Connector)connector).Inner);
        }
    }

    internal sealed class Ndv2MemoryWindow : INativeRdmaMemoryWindow
    {
        internal readonly V2.RdmaMemoryWindow Inner;
        public Ndv2MemoryWindow(V2.RdmaMemoryWindow memoryWindow) { Inner = memoryWindow; }
        public uint RemoteToken { get { return Inner.RemoteToken; } }
    }
}
#endif
