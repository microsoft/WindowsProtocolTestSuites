// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    [TestClass]
    public class DrsrFailureTestClassBase : DrsrTTTTestClassBase
    {
        public static DRSRFailureTestClient drsTestClient = new DRSRFailureTestClient();
        protected ILdapAdapter ldapAdpter;
        protected UpdatesStorage updateStorage;
        protected IDrsrSutControlAdapter sutControlAdapter;

        #region Class Initialization and Cleanup
        public static void BaseInitialize(TestContext context)
        {
            TestClassBase.Initialize(context);
            BaseTestSite.DefaultProtocolDocShortName = BaseTestSite.Properties["ProtocolName"];
            drsTestClient.Initialize(BaseTestSite);
           
        }

        public static void BaseCleanup()
        {
            TestClassBase.Cleanup();

        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            DrsrFailureTestClassBase.BaseInitialize(context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            DrsrFailureTestClassBase.BaseCleanup();
        }
        #endregion

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            base.TestInitialize();
            ldapAdpter = BaseTestSite.GetAdapter<ILdapAdapter>();
            ldapAdpter.Site = BaseTestSite;
            sutControlAdapter = BaseTestSite.GetAdapter<IDrsrSutControlAdapter>();
            updateStorage = UpdatesStorage.GetInstance();
            drsTestClient.Reset();
            EnvironmentConfig.ExpectSuccess = false;
        }

        protected override void TestCleanup()
        {
            drsTestClient.Reset();
            base.TestCleanup();
        }
        #endregion
    }


}
