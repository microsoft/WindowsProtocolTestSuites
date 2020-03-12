// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices.Protocols;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    /// <summary>
    /// Verify DRSR value in requests and responses
    /// </summary>
    public partial class DRSRVerifier : IDRSRVerifier
    {
        /// <summary>
        /// PTF test site
        /// </summary>
        ITestSite testSite;

        /// <summary>
        /// ldap adapter
        /// </summary>
        ILdapAdapter ldapAd;

        /// <summary>
        /// Initialize via PTF TestSite
        /// </summary>
        /// <param name="site">PTF Testsite</param>
        public void Initilize(ITestSite site)
        {
            testSite = site;
            ldapAd = testSite.GetAdapter<ILdapAdapter>();
        }

        /// <summary>
        /// Verify IDL_DRSBind
        /// </summary>
        /// <param name="machine">AD DS or LDS instance</param>
        /// <param name="user">the user credential used for DRSR session</param>
        /// <param name="drsHandle">DRS Handle</param>
        /// <param name="retVal">return value of IDL_DRSBind</param>
        /// <param name="clientDsa">puuidClientDsa in request</param>
        /// <param name="clientExt">pextClient in request</param>
        /// <param name="serverExt">ppextServer in response</param>
        public void VerifyDRSBind(EnvironmentConfig.Machine machine, EnvironmentConfig.User user, IntPtr drsHandle, uint retVal, DRS_EXTENSIONS_INT serverExt)
        {
            if (user != EnvironmentConfig.User.InvalidAccount)
            {
                testSite.Assert.AreEqual<uint>(0, retVal, "IDL_DRSBind: must return 0x0 when succeeded");

                testSite.Assert.AreNotEqual<IntPtr>(drsHandle, IntPtr.Zero, "IDL_DRSBind: must return a handle when succeeded");

                testSite.Assert.AreEqual<Guid>(((DsServer)EnvironmentConfig.MachineStore[machine]).Site.Guid, serverExt.SiteObjGuid, "IDL_DRSBind: DC should set the objectGuid of the site where it locates, to DRS_EXTENSIONS_INT!SiteObjGuid");

                if (EnvironmentConfig.DomainLevel > DrsrDomainFunctionLevel.DS_BEHAVIOR_WIN2003)
                    testSite.Assert.AreEqual<Guid>(EnvironmentConfig.MachineStore[machine].Domain.ConfigNC.Guid, serverExt.ConfigObjGUID, "IDL_DRSBind: DC should set the ConfigNC!ObjectGuid to DRS_EXTENSIONS_INT!ConfigObjGUID");

                if (EnvironmentConfig.DomainLevel >= DrsrDomainFunctionLevel.DS_BEHAVIOR_WIN2012R2)
                {

                }

            }
        }

        /// <summary>
        /// Verify IDL_DRSUnbind
        /// </summary>
        /// <param name="retVal">return value of IDL_DRSUnbind</param>
        public void VerifyDRSUnbind(uint retVal)
        {
            testSite.Assert.IsTrue(retVal == 0x0, "IDL_DRSUnbind: should return 0x0 when succeeds");
        }

        #region replication
        /// <summary>
        /// verify DrsGetReplInfo API
        /// </summary>
        /// <param name="dwInVersion">request message version</param>
        /// <param name="inMessage">request message</param>
        /// <param name="outVersion">response message version</param>
        /// <param name="outMessage">response message version</param>
        public void VerifyDrsGetReplInfo(
            uint dwInVersion,
            DRS_MSG_GETREPLINFO_REQ? inMessage,
            uint? outVersion,
            DRS_MSG_GETREPLINFO_REPLY? outMessage)
        {

            uint InfoType;
            switch (dwInVersion)
            {
                case 1:
                    testSite.Assert.IsNotNull(inMessage.Value.V1, "DrsGetReplInfo request message should contains the V1 request message");
                    InfoType = inMessage.Value.V1.InfoType;

                    break;
                case 2:
                    testSite.Assert.IsNotNull(inMessage.Value.V2, "DrsGetReplInfo request message should contains the V2 request message");
                    InfoType = inMessage.Value.V2.InfoType;
                    break;
                default:
                    testSite.Assert.Fail("DrsGetReplInfo request version is invalid");
                    return;
            }

            switch (InfoType)
            {
                case 0:
                    testSite.Assert.IsNotNull(outMessage.Value.pNeighbors, "DrsGetReplInfo reply message should contains pNeighbors ask for InfoType 0");
                    foreach (DS_REPL_NEIGHBORSW Neighbors in outMessage.Value.pNeighbors)
                    {
                        testSite.Assert.AreEqual<int>((int)Neighbors.cNumNeighbors, (int)Neighbors.rgNeighbor.Length, "rgNeighbor array of Neighbors should have cNumNeighbors to set the size");
                    }
                    break;
                case 1:
                    testSite.Assert.IsNotNull(outMessage.Value.pCursors, "DrsGetReplInfo reply message should contains pCursors ask for InfoType 1");
                    foreach (DS_REPL_CURSORS cur in outMessage.Value.pCursors)
                    {
                        testSite.Assert.AreEqual<int>((int)cur.cNumCursors, (int)cur.rgCursor.Length, "pCursor array of pCursors should have cNumCursors to set the size");
                    }
                    break;
                case 2:
                    testSite.Assert.IsNotNull(outMessage.Value.pObjMetaData, "DrsGetReplInfo response message should contains pObjMetaData ask for InfoType 2");
                    foreach (DS_REPL_OBJ_META_DATA data in outMessage.Value.pObjMetaData)
                    {
                        testSite.Assert.AreEqual<int>((int)data.cNumEntries, (int)data.rgMetaData.Length, "rgMetaData array of pObjMetaData should have cNumEntries to set the size");
                    }
                    break;
                case 3:
                    testSite.Assert.IsNotNull(outMessage.Value.pConnectFailures, "DrsGetReplInfo response message should contains pConnectFailures ask for InfoType 3");
                    foreach (DS_REPL_KCC_DSA_FAILURESW cFailure in outMessage.Value.pConnectFailures)
                    {
                        testSite.Assert.AreEqual<int>((int)cFailure.cNumEntries, (int)cFailure.rgDsaFailure.Length, "rgDsaFailure array of pConnectFailures should have cNumEntries to set the size");
                    }
                    break;
                case 4:
                    testSite.Assert.IsNotNull(outMessage.Value.pLinkFailures, "DrsGetReplInfo response message should contains pLinkFailures ask for InfoType 4");
                    foreach (DS_REPL_KCC_DSA_FAILURESW lFailure in outMessage.Value.pLinkFailures)
                    {
                        testSite.Assert.AreEqual<int>((int)lFailure.cNumEntries, (int)lFailure.rgDsaFailure.Length, "rgDsaFailure array of pLinkFailures should have cNumEntries to set the size");
                    }
                    break;
                case 5:
                    testSite.Assert.IsNotNull(outMessage.Value.pPendingOps, "DrsGetReplInfo response message should contains pPendingOps ask for InfoType 5");
                    foreach (DS_REPL_PENDING_OPSW pOps in outMessage.Value.pPendingOps)
                    {
                        testSite.Assert.AreEqual<int>((int)pOps.cNumPendingOps, (int)pOps.rgPendingOp.Length, "rgPendingOp array of pPendingOps should have cNumPendingOps to set the size");
                    }
                    break;
                case 6:
                    testSite.Assert.IsNotNull(outMessage.Value.pAttrValueMetaData, "DrsGetReplInfo response message should contains pAttrValueMetaData ask for InfoType 6");
                    foreach (DS_REPL_ATTR_VALUE_META_DATA mData in outMessage.Value.pAttrValueMetaData)
                    {
                        testSite.Assert.AreEqual<int>((int)mData.cNumEntries, (int)mData.rgMetaData.Length, "rgMetaData array of pAttrValueMetaData should have cNumEntries to set the size");
                    }
                    break;
                case 7:
                    testSite.Assert.IsNotNull(outMessage.Value.pCursors2, "DrsGetReplInfo response message should contains pCursors2 ask for InfoType 7");
                    foreach (DS_REPL_CURSORS_2 cur2 in outMessage.Value.pCursors2)
                    {
                        testSite.Assert.AreEqual<int>((int)cur2.cNumCursors, (int)cur2.rgCursor.Length, "pCursor array of pCursors2 should have cNumCursors to set the size");
                    }
                    break;
                case 8:
                    testSite.Assert.IsNotNull(outMessage.Value.pCursors3, "DrsGetReplInfo response message should contains pCursors3 ask for InfoType 8");
                    foreach (DS_REPL_CURSORS_3W cur3 in outMessage.Value.pCursors3)
                    {
                        testSite.Assert.AreEqual<int>((int)cur3.cNumCursors, (int)cur3.rgCursor.Length, "pCursor array of pCursors3 should have cNumCursors to set the size");
                    }
                    break;
                case 9:
                    testSite.Assert.IsNotNull(outMessage.Value.pObjMetaData2, "DrsGetReplInfo response message should contains pObjMetaData2 ask for InfoType 9");
                    foreach (DS_REPL_OBJ_META_DATA_2 mObjData2 in outMessage.Value.pObjMetaData2)
                    {
                        testSite.Assert.AreEqual<int>((int)mObjData2.cNumEntries, (int)mObjData2.rgMetaData.Length, "rgMetaData array of pObjMetaData2 should have cNumEntries to set the size");
                    }
                    break;
                case 10:
                    testSite.Assert.IsNotNull(outMessage.Value.pAttrValueMetaData2, "DrsGetReplInfo response message should contains pAttrValueMetaData2 ask for InfoType 10");
                    foreach (DS_REPL_ATTR_VALUE_META_DATA_2 mAttData2 in outMessage.Value.pAttrValueMetaData2)
                    {
                        testSite.Assert.AreEqual<int>((int)mAttData2.cNumEntries, (int)mAttData2.rgMetaData.Length, "rgMetaData array of pAttrValueMetaData2 should have cNumEntries to set the size");
                    }
                    break;
                case 0xFFFFFFFA:
                    testSite.Assert.IsNotNull(outMessage.Value.pServerOutgoingCalls, "DrsGetReplInfo response message should contains pServerOutgoingCalls ask for InfoType 0xFFFFFFFA");
                    foreach (DS_REPL_SERVER_OUTGOING_CALLS gCall in outMessage.Value.pServerOutgoingCalls)
                    {
                        testSite.Assert.AreEqual<int>((int)gCall.cNumCalls, (int)gCall.rgCall.Length, "rgCall array of pServerOutgoingCalls should have cNumCalls to set the size");
                    }
                    break;
                case 0xFFFFFFFB:
                    testSite.Assert.IsNotNull(outMessage.Value.pUpToDateVec, "DrsGetReplInfo response message should contains pUpToDateVec ask for InfoType 0xFFFFFFFB");
                    foreach (UPTODATE_VECTOR_V1_EXT dateVec in outMessage.Value.pUpToDateVec)
                    {
                        testSite.Assert.AreEqual<int>((int)dateVec.cNumCursors, (int)dateVec.rgCursors.Length, "rgCursors array of pUpToDateVec should have cNumCursors to set the size");
                    }
                    break;
                case 0xFFFFFFFC:
                    testSite.Assert.IsNotNull(outMessage.Value.pClientContexts, "DrsGetReplInfo response message should contains pClientContexts ask for InfoType 0xFFFFFFFC");
                    foreach (DS_REPL_CLIENT_CONTEXTS clientContext in outMessage.Value.pClientContexts)
                    {
                        testSite.Assert.AreEqual<int>((int)clientContext.cNumContexts, (int)clientContext.rgContext.Length, "rgContext array of pClientContexts should have cNumContexts to set the size");
                    }
                    break;
                case 0xFFFFFFFE:
                    testSite.Assert.IsNotNull(outMessage.Value.pRepsTo, "DrsGetReplInfo response message should contains pRepsTo ask for InfoType 0xFFFFFFFE");
                    foreach (DS_REPL_NEIGHBORSW repsTo in outMessage.Value.pRepsTo)
                    {
                        testSite.Assert.AreEqual<int>((int)repsTo.cNumNeighbors, (int)repsTo.rgNeighbor.Length, "rgNeighbor array of pRepsTo should have cNumNeighbors to set the size");
                    }
                    break;
                default:
                    testSite.Assert.Fail("DrsGetReplInfo reply message version is invalid");
                    return;
            }

            return;
        }

        /// <summary>
        /// verify DrsGetNCChanges API
        /// </summary>
        /// <param name="dwInVersion">request message version</param>
        /// <param name="inMessage">request message</param>            
        /// <param name="outVersion">response message version</param>
        /// <param name="outMessage">response message version</param>
        public void VerifyDrsGetNCChanges(
            uint retVal,
            uint dwInVersion,
            DRS_MSG_GETCHGREQ? inMessage,
            uint? outVersion,
            DRS_MSG_GETCHGREPLY? outMessage)
        {
            testSite.Assert.AreEqual<uint>(0, retVal, "IDL_DRSGetNCChanges should return 0x0 for successful operation");
            testSite.Assert.IsNotNull(outVersion, "DrsGetNCChanges reply version should not be null");
            switch (dwInVersion)
            {

                case 5:
                    testSite.Assert.IsNotNull(inMessage.Value.V5, "DrsGetNCChanges request message should contains the V5 request message ask for dwInVersion 5");
                    break;
                case 8:
                    testSite.Assert.IsNotNull(inMessage.Value.V8, "DrsGetNCChanges request message should contains the V8 request message ask for dwInVersion 8");
                    break;
                case 10:
                    testSite.Assert.IsNotNull(inMessage.Value.V10, "DrsGetNCChanges request message should contains the V10 request message ask for dwInVersion 10");
                    break;
                default:
                    return;
            }

            switch (outVersion)
            {

                case 1:
                    testSite.Assert.IsNotNull(outMessage.Value.V1, "V1 reply should not be null if reply version is 1");
                    break;
                case 2:
                    testSite.Assert.IsNotNull(outMessage.Value.V2, "V2 reply should not be null if reply version is 2");
                    testSite.Assert.AreEqual<uint>(outMessage.Value.V2.CompressedV1.cbCompressedSize, (uint)outMessage.Value.V2.CompressedV1.pbCompressedData.Length, "the pbCompressedData should have cbCompressedSize to set the array length");
                    break;
                case 6:
                    testSite.Assert.IsNotNull(outMessage.Value.V6, "V6 reply should not be null if reply version is 6");

                    if (dwInVersion == 8)
                    {
                        testSite.Assert.AreEqual<Guid>(inMessage.Value.V8.pNC.Value.Guid, outMessage.Value.V6.pNC.Value.Guid, "V6 reply should contain PNC {0} in response pNC", inMessage.Value.V8.pNC);
                        testSite.Assert.AreEqual<USN_VECTOR>(inMessage.Value.V8.usnvecFrom, outMessage.Value.V6.usnvecFrom, "V6 reply should contain usnvecform of in message");
                    }
                    break;
                case 7:
                    testSite.Assert.IsNotNull(outMessage.Value.V7, "V7 reply should not be null if reply version is 7");
                    break;
                case 9:
                    testSite.Assert.IsNotNull(outMessage.Value.V9, "V9 reply should not be null if reply version is 9");
                    break;
                default:
                    testSite.Assert.Fail("VerifyDrsGetNCChanges reply version {0} is invalid", outVersion);
                    return;
            }
        }

        #endregion
        #region DC locate
        /// <summary>
        /// verify DrsGetObjectExistence API
        /// </summary>
        /// <param name="dwInVersion">request message version</param>
        /// <param name="inMessage">request message</param>
        /// <param name="outVersion">response message version</param>
        /// <param name="outMessage">response message version</param>
        public void VerifyDrsGetObjectExistence(
            uint dwInVersion,
            DRS_MSG_EXISTREQ? inMessage,
            uint? outVersion,
            DRS_MSG_EXISTREPLY? outMessage)
        {
            testSite.Assert.AreEqual<int>((int)dwInVersion, 1, "DrsGetObjectExistence request message version should be 1");
            testSite.Assert.AreEqual<int>((int)outVersion, 1, "DrsGetObjectExistence reply message version should be 1");

            if (outMessage.HasValue && outMessage.Value.V1.cNumGuids > 0)
            {
                testSite.Assert.AreEqual<int>((int)outMessage.Value.V1.cNumGuids, outMessage.Value.V1.rgGuids.Length, "rgGuids array of DRS_MSG_EXISTREPLY_V1 should have cNumGuids to set the size");
            }

        }

        /// <summary>
        /// return number of DC servers in specified domain
        /// </summary>
        /// <param name="domain">domain</param>
        /// <returns>count</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1304:SpecifyCultureInfo", MessageId = "System.String.ToLower")]
        int getDCCountInDomain(string domain)
        {
            int ret = 0;
            string lowed_name = domain.ToLower();
            Dictionary<EnvironmentConfig.Machine, DsMachine>.Enumerator enumer = EnvironmentConfig.PhysicalMachineStore.GetEnumerator();
            while (enumer.MoveNext())
            {
                if ((enumer.Current.Value.Domain.NetbiosName.ToLower() == lowed_name
                    || enumer.Current.Value.Domain.DNSName.ToLower() == lowed_name
                    || enumer.Current.Value.Domain.Name.ToLower() == lowed_name)
                    && enumer.Current.Key != EnvironmentConfig.Machine.Endpoint)
                    ret++;
            }
            return ret;
        }

        /// <summary>
        /// Verify DRSDomainControllerInfo API
        /// </summary>
        /// <param name="retVal">return value</param>
        /// <param name="outVer">response version</param>
        /// <param name="req">request data</param>
        /// <param name="reply">response data</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1304:SpecifyCultureInfo", MessageId = "System.String.ToLower")]
        public void VerifyDRSDomainControllerInfo(uint retVal, EnvironmentConfig.Machine svr, uint? outVer, DRS_MSG_DCINFOREQ? req, DRS_MSG_DCINFOREPLY? reply)
        {
            DsServer sut = (DsServer)EnvironmentConfig.MachineStore[svr];
            testSite.Assert.AreEqual<uint>(0, retVal, "IDL_DRSDomainController: Checking return value - got: {0}, expect: 0, should return 0 on success", retVal);

            if (reply.Value.V1.cItems > 0)
            {
                testSite.Assert.AreEqual<int>(
                    (int)reply.Value.V1.cItems,
                    reply.Value.V1.rItems.Length,
                    "IDL_DRSDomainController: Checking the length of cItems and rItems in reply - cItems: {0}, rItems: {1}",
                    (int)reply.Value.V1.cItems,
                    reply.Value.V1.rItems.Length);
            }
            else
            {
                testSite.Assert.IsNull(reply.Value.V1.rItems, "IDL_DRSDomainController: Checking if rItems in reply is null - {0}", reply.Value.V1.rItems == null);
            }

            int servercount = getDCCountInDomain(req.Value.V1.Domain);
            switch (req.Value.V1.InfoLevel)
            {
                case 1:

                    testSite.Assert.IsNotNull(reply.Value.V1, "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains V1 data when DRS_MSG_DCINFOREQ asks for InfoLevel 1");

                    testSite.Assert.AreEqual<int>(
                        (int)reply.Value.V1.cItems,
                        reply.Value.V1.rItems.Length,
                        "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY have cItems = {0}, should equal to the length of rItems = {1}",
                        (int)reply.Value.V1.cItems,
                        reply.Value.V1.rItems.Length
                        );


                    if (EnvironmentConfig.PhysicalMachineStore.ContainsKey(EnvironmentConfig.Machine.RODC)) //get count of writable physicial dc servers only
                        servercount--;
                    testSite.Assert.AreEqual<int>(
                        servercount,
                        reply.Value.V1.rItems.Length,
                        "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY have server count = {0}, should equal to the number of physical DC servers {1} in this domain",
                        servercount,
                        reply.Value.V1.rItems.Length
                        );

                    foreach (DS_DOMAIN_CONTROLLER_INFO_1W w1 in reply.Value.V1.rItems)
                    {
                        #region inner verify
                        DsServer found = (DsServer)EnvironmentConfig.FoundMatchedServerByDNSName(w1.DnsHostName);
                        testSite.Assert.IsNotNull(found, "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains corresponding  DC information");
                        testSite.Assert.Pass("IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains corresponding DnsHostName");

                        testSite.Assert.AreEqual<string>(found.NetbiosName.ToLower(), w1.NetbiosName.ToLower(), "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains corresponding NetbiosName");

                        testSite.Assert.AreEqual<string>(found.Site.CN.ToLower(), w1.SiteName.ToLower(), "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains corresponding SiteName");

                        testSite.Assert.IsTrue(DrsrHelper.AreDNsSame(found.ComputerObjectName, w1.ComputerObjectName), "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains corresponding ComputerObjectName");

                        testSite.Assert.IsTrue(DrsrHelper.AreDNsSame(found.ServerObjectName, w1.ServerObjectName), "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains corresponding ServerObjectName");

                        testSite.Assert.AreEqual<bool>(((int)found.FsmoRoles & (0xffffffff ^ (int)FSMORoles.PDC)) > 0, w1.fIsPdc == 1, "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should set fIsPdc if this DC has PDC FSMO role");

                        testSite.Assert.AreEqual<int>(1, w1.fDsEnabled, "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY MUST set fDsEnabled to TRUE");
                        #endregion
                    }
                    break;
                case 2:
                    testSite.Assert.IsNotNull(reply.Value.V2, "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains V2 data when DRS_MSG_DCINFOREQ asks for InfoLevel 2");

                    testSite.Assert.AreEqual<int>((int)reply.Value.V2.cItems, reply.Value.V2.rItems.Length, "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should have cItems set same as length of rItems");

                    if (EnvironmentConfig.PhysicalMachineStore.ContainsKey(EnvironmentConfig.Machine.RODC))   //get count of writable physicial dc servers only
                        servercount--;
                    testSite.Assert.AreEqual<int>(servercount, reply.Value.V2.rItems.Length, "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should return same number of information as physical DC servers in this domain");

                    foreach (DS_DOMAIN_CONTROLLER_INFO_2W w2 in reply.Value.V2.rItems)
                    {
                        #region inner verify
                        DsServer found = (DsServer)EnvironmentConfig.FoundMatchedServerByDNSName(w2.DnsHostName);
                        testSite.Assert.IsNotNull(found, "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains corresponding DC information");
                        testSite.Assert.Pass("IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains corresponding DnsHostName");

                        testSite.Assert.AreEqual<string>(found.NetbiosName.ToLower(), w2.NetbiosName.ToLower(), "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains corresponding NetbiosName");

                        testSite.Assert.AreEqual<string>(found.Site.CN.ToLower(), w2.SiteName.ToLower(), "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains corresponding SiteName");

                        testSite.Assert.IsTrue(DrsrHelper.AreDNsSame(found.Site.DN, w2.SiteObjectName), "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains corresponding SiteObjectName");

                        testSite.Assert.IsTrue(DrsrHelper.AreDNsSame(found.NtdsDsaObjectName, w2.NtdsDsaObjectName), "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains corresponding NtdsDsaObjectName");

                        //testSite.Assert.AreEqual<bool>(w2.fIsGc == 1, EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.GCServer].DnsHostName.ToLower() == w2.DnsHostName.ToLower(), "DRS_MSG_DCINFOREPLY should set fIsGc to true if DC is also a GC");

                        testSite.Assert.IsTrue(DrsrHelper.AreDNsSame(found.ComputerObjectName, w2.ComputerObjectName), "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains corresponding ComputerObjectName");

                        testSite.Assert.IsTrue(DrsrHelper.AreDNsSame(found.ServerObjectName, w2.ServerObjectName), "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains corresponding ServerObjectName");

                        testSite.Assert.AreEqual<bool>(((int)found.FsmoRoles & (0xffffffff ^ (int)FSMORoles.PDC)) > 0, w2.fIsPdc == 1, "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should set fIsPdc if this DC has PDC FSMO role");

                        testSite.Assert.AreEqual<Guid>(found.NtdsDsaObjectGuid, w2.NtdsDsaObjectGuid, "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains corresponding nTDSDSA object Guid");

                        testSite.Assert.AreEqual<Guid>(found.Site.Guid, w2.SiteObjectGuid, "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains corresponding SiteObjectGuid");

                        testSite.Assert.AreEqual<int>(1, w2.fDsEnabled, "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY MUST set fDsEnabled to TRUE");
                        #endregion
                    }
                    break;
                case 3:
                    testSite.Assert.IsNotNull(reply.Value.V3, "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains V3 data when DRS_MSG_DCINFOREQ asks for InfoLevel 3");

                    testSite.Assert.AreEqual<int>((int)reply.Value.V3.cItems, reply.Value.V3.rItems.Length, "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should have cItems set same as length of rItems");

                    testSite.Assert.AreEqual<int>(servercount, reply.Value.V3.rItems.Length, "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should return same number of information as physical DC servers in this domain");

                    foreach (DS_DOMAIN_CONTROLLER_INFO_3W w3 in reply.Value.V3.rItems)
                    {
                        #region inner verify
                        DsServer found = (DsServer)EnvironmentConfig.FoundMatchedServerByDNSName(w3.DnsHostName);
                        testSite.Assert.IsNotNull(found, "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains corresponding DC information");
                        testSite.Assert.Pass("IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains corresponding DnsHostName");

                        testSite.Assert.AreEqual<string>(found.NetbiosName.ToLower(), w3.NetbiosName.ToLower(), "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains corresponding NetbiosName");

                        testSite.Assert.AreEqual<string>(found.Site.CN.ToLower(), w3.SiteName.ToLower(), "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains corresponding SiteName");

                        testSite.Assert.IsTrue(DrsrHelper.AreDNsSame(found.Site.DN, w3.SiteObjectName), "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains corresponding SiteObjectName");

                        testSite.Assert.IsTrue(DrsrHelper.AreDNsSame(found.NtdsDsaObjectName, w3.NtdsDsaObjectName), "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains corresponding NtdsDsaObjectName");

                        //testSite.Assert.AreEqual<bool>(w3.fIsGc == 1, EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.GCServer].DnsHostName.ToLower() == w3.DnsHostName.ToLower(), "DRS_MSG_DCINFOREPLY should set fIsGc to true if DC is also a GC");

                        if (EnvironmentConfig.MachineStore.ContainsKey(EnvironmentConfig.Machine.RODC))
                        {
                            if (EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.RODC].DnsHostName.ToLower() == w3.DnsHostName.ToLower())
                                testSite.Assert.IsTrue((w3.fIsRodc == 1), "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should set fIsRodc to true if DC is also a rodc");
                        }
                        else
                            testSite.Assert.AreEqual<int>(w3.fIsRodc, 0, "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should set fIsRodc to true if DC is also a rodc");

                        testSite.Assert.IsTrue(DrsrHelper.AreDNsSame(found.ComputerObjectName, w3.ComputerObjectName), "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains corresponding ComputerObjectName");

                        testSite.Assert.IsTrue(DrsrHelper.AreDNsSame(found.ServerObjectName, w3.ServerObjectName), "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains corresponding ServerObjectName");

                        testSite.Assert.AreEqual<bool>(((int)found.FsmoRoles & (0xffffffff ^ (int)FSMORoles.PDC)) > 0, w3.fIsPdc == 1, "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should set fIsPdc if this DC has PDC FSMO role");

                        testSite.Assert.AreEqual<Guid>(found.NtdsDsaObjectGuid, w3.NtdsDsaObjectGuid, "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains corresponding nTDSDSA object Guid");

                        testSite.Assert.AreEqual<Guid>(found.Site.Guid, w3.SiteObjectGuid, "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains corresponding SiteObjectGuid");

                        testSite.Assert.AreEqual<int>(1, w3.fDsEnabled, "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY MUST set fDsEnabled to TRUE");
                        #endregion
                    }
                    break;
                case 0xffffffff:
                    testSite.Assert.IsNotNull(reply.Value.VFFFFFFFF, "IDL_DRSDomainController: DRS_MSG_DCINFOREPLY should contains V0xffffffff data when DRS_MSG_DCINFOREQ asks for InfoLevel 0xffffffff");
                    testSite.Assert.AreEqual<uint>((uint)reply.Value.VFFFFFFFF.rItems.Length, reply.Value.VFFFFFFFF.cItems, "IDL_DRSDomainController: cItems in V0xffffffff should be count of rItems");

                    foreach (DS_DOMAIN_CONTROLLER_INFO_FFFFFFFFW fw in reply.Value.VFFFFFFFF.rItems)
                    {
                        if (sut.IsWindows)
                        {
                            testSite.Assert.AreEqual<Reserved1_Values>(Reserved1_Values.V1, fw.Reserved1, "IDL_DRSDomainController: In V0xffffffff data, Reserved1 MUST be 0 for windows");
                        }
                    }
                    //nothing else to check because they are about existing LDAP connections
                    break;
                default:
                    testSite.Assert.Fail("IDL_DRSDomainController: DRS_MSG_DCINFOREPLY version is invalid");
                    break;
            }
        }



        /// <summary>
        /// Verify DRSQuerySitesByCost API
        /// </summary>
        /// <param name="retVal">return value</param>
        /// <param name="outVer">response version</param>
        /// <param name="from">"from" this site</param>
        /// <param name="tp">"to" these sites</param>
        /// <param name="reply">response data</param>
        public void VerifyDRSQuerySitesByCost(uint retVal, EnvironmentConfig.Machine svr, uint? outVer, DsSite from, DsSite[] to, DRS_MSG_QUERYSITESREPLY? reply)
        {
            DsServer sut = (DsServer)EnvironmentConfig.MachineStore[svr];
            testSite.Assert.AreEqual<uint>(0, retVal, "IDL_DRSQuerySitesByCost: Checking return value - got: {0}, expect: 0, must return 0x0 when succeed", retVal);

            testSite.Assert.AreEqual<uint>(1, outVer.Value, "IDL_DRSQuerySitesByCost: Checking outVersion in reply - got: {0}, expect: 1, must set out version to 0x1 when succeeds", outVer.Value);

            testSite.Assert.AreEqual<uint>((uint)to.Length, reply.Value.V1.cToSites, "IDL_DRSQuerySitesByCost: Checking cToSites in reply - got: {0}, expect: {1}", reply.Value.V1.cToSites, (uint)to.Length);

            if (reply.Value.V1.rgCostInfo != null)
            {
                foreach (DRS_MSG_QUERYSITESREPLYELEMENT_V1 ele in reply.Value.V1.rgCostInfo)
                {
                    testSite.Assert.AreEqual<uint>(0, ele.dwErrorCode, "IDL_DRSQuerySitesByCost: Checking dwErrorCode in reply - got: {0}, expect: 0", ele.dwErrorCode);
                }
            }
            if (sut.IsWindows)
                testSite.Assert.AreEqual<DRS_MSG_QUERYSITESREPLY_V1_dwFlags_Values>(
                    DRS_MSG_QUERYSITESREPLY_V1_dwFlags_Values.V1,
                    reply.Value.V1.dwFlags,
                    "IDL_DRSQuerySitesByCost: Checking dwFlags in reply - got: {0}, expect: {1}",
                    (uint)reply.Value.V1.dwFlags,
                    (uint)DRS_MSG_QUERYSITESREPLY_V1_dwFlags_Values.V1);
        }
        #endregion

        #region KCC
        /// <summary>
        /// Verify DRSExecuteKCC API
        /// </summary>
        /// <param name="retVal">return value</param>
        /// <param name="isAsync">async or sync</param>
        public void VerifyDRSExecuteKCC(uint retVal)
        {
            testSite.Assert.AreEqual<uint>(0, retVal, "IDL_DRSExecuteKCC: Checking return value - got: {0}, expect: 0, should return 0x0 for successful operation", retVal);
        }

        /// <summary>
        /// Verify DRSReplicaAdd API
        /// </summary>
        /// <param name="retVal">return value</param>
        /// <param name="machine">SUT</param>
        /// <param name="src">replication source</param>
        /// <param name="options">options</param>
        public void VerifyDRSReplicaAdd(uint retVal, EnvironmentConfig.Machine machine, DsServer src, DRS_OPTIONS options)
        {
            testSite.Assert.AreEqual<uint>(0, retVal, "IDL_DRSReplicaAdd: Checking return value - got: {0}, expect: 0, should return 0x0 for successful operation", retVal);

            if ((options & DRS_OPTIONS.DRS_ASYNC_OP) > 0)
                return;

            DsServer sut = (DsServer)EnvironmentConfig.MachineStore[machine];
            //check "src"'s record in "machine"
            REPS_FROM[] froms = ldapAd.GetRepsFrom(sut, NamingContext.ConfigNC);
            bool contains = false;
            if (froms != null)
            {
                for (int i = 0; i < froms.Length; i++)
                {
                    if (froms[i].uuidDsaObj == src.NtdsDsaObjectGuid && froms[i].uuidInvocId == src.InvocationId)
                    {
                        contains = true;
                        break;
                    }
                }
            }
            testSite.Assert.IsTrue(contains, "IDL_DRSReplicaAdd: SUT should contains the REPS_FROM record for specified source after IDL_DRSReplicaAdd has succeeded");

        }

        /// <summary>
        /// verify DRSReplicaDel API
        /// </summary>
        /// <param name="retVal">return value</param>
        /// <param name="machine">SUT</param>
        /// <param name="src">replication source</param>
        /// <param name="options">options</param>
        public void VerifyDRSReplicaDel(uint retVal, EnvironmentConfig.Machine machine, DsServer src, DRS_OPTIONS options)
        {
            testSite.Assert.AreEqual<uint>(0, retVal, "IDL_DRSReplicaDel: Checking return value - got: {0}, expect: 0, should return 0x0 for successful operation", retVal);

            DsServer sut = (DsServer)EnvironmentConfig.MachineStore[machine];
            //check "src"'s record in "machine"
            bool contains = false;
            try
            {
                REPS_FROM[] froms = ldapAd.GetRepsFrom(sut, NamingContext.ConfigNC);

                if (froms != null)
                {
                    foreach (REPS_FROM f in froms)
                    {
                        if (f.uuidDsaObj == sut.NtdsDsaObjectGuid || f.uuidInvocId == sut.InvocationId)
                        {
                            contains = true;
                            break;
                        }
                    }
                }
            }
            catch
            {
            }
            testSite.Assert.IsFalse(contains, "IDL_DRSReplicaDel: SUT should not contains the REPS_FROM record for specified source after IDL_DRSReplicaDel has succeeded");
        }

        /// <summary>
        /// Verify DRSReplicaModify API
        /// </summary>
        /// <param name="retVal">return value</param>
        /// <param name="machine">SUT</param>
        /// <param name="src">replication source</param>
        /// <param name="replicaFlags">replication flags</param>
        /// <param name="fields">modified fields</param>
        /// <param name="options">options</param>
        public void VerifyDRSReplicaModify(uint retVal, EnvironmentConfig.Machine machine, DsServer src, DRS_OPTIONS replicaFlags, DRS_MSG_REPMOD_FIELDS fields, DRS_OPTIONS options)
        {
            testSite.Assert.AreEqual<uint>(0, retVal, "IDL_DRSReplicaModify: Checking return value - got: {0}, expect: 0, should return 0x0 for successful operation", retVal);

            if ((options & DRS_OPTIONS.DRS_ASYNC_OP) > 0)
                return;

            REPS_FROM[] froms = ldapAd.GetRepsFrom((DsServer)EnvironmentConfig.MachineStore[machine], NamingContext.ConfigNC);
            bool passed = false;
            if (froms != null)
            {
                foreach (REPS_FROM f in froms)
                {
                    if (f.uuidDsaObj == src.NtdsDsaObjectGuid)
                    {
                        for (int i = 0; i < f.rtSchedule.rgTimes.Length; i++)
                        {
                            if (f.rtSchedule.rgTimes[i] != 1)
                                break;
                        }

                        if (f.ulReplicaFlags != (uint)DRS_OPTIONS.DRS_WRIT_REP)
                            break;

                        passed = true;
                    }
                }
            }


            testSite.Assert.IsTrue(passed, "IDL_DRSReplicaModify: should update replication source information if it has succeeded");


        }

        /// <summary>
        /// Verify DRSUpdateRefs API
        /// </summary>
        /// <param name="retVal">return value</param>
        /// <param name="machine">SUT</param>
        /// <param name="dest">replication destination</param>
        /// <param name="options">options</param>
        public void VerifyDRSUpdateRefs(uint retVal, EnvironmentConfig.Machine machine, DsServer dest, DRS_OPTIONS options)
        {
            testSite.Assert.AreEqual<uint>(0, retVal, "IDL_DRSUpdateRefs should return 0x0 for successful operation");
        }
        #endregion


        #region Administration Tool Support
        /// <summary>
        /// verify DRSAddEntry API
        /// </summary>
        /// <param name="dwInVersion">request message version</param>
        /// <param name="inMessage">request message</param>
        /// <param name="outVersion">reply message version</param>
        /// <param name="outMessage">reply message</param>
        public void VerifyDRSAddEntry(
            uint dwInVersion,
            DRS_MSG_ADDENTRYREQ? inMessage,
            uint? outVersion,
            DRS_MSG_ADDENTRYREPLY? outMessage)
        {

            testSite.Assert.AreEqual<uint>(dwInVersion, outVersion.Value, "request message version should equal to the request version");

            switch (dwInVersion)
            {
                case 2:
                    testSite.Assert.IsNotNull(inMessage.Value.V2, "V2 request message should not be null when request version is V2");
                    break;
                case 3:
                    testSite.Assert.IsNotNull(inMessage.Value.V3, "V3 request message should not be null when request version is V3");
                    testSite.Assert.AreEqual<int>((int)outMessage.Value.V3.cObjectsAdded, outMessage.Value.V3.infoList.Length, "infoList array of DRS_MSG_ADDENTRYREPLY_V3 should have cObjectsAdded to set the size");
                    break;
                default:
                    testSite.Assert.Fail("Invalid request message version");
                    return;
            }

            return;

        }

        /// <summary>
        /// verify DrsGetReplInfo API
        /// </summary>
        /// <param name="dwInVersion">request message version</param>
        /// <param name="inMessage">request message</param>
        /// <param name="outVersion">reply message version</param>
        /// <param name="outMessage">reply message</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.UInt32.ToString")]
        public void VerifyDRSAddSidHistory(
            uint retVal,
            string srcSid,
            string srcObjDn,
            ObjectNameType srcNameType,
            string destObjDn,
            ObjectNameType destNameType,
            EnvironmentConfig.Machine src,
            EnvironmentConfig.Machine dest,
            IDL_DRSAddSidHistory_dwInVersion_Values dwInVersion,
            DRS_MSG_ADDSIDREQ? inMessage,
            IDL_DRSAddSidHistory_pdwOutVersion_Values? outVersion,
            DRS_MSG_ADDSIDREPLY? outMessage)
        {
            testSite.Assert.AreEqual<uint>(0, retVal, "IDL_DRSAddSidHistory should return 0x0 for successful operation");
            testSite.Assert.IsNotNull(outVersion, "AddSidHistory reply message version should not be null");
            testSite.Assert.AreEqual<IDL_DRSAddSidHistory_pdwOutVersion_Values>(outVersion.Value, IDL_DRSAddSidHistory_pdwOutVersion_Values.V1, "Only V1 AddSidHistory reply message is supported");
            testSite.Assert.IsNotNull(outMessage, "AddSidHistory reply message should not be null");
            testSite.Assert.IsTrue(outMessage.Value.V1.dwWin32Error == 0, "AddSidHistory failed with windows error code " + outMessage.Value.V1.dwWin32Error.ToString());

            //this flag does not change object
            if ((DRS_ADDSID_FLAGS)inMessage.Value.V1.Flags == DRS_ADDSID_FLAGS.DS_ADDSID_FLAG_PRIVATE_CHK_SECURE)
                return;

            DsServer srcDC = (DsServer)EnvironmentConfig.MachineStore[src];
            DsServer destDC = (DsServer)EnvironmentConfig.MachineStore[dest];
            string dest_sidhistory = null;
            bool need_dest_sidhistory = false;
            switch ((DRS_ADDSID_FLAGS)inMessage.Value.V1.Flags)
            {
                case DRS_ADDSID_FLAGS.NONE:
                    string src_sid = null;
                    if (srcNameType == ObjectNameType.SAM)
                    {
                        string dn = LdapUtility.GetObjectDNFromSAMName(srcDC, srcObjDn);
                        src_sid = LdapUtility.GetObjectStringSid(srcDC, dn);
                    }
                    else
                        src_sid = LdapUtility.GetObjectStringSidHistory(srcDC, srcObjDn);
                    srcSid = src_sid;

                    need_dest_sidhistory = true;
                    break;

                case DRS_ADDSID_FLAGS.DS_ADDSID_FLAG_PRIVATE_DEL_SRC_OBJ:
                    need_dest_sidhistory = true;
                    Guid? guid = LdapUtility.GetObjectGuid(srcDC, srcObjDn);
                    testSite.Assert.IsNull(guid, "source object should be deleted");
                    break;
                default:
                    testSite.Assert.Fail("the Flag should not be set to other other conditions not in TD");
                    break;
            }

            if (need_dest_sidhistory)
            {
                if (destNameType == ObjectNameType.SAM)
                {
                    string dn = LdapUtility.GetObjectDNFromSAMName(destDC, destObjDn);
                    dest_sidhistory = LdapUtility.GetObjectStringSidHistory(destDC, dn);
                }
                else
                    dest_sidhistory = LdapUtility.GetObjectStringSidHistory(destDC, destObjDn);
            }


            testSite.Assert.AreEqual<string>(srcSid, dest_sidhistory, "SID of source object should be added into destination object");


            return;
        }

        /// <summary>
        /// verify DrsGetReplInfo API
        /// </summary>
        /// <param name="dwInVersion">request message version</param>
        /// <param name="inMessage">request message</param>
        /// <param name="outVersion">reply message version</param>
        /// <param name="outMessage">reply message</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.UInt32.ToString")]
        public void VerifyDRSWriteSPN(
            dwInVersion_Values dwInVersion,
            DRS_MSG_SPNREQ? inMessage,
            pdwOutVersion_Values? outVersion,
            DRS_MSG_SPNREPLY? outMessage)
        {
            testSite.Assert.AreEqual<dwInVersion_Values>(dwInVersion, dwInVersion_Values.V1, "Only V1 DRSWriteSPN request message is supported");
            testSite.Assert.IsNotNull(outVersion, "DRSWriteSPN reply message version should not be null");
            testSite.Assert.AreEqual<pdwOutVersion_Values>(outVersion.Value, pdwOutVersion_Values.V1, "Only V1 DRSWriteSPN reply message is supported");
            testSite.Assert.IsNotNull(outMessage, "DRSWriteSPN reply message should not be null");

            testSite.Assert.IsTrue(outMessage.Value.V1.retVal == 0, "DRSWriteSPN failed with windows error code " + outMessage.Value.V1.retVal.ToString());
            if (inMessage.Value.V1.operation == (uint)DS_SPN_OPREATION.DS_SPN_REPLACE_SPN_OP && inMessage.Value.V1.cSPN == 0)
                testSite.Assert.IsNull(inMessage.Value.V1.rpwszSPN, "All SPNs should be removed if operation is DS_SPN_REPLACE_SPN_OP and cSPN in request is NULL");
            else
                testSite.Assert.AreEqual<int>((int)inMessage.Value.V1.cSPN, inMessage.Value.V1.rpwszSPN.Length, "rpwszSPN array of DRS_MSG_SPNREQ_V1 should have cSPN to set the size");

            return;
        }

        /// <summary>
        /// Verify DrsRemoveDsServer results.
        /// </summary>
        /// <param name="machine"></param>
        /// <param name="dwInVersion"></param>
        /// <param name="req"></param>
        /// <param name="outVersion"></param>
        /// <param name="reply"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", MessageId = "System.String.IndexOf(System.String)")]
        public void VerifyDrsRemoveDsServer(
            uint retVal,
            EnvironmentConfig.Machine machine,
            IDL_DRSRemoveDsServer_dwInVersion_Values dwInVersion,
            DRS_MSG_RMSVRREQ req,
            IDL_DRSRemoveDsServer_pdwOutVersion_Values? outVersion,
            DRS_MSG_RMSVRREPLY? reply)
        {
            testSite.Assert.AreEqual<uint>(0, retVal,
                "IDL_DRSRemoveDsDomain: Checking return value - got: {0}, expect: {1}, return value should be 0 for successful operation",
                retVal, 0);
            bool isLastDc = false;
            // IDL_DRSRemoveDsServer supports only V1
            testSite.Assert.IsTrue(outVersion == IDL_DRSRemoveDsServer_pdwOutVersion_Values.V1,
                "IDL_DRSRemoveDsServer: Checking outVersion - got: {0}, expect: {1}, outVersion should be 1",
                outVersion, 1);

            DsServer srv = (DsServer)EnvironmentConfig.MachineStore[machine];

            string domainDn = req.V1.DomainDN;
            string serverDn = req.V1.ServerDN;

            string[] dcs = ldapAd.ListDCNamesInDomain(srv, domainDn);
            bool dsaObjExist = ldapAd.IsObjectExist(srv, "CN=NTDS Settings," + serverDn);
            if (req.V1.fCommit == 1)
            {
                // Commit removal
                if (dcs == null || dcs.Length == 0)
                {
                    // If the server is the last DC in domain, after removal
                    // dcs should be empty.
                    isLastDc = true;
                }

                // To verify: the NTDSDSA object of the server is removed.
                testSite.Assert.IsFalse(dsaObjExist,
                    "IDL_DRSRemoveDsServer: NTDS DSA object of {0} should be removed when fCommit is true.", serverDn);
            }
            else
            {
                // Check if the server in serverDn is the last DC in the domain.
                string t = serverDn.Split(',')[0];
                string serverName = t.Substring(t.IndexOf("=") + 1).Trim();

                if (dcs.Length == 1)
                {
                    string dc = dcs[0].Split(',')[0];
                    string s = dc.Substring(dc.IndexOf("=") + 1).Trim();
                    if (s == serverName)
                        isLastDc = true;
                }

                // Just verification, no removal is performed.
                // To verify: the NTDSDSA object of the server is not removed.
                testSite.Assert.IsTrue(dsaObjExist,
                    "IDL_DRSRemoveDsServer: NTDS DSA object of {0} should not be removed when fCommit is false.", serverDn);
            }

            testSite.Assert.IsTrue(isLastDc == (reply.Value.V1.fLastDcInDomain == 1),
                "IDL_DRSRemoveDsServer: LDAP indicate {0} is {1} the last DC in domain {2},"
                + "however IDL_DRSRemoveDsServer shows {0} is {3} the last DC in domain {2}",
                serverDn,
                (isLastDc == true) ? "" : "not",
                domainDn,
                (reply.Value.V1.fLastDcInDomain == 1) ? "" : "not");
        }


        /// <summary>
        /// Verify DrsRemoveDsDomain results.
        /// </summary>
        /// <param name="machine"></param>
        /// <param name="dwInVersion"></param>
        /// <param name="req"></param>
        /// <param name="outVersion"></param>
        /// <param name="reply"></param>
        public void VerifyDrsRemoveDsDomain(
            uint retVal,
            EnvironmentConfig.Machine machine,
            IDL_DRSRemoveDsDomain_dwInVersion_Values dwInVersion,
            DRS_MSG_RMDMNREQ req,
            IDL_DRSRemoveDsDomain_pdwOutVersion_Values? outVersion,
            DRS_MSG_RMDMNREPLY? reply)
        {
            testSite.Assert.AreEqual<uint>(0, retVal, "IDL_DRSRemoveDsDomain should return 0 for successful operation");
            DsServer sut = (DsServer)EnvironmentConfig.MachineStore[machine];
            // IDL_DRSRemoveDsServer supports only V1
            testSite.Assert.IsTrue(outVersion == IDL_DRSRemoveDsDomain_pdwOutVersion_Values.V1,
                "IDL_DRSRemoveDsDomain: outVersion should be 1");

            if (sut.IsWindows)
                testSite.Assert.IsTrue(reply.Value.V1.Reserved == 0,
                    "IDL_DRSRemoveDsDomain: Reserved field must be 0.");

            DsServer srv = (DsServer)EnvironmentConfig.MachineStore[machine];
            string domainDn = req.V1.DomainDN;

            RootDSE rootDse = LdapUtility.GetRootDSE(srv);
            SearchResultEntryCollection results = null;
            ResultCode re = ldapAd.Search(
                srv,
                "CN=Partitions," + rootDse.configurationNamingContext,
                "(&(objectClass=crossRef)(nCName=" + domainDn + "))",
                SearchScope.Subtree,
                null,
                out results);

            testSite.Assert.AreEqual<int>(
                0,
                results.Count,
                "IDL_DRSRemoveDsDomain: The crossRef object should not exist."
                );
        }

        #endregion

        #region LDS Demotion
        /// <summary>
        /// verification LDS demotion init result
        /// </summary>
        /// <param name="retVal">return value</param>
        public void VerifyDRSInitDemotion(uint retVal)
        {
            testSite.Assert.AreEqual<uint>(0, retVal, "IDL_DRSInitDemotion should return 0x0 for successful operation");
        }

        /// <summary>
        /// Verify LDS demotion result
        /// </summary>
        /// <param name="retVal">return value</param>
        /// <param name="svr">lds target</param>
        /// <param name="ops">operations</param>
        /// <param name="verificationPartner">partner lds instance for verification</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", MessageId = "System.String.IndexOf(System.String)")]
        public void VerifyDRSFinishDemotion(uint retVal, EnvironmentConfig.Machine svr, DRS_MSG_FINISH_DEMOTIONREQ_OPERATIONS ops, EnvironmentConfig.Machine verificationPartner)
        {
            testSite.Assert.AreEqual<uint>(0, retVal, "IDL_DRSFinishDemotion should return 0x0 for successful operation");
            AdldsServer server = (AdldsServer)EnvironmentConfig.MachineStore[svr];

            if ((uint)(ops & DRS_MSG_FINISH_DEMOTIONREQ_OPERATIONS.DS_DEMOTE_ROLLBACK_DEMOTE) > 0)
                return;

            if ((uint)(ops & DRS_MSG_FINISH_DEMOTIONREQ_OPERATIONS.DS_DEMOTE_UNREGISTER_SPNS) > 0)
            {
                string[] spns = ldapAd.GetServicePrincipalName(EnvironmentConfig.MainDC);
                bool passed = true;
                foreach (string str in spns)
                {
                    if (str.Contains(":" + server.Port))
                    {
                        passed = false;
                        break;
                    }
                }


                testSite.Assert.IsTrue(passed, "Server should not contains SPN for this LDS Instance after demotion if DS_DEMOTE_UNREGISTER_SPNS flag is set");

            }

            if ((uint)(ops & DRS_MSG_FINISH_DEMOTIONREQ_OPERATIONS.DS_DEMOTE_UNREGISTER_SCPS) > 0)
            {
                SearchResultEntryCollection srec = null;
                ldapAd.Search(EnvironmentConfig.MainDC, "CN=" + EnvironmentConfig.MainDC.NetbiosName + ",OU=Domain Controllers," + EnvironmentConfig.MainDC.Domain.Name, "(objectclass=serviceconnectionpoint)", SearchScope.Subtree, null, out srec);
                bool passed = true;
                foreach (SearchResultEntry sre in srec)
                {
                    if (sre.DistinguishedName.Contains(server.InstanceID.ToString()))
                    {
                        passed = false;
                        break;
                    }
                }


                testSite.Assert.IsTrue(passed, "Server should not contains SCP for this LDS Instance after demotion if DS_DEMOTE_UNREGISTER_SCPS flag is set");
            }

            if ((uint)(ops & DRS_MSG_FINISH_DEMOTIONREQ_OPERATIONS.DS_DEMOTE_DELETE_CSMETA) > 0 &&
                (
                verificationPartner != EnvironmentConfig.Machine.None
                && verificationPartner != EnvironmentConfig.Machine.InvalidLDSDC
                && verificationPartner != EnvironmentConfig.Machine.Endpoint
                ))
            {
                bool deleted = false;
                try
                {
                    SearchResultEntryCollection srec = null;
                    ldapAd.Search((AdldsServer)EnvironmentConfig.MachineStore[verificationPartner],
                        "CN=NTDS Settings," + server.ServerObjectName.Substring(0, server.ServerObjectName.IndexOf(",")) + ",CN=Servers," + ((AdldsServer)EnvironmentConfig.MachineStore[verificationPartner]).Site.DN,
                        "(objectclass=nTDSDSA)",
                         SearchScope.Subtree,
                         new string[] { "cn" },
                         out srec);
                    if (srec == null)
                        deleted = true;
                }
                catch
                {
                    deleted = true;
                }

                testSite.Assert.IsTrue(deleted, "When DS_DEMOTE_DELETE_CSMETA flag is set, other replication partners should remove the nTDSDSA objects of this removed LDS instance");
            }
        }

        #endregion

        #region replica demotion
        /// <summary>
        /// verify IDL_DRSReplicaDemotion result
        /// </summary>
        /// <param name="retVal">return value</param>
        /// <param name="svr">target</param>
        /// <param name="ncType">nc type</param>
        /// <param name="reply">DRS_MSG_REPLICA_DEMOTIONREPLY</param>
        /// <param name="partner">replica partner</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1304:SpecifyCultureInfo", MessageId = "System.String.ToLower")]
        public void VerifyDRSReplicaDemotion(uint retVal, EnvironmentConfig.Machine svr, NamingContext ncType, DRS_MSG_REPLICA_DEMOTIONREPLY reply, EnvironmentConfig.Machine partner)
        {
            testSite.Assert.AreEqual<uint>(0, retVal, "IDL_DRSReplicaDemotion should return 0x0 for successful operation");
            testSite.Assert.AreEqual<uint>(0, reply.V1.dwOpError, "DRS_MSG_REPLICA_DEMOTIONREPLY.V1.dwOpError should be set to 0x0");

            DsServer p = (DsServer)EnvironmentConfig.MachineStore[partner];
            DsServer target = (DsServer)EnvironmentConfig.MachineStore[svr];
            DSNAME nc = new DSNAME();
            string owner = null;
            if (ncType == NamingContext.SchemaNC)
            {
                nc = target.Domain.SchemaNC;
                owner = ldapAd.GetAttributeValueInString(p, LdapUtility.ConvertUshortArrayToString(nc.StringName), "fSMORoleOwner");
            }
            else if (ncType == NamingContext.ConfigNC)
            {
                nc = target.Domain.ConfigNC;
                owner = ldapAd.GetAttributeValueInString(p, "cn=partitions," + LdapUtility.ConvertUshortArrayToString(nc.StringName), "fSMORoleOwner");
            }
            else if (ncType == NamingContext.DomainNC)
            {
                nc = ((AddsDomain)target.Domain).DomainNC;
                owner = ldapAd.GetAttributeValueInString(p, LdapUtility.ConvertUshortArrayToString(nc.StringName), "fSMORoleOwner");

            }
            testSite.Assert.IsFalse(owner.ToLower().Contains(target.ServerObjectName.ToLower()), "server should abandon its fsmo roles for successful IDL_DRSReplicaDemotion");
        }
        #endregion

        #region dsaop RPC methods

        /// <summary>
        /// Wrapper for IDL_DSAPrepareScript.
        /// </summary>
        /// <param name="outVersion">A pointer to the version of response message.</param>
        /// <param name="outMessage">A pointer to the response message.</param>
        public void VerifyDsaPrepareScript(uint? outVersion, DSA_MSG_PREPARE_SCRIPT_REPLY? outMessage)
        {
            this.testSite.Assert.IsTrue(outMessage.HasValue, "[IDL_DSAPrepareScript] Server should reply to the request.");
            this.testSite.Assert.AreEqual<uint>(1, outVersion.Value, "[IDL_DSAPrepareScript] Returned message should be version 1.");

            DSA_MSG_PREPARE_SCRIPT_REPLY reply = outMessage.Value;

            //Verify cbPassword equals the length of pbPassword.
            if (reply.V1.pbPassword != null)
            {
                this.testSite.Assert.AreEqual<uint>((uint)reply.V1.pbPassword.Length, reply.V1.cbPassword, "[IDL_DSAPrepareScript] cbPassword: The count, in bytes, of the pbPassword array.");
            }
            else
            {
                this.testSite.Assert.AreEqual<uint>(0, reply.V1.cbPassword, "[IDL_DSAPrepareScript] cbPassword: The count, in bytes, of the pbPassword array.");
            }

            //Verify cbHashBody equals the length of pbHashBody.
            if (reply.V1.pbHashBody != null)
            {
                this.testSite.Assert.AreEqual<uint>((uint)reply.V1.pbHashBody.Length, reply.V1.cbHashBody, "[IDL_DSAPrepareScript] cbHashBody: The count, in bytes, of the pbHashBody array.");
            }
            else
            {
                this.testSite.Assert.AreEqual<uint>(0, reply.V1.cbHashBody, "[IDL_DSAPrepareScript] cbHashBody: The count, in bytes, of the pbHashBody array.");
            }

            //Verify cbHashSignature equals the length of pbHashSignature.
            if (reply.V1.pbHashSignature != null)
            {
                this.testSite.Assert.AreEqual<uint>((uint)reply.V1.pbHashSignature.Length, reply.V1.cbHashSignature, "[IDL_DSAPrepareScript] cbHashSignature: The count, in bytes, of the pbHashSignature array.");
            }
            else
            {
                this.testSite.Assert.AreEqual<uint>(0, reply.V1.cbHashSignature, "[IDL_DSAPrepareScript] cbHashSignature: The count, in bytes, of the pbHashSignature array.");
            }
        }

        /// <summary>
        /// IDL_DSAExecuteScript 
        /// </summary>
        /// <param name="outVersion">A pointer to the version of response message.</param>
        /// <param name="outMessage">A pointer to the response message.</param>
        public void VerifyDsaExecuteScript(uint? outVersion, DSA_MSG_EXECUTE_SCRIPT_REPLY? outMessage)
        {
            this.testSite.Assert.IsTrue(outMessage.HasValue, "[IDL_DSAExecuteScript] Server should reply to the request.");
            this.testSite.Assert.AreEqual<uint>(1, outVersion.Value, "[IDL_DSAExecuteScript] Returned message should be version 1.");
        }

        #endregion

        #region Lookups
        /// <summary>
        /// Verify DrsGetMemberships results.
        /// </summary>
        /// <param name="machine"></param>
        /// <param name="dwInVersion"></param>
        /// <param name="req"></param>
        /// <param name="outVersion"></param>
        /// <param name="reply"></param>
        public void VerifyDrsGetMemberships(
            EnvironmentConfig.Machine machine,
            uint dwInVersion,
            DRS_MSG_REVMEMB_REQ_V1 req,
            uint? outVersion,
            DRS_MSG_REVMEMB_REPLY_V1? reply)
        {
            // IDL_DrsGetMembership supports only V1
            testSite.Assert.IsTrue(outVersion == 1, "IDL_DRSGetMemberships: Checking outVersion - got: {0}, expect: {1}, outVersion should be 1", outVersion, 1);

            const uint DRS_REVMEMB_FLAG_GET_ATTRIBUTES = 0x00000001;
            bool isGetAttribute = (req.dwFlags == DRS_REVMEMB_FLAG_GET_ATTRIBUTES);

            REVERSE_MEMBERSHIP_OPERATION_TYPE opType = req.OperationType;

            DsServer srv = (DsServer)EnvironmentConfig.MachineStore[machine];
            testSite.Assert.IsTrue(reply.HasValue, "reply cannot be null");

            int unknownUserCount = 0;
            for (int i = 0; i < req.cDsNames; ++i)
            {
                DSNAME name = req.ppDsNames[i][0];
                string groupName = DrsrHelper.ConvertUshortArrayToString(name.StringName);
                testSite.Log.Add(LogEntryKind.Checkpoint, "Verifying members of group: " + groupName);
                DSNAME? limitingDomain = req.pLimitingDomain;

                if (reply.Value.cDsNames > 0)
                {
                    for (int k = 0; k < reply.Value.ppDsNames.Length; k++)
                    {
                        byte[] sidData = new byte[reply.Value.ppDsNames[k][0].SidLen];
                        Array.Copy(reply.Value.ppDsNames[k][0].Sid.Data, sidData, reply.Value.ppDsNames[k][0].SidLen);
                        SearchResultEntryCollection srec = null;
                        string sidHexString = "\\" + BitConverter.ToString(sidData).Replace('-', '\\');
                        ldapAd.Search(srv, DrsrHelper.GetDNFromFQDN(Common.ADCommonServerAdapter.Instance(testSite).PrimaryDomainDnsName), "(objectSid=" + sidHexString + ")", SearchScope.Subtree, new string[] { "cn" }, out srec);
                        if (srec != null && srec.Count > 0)
                        {
                            testSite.Log.Add(LogEntryKind.Checkpoint, "Reply returned user: " + srec[0].DistinguishedName);
                        }
                        else
                        {
                            unknownUserCount++;
                            testSite.Log.Add(LogEntryKind.Checkpoint, "Reply returned unknown user with sid: " + sidHexString);
                        }
                    }
                }

                DSNAME[] memberOfs = ldapAd.GetMemberships(srv, name, opType, limitingDomain);
                foreach (DSNAME dsn in memberOfs)
                {
                    string dsnName = DrsrHelper.ConvertUshortArrayToString(dsn.StringName);
                    testSite.Log.Add(LogEntryKind.Checkpoint, "Ldap searched member: " + dsnName);
                }
                if (unknownUserCount > 0)
                    testSite.Log.Add(LogEntryKind.Checkpoint, "the number will be different because some user cannnot be accessed due to ACE issue");
                testSite.Assert.IsTrue(
                    memberOfs.Length == (reply.Value.cDsNames - unknownUserCount),
                    "IDL_DRSGetMemberships: membership number should equal, expect:{0}, got:{1}",
                    memberOfs.Length,
                    reply.Value.cDsNames);
            }
        }


        /// <summary>
        /// Verify DrsGetMemberships2 results.
        /// </summary>
        /// <param name="machine"></param>
        /// <param name="dwInVersion"></param>
        /// <param name="req"></param>
        /// <param name="outVersion"></param>
        /// <param name="reply"></param>
        public void VerifyDrsGetMemberships2(
            EnvironmentConfig.Machine machine,
            uint dwInVersion,
            DRS_MSG_GETMEMBERSHIPS2_REQ req,
            uint? outVersion,
            DRS_MSG_GETMEMBERSHIPS2_REPLY? reply)
        {
            // IDL_DRSGetMemberships2 supports only V1
            testSite.Assert.IsTrue(outVersion == 1,
                           "IDL_DRSGetMemberships2: Checking outVersion - got: {0}, expect: {1}, outVersion should be 1.", outVersion, 1);

            DsServer srv = (DsServer)EnvironmentConfig.MachineStore[machine];

            const uint totalResponses = 7;
            testSite.Assert.IsTrue(
                totalResponses == reply.Value.V1.Count,
                 "IDL_DRSGetMemberships2: Checking totalResponses - got: {0}, expect: {1}, the response should have 7 entries.",
                 reply.Value.V1.Count, totalResponses
                );
            for (int i = 0; i < reply.Value.V1.Count; ++i)
            {
                // Verify each GetMemberships response.
                VerifyDrsGetMemberships(
                    machine,
                    dwInVersion,
                    req.V1.Requests[i],
                    outVersion,
                    reply.Value.V1.Replies[i]
                    );
            }
        }

        /// <summary>
        /// Verify DrsVerifyNames results.
        /// </summary>
        /// <param name="machine"></param>
        /// <param name="dwInVersion"></param>
        /// <param name="req"></param>
        /// <param name="outVersion"></param>
        /// <param name="reply"></param>
        /// <param name="objectDNs">The DNs of the object to be verified.</param>
        public void VerifyDrsVerifyNames(
            EnvironmentConfig.Machine machine,
            uint dwInVersion,
            DRS_MSG_VERIFYREQ req,
            uint? outVersion,
            DRS_MSG_VERIFYREPLY? reply,
            string[] objectDNs
            )
        {
            // IDL_DRSVerifyNames supports only V1
            testSite.Assert.IsTrue(outVersion == 1, "IDL_DRSVerifyNames: Checking outVersion - got: {0}, expect: {1}, outVersion should be 1.", outVersion, 1);
            DsServer srv = (DsServer)EnvironmentConfig.MachineStore[machine];

            DRS_MSG_VERIFYREQ_V1_dwFlags_Values flag = (DRS_MSG_VERIFYREQ_V1_dwFlags_Values)req.V1.dwFlags;

            testSite.Assert.IsTrue(
                reply.Value.V1.cNames == 1,
                "IDL_DRSVerifyNames: Checking cNames - got: {0}, expect: {1}, only 1 DSName being verified.",
                reply.Value.V1.cNames, 1
            );

            if (srv.IsWindows)
                testSite.Assert.AreEqual<error_Values>(error_Values.V1, reply.Value.V1.error, "Windows MUST set error in reply structure to 0");

            for (int k = 0; k < reply.Value.V1.cNames; ++k)
            {
                // Check the DNs first.
                string actualDn = LdapUtility.ConvertUshortArrayToString(reply.Value.V1.rpEntInf[k].pName.Value.StringName);

                testSite.Assert.IsTrue(
                    DrsrHelper.AreDNsSame(actualDn, objectDNs[k]),
                    "IDL_DRSVerifyNames: given DN and verified results should be the same."
                    );

                switch (flag)
                {
                    case DRS_MSG_VERIFYREQ_V1_dwFlags_Values.DRS_VERIFY_DSNAMES:
                        {
                            Guid guid = req.V1.rpNames[k][0].Guid;
                            Guid actualGuid = reply.Value.V1.rpEntInf[k].pName.Value.Guid;

                            testSite.Assert.IsTrue(
                                actualGuid == guid,
                                "IDL_DRSVerifyNames: Checking GUIDs - got: {0}, expect: {1}, GUIDs of the DSNames should be the same.",
                                actualGuid, guid
                                );
                            // We didn't set the SID for the object.
                            break;
                        }
                    case DRS_MSG_VERIFYREQ_V1_dwFlags_Values.DRS_VERIFY_FPOS:
                        goto default;
                    case DRS_MSG_VERIFYREQ_V1_dwFlags_Values.DRS_VERIFY_SIDS:
                        goto default;
                    case DRS_MSG_VERIFYREQ_V1_dwFlags_Values.DRS_VERIFY_SAM_ACCOUNT_NAMES:
                        // We didn't set the SID for the object.
                        break;
                    default:
                        {
                            for (int i = 0; i < 28; ++i)
                            {
                                testSite.Assert.IsTrue(
                                    req.V1.rpNames[k][0].Sid.Data[i] == reply.Value.V1.rpEntInf[k].pName.Value.Sid.Data[i],
                                    "IDL_DRSVerifyNames: Checking SIDs - got: {0}, expect: {1}, SIDs of the DSNames should be the same.",
                                    req.V1.rpNames[k][0].Sid.Data[i], reply.Value.V1.rpEntInf[k].pName.Value.Sid.Data[i]
                                    );
                            }
                        }
                        break;
                }
            }

            if (reply.Value.V1.cNames == 1)
            {
                // Verify attribute-OID
                string rdnAttrId = DRSConstants.RDN_OID;
                SCHEMA_PREFIX_TABLE prefixTable = reply.Value.V1.PrefixTable;
                testSite.Assert.IsTrue(
                    prefixTable.PrefixCount > 0,
                    "IDL_DRSVerifyNames: the prefix table should containing entries."
                    );

                ATTRBLOCK attrBlock = reply.Value.V1.rpEntInf[0].AttrBlock;
                testSite.Assert.IsTrue(
                    attrBlock.attrCount == 1,
                    "IDL_DRSVerifyNames: Checking Attribute Count- got: {0}, expect: {1}, only 1 attribute being returned.",
                    attrBlock.attrCount, 1
                    );

                uint attrTyp = attrBlock.pAttr[0].attrTyp;
                string actualAttrId = OIDUtility.OidFromAttrid(
                    prefixTable,
                    attrTyp
                    );

                testSite.Assert.IsTrue(
                    actualAttrId == rdnAttrId,
                    "IDL_DRSVerifyNames: Checking attribute OID - got: {0}, expect: {1}, attribute OID should be the same.",
                    actualAttrId, rdnAttrId
                    );

                // Verify attribute value
                string rdn = System.Text.Encoding.Unicode.GetString(
                    attrBlock.pAttr[0].AttrVal.pAVal[0].pVal
                    );

                // Get RDN Ldap value
                string rdnLdap = ldapAd.GetAttributeValueInString(srv, objectDNs[0], "name");

                testSite.Assert.IsTrue(
                    DrsrHelper.AreDNsSame(rdnLdap, rdn),
                    "IDL_DRSVerifyNames: attribute RDN should be the same."
                    );
            }
            else
            {
                testSite.Assert.IsNull(
                    reply.Value.V1.rpEntInf,
                    "IDL_DRSVerifyNames: Checking rpEntInf - got: {0}, expect: null, rpEntInf should be null when more than one name is being verified.",
                    reply.Value.V1.rpEntInf
                    );
            }
        }


        #endregion


        #region dc clone
        /// <summary>
        /// verify IDL_DRSAddCloneDC result
        /// </summary>
        /// <param name="retVal">return value</param>
        /// <param name="svr">target dc</param>
        /// <param name="machinename">cloned dc name</param>
        /// <param name="sitename">site where the cloned dc is</param>
        public void VerifyDRSAddCloneDC(uint retVal, EnvironmentConfig.Machine svr, string machinename, string sitename)
        {
            testSite.Assert.AreEqual<uint>(0, retVal, "IDL_DRSAddCloneDC should return 0x0 for a successful operation");
            DsServer holder = (DsServer)EnvironmentConfig.MachineStore[svr];
            SearchResultEntryCollection srec = null;
            bool machineObjCreated = false;

            if (ResultCode.Success == ldapAd.Search(holder, holder.ServerObjectName.Replace(holder.NetbiosName, machinename), "(objectClass=*)", SearchScope.Base, null, out srec))
            {
                if (srec.Count > 0)
                    machineObjCreated = true;
            }

            testSite.Assert.IsTrue(machineObjCreated, "A new machine account should be created if DC is cloned");

            bool ntdsObjCreated = false;

            if (ResultCode.Success == ldapAd.Search(holder, holder.NtdsDsaObjectName.Replace(holder.NetbiosName, machinename), "(objectClass=*)", SearchScope.Base, null, out srec))
            {
                if (srec.Count > 0)
                    ntdsObjCreated = true;
            }

            testSite.Assert.IsTrue(ntdsObjCreated, "A new ntds account should be created if DC is cloned");
        }
        #endregion

        #region NGC Key
        /// <summary>
        /// verify DRSWriteNgcKey API
        /// </summary>
        /// <param name="dwInVersion">request message version</param>
        /// <param name="inMessage">request message</param>
        /// <param name="outVersion">reply message version</param>
        /// <param name="outMessage">reply message</param>
        public void VerifyDRSWriteNgcKey(
            uint dwInVersion,
            DRS_MSG_WRITENGCKEYREQ? inMessage,
            uint? outVersion,
            DRS_MSG_WRITENGCKEYREPLY? outMessage)
        {
            testSite.Assert.AreEqual<uint>(1, dwInVersion, "Only V1 DRSWriteNgcKey request message is supported");
            testSite.Assert.IsNotNull(outVersion, "DRSWriteNgcKey reply message version should not be null");
            testSite.Assert.AreEqual<uint>(1, (uint)outVersion, "Only V1 DRSWriteNgcKey reply message is supported");
            testSite.Assert.IsNotNull(outMessage, "DRSWriteNgcKey reply message should not be null");

            testSite.Assert.AreEqual<uint>(0, outMessage.Value.V1.retVal, "DRSWriteNgcKey failed with windows error code " + outMessage.Value.V1.retVal.ToString());
        }

        /// <summary>
        /// verify DRSReadNgcKey API
        /// </summary>
        /// <param name="dwInVersion">request message version</param>
        /// <param name="inMessage">request message</param>
        /// <param name="outVersion">reply message version</param>
        /// <param name="outMessage">reply message</param>
        public void VerifyDRSReadNgcKey(
            uint dwInVersion,
            DRS_MSG_READNGCKEYREQ? inMessage,
            uint? outVersion,
            DRS_MSG_READNGCKEYREPLY? outMessage)
        {
            testSite.Assert.AreEqual<uint>(1, dwInVersion, "Only V1 DRSReadNgcKey request message is supported");
            testSite.Assert.IsNotNull(outVersion, "DRSReadNgcKey reply message version should not be null");
            testSite.Assert.AreEqual<uint>(1, (uint)outVersion, "Only V1 DRSReadNgcKey reply message is supported");
            testSite.Assert.IsNotNull(outMessage, "DRSReadNgcKey reply message should not be null");

            testSite.Assert.AreEqual<uint>(0, outMessage.Value.V1.retVal, "DRSReadNgcKey failed with windows error code " + outMessage.Value.V1.retVal.ToString());
        }
        #endregion


        /// <summary>
        /// Verification method for IDL_DRSInterDomainMove.
        /// </summary>
        /// <param name="reqVer">The request version.</param>
        /// <param name="repVer">The reply version.</param>
        /// <param name="reply">The response message.</param>
        /// <param name="ret">The return value.</param>
        public void VerifyDrsInterDomainMove(DRSInterDomainMove_Versions reqVer, uint repVer, DRS_MSG_MOVEREPLY? reply, uint ret)
        {
            testSite.Assert.AreEqual<uint>((uint)2, repVer, "[IDL_DRSInterDomainMove] The response version should be 2.");
            testSite.Assert.IsTrue(reply.HasValue, "[IDL_DRSInterDomainMove] Server should respond to request.");
            testSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSInterDomainMove should return 0x0 for successfully-validated operation");

            if (reqVer == DRSInterDomainMove_Versions.V1)
            {
                testSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_GENERIC_ERROR, reply.Value.V2.win32Error, "[IDL_DRSInterDomainMove] Server should set the win32error code to ERROR_DS_GENERIC_ERROR if request version is 1.");
                testSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_INVALID_PARAMETER, ret, "[IDL_DRSInterDomainMove] Server should set the return value to ERROR_DS_DRA_INVALID_PARAMETER if request version is 1.");
            }
            else if (reqVer == DRSInterDomainMove_Versions.V2)
                testSite.Assert.AreEqual<uint>(0, reply.Value.V2.win32Error, "win32Error should be 0x0 for a successful move operation");
        }

    }
}

