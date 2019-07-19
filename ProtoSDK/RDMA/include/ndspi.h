/*++

Copyright (c) Microsoft Corporation.  All rights reserved.

ndspi.h - NetworkDirect Service Provider Interface

--*/

#pragma once

#ifndef _NDSPI_H_
#define _NDSPI_H_

#include <winsock2.h>
#include <unknwn.h>
#include "ndstatus.h"


#define ND_VERSION_1    0x1
#define ND_VERSION_2    0x20000

#ifndef NDVER
#define NDVER      ND_VERSION_2
#endif


typedef struct _ND2_ADAPTER_INFO
{
    LONG InfoVersion;

    UINT16 VendorId;
    UINT16 DeviceId;
    UINT64 AdapterId;

    SIZE_T MaxRegistrationSize;
    SIZE_T MaxWindowSize;

    ULONG MaxInitiatorSge;
    ULONG MaxReceiveSge;
    ULONG MaxReadSge;

    ULONG MaxTransferLength;

    ULONG MaxInlineDataSize;

    ULONG MaxInboundReadLimit;
    ULONG MaxOutboundReadLimit;

    ULONG MaxReceiveQueueDepth;
    ULONG MaxInitiatorQueueDepth;
    ULONG MaxSharedReceiveQueueDepth;
    ULONG MaxCompletionQueueDepth;

    ULONG InlineRequestThreshold;
    ULONG LargeRequestThreshold;

    ULONG MaxCallerData;
    ULONG MaxCalleeData;

    BOOL InOrderDMA;
    BOOL SupportsCQResize;
    BOOL SupportsLoopbackConnections;

} ND2_ADAPTER_INFO;

typedef enum _ND2_REQUEST_TYPE
{
    Nd2RequestTypeSend,
    Nd2RequestTypeReceive,
    Nd2RequestTypeBind,
    Nd2RequestTypeInvalidate,
    Nd2RequestTypeRead,
    Nd2RequestTypeWrite

} ND2_REQUEST_TYPE;

typedef struct _ND2_RESULT
{
    HRESULT Status;
    ULONG BytesTransferred;
    VOID* QueuePairContext;
    VOID* RequestContext;
    ND2_REQUEST_TYPE RequestType;

} ND2_RESULT;

typedef struct _ND2_SGE
{
    VOID* Buffer;
    ULONG BufferLength;
    UINT32 MemoryRegionToken;

} ND2_SGE;


//
// Overlapped object
//
#undef INTERFACE
#define INTERFACE IND2Overlapped

// {ABF72719-B016-4a40-A6F7-622791A7044C}
DEFINE_GUID(IID_IND2Overlapped, 
0xabf72719, 0xb016, 0x4a40, 0xa6, 0xf7, 0x62, 0x27, 0x91, 0xa7, 0x4, 0x4c);

DECLARE_INTERFACE_(IND2Overlapped, IUnknown)
{
    // *** IUnknown methods ***
    IFACEMETHOD(QueryInterface)(
        THIS_
        REFIID riid,
        _Deref_out_ LPVOID* ppvObj
        ) PURE;

    IFACEMETHOD_(ULONG,AddRef)(
        THIS
        ) PURE;

    IFACEMETHOD_(ULONG,Release)(
        THIS
        ) PURE;

    // *** IND2Overlapped methods ***
    STDMETHOD(CancelOverlappedRequests)(
        THIS
        ) PURE;

    STDMETHOD(GetOverlappedResult)(
        THIS_
        _In_ OVERLAPPED* pOverlapped,
        BOOL wait
        ) PURE;
};


//
// Completion Queue
//
#undef INTERFACE
#define INTERFACE IND2CompletionQueue

// {20CC445E-64A0-4cbb-AA75-F6A7251FDA9E}
DEFINE_GUID(IID_IND2CompletionQueue, 
0x20cc445e, 0x64a0, 0x4cbb, 0xaa, 0x75, 0xf6, 0xa7, 0x25, 0x1f, 0xda, 0x9e);

DECLARE_INTERFACE_(IND2CompletionQueue, IND2Overlapped)
{
    // *** IUnknown methods ***
    IFACEMETHOD(QueryInterface)(
        THIS_
        REFIID riid,
        _Deref_out_ LPVOID* ppvObj
        ) PURE;

    IFACEMETHOD_(ULONG,AddRef)(
        THIS
        ) PURE;

    IFACEMETHOD_(ULONG,Release)(
        THIS
        ) PURE;

    // *** IND2Overlapped methods ***
    IFACEMETHOD(CancelOverlappedRequests)(
        THIS
        ) PURE;

    IFACEMETHOD(GetOverlappedResult)(
        THIS_
        _In_ OVERLAPPED* pOverlapped,
        BOOL wait
        ) PURE;

    // *** IND2CompletionQueue methods ***
    STDMETHOD(GetNotifyAffinity)(
        THIS_
        _Out_ USHORT* pGroup,
        _Out_ KAFFINITY* pAffinity
        ) PURE;

    STDMETHOD(Resize)(
        THIS_
        ULONG queueDepth
        ) PURE;

#define ND_CQ_NOTIFY_ERRORS     0
#define ND_CQ_NOTIFY_ANY        1
#define ND_CQ_NOTIFY_SOLICITED  2

    STDMETHOD(Notify)(
        THIS_
        ULONG type,
        _Inout_ OVERLAPPED* pOverlapped
        ) PURE;

    STDMETHOD_(ULONG,GetResults)(
        THIS_
        _Out_cap_post_count_(nResults,return) ND2_RESULT results[],
        ULONG nResults
        ) PURE;
};


