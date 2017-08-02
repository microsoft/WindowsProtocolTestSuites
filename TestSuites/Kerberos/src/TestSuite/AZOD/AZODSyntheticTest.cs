// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using Microsoft.Protocol.TestSuites.Kerberos.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocol.TestSuites.Kerberos.TestSuite.AZOD
{
    [TestClass]
    public class AZODSyntheticTest : TraditionTestBase
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

        #region DAC

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Description("This test case is designed to verify the process that a user access file successfully on an SMB2 server using Kerberos authentication.")]
        public void DAC_Smb2_Possitive_AccessFile()
        {
            DAC_Smb2_AccessFile(this.testConfig.LocalRealm.RealmName, 
                this.testConfig.LocalRealm.User[2], 
                testConfig.LocalRealm.KDC[0],
                testConfig.TransportType, 
                this.testConfig.LocalRealm.FileServer[0], 
                this.testConfig.LocalRealm.FileServer[0].DACShareFolder,
                string.Format("{0}_{1}.txt", MethodInfo.GetCurrentMethod().Name, Guid.NewGuid()), 
                false);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Description("This test case is designed to verify the process that a user access file on an SMB2 server using Kerberos authentication, and the access is denied due to no authorization")]
        public void DAC_Smb2_Negative_AccessFile()
        {
            DAC_Smb2_AccessFile(this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[1],
                testConfig.LocalRealm.KDC[0],
                testConfig.TransportType,
                this.testConfig.LocalRealm.FileServer[0],
                this.testConfig.LocalRealm.FileServer[0].DACShareFolder,
                string.Format("{0}_{1}.txt", MethodInfo.GetCurrentMethod().Name, Guid.NewGuid()), 
                true);
        }

        #endregion

        #region Claim
        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.Claim)]        
        [Description("This test case is designed to verify the process that a user logons to an SMB2 server successfully using Kerberos authentication with Claims enabled.")]
        public void CBAC_Smb2_Possitive_AccessFile()
        {
            CBAC_Smb2_AccessFile(this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[2], 
                testConfig.LocalRealm.KDC[0], 
                testConfig.TransportType,
                this.testConfig.LocalRealm.FileServer[0],
                this.testConfig.LocalRealm.FileServer[0].CBACShareFolder,
                string.Format("{0}_{1}.txt", MethodInfo.GetCurrentMethod().Name, Guid.NewGuid()), 
                false);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.Claim)]
        [Description("This test case is designed to verify the process that a user logons to an SMB2 server using Kerberos authentication with Claims enabled, and the access is denied due to no authorization")]
        public void CBAC_Smb2_Negative_AccessFile()
        {
            CBAC_Smb2_AccessFile(this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[1], 
                testConfig.LocalRealm.KDC[0], 
                testConfig.TransportType, 
                this.testConfig.LocalRealm.FileServer[0],
                this.testConfig.LocalRealm.FileServer[0].CBACShareFolder,
                string.Format("{0}_{1}.txt", MethodInfo.GetCurrentMethod().Name, Guid.NewGuid()), 
                true);
        }

        #endregion

        #region Cross-Realm

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.CrossRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.Claim)]
        [Description("This test case is designed to verify the process that a user logons to a cross realm SMB2 server using Kerberos authentication with Claims enabled, and the access is successful.")]
        public void CrossRealm_Smb2_Possitive_AccessFile()
        {
            CrossRealm_Smb2_AccessFile(this.testConfig.LocalRealm.RealmName, 
                this.testConfig.TrustedRealm.RealmName, 
                this.testConfig.LocalRealm.User[2], 
                testConfig.LocalRealm.KDC[0], 
                this.testConfig.TrustedRealm.KDC[0], 
                testConfig.TransportType, 
                this.testConfig.TrustedRealm.FileServer[0], 
                this.testConfig.TrustedRealm.FileServer[0].CBACShareFolder,
                string.Format("{0}_{1}.txt", MethodInfo.GetCurrentMethod().Name, Guid.NewGuid()), 
                false);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.CrossRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.Claim)]
        [Description("This test case is designed to verify the process that a user logons to a cross realm SMB2 server using Kerberos authentication with Claims enabled, and the access fails due to authorization issue.")]
        public void CrossRealm_Smb2_Negative_AccessFile()
        {
            CrossRealm_Smb2_AccessFile(this.testConfig.LocalRealm.RealmName, 
                this.testConfig.TrustedRealm.RealmName,
                this.testConfig.LocalRealm.User[1], 
                testConfig.LocalRealm.KDC[0], 
                this.testConfig.TrustedRealm.KDC[0],
                testConfig.TransportType,
                this.testConfig.TrustedRealm.FileServer[0],
                this.testConfig.TrustedRealm.FileServer[0].CBACShareFolder,
                string.Format("{0}_{1}.txt", MethodInfo.GetCurrentMethod().Name, Guid.NewGuid()),  
                true);
        }

        #endregion

        #region private methods

        private void DAC_Smb2_AccessFile(string RealmName, User user, Computer kdc, TransportType transportType, FileServer fileserver, string filePath, string fileName, bool expectAccessDeny)
        {
            base.Logging();

            client = new KerberosTestClient(RealmName,
                user.Username,
                user.Password,
                KerberosAccountType.User,
                kdc.IPAddress,
                kdc.Port,
                transportType,
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
            client.SendTgsRequest(fileserver.Smb2ServiceName, options);
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();

            BaseTestSite.Assert.AreEqual(fileserver.Smb2ServiceName,
                KerberosUtility.PrincipalName2String(tgsResponse.Response.ticket.sname),
                "Service principal name in service ticket should match expected.");
            EncryptionKey key = testConfig.QueryKey(fileserver.Smb2ServiceName, client.Context.Realm.ToString(), client.Context.SelectedEType);
            tgsResponse.DecryptTicket(key);

            BaseTestSite.Assert.AreEqual(RealmName.ToLower(),
                tgsResponse.TicketEncPart.crealm.Value.ToLower(),
                "Realm name in service ticket encrypted part should match expected.");
            BaseTestSite.Assert.AreEqual(user.Username.ToLower(),
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

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Logon to fileserver and Access File.");
            AccessFile(filePath, fileName, fileserver, token, tgsResponse.EncPart.key, expectAccessDeny);

        }

        private void CBAC_Smb2_AccessFile(string RealmName, User user, Computer kdc, TransportType transportType, FileServer fileserver, string filePath, string fileName, bool expectAccessDeny)
        {
            base.Logging();

            client = new KerberosTestClient(RealmName,
                user.Username,
                user.Password,
                KerberosAccountType.User,
                kdc.IPAddress,
                kdc.Port,
                transportType,
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
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send FAST armored TGS request: {0}.", fileserver.Smb2ServiceName);
            Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paPacRequest.Data, paPacOptions.Data });
            client.SendTgsRequest(fileserver.Smb2ServiceName, options, seqOfPaData2);
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();
            EncryptionKey key = testConfig.QueryKey(fileserver.Smb2ServiceName, client.Context.Realm.ToString(), client.Context.SelectedEType);
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

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Logon to fileserver and Access File.");
            AccessFile(filePath, fileName, fileserver, token, tgsResponse.EncPart.key, expectAccessDeny);
        }

        private void CrossRealm_Smb2_AccessFile(string localRealmName, string trustuedRealmName, User localUser, Computer localKDC, Computer trustedKDC, TransportType transportType, FileServer fileserverInTrustedRealm, string filePath, string fileName, bool expectAccessDeny)
        {
            base.Logging();

            client = new KerberosTestClient(localRealmName,
                localUser.Username,
                localUser.Password,
                KerberosAccountType.User,
                localKDC.IPAddress,
                localKDC.Port,
                transportType,
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
                client.SendTgsRequest(fileserverInTrustedRealm.Smb2ServiceName, options);
            }
            else if (this.testConfig.TrustType == Adapter.TrustType.Realm)
            {
                client.SendTgsRequest(trustedKDC.DefaultServiceName, options);
            }
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send TGS request");
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Receive a referral TGS response.");

            BaseTestSite.Assert.AreEqual(trustedKDC.DefaultServiceName,
               KerberosUtility.PrincipalName2String(tgsResponse.Response.ticket.sname),
               "The service principal name in referral ticket should match expected.");
            BaseTestSite.Assert.AreEqual(localRealmName.ToLower(),
               tgsResponse.Response.ticket.realm.Value.ToLower(),
               "The realm name in referral ticket should match expected.");

            //Change realm
            client.ChangeRealm(trustuedRealmName,
                trustedKDC.IPAddress,
                trustedKDC.Port,
                this.testConfig.TransportType);

            //Create and send referral TGS request
            client.SendTgsRequest(fileserverInTrustedRealm.Smb2ServiceName, options);
            KerberosTgsResponse refTgsResponse = client.ExpectTgsResponse();

            BaseTestSite.Assert.AreEqual(fileserverInTrustedRealm.Smb2ServiceName,
               KerberosUtility.PrincipalName2String(refTgsResponse.Response.ticket.sname),
               "The service principal name in service ticket should match expected.");
            BaseTestSite.Assert.AreEqual(trustuedRealmName.ToLower(),
               refTgsResponse.Response.ticket.realm.Value.ToLower(),
               "The realm name in service ticket should match expected.");

            EncryptionKey key = testConfig.QueryKey(fileserverInTrustedRealm.Smb2ServiceName, client.Context.Realm.ToString(), client.Context.SelectedEType);
            refTgsResponse.DecryptTicket(key);

            BaseTestSite.Assert.AreEqual(localRealmName.ToLower(),
                refTgsResponse.TicketEncPart.crealm.Value.ToLower(),
                "Realm name in service ticket encrypted part should match expected.");
            BaseTestSite.Assert.AreEqual(localUser.Username.ToLower(),
                KerberosUtility.PrincipalName2String(refTgsResponse.TicketEncPart.cname).ToLower(),
                "User name in service ticket encrypted part should match expected.");

            AuthorizationData data = null;
            EncryptionKey subkey = KerberosUtility.GenerateKey(client.Context.SessionKey);
            byte[] token = client.CreateGssApiToken(ApOptions.MutualRequired,
                data,
                subkey,
                ChecksumFlags.GSS_C_MUTUAL_FLAG | ChecksumFlags.GSS_C_INTEG_FLAG);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Logon to fileserver and Access File.");
            AccessFile(filePath, fileName, fileserverInTrustedRealm, token, refTgsResponse.EncPart.key, expectAccessDeny);
        }

        private void AccessFile(string sharePath, string fileName, FileServer fileServer, byte[] gssApiToken, EncryptionKey subkey, bool expectAccessDeny)
        {
            BaseTestSite.Log.Add(LogEntryKind.Comment, "AccessFile: Create a SMB2 Client and Negotiate");
            Smb2FunctionalTestClient smb2Client = new Smb2FunctionalTestClient(KerberosConstValue.TIMEOUT_FOR_SMB2AP);
            smb2Client.ConnectToServerOverTCP(System.Net.IPAddress.Parse(fileServer.IPAddress));
            DialectRevision smb2Dialect = (DialectRevision)Enum.Parse(typeof(DialectRevision), fileServer.Smb2Dialect);
            DialectRevision selectedDialect;
            uint status = smb2Client.Negotiate(
                new DialectRevision[] { smb2Dialect },
                SecurityMode_Values.NONE,
                Capabilities_Values.GLOBAL_CAP_DFS,
                Guid.NewGuid(),
                out selectedDialect);
            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "Negotiate failed with error: {0}", status);

            byte[] repToken;
            BaseTestSite.Log.Add(LogEntryKind.Comment, "AccessFile: Session Setup");
            status = smb2Client.SessionSetup(
                SESSION_SETUP_Request_SecurityMode_Values.NONE,
                SESSION_SETUP_Request_Capabilities_Values.GLOBAL_CAP_DFS,
                SecurityPackageType.Kerberos,
                fileServer.FQDN,
                gssApiToken,
                out repToken);

            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "Session setup failed with error: {0}", status);

            KerberosApResponse apRep = client.GetApResponseFromToken(repToken);

            
            // Get subkey from AP response, which used for signing in smb2
            apRep.Decrypt(subkey.keyvalue.ByteArrayValue);
            smb2Client.SetSessionSigningAndEncryption(true, false, apRep.ApEncPart.subkey.keyvalue.ByteArrayValue);
            

            
            BaseTestSite.Log.Add(LogEntryKind.Comment, "AccessFile: Tree Connect");
            uint treeId;
            string path = @"\\" + fileServer.FQDN + @"\" + sharePath;
            status = smb2Client.TreeConnect(path, out treeId);
            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "TreeConnect failed with error: {0}", status);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "AccessFile: Create");
            Smb2CreateContextResponse[] serverCreateContexts;
            FILEID fileId;
            status = smb2Client.Create(
                treeId,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                new Smb2CreateContextRequest[]{
                    new Smb2CreateRequestLeaseV2
                    {
                        LeaseKey = Guid.NewGuid(),
                        LeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING |
                        LeaseStateValues.SMB2_LEASE_HANDLE_CACHING |
                        LeaseStateValues.SMB2_LEASE_WRITE_CACHING,
                    }
                },
                checker: SkipResponseCheck);
            if (expectAccessDeny)
            {
                BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_ACCESS_DENIED, status, "Create Operation should fail due to STATUS_ACCESS_DENIED, the received status is: {0}", status);
            }
            else
            {
                // Create success
                BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "Create failed with error: {0}", status);
                BaseTestSite.Log.Add(LogEntryKind.Comment, "AccessFile: Close");
                status = smb2Client.Close(treeId, fileId);
                BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "Close failed with error: {0}", status);
            }

            BaseTestSite.Log.Add(LogEntryKind.Comment, "AccessFile: Tree Disconnect");
            status = smb2Client.TreeDisconnect(treeId);
            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "Tree Disconnect failed with error: {0}", status);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "AccessFile: Logoff");
            status = smb2Client.LogOff();
            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "Logoff failed with error: {0}", status);

            smb2Client.Disconnect();
        }

        private void SkipResponseCheck<T>(Packet_Header header, T response)
        {
            // Do nothing, Used to skip check
        }

        #endregion
    }
}
