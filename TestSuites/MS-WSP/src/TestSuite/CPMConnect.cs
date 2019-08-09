// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP.Adapter;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestSuites.WspTS
{
    [TestClass]
    public partial class CPMConnectTestCases: TestClassBase
    {
        private WspAdapter wspAdapter;
        private const uint InvalidClientVersion = 0;
        private const string InvalidCatalogNameFormat = "InvalidCatalogNameFormat";
        private string EmptyCatalogName = string.Empty;
        private const string NotExistedCatalogName = "Windows\\SYSTEM\0";

        private enum ArgumentType
        {
            AllValid,
            InvalidIsClientRemote,
            InvalidClientVersion,
            EmptyCatalogName,
            InvalidCatalogNameFormat,
            NotExistedCatalogName
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
            wspAdapter = new WspAdapter();
            wspAdapter.Initialize(this.Site);
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
            wspAdapter.CPMConnectInRequest();
        }

        [TestMethod]
        [TestCategory("CPMConnect")]
        [Description("This test case is designed to verify the server response if invalid isClientRemote is sent in CPMConnectIn.")]
        public void CPMGetRows_InvalidIsClientRemote()
        {            
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn with invalid isClientRemote and expects STATUS_INVALID_PARAMETER.");           

            argumentType = ArgumentType.InvalidIsClientRemote;
            wspAdapter.CPMConnectInRequest(wspAdapter.clientVersion, (int)ClientType.LocalClient, wspAdapter.catalogName);
        }

        [TestMethod]
        [TestCategory("CPMConnect")]
        [Description("This test case is designed to verify the server response if invalid client version is sent in CPMConnectIn.")]
        public void CPMGetRows_InvalidClientVersion()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn with invalid client version and expects STATUS_INVALID_PARAMETER_MIX.");

            argumentType = ArgumentType.InvalidClientVersion;
            wspAdapter.CPMConnectInRequest((uint)InvalidClientVersion, (int)ClientType.RemoteClient, wspAdapter.catalogName);
        }

        [TestMethod]
        [TestCategory("CPMConnect")]
        [Description("This test case is designed to verify the server response if empty catalog name is sent in CPMConnectIn.")]
        public void CPMGetRows_EmptyCatalogName()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn with empty catalog name and expects NOT SUCCEED.");

            argumentType = ArgumentType.EmptyCatalogName;
            wspAdapter.CPMConnectInRequest(wspAdapter.clientVersion, (int)ClientType.RemoteClient, EmptyCatalogName);
            //E_OUTOFMEMORY
        }

        [TestMethod]
        [TestCategory("CPMConnect")]
        [Description("This test case is designed to verify the server response if invalid catalog name format is sent in CPMConnectIn.")]
        public void CPMGetRows_InvalidCatalogNameFormat()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn with invalid catalog name format and expects NOT SUCCEED.");

            argumentType = ArgumentType.InvalidCatalogNameFormat;
            wspAdapter.CPMConnectInRequest(wspAdapter.clientVersion, (int)ClientType.RemoteClient, InvalidCatalogNameFormat);
            //0xd000000d
        }

        [TestMethod]
        [TestCategory("CPMConnect")]
        [Description("This test case is designed to verify the server response if not existed catalog name is sent in CPMConnectIn.")]
        public void CPMGetRows_NotExistedCatalogName()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn with not existed catalog name and expects NOT SUCCEED.");

            argumentType = ArgumentType.NotExistedCatalogName;
            wspAdapter.CPMConnectInRequest(wspAdapter.clientVersion, (int)ClientType.RemoteClient, NotExistedCatalogName);
            //MSS_E_CATALOGNOTFOUND
        }
        #endregion

        private void CPMConnectOut(uint errorCode)
        {
            switch (argumentType)
            {
                case ArgumentType.AllValid:
                    Site.Assert.AreEqual((uint)0, errorCode, "CPMConnectIn should succeed.");
                    break;
                case ArgumentType.InvalidIsClientRemote:
                    Site.Assert.AreEqual((uint)WspErrorCode.STATUS_INVALID_PARAMETER, errorCode, "CPMGetRowsOut should return STATUS_INVALID_PARAMETER if isClientRemote of CPMGetRowsIn is invalid.");
                    break;
                case ArgumentType.InvalidClientVersion:
                    Site.Assert.AreEqual((uint)WspErrorCode.STATUS_INVALID_PARAMETER_MIX, errorCode, "CPMGetRowsOut should return STATUS_INVALID_PARAMETER_MIX if client version of CPMGetRowsIn is invalid.");
                    break;
                case ArgumentType.EmptyCatalogName:
                    Site.Assert.AreNotEqual((uint)WspErrorCode.SUCCESS, errorCode, "CPMGetRowsOut should not return succeed if catalog name of CPMGetRowsIn is empty.");
                    break;
                case ArgumentType.InvalidCatalogNameFormat:
                    Site.Assert.AreNotEqual((uint)WspErrorCode.SUCCESS, errorCode, "CPMGetRowsOut should not return succeed if catalog name format of CPMGetRowsIn is invalid.");
                    break;
                case ArgumentType.NotExistedCatalogName:
                    Site.Assert.AreEqual((uint)WspErrorCode.MSS_E_CATALOGNOTFOUND, errorCode, "CPMGetRowsOut should return MSS_E_CATALOGNOTFOUND if catalog name of CPMGetRowsIn not existed.");
                    break;
            }
        }
    }
}
