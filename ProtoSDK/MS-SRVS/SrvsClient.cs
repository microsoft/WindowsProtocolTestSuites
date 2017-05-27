// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Srvs
{
    public class SrvsClient : IDisposable
    {
        #region Properties

        internal RpceClientTransport RpceClientTransport { get; set; }

        public TimeSpan RpceTimeout { get; private set; }

        public RpceAuthenticationLevel AuthenticationLevel { get; private set; }

        public IntPtr Handle
        {
            get { return RpceClientTransport.Handle; }
        }

        #endregion

        #region Constructor

        public SrvsClient(TimeSpan timeout)
        {
            RpceTimeout = timeout;
            AuthenticationLevel = RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_NONE;
        }

        #endregion

        #region IDisposable members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (RpceClientTransport != null)
                {
                    RpceClientTransport.Dispose();
                    RpceClientTransport = null;
                }
            }
        }

        ~SrvsClient()
        {
            Dispose(false);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Set up an RPC session with a specific server.
        /// </summary>
        /// <param name="serverName">Server computer name</param>
        /// <param name="credential">User account used to setup this session</param>
        /// <param name="securityContext">Security context of session</param>
        public void Bind(
            string serverName,
            AccountCredential credential,
            ClientSecurityContext securityContext)
        {
            if (string.IsNullOrEmpty(serverName))
            {
                throw new ArgumentNullException("serverName");
            }

            InnerBind(
                RpceUtility.RPC_OVER_NAMED_PIPE_PROTOCOL_SEQUENCE,
                serverName,
                SrvsUtility.SRVS_NAMED_PIPE,
                credential,
                securityContext);
        }

        /// <summary>
        /// Unbind and release the handle.
        /// </summary>
        public void UnBind()
        {
            if (RpceClientTransport != null)
            {
                RpceClientTransport.Unbind(RpceTimeout);
                RpceClientTransport.Dispose();
                RpceClientTransport = null;
            }
        }

        /// <summary>
        /// The NetrShareEnum method retrieves information about each shared resource on a server.
        /// </summary>
        /// <param name="serverName">A string that identifies the server. If this parameter is NULL, the local computer is used.</param>
        /// <param name="infoStruct">A SHARE_ENUM_STRUCT structure. The SHARE_ENUM_STRUCT structure has a Level member 
        /// that specifies the type of structure to return in the ShareInfo member.</param>
        /// <param name="PreferedMaximumLength">Specifies the preferred maximum length, in bytes, of the returned data. 
        /// If the specified value is MAX_PREFERRED_LENGTH, the method MUST attempt to return all entries.</param>
        /// <param name="TotalEntries">The total number of entries that could have been enumerated if the buffer had been big enough to hold all the entries.</param>
        /// <param name="ResumeHandle">A pointer to a value that contains a handle, which is used to continue an existing share search in ShareList. 
        /// handle MUST be zero on the first call and remain unchanged for subsequent calls. If the ResumeHandle parameter is NULL, no resume handle MUST be stored. 
        /// If this parameter is not NULL and the method returns ERROR_MORE_DATA, this parameter receives a nonzero value that can be passed in subsequent 
        /// calls to this method to continue with the enumeration in ShareList. If this parameter is NULL or points to 0x00000000, the enumeration starts from the beginning of the ShareList.
        /// </param>
        /// <returns>The method returns 0x00000000 (NERR_Success) to indicate success; otherwise, it returns a nonzero error code.</returns>
        public uint NetrShareEnum(
            string serverName,
            ref SHARE_ENUM_STRUCT infoStruct,
            uint PreferedMaximumLength,
            out uint? TotalEntries,
            ref uint? ResumeHandle)
        {
            /* 	NET_API_STATUS NetrShareEnum(
                  [in, string, unique] SRVSVC_HANDLE ServerName,
                  [in, out] LPSHARE_ENUM_STRUCT InfoStruct,
                  [in] DWORD PreferedMaximumLength,
                  [out] DWORD* TotalEntries,
                  [in, out, unique] DWORD* ResumeHandle
                );
            */

            Int3264[] paramList;
            TotalEntries = 0;
            uint retVal = 0;
            using (SafeIntPtr pServerName = Marshal.StringToHGlobalUni(serverName),
                pInfoStruct = TypeMarshal.ToIntPtr(infoStruct),
                pResumeHandle = TypeMarshal.ToIntPtr(ResumeHandle))
            {
                paramList = new Int3264[]{
                    pServerName,
                    pInfoStruct,
                    PreferedMaximumLength,
                    IntPtr.Zero, // out value
                    pResumeHandle,
                    IntPtr.Zero // return value
                };

                using (RpceInt3264Collection outParamList = RpceCall(paramList, (ushort)SRVS_OPNUM.NetrShareEnum))
                {
                    retVal = outParamList[paramList.Length - 1].ToUInt32();
                    if (retVal == (uint)Win32ErrorCode_32.ERROR_SUCCESS)
                    {
                        infoStruct = TypeMarshal.ToStruct<SHARE_ENUM_STRUCT>(outParamList[1].ToIntPtr());
                        TotalEntries = TypeMarshal.ToNullableStruct<uint>(outParamList[3]);
                        ResumeHandle = TypeMarshal.ToNullableStruct<uint>(outParamList[4]);
                    }
                }
            }
            return retVal;
        }

        /// <summary>
        /// The NetrShareGetInfo method retrieves information about a particular shared resource on the server from the ShareList.
        /// </summary>
        /// <param name="ServerName">A string that identifies the server. If this parameter is NULL, the local computer is used.</param>
        /// <param name="NetName">The name of the share to return information for.</param>
        /// <param name="Level">Specifies the information level of the data. This parameter MUST be one of the following values.</param>
        /// <param name="InfoStruct">Its contents are determined by the value of the Level parameter, as shown in the preceding table.</param>
        /// <returns>The method returns 0x00000000 (NERR_Success) to indicate success; otherwise, it returns a nonzero error code.</returns>
        public uint NetrShareGetInfo(string ServerName, string NetName, SHARE_ENUM_STRUCT_LEVEL Level, out SHARE_INFO? InfoStruct)
        {
            /*
            NET_API_STATUS NetrShareGetInfo(
              [in, string, unique] SRVSVC_HANDLE ServerName,
              [in, string] WCHAR* NetName,
              [in] DWORD Level,
              [out, switch_is(Level)] LPSHARE_INFO InfoStruct
            );
             */
            Int3264[] paramList;
            uint retVal = 0;
            InfoStruct = null;
            using (SafeIntPtr pServerName = Marshal.StringToHGlobalUni(ServerName), pNetName = Marshal.StringToHGlobalUni(NetName))
            {
                paramList = new Int3264[]{
                    pServerName,
                    pNetName,
                    (uint)Level,
                    IntPtr.Zero, // out value
                    IntPtr.Zero // return value
                };
                using (RpceInt3264Collection outParamList = RpceCall(paramList, (ushort)SRVS_OPNUM.NetrShareGetInfo))
                {
                    retVal = outParamList[paramList.Length - 1].ToUInt32();
                    if (retVal == (uint)Win32ErrorCode_32.ERROR_SUCCESS)
                    {
                        InfoStruct = TypeMarshal.ToStruct<SHARE_INFO>(outParamList[3], Level, null, null);
                    }
                }
            }
            return retVal;
        }

        /// <summary>
        /// The NetrShareSetInfo method sets the parameters of a shared resource in a ShareList.
        /// </summary>
        /// <param name="ServerName">A string that identifies the server. If this parameter is NULL, the local computer is used.</param>
        /// <param name="NetName">The name of the share to return information for.</param>
        /// <param name="Level">Specifies the information level of the data. This parameter MUST be one of the following values.</param>
        /// <param name="InfoStruct">Its contents are determined by the value of the Level parameter, as shown in the preceding table.
        /// This parameter MUST NOT contain a null value. </param>
        /// <param name="ParmErr">An integer value that receives the index of the first member of the share information 
        /// structure that caused the ERROR_INVALID_PARAMETER error, if it occurs</param>
        /// <returns>The method returns 0x00000000 (NERR_Success) to indicate success; otherwise, it returns a nonzero error code.</returns>
        public uint NetrShareSetInfo(string ServerName, string NetName, SHARE_ENUM_STRUCT_LEVEL Level, SHARE_INFO InfoStruct, ref uint? ParmErr)
        {
            /*
            NET_API_STATUS NetrShareSetInfo(
                [in, string, unique] SRVSVC_HANDLE ServerName,
                [in, string] WCHAR* NetName,
                [in] DWORD Level,
                [in, switch_is(Level)] LPSHARE_INFO ShareInfo,
                [in, out, unique] DWORD* ParmErr
            );
             */

            Int3264[] paramList;
            uint retVal = 0;

            using (SafeIntPtr pServerName = Marshal.StringToHGlobalUni(ServerName),
                pNetName = Marshal.StringToHGlobalUni(NetName),
                pShareInfo = TypeMarshal.ToIntPtr(InfoStruct, Level, null, null),
                pParmErr = TypeMarshal.ToIntPtr(ParmErr))
            {
                paramList = new Int3264[]{
                    pServerName,
                    pNetName,
                    (uint)Level,
                    pShareInfo, // out value
                    pParmErr,
                    IntPtr.Zero // return value
                    };
                using (RpceInt3264Collection outParamList = RpceCall(paramList, (ushort)SRVS_OPNUM.NetrShareSetInfo))
                {
                    retVal = outParamList[paramList.Length - 1].ToUInt32();
                    if (retVal == (uint)Win32ErrorCode_32.ERROR_INVALID_PARAMETER)
                    {
                        ParmErr = TypeMarshal.ToNullableStruct<uint>(outParamList[4]);
                    }
                }
            }
            return retVal;
        }

        #endregion

        #region Private Methods

        private void InnerBind(
            string protocolSequence,
            string networkAddress,
            string endpoint,
            AccountCredential transportCredential,
            ClientSecurityContext securityContext)
        {
            if (RpceClientTransport != null)
            {
                throw new InvalidOperationException("SRVS has already been bind");
            }

            RpceClientTransport = new RpceClientTransport();

            try
            {
                RpceClientTransport.Bind(
                    protocolSequence,
                    networkAddress,
                    endpoint,
                    transportCredential,
                    SrvsUtility.SRVS_INTERFACE_UUID,
                    SrvsUtility.SRVS_INTERFACE_MAJOR_VERSION,
                    SrvsUtility.SRVS_INTERFACE_MINOR_VERSION,
                    securityContext,
                    AuthenticationLevel,
                    true,
                    RpceTimeout);
            }
            catch
            {
                RpceClientTransport = null;
                throw;
            }
        }

        private RpceInt3264Collection RpceCall(Int3264[] paramList, ushort opnum)
        {
            byte[] requestStub;
            byte[] responseStub;
            RpceInt3264Collection outParamList = null;

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    SrvsStubFormatString.TypeFormatString,
                    null,
                    SrvsStubFormatString.ProcFormatString,
                    SrvsStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            RpceClientTransport.Call(opnum, requestStub, RpceTimeout, out responseStub);

            outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    SrvsStubFormatString.TypeFormatString,
                    null,
                    SrvsStubFormatString.ProcFormatString,
                    SrvsStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    RpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList);

            return outParamList;
        }

        #endregion
    }
}
