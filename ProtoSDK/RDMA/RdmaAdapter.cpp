// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#include "Rdma.h"

using namespace System;
using namespace System::Runtime::InteropServices;


// Begin SMBD client Namespace
BEGIN_RDMA_NAMESPACE


// =================================================================================
// RMDA Adapter Implementation
RdmaAdapter::~RdmaAdapter()
{
	ReleaseRegisterMemory();
}

/// <summary>
/// Query the capabilities and limits of the Network Direct adapter.
/// </summary>
/// <param name="adapterInformation">Network Direct adapter's information </param>
HRESULT RdmaAdapter::Query([Out]RdmaAdapterInfo^% adapterInformation)
{
	ND_ADAPTER_INFO info;
	SIZE_T size = sizeof(info);
	HRESULT result = _adapter->Query(1, &info, &size);
	if(result != ND_SUCCESS)
	{
		return result;
	}

	adapterInformation = gcnew RdmaAdapterInfo();
	// copy value
	adapterInformation->VendorId				= info.VendorId;
    adapterInformation->DeviceId				= info.DeviceId;
    adapterInformation->MaxInboundSge			= (unsigned __int32)info.MaxInboundSge;
    adapterInformation->MaxInboundRequests		= (unsigned __int32)info.MaxInboundRequests;
    adapterInformation->MaxInboundLength		= (unsigned __int32)info.MaxInboundLength;
    adapterInformation->MaxOutboundSge			= (unsigned __int32)info.MaxOutboundSge;
    adapterInformation->MaxOutboundRequests		= (unsigned __int32)info.MaxOutboundRequests;
    adapterInformation->MaxOutboundLength		= (unsigned __int32)info.MaxOutboundLength;
    adapterInformation->MaxInlineData			= (unsigned __int32)info.MaxInlineData;
    adapterInformation->MaxInboundReadLimit		= (unsigned __int32)info.MaxInboundReadLimit;
    adapterInformation->MaxOutboundReadLimit	= (unsigned __int32)info.MaxOutboundReadLimit;
    adapterInformation->MaxCqEntries			= (unsigned __int32)info.MaxCqEntries;
    adapterInformation->MaxRegistrationSize		= (unsigned __int32)info.MaxRegistrationSize;
    adapterInformation->MaxWindowSize			= (unsigned __int32)info.MaxWindowSize;
    adapterInformation->LargeRequestThreshold	= (unsigned __int32)info.LargeRequestThreshold;
    adapterInformation->MaxCallerData			= (unsigned __int32)info.MaxCallerData;
    adapterInformation->MaxCalleeData			= (unsigned __int32)info.MaxCalleeData;
	return ND_SUCCESS;
}

/// <summary>
/// Allocates a completion queue.
/// </summary>
/// <param name="entrySize">The number of completion queue entries to support. </param>
/// <param name="completionQueue">completion queue</param>
HRESULT RdmaAdapter::CreateCompletionQueue(
	unsigned __int32 entrySize,
	[Out]RdmaCompletionQueue^% completionQueue)
{
	INDCompletionQueue *pCompletionQueue;
	HRESULT result = _adapter->CreateCompletionQueue((SIZE_T)entrySize, &pCompletionQueue);
	if(result != ND_SUCCESS)
	{
		return result;
	}

	completionQueue = gcnew RdmaCompletionQueue(pCompletionQueue);
	return ND_SUCCESS;
}

/// <summary>
/// register memory for send and receive
/// </summary>
/// <param name="bufferSize">size of buffer to register</param>
/// <param name="memoryHandler">handler of registered memory</param>
HRESULT RdmaAdapter::RegisterMemory(unsigned __int32 bufferSize, [Out]unsigned __int64% memoryHandler)
{
	ND_MR_HANDLE hMr;
	void *buffer = new char[(int)bufferSize];
	HRESULT result = _adapter->RegisterMemory(buffer, (SIZE_T)bufferSize, _pOverlapped, &hMr);
	if( result == ND_PENDING )
    {
        SIZE_T BytesRet;
        result = _adapter->GetOverlappedResult( _pOverlapped, &BytesRet, TRUE );
    }
	if( result != ND_SUCCESS)
    {
		return result;
    }

	// register memory success
	RdmaMrHandler *newElem = new RdmaMrHandler;
	newElem->MemoryHandler = hMr;
	newElem->MagicNumber = RDMA_MAGIC_NUM;
	newElem->Buffer = buffer;
	newElem->Length = bufferSize;

	// insert at the beginning
	newElem->Prev = _memoryHandlerList;
	newElem->Next = _memoryHandlerList->Next;
	if(_memoryHandlerList->Next != NULL)
	{
		_memoryHandlerList->Next->Prev = newElem;
	}
	_memoryHandlerList->Next = newElem;

	memoryHandler = (unsigned __int64)newElem;
	return ND_SUCCESS;
}

