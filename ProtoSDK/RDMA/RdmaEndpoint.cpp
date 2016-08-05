// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#include "Rdma.h"

using namespace System;
using namespace System::Runtime::InteropServices;


// Begin SMBD client Namespace
BEGIN_RDMA_NAMESPACE
/// <summary>
/// Deconstructor
/// </summary>
RdmaEndpoint::~RdmaEndpoint()
{
	delete _pOverlapped;
}

/// <summary>
/// Flush out all outstanding requests in the inbound and outbound completion queues,
/// which will cancel all the requests.
/// </summary>
HRESULT RdmaEndpoint::Flush()
{
	return _endpoint->Flush();
}

/// <summary>
/// Raise send request to the completion queue.
/// </summary>
/// <param name="segments">segments, which contain the buffer and registered memory handler</param>
/// <param name="resultHandler">result handler</param>
HRESULT RdmaEndpoint::Send(array<RdmaSegment^>^ segments, [Out]unsigned __int64% resultHandler)
{
	HRESULT result;

	if(segments->Length == 0)
	{
		return ND_SUCCESS;
	}

	ND_RESULT *ndResult = new ND_RESULT;
	resultHandler = (unsigned __int64)ndResult;
	ND_SGE* segmentList;
	SIZE_T segmentListSize = 0;


	result = TransferSegment(segments, &segmentList, &segmentListSize);
	if(result != ND_SUCCESS)
	{
		return result;
	}
	result = _endpoint->Send(ndResult, segmentList, segmentListSize, 0);
	delete[] segmentList; // release memory

	return result;
}

/// <summary>
/// Raise send request to the completion queue and invalid peer's memory window
/// </summary>
/// <param name="segments">segments, which contain the buffer and registered memory handler</param>
/// <param name="bufferDescriptor">remote buffer descriptor</param>
/// <param name="reverseMemory">little-endian and big-endian has been reversed in bufferDescriptor</param>
/// <param name="resultHandler">result handler</param>
HRESULT RdmaEndpoint::SendAndInvalidate(
	array<RdmaSegment^>^ segments,
	RdmaBufferDescriptorV1 bufferDescriptor,
	bool reverseMemory,
	[Out]unsigned __int64% resultHandler)
{
	HRESULT result;

	if(segments->Length == 0)
	{
		return ND_SUCCESS;
	}

	ND_RESULT *ndResult = new ND_RESULT;
	resultHandler = (unsigned __int64)ndResult;
	ND_SGE* segmentList;
	SIZE_T segmentListSize = 0;


	result = TransferSegment(segments, &segmentList, &segmentListSize);
	if(result != ND_SUCCESS)
	{
		return result;
	}

	ND_MW_DESCRIPTOR mwDescriptor; // memory window descriptor
	unsigned __int64 tmpLength = bufferDescriptor.Length;
	unsigned __int64 tmpBase = bufferDescriptor.Offset;
	unsigned __int32 tmpToken = bufferDescriptor.Token;

	if(reverseMemory)
	{
		// reverse memory
		ReverseMemory(&mwDescriptor.Base, &tmpBase, sizeof(tmpBase));
		ReverseMemory(&mwDescriptor.Token,  &tmpToken, sizeof( tmpToken));
		ReverseMemory(&mwDescriptor.Length, &tmpLength, sizeof( tmpLength));
	}
	result = _endpoint->SendAndInvalidate(ndResult, segmentList, segmentListSize, &mwDescriptor, 0);
	delete[] segmentList;

	return result;
}

/// <summary>
/// Raise receive request to the completion queue
/// </summary>
/// <param name="segments">segments, which contain the buffer and registered memory handler</param>
/// <param name="resultHandler">result handler</param>
HRESULT RdmaEndpoint::Receive(array<RdmaSegment^>^ segments, [Out]unsigned __int64% resultHandler)
{
	HRESULT result;

	if(segments->Length == 0)
	{
		return ND_SUCCESS;
	}

	ND_RESULT *ndResult = new ND_RESULT;
	resultHandler = (unsigned __int64)ndResult;
	ND_SGE* segmentList;
	SIZE_T segmentListSize = 0;


	result = TransferSegment(segments, &segmentList, &segmentListSize);
	if(result != ND_SUCCESS)
	{
		return result;
	}

	result = _endpoint->Receive(ndResult, segmentList, segmentListSize);
	delete[] segmentList;
	return result;
}

