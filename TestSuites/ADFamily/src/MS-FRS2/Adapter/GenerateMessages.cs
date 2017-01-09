// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestSuites.Frs2DataTypes;
using Microsoft.Protocols.TestTools.Messages.Marshaling;
namespace Microsoft.Protocols.TestSuites.MS_FRS2
{

    /// <summary>
    /// Represent the class containing the dllImports.
    /// </summary>
    class GenerateMessages
    {

        [DllImport("IMS_FRS2_RpcAdapter_RpcStubs.dll", EntryPoint = "CheckConnectivity", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint CheckConnectivity(IntPtr handle, Guid replicaId, Guid connectionId);


        [DllImport("IMS_FRS2_RpcAdapter_RpcStubs.dll", EntryPoint = "EstablishConnection", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint EstablishConnection(System.IntPtr bindingHandle,
                                                      System.Guid replicaSetId,
                                                      System.Guid connectionId,
                                                      uint downstreamProtocolVersion,
                                                      uint downstreamFlags,
                                                      out uint upstreamProtocolVersion,
                                                      out uint upStreamFlags);

        [DllImport("IMS_FRS2_RpcAdapter_RpcStubs.dll", EntryPoint = "EstablishSession", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint EstablishSession(System.IntPtr bindingHandle, System.Guid connectionId, System.Guid contentSetId);

        [DllImport("IMS_FRS2_RpcAdapter_RpcStubs.dll", EntryPoint = "FRS2RequestUpdates", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint FRS2RequestUpdates(System.IntPtr bindingHandle,
                                                     System.Guid connectionId,
                                                     System.Guid contentSetId,
                                                     uint creditsAvailable,
                                                     int hashRequested,
                                                     _UPDATE_REQUEST_TYPE updateRequestType,
                                                     uint versionVectorDiffcount,
                                                     _FRS_VERSION_VECTOR[] versionVector,
                                                      System.IntPtr frsUpdate,
                                                     ref uint updateCount,
                                                     ref _UPDATE_STATUS updateStatus,
                                                     ref IntPtr gvsnDbGuid,
                                                     ref System.UInt64 gvsnVersion);

        [DllImport("IMS_FRS2_RpcAdapter_RpcStubs.dll", EntryPoint = "FRSRequestVersionVector", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint FRSRequestVersionVector(System.IntPtr bindingHandle, uint sequenceNumber, System.Guid connectionId, System.Guid contentSetId, _VERSION_REQUEST_TYPE requestType, _VERSION_CHANGE_TYPE changeType, ulong vvGeneration);

        [DllImport("IMS_FRS2_RpcAdapter_RpcStubs.dll", EntryPoint = "FRS2AsyncPoll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint FRS2AsyncPoll(System.IntPtr bindingHandle, System.Guid connectionId, out _FRS_ASYNC_RESPONSE_CONTEXT_POINTER response);//out IntPtr response);// 

        [DllImport("IMS_FRS2_RpcAdapter_RpcStubs.dll", EntryPoint = "FRS2RequestRecords", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint FRS2RequestRecords(System.IntPtr bindingHandle,
                                                     System.Guid connectionId,
                                                     System.Guid contentsetId,
                                                     System.Guid uidDBGuid,
                                                     ulong uidVersion,
                                                     ref uint maxRecords,
                                                     out uint numRecords,
                                                     out uint numBytes,
                                                     ref IntPtr compressedRecords,
                                                     out _RECORDS_STATUS recordsstatus);


        [DllImport("IMS_FRS2_RpcAdapter_RpcStubs.dll", EntryPoint = "FRS2UpdateCancel", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint FRS2UpdateCancel(System.IntPtr bindingHandle,
                                                   System.Guid connectionId,
                                                   IntPtr cancelData);

        [DllImport("IMS_FRS2_RpcAdapter_RpcStubs.dll", EntryPoint = "RawGetFileData", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint RawGetFileData(
                                     ref System.IntPtr serverContext,
                                     byte[] dataBuffer,
                                     uint bufferSize,
                                     out System.UInt32 sizeRead,
                                     out System.Int32 isEndOfFile);


        [DllImport("IMS_FRS2_RpcAdapter_RpcStubs.dll", EntryPoint = "RdcGetSignatures", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint RdcGetSignatures(
                                                   System.IntPtr serverContext,
                                                   byte level,
                                                   ulong offset,
                                                   IntPtr bufferPtr,
                                                   uint length,
                                                   out uint sizeRead);
        [DllImport("IMS_FRS2_RpcAdapter_RpcStubs.dll", EntryPoint = "RdcPushSourceNeeds", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint RdcPushSourceNeeds(System.IntPtr serverContext,
                                                     [Size("needCount")] _FRS_RDC_SOURCE_NEED[] sourceNeeds,
                                                     uint needCount);
        [DllImport("IMS_FRS2_RpcAdapter_RpcStubs.dll", EntryPoint = "RdcGetFileData", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint RdcGetFileData(System.IntPtr serverContext,
                                                 IntPtr bufferPtr,
                                                 uint bufferSize,
                                                 out  uint sizeReturned);
        [DllImport("IMS_FRS2_RpcAdapter_RpcStubs.dll", EntryPoint = "RdcClose", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint RdcClose(ref System.IntPtr bindingHandle);
        [DllImport("IMS_FRS2_RpcAdapter_RpcStubs.dll", EntryPoint = "FRS2InitializeFileTransferAsync", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint
            FRS2InitializeFileTransferAsync(System.IntPtr bindingHandle,
                                            System.Guid connectionId,
                                            IntPtr Test,
                                            int rdcDesired,
                                             ref _FRS_REQUESTED_STAGING_POLICY stagingPolicy,
                                             out IntPtr serverContext,
                                            IntPtr rdcFileInfo,
                                             IntPtr dataBuffer,
                                            uint bufferSize,
                                            out System.UInt32 sizeRead,
                                            out int isEndOfFile);
        [DllImport("IMS_FRS2_RpcAdapter_RpcStubs.dll", EntryPoint = "InitializeFileDataTransfer", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint InitializeFileDataTransfer(System.IntPtr bindingHandle, System.Guid connectionId, ref _FRS_UPDATE frsUpdate, out System.IntPtr? serverContext, ulong offset, ulong length, [Inline()] [Length("*sizeRead")] [Size("bufferSize")] out byte[] dataBuffer, uint bufferSize, out System.UInt32? sizeRead, out System.Int32? isEndOfFile);
        [DllImport("IMS_FRS2_RpcAdapter_RpcStubs.dll", EntryPoint = "FRS2RawGetFileDataAsync", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint FRS2RawGetFileDataAsync(System.IntPtr serverContext, out IntPtr BufferPtr);
        [DllImport("IMS_FRS2_RpcAdapter_RpcStubs.dll", EntryPoint = "FRS2RdcGetFileDataAsync", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint FRS2RdcGetFileDataAsync(System.IntPtr serverContext, out IntPtr bytePipe);


    }
}
