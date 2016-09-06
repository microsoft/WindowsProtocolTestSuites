// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#include "Rdma.h"



using namespace System;
using namespace System::Runtime::InteropServices;


// Begin SMBD client Namespace
BEGIN_RDMA_NAMESPACE


RdmaListen::~RdmaListen()
{
	delete _pOverlapped;
}

/// <summary>
/// Retrieves the handle for a pending connection request.
/// </summary>
HRESULT RdmaListen::GetConnectionRequest(RdmaConnector^ connector)
{
	// wait for connection
	HRESULT result = _listen->GetConnectionRequest( connector->_connector, _pOverlapped );
    if( result == ND_PENDING )
    {
        SIZE_T BytesRet;
        result = _listen->GetOverlappedResult( _pOverlapped, &BytesRet, TRUE );
    }
    return result;
}
// ====================================================================
// internal

RdmaListen::RdmaListen(INDListen *pListen)
{
	_listen = pListen;

	_pOverlapped = new OVERLAPPED;
	_pOverlapped->hEvent = CreateEvent( NULL, FALSE, FALSE, NULL );
}

END_RDMA_NAMESPACE