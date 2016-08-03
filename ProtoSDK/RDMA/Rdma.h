// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#ifndef _RDMA_PROVIDER_H
#define _RDMA_PROVIDER_H

#include <ndspi.h>


// Type definition
typedef HRESULT  (_stdcall *DLLGETCLASSOBJECT)(
	const CLSID &rclsid,
	const IID &rrid,
	void* ppv);
typedef HRESULT (*DLLCANUNLOADNOW)(void);


/// <summary>
/// Registered memory handler
/// </summary>
typedef struct _RdmaMrHandler
{
	ND_MR_HANDLE MemoryHandler;
	void *Buffer;
	unsigned __int32 Length;
	__int8 MagicNumber;
	struct _RdmaMrHandler *Next;
	struct _RdmaMrHandler *Prev;
} RdmaMrHandler;


// The magic number for RDMA SDK to check the memory is valid.
#define RDMA_MAGIC_NUM 123

// Namespace definition
#define BEGIN_RDMA_NAMESPACE namespace Microsoft { namespace Protocols { namespace TestTools { namespace StackSdk { namespace FileAccessService { namespace Rdma { 
#define END_RDMA_NAMESPACE } } } } } }

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
	unsigned __int32 Token; // An RDMA provider-assigned Steering Tag for accessing the registered buffer.
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
	HRESULT Status; // status of result
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
// Interfaces

public ref class RdmaMemoryWindow
{
public:
	/// <summary>
	/// Deconstructor
	/// </summary>
	~RdmaMemoryWindow();
internal:
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="memoryWindow">Memory window entity with NDSPI type</param>
	RdmaMemoryWindow(INDMemoryWindow *memoryWindow);
	INDMemoryWindow *_memoryWindow;
};


public ref class RdmaEndpoint
{
public:
	/// <summary>
	/// Deconstructor
	/// </summary>
	~RdmaEndpoint();

	/// <summary>
	/// Flush out all outstanding requests in the inbound and outbound completion queues,
	/// which will cancel all the requests.
	/// </summary>
	HRESULT Flush();

	/// <summary>
	/// Raise send request to the completion queue.
	/// </summary>
	/// <param name="segments">segments, which contain the buffer and registered memory handler</param>
	/// <param name="resultHandler">result handler</param>
	HRESULT Send(array<RdmaSegment^>^ segments, [Out]unsigned __int64% resultHandler);

	/// <summary>
	/// Raise send request to the completion queue and invalid peer's memory window
	/// </summary>
	/// <param name="segments">segments, which contain the buffer and registered memory handler</param>
	/// <param name="bufferDescriptor">remote buffer descriptor</param>
	/// <param name="reverseMemory">little-endian and big-endian has been reversed in bufferDescriptor</param>
	/// <param name="resultHandler">result handler</param>
	HRESULT SendAndInvalidate(
		array<RdmaSegment^>^ segments,
		RdmaBufferDescriptorV1 bufferDescriptor,
		bool reverseMemory,
		[Out]unsigned __int64% resultHandler);

	/// <summary>
	/// Raise receive request to the completion queue
	/// </summary>
	/// <param name="segments">segments, which contain the buffer and registered memory handler</param>
	/// <param name="resultHandler">result handler</param>
	HRESULT Receive(array<RdmaSegment^>^ segments, [Out]unsigned __int64% resultHandler);

	/// <summary>
	/// Binds a memory window to a buffer that is within the registered memory.
	/// </summary>
	/// <param name="memoryHandler">A handle to the registered memory</param>
	/// <param name="memoryWindow">The memory window to bind to the registered memory. </param>
	/// <param name="flag">Read, Write or Read and Write flag</param>
	/// <param name="reverseMemory">if it is true, little-endian and big-endian will be reversed in bufferDescriptor</param>
	/// <param name="bufferDescriptor">buffer descriptor</param>
	/// <param name="resultHandler">result handler</param>
	HRESULT Bind(
		unsigned __int64 memoryHandler,
		RdmaMemoryWindow^ memoryWindow,
		RdmaOperationReadWriteFlag flag,
		bool reverseMemory,
		[Out]RdmaBufferDescriptorV1^% bufferDescriptor,
		[Out]unsigned __int64% resultHandler);

	/// <summary>
	/// Invalidates a local memory window.
	/// </summary>
	/// <param name="memoryWindow">Memory window</param>
	/// <param name="resultHandler">result handler</param>
	HRESULT Invalidate(RdmaMemoryWindow^ memoryWindow, [Out]unsigned __int64% resultHandler);

	/// <summary>
	/// Initiates an RDMA Read request
	/// </summary>
	/// <param name="segments">segments, which contain the buffer and registered memory handler</param>
	/// <param name="bufferDescriptor">buffer descriptor</param>
	/// <param name="reverseMemory">little-endian and big-endian has been reversed in bufferDescriptor</param>
	/// <param name="offset">Zero-based offset into the memory window at which the read operation begins</param>
	/// <param name="resultHandler">result handler</param>
	HRESULT Read(
		array<RdmaSegment^>^ segments,
		RdmaBufferDescriptorV1^ bufferDescriptor,
		bool reverseMemory,
		ULONGLONG offset,
		[Out]unsigned __int64% resultHandler);

	/// <summary>
	/// Initiates an RDMA Write request
	/// </summary>
	/// <param name="segments">segments, which contain the buffer and registered memory handler</param>
	/// <param name="bufferDescriptor">buffer descriptor</param>
	/// <param name="reverseMemory">little-endian and big-endian has been reversed in bufferDescriptor</param>
	/// <param name="offset">Zero-based offset into the memory window at which the read operation begins</param>
	/// <param name="resultHandler">result handler</param>
	HRESULT Write(
		array<RdmaSegment^>^ segments,
		RdmaBufferDescriptorV1^ bufferDescriptor,
		bool reverseMemory,
		ULONGLONG offset,
		[Out]unsigned __int64% resultHandler);

	
	/// <summary>
	/// Valid registered memory
	/// </summary>
	static HRESULT ValidRegisteredMemory(unsigned __int64 memoryHandler);

	/// <summary>
	/// Write data to the memory which memory handler points to.
	/// </summary>
	static HRESULT WriteToMemory(unsigned __int64 memoryHandler, array<System::Byte>^ buffer);

	/// <summary>
	/// Read data from the memory which memory handler points to.
	/// </summary>
	static HRESULT ReadFromMemory(unsigned __int64 memoryHandler, array<System::Byte>^ buffer);
internal:

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="endpoint">Endpoint entity with NDSPI type</param>
	RdmaEndpoint(INDEndpoint *endpoint);

	HRESULT TransferSegment(
		array<RdmaSegment^>^ segments,
		ND_SGE** segmentList,
		SIZE_T *segmentListSize);

	/// <summary>
	/// copy memory from source to destination with reversed order
	/// </summary>
	/// <param name="destination">base address of destination memory</param>
	/// <param name="source">base address of source memory</param>
	/// <param name="length">length of bytes to copy</param>
	void ReverseMemory(void *destination, const void *source, int length)
	{
		char *destinationInChar = (char *)destination;
		char *sourceInChar = (char *)source;

		for(int i = 0; i < length; ++i)
		{
			destinationInChar[length - 1 - i] = sourceInChar[i];
		}
	}

	INDEndpoint *_endpoint;
	OVERLAPPED *_pOverlapped;
};

