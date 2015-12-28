// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.DirectoryServices.ActiveDirectory;

namespace Microsoft.Protocols.TestSuites.FileSharing.Auth.TestSuite
{
    [TestClass]
    public class KerberosAuthentication : AuthenticationTestBase
    {
        [Flags]
        protected enum CaseVariant
        {
            NONE = 0,
            AUTHENTICATOR_CNAME_NOT_MATCH = 1 << 0,
            AUTHENTICATOR_CREALM_NOT_MATCH = 1 << 1,
            AUTHENTICATOR_WRONG_ENC_KEY = 1 << 2,
            AUTHENTICATOR_EXCEED_TIME_SKEW = 1 << 3,
            TICKET_WRONG_REALM = 1 << 4,
            TICKET_WRONG_SNAME = 1 << 5,
            TICKET_WRONG_KVNO = 1 << 6,
            TICKET_NOT_VALID = 1 << 7,
            TICKET_EXPIRED = 1 << 8,
            TICKET_WRONG_ENC_KEY = 1 << 9,
            AUTHDATA_UNKNOWN_TYPE_IN_TKT = 1 << 10,
            AUTHDATA_UNKNOWN_TYPE_IN_TKT_OPTIONAL = 1 << 11,
            AUTHDATA_UNKNOWN_TYPE_IN_AUTHENTICATOR = 1 << 12,
            AUTHDATA_UNKNOWN_TYPE_IN_AUTHENTICATOR_OPTIONAL = 1 << 13,
        }

        public const int DefaultKdcPort = 88;

        public static bool IsComputerInDomain;
        public static string KDCIP;
        public static int KDCPort;
        public static KerberosConstValue.OidPkt OidPkt;
        public KerberosConstValue.GSSToken GssToken;

        public KeyManager keyManager;

        private string servicePrincipalName;

        #region Test Initialize and Cleanup
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            Initialize(testContext);
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Check if current computer is in a domain.");
            try
            {
                using (Domain domain = Domain.GetComputerDomain())
                {
                    DomainController dc = domain.FindDomainController(LocatorOptions.KdcRequired);
                    KDCIP = dc.IPAddress;
                    IsComputerInDomain = true;
                }
                KDCPort = DefaultKdcPort;
                OidPkt = KerberosConstValue.OidPkt.KerberosToken;
            }
            catch
            {
                IsComputerInDomain = false;
            }
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Cleanup();
        }

