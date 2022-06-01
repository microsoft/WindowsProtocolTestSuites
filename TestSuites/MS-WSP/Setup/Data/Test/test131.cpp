/* Common Utility Functions used across the Automation */


#include "stdafx.h"
#include "legacy.h"
#include "ms-dtyp.h"
#include <stdlib.h>
#include <stdio.h>
#include <strsafe.h>
 

    RPC_STATUS TermSrvBindSecure(LPCWSTR pszUuid,
                                 LPCWSTR pszProtocolSequence,
                                 LPCWSTR pszNetworkAddress,
                                 LPCWSTR pszEndPoint,
                                 LPCWSTR pszOptions,
                                 RPC_BINDING_HANDLE *pHandle
                                 );
	RPC_STATUS TermSrvBind ( IN LPCWSTR pszUuid,
                             IN LPCWSTR pszProtocolSequence,
                             IN LPCWSTR pszNetworkAddress,
                             IN LPCWSTR pszEndPoint,
                             IN LPCWSTR pszOptions,
                             OUT RPC_BINDING_HANDLE *pHandle
                             );
	BOOL PrepareServerSPN(IN LPCWSTR pszNetworkAddress,
                      __deref_out_opt LPWSTR *ppwszServerSPN
                      );


   HANDLE GetRPCBinding(LPWSTR pszServerName, int Flag_UUID, int Flag_NamedPipe)
{
	wchar_t  *gpszPublicUuid;
	switch(Flag_UUID)
	{
	case 1: 
		gpszPublicUuid                   = L"484809d6-4239-471b-b5bc-61df8c23ac48";
		break;
	case 2:
		gpszPublicUuid                   = L"11899a43-2b68-4a76-92e3-a3d6ad8c26ce";
		break;
	case 3:
		gpszPublicUuid                   = L"88143fd0-c28d-4b2b-8fef-8d882f6a9390";
		break;
	case 4:
		gpszPublicUuid = L"bde95fdf-eee0-45de-9e12-e5a61cd0d4fe";
		break;
	case 5:
		gpszPublicUuid = L"497d95a6-2d27-4bf5-9bbd-a6046957133c";
		break;
	case 6:
		gpszPublicUuid = L"5ca4a760-ebb1-11cf-8611-00a0245420ed";
		break;
	}
    wchar_t  *pszEndpoint;

	switch(Flag_NamedPipe)
	{
	case 1:
		pszEndpoint = L"\\PIPE\\LSM_API_service";
		break;
	case 2:
		pszEndpoint = L"\\PIPE\\TermSrv_API_service";
		break;
	case 3:
		pszEndpoint = L"\\PIPE\\Ctx_WinStation_API_service";
	}
                      
    wchar_t  *gpszRemoteProtocolSequence	   = L"ncacn_np";
	wchar_t  *gpszOptions                      = NULL;
	RPC_BINDING_HANDLE hLSMBinding             = (RPC_BINDING_HANDLE)MIDL_user_allocate(512);

//HANDLE hLSMBinding = NULL;
RPC_STATUS rpcStatus = RPC_S_OK;
//ASSERT( NULL != pszServerName );
rpcStatus = TermSrvBindSecure((LPCWSTR)gpszPublicUuid,
							(LPCWSTR)gpszRemoteProtocolSequence,
							(LPCWSTR)pszServerName,
							(LPCWSTR)pszEndpoint,
							(LPCWSTR)gpszOptions,
							(RPC_BINDING_HANDLE*)&hLSMBinding
							);
if( rpcStatus != RPC_S_OK || hLSMBinding == NULL)
{
wprintf(L"ERR: TermSrvBindSecure failed: %d\n",
rpcStatus );
SetLastError( rpcStatus );
}
return hLSMBinding;
}

 RPC_STATUS TermSrvBindSecure(LPCWSTR pszUuid,
                             LPCWSTR pszProtocolSequence,
                             LPCWSTR pszNetworkAddress,
                             LPCWSTR pszEndPoint,
                             LPCWSTR pszOptions,
                             RPC_BINDING_HANDLE *pHandle
                             )
{
	RPC_STATUS Status;
	RPC_SECURITY_QOS qos;
	LPWSTR wszServerSPN = NULL;
	*pHandle = NULL;
	printf("Calling TermSrvBind\n");
	Status = TermSrvBind(pszUuid,
	                     pszProtocolSequence,
	                     pszNetworkAddress,
	                     pszEndPoint,
	                     pszOptions,
	                     pHandle);
    
	if( Status != RPC_S_OK )
	{
	wprintf(L"Error %d in TermSrvBind", Status );
	//TRC( ERR, "Error %d in TermSrvBind", Status );
	//TS_QUIT;
	}
	qos.Capabilities = RPC_C_QOS_CAPABILITIES_MUTUAL_AUTH;
	qos.IdentityTracking = RPC_C_QOS_IDENTITY_DYNAMIC;
	qos.ImpersonationType = RPC_C_IMP_LEVEL_IMPERSONATE;
	qos.Version = RPC_C_SECURITY_QOS_VERSION;
	
	if( PrepareServerSPN( pszNetworkAddress, &wszServerSPN ))
		{
         Status = RpcBindingSetAuthInfoEx(*pHandle,
			                              (RPC_WSTR)wszServerSPN,
                                          RPC_C_AUTHN_LEVEL_PKT_PRIVACY,
                                          RPC_C_AUTHN_GSS_NEGOTIATE,
                                          NULL,
                                          RPC_C_AUTHZ_NAME,
                                          &qos);
	LocalFree(wszServerSPN);
	}
	else
	{
	Status = RpcBindingSetAuthInfoEx(*pHandle,
	                                 (RPC_WSTR)pszNetworkAddress,
	                                 RPC_C_AUTHN_LEVEL_PKT_PRIVACY,
	                                 RPC_C_AUTHN_GSS_NEGOTIATE,
	                                 NULL,
	                                 RPC_C_AUTHZ_NAME,
	                                 &qos);
	}
	if ( RPC_S_OK != Status )
	{
	wprintf(L"Error %d in RpcBindingSetAuthInfoEx", Status );	
	//TRC( ERR, "Error %d in RpcBindingSetAuthInfoEx", Status );
	//TS_QUIT;
	}
	//TS_EXIT_POINT:
	if ( RPC_S_OK != Status &&
	NULL != *pHandle )
	{
	RpcBindingFree( pHandle );
	}
	//TS_LEAVEFN();
	return Status;
	}


	RPC_STATUS TermSrvBind(IN LPCWSTR pszUuid,
                           IN LPCWSTR pszProtocolSequence,
                           IN LPCWSTR pszNetworkAddress,
                           IN LPCWSTR pszEndPoint,
                           IN LPCWSTR pszOptions,
                           OUT RPC_BINDING_HANDLE *pHandle
                           )
	{
	RPC_STATUS Status;
	LPWSTR pszString = NULL;
	//TS_ENTERFN();
	/*
	* Compose the binding string using the helper routine
	* and our protocol sequence, security options, UUID, etc.
	*/
	Status = RpcStringBindingCompose((RPC_WSTR)pszUuid,
	                                 (RPC_WSTR)pszProtocolSequence,
	                                 (RPC_WSTR)pszNetworkAddress,
	                                 (RPC_WSTR)pszEndPoint,
	                                 (RPC_WSTR)pszOptions,
	                                 (RPC_WSTR*)&pszString
	                                 );
	if( Status != RPC_S_OK )
	{
    wprintf(L"Error %d in RpcStringBindingCompose", Status );
	//TRC( ERR, "Error %d in RpcStringBindingCompose", Status );
	//TS_QUIT;
	}
	/*
	* Now generate the RPC binding from the cononical RPC
	* binding string.
	*/
	Status = RpcBindingFromStringBinding((RPC_WSTR)pszString,
	                                     pHandle
	                                    );
	if( Status != RPC_S_OK )
	{
	wprintf(L"Error %d in RpcBindingFromStringBinding", Status );
	//TRC( ERR, "Error %d in RpcBindingFromStringBinding", Status );
	//TS_QUIT;
	}
	//TS_EXIT_POINT:
	if ( NULL != pszString )
	{	
	//Free the memory returned from RpcStringBindingCompose()	
	RpcStringFree( (RPC_WSTR*)&pszString);
	}
	//TS_LEAVEFN();
	return( Status );
	}


