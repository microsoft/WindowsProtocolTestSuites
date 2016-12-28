

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 7.00.0500 */
/* at Fri Aug 01 11:56:56 2008
 */
/* Compiler settings for ms-dtyp.idl:
    Oicf, W1, Zp8, env=Win32 (32b run)
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
//@@MIDL_FILE_HEADING(  )

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


#ifndef __ms2Ddtyp_h__
#define __ms2Ddtyp_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"

#ifdef __cplusplus
extern "C"{
#endif 


/* interface __MIDL_itf_ms2Ddtyp_0000_0000 */
/* [local] */ 

typedef int BOOL;

typedef int *PBOOL;

typedef int *LPBOOL;

typedef unsigned char BYTE;

typedef unsigned char *PBYTE;

typedef unsigned char *LPBYTE;

typedef BYTE BOOLEAN;

typedef BYTE *PBOOLEAN;

typedef wchar_t WCHAR;

typedef wchar_t *PWCHAR;

typedef WCHAR *BSTR;

//typedef unsigned char CHAR;

//typedef unsigned char *PCHAR;

typedef double DOUBLE;

typedef unsigned long DWORD;

typedef unsigned long *PDWORD;

typedef unsigned long *LPDWORD;

typedef unsigned int DWORD32;

typedef unsigned __int64 DWORD64;

typedef unsigned __int64 ULONGLONG;

typedef ULONGLONG DWORDLONG;

typedef ULONGLONG *PDWORDLONG;

typedef unsigned long error_status_t;

typedef float FLOAT;

typedef unsigned char UCHAR;

typedef unsigned char *PUCHAR;

typedef short SHORT;

typedef void *HANDLE;

typedef DWORD HCALL;

typedef int INT;

typedef int *LPINT;

typedef signed char INT8;

typedef short INT16;

typedef int INT32;

typedef __int64 INT64;

typedef const wchar_t *LMCSTR;

typedef WCHAR *LMSTR;

typedef long LONG;

typedef long *PLONG;

typedef long *LPLONG;

typedef INT64 LONGLONG;

typedef LONG HRESULT;

//typedef /* [custom] */ __int3264 LONG_PTR;

//typedef /* [custom] */ unsigned __int3264 ULONG_PTR;

typedef int LONG32;

typedef __int64 LONG64;

//typedef const unsigned char *LPCSTR;

typedef const wchar_t *LPCWSTR;

//typedef unsigned char *PSTR;

//typedef unsigned char *LPSTR;

typedef wchar_t *LPWSTR;

typedef wchar_t *PWSTR;

typedef DWORD NET_API_STATUS;

typedef long NTSTATUS;

typedef /* [context_handle] */ void *PCONTEXT_HANDLE;

typedef /* [ref] */ PCONTEXT_HANDLE *PPCONTEXT_HANDLE;

typedef unsigned __int64 QWORD;

typedef void *RPC_BINDING_HANDLE;

typedef UCHAR *STRING;

typedef __int64 TIME;

typedef unsigned int UINT;

typedef unsigned char UINT8;

typedef unsigned short UINT16;

typedef unsigned int UINT32;

typedef unsigned __int64 UINT64;

typedef unsigned long ULONG;

typedef unsigned long *PULONG;

typedef ULONG_PTR DWORD_PTR;

typedef ULONG_PTR SIZE_T;

typedef unsigned int ULONG32;

typedef unsigned __int64 ULONG64;

//typedef wchar_t UNICODE;

typedef unsigned short USHORT;

//typedef void VOID;

typedef void *PVOID;

typedef unsigned short WORD;

typedef unsigned short *PWORD;

typedef unsigned short *LPWORD;

//typedef struct _GUID
//    {
//    unsigned long Data1;
//    unsigned short Data2;
//    unsigned short Data3;
//    byte Data4[ 8 ];
//    } 	GUID;

typedef struct _GUID UUID;

typedef struct _GUID *PGUID;

typedef DWORD LCID;

typedef struct _RPC_UNICODE_STRING
    {
    unsigned short Length;
    unsigned short MaximumLength;
    WCHAR *Buffer;
    } 	RPC_UNICODE_STRING;

typedef struct _RPC_UNICODE_STRING *PRPC_UNICODE_STRING;

typedef struct _UINT128
    {
    UINT64 lower;
    UINT64 upper;
    } 	UINT128;

typedef struct _UINT128 *PUINT128;

typedef struct _UNICODE_STRING
    {
    USHORT Length;
    USHORT MaximumLength;
    WCHAR *Buffer;
    } 	UNICODE_STRING;

typedef struct _UNICODE_STRING *PUNICODE_STRING;

//typedef struct _ACCESS_MASK
//    {
//    unsigned long ACCESS_MASK;
//    } 	ACCESS_MASK;

//typedef struct _ACCESS_MASK *PACCESS_MASK;

//typedef struct _ACE_HEADER
//    {
//    UCHAR AceType;
//    UCHAR AceFlags;
//    USHORT AceSize;
//    } 	ACE_HEADER;

typedef struct _ACE_HEADER *PACE_HEADER;

//typedef struct _ACCESS_ALLOWED_ACE
//    {
//    ACE_HEADER Header;
//    ACCESS_MASK Mask;
//    DWORD SidStart;
//    } 	ACCESS_ALLOWED_ACE;

