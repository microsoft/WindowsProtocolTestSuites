// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocol.TestSuites.Kerberos.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocol.TestSuites.Kerberos.TestSuite
{
    /// <summary>
    ///
    /// </summary>
    [TestClass]
    public class KileLdapApTest : TraditionTestBase
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
        [TestCategory(TestCategories.LdapAp)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]        
        [Feature(Feature.Kile | Feature.Pac)]
        [ApplicationServer(ApplicationServer.Ldap)]
        [Description("This test case is designed to verify if client can request ticket without PAC from KDC")]
        public void NetworkLogonLdapWithoutPac_Ldap()
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
            PaPacRequest paPacRequest = new PaPacRequest(false);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");
            
            //Create and send TGS request
            client.SendTgsRequest(this.testConfig.LocalRealm.LdapServer[0].LdapServiceName, options);
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();

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
        [Priority(1)]
        [TestCategory(TestCategories.LdapAp)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Kile | Feature.Pac)]
        [ApplicationServer(ApplicationServer.Ldap)]
        [Description("This test case is designed to verify if client can request ticket with PAC from KDC")]
        public void NetworkLogonLdapWithPac_Ldap()
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
            client.SendTgsRequest(this.testConfig.LocalRealm.LdapServer[0].LdapServiceName, options);
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();

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
        [Priority(1)]
        [TestCategory(TestCategories.LdapAp)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.Claim)]
        [Feature(Feature.Kile | Feature.Pac | Feature.Claim)]
        [ApplicationServer(ApplicationServer.Ldap)]
        [Description("This test case is designed to verify if client can request ticket with PAC and claim from KDC")]
        public void NetworkLogonLdapWithClaim_Ldap()
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
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data, paPacOptions.Data });
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create and send TGS request
            client.SendTgsRequest(this.testConfig.LocalRealm.LdapServer[0].LdapServiceName, options);
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();


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

    }
}
