// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestSuites.Identity.ADFSPIP.RequestHandlers;
using System.Threading;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    /// <summary>
    /// Test class to cover MS-ADFSPIP Section 3.13
    /// Application Proxy Runtime Behaviors Client Details
    /// </summary>
    [TestClass]
    public class S6_ActiveClientAuth : BaseTestClass
    {
        protected MockClient MockClient;
        private bool IsServiceStart = true;

        #region Class Initialization and Cleanup

        [ClassInitialize]
        public static void ClassInitialize(TestContext context) { BaseInitialize(context); }

        [ClassCleanup]
        public static void ClassCleanup() { BaseCleanup(); }

        #endregion

        #region Test Initialization and Cleanup

        protected override void TestInitialize()
        {
            base.TestInitialize();

            Factory.RegisterRequestHandler<PostProxyPublishSettingsRequestHandler>();
            Factory.RegisterRequestHandler<GetFederationMetadataRequestHandler>();

            // initialize mock client to visit the proxy
            MockClient = new MockClient(IPAddress.Parse(EnvironmentConfig.SUTIP));

            // make sure the application proxy is deployed
            TestSite.Assert.IsTrue(SutAdapter.IsApplicationProxyConfigured(),
            "The Application must be deployed before running this test case.");

            // Start ADFS service
            Thread workerThread = new Thread(this.ADFSService);
            workerThread.Start();
        }

        protected override void TestCleanup()
        {
            this.IsServiceStart = false;
            Thread.Sleep(1000); //wait ADFS exit;
            base.TestCleanup();
        }

        #endregion

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("Runtime")]
        [TestCategory("Win2016")]
        public void S6_ActiveClientAuth_AccessDenied()
        {
            ServerDataModel.ResetPublishedEndpoint();
            TestSite.Log.Add(LogEntryKind.Comment, "Reset Published Endpoint");
            System.Threading.Thread.Sleep(1000 * 30);
            // make a request to the proxy with authToken
            var sut = AsyncSutAdapter.TriggerPublishNonClaimsAppAsync();
            TestSite.Log.Add(LogEntryKind.Comment, "Triggering proxy client to publish a web application");

            System.Threading.Thread.Sleep(1000 * 60);

            TestSite.Log.Add(LogEntryKind.Comment, "Client start to access webapp");

            System.Threading.Thread.Sleep(1000 * 60);

            // make a request to the proxy with authToken
            var client = MockClient.MakeWebRequestWithBasicAuth(new Uri(EnvironmentConfig.App2Url),
                        ServerDataModel.GetIncorrectBasicAuthorizationHeader());

            TestSite.Assert.IsTrue(client.StatusCode == HttpStatusCode.Unauthorized, "Preauthenticated request failed redirected as access denied.");
        }

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("Runtime")]
        [TestCategory("Win2016")]
        public void S6_ActiveClientAuth_AccessSuccess()
        {
            ServerDataModel.ResetPublishedEndpoint();
            TestSite.Log.Add(LogEntryKind.Comment, "Reset Published Endpoint");
            System.Threading.Thread.Sleep(1000 * 30);
            // make a request to the proxy with authToken
            var sut = AsyncSutAdapter.TriggerPublishNonClaimsAppAsync();
            TestSite.Log.Add(LogEntryKind.Comment, "Triggering proxy client to publish a web application");

            System.Threading.Thread.Sleep(1000 * 60);

            TestSite.Log.Add(LogEntryKind.Comment, "Client start to access webapp");

            System.Threading.Thread.Sleep(1000 * 60);

            // make a request to the proxy with authToken
            var client = MockClient.MakeWebRequestWithBasicAuth(new Uri(EnvironmentConfig.App2Url),
                        ServerDataModel.GetBasicAuthorizationHeader());

            TestSite.Assert.IsTrue(client.StatusCode == HttpStatusCode.ServiceUnavailable, "Preauthenticated request successfully redirected.");
        }

        private void ADFSService()
        {
            while (IsServiceStart)
            {
                try
                {

                    CurrentRequest = ClientAdapter.ExpectRequest();

                    if (CurrentRequest != null)
                    {
                        TestSite.Log.Add(LogEntryKind.Comment, "Received request for " + CurrentRequest.RequestUri.AbsolutePath);

                        CurrentHandler = (ProxyRequestHandlerBase)Factory.GetRequestHandler(CurrentRequest);
                        TestSite.Assert.IsTrue(CurrentHandler.VerifyRequest(out VerifyMessage), VerifyMessage);

                        CurrentResponse = CurrentHandler.GetResponse();
                        ClientAdapter.SendResponse(CurrentResponse);
                        TestSite.Log.Add(LogEntryKind.Comment, "Response has been sent to the client.");
                    }
                }
                catch { }
                System.Threading.Thread.Sleep(100 * 5);
            }
        }
    }
}
