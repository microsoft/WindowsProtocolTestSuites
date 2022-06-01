// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Protocols.TestSuites.WspTS
{
    [TestClass]
    public partial class CPMConnectTestCases : WspCommonTestBase
    {
        private const uint invalidClientVersion = 0;
        private const string invalidCatalogNameFormat = "InvalidCatalogNameFormat";
        private string emptyCatalogName = string.Empty;
        private const string notExistingCatalogName = "Windows\\AYSTEMINDEX";

        private enum ArgumentType
        {
            AllValid,
            InvalidIsClientRemote,
            InvalidClientVersion,
            EmptyCatalogName,
            InvalidCatalogNameFormat,
            NotExistingCatalogName,
            SmallerCExtPropSet,
            LargerCExtPropSet
        }

        private enum ClientType
        {
            LocalClient = 0,
            RemoteClient = 1
        }

        private ArgumentType argumentType;

        #region Test Class Initialize and Cleanup
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }
        #endregion

        #region Test Case Initialize and Cleanup
        protected override void TestInitialize()
        {
            base.TestInitialize();

            wspAdapter.CPMConnectOutResponse -= EnsureSuccessfulCPMConnectOut;
            wspAdapter.CPMConnectOutResponse += CPMConnectOut;
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }
        #endregion

        #region Test Cases
        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("CPMConnect")]
        [Description("This test case is designed to test the basic functionality of CPMConnect.")]
        public void BVT_CPMConnect()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");

            argumentType = ArgumentType.AllValid;
            wspAdapter.CPMConnectIn();
        }

        [TestMethod]
        [TestCategory("CPMConnect")]
        [Description("This test case is designed to verify the server response if invalid isClientRemote is sent in CPMConnectIn.")]
        public void CPMConnect_InvalidIsClientRemote()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn with invalid isClientRemote and expects STATUS_INVALID_PARAMETER.");

            argumentType = ArgumentType.InvalidIsClientRemote;
            wspAdapter.CPMConnectIn(wspAdapter.ClientVersion, (int)ClientType.LocalClient, wspAdapter.CatalogName);
        }

        [TestMethod]
        [TestCategory("CPMConnect")]
        [Description("This test case is designed to verify the server response if invalid client version is sent in CPMConnectIn.")]
        public void CPMConnect_InvalidClientVersion()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn with invalid client version and expects STATUS_INVALID_PARAMETER_MIX.");

            argumentType = ArgumentType.InvalidClientVersion;
            wspAdapter.CPMConnectIn((uint)invalidClientVersion, (int)ClientType.RemoteClient, wspAdapter.CatalogName);
        }

        [TestMethod]
        [TestCategory("CPMConnect")]
        [Description("This test case is designed to verify the server response if empty catalog name is sent in CPMConnectIn.")]
        public void CPMConnect_EmptyCatalogName()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn with empty catalog name and expects NOT SUCCEED.");

            argumentType = ArgumentType.EmptyCatalogName;
            wspAdapter.CPMConnectIn(wspAdapter.ClientVersion, (int)ClientType.RemoteClient, emptyCatalogName);
            //E_OUTOFMEMORY
        }

        [TestMethod]
        [TestCategory("CPMConnect")]
        [Description("This test case is designed to verify the server response if invalid catalog name format is sent in CPMConnectIn.")]
        public void CPMConnect_InvalidCatalogNameFormat()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn with invalid catalog name format and expects NOT SUCCEED.");

            argumentType = ArgumentType.InvalidCatalogNameFormat;
            wspAdapter.CPMConnectIn(wspAdapter.ClientVersion, (int)ClientType.RemoteClient, invalidCatalogNameFormat);
            //0xd000000d
        }

        [TestMethod]
        [TestCategory("CPMConnect")]
        [Description("This test case is designed to verify the server response if not existing catalog name is sent in CPMConnectIn.")]
        public void CPMConnect_NotExistingCatalogName()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn with not existed catalog name and expects NOT SUCCEED.");

            argumentType = ArgumentType.NotExistingCatalogName;
            wspAdapter.CPMConnectIn((uint)Convert.ToUInt32
                (BaseTestSite.Properties["ClientVersion"]), (int)ClientType.RemoteClient, notExistingCatalogName);
            //MSS_E_CATALOGNOTFOUND
        }

        [TestMethod]
        [TestCategory("CPMConnect")]
        [Description("This test case is designed to verify the server response if smaller cExtPropSet field is sent in CPMConnectIn.")]
        public void CPMConnect_SmallerCExtPropSet()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn with smaller cExtPropSet field and expects NOT SUCCEED.");

            argumentType = ArgumentType.SmallerCExtPropSet;
            wspAdapter.CPMConnectIn((uint)Convert.ToUInt32
                (BaseTestSite.Properties["ClientVersion"]), (int)ClientType.RemoteClient, wspAdapter.CatalogName, cExtPropSet: 2);
        }

        [TestMethod]
        [TestCategory("CPMConnect")]
        [Description("This test case is designed to verify the server response if larger cExtPropSet field is sent in CPMConnectIn.")]
        public void CPMConnect_LargerCExtPropSet()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn with larger cExtPropSet field and expects NOT SUCCEED.");

            argumentType = ArgumentType.LargerCExtPropSet;
            wspAdapter.CPMConnectIn((uint)Convert.ToUInt32
                (BaseTestSite.Properties["ClientVersion"]), (int)ClientType.RemoteClient, wspAdapter.CatalogName, cExtPropSet: 5);
        }
        #endregion

        private void CPMConnectOut(uint errorCode)
        {
            switch (argumentType)
            {
                case ArgumentType.AllValid:
                    Site.Assert.AreEqual((uint)WspErrorCode.SUCCESS, errorCode, "Server should return succeed for COMConnectIn.");
                    break;
                case ArgumentType.InvalidIsClientRemote:
                    Site.Assert.AreEqual((uint)WspErrorCode.STATUS_INVALID_PARAMETER, errorCode, "Server should return STATUS_INVALID_PARAMETER if isClientRemote of CPMConnectIn is invalid.");
                    break;
                case ArgumentType.InvalidClientVersion:
                    Site.Assert.AreEqual((uint)WspErrorCode.STATUS_INVALID_PARAMETER_MIX, errorCode, "Server should return STATUS_INVALID_PARAMETER_MIX if client version of CPMConnectIn is invalid.");
                    break;
                case ArgumentType.EmptyCatalogName:
                    Site.Assert.AreNotEqual((uint)WspErrorCode.SUCCESS, errorCode, "Server should not return succeed if catalog name of CPMConnectIn is empty.");
                    break;
                case ArgumentType.InvalidCatalogNameFormat:
                    Site.Assert.AreNotEqual((uint)WspErrorCode.SUCCESS, errorCode, "Server should not return succeed if catalog name format of CPMConnectIn is invalid.");
                    break;
                case ArgumentType.NotExistingCatalogName:
                    Site.Assert.AreEqual((uint)WspErrorCode.MSS_E_CATALOGNOTFOUND, errorCode, "Server should return MSS_E_CATALOGNOTFOUND if catalog name of CPMConnectIn is not existed.");
                    break;
                case ArgumentType.SmallerCExtPropSet:
                    Site.Assert.AreEqual((uint)WspErrorCode.ERROR_INVALID_PARAMETER, errorCode, "Server should return ERROR_INVALID_PARAMETER if cExtPropSet of CPMConnectIn is smaller than the number of CDbPropSet structures in aPropertySets.");
                    break;
                case ArgumentType.LargerCExtPropSet:
                    Site.Assert.AreEqual((uint)WspErrorCode.E_ABORT, errorCode, "Server should return E_ABORT if cExtPropSet of CPMConnectIn is larger than the number of CDbPropSet structures in aPropertySets.");
                    break;
            }
        }
    }
}
