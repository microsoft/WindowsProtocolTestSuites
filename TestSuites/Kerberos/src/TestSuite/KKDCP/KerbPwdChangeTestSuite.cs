// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocol.TestSuites.Kerberos.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocol.TestSuites.Kerberos.TestSuite
{
    /// <summary>
    /// This is for Kpassword protocol
    /// </summary>
    [TestClass]
    public class KerbPwdChangeTestSuite : TraditionTestBase
    {
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.BVT)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify Kpassword when the password change succeeds.")]
        public void ChangePasswordSuccess()
        {
            base.Logging();

            if (!this.testConfig.UseProxy)
            {
                BaseTestSite.Assert.Inconclusive("This case is only applicable when Kerberos Proxy Service is in use.");
            }

            //Create kerberos test client and connect
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[22].Username,
                this.testConfig.LocalRealm.User[22].Password,
                KerberosAccountType.User,
                testConfig.LocalRealm.KDC[0].IPAddress,
                testConfig.LocalRealm.KDC[0].Port,
                testConfig.TransportType,
                testConfig.SupportedOid);

            // Kerberos Proxy Service is used
            if (this.testConfig.UseProxy)
            {
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Initialize KKDCP Client .");
                KKDCPClient proxyClient = new KKDCPClient(proxyClientConfig);
                proxyClient.TargetDomain = this.testConfig.LocalRealm.RealmName;
                client.UseProxy = true;
                client.ProxyClient = proxyClient;
            }

            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends AS_REQ without Pre-Authentication data");
            client.SendAsRequestForPwdChange(options, null);
            //Recieve preauthentication required error
            METHOD_DATA methodData;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "KDC returns KRB_ERROR: KDC_ERR_PREAUTH_REQUIRED");
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);

            //Create sequence of PA data
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends AS_REQ with PA-ENC-TIMESTAMP and PA-PAC-REQUEST");
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp,
                0,
                this.client.Context.SelectedEType,
                this.client.Context.CName.Password,
                this.client.Context.CName.Salt);

            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });
            //Create and send AS request
            client.SendAsRequestForPwdChange(options, seqOfPaData);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "KDC returns AS_REP");
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create kpassword test client and connect            
            KpasswdTestClient kpassClient = new KpasswdTestClient(
                testConfig.LocalRealm.KDC[0].IPAddress,
                KerberosConstValue.KPASSWORD_PORT,
                testConfig.TransportType,
                client.Context.Ticket);

            // Kerberos Proxy Service is used
            if (this.testConfig.UseProxy)
            {
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Initialize KKDCP Client .");
                KKDCPClient proxyClient = new KKDCPClient(proxyClientConfig);
                proxyClient.TargetDomain = this.testConfig.LocalRealm.RealmName;
                kpassClient.UseProxy = true;
                kpassClient.ProxyClient = proxyClient;
            }

            //Create and send Kpassword request
            string newPwd = this.testConfig.LocalRealm.User[22].Password;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends KpasswordRequest");
            kpassClient.SendKpasswordRequest(newPwd);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "KDC returns KpasswordResponse");
            KpasswordResponse kpassResponse = kpassClient.ExpectKpasswordResponse();

            //Verify the result code 
            BaseTestSite.Assert.AreEqual(KpasswdError.KRB5_KPASSWD_SUCCESS, (KpasswdError)kpassClient.GetResultCode(kpassResponse), "The result code should be KRB5_KPASSWD_SUCCESS.");
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.BVT)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify Kpassword when the password change fails.")]
        public void ChangePasswordError()
        {
            base.Logging();

            if (!this.testConfig.UseProxy)
            {
                BaseTestSite.Assert.Inconclusive("This case is only applicable when Kerberos Proxy Service is in use.");
            }

            #region KRB5_KPASSWD_SOFTERROR
            //Create kerberos test client and connect
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[22].Username,
                this.testConfig.LocalRealm.User[22].Password,
                KerberosAccountType.User,
                testConfig.LocalRealm.KDC[0].IPAddress,
                testConfig.LocalRealm.KDC[0].Port,
                testConfig.TransportType,
                testConfig.SupportedOid);

            // Kerberos Proxy Service is used
            if (this.testConfig.UseProxy)
            {
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Initialize KKDCP Client .");
                KKDCPClient proxyClient = new KKDCPClient(proxyClientConfig);
                proxyClient.TargetDomain = this.testConfig.LocalRealm.RealmName;
                client.UseProxy = true;
                client.ProxyClient = proxyClient;
            }

            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends AS_REQ without Pre-Authentication data for password change");
            client.SendAsRequestForPwdChange(options, null);
            //Recieve preauthentication required error
            METHOD_DATA methodData;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "KDC returns KRB_ERROR: KDC_ERR_PREAUTH_REQUIRED");
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);

            //Create sequence of PA data
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends AS_REQ with PA-ENC-TIMESTAMP and PA-PAC-REQUEST for password change");
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp,
                0,
                this.client.Context.SelectedEType,
                this.client.Context.CName.Password,
                this.client.Context.CName.Salt);

            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });
            //Create and send AS request
            client.SendAsRequestForPwdChange(options, seqOfPaData);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "KDC returns AS_REP");
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create kpassword test client and connect
            KpasswdTestClient kpassClient = new KpasswdTestClient(
                testConfig.LocalRealm.KDC[0].IPAddress,
                KerberosConstValue.KPASSWORD_PORT,
                testConfig.TransportType,
                client.Context.Ticket);

            // Kerberos Proxy Service is used
            if (this.testConfig.UseProxy)
            {
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Initialize KKDCP Client .");
                KKDCPClient proxyClient = new KKDCPClient(proxyClientConfig);
                proxyClient.TargetDomain = this.testConfig.LocalRealm.RealmName;
                kpassClient.UseProxy = true;
                kpassClient.ProxyClient = proxyClient;
            }

            //Specify a new password which doesn't meet the complexity requirements
            string newPwd = "123";
            //Create and send Kpassword request
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends KpasswordRequest");
            kpassClient.SendKpasswordRequest(newPwd);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "KDC returns KpasswordResponse");
            KpasswordResponse kpassResponse = kpassClient.ExpectKpasswordResponse();

            //Verify the result code
            BaseTestSite.Assert.AreEqual(KpasswdError.KRB5_KPASSWD_SOFTERROR, (KpasswdError)kpassClient.GetResultCode(kpassResponse),
                "The result code should be KRB5_KPASSWD_SOFTERROR when the new password doesn't meet the complexity requirements.");
            #endregion KRB5_KPASSWD_SOFTERROR

            #region KRB5_KPASSWD_MALFORMED
            newPwd = this.testConfig.LocalRealm.User[22].Password;
            //Create and send Kpassword request
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends KpasswordRequest");
            kpassClient.SendMalformedKpasswordRequest(newPwd);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "KDC returns KpasswordResponse");
            kpassResponse = kpassClient.ExpectKpasswordResponse();

            //Verify the result code
            BaseTestSite.Assert.AreEqual(KpasswdError.KRB5_KPASSWD_MALFORMED, (KpasswdError)kpassClient.GetResultCode(kpassResponse),
                "The result code should be KRB5_KPASSWD_MALFORMED when the request is malformed.");
            #endregion KRB5_KPASSWD_MALFORMED

            #region KRB5_KPASSWD_AUTHERROR
            //Create and send Kpassword request
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends KpasswordRequest");
            kpassClient.SendAuthErrorKpasswordRequest(newPwd);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "KDC returns KpasswordResponse");
            kpassResponse = kpassClient.ExpectKpasswordResponse();

            //Verify the result code
            BaseTestSite.Assert.AreEqual(KpasswdError.KRB5_KPASSWD_AUTHERROR, (KpasswdError)kpassClient.GetResultCode(kpassResponse),
                "The result code should be KRB5_KPASSWD_AUTHERROR when the request fails due to an error in authentication processing.");
            #endregion KRB5_KPASSWD_AUTHERROR

            #region KRB5_KPASSWD_ACCESSDENIED
            //Create kerberos test client and connect
            //Use the user who can't change the password
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[1].Username,
                this.testConfig.LocalRealm.User[1].Password,
                KerberosAccountType.User,
                testConfig.LocalRealm.KDC[0].IPAddress,
                testConfig.LocalRealm.KDC[0].Port,
                testConfig.TransportType,
                testConfig.SupportedOid);

            // Kerberos Proxy Service is used
            if (this.testConfig.UseProxy)
            {
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Initialize KKDCP Client .");
                KKDCPClient proxyClient = new KKDCPClient(proxyClientConfig);
                proxyClient.TargetDomain = this.testConfig.LocalRealm.RealmName;
                client.UseProxy = true;
                client.ProxyClient = proxyClient;
            }

            options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends AS_REQ without Pre-Authentication data for password change");
            client.SendAsRequestForPwdChange(options, null);
            //Recieve preauthentication required error
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "KDC returns KRB_ERROR: KDC_ERR_PREAUTH_REQUIRED");
            krbError = client.ExpectPreauthRequiredError(out methodData);

            //Create sequence of PA data
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends AS_REQ with PA-ENC-TIMESTAMP and PA-PAC-REQUEST for password change");
            timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            paEncTimeStamp = new PaEncTimeStamp(timeStamp,
                0,
                this.client.Context.SelectedEType,
                this.client.Context.CName.Password,
                this.client.Context.CName.Salt);

            paPacRequest = new PaPacRequest(true);
            seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });
            //Create and send AS request
            client.SendAsRequestForPwdChange(options, seqOfPaData);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "KDC returns AS_REP");
            asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create kpassword test client and connect
            kpassClient = new KpasswdTestClient(
                testConfig.LocalRealm.KDC[0].IPAddress,
                KerberosConstValue.KPASSWORD_PORT,
                testConfig.TransportType,
                client.Context.Ticket);

            // Kerberos Proxy Service is used
            if (this.testConfig.UseProxy)
            {
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Initialize KKDCP Client .");
                KKDCPClient proxyClient = new KKDCPClient(proxyClientConfig);
                proxyClient.TargetDomain = this.testConfig.LocalRealm.RealmName;
                kpassClient.UseProxy = true;
                kpassClient.ProxyClient = proxyClient;
            }

            newPwd = this.testConfig.LocalRealm.User[1].Password;
            //Create and send Kpassword request
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends KpasswordRequest");
            kpassClient.SendKpasswordRequest(newPwd);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "KDC returns KpasswordResponse");
            kpassResponse = kpassClient.ExpectKpasswordResponse();

            //Verify the result code
            BaseTestSite.Assert.AreEqual(KpasswdError.KRB5_KPASSWD_ACCESSDENIED, (KpasswdError)kpassClient.GetResultCode(kpassResponse),
                "The result code should be KRB5_KPASSWD_ACCESSDENIED when the user has no access to change the password.");
            #endregion KRB5_KPASSWD_ACCESSDENIED
        }
    }
}