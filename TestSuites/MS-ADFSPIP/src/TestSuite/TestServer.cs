// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Principal;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Http;
using Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{

    public static class DebugData
    {
        public static byte[] CertRaw;
    }


    /// <summary>
    /// Please read the sample in Bind() to see how you can use this test client
    /// </summary>
    public class TestServer
    {
        /// <summary>
        /// PTF test site
        /// </summary>
        ITestSite testSite = null;

        HttpServer httpServer;

        public StsService sts;

        bool remoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            // always return true since we do not care client side certificate
            return true;
        }


        /// <summary>
        /// Initialize the test client
        /// </summary>
        /// <param name="site">TestSite from PTF</param>
        public void Initialize(ITestSite site)
        {
            testSite = site;
            httpServer = HttpServer.GetInstance();
            // sts = new StsService(Constraints.HTTPSServiceDefaultPort);
            sts = new StsService();

            X509Certificate2 cert = null;

            try
            {
                cert = new X509Certificate2(EnvironmentConfig.TLSServerCertificatePath, EnvironmentConfig.TLSServerCertificatePassword);
            }
            catch
            {
                site.Assert.Fail("Failed to load certificate as in PTFConfig: " + EnvironmentConfig.TLSServerCertificatePath + " with password:" + EnvironmentConfig.TLSServerCertificatePassword);
            }

            TcpPortInfo p = new TcpPortInfo();
            p.Port = Constraints.HTTPSServiceDefaultPort;
            p.IsSSL = true; p.ServerCertificate = cert;
            p.EncryptionPolicy = EncryptionPolicy.AllowNoEncryption;
            httpServer.Initialize(new TimeSpan(0, 0, EnvironmentConfig.SUTMaxDelayBetweenOperations));
            httpServer.AddPort(p);
            sts.Port = Constraints.HTTPSServiceDefaultPort;
            sts.IsSSL = true;
            sts.Initialize();
            try
            {
                // init pip server object
            }
            catch (Exception e)
            {
                site.Assert.Fail("Failed to init discovery or enrollment service endpoint" + (e.Message == null ? "" : (" due to reason: " + e.Message)));
            }
        }


        /// <summary>
        /// reset test client, dispose all resource and revert changes
        /// </summary>
        public void Dispose()
        {
            httpServer.Dispose();
        }

        /// <summary>
        /// wait some time and try to get ManagedEvent from underlying SDK, then parse, put into validation model
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        private int expectMessages(TimeSpan? duration, ref List<ManagedEvent> all)
        {
            TimeSpan maxSleep;
            if (duration == null)
                maxSleep = new TimeSpan(0, 0, EnvironmentConfig.SUTMaxDelayBetweenOperations);
            else
                maxSleep = duration.Value;

            List<ManagedEvent> tmp = new List<ManagedEvent>();

            httpServer.ExpectMessages(maxSleep, ref all);

            //all = HttpServerUtility.ReassembleRequests(tmp);
            return all.Count;
        }

        /// <summary>
        /// internally call expectMessages() and using filter: 1st message && from SUT 
        /// </summary>
        /// <returns></returns>
        private List<ManagedEvent> expectClientRequest(TimeSpan? timeout = null)
        {
            List<ManagedEvent> ret = new List<ManagedEvent>();
            expectMessages(timeout, ref ret);
            if (ret.Count == 0)
                throw new TimeoutException();
            foreach (ManagedEvent e in ret)
            {
                if (e.Sender.Address.ToString() != EnvironmentConfig.SUTIP)
                    testSite.Assert.Fail("received message from unknown machine");
            }
            return ret;
        }


        public void ResetFilter(List<string> urls)
        {
            filterUrls.Clear();
            foreach (string str in urls)
                filterUrls.Add(str);
        }

        List<string> filterUrls = new List<string>();

        /// <summary>
        /// normally we will filter message not under test
        /// </summary>
        /// <param name="events"></param>
        /// <param name="ret"></param>
        /// <returns></returns>
        private bool filterIgnored(List<ManagedEvent> events, out List<ManagedEvent> ret)
        {
            ret = new List<ManagedEvent>();
            foreach (ManagedEvent e in events)
            {
                if (e.ParsedHttpRequest != null)
                {
                    bool isFiltered = false;
                    foreach (string str in filterUrls)
                    {
                        if (e.ParsedHttpRequest.RequestUrl.Path.ToLower().Contains(str.ToLower()))
                        {
                            isFiltered = true;
                            break;
                        }
                    }
                    if (isFiltered)
                    {

                        if (e.ParsedHttpRequest.RequestUrl.Path.ToLower() == Constraints.RenewTrustUrl.ToLower())
                            continue;
                        else if (EnvironmentConfig.TestDeployment)
                        {
                        }
                        else
                        {
                            if (e.ParsedHttpRequest.RequestUrl.Path.ToLower() == Constraints.StoreUrl.ToLower() && e.ParsedHttpRequest.Method == HttpRequest.HttpMethod.GET)
                            {
                                HttpResponse res = ValidationModel.VerifyRequest(e.ParsedHttpRequest);
                                sts.SendMessage(e.Sender, Encoding.UTF8.GetBytes(res.ToString()));
                                continue;
                            }
                            else if (e.ParsedHttpRequest.RequestUrl.Path.ToLower() == (Constraints.StoreUrl.ToLower() + "/" + (EnvironmentConfig.IsWin2016 ? EnvironmentConfig.SUTConfigEntryKey_2016 : EnvironmentConfig.SUTConfigEntryKey_2012R2).ToLower()) && e.ParsedHttpRequest.Method == HttpRequest.HttpMethod.GET)
                            {
                                HttpResponse res = ValidationModel.VerifyRequest(e.ParsedHttpRequest);
                                sts.SendMessage(e.Sender, Encoding.UTF8.GetBytes(res.ToString()));
                                continue;
                            }
                            else if (e.ParsedHttpRequest.RequestUrl.Path.ToLower() == Constraints.GetSTSConfigurationUrl.ToLower() && e.ParsedHttpRequest.Method == HttpRequest.HttpMethod.GET)
                            {
                                HttpResponse res = ValidationModel.VerifyRequest(e.ParsedHttpRequest);
                                sts.SendMessage(e.Sender, Encoding.UTF8.GetBytes(res.ToString()));
                                continue;
                            }
                            else
                            {
                            }
                        }
                    }
                }
                ret.Add(e);
            }
            if (events.Count != ret.Count)
                return true;
            else
                return false;
        }

        /// <summary>
        /// internally call expectClientRequest and return if it's discovery request
        /// </summary>
        /// <returns></returns>
        public void EstablishTrust(TimeSpan? timeout = null)
        {
            List<ManagedEvent> results = null;
            // 4 times means 1st unauthorizaed request with saparated body and then request with token and saparated body
            int maxTryTimes = 8;
            ManagedEvent cache = null;
            ManagedEvent req = null;
            do
            {
                results = expectClientRequest(timeout);
                bool wantToProcess = true;
                foreach (ManagedEvent mge in results)
                {
                    ManagedEvent result = mge;
                    if (!wantToProcess)
                        break;
                    if (result.ParsedHttpRequest == null && result.EventType == EventType.IncomingRequest && cache != null)
                    {
                        if (cache.ParsedHttpRequest.Body == null)
                            cache.ParsedHttpRequest.Body = "";
                        cache.ParsedHttpRequest.Body += result.Text;
                        if (int.Parse(cache.ParsedHttpRequest.GetHeaderFieldValue(System.Net.HttpRequestHeader.ContentLength)) <= cache.ParsedHttpRequest.Body.Length)
                        {
                            result = cache;
                            cache = null;
                        }

                    }
                    if (result.ParsedHttpRequest != null && result.ParsedHttpRequest.RequestUrl.Path.ToLower() == Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP.Constraints.EstablishTrustUrl.ToLower())
                    {
                        if (null == result.ParsedHttpRequest.GetHeaderFieldValue(System.Net.HttpRequestHeader.Authorization))
                        {
                            HttpResponse tosend = new HttpResponse(System.Net.HttpStatusCode.Unauthorized);
                            // normally asks for NTLM authentication, may refactor later
                            tosend.SetHeaderField(System.Net.HttpResponseHeader.WwwAuthenticate, "Basic realm=\"\"");
                            sts.SendMessage(result.Sender, Encoding.UTF8.GetBytes(tosend.ToString()));
                            wantToProcess = false;
                        }
                        else
                        {
                            if (result.ParsedHttpRequest.Body == null)
                            {
                                string expect = result.ParsedHttpRequest.GetHeaderFieldValue(System.Net.HttpRequestHeader.Expect);

                                if (expect != null && expect.ToLower() == "100-continue")
                                {
                                    sts.SendMessage(result.Sender, Encoding.UTF8.GetBytes(new HttpResponse(System.Net.HttpStatusCode.Continue).ToString()));
                                    cache = result;

                                }

                            }
                            else
                            {
                                wantToProcess = false;
                                maxTryTimes = 0;
                                req = result;
                                break;
                            }
                        }
                    }
                    else if (result.EventType != EventType.IncomingRequest)
                    {
                        testSite.Assert.Fail("Failed to expect Establish Trust request");
                    }
                }
                maxTryTimes--;
            } while (maxTryTimes > 0);
            if (req == null)
                testSite.Assert.Fail("Failed to expect Establish Trust request");
            HttpResponse res = ValidationModel.VerifyRequest(req.ParsedHttpRequest);
            sts.SendMessage(req.Sender, Encoding.UTF8.GetBytes(res.ToString()));

        }



        public void GetSTSConfiguration(TimeSpan? timeout = null)
        {
            bool conti = false;
            bool hasFiltered = false;
            do
            {
                hasFiltered = false;
                List<ManagedEvent> tmp = expectClientRequest(timeout);
                List<ManagedEvent> results = null;
                hasFiltered = filterIgnored(tmp, out results);



                if (results.Count > 1)
                {
                    testSite.Assert.Fail("too many requests");
                }
                foreach (ManagedEvent e in results)
                {

                    if (e.ParsedHttpRequest != null)
                    {
                        if (e.ParsedHttpRequest.RequestUrl.Path.ToLower() == Constraints.GetSTSConfigurationUrl.ToLower() && e.ParsedHttpRequest.Method == HttpRequest.HttpMethod.GET)
                        {
                            HttpResponse res = ValidationModel.VerifyRequest(results[0].ParsedHttpRequest);
                            //HttpResponse res = new HttpResponse(System.Net.HttpStatusCode.OK);
                            //STSConfiguration conf = createValidStsConfigurationResponse();
                            //res.Body = Utility.Encode(conf.ToString());
                            //res.SetHeaderField(System.Net.HttpResponseHeader.ContentLength, res.Body.Length.ToString());
                            sts.SendMessage(results[0].Sender, Encoding.UTF8.GetBytes(res.ToString()));
                            return;
                        }
                        else
                            testSite.Assert.Fail("Failed to expect Get STS Configuration request");
                    }
                    else
                        conti = true;
                }
            } while (conti || hasFiltered);
        }

        // it's a test api to infinitely receive non-filtered messages
        public List<ManagedEvent> GetEcho(bool keep = false)
        {
            List<ManagedEvent> results = null;
            do
            {
                // hasFiltered = false;
                List<ManagedEvent> tmp = expectClientRequest(new TimeSpan(1, 0, 0));

                filterIgnored(tmp, out results);



                ////if (results.Count > 1)
                ////{
                ////    testSite.Assert.Fail("too many requests");
                ////}
                foreach (ManagedEvent e in results)
                {

                    if (e.ParsedHttpRequest != null)
                    {
                        //if (e.ParsedHttpRequest.RequestUrl.Path.ToLower() == Constraints.RelyingPartyTrustUrl.ToLower() && e.ParsedHttpRequest.Method == HttpRequest.HttpMethod.GET)
                        //{
                        //HttpResponse res = ValidationModel.VerifyRequest(results[0].ParsedHttpRequest);
                        ////if (EnvironmentConfig.TestDeployment)
                        ////{
                        ////        res = new HttpResponse(System.Net.HttpStatusCode.OK);
                        ////        ProxyTrust pt = new ProxyTrust();
                        ////        pt.Identifier = Constraints.DefaultProxyRelyingPartyTrustIdentifier;
                        ////        res.Body = pt.ToString();
                        ////        res.SetHeaderField(System.Net.HttpResponseHeader.ContentLength, res.Body.Length.ToString());
                        ////    }
                        ////}
                        //sts.SendMessage(results[0].Sender, Encoding.UTF8.GetBytes(res.ToString()));

                        //return;
                        //}
                        //HttpResponse res = ValidationModel.VerifyRequest(e.ParsedHttpRequest);
                        //sts.SendMessage(e.Sender, Encoding.UTF8.GetBytes(res.ToString()));
                    }
                    //else
                    //    conti = true;
                }
            } while (keep);

            return results;
        }

        public void GetStoreVersion(TimeSpan? timeout = null)
        {
            bool conti = false;
            bool hasFiltered = false;
            do
            {
                hasFiltered = false;
                List<ManagedEvent> tmp = expectClientRequest(timeout);
                List<ManagedEvent> results = null;
                hasFiltered = filterIgnored(tmp, out results);



                if (results.Count > 1)
                {
                    testSite.Assert.Fail("too many requests");
                }
                foreach (ManagedEvent e in results)
                {

                    if (e.ParsedHttpRequest != null)
                    {
                        if (e.ParsedHttpRequest.RequestUrl.Path.ToLower() == Constraints.StoreUrl.ToLower() && e.ParsedHttpRequest.Method == HttpRequest.HttpMethod.GET)
                        {
                            HttpResponse res = ValidationModel.VerifyRequest(results[0].ParsedHttpRequest);
                            //HttpResponse res = new HttpResponse(System.Net.HttpStatusCode.OK);
                            //if (EnvironmentConfig.TestDeployment)
                            //{
                            //    //// ecxpected a array
                            //    if (initialed)
                            //    {
                            //        res = new HttpResponse(System.Net.HttpStatusCode.OK);
                            //        StoreVersion ver = new StoreVersion();
                            //        ver.key = EnvironmentConfig.SUTConfigEntryKey;
                            //        ver.version = 0;
                            //        res.Body = "[" + ver.ToString() + "]";

                            //        res.SetHeaderField(System.Net.HttpResponseHeader.ContentLength, res.Body.Length.ToString());
                            //    }
                            //}
                            sts.SendMessage(results[0].Sender, Encoding.UTF8.GetBytes(res.ToString()));
                            return;
                        }
                        else
                            testSite.Assert.Fail("Failed to expect Get Data store version request");
                    }
                    else
                        conti = true;
                }
            } while (conti || hasFiltered);
        }

        XmlDocument createGlobalConfiguration()
        {
            /* Yuqing: I know there is a lot of hardcode, we will refactor later, for now only fix those value effect testing
             * with different AD FS name, domain name, certificates
             * 
             */
            XmlDocument doc = new XmlDocument();
            XmlNode configuration = doc.CreateNode(XmlNodeType.Element, "Configuration", null);
            XmlNode global = doc.CreateNode(XmlNodeType.Element, "GlobalConfig", null);
            XmlNode endpoint = doc.CreateNode(XmlNodeType.Element, "EndpointConfig", null);
            doc.AppendChild(configuration);
            configuration.AppendChild(global);
            configuration.AppendChild(endpoint);
            XmlAttribute attr = doc.CreateAttribute("ADFSWebApplicationProxyRelyingPartyUri");
            attr.Value = Constraints.DefaultProxyRelyingPartyTrustIdentifier;
            global.Attributes.Append(attr);
            attr = doc.CreateAttribute("AccessCookieEncryption");
            attr.Value = "true";
            global.Attributes.Append(attr);
            attr = doc.CreateAttribute("AccessCookieEncryptionKey");
            attr.Value = "03u7p4AjlGaItiKM+tH8Dw==";
            global.Attributes.Append(attr);
            attr = doc.CreateAttribute("AccessTokenApplicationUrlClaimName");
            attr.Value = "";
            global.Attributes.Append(attr);
            attr = doc.CreateAttribute("AccessTokenClientCertificateClaimName");
            attr.Value = "";
            global.Attributes.Append(attr);
            attr = doc.CreateAttribute("AccessTokenName");
            attr.Value = "";
            global.Attributes.Append(attr);
            attr = doc.CreateAttribute("AccessTokenUpnClaimName");
            attr.Value = "";
            global.Attributes.Append(attr);
            attr = doc.CreateAttribute("AppProxySPN");
            attr.Value = "";
            global.Attributes.Append(attr);
            attr = doc.CreateAttribute("AuthenticationPackageNameLSA");
            attr.Value = "";
            global.Attributes.Append(attr);
            attr = doc.CreateAttribute("AuthenticationPackageNameSSPI");
            attr.Value = "";
            global.Attributes.Append(attr);
            attr = doc.CreateAttribute("ConfigurationChangesPollingIntervalSec");
            attr.Value = "30";
            global.Attributes.Append(attr);
            attr = doc.CreateAttribute("ConnectedServersName");
            attr.Value = EnvironmentConfig.SUTDNS + ":";
            global.Attributes.Append(attr);
            attr = doc.CreateAttribute("OAuthAuthenticationURL");
            attr.Value = "https://" + EnvironmentConfig.ADFSFamrDNSName + AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.OAuth2Auth);
            global.Attributes.Append(attr);
            attr = doc.CreateAttribute("ServiceAccountNameForKCD");
            attr.Value = "";
            global.Attributes.Append(attr);
            attr = doc.CreateAttribute("ServiceAccountPasswordForKCD");
            attr.Value = "";
            global.Attributes.Append(attr);
            attr = doc.CreateAttribute("StsTokenSigningCertificatePublicKey");
            attr.Value = "MIIC3DCCAcSgAwIBAgIQQFj5UayT6otPjYDPbLshbjANBgkqhkiG9w0BAQsFADAqMSgwJgYDVQQDEx9BREZTIFNpZ25pbmcgLSBhZGZzLmNvbnRvc28uY29tMB4XDTEzMTEwMTA3NDg0OVoXDTE0MTEwMTA3NDg0OVowKjEoMCYGA1UEAxMfQURGUyBTaWduaW5nIC0gYWRmcy5jb250b3NvLmNvbTCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBALedGmaVh0sbS3jQZLQSoVedbP+kzA5cosw8MVuIrKJefxOom4uOGH99wKic88kbezEoeDAbYugZ9Iwgez0Lpp1YYvDfaNwmNX3wPRciPcZ7NQMTkLO2GcBBw+zhWtcN31f2MMePTJIQIPvRAzUUtGwerbH9MkmBdmKAkVmVXV50lqFmngZg4fUMV1MRDe3mdc3IkWn/JZB2ffCoyI4ojRfh6hYcOeKT4mtixjyt7w+/lzLB55LceMnwjvS8SAUeLRHUFLn9NgnKFWjUj2556oszVDTgNwcbyWp6WklvlzU3bMwRvoXTliZhPcktryv5jPnNXJ8T8B1ohEoeU8ti/UUCAwEAATANBgkqhkiG9w0BAQsFAAOCAQEALnDcFyHYQmPCoHaABsj0u58RluT56iXduTeIO+FoC9BGO5uxqbcFte9yWRXkSUUkdatxgprhG8V+BlcU3f74MzKlZkDDHSrtMvTkabK+zRE6HBllQ1k29ve9a2KFWWEU8LEgeTa+i5aY6oC34GiiaoNjewyiC/jahsGeeY0SPl4F2yArhLlQubGlFjdkWitkOUmTM9S3tykhWmOkGQrmWQRisLzwJZ2//Xb3vAexg+mHpNE69v0Syn9uxblmZ3pgF1voGbXswG81NBkiVYZBGZ5dqPRVkDju0N7zWxsgARg/o1ZhCPaQXWNqTpliLzppkM1grUVqs5XJMYzcY/A4Mw==";
            global.Attributes.Append(attr);
            attr = doc.CreateAttribute("StsUrl");
            attr.Value = "https://" + EnvironmentConfig.ADFSFamrDNSName + AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.Ls);
            global.Attributes.Append(attr);

            //attr = doc.CreateAttribute("ADFSRelyingPartyID");
            //attr.Value = "0c820403-cd42-e311-80b9-00155db08b14";
            //endpoint.Attributes.Append(attr);
            //attr = doc.CreateAttribute("ADFSRelyingPartyName");
            //attr.Value = "fed1";
            //endpoint.Attributes.Append(attr);
            //attr = doc.CreateAttribute("AppID");
            //attr.Value = "30D8A29C-A508-84F8-F5B3-726D1513B785";
            //endpoint.Attributes.Append(attr);

            //attr = doc.CreateAttribute("AppName");
            //attr.Value = EnvironmentConfig.App1Name;
            //endpoint.Attributes.Append(attr);
            //attr = doc.CreateAttribute("ApplicationType");
            //attr.Value = "PublishedWebApplication";
            //endpoint.Attributes.Append(attr);
            //attr = doc.CreateAttribute("BackendAuthNMode");
            //attr.Value = "None";
            //endpoint.Attributes.Append(attr);
            //attr = doc.CreateAttribute("BackendAuthNSPN");
            //attr.Value = "";
            //endpoint.Attributes.Append(attr);
            //attr = doc.CreateAttribute("BackendCertValidationMode");
            //attr.Value = "None";
            //endpoint.Attributes.Append(attr);
            //attr = doc.CreateAttribute("BackendUrl");
            //attr.Value = EnvironmentConfig.App1Url;
            //endpoint.Attributes.Append(attr);
            //attr = doc.CreateAttribute("ClientCertBindingMode");
            //attr.Value = "None";
            //endpoint.Attributes.Append(attr);
            //attr = doc.CreateAttribute("ClientCertificatePreauthenticationThumbprint");
            //attr.Value = "";
            //endpoint.Attributes.Append(attr);
            //attr = doc.CreateAttribute("ExternalCertificateThumbprint");
            //attr.Value = EnvironmentConfig.SUTCertificateThumbnailForPublishedApp;
            //endpoint.Attributes.Append(attr);
            //attr = doc.CreateAttribute("ExternalPreauthentication");
            //attr.Value = "ADFS";
            //endpoint.Attributes.Append(attr);
            //attr = doc.CreateAttribute("FrontendUrl");
            //attr.Value = EnvironmentConfig.App1Url;
            //endpoint.Attributes.Append(attr);
            //attr = doc.CreateAttribute("InactiveTransactionsTimeoutSec");
            //attr.Value = "300";
            //endpoint.Attributes.Append(attr);
            //attr = doc.CreateAttribute("TranslateUrlInRequestHeaders");
            //attr.Value = "true";
            //endpoint.Attributes.Append(attr);
            //attr = doc.CreateAttribute("TranslateUrlInResponseHeaders");
            //attr.Value = "true";
            //endpoint.Attributes.Append(attr);
            //attr = doc.CreateAttribute("UseOAuthAuthentication");
            //attr.Value = "false";
            //endpoint.Attributes.Append(attr);


            return doc;
        }

        public void GetProxyGlobalConfiguration(TimeSpan? timeout = null)
        {
            bool conti = false;
            bool hasFiltered = false;
            do
            {
                hasFiltered = false;
                List<ManagedEvent> tmp = expectClientRequest(timeout);
                List<ManagedEvent> results = null;
                hasFiltered = filterIgnored(tmp, out results);

                if (results.Count > 1)
                {
                    testSite.Assert.Fail("too many requests");
                }
                foreach (ManagedEvent e in results)
                {

                    if (e.ParsedHttpRequest != null)
                    {
                        if (e.ParsedHttpRequest.RequestUrl.Path.ToLower() == (Constraints.StoreUrl.ToLower() + "/" + (EnvironmentConfig.IsWin2016 ? EnvironmentConfig.SUTConfigEntryKey_2016 : EnvironmentConfig.SUTConfigEntryKey_2012R2).ToLower()))
                        {
                            HttpResponse res = ValidationModel.VerifyRequest(results[0].ParsedHttpRequest);

                            //// we support return a specific configuration to test sequence
                            //if (EnvironmentConfig.TestDeployment)
                            //{
                            //    if (initialed)
                            //    {
                            //        res = new HttpResponse(System.Net.HttpStatusCode.OK);
                            //        StoreEntry entry = new StoreEntry();
                            //        entry.key = EnvironmentConfig.SUTConfigEntryKey;
                            //        entry.version = 0;
                            //        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                            //        {
                            //            createGlobalConfiguration().Save(ms);
                            //            entry.value = Convert.ToBase64String(ms.ToArray());
                            //        }
                            //        res.Body = entry.ToString();
                            //        res.SetHeaderField(System.Net.HttpResponseHeader.ContentLength, res.Body.Length.ToString());
                            //    }
                            //    else
                            //    {
                            //        res = new HttpResponse(System.Net.HttpStatusCode.NotFound);
                            //        res.SetHeaderField(System.Net.HttpResponseHeader.ContentLength, "0");
                            //    }


                            //}
                            sts.SendMessage(results[0].Sender, Encoding.UTF8.GetBytes(res.ToString()));
                            return;
                        }
                        else
                            testSite.Assert.Fail("Failed to expect Get Data store entry request");
                    }
                    else
                        conti = true;
                }
            } while (conti || hasFiltered);
        }


        public void PostProxyGlobalConfiguration(TimeSpan? timeout = null)
        {
            bool conti = false;
            bool hasFiltered = false;
            do
            {
                hasFiltered = false;
                List<ManagedEvent> tmp = expectClientRequest(timeout);
                List<ManagedEvent> results = null;
                hasFiltered = filterIgnored(tmp, out results);

                if (results.Count > 1)
                {
                    testSite.Assert.Fail("too many requests");
                }
                foreach (ManagedEvent e in results)
                {

                    if (e.ParsedHttpRequest != null)
                    {
                        if (e.ParsedHttpRequest.RequestUrl.Path.ToLower() == (Constraints.StoreUrl.ToLower() + "/" + (EnvironmentConfig.IsWin2016 ? EnvironmentConfig.SUTConfigEntryKey_2016 : EnvironmentConfig.SUTConfigEntryKey_2012R2).ToLower()) && e.ParsedHttpRequest.Method == HttpRequest.HttpMethod.POST)
                        {
                            HttpResponse res = ValidationModel.VerifyRequest(results[0].ParsedHttpRequest);
                            sts.SendMessage(results[0].Sender, Encoding.UTF8.GetBytes(res.ToString()));
                            return;
                        }
                        else
                            testSite.Assert.Fail("Failed to expect Get Data store entry request");
                    }
                    else
                        conti = true;
                }
            } while (conti || hasFiltered);
        }
        public void GetProxyTrust(TimeSpan? timeout = null)
        {
            bool conti = false;
            bool hasFiltered = false;
            do
            {
                hasFiltered = false;
                List<ManagedEvent> tmp = expectClientRequest(timeout);
                List<ManagedEvent> results = null;
                hasFiltered = filterIgnored(tmp, out results);



                if (results.Count > 1)
                {
                    testSite.Assert.Fail("too many requests");
                }
                foreach (ManagedEvent e in results)
                {

                    if (e.ParsedHttpRequest != null)
                    {
                        if (e.ParsedHttpRequest.RequestUrl.Path.ToLower() == Constraints.ProxyTrustUrl.ToLower() && e.ParsedHttpRequest.Method == HttpRequest.HttpMethod.GET)
                        {
                            HttpResponse res = ValidationModel.VerifyRequest(results[0].ParsedHttpRequest);
                            //if (EnvironmentConfig.TestDeployment)
                            //{
                            //        res = new HttpResponse(System.Net.HttpStatusCode.OK);
                            //        ProxyTrust pt = new ProxyTrust();
                            //        pt.Identifier = Constraints.DefaultProxyRelyingPartyTrustIdentifier;
                            //        res.Body = pt.ToString();
                            //        res.SetHeaderField(System.Net.HttpResponseHeader.ContentLength, res.Body.Length.ToString());
                            //    }
                            //}
                            sts.SendMessage(results[0].Sender, Encoding.UTF8.GetBytes(res.ToString()));

                            return;
                        }
                        else
                            testSite.Assert.Fail("Failed to expect Get Proxy Trust identifer request");
                    }
                    else
                        conti = true;
                }
            } while (conti || hasFiltered);
        }

        public void GetRelyingPartyTrusts(TimeSpan? timeout = null)
        {
            bool conti = false;
            bool hasFiltered = false;
            do
            {
                hasFiltered = false;
                List<ManagedEvent> tmp = expectClientRequest(timeout);
                List<ManagedEvent> results = null;
                hasFiltered = filterIgnored(tmp, out results);



                if (results.Count > 1)
                {
                    testSite.Assert.Fail("too many requests");
                }
                foreach (ManagedEvent e in results)
                {

                    if (e.ParsedHttpRequest != null)
                    {
                        if (e.ParsedHttpRequest.RequestUrl.Path.ToLower() == Constraints.RelyingPartyTrustUrl.ToLower() && e.ParsedHttpRequest.Method == HttpRequest.HttpMethod.GET)
                        {
                            HttpResponse res = ValidationModel.VerifyRequest(results[0].ParsedHttpRequest);
                            //if (EnvironmentConfig.TestDeployment)
                            //{
                            //        res = new HttpResponse(System.Net.HttpStatusCode.OK);
                            //        ProxyTrust pt = new ProxyTrust();
                            //        pt.Identifier = Constraints.DefaultProxyRelyingPartyTrustIdentifier;
                            //        res.Body = pt.ToString();
                            //        res.SetHeaderField(System.Net.HttpResponseHeader.ContentLength, res.Body.Length.ToString());
                            //    }
                            //}
                            sts.SendMessage(results[0].Sender, Encoding.UTF8.GetBytes(res.ToString()));

                            return;
                        }
                        else
                            testSite.Assert.Fail("Failed to expect Get Proxy Trust identifer request");
                    }
                    else
                        conti = true;
                }
            } while (conti || hasFiltered);
        }

        public void PostPublishSettings(TimeSpan? timeout = null)
        {
            bool conti = false;
            bool hasFiltered = false;
            do
            {
                hasFiltered = false;
                List<ManagedEvent> tmp = expectClientRequest(timeout);
                List<ManagedEvent> results = null;
                hasFiltered = filterIgnored(tmp, out results);



                if (results.Count > 1)
                {
                    testSite.Assert.Fail("too many requests");
                }
                foreach (ManagedEvent e in results)
                {

                    if (e.ParsedHttpRequest != null)
                    {
                        //if (e.ParsedHttpRequest.RequestUrl.Path.ToLower() == Constraints.RelyingPartyTrustUrl.ToLower() && e.ParsedHttpRequest.Method == HttpRequest.HttpMethod.GET)
                        //{
                        // HttpResponse res = ValidationModel.VerifyRequest(results[0].ParsedHttpRequest);
                        //if (EnvironmentConfig.TestDeployment)
                        //{
                        //        res = new HttpResponse(System.Net.HttpStatusCode.OK);
                        //        ProxyTrust pt = new ProxyTrust();
                        //        pt.Identifier = Constraints.DefaultProxyRelyingPartyTrustIdentifier;
                        //        res.Body = pt.ToString();
                        //        res.SetHeaderField(System.Net.HttpResponseHeader.ContentLength, res.Body.Length.ToString());
                        //    }
                        //}
                        sts.SendMessage(results[0].Sender, Encoding.UTF8.GetBytes(new HttpResponse(System.Net.HttpStatusCode.OK).ToString()));

                        return;
                        //}
                        //else
                        //    testSite.Assert.Fail("Failed to expect Get Proxy Trust identifer request");
                    }
                    else
                        conti = true;
                }
            } while (conti || hasFiltered);
        }
        public void PostProxyTrust(TimeSpan? timeout = null)
        {
            List<ManagedEvent> results = null;
            // 4 times means 1st unauthorizaed request with saparated body and then request with token and saparated body
            int maxTryTimes = 4;
            ManagedEvent cache = null;
            ManagedEvent req = null;
            do
            {
                results = expectClientRequest(timeout);
                bool wantToProcess = true;
                foreach (ManagedEvent mge in results)
                {
                    ManagedEvent result = mge;
                    if (!wantToProcess)
                        break;
                    if (result.ParsedHttpRequest == null && result.EventType == EventType.IncomingRequest && cache != null)
                    {
                        if (cache.ParsedHttpRequest.Body == null)
                            cache.ParsedHttpRequest.Body = "";
                        cache.ParsedHttpRequest.Body += result.Text;
                        if (int.Parse(cache.ParsedHttpRequest.GetHeaderFieldValue(System.Net.HttpRequestHeader.ContentLength)) <= cache.ParsedHttpRequest.Body.Length)
                        {
                            result = cache;
                            cache = null;
                        }

                    }
                    if (result.ParsedHttpRequest != null && result.ParsedHttpRequest.RequestUrl.Path.ToLower() == Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP.Constraints.ProxyTrustUrl.ToLower() && result.ParsedHttpRequest.Method == HttpRequest.HttpMethod.POST)
                    {

                        if (result.ParsedHttpRequest.Body == null)
                        {
                            string expect = result.ParsedHttpRequest.GetHeaderFieldValue(System.Net.HttpRequestHeader.Expect);

                            if (expect != null && expect.ToLower() == "100-continue")
                            {
                                sts.SendMessage(result.Sender, Encoding.UTF8.GetBytes(new HttpResponse(System.Net.HttpStatusCode.Continue).ToString()));
                                cache = result;

                            }

                        }
                        else
                        {
                            wantToProcess = false;
                            maxTryTimes = 0;
                            req = result;
                            break;
                        }
                        //  }
                    }
                    else if (result.EventType != EventType.IncomingRequest)
                    {
                        testSite.Assert.Fail("Failed to expect Establish Trust request");
                    }
                }
                maxTryTimes--;
            } while (maxTryTimes > 0);
            HttpResponse res = ValidationModel.VerifyRequest(req.ParsedHttpRequest);
            sts.SendMessage(req.Sender, Encoding.UTF8.GetBytes(res.ToString()));

        }

        public void GetFederationMetadata(TimeSpan? timeout = null)
        {
            bool conti = false;
            bool hasFiltered = false;
            do
            {
                hasFiltered = false;
                List<ManagedEvent> tmp = expectClientRequest(timeout);
                List<ManagedEvent> results = null;
                hasFiltered = filterIgnored(tmp, out results);



                if (results.Count > 1)
                {
                    testSite.Assert.Fail("too many requests");
                }
                foreach (ManagedEvent e in results)
                {

                    if (e.ParsedHttpRequest != null)
                    {
                        if (e.ParsedHttpRequest.RequestUrl.Path.ToLower() == Constraints.FederationMetadataUrl.ToLower() && e.ParsedHttpRequest.Method == HttpRequest.HttpMethod.GET)
                        {
                            ValidationModel.VerifyRequest(results[0].ParsedHttpRequest);
                            string temp = null;
                            using (System.IO.StreamReader sr = new System.IO.StreamReader(@"c:\temp\metadataresponse.txt"))
                            {
                                temp = sr.ReadToEnd();
                            }
                            HttpResponse res = new HttpResponse();
                            res.Parse(temp);
                            string tosend = res.ToString();
                            string[] frags = res.Body.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                            //if (0 != string.Compare(tosend, temp))
                            //{
                            //}
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(frags[1]);
                            string cert = doc.ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;
                            X509Certificate2 cert2 = new X509Certificate2("c:\\sign.pfx", "123");
                            doc.ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText = Convert.ToBase64String(cert2.GetRawCertData());
                            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                            {
                                doc.Save(ms);
                                string newBody = frags[0] + "\r\n" + Encoding.UTF8.GetString(ms.ToArray()) + "\r\n" + frags[2] + "\r\n\r\n";
                                res.Body = newBody;
                            }
                            sts.SendMessage(results[0].Sender, Encoding.UTF8.GetBytes(tosend));
                            return;
                        }
                        else
                            testSite.Assert.Fail("Failed to expect Get Federation Metadata request");
                    }
                    else
                        conti = true;
                }
            } while (conti || hasFiltered);
        }

        public List<ManagedEvent> ExpectClientRequest()
        {
            return this.expectClientRequest(TimeSpan.FromSeconds(15));
        }

        public void SendResponse(System.Net.IPEndPoint ipEndPoint, HttpResponse response)
        {
            sts.SendMessage(ipEndPoint, Encoding.UTF8.GetBytes(response.ToString()));
        }

        public delegate HttpResponse HandleRequest(HttpRequest request);

        public void StartReceivingClientRequest(HandleRequest handleRequestCallback)
        {
            var continuedRequest = new Queue<HttpRequest>();

            while (true)
            {
                try
                {
                    var @event = ExpectClientRequest()[0];
                    var request = @event.ParsedHttpRequest;

                    if (request != null && request.GetHeaderFieldValue(HttpRequestHeader.Expect) != null)
                    {
                        if (request.GetHeaderFieldValue(HttpRequestHeader.Expect).Contains("100"))
                        {
                            continuedRequest.Enqueue(request);
                            SendResponse(@event.Sender, new HttpResponse(System.Net.HttpStatusCode.Continue));
                            continue;
                        }
                    }

                    if (request == null)
                    {
                        if (@event.EventType == EventType.IncomingRequest && continuedRequest.Count > 0)
                        {
                            request = continuedRequest.Dequeue();
                            request.Body = @event.Text;
                        }
                        else
                        {
                            throw new IOException("Non-HTTP request received.");
                        }
                    }

                    Debug.WriteLine("\n\rReceived request at " + DateTime.Now);
                    Debug.WriteLine(request.ToString().Trim());

                    var response = handleRequestCallback(request);
                    SendResponse(@event.Sender, response);
                    Debug.WriteLine("\n\rSent response at " + DateTime.Now);
                    Debug.WriteLine(response.ToString().Trim());
                }
                catch (TimeoutException)
                {
                    break;
                }
            }
        }
    }
}
