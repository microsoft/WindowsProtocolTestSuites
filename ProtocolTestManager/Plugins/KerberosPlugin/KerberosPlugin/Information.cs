// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestManager.KerberosPlugin
{
    public enum DetectResult
    {
        Supported,
        UnSupported,
        DetectFail,
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

    public class DetectionInfo
    {
        #region common info

        public string TransportType;
        public string TransportBufferSize;
        public IPVersion ipVersion;
        public KerberosTrustType trustType;
        public string SupportedOid;
        public bool IsKileImplemented = true;
        public bool IsClaimSupported;
        public string TrustPwd;
        public bool UseKKdcp;
        public bool HasHttpServer = false;
        public bool HasSmbServer = false;
        public bool HasLdapServer = false;

        #endregion

        //domain info
        public DomainInfo localDomain;
        public DomainInfo trustDomain;

        //kkdcp info
        public KkdcpInfo kkdcpInfo;

        //local domain
        public Server localDC;
        public Server localClient;
        public Server localAP;
        public Dictionary<string, User> localUsers;


        //trust domain
        public Server trustDC;
        public Server trustAP;
        public Dictionary<string, User> trustUsers;

        public Dictionary<string, string> detectExceptions;
        public List<string> unsupportedItems;

        public DetectionInfo()
        {
            ResetDetectResult();
        }

        public void ResetDetectResult()
        {
            detectExceptions = new Dictionary<string, string>();

            localDomain = new DomainInfo();
            localDomain.KrbtgtPassword = "Password01$";
            trustDomain = new DomainInfo();

            kkdcpInfo = new KkdcpInfo();
            kkdcpInfo.KKDCPServerUrl = "https://proxy01.contoso.com/KdcProxy";
            kkdcpInfo.KKDCPClientCertPassword = "";
            kkdcpInfo.KKDCPClientCertPath = "";

            trustType = KerberosTrustType.NoTrust;

            localDC = new Server();
            localDC.Password = "Password01!";
            localDC.Port = "88";
            localDC.ldapService = new LDAPService();

            localClient = new Server();
            localClient.Password = "Password05!";
            localClient.Port = "88";

            localAP = new Server();
            localAP.Password = "Password02!";

            localAP.smb2Service = new Smb2Service();

            localAP.httpService = new HttpService();
            localAP.httpService.HttpServiceName = "http/ap01.contoso.com";
            localAP.httpService.Uri = "http://ap01.contoso.com";

            localAP.authNotReqService = new OtherService();
            localAP.authNotReqService.DefaultServiceName = "host/AuthNotRequired.contoso.com";
            localAP.authNotReqService.ServiceSalt = "CONTOSO.COMhostauthnotrequired.contoso.com";
            localAP.authNotReqService.FQDN = "AuthNotRequired.contoso.com";
            localAP.authNotReqService.NetBios = "AuthNotRequired$";
            localAP.authNotReqService.Password = "Password01!";

            localAP.localResourceService1 = new OtherService();
            localAP.localResourceService1.DefaultServiceName = "host/localResource01.contoso.com";
            localAP.localResourceService1.ServiceSalt = "CONTOSO.COMhostlocalresource01.contoso.com";
            localAP.localResourceService1.FQDN = "localResource01.contoso.com";
            localAP.localResourceService1.NetBios = "localResource01$";
            localAP.localResourceService1.Password = "Password01!";

            localAP.localResourceService2 = new OtherService();
            localAP.localResourceService2.DefaultServiceName = "host/localResource02.contoso.com";
            localAP.localResourceService2.ServiceSalt = "CONTOSO.COMhostlocalresource02.contoso.com";
            localAP.localResourceService2.FQDN = "localResource02.contoso.com";
            localAP.localResourceService2.NetBios = "localResource02$";
            localAP.localResourceService2.Password = "Password01!";

            localUsers = new Dictionary<string, User>();
            localUsers.Add("User01", new User("test01", "Password01^", null, "CONTOSO.COMtest01", null));
            localUsers.Add("User02", new User("test02", "Password01&", null, null, null));
            localUsers.Add("User03", new User("UserDelegNotAllowed", "Chenjialuo;", null, null, null));
            localUsers.Add("User04", new User("UserTrustedForDeleg", "Yuanchengzhi;", null, null, "abc/UserTrustedForDeleg"));
            localUsers.Add("User05", new User("UserWithoutUPN", "Zhangwuji;", null, null, null));
            localUsers.Add("User06", new User("UserPreAuthNotReq", "Duanyu;", null, null, null));
            localUsers.Add("User07", new User("UserDisabled", "Chenjinnan;", null, null, null));
            localUsers.Add("User08", new User("UserExpired", "Guojing;", null, null, null));
            localUsers.Add("User09", new User("UserLocked", "Qiaofeng;", null, null, null));
            localUsers.Add("User10", new User("UserOutofLogonHours", "Huyidao;", null, null, null));
            localUsers.Add("User11", new User("UserPwdMustChgPast", "Weixiaobao;", null, null, null));
            localUsers.Add("User12", new User("UserPwdMustChgZero", "Yangguo;", null, null, null));
            localUsers.Add("User13", new User("UserLocalGroup", "Yantengda;", null, null, null));
            localUsers.Add("User14", new User("UserDesOnly", "Renyingying;", null, null, null));
            localUsers.Add("User15", new User("testsilo01", "Password01!", null, null, null));
            localUsers.Add("User16", new User("testsilo02", "Password01!", null, null, null));
            localUsers.Add("User17", new User("testsilo03", "Password01!", null, null, null));
            localUsers.Add("User18", new User("testsilo04", "Password01!", null, null, null));
            localUsers.Add("User19", new User("testsilo05", "Password01!", null, null, null));
            localUsers.Add("User22", new User("testpwd", "Password01!", null, null, null));

            trustDC = new Server();
            trustDC.Password = "Password03!";
            trustDC.Port = "88";
            trustDC.FQDN = "AP02.kerb.com";
            trustDC.NetBIOS = "AP02$";
            trustDC.IPv4 = "192.168.0.20";
            trustDC.IPv6 = "2012::2";
            trustDC.DefaultServiceName = "krbtgt/KERB.COM";
            trustDC.ServiceSalt = "KERB.COMhostap02.kerb.com";

            trustDC.ldapService = new LDAPService();
            trustDC.ldapService.LdapServiceName = "ldap/dc02.kerb.com";
            trustDC.ldapService.GssToken = "GSSAPI";
            trustDC.ldapService.Port = "389";

            trustAP = new Server();
            trustAP.FQDN = "AP02.kerb.com";
            trustAP.NetBIOS = "AP02$";
            trustAP.Password = "Password04!";
            trustAP.IPv4 = "192.168.0.20";
            trustAP.IPv6 = "2012::20";
            trustAP.DefaultServiceName = "host/ap02.kerb.com";
            trustAP.ServiceSalt = "KERB.COMhostap02.kerb.com";


            trustAP.smb2Service = new Smb2Service();
            trustAP.smb2Service.SMB2ServiceName = "cifs/ap02.kerb.com";
            trustAP.smb2Service.SMB2Dialect = "Smb30";
            trustAP.smb2Service.CBACShare = "share";

            trustAP.httpService = new HttpService();
            trustAP.httpService.HttpServiceName = "http/ap02.kerb.com";
            trustAP.httpService.Uri = "http://ap02.kerb.com";

            trustAP.authNotReqService = new OtherService();
            trustAP.localResourceService1 = new OtherService();
            trustAP.localResourceService2 = new OtherService();

            trustUsers = new Dictionary<string, User>();
            trustUsers.Add("krbtgt", new User("krbtgt", "Password01%", null, null, null));
            trustUsers.Add("Admin", new User("administrator", "Password01#", null, null, null));
            trustUsers.Add("User01", new User("test03", "Password01*", null, null, null));
            trustUsers.Add("User02", new User("test04", "Password01(", null, null, null));

        }
    }
}
