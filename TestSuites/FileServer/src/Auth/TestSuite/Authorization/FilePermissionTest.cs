// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Auth.TestSuite;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Protocols.TestSuites.FileSharing.Auth.TestSuite
{
    [TestClass]
    public class FilePermissionTest : AuthorizationTestBase
    {
        private Smb2FunctionalClient client;
        /// <summary>
        /// UNC path of the share used to test file permission
        /// </summary>
        private string FilePermissionTestShareUncPath;

        /// <summary>
        /// Indicates if the FilePermissionTestShare exists or not.
        /// </summary>
        private bool FilePermissionTestShareExist; 
        private string tempFileName;
        private _SECURITY_DESCRIPTOR baseSD;

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
            if (FilePermissionTestShareUncPath == null)
            {
                FilePermissionTestShareUncPath = Smb2Utility.GetUncPath(testConfig.SutComputerName, TestConfig.FilePermissionTestShare);
                FilePermissionTestShareExist = ShareExists(TestConfig.AccountCredential, FilePermissionTestShareUncPath);
            }
            if (!FilePermissionTestShareExist)
            {
                BaseTestSite.Assert.Inconclusive("Required share does not exist: {0}", FilePermissionTestShareUncPath);
            }

            tempFileName = GetTestFileName(FilePermissionTestShareUncPath);
            CreateNewFile(FilePermissionTestShareUncPath, tempFileName);
            baseSD = QuerySecurityDescriptor(FilePermissionTestShareUncPath, tempFileName,
                AdditionalInformation_Values.DACL_SECURITY_INFORMATION |
                AdditionalInformation_Values.GROUP_SECURITY_INFORMATION |
                AdditionalInformation_Values.OWNER_SECURITY_INFORMATION);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Original Security Descriptor of the file ({0}): {1}",
                Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.FilePermissionTestShare, tempFileName), DtypUtility.ToSddlString(baseSD));
            client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
        }

        protected override void TestCleanup()
        {
            client.Disconnect();
            DeleteExistingFile(FilePermissionTestShareUncPath, tempFileName);
            base.TestCleanup();
        }
        #endregion

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.FileAccessCheck)]
        [Description("This test case is designed to test whether a user can read a file when " +
            "ACCESS_ALLOWED_ACE with user SID exists in file Security Descriptor.")]
        public void BVT_FilePermission_AccessAllow_UserSid()
        {
            _SID sid = DtypUtility.GetSidFromAccount(TestConfig.DomainName, azUser01Name);
            object ace = DtypUtility.CreateAccessAllowedAce(sid, DtypUtility.ACCESS_MASK_GENERIC_READ, ACE_FLAGS.None);
            SetSecurityDescriptorOnFile(ace);

            BaseTestSite.Assert.IsTrue(
                TryReadFile(), 
                "ACCESS_ALLOWED_ACE with user SID ({0}) exists in file Security Descriptor. User should be able to read the file.", 
                DtypUtility.ToSddlString(sid));
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.FileAccessCheck)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether a user can read a file when " +
            "ACCESS_ALLOWED_ACE with user's group SID exists in file Security Descriptor.")]
        public void FilePermission_AccessAllow_GroupSid()
        {
            _SID sid = DtypUtility.GetSidFromAccount(TestConfig.DomainName, azGroup01Name);
            object ace = DtypUtility.CreateAccessAllowedAce(sid, DtypUtility.ACCESS_MASK_GENERIC_READ, ACE_FLAGS.None);
            SetSecurityDescriptorOnFile(ace);

            BaseTestSite.Assert.IsTrue(
                TryReadFile(), 
                "ACCESS_ALLOWED_ACE with user's group SID ({0}) exists in file Security Descriptor. User should be able to read the file.", 
                DtypUtility.ToSddlString(sid));
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.FileAccessCheck)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether a user is denied to read a file when " +
            "ACCESS_DENIED_ACE with user SID exists in file Security Descriptor.")]
        public void FilePermission_AccessDeny_UserSid()
        {
            _SID sid = DtypUtility.GetSidFromAccount(TestConfig.DomainName, azUser01Name);
            object ace = DtypUtility.CreateAccessDeniedAce(sid, DtypUtility.ACCESS_MASK_GENERIC_READ, ACE_FLAGS.None);
            SetSecurityDescriptorOnFile(ace);

            BaseTestSite.Assert.IsFalse(
                TryReadFile(), 
                "ACCESS_DENIED_ACE with user SID ({0}) exists in folder Security Descriptor. User should not be able to read the file.", 
                DtypUtility.ToSddlString(sid));
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.FileAccessCheck)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether a user is denied to read a file when " +
            "ACCESS_DENIED_ACE with user's group SID exists in file Security Descriptor.")]
        public void FilePermission_AccessDeny_GroupSid()
        {
            _SID sid = DtypUtility.GetSidFromAccount(TestConfig.DomainName, azGroup01Name);
            object ace = DtypUtility.CreateAccessDeniedAce(sid, DtypUtility.ACCESS_MASK_GENERIC_READ, ACE_FLAGS.None);
            SetSecurityDescriptorOnFile(ace);

            BaseTestSite.Assert.IsFalse(
                TryReadFile(), 
                "ACCESS_DENIED_ACE with user's group SID ({0}) exists in file Security Descriptor. User should be not able to read the file.", 
                DtypUtility.ToSddlString(sid));
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.FileAccessCheck)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether a user is denied to read a file when " +
            "user SID does not exist in file Security Descriptor.")]
        public void FilePermission_AccessDeny_SidNoInclude()
        {
            _SID sid = DtypUtility.GetSidFromAccount(TestConfig.DomainName, azUser01Name);

            BaseTestSite.Assert.IsFalse(
                TryReadFile(), 
                "User SID ({0}) is not in file Security Descriptor. User should not be able to read the file.", 
                DtypUtility.ToSddlString(sid));
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.FileAccessCheck)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether a user is denied to read a file when " +
            "ACCESS_ALLOWED_ACE associated with the user does not have READ permission in file Security Descriptor.")]
        public void FilePermission_AccessDeny_UserSidWithoutReadPermission()
        {
            _SID sid = DtypUtility.GetSidFromAccount(TestConfig.DomainName, azUser01Name);
            object ace = DtypUtility.CreateAccessAllowedAce(sid, 0, ACE_FLAGS.None); // 0 stands for non access mask flag set
            SetSecurityDescriptorOnFile(ace);

            BaseTestSite.Assert.IsFalse(
                TryReadFile(), 
                "ACCESS_ALLOWED_ACE with user SID ({0}) without READ permission in folder Security Descriptor. User should not be able to read the file.", 
                DtypUtility.ToSddlString(sid));
        }

        private void SetSecurityDescriptorOnFile(object ace)
        {
            _ACL oriAcl = baseSD.Dacl.Value;
            DtypUtility.AddAceToAcl(ref oriAcl, true, ace);
            baseSD.Dacl = oriAcl;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Set Security Descriptor on the temp file ({0}): {1}.", FilePermissionTestShareUncPath, DtypUtility.ToSddlString(baseSD));
            SetSecurityDescriptor(FilePermissionTestShareUncPath, tempFileName, baseSD, SET_INFO_Request_AdditionalInformation_Values.DACL_SECURITY_INFORMATION);
        }

        private bool TryReadFile()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, 
                "Try read the file {0} using account: {1}@{2}.", 
                Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.FilePermissionTestShare, tempFileName), 
                azUser01Name, 
                TestConfig.DomainName);
            bool result = TryReadFile(client, new AccountCredential(TestConfig.DomainName, azUser01Name, TestConfig.UserPassword), FilePermissionTestShareUncPath, tempFileName);
            return result;
        }
    }
}
