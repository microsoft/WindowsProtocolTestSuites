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
    public class S3_RemoveApplication : BaseTestClass
    {
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

            // register request handlers used by this test class
            Factory.RegisterRequestHandler<DeleteProxyPublishSettingsRequestHandler>();

            // make sure the application proxy is installed on the client
            TestSite.Assert.IsTrue(SutAdapter.IsApplicationProxyConfigured(),
            "Application proxy must be configured before removing applications");

            // set an application published before removing it
            ServerDataModel.SetPublishedEndpoint();
        }

        #endregion

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("Management")]
        public void S3_RemoveApplication_Success()
        {
            // trigger the client to remove an application asynchronously
            // so that it does not block the main thread
            var sut = AsyncSutAdapter.TriggerRemoveApplicationAsync();
            TestSite.Log.Add(LogEntryKind.Comment, "Triggering proxy client to remove a web application");

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

            // check whether the client has removed the application successfully
            TestSite.Assert.IsTrue(sut.Result.Return, 
                string.IsNullOrEmpty(sut.Result.Error)
                    ? "The client has removed the application successfully. "
                    : sut.Result.Error);      
        }

        [TestMethod]
        [TestCategory("NonBVT")]
        [TestCategory("Negative")]
        [TestCategory("Management")]
        public void S3_RemoveApplication_NotPublished_Fail()
        {
            // remove the published application
            ServerDataModel.ResetPublishedEndpoint();
            // trigger the client to remove an application asynchronously
            // so that it does not block the main thread
            var sut = AsyncSutAdapter.TriggerRemoveApplicationAsync();
            TestSite.Log.Add(LogEntryKind.Comment, "Triggering proxy client to remove a web application");

            // start receiving client request 
            // when request comes, forward it to the corresponding handler
            while ((CurrentRequest = ClientAdapter.ExpectRequest()) != null) {
                TestSite.Log.Add(LogEntryKind.Comment, "Received request for " + CurrentRequest.RequestUri.AbsolutePath);

                CurrentHandler = (ProxyRequestHandlerBase) Factory.GetRequestHandler(CurrentRequest);
                TestSite.Assert.IsTrue(CurrentHandler.VerifyRequest(out VerifyMessage), VerifyMessage);

                if (CurrentHandler is DeleteProxyPublishSettingsRequestHandler)
                    CurrentResponse = ((DeleteProxyPublishSettingsRequestHandler) CurrentHandler)
                        .GetNotFoundResponse();
                else
                    CurrentResponse = CurrentHandler.GetResponse();

                ClientAdapter.SendResponse(CurrentResponse);
                TestSite.Log.Add(LogEntryKind.Comment, "Response has been sent to the client.");
            }

            TestSite.Assert.IsFalse(sut.Result.Return, "Client acted correctly with 404 response.");
        }
    }
}