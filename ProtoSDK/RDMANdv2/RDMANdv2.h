// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#ifndef _RDMA_NDV2_PROVIDER_H
#define _RDMA_NDV2_PROVIDER_H

// IMPORTANT: <ndspi.h> pulls in <winsock2.h> before any later <windows.h>
// inclusion would otherwise drag in the legacy <winsock.h> via <windef.h>.
// All .cpp files in this project must include "RDMANdv2.h" before (or
// instead of) <windows.h> to avoid winsock.h/winsock2.h redefinition
// errors (C2011/C2375/C2059 in winsock2.h and ws2def.h).
#include <initguid.h>
#include <ndspi.h>


// Type definition
typedef HRESULT  (_stdcall *DLLGETCLASSOBJECT)(
    const CLSID &rclsid,
    const IID &rrid,
    void* ppv);
typedef HRESULT (*DLLCANUNLOADNOW)(void);


/// <summary>
/// Registered memory handler (NDSPI v2).
/// </summary>
typedef struct _RdmaMrHandler
{
    IND2MemoryRegion *MemoryRegion;
    void *Buffer;
    unsigned __int32 Length;            // Requested/usable length (reported in SMB Direct buffer descriptor).
    unsigned __int32 RegisteredLength;  // Actual registered length, rounded up to page boundary.
    UINT32 LocalToken;
    UINT32 RemoteToken;
    __int8 MagicNumber;
    struct _RdmaMrHandler *Next;
    struct _RdmaMrHandler *Prev;
} RdmaMrHandler;


// The magic number for RDMA SDK to check the memory is valid.
#define RDMA_MAGIC_NUM 123

// Namespace definition. Two aliases are exposed so that source files using
// either BEGIN_RDMA_NAMESPACE (mirroring the v1 wrapper) or
// BEGIN_RDMA_NDV2_NAMESPACE (used by RDMANdv2Provider.cpp) compile correctly.
#define BEGIN_RDMA_NAMESPACE namespace Microsoft { namespace Protocols { namespace TestTools { namespace StackSdk { namespace FileAccessService { namespace Rdma {
#define END_RDMA_NAMESPACE } } } } } }

#define BEGIN_RDMA_NDV2_NAMESPACE BEGIN_RDMA_NAMESPACE
#define END_RDMA_NDV2_NAMESPACE END_RDMA_NAMESPACE

using namespace System;
using namespace System::Runtime::InteropServices;

BEGIN_RDMA_NAMESPACE

// ==================================================================================
// structures
public ref struct RdmaAdapterInfo
{
    __int32 VendorId;
    __int32 DeviceId;
    unsigned __int32 MaxInboundSge;
    unsigned __int32 MaxInboundRequests;
    unsigned __int32 MaxInboundLength;
    unsigned __int32 MaxOutboundSge;
    unsigned __int32 MaxOutboundRequests;
    unsigned __int32 MaxOutboundLength;
    unsigned __int32 MaxInlineData;
    unsigned __int32 MaxInboundReadLimit;
    unsigned __int32 MaxOutboundReadLimit;
    unsigned __int32 MaxCqEntries;
    unsigned __int32 MaxRegistrationSize;
    unsigned __int32 MaxWindowSize;
    unsigned __int32 LargeRequestThreshold;
    unsigned __int32 MaxCallerData;
    unsigned __int32 MaxCalleeData;
};

// RDMA buffer descriptor
public ref struct RdmaBufferDescriptorV1
{
    unsigned __int64 Offset; // The RDMA provider-specific offset.
    unsigned __int32 Token;  // An RDMA provider-assigned Steering Tag for accessing the registered buffer.
    unsigned __int32 Length; // Size of the registered buffer
};

// Transfer segment
public ref struct RdmaSegment
{
    unsigned __int64 MemoryHandler;
    unsigned __int32 Length;
};

// Rdma notification result
public ref struct RdmaNetworkDirectResult
{
    unsigned __int32 BytesTransferred; // size of data
    HRESULT Status;                    // status of result
};

public ref struct RdmaAddress
{
    array<unsigned char>^ Data;
    int Family;
};

ref struct RdmaProviderInfo;

// Rdma buffer read or write flag
public enum class RdmaOperationReadWriteFlag
{
    Read = ND_OP_FLAG_ALLOW_READ,
    Write = ND_OP_FLAG_ALLOW_WRITE,
    ReadAndWrite = ND_OP_FLAG_ALLOW_READ | ND_OP_FLAG_ALLOW_WRITE
};

