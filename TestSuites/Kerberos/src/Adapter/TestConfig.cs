// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;

namespace Microsoft.Protocol.TestSuites.Kerberos.Adapter
{
    public class Computer
    {
        public string FQDN { get; set; }
        public string NetBiosName { get; set; }
        public string Password { get; set; }
        public string AccountSalt { get; set; }
        public string IPAddress { get; set; }
        internal string Port_Str { get; set; }
        public int Port
        {
            get
            {
                int value;
                if (!int.TryParse(this.Port_Str, out value))
                {
                    TestClassBase.BaseTestSite.Assume.Fail("Value {0} for Port cannot be parsed.", this.Port_Str);
                }
                return value;
            }
            set
            {
                this.Port_Str = value.ToString();
            }
        }
        public string DefaultServiceName { get; set; }
        public string ServiceSalt { get; set; }
        internal string IsWindows_Str { get; set; }
        public bool IsWindows
        {
            get
            {
                bool value;
                if (!bool.TryParse(this.IsWindows_Str, out value))
                {
                    TestClassBase.BaseTestSite.Assume.Fail("Value {0} for IsWindows cannot be parsed.", this.IsWindows_Str);
                }
                return value;
            }
            set
            {
                this.IsWindows_Str = value.ToString();
            }
        }

        public Computer() { }
    }

    public class ApplicationServer : Computer
    {
        internal string GssToken_Str { get; set; }
        public KerberosConstValue.GSSToken GssToken
        {
            get
            {
                KerberosConstValue.GSSToken value;
                if (!Enum.TryParse<KerberosConstValue.GSSToken>(this.GssToken_Str, out value))
                {
                    TestClassBase.BaseTestSite.Assume.Fail("Value {0} for GssToken cannot be parsed.", this.GssToken_Str);
                }
                return value;
            }
            set
            {
                this.GssToken_Str = value.ToString();
            }
        }
    }
    public class DomainController : Computer
    {
        //public string DomainControllerFunctionality { get; set; }

        public DomainController() { }
    }

    public class FileServer : ApplicationServer
    {
        public string Smb2ServiceName { get; set; }
        public string Smb2Dialect { get; set; }
        public string DACShareFolder { get; set; }
        public string CBACShareFolder { get; set; }
        public FileServer() { }
    }

    public class WebServer : ApplicationServer
    {
        public string HttpServiceName { get; set; }
        public string HttpUri { get; set; }

        public WebServer() { }
    }

    public class LdapServer : ApplicationServer
    {
        public string LdapServiceName { get; set; }
        internal string LdapPort_Str { get; set; }
        public int LdapPort
        {
            get
            {
                int value;
                if (!int.TryParse(this.LdapPort_Str, out value))
                {
                    TestClassBase.BaseTestSite.Assume.Fail("Value {0} for LdapPort cannot be parsed.", this.LdapPort_Str);
                }
                return value;
            }
            set
            {
                this.LdapPort_Str = value.ToString();
            }
        }

        public LdapServer() { } 
    }

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string ServiceName { get; set; }
        public string TransformedClaims { get; set; }
        public DomainAccountInfo DomainAccountInfo;