//
// Shared Receive Queue
//
#undef INTERFACE
#define INTERFACE IND2SharedReceiveQueue

// {AABD67DC-459A-4db1-826B-56CFCC278883}
DEFINE_GUID(IID_IND2SharedReceiveQueue, 
0xaabd67dc, 0x459a, 0x4db1, 0x82, 0x6b, 0x56, 0xcf, 0xcc, 0x27, 0x88, 0x83);

DECLARE_INTERFACE_(IND2SharedReceiveQueue, IND2Overlapped)
{
    // *** IUnknown methods ***
    IFACEMETHOD(QueryInterface)(
        THIS_
        REFIID riid,
        _Deref_out_ LPVOID* ppvObj
        ) PURE;

    IFACEMETHOD_(ULONG,AddRef)(
        THIS
        ) PURE;

    IFACEMETHOD_(ULONG,Release)(
        THIS
        ) PURE;

    // *** IND2Overlapped methods ***
    IFACEMETHOD(CancelOverlappedRequests)(
        THIS
        ) PURE;

    IFACEMETHOD(GetOverlappedResult)(
        THIS_
        _In_ OVERLAPPED* pOverlapped,
        BOOL wait
        ) PURE;

    // *** IND2SharedReceiveQueue methods ***
    STDMETHOD(GetNotifyAffinity)(
        THIS_
        _Out_ USHORT* pGroup,
        _Out_ KAFFINITY* pAffinity
        ) PURE;

    STDMETHOD(Modify)(
        THIS_
        ULONG queueDepth,
        ULONG notifyThreshold
        ) PURE;

    STDMETHOD(Notify)(
        THIS_
        _Inout_ OVERLAPPED* pOverlapped
        ) PURE;

    STDMETHOD(Receive)(
        THIS_
        _In_ VOID* requestContext,
        _In_opt_count_(nSge) const ND2_SGE pSge[],
        ULONG nSge
        ) PURE;
};


//
// Memory Window
//
#undef INTERFACE
#define INTERFACE IND2MemoryWindow

// {070FE1F5-0AB5-4361-88DB-974BA704D4B9}
DEFINE_GUID(IID_IND2MemoryWindow, 
0x70fe1f5, 0xab5, 0x4361, 0x88, 0xdb, 0x97, 0x4b, 0xa7, 0x4, 0xd4, 0xb9);

DECLARE_INTERFACE_(IND2MemoryWindow, IUnknown)
{
    // *** IUnknown methods ***
    IFACEMETHOD(QueryInterface)(
        THIS_
        REFIID riid,
        _Deref_out_ LPVOID* ppvObj
        ) PURE;

    IFACEMETHOD_(ULONG,AddRef)(
        THIS
        ) PURE;

    IFACEMETHOD_(ULONG,Release)(
        THIS
        ) PURE;

    // *** IND2MemoryWindow methods ***
    STDMETHOD_(UINT32, GetRemoteToken)(
        THIS
        ) PURE;
};


//
// Memory Region
//
#undef INTERFACE
#define INTERFACE IND2MemoryRegion

// {55DFEA2F-BC56-4982-8A45-0301BE46C413}
DEFINE_GUID(IID_IND2MemoryRegion, 
0x55dfea2f, 0xbc56, 0x4982, 0x8a, 0x45, 0x3, 0x1, 0xbe, 0x46, 0xc4, 0x13);

