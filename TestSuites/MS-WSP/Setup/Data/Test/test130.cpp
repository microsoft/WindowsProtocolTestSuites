// tsts2.cpp : Defines the entry point for the console application.
//
#define _CRT_SECURE_NO_DEPRECATE 
#include "stdafx.h"
#include "legacy.h"
#include "ms-dtyp.h"
#include <malloc.h>
//#include "winuser.h"
//#include "windows.h"
#include <string.h>
#include "stdio.h"
#include "winsta.h"
#include "allproc.h"
#define MEM_MAX 1024

//#include "Winsock2.h"



SERVER_HANDLE phServer;
HANDLE GetRPCBinding(LPWSTR pszServerName, int Flag_UUID, int Flag_NamedPipe);
extern HANDLE RpcIcaapiHandle = NULL;

/*******************OPNUM 1*****************************/
void Opnum1(SERVER_HANDLE phServer )
{
	DWORD* pResult2;
	pResult2=(DWORD*)malloc(sizeof(pResult2)); 
    BOOLEAN rc2 = RpcWinStationCloseServer(phServer,
	                                       pResult2
										   );
   if(TRUE != rc2)
   {
     printf("Error : RpcWinStationCloseServerEx \n");
   }
   else
   {
   printf("\n RpcWinStationCloseServerEx \n");
   }
    printf("Result %x :",*pResult2);


}
//BOOLEAN RpcWinStationNameFromLogonId( [in] SERVER_HANDLE hServer,
//									 [out] DWORD* pResult, 
//									 [in] DWORD LoginId, 
//									 [in, out, size_is(NameSize)] PWCHAR pWinStationName, 
//									 [in, range(0, 256 )] DWORD NameSize );
void Opnum9(SERVER_HANDLE phServer)
{
	DWORD pResult;
	DWORD LoginId=4;
	PWCHAR pWinStationName=(PWCHAR)malloc(sizeof(PWCHAR));
	DWORD NameSize=90;
	boolean rc9=RpcWinStationNameFromLogonId(phServer,&pResult,LoginId,pWinStationName,NameSize);
}
//BOOLEAN RpcWinStationSetInformation( [in] SERVER_HANDLE hServer, 
//									[out] DWORD* pResult, 
//									[in] DWORD LogonId, 
//									[in] DWORD WinStationInformationClass, 
//									[in, out, unique, size_is(WinStationInformationLength)] PCHAR pWinStationInformation,
//									[in, range(0, 0x8000)] DWORD WinStationInformationLength );
void Opnum5(SERVER_HANDLE phServer)
{
	DWORD pResult=0;
    DWORD LogonId =0;
	DWORD WinStationInformationClass=WinStationNtSecurity ;
    char *	 pWinStationInformation=(char *)malloc(sizeof(PCHAR));
   	DWORD WinStationInformationLength=100 ;
BOOLEAN rc5=RpcWinStationSetInformation(phServer,&pResult,LogonId,WinStationInformationClass,pWinStationInformation,WinStationInformationLength);

}/*
BOOLEAN RpcWinStationReset( [in] SERVER_HANDLE hServer,
						   [out] DWORD* pResult, 
						   [in] DWORD LogonId, 
						   [in] BOOLEAN bWait );*/
