// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    [Serializable]
    [XmlRoot("GlobalConfig")]
    public class GlobalConfig
    {
        [XmlAttribute] public string ADFSWebApplicationProxyRelyingPartyUri;
        [XmlAttribute] public string AccessCookieEncryption;
        [XmlAttribute] public string AccessCookieEncryptionKey;
        [XmlAttribute] public string AccessTokenApplicationUrlClaimName;
        [XmlAttribute] public string AccessTokenClientCertificateClaimName;
        [XmlAttribute] public string AccessTokenName;
        [XmlAttribute] public string AccessTokenUpnClaimName;
        [XmlAttribute] public string AppProxySPN;
        [XmlAttribute] public string AuthenticationPackageNameLSA;
        [XmlAttribute] public string AuthenticationPackageNameSSPI;
        [XmlAttribute] public string ConfigurationChangesPollingIntervalSec;
        [XmlAttribute] public string ConnectedServersName;
        [XmlAttribute] public string OAuthAuthenticationURL;
        [XmlAttribute] public string ServiceAccountNameForKCD;
        [XmlAttribute] public string ServiceAccountPasswordForKCD;
        [XmlAttribute] public string StsTokenSigningCertificatePublicKey;
        [XmlAttribute] public string StsUrl;
        public void Initialize(){ this.ResetStringFields();}
    }

    [Serializable]
    [XmlRoot("GlobalConfig")]
    public class GlobalConfig_2016 : GlobalConfig
    {
        [XmlAttribute] public string AccessTokenAcceptanceDurationSec;
        [XmlAttribute] public string ActiveEndpointAuthenticationURL;
        [XmlAttribute] public string SchemaVersion;
        [XmlAttribute] public string StsSignOutURL;
    }

    [Serializable]
    [XmlRoot("EndpointConfig")]
    public class EndpointConfig
    {
        [XmlAttribute] public string ADFSRelyingPartyID;
        [XmlAttribute] public string ADFSRelyingPartyName;
        [XmlAttribute] public string AppID;
        [XmlAttribute] public string AppName;
        [XmlAttribute] public string ApplicationType;
        [XmlAttribute] public string BackendAuthNMode;
        [XmlAttribute] public string BackendAuthNSPN;
        [XmlAttribute] public string BackendCertValidationMode;
        [XmlAttribute] public string BackendUrl;
        [XmlAttribute] public string ClientCertBindingMode;
        [XmlAttribute] public string ClientCertificatePreauthenticationThumbprint;
        [XmlAttribute] public string ExternalCertificateThumbprint;
        [XmlAttribute] public string ExternalPreauthentication;
        [XmlAttribute] public string FrontendUrl;
        [XmlAttribute] public string InactiveTransactionsTimeoutSec;
        [XmlAttribute] public string TranslateUrlInRequestHeaders;
        [XmlAttribute] public string TranslateUrlInResponseHeaders;
        [XmlAttribute] public string UseOAuthAuthentication;
        public void Initialize(){ this.ResetStringFields();}
    }

    [Serializable]
    [XmlRoot("EndpointConfig")]
    public class EndpointConfig_2016 : EndpointConfig
    {
        [XmlAttribute] public string ADFSUserCertificateStore;
        [XmlAttribute] public string DisableHttpOnlyCookieProtection;
        [XmlAttribute] public string EnableHttpRedirect;
        [XmlAttribute] public string EnableSignOut;
        [XmlAttribute] public string PersistentAccessCookieExpirationTimeSec;
    }

    /// <summary>
    /// Represent the Configuration XML in the value of a Story Entry
    /// </summary>
    [Serializable]
    [XmlRoot("Configuration")]
    public class StoreConfig
    {
        [XmlElement]
        public GlobalConfig GlobalConfig;
        [XmlElement]
        public EndpointConfig[] EndpointConfig;

        #region Serialization
        /// <summary>
        /// Deserialize from an XML string.
        /// </summary>
        public static StoreConfig FromXml(string xml)
        {
            object obj;
            var ser = new XmlSerializer(typeof(StoreConfig));
            using (var reader = new StringReader(xml))
            {
                obj = ser.Deserialize(reader);
            }
            return (StoreConfig)obj;
        }

        /// <summary>
        /// Serialize to XML string.
        /// </summary>
        public string ToXml()
        {
            // No namespace declaration in the output xml string
            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(typeof(StoreConfig));
            var settings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true };
            // omit xml declaration in the output xml string

            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, this, emptyNamepsaces);
                return stream.ToString();
            }
        }

        /// <summary>
        /// Serialize to XML string.
        /// </summary>
        public override string ToString() { return ToXml(); }

        #endregion
    }

    /// <summary>
    /// Represent the Configuration XML in the value of a Story Entry
    /// </summary>
    [Serializable]
    [XmlRoot("Configuration")]
    public class StoreConfig_2016
    {
        [XmlElement]
        public GlobalConfig_2016 GlobalConfig;
        [XmlElement]
        public EndpointConfig_2016[] EndpointConfig;

        #region Serialization
        /// <summary>
        /// Deserialize from an XML string.
        /// </summary>
        public static StoreConfig_2016 FromXml(string xml)
        {
            object obj;
            var ser = new XmlSerializer(typeof(StoreConfig_2016));
            using (var reader = new StringReader(xml))
            {
                obj = ser.Deserialize(reader);
            }
            return (StoreConfig_2016)obj;
        }

        /// <summary>
        /// Serialize to XML string.
        /// </summary>
        public string ToXml()
        {
            // No namespace declaration in the output xml string
            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(typeof(StoreConfig_2016));
            var settings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true };
            // omit xml declaration in the output xml string

            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, this, emptyNamepsaces);
                return stream.ToString();
            }
        }

        /// <summary>
        /// Serialize to XML string.
        /// </summary>
        public override string ToString() { return ToXml(); }

        #endregion
    }
}
