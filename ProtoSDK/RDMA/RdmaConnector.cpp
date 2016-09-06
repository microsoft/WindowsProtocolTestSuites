// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#include "Rdma.h"



using namespace System;
using namespace System::Runtime::InteropServices;


// Begin SMBD client Namespace
BEGIN_RDMA_NAMESPACE

RdmaConnector::~RdmaConnector()
{
	delete _pOverlapped;
}

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
	SIZE_T maxInline;
	INDEndpoint *_endpoint;

	HRESULT result = _connector->CreateEndpoint(
		completionQueue->_completionQueue,
		completionQueue->_completionQueue,
		(SIZE_T)inboundEntries,
		(SIZE_T)outboundEntries,
		(SIZE_T)inboundSegment,
		(SIZE_T)outboundSegment,
		(SIZE_T)inboundReadLimit,
		(SIZE_T)outboundReadLimit,
		&maxInline,
		&_endpoint);
	if(result != ND_SUCCESS)
	{
		return result;
	}
	endpoint = gcnew RdmaEndpoint(_endpoint);
	maxInlineData = (unsigned __int32)maxInline;
	return result;
}

/// <summary>
/// Connects the endpoint to a listening peer.
/// </summary>
/// <param name="endpoint">An INDEndpoint interface that identifies the endpoint to use for the connection. 
///  The endpoint must have been created using this instance of the Connector object.</param>
/// <param name="ipAddress">ip address</param>
/// <param name="port">port</param>
/// <param name="protocol">An IANA Internet Protocol number</param>
HRESULT RdmaConnector::Connect(
	RdmaEndpoint^ endpoint,
	String^ ipAddress,
	int port,
	int protocol)
{
	IntPtr ptrRemoteIP = Marshal::StringToHGlobalAnsi(ipAddress);
	const char* pRemoteIP = static_cast<const char*>(ptrRemoteIP.ToPointer());

	// init remote IP & port
	sockaddr_in remoteAddr = {0};
	remoteAddr.sin_family = AF_INET;
	remoteAddr.sin_addr.s_addr = inet_addr(pRemoteIP);
	remoteAddr.sin_port = htons(port);

	HRESULT result = _connector->Connect(
		endpoint->_endpoint,
		(const struct sockaddr*)&remoteAddr,
        sizeof(remoteAddr),
        protocol, /* for tcp */
        0, /* for local ports*/
        NULL,
        0,
        _pOverlapped );
	if( result == ND_PENDING )
    {
        SIZE_T BytesRet;
        result = _connector->GetOverlappedResult( _pOverlapped, &BytesRet, TRUE );
    }
	return result;
}


/// <summary>
/// Completes the connection request initiated by a previous 
/// </summary>
HRESULT RdmaConnector::CompleteConnect()
{
	HRESULT result = _connector->CompleteConnect(_pOverlapped);
	if(result == ND_PENDING)
	{
		SIZE_T BytesRet;
        result = _connector->GetOverlappedResult( _pOverlapped, &BytesRet, TRUE );
	}
	return result;
}

/// <summary>
/// Accept arrival network connection
/// </summary>
/// <param name="endpoint">the endpoint to connect to the peer's endpoint. </param>
HRESULT RdmaConnector::Accept(RdmaEndpoint^ endpoint)
{
	HRESULT hr = _connector->Accept(
		endpoint->_endpoint,
		NULL,
		0,
		_pOverlapped);
	if(hr == ND_PENDING)
	{
		SIZE_T BytesRet;
        hr = _connector->GetOverlappedResult( _pOverlapped, &BytesRet, TRUE );
	}
	return hr;
}

/// <summary>
/// Notifies the caller that an established connection was disconnected.
/// </summary>
HRESULT RdmaConnector::NotifyDisconnect()
{
	HRESULT result = _connector->NotifyDisconnect( _pOverlapped );
	if(result == ND_PENDING)
	{
		SIZE_T BytesRet;
        result = _connector->GetOverlappedResult( _pOverlapped, &BytesRet, TRUE );
	}
	return result;
}

/// <summary>
/// Disconnects the endpoint from the peer.
/// </summary>
void RdmaConnector::Disconnect()
{
	_connector->Disconnect( _pOverlapped );
}
// =============================================================
// private

RdmaConnector::RdmaConnector(INDConnector *connector)
{
	_connector = connector;

	_pOverlapped = new OVERLAPPED;
	_pOverlapped->hEvent = CreateEvent( NULL, FALSE, FALSE, NULL );
}
END_RDMA_NAMESPACE