// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net;
using Microsoft.Protocol.TestSuites.Kerberos.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using Microsoft.Protocols.TestTools.StackSdk.Security.Pac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocol.TestSuites.Kerberos.TestSuite
{
    /// <summary>
    ///
    /// </summary>
    [TestClass]
    public class KileTestSuite : TraditionTestBase
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
        [TestCategory(TestCategories.BVT)]        
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the interactive logon process.")]
        public void InteractiveLogon()
        {
            base.Logging();

            //Create kerberos test client and connect
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

            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends AS_REQ without Pre-Authentication data");
            client.SendAsRequest(options, null);
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
            client.SendAsRequest(options, seqOfPaData);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "KDC returns AS_REP");
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create and send TGS request
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends TGS_REQ");
            client.SendTgsRequest(this.testConfig.LocalRealm.ClientComputer.DefaultServiceName, options);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "KDC returns TGS_REP");
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();

            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.RealmName.ToLower(),
                tgsResponse.Response.ticket.realm.Value.ToLower(),
                "The realm in ticket should match expected.");
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.ClientComputer.DefaultServiceName,
               KerberosUtility.PrincipalName2String(tgsResponse.Response.ticket.sname),
               "The Service principal name in ticket should match expected.");

            EncryptionKey key = testConfig.QueryKey(this.testConfig.LocalRealm.ClientComputer.DefaultServiceName, this.testConfig.LocalRealm.RealmName, this.client.Context.SelectedEType);
            tgsResponse.DecryptTicket(key);

            //tgsResponse.DecryptTicket(this.testConfig.LocalRealm.ClientComputer.Password, this.testConfig.LocalRealm.ClientComputer.ServiceSalt);
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.RealmName.ToLower(),
               tgsResponse.TicketEncPart.crealm.Value.ToLower(),
               "The realm in ticket encrypted part should match expected.");
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[1].Username.ToLower(),
                KerberosUtility.PrincipalName2String(tgsResponse.TicketEncPart.cname).ToLower(),
                "The client principal name in ticket encrypted part should match expected.");
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.FAST)]
        [Feature(Feature.FAST)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the interactive logon process with FAST.")]
        public void InteractiveLogonUseFast()
        {
            base.Logging();

            client = new KerberosTestClient(
                this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.ClientComputer.NetBiosName,
                this.testConfig.LocalRealm.ClientComputer.Password,
                KerberosAccountType.Device,
                testConfig.LocalRealm.KDC[0].IPAddress,
                testConfig.LocalRealm.KDC[0].Port,
                testConfig.TransportType,
                testConfig.SupportedOid,
                testConfig.LocalRealm.ClientComputer.AccountSalt);

            // Kerberos Proxy Service is used
            if (this.testConfig.UseProxy)
            {
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Initialize KKDCP Client .");
                KKDCPClient proxyClient = new KKDCPClient(proxyClientConfig);
                proxyClient.TargetDomain = this.testConfig.LocalRealm.RealmName;
                client.UseProxy = true;
                client.ProxyClient = proxyClient;
            }

            // AS_REQ and KRB-ERROR using device principal
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);
            METHOD_DATA methodData;
            KerberosKrbError krbError1 = client.ExpectPreauthRequiredError(out methodData);

            // AS_REQ and AS_REP using device principal
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(
                timeStamp,
                0,
                client.Context.SelectedEType,
                this.client.Context.CName.Password,
                this.client.Context.CName.Salt);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data });
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");
            BaseTestSite.Assert.IsNotNull(asResponse.EncPart, "The encrypted part of AS response is decrypted.");
            BaseTestSite.Assert.IsNotNull(asResponse.EncPart.key, "AS response should contain a session key.");
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                string.Format("The type of AS-REP encrypted part is {0}.", asResponse.EncPart.GetType().Name));

            // Switch to user principal
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Switch to user principal.");
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                string.Format("Construct Kerberos client using user account: {0}.",
                this.testConfig.LocalRealm.User[1].Username));
            client = new KerberosTestClient(
                this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[1].Username,
                this.testConfig.LocalRealm.User[1].Password,
                KerberosAccountType.User,
                client.Context.Ticket,
                client.Context.SessionKey,
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

            // FAST armored AS_REQ and KRB-ERROR using user principal
            //Create a "random" key.
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send FAST armored AS request with no pre-authentication padata.");
            var subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password02!", "this is a salt");
            var fastOptions = new Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth.FastOptions(KerberosUtility.ConvertInt2Flags((int)0));
            var apOptions = ApOptions.None;

            Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { new PA_DATA(new KerbInt32((long)PaDataType.PA_FX_FAST), null) });
            client.SendAsRequestWithFast(options, seqOfPaData2, null, subkey, fastOptions, apOptions);
            KerberosKrbError krbError2 = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(krbError2.ErrorCode, KRB_ERROR_CODE.KDC_ERR_PREAUTH_REQUIRED, "Pre-authentication required.");

            // FAST armored AS_REQ and AS_REP using user principal
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with PaEncryptedChallenge.");
            var userKey = KerberosUtility.MakeKey(
                client.Context.SelectedEType,
                client.Context.CName.Password,
                client.Context.CName.Salt);
            PaEncryptedChallenge paEncTimeStamp3 = new PaEncryptedChallenge(
                client.Context.SelectedEType,
                KerberosUtility.CurrentKerberosTime.Value,
                0,
                client.Context.FastArmorkey,
                userKey);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> outerSeqPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paPacRequest.Data });
            Asn1SequenceOf<PA_DATA> seqOfPaData3 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp3.Data });
            client.SendAsRequestWithFast(options, seqOfPaData3, outerSeqPaData, subkey, fastOptions, apOptions);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve AS response.");
            KerberosAsResponse userKrbAsRep = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(userKrbAsRep.Response.ticket, "AS response should contain a TGT.");
            BaseTestSite.Assert.IsNotNull(userKrbAsRep.EncPart, "The encrypted part of AS response is decrypted.");
            BaseTestSite.Assert.IsNotNull(userKrbAsRep.EncPart.key, "AS response should contain a session key.");
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                string.Format("The type of AS-REP encrypted part is {0}.", userKrbAsRep.EncPart.GetType().Name));
            // FAST armored TGS_REQ and TGS_REP using user principal
            subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password03!", "this is a salt");
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send FAST armored TGS request.");
            client.SendTgsRequestWithFast(testConfig.LocalRealm.ClientComputer.DefaultServiceName, options, null, null, subkey, fastOptions, apOptions);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve TGS response.");
            KerberosTgsResponse userKrbTgsRep = client.ExpectTgsResponse(KeyUsageNumber.TGS_REP_encrypted_part_subkey);
            BaseTestSite.Assert.IsNotNull(userKrbTgsRep.EncPart, "The encrypted part of TGS-REP is decrypted.");

            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.RealmName.ToLower(),
                userKrbTgsRep.Response.ticket.realm.Value.ToLower(),
                "The realm in ticket should match expected.");
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.ClientComputer.DefaultServiceName,
               KerberosUtility.PrincipalName2String(userKrbTgsRep.Response.ticket.sname),
               "The Service principal name in ticket should match expected.");

            EncryptionKey key = testConfig.QueryKey(this.testConfig.LocalRealm.ClientComputer.DefaultServiceName, this.testConfig.LocalRealm.RealmName, this.client.Context.SelectedEType);
            userKrbTgsRep.DecryptTicket(key);

            //userKrbTgsRep.DecryptTicket(this.testConfig.LocalRealm.ClientComputer.Password, this.testConfig.LocalRealm.ClientComputer.ServiceSalt);
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.RealmName.ToLower(),
               userKrbTgsRep.TicketEncPart.crealm.Value.ToLower(),
               "The realm in ticket encrypted part should match expected.");
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[1].Username.ToLower(),
                KerberosUtility.PrincipalName2String(userKrbTgsRep.TicketEncPart.cname).ToLower(),
                "The client principal name in ticket encrypted part should match expected.");
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.FAST)]
        [Feature(Feature.FAST)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the interactive logon process with explicit FAST armor in TGS-REQ.")]
        public void InteractiveLogonUseExplicitFast()
        {
            base.Logging();

            client = new KerberosTestClient(
                this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.ClientComputer.NetBiosName,
                this.testConfig.LocalRealm.ClientComputer.Password,
                KerberosAccountType.Device,
                testConfig.LocalRealm.KDC[0].IPAddress,
                testConfig.LocalRealm.KDC[0].Port,
                testConfig.TransportType,
                testConfig.SupportedOid,
                testConfig.LocalRealm.ClientComputer.AccountSalt);

            // Kerberos Proxy Service is used
            if (this.testConfig.UseProxy)
            {
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Initialize KKDCP Client .");
                KKDCPClient proxyClient = new KKDCPClient(proxyClientConfig);
                proxyClient.TargetDomain = this.testConfig.LocalRealm.RealmName;
                client.UseProxy = true;
                client.ProxyClient = proxyClient;
            }

            // AS_REQ and KRB-ERROR using device principal
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with no PA data.");
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve preauthentication required error.");
            METHOD_DATA methodData;
            KerberosKrbError krbError1 = client.ExpectPreauthRequiredError(out methodData);

            // AS_REQ and AS_REP using device principal
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with PaEncTimeStamp.");
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(
                timeStamp,
                0,
                client.Context.SelectedEType,
                this.client.Context.CName.Password,
                this.client.Context.CName.Salt);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data });
            client.SendAsRequest(options, seqOfPaData);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve AS response.");
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");
            BaseTestSite.Assert.IsNotNull(asResponse.EncPart, "The encrypted part of AS response is decrypted.");
            BaseTestSite.Assert.IsNotNull(asResponse.EncPart.key, "AS response should contain a session key.");
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                string.Format("The type of AS-REP encrypted part is {0}.", asResponse.EncPart.GetType().Name));

            // Switch to user principal
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Switch to user principal.");
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                string.Format("Construct Kerberos client using user account: {0}.",
                this.testConfig.LocalRealm.User[1].Username));
            client = new KerberosTestClient(
                this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[1].Username,
                this.testConfig.LocalRealm.User[1].Password,
                KerberosAccountType.User,
                client.Context.Ticket,
                client.Context.SessionKey,
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

            // FAST armored AS_REQ and KRB-ERROR using user principal
            //Create a "random" key.
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send FAST armored AS request with no pre-authentication padata.");
            var subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password02!", "this is a salt");
            var fastOptions = new Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth.FastOptions(KerberosUtility.ConvertInt2Flags((int)0));
            var apOptions = ApOptions.None;

            Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { new PA_DATA(new KerbInt32((long)PaDataType.PA_FX_FAST), null) });
            client.SendAsRequestWithFast(options, seqOfPaData2, null, subkey, fastOptions, apOptions);
            KerberosKrbError krbError2 = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(krbError2.ErrorCode, KRB_ERROR_CODE.KDC_ERR_PREAUTH_REQUIRED, "Pre-authentication required.");

            // FAST armored AS_REQ and AS_REP using user principal
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with PaEncryptedChallenge.");
            var userKey = KerberosUtility.MakeKey(
                client.Context.SelectedEType,
                client.Context.CName.Password,
                client.Context.CName.Salt);
            PaEncryptedChallenge paEncTimeStamp3 = new PaEncryptedChallenge(
                client.Context.SelectedEType,
                KerberosUtility.CurrentKerberosTime.Value,
                0,
                client.Context.FastArmorkey,
                userKey);
            Asn1SequenceOf<PA_DATA> seqOfPaData3 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp3.Data });
            client.SendAsRequestWithFast(options, seqOfPaData3, null, subkey, fastOptions, apOptions);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve AS response.");
            KerberosAsResponse userKrbAsRep = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(userKrbAsRep.Response.ticket, "AS response should contain a TGT.");
            BaseTestSite.Assert.IsNotNull(userKrbAsRep.EncPart, "The encrypted part of AS response is decrypted.");
            BaseTestSite.Assert.IsNotNull(userKrbAsRep.EncPart.key, "AS response should contain a session key.");
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                string.Format("The type of AS-REP encrypted part is {0}.", userKrbAsRep.EncPart.GetType().Name));
            // FAST armored TGS_REQ and TGS_REP using user principal
            subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password03!", "this is a salt");
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send explicit FAST armored TGS request.");
            client.SendTgsRequestWithExplicitFast(testConfig.LocalRealm.ClientComputer.DefaultServiceName, options, null, null, subkey, fastOptions, apOptions);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve TGS response.");
            KerberosTgsResponse userKrbTgsRep = client.ExpectTgsResponse(KeyUsageNumber.TGS_REP_encrypted_part_subkey);
            BaseTestSite.Assert.IsNotNull(userKrbTgsRep.EncPart, "The encrypted part of TGS-REP is decrypted.");

            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.RealmName.ToLower(),
                userKrbTgsRep.Response.ticket.realm.Value.ToLower(),
                "The realm in ticket should match expected.");
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.ClientComputer.DefaultServiceName,
               KerberosUtility.PrincipalName2String(userKrbTgsRep.Response.ticket.sname),
               "The Service principal name in ticket should match expected.");

            EncryptionKey key = testConfig.QueryKey(this.testConfig.LocalRealm.ClientComputer.DefaultServiceName, this.testConfig.LocalRealm.RealmName, this.client.Context.SelectedEType);
            userKrbTgsRep.DecryptTicket(key);

            //userKrbTgsRep.DecryptTicket(this.testConfig.LocalRealm.ClientComputer.Password, this.testConfig.LocalRealm.ClientComputer.ServiceSalt);
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.RealmName.ToLower(),
               userKrbTgsRep.TicketEncPart.crealm.Value.ToLower(),
               "The realm in ticket encrypted part should match expected.");
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[1].Username.ToLower(),
                KerberosUtility.PrincipalName2String(userKrbTgsRep.TicketEncPart.cname).ToLower(),
                "The client principal name in ticket encrypted part should match expected.");
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Kile | Feature.Pac)]
        [ApplicationServer(ApplicationServer.Smb2)]
        [Description("This test case is designed to verify the process that a user logons to an SMB server using Kerberos authentication.")]
        public void NetworkLogonSmb2()
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
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create and send TGS request
            client.SendTgsRequest(this.testConfig.LocalRealm.FileServer[0].Smb2ServiceName, options);
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();

            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.FileServer[0].Smb2ServiceName,
                KerberosUtility.PrincipalName2String(tgsResponse.Response.ticket.sname),
                "Service principal name in service ticket should match expected.");
            
            EncryptionKey key = testConfig.QueryKey(this.testConfig.LocalRealm.FileServer[0].Smb2ServiceName, client.Context.Realm.ToString(), client.Context.SelectedEType);
            tgsResponse.DecryptTicket(key);

            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.RealmName.ToLower(),
                tgsResponse.TicketEncPart.crealm.Value.ToLower(),
                "Realm name in service ticket encrypted part should match expected.");
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[1].Username.ToLower(),
                KerberosUtility.PrincipalName2String(tgsResponse.TicketEncPart.cname).ToLower(),
                "User name in service ticket encrypted part should match expected.");

            //Assert authorization data
            if (this.testConfig.IsKileImplemented)
            {
                BaseTestSite.Assert.IsNotNull(tgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(tgsResponse.TicketEncPart.authorization_data.Elements);
                BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2KPac.");
            }

            AuthorizationData data = null;
            EncryptionKey subkey = KerberosUtility.GenerateKey(client.Context.SessionKey);
            byte[] token = client.CreateGssApiToken(ApOptions.MutualRequired,
                data,
                subkey,
                ChecksumFlags.GSS_C_MUTUAL_FLAG | ChecksumFlags.GSS_C_INTEG_FLAG);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send Smb2 request.");
            KerberosApResponse apRep = client.GetApResponseFromToken(SendAndRecieveSmb2Ap(this.testConfig.LocalRealm.FileServer[0], token));
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve Smb2 response.");
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12)]        
        [TestCategory(TestCategories.Claim)]        
        [ApplicationServer(ApplicationServer.Smb2)]
        [Description("This test case is designed to verify the process that a user logons to an SMB server using Kerberos authentication with Claims enabled.")]
        public void NetworkLogonClaimsSmb2()
        {
            base.Logging();

            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[2].Username,
                this.testConfig.LocalRealm.User[2].Password,
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
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with PaEncTimeStamp, PaPacRequest and paPacOptions.");
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp,
                0,
                client.Context.SelectedEType,
                client.Context.CName.Password,
                this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data, paPacOptions.Data });
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();

            //Verify encrypted padata
            PaSupportedEncTypes paSupportedEncTypes = null;
            BaseTestSite.Assert.IsNotNull(asResponse.EncPart, "The encrypted part of AS-REP is decrypted.");
            BaseTestSite.Assert.IsNotNull(asResponse.EncPart.pa_datas, "The encrypted padata is not null.");
            if (this.testConfig.IsKileImplemented)
            {
                foreach (var padata in asResponse.EncPart.pa_datas.Elements)
                {
                    var parsedPadata = PaDataParser.ParseRepPaData(padata);
                    if (parsedPadata is PaSupportedEncTypes)
                        paSupportedEncTypes = parsedPadata as PaSupportedEncTypes;
                }
                BaseTestSite.Assert.IsNotNull(paSupportedEncTypes, "The encrypted padata of AS-REP contains PA_SUPPORTED_ENCTYPES.");
                if (this.testConfig.IsClaimSupported)
                    BaseTestSite.Assert.IsTrue(
                        paSupportedEncTypes.SupportedEncTypes.HasFlag(SupportedEncryptionTypes.Claims_Supported),
                        "Claims is supported.");
            }
            //TGS exchange
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send FAST armored TGS request: {0}.", this.testConfig.LocalRealm.FileServer[0].Smb2ServiceName);
            Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paPacRequest.Data, paPacOptions.Data });
            client.SendTgsRequest(this.testConfig.LocalRealm.FileServer[0].Smb2ServiceName, options, seqOfPaData2);
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();
            
            EncryptionKey key = testConfig.QueryKey(this.testConfig.LocalRealm.FileServer[0].Smb2ServiceName, client.Context.Realm.ToString(), client.Context.SelectedEType);
            tgsResponse.DecryptTicket(key);
            
            BaseTestSite.Assert.IsNotNull(tgsResponse.EncPart, "The encrypted part of TGS-REP is decrypted.");

            //Verify TGS encryped padata
            paSupportedEncTypes = null;
            BaseTestSite.Assert.IsNotNull(tgsResponse.EncPart, "The encrypted part of TGS-REP is decrypted.");
            BaseTestSite.Assert.IsNotNull(tgsResponse.EncPart.pa_datas, "The encrypted padata of TGS-REP is not null.");
            if (this.testConfig.IsKileImplemented)
            {
                foreach (var padata in tgsResponse.EncPart.pa_datas.Elements)
                {
                    var parsedPadata = PaDataParser.ParseRepPaData(padata);
                    if (parsedPadata is PaSupportedEncTypes)
                        paSupportedEncTypes = parsedPadata as PaSupportedEncTypes;
                }
                BaseTestSite.Assert.IsNotNull(paSupportedEncTypes, "The encrypted padata of TGS-REP contains PA_SUPPORTED_ENCTYPES.");
            }
            // ***The SupportedEncryptionTypes.Claims_Supported bit is not set. May be a bug or a TDI.***
            //
            //BaseTestSite.Assert.IsTrue(
            //    paSupportedEncTypes.SupportedEncTypes.HasFlag(SupportedEncryptionTypes.Claims_Supported),
            //    "Claims is supported.");

            if (this.testConfig.IsKileImplemented)
            {
                BaseTestSite.Assert.IsNotNull(tgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(tgsResponse.TicketEncPart.authorization_data.Elements);
                BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2KPac.");
            }

            AuthorizationData data = null;
            EncryptionKey subkey = KerberosUtility.GenerateKey(client.Context.SessionKey);
            byte[] token = client.CreateGssApiToken(ApOptions.MutualRequired,
                data,
                subkey,
                ChecksumFlags.GSS_C_MUTUAL_FLAG | ChecksumFlags.GSS_C_INTEG_FLAG);

            KerberosApResponse apRep = client.GetApResponseFromToken(SendAndRecieveSmb2Ap(this.testConfig.LocalRealm.FileServer[0], token)); ;
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.FAST)] 
        [Feature(Feature.Claim | Feature.FAST | Feature.Kile | Feature.Pac)]
        [ApplicationServer(ApplicationServer.Smb2)]
        [Description("This test case is designed to verify the process that a user logons to an SMB server using Kerberos authentication with compound identity.")]
        public void NetworkLogonCompoundIdentitySmb2()
        {
            base.Logging();

            client = new KerberosTestClient(
                this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.ClientComputer.NetBiosName,
                this.testConfig.LocalRealm.ClientComputer.Password,
                KerberosAccountType.Device,
                testConfig.LocalRealm.KDC[0].IPAddress,
                testConfig.LocalRealm.KDC[0].Port,
                testConfig.TransportType,
                testConfig.SupportedOid,
                testConfig.LocalRealm.ClientComputer.AccountSalt);

            // Kerberos Proxy Service is used
            if (this.testConfig.UseProxy)
            {
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Initialize KKDCP Client .");
                KKDCPClient proxyClient = new KKDCPClient(proxyClientConfig);
                proxyClient.TargetDomain = this.testConfig.LocalRealm.RealmName;
                client.UseProxy = true;
                client.ProxyClient = proxyClient;
            }

            // AS_REQ and KRB-ERROR using device principal
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);
            METHOD_DATA methodData;
            KerberosKrbError krbError1 = client.ExpectPreauthRequiredError(out methodData);

            // AS_REQ and AS_REP using device principal
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp,
                0,
                client.Context.SelectedEType,
                this.client.Context.CName.Password,
                this.client.Context.CName.Salt);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data });
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();

            // Switch to user principal
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[2].Username,
                this.testConfig.LocalRealm.User[2].Password,
                KerberosAccountType.User,
                client.Context.Ticket,
                client.Context.SessionKey,
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

            // FAST armored AS_REQ and KRB-ERROR using user principal
            //Create a "random" key.
            var subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password02!", "this is a salt");

            var fastOptions = new Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth.FastOptions(KerberosUtility.ConvertInt2Flags((int)0));
            var apOptions = ApOptions.None;
            string timeStamp2 = KerberosUtility.CurrentKerberosTime.Value;
            PaFxFastReq paFxFastReq = new PaFxFastReq(null);
            //PaEncTimeStamp paEncTimeStamp2 = new PaEncTimeStamp(timeStamp2, 0, client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { (paFxFastReq.Data) });

            client.SendAsRequestWithFast(options, seqOfPaData2, null, subkey, fastOptions, apOptions);
            KerberosKrbError krbError2 = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(krbError2.ErrorCode, KRB_ERROR_CODE.KDC_ERR_PREAUTH_REQUIRED, "Pre-authentication required.");

            // FAST armored AS_REQ and AS_REP using user principal
            var userKey = KerberosUtility.MakeKey(
                client.Context.SelectedEType,
                client.Context.CName.Password,
                client.Context.CName.Salt);
            PaEncryptedChallenge paEncTimeStamp3 = new PaEncryptedChallenge(
                client.Context.SelectedEType,
                KerberosUtility.CurrentKerberosTime.Value,
                0,
                client.Context.FastArmorkey,
                userKey);
            Asn1SequenceOf<PA_DATA> seqOfPaData3 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp3.Data });
            PaPacRequest paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> outerSeqPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paPacRequest.Data, paPacOptions.Data });
            client.SendAsRequestWithFast(options, seqOfPaData3, outerSeqPaData, subkey, fastOptions, apOptions);
            KerberosAsResponse userKrbAsRep = client.ExpectAsResponse();

            PaSupportedEncTypes paSupportedEncTypes = null;
            if (this.testConfig.IsKileImplemented)
            {
                foreach (var padata in userKrbAsRep.EncPart.pa_datas.Elements)
                {
                    var parsedPadata = PaDataParser.ParseRepPaData(padata);
                    if (parsedPadata is PaSupportedEncTypes)
                        paSupportedEncTypes = parsedPadata as PaSupportedEncTypes;
                }

                BaseTestSite.Assert.IsNotNull(paSupportedEncTypes, "The encrypted padata of AS-REP contains PA_SUPPORTED_ENCTYPES.");
                if (testConfig.IsClaimSupported)
                {
                    BaseTestSite.Assert.IsTrue(
                        paSupportedEncTypes.SupportedEncTypes.HasFlag(SupportedEncryptionTypes.Claims_Supported),
                        "Claims is supported.");
                }
                BaseTestSite.Assert.IsTrue(
                    paSupportedEncTypes.SupportedEncTypes.HasFlag(SupportedEncryptionTypes.FAST_Supported),
                    "FAST is supported.");
            }
            // FAST armored TGS_REQ and TGS_REP using user principal
            subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password03!", "this is a salt");

            client.Context.ArmorSessionKey = client.Context.Ticket.SessionKey;
            client.Context.ArmorTicket = client.Context.Ticket;

            client.SendTgsRequestWithExplicitFast(testConfig.LocalRealm.FileServer[0].Smb2ServiceName, options, null, null, subkey, fastOptions, apOptions);
            KerberosTgsResponse userKrbTgsRep = client.
                ExpectTgsResponse(KeyUsageNumber.TGS_REP_encrypted_part_subkey);
            paSupportedEncTypes = null;
            BaseTestSite.Assert.IsNotNull(asResponse.EncPart, "The encrypted part of AS-REP is decrypted.");
            BaseTestSite.Assert.IsNotNull(asResponse.EncPart.pa_datas, "The encrypted padata is not null.");
            if (this.testConfig.IsKileImplemented)
            {
                foreach (var padata in userKrbTgsRep.EncPart.pa_datas.Elements)
                {
                    var parsedPadata = PaDataParser.ParseRepPaData(padata);
                    if (parsedPadata is PaSupportedEncTypes)
                        paSupportedEncTypes = parsedPadata as PaSupportedEncTypes;
                }
                BaseTestSite.Assert.IsNotNull(paSupportedEncTypes, "The encrypted padata of AS-REP contains PA_SUPPORTED_ENCTYPES.");
                if (this.testConfig.IsClaimSupported)
                {
                    BaseTestSite.Assert.IsTrue(
                        paSupportedEncTypes.SupportedEncTypes.HasFlag(SupportedEncryptionTypes.CompoundIdentity_Supported),
                        "Compound identity is supported.");
                }
            }
            AuthorizationData data = null;
            subkey = KerberosUtility.GenerateKey(client.Context.SessionKey);
            byte[] token = client.CreateGssApiToken(ApOptions.MutualRequired,
                data,
                subkey,
                ChecksumFlags.GSS_C_MUTUAL_FLAG | ChecksumFlags.GSS_C_INTEG_FLAG);

            KerberosApResponse apRep = client.GetApResponseFromToken(SendAndRecieveSmb2Ap(this.testConfig.LocalRealm.FileServer[0], token)); ;
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.HttpAp)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Kile | Feature.Pac)]
        [ApplicationServer(ApplicationServer.Http)]
        [Description("This test case is designed to verify the process that a user logons to an HTTP server using Kerberos authentication.")]
        public void NetworkLogonHttp()
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
                this.client.Context.SelectedEType,
                this.client.Context.CName.Password,
                this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create and send TGS request
            client.SendTgsRequest(this.testConfig.LocalRealm.WebServer[0].HttpServiceName, options);
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();

            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.WebServer[0].HttpServiceName,
                KerberosUtility.PrincipalName2String(tgsResponse.Response.ticket.sname),
                "Service principal name in service ticket should match expected.");
            
            EncryptionKey key = testConfig.QueryKey(this.testConfig.LocalRealm.WebServer[0].HttpServiceName, this.testConfig.LocalRealm.RealmName, this.client.Context.SelectedEType);
            tgsResponse.DecryptTicket(key);

            //tgsResponse.DecryptTicket(this.testConfig.LocalRealm.WebServer[0].Password, this.testConfig.LocalRealm.WebServer[0].ServiceSalt);
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.RealmName.ToLower(),
                tgsResponse.TicketEncPart.crealm.Value.ToLower(),
                "Realm name in service ticket encrypted part should match expected.");
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[1].Username.ToLower(),
                KerberosUtility.PrincipalName2String(tgsResponse.TicketEncPart.cname).ToLower(),
                "User name in service ticket encrypted part should match expected.");

            //Assert authorization data
            if (this.testConfig.IsKileImplemented)
            {
                BaseTestSite.Assert.IsNotNull(tgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(tgsResponse.TicketEncPart.authorization_data.Elements);
                BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2KPac.");
            }

            AuthorizationData data = null;
            EncryptionKey subkey = KerberosUtility.GenerateKey(client.Context.SessionKey);
            byte[] token = client.CreateGssApiToken(ApOptions.MutualRequired,
                data,
                subkey,
                ChecksumFlags.GSS_C_MUTUAL_FLAG | ChecksumFlags.GSS_C_INTEG_FLAG);

            //AP exchange part
            //Negotiate authentication methods
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send Http request without authorization data.");
            HttpFunctionalTestClient httpclient = new HttpFunctionalTestClient(this.testConfig.LocalRealm.WebServer[0].HttpUri);
            HttpStatusCode status = httpclient.GetHttpResponse();
            BaseTestSite.Assert.AreEqual(HttpStatusCode.Unauthorized, status, "Http server requires authorization data.");
            //Get authentication methods
            string[] methods = null;
            methods = httpclient.GetAuthMethods();
            BaseTestSite.Assert.IsNotNull(methods, "Negotiate authentication method is inside the authentication header.");

            //Sent AP request with security token
            httpclient = new HttpFunctionalTestClient(this.testConfig.LocalRealm.WebServer[0].HttpUri, token);
            httpclient.SetNegoAuthHeader(token);
            byte[] repToken = null;
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send Http request with authorization data.");
            status = httpclient.GetHttpResponse();
            BaseTestSite.Assert.AreEqual(HttpStatusCode.OK, status, "Receive Http response status.");
            //get response success
            repToken = httpclient.GetNegoAuthHeader();
            BaseTestSite.Assert.IsNotNull(repToken, "AP_REP is inside the authentication header.");

            //verify repToken here
            if (repToken != null)
            {
                KerberosApResponse apRep = client.GetApResponseFromToken(repToken);
            }
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.LdapAp)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Kile | Feature.Pac)]
        [ApplicationServer(ApplicationServer.Ldap)]
        [Description("This test case is designed to verify the process that a user logons to an LDAP server using Kerberos authentication.")]
        public void NetworkLogonLdap()
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
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with no PA data.");

            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Receive preauthentication required error.");

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, this.client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with PA data.");
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create and send TGS request
            client.SendTgsRequest(this.testConfig.LocalRealm.LdapServer[0].LdapServiceName, options);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send TGS request.");
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Receive a TGS response.");


            AuthorizationData data = null;
            EncryptionKey subkey = KerberosUtility.GenerateKey(client.Context.SessionKey);
            byte[] token = client.CreateGssApiToken(ApOptions.MutualRequired,
                data,
                subkey,
                ChecksumFlags.GSS_C_MUTUAL_FLAG | ChecksumFlags.GSS_C_INTEG_FLAG,
                this.testConfig.LocalRealm.LdapServer[0].GssToken);

            //AP exchange part
            byte[] repToken = this.SendAndRecieveLdapAp(this.testConfig.LocalRealm.LdapServer[0], token, this.testConfig.TrustedRealm.LdapServer[0].GssToken);
            KerberosApResponse apResponse = client.GetApResponseFromToken(repToken, this.testConfig.LocalRealm.LdapServer[0].GssToken);
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.CrossRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Kile | Feature.Pac)]
        [ApplicationServer(ApplicationServer.Smb2)]
        [Description("This test case is designed to verify the process that a user logons to an SMB server in another realm using Kerberos authentication.")]
        public void CrossRealmNetworkLogonSmb2()
        {
            base.Logging();

            using (client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[1].Username,
                this.testConfig.LocalRealm.User[1].Password,
                KerberosAccountType.User,
                testConfig.LocalRealm.KDC[0].IPAddress,
                testConfig.LocalRealm.KDC[0].Port,
                testConfig.TransportType,
                testConfig.SupportedOid))
            {
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
                    this.client.Context.SelectedEType,
                    this.client.Context.CName.Password,
                    this.client.Context.CName.Salt);
                PaPacRequest paPacRequest = new PaPacRequest(true);
                Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });
                //Create and send AS request
                client.SendAsRequest(options, seqOfPaData);
                KerberosAsResponse asResponse = client.ExpectAsResponse();

                //Create and send TGS request
                if (this.testConfig.TrustType == Adapter.TrustType.Forest)
                {
                    client.SendTgsRequest(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, options);
                }
                else if (this.testConfig.TrustType == Adapter.TrustType.Realm)
                {
                    client.SendTgsRequest(this.testConfig.TrustedRealm.KDC[0].DefaultServiceName, options);
                }
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send TGS request");
                KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();
                EncryptionKey key = testConfig.QueryKey(
                    this.testConfig.TrustedRealm.KDC[0].DefaultServiceName + "@" + this.testConfig.LocalRealm.RealmName,
                    client.Context.Realm.ToString(),
                    client.Context.SelectedEType);
                tgsResponse.DecryptTicket(key);

                BaseTestSite.Log.Add(LogEntryKind.Comment, "Receive a referral TGS response.");

            BaseTestSite.Assert.AreEqual(this.testConfig.TrustedRealm.KDC[0].DefaultServiceName,
               KerberosUtility.PrincipalName2String(tgsResponse.Response.ticket.sname),
               "The service principal name in referral ticket should match expected.");
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.RealmName.ToLower(),
               tgsResponse.Response.ticket.realm.Value.ToLower(),
               "The realm name in referral ticket should match expected.");

            //Change realm
            client.ChangeRealm(this.testConfig.TrustedRealm.RealmName,
                this.testConfig.TrustedRealm.KDC[0].IPAddress,
                this.testConfig.TrustedRealm.KDC[0].Port,
                this.testConfig.TransportType);

            //Create and send referral TGS request
            client.SendTgsRequest(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, options);
            KerberosTgsResponse refTgsResponse = client.ExpectTgsResponse();

            BaseTestSite.Assert.AreEqual(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName,
               KerberosUtility.PrincipalName2String(refTgsResponse.Response.ticket.sname),
               "The service principal name in service ticket should match expected.");
            BaseTestSite.Assert.AreEqual(this.testConfig.TrustedRealm.RealmName.ToLower(),
               refTgsResponse.Response.ticket.realm.Value.ToLower(),
               "The realm name in service ticket should match expected.");

                key = testConfig.QueryKey(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, client.Context.Realm.ToString(), client.Context.SelectedEType);
                refTgsResponse.DecryptTicket(key);

            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.RealmName.ToLower(),
                refTgsResponse.TicketEncPart.crealm.Value.ToLower(),
                "Realm name in service ticket encrypted part should match expected.");
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[1].Username.ToLower(),
                KerberosUtility.PrincipalName2String(refTgsResponse.TicketEncPart.cname).ToLower(),
                "User name in service ticket encrypted part should match expected.");

            AuthorizationData data = null;
            EncryptionKey subkey = KerberosUtility.GenerateKey(client.Context.SessionKey);
            byte[] token = client.CreateGssApiToken(ApOptions.MutualRequired,
                data,
                subkey,
                ChecksumFlags.GSS_C_MUTUAL_FLAG | ChecksumFlags.GSS_C_INTEG_FLAG);

                BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send Smb2 request.");
                KerberosApResponse apRep = client.GetApResponseFromToken(SendAndRecieveSmb2Ap(this.testConfig.TrustedRealm.FileServer[0], token));
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve Smb2 response.");
            }
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.CrossRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Kile | Feature.Pac)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the process that KDC can issue a referral ticket.")]
        public void CrossRealmGetReferralTGT()
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
                this.client.Context.SelectedEType,
                this.client.Context.CName.Password,
                this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            

            //Create and send TGS request            
            if (this.testConfig.TrustType == Adapter.TrustType.Forest)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, options);
            }
            else if (this.testConfig.TrustType == Adapter.TrustType.Realm)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.KDC[0].DefaultServiceName, options);
            }
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();
            EncryptionKey key = testConfig.QueryKey(
                    this.testConfig.TrustedRealm.KDC[0].DefaultServiceName + "@" + this.testConfig.LocalRealm.RealmName,
                    client.Context.Realm.ToString(),
                    client.Context.SelectedEType);
            tgsResponse.DecryptTicket(key);

            BaseTestSite.Assert.AreEqual(this.testConfig.TrustedRealm.KDC[0].DefaultServiceName,
               KerberosUtility.PrincipalName2String(tgsResponse.Response.ticket.sname),
               "The service principal name in referral ticket should match expected.");
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.RealmName.ToLower(),
               tgsResponse.Response.ticket.realm.Value.ToLower(),
               "The realm name in referral ticket should match expected.");

            //Change realm
            client.ChangeRealm(this.testConfig.TrustedRealm.RealmName,
                this.testConfig.TrustedRealm.KDC[0].IPAddress,
                this.testConfig.TrustedRealm.KDC[0].Port,
                this.testConfig.TransportType);

            //Create and send referral TGS request
            client.SendTgsRequest(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, options);
            KerberosTgsResponse refTgsResponse = client.ExpectTgsResponse();

            BaseTestSite.Assert.AreEqual(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName,
               KerberosUtility.PrincipalName2String(refTgsResponse.Response.ticket.sname),
               "The service principal name in service ticket should match expected.");
            BaseTestSite.Assert.AreEqual(this.testConfig.TrustedRealm.RealmName.ToLower(),
               refTgsResponse.Response.ticket.realm.Value.ToLower(),
               "The realm name in service ticket should match expected.");

            //verify referral info
            PaSvrReferralInfo paSvrReferralInfo = null;

            foreach (PA_DATA item in tgsResponse.EncPart.pa_datas.Elements)
            {
                if (item.padata_type.Value == (long)PaDataType.PA_SVR_REFERRAL_INFO)
                {
                    paSvrReferralInfo = PaSvrReferralInfo.Parse(item);
                    break;
                }
            }
            BaseTestSite.Assert.IsNotNull(paSvrReferralInfo, "Tgs response should include PaSvrReferralInfo in encrypted part.");
            BaseTestSite.Assert.AreEqual(this.testConfig.TrustedRealm.RealmName.ToLower(),
                paSvrReferralInfo.PaSvrReferralData.referred_realm.Value.ToLower(),
                "Realm name in PaSvrReferralInfo should match expect trusted realm name.");
            
            //decrypt ticket in the TGS response
            key = testConfig.QueryKey(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, client.Context.Realm.ToString(), client.Context.SelectedEType);
            refTgsResponse.DecryptTicket(key);

            
            //verify the unencrypted part
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.RealmName.ToLower(),
                refTgsResponse.TicketEncPart.crealm.Value.ToLower(),
                "Realm name in service ticket encrypted part should match expected.");
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[1].Username.ToLower(),
                KerberosUtility.PrincipalName2String(refTgsResponse.TicketEncPart.cname).ToLower(),
                "User name in service ticket encrypted part should match expected.");
                  
                        
            //Verify PAC
            if (this.testConfig.IsKileImplemented)
            {
                BaseTestSite.Assert.IsNotNull(tgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");

                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(tgsResponse.TicketEncPart.authorization_data.Elements);
                BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2KPac.");
                
                foreach (var buf in adWin2kPac.Pac.PacInfoBuffers)
                {
                    if (buf is KerbValidationInfo)
                    {
                        KerbValidationInfo kerbValidationInfo =null ;
                        kerbValidationInfo=buf as KerbValidationInfo;
                        uint userId = kerbValidationInfo.NativeKerbValidationInfo.UserId;
                        
                        Adapter.PacHelper.commonUserFields commonUserFields = new Adapter.PacHelper.commonUserFields();
                        if (this.testConfig.LocalRealm.KDC[0].IsWindows)
                        {
                            //Don't use the same user account for ldap querys, it will change the current user account attributes
                            NetworkCredential cred = new NetworkCredential(this.testConfig.LocalRealm.Admin.Username, this.testConfig.LocalRealm.Admin.Password, this.testConfig.LocalRealm.RealmName);
                            commonUserFields = Adapter.PacHelper.GetCommonUserFields(this.testConfig.LocalRealm.RealmName, this.testConfig.LocalRealm.User[1].Username, cred);
                        }
                        BaseTestSite.Assert.AreEqual(commonUserFields.userId, userId, "The UserID field SHOULD match local realm client user sid.");

                    }
                    if (buf is PacClientInfo)
                    {
                        PacClientInfo pacClientInfo = null;
                        pacClientInfo = buf as PacClientInfo;
                        BaseTestSite.Assert.IsNotNull(pacClientInfo, "PAC_CLIENT_INFO is generated.");
                        var authTimeInPac = DtypUtility.ToDateTime(pacClientInfo.NativePacClientInfo.ClientId).ToString("yyyyMMddHHmmss") + "Z";
                        var c = asResponse.EncPart.authtime;
                        BaseTestSite.Assert.AreEqual(
                            asResponse.EncPart.authtime.ToString(),
                            authTimeInPac,
                            "ClientId field is a FILETIME structure in little-endian format that contains the Kerberos initial TGT auth time.");
                        string clientName = new string(pacClientInfo.NativePacClientInfo.Name);
                        BaseTestSite.Assert.AreEqual(
                            pacClientInfo.NativePacClientInfo.Name.Length * sizeof(char),
                            pacClientInfo.NativePacClientInfo.NameLength,
                            "The User NameLength field should match the client user account name length.");
                        BaseTestSite.Assert.AreEqual(
                            this.testConfig.LocalRealm.User[1].Username.ToLower(),
                            clientName.ToLower(),
                            "The User Name field should match the client user account name.");
                    }
                    if (buf is UpnDnsInfo)
                    {
                        UpnDnsInfo upnDnsInfo = null;
                        upnDnsInfo = buf as UpnDnsInfo;
                        BaseTestSite.Assert.IsNotNull(buf, "UPN_DNS_INFO is generated.");
                        BaseTestSite.Assert.AreEqual(upnDnsInfo.Upn.Length * 2,
                            upnDnsInfo.NativeUpnDnsInfo.UpnLength,
                            "The UpnLength field SHOULD be the length of the UPN field, in bytes.");
                        //upnDnsInfo.NativeUpnDnsInfo.UpnOffset;
                        BaseTestSite.Assert.AreEqual(upnDnsInfo.DnsDomain.Length * 2,
                            upnDnsInfo.NativeUpnDnsInfo.DnsDomainNameLength,
                            "The DnsDomainNameLength field SHOULD be the length of the DnsDomainName field, in bytes.");
                        //upnDnsInfo.NativeUpnDnsInfo.DnsDomainNameOffset; 
                        BaseTestSite.Assert.AreEqual(upnDnsInfo.Upn.ToLower(), 
                            this.testConfig.LocalRealm.User[1].Username.ToLower() + "@" + this.testConfig.LocalRealm.RealmName.ToLower(), 
                            "The UPN field should be the user principal name in local realm ");
                        BaseTestSite.Assert.AreEqual(upnDnsInfo.DnsDomain.ToLower(),
                            this.testConfig.LocalRealm.RealmName.ToLower(),
                            "The dnsDomain field should be the local realm name");
                    }
                    if (buf is PacServerSignature)
                    {
                        PacServerSignature serverSignature = null;
                        serverSignature = buf as PacServerSignature;
                        BaseTestSite.Assert.IsNotNull(serverSignature, "Server Signature is generated.");
                    }
                    if(buf is PacKdcSignature)
                    {
                        PacKdcSignature kdcSignature = null;
                        kdcSignature = buf as PacKdcSignature;
                        BaseTestSite.Assert.IsNotNull(kdcSignature, "KDC Signature is generated.");
                    }
                }
            }
        }
    }
}

