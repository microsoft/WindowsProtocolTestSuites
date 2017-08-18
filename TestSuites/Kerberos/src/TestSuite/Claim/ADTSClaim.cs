// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Protocol.TestSuites.Kerberos.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using Microsoft.Protocols.TestTools.StackSdk.Security.Pac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocol.TestSuites.Kerberos.TestSuite.Claim
{
    [TestClass]
    public class ClaimsTest : TraditionTestBase
    {
        #region Test Suite Initialization

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }

        #endregion

        /// <summary>
        /// This case tests AD source claims in single realm environment
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.Claim)] 
        public void Kerberos_SingleRealm_ADSource_User_Only()
        {
            CLAIMS_SET? claims = GetADUserClaims_SingleRealm(
                this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[2].Username,
                this.testConfig.LocalRealm.User[2].Password,
                this.testConfig.LocalRealm.KDC[0].IPAddress,
                this.testConfig.LocalRealm.FileServer[0].DefaultServiceName,
                this.testConfig.LocalRealm.FileServer[0].Password);

            BaseTestSite.Assert.IsTrue(claims.HasValue, "CLAIMS_SET is returned for user claims");

            CLAIMS_SET val = claims.Value;

            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Start load claims from ad");
            string ClaimLocalforestUserDN = "cn=" + this.testConfig.LocalRealm.User[2].Username + ",cn=users,dc=" + this.testConfig.LocalRealm.RealmName.Replace(".", ",dc=");
            ClaimHelper.LoadClaims(ClaimLocalforestUserDN, ClaimsPrincipalClass.User,
                 this.testConfig.LocalRealm.KDC[0].IPAddress, this.testConfig.LocalRealm.RealmName, this.testConfig.LocalRealm.Admin.Username,
                this.testConfig.LocalRealm.Admin.Password);


            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Start compare claims between AD and Kerberos Ticket");
            for (int i = 0; i < val.ClaimsArrays.Length; i++)
            {
                for (int j = 0; j < val.ClaimsArrays[i].ClaimEntries.Length; j++)
                {
                    CLAIM_ENTRY entry = val.ClaimsArrays[i].ClaimEntries[j];
                    string str = ClaimUtility.ConvertEntryUniontoString(entry.Type, entry.Values);
                    BaseTestSite.Assert.IsTrue(ClaimHelper.FoundMatchedClaim(this.testConfig.LocalRealm.User[2].Username,
                         ClaimsPrincipalClass.User,
                          CLAIMS_SOURCE_TYPE.CLAIMS_SOURCE_TYPE_AD,
                          entry.Id,
                          entry.Type,
                          str),
                          "Should find same claim in AD");
                }
            }
        }

        /// <summary>
        /// This case tests AD source claims in cross realm environment
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.CrossRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.Claim)] 
        public void Kerberos_CrossRealm_ADSource_User_Only_Transform_Use_AD_and_CTA()
        {
            claimsTest_Kerberos_CrossRealm_ADSource_User_Only(false);
        }

        /// This case tests AD source claims in cross realm environment
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.CrossRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.Claim)]
        public void Kerberos_CrossRealm_ADSource_User_Only_Transform_From_Config()
        {
            claimsTest_Kerberos_CrossRealm_ADSource_User_Only(true);

        }

        private CLAIMS_SET? GetADUserClaims_SingleRealm(string realm, string user, string userPwd, string server, string servicePwd, string serviceSpn)
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

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send FAST armored TGS request: {0}.", this.testConfig.LocalRealm.FileServer[0].Smb2ServiceName);
            Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paPacRequest.Data, paPacOptions.Data });
            client.SendTgsRequest(this.testConfig.LocalRealm.FileServer[0].Smb2ServiceName, options, seqOfPaData2);
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();
            EncryptionKey key = testConfig.QueryKey(this.testConfig.LocalRealm.FileServer[0].Smb2ServiceName, client.Context.Realm.ToString(), client.Context.SelectedEType);
            tgsResponse.DecryptTicket(key);
            BaseTestSite.Assert.IsNotNull(tgsResponse.EncPart, "The encrypted part of TGS-REP is decrypted.");

            if (this.testConfig.IsKileImplemented)
            {
                BaseTestSite.Assert.IsNotNull(tgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(tgsResponse.TicketEncPart.authorization_data.Elements);
                BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2KPac.");
                    
                foreach (PacInfoBuffer buf in adWin2kPac.Pac.PacInfoBuffers)
                {
                    if (buf.GetType() == typeof(ClientClaimsInfo))
                    {
                        return ((ClientClaimsInfo)buf).NativeClaimSet;
                    }
                }
            }
            return null;
        }

        private void claimsTest_Kerberos_CrossRealm_ADSource_User_Only(bool ctaFromConfig)
        {
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName,
                 this.testConfig.LocalRealm.User[2].Username,
                 this.testConfig.LocalRealm.User[2].Password,
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
            Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data});
            //Create and send TGS request
            client.SendTgsRequest(this.testConfig.TrustedRealm.KDC[0].DefaultServiceName, options, seqOfPaData2);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send TGS request");
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Receive a referral TGS response.");

            BaseTestSite.Assert.AreEqual(this.testConfig.TrustedRealm.KDC[0].DefaultServiceName,
               KerberosUtility.PrincipalName2String(tgsResponse.Response.ticket.sname),
               "The service principal name in referral ticket should match expected.");
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.RealmName.ToLower(),
               tgsResponse.Response.ticket.realm.Value.ToLower(),
               "The realm name in referral ticket should match expected.");

            EncryptionKey key = testConfig.QueryKey(this.testConfig.TrustedRealm.KDC[0].DefaultServiceName + "@" + this.testConfig.LocalRealm.RealmName, client.Context.Realm.ToString(), client.Context.SelectedEType);
            tgsResponse.DecryptTicket(key);

            CLAIMS_SET claims = new CLAIMS_SET();
            AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(tgsResponse.TicketEncPart.authorization_data.Elements);
            bool foundClaims = false;
            foreach (PacInfoBuffer buf in adWin2kPac.Pac.PacInfoBuffers)
            {
                if (buf.GetType() == typeof(ClientClaimsInfo))
                {
                    claims = ((ClientClaimsInfo)buf).NativeClaimSet;
                    foundClaims = true;
                }
            }
            BaseTestSite.Assert.IsTrue(foundClaims, "Found claims in referral TGS Ticket");
            foundClaims = false;

            #region genertaed transformed claims
            Dictionary<string, string> expectedClaims = new Dictionary<string, string>();
            if (!ctaFromConfig)
            {
                ClaimTransformer transformer = new ClaimTransformer(this.testConfig.TrustedRealm.KDC[0].IPAddress, this.testConfig.TrustedRealm.RealmName, this.testConfig.TrustedRealm.Admin.Username, this.testConfig.TrustedRealm.Admin.Password);
                List<CLAIMS_ARRAY> transformed = null;
                BaseTestSite.Assert.AreEqual<Win32ErrorCode_32>(Win32ErrorCode_32.ERROR_SUCCESS, transformer.TransformClaimsOnTrustTraversal(claims.ClaimsArrays, this.testConfig.LocalRealm.RealmName, true, out transformed), "should successfully transform claims");
                foreach (CLAIMS_ARRAY array in transformed)
                {
                    foreach (CLAIM_ENTRY entry in array.ClaimEntries)
                    {
                        string id = entry.Id;
                        string value = null;
                        switch (entry.Type)
                        {
                            case CLAIM_TYPE.CLAIM_TYPE_BOOLEAN:
                                value = entry.Values.Struct4.BooleanValues[0].ToString();
                                break;
                            case CLAIM_TYPE.CLAIM_TYPE_INT64:
                                value = entry.Values.Struct1.Int64Values[0].ToString();
                                break;
                            case CLAIM_TYPE.CLAIM_TYPE_STRING:
                                value = entry.Values.Struct3.StringValues[0].ToString();
                                break;
                            case CLAIM_TYPE.CLAIM_TYPE_UINT64:
                                value = entry.Values.Struct2.Uint64Values[0].ToString();
                                break;
                            default:
                                BaseTestSite.Assert.Fail("Found invalid claim type during transform, value:" + (int)entry.Type);
                                break;
                        }
                        expectedClaims.Add(id.ToLower(), value.ToLower());
                    }
                }
            }
            else
            {
                string[] tmp = this.testConfig.LocalRealm.User[2].TransformedClaims.ToLower().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                BaseTestSite.Assert.IsTrue(tmp != null && tmp.Length % 2 == 0, "Claim.Crossforest.TransformedClaims in PTFConfig should be valid and not empty");
                for (int i = 0; i < tmp.Length; i += 2)
                {
                    expectedClaims.Add(tmp[i], tmp[i + 1]);
                }
            }
            #endregion
            //Change realm
            client.ChangeRealm(this.testConfig.TrustedRealm.RealmName,
                this.testConfig.TrustedRealm.KDC[0].IPAddress,
                this.testConfig.TrustedRealm.KDC[0].Port,
                this.testConfig.TransportType);

            //Create and send referal TGS request
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
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[2].Username,
                KerberosUtility.PrincipalName2String(refTgsResponse.TicketEncPart.cname).ToLower(),
                "User name in service ticket encrypted part should match expected.");

            adWin2kPac = FindOneInAuthData<AdWin2KPac>(refTgsResponse.TicketEncPart.authorization_data.Elements);
            foreach (PacInfoBuffer buf in adWin2kPac.Pac.PacInfoBuffers)
            {
                if (buf.GetType() == typeof(ClientClaimsInfo))
                {
                    foundClaims = true;
                    claims = ((ClientClaimsInfo)buf).NativeClaimSet;
                }
            }

            int errors = 0;
            BaseTestSite.Assert.IsTrue(foundClaims, "Found claims in reference TGS Ticket");
            for (int i = 0; i < claims.ClaimsArrays[0].ClaimEntries.Length; i++)
            {
                string claimvalue = null;
                if (!expectedClaims.TryGetValue(claims.ClaimsArrays[0].ClaimEntries[i].Id.ToLower(), out claimvalue))
                {
                    errors++;
                    BaseTestSite.Log.Add(LogEntryKind.CheckFailed, "Found unexpected claim with id: " + claims.ClaimsArrays[0].ClaimEntries[i].Id + " after transform");
                }
                else
                {
                    if (claimvalue != claims.ClaimsArrays[0].ClaimEntries[i].Values.Struct3.StringValues[0].ToLower())
                    {
                        errors++;
                        BaseTestSite.Log.Add(
                            LogEntryKind.CheckFailed,
                            "Value of claim \"" + claims.ClaimsArrays[0].ClaimEntries[i].Id + "\" is not expected, expected: " + claimvalue + " ,actual: " + claims.ClaimsArrays[0].ClaimEntries[i].Values.Struct3.StringValues[0]);
                    }
                    expectedClaims.Remove(claims.ClaimsArrays[0].ClaimEntries[i].Id);
                }
            }

            BaseTestSite.Assert.AreEqual(expectedClaims.Count, claims.ClaimsArrays[0].ClaimEntries.Count(),"Claims count should be equal.");
            BaseTestSite.Assert.AreEqual<int>(0, errors, "Expect no error should be found when compare claims from reference TGS ticket");
        }
    }
}
