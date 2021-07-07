// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib;
using System;
using System.Net;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    public class KerberosClientSecurityConfig : SecurityConfig
    {
        private int kdcPort = 88;

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
        private TransportType transportType;


        /// <summary>
        /// Client account credential
        /// </summary>
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

        public int KdcPort
        {
            get
            {
                return this.kdcPort;
            }
        }

        /// <summary>
        /// Client security context attributes.
        /// </summary>
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
        public TransportType TransportType
        {
            get
            {
                return this.transportType;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="account">Account credential.</param>
        /// <param name="logonName">Logon name.</param>
        /// <param name="serviceName">Service name.</param>
        /// <param name="kdcIpAddress">KDC IP address</param>
        /// <param name="kdcPort">KDC Port</param>
        /// <param name="attributes">Client security attributes.</param>
        /// <param name="connectionType">Connection type.</param>
        public KerberosClientSecurityConfig(
                AccountCredential account,
                string logonName,
                string serviceName,
                IPAddress kdcIpAddress,
                int kdcPort,
                ClientSecurityContextAttribute attributes,
                TransportType transportType)
            : base(SecurityPackageType.Kerberos)
        {
            this.clientCredential = account;
            this.logonName = logonName;
            this.serviceName = serviceName;
            this.kdcIpAddress = kdcIpAddress;
            this.kdcPort = kdcPort;
            this.securityAttributes = attributes;
            this.transportType = transportType;
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
        public KerberosClientSecurityConfig(
                AccountCredential account,
                string serviceName,
                ClientSecurityContextAttribute attributes)
            : base(SecurityPackageType.Kerberos)
        {
            this.clientCredential = account;
            this.logonName = account.AccountName;
            this.serviceName = serviceName;
            this.kdcIpAddress = account.DomainName.ParseIPAddress();
            this.securityAttributes = attributes;
            this.transportType = TransportType.TCP;
        }
    }
}