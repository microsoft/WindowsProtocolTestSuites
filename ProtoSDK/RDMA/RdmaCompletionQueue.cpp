// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#include "Rdma.h"



using namespace System;
using namespace System::Runtime::InteropServices;


// Begin SMBD client Namespace
BEGIN_RDMA_NAMESPACE


RdmaCompletionQueue::~RdmaCompletionQueue()
{
	delete _pOverlapped;
}

/// <summary>
/// Requests notification for errors and completions
/// </summary>
HRESULT RdmaCompletionQueue::Notify()
{
	HRESULT result = _completionQueue->Notify(ND_CQ_NOTIFY_ANY, _pOverlapped);
	if(result == ND_PENDING)
	{
		SIZE_T BytesRet;
        result = _completionQueue->GetOverlappedResult( _pOverlapped, &BytesRet, TRUE );
	}
	return result;
}

/// <summary>
/// Retrieves the requested completion results from the completion queue.
/// </summary>
/// <param name="resultHandler">handler of result</param>
/// <param name="requestResult">result information of send/receive/read/write/bind/invalid request</param>
unsigned __int64 RdmaCompletionQueue::GetResult(
	[Out]unsigned __int64% resultHandler,
	[Out]RdmaNetworkDirectResult^% requestResult)
{
	ND_RESULT *pResult;
	unsigned __int64 count = _completionQueue->GetResults(&pResult, 1);
	if(count <= 0)
	{
		return 0;
	}

	resultHandler = (unsigned __int64)pResult;
	requestResult = gcnew RdmaNetworkDirectResult();
	requestResult->Status = pResult->Status;
	requestResult->BytesTransferred = (unsigned __int32)pResult->BytesTransferred;

	delete pResult;
	return count;
}

// =============================================================
// private


RdmaCompletionQueue::RdmaCompletionQueue(INDCompletionQueue *completionQueue)
{
	_completionQueue = completionQueue;

	_pOverlapped = new OVERLAPPED;
	_pOverlapped->hEvent = CreateEvent( NULL, FALSE, FALSE, NULL );
}
END_RDMA_NAMESPACE