/// <summary>
/// Binds a memory window to a buffer that is within the registered memory.
/// </summary>
/// <param name="memoryHandler">A handle to the registered memory</param>
/// <param name="memoryWindow">The memory window to bind to the registered memory. </param>
/// <param name="flag">Read, Write or Read and Write flag</param>
/// <param name="reverseMemory">if it is true, little-endian and big-endian will be reversed in bufferDescriptor</param>
/// <param name="bufferDescriptor">buffer descriptor</param>
/// <param name="resultHandler">result handler</param>
HRESULT RdmaEndpoint::Bind(
	unsigned __int64 memoryHandler,
	RdmaMemoryWindow^ memoryWindow,
	RdmaOperationReadWriteFlag flag,
	bool reverseMemory,
	[Out]RdmaBufferDescriptorV1^% bufferDescriptor,
	[Out]unsigned __int64% resultHandler)
{
	RdmaMrHandler *mrHandler = (RdmaMrHandler *)memoryHandler;
	if(mrHandler->MagicNumber != RDMA_MAGIC_NUM)
	{
		return ND_INVALID_PARAMETER;
	}

	ND_RESULT *ndResult = new ND_RESULT;
	resultHandler = (unsigned __int64)ndResult;

	ND_MW_DESCRIPTOR mwDescriptor;
	HRESULT result = _endpoint->Bind(
		ndResult,
		mrHandler->MemoryHandler,
		memoryWindow->_memoryWindow,
		mrHandler->Buffer,
		(SIZE_T)mrHandler->Length,
		(DWORD)flag,
		&mwDescriptor);
	if(result != ND_SUCCESS)
	{
		return result;
	}

	unsigned __int64 tmpLength;
	unsigned __int64 tmpBase;
	unsigned __int32 tmpToken;

	if(reverseMemory)
	{
		ReverseMemory(&tmpBase, &mwDescriptor.Base, sizeof(mwDescriptor.Base));
		ReverseMemory(&tmpToken,  &mwDescriptor.Token, sizeof( mwDescriptor.Token));
		ReverseMemory(&tmpLength, &mwDescriptor.Length, sizeof( mwDescriptor.Length));
	}
	bufferDescriptor = gcnew RdmaBufferDescriptorV1();
	bufferDescriptor->Offset = tmpBase;
	bufferDescriptor->Token = tmpToken;
	bufferDescriptor->Length = (unsigned __int32)tmpLength;

	return result;
}

/// <summary>
/// Invalidates a local memory window.
/// </summary>
/// <param name="memoryWindow">Memory window</param>
/// <param name="resultHandler">result handler</param>
HRESULT RdmaEndpoint::Invalidate(RdmaMemoryWindow^ memoryWindow, [Out]unsigned __int64% resultHandler)
{
	ND_RESULT *ndResult = new ND_RESULT;
	resultHandler = (unsigned __int64)ndResult;

	return _endpoint->Invalidate(ndResult, memoryWindow->_memoryWindow, 0);
}

/// <summary>
/// Initiates an RDMA Read request
/// </summary>
/// <param name="segments">segments, which contain the buffer and registered memory handler</param>
/// <param name="bufferDescriptor">buffer descriptor</param>
/// <param name="reverseMemory">little-endian and big-endian has been reversed in bufferDescriptor</param>
/// <param name="offset">Zero-based offset into the memory window at which the read operation begins</param>
/// <param name="resultHandler">result handler</param>
HRESULT RdmaEndpoint::Read(
	array<RdmaSegment^>^ segments,
	RdmaBufferDescriptorV1^ bufferDescriptor,
	bool reverseMemory,
	ULONGLONG offset,
	[Out]unsigned __int64% resultHandler)
{
	HRESULT result;

	if(segments->Length == 0)
	{
		return ND_SUCCESS;
	}

	ND_RESULT *ndResult = new ND_RESULT;
	resultHandler = (unsigned __int64)ndResult;
	ND_SGE* segmentList;
	SIZE_T segmentListSize = 0;


	result = TransferSegment(segments, &segmentList, &segmentListSize);
	if(result != ND_SUCCESS)
	{
		return result;
	}

	ND_MW_DESCRIPTOR mwDescriptor;
	unsigned __int64 tmpLength = bufferDescriptor->Length;
	unsigned __int64 tmpBase = bufferDescriptor->Offset;
	unsigned __int32 tmpToken = bufferDescriptor->Token;
	if(reverseMemory)
	{
		// reverse memory
		ReverseMemory(&mwDescriptor.Base, &tmpBase, sizeof(tmpBase));
		ReverseMemory(&mwDescriptor.Token,  &tmpToken, sizeof( tmpToken));
		ReverseMemory(&mwDescriptor.Length, &tmpLength, sizeof( tmpLength));
	}

	result = _endpoint->Read(
		ndResult,
		segmentList, 
		segmentListSize,
		&mwDescriptor,
		offset,
		0);
	delete[] segmentList;

	return result;
}

