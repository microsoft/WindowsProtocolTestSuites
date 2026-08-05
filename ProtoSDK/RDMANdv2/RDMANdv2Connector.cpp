// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// RDMANdv2.h includes <ndspi.h>, which brings in <winsock2.h> in the
// correct order, so we must not include <winsock2.h> separately.
#include "RDMANdv2.h"

using namespace System;
using namespace System::Runtime::InteropServices;

BEGIN_RDMA_NAMESPACE

RdmaConnector::~RdmaConnector()
{
    if (_pOverlapped != NULL)
    {
        CloseHandle(_pOverlapped->hEvent);
        delete _pOverlapped;
        _pOverlapped = NULL;
    }

    if (_connector != NULL)
    {
        _connector->Release();
        _connector = NULL;
    }
}

HRESULT RdmaConnector::CreateEndpoint(
    RdmaCompletionQueue^ completionQueue,
    unsigned __int32 inboundEntries,
    unsigned __int32 outboundEntries,
    unsigned __int32 inboundSegment,
    unsigned __int32 outboundSegment,
    unsigned __int32 inboundReadLimit,
    unsigned __int32 outboundReadLimit,
    [Out]unsigned __int32% maxInlineData,
    [Out]RdmaEndpoint^% endpoint)
{
    SIZE_T maxInline = 0;
    IND2QueuePair *_queuePair = NULL;

    // In NDSPI v2 the queue pair is created through the adapter, not the connector.
    HRESULT result = _adapter->_adapter->CreateQueuePair(
        IID_IND2QueuePair,
        completionQueue->_completionQueue,
        completionQueue->_completionQueue,
        NULL,
        (ULONG)inboundEntries,
        (ULONG)outboundEntries,
        (ULONG)inboundSegment,
        (ULONG)outboundSegment,
        0, // inlineDataSize: let the provider choose the default
        reinterpret_cast<void**>(&_queuePair));

    if (result != ND_SUCCESS)
    {
        return result;
    }

    // Store read limits for Connect/Accept.
    _inboundReadLimit = inboundReadLimit;
    _outboundReadLimit = outboundReadLimit;

    endpoint = gcnew RdmaEndpoint(_queuePair);
    maxInlineData = (unsigned __int32)maxInline;
    return result;
}

HRESULT RdmaConnector::Connect(
    RdmaEndpoint^ endpoint,
    String^ ipAddress,
    int port,
    int protocol)
{
    IntPtr ptrRemoteIP = Marshal::StringToHGlobalAnsi(ipAddress);
    const char* pRemoteIP = static_cast<const char*>(ptrRemoteIP.ToPointer());

    sockaddr_in remoteAddr = {0};
    remoteAddr.sin_family = AF_INET;
    remoteAddr.sin_addr.s_addr = inet_addr(pRemoteIP);
    remoteAddr.sin_port = htons(port);

    // NDSPI v2 requires the connector to be bound to the local address
    // before Connect. Otherwise the provider returns STATUS_ADDRESS_NOT_ASSOCIATED.
    IntPtr ptrLocalIP = Marshal::StringToHGlobalAnsi(_adapter->_localIpAddress);
    const char* pLocalIP = static_cast<const char*>(ptrLocalIP.ToPointer());

    sockaddr_in localAddr = {0};
    localAddr.sin_family = _adapter->_ipFamily;
    localAddr.sin_addr.s_addr = inet_addr(pLocalIP);
    localAddr.sin_port = 0;

    HRESULT result = _connector->Bind(
        (const struct sockaddr*)&localAddr,
        sizeof(localAddr));

    Marshal::FreeHGlobal(ptrLocalIP);

    if (result != ND_SUCCESS)
    {
        Marshal::FreeHGlobal(ptrRemoteIP);
        return result;
    }

    // NDSPI v2 Connect does not take a protocol parameter;
    // it requires inbound/outbound read limits instead.
    result = _connector->Connect(
        endpoint->_queuePair,
        (const struct sockaddr*)&remoteAddr,
        sizeof(remoteAddr),
        (ULONG)_inboundReadLimit,
        (ULONG)_outboundReadLimit,
        NULL,
        0,
        _pOverlapped);

    if (result == ND_PENDING)
    {
        result = _connector->GetOverlappedResult(_pOverlapped, TRUE);
    }

    Marshal::FreeHGlobal(ptrRemoteIP);
    return result;
}

HRESULT RdmaConnector::CompleteConnect()
{
    HRESULT result = _connector->CompleteConnect(_pOverlapped);
    if (result == ND_PENDING)
    {
        result = _connector->GetOverlappedResult(_pOverlapped, TRUE);
    }
    return result;
}

HRESULT RdmaConnector::Accept(RdmaEndpoint^ endpoint)
{
    HRESULT hr = _connector->Accept(
        endpoint->_queuePair,
        (ULONG)_inboundReadLimit,
        (ULONG)_outboundReadLimit,
        NULL,
        0,
        _pOverlapped);

    if (hr == ND_PENDING)
    {
        hr = _connector->GetOverlappedResult(_pOverlapped, TRUE);
    }
    return hr;
}

HRESULT RdmaConnector::NotifyDisconnect()
{
    HRESULT result = _connector->NotifyDisconnect(_pOverlapped);
    if (result == ND_PENDING)
    {
        result = _connector->GetOverlappedResult(_pOverlapped, TRUE);
    }
    return result;
}

void RdmaConnector::Disconnect()
{
    _connector->Disconnect(_pOverlapped);
}

RdmaConnector::RdmaConnector(IND2Connector *connector, RdmaAdapter^ adapter)
{
    _connector = connector;
    _adapter = adapter;
    _inboundReadLimit = 0;
    _outboundReadLimit = 0;

    _pOverlapped = new OVERLAPPED{}; // value-initialize to zero all fields
    _pOverlapped->hEvent = CreateEvent(NULL, FALSE, FALSE, NULL);
}

END_RDMA_NAMESPACE