void Opnum14(SERVER_HANDLE phServer)
{   
	DWORD pResult6=0;
    DWORD LogonId =1;
	BOOLEAN bWait=true;
	BOOLEAN rc14=RpcWinStationReset(phServer,&pResult6,LogonId,bWait);

}
void Opnum6(SERVER_HANDLE phServer)
{

	DWORD pResult6=0;
    DWORD LogonId =1;
	DWORD pReturnLength;
 	DWORD WinStationInformationClass ;
    char *	 pWinStationInformation;
   	DWORD WinStationInformationLength ;
	DWORD enumvalue;
	
	
	WINSTATIONCREATEW w;
    WINSTATIONCONFIGW winstationconfigw;
	USERCONFIGW we;
	int sizeofwe=sizeof(we);

	int sizeofconfigw=sizeof(winstationconfigw);
	
    PDPARAMSW pdparamsw;
    WDCONFIGW wdconfig;
    PDCONFIGW pdconfigw;
    WINSTATIONCLIENTW winstationclientw;
    WINSTATIONINFORMATIONW winstationinformationw;
    WINSTATIONUSERTOKEN winstationusertoken;
    WINSTATIONVIDEODATA winstationvideodata;
	CDCONFIG cdconfig;
	WINSTATIONLOADINDICATORDATA loadindicatordata;
	WINSTATIONPRODID winstationproductid;
	BOOL lockedstate;
	WINSTATIONREMOTEADDRESS remoteaddress;
	remoteaddress.sin_family=AF_INET;
	ULONG idletime;
    ULONG reconnecttype;
while(1)
	{
	printf("\nselect the class of data:\n\tWinStationCreateData(0)");
	printf("\n\tWinStationConfiguration(1)\n\tWinStationPdParams(2)");
	printf("\n\tWinStationWd(3)\n\tWinStationPd(4)\n\tWinStationClient(6)\n\tWinStationModules(7)");
	printf("\n\tWinStationInformation(8)\n\tWinStationUserToken(14)");
    printf("\n\tWinStationVideoData(16)\n\tWinStationCd(18)\n\tWinStationLoadBalanceSessionTarget(24)");
	printf("\n\tWinStationLoadIndicator(25)\n\tWinStationShadowInfo(26)");
	printf("\n\tWinStationDigProductId(27)\n\tWinStationLockedState(28)");
	printf("\n\tWinStationRemoteAddress(29)\n\tWinStationIdleTime(30)");
	printf("\n\tWinStationLastReconnectType(31)");

	printf("\n");
    scanf("%d",&enumvalue);
	
    switch(enumvalue)
	{
	case 0 :													 //Method Call successful  
			 WinStationInformationClass=WinStationCreateData;
			 pWinStationInformation=(char *)&w;
			 WinStationInformationLength=sizeof(w);
			 break;
	case 1:													     //Giving ERROR code 0xc00000e8
			 WinStationInformationClass=WinStationConfiguration;
			 pWinStationInformation=(char *)&winstationconfigw;
			 WinStationInformationLength=2818;//sizeof(winstationconfigw);
			 break;
	case 2:													     //Giving ERROR code 0xc00000e8
		     WinStationInformationClass=WinStationPdParams;
		     pWinStationInformation=(char *)&pdparamsw;
			 WinStationInformationLength=sizeof(pdparamsw);
			 break;
	case 3:												         //Giving ERROR code 0xc00000e8
			 WinStationInformationClass=WinStationWd;
			 pWinStationInformation=(char *)&wdconfig;
			 WinStationInformationLength=sizeof(wdconfig);
			 break;
	case 4:														 //Giving ERROR code 0xc00000e8
		     WinStationInformationClass=WinStationPd;
			 pWinStationInformation=(char *)&pdconfigw;
			 WinStationInformationLength=sizeof(pdconfigw);
			 break;
	case 6:														 //Giving ERROR code 0xc00000e8
		     WinStationInformationClass=WinStationClient;
			 pWinStationInformation=(char *)&winstationclientw;
			 WinStationInformationLength=sizeof(winstationclientw);
			 break;
	case 7:														  //Method Call successful 
		     WinStationInformationClass=WinStationModules;
			 pWinStationInformation=(char *)midl_user_allocate(100);;
			 WinStationInformationLength=100;
			 break;
	case 8:														 //Giving ERROR code 0xc00000e8
		     WinStationInformationClass=WinStationInformation;
			 pWinStationInformation=(char *)&winstationclientw;
			 WinStationInformationLength=sizeof(winstationclientw);
			 break;
	case 14:
		     WinStationInformationClass=WinStationUserToken;
			 pWinStationInformation=(char *)&winstationusertoken;
			 WinStationInformationLength=sizeof(winstationusertoken);
			 break;
	case 16:                                     //Giving ERROR code 0xc00000e8 for console //for others it si giving success
		     WinStationInformationClass=WinStationVideoData;
			 pWinStationInformation=(char *)&winstationvideodata;
			 WinStationInformationLength=sizeof(winstationvideodata);
			 break;
	case 18:                                                   //Method Call successful 
		     WinStationInformationClass=WinStationCd;
			 pWinStationInformation=(char *)&cdconfig;
			 WinStationInformationLength=sizeof(cdconfig);
			 break;
	case 24:                                                   //Method Call successful 
		     WinStationInformationClass=WinStationLoadBalanceSessionTarget;
			 ULONG a;
			 pWinStationInformation=(char *)&a;
			 WinStationInformationLength=sizeof(a);
			 break;
	case 25:												   //Method Call successful 
		     WinStationInformationClass=WinStationLoadIndicator;
			 pWinStationInformation=(char *)&loadindicatordata;
			 WinStationInformationLength=sizeof(loadindicatordata);
			 break;
	case 26:                                                  //Method Call successful
		     WinStationInformationClass=WinStationLoadIndicator;
			 pWinStationInformation=(char *)&loadindicatordata;
			 WinStationInformationLength=sizeof(loadindicatordata);
			 break;
	case 27:												   //Method Call successful
		     WinStationInformationClass=WinStationDigProductId;
			 pWinStationInformation=(char *)&winstationproductid;
			 WinStationInformationLength=sizeof(winstationproductid);
			 break;
	case 28:													//Method Call successful
		     WinStationInformationClass=WinStationLockedState;
			 pWinStationInformation=(char *)&lockedstate;
			 WinStationInformationLength=sizeof(lockedstate);
			 break;
	case 29:													//Method Call successful
		     WinStationInformationClass=WinStationRemoteAddress;
			 pWinStationInformation=(char *)&remoteaddress;
			 WinStationInformationLength=sizeof(remoteaddress);
			 break;
	case 30:                                                 //Method Call successful
		     WinStationInformationClass=WinStationIdleTime;
			 pWinStationInformation=(char *)&idletime;
			 WinStationInformationLength=sizeof(idletime);
			 break;
	case 31:                                                 //Method Call successful
			 WinStationInformationClass=WinStationLastReconnectType;
			 pWinStationInformation=(char *)&reconnecttype;
			 WinStationInformationLength=sizeof(reconnecttype);
			 break;
	default: 
		     exit(0);
	}
	int i=0;
	BOOLEAN rc6;
 //for (i=1000;i<3500;i++)
 //{
	// pWinStationInformation=(char*)malloc(i);
	// for (int j=0;j<i;j++)
	// {
	//	 pWinStationInformation[j]=0x00;
	// }
	rc6 = RpcWinStationQueryInformation(phServer,
	                                            &pResult6,
											    LogonId,
											    WinStationInformationClass,
												pWinStationInformation,
												WinStationInformationLength,
											    &pReturnLength
												);
//
//	if(pResult6 == 0)
//	{
//	/*	break;
//      printf("\n\nFound..  ");*/
//	}
//	else
//	{
//		free(pWinStationInformation);
//	}
//	::Sleep(100);
//
// }
//// printf("The correct size is : %d\n",i);
	

	if(TRUE != rc6)
	{
      printf("\n\nError :RpcWinStationQueryInformation  ");
	}
	else
	{

	 printf("\nRpcWinStationQueryInformation  call successful ");
	}
     printf("\nError code of RpcWinStationQueryInformation %x :",pResult6);
}
}





