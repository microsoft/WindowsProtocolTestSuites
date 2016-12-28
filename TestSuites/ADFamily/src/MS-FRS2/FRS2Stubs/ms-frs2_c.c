// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/* this ALWAYS GENERATED file contains the RPC client stubs */


 /* File created by MIDL compiler version 8.00.0595 */
/* at Thu Mar 19 14:51:41 2015
 */
/* Compiler settings for ms-frs2.idl:
    Oicf, W1, Zp8, env=Win64 (32b run), target_arch=AMD64 8.00.0595 
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
/* @@MIDL_FILE_HEADING(  ) */

#if defined(_M_AMD64)


#pragma warning( disable: 4049 )  /* more than 64k source lines */
#if _MSC_VER >= 1200
#pragma warning(push)
#endif

#pragma warning( disable: 4211 )  /* redefine extern to static */
#pragma warning( disable: 4232 )  /* dllimport identity*/
#pragma warning( disable: 4024 )  /* array to pointer mapping*/

#include <string.h>

#include "ms-frs2.h"

#define TYPE_FORMAT_STRING_SIZE   841                               
#define PROC_FORMAT_STRING_SIZE   1125                              
#define EXPR_FORMAT_STRING_SIZE   1                                 
#define TRANSMIT_AS_TABLE_SIZE    0            
#define WIRE_MARSHAL_TABLE_SIZE   0            

typedef struct _ms2Dfrs2_MIDL_TYPE_FORMAT_STRING
    {
    short          Pad;
    unsigned char  Format[ TYPE_FORMAT_STRING_SIZE ];
    } ms2Dfrs2_MIDL_TYPE_FORMAT_STRING;

typedef struct _ms2Dfrs2_MIDL_PROC_FORMAT_STRING
    {
    short          Pad;
    unsigned char  Format[ PROC_FORMAT_STRING_SIZE ];
    } ms2Dfrs2_MIDL_PROC_FORMAT_STRING;

typedef struct _ms2Dfrs2_MIDL_EXPR_FORMAT_STRING
    {
    long          Pad;
    unsigned char  Format[ EXPR_FORMAT_STRING_SIZE ];
    } ms2Dfrs2_MIDL_EXPR_FORMAT_STRING;


static const RPC_SYNTAX_IDENTIFIER  _RpcTransferSyntax = 
{{0x8A885D04,0x1CEB,0x11C9,{0x9F,0xE8,0x08,0x00,0x2B,0x10,0x48,0x60}},{2,0}};


extern const ms2Dfrs2_MIDL_TYPE_FORMAT_STRING ms2Dfrs2__MIDL_TypeFormatString;
extern const ms2Dfrs2_MIDL_PROC_FORMAT_STRING ms2Dfrs2__MIDL_ProcFormatString;
extern const ms2Dfrs2_MIDL_EXPR_FORMAT_STRING ms2Dfrs2__MIDL_ExprFormatString;

#define GENERIC_BINDING_TABLE_SIZE   0            


/* Standard interface: __MIDL_itf_ms2Dfrs2_0000_0000, ver. 0.0,
   GUID={0x00000000,0x0000,0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}} */


/* Standard interface: FrsTransport, ver. 1.0,
   GUID={0x897e2e5f,0x93f3,0x4376,{0x9c,0x9c,0xfd,0x22,0x77,0x49,0x5c,0x27}} */



static const RPC_CLIENT_INTERFACE FrsTransport___RpcClientInterface =
    {
    sizeof(RPC_CLIENT_INTERFACE),
    {{0x897e2e5f,0x93f3,0x4376,{0x9c,0x9c,0xfd,0x22,0x77,0x49,0x5c,0x27}},{1,0}},
    {{0x8A885D04,0x1CEB,0x11C9,{0x9F,0xE8,0x08,0x00,0x2B,0x10,0x48,0x60}},{2,0}},
    0,
    0,
    0,
    0,
    0,
    0x00000001
    };
RPC_IF_HANDLE FrsTransport_v1_0_c_ifspec = (RPC_IF_HANDLE)& FrsTransport___RpcClientInterface;

extern const MIDL_STUB_DESC FrsTransport_StubDesc;

static RPC_BINDING_HANDLE FrsTransport__MIDL_AutoBindHandle;


DWORD CheckConnectivity( 
    /* [in] */ handle_t IDL_handle,
    /* [in] */ FRS_REPLICA_SET_ID replicaSetId,
    /* [in] */ FRS_CONNECTION_ID connectionId)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&FrsTransport_StubDesc,
                  (PFORMAT_STRING) &ms2Dfrs2__MIDL_ProcFormatString.Format[0],
                  IDL_handle,
                  replicaSetId,
                  connectionId);
    return ( DWORD  )_RetVal.Simple;
    
}


DWORD EstablishConnection( 
    /* [in] */ handle_t IDL_handle,
    /* [in] */ FRS_REPLICA_SET_ID replicaSetId,
    /* [in] */ FRS_CONNECTION_ID connectionId,
    /* [in] */ DWORD downstreamProtocolVersion,
    /* [in] */ DWORD downstreamFlags,
    /* [out] */ DWORD *upstreamProtocolVersion,
    /* [out] */ DWORD *upstreamFlags)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&FrsTransport_StubDesc,
                  (PFORMAT_STRING) &ms2Dfrs2__MIDL_ProcFormatString.Format[48],
                  IDL_handle,
                  replicaSetId,
                  connectionId,
                  downstreamProtocolVersion,
                  downstreamFlags,
                  upstreamProtocolVersion,
                  upstreamFlags);
    return ( DWORD  )_RetVal.Simple;
    
}


DWORD EstablishSession( 
    /* [in] */ handle_t IDL_handle,
    /* [in] */ FRS_CONNECTION_ID connectionId,
    /* [in] */ FRS_CONTENT_SET_ID contentSetId)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&FrsTransport_StubDesc,
                  (PFORMAT_STRING) &ms2Dfrs2__MIDL_ProcFormatString.Format[120],
                  IDL_handle,
                  connectionId,
                  contentSetId);
    return ( DWORD  )_RetVal.Simple;
    
}


/* [async] */ void  RequestUpdates( 
    /* [in] */ PRPC_ASYNC_STATE RequestUpdates_AsyncHandle,
    /* [in] */ handle_t IDL_handle,
    /* [in] */ FRS_CONNECTION_ID connectionId,
    /* [in] */ FRS_CONTENT_SET_ID contentSetId,
    /* [range][in] */ DWORD creditsAvailable,
    /* [range][in] */ long hashRequested,
    /* [range][in] */ UPDATE_REQUEST_TYPE updateRequestType,
    /* [in] */ unsigned long versionVectorDiffCount,
    /* [size_is][in] */ FRS_VERSION_VECTOR *versionVectorDiff,
    /* [length_is][size_is][out] */ FRS_UPDATE *frsUpdate,
    /* [out] */ DWORD *updateCount,
    /* [out] */ UPDATE_STATUS *updateStatus,
    /* [out] */ GUID *gvsnDbGuid,
    /* [out] */ DWORDLONG *gvsnVersion)
{

    NdrAsyncClientCall(
                      ( PMIDL_STUB_DESC  )&FrsTransport_StubDesc,
                      (PFORMAT_STRING) &ms2Dfrs2__MIDL_ProcFormatString.Format[168],
                      RequestUpdates_AsyncHandle,
                      IDL_handle,
                      connectionId,
                      contentSetId,
                      creditsAvailable,
                      hashRequested,
                      updateRequestType,
                      versionVectorDiffCount,
                      versionVectorDiff,
                      frsUpdate,
                      updateCount,
                      updateStatus,
                      gvsnDbGuid,
                      gvsnVersion);
    
}


/* [async] */ void  RequestVersionVector( 
    /* [in] */ PRPC_ASYNC_STATE RequestVersionVector_AsyncHandle,
    /* [in] */ handle_t IDL_handle,
    /* [in] */ DWORD sequenceNumber,
    /* [in] */ FRS_CONNECTION_ID connectionId,
    /* [in] */ FRS_CONTENT_SET_ID contentSetId,
    /* [range][in] */ VERSION_REQUEST_TYPE requestType,
    /* [range][in] */ VERSION_CHANGE_TYPE changeType,
    /* [in] */ ULONGLONG vvGeneration)
{

    NdrAsyncClientCall(
                      ( PMIDL_STUB_DESC  )&FrsTransport_StubDesc,
                      (PFORMAT_STRING) &ms2Dfrs2__MIDL_ProcFormatString.Format[276],
                      RequestVersionVector_AsyncHandle,
                      IDL_handle,
                      sequenceNumber,
                      connectionId,
                      contentSetId,
                      requestType,
                      changeType,
                      vvGeneration);
    
}


/* [async] */ void  AsyncPoll( 
    /* [in] */ PRPC_ASYNC_STATE AsyncPoll_AsyncHandle,
    /* [in] */ handle_t IDL_handle,
    /* [in] */ FRS_CONNECTION_ID connectionId,
    /* [out] */ FRS_ASYNC_RESPONSE_CONTEXT *response)
{

    NdrAsyncClientCall(
                      ( PMIDL_STUB_DESC  )&FrsTransport_StubDesc,
                      (PFORMAT_STRING) &ms2Dfrs2__MIDL_ProcFormatString.Format[348],
                      AsyncPoll_AsyncHandle,
                      IDL_handle,
                      connectionId,
                      response);
    
}


