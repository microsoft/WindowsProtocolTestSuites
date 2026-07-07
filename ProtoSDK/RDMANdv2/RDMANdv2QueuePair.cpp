// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#include "RDMANdv2.h"
#include <string.h>

using namespace System;
using namespace System::Runtime::InteropServices;

BEGIN_RDMA_NAMESPACE

RdmaEndpoint::~RdmaEndpoint()
{
    if (_pOverlapped != NULL)
    {
        CloseHandle(_pOverlapped->hEvent);
        delete _pOverlapped;
        _pOverlapped = NULL;
    }

    if (_queuePair != NULL)
    {
        _queuePair->Release();
        _queuePair = NULL;
    }
}

HRESULT RdmaEndpoint::Flush()
{
    return _queuePair->Flush();
}

HRESULT RdmaEndpoint::Send(array<RdmaSegment^>^ segments, [Out]unsigned __int64% resultHandler)
{
    if (segments->Length == 0)
    {
        return ND_SUCCESS;
    }

    unsigned __int64 contextId = ++_nextRequestContext;
    if (contextId == 0)
    {
        contextId = ++_nextRequestContext;
    }
    resultHandler = contextId;

    ND2_SGE* segmentList = NULL;
    SIZE_T segmentListSize = 0;
    HRESULT result = TransferSegment(segments, &segmentList, &segmentListSize);
    if (result != ND_SUCCESS)
    {
        return result;
    }

    result = _queuePair->Send(
        reinterpret_cast<void*>(contextId),
        segmentList,
        (ULONG)segmentListSize,
        0);

    delete[] segmentList;
    return result;
}

HRESULT RdmaEndpoint::SendAndInvalidate(
    array<RdmaSegment^>^ segments,
    RdmaBufferDescriptorV1 bufferDescriptor,
    bool reverseMemory,
    [Out]unsigned __int64% resultHandler)
{
    // NDSPI v2 does not expose a single SendAndInvalidate primitive.
    // The SMBD layer currently does not call this API, so fall back to Send.
    return Send(segments, resultHandler);
}

HRESULT RdmaEndpoint::Receive(array<RdmaSegment^>^ segments, [Out]unsigned __int64% resultHandler)
{
    if (segments->Length == 0)
    {
        return ND_SUCCESS;
    }

    unsigned __int64 contextId = ++_nextRequestContext;
    if (contextId == 0)
    {
        contextId = ++_nextRequestContext;
    }
    resultHandler = contextId;

    ND2_SGE* segmentList = NULL;
    SIZE_T segmentListSize = 0;
    HRESULT result = TransferSegment(segments, &segmentList, &segmentListSize);
    if (result != ND_SUCCESS)
    {
        return result;
    }

    result = _queuePair->Receive(
        reinterpret_cast<void*>(contextId),
        segmentList,
        (ULONG)segmentListSize);

    delete[] segmentList;
    return result;
}

HRESULT RdmaEndpoint::Bind(
    unsigned __int64 memoryHandler,
    RdmaMemoryWindow^ memoryWindow,
    RdmaOperationReadWriteFlag flag,
    bool reverseMemory,
    [Out]RdmaBufferDescriptorV1^% bufferDescriptor,
    [Out]unsigned __int64% resultHandler)
{
    HRESULT result = ValidRegisteredMemory(memoryHandler);
    if (result != ND_SUCCESS)
    {
        return result;
    }

    RdmaMrHandler *mrHandler = (RdmaMrHandler *)memoryHandler;

    unsigned __int64 contextId = ++_nextRequestContext;
    if (contextId == 0)
    {
        contextId = ++_nextRequestContext;
    }
    resultHandler = contextId;

    result = _queuePair->Bind(
        reinterpret_cast<void*>(contextId),
        mrHandler->MemoryRegion,
        memoryWindow->_memoryWindow,
        mrHandler->Buffer,
        (SIZE_T)mrHandler->Length,
        (DWORD)flag);

    if (result != ND_SUCCESS)
    {
        return result;
    }

    unsigned __int64 base = (unsigned __int64)mrHandler->Buffer;
    unsigned __int32 token = memoryWindow->RemoteToken;
    unsigned __int32 length = mrHandler->Length;

    System::Diagnostics::Trace::WriteLine(System::String::Format(
        "RDMANdv2::Bind immediate token = 0x{0:X8}, base = 0x{1:X16}, length = {2}",
        token, base, length));

    bufferDescriptor = gcnew RdmaBufferDescriptorV1();
    bufferDescriptor->Offset = base;
    bufferDescriptor->Token = token;
    bufferDescriptor->Length = length;

    return result;
}

HRESULT RdmaEndpoint::Invalidate(RdmaMemoryWindow^ memoryWindow, [Out]unsigned __int64% resultHandler)
{
    unsigned __int64 contextId = ++_nextRequestContext;
    if (contextId == 0)
    {
        contextId = ++_nextRequestContext;
    }
    resultHandler = contextId;

    return _queuePair->Invalidate(
        reinterpret_cast<void*>(contextId),
        memoryWindow->_memoryWindow,
        0);
}