DECLARE_INTERFACE_(IND2MemoryRegion, IND2Overlapped)
{
    // *** IUnknown methods ***
    IFACEMETHOD(QueryInterface)(
        THIS_
        REFIID riid,
        _Deref_out_ LPVOID* ppvObj
        ) PURE;

    IFACEMETHOD_(ULONG,AddRef)(
        THIS
        ) PURE;

    IFACEMETHOD_(ULONG,Release)(
        THIS
        ) PURE;

    // *** IND2Overlapped methods ***
    IFACEMETHOD(CancelOverlappedRequests)(
        THIS
        ) PURE;

    IFACEMETHOD(GetOverlappedResult)(
        THIS_
        _In_ OVERLAPPED* pOverlapped,
        BOOL wait
        ) PURE;

    // *** IND2MemoryRegion methods ***
#define ND_MR_FLAG_ALLOW_LOCAL_READ             0x00000000
#define ND_MR_FLAG_ALLOW_LOCAL_WRITE            0x00000001
#define ND_MR_FLAG_ALLOW_REMOTE_READ            0x00000002
#define ND_MR_FLAG_ALLOW_REMOTE_WRITE           0x00000005
#define ND_MR_FLAG_RDMA_READ_SINK               0x00000008
#define ND_MR_FLAG_DO_NOT_SECURE_VM             0x80000000  // To support AWE memory.

    //////////////////////////////////
    // flags - Combination of ND2_MR_FLAG_ALLOW_XXX.  Note remote flags imply local.
    STDMETHOD(Register)(
        THIS_
        _In_bytecount_(cbBuffer) const VOID* pBuffer,
        SIZE_T cbBuffer,
        ULONG flags,
        _Inout_ OVERLAPPED* pOverlapped
        ) PURE;

    STDMETHOD(Deregister)(
        THIS_
        _Inout_ OVERLAPPED* pOverlapped
        );

    STDMETHOD_(UINT32, GetLocalToken)(
        THIS
        ) PURE;

    STDMETHOD_(UINT32, GetRemoteToken)(
        THIS
        ) PURE;
};


//
// QueuePair
//
#undef INTERFACE
#define INTERFACE IND2QueuePair

// {EEF2F332-B75D-4063-BCE3-3A0BAD2D02CE}
DEFINE_GUID(IID_IND2QueuePair, 
0xeef2f332, 0xb75d, 0x4063, 0xbc, 0xe3, 0x3a, 0xb, 0xad, 0x2d, 0x2, 0xce);

DECLARE_INTERFACE_(IND2QueuePair, IUnknown)
{
    // *** IUnknown methods ***
    IFACEMETHOD(QueryInterface)(
        THIS_
        REFIID riid,
        _Deref_out_ LPVOID* ppvObj
        ) PURE;

    IFACEMETHOD_(ULONG,AddRef)(
        THIS
        ) PURE;

    IFACEMETHOD_(ULONG,Release)(
        THIS
        ) PURE;

    // *** IND2QueuePair methods ***
    STDMETHOD(Flush)(
        THIS
        ) PURE;

#define ND_OP_FLAG_SILENT_SUCCESS           0x00000001
#define ND_OP_FLAG_READ_FENCE               0x00000002
#define ND_OP_FLAG_SEND_AND_SOLICIT_EVENT   0x00000004
#define ND_OP_FLAG_ALLOW_READ               0x00000008
#define ND_OP_FLAG_ALLOW_WRITE              0x00000010
#if NDVER >= ND_VERSION_2
#define ND_OP_FLAG_INLINE                   0x00000020
#endif

    STDMETHOD(Send)(
        THIS_
        _In_opt_ VOID* requestContext,
        _In_opt_count_(nSge) const ND2_SGE pSge[],
        ULONG nSge,
        ULONG flags
        ) PURE;

    STDMETHOD(Receive)(
        THIS_
        _In_opt_ VOID* requestContext,
        _In_opt_count_(nSge) const ND2_SGE pSge[],
        ULONG nSge
        ) PURE;

    // RemoteToken available thorugh IND2Mw::GetRemoteToken.
    STDMETHOD(Bind)(
        THIS_
        _In_opt_ VOID* requestContext,
        _In_ IUnknown* pMemoryRegion,
        _Inout_ IUnknown* pMemoryWindow,
        _In_bytecount_(cbBuffer) const VOID* pBuffer,
        SIZE_T cbBuffer,
        ULONG flags
        ) PURE;

    STDMETHOD(Invalidate)(
        THIS_
        _In_opt_ VOID* requestContext,
        _In_ IUnknown* pMemoryWindow,
        ULONG flags
        ) PURE;

    STDMETHOD(Read)(
        THIS_
        _In_opt_ VOID* requestContext,
        _In_opt_count_(nSge) const ND2_SGE pSge[],
        ULONG nSge,
        UINT64 remoteAddress,
        UINT32 remoteToken,
        ULONG flags
        ) PURE;

    STDMETHOD(Write)(
        THIS_
        _In_opt_ VOID* requestContext,
        _In_opt_count_(nSge) const ND2_SGE pSge[],
        ULONG nSge,
        UINT64 remoteAddress,
        UINT32 remoteToken,
        ULONG flags
        ) PURE;
};


//
// Connector
//
#undef INTERFACE
#define INTERFACE IND2Connector

// {CF369E3D-8019-4761-A323-A7E027426DE7}
DEFINE_GUID(IID_IND2Connector, 
0xcf369e3d, 0x8019, 0x4761, 0xa3, 0x23, 0xa7, 0xe0, 0x27, 0x42, 0x6d, 0xe7);

