

/* this ALWAYS GENERATED file contains the RPC client stubs */


 /* File created by MIDL compiler version 8.00.0595 */
/* at Tue May 15 11:07:02 2018
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

#define TYPE_FORMAT_STRING_SIZE   8275                              
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
/* 208 */	NdrFcShort( 0x124 ),	/* Type Offset=292 */

	/* Parameter pdwOutVersion */

/* 210 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 212 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 214 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 216 */	NdrFcShort( 0x113 ),	/* Flags:  must size, must free, out, simple ref, */
/* 218 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 220 */	NdrFcShort( 0x3c6 ),	/* Type Offset=966 */

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
/* 276 */	NdrFcShort( 0x736 ),	/* Type Offset=1846 */

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
/* 332 */	NdrFcShort( 0x79c ),	/* Type Offset=1948 */

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
/* 388 */	NdrFcShort( 0x842 ),	/* Type Offset=2114 */

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
/* 444 */	NdrFcShort( 0x87c ),	/* Type Offset=2172 */

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
/* 500 */	NdrFcShort( 0x8be ),	/* Type Offset=2238 */

	/* Parameter pdwOutVersion */

/* 502 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 504 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 506 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 508 */	NdrFcShort( 0x8113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=32 */
/* 510 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 512 */	NdrFcShort( 0x932 ),	/* Type Offset=2354 */

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
/* 568 */	NdrFcShort( 0x9a2 ),	/* Type Offset=2466 */

	/* Parameter pdwOutVersion */

/* 570 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 572 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 574 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 576 */	NdrFcShort( 0xa113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=40 */
/* 578 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 580 */	NdrFcShort( 0xa22 ),	/* Type Offset=2594 */

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
/* 636 */	NdrFcShort( 0xabc ),	/* Type Offset=2748 */

	/* Parameter pdwOutVersion */

/* 638 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 640 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 642 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 644 */	NdrFcShort( 0x8113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=32 */
/* 646 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 648 */	NdrFcShort( 0xb92 ),	/* Type Offset=2962 */

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
/* 704 */	NdrFcShort( 0xbe8 ),	/* Type Offset=3048 */

	/* Parameter pdwOutVersion */

/* 706 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 708 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 710 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 712 */	NdrFcShort( 0x113 ),	/* Flags:  must size, must free, out, simple ref, */
/* 714 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 716 */	NdrFcShort( 0xc42 ),	/* Type Offset=3138 */

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
/* 772 */	NdrFcShort( 0xcba ),	/* Type Offset=3258 */

	/* Parameter pdwOutVersion */

/* 774 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 776 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 778 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 780 */	NdrFcShort( 0x2113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=8 */
/* 782 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 784 */	NdrFcShort( 0xd2a ),	/* Type Offset=3370 */

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
/* 840 */	NdrFcShort( 0xdac ),	/* Type Offset=3500 */

	/* Parameter pdwOutVersion */

/* 842 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 844 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 846 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 848 */	NdrFcShort( 0x2113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=8 */
/* 850 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 852 */	NdrFcShort( 0xe1e ),	/* Type Offset=3614 */

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
/* 908 */	NdrFcShort( 0xe48 ),	/* Type Offset=3656 */

	/* Parameter pdwOutVersion */

/* 910 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 912 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 914 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 916 */	NdrFcShort( 0x2113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=8 */
/* 918 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 920 */	NdrFcShort( 0xe82 ),	/* Type Offset=3714 */

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
/* 976 */	NdrFcShort( 0xea6 ),	/* Type Offset=3750 */

	/* Parameter pdwOutVersion */

/* 978 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 980 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 982 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 984 */	NdrFcShort( 0x2113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=8 */
/* 986 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 988 */	NdrFcShort( 0xed8 ),	/* Type Offset=3800 */

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
/* 1044 */	NdrFcShort( 0xefc ),	/* Type Offset=3836 */

	/* Parameter pdwOutVersion */

/* 1046 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1048 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1050 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 1052 */	NdrFcShort( 0x4113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=16 */
/* 1054 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1056 */	NdrFcShort( 0xf30 ),	/* Type Offset=3888 */

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
/* 1112 */	NdrFcShort( 0x113e ),	/* Type Offset=4414 */

	/* Parameter pdwOutVersion */

/* 1114 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1116 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1118 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 1120 */	NdrFcShort( 0x113 ),	/* Flags:  must size, must free, out, simple ref, */
/* 1122 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1124 */	NdrFcShort( 0x11b2 ),	/* Type Offset=4530 */

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
/* 1180 */	NdrFcShort( 0x1408 ),	/* Type Offset=5128 */

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
/* 1236 */	NdrFcShort( 0x1434 ),	/* Type Offset=5172 */

	/* Parameter pdwOutVersion */

/* 1238 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1240 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1242 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 1244 */	NdrFcShort( 0x2113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=8 */
/* 1246 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1248 */	NdrFcShort( 0x1494 ),	/* Type Offset=5268 */

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
/* 1304 */	NdrFcShort( 0x1912 ),	/* Type Offset=6418 */

	/* Parameter pdwOutVersion */

/* 1306 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1308 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1310 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 1312 */	NdrFcShort( 0x2113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=8 */
/* 1314 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1316 */	NdrFcShort( 0x19d8 ),	/* Type Offset=6616 */

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
/* 1372 */	NdrFcShort( 0x19fc ),	/* Type Offset=6652 */

	/* Parameter pdwOutVersion */

/* 1374 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1376 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1378 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 1380 */	NdrFcShort( 0x4113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=16 */
/* 1382 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1384 */	NdrFcShort( 0x1a68 ),	/* Type Offset=6760 */

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
/* 1440 */	NdrFcShort( 0x1ad4 ),	/* Type Offset=6868 */

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
/* 1496 */	NdrFcShort( 0x1b0c ),	/* Type Offset=6924 */

	/* Parameter pdwOutVersion */

/* 1498 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1500 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1502 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 1504 */	NdrFcShort( 0x4113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=16 */
/* 1506 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1508 */	NdrFcShort( 0x1b54 ),	/* Type Offset=6996 */

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
/* 1564 */	NdrFcShort( 0x1bc0 ),	/* Type Offset=7104 */

	/* Parameter pdwOutVersion */

/* 1566 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1568 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1570 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 1572 */	NdrFcShort( 0x6113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=24 */
/* 1574 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1576 */	NdrFcShort( 0x1c32 ),	/* Type Offset=7218 */

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
/* 1632 */	NdrFcShort( 0x1ca0 ),	/* Type Offset=7328 */

	/* Parameter pdwOutVersion */

/* 1634 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1636 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1638 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 1640 */	NdrFcShort( 0x2113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=8 */
/* 1642 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1644 */	NdrFcShort( 0x1cc4 ),	/* Type Offset=7364 */

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
/* 1700 */	NdrFcShort( 0x1ce8 ),	/* Type Offset=7400 */

	/* Parameter pdwOutVersion */

/* 1702 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1704 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1706 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 1708 */	NdrFcShort( 0x2113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=8 */
/* 1710 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1712 */	NdrFcShort( 0x1d20 ),	/* Type Offset=7456 */

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
/* 1768 */	NdrFcShort( 0x1d44 ),	/* Type Offset=7492 */

	/* Parameter pdwOutVersion */

/* 1770 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1772 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1774 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 1776 */	NdrFcShort( 0x4113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=16 */
/* 1778 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1780 */	NdrFcShort( 0x1d7c ),	/* Type Offset=7548 */

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
/* 1836 */	NdrFcShort( 0x1da8 ),	/* Type Offset=7592 */

	/* Parameter pdwOutVersion */

/* 1838 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1840 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1842 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 1844 */	NdrFcShort( 0x8113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=32 */
/* 1846 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1848 */	NdrFcShort( 0x1de0 ),	/* Type Offset=7648 */

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
/* 1904 */	NdrFcShort( 0x1e42 ),	/* Type Offset=7746 */

	/* Parameter pdwOutVersion */

/* 1906 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1908 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1910 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 1912 */	NdrFcShort( 0x2113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=8 */
/* 1914 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1916 */	NdrFcShort( 0x1e88 ),	/* Type Offset=7816 */

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
/* 1972 */	NdrFcShort( 0x1eac ),	/* Type Offset=7852 */

	/* Parameter pdwOutVersion */

/* 1974 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1976 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 1978 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pmsgOut */

/* 1980 */	NdrFcShort( 0x4113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=16 */
/* 1982 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 1984 */	NdrFcShort( 0x1ede ),	/* Type Offset=7902 */

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
/* 2032 */	NdrFcShort( 0x1f20 ),	/* Type Offset=7968 */

	/* Parameter pmsgIn */

/* 2034 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 2036 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 2038 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pdwOutVersion */

/* 2040 */	NdrFcShort( 0x113 ),	/* Flags:  must size, must free, out, simple ref, */
/* 2042 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 2044 */	NdrFcShort( 0x1f44 ),	/* Type Offset=8004 */

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
/* 2092 */	NdrFcShort( 0x1fe0 ),	/* Type Offset=8160 */

	/* Parameter pmsgIn */

/* 2094 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 2096 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 2098 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Parameter pdwOutVersion */

/* 2100 */	NdrFcShort( 0x4113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=16 */
/* 2102 */	NdrFcShort( 0x20 ),	/* X64 Stack size/offset = 32 */
/* 2104 */	NdrFcShort( 0x2022 ),	/* Type Offset=8226 */

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
/* 122 */	NdrFcShort( 0x40 ),	/* 64 */
/* 124 */	NdrFcShort( 0x2 ),	/* 2 */
/* 126 */	NdrFcLong( 0x1 ),	/* 1 */
/* 130 */	NdrFcShort( 0x42 ),	/* Offset= 66 (196) */
/* 132 */	NdrFcLong( 0x2 ),	/* 2 */
/* 136 */	NdrFcShort( 0x76 ),	/* Offset= 118 (254) */
/* 138 */	NdrFcShort( 0xffff ),	/* Offset= -1 (137) */
/* 140 */	
			0x1d,		/* FC_SMFARRAY */
			0x0,		/* 0 */
/* 142 */	NdrFcShort( 0x1c ),	/* 28 */
/* 144 */	0x2,		/* FC_CHAR */
			0x5b,		/* FC_END */
/* 146 */	
			0x15,		/* FC_STRUCT */
			0x0,		/* 0 */
/* 148 */	NdrFcShort( 0x1c ),	/* 28 */
/* 150 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 152 */	NdrFcShort( 0xfff4 ),	/* Offset= -12 (140) */
/* 154 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 156 */	
			0x1b,		/* FC_CARRAY */
			0x1,		/* 1 */
/* 158 */	NdrFcShort( 0x2 ),	/* 2 */
/* 160 */	0x9,		/* Corr desc: FC_ULONG */
			0x57,		/* FC_ADD_1 */
/* 162 */	NdrFcShort( 0xfffc ),	/* -4 */
/* 164 */	NdrFcShort( 0x11 ),	/* Corr flags:  early, */
/* 166 */	0x1 , /* correlation range */
			0x0,		/* 0 */
/* 168 */	NdrFcLong( 0x0 ),	/* 0 */
/* 172 */	NdrFcLong( 0xa00001 ),	/* 10485761 */
/* 176 */	0x5,		/* FC_WCHAR */
			0x5b,		/* FC_END */
/* 178 */	
			0x17,		/* FC_CSTRUCT */
			0x3,		/* 3 */
/* 180 */	NdrFcShort( 0x38 ),	/* 56 */
/* 182 */	NdrFcShort( 0xffe6 ),	/* Offset= -26 (156) */
/* 184 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 186 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 188 */	NdrFcShort( 0xff50 ),	/* Offset= -176 (12) */
/* 190 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 192 */	NdrFcShort( 0xffd2 ),	/* Offset= -46 (146) */
/* 194 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 196 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 198 */	NdrFcShort( 0x28 ),	/* 40 */
/* 200 */	NdrFcShort( 0x0 ),	/* 0 */
/* 202 */	NdrFcShort( 0xc ),	/* Offset= 12 (214) */
/* 204 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 206 */	0x0,		/* 0 */
			NdrFcShort( 0xff3d ),	/* Offset= -195 (12) */
			0x36,		/* FC_POINTER */
/* 210 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 212 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 214 */	
			0x11, 0x0,	/* FC_RP */
/* 216 */	NdrFcShort( 0xffda ),	/* Offset= -38 (178) */
/* 218 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 220 */	
			0x22,		/* FC_C_CSTRING */
			0x5c,		/* FC_PAD */
/* 222 */	
			0x1b,		/* FC_CARRAY */
			0x0,		/* 0 */
/* 224 */	NdrFcShort( 0x1 ),	/* 1 */
/* 226 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 228 */	NdrFcShort( 0xfff4 ),	/* -12 */
/* 230 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 232 */	0x0 , 
			0x0,		/* 0 */
/* 234 */	NdrFcLong( 0x0 ),	/* 0 */
/* 238 */	NdrFcLong( 0x0 ),	/* 0 */
/* 242 */	0x2,		/* FC_CHAR */
			0x5b,		/* FC_END */
/* 244 */	
			0x17,		/* FC_CSTRUCT */
			0x7,		/* 7 */
/* 246 */	NdrFcShort( 0x10 ),	/* 16 */
/* 248 */	NdrFcShort( 0xffe6 ),	/* Offset= -26 (222) */
/* 250 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 252 */	0xb,		/* FC_HYPER */
			0x5b,		/* FC_END */
/* 254 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 256 */	NdrFcShort( 0x40 ),	/* 64 */
/* 258 */	NdrFcShort( 0x0 ),	/* 0 */
/* 260 */	NdrFcShort( 0x10 ),	/* Offset= 16 (276) */
/* 262 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 264 */	0x0,		/* 0 */
			NdrFcShort( 0xff03 ),	/* Offset= -253 (12) */
			0x36,		/* FC_POINTER */
/* 268 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 270 */	0x0,		/* 0 */
			NdrFcShort( 0xfefd ),	/* Offset= -259 (12) */
			0x40,		/* FC_STRUCTPAD4 */
/* 274 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 276 */	
			0x11, 0x0,	/* FC_RP */
/* 278 */	NdrFcShort( 0xff9c ),	/* Offset= -100 (178) */
/* 280 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 282 */	
			0x22,		/* FC_C_CSTRING */
			0x5c,		/* FC_PAD */
/* 284 */	
			0x12, 0x0,	/* FC_UP */
/* 286 */	NdrFcShort( 0xffd6 ),	/* Offset= -42 (244) */
/* 288 */	
			0x11, 0x0,	/* FC_RP */
/* 290 */	NdrFcShort( 0x2 ),	/* Offset= 2 (292) */
/* 292 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 294 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 296 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 298 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 300 */	0x0 , 
			0x0,		/* 0 */
/* 302 */	NdrFcLong( 0x0 ),	/* 0 */
/* 306 */	NdrFcLong( 0x0 ),	/* 0 */
/* 310 */	NdrFcShort( 0x2 ),	/* Offset= 2 (312) */
/* 312 */	NdrFcShort( 0xa8 ),	/* 168 */
/* 314 */	NdrFcShort( 0x6 ),	/* 6 */
/* 316 */	NdrFcLong( 0x4 ),	/* 4 */
/* 320 */	NdrFcShort( 0x168 ),	/* Offset= 360 (680) */
/* 322 */	NdrFcLong( 0x5 ),	/* 5 */
/* 326 */	NdrFcShort( 0x17e ),	/* Offset= 382 (708) */
/* 328 */	NdrFcLong( 0x7 ),	/* 7 */
/* 332 */	NdrFcShort( 0x1a0 ),	/* Offset= 416 (748) */
/* 334 */	NdrFcLong( 0x8 ),	/* 8 */
/* 338 */	NdrFcShort( 0x1be ),	/* Offset= 446 (784) */
/* 340 */	NdrFcLong( 0xa ),	/* 10 */
/* 344 */	NdrFcShort( 0x1ee ),	/* Offset= 494 (838) */
/* 346 */	NdrFcLong( 0xb ),	/* 11 */
/* 350 */	NdrFcShort( 0x220 ),	/* Offset= 544 (894) */
/* 352 */	NdrFcShort( 0xffff ),	/* Offset= -1 (351) */
/* 354 */	
			0x15,		/* FC_STRUCT */
			0x7,		/* 7 */
/* 356 */	NdrFcShort( 0x18 ),	/* 24 */
/* 358 */	0xb,		/* FC_HYPER */
			0xb,		/* FC_HYPER */
/* 360 */	0xb,		/* FC_HYPER */
			0x5b,		/* FC_END */
/* 362 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 364 */	NdrFcLong( 0x0 ),	/* 0 */
/* 368 */	NdrFcLong( 0x100000 ),	/* 1048576 */
/* 372 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 374 */	NdrFcLong( 0x0 ),	/* 0 */
/* 378 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 382 */	
			0x1b,		/* FC_CARRAY */
			0x0,		/* 0 */
/* 384 */	NdrFcShort( 0x1 ),	/* 1 */
/* 386 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 388 */	NdrFcShort( 0x0 ),	/* 0 */
/* 390 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 392 */	0x0 , 
			0x0,		/* 0 */
/* 394 */	NdrFcLong( 0x0 ),	/* 0 */
/* 398 */	NdrFcLong( 0x0 ),	/* 0 */
/* 402 */	0x2,		/* FC_CHAR */
			0x5b,		/* FC_END */
/* 404 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 406 */	NdrFcShort( 0x10 ),	/* 16 */
/* 408 */	NdrFcShort( 0x0 ),	/* 0 */
/* 410 */	NdrFcShort( 0xa ),	/* Offset= 10 (420) */
/* 412 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 414 */	NdrFcShort( 0xffd6 ),	/* Offset= -42 (372) */
/* 416 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 418 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 420 */	
			0x12, 0x0,	/* FC_UP */
/* 422 */	NdrFcShort( 0xffd8 ),	/* Offset= -40 (382) */
/* 424 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 426 */	NdrFcShort( 0x18 ),	/* 24 */
/* 428 */	NdrFcShort( 0x0 ),	/* 0 */
/* 430 */	NdrFcShort( 0x0 ),	/* Offset= 0 (430) */
/* 432 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 434 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 436 */	NdrFcShort( 0xffe0 ),	/* Offset= -32 (404) */
/* 438 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 440 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 442 */	NdrFcShort( 0x0 ),	/* 0 */
/* 444 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 446 */	NdrFcShort( 0x0 ),	/* 0 */
/* 448 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 450 */	0x0 , 
			0x0,		/* 0 */
/* 452 */	NdrFcLong( 0x0 ),	/* 0 */
/* 456 */	NdrFcLong( 0x0 ),	/* 0 */
/* 460 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 464 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 466 */	0x0 , 
			0x0,		/* 0 */
/* 468 */	NdrFcLong( 0x0 ),	/* 0 */
/* 472 */	NdrFcLong( 0x0 ),	/* 0 */
/* 476 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 478 */	NdrFcShort( 0xffca ),	/* Offset= -54 (424) */
/* 480 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 482 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 484 */	NdrFcShort( 0x10 ),	/* 16 */
/* 486 */	NdrFcShort( 0x0 ),	/* 0 */
/* 488 */	NdrFcShort( 0xa ),	/* Offset= 10 (498) */
/* 490 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 492 */	NdrFcShort( 0xff7e ),	/* Offset= -130 (362) */
/* 494 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 496 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 498 */	
			0x12, 0x0,	/* FC_UP */
/* 500 */	NdrFcShort( 0xffc4 ),	/* Offset= -60 (440) */
/* 502 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 504 */	NdrFcLong( 0x0 ),	/* 0 */
/* 508 */	NdrFcLong( 0x100000 ),	/* 1048576 */
/* 512 */	
			0x15,		/* FC_STRUCT */
			0x7,		/* 7 */
/* 514 */	NdrFcShort( 0x18 ),	/* 24 */
/* 516 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 518 */	NdrFcShort( 0xfe06 ),	/* Offset= -506 (12) */
/* 520 */	0xb,		/* FC_HYPER */
			0x5b,		/* FC_END */
/* 522 */	
			0x1b,		/* FC_CARRAY */
			0x7,		/* 7 */
/* 524 */	NdrFcShort( 0x18 ),	/* 24 */
/* 526 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 528 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 530 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 532 */	0x0 , 
			0x0,		/* 0 */
/* 534 */	NdrFcLong( 0x0 ),	/* 0 */
/* 538 */	NdrFcLong( 0x0 ),	/* 0 */
/* 542 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 544 */	NdrFcShort( 0xffe0 ),	/* Offset= -32 (512) */
/* 546 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 548 */	0xb1,		/* FC_FORCED_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 550 */	NdrFcShort( 0x10 ),	/* 16 */
/* 552 */	NdrFcShort( 0xffe2 ),	/* Offset= -30 (522) */
/* 554 */	NdrFcShort( 0x0 ),	/* Offset= 0 (554) */
/* 556 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 558 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 560 */	NdrFcShort( 0xffc6 ),	/* Offset= -58 (502) */
/* 562 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 564 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 566 */	NdrFcLong( 0x1 ),	/* 1 */
/* 570 */	NdrFcLong( 0x100000 ),	/* 1048576 */
/* 574 */	
			0x1b,		/* FC_CARRAY */
			0x3,		/* 3 */
/* 576 */	NdrFcShort( 0x4 ),	/* 4 */
/* 578 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 580 */	NdrFcShort( 0xfffc ),	/* -4 */
/* 582 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 584 */	0x0 , 
			0x0,		/* 0 */
/* 586 */	NdrFcLong( 0x0 ),	/* 0 */
/* 590 */	NdrFcLong( 0x0 ),	/* 0 */
/* 594 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 596 */	0xb1,		/* FC_FORCED_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 598 */	NdrFcShort( 0xc ),	/* 12 */
/* 600 */	NdrFcShort( 0xffe6 ),	/* Offset= -26 (574) */
/* 602 */	NdrFcShort( 0x0 ),	/* Offset= 0 (602) */
/* 604 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 606 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 608 */	NdrFcShort( 0xffd4 ),	/* Offset= -44 (564) */
/* 610 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 612 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 614 */	NdrFcShort( 0x70 ),	/* 112 */
/* 616 */	NdrFcShort( 0x0 ),	/* 0 */
/* 618 */	NdrFcShort( 0x1a ),	/* Offset= 26 (644) */
/* 620 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 622 */	NdrFcShort( 0xfd9e ),	/* Offset= -610 (12) */
/* 624 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 626 */	NdrFcShort( 0xfd9a ),	/* Offset= -614 (12) */
/* 628 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 630 */	0x0,		/* 0 */
			NdrFcShort( 0xfeeb ),	/* Offset= -277 (354) */
			0x36,		/* FC_POINTER */
/* 634 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 636 */	0x0,		/* 0 */
			NdrFcShort( 0xff65 ),	/* Offset= -155 (482) */
			0x8,		/* FC_LONG */
/* 640 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 642 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 644 */	
			0x11, 0x0,	/* FC_RP */
/* 646 */	NdrFcShort( 0xfe2c ),	/* Offset= -468 (178) */
/* 648 */	
			0x12, 0x0,	/* FC_UP */
/* 650 */	NdrFcShort( 0xff9a ),	/* Offset= -102 (548) */
/* 652 */	
			0x12, 0x0,	/* FC_UP */
/* 654 */	NdrFcShort( 0xffc6 ),	/* Offset= -58 (596) */
/* 656 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 658 */	NdrFcLong( 0x1 ),	/* 1 */
/* 662 */	NdrFcLong( 0x100 ),	/* 256 */
/* 666 */	0xb1,		/* FC_FORCED_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 668 */	NdrFcShort( 0x4 ),	/* 4 */
/* 670 */	NdrFcShort( 0xfd88 ),	/* Offset= -632 (38) */
/* 672 */	NdrFcShort( 0x0 ),	/* Offset= 0 (672) */
/* 674 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 676 */	NdrFcShort( 0xffec ),	/* Offset= -20 (656) */
/* 678 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 680 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 682 */	NdrFcShort( 0x88 ),	/* 136 */
/* 684 */	NdrFcShort( 0x0 ),	/* 0 */
/* 686 */	NdrFcShort( 0xc ),	/* Offset= 12 (698) */
/* 688 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 690 */	NdrFcShort( 0xfd5a ),	/* Offset= -678 (12) */
/* 692 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 694 */	0x0,		/* 0 */
			NdrFcShort( 0xffad ),	/* Offset= -83 (612) */
			0x5b,		/* FC_END */
/* 698 */	
			0x11, 0x0,	/* FC_RP */
/* 700 */	NdrFcShort( 0xffde ),	/* Offset= -34 (666) */
/* 702 */	
			0x15,		/* FC_STRUCT */
			0x7,		/* 7 */
/* 704 */	NdrFcShort( 0x8 ),	/* 8 */
/* 706 */	0xb,		/* FC_HYPER */
			0x5b,		/* FC_END */
/* 708 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 710 */	NdrFcShort( 0x60 ),	/* 96 */
/* 712 */	NdrFcShort( 0x0 ),	/* 0 */
/* 714 */	NdrFcShort( 0x1a ),	/* Offset= 26 (740) */
/* 716 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 718 */	NdrFcShort( 0xfd3e ),	/* Offset= -706 (12) */
/* 720 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 722 */	NdrFcShort( 0xfd3a ),	/* Offset= -710 (12) */
/* 724 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 726 */	0x0,		/* 0 */
			NdrFcShort( 0xfe8b ),	/* Offset= -373 (354) */
			0x36,		/* FC_POINTER */
/* 730 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 732 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 734 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 736 */	NdrFcShort( 0xffde ),	/* Offset= -34 (702) */
/* 738 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 740 */	
			0x11, 0x0,	/* FC_RP */
/* 742 */	NdrFcShort( 0xfdcc ),	/* Offset= -564 (178) */
/* 744 */	
			0x12, 0x0,	/* FC_UP */
/* 746 */	NdrFcShort( 0xff3a ),	/* Offset= -198 (548) */
/* 748 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 750 */	NdrFcShort( 0xa8 ),	/* 168 */
/* 752 */	NdrFcShort( 0x0 ),	/* 0 */
/* 754 */	NdrFcShort( 0x12 ),	/* Offset= 18 (772) */
/* 756 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 758 */	NdrFcShort( 0xfd16 ),	/* Offset= -746 (12) */
/* 760 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 762 */	0x0,		/* 0 */
			NdrFcShort( 0xff69 ),	/* Offset= -151 (612) */
			0x36,		/* FC_POINTER */
/* 766 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 768 */	0x0,		/* 0 */
			NdrFcShort( 0xfee1 ),	/* Offset= -287 (482) */
			0x5b,		/* FC_END */
/* 772 */	
			0x11, 0x0,	/* FC_RP */
/* 774 */	NdrFcShort( 0xff94 ),	/* Offset= -108 (666) */
/* 776 */	
			0x12, 0x0,	/* FC_UP */
/* 778 */	NdrFcShort( 0xff4a ),	/* Offset= -182 (596) */
/* 780 */	
			0x12, 0x0,	/* FC_UP */
/* 782 */	NdrFcShort( 0xff46 ),	/* Offset= -186 (596) */
/* 784 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 786 */	NdrFcShort( 0x80 ),	/* 128 */
/* 788 */	NdrFcShort( 0x0 ),	/* 0 */
/* 790 */	NdrFcShort( 0x20 ),	/* Offset= 32 (822) */
/* 792 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 794 */	NdrFcShort( 0xfcf2 ),	/* Offset= -782 (12) */
/* 796 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 798 */	NdrFcShort( 0xfcee ),	/* Offset= -786 (12) */
/* 800 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 802 */	0x0,		/* 0 */
			NdrFcShort( 0xfe3f ),	/* Offset= -449 (354) */
			0x36,		/* FC_POINTER */
/* 806 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 808 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 810 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 812 */	NdrFcShort( 0xff92 ),	/* Offset= -110 (702) */
/* 814 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 816 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 818 */	NdrFcShort( 0xfeb0 ),	/* Offset= -336 (482) */
/* 820 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 822 */	
			0x11, 0x0,	/* FC_RP */
/* 824 */	NdrFcShort( 0xfd7a ),	/* Offset= -646 (178) */
/* 826 */	
			0x12, 0x0,	/* FC_UP */
/* 828 */	NdrFcShort( 0xfee8 ),	/* Offset= -280 (548) */
/* 830 */	
			0x12, 0x0,	/* FC_UP */
/* 832 */	NdrFcShort( 0xff14 ),	/* Offset= -236 (596) */
/* 834 */	
			0x12, 0x0,	/* FC_UP */
/* 836 */	NdrFcShort( 0xff10 ),	/* Offset= -240 (596) */
/* 838 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 840 */	NdrFcShort( 0x88 ),	/* 136 */
/* 842 */	NdrFcShort( 0x0 ),	/* 0 */
/* 844 */	NdrFcShort( 0x22 ),	/* Offset= 34 (878) */
/* 846 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 848 */	NdrFcShort( 0xfcbc ),	/* Offset= -836 (12) */
/* 850 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 852 */	NdrFcShort( 0xfcb8 ),	/* Offset= -840 (12) */
/* 854 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 856 */	0x0,		/* 0 */
			NdrFcShort( 0xfe09 ),	/* Offset= -503 (354) */
			0x36,		/* FC_POINTER */
/* 860 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 862 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 864 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 866 */	NdrFcShort( 0xff5c ),	/* Offset= -164 (702) */
/* 868 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 870 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 872 */	NdrFcShort( 0xfe7a ),	/* Offset= -390 (482) */
/* 874 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 876 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 878 */	
			0x11, 0x0,	/* FC_RP */
/* 880 */	NdrFcShort( 0xfd42 ),	/* Offset= -702 (178) */
/* 882 */	
			0x12, 0x0,	/* FC_UP */
/* 884 */	NdrFcShort( 0xfeb0 ),	/* Offset= -336 (548) */
/* 886 */	
			0x12, 0x0,	/* FC_UP */
/* 888 */	NdrFcShort( 0xfedc ),	/* Offset= -292 (596) */
/* 890 */	
			0x12, 0x0,	/* FC_UP */
/* 892 */	NdrFcShort( 0xfed8 ),	/* Offset= -296 (596) */
/* 894 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 896 */	NdrFcShort( 0xa0 ),	/* 160 */
/* 898 */	NdrFcShort( 0x0 ),	/* 0 */
/* 900 */	NdrFcShort( 0x26 ),	/* Offset= 38 (938) */
/* 902 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 904 */	NdrFcShort( 0xfc84 ),	/* Offset= -892 (12) */
/* 906 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 908 */	NdrFcShort( 0xfc80 ),	/* Offset= -896 (12) */
/* 910 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 912 */	0x0,		/* 0 */
			NdrFcShort( 0xfdd1 ),	/* Offset= -559 (354) */
			0x36,		/* FC_POINTER */
/* 916 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 918 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 920 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 922 */	NdrFcShort( 0xff24 ),	/* Offset= -220 (702) */
/* 924 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 926 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 928 */	NdrFcShort( 0xfe42 ),	/* Offset= -446 (482) */
/* 930 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 932 */	0x0,		/* 0 */
			NdrFcShort( 0xfc67 ),	/* Offset= -921 (12) */
			0x40,		/* FC_STRUCTPAD4 */
/* 936 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 938 */	
			0x11, 0x0,	/* FC_RP */
/* 940 */	NdrFcShort( 0xfd06 ),	/* Offset= -762 (178) */
/* 942 */	
			0x12, 0x0,	/* FC_UP */
/* 944 */	NdrFcShort( 0xfe74 ),	/* Offset= -396 (548) */
/* 946 */	
			0x12, 0x0,	/* FC_UP */
/* 948 */	NdrFcShort( 0xfea0 ),	/* Offset= -352 (596) */
/* 950 */	
			0x12, 0x0,	/* FC_UP */
/* 952 */	NdrFcShort( 0xfe9c ),	/* Offset= -356 (596) */
/* 954 */	
			0x12, 0x0,	/* FC_UP */
/* 956 */	NdrFcShort( 0xfd38 ),	/* Offset= -712 (244) */
/* 958 */	
			0x11, 0xc,	/* FC_RP [alloced_on_stack] [simple_pointer] */
/* 960 */	0x8,		/* FC_LONG */
			0x5c,		/* FC_PAD */
/* 962 */	
			0x11, 0x0,	/* FC_RP */
/* 964 */	NdrFcShort( 0x2 ),	/* Offset= 2 (966) */
/* 966 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 968 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 970 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 972 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 974 */	0x0 , 
			0x0,		/* 0 */
/* 976 */	NdrFcLong( 0x0 ),	/* 0 */
/* 980 */	NdrFcLong( 0x0 ),	/* 0 */
/* 984 */	NdrFcShort( 0x2 ),	/* Offset= 2 (986) */
/* 986 */	NdrFcShort( 0xa8 ),	/* 168 */
/* 988 */	NdrFcShort( 0x5 ),	/* 5 */
/* 990 */	NdrFcLong( 0x1 ),	/* 1 */
/* 994 */	NdrFcShort( 0x160 ),	/* Offset= 352 (1346) */
/* 996 */	NdrFcLong( 0x2 ),	/* 2 */
/* 1000 */	NdrFcShort( 0x1b2 ),	/* Offset= 434 (1434) */
/* 1002 */	NdrFcLong( 0x6 ),	/* 6 */
/* 1006 */	NdrFcShort( 0x256 ),	/* Offset= 598 (1604) */
/* 1008 */	NdrFcLong( 0x7 ),	/* 7 */
/* 1012 */	NdrFcShort( 0x28e ),	/* Offset= 654 (1666) */
/* 1014 */	NdrFcLong( 0x9 ),	/* 9 */
/* 1018 */	NdrFcShort( 0x2fa ),	/* Offset= 762 (1780) */
/* 1020 */	NdrFcShort( 0xffff ),	/* Offset= -1 (1019) */
/* 1022 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 1024 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1028 */	NdrFcLong( 0x100000 ),	/* 1048576 */
/* 1032 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 1034 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1038 */	NdrFcLong( 0xa00000 ),	/* 10485760 */
/* 1042 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 1044 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1048 */	NdrFcLong( 0x1900000 ),	/* 26214400 */
/* 1052 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 1054 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1056 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1058 */	NdrFcShort( 0xa ),	/* Offset= 10 (1068) */
/* 1060 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1062 */	NdrFcShort( 0xffec ),	/* Offset= -20 (1042) */
/* 1064 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 1066 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1068 */	
			0x12, 0x0,	/* FC_UP */
/* 1070 */	NdrFcShort( 0xfd50 ),	/* Offset= -688 (382) */
/* 1072 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 1074 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1076 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 1078 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1080 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1082 */	0x0 , 
			0x0,		/* 0 */
/* 1084 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1088 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1092 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 1096 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 1098 */	0x0 , 
			0x0,		/* 0 */
/* 1100 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1104 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1108 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1110 */	NdrFcShort( 0xffc6 ),	/* Offset= -58 (1052) */
/* 1112 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1114 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 1116 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1118 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1120 */	NdrFcShort( 0xa ),	/* Offset= 10 (1130) */
/* 1122 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1124 */	NdrFcShort( 0xffa4 ),	/* Offset= -92 (1032) */
/* 1126 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 1128 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1130 */	
			0x12, 0x0,	/* FC_UP */
/* 1132 */	NdrFcShort( 0xffc4 ),	/* Offset= -60 (1072) */
/* 1134 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 1136 */	NdrFcShort( 0x18 ),	/* 24 */
/* 1138 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1140 */	NdrFcShort( 0x0 ),	/* Offset= 0 (1140) */
/* 1142 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 1144 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1146 */	NdrFcShort( 0xffe0 ),	/* Offset= -32 (1114) */
/* 1148 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1150 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 1152 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1154 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 1156 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1158 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1160 */	0x0 , 
			0x0,		/* 0 */
/* 1162 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1166 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1170 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 1174 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 1176 */	0x0 , 
			0x0,		/* 0 */
/* 1178 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1182 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1186 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1188 */	NdrFcShort( 0xffca ),	/* Offset= -54 (1134) */
/* 1190 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1192 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 1194 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1196 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1198 */	NdrFcShort( 0xa ),	/* Offset= 10 (1208) */
/* 1200 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1202 */	NdrFcShort( 0xff4c ),	/* Offset= -180 (1022) */
/* 1204 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 1206 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1208 */	
			0x12, 0x0,	/* FC_UP */
/* 1210 */	NdrFcShort( 0xffc4 ),	/* Offset= -60 (1150) */
/* 1212 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 1214 */	NdrFcShort( 0x20 ),	/* 32 */
/* 1216 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1218 */	NdrFcShort( 0xa ),	/* Offset= 10 (1228) */
/* 1220 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 1222 */	0x40,		/* FC_STRUCTPAD4 */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1224 */	0x0,		/* 0 */
			NdrFcShort( 0xffdf ),	/* Offset= -33 (1192) */
			0x5b,		/* FC_END */
/* 1228 */	
			0x12, 0x0,	/* FC_UP */
/* 1230 */	NdrFcShort( 0xfbe4 ),	/* Offset= -1052 (178) */
/* 1232 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 1234 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1238 */	NdrFcLong( 0x100000 ),	/* 1048576 */
/* 1242 */	0xb1,		/* FC_FORCED_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 1244 */	NdrFcShort( 0x28 ),	/* 40 */
/* 1246 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1248 */	NdrFcShort( 0x0 ),	/* Offset= 0 (1248) */
/* 1250 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 1252 */	0xb,		/* FC_HYPER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1254 */	0x0,		/* 0 */
			NdrFcShort( 0xfb25 ),	/* Offset= -1243 (12) */
			0xb,		/* FC_HYPER */
/* 1258 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1260 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x7,		/* 7 */
/* 1262 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1264 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 1266 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 1268 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1270 */	0x0 , 
			0x0,		/* 0 */
/* 1272 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1276 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1280 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 1284 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 1286 */	0x0 , 
			0x0,		/* 0 */
/* 1288 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1292 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1296 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1298 */	NdrFcShort( 0xffc8 ),	/* Offset= -56 (1242) */
/* 1300 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1302 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 1304 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1306 */	NdrFcShort( 0xffd2 ),	/* Offset= -46 (1260) */
/* 1308 */	NdrFcShort( 0x0 ),	/* Offset= 0 (1308) */
/* 1310 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1312 */	NdrFcShort( 0xffb0 ),	/* Offset= -80 (1232) */
/* 1314 */	0x40,		/* FC_STRUCTPAD4 */
			0x5b,		/* FC_END */
/* 1316 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 1318 */	NdrFcShort( 0x40 ),	/* 64 */
/* 1320 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1322 */	NdrFcShort( 0xc ),	/* Offset= 12 (1334) */
/* 1324 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1326 */	0x0,		/* 0 */
			NdrFcShort( 0xff8d ),	/* Offset= -115 (1212) */
			0x8,		/* FC_LONG */
/* 1330 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 1332 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 1334 */	
			0x12, 0x0,	/* FC_UP */
/* 1336 */	NdrFcShort( 0xffec ),	/* Offset= -20 (1316) */
/* 1338 */	
			0x12, 0x0,	/* FC_UP */
/* 1340 */	NdrFcShort( 0xfad0 ),	/* Offset= -1328 (12) */
/* 1342 */	
			0x12, 0x0,	/* FC_UP */
/* 1344 */	NdrFcShort( 0xffd6 ),	/* Offset= -42 (1302) */
/* 1346 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 1348 */	NdrFcShort( 0x90 ),	/* 144 */
/* 1350 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1352 */	NdrFcShort( 0x20 ),	/* Offset= 32 (1384) */
/* 1354 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1356 */	NdrFcShort( 0xfac0 ),	/* Offset= -1344 (12) */
/* 1358 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1360 */	NdrFcShort( 0xfabc ),	/* Offset= -1348 (12) */
/* 1362 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1364 */	0x0,		/* 0 */
			NdrFcShort( 0xfc0d ),	/* Offset= -1011 (354) */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1368 */	0x0,		/* 0 */
			NdrFcShort( 0xfc09 ),	/* Offset= -1015 (354) */
			0x36,		/* FC_POINTER */
/* 1372 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1374 */	NdrFcShort( 0xfc84 ),	/* Offset= -892 (482) */
/* 1376 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 1378 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 1380 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 1382 */	0x40,		/* FC_STRUCTPAD4 */
			0x5b,		/* FC_END */
/* 1384 */	
			0x12, 0x0,	/* FC_UP */
/* 1386 */	NdrFcShort( 0xfb48 ),	/* Offset= -1208 (178) */
/* 1388 */	
			0x12, 0x0,	/* FC_UP */
/* 1390 */	NdrFcShort( 0xfcb6 ),	/* Offset= -842 (548) */
/* 1392 */	
			0x12, 0x0,	/* FC_UP */
/* 1394 */	NdrFcShort( 0xffb2 ),	/* Offset= -78 (1316) */
/* 1396 */	
			0x1b,		/* FC_CARRAY */
			0x0,		/* 0 */
/* 1398 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1400 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 1402 */	NdrFcShort( 0x4 ),	/* 4 */
/* 1404 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1406 */	0x0 , 
			0x0,		/* 0 */
/* 1408 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1412 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1416 */	0x2,		/* FC_CHAR */
			0x5b,		/* FC_END */
/* 1418 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 1420 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1422 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1424 */	NdrFcShort( 0x6 ),	/* Offset= 6 (1430) */
/* 1426 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 1428 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 1430 */	
			0x12, 0x0,	/* FC_UP */
/* 1432 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (1396) */
/* 1434 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 1436 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1438 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1440 */	NdrFcShort( 0x0 ),	/* Offset= 0 (1440) */
/* 1442 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1444 */	NdrFcShort( 0xffe6 ),	/* Offset= -26 (1418) */
/* 1446 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1448 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 1450 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1454 */	NdrFcLong( 0x100000 ),	/* 1048576 */
/* 1458 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 1460 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1464 */	NdrFcLong( 0x100000 ),	/* 1048576 */
/* 1468 */	
			0x15,		/* FC_STRUCT */
			0x7,		/* 7 */
/* 1470 */	NdrFcShort( 0x20 ),	/* 32 */
/* 1472 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1474 */	NdrFcShort( 0xfa4a ),	/* Offset= -1462 (12) */
/* 1476 */	0xb,		/* FC_HYPER */
			0xb,		/* FC_HYPER */
/* 1478 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1480 */	
			0x1b,		/* FC_CARRAY */
			0x7,		/* 7 */
/* 1482 */	NdrFcShort( 0x20 ),	/* 32 */
/* 1484 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 1486 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 1488 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1490 */	0x0 , 
			0x0,		/* 0 */
/* 1492 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1496 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1500 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1502 */	NdrFcShort( 0xffde ),	/* Offset= -34 (1468) */
/* 1504 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1506 */	0xb1,		/* FC_FORCED_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 1508 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1510 */	NdrFcShort( 0xffe2 ),	/* Offset= -30 (1480) */
/* 1512 */	NdrFcShort( 0x0 ),	/* Offset= 0 (1512) */
/* 1514 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 1516 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1518 */	NdrFcShort( 0xffc4 ),	/* Offset= -60 (1458) */
/* 1520 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 1522 */	0xb1,		/* FC_FORCED_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 1524 */	NdrFcShort( 0x30 ),	/* 48 */
/* 1526 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1528 */	NdrFcShort( 0x0 ),	/* Offset= 0 (1528) */
/* 1530 */	0xb,		/* FC_HYPER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1532 */	0x0,		/* 0 */
			NdrFcShort( 0xfedd ),	/* Offset= -291 (1242) */
			0x5b,		/* FC_END */
/* 1536 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 1538 */	NdrFcShort( 0x58 ),	/* 88 */
/* 1540 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1542 */	NdrFcShort( 0x10 ),	/* Offset= 16 (1558) */
/* 1544 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 1546 */	0x40,		/* FC_STRUCTPAD4 */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1548 */	0x0,		/* 0 */
			NdrFcShort( 0xfe0f ),	/* Offset= -497 (1052) */
			0x8,		/* FC_LONG */
/* 1552 */	0x40,		/* FC_STRUCTPAD4 */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1554 */	0x0,		/* 0 */
			NdrFcShort( 0xffdf ),	/* Offset= -33 (1522) */
			0x5b,		/* FC_END */
/* 1558 */	
			0x12, 0x0,	/* FC_UP */
/* 1560 */	NdrFcShort( 0xfa9a ),	/* Offset= -1382 (178) */
/* 1562 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x7,		/* 7 */
/* 1564 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1566 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 1568 */	NdrFcShort( 0x94 ),	/* 148 */
/* 1570 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1572 */	0x0 , 
			0x0,		/* 0 */
/* 1574 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1578 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1582 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 1586 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 1588 */	0x0 , 
			0x0,		/* 0 */
/* 1590 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1594 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1598 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1600 */	NdrFcShort( 0xffc0 ),	/* Offset= -64 (1536) */
/* 1602 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1604 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 1606 */	NdrFcShort( 0xa8 ),	/* 168 */
/* 1608 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1610 */	NdrFcShort( 0x28 ),	/* Offset= 40 (1650) */
/* 1612 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1614 */	NdrFcShort( 0xf9be ),	/* Offset= -1602 (12) */
/* 1616 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1618 */	NdrFcShort( 0xf9ba ),	/* Offset= -1606 (12) */
/* 1620 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1622 */	0x0,		/* 0 */
			NdrFcShort( 0xfb0b ),	/* Offset= -1269 (354) */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1626 */	0x0,		/* 0 */
			NdrFcShort( 0xfb07 ),	/* Offset= -1273 (354) */
			0x36,		/* FC_POINTER */
/* 1630 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1632 */	NdrFcShort( 0xfb82 ),	/* Offset= -1150 (482) */
/* 1634 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 1636 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 1638 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 1640 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 1642 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1644 */	NdrFcShort( 0xff3c ),	/* Offset= -196 (1448) */
/* 1646 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 1648 */	0x40,		/* FC_STRUCTPAD4 */
			0x5b,		/* FC_END */
/* 1650 */	
			0x12, 0x0,	/* FC_UP */
/* 1652 */	NdrFcShort( 0xfa3e ),	/* Offset= -1474 (178) */
/* 1654 */	
			0x12, 0x0,	/* FC_UP */
/* 1656 */	NdrFcShort( 0xff6a ),	/* Offset= -150 (1506) */
/* 1658 */	
			0x12, 0x0,	/* FC_UP */
/* 1660 */	NdrFcShort( 0xfea8 ),	/* Offset= -344 (1316) */
/* 1662 */	
			0x12, 0x0,	/* FC_UP */
/* 1664 */	NdrFcShort( 0xff9a ),	/* Offset= -102 (1562) */
/* 1666 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 1668 */	NdrFcShort( 0x18 ),	/* 24 */
/* 1670 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1672 */	NdrFcShort( 0x0 ),	/* Offset= 0 (1672) */
/* 1674 */	0x8,		/* FC_LONG */
			0xd,		/* FC_ENUM16 */
/* 1676 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1678 */	NdrFcShort( 0xfefc ),	/* Offset= -260 (1418) */
/* 1680 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1682 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 1684 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1688 */	NdrFcLong( 0x100000 ),	/* 1048576 */
/* 1692 */	0xb1,		/* FC_FORCED_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 1694 */	NdrFcShort( 0x48 ),	/* 72 */
/* 1696 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1698 */	NdrFcShort( 0x0 ),	/* Offset= 0 (1698) */
/* 1700 */	0xb,		/* FC_HYPER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1702 */	0x0,		/* 0 */
			NdrFcShort( 0xfe33 ),	/* Offset= -461 (1242) */
			0x8,		/* FC_LONG */
/* 1706 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 1708 */	0x40,		/* FC_STRUCTPAD4 */
			0xb,		/* FC_HYPER */
/* 1710 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1712 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 1714 */	NdrFcShort( 0x70 ),	/* 112 */
/* 1716 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1718 */	NdrFcShort( 0x10 ),	/* Offset= 16 (1734) */
/* 1720 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 1722 */	0x40,		/* FC_STRUCTPAD4 */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1724 */	0x0,		/* 0 */
			NdrFcShort( 0xfd5f ),	/* Offset= -673 (1052) */
			0x8,		/* FC_LONG */
/* 1728 */	0x40,		/* FC_STRUCTPAD4 */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1730 */	0x0,		/* 0 */
			NdrFcShort( 0xffd9 ),	/* Offset= -39 (1692) */
			0x5b,		/* FC_END */
/* 1734 */	
			0x12, 0x0,	/* FC_UP */
/* 1736 */	NdrFcShort( 0xf9ea ),	/* Offset= -1558 (178) */
/* 1738 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x7,		/* 7 */
/* 1740 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1742 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 1744 */	NdrFcShort( 0x94 ),	/* 148 */
/* 1746 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1748 */	0x0 , 
			0x0,		/* 0 */
/* 1750 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1754 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1758 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 1762 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 1764 */	0x0 , 
			0x0,		/* 0 */
/* 1766 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1770 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1774 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1776 */	NdrFcShort( 0xffc0 ),	/* Offset= -64 (1712) */
/* 1778 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1780 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 1782 */	NdrFcShort( 0xa8 ),	/* 168 */
/* 1784 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1786 */	NdrFcShort( 0x28 ),	/* Offset= 40 (1826) */
/* 1788 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1790 */	NdrFcShort( 0xf90e ),	/* Offset= -1778 (12) */
/* 1792 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1794 */	NdrFcShort( 0xf90a ),	/* Offset= -1782 (12) */
/* 1796 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1798 */	0x0,		/* 0 */
			NdrFcShort( 0xfa5b ),	/* Offset= -1445 (354) */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1802 */	0x0,		/* 0 */
			NdrFcShort( 0xfa57 ),	/* Offset= -1449 (354) */
			0x36,		/* FC_POINTER */
/* 1806 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1808 */	NdrFcShort( 0xfad2 ),	/* Offset= -1326 (482) */
/* 1810 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 1812 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 1814 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 1816 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 1818 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1820 */	NdrFcShort( 0xff76 ),	/* Offset= -138 (1682) */
/* 1822 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 1824 */	0x40,		/* FC_STRUCTPAD4 */
			0x5b,		/* FC_END */
/* 1826 */	
			0x12, 0x0,	/* FC_UP */
/* 1828 */	NdrFcShort( 0xf98e ),	/* Offset= -1650 (178) */
/* 1830 */	
			0x12, 0x0,	/* FC_UP */
/* 1832 */	NdrFcShort( 0xfeba ),	/* Offset= -326 (1506) */
/* 1834 */	
			0x12, 0x0,	/* FC_UP */
/* 1836 */	NdrFcShort( 0xfdf8 ),	/* Offset= -520 (1316) */
/* 1838 */	
			0x12, 0x0,	/* FC_UP */
/* 1840 */	NdrFcShort( 0xff9a ),	/* Offset= -102 (1738) */
/* 1842 */	
			0x11, 0x0,	/* FC_RP */
/* 1844 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1846) */
/* 1846 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 1848 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 1850 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 1852 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1854 */	0x0 , 
			0x0,		/* 0 */
/* 1856 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1860 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1864 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1866) */
/* 1866 */	NdrFcShort( 0x40 ),	/* 64 */
/* 1868 */	NdrFcShort( 0x2 ),	/* 2 */
/* 1870 */	NdrFcLong( 0x1 ),	/* 1 */
/* 1874 */	NdrFcShort( 0xa ),	/* Offset= 10 (1884) */
/* 1876 */	NdrFcLong( 0x2 ),	/* 2 */
/* 1880 */	NdrFcShort( 0x1e ),	/* Offset= 30 (1910) */
/* 1882 */	NdrFcShort( 0xffff ),	/* Offset= -1 (1881) */
/* 1884 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 1886 */	NdrFcShort( 0x28 ),	/* 40 */
/* 1888 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1890 */	NdrFcShort( 0xc ),	/* Offset= 12 (1902) */
/* 1892 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 1894 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1896 */	NdrFcShort( 0xf8a4 ),	/* Offset= -1884 (12) */
/* 1898 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 1900 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1902 */	
			0x11, 0x0,	/* FC_RP */
/* 1904 */	NdrFcShort( 0xf942 ),	/* Offset= -1726 (178) */
/* 1906 */	
			0x11, 0x8,	/* FC_RP [simple_pointer] */
/* 1908 */	
			0x22,		/* FC_C_CSTRING */
			0x5c,		/* FC_PAD */
/* 1910 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 1912 */	NdrFcShort( 0x40 ),	/* 64 */
/* 1914 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1916 */	NdrFcShort( 0x10 ),	/* Offset= 16 (1932) */
/* 1918 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 1920 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1922 */	NdrFcShort( 0xf88a ),	/* Offset= -1910 (12) */
/* 1924 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1926 */	0x0,		/* 0 */
			NdrFcShort( 0xf885 ),	/* Offset= -1915 (12) */
			0x40,		/* FC_STRUCTPAD4 */
/* 1930 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 1932 */	
			0x11, 0x0,	/* FC_RP */
/* 1934 */	NdrFcShort( 0xf924 ),	/* Offset= -1756 (178) */
/* 1936 */	
			0x11, 0x8,	/* FC_RP [simple_pointer] */
/* 1938 */	
			0x22,		/* FC_C_CSTRING */
			0x5c,		/* FC_PAD */
/* 1940 */	
			0x12, 0x0,	/* FC_UP */
/* 1942 */	NdrFcShort( 0xf95e ),	/* Offset= -1698 (244) */
/* 1944 */	
			0x11, 0x0,	/* FC_RP */
/* 1946 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1948) */
/* 1948 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 1950 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 1952 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 1954 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1956 */	0x0 , 
			0x0,		/* 0 */
/* 1958 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1962 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1966 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1968) */
/* 1968 */	NdrFcShort( 0x90 ),	/* 144 */
/* 1970 */	NdrFcShort( 0x3 ),	/* 3 */
/* 1972 */	NdrFcLong( 0x1 ),	/* 1 */
/* 1976 */	NdrFcShort( 0x20 ),	/* Offset= 32 (2008) */
/* 1978 */	NdrFcLong( 0x2 ),	/* 2 */
/* 1982 */	NdrFcShort( 0x32 ),	/* Offset= 50 (2032) */
/* 1984 */	NdrFcLong( 0x3 ),	/* 3 */
/* 1988 */	NdrFcShort( 0x4e ),	/* Offset= 78 (2066) */
/* 1990 */	NdrFcShort( 0xffff ),	/* Offset= -1 (1989) */
/* 1992 */	
			0x1d,		/* FC_SMFARRAY */
			0x0,		/* 0 */
/* 1994 */	NdrFcShort( 0x54 ),	/* 84 */
/* 1996 */	0x2,		/* FC_CHAR */
			0x5b,		/* FC_END */
/* 1998 */	
			0x15,		/* FC_STRUCT */
			0x0,		/* 0 */
/* 2000 */	NdrFcShort( 0x54 ),	/* 84 */
/* 2002 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 2004 */	NdrFcShort( 0xfff4 ),	/* Offset= -12 (1992) */
/* 2006 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 2008 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 2010 */	NdrFcShort( 0x68 ),	/* 104 */
/* 2012 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2014 */	NdrFcShort( 0xa ),	/* Offset= 10 (2024) */
/* 2016 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 2018 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 2020 */	NdrFcShort( 0xffea ),	/* Offset= -22 (1998) */
/* 2022 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 2024 */	
			0x11, 0x0,	/* FC_RP */
/* 2026 */	NdrFcShort( 0xf8c8 ),	/* Offset= -1848 (178) */
/* 2028 */	
			0x11, 0x8,	/* FC_RP [simple_pointer] */
/* 2030 */	
			0x22,		/* FC_C_CSTRING */
			0x5c,		/* FC_PAD */
/* 2032 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 2034 */	NdrFcShort( 0x78 ),	/* 120 */
/* 2036 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2038 */	NdrFcShort( 0xc ),	/* Offset= 12 (2050) */
/* 2040 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 2042 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 2044 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 2046 */	NdrFcShort( 0xffd0 ),	/* Offset= -48 (1998) */
/* 2048 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 2050 */	
			0x11, 0x0,	/* FC_RP */
/* 2052 */	NdrFcShort( 0xf8ae ),	/* Offset= -1874 (178) */
/* 2054 */	
			0x12, 0x0,	/* FC_UP */
/* 2056 */	NdrFcShort( 0xf8aa ),	/* Offset= -1878 (178) */
/* 2058 */	
			0x12, 0x0,	/* FC_UP */
/* 2060 */	NdrFcShort( 0xf8a6 ),	/* Offset= -1882 (178) */
/* 2062 */	
			0x11, 0x8,	/* FC_RP [simple_pointer] */
/* 2064 */	
			0x22,		/* FC_C_CSTRING */
			0x5c,		/* FC_PAD */
/* 2066 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 2068 */	NdrFcShort( 0x90 ),	/* 144 */
/* 2070 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2072 */	NdrFcShort( 0x12 ),	/* Offset= 18 (2090) */
/* 2074 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 2076 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 2078 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 2080 */	NdrFcShort( 0xffae ),	/* Offset= -82 (1998) */
/* 2082 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 2084 */	0x0,		/* 0 */
			NdrFcShort( 0xf7e7 ),	/* Offset= -2073 (12) */
			0x36,		/* FC_POINTER */
/* 2088 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 2090 */	
			0x11, 0x0,	/* FC_RP */
/* 2092 */	NdrFcShort( 0xf886 ),	/* Offset= -1914 (178) */
/* 2094 */	
			0x12, 0x0,	/* FC_UP */
/* 2096 */	NdrFcShort( 0xf882 ),	/* Offset= -1918 (178) */
/* 2098 */	
			0x12, 0x0,	/* FC_UP */
/* 2100 */	NdrFcShort( 0xf87e ),	/* Offset= -1922 (178) */
/* 2102 */	
			0x11, 0x8,	/* FC_RP [simple_pointer] */
/* 2104 */	
			0x22,		/* FC_C_CSTRING */
			0x5c,		/* FC_PAD */
/* 2106 */	
			0x12, 0x0,	/* FC_UP */
/* 2108 */	NdrFcShort( 0xf8b8 ),	/* Offset= -1864 (244) */
/* 2110 */	
			0x11, 0x0,	/* FC_RP */
/* 2112 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2114) */
/* 2114 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 2116 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 2118 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 2120 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2122 */	0x0 , 
			0x0,		/* 0 */
/* 2124 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2128 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2132 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2134) */
/* 2134 */	NdrFcShort( 0x18 ),	/* 24 */
/* 2136 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2138 */	NdrFcLong( 0x1 ),	/* 1 */
/* 2142 */	NdrFcShort( 0x4 ),	/* Offset= 4 (2146) */
/* 2144 */	NdrFcShort( 0xffff ),	/* Offset= -1 (2143) */
/* 2146 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 2148 */	NdrFcShort( 0x18 ),	/* 24 */
/* 2150 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2152 */	NdrFcShort( 0x8 ),	/* Offset= 8 (2160) */
/* 2154 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 2156 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 2158 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 2160 */	
			0x11, 0x0,	/* FC_RP */
/* 2162 */	NdrFcShort( 0xf840 ),	/* Offset= -1984 (178) */
/* 2164 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2166 */	
			0x22,		/* FC_C_CSTRING */
			0x5c,		/* FC_PAD */
/* 2168 */	
			0x11, 0x0,	/* FC_RP */
/* 2170 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2172) */
/* 2172 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 2174 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 2176 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 2178 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2180 */	0x0 , 
			0x0,		/* 0 */
/* 2182 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2186 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2190 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2192) */
/* 2192 */	NdrFcShort( 0x80 ),	/* 128 */
/* 2194 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2196 */	NdrFcLong( 0x1 ),	/* 1 */
/* 2200 */	NdrFcShort( 0x4 ),	/* Offset= 4 (2204) */
/* 2202 */	NdrFcShort( 0xffff ),	/* Offset= -1 (2201) */
/* 2204 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 2206 */	NdrFcShort( 0x80 ),	/* 128 */
/* 2208 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2210 */	NdrFcShort( 0x10 ),	/* Offset= 16 (2226) */
/* 2212 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 2214 */	0x0,		/* 0 */
			NdrFcShort( 0xf765 ),	/* Offset= -2203 (12) */
			0x36,		/* FC_POINTER */
/* 2218 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 2220 */	NdrFcShort( 0xff22 ),	/* Offset= -222 (1998) */
/* 2222 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 2224 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 2226 */	
			0x11, 0x0,	/* FC_RP */
/* 2228 */	NdrFcShort( 0xf7fe ),	/* Offset= -2050 (178) */
/* 2230 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2232 */	
			0x22,		/* FC_C_CSTRING */
			0x5c,		/* FC_PAD */
/* 2234 */	
			0x11, 0x0,	/* FC_RP */
/* 2236 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2238) */
/* 2238 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 2240 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 2242 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 2244 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2246 */	0x0 , 
			0x0,		/* 0 */
/* 2248 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2252 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2256 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2258) */
/* 2258 */	NdrFcShort( 0x30 ),	/* 48 */
/* 2260 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2262 */	NdrFcLong( 0x1 ),	/* 1 */
/* 2266 */	NdrFcShort( 0x38 ),	/* Offset= 56 (2322) */
/* 2268 */	NdrFcShort( 0xffff ),	/* Offset= -1 (2267) */
/* 2270 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 2272 */	NdrFcLong( 0x1 ),	/* 1 */
/* 2276 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 2280 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 2282 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2284 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 2286 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2288 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2290 */	0x0 , 
			0x0,		/* 0 */
/* 2292 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2296 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2300 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 2304 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 2306 */	0x0 , 
			0x0,		/* 0 */
/* 2308 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2312 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2316 */	
			0x12, 0x0,	/* FC_UP */
/* 2318 */	NdrFcShort( 0xf7a4 ),	/* Offset= -2140 (178) */
/* 2320 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 2322 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 2324 */	NdrFcShort( 0x30 ),	/* 48 */
/* 2326 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2328 */	NdrFcShort( 0x12 ),	/* Offset= 18 (2346) */
/* 2330 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 2332 */	0x0,		/* 0 */
			NdrFcShort( 0xffc1 ),	/* Offset= -63 (2270) */
			0x36,		/* FC_POINTER */
/* 2336 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 2338 */	NdrFcShort( 0xfb86 ),	/* Offset= -1146 (1192) */
/* 2340 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 2342 */	NdrFcShort( 0xf8bc ),	/* Offset= -1860 (482) */
/* 2344 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 2346 */	
			0x12, 0x0,	/* FC_UP */
/* 2348 */	NdrFcShort( 0xffbc ),	/* Offset= -68 (2280) */
/* 2350 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 2352 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2354) */
/* 2354 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 2356 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 2358 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 2360 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2362 */	0x0 , 
			0x0,		/* 0 */
/* 2364 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2368 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2372 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2374) */
/* 2374 */	NdrFcShort( 0x20 ),	/* 32 */
/* 2376 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2378 */	NdrFcLong( 0x1 ),	/* 1 */
/* 2382 */	NdrFcShort( 0x38 ),	/* Offset= 56 (2438) */
/* 2384 */	NdrFcShort( 0xffff ),	/* Offset= -1 (2383) */
/* 2386 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 2388 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2392 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 2396 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 2398 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2400 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 2402 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2404 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2406 */	0x0 , 
			0x0,		/* 0 */
/* 2408 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2412 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2416 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 2420 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 2422 */	0x0 , 
			0x0,		/* 0 */
/* 2424 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2428 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2432 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 2434 */	NdrFcShort( 0xfb3a ),	/* Offset= -1222 (1212) */
/* 2436 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 2438 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 2440 */	NdrFcShort( 0x20 ),	/* 32 */
/* 2442 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2444 */	NdrFcShort( 0xe ),	/* Offset= 14 (2458) */
/* 2446 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 2448 */	0x0,		/* 0 */
			NdrFcShort( 0xffc1 ),	/* Offset= -63 (2386) */
			0x36,		/* FC_POINTER */
/* 2452 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 2454 */	NdrFcShort( 0xf84c ),	/* Offset= -1972 (482) */
/* 2456 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 2458 */	
			0x12, 0x0,	/* FC_UP */
/* 2460 */	NdrFcShort( 0xffc0 ),	/* Offset= -64 (2396) */
/* 2462 */	
			0x11, 0x0,	/* FC_RP */
/* 2464 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2466) */
/* 2466 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 2468 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 2470 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 2472 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2474 */	0x0 , 
			0x0,		/* 0 */
/* 2476 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2480 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2484 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2486) */
/* 2486 */	NdrFcShort( 0x20 ),	/* 32 */
/* 2488 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2490 */	NdrFcLong( 0x1 ),	/* 1 */
/* 2494 */	NdrFcShort( 0x42 ),	/* Offset= 66 (2560) */
/* 2496 */	NdrFcShort( 0xffff ),	/* Offset= -1 (2495) */
/* 2498 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 2500 */	NdrFcLong( 0x1 ),	/* 1 */
/* 2504 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 2508 */	0xb7,		/* FC_RANGE */
			0xd,		/* 13 */
/* 2510 */	NdrFcLong( 0x1 ),	/* 1 */
/* 2514 */	NdrFcLong( 0x7 ),	/* 7 */
/* 2518 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 2520 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2522 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 2524 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2526 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2528 */	0x0 , 
			0x0,		/* 0 */
/* 2530 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2534 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2538 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 2542 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 2544 */	0x0 , 
			0x0,		/* 0 */
/* 2546 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2550 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2554 */	
			0x12, 0x0,	/* FC_UP */
/* 2556 */	NdrFcShort( 0xf6b6 ),	/* Offset= -2378 (178) */
/* 2558 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 2560 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 2562 */	NdrFcShort( 0x20 ),	/* 32 */
/* 2564 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2566 */	NdrFcShort( 0x10 ),	/* Offset= 16 (2582) */
/* 2568 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 2570 */	NdrFcShort( 0xffb8 ),	/* Offset= -72 (2498) */
/* 2572 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 2574 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 2576 */	0x0,		/* 0 */
			NdrFcShort( 0xffbb ),	/* Offset= -69 (2508) */
			0x36,		/* FC_POINTER */
/* 2580 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 2582 */	
			0x12, 0x0,	/* FC_UP */
/* 2584 */	NdrFcShort( 0xffbe ),	/* Offset= -66 (2518) */
/* 2586 */	
			0x12, 0x0,	/* FC_UP */
/* 2588 */	NdrFcShort( 0xf696 ),	/* Offset= -2410 (178) */
/* 2590 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 2592 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2594) */
/* 2594 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 2596 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 2598 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 2600 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2602 */	0x0 , 
			0x0,		/* 0 */
/* 2604 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2608 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2612 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2614) */
/* 2614 */	NdrFcShort( 0x28 ),	/* 40 */
/* 2616 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2618 */	NdrFcLong( 0x1 ),	/* 1 */
/* 2622 */	NdrFcShort( 0x58 ),	/* Offset= 88 (2710) */
/* 2624 */	NdrFcShort( 0xffff ),	/* Offset= -1 (2623) */
/* 2626 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 2628 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2632 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 2636 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 2638 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2642 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 2646 */	
			0x1b,		/* FC_CARRAY */
			0x3,		/* 3 */
/* 2648 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2650 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 2652 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2654 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2656 */	0x0 , 
			0x0,		/* 0 */
/* 2658 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2662 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2666 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 2668 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 2670 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2672 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 2674 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2676 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2678 */	0x0 , 
			0x0,		/* 0 */
/* 2680 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2684 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2688 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 2692 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 2694 */	0x0 , 
			0x0,		/* 0 */
/* 2696 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2700 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2704 */	
			0x12, 0x0,	/* FC_UP */
/* 2706 */	NdrFcShort( 0xf600 ),	/* Offset= -2560 (146) */
/* 2708 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 2710 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 2712 */	NdrFcShort( 0x28 ),	/* 40 */
/* 2714 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2716 */	NdrFcShort( 0x10 ),	/* Offset= 16 (2732) */
/* 2718 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 2720 */	0x0,		/* 0 */
			NdrFcShort( 0xffa1 ),	/* Offset= -95 (2626) */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 2724 */	0x0,		/* 0 */
			NdrFcShort( 0xffa7 ),	/* Offset= -89 (2636) */
			0x40,		/* FC_STRUCTPAD4 */
/* 2728 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 2730 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 2732 */	
			0x12, 0x0,	/* FC_UP */
/* 2734 */	NdrFcShort( 0xfe3a ),	/* Offset= -454 (2280) */
/* 2736 */	
			0x12, 0x0,	/* FC_UP */
/* 2738 */	NdrFcShort( 0xffa4 ),	/* Offset= -92 (2646) */
/* 2740 */	
			0x12, 0x0,	/* FC_UP */
/* 2742 */	NdrFcShort( 0xffb6 ),	/* Offset= -74 (2668) */
/* 2744 */	
			0x11, 0x0,	/* FC_RP */
/* 2746 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2748) */
/* 2748 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 2750 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 2752 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 2754 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2756 */	0x0 , 
			0x0,		/* 0 */
/* 2758 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2762 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2766 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2768) */
/* 2768 */	NdrFcShort( 0x40 ),	/* 64 */
/* 2770 */	NdrFcShort( 0x2 ),	/* 2 */
/* 2772 */	NdrFcLong( 0x1 ),	/* 1 */
/* 2776 */	NdrFcShort( 0xa ),	/* Offset= 10 (2786) */
/* 2778 */	NdrFcLong( 0x2 ),	/* 2 */
/* 2782 */	NdrFcShort( 0x88 ),	/* Offset= 136 (2918) */
/* 2784 */	NdrFcShort( 0xffff ),	/* Offset= -1 (2783) */
/* 2786 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 2788 */	NdrFcShort( 0x30 ),	/* 48 */
/* 2790 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2792 */	NdrFcShort( 0xc ),	/* Offset= 12 (2804) */
/* 2794 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 2796 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 2798 */	0x0,		/* 0 */
			NdrFcShort( 0xf6f3 ),	/* Offset= -2317 (482) */
			0x8,		/* FC_LONG */
/* 2802 */	0x40,		/* FC_STRUCTPAD4 */
			0x5b,		/* FC_END */
/* 2804 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2806 */	0x2,		/* FC_CHAR */
			0x5c,		/* FC_PAD */
/* 2808 */	
			0x12, 0x0,	/* FC_UP */
/* 2810 */	NdrFcShort( 0xf9c2 ),	/* Offset= -1598 (1212) */
/* 2812 */	
			0x12, 0x0,	/* FC_UP */
/* 2814 */	NdrFcShort( 0xf50e ),	/* Offset= -2802 (12) */
/* 2816 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 2818 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2822 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 2826 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 2828 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2832 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 2836 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 2838 */	NdrFcShort( 0x10 ),	/* 16 */
/* 2840 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2842 */	NdrFcShort( 0xa ),	/* Offset= 10 (2852) */
/* 2844 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 2846 */	NdrFcShort( 0xffec ),	/* Offset= -20 (2826) */
/* 2848 */	0x8,		/* FC_LONG */
			0x36,		/* FC_POINTER */
/* 2850 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 2852 */	
			0x12, 0x0,	/* FC_UP */
/* 2854 */	NdrFcShort( 0xf658 ),	/* Offset= -2472 (382) */
/* 2856 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 2858 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2860 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 2862 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2864 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2866 */	0x0 , 
			0x0,		/* 0 */
/* 2868 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2872 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2876 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 2880 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 2882 */	0x0 , 
			0x0,		/* 0 */
/* 2884 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2888 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2892 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 2894 */	NdrFcShort( 0xffc6 ),	/* Offset= -58 (2836) */
/* 2896 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 2898 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 2900 */	NdrFcShort( 0x10 ),	/* 16 */
/* 2902 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2904 */	NdrFcShort( 0xa ),	/* Offset= 10 (2914) */
/* 2906 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 2908 */	0x0,		/* 0 */
			NdrFcShort( 0xffa3 ),	/* Offset= -93 (2816) */
			0x36,		/* FC_POINTER */
/* 2912 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 2914 */	
			0x12, 0x0,	/* FC_UP */
/* 2916 */	NdrFcShort( 0xffc4 ),	/* Offset= -60 (2856) */
/* 2918 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 2920 */	NdrFcShort( 0x40 ),	/* 64 */
/* 2922 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2924 */	NdrFcShort( 0xe ),	/* Offset= 14 (2938) */
/* 2926 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 2928 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 2930 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 2932 */	0x0,		/* 0 */
			NdrFcShort( 0xf66d ),	/* Offset= -2451 (482) */
			0x8,		/* FC_LONG */
/* 2936 */	0x40,		/* FC_STRUCTPAD4 */
			0x5b,		/* FC_END */
/* 2938 */	
			0x12, 0x0,	/* FC_UP */
/* 2940 */	NdrFcShort( 0xf536 ),	/* Offset= -2762 (178) */
/* 2942 */	
			0x12, 0x0,	/* FC_UP */
/* 2944 */	NdrFcShort( 0xf93c ),	/* Offset= -1732 (1212) */
/* 2946 */	
			0x12, 0x0,	/* FC_UP */
/* 2948 */	NdrFcShort( 0xf52e ),	/* Offset= -2770 (178) */
/* 2950 */	
			0x12, 0x0,	/* FC_UP */
/* 2952 */	NdrFcShort( 0xf52a ),	/* Offset= -2774 (178) */
/* 2954 */	
			0x12, 0x0,	/* FC_UP */
/* 2956 */	NdrFcShort( 0xffc6 ),	/* Offset= -58 (2898) */
/* 2958 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 2960 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2962) */
/* 2962 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 2964 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 2966 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 2968 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2970 */	0x0 , 
			0x0,		/* 0 */
/* 2972 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2976 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2980 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2982) */
/* 2982 */	NdrFcShort( 0x20 ),	/* 32 */
/* 2984 */	NdrFcShort( 0x2 ),	/* 2 */
/* 2986 */	NdrFcLong( 0x1 ),	/* 1 */
/* 2990 */	NdrFcShort( 0xe ),	/* Offset= 14 (3004) */
/* 2992 */	NdrFcLong( 0x2 ),	/* 2 */
/* 2996 */	NdrFcShort( 0x20 ),	/* Offset= 32 (3028) */
/* 2998 */	NdrFcShort( 0xffff ),	/* Offset= -1 (2997) */
/* 3000 */	
			0x12, 0x0,	/* FC_UP */
/* 3002 */	NdrFcShort( 0xf902 ),	/* Offset= -1790 (1212) */
/* 3004 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 3006 */	NdrFcShort( 0x20 ),	/* 32 */
/* 3008 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3010 */	NdrFcShort( 0xa ),	/* Offset= 10 (3020) */
/* 3012 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 3014 */	0x0,		/* 0 */
			NdrFcShort( 0xf61b ),	/* Offset= -2533 (482) */
			0x36,		/* FC_POINTER */
/* 3018 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 3020 */	
			0x12, 0x10,	/* FC_UP [pointer_deref] */
/* 3022 */	NdrFcShort( 0xffea ),	/* Offset= -22 (3000) */
/* 3024 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3026 */	0x8,		/* FC_LONG */
			0x5c,		/* FC_PAD */
/* 3028 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 3030 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3032 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3034 */	NdrFcShort( 0x6 ),	/* Offset= 6 (3040) */
/* 3036 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 3038 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 3040 */	
			0x12, 0x0,	/* FC_UP */
/* 3042 */	NdrFcShort( 0xf4d0 ),	/* Offset= -2864 (178) */
/* 3044 */	
			0x11, 0x0,	/* FC_RP */
/* 3046 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3048) */
/* 3048 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 3050 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 3052 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 3054 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3056 */	0x0 , 
			0x0,		/* 0 */
/* 3058 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3062 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3066 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3068) */
/* 3068 */	NdrFcShort( 0x18 ),	/* 24 */
/* 3070 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3072 */	NdrFcLong( 0x1 ),	/* 1 */
/* 3076 */	NdrFcShort( 0x24 ),	/* Offset= 36 (3112) */
/* 3078 */	NdrFcShort( 0xffff ),	/* Offset= -1 (3077) */
/* 3080 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 3082 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3086 */	NdrFcLong( 0xa00000 ),	/* 10485760 */
/* 3090 */	
			0x1b,		/* FC_CARRAY */
			0x0,		/* 0 */
/* 3092 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3094 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 3096 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3098 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3100 */	0x0 , 
			0x0,		/* 0 */
/* 3102 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3106 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3110 */	0x2,		/* FC_CHAR */
			0x5b,		/* FC_END */
/* 3112 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 3114 */	NdrFcShort( 0x18 ),	/* 24 */
/* 3116 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3118 */	NdrFcShort( 0xc ),	/* Offset= 12 (3130) */
/* 3120 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 3122 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 3124 */	NdrFcShort( 0xffd4 ),	/* Offset= -44 (3080) */
/* 3126 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 3128 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 3130 */	
			0x12, 0x0,	/* FC_UP */
/* 3132 */	NdrFcShort( 0xffd6 ),	/* Offset= -42 (3090) */
/* 3134 */	
			0x11, 0x0,	/* FC_RP */
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
/* 3158 */	NdrFcShort( 0x50 ),	/* 80 */
/* 3160 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3162 */	NdrFcLong( 0x1 ),	/* 1 */
/* 3166 */	NdrFcShort( 0x36 ),	/* Offset= 54 (3220) */
/* 3168 */	NdrFcShort( 0xffff ),	/* Offset= -1 (3167) */
/* 3170 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 3172 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3176 */	NdrFcLong( 0xa00000 ),	/* 10485760 */
/* 3180 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 3182 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3186 */	NdrFcLong( 0xa00000 ),	/* 10485760 */
/* 3190 */	
			0x15,		/* FC_STRUCT */
			0x7,		/* 7 */
/* 3192 */	NdrFcShort( 0x30 ),	/* 48 */
/* 3194 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 3196 */	NdrFcShort( 0xf642 ),	/* Offset= -2494 (702) */
/* 3198 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 3200 */	NdrFcShort( 0xf63e ),	/* Offset= -2498 (702) */
/* 3202 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 3204 */	NdrFcShort( 0xf63a ),	/* Offset= -2502 (702) */
/* 3206 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 3208 */	NdrFcShort( 0xf636 ),	/* Offset= -2506 (702) */
/* 3210 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 3212 */	NdrFcShort( 0xf632 ),	/* Offset= -2510 (702) */
/* 3214 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 3216 */	NdrFcShort( 0xf62e ),	/* Offset= -2514 (702) */
/* 3218 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 3220 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 3222 */	NdrFcShort( 0x50 ),	/* 80 */
/* 3224 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3226 */	NdrFcShort( 0x14 ),	/* Offset= 20 (3246) */
/* 3228 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 3230 */	NdrFcShort( 0xffc4 ),	/* Offset= -60 (3170) */
/* 3232 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 3234 */	NdrFcShort( 0xffca ),	/* Offset= -54 (3180) */
/* 3236 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 3238 */	NdrFcShort( 0xffd0 ),	/* Offset= -48 (3190) */
/* 3240 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 3242 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 3244 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 3246 */	
			0x12, 0x0,	/* FC_UP */
/* 3248 */	NdrFcShort( 0xf4ce ),	/* Offset= -2866 (382) */
/* 3250 */	
			0x12, 0x0,	/* FC_UP */
/* 3252 */	NdrFcShort( 0xf8c0 ),	/* Offset= -1856 (1396) */
/* 3254 */	
			0x11, 0x0,	/* FC_RP */
/* 3256 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3258) */
/* 3258 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 3260 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 3262 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 3264 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3266 */	0x0 , 
			0x0,		/* 0 */
/* 3268 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3272 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3276 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3278) */
/* 3278 */	NdrFcShort( 0x20 ),	/* 32 */
/* 3280 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3282 */	NdrFcLong( 0x1 ),	/* 1 */
/* 3286 */	NdrFcShort( 0x38 ),	/* Offset= 56 (3342) */
/* 3288 */	NdrFcShort( 0xffff ),	/* Offset= -1 (3287) */
/* 3290 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 3292 */	NdrFcLong( 0x1 ),	/* 1 */
/* 3296 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 3300 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 3302 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3304 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 3306 */	NdrFcShort( 0x14 ),	/* 20 */
/* 3308 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3310 */	0x0 , 
			0x0,		/* 0 */
/* 3312 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3316 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3320 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 3324 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 3326 */	0x0 , 
			0x0,		/* 0 */
/* 3328 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3332 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3336 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3338 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3340 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 3342 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 3344 */	NdrFcShort( 0x20 ),	/* 32 */
/* 3346 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3348 */	NdrFcShort( 0xe ),	/* Offset= 14 (3362) */
/* 3350 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 3352 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 3354 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 3356 */	0x0,		/* 0 */
			NdrFcShort( 0xffbd ),	/* Offset= -67 (3290) */
			0x36,		/* FC_POINTER */
/* 3360 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 3362 */	
			0x12, 0x0,	/* FC_UP */
/* 3364 */	NdrFcShort( 0xffc0 ),	/* Offset= -64 (3300) */
/* 3366 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 3368 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3370) */
/* 3370 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 3372 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 3374 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 3376 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3378 */	0x0 , 
			0x0,		/* 0 */
/* 3380 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3384 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3388 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3390) */
/* 3390 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3392 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3394 */	NdrFcLong( 0x1 ),	/* 1 */
/* 3398 */	NdrFcShort( 0x54 ),	/* Offset= 84 (3482) */
/* 3400 */	NdrFcShort( 0xffff ),	/* Offset= -1 (3399) */
/* 3402 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 3404 */	NdrFcShort( 0x18 ),	/* 24 */
/* 3406 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3408 */	NdrFcShort( 0x8 ),	/* Offset= 8 (3416) */
/* 3410 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 3412 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 3414 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 3416 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3418 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3420 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3422 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3424 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 3426 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3428 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 3430 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3432 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3434 */	0x0 , 
			0x0,		/* 0 */
/* 3436 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3440 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3444 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 3448 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 3450 */	0x0 , 
			0x0,		/* 0 */
/* 3452 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3456 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3460 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 3462 */	NdrFcShort( 0xffc4 ),	/* Offset= -60 (3402) */
/* 3464 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 3466 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 3468 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3470 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3472 */	NdrFcShort( 0x6 ),	/* Offset= 6 (3478) */
/* 3474 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 3476 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 3478 */	
			0x12, 0x0,	/* FC_UP */
/* 3480 */	NdrFcShort( 0xffc8 ),	/* Offset= -56 (3424) */
/* 3482 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 3484 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3486 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3488 */	NdrFcShort( 0x4 ),	/* Offset= 4 (3492) */
/* 3490 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 3492 */	
			0x12, 0x0,	/* FC_UP */
/* 3494 */	NdrFcShort( 0xffe4 ),	/* Offset= -28 (3466) */
/* 3496 */	
			0x11, 0x0,	/* FC_RP */
/* 3498 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3500) */
/* 3500 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 3502 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 3504 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 3506 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3508 */	0x0 , 
			0x0,		/* 0 */
/* 3510 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3514 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3518 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3520) */
/* 3520 */	NdrFcShort( 0x20 ),	/* 32 */
/* 3522 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3524 */	NdrFcLong( 0x1 ),	/* 1 */
/* 3528 */	NdrFcShort( 0x38 ),	/* Offset= 56 (3584) */
/* 3530 */	NdrFcShort( 0xffff ),	/* Offset= -1 (3529) */
/* 3532 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 3534 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3538 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 3542 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 3544 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3546 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 3548 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3550 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3552 */	0x0 , 
			0x0,		/* 0 */
/* 3554 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3558 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3562 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 3566 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 3568 */	0x0 , 
			0x0,		/* 0 */
/* 3570 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3574 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3578 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3580 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3582 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 3584 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 3586 */	NdrFcShort( 0x20 ),	/* 32 */
/* 3588 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3590 */	NdrFcShort( 0xc ),	/* Offset= 12 (3602) */
/* 3592 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 3594 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 3596 */	0x0,		/* 0 */
			NdrFcShort( 0xffbf ),	/* Offset= -65 (3532) */
			0x40,		/* FC_STRUCTPAD4 */
/* 3600 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 3602 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3604 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3606 */	
			0x12, 0x0,	/* FC_UP */
/* 3608 */	NdrFcShort( 0xffbe ),	/* Offset= -66 (3542) */
/* 3610 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 3612 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3614) */
/* 3614 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 3616 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 3618 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 3620 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3622 */	0x0 , 
			0x0,		/* 0 */
/* 3624 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3628 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3632 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3634) */
/* 3634 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3636 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3638 */	NdrFcLong( 0x1 ),	/* 1 */
/* 3642 */	NdrFcShort( 0x4 ),	/* Offset= 4 (3646) */
/* 3644 */	NdrFcShort( 0xffff ),	/* Offset= -1 (3643) */
/* 3646 */	
			0x15,		/* FC_STRUCT */
			0x3,		/* 3 */
/* 3648 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3650 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 3652 */	
			0x11, 0x0,	/* FC_RP */
/* 3654 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3656) */
/* 3656 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 3658 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 3660 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 3662 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3664 */	0x0 , 
			0x0,		/* 0 */
/* 3666 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3670 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3674 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3676) */
/* 3676 */	NdrFcShort( 0x18 ),	/* 24 */
/* 3678 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3680 */	NdrFcLong( 0x1 ),	/* 1 */
/* 3684 */	NdrFcShort( 0x4 ),	/* Offset= 4 (3688) */
/* 3686 */	NdrFcShort( 0xffff ),	/* Offset= -1 (3685) */
/* 3688 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 3690 */	NdrFcShort( 0x18 ),	/* 24 */
/* 3692 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3694 */	NdrFcShort( 0x8 ),	/* Offset= 8 (3702) */
/* 3696 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 3698 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 3700 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 3702 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3704 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3706 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3708 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3710 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 3712 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3714) */
/* 3714 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 3716 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 3718 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 3720 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3722 */	0x0 , 
			0x0,		/* 0 */
/* 3724 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3728 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3732 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3734) */
/* 3734 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3736 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3738 */	NdrFcLong( 0x1 ),	/* 1 */
/* 3742 */	NdrFcShort( 0xffa0 ),	/* Offset= -96 (3646) */
/* 3744 */	NdrFcShort( 0xffff ),	/* Offset= -1 (3743) */
/* 3746 */	
			0x11, 0x0,	/* FC_RP */
/* 3748 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3750) */
/* 3750 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 3752 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 3754 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 3756 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3758 */	0x0 , 
			0x0,		/* 0 */
/* 3760 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3764 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3768 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3770) */
/* 3770 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3772 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3774 */	NdrFcLong( 0x1 ),	/* 1 */
/* 3778 */	NdrFcShort( 0x4 ),	/* Offset= 4 (3782) */
/* 3780 */	NdrFcShort( 0xffff ),	/* Offset= -1 (3779) */
/* 3782 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 3784 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3786 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3788 */	NdrFcShort( 0x4 ),	/* Offset= 4 (3792) */
/* 3790 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 3792 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3794 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3796 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 3798 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3800) */
/* 3800 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 3802 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 3804 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 3806 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3808 */	0x0 , 
			0x0,		/* 0 */
/* 3810 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3814 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3818 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3820) */
/* 3820 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3822 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3824 */	NdrFcLong( 0x1 ),	/* 1 */
/* 3828 */	NdrFcShort( 0xff4a ),	/* Offset= -182 (3646) */
/* 3830 */	NdrFcShort( 0xffff ),	/* Offset= -1 (3829) */
/* 3832 */	
			0x11, 0x0,	/* FC_RP */
/* 3834 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3836) */
/* 3836 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 3838 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 3840 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 3842 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3844 */	0x0 , 
			0x0,		/* 0 */
/* 3846 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3850 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3854 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3856) */
/* 3856 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3858 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3860 */	NdrFcLong( 0x1 ),	/* 1 */
/* 3864 */	NdrFcShort( 0x4 ),	/* Offset= 4 (3868) */
/* 3866 */	NdrFcShort( 0xffff ),	/* Offset= -1 (3865) */
/* 3868 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 3870 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3872 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3874 */	NdrFcShort( 0x6 ),	/* Offset= 6 (3880) */
/* 3876 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 3878 */	0x40,		/* FC_STRUCTPAD4 */
			0x5b,		/* FC_END */
/* 3880 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3882 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3884 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 3886 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3888) */
/* 3888 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 3890 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 3892 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 3894 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3896 */	0x0 , 
			0x0,		/* 0 */
/* 3898 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3902 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3906 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3908) */
/* 3908 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3910 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3912 */	NdrFcLong( 0x1 ),	/* 1 */
/* 3916 */	NdrFcShort( 0x6e ),	/* Offset= 110 (4026) */
/* 3918 */	NdrFcLong( 0x2 ),	/* 2 */
/* 3922 */	NdrFcShort( 0xf0 ),	/* Offset= 240 (4162) */
/* 3924 */	NdrFcLong( 0x3 ),	/* 3 */
/* 3928 */	NdrFcShort( 0x172 ),	/* Offset= 370 (4298) */
/* 3930 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 3934 */	NdrFcShort( 0x1c8 ),	/* Offset= 456 (4390) */
/* 3936 */	NdrFcShort( 0xffff ),	/* Offset= -1 (3935) */
/* 3938 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 3940 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3944 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 3948 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 3950 */	NdrFcShort( 0x30 ),	/* 48 */
/* 3952 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3954 */	NdrFcShort( 0xa ),	/* Offset= 10 (3964) */
/* 3956 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 3958 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 3960 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 3962 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 3964 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3966 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3968 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3970 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3972 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3974 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3976 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3978 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3980 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3982 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 3984 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 3986 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3988 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 3990 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3992 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3994 */	0x0 , 
			0x0,		/* 0 */
/* 3996 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4000 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4004 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 4008 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 4010 */	0x0 , 
			0x0,		/* 0 */
/* 4012 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4016 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4020 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4022 */	NdrFcShort( 0xffb6 ),	/* Offset= -74 (3948) */
/* 4024 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4026 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4028 */	NdrFcShort( 0x10 ),	/* 16 */
/* 4030 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4032 */	NdrFcShort( 0xa ),	/* Offset= 10 (4042) */
/* 4034 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4036 */	NdrFcShort( 0xff9e ),	/* Offset= -98 (3938) */
/* 4038 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 4040 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4042 */	
			0x12, 0x0,	/* FC_UP */
/* 4044 */	NdrFcShort( 0xffc4 ),	/* Offset= -60 (3984) */
/* 4046 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 4048 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4052 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 4056 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4058 */	NdrFcShort( 0x88 ),	/* 136 */
/* 4060 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4062 */	NdrFcShort( 0x1e ),	/* Offset= 30 (4092) */
/* 4064 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 4066 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 4068 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 4070 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 4072 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 4074 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4076 */	NdrFcShort( 0xf020 ),	/* Offset= -4064 (12) */
/* 4078 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4080 */	NdrFcShort( 0xf01c ),	/* Offset= -4068 (12) */
/* 4082 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4084 */	NdrFcShort( 0xf018 ),	/* Offset= -4072 (12) */
/* 4086 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4088 */	NdrFcShort( 0xf014 ),	/* Offset= -4076 (12) */
/* 4090 */	0x40,		/* FC_STRUCTPAD4 */
			0x5b,		/* FC_END */
/* 4092 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 4094 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 4096 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 4098 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 4100 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 4102 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 4104 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 4106 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 4108 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 4110 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 4112 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 4114 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 4116 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 4118 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 4120 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 4122 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4124 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 4126 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4128 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 4130 */	0x0 , 
			0x0,		/* 0 */
/* 4132 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4136 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4140 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 4144 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 4146 */	0x0 , 
			0x0,		/* 0 */
/* 4148 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4152 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4156 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4158 */	NdrFcShort( 0xff9a ),	/* Offset= -102 (4056) */
/* 4160 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4162 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4164 */	NdrFcShort( 0x10 ),	/* 16 */
/* 4166 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4168 */	NdrFcShort( 0xa ),	/* Offset= 10 (4178) */
/* 4170 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4172 */	NdrFcShort( 0xff82 ),	/* Offset= -126 (4046) */
/* 4174 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 4176 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4178 */	
			0x12, 0x0,	/* FC_UP */
/* 4180 */	NdrFcShort( 0xffc4 ),	/* Offset= -60 (4120) */
/* 4182 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 4184 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4188 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 4192 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4194 */	NdrFcShort( 0x88 ),	/* 136 */
/* 4196 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4198 */	NdrFcShort( 0x1e ),	/* Offset= 30 (4228) */
/* 4200 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 4202 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 4204 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 4206 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 4208 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 4210 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 4212 */	0x0,		/* 0 */
			NdrFcShort( 0xef97 ),	/* Offset= -4201 (12) */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 4216 */	0x0,		/* 0 */
			NdrFcShort( 0xef93 ),	/* Offset= -4205 (12) */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 4220 */	0x0,		/* 0 */
			NdrFcShort( 0xef8f ),	/* Offset= -4209 (12) */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 4224 */	0x0,		/* 0 */
			NdrFcShort( 0xef8b ),	/* Offset= -4213 (12) */
			0x5b,		/* FC_END */
/* 4228 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 4230 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 4232 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 4234 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 4236 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 4238 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 4240 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 4242 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 4244 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 4246 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 4248 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 4250 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 4252 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 4254 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 4256 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 4258 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4260 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 4262 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4264 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 4266 */	0x0 , 
			0x0,		/* 0 */
/* 4268 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4272 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4276 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 4280 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 4282 */	0x0 , 
			0x0,		/* 0 */
/* 4284 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4288 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4292 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4294 */	NdrFcShort( 0xff9a ),	/* Offset= -102 (4192) */
/* 4296 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4298 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4300 */	NdrFcShort( 0x10 ),	/* 16 */
/* 4302 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4304 */	NdrFcShort( 0xa ),	/* Offset= 10 (4314) */
/* 4306 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4308 */	NdrFcShort( 0xff82 ),	/* Offset= -126 (4182) */
/* 4310 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 4312 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4314 */	
			0x12, 0x0,	/* FC_UP */
/* 4316 */	NdrFcShort( 0xffc4 ),	/* Offset= -60 (4256) */
/* 4318 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 4320 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4324 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 4328 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4330 */	NdrFcShort( 0x20 ),	/* 32 */
/* 4332 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4334 */	NdrFcShort( 0xa ),	/* Offset= 10 (4344) */
/* 4336 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 4338 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 4340 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 4342 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 4344 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 4346 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 4348 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 4350 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4352 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 4354 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4356 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 4358 */	0x0 , 
			0x0,		/* 0 */
/* 4360 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4364 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4368 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 4372 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 4374 */	0x0 , 
			0x0,		/* 0 */
/* 4376 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4380 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4384 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4386 */	NdrFcShort( 0xffc6 ),	/* Offset= -58 (4328) */
/* 4388 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4390 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4392 */	NdrFcShort( 0x10 ),	/* 16 */
/* 4394 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4396 */	NdrFcShort( 0xa ),	/* Offset= 10 (4406) */
/* 4398 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4400 */	NdrFcShort( 0xffae ),	/* Offset= -82 (4318) */
/* 4402 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 4404 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4406 */	
			0x12, 0x0,	/* FC_UP */
/* 4408 */	NdrFcShort( 0xffc4 ),	/* Offset= -60 (4348) */
/* 4410 */	
			0x11, 0x0,	/* FC_RP */
/* 4412 */	NdrFcShort( 0x2 ),	/* Offset= 2 (4414) */
/* 4414 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 4416 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 4418 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 4420 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 4422 */	0x0 , 
			0x0,		/* 0 */
/* 4424 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4428 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4432 */	NdrFcShort( 0x2 ),	/* Offset= 2 (4434) */
/* 4434 */	NdrFcShort( 0x30 ),	/* 48 */
/* 4436 */	NdrFcShort( 0x3 ),	/* 3 */
/* 4438 */	NdrFcLong( 0x1 ),	/* 1 */
/* 4442 */	NdrFcShort( 0x10 ),	/* Offset= 16 (4458) */
/* 4444 */	NdrFcLong( 0x2 ),	/* 2 */
/* 4448 */	NdrFcShort( 0x2e ),	/* Offset= 46 (4494) */
/* 4450 */	NdrFcLong( 0x3 ),	/* 3 */
/* 4454 */	NdrFcShort( 0x36 ),	/* Offset= 54 (4508) */
/* 4456 */	NdrFcShort( 0xffff ),	/* Offset= -1 (4455) */
/* 4458 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4460 */	NdrFcShort( 0x18 ),	/* 24 */
/* 4462 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4464 */	NdrFcShort( 0x8 ),	/* Offset= 8 (4472) */
/* 4466 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 4468 */	0x0,		/* 0 */
			NdrFcShort( 0xf333 ),	/* Offset= -3277 (1192) */
			0x5b,		/* FC_END */
/* 4472 */	
			0x11, 0x0,	/* FC_RP */
/* 4474 */	NdrFcShort( 0xef38 ),	/* Offset= -4296 (178) */
/* 4476 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4478 */	NdrFcShort( 0x28 ),	/* 40 */
/* 4480 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4482 */	NdrFcShort( 0x8 ),	/* Offset= 8 (4490) */
/* 4484 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 4486 */	0x0,		/* 0 */
			NdrFcShort( 0xf335 ),	/* Offset= -3275 (1212) */
			0x5b,		/* FC_END */
/* 4490 */	
			0x12, 0x0,	/* FC_UP */
/* 4492 */	NdrFcShort( 0xfff0 ),	/* Offset= -16 (4476) */
/* 4494 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4496 */	NdrFcShort( 0x28 ),	/* 40 */
/* 4498 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4500 */	NdrFcShort( 0x0 ),	/* Offset= 0 (4500) */
/* 4502 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4504 */	NdrFcShort( 0xffe4 ),	/* Offset= -28 (4476) */
/* 4506 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4508 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4510 */	NdrFcShort( 0x30 ),	/* 48 */
/* 4512 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4514 */	NdrFcShort( 0x8 ),	/* Offset= 8 (4522) */
/* 4516 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4518 */	NdrFcShort( 0xffd6 ),	/* Offset= -42 (4476) */
/* 4520 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 4522 */	
			0x12, 0x0,	/* FC_UP */
/* 4524 */	NdrFcShort( 0xf9a6 ),	/* Offset= -1626 (2898) */
/* 4526 */	
			0x11, 0x0,	/* FC_RP */
/* 4528 */	NdrFcShort( 0x2 ),	/* Offset= 2 (4530) */
/* 4530 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 4532 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 4534 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 4536 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 4538 */	0x0 , 
			0x0,		/* 0 */
/* 4540 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4544 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4548 */	NdrFcShort( 0x2 ),	/* Offset= 2 (4550) */
/* 4550 */	NdrFcShort( 0x40 ),	/* 64 */
/* 4552 */	NdrFcShort( 0x3 ),	/* 3 */
/* 4554 */	NdrFcLong( 0x1 ),	/* 1 */
/* 4558 */	NdrFcShort( 0x10 ),	/* Offset= 16 (4574) */
/* 4560 */	NdrFcLong( 0x2 ),	/* 2 */
/* 4564 */	NdrFcShort( 0x64 ),	/* Offset= 100 (4664) */
/* 4566 */	NdrFcLong( 0x3 ),	/* 3 */
/* 4570 */	NdrFcShort( 0x20a ),	/* Offset= 522 (5092) */
/* 4572 */	NdrFcShort( 0xffff ),	/* Offset= -1 (4571) */
/* 4574 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4576 */	NdrFcShort( 0x40 ),	/* 64 */
/* 4578 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4580 */	NdrFcShort( 0x0 ),	/* Offset= 0 (4580) */
/* 4582 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4584 */	NdrFcShort( 0xee24 ),	/* Offset= -4572 (12) */
/* 4586 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4588 */	NdrFcShort( 0xeea6 ),	/* Offset= -4442 (146) */
/* 4590 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 4592 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 4594 */	0x6,		/* FC_SHORT */
			0x3e,		/* FC_STRUCTPAD2 */
/* 4596 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4598 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 4600 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4604 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 4608 */	
			0x15,		/* FC_STRUCT */
			0x3,		/* 3 */
/* 4610 */	NdrFcShort( 0x2c ),	/* 44 */
/* 4612 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4614 */	NdrFcShort( 0xee06 ),	/* Offset= -4602 (12) */
/* 4616 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4618 */	NdrFcShort( 0xee88 ),	/* Offset= -4472 (146) */
/* 4620 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4622 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 4624 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4626 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 4628 */	NdrFcShort( 0x1c ),	/* 28 */
/* 4630 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 4632 */	0x0 , 
			0x0,		/* 0 */
/* 4634 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4638 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4642 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 4646 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 4648 */	0x0 , 
			0x0,		/* 0 */
/* 4650 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4654 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4658 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 4660 */	NdrFcShort( 0xffcc ),	/* Offset= -52 (4608) */
/* 4662 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4664 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4666 */	NdrFcShort( 0x28 ),	/* 40 */
/* 4668 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4670 */	NdrFcShort( 0x10 ),	/* Offset= 16 (4686) */
/* 4672 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 4674 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 4676 */	0x8,		/* FC_LONG */
			0x6,		/* FC_SHORT */
/* 4678 */	0x3e,		/* FC_STRUCTPAD2 */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 4680 */	0x0,		/* 0 */
			NdrFcShort( 0xffad ),	/* Offset= -83 (4598) */
			0x36,		/* FC_POINTER */
/* 4684 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4686 */	
			0x12, 0x0,	/* FC_UP */
/* 4688 */	NdrFcShort( 0xee62 ),	/* Offset= -4510 (178) */
/* 4690 */	
			0x12, 0x0,	/* FC_UP */
/* 4692 */	NdrFcShort( 0xffba ),	/* Offset= -70 (4622) */
/* 4694 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 4696 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4700 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 4704 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 4706 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 4708 */	NdrFcShort( 0x8 ),	/* 8 */
/* 4710 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 4712 */	0x0 , 
			0x0,		/* 0 */
/* 4714 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4718 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4722 */	NdrFcShort( 0x2 ),	/* Offset= 2 (4724) */
/* 4724 */	NdrFcShort( 0x10 ),	/* 16 */
/* 4726 */	NdrFcShort( 0x1 ),	/* 1 */
/* 4728 */	NdrFcLong( 0x1 ),	/* 1 */
/* 4732 */	NdrFcShort( 0x12e ),	/* Offset= 302 (5034) */
/* 4734 */	NdrFcShort( 0xffff ),	/* Offset= -1 (4733) */
/* 4736 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 4738 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 4740 */	NdrFcShort( 0x4 ),	/* 4 */
/* 4742 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 4744 */	0x0 , 
			0x0,		/* 0 */
/* 4746 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4750 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4754 */	NdrFcShort( 0x2 ),	/* Offset= 2 (4756) */
/* 4756 */	NdrFcShort( 0x40 ),	/* 64 */
/* 4758 */	NdrFcShort( 0x7 ),	/* 7 */
/* 4760 */	NdrFcLong( 0x1 ),	/* 1 */
/* 4764 */	NdrFcShort( 0x4e ),	/* Offset= 78 (4842) */
/* 4766 */	NdrFcLong( 0x2 ),	/* 2 */
/* 4770 */	NdrFcShort( 0x5c ),	/* Offset= 92 (4862) */
/* 4772 */	NdrFcLong( 0x3 ),	/* 3 */
/* 4776 */	NdrFcShort( 0xe2 ),	/* Offset= 226 (5002) */
/* 4778 */	NdrFcLong( 0x4 ),	/* 4 */
/* 4782 */	NdrFcShort( 0xee ),	/* Offset= 238 (5020) */
/* 4784 */	NdrFcLong( 0x5 ),	/* 5 */
/* 4788 */	NdrFcShort( 0xe8 ),	/* Offset= 232 (5020) */
/* 4790 */	NdrFcLong( 0x6 ),	/* 6 */
/* 4794 */	NdrFcShort( 0xe2 ),	/* Offset= 226 (5020) */
/* 4796 */	NdrFcLong( 0x7 ),	/* 7 */
/* 4800 */	NdrFcShort( 0xdc ),	/* Offset= 220 (5020) */
/* 4802 */	NdrFcShort( 0xffff ),	/* Offset= -1 (4801) */
/* 4804 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4806 */	NdrFcShort( 0x28 ),	/* 40 */
/* 4808 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4810 */	NdrFcShort( 0x0 ),	/* Offset= 0 (4810) */
/* 4812 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 4814 */	0x8,		/* FC_LONG */
			0x6,		/* FC_SHORT */
/* 4816 */	0x3e,		/* FC_STRUCTPAD2 */
			0x8,		/* FC_LONG */
/* 4818 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 4820 */	0x0,		/* 0 */
			NdrFcShort( 0xf147 ),	/* Offset= -3769 (1052) */
			0x5b,		/* FC_END */
/* 4824 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4826 */	NdrFcShort( 0x30 ),	/* 48 */
/* 4828 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4830 */	NdrFcShort( 0x8 ),	/* Offset= 8 (4838) */
/* 4832 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 4834 */	0x0,		/* 0 */
			NdrFcShort( 0xffe1 ),	/* Offset= -31 (4804) */
			0x5b,		/* FC_END */
/* 4838 */	
			0x12, 0x0,	/* FC_UP */
/* 4840 */	NdrFcShort( 0xfff0 ),	/* Offset= -16 (4824) */
/* 4842 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4844 */	NdrFcShort( 0x40 ),	/* 64 */
/* 4846 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4848 */	NdrFcShort( 0xa ),	/* Offset= 10 (4858) */
/* 4850 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 4852 */	0x40,		/* FC_STRUCTPAD4 */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 4854 */	0x0,		/* 0 */
			NdrFcShort( 0xffe1 ),	/* Offset= -31 (4824) */
			0x5b,		/* FC_END */
/* 4858 */	
			0x12, 0x0,	/* FC_UP */
/* 4860 */	NdrFcShort( 0xedb6 ),	/* Offset= -4682 (178) */
/* 4862 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4864 */	NdrFcShort( 0x18 ),	/* 24 */
/* 4866 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4868 */	NdrFcShort( 0xa ),	/* Offset= 10 (4878) */
/* 4870 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 4872 */	0x8,		/* FC_LONG */
			0x6,		/* FC_SHORT */
/* 4874 */	0x3e,		/* FC_STRUCTPAD2 */
			0x36,		/* FC_POINTER */
/* 4876 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4878 */	
			0x12, 0x0,	/* FC_UP */
/* 4880 */	NdrFcShort( 0xeda2 ),	/* Offset= -4702 (178) */
/* 4882 */	
			0x15,		/* FC_STRUCT */
			0x1,		/* 1 */
/* 4884 */	NdrFcShort( 0x4 ),	/* 4 */
/* 4886 */	0x2,		/* FC_CHAR */
			0x2,		/* FC_CHAR */
/* 4888 */	0x6,		/* FC_SHORT */
			0x5b,		/* FC_END */
/* 4890 */	
			0x1c,		/* FC_CVARRAY */
			0x1,		/* 1 */
/* 4892 */	NdrFcShort( 0x2 ),	/* 2 */
/* 4894 */	0x17,		/* Corr desc:  field pointer, FC_USHORT */
			0x55,		/* FC_DIV_2 */
/* 4896 */	NdrFcShort( 0x2 ),	/* 2 */
/* 4898 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 4900 */	0x0 , 
			0x0,		/* 0 */
/* 4902 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4906 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4910 */	0x17,		/* Corr desc:  field pointer, FC_USHORT */
			0x55,		/* FC_DIV_2 */
/* 4912 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4914 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 4916 */	0x0 , 
			0x0,		/* 0 */
/* 4918 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4922 */	NdrFcLong( 0x0 ),	/* 0 */
/* 4926 */	0x5,		/* FC_WCHAR */
			0x5b,		/* FC_END */
/* 4928 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4930 */	NdrFcShort( 0x10 ),	/* 16 */
/* 4932 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4934 */	NdrFcShort( 0x8 ),	/* Offset= 8 (4942) */
/* 4936 */	0x6,		/* FC_SHORT */
			0x6,		/* FC_SHORT */
/* 4938 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 4940 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4942 */	
			0x12, 0x0,	/* FC_UP */
/* 4944 */	NdrFcShort( 0xffca ),	/* Offset= -54 (4890) */
/* 4946 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4948 */	NdrFcShort( 0x10 ),	/* 16 */
/* 4950 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4952 */	NdrFcShort( 0x6 ),	/* Offset= 6 (4958) */
/* 4954 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 4956 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 4958 */	
			0x12, 0x0,	/* FC_UP */
/* 4960 */	NdrFcShort( 0xfff2 ),	/* Offset= -14 (4946) */
/* 4962 */	
			0x12, 0x0,	/* FC_UP */
/* 4964 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (4928) */
/* 4966 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 4968 */	NdrFcShort( 0x30 ),	/* 48 */
/* 4970 */	NdrFcShort( 0x0 ),	/* 0 */
/* 4972 */	NdrFcShort( 0x12 ),	/* Offset= 18 (4990) */
/* 4974 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 4976 */	0x0,		/* 0 */
			NdrFcShort( 0xffa1 ),	/* Offset= -95 (4882) */
			0x6,		/* FC_SHORT */
/* 4980 */	0x6,		/* FC_SHORT */
			0x6,		/* FC_SHORT */
/* 4982 */	0x6,		/* FC_SHORT */
			0x40,		/* FC_STRUCTPAD4 */
/* 4984 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 4986 */	0x8,		/* FC_LONG */
			0x2,		/* FC_CHAR */
/* 4988 */	0x3f,		/* FC_STRUCTPAD3 */
			0x5b,		/* FC_END */
/* 4990 */	
			0x12, 0x0,	/* FC_UP */
/* 4992 */	NdrFcShort( 0xed32 ),	/* Offset= -4814 (178) */
/* 4994 */	
			0x12, 0x0,	/* FC_UP */
/* 4996 */	NdrFcShort( 0xffce ),	/* Offset= -50 (4946) */
/* 4998 */	
			0x12, 0x0,	/* FC_UP */
/* 5000 */	NdrFcShort( 0xffde ),	/* Offset= -34 (4966) */
/* 5002 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 5004 */	NdrFcShort( 0x40 ),	/* 64 */
/* 5006 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5008 */	NdrFcShort( 0x0 ),	/* Offset= 0 (5008) */
/* 5010 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 5012 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 5014 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5016 */	NdrFcShort( 0xffce ),	/* Offset= -50 (4966) */
/* 5018 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5020 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 5022 */	NdrFcShort( 0x10 ),	/* 16 */
/* 5024 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5026 */	NdrFcShort( 0x0 ),	/* Offset= 0 (5026) */
/* 5028 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 5030 */	0x8,		/* FC_LONG */
			0x6,		/* FC_SHORT */
/* 5032 */	0x3e,		/* FC_STRUCTPAD2 */
			0x5b,		/* FC_END */
/* 5034 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 5036 */	NdrFcShort( 0x10 ),	/* 16 */
/* 5038 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5040 */	NdrFcShort( 0x6 ),	/* Offset= 6 (5046) */
/* 5042 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 5044 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 5046 */	
			0x12, 0x0,	/* FC_UP */
/* 5048 */	NdrFcShort( 0xfec8 ),	/* Offset= -312 (4736) */
/* 5050 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 5052 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5054 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 5056 */	NdrFcShort( 0x18 ),	/* 24 */
/* 5058 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 5060 */	0x0 , 
			0x0,		/* 0 */
/* 5062 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5066 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5070 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 5074 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 5076 */	0x0 , 
			0x0,		/* 0 */
/* 5078 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5082 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5086 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5088 */	NdrFcShort( 0xfe20 ),	/* Offset= -480 (4608) */
/* 5090 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5092 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 5094 */	NdrFcShort( 0x28 ),	/* 40 */
/* 5096 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5098 */	NdrFcShort( 0xe ),	/* Offset= 14 (5112) */
/* 5100 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 5102 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 5104 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5106 */	NdrFcShort( 0xfe64 ),	/* Offset= -412 (4694) */
/* 5108 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 5110 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5112 */	
			0x12, 0x0,	/* FC_UP */
/* 5114 */	NdrFcShort( 0xecb8 ),	/* Offset= -4936 (178) */
/* 5116 */	
			0x12, 0x0,	/* FC_UP */
/* 5118 */	NdrFcShort( 0xfe62 ),	/* Offset= -414 (4704) */
/* 5120 */	
			0x12, 0x0,	/* FC_UP */
/* 5122 */	NdrFcShort( 0xffb8 ),	/* Offset= -72 (5050) */
/* 5124 */	
			0x11, 0x0,	/* FC_RP */
/* 5126 */	NdrFcShort( 0x2 ),	/* Offset= 2 (5128) */
/* 5128 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 5130 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 5132 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 5134 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 5136 */	0x0 , 
			0x0,		/* 0 */
/* 5138 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5142 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5146 */	NdrFcShort( 0x2 ),	/* Offset= 2 (5148) */
/* 5148 */	NdrFcShort( 0x8 ),	/* 8 */
/* 5150 */	NdrFcShort( 0x1 ),	/* 1 */
/* 5152 */	NdrFcLong( 0x1 ),	/* 1 */
/* 5156 */	NdrFcShort( 0x4 ),	/* Offset= 4 (5160) */
/* 5158 */	NdrFcShort( 0xffff ),	/* Offset= -1 (5157) */
/* 5160 */	
			0x15,		/* FC_STRUCT */
			0x3,		/* 3 */
/* 5162 */	NdrFcShort( 0x8 ),	/* 8 */
/* 5164 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 5166 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5168 */	
			0x11, 0x0,	/* FC_RP */
/* 5170 */	NdrFcShort( 0x2 ),	/* Offset= 2 (5172) */
/* 5172 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 5174 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 5176 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 5178 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 5180 */	0x0 , 
			0x0,		/* 0 */
/* 5182 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5186 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5190 */	NdrFcShort( 0x2 ),	/* Offset= 2 (5192) */
/* 5192 */	NdrFcShort( 0x40 ),	/* 64 */
/* 5194 */	NdrFcShort( 0x2 ),	/* 2 */
/* 5196 */	NdrFcLong( 0x1 ),	/* 1 */
/* 5200 */	NdrFcShort( 0xa ),	/* Offset= 10 (5210) */
/* 5202 */	NdrFcLong( 0x2 ),	/* 2 */
/* 5206 */	NdrFcShort( 0x18 ),	/* Offset= 24 (5230) */
/* 5208 */	NdrFcShort( 0xffff ),	/* Offset= -1 (5207) */
/* 5210 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 5212 */	NdrFcShort( 0x20 ),	/* 32 */
/* 5214 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5216 */	NdrFcShort( 0xa ),	/* Offset= 10 (5226) */
/* 5218 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 5220 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 5222 */	0x0,		/* 0 */
			NdrFcShort( 0xeba5 ),	/* Offset= -5211 (12) */
			0x5b,		/* FC_END */
/* 5226 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5228 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5230 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 5232 */	NdrFcShort( 0x40 ),	/* 64 */
/* 5234 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5236 */	NdrFcShort( 0x10 ),	/* Offset= 16 (5252) */
/* 5238 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 5240 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 5242 */	0x0,		/* 0 */
			NdrFcShort( 0xeb91 ),	/* Offset= -5231 (12) */
			0x8,		/* FC_LONG */
/* 5246 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 5248 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 5250 */	0x40,		/* FC_STRUCTPAD4 */
			0x5b,		/* FC_END */
/* 5252 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5254 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5256 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5258 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5260 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5262 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5264 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 5266 */	NdrFcShort( 0x2 ),	/* Offset= 2 (5268) */
/* 5268 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 5270 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 5272 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 5274 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 5276 */	0x0 , 
			0x0,		/* 0 */
/* 5278 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5282 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5286 */	NdrFcShort( 0x2 ),	/* Offset= 2 (5288) */
/* 5288 */	NdrFcShort( 0x8 ),	/* 8 */
/* 5290 */	NdrFcShort( 0xf ),	/* 15 */
/* 5292 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5296 */	NdrFcShort( 0x58 ),	/* Offset= 88 (5384) */
/* 5298 */	NdrFcLong( 0x1 ),	/* 1 */
/* 5302 */	NdrFcShort( 0xc8 ),	/* Offset= 200 (5502) */
/* 5304 */	NdrFcLong( 0x2 ),	/* 2 */
/* 5308 */	NdrFcShort( 0xd0 ),	/* Offset= 208 (5516) */
/* 5310 */	NdrFcLong( 0x3 ),	/* 3 */
/* 5314 */	NdrFcShort( 0x11e ),	/* Offset= 286 (5600) */
/* 5316 */	NdrFcLong( 0x4 ),	/* 4 */
/* 5320 */	NdrFcShort( 0x118 ),	/* Offset= 280 (5600) */
/* 5322 */	NdrFcLong( 0x5 ),	/* 5 */
/* 5326 */	NdrFcShort( 0x164 ),	/* Offset= 356 (5682) */
/* 5328 */	NdrFcLong( 0x6 ),	/* 6 */
/* 5332 */	NdrFcShort( 0x1c4 ),	/* Offset= 452 (5784) */
/* 5334 */	NdrFcLong( 0x7 ),	/* 7 */
/* 5338 */	NdrFcShort( 0x23c ),	/* Offset= 572 (5910) */
/* 5340 */	NdrFcLong( 0x8 ),	/* 8 */
/* 5344 */	NdrFcShort( 0x26c ),	/* Offset= 620 (5964) */
/* 5346 */	NdrFcLong( 0x9 ),	/* 9 */
/* 5350 */	NdrFcShort( 0x2b8 ),	/* Offset= 696 (6046) */
/* 5352 */	NdrFcLong( 0xa ),	/* 10 */
/* 5356 */	NdrFcShort( 0x30c ),	/* Offset= 780 (6136) */
/* 5358 */	NdrFcLong( 0xfffffffa ),	/* -6 */
/* 5362 */	NdrFcShort( 0x374 ),	/* Offset= 884 (6246) */
/* 5364 */	NdrFcLong( 0xfffffffb ),	/* -5 */
/* 5368 */	NdrFcShort( 0x3cc ),	/* Offset= 972 (6340) */
/* 5370 */	NdrFcLong( 0xfffffffc ),	/* -4 */
/* 5374 */	NdrFcShort( 0x3ca ),	/* Offset= 970 (6344) */
/* 5376 */	NdrFcLong( 0xfffffffe ),	/* -2 */
/* 5380 */	NdrFcShort( 0x4 ),	/* Offset= 4 (5384) */
/* 5382 */	NdrFcShort( 0xffff ),	/* Offset= -1 (5381) */
/* 5384 */	
			0x12, 0x0,	/* FC_UP */
/* 5386 */	NdrFcShort( 0x68 ),	/* Offset= 104 (5490) */
/* 5388 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 5390 */	NdrFcShort( 0x90 ),	/* 144 */
/* 5392 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5394 */	NdrFcShort( 0x26 ),	/* Offset= 38 (5432) */
/* 5396 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 5398 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 5400 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 5402 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5404 */	NdrFcShort( 0xeaf0 ),	/* Offset= -5392 (12) */
/* 5406 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5408 */	NdrFcShort( 0xeaec ),	/* Offset= -5396 (12) */
/* 5410 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5412 */	NdrFcShort( 0xeae8 ),	/* Offset= -5400 (12) */
/* 5414 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5416 */	NdrFcShort( 0xeae4 ),	/* Offset= -5404 (12) */
/* 5418 */	0xb,		/* FC_HYPER */
			0xb,		/* FC_HYPER */
/* 5420 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5422 */	NdrFcShort( 0xfefa ),	/* Offset= -262 (5160) */
/* 5424 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5426 */	NdrFcShort( 0xfef6 ),	/* Offset= -266 (5160) */
/* 5428 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 5430 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5432 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5434 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5436 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5438 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5440 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5442 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5444 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5446 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5448 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x7,		/* 7 */
/* 5450 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5452 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 5454 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 5456 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 5458 */	0x0 , 
			0x0,		/* 0 */
/* 5460 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5464 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5468 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 5472 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 5474 */	0x0 , 
			0x0,		/* 0 */
/* 5476 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5480 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5484 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5486 */	NdrFcShort( 0xff9e ),	/* Offset= -98 (5388) */
/* 5488 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5490 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 5492 */	NdrFcShort( 0x8 ),	/* 8 */
/* 5494 */	NdrFcShort( 0xffd2 ),	/* Offset= -46 (5448) */
/* 5496 */	NdrFcShort( 0x0 ),	/* Offset= 0 (5496) */
/* 5498 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 5500 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5502 */	
			0x12, 0x0,	/* FC_UP */
/* 5504 */	NdrFcShort( 0x2 ),	/* Offset= 2 (5506) */
/* 5506 */	
			0x17,		/* FC_CSTRUCT */
			0x7,		/* 7 */
/* 5508 */	NdrFcShort( 0x8 ),	/* 8 */
/* 5510 */	NdrFcShort( 0xec84 ),	/* Offset= -4988 (522) */
/* 5512 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 5514 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5516 */	
			0x12, 0x0,	/* FC_UP */
/* 5518 */	NdrFcShort( 0x46 ),	/* Offset= 70 (5588) */
/* 5520 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 5522 */	NdrFcShort( 0x38 ),	/* 56 */
/* 5524 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5526 */	NdrFcShort( 0x10 ),	/* Offset= 16 (5542) */
/* 5528 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 5530 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5532 */	NdrFcShort( 0xfe8c ),	/* Offset= -372 (5160) */
/* 5534 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5536 */	NdrFcShort( 0xea6c ),	/* Offset= -5524 (12) */
/* 5538 */	0x40,		/* FC_STRUCTPAD4 */
			0xb,		/* FC_HYPER */
/* 5540 */	0xb,		/* FC_HYPER */
			0x5b,		/* FC_END */
/* 5542 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5544 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5546 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x7,		/* 7 */
/* 5548 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5550 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 5552 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 5554 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 5556 */	0x0 , 
			0x0,		/* 0 */
/* 5558 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5562 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5566 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 5570 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 5572 */	0x0 , 
			0x0,		/* 0 */
/* 5574 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5578 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5582 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5584 */	NdrFcShort( 0xffc0 ),	/* Offset= -64 (5520) */
/* 5586 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5588 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 5590 */	NdrFcShort( 0x8 ),	/* 8 */
/* 5592 */	NdrFcShort( 0xffd2 ),	/* Offset= -46 (5546) */
/* 5594 */	NdrFcShort( 0x0 ),	/* Offset= 0 (5594) */
/* 5596 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 5598 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5600 */	
			0x12, 0x0,	/* FC_UP */
/* 5602 */	NdrFcShort( 0x44 ),	/* Offset= 68 (5670) */
/* 5604 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 5606 */	NdrFcShort( 0x28 ),	/* 40 */
/* 5608 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5610 */	NdrFcShort( 0xe ),	/* Offset= 14 (5624) */
/* 5612 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 5614 */	0x0,		/* 0 */
			NdrFcShort( 0xea1d ),	/* Offset= -5603 (12) */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 5618 */	0x0,		/* 0 */
			NdrFcShort( 0xfe35 ),	/* Offset= -459 (5160) */
			0x8,		/* FC_LONG */
/* 5622 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 5624 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5626 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5628 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 5630 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5632 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 5634 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 5636 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 5638 */	0x0 , 
			0x0,		/* 0 */
/* 5640 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5644 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5648 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 5652 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 5654 */	0x0 , 
			0x0,		/* 0 */
/* 5656 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5660 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5664 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5666 */	NdrFcShort( 0xffc2 ),	/* Offset= -62 (5604) */
/* 5668 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5670 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 5672 */	NdrFcShort( 0x8 ),	/* 8 */
/* 5674 */	NdrFcShort( 0xffd2 ),	/* Offset= -46 (5628) */
/* 5676 */	NdrFcShort( 0x0 ),	/* Offset= 0 (5676) */
/* 5678 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 5680 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5682 */	
			0x12, 0x0,	/* FC_UP */
/* 5684 */	NdrFcShort( 0x54 ),	/* Offset= 84 (5768) */
/* 5686 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 5688 */	NdrFcShort( 0x50 ),	/* 80 */
/* 5690 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5692 */	NdrFcShort( 0x16 ),	/* Offset= 22 (5714) */
/* 5694 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5696 */	NdrFcShort( 0xfde8 ),	/* Offset= -536 (5160) */
/* 5698 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 5700 */	0xd,		/* FC_ENUM16 */
			0x8,		/* FC_LONG */
/* 5702 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 5704 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 5706 */	0x0,		/* 0 */
			NdrFcShort( 0xe9c1 ),	/* Offset= -5695 (12) */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 5710 */	0x0,		/* 0 */
			NdrFcShort( 0xe9bd ),	/* Offset= -5699 (12) */
			0x5b,		/* FC_END */
/* 5714 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5716 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5718 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5720 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5722 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5724 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5726 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 5728 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5730 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 5732 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 5734 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 5736 */	0x0 , 
			0x0,		/* 0 */
/* 5738 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5742 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5746 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 5750 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 5752 */	0x0 , 
			0x0,		/* 0 */
/* 5754 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5758 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5762 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5764 */	NdrFcShort( 0xffb2 ),	/* Offset= -78 (5686) */
/* 5766 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5768 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 5770 */	NdrFcShort( 0x10 ),	/* 16 */
/* 5772 */	NdrFcShort( 0xffd2 ),	/* Offset= -46 (5726) */
/* 5774 */	NdrFcShort( 0x0 ),	/* Offset= 0 (5774) */
/* 5776 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5778 */	NdrFcShort( 0xfd96 ),	/* Offset= -618 (5160) */
/* 5780 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 5782 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5784 */	
			0x12, 0x0,	/* FC_UP */
/* 5786 */	NdrFcShort( 0x70 ),	/* Offset= 112 (5898) */
/* 5788 */	
			0x1b,		/* FC_CARRAY */
			0x0,		/* 0 */
/* 5790 */	NdrFcShort( 0x1 ),	/* 1 */
/* 5792 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 5794 */	NdrFcShort( 0x10 ),	/* 16 */
/* 5796 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 5798 */	0x0 , 
			0x0,		/* 0 */
/* 5800 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5804 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5808 */	0x2,		/* FC_CHAR */
			0x5b,		/* FC_END */
/* 5810 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 5812 */	NdrFcShort( 0x60 ),	/* 96 */
/* 5814 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5816 */	NdrFcShort( 0x1c ),	/* Offset= 28 (5844) */
/* 5818 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 5820 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 5822 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 5824 */	0x0,		/* 0 */
			NdrFcShort( 0xfd67 ),	/* Offset= -665 (5160) */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 5828 */	0x0,		/* 0 */
			NdrFcShort( 0xfd63 ),	/* Offset= -669 (5160) */
			0x8,		/* FC_LONG */
/* 5832 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5834 */	NdrFcShort( 0xfd5e ),	/* Offset= -674 (5160) */
/* 5836 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5838 */	NdrFcShort( 0xe93e ),	/* Offset= -5826 (12) */
/* 5840 */	0x40,		/* FC_STRUCTPAD4 */
			0xb,		/* FC_HYPER */
/* 5842 */	0xb,		/* FC_HYPER */
			0x5b,		/* FC_END */
/* 5844 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5846 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5848 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5850 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5852 */	
			0x14, 0x0,	/* FC_FP */
/* 5854 */	NdrFcShort( 0xffbe ),	/* Offset= -66 (5788) */
/* 5856 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x7,		/* 7 */
/* 5858 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5860 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 5862 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 5864 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 5866 */	0x0 , 
			0x0,		/* 0 */
/* 5868 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5872 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5876 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 5880 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 5882 */	0x0 , 
			0x0,		/* 0 */
/* 5884 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5888 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5892 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5894 */	NdrFcShort( 0xffac ),	/* Offset= -84 (5810) */
/* 5896 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5898 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 5900 */	NdrFcShort( 0x8 ),	/* 8 */
/* 5902 */	NdrFcShort( 0xffd2 ),	/* Offset= -46 (5856) */
/* 5904 */	NdrFcShort( 0x0 ),	/* Offset= 0 (5904) */
/* 5906 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 5908 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5910 */	
			0x12, 0x0,	/* FC_UP */
/* 5912 */	NdrFcShort( 0x2a ),	/* Offset= 42 (5954) */
/* 5914 */	
			0x15,		/* FC_STRUCT */
			0x7,		/* 7 */
/* 5916 */	NdrFcShort( 0x20 ),	/* 32 */
/* 5918 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5920 */	NdrFcShort( 0xe8ec ),	/* Offset= -5908 (12) */
/* 5922 */	0xb,		/* FC_HYPER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 5924 */	0x0,		/* 0 */
			NdrFcShort( 0xfd03 ),	/* Offset= -765 (5160) */
			0x5b,		/* FC_END */
/* 5928 */	
			0x1b,		/* FC_CARRAY */
			0x7,		/* 7 */
/* 5930 */	NdrFcShort( 0x20 ),	/* 32 */
/* 5932 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 5934 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 5936 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 5938 */	0x0 , 
			0x0,		/* 0 */
/* 5940 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5944 */	NdrFcLong( 0x0 ),	/* 0 */
/* 5948 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5950 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (5914) */
/* 5952 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5954 */	
			0x17,		/* FC_CSTRUCT */
			0x7,		/* 7 */
/* 5956 */	NdrFcShort( 0x8 ),	/* 8 */
/* 5958 */	NdrFcShort( 0xffe2 ),	/* Offset= -30 (5928) */
/* 5960 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 5962 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5964 */	
			0x12, 0x0,	/* FC_UP */
/* 5966 */	NdrFcShort( 0x44 ),	/* Offset= 68 (6034) */
/* 5968 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 5970 */	NdrFcShort( 0x28 ),	/* 40 */
/* 5972 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5974 */	NdrFcShort( 0xe ),	/* Offset= 14 (5988) */
/* 5976 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 5978 */	NdrFcShort( 0xe8b2 ),	/* Offset= -5966 (12) */
/* 5980 */	0xb,		/* FC_HYPER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 5982 */	0x0,		/* 0 */
			NdrFcShort( 0xfcc9 ),	/* Offset= -823 (5160) */
			0x36,		/* FC_POINTER */
/* 5986 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 5988 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 5990 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 5992 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x7,		/* 7 */
/* 5994 */	NdrFcShort( 0x0 ),	/* 0 */
/* 5996 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 5998 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 6000 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6002 */	0x0 , 
			0x0,		/* 0 */
/* 6004 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6008 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6012 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 6016 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 6018 */	0x0 , 
			0x0,		/* 0 */
/* 6020 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6024 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6028 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 6030 */	NdrFcShort( 0xffc2 ),	/* Offset= -62 (5968) */
/* 6032 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6034 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 6036 */	NdrFcShort( 0x8 ),	/* 8 */
/* 6038 */	NdrFcShort( 0xffd2 ),	/* Offset= -46 (5992) */
/* 6040 */	NdrFcShort( 0x0 ),	/* Offset= 0 (6040) */
/* 6042 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 6044 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6046 */	
			0x12, 0x0,	/* FC_UP */
/* 6048 */	NdrFcShort( 0x4c ),	/* Offset= 76 (6124) */
/* 6050 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 6052 */	NdrFcShort( 0x40 ),	/* 64 */
/* 6054 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6056 */	NdrFcShort( 0x12 ),	/* Offset= 18 (6074) */
/* 6058 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 6060 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 6062 */	NdrFcShort( 0xfc7a ),	/* Offset= -902 (5160) */
/* 6064 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 6066 */	NdrFcShort( 0xe85a ),	/* Offset= -6054 (12) */
/* 6068 */	0x40,		/* FC_STRUCTPAD4 */
			0xb,		/* FC_HYPER */
/* 6070 */	0xb,		/* FC_HYPER */
			0x36,		/* FC_POINTER */
/* 6072 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6074 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 6076 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 6078 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 6080 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 6082 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x7,		/* 7 */
/* 6084 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6086 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 6088 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 6090 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6092 */	0x0 , 
			0x0,		/* 0 */
/* 6094 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6098 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6102 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 6106 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 6108 */	0x0 , 
			0x0,		/* 0 */
/* 6110 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6114 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6118 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 6120 */	NdrFcShort( 0xffba ),	/* Offset= -70 (6050) */
/* 6122 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6124 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 6126 */	NdrFcShort( 0x8 ),	/* 8 */
/* 6128 */	NdrFcShort( 0xffd2 ),	/* Offset= -46 (6082) */
/* 6130 */	NdrFcShort( 0x0 ),	/* Offset= 0 (6130) */
/* 6132 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 6134 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6136 */	
			0x12, 0x0,	/* FC_UP */
/* 6138 */	NdrFcShort( 0x60 ),	/* Offset= 96 (6234) */
/* 6140 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 6142 */	NdrFcShort( 0x68 ),	/* 104 */
/* 6144 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6146 */	NdrFcShort( 0x1e ),	/* Offset= 30 (6176) */
/* 6148 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 6150 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 6152 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 6154 */	0x0,		/* 0 */
			NdrFcShort( 0xfc1d ),	/* Offset= -995 (5160) */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 6158 */	0x0,		/* 0 */
			NdrFcShort( 0xfc19 ),	/* Offset= -999 (5160) */
			0x8,		/* FC_LONG */
/* 6162 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 6164 */	NdrFcShort( 0xfc14 ),	/* Offset= -1004 (5160) */
/* 6166 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 6168 */	NdrFcShort( 0xe7f4 ),	/* Offset= -6156 (12) */
/* 6170 */	0x40,		/* FC_STRUCTPAD4 */
			0xb,		/* FC_HYPER */
/* 6172 */	0xb,		/* FC_HYPER */
			0x36,		/* FC_POINTER */
/* 6174 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6176 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 6178 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 6180 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 6182 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 6184 */	
			0x14, 0x0,	/* FC_FP */
/* 6186 */	NdrFcShort( 0xfe72 ),	/* Offset= -398 (5788) */
/* 6188 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 6190 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 6192 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x7,		/* 7 */
/* 6194 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6196 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 6198 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 6200 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6202 */	0x0 , 
			0x0,		/* 0 */
/* 6204 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6208 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6212 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 6216 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 6218 */	0x0 , 
			0x0,		/* 0 */
/* 6220 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6224 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6228 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 6230 */	NdrFcShort( 0xffa6 ),	/* Offset= -90 (6140) */
/* 6232 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6234 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 6236 */	NdrFcShort( 0x8 ),	/* 8 */
/* 6238 */	NdrFcShort( 0xffd2 ),	/* Offset= -46 (6192) */
/* 6240 */	NdrFcShort( 0x0 ),	/* Offset= 0 (6240) */
/* 6242 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 6244 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6246 */	
			0x12, 0x0,	/* FC_UP */
/* 6248 */	NdrFcShort( 0x4e ),	/* Offset= 78 (6326) */
/* 6250 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 6252 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6256 */	NdrFcLong( 0x100 ),	/* 256 */
/* 6260 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 6262 */	NdrFcShort( 0x30 ),	/* 48 */
/* 6264 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6266 */	NdrFcShort( 0xe ),	/* Offset= 14 (6280) */
/* 6268 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 6270 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 6272 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 6274 */	0x40,		/* FC_STRUCTPAD4 */
			0xb,		/* FC_HYPER */
/* 6276 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 6278 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6280 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 6282 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 6284 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x7,		/* 7 */
/* 6286 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6288 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 6290 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 6292 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6294 */	0x0 , 
			0x0,		/* 0 */
/* 6296 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6300 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6304 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 6308 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 6310 */	0x0 , 
			0x0,		/* 0 */
/* 6312 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6316 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6320 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 6322 */	NdrFcShort( 0xffc2 ),	/* Offset= -62 (6260) */
/* 6324 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6326 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 6328 */	NdrFcShort( 0x8 ),	/* 8 */
/* 6330 */	NdrFcShort( 0xffd2 ),	/* Offset= -46 (6284) */
/* 6332 */	NdrFcShort( 0x0 ),	/* Offset= 0 (6332) */
/* 6334 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 6336 */	NdrFcShort( 0xffaa ),	/* Offset= -86 (6250) */
/* 6338 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 6340 */	
			0x12, 0x0,	/* FC_UP */
/* 6342 */	NdrFcShort( 0xe95e ),	/* Offset= -5794 (548) */
/* 6344 */	
			0x12, 0x0,	/* FC_UP */
/* 6346 */	NdrFcShort( 0x36 ),	/* Offset= 54 (6400) */
/* 6348 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 6350 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6354 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 6358 */	
			0x15,		/* FC_STRUCT */
			0x7,		/* 7 */
/* 6360 */	NdrFcShort( 0x30 ),	/* 48 */
/* 6362 */	0xb,		/* FC_HYPER */
			0x8,		/* FC_LONG */
/* 6364 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 6366 */	0x0,		/* 0 */
			NdrFcShort( 0xe72d ),	/* Offset= -6355 (12) */
			0xb,		/* FC_HYPER */
/* 6370 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 6372 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6374 */	
			0x1b,		/* FC_CARRAY */
			0x7,		/* 7 */
/* 6376 */	NdrFcShort( 0x30 ),	/* 48 */
/* 6378 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 6380 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 6382 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6384 */	0x0 , 
			0x0,		/* 0 */
/* 6386 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6390 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6394 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 6396 */	NdrFcShort( 0xffda ),	/* Offset= -38 (6358) */
/* 6398 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6400 */	0xb1,		/* FC_FORCED_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 6402 */	NdrFcShort( 0x8 ),	/* 8 */
/* 6404 */	NdrFcShort( 0xffe2 ),	/* Offset= -30 (6374) */
/* 6406 */	NdrFcShort( 0x0 ),	/* Offset= 0 (6406) */
/* 6408 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 6410 */	NdrFcShort( 0xffc2 ),	/* Offset= -62 (6348) */
/* 6412 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 6414 */	
			0x11, 0x0,	/* FC_RP */
/* 6416 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6418) */
/* 6418 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 6420 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 6422 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 6424 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6426 */	0x0 , 
			0x0,		/* 0 */
/* 6428 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6432 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6436 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6438) */
/* 6438 */	NdrFcShort( 0x60 ),	/* 96 */
/* 6440 */	NdrFcShort( 0x1 ),	/* 1 */
/* 6442 */	NdrFcLong( 0x1 ),	/* 1 */
/* 6446 */	NdrFcShort( 0x64 ),	/* Offset= 100 (6546) */
/* 6448 */	NdrFcShort( 0xffff ),	/* Offset= -1 (6447) */
/* 6450 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 6452 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6456 */	NdrFcLong( 0x100 ),	/* 256 */
/* 6460 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 6462 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6466 */	NdrFcLong( 0x100 ),	/* 256 */
/* 6470 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 6472 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6476 */	NdrFcLong( 0x100 ),	/* 256 */
/* 6480 */	
			0x1b,		/* FC_CARRAY */
			0x1,		/* 1 */
/* 6482 */	NdrFcShort( 0x2 ),	/* 2 */
/* 6484 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 6486 */	NdrFcShort( 0x20 ),	/* 32 */
/* 6488 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6490 */	0x0 , 
			0x0,		/* 0 */
/* 6492 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6496 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6500 */	0x5,		/* FC_WCHAR */
			0x5b,		/* FC_END */
/* 6502 */	
			0x1b,		/* FC_CARRAY */
			0x1,		/* 1 */
/* 6504 */	NdrFcShort( 0x2 ),	/* 2 */
/* 6506 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 6508 */	NdrFcShort( 0x30 ),	/* 48 */
/* 6510 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6512 */	0x0 , 
			0x0,		/* 0 */
/* 6514 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6518 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6522 */	0x5,		/* FC_WCHAR */
			0x5b,		/* FC_END */
/* 6524 */	
			0x1b,		/* FC_CARRAY */
			0x1,		/* 1 */
/* 6526 */	NdrFcShort( 0x2 ),	/* 2 */
/* 6528 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 6530 */	NdrFcShort( 0x40 ),	/* 64 */
/* 6532 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6534 */	0x0 , 
			0x0,		/* 0 */
/* 6536 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6540 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6544 */	0x5,		/* FC_WCHAR */
			0x5b,		/* FC_END */
/* 6546 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 6548 */	NdrFcShort( 0x60 ),	/* 96 */
/* 6550 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6552 */	NdrFcShort( 0x1c ),	/* Offset= 28 (6580) */
/* 6554 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 6556 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 6558 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 6560 */	0x0,		/* 0 */
			NdrFcShort( 0xff91 ),	/* Offset= -111 (6450) */
			0x40,		/* FC_STRUCTPAD4 */
/* 6564 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 6566 */	0x0,		/* 0 */
			NdrFcShort( 0xff95 ),	/* Offset= -107 (6460) */
			0x40,		/* FC_STRUCTPAD4 */
/* 6570 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 6572 */	0x0,		/* 0 */
			NdrFcShort( 0xff99 ),	/* Offset= -103 (6470) */
			0x40,		/* FC_STRUCTPAD4 */
/* 6576 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 6578 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 6580 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 6582 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 6584 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 6586 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 6588 */	
			0x14, 0x8,	/* FC_FP [simple_pointer] */
/* 6590 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 6592 */	
			0x12, 0x0,	/* FC_UP */
/* 6594 */	NdrFcShort( 0xff8e ),	/* Offset= -114 (6480) */
/* 6596 */	
			0x12, 0x0,	/* FC_UP */
/* 6598 */	NdrFcShort( 0xffa0 ),	/* Offset= -96 (6502) */
/* 6600 */	
			0x12, 0x0,	/* FC_UP */
/* 6602 */	NdrFcShort( 0xffb2 ),	/* Offset= -78 (6524) */
/* 6604 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 6606 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 6608 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 6610 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 6612 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 6614 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6616) */
/* 6616 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 6618 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 6620 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 6622 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6624 */	0x0 , 
			0x0,		/* 0 */
/* 6626 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6630 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6634 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6636) */
/* 6636 */	NdrFcShort( 0x4 ),	/* 4 */
/* 6638 */	NdrFcShort( 0x1 ),	/* 1 */
/* 6640 */	NdrFcLong( 0x1 ),	/* 1 */
/* 6644 */	NdrFcShort( 0xf44a ),	/* Offset= -2998 (3646) */
/* 6646 */	NdrFcShort( 0xffff ),	/* Offset= -1 (6645) */
/* 6648 */	
			0x11, 0x0,	/* FC_RP */
/* 6650 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6652) */
/* 6652 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 6654 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 6656 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 6658 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6660 */	0x0 , 
			0x0,		/* 0 */
/* 6662 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6666 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6670 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6672) */
/* 6672 */	NdrFcShort( 0x10 ),	/* 16 */
/* 6674 */	NdrFcShort( 0x1 ),	/* 1 */
/* 6676 */	NdrFcLong( 0x1 ),	/* 1 */
/* 6680 */	NdrFcShort( 0x38 ),	/* Offset= 56 (6736) */
/* 6682 */	NdrFcShort( 0xffff ),	/* Offset= -1 (6681) */
/* 6684 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 6686 */	NdrFcLong( 0x1 ),	/* 1 */
/* 6690 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 6694 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 6696 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6698 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 6700 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6702 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6704 */	0x0 , 
			0x0,		/* 0 */
/* 6706 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6710 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6714 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 6718 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 6720 */	0x0 , 
			0x0,		/* 0 */
/* 6722 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6726 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6730 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 6732 */	NdrFcShort( 0xefb4 ),	/* Offset= -4172 (2560) */
/* 6734 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6736 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 6738 */	NdrFcShort( 0x10 ),	/* 16 */
/* 6740 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6742 */	NdrFcShort( 0xa ),	/* Offset= 10 (6752) */
/* 6744 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 6746 */	NdrFcShort( 0xffc2 ),	/* Offset= -62 (6684) */
/* 6748 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 6750 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6752 */	
			0x12, 0x0,	/* FC_UP */
/* 6754 */	NdrFcShort( 0xffc4 ),	/* Offset= -60 (6694) */
/* 6756 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 6758 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6760) */
/* 6760 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 6762 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 6764 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 6766 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6768 */	0x0 , 
			0x0,		/* 0 */
/* 6770 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6774 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6778 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6780) */
/* 6780 */	NdrFcShort( 0x10 ),	/* 16 */
/* 6782 */	NdrFcShort( 0x1 ),	/* 1 */
/* 6784 */	NdrFcLong( 0x1 ),	/* 1 */
/* 6788 */	NdrFcShort( 0x38 ),	/* Offset= 56 (6844) */
/* 6790 */	NdrFcShort( 0xffff ),	/* Offset= -1 (6789) */
/* 6792 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 6794 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6798 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 6802 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 6804 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6806 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 6808 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6810 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6812 */	0x0 , 
			0x0,		/* 0 */
/* 6814 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6818 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6822 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 6826 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 6828 */	0x0 , 
			0x0,		/* 0 */
/* 6830 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6834 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6838 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 6840 */	NdrFcShort( 0xefde ),	/* Offset= -4130 (2710) */
/* 6842 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6844 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 6846 */	NdrFcShort( 0x10 ),	/* 16 */
/* 6848 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6850 */	NdrFcShort( 0xa ),	/* Offset= 10 (6860) */
/* 6852 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 6854 */	NdrFcShort( 0xffc2 ),	/* Offset= -62 (6792) */
/* 6856 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 6858 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6860 */	
			0x12, 0x0,	/* FC_UP */
/* 6862 */	NdrFcShort( 0xffc4 ),	/* Offset= -60 (6802) */
/* 6864 */	
			0x11, 0x0,	/* FC_RP */
/* 6866 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6868) */
/* 6868 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 6870 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 6872 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 6874 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6876 */	0x0 , 
			0x0,		/* 0 */
/* 6878 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6882 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6886 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6888) */
/* 6888 */	NdrFcShort( 0x20 ),	/* 32 */
/* 6890 */	NdrFcShort( 0x1 ),	/* 1 */
/* 6892 */	NdrFcLong( 0x1 ),	/* 1 */
/* 6896 */	NdrFcShort( 0x4 ),	/* Offset= 4 (6900) */
/* 6898 */	NdrFcShort( 0xffff ),	/* Offset= -1 (6897) */
/* 6900 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 6902 */	NdrFcShort( 0x20 ),	/* 32 */
/* 6904 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6906 */	NdrFcShort( 0xa ),	/* Offset= 10 (6916) */
/* 6908 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 6910 */	0x0,		/* 0 */
			NdrFcShort( 0xe50d ),	/* Offset= -6899 (12) */
			0x8,		/* FC_LONG */
/* 6914 */	0x40,		/* FC_STRUCTPAD4 */
			0x5b,		/* FC_END */
/* 6916 */	
			0x11, 0x0,	/* FC_RP */
/* 6918 */	NdrFcShort( 0xe5ac ),	/* Offset= -6740 (178) */
/* 6920 */	
			0x11, 0x0,	/* FC_RP */
/* 6922 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6924) */
/* 6924 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 6926 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 6928 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 6930 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 6932 */	0x0 , 
			0x0,		/* 0 */
/* 6934 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6938 */	NdrFcLong( 0x0 ),	/* 0 */
/* 6942 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6944) */
/* 6944 */	NdrFcShort( 0x38 ),	/* 56 */
/* 6946 */	NdrFcShort( 0x1 ),	/* 1 */
/* 6948 */	NdrFcLong( 0x1 ),	/* 1 */
/* 6952 */	NdrFcShort( 0xa ),	/* Offset= 10 (6962) */
/* 6954 */	NdrFcShort( 0xffff ),	/* Offset= -1 (6953) */
/* 6956 */	
			0x1d,		/* FC_SMFARRAY */
			0x0,		/* 0 */
/* 6958 */	NdrFcShort( 0x10 ),	/* 16 */
/* 6960 */	0x2,		/* FC_CHAR */
			0x5b,		/* FC_END */
/* 6962 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 6964 */	NdrFcShort( 0x38 ),	/* 56 */
/* 6966 */	NdrFcShort( 0x0 ),	/* 0 */
/* 6968 */	NdrFcShort( 0x10 ),	/* Offset= 16 (6984) */
/* 6970 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 6972 */	NdrFcShort( 0xe4d0 ),	/* Offset= -6960 (12) */
/* 6974 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 6976 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 6978 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 6980 */	NdrFcShort( 0xffe8 ),	/* Offset= -24 (6956) */
/* 6982 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 6984 */	
			0x12, 0x0,	/* FC_UP */
/* 6986 */	NdrFcShort( 0xe568 ),	/* Offset= -6808 (178) */
/* 6988 */	
			0x12, 0x0,	/* FC_UP */
/* 6990 */	NdrFcShort( 0xe6d6 ),	/* Offset= -6442 (548) */
/* 6992 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 6994 */	NdrFcShort( 0x2 ),	/* Offset= 2 (6996) */
/* 6996 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 6998 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 7000 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 7002 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7004 */	0x0 , 
			0x0,		/* 0 */
/* 7006 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7010 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7014 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7016) */
/* 7016 */	NdrFcShort( 0x10 ),	/* 16 */
/* 7018 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7020 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7024 */	NdrFcShort( 0x38 ),	/* Offset= 56 (7080) */
/* 7026 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7025) */
/* 7028 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 7030 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7034 */	NdrFcLong( 0xa00000 ),	/* 10485760 */
/* 7038 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 7040 */	NdrFcShort( 0x0 ),	/* 0 */
/* 7042 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 7044 */	NdrFcShort( 0x4 ),	/* 4 */
/* 7046 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7048 */	0x0 , 
			0x0,		/* 0 */
/* 7050 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7054 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7058 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 7062 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 7064 */	0x0 , 
			0x0,		/* 0 */
/* 7066 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7070 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7074 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 7076 */	NdrFcShort( 0xe468 ),	/* Offset= -7064 (12) */
/* 7078 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 7080 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 7082 */	NdrFcShort( 0x10 ),	/* 16 */
/* 7084 */	NdrFcShort( 0x0 ),	/* 0 */
/* 7086 */	NdrFcShort( 0xa ),	/* Offset= 10 (7096) */
/* 7088 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 7090 */	0x0,		/* 0 */
			NdrFcShort( 0xffc1 ),	/* Offset= -63 (7028) */
			0x36,		/* FC_POINTER */
/* 7094 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 7096 */	
			0x12, 0x0,	/* FC_UP */
/* 7098 */	NdrFcShort( 0xffc4 ),	/* Offset= -60 (7038) */
/* 7100 */	
			0x11, 0x0,	/* FC_RP */
/* 7102 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7104) */
/* 7104 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7106 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 7108 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 7110 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7112 */	0x0 , 
			0x0,		/* 0 */
/* 7114 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7118 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7122 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7124) */
/* 7124 */	NdrFcShort( 0x20 ),	/* 32 */
/* 7126 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7128 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7132 */	NdrFcShort( 0x38 ),	/* Offset= 56 (7188) */
/* 7134 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7133) */
/* 7136 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 7138 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7142 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 7146 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 7148 */	NdrFcShort( 0x0 ),	/* 0 */
/* 7150 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 7152 */	NdrFcShort( 0x8 ),	/* 8 */
/* 7154 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7156 */	0x0 , 
			0x0,		/* 0 */
/* 7158 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7162 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7166 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 7170 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 7172 */	0x0 , 
			0x0,		/* 0 */
/* 7174 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7178 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7182 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 7184 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 7186 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 7188 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 7190 */	NdrFcShort( 0x20 ),	/* 32 */
/* 7192 */	NdrFcShort( 0x0 ),	/* 0 */
/* 7194 */	NdrFcShort( 0xc ),	/* Offset= 12 (7206) */
/* 7196 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 7198 */	0x0,		/* 0 */
			NdrFcShort( 0xffc1 ),	/* Offset= -63 (7136) */
			0x40,		/* FC_STRUCTPAD4 */
/* 7202 */	0x36,		/* FC_POINTER */
			0x8,		/* FC_LONG */
/* 7204 */	0x40,		/* FC_STRUCTPAD4 */
			0x5b,		/* FC_END */
/* 7206 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 7208 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 7210 */	
			0x12, 0x0,	/* FC_UP */
/* 7212 */	NdrFcShort( 0xffbe ),	/* Offset= -66 (7146) */
/* 7214 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 7216 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7218) */
/* 7218 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7220 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 7222 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 7224 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7226 */	0x0 , 
			0x0,		/* 0 */
/* 7228 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7232 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7236 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7238) */
/* 7238 */	NdrFcShort( 0x18 ),	/* 24 */
/* 7240 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7242 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7246 */	NdrFcShort( 0x38 ),	/* Offset= 56 (7302) */
/* 7248 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7247) */
/* 7250 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 7252 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7256 */	NdrFcLong( 0x2710 ),	/* 10000 */
/* 7260 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 7262 */	NdrFcShort( 0x0 ),	/* 0 */
/* 7264 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 7266 */	NdrFcShort( 0x0 ),	/* 0 */
/* 7268 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7270 */	0x0 , 
			0x0,		/* 0 */
/* 7272 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7276 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7280 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 7284 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 7286 */	0x0 , 
			0x0,		/* 0 */
/* 7288 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7292 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7296 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 7298 */	NdrFcShort( 0xf7a6 ),	/* Offset= -2138 (5160) */
/* 7300 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 7302 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 7304 */	NdrFcShort( 0x18 ),	/* 24 */
/* 7306 */	NdrFcShort( 0x0 ),	/* 0 */
/* 7308 */	NdrFcShort( 0xc ),	/* Offset= 12 (7320) */
/* 7310 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 7312 */	NdrFcShort( 0xffc2 ),	/* Offset= -62 (7250) */
/* 7314 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 7316 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 7318 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 7320 */	
			0x12, 0x0,	/* FC_UP */
/* 7322 */	NdrFcShort( 0xffc2 ),	/* Offset= -62 (7260) */
/* 7324 */	
			0x11, 0x0,	/* FC_RP */
/* 7326 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7328) */
/* 7328 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7330 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 7332 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 7334 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7336 */	0x0 , 
			0x0,		/* 0 */
/* 7338 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7342 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7346 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7348) */
/* 7348 */	NdrFcShort( 0x4 ),	/* 4 */
/* 7350 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7352 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7356 */	NdrFcShort( 0xf182 ),	/* Offset= -3710 (3646) */
/* 7358 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7357) */
/* 7360 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 7362 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7364) */
/* 7364 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7366 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 7368 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 7370 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7372 */	0x0 , 
			0x0,		/* 0 */
/* 7374 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7378 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7382 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7384) */
/* 7384 */	NdrFcShort( 0x4 ),	/* 4 */
/* 7386 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7388 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7392 */	NdrFcShort( 0xf15e ),	/* Offset= -3746 (3646) */
/* 7394 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7393) */
/* 7396 */	
			0x11, 0x0,	/* FC_RP */
/* 7398 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7400) */
/* 7400 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7402 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 7404 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 7406 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7408 */	0x0 , 
			0x0,		/* 0 */
/* 7410 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7414 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7418 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7420) */
/* 7420 */	NdrFcShort( 0x20 ),	/* 32 */
/* 7422 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7424 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7428 */	NdrFcShort( 0x4 ),	/* Offset= 4 (7432) */
/* 7430 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7429) */
/* 7432 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 7434 */	NdrFcShort( 0x20 ),	/* 32 */
/* 7436 */	NdrFcShort( 0x0 ),	/* 0 */
/* 7438 */	NdrFcShort( 0xa ),	/* Offset= 10 (7448) */
/* 7440 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 7442 */	0x0,		/* 0 */
			NdrFcShort( 0xe2f9 ),	/* Offset= -7431 (12) */
			0x40,		/* FC_STRUCTPAD4 */
/* 7446 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 7448 */	
			0x11, 0x0,	/* FC_RP */
/* 7450 */	NdrFcShort( 0xe398 ),	/* Offset= -7272 (178) */
/* 7452 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 7454 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7456) */
/* 7456 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7458 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 7460 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 7462 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7464 */	0x0 , 
			0x0,		/* 0 */
/* 7466 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7470 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7474 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7476) */
/* 7476 */	NdrFcShort( 0x4 ),	/* 4 */
/* 7478 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7480 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7484 */	NdrFcShort( 0xf102 ),	/* Offset= -3838 (3646) */
/* 7486 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7485) */
/* 7488 */	
			0x11, 0x0,	/* FC_RP */
/* 7490 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7492) */
/* 7492 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7494 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 7496 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 7498 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7500 */	0x0 , 
			0x0,		/* 0 */
/* 7502 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7506 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7510 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7512) */
/* 7512 */	NdrFcShort( 0x20 ),	/* 32 */
/* 7514 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7516 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7520 */	NdrFcShort( 0x4 ),	/* Offset= 4 (7524) */
/* 7522 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7521) */
/* 7524 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 7526 */	NdrFcShort( 0x20 ),	/* 32 */
/* 7528 */	NdrFcShort( 0x0 ),	/* 0 */
/* 7530 */	NdrFcShort( 0xa ),	/* Offset= 10 (7540) */
/* 7532 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 7534 */	0x0,		/* 0 */
			NdrFcShort( 0xe29d ),	/* Offset= -7523 (12) */
			0x40,		/* FC_STRUCTPAD4 */
/* 7538 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 7540 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 7542 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 7544 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 7546 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7548) */
/* 7548 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7550 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 7552 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 7554 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7556 */	0x0 , 
			0x0,		/* 0 */
/* 7558 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7562 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7566 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7568) */
/* 7568 */	NdrFcShort( 0xc ),	/* 12 */
/* 7570 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7572 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7576 */	NdrFcShort( 0x4 ),	/* Offset= 4 (7580) */
/* 7578 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7577) */
/* 7580 */	
			0x15,		/* FC_STRUCT */
			0x3,		/* 3 */
/* 7582 */	NdrFcShort( 0xc ),	/* 12 */
/* 7584 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 7586 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 7588 */	
			0x11, 0x0,	/* FC_RP */
/* 7590 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7592) */
/* 7592 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7594 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 7596 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 7598 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7600 */	0x0 , 
			0x0,		/* 0 */
/* 7602 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7606 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7610 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7612) */
/* 7612 */	NdrFcShort( 0x10 ),	/* 16 */
/* 7614 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7616 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7620 */	NdrFcShort( 0x4 ),	/* Offset= 4 (7624) */
/* 7622 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7621) */
/* 7624 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 7626 */	NdrFcShort( 0x10 ),	/* 16 */
/* 7628 */	NdrFcShort( 0x0 ),	/* 0 */
/* 7630 */	NdrFcShort( 0x6 ),	/* Offset= 6 (7636) */
/* 7632 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 7634 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 7636 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 7638 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 7640 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 7642 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 7644 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 7646 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7648) */
/* 7648 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7650 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 7652 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 7654 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7656 */	0x0 , 
			0x0,		/* 0 */
/* 7658 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7662 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7666 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7668) */
/* 7668 */	NdrFcShort( 0x20 ),	/* 32 */
/* 7670 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7672 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7676 */	NdrFcShort( 0x24 ),	/* Offset= 36 (7712) */
/* 7678 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7677) */
/* 7680 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 7682 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7686 */	NdrFcLong( 0x400 ),	/* 1024 */
/* 7690 */	
			0x1b,		/* FC_CARRAY */
			0x1,		/* 1 */
/* 7692 */	NdrFcShort( 0x2 ),	/* 2 */
/* 7694 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 7696 */	NdrFcShort( 0x10 ),	/* 16 */
/* 7698 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7700 */	0x0 , 
			0x0,		/* 0 */
/* 7702 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7706 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7710 */	0x5,		/* FC_WCHAR */
			0x5b,		/* FC_END */
/* 7712 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 7714 */	NdrFcShort( 0x20 ),	/* 32 */
/* 7716 */	NdrFcShort( 0x0 ),	/* 0 */
/* 7718 */	NdrFcShort( 0xc ),	/* Offset= 12 (7730) */
/* 7720 */	0x36,		/* FC_POINTER */
			0x36,		/* FC_POINTER */
/* 7722 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 7724 */	NdrFcShort( 0xffd4 ),	/* Offset= -44 (7680) */
/* 7726 */	0x40,		/* FC_STRUCTPAD4 */
			0x36,		/* FC_POINTER */
/* 7728 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 7730 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 7732 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 7734 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 7736 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 7738 */	
			0x12, 0x0,	/* FC_UP */
/* 7740 */	NdrFcShort( 0xffce ),	/* Offset= -50 (7690) */
/* 7742 */	
			0x11, 0x0,	/* FC_RP */
/* 7744 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7746) */
/* 7746 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7748 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 7750 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 7752 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7754 */	0x0 , 
			0x0,		/* 0 */
/* 7756 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7760 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7764 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7766) */
/* 7766 */	NdrFcShort( 0x18 ),	/* 24 */
/* 7768 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7770 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7774 */	NdrFcShort( 0xe ),	/* Offset= 14 (7788) */
/* 7776 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7775) */
/* 7778 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 7780 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7784 */	NdrFcLong( 0xffff ),	/* 65535 */
/* 7788 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 7790 */	NdrFcShort( 0x18 ),	/* 24 */
/* 7792 */	NdrFcShort( 0x0 ),	/* 0 */
/* 7794 */	NdrFcShort( 0xa ),	/* Offset= 10 (7804) */
/* 7796 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 7798 */	0x0,		/* 0 */
			NdrFcShort( 0xffeb ),	/* Offset= -21 (7778) */
			0x40,		/* FC_STRUCTPAD4 */
/* 7802 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 7804 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 7806 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 7808 */	
			0x12, 0x0,	/* FC_UP */
/* 7810 */	NdrFcShort( 0xed90 ),	/* Offset= -4720 (3090) */
/* 7812 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 7814 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7816) */
/* 7816 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7818 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 7820 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 7822 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7824 */	0x0 , 
			0x0,		/* 0 */
/* 7826 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7830 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7834 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7836) */
/* 7836 */	NdrFcShort( 0x4 ),	/* 4 */
/* 7838 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7840 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7844 */	NdrFcShort( 0xef9a ),	/* Offset= -4198 (3646) */
/* 7846 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7845) */
/* 7848 */	
			0x11, 0x0,	/* FC_RP */
/* 7850 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7852) */
/* 7852 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7854 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 7856 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 7858 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7860 */	0x0 , 
			0x0,		/* 0 */
/* 7862 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7866 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7870 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7872) */
/* 7872 */	NdrFcShort( 0x8 ),	/* 8 */
/* 7874 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7876 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7880 */	NdrFcShort( 0x4 ),	/* Offset= 4 (7884) */
/* 7882 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7881) */
/* 7884 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 7886 */	NdrFcShort( 0x8 ),	/* 8 */
/* 7888 */	NdrFcShort( 0x0 ),	/* 0 */
/* 7890 */	NdrFcShort( 0x4 ),	/* Offset= 4 (7894) */
/* 7892 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 7894 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 7896 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 7898 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 7900 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7902) */
/* 7902 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7904 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 7906 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 7908 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7910 */	0x0 , 
			0x0,		/* 0 */
/* 7912 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7916 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7920 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7922) */
/* 7922 */	NdrFcShort( 0x10 ),	/* 16 */
/* 7924 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7926 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7930 */	NdrFcShort( 0xe ),	/* Offset= 14 (7944) */
/* 7932 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7931) */
/* 7934 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 7936 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7940 */	NdrFcLong( 0xffff ),	/* 65535 */
/* 7944 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 7946 */	NdrFcShort( 0x10 ),	/* 16 */
/* 7948 */	NdrFcShort( 0x0 ),	/* 0 */
/* 7950 */	NdrFcShort( 0xa ),	/* Offset= 10 (7960) */
/* 7952 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 7954 */	0x0,		/* 0 */
			NdrFcShort( 0xffeb ),	/* Offset= -21 (7934) */
			0x36,		/* FC_POINTER */
/* 7958 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 7960 */	
			0x12, 0x0,	/* FC_UP */
/* 7962 */	NdrFcShort( 0xe65a ),	/* Offset= -6566 (1396) */
/* 7964 */	
			0x11, 0x0,	/* FC_RP */
/* 7966 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7968) */
/* 7968 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 7970 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 7972 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 7974 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 7976 */	0x0 , 
			0x0,		/* 0 */
/* 7978 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7982 */	NdrFcLong( 0x0 ),	/* 0 */
/* 7986 */	NdrFcShort( 0x2 ),	/* Offset= 2 (7988) */
/* 7988 */	NdrFcShort( 0x4 ),	/* 4 */
/* 7990 */	NdrFcShort( 0x1 ),	/* 1 */
/* 7992 */	NdrFcLong( 0x1 ),	/* 1 */
/* 7996 */	NdrFcShort( 0xef02 ),	/* Offset= -4350 (3646) */
/* 7998 */	NdrFcShort( 0xffff ),	/* Offset= -1 (7997) */
/* 8000 */	
			0x11, 0x0,	/* FC_RP */
/* 8002 */	NdrFcShort( 0x2 ),	/* Offset= 2 (8004) */
/* 8004 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 8006 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 8008 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 8010 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 8012 */	0x0 , 
			0x0,		/* 0 */
/* 8014 */	NdrFcLong( 0x0 ),	/* 0 */
/* 8018 */	NdrFcLong( 0x0 ),	/* 0 */
/* 8022 */	NdrFcShort( 0x2 ),	/* Offset= 2 (8024) */
/* 8024 */	NdrFcShort( 0x40 ),	/* 64 */
/* 8026 */	NdrFcShort( 0x1 ),	/* 1 */
/* 8028 */	NdrFcLong( 0x1 ),	/* 1 */
/* 8032 */	NdrFcShort( 0x4e ),	/* Offset= 78 (8110) */
/* 8034 */	NdrFcShort( 0xffff ),	/* Offset= -1 (8033) */
/* 8036 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 8038 */	NdrFcLong( 0x0 ),	/* 0 */
/* 8042 */	NdrFcLong( 0x400 ),	/* 1024 */
/* 8046 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 8048 */	NdrFcLong( 0x0 ),	/* 0 */
/* 8052 */	NdrFcLong( 0xa00000 ),	/* 10485760 */
/* 8056 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 8058 */	NdrFcLong( 0x0 ),	/* 0 */
/* 8062 */	NdrFcLong( 0xa00000 ),	/* 10485760 */
/* 8066 */	
			0x1b,		/* FC_CARRAY */
			0x0,		/* 0 */
/* 8068 */	NdrFcShort( 0x1 ),	/* 1 */
/* 8070 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 8072 */	NdrFcShort( 0x20 ),	/* 32 */
/* 8074 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 8076 */	0x0 , 
			0x0,		/* 0 */
/* 8078 */	NdrFcLong( 0x0 ),	/* 0 */
/* 8082 */	NdrFcLong( 0x0 ),	/* 0 */
/* 8086 */	0x2,		/* FC_CHAR */
			0x5b,		/* FC_END */
/* 8088 */	
			0x1b,		/* FC_CARRAY */
			0x0,		/* 0 */
/* 8090 */	NdrFcShort( 0x1 ),	/* 1 */
/* 8092 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 8094 */	NdrFcShort( 0x30 ),	/* 48 */
/* 8096 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 8098 */	0x0 , 
			0x0,		/* 0 */
/* 8100 */	NdrFcLong( 0x0 ),	/* 0 */
/* 8104 */	NdrFcLong( 0x0 ),	/* 0 */
/* 8108 */	0x2,		/* FC_CHAR */
			0x5b,		/* FC_END */
/* 8110 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 8112 */	NdrFcShort( 0x40 ),	/* 64 */
/* 8114 */	NdrFcShort( 0x0 ),	/* 0 */
/* 8116 */	NdrFcShort( 0x18 ),	/* Offset= 24 (8140) */
/* 8118 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 8120 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 8122 */	0x0,		/* 0 */
			NdrFcShort( 0xffa9 ),	/* Offset= -87 (8036) */
			0x40,		/* FC_STRUCTPAD4 */
/* 8126 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 8128 */	0x0,		/* 0 */
			NdrFcShort( 0xffad ),	/* Offset= -83 (8046) */
			0x40,		/* FC_STRUCTPAD4 */
/* 8132 */	0x36,		/* FC_POINTER */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 8134 */	0x0,		/* 0 */
			NdrFcShort( 0xffb1 ),	/* Offset= -79 (8056) */
			0x40,		/* FC_STRUCTPAD4 */
/* 8138 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 8140 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 8142 */	
			0x25,		/* FC_C_WSTRING */
			0x5c,		/* FC_PAD */
/* 8144 */	
			0x12, 0x0,	/* FC_UP */
/* 8146 */	NdrFcShort( 0xf6ca ),	/* Offset= -2358 (5788) */
/* 8148 */	
			0x12, 0x0,	/* FC_UP */
/* 8150 */	NdrFcShort( 0xffac ),	/* Offset= -84 (8066) */
/* 8152 */	
			0x12, 0x0,	/* FC_UP */
/* 8154 */	NdrFcShort( 0xffbe ),	/* Offset= -66 (8088) */
/* 8156 */	
			0x11, 0x0,	/* FC_RP */
/* 8158 */	NdrFcShort( 0x2 ),	/* Offset= 2 (8160) */
/* 8160 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 8162 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x0,		/*  */
/* 8164 */	NdrFcShort( 0x8 ),	/* X64 Stack size/offset = 8 */
/* 8166 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 8168 */	0x0 , 
			0x0,		/* 0 */
/* 8170 */	NdrFcLong( 0x0 ),	/* 0 */
/* 8174 */	NdrFcLong( 0x0 ),	/* 0 */
/* 8178 */	NdrFcShort( 0x2 ),	/* Offset= 2 (8180) */
/* 8180 */	NdrFcShort( 0x10 ),	/* 16 */
/* 8182 */	NdrFcShort( 0x1 ),	/* 1 */
/* 8184 */	NdrFcLong( 0x1 ),	/* 1 */
/* 8188 */	NdrFcShort( 0xe ),	/* Offset= 14 (8202) */
/* 8190 */	NdrFcShort( 0xffff ),	/* Offset= -1 (8189) */
/* 8192 */	0xb7,		/* FC_RANGE */
			0x8,		/* 8 */
/* 8194 */	NdrFcLong( 0x1 ),	/* 1 */
/* 8198 */	NdrFcLong( 0x400 ),	/* 1024 */
/* 8202 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 8204 */	NdrFcShort( 0x10 ),	/* 16 */
/* 8206 */	NdrFcShort( 0x0 ),	/* 0 */
/* 8208 */	NdrFcShort( 0xa ),	/* Offset= 10 (8218) */
/* 8210 */	0x8,		/* FC_LONG */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 8212 */	0x0,		/* 0 */
			NdrFcShort( 0xffeb ),	/* Offset= -21 (8192) */
			0x36,		/* FC_POINTER */
/* 8216 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 8218 */	
			0x12, 0x0,	/* FC_UP */
/* 8220 */	NdrFcShort( 0xe558 ),	/* Offset= -6824 (1396) */
/* 8222 */	
			0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 8224 */	NdrFcShort( 0x2 ),	/* Offset= 2 (8226) */
/* 8226 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 8228 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
			0x54,		/* FC_DEREFERENCE */
/* 8230 */	NdrFcShort( 0x18 ),	/* X64 Stack size/offset = 24 */
/* 8232 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 8234 */	0x0 , 
			0x0,		/* 0 */
/* 8236 */	NdrFcLong( 0x0 ),	/* 0 */
/* 8240 */	NdrFcLong( 0x0 ),	/* 0 */
/* 8244 */	NdrFcShort( 0x2 ),	/* Offset= 2 (8246) */
/* 8246 */	NdrFcShort( 0x10 ),	/* 16 */
/* 8248 */	NdrFcShort( 0x1 ),	/* 1 */
/* 8250 */	NdrFcLong( 0x1 ),	/* 1 */
/* 8254 */	NdrFcShort( 0x4 ),	/* Offset= 4 (8258) */
/* 8256 */	NdrFcShort( 0xffff ),	/* Offset= -1 (8255) */
/* 8258 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 8260 */	NdrFcShort( 0x10 ),	/* 16 */
/* 8262 */	NdrFcShort( 0x0 ),	/* 0 */
/* 8264 */	NdrFcShort( 0x6 ),	/* Offset= 6 (8270) */
/* 8266 */	0x8,		/* FC_LONG */
			0x40,		/* FC_STRUCTPAD4 */
/* 8268 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 8270 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 8272 */	
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

