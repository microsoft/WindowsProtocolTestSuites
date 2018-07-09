// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.using Microsoft.Protocol.TestSuites.Smbd.Adapter;

using Microsoft.Protocol.TestSuites.Smbd.Adapter;
using Microsoft.Protocols.TestSuites.Smbd.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Protocol.TestSuites.Smbd.TestSuite
{
    [TestClass]
    public class SmbdTestBase : TestClassBase
    {
        #region Variables
        private ISutProtocolControlAdapter sutProtocolControlAdapter;
        protected SmbdAdapter smbdAdapter;
        protected string fileName;
        #endregion

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            base.TestInitialize();
            sutProtocolControlAdapter = BaseTestSite.GetAdapter<ISutProtocolControlAdapter>();
            this.smbdAdapter = new SmbdAdapter(BaseTestSite, LogSmbdEndpointEvent);
            fileName = null;
            SmbdUtilities.LogTestCaseDescription(BaseTestSite);
        }

        protected override void TestCleanup()
        {
            if (!String.IsNullOrEmpty(fileName))
            {
                try
                {
                    sutProtocolControlAdapter.DeleteFile(fileName);
                }
                catch
                {

                }
            }

            try
            {
                smbdAdapter.DisconnectRdma();
            }
            catch
            {

            }

            smbdAdapter = null;

            base.TestCleanup();
        }
        #endregion

        #region Protected Method
        protected void LogSmbdEndpointEvent(string log)
        {
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                log);
        }
        #endregion
    }
}