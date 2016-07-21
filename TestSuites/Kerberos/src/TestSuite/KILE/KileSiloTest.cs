// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocol.TestSuites.Kerberos.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocol.TestSuites.Kerberos.TestSuite
{
    /// <summary>
    ///
    /// </summary>
    [TestClass]
    public class KileSiloTest : TraditionTestBase
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
        [TestCategory(TestCategories.DFL2K12R2)]
        [TestCategory(TestCategories.Silo)]
        [TestCategory("Silo Interactive Logon")]
        [Feature(Feature.Silo)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the KDC behavior when interactive logon with RC4 as pre-authentication.")]
        //user15, testsilo01, in protected users group, with no usedesonly configured.
        //Will fail the pre-authentication with KDC_ERR_ETYPE_NOTSUPP with DES as pre-authentication etype
        public void Protected_Users_DES_PreAuthentication_Fail()
        {
            base.Logging();
            //Section 3.3.5.6: PROTECTED_USERS is not supported by Windows 2000, Windows Server 2003, Windows Server 2008, Windows Server 2008 R2, or Windows Server 2012 KDCs.
            if (int.Parse(this.testConfig.LocalRealm.DomainControllerFunctionality) < 6)
            {
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Section 3.3.5.6: PROTECTED_USERS is not supported in Windows 2000, Windows Server 2003, Windows Server 2008, Windows Server 2008 R2, or Windows Server 2012 KDCs.");
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Will exit the case immediately.");
                return;
            }

            //Create kerberos test client and connect
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[15].Username,
                this.testConfig.LocalRealm.User[15].Password,
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

            //Send AS request
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);

            KerberosKrbError krbError;

            //Recieve preauthentication required error
            METHOD_DATA methodData;
             krbError = client.ExpectPreauthRequiredError(out methodData);

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
           
           //pre-authentication use DES
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp,
                0,
                EncryptionType.DES_CBC_MD5,
                this.client.Context.CName.Password,
                this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data, paPacOptions.Data });
            
            //Create and send AS request with DES as pre-authentication
            client.SendAsRequest(options, seqOfPaData);

            //PROTECTED_USERS is not supported in Windows 2000, Windows Server 2003, Windows Server 2008, Windows Server 2008 R2, or Windows Server 2012 KDCs.

            //Receive KDC_ERR_ETYPE_NOTSUPP error from KDC
            krbError = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_ETYPE_NOTSUPP, krbError.ErrorCode, "Section 3.3.5.6 AS Exchange:" +
                "If domainControllerFunctionality returns a value >= 6 ([MS-ADTS] section 3.1.1.3.2.25), the KDC MUST check whether the account is a member of PROTECTED_USERS ([MS-DTYP] section 2.4.2.4). " +
                "If it is a member of PROTECTED_USERS, then:<50>" +
                "If pre-authentication used DES or RC4, the KDC MUST return KDC_ERR_ETYPE_NOTSUPP.");
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12R2)]
        [TestCategory(TestCategories.Silo)]
        [Feature(Feature.Silo)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the KDC behavior when interactive logon with RC4 as pre-authentication.")]
        //user15, testsilo01, in protected users group, with no usedesonly configured.
        //Will fail the pre-authentication with KDC_ERR_ETYPE_NOTSUPP with RC4 as pre-authentication etype
        public void Protected_Users_RC4_PreAuthentication_Fail()
        {
            base.Logging();
            //Section 3.3.5.6: PROTECTED_USERS is not supported by Windows 2000, Windows Server 2003, Windows Server 2008, Windows Server 2008 R2, or Windows Server 2012 KDCs.
            if (int.Parse(this.testConfig.LocalRealm.DomainControllerFunctionality) < 6)
            {
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Section 3.3.5.6: PROTECTED_USERS is not supported in Windows 2000, Windows Server 2003, Windows Server 2008, Windows Server 2008 R2, or Windows Server 2012 KDCs.");
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Will exit the case immediately.");
                return;
            }

            //Create kerberos test client and connect
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[15].Username,
                this.testConfig.LocalRealm.User[15].Password,
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

            //Send AS requset
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);

            KerberosKrbError krbError;

            //Recieve preauthentication required error
            METHOD_DATA methodData;
            krbError = client.ExpectPreauthRequiredError(out methodData);

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            EncryptionType clientEType = EncryptionType.RC4_HMAC;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp,
                0,
                clientEType,
                this.client.Context.CName.Password,
                this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data, paPacOptions.Data });

            //Create and send AS request with RC4 as pre-authentication
            client.SendAsRequest(options, seqOfPaData);

            //Receive KDC_ERR_ETYPE_NOTSUPP error from KDC
            krbError = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_ETYPE_NOTSUPP, krbError.ErrorCode, "Section 3.3.5.6 AS Exchange:" +
                "If domainControllerFunctionality returns a value >= 6 ([MS-ADTS] section 3.1.1.3.2.25), the KDC MUST check whether the account is a member of PROTECTED_USERS ([MS-DTYP] section 2.4.2.4)." +
                "If it is a member of PROTECTED_USERS, then:<50>" +
                "If pre-authentication used DES or RC4, the KDC MUST return KDC_ERR_ETYPE_NOTSUPP.");
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12R2)]
        [TestCategory(TestCategories.Silo)]
        [Feature(Feature.Silo)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the KDC behavior when interactive logon with User's A2AF is defined in Authentication Policy.")]
        //user16, testsilo02, in protected users group, with no usedesonly configured, in AuthSilo and AuthPolicy
        //Will succeed to get TGT with AES256 as pre-authentication etype
        //User.A2AF User.department==HR. AP01 doesn't matches this condition        
        //AuthPolicy: User TGT lifetime: 60 minutes
        public void Protected_Users_Interactive_Logon_User_A2AF_Fail()
        {
            base.Logging();

            //Section 3.3.5.6: Authentication Policies are not supported by Windows 2000, Windows Server 2003, Windows Server 2008, Windows Server 2008 R2, or Windows Server 2012 KDCs.
            if (int.Parse(this.testConfig.LocalRealm.DomainControllerFunctionality) < 6)
            {
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Section 3.3.5.6: Authentication Policies are not supported by Windows 2000, Windows Server 2003, Windows Server 2008, Windows Server 2008 R2, or Windows Server 2012 KDCs.");
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Will exit the case immediately.");
                return;
            }
            
            //TODO: Consider to udpate Client computer's department attribute and then interactive log on to client computer

            client = new KerberosTestClient(
                this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.FileServer[0].NetBiosName,
                this.testConfig.LocalRealm.FileServer[0].Password,
                KerberosAccountType.Device,
                testConfig.LocalRealm.KDC[0].IPAddress,
                testConfig.LocalRealm.KDC[0].Port,
                testConfig.TransportType,
                testConfig.SupportedOid,
               testConfig.LocalRealm.FileServer[0].AccountSalt);

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
            PaPacRequest paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data, paPacOptions.Data });
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                string.Format("The type of AS-REP encrypted part is {0}.", asResponse.EncPart.GetType().Name));

            // Switch to user principal
            client = new KerberosTestClient(
                this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[16].Username,
                this.testConfig.LocalRealm.User[16].Password,
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

            Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paPacRequest.Data, paPacOptions.Data });
            client.SendAsRequestWithFast(options, seqOfPaData2, null, subkey, fastOptions, apOptions);
            KerberosKrbError krbError2 = client.ExpectKrbError();

            // Access check with User's A2AF failed, KDC will return the KDC_ERR_POLICY error.
            BaseTestSite.Assert.AreEqual(
                KRB_ERROR_CODE.KDC_ERR_POLICY, 
                krbError2.ErrorCode, 
                "Section 3.3.5.6 As Exchange " + 
                "If AllowedToAuthenticateFrom is not NULL, the PAC of the armor TGT MUST be used to perform an access check for the ACTRL_DS_CONTROL_ACCESS right with additional rights GUID against the AllowedToAuthenticateFrom. " +
                "If the access check fails, the KDC MUST return KDC_ERR_POLICY."
                );
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12R2)]
        [TestCategory(TestCategories.Silo)] 
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the KDC behavior when interactive logon with User's A2AF is defined in Authentication Policy.")]
        //user16, testsilo02, in protected users group, with no usedesonly configured, in AuthSilo and AuthPolicy
        //Will succeed to get TGT with AES256 as pre-authentication etype
        //User.A2AF User.department==HR. 
        // DC, department=HR
        //AuthPolicy: User TGT lifetime: 60 minutes
        //loginto KDC, KDC matches the condition
        public void Protected_Users_Interactive_Logon_User_A2AF_Succeed()
        {
            base.Logging();

            //Section 3.3.5.6: Authentication Policies are not supported by Windows 2000, Windows Server 2003, Windows Server 2008, Windows Server 2008 R2, or Windows Server 2012 KDCs.
            if (int.Parse(this.testConfig.LocalRealm.DomainControllerFunctionality) < 6)
            {
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Section 3.3.5.6: Authentication Policies are not supported by Windows 2000, Windows Server 2003, Windows Server 2008, Windows Server 2008 R2, or Windows Server 2012 KDCs.");
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Will exit the case immediately.");
                return;
            }

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

             //AS_REQ and KRB-ERROR using device principal
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
            PaPacRequest paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data, paPacOptions.Data });
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                string.Format("The type of AS-REP encrypted part is {0}.", asResponse.EncPart.GetType().Name));

            // Switch to user principal
            client = new KerberosTestClient(
                this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[16].Username,
                this.testConfig.LocalRealm.User[16].Password,
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

            Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paPacRequest.Data, paPacOptions.Data });
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

            Asn1SequenceOf<PA_DATA> seqOfPaData3 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp3.Data, paPacRequest.Data, paPacOptions.Data });
            client.SendAsRequestWithFast(options, seqOfPaData3, null, subkey, fastOptions, apOptions);
            KerberosAsResponse userKrbAsRep = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(userKrbAsRep.Response.ticket, "AS response should contain a TGT.");
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                string.Format("The type of AS-REP encrypted part is {0}.", userKrbAsRep.EncPart.GetType().Name));

            //verify flags in as reponse
            int flags = KerberosUtility.ConvertFlags2Int(asResponse.EncPart.flags.ByteArrayValue);
            BaseTestSite.Assert.AreEqual((EncTicketFlags)0,
                EncTicketFlags.FORWARDABLE & EncTicketFlags.PROXIABLE & (EncTicketFlags)flags,
                "Section  3.3.5.1: " +
                "If DelegationNotAllowed is set to TRUE on the principal, " +
                "(or if domainControllerFunctionality returns a value >= 6 ([MS-ADTS] section 3.1.1.3.2.25) and the principal is a member of PROTECTED_USERS ([MS-DTYP] section 2.4.2.4)), " +
                "the KILE KDC MUST NOT set the PROXIABLE or FORWARDABLE ticket flags ([RFC4120] sections 2.5 and 2.6)."
                );
               
            //Get user linked authentication policy attribute
            string attributeDN = this.sutController.getAccountAttributeDN(
                this.testConfig.LocalRealm.RealmName.ToLower().Substring(0, this.testConfig.LocalRealm.RealmName.Length - 4),
                this.testConfig.LocalRealm.User[16].Username,
                "Users",
                "msDS-AssignedAuthNPolicy",
                this.testConfig.LocalRealm.Admin.Username,
                this.testConfig.LocalRealm.Admin.Password
                );

            //Get authentication policy TGT lifetime

            // if policy not config the TGT life time, use the default value 4 hours for protected users. 
            // if not a protected user, use the default value 10 hours by DC group policy defined. 
            double? policyTGTLifetime4User = 4 * 36000000000;

            if (attributeDN != null)
            {
                char seperator = ',';
                string userLinkedPolicyName = attributeDN.Substring(3, attributeDN.IndexOf(seperator) - 3);

                //Get Authentication Policy User's TGT lifetime
                policyTGTLifetime4User = this.sutController.getAuthPolicyTGTLifeTime(
                    this.testConfig.LocalRealm.RealmName.ToLower().Substring(0, this.testConfig.LocalRealm.RealmName.Length - 4),
                    userLinkedPolicyName,
                    "msds-UserTGTLifetime",
                    this.testConfig.LocalRealm.Admin.Username,
                    this.testConfig.LocalRealm.Admin.Password
                    );

                if (policyTGTLifetime4User == null)
                {
                    // if policy not config the TGT life time, use the default value 4 hours for protected users. 
                    // if not a protected user, use the default value 10 hours by DC group policy defined. 
                    policyTGTLifetime4User = 4 * 36000000000; 
                }

            }

            // if policy not config the TGT life time, use the default value 4 hours for protected users. 
            // if not a protected user, use the default value 10 hours by DC group policy defined. 
            double? policyTGTLifetime4Computer = 4 * 36000000000;
           
            attributeDN = this.sutController.getAccountAttributeDN(
                this.testConfig.LocalRealm.RealmName.ToLower().Substring(0, this.testConfig.LocalRealm.RealmName.Length - 4),
                this.testConfig.LocalRealm.ClientComputer.NetBiosName.Substring(0, this.testConfig.LocalRealm.ClientComputer.NetBiosName.Length - 1),            
                "Computers",
                "msDS-AssignedAuthNPolicy",
                this.testConfig.LocalRealm.Admin.Username,
                this.testConfig.LocalRealm.Admin.Password
            );

            if (attributeDN != null)
            {
                char seperator = ',';
                string userLinkedPolicyName = attributeDN.Substring(3, attributeDN.IndexOf(seperator) - 3);

                //Get Authentication Policy Computer's TGT lifetime
                policyTGTLifetime4Computer = this.sutController.getAuthPolicyTGTLifeTime(
                    this.testConfig.LocalRealm.RealmName.ToLower().Substring(0, this.testConfig.LocalRealm.RealmName.Length - 4),
                    userLinkedPolicyName,
                    "msds-ComputerTGTLifetime",
                    this.testConfig.LocalRealm.Admin.Username,
                    this.testConfig.LocalRealm.Admin.Password
                    );
                if (policyTGTLifetime4Computer == null)
                {                    
                    policyTGTLifetime4Computer = 4 * 36000000000; 
                }

            }           
            
            //Get TGTlifetime from ticket received
            DateTime startTime = DateTime.ParseExact(
                asResponse.EncPart.starttime.ToString(),
                "yyyyMMddHHmmssZ",
                System.Globalization.CultureInfo.InvariantCulture
                );
            DateTime endTime = DateTime.ParseExact(
                asResponse.EncPart.endtime.ToString(),
                "yyyyMMddHHmmssZ",
                System.Globalization.CultureInfo.InvariantCulture
                );
            DateTime renewTime = DateTime.ParseExact(
                asResponse.EncPart.renew_till.ToString(),
                "yyyyMMddHHmmssZ",
                System.Globalization.CultureInfo.InvariantCulture
                );
            //get timespan
            TimeSpan maxTGTlife = endTime - startTime;
            TimeSpan maxRenewTime = renewTime - startTime;

            double? maxTGTlifeTimeSpan = maxTGTlife.Ticks;
            double? maxRenewTimeSpan = maxRenewTime.Ticks;
                       
            
            //verify MaxRenewAge and MaxTicketAge for the TGT
            BaseTestSite.Assert.AreEqual(
                policyTGTLifetime4Computer, // as response is encrypted by computer's key
                maxTGTlifeTimeSpan,
                "Section 3.3.5.6 MaxTicketAge (section 3.3.1) for the TGT is 4 hours unless specified by policy. " +
                "If TGTLifetime is not 0: MaxTicketAge for the TGT is TGTLifetime."
                );

            BaseTestSite.Assert.AreEqual(
                policyTGTLifetime4Computer,
                maxRenewTimeSpan,
                "Section 3.3.5.6 MaxRenewAge (section 3.3.1) for the TGT is 4 hours unless specified by policy. " +
                "If TGTLifetime is not 0: MaxRenewAge for the TGT is TGTLifetime."
                );

            // verify the PA-DATA of AS_REP: asResponse.EncPart.pa_datas as PA_SUPPORTED_ENCTYPES
            bool isExistPaSupportedEncTypes = false;
            bool isExistPadataValue = false;

            var padataCount = userKrbAsRep.EncPart.pa_datas.Elements.Length;
            for (int i = 0; i < padataCount; i++)
            {
                var padata = PaDataParser.ParseRepPaData(userKrbAsRep.EncPart.pa_datas.Elements[i]);
                if ((PaDataType)padata.Data.padata_type.Value == PaDataType.PA_SUPPORTED_ENCTYPES)
                {
                    isExistPaSupportedEncTypes = true;
                    for (int j = 0; j < padata.Data.padata_value.Value.Length; j++)
                    {
                        if (padata.Data.padata_value.Value[j]==0x1F)
                        {
                            isExistPadataValue = true;
                        }
                    }
                }
            }
            BaseTestSite.Assert.IsTrue(isExistPaSupportedEncTypes, "If domainControllerFunctionality returns a value >= 3:" +
                 "the KDC SHOULD, in the encrypted pre-auth data part ([Referrals-11], Appendix A) of the AS-REP message, " +
                 "include PA-DATA with the padata-type set to PA-SUPPORTED-ENCTYPES (165).");

            BaseTestSite.Assert.IsTrue(isExistPadataValue, "If domainControllerFunctionality returns a value >= 3: " +
                "the KDC SHOULD, in the encrypted pre-auth data part ([Referrals-11], Appendix A) of the AS-REP message, " +
                "include PA-DATA with the padata-type set to PA-SUPPORTED-ENCTYPES (165), and the padata-value set to 0x1F (section 2.2.6).");
            if (testConfig.IsClaimSupported)
            {
                PaSupportedEncTypes paSupportedEncTypes = null;
               // userKrbAsRep.EncPart.pa_datas.GetType

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
        [Priority(0)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12R2)]
        [TestCategory(TestCategories.Silo)]
        [Feature(Feature.Silo)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the KDC behavior when network logon with Computer's A2A2 is defined in Authentication Policy.")]
        //user17, testsilo03, in protected users group, with no usedesonly configured, in AuthSilo and no policy assigned        
        //AP01: compouter.A2A2 User.department==HR. testsilo03 doesn't matches this condition        
        //Case will fail with KDC_ERR_POLICY error.
        public void Protected_Users_Network_Logon_Computer_A2A2_Fail()
        {
            base.Logging();

            //Section 3.3.5.6: Authentication Policies are not supported by Windows 2000, Windows Server 2003, Windows Server 2008, Windows Server 2008 R2, or Windows Server 2012 KDCs.
            if (int.Parse(this.testConfig.LocalRealm.DomainControllerFunctionality) < 6)
            {
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Section 3.3.5.6: Authentication Policies are not supported by Windows 2000, Windows Server 2003, Windows Server 2008, Windows Server 2008 R2, or Windows Server 2012 KDCs.");
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Will exit the case immediately.");
                return;
            }

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
            PaPacRequest paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data, paPacOptions.Data });
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                string.Format("The type of AS-REP encrypted part is {0}.", asResponse.EncPart.GetType().Name));

            // Switch to user principal
            client = new KerberosTestClient(
                this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[17].Username,
                this.testConfig.LocalRealm.User[17].Password,
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

            Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paPacRequest.Data, paPacOptions.Data });
            client.SendAsRequestWithFast(options, seqOfPaData2, null, subkey, fastOptions, apOptions);
            KerberosKrbError krbError = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_PREAUTH_REQUIRED, krbError.ErrorCode, "Pre-authentication required.");

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
            Asn1SequenceOf<PA_DATA> seqOfPaData3 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp3.Data, paPacRequest.Data, paPacOptions.Data });
            client.SendAsRequestWithFast(options, seqOfPaData3, null, subkey, fastOptions, apOptions);

            //receive as response
            KerberosAsResponse userKrbAsRep = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(userKrbAsRep.Response.ticket, "AS response should contain a TGT.");
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                string.Format("The type of AS-REP encrypted part is {0}.", userKrbAsRep.EncPart.GetType().Name));

            // verify the PA-DATA of AS_REP: asResponse.EncPart.pa_datas as PA_SUPPORTED_ENCTYPES
            bool isExistPaSupportedEncTypes = false;
            bool isExistPadataValue = false;

            var padataCount = userKrbAsRep.EncPart.pa_datas.Elements.Length;
            for (int i = 0; i < padataCount; i++)
            {
                var padata = PaDataParser.ParseRepPaData(userKrbAsRep.EncPart.pa_datas.Elements[i]);
                if ((PaDataType)padata.Data.padata_type.Value == PaDataType.PA_SUPPORTED_ENCTYPES)
                {
                    isExistPaSupportedEncTypes = true;
                    for (int j = 0; j < padata.Data.padata_value.Value.Length; j++)
                    {
                        if (padata.Data.padata_value.Value[j] == 0x1F)
                        {
                            isExistPadataValue = true;
                        }
                    }
                }
            }
            BaseTestSite.Assert.IsTrue(isExistPaSupportedEncTypes, "If domainControllerFunctionality returns a value >= 3:" +
                "the KDC SHOULD, in the encrypted pre-auth data part ([Referrals-11], Appendix A) of the AS-REP message, " +
                "include PA-DATA with the padata-type set to PA-SUPPORTED-ENCTYPES (165).");

            BaseTestSite.Assert.IsTrue(isExistPadataValue, "If domainControllerFunctionality returns a value >= 3: " +
                "the KDC SHOULD, in the encrypted pre-auth data part ([Referrals-11], Appendix A) of the AS-REP message, " +
                "include PA-DATA with the padata-type set to PA-SUPPORTED-ENCTYPES (165), and the padata-value set to 0x1F (section 2.2.6).");
            if (testConfig.IsClaimSupported)
            {
                PaSupportedEncTypes paSupportedEncTypes = null;
                // userKrbAsRep.EncPart.pa_datas.GetType

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
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send FAST armored TGS request.");
            client.SendTgsRequestWithFast(testConfig.LocalRealm.FileServer[0].Smb2ServiceName, options, null, null, subkey, fastOptions, apOptions);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve TGS response.");
            krbError = client.ExpectKrbError();

            // Access check with Computer's A2A2 failed, KDC will return the KDC_ERR_POLICY error.
            BaseTestSite.Assert.AreEqual(
                KRB_ERROR_CODE.KDC_ERR_POLICY,
                krbError.ErrorCode,
                "Section 3.3.5.7 TGS Exchange: " +
                "If AllowedToAuthenticateFrom is not NULL, the PAC of the armor TGT MUST be used to perform an access check for the ACTRL_DS_CONTROL_ACCESS right with additional rights GUID against the AllowedToAuthenticateFrom." + 
                "If the access check fails, the KDC MUST return KDC_ERR_POLICY."
                );

        }
        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12R2)]
        [TestCategory(TestCategories.Silo)]         
        [Feature(Feature.Silo)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the KDC behavior when Network logon with Computer's A2A2 is defined in Authentication Policy.")]
        //user16, testsilo02, in protected users group, with no usedesonly configured, in AuthSilo and DepartmentPolicy
        //Will succeed to get TGT with AES256 as pre-authentication etype
        //Computer.A2A2: User.department==HR. testsilo02 matches this condition                
        public void Protected_Users_Network_Logon_Computer_A2A2_Succeed()
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
            PaPacRequest paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data, paPacOptions.Data });
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                string.Format("The type of AS-REP encrypted part is {0}.", asResponse.EncPart.GetType().Name));

            // Switch to user principal
            client = new KerberosTestClient(
                this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[16].Username,
                this.testConfig.LocalRealm.User[16].Password,
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

            Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paPacRequest.Data, paPacOptions.Data });
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
            Asn1SequenceOf<PA_DATA> seqOfPaData3 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp3.Data, paPacRequest.Data, paPacOptions.Data});
            client.SendAsRequestWithFast(options, seqOfPaData3, null, subkey, fastOptions, apOptions);
            KerberosAsResponse userKrbAsRep = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(userKrbAsRep.Response.ticket, "AS response should contain a TGT.");
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                string.Format("The type of AS-REP encrypted part is {0}.", userKrbAsRep.EncPart.GetType().Name));

            // verify the PA-DATA of AS_REP: asResponse.EncPart.pa_datas as PA_SUPPORTED_ENCTYPES
            bool isExistPaSupportedEncTypes = false;
            bool isExistPadataValue = false;

            var padataCount = userKrbAsRep.EncPart.pa_datas.Elements.Length;
            for (int i = 0; i < padataCount; i++)
            {
                var padata = PaDataParser.ParseRepPaData(userKrbAsRep.EncPart.pa_datas.Elements[i]);
                if ((PaDataType)padata.Data.padata_type.Value == PaDataType.PA_SUPPORTED_ENCTYPES)
                {
                    isExistPaSupportedEncTypes = true;
                    for (int j = 0; j < padata.Data.padata_value.Value.Length; j++)
                    {
                        if (padata.Data.padata_value.Value[j] == 0x1F)
                        {
                            isExistPadataValue = true;
                        }
                    }
                }
            }
            BaseTestSite.Assert.IsTrue(isExistPaSupportedEncTypes, "If domainControllerFunctionality returns a value >= 3:" +
                "the KDC SHOULD, in the encrypted pre-auth data part ([Referrals-11], Appendix A) of the AS-REP message, " +
                "include PA-DATA with the padata-type set to PA-SUPPORTED-ENCTYPES (165).");

            BaseTestSite.Assert.IsTrue(isExistPadataValue, "If domainControllerFunctionality returns a value >= 3: " +
                "the KDC SHOULD, in the encrypted pre-auth data part ([Referrals-11], Appendix A) of the AS-REP message, " +
                "include PA-DATA with the padata-type set to PA-SUPPORTED-ENCTYPES (165), and the padata-value set to 0x1F (section 2.2.6).");

            if (testConfig.IsClaimSupported)
            {
                PaSupportedEncTypes paSupportedEncTypes = null;
                // userKrbAsRep.EncPart.pa_datas.GetType

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
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send FAST armored TGS request.");
            client.SendTgsRequestWithFast(testConfig.LocalRealm.FileServer[0].Smb2ServiceName, options, null, null, subkey, fastOptions, apOptions);
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
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12R2)]
        [TestCategory(TestCategories.Silo)] 
        [Feature(Feature.Silo)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the KDC behavior when interactive logon with Computer's A2A2 is defined in Authentication Policy.")]
        //user17, testsilo03, in protected users group, with no usedesonly configured, in AuthSilo and AuthPolicy
        //Will succeed to get TGT with AES256 as pre-authentication etype
        //Computer.A2A2 User.department==HR. testsilo03 doesn't matches this condition                
        public void Protected_Users_Interactive_Logon_Computer_A2A2_Fail()
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
               testConfig.LocalRealm.FileServer[0].AccountSalt);

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
            PaPacRequest paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data, paPacOptions.Data });
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                string.Format("The type of AS-REP encrypted part is {0}.", asResponse.EncPart.GetType().Name));

            // Switch to user principal
            client = new KerberosTestClient(
                this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[17].Username,
                this.testConfig.LocalRealm.User[17].Password,
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

            Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paPacRequest.Data, paPacOptions.Data });
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
            Asn1SequenceOf<PA_DATA> seqOfPaData3 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp3.Data, paPacRequest.Data, paPacOptions.Data });
            client.SendAsRequestWithFast(options, seqOfPaData3, null, subkey, fastOptions, apOptions);
            KerberosAsResponse userKrbAsRep = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(userKrbAsRep.Response.ticket, "AS response should contain a TGT.");
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                string.Format("The type of AS-REP encrypted part is {0}.", userKrbAsRep.EncPart.GetType().Name));

            // verify the PA-DATA of AS_REP: asResponse.EncPart.pa_datas as PA_SUPPORTED_ENCTYPES
            bool isExistPaSupportedEncTypes = false;
            bool isExistPadataValue = false;

            var padataCount = userKrbAsRep.EncPart.pa_datas.Elements.Length;
            for (int i = 0; i < padataCount; i++)
            {
                var padata = PaDataParser.ParseRepPaData(userKrbAsRep.EncPart.pa_datas.Elements[i]);
                if ((PaDataType)padata.Data.padata_type.Value == PaDataType.PA_SUPPORTED_ENCTYPES)
                {
                    isExistPaSupportedEncTypes = true;
                    for (int j = 0; j < padata.Data.padata_value.Value.Length; j++)
                    {
                        if (padata.Data.padata_value.Value[j] == 0x1F)
                        {
                            isExistPadataValue = true;
                        }
                    }
                }
            }
            BaseTestSite.Assert.IsTrue(isExistPaSupportedEncTypes, "If domainControllerFunctionality returns a value >= 3:" +
                  "the KDC SHOULD, in the encrypted pre-auth data part ([Referrals-11], Appendix A) of the AS-REP message, " +
                  "include PA-DATA with the padata-type set to PA-SUPPORTED-ENCTYPES (165).");

            BaseTestSite.Assert.IsTrue(isExistPadataValue, "If domainControllerFunctionality returns a value >= 3: " +
                "the KDC SHOULD, in the encrypted pre-auth data part ([Referrals-11], Appendix A) of the AS-REP message, " +
                "include PA-DATA with the padata-type set to PA-SUPPORTED-ENCTYPES (165), and the padata-value set to 0x1F (section 2.2.6).");

            if (testConfig.IsClaimSupported)
            {
                PaSupportedEncTypes paSupportedEncTypes = null;
                // userKrbAsRep.EncPart.pa_datas.GetType

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
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send FAST armored TGS request.");
            client.SendTgsRequestWithFast(testConfig.LocalRealm.ClientComputer.DefaultServiceName, options, null, null, subkey, fastOptions, apOptions);
            
            // Access check with Computer's A2A2 failed, KDC will return the KDC_ERR_POLICY error.
           
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve TGS response: KDC_ERR_POLICY.");
            KerberosKrbError krbError = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_POLICY, krbError.ErrorCode, "Blue Changes: Section 3.3.5.7 TGS Exchange: " +
                "If AllowedToAuthenticateTo is not NULL, the PAC of the user and the PAC of the armor TGT MUST be used to perform an access check " +
                "for the ACTRL_DS_CONTROL_ACCESS right with additional rights GUID against the AllowedToAuthenticateTo." +
                "If the access check fails, the KDC MUST return KDC_ERR_POLICY.");
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12R2)]
        [TestCategory(TestCategories.Silo)] 
        [Feature(Feature.Silo)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the KDC behavior when interactive logon with Computer's A2A2 is defined in Authentication Policy.")]
        // testsilo06, department=IT, in protected users group, with no usedesonly configured, in AuthSilo but no AuthPolicy
        //Will succeed to get TGT with AES256 as pre-authentication etype
        //computer.A2A2 User.department==HR or user.department==IT testsilo06 matches this condition                
        public void Protected_Users_Interactive_Logon_Computer_A2A2_Succeed()
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
            PaPacRequest paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data, paPacOptions.Data });
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                string.Format("The type of AS-REP encrypted part is {0}.", asResponse.EncPart.GetType().Name));

            // Switch to user principal
            client = new KerberosTestClient(
                this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[19].Username,
                this.testConfig.LocalRealm.User[19].Password,
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

            Asn1SequenceOf<PA_DATA> seqOfPaData2 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paPacRequest.Data, paPacOptions.Data });
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
            Asn1SequenceOf<PA_DATA> seqOfPaData3 = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp3.Data, paPacRequest.Data, paPacOptions.Data });
            client.SendAsRequestWithFast(options, seqOfPaData3, null, subkey, fastOptions, apOptions);
            KerberosAsResponse userKrbAsRep = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(userKrbAsRep.Response.ticket, "AS response should contain a TGT.");
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                string.Format("The type of AS-REP encrypted part is {0}.", userKrbAsRep.EncPart.GetType().Name));
            
            //verify flags in as reponse
            int flags = KerberosUtility.ConvertFlags2Int(asResponse.EncPart.flags.ByteArrayValue);
            BaseTestSite.Assert.AreEqual((EncTicketFlags)0,
                EncTicketFlags.FORWARDABLE & EncTicketFlags.PROXIABLE & (EncTicketFlags)flags,
                "Section  3.3.5.1: If domaincontrollerFunctionality returns a value >= 6 and the principal is a member of PROTECTED_USERS, the KILE KDC MUST NOT set the  PROXIABLE or FORWARDABLE ticket flags");
               
            //Get Computer's Authentication Policy Name
            string attributeDN = this.sutController.getAccountAttributeDN(
               this.testConfig.LocalRealm.RealmName.ToLower().Substring(0,this.testConfig.LocalRealm.RealmName.Length-4),
               this.testConfig.LocalRealm.ClientComputer.NetBiosName.Substring(0, this.testConfig.LocalRealm.ClientComputer.NetBiosName.Length - 1),
               "Computers",
               "msDS-AssignedAuthNPolicy",
               this.testConfig.LocalRealm.Admin.Username,
               this.testConfig.LocalRealm.Admin.Password
               );
            char seperator = ',';
            string computerLinkedPolicyName = attributeDN.Substring(3, attributeDN.IndexOf(seperator) - 3);


            //Get Computer's TGT lifetime of Authentication Policy
            double policyTGTRenewTime = 7 * 24 * 36000000000; // the default value is 7 days defined in DC group policy.

            double? policyTGTLifetime4Computer = this.sutController.getAuthPolicyTGTLifeTime(
                this.testConfig.LocalRealm.RealmName.ToLower().Substring(0, this.testConfig.LocalRealm.RealmName.Length - 4),
                computerLinkedPolicyName,
                "msds-ComputerTGTLifetime",
                this.testConfig.LocalRealm.Admin.Username,
                this.testConfig.LocalRealm.Admin.Password
                );

            if (policyTGTLifetime4Computer == null)
            {
                // if policy not config the TGT life time, use the default value 4 hours for protected users. 
                // if not a protected user, use the default value 10 hours by DC group policy defined.                    
                policyTGTLifetime4Computer = 10 * 36000000000;
            }
            else
            {
                policyTGTRenewTime = (double)policyTGTLifetime4Computer;
            }

            //Get TGTlifetime from ticket received
            DateTime startTime = DateTime.ParseExact(
                asResponse.EncPart.starttime.ToString(),
                "yyyyMMddHHmmssZ",
                System.Globalization.CultureInfo.InvariantCulture
                );
            DateTime endTime = DateTime.ParseExact(
                asResponse.EncPart.endtime.ToString(),
                "yyyyMMddHHmmssZ",
                System.Globalization.CultureInfo.InvariantCulture
                );
            DateTime renewTime = DateTime.ParseExact(
                asResponse.EncPart.renew_till.ToString(),
                "yyyyMMddHHmmssZ",
                System.Globalization.CultureInfo.InvariantCulture
                );

            //get timespan of as response
            TimeSpan maxTGTlife = endTime - startTime;
            TimeSpan maxRenewTime = renewTime- startTime;
            
            double? maxTGTlifeTimeSpan = maxTGTlife.Ticks;
            double? maxRenewTimeSpan = maxRenewTime.Ticks;

            //verify MaxRenewAge and MaxTicketAge for the TGT
            BaseTestSite.Assert.AreEqual(
                policyTGTLifetime4Computer,
                maxTGTlifeTimeSpan,
                "Section 3.3.5.6 MaxTicketAge (section 3.3.1) for the TGT is 4 hours unless specified by policy. " +
                "And if TGTLifetime is not 0: MaxTicketAge for the TGT is TGTLifetime."
                );
            BaseTestSite.Assert.AreEqual(
                policyTGTRenewTime,
                maxRenewTimeSpan,
                "Section 3.3.5.6 MaxRenewAge (section 3.3.1) for the TGT is 4 hours unless specified by policy. " +
                "And if TGTLifetime is not 0: MaxRenewAge for the TGT is TGTLifetime.");

            // verify the PA-DATA of AS_REP: asResponse.EncPart.pa_datas as PA_SUPPORTED_ENCTYPES
            bool isExistPaSupportedEncTypes = false;
            bool isExistPadataValue = false;

            var padataCount = userKrbAsRep.EncPart.pa_datas.Elements.Length;
            for (int i = 0; i < padataCount; i++)
            {
                var padata = PaDataParser.ParseRepPaData(userKrbAsRep.EncPart.pa_datas.Elements[i]);
                if ((PaDataType)padata.Data.padata_type.Value == PaDataType.PA_SUPPORTED_ENCTYPES)
                {
                    isExistPaSupportedEncTypes = true;
                    for (int j = 0; j < padata.Data.padata_value.Value.Length; j++)
                    {
                        if (padata.Data.padata_value.Value[j] == 0x1F)
                        {
                            isExistPadataValue = true;
                        }
                    }

                }
            }

            BaseTestSite.Assert.IsTrue(isExistPaSupportedEncTypes, "If domainControllerFunctionality returns a value >= 3:" +
                   "the KDC SHOULD, in the encrypted pre-auth data part ([Referrals-11], Appendix A) of the AS-REP message, " +
                   "include PA-DATA with the padata-type set to PA-SUPPORTED-ENCTYPES (165).");

            BaseTestSite.Assert.IsTrue(isExistPadataValue, "If domainControllerFunctionality returns a value >= 3: " +
                "the KDC SHOULD, in the encrypted pre-auth data part ([Referrals-11], Appendix A) of the AS-REP message, " +
                "include PA-DATA with the padata-type set to PA-SUPPORTED-ENCTYPES (165), and the padata-value set to 0x1F (section 2.2.6).");
           
            // FAST armored TGS_REQ and TGS_REP using user principal
            subkey = KerberosUtility.MakeKey(client.Context.SelectedEType, "Password03!", "this is a salt");
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send FAST armored TGS request.");
            client.SendTgsRequestWithFast(testConfig.LocalRealm.ClientComputer.DefaultServiceName, options, null, null, subkey, fastOptions, apOptions);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Recieve TGS response.");
            KerberosTgsResponse userKrbTgsRep = client.ExpectTgsResponse(KeyUsageNumber.TGS_REP_encrypted_part_subkey);

            //verify the tgs response is received.
            BaseTestSite.Assert.IsNotNull(userKrbTgsRep.Response, "The Response pare of TGS-REP is not null.");
            BaseTestSite.Assert.IsNotNull(userKrbTgsRep.Response.padata, "The Padata of TGS-REP is not null.");            

        }       
    }
}
