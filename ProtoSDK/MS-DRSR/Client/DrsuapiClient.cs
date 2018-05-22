// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr
{
    /// <summary>
    /// DRSR client for drsuapi methods.
    /// You can call following methods from this class.<para/>
    /// RPC bind methods.<para/>
    /// DRSR RPC methods.<para/>
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public class DrsuapiClient : IDisposable
    {

        //Actual rpc adapter
        internal IDrsuapiRpcAdapter rpcAdapter;


        #region Constructor

        /// <summary>
        /// Constructor, initialize a DRSR client.<para/>
        /// Create the instance will not connect to server, you should call BindOverTcp to
        /// actually connect to DRSR server.
        /// </summary>       
        public DrsuapiClient(IDrsuapiRpcAdapter drsRpcAdapter = null)
        {
            if (drsRpcAdapter == null)
            {
                rpcAdapter = new DrsuapiRpcAdapter();
            }
            else
            {
                rpcAdapter = drsRpcAdapter;
            }
        }

        #endregion


        #region RPC bind methods

        /// <summary>
        /// RPC bind over TCP/IP, using specified endpoint and authenticate provider.
        /// using static endpoint (AD DS?)
        /// </summary>
        /// <param name="serverName">DRSR server machine name.</param>
        /// <param name="securityContext">
        /// Security provider for RPC. 
        /// Set the value to null to disable authentication.
        /// </param>
        /// <param name="authenticationLevel">
        /// Authentication level for RPC. 
        /// </param>
        /// <param name="timeout">Timeout for bind and all future requests.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when serverName is null.
        /// </exception>
        /// <returns>DrsrClientSessionContext after tcp bind</returns>
        [CLSCompliant(false)]
        public DrsrClientSessionContext BindOverTcp(
            string serverName,
            ClientSecurityContext securityContext,
            RpceAuthenticationLevel authenticationLevel,
            TimeSpan timeout)
        {
            DrsrClientSessionContext clientSessionContext = new DrsrClientSessionContext();

            if (serverName == null)
            {
                throw new ArgumentNullException("serverName");
            }



            this.rpcAdapter.Bind(
                RpceUtility.RPC_OVER_TCPIP_PROTOCOL_SEQUENCE,
                serverName,
                DrsrUtility.QueryDrsrTcpEndpoint(DrsrRpcInterfaceType.DRSUAPI, serverName)[0].ToString(),
                securityContext,
                authenticationLevel,
                timeout);

            clientSessionContext.RPCHandle = rpcAdapter.Handle;

            return clientSessionContext;
        }

        /// <summary>
        /// TO Do: need to test whether the dynamic endpoint can be queried by DrsrUtility.QueryDrsrTcpEndpoint
        /// To Do: need to test the two BindOverTcp is for AD DS, AD LDS or for fixed endpoint and dynamic endpoint
        /// To Do: if they are for AD DS and AD LDS, change the name to BindForDS" and "BindForLDS for better understand
        /// RPC bind over TCP/IP, using specified endpoint and authenticate provider.
        /// using dynamic endpoint(AD LDS?)
        /// </summary>
        /// <param name="serverName">DRSR server machine name.</param>
        /// <param name="servicePrincipalName">DRSR server machine Principal name.</param>
        /// <param name="clientGuid">DRS client Guid</param>
        /// <param name="protocolSequence">protocol sequence type</param>
        /// <param name="endPoint">DRSR endpoint, the value can be null when it is AD LDS?</param>
        /// <param name="networkOptions">a string representation of network options. The option string is associated with the protocol sequence.
        ///                              It is used in RpcStringBindingCompose</param>
        /// <param name="securityContext">
        /// Security provider for RPC. 
        /// Set the value to null to disable authentication.
        /// </param>
        /// <param name="authenticationLevel">
        /// Authentication level for RPC. 
        /// </param>
        /// <param name="timeout">Timeout for bind and all future requests.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when serverName is null.
        /// Thrown when servicePrincipalName is null.
        /// </exception>
        /// <returns>DrsrClientSessionContext after tcp bind</returns>
        [CLSCompliant(false)]
        public DrsrClientSessionContext BindOverTcp(
            string serverName,
            string servicePrincipalName,
            Guid? clientGuid,
            String protocolSequence,
            String endPoint,
            String networkOptions,
            ClientSecurityContext securityContext,
            RpceAuthenticationLevel authenticationLevel,
            TimeSpan timeout)
        {
            DrsrClientSessionContext clientSessionContext = new DrsrClientSessionContext();

            uint endpoint;
            endpoint = Convert.ToUInt16(this.InquiryAdldsDynamicPort(servicePrincipalName, clientGuid, protocolSequence, serverName, endPoint, networkOptions));

            if (serverName == null)
            {
                throw new ArgumentNullException("serverName");
            }

            if (servicePrincipalName == null)
            {
                throw new ArgumentNullException("servicePrincipalName");
            }

            this.rpcAdapter.Bind(
                RpceUtility.RPC_OVER_TCPIP_PROTOCOL_SEQUENCE,
                serverName,
                endpoint.ToString(),
                securityContext,
                authenticationLevel,
                timeout);


            clientSessionContext.RPCHandle = rpcAdapter.Handle;

            return clientSessionContext;
        }

        /// <summary>
        /// Use the native RPC library to bind to DRS service.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="domain"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="spn"></param>
        /// <returns></returns>
        public DrsrClientSessionContext Bind(
            string server, 
            string domain, 
            string userName, 
            string password, 
            string spn)
        {
            DrsrClientSessionContext clientSessionContext = new DrsrClientSessionContext();
            bool ret = this.rpcAdapter.Bind(server, domain, userName, password, spn);

            if (ret == true)
                clientSessionContext.RPCHandle = rpcAdapter.Handle;

            return clientSessionContext;
        }

        #endregion


        #region Long API of Creating Request Structures
        /// <summary>
        /// Create a DRSAddCloneDC request message
        /// </summary>
        /// <param name="cloneDCName">The new DC name.</param>
        /// <param name="siteRDN">The RDN of the site of the new DC will be placed into.</param>
        /// <returns>A DRS_MSG_ADDCLONEDCREQ structure</returns>
        [CLSCompliant(false)]
        public DRS_MSG_ADDCLONEDCREQ CreateAddCloneDCRequest(
                string cloneDCName,
                string siteRDN)
        {
            DRS_MSG_ADDCLONEDCREQ_V1 msg = new DRS_MSG_ADDCLONEDCREQ_V1();
            msg.pwszCloneDCName = cloneDCName;
            msg.pwszSite = siteRDN;

            DRS_MSG_ADDCLONEDCREQ request = new DRS_MSG_ADDCLONEDCREQ();
            request.V1 = msg;

            return request;
        }



        /// <summary>
        /// To create a request message of DRDM client capibility without extended fields.
        /// </summary>
        /// <param name="flags">contains individual bit flags that describe the capabilities of the DC that
        /// produced the DRS_EXTENSIONS_INT structure</param>
        /// <param name="siteObjGuid">A GUID. The objectGUID of the site object of which the DC's DSA object is a
        /// descendant. For non-DC client callers, this field SHOULD be set to zero.</param>
        /// <param name="pid">It specifies the process identifier of the client. This is for informational and
        /// debugging purposes only.</param>
        /// <param name="replEpoch">This value is set to zero by all client callers.</param>
        /// <returns> the created RPC input parameter.</returns>
        [CLSCompliant(false)]
        public DRS_EXTENSIONS CreateDrsExtensions(
            bool isDCClient,
            DRS_EXTENSIONS_IN_FLAGS flags,
            Guid siteObjGuid,
            int pid,
            uint replEpoch)
        {
            DRS_EXTENSIONS_INT drsExtensionInt = new DRS_EXTENSIONS_INT();
            drsExtensionInt.dwFlags = (uint)flags;
            drsExtensionInt.SiteObjGuid = siteObjGuid;
            drsExtensionInt.Pid = pid;
            drsExtensionInt.dwReplEpoch = replEpoch;
            if (!isDCClient && drsExtensionInt.dwReplEpoch != 0)
                throw new Exception("If the client is not a DC, the dwReplEpoch field of the DRS_EXTENSIONS_INT structure should be set to zero");
            drsExtensionInt.dwFlagsExt = 0;
            drsExtensionInt.ConfigObjGUID = new Guid();
            if (!isDCClient && drsExtensionInt.ConfigObjGUID != Guid.Empty)
                throw new Exception("If the client is not a DC, sets the ConfigObjGUID field of the DRS_EXTENSIONS_INT structure should be set to the NULL GUID value");

            drsExtensionInt.cb = (uint)(Marshal.SizeOf(drsExtensionInt.dwFlags)
                + Marshal.SizeOf(drsExtensionInt.SiteObjGuid)
                + Marshal.SizeOf(drsExtensionInt.Pid)
                + Marshal.SizeOf(drsExtensionInt.dwReplEpoch));

            DRS_EXTENSIONS resBindExt = new DRS_EXTENSIONS();
            resBindExt.cb = drsExtensionInt.cb;
            resBindExt.rgb = new byte[resBindExt.cb];
            Buffer.BlockCopy(TypeMarshal.ToBytes<DRS_EXTENSIONS_INT>(drsExtensionInt),
                Marshal.SizeOf(drsExtensionInt.cb), resBindExt.rgb, 0, (int)(resBindExt.cb));

            return resBindExt;
        }


        /// <summary>
        /// To create a request message of DRDM client capibility with extended fields.
        /// </summary>
        /// <param name="flags">It contains individual bit flags that describe the capabilities of the DC that
        /// produced the DRS_EXTENSIONS_INT structure</param>
        /// <param name="siteObjGuid">A GUID. The objectGUID of the site object of which the DC's DSA object is a
        /// descendant. For non-DC client callers, this field SHOULD be set to zero.</param>
        /// <param name="pid">It specifies the process identifier of the client. This is for informational and
        /// debugging purposes only.</param>
        /// <param name="replEpoch">This value is set to zero by all client callers.</param>
        /// <param name="flagsExt">An extension of the dwFlags field that contains individual bit flags that
        /// describe the capabilities of the DC that produced the DRS_EXTENSIONS_INT structure. For non-DC client
        /// callers, no bits SHOULD be set.</param>
        /// <param name="configObjGuid">A GUID. The objectGUID of the config NC object.</param>
        /// <returns> the created RPC input parameter.</returns>
        [CLSCompliant(false)]
        public DRS_EXTENSIONS CreateDrsExtensions(
            bool isDCClient,
            DRS_EXTENSIONS_IN_FLAGS flags,
            Guid siteObjGuid,
            int pid,
            uint replEpoch,
            DRS_EXTENSIONS_IN_FLAGSEXT flagsExt,
            Guid configObjGuid,
            uint extCaps)
        {
            DRS_EXTENSIONS_INT drsExtensionInt = new DRS_EXTENSIONS_INT();
            drsExtensionInt.dwFlags = (uint)flags;
            drsExtensionInt.SiteObjGuid = siteObjGuid;
            drsExtensionInt.Pid = pid;
            drsExtensionInt.dwReplEpoch = replEpoch;
            if (!isDCClient && drsExtensionInt.dwReplEpoch != 0)
                throw new Exception("If the client is not a DC, the dwReplEpoch field of the DRS_EXTENSIONS_INT structure should be set to zero");
            drsExtensionInt.dwFlagsExt = (uint)flagsExt;
            drsExtensionInt.ConfigObjGUID = configObjGuid;
            if (!isDCClient && drsExtensionInt.ConfigObjGUID != Guid.Empty)
                throw new Exception("If the client is not a DC, sets the ConfigObjGUID field of the DRS_EXTENSIONS_INT structure should be set to the NULL GUID value");
            drsExtensionInt.dwExtCaps = extCaps;

            drsExtensionInt.cb = (uint)(Marshal.SizeOf(drsExtensionInt.dwFlags)
                + Marshal.SizeOf(drsExtensionInt.SiteObjGuid)
                + Marshal.SizeOf(drsExtensionInt.Pid)
                + Marshal.SizeOf(drsExtensionInt.dwReplEpoch)
                + Marshal.SizeOf(drsExtensionInt.dwFlagsExt)
                + Marshal.SizeOf(drsExtensionInt.ConfigObjGUID)
                + Marshal.SizeOf(drsExtensionInt.dwExtCaps));

            DRS_EXTENSIONS resBindExt = new DRS_EXTENSIONS();
            resBindExt.cb = drsExtensionInt.cb;
            resBindExt.rgb = new byte[resBindExt.cb];
            Buffer.BlockCopy(TypeMarshal.ToBytes<DRS_EXTENSIONS_INT>(drsExtensionInt),
                Marshal.SizeOf(drsExtensionInt.cb), resBindExt.rgb, 0, (int)(resBindExt.cb));

            return resBindExt;
        }


        /// <summary>
        /// To create a request message of DRS_MSG_REPSYNC V1.
        /// </summary>
        /// <param name="pNC">The DSName of the specified NC.</param>
        /// <param name="sourceDsaGuid">The source DSA GUID.</param>
        /// <param name="sourceDsaName">The transport-specific NetworkAddress of the source DC.</param>
        /// <param name="options">The DRS_OPTIONS flags.</param>
        /// <returns> the created RPC input parameter.</returns>
        [CLSCompliant(false)]
        public DRS_MSG_REPSYNC_V1 CreateReplicaSyncRequestV1(
            DSNAME? pNC,
            Guid sourceDsaGuid,
            string sourceDsaName,
            DRS_OPTIONS options)
        {
            DRS_MSG_REPSYNC_V1 message = new DRS_MSG_REPSYNC_V1();
            message.pNC = pNC;
            message.uuidDsaSrc = sourceDsaGuid;
            message.pszDsaSrc = sourceDsaName;
            message.ulOptions = (uint)options;
            return message;
        }

        /// <summary>
        /// To create a request message of DRS_MSG_REPSYNC V2.
        /// </summary>
        /// <param name="v1">The request message of DRS_MSG_REPSYNC V1.</param>
        /// <returns> the created RPC input parameter.</returns>
        public DRS_MSG_REPSYNC_V2 CreateReplicaSyncRequestV2(
            DRS_MSG_REPSYNC_V1 v1
            )
        {
            DRS_MSG_REPSYNC_V2 message = new DRS_MSG_REPSYNC_V2();
            message.pNC = v1.pNC;
            message.uuidDsaSrc = v1.uuidDsaSrc;
            message.pszDsaSrc = v1.pszDsaSrc;
            message.ulOptions = v1.ulOptions;
            return message;
        }

        /// <summary>
        /// defines a portion of the request message that is sent to the IDL_DRSGetNCChanges method as part of SMTP replication ([MS-SRPL]). 
        /// </summary>
        /// <param name="uuidDsaObjDest">DSA GUID of the client DC</param>
        /// <param name="uuidInvocIdSrc">Invocation ID of the server DC.</param>
        /// <param name="ncReplicaDistinguishedName">the object's distinguishedName attribute of the root of an NC
        /// replica on the server.</param>
        /// <param name="ncReplicaObjectGuid">the object's objectGUID attribute of the root of an NC replica on the
        /// server.</param>
        /// <param name="ncReplicaObjectSid">the object's objectSid attribute of the root of an NC replica on the
        /// server.</param>
        /// <param name="usnHighObjUpdate">USN of usnvecFrom</param>
        /// <param name="usnReserved">USN of usnvecFrom</param>
        /// <param name="usnHighPropUpdate">USN of usnvecFrom</param>
        /// <param name="rgCursors">rgCursors of Stamp filter describing updates that the client has already applied</param>
        /// <param name="rgPartialAttr">A set of one or more attributes whose 
        /// values are to be replicated to the client's partial replica</param>
        /// <param name="pPrefixEntry"> to create Prefix table with 
        /// which to convert the ATTRTYP values in pPartialAttrVecDestV1 to OIDs</param>
        /// <param name="ulFlags">A DRS_OPTIONS bit field</param>
        /// <param name="cMaxObjects">An approximate cap on the number of objects to include in the reply</param>
        /// <param name="cMaxBytes">An approximate cap on the number of bytes to include in the reply</param>
        /// <param name="ulExtendedOp">0 or an EXOP_REQ code </param>
        /// <returns>DRS_MSG_GETCHGREQ_V3</returns>
        [CLSCompliant(false)]
        public DRS_MSG_GETCHGREQ_V3 CreateDRS_MSG_GETCHGREQ_V3(
             Guid uuidDsaObjDest,
             Guid uuidInvocIdSrc,
             string ncReplicaDistinguishedName,
             Guid ncReplicaObjectGuid,
             string ncReplicaObjectSid,
             long usnHighObjUpdate,
             long usnReserved,
             long usnHighPropUpdate,
             UPTODATE_CURSOR_V1[] rgCursors,
             uint[] rgPartialAttr,
             PrefixTableEntry[] pPrefixEntry,
             uint ulFlags,
             uint cMaxObjects,
             uint cMaxBytes,
             uint ulExtendedOp)
        {


            DRS_MSG_GETCHGREQ_V3 V3 = new DRS_MSG_GETCHGREQ_V3();
            V3.uuidDsaObjDest = uuidDsaObjDest;
            V3.uuidInvocIdSrc = uuidInvocIdSrc;
            V3.pNC = CreateDsName(ncReplicaDistinguishedName, ncReplicaObjectGuid, ncReplicaObjectSid);

            V3.usnvecFrom = CreateUSN_VECTOR(usnHighObjUpdate, usnHighPropUpdate, usnReserved);

            V3.pUpToDateVecDestV1[0] = CreateUPTODATE_VECTOR_V1_EXT(rgCursors);
            V3.pPartialAttrVecDestV1[0] = CreatePARTIAL_ATTR_VECTOR_V1_EXT(rgPartialAttr);

            SCHEMA_PREFIX_TABLE PrefixTableDest = new SCHEMA_PREFIX_TABLE();
            PrefixTableDest.PrefixCount = pPrefixEntry == null ? 0 : (uint)pPrefixEntry.Length;
            PrefixTableDest.pPrefixEntry = pPrefixEntry;
            V3.PrefixTableDest = PrefixTableDest;

            V3.ulFlags = ulFlags;
            V3.cMaxObjects = cMaxObjects;
            V3.cMaxBytes = cMaxBytes;
            V3.ulExtendedOp = ulExtendedOp;

            return V3;
        }

        /// <summary>
        /// create Windows 2000 SMTP request to get NC Changes
        /// </summary>
        /// <param name="uuidTransportObj">The objectGUID of the interSiteTransport object 
        /// that identifies the transport by which to send the reply</param>
        /// <param name="mtx_name">elements of The pmtxReturnAddress which transport-specific address to which to send the reply</param>
        /// <param name="V3">subset of V4 </param>
        /// <returns>DRS_MSG_GETCHGREQ as GetNCChangesRequest </returns>
        [CLSCompliant(false)]
        public DRS_MSG_GETCHGREQ CreateDRSGetNCChangesRequestV4(
             Guid uuidTransportObj,
             byte[] mtx_name,
             DRS_MSG_GETCHGREQ_V3 V3
        )
        {
            DRS_MSG_GETCHGREQ_V4 message = new DRS_MSG_GETCHGREQ_V4();
            message.uuidTransportObj = uuidTransportObj;
            message.pmtxReturnAddress[0] = CreateMTX_ADDR(mtx_name);
            message.V3 = V3;

            DRS_MSG_GETCHGREQ request = new DRS_MSG_GETCHGREQ();
            request.V4 = message;

            return request;
        }

        /// <summary>
        /// create PrefixTableEntry
        /// </summary>
        /// <param name="ndx">The index assigned to the prefix</param>
        /// <param name="length">length of the OID or length of a prefix of the OID</param>
        /// <param name="elements">elements of the OID or elements of the prefix of the OID </param>
        /// <returns>PrefixTableEntry. PrefixTableEntry structure defines a concrete type
        ///  for mapping a range of ATTRTYP values to and from OIDs.
        ///  It is a component of the type SCHEMA_PREFIX_TABLE</returns>
        [CLSCompliant(false)]
        public PrefixTableEntry CreatePrefixTableEntry(uint ndx, uint length, byte[] elements)
        {
            PrefixTableEntry Entryvalue = new PrefixTableEntry();
            Entryvalue.ndx = ndx;
            OID_t prefix = new OID_t();
            prefix.length = length;
            prefix.elements = elements;

            return Entryvalue;
        }


        /// <summary>
        /// create MTX_ADDR
        /// </summary>
        /// <param name="mtx_name">element of MTX_ADDR</param>
        /// <returns>a concrete type for the
        ///  network name of a DC</returns>
        [CLSCompliant(false)]
        public MTX_ADDR CreateMTX_ADDR(byte[] mtx_name)
        {
            MTX_ADDR pmtxReturnAddress = new MTX_ADDR();
            pmtxReturnAddress.mtx_namelen = mtx_name == null ? 0 : (uint)mtx_name.Length;
            pmtxReturnAddress.mtx_name = mtx_name;

            return pmtxReturnAddress;
        }

        /// <summary>
        /// create UPTODATE_VECTOR_V1_EXT
        /// </summary>
        /// <param name="rgCursors">An array of UPTODATE_CURSOR_V1</param>
        /// <returns>a concrete
        ///  type for the replication state relative to a set of
        ///  DC</returns>
        [CLSCompliant(false)]
        public UPTODATE_VECTOR_V1_EXT CreateUPTODATE_VECTOR_V1_EXT(UPTODATE_CURSOR_V1[] rgCursors)
        {
            UPTODATE_VECTOR_V1_EXT pUpToDateVecDestV1 = new UPTODATE_VECTOR_V1_EXT();
            pUpToDateVecDestV1.dwVersion = UPTODATE_VECTOR_V1_EXT_dwVersion_Values.V1;
            pUpToDateVecDestV1.dwReserved1 = UPTODATE_VECTOR_V1_EXT_dwReserved1_Values.V1;
            pUpToDateVecDestV1.dwReserved2 = dwReserved2_Values.V1;
            pUpToDateVecDestV1.cNumCursors = rgCursors == null ? 0 : (uint)rgCursors.Length;
            pUpToDateVecDestV1.rgCursors = rgCursors;

            return pUpToDateVecDestV1;
        }

        /// <summary>
        /// create UPTODATE_CURSOR_V1
        /// </summary>
        /// <param name="uuidDsa">The invocationId of the DC performing the update</param>
        /// <param name="usnHighPropUpdate">The USN of the update on the updating DC</param>
        /// <returns>The UPTODATE_CURSOR_V1 structure is a concrete type
        ///  for the replication state relative to a given DC</returns>
        [CLSCompliant(false)]
        public UPTODATE_CURSOR_V1 CreateUPTODATE_CURSOR_V1(Guid uuidDsa, ushort usnHighPropUpdate)
        {
            UPTODATE_CURSOR_V1 UPTODATE_CURSOR_V1Value = new UPTODATE_CURSOR_V1();
            UPTODATE_CURSOR_V1Value.usnHighPropUpdate = usnHighPropUpdate;
            UPTODATE_CURSOR_V1Value.uuidDsa = uuidDsa;

            return UPTODATE_CURSOR_V1Value;
        }

        /// <summary>
        /// Create PARTIAL_ATTR_VECTOR_V1_EXT
        /// </summary>
        /// <param name="rgPartialAttr">attribute of PARTIAL_ATTR_VECTOR_V1_EXT</param>
        /// <returns>concrete
        ///  type for a set of attributes to be replicated to a
        ///  given partial replica</returns>
        [CLSCompliant(false)]
        public static PARTIAL_ATTR_VECTOR_V1_EXT CreatePARTIAL_ATTR_VECTOR_V1_EXT(uint[] rgPartialAttr)
        {
            PARTIAL_ATTR_VECTOR_V1_EXT pPartialAttrVecDestV1 = new PARTIAL_ATTR_VECTOR_V1_EXT();
            pPartialAttrVecDestV1.dwVersion = dwVersion_Values.V1;
            pPartialAttrVecDestV1.dwReserved1 = dwReserved1_Values.V1;
            pPartialAttrVecDestV1.cAttrs = rgPartialAttr == null ? 0 : (uint)rgPartialAttr.Length;
            pPartialAttrVecDestV1.rgPartialAttr = rgPartialAttr;

            return pPartialAttrVecDestV1;
        }

        /// <summary>
        /// create USN_VECTOR
        /// </summary>
        /// <param name="usnHighObjUpdate">USN of usnvecFrom</param>
        /// <param name="usnReserved">USN of usnvecFrom</param>
        /// <param name="usnHighPropUpdate">USN of usnvecFrom</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public USN_VECTOR CreateUSN_VECTOR(long usnHighObjUpdate, long usnHighPropUpdate, long usnReserved)
        {
            USN_VECTOR usnvecFrom = new USN_VECTOR();

            usnvecFrom.usnHighObjUpdate = (ulong)usnHighObjUpdate;
            usnvecFrom.usnHighPropUpdate = (ulong)usnHighPropUpdate;
            usnvecFrom.usnReserved = (ulong)usnReserved;
            return usnvecFrom;
        }

        /// <summary>
        /// Create DRSGetNCChangesRequest on Windows 2000 RPC replication
        /// </summary>
        /// <param name="uuidDsaObjDest">DSA GUID of the client DC</param>
        /// <param name="uuidInvocIdSrc">Invocation ID of the server DC</param>
        /// <param name="pNc">The DSName of the specified NC or object.</param>
        /// <param name="usnFrom">Data used to correlate calls to IDL_DRSGetNCChanges.</param>
        /// <param name="utdVector">rgCursors of Stamp filter describing updates that the client has already applied</param>
        /// <param name="ulFlags">A DRS_OPTIONS bit field</param>
        /// <param name="cMaxObjects">An approximate cap on the number of objects to include in the reply</param>
        /// <param name="cMaxBytes">An approximate cap on the number of bytes to include in the reply</param>
        /// <param name="ulExtendedOp">0 or an EXOP_REQ code </param>
        /// <param name="QuadPart">QuadPart of liFsmoInfo</param>
        /// <returns>create request message for Get NC Changes</returns>
        [CLSCompliant(false)]
        public DRS_MSG_GETCHGREQ CreateDRSGetNCChangesRequestV5(
              Guid uuidDsaObjDest,
              Guid uuidInvocIdSrc,
              DSNAME? pNc,
              USN_VECTOR usnFrom,
              UPTODATE_VECTOR_V1_EXT utdVector,
              uint ulFlags,
              uint cMaxObjects,
              uint cMaxBytes,
              uint ulExtendedOp,
              ulong QuadPart
        )
        {

            DRS_MSG_GETCHGREQ_V5 message = new DRS_MSG_GETCHGREQ_V5();
            message.uuidDsaObjDest = uuidDsaObjDest;
            message.uuidInvocIdSrc = uuidInvocIdSrc;
            message.pNC = pNc;
            message.usnvecFrom = usnFrom;

            message.pUpToDateVecDestV1 = new UPTODATE_VECTOR_V1_EXT[1];
            message.pUpToDateVecDestV1[0] = utdVector;

            message.ulFlags = ulFlags;
            message.cMaxObjects = cMaxObjects;
            message.cMaxBytes = cMaxBytes;
            message.ulExtendedOp = ulExtendedOp;

            ULARGE_INTEGER liFsmoInfo = new ULARGE_INTEGER();
            liFsmoInfo.QuadPart = QuadPart;
            message.liFsmoInfo = liFsmoInfo;

            DRS_MSG_GETCHGREQ request = new DRS_MSG_GETCHGREQ();
            request.V5 = message;

            return request;
        }

        /// <summary>
        /// GetNCChangesRequest on Windows Server 2003 SMTP replication [MS-SRPL]
        /// </summary>
        /// <param name="V4">subset of V5</param>
        /// <param name="rgPartialAttr_pPartialAttrSet">attributes of pPartialAttrSet
        /// A set of one or more attributes whose values are to be replicated to the client's partial replica,
        /// or null if the client has a full replica</param>
        /// <param name="rgPartialAttr_pPartialAttrSetEx"> attributes of pPartialAttrSetEx
        /// A set of one or more attributes 
        /// whose values are to be added to the client's existing partial replica, or null</param>
        /// <param name="pPrefixEntry">An array of PrefixTableEntry items in  PrefixTableDest
        /// Prefix table with which to convert the ATTRTYP values in pPartialAttrSet 
        /// and pPartialAttrSetEx to OIDs</param>
        /// <returns>DRS_MSG_GETCHGREQ request for Windows Server 2003 SMTP replication</returns>
        [CLSCompliant(false)]
        public DRS_MSG_GETCHGREQ CreateDRSGetNCChangesRequestV7(
              DRS_MSG_GETCHGREQ_V4 V4,
              uint[] rgPartialAttr_pPartialAttrSet,
              uint[] rgPartialAttr_pPartialAttrSetEx,
              PrefixTableEntry[] pPrefixEntry
        )
        {

            DRS_MSG_GETCHGREQ_V7 message = new DRS_MSG_GETCHGREQ_V7();
            message.uuidTransportObj = V4.uuidTransportObj;
            message.pmtxReturnAddress = V4.pmtxReturnAddress;
            message.V3 = V4.V3;
            message.pPartialAttrSet = CreatePARTIAL_ATTR_VECTOR_V1_EXT(rgPartialAttr_pPartialAttrSet);
            message.pPartialAttrSetEx = CreatePARTIAL_ATTR_VECTOR_V1_EXT(rgPartialAttr_pPartialAttrSetEx);

            SCHEMA_PREFIX_TABLE PrefixTableDest = new SCHEMA_PREFIX_TABLE();
            PrefixTableDest.PrefixCount = pPrefixEntry == null ? 0 : (uint)pPrefixEntry.Length;
            PrefixTableDest.pPrefixEntry = pPrefixEntry;
            message.PrefixTableDest = PrefixTableDest;

            DRS_MSG_GETCHGREQ request = new DRS_MSG_GETCHGREQ();
            request.V7 = message;

            return request;
        }

        /// <summary>
        /// GetNCChangesRequest on Windows Server 2003 RPC replication
        /// </summary>
        /// <param name="V5">superset of the message</param>
        /// <param name="rgPartialAttr_pPartialAttrSet">attribtues of pPartialAttrSet
        /// A set of one or more attributes whose values are to be replicated to the 
        /// client's partial replica, or null if the client has a full replica</param>
        /// <param name="rgPartialAttr_pPartialAttrSetEx">attribtues of pPartialAttrSetEx
        /// A set of one or more attributes whose values are to be added to the
        /// client's existing partial replica, or null</param>
        /// <param name="prefixTable">Prefix table with which to convert 
        /// the ATTRTYP values in pPartialAttrSet and pPartialAttrSetEx to OIDs</param>
        /// <returns>DRS_MSG_GETCHGREQ ChangesRequest message on Windows Server 2003 RPC replication</returns>
        [CLSCompliant(false)]
        public DRS_MSG_GETCHGREQ CreateDRSGetNCChangesRequestV8(
            DRS_MSG_GETCHGREQ_V5 V5,
            uint[] rgPartialAttr_pPartialAttrSet,
            uint[] rgPartialAttr_pPartialAttrSetEx,
            SCHEMA_PREFIX_TABLE prefixTable
            )
        {
            DRS_MSG_GETCHGREQ_V8 message = new DRS_MSG_GETCHGREQ_V8();
            message.uuidDsaObjDest = V5.uuidDsaObjDest;
            message.uuidInvocIdSrc = V5.uuidInvocIdSrc;
            message.pNC = V5.pNC;
            message.usnvecFrom = V5.usnvecFrom;
            message.pUpToDateVecDest = V5.pUpToDateVecDestV1;
            message.ulFlags = V5.ulFlags;
            message.cMaxObjects = V5.cMaxObjects;
            message.cMaxBytes = V5.cMaxBytes;
            message.ulExtendedOp = V5.ulExtendedOp;
            message.liFsmoInfo = V5.liFsmoInfo;

            if (rgPartialAttr_pPartialAttrSet == null)
            {
                message.pPartialAttrSet = null;
            }
            else
            {
                message.pPartialAttrSet = CreatePARTIAL_ATTR_VECTOR_V1_EXT(rgPartialAttr_pPartialAttrSet);
            }
            if (rgPartialAttr_pPartialAttrSetEx == null)
            {
                message.pPartialAttrSetEx = null;
            }
            else
            {
                message.pPartialAttrSetEx = CreatePARTIAL_ATTR_VECTOR_V1_EXT(rgPartialAttr_pPartialAttrSetEx);
            }
            if (rgPartialAttr_pPartialAttrSet == null && rgPartialAttr_pPartialAttrSetEx == null)
            {
            }

            message.PrefixTableDest = prefixTable;

            DRS_MSG_GETCHGREQ request = new DRS_MSG_GETCHGREQ();
            request.V8 = message;

            return request;
        }

        /// <summary>
        /// GetNCChangesRequest on Windows Server 2008 R2 RPC replication
        /// </summary>
        /// <param name="V8">superset of the message</param>
        /// <param name="ulMoreFlags">A DRS_MORE_GETCHGREQ_OPTIONS bit field </param>
        /// <returns>DRS_MSG_GETCHGREQ ChangesRequest message on Windows Server 2008 R2 RPC replication</returns>
        [CLSCompliant(false)]
        public DRS_MSG_GETCHGREQ CreateDRSGetNCChangesRequestV10(
            DRS_MSG_GETCHGREQ_V8 V8,
            uint ulMoreFlags
        )
        {
            DRS_MSG_GETCHGREQ_V10 message = new DRS_MSG_GETCHGREQ_V10();
            message.uuidDsaObjDest = V8.uuidDsaObjDest;
            message.uuidInvocIdSrc = V8.uuidInvocIdSrc;
            message.pNC = V8.pNC;
            message.usnvecFrom = V8.usnvecFrom;
            message.pUpToDateVecDest = V8.pUpToDateVecDest;
            message.ulFlags = V8.ulFlags;
            message.cMaxObjects = V8.cMaxObjects;
            message.cMaxBytes = V8.cMaxBytes;
            message.ulExtendedOp = V8.ulExtendedOp;
            message.liFsmoInfo = V8.liFsmoInfo;

            message.pPartialAttrSet = V8.pPartialAttrSet;
            message.pPartialAttrSetEx = V8.pPartialAttrSetEx;
            message.PrefixTableDest = V8.PrefixTableDest;

            message.ulMoreFlags = ulMoreFlags;

            DRS_MSG_GETCHGREQ request = new DRS_MSG_GETCHGREQ();
            request.V10 = message;

            return request;
        }

        /// <summary>
        /// GetNCChangesRequest on Windows Server 2008 R2 RPC replication
        /// </summary>
        /// <param name="V10">superset of the message</param>
        /// <param name="ulMoreFlags">A DRS_MORE_GETCHGREQ_OPTIONS bit field </param>
        /// <returns>DRS_MSG_GETCHGREQ ChangesRequest message on Windows Server v1803 operating system RPC replication</returns>
        [CLSCompliant(false)]
        public DRS_MSG_GETCHGREQ CreateDRSGetNCChangesRequestV11(
            DRS_MSG_GETCHGREQ_V10 V10,
            uint ulMoreFlags
        )
        {
            DRS_MSG_GETCHGREQ_V11 message = new DRS_MSG_GETCHGREQ_V11();
            message.uuidDsaObjDest = V10.uuidDsaObjDest;
            message.uuidInvocIdSrc = V10.uuidInvocIdSrc;
            message.pNC = V10.pNC;
            message.usnvecFrom = V10.usnvecFrom;
            message.pUpToDateVecDest = V10.pUpToDateVecDest;
            message.ulFlags = V10.ulFlags;
            message.cMaxObjects = V10.cMaxObjects;
            message.cMaxBytes = V10.cMaxBytes;
            message.ulExtendedOp = V10.ulExtendedOp;
            message.liFsmoInfo = V10.liFsmoInfo;

            message.pPartialAttrSet = V10.pPartialAttrSet;
            message.pPartialAttrSetEx = V10.pPartialAttrSetEx;
            message.PrefixTableDest = V10.PrefixTableDest;

            message.ulMoreFlags = ulMoreFlags;

            DRS_MSG_GETCHGREQ request = new DRS_MSG_GETCHGREQ();
            request.V11 = message;

            return request;
        }

        /// <summary>
        /// To create a request message of DRS_MSG_UPDREFS V1.
        /// </summary>
        /// <param name="ncReplicaDistinguishedName">the object's distinguishedName attribute of the root of an NC
        /// replica on the server.</param>
        /// <param name="ncReplicaObjectGuid">the object's objectGUID attribute of the root of an NC replica on the
        /// server.</param>
        /// <param name="ncReplicaObjectSid">the object's objectSid attribute of the root of an NC replica on the
        /// server.</param>
        /// <param name="destDsaName">The transport-specific NetworkAddress of the dest DC.</param>
        /// <param name="destDsaGuid">The dest DSA GUID.</param>
        /// <param name="options">The DRS_OPTIONS flags.</param>
        /// <returns> the created RPC input parameter.
        /// Marshal.StringToHGlobalAnsi is used to allocate the unmanaged memory required for the pszDsaDest of the 
        /// returned struct. So the caller is responsible for free the memory by calling FreeHGlobal when the returned
        /// struct is no longer needed. </returns>
        [CLSCompliant(false)]
        public DRS_MSG_UPDREFS_V1 CreateUpdateRefsRequestV1(
            string ncReplicaDistinguishedName,
            Guid ncReplicaObjectGuid,
            string ncReplicaObjectSid,
            string destDsaName,
            Guid destDsaGuid,
            DRS_OPTIONS options)
        {
            DRS_MSG_UPDREFS_V1 message = new DRS_MSG_UPDREFS_V1();
            message.pNC = CreateDsName(ncReplicaDistinguishedName, ncReplicaObjectGuid, ncReplicaObjectSid);
            message.pszDsaDest = destDsaName;
            message.uuidDsaObjDest = destDsaGuid;
            message.ulOptions = (uint)options;

            return message;
        }

        /// <summary>
        /// To create a request message of DRS_MSG_UPDREFS V2.
        /// </summary>
        /// <returns> the created RPC input parameter.
        /// Marshal.StringToHGlobalAnsi is used to allocate the unmanaged memory required for the pszDsaDest of the 
        /// returned struct. So the caller is responsible for free the memory by calling FreeHGlobal when the returned
        /// struct is no longer needed. </returns>
        [CLSCompliant(false)]
        public DRS_MSG_UPDREFS_V2 CreateUpdateRefsRequestV2(
            DRS_MSG_UPDREFS_V1 v1)
        {
            DRS_MSG_UPDREFS_V2 message = new DRS_MSG_UPDREFS_V2();
            message.pNC = v1.pNC;
            message.pszDsaDest = v1.pszDsaDest;
            message.uuidDsaObjDest = v1.uuidDsaObjDest;
            message.ulOptions = (uint)v1.ulOptions;
            return message;
        }

        /// <summary>
        /// To create a V1 request message of DRS_MSG_REPADD.
        /// </summary>
        /// <param name="ncReplicaDistinguishedName">the object's distinguishedName attribute of the NC to
        /// replicate.</param>
        /// <param name="ncReplicaObjectGuid">the object's objectGUID attribute of the NC to replicate.</param>
        /// <param name="ncReplicaObjectSid">the object's objectSid attribute of the NC to replicate.</param>
        /// <param name="sourceDsaName">The transport-specific NetworkAddress of the DC from which to replicate
        /// updates.</param>
        /// <param name="schedule">The schedule used to perform periodic replication.</param>
        /// <param name="options">The DRS_OPTIONS flags.</param>
        /// <returns> the created RPC input parameter.
        /// Marshal.StringToHGlobalAnsi is used to allocate the unmanaged memory required for the pszDsaSrc of the 
        /// returned struct. So the caller is responsible for free the memory by calling FreeHGlobal when the returned
        /// struct is no longer needed. </returns>
        [CLSCompliant(false)]
        public DRS_MSG_REPADD CreateReplicaAddV1Request(
            string ncReplicaDistinguishedName,
            Guid ncReplicaObjectGuid,
            string ncReplicaObjectSid,
            string sourceDsaName,
            REPLTIMES schedule,
            DRS_OPTIONS options)
        {
            DRS_MSG_REPADD_V1 message = new DRS_MSG_REPADD_V1();
            message.pNC = CreateDsName(ncReplicaDistinguishedName, ncReplicaObjectGuid, ncReplicaObjectSid);
            message.pszDsaSrc = sourceDsaName;
            message.rtSchedule = schedule;
            message.ulOptions = (uint)options;

            DRS_MSG_REPADD request = new DRS_MSG_REPADD();
            request.V1 = message;

            return request;
        }


        /// <summary>
        /// To create a V2 request message of DRS_MSG_REPADD.
        /// </summary>
        /// <param name="ncReplicaDistinguishedName">the object's distinguishedName attribute of the NC to
        /// replicate.</param>
        /// <param name="ncReplicaObjectGuid">the object's objectGUID attribute of the NC to replicate.</param>
        /// <param name="ncReplicaObjectSid">the object's objectSid attribute of the NC to replicate.</param>
        /// <param name="sourceDsaDistinguishedName">the object's distinguishedName attribute of the DC from which
        /// to replicate changes.</param>
        /// <param name="sourceDsaObjectGuid">the object's objectGUID attribute of the DC from which to replicate
        /// changes.</param>
        /// <param name="sourceDsaObjectSid">the object's objectSid attribute of the DC from which to replicate
        /// changes.</param>
        /// <param name="transportDistinguishedName">the object's distinguishedName attribute of the
        /// interSiteTransport object that identifies the network transport to be used in the server-to-server
        /// replication implementation with the specified DC.</param>
        /// <param name="transportObjectGuid">the object's objectGUID attribute of the interSiteTransport object
        /// that identifies the network transport to be used in the server-to-server replication implementation
        /// with the specified DC.</param>
        /// <param name="transportObjectSid">the object's objectSid attribute of the interSiteTransport object that
        /// identifies the network transport to be used in the server-to-server replication implementation with the
        /// specified DC.</param>
        /// <param name="sourceDsaName">The transport-specific NetworkAddress of the DC from which to replicate
        /// updates.</param>
        /// <param name="schedule">The schedule used to perform periodic replication.</param>
        /// <param name="options">The DRS_OPTIONS flags.</param>
        /// <returns> the created RPC input parameter.
        /// Marshal.StringToHGlobalAnsi is used to allocate the unmanaged memory required for the pszSourceDsaAddress
        /// of the returned struct. So the caller is responsible for free the memory by calling FreeHGlobal when the
        /// returned struct is no longer needed. </returns>
        [CLSCompliant(false)]
        public DRS_MSG_REPADD CreateReplicaAddV2Request(
            string ncReplicaDistinguishedName,
            Guid ncReplicaObjectGuid,
            string ncReplicaObjectSid,
            string sourceDsaDistinguishedName,
            Guid sourceDsaObjectGuid,
            string sourceDsaObjectSid,
            string transportDistinguishedName,
            Guid transportObjectGuid,
            string transportObjectSid,
            string sourceDsaName,
            REPLTIMES schedule,
            DRS_OPTIONS options)
        {
            DRS_MSG_REPADD_V2 message = new DRS_MSG_REPADD_V2();
            message.pNC = CreateDsName(ncReplicaDistinguishedName, ncReplicaObjectGuid, ncReplicaObjectSid);
            message.pSourceDsaDN = CreateDsName(
                sourceDsaDistinguishedName, sourceDsaObjectGuid, sourceDsaObjectSid);
            message.pTransportDN = CreateDsName(
                transportDistinguishedName, transportObjectGuid, transportObjectSid);
            message.pszSourceDsaAddress = sourceDsaName;
            message.rtSchedule = schedule;
            message.ulOptions = (uint)options;

            DRS_MSG_REPADD request = new DRS_MSG_REPADD();
            request.V2 = message;

            return request;
        }


        [CLSCompliant(false)]
        public DRS_MSG_REPADD CreateReplicaAddV3Request(
            string ncReplicaDistinguishedName,
            Guid ncReplicaObjectGuid,
            string ncReplicaObjectSid,
            string sourceDsaDistinguishedName,
            Guid sourceDsaObjectGuid,
            string sourceDsaObjectSid,
            string transportDistinguishedName,
            Guid transportObjectGuid,
            string transportObjectSid,
            string sourceDsaName,
            REPLTIMES schedule,
            DRS_OPTIONS options)
        {
            DRS_MSG_REPADD_V3 message = new DRS_MSG_REPADD_V3();
            message.pNC = CreateDsName(ncReplicaDistinguishedName, ncReplicaObjectGuid, ncReplicaObjectSid);
            message.pSourceDsaDN = CreateDsName(
                sourceDsaDistinguishedName, sourceDsaObjectGuid, sourceDsaObjectSid);
            message.pTransportDN = CreateDsName(
                transportDistinguishedName, transportObjectGuid, transportObjectSid);
            message.pszSourceDsaAddress = sourceDsaName;
            message.rtSchedule = schedule;
            message.ulOptions = (uint)options;

            DRS_MSG_REPADD request = new DRS_MSG_REPADD();
            request.V3 = message;

            return request;
        }
        /// <summary>
        /// To create a request message of DRS_MSG_REPDEL.
        /// </summary>
        /// <param name="ncReplicaDistinguishedName">the object's distinguishedName attribute of the root of an NC
        /// replica on the server.</param>
        /// <param name="ncReplicaObjectGuid">the object's objectGUID attribute of the root of an NC replica on the
        /// server.</param>
        /// <param name="ncReplicaObjectSid">the object's objectSid attribute of the root of an NC replica on the
        /// server.</param>
        /// <param name="sourceDsaName">The transport-specific NetworkAddress of the source DC.</param>
        /// <param name="options">The DRS_OPTIONS flags.</param>
        /// <returns> the created RPC input parameter.
        /// Marshal.StringToHGlobalAnsi is used to allocate the unmanaged memory required for the pszDsaSrc of the 
        /// returned struct. So the caller is responsible for free the memory by calling FreeHGlobal when the returned
        /// struct is no longer needed. </returns>
        [CLSCompliant(false)]
        public DRS_MSG_REPDEL CreateReplicaDelRequest(
            string ncReplicaDistinguishedName,
            Guid ncReplicaObjectGuid,
            string ncReplicaObjectSid,
            string sourceDsaName,
            DRS_OPTIONS options)
        {
            DRS_MSG_REPDEL_V1 message = new DRS_MSG_REPDEL_V1();
            message.pNC = CreateDsName(ncReplicaDistinguishedName, ncReplicaObjectGuid, ncReplicaObjectSid);
            message.pszDsaSrc = sourceDsaName;
            message.ulOptions = (uint)options;

            DRS_MSG_REPDEL request = new DRS_MSG_REPDEL();
            request.V1 = message;

            return request;
        }


        /// <summary>
        /// To create a request message of DRS_MSG_REPMOD.
        /// </summary>
        /// <param name="ncReplicaDistinguishedName">the object's distinguishedName attribute of the root of an NC
        /// replica on the server.</param>
        /// <param name="ncReplicaObjectGuid">the object's objectGUID attribute of the root of an NC replica on the
        /// server.</param>
        /// <param name="ncReplicaObjectSid">the object's objectSid attribute of the root of an NC replica on the
        /// server.</param>
        /// <param name="sourceDsaGuid">The source DSA GUID.</param>
        /// <param name="sourceDsaName">The transport-specific NetworkAddress of the source DC.</param>
        /// <param name="schedule">The schedule used to perform periodic replication.</param>
        /// <param name="replicaFlags">The DRS_OPTIONS flags for the repsFrom value.</param>
        /// <param name="modifyFields">The fields to update.</param>
        /// <param name="options">The DRS_OPTIONS flags.</param>
        /// <returns> the created RPC input parameter.
        /// Marshal.StringToHGlobalAnsi is used to allocate the unmanaged memory required for the pszSourceDRA of the 
        /// returned struct. So the caller is responsible for free the memory by calling FreeHGlobal when the returned
        /// struct is no longer needed. </returns>
        [CLSCompliant(false)]
        public DRS_MSG_REPMOD CreateReplicaModifyRequest(
            string ncReplicaDistinguishedName,
            Guid ncReplicaObjectGuid,
            string ncReplicaObjectSid,
            Guid sourceDsaGuid,
            string sourceDsaName,
            REPLTIMES schedule,
            DRS_OPTIONS replicaFlags,
            DRS_MSG_REPMOD_FIELDS modifyFields,
            DRS_OPTIONS options)
        {
            DRS_MSG_REPMOD_V1 message = new DRS_MSG_REPMOD_V1();
            message.pNC = CreateDsName(ncReplicaDistinguishedName, ncReplicaObjectGuid, ncReplicaObjectSid);
            message.uuidSourceDRA = sourceDsaGuid;
            message.pszSourceDRA = sourceDsaName;
            message.rtSchedule = schedule;
            message.ulReplicaFlags = (uint)replicaFlags;
            message.ulModifyFields = (uint)modifyFields;
            message.ulOptions = (uint)options;

            DRS_MSG_REPMOD request = new DRS_MSG_REPMOD();
            request.V1 = message;

            return request;
        }


        /// <summary>
        /// To create a request message of DRS_MSG_CRACKREQ.
        /// </summary>
        /// <param name="flags">Zero or more of the following bit flags.</param>
        /// <param name="formatOffered">The format of the names in rpNames.</param>
        /// <param name="formatDesired">Format of the names in the rItems field of the DS_NAME_RESULTW structure,
        /// which is returned inside the DRS_MSG_CRACKREPLY message.</param>
        /// <param name="translateNames">Input names to translate.</param>
        /// <returns> the created RPC input parameter.</returns>
        [CLSCompliant(false)]
        public DRS_MSG_CRACKREQ CreateCrackNamesRequest(
            DRS_MSG_CRACKREQ_FLAGS flags,
            uint formatOffered,
            uint formatDesired,
            string[] translateNames)
        {
            DRS_MSG_CRACKREQ_V1 message = new DRS_MSG_CRACKREQ_V1();
            message.CodePage = 0; // This field SHOULD be ignored by the server.
            message.LocaleId = 0; // This field SHOULD be ignored by the server.
            message.dwFlags = (uint)flags;
            message.formatOffered = formatOffered;
            message.formatDesired = formatDesired;
            message.cNames = (translateNames == null) ? 0 : (uint)translateNames.Length;
            message.rpNames = translateNames;

            DRS_MSG_CRACKREQ request = new DRS_MSG_CRACKREQ();
            request.V1 = message;

            return request;
        }


        /// <summary>
        /// To create a request message of DRS_MSG_SPNREQ.
        /// </summary>
        /// <param name="operation">The SPN operation to perform. MUST be one of the DS_SPN_OPERATION values.</param>
        /// <param name="account">The DN of the object to modify.</param>
        /// <param name="servicePricipalNames">The SPN values.</param>
        /// <returns> the created RPC input parameter.</returns>
        [CLSCompliant(false)]
        public DRS_MSG_SPNREQ CreateWriteSpnRequest(
            DS_SPN_OPREATION operation,
            string account,
            string[] servicePricipalNames)
        {
            DRS_MSG_SPNREQ_V1 message = new DRS_MSG_SPNREQ_V1();
            message.operation = (uint)operation;
            message.flags = flags_Values.V1;
            message.pwszAccount = account;
            message.cSPN = servicePricipalNames == null ? 0 : (uint)servicePricipalNames.Length;
            message.rpwszSPN = servicePricipalNames;

            DRS_MSG_SPNREQ request = new DRS_MSG_SPNREQ();
            request.V1 = message;

            return request;
        }


        /// <summary>
        /// To create a request message of DRS_MSG_RMSVRREQ.
        /// </summary>
        /// <param name="serverDistinguishedName">The DN of the server object of the DC to remove.</param>
        /// <param name="domainDistinguishedName">The DN of the NC root of the domain that the DC to be removed
        /// belongs to. May be set to null if the client does not want the server to compute the value of
        /// outMessage^.V1.fLastDCInDomain.</param>
        /// <param name="commit">True if the DC's metadata should actually be removed from the directory. False if
        /// the metadata should not be removed.</param>
        /// <returns> the created RPC input parameter.</returns>
        [CLSCompliant(false)]
        public DRS_MSG_RMSVRREQ CreateRemoveDsServerRequest(
            string serverDistinguishedName,
            string domainDistinguishedName,
            bool commit)
        {
            DRS_MSG_RMSVRREQ_V1 message = new DRS_MSG_RMSVRREQ_V1();
            message.ServerDN = serverDistinguishedName;
            message.DomainDN = domainDistinguishedName;
            message.fCommit = commit ? 1 : 0;

            DRS_MSG_RMSVRREQ request = new DRS_MSG_RMSVRREQ();
            request.V1 = message;

            return request;
        }


        /// <summary>
        /// To create a request message of DRS_MSG_RMDMNREQ.
        /// </summary>
        /// <param name="domainDistinguishedName">The DN of the NC root of the domain NC to remove.</param>
        /// <returns> the created RPC input parameter.</returns>
        [CLSCompliant(false)]
        public DRS_MSG_RMDMNREQ CreateRemoveDsDomainRequest(
            string domainDistinguishedName)
        {
            DRS_MSG_RMDMNREQ_V1 message = new DRS_MSG_RMDMNREQ_V1();
            message.DomainDN = domainDistinguishedName;

            DRS_MSG_RMDMNREQ request = new DRS_MSG_RMDMNREQ();
            request.V1 = message;

            return request;
        }


        /// <summary>
        /// To create a request message of DRS_MSG_DCINFOREQ.
        /// </summary>
        /// <param name="domainDistinguishedName">DNS domain name for which the client requests DC information.</param>
        /// <param name="infoLevel">The response version requested by the client: 1, 2, 3, or 0xFFFFFFFF.</param>
        /// <returns> the created RPC input parameter.</returns>
        [CLSCompliant(false)]
        public DRS_MSG_DCINFOREQ CreateDomainControllerInfoRequest(
            string domainDistinguishedName,
            DRS_MSG_DCINFOREQ_INFOLEVEL infoLevel)
        {
            DRS_MSG_DCINFOREQ_V1 message = new DRS_MSG_DCINFOREQ_V1();
            message.Domain = domainDistinguishedName;
            message.InfoLevel = (uint)infoLevel;

            DRS_MSG_DCINFOREQ request = new DRS_MSG_DCINFOREQ();
            request.V1 = message;

            return request;
        }


        /// <summary>
        /// To create a request message of DRS_MSG_KCC_EXECUTE.
        /// </summary>
        /// <param name="flags">Zero or more of the following bit flags.</param>
        /// <returns> the created RPC input parameter.</returns>
        [CLSCompliant(false)]
        public DRS_MSG_KCC_EXECUTE CreateExecuteKccRequest(
            DRS_MSG_KCC_EXECUTE_FLAGS flags)
        {
            DRS_MSG_KCC_EXECUTE_V1 message = new DRS_MSG_KCC_EXECUTE_V1();
            message.dwTaskID = (uint)dwTaskID_Values.V1;
            message.dwFlags = (uint)flags;

            DRS_MSG_KCC_EXECUTE request = new DRS_MSG_KCC_EXECUTE();
            request.V1 = message;

            return request;
        }


        /// <summary>
        /// To create a V1 request message of DRS_MSG_GETREPLINFO_REQ.
        /// </summary>
        /// <param name="infoType">MUST be a DS_REPL_INFO code.</param>
        /// <param name="objectDistinguishedName">DN of the object on which the operation should be performed. The
        /// meaning of this parameter depends on the value of the InfoType parameter.</param>
        /// <param name="sourceDsaObjGuid">NULL GUID or the DSA GUID of a DC.</param>
        /// <returns> the created RPC input parameter.</returns>
        [CLSCompliant(false)]
        public DRS_MSG_GETREPLINFO_REQ CreateGetReplInfoV1Request(
            DS_REPL_INFO infoType,
            string objectDistinguishedName,
            Guid sourceDsaObjGuid)
        {
            DRS_MSG_GETREPLINFO_REQ_V1 message = new DRS_MSG_GETREPLINFO_REQ_V1();
            message.InfoType = (uint)infoType;
            message.pszObjectDN = objectDistinguishedName;
            message.uuidSourceDsaObjGuid = sourceDsaObjGuid;

            DRS_MSG_GETREPLINFO_REQ request = new DRS_MSG_GETREPLINFO_REQ();
            request.V1 = message;

            return request;
        }


        /// <summary>
        /// To create a V2 request message of DRS_MSG_GETREPLINFO_REQ.
        /// </summary>
        /// <param name="infoType">MUST be a DS_REPL_INFO code.</param>
        /// <param name="objectDistinguishedName">DN of the object on which the operation should be performed. The
        /// meaning of this parameter depends on the value of the InfoType parameter.</param>
        /// <param name="sourceDsaObjGuid">NULL GUID or the DSA GUID of a DC.</param>
        /// <param name="flags">Zero or more of the following bit flags.</param>
        /// <param name="attributeName">Null, or the lDAPDisplayName of a link attribute.</param>
        /// <param name="value">Null, or the DN of the link value for which to retrieve a stamp.</param>
        /// <param name="enumerationContext">The range bound of values to be returned by the server.</param>
        /// <returns> the created RPC input parameter.</returns>
        [CLSCompliant(false)]
        public DRS_MSG_GETREPLINFO_REQ CreateGetReplInfoV2Request(
            DS_REPL_INFO infoType,
            string objectDistinguishedName,
            Guid sourceDsaObjGuid,
            DRS_MSG_GETREPLINFO_REQ_FLAGS flags,
            string attributeName,
            string value,
            uint enumerationContext)
        {
            DRS_MSG_GETREPLINFO_REQ_V2 message = new DRS_MSG_GETREPLINFO_REQ_V2();
            message.InfoType = (uint)infoType;
            message.pszObjectDN = objectDistinguishedName;
            message.uuidSourceDsaObjGuid = sourceDsaObjGuid;
            message.ulFlags = (uint)flags;
            message.pszAttributeName = attributeName;
            message.pszValueDN = value;
            message.dwEnumerationContext = enumerationContext;

            DRS_MSG_GETREPLINFO_REQ request = new DRS_MSG_GETREPLINFO_REQ();
            request.V2 = message;

            return request;
        }


        /// <summary>
        /// To create a request message of DRS_MSG_ADDSIDREQ.
        /// </summary>
        /// <param name="flags">A set of zero or more DRS_ADDSID_FLAGS bit flags.</param>
        /// <param name="sourceDomain">Name of the domain to query for the SID of SrcPrincipal. The domain name can
        /// be an FQDN or a NetBIOS name.</param>
        /// <param name="sourcePrincipal">Name of a security principal (user, computer, or group) in the source
        /// domain. This is the source principal, whose SIDs will be added to the destination principal. If Flags
        /// does not contain DS_ADDSID_FLAG_PRIVATE_DEL_SRC_OBJ, this name is a domain-relative Security Accounts
        /// Manager (SAM) name. Otherwise, it is a DN.</param>
        /// <param name="sourceDomainController">Name of the primary domain controller (PDC) (or PDC role owner) in
        /// the source domain. The DC name can be an Internet host name or a NetBIOS name. If null, the
        /// implementation of DRSAddSidHistory will locate such a DC.</param>
        /// <param name="sourceCredentialUser">User name for the credentials to be used in the source domain.</param>
        /// <param name="sourceCredentialDomain">Domain name for the credentials to be used in the source
        /// domain.</param>
        /// <param name="sourceCredentialPassword">Password for the credentials to be used in the source
        /// domain.</param>
        /// <param name="destDomain">Name of the destination domain in which DstPrincipal resides. The domain name
        /// can be an FQDN or a NetBIOS name.</param>
        /// <param name="destPrincipal">Name of a security principal (user, computer, or group) in the destination
        /// domain. This is the destination principal, to which the source principal's SIDs will be added. If Flags
        /// does not contain DS_ADDSID_FLAG_PRIVATE_DEL_SRC_OBJ, this name is a domain-relative SAM name.
        /// Otherwise, it is a DN.</param>
        /// <returns> the created RPC input parameter.</returns>
        [CLSCompliant(false)]
        public DRS_MSG_ADDSIDREQ CreateAddSidHistoryRequest(
            DRS_ADDSID_FLAGS flags,
            string sourceDomain,
            string sourcePrincipal,
            string sourceDomainController,
            string sourceCredentialUser,
            string sourceCredentialDomain,
            string sourceCredentialPassword,
            string destDomain,
            string destPrincipal)
        {
            DRS_MSG_ADDSIDREQ_V1 message = new DRS_MSG_ADDSIDREQ_V1();
            message.Flags = (uint)flags;
            message.SrcDomain = sourceDomain;
            message.SrcPrincipal = sourcePrincipal;
            message.SrcDomainController = sourceDomainController;
            message.SrcCredsUser = ConvertUnicodeStringToUshortArray(sourceCredentialUser);
            message.SrcCredsUserLength = (uint)message.SrcCredsUser.Length;
            message.SrcCredsDomain = ConvertUnicodeStringToUshortArray(sourceCredentialDomain);
            message.SrcCredsDomainLength = (uint)message.SrcCredsDomain.Length;
            message.SrcCredsPassword = ConvertUnicodeStringToUshortArray(sourceCredentialPassword);
            message.SrcCredsPasswordLength = (uint)message.SrcCredsPassword.Length;
            message.DstDomain = destDomain;
            message.DstPrincipal = destPrincipal;

            DRS_MSG_ADDSIDREQ request = new DRS_MSG_ADDSIDREQ();
            request.V1 = message;

            return request;
        }


        /// <summary>
        /// To create a request message of DRS_MSG_REPVERIFYOBJ.
        /// </summary>
        /// <param name="pNc">The DSName of the specified NC.</param>
        /// <param name="sourceDsaUuid">The objectGUID of the nTDSDSA object for the reference DC.</param>
        /// <param name="options">0 to expunge each object that is not verified, or 1 to log an event that
        /// identifies each such object.</param>
        /// <returns> the created RPC input parameter.</returns>
        [CLSCompliant(false)]
        public DRS_MSG_REPVERIFYOBJ CreateReplicaVerifyObjectsRequest(
            DSNAME? pNc,
            Guid sourceDsaUuid,
            DRS_MSG_REPVERIFYOBJ_OPTIONS options)
        {
            DRS_MSG_REPVERIFYOBJ_V1 message = new DRS_MSG_REPVERIFYOBJ_V1();
            message.pNC = pNc;
            message.uuidDsaSrc = sourceDsaUuid;
            message.ulOptions = (uint)options;

            DRS_MSG_REPVERIFYOBJ request = new DRS_MSG_REPVERIFYOBJ();
            request.V1 = message;

            return request;
        }


        /// <summary>
        /// To create a request message of DRS_MSG_QUERYSITESREQ.
        /// </summary>
        /// <param name="fromSite">The RDN of the site object of the "from" site.</param>
        /// <param name="toSites">The RDNs of the site objects of the "to" sites.</param>
        /// <returns> the created RPC input parameter.</returns>
        [CLSCompliant(false)]
        public DRS_MSG_QUERYSITESREQ CreateQuerySitesByCostRequest(
            string fromSite,
            string[] toSites)
        {
            DRS_MSG_QUERYSITESREQ_V1 message = new DRS_MSG_QUERYSITESREQ_V1();
            message.pwszFromSite = fromSite;
            message.cToSites = toSites == null ? 0 : (uint)toSites.Length;
            message.rgszToSites = toSites;
            message.dwFlags = dwFlags_Values.V1;

            DRS_MSG_QUERYSITESREQ request = new DRS_MSG_QUERYSITESREQ();
            request.V1 = message;

            return request;
        }


        /// <summary>
        /// To create a request message of DRS_MSG_INIT_DEMOTIONREQ.
        /// </summary>
        /// <returns> the created RPC input parameter.</returns>
        [CLSCompliant(false)]
        public DRS_MSG_INIT_DEMOTIONREQ CreateInitDemotionRequest()
        {
            DRS_MSG_INIT_DEMOTIONREQ_V1 message = new DRS_MSG_INIT_DEMOTIONREQ_V1();
            message.dwReserved = DRS_MSG_INIT_DEMOTIONREQ_V1_dwReserved_Values.V1;

            DRS_MSG_INIT_DEMOTIONREQ request = new DRS_MSG_INIT_DEMOTIONREQ();
            request.V1 = message;

            return request;
        }


        /// <summary>
        /// To create a request message of DRS_MSG_REPLICA_DEMOTIONREQ.
        /// </summary>
        /// <param name="flags">Zero or more of the following bit flags.</param>
        /// <param name="ncReplicaDistinguishedName">the object's distinguishedName attribute of the NC to
        /// replicate off.</param>
        /// <param name="ncReplicaObjectGuid">the object's objectGUID attribute of the NC to replicate off.</param>
        /// <param name="ncReplicaObjectSid">the object's objectSid attribute of the NC to replicate off.</param>
        /// <returns> the created RPC input parameter.</returns>
        [CLSCompliant(false)]
        public DRS_MSG_REPLICA_DEMOTIONREQ CreateReplicaDemotionRequest(
            DRS_MSG_REPLICA_DEMOTIONREQ_FLAGS flags,
            string ncReplicaDistinguishedName,
            Guid ncReplicaObjectGuid,
            string ncReplicaObjectSid)
        {
            DRS_MSG_REPLICA_DEMOTIONREQ_V1 message = new DRS_MSG_REPLICA_DEMOTIONREQ_V1();
            message.dwFlags = (uint)flags;
            message.uuidHelperDest = Guid.Empty;
            message.pNC = CreateDsName(ncReplicaDistinguishedName, ncReplicaObjectGuid, ncReplicaObjectSid);

            DRS_MSG_REPLICA_DEMOTIONREQ request = new DRS_MSG_REPLICA_DEMOTIONREQ();
            request.V1 = message;

            return request;
        }


        /// <summary>
        /// To create a request message of DRS_MSG_FINISH_DEMOTIONREQ.
        /// </summary>
        /// <param name="operations">Zero or more of the following bit flags.</param>
        /// <param name="scriptBase">The path name of the folder in which to store SPN unregistration scripts.
        /// Required when DS_DEMOTE_UNREGISTER_SPNS is specified in dwOperations.</param>
        /// <returns> the created RPC input parameter.</returns>
        [CLSCompliant(false)]
        public DRS_MSG_FINISH_DEMOTIONREQ CreateFinishDemotionRequest(
            DRS_MSG_FINISH_DEMOTIONREQ_OPERATIONS operations,
            string scriptBase)
        {
            DRS_MSG_FINISH_DEMOTIONREQ_V1 message = new DRS_MSG_FINISH_DEMOTIONREQ_V1();
            message.dwOperations = (uint)operations;
            message.uuidHelperDest = Guid.Empty;
            message.szScriptBase = scriptBase;

            DRS_MSG_FINISH_DEMOTIONREQ request = new DRS_MSG_FINISH_DEMOTIONREQ();
            request.V1 = message;

            return request;
        }


        /// <summary>
        /// To create a request message of DRS_MSG_VERIFYREQ.
        /// </summary>
        /// <param name="flags">The type of name to be verified;</param>
        /// <param name="names">An array of DSNames that need to be verified.</param>
        /// <param name="requiredAttrs">
        /// The list of attributes to be retrieved for each name that is verified.
        /// </param>
        /// <param name="prefixTable">
        /// The prefix table used to translate ATTRTYP values in RequiredAttrs to OID values.
        /// </param>
        /// <returns>the created RPC input parameter.</returns>
        [CLSCompliant(false)]
        public DRS_MSG_VERIFYREQ CreateVerifyNamesRequest(
            DRS_MSG_VERIFYREQ_V1_dwFlags_Values flags,
            DSNAME[] names,
            ATTRBLOCK requiredAttrs,
            SCHEMA_PREFIX_TABLE prefixTable)
        {
            DRS_MSG_VERIFYREQ_V1 message = new DRS_MSG_VERIFYREQ_V1();
            message.dwFlags = (uint)flags;
            message.cNames = names == null ? 0 : (uint)names.Length;
            message.rpNames = new DSNAME[1][];
            message.rpNames[0] = names;
            message.RequiredAttrs = requiredAttrs;
            message.PrefixTable = prefixTable;

            DRS_MSG_VERIFYREQ request = new DRS_MSG_VERIFYREQ();
            request.V1 = message;

            return request;
        }


        /// <summary>
        ///  To create a request message of DRS_MSG_REVMEMB_REQ.
        /// </summary>
        /// <param name="dsNames">
        ///  The DSName of the object whose reverse membership is
        ///  being requested, plus the DSNames of groups of the
        ///  appropriate type(s) of which it is already known to
        ///  be a member.
        /// </param>
        /// <param name="flags">Zero or more of the following bit flags.</param>
        /// <param name="operationType">
        ///  The type of group membership evaluation to be performed.
        /// </param>
        /// <param name="limitingDomain">
        ///  Domain filter; resulting objects that are not from this
        ///  domain are neither returned nor followed transitively.
        /// </param>
        /// <returns>the created RPC input parameter.</returns>
        [CLSCompliant(false)]
        public DRS_MSG_REVMEMB_REQ CreateGetMembershipsRequest(
            DSNAME[] dsNames,
            uint flags,
            REVERSE_MEMBERSHIP_OPERATION_TYPE operationType,
            DSNAME? limitingDomain)
        {
            DRS_MSG_REVMEMB_REQ_V1 message = new DRS_MSG_REVMEMB_REQ_V1();
            message.cDsNames = dsNames == null ? 0 : (uint)dsNames.Length;
            message.ppDsNames = new DSNAME[1][];
            message.ppDsNames[0] = dsNames;
            message.dwFlags = flags;
            message.OperationType = operationType;
            message.pLimitingDomain = limitingDomain;

            DRS_MSG_REVMEMB_REQ request = new DRS_MSG_REVMEMB_REQ();
            request.V1 = message;

            return request;
        }


        /// <summary>
        /// To create a request message of DRS_MSG_NT4_CHGLOG_REQ.
        /// </summary>
        /// <param name="flags">
        ///  Zero or more of the following bit flags : X,CL and SN.
        /// </param>
        /// <param name="preferredMaximumLength">
        ///  The maximum size, in bytes, of the change log data that
        ///  should be retrieved in a single operation.
        /// </param>
        /// <param name="restart">
        ///  Null to request the change log from the beginning. Otherwise,
        ///  a pointer to an opaque value, returned in some previous
        ///  call to IDL_DRSGetNT4ChangeLog, identifying the last
        ///  change log entry returned in that previous call.
        /// </param>
        /// <returns>the created RPC input parameter.</returns>
        [CLSCompliant(false)]
        public DRS_MSG_NT4_CHGLOG_REQ CreateGetNT4ChangeLogRequest(
            uint flags,
            uint preferredMaximumLength,
            byte[] restart)
        {
            DRS_MSG_NT4_CHGLOG_REQ_V1 message = new DRS_MSG_NT4_CHGLOG_REQ_V1();
            message.dwFlags = flags;
            message.PreferredMaximumLength = preferredMaximumLength;
            message.cbRestart = restart == null ? 0 : (uint)restart.Length;
            message.pRestart = restart;

            DRS_MSG_NT4_CHGLOG_REQ request = new DRS_MSG_NT4_CHGLOG_REQ();
            request.V1 = message;

            return request;
        }


        /// <summary>
        ///  To create a request message of DRS_MSG_GETMEMBERSHIPS2_REQ.
        /// </summary>
        /// <param name="requests">
        ///  Sequence of reverse membership requests.
        /// </param>
        /// <returns>the created RPC input parameter.</returns>
        [CLSCompliant(false)]
        public DRS_MSG_GETMEMBERSHIPS2_REQ CreateGetMemberships2Request(
            DRS_MSG_REVMEMB_REQ_V1[] requests)
        {
            DRS_MSG_GETMEMBERSHIPS2_REQ_V1 message = new DRS_MSG_GETMEMBERSHIPS2_REQ_V1();
            message.Count = requests == null ? 0 : (uint)requests.Length;
            message.Requests = requests;

            DRS_MSG_GETMEMBERSHIPS2_REQ request = new DRS_MSG_GETMEMBERSHIPS2_REQ();
            request.V1 = message;

            return request;
        }

        /// <summary>
        /// Version1 request message sent to the IDL_DRSInterDomainMove method.
        /// </summary>
        /// <param name="pSourceDSA">The NetworkAddress of the client DC</param>
        /// <param name="pObject">The object to be moved.</param>
        /// <param name="pParentUUID">The objectGUID of the new parent object</param>
        /// <param name="prefixTable">The prefix table with which to translate the ATTRTYP values in pObject to OIDs</param>
        /// <returns>DRS_MSG_MOVEREQ</returns>
        [CLSCompliant(false)]
        public DRS_MSG_MOVEREQ CreateDrsInterDomainMoveRequestV1(
            string pSourceDSA,
            ENTINF pObject,
            Guid pParentUUID,
            SCHEMA_PREFIX_TABLE prefixTable
        )
        {
            DRS_MSG_MOVEREQ_V1 V1 = new DRS_MSG_MOVEREQ_V1();
            V1.pSourceDSA = pSourceDSA;
            V1.pObject = new ENTINF[1];
            V1.pObject[0] = pObject;
            V1.pParentUUID = new Guid[] { pParentUUID };
            V1.PrefixTable = prefixTable;
            V1.ulFlags = DRS_MSG_MOVEREQ_V1_ulFlags_Values.V1;

            DRS_MSG_MOVEREQ message = new DRS_MSG_MOVEREQ();
            message.V1 = V1;

            return message;

        }

        /// <summary>
        /// Version2 request message sent to the IDL_DRSInterDomainMove method.
        /// </summary>
        /// <param name="pSrcDSA">The client DC nTDSDSA object</param>
        /// <param name="pSrcObject">The object to be moved</param>
        /// <param name="pDstName">The name the object will have in the destination domain</param>
        /// <param name="pExpectedTargetNC">The NC to which pSrcObject is being moved</param>
        /// <param name="secBufferDesc">The credentials of the user initiating the call</param>
        /// <param name="prefixTable">The prefix table with which to translate the ATTRTYP values in pObject to OIDs</param>
        /// <returns>DRS_MSG_MOVEREQ</returns>
        [CLSCompliant(false)]
        public DRS_MSG_MOVEREQ CreateDrsInterDomainMoveRequestV2(
            DSNAME? pSrcDSA,
            ENTINF pSrcObject,
            DSNAME? pDstName,
            DSNAME? pExpectedTargetNC,
            DRS_SecBufferDesc secBufferDesc,
            SCHEMA_PREFIX_TABLE prefixTable
        )
        {
            DRS_MSG_MOVEREQ_V2 V2 = new DRS_MSG_MOVEREQ_V2();
            V2.pSrcDSA = pSrcDSA;
            V2.pSrcObject = new ENTINF[1];
            V2.pSrcObject[0] = pSrcObject;
            V2.pDstName = pDstName;
            V2.pExpectedTargetNC = pExpectedTargetNC;
            V2.pClientCreds = new DRS_SecBufferDesc[1];
            V2.pClientCreds[0] = secBufferDesc;
            V2.ulFlags = ulFlags_Values.V1;

            DRS_MSG_MOVEREQ message = new DRS_MSG_MOVEREQ();
            message.V2 = V2;

            return message;
        }

        /// <summary>
        /// create ENTINF which is a concrete type for the identity and attributes
        ///  (some or all) of a given object.
        /// </summary>
        /// <param name="ncReplicaDistinguishedName"></param>
        /// <param name="ncReplicaObjectGuid"></param>
        /// <param name="ncReplicaObjectSid"></param>
        /// <param name="ulFlags">       
        ///  A flags field that supports the following flags.01234567891
        ///  01234567892 01234567893 01MDOXXXXXXXXXXXXXXRMXXXXXXXXXXXXXXXX:
        ///  Unused. MUST be zero and ignored.M (ENTINF_FROM_MASTER):
        ///  Retrieved from a full replica.DO (ENTINF_DYNAMIC_OBJECT):
        ///  A dynamic object.RM (ENTINF_REMOTE_MODIFY): A remote
        ///  modify request to IDL_DRSAddEntry (section ).
        ///</param>       
        /// <param name="pAttr">An array of attributes and their values</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public ENTINF CreateENTINF(
            string ncReplicaDistinguishedName,
            Guid ncReplicaObjectGuid,
            string ncReplicaObjectSid,
            uint ulFlags,
            ATTR[] pAttr)
        {
            ENTINF ENTINFValue = new ENTINF();
            ENTINFValue.pName = CreateDsName(ncReplicaDistinguishedName, ncReplicaObjectGuid, ncReplicaObjectSid);
            ENTINFValue.ulFlags = ulFlags;
            ATTRBLOCK attrBlock = new ATTRBLOCK();
            attrBlock.pAttr = pAttr;
            attrBlock.attrCount = pAttr == null ? 0 : (uint)pAttr.Length;

            ENTINFValue.AttrBlock = attrBlock;

            return ENTINFValue;
        }

        /// <summary>
        /// Create ATTR
        /// </summary>
        /// <param name="attrTyp">An attribute</param>
        /// <param name="AttrVal">The sequence of values for this attribute.</param>
        /// <returns>ATTR, The ATTR structure defines a concrete type for the identity
        ///  and values of an attribute.</returns>
        [CLSCompliant(false)]
        public static ATTR CreateATTR(uint attrTyp, ATTRVALBLOCK AttrVal)
        {
            ATTR ATTRValue = new ATTR();
            ATTRValue.attrTyp = attrTyp;
            ATTRValue.AttrVal = AttrVal;

            return ATTRValue;

        }

        /// <summary>
        /// create ATTRVALBLOCK
        /// </summary>
        /// <param name="pAVal">The sequence of attribute values.</param>
        /// <returns>ATTRVALBLOCK. The ATTRVALBLOCK structure defines a concrete type for
        ///  a sequence of attribute values</returns>
        [CLSCompliant(false)]
        public static ATTRVALBLOCK CreateATTRVALBLOCK(ATTRVAL[] pAVal)
        {
            ATTRVALBLOCK ATTRVALBLOCKAvalue = new ATTRVALBLOCK();
            ATTRVALBLOCKAvalue.pAVal = pAVal;
            ATTRVALBLOCKAvalue.valCount = pAVal == null ? 0 : (uint)pAVal.Length;

            return ATTRVALBLOCKAvalue;
        }

        /// <summary>
        /// create ATTRVAL
        /// </summary>
        /// <param name="pVal">The value of the attribute</param>
        /// <returns>The ATTRVAL structure defines a concrete type for the
        ///  value of a single attribute</returns>
        [CLSCompliant(false)]
        public static ATTRVAL CreateATTRVAL(byte[] pVal)
        {
            ATTRVAL ATTRVALValue = new ATTRVAL();
            ATTRVALValue.valLen = pVal == null ? 0 : (uint)pVal.Length;
            ATTRVALValue.pVal = pVal;

            return ATTRVALValue;
        }

        /// <summary>
        /// Create an ATTRBLOCK
        /// </summary>
        /// <param name="attrs">Array of ATTR values.</param>
        /// <returns>An ATTRBLOCK structure.</returns>
        [CLSCompliant(false)]
        public static ATTRBLOCK CreateATTRBLOCK(ATTR[] attrs)
        {
            ATTRBLOCK attrBlock = new ATTRBLOCK();
            attrBlock.attrCount = (attrs == null ? 0 : (uint)attrs.Length);
            attrBlock.pAttr = attrs;

            return attrBlock;
        }
        /// <summary>
        /// Create Drs Add Entry V1 request 
        /// </summary>
        /// <param name="ncReplicaDistinguishedName">the object's distinguishedName attribute of the root of an NC
        /// replica on the server.</param>
        /// <param name="ncReplicaObjectGuid">the object's objectGUID attribute of the root of an NC replica on the
        /// server.</param>
        /// <param name="ncReplicaObjectSid">the object's objectSid attribute of the root of an NC replica on the
        /// server.</param>        
        /// <param name="pAttr">An array of attributes and their values</param>
        /// <returns>DRS_MSG_ADDENTRYREQ V1 request</returns>
        [CLSCompliant(false)]
        public DRS_MSG_ADDENTRYREQ CreateDrsAddEntryV1(
            string ncReplicaDistinguishedName,
            Guid ncReplicaObjectGuid,
            string ncReplicaObjectSid,
            ATTR[] pAttr
        )
        {
            DRS_MSG_ADDENTRYREQ_V1 V1 = new DRS_MSG_ADDENTRYREQ_V1();
            V1.pObject = CreateDsName(ncReplicaDistinguishedName, ncReplicaObjectGuid, ncReplicaObjectSid);
            ATTRBLOCK attrBlock = new ATTRBLOCK();
            attrBlock.pAttr = pAttr;
            attrBlock.attrCount = pAttr == null ? 0 : (uint)pAttr.Length;
            V1.AttrBlock = attrBlock;

            DRS_MSG_ADDENTRYREQ message = new DRS_MSG_ADDENTRYREQ();
            message.V1 = V1;

            return message;
        }

        /// <summary>
        /// Create Drs Add Entry V2 request 
        /// </summary>
        /// <param name="EntInfList">The next ENTINFLIST in the sequence, or null</param>
        /// <returns>DRS_MSG_ADDENTRYREQ V2 request</returns>
        [CLSCompliant(false)]
        public DRS_MSG_ADDENTRYREQ CreateDrsAddEntryV2(
            ENTINFLIST EntInfList
        )
        {
            DRS_MSG_ADDENTRYREQ_V2 V2 = new DRS_MSG_ADDENTRYREQ_V2();
            V2.EntInfList = EntInfList;

            DRS_MSG_ADDENTRYREQ message = new DRS_MSG_ADDENTRYREQ();
            message.V2 = V2;

            return message;
        }

        /// <summary>
        /// Create or add new object to an exist ENTINFLIST
        /// </summary>
        /// <param name="srcENTINFLIST">the ENTINFLIST which will add element to</param>
        /// <param name="Entinf">the Entinf to add element</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public ENTINFLIST AddENTINFLIST(ENTINFLIST? srcENTINFLIST, ENTINF Entinf)
        {
            ENTINFLIST ENTINFLISTENTINE;
            ENTINFLISTENTINE = srcENTINFLIST == null ? new ENTINFLIST() : srcENTINFLIST.Value;

            ENTINFLIST ENTINFLISTElement = new ENTINFLIST();
            ENTINFLISTElement.Entinf = Entinf;
            ENTINFLISTElement.pNextEntInf = null;

            ENTINFLISTENTINE.pNextEntInf[0] = ENTINFLISTElement;

            return ENTINFLISTENTINE;
        }


        /// <summary>
        ///Create Drs Add Entry V2 request 
        /// </summary>
        /// <param name="EntInfList">The objects to be added</param>
        /// <param name="Buffers">Buffers that contain authentication data for pClientCreds
        /// pClientCreds is the user credentials to authorize the operation</param>
        /// <returns>Entry V3 request </returns>
        [CLSCompliant(false)]
        public DRS_MSG_ADDENTRYREQ CreateDrsAddEntryV3(
            ENTINFLIST EntInfList,
            DRS_SecBuffer[] Buffers
        )
        {
            DRS_MSG_ADDENTRYREQ_V3 V3 = new DRS_MSG_ADDENTRYREQ_V3();
            V3.EntInfList = EntInfList;

            DRS_SecBufferDesc pClientCreds = new DRS_SecBufferDesc();
            pClientCreds.ulVersion = ulVersion_Values.V1;
            pClientCreds.Buffers = Buffers;
            pClientCreds.cBuffers = Buffers == null ? 0 : (uint)Buffers.Length;

            V3.pClientCreds = new DRS_SecBufferDesc[1];
            V3.pClientCreds[0] = pClientCreds;

            DRS_MSG_ADDENTRYREQ message = new DRS_MSG_ADDENTRYREQ();
            message.V3 = V3;

            return message;
        }

        /// <summary>
        /// Create DrsGetObject Existence Request
        /// </summary>
        /// <param name="guidStart">The objectGUID of the first object in the client's object sequence.</param>
        /// <param name="cGuids">The number of objects in the client's object sequence</param>
        /// <param name="pNc">The DSName of the specified NC.</param>
        /// <param name="utdVector">rgCursors of the filter excluding objects from the client's object sequence</param>
        /// <param name="Md5Digest">The digest of the objectGUID values of the objects in the client's object sequence</param>
        /// <returns>DRS_MSG_EXISTREQ version 1</returns>
        [CLSCompliant(false)]
        public DRS_MSG_EXISTREQ CreateDrsGetObjectExistenceRequest(
            Guid guidStart,
            uint cGuids,
            DSNAME? pNc,
            UPTODATE_VECTOR_V1_EXT utdVector,
            byte[] Md5Digest
        )
        {
            DRS_MSG_EXISTREQ_V1 V1 = new DRS_MSG_EXISTREQ_V1();
            V1.guidStart = guidStart;
            V1.cGuids = cGuids;
            V1.pNC = pNc;
            V1.pUpToDateVecCommonV1 = new UPTODATE_VECTOR_V1_EXT[1];
            V1.pUpToDateVecCommonV1[0] = utdVector;
            V1.Md5Digest = Md5Digest;

            DRS_MSG_EXISTREQ message = new DRS_MSG_EXISTREQ();

            message.V1 = V1;
            return message;
        }



        /// <summary>
        /// to create a struct of DSNAME.
        /// </summary>
        /// <param name="distinguishedName">the distinguishedName attribute of the DSNAME.</param>
        /// <param name="guid">the objectGUID attribute of the DSNAME.</param>
        /// <param name="sid">the objectSid attribute of the DSNAME.</param>
        /// <returns> the created DSNAME.</returns>
        [CLSCompliant(false)]
        public static DSNAME CreateDsName(
            string distinguishedName,
            Guid guid,
            string sid)
        {
            // The size of Sid is exactly 28 bytes, regardless of the value of SidLen, which specifies how many
            // bytes in this field are used. Note that this is smaller than the theoretical size limit of a SID,
            // which is 68 bytes. While Windows publishes a general SID format, Windows never uses that format
            // in its full generality. 28 bytes is sufficient for a Windows SID.
            const int SidSize = 28;

            // The size of struct DSNAME, excluding the size of StringName.
            const int DsNameSize = 56;

            DSNAME dsName = new DSNAME();

            if (distinguishedName == null)
            {
                dsName.NameLen = 0;
                dsName.StringName = ConvertUnicodeStringToUshortArray("\0");
            }
            else
            {
                dsName.StringName = ConvertUnicodeStringToUshortArray(distinguishedName + "\0");
                dsName.NameLen = (uint)distinguishedName.Length;
            }

            dsName.Guid = guid;
            dsName.Sid = new NT4SID();
            dsName.Sid.Data = new byte[SidSize];

            if (string.IsNullOrEmpty(sid))
            {
                dsName.SidLen = 0;
            }
            else
            {
                SecurityIdentifier securityIdentifier = new SecurityIdentifier(sid);
                dsName.SidLen = (uint)securityIdentifier.BinaryLength;

                if (dsName.SidLen != 0)
                {
                    securityIdentifier.GetBinaryForm(dsName.Sid.Data, 0);
                }
            }

            dsName.structLen = (uint)(DsNameSize + dsName.StringName.Length * 2);

            return dsName;
        }

        /// <summary>
        /// To create a request message of DRS_MSG_WRITENGCKEYREQ.
        /// </summary>
        /// <param name="account">The DN of the object to modify.</param>
        /// <param name="ngcKey">The Ngc Key value to set on the object.</param>
        /// <returns> the created RPC input parameter.</returns>
        [CLSCompliant(false)]
        public DRS_MSG_WRITENGCKEYREQ CreateWriteNgcKeyRequest(
            string account,
            byte[] ngcKey)
        {
            DRS_MSG_WRITENGCKEYREQ_V1 message = new DRS_MSG_WRITENGCKEYREQ_V1();
            message.pwszAccount = account;
            message.cNgcKey = ngcKey == null ? 0 : (uint)ngcKey.Length;
            message.pNgcKey = ngcKey;

            DRS_MSG_WRITENGCKEYREQ request = new DRS_MSG_WRITENGCKEYREQ();
            request.V1 = message;

            return request;
        }

        /// <summary>
        /// To create a request message of DRS_MSG_READNGCKEYREQ.
        /// </summary>
        /// <param name="account">The DN of the object to modify.</param>
        /// <returns> the created RPC input parameter.</returns>
        [CLSCompliant(false)]
        public DRS_MSG_READNGCKEYREQ CreateReadNgcKeyRequest(
            string account)
        {
            DRS_MSG_READNGCKEYREQ_V1 message = new DRS_MSG_READNGCKEYREQ_V1();
            message.pwszAccount = account;

            DRS_MSG_READNGCKEYREQ request = new DRS_MSG_READNGCKEYREQ();
            request.V1 = message;

            return request;
        }

        #endregion

        #region Raw RPC Call

        /// <summary>
        ///  The DRSBind method creates a context handle that
        ///  is necessary to call any other method in this interface.
        ///  Opnum: 0 
        /// </summary>
        /// <param name="ClientSessionContext">
        /// ClientSessionContext contains RPC handle, Server Extensions and DRSRHandle
        ///  RPC handle is an RPC binding handle, as specified in [C706].
        ///  Server Extensions is s pointer to a pointer to server capabilities, for use
        ///  in version negotiation.
        ///  DRSR Handle is a pointer to an RPC context handle (as specified in
        ///  [C706]), which may be used in calls to other methods
        ///  in this interface.
        /// </param>
        /// <param name="clientDsaUuid">
        ///  A pointer to a GUID that identifies the caller.
        /// </param>
        /// <param name="clientExtensions">
        ///  A pointer to client capabilities, for use in version
        ///  negotiation.
        /// </param>     
        /// <returns>RPC return value.</returns>
        [CLSCompliant(false)]
        public uint DrsBind(
            DrsrClientSessionContext ClientSessionContext,
            Guid? clientDsaUuid,
            DRS_EXTENSIONS? clientExtensions
            )
        {

            DRS_EXTENSIONS_INT? serverExtensions = null;
            IntPtr? dsrHandle = null;

            ///<summary>
            ///Section 4.1.3.1: Windows non-DC client callers always pass NTDSAPI_CLIENT_GUID in puuidClientDsa.
            ///If a Windows DC client caller uses the returned DRS_HANDLE for subsequent calls to the IDL_DRSWriteSPN method, 
            ///then the client MUST pass NTDSAPI_CLIENT_GUID in puuidClientDsa.
            ///</summary>           

            uint? retVal = rpcAdapter.IDL_DRSBind(
                ClientSessionContext.RPCHandle,
                clientDsaUuid,
                clientExtensions,
                out serverExtensions,
                out dsrHandle);

            ClientSessionContext.ServerExtensions = serverExtensions;
            ClientSessionContext.DRSHandle = dsrHandle.Value;
            return retVal.Value;
        }


        /// <summary>
        ///  The DRSUnbind method destroys a context handle previously
        ///  created by the DRSBind method. Opnum: 1 
        /// </summary>
        /// <param name="drsHandle">
        ///  A pointer to the RPC context handle returned by the
        ///  DRSBind method. The value is set to null on return.
        /// </param>
        /// <returns>RPC return value.</returns>
        [CLSCompliant(false)]
        public uint DrsUnbind(DrsrClientSessionContext ClientSessionContext)
        {
            IntPtr? dsrHandle;
            dsrHandle = ClientSessionContext.DRSHandle;
            uint retVal = rpcAdapter.IDL_DRSUnbind(ref dsrHandle);

            ClientSessionContext.DRSHandle = dsrHandle.Value;
            return retVal;
        }


        /// <summary>
        ///  The DRSReplicaSync method triggers replication from
        ///  another DC. This method is used only to diagnose,
        ///  monitor, and manage the replication implementation.
        ///  The structures requested and returned through this
        ///  method MAY have meaning to peer DCs and applications
        ///  but are not required for interoperation with Windows
        ///  clients. Opnum: 2 
        /// </summary>
        /// <param name="drsHandle">
        ///  The RPC context handle returned by the DRSBind method.
        /// </param>
        /// <param name="inVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="inMessage">
        ///  A pointer to the request message.
        /// </param>
        /// <returns>RPC return value.</returns>
        [CLSCompliant(false)]
        public uint DrsReplicaSync(
            DrsrClientSessionContext clientSessionContext,
            uint inVersion,
            DRS_MSG_REPSYNC? inMessage)
        {
            return rpcAdapter.IDL_DRSReplicaSync(
                clientSessionContext.DRSHandle,
                inVersion,
                inMessage);
        }


        /// <summary>
        ///  The DRSUpdateRefs method adds or deletes a value
        ///  from the repsTo of a specified NC replica. This method
        ///  is used only to diagnose, monitor, and manage the replication
        ///  implementation. The structures requested and returned
        ///  through this method MAY have meaning to peer DCs and
        ///  applications but are not required for interoperation
        ///  with Windows clients. Opnum: 4 
        /// </summary>
        /// <param name="drsHandle">
        ///  The RPC context handle returned by the DRSBind method.
        /// </param>
        /// <param name="inVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="inMessage">
        ///  A pointer to the request message.
        /// </param>
        /// <returns>RPC return value.</returns>
        [CLSCompliant(false)]
        public uint DrsUpdateRefs(
            DrsrClientSessionContext clientSessionContext,
            uint inVersion,
            DRS_MSG_UPDREFS? inMessage)
        {
            return rpcAdapter.IDL_DRSUpdateRefs(
                clientSessionContext.DRSHandle,
                inVersion,
                inMessage);
        }


        /// <summary>
        ///  The DRSReplicaAdd method adds a replication source
        ///  reference for the specified NC. This method is used
        ///  only to diagnose, monitor, and manage the replication
        ///  implementation. The structures requested and returned
        ///  through this method MAY have meaning to peer DCs and
        ///  applications but are not required for interoperation
        ///  with Windows clients. Opnum: 5 
        /// </summary>
        /// <param name="drsHandle">
        ///  The RPC context handle returned by the DRSBind method.
        /// </param>
        /// <param name="inVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="inMessage">
        ///  A pointer to the request message.
        /// </param>
        /// <returns>RPC return value.</returns>
        [CLSCompliant(false)]
        public uint DrsReplicaAdd(
            DrsrClientSessionContext clientSessionContext,
            uint inVersion,
            DRS_MSG_REPADD? inMessage)
        {
            return rpcAdapter.IDL_DRSReplicaAdd(
                clientSessionContext.DRSHandle,
                inVersion,
                inMessage);
        }


        /// <summary>
        ///  The DRSReplicaDel method deletes a replication source
        ///  reference for the specified NC. This method is used
        ///  only to diagnose, monitor, and manage the replication
        ///  implementation. The structures requested and returned
        ///  through this method MAY have meaning to peer DCs and
        ///  applications but are not required for interoperation
        ///  with Windows clients. Opnum: 6 
        /// </summary>
        /// <param name="drsHandle">
        ///  The RPC context handle returned by DRSBind.
        /// </param>
        /// <param name="inVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="inMessage">
        ///  A pointer to the request message.
        /// </param>
        /// <returns>RPC return value.</returns>
        [CLSCompliant(false)]
        public uint DrsReplicaDel(
            DrsrClientSessionContext clientSessionContext,
            uint inVersion,
            DRS_MSG_REPDEL? inMessage)
        {
            return rpcAdapter.IDL_DRSReplicaDel(
                clientSessionContext.DRSHandle,
                inVersion,
                inMessage);
        }


        /// <summary>
        ///  The DRSReplicaModify method updates the value for
        ///  repsFrom for the NC replica. This method is used only
        ///  to diagnose, monitor, and manage the replication implementation.
        ///  The structures requested and returned through this
        ///  method MAY have meaning to peer DCs and applications
        ///  but are not required for interoperation with Windows
        ///  clients. Opnum: 7 
        /// </summary>
        /// <param name="drsHandle">
        ///  The RPC context handle returned by DRSBind.
        /// </param>
        /// <param name="inVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="inMessage">
        ///  A pointer to the request message.
        /// </param>
        /// <returns>RPC return value.</returns>
        [CLSCompliant(false)]
        public uint DrsReplicaModify(
            DrsrClientSessionContext clientSessionContext,
            uint inVersion,
            DRS_MSG_REPMOD? inMessage)
        {
            return rpcAdapter.IDL_DRSReplicaModify(
                clientSessionContext.DRSHandle,
                inVersion,
                inMessage);
        }


        /// <summary>
        ///  The DRSCrackNames method looks up each of a set
        ///  of objects in the directory and returns it to the caller
        ///  in the requested format. Opnum: 12 
        /// </summary>
        /// <param name="drsHandle">
        ///  RPC context handle returned by the DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  Version of the request message.
        /// </param>
        /// <param name="inMessage">
        ///  Pointer to the request message.
        /// </param>
        /// <param name="outVersion">
        ///  Pointer to the version of the response message.
        /// </param>
        /// <param name="outMessage">
        ///  Pointer to the response message.
        /// </param>
        /// <returns>RPC return value.</returns>
        [CLSCompliant(false)]
        public uint DrsCrackNames(
            DrsrClientSessionContext clientSessionContext,
            uint dwInVersion,
            DRS_MSG_CRACKREQ? inMessage,
            out uint? outVersion,
            out DRS_MSG_CRACKREPLY? outMessage)
        {
            return rpcAdapter.IDL_DRSCrackNames(
                clientSessionContext.DRSHandle,
                dwInVersion,
                inMessage,
                out outVersion,
                out outMessage);
        }


        /// <summary>
        ///  The DRSWriteSPN method updates the set of SPNs on
        ///  an object. Opnum: 13 
        /// </summary>
        /// <param name="drsHandle">
        ///  The RPC context handle returned by the DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message. MUST be set to 1,
        ///  because that is the only version supported.
        /// </param>
        /// <param name="inMessage">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="outVersion">
        ///  A pointer to the version of the response message. The
        ///  value is always 1 because that is the only version
        ///  supported.
        /// </param>
        /// <param name="outMessage">
        ///  A pointer to the response message.
        /// </param>
        /// <returns>RPC return value.</returns>
        [CLSCompliant(false)]
        public uint DrsWriteSpn(
            DrsrClientSessionContext clientSessionContext,
            dwInVersion_Values dwInVersion,
            DRS_MSG_SPNREQ? inMessage,
            out pdwOutVersion_Values? outVersion,
            out DRS_MSG_SPNREPLY? outMessage)
        {
            return rpcAdapter.IDL_DRSWriteSPN(
                clientSessionContext.DRSHandle,
                dwInVersion,
                inMessage,
                out outVersion,
                out outMessage);
        }


        /// <summary>
        ///  The DRSRemoveDsServer method removes the representation
        ///  (also known as metadata) of a DC from the directory.
        ///  Opnum: 14 
        /// </summary>
        /// <param name="drsHandle">
        ///  The RPC context handle returned by the DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message. MUST be set to 1
        ///  because that is the only version supported.
        /// </param>
        /// <param name="inMessage">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="outVersion">
        ///  A pointer to the version of the response message. The
        ///  value is always 1 because that is the only version
        ///  supported.
        /// </param>
        /// <param name="outMessage">
        ///  A pointer to the response message.
        /// </param>
        /// <returns>RPC return value.</returns>
        [CLSCompliant(false)]
        public uint DrsRemoveDsServer(
            DrsrClientSessionContext clientSessionContext,
            IDL_DRSRemoveDsServer_dwInVersion_Values dwInVersion,
            DRS_MSG_RMSVRREQ? inMessage,
            out  IDL_DRSRemoveDsServer_pdwOutVersion_Values? outVersion,
            out DRS_MSG_RMSVRREPLY? outMessage)
        {
            return rpcAdapter.IDL_DRSRemoveDsServer(
                clientSessionContext.DRSHandle,
                dwInVersion,
                inMessage,
                out outVersion,
                out outMessage);
        }


        /// <summary>
        ///  The DRSRemoveDsDomain method removes the representation
        ///  (also known as metadata) of a domain from the directory.
        ///  Opnum: 15 
        /// </summary>
        /// <param name="drsHandle">
        ///  The RPC context handle returned by the DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message. This MUST be set
        ///  to 1, because this is the only version supported.
        /// </param>
        /// <param name="inMessage">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="outVersion">
        ///  A pointer to the version of the response message. The
        ///  value is always 1 because that is the only version
        ///  supported.
        /// </param>
        /// <param name="outMessage">
        ///  A pointer to the response message.
        /// </param>
        /// <returns>RPC return value.</returns>
        [CLSCompliant(false)]
        public uint DrsRemoveDsDomain(
            DrsrClientSessionContext clientSessionContext,
             IDL_DRSRemoveDsDomain_dwInVersion_Values dwInVersion,
            DRS_MSG_RMDMNREQ? inMessage,
            out  IDL_DRSRemoveDsDomain_pdwOutVersion_Values? outVersion,
            out DRS_MSG_RMDMNREPLY? outMessage)
        {
            return rpcAdapter.IDL_DRSRemoveDsDomain(
                clientSessionContext.DRSHandle,
                dwInVersion,
                inMessage,
                out outVersion,
                out outMessage);
        }


        /// <summary>
        ///  The DRSDomainControllerInfo method retrieves information
        ///  about DCs in a given domain. Opnum: 16 
        /// </summary>
        /// <param name="drsHandle">
        ///  RPC context handle returned by the DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  Version of the request message.
        /// </param>
        /// <param name="inMessage">
        ///  Pointer to the request message.
        /// </param>
        /// <param name="outVersion">
        ///  Pointer to the version of the response message.
        /// </param>
        /// <param name="outMessage">
        ///  Pointer to the response message.
        /// </param>
        /// <returns>RPC return value.</returns>
        [CLSCompliant(false)]
        public uint DrsDomainControllerInfo(
            DrsrClientSessionContext clientSessionContext,
            uint dwInVersion,
            DRS_MSG_DCINFOREQ? inMessage,
            out uint? outVersion,
            out DRS_MSG_DCINFOREPLY? outMessage)
        {
            if (inMessage != null)
            {
                if (inMessage.Value.V1.InfoLevel == 3 && clientSessionContext.DomainFunLevel < DrsrDomainFunctionLevel.DS_BEHAVIOR_WIN2008)
                {
                    throw new InvalidOperationException("Infolevel 3 only supported by Domain Function Level 2008 or later");
                }
            }


            return rpcAdapter.IDL_DRSDomainControllerInfo(
                clientSessionContext.DRSHandle,
                dwInVersion,
                inMessage,
                out outVersion,
                out outMessage);
        }


        /// <summary>
        ///  The DRSExecuteKCC method validates the replication
        ///  interconnections of DCs and updates them if necessary.
        ///   This method is used only to diagnose, monitor, and
        ///  manage the replication topology implementation. The
        ///  structures requested and returned through this method
        ///  MAY have meaning to peer DCs and applications but are
        ///  not required for interoperation with Windows clients.
        ///  Opnum: 18 
        /// </summary>
        /// <param name="drsHandle">
        ///  The RPC context handle returned by the DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="inMessage">
        ///  A pointer to the request message.
        /// </param>
        /// <returns>RPC return value.</returns>
        [CLSCompliant(false)]
        public uint DrsExecuteKcc(
            DrsrClientSessionContext clientSessionContext,
            uint dwInVersion,
            DRS_MSG_KCC_EXECUTE? inMessage)
        {
            return rpcAdapter.IDL_DRSExecuteKCC(
                clientSessionContext.DRSHandle,
                dwInVersion,
                inMessage);
        }


        /// <summary>
        ///  The DRSGetReplInfo method retrieves the replication
        ///  state of the server. This method is used only to diagnose,
        ///  monitor, and manage the replication implementation.
        ///  The structures requested and returned through this
        ///  method MAY have meaning to peer DCs and applications
        ///  but are not required for interoperation with Windows
        ///  clients. Opnum: 19 
        /// </summary>
        /// <param name="drsHandle">
        ///  The RPC context handle returned by the DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="inMessage">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="outVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="outMessage">
        ///  A pointer to the response message.
        /// </param>
        /// <returns>RPC return value.</returns>
        [CLSCompliant(false)]
        public uint DrsGetReplInfo(
            DrsrClientSessionContext clientSessionContext,
            uint dwInVersion,
            DRS_MSG_GETREPLINFO_REQ? inMessage,
            out uint? outVersion,
            out DRS_MSG_GETREPLINFO_REPLY? outMessage)
        {
            if (inMessage != null)
            {
                if (dwInVersion == 2
                    && clientSessionContext.DomainFunLevel < DrsrDomainFunctionLevel.DS_BEHAVIOR_WIN2003)
                {
                    throw new Exception("DRS_MSG_GETREPLINFO_REQ_V2 is only supported by Domain Function Level 2003 or later");
                }
            }

            return rpcAdapter.IDL_DRSGetReplInfo(
                clientSessionContext.DRSHandle,
                dwInVersion,
                inMessage,
                out outVersion,
                out outMessage);
        }


        /// <summary>
        ///  The DRSAddSidHistory method adds one or more SIDs
        ///  to the sIDHistoryattribute of a given object. Opnum
        ///  : 20 
        /// </summary>
        /// <param name="drsHandle">
        ///  RPC context handle returned by the DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  Version of the request message. Must be set to 1, because
        ///  no other version is supported.
        /// </param>
        /// <param name="inMessage">
        ///  Pointer to the request message.
        /// </param>
        /// <param name="outVersion">
        ///  Pointer to the version of the response message. The
        ///  value will always be 1, because no other version is
        ///  supported.
        /// </param>
        /// <param name="outMessage">
        ///  Pointer to the response message.
        /// </param>
        /// <returns>RPC return value.</returns>
        [CLSCompliant(false)]
        public uint DrsAddSidHistory(
            DrsrClientSessionContext clientSessionContext,
            IDL_DRSAddSidHistory_dwInVersion_Values dwInVersion,
            DRS_MSG_ADDSIDREQ? inMessage,
            out  IDL_DRSAddSidHistory_pdwOutVersion_Values? outVersion,
            out DRS_MSG_ADDSIDREPLY? outMessage)
        {
            return rpcAdapter.IDL_DRSAddSidHistory(
                clientSessionContext.DRSHandle,
                dwInVersion,
                inMessage,
                out outVersion,
                out outMessage);
        }


        /// <summary>
        ///  The DRSReplicaVerifyObjects method verifies the
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
        /// <param name="drsHandle">
        ///  The RPC context handle returned by the DRSBind method.
        /// </param>
        /// <param name="inVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="inMessage">
        ///  A pointer to the request message.
        /// </param>
        /// <returns>RPC return value.</returns>
        [CLSCompliant(false)]
        public uint DrsReplicaVerifyObjects(
            DrsrClientSessionContext clientSessionContext,
            uint inVersion,
            DRS_MSG_REPVERIFYOBJ? inMessage)
        {
            if (clientSessionContext.DomainFunLevel < DrsrDomainFunctionLevel.DS_BEHAVIOR_WIN2003)
            {
                throw new InvalidOperationException("IDL_DRSReplicaVerifyObjects is only supported by Domain Function Level 2003 or later");
            }
            return rpcAdapter.IDL_DRSReplicaVerifyObjects(
                clientSessionContext.DRSHandle,
                inVersion,
                inMessage);
        }


        /// <summary>
        ///  The DRSQuerySitesByCost method determines the communication
        ///  cost from a "from" site to one or more "to" sites.
        ///  Opnum: 24 
        /// </summary>
        /// <param name="drsHandle">
        ///  The RPC context handle returned by the DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="inMessage">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="outVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="outMessage">
        ///  A pointer to the response message.
        /// </param>
        /// <returns>RPC return value.</returns>
        [CLSCompliant(false)]
        public uint DrsQuerySitesByCost(
            DrsrClientSessionContext clientSessionContext,
            uint dwInVersion,
            DRS_MSG_QUERYSITESREQ? inMessage,
            out uint? outVersion,
            out DRS_MSG_QUERYSITESREPLY? outMessage)
        {

            return rpcAdapter.IDL_DRSQuerySitesByCost(
                clientSessionContext.DRSHandle,
                dwInVersion,
                inMessage,
                out outVersion,
                out outMessage);
        }


        /// <summary>
        ///  The DRSInitDemotion method performs the first phase
        ///  of the removal of a DC from an AD LDS forest. This method
        ///  is supported only by AD LDS. This method is used only
        ///  to diagnose, monitor, and manage the implementation
        ///  of server-to-server DC demotion. The structures requested
        ///  and returned through this method MAY have meaning to
        ///  peer DCs and applications but are not required for
        ///  interoperation with Windows clients. Opnum: 25 
        /// </summary>
        /// <param name="drsHandle">
        ///  The RPC context handle returned by the DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="inMessage">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="outVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="outMessage">
        ///  A pointer to the response message.
        /// </param>
        /// <returns>RPC return value.</returns>
        [CLSCompliant(false)]
        public uint DrsInitDemotion(
            DrsrClientSessionContext clientSessionContext,
            uint dwInVersion,
            DRS_MSG_INIT_DEMOTIONREQ? inMessage,
            out uint? outVersion,
            out DRS_MSG_INIT_DEMOTIONREPLY? outMessage)
        {
            if (clientSessionContext.DomainFunLevel < DrsrDomainFunctionLevel.DS_BEHAVIOR_WIN2008)
            {
                throw new InvalidOperationException("IDL_DRSInitDemotion is only supported by Domain Function Level 2008 or later");
            }

            return rpcAdapter.IDL_DRSInitDemotion(
                clientSessionContext.DRSHandle,
                dwInVersion,
                inMessage,
                out outVersion,
                out outMessage);
        }


        /// <summary>
        ///  The DRSReplicaDemotion method  replicates initiates
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
        /// <param name="drsHandle">
        ///  The RPC context handle returned by the DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="inMessage">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="outVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="outMessage">
        ///  A pointer to the response message.
        /// </param>
        /// <returns>RPC return value.</returns>
        [CLSCompliant(false)]
        public uint DrsReplicaDemotion(
            DrsrClientSessionContext clientSessionContext,
            uint dwInVersion,
            DRS_MSG_REPLICA_DEMOTIONREQ? inMessage,
            out uint? outVersion,
            out DRS_MSG_REPLICA_DEMOTIONREPLY? outMessage)
        {
            if (clientSessionContext.DomainFunLevel < DrsrDomainFunctionLevel.DS_BEHAVIOR_WIN2008)
            {
                throw new InvalidOperationException("IDL_DRSReplicaDemotion is only supported by Domain Function Level 2008 or later");
            }

            return rpcAdapter.IDL_DRSReplicaDemotion(
                clientSessionContext.DRSHandle,
                dwInVersion,
                inMessage,
                out outVersion,
                out outMessage);
        }


        /// <summary>
        ///  The DRSFinishDemotion method either performs one
        ///  or more steps toward the complete removal of a DC from
        ///  an AD LDS forest, or it undoes the effects of the first
        ///  phase of removal (performed by DRSInitDemotion).
        ///  This method is supported by AD LDS only. This method
        ///  is used only to diagnose, monitor, and manage the implementation
        ///  of server-to-server DC demotion. The structures requested
        ///  and returned through this method MAY have meaning to
        ///  peer DCs and applications but are not required for
        ///  interoperation with Windows clients. Opnum: 27 
        /// </summary>
        /// <param name="drsHandle">
        ///  The RPC context handle returned by the DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="inMessage">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="outVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="outMessage">
        ///  A pointer to the response message.
        /// </param>
        /// <returns>RPC return value.</returns>
        [CLSCompliant(false)]
        public uint DrsFinishDemotion(
            DrsrClientSessionContext clientSessionContext,
            uint dwInVersion,
            DRS_MSG_FINISH_DEMOTIONREQ? inMessage,
            out uint? outVersion,
            out DRS_MSG_FINISH_DEMOTIONREPLY? outMessage)
        {
            if (clientSessionContext.DomainFunLevel < DrsrDomainFunctionLevel.DS_BEHAVIOR_WIN2008)
            {
                throw new InvalidOperationException("IDL_DRSFinishDemotion is only supported by Domain Function Level 2008 or later");
            }
            return rpcAdapter.IDL_DRSFinishDemotion(
                clientSessionContext.DRSHandle,
                dwInVersion,
                inMessage,
                out outVersion,
                out outMessage);
        }


        /// <summary>
        ///  The DRSGetNCChanges method replicates updates from an NC replica on the server. 
        ///  Opnum: 3 
        /// </summary>
        /// <param name="drsHandle">
        ///  The RPC context handle returned by the DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="inMessage">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="outVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="outMessage">
        ///  A pointer to the response message.
        /// </param>
        [CLSCompliant(false)]
        public uint DrsGetNcChanges(
            DrsrClientSessionContext clientSessionContext,
            uint dwInVersion,
            DRS_MSG_GETCHGREQ? inMessage,
            out uint? outVersion,
            out DRS_MSG_GETCHGREPLY? outMessage)
        {

            ///<summary>
            ///V4:  Version 4 request (Windows 2000 SMTP replication [MS-SRPL]).
            ///V5:  Version 5 request (Windows 2000 RPC replication).
            ///V7:  Version 7 request (Windows Server 2003 SMTP replication [MS-SRPL]).
            ///V8:  Version 8 request (Windows Server 2003 RPC replication).
            ///V10: Version 10 request (Windows Server 2008 R2 RPC replication).
            ///V11: Version 11 request (Windows Server v1803 operating system RPC replication).
            ///</summary>


            if (inMessage != null)
            {
                if (dwInVersion == 5)
                {
                    if (clientSessionContext.DomainFunLevel < DrsrDomainFunctionLevel.DS_BEHAVIOR_WIN2000)
                        throw new InvalidOperationException("DRS_MSG_GETCHGREQ_V5 is only supported by the Windows 2000 RPC replication");
                }
                else if (dwInVersion == 8)
                {
                    if (clientSessionContext.DomainFunLevel < DrsrDomainFunctionLevel.DS_BEHAVIOR_WIN2003)
                        throw new InvalidOperationException("DRS_MSG_GETCHGREQ_V8 is only supported by the Windows 2003 RPC replication");
                }
                else if (dwInVersion == 10)
                {
                    if (clientSessionContext.DomainFunLevel < DrsrDomainFunctionLevel.DS_BEHAVIOR_WIN2008R2)
                        throw new InvalidOperationException("DRS_MSG_GETCHGREQ_V10 is only supported by the Windows 2012 RPC replication");
                }
                else
                {
                    throw new InvalidOperationException(dwInVersion + " version request message is not supported");
                }
            }


            return rpcAdapter.IDL_DRSGetNCChanges(
                clientSessionContext.DRSHandle,
                dwInVersion,
                inMessage,
                out outVersion,
                out outMessage);
        }


        /// <summary>
        ///  The DRSVerifyNames method resolves a sequence of object identities. 
        ///  Opnum: 8 
        /// </summary>
        /// <param name="drsHandle">
        ///  The RPC context handle returned by the DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="inMessage">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="outVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="outMessage">
        ///  A pointer to the response message.
        /// </param>
        [CLSCompliant(false)]
        public uint DrsVerifyNames(
            DrsrClientSessionContext clientSessionContext,
            uint dwInVersion,
            DRS_MSG_VERIFYREQ? inMessage,
            out uint? outVersion,
            out DRS_MSG_VERIFYREPLY? outMessage)
        {
            return rpcAdapter.IDL_DRSVerifyNames(
                clientSessionContext.DRSHandle,
                dwInVersion,
                inMessage,
                out outVersion,
                out outMessage);
        }


        /// <summary>
        ///  The DRSGetMemberships method retrieves group membership for an object. 
        ///  Opnum: 9 
        /// </summary>
        /// <param name="drsHandle">
        ///  The RPC context handle returned by the DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="inMessage">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="outVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="outMessage">
        ///  A pointer to the response message.
        /// </param>
        [CLSCompliant(false)]
        public uint DrsGetMemberships(
            DrsrClientSessionContext clientSessionContext,
            uint dwInVersion,
            DRS_MSG_REVMEMB_REQ? inMessage,
            out uint? outVersion,
            out DRS_MSG_REVMEMB_REPLY? outMessage)
        {
            return rpcAdapter.IDL_DRSGetMemberships(
                clientSessionContext.DRSHandle,
                dwInVersion,
                inMessage,
                out outVersion,
                out outMessage);
        }


        /// <summary>
        ///  The DRSInterDomainMove method is a helper method used in a cross-NC move LDAP operation.
        ///  Opnum: 10 
        /// </summary>
        /// <param name="drsHandle">
        ///  The RPC context handle returned by the DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="inMessage">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="outVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="outMessage">
        ///  A pointer to the response message.
        /// </param>
        [CLSCompliant(false)]
        public uint DrsInterDomainMove(
            DrsrClientSessionContext clientSessionContext,
            uint dwInVersion,
            DRS_MSG_MOVEREQ? inMessage,
            out uint? outVersion,
            out DRS_MSG_MOVEREPLY? outMessage)
        {
            return rpcAdapter.IDL_DRSInterDomainMove(
                clientSessionContext.DRSHandle,
                dwInVersion,
                inMessage,
                out outVersion,
                out outMessage);
        }


        /// <summary>
        ///  If the server is the PDC emulator FSMO role owner, the DRSGetNT4ChangeLog method
        ///  returns either a sequence of PDC change log entries or the NT4 replication state, or
        ///  both, as requested by the client. 
        ///  Opnum: 11 
        /// </summary>
        /// <param name="drsHandle">
        ///  The RPC context handle returned by the DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="inMessage">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="outVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="outMessage">
        ///  A pointer to the response message.
        /// </param>
        [CLSCompliant(false)]
        public uint DrsGetNt4ChangeLog(
            DrsrClientSessionContext clientSessionContext,
            uint dwInVersion,
            DRS_MSG_NT4_CHGLOG_REQ? inMessage,
            out uint? outVersion,
            out DRS_MSG_NT4_CHGLOG_REPLY? outMessage)
        {
            return rpcAdapter.IDL_DRSGetNT4ChangeLog(
                clientSessionContext.DRSHandle,
                dwInVersion,
                inMessage,
                out outVersion,
                out outMessage);
        }


        /// <summary>
        ///  The DRSAddEntry method adds one or more objects. 
        ///  Opnum: 17 
        /// </summary>
        /// <param name="drsHandle">
        ///  The RPC context handle returned by the DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="inMessage">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="outVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="outMessage">
        ///  A pointer to the response message.
        /// </param>
        [CLSCompliant(false)]
        public uint DrsAddEntry(
            DrsrClientSessionContext clientSessionContext,
            uint dwInVersion,
            DRS_MSG_ADDENTRYREQ? inMessage,
            out uint? outVersion,
            out DRS_MSG_ADDENTRYREPLY? outMessage)
        {
            return rpcAdapter.IDL_DRSAddEntry(
                clientSessionContext.DRSHandle,
                dwInVersion,
                inMessage,
                out outVersion,
                out outMessage);
        }


        /// <summary>
        ///  The DRSGetMemberships2 method retrieves group memberships for a sequence of objects. 
        ///  Opnum: 21 
        /// </summary>
        /// <param name="drsHandle">
        ///  The RPC context handle returned by the DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="inMessage">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="outVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="outMessage">
        ///  A pointer to the response message.
        /// </param>
        [CLSCompliant(false)]
        public uint DrsGetMemberships2(
            DrsrClientSessionContext clientSessionContext,
            uint dwInVersion,
            DRS_MSG_GETMEMBERSHIPS2_REQ? inMessage,
            out uint? outVersion,
            out DRS_MSG_GETMEMBERSHIPS2_REPLY? outMessage)
        {
            return rpcAdapter.IDL_DRSGetMemberships2(
                clientSessionContext.DRSHandle,
                dwInVersion,
                inMessage,
                out outVersion,
                out outMessage);
        }


        /// <summary>
        ///  The DRSGetObjectExistence method helps the client check the consistency of object
        ///  existence between its replica of an NC and the server's replica of the same NC. 
        ///  Checking the consistency of object existence means identifying objects that have 
        ///  replicated to both replicas and that exist in one replica but not in the other.
        ///  For the purposes of this method, an object exists within a NC replica if it is either
        ///  an object or a tombstone.See DRSReplicaVerifyObjects for a use of this method. 
        ///  Opnum: 23 
        /// </summary>
        /// <param name="drsHandle">
        ///  The RPC context handle returned by the DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="inMessage">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="outVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="outMessage">
        ///  A pointer to the response message.
        /// </param>
        [CLSCompliant(false)]
        public uint DrsGetObjectExistence(
            DrsrClientSessionContext clientSessionContext,
            uint dwInVersion,
            DRS_MSG_EXISTREQ? inMessage,
            out uint? outVersion,
            out DRS_MSG_EXISTREPLY? outMessage)
        {
            if (clientSessionContext.DomainFunLevel < DrsrDomainFunctionLevel.DS_BEHAVIOR_WIN2003)
            {
                throw new InvalidOperationException("not support IDL_DRSGetObjectExistence before Domain Function Level 2003");
            }

            return rpcAdapter.IDL_DRSGetObjectExistence(
                clientSessionContext.DRSHandle,
                dwInVersion,
                inMessage,
                out outVersion,
                out outMessage);
        }

        /// <summary>
        /// 	Opnum: 28
        /// </summary>
        /// <param name="drsHandle">
        ///  The RPC context handle returned by the DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="inMessage">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="outVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="outMessage">
        ///  A pointer to the response message.
        /// </param>
        [CLSCompliant(false)]
        public uint DrsAddCloneDC(
                DrsrClientSessionContext clientSessionContext,
                uint dwInVersion,
                DRS_MSG_ADDCLONEDCREQ? inMessage,
                out uint? outVersion,
                out DRS_MSG_ADDCLONEDCREPLY? outMessage)
        {

            return rpcAdapter.IDL_DRSAddCloneDC(
                    clientSessionContext.DRSHandle,
                    dwInVersion,
                    inMessage,
                    out outVersion,
                    out outMessage);
        }

        /// <summary>
        ///  The DRSWriteNgcKey method composes and updates the
        ///  msDS-KeyCredentialLink value on an object.
        ///  Opnum: 29 
        /// </summary>
        /// <param name="drsHandle">
        ///  The RPC context handle returned by the DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message. MUST be set to 1,
        ///  because that is the only version supported.
        /// </param>
        /// <param name="inMessage">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="outVersion">
        ///  A pointer to the version of the response message. The
        ///  value is always 1 because that is the only version
        ///  supported.
        /// </param>
        /// <param name="outMessage">
        ///  A pointer to the response message.
        /// </param>
        /// <returns>RPC return value.</returns>
        [CLSCompliant(false)]
        public uint DrsWriteNgcKey(
            DrsrClientSessionContext clientSessionContext,
            uint dwInVersion,
            DRS_MSG_WRITENGCKEYREQ? inMessage,
            out uint? outVersion,
            out DRS_MSG_WRITENGCKEYREPLY? outMessage)
        {
            return rpcAdapter.IDL_DRSWriteNgcKey(
                clientSessionContext.DRSHandle,
                dwInVersion,
                inMessage,
                out outVersion,
                out outMessage);
        }

        /// <summary>
        ///  The DRSReadNgcKey method composes and updates the
        ///  msDS-KeyCredentialLink value on an object.
        ///  Opnum: 30 
        /// </summary>
        /// <param name="drsHandle">
        ///  The RPC context handle returned by the DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message. MUST be set to 1,
        ///  because that is the only version supported.
        /// </param>
        /// <param name="inMessage">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="outVersion">
        ///  A pointer to the version of the response message. The
        ///  value is always 1 because that is the only version
        ///  supported.
        /// </param>
        /// <param name="outMessage">
        ///  A pointer to the response message.
        /// </param>
        /// <returns>RPC return value.</returns>
        [CLSCompliant(false)]
        public uint DrsReadNgcKey(
            DrsrClientSessionContext clientSessionContext,
            uint dwInVersion,
            DRS_MSG_READNGCKEYREQ? inMessage,
            out uint? outVersion,
            out DRS_MSG_READNGCKEYREPLY? outMessage)
        {
            return rpcAdapter.IDL_DRSReadNgcKey(
                clientSessionContext.DRSHandle,
                dwInVersion,
                inMessage,
                out outVersion,
                out outMessage);
        }

        #endregion


        #region private methods

        /// <summary>
        /// Get the dynamic port which is bound to the ADLDS service.
        /// </summary>
        /// <param name="servicePrincipalName">DRSR server machine Principal name.</param>
        /// <param name="clientGuid">DRS client Guid</param>
        /// <param name="protocolSequence">protocol sequence type</param>
        /// <param name="serverComputerName">DRSR server machine computer name</param>
        /// <param name="endPoint">DRSR endpoint, the value can be null when it is AD LDS?</param>
        /// <param name="networkOptions">a string representation of network options. The option string is associated with the protocol sequence.
        ///                              It is used in RpcStringBindingCompose</param>
        /// <exception cref="System.InvalidOperationException">Fail to PInvoke.</exception>
        /// <returns>dynamic port.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults")]
        private string InquiryAdldsDynamicPort(String servicePrincipalName, Guid? clientGuid, String protocolSequence, String serverComputerName, String endPoint, String networkOptions)
        {
            //Get the ldap port of ADLDS service:
            int index = servicePrincipalName.IndexOf(':');
            if (index < 0)
            {
                throw new InvalidOperationException("The LDAP port should be provided when SUT is ADLDS.");
            }
            string ldapPort = servicePrincipalName.Substring(index + 1);

            IntPtr bindingString = IntPtr.Zero;
            IntPtr bindingHandle = IntPtr.Zero;
            uint status = RpcNativeMethods.RpcStringBindingCompose(
                (clientGuid == null) ?
                    null : clientGuid.Value.ToString(),
                protocolSequence,
                serverComputerName,
                endPoint,
                networkOptions,
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
                RpcNativeMethods.RpcBindingFree(ref bindingString);
                throw new InvalidOperationException("Failed to RpcBindingFromStringBinding. The returned status is "
                    + status);
            }

            RpcNativeMethods.RpcBindingFree(ref bindingString);

            IntPtr inquiryContext = IntPtr.Zero;
            Guid uuid = (clientGuid == null) ?
                new Guid() : clientGuid.Value;
            RPC_IF_ID rpcInterfaceInfo = new RPC_IF_ID();
            rpcInterfaceInfo.Uuid = DrsrUtility.DRSUAPI_RPC_INTERFACE_UUID;
            rpcInterfaceInfo.VersMajor = (short)RPC_C_VERS.MAJOR_ONLY;
            status = RpcNativeMethods.RpcMgmtEpEltInqBegin(
                bindingHandle,
                RPC_C_EP.MATCH_BY_IF,
                ref rpcInterfaceInfo,
                RPC_C_VERS.COMPATIBLE,
                ref uuid,
                out inquiryContext);
            if (status != 0)
            {
                RpcNativeMethods.RpcBindingFree(ref bindingString);
                throw new InvalidOperationException("Failed to RpcMgmtEpEltInqBegin. The returned status is "
                    + status);
            }

            //the loop will break when the dynamic port bound to the ADLDS is found. Otherwise, thrown exception:
            while (true)
            {
                Guid queriedUuid = new Guid();
                IntPtr queriedBindingHandle = IntPtr.Zero;
                IntPtr queriedAnnotation = IntPtr.Zero;
                status = RpcNativeMethods.RpcMgmtEpEltInqNext(
                    inquiryContext,
                    ref rpcInterfaceInfo,
                    out queriedBindingHandle,
                    out queriedUuid,
                    out queriedAnnotation);
                if (status != 0)
                {
                    RpcNativeMethods.RpcMgmtEpEltInqDone(ref inquiryContext);
                    RpcNativeMethods.RpcBindingFree(ref bindingString);
                    throw new InvalidOperationException("Failed to RpcMgmtEpEltInqNext. The returned status is "
                        + status);
                }

                //the LDAP port is found:
                if (Marshal.PtrToStringAnsi(queriedAnnotation).Equals(ldapPort))
                {
                    RpcNativeMethods.RpcStringFree(ref queriedAnnotation);
                    RpcNativeMethods.RpcMgmtEpEltInqDone(ref inquiryContext);

                    IntPtr queriedBindingString = IntPtr.Zero;
                    status = RpcNativeMethods.RpcBindingToStringBinding(
                        queriedBindingHandle, out queriedBindingString);
                    RpcNativeMethods.RpcBindingFree(ref queriedBindingHandle);
                    if (status != 0)
                    {
                        RpcNativeMethods.RpcBindingFree(ref bindingHandle);
                        throw new InvalidOperationException("Failed to RpcBindingToStringBinding. The status is "
                            + status);
                    }

                    //get the port from the stringBinding.
                    string dynamicPort = Marshal.PtrToStringAnsi(queriedBindingString);
                    dynamicPort = dynamicPort.Substring(dynamicPort.IndexOf('[') + 1);
                    dynamicPort = dynamicPort.Remove(dynamicPort.Length - 1, 1);

                    RpcNativeMethods.RpcStringFree(ref queriedBindingString);
                    RpcNativeMethods.RpcBindingFree(ref bindingHandle);
                    return dynamicPort;
                }
                else
                {
                    RpcNativeMethods.RpcStringFree(ref queriedAnnotation);
                    RpcNativeMethods.RpcBindingFree(ref queriedBindingHandle);
                }
            }
        }


        /// <summary>
        /// Convert string to ushort array in Unicode mode
        /// </summary>
        /// <param name="source">the byte array to be converted.</param>
        /// <returns> the ushort array.</returns>
        private static ushort[] ConvertUnicodeStringToUshortArray(
            string source)
        {
            ushort[] target;

            if (source == null)
            {
                target = new ushort[0];
            }
            else
            {
                byte[] sourceBytes = Encoding.Unicode.GetBytes(source);
                target = new ushort[(int)Math.Ceiling((double)sourceBytes.Length / 2)];
                Buffer.BlockCopy(sourceBytes, 0, target, 0, sourceBytes.Length);
            }
            return target;
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
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Release managed resources.
                if (rpcAdapter != null)
                {
                    try
                    {
                        rpcAdapter.Unbind();
                    }
                    finally
                    {
                        rpcAdapter.Dispose();
                        rpcAdapter = null;
                    }
                }
            }

            // Release unmanaged resources.
        }


        /// <summary>
        /// finalizer
        /// </summary>
        ~DrsuapiClient()
        {
            Dispose(false);
        }

        #endregion
    }
}

