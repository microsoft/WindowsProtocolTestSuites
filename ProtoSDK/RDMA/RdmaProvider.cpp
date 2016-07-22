// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#include <WS2spi.h>
#include "Rdma.h"


using namespace System;
using namespace System::Runtime::InteropServices;


// Begin SMBD client Namespace
BEGIN_RDMA_NAMESPACE

WCHAR * GetProviderPath(
	WSAPROTOCOL_INFOW* pProtocol)
{
	INT pathLen;
	INT ret, err;
	WCHAR * pPath;
	WCHAR * pPathEx;

	ret = WSCGetProviderPath(&pProtocol->ProviderId, NULL, &pathLen, &err );
	if( err != WSAEFAULT || pathLen == 0 ) { return NULL; }

	// Alloc memory
	pPath = (WCHAR*)HeapAlloc( GetProcessHeap(), 0, sizeof(WCHAR) * pathLen );
	if( pPath == NULL ) { return NULL; }

	ret = WSCGetProviderPath( &pProtocol->ProviderId, pPath, &pathLen, &err );
    if( ret != 0 )
    {
        HeapFree( GetProcessHeap(), 0, pPath );
        return NULL;
    }

	// Extend path
	pathLen = ExpandEnvironmentStringsW( pPath, NULL, 0 );
    if( pathLen == 0 )
    {
        HeapFree( GetProcessHeap(), 0, pPath );
        return NULL;
    }

    pPathEx = (WCHAR*)HeapAlloc( GetProcessHeap(), 0, sizeof(WCHAR) * pathLen );
    if( pPathEx == NULL )
    {
        HeapFree( GetProcessHeap(), 0, pPath );
        return NULL;
    }

	ret = ExpandEnvironmentStringsW( pPath, pPathEx, pathLen );
    // We don't need the un-expanded path anymore.
    HeapFree( GetProcessHeap(), 0, pPath );
    if( ret != pathLen )
    {
        HeapFree( GetProcessHeap(), 0, pPathEx );
        return NULL;
    }

	return pPathEx;
}

/// <summary>
/// Load provider
/// </summary>
/// <param name="providers">provider to load</param>
HRESULT RdmaProvider::LoadRdmaProviders([Out]array<RdmaProviderInfo^>^% providers)
{
	// ------------------------------------------------------------------
	// Get the protocols 
	DWORD proLen = 0;
	INT proErr = 0;
	
	// To get the Network Direct providers that are registered on the computer
	// Call Winsock SPI function WSCEnumProtocols
	// Reference: http://msdn.microsoft.com/en-us/library/ms742237(v=vs.85).aspx
	INT proRet = WSCEnumProtocols(
		NULL, // lpiProtocols, if it is NULL, information on all available protocols is returned.
		NULL, // lpProtocolBuffer, a pointer to a buffer that is filled with WSAPROTOCOL_INFOW structures
		&proLen, // lpdwBufferLength, on input, size of the lpProtocolBuffer buffer passed to WSCEnumProtocols, in bytes. 
				// On output, the minimum buffer size, in bytes, that can be passed to WSCEnumProtocols to retrieve all the requested information.
		&proErr // lpErrno, a pointer to the error code.
		);

	if( proRet != SOCKET_ERROR || proErr != WSAENOBUFS ) 
	{
		return ND_INTERNAL_ERROR; 
	}

	// -------------------------------------------------------------------
	// Alloc memory to protocols
	WSAPROTOCOL_INFOW* pProtocols = (WSAPROTOCOL_INFOW*)HeapAlloc(GetProcessHeap(), 0, proLen );
	if( pProtocols == NULL ) { return ND_NO_MEMORY; }

	proRet = WSCEnumProtocols( NULL, pProtocols, &proLen, &proErr );
	if( proRet == SOCKET_ERROR ) { return ND_INTERNAL_ERROR; }

	// -------------------------------------------------------------------
	// For each protocol functions returns
	int proCount = proLen / sizeof(WSAPROTOCOL_INFOW);
	NTSTATUS result;

	// return list
	providers = gcnew array<RdmaProviderInfo^>(proCount);

	WCHAR *pPath = NULL;
	for(int i = 0; i< proCount; ++i)
	{
		if(pPath != NULL) HeapFree( GetProcessHeap(), 0, pPath );
		// load Path Information
		pPath = GetProviderPath(&pProtocols[i]);
		providers[i] = gcnew RdmaProviderInfo();
		providers[i]->Path = gcnew String(pPath);

		// Compare the members of the WSAPROTOCOL_INFO structure to 
		// those specified in Registering a Network Direct Provider.
		#define ServiceFlags1Flags (XP1_GUARANTEED_DELIVERY | XP1_GUARANTEED_ORDER | XP1_MESSAGE_ORIENTED | XP1_CONNECT_DATA)

		// Check the provider with below filters
		if( (pProtocols[i].dwServiceFlags1 & ServiceFlags1Flags) != ServiceFlags1Flags ) { continue; }
		if( pProtocols[i].iAddressFamily != AF_INET && pProtocols[i].iAddressFamily != AF_INET6 )  { continue; }
		if( pProtocols[i].iSocketType != -1 )  { continue; }
        if( pProtocols[i].iProtocol != 0 )  { continue; }
        if( pProtocols[i].iProtocolMaxOffset != 0 )  { continue; }

		// Get the match and then load the provider
		INDProvider *provider;
		HMODULE libraryHandler;
		result = LoadProvider(
			pPath,
			pProtocols[i].ProviderId,
			&provider,
			&libraryHandler);
		if(result == ND_SUCCESS)
		{
			// add resource to list
			providers[i]->Provider = gcnew RdmaProvider(provider, libraryHandler);
		}
	}

	// free
	if(pPath != NULL) HeapFree( GetProcessHeap(), 0, pPath );
	HeapFree( GetProcessHeap(), 0, pProtocols );

	return ND_SUCCESS;
}

