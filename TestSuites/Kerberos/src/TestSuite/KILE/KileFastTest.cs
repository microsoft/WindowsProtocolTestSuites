// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocol.TestSuites.Kerberos.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocol.TestSuites.Kerberos.TestSuite
{
    /// <summary>
    ///
    /// </summary>
    [TestClass]
    public class KileFastTest : TraditionTestBase
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
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.FAST)]        
        [Description("This test case is designed to verify that KDCs sends the advertisement of PA-FX-FAST.")]
        [Feature(Feature.FAST)]
        [ApplicationServer(ApplicationServer.All)]
        public void PaFxFastAdvertise()
        {
            base.Logging();

            //Create kerberos test client and connect
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName, this.testConfig.LocalRealm.User[1].Username,
                this.testConfig.LocalRealm.User[1].Password, KerberosAccountType.User, testConfig.LocalRealm.KDC[0].IPAddress, testConfig.LocalRealm.KDC[0].Port, testConfig.TransportType,
                testConfig.SupportedOid);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Construct Kerberos client for testing."); //Create and send AS request

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
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with no PA data.");
            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve preauthentication required error.");

            var seqOfPadata = new Asn1SequenceOf<PA_DATA>();
            BaseTestSite.Assert.IsNotNull(krbError.KrbError.e_data, "e_data is not null.");
            seqOfPadata.BerDecode(new Asn1DecodingBuffer(krbError.KrbError.e_data.ByteArrayValue));
            PaFxFastRep paFxFast = null;
            var padataCount = seqOfPadata.Elements.Length;

            for (int i = 0; i < padataCount; i++)
            {
                var padata = PaDataParser.ParseRepPaData(seqOfPadata.Elements[i]);
                if (padata is PaFxFastRep)
                {
                    paFxFast = padata as PaFxFastRep;
                    break;
                }
            }
            BaseTestSite.Assert.IsNotNull(paFxFast, "The KDC SHOULD advertise PA-FX-FAST in a PREAUTH_REQUIRED error.");
            BaseTestSite.Assert.IsNull(paFxFast.FastReply, "KDCs MUST send the advertisement of PA-FX-FAST with an empty pa-value.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.FAST)]       
        [Description("This test case is designed to verify that KDCs supports strengthen key.")]
        [Feature(Feature.FAST)]
        [ApplicationServer(ApplicationServer.All)]
        public void StrengthenKey()
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
        [Description("This test case is designed to verify that KDCs supports HideClientNames option.")]
        [Feature(Feature.FAST)]
        [ApplicationServer(ApplicationServer.All)]
        public void FastOptions_HideClientNames()
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

            var apOptions = ApOptions.None;

            Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { new PA_DATA(new KerbInt32((long)PaDataType.PA_FX_FAST), null) });

            client.SendAsRequestWithFastHideCName(options, "YouKnowWho", seqOfPaData2, null, subkey, apOptions);

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
            client.SendAsRequestWithFastHideCName(options, "YouKnowWho", seqOfPaData3, outerSeqPaData, subkey, apOptions);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve AS response.");
            KerberosAsResponse userKrbAsRep = client.ExpectAsResponse();
            bool cNameHidden = userKrbAsRep.Response.cname == null 
                || userKrbAsRep.Response.cname.name_string.Elements[0] != client.Context.CName.Name.name_string.Elements[0];
            BaseTestSite.Assert.IsTrue(cNameHidden, "CName is hidden in AS-REP.");

            // FAST armored TGS_REQ and TGS_REP using user principal
            subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password03!", "this is a salt");
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send FAST armored TGS request.");
            client.SendTgsRequestWithFastHideCName(testConfig.LocalRealm.ClientComputer.DefaultServiceName, userKrbAsRep.Response.cname, options, null, null, subkey, apOptions);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve TGS response.");
            KerberosTgsResponse userKrbTgsRep = client.ExpectTgsResponse(KeyUsageNumber.TGS_REP_encrypted_part_subkey);
            cNameHidden = userKrbTgsRep.Response.cname == null
                || userKrbTgsRep.Response.cname.name_string.Elements[0] != client.Context.CName.Name.name_string.Elements[0];
            BaseTestSite.Assert.IsTrue(cNameHidden, "CName is hidden in TGS-REP.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.FAST)]
        [Description("This test case is designed to verify FastKrb_Error.")]
        [Feature(Feature.FAST)]
        [ApplicationServer(ApplicationServer.All)]
        public void FastKrb_Error()
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
            BaseTestSite.Assert.IsNotNull(paFxFastRep , "KDC must reply a PA-FX-Fast response.");
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
            BaseTestSite.Assert.IsNotNull(paFxError , "KDC must include a PA-FX-ERROR padata.");
            BaseTestSite.Assert.IsNull(paFxError.KrbError.KrbError.e_data, "E_data is null.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.FAST)]
        [Description("This test case is designed to verify that KDCs supports KrbFastFinished in AS-REP.")]
        [Feature(Feature.FAST)]
        [ApplicationServer(ApplicationServer.All)]
        public void KrbFastFinishedAsRep()
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
            Checksum ticketChecksum = new Checksum(new KerbInt32((long)KerberosUtility.GetChecksumType(client.Context.SelectedEType)),new Asn1OctetString(KerberosUtility.GetChecksum(
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
        [Description("This test case is designed to verify that KDCs supports KrbFastFinished in TGS-REP.")]
        [Feature(Feature.FAST)]
        [ApplicationServer(ApplicationServer.All)]
        public void KrbFastFinishedTgsRep()
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
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                string.Format("The type of AS-REP encrypted part is {0}.", userKrbAsRep.EncPart.GetType().Name));

            // FAST armored TGS_REQ and TGS_REP using user principal
            subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password03!", "this is a salt");
            client.SendTgsRequestWithFast(testConfig.LocalRealm.ClientComputer.DefaultServiceName, options, null, null, subkey, fastOptions, apOptions);
            KerberosTgsResponse userKrbTgsRep = client.ExpectTgsResponse(KeyUsageNumber.TGS_REP_encrypted_part_subkey);
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
        [Description("This test case is designed to verify that KDCs rejects unarmored TGS-REQ if Ad-fx-fast-used is in the authenticator.")]
        [Feature(Feature.FAST)]
        [ApplicationServer(ApplicationServer.All)]
        public void AdFxFastUsedInAuthenticator()
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
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.FAST)]
        [Description("This test case is designed to verify that KDCs rejects unarmored TGS-REQ if Ad-fx-fast-armor is in the authenticator.")]
        [Feature(Feature.FAST)]
        [ApplicationServer(ApplicationServer.All)]
        public void AdFxFastArmorInAuthenticator()
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
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                string.Format("The type of AS-REP encrypted part is {0}.", asResponse.EncPart.GetType().Name));

            AdFxFastArmor adFxFastArmor = new AdFxFastArmor();
            AuthorizationData authData = new AuthorizationData(new AuthorizationDataElement[] { adFxFastArmor.AuthDataElement });
            client.SendTgsRequest(testConfig.LocalRealm.ClientComputer.DefaultServiceName, options, null, null, authData);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Receive TGS Error, KDC MUST reject the request.");
            KerberosKrbError krbError = client.ExpectKrbError();

        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.FAST)]
        [Feature(Feature.FAST)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify that KDCs rejects unknown fast armor type.")]
        public void UnsupportedFastArmorType()
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
            Asn1SequenceOf<PA_DATA> seqOfPaData3 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp3.Data });
            PaPacRequest paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> outerSeqPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paPacRequest.Data, paPacOptions.Data });
            client.SendAsRequestWithFast(options, seqOfPaData3, outerSeqPaData, subkey, fastOptions, apOptions, 0);
            KerberosKrbError krbError3 = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(krbError3.ErrorCode,
                KRB_ERROR_CODE.KDC_ERR_PREAUTH_FAILED,
                "Pre-authentication failed.");

        }
    }
}