/// <summary>
/// Initiates an RDMA Write request
/// </summary>
/// <param name="segments">segments, which contain the buffer and registered memory handler</param>
/// <param name="bufferDescriptor">buffer descriptor</param>
/// <param name="reverseMemory">little-endian and big-endian has been reversed in bufferDescriptor</param>
/// <param name="offset">Zero-based offset into the memory window at which the read operation begins</param>
/// <param name="resultHandler">result handler</param>
HRESULT RdmaEndpoint::Write(
	array<RdmaSegment^>^ segments,
	RdmaBufferDescriptorV1^ bufferDescriptor,
	bool reverseMemory,
	ULONGLONG offset,
	[Out]unsigned __int64% resultHandler)
{
	HRESULT result;

	if(segments->Length == 0)
	{
		return ND_SUCCESS;
	}

	ND_RESULT *ndResult = new ND_RESULT;
	resultHandler = (unsigned __int64)ndResult;
	ND_SGE* segmentList;
	SIZE_T segmentListSize = 0;


	result = TransferSegment(segments, &segmentList, &segmentListSize);
	if(result != ND_SUCCESS)
	{
		return result;
	}

	ND_MW_DESCRIPTOR mwDescriptor;
	unsigned __int64 tmpLength = bufferDescriptor->Length;
	unsigned __int64 tmpBase = bufferDescriptor->Offset;
	unsigned __int32 tmpToken = bufferDescriptor->Token;
	if(reverseMemory)
	{
		// reverse memory
		ReverseMemory(&mwDescriptor.Base, &tmpBase, sizeof(tmpBase));
		ReverseMemory(&mwDescriptor.Token,  &tmpToken, sizeof( tmpToken));
		ReverseMemory(&mwDescriptor.Length, &tmpLength, sizeof( tmpLength));
	}

	result = _endpoint->Write(ndResult, segmentList, segmentListSize, &mwDescriptor, offset, 0);
	delete[] segmentList;

	return result;
}

/// <summary>
/// Valid registered memory
/// </summary>
HRESULT RdmaEndpoint::ValidRegisteredMemory(unsigned __int64 memoryHandler)
{
	RdmaMrHandler *newElem = (RdmaMrHandler *)memoryHandler;
	if(newElem->MagicNumber != RDMA_MAGIC_NUM)
	{
		return ND_INVALID_PARAMETER;
	}

	return ND_SUCCESS;
}

//void RdmaEndpoint::GetBufferAddress(array<Byte> ^buffer, [Out]unsigned __int64% address)
//{
//	pin_ptr<System::Byte> pin_buf = &buffer[0];
//	address = (unsigned __int64)pin_buf;
//}

/// <summary>
/// Write data to the memory which memory handler points to.
/// </summary>
HRESULT RdmaEndpoint::WriteToMemory(
	unsigned __int64 memoryHandler,
	array<System::Byte>^ buffer)
{
	RdmaMrHandler *newElem = NULL;
	newElem = (RdmaMrHandler *)memoryHandler;
	HRESULT validResult = ValidRegisteredMemory(memoryHandler);
	if(validResult != ND_SUCCESS)
	{
		return validResult;
	}

	if(newElem->Length < (unsigned __int32)buffer->Length)
	{
		return ND_BUFFER_OVERFLOW;
	}

	pin_ptr<System::Byte> pin_buffer = &buffer[0];
	memcpy(newElem->Buffer, pin_buffer, buffer->Length);

	return ND_SUCCESS;
}

/// <summary>
/// Read data from the memory which memory handler points to.
/// </summary>
HRESULT RdmaEndpoint::ReadFromMemory(
	unsigned __int64 memoryHandler,
	array<System::Byte>^ buffer)
{
	RdmaMrHandler *newElem = NULL;
	newElem = (RdmaMrHandler *)memoryHandler;
	HRESULT validResult = ValidRegisteredMemory(memoryHandler);
	if(validResult != ND_SUCCESS)
	{
		return validResult;
	}

	if(newElem->Length < (unsigned __int32)buffer->Length)
	{
		return ND_BUFFER_OVERFLOW;
	}

	pin_ptr<System::Byte> pin_buffer = &buffer[0];
	memcpy(pin_buffer, newElem->Buffer, buffer->Length);

	return ND_SUCCESS;
}

// ===============================================================================

/// <summary>
/// Constructor
/// </summary>
/// <param name="endpoint">Endpoint entity with NDSPI type</param>
RdmaEndpoint::RdmaEndpoint(INDEndpoint *endpoint)
{
	_endpoint = endpoint;

	_pOverlapped = new OVERLAPPED;
	_pOverlapped->hEvent = CreateEvent( NULL, FALSE, FALSE, NULL );
}

HRESULT RdmaEndpoint::TransferSegment(
	array<RdmaSegment^>^ segments,
	ND_SGE** pSegmentList,
	SIZE_T *pSegmentListSize)
{
	*pSegmentList = new ND_SGE[segments->Length];
	*pSegmentListSize = segments->Length;
	ND_SGE *sgl = *pSegmentList;

	for(int i = 0; i < segments->Length; ++i)
	{
		RdmaMrHandler *newElem = NULL;
		newElem = (RdmaMrHandler *)segments[i]->MemoryHandler;
		HRESULT validResult = ValidRegisteredMemory(segments[i]->MemoryHandler);
		if(validResult != ND_SUCCESS)
		{
			delete[] sgl;
			return validResult;
		}
		sgl[i].pAddr = newElem->Buffer;
		sgl[i].Length = segments[i]->Length;
		sgl[i].hMr = newElem->MemoryHandler;

	}

	return ND_SUCCESS;
}

END_RDMA_NAMESPACE