// ==================================================================================
// Forward declarations
ref class RdmaAdapter;
ref class RdmaConnector;
ref class RdmaCompletionQueue;
ref class RdmaEndpoint;
ref class RdmaListen;
ref class RdmaMemoryWindow;
ref class RdmaProvider;


// ==================================================================================
// Interfaces

public ref class RdmaMemoryWindow
{
public:
    /// <summary>
    /// Destructor
    /// </summary>
    ~RdmaMemoryWindow();

    /// <summary>
    /// Remote token (R_Key) of the memory window in host byte order, obtained
    /// from IND2MemoryWindow::GetRemoteToken and converted from network byte
    /// order. For NDSPI v2 the token may only become valid after the Bind
    /// completion has been processed, so this property should be queried after
    /// waiting for the bind result.
    /// </summary>
    property unsigned __int32 RemoteToken
    {
        unsigned __int32 get()
        {
            // Mellanox NDSPI v2 returns the remote token in network byte order.
            // Convert it to host byte order so the descriptor matches the rkey
            // the local hardware expects and serializes correctly on the wire.
            unsigned __int32 token = _memoryWindow->GetRemoteToken();
            return ((token & 0xFF000000) >> 24) |
                   ((token & 0x00FF0000) >> 8)  |
                   ((token & 0x0000FF00) << 8)  |
                   ((token & 0x000000FF) << 24);
        }
    }
internal:
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="memoryWindow">Memory window entity with NDSPI v2 type</param>
    /// <param name="invalidateResultId">Invalidate result identifier returned by the adapter</param>
    RdmaMemoryWindow(IND2MemoryWindow *memoryWindow, unsigned __int64 invalidateResultId);

    IND2MemoryWindow *_memoryWindow;
    unsigned __int64 _invalidateResultId;
};


public ref class RdmaEndpoint
{
public:
    ~RdmaEndpoint();

    HRESULT Flush();

    HRESULT Send(array<RdmaSegment^>^ segments, [Out]unsigned __int64% resultHandler);

    HRESULT SendAndInvalidate(
        array<RdmaSegment^>^ segments,
        RdmaBufferDescriptorV1 bufferDescriptor,
        bool reverseMemory,
        [Out]unsigned __int64% resultHandler);

    HRESULT Receive(array<RdmaSegment^>^ segments, [Out]unsigned __int64% resultHandler);

    HRESULT Bind(
        unsigned __int64 memoryHandler,
        RdmaMemoryWindow^ memoryWindow,
        RdmaOperationReadWriteFlag flag,
        bool reverseMemory,
        [Out]RdmaBufferDescriptorV1^% bufferDescriptor,
        [Out]unsigned __int64% resultHandler);

    HRESULT Invalidate(RdmaMemoryWindow^ memoryWindow, [Out]unsigned __int64% resultHandler);

    HRESULT Read(
        array<RdmaSegment^>^ segments,
        RdmaBufferDescriptorV1^ bufferDescriptor,
        bool reverseMemory,
        ULONGLONG offset,
        [Out]unsigned __int64% resultHandler);

    HRESULT Write(
        array<RdmaSegment^>^ segments,
        RdmaBufferDescriptorV1^ bufferDescriptor,
        bool reverseMemory,
        ULONGLONG offset,
        [Out]unsigned __int64% resultHandler);

    static HRESULT ValidRegisteredMemory(unsigned __int64 memoryHandler);

    static HRESULT WriteToMemory(unsigned __int64 memoryHandler, array<System::Byte>^ buffer);

    static HRESULT ReadFromMemory(unsigned __int64 memoryHandler, array<System::Byte>^ buffer);

internal:
    RdmaEndpoint(IND2QueuePair *queuePair);

    HRESULT TransferSegment(
        array<RdmaSegment^>^ segments,
        ND2_SGE** segmentList,
        SIZE_T *segmentListSize);

    /// <summary>
    /// copy memory from source to destination with reversed order
    /// </summary>
    void ReverseMemory(void *destination, const void *source, int length)
    {
        char *destinationInChar = (char *)destination;
        char *sourceInChar = (char *)source;

        for(int i = 0; i < length; ++i)
        {
            destinationInChar[length - 1 - i] = sourceInChar[i];
        }
    }

    IND2QueuePair *_queuePair;
    OVERLAPPED *_pOverlapped;
    unsigned __int64 _nextRequestContext;
};

