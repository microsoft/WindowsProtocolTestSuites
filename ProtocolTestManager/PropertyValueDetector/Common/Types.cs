// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestManager.Detector.Common
{
    public enum IPVersion
    {
        IPv4,
        IPv6
    }

    public enum KerberosTrustType
    {
        NoTrust,
        Forest,
        Realm,
        Child
    }

    public class DomainInfo
    {
        public string Name;
        public string Admin;
        public string AdminPassword;
        public string FunctionalLevel;
    }
    public class ComputerInfo
    {
        public string ComputerName;
        public string FQDN;
        public string NetBIOS;
        public string IPv4;
        public string IPv6;
        public string Password;
        public string Port;
        public string DefaultServiceName;
        public string ServiceSalt;
        public bool IsWindows;
    }

    public class Smb2Service
    {
        public string SMB2ServiceName;
        public string SMB2Dialect;
        public string DACShare;
        public string CBACShare;
    }
    public class HttpService
    {
        public string HttpServiceName;
        public string Uri;
        public string Port;
    }
    public class LDAPService
    {
        public string LdapServiceName;
        public string Port;
        public string GssToken;
    }

    public class OtherService
    {
        public string FQDN;
        public string NetBios;
        public string Password;
        public string ServiceSalt;
        public string DefaultServiceName;
    }
    public class KkdcpInfo
    {
        public string KKDCPServerUrl = string.Empty;
        public string KKDCPClientCertPath = string.Empty;
        public string KKDCPClientCertPassword = string.Empty;
    }
    public class Server : ComputerInfo
    {
        public Smb2Service smb2Service;
        public HttpService httpService;
        public LDAPService ldapService;
        public OtherService authNotReqService;
        public OtherService localResourceService1;
        public OtherService localResourceService2;
    }

    public class User
    {
        public string Name;
        public string Password;
        public string Domain;
        public string Salt;
        public string ServiceName;

        public User(string name, string password, string domain, string salt, string servicename)
        {
            Name = name;
            Password = password;
            Domain = domain;
            Salt = salt;
            ServiceName = servicename;
        }

    }

    /// <summary>
    /// Platform type of the SUT
    /// </summary>
    public enum Platform
    {
        /// <summary>
        /// Non Windows implementation
        /// </summary>
        NonWindows = 0x00000000,

        /// <summary>
        /// Windows Server 2008
        /// </summary>
        WindowsServer2008 = 0x10000002,

        /// <summary>
        /// Windows Server 2008 R2
        /// </summary>
        WindowsServer2008R2 = 0x10000004,

        /// <summary>
        /// Windows Server 2012
        /// </summary>
        WindowsServer2012 = 0x10000006,

        /// <summary>
        /// Windows Server 2012 R2
        /// </summary>
        WindowsServer2012R2 = 0x10000007,

        /// <summary>
        /// Windows Server 2016
        /// </summary>
        WindowsServer2016 = 0x10000008,
    }
}
