// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// P/Invoke for RPCE stub.
    /// </summary>
    internal static class NativeMethods
    {
        #region Constant definition

        // success
        internal const int RPC_S_OK = 0;

        // The object UUID has already been registered.
        internal const int RPC_S_ALREADY_REGISTERED = 1712;

        // The server is already listening.
        internal const int RPC_S_ALREADY_LISTENING = 1713;

        // This value sets the RPC library to use the default value.
        internal const uint RPC_C_PROTSEQ_MAX_REQS_DEFAULT = 10;

        // Recommended maximum calls.
        internal const uint RPC_C_LISTEN_MAX_CALLS_DEFAULT = 1234;

        // NDR local data representation.
        internal const uint NDR_LOCAL_DATA_REPRESENTATION = 0x00000010;

        // define RPC_BUFFER_COMPLETE according to Windows SDK.
        internal const uint RPC_BUFFER_COMPLETE = 0x00001000;

        #endregion


        # region Delegation definition

        /// <summary>
        /// Delegation function to allocate memory, used by NDR engine.
        /// </summary>
        /// <param name="s">Buffer size.</param>
        /// <returns>Pointer to allocated memory.</returns>
        internal delegate IntPtr PfnAllocate(uint s);


        /// <summary>
        /// Delegation function to free allocated memory , used by NDR engine.
        /// </summary>
        /// <param name="f">Pointer to allocated memory.</param>
        internal delegate void PfnFree(IntPtr f);


        /// <summary>
        /// Expression evaluation callback routine.
        /// </summary>
        /// <param name="stubMsg">pointer to stub message structure</param>
        internal delegate void EXPR_EVAL(IntPtr stubMsg);

        #endregion


        #region Structure definition

#pragma warning disable 649

        /// <summary>
        /// Copied from midl.exe generated stub code.
        /// </summary>
        internal struct RPC_VERSION
        {
            public ushort MajorVersion;
            public ushort MinorVersion;
        }


        /// <summary>
        /// Copied from midl.exe generated stub code.
        /// </summary>
        internal struct RPC_SYNTAX_IDENTIFIER
        {
            public Guid SyntaxGUID;
            public RPC_VERSION SyntaxVersion;
        }


        /// <summary>
        /// Copied from midl.exe generated stub code.
        /// </summary>
        internal struct RPC_CLIENT_INTERFACE
        {
            public uint Length;
            public RPC_SYNTAX_IDENTIFIER InterfaceId;
            public RPC_SYNTAX_IDENTIFIER TransferSyntax;
            public IntPtr DispatchTable;
            public uint RpcProtseqEndpointCount;
            public IntPtr RpcProtseqEndpoint;
            public uint Reserved;
            public IntPtr InterpreterInfo;
            public uint Flags;
        }


        /// <summary>
        /// Copied from midl.exe generated stub code.
        /// </summary>
        internal struct RPC_SERVER_INTERFACE
        {
            public uint Length;
            public RPC_SYNTAX_IDENTIFIER InterfaceId;
            public RPC_SYNTAX_IDENTIFIER TransferSyntax;
            public IntPtr DispatchTable;
            public uint RpcProtseqEndpointCount;
            public IntPtr RpcProtseqEndpoint;
            public IntPtr DefaultManagerEpv;
            public IntPtr InterpreterInfo;
            public uint Flags;
        }


        /// <summary>
        /// The RPC_MESSAGE structure contains information shared between 
        /// NDR and the rest of the RPC or OLE runtime.
        /// </summary>
        internal struct RPC_MESSAGE
        {
            public IntPtr Handle;
            public uint DataRepresentation;
            public IntPtr Buffer;
            public uint BufferLength;
            public uint ProcNum;
            public IntPtr TransferSyntax;  //PRPC_SYNTAX_IDENTIFIER 
            public IntPtr RpcInterfaceInformation;
            public IntPtr ReservedForRuntime;
            public IntPtr ManagerEpv;  //RPC_MGR_EPV *
            public IntPtr ImportContext;
            public uint RpcFlags;
        }


        /// <summary>
        /// The MIDL_STUB_DESC structure is a MIDL-generated structure that contains 
        /// information about the interface stub regarding RPC calls between the client and server.
        /// </summary>
        // Because we release unmanaged resouces in RpceStub.Dispose, suppress fxcop CA1049.
        [SuppressMessage("Microsoft.Design", "CA1049:TypesThatOwnNativeResourcesShouldBeDisposable")]
        internal struct MIDL_STUB_DESC
        {
            public IntPtr RpcInterfaceInformation;

            [MarshalAs(UnmanagedType.FunctionPtr)]
            public PfnAllocate pfnAllocate;

            [MarshalAs(UnmanagedType.FunctionPtr)]
            public PfnFree pfnFree;

            public IntPtr pHandle;

            public IntPtr apfnNdrRundownRoutines;
            public IntPtr aGenericBindingRoutinePairs;
            public IntPtr apfnExprEval;
            public IntPtr aXmitQuintuple;

            public IntPtr pFormatTypes;

            public int fCheckBounds;
            public uint Version;
            public IntPtr pMallocFreeStruct;
            public int MIDLVersion;
            public IntPtr CommFaultOffsets;
            public IntPtr aUserMarshalQuadruple;
            public IntPtr NotifyRoutineTable;
            public uint mFlags;
            public IntPtr CsRoutineTables;
            public IntPtr ProxyServerInfo;
            public IntPtr pExprInfo;
        }


        /// <summary>
        /// The MIDL_STUB_MESSAGE structure is generated by MIDL and 
        /// contains the current status of the RPC stub. 
        /// Applications are not to modify the MIDL_STUB_MESSAGE structure directly.
        /// </summary>
        internal struct MIDL_STUB_MESSAGE
        {
            public IntPtr RpcMsg;  //PRPC_MESSAGE
            public IntPtr Buffer;
            public IntPtr BufferStart;
            public IntPtr BufferEnd;
            public IntPtr BufferMark;
            public uint BufferLength;
            public uint MemorySize;
            public IntPtr Memory;
            public int IsClient;
            public int ReuseBuffer;
            public IntPtr pAllocAllNodesContext;  //struct NDR_ALLOC_ALL_NODES_CONTEXT *
            public IntPtr pPointerQueueState;  //struct NDR_POINTER_QUEUE_STATE *
            public int IgnoreEmbeddedPointers;
            public IntPtr PointerBufferMark;
            public byte fBufferValid;
            public byte uFlags;
            public ushort Unused2;
            public IntPtr MaxCount;  //ULONG_PTR
            public uint Offset;
            public uint ActualCount;
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public PfnAllocate pfnAllocate;
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public PfnFree pfnFree;
            public IntPtr StackTop;
            public IntPtr pPresentedType;
            public IntPtr pTransmitType;
            public IntPtr SavedHandle;
            public IntPtr StubDesc;  //const struct _MIDL_STUB_DESC *
            public IntPtr FullPtrXlatTables;  //struct _FULL_PTR_XLAT_TABLES *
            public uint FullPtrRefId;
            public uint PointerLength;
            public uint Flags;
            //int fInDontFree  :1;
            //int fDontCallFreeInst  :1;
            //int fInOnlyParam  :1;
            //int fHasReturn  :1;
            //int fHasExtensions  :1;
            //int fHasNewCorrDesc  :1;
            //int fUnused  :10;
            //int fUnused2  :16;
            public uint dwDestContext;
            public IntPtr pvDestContext;
            public IntPtr SavedContextHandles;  //NDR_SCONTEXT *
            public int ParamNumber;
            public IntPtr pRpcChannelBuffer;  //struct IRpcChannelBuffer *
            public IntPtr pArrayInfo;  //PARRAY_INFO
            public IntPtr SizePtrCountArray;  //unsigned long *
            public IntPtr SizePtrOffsetArray;  //unsigned long *
            public IntPtr SizePtrLengthArray;  //unsigned long *
            public IntPtr pArgQueue;
            public uint dwStubPhase;
            public IntPtr LowStackMark;
            public IntPtr pAsyncMsg;  //PNDR_ASYNC_MESSAGE
            public IntPtr pCorrInfo;  //PNDR_CORRELATION_INFO
            public IntPtr pCorrMemory;  //unsigned char *
            public IntPtr pMemoryList;
            public IntPtr pCSInfo;  //CS_STUB_INFO *
            public IntPtr ConformanceMark;  //unsigned char *
            public IntPtr VarianceMark;  //unsigned char *
            public IntPtr BackingStoreLowMark;
            public IntPtr Unused;  //INT_PTR
            public IntPtr pContext;  //struct _NDR_PROC_CONTEXT *
            public IntPtr Reserved51_1;  //INT_PTR
            public IntPtr Reserved51_2;  //INT_PTR
            public IntPtr Reserved51_3;  //INT_PTR
            public IntPtr Reserved51_4;  //INT_PTR
            public IntPtr Reserved51_5;  //INT_PTR
        }

#pragma warning restore 649

        #endregion


        #region RPC Windows API definition

        /// <summary>
        /// The RpcServerUseProtseqEp function tells the RPC run-time library 
        /// to use the specified protocol sequence combined with the specified endpoint 
        /// for receiving remote procedure calls.
        /// </summary>
        /// <param name="Protseq">
        /// Pointer to a string identifier of the protocol sequence to register with the RPC run-time library.
        /// </param>
        /// <param name="MaxCalls">
        /// Backlog queue length for the ncacn_ip_tcp protocol sequence. 
        /// All other protocol sequences ignore this parameter. 
        /// Use RPC_C_PROTSEQ_MAX_REQS_DEFAULT to specify the default value.
        /// </param>
        /// <param name="Endpoint">
        /// Pointer to the endpoint-address information to use in creating a binding 
        /// for the protocol sequence specified in the Protseq parameter.
        /// </param>
        /// <param name="SecurityDescriptor">
        /// Pointer to an optional parameter provided for the security subsystem. 
        /// Used only for ncacn_np and ncalrpc protocol sequences. 
        /// All other protocol sequences ignore this parameter. 
        /// Using a security descriptor on the endpoint in order to make a server secure is not recommended. 
        /// This parameter does not appear in the DCE specification for this API.
        /// </param>
        /// <returns>
        /// RPC_S_OK The call succeeded.<para/>
        /// RPC_S_PROTSEQ_NOT_SUPPORTED The protocol sequence is not supported on this host.<para/>
        /// RPC_S_INVALID_RPC_PROTSEQ The protocol sequence is invalid.<para/>
        /// RPC_S_INVALID_ENDPOINT_FORMAT The endpoint format is invalid.<para/>
        /// RPC_S_OUT_OF_MEMORY The system is out of memory.<para/>
        /// RPC_S_DUPLICATE_ENDPOINT The endpoint is a duplicate.<para/>
        /// RPC_S_INVALID_SECURITY_DESC The security descriptor is invalid.
        /// </returns>
        [DllImport("rpcrt4.dll")]
        internal extern static int RpcServerUseProtseqEp(
            string Protseq,
            uint MaxCalls,
            string Endpoint,
            IntPtr SecurityDescriptor);


        /// <summary>
        /// The RpcServerRegisterIf function registers an interface with the RPC run-time library.
        /// </summary>
        /// <param name="IfSpec">MIDL-generated structure indicating the interface to register.</param>
        /// <param name="MgrTypeUuid">
        /// Pointer to a type UUID to associate with the MgrEpv parameter. 
        /// Specifying a null parameter value (or a nil UUID) registers IfSpec with a nil-type UUID.
        /// </param>
        /// <param name="MgrEpv">
        /// Manager routines' entry-point vector (EPV). 
        /// To use the MIDL-generated default EPV, specify a null value.
        /// </param>
        /// <returns>Returns RPC_S_OK upon success.</returns>
        [DllImport("rpcrt4.dll")]
        internal extern static int RpcServerRegisterIf(
            IntPtr IfSpec,
            IntPtr MgrTypeUuid,
            IntPtr MgrEpv);


        /// <summary>
        /// The RpcServerUnregisterIf function removes an interface from the RPC run-time library registry.
        /// </summary>
        /// <param name="IfSpec">Interface to remove from the registry.</param>
        /// <param name="MgrTypeUuid">
        /// Pointer to the type UUID of the manager entry-point vector (EPV) to remove from the registry.
        /// </param>
        /// <param name="WaitForCallsToComplete">
        /// Flag that indicates whether to remove the interface 
        /// from the registry immediately or to wait until all current calls are complete.<para/>
        /// Specify a value of zero to disregard calls in progress and 
        /// remove the interface from the registry immediately.<para/>
        /// Specify any nonzero value to wait until all active calls complete.
        /// </param>
        /// <returns>
        /// RPC_S_OK The call succeeded.<para/>
        /// RPC_S_UNKNOWN_MGR_TYPE The manager type is unknown.<para/>
        /// RPC_S_UNKNOWN_IF The interface is unknown.
        /// </returns>
        [DllImport("rpcrt4.dll")]
        internal extern static int RpcServerUnregisterIf(
            IntPtr IfSpec,
            IntPtr MgrTypeUuid,
            uint WaitForCallsToComplete);


        /// <summary>
        /// The RpcServerListen function signals the RPC run-time library 
        /// to listen for remote procedure calls.
        /// </summary>
        /// <param name="MinimumCallThreads">
        /// Hint to the RPC run time that specifies the minimum 
        /// number of call threads that should 
        /// be created and maintained in the given server.
        /// </param>
        /// <param name="MaxCalls">
        /// Recommended maximum number of concurrent remote procedure calls 
        /// the server can execute.
        /// </param>
        /// <param name="DontWait">
        /// Flag controlling the return from RpcServerListen. 
        /// A value of nonzero indicates that RpcServerListen 
        /// should return immediately after completing function processing. 
        /// A value of zero indicates that RpcServerListen should not return 
        /// until the RpcMgmtStopServerListening function has been called and 
        /// all remote calls have completed.
        /// </param>
        /// <returns>
        /// RPC_S_OK The call succeeded.<para/>
        /// RPC_S_ALREADY_LISTENING The server is already listening.<para/>
        /// RPC_S_NO_PROTSEQS_REGISTERED There are no protocol sequences registered.<para/>
        /// RPC_S_MAX_CALLS_TOO_SMALL The maximum calls value is too small.
        /// </returns>
        [DllImport("rpcrt4.dll")]
        internal extern static int RpcServerListen(
            uint MinimumCallThreads,
            uint MaxCalls,
            uint DontWait);


        /// <summary>
        /// The RpcMgmtStopServerListening function tells a server to stop listening for remote procedure calls.
        /// </summary>
        /// <param name="Binding">
        /// To direct a remote application to stop listening for remote procedure calls, 
        /// specify a server binding handle for that application. 
        /// To direct your own (local) application to stop listening for remote procedure calls, 
        /// specify a value of NULL.
        /// </param>
        /// <returns>
        /// RPC_S_OK The call succeeded.<para/>
        /// RPC_S_INVALID_BINDING The binding handle was invalid.<para/>
        /// RPC_S_WRONG_KIND_OF_BINDING This was the wrong kind of binding for the operation.
        /// </returns>
        [DllImport("rpcrt4.dll")]
        internal extern static int RpcMgmtStopServerListening(IntPtr Binding);


        /// <summary>
        /// The RpcStringBindingCompose function creates a string binding handle.
        /// </summary>
        /// <param name="ObjUuid">Pointer to a null-terminated string representation of an object UUID.</param>
        /// <param name="ProtSeq">Pointer to a null-terminated string representation of a protocol sequence.</param>
        /// <param name="NetworkAddr">
        /// Pointer to a null-terminated string representation of a network address.
        /// The network-address format is associated with the protocol sequence.
        /// </param>
        /// <param name="EndPoint">
        /// Pointer to a null-terminated string representation of an endpoint.
        /// The endpoint format and content are associated with the protocol sequence.
        /// </param>
        /// <param name="Options">
        /// Pointer to a null-terminated string representation of network options.
        /// The option string is associated with the protocol sequence.
        /// </param>
        /// <param name="StringBinding">
        /// Returns a pointer to a pointer to a null-terminated string representation of a binding handle.
        /// </param>
        /// <returns>
        /// RPC_S_OK The call succeeded.<para/>
        /// RPC_S_INVALID_STRING_UUID The string representation of the UUID is not valid.
        /// </returns>
        [DllImport("rpcrt4.dll")]
        internal static extern int RpcStringBindingCompose(
            string ObjUuid,
            string ProtSeq,
            string NetworkAddr,
            string EndPoint,
            string Options,
            out IntPtr StringBinding);


        /// <summary>
        /// The RpcBindingFromStringBinding function returns a binding handle 
        /// from a string representation of a binding handle.
        /// </summary>
        /// <param name="StringBinding">Pointer to a string representation of a binding handle.</param>
        /// <param name="Binding">Returns a pointer to the server binding handle.</param>
        /// <returns>
        /// RPC_S_OK The call succeeded.<para/>
        /// RPC_S_INVALID_STRING_BINDING The string binding is not valid.<para/>
        /// RPC_S_PROTSEQ_NOT_SUPPORTED Protocol sequence not supported on this host.<para/>
        /// RPC_S_INVALID_RPC_PROTSEQ The protocol sequence is not valid.<para/>
        /// RPC_S_INVALID_ENDPOINT_FORMAT The endpoint format is not valid.<para/>
        /// RPC_S_STRING_TOO_LONG String too long.<para/>
        /// RPC_S_INVALID_NET_ADDR The network address is not valid.<para/>
        /// RPC_S_INVALID_ARG The argument was not valid.<para/>
        /// RPC_S_INVALID_NAF_ID The network address family identifier is not valid.
        /// </returns>
        [DllImport("rpcrt4.dll")]
        internal static extern int RpcBindingFromStringBinding(
            IntPtr StringBinding,
            out IntPtr Binding);


        /// <summary>
        /// The RpcStringFree function frees a character string allocated by the RPC run-time library.
        /// </summary>
        /// <param name="String">Pointer to a pointer to the character string to free.</param>
        /// <returns>RPC_S_OK The call succeeded.</returns>
        [DllImport("rpcrt4.dll")]
        internal static extern int RpcStringFree(ref IntPtr String);


        /// <summary>
        /// The RpcBindingFree function releases binding-handle resources.
        /// </summary>
        /// <param name="Binding">Pointer to the server binding to be freed.</param>
        /// <returns>
        /// RPC_S_OK The call succeeded.<para/>
        /// RPC_S_INVALID_BINDING The binding handle was invalid.<para/>
        /// RPC_S_WRONG_KIND_OF_BINDING This was the wrong kind of binding for the operation.
        /// </returns>
        [DllImport("rpcrt4.dll")]
        public static extern int RpcBindingFree(ref IntPtr Binding);

        #endregion


        #region NDR Windows API definition

        /// <summary>
        ///  This routine is called by client side stubs to initialize the RPC message  
        /// and stub message, and to get the RPC buffer.
        /// </summary>
        /// <param name="pRpcMsg">pointer to RPC message structure</param>
        /// <param name="pStubMsg">pointer to stub message structure</param>
        /// <param name="pStubDescriptor">pointer to stub descriptor structure</param>
        /// <param name="ProcNum">remote procedure number</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrClientInitializeNew(
            IntPtr pRpcMsg,
            IntPtr pStubMsg,
            IntPtr pStubDescriptor,
            uint ProcNum);


        /// <summary>
        /// This routine is called by the server stubs before unmarshalling.<para />
        /// It initializes the stub message fields.<para />
        /// Note:<para />
        /// NdrServerInitializeNew is almost identical to NdrServerInitializePartial.<para />
        /// NdrServerInitializeNew is generated for non-pipes and is backward comp.<para />
        /// NdrServerInitializePartial is generated for routines with pipes args.<para />
        /// </summary>
        /// <param name="pRpcMsg">pointer to RPC message structure</param>
        /// <param name="pStubMsg">pointer to the stub message structure</param>
        /// <param name="pStubDescriptor">pointer to the stub descriptor structure</param>
        [DllImport("rpcrt4.dll")]
        internal extern static IntPtr NdrServerInitializeNew(
            IntPtr pRpcMsg,
            IntPtr pStubMsg,
            IntPtr pStubDescriptor);


        /// <summary>
        /// Performs an RpcGetBuffer.
        /// </summary>
        /// <param name="pStubMsg">Pointer to stub message structure.</param>
        /// <param name="BufferLength">Length of requested rpc message buffer.</param>
        /// <param name="Handle">Bound handle.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static IntPtr NdrGetBuffer(
            IntPtr pStubMsg,
            uint BufferLength,
            IntPtr Handle);


        /// <summary>
        /// Performs an RpcFreeBuffer.
        /// </summary>
        /// <param name="pStubMsg">pointer to stub message structure</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrFreeBuffer(IntPtr pStubMsg);


        /// <summary>
        /// The NdrConvert2 function converts the network buffer from the data representation of
        /// the sender to the data representation of the receiver if they are different.
        /// This routine handles the conversion of all parameters in a procedure.
        /// </summary>
        /// <param name="pStubMsg">
        /// Pointer to a MIDL_STUB_MESSAGE structure that maintains the current status of the RPC stub.
        /// The pRpcMsg member points to a structure whose Buffer member contains the data to convert.
        /// This structure is for internal use only and should not be modified.</param>
        /// <param name="pFormat">Pointer to type format of the data to convert.</param>
        /// <param name="NumberParams">The number of parameters in the procedure.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrConvert2(
            IntPtr pStubMsg,
            IntPtr pFormat,
            long NumberParams);


        /// <summary>
        /// The NdrPointerBufferSize function computes the needed buffer size, in bytes, 
        /// for a top-level pointer to anything.
        /// </summary>
        /// <param name="pStubMsg">
        /// Pointer to a MIDL_STUB_MESSAGE structure that maintains the current status of the RPC stub. 
        /// The BufferLength member contains the size of the buffer. 
        /// This structure is for internal use only and should not be modified.
        /// </param>
        /// <param name="pMemory">Pointer to the data being sized.</param>
        /// <param name="pFormat">Pointer to the format string description.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrPointerBufferSize(
            IntPtr pStubMsg,
            IntPtr pMemory,
            IntPtr pFormat);


        /// <summary>
        /// The NdrPointerMarshall function marshals a top level pointer to anything. 
        /// Pointers embedded in structures, arrays, or unions call NdrPointerMarshall directly. 
        /// </summary>
        /// <param name="pStubMsg">
        /// Pointer to a MIDL_STUB_MESSAGE structure that maintains the current status of the RPC stub. 
        /// Structure is for internal use only; do not modify.</param>
        /// <param name="pMemory">Pointer to the pointer to be marshalled.</param>
        /// <param name="pFormat">Pointer to the format string description.</param>
        /// <returns>Returns NULL upon success. If an error occurs, the function throws exception.</returns>
        [DllImport("rpcrt4.dll")]
        internal extern static IntPtr NdrPointerMarshall(
            IntPtr pStubMsg,
            IntPtr pMemory,
            IntPtr pFormat);


        /// <summary>
        /// The NdrPointerUnmarshall function unmarshalls a top level pointer to anything. 
        /// Pointers embedded in structures, arrays, or unions call NdrPointerUnmarshall directly. 
        /// </summary>
        /// <param name="pStubMsg">
        /// Pointer to a MIDL_STUB_MESSAGE structure that maintains the current status of the RPC stub. 
        /// Structure is for internal use only; do not modify.
        /// </param>
        /// <param name="ppMemory">Pointer to memory where pointer will be unmarshalled.</param>
        /// <param name="pFormat">Pointer to the format string description.</param>
        /// <param name="fSkipRefCheck">Unused.</param>
        /// <returns>Returns NULL upon success. If an error occurs, the function throws exception.</returns>
        [DllImport("rpcrt4.dll")]
        internal extern static IntPtr NdrPointerUnmarshall(
            IntPtr pStubMsg,
            ref IntPtr ppMemory,
            IntPtr pFormat,
            byte fSkipRefCheck);


        /// <summary>
        /// The NdrSimpleStructBufferSize function calculates the required buffer size, in bytes, 
        /// to marshal the simple structure.
        /// </summary>
        /// <param name="pStubMsg">
        /// Pointer to a MIDL_STUB_MESSAGE structure that maintains the current status of the RPC stub. 
        /// The BufferLength member contains the size of the buffer. 
        /// The MIDL_STUB_MESSAGE structure is for internal use only, and must not be modified.
        /// </param>
        /// <param name="pMemory">Pointer to the simple structure to be calculated.</param>
        /// <param name="pFormat">Pointer to the format string description.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrSimpleStructBufferSize(
            IntPtr pStubMsg,
            IntPtr pMemory,
            IntPtr pFormat);


        /// <summary>
        /// The NdrSimpleStructMarshall function marshals the simple structure into a network buffer.
        /// </summary>
        /// <param name="pStubMsg">
        /// Pointer to a MIDL_STUB_MESSAGE structure that maintains the current status of the RPC stub. 
        /// The MIDL_STUB_MESSAGE structure is for internal use only, and must not be modified.
        /// </param>
        /// <param name="pMemory">Pointer to the simple structure to be marshaled.</param>
        /// <param name="pFormat">Pointer to the format string description.</param>
        /// <returns>Returns null upon success. Raises exception upon failure.</returns>
        [DllImport("rpcrt4.dll")]
        internal extern static IntPtr NdrSimpleStructMarshall(
            IntPtr pStubMsg,
            IntPtr pMemory,
            IntPtr pFormat);


        /// <summary>
        /// The NdrSimpleStructUnmarshall function unmarshals the simple structure 
        /// from the network buffer to memory.
        /// </summary>
        /// <param name="pStubMsg">
        /// Pointer to a MIDL_STUB_MESSAGE structure that maintains 
        /// the current status of the RPC stub. 
        /// The MIDL_STUB_MESSAGE structure is for internal use only, 
        /// and must not be modified. 
        /// </param>
        /// <param name="ppMemory">
        /// Address to a pointer to the unmarshalled simple structure. 
        /// If set to null, or if the fMustAlloc parameter is set to TRUE, 
        /// the stub will allocate the memory.
        /// </param>
        /// <param name="pFormat">Pointer to the format string description.</param>
        /// <param name="fMustAlloc">
        /// Flag that specifies whether the stub must allocate the memory into 
        /// which the simple structure is to be marshaled. 
        /// Specify TRUE if RPC must allocate ppMemory. 
        /// </param>
        /// <returns>
        /// Returns NULL upon success. 
        /// If an error occurs, the function throws exception.
        /// </returns>
        [DllImport("rpcrt4.dll")]
        internal extern static IntPtr NdrSimpleStructUnmarshall(
            IntPtr pStubMsg,
            ref IntPtr ppMemory,
            IntPtr pFormat,
            byte fMustAlloc);


        /// <summary>
        /// The NdrComplexStructBufferSize function calculates the required buffer size, in bytes, 
        /// to marshal the complex structure.
        /// </summary>
        /// <param name="pStubMsg">
        /// Pointer to a MIDL_STUB_MESSAGE structure that maintains the current status of the RPC stub. 
        /// The BufferLength member contains the size of the buffer. 
        /// The MIDL_STUB_MESSAGE structure is for internal use only, and must not be modified.
        /// </param>
        /// <param name="pMemory">Pointer to the complex structure to be calculated.</param>
        /// <param name="pFormat">Pointer to the format string description.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrComplexStructBufferSize(
            IntPtr pStubMsg,
            IntPtr pMemory,
            IntPtr pFormat);


        /// <summary>
        /// The NdrComplexStructMarshall function marshals the complex structure into a network buffer.
        /// </summary>
        /// <param name="pStubMsg">
        /// Pointer to a MIDL_STUB_MESSAGE structure that maintains the current status of the RPC stub. 
        /// The MIDL_STUB_MESSAGE structure is for internal use only, and must not be modified.
        /// </param>
        /// <param name="pMemory">Pointer to the complex structure to be marshaled.</param>
        /// <param name="pFormat">Pointer to the format string description.</param>
        /// <returns>Returns null upon success. Raises exception upon failure.</returns>
        [DllImport("rpcrt4.dll")]
        internal extern static IntPtr NdrComplexStructMarshall(
            IntPtr pStubMsg,
            IntPtr pMemory,
            IntPtr pFormat);


        /// <summary>
        /// The NdrComplexStructUnmarshall function unmarshals the complex structure 
        /// from the network buffer to memory.
        /// </summary>
        /// <param name="pStubMsg">
        /// Pointer to a MIDL_STUB_MESSAGE structure that maintains the current status of the RPC stub. 
        /// The MIDL_STUB_MESSAGE structure is for internal use only, and must not be modified.
        /// </param>
        /// <param name="ppMemory">
        /// Address to a pointer to the unmarshalled complex structure. 
        /// If set to null, or if the fMustAlloc parameter is set to TRUE, the stub will allocate the memory.
        /// </param>
        /// <param name="pFormat">Pointer to the format string description.</param>
        /// <param name="fMustAlloc">
        /// Flag that specifies whether the stub must allocate the memory into 
        /// which the complex structure is to be marshaled. 
        /// Specify TRUE if RPC must allocate ppMemory.
        /// </param>
        /// <returns>Returns null upon success. Raises exception upon failure.</returns>
        [DllImport("rpcrt4.dll")]
        internal extern static IntPtr NdrComplexStructUnmarshall(
            IntPtr pStubMsg,
            ref IntPtr ppMemory,
            IntPtr pFormat,
            byte fMustAlloc);


        /// <summary>
        /// Get simple type buffer size.
        /// </summary>
        /// <param name="FormatChar">Format char.</param>
        /// <returns>The buffer size.</returns>
        [DllImport("rpcrt4.dll")]
        internal extern static byte NdrGetSimpleTypeBufferSize(byte FormatChar);


        /// <summary>
        /// The NdrSimpleTypeMarshall function marshals a simple type.
        /// </summary>
        /// <param name="pStubMsg">
        /// Pointer to a MIDL_STUB_MESSAGE structure that maintains the current status of the RPC stub. 
        /// Structure is for internal use only; do not modify.
        /// </param>
        /// <param name="pMemory">
        /// Pointer to the simple type to be marshalled.
        /// </param>
        /// <param name="FormatChar">
        /// Simple type format character.
        /// </param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrSimpleTypeMarshall(
            IntPtr pStubMsg,
            IntPtr pMemory,
            byte FormatChar);


        /// <summary>
        /// The NdrSimpleTypeUnmarshall function unmarshalls a simple type.
        /// </summary>
        /// <param name="pStubMsg">
        /// Pointer to a MIDL_STUB_MESSAGE structure that maintains the current status of the RPC stub. 
        /// Structure is for internal use only; do not modify.
        /// </param>
        /// <param name="pMemory">
        /// Pointer to memory to unmarshall.
        /// </param>
        /// <param name="FormatChar">
        /// Format string of the simple type.
        /// </param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrSimpleTypeUnmarshall(
            IntPtr pStubMsg,
            IntPtr pMemory,
            byte FormatChar);


        /// <summary>
        /// Computes the buffer size needed for a non encapsulated union.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="pMemory">Pointer to the union being sized.</param>
        /// <param name="pFormat">Union's format string description.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrNonEncapsulatedUnionBufferSize(
            IntPtr pStubMsg,
            IntPtr pMemory,
            IntPtr pFormat);


        /// <summary>
        /// Marshalls a non encapsulated union.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="pMemory">Pointer to the union being marshalled.</param>
        /// <param name="pFormat">Union's format string description.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrNonEncapsulatedUnionMarshall(
            IntPtr pStubMsg,
            IntPtr pMemory,
            IntPtr pFormat);


        /// <summary>
        /// Unmarshalls a non encapsulated union.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="ppMemory">Double pointer to where the union should be unmarshalled.</param>
        /// <param name="pFormat">Union's format string description.</param>
        /// <param name="fMustAlloc">Ignored.</param>
        /// <returns>None.</returns>
        [DllImport("rpcrt4.dll")]
        internal extern static IntPtr NdrNonEncapsulatedUnionUnmarshall(
            IntPtr pStubMsg,
            ref IntPtr ppMemory,
            IntPtr pFormat,
            byte fMustAlloc);


        /// <summary>
        /// Computes the buffer size needed for a top level conformant string.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="pMemory">Pointer to the array being sized.</param>
        /// <param name="pFormat">Array's format string description.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrConformantStringBufferSize(
            IntPtr pStubMsg,
            IntPtr pMemory,
            IntPtr pFormat);


        /// <summary>
        /// Marshalls a top level conformant string.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="pMemory">Pointer to the string to be marshalled.</param>
        /// <param name="pFormat">String's format string description.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrConformantStringMarshall(
            IntPtr pStubMsg,
            IntPtr pMemory,
            IntPtr pFormat);


        /// <summary>
        /// Unmarshalls a top level conformant string.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="ppMemory">Double pointer to where the string should be unmarshalled.</param>
        /// <param name="pFormat">String's format string description.</param>
        /// <param name="fMustAlloc">TRUE if the string must be allocated, FALSE otherwise.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static IntPtr NdrConformantStringUnmarshall(
            IntPtr pStubMsg,
            ref IntPtr ppMemory,
            IntPtr pFormat,
            byte fMustAlloc);


        /// <summary>
        /// The NdrConformantArrayBufferSize function calculates the required buffer size, 
        /// in bytes, to marshal the conformant array.
        /// </summary>
        /// <param name="pStubMsg">
        /// Pointer to a MIDL_STUB_MESSAGE structure that maintains the current status 
        /// of the RPC stub. The BufferLength member contains the size of the buffer. 
        /// The MIDL_STUB_MESSAGE structure is for internal use only, and must not be modified. 
        /// </param>
        /// <param name="pMemory">
        /// Pointer to the conformant array to be calculated.
        /// </param>
        /// <param name="pFormat">
        /// Pointer to the format string description.
        /// </param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrConformantArrayBufferSize(
            IntPtr pStubMsg,
            IntPtr pMemory,
            IntPtr pFormat);


        /// <summary>
        /// The NdrConformantArrayMarshall function marshals the conformant array into a network buffer.
        /// </summary>
        /// <param name="pStubMsg">
        /// Pointer to a MIDL_STUB_MESSAGE structure that maintains the current status of the RPC stub. 
        /// The MIDL_STUB_MESSAGE structure is for internal use only, and must not be modified.
        /// </param>
        /// <param name="pMemory">Pointer to the conformant array to be marshaled.</param>
        /// <param name="pFormat">Pointer to the format string description.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrConformantArrayMarshall(
            IntPtr pStubMsg,
            IntPtr pMemory,
            IntPtr pFormat);


        /// <summary>
        /// Unmarshalls a top level one dimensional conformant array.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="ppMemory">Pointer to array to be unmarshalled.</param>
        /// <param name="pFormat">Array's format string description.</param>
        /// <param name="fMustAlloc">TRUE if the string must be allocated, FALSE otherwise.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrConformantArrayUnmarshall(
            IntPtr pStubMsg,
            ref IntPtr ppMemory,
            IntPtr pFormat,
            byte fMustAlloc);


        /// <summary>
        /// Computes the buffer size needed for a fixed array of any number of dimensions.  
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="pMemory">Pointer to the array being sized.</param>
        /// <param name="pFormat">Array's format string description.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrFixedArrayBufferSize(
            IntPtr pStubMsg,
            IntPtr pMemory,
            IntPtr pFormat);


        /// <summary>
        /// Marshalls a fixed array of any number of dimensions.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="pMemory">Pointer to the array to be marshalled.</param>
        /// <param name="pFormat">Array's format string description.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrFixedArrayMarshall(
            IntPtr pStubMsg,
            IntPtr pMemory,
            IntPtr pFormat);


        /// <summary>
        /// Unmarshalls a fixed array of any number of dimensions.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="ppMemory">Pointer to the array to unmarshall.</param>
        /// <param name="pFormat">Array's format string description.</param>
        /// <param name="fMustAlloc">TRUE if the array must be allocated, FALSE otherwise.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrFixedArrayUnmarshall(
            IntPtr pStubMsg,
            ref IntPtr ppMemory,
            IntPtr pFormat,
            byte fMustAlloc);


        /// <summary>
        /// Computes the buffer size needed for a conformant structure.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="pMemory">Pointer to the structure being sized.</param>
        /// <param name="pFormat">Structure's format string description.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrConformantStructBufferSize(
            IntPtr pStubMsg,
            IntPtr pMemory,
            IntPtr pFormat);


        /// <summary>
        /// Marshalls a conformant structure.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="pMemory">Pointer to the structure to be marshalled.</param>
        /// <param name="pFormat">Structure's format string description.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrConformantStructMarshall(
            IntPtr pStubMsg,
            IntPtr pMemory,
            IntPtr pFormat);


        /// <summary>
        /// Unmarshalls a conformant structure.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="ppMemory">Double pointer to where the structure should be unmarshalled.</param>
        /// <param name="pFormat">Structure's format string description.</param>
        /// <param name="fMustAlloc">TRUE if the structure must be allocate, FALSE otherwise.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrConformantStructUnmarshall(
            IntPtr pStubMsg,
            ref IntPtr ppMemory,
            IntPtr pFormat,
            byte fMustAlloc);


        /// <summary>
        /// Computes the buffer size needed for a conformant varying structure.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="pMemory">Pointer to the structure being sized.</param>
        /// <param name="pFormat">Structure's format string description.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrConformantVaryingStructBufferSize(
            IntPtr pStubMsg, 
            IntPtr pMemory, 
            IntPtr pFormat);


        /// <summary>
        /// Marshalls a structure which contains a conformant varying array.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="pMemory">Pointer to the structure to be marshalled.</param>
        /// <param name="pFormat">Structure's format string description.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrConformantVaryingStructMarshall(
            IntPtr pStubMsg, 
            IntPtr pMemory, 
            IntPtr pFormat);


        /// <summary>
        /// Unmarshalls a structure which contains a conformant varying array.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="ppMemory">Double pointer to where the structure should be unmarshalled.</param>
        /// <param name="pFormat">Structure's format string description.</param>
        /// <param name="fMustAlloc">Ignored.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrConformantVaryingStructUnmarshall(
            IntPtr pStubMsg,
            ref IntPtr ppMemory,
            IntPtr pFormat,
            byte fMustAlloc);


        /// <summary>
        /// Computes the buffer size needed for a top level one dimensional conformant 
        /// varying array.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="pMemory">Pointer to the array being sized.</param>
        /// <param name="pFormat">Array's format string description.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrConformantVaryingArrayBufferSize(
            IntPtr pStubMsg, 
            IntPtr pMemory, 
            IntPtr pFormat);


        /// <summary>
        /// Marshalls a top level one dimensional conformant varying array.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="pMemory">Pointer to the array being marshalled.</param>
        /// <param name="pFormat">Array's format string description.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrConformantVaryingArrayMarshall(
            IntPtr pStubMsg, 
            IntPtr pMemory, 
            IntPtr pFormat);

        
        /// <summary>
        /// Unmarshalls a top level one dimensional conformant varying array.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="ppMemory">Pointer to the array being unmarshalled.</param>
        /// <param name="pFormat">Array's format string description.</param>
        /// <param name="fMustAlloc">Ignored.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrConformantVaryingArrayUnmarshall(
            IntPtr pStubMsg,
            ref IntPtr ppMemory,
            IntPtr pFormat,
            byte fMustAlloc);

        
        /// <summary>
        /// Computes the buffer size needed for a top level or embedded one 
        /// dimensional varying array.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="pMemory">Pointer to the array being sized.</param>
        /// <param name="pFormat">Array's format string description.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrVaryingArrayBufferSize(
            IntPtr pStubMsg, 
            IntPtr pMemory, 
            IntPtr pFormat);


        /// <summary>
        /// Marshalls a top level or embedded one dimensional varying array.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="pMemory">Pointer to the array being marshalled.</param>
        /// <param name="pFormat">Array's format string description.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrVaryingArrayMarshall(
            IntPtr pStubMsg, 
            IntPtr pMemory, 
            IntPtr pFormat);


        /// <summary>
        /// Unmarshalls top level or embedded a one dimensional varying array.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="ppMemory">Array being unmarshalled.</param>
        /// <param name="pFormat">Array's format string description.</param>
        /// <param name="fMustAlloc">Ignored.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrVaryingArrayUnmarshall(
            IntPtr pStubMsg,
            ref IntPtr ppMemory,
            IntPtr pFormat,
            byte fMustAlloc);


        /// <summary>
        /// Computes the buffer size needed for a top level complex array.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="pMemory">Pointer to the array being sized.</param>
        /// <param name="pFormat">Array's format string description.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrComplexArrayBufferSize(
            IntPtr pStubMsg, 
            IntPtr pMemory, 
            IntPtr pFormat);


        /// <summary>
        /// Marshalls a top level complex array.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="pMemory">Pointer to the array being marshalled.</param>
        /// <param name="pFormat">Array's format string description.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrComplexArrayMarshall(
            IntPtr pStubMsg, 
            IntPtr pMemory, 
            IntPtr pFormat);


        /// <summary>
        /// Unmarshalls a top level complex array.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="ppMemory">Pointer to the array being unmarshalled.</param>
        /// <param name="pFormat">Array's format string description.</param>
        /// <param name="fMustAlloc">Ignored.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrComplexArrayUnmarshall(
            IntPtr pStubMsg,
            ref IntPtr ppMemory,
            IntPtr pFormat,
            byte fMustAlloc);


        /// <summary>
        /// Computes the buffer size needed for a non conformant string.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="pMemory">Pointer to the array being sized.</param>
        /// <param name="pFormat">Array's format string description.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrNonConformantStringBufferSize(
            IntPtr pStubMsg, 
            IntPtr pMemory, 
            IntPtr pFormat);


        /// <summary>
        /// Marshalls a non conformant string.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="pMemory">Pointer to the string to be marshalled.</param>
        /// <param name="pFormat">String's format string description.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrNonConformantStringMarshall(
            IntPtr pStubMsg, 
            IntPtr pMemory, 
            IntPtr pFormat);


        /// <summary>
        /// Unmarshalls a non conformant string.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="ppMemory">Double pointer to the string should be unmarshalled.</param>
        /// <param name="pFormat">String's format string description.</param>
        /// <param name="fMustAlloc">Ignored.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrNonConformantStringUnmarshall(
            IntPtr pStubMsg,
            ref IntPtr ppMemory,
            IntPtr pFormat,
            byte fMustAlloc);


        /// <summary>
        /// Computes the buffer size needed for an encapsulated union.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="pMemory">Pointer to the union being sized.</param>
        /// <param name="pFormat">Union's format string description.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrEncapsulatedUnionBufferSize(
            IntPtr pStubMsg, 
            IntPtr pMemory, 
            IntPtr pFormat);


        /// <summary>
        /// Marshalls an encapsulated union.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="pMemory">Pointer to the union being marshalled.</param>
        /// <param name="pFormat">Union's format string description.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrEncapsulatedUnionMarshall(
            IntPtr pStubMsg, 
            IntPtr pMemory, 
            IntPtr pFormat);


        /// <summary>
        /// Unmarshalls an encapsulated union.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="ppMemory">Double pointer to where the union should be unmarshalled.</param>
        /// <param name="pFormat">Union's format string description.</param>
        /// <param name="fMustAlloc">Ignored.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrEncapsulatedUnionUnmarshall(
            IntPtr pStubMsg,
            ref IntPtr ppMemory,
            IntPtr pFormat,
            byte fMustAlloc);


        /// <summary>
        /// Computes the buffer size needed for a byte count pointer.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="pMemory">The byte count pointer being sized.</param>
        /// <param name="pFormat">Byte count pointer's format string description.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrByteCountPointerBufferSize(
            IntPtr pStubMsg, 
            IntPtr pMemory, 
            IntPtr pFormat);

        /// <summary>
        /// Marshalls a pointer with the byte count attribute applied to it.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="pMemory">Pointer to the byte count pointer being marshalled.</param>
        /// <param name="pFormat">Byte count pointer's format string description.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrByteCountPointerMarshall(
            IntPtr pStubMsg, 
            IntPtr pMemory, 
            IntPtr pFormat);


        /// <summary>
        /// Unmarshalls a pointer with the byte count attribute applied to it.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="ppMemory">Double pointer to where the byte count pointer should be unmarshalled.</param>
        /// <param name="pFormat">Byte count pointer's format string description.</param>
        /// <param name="fMustAlloc">Ignored.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrByteCountPointerUnmarshall(
            IntPtr pStubMsg,
            ref IntPtr ppMemory,
            IntPtr pFormat,
            byte fMustAlloc);
        
        
        /*/// <summary>
        /// Computes the buffer size needed for a transmit as or represent as object.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="pMemory">Pointer to the transmit/represent as object being sized.</param>
        /// <param name="pFormat">Object's format string description.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrXmitOrRepAsBufferSize(
            IntPtr pStubMsg, 
            IntPtr pMemory, 
            IntPtr pFormat);


        /// <summary>
        /// Marshalls a transmit as or represent as argument.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="pMemory">presented type translated into transmitted type and than to be marshalled.</param>
        /// <param name="pFormat">format string description</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrXmitOrRepAsMarshall(
            IntPtr pStubMsg, 
            IntPtr pMemory, 
            IntPtr pFormat);


        /// <summary>
        /// Unmarshalls a transmit as (or represent as)object.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="ppMemory">pointer to the presented type where to put data.</param>
        /// <param name="pFormat">format string description</param>
        /// <param name="fMustAlloc">allocate flag</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrXmitOrRepAsUnmarshall(
            IntPtr pStubMsg,
            ref IntPtr ppMemory,
            IntPtr pFormat,
            byte fMustAlloc);*/


        /// <summary>
        /// Computes the buffer size needed for a context handle.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="pMemory">Ignored.</param>
        /// <param name="pFormat">Ignored.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrContextHandleSize(
            IntPtr pStubMsg, 
            IntPtr pMemory, 
            IntPtr pFormat);


        /*/// <summary>
        /// Computes the buffer size needed for a user marshal object.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="pMemory">Pointer the user marshal object being sized.</param>
        /// <param name="pFormat">User marshal object's format string description.</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrUserMarshalBufferSize(
            IntPtr pStubMsg, 
            IntPtr pMemory, 
            IntPtr pFormat);


        /// <summary>
        /// Marshalls a user marshal object.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="pMemory">Pointer to the user marshal object to be marshalled.</param>
        /// <param name="pFormat">User marshal object' format string description</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrUserMarshalMarshall(
            IntPtr pStubMsg, 
            IntPtr pMemory, 
            IntPtr pFormat);


        /// <summary>
        /// Unmarshalls a user marshal object.
        /// </summary>
        /// <param name="pStubMsg">Pointer to the stub message.</param>
        /// <param name="ppMemory">Double pointer to where the user marshal object should be unmarshalled.</param>
        /// <param name="pFormat">User marshal object's format string description</param>
        /// <param name="fMustAlloc">allocate flag</param>
        [DllImport("rpcrt4.dll")]
        internal extern static void NdrUserMarshalUnmarshall(
            IntPtr pStubMsg,
            ref IntPtr ppMemory,
            IntPtr pFormat,
            byte fMustAlloc);*/

        #endregion


        #region General Windows API definition

        /// <summary>
        /// Moves a block of memory from one location to another.
        /// </summary>
        /// <param name="Destination">
        /// A pointer to the starting address of the move destination. 
        /// </param>
        /// <param name="Source">
        /// A pointer to the starting address of the block of memory to be moved.
        /// </param>
        /// <param name="Length">
        /// The size of the block of memory to move, in bytes.
        /// </param>
        [DllImport("kernel32.dll")]
        public static extern void RtlMoveMemory(IntPtr Destination, IntPtr Source, uint Length);

        #endregion

    }
}