/// <summary>
/// Load provider
/// </summary>
HRESULT RdmaProvider::LoadRdmaProvider(System::Guid providerId, String^ path, [Out]RdmaProviderInfo^% provider)
{
	array<Byte>^ guidData = providerId.ToByteArray();
	pin_ptr<Byte> data = &(guidData[ 0 ]);
	_GUID innerProviderId = *(_GUID *)data;

	// EstablishConnection
	IntPtr ptrPath = Marshal::StringToHGlobalAnsi(path);
	char* pPath = static_cast<char*>(ptrPath.ToPointer());
	size_t newSize = path->Length + 1;
	size_t convertedSize = 0;
	WCHAR *pwPath = (WCHAR*)malloc(newSize*sizeof(WCHAR));
	NTSTATUS result = mbstowcs_s(&convertedSize, pwPath, newSize, pPath, _TRUNCATE);
	if(result != ND_SUCCESS)
	{
		return result;
	}

	INDProvider *pProvider;
	HMODULE libraryHandler;
	result = LoadProvider(
			pwPath,
			innerProviderId,
			&pProvider,
			&libraryHandler);
	if(result == ND_SUCCESS)
	{
		provider = gcnew RdmaProviderInfo();
		provider->Provider = gcnew RdmaProvider(pProvider, libraryHandler);
		provider->Path = path;
	}
	return result;
}

/// <summary>
/// Deconstructor, release library resource and provider
/// </summary>
RdmaProvider::~RdmaProvider()
{
	if( _provider != NULL )
	{
		_provider->Release();
	}

	if( _libraryHandler != NULL )
	{
		FreeLibrary( _libraryHandler );
	}
}

/// <summary>
/// Open adapter with specific address
/// </summary>
/// <param name="ipAddress">IP Address</param>
/// <param name="ipFamily">IP Family</param>
/// <param name="adapter">opened adapter</param>
HRESULT RdmaProvider::OpenAdapter(String^ ipAddress, short ipFamily, [Out]RdmaAdapter^% adapter)
{
	// EstablishConnection
	IntPtr ptrLocalIP = Marshal::StringToHGlobalAnsi(ipAddress);
	const char* pLocalIP = static_cast<const char*>(ptrLocalIP.ToPointer());

	// convert the IP address and Port
	sockaddr_in localAddr;
	localAddr.sin_family = ipFamily;
	localAddr.sin_addr.s_addr = inet_addr(pLocalIP);

	INDAdapter *nativeAdapter;
	HRESULT result = _provider->OpenAdapter(
		(const sockaddr *)&localAddr,
		sizeof(localAddr),
		&nativeAdapter
		);
	if(result != ND_SUCCESS)
	{
		return result;
	}

	adapter = gcnew RdmaAdapter(nativeAdapter);
	return result;
}

