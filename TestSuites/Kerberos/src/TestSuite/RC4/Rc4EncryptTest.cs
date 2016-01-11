// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocol.TestSuites.Kerberos.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth;
using Microsoft.Protocols.TestTools.StackSdk.Security.Pac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocol.TestSuites.Kerberos.TestSuite
{
    [TestClass]
    public class Rc4EncryptTest : TraditionTestBase
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

        #region KileTestSuite in RC4

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.FAST)]
        [TestCategory(TestCategories.RC4)]
        [Feature(Feature.FAST | Feature.RC4)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the interactive logon process with FAST when using RC4-HMAC.")]
        public void RC4_InteractiveLogonUseFast()
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

            EncryptionType[] rc4HmacType = new EncryptionType[]
            { 
                EncryptionType.RC4_HMAC
            };

            // Define device principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set device principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);

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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

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

            // Define user principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set user principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);

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
            // Create a "random" key.
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send FAST armored AS request with no pre-authentication padata.");
            var subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password02!", "this is a salt");
            var fastOptions = new Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth.FastOptions(KerberosUtility.ConvertInt2Flags((int)0));
            var apOptions = ApOptions.None;

            Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { new PA_DATA(new KerbInt32((long)PaDataType.PA_FX_FAST), null) });
            client.SendAsRequestWithFast(options, seqOfPaData2, null, subkey, fastOptions, apOptions);
            KerberosKrbError krbError2 = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_PREAUTH_REQUIRED, krbError2.ErrorCode, "Pre-authentication required.");

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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
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
        [TestCategory(TestCategories.RC4)]
        [Feature(Feature.FAST | Feature.RC4)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the interactive logon process with explicit FAST armor in TGS-REQ when using RC4-HMAC.")]
        public void RC4_InteractiveLogonUseExplicitFast()
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

            EncryptionType[] rc4HmacType = new EncryptionType[]
            { 
                EncryptionType.RC4_HMAC
            };

            // Define device principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set device principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);

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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
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
                string.Format("Construct kerberos client using user account: {0}.",
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

            // Define user principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set user principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);

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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_PREAUTH_REQUIRED, krbError2.ErrorCode, "Pre-authentication required.");

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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
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
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.FAST)]
        [TestCategory(TestCategories.RC4)]
        [Feature(Feature.Claim | Feature.FAST | Feature.Kile | Feature.Pac | Feature.RC4)]
        [ApplicationServer(ApplicationServer.Smb2)]
        [Description("This test case is designed to verify the process that a user logons to an SMB server using Kerberos authentication with compound identity when using RC4-HMAC.")]
        public void RC4_NetworkLogonCompoundIdentitySmb2()
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

            EncryptionType[] rc4HmacType = new EncryptionType[]
            { 
                EncryptionType.RC4_HMAC
            };

            // Define device principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set device principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);

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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

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

            // Define user principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set user principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);

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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

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

        #endregion

        #region KileFastTest in RC4

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.FAST)]
        [TestCategory(TestCategories.RC4)]
        [Feature(Feature.FAST | Feature.RC4)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify that KDCs rejects unknown fast armor type encrypted when using RC4-HMAC.")]
        public void RC4_UnsupportedFastArmorType()
        {
            base.Logging();

            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.ClientComputer.NetBiosName,
               this.testConfig.LocalRealm.ClientComputer.Password,
               KerberosAccountType.Device,
               testConfig.LocalRealm.KDC[0].IPAddress,
               testConfig.LocalRealm.KDC[0].Port,
               testConfig.TransportType,
                testConfig.SupportedOid);

            EncryptionType[] rc4HmacType = new EncryptionType[]
            { 
                EncryptionType.RC4_HMAC
            };

            // Define device principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set device principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);

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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

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

            // FAST armored AS_REQ and KRB-ERROR using user principal
            //Create a "random" key.
            var subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password02!", "this is a salt");

            var fastOptions = new Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth.FastOptions(KerberosUtility.ConvertInt2Flags((int)0));
            var apOptions = ApOptions.None;
            string timeStamp2 = KerberosUtility.CurrentKerberosTime.Value;
            PaFxFastReq paFxReq = new PaFxFastReq(null);
            Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { (paFxReq.Data) });

            client.SendAsRequestWithFast(options, seqOfPaData2, null, subkey, fastOptions, apOptions);
            KerberosKrbError krbError2 = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
            BaseTestSite.Assert.AreEqual(krbError2.ErrorCode,
                KRB_ERROR_CODE.KDC_ERR_PREAUTH_REQUIRED,
                "Pre-authentication required.");

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
            client.SendAsRequestWithFast(options, seqOfPaData3, outerSeqPaData, subkey, fastOptions, apOptions, 0);
            KerberosKrbError krbError3 = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
            BaseTestSite.Assert.AreEqual(krbError3.ErrorCode,
                KRB_ERROR_CODE.KDC_ERR_PREAUTH_FAILED,
                "Pre-authentication failed.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.FAST)]
        [TestCategory(TestCategories.RC4)]
        [Feature(Feature.FAST | Feature.RC4)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify that KDCs supports strengthen key encrypted when using RC4-HMAC.")]
        public void RC4_StrengthenKey()
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

            EncryptionType[] rc4HmacType = new EncryptionType[]
            { 
                EncryptionType.RC4_HMAC
            };

            // Define device principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set device principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);

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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
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

            // Define user principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set user principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);

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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
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

            BaseTestSite.Assert.IsNotNull(userKrbTgsRep.Response, "The Response pare of TGS-REP is not null.");
            BaseTestSite.Assert.IsNotNull(userKrbTgsRep.Response.padata, "The Padata of TGS-REP is not null.");

            EncryptionKey strengthenKey = null;
            foreach (PA_DATA paData in userKrbTgsRep.Response.padata.Elements)
            {
                var parsedPaData = PaDataParser.ParseRepPaData(paData);
                if (parsedPaData is PaFxFastRep)
                {
                    var armoredRep = ((PaFxFastRep)parsedPaData).GetArmoredRep();
                    var kerbRep = ((PaFxFastRep)parsedPaData).GetKerberosFastRep(client.Context.FastArmorkey);
                    strengthenKey = kerbRep.FastResponse.strengthen_key;
                }
            }
            BaseTestSite.Assert.IsNotNull(strengthenKey, "Strengthen key field must be set in TGS-REP.");

            BaseTestSite.Assert.IsNotNull(userKrbTgsRep.EncPart, "The encrypted part of TGS-REP is decrypted.");

        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.FAST)]
        [TestCategory(TestCategories.RC4)]
        [Feature(Feature.FAST | Feature.RC4)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify FastKrb_Error when using RC4-HMAC.")]
        public void RC4_FastKrb_Error()
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

            EncryptionType[] rc4HmacType = new EncryptionType[]
            { 
                EncryptionType.RC4_HMAC
            };

            // Define device principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set device principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);

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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
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

            // FAST armored AS_REQ and KRB-ERROR using user principal
            //Create a "random" key.
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send FAST armored AS request with no pre-authentication padata.");
            var subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password02!", "this is a salt");
            var fastOptions = new Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth.FastOptions(KerberosUtility.ConvertInt2Flags((int)0));
            var apOptions = ApOptions.None;

            Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { new PA_DATA(new KerbInt32((long)PaDataType.PA_FX_FAST), null) });
            client.SendAsRequestWithFast(options, seqOfPaData2, null, subkey, fastOptions, apOptions);
            KerberosKrbError krbError2 = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
            BaseTestSite.Assert.AreEqual(krbError2.ErrorCode, KRB_ERROR_CODE.KDC_ERR_PREAUTH_REQUIRED, "Pre-authentication required.");

            // FAST armored AS_REQ and AS_REP using user principal
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with PaEncryptedChallenge.");
            var userKey = KerberosUtility.MakeKey(
                client.Context.SelectedEType,
                client.Context.CName.Password,
                client.Context.CName.Salt);

            var invalidKey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Invalid", "this is a salt");
            PaEncryptedChallenge paEncTimeStamp3 = new PaEncryptedChallenge(
                client.Context.SelectedEType,
                KerberosUtility.CurrentKerberosTime.ToString(),
                0,
                invalidKey,
                userKey);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> outerSeqPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paPacRequest.Data });
            Asn1SequenceOf<PA_DATA> seqOfPaData3 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp3.Data });
            client.SendAsRequestWithFast(options, seqOfPaData3, outerSeqPaData, subkey, fastOptions, apOptions);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve AS response.");
            KerberosKrbError error = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

            BaseTestSite.Assert.IsNotNull(error.KrbError.e_data, "E_data is not null");
            Asn1SequenceOf<PA_DATA> seqOfPadata = new Asn1SequenceOf<PA_DATA>();
            seqOfPadata.BerDecode(new Asn1DecodingBuffer(error.KrbError.e_data.ByteArrayValue));
            var padataCount = seqOfPadata.Elements.Length;
            PaFxFastRep paFxFastRep = null;
            for (int i = 0; i < padataCount; i++)
            {
                var padata = PaDataParser.ParseRepPaData(seqOfPadata.Elements[i]);
                //Fix me: PaETypeInfo is also possible
                if (padata is PaFxFastRep)
                {
                    paFxFastRep = padata as PaFxFastRep;
                }
            }
            BaseTestSite.Assert.IsNotNull(paFxFastRep, "KDC must reply a PA-FX-Fast response.");
            PaFxError paFxError = null;
            var armoredRep = paFxFastRep.GetKerberosFastRep(client.Context.FastArmorkey);
            foreach (var padata in armoredRep.PaData)
            {
                if (padata is PaFxError)
                {
                    paFxError = padata as PaFxError;
                    break;
                }
            }
            BaseTestSite.Assert.IsNotNull(paFxError, "KDC must include a PA-FX-ERROR padata.");
            BaseTestSite.Assert.IsNull(paFxError.KrbError.KrbError.e_data, "E_data is null.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.FAST)]
        [TestCategory(TestCategories.RC4)]
        [Feature(Feature.FAST | Feature.RC4)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify that KDCs supports KrbFastFinished in AS-REP when using RC4-HMAC.")]
        public void RC4_KrbFastFinishedAsRep()
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

            EncryptionType[] rc4HmacType = new EncryptionType[]
            { 
                EncryptionType.RC4_HMAC
            };

            // Define device principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set device principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);

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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                string.Format("The type of AS-REP encrypted part is {0}.", asResponse.EncPart.GetType().Name));

            // Switch to user principal
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

            // Define user principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set user principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);

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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
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
            client.SendAsRequestWithFast(options, seqOfPaData3, null, subkey, fastOptions, apOptions);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve AS response.");
            KerberosAsResponse userKrbAsRep = client.ExpectAsResponse();
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
            BaseTestSite.Assert.IsNotNull(userKrbAsRep.Response.ticket, "AS response should contain a TGT.");
            BaseTestSite.Assert.IsNotNull(userKrbAsRep.EncPart, "The encrypted part of AS response is decrypted.");
            BaseTestSite.Assert.IsNotNull(userKrbAsRep.EncPart.key, "AS response should contain a session key.");
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                string.Format("The type of AS-REP encrypted part is {0}.", userKrbAsRep.EncPart.GetType().Name));
            //Verify KrbFastFinished
            KrbFastFinished fastFinishedAs = null;
            BaseTestSite.Assert.IsNotNull(userKrbAsRep.Response.padata, "The padata of AS-REP is not null.");
            BaseTestSite.Assert.IsNotNull(userKrbAsRep.Response.padata.Elements, "The padata of AS-REP is not empty.");

            foreach (PA_DATA paData in userKrbAsRep.Response.padata.Elements)
            {
                var parsedPaData = PaDataParser.ParseRepPaData(paData);
                if (parsedPaData is PaFxFastRep)
                {
                    var armoredRep = ((PaFxFastRep)parsedPaData).GetArmoredRep();
                    var kerbFastRep = ((PaFxFastRep)parsedPaData).GetKerberosFastRep(client.Context.FastArmorkey);
                    fastFinishedAs = kerbFastRep.FastResponse.finished;
                    break;
                }
            }
            BaseTestSite.Assert.IsNotNull(fastFinishedAs, "The finished field contains a KrbFastFinished structure.");
            Asn1BerEncodingBuffer buf = new Asn1BerEncodingBuffer();
            userKrbAsRep.Response.ticket.BerEncode(buf);
            Checksum ticketChecksum = new Checksum(new KerbInt32((long)KerberosUtility.GetChecksumType(client.Context.SelectedEType)), new Asn1OctetString(KerberosUtility.GetChecksum(
                client.Context.FastArmorkey.keyvalue.ByteArrayValue,
                buf.Data,
                (int)KeyUsageNumber.FAST_FINISHED,
                KerberosUtility.GetChecksumType(client.Context.SelectedEType))));
            BaseTestSite.Assert.IsTrue(
            KerberosUtility.CompareChecksum(fastFinishedAs.ticket_checksum, ticketChecksum),
            "The ticket checksum is correct.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.FAST)]
        [TestCategory(TestCategories.RC4)]
        [Feature(Feature.FAST | Feature.RC4)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify that KDCs supports KrbFastFinished in TGS-REP using RC4-HMAC.")]
        public void RC4_KrbFastFinishedTgsRep()
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

            EncryptionType[] rc4HmacType = new EncryptionType[]
            { 
                EncryptionType.RC4_HMAC
            };

            // Define device principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set device principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);

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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                string.Format("The type of AS-REP encrypted part is {0}.", asResponse.EncPart.GetType().Name));

            // Switch to user principal
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

            // Define user principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set user principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);

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

            Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { new PA_DATA(new KerbInt32((long)PaDataType.PA_FX_FAST), null) });
            client.SendAsRequestWithFast(options, seqOfPaData2, null, subkey, fastOptions, apOptions);
            KerberosKrbError krbError2 = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
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
            client.SendAsRequestWithFast(options, seqOfPaData3, null, subkey, fastOptions, apOptions);
            KerberosAsResponse userKrbAsRep = client.ExpectAsResponse();
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                string.Format("The type of AS-REP encrypted part is {0}.", userKrbAsRep.EncPart.GetType().Name));

            // FAST armored TGS_REQ and TGS_REP using user principal
            subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password03!", "this is a salt");
            client.SendTgsRequestWithFast(testConfig.LocalRealm.ClientComputer.DefaultServiceName, options, null, null, subkey, fastOptions, apOptions);
            KerberosTgsResponse userKrbTgsRep = client.ExpectTgsResponse(KeyUsageNumber.TGS_REP_encrypted_part_subkey);
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
            BaseTestSite.Assert.IsNotNull(userKrbTgsRep.EncPart, "The encrypted part of TGS-REP is decrypted.");
            //Verify KrbFastFinished.
            KrbFastFinished fastFinishedTgs = null;
            BaseTestSite.Assert.IsNotNull(userKrbAsRep.Response.padata, "The padata of AS-REP is not null.");
            BaseTestSite.Assert.IsNotNull(userKrbAsRep.Response.padata.Elements, "The padata of AS-REP is not empty.");

            foreach (PA_DATA paData in userKrbTgsRep.Response.padata.Elements)
            {
                var parsedPaData = PaDataParser.ParseRepPaData(paData);
                if (parsedPaData is PaFxFastRep)
                {
                    var armoredRep = ((PaFxFastRep)parsedPaData).GetArmoredRep();
                    var kerbFastRep = ((PaFxFastRep)parsedPaData).GetKerberosFastRep(client.Context.FastArmorkey);
                    fastFinishedTgs = kerbFastRep.FastResponse.finished;
                    break;
                }
            }
            BaseTestSite.Assert.IsNotNull(fastFinishedTgs, "The finished field contains a KrbFastFinished structure.");
            Asn1BerEncodingBuffer buf2 = new Asn1BerEncodingBuffer();
            userKrbTgsRep.Response.ticket.BerEncode(buf2);
            Checksum ticketChecksumTgs = new Checksum(new KerbInt32((long)KerberosUtility.GetChecksumType(client.Context.SelectedEType)), new Asn1OctetString(KerberosUtility.GetChecksum(
                client.Context.FastArmorkey.keyvalue.ByteArrayValue,
                buf2.Data,
                (int)KeyUsageNumber.FAST_FINISHED,
                KerberosUtility.GetChecksumType(client.Context.SelectedEType))));
            BaseTestSite.Assert.IsTrue(
                KerberosUtility.CompareChecksum(fastFinishedTgs.ticket_checksum, ticketChecksumTgs),
                "The ticket checksum is correct.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.FAST)]
        [TestCategory(TestCategories.RC4)]
        [Feature(Feature.FAST | Feature.RC4)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify that KDCs rejects unarmored TGS-REQ if Ad-fx-fast-used is in the authenticator when using RC4-HMAC.")]
        public void RC4_AdFxFastUsedInAuthenticator()
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

            EncryptionType[] rc4HmacType = new EncryptionType[]
            { 
                EncryptionType.RC4_HMAC
            };

            // Define device principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set device principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);

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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                string.Format("The type of AS-REP encrypted part is {0}.", asResponse.EncPart.GetType().Name));

            // Switch to user principal
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

            // Define user principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set user principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);

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

            Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { new PA_DATA(new KerbInt32((long)PaDataType.PA_FX_FAST), null) });
            client.SendAsRequestWithFast(options, seqOfPaData2, null, subkey, fastOptions, apOptions);
            KerberosKrbError krbError2 = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(krbError2.ErrorCode, KRB_ERROR_CODE.KDC_ERR_PREAUTH_REQUIRED, "Pre-authentication required.");
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

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
            client.SendAsRequestWithFast(options, seqOfPaData3, null, subkey, fastOptions, apOptions);
            KerberosAsResponse userKrbAsRep = client.ExpectAsResponse();
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
            BaseTestSite.Assert.IsNotNull(userKrbAsRep.Response.ticket, "AS response should contain a TGT.");
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                string.Format("The type of AS-REP encrypted part is {0}.", userKrbAsRep.EncPart.GetType().Name));

            // FAST armored TGS_REQ and TGS_REP using user principal
            subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password03!", "this is a salt");
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send unarmored TGS request with AD-fx-fast-used.");
            AdFxFastUsed adFxFastUsed = new AdFxFastUsed();
            AuthorizationData authData = new AuthorizationData(new AuthorizationDataElement[] { adFxFastUsed.AuthDataElement });
            client.SendTgsRequest(testConfig.LocalRealm.ClientComputer.DefaultServiceName, options, null, null, authData);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Receive TGS Error, KDC MUST reject the request.");
            KerberosKrbError krbError = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
        }

        #endregion

        #region KileHttpApTest in RC4

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.HttpAp)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.FAST)]
        [TestCategory(TestCategories.RC4)]
        [Feature(Feature.Kile | Feature.Pac | Feature.Claim | Feature.FAST | Feature.RC4)]
        [ApplicationServer(ApplicationServer.Http)]
        [Description("This test case is designed to verify if client can request ticket via FAST from KDC when using RC4-HMAC.")]
        public void RC4_UsingFAST_Http()
        {
            base.Logging();

            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName,
               this.testConfig.LocalRealm.ClientComputer.NetBiosName,
               this.testConfig.LocalRealm.ClientComputer.Password,
               KerberosAccountType.Device,
               testConfig.LocalRealm.KDC[0].IPAddress,
               testConfig.LocalRealm.KDC[0].Port,
               testConfig.TransportType,
               testConfig.SupportedOid);

            EncryptionType[] rc4HmacType = new EncryptionType[]
            { 
                EncryptionType.RC4_HMAC
            };

            // Define device principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set device principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);


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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

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

            // Define user principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set user principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);

            // FAST armored AS_REQ and KRB-ERROR using user principal
            //Create a "random" key.
            var subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password02!", "this is a salt");

            var fastOptions = new Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth.FastOptions(KerberosUtility.ConvertInt2Flags((int)0));
            var apOptions = ApOptions.None;
            string timeStamp2 = KerberosUtility.CurrentKerberosTime.Value;
            PaFxFastReq paFxReq = new PaFxFastReq(null);
            Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { (paFxReq.Data) });

            client.SendAsRequestWithFast(options, seqOfPaData2, null, subkey, fastOptions, apOptions);
            KerberosKrbError krbError2 = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_PREAUTH_REQUIRED, krbError2.ErrorCode, "Pre-authentication required.");

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
            PaPacRequest paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> seqOfPaData3 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp3.Data, paPacRequest.Data, paPacOptions.Data });

            client.SendAsRequestWithFast(options, seqOfPaData3, null, subkey, fastOptions, apOptions);
            KerberosAsResponse userKrbAsRep = client.ExpectAsResponse();
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

            // FAST armored TGS_REQ and TGS_REP using user principal
            subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password03!", "this is a salt");
            client.SendTgsRequestWithFast(testConfig.LocalRealm.WebServer[0].HttpServiceName, options, null, null, subkey, fastOptions, apOptions);
            KerberosTgsResponse userKrbTgsRep = client.
                ExpectTgsResponse(KeyUsageNumber.TGS_REP_encrypted_part_subkey);
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

            AuthorizationData data = null;
            subkey = KerberosUtility.GenerateKey(client.Context.SessionKey);
            byte[] token = client.CreateGssApiToken(ApOptions.MutualRequired,
                data,
                subkey,
                ChecksumFlags.GSS_C_MUTUAL_FLAG | ChecksumFlags.GSS_C_INTEG_FLAG);

            KerberosApResponse apRep = client.GetApResponseFromToken(SendAndRecieveHttpAp(this.testConfig.LocalRealm.WebServer[0], token));
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.HttpAp)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.Claim)]
        [TestCategory(TestCategories.RC4)]
        [Feature(Feature.Kile | Feature.Pac | Feature.Claim | Feature.RC4)]
        [ApplicationServer(ApplicationServer.Http)]
        [Description("This test case is designed to verify if client can request ticket with device claim from KDC when using RC4-HMAC.")]
        public void RC4_RequestDeviceClaim_Http()
        {
            base.Logging();

            client = new KerberosTestClient(
                this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.ClientComputer.NetBiosName,
                this.testConfig.LocalRealm.ClientComputer.Password,
                KerberosAccountType.Device, testConfig.LocalRealm.KDC[0].IPAddress,
                testConfig.LocalRealm.KDC[0].Port,
                testConfig.TransportType,
                testConfig.SupportedOid,
                testConfig.LocalRealm.ClientComputer.AccountSalt);

            EncryptionType[] rc4HmacType = new EncryptionType[]
            { 
                EncryptionType.RC4_HMAC
            };

            // Define device principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set device principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);

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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

            // AS_REQ and AS_REP using device principal
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data });
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();

            // Switch to user principal
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName, this.testConfig.LocalRealm.User[2].Username, this.testConfig.LocalRealm.User[2].Password, KerberosAccountType.User, client.Context.Ticket, client.Context.SessionKey,
                testConfig.LocalRealm.KDC[0].IPAddress, testConfig.LocalRealm.KDC[0].Port, testConfig.TransportType,
                testConfig.SupportedOid);
            // Define user principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set user principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);

            // FAST armored AS_REQ and KRB-ERROR using user principal
            //Create a "random" key.
            var subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password02!", "this is a salt");

            var fastOptions = new Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth.FastOptions(KerberosUtility.ConvertInt2Flags((int)0));
            var apOptions = ApOptions.None;
            string timeStamp2 = KerberosUtility.CurrentKerberosTime.Value;
            PaFxFastReq paFxFastReq = new PaFxFastReq(null);
            Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { (paFxFastReq.Data) });

            client.SendAsRequestWithFast(options, seqOfPaData2, null, subkey, fastOptions, apOptions);
            KerberosKrbError krbError2 = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_PREAUTH_REQUIRED, krbError2.ErrorCode, "Pre-authentication required.");

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

            PaPacRequest paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> seqOfPaData3 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp3.Data, paPacRequest.Data, paPacOptions.Data });

            client.SendAsRequestWithFast(options, seqOfPaData3, null, subkey, fastOptions, apOptions);
            KerberosAsResponse userKrbAsRep = client.ExpectAsResponse();
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
            if (testConfig.IsClaimSupported)
            {
                PaSupportedEncTypes paSupportedEncTypes = null;
                foreach (var padata in userKrbAsRep.EncPart.pa_datas.Elements)
                {
                    var parsedPadata = PaDataParser.ParseRepPaData(padata);
                    if (parsedPadata is PaSupportedEncTypes)
                        paSupportedEncTypes = parsedPadata as PaSupportedEncTypes;
                }

                BaseTestSite.Assert.IsNotNull(paSupportedEncTypes, "The encrypted padata of AS-REP contains PA_SUPPORTED_ENCTYPES.");
                BaseTestSite.Assert.IsTrue(
                    paSupportedEncTypes.SupportedEncTypes.HasFlag(SupportedEncryptionTypes.Claims_Supported),
                    "Claims is supported.");
                BaseTestSite.Assert.IsTrue(
                    paSupportedEncTypes.SupportedEncTypes.HasFlag(SupportedEncryptionTypes.FAST_Supported),
                    "FAST is supported.");
            }
            // FAST armored TGS_REQ and TGS_REP using user principal
            subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password03!", "this is a salt");

            client.Context.ArmorSessionKey = client.Context.Ticket.SessionKey;
            client.Context.ArmorTicket = client.Context.Ticket;

            client.SendTgsRequestWithExplicitFast(testConfig.LocalRealm.WebServer[0].HttpServiceName, options, null, null, subkey, fastOptions, apOptions);
            KerberosTgsResponse userKrbTgsRep = client.ExpectTgsResponse(KeyUsageNumber.TGS_REP_encrypted_part_subkey);
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
            if (testConfig.IsClaimSupported)
            {
                PaSupportedEncTypes paSupportedEncTypes = null;
                BaseTestSite.Assert.IsNotNull(asResponse.EncPart, "The encrypted part of AS-REP is decrypted.");
                BaseTestSite.Assert.IsNotNull(asResponse.EncPart.pa_datas, "The encrypted padata is not null.");
                foreach (var padata in userKrbTgsRep.EncPart.pa_datas.Elements)
                {
                    var parsedPadata = PaDataParser.ParseRepPaData(padata);
                    if (parsedPadata is PaSupportedEncTypes)
                        paSupportedEncTypes = parsedPadata as PaSupportedEncTypes;
                }
                BaseTestSite.Assert.IsNotNull(paSupportedEncTypes, "The encrypted padata of AS-REP contains PA_SUPPORTED_ENCTYPES.");
                BaseTestSite.Assert.IsTrue(
                    paSupportedEncTypes.SupportedEncTypes.HasFlag(SupportedEncryptionTypes.CompoundIdentity_Supported),
                    "Compound identity is supported.");
            }

            EncryptionKey key = testConfig.QueryKey(this.testConfig.LocalRealm.WebServer[0].HttpServiceName, this.testConfig.LocalRealm.RealmName, this.client.Context.SelectedEType);
            userKrbTgsRep.DecryptTicket(key);

            //userKrbTgsRep.DecryptTicket(testConfig.LocalRealm.WebServer[0].Password, testConfig.LocalRealm.WebServer[0].ServiceSalt);

            //Verify PAC
            if (testConfig.IsKileImplemented)
            {
                BaseTestSite.Assert.IsNotNull(userKrbTgsRep.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(userKrbTgsRep.TicketEncPart.authorization_data.Elements);
                BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2KPac.");

                DeviceClaimsInfo deviceClaimsInfo = null;
                foreach (var buf in adWin2kPac.Pac.PacInfoBuffers)
                {
                    if (buf is DeviceClaimsInfo)
                    {
                        deviceClaimsInfo = buf as DeviceClaimsInfo;
                        break;
                    }
                }
                BaseTestSite.Assert.IsNotNull(deviceClaimsInfo, "PAC_DEVICE_INFO is generated.");
            }

            AuthorizationData data = null;
            subkey = KerberosUtility.GenerateKey(client.Context.SessionKey);
            byte[] token = client.CreateGssApiToken(ApOptions.MutualRequired,
                data,
                subkey,
                ChecksumFlags.GSS_C_MUTUAL_FLAG | ChecksumFlags.GSS_C_INTEG_FLAG);

            KerberosApResponse apRep = client.GetApResponseFromToken(SendAndRecieveHttpAp(this.testConfig.LocalRealm.WebServer[0], token));
        }

        #endregion

        #region KileSmb2ApTest in RC4

        [TestMethod]
        [Priority(2)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.FAST)]
        [TestCategory(TestCategories.RC4)]
        [Feature(Feature.Kile | Feature.Pac | Feature.Claim | Feature.FAST | Feature.RC4)]
        [ApplicationServer(ApplicationServer.Smb2)]
        [Description("This test case is designed to verify if client can request ticket via FAST from KDC when using RC4-HMAC.")]
        public void RC4_UsingFAST_Smb2()
        {
            base.Logging();

            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.ClientComputer.NetBiosName,
               this.testConfig.LocalRealm.ClientComputer.Password,
               KerberosAccountType.Device,
               testConfig.LocalRealm.KDC[0].IPAddress,
               testConfig.LocalRealm.KDC[0].Port,
               testConfig.TransportType,
                testConfig.SupportedOid);

            EncryptionType[] rc4HmacType = new EncryptionType[]
            { 
                EncryptionType.RC4_HMAC
            };

            // Define device principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set device principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);

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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

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

            // Define user principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set user principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);

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
            PaFxFastReq paFxReq = new PaFxFastReq(null);
            Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { (paFxReq.Data) });

            client.SendAsRequestWithFast(options, seqOfPaData2, null, subkey, fastOptions, apOptions);
            KerberosKrbError krbError2 = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(krbError2.ErrorCode,
                KRB_ERROR_CODE.KDC_ERR_PREAUTH_REQUIRED,
                "Pre-authentication required.");

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

            PaPacRequest paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> seqOfPaData3 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp3.Data, paPacRequest.Data, paPacOptions.Data });

            client.SendAsRequestWithFast(options, seqOfPaData3, null, subkey, fastOptions, apOptions);
            KerberosAsResponse userKrbAsRep = client.ExpectAsResponse();
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

            // FAST armored TGS_REQ and TGS_REP using user principal
            subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password03!", "this is a salt");
            client.SendTgsRequestWithFast(testConfig.LocalRealm.FileServer[0].Smb2ServiceName, options, null, null, subkey, fastOptions, apOptions);
            KerberosTgsResponse userKrbTgsRep = client.ExpectTgsResponse(KeyUsageNumber.TGS_REP_encrypted_part_subkey);
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

            AuthorizationData data = null;
            subkey = KerberosUtility.GenerateKey(client.Context.SessionKey);
            byte[] token = client.CreateGssApiToken(ApOptions.MutualRequired,
                data,
                subkey,
                ChecksumFlags.GSS_C_MUTUAL_FLAG | ChecksumFlags.GSS_C_INTEG_FLAG);

            KerberosApResponse apRep = client.GetApResponseFromToken(SendAndRecieveSmb2Ap(this.testConfig.LocalRealm.FileServer[0], token));
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.Claim)]
        [TestCategory(TestCategories.RC4)]
        [Feature(Feature.Kile | Feature.Pac | Feature.Claim | Feature.RC4)]
        [ApplicationServer(ApplicationServer.Smb2)]
        [Description("This test case is designed to verify if client can request ticket with device claim from KDC when using RC4-HMAC.")]
        public void RC4_RequestDeviceClaim_Smb2()
        {
            base.Logging();

            client = new KerberosTestClient(
                this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.ClientComputer.NetBiosName,
               this.testConfig.LocalRealm.ClientComputer.Password,
               KerberosAccountType.Device, testConfig.LocalRealm.KDC[0].IPAddress,
               testConfig.LocalRealm.KDC[0].Port,
               testConfig.TransportType,
                testConfig.SupportedOid,
               testConfig.LocalRealm.ClientComputer.AccountSalt);

            EncryptionType[] rc4HmacType = new EncryptionType[]
            { 
                EncryptionType.RC4_HMAC
            };

            // Define device principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set device principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);

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

            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

            // AS_REQ and AS_REP using device principal
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data });
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

            // Switch to user principal
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName, this.testConfig.LocalRealm.User[2].Username, this.testConfig.LocalRealm.User[2].Password, KerberosAccountType.User, client.Context.Ticket, client.Context.SessionKey,
                testConfig.LocalRealm.KDC[0].IPAddress, testConfig.LocalRealm.KDC[0].Port, testConfig.TransportType,
                testConfig.SupportedOid);

            // Define user principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set user principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);

            // FAST armored AS_REQ and KRB-ERROR using user principal
            //Create a "random" key.
            var subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password02!", "this is a salt");

            var fastOptions = new Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth.FastOptions(KerberosUtility.ConvertInt2Flags((int)0));
            var apOptions = ApOptions.None;
            string timeStamp2 = KerberosUtility.CurrentKerberosTime.Value;
            PaFxFastReq paFxFastReq = new PaFxFastReq(null);
            Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { (paFxFastReq.Data) });

            client.SendAsRequestWithFast(options, seqOfPaData2, null, subkey, fastOptions, apOptions);
            KerberosKrbError krbError2 = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_PREAUTH_REQUIRED, krbError2.ErrorCode, "Pre-authentication required.");

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

            PaPacRequest paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> seqOfPaData3 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp3.Data, paPacRequest.Data, paPacOptions.Data });

            client.SendAsRequestWithFast(options, seqOfPaData3, null, subkey, fastOptions, apOptions);
            KerberosAsResponse userKrbAsRep = client.ExpectAsResponse();
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
            if (testConfig.IsClaimSupported)
            {
                PaSupportedEncTypes paSupportedEncTypes = null;
                foreach (var padata in userKrbAsRep.EncPart.pa_datas.Elements)
                {
                    var parsedPadata = PaDataParser.ParseRepPaData(padata);
                    if (parsedPadata is PaSupportedEncTypes)
                        paSupportedEncTypes = parsedPadata as PaSupportedEncTypes;
                }

                BaseTestSite.Assert.IsNotNull(paSupportedEncTypes, "The encrypted padata of AS-REP contains PA_SUPPORTED_ENCTYPES.");
                BaseTestSite.Assert.IsTrue(
                    paSupportedEncTypes.SupportedEncTypes.HasFlag(SupportedEncryptionTypes.Claims_Supported),
                    "Claims is supported.");
                BaseTestSite.Assert.IsTrue(
                    paSupportedEncTypes.SupportedEncTypes.HasFlag(SupportedEncryptionTypes.FAST_Supported),
                    "FAST is supported.");
            }
            // FAST armored TGS_REQ and TGS_REP using user principal
            subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password03!", "this is a salt");

            client.Context.ArmorSessionKey = client.Context.Ticket.SessionKey;
            client.Context.ArmorTicket = client.Context.Ticket;

            client.SendTgsRequestWithExplicitFast(testConfig.LocalRealm.FileServer[0].Smb2ServiceName, options, null, null, subkey, fastOptions, apOptions);
            KerberosTgsResponse userKrbTgsRep = client.ExpectTgsResponse(KeyUsageNumber.TGS_REP_encrypted_part_subkey);
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
            if (testConfig.IsClaimSupported)
            {
                PaSupportedEncTypes paSupportedEncTypes = null;
                BaseTestSite.Assert.IsNotNull(asResponse.EncPart, "The encrypted part of AS-REP is decrypted.");
                BaseTestSite.Assert.IsNotNull(asResponse.EncPart.pa_datas, "The encrypted padata is not null.");
                foreach (var padata in userKrbTgsRep.EncPart.pa_datas.Elements)
                {
                    var parsedPadata = PaDataParser.ParseRepPaData(padata);
                    if (parsedPadata is PaSupportedEncTypes)
                        paSupportedEncTypes = parsedPadata as PaSupportedEncTypes;
                }
                BaseTestSite.Assert.IsNotNull(paSupportedEncTypes, "The encrypted padata of AS-REP contains PA_SUPPORTED_ENCTYPES.");
                BaseTestSite.Assert.IsTrue(
                    paSupportedEncTypes.SupportedEncTypes.HasFlag(SupportedEncryptionTypes.CompoundIdentity_Supported),
                    "Compound identity is supported.");
            }

            EncryptionKey key = testConfig.QueryKey(this.testConfig.LocalRealm.FileServer[0].Smb2ServiceName, this.testConfig.LocalRealm.RealmName, this.client.Context.SelectedEType);
            userKrbTgsRep.DecryptTicket(key);

            //Verify PAC
            if (testConfig.IsKileImplemented)
            {
                BaseTestSite.Assert.IsNotNull(userKrbTgsRep.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(userKrbTgsRep.TicketEncPart.authorization_data.Elements);
                BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2KPac.");

                DeviceClaimsInfo deviceClaimsInfo = null;
                foreach (var buf in adWin2kPac.Pac.PacInfoBuffers)
                {
                    if (buf is DeviceClaimsInfo)
                    {
                        deviceClaimsInfo = buf as DeviceClaimsInfo;
                        break;
                    }
                }
                BaseTestSite.Assert.IsNotNull(deviceClaimsInfo, "PAC_DEVICE_INFO is generated.");
            }

            AuthorizationData data = null;
            subkey = KerberosUtility.GenerateKey(client.Context.SessionKey);
            byte[] token = client.CreateGssApiToken(ApOptions.MutualRequired,
                data,
                subkey,
                ChecksumFlags.GSS_C_MUTUAL_FLAG | ChecksumFlags.GSS_C_INTEG_FLAG);

            KerberosApResponse apRep = client.GetApResponseFromToken(SendAndRecieveSmb2Ap(this.testConfig.LocalRealm.FileServer[0], token));
        }

        #endregion

        #region PacTestSuite in RC4

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.FAST)]
        [TestCategory(TestCategories.RC4)]
        [Feature(Feature.Kile | Feature.Pac | Feature.Claim | Feature.FAST | Feature.RC4)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the PAC_DEVICE_INFO in PAC when using RC4-HMAC.")]
        public void RC4_PAC_DEVICE_INFO()
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

            EncryptionType[] rc4HmacType = new EncryptionType[]
            { 
                EncryptionType.RC4_HMAC
            };

            // Define device principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set device principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);

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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

            // AS_REQ and AS_REP using device principal
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data });
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

            // Switch to user principal
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName, this.testConfig.LocalRealm.User[2].Username, this.testConfig.LocalRealm.User[2].Password, KerberosAccountType.User, client.Context.Ticket, client.Context.SessionKey,
                testConfig.LocalRealm.KDC[0].IPAddress, testConfig.LocalRealm.KDC[0].Port, testConfig.TransportType,
                testConfig.SupportedOid);

            // Define user principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set user principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);

            // FAST armored AS_REQ and KRB-ERROR using user principal
            //Create a "random" key.
            var subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password02!", "this is a salt");

            var fastOptions = new Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth.FastOptions(KerberosUtility.ConvertInt2Flags((int)0));
            var apOptions = ApOptions.None;
            PaFxFastReq paFxFastReq = new PaFxFastReq(null);
            Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { (paFxFastReq.Data) });

            client.SendAsRequestWithFast(options, seqOfPaData2, null, subkey, fastOptions, apOptions);
            KerberosKrbError krbError2 = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_PREAUTH_REQUIRED, krbError2.ErrorCode, "Pre-authentication required.");

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

            PaPacRequest paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> seqOfPaData3 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp3.Data, paPacRequest.Data, paPacOptions.Data });

            client.SendAsRequestWithFast(options, seqOfPaData3, null, subkey, fastOptions, apOptions);
            KerberosAsResponse userKrbAsRep = client.ExpectAsResponse();
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
            if (testConfig.IsClaimSupported)
            {
                PaSupportedEncTypes paSupportedEncTypes = null;
                BaseTestSite.Assert.IsNotNull(userKrbAsRep.EncPart.pa_datas, "The encrypted padata of AS-REP is not null.");
                foreach (var padata in userKrbAsRep.EncPart.pa_datas.Elements)
                {
                    var parsedPadata = PaDataParser.ParseRepPaData(padata);
                    if (parsedPadata is PaSupportedEncTypes)
                        paSupportedEncTypes = parsedPadata as PaSupportedEncTypes;
                }

                BaseTestSite.Assert.IsNotNull(paSupportedEncTypes, "The encrypted padata of AS-REP contains PA_SUPPORTED_ENCTYPES.");
                BaseTestSite.Assert.IsTrue(
                    paSupportedEncTypes.SupportedEncTypes.HasFlag(SupportedEncryptionTypes.Claims_Supported),
                    "Claims is supported.");
                BaseTestSite.Assert.IsTrue(
                    paSupportedEncTypes.SupportedEncTypes.HasFlag(SupportedEncryptionTypes.FAST_Supported),
                    "FAST is supported.");
            }
            // FAST armored TGS_REQ and TGS_REP using user principal
            subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password03!", "this is a salt");

            client.Context.ArmorSessionKey = client.Context.Ticket.SessionKey;
            client.Context.ArmorTicket = client.Context.Ticket;

            client.SendTgsRequestWithExplicitFast(testConfig.LocalRealm.FileServer[0].Smb2ServiceName, options, null, null, subkey, fastOptions, apOptions);
            KerberosTgsResponse userKrbTgsRep = client.ExpectTgsResponse(KeyUsageNumber.TGS_REP_encrypted_part_subkey);
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
            if (testConfig.IsClaimSupported)
            {
                PaSupportedEncTypes paSupportedEncTypes = null;
                BaseTestSite.Assert.IsNotNull(asResponse.EncPart, "The encrypted part of AS-REP is decrypted.");
                BaseTestSite.Assert.IsNotNull(asResponse.EncPart.pa_datas, "The encrypted padata is not null.");
                foreach (var padata in userKrbTgsRep.EncPart.pa_datas.Elements)
                {
                    var parsedPadata = PaDataParser.ParseRepPaData(padata);
                    if (parsedPadata is PaSupportedEncTypes)
                        paSupportedEncTypes = parsedPadata as PaSupportedEncTypes;
                }
                BaseTestSite.Assert.IsNotNull(paSupportedEncTypes, "The encrypted padata of AS-REP contains PA_SUPPORTED_ENCTYPES.");
                BaseTestSite.Assert.IsTrue(
                    paSupportedEncTypes.SupportedEncTypes.HasFlag(SupportedEncryptionTypes.CompoundIdentity_Supported),
                    "Compound identity is supported.");

                EncryptionKey key = testConfig.QueryKey(this.testConfig.LocalRealm.FileServer[0].Smb2ServiceName, this.testConfig.LocalRealm.RealmName, this.client.Context.SelectedEType);
                userKrbTgsRep.DecryptTicket(key);

                //userKrbTgsRep.DecryptTicket(testConfig.LocalRealm.FileServer[0].Password, testConfig.LocalRealm.FileServer[0].ServiceSalt);

                //Verify PAC
                BaseTestSite.Assert.IsNotNull(userKrbTgsRep.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(userKrbTgsRep.TicketEncPart.authorization_data.Elements);
                BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2KPac.");

                PacDeviceInfo pacDeviceInfo = null;
                foreach (var buf in adWin2kPac.Pac.PacInfoBuffers)
                {
                    if (buf is PacDeviceInfo)
                    {
                        pacDeviceInfo = buf as PacDeviceInfo;
                        break;
                    }
                }
                BaseTestSite.Assert.IsNotNull(pacDeviceInfo, "PAC_DEVICE_INFO is generated.");
            }
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.FAST)]
        [TestCategory(TestCategories.RC4)]
        [Feature(Feature.Kile | Feature.Pac | Feature.Claim | Feature.FAST | Feature.RC4)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the PAC_DEVICE_CLAIMS_INFO in PAC when using RC4-HMAC.")]
        public void RC4_PAC_DEVICE_CLAIMS_INFO()
        {
            base.Logging();

            client = new KerberosTestClient(
                this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.ClientComputer.NetBiosName,
               this.testConfig.LocalRealm.ClientComputer.Password,
               KerberosAccountType.Device, testConfig.LocalRealm.KDC[0].IPAddress,
               testConfig.LocalRealm.KDC[0].Port,
               testConfig.TransportType,
                testConfig.SupportedOid,
               testConfig.LocalRealm.ClientComputer.AccountSalt);

            EncryptionType[] rc4HmacType = new EncryptionType[]
            { 
                EncryptionType.RC4_HMAC
            };

            // Define device principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set device principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);

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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

            // AS_REQ and AS_REP using device principal
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data });
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

            // Switch to user principal
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName, this.testConfig.LocalRealm.User[2].Username, this.testConfig.LocalRealm.User[2].Password, KerberosAccountType.User, client.Context.Ticket, client.Context.SessionKey,
                testConfig.LocalRealm.KDC[0].IPAddress, testConfig.LocalRealm.KDC[0].Port, testConfig.TransportType,
                testConfig.SupportedOid);

            // Define user principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set user principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);

            // FAST armored AS_REQ and KRB-ERROR using user principal
            //Create a "random" key.
            var subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password02!", "this is a salt");

            var fastOptions = new Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth.FastOptions(KerberosUtility.ConvertInt2Flags((int)0));
            var apOptions = ApOptions.None;
            string timeStamp2 = KerberosUtility.CurrentKerberosTime.Value;
            PaFxFastReq paFxFastReq = new PaFxFastReq(null);
            Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { (paFxFastReq.Data) });

            client.SendAsRequestWithFast(options, seqOfPaData2, null, subkey, fastOptions, apOptions);
            KerberosKrbError krbError2 = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_PREAUTH_REQUIRED, krbError2.ErrorCode, "Pre-authentication required.");

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

            PaPacRequest paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> seqOfPaData3 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp3.Data, paPacRequest.Data, paPacOptions.Data });

            client.SendAsRequestWithFast(options, seqOfPaData3, null, subkey, fastOptions, apOptions);
            KerberosAsResponse userKrbAsRep = client.ExpectAsResponse();
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
            if (testConfig.IsClaimSupported)
            {
                PaSupportedEncTypes paSupportedEncTypes = null;
                foreach (var padata in userKrbAsRep.EncPart.pa_datas.Elements)
                {
                    var parsedPadata = PaDataParser.ParseRepPaData(padata);
                    if (parsedPadata is PaSupportedEncTypes)
                        paSupportedEncTypes = parsedPadata as PaSupportedEncTypes;
                }

                BaseTestSite.Assert.IsNotNull(paSupportedEncTypes, "The encrypted padata of AS-REP contains PA_SUPPORTED_ENCTYPES.");
                BaseTestSite.Assert.IsTrue(
                    paSupportedEncTypes.SupportedEncTypes.HasFlag(SupportedEncryptionTypes.Claims_Supported),
                    "Claims is supported.");
                BaseTestSite.Assert.IsTrue(
                    paSupportedEncTypes.SupportedEncTypes.HasFlag(SupportedEncryptionTypes.FAST_Supported),
                    "FAST is supported.");
            }
            // FAST armored TGS_REQ and TGS_REP using user principal
            subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password03!", "this is a salt");

            client.Context.ArmorSessionKey = client.Context.Ticket.SessionKey;
            client.Context.ArmorTicket = client.Context.Ticket;

            client.SendTgsRequestWithExplicitFast(testConfig.LocalRealm.FileServer[0].Smb2ServiceName, options, null, null, subkey, fastOptions, apOptions);
            KerberosTgsResponse userKrbTgsRep = client.ExpectTgsResponse(KeyUsageNumber.TGS_REP_encrypted_part_subkey);
            if (testConfig.IsClaimSupported)
            {
                PaSupportedEncTypes paSupportedEncTypes = null;
                BaseTestSite.Assert.IsNotNull(asResponse.EncPart, "The encrypted part of AS-REP is decrypted.");
                BaseTestSite.Assert.IsNotNull(asResponse.EncPart.pa_datas, "The encrypted padata is not null.");
                foreach (var padata in userKrbTgsRep.EncPart.pa_datas.Elements)
                {
                    var parsedPadata = PaDataParser.ParseRepPaData(padata);
                    if (parsedPadata is PaSupportedEncTypes)
                        paSupportedEncTypes = parsedPadata as PaSupportedEncTypes;
                }
                BaseTestSite.Assert.IsNotNull(paSupportedEncTypes, "The encrypted padata of AS-REP contains PA_SUPPORTED_ENCTYPES.");
                BaseTestSite.Assert.IsTrue(
                    paSupportedEncTypes.SupportedEncTypes.HasFlag(SupportedEncryptionTypes.CompoundIdentity_Supported),
                    "Compound identity is supported.");
            }

            EncryptionKey key = testConfig.QueryKey(this.testConfig.LocalRealm.FileServer[0].Smb2ServiceName, this.testConfig.LocalRealm.RealmName, this.client.Context.SelectedEType);
            userKrbTgsRep.DecryptTicket(key);

            //userKrbTgsRep.DecryptTicket(testConfig.LocalRealm.FileServer[0].Password, testConfig.LocalRealm.FileServer[0].ServiceSalt);

            //Verify PAC
            if (testConfig.IsKileImplemented)
            {
                BaseTestSite.Assert.IsNotNull(userKrbTgsRep.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(userKrbTgsRep.TicketEncPart.authorization_data.Elements);
                BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2KPac.");

                DeviceClaimsInfo deviceClaimsInfo = null;
                foreach (var buf in adWin2kPac.Pac.PacInfoBuffers)
                {
                    if (buf is DeviceClaimsInfo)
                    {
                        deviceClaimsInfo = buf as DeviceClaimsInfo;
                        break;
                    }
                }
                BaseTestSite.Assert.IsNotNull(deviceClaimsInfo, "PAC_DEVICE_INFO is generated.");
            }
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.Claim)]
        [TestCategory(TestCategories.RC4)]
        [Feature(Feature.Kile | Feature.Pac | Feature.Claim | Feature.RC4)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the PAC_CLIENT_CLAIMS_INFO in PAC when using RC4-HMAC.")]
        public void RC4_PAC_CLIENT_CLAIMS_INFO()
        {
            base.Logging();

            client = new KerberosTestClient(
                this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.ClientComputer.NetBiosName,
               this.testConfig.LocalRealm.ClientComputer.Password,
               KerberosAccountType.Device, testConfig.LocalRealm.KDC[0].IPAddress,
               testConfig.LocalRealm.KDC[0].Port,
               testConfig.TransportType,
                testConfig.SupportedOid,
               testConfig.LocalRealm.ClientComputer.AccountSalt);

            EncryptionType[] rc4HmacType = new EncryptionType[]
            { 
                EncryptionType.RC4_HMAC
            };

            // Define device principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set device principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);

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
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

            // AS_REQ and AS_REP using device principal
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data });
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

            // Switch to user principal
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName, this.testConfig.LocalRealm.User[2].Username, this.testConfig.LocalRealm.User[2].Password, KerberosAccountType.User, client.Context.Ticket, client.Context.SessionKey,
                testConfig.LocalRealm.KDC[0].IPAddress, testConfig.LocalRealm.KDC[0].Port, testConfig.TransportType,
                testConfig.SupportedOid);

            // Define user principal client supported encryption type
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set user principal client supported encryption type as RC4_HMAC.");
            client.SetSupportedEType(rc4HmacType);

            // FAST armored AS_REQ and KRB-ERROR using user principal
            //Create a "random" key.
            var subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password02!", "this is a salt");

            var fastOptions = new Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth.FastOptions(KerberosUtility.ConvertInt2Flags((int)0));
            var apOptions = ApOptions.None;
            string timeStamp2 = KerberosUtility.CurrentKerberosTime.Value;
            PaFxFastReq paFxFastReq = new PaFxFastReq(null);
            Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { (paFxFastReq.Data) });

            client.SendAsRequestWithFast(options, seqOfPaData2, null, subkey, fastOptions, apOptions);
            KerberosKrbError krbError2 = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_PREAUTH_REQUIRED, krbError2.ErrorCode, "Pre-authentication required.");

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
            PaPacRequest paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> seqOfPaData3 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp3.Data, paPacRequest.Data, paPacOptions.Data });

            client.SendAsRequestWithFast(options, seqOfPaData3, null, subkey, fastOptions, apOptions);
            KerberosAsResponse userKrbAsRep = client.ExpectAsResponse();
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
            if (testConfig.IsClaimSupported)
            {
                PaSupportedEncTypes paSupportedEncTypes = null;
                foreach (var padata in userKrbAsRep.EncPart.pa_datas.Elements)
                {
                    var parsedPadata = PaDataParser.ParseRepPaData(padata);
                    if (parsedPadata is PaSupportedEncTypes)
                        paSupportedEncTypes = parsedPadata as PaSupportedEncTypes;
                }

                BaseTestSite.Assert.IsNotNull(paSupportedEncTypes, "The encrypted padata of AS-REP contains PA_SUPPORTED_ENCTYPES.");
                BaseTestSite.Assert.IsTrue(
                    paSupportedEncTypes.SupportedEncTypes.HasFlag(SupportedEncryptionTypes.Claims_Supported),
                    "Claims is supported.");
                BaseTestSite.Assert.IsTrue(
                    paSupportedEncTypes.SupportedEncTypes.HasFlag(SupportedEncryptionTypes.FAST_Supported),
                    "FAST is supported.");
            }
            // FAST armored TGS_REQ and TGS_REP using user principal
            subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password03!", "this is a salt");

            client.Context.ArmorSessionKey = client.Context.Ticket.SessionKey;
            client.Context.ArmorTicket = client.Context.Ticket;

            client.SendTgsRequestWithExplicitFast(testConfig.LocalRealm.FileServer[0].Smb2ServiceName, options, null, null, subkey, fastOptions, apOptions);
            KerberosTgsResponse userKrbTgsRep = client.ExpectTgsResponse(KeyUsageNumber.TGS_REP_encrypted_part_subkey);
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
            if (testConfig.IsClaimSupported)
            {
                PaSupportedEncTypes paSupportedEncTypes = null;
                BaseTestSite.Assert.IsNotNull(asResponse.EncPart, "The encrypted part of AS-REP is decrypted.");
                BaseTestSite.Assert.IsNotNull(asResponse.EncPart.pa_datas, "The encrypted padata is not null.");
                foreach (var padata in userKrbTgsRep.EncPart.pa_datas.Elements)
                {
                    var parsedPadata = PaDataParser.ParseRepPaData(padata);
                    if (parsedPadata is PaSupportedEncTypes)
                        paSupportedEncTypes = parsedPadata as PaSupportedEncTypes;
                }
                BaseTestSite.Assert.IsNotNull(paSupportedEncTypes, "The encrypted padata of AS-REP contains PA_SUPPORTED_ENCTYPES.");
                BaseTestSite.Assert.IsTrue(
                    paSupportedEncTypes.SupportedEncTypes.HasFlag(SupportedEncryptionTypes.CompoundIdentity_Supported),
                    "Compound identity is supported.");
            }

            EncryptionKey key = testConfig.QueryKey(this.testConfig.LocalRealm.FileServer[0].Smb2ServiceName, this.testConfig.LocalRealm.RealmName, this.client.Context.SelectedEType);
            userKrbTgsRep.DecryptTicket(key);

            //userKrbTgsRep.DecryptTicket(testConfig.LocalRealm.FileServer[0].Password, testConfig.LocalRealm.FileServer[0].ServiceSalt);

            //Verify PAC
            if (testConfig.IsKileImplemented)
            {
                BaseTestSite.Assert.IsNotNull(userKrbTgsRep.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(userKrbTgsRep.TicketEncPart.authorization_data.Elements);
                BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2KPac.");

                ClientClaimsInfo clientClaimsInfo = null;
                foreach (var buf in adWin2kPac.Pac.PacInfoBuffers)
                {
                    if (buf is ClientClaimsInfo)
                    {
                        clientClaimsInfo = buf as ClientClaimsInfo;
                        break;
                    }
                }
                BaseTestSite.Assert.IsNotNull(clientClaimsInfo, "PAC_CLIENT_INFO is generated.");
            }
        }

        #endregion

        #region KileCrossRealmTest in RC4

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.CrossRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.Claim)]
        [TestCategory(TestCategories.RC4)]
        [Feature(Feature.Kile | Feature.Pac | Feature.Claim | Feature.RC4)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("Test PAC_DEVICE_INFO structure in cross realm environment when using RC4-HMAC.")]
        public void RC4_CrossRealm_PAC_DEVICE_INFO()
        {
            base.Logging();

            // Clear trust realm encryption type
            IClientControlAdapter adapter = BaseTestSite.GetAdapter<IClientControlAdapter>();
            adapter.ClearTrustRealmEncType();

            try
            {
                client = new KerberosTestClient(
                  this.testConfig.LocalRealm.RealmName,
                  this.testConfig.LocalRealm.ClientComputer.NetBiosName,
                  this.testConfig.LocalRealm.ClientComputer.Password,
                  KerberosAccountType.Device, testConfig.LocalRealm.KDC[0].IPAddress,
                  testConfig.LocalRealm.KDC[0].Port,
                  testConfig.TransportType,
                    testConfig.SupportedOid,
                  testConfig.LocalRealm.ClientComputer.AccountSalt);

                EncryptionType[] rc4HmacType = new EncryptionType[]
                { 
                    EncryptionType.RC4_HMAC
                };

                // Define device principal client supported encryption type
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Set device principal client supported encryption type as RC4_HMAC.");
                client.SetSupportedEType(rc4HmacType);

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
                BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

                //Create sequence of PA data
                string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
                PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, this.client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
                PaPacRequest paPacRequest = new PaPacRequest(true);
                Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });

                //Create and send AS request
                client.SendAsRequest(options, seqOfPaData);
                KerberosAsResponse asResponse = client.ExpectAsResponse();
                BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

                //Create and send TGS request
                client.SendTgsRequest(this.testConfig.TrustedRealm.KDC[0].DefaultServiceName, options);
                KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();
                BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

                KerberosTicket referralComputerTicket = client.Context.Ticket;
                EncryptionKey referralComputerSessionKey = client.Context.Ticket.SessionKey;

                //start
                client = new KerberosTestClient(
                  this.testConfig.LocalRealm.RealmName,
                  this.testConfig.LocalRealm.ClientComputer.NetBiosName,
                  this.testConfig.LocalRealm.ClientComputer.Password,
                  KerberosAccountType.Device, testConfig.LocalRealm.KDC[0].IPAddress,
                  testConfig.LocalRealm.KDC[0].Port,
                  testConfig.TransportType,
                    testConfig.SupportedOid,
                  testConfig.LocalRealm.ClientComputer.AccountSalt);

                // Define device principal client supported encryption type
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Set device principal client supported encryption type as RC4_HMAC.");
                client.SetSupportedEType(rc4HmacType);

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
                options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
                client.SendAsRequest(options, null);
                KerberosKrbError krbError1 = client.ExpectPreauthRequiredError(out methodData);
                BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

                // AS_REQ and AS_REP using device principal
                timeStamp = KerberosUtility.CurrentKerberosTime.Value;
                paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
                seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data });
                client.SendAsRequest(options, seqOfPaData);
                asResponse = client.ExpectAsResponse();
                BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

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

                // Define user principal client supported encryption type
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Set user principal client supported encryption type as RC4_HMAC.");
                client.SetSupportedEType(rc4HmacType);

                // FAST armored AS_REQ and KRB-ERROR using user principal
                //Create a "random" key.
                var subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password02!", "this is a salt");

                var fastOptions = new Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth.FastOptions(KerberosUtility.ConvertInt2Flags((int)0));
                var apOptions = ApOptions.None;
                string timeStamp2 = KerberosUtility.CurrentKerberosTime.Value;
                PaFxFastReq paFxFastReq = new PaFxFastReq(null);
                Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { (paFxFastReq.Data) });

                client.SendAsRequestWithFast(options, seqOfPaData2, null, subkey, fastOptions, apOptions);
                KerberosKrbError krbError2 = client.ExpectKrbError();
                BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
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
                paPacRequest = new PaPacRequest(true);
                PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
                Asn1SequenceOf<PA_DATA> outerSeqPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paPacRequest.Data, paPacOptions.Data });
                client.SendAsRequestWithFast(options, seqOfPaData3, outerSeqPaData, subkey, fastOptions, apOptions);
                KerberosAsResponse userKrbAsRep = client.ExpectAsResponse();
                BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

                if (testConfig.IsClaimSupported)
                {
                    PaSupportedEncTypes paSupportedEncTypes = null;
                    foreach (var padata in userKrbAsRep.EncPart.pa_datas.Elements)
                    {
                        var parsedPadata = PaDataParser.ParseRepPaData(padata);
                        if (parsedPadata is PaSupportedEncTypes)
                            paSupportedEncTypes = parsedPadata as PaSupportedEncTypes;
                    }

                    BaseTestSite.Assert.IsNotNull(paSupportedEncTypes, "The encrypted padata of AS-REP contains PA_SUPPORTED_ENCTYPES.");
                    BaseTestSite.Assert.IsTrue(
                        paSupportedEncTypes.SupportedEncTypes.HasFlag(SupportedEncryptionTypes.Claims_Supported),
                        "Claims is supported.");
                    BaseTestSite.Assert.IsTrue(
                        paSupportedEncTypes.SupportedEncTypes.HasFlag(SupportedEncryptionTypes.FAST_Supported),
                        "FAST is supported.");
                }
                // FAST armored TGS_REQ and TGS_REP using user principal
                subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password03!", "this is a salt");

                client.Context.ArmorSessionKey = client.Context.Ticket.SessionKey;
                client.Context.ArmorTicket = client.Context.Ticket;

                if (this.testConfig.TrustType == Adapter.TrustType.Forest)
                {
                    client.SendTgsRequestWithExplicitFast(testConfig.TrustedRealm.FileServer[0].Smb2ServiceName,
                        options,
                        null,
                        null,
                        subkey,
                        fastOptions,
                        apOptions);
                }
                else if (this.testConfig.TrustType == Adapter.TrustType.Realm)
                {
                    client.SendTgsRequestWithExplicitFast(testConfig.TrustedRealm.KDC[0].DefaultServiceName,
                        options,
                        null,
                        null,
                        subkey,
                        fastOptions,
                        apOptions);
                }
                KerberosTgsResponse userKrbTgsRep = client.ExpectTgsResponse(KeyUsageNumber.TGS_REP_encrypted_part_subkey);
                BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");
                EncryptionKey key = testConfig.QueryKey(
                        this.testConfig.TrustedRealm.KDC[0].DefaultServiceName + "@" + this.testConfig.LocalRealm.RealmName,
                        client.Context.Realm.ToString(),
                        client.Context.SelectedEType);
                userKrbTgsRep.DecryptTicket(key);

                client.ChangeRealm(this.testConfig.TrustedRealm.RealmName,
                this.testConfig.TrustedRealm.KDC[0].IPAddress,
                this.testConfig.TrustedRealm.KDC[0].Port,
                this.testConfig.TransportType);

                //Create and send referral TGS request            
                client.Context.ArmorTicket = referralComputerTicket;
                client.Context.ArmorSessionKey = referralComputerTicket.SessionKey;
                client.SendTgsRequestWithExplicitFast(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, options, null, null, subkey, fastOptions, apOptions);
                KerberosTgsResponse refTgsResponse = client.ExpectTgsResponse(KeyUsageNumber.TGS_REP_encrypted_part_subkey);
                BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

                key = testConfig.QueryKey(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, client.Context.Realm.ToString(), client.Context.SelectedEType);
                refTgsResponse.DecryptTicket(key);

                if (testConfig.IsClaimSupported && testConfig.IsKileImplemented)
                {
                    BaseTestSite.Assert.IsNotNull(refTgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                    PaSupportedEncTypes paSupportedEncTypes = null;
                    BaseTestSite.Assert.IsNotNull(asResponse.EncPart, "The encrypted part of AS-REP is decrypted.");
                    BaseTestSite.Assert.IsNotNull(asResponse.EncPart.pa_datas, "The encrypted padata is not null.");
                    foreach (var padata in refTgsResponse.EncPart.pa_datas.Elements)
                    {
                        var parsedPadata = PaDataParser.ParseRepPaData(padata);
                        if (parsedPadata is PaSupportedEncTypes)
                            paSupportedEncTypes = parsedPadata as PaSupportedEncTypes;
                    }
                    BaseTestSite.Assert.IsNotNull(paSupportedEncTypes, "The encrypted padata of AS-REP contains PA_SUPPORTED_ENCTYPES.");
                    BaseTestSite.Assert.IsTrue(
                        paSupportedEncTypes.SupportedEncTypes.HasFlag(SupportedEncryptionTypes.CompoundIdentity_Supported),
                        "Compound identity is supported.");

                    //Verify PAC
                    BaseTestSite.Assert.IsNotNull(refTgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                    AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(refTgsResponse.TicketEncPart.authorization_data.Elements);
                    BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2KPac.");

                    PacDeviceInfo pacDeviceInfo = null;
                    foreach (var buf in adWin2kPac.Pac.PacInfoBuffers)
                    {
                        if (buf is PacDeviceInfo)
                        {
                            pacDeviceInfo = buf as PacDeviceInfo;
                            break;
                        }
                    }
                    BaseTestSite.Assert.IsNotNull(pacDeviceInfo, "PAC_DEVICE_INFO is generated.");
                }
            }
            finally
            {
                adapter.SetTrustRealmEncTypeAsAes();
            }
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.CrossRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.Claim)]
        [TestCategory(TestCategories.RC4)]
        [Feature(Feature.Kile | Feature.Pac | Feature.Claim | Feature.RC4)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("Test PAC_DEVICE_CLAIMS_INFO structure in cross realm environment when using RC4-HMAC.")]
        public void RC4_CrossRealm_PAC_DEVICE_CLAIMS_INFO()
        {
            base.Logging();

            // Clear trust realm encryption type
            IClientControlAdapter adapter = BaseTestSite.GetAdapter<IClientControlAdapter>();
            adapter.ClearTrustRealmEncType();

            try
            {
                client = new KerberosTestClient(
                  this.testConfig.LocalRealm.RealmName,
                  this.testConfig.LocalRealm.ClientComputer.NetBiosName,
                  this.testConfig.LocalRealm.ClientComputer.Password,
                  KerberosAccountType.Device, testConfig.LocalRealm.KDC[0].IPAddress,
                  testConfig.LocalRealm.KDC[0].Port,
                  testConfig.TransportType,
                    testConfig.SupportedOid,
                  testConfig.LocalRealm.ClientComputer.AccountSalt);

                EncryptionType[] rc4HmacType = new EncryptionType[]
                { 
                    EncryptionType.RC4_HMAC
                };

                // Define device principal client supported encryption type
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Set device principal client supported encryption type as RC4_HMAC.");
                client.SetSupportedEType(rc4HmacType);

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
                BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

                //Create sequence of PA data
                string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
                PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, this.client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
                PaPacRequest paPacRequest = new PaPacRequest(true);
                Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });

                //Create and send AS request
                client.SendAsRequest(options, seqOfPaData);
                KerberosAsResponse asResponse = client.ExpectAsResponse();
                BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

                //Create and send TGS request

                client.SendTgsRequest(this.testConfig.TrustedRealm.KDC[0].DefaultServiceName, options);
                KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();
                BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

                KerberosTicket referralComputerTicket = client.Context.Ticket;
                EncryptionKey referralComputerSessionKey = client.Context.Ticket.SessionKey;

                //start
                client = new KerberosTestClient(
                  this.testConfig.LocalRealm.RealmName,
                  this.testConfig.LocalRealm.ClientComputer.NetBiosName,
                  this.testConfig.LocalRealm.ClientComputer.Password,
                  KerberosAccountType.Device, testConfig.LocalRealm.KDC[0].IPAddress,
                  testConfig.LocalRealm.KDC[0].Port,
                  testConfig.TransportType,
                    testConfig.SupportedOid,
                  testConfig.LocalRealm.ClientComputer.AccountSalt);

                // Define device principal client supported encryption type
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Set device principal client supported encryption type as RC4_HMAC.");
                client.SetSupportedEType(rc4HmacType);

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
                options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
                client.SendAsRequest(options, null);
                KerberosKrbError krbError1 = client.ExpectPreauthRequiredError(out methodData);
                BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

                // AS_REQ and AS_REP using device principal
                timeStamp = KerberosUtility.CurrentKerberosTime.Value;
                paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
                seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data });
                client.SendAsRequest(options, seqOfPaData);
                asResponse = client.ExpectAsResponse();
                BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

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

                // Define user principal client supported encryption type
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Set user principal client supported encryption type as RC4_HMAC.");
                client.SetSupportedEType(rc4HmacType);

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
                Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { (paFxFastReq.Data) });

                client.SendAsRequestWithFast(options, seqOfPaData2, null, subkey, fastOptions, apOptions);
                KerberosKrbError krbError2 = client.ExpectKrbError();
                BaseTestSite.Assert.AreEqual(krbError2.ErrorCode, KRB_ERROR_CODE.KDC_ERR_PREAUTH_REQUIRED, "Pre-authentication required.");
                BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

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
                paPacRequest = new PaPacRequest(true);
                PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
                Asn1SequenceOf<PA_DATA> outerSeqPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paPacRequest.Data, paPacOptions.Data });
                client.SendAsRequestWithFast(options, seqOfPaData3, outerSeqPaData, subkey, fastOptions, apOptions);
                KerberosAsResponse userKrbAsRep = client.ExpectAsResponse();
                BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

                if (testConfig.IsClaimSupported)
                {
                    PaSupportedEncTypes paSupportedEncTypes = null;
                    foreach (var padata in userKrbAsRep.EncPart.pa_datas.Elements)
                    {
                        var parsedPadata = PaDataParser.ParseRepPaData(padata);
                        if (parsedPadata is PaSupportedEncTypes)
                            paSupportedEncTypes = parsedPadata as PaSupportedEncTypes;
                    }

                    BaseTestSite.Assert.IsNotNull(paSupportedEncTypes, "The encrypted padata of AS-REP contains PA_SUPPORTED_ENCTYPES.");
                    BaseTestSite.Assert.IsTrue(
                        paSupportedEncTypes.SupportedEncTypes.HasFlag(SupportedEncryptionTypes.Claims_Supported),
                        "Claims is supported.");
                    BaseTestSite.Assert.IsTrue(
                        paSupportedEncTypes.SupportedEncTypes.HasFlag(SupportedEncryptionTypes.FAST_Supported),
                        "FAST is supported.");
                }
                // FAST armored TGS_REQ and TGS_REP using user principal
                subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password03!", "this is a salt");

                client.Context.ArmorSessionKey = client.Context.Ticket.SessionKey;
                client.Context.ArmorTicket = client.Context.Ticket;

                if (this.testConfig.TrustType == Adapter.TrustType.Forest)
                {
                    client.SendTgsRequestWithExplicitFast(testConfig.TrustedRealm.FileServer[0].Smb2ServiceName,
                        options,
                        null,
                        null,
                        subkey,
                        fastOptions,
                        apOptions);
                }
                else if (this.testConfig.TrustType == Adapter.TrustType.Realm)
                {
                    client.SendTgsRequestWithExplicitFast(testConfig.TrustedRealm.KDC[0].DefaultServiceName,
                        options,
                        null,
                        null,
                        subkey,
                        fastOptions,
                        apOptions);
                }
                KerberosTgsResponse userKrbTgsRep = client.ExpectTgsResponse(KeyUsageNumber.TGS_REP_encrypted_part_subkey);
                BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

                client.ChangeRealm(this.testConfig.TrustedRealm.RealmName,
                this.testConfig.TrustedRealm.KDC[0].IPAddress,
                this.testConfig.TrustedRealm.KDC[0].Port,
                this.testConfig.TransportType);

                //Create and send referral TGS request            
                client.Context.ArmorTicket = referralComputerTicket;
                client.Context.ArmorSessionKey = referralComputerTicket.SessionKey;
                client.SendTgsRequestWithExplicitFast(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, options, null, null, subkey, fastOptions, apOptions);
                KerberosTgsResponse refTgsResponse = client.ExpectTgsResponse(KeyUsageNumber.TGS_REP_encrypted_part_subkey);
                BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, client.Context.SelectedEType, "Client selected encryption type should be RC4_HMAC.");

                EncryptionKey key = testConfig.QueryKey(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, client.Context.Realm.ToString(), client.Context.SelectedEType);
                refTgsResponse.DecryptTicket(key);

                //Verify PAC
                if (testConfig.IsKileImplemented && testConfig.IsClaimSupported)
                {
                    BaseTestSite.Assert.IsNotNull(refTgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                    AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(refTgsResponse.TicketEncPart.authorization_data.Elements);
                    BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2KPac.");

                    DeviceClaimsInfo deviceClaimsInfo = null;
                    foreach (var buf in adWin2kPac.Pac.PacInfoBuffers)
                    {
                        if (buf is DeviceClaimsInfo)
                        {
                            deviceClaimsInfo = buf as DeviceClaimsInfo;
                            break;
                        }
                    }
                    BaseTestSite.Assert.IsNotNull(deviceClaimsInfo, "PAC_DEVICE_CLAIM_INFO should be generated.");
                }
            }
            finally
            {
                adapter.SetTrustRealmEncTypeAsAes();
            }
        }

        #endregion

    }
}