public ref class RdmaCompletionQueue
{
public:
	~RdmaCompletionQueue();

	/// <summary>
	/// Requests notification for errors and completions
	/// </summary>
	HRESULT Notify();

	/// <summary>
	/// Retrieves the requested completion results from the completion queue.
	/// </summary>
	/// <param name="resultHandler">handler of result</param>
	/// <param name="requestResult">result information of send/receive/read/write/bind/invalid request</param>
	unsigned __int64 GetResult(
		[Out]unsigned __int64% resultHandler,
		[Out]RdmaNetworkDirectResult^% requestResult);
internal:
	RdmaCompletionQueue(INDCompletionQueue *completionQueue);

	INDCompletionQueue *_completionQueue;
	OVERLAPPED *_pOverlapped;
};

public ref class RdmaConnector
{
public:
	~RdmaConnector();

	/// <summary>
	/// Creates an endpoint to use for data transfers.
	/// </summary>
	/// <param name="completionQueue">The interface is used to queue Receive request results.
	/// The interface is used to queue Send, SendAndInvalidate, Bind, Invalidate, Read, and Write request results.</param>
	/// <param name="inboundEntries">The maximum number of outstanding Receive requests</param>
	/// <param name="outboundEntries">The maximum number of outstanding Send, SendAndInvalidate, Bind, Invalidate, Read, and Write requests.</param>
	/// <param name="inboundSegment">The maximum number of scatter/gather entries supported for Receive requests.</param>
	/// <param name="outboundSegment">The maximum number of scatter/gather entries supported for Send, Read, and Write requests.</param>
	/// <param name="inboundReadLimit">The maximum inbound read limit for the local Network Direct adapter. This value can be zero if you do not support Read requests from the peer.</param>
	/// <param name="outboundReadLimit">The maximum outbound read limit for the local Network Direct adapter. This value can be zero if you are not issuing Read requests to the peer.</param>
	/// <param name="maxInlineData">The maximum amount of inline data, in bytes, that the endpoint supports.</param>
	/// <param name="endpoint">endpoint in NDSPI type</param>
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

	/// <summary>
	/// Connects the endpoint to a listening peer.
	/// </summary>
	/// <param name="endpoint">An INDEndpoint interface that identifies the endpoint to use for the connection. 
	///  The endpoint must have been created using this instance of the Connector object.</param>
	/// <param name="ipAddress">ip address</param>
	/// <param name="port">port</param>
	/// <param name="protocol">An IANA Internet Protocol number</param>
	HRESULT Connect(
		RdmaEndpoint^ endpoint,
		String^ ipAddress,
		int port,
		int protocol);

	/// <summary>
	/// Completes the connection request initiated by a previous 
	/// </summary>
	HRESULT CompleteConnect();

	/// <summary>
	/// Accept arrival network connection
	/// </summary>
	/// <param name="endpoint">the endpoint to connect to the peer's endpoint. </param>
	HRESULT Accept(RdmaEndpoint^ endpoint);

	/// <summary>
	/// Notifies the caller that an established connection was disconnected.
	/// </summary>
	HRESULT NotifyDisconnect();

	/// <summary>
	/// Disconnects the endpoint from the peer.
	/// </summary>
	void Disconnect();
internal:
	RdmaConnector(INDConnector *connector);
	INDConnector *_connector;

	OVERLAPPED *_pOverlapped;
};

