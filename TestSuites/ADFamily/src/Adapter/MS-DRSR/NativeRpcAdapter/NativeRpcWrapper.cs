// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.Messages;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    #region Unmanaged Structure Wrapper
    /// <summary>
    /// SEC_WINNT_AUTH_IDENTITY structure contains client credentials
    /// </summary>
    public struct SEC_WINNT_AUTH_IDENTITY
    {
        /// <summary>
        /// The user
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string User;

        /// <summary>
        /// The length of user
        /// </summary>
        public uint UserLength;

        /// <summary>
        /// The domain of client
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Domain;

        /// <summary>
        /// The domain 's length
        /// </summary>
        public uint DomainLength;

        /// <summary>
        /// The password of client
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string StringPassword;

        /// <summary>
        /// The length of password
        /// </summary>
        public uint PasswordLength;

        /// <summary>
        /// The flags of client
        /// </summary>
        public uint Flags;
    }

    /// <summary>
    /// RPC_SECURITY_QOS structure
    /// </summary>
    public struct RPC_SECURITY_QOS
    {
        /// <summary>
        /// Version of QOS
        /// </summary>
        public uint Version;

        /// <summary>
        /// The Capabilities
        /// </summary>
        public uint Capabilities;

        /// <summary>
        /// Identity Tracking
        /// </summary>
        public uint IdentityTracking;

        /// <summary>
        /// Impersonation type
        /// </summary>
        public uint ImpersonationType;
    }
    #endregion

    static class UnmanagedRpcMethods
    {
        // Unmanaged entries
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA2101:SpecifyMarshalingForPInvokeStringArguments", MessageId = "0"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA2101:SpecifyMarshalingForPInvokeStringArguments", MessageId = "4"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA2101:SpecifyMarshalingForPInvokeStringArguments", MessageId = "3"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA2101:SpecifyMarshalingForPInvokeStringArguments", MessageId = "2"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA2101:SpecifyMarshalingForPInvokeStringArguments", MessageId = "1"), DllImport("rpcrt4.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern uint RpcStringBindingCompose(
            [MarshalAs(UnmanagedType.LPTStr)] string objUuid,
            [MarshalAs(UnmanagedType.LPTStr)] string protSeq,
            [MarshalAs(UnmanagedType.LPTStr)] string networkAddr,
            [MarshalAs(UnmanagedType.LPTStr)] string endPoint,
            [MarshalAs(UnmanagedType.LPTStr)] string Options,
            ref IntPtr stringBinding);

        // Gets the binding handle from the string binding
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA2101:SpecifyMarshalingForPInvokeStringArguments", MessageId = "0"), DllImport("rpcrt4.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern uint RpcBindingFromStringBinding(
            [MarshalAs(UnmanagedType.LPTStr)] string stringBinding,
            ref IntPtr Binding);

        [DllImport("rpcrt4.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern uint RpcBindingSetAuthInfoEx(
            IntPtr Binding,
            [MarshalAs(UnmanagedType.LPWStr)]string ServerPrincName,
            uint AuthnLevel,
            uint AuthnSvc,
            ref SEC_WINNT_AUTH_IDENTITY AuthIdentity,
            uint AuthzService,
            ref RPC_SECURITY_QOS SecurityQOS);

        // Free Binding Handle
        [DllImport("rpcrt4.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern uint RpcBindingFree(ref IntPtr Binding);

        /// <summary>
        /// The RpcBindingToStringBinding function returns a string representation of a binding handle.
        /// </summary>
        /// <param name="Binding">Client or server binding handle to convert to 
        /// a string representation of a binding handle.</param>
        /// <param name="strBinding">A pointer to the string representation of 
        /// the binding handle specified in the Binding parameter.</param>
        /// <returns>Rpc status</returns>
        [DllImport("rpcrt4.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern uint RpcBindingToStringBinding(
            IntPtr Binding,
            ref string strBinding);
    }

    static class RpcNativeMethods
    {
        /// <summary>
        /// Bind and compose rpc
        /// </summary>
        /// <param name="objUuid">The object UUID that RpcMgmtEpEltInqNext looks for in endpoint-map elements.</param>
        /// <param name="protSeq">The prot seq</param>
        /// <param name="networkAddr">The address of network</param>
        /// <param name="endPoint">The end point</param>
        /// <param name="options">The repsForm attribute</param>
        /// <param name="stringBinding">A pointer to the string representation of 
        /// the binding handle specified in the Binding parameter.</param>
        /// <returns> Rpc status</returns>
        public static uint RpcStringBindingCompose(
            string objUuid,
            string protSeq,
            string networkAddr,
            string endPoint,
            string options,
            ref IntPtr stringBinding)
        {
            return UnmanagedRpcMethods.RpcStringBindingCompose(
                objUuid,
                protSeq,
                networkAddr,
                endPoint,
                options,
                ref stringBinding);
        }

        /// <summary>
        /// Gets the binding handle from the string binding
        /// </summary>
        /// <param name="stringBinding">A pointer to the string representation of 
        /// the binding handle specified in the Binding parameter.</param>
        /// <param name="binding">Binding handle to a host whose endpoint-map elements is to be viewed</param>
        /// <returns>Rpc status</returns>
        public static uint RpcBindingFromStringBinding(string stringBinding, ref IntPtr binding)
        {
            return UnmanagedRpcMethods.RpcBindingFromStringBinding(stringBinding, ref binding);
        }

        /// <summary>
        /// Set the binding information
        /// </summary>
        /// <param name="binding">Binding handle to a host whose endpoint-map elements is to be viewed</param>
        /// <param name="serverPrincName">The server principal name</param>
        /// <param name="authnLevel">The auth level</param>
        /// <param name="authnSvc">The auth svc</param>
        /// <param name="authIdentity">The auth identity</param>
        /// <param name="authzService">The auth service</param>
        /// <param name="securityQOS">The security qos</param>
        /// <returns>Rpc status</returns>
        public static uint RpcBindingSetAuthInfoEx(
            IntPtr binding,
            string serverPrincName,
            uint authnLevel,
            uint authnSvc,
            ref SEC_WINNT_AUTH_IDENTITY authIdentity,
            uint authzService,
            ref RPC_SECURITY_QOS securityQOS)
        {
            return UnmanagedRpcMethods.RpcBindingSetAuthInfoEx(
                binding,
                serverPrincName,
                authnLevel,
                authnSvc,
                ref authIdentity,
                authzService,
                ref securityQOS);
        }


        /// <summary>
        /// Free Binding Handle
        /// </summary>
        /// <param name="binding">Binding handle to a host whose endpoint-map elements is to be viewed</param>
        /// <returns>Rpc status</returns>
        public static uint RpcBindingFree(ref IntPtr binding)
        {
            return UnmanagedRpcMethods.RpcBindingFree(ref binding);
        }

        /// <summary>
        /// The RpcBindingToStringBinding function returns a string representation of a binding handle.
        /// </summary>
        /// <param name="binding">Client or server binding handle to convert to 
        /// a string representation of a binding handle.</param>
        /// <param name="strBinding">A pointer to the string representation of 
        /// the binding handle specified in the Binding parameter.</param>
        /// <returns>Rpc status</returns>
        public static uint RpcBindingToStringBinding(IntPtr binding, ref string strBinding)
        {
            return UnmanagedRpcMethods.RpcBindingToStringBinding(binding, ref strBinding);
        }

    }

    public class NativeRpcWrapper : IDrsuapiRpcAdapter, IDisposable
    {
        // Copy structure obj of type t1 to t2
        public static object CopyFieldsFrom(Type t1, Type t2, object obj)
        {
            object ret = null;
            if (obj == null)
                return ret;

            if (obj is Array)
            {
                int l = ((Array)obj).Length;
                ret = Array.CreateInstance(t2.GetElementType(), l);
                for (int i = 0; i < l; ++i)
                {
                    (ret as Array).SetValue(CopyFieldsFrom(t1.GetElementType(), t2.GetElementType(), (obj as Array).GetValue(i)), i);
                }
                return ret;
            }
            else
            {
                ret = FormatterServices.GetUninitializedObject(t2);
            }

            System.Reflection.FieldInfo[] field1 = null;
            System.Reflection.FieldInfo[] field2 = null;

            if (t1.IsGenericType && t1.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                field1 = Nullable.GetUnderlyingType(t1).GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                field2 = Nullable.GetUnderlyingType(t2).GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            }
            else
            {
                field1 = t1.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                field2 = t2.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            }


            foreach (System.Reflection.FieldInfo item1 in field1)
            {
                foreach (System.Reflection.FieldInfo item2 in field2)
                {
                    if (item1.Name == item2.Name)
                    {
                        Type tt1 = item1.FieldType;
                        Type tt2 = item2.FieldType;
                        object v;
                        if (tt1 != tt2)
                        {
                            object h = item1.GetValue(obj);
                            v = CopyFieldsFrom(tt1, tt2, h);
                        }
                        else
                        {
                            v = item1.GetValue(obj);
                        }
                        item2.SetValue(ret, v);
                    }
                }
            }

            return ret;
        }


        IntPtr bindingHandle = IntPtr.Zero;

        IMS_DRSR_RpcAdapter nativeRpcAdapter = null;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// RPC handle.
        /// </summary>
        public IntPtr Handle
        {
            get { return bindingHandle; }
        }

        public IMS_DRSR_RpcAdapter NativeRpcAdapter
        {
            set { nativeRpcAdapter = value; }
            get { return nativeRpcAdapter; }
        }

        [CLSCompliant(false)]
        public void Bind(
            string protocolSequence,
            string networkAddress,
            string endpoint,
            ClientSecurityContext securityContext,
            RpceAuthenticationLevel authenticationLevel,
            TimeSpan timeout)
        {
            // do nothing.
        }

        [CLSCompliant(false)]
        public bool Bind(string server, string domain, string userName, string password, string spn)
        {
            IntPtr stringBinding = IntPtr.Zero;

            uint status = RpcNativeMethods.RpcStringBindingCompose(null, "ncacn_ip_tcp", server, null, null, ref stringBinding);
            if (status != 0)
                return false;

            string strBinding = Marshal.PtrToStringAuto(stringBinding);
            status = RpcNativeMethods.RpcBindingFromStringBinding(strBinding, ref bindingHandle);
            if (status != 0)
                return false;

            SEC_WINNT_AUTH_IDENTITY secAuthIdentity = new SEC_WINNT_AUTH_IDENTITY();
            secAuthIdentity.Domain = domain;
            secAuthIdentity.User = userName;
            secAuthIdentity.StringPassword = password;
            secAuthIdentity.DomainLength = (uint)secAuthIdentity.Domain.Length;
            secAuthIdentity.UserLength = (uint)secAuthIdentity.User.Length;
            secAuthIdentity.PasswordLength = (uint)secAuthIdentity.StringPassword.Length;
            secAuthIdentity.Flags = 2;

            // Construct the struct RPC_SECURITY_QOS used for 
            // security quauty-of-service settings on the rpc binging handle.
            RPC_SECURITY_QOS rpcsecQOS = new RPC_SECURITY_QOS();
            rpcsecQOS.Version = 1;
            rpcsecQOS.Capabilities = 9;
            rpcsecQOS.IdentityTracking = 0;
            rpcsecQOS.ImpersonationType = 2;

            // authn_level
            uint authn_level = 6;
            uint authn_svc = 9;
            status = RpcNativeMethods.RpcBindingSetAuthInfoEx(
                bindingHandle,
                spn,
                authn_level,
                authn_svc,
                ref secAuthIdentity,
                authn_svc,
                ref rpcsecQOS);

            if (0 != status)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// RPC unbind.
        /// </summary>
        public void Unbind()
        {
            // do nothing;
        }

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
        [CLSCompliant(false)]
        public uint IDL_DRSBind(
            IntPtr rpc_handle,
            Guid? puuidClientDsa,
            DRS_EXTENSIONS? pextClient,
            out DRS_EXTENSIONS_INT? ppextServer,
            out IntPtr? phDrs)
        {
            NativeTypes.DRS_EXTENSIONS _pextClient = (NativeTypes.DRS_EXTENSIONS)CopyFieldsFrom(
                typeof(DRS_EXTENSIONS),
                typeof(NativeTypes.DRS_EXTENSIONS),
                pextClient.Value);

            NativeTypes.DRS_EXTENSIONS_INT? _ppextServer;
            uint ret = NativeRpcAdapter.IDL_DRSBind(
                    rpc_handle,
                    puuidClientDsa.Value,
                    _pextClient,
                    out _ppextServer,
                    out phDrs);

            ppextServer = (DRS_EXTENSIONS_INT?)CopyFieldsFrom(
                typeof(NativeTypes.DRS_EXTENSIONS_INT),
                typeof(DRS_EXTENSIONS_INT),
                _ppextServer.Value);

            return ret;
        }


        /// <summary>
        ///  The IDL_DRSUnbind method destroys a context handle previously
        ///  created by the IDL_DRSBind method. Opnum: 1 
        /// </summary>
        /// <param name="phDrs">
        ///  A pointer to the RPC context handle returned by the
        ///  IDL_DRSBind method. The value is set to null on return.
        /// </param>
        [CLSCompliant(false)]
        public uint IDL_DRSUnbind(ref IntPtr? phDrs)
        {
            return NativeRpcAdapter.IDL_DRSUnbind(ref phDrs);
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
        [CLSCompliant(false)]
        public uint IDL_DRSReplicaSync(
            IntPtr hDrs,
            uint dwVersion,
            //[Switch("dwVersion")]
            DRS_MSG_REPSYNC? pmsgSync)
        {
            NativeTypes.DRS_MSG_REPSYNC _pmsgIn =
                (NativeTypes.DRS_MSG_REPSYNC)CopyFieldsFrom(
                typeof(DRS_MSG_REPSYNC),
                typeof(NativeTypes.DRS_MSG_REPSYNC),
                pmsgSync.Value);

            uint ret = NativeRpcAdapter.IDL_DRSReplicaSync(
                hDrs,
                dwVersion,
                _pmsgIn);

            return ret;
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
        [CLSCompliant(false)]
        public uint IDL_DRSUpdateRefs(
            IntPtr hDrs,
            uint dwVersion,
            //[Switch("dwVersion")] 
            DRS_MSG_UPDREFS? pmsgUpdRefs)
        {
            NativeTypes.DRS_MSG_UPDREFS _pmsgIn =
                (NativeTypes.DRS_MSG_UPDREFS)CopyFieldsFrom(
                typeof(DRS_MSG_UPDREFS),
                typeof(NativeTypes.DRS_MSG_UPDREFS),
                pmsgUpdRefs.Value);

            uint ret = NativeRpcAdapter.IDL_DRSUpdateRefs(
                hDrs,
                dwVersion,
                _pmsgIn);

            return ret;
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
        [CLSCompliant(false)]
        public uint IDL_DRSReplicaAdd(
            IntPtr hDrs,
            uint dwVersion,
            //[Switch("dwVersion")]
            DRS_MSG_REPADD? pmsgAdd)
        {
            NativeTypes.DRS_MSG_REPADD _pmsgIn =
                (NativeTypes.DRS_MSG_REPADD)CopyFieldsFrom(
                typeof(DRS_MSG_REPADD),
                typeof(NativeTypes.DRS_MSG_REPADD),
                pmsgAdd.Value);

            uint ret = NativeRpcAdapter.IDL_DRSReplicaAdd(
                hDrs,
                dwVersion,
                _pmsgIn);

            return ret;
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
        [CLSCompliant(false)]
        public uint IDL_DRSReplicaDel(
            IntPtr hDrs,
            uint dwVersion,
            //[Switch("dwVersion")] 
            DRS_MSG_REPDEL? pmsgDel)
        {
            NativeTypes.DRS_MSG_REPDEL _pmsgIn =
                (NativeTypes.DRS_MSG_REPDEL)CopyFieldsFrom(
                typeof(DRS_MSG_REPDEL),
                typeof(NativeTypes.DRS_MSG_REPDEL),
                pmsgDel.Value);

            uint ret = NativeRpcAdapter.IDL_DRSReplicaDel(
                hDrs,
                dwVersion,
                _pmsgIn);

            return ret;
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
        [CLSCompliant(false)]
        public uint IDL_DRSReplicaModify(
            IntPtr hDrs,
            uint dwVersion,
            //[Switch("dwVersion")] 
            DRS_MSG_REPMOD? pmsgMod)
        {
            NativeTypes.DRS_MSG_REPMOD _pmsgIn =
                (NativeTypes.DRS_MSG_REPMOD)CopyFieldsFrom(
                typeof(DRS_MSG_REPMOD),
                typeof(NativeTypes.DRS_MSG_REPMOD),
                pmsgMod.Value);

            uint ret = NativeRpcAdapter.IDL_DRSReplicaModify(
                hDrs,
                dwVersion,
                _pmsgIn);

            return ret;
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
        [CLSCompliant(false)]
        public uint IDL_DRSCrackNames(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")] 
            DRS_MSG_CRACKREQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")] 
            out DRS_MSG_CRACKREPLY? pmsgOut)
        {
            NativeTypes.DRS_MSG_CRACKREQ _pmsgIn =
                (NativeTypes.DRS_MSG_CRACKREQ)CopyFieldsFrom(
                typeof(DRS_MSG_CRACKREQ),
                typeof(NativeTypes.DRS_MSG_CRACKREQ),
                pmsgIn.Value);

            NativeTypes.DRS_MSG_CRACKREPLY? _pmsgOut;
            uint ret = NativeRpcAdapter.IDL_DRSCrackNames(
                hDrs,
                dwInVersion,
                _pmsgIn,
                out pdwOutVersion,
                out _pmsgOut);

            pmsgOut = (DRS_MSG_CRACKREPLY?)CopyFieldsFrom(
                typeof(NativeTypes.DRS_MSG_CRACKREPLY),
                typeof(DRS_MSG_CRACKREPLY),
                _pmsgOut.Value);

            return ret;
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
        [CLSCompliant(false)]
        public uint IDL_DRSWriteSPN(
            IntPtr hDrs,
            dwInVersion_Values dwInVersion,
            //[Switch("dwInVersion")]
            DRS_MSG_SPNREQ? pmsgIn,
            out pdwOutVersion_Values? pdwOutVersion,
            //[Switch("*pdwOutVersion")] 
            out DRS_MSG_SPNREPLY? pmsgOut)
        {

            NativeTypes.DRS_MSG_SPNREQ _pmsgIn =
                (NativeTypes.DRS_MSG_SPNREQ)CopyFieldsFrom(
                typeof(DRS_MSG_SPNREQ),
                typeof(NativeTypes.DRS_MSG_SPNREQ),
                pmsgIn.Value);
            NativeTypes.pdwOutVersion_Values? outVersion = 0;
            NativeTypes.DRS_MSG_SPNREPLY? _pmsgOut;
            uint ret = NativeRpcAdapter.IDL_DRSWriteSPN(
                hDrs,
                (NativeTypes.dwInVersion_Values)dwInVersion,
                _pmsgIn,
                out outVersion,
                out _pmsgOut);

            pmsgOut = (DRS_MSG_SPNREPLY?)CopyFieldsFrom(
                typeof(NativeTypes.DRS_MSG_SPNREPLY),
                typeof(DRS_MSG_SPNREPLY),
                _pmsgOut.Value);
            pdwOutVersion = (pdwOutVersion_Values)outVersion.Value;
            return ret;
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
            //[Switch("dwInVersion")] 
            DRS_MSG_RMSVRREQ? pmsgIn,
            out IDL_DRSRemoveDsServer_pdwOutVersion_Values? pdwOutVersion,
            //[Switch("*pdwOutVersion")] 
            out DRS_MSG_RMSVRREPLY? pmsgOut)
        {

            NativeTypes.DRS_MSG_RMSVRREQ _pmsgIn =
                (NativeTypes.DRS_MSG_RMSVRREQ)CopyFieldsFrom(
                typeof(DRS_MSG_RMSVRREQ),
                typeof(NativeTypes.DRS_MSG_RMSVRREQ),
                pmsgIn.Value);
            uint? outVersion = 0;
            NativeTypes.DRS_MSG_RMSVRREPLY? _pmsgOut;
            uint ret = NativeRpcAdapter.IDL_DRSRemoveDsServer(
                hDrs,
                (uint)dwInVersion,
                _pmsgIn,
                out outVersion,
                out _pmsgOut);

            pmsgOut = (DRS_MSG_RMSVRREPLY?)CopyFieldsFrom(
                typeof(NativeTypes.DRS_MSG_RMSVRREPLY),
                typeof(DRS_MSG_RMSVRREPLY),
                _pmsgOut.Value);
            pdwOutVersion = (IDL_DRSRemoveDsServer_pdwOutVersion_Values)outVersion.Value;
            return ret;
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
        [CLSCompliant(false)]
        public uint IDL_DRSRemoveDsDomain(
            IntPtr hDrs,
            IDL_DRSRemoveDsDomain_dwInVersion_Values dwInVersion,
            //[Switch("dwInVersion")] 
            DRS_MSG_RMDMNREQ? pmsgIn,
            out IDL_DRSRemoveDsDomain_pdwOutVersion_Values? pdwOutVersion,
            //[Switch("*pdwOutVersion")] 
            out DRS_MSG_RMDMNREPLY? pmsgOut)
        {

            NativeTypes.DRS_MSG_RMDMNREQ _pmsgIn =
                (NativeTypes.DRS_MSG_RMDMNREQ)CopyFieldsFrom(
                typeof(DRS_MSG_RMDMNREQ),
                typeof(NativeTypes.DRS_MSG_RMDMNREQ),
                pmsgIn.Value);
            uint? outVersion = 0;
            NativeTypes.DRS_MSG_RMDMNREPLY? _pmsgOut;
            uint ret = NativeRpcAdapter.IDL_DRSRemoveDsDomain(
                hDrs,
                (uint)dwInVersion,
                _pmsgIn,
                out outVersion,
                out _pmsgOut);

            pmsgOut = (DRS_MSG_RMDMNREPLY?)CopyFieldsFrom(
                typeof(NativeTypes.DRS_MSG_RMDMNREPLY),
                typeof(DRS_MSG_RMDMNREPLY),
                _pmsgOut.Value);
            pdwOutVersion = (IDL_DRSRemoveDsDomain_pdwOutVersion_Values)outVersion.Value;
            return ret;
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
        [CLSCompliant(false)]
        public uint IDL_DRSDomainControllerInfo(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")]
            DRS_MSG_DCINFOREQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")]
            out DRS_MSG_DCINFOREPLY? pmsgOut)
        {
            NativeTypes.DRS_MSG_DCINFOREQ _pmsgIn =
                (NativeTypes.DRS_MSG_DCINFOREQ)CopyFieldsFrom(
                typeof(DRS_MSG_DCINFOREQ),
                typeof(NativeTypes.DRS_MSG_DCINFOREQ),
                pmsgIn.Value);

            NativeTypes.DRS_MSG_DCINFOREPLY? _pmsgOut;
            uint ret = NativeRpcAdapter.IDL_DRSDomainControllerInfo(
                hDrs,
                dwInVersion,
                _pmsgIn,
                out pdwOutVersion,
                out _pmsgOut);

            pmsgOut = (DRS_MSG_DCINFOREPLY?)CopyFieldsFrom(
                typeof(NativeTypes.DRS_MSG_DCINFOREPLY),
                typeof(DRS_MSG_DCINFOREPLY),
                _pmsgOut.Value);

            return ret;
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
        [CLSCompliant(false)]
        public uint IDL_DRSExecuteKCC(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")]
            DRS_MSG_KCC_EXECUTE? pmsgIn)
        {
            NativeTypes.DRS_MSG_KCC_EXECUTE _pmsgIn =
                (NativeTypes.DRS_MSG_KCC_EXECUTE)CopyFieldsFrom(
                typeof(DRS_MSG_KCC_EXECUTE),
                typeof(NativeTypes.DRS_MSG_KCC_EXECUTE),
                pmsgIn.Value);

            uint ret = NativeRpcAdapter.IDL_DRSExecuteKCC(
                hDrs,
                dwInVersion,
                _pmsgIn);

            return ret;
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
        [CLSCompliant(false)]
        public uint IDL_DRSGetReplInfo(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")] 
            DRS_MSG_GETREPLINFO_REQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")]
            out DRS_MSG_GETREPLINFO_REPLY? pmsgOut)
        {
            NativeTypes.DRS_MSG_GETREPLINFO_REQ _pmsgIn =
                (NativeTypes.DRS_MSG_GETREPLINFO_REQ)CopyFieldsFrom(
                typeof(DRS_MSG_GETREPLINFO_REQ),
                typeof(NativeTypes.DRS_MSG_GETREPLINFO_REQ),
                pmsgIn.Value);

            NativeTypes.DRS_MSG_GETREPLINFO_REPLY? _pmsgOut;
            uint ret = NativeRpcAdapter.IDL_DRSGetReplInfo(
                hDrs,
                dwInVersion,
                _pmsgIn,
                out pdwOutVersion,
                out _pmsgOut);

            pmsgOut = (DRS_MSG_GETREPLINFO_REPLY?)CopyFieldsFrom(
                typeof(NativeTypes.DRS_MSG_GETREPLINFO_REPLY),
                typeof(DRS_MSG_GETREPLINFO_REPLY),
                _pmsgOut.Value);

            return ret;
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
        [CLSCompliant(false)]
        public uint IDL_DRSAddSidHistory(
            IntPtr hDrs,
            IDL_DRSAddSidHistory_dwInVersion_Values dwInVersion,
            //[Switch("dwInVersion")]
            DRS_MSG_ADDSIDREQ? pmsgIn,
            out IDL_DRSAddSidHistory_pdwOutVersion_Values? pdwOutVersion,
            //[Switch("*pdwOutVersion")]
            out DRS_MSG_ADDSIDREPLY? pmsgOut)
        {

            NativeTypes.DRS_MSG_ADDSIDREQ _pmsgIn =
                (NativeTypes.DRS_MSG_ADDSIDREQ)CopyFieldsFrom(
                typeof(DRS_MSG_ADDSIDREQ),
                typeof(NativeTypes.DRS_MSG_ADDSIDREQ),
                pmsgIn.Value);
            uint? outVersion = 0;
            NativeTypes.DRS_MSG_ADDSIDREPLY? _pmsgOut;
            uint ret = NativeRpcAdapter.IDL_DRSAddSidHistory(
                hDrs,
                (uint)dwInVersion,
                _pmsgIn,
                out outVersion,
                out _pmsgOut);

            pmsgOut = (DRS_MSG_ADDSIDREPLY?)CopyFieldsFrom(
                typeof(NativeTypes.DRS_MSG_ADDSIDREPLY),
                typeof(DRS_MSG_ADDSIDREPLY),
                _pmsgOut.Value);
            pdwOutVersion = (IDL_DRSAddSidHistory_pdwOutVersion_Values)outVersion.Value;
            return ret;
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
        [CLSCompliant(false)]
        public uint IDL_DRSReplicaVerifyObjects(
            IntPtr hDrs,
            uint dwVersion,
            //[Switch("dwVersion")] 
            DRS_MSG_REPVERIFYOBJ? pmsgVerify)
        {
            NativeTypes.DRS_MSG_REPVERIFYOBJ _pmsgVerify =
                (NativeTypes.DRS_MSG_REPVERIFYOBJ)CopyFieldsFrom(
                typeof(DRS_MSG_REPVERIFYOBJ),
                typeof(NativeTypes.DRS_MSG_REPVERIFYOBJ),
                pmsgVerify.Value);

            uint ret = NativeRpcAdapter.IDL_DRSReplicaVerifyObjects(
                hDrs,
                dwVersion,
                _pmsgVerify);

            return ret;
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
        [CLSCompliant(false)]
        public uint IDL_DRSQuerySitesByCost(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")]
            DRS_MSG_QUERYSITESREQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")]
            out DRS_MSG_QUERYSITESREPLY? pmsgOut)
        {
            NativeTypes.DRS_MSG_QUERYSITESREQ _pmsgIn =
                (NativeTypes.DRS_MSG_QUERYSITESREQ)CopyFieldsFrom(
                typeof(DRS_MSG_QUERYSITESREQ),
                typeof(NativeTypes.DRS_MSG_QUERYSITESREQ),
                pmsgIn.Value);

            NativeTypes.DRS_MSG_QUERYSITESREPLY? _pmsgOut;
            uint ret = NativeRpcAdapter.IDL_DRSQuerySitesByCost(
                hDrs,
                dwInVersion,
                _pmsgIn,
                out pdwOutVersion,
                out _pmsgOut);

            pmsgOut = (DRS_MSG_QUERYSITESREPLY?)CopyFieldsFrom(
                typeof(NativeTypes.DRS_MSG_QUERYSITESREPLY),
                typeof(DRS_MSG_QUERYSITESREPLY),
                _pmsgOut.Value);

            return ret;
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
        [CLSCompliant(false)]
        public uint IDL_DRSInitDemotion(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")]
            DRS_MSG_INIT_DEMOTIONREQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")] 
            out DRS_MSG_INIT_DEMOTIONREPLY? pmsgOut)
        {
            NativeTypes.DRS_MSG_INIT_DEMOTIONREQ _pmsgIn =
                (NativeTypes.DRS_MSG_INIT_DEMOTIONREQ)CopyFieldsFrom(
                typeof(DRS_MSG_INIT_DEMOTIONREQ),
                typeof(NativeTypes.DRS_MSG_INIT_DEMOTIONREQ),
                pmsgIn.Value);

            NativeTypes.DRS_MSG_INIT_DEMOTIONREPLY? _pmsgOut;
            uint ret = NativeRpcAdapter.IDL_DRSInitDemotion(
                hDrs,
                dwInVersion,
                _pmsgIn,
                out pdwOutVersion,
                out _pmsgOut);

            pmsgOut = (DRS_MSG_INIT_DEMOTIONREPLY?)CopyFieldsFrom(
                typeof(NativeTypes.DRS_MSG_INIT_DEMOTIONREPLY),
                typeof(DRS_MSG_INIT_DEMOTIONREPLY),
                _pmsgOut.Value);

            return ret;
        }


        /// <summary>
        ///  The IDL_DRSReplicaDemotion method replicates initiates
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
        [CLSCompliant(false)]
        public uint IDL_DRSReplicaDemotion(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")]
            DRS_MSG_REPLICA_DEMOTIONREQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")]
            out DRS_MSG_REPLICA_DEMOTIONREPLY? pmsgOut)
        {
            NativeTypes.DRS_MSG_REPLICA_DEMOTIONREQ _pmsgIn =
                (NativeTypes.DRS_MSG_REPLICA_DEMOTIONREQ)CopyFieldsFrom(
                typeof(DRS_MSG_REPLICA_DEMOTIONREQ),
                typeof(NativeTypes.DRS_MSG_REPLICA_DEMOTIONREQ),
                pmsgIn.Value);

            NativeTypes.DRS_MSG_REPLICA_DEMOTIONREPLY? _pmsgOut;
            uint ret = NativeRpcAdapter.IDL_DRSReplicaDemotion(
                hDrs,
                dwInVersion,
                _pmsgIn,
                out pdwOutVersion,
                out _pmsgOut);

            pmsgOut = (DRS_MSG_REPLICA_DEMOTIONREPLY?)CopyFieldsFrom(
                typeof(NativeTypes.DRS_MSG_REPLICA_DEMOTIONREPLY),
                typeof(DRS_MSG_REPLICA_DEMOTIONREPLY),
                _pmsgOut.Value);

            return ret;
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
        [CLSCompliant(false)]
        public uint IDL_DRSFinishDemotion(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")] 
            DRS_MSG_FINISH_DEMOTIONREQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")] 
            out DRS_MSG_FINISH_DEMOTIONREPLY? pmsgOut)
        {
            NativeTypes.DRS_MSG_FINISH_DEMOTIONREQ _pmsgIn =
                (NativeTypes.DRS_MSG_FINISH_DEMOTIONREQ)CopyFieldsFrom(
                typeof(DRS_MSG_FINISH_DEMOTIONREQ),
                typeof(NativeTypes.DRS_MSG_FINISH_DEMOTIONREQ),
                pmsgIn.Value);

            NativeTypes.DRS_MSG_FINISH_DEMOTIONREPLY? _pmsgOut;
            uint ret = NativeRpcAdapter.IDL_DRSFinishDemotion(
                hDrs,
                dwInVersion,
                _pmsgIn,
                out pdwOutVersion,
                out _pmsgOut);

            pmsgOut = (DRS_MSG_FINISH_DEMOTIONREPLY?)CopyFieldsFrom(
                typeof(NativeTypes.DRS_MSG_FINISH_DEMOTIONREPLY),
                typeof(DRS_MSG_FINISH_DEMOTIONREPLY),
                _pmsgOut.Value);

            return ret;
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
        [CLSCompliant(false)]
        public uint IDL_DRSGetNCChanges(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")] 
            DRS_MSG_GETCHGREQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")]
            out DRS_MSG_GETCHGREPLY? pmsgOut)
        {
            NativeTypes.DRS_MSG_GETCHGREQ _pmsgIn =
                (NativeTypes.DRS_MSG_GETCHGREQ)CopyFieldsFrom(
                typeof(DRS_MSG_GETCHGREQ),
                typeof(NativeTypes.DRS_MSG_GETCHGREQ),
                pmsgIn.Value);

            NativeTypes.DRS_MSG_GETCHGREPLY? _pmsgOut;
            uint ret = NativeRpcAdapter.IDL_DRSGetNCChanges(
                hDrs,
                dwInVersion,
                _pmsgIn,
                out pdwOutVersion,
                out _pmsgOut);

            pmsgOut = (DRS_MSG_GETCHGREPLY?)CopyFieldsFrom(
                typeof(NativeTypes.DRS_MSG_GETCHGREPLY),
                typeof(DRS_MSG_GETCHGREPLY),
                _pmsgOut.Value);

            return ret;
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
        [CLSCompliant(false)]
        public uint IDL_DRSVerifyNames(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")] 
            DRS_MSG_VERIFYREQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")]
            out DRS_MSG_VERIFYREPLY? pmsgOut)
        {
            NativeTypes.DRS_MSG_VERIFYREQ _pmsgIn =
                (NativeTypes.DRS_MSG_VERIFYREQ)CopyFieldsFrom(
                typeof(DRS_MSG_VERIFYREQ),
                typeof(NativeTypes.DRS_MSG_VERIFYREQ),
                pmsgIn.Value);

            NativeTypes.DRS_MSG_VERIFYREPLY? _pmsgOut;
            uint ret = NativeRpcAdapter.IDL_DRSVerifyNames(
                hDrs,
                dwInVersion,
                _pmsgIn,
                out pdwOutVersion,
                out _pmsgOut);

            pmsgOut = (DRS_MSG_VERIFYREPLY?)CopyFieldsFrom(
                typeof(NativeTypes.DRS_MSG_VERIFYREPLY),
                typeof(DRS_MSG_VERIFYREPLY),
                _pmsgOut.Value);

            return ret;
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
        [CLSCompliant(false)]
        public uint IDL_DRSGetMemberships(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")] 
            DRS_MSG_REVMEMB_REQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")] 
            out DRS_MSG_REVMEMB_REPLY? pmsgOut)
        {
            NativeTypes.DRS_MSG_REVMEMB_REQ _pmsgIn =
                (NativeTypes.DRS_MSG_REVMEMB_REQ)CopyFieldsFrom(
                typeof(DRS_MSG_REVMEMB_REQ),
                typeof(NativeTypes.DRS_MSG_REVMEMB_REQ),
                pmsgIn.Value);

            NativeTypes.DRS_MSG_REVMEMB_REPLY? _pmsgOut;
            uint ret = NativeRpcAdapter.IDL_DRSGetMemberships(
                hDrs,
                dwInVersion,
                _pmsgIn,
                out pdwOutVersion,
                out _pmsgOut);

            pmsgOut = (DRS_MSG_REVMEMB_REPLY?)CopyFieldsFrom(
                typeof(NativeTypes.DRS_MSG_REVMEMB_REPLY),
                typeof(DRS_MSG_REVMEMB_REPLY),
                _pmsgOut.Value);

            return ret;
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
        [CLSCompliant(false)]
        public uint IDL_DRSInterDomainMove(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")]
            DRS_MSG_MOVEREQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")]
            out DRS_MSG_MOVEREPLY? pmsgOut)
        {
            NativeTypes.DRS_MSG_MOVEREQ _pmsgIn =
                (NativeTypes.DRS_MSG_MOVEREQ)CopyFieldsFrom(
                typeof(DRS_MSG_MOVEREQ),
                typeof(NativeTypes.DRS_MSG_MOVEREQ),
                pmsgIn.Value);

            NativeTypes.DRS_MSG_MOVEREPLY? _pmsgOut;
            uint ret = NativeRpcAdapter.IDL_DRSInterDomainMove(
                hDrs,
                dwInVersion,
                _pmsgIn,
                out pdwOutVersion,
                out _pmsgOut);

            pmsgOut = (DRS_MSG_MOVEREPLY?)CopyFieldsFrom(
                typeof(NativeTypes.DRS_MSG_MOVEREPLY),
                typeof(DRS_MSG_MOVEREPLY),
                _pmsgOut.Value);

            return ret;
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
        [CLSCompliant(false)]
        public uint IDL_DRSGetNT4ChangeLog(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")] 
            DRS_MSG_NT4_CHGLOG_REQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")]
            out DRS_MSG_NT4_CHGLOG_REPLY? pmsgOut)
        {
            NativeTypes.DRS_MSG_NT4_CHGLOG_REQ _pmsgIn =
                (NativeTypes.DRS_MSG_NT4_CHGLOG_REQ)CopyFieldsFrom(
                typeof(DRS_MSG_NT4_CHGLOG_REQ),
                typeof(NativeTypes.DRS_MSG_NT4_CHGLOG_REQ),
                pmsgIn.Value);

            NativeTypes.DRS_MSG_NT4_CHGLOG_REPLY? _pmsgOut;
            uint ret = NativeRpcAdapter.IDL_DRSGetNT4ChangeLog(
                hDrs,
                dwInVersion,
                _pmsgIn,
                out pdwOutVersion,
                out _pmsgOut);

            pmsgOut = (DRS_MSG_NT4_CHGLOG_REPLY?)CopyFieldsFrom(
                typeof(NativeTypes.DRS_MSG_NT4_CHGLOG_REPLY),
                typeof(DRS_MSG_NT4_CHGLOG_REPLY),
                _pmsgOut.Value);

            return ret;
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
        [CLSCompliant(false)]
        public uint IDL_DRSAddEntry(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")] 
            DRS_MSG_ADDENTRYREQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")] 
            out DRS_MSG_ADDENTRYREPLY? pmsgOut)
        {
            NativeTypes.DRS_MSG_ADDENTRYREQ _pmsgIn =
                (NativeTypes.DRS_MSG_ADDENTRYREQ)CopyFieldsFrom(
                typeof(DRS_MSG_ADDENTRYREQ),
                typeof(NativeTypes.DRS_MSG_ADDENTRYREQ),
                pmsgIn.Value);

            NativeTypes.DRS_MSG_ADDENTRYREPLY? _pmsgOut;
            uint ret = NativeRpcAdapter.IDL_DRSAddEntry(
                hDrs,
                dwInVersion,
                _pmsgIn,
                out pdwOutVersion,
                out _pmsgOut);

            pmsgOut = (DRS_MSG_ADDENTRYREPLY?)CopyFieldsFrom(
                typeof(NativeTypes.DRS_MSG_ADDENTRYREPLY),
                typeof(DRS_MSG_ADDENTRYREPLY),
                _pmsgOut.Value);

            return ret;
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
        [CLSCompliant(false)]
        public uint IDL_DRSGetMemberships2(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")]
            DRS_MSG_GETMEMBERSHIPS2_REQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")]
            out DRS_MSG_GETMEMBERSHIPS2_REPLY? pmsgOut)
        {
            NativeTypes.DRS_MSG_GETMEMBERSHIPS2_REQ _pmsgIn =
                (NativeTypes.DRS_MSG_GETMEMBERSHIPS2_REQ)CopyFieldsFrom(
                typeof(DRS_MSG_GETMEMBERSHIPS2_REQ),
                typeof(NativeTypes.DRS_MSG_GETMEMBERSHIPS2_REQ),
                pmsgIn.Value);

            NativeTypes.DRS_MSG_GETMEMBERSHIPS2_REPLY? _pmsgOut;
            uint ret = NativeRpcAdapter.IDL_DRSGetMemberships2(
                hDrs,
                dwInVersion,
                _pmsgIn,
                out pdwOutVersion,
                out _pmsgOut);

            pmsgOut = (DRS_MSG_GETMEMBERSHIPS2_REPLY?)CopyFieldsFrom(
                typeof(NativeTypes.DRS_MSG_GETMEMBERSHIPS2_REPLY),
                typeof(DRS_MSG_GETMEMBERSHIPS2_REPLY),
                _pmsgOut.Value);

            return ret;
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
        [CLSCompliant(false)]
        public uint IDL_DRSGetObjectExistence(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")] 
            DRS_MSG_EXISTREQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")] 
            out DRS_MSG_EXISTREPLY? pmsgOut)
        {
            NativeTypes.DRS_MSG_EXISTREQ _pmsgIn =
                (NativeTypes.DRS_MSG_EXISTREQ)CopyFieldsFrom(
                typeof(DRS_MSG_EXISTREQ),
                typeof(NativeTypes.DRS_MSG_EXISTREQ),
                pmsgIn.Value);

            NativeTypes.DRS_MSG_EXISTREPLY? _pmsgOut;
            uint ret = NativeRpcAdapter.IDL_DRSGetObjectExistence(
                hDrs,
                dwInVersion,
                _pmsgIn,
                out pdwOutVersion,
                out _pmsgOut);

            pmsgOut = (DRS_MSG_EXISTREPLY?)CopyFieldsFrom(
                typeof(NativeTypes.DRS_MSG_EXISTREPLY),
                typeof(DRS_MSG_EXISTREPLY),
                _pmsgOut.Value);

            return ret;
        }

        /// <summary>
        ///  The IDL_DRSAddCloneDC method is used to create a new DC object by copying attributes 
        ///  from an existing DC object.
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
        [CLSCompliant(false)]
        public uint IDL_DRSAddCloneDC(
            IntPtr hDrs,
            uint dwInVersion,
            DRS_MSG_ADDCLONEDCREQ? pmsgIn,
            out uint? pdwOutVersion,
            out DRS_MSG_ADDCLONEDCREPLY? pmsgOut)
        {
            NativeTypes.DRS_MSG_ADDCLONEDCREQ _pmsgIn =
                (NativeTypes.DRS_MSG_ADDCLONEDCREQ)CopyFieldsFrom(
                typeof(DRS_MSG_ADDCLONEDCREQ),
                typeof(NativeTypes.DRS_MSG_ADDCLONEDCREQ),
                pmsgIn.Value);

            NativeTypes.DRS_MSG_ADDCLONEDCREPLY? _pmsgOut;
            uint ret = NativeRpcAdapter.IDL_DRSAddCloneDC(
                hDrs,
                dwInVersion,
                _pmsgIn,
                out pdwOutVersion,
                out _pmsgOut);

            pmsgOut = (DRS_MSG_ADDCLONEDCREPLY?)CopyFieldsFrom(
                typeof(NativeTypes.DRS_MSG_ADDCLONEDCREPLY),
                typeof(DRS_MSG_ADDCLONEDCREPLY),
                _pmsgOut.Value);

            return ret;
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
        [CLSCompliant(false)]
        public uint IDL_DRSWriteNgcKey(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")]
            DRS_MSG_WRITENGCKEYREQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")] 
            out DRS_MSG_WRITENGCKEYREPLY? pmsgOut)
        {

            NativeTypes.DRS_MSG_WRITENGCKEYREQ _pmsgIn =
                (NativeTypes.DRS_MSG_WRITENGCKEYREQ)CopyFieldsFrom(
                typeof(DRS_MSG_WRITENGCKEYREQ),
                typeof(NativeTypes.DRS_MSG_WRITENGCKEYREQ),
                pmsgIn.Value);

            NativeTypes.DRS_MSG_WRITENGCKEYREPLY? _pmsgOut;
            uint ret = NativeRpcAdapter.IDL_DRSWriteNgcKey(
                hDrs,
                dwInVersion,
                _pmsgIn,
                out pdwOutVersion,
                out _pmsgOut);

            pmsgOut = (DRS_MSG_WRITENGCKEYREPLY?)CopyFieldsFrom(
                typeof(NativeTypes.DRS_MSG_WRITENGCKEYREPLY),
                typeof(DRS_MSG_WRITENGCKEYREPLY),
                _pmsgOut.Value);

            return ret;
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
        [CLSCompliant(false)]
        public uint IDL_DRSReadNgcKey(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")]
            DRS_MSG_READNGCKEYREQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")] 
            out DRS_MSG_READNGCKEYREPLY? pmsgOut)
        {

            NativeTypes.DRS_MSG_READNGCKEYREQ _pmsgIn =
                (NativeTypes.DRS_MSG_READNGCKEYREQ)CopyFieldsFrom(
                typeof(DRS_MSG_READNGCKEYREQ),
                typeof(NativeTypes.DRS_MSG_READNGCKEYREQ),
                pmsgIn.Value);

            NativeTypes.DRS_MSG_READNGCKEYREPLY? _pmsgOut;
            uint ret = NativeRpcAdapter.IDL_DRSReadNgcKey(
                hDrs,
                dwInVersion,
                _pmsgIn,
                out pdwOutVersion,
                out _pmsgOut);

            pmsgOut = (DRS_MSG_READNGCKEYREPLY?)CopyFieldsFrom(
                typeof(NativeTypes.DRS_MSG_READNGCKEYREPLY),
                typeof(DRS_MSG_READNGCKEYREPLY),
                _pmsgOut.Value);

            return ret;
        }
    }
}
