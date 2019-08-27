// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.DirectoryServices.Protocols;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;
//using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

// Error codes
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    [TestClass]
    public class RodcTests : DrsrTestClassBase
    {
        static Random _rnd = null;

        // Max number of timeouts allowed.
        const int kMaxTimeOut = 200;

        // Sleep duration between each query attempt.
        const int kMaxTimeoutInMilliseconds = 5000;

        // searchFlags value to indicate the attribute is in FAS
        const int kRODC_FAS = (int)RodcFasFlags.CONFIDENTIAL | (int)RodcFasFlags.RODC_FAS;

        #region Class Initialization and Cleanup
        [ClassInitialize]
        public static new void ClassInitialize(TestContext context)
        {
            DrsrTestClassBase.BaseInitialize(context);
            _rnd = new Random();
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
            SetTTTMachines(new EnvironmentConfig.Machine[] { EnvironmentConfig.Machine.PDC, EnvironmentConfig.Machine.RODC });
            base.TestInitialize();
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }
        #endregion

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1304:SpecifyCultureInfo", MessageId = "System.String.ToLower"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Int32.ToString")]
        [TestCategory("RODC")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [BVT]
        [ServerType(DcServerTypes.RODC)]
        [TestCategory("Win2008")]
        [Description("RODC feature: Read-only LDAP")]
        public void DRSR_RODC_ReadOnly_LDAP()
        {
            DrsrTestChecker.Check();
            // although this case is really not related to DRSR, using LDAP write access
            // to a RODC here could really prove that the DC is Read-only

            // A proper-functioning RODC should just return a referral.

            EnvironmentConfig.Machine rodcEnum = EnvironmentConfig.Machine.RODC;
            EnvironmentConfig.Machine dc1Enum = EnvironmentConfig.Machine.WritableDC1;
            EnvironmentConfig.Machine dc2Enum = EnvironmentConfig.Machine.WritableDC2;
            DsServer dc1 = (DsServer)EnvironmentConfig.MachineStore[dc1Enum];
            DsServer dc2 = (DsServer)EnvironmentConfig.MachineStore[dc2Enum];
            DsServer rodc = (DsServer)EnvironmentConfig.MachineStore[rodcEnum];

            // try to add a user on the RODC without referral chasing
            rodc.LdapConn.SessionOptions.ReferralChasing = ReferralChasingOptions.None;

            // to avoid naming conflict
            Random rnd = new Random();
            string nc = LdapUtility.ConvertUshortArrayToString(((AddsDomain)rodc.Domain).DomainNC.StringName);

            string dn = "CN=ShouldNotExist" + rnd.Next().ToString() + ",CN=Users," + nc;

            AddResponse resp = null;
            try
            {
                AddRequest req = new AddRequest(dn, "user");
                resp = (AddResponse)rodc.LdapConn.SendRequest(req);
            }
            catch (DirectoryOperationException e)
            {
                BaseTestSite.Assert.Fail(e.Message);
            }

            // first, verify the response is a referral
            BaseTestSite.Assert.IsTrue(resp.ResultCode == ResultCode.Referral, "Expected a referral");
            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "referral host is " + resp.Referral[0].Host);
            // then, verify it is referring to the writable DC
            BaseTestSite.Assert.IsTrue(
                dc1.DnsHostName.ToLower().Trim() == resp.Referral[0].Host.ToLower().Trim()
                || dc2.DnsHostName.ToLower().Trim() == resp.Referral[0].Host.ToLower().Trim(),
                "The referral should point to the writable DC");
        }

        [TestCategory("RODC")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [BVT]
        [ServerType(DcServerTypes.RODC)]
        [RequireDcPartner]
        [TestCategory("Win2008")]
        [Description("RODC feature: Credential Caching when a user is NOT IN the Revealed List")]
        public void DRSR_RODC_Credential_Caching_Not_Revealed()
        {
            uint timeOut = 0;
            DrsrTestChecker.Check();

            EnvironmentConfig.Machine rodcEnum = EnvironmentConfig.Machine.RODC;
            EnvironmentConfig.Machine dc1Enum = EnvironmentConfig.Machine.WritableDC1;
            DsServer dc1 = (DsServer)EnvironmentConfig.MachineStore[dc1Enum];
            DsServer rodc = (DsServer)EnvironmentConfig.MachineStore[rodcEnum];

            // take a snapshot of the current replication state of the RODC
            USN_VECTOR? usnFrom = null;
            UPTODATE_VECTOR_V1_EXT? utdVector = null;
            SnapshotReplicationState(dc1, rodc, NamingContext.DomainNC, out usnFrom, out utdVector);

            // we need a user and put it into the Revealed List.
            // create the user first if it doesn't exist.
            string userDn = ldapAdapter.TestAddUserObj(dc1);
            Assert.IsNotNull(userDn);

            // at this time, the user is NOT in the revealed list
            // wait until the object is replicated by the actual RODC
            bool replicated = false;
            for (timeOut = 0; timeOut < kMaxTimeOut; ++timeOut)
            {
                if (IsObjectReplicated(dc1, rodc, NamingContext.DomainNC, userDn))
                {
                    replicated = true;
                    break;
                }

                System.Threading.Thread.Sleep(kMaxTimeoutInMilliseconds);
            }
            BaseTestSite.Assert.IsTrue(replicated, "The object should be replicated to the RODC");
            // DRSBind
            DRS_EXTENSIONS_IN_FLAGS clientCapabilities
                = DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                | DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_GETCHGREPLY_V6
                | DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_STRONG_ENCRYPTION;

            uint ret = drsTestClient.DrsBind(dc1Enum, EnvironmentConfig.User.RODCMachineAccount, clientCapabilities);

            Assert.IsTrue(ret == 0);

            uint? outVersion;
            DRS_MSG_GETCHGREPLY? outMessage;

            ret = drsTestClient.DrsGetNCChangesV2(
                            dc1Enum,
                            dc1,
                            rodc,
                            userDn,
                            usnFrom.Value,
                            utdVector.Value,
                            true,  // request secrets
                            out outVersion,
                            out outMessage);

            BaseTestSite.Assert.IsTrue(
                (uint)Win32ErrorCode_32.ERROR_DS_DRA_SECRETS_DENIED == ret,
                "Replication of secrets should be denied when the object is not in the revealed list"
                );

            // remove the temp user
            ldapAdapter.DeleteObject(dc1, userDn);

            // DRSUnbind
            ret = drsTestClient.DrsUnbind(dc1Enum);



        }

        [TestCategory("RODC")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [BVT]
        [ServerType(DcServerTypes.RODC)]
        [RequireDcPartner]
        [TestCategory("Win2008")]
        [Description("RODC feature: Credential Caching when a user is NOT IN the Revealed List")]
        public void DRSR_RODC_Credential_Caching_No_Secrets_Replicated()
        {
            uint timeOut = 0;
            DrsrTestChecker.Check();

            EnvironmentConfig.Machine rodcEnum = EnvironmentConfig.Machine.RODC;
            EnvironmentConfig.Machine dc1Enum = EnvironmentConfig.Machine.WritableDC1;
            DsServer dc1 = (DsServer)EnvironmentConfig.MachineStore[dc1Enum];
            DsServer rodc = (DsServer)EnvironmentConfig.MachineStore[rodcEnum];

            // take a snapshot of the current replication state of the RODC
            USN_VECTOR? usnFrom = null;
            UPTODATE_VECTOR_V1_EXT? utdVector = null;
            SnapshotReplicationState(dc1, rodc, NamingContext.DomainNC, out usnFrom, out utdVector);

            // we need a user and put it into the Revealed List.
            // create the user first if it doesn't exist.
            string userDn = ldapAdapter.TestAddUserObj(dc1);
            Assert.IsNotNull(userDn);

            // wait until the object is replicated by the actual RODC
            bool replicated = false;
            for (timeOut = 0; timeOut < kMaxTimeOut; ++timeOut)
            {
                if (IsObjectReplicated(dc1, rodc, NamingContext.DomainNC, userDn))
                {
                    replicated = true;
                    break;
                }

                System.Threading.Thread.Sleep(kMaxTimeoutInMilliseconds);
            }

            BaseTestSite.Assert.IsTrue(replicated, "The object should be replicated to the RODC");

            // DRSBind
            DRS_EXTENSIONS_IN_FLAGS clientCapabilities
                = DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                | DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_GETCHGREPLY_V6
                | DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_STRONG_ENCRYPTION;

            uint ret = drsTestClient.DrsBind(dc1Enum, EnvironmentConfig.User.RODCMachineAccount, clientCapabilities);
            Assert.IsTrue(ret == 0);
            // at this time, the user is NOT in the revealed list

            uint? outVersion;
            DRS_MSG_GETCHGREPLY? outMessage;

            ret = drsTestClient.DrsGetNCChangesV2(
                dc1Enum,
                dc1,
                rodc,
                userDn,
                usnFrom.Value,
                utdVector.Value,
                false,  // Don't request secrets
                out outVersion,
                out outMessage);
            // remove the temp user
            ldapAdapter.DeleteObject(dc1, userDn);

            // DRSUnbind
            ret = drsTestClient.DrsUnbind(dc1Enum);
            Assert.IsTrue(ret == 0);

            // check in outMessage that the secret attributes are not replicated.
            DRS_MSG_GETCHGREPLY_V6 replyV6 = outMessage.Value.V6;
            REPLENTINFLIST[] objectList = replyV6.pObjects;

            bool secretFound = false;
            string firstSecretAttrbute = null;
            foreach (REPLENTINFLIST entInf in objectList)
            {
                for (int i = 0; i < entInf.Entinf.AttrBlock.attrCount; ++i)
                {
                    ATTR attr = entInf.Entinf.AttrBlock.pAttr[i];
                    if (IsSecretAttribute(dc1, attr.attrTyp, replyV6.PrefixTableSrc, out firstSecretAttrbute))
                    {
                        // the secret attribute should never have its value replicated
                        if (attr.AttrVal.pAVal != null)
                        {
                            secretFound = true;
                            break;
                        }
                    }
                }
            }

            BaseTestSite.Assert.IsFalse(
                secretFound,
                "Secret attribute {0} should not appear in the response when user is not in the revealed list",
                firstSecretAttrbute);

        }

        [Ignore]
        [TestCategory("RODC")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [BVT]
        [ServerType(DcServerTypes.RODC)]
        [RequireDcPartner]
        [TestCategory("Win2008")]
        [Description("RODC feature: Credential Caching when a user is in the Revealed List")]
        public void DRSR_RODC_Credential_Caching_Revealed()
        {
            int timeOut = 0;
            DrsrTestChecker.Check();

            EnvironmentConfig.Machine rodcEnum = EnvironmentConfig.Machine.RODC;
            EnvironmentConfig.Machine dc1Enum = EnvironmentConfig.Machine.WritableDC1;
            DsServer dc1 = (DsServer)EnvironmentConfig.MachineStore[dc1Enum];
            DsServer rodc = (DsServer)EnvironmentConfig.MachineStore[rodcEnum];

            // take a snapshot of the current replication state of the RODC
            USN_VECTOR? usnFrom = null;
            UPTODATE_VECTOR_V1_EXT? utdVector = null;
            SnapshotReplicationState(dc1, rodc, NamingContext.DomainNC, out usnFrom, out utdVector);


            // we need a user and put it into the Revealed List.
            // create the user first if it doesn't exist.
            string nc = LdapUtility.GetDnFromNcType(dc1, NamingContext.DomainNC);
            string userDn = ldapAdapter.TestAddUserObj(dc1);
            Assert.IsNotNull(userDn);

            // add this user to the "Allowed RODC Password Replication Group"
            string allowedDn = "CN=Allowed RODC Password Replication Group, CN=Users,"
                    + LdapUtility.GetDnFromNcType(dc1, NamingContext.DomainNC);

            ResultCode r = ldapAdapter.AddObjectToGroup(dc1, userDn, allowedDn);

            Assert.IsTrue(r == ResultCode.Success);
            // Set password of the user
            LdapUtility.ChangeUserPassword(dc1, userDn, "1*admin");

            // wait until the object is replicated by the actual RODC
            bool replicated = false;
            for (timeOut = 0; timeOut < kMaxTimeOut; ++timeOut)
            {
                if (IsObjectReplicated(dc1, rodc, NamingContext.DomainNC, userDn))
                {
                    replicated = true;
                    break;
                }

                System.Threading.Thread.Sleep(kMaxTimeoutInMilliseconds);
            }

            // wait until the "Allowed RODC Password Replication Group object is replicated by the actual RODC
            replicated = false;
            for (timeOut = 0; timeOut < kMaxTimeOut; ++timeOut)
            {
                if (IsObjectReplicated(dc1, rodc, NamingContext.DomainNC, allowedDn))
                {
                    replicated = true;
                    break;
                }

                System.Threading.Thread.Sleep(kMaxTimeoutInMilliseconds);
            }

            BaseTestSite.Assert.IsTrue(replicated, "{0} should be replicated to the RODC", allowedDn);

            // DRSBind
            DRS_EXTENSIONS_IN_FLAGS clientCapabilities
                = DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                | DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_GETCHGREPLY_V6
                | DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_STRONG_ENCRYPTION;

            uint ret = drsTestClient.DrsBind(dc1Enum, EnvironmentConfig.User.RODCMachineAccount, clientCapabilities);
            Assert.IsTrue(ret == 0);

            uint? outVersion;
            DRS_MSG_GETCHGREPLY? outMessage = null;

            ret = drsTestClient.DrsGetNCChangesV2(
                dc1Enum,
                dc1,
                rodc,
                userDn,
                usnFrom.Value,
                utdVector.Value,
                true,  // request secrets
                out outVersion,
                out outMessage);

            // DRSUnbind
            ret = drsTestClient.DrsUnbind(dc1Enum);
            Assert.IsTrue(ret == 0);

            ldapAdapter.DeleteObject(dc1, userDn);

            // check in outMessage that the secret attributes are not replicated.
            DRS_MSG_GETCHGREPLY_V6 replyV6 = outMessage.Value.V6;
            REPLENTINFLIST[] objectList = replyV6.pObjects;

            bool secretFound = false;
            string firstSecretAttrbute = null;
            foreach (REPLENTINFLIST entInf in objectList)
            {
                for (int i = 0; i < entInf.Entinf.AttrBlock.attrCount; ++i)
                {
                    ATTR attr = entInf.Entinf.AttrBlock.pAttr[i];
                    if (IsSecretAttribute(dc1, attr.attrTyp, replyV6.PrefixTableSrc, out firstSecretAttrbute))
                    {
                        if (attr.AttrVal.pAVal != null)
                        {
                            secretFound = true;
                        }
                    }
                }
            }

            BaseTestSite.Assert.IsTrue(
                secretFound,
                "Secret attribute {0} should appear in the response when user is in the revealed list",
                firstSecretAttrbute);
        }

        [TestCategory("RODC")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [BVT]
        [ServerType(DcServerTypes.RODC)]
        [RequireDcPartner]
        [TestCategory("Win2008")]
        [Description("RODC feature: any attempt to replicate from RODC will denied.")]
        public void DRSR_RODC_Unidirectional_Replication_Denied()
        {
            DrsrTestChecker.Check();

            EnvironmentConfig.Machine rodcEnum = EnvironmentConfig.Machine.RODC;
            EnvironmentConfig.Machine dc1Enum = EnvironmentConfig.Machine.WritableDC1;
            DsServer dc1 = (DsServer)EnvironmentConfig.MachineStore[dc1Enum];
            DsServer rodc = (DsServer)EnvironmentConfig.MachineStore[rodcEnum];

            // DRSBind
            DRS_EXTENSIONS_IN_FLAGS clientCapabilities
                = DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                | DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_GETCHGREPLY_V6
                | DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_STRONG_ENCRYPTION;

            uint ret = drsTestClient.DrsBind(rodcEnum, EnvironmentConfig.User.MainDCAccount, clientCapabilities);

            Assert.IsTrue(ret == 0);

            // test user DN
            string nc = LdapUtility.GetDnFromNcType(dc1, NamingContext.DomainNC);
            string userDn = "CN=" + EnvironmentConfig.DomainUserName + ",CN=Users," + nc;

            EnvironmentConfig.ExpectSuccess = false;

            uint? outVersion;
            DRS_MSG_GETCHGREPLY? outMessage;

            bool exceptionCaught = false;
            try
            {
                ret = drsTestClient.DrsGetNCChanges(
                    rodcEnum,
                    DrsGetNCChanges_Versions.V8,
                    dc1Enum,
                    DRS_OPTIONS.NONE,
                    NamingContext.DomainNC,
                    EXOP_REQ_Codes.EXOP_REPL_SECRETS,
                    FSMORoles.None,
                    userDn,
                    out outVersion,
                    out outMessage);
            }
            catch (Exception e)
            {
                if (EnvironmentConfig.UseNativeRpcLib)
                {
                    // windows behavior, an RODC cannot process GetNCChange request
                    // Illegal RODC RPC call
                    BaseTestSite.Assert.IsInstanceOfType(
                        e,
                        typeof(SEHException),
                        "The call should not be accepted by the RODC");
                }
                else
                {
                    BaseTestSite.Assert.AreEqual<int>(87, int.Parse(e.Message), "RPC error code " + e.Message);
                }
                exceptionCaught = true;
            }
            finally
            {
                EnvironmentConfig.ExpectSuccess = true;
            }

            BaseTestSite.Assert.IsTrue(exceptionCaught, "An exception should be raised to indicate invalid GetNCChange calls");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Int32.ToString"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Convert.ToUInt32(System.Object)")]
        [TestCategory("RODC")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [BVT]
        [ServerType(DcServerTypes.RODC)]
        [RequireDcPartner]
        [TestCategory("Win2008")]
        [Description("RODC feature: Add an attribute to the Filtered Attribute Set.")]
        public void DRSR_RODC_FAS_Add_Attribute()
        {
            int timeOut = 0;

            DrsrTestChecker.Check();

            EnvironmentConfig.Machine rodcEnum = EnvironmentConfig.Machine.RODC;
            EnvironmentConfig.Machine dc1Enum = EnvironmentConfig.Machine.WritableDC1;
            DsServer dc1 = (DsServer)EnvironmentConfig.MachineStore[dc1Enum];
            DsServer rodc = (DsServer)EnvironmentConfig.MachineStore[rodcEnum];
            ResultCode r = ResultCode.Other;

            // FAS: first try "Employee-Number"
            string nc = LdapUtility.GetDnFromNcType(dc1, NamingContext.SchemaNC);
            string searchFlagsAttr = "searchFlags";
            string attrDn = "CN=Employee-Number," + nc;
            string attrName = "employeeNumber";

            uint searchFlags = 0;

            // wait until FAS is modified on DC01
            for (timeOut = 0; timeOut < kMaxTimeOut; ++timeOut)
            {
                searchFlags = Convert.ToUInt32(
                        ldapAdapter.GetAttributeValueInString(dc1, attrDn, searchFlagsAttr)
                    );

                if ((searchFlags & kRODC_FAS) != 0)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Comment, "FAS already effective.");
                    // the attribute is already in the FAS
                    break;
                }

                ldapAdapter.ModifyAttribute(dc1, attrDn, new DirectoryAttribute(searchFlagsAttr, kRODC_FAS.ToString()));

                System.Threading.Thread.Sleep(kMaxTimeoutInMilliseconds);
            }

            BaseTestSite.Assert.IsTrue((searchFlags & kRODC_FAS) != 0, "FAS is set successfully on {0}", dc1.NetbiosName);

            uint ret = drsTestClient.DrsBind(rodcEnum, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            Assert.IsTrue(ret == 0);
            ret = drsTestClient.DrsReplicaSync(rodcEnum, DrsReplicaSync_Versions.V1, dc1Enum, DRS_OPTIONS.DRS_FULL_SYNC_NOW, false, NamingContext.SchemaNC);
            BaseTestSite.Assert.IsTrue(ret == 0, "Start replica from {0} to {1}.", dc1.NetbiosName, rodc.NetbiosName);

            // wait until FAS is replicated to RODC
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Waiting for FAS to be replicated, searchFlags: {0}", searchFlags);
            bool isReplicated = false;
            for (timeOut = 0; timeOut < kMaxTimeOut; ++timeOut)
            {
                if (IsObjectReplicated(dc1, rodc, NamingContext.SchemaNC, attrDn))
                {
                    // exit the loop when FAS is replicated to the RODC
                    isReplicated = true;
                    BaseTestSite.Log.Add(LogEntryKind.Comment, "FAS applied.");
                    break;
                }
                System.Threading.Thread.Sleep(kMaxTimeoutInMilliseconds);
            }
            BaseTestSite.Assert.IsTrue(isReplicated, "Replica from {0} succeeded.", dc1.NetbiosName);

            // create the user first if it doesn't exist.
            string userDn = ldapAdapter.TestAddUserObj(dc1);
            Assert.IsNotNull(userDn);

            // take a "snapshot" of the current RODC replication state.
            // We'll use this snapshot to impersonate earlier state of the RODC
            // AFTER the changes are replicated to the actual RODC.

            USN_VECTOR? usnFrom = null;
            UPTODATE_VECTOR_V1_EXT? utdVector = null;

            SnapshotReplicationState(dc1, rodc, NamingContext.DomainNC, out usnFrom, out utdVector);

            // modify Employee Number
            int newValue = _rnd.Next();

            DirectoryAttribute employeeNumberAttr = new DirectoryAttribute(attrName, newValue.ToString());
            r = ldapAdapter.ModifyAttribute(dc1, userDn, employeeNumberAttr);

            Assert.AreEqual<ResultCode>(ResultCode.Success, r);

            //ret = drsTestClient.DrsBind(rodcEnum, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            //Assert.IsTrue(ret == 0);
            ret = drsTestClient.DrsReplicaSync(rodcEnum, DrsReplicaSync_Versions.V1, dc1Enum, DRS_OPTIONS.DRS_ASYNC_OP, false, NamingContext.DomainNC);
            BaseTestSite.Assert.IsTrue(ret == 0, "Start replica from {0} to {1}.", dc1.NetbiosName, rodc.NetbiosName);

            // Wait until the change is replicated by the actual RODC

            bool replicated = false;
            for (timeOut = 0; timeOut < kMaxTimeOut; ++timeOut)
            {

                // First, check the originating USNs on RODC to make sure the replication has completed.
                if (IsObjectReplicated(dc1, rodc, NamingContext.DomainNC, userDn))
                {
                    replicated = true;
                    break;
                }

                // sleep
                System.Threading.Thread.Sleep(kMaxTimeoutInMilliseconds);
            }

            uint? outVersion;
            DRS_MSG_GETCHGREPLY? outMessage = null;

            // DRSBind
            DRS_EXTENSIONS_IN_FLAGS clientCapabilities
                = DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                | DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_GETCHGREPLY_V6
                | DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_STRONG_ENCRYPTION;

            ret = drsTestClient.DrsBind(dc1Enum, EnvironmentConfig.User.RODCMachineAccount, clientCapabilities);
            Assert.IsTrue(ret == 0);

            if (replicated)
            {
                ret = drsTestClient.DrsGetNCChangesV2(
                    dc1Enum,
                    dc1,
                    rodc,
                    userDn,
                    usnFrom.Value,
                    utdVector.Value,
                    false,
                    out outVersion,
                    out outMessage);
            }

            // remove the temp user
            ldapAdapter.DeleteObject(dc1, userDn);

            // DRSUnbind
            ret = drsTestClient.DrsUnbind(dc1Enum);
            Assert.IsTrue(ret == 0);

            BaseTestSite.Assert.IsTrue(replicated, "Replica from {0} succeeded.", dc1.NetbiosName);

            // check in outMessage that the secret attributes are not replicated.
            DRS_MSG_GETCHGREPLY_V6 replyV6 = outMessage.Value.V6;
            REPLENTINFLIST[] objectList = replyV6.pObjects;

            if (objectList != null)
            {
                foreach (REPLENTINFLIST entInf in objectList)
                {
                    for (int i = 0; i < entInf.Entinf.AttrBlock.attrCount; ++i)
                    {
                        ATTR attr = entInf.Entinf.AttrBlock.pAttr[i];
                        string displayName = GetLdapDisplayName(dc1, attr.attrTyp, replyV6.PrefixTableSrc);

                        if (displayName == attrName)
                        {
                            // examine the new value
                            string value = System.Text.Encoding.Unicode.GetString(
                                attr.AttrVal.pAVal[0].pVal);

                            BaseTestSite.Assert.AreNotEqual<string>(
                                newValue.ToString(),
                                value,
                                "{0} is in FAS, should not be replicated", attrName);
                        }
                    }
                }
            }


            // FAS: Remove "Employee-Number" from the FAS
            DirectoryAttribute searchAttr = new DirectoryAttribute(searchFlagsAttr, "0");

            r = ldapAdapter.ModifyAttribute(dc1, attrDn, searchAttr);
        }


        // Helper functions

        string GetLdapDisplayName(DsServer dc, uint attrType, SCHEMA_PREFIX_TABLE prefixTable)
        {
            // translate the attrType to OID
            string oid = OIDUtility.OidFromAttrid(prefixTable, attrType);

            // get the lDAPDisplayName of the attribute

            // schema nc
            string schemaNc = LdapUtility.GetDnFromNcType(dc, NamingContext.SchemaNC);

            return ldapAdapter.GetAttributeValueInString(
                dc,
                schemaNc,
                "lDAPDisplayName",
                "(attributeID=" + oid + ")",
                SearchScope.OneLevel
                );
        }

        /// <summary>
        /// If the attribute is one of 
        /// 
        /// currentValue, dBCSPwd, initialAuthIncoming, initialAuthOutgoing, 
        /// lmPwdHistory, ntPwdHistory, priorValue, supplementalCredentials, 
        /// trustAuthIncoming, trustAuthOutgoing, unicodePwd
        /// 
        /// this function will return true
        /// </summary>
        /// <param name="attrType">the attribute type</param>
        /// <param name="prefixTable">the prefix table from the server</param>
        /// <returns></returns>
        bool IsSecretAttribute(DsServer dc, uint attrType, SCHEMA_PREFIX_TABLE prefixTable, out string displayName)
        {
            HashSet<string> secretAttrs = new HashSet<string>(new string[] {
                "currentValue", "dBCSPwd", "initialAuthIncoming", "initialAuthOutgoing", 
                "lmPwdHistory", "ntPwdHistory", "priorValue", "supplementalCredentials", 
                "trustAuthIncoming", "trustAuthOutgoing", "unicodePwd"
            });

            displayName = GetLdapDisplayName(dc, attrType, prefixTable);
            return secretAttrs.Contains(displayName);
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Convert.ToUInt32(System.Object)")]
        bool IsObjectReplicated(DsServer orig, DsServer dest, NamingContext ncType, string objectDn)
        {
            // FIXME: likezh-11-06-2013: 
            //      Correct me if this is wrong, but it seems to be working quite well...
            //
            // The property is replicated if it meets:
            //      - The originating USN of the property in orig is less or equal to the one in dest

            // First get the UTD in dest
            UPTODATE_VECTOR_V1_EXT utdVectorDest = ldapAdapter.GetReplUTD(dest, ncType);

            // No way to get the object metadata (ENTINF) thru LDAP, so try uSNChanged of the object instead.
            uint usnChanged = Convert.ToUInt32(ldapAdapter.GetAttributeValueInString(orig, objectDn, "uSNChanged"));

            for (int i = 0; i < utdVectorDest.cNumCursors; ++i)
            {
                UPTODATE_CURSOR_V1 cursor = utdVectorDest.rgCursors[i];
                if (cursor.uuidDsa == orig.InvocationId)
                {
                    long usnHigh = cursor.usnHighPropUpdate;
                    BaseTestSite.Log.Add(LogEntryKind.Comment, "{0} - usnHigh: {1}, usnChanged: {2}", objectDn, usnHigh, usnChanged);
                    if (usnHigh >= usnChanged)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        void SnapshotReplicationState(DsServer orig, DsServer dest, NamingContext ncType, out USN_VECTOR? usn, out UPTODATE_VECTOR_V1_EXT? utd)
        {
            usn = null;
            utd = null;

            // take a "snapshot" of the current replication state of a DC.
            // We'll use this snapshot to impersonate earlier state of the RODC
            // AFTER the changes are replicated to the actual RODC.
            REPS_FROM[] repsFroms = ldapAdapter.GetRepsFrom(dest, ncType);

            foreach (REPS_FROM rf in repsFroms)
            {
                if (rf.uuidInvocId == orig.InvocationId)
                {
                    usn = rf.usnVec;
                    break;
                }
            }

            utd = ldapAdapter.GetReplUTD(dest, ncType);
        }
    }
}
