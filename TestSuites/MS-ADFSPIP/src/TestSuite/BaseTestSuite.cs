// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestSuites.Identity.ADFSPIP;
using Microsoft.Protocols.TestTools;
using System.Diagnostics;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    [AttributeUsage(AttributeTargets.All)]
    public sealed class DeployScenario : Attribute
    {
    }


    [TestClass]
    public class BaseTestSuite : TestClassBase
    {
        protected static TestServer server;

        protected static ISUTControlAdapter sutAdapter;

        #region Class Initialization and Cleanup
        public static void BaseInitialize(TestContext context)
        {
            TestClassBase.Initialize(context);
        }

        public static void BaseCleanup()
        {
            TestClassBase.Cleanup();

        }
        #endregion

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            base.TestInitialize();
            sutAdapter = BaseTestSite.GetAdapter<ISUTControlAdapter>();
            EnvironmentConfig.LoadParameters(BaseTestSite);
            server = new TestServer();
            server.Initialize(BaseTestSite);
            bool isDeploy = EnvironmentConfig.TestDeployment;
            StackTrace trace = new System.Diagnostics.StackTrace();
            StackFrame[] frames = trace.GetFrames();            
            ValidationModel.Initialize(BaseTestSite, isDeploy);
            ValidationModel.Reset();
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
            server.Dispose();
        }
        #endregion
    }


}
