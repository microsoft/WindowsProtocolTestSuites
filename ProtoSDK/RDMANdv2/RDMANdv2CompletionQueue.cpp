// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#include "RDMANdv2.h"

using namespace System;
using namespace System::Runtime::InteropServices;

BEGIN_RDMA_NAMESPACE

RdmaCompletionQueue::~RdmaCompletionQueue()
{
    if (_pOverlapped != NULL)
    {
        CloseHandle(_pOverlapped->hEvent);
        delete _pOverlapped;
        _pOverlapped = NULL;
    }

    if (_completionQueue != NULL)
    {
        _completionQueue->Release();
        _completionQueue = NULL;
    }
}

HRESULT RdmaCompletionQueue::Notify()
{
    HRESULT result = _completionQueue->Notify(ND_CQ_NOTIFY_ANY, _pOverlapped);
    if (result == ND_PENDING)
    {
        result = _completionQueue->GetOverlappedResult(_pOverlapped, TRUE);
    }
    return result;
}

unsigned __int64 RdmaCompletionQueue::GetResult(
    [Out]unsigned __int64% resultHandler,
    [Out]RdmaNetworkDirectResult^% requestResult)
{
    ND2_RESULT result;
    ZeroMemory(&result, sizeof(result));
    ULONG count = _completionQueue->GetResults(&result, 1);
    if (count == 0)
    {
        return 0;
    }

    resultHandler = (unsigned __int64)result.RequestContext;
    requestResult = gcnew RdmaNetworkDirectResult();
    requestResult->Status = result.Status;
    requestResult->BytesTransferred = result.BytesTransferred;

    return count;
}

RdmaCompletionQueue::RdmaCompletionQueue(IND2CompletionQueue *completionQueue)
{
    _completionQueue = completionQueue;

    _pOverlapped = new OVERLAPPED;
    _pOverlapped->hEvent = CreateEvent(NULL, FALSE, FALSE, NULL);
}

END_RDMA_NAMESPACE
