// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Samr
{
    /// <summary>
    /// SamrRpcAdapter which implements SAMR RPC with RPCE SDK.
    /// </summary>
    public class SamrRpcAdapter : ISamrRpcAdapter
    {
        #region Member variables

        /// <summary>
        /// RPCE client transport
        /// </summary>
        private RpceClientTransport rpceClientTransport;

        /// <summary>
        /// Timeout for RPC bind/call 
        /// </summary>
        private TimeSpan rpceTimeout;

        /// <summary>
        /// RPCE client transport
        /// </summary>
        internal RpceClientTransport RpceClientTransport
        {
            get
            {
                return rpceClientTransport;
            }
            set
            {
                rpceClientTransport = value;
            }
        }

        #endregion Member variables

        #region Helper methods

        /// <summary>
        /// The common part of all the Samr Rpc methods
        /// </summary>
        /// <param name="paramList">input param list to decode</param>
        /// <param name="opnum">opnum of the current Samr Rpc method</param>
        /// <returns>the decoded parameter list</returns>
        private RpceInt3264Collection RpceCall(Int3264[] paramList, ushort opnum)
        {
            byte[] requestStub;
            byte[] responseStub;

            requestStub = RpceStubEncoder.ToBytes(
          RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { 
                        new RpceStubExprEval(samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(samr_SAMPR_LOGON_HOURSExprEval_0003) },
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
          RpceStubHelper.GetPlatform(),
                    SamrRpcStubFormatString.TypeFormatString,
                    new RpceStubExprEval[] { 
                        new RpceStubExprEval(samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000),
                        new RpceStubExprEval(samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001),
                        new RpceStubExprEval(samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002),
                        new RpceStubExprEval(samr_SAMPR_LOGON_HOURSExprEval_0003) },
                    SamrRpcStubFormatString.ProcFormatString,
                    SamrRpcStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList);

            return outParamList;
        }


        /// <summary>
        /// samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000 defined by MIDL.
        /// </summary>
        /// <param name="rpcStub">RpceStub structure.</param>
        internal static void samr_SAMPR_USER_LOGON_INFORMATIONExprEval_0000(RpceStub rpcStub)
        {
            IntPtr pStackTop = rpcStub.GetStackTop();
            _SAMPR_USER_LOGON_INFORMATION logonInfo =
                TypeMarshal.ToStruct<_SAMPR_USER_LOGON_INFORMATION>(pStackTop);
            rpcStub.SetOffset(0);
            rpcStub.SetMaxCount((uint)((logonInfo.LogonHours.UnitsPerWeek + 7) / 8));
        }


        /// <summary>
        /// samr_SAMPR_USER_LOGON_HOUR_INFORMATIONExprEval_0001 defined by MIDL.
        /// </summary>
        /// <param name="rpcStub">RpceStub structure.</param>
        internal static void samr_SAMPR_USER_LOGON_HOURS_INFORMATIONExprEval_0001(RpceStub rpcStub)
        {
            IntPtr pStackTop = rpcStub.GetStackTop();
            _SAMPR_USER_LOGON_HOURS_INFORMATION logonInfo =
                TypeMarshal.ToStruct<_SAMPR_USER_LOGON_HOURS_INFORMATION>(pStackTop);
            rpcStub.SetOffset(0);
            rpcStub.SetMaxCount((uint)((logonInfo.LogonHours.UnitsPerWeek + 7) / 8));
        }


        /// <summary>
        /// samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002 defined by MIDL.
        /// </summary>
        /// <param name="rpcStub">RpceStub structure.</param>
        internal static void samr_SAMPR_USER_ACCOUNT_INFORMATIONExprEval_0002(RpceStub rpcStub)
        {
            IntPtr pStackTop = rpcStub.GetStackTop();
            _SAMPR_USER_ACCOUNT_INFORMATION accountInfo =
                TypeMarshal.ToStruct<_SAMPR_USER_ACCOUNT_INFORMATION>(pStackTop);
            rpcStub.SetOffset(0);
            rpcStub.SetMaxCount((uint)((accountInfo.LogonHours.UnitsPerWeek + 7) / 8));
        }


        /// <summary>
        /// samr_SAMPR_LOGON_HOURSExprEval_0003 defined by MIDL.
        /// </summary>
        /// <param name="rpcStub">RpceStub structure.</param>
        internal static void samr_SAMPR_LOGON_HOURSExprEval_0003(RpceStub rpcStub)
        {
            IntPtr pStackTop = rpcStub.GetStackTop();
            _SAMPR_LOGON_HOURS logonHours = TypeMarshal.ToStruct<_SAMPR_LOGON_HOURS>(pStackTop);
            rpcStub.SetOffset(0);
            rpcStub.SetMaxCount((uint)((logonHours.UnitsPerWeek + 7) / 8));
        }
        #endregion Helper methods

        #region ISamrRpcAdapter Members

        /// <summary>
        /// Bind to SAMR RPC server.
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
        /// <param name="transportCredential">
        /// If connect by SMB/SMB2, it's the security credential 
        /// used by under layer transport (SMB/SMB2). 
        /// If connect by TCP, this parameter is ignored.
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
            AccountCredential transportCredential,
            ClientSecurityContext securityContext,
            RpceAuthenticationLevel authenticationLevel,
            TimeSpan timeout)
        {
            if (rpceClientTransport != null)
            {
                throw new InvalidOperationException("SAMR has already been binded.");
            }

            rpceTimeout = timeout;
            rpceClientTransport = new RpceClientTransport();

            rpceClientTransport.Bind(
                protocolSequence,
                networkAddress,
                endpoint,
                transportCredential,
                SamrUtility.SAMR_RPC_INTERFACE_UUID,
                SamrUtility.SAMR_RPC_INTERFACE_MAJOR_VERSION,
                SamrUtility.SAMR_RPC_INTERFACE_MINOR_VERSION,
                securityContext,
                authenticationLevel,
                false,
                rpceTimeout);
        }


        /// <summary>
        /// RPC unbind.
        /// </summary>
        public void Unbind()
        {
            if (rpceClientTransport != null)
            {
                rpceClientTransport.Unbind(rpceTimeout);
                rpceClientTransport.Dispose();
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


        /// <summary>
        /// Gets session key.
        /// </summary>
        public byte[] SessionKey
        {
            get
            {
                FileServiceClientContext fsTransportContext =
                    rpceClientTransport.Context.FileServiceTransportContext;
                SmbClientContext smbContext = fsTransportContext as SmbClientContext;
                if (smbContext != null
                    && smbContext.Connection != null
                    && smbContext.Connection.SessionTable != null
                    && smbContext.Connection.SessionTable.Count > 0)
                {
                    return smbContext.Connection.SessionTable[0].SessionKey4Smb;
                }
                Smb2ClientGlobalContext smb2Context = fsTransportContext as Smb2ClientGlobalContext;
                if (smb2Context != null
                    && smb2Context.ConnectionTable != null
                    && smb2Context.ConnectionTable.Count > 0)
                {
                    Smb2ClientConnection[] connections =
                        new Smb2ClientConnection[smb2Context.ConnectionTable.Count];
                    smb2Context.ConnectionTable.Values.CopyTo(connections, 0);

                    if (connections[0].SessionTable != null && connections[0].SessionTable.Count > 0)
                    {
                        Smb2ClientSession[] sessions =
                            new Smb2ClientSession[connections[0].SessionTable.Count];

                        connections[0].SessionTable.Values.CopyTo(sessions, 0);

                        return sessions[0].SessionKey;
                    }
                }
                return null;
            }
        }


        /// <summary>
        ///  The SamrConnect method returns a handle to a server
        ///  object. Opnum: 0 
        /// </summary>
        /// <param name="ServerName">
        ///  The first character of the NETBIOS name of the responder;
        ///  this parameter MAYServerName is ignored on receipt.
        ///   be ignored on receipt.
        /// </param>
        /// <param name="ServerHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <param name="DesiredAccess">
        ///  An ACCESS_MASK that indicates the access requested for
        ///  ServerHandle upon output. See section  for a listing
        ///  of possible values.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrConnect(string ServerName, out IntPtr ServerHandle, uint DesiredAccess)
        {
            const ushort opnum = 0;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pServerName = Marshal.StringToHGlobalUni(ServerName);

            paramList = new Int3264[] {
                pServerName,
                IntPtr.Zero,
                DesiredAccess,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    ServerHandle = Marshal.ReadIntPtr(outParamList[1]);
                    retVal = outParamList[3].ToInt32();
                }
            }
            finally
            {
                pServerName.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrCloseHandle method closes (that is, releases
        ///  server-side resources used by) any context handle obtained
        ///  from this RPC interface. Opnum: 1 
        /// </summary>
        /// <param name="SamHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  any context handle returned from this interface.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrCloseHandle(ref IntPtr SamHandle)
        {
            const ushort opnum = 1;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr ppSamHandle = TypeMarshal.ToIntPtr(SamHandle);

            paramList = new Int3264[] {
                ppSamHandle,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    SamHandle = Marshal.ReadIntPtr(outParamList[0]);
                    retVal = outParamList[1].ToInt32();
                }
            }
            finally
            {
                ppSamHandle.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrSetSecurityObject method sets the access control
        ///  on a server, domain, user, group, or alias object.
        ///  Opnum: 2 
        /// </summary>
        /// <param name="ObjectHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a server, domain, user, group, or alias object.
        /// </param>
        /// <param name="SecurityInformation">
        ///  A bit field that indicates the fields of SecurityDescriptor
        ///  that are requested to be set. The SECURITY_INFORMATION
        ///  type is defined in [MS-DTYP] section. The following
        ///  bits are valid; all other bits MUST be zero on send
        ///  and ignored on receipt.
        /// </param>
        /// <param name="SecurityDescriptor">
        ///  A security descriptor expressing access that is specific
        ///  to the ObjectHandle.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrSetSecurityObject(
            IntPtr ObjectHandle,
            SecurityInformation_Values SecurityInformation,
            _SAMPR_SR_SECURITY_DESCRIPTOR? SecurityDescriptor)
        {
            const ushort opnum = 2;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pSecurityDescriptor = TypeMarshal.ToIntPtr(SecurityDescriptor);

            paramList = new Int3264[] {
                ObjectHandle,
                (uint)SecurityInformation,
                pSecurityDescriptor,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    retVal = outParamList[3].ToInt32();
                }
            }
            finally
            {
                pSecurityDescriptor.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrQuerySecurityObject method queries the access
        ///  control on a server, domain, user, group, or alias
        ///  object. Opnum: 3 
        /// </summary>
        /// <param name="ObjectHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a server, domain, user, group, or alias object.
        /// </param>
        /// <param name="SecurityInformation">
        ///  A bit field that specifies which fields of SecurityDescriptor
        ///  the client is requesting to be returned. The SECURITY_INFORMATION
        ///  type is defined in [MS-DTYP] section. The following
        ///  bits are valid; all other bits MUST be zero on send
        ///  and ignored on receipt.
        /// </param>
        /// <param name="SecurityDescriptor">
        ///  A security descriptor expressing accesses that are specific
        ///  to the ObjectHandle and the owner and group of the
        ///  object. [MS-DTYP] section  contains the specification
        ///  for a valid security descriptor.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrQuerySecurityObject(
            IntPtr ObjectHandle,
            SamrQuerySecurityObject_SecurityInformation_Values SecurityInformation,
            out _SAMPR_SR_SECURITY_DESCRIPTOR? SecurityDescriptor)
        {
            const ushort opnum = 3;
            Int3264[] paramList;
            int retVal = 0;

            paramList = new Int3264[] {
                ObjectHandle,
                (int)SecurityInformation,
                IntPtr.Zero,
                IntPtr.Zero
            };

            using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
            {
                IntPtr pSecurityDescriptor = Marshal.ReadIntPtr(outParamList[2]);
                SecurityDescriptor = TypeMarshal.ToNullableStruct<_SAMPR_SR_SECURITY_DESCRIPTOR>(
                    pSecurityDescriptor);
                retVal = outParamList[3].ToInt32();
            }

            return retVal;
        }


        /// <summary>
        ///  Reserved for local use. Opnum: 4 
        /// </summary>
        public void Opnum4NotUsedOnWire()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///  The SamrLookupDomainInSamServer method obtains the SID
        ///  of a domain object, given the object's name. Opnum
        ///  : 5 
        /// </summary>
        /// <param name="ServerHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a server object.
        /// </param>
        /// <param name="Name">
        ///  A UTF-16 encoded string.
        /// </param>
        /// <param name="DomainId">
        ///  A SID value of a domain that matches the Name passed
        ///  in. The match must be exact (no wildcards are permitted).
        ///  See message processing later in this section for more
        ///  details.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrLookupDomainInSamServer(IntPtr ServerHandle, _RPC_UNICODE_STRING Name, out _RPC_SID? DomainId)
        {
            const ushort opnum = 5;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pName = TypeMarshal.ToIntPtr(Name);

            paramList = new Int3264[] {
                ServerHandle,
                pName,
                IntPtr.Zero,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    DomainId = TypeMarshal.ToNullableStruct<_RPC_SID>(Marshal.ReadIntPtr(outParamList[2]));
                    retVal = outParamList[3].ToInt32();
                }
            }
            finally
            {
                pName.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrEnumerateDomainsInSamServer method obtains a
        ///  listing of all domains hosted by the server side of
        ///  this protocol. Opnum: 6 
        /// </summary>
        /// <param name="ServerHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a server object.
        /// </param>
        /// <param name="EnumerationContext">
        ///  This value is a cookie that the server can use to continue
        ///  an enumeration on a subsequent call. It is an opaque
        ///  value to the client.
        /// </param>
        /// <param name="Buffer">
        ///  A listing of domain information, as described in section
        ///  .
        /// </param>
        /// <param name="PreferedMaximumLength">
        ///  The requested maximum number of bytes to return in Buffer.
        /// </param>
        /// <param name="CountReturned">
        ///  The count of domain elements returned in Buffer.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrEnumerateDomainsInSamServer(
            IntPtr ServerHandle,
            ref uint? EnumerationContext,
            out _SAMPR_ENUMERATION_BUFFER? Buffer,
            uint PreferedMaximumLength,
            out uint CountReturned)
        {
            const ushort opnum = 6;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pEnumerationContext = TypeMarshal.ToIntPtr(EnumerationContext);

            paramList = new Int3264[] {
                ServerHandle,
                pEnumerationContext,
                IntPtr.Zero,
                PreferedMaximumLength,
                IntPtr.Zero,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    EnumerationContext = TypeMarshal.ToNullableStruct<uint>(outParamList[1]);
                    Buffer = TypeMarshal.ToNullableStruct<_SAMPR_ENUMERATION_BUFFER>(
                        Marshal.ReadIntPtr(outParamList[2]));
                    CountReturned = TypeMarshal.ToStruct<uint>(outParamList[4]);
                    retVal = outParamList[5].ToInt32();
                }
            }
            finally
            {
                pEnumerationContext.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrOpenDomain method obtains a handle to a domain
        ///  object, given a SID. Opnum: 7 
        /// </summary>
        /// <param name="ServerHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a server object.
        /// </param>
        /// <param name="DesiredAccess">
        ///  An ACCESS_MASK. See section  for a list of domain access
        ///  values.
        /// </param>
        /// <param name="DomainId">
        ///  A SID value of a domain hosted by the server side of
        ///  this protocol.
        /// </param>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrOpenDomain(IntPtr ServerHandle, uint DesiredAccess, _RPC_SID? DomainId, out IntPtr DomainHandle)
        {
            const ushort opnum = 7;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pDomainId = TypeMarshal.ToIntPtr(DomainId);

            paramList = new Int3264[] {
                ServerHandle,
                DesiredAccess,
                pDomainId,
                IntPtr.Zero,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    DomainHandle = Marshal.ReadIntPtr(outParamList[3]);
                    retVal = outParamList[4].ToInt32();
                }
            }
            finally
            {
                pDomainId.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrQueryInformationDomain method obtains attributes
        ///  from a domain object. Opnum: 8 
        /// </summary>
        /// <param name="DomainHandle">
        ///  DomainHandle parameter.
        /// </param>
        /// <param name="DomainInformationClass">
        ///  DomainInformationClass parameter.
        /// </param>
        /// <param name="Buffer">
        ///  Buffer parameter.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrQueryInformationDomain(
            IntPtr DomainHandle,
            _DOMAIN_INFORMATION_CLASS DomainInformationClass,
            out _SAMPR_DOMAIN_INFO_BUFFER? Buffer)
        {
            const ushort opnum = 8;
            Int3264[] paramList;
            int retVal = 0;

            paramList = new Int3264[] {
                DomainHandle,
                (int)DomainInformationClass,
                IntPtr.Zero,
                IntPtr.Zero
            };

            using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
            {
                Buffer = TypeMarshal.ToNullableStruct<_SAMPR_DOMAIN_INFO_BUFFER>(
                    Marshal.ReadIntPtr(outParamList[2]), DomainInformationClass, null, null);
                retVal = outParamList[3].ToInt32();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrSetInformationDomain method updates attributes
        ///  on a domain object. Opnum: 9 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="DomainInformationClass">
        ///  An enumeration indicating which attributes to update.
        ///  See section  for a list of possible values.
        /// </param>
        /// <param name="DomainInformation">
        ///  The requested attributes and values to update. See section
        ///   for structure details.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrSetInformationDomain(
            IntPtr DomainHandle,
            _DOMAIN_INFORMATION_CLASS DomainInformationClass,
            _SAMPR_DOMAIN_INFO_BUFFER DomainInformation)
        {
            const ushort opnum = 9;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pDomainInformation = TypeMarshal.ToIntPtr(
                DomainInformation,
                DomainInformationClass,
                null,
                null);

            paramList = new Int3264[] {
                DomainHandle,
                (int)DomainInformationClass,
                pDomainInformation,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    retVal = outParamList[3].ToInt32();
                }
            }
            finally
            {
                pDomainInformation.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrCreateGroupInDomain method creates a group object
        ///  within a domain. Opnum: 10 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="Name">
        ///  The value to use as the name of the group. Details on
        ///  how this value maps to the data model are provided
        ///  later in this section.
        /// </param>
        /// <param name="DesiredAccess">
        ///  The access requested on the GroupHandle on output. See
        ///  section  for a listing of possible values.
        /// </param>
        /// <param name="GroupHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <param name="RelativeId">
        ///  The RID of the newly created group.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrCreateGroupInDomain(
            IntPtr DomainHandle,
            _RPC_UNICODE_STRING Name,
            uint DesiredAccess,
            out IntPtr GroupHandle,
            out uint RelativeId)
        {
            const ushort opnum = 10;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pName = TypeMarshal.ToIntPtr(Name);

            paramList = new Int3264[] {
                DomainHandle,
                pName,
                DesiredAccess,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    GroupHandle = Marshal.ReadIntPtr(outParamList[3]);
                    RelativeId = TypeMarshal.ToStruct<uint>(outParamList[4]);
                    retVal = outParamList[5].ToInt32();
                }
            }
            finally
            {
                pName.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrEnumerateGroupsInDomain method enumerates all
        ///  groups. Opnum: 11 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="EnumerationContext">
        ///  This value is a cookie that the server can use to continue
        ///  an enumeration on a subsequent call. It is an opaque
        ///  value to the client.
        /// </param>
        /// <param name="Buffer">
        ///  A list of group information, as specified in section
        ///  .
        /// </param>
        /// <param name="PreferedMaximumLength">
        ///  The requested maximum number of bytes to return in Buffer.
        /// </param>
        /// <param name="CountReturned">
        ///  The count of domain elements returned in Buffer.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrEnumerateGroupsInDomain(
            IntPtr DomainHandle,
            ref uint? EnumerationContext,
            out _SAMPR_ENUMERATION_BUFFER? Buffer,
            uint PreferedMaximumLength,
            out uint CountReturned)
        {
            const ushort opnum = 11;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pEnumerationContext = TypeMarshal.ToIntPtr(EnumerationContext);

            paramList = new Int3264[] {
                DomainHandle,
                pEnumerationContext,
                IntPtr.Zero,
                PreferedMaximumLength,
                IntPtr.Zero,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    EnumerationContext = TypeMarshal.ToNullableStruct<uint>(outParamList[1]);
                    Buffer = TypeMarshal.ToNullableStruct<_SAMPR_ENUMERATION_BUFFER>(
                        Marshal.ReadIntPtr(outParamList[2]));
                    CountReturned = TypeMarshal.ToStruct<uint>(outParamList[4]);
                    retVal = outParamList[5].ToInt32();
                }
            }
            finally
            {
                pEnumerationContext.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrCreateUserInDomain method creates a user. Opnum
        ///  : 12 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="Name">
        ///  The value to use as the name of the user. See the message
        ///  processing shown later in this section for details
        ///  on how this value maps to the data model.
        /// </param>
        /// <param name="DesiredAccess">
        ///  The access requested on the UserHandle on output. See
        ///  section  for a listing of possible values.
        /// </param>
        /// <param name="UserHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <param name="RelativeId">
        ///  The RID of the newly created user.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrCreateUserInDomain(
            IntPtr DomainHandle,
            _RPC_UNICODE_STRING Name,
            uint DesiredAccess,
            out IntPtr UserHandle,
            out uint RelativeId)
        {
            const ushort opnum = 12;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pName = TypeMarshal.ToIntPtr(Name);

            paramList = new Int3264[] {
                DomainHandle,
                pName,
                DesiredAccess,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    UserHandle = Marshal.ReadIntPtr(outParamList[3]);
                    RelativeId = TypeMarshal.ToStruct<uint>(outParamList[4]);
                    retVal = outParamList[5].ToInt32();
                }
            }
            finally
            {
                pName.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrEnumerateUsersInDomain method enumerates all
        ///  users. Opnum: 13 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="EnumerationContext">
        ///  This value is a cookie that the server can use to continue
        ///  an enumeration on a subsequent call. It is an opaque
        ///  value to the client.
        /// </param>
        /// <param name="UserAccountControl">
        ///  A filter value to be used on the userAccountControl
        ///  attribute.
        /// </param>
        /// <param name="Buffer">
        ///  A list of user information, as specified in section
        ///  .
        /// </param>
        /// <param name="PreferedMaximumLength">
        ///  The requested maximum number of bytes to return in Buffer.
        /// </param>
        /// <param name="CountReturned">
        ///  The count of domain elements returned in Buffer.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrEnumerateUsersInDomain(
            IntPtr DomainHandle,
            ref uint? EnumerationContext,
            uint UserAccountControl,
            out _SAMPR_ENUMERATION_BUFFER? Buffer,
            uint PreferedMaximumLength,
            out uint CountReturned)
        {
            const ushort opnum = 13;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pEnumerationContext = TypeMarshal.ToIntPtr(EnumerationContext);

            paramList = new Int3264[] {
                DomainHandle,
                pEnumerationContext,
                UserAccountControl,
                IntPtr.Zero,
                PreferedMaximumLength,
                IntPtr.Zero,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    EnumerationContext = TypeMarshal.ToNullableStruct<uint>(outParamList[1]);
                    Buffer = TypeMarshal.ToNullableStruct<_SAMPR_ENUMERATION_BUFFER>(
                        Marshal.ReadIntPtr(outParamList[3]));
                    CountReturned = TypeMarshal.ToStruct<uint>(outParamList[5]);
                    retVal = outParamList[6].ToInt32();
                }
            }
            finally
            {
                pEnumerationContext.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrCreateAliasInDomain method creates an alias.
        ///  Opnum: 14 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="AccountName">
        ///  The value to use as the name of the alias. Details on
        ///  how this value maps to the data model are provided
        ///  later in this section.
        /// </param>
        /// <param name="DesiredAccess">
        ///  The access requested on the AliasHandle on output. See
        ///  section  for a listing of possible values.
        /// </param>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <param name="RelativeId">
        ///  The RID of the newly created alias.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrCreateAliasInDomain(
            IntPtr DomainHandle,
            _RPC_UNICODE_STRING AccountName,
            uint DesiredAccess,
            out IntPtr AliasHandle,
            out uint RelativeId)
        {
            const ushort opnum = 14;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pAccountName = TypeMarshal.ToIntPtr(AccountName);

            paramList = new Int3264[] {
                DomainHandle,
                pAccountName,
                DesiredAccess,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    AliasHandle = Marshal.ReadIntPtr(outParamList[3]);
                    RelativeId = TypeMarshal.ToStruct<uint>(outParamList[4]);
                    retVal = outParamList[5].ToInt32();
                }
            }
            finally
            {
                pAccountName.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrEnumerateAliasesInDomain method enumerates all
        ///  aliases. Opnum: 15 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="EnumerationContext">
        ///  This value is a cookie that the server can use to continue
        ///  an enumeration on a subsequent call. It is an opaque
        ///  value to the client.
        /// </param>
        /// <param name="Buffer">
        ///  A list of alias information, as specified in section
        ///  .
        /// </param>
        /// <param name="PreferedMaximumLength">
        ///  The requested maximum number of bytes to return in Buffer.
        /// </param>
        /// <param name="CountReturned">
        ///  The count of domain elements returned in Buffer.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrEnumerateAliasesInDomain(
            IntPtr DomainHandle,
            ref uint? EnumerationContext,
            out _SAMPR_ENUMERATION_BUFFER? Buffer,
            uint PreferedMaximumLength,
            out uint CountReturned)
        {
            const ushort opnum = 15;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pEnumerationContext = TypeMarshal.ToIntPtr(EnumerationContext);

            paramList = new Int3264[] {
                DomainHandle,
                pEnumerationContext,
                IntPtr.Zero,
                PreferedMaximumLength,
                IntPtr.Zero,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    EnumerationContext = TypeMarshal.ToNullableStruct<uint>(outParamList[1]);
                    Buffer = TypeMarshal.ToNullableStruct<_SAMPR_ENUMERATION_BUFFER>(
                        Marshal.ReadIntPtr(outParamList[2]));
                    CountReturned = TypeMarshal.ToStruct<uint>(outParamList[4]);
                    retVal = outParamList[5].ToInt32();
                }
            }
            finally
            {
                pEnumerationContext.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrGetAliasMembership method obtains the union
        ///  of all aliases that a given set of SIDs is a member
        ///  of. Opnum: 16 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="SidArray">
        ///  A list of SIDs.
        /// </param>
        /// <param name="Membership">
        ///  The union of all aliases (represented by RIDs) that
        ///  all SIDs in SidArray are a member of.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrGetAliasMembership(
            IntPtr DomainHandle,
            _SAMPR_PSID_ARRAY SidArray,
            out _SAMPR_ULONG_ARRAY Membership)
        {
            const ushort opnum = 16;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pSidArray = TypeMarshal.ToIntPtr(SidArray);

            paramList = new Int3264[] {
                DomainHandle,
                pSidArray,
                IntPtr.Zero,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    Membership = TypeMarshal.ToStruct<_SAMPR_ULONG_ARRAY>(outParamList[2]);
                    retVal = outParamList[3].ToInt32();
                }
            }
            finally
            {
                pSidArray.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrLookupNamesInDomain method translates a set
        ///  of account names into a set of RIDs. Opnum: 17 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="Count">
        ///  The number of elements in Names. The maximum value of
        ///  1,000 is chosen to limit the amount of memory that
        ///  the client can force the server to allocate.
        /// </param>
        /// <param name="Names">
        ///  An array of strings that are to be mapped to RIDs.
        /// </param>
        /// <param name="RelativeIds">
        ///  An array of RIDs of accounts that correspond to the
        ///  elements in Names.
        /// </param>
        /// <param name="Use">
        ///  An array of SID_NAME_USE enumeration values that describe
        ///  the type of account for each entry in RelativeIds.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when the pNames is null.
        /// </exception>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrLookupNamesInDomain(
            IntPtr DomainHandle,
            uint Count,
            _RPC_UNICODE_STRING[] Names,
            out _SAMPR_ULONG_ARRAY RelativeIds,
            out _SAMPR_ULONG_ARRAY Use)
        {
            const ushort opnum = 17;
            Int3264[] paramList;
            int retVal = 0;

            if (Names == null)
            {
                throw new ArgumentNullException("Names");
            }

            SafeIntPtr pNames = TypeMarshal.ToIntPtr(Names);

            paramList = new Int3264[] {
                DomainHandle,
                Count,
                Marshal.ReadIntPtr(pNames),
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    RelativeIds = TypeMarshal.ToStruct<_SAMPR_ULONG_ARRAY>(outParamList[3]);
                    Use = TypeMarshal.ToStruct<_SAMPR_ULONG_ARRAY>(outParamList[4]);
                    retVal = outParamList[5].ToInt32();
                }
            }
            finally
            {
                pNames.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrLookupIdsInDomain method translates a set of
        ///  RIDs into account names. Opnum: 18 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="Count">
        ///  The number of elements in RelativeIds. The maximum value
        ///  of 1,000 is chosen to limit the amount of memory that
        ///  the client can force the server to allocate.
        /// </param>
        /// <param name="RelativeIds">
        ///  An array of RIDs that are to be mapped to account names.
        /// </param>
        /// <param name="Names">
        ///  A structure containing an array of account names that
        ///  correspond to the elements in RelativeIds.
        /// </param>
        /// <param name="Use">
        ///  A structure containing an array of SID_NAME_USE enumeration
        ///  values that describe the type of account for each entry
        ///  in RelativeIds.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when the RelativeIds is null.
        /// </exception>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrLookupIdsInDomain(
            IntPtr DomainHandle,
            uint Count,
            uint[] RelativeIds,
            out _SAMPR_RETURNED_USTRING_ARRAY Names,
            out _SAMPR_ULONG_ARRAY Use)
        {
            const ushort opnum = 18;
            Int3264[] paramList;
            int retVal = 0;

            if (RelativeIds == null)
            {
                throw new ArgumentNullException("RelativeIds");
            }

            SafeIntPtr pRelativeIds = TypeMarshal.ToIntPtr(RelativeIds);

            paramList = new Int3264[] {
                DomainHandle,
                Count,
                Marshal.ReadIntPtr(pRelativeIds),
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    Names = TypeMarshal.ToStruct<_SAMPR_RETURNED_USTRING_ARRAY>(outParamList[3]);
                    Use = TypeMarshal.ToStruct<_SAMPR_ULONG_ARRAY>(outParamList[4]);
                    retVal = outParamList[5].ToInt32();
                }
            }
            finally
            {
                pRelativeIds.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrOpenGroup method obtains a handle to a group,
        ///  given a RID. Opnum: 19 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="DesiredAccess">
        ///  An ACCESS_MASK that indicates the requested access for
        ///  the returned handle. See section  for a list of group
        ///  access values.
        /// </param>
        /// <param name="GroupId">
        ///  A RID of a group.
        /// </param>
        /// <param name="GroupHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrOpenGroup(IntPtr DomainHandle, uint DesiredAccess, uint GroupId, out IntPtr GroupHandle)
        {
            const ushort opnum = 19;
            Int3264[] paramList;
            int retVal = 0;

            paramList = new Int3264[] {
                DomainHandle,
                DesiredAccess,
                GroupId,
                IntPtr.Zero,
                IntPtr.Zero
            };

            using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
            {
                GroupHandle = Marshal.ReadIntPtr(outParamList[3]);
                retVal = outParamList[4].ToInt32();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrQueryInformationGroup method obtains attributes
        ///  from a group object. Opnum: 20 
        /// </summary>
        /// <param name="GroupHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a group object.
        /// </param>
        /// <param name="GroupInformationClass">
        ///  An enumeration indicating which attributes to return.
        ///  See section  for a listing of possible values.
        /// </param>
        /// <param name="Buffer">
        ///  The requested attributes on output. See section  for
        ///  structure details.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrQueryInformationGroup(
            IntPtr GroupHandle,
            _GROUP_INFORMATION_CLASS GroupInformationClass,
            out _SAMPR_GROUP_INFO_BUFFER? Buffer)
        {
            const ushort opnum = 20;
            Int3264[] paramList;
            int retVal = 0;

            paramList = new Int3264[] {
                GroupHandle,
                (int)GroupInformationClass,
                IntPtr.Zero,
                IntPtr.Zero
            };

            using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
            {
                Buffer = TypeMarshal.ToNullableStruct<_SAMPR_GROUP_INFO_BUFFER>(
                    Marshal.ReadIntPtr(outParamList[2]), GroupInformationClass, null, null);
                retVal = outParamList[3].ToInt32();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrSetInformationGroup method updates attributes
        ///  on a group object. Opnum: 21 
        /// </summary>
        /// <param name="GroupHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a group object.
        /// </param>
        /// <param name="GroupInformationClass">
        ///  An enumeration indicating which attributes to update.
        ///  See section  for a listing of possible values.
        /// </param>
        /// <param name="Buffer">
        ///  The requested attributes and values to update. See section
        ///   for structure details.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrSetInformationGroup(
            IntPtr GroupHandle,
            _GROUP_INFORMATION_CLASS GroupInformationClass,
            _SAMPR_GROUP_INFO_BUFFER Buffer)
        {
            const ushort opnum = 21;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pBuffer = TypeMarshal.ToIntPtr(Buffer, GroupInformationClass, null, null);

            paramList = new Int3264[] {
                GroupHandle,
                (int)GroupInformationClass,
                pBuffer,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    retVal = outParamList[3].ToInt32();
                }
            }
            finally
            {
                pBuffer.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrAddMemberToGroup method adds a member to a group.
        ///  Opnum: 22 
        /// </summary>
        /// <param name="GroupHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a group object.
        /// </param>
        /// <param name="MemberId">
        ///  A RID representing an account to add to the group's
        ///  membership list.
        /// </param>
        /// <param name="Attributes">
        ///  The characteristics of the membership relationship.
        ///  See section  for legal values and semantics.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrAddMemberToGroup(IntPtr GroupHandle, uint MemberId, uint Attributes)
        {
            const ushort opnum = 22;
            Int3264[] paramList;
            int retVal = 0;

            paramList = new Int3264[] {
                GroupHandle,
                MemberId,
                Attributes,
                IntPtr.Zero
            };

            using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
            {
                retVal = outParamList[3].ToInt32();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrDeleteGroup method removes a group object. Opnum
        ///  : 23 
        /// </summary>
        /// <param name="GroupHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a group object.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrDeleteGroup(ref IntPtr GroupHandle)
        {
            const ushort opnum = 23;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr ppGroupHandle = TypeMarshal.ToIntPtr(GroupHandle);

            paramList = new Int3264[] {
                ppGroupHandle,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    GroupHandle = Marshal.ReadIntPtr(outParamList[0]);
                    retVal = outParamList[1].ToInt32();
                }
            }
            finally
            {
                ppGroupHandle.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrRemoveMemberFromGroup method removes a member
        ///  from a group. Opnum: 24 
        /// </summary>
        /// <param name="GroupHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a group object.
        /// </param>
        /// <param name="MemberId">
        ///  A RID representing an account to remove from the group's
        ///  membership list.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrRemoveMemberFromGroup(IntPtr GroupHandle, uint MemberId)
        {
            const ushort opnum = 24;
            Int3264[] paramList;
            int retVal = 0;

            paramList = new Int3264[] {
                GroupHandle,
                MemberId,
                IntPtr.Zero
            };

            using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
            {
                retVal = outParamList[2].ToInt32();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrGetMembersInGroup method reads the members of
        ///  a group. Opnum: 25 
        /// </summary>
        /// <param name="GroupHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a group object.
        /// </param>
        /// <param name="Members">
        ///  A structure containing an array of RIDs, as well as
        ///  an array of attribute values.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrGetMembersInGroup(IntPtr GroupHandle, out _SAMPR_GET_MEMBERS_BUFFER? Members)
        {
            const ushort opnum = 25;
            Int3264[] paramList;
            int retVal = 0;

            paramList = new Int3264[] {
                GroupHandle,
                IntPtr.Zero,
                IntPtr.Zero
            };

            using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
            {
                Members = TypeMarshal.ToNullableStruct<_SAMPR_GET_MEMBERS_BUFFER>(
                    Marshal.ReadIntPtr(outParamList[1]));
                retVal = outParamList[2].ToInt32();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrSetMemberAttributesOfGroup method sets the attributes
        ///  of a member relationship. Opnum: 26 
        /// </summary>
        /// <param name="GroupHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a group object.
        /// </param>
        /// <param name="MemberId">
        ///  A RID that represents a member of a group (which is
        ///  a user or machine account).
        /// </param>
        /// <param name="Attributes">
        ///  The characteristics of the membership relationship.
        ///  For legal values, see section.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrSetMemberAttributesOfGroup(IntPtr GroupHandle, uint MemberId, uint Attributes)
        {
            const ushort opnum = 26;
            Int3264[] paramList;
            int retVal = 0;

            paramList = new Int3264[] {
                GroupHandle,
                MemberId,
                Attributes,
                IntPtr.Zero
            };

            using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
            {
                retVal = outParamList[3].ToInt32();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrOpenAlias method obtains a handle to an alias,
        ///  given a RID. Opnum: 27 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="DesiredAccess">
        ///  An ACCESS_MASK that indicates the requested access for
        ///  the returned handle. See section  for a list of alias
        ///  access values.
        /// </param>
        /// <param name="AliasId">
        ///  A RID of an alias.
        /// </param>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrOpenAlias(IntPtr DomainHandle, uint DesiredAccess, uint AliasId, out IntPtr AliasHandle)
        {
            const ushort opnum = 27;
            Int3264[] paramList;
            int retVal = 0;

            paramList = new Int3264[] {
                DomainHandle,
                DesiredAccess,
                AliasId,
                IntPtr.Zero,
                IntPtr.Zero
            };

            using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
            {
                AliasHandle = Marshal.ReadIntPtr(outParamList[3]);
                retVal = outParamList[4].ToInt32();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrQueryInformationAlias method obtains attributes
        ///  from an alias object. Opnum: 28 
        /// </summary>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  an alias object.
        /// </param>
        /// <param name="AliasInformationClass">
        ///  An enumeration indicating which attributes to return.
        ///  See section  for a listing of possible values.
        /// </param>
        /// <param name="Buffer">
        ///  The requested attributes on output. See section  for
        ///  structure details.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrQueryInformationAlias(
            IntPtr AliasHandle,
            _ALIAS_INFORMATION_CLASS AliasInformationClass,
            out _SAMPR_ALIAS_INFO_BUFFER? Buffer)
        {
            const ushort opnum = 28;
            Int3264[] paramList;
            int retVal = 0;

            paramList = new Int3264[] {
                AliasHandle,
                (int)AliasInformationClass,
                IntPtr.Zero,
                IntPtr.Zero
            };

            using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
            {
                Buffer = TypeMarshal.ToNullableStruct<_SAMPR_ALIAS_INFO_BUFFER>(
                    Marshal.ReadIntPtr(outParamList[2]), AliasInformationClass, null, null);
                retVal = outParamList[3].ToInt32();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrSetInformationAlias method  updates attributes
        ///  on an alias object. Opnum: 29 
        /// </summary>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  an alias object.
        /// </param>
        /// <param name="AliasInformationClass">
        ///  An enumeration indicating which attributes to update.
        ///  See section  for a listing of possible values.
        /// </param>
        /// <param name="Buffer">
        ///  The requested attributes and values to update. See section
        ///   for structure details.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrSetInformationAlias(
            IntPtr AliasHandle,
            _ALIAS_INFORMATION_CLASS AliasInformationClass,
            _SAMPR_ALIAS_INFO_BUFFER Buffer)
        {
            const ushort opnum = 29;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pBuffer = TypeMarshal.ToIntPtr(Buffer, AliasInformationClass, null, null);

            paramList = new Int3264[] {
                AliasHandle,
                (int)AliasInformationClass,
                pBuffer,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    retVal = outParamList[3].ToInt32();
                }
            }
            finally
            {
                pBuffer.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrDeleteAlias method removes an alias object.
        ///  Opnum: 30 
        /// </summary>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  an alias object.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrDeleteAlias(ref IntPtr AliasHandle)
        {
            const ushort opnum = 30;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr ppAliasHandle = TypeMarshal.ToIntPtr(AliasHandle);

            paramList = new Int3264[] {
                ppAliasHandle,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    AliasHandle = Marshal.ReadIntPtr(outParamList[0]);
                    retVal = outParamList[1].ToInt32();
                }
            }
            finally
            {
                ppAliasHandle.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrAddMemberToAlias method adds a member to an
        ///  alias. Opnum: 31 
        /// </summary>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  an alias object.
        /// </param>
        /// <param name="MemberId">
        ///  The SID of an account to add to the alias.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrAddMemberToAlias(IntPtr AliasHandle, _RPC_SID MemberId)
        {
            const ushort opnum = 31;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pMemberId = TypeMarshal.ToIntPtr(MemberId);

            paramList = new Int3264[] {
                AliasHandle,
                pMemberId,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    retVal = outParamList[2].ToInt32();
                }
            }
            finally
            {
                pMemberId.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrRemoveMemberFromAlias method removes a member
        ///  from an alias. Opnum: 32 
        /// </summary>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  an alias object.
        /// </param>
        /// <param name="MemberId">
        ///  The SID of an account to remove from the alias.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrRemoveMemberFromAlias(IntPtr AliasHandle, _RPC_SID MemberId)
        {
            const ushort opnum = 32;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pMemberId = TypeMarshal.ToIntPtr(MemberId);

            paramList = new Int3264[] {
                AliasHandle,
                pMemberId,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    retVal = outParamList[2].ToInt32();
                }
            }
            finally
            {
                pMemberId.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrGetMembersInAlias method obtains the membership
        ///  list of an alias. Opnum: 33 
        /// </summary>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  an alias object.
        /// </param>
        /// <param name="Members">
        ///  A structure containing an array of SIDs that represent
        ///  the membership list of the alias referenced by AliasHandle.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrGetMembersInAlias(IntPtr AliasHandle, out _SAMPR_PSID_ARRAY Members)
        {
            const ushort opnum = 33;
            Int3264[] paramList;
            int retVal = 0;

            paramList = new Int3264[] {
                AliasHandle,
                IntPtr.Zero,
                IntPtr.Zero
            };

            using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
            {
                Members = TypeMarshal.ToStruct<_SAMPR_PSID_ARRAY>(outParamList[1]);
                retVal = outParamList[2].ToInt32();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrOpenUser method obtains a handle to a user,
        ///  given a RID. Opnum: 34 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="DesiredAccess">
        ///  An ACCESS_MASK that indicates the requested access for
        ///  the returned handle. See section  for a list of user
        ///  access values.
        /// </param>
        /// <param name="UserId">
        ///  A RID of a user account.
        /// </param>
        /// <param name="UserHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrOpenUser(IntPtr DomainHandle, uint DesiredAccess, uint UserId, out IntPtr UserHandle)
        {
            const ushort opnum = 34;
            Int3264[] paramList;
            int retVal = 0;

            paramList = new Int3264[] {
                DomainHandle,
                DesiredAccess,
                UserId,
                IntPtr.Zero,
                IntPtr.Zero
            };

            using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
            {
                UserHandle = Marshal.ReadIntPtr(outParamList[3]);
                retVal = outParamList[4].ToInt32();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrDeleteUser method removes a user object. Opnum
        ///  : 35 
        /// </summary>
        /// <param name="UserHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a user object.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrDeleteUser(ref IntPtr UserHandle)
        {
            const ushort opnum = 35;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr ppUserHandle = TypeMarshal.ToIntPtr(UserHandle);

            paramList = new Int3264[] {
                ppUserHandle,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    UserHandle = Marshal.ReadIntPtr(outParamList[0]);
                    retVal = outParamList[1].ToInt32();
                }
            }
            finally
            {
                ppUserHandle.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrQueryInformationUser method obtains attributes
        ///  from a user object. Opnum: 36 
        /// </summary>
        /// <param name="UserHandle">
        ///  UserHandle parameter.
        /// </param>
        /// <param name="UserInformationClass">
        ///  UserInformationClass parameter.
        /// </param>
        /// <param name="Buffer">
        ///  Buffer parameter.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrQueryInformationUser(
            IntPtr UserHandle,
            _USER_INFORMATION_CLASS UserInformationClass,
            out _SAMPR_USER_INFO_BUFFER? Buffer)
        {
            const ushort opnum = 36;
            Int3264[] paramList;
            int retVal = 0;

            paramList = new Int3264[] {
                UserHandle,
                (int)UserInformationClass,
                IntPtr.Zero,
                IntPtr.Zero
            };

            using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
            {
                Buffer = TypeMarshal.ToNullableStruct<_SAMPR_USER_INFO_BUFFER>(
                    Marshal.ReadIntPtr(outParamList[2]), UserInformationClass, null, null);
                retVal = outParamList[3].ToInt32();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrSetInformationUser method updates attributes
        ///  on a user object. Opnum: 37 
        /// </summary>
        /// <param name="UserHandle">
        ///  UserHandle parameter.
        /// </param>
        /// <param name="UserInformationClass">
        ///  UserInformationClass parameter.
        /// </param>
        /// <param name="Buffer">
        ///  Buffer parameter.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrSetInformationUser(
            IntPtr UserHandle,
            _USER_INFORMATION_CLASS UserInformationClass,
            _SAMPR_USER_INFO_BUFFER Buffer)
        {
            const ushort opnum = 37;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pBuffer = TypeMarshal.ToIntPtr(Buffer, UserInformationClass, null, null);

            paramList = new Int3264[] {
                UserHandle,
                (int)UserInformationClass,
                pBuffer,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    retVal = outParamList[3].ToInt32();
                }
            }
            finally
            {
                pBuffer.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrChangePasswordUser method changes the password
        ///  of a user object. Opnum: 38 
        /// </summary>
        /// <param name="UserHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a user object.
        /// </param>
        /// <param name="LmPresent">
        ///  If this parameter is zero, the LmOldEncryptedWithLmNew
        ///  and LmNewEncryptedWithLmOld fields MUST be ignored
        ///  by the server; otherwise these fields MUST be processed.
        /// </param>
        /// <param name="OldLmEncryptedWithNewLm">
        ///  The LM hash of the target user's existing password (as
        ///  presented by the client) encrypted according to the
        ///  specification of ENCRYPTED_LM_OWF_PASSWORD, where the
        ///  key is the LM hash of the new password for the target
        ///  user (as presented by the client in the LmNewEncryptedWithLmOld
        ///  parameter).
        /// </param>
        /// <param name="NewLmEncryptedWithOldLm">
        ///  The LM hash of the target user's new password (as presented
        ///  by the client) encrypted according to the specification
        ///  of ENCRYPTED_LM_OWF_PASSWORD, where the key is the
        ///  LM hash of the existing password for the target user
        ///  (as presented by the client in the LmOldEncryptedWithLmNew
        ///  parameter).
        /// </param>
        /// <param name="NtPresent">
        ///  If this parameter is zero, NtOldEncryptedWithNtNew and
        ///  NtNewEncryptedWithNtOld MUST be ignored by the server;
        ///  otherwise these fields MUST be processed. 
        /// </param>
        /// <param name="OldNtEncryptedWithNewNt">
        ///  The NT hash of the target user's existing password (as
        ///  presented by the client) encrypted according to the
        ///  specification of  ENCRYPTED_NT_OWF_PASSWORD, where
        ///  the key is the NT hash of the new password for the
        ///  target user (as presented by the client).
        /// </param>
        /// <param name="NewNtEncryptedWithOldNt">
        ///  The NT hash of the target user's new password (as presented
        ///  by the client) encrypted according to the specification
        ///  of ENCRYPTED_NT_OWF_PASSWORD, where the key is the
        ///  NT hash of the existing password for the target user
        ///  (as presented by the client).
        /// </param>
        /// <param name="NtCrossEncryptionPresent">
        ///  If this parameter is zero, NtNewEncryptedWithLmNew MUST
        ///  be ignored; otherwise, this field MUST be processed.
        /// </param>
        /// <param name="NewNtEncryptedWithNewLm">
        ///  The NT hash of the target user's new password (as presented
        ///  by the client) encrypted according to the specification
        ///  of ENCRYPTED_NT_OWF_PASSWORD, where the key is the
        ///  LM hash of the new password for the target user (as
        ///  presented by the client).
        /// </param>
        /// <param name="LmCrossEncryptionPresent">
        ///  If this parameter is zero, LmNewEncryptedWithNtNew MUST
        ///  be ignored; otherwise, this field MUST be processed.
        /// </param>
        /// <param name="NewLmEncryptedWithNewNt">
        ///  The LM hash of the target user's new password (as presented
        ///  by the client) encrypted according to the specification
        ///  of ENCRYPTED_LM_OWF_PASSWORD, where the key is the
        ///  NT hash of the new password for the target user (as
        ///  presented by the client).
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrChangePasswordUser(
            IntPtr UserHandle,
            byte LmPresent,
            _ENCRYPTED_LM_OWF_PASSWORD? OldLmEncryptedWithNewLm,
            _ENCRYPTED_LM_OWF_PASSWORD? NewLmEncryptedWithOldLm,
            byte NtPresent,
            _ENCRYPTED_LM_OWF_PASSWORD? OldNtEncryptedWithNewNt,
            _ENCRYPTED_LM_OWF_PASSWORD? NewNtEncryptedWithOldNt,
            byte NtCrossEncryptionPresent,
            _ENCRYPTED_LM_OWF_PASSWORD? NewNtEncryptedWithNewLm,
            byte LmCrossEncryptionPresent,
            _ENCRYPTED_LM_OWF_PASSWORD? NewLmEncryptedWithNewNt)
        {
            const ushort opnum = 38;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pOldLmEncryptedWithNewLm = TypeMarshal.ToIntPtr(OldLmEncryptedWithNewLm);
            SafeIntPtr pNewLmEncryptedWithOldLm = TypeMarshal.ToIntPtr(NewLmEncryptedWithOldLm);
            SafeIntPtr pOldNtEncryptedWithNewNt = TypeMarshal.ToIntPtr(OldNtEncryptedWithNewNt);
            SafeIntPtr pNewNtEncryptedWithOldNt = TypeMarshal.ToIntPtr(NewNtEncryptedWithOldNt);
            SafeIntPtr pNewNtEncryptedWithNewLm = TypeMarshal.ToIntPtr(NewNtEncryptedWithNewLm);
            SafeIntPtr pNewLmEncryptedWithNewNt = TypeMarshal.ToIntPtr(NewLmEncryptedWithNewNt);

            paramList = new Int3264[] {
                UserHandle,
                (uint)LmPresent,
                pOldLmEncryptedWithNewLm,
                pNewLmEncryptedWithOldLm,
                (uint)NtPresent,
                pOldNtEncryptedWithNewNt,
                pNewNtEncryptedWithOldNt,
                (uint)NtCrossEncryptionPresent,
                pNewNtEncryptedWithNewLm,
                (uint)LmCrossEncryptionPresent,
                pNewLmEncryptedWithNewNt,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    retVal = outParamList[11].ToInt32();
                }
            }
            finally
            {
                pNewLmEncryptedWithNewNt.Dispose();
                pNewLmEncryptedWithOldLm.Dispose();
                pNewNtEncryptedWithNewLm.Dispose();
                pNewNtEncryptedWithOldNt.Dispose();
                pOldLmEncryptedWithNewLm.Dispose();
                pOldNtEncryptedWithNewNt.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrGetGroupsForUser method obtains a listing of
        ///  groups that a user is a member of. Opnum: 39 
        /// </summary>
        /// <param name="UserHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a user object.
        /// </param>
        /// <param name="Groups">
        ///  An array of RIDs of the groups that the user referenced
        ///  by UserHandle is a member of.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrGetGroupsForUser(IntPtr UserHandle, out _SAMPR_GET_GROUPS_BUFFER? Groups)
        {
            const ushort opnum = 39;
            Int3264[] paramList;
            int retVal = 0;

            paramList = new Int3264[] {
                UserHandle,
                IntPtr.Zero,
                IntPtr.Zero
            };

            using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
            {
                Groups = TypeMarshal.ToNullableStruct<_SAMPR_GET_GROUPS_BUFFER>(
                    Marshal.ReadIntPtr(outParamList[1]));
                retVal = outParamList[2].ToInt32();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrQueryDisplayInformation method obtains a list
        ///  of accounts in name-sorted order, starting at a specified
        ///  index. Opnum: 40 
        /// </summary>
        /// <param name="DomainHandle">
        ///  DomainHandle parameter.
        /// </param>
        /// <param name="DisplayInformationClass">
        ///  DisplayInformationClass parameter.
        /// </param>
        /// <param name="Index">
        ///  Index parameter.
        /// </param>
        /// <param name="EntryCount">
        ///  EntryCount parameter.
        /// </param>
        /// <param name="PreferredMaximumLength">
        ///  PreferredMaximumLength parameter.
        /// </param>
        /// <param name="TotalAvailable">
        ///  TotalAvailable parameter.
        /// </param>
        /// <param name="TotalReturned">
        ///  TotalReturned parameter.
        /// </param>
        /// <param name="Buffer">
        ///  Buffer parameter.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrQueryDisplayInformation(
            IntPtr DomainHandle,
            _DOMAIN_DISPLAY_INFORMATION DisplayInformationClass,
            uint Index,
            uint EntryCount,
            uint PreferredMaximumLength,
            out uint TotalAvailable,
            out uint TotalReturned,
            out _SAMPR_DISPLAY_INFO_BUFFER Buffer)
        {
            const ushort opnum = 40;
            Int3264[] paramList;
            int retVal = 0;

            paramList = new Int3264[] {
                DomainHandle,
                (int)DisplayInformationClass,
                Index,
                EntryCount,
                PreferredMaximumLength,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero
            };

            using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
            {
                TotalAvailable = TypeMarshal.ToStruct<uint>(outParamList[5]);
                TotalReturned = TypeMarshal.ToStruct<uint>(outParamList[6]);
                Buffer = TypeMarshal.ToStruct<_SAMPR_DISPLAY_INFO_BUFFER>(
                    outParamList[7], DisplayInformationClass, null, null);
                retVal = outParamList[8].ToInt32();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrGetDisplayEnumerationIndex method obtains an
        ///  index into an account-namesorted list of accounts.
        ///  Opnum: 41 
        /// </summary>
        /// <param name="DomainHandle">
        ///  DomainHandle parameter.
        /// </param>
        /// <param name="DisplayInformationClass">
        ///  DisplayInformationClass parameter.
        /// </param>
        /// <param name="Prefix">
        ///  Prefix parameter.
        /// </param>
        /// <param name="Index">
        ///  Index parameter.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrGetDisplayEnumerationIndex(
            IntPtr DomainHandle,
            _DOMAIN_DISPLAY_INFORMATION DisplayInformationClass,
            _RPC_UNICODE_STRING Prefix,
            out uint Index)
        {
            const ushort opnum = 41;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pPrefix = TypeMarshal.ToIntPtr(Prefix);

            paramList = new Int3264[] {
                DomainHandle,
                (int)DisplayInformationClass,
                pPrefix,
                IntPtr.Zero,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    Index = TypeMarshal.ToStruct<uint>(outParamList[3]);
                    retVal = outParamList[4].ToInt32();
                }
            }
            finally
            {
                pPrefix.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  Reserved for local use. Opnum: 42 
        /// </summary>
        public void Opnum42NotUsedOnWire()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///  Reserved for local use. Opnum: 43 
        /// </summary>
        public void Opnum43NotUsedOnWire()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///  The SamrGetUserDomainPasswordInformation method obtains
        ///  select password policy information (without requiring
        ///  a domain handle). Opnum: 44 
        /// </summary>
        /// <param name="UserHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a user object.
        /// </param>
        /// <param name="PasswordInformation">
        ///  Password policy information from the user's domain.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrGetUserDomainPasswordInformation(
            IntPtr UserHandle,
            out _USER_DOMAIN_PASSWORD_INFORMATION PasswordInformation)
        {
            const ushort opnum = 44;
            Int3264[] paramList;
            int retVal = 0;

            paramList = new Int3264[] {
                UserHandle,
                IntPtr.Zero,
                IntPtr.Zero
            };

            using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
            {
                PasswordInformation = TypeMarshal.ToStruct<_USER_DOMAIN_PASSWORD_INFORMATION>(outParamList[1]);
                retVal = outParamList[2].ToInt32();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrRemoveMemberFromForeignDomain method removes
        ///  a member from all aliases. Opnum: 45 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="MemberSid">
        ///  The SID to remove from the membership.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrRemoveMemberFromForeignDomain(IntPtr DomainHandle, _RPC_SID? MemberSid)
        {
            const ushort opnum = 45;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pMemberSid = TypeMarshal.ToIntPtr(MemberSid);

            paramList = new Int3264[] {
                DomainHandle,
                pMemberSid,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    retVal = outParamList[2].ToInt32();
                }
            }
            finally
            {
                pMemberSid.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrQueryInformationDomain2 method obtains attributes
        ///  from a domain object. Opnum: 46 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="DomainInformationClass">
        ///  An enumeration indicating which attributes to return.
        ///  See section  for a listing of possible values.
        /// </param>
        /// <param name="Buffer">
        ///  The requested attributes on output. See section  for
        ///  structure details.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrQueryInformationDomain2(
            IntPtr DomainHandle,
            _DOMAIN_INFORMATION_CLASS DomainInformationClass,
            out _SAMPR_DOMAIN_INFO_BUFFER? Buffer)
        {
            const ushort opnum = 46;
            Int3264[] paramList;
            int retVal = 0;

            paramList = new Int3264[] {
                DomainHandle,
                (int)DomainInformationClass,
                IntPtr.Zero,
                IntPtr.Zero
            };

            using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
            {
                Buffer = TypeMarshal.ToNullableStruct<_SAMPR_DOMAIN_INFO_BUFFER>(
                    Marshal.ReadIntPtr(outParamList[2]), DomainInformationClass, null, null);
                retVal = outParamList[3].ToInt32();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrQueryInformationUser2 method obtains attributes
        ///  from a user object. Opnum: 47 
        /// </summary>
        /// <param name="UserHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a user object.
        /// </param>
        /// <param name="UserInformationClass">
        ///  An enumeration indicating which attributes to return.
        ///  See section  for a list of possible values.
        /// </param>
        /// <param name="Buffer">
        ///  The requested attributes on output. See section  for
        ///  structure details.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrQueryInformationUser2(
            IntPtr UserHandle,
            _USER_INFORMATION_CLASS UserInformationClass,
            out _SAMPR_USER_INFO_BUFFER? Buffer)
        {
            const ushort opnum = 47;
            Int3264[] paramList;
            int retVal = 0;

            paramList = new Int3264[] {
                UserHandle,
                (int)UserInformationClass,
                IntPtr.Zero,
                IntPtr.Zero
            };

            using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
            {
                Buffer = TypeMarshal.ToNullableStruct<_SAMPR_USER_INFO_BUFFER>(
                    Marshal.ReadIntPtr(outParamList[2]), UserInformationClass, null, null);
                retVal = outParamList[3].ToInt32();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrQueryDisplayInformation2 method obtains a list
        ///  of accounts in name-sorted order, starting at a specified
        ///  index. Opnum: 48 
        /// </summary>
        /// <param name="DomainHandle">
        ///  DomainHandle parameter.
        /// </param>
        /// <param name="DisplayInformationClass">
        ///  DisplayInformationClass parameter.
        /// </param>
        /// <param name="Index">
        ///  Index parameter.
        /// </param>
        /// <param name="EntryCount">
        ///  EntryCount parameter.
        /// </param>
        /// <param name="PreferredMaximumLength">
        ///  PreferredMaximumLength parameter.
        /// </param>
        /// <param name="TotalAvailable">
        ///  TotalAvailable parameter.
        /// </param>
        /// <param name="TotalReturned">
        ///  TotalReturned parameter.
        /// </param>
        /// <param name="Buffer">
        ///  Buffer parameter.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrQueryDisplayInformation2(
            IntPtr DomainHandle,
            _DOMAIN_DISPLAY_INFORMATION DisplayInformationClass,
            uint Index,
            uint EntryCount,
            uint PreferredMaximumLength,
            out uint TotalAvailable,
            out uint TotalReturned,
            out _SAMPR_DISPLAY_INFO_BUFFER Buffer)
        {
            const ushort opnum = 48;
            Int3264[] paramList;
            int retVal = 0;

            paramList = new Int3264[] {
                DomainHandle,
                (int)DisplayInformationClass,
                Index,
                EntryCount,
                PreferredMaximumLength,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero
            };

            using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
            {
                TotalAvailable = TypeMarshal.ToStruct<uint>(outParamList[5]);
                TotalReturned = TypeMarshal.ToStruct<uint>(outParamList[6]);
                Buffer = TypeMarshal.ToStruct<_SAMPR_DISPLAY_INFO_BUFFER>(
                    outParamList[7], DisplayInformationClass, null, null);
                retVal = outParamList[8].ToInt32();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrGetDisplayEnumerationIndex2 method obtains an
        ///  index into an account-namesorted list of accounts,
        ///  such that the index is the position in the list of
        ///  the accounts whose account name best matches a client-provided
        ///  string. Opnum: 49 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="DisplayInformationClass">
        ///  An enumeration indicating which set of objects to return
        ///  an index into (for a subsequent SamrQueryDisplayInformation3
        ///  method call).
        /// </param>
        /// <param name="Prefix">
        ///  A string matched against the account name to find a
        ///  starting point for an enumeration. The Prefix parameter
        ///  enables the client to obtain a listing of an account
        ///  from SamrQueryDisplayInformation3  such that the accounts
        ///  are returned in alphabetical order with respect to
        ///  their account name, starting with the account name
        ///  that most closely matches Prefix. See details later
        ///  in this section.
        /// </param>
        /// <param name="Index">
        ///  A value to use as input to SamrQueryDisplayInformation3
        ///   in order to control the accounts that are returned
        ///  from that method.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrGetDisplayEnumerationIndex2(
            IntPtr DomainHandle,
            _DOMAIN_DISPLAY_INFORMATION DisplayInformationClass,
            _RPC_UNICODE_STRING Prefix,
            out uint Index)
        {
            const ushort opnum = 49;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pPrefix = TypeMarshal.ToIntPtr(Prefix);

            paramList = new Int3264[] {
                DomainHandle,
                (int)DisplayInformationClass,
                pPrefix,
                IntPtr.Zero,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    Index = TypeMarshal.ToStruct<uint>(outParamList[3]);
                    retVal = outParamList[4].ToInt32();
                }
            }
            finally
            {
                pPrefix.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrCreateUser2InDomain method creates a user. Opnum
        ///  : 50 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="Name">
        ///  The value to use as the name of the user. See the message
        ///  processing shown later in this section for details
        ///  on how this value maps to the data model.
        /// </param>
        /// <param name="AccountType">
        ///  A 32-bit value indicating the type of account to create.
        ///  See the message processing shown later in this section
        ///  for possible values.
        /// </param>
        /// <param name="DesiredAccess">
        ///  The access requested on the UserHandle on output. See
        ///  section  for a listing of possible values.
        /// </param>
        /// <param name="UserHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <param name="GrantedAccess">
        ///  The access granted on UserHandle.
        /// </param>
        /// <param name="RelativeId">
        ///  The RID of the newly created user.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrCreateUser2InDomain(
            IntPtr DomainHandle,
            _RPC_UNICODE_STRING? Name,
            uint AccountType,
            uint DesiredAccess,
            out IntPtr UserHandle,
            out uint GrantedAccess,
            out uint RelativeId)
        {
            const ushort opnum = 50;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pName = TypeMarshal.ToIntPtr(Name);

            paramList = new Int3264[] {
                DomainHandle,
                pName,
                AccountType,
                DesiredAccess,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    UserHandle = Marshal.ReadIntPtr(outParamList[4]);
                    GrantedAccess = TypeMarshal.ToStruct<uint>(outParamList[5]);
                    RelativeId = TypeMarshal.ToStruct<uint>(outParamList[6]);
                    retVal = outParamList[7].ToInt32();
                }
            }
            finally
            {
                pName.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrQueryDisplayInformation3 method obtains a listing
        ///  of accounts in name-sorted order, starting at a specified
        ///  index. Opnum: 51 
        /// </summary>
        /// <param name="DomainHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a domain object.
        /// </param>
        /// <param name="DisplayInformationClass">
        ///  An enumeration (see section) that indicates the type
        ///  of accounts, as well as the type of attributes on the
        ///  accounts, to return via the Buffer parameter.
        /// </param>
        /// <param name="Index">
        ///  A cursor into an account-namesorted list of accounts.
        /// </param>
        /// <param name="EntryCount">
        ///  The number of accounts that the client is requesting
        ///  on output.
        /// </param>
        /// <param name="PreferredMaximumLength">
        ///  The requested maximum number of bytes to return in Buffer;
        ///  this value overrides EntryCount if this value is reached
        ///  before EntryCount is reached.
        /// </param>
        /// <param name="TotalAvailable">
        ///  The number of bytes required to see a complete listing
        ///  of accounts specified by the DisplayInformationClass
        ///  parameter.
        /// </param>
        /// <param name="TotalReturned">
        ///  The number of bytes returned. This value is estimated
        ///  and is not accurate.  clients do not rely on the accuracy
        ///  of this value.
        /// </param>
        /// <param name="Buffer">
        ///  The accounts that are returned.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrQueryDisplayInformation3(
            IntPtr DomainHandle,
            _DOMAIN_DISPLAY_INFORMATION DisplayInformationClass,
            uint Index,
            uint EntryCount,
            uint PreferredMaximumLength,
            out uint TotalAvailable,
            out uint TotalReturned,
            out _SAMPR_DISPLAY_INFO_BUFFER Buffer)
        {
            const ushort opnum = 51;
            Int3264[] paramList;
            int retVal = 0;

            paramList = new Int3264[] {
                DomainHandle,
                (int)DisplayInformationClass,
                Index,
                EntryCount,
                PreferredMaximumLength,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero
            };

            using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
            {
                TotalAvailable = TypeMarshal.ToStruct<uint>(outParamList[5]);
                TotalReturned = TypeMarshal.ToStruct<uint>(outParamList[6]);
                Buffer = TypeMarshal.ToStruct<_SAMPR_DISPLAY_INFO_BUFFER>(
                    outParamList[7], DisplayInformationClass, null, null);
                retVal = outParamList[8].ToInt32();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrAddMultipleMembersToAlias method adds multiple
        ///  members to an alias. Opnum: 52 
        /// </summary>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  an alias object.
        /// </param>
        /// <param name="MembersBuffer">
        ///  A structure containing a list of SIDs to add as members
        ///  to the alias.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrAddMultipleMembersToAlias(IntPtr AliasHandle, _SAMPR_PSID_ARRAY? MembersBuffer)
        {
            const ushort opnum = 52;
            Int3264[] paramList;
            int retVal = 0;
            SafeIntPtr pMembersBuffer = TypeMarshal.ToIntPtr(MembersBuffer);

            paramList = new Int3264[] {
                AliasHandle,
                pMembersBuffer,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    retVal = outParamList[2].ToInt32();
                }
            }
            finally
            {
                pMembersBuffer.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrRemoveMultipleMembersFromAlias method removes
        ///  multiple members from an alias. Opnum: 53 
        /// </summary>
        /// <param name="AliasHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  an alias object.
        /// </param>
        /// <param name="MembersBuffer">
        ///  A structure containing a list of SIDs to remove from
        ///  the alias's membership list.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrRemoveMultipleMembersFromAlias(IntPtr AliasHandle, _SAMPR_PSID_ARRAY? MembersBuffer)
        {
            const ushort opnum = 53;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pMembersBuffer = TypeMarshal.ToIntPtr(MembersBuffer);

            paramList = new Int3264[] {
                AliasHandle,
                pMembersBuffer,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    retVal = outParamList[2].ToInt32();
                }
            }
            finally
            {
                pMembersBuffer.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrOemChangePasswordUser2 method changes a user's
        ///  password.  Opnum: 54 
        /// </summary>
        /// <param name="BindingHandle">
        ///  An RPC binding handle parameter as specified in [C706-Ch2Intro].
        /// </param>
        /// <param name="ServerName">
        ///  A counted string, encoded in the OEM character set,
        ///  containing the NETBIOS name of the server; this parameter
        ///  MAY servers ignore the ServerName parameter.  be ignored
        ///  by the server.
        /// </param>
        /// <param name="UserName">
        ///  A counted string, encoded in the OEM character set,
        ///  containing the name of the user whose password is to
        ///  be changed; see message processing later in this section
        ///  for details on how this value is used as a database
        ///  key to locate the account that is the target of this
        ///  password change operation.
        /// </param>
        /// <param name="NewPasswordEncryptedWithOldLm">
        ///  A clear text password encrypted according to the specification
        ///  of SAMPR_ENCRYPTED_USER_PASSWORD, where the key is
        ///  the LM hash of the existing password for the target
        ///  user (as presented by the client). The clear text password
        ///  MUST be encoded in an OEM code page character set (as
        ///  opposed to UTF-16).
        /// </param>
        /// <param name="OldLmOwfPasswordEncryptedWithNewLm">
        ///  The LM hash of the target user's existing password (as
        ///  presented by the client) encrypted according to the
        ///  specification of ENCRYPTED_LM_OWF_PASSWORD, where the
        ///  key is the LM hash of the clear text password obtained
        ///  from decrypting NewPasswordEncryptedWithOldLm (see
        ///  the preceding description for decryption details).
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrOemChangePasswordUser2(
            System.IntPtr BindingHandle,
            [Indirect()] _RPC_STRING ServerName,
            [Indirect()] _RPC_STRING UserName,
            [Indirect()] _SAMPR_ENCRYPTED_USER_PASSWORD NewPasswordEncryptedWithOldLm,
            [Indirect()] _ENCRYPTED_LM_OWF_PASSWORD OldLmOwfPasswordEncryptedWithNewLm)
        {
            const ushort opnum = 54;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pServerName = TypeMarshal.ToIntPtr(ServerName);
            SafeIntPtr pUserName = TypeMarshal.ToIntPtr(UserName);
            SafeIntPtr pNewPasswordEncryptedWithOldLm = TypeMarshal.ToIntPtr(NewPasswordEncryptedWithOldLm);
            SafeIntPtr pOldLmOwfPasswordEncryptedWithNewLm = TypeMarshal.ToIntPtr(OldLmOwfPasswordEncryptedWithNewLm);

            paramList = new Int3264[] {
                pServerName,
                pUserName,
                pNewPasswordEncryptedWithOldLm,
                pOldLmOwfPasswordEncryptedWithNewLm,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    retVal = outParamList[4].ToInt32();
                }
            }
            finally
            {
                pServerName.Dispose();
                pUserName.Dispose();
                pNewPasswordEncryptedWithOldLm.Dispose();
                pOldLmOwfPasswordEncryptedWithNewLm.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrUnicodeChangePasswordUser2 method changes a
        ///  user account's password. Opnum: 55 
        /// </summary>
        /// <param name="BindingHandle">
        ///  An RPC binding handle parameter as specified in [C706-Ch2Intro].
        /// </param>
        /// <param name="ServerName">
        ///  A null-terminated string containing the NETBIOS name
        ///  of the server; this parameter MAY servers ignore the
        ///  ServerName parameter.  be ignored by the server.
        /// </param>
        /// <param name="UserName">
        ///  The name of the user. See the message processing later
        ///  in this section for details on how this value is used
        ///  as a database key to locate the account that is the
        ///  target of this password change operation.
        /// </param>
        /// <param name="NewPasswordEncryptedWithOldNt">
        ///  A clear text password encrypted according to the specification
        ///  of SAMPR_ENCRYPTED_USER_PASSWORD, where the key is
        ///  the NT hash of the existing password for the target
        ///  user (as presented by the client in the OldNtOwfPasswordEncryptedWithNewNt
        ///  parameter). 
        /// </param>
        /// <param name="OldNtOwfPasswordEncryptedWithNewNt">
        ///  The NT hash of the target user's existing password (as
        ///  presented by the client) encrypted according to the
        ///  specification of ENCRYPTED_LM_OWF_PASSWORD, where the
        ///  key is the NT hash of the clear text password obtained
        ///  from decrypting NewPasswordEncryptedWithOldNt.
        /// </param>
        /// <param name="LmPresent">
        ///  If this parameter is zero, NewPasswordEncryptedWithOldLm
        ///  and OldLmOwfPasswordEncryptedWithOldLm MUST be ignored;
        ///  otherwise these fields MUST be processed.
        /// </param>
        /// <param name="NewPasswordEncryptedWithOldLm">
        ///  A clear text password encrypted according to the specification
        ///  of SAMPR_ENCRYPTED_USER_PASSWORD, where the key is
        ///  the LM hash of the existing password for the target
        ///  user (as presented by the client).
        /// </param>
        /// <param name="OldLmOwfPasswordEncryptedWithNewNt">
        ///  The LM hash the target user's existing password (as
        ///  presented by the client) encrypted according to the
        ///  specification of ENCRYPTED_LM_OWF_PASSWORD, where the
        ///  key is the NT hash of the clear text password obtained
        ///  from decrypting NewPasswordEncryptedWithOldNt.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrUnicodeChangePasswordUser2(
            System.IntPtr BindingHandle,
            [Indirect()] _RPC_UNICODE_STRING ServerName,
            [Indirect()] _RPC_UNICODE_STRING UserName,
            [Indirect()] _SAMPR_ENCRYPTED_USER_PASSWORD NewPasswordEncryptedWithOldNt,
            [Indirect()] _ENCRYPTED_LM_OWF_PASSWORD OldNtOwfPasswordEncryptedWithNewNt,
            byte LmPresent,
            [Indirect()] _SAMPR_ENCRYPTED_USER_PASSWORD NewPasswordEncryptedWithOldLm,
            [Indirect()] _ENCRYPTED_LM_OWF_PASSWORD OldLmOwfPasswordEncryptedWithNewNt)
        {
            const ushort opnum = 55;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pServerName = TypeMarshal.ToIntPtr(ServerName);
            SafeIntPtr pUserName = TypeMarshal.ToIntPtr(UserName);
            SafeIntPtr pNewPasswordEncryptedWithOldNt = TypeMarshal.ToIntPtr(NewPasswordEncryptedWithOldNt);
            SafeIntPtr pOldNtOwfPasswordEncryptedWithNewNt = TypeMarshal.ToIntPtr(OldNtOwfPasswordEncryptedWithNewNt);
            SafeIntPtr pNewPasswordEncryptedWithOldLm = TypeMarshal.ToIntPtr(NewPasswordEncryptedWithOldLm);
            SafeIntPtr pOldLmOwfPasswordEncryptedWithNewNt = TypeMarshal.ToIntPtr(OldLmOwfPasswordEncryptedWithNewNt);

            paramList = new Int3264[] {
                pServerName,
                pUserName,
                pNewPasswordEncryptedWithOldNt,
                pOldNtOwfPasswordEncryptedWithNewNt,
                (uint)LmPresent,
                pNewPasswordEncryptedWithOldLm,
                pOldLmOwfPasswordEncryptedWithNewNt,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    retVal = outParamList[7].ToInt32();
                }
            }
            finally
            {
                pServerName.Dispose();
                pUserName.Dispose();
                pNewPasswordEncryptedWithOldNt.Dispose();
                pOldNtOwfPasswordEncryptedWithNewNt.Dispose();
                pNewPasswordEncryptedWithOldLm.Dispose();
                pOldLmOwfPasswordEncryptedWithNewNt.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrGetDomainPasswordInformation method obtains
        ///  select password policy information (without authenticating
        ///  to the server). Opnum: 56 
        /// </summary>
        /// <param name="BindingHandle">
        ///  An RPC binding handle parameter, as specified in [C706-Ch2Intro].
        /// </param>
        /// <param name="Unused">
        ///  A string value that is unused by the protocol. It is
        ///  ignored by the server. The client MAY clients set this
        ///  value to be the NULL-terminated NETBIOS name of the
        ///  server. set any value.
        /// </param>
        /// <param name="PasswordInformation">
        ///  Password policy information from the account domain.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrGetDomainPasswordInformation(
            System.IntPtr BindingHandle,
            [Indirect()] _RPC_UNICODE_STRING Unused,
            out _USER_DOMAIN_PASSWORD_INFORMATION PasswordInformation)
        {
            const ushort opnum = 56;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pUnused = TypeMarshal.ToIntPtr(Unused);

            paramList = new Int3264[] {
                pUnused,
                IntPtr.Zero,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    PasswordInformation = TypeMarshal.ToStruct<_USER_DOMAIN_PASSWORD_INFORMATION>(outParamList[1]);
                    retVal = outParamList[2].ToInt32();
                }
            }
            finally
            {
                pUnused.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrConnect2 method returns a handle to a server
        ///  object. Opnum: 57 
        /// </summary>
        /// <param name="ServerName">
        ///  The null-terminated NETBIOS name of the server; this
        ///  parameter MAYServerName is ignored on receipt.  be
        ///  ignored on receipt.
        /// </param>
        /// <param name="ServerHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <param name="DesiredAccess">
        ///  An ACCESS_MASK that indicates the access requested for
        ///  ServerHandle on output. See section  for a listing
        ///  of possible values.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrConnect2(string ServerName, out System.IntPtr ServerHandle, uint DesiredAccess)
        {
            const ushort opnum = 57;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pServerName = Marshal.StringToHGlobalUni(ServerName);

            paramList = new Int3264[] {
                pServerName,
                IntPtr.Zero,
                DesiredAccess,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    ServerHandle = Marshal.ReadIntPtr(outParamList[1]);
                    retVal = outParamList[3].ToInt32();
                }
            }
            finally
            {
                pServerName.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrSetInformationUser2 method updates attributes
        ///  on a user object. Opnum: 58 
        /// </summary>
        /// <param name="UserHandle">
        ///  An RPC context handle, as specified in section , representing
        ///  a user object.
        /// </param>
        /// <param name="UserInformationClass">
        ///  An enumeration indicating which attributes to update.
        ///  See section  for a listing of possible values.
        /// </param>
        /// <param name="Buffer">
        ///  The requested attributes and values to update. See section
        ///   for structure details.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrSetInformationUser2(
            System.IntPtr UserHandle,
            _USER_INFORMATION_CLASS UserInformationClass,
            [Switch("UserInformationClass")] _SAMPR_USER_INFO_BUFFER Buffer)
        {
            const ushort opnum = 58;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pBuffer = TypeMarshal.ToIntPtr(Buffer, UserInformationClass, null, null);

            paramList = new Int3264[] {
                UserHandle,
                (int)UserInformationClass,
                pBuffer,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    retVal = outParamList[3].ToInt32();
                }
            }
            finally
            {
                pBuffer.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  Reserved for local use. Opnum: 59 
        /// </summary>
        public void Opnum59NotUsedOnWire()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///  Reserved for local use. Opnum: 60 
        /// </summary>
        public void Opnum60NotUsedOnWire()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///  Reserved for local use. Opnum: 61 
        /// </summary>
        public void Opnum61NotUsedOnWire()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///  The SamrConnect4 method obtains a handle to a server
        ///  object. Opnum: 62 
        /// </summary>
        /// <param name="ServerName">
        ///  The null-terminated NETBIOS name of the server; this
        ///  parameter MAYServerName is ignored on receipt.  be
        ///  ignored on receipt.
        /// </param>
        /// <param name="ServerHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <param name="ClientRevision">
        ///  Indicates the revision (for this protocol) of the client.
        ///  See the Revision field of SAMPR_REVISION_INFO_V1 for
        ///  possible values.
        /// </param>
        /// <param name="DesiredAccess">
        ///  An ACCESS_MASK that indicates the access requested for
        ///  ServerHandle on output. See section  for a listing
        ///  of possible values.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrConnect4(
            string ServerName,
            out System.IntPtr ServerHandle,
            uint ClientRevision,
            uint DesiredAccess)
        {
            const ushort opnum = 62;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pServerName = Marshal.StringToHGlobalUni(ServerName);

            paramList = new Int3264[] {
                pServerName,
                IntPtr.Zero,
                ClientRevision,
                DesiredAccess,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    ServerHandle = Marshal.ReadIntPtr(outParamList[1]);
                    retVal = outParamList[4].ToInt32();
                }
            }
            finally
            {
                pServerName.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  Reserved for local use. Opnum: 63 
        /// </summary>
        public void Opnum63NotUsedOnWire()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///  The SamrConnect5 method obtains a handle to a server
        ///  object. Opnum: 64 
        /// </summary>
        /// <param name="ServerName">
        ///  The null-terminated NETBIOS name of the server; this
        ///  parameter MAYServerName is ignored on receipt.  be
        ///  ignored on receipt.
        /// </param>
        /// <param name="DesiredAccess">
        ///  An ACCESS_MASK that indicates the access requested for
        ///  ServerHandle on output. For a listing of possible values,
        ///  see section.
        /// </param>
        /// <param name="InVersion">
        ///  Indicates which field of the InRevisionInfo union is
        ///  used.
        /// </param>
        /// <param name="InRevisionInfo">
        ///  Revision information. For details, see the definition
        ///  of the SAMPR_REVISION_INFO_V1 structure, which is contained
        ///  in the SAMPR_REVISION_INFO union.
        /// </param>
        /// <param name="OutVersion">
        ///  Indicates which field of the OutRevisionInfo union is
        ///  used.
        /// </param>
        /// <param name="OutRevisionInfo">
        ///  Revision information. For details, see the definition
        ///  of the SAMPR_REVISION_INFO_V1 structure, which is contained
        ///  in the SAMPR_REVISION_INFO union.
        /// </param>
        /// <param name="ServerHandle">
        ///  An RPC context handle, as specified in section.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrConnect5(
            [String()] string ServerName,
            uint DesiredAccess,
            uint InVersion,
            [Switch("InVersion")] SAMPR_REVISION_INFO InRevisionInfo,
            out System.UInt32 OutVersion,
            [Switch("*OutVersion")] out SAMPR_REVISION_INFO OutRevisionInfo,
            out System.IntPtr ServerHandle)
        {
            const ushort opnum = 64;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pServerName = Marshal.StringToHGlobalUni(ServerName);
            SafeIntPtr pInRevisionInfo = TypeMarshal.ToIntPtr(InRevisionInfo, InVersion, null, null);

            paramList = new Int3264[] {
                pServerName,
                DesiredAccess,
                InVersion,
                pInRevisionInfo,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    OutVersion = TypeMarshal.ToStruct<uint>(outParamList[4]);
                    OutRevisionInfo = TypeMarshal.ToStruct<SAMPR_REVISION_INFO>(
                        outParamList[5], OutVersion, null, null);
                    ServerHandle = Marshal.ReadIntPtr(outParamList[6]);
                    retVal = outParamList[7].ToInt32();
                }
            }
            finally
            {
                pServerName.Dispose();
                pInRevisionInfo.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrRidToSid method obtains the SID of an account,
        ///  given a RID. Opnum: 65 
        /// </summary>
        /// <param name="ObjectHandle">
        ///  An RPC context handle, as specified in section. The
        ///  message processing shown later in this section contains
        ///  details on which types of ObjectHandle are accepted
        ///  by the server.
        /// </param>
        /// <param name="Rid">
        ///  A RID of an account.
        /// </param>
        /// <param name="Sid">
        ///  The SID of the account referenced by Rid.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrRidToSid(System.IntPtr ObjectHandle, uint Rid, out _RPC_SID? Sid)
        {
            const ushort opnum = 65;
            Int3264[] paramList;
            int retVal = 0;

            paramList = new Int3264[] {
                ObjectHandle,
                Rid,
                IntPtr.Zero,
                IntPtr.Zero
            };

            using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
            {
                Sid = TypeMarshal.ToNullableStruct<_RPC_SID>(Marshal.ReadIntPtr(outParamList[2]));
                retVal = outParamList[3].ToInt32();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrSetDSRMPassword method sets a local recovery
        ///  password. Opnum: 66 
        /// </summary>
        /// <param name="BindingHandle">
        ///  An RPC binding handle parameter, as specified in [C706-Ch2Intro].
        /// </param>
        /// <param name="Unused">
        ///  A string value. This value is not used in the protocol
        ///  and is ignored by the server.
        /// </param>
        /// <param name="UserId">
        ///  A RID of a user account. See the message processing
        ///  later in this section for details on restrictions on
        ///  this value.
        /// </param>
        /// <param name="EncryptedNtOwfPassword">
        ///  The NT hash of the new password (as presented by the
        ///  client) encrypted according to the specification of
        ///  ENCRYPTED_NT_OWF_PASSWORD, where the key is the User ID.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrSetDSRMPassword(
            System.IntPtr BindingHandle,
            [Indirect()] _RPC_UNICODE_STRING Unused,
            uint UserId,
            [Indirect()] _ENCRYPTED_LM_OWF_PASSWORD EncryptedNtOwfPassword)
        {
            const ushort opnum = 66;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pUnused = TypeMarshal.ToIntPtr(Unused);
            SafeIntPtr pEncryptedNtOwfPassword = TypeMarshal.ToIntPtr(EncryptedNtOwfPassword);

            paramList = new Int3264[] {
                pUnused,
                UserId,
                pEncryptedNtOwfPassword,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    retVal = outParamList[3].ToInt32();
                }
            }
            finally
            {
                pUnused.Dispose();
                pEncryptedNtOwfPassword.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  The SamrValidatePassword method validates an application
        ///  password against the locally stored policy. Opnum :
        ///  67 
        /// </summary>
        /// <param name="Handle">
        ///  An RPC binding handle parameter, as specified in [C706-Ch2Intro].
        /// </param>
        /// <param name="ValidationType">
        ///  The password policy validation requested.
        /// </param>
        /// <param name="InputArg">
        ///  The password-related material to validate.
        /// </param>
        /// <param name="OutputArg">
        ///  The result of the validation.
        /// </param>
        /// <returns>
        /// status of the function call, for example: 0 indicates STATUS_SUCCESS
        /// </returns>
        public int SamrValidatePassword(
            System.IntPtr Handle,
            _PASSWORD_POLICY_VALIDATION_TYPE ValidationType,
            _SAM_VALIDATE_INPUT_ARG InputArg,
            out _SAM_VALIDATE_OUTPUT_ARG? OutputArg)
        {
            const ushort opnum = 67;
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pInputArg = TypeMarshal.ToIntPtr(InputArg, ValidationType, null, null);

            paramList = new Int3264[] {
                (int)ValidationType,
                pInputArg,
                IntPtr.Zero,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, opnum))
                {
                    OutputArg = TypeMarshal.ToNullableStruct<_SAM_VALIDATE_OUTPUT_ARG>(
                        Marshal.ReadIntPtr(outParamList[2]), ValidationType, null, null);
                    retVal = outParamList[3].ToInt32();
                }
            }
            finally
            {
                pInputArg.Dispose();
            }

            return retVal;
        }


        /// <summary>
        ///  Reserved for local use. Opnum: 68 
        /// </summary>
        public void Opnum68NotUsedOnWire()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///  Reserved for local use. Opnum: 69 
        /// </summary>
        public void Opnum69NotUsedOnWire()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Disposes rpc adapter.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
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
                if (rpceClientTransport != null)
                {
                    rpceClientTransport.Dispose();
                    rpceClientTransport = null;
                }
            }
        }


        /// <summary>
        /// finalizer
        /// </summary>
        ~SamrRpcAdapter()
        {
            Dispose(false);
        }
        #endregion
    }
}