DECLARE_INTERFACE_(IND2Connector, IND2Overlapped)
{
    // *** IUnknown methods ***
    IFACEMETHOD(QueryInterface)(
        THIS_
        REFIID riid,
        _Deref_out_ LPVOID* ppvObj
        ) PURE;

    IFACEMETHOD_(ULONG,AddRef)(
        THIS
        ) PURE;

    IFACEMETHOD_(ULONG,Release)(
        THIS
        ) PURE;

    // *** IND2Overlapped methods ***
    IFACEMETHOD(CancelOverlappedRequests)(
        THIS
        ) PURE;

    IFACEMETHOD(GetOverlappedResult)(
        THIS_
        _In_ OVERLAPPED* pOverlapped,
        BOOL wait
        ) PURE;

    // *** IND2Connector methods ***
    STDMETHOD(Connect)(
        THIS_
        _In_ IUnknown* pQueuePair,
        _In_bytecount_(cbSrcAddress) const struct sockaddr* pSrcAddress,
        SIZE_T cbSrcAddress,
        _In_bytecount_(cbDestAddress) const struct sockaddr* pDestAddress,
        SIZE_T cbDestAddress,
        ULONG inboundReadLimit,
        ULONG outboundReadLimit,
        _In_opt_bytecount_(cbPrivateData) const VOID* pPrivateData,
        ULONG cbPrivateData,
        _Inout_ OVERLAPPED* pOverlapped
        ) PURE;

    STDMETHOD(CompleteConnect)(
        THIS_
        _Inout_ OVERLAPPED* pOverlapped
        ) PURE;

    STDMETHOD(Accept)(
        THIS_
        _In_ IUnknown* pQueuePair,
        ULONG inboundReadLimit,
        ULONG outboundReadLimit,
        _In_opt_bytecount_(cbPrivateData) const VOID* pPrivateData,
        ULONG cbPrivateData,
        _Inout_ OVERLAPPED* pOverlapped
        ) PURE;

    STDMETHOD(Reject)(
        THIS_
        _In_opt_bytecount_(cbPrivateData) const VOID* pPrivateData,
        ULONG cbPrivateData
        ) PURE;

    STDMETHOD(GetReadLimits)(
        THIS_
        _Out_opt_ ULONG* pInboundReadLimit,
        _Out_opt_ ULONG* pOutboundReadLimit
        ) PURE;

    STDMETHOD(GetPrivateData)(
        THIS_
        _Out_opt_bytecap_(*pcbPrivateData) VOID* pPrivateData,
        _Inout_ ULONG* pcbPrivateData
        ) PURE;

    STDMETHOD(GetLocalAddress)(
        THIS_
        _Out_opt_bytecap_post_bytecount_(*pcbAddress, *pcbAddress) struct sockaddr* pAddress,
        _Inout_ SIZE_T* pcbAddress
        ) PURE;

    STDMETHOD(GetPeerAddress)(
        THIS_
        _Out_opt_bytecap_post_bytecount_(*pcbAddress, *pcbAddress) struct sockaddr* pAddress,
        _Inout_ SIZE_T* pcbAddress
        ) PURE;

    STDMETHOD(NotifyDisconnect)(
        THIS_
        _Inout_ OVERLAPPED* pOverlapped
        ) PURE;

    STDMETHOD(Disconnect)(
        THIS_
        _Inout_ OVERLAPPED* pOverlapped
        ) PURE;
};


//
// Listen
//
#undef INTERFACE
#define INTERFACE IND2Listen

// {34284CA6-C03E-41cd-8424-972478DC4BA5}
DEFINE_GUID(IID_IND2Listen, 
0x34284ca6, 0xc03e, 0x41cd, 0x84, 0x24, 0x97, 0x24, 0x78, 0xdc, 0x4b, 0xa5);

DECLARE_INTERFACE_(IND2Listen, IND2Overlapped)
{
    // *** IUnknown methods ***
    IFACEMETHOD(QueryInterface)(
        THIS_
        REFIID riid,
        _Deref_out_ LPVOID* ppvObj
        ) PURE;

    IFACEMETHOD_(ULONG,AddRef)(
        THIS
        ) PURE;

    IFACEMETHOD_(ULONG,Release)(
        THIS
        ) PURE;

    // *** IND2Overlapped methods ***
    IFACEMETHOD(CancelOverlappedRequests)(
        THIS
        ) PURE;

    IFACEMETHOD(GetOverlappedResult)(
        THIS_
        _In_ OVERLAPPED* pOverlapped,
        BOOL wait
        ) PURE;

    // *** IND2Listen methods ***
    STDMETHOD(Listen)(
        THIS_
        _In_bytecount_(cbAddress) const struct sockaddr* pAddress,
        SIZE_T cbAddress,
        SIZE_T backlog
        ) PURE;

    STDMETHOD(GetLocalAddress)(
        THIS_
        _Out_opt_bytecap_post_bytecount_(*pcbAddress, *pcbAddress) struct sockaddr* pAddress,
        _Inout_ SIZE_T* pcbAddress
        ) PURE;

    STDMETHOD(GetConnectionRequest)(
        THIS_
        _Inout_ IUnknown* pConnector,
        _Inout_ OVERLAPPED* pOverlapped
        ) PURE;
};