        public User() { }
    }

    public class DomainAccountInfo
    {
        public string ScriptPath { get; set; }
        public string ProfilePath { get; set; }
        public string HomeDirectory { get; set; }
        public string HomeDrive { get; set; }
        internal string UserID_Str { get; set; }
        public uint UserID
        {
            get
            {
                uint value;
                if (!uint.TryParse(this.UserID_Str, out value))
                {
                    TestClassBase.BaseTestSite.Assume.Fail("Value {0} for UserID cannot be parsed.", this.UserID_Str);
                }
                return value;
            }
            set
            {
                this.UserID_Str = value.ToString();
            }
        }
        internal string PrimaryGroupID_Str { get; set; }
        public uint PrimaryGroupId
        {
            get
            {
                uint value;
                if (!uint.TryParse(this.PrimaryGroupID_Str, out value))
                {
                    TestClassBase.BaseTestSite.Assume.Fail("Value {0} for PrimaryGroupId cannot be parsed.", this.PrimaryGroupID_Str);
                }
                return value;
            }
            set
            {
                this.PrimaryGroupID_Str = value.ToString();
            }
        }
        internal string UserAccountControl_Str { get; set; }
        public uint UserAccountControl
        {
            get
            {
                uint value;
                if (!uint.TryParse(this.UserAccountControl_Str, out value))
                {
                    TestClassBase.BaseTestSite.Assume.Fail("Value {0} for UserAccountControl cannot be parsed.", this.UserAccountControl_Str);
                }
                return value;
            }
            set
            {
                this.UserAccountControl_Str = value.ToString();
            }
        }
        internal string GroupCount_Str { get; set; }
        public uint GroupCount
        {
            get
            {
                uint value;
                if (!uint.TryParse(this.GroupCount_Str, out value))
                {
                    TestClassBase.BaseTestSite.Assume.Fail("Value {0} for GroupCount cannot be parsed.", this.GroupCount_Str);
                }
                return value;
            }
            set
            {
                this.GroupCount_Str = value.ToString();
            }
        }

        public DomainAccountInfo() { }
    }

    public class Group
    {
        public string GroupName { get; set; }
        public Group() { }
    }

    public class Realm
    {
        public string RealmName { get; set; }
        public string DomainControllerFunctionality { get; set; }   
        public DomainController[] KDC;
        public Computer ClientComputer;
        public Computer AuthNotRequired;
        public Computer[] LocalResources;
        public FileServer[] FileServer;
        public WebServer[] WebServer;
        public LdapServer[] LdapServer;
        public User[] User;
        public User Admin;
        public ResourceGroups ResourceGroups;
        
        public Realm() { }
    }

    public class ResourceGroups
    {
        internal string GroupCount_Str { get; set; }
        public uint GroupCount
        {
            get
            {
                uint value;
                if (!uint.TryParse(this.GroupCount_Str, out value))
                {
                    TestClassBase.BaseTestSite.Assume.Fail("Value {0} for GroupCount cannot be parsed.", this.GroupCount_Str);
                }
                return value;
            }
            set
            {
                this.GroupCount_Str = value.ToString();
            }
        }
        public Group[] Groups;

    }

    public enum TrustType
    {
        Forest,
        Realm,
        NoTrust,
        Child
    }

    public class TestConfig
    {
        public ITestSite TestSite { get; set; }

        public TransportType TransportType { get; set; }
        internal string TransportBufferSize_Str { get; set; }
        public int TransportBufferSize
        {
            get
            {
                int value;
                if (!int.TryParse(this.TransportBufferSize_Str, out value))
                {
                    TestClassBase.BaseTestSite.Assume.Fail("Value {0} for TransportBufferSize cannot be parsed.", this.TransportBufferSize_Str);
                }
                return value;
            }
            set
            {
                this.TransportBufferSize_Str = value.ToString();
            }
        }
        internal string SupportedOid_Str { get; set; }
        public KerberosConstValue.OidPkt SupportedOid
        {
            get
            {
                KerberosConstValue.OidPkt value;
                if (!Enum.TryParse<KerberosConstValue.OidPkt>(this.SupportedOid_Str, out value))
                {
                    TestClassBase.BaseTestSite.Assume.Fail("Value {0} for SupportedOid cannot be parsed.", this.SupportedOid_Str);
                }
                return value;
            }
            set
            {
                this.SupportedOid_Str = value.ToString();
            }
        }
        internal string IsKileImplemented_Str { get; set; }
        public bool IsKileImplemented
        {
            get
            {
                bool value;
                if (!bool.TryParse(this.IsKileImplemented_Str, out value))
                {
                    TestClassBase.BaseTestSite.Assume.Fail("Value {0} for IsKileImplemented cannot be parsed.", this.IsKileImplemented_Str);
                }
                return value;
            }
            set
            {
                this.IsKileImplemented_Str = value.ToString();
            }
        }
        internal string IsClaimSupported_Str { get; set; }
        public bool IsClaimSupported
        {
            get
            {
                bool value;
                if (!bool.TryParse(this.IsClaimSupported_Str, out value))
                {
                    TestClassBase.BaseTestSite.Assume.Fail("Value {0} for IsClaimSupported cannot be parsed.", this.IsClaimSupported_Str);
                }
                return value;
            }
            set
            {
                this.IsClaimSupported_Str = value.ToString();
            }
        }
        internal string TrustType_Str { get; set; }
        public TrustType TrustType
        {
            get
            {
                TrustType value;
                if (!Enum.TryParse<TrustType>(this.TrustType_Str, out value))
                {
                    TestClassBase.BaseTestSite.Assume.Fail("Value {0} for TrustType cannot be parsed.", this.TrustType_Str);
                }
                return value;
            }
            set
            {
                this.TrustType_Str = value.ToString();
            }
        }

        internal string UseProxy_Str { get; set; }
        public bool UseProxy
        {
            get
            {
                bool value;
                if (!bool.TryParse(this.UseProxy_Str, out value))
                {
                    TestClassBase.BaseTestSite.Assume.Fail("Value {0} for UseProxy cannot be parsed.", this.UseProxy_Str);
                }
                return value;
            }
            set
            {
                this.UseProxy_Str = value.ToString();
            }
        }
        public string KKDCPServerUrl { get; set; }
        public string KKDCPClientCertPath { get; set; }
        public string KKDCPClientCertPassword { get; set; }

        public Realm LocalRealm;
        public Realm TrustedRealm;

        protected KeyManager keytab;

        public TestConfig()
        {
            this.TestSite = TestClassBase.BaseTestSite;
            //Common properties
            string type = this.TestSite.Properties.Get("TransportType");
            if (type.ToLower() == "tcp")
            {
                this.TransportType = TransportType.TCP;
            }
            else if (type.ToLower() == "udp")
            {
                this.TransportType = TransportType.UDP;
            }
            this.TransportBufferSize_Str = this.TestSite.Properties.Get("TransportBufferSize");
            string ipversion = this.TestSite.Properties.Get("IpVersion");
            this.TrustType_Str = this.TestSite.Properties.Get("TrustType");
            this.SupportedOid_Str = this.TestSite.Properties.Get("SupportedOid");

            this.IsKileImplemented_Str = this.TestSite.Properties.Get("IsKileImplemented");
            this.IsClaimSupported_Str = this.TestSite.Properties.Get("IsClaimSupported");

            //KKDCP configs
            this.UseProxy_Str = this.TestSite.Properties.Get("UseProxy");
            this.KKDCPServerUrl = this.TestSite.Properties.Get("KKDCPServerUrl");
            this.KKDCPClientCertPath = this.TestSite.Properties.Get("KKDCPClientCertPath");
            this.KKDCPClientCertPassword = this.TestSite.Properties.Get("KKDCPClientCertPassword");

            //Local realm common properties
            this.LocalRealm = new Realm();
            this.LocalRealm.RealmName = this.TestSite.Properties.Get("LocalRealm.RealmName");
            this.LocalRealm.DomainControllerFunctionality = this.TestSite.Properties.Get("LocalRealm.DomainControllerFunctionality");

            //Local realm kdc
            this.LocalRealm.KDC = new DomainController[1];
            this.LocalRealm.KDC[0] = new DomainController();
            this.LocalRealm.KDC[0].IsWindows_Str = this.TestSite.Properties.Get("LocalRealm.KDC01.IsWindows");
            this.LocalRealm.KDC[0].FQDN = this.TestSite.Properties.Get("LocalRealm.KDC01.FQDN");
            this.LocalRealm.KDC[0].NetBiosName = this.TestSite.Properties.Get("LocalRealm.KDC01.NetBiosName");
            this.LocalRealm.KDC[0].Password = this.TestSite.Properties.Get("LocalRealm.KDC01.Password");
            if (ipversion.ToLower() == "ipv4")
            {
                this.LocalRealm.KDC[0].IPAddress = this.TestSite.Properties.Get("LocalRealm.KDC01.IPv4Address");
            }
            else if (ipversion.ToLower() == "ipv6")
            {
                this.LocalRealm.KDC[0].IPAddress = this.TestSite.Properties.Get("LocalRealm.KDC01.IPv6Address");
            }
            else
            {
                this.LocalRealm.KDC[0].IPAddress = null;
            }
            this.LocalRealm.KDC[0].Port_Str = this.TestSite.Properties.Get("LocalRealm.KDC01.Port");

            //Local realm client computer
            this.LocalRealm.ClientComputer = new Computer();
            this.LocalRealm.ClientComputer.FQDN = this.TestSite.Properties.Get("LocalRealm.ClientComputer.FQDN");
            this.LocalRealm.ClientComputer.NetBiosName = this.TestSite.Properties.Get("LocalRealm.ClientComputer.NetBiosName");
            this.LocalRealm.ClientComputer.Password = this.TestSite.Properties.Get("LocalRealm.ClientComputer.Password");
            if (ipversion.ToLower() == "ipv4")
            {
                this.LocalRealm.ClientComputer.IPAddress = this.TestSite.Properties.Get("LocalRealm.ClientComputer.IPv4Address");
            }
            else if (ipversion.ToLower() == "ipv6")
            {
                this.LocalRealm.ClientComputer.IPAddress = this.TestSite.Properties.Get("LocalRealm.ClientComputer.IPv6Address");
            }
            else
            {
                this.LocalRealm.KDC[0].IPAddress = null;
            }
            this.LocalRealm.ClientComputer.Port_Str = this.TestSite.Properties.Get("LocalRealm.ClientComputer.Port");
            this.LocalRealm.ClientComputer.DefaultServiceName = this.TestSite.Properties.Get("LocalRealm.ClientComputer.DefaultServiceName");
            this.LocalRealm.ClientComputer.ServiceSalt = this.TestSite.Properties.Get("LocalRealm.ClientComputer.ServiceSalt");
            this.LocalRealm.ClientComputer.AccountSalt = this.TestSite.Properties.Get("LocalRealm.ClientComputer.AccountSalt");

            //Local realm File Server
            this.LocalRealm.FileServer = new FileServer[1];
            this.LocalRealm.FileServer[0] = new FileServer();
            this.LocalRealm.FileServer[0].FQDN = this.TestSite.Properties.Get("LocalRealm.FileServer01.FQDN");
            this.LocalRealm.FileServer[0].NetBiosName = this.TestSite.Properties.Get("LocalRealm.FileServer01.NetBiosName");
            this.LocalRealm.FileServer[0].Password = this.TestSite.Properties.Get("LocalRealm.FileServer01.Password");
            if (ipversion.ToLower() == "ipv4")
            {
                this.LocalRealm.FileServer[0].IPAddress = this.TestSite.Properties.Get("LocalRealm.FileServer01.IPv4Address");
            }
            else if (ipversion.ToLower() == "ipv6")
            {
                this.LocalRealm.FileServer[0].IPAddress = this.TestSite.Properties.Get("LocalRealm.FileServer01.IPv6Address");
            }
            else
            {
                this.LocalRealm.FileServer[0].IPAddress = null;
            }
            this.LocalRealm.FileServer[0].DefaultServiceName = this.TestSite.Properties.Get("LocalRealm.FileServer01.DefaultServiceName");
            this.LocalRealm.FileServer[0].ServiceSalt = this.TestSite.Properties.Get("LocalRealm.FileServer01.ServiceSalt");
            this.LocalRealm.FileServer[0].Smb2ServiceName = this.TestSite.Properties.Get("LocalRealm.FileServer01.Smb2ServiceName");
            this.LocalRealm.FileServer[0].Smb2Dialect = this.TestSite.Properties.Get("LocalRealm.FileServer01.Smb2Dialect");
            this.LocalRealm.FileServer[0].DACShareFolder = this.TestSite.Properties.Get("LocalRealm.FileServer01.DACShareFolder");
            this.LocalRealm.FileServer[0].CBACShareFolder = this.TestSite.Properties.Get("LocalRealm.FileServer01.CBACShareFolder");

            //Local realm ldap server
            this.LocalRealm.LdapServer = new LdapServer[1];
            this.LocalRealm.LdapServer[0] = new LdapServer();
            this.LocalRealm.LdapServer[0].FQDN = this.TestSite.Properties.Get("LocalRealm.LdapServer01.FQDN");
            this.LocalRealm.LdapServer[0].NetBiosName = this.TestSite.Properties.Get("LocalRealm.LdapServer01.NetBiosName");
            this.LocalRealm.LdapServer[0].Password = this.TestSite.Properties.Get("LocalRealm.LdapServer01.Password");
            if (ipversion.ToLower() == "ipv4")
            {
                this.LocalRealm.LdapServer[0].IPAddress = this.TestSite.Properties.Get("LocalRealm.LdapServer01.IPv4Address");
            }
            else if (ipversion.ToLower() == "ipv6")
            {
                this.LocalRealm.LdapServer[0].IPAddress = this.TestSite.Properties.Get("LocalRealm.LdapServer01.IPv6Address");
            }
            else
            {
                this.LocalRealm.LdapServer[0].IPAddress = null;
            }
            this.LocalRealm.LdapServer[0].DefaultServiceName = this.TestSite.Properties.Get("LocalRealm.LdapServer01.DefaultServiceName");
            this.LocalRealm.LdapServer[0].ServiceSalt = this.TestSite.Properties.Get("LocalRealm.LdapServer01.ServiceSalt");
            this.LocalRealm.LdapServer[0].LdapServiceName = this.TestSite.Properties.Get("LocalRealm.LdapServer01.LdapServiceName");
            this.LocalRealm.LdapServer[0].LdapPort_Str = this.TestSite.Properties.Get("LocalRealm.LdapServer01.LdapPort");
            this.LocalRealm.LdapServer[0].GssToken_Str = this.TestSite.Properties.Get("LocalRealm.LdapServer01.GssToken");

            this.LocalRealm.WebServer = new WebServer[1];
            this.LocalRealm.WebServer[0] = new WebServer();
            this.LocalRealm.WebServer[0].FQDN = this.TestSite.Properties.Get("LocalRealm.WebServer01.FQDN");
            this.LocalRealm.WebServer[0].NetBiosName = this.TestSite.Properties.Get("LocalRealm.WebServer01.NetBiosName");
            this.LocalRealm.WebServer[0].Password = this.TestSite.Properties.Get("LocalRealm.WebServer01.Password");
            if (ipversion.ToLower() == "ipv4")
            {
                this.LocalRealm.WebServer[0].IPAddress = this.TestSite.Properties.Get("LocalRealm.WebServer01.IPv4Address");
            }
            else if (ipversion.ToLower() == "ipv6")
            {
                this.LocalRealm.WebServer[0].IPAddress = this.TestSite.Properties.Get("LocalRealm.WebServer01.IPv6Address");
            }
            else
            {
                this.LocalRealm.WebServer[0].IPAddress = null;
            }
            this.LocalRealm.WebServer[0].DefaultServiceName = this.TestSite.Properties.Get("LocalRealm.WebServer01.DefaultServiceName");
            this.LocalRealm.WebServer[0].ServiceSalt = this.TestSite.Properties.Get("LocalRealm.WebServer01.ServiceSalt");
            this.LocalRealm.WebServer[0].HttpServiceName = this.TestSite.Properties.Get("LocalRealm.WebServer01.HttpServiceName");
            this.LocalRealm.WebServer[0].HttpUri = this.TestSite.Properties.Get("LocalRealm.WebServer01.HttpUri");

            //authnotrequired
            this.LocalRealm.AuthNotRequired = new Computer();
            this.LocalRealm.AuthNotRequired.FQDN = this.TestSite.Properties.Get("LocalRealm.AuthNotRequired.FQDN");
            this.LocalRealm.AuthNotRequired.NetBiosName = this.TestSite.Properties.Get("LocalRealm.AuthNotRequired.NetBiosName");
            this.LocalRealm.AuthNotRequired.Password = this.TestSite.Properties.Get("LocalRealm.AuthNotRequired.Password");
            this.LocalRealm.AuthNotRequired.DefaultServiceName = this.TestSite.Properties.Get("LocalRealm.AuthNotRequired.DefaultServiceName");
            this.LocalRealm.AuthNotRequired.ServiceSalt = this.TestSite.Properties.Get("LocalRealm.AuthNotRequired.ServiceSalt");

            this.LocalRealm.LocalResources = new Computer[2];
            this.LocalRealm.LocalResources[0] = new Computer();
            this.LocalRealm.LocalResources[0].FQDN = this.TestSite.Properties.Get("LocalRealm.localResource01.FQDN");
            this.LocalRealm.LocalResources[0].NetBiosName = this.TestSite.Properties.Get("LocalRealm.localResource01.NetBiosName");
            this.LocalRealm.LocalResources[0].Password = this.TestSite.Properties.Get("LocalRealm.localResource01.Password");
            this.LocalRealm.LocalResources[0].DefaultServiceName = this.TestSite.Properties.Get("LocalRealm.localResource01.DefaultServiceName");
            this.LocalRealm.LocalResources[0].ServiceSalt = this.TestSite.Properties.Get("LocalRealm.localResource01.ServiceSalt");
            this.LocalRealm.LocalResources[1] = new Computer();
            this.LocalRealm.LocalResources[1].FQDN = this.TestSite.Properties.Get("LocalRealm.localResource02.FQDN");
            this.LocalRealm.LocalResources[1].NetBiosName = this.TestSite.Properties.Get("LocalRealm.localResource02.NetBiosName");
            this.LocalRealm.LocalResources[1].Password = this.TestSite.Properties.Get("LocalRealm.localResource02.Password");
            this.LocalRealm.LocalResources[1].DefaultServiceName = this.TestSite.Properties.Get("LocalRealm.localResource02.DefaultServiceName");
            this.LocalRealm.LocalResources[1].ServiceSalt = this.TestSite.Properties.Get("LocalRealm.localResource02.ServiceSalt");

            //ResourceGroups
            this.LocalRealm.ResourceGroups = new ResourceGroups();
            this.LocalRealm.ResourceGroups.GroupCount_Str = this.TestSite.Properties.Get("LocalRealm.resourceGroups.resourceGroupCount");
            if (this.LocalRealm.ResourceGroups.GroupCount > 0)
            {
                this.LocalRealm.ResourceGroups.Groups = new Group[this.LocalRealm.ResourceGroups.GroupCount];
                this.LocalRealm.ResourceGroups.Groups[0] = new Group();
                this.LocalRealm.ResourceGroups.Groups[0].GroupName = this.TestSite.Properties.Get("LocalRealm.resourceGroups.resourceGroup01.GroupName");
                this.LocalRealm.ResourceGroups.Groups[1] = new Group();
                this.LocalRealm.ResourceGroups.Groups[1].GroupName = this.TestSite.Properties.Get("LocalRealm.resourceGroups.resourceGroup02.GroupName");
            }
            
            //Local realm users
            this.LocalRealm.Admin = new User();
            this.LocalRealm.Admin.Username = this.TestSite.Properties.Get("LocalRealm.Users.Admin.Username");
            this.LocalRealm.Admin.Password = this.TestSite.Properties.Get("LocalRealm.Users.Admin.Password");

            this.LocalRealm.User = new User[23];
            this.LocalRealm.User[0] = new User();
            this.LocalRealm.User[0].Username = this.TestSite.Properties.Get("LocalRealm.Users.Krbtgt.Username");
            this.LocalRealm.User[0].Password = this.TestSite.Properties.Get("LocalRealm.Users.Krbtgt.Password");
            this.LocalRealm.User[1] = new User();
            this.LocalRealm.User[1].Username = this.TestSite.Properties.Get("LocalRealm.Users.User01.Username");
            this.LocalRealm.User[1].Password = this.TestSite.Properties.Get("LocalRealm.Users.User01.Password");
            this.LocalRealm.User[1].Salt = this.TestSite.Properties.Get("LocalRealm.Users.User01.Salt");

            this.LocalRealm.User[1].DomainAccountInfo = new DomainAccountInfo();
            this.LocalRealm.User[1].DomainAccountInfo.ScriptPath = this.TestSite.Properties.Get("LocalRealm.Users.User01.DomainAccountInfo.ScriptPath");
            this.LocalRealm.User[1].DomainAccountInfo.ProfilePath = this.TestSite.Properties.Get("LocalRealm.Users.User01.DomainAccountInfo.ProfilePath");
            this.LocalRealm.User[1].DomainAccountInfo.HomeDirectory = this.TestSite.Properties.Get("LocalRealm.Users.User01.DomainAccountInfo.HomeDirectory");
            this.LocalRealm.User[1].DomainAccountInfo.HomeDrive = this.TestSite.Properties.Get("LocalRealm.Users.User01.DomainAccountInfo.HomeDrive");
            this.LocalRealm.User[1].DomainAccountInfo.UserID_Str = this.TestSite.Properties.Get("LocalRealm.Users.User01.DomainAccountInfo.UserID");
            this.LocalRealm.User[1].DomainAccountInfo.PrimaryGroupID_Str = this.TestSite.Properties.Get("LocalRealm.Users.User01.DomainAccountInfo.PrimaryGroupId");
            this.LocalRealm.User[1].DomainAccountInfo.UserAccountControl_Str = this.TestSite.Properties.Get("LocalRealm.Users.User01.DomainAccountInfo.UserAccountControl");
            this.LocalRealm.User[1].DomainAccountInfo.GroupCount_Str = this.TestSite.Properties.Get("LocalRealm.Users.User01.DomainAccountInfo.GroupCount");

            this.LocalRealm.User[2] = new User();
            this.LocalRealm.User[2].Username = this.TestSite.Properties.Get("LocalRealm.Users.User02.Username");
            this.LocalRealm.User[2].Password = this.TestSite.Properties.Get("LocalRealm.Users.User02.Password");
            this.LocalRealm.User[2].TransformedClaims = this.TestSite.Properties.Get("LocalRealm.Users.User02.TransformedClaims");
            this.LocalRealm.User[3] = new User();
            this.LocalRealm.User[3].Username = this.TestSite.Properties.Get("LocalRealm.Users.User03.Username");
            this.LocalRealm.User[3].Password = this.TestSite.Properties.Get("LocalRealm.Users.User03.Password");
            this.LocalRealm.User[4] = new User();
            this.LocalRealm.User[4].Username = this.TestSite.Properties.Get("LocalRealm.Users.User04.Username");
            this.LocalRealm.User[4].Password = this.TestSite.Properties.Get("LocalRealm.Users.User04.Password");
            this.LocalRealm.User[4].ServiceName = this.TestSite.Properties.Get("LocalRealm.Users.User04.ServiceName");
            this.LocalRealm.User[5] = new User();
            this.LocalRealm.User[5].Username = this.TestSite.Properties.Get("LocalRealm.Users.User05.Username");
            this.LocalRealm.User[5].Password = this.TestSite.Properties.Get("LocalRealm.Users.User05.Password");
            this.LocalRealm.User[6] = new User();
            this.LocalRealm.User[6].Username = this.TestSite.Properties.Get("LocalRealm.Users.User06.Username");
            this.LocalRealm.User[6].Password = this.TestSite.Properties.Get("LocalRealm.Users.User06.Password");
            this.LocalRealm.User[7] = new User();
            this.LocalRealm.User[7].Username = this.TestSite.Properties.Get("LocalRealm.Users.User07.Username");
            this.LocalRealm.User[7].Password = this.TestSite.Properties.Get("LocalRealm.Users.User07.Password");
            this.LocalRealm.User[8] = new User();
            this.LocalRealm.User[8].Username = this.TestSite.Properties.Get("LocalRealm.Users.User08.Username");
            this.LocalRealm.User[8].Password = this.TestSite.Properties.Get("LocalRealm.Users.User08.Password");
            this.LocalRealm.User[9] = new User();
            this.LocalRealm.User[9].Username = this.TestSite.Properties.Get("LocalRealm.Users.User09.Username");
            this.LocalRealm.User[9].Password = this.TestSite.Properties.Get("LocalRealm.Users.User09.Password");
            this.LocalRealm.User[10] = new User();
            this.LocalRealm.User[10].Username = this.TestSite.Properties.Get("LocalRealm.Users.User10.Username");
            this.LocalRealm.User[10].Password = this.TestSite.Properties.Get("LocalRealm.Users.User10.Password");
            this.LocalRealm.User[11] = new User();
            this.LocalRealm.User[11].Username = this.TestSite.Properties.Get("LocalRealm.Users.User11.Username");
            this.LocalRealm.User[11].Password = this.TestSite.Properties.Get("LocalRealm.Users.User11.Password");
            this.LocalRealm.User[12] = new User();
            this.LocalRealm.User[12].Username = this.TestSite.Properties.Get("LocalRealm.Users.User12.Username");
            this.LocalRealm.User[12].Password = this.TestSite.Properties.Get("LocalRealm.Users.User12.Password");
            this.LocalRealm.User[13] = new User();
            this.LocalRealm.User[13].Username = this.TestSite.Properties.Get("LocalRealm.Users.User13.Username");
            this.LocalRealm.User[13].Password = this.TestSite.Properties.Get("LocalRealm.Users.User13.Password");
            this.LocalRealm.User[14] = new User();
            this.LocalRealm.User[14].Username = this.TestSite.Properties.Get("LocalRealm.Users.User14.Username");
            this.LocalRealm.User[14].Password = this.TestSite.Properties.Get("LocalRealm.Users.User14.Password");
            this.LocalRealm.User[15] = new User();
            this.LocalRealm.User[15].Username = this.TestSite.Properties.Get("LocalRealm.Users.User15.Username");
            this.LocalRealm.User[15].Password = this.TestSite.Properties.Get("LocalRealm.Users.User15.Password");
            this.LocalRealm.User[16] = new User();
            this.LocalRealm.User[16].Username = this.TestSite.Properties.Get("LocalRealm.Users.User16.Username");
            this.LocalRealm.User[16].Password = this.TestSite.Properties.Get("LocalRealm.Users.User16.Password");
            this.LocalRealm.User[17] = new User();
            this.LocalRealm.User[17].Username = this.TestSite.Properties.Get("LocalRealm.Users.User17.Username");
            this.LocalRealm.User[17].Password = this.TestSite.Properties.Get("LocalRealm.Users.User17.Password");
            this.LocalRealm.User[18] = new User();
            this.LocalRealm.User[18].Username = this.TestSite.Properties.Get("LocalRealm.Users.User18.Username");
            this.LocalRealm.User[18].Password = this.TestSite.Properties.Get("LocalRealm.Users.User18.Password");
            this.LocalRealm.User[19] = new User();
            this.LocalRealm.User[19].Username = this.TestSite.Properties.Get("LocalRealm.Users.User19.Username");
            this.LocalRealm.User[19].Password = this.TestSite.Properties.Get("LocalRealm.Users.User19.Password");
            this.LocalRealm.User[20] = new User();
            this.LocalRealm.User[20].Username = this.TestSite.Properties.Get("LocalRealm.Users.User20.Username");
            this.LocalRealm.User[20].Password = this.TestSite.Properties.Get("LocalRealm.Users.User20.Password");
            this.LocalRealm.User[21] = new User();
            this.LocalRealm.User[21].Username = this.TestSite.Properties.Get("LocalRealm.Users.User21.Username");
            this.LocalRealm.User[21].Password = this.TestSite.Properties.Get("LocalRealm.Users.User21.Password");
            this.LocalRealm.User[22] = new User();
            this.LocalRealm.User[22].Username = this.TestSite.Properties.Get("LocalRealm.Users.User22.Username");
            this.LocalRealm.User[22].Password = this.TestSite.Properties.Get("LocalRealm.Users.User22.Password");
            
            this.TrustedRealm = new Realm();
            this.TrustedRealm.RealmName = this.TestSite.Properties.Get("TrustedRealm.RealmName");
            this.TrustedRealm.DomainControllerFunctionality = this.TestSite.Properties.Get("TrustedRealm.DomainControllerFunctionality");            

            this.TrustedRealm.KDC = new DomainController[1];
            this.TrustedRealm.KDC[0] = new DomainController();
            this.TrustedRealm.KDC[0].IsWindows_Str = this.TestSite.Properties.Get("TrustedRealm.KDC01.IsWindows");            
            this.TrustedRealm.KDC[0].FQDN = this.TestSite.Properties.Get("TrustedRealm.KDC01.FQDN");
            this.TrustedRealm.KDC[0].NetBiosName = this.TestSite.Properties.Get("TrustedRealm.KDC01.NetBiosName");
            this.TrustedRealm.KDC[0].Password = this.TestSite.Properties.Get("TrustedRealm.KDC01.Password");
            if (ipversion.ToLower() == "ipv4")
            {
                this.TrustedRealm.KDC[0].IPAddress = this.TestSite.Properties.Get("TrustedRealm.KDC01.IPv4Address");
            }
            else if (ipversion.ToLower() == "ipv6")
            {
                this.TrustedRealm.KDC[0].IPAddress = this.TestSite.Properties.Get("TrustedRealm.KDC01.IPv6Address");
            }
            else
            {
                this.TrustedRealm.KDC[0].IPAddress = null;
            }
            this.TrustedRealm.KDC[0].Port_Str = this.TestSite.Properties.Get("TrustedRealm.KDC01.Port");
            this.TrustedRealm.KDC[0].DefaultServiceName = this.TestSite.Properties.Get("TrustedRealm.KDC01.DefaultServiceName");

            this.TrustedRealm.FileServer = new FileServer[1];
            this.TrustedRealm.FileServer[0] = new FileServer();
            this.TrustedRealm.FileServer[0].FQDN = this.TestSite.Properties.Get("TrustedRealm.FileServer01.FQDN");
            this.TrustedRealm.FileServer[0].NetBiosName = this.TestSite.Properties.Get("TrustedRealm.FileServer01.NetBiosName");
            this.TrustedRealm.FileServer[0].Password = this.TestSite.Properties.Get("TrustedRealm.FileServer01.Password");
            if (ipversion.ToLower() == "ipv4")
            {
                this.TrustedRealm.FileServer[0].IPAddress = this.TestSite.Properties.Get("TrustedRealm.FileServer01.IPv4Address");
            }
            else if (ipversion.ToLower() == "ipv6")
            {
                this.TrustedRealm.FileServer[0].IPAddress = this.TestSite.Properties.Get("TrustedRealm.FileServer01.IPv6Address");
            }
            else
            {
                this.TrustedRealm.FileServer[0].IPAddress = null;
            }            
            this.TrustedRealm.FileServer[0].DefaultServiceName = this.TestSite.Properties.Get("TrustedRealm.FileServer01.DefaultServiceName");
            this.TrustedRealm.FileServer[0].ServiceSalt = this.TestSite.Properties.Get("TrustedRealm.FileServer01.ServiceSalt");
            this.TrustedRealm.FileServer[0].Smb2ServiceName = this.TestSite.Properties.Get("TrustedRealm.FileServer01.Smb2ServiceName");
            this.TrustedRealm.FileServer[0].Smb2Dialect = this.TestSite.Properties.Get("TrustedRealm.FileServer01.Smb2Dialect");
            this.TrustedRealm.FileServer[0].CBACShareFolder = this.TestSite.Properties.Get("TrustedRealm.FileServer01.CBACShareFolder");

            this.TrustedRealm.WebServer = new WebServer[1];
            this.TrustedRealm.WebServer[0] = new WebServer();
            this.TrustedRealm.WebServer[0].FQDN = this.TestSite.Properties.Get("TrustedRealm.WebServer01.FQDN");
            this.TrustedRealm.WebServer[0].NetBiosName = this.TestSite.Properties.Get("TrustedRealm.WebServer01.NetBiosName");
            this.TrustedRealm.WebServer[0].Password = this.TestSite.Properties.Get("TrustedRealm.WebServer01.Password");
            if (ipversion.ToLower() == "ipv4")
            {
                this.TrustedRealm.WebServer[0].IPAddress = this.TestSite.Properties.Get("TrustedRealm.WebServer01.IPv4Address");
            }
            else if (ipversion.ToLower() == "ipv6")
            {
                this.TrustedRealm.WebServer[0].IPAddress = this.TestSite.Properties.Get("TrustedRealm.WebServer01.IPv6Address");
            }
            else
            {
                this.TrustedRealm.WebServer[0].IPAddress = null;
            } 
            this.TrustedRealm.WebServer[0].DefaultServiceName = this.TestSite.Properties.Get("TrustedRealm.WebServer01.DefaultServiceName");
            this.TrustedRealm.WebServer[0].ServiceSalt = this.TestSite.Properties.Get("TrustedRealm.WebServer01.ServiceSalt");
            this.TrustedRealm.WebServer[0].HttpServiceName = this.TestSite.Properties.Get("TrustedRealm.WebServer01.HttpServiceName");
            this.TrustedRealm.WebServer[0].HttpUri = this.TestSite.Properties.Get("TrustedRealm.WebServer01.HttpUri");

            this.TrustedRealm.LdapServer = new LdapServer[1];
            this.TrustedRealm.LdapServer[0] = new LdapServer();
            this.TrustedRealm.LdapServer[0].FQDN = this.TestSite.Properties.Get("TrustedRealm.LdapServer01.FQDN");
            this.TrustedRealm.LdapServer[0].NetBiosName = this.TestSite.Properties.Get("TrustedRealm.LdapServer01.NetBiosName");
            this.TrustedRealm.LdapServer[0].Password = this.TestSite.Properties.Get("TrustedRealm.LdapServer01.Password");
            if (ipversion.ToLower() == "ipv4")
            {
                this.TrustedRealm.LdapServer[0].IPAddress = this.TestSite.Properties.Get("TrustedRealm.LdapServer01.IPv4Address");
            }
            else if (ipversion.ToLower() == "ipv6")
            {
                this.TrustedRealm.LdapServer[0].IPAddress = this.TestSite.Properties.Get("TrustedRealm.LdapServer01.IPv6Address");
            }
            else
            {
                this.TrustedRealm.LdapServer[0].IPAddress = null;
            }
            this.TrustedRealm.LdapServer[0].DefaultServiceName = this.TestSite.Properties.Get("TrustedRealm.LdapServer01.DefaultServiceName");
            this.TrustedRealm.LdapServer[0].ServiceSalt = this.TestSite.Properties.Get("TrustedRealm.LdapServer01.ServiceSalt");
            this.TrustedRealm.LdapServer[0].LdapServiceName = this.TestSite.Properties.Get("TrustedRealm.LdapServer01.LdapServiceName");
            this.TrustedRealm.LdapServer[0].LdapPort_Str = this.TestSite.Properties.Get("TrustedRealm.LdapServer01.LdapPort");
            this.TrustedRealm.LdapServer[0].GssToken_Str = this.TestSite.Properties.Get("TrustedRealm.LdapServer01.GssToken");

            this.TrustedRealm.Admin = new User();
            this.TrustedRealm.Admin.Username = this.TestSite.Properties.Get("TrustedRealm.Users.Admin.Username");
            this.TrustedRealm.Admin.Password = this.TestSite.Properties.Get("TrustedRealm.Users.Admin.Password");

            this.TrustedRealm.User = new User[3];
            this.TrustedRealm.User[0] = new User();
            this.TrustedRealm.User[0].Username = this.TestSite.Properties.Get("TrustedRealm.Users.Krbtgt.Username");
            this.TrustedRealm.User[0].Password = this.TestSite.Properties.Get("TrustedRealm.Users.Krbtgt.Password");
            this.TrustedRealm.User[1] = new User();
            this.TrustedRealm.User[1].Username = this.TestSite.Properties.Get("TrustedRealm.Users.User01.Username");
            this.TrustedRealm.User[1].Password = this.TestSite.Properties.Get("TrustedRealm.Users.User01.Password");
            this.TrustedRealm.User[2] = new User();
            this.TrustedRealm.User[2].Username = this.TestSite.Properties.Get("TrustedRealm.Users.User02.Username");
            this.TrustedRealm.User[2].Password = this.TestSite.Properties.Get("TrustedRealm.Users.User02.Password");

            keytab = new KeyManager();

            string keytabDir = TestSite.Properties.Get("KeytabDirectory");

            if (!string.IsNullOrEmpty(keytabDir))
            {
                var files = Directory.EnumerateFiles(keytabDir, "*.keytab");
                foreach (var file in files)
                {
                    keytab.LoadKeytab(file);
                }
            }

            Adapter.Realm realm = LocalRealm;
            if (!keytab.CheckPrincipalExistence(realm.ClientComputer.DefaultServiceName, realm.RealmName))
                keytab.MakeKey(realm.ClientComputer.DefaultServiceName, realm.RealmName, realm.ClientComputer.Password, realm.ClientComputer.ServiceSalt);
            
            foreach (var server in realm.FileServer)
            {
                if (!keytab.CheckPrincipalExistence(server.Smb2ServiceName, realm.RealmName))
                    keytab.MakeKey(server.Smb2ServiceName, realm.RealmName, server.Password, server.ServiceSalt);
            }
            foreach (var server in realm.LdapServer)
            {
                if (!keytab.CheckPrincipalExistence(server.LdapServiceName, realm.RealmName))
                    keytab.MakeKey(server.LdapServiceName, realm.RealmName, server.Password, server.ServiceSalt);
            }
            foreach (var server in realm.WebServer)
            {
                if (!keytab.CheckPrincipalExistence(server.HttpServiceName, realm.RealmName))
                    keytab.MakeKey(server.HttpServiceName, realm.RealmName, server.Password, server.ServiceSalt);
            }
            realm = TrustedRealm;
            foreach (var server in realm.FileServer)
            {
                if (!keytab.CheckPrincipalExistence(server.Smb2ServiceName, realm.RealmName))
                    keytab.MakeKey(server.Smb2ServiceName, realm.RealmName, server.Password, server.ServiceSalt);
            }
            foreach (var server in realm.LdapServer)
            {
                if (!keytab.CheckPrincipalExistence(server.LdapServiceName, realm.RealmName))
                    keytab.MakeKey(server.LdapServiceName, realm.RealmName, server.Password, server.ServiceSalt);
            }
            foreach (var server in realm.WebServer)
            {
                if (!keytab.CheckPrincipalExistence(server.HttpServiceName, realm.RealmName))
                    keytab.MakeKey(server.HttpServiceName, realm.RealmName, server.Password, server.ServiceSalt);
            }

            // for example, SPN: krbtgt/KERB.COM@CONTOSO.COM SALT: CONTOSO.COMkrbtgtKERB.COM
            if (!keytab.CheckPrincipalExistence(TrustedRealm.KDC[0].DefaultServiceName + "@" + LocalRealm.RealmName, LocalRealm.RealmName))
            {
                keytab.MakeKey(TrustedRealm.KDC[0].DefaultServiceName + "@" + LocalRealm.RealmName, LocalRealm.RealmName, TestSite.Properties.Get("TrustedRealm.TrustPassword"), LocalRealm.RealmName.ToUpper() + "krbtgt" + TrustedRealm.RealmName.ToUpper());
            }
        }

        public EncryptionKey QueryKey(string principal, string realm, Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic.EncryptionType type, uint kvno = 0)
        {
            return keytab.QueryKey(principal, realm, type, kvno);
        }
    }
}
