// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.DirectoryServices.Protocols;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    /// <summary>
    /// Test class for replication methods.
    /// </summary>
    [TestClass]
    public class ReplicationTest : DrsrTestClassBase
    {
        #region Class Initialization and Cleanup
        [ClassInitialize]
        public static new void ClassInitialize(TestContext context)
        {
            DrsrTestClassBase.BaseInitialize(context);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            DrsrTestClassBase.BaseCleanup();
        }
        #endregion

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            SetTTTMachines(new EnvironmentConfig.Machine[] { EnvironmentConfig.Machine.PDC, EnvironmentConfig.Machine.WritableDC2 });
            base.TestInitialize();
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }
        #endregion

        #region IDL_DRSGetReplInfo
        [Priority(0)]
        [BVT]
        [TestCategory("Win2000")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSGetReplInfo with infoType DS_REPL_INFO_NEIGHBORS to find replication neighbors for a specified NC.")]
        public void DRSR_DRSGetReplInfo_V1_Find_Neighbors_With_SpecifiedNC()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            NamingContext specifiedNC = NamingContext.SchemaNC;
            DsServer dcMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];
            string ncDN = DrsrHelper.GetNamingContextDN(dcMachine.Domain, specifiedNC);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "IDL_DRSGetReplInfo: Searching the replication partners for config NC of {0}", dcServer);
            REPS_FROM[] rfs = ldapAdapter.GetRepsFrom(dcMachine, specifiedNC);
            BaseTestSite.Assert.IsNotNull(rfs, "RepsFrom attribute should not be null.");
            for (int i = 0; i < rfs.Length; i++)
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "IDL_DRSGetReplInfo: Replication partner {0} of config NC is {1}.", i, rfs[i].uuidDsaObj);

                //convert for RepNbrOptionToDra limitation

                rfs[i].ulReplicaFlags = DrsrUtility.ConvertRepFlagsToRepNbrOption(rfs[i].ulReplicaFlags);
            }

            BaseTestSite.Log.Add(LogEntryKind.Comment, "IDL_DRSBind: Binding to DC server: {0}", dcServer);
            uint ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind: Checking return value - got: {0}, expect: 0, should return 0 with a success bind to DC", ret);

            uint? outVersion;
            DRS_MSG_GETREPLINFO_REPLY? outMessage;
            ret = drsTestClient.DrsGetReplInfo(
                dcServer,
                DrsGetReplInfo_Versions.V1,
                EnvironmentConfig.Machine.None,
                DS_REPL_INFO.DS_REPL_INFO_NEIGHBORS,
                ncDN,
                //null,
                null, null,
                out outVersion, out outMessage);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSGetReplInfo: Checking return value - got: {0}, expect: 0, should return 0 if successful.", ret);
            BaseTestSite.Assert.AreEqual<uint>((uint)DS_REPL_INFO.DS_REPL_INFO_NEIGHBORS, outVersion.Value,
                "IDL_DRSGetReplInfo: Checking the outVersion - got: {0}, expect: {1}", outVersion.Value, (uint)DS_REPL_INFO.DS_REPL_INFO_NEIGHBORS);

            //Verify replied replication partners are consistent with that of retrieved by LDAP. 
            DS_REPL_NEIGHBORSW nbs = outMessage.Value.pNeighbors[0];
            BaseTestSite.Assert.AreEqual<int>(rfs.Length, (int)nbs.cNumNeighbors,
                "IDL_DRSGetReplInfo: Checking if the count of the replication partners - {0} for Config NC is the same as that retrieved by LDAP - {1}",
                rfs.Length,
                (int)nbs.cNumNeighbors
                );

            foreach (DS_REPL_NEIGHBORW nb in nbs.rgNeighbor)
            {
                BaseTestSite.Assert.AreEqual<string>(ncDN, nb.pszNamingContext,
                    "IDL_DRSGetReplInfo: Checking the replied neighbor - got: {0}, expect: {1}", nb.pszNamingContext, ncDN);

                foreach (REPS_FROM rf in rfs)
                {
                    if (nb.uuidSourceDsaInvocationID.Equals(rf.uuidInvocId))
                    {
                        BaseTestSite.Assert.AreEqual<Guid>(
                            rf.uuidInvocId,
                            nb.uuidSourceDsaInvocationID,
                            "IDL_DRSGetReplInfo: Checking Invocation ID - got: {0}, expect: {1}",
                            nb.uuidSourceDsaInvocationID.ToString(),
                            rf.uuidInvocId.ToString());

                        BaseTestSite.Assert.AreEqual<Guid>(
                            rf.uuidDsaObj,
                            nb.uuidSourceDsaObjGuid,
                            "IDL_DRSGetReplInfo: Checking source DSA Guid - got: {0}, expect: {1}",
                            nb.uuidSourceDsaObjGuid.ToString(),
                            rf.uuidDsaObj.ToString()
                            );

                        BaseTestSite.Assert.AreEqual<Guid>(
                            rf.uuidTransportObj,
                            nb.uuidAsyncIntersiteTransportObjGuid,
                            "IDL_DRSGetReplInfo: Checking Transport object GUID - got: {0}, expect: {1}",
                            nb.uuidAsyncIntersiteTransportObjGuid.ToString(),
                            rf.uuidTransportObj.ToString()
                            );
                        if (bool.Parse(Site.Properties["MS_DRSR.TDI.67260"]))
                        {
                            BaseTestSite.Assert.AreEqual<uint>(
                                rf.ulReplicaFlags,
                                nb.dwReplicaFlags,
                                "IDL_DRSGetReplInfo: Checking replica flags - got: {0}, expect: {1}",
                                nb.dwReplicaFlags,
                                rf.ulReplicaFlags
                                );
                        }
                        break;
                    }
                }
            }
        }

        [BVT]
        [TestCategory("Win2003")]
        [Priority(0)]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSGetReplInfo with infoType DS_REPL_INFO_CURSORS_FOR_NC to query the replication state of specified NC.")]
        public void DRSR_DRSGetReplInfo_V2_Query_CURSORS_1()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            NamingContext specifiedNC = NamingContext.SchemaNC;
            DsServer dcMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];
            string ncDN = DrsrHelper.GetNamingContextDN(dcMachine.Domain, specifiedNC);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "IDL_DRSGetReplInfo: Searching the replUpToDateVector info for the Schema NC of {0}", dcServer);
            UPTODATE_VECTOR_V1_EXT utdVector = ldapAdapter.GetReplUTD(dcMachine, specifiedNC);
            BaseTestSite.Assert.AreNotEqual<uint>(0, utdVector.cNumCursors, "replUpToDateVector attribute should not be empty.");

            BaseTestSite.Log.Add(LogEntryKind.Comment, "IDL_DRSBind: Binding to DC server: {0}", dcServer);
            uint ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind: Checking return value - got: {0}, expect: 0, should return 0 with a success bind to DC", ret);

            uint? outVersion;
            DRS_MSG_GETREPLINFO_REPLY? outMessage;
            ret = drsTestClient.DrsGetReplInfo(
                dcServer,
                DrsGetReplInfo_Versions.V1,
                EnvironmentConfig.Machine.None,
                DS_REPL_INFO.DS_REPL_INFO_CURSORS_FOR_NC,
                ncDN,
                null, null,
                out outVersion, out outMessage);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSGetReplInfo: Checking return value - got: {0}, expect: 0, should return 0 if successful.", ret);
            BaseTestSite.Assert.AreEqual<uint>(
                (uint)DS_REPL_INFO.DS_REPL_INFO_CURSORS_FOR_NC,
                outVersion.Value,
                "IDL_DRSGetReplInfo: Checking outVersion - got: {0}, expect: {1}",
                outVersion.Value,
                (uint)DS_REPL_INFO.DS_REPL_INFO_CURSORS_FOR_NC);

            //Verify replied cursors are consistent with that of retrieved by LDAP. 
            DS_REPL_CURSORS cursors = outMessage.Value.pCursors[0];
            BaseTestSite.Assert.AreEqual<uint>(
                utdVector.cNumCursors,
                cursors.cNumCursors - 1,
                "IDL_DRSGetReplInfo: Checking the count of the cursors - got: {0}, expect: {1}",
                cursors.cNumCursors - 1,
                utdVector.cNumCursors);

            foreach (DS_REPL_CURSOR repliedCursor in cursors.rgCursor)
            {
                foreach (UPTODATE_CURSOR_V1 ldapCursor in utdVector.rgCursors)
                {
                    if (repliedCursor.uuidSourceDsaInvocationID.Equals(ldapCursor.uuidDsa))
                    {
                        BaseTestSite.Assert.AreEqual<long>(
                            repliedCursor.usnAttributeFilter,
                            ldapCursor.usnHighPropUpdate,
                            "IDL_DRSGetReplInfo: Checking the USN of the update on partner DC ({0}) - got: {1}, expect: {2}",
                            repliedCursor.uuidSourceDsaInvocationID,
                            repliedCursor.usnAttributeFilter,
                            ldapCursor.usnHighPropUpdate
                            );
                        break;
                    }
                }

            }
        }

        [TestCategory("Win2003")]
        [Priority(0)]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSGetReplInfo with infoType DS_REPL_INFO_METADATA_FOR_OBJ to query the replication metadata for a specific object")]
        public void DRSR_DRSGetReplInfo_V2_Query_Metadata_1_For_Obj()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            DsServer dcMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];

            DsUser admin = dcMachine.Domain.Admin;
            string existUser = string.Format(DRSTestData.DRSGetReplInfo_ExistUser, admin.Username);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "DRSTestData.DRSGetReplInfo_ExistUser: {0}", existUser);

            string ncDN = existUser + "," + LdapUtility.ConvertUshortArrayToString(((AddsDomain)dcMachine.Domain).DomainNC.StringName);
            LDAP_PROPERTY_META_DATA[] ncDNattributes = LdapUtility.GetMetaData(dcMachine, ncDN);


            BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServer);
            uint ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Call IDL_DRSGetReplInfo with infoType DS_REPL_INFO_METADATA_FOR_OBJ to query the replication state of specified NC.");
            uint? outVersion;
            DRS_MSG_GETREPLINFO_REPLY? outMessage;
            ret = drsTestClient.DrsGetReplInfo(
                dcServer,
                DrsGetReplInfo_Versions.V2,
                EnvironmentConfig.Machine.None,
                DS_REPL_INFO.DS_REPL_INFO_METADATA_FOR_OBJ,
                ncDN,
                null, null,
                out outVersion, out outMessage);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSGetReplInfo should return 0 if successful.");

            BaseTestSite.Assert.AreEqual<uint>((uint)DS_REPL_INFO.DS_REPL_INFO_METADATA_FOR_OBJ, outVersion.Value,
               "[IDL_DRSGetReplInfo] the reply version should be same as that specified by infoType of request.");

            //Verify replied meta data object are consistent with attributes of retrieved by LDAP.
            DS_REPL_OBJ_META_DATA metaDatas = outMessage.Value.pObjMetaData[0];
            DS_REPL_ATTR_META_DATA[] attremetaDatas = metaDatas.rgMetaData;
            BaseTestSite.Assert.IsNotNull(ncDNattributes, "LDAP_PROPERTY_META_DATA queried from LDAP should not be null");

            BaseTestSite.Assert.AreEqual<uint>((uint)ncDNattributes.Length, metaDatas.cNumEntries, "the reply metadata count of dn {0} should be the same with the attributes queried by LDAP", ncDN);

            foreach (DS_REPL_ATTR_META_DATA attributeMetaData in attremetaDatas)
            {
                uint attrType;
                attrType = LdapUtility.attrTyp(dcMachine, attributeMetaData.pszAttributeName);
                foreach (LDAP_PROPERTY_META_DATA property_meta_data in ncDNattributes)
                {
                    if (attrType == (uint)property_meta_data.attrType)
                    {
                        BaseTestSite.Assert.AreEqual<Guid>(property_meta_data.uuidDsaOriginating, attributeMetaData.uuidLastOriginatingDsaInvocationID,
                            "replicate uuidLastOriginatingDsaInvocationID");
                    }
                }

            }

        }

        [Priority(0)]
        [Ignore]
        [TestCategory("Win2003")]
        [RequireDcPartner]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSGetReplInfo with infoType DS_REPL_INFO_KCC_DSA_LINK_FAILURES to verify whether DC server has any link failures with in bound replication partners")]
        public void DRSR_DRSGetReplInfo_V2_Verify_Link_Failures()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            DsServer dcServerMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];
            EnvironmentConfig.Machine dcParnter = EnvironmentConfig.Machine.WritableDC2;
            DsServer dcParnterMachine = (DsServer)EnvironmentConfig.MachineStore[dcParnter];

            string serverAddress = dcParnterMachine.DnsHostName;
            DsUser admin = dcParnterMachine.Domain.Admin;


            Guid dsaguid = dcParnterMachine.NtdsDsaObjectGuid;
            string dsaDN = dcParnterMachine.NtdsDsaObjectName;

            string dn = null;
            //Disable inbound/outbound traffic
            bool succ = false;
            if (EnvironmentConfig.TestDS)
            {
                dn = LdapUtility.GetDnFromNcType(dcParnterMachine, NamingContext.DomainNC);

                succ = sutControlAdapter.ChangeReplTrafficStatus(
                   serverAddress,
                   admin.Username,
                   admin.Password,
                   dcParnterMachine.Domain.DNSName,
                   false
                   );

            }
            else
            {
                succ = sutControlAdapter.ChangeReplTrafficStatus(
                   serverAddress,
                   admin.Username,
                   admin.Password,
                   dcParnterMachine.Domain.DNSName,
                   false,
                   ((AdldsServer)dcParnterMachine).Port
                   );
            }
            BaseTestSite.Assert.IsTrue(succ, "Disable inbound/outbound traffic on DC {0} failed", dcParnterMachine);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServer);
            uint ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Call IDL_DRSGetReplInfo with infoType DS_REPL_INFO_KCC_DSA_LINK_FAILURES to query the connection failure");
            uint? outVersion;
            DRS_MSG_GETREPLINFO_REPLY? outMessage;
            ret = drsTestClient.DrsGetReplInfo(
                dcServer,
                DrsGetReplInfo_Versions.V2,
                dcParnter,
                DS_REPL_INFO.DS_REPL_INFO_KCC_DSA_LINK_FAILURES,
                dn,
                null, null,
                out outVersion, out outMessage);

            BaseTestSite.Assert.AreEqual<uint>((uint)DS_REPL_INFO.DS_REPL_INFO_KCC_DSA_LINK_FAILURES, outVersion.Value, "[IDL_DRSGetReplInfo] the reply version should be same as that specified by infoType of request.");

            DS_REPL_KCC_DSA_FAILUREW[] REPL_KCC_DSA_FAILUREW_List = outMessage.Value.pLinkFailures[0].rgDsaFailure;

            bool IsExistfailInfo = false;
            foreach (DS_REPL_KCC_DSA_FAILUREW REPL_KCC_DSA_FAILUREW in REPL_KCC_DSA_FAILUREW_List)
            {
                if (dsaguid == REPL_KCC_DSA_FAILUREW.uuidDsaObjGuid && dsaDN == REPL_KCC_DSA_FAILUREW.pszDsaDN)
                {
                    IsExistfailInfo = true;
                }
            }

            BaseTestSite.Assert.IsTrue(IsExistfailInfo, "replicate link failure pLinkFailures infomation should include the offline DC dsa guid {0} and dsa dnname {1}", dsaguid, dsaDN);


            //clean up Enable inbound/outbound traffic
            succ = false;
            if (EnvironmentConfig.TestDS)
            {
                dn = LdapUtility.GetDnFromNcType(dcParnterMachine, NamingContext.DomainNC);

                succ = sutControlAdapter.ChangeReplTrafficStatus(
                   serverAddress,
                   admin.Username,
                   admin.Password,
                   dcParnterMachine.Domain.DNSName,
                   true
                   );

            }
            else
            {
                succ = sutControlAdapter.ChangeReplTrafficStatus(
                   serverAddress,
                   admin.Username,
                   admin.Password,
                   dcParnterMachine.Domain.DNSName,
                   true,
                   ((AdldsServer)dcParnterMachine).Port
                   );
            }
            BaseTestSite.Assert.IsTrue(succ, "Enable inbound/outbound traffic on DC {0} failed", dcParnterMachine);

        }

        [TestCategory("Win2003")]
        [Priority(0)]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSGetReplInfo with infoType DS_REPL_INFO_PENDING_OPS to query the replication the replication tasks that are currently executing or that are queued to windows 2003 server")]
        public void DRSR_DRSGetReplInfo_V2_Query_pending_OPS()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            DsServer dcMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServer);
            uint ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Call IDL_DRSGetReplInfo with infoType DRSGetReplInfo_V2_Query_pending_OPS to query the replication state of specified NC.");
            uint? outVersion;
            DRS_MSG_GETREPLINFO_REPLY? outMessage;
            ret = drsTestClient.DrsGetReplInfo(
                dcServer,
                DrsGetReplInfo_Versions.V2,
                EnvironmentConfig.Machine.None,
                DS_REPL_INFO.DS_REPL_INFO_PENDING_OPS,
                null,
                null, null,
                out outVersion, out outMessage);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSGetReplInfo should return 0 if successful.");

            BaseTestSite.Assert.AreEqual<uint>((uint)DS_REPL_INFO.DS_REPL_INFO_PENDING_OPS, outVersion.Value,
               "[IDL_DRSGetReplInfo] the reply version should be same as that specified by infoType of request.");

        }

        [Priority(0)]
        [TestCategory("Win2003")]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSGetReplInfo with infoType DS_REPL_INFO_METADATA_FOR_ATTR_VALUE to query the replication metadata for a specific value of a link attribute")]
        public void DRSR_DRSGetReplInfo_V2_Query_Metadata_1_For_Attr_Value()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            DsServer dcMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];
            string ncDN = DRSTestData.DRSGetReplInfo_ExistGroup + "," + LdapUtility.ConvertUshortArrayToString(((AddsDomain)dcMachine.Domain).DomainNC.StringName);

            string[] member = LdapUtility.GetAttributeValuesString(dcMachine, ncDN, "member");


            BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServer);
            uint ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Call IDL_DRSGetReplInfo with infoType DS_REPL_INFO_METADATA_FOR_ATTR_VALUE to query the replication state of specified NC.");
            uint? outVersion;
            DRS_MSG_GETREPLINFO_REPLY? outMessage;
            ret = drsTestClient.DrsGetReplInfo(
                dcServer,
                DrsGetReplInfo_Versions.V2,
                EnvironmentConfig.Machine.None,
                DS_REPL_INFO.DS_REPL_INFO_METADATA_FOR_ATTR_VALUE,
                ncDN,
                "member", member[0],
                out outVersion, out outMessage);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSGetReplInfo should return 0 if successful.");

            BaseTestSite.Assert.AreEqual<uint>((uint)DS_REPL_INFO.DS_REPL_INFO_METADATA_FOR_ATTR_VALUE, outVersion.Value,
               "[IDL_DRSGetReplInfo] the reply version should be same as that specified by infoType of request.");

            //Verify replied meta data object are consistent with attributes of retrieved by LDAP.
            DS_REPL_ATTR_VALUE_META_DATA metaData = outMessage.Value.pAttrValueMetaData[0];
            BaseTestSite.Assert.AreEqual<uint>(1, metaData.cNumEntries, "replicate cNumEntries should be 1");
            BaseTestSite.Assert.AreEqual<string>(member[0], metaData.rgMetaData[0].pszObjectDn, "replicate pszObjectDn{0} not consistent with member {1} queried by LDAP", metaData.rgMetaData[0].pszObjectDn, member[0]);

        }


        [TestCategory("Win2003")]
        [Priority(0)]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSGetReplInfo with infoType DS_REPL_INFO_CURSORS_2_FOR_NC to query the replication state of specified NC.")]
        public void DRSR_DRSGetReplInfo_V2_Query_CURSORS_2()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            NamingContext specifiedNC = NamingContext.SchemaNC;
            DsServer dcMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];
            string ncDN = DrsrHelper.GetNamingContextDN(dcMachine.Domain, specifiedNC);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "[LDAP] search the replUpToDateVector info for the Schema NC of {0}", dcServer);
            UPTODATE_VECTOR_V1_EXT utdVector = ldapAdapter.GetReplUTD(dcMachine, specifiedNC);
            BaseTestSite.Assert.AreNotEqual<uint>(0, utdVector.cNumCursors, "replUpToDateVector attribute should not be empty.");

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServer);
            uint ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Call IDL_DRSGetReplInfo with infoType DS_REPL_INFO_CURSORS_2_FOR_NC to query the replication state of specified NC.");
            uint? outVersion;
            DRS_MSG_GETREPLINFO_REPLY? outMessage;
            ret = drsTestClient.DrsGetReplInfo(
                dcServer,
                DrsGetReplInfo_Versions.V2,
                EnvironmentConfig.Machine.None,
                DS_REPL_INFO.DS_REPL_INFO_CURSORS_2_FOR_NC,
                ncDN,
                null, null,
                out outVersion, out outMessage);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSGetReplInfo should return 0 if successful.");
            BaseTestSite.Assert.AreEqual<uint>((uint)DS_REPL_INFO.DS_REPL_INFO_CURSORS_2_FOR_NC, outVersion.Value,
                "[IDL_DRSGetReplInfo] the reply version should be same as that specified by infoType of request.");

            //Verify replied cursors are consistent with that of retrieved by LDAP. 
            DS_REPL_CURSORS_2 cursors = outMessage.Value.pCursors2[0];
            BaseTestSite.Assert.AreEqual<uint>(utdVector.cNumCursors, cursors.cNumCursors - 1,
                "[IDL_DRSGetReplInfo] the count of the cursors for should be same as that retrieved by LDAP.");

            foreach (DS_REPL_CURSOR_2 repliedCursor in cursors.rgCursor)
            {
                foreach (UPTODATE_CURSOR_V1 ldapCursor in utdVector.rgCursors)
                {
                    if (repliedCursor.uuidSourceDsaInvocationID.Equals(ldapCursor.uuidDsa))
                    {
                        BaseTestSite.Assert.AreEqual<long>(repliedCursor.usnAttributeFilter, ldapCursor.usnHighPropUpdate, "The USN of the update on partner DC ({0}) should be same as that retrieved by LDAP.", repliedCursor.uuidSourceDsaInvocationID);
                        break;
                    }
                }

            }
        }

        [TestCategory("Win2003")]
        [Priority(1)]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSGetReplInfo with infoType DS_REPL_INFO_CURSORS_3_FOR_NC to query the replication state of Config NC")]
        public void DRSR_DRSGetReplInfo_V2_Query_CURSORS_3()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            NamingContext specifiedNC = NamingContext.SchemaNC;
            DsServer dcMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];
            string ncDN = DrsrHelper.GetNamingContextDN(dcMachine.Domain, specifiedNC);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "[LDAP] search the replUpToDateVector info for the Schema NC of {0}", dcServer);
            UPTODATE_VECTOR_V1_EXT utdVector = ldapAdapter.GetReplUTD(dcMachine, specifiedNC);
            BaseTestSite.Assert.AreNotEqual<uint>(0, utdVector.cNumCursors, "replUpToDateVector attribute should not be empty.");

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServer);
            uint ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Call IDL_DRSGetReplInfo with infoType DS_REPL_INFO_CURSORS_FOR_NC to query the replication state of specified NC.");
            uint? outVersion;
            DRS_MSG_GETREPLINFO_REPLY? outMessage;
            ret = drsTestClient.DrsGetReplInfo(
                dcServer,
                DrsGetReplInfo_Versions.V2,
                EnvironmentConfig.Machine.None,
                DS_REPL_INFO.DS_REPL_INFO_CURSORS_3_FOR_NC,
                ncDN,
                null, null,
                out outVersion, out outMessage);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSGetReplInfo should return 0 if successful.");
            BaseTestSite.Assert.AreEqual<uint>((uint)DS_REPL_INFO.DS_REPL_INFO_CURSORS_3_FOR_NC, outVersion.Value,
                "[IDL_DRSGetReplInfo] the reply version should be same as that specified by infoType of request.");

            //Verify replied cursors are consistent with that of retrieved by LDAP. 
            DS_REPL_CURSORS_3W cursors = outMessage.Value.pCursors3[0];
            BaseTestSite.Assert.AreEqual<uint>(utdVector.cNumCursors, cursors.cNumCursors - 1,
                "[IDL_DRSGetReplInfo] the count of the cursors for should be same as that retrieved by LDAP.");

            foreach (DS_REPL_CURSOR_3W repliedCursor in cursors.rgCursor)
            {
                foreach (UPTODATE_CURSOR_V1 ldapCursor in utdVector.rgCursors)
                {
                    if (repliedCursor.uuidSourceDsaInvocationID.Equals(ldapCursor.uuidDsa))
                    {
                        BaseTestSite.Assert.AreEqual<long>(repliedCursor.usnAttributeFilter, ldapCursor.usnHighPropUpdate, "The USN of the update on partner DC ({0}) should be same as that retrieved by LDAP.", repliedCursor.uuidSourceDsaInvocationID);
                        break;
                    }
                }

            }
        }

        [TestCategory("Win2003")]
        [Priority(1)]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSGetReplInfo with infoType DS_REPL_INFO_METADATA_2_FOR_OBJ to query the replication metadata for a specific object")]
        public void DRSR_DRSGetReplInfo_V2_Query_Metadata_2_For_Obj()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            DsServer dcMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];

            DsUser admin = dcMachine.Domain.Admin;
            string existUser = string.Format(DRSTestData.DRSGetReplInfo_ExistUser, admin.Username);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "DRSTestData.DRSGetReplInfo_ExistUser: {0}", existUser);

            string ncDN = existUser + "," + LdapUtility.ConvertUshortArrayToString(((AddsDomain)dcMachine.Domain).DomainNC.StringName);
            LDAP_PROPERTY_META_DATA[] ncDNattributes = LdapUtility.GetMetaData(dcMachine, ncDN);


            BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServer);
            uint ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Call IDL_DRSGetReplInfo with infoType DS_REPL_INFO_METADATA_2_FOR_OBJ to query the replication state of specified NC.");
            uint? outVersion;
            DRS_MSG_GETREPLINFO_REPLY? outMessage;
            ret = drsTestClient.DrsGetReplInfo(
                dcServer,
                DrsGetReplInfo_Versions.V2,
                EnvironmentConfig.Machine.None,
                DS_REPL_INFO.DS_REPL_INFO_METADATA_2_FOR_OBJ,
                ncDN,
                null, null,
                out outVersion, out outMessage);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSGetReplInfo should return 0 if successful.");

            BaseTestSite.Assert.AreEqual<uint>((uint)DS_REPL_INFO.DS_REPL_INFO_METADATA_2_FOR_OBJ, outVersion.Value,
               "[IDL_DRSGetReplInfo] the reply version should be same as that specified by infoType of request.");

            //Verify replied meta data object are consistent with attributes of retrieved by LDAP.
            DS_REPL_OBJ_META_DATA_2 metaDatas = outMessage.Value.pObjMetaData2[0];
            DS_REPL_ATTR_META_DATA_2[] attremetaDatas = metaDatas.rgMetaData;
            BaseTestSite.Assert.IsNotNull(ncDNattributes, "LDAP_PROPERTY_META_DATA queried from LDAP should not be null");
            BaseTestSite.Assert.AreEqual<uint>((uint)ncDNattributes.Length, metaDatas.cNumEntries, "the reply metadata count of dn {0} should be the same with the attributes queried by LDAP", ncDN);

            foreach (DS_REPL_ATTR_META_DATA_2 attributeMetaData in attremetaDatas)
            {
                uint attrType;
                attrType = LdapUtility.attrTyp(dcMachine, attributeMetaData.pszAttributeName);
                foreach (LDAP_PROPERTY_META_DATA property_meta_data in ncDNattributes)
                {
                    if (attrType == (uint)property_meta_data.attrType)
                    {
                        BaseTestSite.Assert.AreEqual<Guid>(property_meta_data.uuidDsaOriginating, attributeMetaData.uuidLastOriginatingDsaInvocationID,
                            "replicate uuidLastOriginatingDsaInvocationID");
                    }
                }

            }

        }

        [Priority(1)]
        [TestCategory("Win2003")]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSGetReplInfo with infoType DS_REPL_INFO_METADATA_2_FOR_ATTR_VALUE to query the replication metadata for a specific value of a link attribute")]
        public void DRSR_DRSGetReplInfo_V2_Query_Metadata_2_For_Attr_Value()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            DsServer dcMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];
            string ncDN = DRSTestData.DRSGetReplInfo_ExistGroup + "," + LdapUtility.ConvertUshortArrayToString(((AddsDomain)dcMachine.Domain).DomainNC.StringName);

            string[] member = LdapUtility.GetAttributeValuesString(dcMachine, ncDN, "member");


            BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServer);
            uint ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Call IDL_DRSGetReplInfo with infoType DS_REPL_INFO_METADATA_2_FOR_ATTR_VALUE to query the replication state of specified NC.");
            uint? outVersion;
            DRS_MSG_GETREPLINFO_REPLY? outMessage;
            ret = drsTestClient.DrsGetReplInfo(
                dcServer,
                DrsGetReplInfo_Versions.V2,
                EnvironmentConfig.Machine.None,
                DS_REPL_INFO.DS_REPL_INFO_METADATA_2_FOR_ATTR_VALUE,
                ncDN,
                "member", member[0],
                out outVersion, out outMessage);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSGetReplInfo should return 0 if successful.");

            BaseTestSite.Assert.AreEqual<uint>((uint)DS_REPL_INFO.DS_REPL_INFO_METADATA_2_FOR_ATTR_VALUE, outVersion.Value,
               "[IDL_DRSGetReplInfo] the reply version should be same as that specified by infoType of request.");

            //Verify replied meta data object are consistent with attributes of retrieved by LDAP.
            DS_REPL_ATTR_VALUE_META_DATA_2 metaData = outMessage.Value.pAttrValueMetaData2[0];
            BaseTestSite.Assert.AreEqual<uint>(1, metaData.cNumEntries, "replicate cNumEntries should be 1");
            BaseTestSite.Assert.AreEqual<string>(member[0], metaData.rgMetaData[0].pszObjectDn, "replicate pszObjectDn{0} not consistent with member {1} queried by LDAP", metaData.rgMetaData[0].pszObjectDn, member[0]);

        }

        [Priority(0)]
        [TestCategory("Win2003")]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSGetReplInfo with infoType DS_REPL_INFO_METADATA_2_FOR_ATTR_VALUE to query the replication metadata for a specific link attribute of a given object.")]
        public void DRSR_DRSGetReplInfo_V2_Query_Metadata_2_For_Attr()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            DsServer dcMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];
            string ncDN = DRSTestData.DRSGetReplInfo_ExistGroup + "," + LdapUtility.ConvertUshortArrayToString(((AddsDomain)dcMachine.Domain).DomainNC.StringName);

            string[] member = LdapUtility.GetAttributeValuesString(dcMachine, ncDN, "member");


            BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServer);
            uint ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Call IDL_DRSGetReplInfo with infoType DS_REPL_INFO_METADATA_2_FOR_ATTR_VALUE to query the replication state of specified NC.");
            uint? outVersion;
            DRS_MSG_GETREPLINFO_REPLY? outMessage;
            ret = drsTestClient.DrsGetReplInfo(
                dcServer,
                DrsGetReplInfo_Versions.V2,
                EnvironmentConfig.Machine.None,
                DS_REPL_INFO.DS_REPL_INFO_METADATA_2_FOR_ATTR_VALUE,
                ncDN,
                "member", null,
                out outVersion, out outMessage);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSGetReplInfo should return 0 if successful.");

            BaseTestSite.Assert.AreEqual<uint>((uint)DS_REPL_INFO.DS_REPL_INFO_METADATA_2_FOR_ATTR_VALUE, outVersion.Value,
               "[IDL_DRSGetReplInfo] the reply version should be same as that specified by infoType of request.");

            BaseTestSite.Assert.AreEqual<int>(1, outMessage.Value.pAttrValueMetaData2.Length, "replicate pAttrValueMetaData length should be 1");

            //Verify replied meta data object are consistent with attributes of retrieved by LDAP.
            DS_REPL_VALUE_META_DATA_2[] metaDataList = outMessage.Value.pAttrValueMetaData2[0].rgMetaData;
            BaseTestSite.Assert.AreEqual<uint>((uint)member.Length, outMessage.Value.pAttrValueMetaData2[0].cNumEntries, "members length queried by ldap should be the same with response message");
            foreach (DS_REPL_VALUE_META_DATA_2 meatadata2 in metaDataList)
            {
                bool isexistpszObjectDn = false;
                foreach (string mem in member)
                {
                    if (mem == meatadata2.pszObjectDn)
                        isexistpszObjectDn = true;
                }
                BaseTestSite.Assert.IsTrue(isexistpszObjectDn, "replicate pszObjectDn{0} can not be found in member {1} queried by LDAP", meatadata2.pszObjectDn, member);
            }
        }

        [Priority(0)]
        [TestCategory("Win2003")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSGetReplInfo with infoType DS_REPL_INFO_SERVER_OUTGOING_CALLS to query the list of all outstanding RPC server call contexts.")]
        public void DRSR_DRSGetReplInfo_V2_Query_Server_Outgoing_Calls()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            DsServer dcMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];


            BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServer);
            uint ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Call IDL_DRSGetReplInfo with infoType DS_REPL_INFO_SERVER_OUTGOING_CALLS to query the replication state of specified NC.");
            uint? outVersion;
            DRS_MSG_GETREPLINFO_REPLY? outMessage;
            ret = drsTestClient.DrsGetReplInfo(
                dcServer,
                DrsGetReplInfo_Versions.V2,
                EnvironmentConfig.Machine.None,
                DS_REPL_INFO.DS_REPL_INFO_SERVER_OUTGOING_CALLS,
                null,
                null, null,
                out outVersion, out outMessage);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSGetReplInfo should return 0 if successful.");

            BaseTestSite.Assert.AreEqual<uint>((uint)DS_REPL_INFO.DS_REPL_INFO_SERVER_OUTGOING_CALLS, outVersion.Value,
               "[IDL_DRSGetReplInfo] the reply version should be same as that specified by infoType of request.");

            //Verify replied meta data object are consistent with attributes of retrieved by LDAP.

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1304:SpecifyCultureInfo", MessageId = "System.String.ToLower"), Priority(0)]
        [TestCategory("Win2003")]
        [RequireDcPartner]
        [Ignore]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSGetReplInfo with infoType DS_REPL_INFO_NEIGHBORS to find which naming contexts a DC receives updates for from a replication neighbor")]
        public void DRSR_DRSGetReplInfo_V2_Find_Neighbors_Without_SpecifiedNC()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            DsServer dcServerMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];
            EnvironmentConfig.Machine dcParnter = EnvironmentConfig.Machine.WritableDC2;
            DsServer dcParnterMachine = (DsServer)EnvironmentConfig.MachineStore[dcParnter];

            Guid uuidInvocId = dcParnterMachine.InvocationId;
            int replicationCounte = 0;

            string[] ncs = LdapUtility.GetAttributeValuesString(dcServerMachine, "", "namingContexts");

            LinkedList<string> NClist = new LinkedList<string>(ncs);

            Dictionary<string, REPS_FROM[]> rfs_all = new Dictionary<string, REPS_FROM[]>();

            foreach (string nc in NClist)
            {
                REPS_FROM[] rfs = ldapAdapter.GetRepsFrom(dcServerMachine, nc);

                if (rfs != null)
                {
                    for (int i = 0; i < rfs.Length; i++)
                    {
                        if (rfs[i].uuidInvocId == uuidInvocId)
                            replicationCounte++;

                        //convert for RepNbrOptionToDra limitation
                        rfs[i].ulReplicaFlags = DrsrUtility.ConvertRepFlagsToRepNbrOption(rfs[i].ulReplicaFlags);
                    }
                    rfs_all.Add(nc, rfs);
                }
            }


            BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServer);
            uint ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Call IDL_DRSGetReplInfo with infoType DS_REPL_INFO_NEIGHBORS to find replication neighbors for config NC.");
            uint? outVersion;
            DRS_MSG_GETREPLINFO_REPLY? outMessage;
            ret = drsTestClient.DrsGetReplInfo(
                dcServer,
                DrsGetReplInfo_Versions.V2,
                dcParnter,
                DS_REPL_INFO.DS_REPL_INFO_NEIGHBORS,
                null,
                null, null,
                out outVersion, out outMessage);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSGetReplInfo should return 0 if successful.");
            BaseTestSite.Assert.AreEqual<uint>((uint)DS_REPL_INFO.DS_REPL_INFO_NEIGHBORS, outVersion.Value,
                "[IDL_DRSGetReplInfo] the reply version should be same as that specified by infoType of request.");

            //Verify replied replication partners are consistent with that of retrieved by LDAP. 
            DS_REPL_NEIGHBORSW nbs = outMessage.Value.pNeighbors[0];
            //BaseTestSite.Assert.AreEqual<int>(replicationCounte, (int)nbs.cNumNeighbors,
            //    "[IDL_DRSGetReplInfo] the count of the replication partners for Config NC should be same as that retrieved by LDAP.");
            foreach (DS_REPL_NEIGHBORW nb in nbs.rgNeighbor)
            {
                if (EnvironmentConfig.MachineStore.ContainsKey(EnvironmentConfig.Machine.CDC))
                {
                    if (nb.pszNamingContext.ToLower() == EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.CDC].Domain.Name.ToLower())
                    {
                        //skip child domain default NC because there won't be record on dc01 that can be read via LDAP to verify
                        continue;
                    }
                }
                BaseTestSite.Assert.IsNotNull(rfs_all[nb.pszNamingContext], "the replicate pszNamingContext {0} should can be query by ldap", nb.pszNamingContext);
                REPS_FROM[] rfs = rfs_all[nb.pszNamingContext];

                foreach (REPS_FROM rf in rfs)
                {
                    if (nb.uuidSourceDsaInvocationID.Equals(rf.uuidInvocId))
                    {
                        BaseTestSite.Assert.AreEqual<Guid>(rf.uuidDsaObj, nb.uuidSourceDsaObjGuid, "The replied neighbor should have the same DSA GUID of the source DC.");
                        BaseTestSite.Assert.AreEqual<Guid>(rf.uuidTransportObj, nb.uuidAsyncIntersiteTransportObjGuid, "The replied neighbor should have the same Transportobject GUID.");
                        if (bool.Parse(Site.Properties["TDI.67260"]))
                        {
                            BaseTestSite.Assert.AreEqual<uint>(rf.ulReplicaFlags, nb.dwReplicaFlags, "The replied neighbor should have the same replication flags.");
                        }
                        break;
                    }
                }
            }

        }

        [TestCategory("Win2003")]
        [Priority(0)]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSGetReplInfo with infoType DS_REPL_INFO_UPTODATE_VECTOR_V1 to query the replication state for the NC replica of a given NC.")]
        public void DRSR_DRSGetReplInfo_V2_Query_UpdateToDate_Vector_V1()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            NamingContext specifiedNC = NamingContext.SchemaNC;
            DsServer dcMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];
            string ncDN = DrsrHelper.GetNamingContextDN(dcMachine.Domain, specifiedNC);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "[LDAP] search the replUpToDateVector info for the Schema NC of {0}", dcServer);
            UPTODATE_VECTOR_V1_EXT utdVector = ldapAdapter.GetReplUTD(dcMachine, specifiedNC);
            BaseTestSite.Assert.AreNotEqual<uint>(0, utdVector.cNumCursors, "replUpToDateVector attribute should not be empty.");

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServer);
            uint ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Call DS_REPL_INFO_UPTODATE_VECTOR_V1 with infoType DS_REPL_INFO_UPTODATE_VECTOR_V1 to query the replication state of specified NC.");
            uint? outVersion;
            DRS_MSG_GETREPLINFO_REPLY? outMessage;
            ret = drsTestClient.DrsGetReplInfo(
                dcServer,
                DrsGetReplInfo_Versions.V1,
                EnvironmentConfig.Machine.None,
                DS_REPL_INFO.DS_REPL_INFO_UPTODATE_VECTOR_V1,
                ncDN,
                null, null,
                out outVersion, out outMessage);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSGetReplInfo should return 0 if successful.");
            BaseTestSite.Assert.AreEqual<uint>((uint)DS_REPL_INFO.DS_REPL_INFO_UPTODATE_VECTOR_V1, outVersion.Value,
                "[IDL_DRSGetReplInfo] the reply version should be same as that specified by infoType of request.");

            //Verify replied cursors are consistent with that of retrieved by LDAP. 
            UPTODATE_CURSOR_V1[] cursors = outMessage.Value.pUpToDateVec[0].rgCursors;
            BaseTestSite.Assert.AreEqual<uint>(utdVector.cNumCursors, (uint)(cursors.Length - 1),
                "[IDL_DRSGetReplInfo] the count of the cursors for should be same as that retrieved by LDAP.");

            foreach (UPTODATE_CURSOR_V1 uptodatecursv1 in cursors)
            {
                foreach (UPTODATE_CURSOR_V1 ldapCursor in utdVector.rgCursors)
                {
                    if (uptodatecursv1.uuidDsa.Equals(ldapCursor.uuidDsa))
                    {
                        BaseTestSite.Assert.AreEqual<long>(uptodatecursv1.usnHighPropUpdate, ldapCursor.usnHighPropUpdate, "The USN of the update on partner DC ({0}) should be same as that retrieved by LDAP.", uptodatecursv1.usnHighPropUpdate);
                        break;
                    }
                }

            }
        }

        [Priority(0)]
        [TestCategory("Win2003")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSGetReplInfo with infoType DS_REPL_INFO_CLIENT_CONTEXTS without DSA to query the replication metadata for a specific value of a link attribute")]
        public void DRSR_DRSGetReplInfo_V2_Query_Client_Context()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            DsServer dcMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];
            EnvironmentConfig.Machine dcClient = EnvironmentConfig.Machine.WritableDC2;
            DsServer dcClientMachine = (DsServer)EnvironmentConfig.MachineStore[dcClient];


            BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServer);
            uint ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Call IDL_DRSGetReplInfo with infoType DS_REPL_INFO_CLIENT_CONTEXTS to query the replication state of specified NC.");
            uint? outVersion;
            DRS_MSG_GETREPLINFO_REPLY? outMessage;
            ret = drsTestClient.DrsGetReplInfo(
                dcServer,
                DrsGetReplInfo_Versions.V2,
                EnvironmentConfig.Machine.None,
                DS_REPL_INFO.DS_REPL_INFO_CLIENT_CONTEXTS,
                null,
                null, null,
                out outVersion, out outMessage);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSGetReplInfo should return 0 if successful.");

            BaseTestSite.Assert.AreEqual<uint>((uint)DS_REPL_INFO.DS_REPL_INFO_CLIENT_CONTEXTS, outVersion.Value,
               "[IDL_DRSGetReplInfo] the reply version should be same as that specified by infoType of request.");

            //Verify replied meta data object are consistent with attributes of retrieved by LDAP.           
            DrsrClientSessionContext drsContext = EnvironmentConfig.DrsContextStore[dcServer];
            DS_REPL_CLIENT_CONTEXTS clientContexts = outMessage.Value.pClientContexts[0];

            bool isDSAID = false;
            foreach (DS_REPL_CLIENT_CONTEXT clientContext in clientContexts.rgContext)
            {
                if (dcMachine.NtdsDsaObjectGuid == clientContext.uuidClient || dcClientMachine.NtdsDsaObjectGuid == clientContext.uuidClient)
                    isDSAID = true;
            }
            BaseTestSite.Assert.IsTrue(isDSAID, "replicate guid of context is constant with InvocationId when bind to the machine {0}", dcMachine.InvocationId);

        }

        [Priority(0)]
        [BVT]
        [TestCategory("Win2000")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSGetReplInfo with infoType DS_REPL_INFO_REPSTO to find replication neighbors for a specified NC.")]
        public void DRSR_DRSGetReplInfo_V2_Find_RepsTo_With_SpecifiedNC()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            NamingContext specifiedNC = NamingContext.ConfigNC;
            DsServer dcMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];
            string ncDN = DrsrHelper.GetNamingContextDN(dcMachine.Domain, specifiedNC);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "[LDAP] Search the replication partners for config NC of {0}", dcServer);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServer);
            uint ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            //try to add a record just in case DCs have deleted them
            EnvironmentConfig.ExpectSuccess = false;
            drsTestClient.DrsUpdateRefs(EnvironmentConfig.Machine.WritableDC1, DrsUpdateRefs_Versions.V1, (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2], DRS_OPTIONS.DRS_ADD_REF);
            EnvironmentConfig.ExpectSuccess = true;

            REPS_TO[] rfs = ldapAdapter.GetRepsTo(dcMachine, specifiedNC);

            for (int i = 0; i < rfs.Length; i++)
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Replication partner {0} of config NC: {1}.", i, rfs[i].uuidDsaObj);
            }



            BaseTestSite.Log.Add(LogEntryKind.Comment, "Call IDL_DRSGetReplInfo with infoType DS_REPL_INFO_REPSTO to find replication neighbors for config NC.");
            uint? outVersion;
            DRS_MSG_GETREPLINFO_REPLY? outMessage;
            ret = drsTestClient.DrsGetReplInfo(
                dcServer,
                DrsGetReplInfo_Versions.V2,
                EnvironmentConfig.Machine.None,
                DS_REPL_INFO.DS_REPL_INFO_REPSTO,
                ncDN,
                null, null,
                out outVersion, out outMessage);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSGetReplInfo should return 0 if successful.");
            BaseTestSite.Assert.AreEqual<uint>((uint)DS_REPL_INFO.DS_REPL_INFO_REPSTO, outVersion.Value,
                "[IDL_DRSGetReplInfo] the reply version should be same as that specified by infoType of request.");

            //Verify replied replication partners are consistent with that of retrieved by LDAP. 
            DS_REPL_NEIGHBORSW nbs = outMessage.Value.pRepsTo[0];
            BaseTestSite.Assert.AreEqual<int>(rfs.Length, (int)nbs.cNumNeighbors,
                "[IDL_DRSGetReplInfo] the count of the replication partners for Config NC should be same as that retrieved by LDAP.");
            foreach (DS_REPL_NEIGHBORW nb in nbs.rgNeighbor)
            {
                BaseTestSite.Assert.AreEqual<string>(ncDN, nb.pszNamingContext,
                    "[IDL_DRSGetReplInfo] the replied neighbor should be replication partner of the Config NC.");

                foreach (REPS_TO rf in rfs)
                {
                    if (nb.uuidSourceDsaInvocationID.Equals(rf.uuidInvocId))
                    {
                        BaseTestSite.Assert.AreEqual<Guid>(rf.uuidInvocId, nb.uuidSourceDsaInvocationID, "The replied neighbor should have the same invocation GUID of the source DC.");
                        BaseTestSite.Assert.AreEqual<Guid>(rf.uuidDsaObj, nb.uuidSourceDsaObjGuid, "The replied neighbor should have the same DSA GUID of the source DC.");
                        BaseTestSite.Assert.AreEqual<Guid>(rf.uuidTransportObj, nb.uuidAsyncIntersiteTransportObjGuid, "The replied neighbor should have the same Transportobject GUID.");
                        BaseTestSite.Assert.AreEqual<uint>(rf.ulReplicaFlags, nb.dwReplicaFlags, "The replied neighbor should have the same replication flags.");
                        break;
                    }
                }
            }
        }
        #endregion

        #region IDL_DRSReplicaSync

        [BVT]
        [Priority(0)]
        [TestCategory("Win2000")]
        [RequireDcPartner]
        [ServerType(DcServerTypes.WritableDC)]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSReplicaSync to trigger a DC server to replicate from another DC synchronously. The source DC is specified by the DSA GUID.")]
        public void DRSR_DRSReplicaSync_Trigger_Replication_ByDsaGuid_NoAsync()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServerType = EnvironmentConfig.Machine.WritableDC1;
            EnvironmentConfig.Machine dcPartnerType = EnvironmentConfig.Machine.WritableDC2;
            DsServer partnerDc = (DsServer)EnvironmentConfig.MachineStore[dcPartnerType];
            DsServer serverDc = (DsServer)EnvironmentConfig.MachineStore[dcServerType];
            NamingContext specifiedNC = NamingContext.ConfigNC;
            drsTestClient.SyncDCs(dcServerType, dcPartnerType);
            //Add an object to verify if replicaton sync successful.
            string newObjDn = ldapAdapter.TestAddSiteObj(partnerDc);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "IDL_DRSReplicaSync: LDAP: add a new object on {0}: {1}.", dcPartnerType, newObjDn);
            AddObjectUpdate addUpdate = new AddObjectUpdate(dcPartnerType, newObjDn);
            updateStorage.PushUpdate(addUpdate);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "IDL_DRSBind: Binding to DC server: {0}", dcServerType);
            uint ret = drsTestClient.DrsBind(dcServerType, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind: Checking return value - got: {0}, expect: 0, should return 0 with a success bind to DC", ret);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Call IDL_DRSReplicaSync to trigger a DC server to replicate from another DC synchronously. The source DC is specified by the DSA GUID.");
            ret = drsTestClient.DrsReplicaSync(dcServerType, DrsReplicaSync_Versions.V1, dcPartnerType, DRS_OPTIONS.NONE, true, specifiedNC);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSReplicaSync: Checking return value - got: {0}, expect: 0, should return 0 if successful.", ret);

            //Verify if the new added object replicated to server.
            bool fReplicated = ldapAdapter.IsObjectExist(serverDc, newObjDn);
            BaseTestSite.Assert.IsTrue(fReplicated, "IDL_DRSReplicaSync: the new added object on {0} should be replicated to {1}.", dcPartnerType, dcServerType);
        }

        [Priority(0)]
        [TestCategory("Win2000")]
        [RequireDcPartner]
        [ServerType(DcServerTypes.WritableDC)]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSReplicaSync to trigger a DC server to replicate from another DC synchronously. The source DC is specified by the network name.")]
        public void DRSR_DRSReplicaSync_Trigger_Replication_ByName_NoAsync()
        {
            DrsrTestChecker.Check();

            EnvironmentConfig.Machine dcServerType = EnvironmentConfig.Machine.WritableDC1;
            EnvironmentConfig.Machine dcPartnerType = EnvironmentConfig.Machine.WritableDC2;
            DsServer partnerDc = (DsServer)EnvironmentConfig.MachineStore[dcPartnerType];
            DsServer serverDc = (DsServer)EnvironmentConfig.MachineStore[dcServerType];
            NamingContext specifiedNC = NamingContext.DomainNC;
            drsTestClient.SyncDCs(dcServerType, dcPartnerType);
            //Add an object to verify if replicaton sync successful.
            string newObjDn = ldapAdapter.TestAddUserObj(partnerDc);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "LDAP: add a new object on {0}: {1}.", dcPartnerType, newObjDn);
            AddObjectUpdate addUpdate = new AddObjectUpdate(dcPartnerType, newObjDn);
            updateStorage.PushUpdate(addUpdate);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServerType);
            uint ret = drsTestClient.DrsBind(dcServerType, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Call IDL_DRSReplicaSync to trigger a DC server to replicate from another DC synchronously. The source DC is specified by network name.");
            ret = drsTestClient.DrsReplicaSync(dcServerType, DrsReplicaSync_Versions.V1, dcPartnerType, DRS_OPTIONS.DRS_SYNC_BYNAME, false, specifiedNC);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSReplicaSync should return 0 if successful.");

            //Verify if the new added object replicated to server.
            bool fReplicated = ldapAdapter.IsObjectExist(serverDc, newObjDn);
            BaseTestSite.Assert.IsTrue(fReplicated, "IDL_DRSReplicaSync: the new added object on {0} should be replicated to {1}.", dcPartnerType, dcServerType);

        }

        [Priority(1)]
        [TestCategory("Win2000")]
        [RequireDcPartner]
        [ServerType(DcServerTypes.WritableDC)]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSReplicaSync to trigger a DC server to replicate from another DC asynchronously. The source DC is specified by the network name.")]
        public void DRSR_DRSReplicaSync_Trigger_Replication_ByName_Async()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            EnvironmentConfig.Machine dcPartner = EnvironmentConfig.Machine.WritableDC2;
            NamingContext specifiedNC = NamingContext.ConfigNC;
            drsTestClient.SyncDCs(dcServer, dcPartner);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServer);
            uint ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Call IDL_DRSReplicaSync to trigger a DC server to replicate from another DC asynchronously. The source DC is specified by network name.");
            ret = drsTestClient.DrsReplicaSync(dcServer, DrsReplicaSync_Versions.V1, dcPartner, DRS_OPTIONS.DRS_ASYNC_OP | DRS_OPTIONS.DRS_SYNC_BYNAME, false, specifiedNC);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSReplicaSync should return 0 if successful.");
        }

        #endregion

        #region IDL_DRSGetNCChanges
        [BVT]
        [Priority(0)]
        [TestCategory("Win2003")]
        [DcClient]
        [SupportedADType(ADInstanceType.DS)]
        [RequireDcPartner]
        [ServerType(DcServerTypes.WritableDC)]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSGetNCChanges (V8 request) to get changes for a specified NC from a DC server. The change is create a user. The reply compression is not required.")]
        public void DRSR_DRSGetNCChanges_V8_Get_Changes_Create_User_NoCompression()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            EnvironmentConfig.Machine dcPartner = EnvironmentConfig.Machine.WritableDC2;
            DsServer dcServerMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];
            DsServer dcPartnerMachine = (DsServer)EnvironmentConfig.MachineStore[dcPartner];
            NamingContext specifiedNC = NamingContext.ConfigNC;


            DRS_OPTIONS ulFlags = DRS_OPTIONS.NONE;

            string newObjDN = ldapAdapter.TestAddSiteObj(dcServerMachine);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "IDL_DRSGetNCChanges: Adding a site object {0} on {1}.", newObjDN, dcServer);
            AddObjectUpdate addUpdate = new AddObjectUpdate(dcServer, newObjDN);
            updateStorage.PushUpdate(addUpdate);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "IDL_DRSGetNCChanges: Checking newObjDN {0} on DC {1} and DC {2}.", newObjDN, dcServer, dcPartner);

            bool exist1 = ldapAdapter.IsObjectExist(dcServerMachine, newObjDN);
            bool exist2 = ldapAdapter.IsObjectExist(dcPartnerMachine, newObjDN);
            BaseTestSite.Assert.IsTrue(exist1, "IDL_DRSGetNCChanges: Checking if object dn {0} exists on DC {1} - {2}", newObjDN, dcServer, exist1);
            //BaseTestSite.Assert.IsFalse(exist2, "IDL_DRSGetNCChanges: Checking if object dn {0} doesn't exist on DC {1} - {2}", newObjDN, dcPartner, exist2);


            DRS_EXTENSIONS_IN_FLAGS clientCapbilities = DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE | DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_GETCHGREPLY_V6;

            BaseTestSite.Log.Add(LogEntryKind.Comment, "IDL_DRSBind: Binding to DC server: {0}", dcServer);
            uint ret = 1;
            if (EnvironmentConfig.TestDS)
            {
                ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.WritableDC2Account, clientCapbilities);
            }
            else
            {
                ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.ParentDomainAdmin, clientCapbilities);
            }

            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind: Checking return value - got: {0}, expect: 0, should return 0 with a success bind to DC", ret);

            uint? outVersion;
            DRS_MSG_GETCHGREPLY? outMessage;
            ret = drsTestClient.DrsGetNCChanges(
                dcServer,
                DrsGetNCChanges_Versions.V8,
                dcPartner,
                ulFlags,
                specifiedNC,
                EXOP_REQ_Codes.None,
                FSMORoles.None,
                null,
                out outVersion,
                out outMessage);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSGetNCChanges: Checking return value - got: {0}, expect: 0, should return 0 if successful.", ret);

            //v8 returns version V6 response               
            BaseTestSite.Assert.AreEqual<uint>((uint)6, outVersion.Value,
                "IDL_DRSGetNCChanges: Checking outVersion - got: {0}, expect: 6, server should reply a version 6 response for version 10 request when DRS_USE_COMPRESSION is set.",
                outVersion.Value);

            BaseTestSite.Assert.AreEqual<uint>(0, outMessage.Value.V6.ulExtendedRet, "IDL_DRSGetNCChanges: Checking ulExtendedRet in reply - got: {0}, expect: 0", outMessage.Value.V6.ulExtendedRet);

            //if there is some other dns have not replicated, the cNumObjects will not be bigger than 1       
            BaseTestSite.Assert.IsTrue(
                outMessage.Value.V6.cNumObjects >= 1,
                "IDL_DRSGetNCChanges: Checking cNumObjects in reply - got: {0}, expect : >= 1 - {1}",
                outMessage.Value.V6.cNumObjects,
                outMessage.Value.V6.cNumObjects >= 1);

            BaseTestSite.Assert.AreEqual<int>(0, outMessage.Value.V6.fMoreData, "IDL_DRSGetNCChanges: Checking fMoreData in reply - got: {0}, expect: 0", outMessage.Value.V6.fMoreData);
            BaseTestSite.Assert.IsTrue(outMessage.Value.V6.cNumBytes > 0, "IDL_DRSGetNCChanges: Checking cNumBytes in reply - got: {0}, expect: > 0 - {1}", outMessage.Value.V6.cNumBytes, outMessage.Value.V6.cNumBytes > 0);

            GetNCChangePostiveV6ResponseVerification(dcServerMachine, newObjDN, outMessage);
        }

        [Priority(0)]
        [TestCategory("Win2003")]
        [DcClient]
        [Ignore]
        [SupportedADType(ADInstanceType.DS)]
        [RequireDcPartner]
        [ServerType(DcServerTypes.WritableDC)]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSNCChanges (V8 request) to get changes for a specified NC from a DC server. The change is Add a user to a group. The reply compression is not required.")]
        public void DRSR_DRSGetNCChanges_V8_Get_Changes_AddUserToGroup_NoCompression()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            EnvironmentConfig.Machine dcPartner = EnvironmentConfig.Machine.WritableDC2;
            DsServer dcServerMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];
            DsServer dcPartnerMachine = (DsServer)EnvironmentConfig.MachineStore[dcPartner];
            NamingContext specifiedNC = NamingContext.DomainNC;

            string userdn = ldapAdapter.TestAddUserObj(dcServerMachine);
            AddObjectUpdate aou = new AddObjectUpdate(dcServer, userdn);
            updateStorage.PushUpdate(aou);

            string groupdn = DRSTestData.DRSGetNCChange_ExistGroup + "," + LdapUtility.ConvertUshortArrayToString(((AddsDomain)dcServerMachine.Domain).DomainNC.StringName);

            DRS_OPTIONS ulFlags = DRS_OPTIONS.NONE;

            try
            {
                ldapAdapter.RemoveObjectFromGroup(dcServerMachine, userdn, groupdn);
            }
            catch
            {
                //it's OK if user is not in group
            }
            drsTestClient.SyncDCs(dcServer, dcServer);

            //add a user to group dn
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Add user dn {0} to group dn {1} on DC {2}", userdn, groupdn, dcServer);
            ResultCode addret = ldapAdapter.AddObjectToGroup(dcServerMachine, userdn, groupdn);
            BaseTestSite.Assert.IsTrue(addret == ResultCode.Success, "add userdn {0} to group dn {1} failed", userdn, groupdn);
            AddObjectUpdate adduserUpdate = new AddObjectUpdate(dcServer, userdn);
            updateStorage.PushUpdate(adduserUpdate);


            DRS_EXTENSIONS_IN_FLAGS clientCapbilities = DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE | DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_GETCHGREPLY_V6 | DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_STRONG_ENCRYPTION;

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServer);
            uint ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.WritableDC2Account, clientCapbilities);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            BaseTestSite.Log.Add(LogEntryKind.Comment,
                "Calling IDL_DRSNCChanges (V8 request) to get changes for a specified NC from a DC server. The reply compression is not required.");
            uint? outVersion;
            DRS_MSG_GETCHGREPLY? outMessage;
            ret = drsTestClient.DrsGetNCChanges(
                dcServer,
                DrsGetNCChanges_Versions.V8,
                dcPartner,
                ulFlags,
                specifiedNC,
                EXOP_REQ_Codes.None,
                FSMORoles.None,
                null,
                out outVersion,
                out outMessage);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSNCChanges should return 0 if successful.");

            //V8 returns version V6 response           
            BaseTestSite.Assert.AreEqual<uint>((uint)6, outVersion.Value,
                "[IDL_DRSNCChanges] server should reply a version 6 response for version 8 request when DRS_USE_COMPRESSION is set.");

            BaseTestSite.Assert.AreEqual<uint>(0, outMessage.Value.V6.ulExtendedRet, "[IDL_DRSNCChanges] server should reply ulExtendedRet value 0");
            BaseTestSite.Assert.AreEqual<int>(0, outMessage.Value.V6.fMoreData, "[IDL_DRSNCChanges] server should reply fMoreData value 0");
            GetNCChangePostiveV6ResponseVerification(dcServerMachine, userdn, outMessage);

        }

        [BVT]
        [Priority(0)]
        [TestCategory("WinThreshold")]
        [DcClient]
        [RequireDcPartner]
        [ServerType(DcServerTypes.WritableDC)]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWinThreshold")]
        [TestCategory("ForestWinThreshold")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSNCChanges (V10 request) to get changes for a specified NC from a DC server. The reply compression is not required.")]
        public void DRSR_DRSGetNCChanges_V9_Get_Changes_NoCompression()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            EnvironmentConfig.Machine dcPartner = EnvironmentConfig.Machine.WritableDC2;
            DsServer dcServerMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];
            DsServer dcPartnerMachine = (DsServer)EnvironmentConfig.MachineStore[dcPartner];
            NamingContext specifiedNC = NamingContext.DomainNC;


            DRS_OPTIONS ulFlags = DRS_OPTIONS.NONE;

            BaseTestSite.Log.Add(LogEntryKind.Comment, "[LDAP] add a group object on {0}.", dcServer);
            string newObjDN = ldapAdapter.TestAddGroupObj(dcServerMachine);
            AddObjectUpdate addUpdate = new AddObjectUpdate(dcServer, newObjDN);
            updateStorage.PushUpdate(addUpdate);
            string userDN = ldapAdapter.GetUserDn(dcServerMachine, EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainUser]);


            BaseTestSite.Assert.AreEqual<ResultCode>(ResultCode.Success, ldapAdapter.AddObjectToGroup(dcServerMachine, userDN, newObjDN), "should successfully add user " + userDN + " into group " + newObjDN);

            BaseTestSite.Assert.AreEqual<ResultCode>(ResultCode.Success, ldapAdapter.RemoveObjectFromGroup(dcServerMachine, userDN, newObjDN), "should successfully remove user " + userDN + " from group " + newObjDN);


            BaseTestSite.Log.Add(LogEntryKind.Comment, "Check newObjDN {0} should can be found on DC {1} and cannot be found on DC {1}.", newObjDN, dcServer, dcPartner);
            BaseTestSite.Assert.IsTrue(ldapAdapter.IsObjectExist(dcServerMachine, newObjDN), "object dn {0} should exist on DC {1}", newObjDN, dcServer);


            DRS_EXTENSIONS_IN_FLAGS clientCapbilities = DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE | DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_GETCHGREPLY_V6;

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServer);
            uint ret = 1;
            if (EnvironmentConfig.TestDS)
            {
                ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.WritableDC2Account, clientCapbilities, DRS_EXTENSIONS_IN_FLAGSEXT.DRS_EXT_ADAM | DRS_EXTENSIONS_IN_FLAGSEXT.DRS_EXT_LH_BETA2 | DRS_EXTENSIONS_IN_FLAGSEXT.DRS_EXT_RECYCLE_BIN | DRS_EXTENSIONS_IN_FLAGSEXT.DRS_EXT_GETCHGREPLY_V9, (uint)(DRS_EXTENSIONS_IN_FLAGSEXT.DRS_EXT_ADAM | DRS_EXTENSIONS_IN_FLAGSEXT.DRS_EXT_LH_BETA2 | DRS_EXTENSIONS_IN_FLAGSEXT.DRS_EXT_RECYCLE_BIN | DRS_EXTENSIONS_IN_FLAGSEXT.DRS_EXT_GETCHGREPLY_V9));
            }
            else
            {
                ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.ParentDomainAdmin, clientCapbilities, DRS_EXTENSIONS_IN_FLAGSEXT.DRS_EXT_ADAM | DRS_EXTENSIONS_IN_FLAGSEXT.DRS_EXT_LH_BETA2 | DRS_EXTENSIONS_IN_FLAGSEXT.DRS_EXT_RECYCLE_BIN | DRS_EXTENSIONS_IN_FLAGSEXT.DRS_EXT_GETCHGREPLY_V9, (uint)(DRS_EXTENSIONS_IN_FLAGSEXT.DRS_EXT_ADAM | DRS_EXTENSIONS_IN_FLAGSEXT.DRS_EXT_LH_BETA2 | DRS_EXTENSIONS_IN_FLAGSEXT.DRS_EXT_RECYCLE_BIN | DRS_EXTENSIONS_IN_FLAGSEXT.DRS_EXT_GETCHGREPLY_V9));
            }

            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            BaseTestSite.Log.Add(LogEntryKind.Comment,
                "Calling IDL_DRSNCChanges (V10 request) to get changes for a specified NC from a DC server. The reply compression is not required.");
            uint? outVersion;
            DRS_MSG_GETCHGREPLY? outMessage;
            ret = drsTestClient.DrsGetNCChanges(
                dcServer,
                DrsGetNCChanges_Versions.V10,
                dcPartner,
                ulFlags,
                specifiedNC,
                EXOP_REQ_Codes.None,
                FSMORoles.None,
                null,
                out outVersion,
                out outMessage);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSNCChanges should return 0 if successful.");

            //v10 returns version V9 response               
            BaseTestSite.Assert.AreEqual<uint>((uint)9, outVersion.Value,
                "[IDL_DRSNCChanges] server should reply a version 9 response for version 10 request when DRS_USE_COMPRESSION is set and DRS_EXT_GETCHGREPLY_V9 is set in extFlags field in bind.");

            BaseTestSite.Assert.AreEqual<uint>(0, outMessage.Value.V9.ulExtendedRet, "[IDL_DRSNCChanges] server should reply ulExtendedRet value 0");

            //if there is some other dns have not replicated, the cNumObjects will not be bigger than 1       
            BaseTestSite.Assert.IsTrue(outMessage.Value.V9.cNumObjects >= 1, "[IDL_DRSNCChanges] server should reply equal or more than one cNumObjects");
            BaseTestSite.Assert.AreEqual<int>(0, outMessage.Value.V9.fMoreData, "[IDL_DRSNCChanges] server should reply fMoreData value 0");
            BaseTestSite.Assert.IsTrue(outMessage.Value.V9.cNumBytes > 0, "[IDL_DRSNCChanges] server should reply cNumBytes value bigger than 1");

            GetNCChangePostiveV9ResponseVerification(dcServerMachine, newObjDN, outMessage);


        }

        [BVT]
        [Priority(0)]
        [TestCategory("Win2008R2")]
        [DcClient]
        [RequireDcPartner]
        [ServerType(DcServerTypes.WritableDC)]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSNCChanges (V10 request) to get changes for a specified NC from a DC server. The reply compression is not required.")]
        public void DRSR_DRSGetNCChanges_V10_Get_Changes_NoCompression()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            EnvironmentConfig.Machine dcPartner = EnvironmentConfig.Machine.WritableDC2;
            DsServer dcServerMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];
            DsServer dcPartnerMachine = (DsServer)EnvironmentConfig.MachineStore[dcPartner];
            NamingContext specifiedNC = NamingContext.ConfigNC;


            DRS_OPTIONS ulFlags = DRS_OPTIONS.NONE;

            BaseTestSite.Log.Add(LogEntryKind.Comment, "[LDAP] add a site object on {0}.", dcServer);
            string newObjDN = ldapAdapter.TestAddSiteObj(dcServerMachine);
            AddObjectUpdate addUpdate = new AddObjectUpdate(dcServer, newObjDN);
            updateStorage.PushUpdate(addUpdate);
            //try
            //{
            //    ldapAdpter.DeleteObject(dcPartnerMachine, newObjDN);
            //}
            //catch
            //{
            //}
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Check newObjDN {0} should can be found on DC {1} and cannot be found on DC {1}.", newObjDN, dcServer, dcPartner);
            BaseTestSite.Assert.IsTrue(ldapAdapter.IsObjectExist(dcServerMachine, newObjDN), "object dn {0} should exist on DC {1}", newObjDN, dcServer);
            //BaseTestSite.Assert.IsFalse(ldapAdpter.IsObjectExist(dcPartnerMachine, newObjDN), "object dn {0} should not exist on DC {1}", newObjDN, dcPartner);


            DRS_EXTENSIONS_IN_FLAGS clientCapbilities = DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE | DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_GETCHGREPLY_V6;

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServer);
            uint ret = 1;
            if (EnvironmentConfig.TestDS)
            {
                ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.WritableDC2Account, clientCapbilities);
            }
            else
            {
                ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.ParentDomainAdmin, clientCapbilities);
            }

            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            BaseTestSite.Log.Add(LogEntryKind.Comment,
                "Calling IDL_DRSNCChanges (V10 request) to get changes for a specified NC from a DC server. The reply compression is not required.");
            uint? outVersion;
            DRS_MSG_GETCHGREPLY? outMessage;
            ret = drsTestClient.DrsGetNCChanges(
                dcServer,
                DrsGetNCChanges_Versions.V10,
                dcPartner,
                ulFlags,
                specifiedNC,
                EXOP_REQ_Codes.None,
                FSMORoles.None,
                null,
                out outVersion,
                out outMessage);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSNCChanges should return 0 if successful.");

            //v10 returns version V6 response               
            BaseTestSite.Assert.AreEqual<uint>((uint)6, outVersion.Value,
                "[IDL_DRSNCChanges] server should reply a version 6 response for version 10 request when DRS_USE_COMPRESSION is set.");

            BaseTestSite.Assert.AreEqual<uint>(0, outMessage.Value.V6.ulExtendedRet, "[IDL_DRSNCChanges] server should reply ulExtendedRet value 0");

            //if there is some other dns have not replicated, the cNumObjects will not be bigger than 1       
            BaseTestSite.Assert.IsTrue(outMessage.Value.V6.cNumObjects >= 1, "[IDL_DRSNCChanges] server should reply equal or more than one cNumObjects");
            BaseTestSite.Assert.AreEqual<int>(0, outMessage.Value.V6.fMoreData, "[IDL_DRSNCChanges] server should reply fMoreData value 0");
            BaseTestSite.Assert.IsTrue(outMessage.Value.V6.cNumBytes > 0, "[IDL_DRSNCChanges] server should reply cNumBytes value bigger than 1");

            GetNCChangePostiveV6ResponseVerification(dcServerMachine, newObjDN, outMessage);


        }

        [Priority(1)]
        [TestCategory("Win2003")]
        [DcClient]
        [RequireDcPartner]
        [ServerType(DcServerTypes.WritableDC)]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSNCChanges (V8 request) to replicate single object (EXOP_REPL_OBJ).")]
        public void DRSR_DRSGetNCChanges_V8_EXOP_Replicate_Single_Object()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            EnvironmentConfig.Machine dcPartner = EnvironmentConfig.Machine.WritableDC2;
            DsServer dcServerMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];
            DsServer dcPartnerMachine = (DsServer)EnvironmentConfig.MachineStore[dcPartner];
            NamingContext specifiedNC = NamingContext.ConfigNC;


            DRS_OPTIONS ulFlags = DRS_OPTIONS.NONE;
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    BaseTestSite.Log.Add(LogEntryKind.Comment, "[LDAP] add a user object on {0}.", dcServer);
                    string newObjDN = ldapAdapter.TestAddSiteObj(dcServerMachine);
                    AddObjectUpdate addUpdate = new AddObjectUpdate(dcServer, newObjDN);
                    updateStorage.PushUpdate(addUpdate);

                    BaseTestSite.Log.Add(LogEntryKind.Comment, "Check newObjDN {0} should can be found on DC {1} and cannot be found on DC {1}.", newObjDN, dcServer, dcPartner);
                    BaseTestSite.Assert.IsTrue(ldapAdapter.IsObjectExist(dcServerMachine, newObjDN), "object dn {0} should exist on DC {1}", newObjDN, dcServer);
                    //BaseTestSite.Assert.IsFalse(ldapAdpter.IsObjectExist(dcPartnerMachine, newObjDN), "object dn {0} should not exist on DC {1}", newObjDN, dcPartner);

                    DRS_EXTENSIONS_IN_FLAGS clientCapbilities = DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE | DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_GETCHGREPLY_V6;

                    BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServer);
                    uint ret = 1;
                    if (EnvironmentConfig.TestDS)
                    {
                        ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.WritableDC2Account, clientCapbilities);
                    }
                    else
                    {
                        ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.ParentDomainAdmin, clientCapbilities);
                    }

                    BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

                    BaseTestSite.Log.Add(LogEntryKind.Comment,
                        "Calling IDL_DRSNCChanges (V8 request) to get changes for a specified NC from a DC server. The reply compression is not required.");
                    uint? outVersion;
                    DRS_MSG_GETCHGREPLY? outMessage;
                    ret = drsTestClient.DrsGetNCChanges(
                        dcServer,
                        DrsGetNCChanges_Versions.V8,
                        dcPartner,
                        ulFlags,
                        specifiedNC,
                        EXOP_REQ_Codes.EXOP_REPL_OBJ,
                        FSMORoles.None,
                        newObjDN,
                        out outVersion,
                        out outMessage);
                    BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSNCChanges should return 0 if successful.");
                    //V8 returns version V6 response            
                    BaseTestSite.Assert.AreEqual<uint>((uint)6, outVersion.Value,
                                   "[IDL_DRSNCChanges] server should reply a version 6 response for version 10 request when DRS_USE_COMPRESSION is set.");

                    BaseTestSite.Assert.AreEqual<uint>((uint)1, outMessage.Value.V6.ulExtendedRet,
                                 "[IDL_DRSNCChanges] server should reply a version 6 response for version 8 request with ulExtendedRet set to 1");

                    BaseTestSite.Assert.IsTrue(outMessage.Value.V6.cNumObjects == 1, "[IDL_DRSNCChanges] server should reply equal or more than one cNumObjects");
                    BaseTestSite.Assert.AreEqual<int>(0, outMessage.Value.V6.fMoreData, "[IDL_DRSNCChanges] server should reply fMoreData value 0");


                    Guid newobjdnGuid = (LdapUtility.GetObjectGuid(dcServerMachine, newObjDN)).Value;
                    REPLENTINFLIST ObjectList = outMessage.Value.V6.pObjects[0];
                    ENTINF enfREP = ObjectList.Entinf;
                    BaseTestSite.Assert.AreEqual<Guid>(newobjdnGuid, ObjectList.Entinf.pName.Value.Guid, "[IDL_DRSNCChanges] server should reply only the update for the newObjDN {0} ", newObjDN);

                    LDAP_PROPERTY_META_DATA[] nmeta = LdapUtility.GetMetaData(dcServerMachine, newObjDN);
                    BaseTestSite.Assert.IsNotNull(nmeta, "LDAP_PROPERTY_META_DATA queried from LDAP should not be null");

                    BaseTestSite.Assert.IsTrue(nmeta.Length >= enfREP.AttrBlock.pAttr.Length, "All replicated attribute of dn {0} should be bigger or equal to replicate happened last time", newObjDN);
                    foreach (ATTR attr in enfREP.AttrBlock.pAttr)
                    {
                        bool isAttrExist = false;
                        foreach (LDAP_PROPERTY_META_DATA meta in nmeta)
                        {
                            if (meta.attrType == attr.attrTyp)
                                isAttrExist = true;
                        }
                        BaseTestSite.Assert.IsTrue(isAttrExist, "attribute attr {0} is not replicated attribute of dn {1}", attr, newObjDN);
                    }

                    GetNCChangePostiveV6ResponseVerification(dcServerMachine, newObjDN, outMessage);
                    return;
                }
                catch (Exception e)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Will sleep 10 seconds and then retry due to error: {0}", e.Message);
                    System.Threading.Thread.Sleep(10000);
                    base.TestCleanup();
                    base.TestInitialize();
                }
            }
        }

        [Priority(0)]
        [TestCategory("Win2003")]
        [DcClient]
        [SupportedADType(ADInstanceType.DS)]
        [RequireDcPartner]
        [Ignore]
        [ServerType(DcServerTypes.RODC)]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSNCChanges (V8 request) to replicate a single object including secret data.")]
        public void DRSR_DRSGetNCChanges_V8_EXOP_Replicate_Single_Object_With_Secrets()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            EnvironmentConfig.Machine dcPartner = EnvironmentConfig.Machine.WritableDC2;    // FIXME: should be an RODC
            DsServer dcServerMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];
            DsServer dcPartnerMachine = (DsServer)EnvironmentConfig.MachineStore[dcPartner];
            NamingContext specifiedNC = NamingContext.DomainNC;

            string userdn = DRSTestData.DRSGetNCChange_ExistUser + "," + LdapUtility.ConvertUshortArrayToString(((AddsDomain)dcServerMachine.Domain).DomainNC.StringName);
            string oldpwd = DRSTestData.DRSGetNCChanges_OldPassword;
            string newpwd = DRSTestData.DRSGetNCChanges_NewPassword;
            LdapUtility.ChangeUserPassword(dcServerMachine, userdn, oldpwd);

            DRS_OPTIONS ulFlags = DRS_OPTIONS.NONE;

            BaseTestSite.Log.Add(LogEntryKind.Comment, "change dn {0} password on {1}.", userdn, dcServer);
            BaseTestSite.Assert.IsTrue(LdapUtility.ChangeUserPassword(dcServerMachine, userdn, newpwd), "change dn {0} password on {1} failed.", userdn, dcServer);



            DRS_EXTENSIONS_IN_FLAGS clientCapbilities = DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE | DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_GETCHGREPLY_V6 | DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_STRONG_ENCRYPTION;

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServer);

            // FIXME: should be an RODC machine account
            uint ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.WritableDC2Account, clientCapbilities);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            BaseTestSite.Log.Add(LogEntryKind.Comment,
                "Calling IDL_DRSNCChanges (V8 request) to get changes for a specified NC from a DC server. The reply compression is not required.");
            uint? outVersion;
            DRS_MSG_GETCHGREPLY? outMessage;
            ret = drsTestClient.DrsGetNCChanges(
                dcServer,
                DrsGetNCChanges_Versions.V8,
                dcPartner,
                ulFlags,
                specifiedNC,
                EXOP_REQ_Codes.EXOP_REPL_SECRETS,
                FSMORoles.None,
                userdn,
                out outVersion,
                out outMessage);

            LdapUtility.ChangeUserPassword(dcServerMachine, userdn, oldpwd);

            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSNCChanges should return 0 if successful.");


            //V8 returns version V6 response            
            BaseTestSite.Assert.AreEqual<uint>((uint)6, outVersion.Value,
                           "[IDL_DRSNCChanges] server should reply a version 6 response for version 10 request when DRS_USE_COMPRESSION is set.");

            BaseTestSite.Assert.AreEqual<int>(0, outMessage.Value.V6.fMoreData, "[IDL_DRSNCChanges] server should reply fMoreData value 0");

            GetNCChangePostiveV6ResponseVerification(dcServerMachine, userdn, outMessage);
        }

        [Priority(0)]
        [TestCategory("Win2003,BreakEnvironment")]
        [DcClient]
        [BreakEnvironment]
        [ServerFSMORoleAttribute(FSMORoles.Schema)]
        [RequireDcPartner]
        [Ignore]
        [ServerType(DcServerTypes.WritableDC)]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSNCChanges (V8 request) to replicate DC server transferring a specified FSMO role to DC client")]
        public void DRSR_DRSGetNCChanges_V8_EXOP_Request_FSMORole()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            EnvironmentConfig.Machine dcPartner = EnvironmentConfig.Machine.WritableDC2;
            DsServer dcServerMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];
            DsServer dcPartnerMachine = (DsServer)EnvironmentConfig.MachineStore[dcPartner];
            NamingContext specifiedNC = NamingContext.ConfigNC;
            FSMORoles fsmorole = FSMORoles.Schema;

            DRS_OPTIONS ulFlags = DRS_OPTIONS.NONE;


            DRS_EXTENSIONS_IN_FLAGS clientCapbilities = DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE | DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_GETCHGREPLY_V6;

            uint ret = 1;
            if (EnvironmentConfig.TestDS)
            {
                ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.WritableDC2Account, clientCapbilities);
            }
            else
            {
                ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.ParentDomainAdmin, clientCapbilities);
            }

            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            BaseTestSite.Log.Add(LogEntryKind.Comment,
                "Calling IDL_DRSNCChanges (V8 request) to get changes for a specified NC from a DC server. The reply compression is not required.");
            uint? outVersion;
            DRS_MSG_GETCHGREPLY? outMessage;
            ret = drsTestClient.DrsGetNCChanges(
                dcServer,
                DrsGetNCChanges_Versions.V8,
                dcPartner,
                ulFlags,
                specifiedNC,
                EXOP_REQ_Codes.EXOP_FSMO_REQ_ROLE,
                fsmorole,
                null,
                out outVersion,
                out outMessage);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSNCChanges should return 0 if successful.");
            //V8 returns version V6 response 
            BaseTestSite.Assert.AreEqual<uint>((uint)6, outVersion.Value,
                "[IDL_DRSNCChanges] server should reply a version 6 response for version 8 request when DRS_USE_COMPRESSION is not set.");

            BaseTestSite.Assert.IsTrue(outMessage.Value.V6.cNumObjects >= 1, "[IDL_DRSNCChanges] server should reply equal or more than one cNumObjects");
            BaseTestSite.Assert.AreEqual<int>(0, outMessage.Value.V6.fMoreData, "[IDL_DRSNCChanges] server should reply fMoreData value 0");
            BaseTestSite.Assert.IsTrue(outMessage.Value.V6.ulExtendedRet == 0 || outMessage.Value.V6.ulExtendedRet == 1, "[IDL_DRSNCChanges] server should reply ulExtendedRet value 0 or success");

            string dn = DrsrHelper.GetFSMORoleCN(dcServerMachine.Domain, fsmorole);
            GetNCChangePostiveV6ResponseVerification(dcServerMachine, dn, outMessage);

            //to do : sync two DC method has bug
            System.Threading.Thread.Sleep(5000);
            DsDomain dsdomain = dcServerMachine.Domain;

            BaseTestSite.Assert.IsTrue(ldapAdapter.GetFsmoRoleOwners(dcServerMachine, ref dsdomain), "get FSMO owner from {0} fail", dcServerMachine);
            BaseTestSite.Assert.IsFalse(dcServerMachine.Domain.FsmoRoleOwners[fsmorole].Contains(dcServerMachine.NetbiosName), "the FSMORole owner should not be DC {0} after transfer", dcServer);
            BaseTestSite.Assert.IsTrue(dcServerMachine.Domain.FsmoRoleOwners[fsmorole].Contains(dcPartnerMachine.NetbiosName), "the FSMORole owner should be DC {0} after transfer", dcPartner);

        }

        [Priority(0)]
        [TestCategory("Win2000,BreakEnvironment")]
        [DcClient]
        [RequireDcPartner]
        [SupportedADType(ADInstanceType.DS)]
        [ServerFSMORoleAttribute(FSMORoles.PDC)]
        [BreakEnvironment]
        [ServerType(DcServerTypes.WritableDC)]
        [Ignore]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSNCChanges (V8 request) to replicate DC server transferring the PDC role to DC client")]
        public void DRSR_DRSGetNCChanges_V8_EXOP_Request_PDC()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            EnvironmentConfig.Machine dcPartner = EnvironmentConfig.Machine.WritableDC2;
            DsServer dcServerMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];
            DsServer dcPartnerMachine = (DsServer)EnvironmentConfig.MachineStore[dcPartner];
            NamingContext specifiedNC = NamingContext.ConfigNC;
            FSMORoles fsmorole = FSMORoles.PDC;

            DRS_OPTIONS ulFlags = DRS_OPTIONS.NONE;


            DRS_EXTENSIONS_IN_FLAGS clientCapbilities = DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE | DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_GETCHGREPLY_V6;

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServer);
            uint ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.WritableDC2Account, clientCapbilities);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            BaseTestSite.Log.Add(LogEntryKind.Comment,
                "Calling IDL_DRSNCChanges (V8 request) to get changes for a specified NC from a DC server. The reply compression is not required.");
            uint? outVersion;
            DRS_MSG_GETCHGREPLY? outMessage;
            ret = drsTestClient.DrsGetNCChanges(
                dcServer,
                DrsGetNCChanges_Versions.V8,
                dcPartner,
                ulFlags,
                specifiedNC,
                EXOP_REQ_Codes.EXOP_FSMO_REQ_PDC,
                fsmorole,
                null,
                out outVersion,
                out outMessage);

            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSNCChanges should return 0 if successful.");
            //V8 returns version V6 response            
            BaseTestSite.Assert.AreEqual<uint>((uint)6, outVersion.Value,
                "[IDL_DRSNCChanges] server should reply a version 6 response for version 8 request when DRS_USE_COMPRESSION is not set.");

            BaseTestSite.Assert.IsTrue(outMessage.Value.V6.cNumObjects >= 1, "[IDL_DRSNCChanges] server should reply equal or more than one cNumObjects");
            BaseTestSite.Assert.AreEqual<int>(0, outMessage.Value.V6.fMoreData, "[IDL_DRSNCChanges] server should reply fMoreData value 0");
            BaseTestSite.Assert.IsTrue(outMessage.Value.V6.ulExtendedRet == 0 || outMessage.Value.V6.ulExtendedRet == 1, "[IDL_DRSNCChanges] server should reply ulExtendedRet value 0 or success");

            string dn = DrsrHelper.GetFSMORoleCN(dcServerMachine.Domain, fsmorole);
            GetNCChangePostiveV6ResponseVerification(dcServerMachine, dn, outMessage);

            //to do : sync two DC
            DsDomain dsdomain = dcServerMachine.Domain;

            BaseTestSite.Assert.IsTrue(ldapAdapter.GetFsmoRoleOwners(dcServerMachine, ref dsdomain), "get FSMO owner from {0} fail", dcServerMachine);
            BaseTestSite.Assert.IsFalse(dcServerMachine.Domain.FsmoRoleOwners[fsmorole].Contains(dcServerMachine.NetbiosName), "the FSMORole owner should not be DC {0} after transfer", dcServer);
            BaseTestSite.Assert.IsTrue(dcServerMachine.Domain.FsmoRoleOwners[fsmorole].Contains(dcPartnerMachine.NetbiosName), "the FSMORole owner should be DC {0} after transfer", dcPartner);

        }

        [Priority(0)]
        [TestCategory("Win2000,BreakEnvironment")]
        [DcClient]
        [RequireDcPartner]
        [SupportedADType(ADInstanceType.DS)]
        [ServerFSMORoleAttribute(FSMORoles.RidAllocation)]
        [BreakEnvironment]
        [ServerType(DcServerTypes.WritableDC)]
        [Ignore]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSNCChanges (V8 request) to replicate DC server transferring the RID Allocation master role to DC client")]
        public void DRSR_DRSGetNCChanges_V8_EXOP_Request_Rid_Role()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            EnvironmentConfig.Machine dcPartner = EnvironmentConfig.Machine.WritableDC2;
            DsServer dcServerMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];
            DsServer dcPartnerMachine = (DsServer)EnvironmentConfig.MachineStore[dcPartner];
            NamingContext specifiedNC = NamingContext.ConfigNC;
            FSMORoles fsmorole = FSMORoles.RidAllocation;

            DRS_OPTIONS ulFlags = DRS_OPTIONS.NONE;


            DRS_EXTENSIONS_IN_FLAGS clientCapbilities = DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE | DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_GETCHGREPLY_V6;

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServer);
            uint ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.WritableDC2Account, clientCapbilities);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            BaseTestSite.Log.Add(LogEntryKind.Comment,
                "Calling IDL_DRSNCChanges (V8 request) to get changes for a specified NC from a DC server. The reply compression is not required.");
            uint? outVersion;
            DRS_MSG_GETCHGREPLY? outMessage;
            ret = drsTestClient.DrsGetNCChanges(
                dcServer,
                DrsGetNCChanges_Versions.V8,
                dcPartner,
                ulFlags,
                specifiedNC,
                EXOP_REQ_Codes.EXOP_FSMO_RID_REQ_ROLE,
                fsmorole,
                null,
                out outVersion,
                out outMessage);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSNCChanges should return 0 if successful.");
            //V8 returns version V6 response            
            BaseTestSite.Assert.AreEqual<uint>((uint)6, outVersion.Value,
                "[IDL_DRSNCChanges] server should reply a version 6 response for version 8 request when DRS_USE_COMPRESSION is not set.");

            BaseTestSite.Assert.IsTrue(outMessage.Value.V6.cNumObjects >= 1, "[IDL_DRSNCChanges] server should reply equal or more than one cNumObjects");
            BaseTestSite.Assert.AreEqual<int>(0, outMessage.Value.V6.fMoreData, "[IDL_DRSNCChanges] server should reply fMoreData value 0");
            BaseTestSite.Assert.IsTrue(outMessage.Value.V6.ulExtendedRet == 0 || outMessage.Value.V6.ulExtendedRet == 1, "[IDL_DRSNCChanges] server should reply ulExtendedRet value 0 or success");

            string dn = DrsrHelper.GetFSMORoleCN(dcServerMachine.Domain, fsmorole);
            GetNCChangePostiveV6ResponseVerification(dcServerMachine, dn, outMessage);


            DsDomain dsdomain = dcServerMachine.Domain;

            BaseTestSite.Assert.IsTrue(ldapAdapter.GetFsmoRoleOwners(dcServerMachine, ref dsdomain), "get FSMO owner from {0} fail", dcServerMachine);
            BaseTestSite.Assert.IsFalse(dcServerMachine.Domain.FsmoRoleOwners[fsmorole].Contains(dcServerMachine.NetbiosName), "the FSMORole owner should not be DC {0} after transfer", dcServer);
            BaseTestSite.Assert.IsTrue(dcServerMachine.Domain.FsmoRoleOwners[fsmorole].Contains(dcPartnerMachine.NetbiosName), "the FSMORole owner should be DC {0} after transfer", dcPartner);

        }

        [Ignore]
        [Priority(0)]
        [TestCategory("Win2000,BreakEnvironment")]
        [DcClient]
        [RequireDcPartner]
        [BreakEnvironment]
        [ServerFSMORoleAttribute(FSMORoles.Schema)]
        [ServerType(DcServerTypes.WritableDC)]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSNCChanges (V8 request) to replicate DC server to abandon a specified FSMO role to DC client")]
        public void DRSR_DRSGetNCChanges_V8_EXOP_Abandon_FSMORole()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            EnvironmentConfig.Machine dcPartner = EnvironmentConfig.Machine.WritableDC2;
            DsServer dcServerMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];
            DsServer dcPartnerMachine = (DsServer)EnvironmentConfig.MachineStore[dcPartner];
            NamingContext specifiedNC = NamingContext.ConfigNC;
            FSMORoles fsmorole = FSMORoles.Schema;

            DRS_OPTIONS ulFlags = DRS_OPTIONS.NONE;


            DRS_EXTENSIONS_IN_FLAGS clientCapbilities = DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE | DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_GETCHGREPLY_V6;

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServer);
            uint ret = 1;
            if (EnvironmentConfig.TestDS)
            {
                ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.WritableDC2Account, clientCapbilities);
            }
            else
            {
                ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.ParentDomainAdmin, clientCapbilities);
            }

            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            BaseTestSite.Log.Add(LogEntryKind.Comment,
                "Calling IDL_DRSNCChanges (V8 request) to get changes for a specified NC from a DC server. The reply compression is not required.");
            uint? outVersion;
            DRS_MSG_GETCHGREPLY? outMessage;
            ret = drsTestClient.DrsGetNCChanges(
                dcServer,
                DrsGetNCChanges_Versions.V8,
                dcPartner,
                ulFlags,
                specifiedNC,
                EXOP_REQ_Codes.EXOP_FSMO_ABANDON_ROLE,
                fsmorole,
                null,
                out outVersion,
                out outMessage);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSNCChanges should return 0 if successful.");
            //V8 returns version V6 response            
            BaseTestSite.Assert.AreEqual<uint>((uint)6, outVersion.Value,
                "[IDL_DRSNCChanges] server should reply a version 6 response for version 8 request when DRS_USE_COMPRESSION is not set.");

            BaseTestSite.Assert.AreEqual<int>(0, outMessage.Value.V6.fMoreData, "[IDL_DRSNCChanges] server should reply fMoreData value 0");
            BaseTestSite.Assert.IsTrue(outMessage.Value.V6.ulExtendedRet == 0 || outMessage.Value.V6.ulExtendedRet == 1, "[IDL_DRSNCChanges] server should reply ulExtendedRet value 0 or success");

            string dn = DrsrHelper.GetFSMORoleCN(dcServerMachine.Domain, fsmorole);
            GetNCChangePostiveV6ResponseVerification(dcServerMachine, dn, outMessage);

            DsDomain dsdomain = dcServerMachine.Domain;

            BaseTestSite.Assert.IsTrue(ldapAdapter.GetFsmoRoleOwners(dcServerMachine, ref dsdomain), "get FSMO owner from {0} fail", dcServerMachine);
            BaseTestSite.Assert.IsTrue(dcServerMachine.Domain.FsmoRoleOwners[fsmorole].Contains(dcServerMachine.NetbiosName), "the FSMORole owner should not be DC {0} after transfer", dcServer);

        }

        [Priority(0)]
        [TestCategory("Win2000,BreakEnvironment")]
        [DcClient]
        [BreakEnvironment]
        [SupportedADType(ADInstanceType.DS)]
        [RequireDcPartner]
        [ServerFSMORoleAttribute(FSMORoles.RidAllocation)]
        [ServerType(DcServerTypes.WritableDC)]
        [Ignore]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSNCChanges (V8 request) to request DC server to allocate a new block of RIDs to DC client")]
        public void DRSR_DRSGetNCChanges_V8_EXOP_Request_Rid_Alloc()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            EnvironmentConfig.Machine dcPartner = EnvironmentConfig.Machine.WritableDC2;
            DsServer dcServerMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];
            DsServer dcPartnerMachine = (DsServer)EnvironmentConfig.MachineStore[dcPartner];
            NamingContext specifiedNC = NamingContext.ConfigNC;
            FSMORoles fsmorole = FSMORoles.RidAllocation;

            DRS_OPTIONS ulFlags = DRS_OPTIONS.NONE;


            DRS_EXTENSIONS_IN_FLAGS clientCapbilities = DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE | DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_GETCHGREPLY_V6;

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServer);
            uint ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.WritableDC2Account, clientCapbilities);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            BaseTestSite.Log.Add(LogEntryKind.Comment,
                "Calling IDL_DRSNCChanges (V8 request) to get changes for a specified NC from a DC server. The reply compression is not required.");
            uint? outVersion;
            DRS_MSG_GETCHGREPLY? outMessage;
            ret = drsTestClient.DrsGetNCChanges(
                 dcServer,
                 DrsGetNCChanges_Versions.V8,
                 dcPartner,
                 ulFlags,
                 specifiedNC,
                 EXOP_REQ_Codes.EXOP_FSMO_REQ_RID_ALLOC,
                 fsmorole,
                 null,
                 out outVersion,
                 out outMessage);

            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSNCChanges should return 0 if successful.");
            //V8 returns version V6 response            
            BaseTestSite.Assert.AreEqual<uint>((uint)6, outVersion.Value,
                "[IDL_DRSNCChanges] server should reply a version 6 response for version 8 request when DRS_USE_COMPRESSION is not set.");

            BaseTestSite.Assert.AreEqual<int>(0, outMessage.Value.V6.fMoreData, "[IDL_DRSNCChanges] server should reply fMoreData value 0");
            BaseTestSite.Assert.IsTrue(outMessage.Value.V6.ulExtendedRet == 0 || outMessage.Value.V6.ulExtendedRet == 1, "[IDL_DRSNCChanges] server should reply ulExtendedRet value 0 or success");

            string dn = DrsrHelper.GetFSMORoleCN(dcServerMachine.Domain, fsmorole);
            GetNCChangePostiveV6ResponseVerification(dcServerMachine, dn, outMessage);

            DsDomain dsdomain = dcServerMachine.Domain;

            ulong AV_RID_Pool_RIDSet = ldapAdapter.GetRidAllocationPoolFromDSA(dcPartnerMachine, dcPartnerMachine.ServerObjectName);
            ulong AV_RID_Pool_RIDManage = ldapAdapter.GetRidAllocationPoolFromRIDManager(dcPartnerMachine, dn);

            BaseTestSite.Assert.IsTrue((AV_RID_Pool_RIDManage >> 32) > (AV_RID_Pool_RIDSet >> 32), "available rid pool in RIDManage should be big than the rid set of client DC {0}", dcPartnerMachine);
        }
        #endregion

        #region IDL_DRSReplicaVerifyObjects

        [BVT]
        [Priority(0)]
        [Ignore]
        [BreakEnvironment]
        [TestCategory("Win2003,BreakEnvironment")]
        [RequireDcPartner]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description(@"Calling IDL_DRSReplicaVerifyObjects to verify the existence of objects in an NC replica 
            by comparing against a replica of the same NC on a reference DC, deleting any objects that do not exist on the reference DC.")]
        public void DRSR_DRSReplicaVerifyObjects_Verify_Objects_Expunge_NotExistObject()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServerType = EnvironmentConfig.Machine.WritableDC1;
            DsServer dcServer = (DsServer)EnvironmentConfig.MachineStore[dcServerType];
            EnvironmentConfig.Machine dcPartnerType = EnvironmentConfig.Machine.WritableDC2;
            DsServer dcPartner = (DsServer)EnvironmentConfig.MachineStore[dcPartnerType];
            NamingContext specifiedNC = NamingContext.ConfigNC;
            DsUser admin = dcServer.Domain.Admin;

            BaseTestSite.Log.Add(LogEntryKind.Comment, "[LDAP] Generate a lingering object for the {0} on {1}", specifiedNC, dcServerType);

            sutControlAdapter.CreateLingeringObject(
                dcServer.DnsHostName,
                dcPartner.DnsHostName,
                admin.Domain.NetbiosName + "\\" + admin.Username,
                admin.Password,
                LdapUtility.GetDnFromNcType(dcServer, specifiedNC),
                ".\\CreateLingeringObjectRemote.ps1");
            string objDN = "CN=testLingeringSite,CN=Sites," + LdapUtility.GetDnFromNcType(dcServer, specifiedNC);
            BaseTestSite.Assert.IsNotNull(objDN, "[LDAP] CreateLingeringObject should be successful.");

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServerType);
            uint ret = drsTestClient.DrsBind(dcPartnerType, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            BaseTestSite.Log.Add(LogEntryKind.Comment,
                @"Calling IDL_DRSReplicaVerifyObjects to verify the existence of objects in an NC replica 
                by comparing against a replica of the same NC on a reference DC, deleting any objects that do not exist on the reference DC.");
            //Remove the lingering object
            ret = drsTestClient.DrsReplicaVerifyObjects(dcPartnerType, DrsReplicaVerifyObjects_Versions.V1, dcServerType, DRS_MSG_REPVERIFYOBJ_OPTIONS.EXPUNGE, specifiedNC);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSReplicaVerifyObjects should return 0 if successful.");

            System.Threading.Thread.Sleep(20000);
            bool bDeleted = !ldapAdapter.IsObjectExist(dcPartner, objDN);
            BaseTestSite.Assert.IsTrue(bDeleted, "IDL_DRSReplicaVerifyObjects: the lingering object should be removed from server.");
        }

        [Priority(0)]
        [Ignore]
        [BreakEnvironment]
        [TestCategory("Win2003,BreakEnvironment")]
        [RequireDcPartner]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description(@"Calling IDL_DRSReplicaVerifyObjects to verify the existence of objects in an NC replica by comparing against a replica of the same NC on a reference DC, 
            logging an event for any objects that do not exist on the reference DC.")]
        public void DRSR_DRSReplicaVerifyObjects_Verify_Objects_Log_NotExistObject()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServerType = EnvironmentConfig.Machine.WritableDC1;
            DsServer dcServer = (DsServer)EnvironmentConfig.MachineStore[dcServerType];
            EnvironmentConfig.Machine dcPartnerType = EnvironmentConfig.Machine.WritableDC2;
            DsServer dcPartner = (DsServer)EnvironmentConfig.MachineStore[dcPartnerType];
            NamingContext specifiedNC = NamingContext.ConfigNC;
            DsUser admin = dcServer.Domain.Admin;

            BaseTestSite.Log.Add(LogEntryKind.Comment, "[LDAP] Generate a lingering object for the {0} on {1}", specifiedNC, dcServerType);

            sutControlAdapter.CreateLingeringObject(
                dcServer.DnsHostName,
                dcPartner.DnsHostName,
                admin.Domain.NetbiosName + "\\" + admin.Username,
                admin.Password,
                LdapUtility.GetDnFromNcType(dcServer, specifiedNC),
                ".\\CreateLingeringObjectRemote.ps1");

            string objDN = "CN=testLingeringSite,CN=Sites," + LdapUtility.GetDnFromNcType(dcServer, specifiedNC);
            BaseTestSite.Assert.IsNotNull(objDN, "[LDAP] CreateLingeringObject should be successful.");

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServerType);
            uint ret = drsTestClient.DrsBind(dcPartnerType, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            BaseTestSite.Log.Add(LogEntryKind.Comment,
                @"Calling IDL_DRSReplicaVerifyObjects to verify the existence of objects in an NC replica 
                by comparing against a replica of the same NC on a reference DC, logging an event for any objects that do not exist on the reference DC.");
            ret = drsTestClient.DrsReplicaVerifyObjects(dcPartnerType, DrsReplicaVerifyObjects_Versions.V1, dcServerType, DRS_MSG_REPVERIFYOBJ_OPTIONS.LOG, specifiedNC);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSReplicaVerifyObjects should return 0 if successful.");

            bool bDeleted = !ldapAdapter.IsObjectExist(dcPartner, objDN);
            BaseTestSite.Assert.IsFalse(bDeleted, "IDL_DRSReplicaVerifyObjects: the lingering object should be logged an event but not deleted.");

            //Remove the lingering object
            ret = drsTestClient.DrsReplicaVerifyObjects(dcPartnerType, DrsReplicaVerifyObjects_Versions.V1, dcServerType, DRS_MSG_REPVERIFYOBJ_OPTIONS.EXPUNGE, specifiedNC);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSReplicaVerifyObjects should return 0 if successful.");

            bDeleted = !ldapAdapter.IsObjectExist(dcPartner, objDN);
            BaseTestSite.Assert.IsTrue(bDeleted, "IDL_DRSReplicaVerifyObjects: the lingering object should be removed from server.");
        }
        #endregion

        #region IDL_DRSGetObjectExistence

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Int64.Parse(System.String)"), BVT]
        [Priority(0)]
        [TestCategory("Win2003,BreakEnvironment")]
        [DcClient]
        [Ignore]
        [BreakEnvironment]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description(@"Calling IDL_DRSGetObjectExistence to check the DC server’s behaviors 
            when the object existence between a specified NC replica of DC client and DC server is consistent.")]
        public void DRSR_DRSGetObjectExistence_Get_ObjectExistence_SameDigest()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServerType = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[dcServerType];
            NamingContext specifiedNC = NamingContext.ConfigNC;
            const int guidCount = 10; //The count of GUID sequence.

            string existedObjDN = ldapAdapter.TestAddSiteObj(server);
            AddObjectUpdate addUpdate = new AddObjectUpdate(dcServerType, existedObjDN);
            this.updateStorage.PushUpdate(addUpdate);

            //Get the start GUID
            DSNAME existedObjDsName = ldapAdapter.GetDsName(server, existedObjDN).Value;
            Guid startGuid = existedObjDsName.Guid;

            string usn = ldapAdapter.GetAttributeValueInString(server, "", "highestCommittedUSN");


            //Prepare the UTD filter.
            UPTODATE_VECTOR_V1_EXT utdFilter = ldapAdapter.GetReplUTD(server, specifiedNC);

            utdFilter.cNumCursors = utdFilter.cNumCursors + 1;

            UPTODATE_CURSOR_V1[] orgCursors = utdFilter.rgCursors;
            utdFilter.rgCursors = new UPTODATE_CURSOR_V1[utdFilter.cNumCursors];
            utdFilter.rgCursors[0].uuidDsa = server.InvocationId;
            utdFilter.rgCursors[0].usnHighPropUpdate = long.Parse(usn);
            for (int i = 0; i < orgCursors.Length; i++)
            {
                utdFilter.rgCursors[i + 1] = orgCursors[i];
            }

            //Serach the GUID sequence on server
            Guid[] guidSeq = ldapAdapter.GetObjectGuidSequence(server, specifiedNC, utdFilter, startGuid, guidCount);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServerType);
            uint ret = drsTestClient.DrsBind(dcServerType, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            BaseTestSite.Log.Add(LogEntryKind.Comment, @"Call IDL_DRSGetObjectExistence to check the DC server’s behaviors 
            when the object existence between a specified NC replica of DC client and DC server is consistent.");
            uint? outVersion;
            DRS_MSG_EXISTREPLY? outMessage;
            drsTestClient.DrsGetObjectExistence(
                dcServerType,
                DrsGetObjectExistence_Versions.V1,
                utdFilter,
                Md5Digest_Flags.ConsistentDigest,
                guidSeq,
                specifiedNC,
                out outVersion, out outMessage);

            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSGetObjectExistence should return 0 if successful.");
            BaseTestSite.Assert.AreEqual<uint>((uint)1, outVersion.Value,
                "[IDL_DRSGetObjectExistence] server should reply a version 1 response for version 1 request.");

            BaseTestSite.Assert.AreEqual<uint>(1, outMessage.Value.V1.dwStatusFlags, "[IDL_DRSGetObjectExistence] dwStatusFlags should be 1 if the digests of the object sequences on the client and server are the same.");
            BaseTestSite.Assert.AreEqual<uint>(0, outMessage.Value.V1.cNumGuids, "[IDL_DRSGetObjectExistence] server should not return the objectGUID sequence if the digests of the object sequences on the client and server are the same.");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Int64.Parse(System.String)"), Priority(1)]
        [TestCategory("Win2003")]
        [DcClient]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description(@"Calling IDL_DRSGetObjectExistence to check the DC server’s behaviors 
            when the object existence between a specified NC replica of DC client and DC server is not consistent.")]
        public void DRSR_DRSGetObjectExistence_Get_ObjectExistence_DifferentDigest()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServerType = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[dcServerType];
            NamingContext specifiedNC = NamingContext.ConfigNC;
            const int guidCount = 10; //The count of GUID sequence.

            string existedObjDN = ldapAdapter.TestAddSiteObj(server);
            AddObjectUpdate addUpdate = new AddObjectUpdate(dcServerType, existedObjDN);
            this.updateStorage.PushUpdate(addUpdate);

            //Get the start GUID
            DSNAME existedObjDsName = ldapAdapter.GetDsName(server, existedObjDN).Value;
            Guid startGuid = existedObjDsName.Guid;

            string usn = ldapAdapter.GetAttributeValueInString(server, "", "highestCommittedUSN");


            //Prepare the UTD filter.
            UPTODATE_VECTOR_V1_EXT utdFilter = ldapAdapter.GetReplUTD(server, specifiedNC);
            BaseTestSite.Assert.AreNotEqual<uint>(0, utdFilter.cNumCursors, "replUpToDateVector attribute should not be empty.");
            utdFilter.cNumCursors = utdFilter.cNumCursors + 1;

            UPTODATE_CURSOR_V1[] orgCursors = utdFilter.rgCursors;
            utdFilter.rgCursors = new UPTODATE_CURSOR_V1[utdFilter.cNumCursors];
            utdFilter.rgCursors[0].uuidDsa = server.InvocationId;
            utdFilter.rgCursors[0].usnHighPropUpdate = long.Parse(usn);
            for (int i = 0; i < orgCursors.Length; i++)
            {
                utdFilter.rgCursors[i + 1] = orgCursors[i];
            }

            //Search the GUID sequence on server
            Guid[] guidSeq = ldapAdapter.GetObjectGuidSequence(server, specifiedNC, utdFilter, startGuid, guidCount);
            BaseTestSite.Assert.IsTrue(guidSeq.Length > 0, "should find out several objects for later get existence operation. If failed, please check CheckFailed type logs.");
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServerType);
            uint ret = drsTestClient.DrsBind(dcServerType, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            BaseTestSite.Log.Add(LogEntryKind.Comment, @"Call IDL_DRSGetObjectExistence to check the DC server’s behaviors 
            when the object existence between a specified NC replica of DC client and DC server is consistent.");
            uint? outVersion;
            DRS_MSG_EXISTREPLY? outMessage;
            drsTestClient.DrsGetObjectExistence(
                dcServerType,
                DrsGetObjectExistence_Versions.V1,
                utdFilter,
                Md5Digest_Flags.InconsistentDigest,
                guidSeq,
                specifiedNC,
                out outVersion, out outMessage);

            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSGetObjectExistence should return 0 if successful.");
            BaseTestSite.Assert.AreEqual<uint>((uint)1, outVersion.Value,
                "[IDL_DRSGetObjectExistence] server should reply a version 1 response for version 1 request.");

            BaseTestSite.Assert.AreEqual<uint>(0, outMessage.Value.V1.dwStatusFlags,
                "[IDL_DRSGetObjectExistence] dwStatusFlags should be 0 if the digest computed by the server does not equals the digest in the client's request.");
        }

        #endregion

        #region Help methods
        private void GetNCChangePostiveV6ResponseVerification(DsServer sourceServer, string objectdn, DRS_MSG_GETCHGREPLY? outMessage)
        {
            BaseTestSite.Assert.AreEqual<Guid>(
                sourceServer.NtdsDsaObjectGuid,
                outMessage.Value.V6.uuidDsaObjSrc,
                "IDL_DRSGetNCChanges: Checking NTDS DSA Guid - got: {0}, expect: {1}",
                outMessage.Value.V6.uuidDsaObjSrc.ToString(),
                sourceServer.NtdsDsaObjectGuid.ToString());

            BaseTestSite.Assert.AreEqual<Guid>(
                sourceServer.InvocationId,
                outMessage.Value.V6.uuidInvocIdSrc,
                "IDL_DRSGetNCChanges: Checking Invocation ID - got: {0}, expect: {1}",
                sourceServer.InvocationId.ToString(),
                outMessage.Value.V6.uuidInvocIdSrc.ToString()
                );

            bool isExistInvokeID = false;

            if (outMessage.Value.V6.pUpToDateVecSrc != null)
            {
                UPTODATE_CURSOR_V2[] cursorList = outMessage.Value.V6.pUpToDateVecSrc[0].rgCursors;
                foreach (UPTODATE_CURSOR_V2 cursor in cursorList)
                {
                    if (sourceServer.InvocationId == cursor.uuidDsa)
                        isExistInvokeID = true;
                }
                BaseTestSite.Assert.IsTrue(isExistInvokeID, "IDL_DRSGetNCChanges: Checking if Invocation ID of the server is in the UTD vector - {0}", isExistInvokeID);
            }
            if (outMessage.Value.V6.pObjects != null)
            {
                Guid newobjdnGuid = (LdapUtility.GetObjectGuid(sourceServer, objectdn)).Value;
                REPLENTINFLIST ObjectList = outMessage.Value.V6.pObjects[0];
                ENTINF enfREP = new ENTINF();
                if (ObjectList.Entinf.pName.Value.Guid == newobjdnGuid)
                    enfREP = ObjectList.Entinf;
                else
                {
                    while (ObjectList.pNextEntInf != null)
                    {
                        ObjectList = (ObjectList.pNextEntInf)[0];
                        if (ObjectList.Entinf.pName.Value.Guid == newobjdnGuid)
                        {
                            enfREP = ObjectList.Entinf;
                            break;
                        }
                    }

                }
                LDAP_PROPERTY_META_DATA[] nmeta = LdapUtility.GetMetaData(sourceServer, objectdn);
                BaseTestSite.Assert.IsFalse(nmeta == null, "Attribute replPropertyMetaData for {0} should not be null.", objectdn);
                BaseTestSite.Assert.IsTrue(
                    nmeta.Length >= enfREP.AttrBlock.pAttr.Length,
                    "IDL_DRSGetNCChanges: Checking if all replicated attribute of {0} should be bigger or equal to replicate happened last time - {1}",
                    objectdn,
                    nmeta.Length >= enfREP.AttrBlock.pAttr.Length
                    );
                foreach (ATTR attr in enfREP.AttrBlock.pAttr)
                {
                    bool isAttrExist = false;
                    foreach (LDAP_PROPERTY_META_DATA meta in nmeta)
                    {
                        if (meta.attrType == attr.attrTyp)
                            isAttrExist = true;
                    }
                    BaseTestSite.Assert.IsTrue(isAttrExist, "IDL_DRSGetNCChanges: Checking if attribute {0} of {1} is replicated - {2}", attr, objectdn, isAttrExist);
                }
            }
        }

        private void GetNCChangePostiveV9ResponseVerification(DsServer sourceServer, string objectdn, DRS_MSG_GETCHGREPLY? outMessage)
        {
            BaseTestSite.Assert.AreEqual<Guid>(
                sourceServer.NtdsDsaObjectGuid,
                outMessage.Value.V9.uuidDsaObjSrc,
                "IDL_DRSGetNCChanges: Checking NTDS DSA Guid - got: {0}, expect: {1}",
                outMessage.Value.V9.uuidDsaObjSrc.ToString(),
                sourceServer.NtdsDsaObjectGuid.ToString());

            BaseTestSite.Assert.AreEqual<Guid>(
                sourceServer.InvocationId,
                outMessage.Value.V9.uuidInvocIdSrc,
                "IDL_DRSGetNCChanges: Checking Invocation ID - got: {0}, expect: {1}",
                sourceServer.InvocationId.ToString(),
                outMessage.Value.V9.uuidInvocIdSrc.ToString()
                );

            bool isExistInvokeID = false;

            if (outMessage.Value.V9.pUpToDateVecSrc != null)
            {
                UPTODATE_CURSOR_V2[] cursorList = outMessage.Value.V9.pUpToDateVecSrc[0].rgCursors;
                foreach (UPTODATE_CURSOR_V2 cursor in cursorList)
                {
                    if (sourceServer.InvocationId == cursor.uuidDsa)
                        isExistInvokeID = true;
                }
                BaseTestSite.Assert.IsTrue(isExistInvokeID, "IDL_DRSGetNCChanges: Checking if Invocation ID of the server is in the UTD vector - {0}", isExistInvokeID);
            }
            if (outMessage.Value.V9.pObjects != null)
            {
                Guid newobjdnGuid = (LdapUtility.GetObjectGuid(sourceServer, objectdn)).Value;
                REPLENTINFLIST ObjectList = outMessage.Value.V9.pObjects[0];
                ENTINF enfREP = new ENTINF();
                if (ObjectList.Entinf.pName.Value.Guid == newobjdnGuid)
                    enfREP = ObjectList.Entinf;
                else
                {
                    while (ObjectList.pNextEntInf != null)
                    {
                        ObjectList = (ObjectList.pNextEntInf)[0];
                        if (ObjectList.Entinf.pName.Value.Guid == newobjdnGuid)
                        {
                            enfREP = ObjectList.Entinf;
                            break;
                        }
                    }

                }
                LDAP_PROPERTY_META_DATA[] nmeta = LdapUtility.GetMetaData(sourceServer, objectdn);
                BaseTestSite.Assert.IsFalse(nmeta == null, "Attribute replPropertyMetaData for {0} should not be null.", objectdn);
                BaseTestSite.Assert.IsTrue(
                    nmeta.Length >= enfREP.AttrBlock.pAttr.Length,
                    "IDL_DRSGetNCChanges: Checking if all replicated attribute of {0} should be bigger or equal to replicate happened last time - {1}",
                    objectdn,
                    nmeta.Length >= enfREP.AttrBlock.pAttr.Length
                    );
                foreach (ATTR attr in enfREP.AttrBlock.pAttr)
                {
                    bool isAttrExist = false;
                    foreach (LDAP_PROPERTY_META_DATA meta in nmeta)
                    {
                        if (meta.attrType == attr.attrTyp)
                            isAttrExist = true;
                    }
                    BaseTestSite.Assert.IsTrue(isAttrExist, "IDL_DRSGetNCChanges: Checking if attribute {0} of {1} is replicated - {2}", attr, objectdn, isAttrExist);
                }
            }
        }

        #endregion
    }
}
