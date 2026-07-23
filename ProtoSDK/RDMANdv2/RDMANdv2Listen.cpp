// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#include "RDMANdv2.h"

using namespace System;
using namespace System::Runtime::InteropServices;

BEGIN_RDMA_NAMESPACE

RdmaListen::~RdmaListen()
{
    if (_pOverlapped != NULL)
    {
        CloseHandle(_pOverlapped->hEvent);
        delete _pOverlapped;
        _pOverlapped = NULL;
    }

    if (_listen != NULL)
    {
        _listen->Release();
        _listen = NULL;
    }
}

HRESULT RdmaListen::GetConnectionRequest(RdmaConnector^ connector)
{
    HRESULT result = _listen->GetConnectionRequest(connector->_connector, _pOverlapped);
    if (result == ND_PENDING)
    {
        result = _listen->GetOverlappedResult(_pOverlapped, TRUE);
    }
    return result;
}

RdmaListen::RdmaListen(IND2Listener *pListen)
{
    _listen = pListen;

    _pOverlapped = new OVERLAPPED;
    _pOverlapped->hEvent = CreateEvent(NULL, FALSE, FALSE, NULL);
}

END_RDMA_NAMESPACE
