// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// Bind only server, it is used to negotiate NDR with stubEncoder.
    /// </summary>
    internal class RpceBindOnlyServer : IDisposable
    {
        // IF_UUID, vers_major and vers_minor of internal RPC interface. it's random.
        internal readonly Guid IF_ID = Guid.NewGuid();
        internal readonly ushort IF_VERS_MAJOR = 1;
        internal readonly ushort IF_VERS_MINOR = 0;

        
        // RPC interface handle
        private IntPtr pRpcIfHandle;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="localRpcEndpoint">A local RPC endpoint.</param>
        [SuppressMessage("Microsoft.Performance", "CA1805:DoNotInitializeUnnecessarily")]
        internal RpceBindOnlyServer(string localRpcEndpoint)
        {
            NativeMethods.RPC_SERVER_INTERFACE rpcServerInterface = new NativeMethods.RPC_SERVER_INTERFACE();
            rpcServerInterface.Length = (uint)Marshal.SizeOf(typeof(NativeMethods.RPC_SERVER_INTERFACE));
            rpcServerInterface.InterfaceId = new NativeMethods.RPC_SYNTAX_IDENTIFIER();
            rpcServerInterface.InterfaceId.SyntaxGUID = IF_ID;
            rpcServerInterface.InterfaceId.SyntaxVersion.MajorVersion = IF_VERS_MAJOR;
            rpcServerInterface.InterfaceId.SyntaxVersion.MinorVersion = IF_VERS_MINOR;
            rpcServerInterface.TransferSyntax = new NativeMethods.RPC_SYNTAX_IDENTIFIER();
            rpcServerInterface.TransferSyntax.SyntaxGUID = RpceUtility.NDR_INTERFACE_UUID;
            rpcServerInterface.TransferSyntax.SyntaxVersion.MajorVersion = RpceUtility.NDR_INTERFACE_MAJOR_VERSION;
            rpcServerInterface.TransferSyntax.SyntaxVersion.MinorVersion = RpceUtility.NDR_INTERFACE_MINOR_VERSION;
            rpcServerInterface.DispatchTable = IntPtr.Zero;
            rpcServerInterface.RpcProtseqEndpointCount = 0;
            rpcServerInterface.RpcProtseqEndpoint = IntPtr.Zero;
            rpcServerInterface.DefaultManagerEpv = IntPtr.Zero;
            rpcServerInterface.InterpreterInfo = IntPtr.Zero;
            rpcServerInterface.Flags = 0;

            pRpcIfHandle = Marshal.AllocHGlobal(Marshal.SizeOf(rpcServerInterface));
            Marshal.StructureToPtr(rpcServerInterface, pRpcIfHandle, false);

            int status;

            status = NativeMethods.RpcServerUseProtseqEp(
                "ncalrpc",
                NativeMethods.RPC_C_PROTSEQ_MAX_REQS_DEFAULT,
                localRpcEndpoint,
                IntPtr.Zero);
            if (status != NativeMethods.RPC_S_OK)
            {
                throw new InvalidOperationException(
                    string.Format("RpcServerUseProtseqEp failed with error code {0}.", status));
            }

            status = NativeMethods.RpcServerRegisterIf(
                pRpcIfHandle,
                IntPtr.Zero,
                IntPtr.Zero);
            if (status != NativeMethods.RPC_S_OK
                && status != NativeMethods.RPC_S_ALREADY_REGISTERED)
            {
                throw new InvalidOperationException(
                    string.Format("RpcServerRegisterIf failed with error code {0}.", status));
            }

            const uint MIN_CALL_THREADS = 1;
            const uint DONT_WAIT = 1; // TRUE

            status = NativeMethods.RpcServerListen(
                MIN_CALL_THREADS,
                NativeMethods.RPC_C_LISTEN_MAX_CALLS_DEFAULT,
                DONT_WAIT);
            if (status != NativeMethods.RPC_S_OK 
                && status != NativeMethods.RPC_S_ALREADY_LISTENING)
            {
                throw new InvalidOperationException(
                    string.Format("RpcServerListen failed with error code {0}.", status));
            }
        }

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
        //This is Dispose method, just ignore method return results.
        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults")]
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Release managed resources.
            }

            // Release unmanaged resources.
            NativeMethods.RpcMgmtStopServerListening(IntPtr.Zero);

            if (pRpcIfHandle != IntPtr.Zero)
            {
                const uint WAIT_FOR_CALLS_TO_COMPLETE = 1; // TRUE

                NativeMethods.RpcServerUnregisterIf(
                    pRpcIfHandle, 
                    IntPtr.Zero,
                    WAIT_FOR_CALLS_TO_COMPLETE);

                Marshal.FreeHGlobal(pRpcIfHandle);
                pRpcIfHandle = IntPtr.Zero;
            }
        }


        /// <summary>
        /// finalizer
        /// </summary>
        ~RpceBindOnlyServer()
        {
            Dispose(false);
        }

        #endregion

    }
}