void Opnum7(SERVER_HANDLE phServer)
{
	DWORD *pResult7;
    pResult7 =(DWORD*)malloc(sizeof(pResult7));
    DWORD LogonId2 =0;
	wchar_t* pTitle=(wchar_t*)malloc(1024) ;
	pTitle = L"title";
	DWORD TitleLength = (DWORD)wcslen(pTitle);
	wchar_t *pMessage=(wchar_t*)malloc(1024);
	pMessage = L"message2";
	DWORD MessageLength=(DWORD)wcslen(pMessage);
	DWORD Style=MB_YESNOCANCEL;
	DWORD Timeout=5;
	DWORD* pRespsonse=(DWORD*)malloc(sizeof(DWORD));
	BOOLEAN DoNotWait=FALSE;

	BOOLEAN rc7 = RpcWinStationSendMessage(phServer,
		                                   pResult7,
                                           LogonId2,
                                           pTitle,
										   TitleLength,
                                           pMessage,
                                           MessageLength,
                                           Style,
										   Timeout,
                                           pRespsonse,
                                           DoNotWait
										   );
   if(TRUE != rc7)
   {
      printf(" \n\nError :RpcWinStationSendMessage  ");
   }
   else
   {
	  printf(" \nRpcWinStationSendMessage pRespsonse :%d\n",*pRespsonse);
   }
   printf("Result %x :",*pResult7);
}


 

 void Opnum30(SERVER_HANDLE phServer)
{
	DWORD *pResult30=(DWORD *)malloc(sizeof(DWORD));
	BOOLEAN rc30=RpcWinStationReadRegistry(phServer,pResult30);
	if(rc30!=TRUE)
	{
		printf("\nRpcWinStationReadRegistry method call failed \n");
	}
	else
	{
	printf("\nRpcWinStationReadRegistry method call sucessful \n");
	}

	printf("\nPResult value %x \n",*pResult30);

}/*

BOOLEAN RpcWinStationGetMachinePolicy( [in] SERVER_HANDLE hServer, 
									  [in, out, size_is(bufferSize)] PBYTE pPolicy,
									  [in, range(0, 0x8000 )] ULONG bufferSize );*/