/// <summary>
/// Retrieves a list of local addresses that the provider supports.
/// </summary>
HRESULT RdmaProvider::QueryAddressList([Out]array<RdmaAddress^>^% addressList)
{
	SOCKET_ADDRESS_LIST *innerAddressList;
	SIZE_T bufferSize = 0;

	HRESULT result = _provider->QueryAddressList(
		NULL,
		&bufferSize
		);
	if(result == ND_BUFFER_OVERFLOW)
	{
		innerAddressList = (SOCKET_ADDRESS_LIST *)malloc(bufferSize);
	}
	else
	{
		addressList = gcnew array<RdmaAddress^>(0);
		return result;
	}

	result = _provider->QueryAddressList(
		innerAddressList,
		&bufferSize
		);
	if(result != ND_SUCCESS)
	{
		addressList = gcnew array<RdmaAddress^>(0);
		free(innerAddressList);
		return result;
	}

	addressList = gcnew array<RdmaAddress^>(innerAddressList->iAddressCount);
	for(int i = 0; i < innerAddressList->iAddressCount; ++i)
	{
		sockaddr *address = innerAddressList->Address[i].lpSockaddr;
		struct sockaddr_in *addressV4;

		switch(address->sa_family) 
		{
			case AF_INET:
				addressV4 = (struct sockaddr_in *)address;
				addressList[i] = gcnew RdmaAddress();
				addressList[i]->Family = AF_INET;
				addressList[i]->Data = gcnew array<unsigned char>(4);
				addressList[i]->Data[0] = ((unsigned char*)&addressV4->sin_addr)[0];
				addressList[i]->Data[1] = ((unsigned char*)&addressV4->sin_addr)[1];
				addressList[i]->Data[2] = ((unsigned char*)&addressV4->sin_addr)[2];
				addressList[i]->Data[3] = ((unsigned char*)&addressV4->sin_addr)[3];
				break;
			default:
				addressList[i] = gcnew RdmaAddress();
				addressList[i]->Family = address->sa_family;
				addressList[i]->Data = gcnew array<unsigned char>(0);
				break;
		}
	}

	free(innerAddressList);
	return result;
}

// =============================================================================
// private

RdmaProvider::RdmaProvider(INDProvider *provider, HMODULE libraryHandler)
{
	_provider = provider;
	_libraryHandler = libraryHandler;
}

/// <summary>
/// Load provider
/// </summary>
NTSTATUS RdmaProvider::LoadProvider(
	WCHAR * path,
	_GUID protocolId,
	INDProvider **pProvider,
	HMODULE *pLibraryHandler)
{
	// -------------------------------------------------------------------
	// Load the provider's DLL. 
	// To get the path to the DLL, call the WSCGetProviderPath function using the ProviderId 
	// member from the WSAPROTOCOL_INFO structure.
	// Call the library's DllGetClassObject 
	// entry point to get an IClassFactory interface.
	*pLibraryHandler = LoadLibraryW( path );

	if( *pLibraryHandler == NULL )
	{
		return __HRESULT_FROM_WIN32( GetLastError() );
	}

	DLLGETCLASSOBJECT pfnDllGetClassObject = (DLLGETCLASSOBJECT)GetProcAddress(
													*pLibraryHandler,
													"DllGetClassObject" );
	if( pfnDllGetClassObject == NULL )
	{
		return __HRESULT_FROM_WIN32( GetLastError() );
	}

	DLLCANUNLOADNOW g_pfnDllCanUnloadNow = (DLLCANUNLOADNOW)GetProcAddress(
													*pLibraryHandler,
													"DllCanUnloadNow" );
	if( g_pfnDllCanUnloadNow == NULL )
	{
		return __HRESULT_FROM_WIN32( GetLastError() );
	}

	IClassFactory* pClassFactory;
	HRESULT hr = pfnDllGetClassObject(
		protocolId,
		IID_IClassFactory,
		(void**)&pClassFactory
		);

	if( FAILED(hr) ) 
	{ 
		return hr;
	}

	GUID _IID_INDProvider; // GUID for NDSPI COM
	_IID_INDProvider.Data1 = 0xc5dd316;
	_IID_INDProvider.Data2 = 0x5fdf;
	_IID_INDProvider.Data3 = 0x47e6;
	_IID_INDProvider.Data4[0] = 0xb2;
	_IID_INDProvider.Data4[1] = 0xd0;
	_IID_INDProvider.Data4[2] = 0x2a;
	_IID_INDProvider.Data4[3] = 0x6e;
	_IID_INDProvider.Data4[4] = 0xda;
	_IID_INDProvider.Data4[5] = 0x8d;
	_IID_INDProvider.Data4[6] = 0x39;
	_IID_INDProvider.Data4[7] = 0xdd;
	// ------------------------------------------------------------------
	// 3.Use the factory's CreateInstance method to 
	// instantiate the INDProvider interface.
    hr = pClassFactory->CreateInstance( 
		NULL,
		_IID_INDProvider,
		(void**)pProvider
		);

    // Now that we asked for the provider, we don't need the class factory.
    pClassFactory->Release();
    return hr;
}

END_RDMA_NAMESPACE