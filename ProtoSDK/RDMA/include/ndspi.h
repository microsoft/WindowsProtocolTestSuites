/*++

Copyright (c) Microsoft Corporation.  All rights reserved.

ndspi.h - Network Direct Service Provider Interface

--*/

#pragma once

#ifndef _NDSPI_H_
#define _NDSPI_H_

#include <winsock2.h>
#include <unknwn.h>
#include "ndstatus.h"


DECLARE_HANDLE(ND_MR_HANDLE);


typedef struct _ND_ADAPTER_INFO1
{
    UINT32 VendorId;
    UINT32 DeviceId;
    SIZE_T MaxInboundSge;
    SIZE_T MaxInboundRequests;
    SIZE_T MaxInboundLength;
    SIZE_T MaxOutboundSge;
    SIZE_T MaxOutboundRequests;
    SIZE_T MaxOutboundLength;
    SIZE_T MaxInlineData;
    SIZE_T MaxInboundReadLimit;
    SIZE_T MaxOutboundReadLimit;
    SIZE_T MaxCqEntries;
    SIZE_T MaxRegistrationSize;
    SIZE_T MaxWindowSize;
    SIZE_T LargeRequestThreshold;
    SIZE_T MaxCallerData;
    SIZE_T MaxCalleeData;

} ND_ADAPTER_INFO1;
#define ND_ADAPTER_INFO ND_ADAPTER_INFO1

typedef struct _ND_RESULT
{
    HRESULT Status;
    SIZE_T BytesTransferred;

} ND_RESULT;

#pragma pack( push, 1 )
typedef struct _ND_MW_DESCRIPTOR
{
    UINT64 Base;    // Network byte order
    UINT64 Length;  // Network byte order
    UINT32 Token;   // Network byte order

} ND_MW_DESCRIPTOR;
#pragma pack( pop )

typedef struct _ND_SGE
{
    VOID* pAddr;
    SIZE_T Length;
    ND_MR_HANDLE hMr;

} ND_SGE;

//
// Overlapped object
//
#undef INTERFACE
#define INTERFACE INDOverlapped

// {C859E15E-75E2-4fe3-8D6D-0DFF36F02442}
DEFINE_GUID(IID_INDOverlapped, 
0xc859e15e, 0x75e2, 0x4fe3, 0x8d, 0x6d, 0xd, 0xff, 0x36, 0xf0, 0x24, 0x42);

DECLARE_INTERFACE_(INDOverlapped, IUnknown)
{
    // *** IUnknown methods ***
    __override STDMETHOD(QueryInterface)(
        THIS_
        REFIID riid,
        LPVOID FAR* ppvObj
        ) PURE;

    __override STDMETHOD_(ULONG,AddRef)(
        THIS
        ) PURE;

    __override STDMETHOD_(ULONG,Release)(
        THIS
        ) PURE;

    // *** INDOverlapped methods ***
    STDMETHOD(CancelOverlappedRequests)(
        THIS
        ) PURE;

    STDMETHOD(GetOverlappedResult)(
        THIS_
        __inout OVERLAPPED *pOverlapped,
        __out SIZE_T *pNumberOfBytesTransferred,
        __in BOOL bWait
        ) PURE;
};

//
// Completion Queue
//
#undef INTERFACE
#define INTERFACE INDCompletionQueue

// {1245A633-2A32-473a-830C-E05D1F869D02}
DEFINE_GUID(IID_INDCompletionQueue, 
0x1245a633, 0x2a32, 0x473a, 0x83, 0xc, 0xe0, 0x5d, 0x1f, 0x86, 0x9d, 0x2);