//
// Adapter
//
#undef INTERFACE
#define INTERFACE IND2Adapter

// {F7329B18-B3A9-4a8b-A6A3-1956F93950A8}
DEFINE_GUID(IID_IND2Adapter, 
0xf7329b18, 0xb3a9, 0x4a8b, 0xa6, 0xa3, 0x19, 0x56, 0xf9, 0x39, 0x50, 0xa8);

DECLARE_INTERFACE_(IND2Adapter, IUnknown)
{
    // *** IUnknown methods ***
    IFACEMETHOD(QueryInterface)(
        THIS_
        REFIID riid,
        _Deref_out_ LPVOID* ppvObj
        ) PURE;

    IFACEMETHOD_(ULONG,AddRef)(
        THIS
        ) PURE;

    IFACEMETHOD_(ULONG,Release)(
        THIS
        ) PURE;

    // *** IND2Adapter methods ***
    STDMETHOD(CreateOverlappedFile)(
        THIS_
        _Deref_out_ HANDLE* phOverlappedFile
        ) PURE;

    STDMETHOD(Query)(
        THIS_
        _Inout_opt_bytecap_(*pcbInfo) ND2_ADAPTER_INFO* pInfo,
        _Inout_ SIZE_T* pcbInfo
        ) PURE;

    STDMETHOD(QueryAddressList)(
        THIS_
        _Out_opt_bytecap_post_bytecount_(*pcbAddressList, *pcbAddressList) SOCKET_ADDRESS_LIST* pAddressList,
        _Inout_ SIZE_T* pcbAddressList
        ) PURE;

    STDMETHOD(CreateCompletionQueue)(
        THIS_
        _In_ REFIID iid,
        _In_ HANDLE hOverlappedFile,
        ULONG queueDepth,
        USHORT group,
        KAFFINITY affinity,
        _Deref_out_ VOID** ppCompletionQueue
        ) PURE;

    STDMETHOD(CreateMemoryRegion)(
        THIS_
        _In_ REFIID iid,
        _In_ HANDLE hOverlappedFile,
        _Deref_out_ VOID** ppMemoryRegion
        ) PURE;

    STDMETHOD(CreateMemoryWindow)(
        THIS_
        _In_ REFIID iid,
        _Deref_out_ VOID** ppMemoryWindow
        ) PURE;

    STDMETHOD(CreateSharedReceiveQueue)(
        THIS_
        _In_ REFIID iid,
        _In_ HANDLE hOverlappedFile,
        ULONG queueDepth,
        ULONG maxSge,
        ULONG notifyThreshold,
        USHORT group,
        KAFFINITY affinity,
        _Deref_out_ VOID** ppSharedReceiveQueue
        ) PURE;

    STDMETHOD(CreateQueuePair)(
        THIS_
        _In_ REFIID iid,
        _In_ IUnknown* pReceiveCompletionQueue,
        _In_ IUnknown* pInitiatorCompletionQueue,
        _In_opt_ VOID* context,
        ULONG receiveQueueDepth,
        ULONG initiatorQueueDepth,
        ULONG maxReceiveRequestSge,
        ULONG maxInitiatorRequestSge,
        _Deref_out_ VOID** ppQueuePair
        ) PURE;

    STDMETHOD(CreateQueuePairWithSrq)(
        THIS_
        _In_ REFIID iid,
        _In_ IUnknown* pReceiveCompletionQueue,
        _In_ IUnknown* pInitiatorCompletionQueue,
        _In_ IUnknown* pSharedReceiveQueue,
        _In_opt_ VOID* context,
        ULONG initiatorQueueDepth,
        ULONG maxInitiatorRequestSge,
        _Deref_out_ VOID** ppQueuePair
        ) PURE;

    STDMETHOD(CreateConnector)(
        THIS_
        _In_ REFIID iid,
        _In_ HANDLE hOverlappedFile,
        _Deref_out_ VOID** ppConnector
        ) PURE;

    STDMETHOD(CreateListen)(
        THIS_
        _In_ REFIID iid,
        _In_ HANDLE hOverlappedFile,
        _Deref_out_ VOID** ppListen
        ) PURE;
};


//
// Provider
//
#undef INTERFACE
#define INTERFACE IND2Provider

// {4E6A7630-B317-47eb-969F-3AE42F39B0E3}
DEFINE_GUID(IID_IND2Provider, 
0x4e6a7630, 0xb317, 0x47eb, 0x96, 0x9f, 0x3a, 0xe4, 0x2f, 0x39, 0xb0, 0xe3);

