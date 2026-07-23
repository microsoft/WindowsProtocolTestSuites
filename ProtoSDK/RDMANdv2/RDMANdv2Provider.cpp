// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// RDMANdv2.h must come first so that <ndspi.h> pulls in <winsock2.h>
// before <WS2spi.h> (whose transitive <windows.h> would otherwise drag
// in the legacy <winsock.h>).
#include "RDMANdv2.h"
#include <WS2spi.h>
#include <stdlib.h>


using namespace System;
using namespace System::Runtime::InteropServices;


// Begin RDMANdv2 namespace
BEGIN_RDMA_NDV2_NAMESPACE

WCHAR * GetProviderPath(
    WSAPROTOCOL_INFOW* pProtocol)
{
    INT pathLen;
    INT ret, err;
    WCHAR * pPath;
    WCHAR * pPathEx;

    ret = WSCGetProviderPath(&pProtocol->ProviderId, NULL, &pathLen, &err );
    if( err != WSAEFAULT || pathLen == 0 ) { return NULL; }

    pPath = (WCHAR*)HeapAlloc( GetProcessHeap(), 0, sizeof(WCHAR) * pathLen );
    if( pPath == NULL ) { return NULL; }

    ret = WSCGetProviderPath( &pProtocol->ProviderId, pPath, &pathLen, &err );
    if( ret != 0 )
    {
        HeapFree( GetProcessHeap(), 0, pPath );
        return NULL;
    }

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
    HeapFree( GetProcessHeap(), 0, pPath );
    if( ret != pathLen )
    {
        HeapFree( GetProcessHeap(), 0, pPathEx );
        return NULL;
    }

    return pPathEx;
}

/// <summary>
/// Load providers.
/// </summary>
HRESULT RdmaProvider::LoadRdmaProviders([Out]array<RdmaProviderInfo^>^% providers)
{
    // Same Winsock-SPI enumeration as v1: NDSPI v1 and v2 providers register
    // through the same set of WSP service flags, so the filter is unchanged.
    DWORD proLen = 0;
    INT proErr = 0;

    INT proRet = WSCEnumProtocols(
        NULL,
        NULL,
        &proLen,
        &proErr);

    if( proRet != SOCKET_ERROR || proErr != WSAENOBUFS )
    {
        return ND_INTERNAL_ERROR;
    }

    WSAPROTOCOL_INFOW* pProtocols = (WSAPROTOCOL_INFOW*)HeapAlloc(GetProcessHeap(), 0, proLen );
    if( pProtocols == NULL ) { return ND_NO_MEMORY; }

    proRet = WSCEnumProtocols( NULL, pProtocols, &proLen, &proErr );
    if( proRet == SOCKET_ERROR )
    {
        HeapFree( GetProcessHeap(), 0, pProtocols );
        return ND_INTERNAL_ERROR;
    }

    int proCount = proLen / sizeof(WSAPROTOCOL_INFOW);
    NTSTATUS result;

    providers = gcnew array<RdmaProviderInfo^>(proCount);

    WCHAR *pPath = NULL;
    for(int i = 0; i< proCount; ++i)
    {
        if(pPath != NULL) HeapFree( GetProcessHeap(), 0, pPath );
        pPath = GetProviderPath(&pProtocols[i]);
        providers[i] = gcnew RdmaProviderInfo();
        if (pPath != NULL)
        {
            providers[i]->Path = gcnew String(pPath);
        }
        else
        {
            providers[i]->Path = gcnew String("");
        }

        #define ServiceFlags1Flags (XP1_GUARANTEED_DELIVERY | XP1_GUARANTEED_ORDER | XP1_MESSAGE_ORIENTED | XP1_CONNECT_DATA)

        if( (pProtocols[i].dwServiceFlags1 & ServiceFlags1Flags) != ServiceFlags1Flags ) { continue; }
        if( pProtocols[i].iAddressFamily != AF_INET && pProtocols[i].iAddressFamily != AF_INET6 )  { continue; }
        if( pProtocols[i].iSocketType != -1 )  { continue; }
        if( pProtocols[i].iProtocol != 0 )  { continue; }
        if( pProtocols[i].iProtocolMaxOffset != 0 )  { continue; }

        IND2Provider *provider;
        HMODULE libraryHandler;
        result = LoadProvider(
            pPath,
            pProtocols[i].ProviderId,
            &provider,
            &libraryHandler);
        if(result == ND_SUCCESS)
        {
            providers[i]->Provider = gcnew RdmaProvider(provider, libraryHandler);
        }
    }

    if(pPath != NULL) HeapFree( GetProcessHeap(), 0, pPath );
    HeapFree( GetProcessHeap(), 0, pProtocols );

    return ND_SUCCESS;
}