DECLARE_INTERFACE_(INDCompletionQueue, INDOverlapped)
{
    // *** IUnknown methods ***
    __override STDMETHOD(QueryInterface)(
        THIS_
        REFIID riid,
        LPVOID FAR* ppvObj
        ) PURE;

    __override STDMETHOD_(ULONG,AddRef)(
        THIS
        ) PURE;

    __override STDMETHOD_(ULONG,Release)(
        THIS
        ) PURE;

    // *** INDOverlapped methods ***
    __override STDMETHOD(CancelOverlappedRequests)(
        THIS
        ) PURE;

    __override STDMETHOD(GetOverlappedResult)(
        THIS_
        __inout OVERLAPPED *pOverlapped,
        __out SIZE_T *pNumberOfBytesTransferred,
        __in BOOL bWait
        ) PURE;

    // *** INDCompletionQueue methods ***
    STDMETHOD(Resize)(
        THIS_
        __in SIZE_T nEntries
        ) PURE;

#define ND_CQ_NOTIFY_ERRORS 0
#define ND_CQ_NOTIFY_ANY 1
#define ND_CQ_NOTIFY_SOLICITED 2

    STDMETHOD(Notify)(
        THIS_
        __in DWORD Type,
        __inout OVERLAPPED* pOverlapped
        ) PURE;

    STDMETHOD_(SIZE_T,GetResults)(
        THIS_
        __out_ecount(nResults) ND_RESULT* pResults[],
        __in SIZE_T nResults
        ) PURE;
};


//
// Remove View
//
#undef INTERFACE
#define INTERFACE INDMemoryWindow

// {070FE1F5-0AB5-4361-88DB-974BA704D4B9}
DEFINE_GUID(IID_INDMemoryWindow, 
0x70fe1f5, 0xab5, 0x4361, 0x88, 0xdb, 0x97, 0x4b, 0xa7, 0x4, 0xd4, 0xb9);

DECLARE_INTERFACE_(INDMemoryWindow, IUnknown)
{
    // *** IUnknown methods ***
    __override STDMETHOD(QueryInterface)(
        THIS_
        REFIID riid,
        LPVOID FAR* ppvObj
        ) PURE;

    __override STDMETHOD_(ULONG,AddRef)(
        THIS
        ) PURE;

    __override STDMETHOD_(ULONG,Release)(
        THIS
        ) PURE;
};


//
// Endpoint
//
#undef INTERFACE
#define INTERFACE INDEndpoint

// {DBD00EAB-B679-44a9-BD65-E82F3DE12D1A}
DEFINE_GUID(IID_INDEndpoint, 
0xdbd00eab, 0xb679, 0x44a9, 0xbd, 0x65, 0xe8, 0x2f, 0x3d, 0xe1, 0x2d, 0x1a);

