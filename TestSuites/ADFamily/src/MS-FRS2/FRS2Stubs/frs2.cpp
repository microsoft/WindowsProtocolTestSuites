// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#include "stdafx.h"
//#include "ms-dtyp.h"
#include "ms-frs2.h"
#include <malloc.h>
#include  <iostream>
#include <vector>
using namespace std;




void  __RPC_FAR * __RPC_USER midl_user_allocate(size_t len)
{
	return (malloc(len));
}

void __RPC_USER midl_user_free(void __RPC_FAR * ptr)
{
	free(ptr);
}



DWORD WaitAsyncResult(RPC_ASYNC_STATE* state)
{
	RPC_STATUS  Status  = 0; 
	DWORD nReply = 100;
	int totalSleep = 60000;
	if(NULL == state) throw 0x999;
	do 
	{
		Status = RpcAsyncGetCallStatus(state);
		if(RPC_S_OK == Status)
		{
			Status = RpcAsyncCompleteCall(state, &nReply);
			if(RpcCallComplete == state->Event)
			{
				printf("\n Async call is Completed Successfully \n"); 		
				break;
			}
			else if(RPC_S_ASYNC_CALL_PENDING != Status)
			{
				throw 0x999;
			}
		}
		else if(RPC_S_ASYNC_CALL_PENDING != Status)
		{
			throw 0x999;
		}
		totalSleep -= 50;
		Sleep(50);
	}while(totalSleep > 0);
	return nReply;
}

vector<RPC_ASYNC_STATE*> m_asyncCol;

//FRS2AsyncPoll is AsyncPoll
DWORD FRS2AsyncPoll(handle_t bindngHandle,FRS_CONNECTION_ID connectionId, FRS_ASYNC_RESPONSE_CONTEXT *response)
{
	RPC_ASYNC_STATE* AsyncHandle = (RPC_ASYNC_STATE*)malloc(sizeof(RPC_ASYNC_STATE));
	m_asyncCol.push_back(AsyncHandle);
	RPC_STATUS  Status  = 0; 
	DWORD nReply = 100;

	Status = RpcAsyncInitializeHandle(AsyncHandle,sizeof(RPC_ASYNC_STATE));
	if(Status)
	{
		throw Status;
	}
	AsyncHandle->Flags = AsyncHandle->Flags | RPC_C_NOTIFY_ON_SEND_COMPLETE;
	AsyncHandle->NotificationType = RpcNotificationTypeEvent;
	AsyncHandle->u.hEvent = CreateEvent(NULL, FALSE, FALSE, NULL);
	if(AsyncHandle->u.hEvent == 0)
	{
		throw 0xFFFF0001;
	}	
	__try
	{
		AsyncPoll(AsyncHandle,bindngHandle,connectionId, response);

		nReply = WaitAsyncResult(AsyncHandle);		
	}
	__except(EXCEPTION_EXECUTE_HANDLER)
	{
		DWORD Error = GetExceptionCode();
		printf("Exception in Create tunnel Call is %x \n", Error);
		return Error;
	}
	return nReply;	



}
//FRSRequestVersionvector is  RequestVersion vector
DWORD FRSRequestVersionVector(handle_t bindingHandle,
							  unsigned int sequnceNumber, 
							  FRS_CONNECTION_ID connectionId,
							  FRS_CONTENT_SET_ID contentSetId,
							  VERSION_REQUEST_TYPE requestType,
							  VERSION_CHANGE_TYPE changeType,
							  ULONGLONG vvGeneration)
{

	RPC_ASYNC_STATE AsyncHandle;
	RPC_STATUS  Status  = 0; 
	DWORD nReply = 1;

	Status = RpcAsyncInitializeHandle(&AsyncHandle,sizeof(RPC_ASYNC_STATE));
	if(Status)
	{
		printf("AsyncHandle failed \n");
		throw Status;
	}
	AsyncHandle.Flags = AsyncHandle.Flags | RPC_C_NOTIFY_ON_SEND_COMPLETE;
	AsyncHandle.NotificationType = RpcNotificationTypeEvent;
	AsyncHandle.u.hEvent = CreateEvent(NULL, FALSE, FALSE, NULL);

	if(AsyncHandle.u.hEvent == 0)
	{
		throw 0xFFFF0001;
	}	
	__try
	{

		RequestVersionVector(&AsyncHandle,
			bindingHandle,
			sequnceNumber,
			connectionId,
			contentSetId,
			requestType,
			changeType,
			vvGeneration);

		nReply = WaitAsyncResult(&AsyncHandle);			
	}
	__except(EXCEPTION_EXECUTE_HANDLER)
	{
		DWORD Error = GetExceptionCode();
		printf("Exception in Create tunnel Call is %x \n", Error);
		return Error;
	}
	return nReply;	


}

