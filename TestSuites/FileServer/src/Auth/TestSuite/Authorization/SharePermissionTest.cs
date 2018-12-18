// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.Srvs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Protocols.TestSuites.FileSharing.Auth.TestSuite
{
    [TestClass]
    public class SharePermissionTest : AuthorizationTestBase
    {
        private bool dynamicallyConfigurableShareExist;
        private string dynamicallyConfigurableShareName;
        private SHARE_INFO_502_I? originalShareInfo;

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

            if (dynamicallyConfigurableShareName == null)
            {
                dynamicallyConfigurableShareName = TestConfig.SharePermissionTestShare;
                dynamicallyConfigurableShareExist = ShareExists(TestConfig.AccountCredential, Smb2Utility.GetUncPath(testConfig.SutComputerName, dynamicallyConfigurableShareName));
            }

            if (dynamicallyConfigurableShareExist)
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Configurable share exists: {0}. MS-SRVS protocol will be used to set share permissions.",
                    Smb2Utility.GetUncPath(testConfig.SutComputerName, dynamicallyConfigurableShareName));
                if (originalShareInfo == null)
                {
                    originalShareInfo = GetShareInfo(dynamicallyConfigurableShareName);
                }
            }
        }

        protected override void TestCleanup()
        {
            if (dynamicallyConfigurableShareExist && originalShareInfo != null)
            {
                SetShareInfo(dynamicallyConfigurableShareName, originalShareInfo.Value);
            }
            base.TestCleanup();
        }
        #endregion

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.ShareAccessCheck)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("This test case is designed to test whether a user can access a share when " +
            "ACCESS_ALLOWED_ACE with user SID exists in share Security Descriptor.")]
        public void BVT_SharePermission_AccessAllow_UserSid()
        {
            _SID sid = DtypUtility.GetSidFromAccount(TestConfig.DomainName, azUser01Name);
            string shareName;
            if (dynamicallyConfigurableShareExist)
            {
                object ace = DtypUtility.CreateAccessAllowedAce(sid, DtypUtility.ACCESS_MASK_STANDARD_RIGHTS_ALL | DtypUtility.ACCESS_MASK_SPECIFIC_RIGHTS_ALL, ACE_FLAGS.None);
                SetSecurityDescriptorOnDynamicallyConfigurableShare(ace);
                shareName = dynamicallyConfigurableShareName;
            }
            else
            {
                shareName = "AzShare01";
            }

            bool result = AccessShare(shareName);
            BaseTestSite.Assert.IsTrue(result, "ACCESS_ALLOWED_ACE with user SID ({0}) exists in folder Security Descriptor. User should be able to access the share.", DtypUtility.ToSddlString(sid));
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.ShareAccessCheck)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether a user can access a share when " +
            "ACCESS_ALLOWED_ACE with user's group SID exists in share Security Descriptor.")]
        public void SharePermission_AccessAllow_GroupSid()
        {
            _SID sid = DtypUtility.GetSidFromAccount(TestConfig.DomainName, azGroup01Name);
            string shareName;
            if (dynamicallyConfigurableShareExist)
            {
                object ace = DtypUtility.CreateAccessAllowedAce(sid, DtypUtility.ACCESS_MASK_STANDARD_RIGHTS_ALL | DtypUtility.ACCESS_MASK_SPECIFIC_RIGHTS_ALL, ACE_FLAGS.None);
                SetSecurityDescriptorOnDynamicallyConfigurableShare(ace);
                shareName = dynamicallyConfigurableShareName;
            }
            else
            {
                shareName = "AzShare02";
            }

            BaseTestSite.Assert.IsTrue(
                AccessShare(shareName), 
                "ACCESS_ALLOWED_ACE with user's group SID ({0}) exists in share Security Descriptor. User should be able to access the share.", 
                DtypUtility.ToSddlString(sid));
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.ShareAccessCheck)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether a user is denied to access a share when " +
            "ACCESS_DENIED_ACE with user SID exists in share Security Descriptor.")]
        public void SharePermission_AccessDeny_UserSid()
        {
            _SID sid = DtypUtility.GetSidFromAccount(TestConfig.DomainName, azUser01Name);
            string shareName;
            if (dynamicallyConfigurableShareExist)
            {
                object ace = DtypUtility.CreateAccessDeniedAce(sid, DtypUtility.ACCESS_MASK_STANDARD_RIGHTS_ALL | DtypUtility.ACCESS_MASK_SPECIFIC_RIGHTS_ALL, ACE_FLAGS.None);
                SetSecurityDescriptorOnDynamicallyConfigurableShare(ace);
                shareName = dynamicallyConfigurableShareName;
            }
            else
            {
                shareName = "AzShare03";
            }

            bool result = AccessShare(shareName);
            BaseTestSite.Assert.IsFalse(result, "ACCESS_DENIED_ACE with user SID ({0}) exists in folder Security Descriptor. User should not be able to access the share.", DtypUtility.ToSddlString(sid));
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.ShareAccessCheck)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether a user is denied to access a share when " +
            "ACCESS_DENIED_ACE with user's group SID exists in share Security Descriptor.")]
        public void SharePermission_AccessDeny_GroupSid()
        {
            _SID sid = DtypUtility.GetSidFromAccount(TestConfig.DomainName, azGroup01Name);
            string shareName;
            if (dynamicallyConfigurableShareExist)
            {
                object ace = DtypUtility.CreateAccessDeniedAce(sid, DtypUtility.ACCESS_MASK_STANDARD_RIGHTS_ALL | DtypUtility.ACCESS_MASK_SPECIFIC_RIGHTS_ALL, ACE_FLAGS.None);
                SetSecurityDescriptorOnDynamicallyConfigurableShare(ace);
                shareName = dynamicallyConfigurableShareName;
            }
            else
            {
                shareName = "AzShare04";
            }

            BaseTestSite.Assert.IsFalse(
                AccessShare(shareName), 
                "ACCESS_DENIED_ACE with user's group SID ({0}) exists in file Security Descriptor. User should be not able to access the share.", 
                DtypUtility.ToSddlString(sid));
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.ShareAccessCheck)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether a user is denied to access a share when " +
            "user SID does not exist in share Security Descriptor.")]
        public void SharePermission_AccessDeny_SidNoInclude()
        {
            _SID sid = DtypUtility.GetSidFromAccount(TestConfig.DomainName, azUser01Name);
            string shareName;
            if (dynamicallyConfigurableShareExist)
            {
                shareName = dynamicallyConfigurableShareName;
            }
            else
            {
                shareName = "AzShare05";
            }

            BaseTestSite.Assert.IsFalse(
                AccessShare(shareName), 
                "User SID ({0}) is not in share Security Descriptor. User should not be able to access the share.", 
                DtypUtility.ToSddlString(sid));
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.ShareAccessCheck)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether a user can access a share when " +
            "ACCESS_ALLOWED_ACE with user SID exists in share Security Descriptor.")]
        public void SharePermission_AccessDeny_UserSidWithoutReadPermission()
        {
            _SID sid = DtypUtility.GetSidFromAccount(TestConfig.DomainName, azUser01Name);
            string shareName;
            if (dynamicallyConfigurableShareExist)
            {
                object ace = DtypUtility.CreateAccessDeniedAce(sid, 0, ACE_FLAGS.None);
                SetSecurityDescriptorOnDynamicallyConfigurableShare(ace);
                shareName = dynamicallyConfigurableShareName;
            }
            else
            {
                shareName = "AzShare06";
            }

            BaseTestSite.Assert.IsFalse(
                AccessShare(shareName), 
                "ACCESS_ALLOWED_ACE with user SID ({0}) without READ permission in folder Security Descriptor. User should not be able to access the share.", 
                DtypUtility.ToSddlString(sid));
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.ShareAccessCheck)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("This case is designed to test whether server can handle file deletion request when Treeconnect.MaximalAccess does not include DELETE or GENERIC_ALL.")]
        public void SharePermission_CreateClose_DeleteFile_MaximalAccessNotIncludeDeleteOrGenericAll()
        {
            _SID sid = DtypUtility.GetSidFromAccount(TestConfig.DomainName, azUser01Name);
            if (!dynamicallyConfigurableShareExist)
            {
                BaseTestSite.Assert.Inconclusive("Required share: {0} does not exist!", dynamicallyConfigurableShareName);
            }
            object ace = DtypUtility.CreateAccessAllowedAce(sid, (DtypUtility.ACCESS_MASK_STANDARD_RIGHTS_ALL | DtypUtility.ACCESS_MASK_SPECIFIC_RIGHTS_ALL) & ~DtypUtility.ACCESS_MASK_DELETE, ACE_FLAGS.None);
            SetSecurityDescriptorOnDynamicallyConfigurableShare(ace);
            string shareName = dynamicallyConfigurableShareName;
            string shareUncPath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, shareName);

            Smb2FunctionalClient client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            AccountCredential user = new AccountCredential(TestConfig.DomainName, azUser01Name, TestConfig.UserPassword);
            try
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Client sends NEGOTIATE message.");
                client.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Client sends SESSION_SETUP message using account: {0}@{1}.", user.AccountName, user.DomainName);
                client.SessionSetup(TestConfig.DefaultSecurityPackage, TestConfig.SutComputerName, user, false);

                uint treeId;
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Client sends TREE_CONNECT message to access share: {0}.", shareUncPath);
                client.TreeConnect(shareUncPath, out treeId, checker: (header, response) =>
                {
                    BaseTestSite.Assert.IsTrue((response.MaximalAccess.ACCESS_MASK & (DtypUtility.ACCESS_MASK_DELETE | DtypUtility.ACCESS_MASK_GENERIC_ALL)) == 0,
                        "Treeconnect.MaximalAccess does not include DELETE or GENERIC_ALL.");
                });

                string fileName = GetTestFileName(shareUncPath);
                FILEID fileId;
                Smb2CreateContextResponse[] createContexResponse;
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Create the file: {0}", fileName);
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Client sends CREATE request.");
                uint status = client.Create(
                    treeId,
                    fileName,
                    CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                    out fileId,
                    out createContexResponse,
                    accessMask: AccessMask.FILE_READ_DATA | AccessMask.FILE_WRITE_DATA | AccessMask.FILE_APPEND_DATA |
                    AccessMask.FILE_READ_ATTRIBUTES | AccessMask.FILE_READ_EA | AccessMask.FILE_WRITE_ATTRIBUTES | 
                    AccessMask.FILE_WRITE_EA | AccessMask.READ_CONTROL | AccessMask.WRITE_DAC | AccessMask.SYNCHRONIZE, // Windows client behavior
                    shareAccess: ShareAccess_Values.NONE,
                    createDisposition: CreateDisposition_Values.FILE_CREATE);
                client.Close(treeId, fileId);

                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Delete the file: {0}", fileName);
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Client sends CREATE request with FILE_DELETE_ON_CLOSE flag set in CreateOptions .");
                status = client.Create(
                    treeId,
                    fileName,
                    CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                    out fileId,
                    out createContexResponse,
                    accessMask: AccessMask.DELETE | AccessMask.FILE_READ_ATTRIBUTES | AccessMask.SYNCHRONIZE, // Windows client behavior
                    shareAccess: ShareAccess_Values.FILE_SHARE_DELETE,
                    createDisposition: CreateDisposition_Values.FILE_OPEN,
                    checker:(header, response) => 
                    {
                        if(TestConfig.Platform == Platform.NonWindows)
                        {
                            BaseTestSite.Assert.AreNotEqual(Smb2Status.STATUS_SUCCESS, header.Status,
                                "If the FILE_DELETE_ON_CLOSE flag is set in CreateOptions and " +
                                "Treeconnect.MaximalAccess does not include DELETE or GENERIC_ALL, " +
                                "the server SHOULD fail the request with STATUS_ACCESS_DENIED");
                        }
                        else
                        {
                            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_ACCESS_DENIED, header.Status, 
                                "If the FILE_DELETE_ON_CLOSE flag is set in CreateOptions and " + 
                                "Treeconnect.MaximalAccess does not include DELETE or GENERIC_ALL, " +
                                "the server SHOULD fail the request with STATUS_ACCESS_DENIED");
                        }
                    });

                client.TreeDisconnect(treeId);
                client.LogOff();
            }
            catch(Exception e)
            {
                BaseTestSite.Assert.Fail("Case failed due to: {0}", e.Message);
            }
            finally
            {
                client.Disconnect();
            }
        }

        private void SetSecurityDescriptorOnDynamicallyConfigurableShare(object ace)
        {
            _SECURITY_DESCRIPTOR sd = DtypUtility.DecodeSecurityDescriptor(originalShareInfo.Value.shi502_security_descriptor);

            _ACL dacl = sd.Dacl.Value;
            DtypUtility.AddAceToAcl(ref dacl, true, ace);
            sd.Dacl = dacl;

            DtypUtility.UpdateSecurityDescriptor(ref sd);

            SHARE_INFO_502_I newShareInfo = originalShareInfo.Value;
            newShareInfo.shi502_security_descriptor = DtypUtility.EncodeSecurityDescriptor(sd);
            newShareInfo.shi502_reserved = (uint)newShareInfo.shi502_security_descriptor.Length;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Set Security Descriptor on the share ({0}): {1}.",
                Smb2Utility.GetUncPath(TestConfig.SutComputerName, dynamicallyConfigurableShareName), DtypUtility.ToSddlString(sd));
            SetShareInfo(dynamicallyConfigurableShareName, newShareInfo);
        }

        private bool AccessShare(string shareName)
        {
            string shareUncPath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, shareName);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Access the share {0} using account: {1}@{2}.", shareUncPath, azUser01Name, TestConfig.DomainName);
            bool result = AccessShare(new AccountCredential(TestConfig.DomainName, azUser01Name, TestConfig.UserPassword), shareUncPath);
            return result;
        }
    }
}
