// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


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

#pragma warning( disable: 4049 )  /* more than 64k source lines */


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 475
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__


#ifndef __ms2Dfrs2_h__
#define __ms2Dfrs2_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

/* header files for imported files */
//#include "ms-dtyp.h"

#ifdef __cplusplus
extern "C"{
#endif 


/* interface __MIDL_itf_ms2Dfrs2_0000_0000 */
/* [local] */ 

typedef GUID FRS_REPLICA_SET_ID;

typedef GUID FRS_CONTENT_SET_ID;

typedef GUID FRS_DATABASE_ID;

typedef GUID FRS_MEMBER_ID;

typedef GUID FRS_CONNECTION_ID;

typedef SYSTEMTIME EPOQUE;

typedef struct _FRS_VERSION_VECTOR
    {
    GUID dbGuid;
    DWORDLONG low;
    DWORDLONG high;
    } 	FRS_VERSION_VECTOR;

typedef struct _FRS_EPOQUE_VECTOR
    {
    GUID machine;
    EPOQUE epoque;
    } 	FRS_EPOQUE_VECTOR;

typedef struct _FRS_ID_GVSN
    {
    GUID uidDbGuid;
    DWORDLONG uidVersion;
    GUID gvsnDbGuid;
    DWORDLONG gvsnVersion;
    } 	FRS_ID_GVSN;

typedef struct _FRS_UPDATE
    {
    long present;
    long nameConflict;
    unsigned long attributes;
    FILETIME fence;
    FILETIME clock;
    FILETIME createTime;
    FRS_CONTENT_SET_ID contentSetId;
    unsigned char hash[ 20 ];
    unsigned char rdcSimilarity[ 16 ];
    GUID uidDbGuid;
    DWORDLONG uidVersion;
    GUID gvsnDbGuid;
    DWORDLONG gvsnVersion;
    GUID parentDbGuid;
    DWORDLONG parentVersion;
    /* [string] */ WCHAR name[ 261 ];
    long flags;
    } 	FRS_UPDATE;

typedef struct _FRS_UPDATE_CANCEL_DATA
    {
    FRS_UPDATE blockingUpdate;
    FRS_CONTENT_SET_ID contentSetId;
    FRS_DATABASE_ID gvsnDatabaseId;
    FRS_DATABASE_ID uidDatabaseId;
    FRS_DATABASE_ID parentDatabaseId;
    DWORDLONG gvsnVersion;
    DWORDLONG uidVersion;
    DWORDLONG parentVersion;
    unsigned long cancelType;
    long isUidValid;
    long isParentUidValid;
    long isBlockerValid;
    } 	FRS_UPDATE_CANCEL_DATA;

typedef struct _FRS_RDC_SOURCE_NEED
    {
    ULONGLONG needOffset;
    ULONGLONG needSize;
    } 	FRS_RDC_SOURCE_NEED;

typedef /* [public] */ 
enum __MIDL___MIDL_itf_ms2Dfrs2_0000_0000_0001
    {
        TRANSPORT_SUPPORTS_RDC_SIMILARITY	= 1
    } 	TransportFlags;

typedef /* [public][public][public] */ 
enum __MIDL___MIDL_itf_ms2Dfrs2_0000_0000_0002
    {
        RDC_UNCOMPRESSED	= 0,
        RDC_XPRESS	= 1
    } 	RDC_FILE_COMPRESSION_TYPES;

typedef /* [public] */ 
enum __MIDL___MIDL_itf_ms2Dfrs2_0000_0000_0003
    {
        RDC_FILTERGENERIC	= 0,
        RDC_FILTERMAX	= 1,
        RDC_FILTERPOINT	= 2,
        RDC_MAXALGORITHM	= 3
    } 	RDC_CHUNKER_ALGORITHM;

typedef /* [public][public] */ 
enum __MIDL___MIDL_itf_ms2Dfrs2_0000_0000_0004
    {
        UPDATE_REQUEST_ALL	= 0,
        UPDATE_REQUEST_TOMBSTONES	= 1,
        UPDATE_REQUEST_LIVE	= 2
    } 	UPDATE_REQUEST_TYPE;

typedef /* [public][public] */ 
enum __MIDL___MIDL_itf_ms2Dfrs2_0000_0000_0005
    {
        UPDATE_STATUS_DONE	= 2,
        UPDATE_STATUS_MORE	= 3
    } 	UPDATE_STATUS;

typedef /* [public][public] */ 
enum __MIDL___MIDL_itf_ms2Dfrs2_0000_0000_0006
    {
        RECORDS_STATUS_DONE	= 0,
        RECORDS_STATUS_MORE	= 1
    } 	RECORDS_STATUS;

typedef /* [public][public] */ 
enum __MIDL___MIDL_itf_ms2Dfrs2_0000_0000_0007
    {
        REQUEST_NORMAL_SYNC	= 0,
        REQUEST_SLOW_SYNC	= 1,
        REQUEST_SLAVE_SYNC	= 2
    } 	VERSION_REQUEST_TYPE;

typedef /* [public][public] */ 
enum __MIDL___MIDL_itf_ms2Dfrs2_0000_0000_0008
    {
        CHANGE_NOTIFY	= 0,
        CHANGE_ALL	= 2
    } 	VERSION_CHANGE_TYPE;

typedef /* [public][public] */ 
enum __MIDL___MIDL_itf_ms2Dfrs2_0000_0000_0009
    {
        SERVER_DEFAULT	= 0,
        STAGING_REQUIRED	= 1,
        RESTAGING_REQUIRED	= 2
    } 	FRS_REQUESTED_STAGING_POLICY;

typedef struct _FRS_RDC_PARAMETERS_FILTERMAX
    {
    /* [range] */ unsigned short horizonSize;
    /* [range] */ unsigned short windowSize;
    } 	FRS_RDC_PARAMETERS_FILTERMAX;

typedef struct _FRS_RDC_PARAMETERS_FILTERPOINT
    {
    unsigned short minChunkSize;
    unsigned short maxChunkSize;
    } 	FRS_RDC_PARAMETERS_FILTERPOINT;

typedef struct _FRS_RDC_PARAMETERS_GENERIC
    {
    unsigned short chunkerType;
    byte chunkerParameters[ 64 ];
    } 	FRS_RDC_PARAMETERS_GENERIC;

typedef /* [public][public][public] */ struct __MIDL___MIDL_itf_ms2Dfrs2_0000_0000_0010
    {
    unsigned short rdcChunkerAlgorithm;
    /* [switch_is] */ /* [switch_type] */ union 
        {
        /* [case()] */ FRS_RDC_PARAMETERS_GENERIC filterGeneric;
        /* [case()] */ FRS_RDC_PARAMETERS_FILTERMAX filterMax;
        /* [case()] */ FRS_RDC_PARAMETERS_FILTERPOINT filterPoint;
        } 	u;
    } 	FRS_RDC_PARAMETERS;

typedef struct _FRS_RDC_FILEINFO
    {
    DWORDLONG onDiskFileSize;
    DWORDLONG fileSizeEstimate;
    unsigned short rdcVersion;
    unsigned short rdcMinimumCompatibleVersion;
    /* [range] */ byte rdcSignatureLevels;
    RDC_FILE_COMPRESSION_TYPES compressionAlgorithm;
    /* [size_is] */ FRS_RDC_PARAMETERS rdcFilterParameters[ 1 ];
    } 	FRS_RDC_FILEINFO;

typedef struct _FRS_ASYNC_VERSION_VECTOR_RESPONSE
    {
    ULONGLONG vvGeneration;
    unsigned long versionVectorCount;
    /* [size_is] */ FRS_VERSION_VECTOR *versionVector;
    unsigned long epoqueVectorCount;
    /* [size_is] */ FRS_EPOQUE_VECTOR *epoqueVector;
    } 	FRS_ASYNC_VERSION_VECTOR_RESPONSE;

typedef struct _FRS_ASYNC_RESPONSE_CONTEXT
    {
    unsigned long sequenceNumber;
    DWORD status;
    FRS_ASYNC_VERSION_VECTOR_RESPONSE result;
    } 	FRS_ASYNC_RESPONSE_CONTEXT;

typedef struct ASYNC_pipe_BYTE_PIPE
    {
    RPC_STATUS (__RPC_USER * pull) (
        char * state,
        byte * buf,
        unsigned long esize,
        unsigned long * ecount );
    RPC_STATUS (__RPC_USER * push) (
        char * state,
        byte * buf,
        unsigned long ecount );
    RPC_STATUS (__RPC_USER * alloc) (
        char * state,
        unsigned long bsize,
        byte * * buf,
        unsigned long * bcount );
    char * state;
    } 	ASYNC_BYTE_PIPE;

typedef struct pipe_BYTE_PIPE
    {
    void (__RPC_USER * pull) (
        char * state,
        byte * buf,
        unsigned long esize,
        unsigned long * ecount );
    void (__RPC_USER * push) (
        char * state,
        byte * buf,
        unsigned long ecount );
    void (__RPC_USER * alloc) (
        char * state,
        unsigned long bsize,
        byte * * buf,
        unsigned long * bcount );
    char * state;
    } 	BYTE_PIPE;



extern RPC_IF_HANDLE __MIDL_itf_ms2Dfrs2_0000_0000_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_ms2Dfrs2_0000_0000_v0_0_s_ifspec;

#ifndef __FrsTransport_INTERFACE_DEFINED__
#define __FrsTransport_INTERFACE_DEFINED__

/* interface FrsTransport */
/* [strict_context_handle][explicit_handle][version][uuid] */ 

DWORD CheckConnectivity( 
    /* [in] */ handle_t IDL_handle,
    /* [in] */ FRS_REPLICA_SET_ID replicaSetId,
    /* [in] */ FRS_CONNECTION_ID connectionId);

DWORD EstablishConnection( 
    /* [in] */ handle_t IDL_handle,
    /* [in] */ FRS_REPLICA_SET_ID replicaSetId,
    /* [in] */ FRS_CONNECTION_ID connectionId,
    /* [in] */ DWORD downstreamProtocolVersion,
    /* [in] */ DWORD downstreamFlags,
    /* [out] */ DWORD *upstreamProtocolVersion,
    /* [out] */ DWORD *upstreamFlags);

DWORD EstablishSession( 
    /* [in] */ handle_t IDL_handle,
    /* [in] */ FRS_CONNECTION_ID connectionId,
    /* [in] */ FRS_CONTENT_SET_ID contentSetId);

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
    /* [out] */ DWORDLONG *gvsnVersion);

