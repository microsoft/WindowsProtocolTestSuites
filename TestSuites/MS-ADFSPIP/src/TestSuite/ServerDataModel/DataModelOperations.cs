// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    public partial class ServerDataModel
    {
        #region Proxy Trust Operations

        /// <summary>
        /// Add a proxy identifier.
        /// </summary>
        public void AddProxyRelyingPartyTrust(ProxyRelyingPartyTrust proxy)
        {
            _proxyRelyingPartyTrust = proxy;

            NotifyPropertyChanged("ProxyRelyingPartyTrust");
        }

        /// <summary>
        /// Add a proxy certificate.
        /// </summary>
        public void AddProxyTrustCertificate(ProxyTrust proxy)
        {
            // check if the certificate already exists
            if (_proxyTrustedCertificate.Any(_ => _.SerializedTrustCertificate == proxy.SerializedTrustCertificate))
            return;

            _proxyTrustedCertificate.Add(proxy);

            NotifyPropertyChanged("ProxyTrust");
        }

        /// <summary>
        /// Renew a proxy certificate.
        /// </summary>
        public void AddProxyTrustCertificate(ProxyTrustRenewal proxy)
        {
            // check if the certificate already exists
            if (_proxyTrustedCertificate.Any(_ => _.SerializedTrustCertificate == proxy.SerializedReplacementCertificate))
            return;

            _proxyTrustedCertificate.Add(new ProxyTrust {
                SerializedTrustCertificate = proxy.SerializedReplacementCertificate
            });

            NotifyPropertyChanged("ProxyTrust");
        }

        /// <summary>
        /// Gets serialized JSON prosy relying party trusts.
        /// </summary>
        public string GetSerializedProxyRelyingPartyTrust()
        {
            return JsonUtility.SerializeJSON(_proxyRelyingPartyTrust);
        }

        #endregion

        #region STS Common Operations
        
        /// <summary>
        /// Gets serialized STS configurations.
        /// </summary>
        public string GetSerializedStsConfiguration()
        {
            return JsonUtility.Encode(Configuration.ToString());
        }

        /// <summary>
        /// Gets the federation metadata in a string.
        /// </summary>
        public string GetFederationMetadata()
        {
            return _metadata.ToString(SaveOptions.DisableFormatting);
        }

        #endregion

        #region Data Store Operations

        /// <summary>
        /// Gets serialized data story entries.
        /// </summary>
        public string GetSerializedStoreEntry()
        {
            return JsonUtility.SerializeJSON(_proxyStore.Select(_ => new StoreVersion {
                    key     = _.key,
                    version = _.version
            }).ToArray());
        }

        /// <summary>
        /// Gets the store data specified by the entry key.
        /// </summary>
        public string GetSerializedStoreEntry(string entryKey)
        {
            // query store for the entry with the specified key
            var entry = _proxyStore.FirstOrDefault(_ => _.key.EqualsIgnoreCase(entryKey));

            return entry != null ? entry.ToString() : string.Empty;
        }

        /// <summary>
        /// Update the data store with a new entry.
        /// </summary>
        public void UpdateStoreEntry(StoreEntry newEntry)
        {
            // the server MUST find a corresponding Store Entry 
            // on [Server State].ProxyStore for the corresponding key
            var targetEntry = _proxyStore.FirstOrDefault(_ => _.key.EqualsIgnoreCase(newEntry.key));

            if (targetEntry == null) throw new InvalidOperationException("Cannot find Entry " + newEntry.key);
            
            // skip version checking 
            targetEntry.version++;
            // the server MUST set the value of [Store Entry].value
            targetEntry.value = newEntry.value;

            NotifyPropertyChanged("StoreEntry");
        }

        /// <summary>
        /// Add a new entry to the data store.
        /// </summary>
        public void AddStoreEntry(StoreEntry newEntry)
        {
            // check if the entry already exists
            if (_proxyStore.Any(_ => _.key.EqualsIgnoreCase(newEntry.key))) {
                throw new InvalidOperationException("Entry already exists");
            }

            // update proxy store
            _proxyStore.Add(newEntry);

            NotifyPropertyChanged("StoreEntry");
        }

        #endregion

        #region Relying Party Operations

        /// <summary>
        /// Gets serialized JSON relying parties.
        /// </summary>
        public string GetSerializedRelyingPartyTrusts()
        {
            return JsonUtility.SerializeJSON(_relyingPartyTrust.ToArray());
        }

        /// <summary>
        /// Set the test endpoint to be published.
        /// </summary>
        public void SetPublishedEndpoint()
        {
            InitializeDataStore(includeEndpoint: true);
            NotifyPropertyChanged("StoreEntry");

            InitializeRelyingParties(published: true);
            NotifyPropertyChanged("RelyingPartyTrust");
        }

        /// <summary>
        /// Set the test endpoint to be un-published.
        /// </summary>
        public void ResetPublishedEndpoint()
        {
            InitializeDataStore(includeEndpoint: false);
            NotifyPropertyChanged("StoreEntry");

            InitializeRelyingParties(published: false);
            NotifyPropertyChanged("RelyingPartyTrust");
        }

        /// <summary>
        /// Get published application with the given url.
        /// </summary>
        public EndpointConfig GetPublishedEndpoint(string endpointUrl)
        {
            foreach (var entry in _proxyStore) {
                // deserialize the store entry
                if(EnvironmentConfig.IsWin2016)
                {
                    StoreConfig_2016 conf = StoreConfig_2016.FromXml(entry.value.DecodeFromBase64());
                    var endpoint = conf.EndpointConfig.FirstOrDefault(
                    _ => _.FrontendUrl.EqualsIgnoreCase(endpointUrl));

                    // if the endpoint was found, return it
                    if (endpoint != null) return endpoint;
                }
                else
                {
                    StoreConfig conf = StoreConfig.FromXml(entry.value.DecodeFromBase64());
                    var endpoint = conf.EndpointConfig.FirstOrDefault(
                    _ => _.FrontendUrl.EqualsIgnoreCase(endpointUrl));

                    // if the endpoint was found, return it
                    if (endpoint != null) return endpoint;
                }
            }

            // return null if no endpoint was found
            return null;
        }

        #endregion

        #region Authentication Token

        /// <summary>
        /// The the JWT authToken used in url query string.
        /// </summary>
        public string GetAuthToken()
        {
            var proxyToken = new ProxyToken();

            proxyToken.ver                 = "1.0";
            proxyToken.deviceregid         = String.Empty;
            proxyToken.authinstant         = DateTime.UtcNow.ToString("O") + "Z";
            proxyToken.authmethod          = Constraints.DefaultAuthMethod;
            proxyToken.aud                 = Constraints.DefaultProxyRelyingPartyTrustIdentifier;
            proxyToken.iss                 = UrlHelper.CombineUrls(EnvironmentConfig.ADFSServerUrl, AdfsServicePathPairs.Trust);
            proxyToken.upn                 = EnvironmentConfig.AppUserUpn;
            proxyToken.iat                 = Convert.ToInt64(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
            proxyToken.exp                 = Convert.ToInt64(TimeSpan.FromHours(1).TotalSeconds) + proxyToken.iat;
            proxyToken.relyingpartytrustid = TestEndpointGuid1;

            return PreauthJsonWebToken.Encode(proxyToken, _adfsSigningCertificate);
        }

        public string GetBasicAuthorizationHeader()
        {
            string credential = string.Format("{0}:{1}", EnvironmentConfig.DomainAdminUser, EnvironmentConfig.DomainAdminPassword);
            string hash = Base64Helper.Base64Encode(System.Text.Encoding.UTF8.GetBytes(credential));
            return "Basic " + hash;
        }

        public string GetIncorrectBasicAuthorizationHeader()
        {
            string credential = string.Format("{0}:{1}", EnvironmentConfig.DomainAdminUser + "_123", EnvironmentConfig.DomainAdminPassword);
            string hash = Base64Helper.Base64Encode(System.Text.Encoding.UTF8.GetBytes(credential));
            return "Basic " + hash;
        }

        public string GetPreAuthTokenHeader()
        {
            string proxy_token = GetAuthToken();
            string access_token = "VGhpcyBpcyB5b3VyIHBhc3N3b3Jk"; //mocked access token
            string combined_token = "{" + string.Format("\"proxy_token\":\"{0}\", \"access_token\":\"{1}\"", proxy_token, access_token) + "}";
            string hash = Base64Helper.Base64Encode(System.Text.Encoding.UTF8.GetBytes(combined_token));
            return hash;
        }

        #endregion
    }
}
