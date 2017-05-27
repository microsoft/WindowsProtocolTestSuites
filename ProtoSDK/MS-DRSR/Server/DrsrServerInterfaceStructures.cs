// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr
{
    /// <summary>
    /// Opnums of Drsr drsuapi methods
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum DrsuapiMethodOpnums : ushort
    {
        /// <summary>
        /// Opnum of method DrsBind
        /// </summary>
        DrsBind = 0,

        /// <summary>
        /// Opnum of method DrsUnbind
        /// </summary>
        DrsUnbind = 1,

        /// <summary>
        /// Opnum of method DrsReplicaSync
        /// </summary>
        DrsReplicaSync = 2,

        /// <summary>
        /// Opnum of method DrsGetNcChanges
        /// </summary>
        DrsGetNcChanges = 3,

        /// <summary>
        /// Opnum of method DrsUpdateRefs
        /// </summary>
        DrsUpdateRefs = 4,

        /// <summary>
        /// Opnum of method DrsReplicaAdd
        /// </summary>
        DrsReplicaAdd = 5,

        /// <summary>
        /// Opnum of method DrsReplicaDel
        /// </summary>
        DrsReplicaDel = 6,

        /// <summary>
        /// Opnum of method DrsReplicaModify
        /// </summary>
        DrsReplicaModify = 7,

        /// <summary>
        /// Opnum of method DrsVerifyNames
        /// </summary>
        DrsVerifyNames = 8,

        /// <summary>
        /// Opnum of method DrsGetMemberships
        /// </summary>
        DrsGetMemberships = 9,

        /// <summary>
        /// Opnum of method DrsInterDomainMove
        /// </summary>
        DrsInterDomainMove = 10,

        /// <summary>
        /// Opnum of method DrsGetNt4ChangeLog
        /// </summary>
        DrsGetNt4ChangeLog = 11,

        /// <summary>
        /// Opnum of method DrsCrackNames
        /// </summary>
        DrsCrackNames = 12,

        /// <summary>
        /// Opnum of method DrsWriteSPN
        /// </summary>
        DrsWriteSPN = 13,

        /// <summary>
        /// Opnum of method DrsRemoveDsServer
        /// </summary>
        DrsRemoveDsServer = 14,

        /// <summary>
        /// Opnum of method DrsRemoveDsDomain
        /// </summary>
        DrsRemoveDsDomain = 15,

        /// <summary>
        /// Opnum of method DrsDomainControllerInfo
        /// </summary>
        DrsDomainControllerInfo = 16,

        /// <summary>
        /// Opnum of method DrsAddEntry
        /// </summary>
        DrsAddEntry = 17,

        /// <summary>
        /// Opnum of method DrsExecuteKcc
        /// </summary>
        DrsExecuteKcc = 18,

        /// <summary>
        /// Opnum of method DrsGetReplInfo
        /// </summary>
        DrsGetReplInfo = 19,

        /// <summary>
        /// Opnum of method DrsAddSidHistory
        /// </summary>
        DrsAddSidHistory = 20,

        /// <summary>
        /// Opnum of method DrsGetMemberships2
        /// </summary>
        DrsGetMemberships2 = 21,

        /// <summary>
        /// Opnum of method DrsReplicaVerifyObjects
        /// </summary>
        DrsReplicaVerifyObjects = 22,

        /// <summary>
        /// Opnum of method DrsGetObjectExistence
        /// </summary>
        DrsGetObjectExistence = 23,

        /// <summary>
        /// Opnum of method DrsQuerySitesByCost
        /// </summary>
        DrsQuerySitesByCost = 24,

        /// <summary>
        /// Opnum of method DrsInitDemotion
        /// </summary>
        DrsInitDemotion = 25,

        /// <summary>
        /// Opnum of method DrsReplicaDemotion
        /// </summary>
        DrsReplicaDemotion = 26,

        /// <summary>
        /// Opnum of method DrsFinishDemotion
        /// </summary>
        DrsFinishDemotion = 27,
    }


    /// <summary>
    /// Opnums of Drsr dsaop methods
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum DsaopMethodOpnums : ushort
    {
        /// <summary>
        /// Opnum of method DsaPrepareScript
        /// </summary>
        DsaPrepareScript = 0,

        /// <summary>
        /// Opnum of method DsaExecuteScript
        /// </summary>
        DsaExecuteScript = 1,
    }


    /// <summary>
    /// The base class of all requests.
    /// </summary>
    public abstract class DrsrRequestStub
    {
        private int rpceLayerOpnum;

        /// <summary>
        ///  Opnum at RPC layer
        /// </summary>
        public int Opnum
        {
            get
            {
                return rpceLayerOpnum;
            }

            protected set
            {
                rpceLayerOpnum = value;
            }
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal abstract void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub);
    }


    /// <summary>
    /// The base class of all responses
    /// </summary>
    public abstract class DrsrResponseStub
    {
        /// <summary>
        /// Return value of the RPC method.
        /// </summary>
        [CLSCompliant(false)]
        public uint Status;

        /// <summary>
        ///  Opnum at RPC layer
        /// </summary>
        private ushort rpceLayerOpnum;

        /// <summary>
        ///  Opnum at RPC layer
        /// </summary>
        [CLSCompliant(false)]
        public ushort Opnum
        {
            get
            {
                return rpceLayerOpnum;
            }

            protected set
            {
                rpceLayerOpnum = value;
            }
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal abstract byte[] Encode(DrsrServerSessionContext sessionContext);
    }


    #region DRSR drsuapi methods

    /// <summary>
    /// The base class of all drsuapi request
    /// </summary>
    public abstract class DrsuapiRequestStub : DrsrRequestStub
    {
    }


    /// <summary>
    /// The base class of all drsuapi response
    /// </summary>
    public abstract class DrsuapiResponseStub : DrsrResponseStub
    {
    }


    #region Structures of input and output parameters of drsuapi methods

    /// <summary>
    /// The DrsBindRequest class defines input parameters of method DrsBind.
    /// </summary>
    public class DrsBindRequest : DrsuapiRequestStub
    {
        /// <summary>
        /// RPC handle.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr rpc_handle;

        /// <summary>
        /// clientDsaUuid parameter.
        /// </summary>
        public Guid? clientDsaUuid;

        /// <summary>
        /// clientExtensions parameter
        /// </summary>
        public DRS_EXTENSIONS? clientExtensions;

        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsBindRequest()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsBind;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                RpceStubHelper.GetPlatform(),
                DrsrRpcStubFormatString.TypeFormatString,
                null,
                DrsrRpcStubFormatString.ProcFormatString,
                DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                rpc_handle = sessionContext.RpceLayerSessionContext.Handle;
                clientDsaUuid = TypeMarshal.ToNullableStruct<Guid>(outParamList[0]);
                clientExtensions = TypeMarshal.ToNullableStruct<DRS_EXTENSIONS>(outParamList[1]);
            }
        }
    }


    /// <summary>
    /// The DrsBindResponse class defines output parameters of method DrsBind.
    /// </summary>
    public class DrsBindResponse : DrsuapiResponseStub
    {
        /// <summary>
        ///  serverExtensions parameter.
        /// </summary>
        public DRS_EXTENSIONS? serverExtensions;

        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr drsHandle;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsBindResponse()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsBind;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            SafeIntPtr ptrExtServer = TypeMarshal.ToIntPtr(serverExtensions);
            SafeIntPtr ptrToPtrExtServer = TypeMarshal.ToIntPtr(ptrExtServer.Value);
            SafeIntPtr ptrHDrs = TypeMarshal.ToIntPtr(drsHandle);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ptrToPtrExtServer,
                    ptrHDrs,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                ptrExtServer.Dispose();
                ptrToPtrExtServer.Dispose();
                ptrHDrs.Dispose();
            }
        }
    }


    /// <summary>
    /// The DrsUnbindRequest class defines input parameters of method DrsUnbind.
    /// </summary>
    public class DrsUnbindRequest : DrsuapiRequestStub
    {
        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr? drsHandle;

        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsUnbindRequest()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsUnbind;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                RpceStubHelper.GetPlatform(),
                DrsrRpcStubFormatString.TypeFormatString,
                null,
                DrsrRpcStubFormatString.ProcFormatString,
                DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                drsHandle = TypeMarshal.ToNullableStruct<IntPtr>(outParamList[0]);
            }
        }
    }


    /// <summary>
    /// The DrsUnbindResponse class defines output parameters of method DrsUnbind.
    /// </summary>
    public class DrsUnbindResponse : DrsuapiResponseStub
    {
        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr drsHandle;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsUnbindResponse()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsUnbind;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            SafeIntPtr ptrHDrs = TypeMarshal.ToIntPtr(drsHandle);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    ptrHDrs,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                ptrHDrs.Dispose();
            }
        }
    }


    /// <summary>
    /// The DrsReplicaSyncRequest class defines input parameters of method DrsReplicaSync.
    /// </summary>
    public class DrsReplicaSyncRequest : DrsuapiRequestStub
    {

        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr drsHandle;

        /// <summary>
        /// inVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint inVersion;

        /// <summary>
        /// inMessage parameter.
        /// </summary>
        public DRS_MSG_REPSYNC? inMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsReplicaSyncRequest()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsReplicaSync;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                RpceStubHelper.GetPlatform(),
                DrsrRpcStubFormatString.TypeFormatString,
                null,
                DrsrRpcStubFormatString.ProcFormatString,
                DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                false,
                requestStub,
                sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                drsHandle = outParamList[0].ToIntPtr();
                inVersion = outParamList[1].ToUInt32();
                inMessage = TypeMarshal.ToNullableStruct<DRS_MSG_REPSYNC>(outParamList[2], inVersion, null, null);
            }
        }
    }


    /// <summary>
    /// The DrsReplicaSyncResponse class defines output parameters of method DrsReplicaSync.
    /// </summary>
    public class DrsReplicaSyncResponse : DrsuapiResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsReplicaSyncResponse()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsReplicaSync;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
            }
        }
    }


    /// <summary>
    /// The DrsUpdateRefsRequest class defines input parameters of method DrsUpdateRefs.
    /// </summary>
    public class DrsUpdateRefsRequest : DrsuapiRequestStub
    {
        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr drsHandle;

        /// <summary>
        /// inVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint inVersion;

        /// <summary>
        /// inMessage parameter.
        /// </summary>
        public DRS_MSG_UPDREFS? inMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsUpdateRefsRequest()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsUpdateRefs;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
             RpceStubHelper.GetPlatform(),
             DrsrRpcStubFormatString.TypeFormatString,
             null,
             DrsrRpcStubFormatString.ProcFormatString,
             DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
             false,
             requestStub,
             sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                drsHandle = outParamList[0].ToIntPtr();
                inVersion = outParamList[1].ToUInt32();
                inMessage = TypeMarshal.ToNullableStruct<DRS_MSG_UPDREFS>(outParamList[2], inVersion, null, null);
            }
        }
    }


    /// <summary>
    /// The DrsUpdateRefsResponse class defines output parameters of method DrsUpdateRefs.
    /// </summary>
    public class DrsUpdateRefsResponse : DrsuapiResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsUpdateRefsResponse()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsUpdateRefs;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
            }
        }
    }


    /// <summary>
    /// The DrsReplicaAddRequest class defines input parameters of method DrsReplicaAdd.
    /// </summary>
    public class DrsReplicaAddRequest : DrsuapiRequestStub
    {
        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr drsHandle;

        /// <summary>
        /// inVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint inVersion;

        /// <summary>
        /// inMessage parameter.
        /// </summary>
        public DRS_MSG_REPADD? inMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsReplicaAddRequest()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsReplicaAdd;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
             RpceStubHelper.GetPlatform(),
             DrsrRpcStubFormatString.TypeFormatString,
             null,
             DrsrRpcStubFormatString.ProcFormatString,
             DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
             false,
             requestStub,
             sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                drsHandle = outParamList[0].ToIntPtr();
                inVersion = outParamList[1].ToUInt32();
                inMessage = TypeMarshal.ToNullableStruct<DRS_MSG_REPADD>(outParamList[2], inVersion, null, null);
            }
        }
    }


    /// <summary>
    /// The DrsReplicaAddResponse class defines output parameters of method DrsReplicaAdd.
    /// </summary>
    public class DrsReplicaAddResponse : DrsuapiResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsReplicaAddResponse()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsReplicaAdd;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
            }
        }
    }


    /// <summary>
    /// The DrsReplicaDelRequest class defines input parameters of method DrsReplicaDel.
    /// </summary>
    public class DrsReplicaDelRequest : DrsuapiRequestStub
    {
        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr drsHandle;

        /// <summary>
        /// inVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint inVersion;

        /// <summary>
        /// inMessage parameter.
        /// </summary>
        public DRS_MSG_REPDEL? inMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsReplicaDelRequest()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsReplicaDel;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
             RpceStubHelper.GetPlatform(),
             DrsrRpcStubFormatString.TypeFormatString,
             null,
             DrsrRpcStubFormatString.ProcFormatString,
             DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
             false,
             requestStub,
             sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                drsHandle = outParamList[0].ToIntPtr();
                inVersion = outParamList[1].ToUInt32();
                inMessage = TypeMarshal.ToNullableStruct<DRS_MSG_REPDEL>(
                    outParamList[2], inVersion, null, null);
            }
        }
    }


    /// <summary>
    /// The DrsReplicaDelResponse class defines output parameters of method DrsReplicaDel.
    /// </summary>
    public class DrsReplicaDelResponse : DrsuapiResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsReplicaDelResponse()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsReplicaDel;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
            }
        }
    }


    /// <summary>
    /// The DrsReplicaModifyRequest class defines input parameters of method DrsReplicaModify.
    /// </summary>
    public class DrsReplicaModifyRequest : DrsuapiRequestStub
    {
        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr drsHandle;

        /// <summary>
        /// inVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint inVersion;

        /// <summary>
        /// inMessage parameter.
        /// </summary>
        public DRS_MSG_REPMOD? inMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsReplicaModifyRequest()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsReplicaModify;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
             RpceStubHelper.GetPlatform(),
             DrsrRpcStubFormatString.TypeFormatString,
             null,
             DrsrRpcStubFormatString.ProcFormatString,
             DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
             false,
             requestStub,
             sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                drsHandle = outParamList[0].ToIntPtr();
                inVersion = outParamList[1].ToUInt32();
                inMessage = TypeMarshal.ToNullableStruct<DRS_MSG_REPMOD>(
                    outParamList[2], inVersion, null, null);
            }
        }
    }


    /// <summary>
    /// The DrsReplicaModifyResponse class defines output parameters of method DrsReplicaModify.
    /// </summary>
    public class DrsReplicaModifyResponse : DrsuapiResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsReplicaModifyResponse()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsReplicaModify;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
            }
        }
    }


    /// <summary>
    /// The DrsCrackNamesRequest class defines input parameters of method DrsCrackNames.
    /// </summary>
    public class DrsCrackNamesRequest : DrsuapiRequestStub
    {
        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr drsHandle;

        /// <summary>
        /// dwInVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwInVersion;

        /// <summary>
        /// inMessage parameter.
        /// </summary>
        public DRS_MSG_CRACKREQ? inMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsCrackNamesRequest()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsCrackNames;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
           RpceStubHelper.GetPlatform(),
             DrsrRpcStubFormatString.TypeFormatString,
             null,
             DrsrRpcStubFormatString.ProcFormatString,
             DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
             false,
             requestStub,
             sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                drsHandle = outParamList[0].ToIntPtr();
                dwInVersion = outParamList[1].ToUInt32();
                inMessage = TypeMarshal.ToNullableStruct<DRS_MSG_CRACKREQ>(
                    outParamList[2], dwInVersion, null, null);
            }
        }
    }


    /// <summary>
    /// The DrsCrackNamesResponse class defines output parameters of method DrsCrackNames.
    /// </summary>
    public class DrsCrackNamesResponse : DrsuapiResponseStub
    {
        /// <summary>
        /// outVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint? outVersion;

        /// <summary>
        /// outMessage parameter.
        /// </summary>
        public DRS_MSG_CRACKREPLY? outMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsCrackNamesResponse()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsCrackNames;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            SafeIntPtr ptrOutVersion = TypeMarshal.ToIntPtr(outVersion);
            SafeIntPtr ptrOutMessage = TypeMarshal.ToIntPtr(outMessage, outVersion, null, null);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ptrOutVersion,
                    ptrOutMessage,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                ptrOutVersion.Dispose();
                ptrOutMessage.Dispose();
            }
        }
    }


    /// <summary>
    /// The DrsWriteSpnRequest class defines input parameters of method DrsWriteSPN.
    /// </summary>
    public class DrsWriteSpnRequest : DrsuapiRequestStub
    {
        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr drsHandle;

        /// <summary>
        /// dwInVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public dwInVersion_Values dwInVersion;

        /// <summary>
        /// inMessage parameter.
        /// </summary>
        public DRS_MSG_SPNREQ? inMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsWriteSpnRequest()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsWriteSPN;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
             RpceStubHelper.GetPlatform(),
             DrsrRpcStubFormatString.TypeFormatString,
             null,
             DrsrRpcStubFormatString.ProcFormatString,
             DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
             false,
             requestStub,
             sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                drsHandle = outParamList[0].ToIntPtr();
                dwInVersion = (dwInVersion_Values)(outParamList[1].ToUInt32());
                inMessage = TypeMarshal.ToNullableStruct<DRS_MSG_SPNREQ>(
                    outParamList[2], dwInVersion, null, null);
            }
        }
    }


    /// <summary>
    /// The DrsWriteSpnResponse class defines output parameters of method DrsWriteSPN.
    /// </summary>
    public class DrsWriteSpnResponse : DrsuapiResponseStub
    {
        /// <summary>
        /// outVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public pdwOutVersion_Values? outVersion;

        /// <summary>
        /// outMessage parameter.
        /// </summary>
        public DRS_MSG_SPNREPLY? outMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsWriteSpnResponse()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsWriteSPN;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            SafeIntPtr ptrOutVersion = TypeMarshal.ToIntPtr(outVersion);
            SafeIntPtr ptrOutMessage = TypeMarshal.ToIntPtr(outMessage, outVersion, null, null);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ptrOutVersion,
                    ptrOutMessage,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                ptrOutVersion.Dispose();
                ptrOutMessage.Dispose();
            }
        }
    }


    /// <summary>
    /// The DrsRemoveDsServerRequest class defines input parameters of method DrsRemoveDsServer.
    /// </summary>
    public class DrsRemoveDsServerRequest : DrsuapiRequestStub
    {
        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr drsHandle;

        /// <summary>
        /// dwInVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public IDL_DRSRemoveDsServer_dwInVersion_Values dwInVersion;

        /// <summary>
        /// inMessage parameter.
        /// </summary>
        public DRS_MSG_RMSVRREQ? inMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsRemoveDsServerRequest()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsRemoveDsServer;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
             RpceStubHelper.GetPlatform(),
             DrsrRpcStubFormatString.TypeFormatString,
             null,
             DrsrRpcStubFormatString.ProcFormatString,
             DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
             false,
             requestStub,
             sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                drsHandle = outParamList[0].ToIntPtr();
                dwInVersion = (IDL_DRSRemoveDsServer_dwInVersion_Values)(outParamList[1].ToUInt32());
                inMessage = TypeMarshal.ToNullableStruct<DRS_MSG_RMSVRREQ>(
                    outParamList[2], dwInVersion, null, null);
            }
        }
    }


    /// <summary>
    /// The DrsRemoveDsServerResponse class defines output parameters of method DrsRemoveDsServer.
    /// </summary>
    public class DrsRemoveDsServerResponse : DrsuapiResponseStub
    {
        /// <summary>
        /// outVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public IDL_DRSRemoveDsServer_pdwOutVersion_Values? outVersion;

        /// <summary>
        /// outMessage parameter.
        /// </summary>
        public DRS_MSG_RMSVRREPLY? outMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsRemoveDsServerResponse()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsRemoveDsServer;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            SafeIntPtr ptrOutVersion = TypeMarshal.ToIntPtr(outVersion);
            SafeIntPtr ptrOutMessage = TypeMarshal.ToIntPtr(outMessage, outVersion, null, null);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ptrOutVersion,
                    ptrOutMessage,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                ptrOutVersion.Dispose();
                ptrOutMessage.Dispose();
            }
        }
    }


    /// <summary>
    /// The DrsRemoveDsDomainRequest class defines input parameters of method DrsRemoveDsDomain.
    /// </summary>
    public class DrsRemoveDsDomainRequest : DrsuapiRequestStub
    {
        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr drsHandle;

        /// <summary>
        /// dwInVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public IDL_DRSRemoveDsDomain_dwInVersion_Values dwInVersion;

        /// <summary>
        /// inMessage parameter.
        /// </summary>
        public DRS_MSG_RMDMNREQ? inMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsRemoveDsDomainRequest()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsRemoveDsDomain;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
             RpceStubHelper.GetPlatform(),
             DrsrRpcStubFormatString.TypeFormatString,
             null,
             DrsrRpcStubFormatString.ProcFormatString,
             DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
             false,
             requestStub,
             sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                drsHandle = outParamList[0].ToIntPtr();
                dwInVersion = (IDL_DRSRemoveDsDomain_dwInVersion_Values)(outParamList[1].ToUInt32());
                inMessage = TypeMarshal.ToNullableStruct<DRS_MSG_RMDMNREQ>(
                    outParamList[2], dwInVersion, null, null);
            }
        }
    }


    /// <summary>
    /// The DrsRemoveDsDomainResponse class defines output parameters of method DrsRemoveDsDomain.
    /// </summary>
    public class DrsRemoveDsDomainResponse : DrsuapiResponseStub
    {
        /// <summary>
        /// outVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public IDL_DRSRemoveDsDomain_pdwOutVersion_Values? outVersion;

        /// <summary>
        /// outMessage parameter.
        /// </summary>
        public DRS_MSG_RMDMNREPLY? outMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsRemoveDsDomainResponse()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsRemoveDsDomain;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            SafeIntPtr ptrOutVersion = TypeMarshal.ToIntPtr(outVersion);
            SafeIntPtr ptrOutMessage = TypeMarshal.ToIntPtr(outMessage, outVersion, null, null);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ptrOutVersion,
                    ptrOutMessage,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                ptrOutVersion.Dispose();
                ptrOutMessage.Dispose();
            }
        }
    }


    /// <summary>
    /// The DrsDomainControllerInfoRequest class defines input parameters of method DrsDomainControllerInfo.
    /// </summary>
    public class DrsDomainControllerInfoRequest : DrsuapiRequestStub
    {
        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr drsHandle;

        /// <summary>
        /// dwInVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwInVersion;

        /// <summary>
        /// inMessage parameter.
        /// </summary>
        public DRS_MSG_DCINFOREQ? inMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsDomainControllerInfoRequest()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsDomainControllerInfo;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
             RpceStubHelper.GetPlatform(),
             DrsrRpcStubFormatString.TypeFormatString,
             null,
             DrsrRpcStubFormatString.ProcFormatString,
             DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
             false,
             requestStub,
             sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                drsHandle = outParamList[0].ToIntPtr();
                dwInVersion = outParamList[1].ToUInt32();
                inMessage = TypeMarshal.ToNullableStruct<DRS_MSG_DCINFOREQ>(
                    outParamList[2], dwInVersion, null, null);
            }
        }
    }


    /// <summary>
    /// The DrsDomainControllerInfoResponse class defines output parameters of method DrsDomainControllerInfo.
    /// </summary>
    public class DrsDomainControllerInfoResponse : DrsuapiResponseStub
    {
        /// <summary>
        /// outVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint? outVersion;

        /// <summary>
        /// outMessage parameter.
        /// </summary>
        public DRS_MSG_DCINFOREPLY? outMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsDomainControllerInfoResponse()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsDomainControllerInfo;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            SafeIntPtr ptrOutVersion = TypeMarshal.ToIntPtr(outVersion);
            SafeIntPtr ptrOutMessage = TypeMarshal.ToIntPtr(outMessage, outVersion, null, null);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ptrOutVersion,
                    ptrOutMessage,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                ptrOutVersion.Dispose();
                ptrOutMessage.Dispose();
            }
        }
    }


    /// <summary>
    /// The DrsExecuteKccRequest class defines input parameters of method DrsExecuteKcc.
    /// </summary>
    public class DrsExecuteKccRequest : DrsuapiRequestStub
    {
        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr drsHandle;

        /// <summary>
        /// dwInVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwInVersion;

        /// <summary>
        /// inMessage parameter.
        /// </summary>
        public DRS_MSG_KCC_EXECUTE? inMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsExecuteKccRequest()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsExecuteKcc;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
             RpceStubHelper.GetPlatform(),
             DrsrRpcStubFormatString.TypeFormatString,
             null,
             DrsrRpcStubFormatString.ProcFormatString,
             DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
             false,
             requestStub,
             sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                drsHandle = outParamList[0].ToIntPtr();
                dwInVersion = outParamList[1].ToUInt32();
                inMessage = TypeMarshal.ToNullableStruct<DRS_MSG_KCC_EXECUTE>(
                    outParamList[2], dwInVersion, null, null);
            }
        }
    }


    /// <summary>
    /// The DrsExecuteKccResponse class defines output parameters of method DrsExecuteKcc.
    /// </summary>
    public class DrsExecuteKccResponse : DrsuapiResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsExecuteKccResponse()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsExecuteKcc;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
            }
        }
    }


    /// <summary>
    /// The DrsGetReplInfoRequest class defines input parameters of method DrsGetReplInfo.
    /// </summary>
    public class DrsGetReplInfoRequest : DrsuapiRequestStub
    {
        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr drsHandle;

        /// <summary>
        /// dwInVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwInVersion;

        /// <summary>
        /// inMessage parameter.
        /// </summary>
        public DRS_MSG_GETREPLINFO_REQ? inMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsGetReplInfoRequest()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsGetReplInfo;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
             RpceStubHelper.GetPlatform(),
             DrsrRpcStubFormatString.TypeFormatString,
             null,
             DrsrRpcStubFormatString.ProcFormatString,
             DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
             false,
             requestStub,
             sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                drsHandle = outParamList[0].ToIntPtr();
                dwInVersion = outParamList[1].ToUInt32();
                inMessage = TypeMarshal.ToNullableStruct<DRS_MSG_GETREPLINFO_REQ>(
                    outParamList[2], dwInVersion, null, null);
            }
        }
    }


    /// <summary>
    /// The DrsGetReplInfoResponse class defines output parameters of method DrsGetReplInfo.
    /// </summary>
    public class DrsGetReplInfoResponse : DrsuapiResponseStub
    {
        /// <summary>
        /// outVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint? outVersion;

        /// <summary>
        /// outMessage parameter.
        /// </summary>
        public DRS_MSG_GETREPLINFO_REPLY? outMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsGetReplInfoResponse()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsGetReplInfo;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            SafeIntPtr ptrOutVersion = TypeMarshal.ToIntPtr(outVersion);
            SafeIntPtr ptrOutMessage = TypeMarshal.ToIntPtr(outMessage, outVersion, null, null);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ptrOutVersion,
                    ptrOutMessage,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                ptrOutVersion.Dispose();
                ptrOutMessage.Dispose();
            }
        }
    }


    /// <summary>
    /// The DrsAddSidHistoryRequest class defines input parameters of method DrsAddSidHistory.
    /// </summary>
    public class DrsAddSidHistoryRequest : DrsuapiRequestStub
    {
        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr drsHandle;

        /// <summary>
        /// dwInVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public IDL_DRSAddSidHistory_dwInVersion_Values dwInVersion;

        /// <summary>
        /// inMessage parameter.
        /// </summary>
        public DRS_MSG_ADDSIDREQ? inMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsAddSidHistoryRequest()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsAddSidHistory;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
             RpceStubHelper.GetPlatform(),
             DrsrRpcStubFormatString.TypeFormatString,
             null,
             DrsrRpcStubFormatString.ProcFormatString,
             DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
             false,
             requestStub,
             sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                drsHandle = outParamList[0].ToIntPtr();
                dwInVersion = (IDL_DRSAddSidHistory_dwInVersion_Values)(outParamList[1].ToUInt32());
                inMessage = TypeMarshal.ToNullableStruct<DRS_MSG_ADDSIDREQ>(
                    outParamList[2], dwInVersion, null, null);
            }
        }
    }


    /// <summary>
    /// The DrsAddSidHistoryResponse class defines output parameters of method DrsAddSidHistory.
    /// </summary>
    public class DrsAddSidHistoryResponse : DrsuapiResponseStub
    {
        /// <summary>
        /// outVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public IDL_DRSAddSidHistory_pdwOutVersion_Values? outVersion;

        /// <summary>
        /// outMessage parameter.
        /// </summary>
        public DRS_MSG_ADDSIDREPLY? outMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsAddSidHistoryResponse()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsAddSidHistory;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            SafeIntPtr ptrOutVersion = TypeMarshal.ToIntPtr(outVersion);
            SafeIntPtr ptrOutMessage = TypeMarshal.ToIntPtr(outMessage, outVersion, null, null);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ptrOutVersion,
                    ptrOutMessage,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                ptrOutVersion.Dispose();
                ptrOutMessage.Dispose();
            }
        }
    }


    /// <summary>
    /// The DrsReplicaVerifyObjectsRequest class defines input parameters of method DrsReplicaVerifyObjects.
    /// </summary>
    public class DrsReplicaVerifyObjectsRequest : DrsuapiRequestStub
    {
        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr drsHandle;

        /// <summary>
        /// inVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint inVersion;

        /// <summary>
        /// inMessage parameter.
        /// </summary>
        public DRS_MSG_REPVERIFYOBJ? inMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsReplicaVerifyObjectsRequest()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsReplicaVerifyObjects;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
             RpceStubHelper.GetPlatform(),
             DrsrRpcStubFormatString.TypeFormatString,
             null,
             DrsrRpcStubFormatString.ProcFormatString,
             DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
             false,
             requestStub,
             sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                drsHandle = outParamList[0].ToIntPtr();
                inVersion = outParamList[1].ToUInt32();
                inMessage = TypeMarshal.ToNullableStruct<DRS_MSG_REPVERIFYOBJ>(
                    outParamList[2], inVersion, null, null);
            }
        }
    }


    /// <summary>
    /// The DrsReplicaVerifyObjectsResponse class defines output parameters of method DrsReplicaVerifyObjects.
    /// </summary>
    public class DrsReplicaVerifyObjectsResponse : DrsuapiResponseStub
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsReplicaVerifyObjectsResponse()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsReplicaVerifyObjects;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
            }
        }
    }


    /// <summary>
    /// The DrsQuerySitesByCostRequest class defines input parameters of method DrsQuerySitesByCost.
    /// </summary>
    public class DrsQuerySitesByCostRequest : DrsuapiRequestStub
    {
        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr drsHandle;

        /// <summary>
        /// dwInVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwInVersion;

        /// <summary>
        /// inMessage parameter.
        /// </summary>
        public DRS_MSG_QUERYSITESREQ? inMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsQuerySitesByCostRequest()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsQuerySitesByCost;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
             RpceStubHelper.GetPlatform(),
             DrsrRpcStubFormatString.TypeFormatString,
             null,
             DrsrRpcStubFormatString.ProcFormatString,
             DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
             false,
             requestStub,
             sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                drsHandle = outParamList[0].ToIntPtr();
                dwInVersion = outParamList[1].ToUInt32();
                inMessage = TypeMarshal.ToNullableStruct<DRS_MSG_QUERYSITESREQ>(
                    outParamList[2], dwInVersion, null, null);
            }
        }
    }


    /// <summary>
    /// The DrsQuerySitesByCostResponse class defines output parameters of method DrsQuerySitesByCost.
    /// </summary>
    public class DrsQuerySitesByCostResponse : DrsuapiResponseStub
    {
        /// <summary>
        /// outVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint? outVersion;

        /// <summary>
        /// outMessage parameter.
        /// </summary>
        public DRS_MSG_QUERYSITESREPLY? outMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsQuerySitesByCostResponse()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsQuerySitesByCost;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            SafeIntPtr ptrOutVersion = TypeMarshal.ToIntPtr(outVersion);
            SafeIntPtr ptrOutMessage = TypeMarshal.ToIntPtr(outMessage, outVersion, null, null);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ptrOutVersion,
                    ptrOutMessage,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                ptrOutVersion.Dispose();
                ptrOutMessage.Dispose();
            }
        }
    }


    /// <summary>
    /// The DrsInitDemotionRequest class defines input parameters of method DrsInitDemotion.
    /// </summary>
    public class DrsInitDemotionRequest : DrsuapiRequestStub
    {
        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr drsHandle;

        /// <summary>
        /// dwInVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwInVersion;

        /// <summary>
        /// inMessage parameter.
        /// </summary>
        public DRS_MSG_INIT_DEMOTIONREQ? inMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsInitDemotionRequest()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsInitDemotion;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
             RpceStubHelper.GetPlatform(),
             DrsrRpcStubFormatString.TypeFormatString,
             null,
             DrsrRpcStubFormatString.ProcFormatString,
             DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
             false,
             requestStub,
             sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                drsHandle = outParamList[0].ToIntPtr();
                dwInVersion = outParamList[1].ToUInt32();
                inMessage = TypeMarshal.ToNullableStruct<DRS_MSG_INIT_DEMOTIONREQ>(
                    outParamList[2], dwInVersion, null, null);
            }
        }
    }


    /// <summary>
    /// The DrsInitDemotionResponse class defines output parameters of method DrsInitDemotion.
    /// </summary>
    public class DrsInitDemotionResponse : DrsuapiResponseStub
    {
        /// <summary>
        /// outVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint? outVersion;

        /// <summary>
        /// outMessage parameter.
        /// </summary>
        public DRS_MSG_INIT_DEMOTIONREPLY? outMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsInitDemotionResponse()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsInitDemotion;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            SafeIntPtr ptrOutVersion = TypeMarshal.ToIntPtr(outVersion);
            SafeIntPtr ptrOutMessage = TypeMarshal.ToIntPtr(outMessage, outVersion, null, null);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ptrOutVersion,
                    ptrOutMessage,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                ptrOutVersion.Dispose();
                ptrOutMessage.Dispose();
            }
        }
    }


    /// <summary>
    /// The DrsReplicaDemotionRequest class defines input parameters of method DrsReplicaDemotion.
    /// </summary>
    public class DrsReplicaDemotionRequest : DrsuapiRequestStub
    {
        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr drsHandle;

        /// <summary>
        /// dwInVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwInVersion;

        /// <summary>
        /// inMessage parameter.
        /// </summary>
        public DRS_MSG_REPLICA_DEMOTIONREQ? inMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsReplicaDemotionRequest()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsReplicaDemotion;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
             RpceStubHelper.GetPlatform(),
             DrsrRpcStubFormatString.TypeFormatString,
             null,
             DrsrRpcStubFormatString.ProcFormatString,
             DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
             false,
             requestStub,
             sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                drsHandle = outParamList[0].ToIntPtr();
                dwInVersion = outParamList[1].ToUInt32();
                inMessage = TypeMarshal.ToNullableStruct<DRS_MSG_REPLICA_DEMOTIONREQ>(
                    outParamList[2], dwInVersion, null, null);
            }
        }
    }


    /// <summary>
    /// The DrsReplicaDemotionResponse class defines output parameters of method DrsReplicaDemotion.
    /// </summary>
    public class DrsReplicaDemotionResponse : DrsuapiResponseStub
    {
        /// <summary>
        /// outVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint? outVersion;

        /// <summary>
        /// outMessage parameter.
        /// </summary>
        public DRS_MSG_REPLICA_DEMOTIONREPLY? outMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsReplicaDemotionResponse()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsReplicaDemotion;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            SafeIntPtr ptrOutVersion = TypeMarshal.ToIntPtr(outVersion);
            SafeIntPtr ptrOutMessage = TypeMarshal.ToIntPtr(outMessage, outVersion, null, null);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ptrOutVersion,
                    ptrOutMessage,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                ptrOutVersion.Dispose();
                ptrOutMessage.Dispose();
            }
        }
    }


    /// <summary>
    /// The DrsFinishDemotionRequest class defines input parameters of method DrsFinishDemotion.
    /// </summary>
    public class DrsFinishDemotionRequest : DrsuapiRequestStub
    {
        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr drsHandle;

        /// <summary>
        /// dwInVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwInVersion;

        /// <summary>
        /// inMessage parameter.
        /// </summary>
        public DRS_MSG_FINISH_DEMOTIONREQ? inMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsFinishDemotionRequest()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsFinishDemotion;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
             RpceStubHelper.GetPlatform(),
             DrsrRpcStubFormatString.TypeFormatString,
             null,
             DrsrRpcStubFormatString.ProcFormatString,
             DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
             false,
             requestStub,
             sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                drsHandle = outParamList[0].ToIntPtr();
                dwInVersion = outParamList[1].ToUInt32();
                inMessage = TypeMarshal.ToNullableStruct<DRS_MSG_FINISH_DEMOTIONREQ>(
                    outParamList[2], dwInVersion, null, null);
            }
        }
    }


    /// <summary>
    /// The DrsFinishDemotionResponse class defines output parameters of method DrsFinishDemotion.
    /// </summary>
    public class DrsFinishDemotionResponse : DrsuapiResponseStub
    {
        /// <summary>
        /// outVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint? outVersion;

        /// <summary>
        /// outMessage parameter.
        /// </summary>
        public DRS_MSG_FINISH_DEMOTIONREPLY? outMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsFinishDemotionResponse()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsFinishDemotion;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            SafeIntPtr ptrOutVersion = TypeMarshal.ToIntPtr(outVersion);
            SafeIntPtr ptrOutMessage = TypeMarshal.ToIntPtr(outMessage, outVersion, null, null);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ptrOutVersion,
                    ptrOutMessage,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                ptrOutVersion.Dispose();
                ptrOutMessage.Dispose();
            }
        }
    }


    /// <summary>
    /// The DrsGetNcChangesRequest class defines input parameters of method DrsGetNcChanges.
    /// </summary>
    public class DrsGetNcChangesRequest : DrsuapiRequestStub
    {
        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr drsHandle;

        /// <summary>
        /// dwInVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwInVersion;

        /// <summary>
        /// inMessage parameter.
        /// </summary>
        public DRS_MSG_GETCHGREQ? inMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsGetNcChangesRequest()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsGetNcChanges;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
             RpceStubHelper.GetPlatform(),
             DrsrRpcStubFormatString.TypeFormatString,
             null,
             DrsrRpcStubFormatString.ProcFormatString,
             DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
             false,
             requestStub,
             sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                drsHandle = outParamList[0].ToIntPtr();
                dwInVersion = outParamList[1].ToUInt32();
                inMessage = TypeMarshal.ToNullableStruct<DRS_MSG_GETCHGREQ>(
                    outParamList[2], dwInVersion, null, null);
            }
        }
    }


    /// <summary>
    /// The DrsGetNcChangesResponse class defines output parameters of method DrsGetNcChanges.
    /// </summary>
    public class DrsGetNcChangesResponse : DrsuapiResponseStub
    {
        /// <summary>
        /// outVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint? outVersion;

        /// <summary>
        /// outMessage parameter.
        /// </summary>
        public DRS_MSG_GETCHGREPLY? outMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsGetNcChangesResponse()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsGetNcChanges;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            SafeIntPtr ptrOutVersion = TypeMarshal.ToIntPtr(outVersion);
            SafeIntPtr ptrOutMessage = TypeMarshal.ToIntPtr(outMessage, outVersion, null, null);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ptrOutVersion,
                    ptrOutMessage,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                ptrOutVersion.Dispose();
                ptrOutMessage.Dispose();
            }
        }
    }


    /// <summary>
    /// The DrsVerifyNamesRequest class defines input parameters of method DrsVerifyNames.
    /// </summary>
    public class DrsVerifyNamesRequest : DrsuapiRequestStub
    {
        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr drsHandle;

        /// <summary>
        /// dwInVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwInVersion;

        /// <summary>
        /// inMessage parameter.
        /// </summary>
        public DRS_MSG_VERIFYREQ? inMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsVerifyNamesRequest()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsVerifyNames;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
             RpceStubHelper.GetPlatform(),
             DrsrRpcStubFormatString.TypeFormatString,
             null,
             DrsrRpcStubFormatString.ProcFormatString,
             DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
             false,
             requestStub,
             sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                drsHandle = outParamList[0].ToIntPtr();
                dwInVersion = outParamList[1].ToUInt32();
                inMessage = TypeMarshal.ToNullableStruct<DRS_MSG_VERIFYREQ>(
                    outParamList[2], dwInVersion, null, null);
            }
        }
    }


    /// <summary>
    /// The DrsVerifyNamesResponse class defines output parameters of method DrsVerifyNames.
    /// </summary>
    public class DrsVerifyNamesResponse : DrsuapiResponseStub
    {
        /// <summary>
        /// outVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint? outVersion;

        /// <summary>
        /// outMessage parameter.
        /// </summary>
        public DRS_MSG_VERIFYREPLY? outMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsVerifyNamesResponse()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsVerifyNames;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            SafeIntPtr ptrOutVersion = TypeMarshal.ToIntPtr(outVersion);
            SafeIntPtr ptrOutMessage = TypeMarshal.ToIntPtr(outMessage, outVersion, null, null);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ptrOutVersion,
                    ptrOutMessage,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                ptrOutVersion.Dispose();
                ptrOutMessage.Dispose();
            }
        }
    }


    /// <summary>
    /// The DrsGetMembershipsRequest class defines input parameters of method DrsGetMemberships.
    /// </summary>
    public class DrsGetMembershipsRequest : DrsuapiRequestStub
    {
        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr drsHandle;

        /// <summary>
        /// dwInVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwInVersion;

        /// <summary>
        /// inMessage parameter.
        /// </summary>
        public DRS_MSG_REVMEMB_REQ? inMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsGetMembershipsRequest()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsGetMemberships;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
             RpceStubHelper.GetPlatform(),
             DrsrRpcStubFormatString.TypeFormatString,
             null,
             DrsrRpcStubFormatString.ProcFormatString,
             DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
             false,
             requestStub,
             sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                drsHandle = outParamList[0].ToIntPtr();
                dwInVersion = outParamList[1].ToUInt32();
                inMessage = TypeMarshal.ToNullableStruct<DRS_MSG_REVMEMB_REQ>(
                    outParamList[2], dwInVersion, null, null);
            }
        }
    }


    /// <summary>
    /// The DrsGetMembershipsResponse class defines output parameters of method DrsGetMemberships.
    /// </summary>
    public class DrsGetMembershipsResponse : DrsuapiResponseStub
    {
        /// <summary>
        /// outVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint? outVersion;

        /// <summary>
        /// outMessage parameter.
        /// </summary>
        public DRS_MSG_REVMEMB_REPLY? outMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsGetMembershipsResponse()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsGetMemberships;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            SafeIntPtr ptrOutVersion = TypeMarshal.ToIntPtr(outVersion);
            SafeIntPtr ptrOutMessage = TypeMarshal.ToIntPtr(outMessage, outVersion, null, null);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ptrOutVersion,
                    ptrOutMessage,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                ptrOutVersion.Dispose();
                ptrOutMessage.Dispose();
            }
        }
    }


    /// <summary>
    /// The DrsInterDomainMoveRequest class defines input parameters of method DrsInterDomainMove.
    /// </summary>
    public class DrsInterDomainMoveRequest : DrsuapiRequestStub
    {
        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr drsHandle;

        /// <summary>
        /// dwInVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwInVersion;

        /// <summary>
        /// inMessage parameter.
        /// </summary>
        public DRS_MSG_MOVEREQ? inMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsInterDomainMoveRequest()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsInterDomainMove;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
             RpceStubHelper.GetPlatform(),
             DrsrRpcStubFormatString.TypeFormatString,
             null,
             DrsrRpcStubFormatString.ProcFormatString,
             DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
             false,
             requestStub,
             sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                drsHandle = outParamList[0].ToIntPtr();
                dwInVersion = outParamList[1].ToUInt32();
                inMessage = TypeMarshal.ToNullableStruct<DRS_MSG_MOVEREQ>(
                    outParamList[2], dwInVersion, null, null);
            }
        }
    }


    /// <summary>
    /// The DrsInterDomainMoveResponse class defines output parameters of method DrsInterDomainMove.
    /// </summary>
    public class DrsInterDomainMoveResponse : DrsuapiResponseStub
    {
        /// <summary>
        /// outVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint? outVersion;

        /// <summary>
        /// outMessage parameter.
        /// </summary>
        public DRS_MSG_MOVEREPLY? outMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsInterDomainMoveResponse()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsInterDomainMove;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            SafeIntPtr ptrOutVersion = TypeMarshal.ToIntPtr(outVersion);
            SafeIntPtr ptrOutMessage = TypeMarshal.ToIntPtr(outMessage, outVersion, null, null);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ptrOutVersion,
                    ptrOutMessage,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                ptrOutVersion.Dispose();
                ptrOutMessage.Dispose();
            }
        }
    }


    /// <summary>
    /// The DrsGetNt4ChangeLogRequest class defines input parameters of method DrsGetNt4ChangeLog.
    /// </summary>
    public class DrsGetNt4ChangeLogRequest : DrsuapiRequestStub
    {
        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr drsHandle;

        /// <summary>
        /// dwInVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwInVersion;

        /// <summary>
        /// inMessage parameter.
        /// </summary>
        public DRS_MSG_NT4_CHGLOG_REQ? inMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsGetNt4ChangeLogRequest()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsGetNt4ChangeLog;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
             RpceStubHelper.GetPlatform(),
             DrsrRpcStubFormatString.TypeFormatString,
             null,
             DrsrRpcStubFormatString.ProcFormatString,
             DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
             false,
             requestStub,
             sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                drsHandle = outParamList[0].ToIntPtr();
                dwInVersion = outParamList[1].ToUInt32();
                inMessage = TypeMarshal.ToNullableStruct<DRS_MSG_NT4_CHGLOG_REQ>(
                    outParamList[2], dwInVersion, null, null);
            }
        }
    }


    /// <summary>
    /// The DrsGetNt4ChangeLogResponse class defines output parameters of method DrsGetNt4ChangeLog.
    /// </summary>
    public class DrsGetNt4ChangeLogResponse : DrsuapiResponseStub
    {
        /// <summary>
        /// outVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint? outVersion;

        /// <summary>
        /// outMessage parameter.
        /// </summary>
        public DRS_MSG_NT4_CHGLOG_REPLY? outMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsGetNt4ChangeLogResponse()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsGetNt4ChangeLog;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            SafeIntPtr ptrOutVersion = TypeMarshal.ToIntPtr(outVersion);
            SafeIntPtr ptrOutMessage = TypeMarshal.ToIntPtr(outMessage, outVersion, null, null);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ptrOutVersion,
                    ptrOutMessage,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                ptrOutVersion.Dispose();
                ptrOutMessage.Dispose();
            }
        }
    }


    /// <summary>
    /// The DrsAddEntryRequest class defines input parameters of method DrsAddEntry.
    /// </summary>
    public class DrsAddEntryRequest : DrsuapiRequestStub
    {
        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr drsHandle;

        /// <summary>
        /// dwInVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwInVersion;

        /// <summary>
        /// inMessage parameter.
        /// </summary>
        public DRS_MSG_ADDENTRYREQ? inMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsAddEntryRequest()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsAddEntry;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
             RpceStubHelper.GetPlatform(),
             DrsrRpcStubFormatString.TypeFormatString,
             null,
             DrsrRpcStubFormatString.ProcFormatString,
             DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
             false,
             requestStub,
             sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                drsHandle = outParamList[0].ToIntPtr();
                dwInVersion = outParamList[1].ToUInt32();
                inMessage = TypeMarshal.ToNullableStruct<DRS_MSG_ADDENTRYREQ>(
                    outParamList[2], dwInVersion, null, null);
            }
        }
    }


    /// <summary>
    /// The DrsAddEntryResponse class defines output parameters of method DrsAddEntry.
    /// </summary>
    public class DrsAddEntryResponse : DrsuapiResponseStub
    {
        /// <summary>
        /// outVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint? outVersion;

        /// <summary>
        /// outMessage parameter.
        /// </summary>
        public DRS_MSG_ADDENTRYREPLY? outMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsAddEntryResponse()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsAddEntry;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            SafeIntPtr ptrOutVersion = TypeMarshal.ToIntPtr(outVersion);
            SafeIntPtr ptrOutMessage = TypeMarshal.ToIntPtr(outMessage, outVersion, null, null);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ptrOutVersion,
                    ptrOutMessage,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                ptrOutVersion.Dispose();
                ptrOutMessage.Dispose();
            }
        }
    }


    /// <summary>
    /// The DrsGetMemberships2Request class defines input parameters of method DrsGetMemberships2.
    /// </summary>
    public class DrsGetMemberships2Request : DrsuapiRequestStub
    {
        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr drsHandle;

        /// <summary>
        /// dwInVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwInVersion;

        /// <summary>
        /// inMessage parameter.
        /// </summary>
        public DRS_MSG_GETMEMBERSHIPS2_REQ? inMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsGetMemberships2Request()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsGetMemberships2;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
             RpceStubHelper.GetPlatform(),
             DrsrRpcStubFormatString.TypeFormatString,
             null,
             DrsrRpcStubFormatString.ProcFormatString,
             DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
             false,
             requestStub,
             sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                drsHandle = outParamList[0].ToIntPtr();
                dwInVersion = outParamList[1].ToUInt32();
                inMessage = TypeMarshal.ToNullableStruct<DRS_MSG_GETMEMBERSHIPS2_REQ>(
                    outParamList[2], dwInVersion, null, null);
            }
        }
    }


    /// <summary>
    /// The DrsGetMemberships2Response class defines output parameters of method DrsGetMemberships2.
    /// </summary>
    public class DrsGetMemberships2Response : DrsuapiResponseStub
    {
        /// <summary>
        /// outVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint? outVersion;

        /// <summary>
        /// outMessage parameter.
        /// </summary>
        public DRS_MSG_GETMEMBERSHIPS2_REPLY? outMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsGetMemberships2Response()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsGetMemberships2;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            SafeIntPtr ptrOutVersion = TypeMarshal.ToIntPtr(outVersion);
            SafeIntPtr ptrOutMessage = TypeMarshal.ToIntPtr(outMessage, outVersion, null, null);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ptrOutVersion,
                    ptrOutMessage,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                ptrOutVersion.Dispose();
                ptrOutMessage.Dispose();
            }
        }
    }


    /// <summary>
    /// The DrsGetObjectExistenceRequest class defines input parameters of method DrsGetObjectExistence.
    /// </summary>
    public class DrsGetObjectExistenceRequest : DrsuapiRequestStub
    {
        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr drsHandle;

        /// <summary>
        /// dwInVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwInVersion;

        /// <summary>
        /// inMessage parameter.
        /// </summary>
        public DRS_MSG_EXISTREQ? inMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsGetObjectExistenceRequest()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsGetObjectExistence;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
             RpceStubHelper.GetPlatform(),
             DrsrRpcStubFormatString.TypeFormatString,
             null,
             DrsrRpcStubFormatString.ProcFormatString,
             DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
             false,
             requestStub,
             sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                drsHandle = outParamList[0].ToIntPtr();
                dwInVersion = outParamList[1].ToUInt32();
                inMessage = TypeMarshal.ToNullableStruct<DRS_MSG_EXISTREQ>(
                    outParamList[2], dwInVersion, null, null);
            }
        }
    }


    /// <summary>
    /// The DrsGetObjectExistenceResponse class defines output parameters of method DrsGetObjectExistence.
    /// </summary>
    public class DrsGetObjectExistenceResponse : DrsuapiResponseStub
    {
        /// <summary>
        /// outVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint? outVersion;

        /// <summary>
        /// outMessage parameter.
        /// </summary>
        public DRS_MSG_EXISTREPLY? outMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DrsGetObjectExistenceResponse()
        {
            Opnum = (ushort)DrsuapiMethodOpnums.DrsGetObjectExistence;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            SafeIntPtr ptrOutVersion = TypeMarshal.ToIntPtr(outVersion);
            SafeIntPtr ptrOutMessage = TypeMarshal.ToIntPtr(outMessage, outVersion, null, null);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ptrOutVersion,
                    ptrOutMessage,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Drsuapi_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                ptrOutVersion.Dispose();
                ptrOutMessage.Dispose();
            }
        }
    }

    #endregion

    #endregion


    #region DRSR dsaop methods

    /// <summary>
    /// The base class of all dsaop request
    /// </summary>
    public abstract class DsaopRequestStub : DrsrRequestStub
    {
    }


    /// <summary>
    /// The base class of all dsaop response
    /// </summary>
    public abstract class DsaopResponseStub : DrsrResponseStub
    {
    }


    #region Structures of input and output parameters of dsaop methods

    /// <summary>
    /// The DsaPrepareScriptRequest class defines input parameters of method DsaPrepareScript.
    /// </summary>
    public class DsaPrepareScriptRequest : DsaopRequestStub
    {
        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr rpcHandle;

        /// <summary>
        /// dwInVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwInVersion;

        /// <summary>
        /// inMessage parameter.
        /// </summary>
        public DSA_MSG_PREPARE_SCRIPT_REQ? inMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DsaPrepareScriptRequest()
        {
            Opnum = (ushort)DsaopMethodOpnums.DsaPrepareScript;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
             RpceStubHelper.GetPlatform(),
             DrsrRpcStubFormatString.TypeFormatString,
             null,
             DrsrRpcStubFormatString.ProcFormatString,
             DrsrRpcStubFormatString.Dsaop_ProcFormatStringOffsetTable[(int)Opnum],
             false,
             requestStub,
             sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                rpcHandle = sessionContext.RpceLayerSessionContext.Handle;
                dwInVersion = outParamList[0].ToUInt32();
                inMessage = TypeMarshal.ToNullableStruct<DSA_MSG_PREPARE_SCRIPT_REQ>(
                    outParamList[1], dwInVersion, null, null);
            }
        }
    }


    /// <summary>
    /// The DsaPrepareScriptResponse class defines output parameters of method DsaPrepareScript.
    /// </summary>
    public class DsaPrepareScriptResponse : DsaopResponseStub
    {
        /// <summary>
        /// outVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint? outVersion;

        /// <summary>
        /// outMessage parameter.
        /// </summary>
        public DSA_MSG_PREPARE_SCRIPT_REPLY? outMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DsaPrepareScriptResponse()
        {
            Opnum = (ushort)DsaopMethodOpnums.DsaPrepareScript;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            SafeIntPtr ptrOutVersion = TypeMarshal.ToIntPtr(outVersion);
            SafeIntPtr ptrOutMessage = TypeMarshal.ToIntPtr(outMessage, outVersion, null, null);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ptrOutVersion,
                    ptrOutMessage,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Dsaop_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                ptrOutVersion.Dispose();
                ptrOutMessage.Dispose();
            }
        }
    }


    /// <summary>
    /// The DsaExecuteScriptRequest class defines input parameters of method DsaExecuteScript.
    /// </summary>
    public class DsaExecuteScriptRequest : DsaopRequestStub
    {
        /// <summary>
        ///  drsHandle parameter.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr rpcHandle;

        /// <summary>
        /// dwInVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwInVersion;

        /// <summary>
        /// inMessage parameter.
        /// </summary>
        public DSA_MSG_EXECUTE_SCRIPT_REQ? inMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DsaExecuteScriptRequest()
        {
            Opnum = (ushort)DsaopMethodOpnums.DsaExecuteScript;
        }


        /// <summary>
        ///  Decodes the request stub, and fills the fields of the class
        /// </summary>
        /// <param name="sessionContext">The session context of the request received</param>
        /// <param name="requestStub">The request stub got from RPCE layer</param>
        internal override void Decode(DrsrServerSessionContext sessionContext, byte[] requestStub)
        {
            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
             RpceStubHelper.GetPlatform(),
             DrsrRpcStubFormatString.TypeFormatString,
             null,
             DrsrRpcStubFormatString.ProcFormatString,
             DrsrRpcStubFormatString.Dsaop_ProcFormatStringOffsetTable[(int)Opnum],
             false,
             requestStub,
             sessionContext.RpceLayerSessionContext.PackedDataRepresentationFormat))
            {
                rpcHandle = sessionContext.RpceLayerSessionContext.Handle;
                dwInVersion = outParamList[0].ToUInt32();
                inMessage = TypeMarshal.ToNullableStruct<DSA_MSG_EXECUTE_SCRIPT_REQ>(
                    outParamList[1], dwInVersion, null, null);
            }
        }
    }


    /// <summary>
    /// The DsaExecuteScriptResponse class defines output parameters of method DsaExecuteScript.
    /// </summary>
    public class DsaExecuteScriptResponse : DsaopResponseStub
    {
        /// <summary>
        /// outVersion parameter.
        /// </summary>
        [CLSCompliant(false)]
        public uint? outVersion;

        /// <summary>
        /// outMessage parameter.
        /// </summary>
        public DSA_MSG_EXECUTE_SCRIPT_REPLY? outMessage;


        /// <summary>
        /// Constructor method
        /// </summary>
        public DsaExecuteScriptResponse()
        {
            Opnum = (ushort)DsaopMethodOpnums.DsaExecuteScript;
        }


        /// <summary>
        ///  Encodes the response stub
        /// </summary>
        /// <param name="sessionContext">The session context of the response to send</param>
        /// <returns>The response stub</returns>
        internal override byte[] Encode(DrsrServerSessionContext sessionContext)
        {
            SafeIntPtr ptrOutVersion = TypeMarshal.ToIntPtr(outVersion);
            SafeIntPtr ptrOutMessage = TypeMarshal.ToIntPtr(outMessage, outVersion, null, null);

            try
            {
                Int3264[] paramList = new Int3264[] 
                {
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ptrOutVersion,
                    ptrOutMessage,
                    Status
                };

                return RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Dsaop_ProcFormatStringOffsetTable[(int)Opnum],
                    false,
                    paramList);
            }
            finally
            {
                ptrOutVersion.Dispose();
                ptrOutMessage.Dispose();
            }
        }
    }

    #endregion

    #endregion
}
