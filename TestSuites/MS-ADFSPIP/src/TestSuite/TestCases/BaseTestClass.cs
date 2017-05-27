// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Protocols.TestSuites.Identity.ADFSPIP.RequestHandlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    [TestClass]
    public class BaseTestClass : TestClassBase
    {
        #region Fields
        protected const int ServerTimeoutSeconds = 60;
        protected static ISUTControlAdapter SutAdapter;
        protected static SUTControlAdapterAsync AsyncSutAdapter;
        protected static IMSADFSPIPClientAdapter ClientAdapter;
        protected static ServerDataModel ServerDataModel;
        protected ProxyRequestHandlerFactory Factory;
        protected HttpRequestMessage CurrentRequest;
        protected ProxyRequestHandlerBase CurrentHandler;
        protected HttpResponseMessage CurrentResponse;
        protected string VerifyMessage;
        protected static bool m_sslCertBinded = false;

        protected ITestSite TestSite
        {
            get { return BaseTestSite; }
        }
        #endregion

        #region Class Initialization and Cleanup
        public static void BaseInitialize(TestContext context)
        {
            TestClassBase.Initialize(context);

            // load ptfconfig properties
            EnvironmentConfig.LoadParameters(BaseTestSite);
            EnvironmentConfig.CheckParameters(BaseTestSite);

            // initialize controller adapter
            SutAdapter = BaseTestSite.GetAdapter<ISUTControlAdapter>();
            AsyncSutAdapter = new SUTControlAdapterAsync(SutAdapter);

            // initialize client adapter
            ClientAdapter = BaseTestSite.GetAdapter<IMSADFSPIPClientAdapter>();
            if (!m_sslCertBinded)
            {
                ClientAdapter.BindCertificate(new X509Certificate2(
                    EnvironmentConfig.TLSServerCertificatePath,
                    EnvironmentConfig.TLSServerCertificatePassword));
                m_sslCertBinded = true;
            }

            // initialize server data model and handler factory
            ServerDataModel = ServerDataModel.InitiateServerDataModel();
        }

        public static void BaseCleanup()
        {
            TestClassBase.Cleanup();
            ClientAdapter.Dispose();

        }
        #endregion

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            base.TestInitialize();

            // check parameters before each test case
            EnvironmentConfig.CheckParameters(TestSite);

            // create the request handler factory instance
            Factory = new ProxyRequestHandlerFactory(ServerDataModel);

            // register common request handlers used by all cases
            Factory.RegisterRequestHandler<GetProxyStoreEntryRequestHandler>();
            Factory.RegisterRequestHandler<PutProxyStoreEntryRequestHandler>();
            Factory.RegisterRequestHandler<GetProxyRelyingPartyTrustRequestHandler>();
            Factory.RegisterRequestHandler<GetConfigurationRequestHandler>();
            Factory.RegisterRequestHandler<PostProxyRenewTrustRequestHandler>();
            Factory.RegisterRequestHandler<PostProxyStoreEntryRequestHandler>();
            Factory.RegisterRequestHandler<RelyingPartyTokenHandler>();
            Factory.DefaultRequestHander = new UnknownRequestHandler();

            // these are common variables used by every test cases
            VerifyMessage = string.Empty;
            CurrentRequest = null;
            CurrentResponse = null;
            CurrentHandler = null;
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();

            // deal with all unhandled requests. response 204.
            // we do not actually response these requests
            // but keep the unhandled requests in the queue will
            // sometimes break the HttpListener underneath.
            ClientAdapter.PendingRequests.ToList().ForEach(_ =>
            ClientAdapter.SendResponse(new HttpResponseMessage(204)));
        }
        #endregion

        #region Proteced Methods

        /// <summary>
        /// Wait for a specified request until timeout; all other request will be
        /// handled automatically handled if possible.
        /// </summary>
        protected HttpRequestMessage WaitForReqeust(string requestUri)
        {
            // set default timeout to be 1 minute
            return WaitForReqeust(requestUri, TimeSpan.FromSeconds(ServerTimeoutSeconds));
        }

        /// <summary>
        /// Wait for a specified request until timeout; all other request will be
        /// handled automatically handled if possible.
        /// </summary>
        protected HttpRequestMessage WaitForReqeust(string requestUri, TimeSpan timeout)
        {
            var startTime = DateTime.Now;

            while (DateTime.Now - startTime < timeout)
            {
                // wait for client request
                var request = ClientAdapter.ExpectRequest();
                if (request == null) continue;

                // if the current request matches the given uri, return the request
                if (request.RequestUri.AbsoluteUri.ContainsIgnoreCase(requestUri))
                    return request;

                // or else handle the request automatically
                HandleRequest(request);

                // log that the request has been handled
                TestSite.Log.Add(LogEntryKind.Comment, "Handled request: "
                    + request.RequestUri.AbsoluteUri);
            }

            // return null if time out
            return null;
        }

        /// <summary>
        /// Handle the request automatically with the handlers registered 
        /// in the factory.
        /// </summary>
        protected void HandleRequest(HttpRequestMessage request)
        {
            var handler = (ProxyRequestHandlerBase)Factory.GetRequestHandler(request);
            var response = handler.GetResponse();
            ClientAdapter.SendResponse(response);
        }

        #endregion
    }
}