/* [async] */ void  RequestRecords( 
    /* [in] */ PRPC_ASYNC_STATE RequestRecords_AsyncHandle,
    /* [in] */ handle_t IDL_handle,
    /* [in] */ FRS_CONNECTION_ID connectionId,
    /* [in] */ FRS_CONTENT_SET_ID contentSetId,
    /* [in] */ FRS_DATABASE_ID uidDbGuid,
    /* [in] */ DWORDLONG uidVersion,
    /* [out][in] */ DWORD *maxRecords,
    /* [out] */ DWORD *numRecords,
    /* [out] */ DWORD *numBytes,
    /* [size_is][size_is][out] */ byte **compressedRecords,
    /* [out] */ RECORDS_STATUS *recordsStatus)
{

    NdrAsyncClientCall(
                      ( PMIDL_STUB_DESC  )&FrsTransport_StubDesc,
                      (PFORMAT_STRING) &ms2Dfrs2__MIDL_ProcFormatString.Format[396],
                      RequestRecords_AsyncHandle,
                      IDL_handle,
                      connectionId,
                      contentSetId,
                      uidDbGuid,
                      uidVersion,
                      maxRecords,
                      numRecords,
                      numBytes,
                      compressedRecords,
                      recordsStatus);
    
}


DWORD UpdateCancel( 
    /* [in] */ handle_t IDL_handle,
    /* [in] */ FRS_CONNECTION_ID connectionId,
    /* [in] */ FRS_UPDATE_CANCEL_DATA cancelData)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&FrsTransport_StubDesc,
                  (PFORMAT_STRING) &ms2Dfrs2__MIDL_ProcFormatString.Format[486],
                  IDL_handle,
                  connectionId,
                  cancelData);
    return ( DWORD  )_RetVal.Simple;
    
}


DWORD RawGetFileData( 
    /* [out][in] */ PFRS_SERVER_CONTEXT *serverContext,
    /* [length_is][size_is][out] */ byte *dataBuffer,
    /* [range][in] */ DWORD bufferSize,
    /* [out] */ DWORD *sizeRead,
    /* [out] */ long *isEndOfFile)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&FrsTransport_StubDesc,
                  (PFORMAT_STRING) &ms2Dfrs2__MIDL_ProcFormatString.Format[534],
                  serverContext,
                  dataBuffer,
                  bufferSize,
                  sizeRead,
                  isEndOfFile);
    return ( DWORD  )_RetVal.Simple;
    
}


DWORD RdcGetSignatures( 
    /* [in] */ PFRS_SERVER_CONTEXT serverContext,
    /* [range][in] */ byte level,
    /* [in] */ DWORDLONG offset,
    /* [length_is][size_is][out] */ byte *buffer,
    /* [range][in] */ DWORD length,
    /* [out] */ DWORD *sizeRead)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&FrsTransport_StubDesc,
                  (PFORMAT_STRING) &ms2Dfrs2__MIDL_ProcFormatString.Format[602],
                  serverContext,
                  level,
                  offset,
                  buffer,
                  length,
                  sizeRead);
    return ( DWORD  )_RetVal.Simple;
    
}


DWORD RdcPushSourceNeeds( 
    /* [in] */ PFRS_SERVER_CONTEXT serverContext,
    /* [size_is][in] */ FRS_RDC_SOURCE_NEED *sourceNeeds,
    /* [range][in] */ DWORD needCount)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&FrsTransport_StubDesc,
                  (PFORMAT_STRING) &ms2Dfrs2__MIDL_ProcFormatString.Format[676],
                  serverContext,
                  sourceNeeds,
                  needCount);
    return ( DWORD  )_RetVal.Simple;
    
}


DWORD RdcGetFileData( 
    /* [in] */ PFRS_SERVER_CONTEXT serverContext,
    /* [length_is][size_is][out] */ byte *dataBuffer,
    /* [range][in] */ DWORD bufferSize,
    /* [out] */ DWORD *sizeReturned)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&FrsTransport_StubDesc,
                  (PFORMAT_STRING) &ms2Dfrs2__MIDL_ProcFormatString.Format[732],
                  serverContext,
                  dataBuffer,
                  bufferSize,
                  sizeReturned);
    return ( DWORD  )_RetVal.Simple;
    
}


DWORD RdcClose( 
    /* [out][in] */ PFRS_SERVER_CONTEXT *serverContext)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&FrsTransport_StubDesc,
                  (PFORMAT_STRING) &ms2Dfrs2__MIDL_ProcFormatString.Format[794],
                  serverContext);
    return ( DWORD  )_RetVal.Simple;
    
}


/* [async] */ void  InitializeFileTransferAsync( 
    /* [in] */ PRPC_ASYNC_STATE InitializeFileTransferAsync_AsyncHandle,
    /* [in] */ handle_t IDL_handle,
    /* [in] */ FRS_CONNECTION_ID connectionId,
    /* [out][in] */ FRS_UPDATE *frsUpdate,
    /* [range][in] */ long rdcDesired,
    /* [out][in] */ FRS_REQUESTED_STAGING_POLICY *stagingPolicy,
    /* [out] */ PFRS_SERVER_CONTEXT *serverContext,
    /* [out] */ FRS_RDC_FILEINFO **rdcFileInfo,
    /* [length_is][size_is][out] */ byte *dataBuffer,
    /* [range][in] */ DWORD bufferSize,
    /* [out] */ DWORD *sizeRead,
    /* [out] */ long *isEndOfFile)
{

    NdrAsyncClientCall(
                      ( PMIDL_STUB_DESC  )&FrsTransport_StubDesc,
                      (PFORMAT_STRING) &ms2Dfrs2__MIDL_ProcFormatString.Format[838],
                      InitializeFileTransferAsync_AsyncHandle,
                      IDL_handle,
                      connectionId,
                      frsUpdate,
                      rdcDesired,
                      stagingPolicy,
                      serverContext,
                      rdcFileInfo,
                      dataBuffer,
                      bufferSize,
                      sizeRead,
                      isEndOfFile);
    
}


DWORD InitializeFileDataTransfer( 
    /* [in] */ handle_t IDL_handle,
    /* [in] */ FRS_CONNECTION_ID connectionId,
    /* [out][in] */ FRS_UPDATE *frsUpdate,
    /* [out] */ PFRS_SERVER_CONTEXT *serverContext,
    /* [in] */ DWORDLONG offset,
    /* [in] */ DWORDLONG length,
    /* [length_is][size_is][out] */ byte *dataBuffer,
    /* [range][in] */ DWORD bufferSize,
    /* [out] */ DWORD *sizeRead,
    /* [out] */ long *isEndOfFile)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&FrsTransport_StubDesc,
                  (PFORMAT_STRING) &ms2Dfrs2__MIDL_ProcFormatString.Format[934],
                  IDL_handle,
                  connectionId,
                  frsUpdate,
                  serverContext,
                  offset,
                  length,
                  dataBuffer,
                  bufferSize,
                  sizeRead,
                  isEndOfFile);
    return ( DWORD  )_RetVal.Simple;
    
}


/* [async] */ void  RawGetFileDataAsync( 
    /* [in] */ PRPC_ASYNC_STATE RawGetFileDataAsync_AsyncHandle,
    /* [in] */ PFRS_SERVER_CONTEXT serverContext,
    /* [out] */ ASYNC_BYTE_PIPE *bytePipe)
{

    NdrAsyncClientCall(
                      ( PMIDL_STUB_DESC  )&FrsTransport_StubDesc,
                      (PFORMAT_STRING) &ms2Dfrs2__MIDL_ProcFormatString.Format[1024],
                      RawGetFileDataAsync_AsyncHandle,
                      serverContext,
                      bytePipe);
    
}


/* [async] */ void  RdcGetFileDataAsync( 
    /* [in] */ PRPC_ASYNC_STATE RdcGetFileDataAsync_AsyncHandle,
    /* [in] */ PFRS_SERVER_CONTEXT serverContext,
    /* [out] */ ASYNC_BYTE_PIPE *bytePipe)
{

    NdrAsyncClientCall(
                      ( PMIDL_STUB_DESC  )&FrsTransport_StubDesc,
                      (PFORMAT_STRING) &ms2Dfrs2__MIDL_ProcFormatString.Format[1074],
                      RdcGetFileDataAsync_AsyncHandle,
                      serverContext,
                      bytePipe);
    
}


#if !defined(__RPC_WIN64__)
#error  Invalid build platform for this stub.
#endif

