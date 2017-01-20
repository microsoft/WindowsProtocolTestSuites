// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Http;
using Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP;
using Microsoft.Protocols.TestTools;
using System.Xml;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{

    public class ValidationModel
    {
        static private STSConfiguration createValidStsConfigurationResponse()
        {

            EndpointConfiguration ec = new EndpointConfiguration();
            List<Endpoint> eps = new List<Endpoint>();

            Endpoint fed = new Endpoint();
            fed.AuthenticationSchemes = AuthType.Anonymous;
            fed.CertificateValidation = CertificateValidation.None;
            fed.ClientCertificateQueryMode = ClientCertificateQueryMode.None;
            fed.Path = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.FederationMetadata);
            fed.ServicePath = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.FederationMetadata);
            fed.ServicePortType = PortType.HttpsPort;
            fed.PortType = PortType.HttpsPort;
            fed.SupportsNtlm = false;
            eps.Add(fed);

            Endpoint ls = new Endpoint();
            ls.AuthenticationSchemes = AuthType.Anonymous;
            ls.CertificateValidation = CertificateValidation.Device;
            ls.ClientCertificateQueryMode = ClientCertificateQueryMode.QueryAndRequire;
            ls.Path = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.Ls);
            ls.ServicePath = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.Ls);
            ls.ServicePortType = PortType.HttpsPort;
            ls.PortType = PortType.HttpsPort;
            ls.SupportsNtlm = false;
            eps.Add(ls);

            Endpoint ls2 = new Endpoint();
            ls2.AuthenticationSchemes = AuthType.Anonymous;
            ls2.CertificateValidation = CertificateValidation.User;
            ls2.ClientCertificateQueryMode = ClientCertificateQueryMode.QueryAndRequire;
            ls2.Path = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.Ls);
            ls2.ServicePath = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.Ls);
            ls2.ServicePortType = PortType.HttpsPortForUserTlsAuth;
            ls2.PortType = PortType.HttpsPortForUserTlsAuth;
            ls2.SupportsNtlm = false;
            eps.Add(ls2);

            Endpoint portal1 = new Endpoint();
            portal1.AuthenticationSchemes = AuthType.Anonymous;
            portal1.CertificateValidation = CertificateValidation.None;
            portal1.ClientCertificateQueryMode = ClientCertificateQueryMode.None;
            portal1.Path = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.Portal);
            portal1.ServicePath = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.Portal);
            portal1.ServicePortType = PortType.HttpsPortForUserTlsAuth;
            portal1.PortType = PortType.HttpsPortForUserTlsAuth;
            portal1.SupportsNtlm = false;
            eps.Add(portal1);

            Endpoint oauth2token = new Endpoint();
            oauth2token.AuthenticationSchemes = AuthType.Anonymous;
            oauth2token.CertificateValidation = CertificateValidation.None;
            oauth2token.ClientCertificateQueryMode = ClientCertificateQueryMode.None;
            oauth2token.Path = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.OAuth2Token);
            oauth2token.ServicePath = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.OAuth2Token);
            oauth2token.ServicePortType = PortType.HttpsPort;
            oauth2token.PortType = PortType.HttpsPort;
            oauth2token.SupportsNtlm = false;
            eps.Add(oauth2token);

            Endpoint oauth2auth = new Endpoint();
            oauth2auth.AuthenticationSchemes = AuthType.Anonymous;
            oauth2auth.CertificateValidation = CertificateValidation.Device;
            oauth2auth.ClientCertificateQueryMode = ClientCertificateQueryMode.None;
            oauth2auth.Path = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.OAuth2Auth);
            oauth2auth.ServicePath = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.OAuth2Auth);
            oauth2auth.ServicePortType = PortType.HttpsPort;
            oauth2auth.PortType = PortType.HttpsPort;
            oauth2auth.SupportsNtlm = false;
            eps.Add(oauth2auth);


            Endpoint oauth2auth2 = new Endpoint();
            oauth2auth2.AuthenticationSchemes = AuthType.Anonymous;
            oauth2auth2.CertificateValidation = CertificateValidation.User;
            oauth2auth2.ClientCertificateQueryMode = ClientCertificateQueryMode.QueryAndRequire;
            oauth2auth2.Path = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.OAuth2Auth);
            oauth2auth2.ServicePath = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.OAuth2Auth);
            oauth2auth2.ServicePortType = PortType.HttpsPortForUserTlsAuth;
            oauth2auth2.PortType = PortType.HttpsPortForUserTlsAuth;
            oauth2auth2.SupportsNtlm = false;
            eps.Add(oauth2auth2);

            Endpoint enroll = new Endpoint();
            enroll.AuthenticationSchemes = AuthType.Anonymous;
            enroll.CertificateValidation = CertificateValidation.None;
            enroll.ClientCertificateQueryMode = ClientCertificateQueryMode.None;
            enroll.Path = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.EnrollmentServer);
            enroll.ServicePath = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.EnrollmentServer);
            enroll.ServicePortType = PortType.HttpsPort;
            enroll.PortType = PortType.HttpsPort;
            enroll.SupportsNtlm = false;
            eps.Add(enroll);

            Endpoint portal2 = new Endpoint();
            portal2.AuthenticationSchemes = AuthType.Anonymous;
            portal2.CertificateValidation = CertificateValidation.None;
            portal2.ClientCertificateQueryMode = ClientCertificateQueryMode.None;
            portal2.Path = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.Portal);
            portal2.ServicePath = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.Portal);
            portal2.ServicePortType = PortType.HttpsPort;
            portal2.PortType = PortType.HttpsPort;
            portal2.SupportsNtlm = false;
            eps.Add(portal2);

            Endpoint winTrans = new Endpoint();
            winTrans.AuthenticationSchemes = AuthType.Anonymous;
            winTrans.CertificateValidation = CertificateValidation.None;
            winTrans.ClientCertificateQueryMode = ClientCertificateQueryMode.None;
            winTrans.Path = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.WindowsTransportTrust2005);
            winTrans.ServicePath = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.WindowsTransportTrust2005);
            winTrans.ServicePortType = PortType.HttpsPort;
            winTrans.PortType = PortType.HttpsPort;
            winTrans.SupportsNtlm = false;
            eps.Add(winTrans);


            Endpoint cert2005 = new Endpoint();
            cert2005.AuthenticationSchemes = AuthType.Anonymous;
            cert2005.CertificateValidation = CertificateValidation.None;
            cert2005.ClientCertificateQueryMode = ClientCertificateQueryMode.None;
            cert2005.Path = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.CertificateMixedTrust2005);
            cert2005.ServicePath = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.CertificateMixedTrust2005);
            cert2005.ServicePortType = PortType.HttpsPort;
            cert2005.PortType = PortType.HttpsPort;
            cert2005.SupportsNtlm = false;
            eps.Add(cert2005);


            Endpoint certTrans = new Endpoint();
            certTrans.AuthenticationSchemes = AuthType.Anonymous;
            certTrans.CertificateValidation = CertificateValidation.None;
            certTrans.ClientCertificateQueryMode = ClientCertificateQueryMode.QueryAndRequire;
            certTrans.Path = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.CertificateTransportTrust2005);
            certTrans.ServicePath = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.CertificateTransportTrust2005);
            certTrans.ServicePortType = PortType.HttpsPortForUserTlsAuth;
            certTrans.PortType = PortType.HttpsPortForUserTlsAuth;
            certTrans.SupportsNtlm = false;
            eps.Add(certTrans);

            Endpoint user2005 = new Endpoint();
            user2005.AuthenticationSchemes = AuthType.Anonymous;
            user2005.CertificateValidation = CertificateValidation.None;
            user2005.ClientCertificateQueryMode = ClientCertificateQueryMode.None;
            user2005.Path = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.UsernameMixedTrust2005);
            user2005.ServicePath = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.UsernameMixedTrust2005);
            user2005.ServicePortType = PortType.HttpsPort;
            user2005.PortType = PortType.HttpsPort;
            user2005.SupportsNtlm = false;
            eps.Add(user2005);

            Endpoint asym2005 = new Endpoint();
            asym2005.AuthenticationSchemes = AuthType.Anonymous;
            asym2005.CertificateValidation = CertificateValidation.None;
            asym2005.ClientCertificateQueryMode = ClientCertificateQueryMode.None;
            asym2005.Path = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.IssuedTokenMixedAsymmetricBasic256Trust2005);
            asym2005.ServicePath = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.IssuedTokenMixedAsymmetricBasic256Trust2005);
            asym2005.ServicePortType = PortType.HttpsPort;
            asym2005.PortType = PortType.HttpsPort;
            asym2005.SupportsNtlm = false;
            eps.Add(asym2005);

            Endpoint sym2005 = new Endpoint();
            sym2005.AuthenticationSchemes = AuthType.Anonymous;
            sym2005.CertificateValidation = CertificateValidation.None;
            sym2005.ClientCertificateQueryMode = ClientCertificateQueryMode.None;
            sym2005.Path = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.IssuedTokenMixedSymmetricBasic256Trust2005);
            sym2005.ServicePath = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.IssuedTokenMixedSymmetricBasic256Trust2005);
            sym2005.ServicePortType = PortType.HttpsPort;
            sym2005.PortType = PortType.HttpsPort;
            sym2005.SupportsNtlm = false;
            eps.Add(sym2005);

            Endpoint cert13 = new Endpoint();
            cert13.AuthenticationSchemes = AuthType.Anonymous;
            cert13.CertificateValidation = CertificateValidation.None;
            cert13.ClientCertificateQueryMode = ClientCertificateQueryMode.None;
            cert13.Path = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.CertificateMixedTrust13);
            cert13.ServicePath = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.CertificateMixedTrust13);
            cert13.ServicePortType = PortType.HttpsPort;
            cert13.PortType = PortType.HttpsPort;
            cert13.SupportsNtlm = false;
            eps.Add(cert13);

            Endpoint user13 = new Endpoint();
            user13.AuthenticationSchemes = AuthType.Anonymous;
            user13.CertificateValidation = CertificateValidation.None;
            user13.ClientCertificateQueryMode = ClientCertificateQueryMode.None;
            user13.Path = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.UsernameMixedTrust13);
            user13.ServicePath = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.UsernameMixedTrust13);
            user13.ServicePortType = PortType.HttpsPort;
            user13.PortType = PortType.HttpsPort;
            user13.SupportsNtlm = false;
            eps.Add(user13);

            Endpoint asym13 = new Endpoint();
            asym13.AuthenticationSchemes = AuthType.Anonymous;
            asym13.CertificateValidation = CertificateValidation.None;
            asym13.ClientCertificateQueryMode = ClientCertificateQueryMode.None;
            asym13.Path = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.IssuedTokenMixedAsymmetricBasic256Trust13);
            asym13.ServicePath = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.IssuedTokenMixedAsymmetricBasic256Trust13);
            asym13.ServicePortType = PortType.HttpsPort;
            asym13.PortType = PortType.HttpsPort;
            asym13.SupportsNtlm = false;
            eps.Add(asym13);

            Endpoint sym13 = new Endpoint();
            sym13.AuthenticationSchemes = AuthType.Anonymous;
            sym13.CertificateValidation = CertificateValidation.None;
            sym13.ClientCertificateQueryMode = ClientCertificateQueryMode.None;
            sym13.Path = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.IssuedTokenMixedSymmetricBasic256Trust13);
            sym13.ServicePath = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.IssuedTokenMixedSymmetricBasic256Trust13);
            sym13.ServicePortType = PortType.HttpsPort;
            sym13.PortType = PortType.HttpsPort;
            sym13.SupportsNtlm = false;
            eps.Add(sym13);

            Endpoint proxyMex = new Endpoint();
            proxyMex.AuthenticationSchemes = AuthType.Anonymous;
            proxyMex.CertificateValidation = CertificateValidation.None;
            proxyMex.ClientCertificateQueryMode = ClientCertificateQueryMode.None;
            proxyMex.Path = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.Mex);
            proxyMex.ServicePath = AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.ProxyMex);
            proxyMex.ServicePortType = PortType.HttpsPort;
            proxyMex.PortType = PortType.HttpsPort;
            proxyMex.SupportsNtlm = false;
            eps.Add(proxyMex);

            ec.Endpoints = eps.ToArray();
            ServiceConfiguration sc = new ServiceConfiguration();
            sc.HttpPort = 80;
            sc.HttpsPort = 443;
            sc.HttpsPortForUserTlsAuth = 10000;
            sc.ProxyTrustCertificateLifetime = 21600;
            sc.ServiceHostName = EnvironmentConfig.ADFSFamrDNSName;
            sc.CustomUpnSuffixes = new string[0];
            sc.DeviceCertificateIssuers = new string[0];
            sc.DiscoveredUpnSuffixes = new string[0];
            STSConfiguration sts = new STSConfiguration();
            sts.EndpointConfiguration = ec;
            sts.ServiceConfiguration = sc;
            return sts;
        }

        public static System.Security.Cryptography.X509Certificates.X509Certificate2 TrustCertificate { get; set; }

        public static System.Security.Cryptography.X509Certificates.X509Certificate2 SigningCertificate { get; set; }

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
            attr.Value = "Proxy.contoso.com:";
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
            attr.Value = Convert.ToBase64String(SigningCertificate.GetPublicKey()); //"MIIC3DCCAcSgAwIBAgIQQFj5UayT6otPjYDPbLshbjANBgkqhkiG9w0BAQsFADAqMSgwJgYDVQQDEx9BREZTIFNpZ25pbmcgLSBhZGZzLmNvbnRvc28uY29tMB4XDTEzMTEwMTA3NDg0OVoXDTE0MTEwMTA3NDg0OVowKjEoMCYGA1UEAxMfQURGUyBTaWduaW5nIC0gYWRmcy5jb250b3NvLmNvbTCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBALedGmaVh0sbS3jQZLQSoVedbP+kzA5cosw8MVuIrKJefxOom4uOGH99wKic88kbezEoeDAbYugZ9Iwgez0Lpp1YYvDfaNwmNX3wPRciPcZ7NQMTkLO2GcBBw+zhWtcN31f2MMePTJIQIPvRAzUUtGwerbH9MkmBdmKAkVmVXV50lqFmngZg4fUMV1MRDe3mdc3IkWn/JZB2ffCoyI4ojRfh6hYcOeKT4mtixjyt7w+/lzLB55LceMnwjvS8SAUeLRHUFLn9NgnKFWjUj2556oszVDTgNwcbyWp6WklvlzU3bMwRvoXTliZhPcktryv5jPnNXJ8T8B1ohEoeU8ti/UUCAwEAATANBgkqhkiG9w0BAQsFAAOCAQEALnDcFyHYQmPCoHaABsj0u58RluT56iXduTeIO+FoC9BGO5uxqbcFte9yWRXkSUUkdatxgprhG8V+BlcU3f74MzKlZkDDHSrtMvTkabK+zRE6HBllQ1k29ve9a2KFWWEU8LEgeTa+i5aY6oC34GiiaoNjewyiC/jahsGeeY0SPl4F2yArhLlQubGlFjdkWitkOUmTM9S3tykhWmOkGQrmWQRisLzwJZ2//Xb3vAexg+mHpNE69v0Syn9uxblmZ3pgF1voGbXswG81NBkiVYZBGZ5dqPRVkDju0N7zWxsgARg/o1ZhCPaQXWNqTpliLzppkM1grUVqs5XJMYzcY/A4Mw==";
            global.Attributes.Append(attr);
            attr = doc.CreateAttribute("StsUrl");
            attr.Value = "https://" + EnvironmentConfig.ADFSFamrDNSName + AdfsServicePathPairs.GetServicePath(AdfsServicePathPairs.PathKey.Ls);
            global.Attributes.Append(attr);

            attr = doc.CreateAttribute("ADFSRelyingPartyID");
            attr.Value = "0c820403-cd42-e311-80b9-00155db08b14";
            endpoint.Attributes.Append(attr);
            attr = doc.CreateAttribute("ADFSRelyingPartyName");
            attr.Value = "fed1";
            endpoint.Attributes.Append(attr);
            attr = doc.CreateAttribute("AppID");
            attr.Value = "30D8A29C-A508-84F8-F5B3-726D1513B785";
            endpoint.Attributes.Append(attr);

            attr = doc.CreateAttribute("AppName");
            attr.Value = EnvironmentConfig.App1Name;
            endpoint.Attributes.Append(attr);
            attr = doc.CreateAttribute("ApplicationType");
            attr.Value = "PublishedWebApplication";
            endpoint.Attributes.Append(attr);
            attr = doc.CreateAttribute("BackendAuthNMode");
            attr.Value = "None";
            endpoint.Attributes.Append(attr);
            attr = doc.CreateAttribute("BackendAuthNSPN");
            attr.Value = "";
            endpoint.Attributes.Append(attr);
            attr = doc.CreateAttribute("BackendCertValidationMode");
            attr.Value = "None";
            endpoint.Attributes.Append(attr);
            attr = doc.CreateAttribute("BackendUrl");
            attr.Value = EnvironmentConfig.App1Url;
            endpoint.Attributes.Append(attr);
            attr = doc.CreateAttribute("ClientCertBindingMode");
            attr.Value = "None";
            endpoint.Attributes.Append(attr);
            attr = doc.CreateAttribute("ClientCertificatePreauthenticationThumbprint");
            attr.Value = "";
            endpoint.Attributes.Append(attr);
            attr = doc.CreateAttribute("ExternalCertificateThumbprint");
            attr.Value = new X509Certificate2(EnvironmentConfig.WebAppCert, "123").Thumbprint;
            endpoint.Attributes.Append(attr);
            attr = doc.CreateAttribute("ExternalPreauthentication");
            attr.Value = "ADFS";
            endpoint.Attributes.Append(attr);
            attr = doc.CreateAttribute("FrontendUrl");
            attr.Value = EnvironmentConfig.App1Url;
            endpoint.Attributes.Append(attr);
            attr = doc.CreateAttribute("InactiveTransactionsTimeoutSec");
            attr.Value = "300";
            endpoint.Attributes.Append(attr);
            attr = doc.CreateAttribute("TranslateUrlInRequestHeaders");
            attr.Value = "true";
            endpoint.Attributes.Append(attr);
            attr = doc.CreateAttribute("TranslateUrlInResponseHeaders");
            attr.Value = "true";
            endpoint.Attributes.Append(attr);
            attr = doc.CreateAttribute("UseOAuthAuthentication");
            attr.Value = "false";
            endpoint.Attributes.Append(attr);


            return doc;
        }
        /// <summary>
        /// called when processing a valid request message and then verify state, discovery or enrollment
        /// </summary>
        /// <param name="req"></param>
        public static HttpResponse VerifyRequest(HttpMessage req)
        {
            HttpResponse ret = null;
            if (req == null)
                testSite.Assert.Fail("Not receive any request");

            if (!(req is HttpRequest))
                testSite.Assert.Fail("only expect HTTP request");
            HttpRequest r = (HttpRequest)req;
            switch (r.Method)
            {
                case HttpRequest.HttpMethod.PUT:
                    {
                        if (r.RequestUrl.Path.ToLower().StartsWith(Constraints.StoreUrl.ToLower() + "/"))
                        {
                            StoreEntry entry = (StoreEntry)JsonUtility.DeserializeJSON(r.Body, typeof(StoreEntry));
                            if (addData(entry, true))
                                return createSuccess();
                            else
                                return createInternalError();
                        }
                    }
                    break;
                case HttpRequest.HttpMethod.GET:
                    {
                        if (r.RequestUrl.Path.ToLower() == Constraints.StoreUrl.ToLower())
                        {
                            StoreEntry[] array = getExternalEntries().ToArray();

                            ret = createSuccess();
                            ret.Body = JsonUtility.SerializeJSON(array);
                            ret.SetHeaderField(System.Net.HttpResponseHeader.ContentLength, ret.Body.Length.ToString());
                            return ret;
                        }
                        else if (r.RequestUrl.Path.ToLower() == Constraints.FederationMetadataUrl.ToLower())
                        {
                        }
                        else if (r.RequestUrl.Path.ToLower() == Constraints.GetSTSConfigurationUrl.ToLower())
                        {
                            ret = new HttpResponse(System.Net.HttpStatusCode.OK);
                            STSConfiguration conf = createValidStsConfigurationResponse();
                            ret.Body = JsonUtility.Encode(conf.ToString());
                            ret.SetHeaderField(System.Net.HttpResponseHeader.ContentLength, ret.Body.Length.ToString());
                            return ret;
                        }
                        else if (r.RequestUrl.Path.ToLower() == Constraints.ProxyTrustUrl.ToLower())
                        {
                            StoreEntry entry = getData(TrustIdentifierRecord);
                            if (entry == null)
                            {
                                return createNotFound();
                            }
                            else
                            {
                                ret = new HttpResponse(System.Net.HttpStatusCode.OK);
                                ProxyRelyingPartyTrust pt = new ProxyRelyingPartyTrust();
                                pt.Identifier = entry.value;
                                ret.Body = pt.ToString();
                                return ret;
                            }
                        }
                        else if (r.RequestUrl.Path.ToLower() == Constraints.StoreUrl.ToLower())
                        {
                        }
                        else if (r.RequestUrl.Path.ToLower() == Constraints.RelyingPartyTrustUrl.ToLower())
                        {
                            StoreEntry entry = getData(managedAppsRecord);
                            if (entry == null)
                                return createNotFound();
                            else
                            {
                                ret = createSuccess();
                                ret.Body = entry.value;
                                ret.SetHeaderField(System.Net.HttpResponseHeader.ContentLength, ret.Body.Length.ToString());
                                return ret;
                            }
                        }
                        else if (r.RequestUrl.Path.ToLower().StartsWith(Constraints.StoreUrl.ToLower() + "/"))
                        {
                            StoreEntry entry = getData(r.RequestUrl.Path.ToLower().Substring((Constraints.StoreUrl.ToLower() + "/").Length));
                            if (entry == null)
                                return createNotFound();
                            else
                            {
                                ret = createSuccess();
                                ret.Body = entry.ToString();
                                ret.SetHeaderField(System.Net.HttpResponseHeader.ContentType, "application/json;charset=UTF-8");
                                ret.SetHeaderField(System.Net.HttpResponseHeader.ContentLength, ret.Body.Length.ToString());
                                return ret;
                            }
                        }
                    }
                    break;
                case HttpRequest.HttpMethod.POST:
                    {
                        if (r.RequestUrl.Path.ToLower() == Constraints.EstablishTrustUrl.ToLower())
                        {
                            StoreEntry entry = getData(TrustCertificateRecord);
                            if (entry != null)
                            {
                                testSite.Assert.Fail("Trust already established");
                                return createInternalError();
                            }
                            EstablishTrustRequest dict = (EstablishTrustRequest)JsonUtility.DeserializeJSON(r.Body, typeof(EstablishTrustRequest));
                            entry = new StoreEntry();
                            entry.key = TrustCertificateRecord;
                            entry.version = -1;
                            entry.value = dict.SerializedTrustCertificate;
                            if (addData(entry))
                                return createSuccess();
                        }
                        else if (r.RequestUrl.Path.ToLower().Contains(Constraints.RelyingPartyTrustUrl.ToLower() + "/"))
                        {
                            return createSuccess();
                        }
                        else if (r.RequestUrl.Path.ToLower() == Constraints.ProxyTrustUrl.ToLower())
                        {
                            StoreEntry entry = getData(TrustIdentifierRecord);
                            if (entry != null)
                            {
                                return createInternalError();
                            }
                            ProxyRelyingPartyTrust dict = (ProxyRelyingPartyTrust)JsonUtility.DeserializeJSON(r.Body, typeof(ProxyRelyingPartyTrust));
                            entry = new StoreEntry();
                            entry.key = TrustIdentifierRecord;
                            entry.version = -1;
                            entry.value = dict.Identifier;
                            if (addData(entry))
                                return createSuccess();
                        }
                        else if (r.RequestUrl.Path.ToLower() == Constraints.StoreUrl.ToLower())
                        {
                        }
                        else if (r.RequestUrl.Path.ToLower().StartsWith(Constraints.StoreUrl.ToLower() + "/"))
                        {
                            StoreEntry entry = (StoreEntry)JsonUtility.DeserializeJSON(r.Body, typeof(StoreEntry));
                            if (addData(entry))
                            {
                                HttpResponse tosend = createSuccess();
                                // Yuqing: MUST set bellows!!!
                                tosend.SetHeaderField(System.Net.HttpResponseHeader.ContentLength, "0");
                                tosend.SetHeaderField(System.Net.HttpResponseHeader.ContentType, "text/html;charset=UTF-8");
                                return tosend;
                            }
                            else
                                return createInternalError();
                        }
                    }
                    break;
                default:
                    testSite.Assert.Fail("Unsupported HTTP method");
                    break;
            }
            //testSite.Assert.IsNotNull(ret, "Request is handled");
            return ret;
        }

        static HttpResponse createSuccess()
        {
            HttpResponse ret = new HttpResponse(System.Net.HttpStatusCode.OK);
            ret.Header.Add(new KeyValuePair<string, string>("Host", EnvironmentConfig.ADFSFamrDNSName));
            return ret;
        }

        static HttpResponse createNotFound()
        {
            HttpResponse t = new HttpResponse(System.Net.HttpStatusCode.NotFound);
            t.SetHeaderField(System.Net.HttpResponseHeader.ContentLength, "0");
            return t;
        }

        static HttpResponse createInternalError()
        {
            HttpResponse t = new HttpResponse(System.Net.HttpStatusCode.InternalServerError);
            t.SetHeaderField(System.Net.HttpResponseHeader.ContentLength, "0");
            return t;
        }

        #region variables used to record state of proxy
        // Suppress OACR Error CA1802
        private const string TrustCertificateRecord = "ProxyTrustCertificateRecord";
        private const string TrustIdentifierRecord = "ProxyTrustIdentifierRecord";

        #endregion

        public static void Initialize(ITestSite site, bool forDeployment)
        {
            testSite = site;
            dataStoreFile = EnvironmentConfig.DataStorePath;
            testSite.Assert.IsNotNull(dataStoreFile, "File path for data store cache cannot be null");
            try
            {
                System.IO.File.Exists(dataStoreFile);
            }
            catch
            {
                testSite.Assert.Fail(dataStoreFile + "  is not a valid file path");
            }

            if (forDeployment && System.IO.File.Exists(dataStoreFile))
                System.IO.File.Delete(dataStoreFile);
            //testSite.Assert.Fail("When run deployment test cases, the datastore file should not exist");
            Reset();
        }

        static ITestSite testSite;

        /// <summary>
        /// reset state cached in this
        /// </summary>
        public static void Reset()
        {
            //load data store


            //{
            //    dataStore.Load(dataStoreFile);
            //}
            //else
            InitStore();
        }

        static bool addData(StoreEntry entry, bool overwrite = false)
        {
            XmlNode duplicated = null;
            foreach (XmlNode node in dataStore.DocumentElement.ChildNodes)
            {
                if (node.Attributes["key"].Value == entry.key)
                {
                    duplicated = node;
                    break;
                }
            }
            if (!overwrite && duplicated != null)
                return false;
            if (duplicated != null)
                dataStore.DocumentElement.RemoveChild(duplicated);
            XmlNode n = dataStore.CreateElement("Data");
            XmlAttribute attr = dataStore.CreateAttribute("key");
            attr.Value = entry.key;
            //n.Attributes = new XmlAttributeCollection();
            n.Attributes.Append(attr);
            attr = dataStore.CreateAttribute("version");
            attr.Value = entry.version.ToString();
            n.Attributes.Append(attr);
            attr = dataStore.CreateAttribute("value");
            attr.Value = entry.value;
            n.Attributes.Append(attr);
            dataStore.DocumentElement.AppendChild(n);
            dataStore.Save(dataStoreFile);
            InitStore();
            return true;
        }

        static void deleteData(string key)
        {
        }

        static void InitStore()
        {

            SigningCertificate = CertificateHelper.CreateSelfSignedCertificate(
                "ADFS Signing - " + EnvironmentConfig.ADFSFamrDNSName);
            if (System.IO.File.Exists(dataStoreFile))
                dataStore.Load(dataStoreFile);
            else if (dataStore.DocumentElement == null)
            {
                dataStore.AppendChild(dataStore.CreateElement("Root"));



                XmlNode defaultApp = dataStore.CreateElement("Data");
                XmlAttribute attr = dataStore.CreateAttribute("key");
                attr.Value = managedAppsRecord;
                defaultApp.Attributes.Append(attr);
                attr = dataStore.CreateAttribute("version");
                attr.Value = "-1";
                defaultApp.Attributes.Append(attr);
                attr = dataStore.CreateAttribute("value");
                RelyingPartyTrust[] relies = new RelyingPartyTrust[1];

                relies[0] = createInitialRelyingPartyTrust(managedApp1Name, managedApp1Id);

                attr.Value = JsonUtility.SerializeJSON(relies);
                defaultApp.Attributes.Append(attr);
                dataStore.DocumentElement.AppendChild(defaultApp);
            }
        }

        // Suppress OACR Error CA1802
        private const string managedAppsRecord = "ManagedApps";

        private const string managedApp1Name = "TestSuite1";

        static readonly string managedApp1Id = Guid.NewGuid().ToString();

        static RelyingPartyTrust createInitialRelyingPartyTrust(string name, string identifier)
        {
            RelyingPartyTrust ret = new RelyingPartyTrust();
            ret.name = name;
            ret.objectIdentifier = identifier;
            ret.enabled = true;
            return ret;
        }

        static StoreEntry getData(string key)
        {
            StoreEntry ret = null;
            foreach (XmlNode node in dataStore.DocumentElement.ChildNodes)
            {
                if (node.Attributes["key"].Value == key)
                {
                    ret = new StoreEntry();
                    ret.key = key;
                    ret.version = int.Parse(node.Attributes["version"].Value);
                    ret.value = node.Attributes["value"].Value;
                }
            }
            return ret;
        }

        static List<StoreEntry> getExternalEntries()
        {
            List<StoreEntry> list = new List<StoreEntry>();
            foreach (XmlNode node in dataStore.DocumentElement.ChildNodes)
            {

                StoreEntry ret = new StoreEntry();
                ret.key = node.Attributes["key"].Value; ;
                ret.version = int.Parse(node.Attributes["version"].Value);
                if (ret.version == -1)
                    continue;
                ret.value = node.Attributes["value"].Value;
                list.Add(ret);
            }
            return list;
        }


        static string dataStoreFile = "DataStore.xml";

        static XmlDocument dataStore = new XmlDocument();
    }
}
