// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    /// <summary>
    /// Test class to cover MS-ADFSPIP Section 3.11 
    /// Proxy Runtime Behaviours Client Details
    /// </summary>
    [TestClass]
    public class S4_ProxyRuntime : BaseTestClass
    {
        protected MockClient MockClient;
        protected X509Certificate2 ClientCert;

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
            MockClient = new MockClient(System.Net.IPAddress.Parse(EnvironmentConfig.SUTIP));
            ClientCert = new X509Certificate2(EnvironmentConfig.ClientCert, EnvironmentConfig.PfxPassword);        
            
            // make sure the application proxy is deployed
            TestSite.Assert.IsTrue(SutAdapter.IsApplicationProxyConfigured(),
            "The Application must be deployed before running this test case.");
        }

        #endregion

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("Runtime")]
        [TestCategory("Disabled")]
        public void S4_ProxyRuntime_NoCertificateValidation_Success()
        {
            // choose endpoint /FederationMetadata/2007-06/ to test this message
            // because it has a CertificateValidation is set to None
            var endpointUri = new UriBuilder {
                Scheme = Constraints.DefaultUriSchema,
                Host   = EnvironmentConfig.ADFSFamrDNSName,
                Path   = Constraints.FederationMetadataUrl,
                Port   = ServerDataModel.Configuration.ServiceConfiguration.HttpsPort
            }.Uri;

            TestSite.Log.Add(LogEntryKind.Comment, "Sending service endpoint request to the proxy with no client certificate...");
            var client = MockClient.MakeRequestToEndpointAsync(endpointUri);

            // skip any other request until we get the target request
            var replayedRequest = WaitForReqeust(Constraints.FederationMetadataUrl);

            // we do not actually handle this request here
            // as long as the request is correctly forwarded to the server, it's done
            TestSite.Assert.IsTrue(replayedRequest != null, "Proxy successfully replayed the request to server.");  
        }

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("Runtime")]
        public void S4_ProxyRuntime_SSLCertificateValidation_Success()
        {
            // choose endpoint /adfs/ls/ to test this message because
            // it has a CertificateValidation is set to User
            // and PortType is HttpsPortForUserTlsAuth.
            var endpointUri = new UriBuilder {
                Scheme = Constraints.DefaultUriSchema,
                Host   = EnvironmentConfig.ADFSFamrDNSName,
                Path   = Constraints.FederationAuthUrl,
                Port   = ServerDataModel.Configuration.ServiceConfiguration.HttpsPortForUserTlsAuth
            }.Uri;

            TestSite.Log.Add(LogEntryKind.Comment, "Sending service endpoint request to the proxy with client certificate...");
            var client = MockClient.MakeRequestToEndpointAsync(endpointUri, ClientCert);

            // skip any other request until we get the target request
            var replayedRequest = WaitForReqeust(Constraints.BackEndProxyTLSUrl);
            TestSite.Assert.IsTrue(replayedRequest != null, "Proxy replayed the request to server in BackendProxyTls request.");

            // validate the replayed request
            var requestHandler = new PostBackendProxyTlsRequestHandler {Request = replayedRequest};
            // just drop anything needed to be checked into the dynamic object
            requestHandler.DynamicObject.EndpointUri = endpointUri.ToString();
            requestHandler.DynamicObject.ClientCertificate = ClientCert.ToBase64String();

            Assert.IsTrue(requestHandler.VerifyRequest(out VerifyMessage), VerifyMessage);           
        }

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("Runtime")]
        [TestCategory("Disabled")]
        public void S4_ProxyRuntime_DeviceCertificateValidation_Success()
        {
            // TODO: implement this test case in the future
            // this test case is skipped bacause I do know how to
            // programmingly make a device certificate.
        }
    }
}