HRESULT RdmaEndpoint::Read(
    array<RdmaSegment^>^ segments,
    RdmaBufferDescriptorV1^ bufferDescriptor,
    bool reverseMemory,
    ULONGLONG offset,
    [Out]unsigned __int64% resultHandler)
{
    if (segments->Length == 0)
    {
        return ND_SUCCESS;
    }

    unsigned __int64 contextId = ++_nextRequestContext;
    if (contextId == 0)
    {
        contextId = ++_nextRequestContext;
    }
    resultHandler = contextId;

    ND2_SGE* segmentList = NULL;
    SIZE_T segmentListSize = 0;
    HRESULT result = TransferSegment(segments, &segmentList, &segmentListSize);
    if (result != ND_SUCCESS)
    {
        return result;
    }

    // IND2QueuePair::Read expects remoteAddress/remoteToken in host order.
    // The bufferDescriptor fields are logical values, so use them directly.
    unsigned __int64 remoteAddress = bufferDescriptor->Offset + offset;
    unsigned __int32 remoteToken = bufferDescriptor->Token;

    result = _queuePair->Read(
        reinterpret_cast<void*>(contextId),
        segmentList,
        (ULONG)segmentListSize,
        remoteAddress,
        remoteToken,
        0);

    delete[] segmentList;
    return result;
}

HRESULT RdmaEndpoint::Write(
    array<RdmaSegment^>^ segments,
    RdmaBufferDescriptorV1^ bufferDescriptor,
    bool reverseMemory,
    ULONGLONG offset,
    [Out]unsigned __int64% resultHandler)
{
    if (segments->Length == 0)
    {
        return ND_SUCCESS;
    }

    unsigned __int64 contextId = ++_nextRequestContext;
    if (contextId == 0)
    {
        contextId = ++_nextRequestContext;
    }
    resultHandler = contextId;

    ND2_SGE* segmentList = NULL;
    SIZE_T segmentListSize = 0;
    HRESULT result = TransferSegment(segments, &segmentList, &segmentListSize);
    if (result != ND_SUCCESS)
    {
        return result;
    }

    // IND2QueuePair::Write expects remoteAddress/remoteToken in host order.
    // The bufferDescriptor fields are logical values, so use them directly.
    unsigned __int64 remoteAddress = bufferDescriptor->Offset + offset;
    unsigned __int32 remoteToken = bufferDescriptor->Token;

    result = _queuePair->Write(
        reinterpret_cast<void*>(contextId),
        segmentList,
        (ULONG)segmentListSize,
        remoteAddress,
        remoteToken,
        0);

    delete[] segmentList;
    return result;
}

HRESULT RdmaEndpoint::ValidRegisteredMemory(unsigned __int64 memoryHandler)
{
    RdmaMrHandler *elem = (RdmaMrHandler *)memoryHandler;
    if (elem == NULL || elem->MagicNumber != RDMA_MAGIC_NUM)
    {
        return ND_INVALID_PARAMETER;
    }

    return ND_SUCCESS;
}

HRESULT RdmaEndpoint::WriteToMemory(
    unsigned __int64 memoryHandler,
    array<System::Byte>^ buffer)
{
    RdmaMrHandler *elem = (RdmaMrHandler *)memoryHandler;
    HRESULT result = ValidRegisteredMemory(memoryHandler);
    if (result != ND_SUCCESS)
    {
        return result;
    }

    if (elem->Length < (unsigned __int32)buffer->Length)
    {
        return ND_BUFFER_OVERFLOW;
    }

    pin_ptr<System::Byte> pin_buffer = &buffer[0];
    memcpy(elem->Buffer, pin_buffer, buffer->Length);

    return ND_SUCCESS;
}

HRESULT RdmaEndpoint::ReadFromMemory(
    unsigned __int64 memoryHandler,
    array<System::Byte>^ buffer)
{
    RdmaMrHandler *elem = (RdmaMrHandler *)memoryHandler;
    HRESULT result = ValidRegisteredMemory(memoryHandler);
    if (result != ND_SUCCESS)
    {
        return result;
    }

    if (elem->Length < (unsigned __int32)buffer->Length)
    {
        return ND_BUFFER_OVERFLOW;
    }

    pin_ptr<System::Byte> pin_buffer = &buffer[0];
    memcpy(pin_buffer, elem->Buffer, buffer->Length);

    return ND_SUCCESS;
}

RdmaEndpoint::RdmaEndpoint(IND2QueuePair *queuePair)
{
    _queuePair = queuePair;

    _pOverlapped = new OVERLAPPED;
    _pOverlapped->hEvent = CreateEvent(NULL, FALSE, FALSE, NULL);
    _nextRequestContext = 1;
}

HRESULT RdmaEndpoint::TransferSegment(
    array<RdmaSegment^>^ segments,
    ND2_SGE** pSegmentList,
    SIZE_T *pSegmentListSize)
{
    *pSegmentList = new ND2_SGE[segments->Length];
    *pSegmentListSize = segments->Length;
    ND2_SGE *sgl = *pSegmentList;

    for(int i = 0; i < segments->Length; ++i)
    {
        RdmaMrHandler *elem = (RdmaMrHandler *)segments[i]->MemoryHandler;
        HRESULT result = ValidRegisteredMemory(segments[i]->MemoryHandler);
        if (result != ND_SUCCESS)
        {
            delete[] sgl;
            return result;
        }
        sgl[i].Buffer = elem->Buffer;
        sgl[i].BufferLength = segments[i]->Length;
        sgl[i].MemoryRegionToken = elem->LocalToken;
    }

    return ND_SUCCESS;
}

END_RDMA_NAMESPACE
