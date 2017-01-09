// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;

using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Kile
{
    /// <summary>
    /// Kerberos client SecurityContext configuration
    /// </summary>
    [CLSCompliant(false)]
    public class KileClientSecurityConfig : SecurityConfig
    {
        /// <summary>
        /// Client account credential
        /// </summary>
        private AccountCredential clientCredential;

        /// <summary>
        /// Logon name
        /// </summary>
        private string logonName;

        /// <summary>
        /// Service name
        /// </summary>
        private string serviceName;

        /// <summary>
        /// KDC IP address
        /// </summary>
        private IPAddress kdcIpAddress;

        /// <summary>
        /// Client security context attributes
        /// </summary>
        private ClientSecurityContextAttribute securityAttributes;

        /// <summary>
        /// Connection type, e.g. TCP
        /// </summary>
        private KileConnectionType connectionType;


        /// <summary>
        /// Client account credential
        /// </summary>
        [CLSCompliant(false)]
        public AccountCredential ClientCredential
        {
            get
            {
                return this.clientCredential;
            }
        }

        /// <summary>
        /// Logon name.
        /// </summary>
        public string LogonName
        {
            get
            {
                return this.logonName;
            }
        }

        /// <summary>
        /// Service name
        /// </summary>
        public string ServiceName
        {
            get
            {
                return this.serviceName;
            }
        }

        /// <summary>
        /// KDC IP address
        /// </summary>
        public IPAddress KdcIpAddress
        {
            get
            {
                return this.kdcIpAddress;
            }
        }

        /// <summary>
        /// Client security context attributes.
        /// </summary>
        [CLSCompliant(false)]
        public ClientSecurityContextAttribute SecurityAttributes
        {
            get
            {
                return this.securityAttributes;
            }
        }

        /// <summary>
        /// Connection type, e.g. TCP or UDP
        /// </summary>
        public KileConnectionType ConnectionType
        {
            get
            {
                return this.connectionType;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="account">Account credential.</param>
        /// <param name="logonName">Logon name.</param>
        /// <param name="serviceName">Service name.</param>
        /// <param name="kdcIpAddress">KDC IP address</param>
        /// <param name="attributes">Client security attributes.</param>
        /// <param name="connectionType">Connection type.</param>
        public KileClientSecurityConfig(
                AccountCredential account,
                string logonName,
                string serviceName,
                IPAddress kdcIpAddress,
                ClientSecurityContextAttribute attributes,
                KileConnectionType connectionType)
            : base(SecurityPackageType.Kerberos)
        {
            this.clientCredential = account;
            this.logonName = logonName;
            this.serviceName = serviceName;
            this.kdcIpAddress = kdcIpAddress;
            this.securityAttributes = attributes;
            this.connectionType = connectionType;
        }
    }
}