public ref class RdmaListen
{
public:
	~RdmaListen();

	/// <summary>
	/// Retrieves the handle for a pending connection request.
	/// </summary>
	HRESULT GetConnectionRequest(RdmaConnector^ connector);
internal:
	RdmaListen(INDListen *listen);
	INDListen *_listen;

	OVERLAPPED *_pOverlapped;
};


public ref class RdmaAdapter
{
public:
	~RdmaAdapter();

	/// <summary>
	/// Query the capabilities and limits of the Network Direct adapter.
	/// </summary>
	/// <param name="adapterInformation">Network Direct adapter's information </param>
	HRESULT Query([Out]RdmaAdapterInfo^% adapterInformation);

	/// <summary>
	/// Allocates a completion queue.
	/// </summary>
	/// <param name="entrySize">The number of completion queue entries to support. </param>
	/// <param name="completionQueue">completion queue</param>
	HRESULT CreateCompletionQueue(
		unsigned __int32 entrySize,
		[Out]RdmaCompletionQueue^% completionQueue);

	/// <summary>
	/// register memory for send and receive
	/// </summary>
	/// <param name="bufferSize">size of buffer to register </param>
	/// <param name="memoryHandler">handler of registered memory</param>
	HRESULT RegisterMemory(
		unsigned __int32 bufferSize,
		[Out]unsigned __int64% memoryHandler);

	/// <summary>
	/// deregister memory for send and receive
	/// </summary>
	/// <param name="memoryHandler">handler of registered memory</param>
	HRESULT DeregisterMemory(unsigned __int64 memoryHandler);

	/// <summary>
	/// Creates an uninitialized memory window
	/// </summary>
	/// <param name="invalidateResult">result id which is used when the remote peer sends a SendAndInvalidate request</param>
	/// <param name="memoryWindow">memory window handler</param>
	HRESULT CreateMemoryWindow([Out]unsigned __int64 invalidateResult, [Out]RdmaMemoryWindow^% memoryWindow);

	/// <summary>
	/// create connector
	/// </summary>
	/// <param name="connector">created connector</param>
	HRESULT CreateConnector([Out]RdmaConnector^% connector);

	/// <summary>
	/// Creates a Listen object that listens for incoming connection requests
	/// </summary>
	/// <param name="protocol">An IANA Internet Protocol number. Use the AF_INET and AF_INET6 constants to set this parameter.</param>
	/// <param name="port">port number</param>
	/// <param name="listen">A entity used to listen for connection requests.</param>
	HRESULT Listen(int protocol, unsigned __int16 port, [Out]RdmaListen^% listen);

	/// <summary>
	/// Release all registerd memory
	/// </summary>
	void ReleaseRegisterMemory();

internal:
	RdmaAdapter(INDAdapter *adapter);

	HRESULT GetMemoryHandler(unsigned __int64 memoryID, RdmaMrHandler **memoryHandler);

	INDAdapter *_adapter;

	RdmaMrHandler *_memoryHandlerList;

	OVERLAPPED *_pOverlapped;
};


public ref class RdmaProvider
{
public:

	/// <summary>
	/// Load providers
	/// </summary>
	static HRESULT LoadRdmaProviders([Out]array<RdmaProviderInfo^>^% providers);

	/// <summary>
	/// Load provider
	/// </summary>
	static HRESULT LoadRdmaProvider(System::Guid providerId, String^ path, [Out]RdmaProviderInfo^% provider);

	/// <summary>
	/// Deconstructor, release library resource and provider
	/// </summary>
	~RdmaProvider();

	/// <summary>
	/// Open adapter with specific address
	/// </summary>
	/// <param name="ipAddress">IP Address</param>
	/// <param name="ipFamily">IP Family</param>
	/// <param name="adapter">opened adapter</param>
	HRESULT OpenAdapter(String^ ipAddress, short ipFamily, [Out]RdmaAdapter^% adapter);

	/// <summary>
	/// Retrieves a list of local addresses that the provider supports.
	/// </summary>
	HRESULT QueryAddressList([Out]array<RdmaAddress^>^% addressList);
private:

	RdmaProvider(INDProvider *provider, HMODULE libraryHandler);

	/// <summary>
	/// Load provider
	/// </summary>
	static NTSTATUS LoadProvider(
		WCHAR * path,
		_GUID protocolId,
		INDProvider **pProvider,
		HMODULE *pLibraryHandler);

	INDProvider *_provider;
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

#endif