DECLARE_INTERFACE_(INDEndpoint, IUnknown)
{
    // *** IUnknown methods ***
    __override STDMETHOD(QueryInterface)(
        THIS_
        REFIID riid,
        LPVOID FAR* ppvObj
        ) PURE;

    __override STDMETHOD_(ULONG,AddRef)(
        THIS
        ) PURE;

    __override STDMETHOD_(ULONG,Release)(
        THIS
        ) PURE;

    // *** INDEndpoint methods ***
    STDMETHOD(Flush)(
        THIS
        ) PURE;

    STDMETHOD_(void, StartRequestBatch)(
        THIS
        ) PURE;

    STDMETHOD_(void, SubmitRequestBatch)(
        THIS
        ) PURE;

#define ND_OP_FLAG_SILENT_SUCCESS           0x00000001
#define ND_OP_FLAG_READ_FENCE               0x00000002
#define ND_OP_FLAG_SEND_AND_SOLICIT_EVENT   0x00000004
#define ND_OP_FLAG_ALLOW_READ               0x00000008
#define ND_OP_FLAG_ALLOW_WRITE              0x00000010

    STDMETHOD(Send)(
        THIS_
        __out ND_RESULT* pResult,
        __in_ecount(nSge) const ND_SGE* pSgl,
        __in SIZE_T nSge,
        __in DWORD Flags
        ) PURE;

    STDMETHOD(SendAndInvalidate)(
        THIS_
        __out ND_RESULT* pResult,
        __in_ecount(nSge) const ND_SGE* pSgl,
        __in SIZE_T nSge,
        __in const ND_MW_DESCRIPTOR* pRemoteMwDescriptor,
        __in DWORD Flags
        ) PURE;

    STDMETHOD(Receive)(
        THIS_
        __out ND_RESULT* pResult,
        __in_ecount(nSge) const ND_SGE* pSgl,
        __in SIZE_T nSge
        ) PURE;

    STDMETHOD(Bind)(
        THIS_
        __out ND_RESULT* pResult,
        __in ND_MR_HANDLE hMr,
        __in INDMemoryWindow* pMw,
        __in_bcount(BufferSize) const void* pBuffer,
        __in SIZE_T BufferSize,
        __in DWORD Flags,
        __out ND_MW_DESCRIPTOR* pMwDescriptor
        ) PURE;

    STDMETHOD(Invalidate)(
        THIS_
        __out ND_RESULT* pResult,
        __in INDMemoryWindow* pMw,
        __in DWORD Flags
        ) PURE;

    STDMETHOD(Read)(
        THIS_
        __out ND_RESULT* pResult,
        __in_ecount(nSge) const ND_SGE* pSgl,
        __in SIZE_T nSge,
        __in const ND_MW_DESCRIPTOR* pRemoteMwDescriptor,
        __in ULONGLONG Offset,
        __in DWORD Flags
        ) PURE;

    STDMETHOD(Write)(
        THIS_
        __out ND_RESULT* pResult,
        __in_ecount(nSge) const ND_SGE* pSgl,
        __in SIZE_T nSge,
        __in const ND_MW_DESCRIPTOR* pRemoteMwDescriptor,
        __in ULONGLONG Offset,
        __in DWORD Flags
        ) PURE;
};


//
// Connector
//
#undef INTERFACE
#define INTERFACE INDConnector

// {1BCAF2D1-E274-4aeb-AC57-CD5D4376E0B7}
DEFINE_GUID(IID_INDConnector, 
0x1bcaf2d1, 0xe274, 0x4aeb, 0xac, 0x57, 0xcd, 0x5d, 0x43, 0x76, 0xe0, 0xb7);

DECLARE_INTERFACE_(INDConnector, INDOverlapped)
{
    // *** IUnknown methods ***
    __override STDMETHOD(QueryInterface)(
        THIS_
        REFIID riid,
        LPVOID FAR* ppvObj
        ) PURE;

    __override STDMETHOD_(ULONG,AddRef)(
        THIS
        ) PURE;

    __override STDMETHOD_(ULONG,Release)(
        THIS
        ) PURE;

    // *** INDOverlapped methods ***
    __override STDMETHOD(CancelOverlappedRequests)(
        THIS
        ) PURE;

    __override STDMETHOD(GetOverlappedResult)(
        THIS_
        __inout OVERLAPPED *pOverlapped,
        __out SIZE_T *pNumberOfBytesTransferred,
        __in BOOL bWait
        ) PURE;

    // *** INDConnector methods ***
    STDMETHOD(CreateEndpoint)(
        THIS_
        __in INDCompletionQueue* pInboundCq,
        __in INDCompletionQueue* pOutboundCq,
        __in SIZE_T nInboundEntries,
        __in SIZE_T nOutboundEntries,
        __in SIZE_T nInboundSge,
        __in SIZE_T nOutboundSge,
        __in SIZE_T InboundReadLimit,
        __in SIZE_T OutboundReadLimit,
        __out_opt SIZE_T* pMaxInlineData,
        __deref_out INDEndpoint** ppEndpoint
        ) PURE;

    STDMETHOD(Connect)(
        THIS_
        __in INDEndpoint* pEndpoint,
        __in_bcount(AddressLength) const struct sockaddr* pAddress,
        __in SIZE_T AddressLength,
        __in INT Protocol,
        __in_opt USHORT LocalPort,
        __in_bcount_opt(PrivateDataLength) const void* pPrivateData,
        __in SIZE_T PrivateDataLength,
        __inout OVERLAPPED* pOverlapped
        ) PURE;

    STDMETHOD(CompleteConnect)(
        THIS_
        __inout OVERLAPPED* pOverlapped
        ) PURE;

    STDMETHOD(Accept)(
        THIS_
        __in INDEndpoint* pEndpoint,
        __in_bcount_opt(PrivateDataLength) const void* pPrivateData,
        __in SIZE_T PrivateDataLength,
        __inout OVERLAPPED* pOverlapped
        ) PURE;

    STDMETHOD(Reject)(
        THIS_
        __in_bcount_opt(PrivateDataLength) const void* pPrivateData,
        __in SIZE_T PrivateDataLength
        ) PURE;

    STDMETHOD(GetConnectionData)(
        THIS_
        __out_opt SIZE_T* pInboundReadLimit,
        __out_opt SIZE_T* pOutboundReadLimit,
        __out_bcount_part_opt(*pPrivateDataLength, *pPrivateDataLength) void* pPrivateData,
        __inout SIZE_T* pPrivateDataLength
        ) PURE;

    STDMETHOD(GetLocalAddress)(
        THIS_
        __out_bcount_part_opt(*pAddressLength, *pAddressLength) struct sockaddr* pAddress,
        __inout SIZE_T* pAddressLength
        ) PURE;

    STDMETHOD(GetPeerAddress)(
        THIS_
        __out_bcount_part_opt(*pAddressLength, *pAddressLength) struct sockaddr* pAddress,
        __inout SIZE_T* pAddressLength
        ) PURE;

    STDMETHOD(NotifyDisconnect)(
        THIS_
        __inout OVERLAPPED* pOverlapped
        ) PURE;

    STDMETHOD(Disconnect)(
        THIS_
        __inout OVERLAPPED* pOverlapped
        ) PURE;
};


