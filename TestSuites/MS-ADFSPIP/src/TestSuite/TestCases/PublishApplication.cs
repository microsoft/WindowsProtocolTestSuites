// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{   
    /// <summary>
    /// Test class to cover MS-ADFSPIP Section 3.9
    /// Application Publishing Client Details
    /// </summary>
    [TestClass]
    public class S2_PublishApplication : BaseTestClass
    {
        #region Class Initialization and Cleanup

        [ClassInitialize]
        public static void ClassInitialize(TestContext context) { BaseInitialize(context); }

        [ClassCleanup]
        public static void ClassCleanup() {BaseCleanup(); }

        #endregion

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            base.TestInitialize();

            // register request handlers used by this test class
            Factory.RegisterRequestHandler<PostProxyPublishSettingsRequestHandler>();

            // make sure the application proxy is installed on the client
            TestSite.Assert.IsTrue(SutAdapter.IsApplicationProxyConfigured(),
            "Application proxy must be configured before publishing applications");

            // remove all published applications
            ServerDataModel.ResetPublishedEndpoint();
        }

        #endregion

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("Management")]
        public void S2_PublishApplication_Success()
        {
            // trigger the client to publish an application asynchronously
            // so that it does not block the main thread
           var sut = AsyncSutAdapter.TriggerPublishApplicationAsync();
            TestSite.Log.Add(LogEntryKind.Comment, "Triggering proxy client to publish a web application");
           
            // start receiving client request 
            // when request comes, forward it to the corresponding handler
            while ((CurrentRequest = ClientAdapter.ExpectRequest()) != null) {
                TestSite.Log.Add(LogEntryKind.Comment, "Received request for " + CurrentRequest.RequestUri.AbsolutePath);

                CurrentHandler = (ProxyRequestHandlerBase) Factory.GetRequestHandler(CurrentRequest);
                TestSite.Assert.IsTrue(CurrentHandler.VerifyRequest(out VerifyMessage), VerifyMessage);

                CurrentResponse = CurrentHandler.GetResponse();
                ClientAdapter.SendResponse(CurrentResponse);
                TestSite.Log.Add(LogEntryKind.Comment, "Response has been sent to the client.");
            }
            
            // check whether the client has published the application successfully
            TestSite.Assert.IsTrue(sut.Result.Return,
                string.IsNullOrEmpty(sut.Result.Error)
                ? "The client has published the application successfully. "
                : sut.Result.Error);
        }

        [TestMethod]
        [TestCategory("NonBVT")]
        [TestCategory("Negative")]
        [TestCategory("Management")]
        public void S2_PublishApplication_AlreadyPublished_Fail()
        {
            // trigger the client to publish an application asynchronously
            // so that it does not block the main thread
            var sut = AsyncSutAdapter.TriggerPublishApplicationAsync();
            TestSite.Log.Add(LogEntryKind.Comment, "Triggering proxy client to publish a web application");
           
            // start receiving client request 
            // when request comes, forward it to the corresponding handler
            while ((CurrentRequest = ClientAdapter.ExpectRequest()) != null) {
                TestSite.Log.Add(LogEntryKind.Comment, "Received request for " + CurrentRequest.RequestUri.AbsolutePath);

                CurrentHandler = (ProxyRequestHandlerBase) Factory.GetRequestHandler(CurrentRequest);
                TestSite.Assert.IsTrue(CurrentHandler.VerifyRequest(out VerifyMessage), VerifyMessage);

                if (CurrentHandler is PostProxyPublishSettingsRequestHandler)
                    CurrentResponse = ((PostProxyPublishSettingsRequestHandler) CurrentHandler)
                        .GetConflictResponse();
                else
                    CurrentResponse = CurrentHandler.GetResponse();

                ClientAdapter.SendResponse(CurrentResponse);
                TestSite.Log.Add(LogEntryKind.Comment, "Response has been sent to the client.");
            }

            TestSite.Assert.IsFalse(sut.Result.Return, "The client acted correctly with 409 response.");
        }
    }
}