static const ms2Dfrs2_MIDL_PROC_FORMAT_STRING ms2Dfrs2__MIDL_ProcFormatString =
    {
        0,
        {

	/* Procedure CheckConnectivity */

			0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/*  2 */	NdrFcLong( 0x0 ),	/* 0 */
/*  6 */	NdrFcShort( 0x0 ),	/* 0 */
/*  8 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 10 */	0x32,		/* FC_BIND_PRIMITIVE */
			0x0,		/* 0 */
/* 12 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 14 */	NdrFcShort( 0x88 ),	/* 136 */
/* 16 */	NdrFcShort( 0x8 ),	/* 8 */
/* 18 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x3,		/* 3 */
/* 20 */	0xa,		/* 10 */
			0x81,		/* Ext Flags:  new corr desc, has big amd64 byval param */
/* 22 */	NdrFcShort( 0x0 ),	/* 0 */
/* 24 */	NdrFcShort( 0x0 ),	/* 0 */
/* 26 */	NdrFcShort( 0x0 ),	/* 0 */
/* 28 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter IDL_handle */

/* 30 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 32 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 34 */	NdrFcShort( 0xc ),	/* Type Offset=12 */

	/* Parameter replicaSetId */

/* 36 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 38 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 40 */	NdrFcShort( 0xc ),	/* Type Offset=12 */

	/* Parameter connectionId */

/* 42 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 44 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 46 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure EstablishConnection */


	/* Return value */

/* 48 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 50 */	NdrFcLong( 0x0 ),	/* 0 */
/* 54 */	NdrFcShort( 0x1 ),	/* 1 */
/* 56 */	NdrFcShort( 0x40 ),	/* X64 Stack size/offset = 64 */
/* 58 */	0x32,		/* FC_BIND_PRIMITIVE */
			0x0,		/* 0 */
/* 60 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 62 */	NdrFcShort( 0x98 ),	/* 152 */
/* 64 */	NdrFcShort( 0x40 ),	/* 64 */
/* 66 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x7,		/* 7 */
/* 68 */	0xa,		/* 10 */
			0x81,		/* Ext Flags:  new corr desc, has big amd64 byval param */
/* 70 */	NdrFcShort( 0x0 ),	/* 0 */
/* 72 */	NdrFcShort( 0x0 ),	/* 0 */
/* 74 */	NdrFcShort( 0x0 ),	/* 0 */
/* 76 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter IDL_handle */

/* 78 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 80 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 82 */	NdrFcShort( 0xc ),	/* Type Offset=12 */

	/* Parameter replicaSetId */

/* 84 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 86 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 88 */	NdrFcShort( 0xc ),	/* Type Offset=12 */

	/* Parameter connectionId */

/* 90 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 92 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 94 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter downstreamProtocolVersion */

/* 96 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 98 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 100 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter downstreamFlags */

/* 102 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 104 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 106 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter upstreamProtocolVersion */

/* 108 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 110 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 112 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter upstreamFlags */

/* 114 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 116 */	NdrFcShort( 0x38 ),	/* X64 Stack size/offset = 56 */
/* 118 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure EstablishSession */


	/* Return value */

/* 120 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 122 */	NdrFcLong( 0x0 ),	/* 0 */
/* 126 */	NdrFcShort( 0x2 ),	/* 2 */
/* 128 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 130 */	0x32,		/* FC_BIND_PRIMITIVE */
			0x0,		/* 0 */
/* 132 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 134 */	NdrFcShort( 0x88 ),	/* 136 */
/* 136 */	NdrFcShort( 0x8 ),	/* 8 */
/* 138 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x3,		/* 3 */
/* 140 */	0xa,		/* 10 */
			0x81,		/* Ext Flags:  new corr desc, has big amd64 byval param */
/* 142 */	NdrFcShort( 0x0 ),	/* 0 */
/* 144 */	NdrFcShort( 0x0 ),	/* 0 */
/* 146 */	NdrFcShort( 0x0 ),	/* 0 */
/* 148 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter IDL_handle */

/* 150 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 152 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 154 */	NdrFcShort( 0xc ),	/* Type Offset=12 */

	/* Parameter connectionId */

/* 156 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 158 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 160 */	NdrFcShort( 0xc ),	/* Type Offset=12 */

	/* Parameter contentSetId */

/* 162 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 164 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 166 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure RequestUpdates */


	/* Return value */

/* 168 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 170 */	NdrFcLong( 0x0 ),	/* 0 */
/* 174 */	NdrFcShort( 0x3 ),	/* 3 */
/* 176 */	NdrFcShort( 0x78 ),	/* X64 Stack size/offset = 120 */
/* 178 */	0x32,		/* FC_BIND_PRIMITIVE */
			0x0,		/* 0 */
/* 180 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 182 */	NdrFcShort( 0xa6 ),	/* 166 */
/* 184 */	NdrFcShort( 0xa6 ),	/* 166 */
/* 186 */	0xc7,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, has async handle */
			0xd,		/* 13 */
/* 188 */	0xa,		/* 10 */
			0x87,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, has big amd64 byval param */
/* 190 */	NdrFcShort( 0x1 ),	/* 1 */
/* 192 */	NdrFcShort( 0x1 ),	/* 1 */
/* 194 */	NdrFcShort( 0x0 ),	/* 0 */
/* 196 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter IDL_handle */

/* 198 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 200 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 202 */	NdrFcShort( 0xc ),	/* Type Offset=12 */

	/* Parameter connectionId */

/* 204 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 206 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 208 */	NdrFcShort( 0xc ),	/* Type Offset=12 */

	/* Parameter contentSetId */

/* 210 */	NdrFcShort( 0x88 ),	/* Flags:  in, by val, */
/* 212 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 214 */	NdrFcShort( 0x1c ),	/* 28 */

	/* Parameter creditsAvailable */

/* 216 */	NdrFcShort( 0x88 ),	/* Flags:  in, by val, */
/* 218 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 220 */	NdrFcShort( 0x26 ),	/* 38 */

	/* Parameter hashRequested */

/* 222 */	NdrFcShort( 0x88 ),	/* Flags:  in, by val, */
/* 224 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 226 */	NdrFcShort( 0x30 ),	/* 48 */

	/* Parameter updateRequestType */

/* 228 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 230 */	NdrFcShort( 0x38 ),	/* X64 Stack size/offset = 56 */
/* 232 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter versionVectorDiffCount */

/* 234 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 236 */	NdrFcShort( 0x40 ),	/* X64 Stack size/offset = 64 */
/* 238 */	NdrFcShort( 0x4a ),	/* Type Offset=74 */

	/* Parameter versionVectorDiff */

/* 240 */	NdrFcShort( 0x113 ),	/* Flags:  must size, must free, out, simple ref, */
/* 242 */	NdrFcShort( 0x48 ),	/* X64 Stack size/offset = 72 */
/* 244 */	NdrFcShort( 0xb6 ),	/* Type Offset=182 */

	/* Parameter frsUpdate */

/* 246 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 248 */	NdrFcShort( 0x50 ),	/* X64 Stack size/offset = 80 */
/* 250 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter updateCount */

/* 252 */	NdrFcShort( 0x2010 ),	/* Flags:  out, srv alloc size=8 */
/* 254 */	NdrFcShort( 0x58 ),	/* X64 Stack size/offset = 88 */
/* 256 */	NdrFcShort( 0xcc ),	/* Type Offset=204 */

	/* Parameter updateStatus */

/* 258 */	NdrFcShort( 0x4112 ),	/* Flags:  must free, out, simple ref, srv alloc size=16 */
/* 260 */	NdrFcShort( 0x60 ),	/* X64 Stack size/offset = 96 */
/* 262 */	NdrFcShort( 0xc ),	/* Type Offset=12 */

	/* Parameter gvsnDbGuid */

/* 264 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 266 */	NdrFcShort( 0x68 ),	/* X64 Stack size/offset = 104 */
/* 268 */	0xb,		/* FC_HYPER */
			0x0,		/* 0 */

	/* Parameter gvsnVersion */

/* 270 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 272 */	NdrFcShort( 0x70 ),	/* X64 Stack size/offset = 112 */
/* 274 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure RequestVersionVector */


	/* Return value */

/* 276 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 278 */	NdrFcLong( 0x0 ),	/* 0 */
/* 282 */	NdrFcShort( 0x4 ),	/* 4 */
/* 284 */	NdrFcShort( 0x48 ),	/* X64 Stack size/offset = 72 */
/* 286 */	0x32,		/* FC_BIND_PRIMITIVE */
			0x0,		/* 0 */
/* 288 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 290 */	NdrFcShort( 0xac ),	/* 172 */
/* 292 */	NdrFcShort( 0x8 ),	/* 8 */
/* 294 */	0xc4,		/* Oi2 Flags:  has return, has ext, has async handle */
			0x7,		/* 7 */
/* 296 */	0xa,		/* 10 */
			0x81,		/* Ext Flags:  new corr desc, has big amd64 byval param */
/* 298 */	NdrFcShort( 0x0 ),	/* 0 */
/* 300 */	NdrFcShort( 0x0 ),	/* 0 */
/* 302 */	NdrFcShort( 0x0 ),	/* 0 */
/* 304 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter IDL_handle */

/* 306 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 308 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 310 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter sequenceNumber */

/* 312 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 314 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 316 */	NdrFcShort( 0xc ),	/* Type Offset=12 */

	/* Parameter connectionId */

/* 318 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 320 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 322 */	NdrFcShort( 0xc ),	/* Type Offset=12 */

	/* Parameter contentSetId */

/* 324 */	NdrFcShort( 0x88 ),	/* Flags:  in, by val, */
/* 326 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 328 */	NdrFcShort( 0xd8 ),	/* 216 */

	/* Parameter requestType */

/* 330 */	NdrFcShort( 0x88 ),	/* Flags:  in, by val, */
/* 332 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 334 */	NdrFcShort( 0xe2 ),	/* 226 */

	/* Parameter changeType */

/* 336 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 338 */	NdrFcShort( 0x38 ),	/* X64 Stack size/offset = 56 */
/* 340 */	0xb,		/* FC_HYPER */
			0x0,		/* 0 */

	/* Parameter vvGeneration */

/* 342 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 344 */	NdrFcShort( 0x40 ),	/* X64 Stack size/offset = 64 */
/* 346 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure AsyncPoll */


	/* Return value */

/* 348 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 350 */	NdrFcLong( 0x0 ),	/* 0 */
/* 354 */	NdrFcShort( 0x5 ),	/* 5 */
/* 356 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 358 */	0x32,		/* FC_BIND_PRIMITIVE */
			0x0,		/* 0 */
/* 360 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 362 */	NdrFcShort( 0x44 ),	/* 68 */
/* 364 */	NdrFcShort( 0x8 ),	/* 8 */
/* 366 */	0xc5,		/* Oi2 Flags:  srv must size, has return, has ext, has async handle */
			0x3,		/* 3 */
/* 368 */	0xa,		/* 10 */
			0x83,		/* Ext Flags:  new corr desc, clt corr check, has big amd64 byval param */
/* 370 */	NdrFcShort( 0x1 ),	/* 1 */
/* 372 */	NdrFcShort( 0x0 ),	/* 0 */
/* 374 */	NdrFcShort( 0x0 ),	/* 0 */
/* 376 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter IDL_handle */

/* 378 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 380 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 382 */	NdrFcShort( 0xc ),	/* Type Offset=12 */

	/* Parameter connectionId */

/* 384 */	NdrFcShort( 0xc113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=48 */
/* 386 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 388 */	NdrFcShort( 0x150 ),	/* Type Offset=336 */

	/* Parameter response */

/* 390 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 392 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 394 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure RequestRecords */


	/* Return value */

/* 396 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 398 */	NdrFcLong( 0x0 ),	/* 0 */
/* 402 */	NdrFcShort( 0x6 ),	/* 6 */
/* 404 */	NdrFcShort( 0x60 ),	/* X64 Stack size/offset = 96 */
/* 406 */	0x32,		/* FC_BIND_PRIMITIVE */
			0x0,		/* 0 */
/* 408 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 410 */	NdrFcShort( 0xf8 ),	/* 248 */
/* 412 */	NdrFcShort( 0x76 ),	/* 118 */
/* 414 */	0xc5,		/* Oi2 Flags:  srv must size, has return, has ext, has async handle */
			0xa,		/* 10 */
/* 416 */	0xa,		/* 10 */
			0x83,		/* Ext Flags:  new corr desc, clt corr check, has big amd64 byval param */
/* 418 */	NdrFcShort( 0x1 ),	/* 1 */
/* 420 */	NdrFcShort( 0x0 ),	/* 0 */
/* 422 */	NdrFcShort( 0x0 ),	/* 0 */
/* 424 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter IDL_handle */

/* 426 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 428 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 430 */	NdrFcShort( 0xc ),	/* Type Offset=12 */

	/* Parameter connectionId */

/* 432 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 434 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 436 */	NdrFcShort( 0xc ),	/* Type Offset=12 */

	/* Parameter contentSetId */

/* 438 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 440 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 442 */	NdrFcShort( 0xc ),	/* Type Offset=12 */

	/* Parameter uidDbGuid */

/* 444 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 446 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 448 */	0xb,		/* FC_HYPER */
			0x0,		/* 0 */

	/* Parameter uidVersion */

/* 450 */	NdrFcShort( 0x158 ),	/* Flags:  in, out, base type, simple ref, */
/* 452 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 454 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter maxRecords */

/* 456 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 458 */	NdrFcShort( 0x38 ),	/* X64 Stack size/offset = 56 */
/* 460 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter numRecords */

/* 462 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 464 */	NdrFcShort( 0x40 ),	/* X64 Stack size/offset = 64 */
/* 466 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter numBytes */

/* 468 */	NdrFcShort( 0x2013 ),	/* Flags:  must size, must free, out, srv alloc size=8 */
/* 470 */	NdrFcShort( 0x48 ),	/* X64 Stack size/offset = 72 */
/* 472 */	NdrFcShort( 0x164 ),	/* Type Offset=356 */

	/* Parameter compressedRecords */

/* 474 */	NdrFcShort( 0x2010 ),	/* Flags:  out, srv alloc size=8 */
/* 476 */	NdrFcShort( 0x50 ),	/* X64 Stack size/offset = 80 */
/* 478 */	NdrFcShort( 0xcc ),	/* Type Offset=204 */

	/* Parameter recordsStatus */

/* 480 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 482 */	NdrFcShort( 0x58 ),	/* X64 Stack size/offset = 88 */
/* 484 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure UpdateCancel */


	/* Return value */

/* 486 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 488 */	NdrFcLong( 0x0 ),	/* 0 */
/* 492 */	NdrFcShort( 0x7 ),	/* 7 */
/* 494 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 496 */	0x32,		/* FC_BIND_PRIMITIVE */
			0x0,		/* 0 */
/* 498 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 500 */	NdrFcShort( 0x44 ),	/* 68 */
/* 502 */	NdrFcShort( 0x8 ),	/* 8 */
/* 504 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
			0x3,		/* 3 */
/* 506 */	0xa,		/* 10 */
			0x81,		/* Ext Flags:  new corr desc, has big amd64 byval param */
/* 508 */	NdrFcShort( 0x0 ),	/* 0 */
/* 510 */	NdrFcShort( 0x0 ),	/* 0 */
/* 512 */	NdrFcShort( 0x0 ),	/* 0 */
/* 514 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter IDL_handle */

/* 516 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 518 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 520 */	NdrFcShort( 0xc ),	/* Type Offset=12 */

	/* Parameter connectionId */

/* 522 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 524 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 526 */	NdrFcShort( 0x17c ),	/* Type Offset=380 */

	/* Parameter cancelData */

/* 528 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 530 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 532 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure RawGetFileData */


	/* Return value */

/* 534 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 536 */	NdrFcLong( 0x0 ),	/* 0 */
/* 540 */	NdrFcShort( 0x8 ),	/* 8 */
/* 542 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 544 */	0x30,		/* FC_BIND_CONTEXT */
			0xe8,		/* Ctxt flags:  via ptr, in, out, strict, */
/* 546 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 548 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 550 */	NdrFcShort( 0x40 ),	/* 64 */
/* 552 */	NdrFcShort( 0x78 ),	/* 120 */
/* 554 */	0x45,		/* Oi2 Flags:  srv must size, has return, has ext, */
			0x6,		/* 6 */
/* 556 */	0xa,		/* 10 */
			0x3,		/* Ext Flags:  new corr desc, clt corr check, */
/* 558 */	NdrFcShort( 0x1 ),	/* 1 */
/* 560 */	NdrFcShort( 0x0 ),	/* 0 */
/* 562 */	NdrFcShort( 0x0 ),	/* 0 */
/* 564 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter serverContext */

/* 566 */	NdrFcShort( 0x118 ),	/* Flags:  in, out, simple ref, */
/* 568 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 570 */	NdrFcShort( 0x1a4 ),	/* Type Offset=420 */

	/* Parameter dataBuffer */

/* 572 */	NdrFcShort( 0x113 ),	/* Flags:  must size, must free, out, simple ref, */
/* 574 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 576 */	NdrFcShort( 0x1ac ),	/* Type Offset=428 */

	/* Parameter bufferSize */

/* 578 */	NdrFcShort( 0x88 ),	/* Flags:  in, by val, */
/* 580 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 582 */	NdrFcShort( 0x1be ),	/* 446 */

	/* Parameter sizeRead */

/* 584 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 586 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 588 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter isEndOfFile */

/* 590 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 592 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 594 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Return value */

/* 596 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 598 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 600 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure RdcGetSignatures */

/* 602 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 604 */	NdrFcLong( 0x0 ),	/* 0 */
/* 608 */	NdrFcShort( 0x9 ),	/* 9 */
/* 610 */	NdrFcShort( 0x38 ),	/* X64 Stack size/offset = 56 */
/* 612 */	0x30,		/* FC_BIND_CONTEXT */
			0x48,		/* Ctxt flags:  in, strict, */
/* 614 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 616 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 618 */	NdrFcShort( 0x41 ),	/* 65 */
/* 620 */	NdrFcShort( 0x24 ),	/* 36 */
/* 622 */	0x45,		/* Oi2 Flags:  srv must size, has return, has ext, */
			0x7,		/* 7 */
/* 624 */	0xa,		/* 10 */
			0x3,		/* Ext Flags:  new corr desc, clt corr check, */
/* 626 */	NdrFcShort( 0x1 ),	/* 1 */
/* 628 */	NdrFcShort( 0x0 ),	/* 0 */
/* 630 */	NdrFcShort( 0x0 ),	/* 0 */
/* 632 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter serverContext */

/* 634 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 636 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 638 */	NdrFcShort( 0x1c8 ),	/* Type Offset=456 */

	/* Parameter level */

/* 640 */	NdrFcShort( 0x88 ),	/* Flags:  in, by val, */
/* 642 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 644 */	NdrFcShort( 0x1cc ),	/* 460 */

	/* Parameter offset */

/* 646 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 648 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 650 */	0xb,		/* FC_HYPER */
			0x0,		/* 0 */

	/* Parameter buffer */

/* 652 */	NdrFcShort( 0x113 ),	/* Flags:  must size, must free, out, simple ref, */
/* 654 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 656 */	NdrFcShort( 0x1da ),	/* Type Offset=474 */

	/* Parameter length */

/* 658 */	NdrFcShort( 0x88 ),	/* Flags:  in, by val, */
/* 660 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 662 */	NdrFcShort( 0x1ec ),	/* 492 */

	/* Parameter sizeRead */

/* 664 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 666 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 668 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Return value */

/* 670 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 672 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 674 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure RdcPushSourceNeeds */

/* 676 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 678 */	NdrFcLong( 0x0 ),	/* 0 */
/* 682 */	NdrFcShort( 0xa ),	/* 10 */
/* 684 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 686 */	0x30,		/* FC_BIND_CONTEXT */
			0x48,		/* Ctxt flags:  in, strict, */
/* 688 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 690 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 692 */	NdrFcShort( 0x2c ),	/* 44 */
/* 694 */	NdrFcShort( 0x8 ),	/* 8 */
/* 696 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
			0x4,		/* 4 */
/* 698 */	0xa,		/* 10 */
			0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 700 */	NdrFcShort( 0x0 ),	/* 0 */
/* 702 */	NdrFcShort( 0x1 ),	/* 1 */
/* 704 */	NdrFcShort( 0x0 ),	/* 0 */
/* 706 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter serverContext */

/* 708 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 710 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 712 */	NdrFcShort( 0x1c8 ),	/* Type Offset=456 */

	/* Parameter sourceNeeds */

/* 714 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 716 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 718 */	NdrFcShort( 0x202 ),	/* Type Offset=514 */

	/* Parameter needCount */

/* 720 */	NdrFcShort( 0x88 ),	/* Flags:  in, by val, */
/* 722 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 724 */	NdrFcShort( 0x218 ),	/* 536 */

	/* Return value */

/* 726 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 728 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 730 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure RdcGetFileData */

/* 732 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 734 */	NdrFcLong( 0x0 ),	/* 0 */
/* 738 */	NdrFcShort( 0xb ),	/* 11 */
/* 740 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 742 */	0x30,		/* FC_BIND_CONTEXT */
			0x48,		/* Ctxt flags:  in, strict, */
/* 744 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 746 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 748 */	NdrFcShort( 0x2c ),	/* 44 */
/* 750 */	NdrFcShort( 0x24 ),	/* 36 */
/* 752 */	0x45,		/* Oi2 Flags:  srv must size, has return, has ext, */
			0x5,		/* 5 */
/* 754 */	0xa,		/* 10 */
			0x3,		/* Ext Flags:  new corr desc, clt corr check, */
/* 756 */	NdrFcShort( 0x1 ),	/* 1 */
/* 758 */	NdrFcShort( 0x0 ),	/* 0 */
/* 760 */	NdrFcShort( 0x0 ),	/* 0 */
/* 762 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter serverContext */

/* 764 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 766 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 768 */	NdrFcShort( 0x1c8 ),	/* Type Offset=456 */

	/* Parameter dataBuffer */

/* 770 */	NdrFcShort( 0x113 ),	/* Flags:  must size, must free, out, simple ref, */
/* 772 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 774 */	NdrFcShort( 0x1ac ),	/* Type Offset=428 */

	/* Parameter bufferSize */

/* 776 */	NdrFcShort( 0x88 ),	/* Flags:  in, by val, */
/* 778 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 780 */	NdrFcShort( 0x222 ),	/* 546 */

	/* Parameter sizeReturned */

/* 782 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 784 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 786 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Return value */

/* 788 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 790 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 792 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure RdcClose */

/* 794 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 796 */	NdrFcLong( 0x0 ),	/* 0 */
/* 800 */	NdrFcShort( 0xc ),	/* 12 */
/* 802 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 804 */	0x30,		/* FC_BIND_CONTEXT */
			0xe8,		/* Ctxt flags:  via ptr, in, out, strict, */
/* 806 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 808 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 810 */	NdrFcShort( 0x38 ),	/* 56 */
/* 812 */	NdrFcShort( 0x40 ),	/* 64 */
/* 814 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x2,		/* 2 */
/* 816 */	0xa,		/* 10 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 818 */	NdrFcShort( 0x0 ),	/* 0 */
/* 820 */	NdrFcShort( 0x0 ),	/* 0 */
/* 822 */	NdrFcShort( 0x0 ),	/* 0 */
/* 824 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter serverContext */

/* 826 */	NdrFcShort( 0x118 ),	/* Flags:  in, out, simple ref, */
/* 828 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 830 */	NdrFcShort( 0x1a4 ),	/* Type Offset=420 */

	/* Return value */

/* 832 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 834 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 836 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure InitializeFileTransferAsync */

/* 838 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 840 */	NdrFcLong( 0x0 ),	/* 0 */
/* 844 */	NdrFcShort( 0xd ),	/* 13 */
/* 846 */	NdrFcShort( 0x68 ),	/* X64 Stack size/offset = 104 */
/* 848 */	0x32,		/* FC_BIND_PRIMITIVE */
			0x0,		/* 0 */
/* 850 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 852 */	NdrFcShort( 0x6e ),	/* 110 */
/* 854 */	NdrFcShort( 0x92 ),	/* 146 */
/* 856 */	0xc7,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, has async handle */
			0xb,		/* 11 */
/* 858 */	0xa,		/* 10 */
			0x83,		/* Ext Flags:  new corr desc, clt corr check, has big amd64 byval param */
/* 860 */	NdrFcShort( 0x1 ),	/* 1 */
/* 862 */	NdrFcShort( 0x0 ),	/* 0 */
/* 864 */	NdrFcShort( 0x0 ),	/* 0 */
/* 866 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter IDL_handle */

/* 868 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 870 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 872 */	NdrFcShort( 0xc ),	/* Type Offset=12 */

	/* Parameter connectionId */

/* 874 */	NdrFcShort( 0x11b ),	/* Flags:  must size, must free, in, out, simple ref, */
/* 876 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 878 */	NdrFcShort( 0x7c ),	/* Type Offset=124 */

	/* Parameter frsUpdate */

/* 880 */	NdrFcShort( 0x88 ),	/* Flags:  in, by val, */
/* 882 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 884 */	NdrFcShort( 0x230 ),	/* 560 */

	/* Parameter rdcDesired */

/* 886 */	NdrFcShort( 0x2018 ),	/* Flags:  in, out, srv alloc size=8 */
/* 888 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 890 */	NdrFcShort( 0xcc ),	/* Type Offset=204 */

	/* Parameter stagingPolicy */

/* 892 */	NdrFcShort( 0x110 ),	/* Flags:  out, simple ref, */
/* 894 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 896 */	NdrFcShort( 0x23e ),	/* Type Offset=574 */

	/* Parameter serverContext */

/* 898 */	NdrFcShort( 0x2013 ),	/* Flags:  must size, must free, out, srv alloc size=8 */
/* 900 */	NdrFcShort( 0x38 ),	/* X64 Stack size/offset = 56 */
/* 902 */	NdrFcShort( 0x242 ),	/* Type Offset=578 */

	/* Parameter rdcFileInfo */

/* 904 */	NdrFcShort( 0x113 ),	/* Flags:  must size, must free, out, simple ref, */
/* 906 */	NdrFcShort( 0x40 ),	/* X64 Stack size/offset = 64 */
/* 908 */	NdrFcShort( 0x2f0 ),	/* Type Offset=752 */

	/* Parameter dataBuffer */

/* 910 */	NdrFcShort( 0x88 ),	/* Flags:  in, by val, */
/* 912 */	NdrFcShort( 0x48 ),	/* X64 Stack size/offset = 72 */
/* 914 */	NdrFcShort( 0x302 ),	/* 770 */

	/* Parameter bufferSize */

/* 916 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 918 */	NdrFcShort( 0x50 ),	/* X64 Stack size/offset = 80 */
/* 920 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter sizeRead */

/* 922 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 924 */	NdrFcShort( 0x58 ),	/* X64 Stack size/offset = 88 */
/* 926 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter isEndOfFile */

/* 928 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 930 */	NdrFcShort( 0x60 ),	/* X64 Stack size/offset = 96 */
/* 932 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure InitializeFileDataTransfer */


	/* Return value */

/* 934 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 936 */	NdrFcLong( 0x0 ),	/* 0 */
/* 940 */	NdrFcShort( 0xe ),	/* 14 */
/* 942 */	NdrFcShort( 0x58 ),	/* X64 Stack size/offset = 88 */
/* 944 */	0x32,		/* FC_BIND_PRIMITIVE */
			0x0,		/* 0 */
/* 946 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 948 */	NdrFcShort( 0x6c ),	/* 108 */
/* 950 */	NdrFcShort( 0x78 ),	/* 120 */
/* 952 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0xa,		/* 10 */
/* 954 */	0xa,		/* 10 */
			0x83,		/* Ext Flags:  new corr desc, clt corr check, has big amd64 byval param */
/* 956 */	NdrFcShort( 0x1 ),	/* 1 */
/* 958 */	NdrFcShort( 0x0 ),	/* 0 */
/* 960 */	NdrFcShort( 0x0 ),	/* 0 */
/* 962 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter IDL_handle */

/* 964 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 966 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 968 */	NdrFcShort( 0xc ),	/* Type Offset=12 */

	/* Parameter connectionId */

/* 970 */	NdrFcShort( 0x11b ),	/* Flags:  must size, must free, in, out, simple ref, */
/* 972 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 974 */	NdrFcShort( 0x7c ),	/* Type Offset=124 */

	/* Parameter frsUpdate */

/* 976 */	NdrFcShort( 0x110 ),	/* Flags:  out, simple ref, */
/* 978 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 980 */	NdrFcShort( 0x23e ),	/* Type Offset=574 */

	/* Parameter serverContext */

/* 982 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 984 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 986 */	0xb,		/* FC_HYPER */
			0x0,		/* 0 */

	/* Parameter offset */

/* 988 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 990 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 992 */	0xb,		/* FC_HYPER */
			0x0,		/* 0 */

	/* Parameter length */

/* 994 */	NdrFcShort( 0x113 ),	/* Flags:  must size, must free, out, simple ref, */
/* 996 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 998 */	NdrFcShort( 0x310 ),	/* Type Offset=784 */

	/* Parameter dataBuffer */

/* 1000 */	NdrFcShort( 0x88 ),	/* Flags:  in, by val, */
/* 1002 */	NdrFcShort( 0x38 ),	/* X64 Stack size/offset = 56 */
/* 1004 */	NdrFcShort( 0x322 ),	/* 802 */

	/* Parameter bufferSize */

/* 1006 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1008 */	NdrFcShort( 0x40 ),	/* X64 Stack size/offset = 64 */
/* 1010 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter sizeRead */

/* 1012 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1014 */	NdrFcShort( 0x48 ),	/* X64 Stack size/offset = 72 */
/* 1016 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter isEndOfFile */

/* 1018 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1020 */	NdrFcShort( 0x50 ),	/* X64 Stack size/offset = 80 */
/* 1022 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure RawGetFileDataAsync */


	/* Return value */

/* 1024 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 1026 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1030 */	NdrFcShort( 0xf ),	/* 15 */
/* 1032 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1034 */	0x30,		/* FC_BIND_CONTEXT */
			0x48,		/* Ctxt flags:  in, strict, */
/* 1036 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 1038 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 1040 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1042 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1044 */	0xcc,		/* Oi2 Flags:  has return, has pipes, has ext, has async handle */
			0x3,		/* 3 */
/* 1046 */	0xa,		/* 10 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 1048 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1050 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1052 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1054 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter serverContext */

/* 1056 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 1058 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 1060 */	NdrFcShort( 0x1c8 ),	/* Type Offset=456 */

	/* Parameter bytePipe */

/* 1062 */	NdrFcShort( 0x4114 ),	/* Flags:  pipe, out, simple ref, srv alloc size=16 */
/* 1064 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 1066 */	NdrFcShort( 0x332 ),	/* Type Offset=818 */

	/* Return value */

/* 1068 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1070 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1072 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure RdcGetFileDataAsync */

/* 1074 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 1076 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1080 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1082 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1084 */	0x30,		/* FC_BIND_CONTEXT */
			0x48,		/* Ctxt flags:  in, strict, */
/* 1086 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 1088 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 1090 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1092 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1094 */	0xcc,		/* Oi2 Flags:  has return, has pipes, has ext, has async handle */
			0x3,		/* 3 */
/* 1096 */	0xa,		/* 10 */
			0x1,		/* Ext Flags:  new corr desc, */
/* 1098 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1100 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1102 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1104 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter serverContext */

/* 1106 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 1108 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 1110 */	NdrFcShort( 0x1c8 ),	/* Type Offset=456 */

	/* Parameter bytePipe */

/* 1112 */	NdrFcShort( 0x4114 ),	/* Flags:  pipe, out, simple ref, srv alloc size=16 */
/* 1114 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 1116 */	NdrFcShort( 0x340 ),	/* Type Offset=832 */

	/* Return value */

/* 1118 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1120 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1122 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

			0x0
        }
    };

static const ms2Dfrs2_MIDL_TYPE_FORMAT_STRING ms2Dfrs2__MIDL_TypeFormatString =
    {
        0,
        {
			NdrFcShort( 0x0 ),	/* 0 */
/*  2 */	
			0x11, 0x0,	/* FC_RP */
/*  4 */	NdrFcShort( 0x8 ),	/* Offset= 8 (12) */
/*  6 */	
			0x1d,		/* FC_SMFARRAY */
			0x0,		/* 0 */
/*  8 */	NdrFcShort( 0x8 ),	/* 8 */
/* 10 */	0x1,		/* FC_BYTE */
			0x5b,		/* FC_END */
/* 12 */	
			0x15,		/* FC_STRUCT */
			0x3,		/* 3 */
/* 14 */	NdrFcShort( 0x10 ),	/* 16 */
/* 16 */	0x8,		/* FC_LONG */
			0x6,		/* FC_SHORT */
/* 18 */	0x6,		/* FC_SHORT */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 20 */	0x0,		/* 0 */
			NdrFcShort( 0xfff1 ),	/* Offset= -15 (6) */
			0x5b,		/* FC_END */
/* 24 */	
			0x11, 0xc,	/* FC_RP [alloced_on_stack] [simple_pointer] */
/* 26 */	0x8,		/* FC_LONG */
			0x5c,		/* FC_PAD */
/* 28 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 30 */	NdrFcLong( 0x0 ),	/* 0 */
/* 34 */	NdrFcLong( 0x100 ),	/* 256 */
/* 38 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 40 */	NdrFcLong( 0x0 ),	/* 0 */
/* 44 */	NdrFcLong( 0x1 ),	/* 1 */
/* 48 */	0xb7,		/* FC_RANGE */
			0xd,		/* 13 */
/* 50 */	NdrFcLong( 0x0 ),	/* 0 */
/* 54 */	NdrFcLong( 0x2 ),	/* 2 */
/* 58 */	
			0x11, 0x0,	/* FC_RP */
/* 60 */	NdrFcShort( 0xe ),	/* Offset= 14 (74) */
/* 62 */	
			0x15,		/* FC_STRUCT */
			0x7,		/* 7 */
/* 64 */	NdrFcShort( 0x20 ),	/* 32 */
/* 66 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 68 */	NdrFcShort( 0xffc8 ),	/* Offset= -56 (12) */
/* 70 */	0xb,		/* FC_HYPER */
			0xb,		/* FC_HYPER */
/* 72 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 74 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x7,		/* 7 */
/* 76 */	NdrFcShort( 0x0 ),	/* 0 */
/* 78 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 80 */	NdrFcShort( 0x38 ),	/* X64 Stack size/offset = 56 */
/* 82 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 84 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 88 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 90 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 92 */	NdrFcShort( 0xffe2 ),	/* Offset= -30 (62) */
/* 94 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 96 */	
			0x11, 0x0,	/* FC_RP */
/* 98 */	NdrFcShort( 0x54 ),	/* Offset= 84 (182) */
/* 100 */	
			0x15,		/* FC_STRUCT */
			0x3,		/* 3 */
/* 102 */	NdrFcShort( 0x8 ),	/* 8 */
/* 104 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 106 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 108 */	
			0x1d,		/* FC_SMFARRAY */
			0x0,		/* 0 */
/* 110 */	NdrFcShort( 0x14 ),	/* 20 */
/* 112 */	0x2,		/* FC_CHAR */
			0x5b,		/* FC_END */
/* 114 */	
			0x1d,		/* FC_SMFARRAY */
			0x0,		/* 0 */
/* 116 */	NdrFcShort( 0x10 ),	/* 16 */
/* 118 */	0x2,		/* FC_CHAR */
			0x5b,		/* FC_END */
/* 120 */	
			0x29,		/* FC_WSTRING */
			0x5c,		/* FC_PAD */
/* 122 */	NdrFcShort( 0x105 ),	/* 261 */
/* 124 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 126 */	NdrFcShort( 0x2b0 ),	/* 688 */
/* 128 */	NdrFcShort( 0x0 ),	/* 0 */
/* 130 */	NdrFcShort( 0x0 ),	/* Offset= 0 (130) */
/* 132 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 134 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 136 */	0x0,		/* 0 */
			NdrFcShort( 0xffdb ),	/* Offset= -37 (100) */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 140 */	0x0,		/* 0 */
			NdrFcShort( 0xffd7 ),	/* Offset= -41 (100) */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 144 */	0x0,		/* 0 */
			NdrFcShort( 0xffd3 ),	/* Offset= -45 (100) */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 148 */	0x0,		/* 0 */
			NdrFcShort( 0xff77 ),	/* Offset= -137 (12) */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 152 */	0x0,		/* 0 */
			NdrFcShort( 0xffd3 ),	/* Offset= -45 (108) */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 156 */	0x0,		/* 0 */
			NdrFcShort( 0xffd5 ),	/* Offset= -43 (114) */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 160 */	0x0,		/* 0 */
			NdrFcShort( 0xff6b ),	/* Offset= -149 (12) */
			0xb,		/* FC_HYPER */
/* 164 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 166 */	NdrFcShort( 0xff66 ),	/* Offset= -154 (12) */
/* 168 */	0xb,		/* FC_HYPER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 170 */	0x0,		/* 0 */
			NdrFcShort( 0xff61 ),	/* Offset= -159 (12) */
			0xb,		/* FC_HYPER */
/* 174 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 176 */	NdrFcShort( 0xffc8 ),	/* Offset= -56 (120) */
/* 178 */	0x3e,		/* FC_STRUCTPAD2 */
			0x8,		/* FC_LONG */
/* 180 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 182 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x7,		/* 7 */
/* 184 */	NdrFcShort( 0x0 ),	/* 0 */
/* 186 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 188 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 190 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 192 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 194 */	NdrFcShort( 0x50 ),	/* X64 Stack size/offset = 80 */
/* 196 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 198 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 200 */	NdrFcShort( 0xffb4 ),	/* Offset= -76 (124) */
/* 202 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 204 */	
			0x11, 0xc,	/* FC_RP [alloced_on_stack] [simple_pointer] */
/* 206 */	0xd,		/* FC_ENUM16 */
			0x5c,		/* FC_PAD */
/* 208 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 210 */	NdrFcShort( 0xff3a ),	/* Offset= -198 (12) */
/* 212 */	
			0x11, 0xc,	/* FC_RP [alloced_on_stack] [simple_pointer] */
/* 214 */	0xb,		/* FC_HYPER */
			0x5c,		/* FC_PAD */
/* 216 */	0xb7,		/* FC_RANGE */
			0xd,		/* 13 */
/* 218 */	NdrFcLong( 0x0 ),	/* 0 */
/* 222 */	NdrFcLong( 0x2 ),	/* 2 */
/* 226 */	0xb7,		/* FC_RANGE */
			0xd,		/* 13 */
/* 228 */	NdrFcLong( 0x0 ),	/* 0 */
/* 232 */	NdrFcLong( 0x2 ),	/* 2 */
/* 236 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 238 */	NdrFcShort( 0x62 ),	/* Offset= 98 (336) */
/* 240 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x7,		/* 7 */
/* 242 */	NdrFcShort( 0x0 ),	/* 0 */
/* 244 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 246 */	NdrFcShort( 0x8 ),	/* 8 */
/* 248 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 250 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 254 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 256 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 258 */	NdrFcShort( 0xff3c ),	/* Offset= -196 (62) */
/* 260 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 262 */	
			0x15,		/* FC_STRUCT */
			0x1,		/* 1 */
/* 264 */	NdrFcShort( 0x10 ),	/* 16 */
/* 266 */	0x6,		/* FC_SHORT */
			0x6,		/* FC_SHORT */
/* 268 */	0x6,		/* FC_SHORT */
			0x6,		/* FC_SHORT */
/* 270 */	0x6,		/* FC_SHORT */
			0x6,		/* FC_SHORT */
/* 272 */	0x6,		/* FC_SHORT */
			0x6,		/* FC_SHORT */
/* 274 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 276 */	
			0x15,		/* FC_STRUCT */
			0x3,		/* 3 */
/* 278 */	NdrFcShort( 0x20 ),	/* 32 */
/* 280 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 282 */	NdrFcShort( 0xfef2 ),	/* Offset= -270 (12) */
/* 284 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 286 */	NdrFcShort( 0xffe8 ),	/* Offset= -24 (262) */
/* 288 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 290 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 292 */	NdrFcShort( 0x0 ),	/* 0 */
/* 294 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 296 */	NdrFcShort( 0x18 ),	/* 24 */
/* 298 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 300 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 304 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 306 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 308 */	NdrFcShort( 0xffe0 ),	/* Offset= -32 (276) */
/* 310 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 312 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 314 */	NdrFcShort( 0x28 ),	/* 40 */
/* 316 */	NdrFcShort( 0x0 ),	/* 0 */
/* 318 */	NdrFcShort( 0xa ),	/* Offset= 10 (328) */
/* 320 */	0xb,		/* FC_HYPER */
			0x8,		/* FC_LONG */
/* 322 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 324 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 326 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 328 */	
			0x12, 0x0,	/* FC_UP */
/* 330 */	NdrFcShort( 0xffa6 ),	/* Offset= -90 (240) */
/* 332 */	
			0x12, 0x0,	/* FC_UP */
/* 334 */	NdrFcShort( 0xffd4 ),	/* Offset= -44 (290) */
/* 336 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 338 */	NdrFcShort( 0x30 ),	/* 48 */
/* 340 */	NdrFcShort( 0x0 ),	/* 0 */
/* 342 */	NdrFcShort( 0x0 ),	/* Offset= 0 (342) */
/* 344 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 346 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 348 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (312) */
/* 350 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 352 */	
			0x11, 0x8,	/* FC_RP [simple_pointer] */
/* 354 */	0x8,		/* FC_LONG */
			0x5c,		/* FC_PAD */
/* 356 */	
			0x11, 0x14,	/* FC_RP [alloced_on_stack] [pointer_deref] */
/* 358 */	NdrFcShort( 0x2 ),	/* Offset= 2 (360) */
/* 360 */	
			0x12, 0x0,	/* FC_UP */
/* 362 */	NdrFcShort( 0x2 ),	/* Offset= 2 (364) */
/* 364 */	
			0x1b,		/* FC_CARRAY */
			0x0,		/* 0 */
/* 366 */	NdrFcShort( 0x1 ),	/* 1 */
/* 368 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 370 */	NdrFcShort( 0x40 ),	/* X64 Stack size/offset = 64 */
/* 372 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 374 */	0x1,		/* FC_BYTE */
			0x5b,		/* FC_END */
/* 376 */	
			0x11, 0x0,	/* FC_RP */
/* 378 */	NdrFcShort( 0x2 ),	/* Offset= 2 (380) */
/* 380 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 382 */	NdrFcShort( 0x318 ),	/* 792 */
/* 384 */	NdrFcShort( 0x0 ),	/* 0 */
/* 386 */	NdrFcShort( 0x0 ),	/* Offset= 0 (386) */
/* 388 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 390 */	NdrFcShort( 0xfef6 ),	/* Offset= -266 (124) */
/* 392 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 394 */	NdrFcShort( 0xfe82 ),	/* Offset= -382 (12) */
/* 396 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 398 */	NdrFcShort( 0xfe7e ),	/* Offset= -386 (12) */
/* 400 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 402 */	NdrFcShort( 0xfe7a ),	/* Offset= -390 (12) */
/* 404 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 406 */	NdrFcShort( 0xfe76 ),	/* Offset= -394 (12) */
/* 408 */	0xb,		/* FC_HYPER */
			0xb,		/* FC_HYPER */
/* 410 */	0xb,		/* FC_HYPER */
			0x8,		/* FC_LONG */
/* 412 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 414 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 416 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 418 */	NdrFcShort( 0x2 ),	/* Offset= 2 (420) */
/* 420 */	0x30,		/* FC_BIND_CONTEXT */
			0xe9,		/* Ctxt flags:  via ptr, in, out, strict, can't be null */
/* 422 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 424 */	
			0x11, 0x0,	/* FC_RP */
/* 426 */	NdrFcShort( 0x2 ),	/* Offset= 2 (428) */
/* 428 */	
			0x1c,		/* FC_CVARRAY */
			0x0,		/* 0 */
/* 430 */	NdrFcShort( 0x1 ),	/* 1 */
/* 432 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 434 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 436 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 438 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 440 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 442 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 444 */	0x1,		/* FC_BYTE */
			0x5b,		/* FC_END */
/* 446 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 448 */	NdrFcLong( 0x0 ),	/* 0 */
/* 452 */	NdrFcLong( 0x40000 ),	/* 262144 */
/* 456 */	0x30,		/* FC_BIND_CONTEXT */
			0x49,		/* Ctxt flags:  in, strict, can't be null */
/* 458 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 460 */	0xb7,		/* FC_RANGE */
			0x1,		/* 1 */
/* 462 */	NdrFcLong( 0x1 ),	/* 1 */
/* 466 */	NdrFcLong( 0x8 ),	/* 8 */
/* 470 */	
			0x11, 0x0,	/* FC_RP */
/* 472 */	NdrFcShort( 0x2 ),	/* Offset= 2 (474) */
/* 474 */	
			0x1c,		/* FC_CVARRAY */
			0x0,		/* 0 */
/* 476 */	NdrFcShort( 0x1 ),	/* 1 */
/* 478 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 480 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 482 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 484 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 486 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 488 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 490 */	0x1,		/* FC_BYTE */
			0x5b,		/* FC_END */
/* 492 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 494 */	NdrFcLong( 0x1 ),	/* 1 */
/* 498 */	NdrFcLong( 0x10000 ),	/* 65536 */
/* 502 */	
			0x11, 0x0,	/* FC_RP */
/* 504 */	NdrFcShort( 0xa ),	/* Offset= 10 (514) */
/* 506 */	
			0x15,		/* FC_STRUCT */
			0x7,		/* 7 */
/* 508 */	NdrFcShort( 0x10 ),	/* 16 */
/* 510 */	0xb,		/* FC_HYPER */
			0xb,		/* FC_HYPER */
/* 512 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 514 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x7,		/* 7 */
/* 516 */	NdrFcShort( 0x0 ),	/* 0 */
/* 518 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 520 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 522 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 524 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 528 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 530 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 532 */	NdrFcShort( 0xffe6 ),	/* Offset= -26 (506) */
/* 534 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 536 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 538 */	NdrFcLong( 0x0 ),	/* 0 */
/* 542 */	NdrFcLong( 0x14 ),	/* 20 */
/* 546 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 548 */	NdrFcLong( 0x0 ),	/* 0 */
/* 552 */	NdrFcLong( 0x40000 ),	/* 262144 */
/* 556 */	
			0x11, 0x0,	/* FC_RP */
/* 558 */	NdrFcShort( 0xfe4e ),	/* Offset= -434 (124) */
/* 560 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 562 */	NdrFcLong( 0x0 ),	/* 0 */
/* 566 */	NdrFcLong( 0x1 ),	/* 1 */
/* 570 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 572 */	NdrFcShort( 0x2 ),	/* Offset= 2 (574) */
/* 574 */	0x30,		/* FC_BIND_CONTEXT */
			0xa8,		/* Ctxt flags:  via ptr, out, strict, */
/* 576 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 578 */	
			0x11, 0x14,	/* FC_RP [alloced_on_stack] [pointer_deref] */
/* 580 */	NdrFcShort( 0x2 ),	/* Offset= 2 (582) */
/* 582 */	
			0x12, 0x0,	/* FC_UP */
/* 584 */	NdrFcShort( 0x90 ),	/* Offset= 144 (728) */
/* 586 */	0xb7,		/* FC_RANGE */
			0x1,		/* 1 */
/* 588 */	NdrFcLong( 0x0 ),	/* 0 */
/* 592 */	NdrFcLong( 0x8 ),	/* 8 */
/* 596 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x7,		/* FC_USHORT */
/* 598 */	0x7,		/* Corr desc: FC_USHORT */
			0x0,		/*  */
/* 600 */	NdrFcShort( 0xfffe ),	/* -2 */
/* 602 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 604 */	NdrFcShort( 0x2 ),	/* Offset= 2 (606) */
/* 606 */	NdrFcShort( 0x42 ),	/* 66 */
/* 608 */	NdrFcShort( 0x3 ),	/* 3 */
/* 610 */	NdrFcLong( 0x0 ),	/* 0 */
/* 614 */	NdrFcShort( 0x16 ),	/* Offset= 22 (636) */
/* 616 */	NdrFcLong( 0x1 ),	/* 1 */
/* 620 */	NdrFcShort( 0x2e ),	/* Offset= 46 (666) */
/* 622 */	NdrFcLong( 0x2 ),	/* 2 */
/* 626 */	NdrFcShort( 0x3a ),	/* Offset= 58 (684) */
/* 628 */	NdrFcShort( 0xffff ),	/* Offset= -1 (627) */
/* 630 */	
			0x1d,		/* FC_SMFARRAY */
			0x0,		/* 0 */
/* 632 */	NdrFcShort( 0x40 ),	/* 64 */
/* 634 */	0x1,		/* FC_BYTE */
			0x5b,		/* FC_END */
/* 636 */	
			0x15,		/* FC_STRUCT */
			0x1,		/* 1 */
/* 638 */	NdrFcShort( 0x42 ),	/* 66 */
/* 640 */	0x6,		/* FC_SHORT */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 642 */	0x0,		/* 0 */
			NdrFcShort( 0xfff3 ),	/* Offset= -13 (630) */
			0x5b,		/* FC_END */
/* 646 */	0xb7,		/* FC_RANGE */
			0x6,		/* 6 */
/* 648 */	NdrFcLong( 0x80 ),	/* 128 */
/* 652 */	NdrFcLong( 0x4000 ),	/* 16384 */
/* 656 */	0xb7,		/* FC_RANGE */
			0x6,		/* 6 */
/* 658 */	NdrFcLong( 0x2 ),	/* 2 */
/* 662 */	NdrFcLong( 0x60 ),	/* 96 */
/* 666 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x1,		/* 1 */
/* 668 */	NdrFcShort( 0x4 ),	/* 4 */
/* 670 */	NdrFcShort( 0x0 ),	/* 0 */
/* 672 */	NdrFcShort( 0x0 ),	/* Offset= 0 (672) */
/* 674 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 676 */	NdrFcShort( 0xffe2 ),	/* Offset= -30 (646) */
/* 678 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 680 */	NdrFcShort( 0xffe8 ),	/* Offset= -24 (656) */
/* 682 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 684 */	
			0x15,		/* FC_STRUCT */
			0x1,		/* 1 */
/* 686 */	NdrFcShort( 0x4 ),	/* 4 */
/* 688 */	0x6,		/* FC_SHORT */
			0x6,		/* FC_SHORT */
/* 690 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 692 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x1,		/* 1 */
/* 694 */	NdrFcShort( 0x44 ),	/* 68 */
/* 696 */	NdrFcShort( 0x0 ),	/* 0 */
/* 698 */	NdrFcShort( 0x0 ),	/* Offset= 0 (698) */
/* 700 */	0x6,		/* FC_SHORT */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 702 */	0x0,		/* 0 */
			NdrFcShort( 0xff95 ),	/* Offset= -107 (596) */
			0x5b,		/* FC_END */
/* 706 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x1,		/* 1 */
/* 708 */	NdrFcShort( 0x0 ),	/* 0 */
/* 710 */	0x3,		/* Corr desc: FC_SMALL */
			0x0,		/*  */
/* 712 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 714 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 716 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 720 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 722 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 724 */	NdrFcShort( 0xffe0 ),	/* Offset= -32 (692) */
/* 726 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 728 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 730 */	NdrFcShort( 0x1c ),	/* 28 */
/* 732 */	NdrFcShort( 0xffe6 ),	/* Offset= -26 (706) */
/* 734 */	NdrFcShort( 0x0 ),	/* Offset= 0 (734) */
/* 736 */	0xb,		/* FC_HYPER */
			0xb,		/* FC_HYPER */
/* 738 */	0x6,		/* FC_SHORT */
			0x6,		/* FC_SHORT */
/* 740 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 742 */	NdrFcShort( 0xff64 ),	/* Offset= -156 (586) */
/* 744 */	0x3f,		/* FC_STRUCTPAD3 */
			0xd,		/* FC_ENUM16 */
/* 746 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 748 */	
			0x11, 0x0,	/* FC_RP */
/* 750 */	NdrFcShort( 0x2 ),	/* Offset= 2 (752) */
/* 752 */	
			0x1c,		/* FC_CVARRAY */
			0x0,		/* 0 */
/* 754 */	NdrFcShort( 0x1 ),	/* 1 */
/* 756 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 758 */	NdrFcShort( 0x48 ),	/* X64 Stack size/offset = 72 */
/* 760 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 762 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 764 */	NdrFcShort( 0x50 ),	/* X64 Stack size/offset = 80 */
/* 766 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 768 */	0x1,		/* FC_BYTE */
			0x5b,		/* FC_END */
/* 770 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 772 */	NdrFcLong( 0x0 ),	/* 0 */
/* 776 */	NdrFcLong( 0x40000 ),	/* 262144 */
/* 780 */	
			0x11, 0x0,	/* FC_RP */
/* 782 */	NdrFcShort( 0x2 ),	/* Offset= 2 (784) */
/* 784 */	
			0x1c,		/* FC_CVARRAY */
			0x0,		/* 0 */
/* 786 */	NdrFcShort( 0x1 ),	/* 1 */
/* 788 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 790 */	NdrFcShort( 0x38 ),	/* X64 Stack size/offset = 56 */
/* 792 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 794 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 796 */	NdrFcShort( 0x40 ),	/* X64 Stack size/offset = 64 */
/* 798 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 800 */	0x1,		/* FC_BYTE */
			0x5b,		/* FC_END */
/* 802 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 804 */	NdrFcLong( 0x0 ),	/* 0 */
/* 808 */	NdrFcLong( 0x40000 ),	/* 262144 */
/* 812 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 814 */	NdrFcShort( 0x4 ),	/* Offset= 4 (818) */
/* 816 */	0x1,		/* FC_BYTE */
			0x5c,		/* FC_PAD */
/* 818 */	0xb5,		/* FC_PIPE */
			0x0,		/* 0 */
/* 820 */	NdrFcShort( 0xfffc ),	/* Offset= -4 (816) */
/* 822 */	NdrFcShort( 0x1 ),	/* 1 */
/* 824 */	NdrFcShort( 0x1 ),	/* 1 */
/* 826 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 828 */	NdrFcShort( 0x4 ),	/* Offset= 4 (832) */
/* 830 */	0x1,		/* FC_BYTE */
			0x5c,		/* FC_PAD */
/* 832 */	0xb5,		/* FC_PIPE */
			0x0,		/* 0 */
/* 834 */	NdrFcShort( 0xfffc ),	/* Offset= -4 (830) */
/* 836 */	NdrFcShort( 0x1 ),	/* 1 */
/* 838 */	NdrFcShort( 0x1 ),	/* 1 */

			0x0
        }
    };

static const unsigned short FrsTransport_FormatStringOffsetTable[] =
    {
    0,
    48,
    120,
    168,
    276,
    348,
    396,
    486,
    534,
    602,
    676,
    732,
    794,
    838,
    934,
    1024,
    1074
    };


static const MIDL_STUB_DESC FrsTransport_StubDesc = 
    {
    (void *)& FrsTransport___RpcClientInterface,
    MIDL_user_allocate,
    MIDL_user_free,
    &FrsTransport__MIDL_AutoBindHandle,
    0,
    0,
    0,
    0,
    ms2Dfrs2__MIDL_TypeFormatString.Format,
    1, /* -error bounds_check flag */
    0x50002, /* Ndr library version */
    0,
    0x8000253, /* MIDL Version 8.0.595 */
    0,
    0,
    0,  /* notify & notify_flag routine table */
    0x1, /* MIDL flag */
    0, /* cs routines */
    0,   /* proxy/server info */
    0
    };
#if _MSC_VER >= 1200
#pragma warning(pop)
#endif


#endif /* defined(_M_AMD64)*/