public ref class RdmaCompletionQueue
{
public:
    ~RdmaCompletionQueue();

    HRESULT Notify();

    unsigned __int64 GetResult(
        [Out]unsigned __int64% resultHandler,
        [Out]RdmaNetworkDirectResult^% requestResult);
internal:
    RdmaCompletionQueue(IND2CompletionQueue *completionQueue);

    IND2CompletionQueue *_completionQueue;
    OVERLAPPED *_pOverlapped;
};

public ref class RdmaConnector
{
public:
    ~RdmaConnector();

    HRESULT CreateEndpoint(
        RdmaCompletionQueue^ completionQueue,
        unsigned __int32 inboundEntries,
        unsigned __int32 outboundEntries,
        unsigned __int32 inboundSegment,
        unsigned __int32 outboundSegment,
        unsigned __int32 inboundReadLimit,
        unsigned __int32 outboundReadLimit,
        [Out]unsigned __int32% maxInlineData,
        [Out]RdmaEndpoint^% endpoint);

    HRESULT Connect(
        RdmaEndpoint^ endpoint,
        String^ ipAddress,
        int port,
        int protocol);

    HRESULT CompleteConnect();

    HRESULT Accept(RdmaEndpoint^ endpoint);

    HRESULT NotifyDisconnect();

    void Disconnect();
internal:
    RdmaConnector(IND2Connector *connector, RdmaAdapter^ adapter);

    IND2Connector *_connector;
    RdmaAdapter^ _adapter;
    unsigned __int32 _inboundReadLimit;
    unsigned __int32 _outboundReadLimit;
    OVERLAPPED *_pOverlapped;
};

public ref class RdmaListen
{
public:
    ~RdmaListen();

    HRESULT GetConnectionRequest(RdmaConnector^ connector);
internal:
    RdmaListen(IND2Listener *listen);

    IND2Listener *_listen;
    OVERLAPPED *_pOverlapped;
};


public ref class RdmaAdapter
{
public:
    ~RdmaAdapter();

    HRESULT Query([Out]RdmaAdapterInfo^% adapterInformation);

    HRESULT CreateCompletionQueue(
        unsigned __int32 entrySize,
        [Out]RdmaCompletionQueue^% completionQueue);

    HRESULT RegisterMemory(
        unsigned __int32 bufferSize,
        [Out]unsigned __int64% memoryHandler);

    HRESULT DeregisterMemory(unsigned __int64 memoryHandler);

    HRESULT CreateMemoryWindow([Out]unsigned __int64 invalidateResult, [Out]RdmaMemoryWindow^% memoryWindow);

    HRESULT CreateConnector([Out]RdmaConnector^% connector);

    HRESULT Listen(int protocol, unsigned __int16 port, [Out]RdmaListen^% listen);

    void ReleaseRegisterMemory();

internal:
    RdmaAdapter(IND2Adapter *adapter);

    HRESULT GetMemoryHandler(unsigned __int64 memoryID, RdmaMrHandler **memoryHandler);

    IND2Adapter *_adapter;
    HANDLE _hAdapterFile;
    RdmaMrHandler *_memoryHandlerList;
    OVERLAPPED *_pOverlapped;
    unsigned __int64 _nextInvalidateId;

    // Local address used to open this adapter. NDSPI v2 requires the
    // connector to be explicitly bound to this address before Connect.
    String^ _localIpAddress;
    short _ipFamily;
};


public ref class RdmaProvider
{
public:
    static HRESULT LoadRdmaProviders([Out]array<RdmaProviderInfo^>^% providers);

    static HRESULT LoadRdmaProvider(System::Guid providerId, String^ path, [Out]RdmaProviderInfo^% provider);

    ~RdmaProvider();

    HRESULT OpenAdapter(String^ ipAddress, short ipFamily, [Out]RdmaAdapter^% adapter);

    HRESULT QueryAddressList([Out]array<RdmaAddress^>^% addressList);
private:

    RdmaProvider(IND2Provider *provider, HMODULE libraryHandler);

    static NTSTATUS LoadProvider(
        WCHAR * path,
        _GUID protocolId,
        IND2Provider **pProvider,
        HMODULE *pLibraryHandler);

    IND2Provider *_provider;
    HMODULE _libraryHandler;
};

// ----------------------------------------------------------------------------------------------
// Structure
public ref struct RdmaProviderInfo
{
    RdmaProvider^ Provider;
    String^ Path;
};

END_RDMA_NAMESPACE

#endif // _RDMA_NDV2_PROVIDER_H
