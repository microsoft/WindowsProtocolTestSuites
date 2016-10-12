// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.Protocols.TestSuites.BranchCache.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc;
using Microsoft.Protocols.TestTools.StackSdk.CommonStack;
using Microsoft.Protocols.TestTools.StackSdk.CommonStack.Enum;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.BranchCache.TestSuite
{
    public class BranchCacheTestClassBase : TestClassBase
    {
        protected TestConfig testConfig;

        protected ISUTControlAdapter sutControlAdapter;

        protected ContentInformationUtility contentInformationUtility;
        
        protected override void TestInitialize()
        {
            base.TestInitialize();

            testConfig = new TestConfig(BaseTestSite);

            BaseTestSite.DefaultProtocolDocShortName = "BranchCache";

            sutControlAdapter = BaseTestSite.GetAdapter<ISUTControlAdapter>();

            contentInformationUtility = new ContentInformationUtility(BaseTestSite, testConfig, sutControlAdapter);
        }

        protected void ResetHostedCacheServer()
        {
            sutControlAdapter.RestartBranchCacheService(testConfig.HostedCacheServerComputerFQDNOrNetBiosName);

            TestUtility.DoUntilSucceed(SetupHttpsConnection, testConfig.Timeout, TimeSpan.Zero);

            sutControlAdapter.ClearHTTPHash(testConfig.ContentServerComputerFQDNOrNetBiosName, testConfig.WebsiteLocalPath);
            sutControlAdapter.ClearSMB2Hash(testConfig.ContentServerComputerFQDNOrNetBiosName, testConfig.FileShareLocalPath);
        }

        protected void ResetContentServer()
        {
            sutControlAdapter.ClearHTTPHash(testConfig.ContentServerComputerFQDNOrNetBiosName, testConfig.WebsiteLocalPath);
            sutControlAdapter.ClearSMB2Hash(testConfig.ContentServerComputerFQDNOrNetBiosName, testConfig.FileShareLocalPath);
        }

        protected void CheckApplicability()
        {
            var testcase = (string)BaseTestSite.TestProperties["CurrentTestCaseName"];

            int lastPointIndex = testcase.LastIndexOf('.');
            string typeName = testcase.Substring(0, lastPointIndex);
            string methodName = testcase.Substring(lastPointIndex + 1);

            var type = Assembly.GetExecutingAssembly().GetType(typeName);
            var method = type.GetMethod(methodName);
            var attributes = method.GetCustomAttributes(typeof(TestCategoryAttribute), false);

            // Ignore the check if test category attribute not found
            if (attributes == null || attributes.Length == 0)
                return;

            var testCategories = (TestCategoryAttribute[])attributes;

            bool applicableV1 = false;
            bool applicableV2 = false;
            bool applicablePccrtp = false;
            bool applicableSmb2 = false;

            foreach (var testCategory in testCategories)
            {
                if (testCategory.TestCategories.Contains("BranchCacheV1"))
                    applicableV1 = true;
                if (testCategory.TestCategories.Contains("BranchCacheV2"))
                    applicableV2 = true;

                if (testCategory.TestCategories.Contains("PCCRTP"))
                    applicablePccrtp = true;
                if (testCategory.TestCategories.Contains("SMB2"))
                    applicableSmb2 = true;
            }

            if (applicableV1 ^ applicableV2)  // Do not need to check if applicable to all versions or version applicability info is not present
            {
                if (applicableV1 && !testConfig.SupportBranchCacheV1 ||
                    applicableV2 && !testConfig.SupportBranchCacheV2)
                {
                    BaseTestSite.Assert.Inconclusive("This test case is not applicable to current supported branchcache version");
                }
            }

            if (applicablePccrtp ^ applicableSmb2)  // Do not need to check if applicable to all transports or transport applicability info is not present
            {
                if (applicablePccrtp && testConfig.ContentTransport != ContentInformationTransport.PCCRTP ||
                    applicableSmb2 && testConfig.ContentTransport != ContentInformationTransport.SMB2)
                {
                    BaseTestSite.Assert.Inconclusive("This test case is not applicable to current content transport");
                }
            }
        }

        protected bool SetupHttpsConnection()
        {
            sutControlAdapter.ClearCache(testConfig.HostedCacheServerComputerFQDNOrNetBiosName);

            int timeout = 20000;
            byte[] content = TestUtility.GenerateRandomArray(10);

            HttpClientTransport testClient = new HttpClientTransport(
                TransferProtocol.HTTPS,
                testConfig.HostedCacheServerComputerName,
                testConfig.HostedCacheServerHTTPSListenPort,
                PchcConsts.HttpsUrl,
                testConfig.DomainName,
                testConfig.UserName,
                testConfig.UserPassword);
            try
            {
                testClient.Send(HttpVersion.Version10, null, content, HttpMethod.POST, timeout);
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                testClient.Dispose();
            }

            return true;
        }
    }
}
