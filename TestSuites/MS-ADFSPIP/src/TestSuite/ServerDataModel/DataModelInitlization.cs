// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    public partial class ServerDataModel
    {
        // constants used privately by this class for test purpose
        private const int MinStoreVersion = 1;
        private const int MaxStoreVersion = 100;
        private const int ConfigPollingIntervalSec = 1000;
        private const int AccessTokenAcceptanceDurationSec = 120;
        private const int PersistentAccessCookieExpirationTimeSec = 0;
        private const string TestEndpointGuid1 = "CD302970-AF1C-4D70-BCB3-F0429E20F95C";
        private const string TestEndpointGuid2 = "CFA623F9-18CF-44F2-9751-D1BC8D7D630A";
        private const string SchemaVersion = "Windows2014";
        private const string farmBehavior = "10.0";

        /// <summary>
        /// Initializes STS configuration.
        /// </summary>
        private void InitializeConfiguration()
        {
            #region Endpoint Configuration

            var endpoints = new List<Endpoint> {
                new Endpoint {
                    AuthenticationSchemes      = AuthType.Anonymous,
                    CertificateValidation      = CertificateValidation.None,
                    ClientCertificateQueryMode = ClientCertificateQueryMode.None,
                    Path                       = AdfsServicePathPairs.FederationMetadata,
                    ServicePath                = AdfsServicePathPairs.FederationMetadata,
                    ServicePortType            = PortType.HttpsPort,
                    PortType                   = PortType.HttpsPort,
                    SupportsNtlm               = false
                },
                new Endpoint {
                    AuthenticationSchemes      = AuthType.Anonymous,
                    CertificateValidation      = CertificateValidation.Device,
                    ClientCertificateQueryMode = ClientCertificateQueryMode.None,
                    Path                       = AdfsServicePathPairs.Ls,
                    ServicePath                = AdfsServicePathPairs.Ls,
                    ServicePortType            = PortType.HttpsPort,
                    PortType                   = PortType.HttpsPort,
                    SupportsNtlm               = false
                },

                new Endpoint {
                    AuthenticationSchemes      = AuthType.Anonymous,
                    CertificateValidation      = CertificateValidation.User,
                    ClientCertificateQueryMode = ClientCertificateQueryMode.QueryAndRequire,
                    Path                       = AdfsServicePathPairs.Ls,
                    ServicePath                = AdfsServicePathPairs.Ls,
                    ServicePortType            = PortType.HttpsPortForUserTlsAuth,
                    PortType                   = PortType.HttpsPortForUserTlsAuth,
                    SupportsNtlm               = false
                },

                new Endpoint {
                    AuthenticationSchemes      = AuthType.Anonymous,
                    CertificateValidation      = CertificateValidation.None,
                    ClientCertificateQueryMode = ClientCertificateQueryMode.None,
                    Path                       = AdfsServicePathPairs.Portal,
                    ServicePath                = AdfsServicePathPairs.Portal,
                    ServicePortType            = PortType.HttpsPortForUserTlsAuth,
                    PortType                   = PortType.HttpsPortForUserTlsAuth,
                    SupportsNtlm               = false
                },

                new Endpoint {
                    AuthenticationSchemes      = AuthType.Anonymous,
                    CertificateValidation      = CertificateValidation.None,
                    ClientCertificateQueryMode = ClientCertificateQueryMode.None,
                    Path                       = AdfsServicePathPairs.OAuth2Token,
                    ServicePath                = AdfsServicePathPairs.OAuth2Token,
                    ServicePortType            = PortType.HttpsPort,
                    PortType                   = PortType.HttpsPort,
                    SupportsNtlm               = false
                },

                new Endpoint {
                    AuthenticationSchemes      = AuthType.Anonymous,
                    CertificateValidation      = CertificateValidation.Device,
                    ClientCertificateQueryMode = ClientCertificateQueryMode.None,
                    Path                       = AdfsServicePathPairs.OAuth2Auth,
                    ServicePath                = AdfsServicePathPairs.OAuth2Auth,
                    ServicePortType            = PortType.HttpsPort,
                    PortType                   = PortType.HttpsPort,
                    SupportsNtlm               = false
                },

                new Endpoint {
                    AuthenticationSchemes      = AuthType.Anonymous,
                    CertificateValidation      = CertificateValidation.User,
                    ClientCertificateQueryMode = ClientCertificateQueryMode.QueryAndRequire,
                    Path                       = AdfsServicePathPairs.OAuth2Auth,
                    ServicePath                = AdfsServicePathPairs.OAuth2Auth,
                    ServicePortType            = PortType.HttpsPortForUserTlsAuth,
                    PortType                   = PortType.HttpsPortForUserTlsAuth,
                    SupportsNtlm               = false
                },
                new Endpoint {
                    AuthenticationSchemes      = AuthType.Anonymous,
                    CertificateValidation      = CertificateValidation.None,
                    ClientCertificateQueryMode = ClientCertificateQueryMode.None,
                    Path                       = AdfsServicePathPairs.EnrollmentServer,
                    ServicePath                = AdfsServicePathPairs.EnrollmentServer,
                    ServicePortType            = PortType.HttpsPort,
                    PortType                   = PortType.HttpsPort,
                    SupportsNtlm               = false
                },

                new Endpoint {
                    AuthenticationSchemes      = AuthType.Anonymous,
                    CertificateValidation      = CertificateValidation.None,
                    ClientCertificateQueryMode = ClientCertificateQueryMode.None,
                    Path                       = AdfsServicePathPairs.Portal,
                    ServicePath                = AdfsServicePathPairs.Portal,
                    ServicePortType            = PortType.HttpsPort,
                    PortType                   = PortType.HttpsPort,
                    SupportsNtlm               = false
                },

                new Endpoint {
                    AuthenticationSchemes      = AuthType.Anonymous,
                    CertificateValidation      = CertificateValidation.None,
                    ClientCertificateQueryMode = ClientCertificateQueryMode.None,
                    Path                       = AdfsServicePathPairs.WindowsTransportTrust2005,
                    ServicePath                = AdfsServicePathPairs.WindowsTransportTrust2005,
                    ServicePortType            = PortType.HttpsPort,
                    PortType                   = PortType.HttpsPort,
                    SupportsNtlm               = false
                },

                new Endpoint {
                    AuthenticationSchemes      = AuthType.Anonymous,
                    CertificateValidation      = CertificateValidation.None,
                    ClientCertificateQueryMode = ClientCertificateQueryMode.None,
                    Path                       = AdfsServicePathPairs.CertificateMixedTrust2005,
                    ServicePath                = AdfsServicePathPairs.CertificateMixedTrust2005,
                    ServicePortType            = PortType.HttpsPort,
                    PortType                   = PortType.HttpsPort,
                    SupportsNtlm               = false
                },
                new Endpoint {
                    AuthenticationSchemes      = AuthType.Anonymous,
                    CertificateValidation      = CertificateValidation.None,
                    ClientCertificateQueryMode = ClientCertificateQueryMode.QueryAndRequire,
                    Path                       = AdfsServicePathPairs.CertificateTransportTrust2005,
                    ServicePath                = AdfsServicePathPairs.CertificateTransportTrust2005,
                    ServicePortType            = PortType.HttpsPortForUserTlsAuth,
                    PortType                   = PortType.HttpsPortForUserTlsAuth,
                    SupportsNtlm               = false
                },
                new Endpoint {
                    AuthenticationSchemes      = AuthType.Anonymous,
                    CertificateValidation      = CertificateValidation.None,
                    ClientCertificateQueryMode = ClientCertificateQueryMode.None,
                    Path                       = AdfsServicePathPairs.UsernameMixedTrust2005,
                    ServicePath                = AdfsServicePathPairs.UsernameMixedTrust2005,
                    ServicePortType            = PortType.HttpsPort,
                    PortType                   = PortType.HttpsPort,
                    SupportsNtlm               = false
                },
                new Endpoint {
                    AuthenticationSchemes      = AuthType.Anonymous,
                    CertificateValidation      = CertificateValidation.None,
                    ClientCertificateQueryMode = ClientCertificateQueryMode.None,
                    Path                       = AdfsServicePathPairs.IssuedTokenMixedAsymmetricBasic256Trust2005,
                    ServicePath                = AdfsServicePathPairs.IssuedTokenMixedAsymmetricBasic256Trust2005,
                    ServicePortType            = PortType.HttpsPort,
                    PortType                   = PortType.HttpsPort,
                    SupportsNtlm               = false
                },
                new Endpoint {
                    AuthenticationSchemes      = AuthType.Anonymous,
                    CertificateValidation      = CertificateValidation.None,
                    ClientCertificateQueryMode = ClientCertificateQueryMode.None,
                    Path                       = AdfsServicePathPairs.IssuedTokenMixedSymmetricBasic256Trust2005,
                    ServicePath                = AdfsServicePathPairs.IssuedTokenMixedSymmetricBasic256Trust2005,
                    ServicePortType            = PortType.HttpsPort,
                    PortType                   = PortType.HttpsPort,
                    SupportsNtlm               = false
                },
                new Endpoint {
                    AuthenticationSchemes      = AuthType.Anonymous,
                    CertificateValidation      = CertificateValidation.None,
                    ClientCertificateQueryMode = ClientCertificateQueryMode.None,
                    Path                       = AdfsServicePathPairs.CertificateMixedTrust13,
                    ServicePath                = AdfsServicePathPairs.CertificateMixedTrust13,
                    ServicePortType            = PortType.HttpsPort,
                    PortType                   = PortType.HttpsPort,
                    SupportsNtlm               = false
                },
                new Endpoint {
                    AuthenticationSchemes      = AuthType.Anonymous,
                    CertificateValidation      = CertificateValidation.None,
                    ClientCertificateQueryMode = ClientCertificateQueryMode.None,
                    Path                       = AdfsServicePathPairs.UsernameMixedTrust13,
                    ServicePath                = AdfsServicePathPairs.UsernameMixedTrust13,
                    ServicePortType            = PortType.HttpsPort,
                    PortType                   = PortType.HttpsPort,
                    SupportsNtlm               = false
                },
                new Endpoint {
                    AuthenticationSchemes      = AuthType.Anonymous,
                    CertificateValidation      = CertificateValidation.None,
                    ClientCertificateQueryMode = ClientCertificateQueryMode.None,
                    Path                       = AdfsServicePathPairs.IssuedTokenMixedAsymmetricBasic256Trust13,
                    ServicePath                = AdfsServicePathPairs.IssuedTokenMixedAsymmetricBasic256Trust13,
                    ServicePortType            = PortType.HttpsPort,
                    PortType                   = PortType.HttpsPort,
                    SupportsNtlm               = false
                },
                new Endpoint {
                    AuthenticationSchemes      = AuthType.Anonymous,
                    CertificateValidation      = CertificateValidation.None,
                    ClientCertificateQueryMode = ClientCertificateQueryMode.None,
                    Path                       = AdfsServicePathPairs.IssuedTokenMixedSymmetricBasic256Trust13,
                    ServicePath                = AdfsServicePathPairs.IssuedTokenMixedSymmetricBasic256Trust13,
                    ServicePortType            = PortType.HttpsPort,
                    PortType                   = PortType.HttpsPort,
                    SupportsNtlm               = false
                },
                new Endpoint {
                    AuthenticationSchemes      = AuthType.Anonymous,
                    CertificateValidation      = CertificateValidation.None,
                    ClientCertificateQueryMode = ClientCertificateQueryMode.None,
                    Path                       = AdfsServicePathPairs.Mex,
                    ServicePath                = AdfsServicePathPairs.ProxyMex,
                    ServicePortType            = PortType.HttpsPort,
                    PortType                   = PortType.HttpsPort,
                    SupportsNtlm               = false
                }
            };
            #endregion

            #region Service Configuration

            var serviceConfiguration = new ServiceConfiguration
            {
                HttpPort = 80,
                HttpsPort = 443,
                HttpsPortForUserTlsAuth = 49443,
                ProxyTrustCertificateLifetime = 21600,
                CustomUpnSuffixes = new string[0],
                DeviceCertificateIssuers = new string[0],
                DiscoveredUpnSuffixes = new string[] { EnvironmentConfig.DomainName.ToUpper() },
                ServiceHostName = EnvironmentConfig.ADFSFamrDNSName
            };

            #endregion

            if (EnvironmentConfig.IsWin2016)
            {
                // Add 2016 new added endpoint
                #region 2016 New Added Endpoints
                endpoints.AddRange(new List<Endpoint>() { 
                    new Endpoint {
                        AuthenticationSchemes      = AuthType.Anonymous,
                        CertificateValidation      = CertificateValidation.None,
                        ClientCertificateQueryMode = ClientCertificateQueryMode.None,
                        Path                       = AdfsServicePathPairs.OpenIdConfiguration,
                        ServicePath                = AdfsServicePathPairs.OpenIdConfiguration,
                        ServicePortType            = PortType.HttpsPort,
                        PortType                   = PortType.HttpsPort,
                        SupportsNtlm               = false
                    },
                    new Endpoint {
                        AuthenticationSchemes      = AuthType.Anonymous,
                        CertificateValidation      = CertificateValidation.None,
                        ClientCertificateQueryMode = ClientCertificateQueryMode.None,
                        Path                       = AdfsServicePathPairs.DiscroveryKeys,
                        ServicePath                = AdfsServicePathPairs.DiscroveryKeys,
                        ServicePortType            = PortType.HttpsPort,
                        PortType                   = PortType.HttpsPort,
                        SupportsNtlm               = false
                    },
                    new Endpoint {
                        AuthenticationSchemes      = AuthType.Anonymous,
                        CertificateValidation      = CertificateValidation.None,
                        ClientCertificateQueryMode = ClientCertificateQueryMode.None,
                        Path                       = AdfsServicePathPairs.WebFinger,
                        ServicePath                = AdfsServicePathPairs.WebFinger,
                        ServicePortType            = PortType.HttpsPort,
                        PortType                   = PortType.HttpsPort,
                        SupportsNtlm               = false
                    },
                    new Endpoint {
                        AuthenticationSchemes      = AuthType.Anonymous,
                        CertificateValidation      = CertificateValidation.None,
                        ClientCertificateQueryMode = ClientCertificateQueryMode.None,
                        Path                       = AdfsServicePathPairs.AdfsUserInfo,
                        ServicePath                = AdfsServicePathPairs.AdfsUserInfo,
                        ServicePortType            = PortType.HttpsPort,
                        PortType                   = PortType.HttpsPort,
                        SupportsNtlm               = false
                    }
                });
                #endregion

                var endpointConfiguration = new EndpointConfiguration() { Endpoints = endpoints.ToArray() };

                _configuration = new STS2016Configuration
                {
                    EndpointConfiguration = endpointConfiguration,
                    ServiceConfiguration = serviceConfiguration,
                    FarmBehavior = farmBehavior //2016 new Added property
                };
            }
            else
            {
                var endpointConfiguration = new EndpointConfiguration() { Endpoints = endpoints.ToArray() };
                _configuration = new STSConfiguration
                {
                    EndpointConfiguration = endpointConfiguration,
                    ServiceConfiguration = serviceConfiguration
                };
            }
        }

        /// <summary>
        /// Initializes server data store.
        /// </summary>
        private void InitializeDataStore(bool includeEndpoint = false)
        {
            // construct global config
            var storeConfigString = string.Empty;

            if (EnvironmentConfig.IsWin2016)
            {
                GlobalConfig_2016 globalConfig = new GlobalConfig_2016();
                EndpointConfig_2016[] endpointConfig = null;

                this.InitialBaseGlobalConfig(globalConfig);
                globalConfig.AccessTokenAcceptanceDurationSec = Convert.ToString(AccessTokenAcceptanceDurationSec);
                globalConfig.ActiveEndpointAuthenticationURL = UrlHelper.CombineUrls(EnvironmentConfig.ADFSServerUrl, AdfsServicePathPairs.ActiveEndpointAuthenticationURL);
                globalConfig.SchemaVersion = SchemaVersion;
                globalConfig.StsSignOutURL = UrlHelper.CombineUrls(EnvironmentConfig.ADFSServerUrl, AdfsServicePathPairs.StsSignOutURL);

                if (includeEndpoint)
                {
                    EndpointConfig_2016 testEndpoint = new EndpointConfig_2016();
                    this.InitialBaseEndpointConfig(testEndpoint);
                    testEndpoint.PersistentAccessCookieExpirationTimeSec = Convert.ToString(PersistentAccessCookieExpirationTimeSec);
                    testEndpoint.DisableHttpOnlyCookieProtection = Convert.ToString(false).ToLower();
                    testEndpoint.EnableHttpRedirect = Convert.ToString(false).ToLower();
                    testEndpoint.EnableSignOut = Convert.ToString(false).ToLower();
                    endpointConfig = new[] { testEndpoint };
                }

                var storeConfig = new StoreConfig_2016
                {
                    GlobalConfig = (GlobalConfig_2016)globalConfig,
                    EndpointConfig = endpointConfig
                };
                storeConfigString = storeConfig.ToString().EncodeToBase64();
            }
            else
            {
                GlobalConfig globalConfig = new GlobalConfig();
                EndpointConfig[] endpointConfig = null;

                this.InitialBaseGlobalConfig(globalConfig);

                if (includeEndpoint)
                {
                    EndpointConfig testEndpoint = new EndpointConfig();
                    this.InitialBaseEndpointConfig(testEndpoint);
                    endpointConfig = new[] { testEndpoint };
                }
                var storeConfig = new StoreConfig
                {
                    GlobalConfig = globalConfig,
                    EndpointConfig = endpointConfig
                };
                storeConfigString = storeConfig.ToString().EncodeToBase64();
            }


            // if the proxy store already exists, increase its version by one
            // if the proxy store has not been initialized yet, set the store version
            // to a random number. 
            //
            // We do this to somewhat make sure that the store gets a different version
            // each time a test case runs. The proxy only updates its state when it gets
            // a different version from its cache. We want the proxy always to sync its
            // state with server's store config.
            //
            // For more detail, refer to Windows source code: 
            // winblue_gdr/ds/security/ADFSv2/Product/ApplicationProxy/Configuration/src/ProxyConfigManager.cpp
            // in Function UpdateProxyConfig, it says:
            // if (m_currentConfigVersion != newConfigVersion)
            //
            var storeEntryKey = EnvironmentConfig.IsWin2016 ? EnvironmentConfig.SUTConfigEntryKey_2016 : EnvironmentConfig.SUTConfigEntryKey_2012R2;
            var newVersion = _proxyStore == null
                ? MinStoreVersion // new Random(DateTime.Now.Millisecond).Next(MinStoreVersion, MaxStoreVersion)
                : _proxyStore.First(_ => _.key.EqualsIgnoreCase(storeEntryKey)).version + 1;

            //  set the new proxy store
            _proxyStore = new List<StoreEntry> {
                new StoreEntry {
                    version = newVersion,
                    key     = storeEntryKey,
                    value   = storeConfigString
                }
            };
        }

        private void InitialBaseGlobalConfig(GlobalConfig globalConfig)
        {
            globalConfig.Initialize();
            // the time interval proxy trying to get data store configuration
            // all bool values must be lower-case, or the proxy won't accept it
            globalConfig.AccessCookieEncryption = Convert.ToString(false).ToLower();
            globalConfig.ConfigurationChangesPollingIntervalSec = Convert.ToString(ConfigPollingIntervalSec);
            globalConfig.StsUrl = UrlHelper.CombineUrls(EnvironmentConfig.ADFSServerUrl, AdfsServicePathPairs.Ls);
            globalConfig.OAuthAuthenticationURL = UrlHelper.CombineUrls(EnvironmentConfig.ADFSServerUrl, AdfsServicePathPairs.OAuth2Auth);
            globalConfig.ADFSWebApplicationProxyRelyingPartyUri = Constraints.DefaultProxyRelyingPartyTrustIdentifier;
            globalConfig.ConnectedServersName = EnvironmentConfig.SUTDNS;
            globalConfig.StsTokenSigningCertificatePublicKey = AdfsSigningCertificate.ToBase64String();
        }

        private void InitialBaseEndpointConfig(EndpointConfig testEndpoint)
        {
            testEndpoint.Initialize();
            testEndpoint.AppID = TestEndpointGuid2;
            testEndpoint.ADFSRelyingPartyID = TestEndpointGuid1;
            testEndpoint.ADFSRelyingPartyName = EnvironmentConfig.App1Name;
            testEndpoint.AppName = EnvironmentConfig.App1Name;
            testEndpoint.BackendUrl = EnvironmentConfig.App1Url;
            testEndpoint.FrontendUrl = EnvironmentConfig.App1Url;
            testEndpoint.InactiveTransactionsTimeoutSec = Convert.ToString(300);
            testEndpoint.TranslateUrlInRequestHeaders = Convert.ToString(true).ToLower();
            testEndpoint.TranslateUrlInResponseHeaders = Convert.ToString(true).ToLower();
            testEndpoint.UseOAuthAuthentication = Convert.ToString(false).ToLower();
            testEndpoint.ExternalCertificateThumbprint = WebAppCertificate.Thumbprint;
            testEndpoint.ApplicationType = "PublishedWebApplication";
            testEndpoint.BackendAuthNMode = "None";
            testEndpoint.BackendCertValidationMode = "None";
            testEndpoint.ClientCertBindingMode = "None";
            testEndpoint.ExternalPreauthentication = "ADFS";
        }

        /// <summary>
        /// Initializes relying parties.
        /// </summary>
        private void InitializeRelyingParties(bool published = false)
        {
            var relyingParty = new RelyingPartyTrust
            {
                name = EnvironmentConfig.App1Name,
                objectIdentifier = TestEndpointGuid1,
                enabled = true,
                nonClaimsAware = false,
                publishedThroughProxy = false
            };

            //add nonClaimsAware
            var nonRP = new RelyingPartyTrust
            {
                name = EnvironmentConfig.App2Name,
                objectIdentifier = TestEndpointGuid2,
                enabled = true,
                nonClaimsAware = true,
                publishedThroughProxy = false
            };

            if (published)
            {
                relyingParty.publishedThroughProxy = true;
                relyingParty.proxyTrustedEndpoints = new[]{
                    EnvironmentConfig.App1Url
                };
                relyingParty.proxyEndpointMappings = new[]{
                    new ProxyEndpointMapping{
                        Key   = EnvironmentConfig.App1Url,
                        Value = EnvironmentConfig.App1Url
                    }
                };

                nonRP.publishedThroughProxy = true;
                nonRP.proxyTrustedEndpoints = new[]{
                    EnvironmentConfig.App2Url
                };
                nonRP.proxyEndpointMappings = new[]{
                    new ProxyEndpointMapping{
                        Key = EnvironmentConfig.App2Url,
                        Value = EnvironmentConfig.App2Url
                    }
                };
            }

            _relyingPartyTrust = new List<RelyingPartyTrust> { relyingParty, nonRP };
        }

        /// <summary>
        /// Initializes proxy trusts.
        /// </summary>
        private void InitializeProxyTrust()
        {
            _proxyRelyingPartyTrust = new ProxyRelyingPartyTrust();
            _proxyTrustedCertificate = new List<ProxyTrust>();
        }

        /// <summary>
        /// Initializes federation metadata.
        /// </summary>
        private void InitializeFederationMetadata()
        {
            _metadata = XDocument.Load(EnvironmentConfig.IsWin2016 ? EnvironmentConfig.MetadataTemplate_2016 : EnvironmentConfig.MetadataTemplate_2012R2);

            // note we do not re-sign the metadata, so the SAML
            // signature is incorrect after modifying.
            // but luckily, it seems that the client does not
            // verify the SAML signature.
            XNamespace nSaml = @"urn:oasis:names:tc:SAML:2.0:metadata";
            XNamespace nSig = @"http://www.w3.org/2000/09/xmldsig#";

            // replace all encryption certificates in the metadata
            _metadata.Root.Descendants(nSaml + "KeyDescriptor")
                          .Where(_ => _.Attribute("use").Value.Equals("encryption")).ToList()
                          .ForEach(_ => _.Descendants(nSig + "X509Certificate").First()
                          .SetValue(_adfsEncryptionCertificate.ToBase64String()));

            // replace all signing certificates in the metadata
            _metadata.Root.Descendants(nSaml + "KeyDescriptor")
                          .Where(_ => _.Attribute("use").Value.Equals("signing")).ToList()
                          .ForEach(_ => _.Descendants(nSig + "X509Certificate").First()
                          .SetValue(_adfsSigningCertificate.ToBase64String()));
        }
    }
}