//BOOLEAN RpcWinStationWaitSystemEvent( [in] SERVER_HANDLE hServer, 
//									 [out] DWORD* pResult,
//									 [in] DWORD EventMask, 
//									 [out] DWORD* pEventFlags );
 void Opnum16(SERVER_HANDLE phServer)
 {
	DWORD pResult16=0;
	DWORD EventMask=0x00000010;
	 DWORD pEventFlags;
BOOLEAN rc16=RpcWinStationWaitSystemEvent(phServer,&pResult16,EventMask,&pEventFlags);
 }
 void Opnum17(SERVER_HANDLE phServer)
{
	DWORD pResult;
	DWORD LogonId=4;
	PWCHAR pTargetServerName=NULL;
	DWORD NameSize=0;
    DWORD TargetLogonId=2;
	BYTE HotKeyVk=0;
	USHORT HotkeyModifiers=(unsigned short)(11 && 12);;

BOOLEAN rc15=RpcWinStationShadow(phServer, &pResult,LogonId,pTargetServerName,NameSize,TargetLogonId,HotKeyVk,HotkeyModifiers);
printf("\n retun value =%d,Error code=%x",rc15,pResult);
//							
 

}
 void Opnum62(SERVER_HANDLE phServer)
 
 {
	 
	 POLICY_TS_MACHINE ps;
	 PBYTE pPolicy=(PBYTE)&ps;
	ULONG bufferSize=sizeof(ps);;
BOOLEAN rc62=RpcWinStationGetMachinePolicy( phServer,pPolicy,bufferSize );
if(rc62!=TRUE)
{
	printf("\n getmachinepolicy failed");
}
else
{
	printf("\n getmachinepolcy call successful");
}
 }
