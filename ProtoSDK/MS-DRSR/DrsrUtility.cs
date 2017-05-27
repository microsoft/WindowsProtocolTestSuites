// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr
{
    /// <summary>
    /// DRSR utility class.
    /// </summary>
    public static class DrsrUtility
    {
        /// <summary>
        /// DRSR RPC interface UUID for drsuapi methods.
        /// </summary>
        public static readonly Guid DRSUAPI_RPC_INTERFACE_UUID = new Guid("e3514235-4b06-11d1-ab04-00c04fc2dcd2");

        /// <summary>
        /// DRSR RPC interface major version for drsuapi methods.
        /// </summary>
        [CLSCompliant(false)]
        public const ushort DRSUAPI_RPC_INTERFACE_MAJOR_VERSION = 4;

        /// <summary>
        /// DRSR RPC interface minor version for drsuapi methods.
        /// </summary>
        [CLSCompliant(false)]
        public const ushort DRSUAPI_RPC_INTERFACE_MINOR_VERSION = 0;

        /// <summary>
        /// DRSR RPC interface UUID for dsaop methods.
        /// </summary>
        public static readonly Guid DSAOP_RPC_INTERFACE_UUID = new Guid("7c44d7d4-31d5-424c-bd5e-2b3e1f323d22");

        /// <summary>
        /// DRSR RPC interface major version for dsaop methods.
        /// </summary>
        [CLSCompliant(false)]
        public const ushort DSAOP_RPC_INTERFACE_MAJOR_VERSION = 1;

        /// <summary>
        /// DRSR RPC interface minor version for dsaop methods.
        /// </summary>
        [CLSCompliant(false)]
        public const ushort DSAOP_RPC_INTERFACE_MINOR_VERSION = 0;


        #region Retrieve DRSR dynamic TCP endpoint of a server

        /// <summary>
        /// Retrieve DRSR dynamic TCP endpoint of a server.
        /// </summary>
        /// <param name="interfaceType">Rpc interface type.</param>
        /// <param name="serverName">Server name.</param>
        /// <returns>TCP endpoints</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when serverName is null.
        /// </exception>
        [CLSCompliant(false)]
        public static ushort[] QueryDrsrTcpEndpoint(DrsrRpcInterfaceType interfaceType, string serverName)
        {
            if (serverName == null)
            {
                throw new ArgumentNullException("serverName");
            }

            ushort[] endpoints;
            if (interfaceType == DrsrRpcInterfaceType.DRSUAPI)
            {
                endpoints = RpceEndpointMapper.QueryDynamicTcpEndpointByInterface(
                    serverName,
                    DRSUAPI_RPC_INTERFACE_UUID,
                    DRSUAPI_RPC_INTERFACE_MAJOR_VERSION,
                    DRSUAPI_RPC_INTERFACE_MINOR_VERSION);
            }
            else
            {
                endpoints = RpceEndpointMapper.QueryDynamicTcpEndpointByInterface(
                    serverName,
                    DSAOP_RPC_INTERFACE_UUID,
                    DSAOP_RPC_INTERFACE_MAJOR_VERSION,
                    DSAOP_RPC_INTERFACE_MINOR_VERSION);
            }

            return endpoints;
        }

        #endregion


        /// <summary>
        /// Convert the DRS_EXTENSIONS to DRS_EXTENSIONS_INT
        /// </summary>
        /// <param name="extensions">the general struct of DRS_EXTENSIONS</param>
        /// <returns>the struct of DRS_EXTENSIONS_INT</returns>
        public static DRS_EXTENSIONS_INT DecodeDrsExtensions(DRS_EXTENSIONS extensions)
        {
            //Size of extensions.cb
            int sizeOfCbField = sizeof(uint);
            byte[] extensionsIntBytes = new byte[Marshal.SizeOf(new DRS_EXTENSIONS_INT())];
            Buffer.BlockCopy(TypeMarshal.ToBytes<uint>(extensions.cb), 0, extensionsIntBytes, 0, sizeOfCbField);
            Buffer.BlockCopy(extensions.rgb, 0, extensionsIntBytes, sizeOfCbField, extensions.rgb.Length);
            return TypeMarshal.ToStruct<DRS_EXTENSIONS_INT>(extensionsIntBytes);
        }


        /// <summary>
        /// Convert the DRS_EXTENSIONS_INT to DRS_EXTENSIONS
        /// </summary>
        /// <param name="extensions">the struct of DRS_EXTENSIONS</param>
        /// <returns>the general struct of DRS_EXTENSIONS_INT</returns>
        public static DRS_EXTENSIONS EncodeDrsExtensions(DRS_EXTENSIONS_INT extensions)
        {
            byte[] extensionsBytes = TypeMarshal.ToBytes<DRS_EXTENSIONS_INT>(extensions);
            DRS_EXTENSIONS drsExtensions = new DRS_EXTENSIONS();
            drsExtensions.cb = extensions.cb;
            drsExtensions.rgb = new byte[drsExtensions.cb];
            Buffer.BlockCopy(extensionsBytes, Marshal.SizeOf(drsExtensions.cb),
                drsExtensions.rgb, 0, drsExtensions.rgb.Length);
            return drsExtensions;
        }


        /// <summary>
        /// Creates an instance of request stub upon opnum received
        /// </summary>
        /// <param name="interfaceType">Rpc interface type.</param>    
        /// <param name="opnum"> opnum received</param>
        /// <returns>an instance of request stub.</returns>
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode")]
        internal static DrsrRequestStub CreateDrsrRequestStub(DrsrRpcInterfaceType interfaceType, ushort opnum)
        {
            DrsrRequestStub requestStub = null;
            if (interfaceType == DrsrRpcInterfaceType.DRSUAPI)
            {
                switch ((DrsuapiMethodOpnums)opnum)
                {
                    case DrsuapiMethodOpnums.DrsBind:
                        requestStub = new DrsBindRequest();
                        break;

                    case DrsuapiMethodOpnums.DrsUnbind:
                        requestStub = new DrsUnbindRequest();
                        break;

                    case DrsuapiMethodOpnums.DrsReplicaSync:
                        requestStub = new DrsReplicaSyncRequest();
                        break;

                    case DrsuapiMethodOpnums.DrsGetNcChanges:
                        requestStub = new DrsGetNcChangesRequest();
                        break;

                    case DrsuapiMethodOpnums.DrsUpdateRefs:
                        requestStub = new DrsUpdateRefsRequest();
                        break;

                    case DrsuapiMethodOpnums.DrsReplicaAdd:
                        requestStub = new DrsReplicaAddRequest();
                        break;

                    case DrsuapiMethodOpnums.DrsReplicaDel:
                        requestStub = new DrsReplicaDelRequest();
                        break;

                    case DrsuapiMethodOpnums.DrsReplicaModify:
                        requestStub = new DrsReplicaModifyRequest();
                        break;

                    case DrsuapiMethodOpnums.DrsVerifyNames:
                        requestStub = new DrsVerifyNamesRequest();
                        break;

                    case DrsuapiMethodOpnums.DrsGetMemberships:
                        requestStub = new DrsGetMembershipsRequest();
                        break;

                    case DrsuapiMethodOpnums.DrsInterDomainMove:
                        requestStub = new DrsInterDomainMoveRequest();
                        break;

                    case DrsuapiMethodOpnums.DrsGetNt4ChangeLog:
                        requestStub = new DrsGetNt4ChangeLogRequest();
                        break;

                    case DrsuapiMethodOpnums.DrsCrackNames:
                        requestStub = new DrsCrackNamesRequest();
                        break;

                    case DrsuapiMethodOpnums.DrsWriteSPN:
                        requestStub = new DrsWriteSpnRequest();
                        break;

                    case DrsuapiMethodOpnums.DrsRemoveDsServer:
                        requestStub = new DrsRemoveDsServerRequest();
                        break;

                    case DrsuapiMethodOpnums.DrsRemoveDsDomain:
                        requestStub = new DrsRemoveDsDomainRequest();
                        break;

                    case DrsuapiMethodOpnums.DrsDomainControllerInfo:
                        requestStub = new DrsDomainControllerInfoRequest();
                        break;

                    case DrsuapiMethodOpnums.DrsAddEntry:
                        requestStub = new DrsAddEntryRequest();
                        break;

                    case DrsuapiMethodOpnums.DrsExecuteKcc:
                        requestStub = new DrsExecuteKccRequest();
                        break;

                    case DrsuapiMethodOpnums.DrsGetReplInfo:
                        requestStub = new DrsGetReplInfoRequest();
                        break;

                    case DrsuapiMethodOpnums.DrsAddSidHistory:
                        requestStub = new DrsAddSidHistoryRequest();
                        break;

                    case DrsuapiMethodOpnums.DrsGetMemberships2:
                        requestStub = new DrsGetMemberships2Request();
                        break;

                    case DrsuapiMethodOpnums.DrsReplicaVerifyObjects:
                        requestStub = new DrsReplicaVerifyObjectsRequest();
                        break;

                    case DrsuapiMethodOpnums.DrsGetObjectExistence:
                        requestStub = new DrsGetObjectExistenceRequest();
                        break;

                    case DrsuapiMethodOpnums.DrsQuerySitesByCost:
                        requestStub = new DrsQuerySitesByCostRequest();
                        break;

                    case DrsuapiMethodOpnums.DrsInitDemotion:
                        requestStub = new DrsInitDemotionRequest();
                        break;

                    case DrsuapiMethodOpnums.DrsReplicaDemotion:
                        requestStub = new DrsReplicaDemotionRequest();
                        break;

                    case DrsuapiMethodOpnums.DrsFinishDemotion:
                        requestStub = new DrsFinishDemotionRequest();
                        break;
                }
            }
            else if (interfaceType == DrsrRpcInterfaceType.DSAOP)
            {
                switch ((DsaopMethodOpnums)opnum)
                {
                    case DsaopMethodOpnums.DsaPrepareScript:
                        requestStub = new DsaPrepareScriptRequest();
                        break;

                    case DsaopMethodOpnums.DsaExecuteScript:
                        requestStub = new DsaExecuteScriptRequest();
                        break;
                }
            }

            return requestStub;
        }

        /// <summary>
        /// check bits according to RepNbrOptionToDra to return allowed bits only
        /// </summary>
        /// <param name="input">a DRS_OPTIONS value</param>
        /// <returns></returns>
        public static uint ConvertRepFlagsToRepNbrOption(uint input)
        {
            Array tmp = Enum.GetValues(typeof(RepNbrOptionToDra));
            uint ret = 0;

            for (int i = 0; i < tmp.Length; i++)
            {
                uint current = (uint)tmp.GetValue(i);
                if ((current & input) > 0)
                    ret |= current;
            }

            return ret;
        }
    }
}
