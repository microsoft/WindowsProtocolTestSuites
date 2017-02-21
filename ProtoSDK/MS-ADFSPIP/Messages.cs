// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;

namespace Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP
{
    [DataContract]
    public class JSONObject
    {
        protected static JavaScriptSerializer Serializer = new JavaScriptSerializer();

        public static T Parse<T>(string json) where T : JSONObject
        {
            return Serializer.Deserialize<T>(json);
        }

        public static bool TryParse<T>(string json, out T parsedObject) where T : JSONObject
        {
            try   { parsedObject = Serializer.Deserialize<T>(json); return true; }
            catch { parsedObject = null; return false; }
        }

        public static bool TryParse<T>(string json) where T : JSONObject
        {
            T obj; return TryParse(json, out obj);
        }

        public override string ToString()
        {
            return Serializer.Serialize(this);
        }
    }

    public class EstablishTrustRequest : JSONObject
    {
        public string SerializedTrustCertificate;
    }

    public class RenewTrustRequest : EstablishTrustRequest
    {
        public string SerializedReplacementCertificate;
    }

    public class Endpoint : JSONObject
    {
        public AuthType                   AuthenticationSchemes;
        public CertificateValidation      CertificateValidation;
        public ClientCertificateQueryMode ClientCertificateQueryMode;
        public PortType                   PortType;
        public PortType                   ServicePortType;
        public string                     ServicePath;
        public string                     Path;
        public bool                       SupportsNtlm;
    }

    public class StoreVersion : JSONObject
    {
        public string key;
        public int    version;
    }

    public class StoreEntry : JSONObject
    {
        public string key;
        public int    version;
        public string value;
    }

    public class ProxyTrust : JSONObject
    {
        public string SerializedTrustCertificate;
    }

    public class ProxyTrustRenewal : JSONObject
    {
        public string SerializedReplacementCertificate;
    }

    public class ProxyRelyingPartyTrust : JSONObject
    {
        public string Identifier;
    }

    public class ServiceConfiguration : JSONObject
    {
        public string[] CustomUpnSuffixes;
        public string[] DeviceCertificateIssuers;
        public string[] DiscoveredUpnSuffixes;
        public int      HttpPort;
        public int      HttpsPort;
        public int      HttpsPortForUserTlsAuth;
        public int      ProxyTrustCertificateLifetime;
        public string   ServiceHostName;
    }

    public class EndpointConfiguration : JSONObject
    {
        public Endpoint[] Endpoints;
    }

    public class STSConfiguration : JSONObject
    {
        public EndpointConfiguration EndpointConfiguration;
        public ServiceConfiguration  ServiceConfiguration;
    }

    public class STS2016Configuration : STSConfiguration
    {
        public string FarmBehavior { get; set; }
    }

    public class RelyingPartyTrustPublishingSettings : JSONObject
    {
        public string ProxyTrustedEndpointUrl;
        public string ExternalUrl;
        public string InternalUrl;
    }

    public class ProxyEndpointMapping : JSONObject
    {
        public string Key;
        public string Value;
    }

    public class RelyingPartyTrust : JSONObject
    {
        public bool                   enabled;
        public string                 name;
        public bool                   nonClaimsAware;
        public string                 objectIdentifier;
        public bool                   publishedThroughProxy;
        public string[]               proxyTrustedEndpoints;
        public ProxyEndpointMapping[] proxyEndpointMappings;
    }

    public class SerializedCookie : JSONObject
    {
        public string Name;
        public string Value;
        public string Path;
        public string Domain;
        public string Expires;
        public string Version;
    }

    public class SerializedHeader : JSONObject
    {
        public string Name;
        public string Value;
    }

    public class SerializedRequest : JSONObject
    {
        public string             ContentEncoding;
        public long               ContentLength;
        public string             ContentType;
        public string             HttpMethod;
        public string             RequestUri;
        public string             UserAgent;
        public string             UserHostAddress;
        public string             UserHostName;
        public string[]           QueryString;
        public string[]           AcceptTypes;
        public string[]           Content;
        public string[]           UserLanguages;
        public SerializedCookie[] Cookies;
        public SerializedHeader[] Headers;
    }

    public class SerializedRequestWithCertificate : JSONObject
    {
        public SerializedRequest Request;
        public String SerializedClientCertificate;
        public CertificateType CertificateUsage;
    }

    public class ProxyToken : JSONObject
    {
        public string ver;
        public string aud;
        public long   iat;
        public long   exp;
        public string iss;
        public string relyingpartytrustid;
        public string deviceregid;
        public string authinstant;
        public string authmethod;
        public string upn;
    }

    public class CombinedToken : JSONObject
    {
        public ProxyToken proxy_token;
        public string     access_token;
    }

    public class ProxyTokenWrapper : JSONObject
    {
        public string AuthToken;
    }

    public enum CertificateValidation
    {
        None   = 0,
        User   = 1,
        Device = 2
    }

    public enum CertificateType
    {
        User   = 0,
        Device = 1
    }

    public enum ClientCertificateQueryMode
    {
        None            = 0,
        QueryAndAccept  = 1,
        QueryAndRequire = 2
    }

    public enum PortType
    {
        HttpPort                = 0,
        HttpsPort               = 1,
        HttpsPortForUserTlsAuth = 2
    }

    public enum AuthType
    {
        Basic = 8,
        Anonymous = 32768
    }
}
