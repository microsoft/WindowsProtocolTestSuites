

/* this ALWAYS GENERATED file contains the RPC client stubs */


 /* File created by MIDL compiler version 8.00.0595 */
/* at Fri Nov 04 11:26:24 2016
 */
/* Compiler settings for ms-drsr.idl:
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

#include "ms-drsr.h"

#define TYPE_FORMAT_STRING_SIZE   8043                              
#define PROC_FORMAT_STRING_SIZE   2113                              
#define EXPR_FORMAT_STRING_SIZE   1                                 
#define TRANSMIT_AS_TABLE_SIZE    0            
#define WIRE_MARSHAL_TABLE_SIZE   0            

typedef struct _ms2Ddrsr_MIDL_TYPE_FORMAT_STRING
    {
    short          Pad;
    unsigned char  Format[ TYPE_FORMAT_STRING_SIZE ];
    } ms2Ddrsr_MIDL_TYPE_FORMAT_STRING;

typedef struct _ms2Ddrsr_MIDL_PROC_FORMAT_STRING
    {
    short          Pad;
    unsigned char  Format[ PROC_FORMAT_STRING_SIZE ];
    } ms2Ddrsr_MIDL_PROC_FORMAT_STRING;

typedef struct _ms2Ddrsr_MIDL_EXPR_FORMAT_STRING
    {
    long          Pad;
    unsigned char  Format[ EXPR_FORMAT_STRING_SIZE ];
    } ms2Ddrsr_MIDL_EXPR_FORMAT_STRING;


static const RPC_SYNTAX_IDENTIFIER  _RpcTransferSyntax = 
{{0x8A885D04,0x1CEB,0x11C9,{0x9F,0xE8,0x08,0x00,0x2B,0x10,0x48,0x60}},{2,0}};


extern const ms2Ddrsr_MIDL_TYPE_FORMAT_STRING ms2Ddrsr__MIDL_TypeFormatString;
extern const ms2Ddrsr_MIDL_PROC_FORMAT_STRING ms2Ddrsr__MIDL_ProcFormatString;
extern const ms2Ddrsr_MIDL_EXPR_FORMAT_STRING ms2Ddrsr__MIDL_ExprFormatString;

#define GENERIC_BINDING_TABLE_SIZE   0            


/* Standard interface: drsuapi, ver. 4.0,
   GUID={0xe3514235,0x4b06,0x11d1,{0xab,0x04,0x00,0xc0,0x4f,0xc2,0xdc,0xd2}} */



static const RPC_CLIENT_INTERFACE drsuapi___RpcClientInterface =
    {
    sizeof(RPC_CLIENT_INTERFACE),
    {{0xe3514235,0x4b06,0x11d1,{0xab,0x04,0x00,0xc0,0x4f,0xc2,0xdc,0xd2}},{4,0}},
    {{0x8A885D04,0x1CEB,0x11C9,{0x9F,0xE8,0x08,0x00,0x2B,0x10,0x48,0x60}},{2,0}},
    0,
    0,
    0,
    0,
    0,
    0x00000000
    };
RPC_IF_HANDLE drsuapi_v4_0_c_ifspec = (RPC_IF_HANDLE)& drsuapi___RpcClientInterface;

extern const MIDL_STUB_DESC drsuapi_StubDesc;

static RPC_BINDING_HANDLE drsuapi__MIDL_AutoBindHandle;


ULONG IDL_DRSBind( 
    /* [in] */ handle_t rpc_handle,
    /* [unique][in] */ UUID *puuidClientDsa,
    /* [unique][in] */ DRS_EXTENSIONS *pextClient,
    /* [out] */ DRS_EXTENSIONS **ppextServer,
    /* [ref][out] */ DRS_HANDLE *phDrs)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[0],
                  rpc_handle,
                  puuidClientDsa,
                  pextClient,
                  ppextServer,
                  phDrs);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSUnbind( 
    /* [ref][out][in] */ DRS_HANDLE *phDrs)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[60],
                  phDrs);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSReplicaSync( 
    /* [ref][in] */ DRS_HANDLE hDrs,
    /* [in] */ DWORD dwVersion,
    /* [switch_is][ref][in] */ DRS_MSG_REPSYNC *pmsgSync)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[104],
                  hDrs,
                  dwVersion,
                  pmsgSync);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSGetNCChanges( 
    /* [ref][in] */ DRS_HANDLE hDrs,
    /* [in] */ DWORD dwInVersion,
    /* [switch_is][ref][in] */ DRS_MSG_GETCHGREQ *pmsgIn,
    /* [ref][out] */ DWORD *pdwOutVersion,
    /* [switch_is][ref][out] */ DRS_MSG_GETCHGREPLY *pmsgOut)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[160],
                  hDrs,
                  dwInVersion,
                  pmsgIn,
                  pdwOutVersion,
                  pmsgOut);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSUpdateRefs( 
    /* [ref][in] */ DRS_HANDLE hDrs,
    /* [in] */ DWORD dwVersion,
    /* [switch_is][ref][in] */ DRS_MSG_UPDREFS *pmsgUpdRefs)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[228],
                  hDrs,
                  dwVersion,
                  pmsgUpdRefs);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSReplicaAdd( 
    /* [ref][in] */ DRS_HANDLE hDrs,
    /* [in] */ DWORD dwVersion,
    /* [switch_is][ref][in] */ DRS_MSG_REPADD *pmsgAdd)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[284],
                  hDrs,
                  dwVersion,
                  pmsgAdd);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSReplicaDel( 
    /* [ref][in] */ DRS_HANDLE hDrs,
    /* [in] */ DWORD dwVersion,
    /* [switch_is][ref][in] */ DRS_MSG_REPDEL *pmsgDel)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[340],
                  hDrs,
                  dwVersion,
                  pmsgDel);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSReplicaModify( 
    /* [ref][in] */ DRS_HANDLE hDrs,
    /* [in] */ DWORD dwVersion,
    /* [switch_is][ref][in] */ DRS_MSG_REPMOD *pmsgMod)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[396],
                  hDrs,
                  dwVersion,
                  pmsgMod);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSVerifyNames( 
    /* [ref][in] */ DRS_HANDLE hDrs,
    /* [in] */ DWORD dwInVersion,
    /* [switch_is][ref][in] */ DRS_MSG_VERIFYREQ *pmsgIn,
    /* [ref][out] */ DWORD *pdwOutVersion,
    /* [switch_is][ref][out] */ DRS_MSG_VERIFYREPLY *pmsgOut)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[452],
                  hDrs,
                  dwInVersion,
                  pmsgIn,
                  pdwOutVersion,
                  pmsgOut);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSGetMemberships( 
    /* [ref][in] */ DRS_HANDLE hDrs,
    /* [in] */ DWORD dwInVersion,
    /* [switch_is][ref][in] */ DRS_MSG_REVMEMB_REQ *pmsgIn,
    /* [ref][out] */ DWORD *pdwOutVersion,
    /* [switch_is][ref][out] */ DRS_MSG_REVMEMB_REPLY *pmsgOut)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[520],
                  hDrs,
                  dwInVersion,
                  pmsgIn,
                  pdwOutVersion,
                  pmsgOut);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSInterDomainMove( 
    /* [ref][in] */ DRS_HANDLE hDrs,
    /* [in] */ DWORD dwInVersion,
    /* [switch_is][ref][in] */ DRS_MSG_MOVEREQ *pmsgIn,
    /* [ref][out] */ DWORD *pdwOutVersion,
    /* [switch_is][ref][out] */ DRS_MSG_MOVEREPLY *pmsgOut)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[588],
                  hDrs,
                  dwInVersion,
                  pmsgIn,
                  pdwOutVersion,
                  pmsgOut);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSGetNT4ChangeLog( 
    /* [ref][in] */ DRS_HANDLE hDrs,
    /* [in] */ DWORD dwInVersion,
    /* [switch_is][ref][in] */ DRS_MSG_NT4_CHGLOG_REQ *pmsgIn,
    /* [ref][out] */ DWORD *pdwOutVersion,
    /* [switch_is][ref][out] */ DRS_MSG_NT4_CHGLOG_REPLY *pmsgOut)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[656],
                  hDrs,
                  dwInVersion,
                  pmsgIn,
                  pdwOutVersion,
                  pmsgOut);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSCrackNames( 
    /* [ref][in] */ DRS_HANDLE hDrs,
    /* [in] */ DWORD dwInVersion,
    /* [switch_is][ref][in] */ DRS_MSG_CRACKREQ *pmsgIn,
    /* [ref][out] */ DWORD *pdwOutVersion,
    /* [switch_is][ref][out] */ DRS_MSG_CRACKREPLY *pmsgOut)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[724],
                  hDrs,
                  dwInVersion,
                  pmsgIn,
                  pdwOutVersion,
                  pmsgOut);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSWriteSPN( 
    /* [ref][in] */ DRS_HANDLE hDrs,
    /* [in] */ DWORD dwInVersion,
    /* [switch_is][ref][in] */ DRS_MSG_SPNREQ *pmsgIn,
    /* [ref][out] */ DWORD *pdwOutVersion,
    /* [switch_is][ref][out] */ DRS_MSG_SPNREPLY *pmsgOut)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[792],
                  hDrs,
                  dwInVersion,
                  pmsgIn,
                  pdwOutVersion,
                  pmsgOut);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSRemoveDsServer( 
    /* [ref][in] */ DRS_HANDLE hDrs,
    /* [in] */ DWORD dwInVersion,
    /* [switch_is][ref][in] */ DRS_MSG_RMSVRREQ *pmsgIn,
    /* [ref][out] */ DWORD *pdwOutVersion,
    /* [switch_is][ref][out] */ DRS_MSG_RMSVRREPLY *pmsgOut)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[860],
                  hDrs,
                  dwInVersion,
                  pmsgIn,
                  pdwOutVersion,
                  pmsgOut);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSRemoveDsDomain( 
    /* [ref][in] */ DRS_HANDLE hDrs,
    /* [in] */ DWORD dwInVersion,
    /* [switch_is][ref][in] */ DRS_MSG_RMDMNREQ *pmsgIn,
    /* [ref][out] */ DWORD *pdwOutVersion,
    /* [switch_is][ref][out] */ DRS_MSG_RMDMNREPLY *pmsgOut)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[928],
                  hDrs,
                  dwInVersion,
                  pmsgIn,
                  pdwOutVersion,
                  pmsgOut);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSDomainControllerInfo( 
    /* [ref][in] */ DRS_HANDLE hDrs,
    /* [in] */ DWORD dwInVersion,
    /* [switch_is][ref][in] */ DRS_MSG_DCINFOREQ *pmsgIn,
    /* [ref][out] */ DWORD *pdwOutVersion,
    /* [switch_is][ref][out] */ DRS_MSG_DCINFOREPLY *pmsgOut)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[996],
                  hDrs,
                  dwInVersion,
                  pmsgIn,
                  pdwOutVersion,
                  pmsgOut);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSAddEntry( 
    /* [ref][in] */ DRS_HANDLE hDrs,
    /* [in] */ DWORD dwInVersion,
    /* [switch_is][ref][in] */ DRS_MSG_ADDENTRYREQ *pmsgIn,
    /* [ref][out] */ DWORD *pdwOutVersion,
    /* [switch_is][ref][out] */ DRS_MSG_ADDENTRYREPLY *pmsgOut)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[1064],
                  hDrs,
                  dwInVersion,
                  pmsgIn,
                  pdwOutVersion,
                  pmsgOut);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSExecuteKCC( 
    /* [ref][in] */ DRS_HANDLE hDrs,
    /* [in] */ DWORD dwInVersion,
    /* [switch_is][ref][in] */ DRS_MSG_KCC_EXECUTE *pmsgIn)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[1132],
                  hDrs,
                  dwInVersion,
                  pmsgIn);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSGetReplInfo( 
    /* [ref][in] */ DRS_HANDLE hDrs,
    /* [in] */ DWORD dwInVersion,
    /* [switch_is][ref][in] */ DRS_MSG_GETREPLINFO_REQ *pmsgIn,
    /* [ref][out] */ DWORD *pdwOutVersion,
    /* [switch_is][ref][out] */ DRS_MSG_GETREPLINFO_REPLY *pmsgOut)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[1188],
                  hDrs,
                  dwInVersion,
                  pmsgIn,
                  pdwOutVersion,
                  pmsgOut);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSAddSidHistory( 
    /* [ref][in] */ DRS_HANDLE hDrs,
    /* [in] */ DWORD dwInVersion,
    /* [switch_is][ref][in] */ DRS_MSG_ADDSIDREQ *pmsgIn,
    /* [ref][out] */ DWORD *pdwOutVersion,
    /* [switch_is][ref][out] */ DRS_MSG_ADDSIDREPLY *pmsgOut)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[1256],
                  hDrs,
                  dwInVersion,
                  pmsgIn,
                  pdwOutVersion,
                  pmsgOut);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSGetMemberships2( 
    /* [ref][in] */ DRS_HANDLE hDrs,
    /* [in] */ DWORD dwInVersion,
    /* [switch_is][ref][in] */ DRS_MSG_GETMEMBERSHIPS2_REQ *pmsgIn,
    /* [ref][out] */ DWORD *pdwOutVersion,
    /* [switch_is][ref][out] */ DRS_MSG_GETMEMBERSHIPS2_REPLY *pmsgOut)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[1324],
                  hDrs,
                  dwInVersion,
                  pmsgIn,
                  pdwOutVersion,
                  pmsgOut);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSReplicaVerifyObjects( 
    /* [ref][in] */ DRS_HANDLE hDrs,
    /* [in] */ DWORD dwVersion,
    /* [switch_is][ref][in] */ DRS_MSG_REPVERIFYOBJ *pmsgVerify)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[1392],
                  hDrs,
                  dwVersion,
                  pmsgVerify);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSGetObjectExistence( 
    /* [ref][in] */ DRS_HANDLE hDrs,
    /* [in] */ DWORD dwInVersion,
    /* [switch_is][ref][in] */ DRS_MSG_EXISTREQ *pmsgIn,
    /* [ref][out] */ DWORD *pdwOutVersion,
    /* [switch_is][ref][out] */ DRS_MSG_EXISTREPLY *pmsgOut)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[1448],
                  hDrs,
                  dwInVersion,
                  pmsgIn,
                  pdwOutVersion,
                  pmsgOut);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSQuerySitesByCost( 
    /* [ref][in] */ DRS_HANDLE hDrs,
    /* [in] */ DWORD dwInVersion,
    /* [switch_is][ref][in] */ DRS_MSG_QUERYSITESREQ *pmsgIn,
    /* [ref][out] */ DWORD *pdwOutVersion,
    /* [switch_is][ref][out] */ DRS_MSG_QUERYSITESREPLY *pmsgOut)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[1516],
                  hDrs,
                  dwInVersion,
                  pmsgIn,
                  pdwOutVersion,
                  pmsgOut);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSInitDemotion( 
    /* [ref][in] */ DRS_HANDLE hDrs,
    /* [in] */ DWORD dwInVersion,
    /* [switch_is][ref][in] */ DRS_MSG_INIT_DEMOTIONREQ *pmsgIn,
    /* [ref][out] */ DWORD *pdwOutVersion,
    /* [switch_is][ref][out] */ DRS_MSG_INIT_DEMOTIONREPLY *pmsgOut)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[1584],
                  hDrs,
                  dwInVersion,
                  pmsgIn,
                  pdwOutVersion,
                  pmsgOut);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSReplicaDemotion( 
    /* [ref][in] */ DRS_HANDLE hDrs,
    /* [in] */ DWORD dwInVersion,
    /* [switch_is][ref][in] */ DRS_MSG_REPLICA_DEMOTIONREQ *pmsgIn,
    /* [ref][out] */ DWORD *pdwOutVersion,
    /* [switch_is][ref][out] */ DRS_MSG_REPLICA_DEMOTIONREPLY *pmsgOut)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[1652],
                  hDrs,
                  dwInVersion,
                  pmsgIn,
                  pdwOutVersion,
                  pmsgOut);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSFinishDemotion( 
    /* [ref][in] */ DRS_HANDLE hDrs,
    /* [in] */ DWORD dwInVersion,
    /* [switch_is][ref][in] */ DRS_MSG_FINISH_DEMOTIONREQ *pmsgIn,
    /* [ref][out] */ DWORD *pdwOutVersion,
    /* [switch_is][ref][out] */ DRS_MSG_FINISH_DEMOTIONREPLY *pmsgOut)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[1720],
                  hDrs,
                  dwInVersion,
                  pmsgIn,
                  pdwOutVersion,
                  pmsgOut);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSAddCloneDC( 
    /* [ref][in] */ DRS_HANDLE hDrs,
    /* [in] */ DWORD dwInVersion,
    /* [switch_is][ref][in] */ DRS_MSG_ADDCLONEDCREQ *pmsgIn,
    /* [ref][out] */ DWORD *pdwOutVersion,
    /* [switch_is][ref][out] */ DRS_MSG_ADDCLONEDCREPLY *pmsgOut)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[1788],
                  hDrs,
                  dwInVersion,
                  pmsgIn,
                  pdwOutVersion,
                  pmsgOut);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSWriteNgcKey( 
    /* [ref][in] */ DRS_HANDLE hDrs,
    /* [in] */ DWORD dwInVersion,
    /* [switch_is][ref][in] */ DRS_MSG_WRITENGCKEYREQ *pmsgIn,
    /* [ref][out] */ DWORD *pdwOutVersion,
    /* [switch_is][ref][out] */ DRS_MSG_WRITENGCKEYREPLY *pmsgOut)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[1856],
                  hDrs,
                  dwInVersion,
                  pmsgIn,
                  pdwOutVersion,
                  pmsgOut);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DRSReadNgcKey( 
    /* [ref][in] */ DRS_HANDLE hDrs,
    /* [in] */ DWORD dwInVersion,
    /* [switch_is][ref][in] */ DRS_MSG_READNGCKEYREQ *pmsgIn,
    /* [ref][out] */ DWORD *pdwOutVersion,
    /* [switch_is][ref][out] */ DRS_MSG_READNGCKEYREPLY *pmsgOut)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&drsuapi_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[1924],
                  hDrs,
                  dwInVersion,
                  pmsgIn,
                  pdwOutVersion,
                  pmsgOut);
    return ( ULONG  )_RetVal.Simple;
    
}


/* Standard interface: dsaop, ver. 1.0,
   GUID={0x7c44d7d4,0x31d5,0x424c,{0xbd,0x5e,0x2b,0x3e,0x1f,0x32,0x3d,0x22}} */



static const RPC_CLIENT_INTERFACE dsaop___RpcClientInterface =
    {
    sizeof(RPC_CLIENT_INTERFACE),
    {{0x7c44d7d4,0x31d5,0x424c,{0xbd,0x5e,0x2b,0x3e,0x1f,0x32,0x3d,0x22}},{1,0}},
    {{0x8A885D04,0x1CEB,0x11C9,{0x9F,0xE8,0x08,0x00,0x2B,0x10,0x48,0x60}},{2,0}},
    0,
    0,
    0,
    0,
    0,
    0x00000000
    };
RPC_IF_HANDLE dsaop_v1_0_c_ifspec = (RPC_IF_HANDLE)& dsaop___RpcClientInterface;

extern const MIDL_STUB_DESC dsaop_StubDesc;

static RPC_BINDING_HANDLE dsaop__MIDL_AutoBindHandle;


ULONG IDL_DSAPrepareScript( 
    /* [in] */ handle_t hRpc,
    /* [in] */ DWORD dwInVersion,
    /* [switch_is][ref][in] */ DSA_MSG_PREPARE_SCRIPT_REQ *pmsgIn,
    /* [ref][out] */ DWORD *pdwOutVersion,
    /* [switch_is][ref][out] */ DSA_MSG_PREPARE_SCRIPT_REPLY *pmsgOut)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&dsaop_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[1992],
                  hRpc,
                  dwInVersion,
                  pmsgIn,
                  pdwOutVersion,
                  pmsgOut);
    return ( ULONG  )_RetVal.Simple;
    
}


ULONG IDL_DSAExecuteScript( 
    /* [in] */ handle_t hRpc,
    /* [in] */ DWORD dwInVersion,
    /* [switch_is][ref][in] */ DSA_MSG_EXECUTE_SCRIPT_REQ *pmsgIn,
    /* [ref][out] */ DWORD *pdwOutVersion,
    /* [switch_is][ref][out] */ DSA_MSG_EXECUTE_SCRIPT_REPLY *pmsgOut)
{

    CLIENT_CALL_RETURN _RetVal;

    _RetVal = NdrClientCall2(
                  ( PMIDL_STUB_DESC  )&dsaop_StubDesc,
                  (PFORMAT_STRING) &ms2Ddrsr__MIDL_ProcFormatString.Format[2052],
                  hRpc,
                  dwInVersion,
                  pmsgIn,
                  pdwOutVersion,
                  pmsgOut);
    return ( ULONG  )_RetVal.Simple;
    
}


#if !defined(__RPC_WIN64__)
#error  Invalid build platform for this stub.
#endif

