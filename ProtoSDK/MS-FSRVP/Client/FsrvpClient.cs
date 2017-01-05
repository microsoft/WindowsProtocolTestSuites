// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Net.Sockets;

using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fsrvp
{
    /// <summary>
    /// FSRVP client class.
    /// </summary>
    public class FsrvpClient : IDisposable
    {
        #region Fileds
        // FSRVP client context
        private FsrvpClientContext context;

        // RPCE client transport
        internal RpceClientTransport rpceClientTransport;

        // Timeout for RPC bind/call
        private TimeSpan rpceTimeout;

        #endregion

        #region Properties
        /// <summary>
        /// RPCE client context.
        /// </summary>
        public FsrvpClientContext Context
        {
            get
            {
                return context;
            }
        }
        /// <summary>
        /// RPCE handle.
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                return rpceClientTransport.Handle;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor, initialize a client context.<para/>
        /// </summary>
        public FsrvpClient()
        {
            context = new FsrvpClientContext();
            context.AuthenticationLevel = RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_PKT_INTEGRITY;
        }

        #endregion

        #region Public interfaces
        /// <summary>
        /// The common part of all the FSRVP methods
        /// </summary>
        /// <param name="paramList">input param list to decode</param>
        /// <param name="opnum">opnum of the current FSRVP method</param>
        /// <returns>the decoded paramlist</returns>
        public RpceInt3264Collection RpceCall(Int3264[] paramList, ushort opnum)
        {
            byte[] requestStub;
            byte[] responseStub;
            RpceInt3264Collection outParamList = null;

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    FsrvpStubFormatString.TypeFormatString,
                    null,
                    FsrvpStubFormatString.ProcFormatString,
                    FsrvpStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    FsrvpStubFormatString.TypeFormatString,
                    null,
                    FsrvpStubFormatString.ProcFormatString,
                    FsrvpStubFormatString.ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList);


            return outParamList;
        }

        /// <summary>
        /// RPC bind to interface, using specified endpoint and authenticate provider.
        /// </summary>
        /// <param name="protocolSequence">RPC protocol sequence.</param>
        /// <param name="networkAddress">RPC network address.</param>
        /// <param name="endpoint">RPC endpoint.</param>
        /// <param name="transportCredential">
        /// If connect by SMB/SMB2, it's the security credential 
        /// used by underlayer transport (SMB/SMB2). 
        /// If connect by TCP, this parameter is ignored.
        /// </param>
        /// <param name="securityContext">RPC security provider.</param>
        /// <param name="authenticationLevel">RPC authentication level.</param>
        /// <param name="timeout">Timeout for bind and all future requests.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when rpceClientTransport is not null.
        /// </exception>
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
                throw new InvalidOperationException("FSRVP has already been bind.");
            }

            rpceTimeout = timeout;

            rpceClientTransport = new RpceClientTransport();

            try
            {
                rpceClientTransport.Bind(
                    protocolSequence,
                    networkAddress,
                    endpoint,
                    transportCredential,
                    FsrvpUtility.FSRVP_INTERFACE_UUID,
                    FsrvpUtility.FSRVP_INTERFACE_MAJOR_VERSION,
                    FsrvpUtility.FSRVP_INTERFACE_MINOR_VERSION,
                    securityContext,
                    authenticationLevel,
                    true,
                    rpceTimeout);
            }
            catch
            {
                rpceClientTransport = null;
                throw;
            }
        }

        /// <summary>
        /// RPC bind over named pipe, using well-known endpoint "\\pipe\\FssagentRpc".
        /// </summary>
        /// <param name="serverName">FSRVP server machine name.</param>
        /// <param name="transportCredential">
        /// If connect by SMB/SMB2, it's the security credential 
        /// used by underlayer transport (SMB/SMB2). 
        /// If connect by TCP, this parameter is ignored.
        /// </param>
        /// <param name="securityContext">
        /// Security provider for RPC. 
        /// Set the value to null to disable authentication.
        /// </param>
        /// <param name="timeout">Timeout for bind and all future requests.</param>
        /// <returns>Return true if success, otherwise return false.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when serverName is null.
        /// </exception>
        public void BindOverNamedPipe(
            string serverName,
            AccountCredential transportCredential,
            ClientSecurityContext securityContext,
            TimeSpan timeout)
        {
            if (serverName == null)
            {
                throw new ArgumentNullException("serverName");
            }

            Bind(
                RpceUtility.RPC_OVER_NAMED_PIPE_PROTOCOL_SEQUENCE,
                serverName,
                FsrvpUtility.FSRVP_NAMED_PIPE,
                transportCredential,
                securityContext,
                context.AuthenticationLevel,
                timeout);
        }

        /// <summary>
        /// RPCE unbind and disconnect.
        /// </summary>
        /// <param name="timeout">Timeout period.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when RPC has not been bind.
        /// </exception>
        public void Unbind(TimeSpan timeout)
        {
            if (rpceClientTransport != null)
            {
                try
                {
                    rpceClientTransport.Unbind(timeout);
                    rpceClientTransport.Dispose();
                }
                catch
                {
                }
                finally
                {
                    rpceClientTransport = null;
                }
            }
        }

        /// <summary>
        /// Get the Server supported version range.  
        /// Windows Server 8 compatiable Agent should return both  
        /// as FSSAGENT_RPC_VERSION_1
        /// </summary>
        /// <param name="MinVersion">The minor version.</param>
        /// <param name="MaxVersion">The maximum version.</param>
        /// <returns></returns>
        public int GetSupportedVersion(out uint MinVersion, out uint MaxVersion)
        {
            Int3264[] paramList;
            int retVal = 0;

            paramList = new Int3264[] {
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero
            };

            using (RpceInt3264Collection outParamList = RpceCall(paramList, (ushort)FSRVP_OPNUM.GetSupportedVersion))
            {
                MinVersion = (uint)Marshal.ReadInt32(outParamList[0]);
                MaxVersion = (uint)Marshal.ReadInt32(outParamList[1]);
                retVal = outParamList[2].ToInt32();
            }

            return retVal;
        }

        /// <summary>
        /// The SetContext method sets the context for subsequent shadow copy-related operations.
        /// </summary>
        /// <param name="Context">The context to be used for the shadow copy operations.</param>
        /// <returns>It MUST be zero or a combination of the CONTEXT_VALUES as specified in section 2.2.2.2.</returns>
        public int SetContext(ulong Context)
        {
            Int3264[] paramList;
            int retVal = 0;

            paramList = new Int3264[] {
                Context,
                IntPtr.Zero
            };

            using (RpceInt3264Collection outParamList = RpceCall(paramList, (ushort)FSRVP_OPNUM.SetContext))
            {
                retVal = outParamList[1].ToInt32();
            }

            return retVal;
        }

        /// <summary>
        /// The StartShadowCopySet method is called by the client to initiate a new shadow copy set for shadow copy creation.
        /// </summary>
        /// <param name="pShadowCopySetId">On output, the GUID of the shadow copy set that must be created by server.</param>
        /// <returns></returns>
        public int StartShadowCopySet(
                        Guid clientShadowCopySetId,
                        out Guid pShadowCopySetId)
        {
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pClientShadowCopySetId = TypeMarshal.ToIntPtr(clientShadowCopySetId);

            paramList = new Int3264[] {
                pClientShadowCopySetId,
                IntPtr.Zero,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, (ushort)FSRVP_OPNUM.StartShadowCopySet))
                {
                    pShadowCopySetId = TypeMarshal.ToStruct<Guid>(outParamList[1]);
                    retVal = outParamList[2].ToInt32();
                }
            }
            finally
            {
                pClientShadowCopySetId.Dispose();
            }

            return retVal;
        }

        /// <summary>
        /// The AddToShadowCopySet method adds a share to an existing shadow copy set.
        /// </summary>
        /// <param name="ClientShadowCopyId">The GUID of the client. This MUST be set to NULL.</param>
        /// <param name="ShadowCopySetId">The GUID of the shadow copy set to which ShareName is to be added.</param>
        /// <param name="ShareName">The name of the share in UNC format for which a shadow copy is required.</param>
        /// <param name="pShadowCopyId">On output, the GUID of the shadow copy associated to the share.</param>
        /// <returns></returns>
        public int AddToShadowCopySet(
                        Guid ClientShadowCopyId,
                        Guid ShadowCopySetId,
                        string ShareName,
                        out Guid pShadowCopyId)
        {
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pClientShadowCopyId = TypeMarshal.ToIntPtr(ClientShadowCopyId);
            SafeIntPtr pShadowCopySetId = TypeMarshal.ToIntPtr(ShadowCopySetId);
            SafeIntPtr pShareName = Marshal.StringToHGlobalUni(ShareName);

            paramList = new Int3264[]{
                pClientShadowCopyId,
                pShadowCopySetId,
                pShareName,
                IntPtr.Zero,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, (ushort)FSRVP_OPNUM.AddToShadowCopySet))
                {
                    pShadowCopyId = TypeMarshal.ToStruct<Guid>(outParamList[3]);
                    retVal = outParamList[4].ToInt32();
                }
            }
            finally
            {
                pClientShadowCopyId.Dispose();
                pShadowCopySetId.Dispose();
                pShareName.Dispose();
            }

            return retVal;
        }

        /// <summary>
        /// The CommitShadowCopySet method is invoked by the client to start the shadow copy creation process
        /// on the server for a given shadow copy set.
        /// </summary>
        /// <param name="ShadowCopySetId">The GUID of the shadow copy set to which ShareName is to be added.</param>
        /// <param name="TimeOutInMilliseconds">The maximum total time in milliseconds for which the server 
        /// MUST wait for the shadow copy commit process.</param>
        /// <returns></returns>
        public int CommitShadowCopySet(
                        Guid ShadowCopySetId,
                        uint TimeOutInMilliseconds)
        {
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pShadowCopySetId = TypeMarshal.ToIntPtr(ShadowCopySetId);

            paramList = new Int3264[] {
                pShadowCopySetId,
                TimeOutInMilliseconds,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, (ushort)FSRVP_OPNUM.CommitShadowCopySet))
                {
                    retVal = outParamList[2].ToInt32();
                }
            }
            finally
            {
                pShadowCopySetId.Dispose();
            }

            return retVal;
        }

        /// <summary>
        /// The ExposeShadowCopySet method exposes all the shadow copies in a shadow copy set as file shares
        /// on the file server.
        /// </summary>
        /// <param name="ShadowCopySetId">The GUID of the shadow copy set.</param>
        /// <param name="TimeOutInMilliseconds">The maximum total time in milliseconds for which the server 
        /// MUST wait for the expose operation.</param>
        /// <returns></returns>
        public int ExposeShadowCopySet(
                        Guid ShadowCopySetId,
                        uint TimeOutInMilliseconds)
        {
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pShadowCopySetId = TypeMarshal.ToIntPtr(ShadowCopySetId);

            paramList = new Int3264[] {
                pShadowCopySetId,
                TimeOutInMilliseconds,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, (ushort)FSRVP_OPNUM.ExposeShadowCopySet))
                {
                    retVal = outParamList[2].ToInt32();
                }
            }
            finally
            {
                pShadowCopySetId.Dispose();
            }

            return retVal;
        }

        /// <summary>
        /// The RecoveryCompleteShadowCopySet method is invoked by the client to indicate the server
        /// that the data associated with the file shares in a shadow copy set have been recovered by
        /// the shadow copy utility.
        /// </summary>
        /// <param name="ShadowCopySetId">The GUID of the shadow copy set.</param>
        /// <returns></returns>
        public int RecoveryCompleteShadowCopySet(Guid ShadowCopySetId)
        {
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pShadowCopySetId = TypeMarshal.ToIntPtr(ShadowCopySetId);

            paramList = new Int3264[] {
                pShadowCopySetId,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, (ushort)FSRVP_OPNUM.RecoveryCompleteShadowCopySet))
                {
                    retVal = outParamList[1].ToInt32();
                }
            }
            finally
            {
                pShadowCopySetId.Dispose();
            }

            return retVal;
        }

        /// <summary>
        /// The AbortShadowCopySet method is invoked by the client to delete a given shadow copy set on the server.
        /// </summary>
        /// <param name="ShadowCopySetId">The GUID of the shadow copy set to which ShareName is to be added.</param>
        /// <returns></returns>
        public int AbortShadowCopySet(Guid ShadowCopySetId)
        {
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pShadowCopySetId = TypeMarshal.ToIntPtr(ShadowCopySetId);

            paramList = new Int3264[] {
                pShadowCopySetId,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, (ushort)FSRVP_OPNUM.AbortShadowCopySet))
                {
                    retVal = outParamList[1].ToInt32();
                }
            }
            finally
            {
                pShadowCopySetId.Dispose();
            }

            return retVal;
        }

        /// <summary>
        /// The IsPathSupported method is invoked by client to query if a given share is supported by the server 
        /// for shadow copy operations.
        /// </summary>
        /// <param name="ShareName">The full path of the share in UNC format.</param>
        /// <param name="SupportedByThisProvider">On output, a Boolean, when set to TRUE, indicates that shadow
        /// copies of this share are supported by the server.</param>
        /// <param name="OwnerMachineName">On output, the name of the server machine to which the client MUST 
        /// connect to create shadow copies of the specified ShareName.</param>
        /// <returns></returns>
        public int IsPathSupported(
                        string ShareName,
                        out bool SupportedByThisProvider,
                        out string OwnerMachineName)
        {
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pShareName = Marshal.StringToHGlobalUni(ShareName);

            paramList = new Int3264[] {
                pShareName,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, (ushort)FSRVP_OPNUM.IsPathSupported))
                {
                    SupportedByThisProvider = TypeMarshal.ToStruct<bool>(outParamList[1]);//(Marshal.ReadInt32(outParamList[1]) == 1);
                    OwnerMachineName = Marshal.PtrToStringUni(Marshal.ReadIntPtr(outParamList[2]));
                    retVal = outParamList[3].ToInt32();
                }
            }
            finally
            {
                pShareName.Dispose();
            }

            return retVal;
        }

        /// <summary>
        /// The IsPathShadowCopied method is invoked by the client to query if any shadow copy for a share already exists.
        /// </summary>
        /// <param name="ShareName">The full path of the share in UNC format.</param>
        /// <param name="ShadowCopyPresent">On output, a Boolean, when set to TRUE, indicates that shadow
        /// copies of this share are supported by the server.</param>
        /// <param name="ShadowCopyCompatibility">On output, this indicates whether certain I/O operations on the object
        /// store containing the shadow copy are disabled. This MUST be zero or a combination of the values 
        /// as specified in section 2.2.2.2.</param>
        /// <returns></returns>
        public int IsPathShadowCopied(
                        string ShareName,
                        out bool ShadowCopyPresent,
                        out long ShadowCopyCompatibility)
        {
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pShareName = Marshal.StringToHGlobalUni(ShareName);

            paramList = new Int3264[] {
                pShareName,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, (ushort)FSRVP_OPNUM.IsPathShadowCopied))
                {
                    retVal = outParamList[paramList.Length - 1].ToInt32();
                    ShadowCopyPresent = TypeMarshal.ToStruct<bool>(outParamList[1]);
                    ShadowCopyCompatibility = Marshal.ReadInt32(outParamList[2]);
                    retVal = outParamList[3].ToInt32();
                }
            }
            finally
            {
                pShareName.Dispose();
            }

            return retVal;
        }

        /// <summary>
        /// The GetShareMapping method is invoked by the client to get the shadow copy information on a given
        /// file share on the server.
        /// </summary>
        /// <param name="ShadowCopyId">The GUID of the shadow copy associated with the share.</param>
        /// <param name="ShadowCopySetId">The GUID of the shadow copy set.</param>
        /// <param name="ShareName">The name of the share in UNC format.</param>
        /// <param name="Level">The format of this data depends on the value of the level parameter.</param>
        /// <param name="ShareMapping">On output, a FSSAGENT_SHARE_MAPPING structure, as specified in section 2.2.3.1.</param>
        /// <returns></returns>
        public int GetShareMapping(
                        Guid ShadowCopyId,
                        Guid ShadowCopySetId,
                        string ShareName,
                        uint Level,
                        out FSSAGENT_SHARE_MAPPING ShareMapping)
        {
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pShadowCopyId = TypeMarshal.ToIntPtr(ShadowCopyId);
            SafeIntPtr pShadowCopySetId = TypeMarshal.ToIntPtr(ShadowCopySetId);
            SafeIntPtr pShareName = Marshal.StringToHGlobalUni(ShareName);

            paramList = new Int3264[] {
                pShadowCopyId,
                pShadowCopySetId,
                pShareName,
                Level,
                IntPtr.Zero,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, (ushort)FSRVP_OPNUM.GetShareMapping))
                {
                    retVal = outParamList[5].ToInt32();
                    ShareMapping = new FSSAGENT_SHARE_MAPPING();
                    if ((FsrvpErrorCode)retVal == FsrvpErrorCode.FSRVP_SUCCESS)
                    {
                        ShareMapping.ShareMapping1 = TypeMarshal.ToStruct<FSSAGENT_SHARE_MAPPING_1>(Marshal.ReadIntPtr(outParamList[4]));
                        ShareMapping.ShareMapping1IsNull = false;
                    }
                    else
                    {
                        ShareMapping.ShareMapping1IsNull = true;
                    }
                }
            }
            finally
            {
                pShadowCopyId.Dispose();
                pShadowCopySetId.Dispose();
                pShareName.Dispose();
            }

            return retVal;
        }

        /// <summary>
        /// The DeleteShareMapping method deletes the mapping of a share’s shadow copy from a shadow copy set.
        /// </summary>
        /// <param name="ShadowCopySetId">The GUID of the shadow copy set.</param>
        /// <param name="ShadowCopyId">The GUID of the shadow copy associated with the share.</param>
        /// <param name="ShareName">The name of the share for which the share mapping is to be deleted.</param>
        /// <returns></returns>
        public int DeleteShareMapping(
                        Guid ShadowCopySetId,
                        Guid ShadowCopyId,
                        string ShareName)
        {
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pShadowCopySetId = TypeMarshal.ToIntPtr(ShadowCopySetId);
            SafeIntPtr pShadowCopyId = TypeMarshal.ToIntPtr(ShadowCopyId);
            SafeIntPtr pShareName = Marshal.StringToHGlobalUni(ShareName);

            paramList = new Int3264[] {
                pShadowCopySetId,
                pShadowCopyId,
                pShareName,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, (ushort)FSRVP_OPNUM.DeleteShareMapping))
                {
                    retVal = outParamList[3].ToInt32();
                }
            }
            finally
            {
                pShadowCopySetId.Dispose();
                pShadowCopyId.Dispose();
                pShareName.Dispose();
            }

            return retVal;
        }

        /// <summary>
        /// This call waits until all the shadowcopy preparation is done on the file server.
        /// It is used to avoid the long preparation time on file server break the
        /// 60 seconds freeze/thaw window on the application server
        /// It must be called between the last AddToShadowCopySet and CommitShadowCopySet
        /// </summary>
        /// <param name="ShadowCopySetId">The GUID of shadow copy set</param>
        /// <param name="TimeOutInMilliseconds">The maximum total time in milliseconds for which the server MUST wait for the expose operation.</param>
        /// <returns></returns>
        public int PrepareShadowCopySet(
            Guid ShadowCopySetId,
            uint TimeOutInMilliseconds
            )
        {
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pShadowCopySetId = TypeMarshal.ToIntPtr(ShadowCopySetId);

            paramList = new Int3264[]{
                pShadowCopySetId,
                TimeOutInMilliseconds,
                IntPtr.Zero
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, (ushort)FSRVP_OPNUM.PrepareShadowCopySet))
                {
                    retVal = outParamList[2].ToInt32();
                }
            }
            finally
            {
                pShadowCopySetId.Dispose();
            }

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
        }

        /// <summary>
        /// finalizer
        /// </summary>
        ~FsrvpClient()
        {
            Dispose(false);
        }

        #endregion
    }
}