//
// Listen
//
#undef INTERFACE
#define INTERFACE INDListen

// {BB902588-BA3F-4441-9FE1-3B6795E4E668}
DEFINE_GUID(IID_INDListen, 
0xbb902588, 0xba3f, 0x4441, 0x9f, 0xe1, 0x3b, 0x67, 0x95, 0xe4, 0xe6, 0x68);

DECLARE_INTERFACE_(INDListen, INDOverlapped)
{
    // *** IUnknown methods ***
    __override STDMETHOD(QueryInterface)(
        THIS_
        REFIID riid,
        LPVOID FAR* ppvObj
        ) PURE;

    __override STDMETHOD_(ULONG,AddRef)(
        THIS
        ) PURE;

    __override STDMETHOD_(ULONG,Release)(
        THIS
        ) PURE;

    // *** INDOverlapped methods ***
    __override STDMETHOD(CancelOverlappedRequests)(
        THIS
        ) PURE;

    __override STDMETHOD(GetOverlappedResult)(
        THIS_
        __inout OVERLAPPED *pOverlapped,
        __out SIZE_T *pNumberOfBytesTransferred,
        __in BOOL bWait
        ) PURE;

    // *** INDListen methods ***
    STDMETHOD(GetConnectionRequest)(
        THIS_
        __inout INDConnector* pConnector,
        __inout OVERLAPPED* pOverlapped
        ) PURE;
};


//
// INDAdapter
//
#undef INTERFACE
#define INTERFACE INDAdapter

// {A023C5A0-5B73-43bc-8D20-33AA07E9510F}
DEFINE_GUID(IID_INDAdapter, 
0xa023c5a0, 0x5b73, 0x43bc, 0x8d, 0x20, 0x33, 0xaa, 0x7, 0xe9, 0x51, 0xf);

