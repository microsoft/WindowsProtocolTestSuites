// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocol.TestSuites.Kerberos.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocol.TestSuites.Kerberos.TestSuite.KILE
{
    [TestClass]
    public class KileKrbErrorTest : TraditionTestBase
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
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KerberosError)]
        [Description("This test case is designed to test KDC when it cannot find the request principal in its database.")]
        public void KrbErrorClientPrincipalUnknown()
        {
            base.Logging();

            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName,
                "UnknownUser",
                 this.testConfig.LocalRealm.User[0].Password,
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

            KdcOptions options = KdcOptions.RENEWABLE;
            Asn1SequenceOf<PA_DATA> seqOfPaData = null;
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with unknown principal.");
            KerberosKrbError krbError = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_C_PRINCIPAL_UNKNOWN,
                krbError.ErrorCode,
                "If the requested client principal named in the request is unknown because it doesn't exist in the KDC's principal database, then an error message with a KDC_ERR_C_PRINCIPAL_UNKNOWN is returned.");
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KerberosError)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to test KDC when encryption type requested by the client is not supported by the KDC.")]
        public void KrbErrorETypeNoSupp()
        {
            base.Logging();

            //Create kerberos test client
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

            EncryptionType[] encryptionTypes = new EncryptionType[] { EncryptionType.None };

            Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32[] etypes =
                new Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32[encryptionTypes.Length];
            for (int i = 0; i < encryptionTypes.Length; i++)
            {
                etypes[i] = new Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32((int)encryptionTypes[i]);
            }

            client.Context.SupportedEType = new Asn1SequenceOf<KerbInt32>(etypes);

            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);

            KerberosKrbError krbError = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_ETYPE_NOTSUPP,
                krbError.ErrorCode,
                "If the server cannot accommodate any encryption type requested by the client, an error message with the code KDC_ERR_ETYPE_NOTSUPP is returned.");
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KerberosError)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to test KDC when pre-authentication is required and failed.")]
        public void KrbErrorPreauthFailed()
        {
            base.Logging();

            //Create kerberos test client and connect
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[1].Username,
                "WrongPassword",
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
            client.SendAsRequest(options, null);
            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp,
                0,
                this.client.Context.SelectedEType,
                this.client.Context.CName.Password,
                this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            krbError = client.ExpectKrbError();
            BaseTestSite.Assert.IsTrue(krbError.ErrorCode == KRB_ERROR_CODE.KDC_ERR_PREAUTH_FAILED || krbError.ErrorCode == KRB_ERROR_CODE.KRB_AP_ERR_BAD_INTEGRITY,
                "If the pre-authentication check fails, an error message with the code KDC_ERR_PREAUTH_FAILED is returned.");
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KerberosError)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.Smb2)]
        [Description("This test case is designed to test if AP supports replay cache.")]
        public void KrbErrorRepeat()
        {
            base.Logging();

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

            //Create and send AS request
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);
            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp,
                0,
                client.Context.SelectedEType,
                this.client.Context.CName.Password,
                this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();

            //Create and send TGS request
            client.SendTgsRequest(this.testConfig.LocalRealm.FileServer[0].Smb2ServiceName, options);
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();

            AuthorizationData data = null;
            EncryptionKey subkey = KerberosUtility.GenerateKey(client.Context.SessionKey);
            byte[] token = client.CreateGssApiToken(ApOptions.MutualRequired,
                data,
                subkey,
                ChecksumFlags.GSS_C_MUTUAL_FLAG | ChecksumFlags.GSS_C_INTEG_FLAG);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send Smb2 request.");
            KerberosApResponse apRep = client.GetApResponseFromToken(SendAndRecieveSmb2Ap(this.testConfig.LocalRealm.FileServer[0], token));
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve Smb2 response.");

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send Smb2 request.");
            KerberosKrbError error = client.GetKrbErrorFromToken(SendAndRecieveSmb2Ap(this.testConfig.LocalRealm.FileServer[0], token));
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve Kerberos error.");
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KRB_AP_ERR_REPEAT,
                error.ErrorCode,
                "If a matching tuple is found, the KRB_AP_ERR_REPEAT error is returned.");
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KerberosError)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to test KDC when it cannot find the requested principal in its database.")]
        public void KrbErrorServicePrincipalUnknown()
        {
            base.Logging();

            client = new KerberosTestClient(
                this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[1].Username,
                this.testConfig.LocalRealm.User[1].Password,
                KerberosAccountType.User,
                testConfig.LocalRealm.KDC[0].IPAddress,
                testConfig.LocalRealm.KDC[0].Port,
                testConfig.TransportType,
                testConfig.SupportedOid);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Construct Kerberos client for testing.");

            // Kerberos Proxy Service is used
            if (this.testConfig.UseProxy)
            {
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Initialize KKDCP Client .");
                KKDCPClient proxyClient = new KKDCPClient(proxyClientConfig);
                proxyClient.TargetDomain = this.testConfig.LocalRealm.RealmName;
                client.UseProxy = true;
                client.ProxyClient = proxyClient;
            }

            //Create and send AS request
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with no PA data.");
            //Recieve preauthentication required error            
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve preauthentication required error.");

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create a sequence of PA data.");
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with PA data.");
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve AS response.");

            //Create and send TGS request
            client.SendTgsRequest("UnknownServerPrincipalName", options);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send TGS request");
            krbError = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_S_PRINCIPAL_UNKNOWN, krbError.ErrorCode,
                "If the requested service principal named in the request is unknown because it doesn't exist in the KDC's principal database, then an error message with a KDC_ERR_S_PRINCIPAL_UNKNOWN is returned.");
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KerberosError)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to test KDC when protocol version number mismatches.")]
        public void KrbErrorBadProtocolVersionNum()
        {
            base.Logging();

            client = new KerberosTestClient(
                this.testConfig.LocalRealm.RealmName,
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

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Construct Kerberos client for testing.");

            //Create and send AS request
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            long badPvno = 255;
            client.SendAsRequest(options, null, badPvno);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with no PA data.");
            KerberosKrbError krbError = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KRB_AP_ERR_BADVERSION, krbError.ErrorCode,
                "If the requested protocol version number mismatches, then an error message with a KRD_AP_ERR_BADVERSION is returned.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KerberosError)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to test KDC when the requested start time is later than the end time.")]
        public void KrbErrorTicketNeverValid()
        {
            base.Logging();

            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName, this.testConfig.LocalRealm.User[1].Username,
                this.testConfig.LocalRealm.User[1].Password, KerberosAccountType.User, testConfig.LocalRealm.KDC[0].IPAddress, testConfig.LocalRealm.KDC[0].Port, testConfig.TransportType,
                testConfig.SupportedOid);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Construct Kerberos client for testing.");

            //Create and send AS request
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            KerberosTime till = new KerberosTime("20110810035805Z");
            KerberosTime from = new KerberosTime("20110811035805Z");
            client.SendAsRequest(options, null, from, till);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with no PA data.");
            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve preauthentication required error.");

            //Create sequence of PA data            
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create a sequence of PA data.");
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData, from, till);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with PA data.");
            krbError = client.ExpectKrbError();
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve Kerberos error.");
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_NEVER_VALID, krbError.ErrorCode, "If the requested expiration time minus the starttime is less than a site-determined minimum lifetime, an error message with code KDC_ERR_NEVER_VALID is returned.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KerberosError)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to test KDC when the request contains options that it does not support.")]
        public void KrbErrorBadOptions()
        {
            base.Logging();

            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName, this.testConfig.LocalRealm.User[1].Username,
                this.testConfig.LocalRealm.User[1].Password, KerberosAccountType.User, testConfig.LocalRealm.KDC[0].IPAddress, testConfig.LocalRealm.KDC[0].Port, testConfig.TransportType,
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

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Construct Kerberos client for testing.");

            //Create and send AS request
            KdcOptions options = KdcOptions.UNUSED7;
            client.SendAsRequest(options, null);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with no PA data.");
            //Recieve Kerberos error
            KerberosKrbError krbError = client.ExpectKrbError();
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve Kerberos error.");
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_BADOPTION, krbError.ErrorCode, "If the KDC cannot accommodate requested option, an error message with code KDC_ERR_BADOPTIONS is returned.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KerberosError)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to test KDC when ticket is not eligible for postdating.")]
        public void KrbErrorCannotPostdate()
        {
            base.Logging();

            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName, this.testConfig.LocalRealm.User[1].Username,
                this.testConfig.LocalRealm.User[1].Password, KerberosAccountType.User, testConfig.LocalRealm.KDC[0].IPAddress, testConfig.LocalRealm.KDC[0].Port, testConfig.TransportType,
                testConfig.SupportedOid);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Construct Kerberos client for testing.");

            // Kerberos Proxy Service is used
            if (this.testConfig.UseProxy)
            {
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Initialize KKDCP Client .");
                KKDCPClient proxyClient = new KKDCPClient(proxyClientConfig);
                proxyClient.TargetDomain = this.testConfig.LocalRealm.RealmName;
                client.UseProxy = true;
                client.ProxyClient = proxyClient;
            }

            //Create and send AS request
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            KerberosTime from = new KerberosTime("20370811035805Z");
            KerberosTime till = new KerberosTime("20370810035805Z");
            client.SendAsRequest(options, null, from, till);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with no PA data.");
            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve preauthentication required error.");

            //Create sequence of PA data            
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create a sequence of PA data.");
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData, from, till);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with PA data.");

            krbError = client.ExpectKrbError();
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve Kerberos error.");
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_CANNOT_POSTDATE, krbError.ErrorCode, "If the requested starttime indicates a time in the future beyond the acceptable clock skew but the POSTDATED option has not been specified, an error message with code KDC_ERR_CANNOT_POSTDATE is returned.");
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KerberosError)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to test KDC when integrity check on decrypted field failed.")]
        public void KrbErrorBadIntegrity()
        {
            base.Logging();

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

            //Create and send AS request
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);
            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp,
                0,
                client.Context.SelectedEType,
                this.client.Context.CName.Password,
                this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();

            // Modify ciphertext of TGT
            byte originalFirstByte = (byte)client.Context.Ticket.Ticket.enc_part.cipher.ByteArrayValue.GetValue(0);
            client.Context.Ticket.Ticket.enc_part.cipher.ByteArrayValue.SetValue((byte)(originalFirstByte + 1), 0);

            //Create and send TGS request
            client.SendTgsRequest(this.testConfig.LocalRealm.FileServer[0].Smb2ServiceName, options);
            krbError = client.ExpectKrbError();
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve Kerberos error.");
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KRB_AP_ERR_BAD_INTEGRITY, krbError.ErrorCode,
                "If decrypting the authenticator using the session key shows that it has been modified, " +
                "the KRB_AP_ERR_BAD_INTEGRITY error is returned");
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KerberosError)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to test KDC when clock skew too great.")]
        public void KrbErrorSkew()
        {
            base.Logging();

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

            //Create and send AS request
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);
            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);

            //Create sequence of PA data
            // Set timeStamp 10 minutes earlier than now.
            string timeStamp = DateTime.Now.AddMinutes(-10).ToUniversalTime().ToString("yyyyMMddHHmmss") + "Z";
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp,
                0,
                client.Context.SelectedEType,
                this.client.Context.CName.Password,
                this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            krbError = client.ExpectKrbError();
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve Kerberos error.");
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KRB_AP_ERR_SKEW, krbError.ErrorCode,
                "If the local (server) time and the client time in the authenticator differ " +
                "by more than the allowable clock skew (5 minutes), the KRB_AP_ERR_SKEW error is returned.");
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KerberosError)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to test AP when ticket and authenticator don't match.")]
        public void KrbErrorBadMatch()
        {
            base.Logging();

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

            //Create and send AS request
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);
            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp,
                0,
                client.Context.SelectedEType,
                this.client.Context.CName.Password,
                this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });

            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();

            //Create and send TGS request
            client.SendTgsRequest(this.testConfig.LocalRealm.FileServer[0].Smb2ServiceName, options);
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();

            // Change username in authenticator
            client.Context.Ticket.TicketOwner.name_string.Elements[0].Value = client.Context.Ticket.TicketOwner.name_string.Elements[0].Value.Insert(0, "BADMATCH");

            AuthorizationData data = null;
            EncryptionKey subkey = KerberosUtility.GenerateKey(client.Context.SessionKey);
            byte[] token = client.CreateGssApiToken(ApOptions.MutualRequired,
                data,
                subkey,
                ChecksumFlags.GSS_C_MUTUAL_FLAG | ChecksumFlags.GSS_C_INTEG_FLAG);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send Smb2 request.");
            KerberosKrbError error = client.GetKrbErrorFromToken(SendAndRecieveSmb2Ap(this.testConfig.LocalRealm.FileServer[0], token));
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve Kerberos error.");
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KRB_AP_ERR_BADMATCH, error.ErrorCode,
                "The name and realm of the client from the ticket are compared against the same fields in the authenticator. " +
                "If they don't match, the KRB_AP_ERR_BADMATCH error is returned");
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KerberosError)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.Smb2)]
        [Description("This test case is designed to test AP when ticket is expired.")]
        public void KrbErrorTktExpired()
        {
            base.Logging();

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
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Initialize KKDCP Client.");
                KKDCPClient proxyClient = new KKDCPClient(proxyClientConfig);
                proxyClient.TargetDomain = this.testConfig.LocalRealm.RealmName;
                client.UseProxy = true;
                client.ProxyClient = proxyClient;
            }

            //Create and send AS request
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);
            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp,
                0,
                client.Context.SelectedEType,
                this.client.Context.CName.Password,
                this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });

            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();

            //Create and send TGS request
            client.SendTgsRequest(this.testConfig.LocalRealm.FileServer[0].Smb2ServiceName, options);
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();

            // Change ticket
            EncryptionKey tgskey = testConfig.QueryKey(
                this.testConfig.LocalRealm.FileServer[0].Smb2ServiceName,
                this.testConfig.LocalRealm.RealmName,
                this.client.Context.SelectedEType);
            // Decrypt ticket
            tgsResponse.DecryptTicket(tgskey);
            // Set ticket end time 10 minutes earlier than now
            string expiredTime = DateTime.Now.AddMinutes(-10).ToUniversalTime().ToString("yyyyMMddHHmmss") + "Z";
            tgsResponse.TicketEncPart.endtime = new KerberosTime(expiredTime);
            Asn1BerEncodingBuffer encodeBuffer = new Asn1BerEncodingBuffer();
            tgsResponse.TicketEncPart.BerEncode(encodeBuffer, true);
            EncryptionType encryptType = (EncryptionType) tgsResponse.Response.ticket.enc_part.etype.Value;
            var key = KeyGenerator.MakeKey(encryptType, this.testConfig.LocalRealm.FileServer[0].Password, this.testConfig.LocalRealm.FileServer[0].ServiceSalt);
            // Re-encrypt ticket
            var encrypedData = KerberosUtility.Encrypt(
                encryptType,
                key,
                encodeBuffer.Data,
                (int)KeyUsageNumber.AS_REP_TicketAndTGS_REP_Ticket);
            tgsResponse.Response.ticket.enc_part = new EncryptedData(new KerbInt32((long)encryptType), null, new Asn1OctetString(encrypedData));

            AuthorizationData data = null;
            EncryptionKey subkey = KerberosUtility.GenerateKey(client.Context.SessionKey);
            byte[] token = client.CreateGssApiToken(ApOptions.MutualRequired,
                data,
                subkey,
                ChecksumFlags.GSS_C_MUTUAL_FLAG | ChecksumFlags.GSS_C_INTEG_FLAG);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send Smb2 request.");
            KerberosKrbError error = client.GetKrbErrorFromToken(SendAndRecieveSmb2Ap(this.testConfig.LocalRealm.FileServer[0], token));
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve Kerberos error.");
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KRB_AP_ERR_TKT_EXPIRED, error.ErrorCode,
                "if the current time is later than end time by more than the allowable clock skew (5 minutes), " +
                "the KRB_AP_ERR_TKT_EXPIRED error is returned.");
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KerberosError)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.Smb2)]
        [Description("This test case is designed to test AP when ticket is not yet valid.")]
        public void KrbErrorTktNotYetValid()
        {
            base.Logging();

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
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Initialize KKDCP Client.");
                KKDCPClient proxyClient = new KKDCPClient(proxyClientConfig);
                proxyClient.TargetDomain = this.testConfig.LocalRealm.RealmName;
                client.UseProxy = true;
                client.ProxyClient = proxyClient;
            }

            //Create and send AS request
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);
            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp,
                0,
                client.Context.SelectedEType,
                this.client.Context.CName.Password,
                this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });

            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();

            //Create and send TGS request
            client.SendTgsRequest(this.testConfig.LocalRealm.FileServer[0].Smb2ServiceName, options);
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();

            // Change ticket
            EncryptionKey tgskey = testConfig.QueryKey(
                this.testConfig.LocalRealm.FileServer[0].Smb2ServiceName,
                this.testConfig.LocalRealm.RealmName,
                this.client.Context.SelectedEType);
            // Decrypt ticket
            tgsResponse.DecryptTicket(tgskey);
            // Set ticket start time 15 minutes later than now
            string nyvTime = DateTime.Now.AddMinutes(15).ToUniversalTime().ToString("yyyyMMddHHmmss") + "Z";
            tgsResponse.TicketEncPart.starttime = new KerberosTime(nyvTime);
            Asn1BerEncodingBuffer encodeBuffer = new Asn1BerEncodingBuffer();
            tgsResponse.TicketEncPart.BerEncode(encodeBuffer, true);
            EncryptionType encryptType = (EncryptionType) tgsResponse.Response.ticket.enc_part.etype.Value;
            var key = KeyGenerator.MakeKey(encryptType, this.testConfig.LocalRealm.FileServer[0].Password, this.testConfig.LocalRealm.FileServer[0].ServiceSalt);
            // Re-encrypt ticket
            var encrypedData = KerberosUtility.Encrypt(
                encryptType,
                key,
                encodeBuffer.Data,
                (int)KeyUsageNumber.AS_REP_TicketAndTGS_REP_Ticket);
            tgsResponse.Response.ticket.enc_part = new EncryptedData(new KerbInt32((long)encryptType), null, new Asn1OctetString(encrypedData));

            AuthorizationData data = null;
            EncryptionKey subkey = KerberosUtility.GenerateKey(client.Context.SessionKey);
            byte[] token = client.CreateGssApiToken(ApOptions.MutualRequired,
                data,
                subkey,
                ChecksumFlags.GSS_C_MUTUAL_FLAG | ChecksumFlags.GSS_C_INTEG_FLAG);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send Smb2 request.");
            KerberosKrbError error = client.GetKrbErrorFromToken(SendAndRecieveSmb2Ap(this.testConfig.LocalRealm.FileServer[0], token));
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve Kerberos error.");
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KRB_AP_ERR_TKT_NYV, error.ErrorCode,
                "If the starttime is later than the current time by more than the allowable clock skew (10 minutes), " +
                "the KRB_AP_ERR_TKT_NYV error is returned.");
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KerberosError)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to test KDC when msg type is invalid.")]
        public void KrbErrorMsgType()
        {
            base.Logging();

            client = new KerberosTestClient(
                this.testConfig.LocalRealm.RealmName,
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

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Construct Kerberos client for testing.");

            //Create and send AS request
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            //Send AsRequest with invalid type
            client.SendAsRequest(options, null, 5, MsgType.None);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with no PA data.");
            KerberosKrbError krbError = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KRB_AP_ERR_MSG_TYPE, krbError.ErrorCode,
                "If the message type is not invalid, the server returns the KRB_AP_ERR_MSG_TYPE error.");
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KerberosError)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to test KDC when Field is too long for this implementation.")]
        public void KrbErrorFieldTooLong()
        {
            base.Logging();

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
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Initialize KKDCP Client.");
                KKDCPClient proxyClient = new KKDCPClient(proxyClientConfig);
                proxyClient.TargetDomain = this.testConfig.LocalRealm.RealmName;
                client.UseProxy = true;
                client.ProxyClient = proxyClient;
            }

            //Create and send AS request
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);
            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp,
                0,
                client.Context.SelectedEType,
                this.client.Context.CName.Password,
                this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });

            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request.");
            //Create and send TGS request with MsgType as KRB_AS_REQ
            client.SendTgsRequest(this.testConfig.LocalRealm.FileServer[0].Smb2ServiceName, options, MsgType.KRB_AS_REQ);
            krbError = client.ExpectKrbError();
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve Kerberos error.");
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KRB_ERR_FIELD_TOOLONG, krbError.ErrorCode,
                "If KDC does not understand how to interpret a set high bit of the length encoding receives " +
                "a request with the high order bit of the length set, it MUST return KRB_ERR_FIELD_TOOLONG.");
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KerberosError)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.Smb2)]
        [Description("This test case is designed to test AP whether it can return a KRB_ERR_GENERIC.")]
        public void KrbErrorGeneric()
        {
            base.Logging();

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
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Initialize KKDCP Client.");
                KKDCPClient proxyClient = new KKDCPClient(proxyClientConfig);
                proxyClient.TargetDomain = this.testConfig.LocalRealm.RealmName;
                client.UseProxy = true;
                client.ProxyClient = proxyClient;
            }

            //Create and send AS request
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);
            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp,
                0,
                client.Context.SelectedEType,
                this.client.Context.CName.Password,
                this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });

            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();

            //Create and send TGS request
            client.SendTgsRequest(this.testConfig.LocalRealm.FileServer[0].Smb2ServiceName, options);
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();

            AuthorizationData data = null;
            EncryptionKey subkey = KerberosUtility.GenerateKey(client.Context.SessionKey);

            // Set ApOptions as None but check on GSS_C_MUTUAL_FLAG
            byte[] token = client.CreateGssApiToken(ApOptions.None,
                data,
                subkey,
                ChecksumFlags.GSS_C_MUTUAL_FLAG);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send Smb2 request.");
            KerberosKrbError error = client.GetKrbErrorFromToken(SendAndRecieveSmb2Ap(this.testConfig.LocalRealm.FileServer[0], token));
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve Kerberos error.");
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KRB_ERR_GENERIC, error.ErrorCode,
                "When a generic security error is returned from AP, KRB_ERR_GENERIC will be returned.");
        }
    }
}