static const ms2Ddrsr_MIDL_PROC_FORMAT_STRING ms2Ddrsr__MIDL_ProcFormatString =
    {
        0,
        {

	/* Procedure IDL_DRSBind */

			0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/*  2 */	NdrFcLong( 0x0 ),	/* 0 */
/*  6 */	NdrFcShort( 0x0 ),	/* 0 */
/*  8 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 10 */	0x32,		/* FC_BIND_PRIMITIVE */
			0x0,		/* 0 */
/* 12 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 14 */	NdrFcShort( 0x44 ),	/* 68 */
/* 16 */	NdrFcShort( 0x40 ),	/* 64 */
/* 18 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0x5,		/* 5 */
/* 20 */	0xa,		/* 10 */
			0x47,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, has range on conformance */
/* 22 */	NdrFcShort( 0x1 ),	/* 1 */
/* 24 */	NdrFcShort( 0x1 ),	/* 1 */
/* 26 */	NdrFcShort( 0x0 ),	/* 0 */
/* 28 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter rpc_handle */

/* 30 */	NdrFcShort( 0xa ),	/* Flags:  must free, in, */
/* 32 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 34 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

	/* Parameter puuidClientDsa */

/* 36 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 38 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 40 */	NdrFcShort( 0x18 ),	/* Type Offset=24 */

	/* Parameter pextClient */

/* 42 */	NdrFcShort( 0x2013 ),	/* Flags:  must size, must free, out, srv alloc size=8 */
/* 44 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 46 */	NdrFcShort( 0x4a ),	/* Type Offset=74 */

	/* Parameter ppextServer */

/* 48 */	NdrFcShort( 0x110 ),	/* Flags:  out, simple ref, */
/* 50 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 52 */	NdrFcShort( 0x52 ),	/* Type Offset=82 */

	/* Parameter phDrs */

/* 54 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 56 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 58 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSUnbind */


	/* Return value */

/* 60 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 62 */	NdrFcLong( 0x0 ),	/* 0 */
/* 66 */	NdrFcShort( 0x1 ),	/* 1 */
/* 68 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 70 */	0x30,		/* FC_BIND_CONTEXT */
			0xe0,		/* Ctxt flags:  via ptr, in, out, */
/* 72 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 74 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 76 */	NdrFcShort( 0x38 ),	/* 56 */
/* 78 */	NdrFcShort( 0x40 ),	/* 64 */
/* 80 */	0x44,		/* Oi2 Flags:  has return, has ext, */
			0x2,		/* 2 */
/* 82 */	0xa,		/* 10 */
			0x41,		/* Ext Flags:  new corr desc, has range on conformance */
/* 84 */	NdrFcShort( 0x0 ),	/* 0 */
/* 86 */	NdrFcShort( 0x0 ),	/* 0 */
/* 88 */	NdrFcShort( 0x0 ),	/* 0 */
/* 90 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter phDrs */

/* 92 */	NdrFcShort( 0x118 ),	/* Flags:  in, out, simple ref, */
/* 94 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 96 */	NdrFcShort( 0x5a ),	/* Type Offset=90 */

	/* Return value */

/* 98 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 100 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 102 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSReplicaSync */

/* 104 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 106 */	NdrFcLong( 0x0 ),	/* 0 */
/* 110 */	NdrFcShort( 0x2 ),	/* 2 */
/* 112 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 114 */	0x30,		/* FC_BIND_CONTEXT */
			0x40,		/* Ctxt flags:  in, */
/* 116 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 118 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 120 */	NdrFcShort( 0x2c ),	/* 44 */
/* 122 */	NdrFcShort( 0x8 ),	/* 8 */
/* 124 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
			0x4,		/* 4 */
/* 126 */	0xa,		/* 10 */
			0x45,		/* Ext Flags:  new corr desc, srv corr check, has range on conformance */
/* 128 */	NdrFcShort( 0x0 ),	/* 0 */
/* 130 */	NdrFcShort( 0x1 ),	/* 1 */
/* 132 */	NdrFcShort( 0x0 ),	/* 0 */
/* 134 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hDrs */

/* 136 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 138 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 140 */	NdrFcShort( 0x5e ),	/* Type Offset=94 */

	/* Parameter dwVersion */

/* 142 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 144 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 146 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgSync */

/* 148 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 150 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 152 */	NdrFcShort( 0x66 ),	/* Type Offset=102 */

	/* Return value */

/* 154 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 156 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 158 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSGetNCChanges */

/* 160 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 162 */	NdrFcLong( 0x0 ),	/* 0 */
/* 166 */	NdrFcShort( 0x3 ),	/* 3 */
/* 168 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 170 */	0x30,		/* FC_BIND_CONTEXT */
			0x40,		/* Ctxt flags:  in, */
/* 172 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 174 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 176 */	NdrFcShort( 0x2c ),	/* 44 */
/* 178 */	NdrFcShort( 0x24 ),	/* 36 */
/* 180 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0x6,		/* 6 */
/* 182 */	0xa,		/* 10 */
			0x47,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, has range on conformance */
/* 184 */	NdrFcShort( 0x1 ),	/* 1 */
/* 186 */	NdrFcShort( 0x1 ),	/* 1 */
/* 188 */	NdrFcShort( 0x0 ),	/* 0 */
/* 190 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hDrs */

/* 192 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 194 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 196 */	NdrFcShort( 0x5e ),	/* Type Offset=94 */

	/* Parameter dwInVersion */

/* 198 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 200 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 202 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgIn */

/* 204 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 206 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 208 */	NdrFcShort( 0xdc ),	/* Type Offset=220 */

	/* Parameter pdwOutVersion */

/* 210 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 212 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 214 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 216 */	NdrFcShort( 0x113 ),	/* Flags:  must size, must free, out, simple ref, */
/* 218 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 220 */	NdrFcShort( 0x338 ),	/* Type Offset=824 */

	/* Return value */

/* 222 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 224 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 226 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSUpdateRefs */

/* 228 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 230 */	NdrFcLong( 0x0 ),	/* 0 */
/* 234 */	NdrFcShort( 0x4 ),	/* 4 */
/* 236 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 238 */	0x30,		/* FC_BIND_CONTEXT */
			0x40,		/* Ctxt flags:  in, */
/* 240 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 242 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 244 */	NdrFcShort( 0x2c ),	/* 44 */
/* 246 */	NdrFcShort( 0x8 ),	/* 8 */
/* 248 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
			0x4,		/* 4 */
/* 250 */	0xa,		/* 10 */
			0x45,		/* Ext Flags:  new corr desc, srv corr check, has range on conformance */
/* 252 */	NdrFcShort( 0x0 ),	/* 0 */
/* 254 */	NdrFcShort( 0x1 ),	/* 1 */
/* 256 */	NdrFcShort( 0x0 ),	/* 0 */
/* 258 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hDrs */

/* 260 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 262 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 264 */	NdrFcShort( 0x5e ),	/* Type Offset=94 */

	/* Parameter dwVersion */

/* 266 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 268 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 270 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgUpdRefs */

/* 272 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 274 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 276 */	NdrFcShort( 0x6a8 ),	/* Type Offset=1704 */

	/* Return value */

/* 278 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 280 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 282 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSReplicaAdd */

/* 284 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 286 */	NdrFcLong( 0x0 ),	/* 0 */
/* 290 */	NdrFcShort( 0x5 ),	/* 5 */
/* 292 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 294 */	0x30,		/* FC_BIND_CONTEXT */
			0x40,		/* Ctxt flags:  in, */
/* 296 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 298 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 300 */	NdrFcShort( 0x2c ),	/* 44 */
/* 302 */	NdrFcShort( 0x8 ),	/* 8 */
/* 304 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
			0x4,		/* 4 */
/* 306 */	0xa,		/* 10 */
			0x45,		/* Ext Flags:  new corr desc, srv corr check, has range on conformance */
/* 308 */	NdrFcShort( 0x0 ),	/* 0 */
/* 310 */	NdrFcShort( 0x1 ),	/* 1 */
/* 312 */	NdrFcShort( 0x0 ),	/* 0 */
/* 314 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hDrs */

/* 316 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 318 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 320 */	NdrFcShort( 0x5e ),	/* Type Offset=94 */

	/* Parameter dwVersion */

/* 322 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 324 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 326 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgAdd */

/* 328 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 330 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 332 */	NdrFcShort( 0x6e6 ),	/* Type Offset=1766 */

	/* Return value */

/* 334 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 336 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 338 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSReplicaDel */

/* 340 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 342 */	NdrFcLong( 0x0 ),	/* 0 */
/* 346 */	NdrFcShort( 0x6 ),	/* 6 */
/* 348 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 350 */	0x30,		/* FC_BIND_CONTEXT */
			0x40,		/* Ctxt flags:  in, */
/* 352 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 354 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 356 */	NdrFcShort( 0x2c ),	/* 44 */
/* 358 */	NdrFcShort( 0x8 ),	/* 8 */
/* 360 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
			0x4,		/* 4 */
/* 362 */	0xa,		/* 10 */
			0x45,		/* Ext Flags:  new corr desc, srv corr check, has range on conformance */
/* 364 */	NdrFcShort( 0x0 ),	/* 0 */
/* 366 */	NdrFcShort( 0x1 ),	/* 1 */
/* 368 */	NdrFcShort( 0x0 ),	/* 0 */
/* 370 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hDrs */

/* 372 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 374 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 376 */	NdrFcShort( 0x5e ),	/* Type Offset=94 */

	/* Parameter dwVersion */

/* 378 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 380 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 382 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgDel */

/* 384 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 386 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 388 */	NdrFcShort( 0x75a ),	/* Type Offset=1882 */

	/* Return value */

/* 390 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 392 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 394 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSReplicaModify */

/* 396 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 398 */	NdrFcLong( 0x0 ),	/* 0 */
/* 402 */	NdrFcShort( 0x7 ),	/* 7 */
/* 404 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 406 */	0x30,		/* FC_BIND_CONTEXT */
			0x40,		/* Ctxt flags:  in, */
/* 408 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 410 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 412 */	NdrFcShort( 0x2c ),	/* 44 */
/* 414 */	NdrFcShort( 0x8 ),	/* 8 */
/* 416 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
			0x4,		/* 4 */
/* 418 */	0xa,		/* 10 */
			0x45,		/* Ext Flags:  new corr desc, srv corr check, has range on conformance */
/* 420 */	NdrFcShort( 0x0 ),	/* 0 */
/* 422 */	NdrFcShort( 0x1 ),	/* 1 */
/* 424 */	NdrFcShort( 0x0 ),	/* 0 */
/* 426 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hDrs */

/* 428 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 430 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 432 */	NdrFcShort( 0x5e ),	/* Type Offset=94 */

	/* Parameter dwVersion */

/* 434 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 436 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 438 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgMod */

/* 440 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 442 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 444 */	NdrFcShort( 0x794 ),	/* Type Offset=1940 */

	/* Return value */

/* 446 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 448 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 450 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSVerifyNames */

/* 452 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 454 */	NdrFcLong( 0x0 ),	/* 0 */
/* 458 */	NdrFcShort( 0x8 ),	/* 8 */
/* 460 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 462 */	0x30,		/* FC_BIND_CONTEXT */
			0x40,		/* Ctxt flags:  in, */
/* 464 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 466 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 468 */	NdrFcShort( 0x2c ),	/* 44 */
/* 470 */	NdrFcShort( 0x24 ),	/* 36 */
/* 472 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0x6,		/* 6 */
/* 474 */	0xa,		/* 10 */
			0x47,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, has range on conformance */
/* 476 */	NdrFcShort( 0x1 ),	/* 1 */
/* 478 */	NdrFcShort( 0x1 ),	/* 1 */
/* 480 */	NdrFcShort( 0x0 ),	/* 0 */
/* 482 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hDrs */

/* 484 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 486 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 488 */	NdrFcShort( 0x5e ),	/* Type Offset=94 */

	/* Parameter dwInVersion */

/* 490 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 492 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 494 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgIn */

/* 496 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 498 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 500 */	NdrFcShort( 0x7d6 ),	/* Type Offset=2006 */

	/* Parameter pdwOutVersion */

/* 502 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 504 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 506 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 508 */	NdrFcShort( 0x8113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=32 */
/* 510 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 512 */	NdrFcShort( 0x84a ),	/* Type Offset=2122 */

	/* Return value */

/* 514 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 516 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 518 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSGetMemberships */

/* 520 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 522 */	NdrFcLong( 0x0 ),	/* 0 */
/* 526 */	NdrFcShort( 0x9 ),	/* 9 */
/* 528 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 530 */	0x30,		/* FC_BIND_CONTEXT */
			0x40,		/* Ctxt flags:  in, */
/* 532 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 534 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 536 */	NdrFcShort( 0x2c ),	/* 44 */
/* 538 */	NdrFcShort( 0x24 ),	/* 36 */
/* 540 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0x6,		/* 6 */
/* 542 */	0xa,		/* 10 */
			0x47,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, has range on conformance */
/* 544 */	NdrFcShort( 0x1 ),	/* 1 */
/* 546 */	NdrFcShort( 0x1 ),	/* 1 */
/* 548 */	NdrFcShort( 0x0 ),	/* 0 */
/* 550 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hDrs */

/* 552 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 554 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 556 */	NdrFcShort( 0x5e ),	/* Type Offset=94 */

	/* Parameter dwInVersion */

/* 558 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 560 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 562 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgIn */

/* 564 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 566 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 568 */	NdrFcShort( 0x8ba ),	/* Type Offset=2234 */

	/* Parameter pdwOutVersion */

/* 570 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 572 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 574 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 576 */	NdrFcShort( 0xa113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=40 */
/* 578 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 580 */	NdrFcShort( 0x93a ),	/* Type Offset=2362 */

	/* Return value */

/* 582 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 584 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 586 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSInterDomainMove */

/* 588 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 590 */	NdrFcLong( 0x0 ),	/* 0 */
/* 594 */	NdrFcShort( 0xa ),	/* 10 */
/* 596 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 598 */	0x30,		/* FC_BIND_CONTEXT */
			0x40,		/* Ctxt flags:  in, */
/* 600 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 602 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 604 */	NdrFcShort( 0x2c ),	/* 44 */
/* 606 */	NdrFcShort( 0x24 ),	/* 36 */
/* 608 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0x6,		/* 6 */
/* 610 */	0xa,		/* 10 */
			0x47,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, has range on conformance */
/* 612 */	NdrFcShort( 0x1 ),	/* 1 */
/* 614 */	NdrFcShort( 0x1 ),	/* 1 */
/* 616 */	NdrFcShort( 0x0 ),	/* 0 */
/* 618 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hDrs */

/* 620 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 622 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 624 */	NdrFcShort( 0x5e ),	/* Type Offset=94 */

	/* Parameter dwInVersion */

/* 626 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 628 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 630 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgIn */

/* 632 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 634 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 636 */	NdrFcShort( 0x9d4 ),	/* Type Offset=2516 */

	/* Parameter pdwOutVersion */

/* 638 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 640 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 642 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 644 */	NdrFcShort( 0x8113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=32 */
/* 646 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 648 */	NdrFcShort( 0xaaa ),	/* Type Offset=2730 */

	/* Return value */

/* 650 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 652 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 654 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSGetNT4ChangeLog */

/* 656 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 658 */	NdrFcLong( 0x0 ),	/* 0 */
/* 662 */	NdrFcShort( 0xb ),	/* 11 */
/* 664 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 666 */	0x30,		/* FC_BIND_CONTEXT */
			0x40,		/* Ctxt flags:  in, */
/* 668 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 670 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 672 */	NdrFcShort( 0x2c ),	/* 44 */
/* 674 */	NdrFcShort( 0x24 ),	/* 36 */
/* 676 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0x6,		/* 6 */
/* 678 */	0xa,		/* 10 */
			0x47,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, has range on conformance */
/* 680 */	NdrFcShort( 0x1 ),	/* 1 */
/* 682 */	NdrFcShort( 0x1 ),	/* 1 */
/* 684 */	NdrFcShort( 0x0 ),	/* 0 */
/* 686 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hDrs */

/* 688 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 690 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 692 */	NdrFcShort( 0x5e ),	/* Type Offset=94 */

	/* Parameter dwInVersion */

/* 694 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 696 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 698 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgIn */

/* 700 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 702 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 704 */	NdrFcShort( 0xb00 ),	/* Type Offset=2816 */

	/* Parameter pdwOutVersion */

/* 706 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 708 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 710 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 712 */	NdrFcShort( 0x113 ),	/* Flags:  must size, must free, out, simple ref, */
/* 714 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 716 */	NdrFcShort( 0xb5a ),	/* Type Offset=2906 */

	/* Return value */

/* 718 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 720 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 722 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSCrackNames */

/* 724 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 726 */	NdrFcLong( 0x0 ),	/* 0 */
/* 730 */	NdrFcShort( 0xc ),	/* 12 */
/* 732 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 734 */	0x30,		/* FC_BIND_CONTEXT */
			0x40,		/* Ctxt flags:  in, */
/* 736 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 738 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 740 */	NdrFcShort( 0x2c ),	/* 44 */
/* 742 */	NdrFcShort( 0x24 ),	/* 36 */
/* 744 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0x6,		/* 6 */
/* 746 */	0xa,		/* 10 */
			0x47,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, has range on conformance */
/* 748 */	NdrFcShort( 0x1 ),	/* 1 */
/* 750 */	NdrFcShort( 0x1 ),	/* 1 */
/* 752 */	NdrFcShort( 0x0 ),	/* 0 */
/* 754 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hDrs */

/* 756 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 758 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 760 */	NdrFcShort( 0x5e ),	/* Type Offset=94 */

	/* Parameter dwInVersion */

/* 762 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 764 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 766 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgIn */

/* 768 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 770 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 772 */	NdrFcShort( 0xbd2 ),	/* Type Offset=3026 */

	/* Parameter pdwOutVersion */

/* 774 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 776 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 778 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 780 */	NdrFcShort( 0x2113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=8 */
/* 782 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 784 */	NdrFcShort( 0xc42 ),	/* Type Offset=3138 */

	/* Return value */

/* 786 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 788 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 790 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSWriteSPN */

/* 792 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 794 */	NdrFcLong( 0x0 ),	/* 0 */
/* 798 */	NdrFcShort( 0xd ),	/* 13 */
/* 800 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 802 */	0x30,		/* FC_BIND_CONTEXT */
			0x40,		/* Ctxt flags:  in, */
/* 804 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 806 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 808 */	NdrFcShort( 0x2c ),	/* 44 */
/* 810 */	NdrFcShort( 0x24 ),	/* 36 */
/* 812 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0x6,		/* 6 */
/* 814 */	0xa,		/* 10 */
			0x47,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, has range on conformance */
/* 816 */	NdrFcShort( 0x1 ),	/* 1 */
/* 818 */	NdrFcShort( 0x1 ),	/* 1 */
/* 820 */	NdrFcShort( 0x0 ),	/* 0 */
/* 822 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hDrs */

/* 824 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 826 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 828 */	NdrFcShort( 0x5e ),	/* Type Offset=94 */

	/* Parameter dwInVersion */

/* 830 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 832 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 834 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgIn */

/* 836 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 838 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 840 */	NdrFcShort( 0xcc4 ),	/* Type Offset=3268 */

	/* Parameter pdwOutVersion */

/* 842 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 844 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 846 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 848 */	NdrFcShort( 0x2113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=8 */
/* 850 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 852 */	NdrFcShort( 0xd36 ),	/* Type Offset=3382 */

	/* Return value */

/* 854 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 856 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 858 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSRemoveDsServer */

/* 860 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 862 */	NdrFcLong( 0x0 ),	/* 0 */
/* 866 */	NdrFcShort( 0xe ),	/* 14 */
/* 868 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 870 */	0x30,		/* FC_BIND_CONTEXT */
			0x40,		/* Ctxt flags:  in, */
/* 872 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 874 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 876 */	NdrFcShort( 0x2c ),	/* 44 */
/* 878 */	NdrFcShort( 0x24 ),	/* 36 */
/* 880 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0x6,		/* 6 */
/* 882 */	0xa,		/* 10 */
			0x47,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, has range on conformance */
/* 884 */	NdrFcShort( 0x1 ),	/* 1 */
/* 886 */	NdrFcShort( 0x1 ),	/* 1 */
/* 888 */	NdrFcShort( 0x0 ),	/* 0 */
/* 890 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hDrs */

/* 892 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 894 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 896 */	NdrFcShort( 0x5e ),	/* Type Offset=94 */

	/* Parameter dwInVersion */

/* 898 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 900 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 902 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgIn */

/* 904 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 906 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 908 */	NdrFcShort( 0xd60 ),	/* Type Offset=3424 */

	/* Parameter pdwOutVersion */

/* 910 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 912 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 914 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 916 */	NdrFcShort( 0x2113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=8 */
/* 918 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 920 */	NdrFcShort( 0xd9a ),	/* Type Offset=3482 */

	/* Return value */

/* 922 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 924 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 926 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSRemoveDsDomain */

/* 928 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 930 */	NdrFcLong( 0x0 ),	/* 0 */
/* 934 */	NdrFcShort( 0xf ),	/* 15 */
/* 936 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 938 */	0x30,		/* FC_BIND_CONTEXT */
			0x40,		/* Ctxt flags:  in, */
/* 940 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 942 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 944 */	NdrFcShort( 0x2c ),	/* 44 */
/* 946 */	NdrFcShort( 0x24 ),	/* 36 */
/* 948 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0x6,		/* 6 */
/* 950 */	0xa,		/* 10 */
			0x47,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, has range on conformance */
/* 952 */	NdrFcShort( 0x1 ),	/* 1 */
/* 954 */	NdrFcShort( 0x1 ),	/* 1 */
/* 956 */	NdrFcShort( 0x0 ),	/* 0 */
/* 958 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hDrs */

/* 960 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 962 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 964 */	NdrFcShort( 0x5e ),	/* Type Offset=94 */

	/* Parameter dwInVersion */

/* 966 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 968 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 970 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgIn */

/* 972 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 974 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 976 */	NdrFcShort( 0xdbe ),	/* Type Offset=3518 */

	/* Parameter pdwOutVersion */

/* 978 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 980 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 982 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 984 */	NdrFcShort( 0x2113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=8 */
/* 986 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 988 */	NdrFcShort( 0xdf0 ),	/* Type Offset=3568 */

	/* Return value */

/* 990 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 992 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 994 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSDomainControllerInfo */

/* 996 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 998 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1002 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1004 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 1006 */	0x30,		/* FC_BIND_CONTEXT */
			0x40,		/* Ctxt flags:  in, */
/* 1008 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1010 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 1012 */	NdrFcShort( 0x2c ),	/* 44 */
/* 1014 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1016 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0x6,		/* 6 */
/* 1018 */	0xa,		/* 10 */
			0x47,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, has range on conformance */
/* 1020 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1022 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1024 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1026 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hDrs */

/* 1028 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 1030 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1032 */	NdrFcShort( 0x5e ),	/* Type Offset=94 */

	/* Parameter dwInVersion */

/* 1034 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1036 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 1038 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgIn */

/* 1040 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1042 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 1044 */	NdrFcShort( 0xe14 ),	/* Type Offset=3604 */

	/* Parameter pdwOutVersion */

/* 1046 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1048 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1050 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 1052 */	NdrFcShort( 0x4113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=16 */
/* 1054 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1056 */	NdrFcShort( 0xe48 ),	/* Type Offset=3656 */

	/* Return value */

/* 1058 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1060 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 1062 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSAddEntry */

/* 1064 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 1066 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1070 */	NdrFcShort( 0x11 ),	/* 17 */
/* 1072 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 1074 */	0x30,		/* FC_BIND_CONTEXT */
			0x40,		/* Ctxt flags:  in, */
/* 1076 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1078 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 1080 */	NdrFcShort( 0x2c ),	/* 44 */
/* 1082 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1084 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0x6,		/* 6 */
/* 1086 */	0xa,		/* 10 */
			0x47,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, has range on conformance */
/* 1088 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1090 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1092 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1094 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hDrs */

/* 1096 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 1098 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1100 */	NdrFcShort( 0x5e ),	/* Type Offset=94 */

	/* Parameter dwInVersion */

/* 1102 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1104 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 1106 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgIn */

/* 1108 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1110 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 1112 */	NdrFcShort( 0x1056 ),	/* Type Offset=4182 */

	/* Parameter pdwOutVersion */

/* 1114 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1116 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1118 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 1120 */	NdrFcShort( 0x113 ),	/* Flags:  must size, must free, out, simple ref, */
/* 1122 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1124 */	NdrFcShort( 0x10ca ),	/* Type Offset=4298 */

	/* Return value */

/* 1126 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1128 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 1130 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSExecuteKCC */

/* 1132 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 1134 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1138 */	NdrFcShort( 0x12 ),	/* 18 */
/* 1140 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1142 */	0x30,		/* FC_BIND_CONTEXT */
			0x40,		/* Ctxt flags:  in, */
/* 1144 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1146 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 1148 */	NdrFcShort( 0x2c ),	/* 44 */
/* 1150 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1152 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
			0x4,		/* 4 */
/* 1154 */	0xa,		/* 10 */
			0x45,		/* Ext Flags:  new corr desc, srv corr check, has range on conformance */
/* 1156 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1158 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1160 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1162 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hDrs */

/* 1164 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 1166 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1168 */	NdrFcShort( 0x5e ),	/* Type Offset=94 */

	/* Parameter dwInVersion */

/* 1170 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1172 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 1174 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgIn */

/* 1176 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1178 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 1180 */	NdrFcShort( 0x1320 ),	/* Type Offset=4896 */

	/* Return value */

/* 1182 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1184 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1186 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSGetReplInfo */

/* 1188 */	0x0,		/* 0 */
			0x49,		/* Old Flags:  full ptr, */
/* 1190 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1194 */	NdrFcShort( 0x13 ),	/* 19 */
/* 1196 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 1198 */	0x30,		/* FC_BIND_CONTEXT */
			0x40,		/* Ctxt flags:  in, */
/* 1200 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1202 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 1204 */	NdrFcShort( 0x2c ),	/* 44 */
/* 1206 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1208 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0x6,		/* 6 */
/* 1210 */	0xa,		/* 10 */
			0x47,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, has range on conformance */
/* 1212 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1214 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1216 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1218 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hDrs */

/* 1220 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 1222 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1224 */	NdrFcShort( 0x5e ),	/* Type Offset=94 */

	/* Parameter dwInVersion */

/* 1226 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1228 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 1230 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgIn */

/* 1232 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1234 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 1236 */	NdrFcShort( 0x134c ),	/* Type Offset=4940 */

	/* Parameter pdwOutVersion */

/* 1238 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1240 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1242 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 1244 */	NdrFcShort( 0x2113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=8 */
/* 1246 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1248 */	NdrFcShort( 0x13ac ),	/* Type Offset=5036 */

	/* Return value */

/* 1250 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1252 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 1254 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSAddSidHistory */

/* 1256 */	0x0,		/* 0 */
			0x49,		/* Old Flags:  full ptr, */
/* 1258 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1262 */	NdrFcShort( 0x14 ),	/* 20 */
/* 1264 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 1266 */	0x30,		/* FC_BIND_CONTEXT */
			0x40,		/* Ctxt flags:  in, */
/* 1268 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1270 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 1272 */	NdrFcShort( 0x2c ),	/* 44 */
/* 1274 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1276 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0x6,		/* 6 */
/* 1278 */	0xa,		/* 10 */
			0x47,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, has range on conformance */
/* 1280 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1282 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1284 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1286 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hDrs */

/* 1288 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 1290 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1292 */	NdrFcShort( 0x5e ),	/* Type Offset=94 */

	/* Parameter dwInVersion */

/* 1294 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1296 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 1298 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgIn */

/* 1300 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1302 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 1304 */	NdrFcShort( 0x182a ),	/* Type Offset=6186 */

	/* Parameter pdwOutVersion */

/* 1306 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1308 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1310 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 1312 */	NdrFcShort( 0x2113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=8 */
/* 1314 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1316 */	NdrFcShort( 0x18f0 ),	/* Type Offset=6384 */

	/* Return value */

/* 1318 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1320 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 1322 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSGetMemberships2 */

/* 1324 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 1326 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1330 */	NdrFcShort( 0x15 ),	/* 21 */
/* 1332 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 1334 */	0x30,		/* FC_BIND_CONTEXT */
			0x40,		/* Ctxt flags:  in, */
/* 1336 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1338 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 1340 */	NdrFcShort( 0x2c ),	/* 44 */
/* 1342 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1344 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0x6,		/* 6 */
/* 1346 */	0xa,		/* 10 */
			0x47,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, has range on conformance */
/* 1348 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1350 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1352 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1354 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hDrs */

/* 1356 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 1358 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1360 */	NdrFcShort( 0x5e ),	/* Type Offset=94 */

	/* Parameter dwInVersion */

/* 1362 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1364 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 1366 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgIn */

/* 1368 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1370 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 1372 */	NdrFcShort( 0x1914 ),	/* Type Offset=6420 */

	/* Parameter pdwOutVersion */

/* 1374 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1376 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1378 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 1380 */	NdrFcShort( 0x4113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=16 */
/* 1382 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1384 */	NdrFcShort( 0x1980 ),	/* Type Offset=6528 */

	/* Return value */

/* 1386 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1388 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 1390 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSReplicaVerifyObjects */

/* 1392 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 1394 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1398 */	NdrFcShort( 0x16 ),	/* 22 */
/* 1400 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1402 */	0x30,		/* FC_BIND_CONTEXT */
			0x40,		/* Ctxt flags:  in, */
/* 1404 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1406 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 1408 */	NdrFcShort( 0x2c ),	/* 44 */
/* 1410 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1412 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
			0x4,		/* 4 */
/* 1414 */	0xa,		/* 10 */
			0x45,		/* Ext Flags:  new corr desc, srv corr check, has range on conformance */
/* 1416 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1418 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1420 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1422 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hDrs */

/* 1424 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 1426 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1428 */	NdrFcShort( 0x5e ),	/* Type Offset=94 */

	/* Parameter dwVersion */

/* 1430 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1432 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 1434 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgVerify */

/* 1436 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1438 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 1440 */	NdrFcShort( 0x19ec ),	/* Type Offset=6636 */

	/* Return value */

/* 1442 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1444 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1446 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSGetObjectExistence */

/* 1448 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 1450 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1454 */	NdrFcShort( 0x17 ),	/* 23 */
/* 1456 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 1458 */	0x30,		/* FC_BIND_CONTEXT */
			0x40,		/* Ctxt flags:  in, */
/* 1460 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1462 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 1464 */	NdrFcShort( 0x2c ),	/* 44 */
/* 1466 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1468 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0x6,		/* 6 */
/* 1470 */	0xa,		/* 10 */
			0x47,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, has range on conformance */
/* 1472 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1474 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1476 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1478 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hDrs */

/* 1480 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 1482 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1484 */	NdrFcShort( 0x5e ),	/* Type Offset=94 */

	/* Parameter dwInVersion */

/* 1486 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1488 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 1490 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgIn */

/* 1492 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1494 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 1496 */	NdrFcShort( 0x1a24 ),	/* Type Offset=6692 */

	/* Parameter pdwOutVersion */

/* 1498 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1500 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1502 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 1504 */	NdrFcShort( 0x4113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=16 */
/* 1506 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1508 */	NdrFcShort( 0x1a6c ),	/* Type Offset=6764 */

	/* Return value */

/* 1510 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1512 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 1514 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSQuerySitesByCost */

/* 1516 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 1518 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1522 */	NdrFcShort( 0x18 ),	/* 24 */
/* 1524 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 1526 */	0x30,		/* FC_BIND_CONTEXT */
			0x40,		/* Ctxt flags:  in, */
/* 1528 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1530 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 1532 */	NdrFcShort( 0x2c ),	/* 44 */
/* 1534 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1536 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0x6,		/* 6 */
/* 1538 */	0xa,		/* 10 */
			0x47,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, has range on conformance */
/* 1540 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1542 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1544 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1546 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hDrs */

/* 1548 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 1550 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1552 */	NdrFcShort( 0x5e ),	/* Type Offset=94 */

	/* Parameter dwInVersion */

/* 1554 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1556 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 1558 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgIn */

/* 1560 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1562 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 1564 */	NdrFcShort( 0x1ad8 ),	/* Type Offset=6872 */

	/* Parameter pdwOutVersion */

/* 1566 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1568 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1570 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 1572 */	NdrFcShort( 0x6113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=24 */
/* 1574 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1576 */	NdrFcShort( 0x1b4a ),	/* Type Offset=6986 */

	/* Return value */

/* 1578 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1580 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 1582 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSInitDemotion */

/* 1584 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 1586 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1590 */	NdrFcShort( 0x19 ),	/* 25 */
/* 1592 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 1594 */	0x30,		/* FC_BIND_CONTEXT */
			0x40,		/* Ctxt flags:  in, */
/* 1596 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1598 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 1600 */	NdrFcShort( 0x2c ),	/* 44 */
/* 1602 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1604 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0x6,		/* 6 */
/* 1606 */	0xa,		/* 10 */
			0x47,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, has range on conformance */
/* 1608 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1610 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1612 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1614 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hDrs */

/* 1616 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 1618 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1620 */	NdrFcShort( 0x5e ),	/* Type Offset=94 */

	/* Parameter dwInVersion */

/* 1622 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1624 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 1626 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgIn */

/* 1628 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1630 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 1632 */	NdrFcShort( 0x1bb8 ),	/* Type Offset=7096 */

	/* Parameter pdwOutVersion */

/* 1634 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1636 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1638 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 1640 */	NdrFcShort( 0x2113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=8 */
/* 1642 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1644 */	NdrFcShort( 0x1bdc ),	/* Type Offset=7132 */

	/* Return value */

/* 1646 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1648 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 1650 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSReplicaDemotion */

/* 1652 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 1654 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1658 */	NdrFcShort( 0x1a ),	/* 26 */
/* 1660 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 1662 */	0x30,		/* FC_BIND_CONTEXT */
			0x40,		/* Ctxt flags:  in, */
/* 1664 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1666 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 1668 */	NdrFcShort( 0x2c ),	/* 44 */
/* 1670 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1672 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0x6,		/* 6 */
/* 1674 */	0xa,		/* 10 */
			0x47,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, has range on conformance */
/* 1676 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1678 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1680 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1682 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hDrs */

/* 1684 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 1686 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1688 */	NdrFcShort( 0x5e ),	/* Type Offset=94 */

	/* Parameter dwInVersion */

/* 1690 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1692 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 1694 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgIn */

/* 1696 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1698 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 1700 */	NdrFcShort( 0x1c00 ),	/* Type Offset=7168 */

	/* Parameter pdwOutVersion */

/* 1702 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1704 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1706 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 1708 */	NdrFcShort( 0x2113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=8 */
/* 1710 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1712 */	NdrFcShort( 0x1c38 ),	/* Type Offset=7224 */

	/* Return value */

/* 1714 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1716 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 1718 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSFinishDemotion */

/* 1720 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 1722 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1726 */	NdrFcShort( 0x1b ),	/* 27 */
/* 1728 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 1730 */	0x30,		/* FC_BIND_CONTEXT */
			0x40,		/* Ctxt flags:  in, */
/* 1732 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1734 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 1736 */	NdrFcShort( 0x2c ),	/* 44 */
/* 1738 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1740 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0x6,		/* 6 */
/* 1742 */	0xa,		/* 10 */
			0x47,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, has range on conformance */
/* 1744 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1746 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1748 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1750 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hDrs */

/* 1752 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 1754 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1756 */	NdrFcShort( 0x5e ),	/* Type Offset=94 */

	/* Parameter dwInVersion */

/* 1758 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1760 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 1762 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgIn */

/* 1764 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1766 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 1768 */	NdrFcShort( 0x1c5c ),	/* Type Offset=7260 */

	/* Parameter pdwOutVersion */

/* 1770 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1772 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1774 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 1776 */	NdrFcShort( 0x4113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=16 */
/* 1778 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1780 */	NdrFcShort( 0x1c94 ),	/* Type Offset=7316 */

	/* Return value */

/* 1782 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1784 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 1786 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSAddCloneDC */

/* 1788 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 1790 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1794 */	NdrFcShort( 0x1c ),	/* 28 */
/* 1796 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 1798 */	0x30,		/* FC_BIND_CONTEXT */
			0x40,		/* Ctxt flags:  in, */
/* 1800 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1802 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 1804 */	NdrFcShort( 0x2c ),	/* 44 */
/* 1806 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1808 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0x6,		/* 6 */
/* 1810 */	0xa,		/* 10 */
			0x47,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, has range on conformance */
/* 1812 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1814 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1816 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1818 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hDrs */

/* 1820 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 1822 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1824 */	NdrFcShort( 0x5e ),	/* Type Offset=94 */

	/* Parameter dwInVersion */

/* 1826 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1828 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 1830 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgIn */

/* 1832 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1834 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 1836 */	NdrFcShort( 0x1cc0 ),	/* Type Offset=7360 */

	/* Parameter pdwOutVersion */

/* 1838 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1840 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1842 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 1844 */	NdrFcShort( 0x8113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=32 */
/* 1846 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1848 */	NdrFcShort( 0x1cf8 ),	/* Type Offset=7416 */

	/* Return value */

/* 1850 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1852 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 1854 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSWriteNgcKey */

/* 1856 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 1858 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1862 */	NdrFcShort( 0x1d ),	/* 29 */
/* 1864 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 1866 */	0x30,		/* FC_BIND_CONTEXT */
			0x40,		/* Ctxt flags:  in, */
/* 1868 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1870 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 1872 */	NdrFcShort( 0x2c ),	/* 44 */
/* 1874 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1876 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0x6,		/* 6 */
/* 1878 */	0xa,		/* 10 */
			0x47,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, has range on conformance */
/* 1880 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1882 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1884 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1886 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hDrs */

/* 1888 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 1890 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1892 */	NdrFcShort( 0x5e ),	/* Type Offset=94 */

	/* Parameter dwInVersion */

/* 1894 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1896 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 1898 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgIn */

/* 1900 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1902 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 1904 */	NdrFcShort( 0x1d5a ),	/* Type Offset=7514 */

	/* Parameter pdwOutVersion */

/* 1906 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1908 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1910 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 1912 */	NdrFcShort( 0x2113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=8 */
/* 1914 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1916 */	NdrFcShort( 0x1da0 ),	/* Type Offset=7584 */

	/* Return value */

/* 1918 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1920 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 1922 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DRSReadNgcKey */

/* 1924 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 1926 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1930 */	NdrFcShort( 0x1e ),	/* 30 */
/* 1932 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 1934 */	0x30,		/* FC_BIND_CONTEXT */
			0x40,		/* Ctxt flags:  in, */
/* 1936 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1938 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 1940 */	NdrFcShort( 0x2c ),	/* 44 */
/* 1942 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1944 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0x6,		/* 6 */
/* 1946 */	0xa,		/* 10 */
			0x47,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, has range on conformance */
/* 1948 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1950 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1952 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1954 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hDrs */

/* 1956 */	NdrFcShort( 0x8 ),	/* Flags:  in, */
/* 1958 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 1960 */	NdrFcShort( 0x5e ),	/* Type Offset=94 */

	/* Parameter dwInVersion */

/* 1962 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1964 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 1966 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgIn */

/* 1968 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1970 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 1972 */	NdrFcShort( 0x1dc4 ),	/* Type Offset=7620 */

	/* Parameter pdwOutVersion */

/* 1974 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1976 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1978 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 1980 */	NdrFcShort( 0x4113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=16 */
/* 1982 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1984 */	NdrFcShort( 0x1df6 ),	/* Type Offset=7670 */

	/* Return value */

/* 1986 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1988 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 1990 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DSAPrepareScript */

/* 1992 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 1994 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1998 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2000 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 2002 */	0x32,		/* FC_BIND_PRIMITIVE */
			0x0,		/* 0 */
/* 2004 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 2006 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2008 */	NdrFcShort( 0x24 ),	/* 36 */
/* 2010 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0x5,		/* 5 */
/* 2012 */	0xa,		/* 10 */
			0x47,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, has range on conformance */
/* 2014 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2016 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2018 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2020 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hRpc */

/* 2022 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 2024 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 2026 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter dwInVersion */

/* 2028 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2030 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 2032 */	NdrFcShort( 0x1e38 ),	/* Type Offset=7736 */

	/* Parameter pmsgIn */

/* 2034 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 2036 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 2038 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pdwOutVersion */

/* 2040 */	NdrFcShort( 0x113 ),	/* Flags:  must size, must free, out, simple ref, */
/* 2042 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 2044 */	NdrFcShort( 0x1e5c ),	/* Type Offset=7772 */

	/* Parameter pmsgOut */

/* 2046 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2048 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 2050 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure IDL_DSAExecuteScript */


	/* Return value */

/* 2052 */	0x0,		/* 0 */
			0x48,		/* Old Flags:  */
/* 2054 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2058 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2060 */	NdrFcShort( 0x30 ),	/* X64 Stack size/offset = 48 */
/* 2062 */	0x32,		/* FC_BIND_PRIMITIVE */
			0x0,		/* 0 */
/* 2064 */	NdrFcShort( 0x0 ),	/* X64 Stack size/offset = 0 */
/* 2066 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2068 */	NdrFcShort( 0x24 ),	/* 36 */
/* 2070 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
			0x5,		/* 5 */
/* 2072 */	0xa,		/* 10 */
			0x47,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, has range on conformance */
/* 2074 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2076 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2078 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2080 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter hRpc */

/* 2082 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 2084 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 2086 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter dwInVersion */

/* 2088 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2090 */	NdrFcShort( 0x10 ),	/* X64 Stack size/offset = 16 */
/* 2092 */	NdrFcShort( 0x1ef8 ),	/* Type Offset=7928 */

	/* Parameter pmsgIn */

/* 2094 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 2096 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 2098 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pdwOutVersion */

/* 2100 */	NdrFcShort( 0x4113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=16 */
/* 2102 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 2104 */	NdrFcShort( 0x1f3a ),	/* Type Offset=7994 */

	/* Parameter pmsgOut */

/* 2106 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2108 */	NdrFcShort( 0x28 ),	/* X64 Stack size/offset = 40 */
/* 2110 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

			0x0
        }
    };

static const ms2Ddrsr_MIDL_TYPE_FORMAT_STRING ms2Ddrsr__MIDL_TypeFormatString =
    {
        0,
        {
			NdrFcShort( 0x0 ),	/* 0 */
/*  2 */	
			0x12, 0x0,	/* FC_UP */
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
			0x12, 0x0,	/* FC_UP */
/* 26 */	NdrFcShort( 0x22 ),	/* Offset= 34 (60) */
/* 28 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 30 */	NdrFcLong( 0x1 ),	/* 1 */
/* 34 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 38 */	
			0x1b,		/* FC_CARRAY */
			0x0,		/* 0 */
/* 40 */	NdrFcShort( 0x1 ),	/* 1 */
/* 42 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 44 */	NdrFcShort( 0xfffc ),	/* -4 */
/* 46 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 48 */	0x0 , 
			0x0,		/* 0 */
/* 50 */	NdrFcLong( 0x0 ),	/* 0 */
/* 54 */	NdrFcLong( 0x0 ),	/* 0 */
/* 58 */	0x2,		/* FC_CHAR */
			0x5b,		/* FC_END */
/* 60 */	0xb1,		/* FC_FORCED_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 62 */	NdrFcShort( 0x4 ),	/* 4 */
/* 64 */	NdrFcShort( 0xffe6 ),	/* Offset= -26 (38) */
/* 66 */	NdrFcShort( 0x0 ),	/* Offset= 0 (66) */
/* 68 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 70 */	NdrFcShort( 0xffd6 ),	/* Offset= -42 (28) */
/* 72 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 74 */	
			0x11, 0x14,	/* FC_RP [alloced_on_stack] [pointer_deref] */
/* 76 */	NdrFcShort( 0xffcc ),	/* Offset= -52 (24) */
/* 78 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 80 */	NdrFcShort( 0x2 ),	/* Offset= 2 (82) */
/* 82 */	0x30,		/* FC_BIND_CONTEXT */
			0xa0,		/* Ctxt flags:  via ptr, out, */
/* 84 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 86 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 88 */	NdrFcShort( 0x2 ),	/* Offset= 2 (90) */
/* 90 */	0x30,		/* FC_BIND_CONTEXT */
			0xe1,		/* Ctxt flags:  via ptr, in, out, can't be null */
/* 92 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 94 */	0x30,		/* FC_BIND_CONTEXT */
			0x41,		/* Ctxt flags:  in, can't be null */
/* 96 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 98 */	
			0x11, 0x0,	/* FC_RP */
/* 100 */	NdrFcShort( 0x2 ),	/* Offset= 2 (102) */
/* 102 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 104 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 106 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 108 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 110 */	0x0 , 
			0x0,		/* 0 */
/* 112 */	NdrFcLong( 0x0 ),	/* 0 */
/* 116 */	NdrFcLong( 0x0 ),	/* 0 */
/* 120 */	NdrFcShort( 0x2 ),	/* Offset= 2 (122) */
/* 122 */	NdrFcShort( 0x28 ),	/* 40 */
/* 124 */	NdrFcShort( 0x1 ),	/* 1 */
/* 126 */	NdrFcLong( 0x1 ),	/* 1 */
/* 130 */	NdrFcShort( 0x3c ),	/* Offset= 60 (190) */
/* 132 */	NdrFcShort( 0xffff ),	/* Offset= -1 (131) */
/* 134 */	
			0x1d,		/* FC_SMFARRAY */
			0x0,		/* 0 */
/* 136 */	NdrFcShort( 0x1c ),	/* 28 */
/* 138 */	0x2,		/* FC_CHAR */
			0x5b,		/* FC_END */
/* 140 */	
			0x15,		/* FC_STRUCT */
			0x0,		/* 0 */
/* 142 */	NdrFcShort( 0x1c ),	/* 28 */
/* 144 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 146 */	NdrFcShort( 0xfff4 ),	/* Offset= -12 (134) */
/* 148 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 150 */	
			0x1b,		/* FC_CARRAY */
			0x1,		/* 1 */
/* 152 */	NdrFcShort( 0x2 ),	/* 2 */
/* 154 */	0x9,		/* Corr desc: FC_ULONG */
			0x57,		/* FC_ADD_1 */
/* 156 */	NdrFcShort( 0xfffc ),	/* -4 */
/* 158 */	NdrFcShort( 0x11 ),	/* Corr flags:  early, */
/* 160 */	0x1 , /* correlation range */
			0x0,		/* 0 */
/* 162 */	NdrFcLong( 0x0 ),	/* 0 */
/* 166 */	NdrFcLong( 0xa00001 ),	/* 10485761 */
/* 170 */	0x5,		/* FC_WCHAR */
			0x5b,		/* FC_END */
/* 172 */	
			0x17,		/* FC_CSTRUCT */
			0x3,		/* 3 */
/* 174 */	NdrFcShort( 0x38 ),	/* 56 */
/* 176 */	NdrFcShort( 0xffe6 ),	/* Offset= -26 (150) */
/* 178 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 180 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 182 */	NdrFcShort( 0xff56 ),	/* Offset= -170 (12) */
/* 184 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 186 */	NdrFcShort( 0xffd2 ),	/* Offset= -46 (140) */
/* 188 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 190 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 192 */	NdrFcShort( 0x28 ),	/* 40 */
/* 194 */	NdrFcShort( 0x0 ),	/* 0 */
/* 196 */	NdrFcShort( 0xc ),	/* Offset= 12 (208) */
/* 198 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 200 */	0x0,		/* 0 */
			NdrFcShort( 0xff43 ),	/* Offset= -189 (12) */
			0x36,		/* FC_POINTER */
/* 204 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 206 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 208 */	
			0x11, 0x0,	/* FC_RP */
/* 210 */	NdrFcShort( 0xffda ),	/* Offset= -38 (172) */
/* 212 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 214 */	
			0x22,		/* FC_C_CSTRING */
			0x5c,		/* FC_PAD */
/* 216 */	
			0x11, 0x0,	/* FC_RP */
/* 218 */	NdrFcShort( 0x2 ),	/* Offset= 2 (220) */
/* 220 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 222 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 224 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 226 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 228 */	0x0 , 
			0x0,		/* 0 */
/* 230 */	NdrFcLong( 0x0 ),	/* 0 */
/* 234 */	NdrFcLong( 0x0 ),	/* 0 */
/* 238 */	NdrFcShort( 0x2 ),	/* Offset= 2 (240) */
/* 240 */	NdrFcShort( 0xa8 ),	/* 168 */
/* 242 */	NdrFcShort( 0x5 ),	/* 5 */
/* 244 */	NdrFcLong( 0x4 ),	/* 4 */
/* 248 */	NdrFcShort( 0x162 ),	/* Offset= 354 (602) */
/* 250 */	NdrFcLong( 0x5 ),	/* 5 */
/* 254 */	NdrFcShort( 0x178 ),	/* Offset= 376 (630) */
/* 256 */	NdrFcLong( 0x7 ),	/* 7 */
/* 260 */	NdrFcShort( 0x19a ),	/* Offset= 410 (670) */
/* 262 */	NdrFcLong( 0x8 ),	/* 8 */
/* 266 */	NdrFcShort( 0x1b8 ),	/* Offset= 440 (706) */
/* 268 */	NdrFcLong( 0xa ),	/* 10 */
/* 272 */	NdrFcShort( 0x1e8 ),	/* Offset= 488 (760) */
/* 274 */	NdrFcShort( 0xffff ),	/* Offset= -1 (273) */
/* 276 */	
			0x15,		/* FC_STRUCT */
			0x7,		/* 7 */
/* 278 */	NdrFcShort( 0x18 ),	/* 24 */
/* 280 */	0xb,		/* FC_HYPER */
			0xb,		/* FC_HYPER */
/* 282 */	0xb,		/* FC_HYPER */
			0x5b,		/* FC_END */
/* 284 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 286 */	NdrFcLong( 0x0 ),	/* 0 */
/* 290 */	NdrFcLong( 0x100000 ),	/* 1048576 */
/* 294 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 296 */	NdrFcLong( 0x0 ),	/* 0 */
/* 300 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 304 */	
			0x1b,		/* FC_CARRAY */
			0x0,		/* 0 */
/* 306 */	NdrFcShort( 0x1 ),	/* 1 */
/* 308 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 310 */	NdrFcShort( 0x0 ),	/* 0 */
/* 312 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 314 */	0x0 , 
			0x0,		/* 0 */
/* 316 */	NdrFcLong( 0x0 ),	/* 0 */
/* 320 */	NdrFcLong( 0x0 ),	/* 0 */
/* 324 */	0x2,		/* FC_CHAR */
			0x5b,		/* FC_END */
/* 326 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 328 */	NdrFcShort( 0x10 ),	/* 16 */
/* 330 */	NdrFcShort( 0x0 ),	/* 0 */
/* 332 */	NdrFcShort( 0xa ),	/* Offset= 10 (342) */
/* 334 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 336 */	NdrFcShort( 0xffd6 ),	/* Offset= -42 (294) */
/* 338 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 340 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 342 */	
			0x12, 0x0,	/* FC_UP */
/* 344 */	NdrFcShort( 0xffd8 ),	/* Offset= -40 (304) */
/* 346 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 348 */	NdrFcShort( 0x18 ),	/* 24 */
/* 350 */	NdrFcShort( 0x0 ),	/* 0 */
/* 352 */	NdrFcShort( 0x0 ),	/* Offset= 0 (352) */
/* 354 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 356 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 358 */	NdrFcShort( 0xffe0 ),	/* Offset= -32 (326) */
/* 360 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 362 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 364 */	NdrFcShort( 0x0 ),	/* 0 */
/* 366 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 368 */	NdrFcShort( 0x0 ),	/* 0 */
/* 370 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 372 */	0x0 , 
			0x0,		/* 0 */
/* 374 */	NdrFcLong( 0x0 ),	/* 0 */
/* 378 */	NdrFcLong( 0x0 ),	/* 0 */
/* 382 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 386 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 388 */	0x0 , 
			0x0,		/* 0 */
/* 390 */	NdrFcLong( 0x0 ),	/* 0 */
/* 394 */	NdrFcLong( 0x0 ),	/* 0 */
/* 398 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 400 */	NdrFcShort( 0xffca ),	/* Offset= -54 (346) */
/* 402 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 404 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 406 */	NdrFcShort( 0x10 ),	/* 16 */
/* 408 */	NdrFcShort( 0x0 ),	/* 0 */
/* 410 */	NdrFcShort( 0xa ),	/* Offset= 10 (420) */
/* 412 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 414 */	NdrFcShort( 0xff7e ),	/* Offset= -130 (284) */
/* 416 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 418 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 420 */	
			0x12, 0x0,	/* FC_UP */
/* 422 */	NdrFcShort( 0xffc4 ),	/* Offset= -60 (362) */
/* 424 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 426 */	NdrFcLong( 0x0 ),	/* 0 */
/* 430 */	NdrFcLong( 0x100000 ),	/* 1048576 */
/* 434 */	
			0x15,		/* FC_STRUCT */
			0x7,		/* 7 */
/* 436 */	NdrFcShort( 0x18 ),	/* 24 */
/* 438 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 440 */	NdrFcShort( 0xfe54 ),	/* Offset= -428 (12) */
/* 442 */	0xb,		/* FC_HYPER */
			0x5b,		/* FC_END */
/* 444 */	
			0x1b,		/* FC_CARRAY */
			0x7,		/* 7 */
/* 446 */	NdrFcShort( 0x18 ),	/* 24 */
/* 448 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 450 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 452 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 454 */	0x0 , 
			0x0,		/* 0 */
/* 456 */	NdrFcLong( 0x0 ),	/* 0 */
/* 460 */	NdrFcLong( 0x0 ),	/* 0 */
/* 464 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 466 */	NdrFcShort( 0xffe0 ),	/* Offset= -32 (434) */
/* 468 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 470 */	0xb1,		/* FC_FORCED_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 472 */	NdrFcShort( 0x10 ),	/* 16 */
/* 474 */	NdrFcShort( 0xffe2 ),	/* Offset= -30 (444) */
/* 476 */	NdrFcShort( 0x0 ),	/* Offset= 0 (476) */
/* 478 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 480 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 482 */	NdrFcShort( 0xffc6 ),	/* Offset= -58 (424) */
/* 484 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 486 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 488 */	NdrFcLong( 0x1 ),	/* 1 */
/* 492 */	NdrFcLong( 0x100000 ),	/* 1048576 */
/* 496 */	
			0x1b,		/* FC_CARRAY */
			0x3,		/* 3 */
/* 498 */	NdrFcShort( 0x4 ),	/* 4 */
/* 500 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 502 */	NdrFcShort( 0xfffc ),	/* -4 */
/* 504 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 506 */	0x0 , 
			0x0,		/* 0 */
/* 508 */	NdrFcLong( 0x0 ),	/* 0 */
/* 512 */	NdrFcLong( 0x0 ),	/* 0 */
/* 516 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 518 */	0xb1,		/* FC_FORCED_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 520 */	NdrFcShort( 0xc ),	/* 12 */
/* 522 */	NdrFcShort( 0xffe6 ),	/* Offset= -26 (496) */
/* 524 */	NdrFcShort( 0x0 ),	/* Offset= 0 (524) */
/* 526 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 528 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 530 */	NdrFcShort( 0xffd4 ),	/* Offset= -44 (486) */
/* 532 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 534 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 536 */	NdrFcShort( 0x70 ),	/* 112 */
/* 538 */	NdrFcShort( 0x0 ),	/* 0 */
/* 540 */	NdrFcShort( 0x1a ),	/* Offset= 26 (566) */
/* 542 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 544 */	NdrFcShort( 0xfdec ),	/* Offset= -532 (12) */
/* 546 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 548 */	NdrFcShort( 0xfde8 ),	/* Offset= -536 (12) */
/* 550 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 552 */	0x0,		/* 0 */
			NdrFcShort( 0xfeeb ),	/* Offset= -277 (276) */
			0x36,		/* FC_POINTER */
/* 556 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 558 */	0x0,		/* 0 */
			NdrFcShort( 0xff65 ),	/* Offset= -155 (404) */
			0x8,		/* FC_LONG */
/* 562 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 564 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 566 */	
			0x11, 0x0,	/* FC_RP */
/* 568 */	NdrFcShort( 0xfe74 ),	/* Offset= -396 (172) */
/* 570 */	
			0x12, 0x0,	/* FC_UP */
/* 572 */	NdrFcShort( 0xff9a ),	/* Offset= -102 (470) */
/* 574 */	
			0x12, 0x0,	/* FC_UP */
/* 576 */	NdrFcShort( 0xffc6 ),	/* Offset= -58 (518) */
/* 578 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 580 */	NdrFcLong( 0x1 ),	/* 1 */
/* 584 */	NdrFcLong( 0x100 ),	/* 256 */
/* 588 */	0xb1,		/* FC_FORCED_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 590 */	NdrFcShort( 0x4 ),	/* 4 */
/* 592 */	NdrFcShort( 0xfdd6 ),	/* Offset= -554 (38) */
/* 594 */	NdrFcShort( 0x0 ),	/* Offset= 0 (594) */
/* 596 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 598 */	NdrFcShort( 0xffec ),	/* Offset= -20 (578) */
/* 600 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 602 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 604 */	NdrFcShort( 0x88 ),	/* 136 */
/* 606 */	NdrFcShort( 0x0 ),	/* 0 */
/* 608 */	NdrFcShort( 0xc ),	/* Offset= 12 (620) */
/* 610 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 612 */	NdrFcShort( 0xfda8 ),	/* Offset= -600 (12) */
/* 614 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 616 */	0x0,		/* 0 */
			NdrFcShort( 0xffad ),	/* Offset= -83 (534) */
			0x5b,		/* FC_END */
/* 620 */	
			0x11, 0x0,	/* FC_RP */
/* 622 */	NdrFcShort( 0xffde ),	/* Offset= -34 (588) */
/* 624 */	
			0x15,		/* FC_STRUCT */
			0x7,		/* 7 */
/* 626 */	NdrFcShort( 0x8 ),	/* 8 */
/* 628 */	0xb,		/* FC_HYPER */
			0x5b,		/* FC_END */
/* 630 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 632 */	NdrFcShort( 0x60 ),	/* 96 */
/* 634 */	NdrFcShort( 0x0 ),	/* 0 */
/* 636 */	NdrFcShort( 0x1a ),	/* Offset= 26 (662) */
/* 638 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 640 */	NdrFcShort( 0xfd8c ),	/* Offset= -628 (12) */
/* 642 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 644 */	NdrFcShort( 0xfd88 ),	/* Offset= -632 (12) */
/* 646 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 648 */	0x0,		/* 0 */
			NdrFcShort( 0xfe8b ),	/* Offset= -373 (276) */
			0x36,		/* FC_POINTER */
/* 652 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 654 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 656 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 658 */	NdrFcShort( 0xffde ),	/* Offset= -34 (624) */
/* 660 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 662 */	
			0x11, 0x0,	/* FC_RP */
/* 664 */	NdrFcShort( 0xfe14 ),	/* Offset= -492 (172) */
/* 666 */	
			0x12, 0x0,	/* FC_UP */
/* 668 */	NdrFcShort( 0xff3a ),	/* Offset= -198 (470) */
/* 670 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 672 */	NdrFcShort( 0xa8 ),	/* 168 */
/* 674 */	NdrFcShort( 0x0 ),	/* 0 */
/* 676 */	NdrFcShort( 0x12 ),	/* Offset= 18 (694) */
/* 678 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 680 */	NdrFcShort( 0xfd64 ),	/* Offset= -668 (12) */
/* 682 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 684 */	0x0,		/* 0 */
			NdrFcShort( 0xff69 ),	/* Offset= -151 (534) */
			0x36,		/* FC_POINTER */
/* 688 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 690 */	0x0,		/* 0 */
			NdrFcShort( 0xfee1 ),	/* Offset= -287 (404) */
			0x5b,		/* FC_END */
/* 694 */	
			0x11, 0x0,	/* FC_RP */
/* 696 */	NdrFcShort( 0xff94 ),	/* Offset= -108 (588) */
/* 698 */	
			0x12, 0x0,	/* FC_UP */
/* 700 */	NdrFcShort( 0xff4a ),	/* Offset= -182 (518) */
/* 702 */	
			0x12, 0x0,	/* FC_UP */
/* 704 */	NdrFcShort( 0xff46 ),	/* Offset= -186 (518) */
/* 706 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 708 */	NdrFcShort( 0x80 ),	/* 128 */
/* 710 */	NdrFcShort( 0x0 ),	/* 0 */
/* 712 */	NdrFcShort( 0x20 ),	/* Offset= 32 (744) */
/* 714 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 716 */	NdrFcShort( 0xfd40 ),	/* Offset= -704 (12) */
/* 718 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 720 */	NdrFcShort( 0xfd3c ),	/* Offset= -708 (12) */
/* 722 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 724 */	0x0,		/* 0 */
			NdrFcShort( 0xfe3f ),	/* Offset= -449 (276) */
			0x36,		/* FC_POINTER */
/* 728 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 730 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 732 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 734 */	NdrFcShort( 0xff92 ),	/* Offset= -110 (624) */
/* 736 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 738 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 740 */	NdrFcShort( 0xfeb0 ),	/* Offset= -336 (404) */
/* 742 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 744 */	
			0x11, 0x0,	/* FC_RP */
/* 746 */	NdrFcShort( 0xfdc2 ),	/* Offset= -574 (172) */
/* 748 */	
			0x12, 0x0,	/* FC_UP */
/* 750 */	NdrFcShort( 0xfee8 ),	/* Offset= -280 (470) */
/* 752 */	
			0x12, 0x0,	/* FC_UP */
/* 754 */	NdrFcShort( 0xff14 ),	/* Offset= -236 (518) */
/* 756 */	
			0x12, 0x0,	/* FC_UP */
/* 758 */	NdrFcShort( 0xff10 ),	/* Offset= -240 (518) */
/* 760 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 762 */	NdrFcShort( 0x88 ),	/* 136 */
/* 764 */	NdrFcShort( 0x0 ),	/* 0 */
/* 766 */	NdrFcShort( 0x22 ),	/* Offset= 34 (800) */
/* 768 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 770 */	NdrFcShort( 0xfd0a ),	/* Offset= -758 (12) */
/* 772 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 774 */	NdrFcShort( 0xfd06 ),	/* Offset= -762 (12) */
/* 776 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 778 */	0x0,		/* 0 */
			NdrFcShort( 0xfe09 ),	/* Offset= -503 (276) */
			0x36,		/* FC_POINTER */
/* 782 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 784 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 786 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 788 */	NdrFcShort( 0xff5c ),	/* Offset= -164 (624) */
/* 790 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 792 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 794 */	NdrFcShort( 0xfe7a ),	/* Offset= -390 (404) */
/* 796 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 798 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 800 */	
			0x11, 0x0,	/* FC_RP */
/* 802 */	NdrFcShort( 0xfd8a ),	/* Offset= -630 (172) */
/* 804 */	
			0x12, 0x0,	/* FC_UP */
/* 806 */	NdrFcShort( 0xfeb0 ),	/* Offset= -336 (470) */
/* 808 */	
			0x12, 0x0,	/* FC_UP */
/* 810 */	NdrFcShort( 0xfedc ),	/* Offset= -292 (518) */
/* 812 */	
			0x12, 0x0,	/* FC_UP */
/* 814 */	NdrFcShort( 0xfed8 ),	/* Offset= -296 (518) */
/* 816 */	
			0x11, 0xc,	/* FC_RP [alloced_on_stack] [simple_pointer] */
/* 818 */	0x8,		/* FC_LONG */
			0x5c,		/* FC_PAD */
/* 820 */	
			0x11, 0x0,	/* FC_RP */
/* 822 */	NdrFcShort( 0x2 ),	/* Offset= 2 (824) */
/* 824 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 826 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 828 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 830 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 832 */	0x0 , 
			0x0,		/* 0 */
/* 834 */	NdrFcLong( 0x0 ),	/* 0 */
/* 838 */	NdrFcLong( 0x0 ),	/* 0 */
/* 842 */	NdrFcShort( 0x2 ),	/* Offset= 2 (844) */
/* 844 */	NdrFcShort( 0xa8 ),	/* 168 */
/* 846 */	NdrFcShort( 0x5 ),	/* 5 */
/* 848 */	NdrFcLong( 0x1 ),	/* 1 */
/* 852 */	NdrFcShort( 0x160 ),	/* Offset= 352 (1204) */
/* 854 */	NdrFcLong( 0x2 ),	/* 2 */
/* 858 */	NdrFcShort( 0x1b2 ),	/* Offset= 434 (1292) */
/* 860 */	NdrFcLong( 0x6 ),	/* 6 */
/* 864 */	NdrFcShort( 0x256 ),	/* Offset= 598 (1462) */
/* 866 */	NdrFcLong( 0x7 ),	/* 7 */
/* 870 */	NdrFcShort( 0x28e ),	/* Offset= 654 (1524) */
/* 872 */	NdrFcLong( 0x9 ),	/* 9 */
/* 876 */	NdrFcShort( 0x2fa ),	/* Offset= 762 (1638) */
/* 878 */	NdrFcShort( 0xffff ),	/* Offset= -1 (877) */
/* 880 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 882 */	NdrFcLong( 0x0 ),	/* 0 */
/* 886 */	NdrFcLong( 0x100000 ),	/* 1048576 */
/* 890 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 892 */	NdrFcLong( 0x0 ),	/* 0 */
/* 896 */	NdrFcLong( 0xa00000 ),	/* 10485760 */
/* 900 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 902 */	NdrFcLong( 0x0 ),	/* 0 */
/* 906 */	NdrFcLong( 0x1900000 ),	/* 26214400 */
/* 910 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 912 */	NdrFcShort( 0x10 ),	/* 16 */
/* 914 */	NdrFcShort( 0x0 ),	/* 0 */
/* 916 */	NdrFcShort( 0xa ),	/* Offset= 10 (926) */
/* 918 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 920 */	NdrFcShort( 0xffec ),	/* Offset= -20 (900) */
/* 922 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 924 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 926 */	
			0x12, 0x0,	/* FC_UP */
/* 928 */	NdrFcShort( 0xfd90 ),	/* Offset= -624 (304) */
/* 930 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 932 */	NdrFcShort( 0x0 ),	/* 0 */
/* 934 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 936 */	NdrFcShort( 0x0 ),	/* 0 */
/* 938 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 940 */	0x0 , 
			0x0,		/* 0 */
/* 942 */	NdrFcLong( 0x0 ),	/* 0 */
/* 946 */	NdrFcLong( 0x0 ),	/* 0 */
/* 950 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 954 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 956 */	0x0 , 
			0x0,		/* 0 */
/* 958 */	NdrFcLong( 0x0 ),	/* 0 */
/* 962 */	NdrFcLong( 0x0 ),	/* 0 */
/* 966 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 968 */	NdrFcShort( 0xffc6 ),	/* Offset= -58 (910) */
/* 970 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 972 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 974 */	NdrFcShort( 0x10 ),	/* 16 */
/* 976 */	NdrFcShort( 0x0 ),	/* 0 */
/* 978 */	NdrFcShort( 0xa ),	/* Offset= 10 (988) */
/* 980 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 982 */	NdrFcShort( 0xffa4 ),	/* Offset= -92 (890) */
/* 984 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 986 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 988 */	
			0x12, 0x0,	/* FC_UP */
/* 990 */	NdrFcShort( 0xffc4 ),	/* Offset= -60 (930) */
/* 992 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 994 */	NdrFcShort( 0x18 ),	/* 24 */
/* 996 */	NdrFcShort( 0x0 ),	/* 0 */
/* 998 */	NdrFcShort( 0x0 ),	/* Offset= 0 (998) */
/* 1000 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 1002 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1004 */	NdrFcShort( 0xffe0 ),	/* Offset= -32 (972) */
/* 1006 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1008 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 1010 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1012 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 1014 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1016 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1018 */	0x0 , 
			0x0,		/* 0 */
/* 1020 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1024 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1028 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 1032 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 1034 */	0x0 , 
			0x0,		/* 0 */
/* 1036 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1040 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1044 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1046 */	NdrFcShort( 0xffca ),	/* Offset= -54 (992) */
/* 1048 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1050 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 1052 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1054 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1056 */	NdrFcShort( 0xa ),	/* Offset= 10 (1066) */
/* 1058 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1060 */	NdrFcShort( 0xff4c ),	/* Offset= -180 (880) */
/* 1062 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 1064 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1066 */	
			0x12, 0x0,	/* FC_UP */
/* 1068 */	NdrFcShort( 0xffc4 ),	/* Offset= -60 (1008) */
/* 1070 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 1072 */	NdrFcShort( 0x20 ),	/* 32 */
/* 1074 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1076 */	NdrFcShort( 0xa ),	/* Offset= 10 (1086) */
/* 1078 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 1080 */	0x40,		/* FC_STRUCTPAD4 */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1082 */	0x0,		/* 0 */
			NdrFcShort( 0xffdf ),	/* Offset= -33 (1050) */
			0x5b,		/* FC_END */
/* 1086 */	
			0x12, 0x0,	/* FC_UP */
/* 1088 */	NdrFcShort( 0xfc6c ),	/* Offset= -916 (172) */
/* 1090 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 1092 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1096 */	NdrFcLong( 0x100000 ),	/* 1048576 */
/* 1100 */	0xb1,		/* FC_FORCED_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 1102 */	NdrFcShort( 0x28 ),	/* 40 */
/* 1104 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1106 */	NdrFcShort( 0x0 ),	/* Offset= 0 (1106) */
/* 1108 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 1110 */	0xb,		/* FC_HYPER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1112 */	0x0,		/* 0 */
			NdrFcShort( 0xfbb3 ),	/* Offset= -1101 (12) */
			0xb,		/* FC_HYPER */
/* 1116 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1118 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x7,		/* 7 */
/* 1120 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1122 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 1124 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 1126 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1128 */	0x0 , 
			0x0,		/* 0 */
/* 1130 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1134 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1138 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 1142 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 1144 */	0x0 , 
			0x0,		/* 0 */
/* 1146 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1150 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1154 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1156 */	NdrFcShort( 0xffc8 ),	/* Offset= -56 (1100) */
/* 1158 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1160 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 1162 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1164 */	NdrFcShort( 0xffd2 ),	/* Offset= -46 (1118) */
/* 1166 */	NdrFcShort( 0x0 ),	/* Offset= 0 (1166) */
/* 1168 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1170 */	NdrFcShort( 0xffb0 ),	/* Offset= -80 (1090) */
/* 1172 */	0x40,		/* FC_STRUCTPAD4 */
			0x5b,		/* FC_END */
/* 1174 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 1176 */	NdrFcShort( 0x40 ),	/* 64 */
/* 1178 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1180 */	NdrFcShort( 0xc ),	/* Offset= 12 (1192) */
/* 1182 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1184 */	0x0,		/* 0 */
			NdrFcShort( 0xff8d ),	/* Offset= -115 (1070) */
			0x8,		/* FC_LONG */
/* 1188 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 1190 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 1192 */	
			0x12, 0x0,	/* FC_UP */
/* 1194 */	NdrFcShort( 0xffec ),	/* Offset= -20 (1174) */
/* 1196 */	
			0x12, 0x0,	/* FC_UP */
/* 1198 */	NdrFcShort( 0xfb5e ),	/* Offset= -1186 (12) */
/* 1200 */	
			0x12, 0x0,	/* FC_UP */
/* 1202 */	NdrFcShort( 0xffd6 ),	/* Offset= -42 (1160) */
/* 1204 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 1206 */	NdrFcShort( 0x90 ),	/* 144 */
/* 1208 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1210 */	NdrFcShort( 0x20 ),	/* Offset= 32 (1242) */
/* 1212 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1214 */	NdrFcShort( 0xfb4e ),	/* Offset= -1202 (12) */
/* 1216 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1218 */	NdrFcShort( 0xfb4a ),	/* Offset= -1206 (12) */
/* 1220 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1222 */	0x0,		/* 0 */
			NdrFcShort( 0xfc4d ),	/* Offset= -947 (276) */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1226 */	0x0,		/* 0 */
			NdrFcShort( 0xfc49 ),	/* Offset= -951 (276) */
			0x36,		/* FC_POINTER */
/* 1230 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1232 */	NdrFcShort( 0xfcc4 ),	/* Offset= -828 (404) */
/* 1234 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 1236 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 1238 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 1240 */	0x40,		/* FC_STRUCTPAD4 */
			0x5b,		/* FC_END */
/* 1242 */	
			0x12, 0x0,	/* FC_UP */
/* 1244 */	NdrFcShort( 0xfbd0 ),	/* Offset= -1072 (172) */
/* 1246 */	
			0x12, 0x0,	/* FC_UP */
/* 1248 */	NdrFcShort( 0xfcf6 ),	/* Offset= -778 (470) */
/* 1250 */	
			0x12, 0x0,	/* FC_UP */
/* 1252 */	NdrFcShort( 0xffb2 ),	/* Offset= -78 (1174) */
/* 1254 */	
			0x1b,		/* FC_CARRAY */
			0x0,		/* 0 */
/* 1256 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1258 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 1260 */	NdrFcShort( 0x4 ),	/* 4 */
/* 1262 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1264 */	0x0 , 
			0x0,		/* 0 */
/* 1266 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1270 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1274 */	0x2,		/* FC_CHAR */
			0x5b,		/* FC_END */
/* 1276 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 1278 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1280 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1282 */	NdrFcShort( 0x6 ),	/* Offset= 6 (1288) */
/* 1284 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 1286 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 1288 */	
			0x12, 0x0,	/* FC_UP */
/* 1290 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (1254) */
/* 1292 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 1294 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1296 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1298 */	NdrFcShort( 0x0 ),	/* Offset= 0 (1298) */
/* 1300 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1302 */	NdrFcShort( 0xffe6 ),	/* Offset= -26 (1276) */
/* 1304 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1306 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 1308 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1312 */	NdrFcLong( 0x100000 ),	/* 1048576 */
/* 1316 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 1318 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1322 */	NdrFcLong( 0x100000 ),	/* 1048576 */
/* 1326 */	
			0x15,		/* FC_STRUCT */
			0x7,		/* 7 */
/* 1328 */	NdrFcShort( 0x20 ),	/* 32 */
/* 1330 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1332 */	NdrFcShort( 0xfad8 ),	/* Offset= -1320 (12) */
/* 1334 */	0xb,		/* FC_HYPER */
			0xb,		/* FC_HYPER */
/* 1336 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1338 */	
			0x1b,		/* FC_CARRAY */
			0x7,		/* 7 */
/* 1340 */	NdrFcShort( 0x20 ),	/* 32 */
/* 1342 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 1344 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 1346 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1348 */	0x0 , 
			0x0,		/* 0 */
/* 1350 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1354 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1358 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1360 */	NdrFcShort( 0xffde ),	/* Offset= -34 (1326) */
/* 1362 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1364 */	0xb1,		/* FC_FORCED_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 1366 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1368 */	NdrFcShort( 0xffe2 ),	/* Offset= -30 (1338) */
/* 1370 */	NdrFcShort( 0x0 ),	/* Offset= 0 (1370) */
/* 1372 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 1374 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1376 */	NdrFcShort( 0xffc4 ),	/* Offset= -60 (1316) */
/* 1378 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 1380 */	0xb1,		/* FC_FORCED_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 1382 */	NdrFcShort( 0x30 ),	/* 48 */
/* 1384 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1386 */	NdrFcShort( 0x0 ),	/* Offset= 0 (1386) */
/* 1388 */	0xb,		/* FC_HYPER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1390 */	0x0,		/* 0 */
			NdrFcShort( 0xfedd ),	/* Offset= -291 (1100) */
			0x5b,		/* FC_END */
/* 1394 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 1396 */	NdrFcShort( 0x58 ),	/* 88 */
/* 1398 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1400 */	NdrFcShort( 0x10 ),	/* Offset= 16 (1416) */
/* 1402 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 1404 */	0x40,		/* FC_STRUCTPAD4 */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1406 */	0x0,		/* 0 */
			NdrFcShort( 0xfe0f ),	/* Offset= -497 (910) */
			0x8,		/* FC_LONG */
/* 1410 */	0x40,		/* FC_STRUCTPAD4 */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1412 */	0x0,		/* 0 */
			NdrFcShort( 0xffdf ),	/* Offset= -33 (1380) */
			0x5b,		/* FC_END */
/* 1416 */	
			0x12, 0x0,	/* FC_UP */
/* 1418 */	NdrFcShort( 0xfb22 ),	/* Offset= -1246 (172) */
/* 1420 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x7,		/* 7 */
/* 1422 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1424 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 1426 */	NdrFcShort( 0x94 ),	/* 148 */
/* 1428 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1430 */	0x0 , 
			0x0,		/* 0 */
/* 1432 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1436 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1440 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 1444 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 1446 */	0x0 , 
			0x0,		/* 0 */
/* 1448 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1452 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1456 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1458 */	NdrFcShort( 0xffc0 ),	/* Offset= -64 (1394) */
/* 1460 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1462 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 1464 */	NdrFcShort( 0xa8 ),	/* 168 */
/* 1466 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1468 */	NdrFcShort( 0x28 ),	/* Offset= 40 (1508) */
/* 1470 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1472 */	NdrFcShort( 0xfa4c ),	/* Offset= -1460 (12) */
/* 1474 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1476 */	NdrFcShort( 0xfa48 ),	/* Offset= -1464 (12) */
/* 1478 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1480 */	0x0,		/* 0 */
			NdrFcShort( 0xfb4b ),	/* Offset= -1205 (276) */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1484 */	0x0,		/* 0 */
			NdrFcShort( 0xfb47 ),	/* Offset= -1209 (276) */
			0x36,		/* FC_POINTER */
/* 1488 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1490 */	NdrFcShort( 0xfbc2 ),	/* Offset= -1086 (404) */
/* 1492 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 1494 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 1496 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 1498 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 1500 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1502 */	NdrFcShort( 0xff3c ),	/* Offset= -196 (1306) */
/* 1504 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 1506 */	0x40,		/* FC_STRUCTPAD4 */
			0x5b,		/* FC_END */
/* 1508 */	
			0x12, 0x0,	/* FC_UP */
/* 1510 */	NdrFcShort( 0xfac6 ),	/* Offset= -1338 (172) */
/* 1512 */	
			0x12, 0x0,	/* FC_UP */
/* 1514 */	NdrFcShort( 0xff6a ),	/* Offset= -150 (1364) */
/* 1516 */	
			0x12, 0x0,	/* FC_UP */
/* 1518 */	NdrFcShort( 0xfea8 ),	/* Offset= -344 (1174) */
/* 1520 */	
			0x12, 0x0,	/* FC_UP */
/* 1522 */	NdrFcShort( 0xff9a ),	/* Offset= -102 (1420) */
/* 1524 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 1526 */	NdrFcShort( 0x18 ),	/* 24 */
/* 1528 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1530 */	NdrFcShort( 0x0 ),	/* Offset= 0 (1530) */
/* 1532 */	0x8,		/* FC_LONG */
			0xd,		/* FC_ENUM16 */
/* 1534 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1536 */	NdrFcShort( 0xfefc ),	/* Offset= -260 (1276) */
/* 1538 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1540 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 1542 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1546 */	NdrFcLong( 0x100000 ),	/* 1048576 */
/* 1550 */	0xb1,		/* FC_FORCED_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 1552 */	NdrFcShort( 0x48 ),	/* 72 */
/* 1554 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1556 */	NdrFcShort( 0x0 ),	/* Offset= 0 (1556) */
/* 1558 */	0xb,		/* FC_HYPER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1560 */	0x0,		/* 0 */
			NdrFcShort( 0xfe33 ),	/* Offset= -461 (1100) */
			0x8,		/* FC_LONG */
/* 1564 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 1566 */	0x40,		/* FC_STRUCTPAD4 */
			0xb,		/* FC_HYPER */
/* 1568 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1570 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 1572 */	NdrFcShort( 0x70 ),	/* 112 */
/* 1574 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1576 */	NdrFcShort( 0x10 ),	/* Offset= 16 (1592) */
/* 1578 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 1580 */	0x40,		/* FC_STRUCTPAD4 */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1582 */	0x0,		/* 0 */
			NdrFcShort( 0xfd5f ),	/* Offset= -673 (910) */
			0x8,		/* FC_LONG */
/* 1586 */	0x40,		/* FC_STRUCTPAD4 */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1588 */	0x0,		/* 0 */
			NdrFcShort( 0xffd9 ),	/* Offset= -39 (1550) */
			0x5b,		/* FC_END */
/* 1592 */	
			0x12, 0x0,	/* FC_UP */
/* 1594 */	NdrFcShort( 0xfa72 ),	/* Offset= -1422 (172) */
/* 1596 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x7,		/* 7 */
/* 1598 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1600 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 1602 */	NdrFcShort( 0x94 ),	/* 148 */
/* 1604 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1606 */	0x0 , 
			0x0,		/* 0 */
/* 1608 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1612 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1616 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 1620 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 1622 */	0x0 , 
			0x0,		/* 0 */
/* 1624 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1628 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1632 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1634 */	NdrFcShort( 0xffc0 ),	/* Offset= -64 (1570) */
/* 1636 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1638 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 1640 */	NdrFcShort( 0xa8 ),	/* 168 */
/* 1642 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1644 */	NdrFcShort( 0x28 ),	/* Offset= 40 (1684) */
/* 1646 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1648 */	NdrFcShort( 0xf99c ),	/* Offset= -1636 (12) */
/* 1650 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1652 */	NdrFcShort( 0xf998 ),	/* Offset= -1640 (12) */
/* 1654 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1656 */	0x0,		/* 0 */
			NdrFcShort( 0xfa9b ),	/* Offset= -1381 (276) */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1660 */	0x0,		/* 0 */
			NdrFcShort( 0xfa97 ),	/* Offset= -1385 (276) */
			0x36,		/* FC_POINTER */
/* 1664 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1666 */	NdrFcShort( 0xfb12 ),	/* Offset= -1262 (404) */
/* 1668 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 1670 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 1672 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 1674 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 1676 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1678 */	NdrFcShort( 0xff76 ),	/* Offset= -138 (1540) */
/* 1680 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 1682 */	0x40,		/* FC_STRUCTPAD4 */
			0x5b,		/* FC_END */
/* 1684 */	
			0x12, 0x0,	/* FC_UP */
/* 1686 */	NdrFcShort( 0xfa16 ),	/* Offset= -1514 (172) */
/* 1688 */	
			0x12, 0x0,	/* FC_UP */
/* 1690 */	NdrFcShort( 0xfeba ),	/* Offset= -326 (1364) */
/* 1692 */	
			0x12, 0x0,	/* FC_UP */
/* 1694 */	NdrFcShort( 0xfdf8 ),	/* Offset= -520 (1174) */
/* 1696 */	
			0x12, 0x0,	/* FC_UP */
/* 1698 */	NdrFcShort( 0xff9a ),	/* Offset= -102 (1596) */
/* 1700 */	
			0x11, 0x0,	/* FC_RP */
/* 1702 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1704) */
/* 1704 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 1706 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 1708 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 1710 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1712 */	0x0 , 
			0x0,		/* 0 */
/* 1714 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1718 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1722 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1724) */
/* 1724 */	NdrFcShort( 0x28 ),	/* 40 */
/* 1726 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1728 */	NdrFcLong( 0x1 ),	/* 1 */
/* 1732 */	NdrFcShort( 0x4 ),	/* Offset= 4 (1736) */
/* 1734 */	NdrFcShort( 0xffff ),	/* Offset= -1 (1733) */
/* 1736 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 1738 */	NdrFcShort( 0x28 ),	/* 40 */
/* 1740 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1742 */	NdrFcShort( 0xc ),	/* Offset= 12 (1754) */
/* 1744 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 1746 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1748 */	NdrFcShort( 0xf938 ),	/* Offset= -1736 (12) */
/* 1750 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 1752 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1754 */	
			0x11, 0x0,	/* FC_RP */
/* 1756 */	NdrFcShort( 0xf9d0 ),	/* Offset= -1584 (172) */
/* 1758 */	
			0x11, 0x8,	/* FC_RP [simple_pointer] */
/* 1760 */	
			0x22,		/* FC_C_CSTRING */
			0x5c,		/* FC_PAD */
/* 1762 */	
			0x11, 0x0,	/* FC_RP */
/* 1764 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1766) */
/* 1766 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 1768 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 1770 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 1772 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1774 */	0x0 , 
			0x0,		/* 0 */
/* 1776 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1780 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1784 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1786) */
/* 1786 */	NdrFcShort( 0x78 ),	/* 120 */
/* 1788 */	NdrFcShort( 0x2 ),	/* 2 */
/* 1790 */	NdrFcLong( 0x1 ),	/* 1 */
/* 1794 */	NdrFcShort( 0x1a ),	/* Offset= 26 (1820) */
/* 1796 */	NdrFcLong( 0x2 ),	/* 2 */
/* 1800 */	NdrFcShort( 0x2c ),	/* Offset= 44 (1844) */
/* 1802 */	NdrFcShort( 0xffff ),	/* Offset= -1 (1801) */
/* 1804 */	
			0x1d,		/* FC_SMFARRAY */
			0x0,		/* 0 */
/* 1806 */	NdrFcShort( 0x54 ),	/* 84 */
/* 1808 */	0x2,		/* FC_CHAR */
			0x5b,		/* FC_END */
/* 1810 */	
			0x15,		/* FC_STRUCT */
			0x0,		/* 0 */
/* 1812 */	NdrFcShort( 0x54 ),	/* 84 */
/* 1814 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1816 */	NdrFcShort( 0xfff4 ),	/* Offset= -12 (1804) */
/* 1818 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1820 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 1822 */	NdrFcShort( 0x68 ),	/* 104 */
/* 1824 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1826 */	NdrFcShort( 0xa ),	/* Offset= 10 (1836) */
/* 1828 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 1830 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1832 */	NdrFcShort( 0xffea ),	/* Offset= -22 (1810) */
/* 1834 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 1836 */	
			0x11, 0x0,	/* FC_RP */
/* 1838 */	NdrFcShort( 0xf97e ),	/* Offset= -1666 (172) */
/* 1840 */	
			0x11, 0x8,	/* FC_RP [simple_pointer] */
/* 1842 */	
			0x22,		/* FC_C_CSTRING */
			0x5c,		/* FC_PAD */
/* 1844 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 1846 */	NdrFcShort( 0x78 ),	/* 120 */
/* 1848 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1850 */	NdrFcShort( 0xc ),	/* Offset= 12 (1862) */
/* 1852 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 1854 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 1856 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1858 */	NdrFcShort( 0xffd0 ),	/* Offset= -48 (1810) */
/* 1860 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 1862 */	
			0x11, 0x0,	/* FC_RP */
/* 1864 */	NdrFcShort( 0xf964 ),	/* Offset= -1692 (172) */
/* 1866 */	
			0x12, 0x0,	/* FC_UP */
/* 1868 */	NdrFcShort( 0xf960 ),	/* Offset= -1696 (172) */
/* 1870 */	
			0x12, 0x0,	/* FC_UP */
/* 1872 */	NdrFcShort( 0xf95c ),	/* Offset= -1700 (172) */
/* 1874 */	
			0x11, 0x8,	/* FC_RP [simple_pointer] */
/* 1876 */	
			0x22,		/* FC_C_CSTRING */
			0x5c,		/* FC_PAD */
/* 1878 */	
			0x11, 0x0,	/* FC_RP */
/* 1880 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1882) */
/* 1882 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 1884 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 1886 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 1888 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1890 */	0x0 , 
			0x0,		/* 0 */
/* 1892 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1896 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1900 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1902) */
/* 1902 */	NdrFcShort( 0x18 ),	/* 24 */
/* 1904 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1906 */	NdrFcLong( 0x1 ),	/* 1 */
/* 1910 */	NdrFcShort( 0x4 ),	/* Offset= 4 (1914) */
/* 1912 */	NdrFcShort( 0xffff ),	/* Offset= -1 (1911) */
/* 1914 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 1916 */	NdrFcShort( 0x18 ),	/* 24 */
/* 1918 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1920 */	NdrFcShort( 0x8 ),	/* Offset= 8 (1928) */
/* 1922 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 1924 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 1926 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1928 */	
			0x11, 0x0,	/* FC_RP */
/* 1930 */	NdrFcShort( 0xf922 ),	/* Offset= -1758 (172) */
/* 1932 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1934 */	
			0x22,		/* FC_C_CSTRING */
			0x5c,		/* FC_PAD */
/* 1936 */	
			0x11, 0x0,	/* FC_RP */
/* 1938 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1940) */
/* 1940 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 1942 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 1944 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 1946 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1948 */	0x0 , 
			0x0,		/* 0 */
/* 1950 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1954 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1958 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1960) */
/* 1960 */	NdrFcShort( 0x80 ),	/* 128 */
/* 1962 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1964 */	NdrFcLong( 0x1 ),	/* 1 */
/* 1968 */	NdrFcShort( 0x4 ),	/* Offset= 4 (1972) */
/* 1970 */	NdrFcShort( 0xffff ),	/* Offset= -1 (1969) */
/* 1972 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 1974 */	NdrFcShort( 0x80 ),	/* 128 */
/* 1976 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1978 */	NdrFcShort( 0x10 ),	/* Offset= 16 (1994) */
/* 1980 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1982 */	0x0,		/* 0 */
			NdrFcShort( 0xf84d ),	/* Offset= -1971 (12) */
			0x36,		/* FC_POINTER */
/* 1986 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1988 */	NdrFcShort( 0xff4e ),	/* Offset= -178 (1810) */
/* 1990 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 1992 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 1994 */	
			0x11, 0x0,	/* FC_RP */
/* 1996 */	NdrFcShort( 0xf8e0 ),	/* Offset= -1824 (172) */
/* 1998 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2000 */	
			0x22,		/* FC_C_CSTRING */
			0x5c,		/* FC_PAD */
/* 2002 */	
			0x11, 0x0,	/* FC_RP */
/* 2004 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2006) */
/* 2006 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 2008 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 2010 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 2012 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2014 */	0x0 , 
			0x0,		/* 0 */
/* 2016 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2020 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2024 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2026) */
/* 2026 */	NdrFcShort( 0x30 ),	/* 48 */
/* 2028 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2030 */	NdrFcLong( 0x1 ),	/* 1 */
/* 2034 */	NdrFcShort( 0x38 ),	/* Offset= 56 (2090) */
/* 2036 */	NdrFcShort( 0xffff ),	/* Offset= -1 (2035) */
/* 2038 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 2040 */	NdrFcLong( 0x1 ),	/* 1 */
/* 2044 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 2048 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 2050 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2052 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 2054 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2056 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2058 */	0x0 , 
			0x0,		/* 0 */
/* 2060 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2064 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2068 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 2072 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 2074 */	0x0 , 
			0x0,		/* 0 */
/* 2076 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2080 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2084 */	
			0x12, 0x0,	/* FC_UP */
/* 2086 */	NdrFcShort( 0xf886 ),	/* Offset= -1914 (172) */
/* 2088 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 2090 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 2092 */	NdrFcShort( 0x30 ),	/* 48 */
/* 2094 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2096 */	NdrFcShort( 0x12 ),	/* Offset= 18 (2114) */
/* 2098 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 2100 */	0x0,		/* 0 */
			NdrFcShort( 0xffc1 ),	/* Offset= -63 (2038) */
			0x36,		/* FC_POINTER */
/* 2104 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 2106 */	NdrFcShort( 0xfbe0 ),	/* Offset= -1056 (1050) */
/* 2108 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 2110 */	NdrFcShort( 0xf956 ),	/* Offset= -1706 (404) */
/* 2112 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 2114 */	
			0x12, 0x0,	/* FC_UP */
/* 2116 */	NdrFcShort( 0xffbc ),	/* Offset= -68 (2048) */
/* 2118 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 2120 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2122) */
/* 2122 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 2124 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 2126 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 2128 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2130 */	0x0 , 
			0x0,		/* 0 */
/* 2132 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2136 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2140 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2142) */
/* 2142 */	NdrFcShort( 0x20 ),	/* 32 */
/* 2144 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2146 */	NdrFcLong( 0x1 ),	/* 1 */
/* 2150 */	NdrFcShort( 0x38 ),	/* Offset= 56 (2206) */
/* 2152 */	NdrFcShort( 0xffff ),	/* Offset= -1 (2151) */
/* 2154 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 2156 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2160 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 2164 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 2166 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2168 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 2170 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2172 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2174 */	0x0 , 
			0x0,		/* 0 */
/* 2176 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2180 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2184 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 2188 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 2190 */	0x0 , 
			0x0,		/* 0 */
/* 2192 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2196 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2200 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 2202 */	NdrFcShort( 0xfb94 ),	/* Offset= -1132 (1070) */
/* 2204 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 2206 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 2208 */	NdrFcShort( 0x20 ),	/* 32 */
/* 2210 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2212 */	NdrFcShort( 0xe ),	/* Offset= 14 (2226) */
/* 2214 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 2216 */	0x0,		/* 0 */
			NdrFcShort( 0xffc1 ),	/* Offset= -63 (2154) */
			0x36,		/* FC_POINTER */
/* 2220 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 2222 */	NdrFcShort( 0xf8e6 ),	/* Offset= -1818 (404) */
/* 2224 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 2226 */	
			0x12, 0x0,	/* FC_UP */
/* 2228 */	NdrFcShort( 0xffc0 ),	/* Offset= -64 (2164) */
/* 2230 */	
			0x11, 0x0,	/* FC_RP */
/* 2232 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2234) */
/* 2234 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 2236 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 2238 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 2240 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2242 */	0x0 , 
			0x0,		/* 0 */
/* 2244 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2248 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2252 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2254) */
/* 2254 */	NdrFcShort( 0x20 ),	/* 32 */
/* 2256 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2258 */	NdrFcLong( 0x1 ),	/* 1 */
/* 2262 */	NdrFcShort( 0x42 ),	/* Offset= 66 (2328) */
/* 2264 */	NdrFcShort( 0xffff ),	/* Offset= -1 (2263) */
/* 2266 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 2268 */	NdrFcLong( 0x1 ),	/* 1 */
/* 2272 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 2276 */	0xb7,		/* FC_RANGE */
			0xd,		/* 13 */
/* 2278 */	NdrFcLong( 0x1 ),	/* 1 */
/* 2282 */	NdrFcLong( 0x7 ),	/* 7 */
/* 2286 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 2288 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2290 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 2292 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2294 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2296 */	0x0 , 
			0x0,		/* 0 */
/* 2298 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2302 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2306 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 2310 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 2312 */	0x0 , 
			0x0,		/* 0 */
/* 2314 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2318 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2322 */	
			0x12, 0x0,	/* FC_UP */
/* 2324 */	NdrFcShort( 0xf798 ),	/* Offset= -2152 (172) */
/* 2326 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 2328 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 2330 */	NdrFcShort( 0x20 ),	/* 32 */
/* 2332 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2334 */	NdrFcShort( 0x10 ),	/* Offset= 16 (2350) */
/* 2336 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 2338 */	NdrFcShort( 0xffb8 ),	/* Offset= -72 (2266) */
/* 2340 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 2342 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 2344 */	0x0,		/* 0 */
			NdrFcShort( 0xffbb ),	/* Offset= -69 (2276) */
			0x36,		/* FC_POINTER */
/* 2348 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 2350 */	
			0x12, 0x0,	/* FC_UP */
/* 2352 */	NdrFcShort( 0xffbe ),	/* Offset= -66 (2286) */
/* 2354 */	
			0x12, 0x0,	/* FC_UP */
/* 2356 */	NdrFcShort( 0xf778 ),	/* Offset= -2184 (172) */
/* 2358 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 2360 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2362) */
/* 2362 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 2364 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 2366 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 2368 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2370 */	0x0 , 
			0x0,		/* 0 */
/* 2372 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2376 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2380 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2382) */
/* 2382 */	NdrFcShort( 0x28 ),	/* 40 */
/* 2384 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2386 */	NdrFcLong( 0x1 ),	/* 1 */
/* 2390 */	NdrFcShort( 0x58 ),	/* Offset= 88 (2478) */
/* 2392 */	NdrFcShort( 0xffff ),	/* Offset= -1 (2391) */
/* 2394 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 2396 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2400 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 2404 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 2406 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2410 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 2414 */	
			0x1b,		/* FC_CARRAY */
			0x3,		/* 3 */
/* 2416 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2418 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 2420 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2422 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2424 */	0x0 , 
			0x0,		/* 0 */
/* 2426 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2430 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2434 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 2436 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 2438 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2440 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 2442 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2444 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2446 */	0x0 , 
			0x0,		/* 0 */
/* 2448 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2452 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2456 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 2460 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 2462 */	0x0 , 
			0x0,		/* 0 */
/* 2464 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2468 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2472 */	
			0x12, 0x0,	/* FC_UP */
/* 2474 */	NdrFcShort( 0xf6e2 ),	/* Offset= -2334 (140) */
/* 2476 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 2478 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 2480 */	NdrFcShort( 0x28 ),	/* 40 */
/* 2482 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2484 */	NdrFcShort( 0x10 ),	/* Offset= 16 (2500) */
/* 2486 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 2488 */	0x0,		/* 0 */
			NdrFcShort( 0xffa1 ),	/* Offset= -95 (2394) */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 2492 */	0x0,		/* 0 */
			NdrFcShort( 0xffa7 ),	/* Offset= -89 (2404) */
			0x40,		/* FC_STRUCTPAD4 */
/* 2496 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 2498 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 2500 */	
			0x12, 0x0,	/* FC_UP */
/* 2502 */	NdrFcShort( 0xfe3a ),	/* Offset= -454 (2048) */
/* 2504 */	
			0x12, 0x0,	/* FC_UP */
/* 2506 */	NdrFcShort( 0xffa4 ),	/* Offset= -92 (2414) */
/* 2508 */	
			0x12, 0x0,	/* FC_UP */
/* 2510 */	NdrFcShort( 0xffb6 ),	/* Offset= -74 (2436) */
/* 2512 */	
			0x11, 0x0,	/* FC_RP */
/* 2514 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2516) */
/* 2516 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 2518 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 2520 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 2522 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2524 */	0x0 , 
			0x0,		/* 0 */
/* 2526 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2530 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2534 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2536) */
/* 2536 */	NdrFcShort( 0x40 ),	/* 64 */
/* 2538 */	NdrFcShort( 0x2 ),	/* 2 */
/* 2540 */	NdrFcLong( 0x1 ),	/* 1 */
/* 2544 */	NdrFcShort( 0xa ),	/* Offset= 10 (2554) */
/* 2546 */	NdrFcLong( 0x2 ),	/* 2 */
/* 2550 */	NdrFcShort( 0x88 ),	/* Offset= 136 (2686) */
/* 2552 */	NdrFcShort( 0xffff ),	/* Offset= -1 (2551) */
/* 2554 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 2556 */	NdrFcShort( 0x30 ),	/* 48 */
/* 2558 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2560 */	NdrFcShort( 0xc ),	/* Offset= 12 (2572) */
/* 2562 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 2564 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 2566 */	0x0,		/* 0 */
			NdrFcShort( 0xf78d ),	/* Offset= -2163 (404) */
			0x8,		/* FC_LONG */
/* 2570 */	0x40,		/* FC_STRUCTPAD4 */
			0x5b,		/* FC_END */
/* 2572 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2574 */	0x2,		/* FC_CHAR */
			0x5c,		/* FC_PAD */
/* 2576 */	
			0x12, 0x0,	/* FC_UP */
/* 2578 */	NdrFcShort( 0xfa1c ),	/* Offset= -1508 (1070) */
/* 2580 */	
			0x12, 0x0,	/* FC_UP */
/* 2582 */	NdrFcShort( 0xf5f6 ),	/* Offset= -2570 (12) */
/* 2584 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 2586 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2590 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 2594 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 2596 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2600 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 2604 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 2606 */	NdrFcShort( 0x10 ),	/* 16 */
/* 2608 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2610 */	NdrFcShort( 0xa ),	/* Offset= 10 (2620) */
/* 2612 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 2614 */	NdrFcShort( 0xffec ),	/* Offset= -20 (2594) */
/* 2616 */	0x8,		/* FC_LONG */
			0x36,		/* FC_POINTER */
/* 2618 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 2620 */	
			0x12, 0x0,	/* FC_UP */
/* 2622 */	NdrFcShort( 0xf6f2 ),	/* Offset= -2318 (304) */
/* 2624 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 2626 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2628 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 2630 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2632 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2634 */	0x0 , 
			0x0,		/* 0 */
/* 2636 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2640 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2644 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 2648 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 2650 */	0x0 , 
			0x0,		/* 0 */
/* 2652 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2656 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2660 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 2662 */	NdrFcShort( 0xffc6 ),	/* Offset= -58 (2604) */
/* 2664 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 2666 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 2668 */	NdrFcShort( 0x10 ),	/* 16 */
/* 2670 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2672 */	NdrFcShort( 0xa ),	/* Offset= 10 (2682) */
/* 2674 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 2676 */	0x0,		/* 0 */
			NdrFcShort( 0xffa3 ),	/* Offset= -93 (2584) */
			0x36,		/* FC_POINTER */
/* 2680 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 2682 */	
			0x12, 0x0,	/* FC_UP */
/* 2684 */	NdrFcShort( 0xffc4 ),	/* Offset= -60 (2624) */
/* 2686 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 2688 */	NdrFcShort( 0x40 ),	/* 64 */
/* 2690 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2692 */	NdrFcShort( 0xe ),	/* Offset= 14 (2706) */
/* 2694 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 2696 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 2698 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 2700 */	0x0,		/* 0 */
			NdrFcShort( 0xf707 ),	/* Offset= -2297 (404) */
			0x8,		/* FC_LONG */
/* 2704 */	0x40,		/* FC_STRUCTPAD4 */
			0x5b,		/* FC_END */
/* 2706 */	
			0x12, 0x0,	/* FC_UP */
/* 2708 */	NdrFcShort( 0xf618 ),	/* Offset= -2536 (172) */
/* 2710 */	
			0x12, 0x0,	/* FC_UP */
/* 2712 */	NdrFcShort( 0xf996 ),	/* Offset= -1642 (1070) */
/* 2714 */	
			0x12, 0x0,	/* FC_UP */
/* 2716 */	NdrFcShort( 0xf610 ),	/* Offset= -2544 (172) */
/* 2718 */	
			0x12, 0x0,	/* FC_UP */
/* 2720 */	NdrFcShort( 0xf60c ),	/* Offset= -2548 (172) */
/* 2722 */	
			0x12, 0x0,	/* FC_UP */
/* 2724 */	NdrFcShort( 0xffc6 ),	/* Offset= -58 (2666) */
/* 2726 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 2728 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2730) */
/* 2730 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 2732 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 2734 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 2736 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2738 */	0x0 , 
			0x0,		/* 0 */
/* 2740 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2744 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2748 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2750) */
/* 2750 */	NdrFcShort( 0x20 ),	/* 32 */
/* 2752 */	NdrFcShort( 0x2 ),	/* 2 */
/* 2754 */	NdrFcLong( 0x1 ),	/* 1 */
/* 2758 */	NdrFcShort( 0xe ),	/* Offset= 14 (2772) */
/* 2760 */	NdrFcLong( 0x2 ),	/* 2 */
/* 2764 */	NdrFcShort( 0x20 ),	/* Offset= 32 (2796) */
/* 2766 */	NdrFcShort( 0xffff ),	/* Offset= -1 (2765) */
/* 2768 */	
			0x12, 0x0,	/* FC_UP */
/* 2770 */	NdrFcShort( 0xf95c ),	/* Offset= -1700 (1070) */
/* 2772 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 2774 */	NdrFcShort( 0x20 ),	/* 32 */
/* 2776 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2778 */	NdrFcShort( 0xa ),	/* Offset= 10 (2788) */
/* 2780 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 2782 */	0x0,		/* 0 */
			NdrFcShort( 0xf6b5 ),	/* Offset= -2379 (404) */
			0x36,		/* FC_POINTER */
/* 2786 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 2788 */	
			0x12, 0x10,	/* FC_UP [pointer_deref] */
/* 2790 */	NdrFcShort( 0xffea ),	/* Offset= -22 (2768) */
/* 2792 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2794 */	0x8,		/* FC_LONG */
			0x5c,		/* FC_PAD */
/* 2796 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 2798 */	NdrFcShort( 0x10 ),	/* 16 */
/* 2800 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2802 */	NdrFcShort( 0x6 ),	/* Offset= 6 (2808) */
/* 2804 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 2806 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 2808 */	
			0x12, 0x0,	/* FC_UP */
/* 2810 */	NdrFcShort( 0xf5b2 ),	/* Offset= -2638 (172) */
/* 2812 */	
			0x11, 0x0,	/* FC_RP */
/* 2814 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2816) */
/* 2816 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 2818 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 2820 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 2822 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2824 */	0x0 , 
			0x0,		/* 0 */
/* 2826 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2830 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2834 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2836) */
/* 2836 */	NdrFcShort( 0x18 ),	/* 24 */
/* 2838 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2840 */	NdrFcLong( 0x1 ),	/* 1 */
/* 2844 */	NdrFcShort( 0x24 ),	/* Offset= 36 (2880) */
/* 2846 */	NdrFcShort( 0xffff ),	/* Offset= -1 (2845) */
/* 2848 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 2850 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2854 */	NdrFcLong( 0xa00000 ),	/* 10485760 */
/* 2858 */	
			0x1b,		/* FC_CARRAY */
			0x0,		/* 0 */
/* 2860 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2862 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 2864 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2866 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2868 */	0x0 , 
			0x0,		/* 0 */
/* 2870 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2874 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2878 */	0x2,		/* FC_CHAR */
			0x5b,		/* FC_END */
/* 2880 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 2882 */	NdrFcShort( 0x18 ),	/* 24 */
/* 2884 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2886 */	NdrFcShort( 0xc ),	/* Offset= 12 (2898) */
/* 2888 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 2890 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 2892 */	NdrFcShort( 0xffd4 ),	/* Offset= -44 (2848) */
/* 2894 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 2896 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 2898 */	
			0x12, 0x0,	/* FC_UP */
/* 2900 */	NdrFcShort( 0xffd6 ),	/* Offset= -42 (2858) */
/* 2902 */	
			0x11, 0x0,	/* FC_RP */
/* 2904 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2906) */
/* 2906 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 2908 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 2910 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 2912 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2914 */	0x0 , 
			0x0,		/* 0 */
/* 2916 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2920 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2924 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2926) */
/* 2926 */	NdrFcShort( 0x50 ),	/* 80 */
/* 2928 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2930 */	NdrFcLong( 0x1 ),	/* 1 */
/* 2934 */	NdrFcShort( 0x36 ),	/* Offset= 54 (2988) */
/* 2936 */	NdrFcShort( 0xffff ),	/* Offset= -1 (2935) */
/* 2938 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 2940 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2944 */	NdrFcLong( 0xa00000 ),	/* 10485760 */
/* 2948 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 2950 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2954 */	NdrFcLong( 0xa00000 ),	/* 10485760 */
/* 2958 */	
			0x15,		/* FC_STRUCT */
			0x7,		/* 7 */
/* 2960 */	NdrFcShort( 0x30 ),	/* 48 */
/* 2962 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 2964 */	NdrFcShort( 0xf6dc ),	/* Offset= -2340 (624) */
/* 2966 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 2968 */	NdrFcShort( 0xf6d8 ),	/* Offset= -2344 (624) */
/* 2970 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 2972 */	NdrFcShort( 0xf6d4 ),	/* Offset= -2348 (624) */
/* 2974 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 2976 */	NdrFcShort( 0xf6d0 ),	/* Offset= -2352 (624) */
/* 2978 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 2980 */	NdrFcShort( 0xf6cc ),	/* Offset= -2356 (624) */
/* 2982 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 2984 */	NdrFcShort( 0xf6c8 ),	/* Offset= -2360 (624) */
/* 2986 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 2988 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 2990 */	NdrFcShort( 0x50 ),	/* 80 */
/* 2992 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2994 */	NdrFcShort( 0x14 ),	/* Offset= 20 (3014) */
/* 2996 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 2998 */	NdrFcShort( 0xffc4 ),	/* Offset= -60 (2938) */
/* 3000 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 3002 */	NdrFcShort( 0xffca ),	/* Offset= -54 (2948) */
/* 3004 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 3006 */	NdrFcShort( 0xffd0 ),	/* Offset= -48 (2958) */
/* 3008 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 3010 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 3012 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 3014 */	
			0x12, 0x0,	/* FC_UP */
/* 3016 */	NdrFcShort( 0xf568 ),	/* Offset= -2712 (304) */
/* 3018 */	
			0x12, 0x0,	/* FC_UP */
/* 3020 */	NdrFcShort( 0xf91a ),	/* Offset= -1766 (1254) */
/* 3022 */	
			0x11, 0x0,	/* FC_RP */
/* 3024 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3026) */
/* 3026 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 3028 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 3030 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 3032 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3034 */	0x0 , 
			0x0,		/* 0 */
/* 3036 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3040 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3044 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3046) */
/* 3046 */	NdrFcShort( 0x20 ),	/* 32 */
/* 3048 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3050 */	NdrFcLong( 0x1 ),	/* 1 */
/* 3054 */	NdrFcShort( 0x38 ),	/* Offset= 56 (3110) */
/* 3056 */	NdrFcShort( 0xffff ),	/* Offset= -1 (3055) */
/* 3058 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 3060 */	NdrFcLong( 0x1 ),	/* 1 */
/* 3064 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 3068 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 3070 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3072 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 3074 */	NdrFcShort( 0x14 ),	/* 20 */
/* 3076 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3078 */	0x0 , 
			0x0,		/* 0 */
/* 3080 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3084 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3088 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 3092 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 3094 */	0x0 , 
			0x0,		/* 0 */
/* 3096 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3100 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3104 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3106 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3108 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 3110 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 3112 */	NdrFcShort( 0x20 ),	/* 32 */
/* 3114 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3116 */	NdrFcShort( 0xe ),	/* Offset= 14 (3130) */
/* 3118 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 3120 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 3122 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 3124 */	0x0,		/* 0 */
			NdrFcShort( 0xffbd ),	/* Offset= -67 (3058) */
			0x36,		/* FC_POINTER */
/* 3128 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 3130 */	
			0x12, 0x0,	/* FC_UP */
/* 3132 */	NdrFcShort( 0xffc0 ),	/* Offset= -64 (3068) */
/* 3134 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 3136 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3138) */
/* 3138 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 3140 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 3142 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 3144 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3146 */	0x0 , 
			0x0,		/* 0 */
/* 3148 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3152 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3156 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3158) */
/* 3158 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3160 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3162 */	NdrFcLong( 0x1 ),	/* 1 */
/* 3166 */	NdrFcShort( 0x54 ),	/* Offset= 84 (3250) */
/* 3168 */	NdrFcShort( 0xffff ),	/* Offset= -1 (3167) */
/* 3170 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 3172 */	NdrFcShort( 0x18 ),	/* 24 */
/* 3174 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3176 */	NdrFcShort( 0x8 ),	/* Offset= 8 (3184) */
/* 3178 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 3180 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 3182 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 3184 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3186 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3188 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3190 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3192 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 3194 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3196 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 3198 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3200 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3202 */	0x0 , 
			0x0,		/* 0 */
/* 3204 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3208 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3212 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 3216 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 3218 */	0x0 , 
			0x0,		/* 0 */
/* 3220 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3224 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3228 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 3230 */	NdrFcShort( 0xffc4 ),	/* Offset= -60 (3170) */
/* 3232 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 3234 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 3236 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3238 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3240 */	NdrFcShort( 0x6 ),	/* Offset= 6 (3246) */
/* 3242 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 3244 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 3246 */	
			0x12, 0x0,	/* FC_UP */
/* 3248 */	NdrFcShort( 0xffc8 ),	/* Offset= -56 (3192) */
/* 3250 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 3252 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3254 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3256 */	NdrFcShort( 0x4 ),	/* Offset= 4 (3260) */
/* 3258 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 3260 */	
			0x12, 0x0,	/* FC_UP */
/* 3262 */	NdrFcShort( 0xffe4 ),	/* Offset= -28 (3234) */
/* 3264 */	
			0x11, 0x0,	/* FC_RP */
/* 3266 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3268) */
/* 3268 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 3270 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 3272 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 3274 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3276 */	0x0 , 
			0x0,		/* 0 */
/* 3278 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3282 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3286 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3288) */
/* 3288 */	NdrFcShort( 0x20 ),	/* 32 */
/* 3290 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3292 */	NdrFcLong( 0x1 ),	/* 1 */
/* 3296 */	NdrFcShort( 0x38 ),	/* Offset= 56 (3352) */
/* 3298 */	NdrFcShort( 0xffff ),	/* Offset= -1 (3297) */
/* 3300 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 3302 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3306 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 3310 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 3312 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3314 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 3316 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3318 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3320 */	0x0 , 
			0x0,		/* 0 */
/* 3322 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3326 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3330 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 3334 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 3336 */	0x0 , 
			0x0,		/* 0 */
/* 3338 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3342 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3346 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3348 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3350 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 3352 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 3354 */	NdrFcShort( 0x20 ),	/* 32 */
/* 3356 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3358 */	NdrFcShort( 0xc ),	/* Offset= 12 (3370) */
/* 3360 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 3362 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 3364 */	0x0,		/* 0 */
			NdrFcShort( 0xffbf ),	/* Offset= -65 (3300) */
			0x40,		/* FC_STRUCTPAD4 */
/* 3368 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 3370 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3372 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3374 */	
			0x12, 0x0,	/* FC_UP */
/* 3376 */	NdrFcShort( 0xffbe ),	/* Offset= -66 (3310) */
/* 3378 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 3380 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3382) */
/* 3382 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 3384 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 3386 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 3388 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3390 */	0x0 , 
			0x0,		/* 0 */
/* 3392 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3396 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3400 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3402) */
/* 3402 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3404 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3406 */	NdrFcLong( 0x1 ),	/* 1 */
/* 3410 */	NdrFcShort( 0x4 ),	/* Offset= 4 (3414) */
/* 3412 */	NdrFcShort( 0xffff ),	/* Offset= -1 (3411) */
/* 3414 */	
			0x15,		/* FC_STRUCT */
			0x3,		/* 3 */
/* 3416 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3418 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 3420 */	
			0x11, 0x0,	/* FC_RP */
/* 3422 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3424) */
/* 3424 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 3426 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 3428 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 3430 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3432 */	0x0 , 
			0x0,		/* 0 */
/* 3434 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3438 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3442 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3444) */
/* 3444 */	NdrFcShort( 0x18 ),	/* 24 */
/* 3446 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3448 */	NdrFcLong( 0x1 ),	/* 1 */
/* 3452 */	NdrFcShort( 0x4 ),	/* Offset= 4 (3456) */
/* 3454 */	NdrFcShort( 0xffff ),	/* Offset= -1 (3453) */
/* 3456 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 3458 */	NdrFcShort( 0x18 ),	/* 24 */
/* 3460 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3462 */	NdrFcShort( 0x8 ),	/* Offset= 8 (3470) */
/* 3464 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 3466 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 3468 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 3470 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3472 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3474 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3476 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3478 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 3480 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3482) */
/* 3482 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 3484 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 3486 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 3488 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3490 */	0x0 , 
			0x0,		/* 0 */
/* 3492 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3496 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3500 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3502) */
/* 3502 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3504 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3506 */	NdrFcLong( 0x1 ),	/* 1 */
/* 3510 */	NdrFcShort( 0xffa0 ),	/* Offset= -96 (3414) */
/* 3512 */	NdrFcShort( 0xffff ),	/* Offset= -1 (3511) */
/* 3514 */	
			0x11, 0x0,	/* FC_RP */
/* 3516 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3518) */
/* 3518 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 3520 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 3522 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 3524 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3526 */	0x0 , 
			0x0,		/* 0 */
/* 3528 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3532 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3536 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3538) */
/* 3538 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3540 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3542 */	NdrFcLong( 0x1 ),	/* 1 */
/* 3546 */	NdrFcShort( 0x4 ),	/* Offset= 4 (3550) */
/* 3548 */	NdrFcShort( 0xffff ),	/* Offset= -1 (3547) */
/* 3550 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 3552 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3554 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3556 */	NdrFcShort( 0x4 ),	/* Offset= 4 (3560) */
/* 3558 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 3560 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3562 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3564 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 3566 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3568) */
/* 3568 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 3570 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 3572 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 3574 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3576 */	0x0 , 
			0x0,		/* 0 */
/* 3578 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3582 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3586 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3588) */
/* 3588 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3590 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3592 */	NdrFcLong( 0x1 ),	/* 1 */
/* 3596 */	NdrFcShort( 0xff4a ),	/* Offset= -182 (3414) */
/* 3598 */	NdrFcShort( 0xffff ),	/* Offset= -1 (3597) */
/* 3600 */	
			0x11, 0x0,	/* FC_RP */
/* 3602 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3604) */
/* 3604 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 3606 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 3608 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 3610 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3612 */	0x0 , 
			0x0,		/* 0 */
/* 3614 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3618 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3622 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3624) */
/* 3624 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3626 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3628 */	NdrFcLong( 0x1 ),	/* 1 */
/* 3632 */	NdrFcShort( 0x4 ),	/* Offset= 4 (3636) */
/* 3634 */	NdrFcShort( 0xffff ),	/* Offset= -1 (3633) */
/* 3636 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 3638 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3640 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3642 */	NdrFcShort( 0x6 ),	/* Offset= 6 (3648) */
/* 3644 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 3646 */	0x40,		/* FC_STRUCTPAD4 */
			0x5b,		/* FC_END */
/* 3648 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3650 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3652 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 3654 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3656) */
/* 3656 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 3658 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 3660 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 3662 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3664 */	0x0 , 
			0x0,		/* 0 */
/* 3666 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3670 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3674 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3676) */
/* 3676 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3678 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3680 */	NdrFcLong( 0x1 ),	/* 1 */
/* 3684 */	NdrFcShort( 0x6e ),	/* Offset= 110 (3794) */
/* 3686 */	NdrFcLong( 0x2 ),	/* 2 */
/* 3690 */	NdrFcShort( 0xf0 ),	/* Offset= 240 (3930) */
/* 3692 */	NdrFcLong( 0x3 ),	/* 3 */
/* 3696 */	NdrFcShort( 0x172 ),	/* Offset= 370 (4066) */
/* 3698 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 3702 */	NdrFcShort( 0x1c8 ),	/* Offset= 456 (4158) */
/* 3704 */	NdrFcShort( 0xffff ),	/* Offset= -1 (3703) */
/* 3706 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 3708 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3712 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 3716 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 3718 */	NdrFcShort( 0x30 ),	/* 48 */
/* 3720 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3722 */	NdrFcShort( 0xa ),	/* Offset= 10 (3732) */
/* 3724 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 3726 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 3728 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 3730 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 3732 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3734 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3736 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3738 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3740 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3742 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3744 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3746 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3748 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3750 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3752 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 3754 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3756 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 3758 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3760 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3762 */	0x0 , 
			0x0,		/* 0 */
/* 3764 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3768 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3772 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 3776 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 3778 */	0x0 , 
			0x0,		/* 0 */
/* 3780 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3784 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3788 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 3790 */	NdrFcShort( 0xffb6 ),	/* Offset= -74 (3716) */
/* 3792 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 3794 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 3796 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3798 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3800 */	NdrFcShort( 0xa ),	/* Offset= 10 (3810) */
/* 3802 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 3804 */	NdrFcShort( 0xff9e ),	/* Offset= -98 (3706) */
/* 3806 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 3808 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 3810 */	
			0x12, 0x0,	/* FC_UP */
/* 3812 */	NdrFcShort( 0xffc4 ),	/* Offset= -60 (3752) */
/* 3814 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 3816 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3820 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 3824 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 3826 */	NdrFcShort( 0x88 ),	/* 136 */
/* 3828 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3830 */	NdrFcShort( 0x1e ),	/* Offset= 30 (3860) */
/* 3832 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 3834 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 3836 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 3838 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 3840 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 3842 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 3844 */	NdrFcShort( 0xf108 ),	/* Offset= -3832 (12) */
/* 3846 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 3848 */	NdrFcShort( 0xf104 ),	/* Offset= -3836 (12) */
/* 3850 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 3852 */	NdrFcShort( 0xf100 ),	/* Offset= -3840 (12) */
/* 3854 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 3856 */	NdrFcShort( 0xf0fc ),	/* Offset= -3844 (12) */
/* 3858 */	0x40,		/* FC_STRUCTPAD4 */
			0x5b,		/* FC_END */
/* 3860 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3862 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3864 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3866 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3868 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3870 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3872 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3874 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3876 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3878 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3880 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3882 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3884 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3886 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3888 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 3890 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3892 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 3894 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3896 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3898 */	0x0 , 
			0x0,		/* 0 */
/* 3900 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3904 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3908 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 3912 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 3914 */	0x0 , 
			0x0,		/* 0 */
/* 3916 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3920 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3924 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 3926 */	NdrFcShort( 0xff9a ),	/* Offset= -102 (3824) */
/* 3928 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 3930 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 3932 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3934 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3936 */	NdrFcShort( 0xa ),	/* Offset= 10 (3946) */
/* 3938 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 3940 */	NdrFcShort( 0xff82 ),	/* Offset= -126 (3814) */
/* 3942 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 3944 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 3946 */	
			0x12, 0x0,	/* FC_UP */
/* 3948 */	NdrFcShort( 0xffc4 ),	/* Offset= -60 (3888) */
/* 3950 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 3952 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3956 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 3960 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 3962 */	NdrFcShort( 0x88 ),	/* 136 */
/* 3964 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3966 */	NdrFcShort( 0x1e ),	/* Offset= 30 (3996) */
/* 3968 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 3970 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 3972 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 3974 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 3976 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 3978 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 3980 */	0x0,		/* 0 */
			NdrFcShort( 0xf07f ),	/* Offset= -3969 (12) */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 3984 */	0x0,		/* 0 */
			NdrFcShort( 0xf07b ),	/* Offset= -3973 (12) */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 3988 */	0x0,		/* 0 */
			NdrFcShort( 0xf077 ),	/* Offset= -3977 (12) */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 3992 */	0x0,		/* 0 */
			NdrFcShort( 0xf073 ),	/* Offset= -3981 (12) */
			0x5b,		/* FC_END */
/* 3996 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3998 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 4000 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 4002 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 4004 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 4006 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 4008 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 4010 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 4012 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 4014 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 4016 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 4018 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 4020 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 4022 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 4024 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 4026 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4028 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 4030 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4032 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 4034 */	0x0 , 
			0x0,		/* 0 */
/* 4036 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4040 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4044 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 4048 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 4050 */	0x0 , 
			0x0,		/* 0 */
/* 4052 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4056 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4060 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4062 */	NdrFcShort( 0xff9a ),	/* Offset= -102 (3960) */
/* 4064 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4066 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4068 */	NdrFcShort( 0x10 ),	/* 16 */
/* 4070 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4072 */	NdrFcShort( 0xa ),	/* Offset= 10 (4082) */
/* 4074 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4076 */	NdrFcShort( 0xff82 ),	/* Offset= -126 (3950) */
/* 4078 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 4080 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4082 */	
			0x12, 0x0,	/* FC_UP */
/* 4084 */	NdrFcShort( 0xffc4 ),	/* Offset= -60 (4024) */
/* 4086 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 4088 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4092 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 4096 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4098 */	NdrFcShort( 0x20 ),	/* 32 */
/* 4100 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4102 */	NdrFcShort( 0xa ),	/* Offset= 10 (4112) */
/* 4104 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 4106 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 4108 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 4110 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 4112 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 4114 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 4116 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 4118 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4120 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 4122 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4124 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 4126 */	0x0 , 
			0x0,		/* 0 */
/* 4128 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4132 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4136 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 4140 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 4142 */	0x0 , 
			0x0,		/* 0 */
/* 4144 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4148 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4152 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4154 */	NdrFcShort( 0xffc6 ),	/* Offset= -58 (4096) */
/* 4156 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4158 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4160 */	NdrFcShort( 0x10 ),	/* 16 */
/* 4162 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4164 */	NdrFcShort( 0xa ),	/* Offset= 10 (4174) */
/* 4166 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4168 */	NdrFcShort( 0xffae ),	/* Offset= -82 (4086) */
/* 4170 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 4172 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4174 */	
			0x12, 0x0,	/* FC_UP */
/* 4176 */	NdrFcShort( 0xffc4 ),	/* Offset= -60 (4116) */
/* 4178 */	
			0x11, 0x0,	/* FC_RP */
/* 4180 */	NdrFcShort( 0x2 ),	/* Offset= 2 (4182) */
/* 4182 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 4184 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 4186 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 4188 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 4190 */	0x0 , 
			0x0,		/* 0 */
/* 4192 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4196 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4200 */	NdrFcShort( 0x2 ),	/* Offset= 2 (4202) */
/* 4202 */	NdrFcShort( 0x30 ),	/* 48 */
/* 4204 */	NdrFcShort( 0x3 ),	/* 3 */
/* 4206 */	NdrFcLong( 0x1 ),	/* 1 */
/* 4210 */	NdrFcShort( 0x10 ),	/* Offset= 16 (4226) */
/* 4212 */	NdrFcLong( 0x2 ),	/* 2 */
/* 4216 */	NdrFcShort( 0x2e ),	/* Offset= 46 (4262) */
/* 4218 */	NdrFcLong( 0x3 ),	/* 3 */
/* 4222 */	NdrFcShort( 0x36 ),	/* Offset= 54 (4276) */
/* 4224 */	NdrFcShort( 0xffff ),	/* Offset= -1 (4223) */
/* 4226 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4228 */	NdrFcShort( 0x18 ),	/* 24 */
/* 4230 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4232 */	NdrFcShort( 0x8 ),	/* Offset= 8 (4240) */
/* 4234 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 4236 */	0x0,		/* 0 */
			NdrFcShort( 0xf38d ),	/* Offset= -3187 (1050) */
			0x5b,		/* FC_END */
/* 4240 */	
			0x11, 0x0,	/* FC_RP */
/* 4242 */	NdrFcShort( 0xf01a ),	/* Offset= -4070 (172) */
/* 4244 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4246 */	NdrFcShort( 0x28 ),	/* 40 */
/* 4248 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4250 */	NdrFcShort( 0x8 ),	/* Offset= 8 (4258) */
/* 4252 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 4254 */	0x0,		/* 0 */
			NdrFcShort( 0xf38f ),	/* Offset= -3185 (1070) */
			0x5b,		/* FC_END */
/* 4258 */	
			0x12, 0x0,	/* FC_UP */
/* 4260 */	NdrFcShort( 0xfff0 ),	/* Offset= -16 (4244) */
/* 4262 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4264 */	NdrFcShort( 0x28 ),	/* 40 */
/* 4266 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4268 */	NdrFcShort( 0x0 ),	/* Offset= 0 (4268) */
/* 4270 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4272 */	NdrFcShort( 0xffe4 ),	/* Offset= -28 (4244) */
/* 4274 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4276 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4278 */	NdrFcShort( 0x30 ),	/* 48 */
/* 4280 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4282 */	NdrFcShort( 0x8 ),	/* Offset= 8 (4290) */
/* 4284 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4286 */	NdrFcShort( 0xffd6 ),	/* Offset= -42 (4244) */
/* 4288 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 4290 */	
			0x12, 0x0,	/* FC_UP */
/* 4292 */	NdrFcShort( 0xf9a6 ),	/* Offset= -1626 (2666) */
/* 4294 */	
			0x11, 0x0,	/* FC_RP */
/* 4296 */	NdrFcShort( 0x2 ),	/* Offset= 2 (4298) */
/* 4298 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 4300 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 4302 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 4304 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 4306 */	0x0 , 
			0x0,		/* 0 */
/* 4308 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4312 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4316 */	NdrFcShort( 0x2 ),	/* Offset= 2 (4318) */
/* 4318 */	NdrFcShort( 0x40 ),	/* 64 */
/* 4320 */	NdrFcShort( 0x3 ),	/* 3 */
/* 4322 */	NdrFcLong( 0x1 ),	/* 1 */
/* 4326 */	NdrFcShort( 0x10 ),	/* Offset= 16 (4342) */
/* 4328 */	NdrFcLong( 0x2 ),	/* 2 */
/* 4332 */	NdrFcShort( 0x64 ),	/* Offset= 100 (4432) */
/* 4334 */	NdrFcLong( 0x3 ),	/* 3 */
/* 4338 */	NdrFcShort( 0x20a ),	/* Offset= 522 (4860) */
/* 4340 */	NdrFcShort( 0xffff ),	/* Offset= -1 (4339) */
/* 4342 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4344 */	NdrFcShort( 0x40 ),	/* 64 */
/* 4346 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4348 */	NdrFcShort( 0x0 ),	/* Offset= 0 (4348) */
/* 4350 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4352 */	NdrFcShort( 0xef0c ),	/* Offset= -4340 (12) */
/* 4354 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4356 */	NdrFcShort( 0xef88 ),	/* Offset= -4216 (140) */
/* 4358 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 4360 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 4362 */	0x6,		/* FC_SHORT */
			0x3e,		/* FC_STRUCTPAD2 */
/* 4364 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4366 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 4368 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4372 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 4376 */	
			0x15,		/* FC_STRUCT */
			0x3,		/* 3 */
/* 4378 */	NdrFcShort( 0x2c ),	/* 44 */
/* 4380 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4382 */	NdrFcShort( 0xeeee ),	/* Offset= -4370 (12) */
/* 4384 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4386 */	NdrFcShort( 0xef6a ),	/* Offset= -4246 (140) */
/* 4388 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4390 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 4392 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4394 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 4396 */	NdrFcShort( 0x1c ),	/* 28 */
/* 4398 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 4400 */	0x0 , 
			0x0,		/* 0 */
/* 4402 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4406 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4410 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 4414 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 4416 */	0x0 , 
			0x0,		/* 0 */
/* 4418 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4422 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4426 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4428 */	NdrFcShort( 0xffcc ),	/* Offset= -52 (4376) */
/* 4430 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4432 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4434 */	NdrFcShort( 0x28 ),	/* 40 */
/* 4436 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4438 */	NdrFcShort( 0x10 ),	/* Offset= 16 (4454) */
/* 4440 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 4442 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 4444 */	0x8,		/* FC_LONG */
			0x6,		/* FC_SHORT */
/* 4446 */	0x3e,		/* FC_STRUCTPAD2 */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 4448 */	0x0,		/* 0 */
			NdrFcShort( 0xffad ),	/* Offset= -83 (4366) */
			0x36,		/* FC_POINTER */
/* 4452 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4454 */	
			0x12, 0x0,	/* FC_UP */
/* 4456 */	NdrFcShort( 0xef44 ),	/* Offset= -4284 (172) */
/* 4458 */	
			0x12, 0x0,	/* FC_UP */
/* 4460 */	NdrFcShort( 0xffba ),	/* Offset= -70 (4390) */
/* 4462 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 4464 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4468 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 4472 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 4474 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 4476 */	NdrFcShort( 0x8 ),	/* 8 */
/* 4478 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 4480 */	0x0 , 
			0x0,		/* 0 */
/* 4482 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4486 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4490 */	NdrFcShort( 0x2 ),	/* Offset= 2 (4492) */
/* 4492 */	NdrFcShort( 0x10 ),	/* 16 */
/* 4494 */	NdrFcShort( 0x1 ),	/* 1 */
/* 4496 */	NdrFcLong( 0x1 ),	/* 1 */
/* 4500 */	NdrFcShort( 0x12e ),	/* Offset= 302 (4802) */
/* 4502 */	NdrFcShort( 0xffff ),	/* Offset= -1 (4501) */
/* 4504 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 4506 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 4508 */	NdrFcShort( 0x4 ),	/* 4 */
/* 4510 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 4512 */	0x0 , 
			0x0,		/* 0 */
/* 4514 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4518 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4522 */	NdrFcShort( 0x2 ),	/* Offset= 2 (4524) */
/* 4524 */	NdrFcShort( 0x40 ),	/* 64 */
/* 4526 */	NdrFcShort( 0x7 ),	/* 7 */
/* 4528 */	NdrFcLong( 0x1 ),	/* 1 */
/* 4532 */	NdrFcShort( 0x4e ),	/* Offset= 78 (4610) */
/* 4534 */	NdrFcLong( 0x2 ),	/* 2 */
/* 4538 */	NdrFcShort( 0x5c ),	/* Offset= 92 (4630) */
/* 4540 */	NdrFcLong( 0x3 ),	/* 3 */
/* 4544 */	NdrFcShort( 0xe2 ),	/* Offset= 226 (4770) */
/* 4546 */	NdrFcLong( 0x4 ),	/* 4 */
/* 4550 */	NdrFcShort( 0xee ),	/* Offset= 238 (4788) */
/* 4552 */	NdrFcLong( 0x5 ),	/* 5 */
/* 4556 */	NdrFcShort( 0xe8 ),	/* Offset= 232 (4788) */
/* 4558 */	NdrFcLong( 0x6 ),	/* 6 */
/* 4562 */	NdrFcShort( 0xe2 ),	/* Offset= 226 (4788) */
/* 4564 */	NdrFcLong( 0x7 ),	/* 7 */
/* 4568 */	NdrFcShort( 0xdc ),	/* Offset= 220 (4788) */
/* 4570 */	NdrFcShort( 0xffff ),	/* Offset= -1 (4569) */
/* 4572 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4574 */	NdrFcShort( 0x28 ),	/* 40 */
/* 4576 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4578 */	NdrFcShort( 0x0 ),	/* Offset= 0 (4578) */
/* 4580 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 4582 */	0x8,		/* FC_LONG */
			0x6,		/* FC_SHORT */
/* 4584 */	0x3e,		/* FC_STRUCTPAD2 */
			0x8,		/* FC_LONG */
/* 4586 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 4588 */	0x0,		/* 0 */
			NdrFcShort( 0xf1a1 ),	/* Offset= -3679 (910) */
			0x5b,		/* FC_END */
/* 4592 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4594 */	NdrFcShort( 0x30 ),	/* 48 */
/* 4596 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4598 */	NdrFcShort( 0x8 ),	/* Offset= 8 (4606) */
/* 4600 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 4602 */	0x0,		/* 0 */
			NdrFcShort( 0xffe1 ),	/* Offset= -31 (4572) */
			0x5b,		/* FC_END */
/* 4606 */	
			0x12, 0x0,	/* FC_UP */
/* 4608 */	NdrFcShort( 0xfff0 ),	/* Offset= -16 (4592) */
/* 4610 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4612 */	NdrFcShort( 0x40 ),	/* 64 */
/* 4614 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4616 */	NdrFcShort( 0xa ),	/* Offset= 10 (4626) */
/* 4618 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 4620 */	0x40,		/* FC_STRUCTPAD4 */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 4622 */	0x0,		/* 0 */
			NdrFcShort( 0xffe1 ),	/* Offset= -31 (4592) */
			0x5b,		/* FC_END */
/* 4626 */	
			0x12, 0x0,	/* FC_UP */
/* 4628 */	NdrFcShort( 0xee98 ),	/* Offset= -4456 (172) */
/* 4630 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4632 */	NdrFcShort( 0x18 ),	/* 24 */
/* 4634 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4636 */	NdrFcShort( 0xa ),	/* Offset= 10 (4646) */
/* 4638 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 4640 */	0x8,		/* FC_LONG */
			0x6,		/* FC_SHORT */
/* 4642 */	0x3e,		/* FC_STRUCTPAD2 */
			0x36,		/* FC_POINTER */
/* 4644 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4646 */	
			0x12, 0x0,	/* FC_UP */
/* 4648 */	NdrFcShort( 0xee84 ),	/* Offset= -4476 (172) */
/* 4650 */	
			0x15,		/* FC_STRUCT */
			0x1,		/* 1 */
/* 4652 */	NdrFcShort( 0x4 ),	/* 4 */
/* 4654 */	0x2,		/* FC_CHAR */
			0x2,		/* FC_CHAR */
/* 4656 */	0x6,		/* FC_SHORT */
			0x5b,		/* FC_END */
/* 4658 */	
			0x1c,		/* FC_CVARRAY */
			0x1,		/* 1 */
/* 4660 */	NdrFcShort( 0x2 ),	/* 2 */
/* 4662 */	0x17,		/* Corr desc:  field pointer, FC_USHORT */
			0x55,		/* FC_DIV_2 */
/* 4664 */	NdrFcShort( 0x2 ),	/* 2 */
/* 4666 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 4668 */	0x0 , 
			0x0,		/* 0 */
/* 4670 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4674 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4678 */	0x17,		/* Corr desc:  field pointer, FC_USHORT */
			0x55,		/* FC_DIV_2 */
/* 4680 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4682 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 4684 */	0x0 , 
			0x0,		/* 0 */
/* 4686 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4690 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4694 */	0x5,		/* FC_WCHAR */
			0x5b,		/* FC_END */
/* 4696 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4698 */	NdrFcShort( 0x10 ),	/* 16 */
/* 4700 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4702 */	NdrFcShort( 0x8 ),	/* Offset= 8 (4710) */
/* 4704 */	0x6,		/* FC_SHORT */
			0x6,		/* FC_SHORT */
/* 4706 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 4708 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4710 */	
			0x12, 0x0,	/* FC_UP */
/* 4712 */	NdrFcShort( 0xffca ),	/* Offset= -54 (4658) */
/* 4714 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4716 */	NdrFcShort( 0x10 ),	/* 16 */
/* 4718 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4720 */	NdrFcShort( 0x6 ),	/* Offset= 6 (4726) */
/* 4722 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 4724 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4726 */	
			0x12, 0x0,	/* FC_UP */
/* 4728 */	NdrFcShort( 0xfff2 ),	/* Offset= -14 (4714) */
/* 4730 */	
			0x12, 0x0,	/* FC_UP */
/* 4732 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (4696) */
/* 4734 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4736 */	NdrFcShort( 0x30 ),	/* 48 */
/* 4738 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4740 */	NdrFcShort( 0x12 ),	/* Offset= 18 (4758) */
/* 4742 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 4744 */	0x0,		/* 0 */
			NdrFcShort( 0xffa1 ),	/* Offset= -95 (4650) */
			0x6,		/* FC_SHORT */
/* 4748 */	0x6,		/* FC_SHORT */
			0x6,		/* FC_SHORT */
/* 4750 */	0x6,		/* FC_SHORT */
			0x40,		/* FC_STRUCTPAD4 */
/* 4752 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 4754 */	0x8,		/* FC_LONG */
			0x2,		/* FC_CHAR */
/* 4756 */	0x3f,		/* FC_STRUCTPAD3 */
			0x5b,		/* FC_END */
/* 4758 */	
			0x12, 0x0,	/* FC_UP */
/* 4760 */	NdrFcShort( 0xee14 ),	/* Offset= -4588 (172) */
/* 4762 */	
			0x12, 0x0,	/* FC_UP */
/* 4764 */	NdrFcShort( 0xffce ),	/* Offset= -50 (4714) */
/* 4766 */	
			0x12, 0x0,	/* FC_UP */
/* 4768 */	NdrFcShort( 0xffde ),	/* Offset= -34 (4734) */
/* 4770 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4772 */	NdrFcShort( 0x40 ),	/* 64 */
/* 4774 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4776 */	NdrFcShort( 0x0 ),	/* Offset= 0 (4776) */
/* 4778 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 4780 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 4782 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4784 */	NdrFcShort( 0xffce ),	/* Offset= -50 (4734) */
/* 4786 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4788 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4790 */	NdrFcShort( 0x10 ),	/* 16 */
/* 4792 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4794 */	NdrFcShort( 0x0 ),	/* Offset= 0 (4794) */
/* 4796 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 4798 */	0x8,		/* FC_LONG */
			0x6,		/* FC_SHORT */
/* 4800 */	0x3e,		/* FC_STRUCTPAD2 */
			0x5b,		/* FC_END */
/* 4802 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4804 */	NdrFcShort( 0x10 ),	/* 16 */
/* 4806 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4808 */	NdrFcShort( 0x6 ),	/* Offset= 6 (4814) */
/* 4810 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 4812 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 4814 */	
			0x12, 0x0,	/* FC_UP */
/* 4816 */	NdrFcShort( 0xfec8 ),	/* Offset= -312 (4504) */
/* 4818 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 4820 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4822 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 4824 */	NdrFcShort( 0x18 ),	/* 24 */
/* 4826 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 4828 */	0x0 , 
			0x0,		/* 0 */
/* 4830 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4834 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4838 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 4842 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 4844 */	0x0 , 
			0x0,		/* 0 */
/* 4846 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4850 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4854 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4856 */	NdrFcShort( 0xfe20 ),	/* Offset= -480 (4376) */
/* 4858 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4860 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4862 */	NdrFcShort( 0x28 ),	/* 40 */
/* 4864 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4866 */	NdrFcShort( 0xe ),	/* Offset= 14 (4880) */
/* 4868 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 4870 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 4872 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4874 */	NdrFcShort( 0xfe64 ),	/* Offset= -412 (4462) */
/* 4876 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 4878 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4880 */	
			0x12, 0x0,	/* FC_UP */
/* 4882 */	NdrFcShort( 0xed9a ),	/* Offset= -4710 (172) */
/* 4884 */	
			0x12, 0x0,	/* FC_UP */
/* 4886 */	NdrFcShort( 0xfe62 ),	/* Offset= -414 (4472) */
/* 4888 */	
			0x12, 0x0,	/* FC_UP */
/* 4890 */	NdrFcShort( 0xffb8 ),	/* Offset= -72 (4818) */
/* 4892 */	
			0x11, 0x0,	/* FC_RP */
/* 4894 */	NdrFcShort( 0x2 ),	/* Offset= 2 (4896) */
/* 4896 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 4898 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 4900 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 4902 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 4904 */	0x0 , 
			0x0,		/* 0 */
/* 4906 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4910 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4914 */	NdrFcShort( 0x2 ),	/* Offset= 2 (4916) */
/* 4916 */	NdrFcShort( 0x8 ),	/* 8 */
/* 4918 */	NdrFcShort( 0x1 ),	/* 1 */
/* 4920 */	NdrFcLong( 0x1 ),	/* 1 */
/* 4924 */	NdrFcShort( 0x4 ),	/* Offset= 4 (4928) */
/* 4926 */	NdrFcShort( 0xffff ),	/* Offset= -1 (4925) */
/* 4928 */	
			0x15,		/* FC_STRUCT */
			0x3,		/* 3 */
/* 4930 */	NdrFcShort( 0x8 ),	/* 8 */
/* 4932 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 4934 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4936 */	
			0x11, 0x0,	/* FC_RP */
/* 4938 */	NdrFcShort( 0x2 ),	/* Offset= 2 (4940) */
/* 4940 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 4942 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 4944 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 4946 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 4948 */	0x0 , 
			0x0,		/* 0 */
/* 4950 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4954 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4958 */	NdrFcShort( 0x2 ),	/* Offset= 2 (4960) */
/* 4960 */	NdrFcShort( 0x40 ),	/* 64 */
/* 4962 */	NdrFcShort( 0x2 ),	/* 2 */
/* 4964 */	NdrFcLong( 0x1 ),	/* 1 */
/* 4968 */	NdrFcShort( 0xa ),	/* Offset= 10 (4978) */
/* 4970 */	NdrFcLong( 0x2 ),	/* 2 */
/* 4974 */	NdrFcShort( 0x18 ),	/* Offset= 24 (4998) */
/* 4976 */	NdrFcShort( 0xffff ),	/* Offset= -1 (4975) */
/* 4978 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4980 */	NdrFcShort( 0x20 ),	/* 32 */
/* 4982 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4984 */	NdrFcShort( 0xa ),	/* Offset= 10 (4994) */
/* 4986 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 4988 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 4990 */	0x0,		/* 0 */
			NdrFcShort( 0xec8d ),	/* Offset= -4979 (12) */
			0x5b,		/* FC_END */
/* 4994 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 4996 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 4998 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 5000 */	NdrFcShort( 0x40 ),	/* 64 */
/* 5002 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5004 */	NdrFcShort( 0x10 ),	/* Offset= 16 (5020) */
/* 5006 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 5008 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 5010 */	0x0,		/* 0 */
			NdrFcShort( 0xec79 ),	/* Offset= -4999 (12) */
			0x8,		/* FC_LONG */
/* 5014 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 5016 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 5018 */	0x40,		/* FC_STRUCTPAD4 */
			0x5b,		/* FC_END */
/* 5020 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5022 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5024 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5026 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5028 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5030 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5032 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 5034 */	NdrFcShort( 0x2 ),	/* Offset= 2 (5036) */
/* 5036 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 5038 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 5040 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 5042 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 5044 */	0x0 , 
			0x0,		/* 0 */
/* 5046 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5050 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5054 */	NdrFcShort( 0x2 ),	/* Offset= 2 (5056) */
/* 5056 */	NdrFcShort( 0x8 ),	/* 8 */
/* 5058 */	NdrFcShort( 0xf ),	/* 15 */
/* 5060 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5064 */	NdrFcShort( 0x58 ),	/* Offset= 88 (5152) */
/* 5066 */	NdrFcLong( 0x1 ),	/* 1 */
/* 5070 */	NdrFcShort( 0xc8 ),	/* Offset= 200 (5270) */
/* 5072 */	NdrFcLong( 0x2 ),	/* 2 */
/* 5076 */	NdrFcShort( 0xd0 ),	/* Offset= 208 (5284) */
/* 5078 */	NdrFcLong( 0x3 ),	/* 3 */
/* 5082 */	NdrFcShort( 0x11e ),	/* Offset= 286 (5368) */
/* 5084 */	NdrFcLong( 0x4 ),	/* 4 */
/* 5088 */	NdrFcShort( 0x118 ),	/* Offset= 280 (5368) */
/* 5090 */	NdrFcLong( 0x5 ),	/* 5 */
/* 5094 */	NdrFcShort( 0x164 ),	/* Offset= 356 (5450) */
/* 5096 */	NdrFcLong( 0x6 ),	/* 6 */
/* 5100 */	NdrFcShort( 0x1c4 ),	/* Offset= 452 (5552) */
/* 5102 */	NdrFcLong( 0x7 ),	/* 7 */
/* 5106 */	NdrFcShort( 0x23c ),	/* Offset= 572 (5678) */
/* 5108 */	NdrFcLong( 0x8 ),	/* 8 */
/* 5112 */	NdrFcShort( 0x26c ),	/* Offset= 620 (5732) */
/* 5114 */	NdrFcLong( 0x9 ),	/* 9 */
/* 5118 */	NdrFcShort( 0x2b8 ),	/* Offset= 696 (5814) */
/* 5120 */	NdrFcLong( 0xa ),	/* 10 */
/* 5124 */	NdrFcShort( 0x30c ),	/* Offset= 780 (5904) */
/* 5126 */	NdrFcLong( 0xfffffffa ),	/* -6 */
/* 5130 */	NdrFcShort( 0x374 ),	/* Offset= 884 (6014) */
/* 5132 */	NdrFcLong( 0xfffffffb ),	/* -5 */
/* 5136 */	NdrFcShort( 0x3cc ),	/* Offset= 972 (6108) */
/* 5138 */	NdrFcLong( 0xfffffffc ),	/* -4 */
/* 5142 */	NdrFcShort( 0x3ca ),	/* Offset= 970 (6112) */
/* 5144 */	NdrFcLong( 0xfffffffe ),	/* -2 */
/* 5148 */	NdrFcShort( 0x4 ),	/* Offset= 4 (5152) */
/* 5150 */	NdrFcShort( 0xffff ),	/* Offset= -1 (5149) */
/* 5152 */	
			0x12, 0x0,	/* FC_UP */
/* 5154 */	NdrFcShort( 0x68 ),	/* Offset= 104 (5258) */
/* 5156 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 5158 */	NdrFcShort( 0x90 ),	/* 144 */
/* 5160 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5162 */	NdrFcShort( 0x26 ),	/* Offset= 38 (5200) */
/* 5164 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 5166 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 5168 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 5170 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5172 */	NdrFcShort( 0xebd8 ),	/* Offset= -5160 (12) */
/* 5174 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5176 */	NdrFcShort( 0xebd4 ),	/* Offset= -5164 (12) */
/* 5178 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5180 */	NdrFcShort( 0xebd0 ),	/* Offset= -5168 (12) */
/* 5182 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5184 */	NdrFcShort( 0xebcc ),	/* Offset= -5172 (12) */
/* 5186 */	0xb,		/* FC_HYPER */
			0xb,		/* FC_HYPER */
/* 5188 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5190 */	NdrFcShort( 0xfefa ),	/* Offset= -262 (4928) */
/* 5192 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5194 */	NdrFcShort( 0xfef6 ),	/* Offset= -266 (4928) */
/* 5196 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 5198 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5200 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5202 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5204 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5206 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5208 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5210 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5212 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5214 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5216 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x7,		/* 7 */
/* 5218 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5220 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 5222 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 5224 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 5226 */	0x0 , 
			0x0,		/* 0 */
/* 5228 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5232 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5236 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 5240 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 5242 */	0x0 , 
			0x0,		/* 0 */
/* 5244 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5248 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5252 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5254 */	NdrFcShort( 0xff9e ),	/* Offset= -98 (5156) */
/* 5256 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5258 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 5260 */	NdrFcShort( 0x8 ),	/* 8 */
/* 5262 */	NdrFcShort( 0xffd2 ),	/* Offset= -46 (5216) */
/* 5264 */	NdrFcShort( 0x0 ),	/* Offset= 0 (5264) */
/* 5266 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 5268 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5270 */	
			0x12, 0x0,	/* FC_UP */
/* 5272 */	NdrFcShort( 0x2 ),	/* Offset= 2 (5274) */
/* 5274 */	
			0x17,		/* FC_CSTRUCT */
			0x7,		/* 7 */
/* 5276 */	NdrFcShort( 0x8 ),	/* 8 */
/* 5278 */	NdrFcShort( 0xed1e ),	/* Offset= -4834 (444) */
/* 5280 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 5282 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5284 */	
			0x12, 0x0,	/* FC_UP */
/* 5286 */	NdrFcShort( 0x46 ),	/* Offset= 70 (5356) */
/* 5288 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 5290 */	NdrFcShort( 0x38 ),	/* 56 */
/* 5292 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5294 */	NdrFcShort( 0x10 ),	/* Offset= 16 (5310) */
/* 5296 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 5298 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5300 */	NdrFcShort( 0xfe8c ),	/* Offset= -372 (4928) */
/* 5302 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5304 */	NdrFcShort( 0xeb54 ),	/* Offset= -5292 (12) */
/* 5306 */	0x40,		/* FC_STRUCTPAD4 */
			0xb,		/* FC_HYPER */
/* 5308 */	0xb,		/* FC_HYPER */
			0x5b,		/* FC_END */
/* 5310 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5312 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5314 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x7,		/* 7 */
/* 5316 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5318 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 5320 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 5322 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 5324 */	0x0 , 
			0x0,		/* 0 */
/* 5326 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5330 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5334 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 5338 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 5340 */	0x0 , 
			0x0,		/* 0 */
/* 5342 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5346 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5350 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5352 */	NdrFcShort( 0xffc0 ),	/* Offset= -64 (5288) */
/* 5354 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5356 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 5358 */	NdrFcShort( 0x8 ),	/* 8 */
/* 5360 */	NdrFcShort( 0xffd2 ),	/* Offset= -46 (5314) */
/* 5362 */	NdrFcShort( 0x0 ),	/* Offset= 0 (5362) */
/* 5364 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 5366 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5368 */	
			0x12, 0x0,	/* FC_UP */
/* 5370 */	NdrFcShort( 0x44 ),	/* Offset= 68 (5438) */
/* 5372 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 5374 */	NdrFcShort( 0x28 ),	/* 40 */
/* 5376 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5378 */	NdrFcShort( 0xe ),	/* Offset= 14 (5392) */
/* 5380 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 5382 */	0x0,		/* 0 */
			NdrFcShort( 0xeb05 ),	/* Offset= -5371 (12) */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 5386 */	0x0,		/* 0 */
			NdrFcShort( 0xfe35 ),	/* Offset= -459 (4928) */
			0x8,		/* FC_LONG */
/* 5390 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 5392 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5394 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5396 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 5398 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5400 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 5402 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 5404 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 5406 */	0x0 , 
			0x0,		/* 0 */
/* 5408 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5412 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5416 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 5420 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 5422 */	0x0 , 
			0x0,		/* 0 */
/* 5424 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5428 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5432 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5434 */	NdrFcShort( 0xffc2 ),	/* Offset= -62 (5372) */
/* 5436 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5438 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 5440 */	NdrFcShort( 0x8 ),	/* 8 */
/* 5442 */	NdrFcShort( 0xffd2 ),	/* Offset= -46 (5396) */
/* 5444 */	NdrFcShort( 0x0 ),	/* Offset= 0 (5444) */
/* 5446 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 5448 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5450 */	
			0x12, 0x0,	/* FC_UP */
/* 5452 */	NdrFcShort( 0x54 ),	/* Offset= 84 (5536) */
/* 5454 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 5456 */	NdrFcShort( 0x50 ),	/* 80 */
/* 5458 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5460 */	NdrFcShort( 0x16 ),	/* Offset= 22 (5482) */
/* 5462 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5464 */	NdrFcShort( 0xfde8 ),	/* Offset= -536 (4928) */
/* 5466 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 5468 */	0xd,		/* FC_ENUM16 */
			0x8,		/* FC_LONG */
/* 5470 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 5472 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 5474 */	0x0,		/* 0 */
			NdrFcShort( 0xeaa9 ),	/* Offset= -5463 (12) */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 5478 */	0x0,		/* 0 */
			NdrFcShort( 0xeaa5 ),	/* Offset= -5467 (12) */
			0x5b,		/* FC_END */
/* 5482 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5484 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5486 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5488 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5490 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5492 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5494 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 5496 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5498 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 5500 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 5502 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 5504 */	0x0 , 
			0x0,		/* 0 */
/* 5506 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5510 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5514 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 5518 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 5520 */	0x0 , 
			0x0,		/* 0 */
/* 5522 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5526 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5530 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5532 */	NdrFcShort( 0xffb2 ),	/* Offset= -78 (5454) */
/* 5534 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5536 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 5538 */	NdrFcShort( 0x10 ),	/* 16 */
/* 5540 */	NdrFcShort( 0xffd2 ),	/* Offset= -46 (5494) */
/* 5542 */	NdrFcShort( 0x0 ),	/* Offset= 0 (5542) */
/* 5544 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5546 */	NdrFcShort( 0xfd96 ),	/* Offset= -618 (4928) */
/* 5548 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 5550 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5552 */	
			0x12, 0x0,	/* FC_UP */
/* 5554 */	NdrFcShort( 0x70 ),	/* Offset= 112 (5666) */
/* 5556 */	
			0x1b,		/* FC_CARRAY */
			0x0,		/* 0 */
/* 5558 */	NdrFcShort( 0x1 ),	/* 1 */
/* 5560 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 5562 */	NdrFcShort( 0x10 ),	/* 16 */
/* 5564 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 5566 */	0x0 , 
			0x0,		/* 0 */
/* 5568 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5572 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5576 */	0x2,		/* FC_CHAR */
			0x5b,		/* FC_END */
/* 5578 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 5580 */	NdrFcShort( 0x60 ),	/* 96 */
/* 5582 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5584 */	NdrFcShort( 0x1c ),	/* Offset= 28 (5612) */
/* 5586 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 5588 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 5590 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 5592 */	0x0,		/* 0 */
			NdrFcShort( 0xfd67 ),	/* Offset= -665 (4928) */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 5596 */	0x0,		/* 0 */
			NdrFcShort( 0xfd63 ),	/* Offset= -669 (4928) */
			0x8,		/* FC_LONG */
/* 5600 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5602 */	NdrFcShort( 0xfd5e ),	/* Offset= -674 (4928) */
/* 5604 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5606 */	NdrFcShort( 0xea26 ),	/* Offset= -5594 (12) */
/* 5608 */	0x40,		/* FC_STRUCTPAD4 */
			0xb,		/* FC_HYPER */
/* 5610 */	0xb,		/* FC_HYPER */
			0x5b,		/* FC_END */
/* 5612 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5614 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5616 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5618 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5620 */	
			0x14, 0x0,	/* FC_FP */
/* 5622 */	NdrFcShort( 0xffbe ),	/* Offset= -66 (5556) */
/* 5624 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x7,		/* 7 */
/* 5626 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5628 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 5630 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 5632 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 5634 */	0x0 , 
			0x0,		/* 0 */
/* 5636 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5640 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5644 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 5648 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 5650 */	0x0 , 
			0x0,		/* 0 */
/* 5652 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5656 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5660 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5662 */	NdrFcShort( 0xffac ),	/* Offset= -84 (5578) */
/* 5664 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5666 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 5668 */	NdrFcShort( 0x8 ),	/* 8 */
/* 5670 */	NdrFcShort( 0xffd2 ),	/* Offset= -46 (5624) */
/* 5672 */	NdrFcShort( 0x0 ),	/* Offset= 0 (5672) */
/* 5674 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 5676 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5678 */	
			0x12, 0x0,	/* FC_UP */
/* 5680 */	NdrFcShort( 0x2a ),	/* Offset= 42 (5722) */
/* 5682 */	
			0x15,		/* FC_STRUCT */
			0x7,		/* 7 */
/* 5684 */	NdrFcShort( 0x20 ),	/* 32 */
/* 5686 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5688 */	NdrFcShort( 0xe9d4 ),	/* Offset= -5676 (12) */
/* 5690 */	0xb,		/* FC_HYPER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 5692 */	0x0,		/* 0 */
			NdrFcShort( 0xfd03 ),	/* Offset= -765 (4928) */
			0x5b,		/* FC_END */
/* 5696 */	
			0x1b,		/* FC_CARRAY */
			0x7,		/* 7 */
/* 5698 */	NdrFcShort( 0x20 ),	/* 32 */
/* 5700 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 5702 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 5704 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 5706 */	0x0 , 
			0x0,		/* 0 */
/* 5708 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5712 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5716 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5718 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (5682) */
/* 5720 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5722 */	
			0x17,		/* FC_CSTRUCT */
			0x7,		/* 7 */
/* 5724 */	NdrFcShort( 0x8 ),	/* 8 */
/* 5726 */	NdrFcShort( 0xffe2 ),	/* Offset= -30 (5696) */
/* 5728 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 5730 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5732 */	
			0x12, 0x0,	/* FC_UP */
/* 5734 */	NdrFcShort( 0x44 ),	/* Offset= 68 (5802) */
/* 5736 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 5738 */	NdrFcShort( 0x28 ),	/* 40 */
/* 5740 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5742 */	NdrFcShort( 0xe ),	/* Offset= 14 (5756) */
/* 5744 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5746 */	NdrFcShort( 0xe99a ),	/* Offset= -5734 (12) */
/* 5748 */	0xb,		/* FC_HYPER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 5750 */	0x0,		/* 0 */
			NdrFcShort( 0xfcc9 ),	/* Offset= -823 (4928) */
			0x36,		/* FC_POINTER */
/* 5754 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5756 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5758 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5760 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x7,		/* 7 */
/* 5762 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5764 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 5766 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 5768 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 5770 */	0x0 , 
			0x0,		/* 0 */
/* 5772 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5776 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5780 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 5784 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 5786 */	0x0 , 
			0x0,		/* 0 */
/* 5788 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5792 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5796 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5798 */	NdrFcShort( 0xffc2 ),	/* Offset= -62 (5736) */
/* 5800 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5802 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 5804 */	NdrFcShort( 0x8 ),	/* 8 */
/* 5806 */	NdrFcShort( 0xffd2 ),	/* Offset= -46 (5760) */
/* 5808 */	NdrFcShort( 0x0 ),	/* Offset= 0 (5808) */
/* 5810 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 5812 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5814 */	
			0x12, 0x0,	/* FC_UP */
/* 5816 */	NdrFcShort( 0x4c ),	/* Offset= 76 (5892) */
/* 5818 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 5820 */	NdrFcShort( 0x40 ),	/* 64 */
/* 5822 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5824 */	NdrFcShort( 0x12 ),	/* Offset= 18 (5842) */
/* 5826 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 5828 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5830 */	NdrFcShort( 0xfc7a ),	/* Offset= -902 (4928) */
/* 5832 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5834 */	NdrFcShort( 0xe942 ),	/* Offset= -5822 (12) */
/* 5836 */	0x40,		/* FC_STRUCTPAD4 */
			0xb,		/* FC_HYPER */
/* 5838 */	0xb,		/* FC_HYPER */
			0x36,		/* FC_POINTER */
/* 5840 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5842 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5844 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5846 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5848 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5850 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x7,		/* 7 */
/* 5852 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5854 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 5856 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 5858 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 5860 */	0x0 , 
			0x0,		/* 0 */
/* 5862 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5866 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5870 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 5874 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 5876 */	0x0 , 
			0x0,		/* 0 */
/* 5878 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5882 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5886 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5888 */	NdrFcShort( 0xffba ),	/* Offset= -70 (5818) */
/* 5890 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5892 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 5894 */	NdrFcShort( 0x8 ),	/* 8 */
/* 5896 */	NdrFcShort( 0xffd2 ),	/* Offset= -46 (5850) */
/* 5898 */	NdrFcShort( 0x0 ),	/* Offset= 0 (5898) */
/* 5900 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 5902 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5904 */	
			0x12, 0x0,	/* FC_UP */
/* 5906 */	NdrFcShort( 0x60 ),	/* Offset= 96 (6002) */
/* 5908 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 5910 */	NdrFcShort( 0x68 ),	/* 104 */
/* 5912 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5914 */	NdrFcShort( 0x1e ),	/* Offset= 30 (5944) */
/* 5916 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 5918 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 5920 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 5922 */	0x0,		/* 0 */
			NdrFcShort( 0xfc1d ),	/* Offset= -995 (4928) */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 5926 */	0x0,		/* 0 */
			NdrFcShort( 0xfc19 ),	/* Offset= -999 (4928) */
			0x8,		/* FC_LONG */
/* 5930 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5932 */	NdrFcShort( 0xfc14 ),	/* Offset= -1004 (4928) */
/* 5934 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5936 */	NdrFcShort( 0xe8dc ),	/* Offset= -5924 (12) */
/* 5938 */	0x40,		/* FC_STRUCTPAD4 */
			0xb,		/* FC_HYPER */
/* 5940 */	0xb,		/* FC_HYPER */
			0x36,		/* FC_POINTER */
/* 5942 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5944 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5946 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5948 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5950 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5952 */	
			0x14, 0x0,	/* FC_FP */
/* 5954 */	NdrFcShort( 0xfe72 ),	/* Offset= -398 (5556) */
/* 5956 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5958 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5960 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x7,		/* 7 */
/* 5962 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5964 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 5966 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 5968 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 5970 */	0x0 , 
			0x0,		/* 0 */
/* 5972 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5976 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5980 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 5984 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 5986 */	0x0 , 
			0x0,		/* 0 */
/* 5988 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5992 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5996 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5998 */	NdrFcShort( 0xffa6 ),	/* Offset= -90 (5908) */
/* 6000 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6002 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 6004 */	NdrFcShort( 0x8 ),	/* 8 */
/* 6006 */	NdrFcShort( 0xffd2 ),	/* Offset= -46 (5960) */
/* 6008 */	NdrFcShort( 0x0 ),	/* Offset= 0 (6008) */
/* 6010 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 6012 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6014 */	
			0x12, 0x0,	/* FC_UP */
/* 6016 */	NdrFcShort( 0x4e ),	/* Offset= 78 (6094) */
/* 6018 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 6020 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6024 */	NdrFcLong( 0x100 ),	/* 256 */
/* 6028 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 6030 */	NdrFcShort( 0x30 ),	/* 48 */
/* 6032 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6034 */	NdrFcShort( 0xe ),	/* Offset= 14 (6048) */
/* 6036 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 6038 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 6040 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 6042 */	0x40,		/* FC_STRUCTPAD4 */
			0xb,		/* FC_HYPER */
/* 6044 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 6046 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6048 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 6050 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 6052 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x7,		/* 7 */
/* 6054 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6056 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 6058 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 6060 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6062 */	0x0 , 
			0x0,		/* 0 */
/* 6064 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6068 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6072 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 6076 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 6078 */	0x0 , 
			0x0,		/* 0 */
/* 6080 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6084 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6088 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 6090 */	NdrFcShort( 0xffc2 ),	/* Offset= -62 (6028) */
/* 6092 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6094 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 6096 */	NdrFcShort( 0x8 ),	/* 8 */
/* 6098 */	NdrFcShort( 0xffd2 ),	/* Offset= -46 (6052) */
/* 6100 */	NdrFcShort( 0x0 ),	/* Offset= 0 (6100) */
/* 6102 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 6104 */	NdrFcShort( 0xffaa ),	/* Offset= -86 (6018) */
/* 6106 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 6108 */	
			0x12, 0x0,	/* FC_UP */
/* 6110 */	NdrFcShort( 0xe9f8 ),	/* Offset= -5640 (470) */
/* 6112 */	
			0x12, 0x0,	/* FC_UP */
/* 6114 */	NdrFcShort( 0x36 ),	/* Offset= 54 (6168) */
/* 6116 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 6118 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6122 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 6126 */	
			0x15,		/* FC_STRUCT */
			0x7,		/* 7 */
/* 6128 */	NdrFcShort( 0x30 ),	/* 48 */
/* 6130 */	0xb,		/* FC_HYPER */
			0x8,		/* FC_LONG */
/* 6132 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 6134 */	0x0,		/* 0 */
			NdrFcShort( 0xe815 ),	/* Offset= -6123 (12) */
			0xb,		/* FC_HYPER */
/* 6138 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 6140 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6142 */	
			0x1b,		/* FC_CARRAY */
			0x7,		/* 7 */
/* 6144 */	NdrFcShort( 0x30 ),	/* 48 */
/* 6146 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 6148 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 6150 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6152 */	0x0 , 
			0x0,		/* 0 */
/* 6154 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6158 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6162 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 6164 */	NdrFcShort( 0xffda ),	/* Offset= -38 (6126) */
/* 6166 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6168 */	0xb1,		/* FC_FORCED_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 6170 */	NdrFcShort( 0x8 ),	/* 8 */
/* 6172 */	NdrFcShort( 0xffe2 ),	/* Offset= -30 (6142) */
/* 6174 */	NdrFcShort( 0x0 ),	/* Offset= 0 (6174) */
/* 6176 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 6178 */	NdrFcShort( 0xffc2 ),	/* Offset= -62 (6116) */
/* 6180 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 6182 */	
			0x11, 0x0,	/* FC_RP */
/* 6184 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6186) */
/* 6186 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 6188 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 6190 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 6192 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6194 */	0x0 , 
			0x0,		/* 0 */
/* 6196 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6200 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6204 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6206) */
/* 6206 */	NdrFcShort( 0x60 ),	/* 96 */
/* 6208 */	NdrFcShort( 0x1 ),	/* 1 */
/* 6210 */	NdrFcLong( 0x1 ),	/* 1 */
/* 6214 */	NdrFcShort( 0x64 ),	/* Offset= 100 (6314) */
/* 6216 */	NdrFcShort( 0xffff ),	/* Offset= -1 (6215) */
/* 6218 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 6220 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6224 */	NdrFcLong( 0x100 ),	/* 256 */
/* 6228 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 6230 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6234 */	NdrFcLong( 0x100 ),	/* 256 */
/* 6238 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 6240 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6244 */	NdrFcLong( 0x100 ),	/* 256 */
/* 6248 */	
			0x1b,		/* FC_CARRAY */
			0x1,		/* 1 */
/* 6250 */	NdrFcShort( 0x2 ),	/* 2 */
/* 6252 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 6254 */	NdrFcShort( 0x20 ),	/* 32 */
/* 6256 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6258 */	0x0 , 
			0x0,		/* 0 */
/* 6260 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6264 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6268 */	0x5,		/* FC_WCHAR */
			0x5b,		/* FC_END */
/* 6270 */	
			0x1b,		/* FC_CARRAY */
			0x1,		/* 1 */
/* 6272 */	NdrFcShort( 0x2 ),	/* 2 */
/* 6274 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 6276 */	NdrFcShort( 0x30 ),	/* 48 */
/* 6278 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6280 */	0x0 , 
			0x0,		/* 0 */
/* 6282 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6286 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6290 */	0x5,		/* FC_WCHAR */
			0x5b,		/* FC_END */
/* 6292 */	
			0x1b,		/* FC_CARRAY */
			0x1,		/* 1 */
/* 6294 */	NdrFcShort( 0x2 ),	/* 2 */
/* 6296 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 6298 */	NdrFcShort( 0x40 ),	/* 64 */
/* 6300 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6302 */	0x0 , 
			0x0,		/* 0 */
/* 6304 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6308 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6312 */	0x5,		/* FC_WCHAR */
			0x5b,		/* FC_END */
/* 6314 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 6316 */	NdrFcShort( 0x60 ),	/* 96 */
/* 6318 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6320 */	NdrFcShort( 0x1c ),	/* Offset= 28 (6348) */
/* 6322 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 6324 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 6326 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 6328 */	0x0,		/* 0 */
			NdrFcShort( 0xff91 ),	/* Offset= -111 (6218) */
			0x40,		/* FC_STRUCTPAD4 */
/* 6332 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 6334 */	0x0,		/* 0 */
			NdrFcShort( 0xff95 ),	/* Offset= -107 (6228) */
			0x40,		/* FC_STRUCTPAD4 */
/* 6338 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 6340 */	0x0,		/* 0 */
			NdrFcShort( 0xff99 ),	/* Offset= -103 (6238) */
			0x40,		/* FC_STRUCTPAD4 */
/* 6344 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 6346 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 6348 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 6350 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 6352 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 6354 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 6356 */	
			0x14, 0x8,	/* FC_FP [simple_pointer] */
/* 6358 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 6360 */	
			0x12, 0x0,	/* FC_UP */
/* 6362 */	NdrFcShort( 0xff8e ),	/* Offset= -114 (6248) */
/* 6364 */	
			0x12, 0x0,	/* FC_UP */
/* 6366 */	NdrFcShort( 0xffa0 ),	/* Offset= -96 (6270) */
/* 6368 */	
			0x12, 0x0,	/* FC_UP */
/* 6370 */	NdrFcShort( 0xffb2 ),	/* Offset= -78 (6292) */
/* 6372 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 6374 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 6376 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 6378 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 6380 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 6382 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6384) */
/* 6384 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 6386 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 6388 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 6390 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6392 */	0x0 , 
			0x0,		/* 0 */
/* 6394 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6398 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6402 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6404) */
/* 6404 */	NdrFcShort( 0x4 ),	/* 4 */
/* 6406 */	NdrFcShort( 0x1 ),	/* 1 */
/* 6408 */	NdrFcLong( 0x1 ),	/* 1 */
/* 6412 */	NdrFcShort( 0xf44a ),	/* Offset= -2998 (3414) */
/* 6414 */	NdrFcShort( 0xffff ),	/* Offset= -1 (6413) */
/* 6416 */	
			0x11, 0x0,	/* FC_RP */
/* 6418 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6420) */
/* 6420 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 6422 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 6424 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 6426 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6428 */	0x0 , 
			0x0,		/* 0 */
/* 6430 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6434 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6438 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6440) */
/* 6440 */	NdrFcShort( 0x10 ),	/* 16 */
/* 6442 */	NdrFcShort( 0x1 ),	/* 1 */
/* 6444 */	NdrFcLong( 0x1 ),	/* 1 */
/* 6448 */	NdrFcShort( 0x38 ),	/* Offset= 56 (6504) */
/* 6450 */	NdrFcShort( 0xffff ),	/* Offset= -1 (6449) */
/* 6452 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 6454 */	NdrFcLong( 0x1 ),	/* 1 */
/* 6458 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 6462 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 6464 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6466 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 6468 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6470 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6472 */	0x0 , 
			0x0,		/* 0 */
/* 6474 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6478 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6482 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 6486 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 6488 */	0x0 , 
			0x0,		/* 0 */
/* 6490 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6494 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6498 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 6500 */	NdrFcShort( 0xefb4 ),	/* Offset= -4172 (2328) */
/* 6502 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6504 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 6506 */	NdrFcShort( 0x10 ),	/* 16 */
/* 6508 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6510 */	NdrFcShort( 0xa ),	/* Offset= 10 (6520) */
/* 6512 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 6514 */	NdrFcShort( 0xffc2 ),	/* Offset= -62 (6452) */
/* 6516 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 6518 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6520 */	
			0x12, 0x0,	/* FC_UP */
/* 6522 */	NdrFcShort( 0xffc4 ),	/* Offset= -60 (6462) */
/* 6524 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 6526 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6528) */
/* 6528 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 6530 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 6532 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 6534 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6536 */	0x0 , 
			0x0,		/* 0 */
/* 6538 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6542 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6546 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6548) */
/* 6548 */	NdrFcShort( 0x10 ),	/* 16 */
/* 6550 */	NdrFcShort( 0x1 ),	/* 1 */
/* 6552 */	NdrFcLong( 0x1 ),	/* 1 */
/* 6556 */	NdrFcShort( 0x38 ),	/* Offset= 56 (6612) */
/* 6558 */	NdrFcShort( 0xffff ),	/* Offset= -1 (6557) */
/* 6560 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 6562 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6566 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 6570 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 6572 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6574 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 6576 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6578 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6580 */	0x0 , 
			0x0,		/* 0 */
/* 6582 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6586 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6590 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 6594 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 6596 */	0x0 , 
			0x0,		/* 0 */
/* 6598 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6602 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6606 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 6608 */	NdrFcShort( 0xefde ),	/* Offset= -4130 (2478) */
/* 6610 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6612 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 6614 */	NdrFcShort( 0x10 ),	/* 16 */
/* 6616 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6618 */	NdrFcShort( 0xa ),	/* Offset= 10 (6628) */
/* 6620 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 6622 */	NdrFcShort( 0xffc2 ),	/* Offset= -62 (6560) */
/* 6624 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 6626 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6628 */	
			0x12, 0x0,	/* FC_UP */
/* 6630 */	NdrFcShort( 0xffc4 ),	/* Offset= -60 (6570) */
/* 6632 */	
			0x11, 0x0,	/* FC_RP */
/* 6634 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6636) */
/* 6636 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 6638 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 6640 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 6642 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6644 */	0x0 , 
			0x0,		/* 0 */
/* 6646 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6650 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6654 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6656) */
/* 6656 */	NdrFcShort( 0x20 ),	/* 32 */
/* 6658 */	NdrFcShort( 0x1 ),	/* 1 */
/* 6660 */	NdrFcLong( 0x1 ),	/* 1 */
/* 6664 */	NdrFcShort( 0x4 ),	/* Offset= 4 (6668) */
/* 6666 */	NdrFcShort( 0xffff ),	/* Offset= -1 (6665) */
/* 6668 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 6670 */	NdrFcShort( 0x20 ),	/* 32 */
/* 6672 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6674 */	NdrFcShort( 0xa ),	/* Offset= 10 (6684) */
/* 6676 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 6678 */	0x0,		/* 0 */
			NdrFcShort( 0xe5f5 ),	/* Offset= -6667 (12) */
			0x8,		/* FC_LONG */
/* 6682 */	0x40,		/* FC_STRUCTPAD4 */
			0x5b,		/* FC_END */
/* 6684 */	
			0x11, 0x0,	/* FC_RP */
/* 6686 */	NdrFcShort( 0xe68e ),	/* Offset= -6514 (172) */
/* 6688 */	
			0x11, 0x0,	/* FC_RP */
/* 6690 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6692) */
/* 6692 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 6694 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 6696 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 6698 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6700 */	0x0 , 
			0x0,		/* 0 */
/* 6702 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6706 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6710 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6712) */
/* 6712 */	NdrFcShort( 0x38 ),	/* 56 */
/* 6714 */	NdrFcShort( 0x1 ),	/* 1 */
/* 6716 */	NdrFcLong( 0x1 ),	/* 1 */
/* 6720 */	NdrFcShort( 0xa ),	/* Offset= 10 (6730) */
/* 6722 */	NdrFcShort( 0xffff ),	/* Offset= -1 (6721) */
/* 6724 */	
			0x1d,		/* FC_SMFARRAY */
			0x0,		/* 0 */
/* 6726 */	NdrFcShort( 0x10 ),	/* 16 */
/* 6728 */	0x2,		/* FC_CHAR */
			0x5b,		/* FC_END */
/* 6730 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 6732 */	NdrFcShort( 0x38 ),	/* 56 */
/* 6734 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6736 */	NdrFcShort( 0x10 ),	/* Offset= 16 (6752) */
/* 6738 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 6740 */	NdrFcShort( 0xe5b8 ),	/* Offset= -6728 (12) */
/* 6742 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 6744 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 6746 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 6748 */	NdrFcShort( 0xffe8 ),	/* Offset= -24 (6724) */
/* 6750 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6752 */	
			0x12, 0x0,	/* FC_UP */
/* 6754 */	NdrFcShort( 0xe64a ),	/* Offset= -6582 (172) */
/* 6756 */	
			0x12, 0x0,	/* FC_UP */
/* 6758 */	NdrFcShort( 0xe770 ),	/* Offset= -6288 (470) */
/* 6760 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 6762 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6764) */
/* 6764 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 6766 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 6768 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 6770 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6772 */	0x0 , 
			0x0,		/* 0 */
/* 6774 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6778 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6782 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6784) */
/* 6784 */	NdrFcShort( 0x10 ),	/* 16 */
/* 6786 */	NdrFcShort( 0x1 ),	/* 1 */
/* 6788 */	NdrFcLong( 0x1 ),	/* 1 */
/* 6792 */	NdrFcShort( 0x38 ),	/* Offset= 56 (6848) */
/* 6794 */	NdrFcShort( 0xffff ),	/* Offset= -1 (6793) */
/* 6796 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 6798 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6802 */	NdrFcLong( 0xa00000 ),	/* 10485760 */
/* 6806 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 6808 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6810 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 6812 */	NdrFcShort( 0x4 ),	/* 4 */
/* 6814 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6816 */	0x0 , 
			0x0,		/* 0 */
/* 6818 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6822 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6826 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 6830 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 6832 */	0x0 , 
			0x0,		/* 0 */
/* 6834 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6838 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6842 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 6844 */	NdrFcShort( 0xe550 ),	/* Offset= -6832 (12) */
/* 6846 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6848 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 6850 */	NdrFcShort( 0x10 ),	/* 16 */
/* 6852 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6854 */	NdrFcShort( 0xa ),	/* Offset= 10 (6864) */
/* 6856 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 6858 */	0x0,		/* 0 */
			NdrFcShort( 0xffc1 ),	/* Offset= -63 (6796) */
			0x36,		/* FC_POINTER */
/* 6862 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6864 */	
			0x12, 0x0,	/* FC_UP */
/* 6866 */	NdrFcShort( 0xffc4 ),	/* Offset= -60 (6806) */
/* 6868 */	
			0x11, 0x0,	/* FC_RP */
/* 6870 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6872) */
/* 6872 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 6874 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 6876 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 6878 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6880 */	0x0 , 
			0x0,		/* 0 */
/* 6882 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6886 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6890 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6892) */
/* 6892 */	NdrFcShort( 0x20 ),	/* 32 */
/* 6894 */	NdrFcShort( 0x1 ),	/* 1 */
/* 6896 */	NdrFcLong( 0x1 ),	/* 1 */
/* 6900 */	NdrFcShort( 0x38 ),	/* Offset= 56 (6956) */
/* 6902 */	NdrFcShort( 0xffff ),	/* Offset= -1 (6901) */
/* 6904 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 6906 */	NdrFcLong( 0x1 ),	/* 1 */
/* 6910 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 6914 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 6916 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6918 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 6920 */	NdrFcShort( 0x8 ),	/* 8 */
/* 6922 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6924 */	0x0 , 
			0x0,		/* 0 */
/* 6926 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6930 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6934 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 6938 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 6940 */	0x0 , 
			0x0,		/* 0 */
/* 6942 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6946 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6950 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 6952 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 6954 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6956 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 6958 */	NdrFcShort( 0x20 ),	/* 32 */
/* 6960 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6962 */	NdrFcShort( 0xc ),	/* Offset= 12 (6974) */
/* 6964 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 6966 */	0x0,		/* 0 */
			NdrFcShort( 0xffc1 ),	/* Offset= -63 (6904) */
			0x40,		/* FC_STRUCTPAD4 */
/* 6970 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 6972 */	0x40,		/* FC_STRUCTPAD4 */
			0x5b,		/* FC_END */
/* 6974 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 6976 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 6978 */	
			0x12, 0x0,	/* FC_UP */
/* 6980 */	NdrFcShort( 0xffbe ),	/* Offset= -66 (6914) */
/* 6982 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 6984 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6986) */
/* 6986 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 6988 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 6990 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 6992 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6994 */	0x0 , 
			0x0,		/* 0 */
/* 6996 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7000 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7004 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7006) */
/* 7006 */	NdrFcShort( 0x18 ),	/* 24 */
/* 7008 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7010 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7014 */	NdrFcShort( 0x38 ),	/* Offset= 56 (7070) */
/* 7016 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7015) */
/* 7018 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 7020 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7024 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 7028 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 7030 */	NdrFcShort( 0x0 ),	/* 0 */
/* 7032 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 7034 */	NdrFcShort( 0x0 ),	/* 0 */
/* 7036 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7038 */	0x0 , 
			0x0,		/* 0 */
/* 7040 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7044 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7048 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 7052 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 7054 */	0x0 , 
			0x0,		/* 0 */
/* 7056 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7060 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7064 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 7066 */	NdrFcShort( 0xf7a6 ),	/* Offset= -2138 (4928) */
/* 7068 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 7070 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 7072 */	NdrFcShort( 0x18 ),	/* 24 */
/* 7074 */	NdrFcShort( 0x0 ),	/* 0 */
/* 7076 */	NdrFcShort( 0xc ),	/* Offset= 12 (7088) */
/* 7078 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 7080 */	NdrFcShort( 0xffc2 ),	/* Offset= -62 (7018) */
/* 7082 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 7084 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 7086 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 7088 */	
			0x12, 0x0,	/* FC_UP */
/* 7090 */	NdrFcShort( 0xffc2 ),	/* Offset= -62 (7028) */
/* 7092 */	
			0x11, 0x0,	/* FC_RP */
/* 7094 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7096) */
/* 7096 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7098 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 7100 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 7102 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7104 */	0x0 , 
			0x0,		/* 0 */
/* 7106 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7110 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7114 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7116) */
/* 7116 */	NdrFcShort( 0x4 ),	/* 4 */
/* 7118 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7120 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7124 */	NdrFcShort( 0xf182 ),	/* Offset= -3710 (3414) */
/* 7126 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7125) */
/* 7128 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 7130 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7132) */
/* 7132 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7134 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 7136 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 7138 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7140 */	0x0 , 
			0x0,		/* 0 */
/* 7142 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7146 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7150 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7152) */
/* 7152 */	NdrFcShort( 0x4 ),	/* 4 */
/* 7154 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7156 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7160 */	NdrFcShort( 0xf15e ),	/* Offset= -3746 (3414) */
/* 7162 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7161) */
/* 7164 */	
			0x11, 0x0,	/* FC_RP */
/* 7166 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7168) */
/* 7168 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7170 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 7172 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 7174 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7176 */	0x0 , 
			0x0,		/* 0 */
/* 7178 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7182 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7186 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7188) */
/* 7188 */	NdrFcShort( 0x20 ),	/* 32 */
/* 7190 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7192 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7196 */	NdrFcShort( 0x4 ),	/* Offset= 4 (7200) */
/* 7198 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7197) */
/* 7200 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 7202 */	NdrFcShort( 0x20 ),	/* 32 */
/* 7204 */	NdrFcShort( 0x0 ),	/* 0 */
/* 7206 */	NdrFcShort( 0xa ),	/* Offset= 10 (7216) */
/* 7208 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 7210 */	0x0,		/* 0 */
			NdrFcShort( 0xe3e1 ),	/* Offset= -7199 (12) */
			0x40,		/* FC_STRUCTPAD4 */
/* 7214 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 7216 */	
			0x11, 0x0,	/* FC_RP */
/* 7218 */	NdrFcShort( 0xe47a ),	/* Offset= -7046 (172) */
/* 7220 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 7222 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7224) */
/* 7224 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7226 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 7228 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 7230 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7232 */	0x0 , 
			0x0,		/* 0 */
/* 7234 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7238 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7242 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7244) */
/* 7244 */	NdrFcShort( 0x4 ),	/* 4 */
/* 7246 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7248 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7252 */	NdrFcShort( 0xf102 ),	/* Offset= -3838 (3414) */
/* 7254 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7253) */
/* 7256 */	
			0x11, 0x0,	/* FC_RP */
/* 7258 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7260) */
/* 7260 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7262 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 7264 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 7266 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7268 */	0x0 , 
			0x0,		/* 0 */
/* 7270 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7274 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7278 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7280) */
/* 7280 */	NdrFcShort( 0x20 ),	/* 32 */
/* 7282 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7284 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7288 */	NdrFcShort( 0x4 ),	/* Offset= 4 (7292) */
/* 7290 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7289) */
/* 7292 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 7294 */	NdrFcShort( 0x20 ),	/* 32 */
/* 7296 */	NdrFcShort( 0x0 ),	/* 0 */
/* 7298 */	NdrFcShort( 0xa ),	/* Offset= 10 (7308) */
/* 7300 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 7302 */	0x0,		/* 0 */
			NdrFcShort( 0xe385 ),	/* Offset= -7291 (12) */
			0x40,		/* FC_STRUCTPAD4 */
/* 7306 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 7308 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 7310 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 7312 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 7314 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7316) */
/* 7316 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7318 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 7320 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 7322 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7324 */	0x0 , 
			0x0,		/* 0 */
/* 7326 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7330 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7334 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7336) */
/* 7336 */	NdrFcShort( 0xc ),	/* 12 */
/* 7338 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7340 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7344 */	NdrFcShort( 0x4 ),	/* Offset= 4 (7348) */
/* 7346 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7345) */
/* 7348 */	
			0x15,		/* FC_STRUCT */
			0x3,		/* 3 */
/* 7350 */	NdrFcShort( 0xc ),	/* 12 */
/* 7352 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 7354 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 7356 */	
			0x11, 0x0,	/* FC_RP */
/* 7358 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7360) */
/* 7360 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7362 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 7364 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 7366 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7368 */	0x0 , 
			0x0,		/* 0 */
/* 7370 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7374 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7378 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7380) */
/* 7380 */	NdrFcShort( 0x10 ),	/* 16 */
/* 7382 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7384 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7388 */	NdrFcShort( 0x4 ),	/* Offset= 4 (7392) */
/* 7390 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7389) */
/* 7392 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 7394 */	NdrFcShort( 0x10 ),	/* 16 */
/* 7396 */	NdrFcShort( 0x0 ),	/* 0 */
/* 7398 */	NdrFcShort( 0x6 ),	/* Offset= 6 (7404) */
/* 7400 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 7402 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 7404 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 7406 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 7408 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 7410 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 7412 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 7414 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7416) */
/* 7416 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7418 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 7420 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 7422 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7424 */	0x0 , 
			0x0,		/* 0 */
/* 7426 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7430 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7434 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7436) */
/* 7436 */	NdrFcShort( 0x20 ),	/* 32 */
/* 7438 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7440 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7444 */	NdrFcShort( 0x24 ),	/* Offset= 36 (7480) */
/* 7446 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7445) */
/* 7448 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 7450 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7454 */	NdrFcLong( 0x400 ),	/* 1024 */
/* 7458 */	
			0x1b,		/* FC_CARRAY */
			0x1,		/* 1 */
/* 7460 */	NdrFcShort( 0x2 ),	/* 2 */
/* 7462 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 7464 */	NdrFcShort( 0x10 ),	/* 16 */
/* 7466 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7468 */	0x0 , 
			0x0,		/* 0 */
/* 7470 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7474 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7478 */	0x5,		/* FC_WCHAR */
			0x5b,		/* FC_END */
/* 7480 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 7482 */	NdrFcShort( 0x20 ),	/* 32 */
/* 7484 */	NdrFcShort( 0x0 ),	/* 0 */
/* 7486 */	NdrFcShort( 0xc ),	/* Offset= 12 (7498) */
/* 7488 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 7490 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 7492 */	NdrFcShort( 0xffd4 ),	/* Offset= -44 (7448) */
/* 7494 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 7496 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 7498 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 7500 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 7502 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 7504 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 7506 */	
			0x12, 0x0,	/* FC_UP */
/* 7508 */	NdrFcShort( 0xffce ),	/* Offset= -50 (7458) */
/* 7510 */	
			0x11, 0x0,	/* FC_RP */
/* 7512 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7514) */
/* 7514 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7516 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 7518 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 7520 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7522 */	0x0 , 
			0x0,		/* 0 */
/* 7524 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7528 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7532 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7534) */
/* 7534 */	NdrFcShort( 0x18 ),	/* 24 */
/* 7536 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7538 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7542 */	NdrFcShort( 0xe ),	/* Offset= 14 (7556) */
/* 7544 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7543) */
/* 7546 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 7548 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7552 */	NdrFcLong( 0xffff ),	/* 65535 */
/* 7556 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 7558 */	NdrFcShort( 0x18 ),	/* 24 */
/* 7560 */	NdrFcShort( 0x0 ),	/* 0 */
/* 7562 */	NdrFcShort( 0xa ),	/* Offset= 10 (7572) */
/* 7564 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 7566 */	0x0,		/* 0 */
			NdrFcShort( 0xffeb ),	/* Offset= -21 (7546) */
			0x40,		/* FC_STRUCTPAD4 */
/* 7570 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 7572 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 7574 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 7576 */	
			0x12, 0x0,	/* FC_UP */
/* 7578 */	NdrFcShort( 0xed90 ),	/* Offset= -4720 (2858) */
/* 7580 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 7582 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7584) */
/* 7584 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7586 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 7588 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 7590 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7592 */	0x0 , 
			0x0,		/* 0 */
/* 7594 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7598 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7602 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7604) */
/* 7604 */	NdrFcShort( 0x4 ),	/* 4 */
/* 7606 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7608 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7612 */	NdrFcShort( 0xef9a ),	/* Offset= -4198 (3414) */
/* 7614 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7613) */
/* 7616 */	
			0x11, 0x0,	/* FC_RP */
/* 7618 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7620) */
/* 7620 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7622 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 7624 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 7626 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7628 */	0x0 , 
			0x0,		/* 0 */
/* 7630 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7634 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7638 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7640) */
/* 7640 */	NdrFcShort( 0x8 ),	/* 8 */
/* 7642 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7644 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7648 */	NdrFcShort( 0x4 ),	/* Offset= 4 (7652) */
/* 7650 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7649) */
/* 7652 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 7654 */	NdrFcShort( 0x8 ),	/* 8 */
/* 7656 */	NdrFcShort( 0x0 ),	/* 0 */
/* 7658 */	NdrFcShort( 0x4 ),	/* Offset= 4 (7662) */
/* 7660 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 7662 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 7664 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 7666 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 7668 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7670) */
/* 7670 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7672 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 7674 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 7676 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7678 */	0x0 , 
			0x0,		/* 0 */
/* 7680 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7684 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7688 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7690) */
/* 7690 */	NdrFcShort( 0x10 ),	/* 16 */
/* 7692 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7694 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7698 */	NdrFcShort( 0xe ),	/* Offset= 14 (7712) */
/* 7700 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7699) */
/* 7702 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 7704 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7708 */	NdrFcLong( 0xffff ),	/* 65535 */
/* 7712 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 7714 */	NdrFcShort( 0x10 ),	/* 16 */
/* 7716 */	NdrFcShort( 0x0 ),	/* 0 */
/* 7718 */	NdrFcShort( 0xa ),	/* Offset= 10 (7728) */
/* 7720 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 7722 */	0x0,		/* 0 */
			NdrFcShort( 0xffeb ),	/* Offset= -21 (7702) */
			0x36,		/* FC_POINTER */
/* 7726 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 7728 */	
			0x12, 0x0,	/* FC_UP */
/* 7730 */	NdrFcShort( 0xe6b4 ),	/* Offset= -6476 (1254) */
/* 7732 */	
			0x11, 0x0,	/* FC_RP */
/* 7734 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7736) */
/* 7736 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7738 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 7740 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 7742 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7744 */	0x0 , 
			0x0,		/* 0 */
/* 7746 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7750 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7754 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7756) */
/* 7756 */	NdrFcShort( 0x4 ),	/* 4 */
/* 7758 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7760 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7764 */	NdrFcShort( 0xef02 ),	/* Offset= -4350 (3414) */
/* 7766 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7765) */
/* 7768 */	
			0x11, 0x0,	/* FC_RP */
/* 7770 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7772) */
/* 7772 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7774 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 7776 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 7778 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7780 */	0x0 , 
			0x0,		/* 0 */
/* 7782 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7786 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7790 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7792) */
/* 7792 */	NdrFcShort( 0x40 ),	/* 64 */
/* 7794 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7796 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7800 */	NdrFcShort( 0x4e ),	/* Offset= 78 (7878) */
/* 7802 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7801) */
/* 7804 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 7806 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7810 */	NdrFcLong( 0x400 ),	/* 1024 */
/* 7814 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 7816 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7820 */	NdrFcLong( 0xa00000 ),	/* 10485760 */
/* 7824 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 7826 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7830 */	NdrFcLong( 0xa00000 ),	/* 10485760 */
/* 7834 */	
			0x1b,		/* FC_CARRAY */
			0x0,		/* 0 */
/* 7836 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7838 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 7840 */	NdrFcShort( 0x20 ),	/* 32 */
/* 7842 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7844 */	0x0 , 
			0x0,		/* 0 */
/* 7846 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7850 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7854 */	0x2,		/* FC_CHAR */
			0x5b,		/* FC_END */
/* 7856 */	
			0x1b,		/* FC_CARRAY */
			0x0,		/* 0 */
/* 7858 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7860 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 7862 */	NdrFcShort( 0x30 ),	/* 48 */
/* 7864 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7866 */	0x0 , 
			0x0,		/* 0 */
/* 7868 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7872 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7876 */	0x2,		/* FC_CHAR */
			0x5b,		/* FC_END */
/* 7878 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 7880 */	NdrFcShort( 0x40 ),	/* 64 */
/* 7882 */	NdrFcShort( 0x0 ),	/* 0 */
/* 7884 */	NdrFcShort( 0x18 ),	/* Offset= 24 (7908) */
/* 7886 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 7888 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 7890 */	0x0,		/* 0 */
			NdrFcShort( 0xffa9 ),	/* Offset= -87 (7804) */
			0x40,		/* FC_STRUCTPAD4 */
/* 7894 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 7896 */	0x0,		/* 0 */
			NdrFcShort( 0xffad ),	/* Offset= -83 (7814) */
			0x40,		/* FC_STRUCTPAD4 */
/* 7900 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 7902 */	0x0,		/* 0 */
			NdrFcShort( 0xffb1 ),	/* Offset= -79 (7824) */
			0x40,		/* FC_STRUCTPAD4 */
/* 7906 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 7908 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 7910 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 7912 */	
			0x12, 0x0,	/* FC_UP */
/* 7914 */	NdrFcShort( 0xf6ca ),	/* Offset= -2358 (5556) */
/* 7916 */	
			0x12, 0x0,	/* FC_UP */
/* 7918 */	NdrFcShort( 0xffac ),	/* Offset= -84 (7834) */
/* 7920 */	
			0x12, 0x0,	/* FC_UP */
/* 7922 */	NdrFcShort( 0xffbe ),	/* Offset= -66 (7856) */
/* 7924 */	
			0x11, 0x0,	/* FC_RP */
/* 7926 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7928) */
/* 7928 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7930 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 7932 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 7934 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7936 */	0x0 , 
			0x0,		/* 0 */
/* 7938 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7942 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7946 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7948) */
/* 7948 */	NdrFcShort( 0x10 ),	/* 16 */
/* 7950 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7952 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7956 */	NdrFcShort( 0xe ),	/* Offset= 14 (7970) */
/* 7958 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7957) */
/* 7960 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 7962 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7966 */	NdrFcLong( 0x400 ),	/* 1024 */
/* 7970 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 7972 */	NdrFcShort( 0x10 ),	/* 16 */
/* 7974 */	NdrFcShort( 0x0 ),	/* 0 */
/* 7976 */	NdrFcShort( 0xa ),	/* Offset= 10 (7986) */
/* 7978 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 7980 */	0x0,		/* 0 */
			NdrFcShort( 0xffeb ),	/* Offset= -21 (7960) */
			0x36,		/* FC_POINTER */
/* 7984 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 7986 */	
			0x12, 0x0,	/* FC_UP */
/* 7988 */	NdrFcShort( 0xe5b2 ),	/* Offset= -6734 (1254) */
/* 7990 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 7992 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7994) */
/* 7994 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7996 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 7998 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 8000 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 8002 */	0x0 , 
			0x0,		/* 0 */
/* 8004 */	NdrFcLong( 0x0 ),	/* 0 */
/* 8008 */	NdrFcLong( 0x0 ),	/* 0 */
/* 8012 */	NdrFcShort( 0x2 ),	/* Offset= 2 (8014) */
/* 8014 */	NdrFcShort( 0x10 ),	/* 16 */
/* 8016 */	NdrFcShort( 0x1 ),	/* 1 */
/* 8018 */	NdrFcLong( 0x1 ),	/* 1 */
/* 8022 */	NdrFcShort( 0x4 ),	/* Offset= 4 (8026) */
/* 8024 */	NdrFcShort( 0xffff ),	/* Offset= -1 (8023) */
/* 8026 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 8028 */	NdrFcShort( 0x10 ),	/* 16 */
/* 8030 */	NdrFcShort( 0x0 ),	/* 0 */
/* 8032 */	NdrFcShort( 0x6 ),	/* Offset= 6 (8038) */
/* 8034 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 8036 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 8038 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 8040 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */

			0x0
        }
    };

static const unsigned short drsuapi_FormatStringOffsetTable[] =
    {
    0,
    60,
    104,
    160,
    228,
    284,
    340,
    396,
    452,
    520,
    588,
    656,
    724,
    792,
    860,
    928,
    996,
    1064,
    1132,
    1188,
    1256,
    1324,
    1392,
    1448,
    1516,
    1584,
    1652,
    1720,
    1788,
    1856,
    1924
    };


static const MIDL_STUB_DESC drsuapi_StubDesc = 
    {
    (void *)& drsuapi___RpcClientInterface,
    MIDL_user_allocate,
    MIDL_user_free,
    &drsuapi__MIDL_AutoBindHandle,
    0,
    0,
    0,
    0,
    ms2Ddrsr__MIDL_TypeFormatString.Format,
    1, /* -error bounds_check flag */
    0x60001, /* Ndr library version */
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

static const unsigned short dsaop_FormatStringOffsetTable[] =
    {
    1992,
    2052
    };


static const MIDL_STUB_DESC dsaop_StubDesc = 
    {
    (void *)& dsaop___RpcClientInterface,
    MIDL_user_allocate,
    MIDL_user_free,
    &dsaop__MIDL_AutoBindHandle,
    0,
    0,
    0,
    0,
    ms2Ddrsr__MIDL_TypeFormatString.Format,
    1, /* -error bounds_check flag */
    0x60001, /* Ndr library version */
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

