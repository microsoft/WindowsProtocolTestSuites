// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    /// <summary>
    /// Test class to cover MS-ADFSPIP Section 3.3
    /// Proxy Registration Client Details 
    /// </summary>
    [TestClass]
    public class S1_EstablishRenewTrust : BaseTestClass
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

            Factory.RegisterRequestHandler<PostProxyEstablishTrustRequestHandler>();
            Factory.RegisterRequestHandler<PostProxyTrustRequestHandler>();
            Factory.RegisterRequestHandler<GetProxyTrustRequestHandler>();
            Factory.RegisterRequestHandler<GetFederationMetadataRequestHandler>();

            ServerDataModel.SetPublishedEndpoint();
        }

        #endregion

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("Deployment")]
        [TestCategory("Management")]
        public void S1_EstablishRenewTrust_Success()
        {
            // trigger the client to install application proxy asynchronously
            // so that it does not block the main thread
            int multi = 1;
            System.Threading.Tasks.Task<SUTControlAdapterAsync.SutResult> sut = null;
            while (multi < 5)
            {
                sut = null;
                sut = AsyncSutAdapter.TriggerInstallApplicationProxyAsync();
                TestSite.Log.Add(LogEntryKind.Comment, "Triggering proxy client to install application proxy.");
                TestSite.Log.Add(LogEntryKind.Comment, "Sleep " + 10 * multi + " secs for Windows side powershell remoting");
                System.Threading.Thread.Sleep(10000 * multi);

                // start receiving client request 
                // when request comes, forward it to the corresponding handler
                while ((CurrentRequest = ClientAdapter.ExpectRequest()) != null)
                {
                    TestSite.Log.Add(LogEntryKind.Comment, "Received request: " + CurrentRequest.RequestUri.AbsolutePath);

                    // get the proper handler to handle the current reqeust
                    CurrentHandler = (ProxyRequestHandlerBase)Factory.GetRequestHandler(CurrentRequest);
                    TestSite.Assert.IsTrue(CurrentHandler.VerifyRequest(out VerifyMessage), VerifyMessage);

                    // get the response 
                    CurrentResponse = CurrentHandler.GetResponse();
                    ClientAdapter.SendResponse(CurrentResponse);
                    TestSite.Log.Add(LogEntryKind.Comment, "Response has been sent to the client.");
                }
                if (sut.Result.Return)
                    break;
                multi++;

            }// verify the operation result from the client side
            // this blocks the current thread until it gets the result
            TestSite.Assert.IsTrue(sut.Result.Return,
                string.IsNullOrEmpty(sut.Result.Error)
                    ? "The client has renewed trust successfully. "
                    : sut.Result.Error);
        }
    }
}