        protected override void TestInitialize()
        {
            base.TestInitialize();

            if (!IsComputerInDomain)
            {
                BaseTestSite.Assert.Inconclusive("Kerberos Authentication test cases are not applicable in non-domain environment");
            }

            if (servicePrincipalName == null)
            {
                servicePrincipalName = Smb2Utility.GetCifsServicePrincipalName(TestConfig.SutComputerName);
            }

            switch (TestConfig.DefaultSecurityPackage)
            {
                case TestTools.StackSdk.Security.Sspi.SecurityPackageType.Negotiate:
                    GssToken = KerberosConstValue.GSSToken.GSSSPNG;
                    break;
                case TestTools.StackSdk.Security.Sspi.SecurityPackageType.Kerberos:
                    GssToken = KerberosConstValue.GSSToken.GSSAPI;
                    break;
                default:
                    BaseTestSite.Assert.Inconclusive("Security package: {0} is not applicable for Kerberos Authentication test cases",
                        TestConfig.DefaultSecurityPackage);
                    break;
            }
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }
        #endregion

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.KerberosAuthentication)]
        [Description("This test case is designed to test whether server can handle Kerberos Authentication using GSSAPI correctly.")]
        public void BVT_KerbAuth_AccessFile_Success()
        {
            LoadConfig();
            Smb2KerberosAuthentication(CaseVariant.NONE);
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.KerberosAuthentication)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("This test case is designed to test whether server can handle wrong cname in the Authenticator in AP_REQ.")]
        public void KerbAuth_Authenticator_CNameNotMatch()
        {
            LoadConfig();
            Smb2KerberosAuthentication(CaseVariant.AUTHENTICATOR_CNAME_NOT_MATCH);
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.KerberosAuthentication)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("This test case is designed to test whether server can handle wrong crealm in the Authenticator in AP_REQ.")]
        public void KerbAuth_Authenticator_CRealmNotMatch()
        {
            LoadConfig();
            Smb2KerberosAuthentication(CaseVariant.AUTHENTICATOR_CREALM_NOT_MATCH);
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.KerberosAuthentication)]
        [TestCategory(TestCategories.InvalidIdentifier)]
        [Description("This test case is designed to test whether server can handle wrong encryption key of the Authenticator in AP_REQ.")]
        public void KerbAuth_Authenticator_WrongEncKey()
        {
            LoadConfig();
            Smb2KerberosAuthentication(CaseVariant.AUTHENTICATOR_WRONG_ENC_KEY);
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.KerberosAuthentication)]
        [TestCategory(TestCategories.UnexpectedContext)]
        [Description("This test case is designed to test whether server can handle unknown Realm in the Ticket in AP_REQ.")]
        public void KerbAuth_Ticket_WrongRealm()
        {
            LoadConfig();
            Smb2KerberosAuthentication(CaseVariant.TICKET_WRONG_REALM);
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.KerberosAuthentication)]
        [TestCategory(TestCategories.UnexpectedContext)]
        [Description("This test case is designed to test whether server can handle unknown SName in the Ticket in AP_REQ.")]
        public void KerbAuth_Ticket_WrongSName()
        {
            LoadConfig();
            Smb2KerberosAuthentication(CaseVariant.TICKET_WRONG_SNAME);
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.KerberosAuthentication)]
        [TestCategory(TestCategories.UnexpectedContext)]
        [Description("This test case is designed to test whether server can handle invalid KVNO in the Ticket in AP_REQ.")]
        public void KerbAuth_Ticket_WrongKvno()
        {
            LoadConfig();
            Smb2KerberosAuthentication(CaseVariant.TICKET_WRONG_KVNO);
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.KerberosAuthentication)]
        [TestCategory(TestCategories.UnexpectedContext)]
        [Description("This test case is designed to test whether server can handle invalid Ticket in AP_REQ.")]
        public void KerbAuth_Ticket_NotValid()
        {
            LoadConfig();
            Smb2KerberosAuthentication(CaseVariant.TICKET_NOT_VALID);
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.KerberosAuthentication)]
        [TestCategory(TestCategories.UnexpectedContext)]
        [Description("This test case is designed to test whether server can handle expired Ticket in AP_REQ.")]
        public void KerbAuth_Ticket_Expired()
        {
            LoadConfig();
            Smb2KerberosAuthentication(CaseVariant.TICKET_EXPIRED);
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.KerberosAuthentication)]
        [TestCategory(TestCategories.UnexpectedContext)]
        [Description("This test case is designed to test whether server can handle wrong encryption key of the Ticket in AP_REQ.")]
        public void KerbAuth_Ticket_WrongEncKey()
        {
            LoadConfig();
            Smb2KerberosAuthentication(CaseVariant.TICKET_WRONG_ENC_KEY);
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.KerberosAuthentication)]
        [TestCategory(TestCategories.UnexpectedContext)]
        [Description("This test case is designed to test whether server can handle unknown AuthorizationData in the Ticket.")]
        public void KerbAuth_AuthData_UnknownType_Ticket()
        {
            LoadConfig();
            Smb2KerberosAuthentication(CaseVariant.AUTHDATA_UNKNOWN_TYPE_IN_TKT);
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.KerberosAuthentication)]
        [TestCategory(TestCategories.UnexpectedContext)]
        [Description("This test case is designed to test whether server can handle " +
                     "unknown AuthorizationData contained in AD-IF-RELEVANT in the Ticket.")]
        public void KerbAuth_AuthData_UnknownType_Optional_Ticket()
        {
            LoadConfig();
            Smb2KerberosAuthentication(CaseVariant.AUTHDATA_UNKNOWN_TYPE_IN_TKT_OPTIONAL);
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.KerberosAuthentication)]
        [TestCategory(TestCategories.UnexpectedContext)]
        [Description("This test case is designed to test whether server can handle unknown AuthorizationData in the Authenticator.")]
        public void KerbAuth_AuthData_UnknownType_Authenticator()
        {
            LoadConfig();
            Smb2KerberosAuthentication(CaseVariant.AUTHDATA_UNKNOWN_TYPE_IN_AUTHENTICATOR);
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.KerberosAuthentication)]
        [TestCategory(TestCategories.UnexpectedContext)]
        [Description("This test case is designed to test whether server can handle " +
                     "unknown AuthorizationData contained in AD-IF-RELEVANT in the Authenticator.")]
        public void KerbAuth_AuthData_UnknownType_Optional_Authenticator()
        {
            LoadConfig();
            Smb2KerberosAuthentication(CaseVariant.AUTHDATA_UNKNOWN_TYPE_IN_AUTHENTICATOR_OPTIONAL);
        }

        [TestMethod]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.KerberosAuthentication)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether server can handle Replay correctly.")]
        public void KerbAuth_Replay()
        {
            #region Get Service Ticket
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Initialize Kerberos Functional Client");
            KerberosFunctionalClient kerberosClient = new KerberosFunctionalClient(
                TestConfig.DomainName,
                TestConfig.UserName,
                TestConfig.UserPassword,
                KerberosAccountType.User,
                KDCIP,
                KDCPort,
                TransportType.TCP,
                OidPkt,
                BaseTestSite);

            //Create and send AS request
            const KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            kerberosClient.SendAsRequest(options, null);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Kerberos Functional Client expects Kerberos Error from KDC");
            //Receive preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = kerberosClient.ExpectPreauthRequiredError(out methodData);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Kerberos Functional Client sends AS request with PA-DATA set");
            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp,
                0,
                kerberosClient.Context.SelectedEType,
                kerberosClient.Context.CName.Password,
                kerberosClient.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new[] { paEncTimeStamp.Data, paPacRequest.Data });
            //Create and send AS request
            kerberosClient.SendAsRequest(options, seqOfPaData);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Kerberos Functional Client expects AS response from KDC");
            KerberosAsResponse asResponse = kerberosClient.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create and send TGS request
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Kerberos Functional Client sends TGS request to KDC");
            kerberosClient.SendTgsRequest(servicePrincipalName, options);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Kerberos Functional Client expects TGS response from KDC");
            KerberosTgsResponse tgsResponse = kerberosClient.ExpectTgsResponse();

            BaseTestSite.Assert.AreEqual(servicePrincipalName,
                KerberosUtility.PrincipalName2String(tgsResponse.Response.ticket.sname),
                "Service principal name in service ticket should match expected.");
            #endregion

            #region Create AP request
            Ticket serviceTicket = kerberosClient.Context.Ticket.Ticket;
            Realm crealm = serviceTicket.realm;
            EncryptionKey subkey = KerberosUtility.GenerateKey(kerberosClient.Context.SessionKey);
            PrincipalName cname = kerberosClient.Context.CName.Name;
            Authenticator authenticator = CreateAuthenticator(cname, crealm, subkey);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Create AP Request");
            KerberosApRequest request = new KerberosApRequest(
                kerberosClient.Context.Pvno,
                new APOptions(KerberosUtility.ConvertInt2Flags((int)ApOptions.MutualRequired)),
                kerberosClient.Context.Ticket,
                authenticator,
                KeyUsageNumber.AP_REQ_Authenticator
            );
            #endregion

            #region Create GSS token and send session setup request
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Create GSS Token");
            byte[] token = KerberosUtility.AddGssApiTokenHeader(request, OidPkt, GssToken);

            Smb2FunctionalClientForKerbAuth smb2Client = new Smb2FunctionalClientForKerbAuth(TestConfig.Timeout, TestConfig, BaseTestSite);
            smb2Client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            byte[] repToken;
            uint status = DoSessionSetupWithGssToken(smb2Client, token, out repToken);

            KerberosApResponse apRep = kerberosClient.GetApResponseFromToken(repToken, GssToken);
            // Get subkey from AP response, which used for signing in smb2
            apRep.Decrypt(kerberosClient.Context.Ticket.SessionKey.keyvalue.ByteArrayValue);
            smb2Client.SetSessionSigningAndEncryption(true, false, apRep.ApEncPart.subkey.keyvalue.ByteArrayValue);
            #endregion

            #region Second client
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Replay the request from another client");
            Smb2FunctionalClientForKerbAuth smb2Client2 = new Smb2FunctionalClientForKerbAuth(TestConfig.Timeout, TestConfig, BaseTestSite);
            smb2Client2.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            status = DoSessionSetupWithGssToken(smb2Client2, token, out repToken);

            BaseTestSite.Assert.AreNotEqual(Smb2Status.STATUS_SUCCESS, status,
                "Session Setup should fail because it uses a Replay of KRB_AP_REQ");

            if (TestConfig.IsWindowsPlatform)
            {
                krbError = kerberosClient.GetKrbErrorFromToken(repToken);
                BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KRB_AP_ERR_REPEAT, krbError.ErrorCode,
                    "SMB Server should return {0}", KRB_ERROR_CODE.KRB_AP_ERR_REPEAT);
            }
            smb2Client2.Disconnect();
            #endregion

            string path = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            AccessFile(smb2Client, path);

            smb2Client.LogOff();
            smb2Client.Disconnect();
        }

        private void LoadConfig()
        {
            if (string.IsNullOrEmpty(TestConfig.KeytabFile) &&
                (string.IsNullOrEmpty(TestConfig.ServicePassword) || string.IsNullOrEmpty(TestConfig.ServiceSaltString)))
            {
                BaseTestSite.Assert.Inconclusive("This case requires either Keytab file or cifs service password and salt string.");
            }

            keyManager = new KeyManager();
            if (!string.IsNullOrEmpty(TestConfig.KeytabFile))
            {
                keyManager.LoadKeytab(TestConfig.KeytabFile);
            }
            else
            {
                keyManager.MakeKey(servicePrincipalName, TestConfig.DomainName, TestConfig.ServicePassword, TestConfig.ServiceSaltString);
            }
        }

        private void Smb2KerberosAuthentication(CaseVariant variant)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Initialize Kerberos Functional Client");
            KerberosFunctionalClient kerberosClient = new KerberosFunctionalClient(
                TestConfig.DomainName,
                TestConfig.UserName,
                TestConfig.UserPassword,
                KerberosAccountType.User,
                KDCIP,
                KDCPort,
                TransportType.TCP,
                OidPkt,
                BaseTestSite);

            #region Service Ticket
            EncryptionKey serviceKey;
            EncTicketPart encTicketPart = RetrieveAndDecryptServiceTicket(kerberosClient, out serviceKey);

            Ticket serviceTicket = kerberosClient.Context.Ticket.Ticket;

            Realm crealm = serviceTicket.realm;
            BaseTestSite.Assert.AreEqual(TestConfig.DomainName.ToLower(),
                encTicketPart.crealm.Value.ToLower(),
                "Realm name in service ticket encrypted part should match as expected, case insensitive");
            BaseTestSite.Assert.AreEqual(TestConfig.UserName.ToLower(),
                KerberosUtility.PrincipalName2String(encTicketPart.cname).ToLower(),
                "User name in service ticket encrypted part should match as expected, case insensitive.");

            if (variant.HasFlag(CaseVariant.AUTHDATA_UNKNOWN_TYPE_IN_TKT))
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Add a type-unknown AuthorizationData to the ticket");
                AuthorizationDataElement unknownElement = GenerateUnKnownAuthorizationDataElement();
                AppendNewAuthDataElement(encTicketPart.authorization_data, unknownElement);
            }

            if (variant.HasFlag(CaseVariant.AUTHDATA_UNKNOWN_TYPE_IN_TKT_OPTIONAL))
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Add a type-unknown AuthorizationData which is inside AD_IF_RELEVANT to the ticket");
                AuthorizationDataElement unknownElement = GenerateUnKnownAuthorizationDataElement();
                AD_IF_RELEVANT ifRelavantElement =
                    new AD_IF_RELEVANT(new[] { unknownElement });
                var dataBuffer = new Asn1BerEncodingBuffer();
                ifRelavantElement.BerEncode(dataBuffer);
                AuthorizationDataElement unknownElementOptional =
                    new AuthorizationDataElement(new KerbInt32((long)AuthorizationData_elementType.AD_IF_RELEVANT), new Asn1OctetString(dataBuffer.Data));
                AppendNewAuthDataElement(encTicketPart.authorization_data, unknownElementOptional);
            }

            if (variant.HasFlag(CaseVariant.TICKET_NOT_VALID))
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Change the Ticket start time to tomorrow");
                DateTime tomorrow = DateTime.Now.AddDays(1);
                string kerbTimeString = tomorrow.ToUniversalTime().ToString("yyyyMMddhhmmssZ");
                encTicketPart.starttime = new KerberosTime(kerbTimeString, true);
            }
            if (variant.HasFlag(CaseVariant.TICKET_EXPIRED))
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Change the Ticket end time to yesterday");
                DateTime yesterday = DateTime.Now.AddDays(-1);
                string kerbTimeString = yesterday.ToUniversalTime().ToString("yyyyMMddhhmmssZ");
                encTicketPart.endtime = new KerberosTime(kerbTimeString, true);
            }

            if (variant.HasFlag(CaseVariant.TICKET_WRONG_ENC_KEY))
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Modify the Ticket, making it encrypted with wrong key");
                serviceKey = KerberosUtility.GenerateKey(serviceKey);
            }

            EncryptedData data = EncryptTicket(encTicketPart, serviceKey);
            serviceTicket.enc_part = data;

            if (variant.HasFlag(CaseVariant.TICKET_WRONG_REALM))
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Change the Realm in the Ticket to an unknown realm");
                serviceTicket.realm = new Realm("kerb.com");
            }
            if (variant.HasFlag(CaseVariant.TICKET_WRONG_SNAME))
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Change the SNAME in the Ticket to an unknown service name");
                serviceTicket.sname = new PrincipalName(new KerbInt32((long)PrincipalType.NT_SRV_INST),
                    KerberosUtility.String2SeqKerbString("UnknownService", TestConfig.DomainName));
            }
            if (variant.HasFlag(CaseVariant.TICKET_WRONG_KVNO))
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep,
                    "Change the KVNO in the Ticket to an invalid Key Version Number (Int32.MaxValue)");
                const int invalidKvno = System.Int32.MaxValue;
                serviceTicket.enc_part.kvno = new KerbInt32(invalidKvno);
            }
            #endregion

            #region Authenticator
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Create Authenticator");
            EncryptionKey subkey = KerberosUtility.GenerateKey(kerberosClient.Context.SessionKey);
            PrincipalName cname;
            if (variant.HasFlag(CaseVariant.AUTHENTICATOR_CNAME_NOT_MATCH))
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Use wrong cname in the Authenticator");
                cname = new PrincipalName(new KerbInt32((long)PrincipalType.NT_PRINCIPAL),
                    KerberosUtility.String2SeqKerbString(TestConfig.NonAdminUserName, TestConfig.DomainName));
            }
            else
            {
                cname = kerberosClient.Context.CName.Name;
            }

            Authenticator authenticator = CreateAuthenticator(cname, crealm, subkey);
            if (variant.HasFlag(CaseVariant.AUTHENTICATOR_CREALM_NOT_MATCH))
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Use wrong crealm in the Authenticator");
                authenticator.crealm = new Realm("kerb.com");
            }

            if (variant.HasFlag(CaseVariant.AUTHENTICATOR_EXCEED_TIME_SKEW))
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Change the ctime in the Authenticator to one hour later");
                DateTime oneHourLater = DateTime.Now.AddHours(1);
                string kerbTimeString = oneHourLater.ToUniversalTime().ToString("yyyyMMddhhmmssZ");
                authenticator.ctime = new KerberosTime(kerbTimeString, true);
            }

            if (variant.HasFlag(CaseVariant.AUTHDATA_UNKNOWN_TYPE_IN_AUTHENTICATOR))
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Add a type-unknown AuthorizationData to the Authenticator");
                AuthorizationDataElement unknownElement = GenerateUnKnownAuthorizationDataElement();
                authenticator.authorization_data = new AuthorizationData(new[] { unknownElement });
            }

            if (variant.HasFlag(CaseVariant.AUTHDATA_UNKNOWN_TYPE_IN_AUTHENTICATOR_OPTIONAL))
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep,
                    "Add a type-unknown AuthorizationData which is inside AD_IF_RELEVANT to the Authenticator");
                AuthorizationDataElement unknownElement = GenerateUnKnownAuthorizationDataElement();
                AD_IF_RELEVANT ifRelavantElement =
                    new AD_IF_RELEVANT(new[] { unknownElement });
                var dataBuffer = new Asn1BerEncodingBuffer();
                ifRelavantElement.BerEncode(dataBuffer);
                AuthorizationDataElement unknownElementOptional =
                    new AuthorizationDataElement(new KerbInt32((long)AuthorizationData_elementType.AD_IF_RELEVANT), new Asn1OctetString(dataBuffer.Data));
                authenticator.authorization_data = new AuthorizationData(new[] { unknownElementOptional });
            }

            #endregion

            #region AP Request
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Create AP Request");
            if (variant.HasFlag(CaseVariant.AUTHENTICATOR_WRONG_ENC_KEY))
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Use wrong key to encrypt the Authenticator");
                kerberosClient.Context.Ticket.SessionKey = KerberosUtility.GenerateKey(kerberosClient.Context.Ticket.SessionKey);
            }
            KerberosApRequest request = new KerberosApRequest(
                kerberosClient.Context.Pvno,
                new APOptions(KerberosUtility.ConvertInt2Flags((int)ApOptions.MutualRequired)),
                kerberosClient.Context.Ticket,
                authenticator,
                KeyUsageNumber.AP_REQ_Authenticator
            );
            #endregion

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Create GSS Token");
            byte[] token = KerberosUtility.AddGssApiTokenHeader(request, OidPkt, GssToken);

            Smb2FunctionalClientForKerbAuth smb2Client = new Smb2FunctionalClientForKerbAuth(TestConfig.Timeout, TestConfig, BaseTestSite);
            smb2Client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);

            #region Check the result

            byte[] repToken;
            uint status = DoSessionSetupWithGssToken(smb2Client, token, out repToken);

            if (variant.HasFlag(CaseVariant.AUTHENTICATOR_CNAME_NOT_MATCH) || variant.HasFlag(CaseVariant.AUTHENTICATOR_CREALM_NOT_MATCH))
            {
                BaseTestSite.Assert.AreNotEqual(Smb2Status.STATUS_SUCCESS, status,
                    "Session Setup should fail because the cname or crealm in the authenticator does not match the same field in the Ticket");
                if (TestConfig.IsWindowsPlatform)
                {
                    KerberosKrbError krbError = kerberosClient.GetKrbErrorFromToken(repToken);
                    BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KRB_AP_ERR_BADMATCH, krbError.ErrorCode,
                        "SMB Server should return {0}", KRB_ERROR_CODE.KRB_AP_ERR_BADMATCH);
                }
                smb2Client.Disconnect();
                return;
            }
            if (variant.HasFlag(CaseVariant.AUTHENTICATOR_WRONG_ENC_KEY) || variant.HasFlag(CaseVariant.TICKET_WRONG_ENC_KEY))
            {
                BaseTestSite.Assert.AreNotEqual(Smb2Status.STATUS_SUCCESS, status,
                    "Session Setup should fail because Ticket or Authenticator cannot be correctly decrypted");
                if (TestConfig.IsWindowsPlatform)
                {
                    KerberosKrbError krbError = kerberosClient.GetKrbErrorFromToken(repToken);
                    BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KRB_AP_ERR_MODIFIED, krbError.ErrorCode,
                        "SMB Server should return {0}", KRB_ERROR_CODE.KRB_AP_ERR_MODIFIED);
                }
                smb2Client.Disconnect();
                return;
            }
            if (variant.HasFlag(CaseVariant.AUTHENTICATOR_EXCEED_TIME_SKEW))
            {
                BaseTestSite.Assert.AreNotEqual(Smb2Status.STATUS_SUCCESS, status,
                    "Session Setup should fail because the server time and the client time " +
                    "in the Authenticator differ by (1 hour) more than the allowable clock skew");
                if (TestConfig.IsWindowsPlatform)
                {
                    KerberosKrbError krbError = kerberosClient.GetKrbErrorFromToken(repToken);
                    BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KRB_AP_ERR_SKEW, krbError.ErrorCode,
                        "SMB Server should return {0}", KRB_ERROR_CODE.KRB_AP_ERR_SKEW);
                }
                smb2Client.Disconnect();
                return;
            }
            if (variant.HasFlag(CaseVariant.TICKET_WRONG_KVNO) ||
                variant.HasFlag(CaseVariant.TICKET_WRONG_REALM) ||
                variant.HasFlag(CaseVariant.TICKET_WRONG_SNAME))
            {
                BaseTestSite.Log.Add(LogEntryKind.Comment, "If decryption fails, server would try other keys");
            }
            if (variant.HasFlag(CaseVariant.TICKET_NOT_VALID))
            {
                BaseTestSite.Assert.AreNotEqual(Smb2Status.STATUS_SUCCESS, status,
                    "Session Setup should fail because the starttime (tomorrow) in the Ticket " +
                    "is later than the current time by more than the allowable clock skew");
                if (TestConfig.IsWindowsPlatform)
                {
                    KerberosKrbError krbError = kerberosClient.GetKrbErrorFromToken(repToken);
                    BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KRB_AP_ERR_TKT_NYV, krbError.ErrorCode,
                        "SMB Server should return {0}", KRB_ERROR_CODE.KRB_AP_ERR_TKT_NYV);
                }
                smb2Client.Disconnect();
                return;
            }
            if (variant.HasFlag(CaseVariant.TICKET_EXPIRED))
            {
                BaseTestSite.Assert.AreNotEqual(Smb2Status.STATUS_SUCCESS, status,
                    "Session Setup should fail because the current time is later than the endtime (yesterday)" +
                    " in the Ticket by more than the allowable clock skew");
                if (TestConfig.IsWindowsPlatform)
                {
                    KerberosKrbError krbError = kerberosClient.GetKrbErrorFromToken(repToken);
                    BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KRB_AP_ERR_TKT_EXPIRED, krbError.ErrorCode,
                        "SMB Server should return {0}", KRB_ERROR_CODE.KRB_AP_ERR_TKT_EXPIRED);
                }
                smb2Client.Disconnect();
                return;
            }
            if (variant.HasFlag(CaseVariant.AUTHDATA_UNKNOWN_TYPE_IN_TKT))
            {
                BaseTestSite.Assert.AreNotEqual(Smb2Status.STATUS_SUCCESS, status,
                    "Session Setup should fail because of the unknown AutherizationData in the ticket");
                smb2Client.Disconnect();
                return;
            }
            if (variant.HasFlag(CaseVariant.AUTHDATA_UNKNOWN_TYPE_IN_AUTHENTICATOR))
            {
                BaseTestSite.Log.Add(LogEntryKind.Comment,
                    "Unknown AuthorizationData in the Authenticator should not fail the request");
            }
            if (variant.HasFlag(CaseVariant.AUTHDATA_UNKNOWN_TYPE_IN_TKT_OPTIONAL) ||
                variant.HasFlag(CaseVariant.AUTHDATA_UNKNOWN_TYPE_IN_AUTHENTICATOR_OPTIONAL))
            {
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Unknown AuthorizationData in AD_IF_RELEVANT is optional. " +
                                                           "Server should not fail the request.");
            }

            KerberosApResponse apRep = kerberosClient.GetApResponseFromToken(repToken, GssToken);
            // Get subkey from AP response, which used for signing in smb2
            apRep.Decrypt(kerberosClient.Context.Ticket.SessionKey.keyvalue.ByteArrayValue);
            smb2Client.SetSessionSigningAndEncryption(true, false, apRep.ApEncPart.subkey.keyvalue.ByteArrayValue);

            string path = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            AccessFile(smb2Client, path);
            #endregion

            smb2Client.LogOff();
            smb2Client.Disconnect();
        }

        private EncTicketPart RetrieveAndDecryptServiceTicket(KerberosFunctionalClient kerberosClient, out EncryptionKey serviceKey)
        {
            //Create and send AS request
            const KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            kerberosClient.SendAsRequest(options, null);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Kerberos Functional Client expects Kerberos Error from KDC");
            //Receive preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = kerberosClient.ExpectPreauthRequiredError(out methodData);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Kerberos Functional Client sends AS request with PA-DATA set");
            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp,
                0,
                kerberosClient.Context.SelectedEType,
                kerberosClient.Context.CName.Password,
                kerberosClient.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new[] { paEncTimeStamp.Data, paPacRequest.Data });
            //Create and send AS request
            kerberosClient.SendAsRequest(options, seqOfPaData);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Kerberos Functional Client expects AS response from KDC");
            KerberosAsResponse asResponse = kerberosClient.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create and send TGS request
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Kerberos Functional Client sends TGS request to KDC");
            kerberosClient.SendTgsRequest(servicePrincipalName, options);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Kerberos Functional Client expects TGS response from KDC");
            KerberosTgsResponse tgsResponse = kerberosClient.ExpectTgsResponse();

            BaseTestSite.Assert.AreEqual(servicePrincipalName,
                KerberosUtility.PrincipalName2String(tgsResponse.Response.ticket.sname),
                "Service principal name in service ticket should match expected.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Decrypt SMB2 Service Ticket");
            serviceKey = keyManager.QueryKey(servicePrincipalName, TestConfig.DomainName, kerberosClient.Context.SelectedEType);
            tgsResponse.DecryptTicket(serviceKey);

            return tgsResponse.TicketEncPart;
        }

        private EncryptedData EncryptTicket(EncTicketPart encTicketPart, EncryptionKey serviceKey)
        {
            Asn1BerEncodingBuffer encodeBuffer = new Asn1BerEncodingBuffer();
            encTicketPart.BerEncode(encodeBuffer, true);
            byte[] encData = KerberosUtility.Encrypt(
                (EncryptionType)serviceKey.keytype.Value,
                serviceKey.keyvalue.ByteArrayValue,
                encodeBuffer.Data,
                (int)KeyUsageNumber.AS_REP_TicketAndTGS_REP_Ticket);

            return new EncryptedData(
                    new KerbInt32(serviceKey.keytype.Value),
                    null,
                    new Asn1OctetString(encData));
        }

        private Authenticator CreateAuthenticator(PrincipalName cname, Realm realm, EncryptionKey subkey = null, AuthorizationData data = null)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Create authenticator");
            Random r = new Random();
            int seqNum = r.Next();

            Authenticator plaintextAuthenticator = new Authenticator
            {
                authenticator_vno = new Asn1Integer(KerberosConstValue.KERBEROSV5),
                crealm = realm,
                cusec = new Microseconds(0),
                ctime = KerberosUtility.CurrentKerberosTime,
                seq_number = new KerbUInt32(seqNum),
                cname = cname,
                subkey = subkey,
                authorization_data = data
            };

            AuthCheckSum checksum = new AuthCheckSum { Lgth = KerberosConstValue.AUTHENTICATOR_CHECKSUM_LENGTH };
            checksum.Bnd = new byte[checksum.Lgth];
            checksum.Flags = (int)(ChecksumFlags.GSS_C_MUTUAL_FLAG | ChecksumFlags.GSS_C_INTEG_FLAG);
            byte[] checkData = ArrayUtility.ConcatenateArrays(BitConverter.GetBytes(checksum.Lgth),
                checksum.Bnd, BitConverter.GetBytes(checksum.Flags));
            plaintextAuthenticator.cksum = new Checksum(new KerbInt32((int)ChecksumType.ap_authenticator_8003), new Asn1OctetString(checkData));

            return plaintextAuthenticator;
        }

        private uint DoSessionSetupWithGssToken(Smb2FunctionalClientForKerbAuth smb2Client,
            byte[] gssTokenByte,
            out byte[] repToken)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Negotiate and SessionSetup using created gssToken");
            smb2Client.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);

            uint status = smb2Client.SessionSetup(
                Packet_Header_Flags_Values.NONE,
                SESSION_SETUP_Request_Flags.NONE,
                SESSION_SETUP_Request_SecurityMode_Values.NONE,
                SESSION_SETUP_Request_Capabilities_Values.GLOBAL_CAP_DFS,
                gssTokenByte,
                out repToken);

            return status;
        }

        private void AccessFile(Smb2FunctionalClientForKerbAuth smb2Client, string path)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Try accessing files.");
            uint treeId;

            smb2Client.TreeConnect(path, out treeId);
            Smb2CreateContextResponse[] serverCreateContexts;
            FILEID fileId;
            smb2Client.Create(
                treeId,
                Guid.NewGuid() + ".txt",
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId,
                out serverCreateContexts);
            smb2Client.Close(treeId, fileId);
            smb2Client.TreeDisconnect(treeId);
        }

        private AuthorizationDataElement GenerateUnKnownAuthorizationDataElement()
        {
            byte[] randomData = new byte[100];
            Random r = new Random();
            r.NextBytes(randomData);
            const int unknownType = Int16.MaxValue;
            AuthorizationDataElement unknowElement = new AuthorizationDataElement(new KerbInt32(unknownType), new Asn1OctetString(randomData));
            return unknowElement;
        }

        private void AppendNewAuthDataElement(AuthorizationData originalAuthData, AuthorizationDataElement newElement)
        {
            int elementNum = originalAuthData.Elements.Length;
            AuthorizationDataElement[] elements = new AuthorizationDataElement[elementNum + 1];
            originalAuthData.Elements.CopyTo(elements, 0);
            elements[elementNum] = newElement;
            originalAuthData.Elements = elements;
        }
    }
}