//FRS2RequestUpdates is RequestUpdates
DWORD FRS2RequestUpdates(handle_t bindingHandle,
						 FRS_CONNECTION_ID connectionId,
						 FRS_CONTENT_SET_ID ContentSetId,
						 DWORD creditsAvailable,
						 long hashRequested,
						 UPDATE_REQUEST_TYPE requestType,
						 unsigned long versionVectorDiffCount,
						 FRS_VERSION_VECTOR *versionVector,
						 FRS_UPDATE *frsUpdate,
						 DWORD *updateCount,
						 UPDATE_STATUS *updateStatus,
						 GUID *gvsnDBGuid,
						 DWORDLONG *gvsnVersion
						 )
{

	RPC_ASYNC_STATE AsyncHandle;
	RPC_STATUS  Status  = 0; 
	DWORD nReply = 1;

	Status = RpcAsyncInitializeHandle(&AsyncHandle,sizeof(RPC_ASYNC_STATE));
	if(Status)
	{
		printf("AsyncHandle failed \n");
		throw Status;
	}
	AsyncHandle.Flags = AsyncHandle.Flags | RPC_C_NOTIFY_ON_SEND_COMPLETE;
	AsyncHandle.NotificationType = RpcNotificationTypeEvent;
	AsyncHandle.u.hEvent = CreateEvent(NULL, FALSE, FALSE, NULL);

	if(AsyncHandle.u.hEvent == 0)
	{
		throw 0xFFFF0001;
	}	
	__try
	{
		RequestUpdates(&AsyncHandle,
			bindingHandle,
			connectionId,
			ContentSetId,
			creditsAvailable,
			hashRequested,
			requestType,
			versionVectorDiffCount,
			versionVector,
			frsUpdate,
			updateCount,
			updateStatus,
			gvsnDBGuid,
			gvsnVersion);

		nReply = WaitAsyncResult(&AsyncHandle);	
	}
	__except(EXCEPTION_EXECUTE_HANDLER)
	{
		DWORD Error = GetExceptionCode();
		printf("Exception in Create tunnel Call is %x \n", Error);
		return Error;
	}
	return nReply;	


}


//FRS2RequestRecords is RequestRecords
DWORD FRS2RequestRecords(  handle_t bindingHandle,
						 FRS_CONNECTION_ID	connectionId,
						 FRS_CONTENT_SET_ID	contentSetId,
						 FRS_DATABASE_ID uidDbGuid,
						 DWORDLONG	uidVersion,
						 DWORD	*maxRecords,
						 DWORD	*numRecords,
						 DWORD	*numBytes,
						 byte	**compressedRecords,
						 RECORDS_STATUS	*recordsStatus)
{

	RPC_STATUS RequestRecordsStatus;
	RPC_ASYNC_STATE RequestRecordsAsyncHandle;
	DWORD nReply = 1;


	RequestRecordsStatus = RpcAsyncInitializeHandle(&RequestRecordsAsyncHandle,sizeof(RPC_ASYNC_STATE));
	if(RequestRecordsStatus)
	{
		printf("AsyncHandle failed \n");
		throw RequestRecordsStatus;
	}
	RequestRecordsAsyncHandle.Flags = RequestRecordsAsyncHandle.Flags | RPC_C_NOTIFY_ON_SEND_COMPLETE;
	RequestRecordsAsyncHandle.NotificationType = RpcNotificationTypeEvent;
	RequestRecordsAsyncHandle.u.hEvent = CreateEvent(NULL, FALSE, FALSE, NULL);

	if(RequestRecordsAsyncHandle.u.hEvent == 0)
	{
		printf("Error in initializing Async Event \n");
		throw 0xFFFF0001;
	}	


	RequestRecords(&RequestRecordsAsyncHandle,
		bindingHandle,
		connectionId,
		contentSetId,
		uidDbGuid,
		uidVersion,
		maxRecords,
		numRecords,
		numBytes,
		compressedRecords,
		recordsStatus);


	nReply = WaitAsyncResult(&RequestRecordsAsyncHandle);	
	/*do
	{
	RequestRecordsStatus=RpcAsyncGetCallStatus(&RequestRecordsAsyncHandle);
	if(RequestRecordsStatus==RPC_S_OK)
	{
	RequestRecordsStatus=RpcAsyncCompleteCall(&RequestRecordsAsyncHandle,&nReply);
	if(RequestRecordsStatus==RPC_S_OK)
	{
	printf("Request Records completed successfully!");

	break;
	}

	}

	if(RequestRecordsStatus==9026)
	{
	return RequestRecordsStatus;
	}

	}while(true);*/
	return nReply;	


}

