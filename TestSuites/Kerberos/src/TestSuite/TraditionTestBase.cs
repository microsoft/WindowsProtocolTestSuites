// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Protocol.TestSuites.Kerberos.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.Transport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocol.TestSuites.Kerberos.TestSuite
{
    public abstract class TraditionTestBase : TestClassBase
    {
        protected KKDCPClientConfig proxyClientConfig;
        protected KerberosTestClient client;
        protected TestConfig testConfig;
        protected ISutControlAdapter sutController;

        protected override void TestInitialize()
        {
            base.TestInitialize();
            this.testConfig = new TestConfig();
            this.sutController = BaseTestSite.GetAdapter<ISutControlAdapter>();

            if (this.testConfig.LocalRealm.KDC[0].IsWindows && this.testConfig.TrustType != TrustType.NoTrust)
            {
                  //set forest trust authentication as forest-wide authentication
                this.sutController.setSelectiveAuth(this.testConfig.LocalRealm.RealmName, this.testConfig.LocalRealm.Admin.Username, this.testConfig.LocalRealm.Admin.Password, this.testConfig.TrustedRealm.RealmName, false);
            }
            if (this.testConfig.TrustedRealm.KDC[0].IsWindows && this.testConfig.TrustType != TrustType.NoTrust)
            {
                //set forest trust authentication as forest-wide authentication
                this.sutController.setSelectiveAuth(this.testConfig.TrustedRealm.RealmName, this.testConfig.TrustedRealm.Admin.Username, this.testConfig.TrustedRealm.Admin.Password, this.testConfig.LocalRealm.RealmName, false);
            }

            // create KKDCP Client Config according to config file
            if (this.testConfig.UseProxy && proxyClientConfig == null)
            {
                X509Certificate2 clientCert = null;
                if (!string.IsNullOrEmpty(this.testConfig.KKDCPClientCertPath))
                {
                    Assert.IsFalse(!string.IsNullOrEmpty(this.testConfig.KKDCPClientCertPassword), "The protected password should be set.");
                    clientCert = new X509Certificate2();
                    try
                    {
                        clientCert.Import(this.testConfig.KKDCPClientCertPath, this.testConfig.KKDCPClientCertPassword, X509KeyStorageFlags.DefaultKeySet);
                    }
                    catch
                    {
                        BaseTestSite.Log.Add(LogEntryKind.TestError, "Error in importing client certificate.");
                        throw;
                    }
                }
                proxyClientConfig = new KKDCPClientConfig()
                {
                    KKDCPServerURL = this.testConfig.KKDCPServerUrl,
                    TlsClientCertificate = clientCert
                };
            }
        }

        protected override void TestCleanup()
        {
            //reset forest trust authentication as forest-wide authentication
            if (this.testConfig.LocalRealm.KDC[0].IsWindows && this.testConfig.TrustType != TrustType.NoTrust)
            {
                this.sutController.setSelectiveAuth(this.testConfig.TrustedRealm.RealmName, this.testConfig.TrustedRealm.Admin.Username, this.testConfig.TrustedRealm.Admin.Password, this.testConfig.LocalRealm.RealmName, false);
            }
            if (this.testConfig.TrustedRealm.KDC[0].IsWindows && this.testConfig.TrustType != TrustType.NoTrust)
            {
                this.sutController.setSelectiveAuth(this.testConfig.LocalRealm.RealmName, this.testConfig.LocalRealm.Admin.Username, this.testConfig.LocalRealm.Admin.Password, this.testConfig.TrustedRealm.RealmName, false);
            }

            base.TestCleanup();
        }

        protected void Logging()
        {
            StackTrace trace = new System.Diagnostics.StackTrace();
            StackFrame[] frames = trace.GetFrames();

            //find who is calling me
            System.Reflection.MethodBase method = frames[1].GetMethod();
            KerberosTestAttribute.Site = this.Site;
            object[] attrs = method.GetCustomAttributes(false);
            if (attrs == null)
                return;

            foreach (object o in attrs)
            {
                //for out test attribute, invoke "Logging" method, for MSTEST attributes, do accordingly
                Type thisType = o.GetType();
                switch (thisType.Name)
                {
                    case "DescriptionAttribute":
                        {
                            Site.Log.Add(LogEntryKind.Comment, "Test description: " + typeof(DescriptionAttribute).GetProperty("Description").GetValue(o, null).ToString());
                        } break;
                    case "PriorityAttribute":
                        {
                            Site.Log.Add(LogEntryKind.Comment, "Implementation priority of this test case is: " + typeof(PriorityAttribute).GetProperty("Priority").GetValue(o, null).ToString());
                        } break;
                    case "TestCategoryAttribute":
                        {
                            IList<string> categories = (IList<string>)typeof(TestCategoryAttribute).GetProperty("TestCategories").GetValue(o, null);
                            if (categories.Count > 0)
                                Site.Log.Add(LogEntryKind.Comment, "This test case is belong to: " + categories[0] + "Test Category");
                        } break;
                    default:
                        if (thisType.BaseType == typeof(KerberosTestAttribute))
                        {
                            try
                            {
                                thisType.GetMethod("Logging").Invoke(o, null);
                            }
                            catch (Exception e)
                            {
                                KerberosTestAttribute.Site.Assert.Fail(e.InnerException.Message);
                            }
                        }
                        break;
                }

            }
        }

        protected void TypicalASExchange(KerberosTestClient client, KdcOptions options)
        {
            //Create and send AS request
            PaPacRequest paPacRequest = new PaPacRequest(true);
            PaPacOptions paPacOptions = new PaPacOptions(PacOptions.Claims);
            Asn1SequenceOf<PA_DATA> paData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paPacRequest.Data, paPacOptions.Data });
            client.SendAsRequest(options, paData);

            //Recieve preauthentication required error
            METHOD_DATA methodData;
            KerberosKrbError krbError = client.ExpectPreauthRequiredError(out methodData);

            //Create pa-enc-timestamp
            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(timeStamp,
                0,
                this.client.Context.SelectedEType,
                this.client.Context.CName.Password,
                this.client.Context.CName.Salt);

            paData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data, paPacOptions.Data });
            //Create and send AS request
            client.SendAsRequest(options, paData);
            KerberosAsResponse asResponse = client.ExpectAsResponse();
        }

        protected byte[] SendAndRecieveHttpAp(WebServer webServer, byte[] gssApiToken)
        {
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Http AP Exchange.");
            //Negotiate authentication methods
            HttpFunctionalTestClient httpclient = new HttpFunctionalTestClient(webServer.HttpUri);
            HttpStatusCode status = httpclient.GetHttpResponse();
            BaseTestSite.Assert.AreEqual(HttpStatusCode.Unauthorized, status, "Http server requires authorization data.");
            string[] authMethods = null;
            authMethods = httpclient.GetAuthMethods();
            BaseTestSite.Assert.IsNotNull(authMethods, "Negotiate authentication method is inside the authentication header.");

            //Sent AP request with security token
            httpclient = new HttpFunctionalTestClient(webServer.HttpUri, gssApiToken);
            httpclient.SetNegoAuthHeader(gssApiToken);
            byte[] repToken = null;
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Create and send Http request with authorization data.");
            status = httpclient.GetHttpResponse();
            if (HttpStatusCode.OK == status)
            {
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Receive Http response, authentication passed.");
            }
            else
            {
                BaseTestSite.Log.Add(LogEntryKind.Comment, "Receive Http response failed. Failure reason: {0}", status);
            }
            //get authentication header from Http response
            repToken = httpclient.GetNegoAuthHeader();
            BaseTestSite.Assert.IsNotNull(repToken, "AP_REP is inside the authentication header.");

            httpclient.Dispose();

            return repToken;
        }

        protected byte[] SendAndRecieveSmb2Ap(FileServer fileServer, byte[] gssApiToken)
        {
            BaseTestSite.Log.Add(LogEntryKind.Comment, "SMB2 AP Exchange.");
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
            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "Negotiate failed with error.");

            byte[] repToken;
            status = smb2Client.SessionSetup(
                SESSION_SETUP_Request_SecurityMode_Values.NONE,
                SESSION_SETUP_Request_Capabilities_Values.GLOBAL_CAP_DFS,
                SecurityPackageType.Kerberos,
                fileServer.FQDN,
                gssApiToken,
                out repToken);

            if (status == Smb2Status.STATUS_SUCCESS)
            {
                status = smb2Client.LogOff();
                BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "Logoff failed with error.");
            }
            smb2Client.Disconnect();

            return repToken;
        }

        protected byte[] SendAndRecieveLdapAp(LdapServer ldapServer, byte[] gssApiToken, KerberosConstValue.GSSToken gssToken)
        {
            SocketTransportConfig transportConfig = new SocketTransportConfig();
            transportConfig.RemoteIpAddress = System.Net.IPAddress.Parse(ldapServer.IPAddress);
            transportConfig.RemoteIpPort = ldapServer.LdapPort;
            transportConfig.BufferSize = 8192;
            transportConfig.Type = StackTransportType.Tcp;
            transportConfig.Role = Role.Client;
            AdtsLdapClient ldapClient = new AdtsLdapClient(AdtsLdapVersion.V3, transportConfig);

            ldapClient.Connect();

            string gss = (gssToken == KerberosConstValue.GSSToken.GSSSPNG) ? "GSS-SPNEGO" : "GSSAPI";
            AdtsBindRequestPacket bindRequest = ldapClient.CreateSaslBindRequest(gss, gssApiToken);
            ldapClient.SendPacket(bindRequest);
            AdtsLdapPacket response = ldapClient.ExpectPacket(KerberosConstValue.TIMEOUT_DEFAULT);

            BaseTestSite.Assert.IsNotNull(response, "Ldap response should not be null");
            BaseTestSite.Assert.IsInstanceOfType(response, typeof(AdtsBindResponsePacket), "Ldap response should be a bind response.");

            AdtsBindResponsePacket bindResponse = (AdtsBindResponsePacket)response;
            //Response code is 14, Sasl Bind In Progress, need future investigate
            byte[] repToken = ((BindResponse)bindResponse.GetInnerRequestOrResponse()).serverSaslCreds.ByteArrayValue;
            return repToken;
        }

        protected static T FindOneInAuthData<T>(AuthorizationDataElement[] elements) where T : class,IAuthDataElement
        {
            T element = null;
            if (typeof(T) == typeof(AdIfRelevent)) throw new NotSupportedException("Search for AdIfRelevent is not supported.");
            LinkedList<IAuthDataElement> authDataList = new LinkedList<IAuthDataElement>();
            AdIfRelevent rootNode = new AdIfRelevent(new AD_IF_RELEVANT(elements));
            authDataList.AddLast(rootNode);
            while (element == null && authDataList.Count > 0)
            {
                var node = authDataList.Last.Value;
                authDataList.RemoveLast();
                if (node is AdIfRelevent)
                {
                    var adIfRelevent = node as AdIfRelevent;
                    foreach (var authData in adIfRelevent.Elements)
                    {
                        authDataList.AddLast(authData);
                    }
                }
                else if (node is T)
                {
                    element = node as T;
                }
            }
            return element;
        }
    }
}