void Opnum66(SERVER_HANDLE phServer)
{
	/*DWORD pResult;
	DWORD TimeOut=10;
	ULONG AddressType=TDI_ADDRESS_TYPE_IP;
	TDI_ADDRESS_IP ip;
	const char *ipaddr="10.50.212.31";
	ip.in_addr=inet_addr (ipaddr);
	ip.sin_port=80;
	ip.sin_zero[0]=0;
	PBYTE pAddress=(PBYTE)&ip;
	ULONG AddressSize=TDI_ADDRESS_LENGTH_IP;

BOOLEAN rc66=RpcConnectCallback(phServer,
								&pResult,
								TimeOut,
								AddressType,
								pAddress,
								AddressSize);
if(rc66!=TRUE)
{
	printf("\n RpcConnectCallback call failed");

}
else
{
	printf("\n RpcConnectCallback  call successful");
}
printf("\n error code =%x",pResult);*/
}
void Opnum75(SERVER_HANDLE phServer)
{
//BOOLEAN RpcWinStationOpenSessionDirectory( [in] SERVER_HANDLE hServer, [out] DWORD* pResult, [in, string, max_is(64)] PWCHAR pszServerName );
DWORD *pResult75=(DWORD*)malloc(sizeof(DWORD));
PWCHAR pszServerName = L"wipro-0b56fc19a";

BOOLEAN rc75 = RpcWinStationOpenSessionDirectory(phServer,pResult75,pszServerName);

if(rc75!=TRUE)
	{
		printf("\n RpcWinStationOpenSessionDirectory  call failed \n");
	}
	else
	{

		printf("RpcWinStationOpenSessionDirectory call sucessful ");
	}
	printf("PResult value %x",*pResult75);
}
static LARGE_INTEGER ProcessStartTime;
static DWORD dwUniqueProcessId;
void opnum43(SERVER_HANDLE phServer)
{ 
	DWORD *pResult43=(DWORD*)malloc(sizeof(DWORD));
    ULONG  Level=0;
    ULONG *pNumberOfProcesses=(ULONG *)malloc(sizeof(ULONG)); 
    *pNumberOfProcesses =1;
	ULONG i=0;
	
	PTS_ALL_PROCESSES_INFO *ppTsAllProcessesInfo=new PTS_ALL_PROCESSES_INFO();
    BOOLEAN rc43=RpcWinStationGetAllProcesses(phServer,
	                                          pResult43,
											  Level,
                                              pNumberOfProcesses,
                                              ppTsAllProcessesInfo);
//	PTS_ALL_PROCESSES_INFO *ptr=ppTsAllProcessesInfo;
   if(rc43==FALSE)
   {
     printf("\n\n  RpcWinStationGetAllProcesses failed" );
   }
   else
   {
     printf("\n RpcWinStationGetAllProcesses sucessful\n");
   }
	 printf("\n no of processes %ld",*pNumberOfProcesses);
	 for(i=0;i<*pNumberOfProcesses;i++)
	 {
		
		 printf("\n Session id   :%l",((*ppTsAllProcessesInfo)->pTsProcessInfo->SessionId));
	//	 printf("\nsid           :%d",((*ppTsAllProcessesInfo)->SizeOfSid));
	//	 printf("\n no of threads:%ld",(*ppTsAllProcessesInfo)->pTsProcessInfo->NumberOfThreads);
	//	 printf("\nHighPart      :%d",(*ppTsAllProcessesInfo)->pTsProcessInfo->CreateTime.HighPart);
	//	 printf("\nLowPart       :%d",(*ppTsAllProcessesInfo)->pTsProcessInfo->CreateTime.LowPart);
	//	 printf("\nQuadPart      :%d",(*ppTsAllProcessesInfo)->pTsProcessInfo->CreateTime.QuadPart);
	//	 printf("\n u            :%d",(*ppTsAllProcessesInfo)->pTsProcessInfo->CreateTime.u);
		 printf("\n \n unique id    :%ld",(*ppTsAllProcessesInfo)->pTsProcessInfo->UniqueProcessId);
		 if((*ppTsAllProcessesInfo)->pTsProcessInfo->UniqueProcessId==368)//winlogon process id
		 {
			 ProcessStartTime=(*ppTsAllProcessesInfo)->pTsProcessInfo->CreateTime;
			 dwUniqueProcessId=(*ppTsAllProcessesInfo)->pTsProcessInfo->UniqueProcessId;
			 printf("\n/n/n/yahooooooooo\n");
			 break;
		 } 
		
		 
	//	ptr++;
		(*ppTsAllProcessesInfo)++;

   }
  printf("\n\n error code  %x:  ",*pResult43);
  printf("\n RETURN VALUE %d",rc43);
}


/**********************RpcWinStationGetProcessSid****************************/
 void opnum44(SERVER_HANDLE phServer)
{
	//DWORD dwUniqueProcessId=516;
	LONG* pResult44=(LONG*)malloc(sizeof(LONG));
	PBYTE pProcessUserSid=(PBYTE)malloc(sizeof(PBYTE));;
	//DWORD dwSidSize=(DWORD)wcslen(pProcessUserSid);
	DWORD dwSidSize=12;
	DWORD* pdwSizeNeeded=(DWORD*)malloc(sizeof(DWORD));
	  
	
    BOOLEAN rc44=RpcWinStationGetProcessSid(phServer,
											   dwUniqueProcessId,
											   ProcessStartTime,
									     	    pResult44,
											pProcessUserSid,
											dwSidSize,
											pdwSizeNeeded);

	  if(rc44==FALSE)
	  {
		  printf("RpcWinStationGetProcessSid method call Failed");
	  }
	  else
	  {
		printf("SID is %u",*pProcessUserSid);
	  }
	  printf("\n\n RESULT  %x:  ",*pResult44);
}

 void Opnum45(SERVER_HANDLE phServer)
{
	DWORD *pResult45=(DWORD*)malloc(sizeof(DWORD));
    
	DWORD dwEntries=(DWORD)malloc(sizeof(DWORD));
	dwEntries=5;
	TS_COUNTER tsCounter[6];
	tsCounter[0].counterHead.dwCounterID=TERMSRV_TOTAL_SESSIONS;
	tsCounter[1].counterHead.dwCounterID=TERMSRV_DISC_SESSIONS;
	tsCounter[2].counterHead.dwCounterID=TERMSRV_RECON_SESSIONS;
	tsCounter[3].counterHead.dwCounterID=TERMSRV_CURRENT_ACTIVE_SESSIONS;
	tsCounter[4].counterHead.dwCounterID=TERMSRV_CURRENT_DISC_SESSIONS;

    PTS_COUNTER pCounter=tsCounter;
	

	
	BOOLEAN rc45 = RpcWinStationGetTermSrvCountersValue(phServer,
	                                          pResult45,
                                              dwEntries,
											  pCounter
											  );

   if(TRUE != rc45)	
	{
      printf(" \n\nError :RpcWinStationGetTermSrvCountersValue");
	}
	else
	{
	    printf(" \n\n RpcWinStationGetTermSrvCountersValue");
				
	 }
   printf(" \n Result : %x \n",*pResult45);
   
   printf(" \n bResult : %d \n",pCounter->counterHead.bResult);
   printf(" \n dwCounterID : %d \n",pCounter->counterHead.dwCounterID);
}
//
//BOOLEAN RpcWinStationIsHelpAssistantSession( [in] SERVER_HANDLE hServer,
//											[out] DWORD* pResult,
//											[in] ULONG SessionId );
 void Opnum61(SERVER_HANDLE phServer)
 {
	 DWORD pResult=88;
	 BOOLEAN rc61=RpcWinStationIsHelpAssistantSession(phServer,&pResult,2);
 }
