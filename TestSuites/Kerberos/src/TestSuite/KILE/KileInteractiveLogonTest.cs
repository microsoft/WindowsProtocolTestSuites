// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocol.TestSuites.Kerberos.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using Microsoft.Protocols.TestTools.StackSdk.Security.Pac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocol.TestSuites.Kerberos.TestSuite.KILE
{
    [TestClass]
    public class KileInteractiveLogonTest : TraditionTestBase
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

        #region Basic
        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.BVT)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify if the KDC client and server connection can be switched from UDP to TCP when dealing with too big responses.")]
        public void UDPtoTCP()
        {
            base.Logging();

            //Create kerberos test client and connect
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName, 
                this.testConfig.LocalRealm.User[1].Username,
                this.testConfig.LocalRealm.User[1].Password, 
                KerberosAccountType.User, 
                testConfig.LocalRealm.KDC[0].IPAddress, 
                testConfig.LocalRealm.KDC[0].Port, 
                Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.TransportType.UDP,
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
            //Recieve preauthentication required error
            krbError = client.ExpectKrbError();
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Receive response too big error.");
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KRB_ERR_RESPONSE_TOO_BIG, 
                krbError.ErrorCode, 
                "If the response cannot be handled using UDP, the KDC MUST return KRB_ERR_RESPONSE_TOO_BIG, forcing the client to retry the request using the TCP transport.");

            //Connect using TCP
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName, this.testConfig.LocalRealm.User[1].Username,
                this.testConfig.LocalRealm.User[1].Password, KerberosAccountType.User, testConfig.LocalRealm.KDC[0].IPAddress, testConfig.LocalRealm.KDC[0].Port, Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.TransportType.TCP,
                testConfig.SupportedOid);
            
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]        
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify if the unknown pre-authentication types can be ignored by the KDC.")]
        public void UnknownPaType()
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

            //Create an unknown padata type
            int invalidPaDataType = 1234567890;
            byte[] invalidPaDataValue = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            PA_DATA invalidPaData = new PA_DATA(new KerbInt32(invalidPaDataType), new Asn1OctetString(invalidPaDataValue));
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data, invalidPaData });
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create a sequence of PA data with invalid PA data.");
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "Unknown pre-authentication types MUST be ignored by KDCs. AS response should contain a TGT.");

            //Create and send TGS request
            client.SendTgsRequest(this.testConfig.LocalRealm.ClientComputer.DefaultServiceName, options);
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
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]        
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify if the KDC supports client initiated password-based authentication and whether the KDC supports AES256 encryption type.")]
        public void UsingPasswordWithAES256()
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
           
            //Define client supported encryption type
            EncryptionType[] encryptionTypes = new EncryptionType[]
            { 
                EncryptionType.AES256_CTS_HMAC_SHA1_96
            };
            Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32[] etypes =
                new Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32[encryptionTypes.Length];
            for (int i = 0; i < encryptionTypes.Length; i++)
            {
                etypes[i] = new Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32((int)encryptionTypes[i]);
            }
            Asn1SequenceOf<KerbInt32> etype = new Asn1SequenceOf<KerbInt32>(etypes);
            client.Context.SupportedEType = etype;

            //Create and send AS request
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);
            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);

            //Validate the methodData in preauthentication required error message
            bool isExistPaEncTimestamp = false;
            bool isExistPaEtypeInfo2 = false;
            var padataCount = methodData.Elements.Length;
            for (int i = 0; i < padataCount; i++)
            {
                var padata = PaDataParser.ParseRepPaData(methodData.Elements[i]);
                if ((PaDataType)padata.Data.padata_type.Value == PaDataType.PA_ENC_TIMESTAMP)
                {
                    isExistPaEncTimestamp = true;
                }
                else if (padata is PaETypeInfo2)
                {
                    var etypeinfo = padata as PaETypeInfo2;
                    foreach (var entry in etypeinfo.ETypeInfo2.Elements)
                    {
                        if ((EncryptionType)entry.etype.Value == EncryptionType.AES256_CTS_HMAC_SHA1_96)
                        {
                            isExistPaEtypeInfo2 = true;
                        }
                    }
                }
            }
            BaseTestSite.Assert.IsTrue(isExistPaEncTimestamp, "When clients perform a password-based initial authentication, they MUST supply the PA-ENC-TIMESTAMP pre-authentication type when they construct the initial AS request.");
            BaseTestSite.Assert.IsTrue(isExistPaEtypeInfo2, "KILE SHOULD support the Advanced Encryption Standard (AES) encryption types: AES256-CTS-HMAC-SHA1-96.");

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, this.client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.AreEqual(EncryptionType.AES256_CTS_HMAC_SHA1_96, (EncryptionType)asResponse.Response.enc_part.etype.Value, "The encrypted part of the AS reply should be encrypted with AES256.");
            //asResponse.EncPart.pa_datas as PA_SUPPORTED_ENCTYPES

            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create and send TGS request
            client.SendTgsRequest(this.testConfig.LocalRealm.ClientComputer.DefaultServiceName, options);
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
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify if the KDC supports client initiated password-based authentication and whether the KDC supports AES128 encryption type.")]
        public void UsingPasswordWithAES128()
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
           
            //Define client supported encryption type
            EncryptionType[] encryptionTypes = new EncryptionType[]
            { 
                EncryptionType.AES128_CTS_HMAC_SHA1_96
            };
            Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32[] etypes =
                new Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32[encryptionTypes.Length];
            for (int i = 0; i < encryptionTypes.Length; i++)
            {
                etypes[i] = new Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32((int)encryptionTypes[i]);
            }
            Asn1SequenceOf<KerbInt32> etype = new Asn1SequenceOf<KerbInt32>(etypes);
            client.Context.SupportedEType = etype;

            //Create and send AS request
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);
            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);
            
            //Validate the methodData in preauthentication required error message
            bool isExistPaEncTimestamp = false;
            bool isExistPaEtypeInfo2 = false;
            var padataCount = methodData.Elements.Length;
            for (int i = 0; i < padataCount; i++)
            {
                var padata = PaDataParser.ParseRepPaData(methodData.Elements[i]);
                if ((PaDataType)padata.Data.padata_type.Value == PaDataType.PA_ENC_TIMESTAMP)
                {
                    isExistPaEncTimestamp = true;
                }
                else if (padata is PaETypeInfo2)
                {
                    var etypeinfo = padata as PaETypeInfo2;
                    foreach (var entry in etypeinfo.ETypeInfo2.Elements)
                    {
                        if ((EncryptionType)entry.etype.Value == EncryptionType.AES128_CTS_HMAC_SHA1_96)
                        {
                            isExistPaEtypeInfo2 = true;
                        }
                    }
                }
            }
            BaseTestSite.Assert.IsTrue(isExistPaEncTimestamp, "When clients perform a password-based initial authentication, they MUST supply the PA-ENC-TIMESTAMP pre-authentication type when they construct the initial AS request.");
            BaseTestSite.Assert.IsTrue(isExistPaEtypeInfo2, "KILE SHOULD support the Advanced Encryption Standard (AES) encryption types: AES128-CTS-HMAC-SHA1-96.");

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, this.client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
             KerberosAsResponse asResponse = client.ExpectAsResponse();
             BaseTestSite.Assert.AreEqual(EncryptionType.AES128_CTS_HMAC_SHA1_96, (EncryptionType)asResponse.Response.enc_part.etype.Value, "The encrypted part of the AS reply should be encrypted with AES256.");            
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create and send TGS request
            client.SendTgsRequest(this.testConfig.LocalRealm.ClientComputer.DefaultServiceName, options);
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
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify if the KDC supports client initiated password-based authentication and whether the KDC supports RC4-HMAC encryption type.")]
        public void UsingPasswordWithRC4HMAC()
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
           
            //Define client supported encryption type
            EncryptionType[] encryptionTypes = new EncryptionType[]
            { 
                EncryptionType.RC4_HMAC
            };
            Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32[] etypes =
                new Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32[encryptionTypes.Length];
            for (int i = 0; i < encryptionTypes.Length; i++)
            {
                etypes[i] = new Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32((int)encryptionTypes[i]);
            }
            Asn1SequenceOf<KerbInt32> etype = new Asn1SequenceOf<KerbInt32>(etypes);
            client.Context.SupportedEType = etype;

            //Create and send AS request
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);
            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);
            
            //Validate the methodData in preauthentication required error message
            bool isExistPaEncTimestamp = false;
            bool isExistPaEtypeInfo2 = false;
            var padataCount = methodData.Elements.Length;
            for (int i = 0; i < padataCount; i++)
            {
                var padata = PaDataParser.ParseRepPaData(methodData.Elements[i]);
                if ((PaDataType)padata.Data.padata_type.Value == PaDataType.PA_ENC_TIMESTAMP)
                {
                    isExistPaEncTimestamp = true;
                }
                else if (padata is PaETypeInfo2)
                {
                    var etypeinfo = padata as PaETypeInfo2;
                    foreach (var entry in etypeinfo.ETypeInfo2.Elements)
                    {
                        if ((EncryptionType)entry.etype.Value == EncryptionType.RC4_HMAC)
                        {
                            isExistPaEtypeInfo2 = true;
                        }
                    }
                }
            }
            BaseTestSite.Assert.IsTrue(isExistPaEncTimestamp, "When clients perform a password-based initial authentication, they MUST supply the PA-ENC-TIMESTAMP pre-authentication type when they construct the initial AS request.");
            BaseTestSite.Assert.IsTrue(isExistPaEtypeInfo2, "KILE MAY support the other following encryption types: RC4-HMAC[23].");

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, this.client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.AreEqual(EncryptionType.RC4_HMAC, (EncryptionType)asResponse.Response.enc_part.etype.Value, "The encrypted part of the AS reply should be encrypted with AES256.");
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create and send TGS request
            client.SendTgsRequest(this.testConfig.LocalRealm.ClientComputer.DefaultServiceName, options);
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
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify if the KDC supports client initiated password-based authentication and whether the KDC supports DES-CBC-MD5 encryption type.")]
        public void UsingPasswordWithDES()
        {
            base.Logging();

            //Create kerberos test client and connect
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName, this.testConfig.LocalRealm.User[14].Username,
                this.testConfig.LocalRealm.User[14].Password, KerberosAccountType.User, testConfig.LocalRealm.KDC[0].IPAddress, testConfig.LocalRealm.KDC[0].Port, testConfig.TransportType,
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

            //Define client supported encryption type
            EncryptionType[] encryptionTypes = new EncryptionType[]
            { 
                EncryptionType.DES_CBC_MD5
            };
            Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32[] etypes =
                new Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32[encryptionTypes.Length];
            for (int i = 0; i < encryptionTypes.Length; i++)
            {
                etypes[i] = new Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32((int)encryptionTypes[i]);
            }
            Asn1SequenceOf<KerbInt32> etype = new Asn1SequenceOf<KerbInt32>(etypes);
            client.Context.SupportedEType = etype;

            //Create and send AS request
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);

            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError;
            krbError = client.ExpectPreauthRequiredError(out methodData);

            //Validate the methodData in preauthentication required error message
            bool isExistPaEncTimestamp = false;
            bool isExistPaEtypeInfo2 = false;
            var padataCount = methodData.Elements.Length;
            for (int i = 0; i < padataCount; i++)
            {
                var padata = PaDataParser.ParseRepPaData(methodData.Elements[i]);
                if ((PaDataType)padata.Data.padata_type.Value == PaDataType.PA_ENC_TIMESTAMP)
                {
                    isExistPaEncTimestamp = true;
                }
                else if (padata is PaETypeInfo2)
                {
                    var etypeinfo = padata as PaETypeInfo2;
                    foreach (var entry in etypeinfo.ETypeInfo2.Elements)
                    {
                        if ((EncryptionType)entry.etype.Value == EncryptionType.DES_CBC_MD5)
                        {
                            isExistPaEtypeInfo2 = true;
                        }
                    }
                }
            }
            BaseTestSite.Assert.IsTrue(isExistPaEncTimestamp, "When clients perform a password-based initial authentication, they MUST supply the PA-ENC-TIMESTAMP pre-authentication type when they construct the initial AS request.");
            BaseTestSite.Assert.IsTrue(isExistPaEtypeInfo2, "KILE MAY support the other following encryption types: DES-CBC-MD5[3].");

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, this.client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.AreEqual(EncryptionType.DES_CBC_MD5, (EncryptionType)asResponse.Response.enc_part.etype.Value, "The encrypted part of the AS reply should be encrypted with DES.");
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create and send TGS request
            client.SendTgsRequest(this.testConfig.LocalRealm.ClientComputer.DefaultServiceName, options);
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
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[14].Username.ToLower(),
                KerberosUtility.PrincipalName2String(tgsResponse.TicketEncPart.cname).ToLower(),
                "The client principal name in ticket encrypted part should match expected.");
            
            
        }
        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify DES downgrade protection.")]
        public void DesDowngradeProtection()
        {
            base.Logging();

            //Create kerberos test client and connect
            //UseDESOnly is not set for user[1]
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

            //Define client supported encryption type
            EncryptionType[] encryptionTypes = new EncryptionType[]
                { 
                    EncryptionType.DES_CBC_MD5
                };
            Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32[] etypes =
                new Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32[encryptionTypes.Length];
            for (int i = 0; i < encryptionTypes.Length; i++)
            {
                etypes[i] = new Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.KerbInt32((int)encryptionTypes[i]);
            }
            Asn1SequenceOf<KerbInt32> etype = new Asn1SequenceOf<KerbInt32>(etypes);
            client.Context.SupportedEType = etype;

            //Create and send AS request
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);

            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError;            

            //DES downgrade protection is not supported in Windows 2000, Windows Server 2003, Windows Server 2008, Windows Server 2008 R2, or Windows Server 2012 KDCs.            
            if (int.Parse(this.testConfig.LocalRealm.DomainControllerFunctionality) < 6)
            {
                krbError = client.ExpectPreauthRequiredError(out methodData); //pre-authentication error                
            }
            //For Windows 2012R2 or later: If DES is used for pre-authentication, the KDC MUST:
            //If UseDESOnly is not set: the KDC MUST return KDC_ERR_ETYPE_NOTSUPP.
            else
            {                               
                krbError = client.ExpectKrbError();
                BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_ETYPE_NOTSUPP,
                    krbError.ErrorCode,
                    "If UseDESOnly is not set: the KDC MUST return KDC_ERR_ETYPE_NOTSUPP");             

            }
    }


        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify if the INITIAL and PRE-AUTHENT ticket flags are supported by the KDC.")]
        public void InitialAndPreAuthentFlag()
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
            int flags = KerberosUtility.ConvertFlags2Int(asResponse.EncPart.flags.ByteArrayValue);
            BaseTestSite.Assert.AreEqual(EncTicketFlags.INITIAL,
                EncTicketFlags.INITIAL & (EncTicketFlags)flags, 
                "The INITIAL flag should be set. This flag indicates that a ticket was issued using the AS protocol, rather than issued based on a TGT. Application servers that want to require the demonstrated knowledge of a client's secret key (e.g., a password-changing program) can insist that this flag be set in any tickets they accept, and can thus be assured that the client's key was recently presented to the authentication server.");
            BaseTestSite.Assert.AreEqual(EncTicketFlags.PRE_AUTHENT, 
                EncTicketFlags.PRE_AUTHENT & (EncTicketFlags)flags, 
                "The PRE_AUTHENT flag should be set. By default, KDCs require pre-authentication when they issue tickets. Clients SHOULD pre-authenticate. KDCs MUST enforce pre-authentication. Therefore, unless the account has been explicitly set to not require Kerberos pre-authentication, the ticket will have the PRE-AUTHENT flag set.");
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create and send TGS request
            client.SendTgsRequest(this.testConfig.LocalRealm.ClientComputer.DefaultServiceName, options);
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
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify if the HW-AUTHENT ticket flag is supported by the KDC.")]
        public void HwAuthentFlag()
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

            KdcOptions options = KdcOptions.OPTHARDWAREAUTH | KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
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
            int flags = KerberosUtility.ConvertFlags2Int(asResponse.EncPart.flags.ByteArrayValue);
            BaseTestSite.Assert.AreEqual((EncTicketFlags)0,
                EncTicketFlags.HW_AUTHENT & (EncTicketFlags)flags, 
                "The HWAUTHENT flag should be set. This flag was originally intended to indicate that hardware-supported authentication was used during pre-authentication. This flag is no longer recommended in the Kerberos V5 protocol. KDCs MUST NOT issue a ticket with this flag set. KDCs SHOULD NOT preserve this flag if it is set by another KDC.");
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create and send TGS request
            client.SendTgsRequest(this.testConfig.LocalRealm.ClientComputer.DefaultServiceName, options);
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
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify if the RENEWABLE ticket flag is supported by the KDC.")]
        public void RenewableFlag()
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
            int flags = KerberosUtility.ConvertFlags2Int(asResponse.EncPart.flags.ByteArrayValue);
            BaseTestSite.Assert.AreEqual(EncTicketFlags.RENEWABLE, 
                EncTicketFlags.RENEWABLE & (EncTicketFlags)flags,
                "The RENEWABLE flag should be set. The RENEWABLE flag is reset by default, but a client MAY request it be set by setting the RENEWABLE option in the KRB_AS_REQ message.");
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create and send TGS request
            client.SendTgsRequest(this.testConfig.LocalRealm.ClientComputer.DefaultServiceName, options);
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
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify if the postdated tickets can be supported by the KDC.")]
        public void MayPostdateAndPostDatedFlag()
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

            KdcOptions options = KdcOptions.ALLOWPOSTDATE | KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
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

            KerberosPdu pdu = client.ExpectPdu(KerberosConstValue.TIMEOUT_DEFAULT);
            if (this.testConfig.IsKileImplemented)
            {
                BaseTestSite.Assert.IsInstanceOfType(pdu, typeof(KerberosKrbError), "Receive Kerberos Error. Postdated tickets SHOULD NOT be supported in KILE.");
            }
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify if the forwarded tickets can be supported by the KDC.")]
        public void ForwardableAndForwardedFlag()
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
            int flags = KerberosUtility.ConvertFlags2Int(asResponse.EncPart.flags.ByteArrayValue);
            BaseTestSite.Assert.AreEqual(EncTicketFlags.FORWARDABLE, 
                EncTicketFlags.FORWARDABLE & (EncTicketFlags)flags, 
                "The FORWARDABLE flag should be set. This flag is reset by default, but users MAY request that it be set by setting the FORWARDABLE option in the AS request when they request their initial TGT.");
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create and send TGS request
            options = KdcOptions.FORWARDED | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendTgsRequest(this.testConfig.LocalRealm.ClientComputer.DefaultServiceName, options);
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();
            flags = KerberosUtility.ConvertFlags2Int(tgsResponse.EncPart.flags.ByteArrayValue);
            BaseTestSite.Assert.AreEqual(EncTicketFlags.FORWARDED,
                EncTicketFlags.FORWARDED & (EncTicketFlags)flags,
                "The FORWARDED flag should be set. The FORWARDED flag is set by the TGS when a client presents a ticket with the FORWARDABLE flag set and requests a forwarded ticket by specifying the FORWARDED KDC option and supplying a set of addresses for the new ticket.");

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
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify if the OK-AS-DELEGATE ticket flag can be supported by the KDC.")]
        public void OkAsDelegateFlag()
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
            client.SendTgsRequest(this.testConfig.LocalRealm.ClientComputer.DefaultServiceName, options);
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();
            int flags = KerberosUtility.ConvertFlags2Int(tgsResponse.EncPart.flags.ByteArrayValue);
            BaseTestSite.Assert.AreEqual(EncTicketFlags.OK_AS_DELEGATE,
                EncTicketFlags.OK_AS_DELEGATE & (EncTicketFlags)flags,
                "The OK-AS-DELEGATE flag should be set. The copy of the ticket flags in the encrypted part of the KDC reply may have the OK-AS-DELEGATE flag set to indicate to the client that the server specified in the ticket has been determined by the policy of the realm to be a suitable recipient of the delegation.");

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
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify if the KDC is case insensitive during the name comparisons, whether user names or domain names.")]
        public void CaseInsensitive()
        {
            base.Logging();

            //Create kerberos test client and connect
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName.ToUpper(), 
                this.testConfig.LocalRealm.User[1].Username.ToUpper(),
                this.testConfig.LocalRealm.User[1].Password, 
                KerberosAccountType.User, 
                testConfig.LocalRealm.KDC[0].IPAddress,
                testConfig.LocalRealm.KDC[0].Port,
                testConfig.TransportType,
                testConfig.SupportedOid, 
                testConfig.LocalRealm.User[1].Salt);

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
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Validate user name and realm name
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[1].Username.ToLower(),
                KerberosUtility.PrincipalName2String(asResponse.Response.cname).ToLower(), 
                "Name comparisons, whether for users or domains, MUST NOT be case sensitive in KILE.");
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.RealmName.ToLower(),
                asResponse.Response.crealm.Value.ToLower(), 
                "Name comparisons, whether for users or domains, MUST NOT be case sensitive in KILE.");

            //Create and send TGS request
            client.SendTgsRequest(this.testConfig.LocalRealm.ClientComputer.DefaultServiceName, options);
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
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify if the requirement that KDC MUST NOT issue a proxiable or forwardable ticket when user is set to delegation not allowed.")]
        public void UserDelegationNotAllowed()
        {
            base.Logging();

            //Create kerberos test client and connect
            //Use the UserDelegNotAllowed user account
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[3].Username,
                this.testConfig.LocalRealm.User[3].Password,
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

            KdcOptions options = KdcOptions.PROXIABLE | KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
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
            int flags = KerberosUtility.ConvertFlags2Int(asResponse.EncPart.flags.ByteArrayValue);
            BaseTestSite.Assert.AreEqual((EncTicketFlags)0, 
                EncTicketFlags.PROXIABLE & (EncTicketFlags)flags,
                "If DelegationNotAllowed is set to TRUE on the principal, the KILE KDC MUST NOT issue a PROXIABLE or FORWARDABLE ticket flags.");
            BaseTestSite.Assert.AreEqual((EncTicketFlags)0,
                EncTicketFlags.FORWARDABLE & (EncTicketFlags)flags, 
                "If DelegationNotAllowed is set to TRUE on the principal, the KILE KDC MUST NOT issue a PROXIABLE or FORWARDABLE ticket flags.");
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create and send TGS request
            client.SendTgsRequest(this.testConfig.LocalRealm.ClientComputer.DefaultServiceName, options);
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
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[3].Username.ToLower(),
                KerberosUtility.PrincipalName2String(tgsResponse.TicketEncPart.cname).ToLower(),
                "The client principal name in ticket encrypted part should match expected.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the requirement that KDC MUST set OK-AS-DELEGATE ticket flag if user is set to trust for delegation.")]
        public void UserTrustedForDelegation()
        {
            base.Logging();

            //Create kerberos test client and connect
            //Use the UserTrustedForDeleg user account
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[4].Username,
                this.testConfig.LocalRealm.User[4].Password,
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
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create and send TGS request for the test service mapped to UserTrustedForDeleg account
            client.SendTgsRequest(this.testConfig.LocalRealm.User[4].ServiceName, options);

            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();
            //Validate ticket flags
            int flags = KerberosUtility.ConvertFlags2Int(tgsResponse.EncPart.flags.ByteArrayValue);
            BaseTestSite.Assert.AreEqual(EncTicketFlags.OK_AS_DELEGATE, 
                EncTicketFlags.OK_AS_DELEGATE & (EncTicketFlags)flags, 
                "If TrustedForDelegation is set to TRUE on the principal, the KILE KDC MUST set the OK-AS-DELEGATE ticket flag.");
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.RealmName.ToLower(),
                tgsResponse.Response.ticket.realm.Value.ToLower(),
                "The realm in ticket should match expected.");
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[4].ServiceName,
               KerberosUtility.PrincipalName2String(tgsResponse.Response.ticket.sname),
               "The Service principal name in ticket should match expected.");

            string serviceSalt = this.testConfig.LocalRealm.RealmName.ToUpper() + this.testConfig.LocalRealm.User[4].ServiceName.Replace("\\","").ToLower();
            tgsResponse.DecryptTicket(this.testConfig.LocalRealm.User[4].Password, serviceSalt);
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.RealmName.ToLower(),
               tgsResponse.TicketEncPart.crealm.Value.ToLower(),
               "The realm in ticket encrypted part should match expected.");
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[4].Username.ToLower(),
                KerberosUtility.PrincipalName2String(tgsResponse.TicketEncPart.cname).ToLower(),
                "The client principal name in ticket encrypted part should match expected.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify if user UPN_DNS_INFO is filled when UPN is not set in the user's AD account.")]
        public void UserWithoutUPN()
        {
            base.Logging();

            //Create kerberos test client and connect
            //Use the UserWithoutUPN user account
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName, 
                this.testConfig.LocalRealm.User[5].Username,
                this.testConfig.LocalRealm.User[5].Password, 
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
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create and send TGS request
            client.SendTgsRequest(this.testConfig.LocalRealm.ClientComputer.DefaultServiceName, options);
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
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[5].Username.ToLower(),
                KerberosUtility.PrincipalName2String(tgsResponse.TicketEncPart.cname).ToLower(),
                "The client principal name in ticket encrypted part should match expected.");

            //Validate upn in UPN_DNS_INFO structure
            BaseTestSite.Assert.IsNotNull(tgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
            AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(tgsResponse.TicketEncPart.authorization_data.Elements);
            BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2kPac.");
            UpnDnsInfo upnDnsInfo = null;
            foreach (var buf in adWin2kPac.Pac.PacInfoBuffers)
            {
                if (buf is UpnDnsInfo)
                {
                    upnDnsInfo = buf as UpnDnsInfo;
                    break;
                }
            }
            BaseTestSite.Assert.AreEqual((this.testConfig.LocalRealm.User[5].Username.ToLower() + "@" + this.testConfig.LocalRealm.RealmName).ToLower(), upnDnsInfo.Upn.ToLower(), "If the user account object does not have the userPrincipalName attribute set, the KDC SHOULD send a UPN_DNS_INFO structure containing a user principal name (UPN), constructed by concatenating the user name, the @ symbol, and the DNS name of the domain.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify if the user requires no authentication procedure when its account is set to PreAuthenticationNotRequired.")]
        public void UserPreAuthenticationNotRequired()
        {
            base.Logging();

            //Create kerberos test client and connect
            //Use the UserPreAuthNotReq user account
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[6].Username,
                this.testConfig.LocalRealm.User[6].Password,
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
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paPacRequest.Data });
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT. If Pre-AuthenticationNotRequired is set to TRUE on the principal, the KDC MUST issue a TGT without validating pre-authentication data provided.");

            //Create and send TGS request
            client.SendTgsRequest(this.testConfig.LocalRealm.ClientComputer.DefaultServiceName, options);
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
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[6].Username.ToLower(),
                KerberosUtility.PrincipalName2String(tgsResponse.TicketEncPart.cname).ToLower(),
                "The client principal name in ticket encrypted part should match expected.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify if the KDC support checking the account policy for every ticket request.")]
        public void UserAccountDisabled()
        {
            base.Logging();

            //Create kerberos test client and connect
            //Use the UserDisabled user account
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[7].Username,
                this.testConfig.LocalRealm.User[7].Password,
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

            //Expect Kerberos Error because user account is disabled
            KerberosKrbError krbError = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_CLIENT_REVOKED, 
                krbError.ErrorCode,
                "If the POLICY_KERBEROS_VALIDATE_CLIENT bit is set in the AuthenticationOptions setting on the KDC then KILE will enforce revocation on the KDCs. When this property is set on the KDC for the client's domain, and the TGT is older than an implementation specific time, the KDC MUST verify that the account is still in good standing. If Disable is TRUE, then the KDC MUST return KDC_ERR_CLIENT_REVOKED.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify if the KDC support checking the account policy for every ticket request.")]
        public void UserAccountExpired()
        {
            base.Logging();

            //Create kerberos test client and connect
            //Use the UserExpired user account
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName, 
                this.testConfig.LocalRealm.User[8].Username,
                this.testConfig.LocalRealm.User[8].Password, 
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

            //Expect Kerberos Error because user account is expired
            KerberosKrbError krbError = client.ExpectKrbError();
            BaseTestSite.Assert.IsTrue(krbError.ErrorCode == KRB_ERROR_CODE.KDC_ERR_CLIENT_REVOKED || krbError.ErrorCode == KRB_ERROR_CODE.KDC_ERR_NAME_EXP, 
                "If the POLICY_KERBEROS_VALIDATE_CLIENT bit is set in the AuthenticationOptions setting on the KDC then KILE will enforce revocation on the KDCs. When this property is set on the KDC for the client's domain, and the TGT is older than an implementation specific time, the KDC MUST verify that the account is still in good standing. If Expire is TRUE, then the KDC MUST return KDC_ERR_CLIENT_REVOKED.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify if the KDC support checking the account policy for every ticket request.")]
        public void UserAccountLocked()
        {
            base.Logging();

            //Create kerberos test client and connect
            //Use the UserLocked user account
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName, 
                this.testConfig.LocalRealm.User[9].Username,
                this.testConfig.LocalRealm.User[9].Password,
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

            //Expect Kerberos Error because user account is locked
            KerberosKrbError krbError = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_CLIENT_REVOKED, 
                krbError.ErrorCode,
                "If the POLICY_KERBEROS_VALIDATE_CLIENT bit is set in the AuthenticationOptions setting on the KDC then KILE will enforce revocation on the KDCs. When this property is set on the KDC for the client's domain, and the TGT is older than an implementation specific time, the KDC MUST verify that the account is still in good standing. If Locked is TRUE, then the KDC MUST return KDC_ERR_CLIENT_REVOKED.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify if the KDC support checking the account policy for every ticket request.")]
        public void UserAccountOutOfLogonHours()
        {
            base.Logging();

            //Create kerberos test client and connect
            //Use the UserOutofLogonHours user account
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName, 
                this.testConfig.LocalRealm.User[10].Username,
                this.testConfig.LocalRealm.User[10].Password,
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

            //Expect Kerberos Error message because user account is out of logon hours
            krbError = client.ExpectKrbError();
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_CLIENT_REVOKED,
                krbError.ErrorCode, 
                "If the POLICY_KERBEROS_VALIDATE_CLIENT bit is set in the AuthenticationOptions setting on the KDC then KILE will enforce revocation on the KDCs. When this property is set on the KDC for the client's domain, and the TGT is older than an implementation specific time, the KDC MUST verify that the account is still in good standing. If current time is not within the LogonHours, then the KDC MUST return KDC_ERR_CLIENT_REVOKED.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify if the KDC support checking the account policy for every ticket request.")]
        public void UserAccountPasswordMustChangePast()
        {
            base.Logging();

            //Create kerberos test client and connect
            //Use the UserAccountPwdChgPast user account
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName, 
                this.testConfig.LocalRealm.User[11].Username,
                this.testConfig.LocalRealm.User[11].Password,
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

            //Expect Kerberos Error message because user account PasswordMustChange is in the past
            KerberosKrbError krbError = client.ExpectKrbError();
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Receive a Kerberos error response.");
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_KEY_EXPIRED, 
                krbError.ErrorCode, 
                "If the POLICY_KERBEROS_VALIDATE_CLIENT bit is set in the AuthenticationOptions setting on the KDC then KILE will enforce revocation on the KDCs. When this property is set on the KDC for the client's domain, and the TGT is older than an implementation specific time, the KDC MUST verify that the account is still in good standing. If PasswordMustChange is set in the past, then the KDC MUST return KDC_ERR_KEY_EXPIRED.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify if the KDC support checking the account policy for every ticket request.")]
        public void UserAccountPasswordMustChangeZero()
        {
            base.Logging();

            //Create kerberos test client and connect
            //Use the UserPwdMustChgZero user account
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName, 
                this.testConfig.LocalRealm.User[12].Username,
                this.testConfig.LocalRealm.User[12].Password, 
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

            //Expect Kerberos Error message because user account PasswordMustChange is set to 0
            KerberosKrbError krbError = client.ExpectKrbError();
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Receive a Kerberos error response.");
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_KEY_EXPIRED, 
                krbError.ErrorCode, 
                "If the POLICY_KERBEROS_VALIDATE_CLIENT bit is set in the AuthenticationOptions setting on the KDC then KILE will enforce revocation on the KDCs. When this property is set on the KDC for the client's domain, and the TGT is older than an implementation specific time, the KDC MUST verify that the account is still in good standing. If PasswordMustChange is set to 0, then the KDC MUST return KDC_ERR_KEY_EXPIRED.");
        }
        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the KDC behavior when interactive logon with DES as pre-authentication.")]
        // user18, testsilo04, not a member of protected user, not configure use des only
        // Will failed with pre-authentication used DES
        public void UserAccount_DES_PreAuthentication_Fail()
        {
            base.Logging();

            //Create kerberos test client and connect
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[18].Username,
                this.testConfig.LocalRealm.User[18].Password,
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

            //Will receive the KDC_ERR_ETYP_NOTSUPP error from KDC
            KerberosKrbError krbError = null;


            //Recieve preauthentication required error
            METHOD_DATA methodData;
            krbError = client.ExpectPreauthRequiredError(out methodData);

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;

            //Use DES as pre-authentication encryption type
            EncryptionType clientEType = EncryptionType.DES_CBC_MD5;

            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp,
                0,
                clientEType,
                this.client.Context.CName.Password,
                this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });

            //Create and send AS request with DES as pre-authentication
            client.SendAsRequest(options, seqOfPaData);

            //Receive error from KDC
            krbError = client.ExpectKrbError();

            //Check KrbError error code against TD
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_ETYPE_NOTSUPP, krbError.ErrorCode, "Section 3.3.5.6 AS Exchange:" +
               "If DES is used for pre-authentication, the KDC MUST:<47>" +
               "If UseDESOnly is not set: the KDC MUST return KDC_ERR_ETYPE_NOTSUPP." +
               "Otherwise, if the account is:" +
               "krbtgt: the KDC MUST return KDC_ERR_ETYPE_NOTSUPP." +
               "The computer account of a KDC: the KDC MUST return KDC_ERR_ETYPE_NOTSUPP.");    
        }
        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the KDC behavior when interactive logon with DES as pre-authentication.")]
        // computer account ap01, 
        // Will failed with KRB_ERR_ETYPE_NOTSUPP in AS_REP: pre-authentication used DES
        public void ComputerAccount_DES_PreAuthentication_Fail()
        {
            base.Logging();

            //Create kerberos test client and connect for the computer account
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.FileServer[0].NetBiosName,
                this.testConfig.LocalRealm.FileServer[0].Password,
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

            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);
            
            KerberosKrbError krbError = null;

            //Recieve preauthentication required error
            METHOD_DATA methodData;
            krbError = client.ExpectPreauthRequiredError(out methodData);

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            EncryptionType clientEType = EncryptionType.DES_CBC_MD5;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp,
                0,
                clientEType,
                this.client.Context.CName.Password,
                this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });

            //Create and send AS request with DES as pre-authentication
            client.SendAsRequest(options, seqOfPaData);

            //Receive error from KDC
            krbError = client.ExpectKrbError();

            //Check KrbError error code against TD
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_ETYPE_NOTSUPP, krbError.ErrorCode, "Section 3.3.5.6 AS Exchange:" +
                "If DES is used for pre-authentication, the KDC MUST:<47>" +
                "If UseDESOnly is not set: the KDC MUST return KDC_ERR_ETYPE_NOTSUPP." +
                "Otherwise, if the account is:" +
                "krbtgt: the KDC MUST return KDC_ERR_ETYPE_NOTSUPP." +
                "The computer account of a KDC: the KDC MUST return KDC_ERR_ETYPE_NOTSUPP.");            

        }
        [TestMethod]
        [Priority(0)]        
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.BVT)]
        [Feature(Feature.Default)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the KDC behavior when interactive logon with DES as pre-authentication.")]
        // krbtgt 
        // Will failed with KRB_ERR_ETYPE_NOTSUPP in AS_REP: pre-authentication used DES        
        public void Krbtgt_DES_PreAuthentication_Fail()
        {
            base.Logging();

            //Create kerberos test client and connect
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[0].Username,
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

            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);

            //Receive the KDC_ERR_ETYP_NOTSUPP error from KDC
            KerberosKrbError krbError = null;


            //Recieve preauthentication required error
            METHOD_DATA methodData;
            krbError = client.ExpectPreauthRequiredError(out methodData);

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            EncryptionType clientEType = EncryptionType.DES_CBC_MD5;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp,
                0,
                clientEType,
                this.client.Context.CName.Password,
                this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });

            //Create and send AS request with DES as pre-authentication
            client.SendAsRequest(options, seqOfPaData);

            //Receive  error from KDC
            krbError = client.ExpectKrbError();

            //Check KrbError error code against TD
            BaseTestSite.Assert.AreEqual(KRB_ERROR_CODE.KDC_ERR_ETYPE_NOTSUPP, krbError.ErrorCode, "Section 3.3.5.6 AS Exchange:" +
               "If DES is used for pre-authentication, the KDC MUST:<47>" +
               "If UseDESOnly is not set: the KDC MUST return KDC_ERR_ETYPE_NOTSUPP." +
               "Otherwise, if the account is:" +
               "krbtgt: the KDC MUST return KDC_ERR_ETYPE_NOTSUPP." +
               "The computer account of a KDC: the KDC MUST return KDC_ERR_ETYPE_NOTSUPP.");    
        }
        #endregion
    }
}