/* [async] */ void  RequestVersionVector( 
    /* [in] */ PRPC_ASYNC_STATE RequestVersionVector_AsyncHandle,
    /* [in] */ handle_t IDL_handle,
    /* [in] */ DWORD sequenceNumber,
    /* [in] */ FRS_CONNECTION_ID connectionId,
    /* [in] */ FRS_CONTENT_SET_ID contentSetId,
    /* [range][in] */ VERSION_REQUEST_TYPE requestType,
    /* [range][in] */ VERSION_CHANGE_TYPE changeType,
    /* [in] */ ULONGLONG vvGeneration);

/* [async] */ void  AsyncPoll( 
    /* [in] */ PRPC_ASYNC_STATE AsyncPoll_AsyncHandle,
    /* [in] */ handle_t IDL_handle,
    /* [in] */ FRS_CONNECTION_ID connectionId,
    /* [out] */ FRS_ASYNC_RESPONSE_CONTEXT *response);

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
    /* [out] */ RECORDS_STATUS *recordsStatus);

DWORD UpdateCancel( 
    /* [in] */ handle_t IDL_handle,
    /* [in] */ FRS_CONNECTION_ID connectionId,
    /* [in] */ FRS_UPDATE_CANCEL_DATA cancelData);

typedef /* [context_handle] */ void *PFRS_SERVER_CONTEXT;