int _tmain(int argc, _TCHAR* argv[])
{
	
	 TCHAR *szChoice=(TCHAR*)malloc(sizeof(szChoice)); 
	 DWORD* pResult1;
	 DWORD dwTestCaseNo;
	 pResult1=(DWORD*)malloc(sizeof(pResult1)); 

     printf("Starting the application\n");
     RpcIcaapiHandle = GetRPCBinding(TEXT("10.50.212.121"), 6, 3);

	 if(!RpcIcaapiHandle)
     {
       printf("\nError in Getting RPC Binding Handle \n");
	 }      
     else
     {
        printf("\n RPC Binding Handle \n");
     }	

   /*********OPNUM 0**************/
    BOOLEAN rc1 = RpcWinStationOpenServer( RpcIcaapiHandle, 
		                                   pResult1,
										   &phServer
	     								   );
   if(TRUE!= rc1)
   {
     printf("Error : RpcWinStationOpenServer \n");
   }
   else
   {
   printf("\n RpcWinStationOpenServer \n");
   }
   printf("\n\nResult %x :  ",*pResult1);
   printf("\n Return value %d",rc1);
   printf("\n\nServer %S : ",phServer);

	while(1) 
	{
	 printf("\n Enter the Opnum number to be exceuted :");
	 scanf("%d",&dwTestCaseNo);
	 printf("\n You have selected %d ", dwTestCaseNo);
	 switch(dwTestCaseNo)
	 {
	 case 5:
		 Opnum5(phServer);
		 break;
	  case 6:
		  Opnum6(phServer);
		  break;
      case 7:
		 Opnum7(phServer);
		 break;
	  case 9:
		  Opnum9(phServer);
		  break;
	  case 14:
		  Opnum14(phServer);
		  break;
	  case 16:
		  Opnum16(phServer);
		  break;
	  case 17:
		  Opnum17(phServer);
		  break;
	  case 30:
		  Opnum30(phServer);
		  break;
	  case 61:
		  Opnum61(phServer);
		  break;
	  case 62:
		  Opnum62(phServer);
		  break;
	  case 66:
		  Opnum66(phServer);
		  break;
	   case 43:
		  printf("\n calling RpcWinStationGetAllProcesses method\n");
		  opnum43(phServer);
		  break;
	  case 44:
		  printf("\n calling RpcWinStationGetProcessSid method\n");
		  opnum44(phServer);
		  break;
	  case 45:
		  Opnum45(phServer);
		  break;
	  case 75:
		  Opnum75(phServer);
		  break;
	  default: 
		   printf("\n Opnum Number Does Not Exist.");
		   break;
	 }	
	 

	 printf("Do you want to continue (Y/N) \n ");
	 scanf("%S",szChoice);


	 if (*szChoice == 'N' || *szChoice == 'n')
	 {
		printf("You have selected 'N' \n");
		break;
	 }
	 else if (*szChoice == 'Y'|| *szChoice == 'y')
	 {
	    printf("You have selected 'Y' \n");
		continue;
	 }
	 else
	 {	
	  printf("Invalid Choice , stopping the execution\n");
	  break;	
	 }
	}
     return 0;
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                