BOOL PrepareServerSPN(IN LPCWSTR pszNetworkAddress,
                      __deref_out_opt LPWSTR *ppwszServerSPN
                     )
	{
	// longhorn RPC does not accept "net use" credential anymore.
	// <Domain>\<Machine> is not a valid SPN, a valid SPN is host/<Machine Name>
	LPWSTR pszTemplate = L"host/%s";
	*ppwszServerSPN = NULL;
	HRESULT hr = S_OK;
	UINT stringLength = (UINT)wcslen(pszTemplate)+wcslen(pszNetworkAddress)+1;
	*ppwszServerSPN = (LPWSTR)LocalAlloc(LPTR, stringLength * sizeof(WCHAR));
	if(*ppwszServerSPN)
	{
      printf("Inside PrepareServerSPN\n ");
	hr = StringCchPrintf( *ppwszServerSPN, stringLength, pszTemplate,
	pszNetworkAddress );
	//ASSERT( SUCCEEDED( hr ));
	}
	if( FAILED(hr) )
	{
	if( NULL != *ppwszServerSPN )
	{
	LocalFree( *ppwszServerSPN );
	*ppwszServerSPN = NULL;
	}
	}
	return SUCCEEDED(hr);
	}




void * __RPC_USER MIDL_user_allocate(size_t memSize)
{
    return malloc(memSize);
} 



void __RPC_USER MIDL_user_free(void *p)
{
    free(p);
} 