typedef struct _ACCESS_ALLOWED_ACE *PACCESS_ALLOWED_ACE;

//typedef struct _ACCESS_ALLOWED_OBJECT_ACE
//    {
//    ACE_HEADER Header;
//    ACCESS_MASK Mask;
//    DWORD Flags;
//    GUID ObjectType;
//    GUID InheritedObjectType;
//    DWORD SidStart;
//    } 	ACCESS_ALLOWED_OBJECT_ACE;

typedef struct _ACCESS_ALLOWED_OBJECT_ACE *PACCESS_ALLOWED_OBJECT_ACE;

//typedef struct _ACCESS_DENIED_ACE
//    {
//    ACE_HEADER Header;
//    ACCESS_MASK Mask;
//    DWORD SidStart;
//    } 	ACCESS_DENIED_ACE;

typedef struct _ACCESS_DENIED_ACE *PACCESS_DENIED_ACE;

//typedef struct _ACCESS_ALLOWED_CALLBACK_ACE
//    {
//    ACE_HEADER Header;
//    ACCESS_MASK Mask;
//    DWORD SidStart;
//    } 	ACCESS_ALLOWED_CALLBACK_ACE;

typedef struct _ACCESS_ALLOWED_CALLBACK_ACE *PACCESS_ALLOWED_CALLBACK_ACE;

//typedef struct _ACCESS_DENIED_CALLBACK_ACE
//    {
//    ACE_HEADER Header;
//    ACCESS_MASK Mask;
//    DWORD SidStart;
//    } 	ACCESS_DENIED_CALLBACK_ACE;

typedef struct _ACCESS_DENIED_CALLBACK_ACE *PACCESS_DENIED_CALLBACK_ACE;

//typedef struct _ACCESS_ALLOWED_CALLBACK_OBJECT_ACE
//    {
//    ACE_HEADER Header;
//    ACCESS_MASK Mask;
//    DWORD Flags;
//    GUID ObjectType;
//    GUID InheritedObjectType;
//    DWORD SidStart;
//    } 	ACCESS_ALLOWED_CALLBACK_OBJECT_ACE;

typedef struct _ACCESS_ALLOWED_CALLBACK_OBJECT_ACE *PACCESS_ALLOWED_CALLBACK_OBJECT_ACE;

//typedef struct _ACCESS_DENIED_CALLBACK_OBJECT_ACE
//    {
//    ACE_HEADER Header;
//    ACCESS_MASK Mask;
//    DWORD Flags;
//    GUID ObjectType;
//    GUID InheritedObjectType;
//    DWORD SidStart;
//    } 	ACCESS_DENIED_CALLBACK_OBJECT_ACE;

typedef struct _ACCESS_DENIED_CALLBACK_OBJECT_ACE *PACCESS_DENIED_CALLBACK_OBJECT_ACE;

//typedef struct _SYSTEM_AUDIT_ACE
//    {
//    ACE_HEADER Header;
//    ACCESS_MASK Mask;
//    DWORD SidStart;
//    } 	SYSTEM_AUDIT_ACE;

typedef struct _SYSTEM_AUDIT_ACE *PSYSTEM_AUDIT_ACE;

//typedef struct _SYSTEM_AUDIT_CALLBACK_ACE
//    {
//    ACE_HEADER Header;
//    ACCESS_MASK Mask;
//    DWORD SidStart;
//    } 	SYSTEM_AUDIT_CALLBACK_ACE;

typedef struct _SYSTEM_AUDIT_CALLBACK_ACE *PSYSTEM_AUDIT_CALLBACK_ACE;

//typedef struct _SYSTEM_MANDATORY_LABEL_ACE
//    {
//    ACE_HEADER Header;
//    ACCESS_MASK Mask;
//    DWORD SidStart;
//    } 	SYSTEM_MANDATORY_LABEL_ACE;

typedef struct _SYSTEM_MANDATORY_LABEL_ACE *PSYSTEM_MANDATORY_LABEL_ACE;

//typedef struct _SYSTEM_AUDIT_CALLBACK_OBJECT_ACE
//    {
//    ACE_HEADER Header;
//    ACCESS_MASK Mask;
//    DWORD Flags;
//    GUID ObjectType;
//    GUID InheritedObjectType;
//    DWORD SidStart;
//    } 	SYSTEM_AUDIT_CALLBACK_OBJECT_ACE;

typedef struct _SYSTEM_AUDIT_CALLBACK_OBJECT_ACE *PSYSTEM_AUDIT_CALLBACK_OBJECT_ACE;

typedef DWORD SECURITY_INFORMATION;

typedef DWORD *PSECURITY_INFORMATION;

typedef struct _RPC_SID
    {
    unsigned char Revision;
    unsigned char SubAuthorityCount;
    SID_IDENTIFIER_AUTHORITY IdentifierAuthority;
    unsigned long SubAuthority[ 1 ];
    } 	RPC_SID;

typedef struct _RPC_SID *PRPC_SID;



extern RPC_IF_HANDLE __MIDL_itf_ms2Ddtyp_0000_0000_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_ms2Ddtyp_0000_0000_v0_0_s_ifspec;

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


