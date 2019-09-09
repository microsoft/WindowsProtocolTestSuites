// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.DirectoryServices.Protocols;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    [TestClass]
    public class VerifyNames : DrsrTestClassBase
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
            base.TestInitialize();
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }
        #endregion

        #region IDL_DRSVerifyNames

        #region DRSVerifyNames_Verify_Dsnames
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [BVT]
        [TestCategory("Win2003")]
        [Description("Resolves a sequence of object identities to DSNAMEs")]
        public void DRSR_DRSVerifyNames_Verify_Dsnames()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(0, ret,
                "IDL_DRSBind: Checking return value - got: {0}, expect: {1}, return value should always be 0 with a success bind to DC",
                ret, 0);

            // Create a DSNAME
            string dn = server.Site.DN;
            DSNAME dsSite = ldapAdapter.GetDsName(server, dn).Value;

            // Prefix table
            SCHEMA_PREFIX_TABLE prefixTable = OIDUtility.CreatePrefixTable();

            // Create the attribute block. Here we go to RDN attribute.
            string rdnAttrId = DRSConstants.RDN_OID;
            uint attrTyp = OIDUtility.MakeAttid(prefixTable, rdnAttrId);

            ATTRVAL attrVal = DrsuapiClient.CreateATTRVAL(null);
            ATTRVALBLOCK attrValBlock = DrsuapiClient.CreateATTRVALBLOCK(new ATTRVAL[] { attrVal });
            ATTR attr = DrsuapiClient.CreateATTR(attrTyp, attrValBlock);
            ATTRBLOCK attrBlock = DrsuapiClient.CreateATTRBLOCK(new ATTR[] { attr });

            // Actual RPC call.
            ret = drsTestClient.DrsVerifyNames(
                srv,
                dwInVersion_Values.V1,
                DRS_MSG_VERIFYREQ_V1_dwFlags_Values.DRS_VERIFY_DSNAMES,
                new DSNAME[] { dsSite },
                new string[] { dn },
                attrBlock,
                prefixTable
                );

            BaseTestSite.Assert.AreEqual<uint>(0, ret,
                "IDL_DRSVerifyNames: Checking return value - got: {0}, expect: {1}, return value should always be 0",
                ret, 0);
            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(0, ret,
                "IDL_DRSUnbind: Checking return value - got: {0}, expect: {1}, return value should always be 0",
                ret, 0);
        }
        #endregion

        #region DRSVerifyNames_Verify_Sids
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Resolves a sequence of object identities to SIDs")]
        public void DRSR_DRSVerifyNames_Verify_Sids()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = (DsUser)EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainAdmin];
            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Create a DSNAME with only SID.
            string dn = ldapAdapter.GetUserDn(server, user);
            DSNAME dsUser = new DSNAME();

            dsUser.structLen = 56;
            dsUser.Guid = Guid.Empty;
            dsUser.StringName = new ushort[1];
            dsUser.SidLen = 28;
            dsUser.Sid = new NT4SID();
            dsUser.Sid.Data = ldapAdapter.GetAttributeValueInBytes(server, dn, "objectSid");

            // Prefix table
            SCHEMA_PREFIX_TABLE prefixTable = OIDUtility.CreatePrefixTable();

            // Create the attribute block. Here we go to RDN attribute.
            string rdnAttrId = DRSConstants.RDN_OID;
            uint attrTyp = OIDUtility.MakeAttid(prefixTable, rdnAttrId);

            ATTRVAL attrVal = DrsuapiClient.CreateATTRVAL(null);
            ATTRVALBLOCK attrValBlock = DrsuapiClient.CreateATTRVALBLOCK(new ATTRVAL[] { attrVal });
            ATTR attr = DrsuapiClient.CreateATTR(attrTyp, attrValBlock);
            ATTRBLOCK attrBlock = DrsuapiClient.CreateATTRBLOCK(new ATTR[] { attr });

            // Actual RPC call.
            ret = drsTestClient.DrsVerifyNames(
                srv,
                dwInVersion_Values.V1,
                DRS_MSG_VERIFYREQ_V1_dwFlags_Values.DRS_VERIFY_SIDS,
                new DSNAME[] { dsUser },
                new string[] { dn },
                attrBlock,
                prefixTable
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSVerifyNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSVerifyNames_Verify_SAM_Account_Names
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Resolves a sequence of object identities to sAMAccountName")]
        public void DRSR_DRSVerifyNames_Verify_SAM_Account_Names()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainAdmin];

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Create a DSNAME with only SAMAccountName.
            string userDn = ldapAdapter.GetUserDn(server, user);
            string sAMAccountName = ldapAdapter.GetAttributeValueInString(server, userDn, "sAMAccountName");
            DSNAME dsUser = DrsuapiClient.CreateDsName(sAMAccountName, Guid.Empty, null);

            // Prefix table
            SCHEMA_PREFIX_TABLE prefixTable = OIDUtility.CreatePrefixTable();

            // Create the attribute block. Here we go to RDN attribute.
            string rdnAttrId = DRSConstants.RDN_OID;
            uint attrTyp = OIDUtility.MakeAttid(prefixTable, rdnAttrId);

            ATTRVAL attrVal = DrsuapiClient.CreateATTRVAL(null);
            ATTRVALBLOCK attrValBlock = DrsuapiClient.CreateATTRVALBLOCK(new ATTRVAL[] { attrVal });
            ATTR attr = DrsuapiClient.CreateATTR(attrTyp, attrValBlock);
            ATTRBLOCK attrBlock = DrsuapiClient.CreateATTRBLOCK(new ATTR[] { attr });

            // Actual RPC call.
            ret = drsTestClient.DrsVerifyNames(
                srv,
                dwInVersion_Values.V1,
                DRS_MSG_VERIFYREQ_V1_dwFlags_Values.DRS_VERIFY_SAM_ACCOUNT_NAMES,
                new DSNAME[] { dsUser },
                new string[] { userDn },
                attrBlock,
                prefixTable
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSVerifyNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #endregion
    }
}