DWORD RawGetFileData( 
    /* [out][in] */ PFRS_SERVER_CONTEXT *serverContext,
    /* [length_is][size_is][out] */ byte *dataBuffer,
    /* [range][in] */ DWORD bufferSize,
    /* [out] */ DWORD *sizeRead,
    /* [out] */ long *isEndOfFile);

DWORD RdcGetSignatures( 
    /* [in] */ PFRS_SERVER_CONTEXT serverContext,
    /* [range][in] */ byte level,
    /* [in] */ DWORDLONG offset,
    /* [length_is][size_is][out] */ byte *buffer,
    /* [range][in] */ DWORD length,
    /* [out] */ DWORD *sizeRead);

DWORD RdcPushSourceNeeds( 
    /* [in] */ PFRS_SERVER_CONTEXT serverContext,
    /* [size_is][in] */ FRS_RDC_SOURCE_NEED *sourceNeeds,
    /* [range][in] */ DWORD needCount);

DWORD RdcGetFileData( 
    /* [in] */ PFRS_SERVER_CONTEXT serverContext,
    /* [length_is][size_is][out] */ byte *dataBuffer,
    /* [range][in] */ DWORD bufferSize,
    /* [out] */ DWORD *sizeReturned);

DWORD RdcClose( 
    /* [out][in] */ PFRS_SERVER_CONTEXT *serverContext);

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
    /* [out] */ long *isEndOfFile);

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
    /* [out] */ long *isEndOfFile);

/* [async] */ void  RawGetFileDataAsync( 
    /* [in] */ PRPC_ASYNC_STATE RawGetFileDataAsync_AsyncHandle,
    /* [in] */ PFRS_SERVER_CONTEXT serverContext,
    /* [out] */ ASYNC_BYTE_PIPE *bytePipe);

/* [async] */ void  RdcGetFileDataAsync( 
    /* [in] */ PRPC_ASYNC_STATE RdcGetFileDataAsync_AsyncHandle,
    /* [in] */ PFRS_SERVER_CONTEXT serverContext,
    /* [out] */ ASYNC_BYTE_PIPE *bytePipe);



extern RPC_IF_HANDLE FrsTransport_v1_0_c_ifspec;
extern RPC_IF_HANDLE FrsTransport_v1_0_s_ifspec;
#endif /* __FrsTransport_INTERFACE_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

void __RPC_USER PFRS_SERVER_CONTEXT_rundown( PFRS_SERVER_CONTEXT );

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


