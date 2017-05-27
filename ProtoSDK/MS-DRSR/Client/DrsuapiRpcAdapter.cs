// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr
{
    /// <summary>
    /// The implementation of IDrsrRpcAdapter
    /// </summary>   
    internal class DrsuapiRpcAdapter : IDrsuapiRpcAdapter, IDisposable
    {
        // RPCE client transport
        internal RpceClientTransport rpceClientTransport;

        // Timeout for RPC bind/call
        private TimeSpan rpceTimeout;

        // Return value for DRSR methods
        private uint retVal;


        /// <summary>
        /// Bind to DRSR RPC server.
        /// </summary>
        /// <param name="protocolSequence">
        /// RPC protocol sequence.
        /// </param>
        /// <param name="networkAddress">
        /// RPC network address.
        /// </param>
        /// <param name="endpoint">
        /// RPC endpoint.
        /// </param>
        /// <param name="securityContext">
        /// RPC security provider.
        /// </param>
        /// <param name="authenticationLevel">
        /// RPC authentication level.
        /// </param>
        /// <param name="timeout">
        /// Timeout
        /// </param>
        public void Bind(
            string protocolSequence,
            string networkAddress,
            string endpoint,
            ClientSecurityContext securityContext,
            RpceAuthenticationLevel authenticationLevel,
            TimeSpan timeout)
        {
            rpceTimeout = timeout;

            rpceClientTransport = new RpceClientTransport();

            rpceClientTransport.Bind(
                protocolSequence,
                networkAddress,
                endpoint,
                null,
                DrsrUtility.DRSUAPI_RPC_INTERFACE_UUID,
                DrsrUtility.DRSUAPI_RPC_INTERFACE_MAJOR_VERSION,
                DrsrUtility.DRSUAPI_RPC_INTERFACE_MINOR_VERSION,
                securityContext,
                authenticationLevel,
                false,
                rpceTimeout);
        }

        public bool Bind(string server, string domain, string userName, string password, string spn)
        {
            return false;
        }

        /// <summary>
        /// RPC unbind.
        /// </summary>
        public void Unbind()
        {
            if (rpceClientTransport != null)
            {
                rpceClientTransport.Unbind(rpceTimeout);
                rpceClientTransport = null;
            }
        }


        /// <summary>
        /// RPC handle.
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                return rpceClientTransport.Handle;
            }
        }


        #region Drsr drsuapi methods

        /// <summary>
        ///  The IDL_DRSBind method creates a context handle that
        ///  is necessary to call any other method in this interface.
        ///  Opnum: 0 
        /// </summary>
        /// <param name="rpc_handle">
        ///  An RPC binding handle, as specified in [C706].
        /// </param>
        /// <param name="puuidClientDsa">
        ///  A pointer to a GUID that identifies the caller.
        /// </param>
        /// <param name="pextClient">
        ///  A pointer to client capabilities, for use in version
        ///  negotiation.
        /// </param>
        /// <param name="ppextServer">
        ///  A pointer to a pointer to server capabilities, for use
        ///  in version negotiation.
        /// </param>
        /// <param name="phDrs">
        ///  A pointer to an RPC context handle (as specified in
        ///  [C706]), which may be used in calls to other methods
        ///  in this interface.
        /// </param>
        public uint IDL_DRSBind(
             IntPtr rpc_handle,
             Guid? puuidClientDsa,
             DRS_EXTENSIONS? pextClient,
             out DRS_EXTENSIONS_INT? ppextServer,
             out IntPtr? phDrs)
        {
            const ushort opnum = 0;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrUuidClientDsa = TypeMarshal.ToIntPtr(puuidClientDsa.Value);
            SafeIntPtr ptrExtClient = TypeMarshal.ToIntPtr(pextClient.Value);

            paramList = new Int3264[] 
			{
				ptrUuidClientDsa,
				ptrExtClient,
				IntPtr.Zero,
				IntPtr.Zero,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                IntPtr ptrExtServer = Marshal.ReadIntPtr(outParamList[2]);
                ppextServer = TypeMarshal.ToNullableStruct<DRS_EXTENSIONS_INT>(ptrExtServer);
                phDrs = TypeMarshal.ToNullableStruct<IntPtr>(outParamList[3]);
                retVal = outParamList[4].ToUInt32();
            }

            ptrUuidClientDsa.Dispose();
            ptrExtClient.Dispose();

            return retVal;
        }


        /// <summary>
        ///  The IDL_DRSUnbind method destroys a context handle previously
        ///  created by the IDL_DRSBind method. Opnum: 1 
        /// </summary>
        /// <param name="phDrs">
        ///  A pointer to the RPC context handle returned by the
        ///  IDL_DRSBind method. The value is set to null on return.
        /// </param>
        public uint IDL_DRSUnbind(ref IntPtr? phDrs)
        {
            const ushort opnum = 1;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrhDrs = TypeMarshal.ToIntPtr(phDrs);

            paramList = new Int3264[] 
			{
				ptrhDrs,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                phDrs = TypeMarshal.ToNullableStruct<IntPtr>(outParamList[0]);
                retVal = outParamList[1].ToUInt32();
            }

            ptrhDrs.Dispose();

            return retVal;
        }


        /// <summary>
        ///  The IDL_DRSReplicaSync method triggers replication from
        ///  another DC. This method is used only to diagnose,
        ///  monitor, and manage the replication implementation.
        ///  The structures requested and returned through this
        ///  method MAY have meaning to peer DCs and applications
        ///  but are not required for interoperation with Windows
        ///  clients. Opnum: 2 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgSync">
        ///  A pointer to the request message.
        /// </param>
        public uint IDL_DRSReplicaSync(
          IntPtr hDrs,
          uint dwVersion,
          DRS_MSG_REPSYNC? pmsgSync)
        {
            const ushort opnum = 2;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgSync = TypeMarshal.ToIntPtr(pmsgSync, dwVersion, null, null);

            paramList = new Int3264[] 
			{
				hDrs,
				dwVersion,
				ptrMsgSync,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                retVal = outParamList[3].ToUInt32();
            }

            ptrMsgSync.Dispose();

            return retVal;
        }


        /// <summary>
        ///  The IDL_DRSUpdateRefs method adds or deletes a value
        ///  from the repsTo of a specified NC replica. This method
        ///  is used only to diagnose, monitor, and manage the replication
        ///  implementation. The structures requested and returned
        ///  through this method MAY have meaning to peer DCs and
        ///  applications but are not required for interoperation
        ///  with Windows clients. Opnum: 4 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgUpdRefs">
        ///  A pointer to the request message.
        /// </param>
        public uint IDL_DRSUpdateRefs(
            IntPtr hDrs,
            uint dwVersion,
            DRS_MSG_UPDREFS? pmsgUpdRefs)
        {
            const ushort opnum = 4;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgUpdRefs = TypeMarshal.ToIntPtr(pmsgUpdRefs, dwVersion, null, null);

            paramList = new Int3264[] 
			{
				hDrs,
				dwVersion,
				ptrMsgUpdRefs,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                retVal = outParamList[3].ToUInt32();
            }

            ptrMsgUpdRefs.Dispose();

            return retVal;
        }


        /// <summary>
        ///  The IDL_DRSReplicaAdd method adds a replication source
        ///  reference for the specified NC. This method is used
        ///  only to diagnose, monitor, and manage the replication
        ///  implementation. The structures requested and returned
        ///  through this method MAY have meaning to peer DCs and
        ///  applications but are not required for interoperation
        ///  with Windows clients. Opnum: 5 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgAdd">
        ///  A pointer to the request message.
        /// </param>
        public uint IDL_DRSReplicaAdd(
         IntPtr hDrs,
         uint dwVersion,
         DRS_MSG_REPADD? pmsgAdd)
        {
            const ushort opnum = 5;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgAdd = TypeMarshal.ToIntPtr(pmsgAdd, dwVersion, null, null);

            paramList = new Int3264[] 
			{
				hDrs,
				dwVersion,
				ptrMsgAdd,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                retVal = outParamList[3].ToUInt32();
            }

            ptrMsgAdd.Dispose();

            return retVal;
        }


        /// <summary>
        ///  The IDL_DRSReplicaDel method deletes a replication source
        ///  reference for the specified NC. This method is used
        ///  only to diagnose, monitor, and manage the replication
        ///  implementation. The structures requested and returned
        ///  through this method MAY have meaning to peer DCs and
        ///  applications but are not required for interoperation
        ///  with Windows clients. Opnum: 6 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by IDL_DRSBind.
        /// </param>
        /// <param name="dwVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgDel">
        ///  A pointer to the request message.
        /// </param>
        public uint IDL_DRSReplicaDel(
            IntPtr hDrs,
            uint dwVersion,
            DRS_MSG_REPDEL? pmsgDel)
        {
            const ushort opnum = 6;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgDel = TypeMarshal.ToIntPtr(pmsgDel, dwVersion, null, null);

            paramList = new Int3264[] 
			{
				hDrs,
				dwVersion,
				ptrMsgDel,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                retVal = outParamList[3].ToUInt32();
            }

            ptrMsgDel.Dispose();

            return retVal;
        }


        /// <summary>
        ///  The IDL_DRSReplicaModify method updates the value for
        ///  repsFrom for the NC replica. This method is used only
        ///  to diagnose, monitor, and manage the replication implementation.
        ///  The structures requested and returned through this
        ///  method MAY have meaning to peer DCs and applications
        ///  but are not required for interoperation with Windows
        ///  clients. Opnum: 7 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by IDL_DRSBind.
        /// </param>
        /// <param name="dwVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgMod">
        ///  A pointer to the request message.
        /// </param>
        public uint IDL_DRSReplicaModify(
            IntPtr hDrs,
            uint dwVersion,
            DRS_MSG_REPMOD? pmsgMod)
        {
            const ushort opnum = 7;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgMod = TypeMarshal.ToIntPtr(pmsgMod, dwVersion, null, null);

            paramList = new Int3264[] 
			{
				hDrs,
				dwVersion,
				ptrMsgMod,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                retVal = outParamList[3].ToUInt32();
            }

            ptrMsgMod.Dispose();

            return retVal;
        }


        /// <summary>
        ///  The IDL_DRSCrackNames method looks up each of a set
        ///  of objects in the directory and returns it to the caller
        ///  in the requested format. Opnum: 12 
        /// </summary>
        /// <param name="hDrs">
        ///  RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  Version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  Pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  Pointer to the version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  Pointer to the response message.
        /// </param>
        public uint IDL_DRSCrackNames(
            IntPtr hDrs,
            uint dwInVersion,
            DRS_MSG_CRACKREQ? pmsgIn,
            out uint? pdwOutVersion,
            out DRS_MSG_CRACKREPLY? pmsgOut)
        {
            const ushort opnum = 12;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgIn = TypeMarshal.ToIntPtr(pmsgIn, dwInVersion, null, null);

            paramList = new Int3264[] 
			{
				hDrs,
				dwInVersion,
				ptrMsgIn,
				IntPtr.Zero,
				IntPtr.Zero,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                pdwOutVersion = TypeMarshal.ToNullableStruct<uint>(outParamList[3]);
                pmsgOut = TypeMarshal.ToNullableStruct<DRS_MSG_CRACKREPLY>(
                    outParamList[4], pdwOutVersion, null, null);
                retVal = outParamList[5].ToUInt32();
            }

            ptrMsgIn.Dispose();

            return retVal;
        }


        /// <summary>
        ///  The IDL_DRSWriteSPN method updates the set of SPNs on
        ///  an object. Opnum: 13 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message. MUST be set to 1,
        ///  because that is the only version supported.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message. The
        ///  value is always 1 because that is the only version
        ///  supported.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        public uint IDL_DRSWriteSPN(
            IntPtr hDrs,
            dwInVersion_Values dwInVersion,
            DRS_MSG_SPNREQ? pmsgIn,
            out pdwOutVersion_Values? pdwOutVersion,
            out DRS_MSG_SPNREPLY? pmsgOut)
        {
            const ushort opnum = 13;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgIn = TypeMarshal.ToIntPtr(pmsgIn, dwInVersion, null, null);

            paramList = new Int3264[] 
			{
				hDrs,
				(uint)dwInVersion,
				ptrMsgIn,
				IntPtr.Zero,
				IntPtr.Zero,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                pdwOutVersion = TypeMarshal.ToNullableStruct<pdwOutVersion_Values>(outParamList[3]);
                pmsgOut = TypeMarshal.ToNullableStruct<DRS_MSG_SPNREPLY>(
                    outParamList[4], pdwOutVersion, null, null);
                retVal = outParamList[5].ToUInt32();
            }

            ptrMsgIn.Dispose();

            return retVal;
        }


        /// <summary>
        ///  The IDL_DRSRemoveDsServer method removes the representation
        ///  (also known as metadata) of a DC from the directory.
        ///  Opnum: 14 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message. MUST be set to 1
        ///  because that is the only version supported.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message. The
        ///  value is always 1 because that is the only version
        ///  supported.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        public uint IDL_DRSRemoveDsServer(
            IntPtr hDrs,
            IDL_DRSRemoveDsServer_dwInVersion_Values dwInVersion,
            DRS_MSG_RMSVRREQ? pmsgIn,
            out IDL_DRSRemoveDsServer_pdwOutVersion_Values? pdwOutVersion,
            out DRS_MSG_RMSVRREPLY? pmsgOut)
        {
            const ushort opnum = 14;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgIn = TypeMarshal.ToIntPtr(pmsgIn, dwInVersion, null, null);

            paramList = new Int3264[] 
			{
				hDrs,
				(uint)dwInVersion,
				ptrMsgIn,
				IntPtr.Zero,
				IntPtr.Zero,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                pdwOutVersion = TypeMarshal.ToNullableStruct<IDL_DRSRemoveDsServer_pdwOutVersion_Values>(outParamList[3]);
                pmsgOut = TypeMarshal.ToNullableStruct<DRS_MSG_RMSVRREPLY>(
                    outParamList[4], pdwOutVersion, null, null);
                retVal = outParamList[5].ToUInt32();
            }

            ptrMsgIn.Dispose();

            return retVal;
        }


        /// <summary>
        ///  The IDL_DRSRemoveDsDomain method removes the representation
        ///  (also known as metadata) of a domain from the directory.
        ///  Opnum: 15 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message. This MUST be set
        ///  to 1, because this is the only version supported.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message. The
        ///  value is always 1 because that is the only version
        ///  supported.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        public uint IDL_DRSRemoveDsDomain(
            IntPtr hDrs,
            IDL_DRSRemoveDsDomain_dwInVersion_Values dwInVersion,
            DRS_MSG_RMDMNREQ? pmsgIn,
            out IDL_DRSRemoveDsDomain_pdwOutVersion_Values? pdwOutVersion,
            out DRS_MSG_RMDMNREPLY? pmsgOut)
        {
            const ushort opnum = 15;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgIn = TypeMarshal.ToIntPtr(pmsgIn, dwInVersion, null, null);

            paramList = new Int3264[] 
			{
				hDrs,
				(uint)dwInVersion,
				ptrMsgIn,
				IntPtr.Zero,
				IntPtr.Zero,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                pdwOutVersion = TypeMarshal.ToNullableStruct<IDL_DRSRemoveDsDomain_pdwOutVersion_Values>(outParamList[3]);
                pmsgOut = TypeMarshal.ToNullableStruct<DRS_MSG_RMDMNREPLY>(
                    outParamList[4], pdwOutVersion, null, null);
                retVal = outParamList[5].ToUInt32();
            }

            ptrMsgIn.Dispose();

            return retVal;
        }


        /// <summary>
        ///  The IDL_DRSDomainControllerInfo method retrieves information
        ///  about DCs in a given domain. Opnum: 16 
        /// </summary>
        /// <param name="hDrs">
        ///  RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  Version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  Pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  Pointer to the version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  Pointer to the response message.
        /// </param>        
        public uint IDL_DRSDomainControllerInfo(
            IntPtr hDrs,
            uint dwInVersion,
            DRS_MSG_DCINFOREQ? pmsgIn,
            out uint? pdwOutVersion,
             out DRS_MSG_DCINFOREPLY? pmsgOut)
        {
            const ushort opnum = 16;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgIn = TypeMarshal.ToIntPtr(pmsgIn, dwInVersion, null, null);

            paramList = new Int3264[] 
			{
				hDrs,
				dwInVersion,
				ptrMsgIn,
				IntPtr.Zero,
				IntPtr.Zero,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                pdwOutVersion = TypeMarshal.ToNullableStruct<uint>(outParamList[3]);
                pmsgOut = TypeMarshal.ToNullableStruct<DRS_MSG_DCINFOREPLY>(
                    outParamList[4], pdwOutVersion, null, null);
                retVal = outParamList[5].ToUInt32();
            }

            ptrMsgIn.Dispose();

            return retVal;
        }


        /// <summary>
        ///  The IDL_DRSExecuteKCC method validates the replication
        ///  interconnections of DCs and updates them if necessary.
        ///   This method is used only to diagnose, monitor, and
        ///  manage the replication topology implementation. The
        ///  structures requested and returned through this method
        ///  MAY have meaning to peer DCs and applications but are
        ///  not required for interoperation with Windows clients.
        ///  Opnum: 18 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        public uint IDL_DRSExecuteKCC(
            IntPtr hDrs,
            uint dwInVersion,
            DRS_MSG_KCC_EXECUTE? pmsgIn)
        {
            const ushort opnum = 18;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgIn = TypeMarshal.ToIntPtr(pmsgIn, dwInVersion, null, null);

            paramList = new Int3264[] 
			{
				hDrs,
				dwInVersion,
				ptrMsgIn,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                retVal = outParamList[3].ToUInt32();
            }

            ptrMsgIn.Dispose();

            return retVal;
        }


        /// <summary>
        ///  The IDL_DRSGetReplInfo method retrieves the replication
        ///  state of the server. This method is used only to diagnose,
        ///  monitor, and manage the replication implementation.
        ///  The structures requested and returned through this
        ///  method MAY have meaning to peer DCs and applications
        ///  but are not required for interoperation with Windows
        ///  clients. Opnum: 19 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        public uint IDL_DRSGetReplInfo(
            IntPtr hDrs,
            uint dwInVersion,
            DRS_MSG_GETREPLINFO_REQ? pmsgIn,
            out uint? pdwOutVersion,
            out DRS_MSG_GETREPLINFO_REPLY? pmsgOut)
        {
            const ushort opnum = 19;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgIn = TypeMarshal.ToIntPtr(pmsgIn, dwInVersion, null, null);

            paramList = new Int3264[] 
			{
				hDrs,
				dwInVersion,
				ptrMsgIn,
				IntPtr.Zero,
				IntPtr.Zero,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                pdwOutVersion = TypeMarshal.ToNullableStruct<uint>(outParamList[3]);
                pmsgOut = TypeMarshal.ToNullableStruct<DRS_MSG_GETREPLINFO_REPLY>(
                    outParamList[4], pdwOutVersion, null, null);
                retVal = outParamList[5].ToUInt32();
            }

            ptrMsgIn.Dispose();

            return retVal;
        }


        /// <summary>
        ///  The IDL_DRSAddSidHistory method adds one or more SIDs
        ///  to the sIDHistoryattribute of a given object. Opnum
        ///  : 20 
        /// </summary>
        /// <param name="hDrs">
        ///  RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  Version of the request message. Must be set to 1, because
        ///  no other version is supported.
        /// </param>
        /// <param name="pmsgIn">
        ///  Pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  Pointer to the version of the response message. The
        ///  value will always be 1, because no other version is
        ///  supported.
        /// </param>
        /// <param name="pmsgOut">
        ///  Pointer to the response message.
        /// </param>
        public uint IDL_DRSAddSidHistory(
            IntPtr hDrs,
            IDL_DRSAddSidHistory_dwInVersion_Values dwInVersion,
            DRS_MSG_ADDSIDREQ? pmsgIn,
            out IDL_DRSAddSidHistory_pdwOutVersion_Values? pdwOutVersion,
            out DRS_MSG_ADDSIDREPLY? pmsgOut)
        {
            const ushort opnum = 20;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgIn = TypeMarshal.ToIntPtr(pmsgIn, dwInVersion, null, null);

            paramList = new Int3264[] 
			{
				hDrs,
				(uint)dwInVersion,
				ptrMsgIn,
				IntPtr.Zero,
				IntPtr.Zero,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                pdwOutVersion = TypeMarshal.ToNullableStruct<IDL_DRSAddSidHistory_pdwOutVersion_Values>(outParamList[3]);
                pmsgOut = TypeMarshal.ToNullableStruct<DRS_MSG_ADDSIDREPLY>(
                    outParamList[4], pdwOutVersion, null, null);
                retVal = outParamList[5].ToUInt32();
            }

            ptrMsgIn.Dispose();

            return retVal;
        }


        /// <summary>
        ///  The IDL_DRSReplicaVerifyObjects method verifies the
        ///  existence of objects in an NC replica by comparing
        ///  against a replica of the same NC on a reference DC,
        ///  optionally deleting any objects that do not exist on
        ///  the reference DC. This method is used only to diagnose,
        ///  monitor, and manage the replication implementation.
        ///  The structures requested and returned through this
        ///  method MAY have meaning to peer DCs and applications
        ///  but are not required for interoperation with Windows
        ///  clients. Opnum: 22 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgVerify">
        ///  A pointer to the request message.
        /// </param>
        public uint IDL_DRSReplicaVerifyObjects(
            IntPtr hDrs,
            uint dwVersion,
            DRS_MSG_REPVERIFYOBJ? pmsgVerify)
        {
            const ushort opnum = 22;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgVerify = TypeMarshal.ToIntPtr(pmsgVerify, dwVersion, null, null);

            paramList = new Int3264[] 
			{
				hDrs,
				dwVersion,
				ptrMsgVerify,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                retVal = outParamList[3].ToUInt32();
            }

            ptrMsgVerify.Dispose();

            return retVal;
        }


        /// <summary>
        ///  The IDL_DRSQuerySitesByCost method determines the communication
        ///  cost from a "from" site to one or more "to" sites.
        ///  Opnum: 24 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        public uint IDL_DRSQuerySitesByCost(
            IntPtr hDrs,
            uint dwInVersion,
            DRS_MSG_QUERYSITESREQ? pmsgIn,
            out uint? pdwOutVersion,
            out DRS_MSG_QUERYSITESREPLY? pmsgOut)
        {
            const ushort opnum = 24;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgIn = TypeMarshal.ToIntPtr(pmsgIn, dwInVersion, null, null);

            paramList = new Int3264[] 
			{
				hDrs,
				dwInVersion,
				ptrMsgIn,
				IntPtr.Zero,
				IntPtr.Zero,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                pdwOutVersion = TypeMarshal.ToNullableStruct<uint>(outParamList[3]);
                pmsgOut = TypeMarshal.ToNullableStruct<DRS_MSG_QUERYSITESREPLY>(
                    outParamList[4], pdwOutVersion, null, null);
                retVal = outParamList[5].ToUInt32();
            }

            ptrMsgIn.Dispose();

            return retVal;
        }


        /// <summary>
        ///  The IDL_DRSInitDemotion method performs the first phase
        ///  of the removal of a DC from an AD LDSforest. This method
        ///  is supported only by AD LDS. This method is used only
        ///  to diagnose, monitor, and manage the implementation
        ///  of server-to-server DC demotion. The structures requested
        ///  and returned through this method MAY have meaning to
        ///  peer DCs and applications but are not required for
        ///  interoperation with Windows clients. Opnum: 25 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        public uint IDL_DRSInitDemotion(
            IntPtr hDrs,
            uint dwInVersion,
            DRS_MSG_INIT_DEMOTIONREQ? pmsgIn,
            out uint? pdwOutVersion,
            out DRS_MSG_INIT_DEMOTIONREPLY? pmsgOut)
        {
            const ushort opnum = 25;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgIn = TypeMarshal.ToIntPtr(pmsgIn, dwInVersion, null, null);

            paramList = new Int3264[] 
			{
				hDrs,
				dwInVersion,
				ptrMsgIn,
				IntPtr.Zero,
				IntPtr.Zero,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                pdwOutVersion = TypeMarshal.ToNullableStruct<uint>(outParamList[3]);
                pmsgOut = TypeMarshal.ToNullableStruct<DRS_MSG_INIT_DEMOTIONREPLY>(
                    outParamList[4], pdwOutVersion, null, null);
                retVal = outParamList[5].ToUInt32();
            }

            ptrMsgIn.Dispose();

            return retVal;
        }


        /// <summary>
        ///  The IDL_DRSReplicaDemotion method  replicates initiates
        ///  server-to-server replication to replicate  off all
        ///  changes to the specified NC and moves any FSMOs held
        ///  to another server. This method is supported only by
        ///  AD LDS. This method is used only to diagnose, monitor,
        ///  and manage the replication and FSMO implementation
        ///  related to DC demotion. The structures requested and
        ///  returned through this method MAY have meaning to peer
        ///  DCs and applications but are not required for interoperation
        ///  with Windows clients. Opnum: 26 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        public uint IDL_DRSReplicaDemotion(
            IntPtr hDrs,
            uint dwInVersion,
            DRS_MSG_REPLICA_DEMOTIONREQ? pmsgIn,
            out uint? pdwOutVersion,
            out DRS_MSG_REPLICA_DEMOTIONREPLY? pmsgOut)
        {
            const ushort opnum = 26;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgIn = TypeMarshal.ToIntPtr(pmsgIn, dwInVersion, null, null);

            paramList = new Int3264[] 
			{
				hDrs,
				dwInVersion,
				ptrMsgIn,
				IntPtr.Zero,
				IntPtr.Zero,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                pdwOutVersion = TypeMarshal.ToNullableStruct<uint>(outParamList[3]);
                pmsgOut = TypeMarshal.ToNullableStruct<DRS_MSG_REPLICA_DEMOTIONREPLY>(
                    outParamList[4], pdwOutVersion, null, null);
                retVal = outParamList[5].ToUInt32();
            }

            ptrMsgIn.Dispose();

            return retVal;
        }


        /// <summary>
        ///  The IDL_DRSFinishDemotion method either performs one
        ///  or more steps toward the complete removal of a DC from
        ///  an AD LDSforest, or it undoes the effects of the first
        ///  phase of removal (performed by IDL_DRSInitDemotion).
        ///  This method is supported by AD LDS only. This method
        ///  is used only to diagnose, monitor, and manage the implementation
        ///  of server-to-server DC demotion. The structures requested
        ///  and returned through this method MAY have meaning to
        ///  peer DCs and applications but are not required for
        ///  interoperation with Windows clients. Opnum: 27 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        public uint IDL_DRSFinishDemotion(
            IntPtr hDrs,
            uint dwInVersion,
            DRS_MSG_FINISH_DEMOTIONREQ? pmsgIn,
            out uint? pdwOutVersion,
            out DRS_MSG_FINISH_DEMOTIONREPLY? pmsgOut)
        {
            const ushort opnum = 27;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgIn = TypeMarshal.ToIntPtr(pmsgIn, dwInVersion, null, null);

            paramList = new Int3264[] 
			{
				hDrs,
				dwInVersion,
				ptrMsgIn,
				IntPtr.Zero,
				IntPtr.Zero,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                pdwOutVersion = TypeMarshal.ToNullableStruct<uint>(outParamList[3]);
                pmsgOut = TypeMarshal.ToNullableStruct<DRS_MSG_FINISH_DEMOTIONREPLY>(
                    outParamList[4], pdwOutVersion, null, null);
                retVal = outParamList[5].ToUInt32();
            }

            ptrMsgIn.Dispose();

            return retVal;
        }


        /// <summary>
        ///  The IDL_DRSGetNCChanges method replicates updates from an NC replica on the server. 
        ///  Opnum: 3 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        public uint IDL_DRSGetNCChanges(
            IntPtr hDrs,
            uint dwInVersion,
            DRS_MSG_GETCHGREQ? pmsgIn,
            out uint? pdwOutVersion,
            out DRS_MSG_GETCHGREPLY? pmsgOut)
        {
            const ushort opnum = 3;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgIn = TypeMarshal.ToIntPtr(pmsgIn, dwInVersion, null, null);

            paramList = new Int3264[] 
			{
				hDrs,
				dwInVersion,
				ptrMsgIn,
				IntPtr.Zero,
				IntPtr.Zero,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                pdwOutVersion = TypeMarshal.ToNullableStruct<uint>(outParamList[3]);
                pmsgOut = TypeMarshal.ToNullableStruct<DRS_MSG_GETCHGREPLY>(
                    outParamList[4], pdwOutVersion, null, null);
                retVal = outParamList[5].ToUInt32();
            }

            ptrMsgIn.Dispose();

            return retVal;
        }


        /// <summary>
        ///  The IDL_DRSVerifyNames method resolves a sequence of object identities. 
        ///  Opnum: 8 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        public uint IDL_DRSVerifyNames(
            IntPtr hDrs,
            uint dwInVersion,
            DRS_MSG_VERIFYREQ? pmsgIn,
            out uint? pdwOutVersion,
            out DRS_MSG_VERIFYREPLY? pmsgOut)
        {
            const ushort opnum = 8;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgIn = TypeMarshal.ToIntPtr(pmsgIn, dwInVersion, null, null);

            paramList = new Int3264[] 
			{
				hDrs,
				dwInVersion,
				ptrMsgIn,
				IntPtr.Zero,
				IntPtr.Zero,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                pdwOutVersion = TypeMarshal.ToNullableStruct<uint>(outParamList[3]);
                pmsgOut = TypeMarshal.ToNullableStruct<DRS_MSG_VERIFYREPLY>(
                    outParamList[4], pdwOutVersion, null, null);
                retVal = outParamList[5].ToUInt32();
            }

            ptrMsgIn.Dispose();

            return retVal;
        }


        /// <summary>
        ///  The IDL_DRSGetMemberships method retrieves group membership for an object. 
        ///  Opnum: 9 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        public uint IDL_DRSGetMemberships(
            IntPtr hDrs,
            uint dwInVersion,
            DRS_MSG_REVMEMB_REQ? pmsgIn,
            out uint? pdwOutVersion,
            out DRS_MSG_REVMEMB_REPLY? pmsgOut)
        {
            const ushort opnum = 9;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgIn = TypeMarshal.ToIntPtr(pmsgIn, dwInVersion, null, null);

            paramList = new Int3264[] 
			{
				hDrs,
				dwInVersion,
				ptrMsgIn,
				IntPtr.Zero,
				IntPtr.Zero,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                pdwOutVersion = TypeMarshal.ToNullableStruct<uint>(outParamList[3]);
                pmsgOut = TypeMarshal.ToNullableStruct<DRS_MSG_REVMEMB_REPLY>(
                    outParamList[4], pdwOutVersion, null, null);
                retVal = outParamList[5].ToUInt32();
            }

            ptrMsgIn.Dispose();

            return retVal;
        }


        /// <summary>
        ///  The IDL_DRSInterDomainMove method is a helper method used in a cross-NC move LDAP operation.
        ///  Opnum: 10 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        public uint IDL_DRSInterDomainMove(
            IntPtr hDrs,
            uint dwInVersion,
            DRS_MSG_MOVEREQ? pmsgIn,
            out uint? pdwOutVersion,
            out DRS_MSG_MOVEREPLY? pmsgOut)
        {
            const ushort opnum = 10;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgIn = TypeMarshal.ToIntPtr(pmsgIn, dwInVersion, null, null);

            paramList = new Int3264[] 
			{
				hDrs,
				dwInVersion,
				ptrMsgIn,
				IntPtr.Zero,
				IntPtr.Zero,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                pdwOutVersion = TypeMarshal.ToNullableStruct<uint>(outParamList[3]);
                pmsgOut = TypeMarshal.ToNullableStruct<DRS_MSG_MOVEREPLY>(
                    outParamList[4], pdwOutVersion, null, null);
                retVal = outParamList[5].ToUInt32();
            }

            ptrMsgIn.Dispose();

            return retVal;
        }


        /// <summary>
        ///  If the server is the PDC emulator FSMO role owner, the IDL_DRSGetNT4ChangeLog method
        ///  returns either a sequence of PDC change log entries or the NT4 replication state, or
        ///  both, as requested by the client. 
        ///  Opnum: 11 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        public uint IDL_DRSGetNT4ChangeLog(
            IntPtr hDrs,
            uint dwInVersion,
            DRS_MSG_NT4_CHGLOG_REQ? pmsgIn,
            out uint? pdwOutVersion,
            out DRS_MSG_NT4_CHGLOG_REPLY? pmsgOut)
        {
            const ushort opnum = 11;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgIn = TypeMarshal.ToIntPtr(pmsgIn, dwInVersion, null, null);

            paramList = new Int3264[] 
			{
				hDrs,
				dwInVersion,
				ptrMsgIn,
				IntPtr.Zero,
				IntPtr.Zero,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                pdwOutVersion = TypeMarshal.ToNullableStruct<uint>(outParamList[3]);
                pmsgOut = TypeMarshal.ToNullableStruct<DRS_MSG_NT4_CHGLOG_REPLY>(
                    outParamList[4], pdwOutVersion, null, null);
                retVal = outParamList[5].ToUInt32();
            }

            ptrMsgIn.Dispose();

            return retVal;
        }


        /// <summary>
        ///  The IDL_DRSAddEntry method adds one or more objects. 
        ///  Opnum: 17 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        public uint IDL_DRSAddEntry(
            IntPtr hDrs,
            uint dwInVersion,
            DRS_MSG_ADDENTRYREQ? pmsgIn,
            out uint? pdwOutVersion,
            out DRS_MSG_ADDENTRYREPLY? pmsgOut)
        {
            const ushort opnum = 17;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgIn = TypeMarshal.ToIntPtr(pmsgIn, dwInVersion, null, null);

            paramList = new Int3264[] 
			{
				hDrs,
				dwInVersion,
				ptrMsgIn,
				IntPtr.Zero,
				IntPtr.Zero,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                pdwOutVersion = TypeMarshal.ToNullableStruct<uint>(outParamList[3]);
                pmsgOut = TypeMarshal.ToNullableStruct<DRS_MSG_ADDENTRYREPLY>(
                    outParamList[4], pdwOutVersion, null, null);
                retVal = outParamList[5].ToUInt32();
            }

            ptrMsgIn.Dispose();

            return retVal;
        }


        /// <summary>
        ///  The IDL_DRSGetMemberships2 method retrieves group memberships for a sequence of objects. 
        ///  Opnum: 21 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        public uint IDL_DRSGetMemberships2(
            IntPtr hDrs,
            uint dwInVersion,
            DRS_MSG_GETMEMBERSHIPS2_REQ? pmsgIn,
            out uint? pdwOutVersion,
            out DRS_MSG_GETMEMBERSHIPS2_REPLY? pmsgOut)
        {
            const ushort opnum = 21;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgIn = TypeMarshal.ToIntPtr(pmsgIn, dwInVersion, null, null);

            paramList = new Int3264[] 
			{
				hDrs,
				dwInVersion,
				ptrMsgIn,
				IntPtr.Zero,
				IntPtr.Zero,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                pdwOutVersion = TypeMarshal.ToNullableStruct<uint>(outParamList[3]);
                pmsgOut = TypeMarshal.ToNullableStruct<DRS_MSG_GETMEMBERSHIPS2_REPLY>(
                    outParamList[4], pdwOutVersion, null, null);
                retVal = outParamList[5].ToUInt32();
            }

            ptrMsgIn.Dispose();

            return retVal;
        }


        /// <summary>
        ///  The IDL_DRSGetObjectExistence method helps the client check the consistency of object
        ///  existence between its replica of an NC and the server's replica of the same NC. 
        ///  Checking the consistency of object existence means identifying objects that have 
        ///  replicated to both replicas and that exist in one replica but not in the other.
        ///  For the purposes of this method, an object exists within a NC replica if it is either
        ///  an object or a tombstone.See IDL_DRSReplicaVerifyObjects for a use of this method. 
        ///  Opnum: 23 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        public uint IDL_DRSGetObjectExistence(
            IntPtr hDrs,
            uint dwInVersion,
            DRS_MSG_EXISTREQ? pmsgIn,
            out uint? pdwOutVersion,
            out DRS_MSG_EXISTREPLY? pmsgOut)
        {
            const ushort opnum = 23;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgIn = TypeMarshal.ToIntPtr(pmsgIn, dwInVersion, null, null);

            paramList = new Int3264[] 
			{
				hDrs,
				dwInVersion,
				ptrMsgIn,
				IntPtr.Zero,
				IntPtr.Zero,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                pdwOutVersion = TypeMarshal.ToNullableStruct<uint>(outParamList[3]);
                pmsgOut = TypeMarshal.ToNullableStruct<DRS_MSG_EXISTREPLY>(
                    outParamList[4], pdwOutVersion, null, null);
                retVal = outParamList[5].ToUInt32();
            }

            ptrMsgIn.Dispose();

            return retVal;
        }

        /// <summary>
        ///  The IDL_DRSAddCloneDC method is used to create a new DC object by 
        ///  copying attributes from an existing DC object.
        ///  Opnum: 28 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        public uint IDL_DRSAddCloneDC(
            IntPtr hDrs,
            uint dwInVersion,
            DRS_MSG_ADDCLONEDCREQ? pmsgIn,
            out uint? pdwOutVersion,
            out DRS_MSG_ADDCLONEDCREPLY? pmsgOut)
        {
            const ushort opnum = 28;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgIn = TypeMarshal.ToIntPtr(
                pmsgIn,
                dwInVersion,
                null,
                null
            );

            paramList = new Int3264[] {
					hDrs,
					(uint)dwInVersion,
					ptrMsgIn,
					IntPtr.Zero,
					IntPtr.Zero,
					0
				};

            requestStub = RpceStubEncoder.ToBytes(
                RpceStubHelper.GetPlatform(),
                DrsrRpcStubFormatString.TypeFormatString,
                null,
                DrsrRpcStubFormatString.ProcFormatString,
                DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                true,
                paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                pdwOutVersion = TypeMarshal.ToNullableStruct<uint>(outParamList[3]);
                pmsgOut = TypeMarshal.ToNullableStruct<DRS_MSG_ADDCLONEDCREPLY>(
                        outParamList[4], pdwOutVersion, null, null);
                retVal = outParamList[5].ToUInt32();
            }

            ptrMsgIn.Dispose();

            return retVal;
        }

        /// <summary>
        ///  The IDL_DRSWriteNgcKey method composes and updates the
        ///  msDS-KeyCredentialLink value on an object.
        ///  Opnum: 29
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message. MUST be set to 1,
        ///  because that is the only version supported.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message. The
        ///  value is always 1 because that is the only version
        ///  supported.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        public uint IDL_DRSWriteNgcKey(
            IntPtr hDrs,
            uint dwInVersion,
            DRS_MSG_WRITENGCKEYREQ? pmsgIn,
            out uint? pdwOutVersion,
            out DRS_MSG_WRITENGCKEYREPLY? pmsgOut)
        {
            const ushort opnum = 29;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgIn = TypeMarshal.ToIntPtr(pmsgIn, dwInVersion, null, null);

            paramList = new Int3264[] 
			{
				hDrs,
				dwInVersion,
				ptrMsgIn,
				IntPtr.Zero,
				IntPtr.Zero,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                pdwOutVersion = TypeMarshal.ToNullableStruct<uint>(outParamList[3]);
                pmsgOut = TypeMarshal.ToNullableStruct<DRS_MSG_WRITENGCKEYREPLY>(
                    outParamList[4], pdwOutVersion, null, null);
                retVal = outParamList[5].ToUInt32();
            }

            ptrMsgIn.Dispose();

            return retVal;
        }

        /// <summary>
        ///  The IDL_DRSReadNgcKey method reads and parses the
        ///  msDS-KeyCredentialLink value on an object.
        ///  Opnum: 30
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message. MUST be set to 1,
        ///  because that is the only version supported.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message. The
        ///  value is always 1 because that is the only version
        ///  supported.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        public uint IDL_DRSReadNgcKey(
            IntPtr hDrs,
            uint dwInVersion,
            DRS_MSG_READNGCKEYREQ? pmsgIn,
            out uint? pdwOutVersion,
            out DRS_MSG_READNGCKEYREPLY? pmsgOut)
        {
            const ushort opnum = 30;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgIn = TypeMarshal.ToIntPtr(pmsgIn, dwInVersion, null, null);

            paramList = new Int3264[] 
			{
				hDrs,
				dwInVersion,
				ptrMsgIn,
				IntPtr.Zero,
				IntPtr.Zero,
				0//retVal
			};

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                pdwOutVersion = TypeMarshal.ToNullableStruct<uint>(outParamList[3]);
                pmsgOut = TypeMarshal.ToNullableStruct<DRS_MSG_READNGCKEYREPLY>(
                    outParamList[4], pdwOutVersion, null, null);
                retVal = outParamList[5].ToUInt32();
            }

            ptrMsgIn.Dispose();

            return retVal;
        }

        #endregion


        #region IDisposable Members

        /// <summary>
        /// Dispose method.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Dispose method.
        /// </summary>
        /// <param name="disposing">
        /// True to release both managed and unmanaged resources.<para/>
        /// False to release unmanaged resources only.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Release managed resources.
                if (rpceClientTransport != null)
                {
                    rpceClientTransport.Dispose();
                    rpceClientTransport = null;
                }
            }

            // Release unmanaged resources.
        }


        /// <summary>
        /// finalizer
        /// </summary>
        ~DrsuapiRpcAdapter()
        {
            Dispose(false);
        }

        #endregion
    }
}