//FRS2InitializeFileTransferAsync is InitializeFileTransferAsync
DWORD FRS2InitializeFileTransferAsync(handle_t bindingHandle,
									  FRS_CONNECTION_ID connectionId,
									  FRS_UPDATE *frsUpdate,
									  long rdcDesired,
									  FRS_REQUESTED_STAGING_POLICY *stagingPolicy,
									  PFRS_SERVER_CONTEXT *serverContext,
									  FRS_RDC_FILEINFO *rdcFileInfo,
									  byte *dataBuffer,
									  DWORD bufferSize,
									  DWORD *sizeRead,
									  long *isEndOfFile)
{

	RPC_ASYNC_STATE AsyncHandle;
	RPC_STATUS  Status  = 0; 
	DWORD nReply = 1;

	Status = RpcAsyncInitializeHandle(&AsyncHandle,sizeof(RPC_ASYNC_STATE));
	if(Status)
	{
		printf("AsyncHandle failed \n");
		throw Status;
	}
	AsyncHandle.Flags = AsyncHandle.Flags | RPC_C_NOTIFY_ON_SEND_COMPLETE;
	AsyncHandle.NotificationType = RpcNotificationTypeEvent;
	AsyncHandle.u.hEvent = CreateEvent(NULL, FALSE, FALSE, NULL);

	if(AsyncHandle.u.hEvent == 0)
	{
		printf("Error in initializing Async Event \n");
		throw 0xFFFF0001;
	}	
	__try
	{

		InitializeFileTransferAsync(&AsyncHandle,
			bindingHandle,
			connectionId,
			frsUpdate,
			rdcDesired,
			stagingPolicy,
			serverContext,
			&rdcFileInfo,
			dataBuffer,
			bufferSize,
			sizeRead,
			isEndOfFile);


		nReply = WaitAsyncResult(&AsyncHandle);		
	}
	__except(EXCEPTION_EXECUTE_HANDLER)
	{
		DWORD Error = GetExceptionCode();
		printf("Exception in Create tunnel Call is %x \n", Error);
		return Error;
	}
	return nReply;	


}


DWORD FRS2UpdateCancel(handle_t bindingHandle,
					   FRS_CONNECTION_ID connectionId,
					   FRS_UPDATE_CANCEL_DATA *cancelData
					   )

{
	DWORD retVal=UpdateCancel(bindingHandle,connectionId,*cancelData);
	return retVal;
}

