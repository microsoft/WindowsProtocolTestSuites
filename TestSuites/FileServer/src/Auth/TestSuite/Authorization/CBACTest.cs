// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestSuites.FileSharing.Auth.TestSuite
{
    [TestClass]
    public class CBACTest : AuthorizationTestBase
    {
        /// <summary>
        /// Save all the central access policies used by cases
        /// </summary>
        private Dictionary<string, CentralAccessPolicy> caps;

        /// <summary>
        /// Save all the information of users
        /// </summary>
        private Dictionary<string, User> users;

        /// <summary>
        /// UNC path of the CBAC share.
        /// </summary>
        private string CBACShareUncPath;

        /// <summary>
        /// Indicates if the CBAC share exists or not.
        /// </summary>
        private bool CBACShareExist; 

        /// <summary>
        /// User: Payrollmember01
        /// Attribute department: Payroll
        /// Attribute countryCode: 156
        /// </summary>
        private const string Payrollmember01 = "Payrollmember01";

        /// <summary>
        /// User: Payrollmember02
        /// Attribute department: Payroll
        /// Attribute countryCode: 840
        /// </summary>
        private const string Payrollmember02 = "Payrollmember02";

        /// <summary>
        /// User: Payrollmember03
        /// Attribute department: Payroll
        /// Attribute countryCode: 392
        /// </summary>
        private const string Payrollmember03 = "Payrollmember03";

        /// <summary>
        /// User: ITadmin01
        /// Attribute department: IT
        /// Attribute countryCode: 156
        /// </summary>
        private const string ITadmin01 = "ITadmin01";

        /// <summary>
        /// User: ITmember01
        /// Attribute department: IT
        /// Attribute countryCode: 392
        /// </summary>
        private const string ITmember01 = "ITmember01";

        /// <summary>
        /// User: noclaimuser
        /// </summary>
        private const string noclaimuser = "noclaimuser";

        #region Test Initialize and Cleanup
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }

        protected override void TestInitialize()
        {
            base.TestInitialize();
            if (CBACShareUncPath == null)
            {
                CBACShareUncPath = Smb2Utility.GetUncPath(testConfig.SutComputerName, TestConfig.CBACShare);
                CBACShareExist = ShareExists(TestConfig.AccountCredential, CBACShareUncPath);
            }
            if (!CBACShareExist)
            {
                BaseTestSite.Assert.Inconclusive("Required share does not exist: {0}", CBACShareUncPath);
            }
            if (caps == null)
            {
                caps = QueryCaps(TestConfig.DomainName, TestConfig.UserName, TestConfig.UserPassword);
            }
            if (users == null)
            {
                users = QueryUserInfo(TestConfig.DomainName, TestConfig.UserName, TestConfig.UserPassword);
            }
        }

        protected override void TestCleanup()
        {
            RemoveCentralAccessPolicy();
            base.TestCleanup();
        }
        #endregion

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.CBAC)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("This test case is designed to test whether users can access the share if the policy CountryCodeEquals156Policy is applied on the share.")]
        public void BVT_CBAC_CountryCodeEquals156Policy()
        {
            SetCentralAccessPolicy("CountryCodeEquals156Policy");

            AccessShareWithSpecifiedUser(Payrollmember01, true);
            AccessShareWithSpecifiedUser(Payrollmember02, false);
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.CBAC)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether users can access the share if the policy CountryCodeNotEquals156Policy is applied on the share.")]
        public void CBAC_CountryCodeNotEquals156Policy()
        {
            SetCentralAccessPolicy("CountryCodeNotEquals156Policy");

            AccessShareWithSpecifiedUser(Payrollmember01, false);
            AccessShareWithSpecifiedUser(Payrollmember02, true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.CBAC)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether users can access the share if the policy CountryCodeAnyOf156Or840Policy is applied on the share.")]
        public void CBAC_CountryCodeAnyOf156Or840Policy()
        {
            SetCentralAccessPolicy("CountryCodeAnyOf156Or840Policy");

            AccessShareWithSpecifiedUser(Payrollmember01, true);
            AccessShareWithSpecifiedUser(Payrollmember02, true);
            AccessShareWithSpecifiedUser(Payrollmember03, false);
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.CBAC)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether users can access the share if the policy CountryCodeNotAnyOf156Or840Policy is applied on the share.")]
        public void CBAC_CountryCodeNotAnyOf156Or840Policy()
        {
            SetCentralAccessPolicy("CountryCodeNotAnyOf156Or840Policy");

            AccessShareWithSpecifiedUser(Payrollmember01, false);
            AccessShareWithSpecifiedUser(Payrollmember02, false);
            AccessShareWithSpecifiedUser(Payrollmember03, true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.CBAC)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether users can access the share if the policy CountryCodeGreaterThan392Policy is applied on the share.")]
        public void CBAC_CountryCodeGreaterThan392Policy()
        {
            SetCentralAccessPolicy("CountryCodeGreaterThan392Policy");

            AccessShareWithSpecifiedUser(Payrollmember01, false);
            AccessShareWithSpecifiedUser(Payrollmember02, true);
            AccessShareWithSpecifiedUser(Payrollmember03, false);
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.CBAC)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether users can access the share if the policy CountryCodeGreaterThanOrEquals392Policy is applied on the share.")]
        public void CBAC_CountryCodeGreaterThanOrEquals392Policy()
        {
            SetCentralAccessPolicy("CountryCodeGreaterThanOrEquals392Policy");

            AccessShareWithSpecifiedUser(Payrollmember01, false);
            AccessShareWithSpecifiedUser(Payrollmember02, true);
            AccessShareWithSpecifiedUser(Payrollmember03, true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.CBAC)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether users can access the share if the policy CountryCodeLessThan392Policy is applied on the share.")]
        public void CBAC_CountryCodeLessThan392Policy()
        {
            SetCentralAccessPolicy("CountryCodeLessThan392Policy");

            AccessShareWithSpecifiedUser(Payrollmember01, true);
            AccessShareWithSpecifiedUser(Payrollmember02, false);
            AccessShareWithSpecifiedUser(Payrollmember03, false);
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.CBAC)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether users can access the share if the policy CountryCodeLessThanOrEquals392Policy is applied on the share.")]
        public void CBAC_CountryCodeLessThanOrEquals392Policy()
        {
            SetCentralAccessPolicy("CountryCodeLessThanOrEquals392Policy");

            AccessShareWithSpecifiedUser(Payrollmember01, true);
            AccessShareWithSpecifiedUser(Payrollmember02, false);
            AccessShareWithSpecifiedUser(Payrollmember03, true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.CBAC)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether users can access the share if the policy CountryCodeEquals156AndITDepartmentPolicy is applied on the share.")]
        public void CBAC_CountryCodeEquals156AndITDepartmentPolicy()
        {
            SetCentralAccessPolicy("CountryCodeEquals156AndITDepartmentPolicy");

            AccessShareWithSpecifiedUser(Payrollmember01, false);
            AccessShareWithSpecifiedUser(Payrollmember02, false);
            AccessShareWithSpecifiedUser(ITmember01, false);
            AccessShareWithSpecifiedUser(ITadmin01, true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.CBAC)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether users can access the share if the policy CountryCodeEquals156OrITDepartmentPolicy is applied on the share.")]
        public void CBAC_CountryCodeEquals156OrITDepartmentPolicy()
        {
            SetCentralAccessPolicy("CountryCodeEquals156OrITDepartmentPolicy");

            AccessShareWithSpecifiedUser(Payrollmember01, true);
            AccessShareWithSpecifiedUser(Payrollmember02, false);
            AccessShareWithSpecifiedUser(ITmember01, true);
            AccessShareWithSpecifiedUser(ITadmin01, true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.CBAC)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether noclaimuser can access the share if any policy is applied on the share.")]
        public void CBAC_NoUserClaimBlockWriteControl()
        {
            SetCentralAccessPolicy("CountryCodeEquals156Policy");

            AccessShareWithSpecifiedUser(noclaimuser, false);
        }

        private void AccessShareWithSpecifiedUser(string userName, bool expectedResult)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Access the share {0} using account: {1}@{2}.", CBACShareUncPath, userName, TestConfig.DomainName);
            BaseTestSite.Log.Add(LogEntryKind.Debug, "User info: {0}", users[userName]);
            bool result = AccessShare(new AccountCredential(TestConfig.DomainName, userName, TestConfig.UserPassword), CBACShareUncPath);
            if (expectedResult)
            {
                BaseTestSite.Assert.IsTrue(result, "User {0} should be able to access the share", userName);
            }
            else
            {
                BaseTestSite.Assert.IsFalse(result, "User {0} should not be able to access the share", userName);
            }
        }

        private void SetCentralAccessPolicy(string capName)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Set Central Access Policy: {0} to the share: {1}", capName, CBACShareUncPath);
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Policy info: {0}", caps[capName]);
            _SID capid = caps[capName].Id;
            base.SetCap(CBACShareUncPath, capid);
        }

        private void RemoveCentralAccessPolicy()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Clear CAP on the share: {0}", CBACShareUncPath);
            base.SetCap(CBACShareUncPath, null);
        }
    }
}
