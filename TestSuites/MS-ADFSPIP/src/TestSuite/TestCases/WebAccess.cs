// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    /// <summary>
    /// Test class to cover MS-ADFSPIP Section 3.13
    /// Application Proxy Runtime Behaviors Client Details
    /// </summary>
    [TestClass]
    public class S5_WebAccess : BaseTestClass
    {
        protected MockClient MockClient;

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

            // initialize mock client to visit the proxy
            MockClient = new MockClient(IPAddress.Parse(EnvironmentConfig.SUTIP));
            
            // make sure there is an application published
            ServerDataModel.SetPublishedEndpoint();
            
            // make sure the application proxy is deployed
            TestSite.Assert.IsTrue(SutAdapter.IsApplicationProxyConfigured(),
            "The Application must be deployed before running this test case.");
        }

        #endregion

        // NOTE:
        // Test cases in this class works only if the proxy has added the web application URL
        // to its listening list. 
        // However the proxy won't automatically listen to the application URL even if the 
        // application is successfully published. The proxy waits to update the listening URLs
        // until it gets the EndpointConfig in the next config polling time, which can be 
        // configured through EndpointConfig.ConfigurationChangesPollingIntervalSec.
        // So to make these cases work, you need update the EndpointConfig in data store 
        // either through establishing trust, or in the next config polling time.
        //
        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("Runtime")]
        [TestCategory("Disabled")]
        public void S5_WebAccess_InitializePreauth_Success()
        {
            // make a request to the proxy without authToken
            var client = MockClient.MakeWebAccessRequestAsync(new Uri(EnvironmentConfig.App1Url));                   
            TestSite.Log.Add(LogEntryKind.Comment, "Sending unauthenticated web access request to the proxy...");

            // auto-handle all other requests until we get the authentication request
            // Note: to get the auto-direct work, make sure the adfs DNS name is pointing to
            // the driver computer, and make sure other network adapters on driver are turned off.
            var rediredtedRequest = WaitForReqeust(Constraints.FederationAuthUrl);

            TestSite.Assert.IsTrue(rediredtedRequest != null, "Successfully redirected.");
                    
            // verify the request especially its authentication query string
            var requestHandler = new FederationAuthRequestHandler {
                ServerDataModel = ServerDataModel,
                Request         = rediredtedRequest
            };
            requestHandler.DynamicObject.InitialUrl = EnvironmentConfig.App1Url;

            TestSite.Assert.IsTrue(requestHandler.VerifyRequest(out VerifyMessage), VerifyMessage);                  
        }

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("Runtime")]
        public void S5_WebAccess_AdfsPreauth_QueryStringBased_Success()
        {
            // make a request to the proxy with authToken
            var client = MockClient.MakeWebAccessRequestAsync(new Uri(EnvironmentConfig.App1Url),
                        ServerDataModel.GetAuthToken());

            TestSite.Log.Add(LogEntryKind.Comment, "Sending preauthenticated web access request to the proxy...");

            // auto-hanlde all other requests to the server until we get
            // the request to the web application.
            var replayedRequest = WaitForReqeust(EnvironmentConfig.App1Url);

            // once we get the redirected request, it means that the proxy has
            // validated the authToken and redirect the request to the internal 
            // url correctly.
            TestSite.Assert.IsTrue(replayedRequest != null, "Preauthenticated request successfully redirected.");

            // After successful preauthentication the proxy MUST remove the authToken 
            // parameter with its value before replaying the request to the internal URL.
            TestSite.Assert.IsTrue(!replayedRequest.RequestUri.Query.ContainsIgnoreCase("authToken"),
                "No authToken parameter contained in the replayed request.");
        }

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("Runtime")]
        [TestCategory("Disabled")]
        public void S5_WebAccess_AdfsPreauth_HttpHeaderBased_Success()
        {
            // TODO: implement this test case in the future
            // this test case is skipped since I do not know how to
            // trigger HTTP header based preauthentication message,
            // neither do I know how to programmingly make an access
            // token.
        }
    }
}