DECLARE_INTERFACE_(IND2Provider, IUnknown)
{
    // *** IUnknown methods ***
    IFACEMETHOD(QueryInterface)(
        THIS_
        REFIID riid,
        _Deref_out_ LPVOID* ppvObj
        ) PURE;

    IFACEMETHOD_(ULONG,AddRef)(
        THIS
        ) PURE;

    IFACEMETHOD_(ULONG,Release)(
        THIS
        ) PURE;

    // *** IND2Provider methods ***
    STDMETHOD(QueryAddressList)(
        THIS_
        _Out_opt_bytecap_post_bytecount_(*pcbAddressList, *pcbAddressList) SOCKET_ADDRESS_LIST* pAddressList,
        _Inout_ SIZE_T* pcbAddressList
        ) PURE;

    STDMETHOD(ResolveAddress)(
        THIS_
        _In_bytecount_(cbAddress) const struct sockaddr* pAddress,
        SIZE_T cbAddress,
        _Out_ UINT64* pAdapterId
        ) PURE;

    STDMETHOD(OpenAdapter)(
        THIS_
        _In_ REFIID iid,
        UINT64 adapterId,
        _Deref_out_ VOID** ppAdapter
        ) PURE;
};


///////////////////////////////////////////////////////////////////////////////
//
// HPC Pack 2008 SDK interface
//
///////////////////////////////////////////////////////////////////////////////

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
    IFACEMETHOD(QueryInterface)(
        THIS_
        REFIID riid,
        _Deref_out_ LPVOID* ppvObj
        ) PURE;

    IFACEMETHOD_(ULONG,AddRef)(
        THIS
        ) PURE;

    IFACEMETHOD_(ULONG,Release)(
        THIS
        ) PURE;

    // *** INDOverlapped methods ***
    STDMETHOD(CancelOverlappedRequests)(
        THIS
        ) PURE;

    STDMETHOD(GetOverlappedResult)(
        THIS_
        _Inout_ OVERLAPPED *pOverlapped,
        _Out_ SIZE_T *pNumberOfBytesTransferred,
        BOOL bWait
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
    IFACEMETHOD(QueryInterface)(
        THIS_
        REFIID riid,
        _Deref_out_ LPVOID* ppvObj
        ) PURE;

    IFACEMETHOD_(ULONG,AddRef)(
        THIS
        ) PURE;

    IFACEMETHOD_(ULONG,Release)(
        THIS
        ) PURE;

    // *** INDOverlapped methods ***
    IFACEMETHOD(CancelOverlappedRequests)(
        THIS
        ) PURE;

    IFACEMETHOD(GetOverlappedResult)(
        THIS_
        _Inout_ OVERLAPPED *pOverlapped,
        _Out_ SIZE_T *pNumberOfBytesTransferred,
        BOOL bWait
        ) PURE;

    // *** INDCompletionQueue methods ***
    STDMETHOD(Resize)(
        THIS_
        SIZE_T nEntries
        ) PURE;

#define ND_CQ_NOTIFY_ERRORS     0
#define ND_CQ_NOTIFY_ANY        1
#define ND_CQ_NOTIFY_SOLICITED  2

    STDMETHOD(Notify)(
        THIS_
        DWORD Type,
        _Inout_ OVERLAPPED* pOverlapped
        ) PURE;

    STDMETHOD_(SIZE_T,GetResults)(
        THIS_
        _Out_cap_post_count_(nResults,return) ND_RESULT* pResults[],
        SIZE_T nResults
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
    IFACEMETHOD(QueryInterface)(
        THIS_
        REFIID riid,
        _Deref_out_ LPVOID* ppvObj
        ) PURE;

    IFACEMETHOD_(ULONG,AddRef)(
        THIS
        ) PURE;

    IFACEMETHOD_(ULONG,Release)(
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
    IFACEMETHOD(QueryInterface)(
        THIS_
        REFIID riid,
        _Deref_out_ LPVOID* ppvObj
        ) PURE;

    IFACEMETHOD_(ULONG,AddRef)(
        THIS
        ) PURE;

    IFACEMETHOD_(ULONG,Release)(
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

    STDMETHOD(Send)(
        THIS_
        _Out_ ND_RESULT* pResult,
        _In_opt_count_(nSge) const ND_SGE* pSgl,
        SIZE_T nSge,
        DWORD Flags
        ) PURE;

    STDMETHOD(SendAndInvalidate)(
        THIS_
        _Out_ ND_RESULT* pResult,
        _In_opt_count_(nSge) const ND_SGE* pSgl,
        SIZE_T nSge,
        _In_ const ND_MW_DESCRIPTOR* pRemoteMwDescriptor,
        DWORD Flags
        ) PURE;

    STDMETHOD(Receive)(
        THIS_
        _Out_ ND_RESULT* pResult,
        _In_opt_count_(nSge) const ND_SGE* pSgl,
        SIZE_T nSge
        ) PURE;

    STDMETHOD(Bind)(
        THIS_
        _Out_ ND_RESULT* pResult,
        _In_ ND_MR_HANDLE hMr,
        _In_ INDMemoryWindow* pMw,
        _In_bytecount_(BufferSize) const void* pBuffer,
        SIZE_T BufferSize,
        DWORD Flags,
        _Out_ ND_MW_DESCRIPTOR* pMwDescriptor
        ) PURE;

    STDMETHOD(Invalidate)(
        THIS_
        _Out_ ND_RESULT* pResult,
        _In_ INDMemoryWindow* pMw,
        DWORD Flags
        ) PURE;

    STDMETHOD(Read)(
        THIS_
        _Out_ ND_RESULT* pResult,
        _In_opt_count_(nSge) const ND_SGE* pSgl,
        SIZE_T nSge,
        _In_ const ND_MW_DESCRIPTOR* pRemoteMwDescriptor,
        ULONGLONG Offset,
        DWORD Flags
        ) PURE;

    STDMETHOD(Write)(
        THIS_
        _Out_ ND_RESULT* pResult,
        _In_opt_count_(nSge) const ND_SGE* pSgl,
        SIZE_T nSge,
        _In_ const ND_MW_DESCRIPTOR* pRemoteMwDescriptor,
        ULONGLONG Offset,
        DWORD Flags
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
    IFACEMETHOD(QueryInterface)(
        THIS_
        REFIID riid,
        _Deref_out_ LPVOID* ppvObj
        ) PURE;

    IFACEMETHOD_(ULONG,AddRef)(
        THIS
        ) PURE;

    IFACEMETHOD_(ULONG,Release)(
        THIS
        ) PURE;

    // *** INDOverlapped methods ***
    IFACEMETHOD(CancelOverlappedRequests)(
        THIS
        ) PURE;

    IFACEMETHOD(GetOverlappedResult)(
        THIS_
        _Inout_ OVERLAPPED *pOverlapped,
        _Out_ SIZE_T *pNumberOfBytesTransferred,
        BOOL bWait
        ) PURE;

    // *** INDConnector methods ***
    STDMETHOD(CreateEndpoint)(
        THIS_
        _In_ INDCompletionQueue* pInboundCq,
        _In_ INDCompletionQueue* pOutboundCq,
        SIZE_T nInboundEntries,
        SIZE_T nOutboundEntries,
        SIZE_T nInboundSge,
        SIZE_T nOutboundSge,
        SIZE_T InboundReadLimit,
        SIZE_T OutboundReadLimit,
        _Out_opt_ SIZE_T* pMaxInlineData,
        _Deref_out_ INDEndpoint** ppEndpoint
        ) PURE;

    STDMETHOD(Connect)(
        THIS_
        _In_ INDEndpoint* pEndpoint,
        _In_bytecount_(AddressLength) const struct sockaddr* pAddress,
        SIZE_T AddressLength,
        INT Protocol,
        USHORT LocalPort,
        _In_opt_bytecount_(PrivateDataLength) const void* pPrivateData,
        SIZE_T PrivateDataLength,
        _Inout_ OVERLAPPED* pOverlapped
        ) PURE;

    STDMETHOD(CompleteConnect)(
        THIS_
        _Inout_ OVERLAPPED* pOverlapped
        ) PURE;

    STDMETHOD(Accept)(
        THIS_
        _In_ INDEndpoint* pEndpoint,
        _In_opt_bytecount_(PrivateDataLength) const void* pPrivateData,
        SIZE_T PrivateDataLength,
        _Inout_ OVERLAPPED* pOverlapped
        ) PURE;

    STDMETHOD(Reject)(
        THIS_
        _In_opt_bytecount_(PrivateDataLength) const void* pPrivateData,
        SIZE_T PrivateDataLength
        ) PURE;

    STDMETHOD(GetConnectionData)(
        THIS_
        _Out_opt_ SIZE_T* pInboundReadLimit,
        _Out_opt_ SIZE_T* pOutboundReadLimit,
        _Out_opt_bytecap_post_bytecount_(*pPrivateDataLength, *pPrivateDataLength) void* pPrivateData,
        _Inout_opt_ SIZE_T* pPrivateDataLength
        ) PURE;

    STDMETHOD(GetLocalAddress)(
        THIS_
        _Out_opt_bytecap_post_bytecount_(*pAddressLength, *pAddressLength) struct sockaddr* pAddress,
        _Inout_ SIZE_T* pAddressLength
        ) PURE;

    STDMETHOD(GetPeerAddress)(
        THIS_
        _Out_opt_bytecap_post_bytecount_(*pAddressLength, *pAddressLength) struct sockaddr* pAddress,
        _Inout_ SIZE_T* pAddressLength
        ) PURE;

    STDMETHOD(NotifyDisconnect)(
        THIS_
        _Inout_ OVERLAPPED* pOverlapped
        ) PURE;

    STDMETHOD(Disconnect)(
        THIS_
        _Inout_ OVERLAPPED* pOverlapped
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
    IFACEMETHOD(QueryInterface)(
        THIS_
        REFIID riid,
        _Deref_out_ LPVOID* ppvObj
        ) PURE;

    IFACEMETHOD_(ULONG,AddRef)(
        THIS
        ) PURE;

    IFACEMETHOD_(ULONG,Release)(
        THIS
        ) PURE;

    // *** INDOverlapped methods ***
    IFACEMETHOD(CancelOverlappedRequests)(
        THIS
        ) PURE;

    IFACEMETHOD(GetOverlappedResult)(
        THIS_
        _Inout_ OVERLAPPED *pOverlapped,
        _Out_ SIZE_T *pNumberOfBytesTransferred,
        BOOL bWait
        ) PURE;

    // *** INDListen methods ***
    STDMETHOD(GetConnectionRequest)(
        THIS_
        _Inout_ INDConnector* pConnector,
        _Inout_ OVERLAPPED* pOverlapped
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
    IFACEMETHOD(QueryInterface)(
        THIS_
        REFIID riid,
        _Deref_out_ LPVOID* ppvObj
        ) PURE;

    IFACEMETHOD_(ULONG,AddRef)(
        THIS
        ) PURE;

    IFACEMETHOD_(ULONG,Release)(
        THIS
        ) PURE;

    // *** INDOverlapped methods ***
    IFACEMETHOD(CancelOverlappedRequests)(
        THIS
        ) PURE;

    IFACEMETHOD(GetOverlappedResult)(
        THIS_
        _Inout_ OVERLAPPED *pOverlapped,
        _Out_ SIZE_T *pNumberOfBytesTransferred,
        BOOL bWait
        ) PURE;

    // *** INDAdapter methods ***
    STDMETHOD_(HANDLE,GetFileHandle)(
        THIS
        ) PURE;

    STDMETHOD(Query)(
        THIS_
        DWORD VersionRequested,
        _Out_opt_bytecap_post_bytecount_(*pBufferSize, *pBufferSize) ND_ADAPTER_INFO* pInfo,
        _Inout_opt_ SIZE_T* pBufferSize
        ) PURE;

    STDMETHOD(Control)(
        THIS_
        DWORD IoControlCode,
        _In_opt_bytecount_(InBufferSize) const void* pInBuffer,
        SIZE_T InBufferSize,
        _Out_opt_bytecap_(OutBufferSize) void* pOutBuffer,
        SIZE_T OutBufferSize,
        _Out_ SIZE_T* pBytesReturned,
        _Inout_ OVERLAPPED* pOverlapped
        ) PURE;

    STDMETHOD(CreateCompletionQueue)(
        THIS_
        SIZE_T nEntries,
        _Deref_out_ INDCompletionQueue** ppCq
        ) PURE;

    STDMETHOD(RegisterMemory)(
        THIS_
        _In_bytecount_(BufferSize) const void* pBuffer,
        SIZE_T BufferSize,
        _Inout_ OVERLAPPED* pOverlapped,
        _Deref_out_ ND_MR_HANDLE* phMr
        ) PURE;

    STDMETHOD(DeregisterMemory)(
        THIS_
        _In_ ND_MR_HANDLE hMr,
        _Inout_ OVERLAPPED* pOverlapped
        ) PURE;

    STDMETHOD(CreateMemoryWindow)(
        THIS_
        _Out_ ND_RESULT* pInvalidateResult,
        _Deref_out_ INDMemoryWindow** ppMw
        ) PURE;

    STDMETHOD(CreateConnector)(
        THIS_
        _Deref_out_ INDConnector** ppConnector
        ) PURE;

    STDMETHOD(Listen)(
        THIS_
        SIZE_T Backlog,
        INT Protocol,
        USHORT Port,
        _Out_opt_ USHORT* pAssignedPort,
        _Deref_out_ INDListen** ppListen
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
    IFACEMETHOD(QueryInterface)(
        THIS_
        REFIID riid,
        _Deref_out_ LPVOID* ppvObj
        ) PURE;

    IFACEMETHOD_(ULONG,AddRef)(
        THIS
        ) PURE;

    IFACEMETHOD_(ULONG,Release)(
        THIS
        ) PURE;

    // *** INDProvider methods ***
    STDMETHOD(QueryAddressList)(
        THIS_
        _Out_opt_bytecap_post_bytecount_(*pBufferSize, *pBufferSize) SOCKET_ADDRESS_LIST* pAddressList,
        _Inout_ SIZE_T* pBufferSize
        ) PURE;

    STDMETHOD(OpenAdapter)(
        THIS_
        _In_bytecount_(AddressLength) const struct sockaddr* pAddress,
        SIZE_T AddressLength,
        _Deref_out_ INDAdapter** ppAdapter
        ) PURE;
};

//
// Map version 1 error values to version 2.
//
#define ND_LOCAL_LENGTH         ND_DATA_OVERRUN
#define ND_INVALIDATION_ERROR   ND_INVALID_DEVICE_REQUEST

#endif // _NDSPI_H_