/// <summary>
/// deregister memory for send and receive
/// </summary>
/// <param name="memoryHandler">handler of registered memory</param>
HRESULT RdmaAdapter::DeregisterMemory(unsigned __int64 memoryHandler)
{
	RdmaMrHandler *newElem;
	HRESULT result = GetMemoryHandler(memoryHandler, &newElem);
	// check magic number
	if(result != ND_SUCCESS)
	{
		return result;
	}
	result = _adapter->DeregisterMemory(newElem->MemoryHandler, _pOverlapped);
	if( result == ND_PENDING )
    {
        SIZE_T BytesRet;
        result = _adapter->GetOverlappedResult(_pOverlapped, &BytesRet, TRUE );
    }
	delete newElem->Buffer;

	// remove from list
	newElem->Prev->Next = newElem->Next;
	if(newElem->Next != NULL)
	{
		newElem->Next->Prev = newElem->Prev;
	}
	delete newElem;
	return result;
}

/// <summary>
/// Creates an uninitialized memory window
/// </summary>
/// <param name="invalidateResult">result id which is used when the remote peer sends a SendAndInvalidate request</param>
/// <param name="memoryWindow">memory window handler</param>
HRESULT RdmaAdapter::CreateMemoryWindow([Out]unsigned __int64 invalidateResult, [Out]RdmaMemoryWindow^% memoryWindow)
{
	ND_RESULT *ndResult = new ND_RESULT;

	INDMemoryWindow *pMw;
	HRESULT result = _adapter->CreateMemoryWindow(ndResult, &pMw);

	if(result != ND_SUCCESS)
	{
		delete ndResult;
		return result;
	}

	invalidateResult = (unsigned __int64)pMw;
	memoryWindow = gcnew RdmaMemoryWindow(pMw);
	return result;
}

/// <summary>
/// create connector
/// </summary>
/// <param name="connector">created connector</param>
HRESULT RdmaAdapter::CreateConnector([Out]RdmaConnector^% connector)
{
	INDConnector *nativeConnector;
	HRESULT result = _adapter->CreateConnector(&nativeConnector);
	if(result != ND_SUCCESS)
	{
		return result;
	}

	connector = gcnew RdmaConnector(nativeConnector);
	return result;
}

/// <summary>
/// Creates a Listen object that listens for incoming connection requests
/// </summary>
/// <param name="protocol">An IANA Internet Protocol number. Use the AF_INET and AF_INET6 constants to set this parameter.</param>
/// <param name="port">port number</param>
/// <param name="listen">A entity used to listen for connection requests.</param>
HRESULT RdmaAdapter::Listen(int protocol, unsigned __int16 port, [Out]RdmaListen^% listen)
{
	INDListen *pListen;
	HRESULT result = _adapter->Listen(
		0,
		protocol,
		htons(port),
		NULL,
		&pListen);
	if(result != ND_SUCCESS)
	{
		return result;
	}

	listen = gcnew RdmaListen(pListen);
	return 0;
}

/// <summary>
/// Release all registerd memory
/// </summary>
void RdmaAdapter::ReleaseRegisterMemory()
{
	while(_memoryHandlerList->Next != NULL)
	{
		DeregisterMemory((unsigned __int64)_memoryHandlerList->Next);
	}
}

// ============================================================================
// private


RdmaAdapter::RdmaAdapter(INDAdapter *adapter)
{
	_adapter = adapter;
	_memoryHandlerList = new RdmaMrHandler;
	memset(_memoryHandlerList, 0, sizeof(RdmaMrHandler));
	_memoryHandlerList->Prev = NULL;
	_memoryHandlerList->Next = NULL;

	_pOverlapped = new OVERLAPPED;
	_pOverlapped->hEvent = CreateEvent( NULL, FALSE, FALSE, NULL );
}

HRESULT RdmaAdapter::GetMemoryHandler(unsigned __int64 memoryID, RdmaMrHandler **memoryHandler)
{
	*memoryHandler = (RdmaMrHandler *)memoryID;
	if((*memoryHandler)->MagicNumber == RDMA_MAGIC_NUM)
	{
		return ND_SUCCESS;
	}

	return ND_INVALID_PARAMETER;
}

END_RDMA_NAMESPACE