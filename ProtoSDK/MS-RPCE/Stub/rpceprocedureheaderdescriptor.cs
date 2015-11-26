// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// RPCE Procedure Header Descriptor. 
    /// The header has been extended several times over the life of the NDR engine. 
    /// The current compiler still generates different headers depending on the mode 
    /// of the compiler. However, more recent headers are a superset of the older ones.<para/>
    /// http://msdn.microsoft.com/en-us/library/aa374387(VS.85).aspx
    /// </summary>
    public struct RpceProcedureHeaderDescriptor
    {
        /// <summary>
        /// The handle_type can be one of the values shown in the following table.<para/>
        /// FC_BIND_GENERIC: 31<para/>
        /// FC_BIND_PRIMITIVE: 32<para/>
        /// FC_AUTO_HANDLE: 33<para/>
        /// FC_CALLBACK_HANDLE: 34<para/>
        /// explicit handle: 0
        /// </summary>
        public byte HandleType;

        /// <summary>
        /// The Oi_flags field is an 8-bit mask of the following flags.<para/>
        /// Oi_FULL_PTR_USED: 0x01: Uses the full pointer package.<para/>
        /// Oi_RPCSS_ALLOC_USED: 0x02 : Uses the RpcSs memory package.<para/>
        /// Oi_OBJECT_PROC: 0x04: A procedure in an object interface.<para/>
        /// Oi_HAS_RPCFLAGS: 0x08: The procedure has nonzero Rpc flags.<para/>
        /// ENCODE_IS_USED: 0x10: Overloaded, Used only in pickling.<para/>
        /// DECODE_IS_USED: 0x20: Overloaded, Used only in pickling.<para/>
        /// Oi_IGNORE_OBJECT_EXCEPTION_HANDLING: 0x10: Overloaded, Not used anymore (old OLE).<para/>
        /// Oi_HAS_COMM_OR_FAULT: 0x20: Overloaded, In raw RPC only, [comm _, fault_status].<para/>
        /// Oi_OBJ_USE_V2_INTERPRETER: 0x20: overloaded, In DCOM only, use -Oif interpreter.<para/>
        /// Oi_USE_NEW_INIT_ROUTINES: 0x40: Uses Windows NT3.5 Beta2+ init routines.<para/>
        /// 0x80: Unused.
        /// </summary>
        public byte OiFlags;

        /// <summary>
        /// The rpc_flags field describes how to set the RpcFlags field of the RPC_MESSAGE structure. 
        /// This field is only present if the Oi_flags field has Oi_HAD_RPCFLAGS set. 
        /// If this field is not present, then the RPC flags for the remote procedure are zero.
        /// </summary>
        public uint? RpcFlags;

        /// <summary>
        /// The proc_num field provides the procedure's procedure number.
        /// </summary>
        public ushort ProcNum;

        /// <summary>
        /// The stack_size provides the total size of all parameters on the stack, 
        /// including any this pointer and/or return value.
        /// </summary>
        public ushort StackSize;

        /// <summary>
        /// The explicit_handle_description field is described later in this document. 
        /// This field is not present if the procedure uses an implicit handle.
        /// </summary>
        public byte[] ExplicitHandleDescription;

        /// <summary>
        /// constant_client_buffer_size
        /// </summary>
        public ushort ClientBufferSize;

        /// <summary>
        /// constant_server_buffer_size
        /// </summary>
        public ushort ServerBufferSize;

        /// <summary>
        /// INTERPRETER_OPT_FLAGS
        /// </summary>
        public byte InterpreterOptFlags;

        /// <summary>
        /// number_of_params
        /// </summary>
        public byte NumberOfParams;

        /// <summary>
        /// size of the extension
        /// </summary>
        public byte? ExtensionSize;

        /// <summary>
        /// INTERPRETER_OPT_FLAGS2 in extension
        /// </summary>
        public RpceInterpreterOptFlags2? ExtensionFlags2;

        /// <summary>
        /// hint of correlation check at client-side in extension
        /// </summary>
        public ushort? ExtensionClientCorrHint;

        /// <summary>
        /// hint of correlation check at server-side in extension
        /// </summary>
        public ushort? ExtensionServerCorrHint;

        /// <summary>
        /// notify index in extension
        /// </summary>
        public ushort? ExtensionNotifyIndex;
    }
}