DECLARE_INTERFACE_(INDAdapter, INDOverlapped)
{
    // *** IUnknown methods ***
    __override STDMETHOD(QueryInterface)(
        THIS_
        REFIID riid,
        LPVOID FAR* ppvObj
        ) PURE;

    __override STDMETHOD_(ULONG,AddRef)(
        THIS
        ) PURE;

    __override STDMETHOD_(ULONG,Release)(
        THIS
        ) PURE;

    // *** INDOverlapped methods ***
    __override STDMETHOD(CancelOverlappedRequests)(
        THIS
        ) PURE;

    __override STDMETHOD(GetOverlappedResult)(
        THIS_
        __inout OVERLAPPED *pOverlapped,
        __out SIZE_T *pNumberOfBytesTransferred,
        __in BOOL bWait
        ) PURE;

    // *** INDAdapter methods ***
    STDMETHOD_(HANDLE,GetFileHandle)(
        THIS
        ) PURE;

    STDMETHOD(Query)(
        THIS_
        __in DWORD VersionRequested,
        __out_bcount_part_opt(*pBufferSize, *pBufferSize) ND_ADAPTER_INFO* pInfo,
        __inout_opt SIZE_T* pBufferSize
        ) PURE;

    STDMETHOD(Control)(
        THIS_
        __in DWORD IoControlCode,
        __in_bcount_opt(InBufferSize) const void* pInBuffer,
        __in SIZE_T InBufferSize,
        __out_bcount_opt(OutBufferSize) void* pOutBuffer,
        __in SIZE_T OutBufferSize,
        __out SIZE_T* pBytesReturned,
        __inout OVERLAPPED* pOverlapped
        ) PURE;

    STDMETHOD(CreateCompletionQueue)(
        THIS_
        __in SIZE_T nEntries,
        __deref_out INDCompletionQueue** ppCq
        ) PURE;

    STDMETHOD(RegisterMemory)(
        THIS_
        __in_bcount(BufferSize) const void* pBuffer,
        __in SIZE_T BufferSize,
        __inout OVERLAPPED* pOverlapped,
        __deref_out ND_MR_HANDLE* phMr
        ) PURE;

    STDMETHOD(DeregisterMemory)(
        THIS_
        __in ND_MR_HANDLE hMr,
        __inout OVERLAPPED* pOverlapped
        ) PURE;

    STDMETHOD(CreateMemoryWindow)(
        THIS_
        __out ND_RESULT* pInvalidateResult,
        __deref_out INDMemoryWindow** ppMw
        ) PURE;

    STDMETHOD(CreateConnector)(
        THIS_
        __deref_out INDConnector** ppConnector
        ) PURE;

    STDMETHOD(Listen)(
        THIS_
        __in SIZE_T Backlog,
        __in INT Protocol,
        __in USHORT Port,
        __out_opt USHORT* pAssignedPort,
        __deref_out INDListen** ppListen
        ) PURE;
};


//
// INDProvider
//
#undef INTERFACE
#define INTERFACE INDProvider

// {0C5DD316-5FDF-47e6-B2D0-2A6EDA8D39DD}
DEFINE_GUID(IID_INDProvider, 
0xc5dd316, 0x5fdf, 0x47e6, 0xb2, 0xd0, 0x2a, 0x6e, 0xda, 0x8d, 0x39, 0xdd);

DECLARE_INTERFACE_(INDProvider, IUnknown)
{
    // *** IUnknown methods ***
    __override STDMETHOD(QueryInterface)(
        THIS_
        REFIID riid,
        LPVOID FAR* ppvObj
        ) PURE;

    __override STDMETHOD_(ULONG,AddRef)(
        THIS
        ) PURE;

    __override STDMETHOD_(ULONG,Release)(
        THIS
        ) PURE;

    // *** INDProvider methods ***
    STDMETHOD(QueryAddressList)(
        THIS_
        __out_bcount_part_opt(*pBufferSize, *pBufferSize) SOCKET_ADDRESS_LIST* pAddressList,
        __inout SIZE_T* pBufferSize
        ) PURE;

    STDMETHOD(OpenAdapter)(
        THIS_
        __in_bcount(AddressLength) const struct sockaddr* pAddress,
        __in SIZE_T AddressLength,
        __deref_out INDAdapter** ppAdapter
        ) PURE;
};

#endif // _NDSPI_H_
