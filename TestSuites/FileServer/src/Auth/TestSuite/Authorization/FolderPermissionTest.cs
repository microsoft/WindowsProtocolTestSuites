// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.FileSharing.Auth.TestSuite
{
    [TestClass]
    public class FolderPermissionTest : AuthorizationTestBase
    {
        private _SECURITY_DESCRIPTOR originalSD;

        /// <summary>
        /// UNC path of the share used to test folder permission
        /// </summary>
        private string FolderPermissionTestShareUncPath;

        /// <summary>
        /// Indicates if the FolderPermissionTestShare exists or not.
        /// </summary>
        private bool FolderPermissionTestShareExist; 

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
            if (FolderPermissionTestShareUncPath == null)
            {
                FolderPermissionTestShareUncPath = Smb2Utility.GetUncPath(testConfig.SutComputerName, TestConfig.FolderPermissionTestShare);
                FolderPermissionTestShareExist = ShareExists(TestConfig.AccountCredential, FolderPermissionTestShareUncPath);
            }
            if (!FolderPermissionTestShareExist)
            {
                BaseTestSite.Assert.Inconclusive("Required share does not exist: {0}", FolderPermissionTestShareUncPath);
            }

            originalSD = QuerySecurityDescriptor(FolderPermissionTestShareUncPath, null,
                AdditionalInformation_Values.DACL_SECURITY_INFORMATION |
                AdditionalInformation_Values.GROUP_SECURITY_INFORMATION |
                AdditionalInformation_Values.OWNER_SECURITY_INFORMATION);
        }

        protected override void TestCleanup()
        {
            SetSecurityDescriptor(FolderPermissionTestShareUncPath, null, originalSD,
                SET_INFO_Request_AdditionalInformation_Values.DACL_SECURITY_INFORMATION |
                SET_INFO_Request_AdditionalInformation_Values.GROUP_SECURITY_INFORMATION |
                SET_INFO_Request_AdditionalInformation_Values.OWNER_SECURITY_INFORMATION);

            base.TestCleanup();
        }
        #endregion

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.FolderAccessCheck)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("This test case is designed to test whether a user can access a share when " +
            "ACCESS_ALLOWED_ACE with user SID exists in folder Security Descriptor.")]
        public void BVT_FolderPermission_AccessAllow_UserSid()
        {
            _SID sid = DtypUtility.GetSidFromAccount(TestConfig.DomainName, azUser01Name);
            object ace = DtypUtility.CreateAccessAllowedAce(sid, DtypUtility.ACCESS_MASK_GENERIC_READ, ACE_FLAGS.None);
            SetSecurityDescriptorOnShare(ace);

            BaseTestSite.Assert.IsTrue(
                AccessShare(), 
                "ACCESS_ALLOWED_ACE with user SID ({0}) exists in folder Security Descriptor. User should be able to access the share.", 
                DtypUtility.ToSddlString(sid));
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.FolderAccessCheck)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether a user can access a share when " +
            "ACCESS_ALLOWED_ACE with user's group SID exists in folder Security Descriptor.")]
        public void FolderPermission_AccessAllow_GroupSid()
        {
            _SID sid = DtypUtility.GetSidFromAccount(TestConfig.DomainName, azGroup01Name);
            object ace = DtypUtility.CreateAccessAllowedAce(sid, DtypUtility.ACCESS_MASK_GENERIC_READ, ACE_FLAGS.None);
            SetSecurityDescriptorOnShare(ace);

            BaseTestSite.Assert.IsTrue(
                AccessShare(), 
                "ACCESS_ALLOWED_ACE with user's group SID ({0}) exists in share Security Descriptor. User should be able to access the share.", 
                DtypUtility.ToSddlString(sid));
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.FolderAccessCheck)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether a user is denied to access a share when " +
            "ACCESS_DENIED_ACE with user SID exists in folder Security Descriptor.")]
        public void FolderPermission_AccessDeny_UserSid()
        {
            _SID sid = DtypUtility.GetSidFromAccount(TestConfig.DomainName, azUser01Name);
            object ace = DtypUtility.CreateAccessDeniedAce(sid, DtypUtility.ACCESS_MASK_GENERIC_READ, ACE_FLAGS.None);
            SetSecurityDescriptorOnShare(ace);

            BaseTestSite.Assert.IsFalse(
                AccessShare(), 
                "ACCESS_DENIED_ACE with user SID ({0}) exists in folder Security Descriptor. User should not be able to access the share.", 
                DtypUtility.ToSddlString(sid));
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.FolderAccessCheck)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether a user is denied to access a share when " +
            "ACCESS_DENIED_ACE with user's group SID exists in folder Security Descriptor.")]
        public void FolderPermission_AccessDeny_GroupSid()
        {
            _SID sid = DtypUtility.GetSidFromAccount(TestConfig.DomainName, azGroup01Name);
            object ace = DtypUtility.CreateAccessDeniedAce(sid, DtypUtility.ACCESS_MASK_GENERIC_READ, ACE_FLAGS.None);
            SetSecurityDescriptorOnShare(ace);

            BaseTestSite.Assert.IsFalse(
                AccessShare(), 
                "ACCESS_DENIED_ACE with user's group SID ({0}) exists in file Security Descriptor. User should be not able to access the share.",
                DtypUtility.ToSddlString(sid));
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.FolderAccessCheck)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether a user is denied to access a share when " +
            "user SID does not exist in folder Security Descriptor.")]
        public void FolderPermission_AccessDeny_SidNoInclude()
        {
            _SID sid = DtypUtility.GetSidFromAccount(TestConfig.DomainName, azUser01Name);
            BaseTestSite.Assert.IsFalse(
                AccessShare(), 
                "User SID ({0}) is not in folder Security Descriptor. User should not be able to access the share.", 
                DtypUtility.ToSddlString(sid));
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.FolderAccessCheck)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether a user is denied to access a share when " +
            "ACCESS_ALLOWED_ACE associated with the user does not have READ permission in folder Security Descriptor.")]
        public void FolderPermission_AccessDeny_UserSidWithoutReadPermission()
        {
            _SID sid = DtypUtility.GetSidFromAccount(TestConfig.DomainName, azUser01Name);
            object ace = DtypUtility.CreateAccessAllowedAce(sid, 0, ACE_FLAGS.None);
            SetSecurityDescriptorOnShare(ace);

            BaseTestSite.Assert.IsFalse(
                AccessShare(), 
                "ACCESS_ALLOWED_ACE with user SID ({0}) without READ permission in folder Security Descriptor. User should not be able to access the share.", 
                DtypUtility.ToSddlString(sid));
        }

        private void SetSecurityDescriptorOnShare(object ace)
        {
            _ACL acl = DtypUtility.CreateAcl(true, ace);
            _SECURITY_DESCRIPTOR sd = originalSD;
            _ACL oriAcl = originalSD.Dacl.Value;
            DtypUtility.AddAceToAcl(ref oriAcl, true, ace);
            sd.Dacl = oriAcl;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Set Security Descriptor on the share ({0}): {1}.",
             FolderPermissionTestShareUncPath, DtypUtility.ToSddlString(sd));
            SetSecurityDescriptor(FolderPermissionTestShareUncPath, null, sd, SET_INFO_Request_AdditionalInformation_Values.DACL_SECURITY_INFORMATION);
        }

        private bool AccessShare()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Access the share \"{0}\" using account: {1}@{2}.", FolderPermissionTestShareUncPath, azUser01Name, TestConfig.DomainName);
            bool result = AccessShare(new AccountCredential(TestConfig.DomainName, azUser01Name, TestConfig.UserPassword), FolderPermissionTestShareUncPath);
            return result;
        }
    }
}
