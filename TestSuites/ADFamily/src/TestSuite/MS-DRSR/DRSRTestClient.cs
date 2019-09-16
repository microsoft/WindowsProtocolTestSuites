// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Security.Principal;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

using System.DirectoryServices.Protocols;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    /// <summary>
    /// Please read the sample in Bind() to see how you can use this test client
    /// </summary>
    public class DRSRTestClient : IDRSRTestClient
    {
        /// <summary>
        /// PTF test site
        /// </summary>
        ITestSite testSite = null;

        /// <summary>
        /// LDAP adapter to retrieve information
        /// </summary>
        ILdapAdapter ldapAd = null;

        /// <summary>
        /// The data verifier to verify request and response for each DRS call
        /// </summary>
        IDRSRVerifier verifier = null;

        /// <summary>
        /// DRS Client to send and receive messages
        /// </summary>
        DrsuapiClient drsClient = null;

        /// <summary>
        /// DRSR client for dsaop methods.
        /// </summary>
        DsaopClient dsaClient = null;

        public DrsuapiClient DRSClient
        {
            get
            {
                return drsClient;
            }
        }

        public DsaopClient DSAOPClient
        {
            get
            {
                return dsaClient;
            }
        }

        /// <summary>
        /// Initialize the test client
        /// </summary>
        /// <param name="site">TestSite from PTF</param>
        public void Initialize(ITestSite site)
        {
            testSite = site;
            EnvironmentConfig.Initialize(site);
            verifier = new DRSRVerifier();
            verifier.Initilize(site);

            IDrsuapiRpcAdapter rpcAdapter = null;
            if (EnvironmentConfig.UseNativeRpcLib)
            {
                IMS_DRSR_RpcAdapter nativeRpcAdapter = site.GetAdapter<IMS_DRSR_RpcAdapter>();
                NativeRpcWrapper wrapper = new NativeRpcWrapper();
                wrapper.NativeRpcAdapter = nativeRpcAdapter;
                rpcAdapter = wrapper;
            }
            drsClient = new DrsuapiClient(rpcAdapter);
            dsaClient = new DsaopClient();

            ldapAd = site.GetAdapter<ILdapAdapter>();
            ldapAd.Site = site;




            DrsrBaseUpdate.SetDrsTestClient(this);
            LdapBaseUpdate.SetLdapAdapter(ldapAd);
            UpdatesStorage.GetInstance().Initialize(site);
        }


        /// <summary>
        /// Unbind all unreleased DRS context
        /// </summary>
        void ClearContexts()
        {
            Dictionary<EnvironmentConfig.Machine, DrsrClientSessionContext>.ValueCollection.Enumerator enumer = EnvironmentConfig.DrsContextStore.Values.GetEnumerator();
            while (enumer.MoveNext())
            {
                //regardless it succeeds or not, for server side fault
                if (enumer.Current.DRSHandle != IntPtr.Zero)
                {
                    try
                    {
                        drsClient.DrsUnbind(enumer.Current);
                    }
                    catch
                    {
                        //ignore server side exception
                    }
                }
            }

            EnvironmentConfig.DrsContextStore.Clear();
        }

        /// <summary>
        /// reset test client, dispose all resource and revert changes
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1304:SpecifyCultureInfo", MessageId = "System.String.ToLower"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", MessageId = "System.String.StartsWith(System.String)")]
        public void Reset()
        {
            //clear test cases bind context firstly. Allow later process to bind sut freely
            ClearContexts();
            using (LdapConnection con
                = new LdapConnection(EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC1].DnsHostName))
            {
                con.SessionOptions.SendTimeout = new TimeSpan(0, 2, 0);
                DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainAdmin];

                con.Credential = new System.Net.NetworkCredential(user.Username, user.Password, user.Domain.DNSName);
                SearchRequest req = new SearchRequest(
                    EnvironmentConfig.DomainStore[DomainEnum.PrimaryDomain].Name,
                    "(objectclass=user)",
                     System.DirectoryServices.Protocols.SearchScope.Subtree,
                     new string[] { "distinguishedname" });
                SearchResponse res = (SearchResponse)con.SendRequest(req);
                foreach (SearchResultEntry entry in res.Entries)
                {
                    if (entry.DistinguishedName.ToLower().StartsWith("cn=user-"))
                    {
                        try
                        {
                            DeleteRequest de = new DeleteRequest(entry.DistinguishedName);
                            con.SendRequest(de);
                        }
                        catch
                        {
                        }
                    }
                }

                req = new SearchRequest(
                   LdapUtility.ConvertUshortArrayToString(EnvironmentConfig.DomainStore[DomainEnum.PrimaryDomain].ConfigNC.StringName),
                    "(objectclass=site)",
                     System.DirectoryServices.Protocols.SearchScope.Subtree,
                     new string[] { "distinguishedname" });
                res = (SearchResponse)con.SendRequest(req);
                foreach (SearchResultEntry entry in res.Entries)
                {
                    if (entry.DistinguishedName.ToLower().StartsWith("cn=site-"))
                    {
                        try
                        {
                            DeleteRequest de = new DeleteRequest(entry.DistinguishedName);
                            con.SendRequest(de);
                        }
                        catch
                        {
                        }
                    }
                }

                UpdatesStorage.GetInstance().Clear();

                if (EnvironmentConfig.EnvBroken)
                {
                    //do revert here
                    IRecoverAdapter recover = testSite.GetAdapter<IRecoverAdapter>();
                    recover.RecoverEnvironment();
                    EnvironmentConfig.EnvBroken = false;
                }
            }

            EnvironmentConfig.ResetMachineLdapConnections();
        }


        protected string convertSidToString(NT4SID id)
        {
            string sid = null;
            try
            {
                sid = new SecurityIdentifier(id.Data, 0).ToString();
            }
            catch
            {
                sid = "";
            }
            return sid;
        }

        public uint DrsBind(EnvironmentConfig.Machine machine, EnvironmentConfig.User user, DRS_EXTENSIONS_IN_FLAGS extFlags)
        {
            return DrsBind(machine, user, extFlags, DRS_EXTENSIONS_IN_FLAGSEXT.DRS_EXT_ADAM | DRS_EXTENSIONS_IN_FLAGSEXT.DRS_EXT_LH_BETA2 | DRS_EXTENSIONS_IN_FLAGSEXT.DRS_EXT_RECYCLE_BIN, (uint)(DRS_EXTENSIONS_IN_FLAGSEXT.DRS_EXT_ADAM | DRS_EXTENSIONS_IN_FLAGSEXT.DRS_EXT_LH_BETA2 | DRS_EXTENSIONS_IN_FLAGSEXT.DRS_EXT_RECYCLE_BIN));
        }

        /// <summary>
        /// Set up RPC session and call IDL_DRSBind to specific AD DS or LDS instance
        /// </summary>
        /// <param name="machine">AD DS or LDS instance on specified SUT</param>
        /// <param name="user">user account for this DRSR session</param>
        /// <param name="extFlags">dwFlagsEx in DRS_EXETENSIONS structure</param>
        /// <returns>0 if IDL_DRSBind success, otherwise, failed</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.UInt32.Parse(System.String)"), SupportedADType(ADInstanceType.Both)]
        public uint DrsBind(EnvironmentConfig.Machine machine, EnvironmentConfig.User user, DRS_EXTENSIONS_IN_FLAGS extFlags, DRS_EXTENSIONS_IN_FLAGSEXT extFlagsExt, uint dwExtCaps)
        {
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[machine];

            uint ret = 0;

            #region preparation
            //RPC bind to server.
            RPCBind(machine, user, DrsrRpcInterfaceType.DRSUAPI);

            Guid cid;
            bool bDcClient;
            uint replEpoch = 0;
            if (user == EnvironmentConfig.User.MainDCAccount || user == EnvironmentConfig.User.WritableDC2Account)
            {

                DsServer Client;
                if (user == EnvironmentConfig.User.MainDCAccount)
                    Client = (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.MainDC];
                else
                    Client = (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2];

                cid = Client.ComputerObjectGuid;

                bDcClient = true;
                string replEpochObj = ldapAd.GetAttributeValueInString(
                    server,
                    server.NtdsDsaObjectName,
                    "msDS-ReplicationEpoch"
                    );
                if (replEpochObj != null)
                {
                    replEpoch = uint.Parse(replEpochObj);
                }


            }

            else
            {
                cid = DRSConstants.NTDSAPI_CLEINT_GUID;
                bDcClient = false;
            }

            testSite.Log.Add(LogEntryKind.Checkpoint, "puuidClientDsa is set to " + cid);
            Guid? puuidClientDsa = cid;

            #endregion

            #region build ext structure

            DRS_EXTENSIONS exts = drsClient.CreateDrsExtensions(
                bDcClient,
                extFlags,
                Guid.Empty,
                0,
                replEpoch,
                extFlagsExt,
                Guid.Empty,
                dwExtCaps);
            //DRS_EXTENSIONS exts = drsClient.CreateDrsExtensions(bDcClient, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE, new Guid(), 0, 0);

            #endregion

            #region DRS Bind
            DrsrClientSessionContext drsContext = EnvironmentConfig.DrsContextStore[machine];

            testSite.Log.Add(LogEntryKind.Checkpoint, "Begin to call IDL_DRSBind");

            try
            {
                ret = drsClient.DrsBind(drsContext, puuidClientDsa, exts);
            }
            catch (Exception e)
            {
                //when exception in IDL_DRSBind, RPC handle should be closed
                EnvironmentConfig.DrsContextStore.Remove(machine);
                testSite.Assert.Fail("IDL_DRSBind throws an exception: " + e.Message);
            }

            testSite.Log.Add(LogEntryKind.Checkpoint, "End call IDL_DRSBind with return value " + ret);

            if (EnvironmentConfig.ExpectSuccess)
                verifier.VerifyDRSBind(machine, user, drsContext.DRSHandle, ret, drsContext.ServerExtensions.Value);

            #endregion


            return ret;
        }

        /// <summary>
        /// Call IDL_DRSUnbind to release specified DRS handle
        /// </summary>
        /// <param name="machine">AD DS or LDS instance on specified SUT</param>
        /// <returns>0 if IDL_DRSBind success, otherwise, failed</returns>
        [SupportedADType(ADInstanceType.Both)]
        public uint DrsUnbind(EnvironmentConfig.Machine machine, bool remove = true)
        {
            testSite.Log.Add(LogEntryKind.Checkpoint, "Begin to call IDL_DRSUnbind");
            uint ret = 0;
            testSite.Assert.IsTrue(EnvironmentConfig.DrsContextStore.ContainsKey(machine), "Load context information for " + machine.ToString());

            ret = drsClient.DrsUnbind(EnvironmentConfig.DrsContextStore[machine]);
            testSite.Log.Add(LogEntryKind.Checkpoint, "End call IDL_DRSUnbind with return value " + ret);
            if (EnvironmentConfig.ExpectSuccess)
            {
                verifier.VerifyDRSUnbind(ret);

            }
            if (remove && ret == 0)
                EnvironmentConfig.DrsContextStore.Remove(machine);
            return ret;
        }

        #region Replication Methods
        /// <summary>
        /// Calling DrsGetReplInfo method to replicate updates from an NC replica on the server, and verify the server response.
        /// </summary>
        /// <param name="server">the bind DC response to the request</param>
        /// <param name="reqVersion">The version of the request.</param>
        /// <param name="neighborDC">The replication neighbor of the called DC server. It can be none.</param>
        /// <param name="infoType">The operation type code.</param>
        /// <param name="objectDN">DN of the object on which the operation should be performed. 
        /// The meaning of this parameter depends on the value of the infoType parameter.</param>
        /// <param name="attrName">Null, or the lDAPDisplayName of a link attribute</param>
        /// <param name="attrValueDN">Null, or the DN of the link value for which to retrieve a stamp.</param>
        /// <param name="outVersion">A pointer to the version of the response message.</param>
        /// <param name="outMessage"> A pointer to the response message.</param>
        /// <returns>The return code of server.</returns>
        public uint DrsGetReplInfo(
             EnvironmentConfig.Machine svr,
             DrsGetReplInfo_Versions reqVer,
             EnvironmentConfig.Machine neighborDC,
             DS_REPL_INFO infoType,
             string objectDN,
             string attrName,
             string attrValueDN,
             out uint? outVersion,
             out DRS_MSG_GETREPLINFO_REPLY? outMessage)
        {
            //return value
            uint ret = 0;

            //get context session
            testSite.Assert.IsTrue(EnvironmentConfig.MachineStore.ContainsKey(svr), "Load machine information for " + svr.ToString());
            DrsrClientSessionContext drsContext = null;
            EnvironmentConfig.DrsContextStore.TryGetValue(svr, out drsContext);

            //get source Dsa Obj Guid
            Guid? sourceDsaObjGuid;
            if (neighborDC == EnvironmentConfig.Machine.None)
            {
                sourceDsaObjGuid = System.Guid.Empty;
            }
            else
            {
                sourceDsaObjGuid = ((DsServer)EnvironmentConfig.MachineStore[neighborDC]).NtdsDsaObjectGuid;
            }


            //create request message
            DRS_MSG_GETREPLINFO_REQ? inMessage = null;
            if (reqVer == DrsGetReplInfo_Versions.V1)
            {
                inMessage = drsClient.CreateGetReplInfoV1Request(infoType, objectDN, sourceDsaObjGuid.Value);
            }
            else if (reqVer == DrsGetReplInfo_Versions.V2)
            {
                inMessage = drsClient.CreateGetReplInfoV2Request(infoType, objectDN, sourceDsaObjGuid.Value, DRS_MSG_GETREPLINFO_REQ_FLAGS.NONE, attrName, attrValueDN, 0);
            }
            else
            {
                testSite.Assert.Fail("not support the message version {0}", reqVer);
            }

            //Send/Response
            ret = drsClient.DrsGetReplInfo(drsContext, (uint)reqVer, inMessage, out outVersion, out outMessage);

            if (EnvironmentConfig.ExpectSuccess)
                verifier.VerifyDrsGetReplInfo((uint)reqVer, inMessage, outVersion, outMessage);

            return ret;
        }

        /// <summary>
        /// To create a request message of DRS_MSG_REPSYNC.
        /// </summary>
        /// <param name="pNC">The DSName of the specified NC.</param>
        /// <param name="reqVer">The version of request</param>
        /// <param name="sourceDsaGuid">The source DSA GUID.</param>
        /// <param name="sourceDsaName">The transport-specific NetworkAddress of the source DC.</param>
        /// <param name="options">The DRS_OPTIONS flags.</param>
        /// <returns> the created RPC input parameter.</returns>
        public DRS_MSG_REPSYNC CreateReplicaSyncRequest(
            DrsReplicaSync_Versions reqVer,
            DSNAME? pNC,
            Guid sourceDsaGuid,
            string sourceDsaName,
            DRS_OPTIONS options)
        {
            DRS_MSG_REPSYNC request = new DRS_MSG_REPSYNC();
            switch (reqVer)
            {
                case DrsReplicaSync_Versions.V1:
                    request.V1 = drsClient.CreateReplicaSyncRequestV1(
                        pNC,
                        sourceDsaGuid,
                        sourceDsaName,
                        options);
                    break;
                case DrsReplicaSync_Versions.V2:
                    request.V1 = drsClient.CreateReplicaSyncRequestV1(
                        pNC,
                        sourceDsaGuid,
                        sourceDsaName,
                        options);

                    request.V2 = drsClient.CreateReplicaSyncRequestV2(request.V1);
                    break;
                default:
                    testSite.Assert.Fail("The version {0} is not supported.", reqVer);
                    break;
            }
            return request;
        }

        /// <summary>
        /// Calling  IDL_DRSReplicaSync method to trigger replication from another DC, and verify the server response.
        /// </summary>
        /// <param name="svr">the bind DC response to the request</param>
        /// <param name="reqVer">The version of request.</param>
        /// <param name="sourceDC">The source DC where to replicated from. None if replicate from all partners.</param>
        /// <param name="options">The operation options.</param>
        /// <param name="bUseDsaGuid">True if use the DSA GUID to specify the source DC, false if use NetworkAddress.</param>
        /// <param name="ncType">The naming context type.</param>
        /// <returns>The return code of server.</returns>
        public uint DrsReplicaSync(
            EnvironmentConfig.Machine svr,
            DrsReplicaSync_Versions reqVer,
            EnvironmentConfig.Machine sourceDC,
            DRS_OPTIONS options,
            bool bUseDsaGuid,
            NamingContext ncType)
        {
            //return value
            uint ret = 0;

            DRS_OPTIONS appendFlags = DRS_OPTIONS.DRS_SYNC_FORCED;

            //get context session
            testSite.Assert.IsTrue(EnvironmentConfig.MachineStore.ContainsKey(svr), "IDL_DRSReplicaSync: Load machine information for " + svr.ToString());
            DrsrClientSessionContext drsContext = null;
            EnvironmentConfig.DrsContextStore.TryGetValue(svr, out drsContext);

            DsServer server = (DsServer)EnvironmentConfig.MachineStore[svr];
            DsServer srcServer = (DsServer)EnvironmentConfig.MachineStore[sourceDC];

            DSNAME pNC = DrsrHelper.GetNamingContextDSName(server.Domain, ncType);

            options |= appendFlags;

            DRS_MSG_REPSYNC? inMessage = null;
            if (options.HasFlag(DRS_OPTIONS.DRS_SYNC_ALL))
            {
                inMessage = CreateReplicaSyncRequest(
                        reqVer,
                        pNC,
                        srcServer.NtdsDsaObjectGuid, //Potential TDI, the Dsa Object Guid should be filled when DRS_SYNC_ALL is set.
                        null,
                        options);
            }
            else
            {

                if (bUseDsaGuid)
                {
                    inMessage = CreateReplicaSyncRequest(
                        reqVer,
                        pNC,
                        srcServer.NtdsDsaObjectGuid,
                        null,
                        options);
                }
                else
                {
                    options |= DRS_OPTIONS.DRS_SYNC_BYNAME;
                    inMessage = CreateReplicaSyncRequest(
                        reqVer,
                        pNC,
                        Guid.Empty,
                        srcServer.DsaNetworkAddress,
                        options);
                }
            }

            //Send/Response
            ret = drsClient.DrsReplicaSync(drsContext, (uint)reqVer, inMessage.Value);


            //    if(EnvironmentConfig.ExpectSuccess)

            return ret;

        }


        public uint DrsGetNCChangesV2(
            EnvironmentConfig.Machine sourceEnum,
            DsServer source,
            DsServer dest,
            string objectDn,
            USN_VECTOR usnvecFrom,
            UPTODATE_VECTOR_V1_EXT utdvecDest,
            bool withSecrets,
            out uint? outVersion,
            out DRS_MSG_GETCHGREPLY? outMessage)
        {
            // more like ReplSingleObjRequestMsg
            // but with customizable USN and UTD
            DRS_MSG_GETCHGREQ_V10 msgIn = new DRS_MSG_GETCHGREQ_V10();
            DRS_MSG_GETCHGREQ msgRequest = new DRS_MSG_GETCHGREQ();

            msgIn.ulFlags = (uint)DRS_OPTIONS.DRS_SPECIAL_SECRET_PROCESSING;
            if (withSecrets) 
            {
                msgIn.ulExtendedOp = (uint)EXOP_REQ_Codes.EXOP_REPL_SECRETS;
                msgIn.ulFlags = 0;
            }
            else
            {
                msgIn.ulExtendedOp = (uint)EXOP_REQ_Codes.EXOP_REPL_OBJ;
            }

            msgIn.ulMoreFlags = 0;
            msgIn.cMaxObjects = 10;
            msgIn.cMaxBytes = 1024 * 10;
            msgIn.uuidDsaObjDest = source.ServerObjectGuid;
            msgIn.uuidInvocIdSrc = dest.NtdsDsaObjectGuid;

            string ncName = LdapUtility.GetDnFromNcType(source, NamingContext.DomainNC);
            msgIn.pNC = (ldapAd.GetDsName(source, objectDn)).Value;

            msgIn.liFsmoInfo = new ULARGE_INTEGER();
            msgIn.liFsmoInfo.QuadPart = 0;

            msgIn.pUpToDateVecDest = new UPTODATE_VECTOR_V1_EXT[1];
            msgIn.pUpToDateVecDest[0] = utdvecDest;

            msgIn.pPartialAttrSetEx = null;

            SCHEMA_PREFIX_TABLE prefixTable = OIDUtility.CreatePrefixTable();

            // create PAS if needed
            // get the abstract PAS from LDAP (nc!partialAttributeSet)

            byte[] pasData = ldapAd.GetAttributeValueInBytes(dest, ncName, "partialAttributeSet");

            uint[] cPAS = null;

            if (pasData != null)
            {
                uint cAttrs = BitConverter.ToUInt32(pasData, 8);
                cPAS = new uint[cAttrs];

                for (int i = 0; i < cAttrs; ++i)
                {
                    cPAS[i] = BitConverter.ToUInt32(pasData, 12 + i * 4);
                }
            }

            msgIn.pPartialAttrSet = DrsuapiClient.CreatePARTIAL_ATTR_VECTOR_V1_EXT(cPAS);
            msgIn.PrefixTableDest = prefixTable;

            msgRequest.V10 = msgIn;
            DrsrClientSessionContext drsContext = null;
            EnvironmentConfig.DrsContextStore.TryGetValue(sourceEnum, out drsContext);
            uint ret = drsClient.DrsGetNcChanges(drsContext, 10, msgRequest, out outVersion, out outMessage);

            return ret;
        }


        /// <summary>
        /// Calling IDL_DRSGetNCChanges method to replicate updates from an NC replica on the server, and verify the server response.
        /// </summary>
        ///  <param name="sourceDcType">the bind DC response to the request</param>   
        /// <param name="reqVer">The version of the request.</param>
        /// <param name="destDCType">Dsa Obj Dest DC to getting the changes</param>
        /// <param name="options">The operation options.</param>
        /// <param name="ncType">The naming context type.</param>
        /// <param name="exopCode">The code of extended operation.</param>
        /// <param name="fsmoRole">The fsmo role to perform the extended operation.</param>
        /// <param name="objectDN">Null, or the DN of an object. It is required when exopCode is either EXOP_REPL_OBJ or EXOP_REPL_SECRETS.</param>
        /// <param name="outVersion">A pointer to the version of the response message.</param>
        /// <param name="outMessage"> A pointer to the response message.</param>
        /// <returns>The return code of server.</returns>
        public uint DrsGetNCChanges(
            EnvironmentConfig.Machine sourceDcType,
            DrsGetNCChanges_Versions reqVer,
            EnvironmentConfig.Machine destDCType,
            DRS_OPTIONS options,
            NamingContext ncType,
            EXOP_REQ_Codes exopCode,
            FSMORoles fsmoRole,
            string objectDN,
            out uint? outVersion,
            out DRS_MSG_GETCHGREPLY? outMessage)
        {
            //return value
            uint ret = 0;

            DRS_OPTIONS userOptions = options;
            ulong QuadPart = 0;

            //get context session
            DrsrClientSessionContext drsContext = null;
            EnvironmentConfig.DrsContextStore.TryGetValue(sourceDcType, out drsContext);
            DsServer sourceServer = (DsServer)EnvironmentConfig.MachineStore[sourceDcType];
            DsServer destDc = (DsServer)EnvironmentConfig.MachineStore[destDCType];

            REPS_FROM[] repsFroms = ldapAd.GetRepsFrom(destDc, ncType);
            USN_VECTOR usnFrom = new USN_VECTOR();
            foreach (REPS_FROM rf in repsFroms)
            {
                if (rf.uuidInvocId == sourceServer.InvocationId)
                {
                    usnFrom = rf.usnVec;
                    options = (DRS_OPTIONS)rf.ulReplicaFlags;
                    break;
                }
            }

            if (!userOptions.HasFlag(DRS_OPTIONS.DRS_USE_COMPRESSION))
            {
                //Remove compression flag.
                options &= ~DRS_OPTIONS.DRS_USE_COMPRESSION;
            }
            else
            {
                options |= DRS_OPTIONS.DRS_USE_COMPRESSION;
            }

            // FIXME: for some reason the repsFroms attribute will set ulFlags 
            // to DRS_SPECIAL_SECRET_PROCESSING, this is not what we want 
            // since when setting EXOP_REPL_SECRETS we're expecting secrets 
            // to be replicated, but setting DRS_SPECIAL_SECRET_PROCESSING 
            // will avoid the data being transfered.
            if (exopCode.HasFlag(EXOP_REQ_Codes.EXOP_REPL_SECRETS)
                && options.HasFlag(DRS_OPTIONS.DRS_SPECIAL_SECRET_PROCESSING))
            {
                options -= DRS_OPTIONS.DRS_SPECIAL_SECRET_PROCESSING;
            }

            //Construct USN Vector            
            UPTODATE_VECTOR_V1_EXT utdVector = ldapAd.GetReplUTD(destDc, ncType);


            DSNAME pNC;
            string dn = null;
            if (exopCode == EXOP_REQ_Codes.EXOP_REPL_OBJ || exopCode == EXOP_REQ_Codes.EXOP_REPL_SECRETS)
            {
                //dn = LdapUtility.GetDnFromNcType(sourceServer, ncType);
                //pNC = ldapAd.GetDsName(sourceServer, dn).Value;
                pNC = (ldapAd.GetDsName(sourceServer, objectDN)).Value;
            }

            else if (exopCode == EXOP_REQ_Codes.EXOP_FSMO_REQ_ROLE || exopCode == EXOP_REQ_Codes.EXOP_FSMO_REQ_PDC
                    || exopCode == EXOP_REQ_Codes.EXOP_FSMO_ABANDON_ROLE || exopCode == EXOP_REQ_Codes.EXOP_FSMO_REQ_RID_ALLOC
                    || exopCode == EXOP_REQ_Codes.EXOP_FSMO_RID_REQ_ROLE)
            {

                dn = DrsrHelper.GetFSMORoleCN(sourceServer.Domain, fsmoRole);
                pNC = (ldapAd.GetDsName(sourceServer, dn)).Value;
                if (exopCode == EXOP_REQ_Codes.EXOP_FSMO_REQ_RID_ALLOC)
                {
                    QuadPart = ldapAd.GetRidAllocationPoolFromDSA(destDc, destDc.ServerObjectName);
                }

            }

            else
            {
                pNC = DrsrHelper.GetNamingContextDSName(sourceServer.Domain, ncType);
            }


            //create request message
            DRS_MSG_GETCHGREQ? inMessage = null;
            DRS_MSG_GETCHGREQ_V5 V5;
            DRS_MSG_GETCHGREQ_V8 V8;
            inMessage = drsClient.CreateDRSGetNCChangesRequestV5(
                        ((DsServer)EnvironmentConfig.MachineStore[destDCType]).NtdsDsaObjectGuid,
                        sourceServer.InvocationId,
                        pNC,
                        usnFrom,
                        utdVector,
                        (uint)options,
                        535,
                        5357731,
                        (uint)exopCode,
                        QuadPart);

            SCHEMA_PREFIX_TABLE perfixTable = OIDUtility.CreatePrefixTable();

            // create PAS if needed
            // get the abstract PAS from LDAP (nc!partialAttributeSet)

            string nc = LdapUtility.GetDnFromNcType(destDc, ncType);
            byte[] pasData = ldapAd.GetAttributeValueInBytes(destDc, nc, "partialAttributeSet");

            uint[] cPAS = null;

            if (pasData != null)
            {
                uint cAttrs = BitConverter.ToUInt32(pasData, 8);
                cPAS = new uint[cAttrs];

                for (int i = 0; i < cAttrs; ++i)
                {
                    cPAS[i] = BitConverter.ToUInt32(pasData, 12 + i * 4);
                }
            }

            switch (reqVer)
            {
                case DrsGetNCChanges_Versions.V5:
                    break;
                case DrsGetNCChanges_Versions.V8:
                    V5 = inMessage.Value.V5;
                    inMessage = drsClient.CreateDRSGetNCChangesRequestV8(V5, cPAS, null, perfixTable);
                    break;
                case DrsGetNCChanges_Versions.V10:
                    V5 = inMessage.Value.V5;
                    V8 = drsClient.CreateDRSGetNCChangesRequestV8(V5, cPAS, null, perfixTable).V8;
                    inMessage = drsClient.CreateDRSGetNCChangesRequestV10(V8, 0);
                    break;
                case DrsGetNCChanges_Versions.V11:
                    V5 = inMessage.Value.V5;
                    V8 = drsClient.CreateDRSGetNCChangesRequestV8(V5, cPAS, null, perfixTable).V8;
                    DRS_MSG_GETCHGREQ_V10 V10 = drsClient.CreateDRSGetNCChangesRequestV10(V8, 0).V10;
                    inMessage = drsClient.CreateDRSGetNCChangesRequestV11(V10, 0);
                    break;
                default:
                    testSite.Assert.Fail("the version {0} is not supported", reqVer);
                    break;
            }

            //Send/Response
            ret = drsClient.DrsGetNcChanges(drsContext, (uint)reqVer, inMessage, out outVersion, out outMessage);

            //verification
            if (EnvironmentConfig.ExpectSuccess)
                verifier.VerifyDrsGetNCChanges(ret, (uint)reqVer, inMessage, outVersion, outMessage);

            return ret;
        }

        /// <summary>
        /// To create a request message of DRS_MSG_UPDREFS.
        /// </summary>
        /// <param name="ncReplicaDistinguishedName">the object's distinguishedName attribute of the root of an NC
        /// replica on the server.</param>
        /// <param name="reqVer">The version of the request.</param>
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
        //[CLSCompliant(false)]
        public DRS_MSG_UPDREFS CreateUpdateRefsRequest(
            DrsUpdateRefs_Versions reqVer,
            string ncReplicaDistinguishedName,
            Guid ncReplicaObjectGuid,
            string ncReplicaObjectSid,
            string destDsaName,
            Guid destDsaGuid,
            DRS_OPTIONS options)
        {
            DRS_MSG_UPDREFS request = new DRS_MSG_UPDREFS();
            switch (reqVer)
            {
                case DrsUpdateRefs_Versions.V1:
                    request.V1 = drsClient.CreateUpdateRefsRequestV1(
                        ncReplicaDistinguishedName,
                        ncReplicaObjectGuid,
                        ncReplicaObjectSid,
                        destDsaName,
                        destDsaGuid,
                        options
                    );
                    break;
                case DrsUpdateRefs_Versions.V2:
                    request.V1 = drsClient.CreateUpdateRefsRequestV1(
                        ncReplicaDistinguishedName,
                        ncReplicaObjectGuid,
                        ncReplicaObjectSid,
                        destDsaName,
                        destDsaGuid,
                        options
                    );
                    request.V2 = drsClient.CreateUpdateRefsRequestV2(request.V1);
                    break;
                default:
                    testSite.Assert.Fail("The version {0} is not supported.", reqVer);
                    break;
            }
            return request;
        }

        /// <summary>
        /// Calling IDL_DRSReplicaVerifyObjects method to verify the existence of objects in an NC replica by comparing against a replica of the same NC on a reference DC.
        /// </summary>
        ///  <param name="svr">the bind DC response to the request</param>
        /// <param name="reqVer">The version of the request.</param>
        /// <param name="referenceDC">The reference DC.</param>
        /// <param name="options">The operation options.</param>
        /// <param name="ncType">The naming context type.</param>
        /// <returns></returns>
        public uint DrsReplicaVerifyObjects(
            EnvironmentConfig.Machine svr,
            DrsReplicaVerifyObjects_Versions reqVer,
            EnvironmentConfig.Machine referenceDC,
            DRS_MSG_REPVERIFYOBJ_OPTIONS options,
            NamingContext ncType)
        {
            //return value
            uint ret = 0;

            //get context session
            testSite.Assert.IsTrue(EnvironmentConfig.MachineStore.ContainsKey(svr), "Load machine information for " + svr.ToString());
            DrsrClientSessionContext drsContext = null;
            EnvironmentConfig.DrsContextStore.TryGetValue(svr, out drsContext);

            DsServer server = (DsServer)EnvironmentConfig.MachineStore[svr];
            DsServer refServer = (DsServer)EnvironmentConfig.MachineStore[referenceDC];

            DSNAME pNC = DrsrHelper.GetNamingContextDSName(server.Domain, ncType);

            //create request message
            DRS_MSG_REPVERIFYOBJ? inMessage = null;

            inMessage = drsClient.CreateReplicaVerifyObjectsRequest(pNC, refServer.NtdsDsaObjectGuid, options);

            //Send/Response
            ret = drsClient.DrsReplicaVerifyObjects(drsContext, (uint)reqVer, inMessage);

            //                if(EnvironmentConfig.ExpectSuccess)

            return ret;
        }

        /// <summary>
        /// Calling IDL_DRSGetObjectExistence to check the consistency of object existence between its replica of an NC and the server's replica of the same NC. 
        /// </summary>
        ///  <param name="svr">the bind DC response to the request</param>
        /// <param name="reqVer">The version of request.</param>
        /// <param name="utdFilter">The filter excluding objects from the client's object sequence.</param>
        /// <param name="clientGuidSequence">The GUID sequence on caller DC.</param>
        /// <param name="flag">Flag that indicates how to set the MD5Digest.</param>
        /// <param name="flag">The naming context type.</param>
        /// <param name="outVersion">A pointer to the version of the response message.</param>
        /// <param name="outMessage"> A pointer to the response message.</param>
        /// <returns>The return code of server.</returns>
        public uint DrsGetObjectExistence(
            EnvironmentConfig.Machine svr,
            DrsGetObjectExistence_Versions reqVer,
            UPTODATE_VECTOR_V1_EXT utdFilter,
            Md5Digest_Flags flag,
            Guid[] clientGuidSequence,
            NamingContext ncType,
            out uint? outVersion,
            out DRS_MSG_EXISTREPLY? outMessage)
        {
            //return value
            uint ret = 0;

            //get context session
            testSite.Assert.IsTrue(EnvironmentConfig.MachineStore.ContainsKey(svr), "Load machine information for " + svr.ToString());
            DrsrClientSessionContext drsContext = null;
            EnvironmentConfig.DrsContextStore.TryGetValue(svr, out drsContext);

            DsServer server = (DsServer)EnvironmentConfig.MachineStore[svr];
            DSNAME pNC = DrsrHelper.GetNamingContextDSName(server.Domain, ncType);

            byte[] Md5Digest = null;



            if (flag == Md5Digest_Flags.InconsistentDigest)
            {
                //Construct a bad GUID sequnce with the same start GUID.
                Guid[] incorrectGuidSeq = new Guid[clientGuidSequence.Length];
                incorrectGuidSeq[0] = clientGuidSequence[0];
                for (int i = 1; i < incorrectGuidSeq.Length; i++)
                {
                    incorrectGuidSeq[i] = Guid.NewGuid();
                }
                Md5Digest = DrsrHelper.ComputeMD5Digest(incorrectGuidSeq);
            }
            else
            {
                Md5Digest = DrsrHelper.ComputeMD5Digest(clientGuidSequence);
            }



            //create request message
            DRS_MSG_EXISTREQ? inMessage = null;
            inMessage = drsClient.CreateDrsGetObjectExistenceRequest(clientGuidSequence[0], (uint)clientGuidSequence.Length, pNC, utdFilter, Md5Digest);


            //Send/Response
            ret = drsClient.DrsGetObjectExistence(drsContext, (uint)reqVer, inMessage, out outVersion, out outMessage);
            if (EnvironmentConfig.ExpectSuccess)
                verifier.VerifyDrsGetObjectExistence((uint)reqVer, inMessage, outVersion, outMessage);

            return ret;
        }

        #endregion


        /// <summary>
        /// call IDL_DRSDomainControllerInfo to get DC info by requested
        /// </summary>
        /// <param name="machine">AD DS or LDS DC</param>
        /// <param name="reqVer">request version</param>
        /// <param name="name">valid FQDN or Netbios name or invalided one</param>
        /// <param name="infoLv">InfoLevel of response</param>
        /// <returns>result code</returns>
        [SupportedADType(ADInstanceType.DS)]
        public uint DrsDomainControllerInfo(EnvironmentConfig.Machine machine, DRS_MSG_DCINFOREQ_Versions reqVer, string name, DRS_MSG_DCINFOREQ_INFOLEVEL infoLv)
        {
            DRS_MSG_DCINFOREQ? req = drsClient.CreateDomainControllerInfoRequest(name, infoLv);
            uint? outVer = null;
            DRS_MSG_DCINFOREPLY? reply = null;
            testSite.Log.Add(LogEntryKind.Checkpoint, "IDL_DRSDomainControllerInfo: Begin to call IDL_DRSDomainControllerInfo");
            uint ret = drsClient.DrsDomainControllerInfo(
                EnvironmentConfig.DrsContextStore[machine],
                (uint)reqVer,
                req,
                out outVer,
                out reply);
            testSite.Log.Add(LogEntryKind.Checkpoint, "IDL_DRSDomainControllerInfo: End call IDL_DRSDomainControllerInfo with return value " + ret);

            if (EnvironmentConfig.ExpectSuccess)
                verifier.VerifyDRSDomainControllerInfo(ret, machine, outVer, req, reply);

            return ret;
        }

        /// <summary>
        /// call IDL_DRSQuerySitesByCost to get site replication cost
        /// </summary>
        /// <param name="machine">AD DS or LDS DC</param>
        /// <param name="reqVer">request version</param>
        /// <param name="from">"from" this site</param>
        /// <param name="to">"to" these sites</param>
        /// <param name="cToSitesCorrect">if true, cToSItes will be number of "to" items, otherwise, will change it to a different value than that number</param>
        /// <returns>result code</returns>
        [SupportedADType(ADInstanceType.DS)]
        public uint DrsQuerySitesByCost(EnvironmentConfig.Machine machine, DRS_MSG_QUERYSITESREQ_Versions reqVer, DsSite from, DsSite[] to, bool cToSitesCorrect = true)
        {
            string[] sites = new string[to.Length];
            for (int i = 0; i < to.Length; i++)
            {
                sites[i] = to[i].CN;
            }
            DRS_MSG_QUERYSITESREQ? req = drsClient.CreateQuerySitesByCostRequest(from.CN, sites);
            uint? outVer;
            DRS_MSG_QUERYSITESREPLY? reply = null;
            testSite.Log.Add(LogEntryKind.Checkpoint, "IDL_DRSInterDomainMove: Begin to call IDL_DRSQuerySitesByCost");
            uint ret = drsClient.DrsQuerySitesByCost(EnvironmentConfig.DrsContextStore[machine],
                (uint)reqVer,
                req,
                out outVer,
                out reply);

            testSite.Log.Add(LogEntryKind.Checkpoint, "IDL_DRSInterDomainMove: End call IDL_DRSQuerySitesByCost with return value " + ret);

            if (EnvironmentConfig.ExpectSuccess)
                verifier.VerifyDRSQuerySitesByCost(ret, machine, outVer, from, to, reply);

            return ret;
        }

        #region KCC
        /// <summary>
        /// IDL_DRSExecuteKCC
        /// </summary>
        /// <param name="machine">SUT</param>
        /// <param name="isAsync">async or sync</param>
        /// <returns>return value</returns>
        public uint DrsExecuteKCC(EnvironmentConfig.Machine machine, bool isAsync)
        {
            DRS_MSG_KCC_EXECUTE? req = drsClient.CreateExecuteKccRequest(isAsync == true ? DRS_MSG_KCC_EXECUTE_FLAGS.DS_KCC_FLAG_DAMPED | DRS_MSG_KCC_EXECUTE_FLAGS.DS_KCC_FLAG_ASYNC_OP : DRS_MSG_KCC_EXECUTE_FLAGS.DS_KCC_FLAG_DAMPED);
            testSite.Log.Add(LogEntryKind.Checkpoint, "IDL_DRSExecuteKCC: Begin to call IDL_DRSExecuteKCC");
            uint ret = drsClient.DrsExecuteKcc(EnvironmentConfig.DrsContextStore[machine], 1, req);

            testSite.Log.Add(LogEntryKind.Checkpoint, "IDL_DRSExecuteKCC: End call IDL_DRSExecuteKCC with return value " + ret);

            if (EnvironmentConfig.ExpectSuccess)
                verifier.VerifyDRSExecuteKCC(ret);

            return ret;
        }

        public DRS_MSG_REPADD CreateReplicaAddReq(EnvironmentConfig.Machine machine, DRS_MSG_REPADD_Versions inVer, DsServer src, DRS_OPTIONS options, NamingContext nc)
        {
            DSNAME n = getNCFromType(src.Domain, nc);
            DRS_MSG_REPADD req = new DRS_MSG_REPADD();
            REPLTIMES s = new REPLTIMES();
            string sid = convertSidToString(n.Sid);
            s.rgTimes = new byte[84];
            switch (inVer)
            {
                case DRS_MSG_REPADD_Versions.V1:
                    req = drsClient.CreateReplicaAddV1Request(LdapUtility.ConvertUshortArrayToString(n.StringName),
                        n.Guid,
                        sid,
                        src.DsaNetworkAddress,
                        s, options);
                    break;
                case DRS_MSG_REPADD_Versions.V2:
                    req = drsClient.CreateReplicaAddV2Request(
                        LdapUtility.ConvertUshortArrayToString(n.StringName),
                        n.Guid,
                        sid,
                        src.NtdsDsaObjectName,
                        src.NtdsDsaObjectGuid,
                        null,
                        LdapUtility.ConvertUshortArrayToString(EnvironmentConfig.TransportObject.StringName).Replace("\0", ""),
                        EnvironmentConfig.TransportObject.Guid,
                        null,
                        src.DsaNetworkAddress,
                        s,
                        options);
                    break;
                case DRS_MSG_REPADD_Versions.V3:
                    req = drsClient.CreateReplicaAddV3Request(
                        LdapUtility.ConvertUshortArrayToString(n.StringName),
                        n.Guid,
                        sid,
                        src.NtdsDsaObjectName,
                        src.NtdsDsaObjectGuid,
                        null,
                        LdapUtility.ConvertUshortArrayToString(EnvironmentConfig.TransportObject.StringName).Replace("\0", ""),
                        EnvironmentConfig.TransportObject.Guid,
                        null,
                        src.DsaNetworkAddress,
                        s,
                        options);
                    break;
                default:
                    testSite.Assert.Fail("Invalid request version for DRS_MSG_REPADD: " + (uint)inVer);
                    break;
            }
            return req;
        }

        /// <summary>
        /// IDL_DRSReplicaAdd
        /// </summary>
        /// <param name="machine">SUT</param>
        /// <param name="inVer">request version</param>
        /// <param name="src">replication source</param>
        /// <param name="options">options</param>
        /// <returns>return value</returns>
        public uint DrsReplicaAdd(EnvironmentConfig.Machine machine, DRS_MSG_REPADD_Versions inVer, DsServer src, DRS_OPTIONS options, NamingContext nc)
        {
            DRS_MSG_REPADD? req = CreateReplicaAddReq(machine, inVer, src, options, nc);

            testSite.Log.Add(LogEntryKind.Checkpoint, "IDL_DRSReplicaAdd: Begin to call IDL_DRSReplicaAdd");
            uint ret = drsClient.DrsReplicaAdd(EnvironmentConfig.DrsContextStore[machine], (uint)inVer, req);

            testSite.Log.Add(LogEntryKind.Checkpoint, "IDL_DRSReplicaAdd: End call IDL_DRSReplicaAdd with return value" + ret);

            if (EnvironmentConfig.ExpectSuccess)
                verifier.VerifyDRSReplicaAdd(ret, machine, src, options);

            return ret;
        }


        public DRS_MSG_REPDEL CreateReplicaDelReq(EnvironmentConfig.Machine machine, DsServer src, DRS_OPTIONS options, NamingContext nc)
        {
            string nc_name = null;
            Guid nc_guid = Guid.Empty;
            string nc_sid = null;
            DSNAME nc_obj = getNCFromType(src.Domain, nc);
            nc_name = LdapUtility.ConvertUshortArrayToString(nc_obj.StringName);
            nc_guid = nc_obj.Guid;
            nc_sid = convertSidToString(nc_obj.Sid);

            return drsClient.CreateReplicaDelRequest(nc_name,
                  nc_guid,
                  nc_sid,
                  src.DsaNetworkAddress,
                  options);
        }

        /// <summary>
        /// IDL_DRSReplicaDel
        /// </summary>
        /// <param name="machine">SUT</param>
        /// <param name="src">replication source</param>
        /// <param name="options">options</param>
        /// <returns>return value</returns>
        public uint DrsReplicaDel(EnvironmentConfig.Machine machine, DsServer src, DRS_OPTIONS options, NamingContext nc)
        {
            DRS_MSG_REPDEL? req = CreateReplicaDelReq(machine, src, options, nc);

            testSite.Log.Add(LogEntryKind.Checkpoint, "IDL_DRSReplicaDel: Begin to call IDL_DRSReplicaDel");
            uint ret = drsClient.DrsReplicaDel(EnvironmentConfig.DrsContextStore[machine], 1, req);

            testSite.Log.Add(LogEntryKind.Checkpoint, "IDL_DRSReplicaDel: End call IDL_DRSReplicaDel with return value " + ret);

            if (EnvironmentConfig.ExpectSuccess)
                verifier.VerifyDRSReplicaDel(ret, machine, src, options);

            return ret;
        }

        /// <summary>
        /// IDL_DRSReplicaModify
        /// </summary>
        /// <param name="machine">SUT</param>
        /// <param name="src">replication source</param>
        /// <param name="replicaFlags">replication flags</param>
        /// <param name="modifyFields">modified fields</param>
        /// <param name="options">options</param>
        /// <returns>return value</returns>
        public uint DrsReplicaModify(EnvironmentConfig.Machine machine, DsServer src, DRS_OPTIONS replicaFlags, DRS_MSG_REPMOD_FIELDS modifyFields, DRS_OPTIONS options)
        {
            REPLTIMES s = new REPLTIMES();
            s.rgTimes = new byte[84];
            for (int i = 0; i < s.rgTimes.Length; i++)
            {
                s.rgTimes[i] = 1;
            }

            string sid = convertSidToString(src.Domain.ConfigNC.Sid);
            DRS_MSG_REPMOD? req = drsClient.CreateReplicaModifyRequest(LdapUtility.ConvertUshortArrayToString(src.Domain.ConfigNC.StringName),
               src.Domain.ConfigNC.Guid,
               sid,
               src.NtdsDsaObjectGuid,
               src.DsaNetworkAddress,
               s,
               DRS_OPTIONS.DRS_WRIT_REP,
               modifyFields,
               options);

            testSite.Log.Add(LogEntryKind.Checkpoint, "IDL_DRSReplicaModify: Begin to call IDL_DRSReplicaModify");
            uint ret = drsClient.DrsReplicaModify(EnvironmentConfig.DrsContextStore[machine], 1, req);

            testSite.Log.Add(LogEntryKind.Checkpoint, "IDL_DRSReplicaModify: End call IDL_DRSReplicaModify with return value " + ret);

            if (EnvironmentConfig.ExpectSuccess)
                verifier.VerifyDRSReplicaModify(ret, machine, src, replicaFlags, modifyFields, options);

            return ret;
        }

        /// <summary>
        /// IDL_DRSUpdateRefs
        /// </summary>
        /// <param name="machine">SUT</param>
        /// <param name="dest">replication destination</param>
        /// <param name="options">options</param>
        /// <returns>return value</returns>
        public uint DrsUpdateRefs(
            EnvironmentConfig.Machine machine,
            DrsUpdateRefs_Versions reqVer,
            DsServer dest,
            DRS_OPTIONS options,
            NamingContext nc)
        {
            string nc_name = null;
            Guid nc_guid = Guid.Empty;
            string nc_sid = null;
            DSNAME nc_obj = getNCFromType(dest.Domain, nc);
            nc_name = LdapUtility.ConvertUshortArrayToString(nc_obj.StringName);
            nc_guid = nc_obj.Guid;
            nc_sid = convertSidToString(nc_obj.Sid);

            DRS_MSG_UPDREFS? req = CreateUpdateRefsRequest(
                reqVer,
                nc_name,
                nc_guid,
                nc_sid,
                dest.DsaNetworkAddress,
                dest.NtdsDsaObjectGuid,
                options);

            testSite.Log.Add(LogEntryKind.Checkpoint, "Begin to call IDL_DRSUpdateRefs");
            uint ret = drsClient.DrsUpdateRefs(EnvironmentConfig.DrsContextStore[machine], (uint)DrsUpdateRefs_Versions.V1, req);

            testSite.Log.Add(LogEntryKind.Checkpoint, "End call IDL_DRSUpdateRefs with return value " + ret);
            if (EnvironmentConfig.ExpectSuccess)
                verifier.VerifyDRSUpdateRefs(ret, machine, dest, options);

            return ret;
        }
        #endregion

        #region Administration Tool Support
        /// <summary>
        /// call DrsAddEntry to adds one or more objects
        /// </summary>
        /// <param name="svr">bind DC</param>
        /// <param name="reqVer">the version of the request</param>
        /// <param name="EntInfList">List contains ENTINF objects</param>
        /// <param name="pClientCreds">Client credentials</param>
        /// <param name="outVersion">the version of the reply</param>
        /// <param name="outMessage">the reply message</param>
        /// <returns>The return code of server.</returns>
        public uint DrsAddEntry(
            EnvironmentConfig.Machine svr,
            DRS_AddEntry_Versions reqVer,
            ENTINFLIST EntInfList,
            DRS_SecBufferDesc[] pClientCreds,
            out uint? outVersion,
            out DRS_MSG_ADDENTRYREPLY? outMessage)
        {
            //return value
            uint ret = 0;

            //get context session
            testSite.Assert.IsTrue(EnvironmentConfig.MachineStore.ContainsKey(svr), "Load machine information for " + svr.ToString());
            DrsrClientSessionContext drsContext = null;
            EnvironmentConfig.DrsContextStore.TryGetValue(svr, out drsContext);

            //create request message
            DRS_MSG_ADDENTRYREQ? inMessage = null;
            switch (reqVer)
            {
                case DRS_AddEntry_Versions.V2:
                    inMessage = drsClient.CreateDrsAddEntryV2(EntInfList);
                    break;
                case DRS_AddEntry_Versions.V3:
                    testSite.Assert.IsNotNull(pClientCreds[0], "pClientCreds cannot be null when request message is version V3");
                    inMessage = drsClient.CreateDrsAddEntryV3(EntInfList, pClientCreds[0].Buffers);
                    break;
                default:
                    // Verification: testSite.Assert.Fail("Only V2 and V3 version are support as Add Entry request message");
                    outVersion = null;
                    outMessage = null;
                    return 1;
            }

            //Send/Response
            ret = drsClient.DrsAddEntry(drsContext, (uint)reqVer, inMessage, out outVersion, out outMessage);

            if (EnvironmentConfig.ExpectSuccess)
                verifier.VerifyDRSAddEntry((uint)reqVer, inMessage, outVersion, outMessage);

            return ret;
        }

        /// <summary>
        /// call DrsAddSidHistory to adds one or more SIDs to the sIDHistory attribute of a given object
        /// </summary>
        /// <param name="svr">bind DC, use for DstDomain</param>
        /// <param name="reqVer">the version of the request</param>
        /// <param name="flag">DRS_ADDSID_FLAGS flag</param>
        /// <param name="srcDN">the setting of SrcPrincipal</param>
        /// <param name="desDN">the setting of desDN</param>
        /// <param name="pDC">setting for SrcDomainController</param>
        /// <param name="User">setting for SrcCredsUser and SrcCredsPassword</param>
        /// <returns>The return code of server.</returns>
        [SupportedADType(ADInstanceType.DS)]
        public uint DrsAddSidHistory(
            EnvironmentConfig.Machine svr,
            EnvironmentConfig.User cred,
            DomainEnum srcDomain,
            DomainEnum destDomain,
            IDL_DRSAddSidHistory_dwInVersion_Values reqVer,
            DRS_ADDSID_FLAGS flag,
            string srcDN,
            ObjectNameType srcNameType,
            string desDN,
            ObjectNameType destNameType,
            EnvironmentConfig.Machine srcDC,
            out IDL_DRSAddSidHistory_pdwOutVersion_Values? outVersion,
            out DRS_MSG_ADDSIDREPLY? outMessage
            )
        {
            //return value
            uint ret = 0;

            //get context session
            testSite.Assert.IsTrue(EnvironmentConfig.MachineStore.ContainsKey(svr), "Load machine information for " + svr.ToString());
            DrsrClientSessionContext drsContext = null;
            EnvironmentConfig.DrsContextStore.TryGetValue(svr, out drsContext);

            DsServer pDC = null;
            string pDCName = null;
            if (srcDC != EnvironmentConfig.Machine.None)
            {
                pDC = (DsServer)EnvironmentConfig.MachineStore[srcDC];
                pDCName = pDC.DnsHostName;
            }

            //create request message
            DRS_MSG_ADDSIDREQ? inMessage = null;

            string udomain = null;
            string uname = null;
            string upwd = null;
            string srcdomain_name = null;
            string destdomain_name = null;
            if (cred != EnvironmentConfig.User.None)
            {
                DsUser user = EnvironmentConfig.UserStore[cred];
                udomain = user.Domain.NetbiosName;
                uname = user.Username;
                upwd = user.Password;
            }
            if (srcDomain != DomainEnum.None)
                srcdomain_name = EnvironmentConfig.DomainStore[srcDomain].NetbiosName;

            if (destDomain != DomainEnum.None)
                destdomain_name = EnvironmentConfig.DomainStore[destDomain].NetbiosName;

            string srcSid = null;
            if (pDC != null)
                srcSid = LdapUtility.GetObjectStringSid(pDC, srcDN);

            if (srcNameType == ObjectNameType.CN || srcNameType == ObjectNameType.SAM)
                srcDN = LdapUtility.GetObjectSAMNameFromDN(pDC, srcDN);

            if (destNameType == ObjectNameType.CN || destNameType == ObjectNameType.SAM)
                desDN = LdapUtility.GetObjectSAMNameFromDN((DsServer)EnvironmentConfig.MachineStore[svr], desDN);

            inMessage = drsClient.CreateAddSidHistoryRequest(flag, srcdomain_name, srcDN, pDCName,
                                                             uname, udomain, upwd,
                                                             destdomain_name,
                                                             desDN);

            testSite.Log.Add(LogEntryKind.Checkpoint, "Start to call IDL_DRSAddSidHistory");
            //Send/Response
            ret = drsClient.DrsAddSidHistory(drsContext, reqVer, inMessage, out outVersion, out outMessage);
            testSite.Log.Add(LogEntryKind.Checkpoint, "End call IDL_DRSAddSidHistory with return value " + ret);

            if (EnvironmentConfig.ExpectSuccess)
                verifier.VerifyDRSAddSidHistory(ret, srcSid, srcDN, srcNameType, desDN, destNameType, srcDC, svr, reqVer, inMessage, outVersion, outMessage);

            return ret;
        }

        /// <summary>
        /// call DrsWriteSPN to updates the set of SPNs on an object
        /// </summary>
        /// <param name="svr">drsr bind machine</param>
        /// <param name="reqVer">the version of the request</param>
        /// <param name="operation">Operation to the spn</param>
        /// <param name="ncType">setting pwszAccount:  The DN of the object to modify</param>
        /// <param name="sPNList">sPN for the operation</param>
        /// <param name="outVersion">the version of the reply</param>
        /// <param name="outMessage">the reply message</param>
        /// <returns></returns>
        [SupportedADType(ADInstanceType.DS)]
        public uint DrsWriteSPN(
            EnvironmentConfig.Machine svr,
            dwInVersion_Values reqVer,
            DS_SPN_OPREATION operation,
            string dn,
            string[] sPNList,
            out pdwOutVersion_Values? outVersion,
            out DRS_MSG_SPNREPLY? outMessage
            )
        {
            //return value
            uint ret = 0;

            //get context session
            testSite.Assert.IsTrue(EnvironmentConfig.MachineStore.ContainsKey(svr), "Load machine information for " + svr.ToString());
            DrsrClientSessionContext drsContext = null;
            EnvironmentConfig.DrsContextStore.TryGetValue(svr, out drsContext);

            //create request message
            DRS_MSG_SPNREQ? inMessage = null;
            inMessage = drsClient.CreateWriteSpnRequest(operation, dn, sPNList);


            //Send/Response
            testSite.Log.Add(LogEntryKind.Checkpoint, "IDL_DRSWriteSPN: Start to call IDL_DRSWriteSPN");
            ret = drsClient.DrsWriteSpn(drsContext, reqVer, inMessage, out outVersion, out outMessage);
            testSite.Log.Add(LogEntryKind.Checkpoint, "IDL_DRSWriteSPN: End call IDL_DRSWriteSPN with return value " + ret);

            if (EnvironmentConfig.ExpectSuccess)
                verifier.VerifyDRSWriteSPN(reqVer, inMessage, outVersion, outMessage);

            return ret;
        }

        /// <summary>
        /// Call IDL_DRSRemoveDsServer to perform remove actions of DCs.
        /// </summary>
        /// <param name="svr">DRS-binded DC.</param>
        /// <param name="reqVer">Request version.</param>
        /// <param name="serverDn">The DN of the server object of the DC to remove.</param>
        /// <param name="domainDn">The DN of the NC root of the domain that the DC to be removed belongs to.</param>
        /// <param name="commit">True if the DC's metadata should be actually removed. False otherwise.</param>
        /// <returns></returns>
        [SupportedADType(ADInstanceType.DS)]
        public uint DrsRemoveDsServer(
            EnvironmentConfig.Machine svr,
            IDL_DRSRemoveDsServer_dwInVersion_Values reqVer,
            string serverDn,
            string domainDn,
            bool commit
            )
        {
            // Validate svr
            testSite.Assert.IsTrue(
                EnvironmentConfig.MachineStore.ContainsKey(svr),
                "IDL_DRSRemoveDsServer: Found machine " + svr.ToString()
                );

            // Get DRSR context
            DrsrClientSessionContext ctx = null;
            EnvironmentConfig.DrsContextStore.TryGetValue(svr, out ctx);

            // Create request message
            DRS_MSG_RMSVRREQ req = drsClient.CreateRemoveDsServerRequest(
                serverDn,
                domainDn,
                commit);

            // Actual DRSR call.
            IDL_DRSRemoveDsServer_pdwOutVersion_Values? outVersion;
            DRS_MSG_RMSVRREPLY? outMessage;
            uint ret = drsClient.DrsRemoveDsServer(
                ctx,
                IDL_DRSRemoveDsServer_dwInVersion_Values.V1,
                req,
                out outVersion,
                out outMessage);

            if (EnvironmentConfig.ExpectSuccess)
                // Verify the responses.
                verifier.VerifyDrsRemoveDsServer(
                    ret,
                    svr,
                    reqVer,
                    req,
                    outVersion,
                    outMessage);

            return ret;
        }

        /// <summary>
        /// Call IDL_DRSRemoveDsDomain to perform remove actions of domain.
        /// </summary>
        /// <param name="svr">DRS-binded DC.</param>
        /// <param name="reqVer">Request version.</param>
        /// <param name="domainDn">The DN of the domain to be removed.</param>
        /// <returns></returns>
        [SupportedADType(ADInstanceType.DS)]
        public uint DrsRemoveDsDomain(
            EnvironmentConfig.Machine svr,
            IDL_DRSRemoveDsDomain_dwInVersion_Values reqVer,
            string domainDn
            )
        {
            // Validate svr
            testSite.Assert.IsTrue(
                EnvironmentConfig.MachineStore.ContainsKey(svr),
                "IDL_DRSRemoveDsDomain: Found machine " + svr.ToString()
                );

            // Get DRSR context
            DrsrClientSessionContext ctx = null;
            EnvironmentConfig.DrsContextStore.TryGetValue(svr, out ctx);

            // Create request version
            DRS_MSG_RMDMNREQ req = drsClient.CreateRemoveDsDomainRequest(domainDn);

            // Actual DRSR call
            IDL_DRSRemoveDsDomain_pdwOutVersion_Values? outVersion;
            DRS_MSG_RMDMNREPLY? reply;
            uint ret = drsClient.DrsRemoveDsDomain(ctx, reqVer, req, out outVersion, out reply);

            if (EnvironmentConfig.ExpectSuccess)
                // Verify the responses
                verifier.VerifyDrsRemoveDsDomain(ret, svr, reqVer, req, outVersion, reply);

            return ret;
        }

        #endregion


        #region Lookups
        /// <summary>
        /// Wrapper for IDL_DRSGetMemberships.
        /// </summary>
        /// <param name="svr">DRSBinded machine.</param>
        /// <param name="reqVer">Request version.</param>
        /// <param name="name">The DSName of the object whose reverse membership is being requested.</param>
        /// <param name="isGetAttribute">Whether the attributes of the group membership is being queried.</param>
        /// <param name="operationType">Type of evaluation.</param>
        /// <param name="limitingDomain">Domain filter.</param>
        /// <returns>Return code.</returns>
        public uint DrsGetMemberships(
            EnvironmentConfig.Machine svr,
            dwInVersion_Values reqVer,
            DSNAME name,
            bool isGetAttribute,
            REVERSE_MEMBERSHIP_OPERATION_TYPE operationType,
            DSNAME? limitingDomain)
        {
            // Validate svr
            testSite.Assert.IsTrue(
                EnvironmentConfig.MachineStore.ContainsKey(svr),
                "IDL_DRSGetMemberships: Found machine " + svr.ToString()
                );

            // Get DRSR context
            DrsrClientSessionContext ctx = null;
            EnvironmentConfig.DrsContextStore.TryGetValue(svr, out ctx);

            // Create request message
            const uint DRS_REVMEMB_FLAG_GET_ATTRIBUTES = 0x00000001;
            DRS_MSG_REVMEMB_REQ req = drsClient.CreateGetMembershipsRequest(
                new DSNAME[] { name },  // Currently we only consider the single DSName scenario
                (isGetAttribute) ? DRS_REVMEMB_FLAG_GET_ATTRIBUTES : 0,
                operationType,
                limitingDomain
                );

            // Actuall DRSR call
            uint? outVersion;
            DRS_MSG_REVMEMB_REPLY? reply;

            uint ret = drsClient.DrsGetMemberships(ctx, (uint)reqVer, req, out outVersion, out reply);

            if (EnvironmentConfig.ExpectSuccess)
                verifier.VerifyDrsGetMemberships(svr, (uint)reqVer, req.V1, outVersion, reply.Value.V1);

            return ret;
        }

        /// <summary>
        /// Wrapper for IDL_DRSGetMemberships2.
        /// Note this wrapper creates a consecutive 7 identical requests.
        /// </summary>
        /// <param name="svr">DRSBinded machine.</param>
        /// <param name="reqVer">Request version.</param>
        /// <param name="name">The DSName of the object whose reverse membership is being requested.</param>
        /// <param name="isGetAttribute">Whether the attributes of the group membership is being queried.</param>
        /// <param name="operationType">Type of evaluation.</param>
        /// <param name="limitingDomain">Domain filter.</param>
        /// <returns>Return code.</returns>
        public uint DrsGetMemberships2(
            EnvironmentConfig.Machine svr,
            dwInVersion_Values reqVer,
            DSNAME name,
            bool isGetAttribute,
            REVERSE_MEMBERSHIP_OPERATION_TYPE operationType,
            DSNAME limitingDomain)
        {
            // Validate svr
            testSite.Assert.IsTrue(
                EnvironmentConfig.MachineStore.ContainsKey(svr),
                "IDL_DRSGetMemberships2: Found machine " + svr.ToString()
                );

            // Get DRSR context
            DrsrClientSessionContext ctx = null;
            EnvironmentConfig.DrsContextStore.TryGetValue(svr, out ctx);

            // Create request messgae
            // Create request message
            const uint DRS_REVMEMB_FLAG_GET_ATTRIBUTES = 0x00000001;
            DRS_MSG_REVMEMB_REQ req = drsClient.CreateGetMembershipsRequest(
                new DSNAME[] { name },  // Currently we only consider the single DSName scenario
                (isGetAttribute) ? DRS_REVMEMB_FLAG_GET_ATTRIBUTES : 0,
                operationType,
                limitingDomain
                );

            DRS_MSG_GETMEMBERSHIPS2_REQ req2 = drsClient.CreateGetMemberships2Request(
                new DRS_MSG_REVMEMB_REQ_V1[] {
                    req.V1,    // 1
                    req.V1,    // 2
                    req.V1,    // 3
                    req.V1,    // 4
                    req.V1,    // 5
                    req.V1,    // 6
                    req.V1     // 7
                }
                );

            // Actual DRSR call
            uint? outVersion;
            DRS_MSG_GETMEMBERSHIPS2_REPLY? reply;

            uint ret = drsClient.DrsGetMemberships2(ctx, 1, req2, out outVersion, out reply);

            if (EnvironmentConfig.ExpectSuccess)
                verifier.VerifyDrsGetMemberships2(svr, 1, req2, outVersion, reply);

            return ret;
        }

        /// <summary>
        /// Wrapper for IDL_DRSCrackNames.
        /// </summary>
        /// <param name="svr">DRSBinded machine.</param>
        /// <param name="flags">Flags for CrackNames operation.</param>
        /// <param name="formatOffered">Format of the given names.</param>
        /// <param name="formatDesired">Format desired on return.</param>
        /// <param name="names">Names to be cracked.</param>
        /// <returns>Return code.</returns>
        public uint DrsCrackNames(
            EnvironmentConfig.Machine svr,
            DRS_MSG_CRACKREQ_FLAGS flags,
            uint formatOffered,
            uint formatDesired,
            string[] names)
        {
            // Validate svr
            testSite.Assert.IsTrue(
                EnvironmentConfig.MachineStore.ContainsKey(svr),
                "IDL_DRSCrackNames: Found machine " + svr.ToString()
                );

            // Get DRSR context
            DrsrClientSessionContext ctx = null;
            EnvironmentConfig.DrsContextStore.TryGetValue(svr, out ctx);

            // Create request message
            DRS_MSG_CRACKREQ req = drsClient.CreateCrackNamesRequest(
                flags,
                formatOffered,
                formatDesired,
                names
                );

            // Actual DRSR call
            uint? outVersion;
            DRS_MSG_CRACKREPLY? reply;

            uint ret = drsClient.DrsCrackNames(
                ctx,
                1,
                req,
                out outVersion,
                out reply
                );

            if (EnvironmentConfig.ExpectSuccess)
                verifier.VerifyDrsCrackNames(svr, ret, 1, req, outVersion, reply);

            return ret;
        }

        /// <summary>
        /// Wrapper for IDL_DRSVerifyNames.
        /// </summary>
        /// <param name="svr">DRSBinded machine.</param>
        /// <param name="reqVer">Request version.</param>
        /// <param name="flags">Flags for VerifyNames operation.</param>
        /// <param name="names">DSNames of the objects to be verified.</param>
        /// <param name="objectDNs">DNs of the objects in names.</param>
        /// <param name="requiredAttrs">Attributes of the verified objects.</param>
        /// <param name="prefixTable">A prefix table used to translate ATTRTYPE to OID values.</param>
        /// <returns>Return code.</returns>
        public uint DrsVerifyNames(
            EnvironmentConfig.Machine svr,
            dwInVersion_Values reqVer,
            DRS_MSG_VERIFYREQ_V1_dwFlags_Values flags,
            DSNAME[] names,
            string[] objectDNs,
            ATTRBLOCK requiredAttrs,
            SCHEMA_PREFIX_TABLE prefixTable)
        {
            // Validate svr
            testSite.Assert.IsTrue(
                EnvironmentConfig.MachineStore.ContainsKey(svr),
                "IDL_DrsVerifyNames: Found machine " + svr.ToString()
                );

            // Get DRSR context
            DrsrClientSessionContext ctx = null;
            EnvironmentConfig.DrsContextStore.TryGetValue(svr, out ctx);

            // Create request message
            DRS_MSG_VERIFYREQ req = drsClient.CreateVerifyNamesRequest(
                flags,
                names,
                requiredAttrs,
                prefixTable
                );

            // Actuall DRSR call
            uint? outVersion;
            DRS_MSG_VERIFYREPLY? reply;

            uint ret = drsClient.DrsVerifyNames(
                ctx,
                (uint)reqVer,
                req,
                out outVersion,
                out reply
                );
            if (EnvironmentConfig.ExpectSuccess)
                verifier.VerifyDrsVerifyNames(svr, (uint)reqVer, req, outVersion, reply, objectDNs);

            return ret;
        }

        #endregion

        #region LDS Demotion
        /// <summary>
        /// init a lds demotion
        /// </summary>
        /// <param name="svr">target</param>
        /// <returns>return value</returns>
        [SupportedADType(ADInstanceType.LDS)]
        public uint DrsInitDemotion(EnvironmentConfig.Machine svr)
        {
            DRS_MSG_INIT_DEMOTIONREQ? req = drsClient.CreateInitDemotionRequest();
            uint? outver = null;
            DRS_MSG_INIT_DEMOTIONREPLY? reply = null;
            testSite.Log.Add(LogEntryKind.Checkpoint, "Begin to call IDL_DRSInitDemotion");
            uint ret = drsClient.DrsInitDemotion(EnvironmentConfig.DrsContextStore[svr], 1, req, out outver, out reply);

            testSite.Log.Add(LogEntryKind.Checkpoint, "End call IDL_DRSInitDemotion with return value " + ret);
            if (EnvironmentConfig.ExpectSuccess)
                verifier.VerifyDRSInitDemotion(ret);
            return ret;
        }

        /// <summary>
        /// finish lds demotion
        /// </summary>
        /// <param name="svr">target</param>
        /// <param name="ops">operations</param>
        /// <param name="verificationPartner">partner for verification</param>
        /// <returns>return value</returns>
        [SupportedADType(ADInstanceType.LDS)]
        public uint DrsFinishDemotion(EnvironmentConfig.Machine svr, DRS_MSG_FINISH_DEMOTIONREQ_OPERATIONS ops, EnvironmentConfig.Machine verificationPartner)
        {
            DRS_MSG_FINISH_DEMOTIONREQ? req = drsClient.CreateFinishDemotionRequest(ops, testSite.Properties["LDSDemotion.Scriptfolder"]);
            uint? outver = null;
            DRS_MSG_FINISH_DEMOTIONREPLY? reply = null;
            testSite.Log.Add(LogEntryKind.Checkpoint, "Begin to call IDL_DRSFinishDemotion");
            uint ret = drsClient.DrsFinishDemotion(EnvironmentConfig.DrsContextStore[svr], 1, req, out outver, out reply);

            testSite.Log.Add(LogEntryKind.Checkpoint, "End call IDL_DRSFinishDemotion with return value" + ret);

            if (EnvironmentConfig.ExpectSuccess)
                verifier.VerifyDRSFinishDemotion(ret, svr, ops, verificationPartner);
            return ret;
        }


        #endregion

        #region clone DC
        /// <summary>
        /// clone a dc
        /// </summary>
        /// <param name="svr">target dc</param>
        /// <param name="machinename">cloned dc name</param>
        /// <param name="sitename">site where cloned dc is</param>
        /// <returns>return value</returns>
        public uint DrsAddCloneDC(EnvironmentConfig.Machine svr, string machinename, string sitename)
        {
            DsServer sut = (DsServer)EnvironmentConfig.MachineStore[svr];
            testSite.Assert.IsTrue(sut.DCFunctional >= 5, "DC should be windows server 2012 or higher, the domainControllerFunction in rootDSE should be not less than 5");

            testSite.Log.Add(LogEntryKind.Checkpoint, "Begin to clone a DC from " + EnvironmentConfig.MachineStore[svr].DnsHostName + " with name " + machinename + " into site " + sitename);
            DRS_MSG_ADDCLONEDCREQ? req = drsClient.CreateAddCloneDCRequest(machinename, sitename);
            DRS_MSG_ADDCLONEDCREPLY? reply = null;
            uint? outVer = null;
            testSite.Log.Add(LogEntryKind.Checkpoint, "Begin to call IDL_DRSAddCloneDC");
            uint ret = drsClient.DrsAddCloneDC(EnvironmentConfig.DrsContextStore[svr],
                1,
                req,
                out outVer,
                out reply);
            testSite.Log.Add(LogEntryKind.Checkpoint, "End call IDL_DRSAddCloneDC with return value " + ret);

            if (EnvironmentConfig.ExpectSuccess)
                verifier.VerifyDRSAddCloneDC(ret, svr, machinename, sitename);

            return ret;
        }
        #endregion

        /// <summary>
        /// Wrapper for IDL_DRSInterDomainMove method which is a helper method used in a cross-NC move LDAP operation.
        /// </summary>
        /// <param name="sourceDc">The source DC.</param>
        /// <param name="reqVer">The request version.</param>
        /// <param name="targetDc">The target DC.</param>
        /// <param name="oldDN">The old distinguish name of the object to be moved.</param>
        /// <param name="newDN">The new DN of the object.</param>
        /// <param name="replyMsg">The reply message.</param>
        /// <returns>0 if successful, otherwise a Windows error code.</returns>
        [SupportedADType(ADInstanceType.DS)]
        public uint DrsInterDomainMove(
            EnvironmentConfig.Machine sourceDc,
            DRSInterDomainMove_Versions reqVer,
            EnvironmentConfig.Machine targetDc,
            string oldDN,
            string newDN,
            out DRS_MSG_MOVEREPLY replyMsg)
        {
            DsServer srcServer = (DsServer)EnvironmentConfig.MachineStore[sourceDc];
            DsServer dstServer = (DsServer)EnvironmentConfig.MachineStore[targetDc];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainAdmin];
            string spn = null;
            string[] spns = ldapAd.GetServicePrincipalName(dstServer);
            foreach (string sname in spns)
            {
                if (sname.StartsWith("GC", StringComparison.CurrentCultureIgnoreCase))
                {
                    spn = sname;
                    break;
                }
            }

            testSite.Assume.IsNotNull(spn, "DrsInterDomainMove: the GC spn of target Server should be found.");

            ENTINF? srcObjRef = ldapAd.GetENTINF(srcServer, oldDN, true);
            testSite.Assume.IsTrue(srcObjRef.HasValue, "DrsInterDomainMove: GetENTINF should be successful.");
            ENTINF srcObj = srcObjRef.Value;
            DSNAME srcPName = ldapAd.GetDsName(srcServer, oldDN).Value;
            srcObj.pName = srcPName;

            SCHEMA_PREFIX_TABLE schPreTable = OIDUtility.CreatePrefixTable();
            DRS_MSG_MOVEREQ moveReq;

            if (reqVer == DRSInterDomainMove_Versions.V1)
            {
                string dstParentDN = DrsrHelper.GetParentDNFromChildDN(newDN);
                DSNAME dstPName = ldapAd.GetDsName(dstServer, dstParentDN).Value;
                moveReq = drsClient.CreateDrsInterDomainMoveRequestV1(srcServer.DnsHostName, srcObj, dstPName.Guid, schPreTable);
            }
            else
            {
                DSNAME srcDsa = DrsuapiClient.CreateDsName(srcServer.NtdsDsaObjectName, srcServer.NtdsDsaObjectGuid, null);
                DSNAME dstName = DrsuapiClient.CreateDsName(newDN, srcPName.Guid, null);
                DSNAME dstNCName = ((AddsDomain)dstServer.Domain).DomainNC;
                DRS_SecBufferDesc secBuffer = DrsrHelper.GetAuthenticationToken(
                    user.Username,
                    user.Password,
                    EnvironmentConfig.DomainStore[DomainEnum.PrimaryDomain].DNSName,
                    spn
                );
                moveReq = drsClient.CreateDrsInterDomainMoveRequestV2(srcDsa, srcObj, dstName, dstNCName, secBuffer, schPreTable);
            }
            uint? outVer;
            DRS_MSG_MOVEREPLY? reply;

            uint ret = drsClient.DrsInterDomainMove(EnvironmentConfig.DrsContextStore[targetDc], (uint)reqVer, moveReq, out outVer, out reply);
            if (EnvironmentConfig.ExpectSuccess)
                verifier.VerifyDrsInterDomainMove(reqVer, outVer.Value, reply, ret);

            replyMsg = reply.Value;
            return ret;
        }

        #region Ngc Key
        /// <summary>
        /// Calling IDL_DRSWriteNgcKey method to compose and update the msDS-KeyCredentialLink value on an object
        /// </summary>
        /// <param name="svr">drsr bind machine</param>
        /// <param name="reqVer">the version of the request</param>
        /// <param name="dn">setting pwszAccount:  The DN of the object to modify</param>
        /// <param name="ngcKey">Ngc Key for the operation</param>
        /// <param name="outVersion">the version of the reply</param>
        /// <param name="outMessage">the reply message</param>
        /// <returns></returns>
        [SupportedADType(ADInstanceType.DS)]
        public uint DrsWriteNgcKey(
            EnvironmentConfig.Machine svr,
            uint reqVer,
            string dn,
            byte[] ngcKey,
            out uint? outVersion,
            out DRS_MSG_WRITENGCKEYREPLY? outMessage
            )
        {
            //return value
            uint ret = 0;

            //get context session
            testSite.Assert.IsTrue(EnvironmentConfig.MachineStore.ContainsKey(svr), "Load machine information for " + svr.ToString());
            DrsrClientSessionContext drsContext = null;
            EnvironmentConfig.DrsContextStore.TryGetValue(svr, out drsContext);

            //create request message
            DRS_MSG_WRITENGCKEYREQ? inMessage = null;
            inMessage = drsClient.CreateWriteNgcKeyRequest(dn, ngcKey);


            //Send/Response
            testSite.Log.Add(LogEntryKind.Checkpoint, "IDL_DRSWriteNgcKey: Start to call IDL_DRSWriteNgcKey");
            ret = drsClient.DrsWriteNgcKey(drsContext, reqVer, inMessage, out outVersion, out outMessage);
            testSite.Log.Add(LogEntryKind.Checkpoint, "IDL_DRSWriteNgcKey: End call IDL_DRSWriteNgcKey with return value " + ret);

            if (EnvironmentConfig.ExpectSuccess)
                verifier.VerifyDRSWriteNgcKey(reqVer, inMessage, outVersion, outMessage);

            return ret;
        }

        /// <summary>
        /// Calling IDL_DRSReadNgcKey method to read and parse the msDS-KeyCredentialLink value on an object
        /// </summary>
        /// <param name="svr">drsr bind machine</param>
        /// <param name="reqVer">the version of the request</param>
        /// <param name="dn">setting pwszAccount:  The DN of the object to modify</param>
        /// <param name="ngcKey">Ngc Key for the operation</param>
        /// <param name="outVersion">the version of the reply</param>
        /// <param name="outMessage">the reply message</param>
        /// <returns></returns>
        [SupportedADType(ADInstanceType.DS)]
        public uint DrsReadNgcKey(
            EnvironmentConfig.Machine svr,
            uint reqVer,
            string dn,
            out uint? outVersion,
            out DRS_MSG_READNGCKEYREPLY? outMessage
            )
        {
            //return value
            uint ret = 0;

            //get context session
            testSite.Assert.IsTrue(EnvironmentConfig.MachineStore.ContainsKey(svr), "Load machine information for " + svr.ToString());
            DrsrClientSessionContext drsContext = null;
            EnvironmentConfig.DrsContextStore.TryGetValue(svr, out drsContext);

            //create request message
            DRS_MSG_READNGCKEYREQ? inMessage = null;
            inMessage = drsClient.CreateReadNgcKeyRequest(dn);


            //Send/Response
            testSite.Log.Add(LogEntryKind.Checkpoint, "IDL_DRSReadNgcKey: Start to call IDL_DRSReadNgcKey");
            ret = drsClient.DrsReadNgcKey(drsContext, reqVer, inMessage, out outVersion, out outMessage);
            testSite.Log.Add(LogEntryKind.Checkpoint, "IDL_DRSReadNgcKey: End call IDL_DRSReadNgcKey with return value " + ret);

            if (EnvironmentConfig.ExpectSuccess)
                verifier.VerifyDRSReadNgcKey(reqVer, inMessage, outVersion, outMessage);

            return ret;
        }
        #endregion

        #region dsaop RPC methods

        /// <summary>
        /// RPC bind to dsaop interface of server.
        /// </summary>
        /// <param name="svr">The server to bind.</param>
        /// <param name="user">The user credential.</param>
        /// <returns>The RPC handle.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1304:SpecifyCultureInfo", MessageId = "System.String.ToUpper"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Int32.ToString"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", MessageId = "System.String.EndsWith(System.String)"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", MessageId = "System.String.IndexOf(System.String)")]
        public void RPCBind(EnvironmentConfig.Machine svr, EnvironmentConfig.User user, DrsrRpcInterfaceType ifType = DrsrRpcInterfaceType.DRSUAPI)
        {
            testSite.Assert.AreNotEqual<DrsrRpcInterfaceType>(DrsrRpcInterfaceType.NONE, ifType, "RPCBind: interface type should be provided.");

            #region RPC bind
            //get machine object
            testSite.Assert.IsTrue(EnvironmentConfig.MachineStore.ContainsKey(svr), "Load machine information for " + svr.ToString());

            DsMachine sut = EnvironmentConfig.MachineStore[svr];

            testSite.Assert.IsTrue(sut.GetType() == typeof(AddsServer) || sut.GetType() == typeof(AdldsServer), "The bind target should be DS or LDS");

            bool isLDS = sut.GetType() == typeof(AdldsServer) ? true : false;

            if (isLDS)
                testSite.Log.Add(LogEntryKind.Checkpoint, "SUT is AD LDS DC");
            else
                testSite.Log.Add(LogEntryKind.Checkpoint, "SUT is AD DS DC");

            //get user account credential
            testSite.Assert.IsTrue(EnvironmentConfig.UserStore.ContainsKey(user), "Load user information for " + user.ToString());

            AccountCredential cred = EnvironmentConfig.UserStore[user].GetAccountCredential();

            //get a SPN for bind
            string[] spns = null;
            if (!isLDS)
                spns = ldapAd.GetServicePrincipalName((DsServer)sut);
            else
                spns = new string[] { "HOST/" + sut.NetbiosName.Substring(0, sut.NetbiosName.IndexOf("$")) + ":" + ((AdldsServer)sut).Port };

            testSite.Assert.IsNotNull(spns, null);
            testSite.Assert.IsTrue(spns.Length > 0, null);
            string spn = null;

            if (isLDS)
            {
                foreach (string str in spns)
                {
                    if (str.EndsWith(((AdldsServer)sut).Port.ToString()))
                    {
                        spn = str;
                        break;
                    }
                }
            }
            else
            {
                // find the proper SPN of DRSUAPI
                string leadingGuid = DRSConstants.DrsRpcInterfaceGuid.ToString().ToUpper();
                foreach (string s in spns)
                {
                    if (s.ToUpper().Contains(leadingGuid))
                    {
                        spn = s;
                        break;
                    }

                    // RODC didn't have that SPN for outbound replication.
                    // look for "GC/" as the SPN.
                    if (((AddsServer)sut).IsRODC)
                    {
                        if (s.ToUpper().Contains("GC/"))
                        {
                            spn = s;
                            break;
                        }
                    }
                }
            }

            //create RPC session
            ClientSecurityContextAttribute contextAttr =
                EnvironmentConfig.UseKerberos == true ?
                (
                ClientSecurityContextAttribute.Connection
                | ClientSecurityContextAttribute.Integrity
                | ClientSecurityContextAttribute.Confidentiality
                | ClientSecurityContextAttribute.UseSessionKey
                | ClientSecurityContextAttribute.DceStyle)
                :
                (ClientSecurityContextAttribute.Connection
                | ClientSecurityContextAttribute.Integrity
                | ClientSecurityContextAttribute.Confidentiality
                | ClientSecurityContextAttribute.DceStyle);

            SspiClientSecurityContext securityContext = new SspiClientSecurityContext(
                EnvironmentConfig.UseKerberos == true ? SecurityPackageType.Kerberos : SecurityPackageType.Negotiate,
                cred,
                spn,
                contextAttr,
                SecurityTargetDataRepresentation.SecurityNativeDrep);

            //bind DS or LDS
            DrsrClientSessionContext clientContext = null;
            if (ifType == DrsrRpcInterfaceType.DRSUAPI)
            {
                #region bind to drsuapi RPC interface

                if (isLDS)
                {
                    if (EnvironmentConfig.UseNativeRpcLib)
                    {
                        throw new NotImplementedException();
                    }
                    else
                    {
                        clientContext = drsClient.BindOverTcp(
                            sut.DnsHostName,
                            spn,
                            null,
                            //always use this proto seq
                            "ncacn_ip_tcp",
                            ((AdldsServer)sut).Port.ToString(),
                            null,
                            securityContext,
                            RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_PKT_PRIVACY,
                            TimeSpan.FromSeconds(EnvironmentConfig.TestTimeout));
                    }
                }
                else
                {
                    if (EnvironmentConfig.UseNativeRpcLib)
                    {
                        clientContext = drsClient.Bind(
                            sut.DnsHostName,
                            sut.Domain.NetbiosName,
                            cred.AccountName,
                            cred.Password,
                            spn);
                    }
                    else
                    {
                        clientContext = drsClient.BindOverTcp(
                            sut.DnsHostName,
                            securityContext,
                            RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_PKT_PRIVACY,
                            TimeSpan.FromSeconds(EnvironmentConfig.TestTimeout));
                    }
                }
                #endregion
            }
            else
            {
                #region bind to dsaop RPC interface

                if (isLDS)
                {
                    clientContext = dsaClient.BindOverTcp(
                        sut.DnsHostName,
                        spn,
                        null,
                        //always use this proto seq
                        "ncacn_ip_tcp",
                        null,
                        null,
                        securityContext,
                        RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_PKT_PRIVACY,
                        TimeSpan.FromSeconds(EnvironmentConfig.TestTimeout));
                }
                else
                {
                    clientContext = dsaClient.BindOverTcp(
                        sut.DnsHostName,
                        securityContext,
                        RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_PKT_PRIVACY,
                        TimeSpan.FromSeconds(EnvironmentConfig.TestTimeout));
                }

                #endregion
            }

            testSite.Assert.IsTrue(clientContext.RPCHandle != IntPtr.Zero, "RPCBind should be successful.");

            clientContext.DomainFunLevel = EnvironmentConfig.DomainLevel;

            EnvironmentConfig.DrsContextStore.Add(svr, clientContext);

            #endregion
        }

        /// <summary>
        /// Wrapper for IDL_DSAPrepareScript.
        /// </summary>
        /// <param name="svr">The DC server.</param>
        /// <param name="inMessage">A pointer to the request message.</param>
        /// <param name="outVersion">A pointer to the version of the response message.</param>
        /// <returns>0 if successful, or a Windows error code if a failure occurs.</returns>
        public uint DsaPrepareScript(EnvironmentConfig.Machine svr, DSAPrepareScript_Versions reqVer, out uint? outVersion, out DSA_MSG_PREPARE_SCRIPT_REPLY? outMessage)
        {
            DSA_MSG_PREPARE_SCRIPT_REQ preScriptRequest = dsaClient.CreateDsaPrepareScript();
            uint ret = dsaClient.DsaPrepareScript(EnvironmentConfig.DrsContextStore[svr], (uint)reqVer, preScriptRequest, out outVersion, out outMessage);
            this.verifier.VerifyDsaPrepareScript(outVersion, outMessage);
            return ret;
        }

        /// <summary>
        /// IDL_DSAExecuteScript 
        /// </summary>
        /// <param name="svr">The DC server.</param>
        /// <param name="pwdBytes">The password in bytes.</param>
        /// <param name="inMessage">A pointer to the request message.</param>
        /// <param name="outVersion">A pointer to the version of the response message.</param>
        /// <returns>0 if successful, or a Windows error code if a failure occurs.</returns>
        public uint DsaExecuteScript(EnvironmentConfig.Machine svr, DSAExecuteScript_Versions reqVer, byte[] pwdBytes, out uint? outVersion, out DSA_MSG_EXECUTE_SCRIPT_REPLY? outMessage)
        {
            DSA_MSG_EXECUTE_SCRIPT_REQ exeScriptReq = dsaClient.CreateDsaExecuteScriptRequest(pwdBytes);
            uint ret = dsaClient.DsaExecuteScript(EnvironmentConfig.DrsContextStore[svr], (uint)reqVer, exeScriptReq, out outVersion, out outMessage);
            this.verifier.VerifyDsaExecuteScript(outVersion, outMessage);
            return ret;
        }
        #endregion

        #region replica demotion
        /// <summary>
        /// IDL_DRSReplicaDemotion
        /// </summary>
        /// <param name="svr">target</param>
        /// <param name="flag">DRS_MSG_REPLICA_DEMOTIONREQ_FLAGS</param>
        /// <param name="ncType">NC type</param>
        /// <param name="verificationPartner">verification partner</param>
        /// <returns>return value</returns>
        public uint DrsReplicaDemotion(EnvironmentConfig.Machine svr, DRS_MSG_REPLICA_DEMOTIONREQ_FLAGS flag, NamingContext ncType, EnvironmentConfig.Machine verificationPartner)
        {
            DsServer sut = (DsServer)EnvironmentConfig.MachineStore[svr];
            if (EnvironmentConfig.TestDS)
                testSite.Assert.IsTrue(sut.DCFunctional < 5, "IDL_DRSReplicaDemotion is not supported by AD DS on Windows Server 2012");

            DSNAME nc = new DSNAME();
            if (ncType == NamingContext.SchemaNC)
                nc = ((DsServer)EnvironmentConfig.MachineStore[svr]).Domain.SchemaNC;
            else if (ncType == NamingContext.ConfigNC)
                nc = ((DsServer)EnvironmentConfig.MachineStore[svr]).Domain.ConfigNC;
            else if (ncType == NamingContext.DomainNC)
                nc = ((AddsDomain)((DsServer)EnvironmentConfig.MachineStore[svr]).Domain).DomainNC;

            testSite.Log.Add(LogEntryKind.Comment, "Target Server is " + svr.ToString());
            testSite.Log.Add(LogEntryKind.Comment, "Set DRS_MSG_REPLICA_DEMOTIONREQ_FLAGS to " + flag.ToString());
            testSite.Log.Add(LogEntryKind.Comment, "Working with NamingContext " + ncType.ToString());
            DRS_MSG_REPLICA_DEMOTIONREQ? req = drsClient.CreateReplicaDemotionRequest(flag, LdapUtility.ConvertUshortArrayToString(nc.StringName), nc.Guid, convertSidToString(nc.Sid));
            uint? outver = null;
            DRS_MSG_REPLICA_DEMOTIONREPLY? reply = null;
            testSite.Log.Add(LogEntryKind.Checkpoint, "Start to call IDL_DRSReplicaDemotion");
            uint ret = drsClient.DrsReplicaDemotion(EnvironmentConfig.DrsContextStore[svr], 1, req, out outver, out reply);
            testSite.Log.Add(LogEntryKind.Checkpoint, "End call IDL_DRSReplicaDemotion with return value " + ret);
            if (EnvironmentConfig.ExpectSuccess)
                verifier.VerifyDRSReplicaDemotion(ret, svr, ncType, reply.Value, verificationPartner);

            return ret;
        }
        #endregion


        /// <summary>
        /// sync two DCs
        /// </summary>
        /// <param name="dc1">dc 1</param>
        /// <param name="dc2">dc 2</param>
        /// <returns>true if succeeded</returns>
        public bool SyncDCs(EnvironmentConfig.Machine dc1, EnvironmentConfig.Machine dc2)
        {
            bool old = EnvironmentConfig.ExpectSuccess;
            EnvironmentConfig.ExpectSuccess = false;
            bool success = true;
            int count = 3;
            do
            {
                success = true;
                uint ret = 0;
                ret = DrsBind(dc1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
                Site.Log.Add(LogEntryKind.Comment, "Bind dc1 result: " + ret);
                if (ret != 0)
                    break;
                try
                {
                    DrsReplicaDel(dc1, (DsServer)EnvironmentConfig.MachineStore[dc2], DRS_OPTIONS.DRS_WRIT_REP, NamingContext.ConfigNC);
                    ret = DrsReplicaAdd(dc1, DRS_MSG_REPADD_Versions.V1, (DsServer)EnvironmentConfig.MachineStore[dc2], DRS_OPTIONS.DRS_WRIT_REP, NamingContext.ConfigNC);
                    Site.Log.Add(LogEntryKind.Comment, "Sync DC dc2 -> dc1 result: " + ret);
                    if (ret != 0)
                        success = false;
                }
                catch
                {
                    if (((DsServer)EnvironmentConfig.MachineStore[dc1]).IsWindows)
                        throw;
                }
                finally
                {
                    DrsUnbind(dc1);
                }

                ret = DrsBind(dc2, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
                Site.Log.Add(LogEntryKind.Comment, "Bind dc2 result: " + ret);
                if (ret != 0)
                    break;

                try
                {
                    DrsReplicaDel(dc2, (DsServer)EnvironmentConfig.MachineStore[dc1], DRS_OPTIONS.DRS_WRIT_REP, NamingContext.ConfigNC);
                    ret = DrsReplicaAdd(dc2, DRS_MSG_REPADD_Versions.V1, (DsServer)EnvironmentConfig.MachineStore[dc1], DRS_OPTIONS.DRS_WRIT_REP, NamingContext.ConfigNC);
                    Site.Log.Add(LogEntryKind.Comment, "Sync DC dc1 -> dc2 result: " + ret);
                    if (ret != 0)
                        success = false;
                }
                catch
                {
                    if (((DsServer)EnvironmentConfig.MachineStore[dc2]).IsWindows)
                        throw;
                }
                finally
                {
                    DrsUnbind(dc2);
                }

                if (success)
                    break;
                System.Threading.Thread.Sleep(2000);
                count--;
            }
            while (count >= 0);

            EnvironmentConfig.ExpectSuccess = old;
            return success;
        }


        DSNAME getNCFromType(DsDomain domain, NamingContext nc)
        {
            switch (nc)
            {
                case NamingContext.ConfigNC:
                    return domain.ConfigNC;
                case NamingContext.SchemaNC:
                    return domain.SchemaNC;

                case NamingContext.AppNC:
                    if (EnvironmentConfig.TestDS)
                    {
                        if (((AddsDomain)domain).OtherNCs == null)
                            testSite.Assert.Fail("AppNC is not set in PTFConfig");
                        return ((AddsDomain)domain).OtherNCs[0];
                    }
                    else
                    {
                        if (((AdldsDomain)domain).AppNCs == null)
                            testSite.Assert.Fail("AppNC is not set in PTFConfig");
                        return ((AdldsDomain)domain).AppNCs[0];
                    }
                case NamingContext.DomainNC:
                    if (!EnvironmentConfig.TestDS)
                        testSite.Assert.Fail("not support Domain NC in testing AD LDS");
                    return ((AddsDomain)domain).DomainNC;
                default:
                    testSite.Assert.Fail("unsupported NC");
                    break;
            }
            return new DSNAME();
        }


        public ITestSite Site
        {
            get { return testSite; }
        }
    }
}
