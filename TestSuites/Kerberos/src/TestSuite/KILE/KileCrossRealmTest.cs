// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Protocol.TestSuites.Kerberos.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using Microsoft.Protocols.TestTools.StackSdk.Security.Pac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocol.TestSuites.Kerberos.TestSuite.KILE
{
    [TestClass]
    public class KileCrossRealmTest : TraditionTestBase
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

        #region Cross Realm Without AP
        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.CrossRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)] 
        [Description("User sends Kerberos request to trusted realm directly")]
        public void UserInAnotherDomain()
        {
            base.Logging();

            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[1].Username,
                this.testConfig.LocalRealm.User[1].Password,
                KerberosAccountType.User,
                testConfig.TrustedRealm.KDC[0].IPAddress,
                testConfig.TrustedRealm.KDC[0].Port,
                testConfig.TransportType,
                testConfig.SupportedOid);

            //Create and send AS request
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);

            //Recieve preauthentication required error
            KerberosKrbError krbError = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_WRONG_REALM,
                krbError.ErrorCode,
                "KDC should return wrong realm error.");
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.CrossRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Description("Transitedpolicy flag MUST not be checked by KILE")]
        public void TransitedPolicyCheckedFlag()
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

            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE | KdcOptions.RENEWABLEOK;
            TypicalASExchange(client, options);

            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> paData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paPacOptions.Data });
            //Create and send TGS request
            if (this.testConfig.TrustType == Adapter.TrustType.Forest)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, options, paData);
            }
            else if (this.testConfig.TrustType == Adapter.TrustType.Realm)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.KDC[0].DefaultServiceName, options, paData);
            }
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();
            EncryptionKey key = testConfig.QueryKey(
                    this.testConfig.TrustedRealm.KDC[0].DefaultServiceName + "@" + this.testConfig.LocalRealm.RealmName,
                    client.Context.Realm.ToString(),
                    client.Context.SelectedEType);
            tgsResponse.DecryptTicket(key);

            //Change realm
            client.ChangeRealm(this.testConfig.TrustedRealm.RealmName,
                this.testConfig.TrustedRealm.KDC[0].IPAddress,
                this.testConfig.TrustedRealm.KDC[0].Port,
                this.testConfig.TransportType);

            //Create and send referral TGS request
            client.SendTgsRequest(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, options, paData);
            KerberosTgsResponse refTgsResponse = client.ExpectTgsResponse();

            if (testConfig.IsKileImplemented)
            {
                EncTicketFlags ticketFlags = (EncTicketFlags)KerberosUtility.ConvertFlags2Int(refTgsResponse.EncPart.flags.ByteArrayValue);
                BaseTestSite.Assert.IsFalse(ticketFlags.HasFlag(EncTicketFlags.TRANSITED_POLICY_CHECKED),
                    "KILE MUST NOT check for transited domains on servers or a KDC.");
            }
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.CrossRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Description("Service principal MUST be canonicalized if set flag")]
        public void CanonicalizeSpnInReferralTgt()
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

            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE | KdcOptions.RENEWABLEOK;
            TypicalASExchange(client, options);

            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> paData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paPacOptions.Data });
            //Create and send TGS request
            if (this.testConfig.TrustType == Adapter.TrustType.Forest)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, options, paData);
            }
            else
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.KDC[0].DefaultServiceName, options, paData);
            }
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();
            EncryptionKey key = testConfig.QueryKey(
                    this.testConfig.TrustedRealm.KDC[0].DefaultServiceName + "@" + this.testConfig.LocalRealm.RealmName,
                    client.Context.Realm.ToString(),
                    client.Context.SelectedEType);
            tgsResponse.DecryptTicket(key);

            //assert sname
            BaseTestSite.Assert.AreEqual(this.testConfig.TrustedRealm.KDC[0].DefaultServiceName,
                KerberosUtility.PrincipalName2String(tgsResponse.Response.ticket.sname),
                "The service principal name in referral TGT MUST be canonicalized.");
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.CrossRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Description("Verify the referred-realm field of PA-SVR-REFERRAL-INFO is correctly set.")]
        public void Pa_Svr_Referral_Info()
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

            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE | KdcOptions.RENEWABLEOK;
            TypicalASExchange(client, options);

            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> paData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paPacOptions.Data });

            //Create and send TGS request
            if (this.testConfig.TrustType == Adapter.TrustType.Forest)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, options, paData);
            }
            else if (this.testConfig.TrustType == Adapter.TrustType.Realm)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.KDC[0].DefaultServiceName, options, paData);
            }
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();
            EncryptionKey key = testConfig.QueryKey(
                    this.testConfig.TrustedRealm.KDC[0].DefaultServiceName + "@" + this.testConfig.LocalRealm.RealmName,
                    client.Context.Realm.ToString(),
                    client.Context.SelectedEType);
            tgsResponse.DecryptTicket(key);

            if (tgsResponse.TicketEncPart.authorization_data != null)
            {
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(tgsResponse.TicketEncPart.authorization_data.Elements);
                BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data should contain AdWin2KPac.");
            }

            if (tgsResponse.EncPart.pa_datas == null || tgsResponse.EncPart.pa_datas.Elements == null || tgsResponse.EncPart.pa_datas.Elements.Length == 0)
            {
                BaseTestSite.Assert.Fail("Tgs response should include PAData in encrypted part.");
            }

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
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.CrossRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]           
        [Feature(Feature.Kile | Feature.Pac)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify if KDC supports Other Organization SID in PAC.")]
        public void CrossRealm_OtherOrgSIDinPACSuccess()
        {
            base.Logging();

            //setSelectiveAuth is for windows only
            if (this.testConfig.TrustedRealm.KDC[0].IsWindows && this.testConfig.TrustType != TrustType.NoTrust)
            {
                sutController.setSelectiveAuth(this.testConfig.TrustedRealm.RealmName,
                    this.testConfig.TrustedRealm.Admin.Username,
                    this.testConfig.TrustedRealm.Admin.Password,
                    this.testConfig.LocalRealm.RealmName,
                    true);
            }
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[1].Username,
                this.testConfig.LocalRealm.User[1].Password,
                KerberosAccountType.User,
                testConfig.LocalRealm.KDC[0].IPAddress,
                testConfig.LocalRealm.KDC[0].Port,
                testConfig.TransportType,
                testConfig.SupportedOid);

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
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data, paPacOptions.Data });
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();

            Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] {paPacRequest.Data, paPacOptions.Data });
            //Create and send TGS request            
            if (this.testConfig.TrustType == Adapter.TrustType.Forest)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, options, seqOfPaData2);
            }
            else if (this.testConfig.TrustType == Adapter.TrustType.Realm)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.KDC[0].DefaultServiceName, options, seqOfPaData2);
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
            client.SendTgsRequest(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, options, seqOfPaData2);
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

            //Verify PAC
            if (this.testConfig.IsKileImplemented && this.testConfig.LocalRealm.KDC[0].IsWindows)
            {
                BaseTestSite.Assert.IsNotNull(refTgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(refTgsResponse.TicketEncPart.authorization_data.Elements);
                BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2KPac.");
                KerbValidationInfo kerbValidationInfo = null;
                foreach (var buf in adWin2kPac.Pac.PacInfoBuffers)
                {
                    if (buf is KerbValidationInfo)
                    {
                        kerbValidationInfo = buf as KerbValidationInfo;
                        break;
                    }
                }
                BaseTestSite.Assert.IsNotNull(kerbValidationInfo, "KerbValidationInfo is generated.");

                //sidcount = 3 because, one for AUTHENTICATION_AUTHORITY_ASSERTED_IDENTITY SID, one for CLAIMS_VALID SID and one for OTHER_ORGANIZATION SID
                uint expectedSidCount = 3;
                BaseTestSite.Assert.AreEqual(expectedSidCount, kerbValidationInfo.NativeKerbValidationInfo.SidCount, "The SidCount includes the number of one AUTHENTICATION_AUTHORITY_ASSERTED_IDENTITY, one CLAIMS_VALID, and one OTHER_ORGANIZATION.");

                KERB_SID_AND_ATTRIBUTES[] expectedDefaultExtraSids = new KERB_SID_AND_ATTRIBUTES[expectedSidCount];
                //The CLAIMS_VALID SID is "S-1-5-21-0-0-0-497"
                _RPC_SID CLAIMS_VALID = new _RPC_SID();
                CLAIMS_VALID.Revision = 0x01;
                CLAIMS_VALID.IdentifierAuthority = new _RPC_SID_IDENTIFIER_AUTHORITY();
                CLAIMS_VALID.IdentifierAuthority.Value = new byte[] { 0, 0, 0, 0, 0, 5 };
                CLAIMS_VALID.SubAuthorityCount = 5;
                CLAIMS_VALID.SubAuthority = new uint[] { 21, 0, 0, 0, 497 };
                expectedDefaultExtraSids[0] = new KERB_SID_AND_ATTRIBUTES();
                expectedDefaultExtraSids[0].Attributes = Attributes_Values.Mandatory | Attributes_Values.EnabledByDefault | Attributes_Values.Enabled;
                expectedDefaultExtraSids[0].SID = new _RPC_SID[1];
                expectedDefaultExtraSids[0].SID[0] = CLAIMS_VALID;

                //The AUTHENTICATION_AUTHORITY_ASSERTED_IDENTITY SID is "S-1-18-1"
                _RPC_SID AUTHENTICATION_AUTHORITY_ASSERTED_IDENTITY = new _RPC_SID();
                AUTHENTICATION_AUTHORITY_ASSERTED_IDENTITY.Revision = 0x01;
                AUTHENTICATION_AUTHORITY_ASSERTED_IDENTITY.IdentifierAuthority = new _RPC_SID_IDENTIFIER_AUTHORITY();
                AUTHENTICATION_AUTHORITY_ASSERTED_IDENTITY.IdentifierAuthority.Value = new byte[] { 0, 0, 0, 0, 0, 18 };
                AUTHENTICATION_AUTHORITY_ASSERTED_IDENTITY.SubAuthorityCount = 1;
                AUTHENTICATION_AUTHORITY_ASSERTED_IDENTITY.SubAuthority = new uint[] { 1 };
                expectedDefaultExtraSids[1] = new KERB_SID_AND_ATTRIBUTES();
                expectedDefaultExtraSids[1].Attributes = Attributes_Values.Mandatory | Attributes_Values.EnabledByDefault | Attributes_Values.Enabled;
                expectedDefaultExtraSids[1].SID = new _RPC_SID[1];
                expectedDefaultExtraSids[1].SID[0] = AUTHENTICATION_AUTHORITY_ASSERTED_IDENTITY;

                //The AUTHENTICATION_AUTHORITY_ASSERTED_IDENTITY SID is "S-1-5-1000"
                _RPC_SID OTHER_ORGANIZATION = new _RPC_SID();
                OTHER_ORGANIZATION.Revision = 0x01;
                OTHER_ORGANIZATION.IdentifierAuthority = new _RPC_SID_IDENTIFIER_AUTHORITY();
                OTHER_ORGANIZATION.IdentifierAuthority.Value = new byte[] { 0, 0, 0, 0, 0, 5 };
                OTHER_ORGANIZATION.SubAuthorityCount = 1;
                OTHER_ORGANIZATION.SubAuthority = new uint[] { 1000 };
                expectedDefaultExtraSids[2] = new KERB_SID_AND_ATTRIBUTES();
                expectedDefaultExtraSids[2].Attributes = Attributes_Values.Mandatory | Attributes_Values.EnabledByDefault | Attributes_Values.Enabled;
                expectedDefaultExtraSids[2].SID = new _RPC_SID[1];
                expectedDefaultExtraSids[2].SID[0] = AUTHENTICATION_AUTHORITY_ASSERTED_IDENTITY;

                var expectedExtraSids = expectedDefaultExtraSids;

                uint res = expectedSidCount;
                for (int i = 0; i < expectedSidCount; i++)
                {
                    for (int j = 0; j < expectedSidCount; j++)
                    {
                        if (Adapter.PacHelper.ExtraSidsAreEqual(expectedExtraSids[i], kerbValidationInfo.NativeKerbValidationInfo.ExtraSids[j]))
                        {
                            res--;
                        }
                    }
                }
                BaseTestSite.Assert.AreEqual((uint)0, res, "The ExtraSids field contains the pointer to a list which is the list copied from the PAC in the TGT plus a list constructed from the domain local groups, notice domain local groups is equal to 0 in this test case.");

                //setSelectiveAuth is for windows only
                if (this.testConfig.TrustedRealm.KDC[0].IsWindows && this.testConfig.TrustType != TrustType.NoTrust)
                {

                    sutController.setSelectiveAuth(this.testConfig.TrustedRealm.RealmName,
                    this.testConfig.TrustedRealm.Admin.Username,
                    this.testConfig.TrustedRealm.Admin.Password,
                    this.testConfig.LocalRealm.RealmName,
                    false);
                }
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
        [Description("This test case is designed to verify if KDC supports Other Organization SID in PAC and will fail if client principal requested is not equal to client user.")]
        public void CrossRealm_OtherOrgSIDinPACFailure()
        {
            base.Logging();

            //setSelectiveAuth is for windows only
            if (this.testConfig.TrustedRealm.KDC[0].IsWindows && this.testConfig.TrustType != TrustType.NoTrust)
            {

                sutController.setSelectiveAuth(this.testConfig.TrustedRealm.RealmName,
                    this.testConfig.TrustedRealm.Admin.Username,
                    this.testConfig.TrustedRealm.Admin.Password,
                    this.testConfig.LocalRealm.RealmName,
                    true);
            }

            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[3].Username,
                this.testConfig.LocalRealm.User[3].Password,
                KerberosAccountType.User,
                testConfig.LocalRealm.KDC[0].IPAddress,
                testConfig.LocalRealm.KDC[0].Port,
                testConfig.TransportType,
                testConfig.SupportedOid);

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

            krbError = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_POLICY,
                krbError.ErrorCode,
                "The KDC MUST perform an ACL check while processing the TGS request...the client principal MUST be that of the client user...if there is a failure in the check, the KDC MUST reject the authentication request with KDC_ERROR_POLICY.");

            //setSelectiveAuth is for windows only
            if (this.testConfig.TrustedRealm.KDC[0].IsWindows && this.testConfig.TrustType != TrustType.NoTrust)
            {
                sutController.setSelectiveAuth(this.testConfig.TrustedRealm.RealmName,
                    this.testConfig.TrustedRealm.Admin.Username,
                    this.testConfig.TrustedRealm.Admin.Password,
                    this.testConfig.LocalRealm.RealmName,
                    false);
            }
        }

        #endregion

        #region Cross Realm PAC Verification
        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.CrossRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]         
        [Feature(Feature.Kile | Feature.Pac)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("Test KERB_VALIDATION_INFO structure in cross realm environment")]
        public void CrossRealm_KERB_VALIDATION_INFO()
        {
            base.Logging();

            //Create kerberos test client and connect
            client = new KerberosTestClient(
                this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[1].Username,
                this.testConfig.LocalRealm.User[1].Password,
                KerberosAccountType.User,
                testConfig.LocalRealm.KDC[0].IPAddress,
                testConfig.LocalRealm.KDC[0].Port,
                testConfig.TransportType,
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

            bool userInfoRetrieved = false; // Only verify user information if retrieved.
            Adapter.PacHelper.commonUserFields commonUserFields = new Adapter.PacHelper.commonUserFields();
            if (this.testConfig.LocalRealm.KDC[0].IsWindows)
            {
                //Don't use the same user account for ldap querys, it will change the current user account attributes
                NetworkCredential cred = new NetworkCredential(this.testConfig.LocalRealm.User[2].Username, this.testConfig.LocalRealm.User[2].Password, this.testConfig.LocalRealm.RealmName);
                commonUserFields = Adapter.PacHelper.GetCommonUserFields(this.testConfig.LocalRealm.RealmName, this.testConfig.LocalRealm.User[1].Username, cred);
                userInfoRetrieved = true;
            }

            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with no PA data.");
            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve preauthentication required error.");

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, this.client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data, paPacOptions.Data });
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create a sequence of PA data.");
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with PA data.");
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve AS response.");
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create and send TGS request
            Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paPacRequest.Data, paPacOptions.Data });
            if (testConfig.TrustType == Adapter.TrustType.Forest)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, options, seqOfPaData2);
            }
            else if (testConfig.TrustType == Adapter.TrustType.Realm)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.KDC[0].DefaultServiceName, options, seqOfPaData2);
            }
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();
            EncryptionKey key = testConfig.QueryKey(
                    this.testConfig.TrustedRealm.KDC[0].DefaultServiceName + "@" + this.testConfig.LocalRealm.RealmName,
                    client.Context.Realm.ToString(),
                    client.Context.SelectedEType);
            tgsResponse.DecryptTicket(key);

            client.ChangeRealm(this.testConfig.TrustedRealm.RealmName,
                this.testConfig.TrustedRealm.KDC[0].IPAddress,
                this.testConfig.TrustedRealm.KDC[0].Port,
                this.testConfig.TransportType);

            //Create and send referral TGS request
            client.Context.Subkey = null;
            client.SendTgsRequest(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, options, seqOfPaData2);
            KerberosTgsResponse refTgsResponse = client.ExpectTgsResponse();

            key = testConfig.QueryKey(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, client.Context.Realm.ToString(), client.Context.SelectedEType);
            refTgsResponse.DecryptTicket(key);

            //Verify PAC
            if (this.testConfig.IsKileImplemented)
            {
                BaseTestSite.Assert.IsNotNull(refTgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(refTgsResponse.TicketEncPart.authorization_data.Elements);
                BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2KPac.");
                KerbValidationInfo kerbValidationInfo = null;
                foreach (var buf in adWin2kPac.Pac.PacInfoBuffers)
                {
                    if (buf is KerbValidationInfo)
                    {
                        kerbValidationInfo = buf as KerbValidationInfo;
                        break;
                    }
                }
                BaseTestSite.Assert.IsNotNull(kerbValidationInfo, "KerbValidationInfo is generated.");
                if (userInfoRetrieved)
                {
                    var flogonTime = (long)kerbValidationInfo.NativeKerbValidationInfo.LogonTime.dwHighDateTime << 32 | (long)kerbValidationInfo.NativeKerbValidationInfo.LogonTime.dwLowDateTime;
                    if (flogonTime != 0x7FFFFFFFFFFFFFFF)
                    {
                        System.DateTime logonTime = System.DateTime.FromFileTime(flogonTime);
                    }
                    BaseTestSite.Assert.AreEqual(commonUserFields.LogonTime, flogonTime, "LogonTime in KERB_VALIDATION_INFO structure should be equal to that in AD.");

                    long fPasswordLastSet = (long)kerbValidationInfo.NativeKerbValidationInfo.PasswordLastSet.dwHighDateTime << 32 | (long)kerbValidationInfo.NativeKerbValidationInfo.PasswordLastSet.dwLowDateTime;
                    if (fPasswordLastSet != 0x7FFFFFFFFFFFFFFF)
                    {
                        System.DateTime passwordLastSet = System.DateTime.FromFileTime(fPasswordLastSet);
                    }
                    BaseTestSite.Assert.AreEqual(commonUserFields.PasswordLastSet, fPasswordLastSet, "PasswordLastSet in KERB_VALIDATION_INFO structure should be equal to that in AD.");
                }

                var effectiveName = Adapter.PacHelper.ConvertUnicode2String(kerbValidationInfo.NativeKerbValidationInfo.EffectiveName.Buffer);
                BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[1].Username.ToLower(), effectiveName.ToLower(), "The EffectiveName field SHOULD be set to the Buffer.SAMPR_USER_ALL_INFORMATION.UserName field of the SamrQueryInformationUser2 response message.");

                string fullName = Adapter.PacHelper.ConvertUnicode2String(kerbValidationInfo.NativeKerbValidationInfo.FullName.Buffer);
                BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[1].Username.ToLower(), fullName.ToLower(), "The FullName field SHOULD be set to the Buffer.SAMPR_USER_ALL_INFORMATION.FullName field of the SamrQueryInformationUser2 response message.");

                string logonScript = Adapter.PacHelper.ConvertUnicode2String(kerbValidationInfo.NativeKerbValidationInfo.LogonScript.Buffer);
                BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[1].DomainAccountInfo.ScriptPath, logonScript, "The LogonScript field SHOULD be set to the Buffer.SAMPR_USER_ALL_INFORMATION.ScriptPath field of the SamrQueryInformationUser2 response message.");

                string profilePath = Adapter.PacHelper.ConvertUnicode2String(kerbValidationInfo.NativeKerbValidationInfo.ProfilePath.Buffer);
                BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[1].DomainAccountInfo.ProfilePath, profilePath, "The ProfilePath field SHOULD be set to the Buffer.SAMPR_USER_ALL_INFORMATION.ProfilePath field of the SamrQueryInformationUser2 response message.");

                string homeDirectory = Adapter.PacHelper.ConvertUnicode2String(kerbValidationInfo.NativeKerbValidationInfo.HomeDirectory.Buffer);
                BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[1].DomainAccountInfo.HomeDirectory, homeDirectory, "The HomeDirectory field SHOULD be set to the Buffer.SAMPR_USER_ALL_INFORMATION.HomeDirectory field of the SamrQueryInformationUser2 response message.");

                string homeDirectoryDrive = Adapter.PacHelper.ConvertUnicode2String(kerbValidationInfo.NativeKerbValidationInfo.HomeDirectoryDrive.Buffer);
                BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[1].DomainAccountInfo.HomeDrive, homeDirectoryDrive, "The HomeDirectoryDrive field SHOULD be set to the Buffer.SAMPR_USER_ALL_INFORMATION.HomeDirectoryDrive field of the SamrQueryInformationUser2 response message.");


                if (userInfoRetrieved)
                {
                    ushort logonCount = kerbValidationInfo.NativeKerbValidationInfo.LogonCount;
                    BaseTestSite.Assert.AreEqual(commonUserFields.LogonCount, logonCount, "The LogonCount field SHOULD be set to the Buffer.SAMPR_USER_ALL_INFORMATION.LogonCount field of the SamrQueryInformationUser2 response message.");

                    ushort badPasswordCount = kerbValidationInfo.NativeKerbValidationInfo.BadPasswordCount;
                    BaseTestSite.Assert.AreEqual(commonUserFields.BadPwdCount, badPasswordCount, "The BadPasswordCount field SHOULD be set to the Buffer.SAMPR_USER_ALL_INFORMATION.BadPasswordCount field of the SamrQueryInformationUser2 response message.");

                    uint userId = kerbValidationInfo.NativeKerbValidationInfo.UserId;
                    BaseTestSite.Assert.AreEqual(commonUserFields.userId, userId, "The UserID field SHOULD be set to the Buffer.SAMPR_USER_ALL_INFORMATION.UserId field of the SamrQueryInformationUser2 response message.");

                    uint primaryGroupId = kerbValidationInfo.NativeKerbValidationInfo.PrimaryGroupId;
                    BaseTestSite.Assert.AreEqual(commonUserFields.primaryGroupId, primaryGroupId, "The PrimaryGroupId field SHOULD be set to the Buffer.SAMPR_USER_ALL_INFORMATION.PrimaryGroupId field of the SamrQueryInformationUser2 response message.");

                    var userAccountControl = kerbValidationInfo.NativeKerbValidationInfo.UserAccountControl;
                    //BaseTestSite.Assert.AreEqual(commonUserFields.userAccountControl, userAccountControl, "The BadPasswordCount field SHOULD be set to the Buffer.SAMPR_USER_ALL_INFORMATION.BadPasswordCount field of the SamrQueryInformationUser2 response message.");

                    //read only, need SUT adapter
                    uint groupCount = kerbValidationInfo.NativeKerbValidationInfo.GroupCount;
                    BaseTestSite.Assert.AreEqual(commonUserFields.groupCount, groupCount, "The GroupCount field SHOULD be set to the Groups.MembershipCount field of the SamrGetGroupsForUser response message.");

                    //read only, need SUT adapter
                    _GROUP_MEMBERSHIP[] groupIds = kerbValidationInfo.NativeKerbValidationInfo.GroupIds;
                    BaseTestSite.Assert.AreEqual(commonUserFields.groupIds.Length, groupIds.Length, "The GroupIds field SHOULD be set to the Groups.Group field of the SamrGetGroupsForUser response message.");
                    BaseTestSite.Assert.IsTrue(groupIds.Select(id => id.RelativeId).OrderBy(id => id).SequenceEqual(commonUserFields.groupIds.OrderBy(id => id)), "The GroupIds field SHOULD be set to the Groups.Group field of the SamrGetGroupsForUser response message.");
                }


                byte[] userSessionKey = kerbValidationInfo.NativeKerbValidationInfo.UserSessionKey;
                byte[] allZero = userSessionKey;
                Array.Clear(allZero, 0, allZero.Length);
                BaseTestSite.Assert.AreEqual(allZero, userSessionKey, "The UserSessionKey field MUST be set to zero.");

                string logonServer = Adapter.PacHelper.ConvertUnicode2String(kerbValidationInfo.NativeKerbValidationInfo.LogonServer.Buffer);
                BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.KDC[0].NetBiosName.Remove(this.testConfig.LocalRealm.KDC[0].NetBiosName.IndexOf("$")), logonServer, "The LogonServer SHOULD be set to NetbiosServerName.");

                string logonDomainName = Adapter.PacHelper.ConvertUnicode2String(kerbValidationInfo.NativeKerbValidationInfo.LogonDomainName.Buffer);
                BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.RealmName.Remove(this.testConfig.LocalRealm.RealmName.IndexOf(".")).ToLower(), logonDomainName.ToLower(), "The LogonDomainName SHOULD be set to NetbiosDomainName.");

                if (userInfoRetrieved)
                {
                    BaseTestSite.Assert.AreEqual(1, kerbValidationInfo.NativeKerbValidationInfo.LogonDomainId.Length, "The number of LogonDomainId should always be 1.");
                    foreach (_RPC_SID element in kerbValidationInfo.NativeKerbValidationInfo.LogonDomainId)
                    {
                        byte[] expectedIdentifierAuthority = new byte[6] { 0, 0, 0, 0, 0, 5 };
                        BaseTestSite.Assert.AreEqual(expectedIdentifierAuthority.Length, element.IdentifierAuthority.Value.Length, "IdentifierAuthority 000005 stands for S-1-5");
                        for (int i = 0; i < expectedIdentifierAuthority.Length; i++)
                        {
                            BaseTestSite.Assert.AreEqual(expectedIdentifierAuthority[i], element.IdentifierAuthority.Value[i], "IdentifierAuthority element {0} should match expected.", i);
                        }
                        uint[] expectedSubAuthority = commonUserFields.domainSid;
                        BaseTestSite.Assert.AreEqual(expectedSubAuthority.Length, element.SubAuthorityCount, "SubAuthorityCount should match expected.");
                        for (int i = 0; i < expectedSubAuthority.Length; i++)
                        {
                            BaseTestSite.Assert.AreEqual(commonUserFields.domainSid[i], element.SubAuthority[i], "SubAuthorityCount element {0} should match expected.");
                        }
                    }
                }

                KERB_VALIDATION_INFO_Reserved1_Values[] reserved1 = kerbValidationInfo.NativeKerbValidationInfo.Reserved1;
                BaseTestSite.Assert.AreEqual(2, reserved1.Length, "The Reserved1 field MUST be set to a two-element array of unsigned 32-bit integers.");
                foreach (KERB_VALIDATION_INFO_Reserved1_Values element in reserved1)
                {
                    BaseTestSite.Assert.AreEqual(KERB_VALIDATION_INFO_Reserved1_Values.V1, element, "Each element of the array MUST be zero.");
                }

                KERB_VALIDATION_INFO_Reserved3_Values[] reserved3 = kerbValidationInfo.NativeKerbValidationInfo.Reserved3;
                BaseTestSite.Assert.AreEqual(7, reserved3.Length, "The Reserved1 field MUST be set to a seven-element array of unsigned 32-bit integers.");
                foreach (KERB_VALIDATION_INFO_Reserved3_Values element in reserved3)
                {
                    BaseTestSite.Assert.AreEqual(KERB_VALIDATION_INFO_Reserved3_Values.V1, element, "Each element of the array MUST be zero.");
                }


                UserFlags_Values userFlags = kerbValidationInfo.NativeKerbValidationInfo.UserFlags;
                BaseTestSite.Assert.AreEqual(UserFlags_Values.ExtraSids, UserFlags_Values.ExtraSids & userFlags, "The D bit SHOULD be set in the UserFlags field if the ExtraSids field is populated and contains additional SIDs and all other bits MUST be set to zero.");

                List<KERB_SID_AND_ATTRIBUTES> extraSidList = new List<KERB_SID_AND_ATTRIBUTES>();

                if (this.testConfig.IsClaimSupported)
                {
                    //The CLAIMS_VALID SID is "S-1-5-21-0-0-0-497"
                    _RPC_SID CLAIMS_VALID = new _RPC_SID();
                    CLAIMS_VALID.Revision = 0x01;
                    CLAIMS_VALID.IdentifierAuthority = new _RPC_SID_IDENTIFIER_AUTHORITY();
                    CLAIMS_VALID.IdentifierAuthority.Value = new byte[] { 0, 0, 0, 0, 0, 5 };
                    CLAIMS_VALID.SubAuthorityCount = 5;
                    CLAIMS_VALID.SubAuthority = new uint[] { 21, 0, 0, 0, 497 };
                    KERB_SID_AND_ATTRIBUTES claimSid = new KERB_SID_AND_ATTRIBUTES();
                    claimSid.Attributes = Attributes_Values.Mandatory | Attributes_Values.EnabledByDefault | Attributes_Values.Enabled;
                    claimSid.SID = new _RPC_SID[1];
                    claimSid.SID[0] = CLAIMS_VALID;

                    extraSidList.Add(claimSid);
                }

                //The AUTHENTICATION_AUTHORITY_ASSERTED_IDENTITY SID is "S-1-18-1"
                _RPC_SID AUTHENTICATION_AUTHORITY_ASSERTED_IDENTITY = new _RPC_SID();
                AUTHENTICATION_AUTHORITY_ASSERTED_IDENTITY.Revision = 0x01;
                AUTHENTICATION_AUTHORITY_ASSERTED_IDENTITY.IdentifierAuthority = new _RPC_SID_IDENTIFIER_AUTHORITY();
                AUTHENTICATION_AUTHORITY_ASSERTED_IDENTITY.IdentifierAuthority.Value = new byte[] { 0, 0, 0, 0, 0, 18 };
                AUTHENTICATION_AUTHORITY_ASSERTED_IDENTITY.SubAuthorityCount = 1;
                AUTHENTICATION_AUTHORITY_ASSERTED_IDENTITY.SubAuthority = new uint[] { 1 };
                KERB_SID_AND_ATTRIBUTES assertedSid = new KERB_SID_AND_ATTRIBUTES();
                assertedSid.Attributes = Attributes_Values.Mandatory | Attributes_Values.EnabledByDefault | Attributes_Values.Enabled;
                assertedSid.SID = new _RPC_SID[1];
                assertedSid.SID[0] = AUTHENTICATION_AUTHORITY_ASSERTED_IDENTITY;
                extraSidList.Add(assertedSid);

                KERB_SID_AND_ATTRIBUTES[] expectedDefaultExtraSids = extraSidList.ToArray();

                uint res = (uint)expectedDefaultExtraSids.Length;
                for (int i = 0; i < expectedDefaultExtraSids.Length; i++)
                {
                    for (int j = 0; j < expectedDefaultExtraSids.Length; j++)
                    {
                        if (Adapter.PacHelper.ExtraSidsAreEqual(expectedDefaultExtraSids[i], kerbValidationInfo.NativeKerbValidationInfo.ExtraSids[j]))
                        {
                            res--;
                        }
                    }
                }

                BaseTestSite.Assert.AreEqual((uint)0, res, "The ExtraSids field contains the pointer to a list which is the list copied from the PAC in the TGT plus a list constructed from the domain local groups, notice domain local groups is equal to 0 in this test case.");
                

                BaseTestSite.Assert.IsNull(kerbValidationInfo.NativeKerbValidationInfo.ResourceGroupDomainSid, "The ResourceGroupDomainSid field MUST be set to zero.");

                BaseTestSite.Assert.AreEqual((uint)0, kerbValidationInfo.NativeKerbValidationInfo.ResourceGroupCount, "The ResourceGroupCount field MUST be set to zero.");

                BaseTestSite.Assert.IsNull(kerbValidationInfo.NativeKerbValidationInfo.ResourceGroupIds, "The ResourceGroupIds field MUST be set to zero.");
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
        [Description("Test PAC_CLIENT_INFO structure in cross realm environment")]
        public void CrossRealm_PAC_CLIENT_INFO()
        {
            base.Logging();

            //Create kerberos test client and connect
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

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Construct Kerberos client for testing."); //Create and send AS request

            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with no PA data.");
            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve preauthentication required error.");

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, this.client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create a sequence of PA data.");
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with PA data.");
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve AS response.");
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create and send TGS request
            if (testConfig.TrustType == Adapter.TrustType.Forest)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, options);
            }
            else if (testConfig.TrustType == Adapter.TrustType.Realm)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.KDC[0].DefaultServiceName, options);
            }
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();
            EncryptionKey key = testConfig.QueryKey(
                    this.testConfig.TrustedRealm.KDC[0].DefaultServiceName + "@" + this.testConfig.LocalRealm.RealmName,
                    client.Context.Realm.ToString(),
                    client.Context.SelectedEType);
            tgsResponse.DecryptTicket(key);

            client.ChangeRealm(this.testConfig.TrustedRealm.RealmName,
           this.testConfig.TrustedRealm.KDC[0].IPAddress,
           this.testConfig.TrustedRealm.KDC[0].Port,
           this.testConfig.TransportType);

            //Create and send referral TGS request
            client.Context.Subkey = null;
            client.SendTgsRequest(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, options);
            KerberosTgsResponse refTgsResponse = client.ExpectTgsResponse();


            key = testConfig.QueryKey(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, client.Context.Realm.ToString(), client.Context.SelectedEType);
            refTgsResponse.DecryptTicket(key);

            BaseTestSite.Assert.IsNotNull(refTgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");


            //Verify PAC
            if (this.testConfig.IsKileImplemented)
            {
                BaseTestSite.Assert.IsNotNull(refTgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(refTgsResponse.TicketEncPart.authorization_data.Elements);
                BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2KPac.");
                PacClientInfo pacClientInfo = null;
                foreach (var buf in adWin2kPac.Pac.PacInfoBuffers)
                {
                    if (buf is PacClientInfo)
                    {
                        pacClientInfo = buf as PacClientInfo;
                        break;
                    }
                }
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
                    "The NameLength field is an unsigned 16-bit integer in little-endian format that specifies the length, in bytes, of the Name field.");
                BaseTestSite.Assert.AreEqual(
                    this.testConfig.LocalRealm.User[1].Username,
                    clientName,
                    "The Name field is an array of 16-bit Unicode characters in little-endian format that contains the client's account name.");
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
        [Description("Test ServerSignature structure in cross realm environment")]
        public void CrossRealm_ServerSignature()
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

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, this.client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create a sequence of PA data.");
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with PA data.");
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve AS response.");
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create and send TGS request
            if (testConfig.TrustType == Adapter.TrustType.Forest)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, options);
            }
            else if (testConfig.TrustType == Adapter.TrustType.Realm)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.KDC[0].DefaultServiceName, options);
            }
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();
            EncryptionKey key = testConfig.QueryKey(
                    this.testConfig.TrustedRealm.KDC[0].DefaultServiceName + "@" + this.testConfig.LocalRealm.RealmName,
                    client.Context.Realm.ToString(),
                    client.Context.SelectedEType);
            tgsResponse.DecryptTicket(key);

            client.ChangeRealm(this.testConfig.TrustedRealm.RealmName,
           this.testConfig.TrustedRealm.KDC[0].IPAddress,
           this.testConfig.TrustedRealm.KDC[0].Port,
           this.testConfig.TransportType);

            //Create and send referral TGS request
            client.Context.Subkey = null;
            client.SendTgsRequest(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, options);
            KerberosTgsResponse refTgsResponse = client.ExpectTgsResponse();

            key = testConfig.QueryKey(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, client.Context.Realm.ToString(), client.Context.SelectedEType);
            refTgsResponse.DecryptTicket(key);

            //Verify PAC
            if (this.testConfig.IsKileImplemented)
            {
                BaseTestSite.Assert.IsNotNull(refTgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(refTgsResponse.TicketEncPart.authorization_data.Elements);
                BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2KPac.");
                PacServerSignature serverSignature = null;
                foreach (var buf in adWin2kPac.Pac.PacInfoBuffers)
                {
                    if (buf is PacServerSignature)
                    {
                        serverSignature = buf as PacServerSignature;
                        break;
                    }
                }
                BaseTestSite.Assert.IsNotNull(serverSignature, "Server Signature is generated.");

                //cannot make a kdc key
                //Fix me, hardcode?
                var KdcKey = testConfig.QueryKey(this.testConfig.LocalRealm.KDC[0].DefaultServiceName, this.testConfig.LocalRealm.RealmName, client.Context.SelectedEType);
                byte[] kdcKey;
                if (KdcKey == null)
                {
                    kdcKey = KeyGenerator.MakeKey(
                       EncryptionType.AES256_CTS_HMAC_SHA1_96,
                       "Password01$",
                       "CONTOSO.COMkrbtgt"
                       );
                }
                else
                    kdcKey = KdcKey.keyvalue.ByteArrayValue;

                bool isServerSignValidate = false;
                bool isKdcSignValidate = false;

                adWin2kPac.Pac.ValidateSign(key.keyvalue.ByteArrayValue, kdcKey, out isServerSignValidate, out isKdcSignValidate);
                BaseTestSite.Assert.IsTrue(isServerSignValidate, "Server Sign doesn't pass validation");

                if ((EncryptionType)refTgsResponse.Response.ticket.enc_part.etype.Value == EncryptionType.RC4_HMAC
                    || (EncryptionType)refTgsResponse.Response.ticket.enc_part.etype.Value == EncryptionType.DES_CBC_MD5)
                {
                    BaseTestSite.Assert.AreEqual(PAC_SIGNATURE_DATA_SignatureType_Values.KERB_CHECKSUM_HMAC_MD5,
                        serverSignature.NativePacSignatureData.SignatureType,
                        "The SignatureType SHOULD be the value ([MS-PAC] section 2.8) corresponding to the cryptographic system used to calculate the checksum.");
                }
                else if ((EncryptionType)refTgsResponse.Response.ticket.enc_part.etype.Value == EncryptionType.AES256_CTS_HMAC_SHA1_96)
                {
                    BaseTestSite.Assert.AreEqual(PAC_SIGNATURE_DATA_SignatureType_Values.HMAC_SHA1_96_AES256,
                        serverSignature.NativePacSignatureData.SignatureType,
                        "The SignatureType SHOULD be the value ([MS-PAC] section 2.8) corresponding to the cryptographic system used to calculate the checksum.");
                }
                else if ((EncryptionType)refTgsResponse.Response.ticket.enc_part.etype.Value == EncryptionType.AES128_CTS_HMAC_SHA1_96)
                {
                    BaseTestSite.Assert.AreEqual(PAC_SIGNATURE_DATA_SignatureType_Values.HMAC_SHA1_96_AES128,
                        serverSignature.NativePacSignatureData.SignatureType,
                        "The SignatureType SHOULD be the value ([MS-PAC] section 2.8) corresponding to the cryptographic system used to calculate the checksum.");
                }
                else
                {
                    throw new Exception("The cryptographic system is none. The checksum operation will fail.");
                }
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
        [Description("Test KdcSignature structure in cross realm environment")]
        public void CrossRealm_KdcSignature()
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

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, this.client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create a sequence of PA data.");
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with PA data.");
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve AS response.");
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create and send TGS request
            if (testConfig.TrustType == Adapter.TrustType.Forest)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, options);
            }
            else if (testConfig.TrustType == Adapter.TrustType.Realm)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.KDC[0].DefaultServiceName, options);
            }
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();
            EncryptionKey key = testConfig.QueryKey(
                    this.testConfig.TrustedRealm.KDC[0].DefaultServiceName + "@" + this.testConfig.LocalRealm.RealmName,
                    client.Context.Realm.ToString(),
                    client.Context.SelectedEType);
            tgsResponse.DecryptTicket(key);

            client.ChangeRealm(this.testConfig.TrustedRealm.RealmName,
                this.testConfig.TrustedRealm.KDC[0].IPAddress,
                this.testConfig.TrustedRealm.KDC[0].Port,
                this.testConfig.TransportType);

            //Create and send referral TGS request
            client.Context.Subkey = null;
            client.SendTgsRequest(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, options);
            KerberosTgsResponse refTgsResponse = client.ExpectTgsResponse();

            key = testConfig.QueryKey(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, client.Context.Realm.ToString(), client.Context.SelectedEType);
            refTgsResponse.DecryptTicket(key);

            if (this.testConfig.IsKileImplemented)
            {
                BaseTestSite.Assert.IsNotNull(refTgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(refTgsResponse.TicketEncPart.authorization_data.Elements);
                BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2KPac.");
                PacKdcSignature kdcSignature = null;
                foreach (var buf in adWin2kPac.Pac.PacInfoBuffers)
                {
                    if (buf is PacKdcSignature)
                    {
                        kdcSignature = buf as PacKdcSignature;
                        break;
                    }
                }
                BaseTestSite.Assert.IsNotNull(kdcSignature, "KDC Signature is generated.");
                //kdcSignature.NativePacSignatureData.SignatureType;
                //kdcSignature.NativePacSignatureData.Signature;
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
        [Description("Test UPN_DNS_INFO structure in cross realm environment")]
        public void CrossRealm_UPN_DNS_INFO()
        {
            base.Logging();

            //Create kerberos test client and connect
            client = new KerberosTestClient(
                this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[5].Username,
                this.testConfig.LocalRealm.User[5].Password,
                KerberosAccountType.User,
                testConfig.LocalRealm.KDC[0].IPAddress,
                testConfig.LocalRealm.KDC[0].Port,
                testConfig.TransportType,
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

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, this.client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create a sequence of PA data.");
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with PA data.");
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve AS response.");
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create and send TGS request
            if (testConfig.TrustType == Adapter.TrustType.Forest)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, options);
            }
            else if (testConfig.TrustType == Adapter.TrustType.Realm)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.KDC[0].DefaultServiceName, options);
            }
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();
            EncryptionKey key = testConfig.QueryKey(
                    this.testConfig.TrustedRealm.KDC[0].DefaultServiceName + "@" + this.testConfig.LocalRealm.RealmName,
                    client.Context.Realm.ToString(),
                    client.Context.SelectedEType);
            tgsResponse.DecryptTicket(key);

            client.ChangeRealm(this.testConfig.TrustedRealm.RealmName,
            this.testConfig.TrustedRealm.KDC[0].IPAddress,
            this.testConfig.TrustedRealm.KDC[0].Port,
            this.testConfig.TransportType);

            //Create and send referral TGS request
            client.Context.Subkey = null;
            client.SendTgsRequest(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, options);
            KerberosTgsResponse refTgsResponse = client.ExpectTgsResponse();

            key = testConfig.QueryKey(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, client.Context.Realm.ToString(), client.Context.SelectedEType);
            refTgsResponse.DecryptTicket(key);
            BaseTestSite.Assert.IsNotNull(refTgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");

            //Verify PAC
            if (this.testConfig.IsKileImplemented)
            {
                BaseTestSite.Assert.IsNotNull(refTgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(refTgsResponse.TicketEncPart.authorization_data.Elements);
                BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2KPac.");
                UpnDnsInfo upnDnsInfo = null;
                foreach (var buf in adWin2kPac.Pac.PacInfoBuffers)
                {
                    if (buf is UpnDnsInfo)
                    {
                        upnDnsInfo = buf as UpnDnsInfo;
                        break;
                    }
                }
                BaseTestSite.Assert.IsNotNull(upnDnsInfo, "UPN_DNS_INFO is generated.");
                BaseTestSite.Assert.AreEqual(upnDnsInfo.Upn.Length * 2,
                    upnDnsInfo.NativeUpnDnsInfo.UpnLength,
                    "The UpnLength field SHOULD be the length of the UPN field, in bytes.");
                //upnDnsInfo.NativeUpnDnsInfo.UpnOffset;
                BaseTestSite.Assert.AreEqual(upnDnsInfo.DnsDomain.Length * 2,
                    upnDnsInfo.NativeUpnDnsInfo.DnsDomainNameLength,
                    "The DnsDomainNameLength field SHOULD be the length of the DnsDomainName field, in bytes.");
                //upnDnsInfo.NativeUpnDnsInfo.DnsDomainNameOffset; ;
                BaseTestSite.Assert.AreEqual(UPN_DNS_INFO_Flags_Values.NoUpnAttribute,
                    upnDnsInfo.NativeUpnDnsInfo.Flags & UPN_DNS_INFO_Flags_Values.NoUpnAttribute,
                    "The Flags field SHOULD set the U bit if the user account object does not have the userPrincipalName attribute ([MS-ADA3] section 2.349) set.");
            }
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.CrossRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.Claim)]
        [Feature(Feature.Kile | Feature.Pac | Feature.Claim)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("Test PAC_DEVICE_INFO structure in cross realm environment")]
        public void CrossRealm_PAC_DEVICE_INFO()
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
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, this.client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });

            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();

            //Create and send TGS request

            client.SendTgsRequest(this.testConfig.TrustedRealm.KDC[0].DefaultServiceName, options);
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();

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

            // AS_REQ and AS_REP using device principal
            timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data });
            client.SendAsRequest(options, seqOfPaData);
            asResponse = client.ExpectAsResponse();

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
            PaFxFastReq paFxFastReq = new PaFxFastReq(null);
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
            paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> outerSeqPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paPacRequest.Data, paPacOptions.Data });
            client.SendAsRequestWithFast(options, seqOfPaData3, outerSeqPaData, subkey, fastOptions, apOptions);
            KerberosAsResponse userKrbAsRep = client.ExpectAsResponse();
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

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.CrossRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.Claim)]
        [Feature(Feature.Kile | Feature.Pac | Feature.Claim)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("Test PAC_DEVICE_CLAIMS_INFO structure in cross realm environment")]
        public void CrossRealm_PAC_DEVICE_CLAIMS_INFO()
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
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, this.client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });

            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();

            //Create and send TGS request

            client.SendTgsRequest(this.testConfig.TrustedRealm.KDC[0].DefaultServiceName, options);
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();

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

            // AS_REQ and AS_REP using device principal
            timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data });
            client.SendAsRequest(options, seqOfPaData);
            asResponse = client.ExpectAsResponse();

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
            paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> outerSeqPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paPacRequest.Data, paPacOptions.Data });
            client.SendAsRequestWithFast(options, seqOfPaData3, outerSeqPaData, subkey, fastOptions, apOptions);
            KerberosAsResponse userKrbAsRep = client.ExpectAsResponse();
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

            client.ChangeRealm(this.testConfig.TrustedRealm.RealmName,
            this.testConfig.TrustedRealm.KDC[0].IPAddress,
            this.testConfig.TrustedRealm.KDC[0].Port,
            this.testConfig.TransportType);

            //Create and send referal TGS request            
            client.Context.ArmorTicket = referralComputerTicket;
            client.Context.ArmorSessionKey = referralComputerTicket.SessionKey;
            client.SendTgsRequestWithExplicitFast(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, options, null, null, subkey, fastOptions, apOptions);
            KerberosTgsResponse refTgsResponse = client.ExpectTgsResponse(KeyUsageNumber.TGS_REP_encrypted_part_subkey);

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
        #endregion

        #region Cross Realm Smb2 AP
        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.CrossRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.Smb2)]
        [Description("Test Forwardable flag in cross realm environment")]
        public void ForwardableTicket_Smb2()
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
            KdcOptions options = KdcOptions.FORWARDABLE;
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
            options = KdcOptions.FORWARDED | KdcOptions.FORWARDABLE;
            if (this.testConfig.TrustType == Adapter.TrustType.Forest)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, options);
            }
            else
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.KDC[0].DefaultServiceName, options);
            }
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();
            EncryptionKey key = testConfig.QueryKey(
                    this.testConfig.TrustedRealm.KDC[0].DefaultServiceName + "@" + this.testConfig.LocalRealm.RealmName,
                    client.Context.Realm.ToString(),
                    client.Context.SelectedEType);
            tgsResponse.DecryptTicket(key);

            KdcOptions returnOptions = (KdcOptions)KerberosUtility.ConvertFlags2Int(tgsResponse.EncPart.flags.ByteArrayValue);
            BaseTestSite.Assert.AreEqual(KdcOptions.FORWARDABLE,
                returnOptions & KdcOptions.FORWARDABLE,
                ".");
            BaseTestSite.Assert.AreEqual(KdcOptions.OK_AS_DELEGATE,
                returnOptions & KdcOptions.OK_AS_DELEGATE,
                ".");
            BaseTestSite.Assert.AreEqual(KdcOptions.FORWARDED,
                returnOptions & KdcOptions.FORWARDED,
                ".");

            //Change realm
            client.ChangeRealm(this.testConfig.TrustedRealm.RealmName,
                this.testConfig.TrustedRealm.KDC[0].IPAddress,
                this.testConfig.TrustedRealm.KDC[0].Port,
                this.testConfig.TransportType);

            //Create and send referral TGS request
            options = KdcOptions.FORWARDABLE | KdcOptions.FORWARDED;
            client.SendTgsRequest(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, options);
            KerberosTgsResponse refTgsResponse = client.ExpectTgsResponse();

            returnOptions = (KdcOptions)KerberosUtility.ConvertFlags2Int(tgsResponse.EncPart.flags.ByteArrayValue);
            BaseTestSite.Assert.AreEqual(KdcOptions.FORWARDABLE,
                returnOptions & KdcOptions.FORWARDABLE,
                ".");
            BaseTestSite.Assert.AreEqual(KdcOptions.OK_AS_DELEGATE,
                returnOptions & KdcOptions.OK_AS_DELEGATE,
                ".");
            BaseTestSite.Assert.AreEqual(KdcOptions.FORWARDED,
                returnOptions & KdcOptions.FORWARDED,
                ".");

        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.CrossRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Kile | Feature.Pac)]
        [ApplicationServer(ApplicationServer.Smb2)]
        [Description("Test PAC in referral ticket")]
        public void ReferralTicketWithPac_Smb2()
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

            //Change realm
            client.ChangeRealm(this.testConfig.TrustedRealm.RealmName,
                this.testConfig.TrustedRealm.KDC[0].IPAddress,
                this.testConfig.TrustedRealm.KDC[0].Port,
                this.testConfig.TransportType);

            //Create and send referral TGS request
            client.SendTgsRequest(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, options);
            KerberosTgsResponse refTgsResponse = client.ExpectTgsResponse();

            key = testConfig.QueryKey(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, client.Context.Realm.ToString(), client.Context.SelectedEType);
            refTgsResponse.DecryptTicket(key);

            if (this.testConfig.IsKileImplemented)
            {
                BaseTestSite.Assert.IsNotNull(refTgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(refTgsResponse.TicketEncPart.authorization_data.Elements);
                BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2KPac.");
            }

            AuthorizationData data = null;
            EncryptionKey subkey = KerberosUtility.GenerateKey(client.Context.SessionKey);
            byte[] token = client.CreateGssApiToken(ApOptions.MutualRequired,
                data,
                subkey,
                ChecksumFlags.GSS_C_MUTUAL_FLAG | ChecksumFlags.GSS_C_INTEG_FLAG);

            byte[] repToken = SendAndRecieveSmb2Ap(this.testConfig.TrustedRealm.FileServer[0], token);
            KerberosApResponse apRep = client.GetApResponseFromToken(repToken);
        }


        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.CrossRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Kile | Feature.Pac)]
        [ApplicationServer(ApplicationServer.Smb2)]
        [Description("KILE should include PAC in referral ticket by default")]
        public void ReferralTicketIncludePacByDefault_Smb2()
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
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, this.client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data });
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

            //Change realm
            client.ChangeRealm(this.testConfig.TrustedRealm.RealmName,
                this.testConfig.TrustedRealm.KDC[0].IPAddress,
                this.testConfig.TrustedRealm.KDC[0].Port,
                this.testConfig.TransportType);

            //Create and send referral TGS request
            client.SendTgsRequest(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, options);
            KerberosTgsResponse refTgsResponse = client.ExpectTgsResponse();

            key = testConfig.QueryKey(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, client.Context.Realm.ToString(), client.Context.SelectedEType);
            refTgsResponse.DecryptTicket(key);

            if (this.testConfig.IsKileImplemented)
            {
                BaseTestSite.Assert.IsNotNull(refTgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(refTgsResponse.TicketEncPart.authorization_data.Elements);
                BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2KPac.");
            }

            AuthorizationData data = null;
            EncryptionKey subkey = KerberosUtility.GenerateKey(client.Context.SessionKey);
            byte[] token = client.CreateGssApiToken(ApOptions.MutualRequired,
                data,
                subkey,
                ChecksumFlags.GSS_C_MUTUAL_FLAG | ChecksumFlags.GSS_C_INTEG_FLAG);

            byte[] repToken = SendAndRecieveSmb2Ap(this.testConfig.TrustedRealm.FileServer[0], token);
            KerberosApResponse apRep = client.GetApResponseFromToken(repToken);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.CrossRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.Claim)]
        [Feature(Feature.Kile | Feature.Pac | Feature.Claim)]
        [ApplicationServer(ApplicationServer.Smb2)]
        [Description("Test PAC and claim in referral ticket")]
        public void ReferralTicketWithPacAndClaim_Smb2()
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

            //Create and send AS request
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);
            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, this.client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data, paPacOptions.Data });
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

            //Change realm
            client.ChangeRealm(this.testConfig.TrustedRealm.RealmName,
                this.testConfig.TrustedRealm.KDC[0].IPAddress,
                this.testConfig.TrustedRealm.KDC[0].Port,
                this.testConfig.TransportType);

            //Create and send referral TGS request
            client.SendTgsRequest(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, options);
            KerberosTgsResponse refTgsResponse = client.ExpectTgsResponse();

            EncryptionKey key = testConfig.QueryKey(this.testConfig.TrustedRealm.FileServer[0].Smb2ServiceName, client.Context.Realm.ToString(), client.Context.SelectedEType);
            refTgsResponse.DecryptTicket(key);

            if (this.testConfig.IsKileImplemented)
            {
                BaseTestSite.Assert.IsNotNull(refTgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(refTgsResponse.TicketEncPart.authorization_data.Elements);
                BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2KPac.");

                if (this.testConfig.IsClaimSupported)
                {
                    ClientClaimsInfo clienClaimsInfo = null;
                    foreach (var buf in adWin2kPac.Pac.PacInfoBuffers)
                    {
                        if (buf is ClientClaimsInfo)
                            clienClaimsInfo = buf as ClientClaimsInfo;
                    }
                    BaseTestSite.Assert.IsNotNull(clienClaimsInfo, "The AdWin2KPac contains ClientClaimsInfo.");
                }
            }

            AuthorizationData data = null;
            EncryptionKey subkey = KerberosUtility.GenerateKey(client.Context.SessionKey);
            byte[] token = client.CreateGssApiToken(ApOptions.MutualRequired,
                data,
                subkey,
                ChecksumFlags.GSS_C_MUTUAL_FLAG | ChecksumFlags.GSS_C_INTEG_FLAG);

            byte[] repToken = SendAndRecieveSmb2Ap(this.testConfig.TrustedRealm.FileServer[0], token);
            KerberosApResponse apRep = client.GetApResponseFromToken(repToken);
        }
        #endregion

        #region Cross Realm Web AP
        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.HttpAp)]
        [TestCategory(TestCategories.CrossRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.Http)]
        [Description("Test Forwardable flag in cross realm environment")]
        public void ForwardableTicket_Http()
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
            KdcOptions options = KdcOptions.FORWARDABLE;
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
            options = KdcOptions.FORWARDED | KdcOptions.FORWARDABLE;
            if (this.testConfig.TrustType == Adapter.TrustType.Forest)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.WebServer[0].HttpServiceName, options);
            }
            else
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.KDC[0].DefaultServiceName, options);
            }
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();
            EncryptionKey key = testConfig.QueryKey(
                    this.testConfig.TrustedRealm.KDC[0].DefaultServiceName + "@" + this.testConfig.LocalRealm.RealmName,
                    client.Context.Realm.ToString(),
                    client.Context.SelectedEType);
            tgsResponse.DecryptTicket(key);

            KdcOptions returnOptions = (KdcOptions)KerberosUtility.ConvertFlags2Int(tgsResponse.EncPart.flags.ByteArrayValue);
            BaseTestSite.Assert.AreEqual(KdcOptions.FORWARDABLE,
                returnOptions & KdcOptions.FORWARDABLE,
                ".");
            BaseTestSite.Assert.AreEqual(KdcOptions.OK_AS_DELEGATE,
                returnOptions & KdcOptions.OK_AS_DELEGATE,
                ".");
            BaseTestSite.Assert.AreEqual(KdcOptions.FORWARDED,
                returnOptions & KdcOptions.FORWARDED,
                ".");

            //Change realm
            client.ChangeRealm(this.testConfig.TrustedRealm.RealmName,
                this.testConfig.TrustedRealm.KDC[0].IPAddress,
                this.testConfig.TrustedRealm.KDC[0].Port,
                this.testConfig.TransportType);

            //Create and send referral TGS request
            options = KdcOptions.FORWARDABLE | KdcOptions.FORWARDED;
            client.SendTgsRequest(this.testConfig.TrustedRealm.WebServer[0].HttpServiceName, options);
            KerberosTgsResponse refTgsResponse = client.ExpectTgsResponse();

            returnOptions = (KdcOptions)KerberosUtility.ConvertFlags2Int(tgsResponse.EncPart.flags.ByteArrayValue);
            BaseTestSite.Assert.AreEqual(KdcOptions.FORWARDABLE,
                returnOptions & KdcOptions.FORWARDABLE,
                ".");
            BaseTestSite.Assert.AreEqual(KdcOptions.OK_AS_DELEGATE,
                returnOptions & KdcOptions.OK_AS_DELEGATE,
                ".");
            BaseTestSite.Assert.AreEqual(KdcOptions.FORWARDED,
                returnOptions & KdcOptions.FORWARDED,
                ".");

        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.HttpAp)]
        [TestCategory(TestCategories.CrossRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Kile | Feature.Pac)]
        [ApplicationServer(ApplicationServer.Http)]
        [Description("Test PAC in referral ticket")]
        public void ReferralTicketWithPac_Http()
        {
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
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, this.client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();

            //Create and send TGS request            
            if (this.testConfig.TrustType == Adapter.TrustType.Forest)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.WebServer[0].HttpServiceName, options);
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

            //Change realm
            client.ChangeRealm(this.testConfig.TrustedRealm.RealmName,
                this.testConfig.TrustedRealm.KDC[0].IPAddress,
                this.testConfig.TrustedRealm.KDC[0].Port,
                this.testConfig.TransportType);

            //Create and send referral TGS request
            client.SendTgsRequest(this.testConfig.TrustedRealm.WebServer[0].HttpServiceName, options);
            KerberosTgsResponse refTgsResponse = client.ExpectTgsResponse();

            key = testConfig.QueryKey(this.testConfig.TrustedRealm.WebServer[0].HttpServiceName, client.Context.Realm.ToString(), client.Context.SelectedEType);
            refTgsResponse.DecryptTicket(key);

            if (this.testConfig.IsKileImplemented)
            {
                BaseTestSite.Assert.IsNotNull(refTgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(refTgsResponse.TicketEncPart.authorization_data.Elements);
                BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2KPac.");
            }

            AuthorizationData data = null;
            EncryptionKey subkey = KerberosUtility.GenerateKey(client.Context.SessionKey);
            byte[] token = client.CreateGssApiToken(ApOptions.MutualRequired,
                data,
                subkey,
                ChecksumFlags.GSS_C_MUTUAL_FLAG | ChecksumFlags.GSS_C_INTEG_FLAG);

            byte[] repToken = SendAndRecieveHttpAp(this.testConfig.TrustedRealm.WebServer[0], token);
            KerberosApResponse apRep = client.GetApResponseFromToken(repToken);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.HttpAp)]
        [TestCategory(TestCategories.CrossRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Kile | Feature.Pac)]
        [ApplicationServer(ApplicationServer.Http)]
        [Description("KILE should include PAC in referral ticket by default")]
        public void ReferralTicketIncludePacByDefault_Http()
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
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, this.client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data });
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();

            //Create and send TGS request
            if (this.testConfig.TrustType == Adapter.TrustType.Forest)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.WebServer[0].HttpServiceName, options);
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

            //Change realm
            client.ChangeRealm(this.testConfig.TrustedRealm.RealmName,
                this.testConfig.TrustedRealm.KDC[0].IPAddress,
                this.testConfig.TrustedRealm.KDC[0].Port,
                this.testConfig.TransportType);

            //Create and send referral TGS request
            client.SendTgsRequest(this.testConfig.TrustedRealm.WebServer[0].HttpServiceName, options);
            KerberosTgsResponse refTgsResponse = client.ExpectTgsResponse();

            key = testConfig.QueryKey(this.testConfig.TrustedRealm.WebServer[0].HttpServiceName, client.Context.Realm.ToString(), client.Context.SelectedEType);
            refTgsResponse.DecryptTicket(key);

            if (this.testConfig.IsKileImplemented)
            {
                BaseTestSite.Assert.IsNotNull(refTgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(refTgsResponse.TicketEncPart.authorization_data.Elements);
                BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2KPac.");
            }

            AuthorizationData data = null;
            EncryptionKey subkey = KerberosUtility.GenerateKey(client.Context.SessionKey);
            byte[] token = client.CreateGssApiToken(ApOptions.MutualRequired,
                data,
                subkey,
                ChecksumFlags.GSS_C_MUTUAL_FLAG | ChecksumFlags.GSS_C_INTEG_FLAG);

            byte[] repToken = SendAndRecieveHttpAp(this.testConfig.TrustedRealm.WebServer[0], token);
            KerberosApResponse apRep = client.GetApResponseFromToken(repToken);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.HttpAp)]
        [TestCategory(TestCategories.CrossRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.Claim)]
        [Feature(Feature.Kile | Feature.Pac | Feature.Claim)]
        [ApplicationServer(ApplicationServer.Http)]
        [Description("Test PAC and claim in referral ticket")]
        public void ReferralTicketWithPacAndClaim_Http()
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

            //Create and send AS request
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);
            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, this.client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data, paPacOptions.Data });
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();

            //Create and send TGS request
            if (this.testConfig.TrustType == Adapter.TrustType.Forest)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.WebServer[0].HttpServiceName, options);
            }
            else if (this.testConfig.TrustType == Adapter.TrustType.Realm)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.KDC[0].DefaultServiceName, options);
            }
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();

            //Change realm
            client.ChangeRealm(this.testConfig.TrustedRealm.RealmName,
                this.testConfig.TrustedRealm.KDC[0].IPAddress,
                this.testConfig.TrustedRealm.KDC[0].Port,
                this.testConfig.TransportType);

            //Create and send referral TGS request
            client.SendTgsRequest(this.testConfig.TrustedRealm.WebServer[0].HttpServiceName, options);
            KerberosTgsResponse refTgsResponse = client.ExpectTgsResponse();

            EncryptionKey key = testConfig.QueryKey(this.testConfig.TrustedRealm.WebServer[0].HttpServiceName, client.Context.Realm.ToString(), client.Context.SelectedEType);
            refTgsResponse.DecryptTicket(key);

            if (this.testConfig.IsKileImplemented)
            {
                BaseTestSite.Assert.IsNotNull(refTgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(refTgsResponse.TicketEncPart.authorization_data.Elements);
                BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2KPac.");

                if (this.testConfig.IsClaimSupported)
                {
                    ClientClaimsInfo clienClaimsInfo = null;
                    foreach (var buf in adWin2kPac.Pac.PacInfoBuffers)
                    {
                        if (buf is ClientClaimsInfo)
                            clienClaimsInfo = buf as ClientClaimsInfo;
                    }
                    BaseTestSite.Assert.IsNotNull(clienClaimsInfo, "The AdWin2KPac contains ClientClaimsInfo.");
                }
            }

            AuthorizationData data = null;
            EncryptionKey subkey = KerberosUtility.GenerateKey(client.Context.SessionKey);
            byte[] token = client.CreateGssApiToken(ApOptions.MutualRequired,
                data,
                subkey,
                ChecksumFlags.GSS_C_MUTUAL_FLAG | ChecksumFlags.GSS_C_INTEG_FLAG);

            byte[] repToken = SendAndRecieveHttpAp(this.testConfig.TrustedRealm.WebServer[0], token);
            KerberosApResponse apRep = client.GetApResponseFromToken(repToken);
        }
        #endregion

        #region Cross Realm Ldap AP
        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.LdapAp)]
        [TestCategory(TestCategories.CrossRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.Ldap)]
        [Description("Test Forwardable flag in cross realm environment")]
        public void ForwardableTicket_Ldap()
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
            KdcOptions options = KdcOptions.FORWARDABLE;
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
            options = KdcOptions.FORWARDED | KdcOptions.FORWARDABLE;
            if (this.testConfig.TrustType == Adapter.TrustType.Forest)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.LdapServer[0].LdapServiceName, options);
            }
            else
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.KDC[0].DefaultServiceName, options);
            }
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();
            EncryptionKey key = testConfig.QueryKey(
                    this.testConfig.TrustedRealm.KDC[0].DefaultServiceName + "@" + this.testConfig.LocalRealm.RealmName,
                    client.Context.Realm.ToString(),
                    client.Context.SelectedEType);
            tgsResponse.DecryptTicket(key);

            KdcOptions returnOptions = (KdcOptions)KerberosUtility.ConvertFlags2Int(tgsResponse.EncPart.flags.ByteArrayValue);
            BaseTestSite.Assert.AreEqual(KdcOptions.FORWARDABLE,
                returnOptions & KdcOptions.FORWARDABLE,
                ".");
            BaseTestSite.Assert.AreEqual(KdcOptions.OK_AS_DELEGATE,
                returnOptions & KdcOptions.OK_AS_DELEGATE,
                ".");
            BaseTestSite.Assert.AreEqual(KdcOptions.FORWARDED,
                returnOptions & KdcOptions.FORWARDED,
                ".");


            //Change realm
            client.ChangeRealm(this.testConfig.TrustedRealm.RealmName,
                this.testConfig.TrustedRealm.KDC[0].IPAddress,
                this.testConfig.TrustedRealm.KDC[0].Port,
                this.testConfig.TransportType);

            //Create and send referral TGS request
            options = KdcOptions.FORWARDABLE | KdcOptions.FORWARDED;
            client.SendTgsRequest(this.testConfig.TrustedRealm.LdapServer[0].LdapServiceName, options);
            KerberosTgsResponse refTgsResponse = client.ExpectTgsResponse();

            returnOptions = (KdcOptions)KerberosUtility.ConvertFlags2Int(tgsResponse.EncPart.flags.ByteArrayValue);
            BaseTestSite.Assert.AreEqual(KdcOptions.FORWARDABLE,
                returnOptions & KdcOptions.FORWARDABLE,
                ".");
            BaseTestSite.Assert.AreEqual(KdcOptions.OK_AS_DELEGATE,
                returnOptions & KdcOptions.OK_AS_DELEGATE,
                ".");
            BaseTestSite.Assert.AreEqual(KdcOptions.FORWARDED,
                returnOptions & KdcOptions.FORWARDED,
                ".");

        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.LdapAp)]
        [TestCategory(TestCategories.CrossRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Kile | Feature.Pac)]
        [ApplicationServer(ApplicationServer.Ldap)]
        [Description("Test PAC in referral ticket")]
        public void ReferralTicketWithPac_Ldap()
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
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, this.client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();

            //Create and send TGS request            
            if (this.testConfig.TrustType == Adapter.TrustType.Forest)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.LdapServer[0].LdapServiceName, options);
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

            //Change realm
            client.ChangeRealm(this.testConfig.TrustedRealm.RealmName,
                this.testConfig.TrustedRealm.KDC[0].IPAddress,
                this.testConfig.TrustedRealm.KDC[0].Port,
                this.testConfig.TransportType);

            //Create and send referral TGS request
            client.SendTgsRequest(this.testConfig.TrustedRealm.LdapServer[0].LdapServiceName, options);
            KerberosTgsResponse refTgsResponse = client.ExpectTgsResponse();

            //EncryptionKey key = testConfig.QueryKey(this.testConfig.TrustedRealm.LdapServer[0].LdapServiceName, client.Context.Realm.ToString(), client.Context.SelectedEType);
            //refTgsResponse.DecryptTicket(key);

            //BaseTestSite.Assert.IsNotNull(refTgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");

            //if (this.testConfig.IsKileImplemented)
            //{
            //    AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(refTgsResponse.TicketEncPart.authorization_data.elements);
            //    BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2KPac.");
            //}

            AuthorizationData data = null;
            EncryptionKey subkey = KerberosUtility.GenerateKey(client.Context.SessionKey);
            byte[] token = client.CreateGssApiToken(ApOptions.MutualRequired,
                data,
                subkey,
                ChecksumFlags.GSS_C_MUTUAL_FLAG | ChecksumFlags.GSS_C_INTEG_FLAG,
                this.testConfig.TrustedRealm.LdapServer[0].GssToken);

            byte[] repToken = SendAndRecieveLdapAp(this.testConfig.TrustedRealm.LdapServer[0], token, this.testConfig.TrustedRealm.LdapServer[0].GssToken);
            KerberosApResponse apResponse = client.GetApResponseFromToken(repToken, this.testConfig.TrustedRealm.LdapServer[0].GssToken);
        }


        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.LdapAp)]
        [TestCategory(TestCategories.CrossRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Kile | Feature.Pac)]
        [ApplicationServer(ApplicationServer.Ldap)]
        [Description("KILE should include PAC in referral ticket by default")]
        public void ReferralTicketIncludePacByDefault_Ldap()
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
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, this.client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data });
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();

            //Create and send TGS request
            if (this.testConfig.TrustType == Adapter.TrustType.Forest)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.LdapServer[0].LdapServiceName, options);
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

            //Change realm
            client.ChangeRealm(this.testConfig.TrustedRealm.RealmName,
                this.testConfig.TrustedRealm.KDC[0].IPAddress,
                this.testConfig.TrustedRealm.KDC[0].Port,
                this.testConfig.TransportType);

            //Create and send referral TGS request
            client.SendTgsRequest(this.testConfig.TrustedRealm.LdapServer[0].LdapServiceName, options);
            KerberosTgsResponse refTgsResponse = client.ExpectTgsResponse();

            //EncryptionKey key = testConfig.QueryKey(this.testConfig.TrustedRealm.LdapServer[0].LdapServiceName, client.Context.Realm.ToString(), client.Context.SelectedEType);
            //refTgsResponse.DecryptTicket(key);
            //BaseTestSite.Assert.IsNotNull(refTgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");

            //if (this.testConfig.IsKileImplemented)
            //{
            //    AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(refTgsResponse.TicketEncPart.authorization_data.elements);
            //    BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2KPac.");
            //}

            AuthorizationData data = null;
            EncryptionKey subkey = KerberosUtility.GenerateKey(client.Context.SessionKey);
            byte[] token = client.CreateGssApiToken(ApOptions.MutualRequired,
                data,
                subkey,
                ChecksumFlags.GSS_C_MUTUAL_FLAG | ChecksumFlags.GSS_C_INTEG_FLAG,
                this.testConfig.TrustedRealm.LdapServer[0].GssToken);

            byte[] repToken = SendAndRecieveLdapAp(this.testConfig.TrustedRealm.LdapServer[0], token, this.testConfig.TrustedRealm.LdapServer[0].GssToken);
            KerberosApResponse apResponse = client.GetApResponseFromToken(repToken, this.testConfig.TrustedRealm.LdapServer[0].GssToken);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.LdapAp)]
        [TestCategory(TestCategories.CrossRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.Claim)]
        [Feature(Feature.Kile | Feature.Pac)]
        [ApplicationServer(ApplicationServer.Ldap)]
        [Description("Test PAC and claim in referral ticket")]
        public void ReferralTicketWithPacAndClaim_Ldap()
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

            //Create and send AS request
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);
            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, this.client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data, paPacOptions.Data });
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();

            //Create and send TGS request
            if (this.testConfig.TrustType == Adapter.TrustType.Forest)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.LdapServer[0].LdapServiceName, options);
            }
            else if (this.testConfig.TrustType == Adapter.TrustType.Realm)
            {
                client.SendTgsRequest(this.testConfig.TrustedRealm.KDC[0].DefaultServiceName, options);
            }
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();

            //Change realm
            client.ChangeRealm(this.testConfig.TrustedRealm.RealmName,
                this.testConfig.TrustedRealm.KDC[0].IPAddress,
                this.testConfig.TrustedRealm.KDC[0].Port,
                this.testConfig.TransportType);

            //Create and send referral TGS request
            client.SendTgsRequest(this.testConfig.TrustedRealm.LdapServer[0].LdapServiceName, options);
            KerberosTgsResponse refTgsResponse = client.ExpectTgsResponse();

            //EncryptionKey key = testConfig.QueryKey(this.testConfig.TrustedRealm.LdapServer[0].LdapServiceName, client.Context.Realm.ToString(), client.Context.SelectedEType);
            //refTgsResponse.DecryptTicket(key);
            //BaseTestSite.Assert.IsNotNull(refTgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");

            //if (this.testConfig.IsKileImplemented)
            //{
            //    AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(refTgsResponse.TicketEncPart.authorization_data.elements);
            //    BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2KPac.");

            //    if (this.testConfig.IsClaimSupported)
            //    {
            //        ClientClaimsInfo clienClaimsInfo = null;
            //        foreach (var buf in adWin2kPac.Pac.PacInfoBuffers)
            //        {
            //            if (buf is ClientClaimsInfo)
            //                clienClaimsInfo = buf as ClientClaimsInfo;
            //        }
            //        BaseTestSite.Assert.IsNotNull(clienClaimsInfo, "The AdWin2KPac contains ClientClaimsInfo.");
            //    }
            //}

            AuthorizationData data = null;
            EncryptionKey subkey = KerberosUtility.GenerateKey(client.Context.SessionKey);
            byte[] token = client.CreateGssApiToken(ApOptions.MutualRequired,
                data,
                subkey,
                ChecksumFlags.GSS_C_MUTUAL_FLAG | ChecksumFlags.GSS_C_INTEG_FLAG,
                this.testConfig.TrustedRealm.LdapServer[0].GssToken);

            byte[] repToken = SendAndRecieveLdapAp(this.testConfig.TrustedRealm.LdapServer[0], token, this.testConfig.TrustedRealm.LdapServer[0].GssToken);
            KerberosApResponse apResponse = client.GetApResponseFromToken(repToken, this.testConfig.LocalRealm.LdapServer[0].GssToken);
        }
        #endregion

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.CrossRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify if the OK-AS-DELEGATE ticket flag is not set when the TRUST_ATTRIBUTE_CROSS_ORGANIZATION_NO_TGT_DELEGATION flag is set.")]
        public void CrossRealm_ReferralTgs_NoOkAsDelegateFlag()
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

            int flags = KerberosUtility.ConvertFlags2Int(refTgsResponse.EncPart.flags.ByteArrayValue);
            BaseTestSite.Assert.AreEqual((EncTicketFlags)0,
                EncTicketFlags.OK_AS_DELEGATE & (EncTicketFlags)flags,
                "If the TRUST_ATTRIBUTE_CROSS_ORGANIZATION_NO_TGT_DELEGATION flag is set in the trustAttributes field ([MS-ADTS] section 6.1.6.7.9)," +
                "the KDC MUST return a ticket with the ok-as-delegate flag not set in TicketFlags.");

        }
    }
}
