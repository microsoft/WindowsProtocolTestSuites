// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP
{
    public static class Constraints
    {
        public const int HTTPSServiceDefaultPort = 443;
        public const string DefaultUriSchema = "https";
        public const string AdfsProxyUrl = "adfs/Proxy";
        public const string EstablishTrustUrl = "adfs/Proxy/EstablishTrust";
        public const string RenewTrustUrl = "adfs/Proxy/RenewTrust";
        public const string ProxyTrustUrl = "adfs/Proxy/WebApplicationProxy/Trust";
        public const string GetSTSConfigurationUrl = "adfs/Proxy/GetConfiguration";
        public const string RelyingPartyTrustUrl = "adfs/Proxy/RelyingPartyTrusts";
        public const string StoreUrl = "adfs/Proxy/WebApplicationProxy/Store";
        public const string BackEndProxyTLSUrl = "adfs/BackEndProxyTLS";
        public const string FederationMetadataUrl = "FederationMetadata/2007-06/FederationMetadata.xml";
        public const string FederationAuthUrl = "adfs/ls";
        public const string DefaultProxyRelyingPartyTrustIdentifier = "urn:AppProxy:com";
        public const string DefaultAuthMethod = "urn:oasis:names:tc:SAML:2.0:ac:classes:PasswordProtectedTransport";
    }

    public static class AdfsServicePathPairs
    {
        public const string Ls = "/adfs/ls/";
        public const string FederationMetadata = "/FederationMetadata/2007-06/";
        public const string Trust = "/adfs/services/trust";
        public const string Mex = "/adfs/services/trust/mex";
        public const string ProxyMex = "/adfs/services/trust/proxymex";
        public const string Portal = "/adfs/portal/";
        public const string OAuth2Auth = "/adfs/oauth2/authorize";
        public const string WindowsTransportTrust2005 = "/adfs/services/trust/2005/windowstransport";
        public const string CertificateMixedTrust2005 = "/adfs/services/trust/2005/certificatemixed";
        public const string CertificateTransportTrust2005 = "/adfs/services/trust/2005/certificatetransport";
        public const string UsernameMixedTrust2005 = "/adfs/services/trust/2005/usernamemixed";
        public const string IssuedTokenMixedAsymmetricBasic256Trust2005 = "/adfs/services/trust/2005/issuedtokenmixedasymmetricbasic256";
        public const string IssuedTokenMixedSymmetricBasic256Trust2005 = "/adfs/services/trust/2005/issuedtokenmixedsymmetricbasic256";
        public const string CertificateMixedTrust13 = "/adfs/services/trust/13/certificatemixed";
        public const string UsernameMixedTrust13 = "/adfs/services/trust/13/usernamemixed";
        public const string IssuedTokenMixedAsymmetricBasic256Trust13 = "/adfs/services/trust/13/issuedtokenmixedasymmetricbasic256";
        public const string IssuedTokenMixedSymmetricBasic256Trust13 = "/adfs/services/trust/13/issuedtokenmixedsymmetricbasic256";
        public const string OAuth2Token = "/adfs/oauth2/token";
        public const string EnrollmentServer = "/EnrollmentServer/";
        public const string OpenIdConfiguration = "/adfs/.well-known/openid-configuration";
        public const string DiscroveryKeys = "/adfs/discovery/keys";
        public const string WebFinger = "/.well-known/webfinger";
        public const string AdfsUserInfo = "/adfs/userinfo";
        public const string ActiveEndpointAuthenticationURL = "adfs/proxy/relyingpartytoken";
        public const string StsSignOutURL = "adfs/ls/?wa=wsignout1.0";

        public enum PathKey
        {
            FederationMetadata,
            Ls,
            OAuth2Auth,
            OAuth2Token,
            WindowsTransportTrust2005,
            CertificateMixedTrust2005,
            CertificateMixedTrust13,
            CertificateTransportTrust2005,
            UsernameMixedTrust2005,
            IssuedTokenMixedAsymmetricBasic256Trust2005,
            IssuedTokenMixedSymmetricBasic256Trust2005,
            UsernameMixedTrust13,
            IssuedTokenMixedAsymmetricBasic256Trust13,
            IssuedTokenMixedSymmetricBasic256Trust13,
            Portal,
            Mex,
            ProxyMex,
            EnrollmentServer,
            ActiveEndpointAuthenticationURL,
            StsSignOutURL
        }

        public static string GetServicePath(PathKey k)
        {
            return typeof(AdfsServicePathPairs).GetField(k.ToString()).GetRawConstantValue().ToString();
        }
    }
}
