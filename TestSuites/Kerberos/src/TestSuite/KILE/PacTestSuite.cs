// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Linq;
using Microsoft.Protocol.TestSuites.Kerberos.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Security.Apds;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc;
using Microsoft.Protocols.TestTools.StackSdk.Security.Pac;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocol.TestSuites.Kerberos.TestSuite
{
    /// <summary>
    ///
    /// </summary>
    [TestClass]
    public class PacTestSuite : TraditionTestBase
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
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Kile | Feature.Pac)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify that KDC returns PAC in the service ticket.")]
        public void Request_PAC_TGT()
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
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with no PA data.");
            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, this.client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data, paPacOptions.Data});

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create a sequence of PA data.");
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with PA data.");
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
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[1].Username.ToLower(),
                KerberosUtility.PrincipalName2String(tgsResponse.TicketEncPart.cname).ToLower(),
                "The client principal name in ticket encrypted part should match expected.");
            //Verify PAC
            if (this.testConfig.IsKileImplemented)
            {
                BaseTestSite.Assert.IsNotNull(tgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(tgsResponse.TicketEncPart.authorization_data.Elements);
                BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2KPac.");
            }
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        [Feature(Feature.Kile | Feature.Pac)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify that KDC returns no PAC when pa-pac-request is false. " +
                     "This test case requires KDC's password, which will change when KDC restarts. " +
                     "The changed password will cause this case fail when decrypting TGS response.")]       
        public void Request_no_PAC_TGT()
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
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with no PA data.");
            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, this.client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(false);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with PA data.");
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create and send TGS request

            client.SendTgsRequest(this.testConfig.LocalRealm.LdapServer[0].LdapServiceName, options);
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();

            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.RealmName.ToLower(),
                tgsResponse.Response.ticket.realm.Value.ToLower(),
                "The realm in ticket should match expected.");
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.LdapServer[0].LdapServiceName,
               KerberosUtility.PrincipalName2String(tgsResponse.Response.ticket.sname),
               "The Service principal name in ticket should match expected.");

            EncryptionKey key = testConfig.QueryKey(this.testConfig.LocalRealm.LdapServer[0].LdapServiceName, this.testConfig.LocalRealm.RealmName, this.client.Context.SelectedEType);
            tgsResponse.DecryptTicket(key);
            
            //tgsResponse.DecryptTicket(this.testConfig.LocalRealm.ClientComputer.Password, this.testConfig.LocalRealm.ClientComputer.ServiceSalt);
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.RealmName.ToLower(),
               tgsResponse.TicketEncPart.crealm.Value.ToLower(),
               "The realm in ticket encrypted part should match expected.");
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[1].Username.ToLower(),
                KerberosUtility.PrincipalName2String(tgsResponse.TicketEncPart.cname).ToLower(),
                "The client principal name in ticket encrypted part should match expected.");
            //Verify PAC
            bool PacNotFound = false;
            if (tgsResponse.TicketEncPart.authorization_data == null)
            {
                PacNotFound = true;
            }
            else
            {
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(tgsResponse.TicketEncPart.authorization_data.Elements);
                PacNotFound = adWin2kPac == null;
            }
            BaseTestSite.Assert.IsTrue(PacNotFound, "AdWin2KPac is not generated.");
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.BVT)]
        [Feature(Feature.Kile | Feature.Pac)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the KERB_VALIDATION_INFO in PAC.")]
        public void KERB_VALIDATION_INFO()
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

            Adapter.PacHelper.commonUserFields commonUserFields = new Adapter.PacHelper.commonUserFields();
            if (this.testConfig.LocalRealm.KDC[0].IsWindows)
            {
                //Don't use the same user account for ldap querys, it will change the current user account attributes
                NetworkCredential cred = new NetworkCredential(this.testConfig.LocalRealm.User[2].Username, this.testConfig.LocalRealm.User[2].Password, this.testConfig.LocalRealm.RealmName);
                commonUserFields = Adapter.PacHelper.GetCommonUserFields(this.testConfig.LocalRealm.RealmName, this.testConfig.LocalRealm.User[1].Username, cred);
            }
                
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with no PA data.");
            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, this.client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data, paPacOptions.Data });
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
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[1].Username.ToLower(),
                KerberosUtility.PrincipalName2String(tgsResponse.TicketEncPart.cname).ToLower(),
                "The client principal name in ticket encrypted part should match expected.");
            //Verify PAC
            if (this.testConfig.IsKileImplemented)
            {
                BaseTestSite.Assert.IsNotNull(tgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(tgsResponse.TicketEncPart.authorization_data.Elements);
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

                if (this.testConfig.LocalRealm.KDC[0].IsWindows)
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
                    BaseTestSite.Assert.AreEqual(commonUserFields.PasswordLastSet,
                        fPasswordLastSet,
                        "PasswordLastSet in KERB_VALIDATION_INFO structure should be equal to that in AD.");

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

                    ushort logonCount = kerbValidationInfo.NativeKerbValidationInfo.LogonCount;
                    BaseTestSite.Assert.AreEqual(commonUserFields.LogonCount, logonCount, "The LogonCount field SHOULD be set to the Buffer.SAMPR_USER_ALL_INFORMATION.LogonCount field of the SamrQueryInformationUser2 response message.");

                    ushort badPasswordCount = kerbValidationInfo.NativeKerbValidationInfo.BadPasswordCount;
                    BaseTestSite.Assert.AreEqual(commonUserFields.BadPwdCount, badPasswordCount, "The BadPasswordCount field SHOULD be set to the Buffer.SAMPR_USER_ALL_INFORMATION.BadPasswordCount field of the SamrQueryInformationUser2 response message.");

                    uint userId = kerbValidationInfo.NativeKerbValidationInfo.UserId;
                    BaseTestSite.Assert.AreEqual(commonUserFields.userId, userId, "The UserID field SHOULD be set to the Buffer.SAMPR_USER_ALL_INFORMATION.UserId field of the SamrQueryInformationUser2 response message.");

                    uint primaryGroupId = kerbValidationInfo.NativeKerbValidationInfo.PrimaryGroupId;
                    BaseTestSite.Assert.AreEqual(commonUserFields.primaryGroupId, primaryGroupId, "The PrimaryGroupId field SHOULD be set to the Buffer.SAMPR_USER_ALL_INFORMATION.PrimaryGroupId field of the SamrQueryInformationUser2 response message.");

                    var userAccountControl = kerbValidationInfo.NativeKerbValidationInfo.UserAccountControl;
                    //not the same with what is in the Active Directory
                    //BaseTestSite.Assert.AreEqual(commonUserFields.userAccountControl, userAccountControl, "The BadPasswordCount field SHOULD be set to the Buffer.SAMPR_USER_ALL_INFORMATION.BadPasswordCount field of the SamrQueryInformationUser2 response message.");

                    //read only, need SUT adapter
                    uint groupCount = kerbValidationInfo.NativeKerbValidationInfo.GroupCount;
                    BaseTestSite.Assert.AreEqual(commonUserFields.groupCount, groupCount, "The GroupCount field SHOULD be set to the Groups.MembershipCount field of the SamrGetGroupsForUser response message.");

                    //read only, need SUT adapter
                    Microsoft.Protocols.TestTools.StackSdk.Security.Pac._GROUP_MEMBERSHIP[] groupIds = kerbValidationInfo.NativeKerbValidationInfo.GroupIds;
                    BaseTestSite.Assert.AreEqual(commonUserFields.groupIds.Length, groupIds.Length, "The GroupIds field SHOULD be set to the Groups.Group field of the SamrGetGroupsForUser response message.");
                    BaseTestSite.Assert.IsTrue(groupIds.Select(id => id.RelativeId).OrderBy(id => id).SequenceEqual(commonUserFields.groupIds.OrderBy(id => id)), "The GroupIds field SHOULD be set to the Groups.Group field of the SamrGetGroupsForUser response message.");

                    UserFlags_Values userFlags = kerbValidationInfo.NativeKerbValidationInfo.UserFlags;
                    BaseTestSite.Assert.AreEqual(UserFlags_Values.ExtraSids, UserFlags_Values.ExtraSids & userFlags, "The D bit SHOULD be set in the UserFlags field if the ExtraSids field is populated and contains additional SIDs and all other bits MUST be set to zero.");

                    byte[] userSessionKey = kerbValidationInfo.NativeKerbValidationInfo.UserSessionKey;
                    byte[] allZero = userSessionKey;
                    Array.Clear(allZero, 0, allZero.Length);
                    BaseTestSite.Assert.AreEqual(allZero, userSessionKey, "The UserSessionKey field MUST be set to zero.");

                    string logonServer = Adapter.PacHelper.ConvertUnicode2String(kerbValidationInfo.NativeKerbValidationInfo.LogonServer.Buffer);
                    BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.KDC[0].NetBiosName.Remove(this.testConfig.LocalRealm.KDC[0].NetBiosName.IndexOf("$")), logonServer, "The LogonServer SHOULD be set to NetbiosServerName.");

                    string logonDomainName = Adapter.PacHelper.ConvertUnicode2String(kerbValidationInfo.NativeKerbValidationInfo.LogonDomainName.Buffer);
                    BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.RealmName.Remove(this.testConfig.LocalRealm.RealmName.IndexOf(".")).ToLower(), logonDomainName.ToLower(), "The LogonDomainName SHOULD be set to NetbiosDomainName.");

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

                    //sidcount =2 because, one for AUTHENTICATION_AUTHORITY_ASSERTED_IDENTITY SID and one for CLAIMS_VALID SID
                    uint expectedSidCount = 2;
                    BaseTestSite.Assert.AreEqual(expectedSidCount, kerbValidationInfo.NativeKerbValidationInfo.SidCount, "The SidCount includes the number of one AUTHENTICATION_AUTHORITY_ASSERTED_IDENTITY, one CLAIMS_VALID.");

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

                    BaseTestSite.Assert.IsNull(kerbValidationInfo.NativeKerbValidationInfo.ResourceGroupDomainSid, "The ResourceGroupDomainSid field MUST be set to zero.");

                    BaseTestSite.Assert.AreEqual((uint)0, kerbValidationInfo.NativeKerbValidationInfo.ResourceGroupCount, "The ResourceGroupCount field MUST be set to zero.");

                    BaseTestSite.Assert.IsNull(kerbValidationInfo.NativeKerbValidationInfo.ResourceGroupIds, "The ResourceGroupIds field MUST be set to zero.");
                }
                else
                {   
                    string effectiveName = Adapter.PacHelper.ConvertUnicode2String(kerbValidationInfo.NativeKerbValidationInfo.EffectiveName.Buffer);
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
                                        
                    uint userId = kerbValidationInfo.NativeKerbValidationInfo.UserId;
                    BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[1].DomainAccountInfo.UserID, userId, "The UserID field SHOULD be set to the Buffer.SAMPR_USER_ALL_INFORMATION.UserId field of the SamrQueryInformationUser2 response message.");

                    uint primaryGroupId = kerbValidationInfo.NativeKerbValidationInfo.PrimaryGroupId;
                    BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[1].DomainAccountInfo.PrimaryGroupId, primaryGroupId, "The PrimaryGroupId field SHOULD be set to the Buffer.SAMPR_USER_ALL_INFORMATION.PrimaryGroupId field of the SamrQueryInformationUser2 response message.");
                    
                    uint userAccountControl = kerbValidationInfo.NativeKerbValidationInfo.UserAccountControl;
                    BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[1].DomainAccountInfo.UserAccountControl, userAccountControl, "The BadPasswordCount field SHOULD be set to the Buffer.SAMPR_USER_ALL_INFORMATION.BadPasswordCount field of the SamrQueryInformationUser2 response message.");

                    uint groupCount = kerbValidationInfo.NativeKerbValidationInfo.GroupCount;
                    BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[1].DomainAccountInfo.GroupCount, groupCount, "The GroupCount field SHOULD be set to the Groups.MembershipCount field of the SamrGetGroupsForUser response message.");

                    UserFlags_Values userFlags = kerbValidationInfo.NativeKerbValidationInfo.UserFlags;
                    BaseTestSite.Assert.AreEqual(UserFlags_Values.ExtraSids, UserFlags_Values.ExtraSids & userFlags, "The D bit SHOULD be set in the UserFlags field if the ExtraSids field is populated and contains additional SIDs and all other bits MUST be set to zero.");

                    byte[] userSessionKey = kerbValidationInfo.NativeKerbValidationInfo.UserSessionKey;
                    byte[] allZero = userSessionKey;
                    Array.Clear(allZero, 0, allZero.Length);
                    BaseTestSite.Assert.AreEqual(allZero, userSessionKey, "The UserSessionKey field MUST be set to zero.");

                    string logonServer = Adapter.PacHelper.ConvertUnicode2String(kerbValidationInfo.NativeKerbValidationInfo.LogonServer.Buffer);
                    BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.KDC[0].NetBiosName.Remove(this.testConfig.LocalRealm.KDC[0].NetBiosName.IndexOf("$")), logonServer, "The LogonServer SHOULD be set to NetbiosServerName.");

                    string logonDomainName = Adapter.PacHelper.ConvertUnicode2String(kerbValidationInfo.NativeKerbValidationInfo.LogonDomainName.Buffer);
                    BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.RealmName.Remove(this.testConfig.LocalRealm.RealmName.IndexOf(".")).ToLower(), logonDomainName.ToLower(), "The LogonDomainName SHOULD be set to NetbiosDomainName.");
                    
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

                    //sidcount =2 because, one for AUTHENTICATION_AUTHORITY_ASSERTED_IDENTITY SID and one for CLAIMS_VALID SID
                    uint expectedSidCount = 2;
                    BaseTestSite.Assert.AreEqual(expectedSidCount, kerbValidationInfo.NativeKerbValidationInfo.SidCount, "The SidCount includes the number of one AUTHENTICATION_AUTHORITY_ASSERTED_IDENTITY, one CLAIMS_VALID.");

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

                    BaseTestSite.Assert.IsNull(kerbValidationInfo.NativeKerbValidationInfo.ResourceGroupDomainSid, "The ResourceGroupDomainSid field MUST be set to zero.");

                    BaseTestSite.Assert.AreEqual((uint)kerbValidationInfo.NativeKerbValidationInfo.ResourceGroupIds.Length, kerbValidationInfo.NativeKerbValidationInfo.ResourceGroupCount, "The ResourceGroupCount field MUST be set to zero.");

                    BaseTestSite.Assert.IsNull(kerbValidationInfo.NativeKerbValidationInfo.ResourceGroupIds, "The ResourceGroupIds field MUST be set to zero.");
                }
            }
        }


        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.BVT)]
        [Feature(Feature.Kile | Feature.Pac)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the PAC_CLIENT_INFO in PAC.")]
        public void PAC_CLIENT_INFO()
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
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with no PA data.");
            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, this.client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data, paPacOptions.Data });
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
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[1].Username.ToLower(),
                KerberosUtility.PrincipalName2String(tgsResponse.TicketEncPart.cname).ToLower(),
                "The client principal name in ticket encrypted part should match expected.");
            //Verify PAC
            if (this.testConfig.IsKileImplemented)
            {
                BaseTestSite.Assert.IsNotNull(tgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");

                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(tgsResponse.TicketEncPart.authorization_data.Elements);
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
                    this.testConfig.LocalRealm.User[1].Username.ToLower(),
                    clientName.ToLower(),
                    "The Name field is an array of 16-bit Unicode characters in little-endian format that contains the client's account name.");
            }
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.BVT)]
        [Feature(Feature.Kile | Feature.Pac)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the Server Signature in PAC.")]
        public void ServerSignature()
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
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with no PA data.");
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
            //Verify PAC
            if (this.testConfig.IsKileImplemented)
            {
                BaseTestSite.Assert.IsNotNull(tgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(tgsResponse.TicketEncPart.authorization_data.Elements);
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

                var serverKey = KeyGenerator.MakeKey(
                    (EncryptionType)tgsResponse.Response.ticket.enc_part.etype.Value,
                    this.testConfig.LocalRealm.ClientComputer.Password,
                    this.testConfig.LocalRealm.ClientComputer.ServiceSalt);
                //cannot make a kdc key
                var kdcKey = KeyGenerator.MakeKey(
                    EncryptionType.AES256_CTS_HMAC_SHA1_96,
                    this.testConfig.LocalRealm.KDC[0].Password,
                    this.testConfig.LocalRealm.RealmName.ToUpper() + "krbtgt"
                    );
                bool isServerSignValidate = false;
                bool isKdcSignValidate = false;

                adWin2kPac.Pac.ValidateSign(serverKey, kdcKey, out isServerSignValidate, out isKdcSignValidate);
                BaseTestSite.Assert.IsTrue(isServerSignValidate, "Server signature should be valid.");

                if ((EncryptionType)tgsResponse.Response.ticket.enc_part.etype.Value == EncryptionType.RC4_HMAC
                    || (EncryptionType)tgsResponse.Response.ticket.enc_part.etype.Value == EncryptionType.DES_CBC_MD5)
                {
                    BaseTestSite.Assert.AreEqual(
                        PAC_SIGNATURE_DATA_SignatureType_Values.KERB_CHECKSUM_HMAC_MD5, 
                        serverSignature.NativePacSignatureData.SignatureType,
                        "The server signature type is KERB_CHECKSUM_HMAC_MD5.");
                }
                else if ((EncryptionType)tgsResponse.Response.ticket.enc_part.etype.Value == EncryptionType.AES256_CTS_HMAC_SHA1_96)
                {
                    BaseTestSite.Assert.AreEqual(
                        PAC_SIGNATURE_DATA_SignatureType_Values.HMAC_SHA1_96_AES256, 
                        serverSignature.NativePacSignatureData.SignatureType,
                        "The server signature type is HMAC_SHA1_96_AES256.");
                }
                else if ((EncryptionType)tgsResponse.Response.ticket.enc_part.etype.Value == EncryptionType.AES128_CTS_HMAC_SHA1_96)
                {
                    BaseTestSite.Assert.AreEqual(PAC_SIGNATURE_DATA_SignatureType_Values.HMAC_SHA1_96_AES128, 
                        serverSignature.NativePacSignatureData.SignatureType,
                        "The server signature type is HMAC_SHA1_96_AES128.");
                }
                else
                {
                    throw new Exception("The cryptographic system is none. The checksum operation will fail.");
                }
            }
        }


        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.BVT)]
        [Feature(Feature.Kile | Feature.Pac)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the KDC signature in PAC.")]
        public void KdcSignature()
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
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with no PA data.");
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
            //Verify PAC

            if (this.testConfig.IsKileImplemented)
            {
                BaseTestSite.Assert.IsNotNull(tgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(tgsResponse.TicketEncPart.authorization_data.Elements);
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

                //cannot make a kdc key
                //kdcSignature untestable
            }
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.BVT)]
        [Feature(Feature.Kile | Feature.Pac)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the UPN_DNS_INFO in PAC.")]
        public void UPN_DNS_INFO()
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
            //Verify PAC
            if (this.testConfig.IsKileImplemented)
            {
                BaseTestSite.Assert.IsNotNull(tgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(tgsResponse.TicketEncPart.authorization_data.Elements);
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
                BaseTestSite.Assert.AreEqual(
                    upnDnsInfo.Upn.Length * 2, 
                    upnDnsInfo.NativeUpnDnsInfo.UpnLength, 
                    "The UpnLength field SHOULD be the length of the UPN field, in bytes.");
                BaseTestSite.Assert.AreEqual(
                    upnDnsInfo.DnsDomain.Length * 2, 
                    upnDnsInfo.NativeUpnDnsInfo.DnsDomainNameLength, 
                    "The DnsDomainNameLength field SHOULD be the length of the DnsDomainName field, in bytes.");
                BaseTestSite.Assert.AreEqual(
                    UPN_DNS_INFO_Flags_Values.NoUpnAttribute, 
                    upnDnsInfo.NativeUpnDnsInfo.Flags & UPN_DNS_INFO_Flags_Values.NoUpnAttribute,
                    "The Flags field SHOULD set the U bit if the user account object does not have the userPrincipalName attribute ([MS-ADA3] (file://%5bMS-ADA3%5d.pdf) section 2.349) set.");
            }
        }


        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.Smb2Ap)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.FAST)]
        [Feature(Feature.Kile | Feature.Pac | Feature.Claim | Feature.FAST)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the PAC_DEVICE_INFO in PAC.")]
        public void PAC_DEVICE_INFO()
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
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data });
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();

            // Switch to user principal
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName, this.testConfig.LocalRealm.User[2].Username, this.testConfig.LocalRealm.User[2].Password, KerberosAccountType.User, client.Context.Ticket, client.Context.SessionKey,
                testConfig.LocalRealm.KDC[0].IPAddress, testConfig.LocalRealm.KDC[0].Port, testConfig.TransportType,
                testConfig.SupportedOid);

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
            KerberosTgsResponse userKrbTgsRep = client.
                ExpectTgsResponse(KeyUsageNumber.TGS_REP_encrypted_part_subkey);
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
        [Feature(Feature.Kile | Feature.Pac | Feature.Claim | Feature.FAST)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the PAC_DEVICE_CLAIMS_INFO in PAC.")]
        public void PAC_DEVICE_CLAIMS_INFO()
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

            // AS_REQ and KRB-ERROR using device principal
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);
            METHOD_DATA methodData;
            KerberosKrbError krbError1 = client.ExpectPreauthRequiredError(out methodData);

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
            KerberosTgsResponse userKrbTgsRep = client.
                ExpectTgsResponse(KeyUsageNumber.TGS_REP_encrypted_part_subkey);
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
        [Feature(Feature.Kile | Feature.Pac | Feature.Claim)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the PAC_CLIENT_CLAIMS_INFO in PAC.")]
        public void PAC_CLIENT_CLAIMS_INFO()
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

            // AS_REQ and KRB-ERROR using device principal
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);
            METHOD_DATA methodData;
            KerberosKrbError krbError1 = client.ExpectPreauthRequiredError(out methodData);

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
            KerberosTgsResponse userKrbTgsRep = client.
                ExpectTgsResponse(KeyUsageNumber.TGS_REP_encrypted_part_subkey);
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

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.BVT)]
        [Feature(Feature.Kile | Feature.Pac)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify that PAC is marked as Ad-if-relevant.")]
        public void PacMarkedAdIfRelevant()
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
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create and send TGS request
            client.SendTgsRequest(this.testConfig.LocalRealm.ClientComputer.DefaultServiceName, options);
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();

            //Verify PAC
            if (this.testConfig.IsKileImplemented)
            {
                EncryptionKey key = testConfig.QueryKey(this.testConfig.LocalRealm.ClientComputer.DefaultServiceName, this.testConfig.LocalRealm.RealmName, this.client.Context.SelectedEType);
                tgsResponse.DecryptTicket(key);
            
                //tgsResponse.DecryptTicket(this.testConfig.LocalRealm.ClientComputer.Password, this.testConfig.LocalRealm.ClientComputer.ServiceSalt);
                BaseTestSite.Assert.IsNotNull(tgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");

                AdWin2KPac adWin2kPac = null;
                foreach (var element in tgsResponse.TicketEncPart.authorization_data.Elements)
                {
                    var authData = AuthDataElementParser.ParseAuthDataElement(element);
                    if (authData is AdIfRelevent)
                    {
                        AdIfRelevent adIfRelevent = authData as AdIfRelevent;
                        foreach (var subElement in adIfRelevent.Elements)
                        {
                            if (subElement is AdWin2KPac)
                            {
                                adWin2kPac = subElement as AdWin2KPac;
                                goto pacFound;
                            }
                        }
                    }
                }
            pacFound:
                BaseTestSite.Assert.IsNotNull(adWin2kPac, "AdWin2KPAC is marked as Ad-If-Relevent.");
            }
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.BVT)]
        [Feature(Feature.Kile | Feature.Pac)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the Domain Local Group Membership populated in service ticket's PAC when Disable Resource SID compression is not set.")]
        public void DomainLocalGroupMembershipWithDisableResourceSIDCompressionUnset()
        {
            base.Logging();

            //Create kerberos test client and connect
            client = new KerberosTestClient(
                this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[13].Username,
                this.testConfig.LocalRealm.User[13].Password,
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

            Adapter.PacHelper.commonUserFields commonUserFields = new Adapter.PacHelper.commonUserFields();
            if (this.testConfig.LocalRealm.KDC[0].IsWindows)
            {
                //Don't use the same user account for ldap querys, it will change the current user account attributes
                NetworkCredential cred = new NetworkCredential(this.testConfig.LocalRealm.Admin.Username, this.testConfig.LocalRealm.Admin.Password, this.testConfig.LocalRealm.RealmName);
                commonUserFields = Adapter.PacHelper.GetCommonUserFields(this.testConfig.LocalRealm.RealmName, this.testConfig.LocalRealm.User[13].Username, cred);
            }

            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with no PA data.");
            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, this.client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data, paPacOptions.Data });
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create and send TGS request
            client.SendTgsRequest(this.testConfig.LocalRealm.LocalResources[0].DefaultServiceName, options, seqOfPaData);
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();

            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.RealmName.ToLower(),
                tgsResponse.Response.ticket.realm.Value.ToLower(),
                "The realm in ticket should match expected.");
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.LocalResources[0].DefaultServiceName,
               KerberosUtility.PrincipalName2String(tgsResponse.Response.ticket.sname),
               "The Service principal name in ticket should match expected.");

            //EncryptionKey key = testConfig.QueryKey(this.testConfig.LocalRealm.LocalResources[0].DefaultServiceName, this.testConfig.LocalRealm.RealmName, this.client.Context.SelectedEType);
            //tgsResponse.DecryptTicket(key);
            
            tgsResponse.DecryptTicket(this.testConfig.LocalRealm.LocalResources[0].Password, this.testConfig.LocalRealm.LocalResources[0].ServiceSalt);
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.RealmName.ToLower(),
               tgsResponse.TicketEncPart.crealm.Value.ToLower(),
               "The realm in ticket encrypted part should match expected.");
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[13].Username,
                KerberosUtility.PrincipalName2String(tgsResponse.TicketEncPart.cname),
                "The client principal name in ticket encrypted part should match expected.");
            //Verify PAC
            if (this.testConfig.IsKileImplemented && this.testConfig.LocalRealm.KDC[0].IsWindows)
            {
                BaseTestSite.Assert.IsNotNull(tgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(tgsResponse.TicketEncPart.authorization_data.Elements);
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
                                
                BaseTestSite.Assert.AreEqual(1, kerbValidationInfo.NativeKerbValidationInfo.ResourceGroupDomainSid.Length, "The number of ResourceGroupDomainSid should be 1 as is configured.");
                foreach (_RPC_SID element in kerbValidationInfo.NativeKerbValidationInfo.ResourceGroupDomainSid)
                {
                    byte[] expectedIdentifierAuthority = new byte[6] { 0, 0, 0, 0, 0, 5 };
                    BaseTestSite.Assert.AreEqual(expectedIdentifierAuthority.Length, element.IdentifierAuthority.Value.Length, "IdentifierAuthority 000005 stands for S-1-5");

                    BaseTestSite.Assert.IsTrue(element.IdentifierAuthority.Value.SequenceEqual(expectedIdentifierAuthority), "IdentifierAuthority elements should match expected.");
                    uint[] expectedSubAuthority = commonUserFields.domainSid;
                    BaseTestSite.Assert.AreEqual(expectedSubAuthority.Length, element.SubAuthorityCount, "SubAuthorityCount should match expected.");
                    BaseTestSite.Assert.IsTrue(element.SubAuthority.SequenceEqual(expectedSubAuthority), "SubAuthorityCount elements should match expected.");
                }

                uint resourceGroupCount = this.testConfig.LocalRealm.ResourceGroups.GroupCount;
                BaseTestSite.Assert.AreEqual((uint)resourceGroupCount, kerbValidationInfo.NativeKerbValidationInfo.ResourceGroupCount, "The ResourceGroupCount field contains the number of groups in the ResourceGroupIds field.");

                Adapter.Group[] resourceGroups = this.testConfig.LocalRealm.ResourceGroups.Groups;
                NetworkCredential cred = new NetworkCredential(this.testConfig.LocalRealm.Admin.Username, this.testConfig.LocalRealm.Admin.Password, this.testConfig.LocalRealm.RealmName);
                uint[] expectedResourceGroupIds = Adapter.PacHelper.GetResourceGroupIds(this.testConfig.LocalRealm.RealmName, cred, resourceGroupCount, resourceGroups);
                for (int i = 0; i < resourceGroupCount; i++)
                {
                    BaseTestSite.Assert.AreEqual(Attributes_Values.Mandatory | Attributes_Values.EnabledByDefault 
                        | Attributes_Values.Enabled | Attributes_Values.Resource, 
                        kerbValidationInfo.NativeKerbValidationInfo.ResourceGroupIds[i].Attributes, 
                        "The Attributes have the A, B, C and E bits set to 1, and all other bits set to zero.");
                        
                }
                BaseTestSite.Assert.IsTrue(kerbValidationInfo.NativeKerbValidationInfo.ResourceGroupIds.Select(id => id.RelativeId).OrderBy(id => id).SequenceEqual(expectedResourceGroupIds.OrderBy(id => id)), "RelativeId contains the RID of the value pmsgOut.ppDsNames.Sid.");
            }
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.BVT)]
        [Feature(Feature.Kile | Feature.Pac)]
        [ApplicationServer(ApplicationServer.All)]
        [Description("This test case is designed to verify the Domain Local Group Membership populated in service ticket's PAC when Disable Resource SID compression is set.")]
        public void DomainLocalGroupMembershipWithDisableResourceSIDCompressionSet()
        {
            base.Logging();

            //Create kerberos test client and connect
            client = new KerberosTestClient(
                this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[13].Username,
                this.testConfig.LocalRealm.User[13].Password,
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

            Adapter.PacHelper.commonUserFields commonUserFields = new Adapter.PacHelper.commonUserFields();
            if (this.testConfig.LocalRealm.KDC[0].IsWindows)
            {
                //Don't use the same user account for ldap querys, it will change the current user account attributes
                NetworkCredential cred = new NetworkCredential(this.testConfig.LocalRealm.Admin.Username, this.testConfig.LocalRealm.Admin.Password, this.testConfig.LocalRealm.RealmName);
                commonUserFields = Adapter.PacHelper.GetCommonUserFields(this.testConfig.LocalRealm.RealmName, this.testConfig.LocalRealm.User[13].Username, cred);
            }

            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with no PA data.");
            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);

            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, this.client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims | PacOptions.ForwardToFullDc);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data, paPacOptions.Data });
            //Create and send AS request
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");

            //Create and send TGS request
            client.SendTgsRequest(this.testConfig.LocalRealm.LocalResources[1].DefaultServiceName, options, seqOfPaData);
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();

            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.RealmName.ToLower(),
                tgsResponse.Response.ticket.realm.Value.ToLower(),
                "The realm in ticket should match expected.");
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.LocalResources[1].DefaultServiceName,
               KerberosUtility.PrincipalName2String(tgsResponse.Response.ticket.sname),
               "The Service principal name in ticket should match expected.");
            
            tgsResponse.DecryptTicket(this.testConfig.LocalRealm.LocalResources[1].Password, this.testConfig.LocalRealm.LocalResources[1].ServiceSalt);
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.RealmName.ToLower(),
               tgsResponse.TicketEncPart.crealm.Value.ToLower(),
               "The realm in ticket encrypted part should match expected.");
            BaseTestSite.Assert.AreEqual(this.testConfig.LocalRealm.User[13].Username.ToLower(),
                KerberosUtility.PrincipalName2String(tgsResponse.TicketEncPart.cname).ToLower(),
                "The client principal name in ticket encrypted part should match expected.");
            //Verify PAC
            if (this.testConfig.IsKileImplemented && this.testConfig.LocalRealm.KDC[0].IsWindows)
            {
                BaseTestSite.Assert.IsNotNull(tgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(tgsResponse.TicketEncPart.authorization_data.Elements);
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

                //sidcount =2 because, one for AUTHENTICATION_AUTHORITY_ASSERTED_IDENTITY SID and one for CLAIMS_VALID SID
                uint resourceGroupCount = this.testConfig.LocalRealm.ResourceGroups.GroupCount;
                uint expectedSidCount = 2 + resourceGroupCount;
                BaseTestSite.Assert.AreEqual(expectedSidCount, kerbValidationInfo.NativeKerbValidationInfo.SidCount, "The SidCount includes the number of one AUTHENTICATION_AUTHORITY_ASSERTED_IDENTITY, one CLAIMS_VALID and the number of localResourceGroup SIDs.");

                
                //BaseTestSite.Assert.IsNull(kerbValidationInfo.NativeKerbValidationInfo.ExtraSids, ".");

                KERB_SID_AND_ATTRIBUTES[] expectedDefaultExtraSids = new KERB_SID_AND_ATTRIBUTES[2];
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
                
                Adapter.Group[] resourceGroups = this.testConfig.LocalRealm.ResourceGroups.Groups;
                NetworkCredential cred = new NetworkCredential(this.testConfig.LocalRealm.Admin.Username, this.testConfig.LocalRealm.Admin.Password, this.testConfig.LocalRealm.RealmName);
                KERB_SID_AND_ATTRIBUTES[] expectedResourceGroupExtraSids = Adapter.PacHelper.GetResourceGroupExtraSids(this.testConfig.LocalRealm.RealmName, cred, resourceGroupCount, resourceGroups);
                var expectedExtraSids = expectedDefaultExtraSids.Concat(expectedResourceGroupExtraSids).ToArray();

                var res = expectedSidCount;

                for (int i = 0; i < expectedSidCount; i++)
                {
                                       
                    for (int j = 0; j < expectedSidCount; j++)
                    {
                        if(Adapter.PacHelper.ExtraSidsAreEqual(expectedExtraSids[i], kerbValidationInfo.NativeKerbValidationInfo.ExtraSids[j]))
                        {
                            res--;
                        } 
                    }
                }
                BaseTestSite.Assert.AreEqual((uint)0, res, "The ExtraSids field contains the pointer to a list which is the list copied from the PAC in the TGT plus a list constructed from the domain local groups...");
            }
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KDC)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K8R2)]
        [TestCategory(TestCategories.KilePac)]
        public void APDS_KERBEROS_PAC_VALIDATION()
        {
            base.Logging();

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Construct Kerberos client for testing."); 
            client = new KerberosTestClient(
                this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.Admin.Username,
                this.testConfig.LocalRealm.Admin.Password,
                KerberosAccountType.User,
                testConfig.LocalRealm.KDC[0].IPAddress,
                testConfig.LocalRealm.KDC[0].Port,
                testConfig.TransportType,
                testConfig.SupportedOid);            
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with no PA data.");
            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            client.SendAsRequest(options, null);
            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);
            //Create sequence of PA data
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp, 0, this.client.Context.SelectedEType, this.client.Context.CName.Password, this.client.Context.CName.Salt);
            PaPacRequest paPacRequest = new PaPacRequest(true);
            Asn1SequenceOf<PA_DATA> seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data});
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send AS request with PA data.");
            client.SendAsRequest(options, seqOfPaData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();
            BaseTestSite.Assert.IsNotNull(asResponse.Response.ticket, "AS response should contain a TGT.");
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send TGS request.");
            client.SendTgsRequest(this.testConfig.LocalRealm.ClientComputer.DefaultServiceName, options);
            KerberosTgsResponse tgsResponse = client.ExpectTgsResponse();
            EncryptionKey key = testConfig.QueryKey(this.testConfig.LocalRealm.ClientComputer.DefaultServiceName, this.testConfig.LocalRealm.RealmName, this.client.Context.SelectedEType);
            tgsResponse.DecryptTicket(key);

            if (this.testConfig.IsKileImplemented)
            {
                //Get Server and KDC Signatures
                PacServerSignature pacServerSignature = null;
                PacKdcSignature pacKdcSignature = null;
                BaseTestSite.Assert.IsNotNull(tgsResponse.TicketEncPart.authorization_data, "The ticket contains Authorization data.");
                AdWin2KPac adWin2kPac = FindOneInAuthData<AdWin2KPac>(tgsResponse.TicketEncPart.authorization_data.Elements);
                BaseTestSite.Assert.IsNotNull(adWin2kPac, "The Authorization data contains AdWin2KPac.");
                foreach (var buf in adWin2kPac.Pac.PacInfoBuffers)
                {
                    if (buf is PacServerSignature)
                    {
                        pacServerSignature = buf as PacServerSignature;
                    }
                    if (buf is PacKdcSignature)
                    {
                        pacKdcSignature = buf as PacKdcSignature;
                    }
                }

                BaseTestSite.Log.Add(LogEntryKind.Comment, "Establish Secure Channel.");
                NrpcClient nrpcClient = NrpcClient.CreateNrpcClient(this.testConfig.LocalRealm.RealmName);
                ushort[] endPointList = NrpcUtility.QueryNrpcTcpEndpoint(testConfig.LocalRealm.KDC[0].FQDN);
                ushort endPoint = endPointList[0];
                MachineAccountCredential machineCredential = new MachineAccountCredential(
                    this.testConfig.LocalRealm.RealmName,
                    testConfig.LocalRealm.ClientComputer.FQDN.Split('.')[0],
                    testConfig.LocalRealm.ClientComputer.Password);

                nrpcClient.Context.NegotiateFlags = NrpcNegotiateFlags.SupportsAESAndSHA2
                    | NrpcNegotiateFlags.SupportsConcurrentRpcCalls
                    | NrpcNegotiateFlags.SupportsCrossForestTrusts
                    | NrpcNegotiateFlags.SupportsGenericPassThroughAuthentication
                    | NrpcNegotiateFlags.SupportsNetrLogonGetDomainInfo
                    | NrpcNegotiateFlags.SupportsNetrLogonSendToSam
                    | NrpcNegotiateFlags.SupportsNetrServerPasswordSet2
                    | NrpcNegotiateFlags.SupportsRC4
                    | NrpcNegotiateFlags.SupportsRefusePasswordChange
                    | NrpcNegotiateFlags.SupportsRodcPassThroughToDifferentDomains
                    | NrpcNegotiateFlags.SupportsSecureRpc
                    | NrpcNegotiateFlags.SupportsStrongKeys
                    | NrpcNegotiateFlags.SupportsTransitiveTrusts;

                NrpcClientSecurityContext securityContext = new NrpcClientSecurityContext(
                    this.testConfig.LocalRealm.RealmName,
                    testConfig.LocalRealm.KDC[0].FQDN.Split('.')[0],
                    machineCredential,
                    true,
                    nrpcClient.Context.NegotiateFlags);

                nrpcClient.BindOverTcp(testConfig.LocalRealm.KDC[0].FQDN, endPoint, securityContext, TimeSpan.FromMilliseconds(600000));

                _NETLOGON_LOGON_INFO_CLASS logonLevel = _NETLOGON_LOGON_INFO_CLASS.NetlogonGenericInformation;
                _NETLOGON_VALIDATION_INFO_CLASS validationLevel = _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationGenericInfo2;
                _NETLOGON_VALIDATION? validationInfomation;
                byte? authoritative;
                NrpcNetrLogonSamLogonExtraFlags? extraFlags = NrpcNetrLogonSamLogonExtraFlags.None;

                BaseTestSite.Log.Add(LogEntryKind.Comment, "Create valid KERB_VERIFY_PAC_REQUEST.");
                KERB_VERIFY_PAC_REQUEST kerberosReq = ApdsUtility.CreateKerbVerifyPacRequest(pacServerSignature.NativePacSignatureData, pacKdcSignature.NativePacSignatureData);
                //Create Kerberos Validation Logon Info
                _NETLOGON_LEVEL netlogonLevel = ApdsUtility.CreatePacLogonInfo(
                    NrpcParameterControlFlags.AllowLogonWithComputerAccount,
                    this.testConfig.LocalRealm.RealmName,
                    this.testConfig.LocalRealm.Admin.Username,
                    testConfig.LocalRealm.KDC[0].FQDN.Split('.')[0],
                    kerberosReq);               
                _NETLOGON_LEVEL encryptedLogonLevel = nrpcClient.EncryptNetlogonLevel(
                    (_NETLOGON_LOGON_INFO_CLASS)logonLevel,
                    netlogonLevel);
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Call NetrLogonSamLogonEx for Kerberos pac validation.");
                NtStatus result = nrpcClient.NetrLogonSamLogonEx(
                    nrpcClient.Handle,
                    testConfig.LocalRealm.KDC[0].FQDN.Split('.')[0],
                    testConfig.LocalRealm.ClientComputer.FQDN.Split('.')[0],
                    logonLevel,
                    encryptedLogonLevel,
                    validationLevel,
                    out validationInfomation,
                    out authoritative,
                    ref extraFlags);
                BaseTestSite.Assert.AreEqual(NtStatus.STATUS_SUCCESS, result, "[MS-APDS]3.2.5.2 If the checksum is verified, the DC MUST return STATUS_SUCCESS.");
                BaseTestSite.Assert.IsNull(validationInfomation.Value.ValidationGeneric2[0].ValidationData, "[MS-APDS]3.2.5.2 There is no return message.");

                BaseTestSite.Log.Add(LogEntryKind.Comment, "Create invalid KERB_VERIFY_PAC_REQUEST.");
                kerberosReq = ApdsUtility.CreateKerbVerifyPacRequest(pacKdcSignature.NativePacSignatureData, pacKdcSignature.NativePacSignatureData);
                //Create Kerberos Validation Logon Info
                netlogonLevel = ApdsUtility.CreatePacLogonInfo(
                    NrpcParameterControlFlags.AllowLogonWithComputerAccount,
                    this.testConfig.LocalRealm.RealmName,
                    this.testConfig.LocalRealm.Admin.Username,
                    testConfig.LocalRealm.KDC[0].FQDN.Split('.')[0],
                    kerberosReq);
                encryptedLogonLevel = nrpcClient.EncryptNetlogonLevel(
                    (_NETLOGON_LOGON_INFO_CLASS)logonLevel,
                    netlogonLevel);
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Call NetrLogonSamLogonEx for Kerberos pac validation.");
                result = nrpcClient.NetrLogonSamLogonEx(
                    nrpcClient.Handle,
                    testConfig.LocalRealm.KDC[0].FQDN.Split('.')[0],
                    testConfig.LocalRealm.ClientComputer.FQDN.Split('.')[0],
                    logonLevel,
                    encryptedLogonLevel,
                    validationLevel,
                    out validationInfomation,
                    out authoritative,
                    ref extraFlags);
                BaseTestSite.Assert.AreEqual(NtStatus.STATUS_LOGON_FAILURE, result, "[MS-APDS]3.2.5.2 If the checksum verification fails, the DC MUST return an error code, STATUS_LOGON_FAILURE (section 2.2) as the return value to the Netlogon Generic Pass-through method.");
            }

        }
    }
}