static int count=0;
DWORD FRS2RawGetFileDataAsync(PFRS_SERVER_CONTEXT serverContext, byte *dataBuffer)
{
	RPC_ASYNC_STATE AsyncHandle;
	RPC_STATUS  Status  = 0; 
	RPC_STATUS  pipeStatus=0;
	DWORD nReply = 1;
	unsigned long  numElements = 0;
	byte *buff = (byte *)malloc(sizeof(byte)*1000);
	DWORD dwWaitStatus = false;

	Status = RpcAsyncInitializeHandle(&AsyncHandle,sizeof(RPC_ASYNC_STATE));
	if(Status)
	{
		printf("AsyncHandle failed \n");
		throw Status;
	}
	AsyncHandle.Flags = AsyncHandle.Flags | RPC_C_NOTIFY_ON_SEND_COMPLETE;
	AsyncHandle.NotificationType = RpcNotificationTypeEvent;
	AsyncHandle.u.hEvent = CreateEvent(NULL, FALSE, FALSE, NULL);

	if(AsyncHandle.u.hEvent == 0)
	{
		printf("Error in initializing Async Event \n");
		throw 0xFFFF0001;
	}	
	__try
	{
		ASYNC_BYTE_PIPE *bytePipe=(ASYNC_BYTE_PIPE*)malloc(sizeof(ASYNC_BYTE_PIPE));
		RawGetFileDataAsync(&AsyncHandle,serverContext,bytePipe);
		int count=0;
		bool fDone=false;
		while(!fDone)
		{
			Sleep(20);
			pipeStatus= bytePipe->pull(bytePipe->state, buff ,1000,&numElements);

			switch(pipeStatus)
			{
			case RPC_S_ASYNC_CALL_PENDING:

				dwWaitStatus = WaitForSingleObject(bytePipe,2000);

				switch( dwWaitStatus )
				{
				case WAIT_OBJECT_0:
					fDone = false;
					break;
				case WAIT_TIMEOUT:
					fDone = true;
					break;
				}

				break;
			case RPC_S_OK:
				if (numElements == 0)
				{
					fDone = true;
				}

				break;
			}
		}

		nReply = WaitAsyncResult(&AsyncHandle);		
	}
	__except(EXCEPTION_EXECUTE_HANDLER)
	{
		DWORD Error = GetExceptionCode();
		return Error;
	}
	return nReply;	
}




DWORD FRS2RdcGetFileDataAsync(PFRS_SERVER_CONTEXT serverContext, byte *dataBuffer )
{

	RPC_ASYNC_STATE AsyncHandle;
	RPC_STATUS  Status  = 0; 
	RPC_STATUS  pipeStatus=0;
	DWORD nReply = 1;
	unsigned long  numElements = 0;
	byte *buff = (byte *)malloc(sizeof(byte)*1000);
	DWORD dwWaitStatus = false;

	Status = RpcAsyncInitializeHandle(&AsyncHandle,sizeof(RPC_ASYNC_STATE));
	if(Status)
	{
		printf("AsyncHandle failed \n");
		throw Status;
	}
	AsyncHandle.Flags = AsyncHandle.Flags | RPC_C_NOTIFY_ON_SEND_COMPLETE;
	AsyncHandle.NotificationType = RpcNotificationTypeEvent;
	AsyncHandle.u.hEvent = CreateEvent(NULL, FALSE, FALSE, NULL);

	if(AsyncHandle.u.hEvent == 0)
	{
		printf("Error in initializing Async Event \n");
		throw 0xFFFF0001;
	}	
	__try
	{
		ASYNC_BYTE_PIPE *bytePipe=(ASYNC_BYTE_PIPE*)malloc(sizeof(ASYNC_BYTE_PIPE));
		RdcGetFileDataAsync(&AsyncHandle,serverContext,bytePipe);
		int count=0;
		bool fDone=false;
		while(!fDone)
		{
			Sleep(20);
			pipeStatus= bytePipe->pull(bytePipe->state, buff ,1000,&numElements);

			switch(pipeStatus)
			{
			case RPC_S_ASYNC_CALL_PENDING:

				dwWaitStatus = WaitForSingleObject(bytePipe,2000);

				switch( dwWaitStatus )
				{
				case WAIT_OBJECT_0:
					fDone = false;
					break;
				case WAIT_TIMEOUT:
					fDone = true;
					break;
				}

				break;
			case RPC_S_OK:
				if (numElements == 0)
				{
					fDone = true;

					break;
				}
			}
		}

		nReply = WaitAsyncResult(&AsyncHandle);		
	}
	__except(EXCEPTION_EXECUTE_HANDLER)
	{
		DWORD Error = GetExceptionCode();
		return Error;
	}
	return nReply;	


}