/// <summary>
/// Load provider.
/// </summary>
HRESULT RdmaProvider::LoadRdmaProvider(System::Guid providerId, String^ path, [Out]RdmaProviderInfo^% provider)
{
    array<Byte>^ guidData = providerId.ToByteArray();
    pin_ptr<Byte> data = &(guidData[ 0 ]);
    _GUID innerProviderId = *(_GUID *)data;

    IntPtr ptrPath = Marshal::StringToHGlobalAnsi(path);
    char* pPath = static_cast<char*>(ptrPath.ToPointer());
    size_t newSize = path->Length + 1;
    size_t convertedSize = 0;
    WCHAR *pwPath = (WCHAR*)malloc(newSize*sizeof(WCHAR));
    NTSTATUS result = mbstowcs_s(&convertedSize, pwPath, newSize, pPath, _TRUNCATE);
    Marshal::FreeHGlobal(ptrPath);
    if(result != ND_SUCCESS)
    {
        free(pwPath);
        return result;
    }

    IND2Provider *pProvider;
    HMODULE libraryHandler;
    result = LoadProvider(
            pwPath,
            innerProviderId,
            &pProvider,
            &libraryHandler);
    free(pwPath);
    if(result == ND_SUCCESS)
    {
        provider = gcnew RdmaProviderInfo();
        provider->Provider = gcnew RdmaProvider(pProvider, libraryHandler);
        provider->Path = path;
    }
    return result;
}

/// <summary>
/// Destructor, release library resource and provider.
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
/// Open adapter with specific address.
/// </summary>
HRESULT RdmaProvider::OpenAdapter(String^ ipAddress, short ipFamily, [Out]RdmaAdapter^% adapter)
{
    IntPtr ptrLocalIP = Marshal::StringToHGlobalAnsi(ipAddress);
    const char* pLocalIP = static_cast<const char*>(ptrLocalIP.ToPointer());

    sockaddr_in localAddr = {0};
    localAddr.sin_family = ipFamily;
    localAddr.sin_port = 0;
    localAddr.sin_addr.s_addr = inet_addr(pLocalIP);

    UINT64 adapterId;
    HRESULT result = _provider->ResolveAddress(
        (const sockaddr *)&localAddr,
        sizeof(localAddr),
        &adapterId
        );
    if(result != ND_SUCCESS)
    {
        Marshal::FreeHGlobal(ptrLocalIP);
        return result;
    }

    IND2Adapter *nativeAdapter;
    result = _provider->OpenAdapter(
        IID_IND2Adapter,
        adapterId,
        reinterpret_cast<void**>(&nativeAdapter)
        );
    if(result != ND_SUCCESS)
    {
        Marshal::FreeHGlobal(ptrLocalIP);
        return result;
    }

    adapter = gcnew RdmaAdapter(nativeAdapter);
    adapter->_localIpAddress = ipAddress;
    adapter->_ipFamily = ipFamily;

    Marshal::FreeHGlobal(ptrLocalIP);
    return result;
}

/// <summary>
/// Retrieves a list of local addresses that the provider supports.
/// </summary>
HRESULT RdmaProvider::QueryAddressList([Out]array<RdmaAddress^>^% addressList)
{
    SOCKET_ADDRESS_LIST *innerAddressList;
    ULONG bufferSize = 0;

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

RdmaProvider::RdmaProvider(IND2Provider *provider, HMODULE libraryHandler)
{
    _provider = provider;
    _libraryHandler = libraryHandler;
}

/// <summary>
/// Load provider.
/// </summary>
NTSTATUS RdmaProvider::LoadProvider(
    WCHAR * path,
    _GUID protocolId,
    IND2Provider **pProvider,
    HMODULE *pLibraryHandler)
{
    // Load the provider's DLL and resolve its class factory.
    *pLibraryHandler = LoadLibraryW(path);
    if (*pLibraryHandler == NULL)
    {
        return __HRESULT_FROM_WIN32(GetLastError());
    }

    DLLGETCLASSOBJECT pfnDllGetClassObject = (DLLGETCLASSOBJECT)GetProcAddress(
        *pLibraryHandler,
        "DllGetClassObject");
    if (pfnDllGetClassObject == NULL)
    {
        FreeLibrary(*pLibraryHandler);
        *pLibraryHandler = NULL;
        return __HRESULT_FROM_WIN32(GetLastError());
    }

    IClassFactory* pClassFactory;
    HRESULT hr = pfnDllGetClassObject(
        protocolId,
        IID_IClassFactory,
        (void**)&pClassFactory);

    if (FAILED(hr))
    {
        FreeLibrary(*pLibraryHandler);
        *pLibraryHandler = NULL;
        return hr;
    }

    // NDSPI v2 headers define IID_IND2Provider via DEFINE_GUID but do not
    // attach the GUID to the interface type, so use the constant directly.
    GUID riidProvider = IID_IND2Provider;

    hr = pClassFactory->CreateInstance(
        NULL,
        riidProvider,
        (void**)pProvider);

    pClassFactory->Release();

    if (FAILED(hr))
    {
        FreeLibrary(*pLibraryHandler);
        *pLibraryHandler = NULL;
    }

    return hr;
}

END_RDMA_NDV2_NAMESPACE
