// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net.Cache;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /// <summary>
    /// The configuration for KKDCP Client
    /// </summary>
    public class KKDCPClientConfig
    {
        /// <summary>
        /// A string containing the URL of the KKDCP server.
        /// </summary>
        public string KKDCPServerURL;

        /// <summary>
        /// The client certificate. 
        /// NULL indicates no client certificate is used when establishing TLS connection.
        /// </summary>
        public X509Certificate2 TlsClientCertificate;

        /// <summary>
        /// System.Net.HttpVersion. The Http version used in the client.
        /// The default value is Http1.1
        /// </summary>
        public Version HttpVersion = System.Net.HttpVersion.Version11;

        /// <summary>
        /// A boolean value indicating whether KeepAlive is true in the Http header.
        /// The default value is true.
        /// </summary>
        public bool HttpKeepAlive = true;

        /// <summary>
        /// Http cache policy used in the client.
        /// The default value is ByPass.
        /// </summary>
        public RequestCachePolicy HttpCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);

        /// <summary>
        /// The UserAgent in the http header.
        /// The default value is Kerberos/1.0
        /// </summary>
        public string HttpUserAgent = "Kerberos/1.0";

        /// <summary>
        /// Timeout for the http request.
        /// The default value is 100,000 milliseconds (100 seconds).
        /// </summary>
        public int HttpRequestTimeout = 100000;
    }
}
