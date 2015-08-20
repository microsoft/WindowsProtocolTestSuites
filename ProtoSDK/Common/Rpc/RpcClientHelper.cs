// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// RpcClientHelper provides common operations for RPC client role. 
    /// </summary>
    public static class RpcClientHelper
    {
        /// <summary>
        /// Use the input parameter of rpcClientConfig to bind a RPC handle and set its authentication and
        /// authorization information.
        /// The caller is responded for calling Unbind to free the output handle.
        /// The following default values will be used when PInvoke RPC binding API:
        /// RpcStringBindingCompose: ObjUuid is set to null, Options is set to null.
        /// </summary>
        /// <param name="rpcClientConfig">Contains the parameters used by WinAPI for RPC binding.</param>
        /// <returns>the RPC binding handle.</returns>
        /// <exception cref="System.InvalidOperationException">Fail to PInvoke.</exception>
        public static IntPtr Bind(RpcClientConfig rpcClientConfig)
        {
            IntPtr bindingHandle = IntPtr.Zero;
            IntPtr bindingString = IntPtr.Zero;

            uint status = RpcNativeMethods.RpcStringBindingCompose(
                (rpcClientConfig.ClientGuid == null) ? null : rpcClientConfig.ClientGuid.Value.ToString(),
                rpcClientConfig.ProtocolSequence,
                rpcClientConfig.ServerComputerName,
                rpcClientConfig.EndPoint,
                rpcClientConfig.NetworkOptions,
                out bindingString);
            if (status != 0)
            {
                throw new InvalidOperationException("Failed to RpcStringBindingCompose. The returned status is "
                    + status);
            }

            status = RpcNativeMethods.RpcBindingFromStringBinding(
                bindingString,
                out bindingHandle);
            if (status != 0)
            {
                throw new InvalidOperationException("Failed to RpcBindingFromStringBinding. The returned status is "
                    + status);
            }

            status = RpcNativeMethods.RpcStringFree(
                ref bindingString);
            if (status != 0)
            {
                throw new InvalidOperationException("Failed to RpcStringFree. The returned status is " + status);
            }

            SEC_WINNT_AUTH_IDENTITY authenticationIdentity = rpcClientConfig.AuthenticationIdentity;
            status = RpcNativeMethods.RpcBindingSetAuthInfo(
                bindingHandle,
                rpcClientConfig.ServicePrincipalName,
                rpcClientConfig.AuthenticationLevel,
                rpcClientConfig.AuthenticationService,
                ref authenticationIdentity,
                rpcClientConfig.AuthorizationService);
            if (status != 0)
            {
                throw new InvalidOperationException("Failed to RpcBindingSetAuthInfo. The returned status is "
                    + status);
            }

            return bindingHandle;
        }


        /// <summary>
        /// To free an RPC binding handle.
        /// </summary>
        /// <param name="hBind">the RPC binding handle to free.</param>
        /// <exception cref="System.InvalidOperationException">Fail to PInvoke RpcBindingFree.</exception>
        public static void Unbind(ref IntPtr hBind)
        {
            uint status = RpcNativeMethods.RpcBindingFree(ref hBind);
            if (status != 0)
            {
                throw new InvalidOperationException("Failed to RpcBindingFree. The returned status is "
                    + status);
            }
        }
    }
}
