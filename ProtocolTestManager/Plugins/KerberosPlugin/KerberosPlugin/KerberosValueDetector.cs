// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using System.Net;
using System.Reflection;
using System.Net.NetworkInformation;
using Microsoft.Protocols.TestManager.KerberosPlugin;
using System.Windows.Controls;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;

namespace Microsoft.Protocols.TestManager.Detector
{
    public class KerberosValueDetector : IValueDetector
    {
        private enum EnvironmentType
        {
            SingleRealm,
            CrossRealm,
        }

        private DetectionInfo detectionInfo = new DetectionInfo();
        private EnvironmentType env = EnvironmentType.CrossRealm;

        // TODO:    Update the file path and file name later
        private Logger logWriter = new Logger("KerberosPlugin_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + ".log");


        #region Flags
        bool hasLocalSmbAP = false;
        bool hasLocalWebAP = false;

        bool hasTrustSmbAP = false;
        bool hasTrustWebAP = false;


        #endregion


        void System.IDisposable.Dispose()
        {
        }

        /// <summary>
        /// Add Detection steps to the log shown when detecting
        /// </summary>
        public List<DetectingItem> GetDetectionSteps()
        {
            List<DetectingItem> DetectingItems = new List<DetectingItem>();

            DetectingItems.Add(new DetectingItem("Detect Client.", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Detect KDC", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Detect Application Server", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Detect users", DetectingStatus.Pending, LogStyle.Default));

            return DetectingItems;
        }
        /// <summary>
        /// Get the prerequisites for auto-detection.
        /// </summary>
        /// <returns>A instance of Prerequisites class.</returns>
        public Prerequisites GetPrerequisites()
        {

            hasLocalSmbAP = false;
            hasLocalWebAP = false;
            hasTrustSmbAP = false;
            hasTrustWebAP = false;

            Prerequisites reqs = new Prerequisites();
            reqs.Title = "Pre-Detect Configure";
            reqs.Summary = "Please set the following values:";
            reqs.Properties = new Dictionary<string, List<string>>();

            reqs.Properties.Add("TrustType", ServerHelper.ConstructValueListUsingPtfConfig("TrustType", "Forest", "Realm", "NoTrust", "Child"));

            //KkdcpInfo
            reqs.Properties.Add("Support KKDCP", ServerHelper.ConstructValueListUsingPtfConfig("IsClaimSupported", "true", "false"));
            reqs.Properties.Add("KKDCP proxy Url", ServerHelper.ConstructValueListUsingPtfConfig("KKDCPServerUrl"));

            #region local realm
            reqs.Properties.Add("Local Domain", ServerHelper.ConstructValueListUsingPtfConfig("LocalRealm.RealmName"));
            reqs.Properties.Add("Local Domain Admin", ServerHelper.ConstructValueListUsingPtfConfig("LocalRealm.Users.Admin.Username"));
            reqs.Properties.Add("Local Domain Admin Pwd", ServerHelper.ConstructValueListUsingPtfConfig("LocalRealm.Users.Admin.Password"));
            reqs.Properties.Add("Local DC is LDAP Server", ServerHelper.ConstructValueList("true","false"));
            reqs.Properties.Add("Local AP Server Name", ServerHelper.ConstructValueListUsingPtfConfig("LocalRealm.FileServer01.FQDN"));
            reqs.Properties.Add("Local AP is File Server", ServerHelper.ConstructValueList("true", "false"));
            reqs.Properties.Add("Local AP is HTTP Server", ServerHelper.ConstructValueList("true", "false"));

            reqs.Properties.Add("Local HTTP Server Uri", ServerHelper.ConstructValueListUsingPtfConfig("LocalRealm.WebServer01.HttpUri"));

            #endregion

            #region  cross realm prequisite
            reqs.Properties.Add("Trust Domain", ServerHelper.ConstructValueListUsingPtfConfig("TrustedRealm.RealmName"));
            reqs.Properties.Add("Trust Domain Admin User", ServerHelper.ConstructValueListUsingPtfConfig("TrustedRealm.Users.Admin.Username"));
            reqs.Properties.Add("Trust Domain Admin Pwd", ServerHelper.ConstructValueListUsingPtfConfig("TrustedRealm.Users.Admin.Password"));
            reqs.Properties.Add("Trust Password", ServerHelper.ConstructValueListUsingPtfConfig("TrustedRealm.TrustPassword"));
            reqs.Properties.Add("Trust DC is LDAP Server", ServerHelper.ConstructValueList("true", "false"));
            reqs.Properties.Add("Trust AP Server Name", ServerHelper.ConstructValueListUsingPtfConfig("TrustedRealm.FileServer01.FQDN"));
            reqs.Properties.Add("Trust AP is File Server", ServerHelper.ConstructValueList("true", "false"));
            reqs.Properties.Add("Trust AP is HTTP Server", ServerHelper.ConstructValueList("true", "false"));

            reqs.Properties.Add("Trust Http Server Uri", ServerHelper.ConstructValueListUsingPtfConfig("TrustedRealm.WebServer01.HttpUri"));

            #endregion
            return reqs;
        }

        /// <summary>
        /// Set the values for the required properties.
        /// </summary>
        /// <param name="properties">Property name and values.</param>
        /// <returns>
        /// Return true if no other property needed. Return false means there are
        /// other property required. PTF Tool will invoke GetPrerequisites and
        /// pop up a dialog to set the value of the properties.
        /// </returns>
        public bool SetPrerequisiteProperties(Dictionary<string, string> properties)
        {
            this.detectionInfo.localDomain.Name = properties["Local Domain"];
            this.detectionInfo.localDomain.Admin = properties["Local Domain Admin"];
            this.detectionInfo.localDomain.AdminPassword = properties["Local Domain Admin Pwd"];
            this.detectionInfo.localDC.IPv4 = ServerHelper.GetDCIP(this.detectionInfo.localDomain.Name);


            //common properties
            string trusttype;

            try
            {
                properties.TryGetValue("TrustType", out trusttype);
                if (String.IsNullOrEmpty(trusttype))
                {
                    return false;
                }

                detectionInfo.trustType = (KerberosTrustType)Enum.Parse(typeof(KerberosTrustType), trusttype);
            }
            catch
            {
                return false;
            }


            string isKkdcapSupported;
            string kkdcpUrl;
            try
            {
                properties.TryGetValue("Support KKDCP", out isKkdcapSupported);
                properties.TryGetValue("KKDCP proxy Url", out kkdcpUrl);

                if (String.IsNullOrEmpty(isKkdcapSupported) || string.IsNullOrEmpty(kkdcpUrl))
                {
                    detectionInfo.UseKKdcp = false;
                    detectionInfo.kkdcpInfo.KKDCPServerUrl = null;
                    return false;
                }
                else
                {
                    detectionInfo.UseKKdcp = (isKkdcapSupported.ToLower() == "true");
                    detectionInfo.kkdcpInfo.KKDCPServerUrl = kkdcpUrl;
                }
            }
            catch
            {
                return false;
            }


            //local domain
            #region
            string localDomainName;
            string localDomainAdminUsername;
            string localDomainAdminPwd;

            if (properties.TryGetValue("Local Domain", out localDomainName) &&
                properties.TryGetValue("Local Domain Admin", out localDomainAdminUsername) &&
                properties.TryGetValue("Local Domain Admin Pwd", out localDomainAdminPwd)

                )
            {
                if (String.IsNullOrEmpty(localDomainName) ||
                    String.IsNullOrEmpty(localDomainAdminUsername) ||
                    String.IsNullOrEmpty(localDomainAdminPwd)
                    )
                {
                    return false;
                }

                //build up the detectioninfo with the local domain configurations
                detectionInfo.localDomain.Name = localDomainName;
                detectionInfo.localDomain.Admin = localDomainAdminUsername;
                detectionInfo.localDomain.AdminPassword = localDomainAdminPwd;

                this.detectionInfo.localAP.authNotReqService.DefaultServiceName = $"host/AuthNotRequired.{localDomainName}";
                this.detectionInfo.localAP.authNotReqService.ServiceSalt = $"{localDomainName.ToUpper()}hostauthnotrequired.{localDomainName}";
                this.detectionInfo.localAP.authNotReqService.FQDN = $"AuthNotRequired.{localDomainName}";

                this.detectionInfo.localAP.localResourceService1.DefaultServiceName = $"host/localResource01.{localDomainName}";
                this.detectionInfo.localAP.localResourceService1.ServiceSalt = $"{localDomainName.ToUpper()}hostlocalresource01.{localDomainName}";
                this.detectionInfo.localAP.localResourceService1.FQDN = $"localResource01.{localDomainName}";

                this.detectionInfo.localAP.localResourceService2.DefaultServiceName = $"host/localResource02.{localDomainName}";
                this.detectionInfo.localAP.localResourceService2.ServiceSalt = $"{localDomainName.ToUpper()}hostlocalresource02.{localDomainName}";
                this.detectionInfo.localAP.localResourceService2.FQDN = $"localResource02.{localDomainName}";

                this.detectionInfo.localUsers["User01"].Salt = $"{localDomainName.ToUpper()}test01";
            }

            #endregion

            //local kdc
            #region

            #endregion

            //local client
            #region
            try
            {
                this.detectionInfo.localClient.IPv4 = ServerHelper.GetComputerIP(Dns.GetHostName());
                this.detectionInfo.localClient.IPv6 = ServerHelper.GetComputerIP(Dns.GetHostName());

                this.detectionInfo.localClient.ComputerName = Dns.GetHostName().Split('.')[0];
                this.detectionInfo.localClient.IsWindows = true;
            }
            catch
            {
            }

            #endregion

            string isLocalSmbAPSUT = string.Empty;
            string isLocalWebAPSUT = string.Empty;
            string isLocalLdapAPSUT = string.Empty;

            //local smb ap
            #region

            properties.TryGetValue("Local AP is File Server", out isLocalSmbAPSUT);

            this.hasLocalSmbAP = false;

            string localSmbServerName = string.Empty;
            string localSmbServerPort = string.Empty;
            string localSmbServerPwd = string.Empty;

            if (isLocalSmbAPSUT.ToLower() == "true")
            {
                try
                {
                    localSmbServerName = properties["Local AP Server Name"];

                    if (!String.IsNullOrEmpty(localSmbServerName))
                    {
                        this.hasLocalSmbAP = true;

                        //build up the detectioninfo with the local AP computer configurations
                        detectionInfo.localAP.FQDN = localSmbServerName;
                    }
                    detectionInfo.HasSmbServer = true;
                }
                catch
                {
                    detectionInfo.HasSmbServer = false;
                    this.hasLocalSmbAP = false;
                }
            }
            #endregion

            //http ap
            #region
            string localHttpUri;
            try
            {
                properties.TryGetValue("Local AP is HTTP Server", out isLocalWebAPSUT);
                properties.TryGetValue("Local HTTP Server Uri", out localHttpUri);

                if (isLocalWebAPSUT.ToLower() == "true" && !string.IsNullOrEmpty(localHttpUri))
                {
                    this.hasLocalWebAP = true;
                    detectionInfo.HasHttpServer = true;
                    this.detectionInfo.localAP.httpService.HttpServiceName = $"http/{localSmbServerName}";
                    this.detectionInfo.localAP.httpService.Uri = localHttpUri;
                }
                else
                {
                    detectionInfo.HasHttpServer = false;
                    this.hasLocalWebAP = false;
                }
            }
            catch
            {
                detectionInfo.HasHttpServer = false;
                this.hasLocalWebAP = false;
            }
            #endregion

            //ldap ap
            #region
            properties.TryGetValue("Local DC is LDAP Server", out isLocalLdapAPSUT);
            string localLdapServerPort = string.Empty;

            if (isLocalLdapAPSUT.ToLower() == "true"
                && !string.IsNullOrEmpty(this.detectionInfo.localDomain.Name)
                )
            {
                this.detectionInfo.localDC.ldapService.GssToken = "GSSAPI";
                this.detectionInfo.localDC.ldapService.Port = "389";
                detectionInfo.HasLdapServer = true;
            }
            #endregion

            //cross realm
            string isTrustSmbAPSUT = string.Empty;
            string isTrustWebAPSUT = string.Empty;
            string isTrustLdapAPSUT = string.Empty;

            #region trust domain
            string trustDomainName = string.Empty;
            if (this.detectionInfo.trustType != KerberosTrustType.NoTrust)
            {
                //trust domain and DC
                #region
                string trustDomainAdminUsername = string.Empty;
                string trustDomainAdminPwd = string.Empty;
                string trustPwd = string.Empty;

                if (
                    properties.TryGetValue("Trust Domain", out trustDomainName) &&
                    properties.TryGetValue("Trust Domain Admin User", out trustDomainAdminUsername) &&
                    properties.TryGetValue("Trust Domain Admin Pwd", out trustDomainAdminPwd) &&
                    properties.TryGetValue("Trust Password", out trustPwd)
                    )
                {
                    if (String.IsNullOrEmpty(trustDomainName) ||
                        String.IsNullOrEmpty(trustDomainAdminUsername) ||
                        String.IsNullOrEmpty(trustDomainAdminPwd) ||
                        string.IsNullOrEmpty(trustPwd)

                        )
                    {
                        return false;
                    }

                    //build up the detectioninfo with the trust domain configurations

                    detectionInfo.trustDomain.Name = trustDomainName;
                    detectionInfo.trustDomain.Admin = trustDomainAdminUsername;
                    detectionInfo.trustDomain.AdminPassword = trustDomainAdminPwd;
                    detectionInfo.TrustPwd = trustPwd;

                    detectionInfo.trustDC.IPv4 = ServerHelper.GetDCIP(this.detectionInfo.trustDomain.Name);
                    detectionInfo.trustDC.IPv6 = ServerHelper.GetDCIP(this.detectionInfo.trustDomain.Name);
                    detectionInfo.trustDC.DefaultServiceName = $"krbtgt/{trustDomainName.ToUpper()}";
                }
            }
            #endregion

            //trust smb ap

            #region

            properties.TryGetValue("Trust AP is File Server", out isTrustSmbAPSUT);
            this.hasTrustSmbAP = false;
            string trustSmbServerName = string.Empty;

            if (isTrustSmbAPSUT.ToLower() == "true" && this.detectionInfo.trustType != KerberosTrustType.NoTrust)
            {
                trustSmbServerName = properties["Trust AP Server Name"];
                this.hasTrustSmbAP = true;

                //build up the detectioninfo with the local AP computer configurations
                detectionInfo.trustAP.ComputerName = trustSmbServerName.Split('.')[0];
                detectionInfo.trustAP.FQDN = trustSmbServerName;
                detectionInfo.trustAP.NetBIOS = $"{trustSmbServerName.Split('.')[0]}$";
                detectionInfo.trustAP.ServiceSalt = $"{trustDomainName.ToUpper()}host{trustSmbServerName}";
                detectionInfo.trustAP.DefaultServiceName = $"host/{trustSmbServerName}";
                detectionInfo.trustAP.smb2Service.SMB2ServiceName = $"cifs/{trustSmbServerName}";
                detectionInfo.trustAP.httpService.HttpServiceName = $"http/{trustSmbServerName}";
            }

            #endregion

            //http ap
            #region
            try
            {
                properties.TryGetValue("Trust AP is HTTP Server", out isTrustWebAPSUT);

                string trustHttpUri;
                properties.TryGetValue("Trust Http Server Uri", out trustHttpUri);

                if (isTrustWebAPSUT.ToLower() == "true"
                    && this.detectionInfo.trustType != KerberosTrustType.NoTrust
                    && !string.IsNullOrEmpty(trustHttpUri)
                    )
                {
                    this.hasTrustWebAP = true;
                    this.detectionInfo.trustAP.httpService.Uri = trustHttpUri;
                }
            }
            catch
            {
                this.hasTrustWebAP = false;
            }

            #endregion

            //ldap ap
            #region
            properties.TryGetValue("Trust DC is LDAP Server", out isTrustLdapAPSUT);

            if (isTrustLdapAPSUT.ToLower() == "true" && this.detectionInfo.trustType != KerberosTrustType.NoTrust)
            {
                this.detectionInfo.trustDC.ldapService.GssToken = "GSSAPI";
                this.detectionInfo.trustDC.ldapService.Port = "389";
            }
            #endregion

            #endregion

            return true;

        }

        private bool DetectDC(DomainInfo domain, Server dc, KerberosDetector detector)
        {
            logWriter.AddLog(string.Format("===== Detect DC in Domain {0} =====", domain.Name), LogLevel.Normal);
            DirectoryContext context = new DirectoryContext(DirectoryContextType.Domain,
                domain.Name,
                domain.Admin, domain.AdminPassword);

            string hostName = DomainController.FindOne(context).Name;
            var hostEntry = Dns.GetHostEntry(hostName);
            try
            {

                string computerName = hostEntry.HostName;
                computerName = computerName.Split('.')[0];
                dc.ComputerName = computerName;
                dc.FQDN = ServerHelper.GetDCAttribute(computerName, "dNSHostName", domain.Name, domain.Admin, domain.AdminPassword);
                dc.IsWindows = detector.FetchPlatformInfo(computerName);
            }
            catch
            {
                logWriter.AddLog("Failed", LogLevel.Normal, false, LogStyle.StepFailed);
                logWriter.AddLineToLog(LogLevel.Advanced);
                return false;
            }

            if (dc.FQDN == null)
            {
                logWriter.AddLog("Failed", LogLevel.Normal, false, LogStyle.StepFailed);
                logWriter.AddLineToLog(LogLevel.Advanced);
                return false;
            }

            try
            {
                dc.NetBIOS = ServerHelper.GetDCAttribute(dc.ComputerName, "sAMAccountName", domain.Name, domain.Admin, domain.AdminPassword);//DC01$: NetBIOS name
                dc.DefaultServiceName = "krbtgt/" + domain.Name.ToUpper();
                dc.ServiceSalt = domain.Name.ToUpper() + "host"+ dc.FQDN.ToLower();
                dc.ldapService.LdapServiceName = "ldap/" + dc.FQDN.ToLower();
            }
            catch
            {
                logWriter.AddLog("Failed", LogLevel.Normal, false, LogStyle.StepFailed);
                logWriter.AddLineToLog(LogLevel.Advanced);
                return false;
            }
            try
            {
                domain.FunctionalLevel = ServerHelper.GetDomainFunctionalLevel(domain.Name, domain.Admin, domain.AdminPassword);
            }
            catch
            {
                logWriter.AddLog("Failed", LogLevel.Normal, false, LogStyle.StepFailed);
                logWriter.AddLineToLog(LogLevel.Advanced);
                return false;
            }

            logWriter.AddLog("Success", LogLevel.Normal, false, LogStyle.StepPassed);
            logWriter.AddLineToLog(LogLevel.Advanced);
            return true;
        }

        private bool DetectClient(DomainInfo domain, ComputerInfo client)
        {
            logWriter.AddLog(string.Format("===== Detect Client in Domain {0} =====", domain.Name), LogLevel.Normal);

            string ip = client.IPv4 != null ? client.IPv4 : client.IPv6;


            try
            {
                string computerName = Dns.GetHostEntry(ip).HostName;
                computerName = computerName.Split('.')[0];
                client.FQDN = ServerHelper.GetAccountAttribute(computerName, "Computers", "dNSHostName", domain.Name, domain.Admin, domain.AdminPassword);
            }
            catch
            {
                logWriter.AddLog("Failed", LogLevel.Normal, false, LogStyle.StepFailed);
                logWriter.AddLineToLog(LogLevel.Advanced);
                return false;
            }

            if (client.FQDN == null)
            {
                logWriter.AddLog("Failed", LogLevel.Normal, false, LogStyle.StepFailed);
                logWriter.AddLineToLog(LogLevel.Advanced);
                return false;
            }

            string[] tempArray = client.FQDN.Split('.');
            client.ComputerName = tempArray[0];

            try
            {
                client.NetBIOS = ServerHelper.GetAccountAttribute(client.ComputerName, "Computers", "sAMAccountName", domain.Name, domain.Admin, domain.AdminPassword);//DC01$: NetBIOS name
                client.DefaultServiceName = "host/" + client.FQDN.ToLower();
                client.ServiceSalt = domain.Name.ToUpper() + "host" + client.FQDN.ToLower();
                client.IsWindows = true; //client is always windows.
            }
            catch
            {
                logWriter.AddLog("Failed", LogLevel.Normal, false, LogStyle.StepFailed);
                logWriter.AddLineToLog(LogLevel.Advanced);
                return false;
            }

            logWriter.AddLog("Success", LogLevel.Normal, false, LogStyle.StepPassed);
            logWriter.AddLineToLog(LogLevel.Advanced);
            return true;


        }

        private bool DetectAP(DomainInfo domain, Server ap, KerberosDetector detector)
        {
            logWriter.AddLog(string.Format("===== Detect Application Server in Domain {0} =====", domain.Name), LogLevel.Normal);

            string hostname = ap.FQDN;
            IPAddress ip = IPAddress.Loopback;
            try
            {
                var hostentry = Dns.GetHostEntry(hostname);
                ip = hostentry.AddressList[0];
                ap.IPv4 = ip.ToString();
                string computerName = hostentry.HostName;
                string machineName = computerName.Split('.')[0];
                ap.FQDN = ServerHelper.GetAccountAttribute(machineName, "Computers", "dNSHostName", domain.Name, domain.Admin, domain.AdminPassword);
                ap.IsWindows = detector.FetchPlatformInfo(computerName);
            }
            catch
            {
                logWriter.AddLog("Failed", LogLevel.Normal, false, LogStyle.StepFailed);
                logWriter.AddLineToLog(LogLevel.Advanced);
                return false;
            }

            if (ap.FQDN == null)
            {
                logWriter.AddLog("Failed", LogLevel.Normal, false, LogStyle.StepFailed);
                logWriter.AddLineToLog(LogLevel.Advanced);
                return false;
            }

            string[] tempArray = ap.FQDN.Split('.');
            ap.ComputerName = tempArray[0];

            try
            {
                ap.NetBIOS = ServerHelper.GetAccountAttribute(ap.ComputerName, "Computers", "sAMAccountName", domain.Name, domain.Admin, domain.AdminPassword);//DC01$: NetBIOS name
                ap.DefaultServiceName = "host/" + ap.FQDN.ToLower();
                ap.ServiceSalt = domain.Name.ToUpper() + "host" + ap.FQDN.ToLower();
                ap.smb2Service.SMB2ServiceName = "cifs/" + ap.FQDN.ToLower();
            }
            catch
            {
                logWriter.AddLog("Failed", LogLevel.Normal, false, LogStyle.StepFailed);
                logWriter.AddLineToLog(LogLevel.Advanced);
                return false;
            }

            try
            {
                if (detectionInfo.HasSmbServer)
                {
                    //get smb dialect
                    Smb2Client clientForInitialOpen = new Smb2Client(new TimeSpan(0, 0, 15));
                    byte[] gssToken;
                    Packet_Header header;
                    try
                    {
                        clientForInitialOpen.ConnectOverTCP(ip);

                        NEGOTIATE_Response negotiateResp;
                        DialectRevision connection_Dialect = DialectRevision.Smb2Unknown;
                        DialectRevision[] requestDialect = new DialectRevision[] { DialectRevision.Smb2002, DialectRevision.Smb21, DialectRevision.Smb30, DialectRevision.Smb302 };
                        ulong messageId = 0;

                        uint status = clientForInitialOpen.Negotiate(
                            1,
                            1,
                            Packet_Header_Flags_Values.NONE,
                            messageId++,
                            requestDialect,
                            SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                            Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU,
                            Guid.NewGuid(),
                            out connection_Dialect,
                            out gssToken,
                            out header,
                            out negotiateResp);

                        if (header.Status != Smb2Status.STATUS_SUCCESS)
                        {
                            logWriter.AddLog("Failed", LogLevel.Normal, false, LogStyle.StepFailed);
                            logWriter.AddLineToLog(LogLevel.Advanced);
                            return false;
                        }
                        else
                        {
                            ap.smb2Service.SMB2Dialect = connection_Dialect.ToString();
                        }

                    }
                    catch
                    {
                        logWriter.AddLog("Failed", LogLevel.Normal, false, LogStyle.StepFailed);
                        logWriter.AddLineToLog(LogLevel.Advanced);
                        return false;
                    }

                    //detect smb share

                    string[] shareList = ServerHelper.EnumShares(ap.IPv4, domain.Admin, domain.Name, domain.AdminPassword);
                    if (shareList.Length > 0)
                    {
                        //only get the first one as default value
                        //can ptftool support add more choices?
                        for (int i = 0; i < shareList.Length; i++)
                        {
                            if (shareList[i].Substring(shareList[i].Length - 1, 1) != "$")
                            {
                                ap.smb2Service.DACShare = shareList[i];
                                ap.smb2Service.CBACShare = shareList[i];
                                break;
                            }
                        }

                    }
                    else
                    {
                        ap.smb2Service.DACShare = string.Empty;
                        ap.smb2Service.CBACShare = string.Empty;
                    }
                }
                if (detectionInfo.HasHttpServer)
                {
                    //detect http server
                    ap.httpService.HttpServiceName = "http/" + ap.FQDN.ToLower();

                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + ap.FQDN);
                        request.Credentials = new NetworkCredential(domain.Admin + "@" + domain.Name, domain.AdminPassword);
                        WebResponse response = request.GetResponse();

                        ap.httpService.Uri = response.ResponseUri.OriginalString;
                    }
                    catch
                    {
                        ap.httpService.Uri = string.Empty;
                    }
                }

            }
            catch
            {
                logWriter.AddLog("Failed", LogLevel.Normal, false, LogStyle.StepFailed);
                logWriter.AddLineToLog(LogLevel.Advanced);
                return false;
            }

            logWriter.AddLog("Success", LogLevel.Normal, false, LogStyle.StepPassed);
            logWriter.AddLineToLog(LogLevel.Advanced);
            return true;
        }

        private bool DetectUsers(ref Dictionary<string, User> users, DomainInfo domain)
        {
            logWriter.AddLog(string.Format("===== Detect Users in Domain {0} =====", domain.Name), LogLevel.Normal);

            string userName = string.Empty;
            string password = string.Empty;
            string salt = string.Empty;
            string serviceName = string.Empty;
            string userDomain = string.Empty;

            try
            {
                foreach (string key in users.Keys)
                {
                    try
                    {
                        User user = users[key];
                        ServerHelper.GetAccountAttribute(user.Name, "Users", "sAMAccountName", domain.Name, domain.Admin, domain.AdminPassword);
                    }
                    catch
                    {
                        //remove the unsupported items.
                        //users.Remove(key);
                    }

                }

                return true;
            }
            catch
            {
                logWriter.AddLog("Failed", LogLevel.Normal, false, LogStyle.StepFailed);
                logWriter.AddLineToLog(LogLevel.Advanced);
                return false;
            }


        }
        /// <summary>
        /// Run property autodetection.
        /// </summary>
        /// <returns>Return true if the function is succeeded.</returns>
        public bool RunDetection()
        {
            logWriter.AddLog("===== Start detecting =====", LogLevel.Normal, false);

            KerberosDetector detector = new KerberosDetector(logWriter);
            // detect
            #region

            //local client
            if (!DetectClient(detectionInfo.localDomain, detectionInfo.localClient))
            {
                return false;
            }

            //local dc
            if (!DetectDC(detectionInfo.localDomain, detectionInfo.localDC, detector))
            {
                return false;
            }

            //local smb2 and http ap
            if (hasLocalSmbAP || hasLocalWebAP)
            {
                if (!DetectAP(detectionInfo.localDomain, detectionInfo.localAP, detector))
                {
                    return false;
                }
            }


            //local users
            DetectUsers(ref detectionInfo.localUsers, detectionInfo.localDomain);


            if (detectionInfo.trustType != KerberosTrustType.NoTrust)
            {
                //trust dc
                if (!DetectDC(detectionInfo.trustDomain, detectionInfo.trustDC, detector))
                {
                    return false;
                }
                //trust users
                DetectUsers(ref detectionInfo.trustUsers, detectionInfo.trustDomain);


                if (hasTrustSmbAP || hasTrustWebAP)
                {
                    //trust smb2 and http ap
                    DetectAP(detectionInfo.trustDomain, detectionInfo.trustAP, detector);
                }
            }
            #endregion
            logWriter.AddLog("Passed", LogLevel.Normal, false, LogStyle.StepPassed);
            logWriter.AddLog("===== End detecting =====", LogLevel.Normal);
            return true;
        }


        /// <summary>
        /// Get the detect result.
        /// </summary>
        /// <param name="name">Property name</param>
        /// <param name="value">Property value</param>
        /// <returns>Return true if the property value is successfully got.</returns>
        public bool GetDetectedProperty(out Dictionary<string, List<string>> propertiesDic)
        {
            Dictionary<string, List<string>> temp = new Dictionary<string, List<string>>();

            #region Common

            temp.Add("IpVersion", new List<string>() { this.detectionInfo.ipVersion.ToString()});
            temp.Add("TrustType", new List<string>() { this.detectionInfo.trustType.ToString()});

            #endregion

            #region KKDCP

            temp.Add("UseProxy", new List<string>() { this.detectionInfo.UseKKdcp ? "true" : "false" });
            temp.Add("KKDCPServerUrl", new List<string>() { this.detectionInfo.kkdcpInfo.KKDCPServerUrl });

            #endregion

            #region Local Realm

            temp.Add("LocalRealm.RealmName", new List<string>() { this.detectionInfo.localDomain.Name });
            temp.Add("LocalRealm.DomainControllerFunctionality", new List<string>() { this.detectionInfo.localDomain.FunctionalLevel });
            temp.Add("LocalRealm.Users.Admin.Username", new List<string>() { this.detectionInfo.localDomain.Admin });
            temp.Add("LocalRealm.Users.Admin.Password", new List<string>() { this.detectionInfo.localDomain.AdminPassword });

            #endregion

            #region Local Realm KDC
            temp.Add("LocalRealm.KDC01.IsWindows", new List<string>() { this.detectionInfo.localDC.IsWindows.ToString()});
            temp.Add("LocalRealm.KDC01.FQDN", new List<string>() { this.detectionInfo.localDC.FQDN});
            temp.Add("LocalRealm.KDC01.NetBiosName", new List<string>() { this.detectionInfo.localDC.NetBIOS});
            temp.Add("LocalRealm.KDC01.Password", new List<string>() { this.detectionInfo.localDC.Password });
            temp.Add("LocalRealm.KDC01.Port", new List<string>() { this.detectionInfo.localDC.Port });

            if (this.detectionInfo.ipVersion == IPVersion.IPv4)
            {
                temp.Add("LocalRealm.KDC01.IPv4Address", new List<string>() { this.detectionInfo.localDC.IPv4 });
            }
            else
            {
                temp.Add("LocalRealm.KDC01.IPv6Address", new List<string>() { this.detectionInfo.localDC.IPv6 });
            }

            #endregion

            #region Local Realm Client

            temp.Add("LocalRealm.ClientComputer.FQDN", new List<string>() { this.detectionInfo.localClient.FQDN });
            temp.Add("LocalRealm.ClientComputer.NetBiosName", new List<string>() { this.detectionInfo.localClient.NetBIOS });
            temp.Add("LocalRealm.ClientComputer.Password", new List<string>() { this.detectionInfo.localClient.Password });
            temp.Add("LocalRealm.ClientComputer.Port", new List<string>() { this.detectionInfo.localClient.Port });

            if (this.detectionInfo.ipVersion == IPVersion.IPv4)
            {
                temp.Add("LocalRealm.ClientComputer.IPv4Address", new List<string>() { this.detectionInfo.localClient.IPv4 });
            }
            else
            {
                temp.Add("LocalRealm.ClientComputer.IPv6Address", new List<string>() { this.detectionInfo.localClient.IPv6 });
            }
            temp.Add("LocalRealm.ClientComputer.DefaultServiceName", new List<string>() { this.detectionInfo.localClient.DefaultServiceName });
            temp.Add("LocalRealm.ClientComputer.ServiceSalt", new List<string>() { this.detectionInfo.localClient.ServiceSalt });

            #endregion

            #region Local Realm SMB Server

            temp.Add("LocalRealm.FileServer01.FQDN", new List<string>() { this.detectionInfo.localAP.FQDN });
            temp.Add("LocalRealm.FileServer01.NetBiosName", new List<string>() {this.detectionInfo.localAP.NetBIOS});
            temp.Add("LocalRealm.FileServer01.Password", new List<string>() { this.detectionInfo.localAP.Password });

            if (this.detectionInfo.ipVersion == IPVersion.IPv4)
            {
                temp.Add("LocalRealm.FileServer01.IPv4Address", new List<string>() { this.detectionInfo.localAP.IPv4 });
            }
            else
            {
                temp.Add("LocalRealm.FileServer01.IPv6Address", new List<string>() { this.detectionInfo.localAP.IPv6 });
            }
            temp.Add("LocalRealm.FileServer01.DefaultServiceName", new List<string>() { this.detectionInfo.localAP.DefaultServiceName });
            temp.Add("LocalRealm.FileServer01.ServiceSalt", new List<string>() { this.detectionInfo.localAP.ServiceSalt });

            temp.Add("LocalRealm.FileServer01.Smb2ServiceName", new List<string>() { this.detectionInfo.localAP.smb2Service.SMB2ServiceName});
            temp.Add("LocalRealm.FileServer01.Smb2Dialect", new List<string>() { this.detectionInfo.localAP.smb2Service.SMB2Dialect });
            temp.Add("LocalRealm.FileServer01.DACShareFolder", new List<string>() { this.detectionInfo.localAP.smb2Service.DACShare });
            temp.Add("LocalRealm.FileServer01.CBACShareFolder", new List<string>() { this.detectionInfo.localAP.smb2Service.CBACShare });

            #endregion

            #region Local Realm Ldap server

            temp.Add("LocalRealm.LdapServer01.FQDN", new List<string>() { this.detectionInfo.localDC.FQDN });
            temp.Add("LocalRealm.LdapServer01.NetBiosName", new List<string>() { this.detectionInfo.localDC.NetBIOS });
            temp.Add("LocalRealm.LdapServer01.Password", new List<string>() { this.detectionInfo.localDC.Password });

            if (this.detectionInfo.ipVersion == IPVersion.IPv4)
            {
                temp.Add("LocalRealm.LdapServer01.IPv4Address", new List<string>() { this.detectionInfo.localDC.IPv4 });
            }
            else
            {
                temp.Add("LocalRealm.LdapServer01.IPv6Address", new List<string>() { this.detectionInfo.localDC.IPv6 });
            }
            temp.Add("LocalRealm.LdapServer01.DefaultServiceName", new List<string>() { this.detectionInfo.localDC.DefaultServiceName });
            temp.Add("LocalRealm.LdapServer01.ServiceSalt", new List<string>() { this.detectionInfo.localDC.ServiceSalt });

            temp.Add("LocalRealm.LdapServer01.LdapServiceName", new List<string>() { this.detectionInfo.localDC.ldapService.LdapServiceName });
            temp.Add("LocalRealm.LdapServer01.LdapPort", new List<string>() { this.detectionInfo.localDC.ldapService.Port });
            temp.Add("LocalRealm.LdapServer01.GssToken", new List<string>() { this.detectionInfo.localDC.ldapService.GssToken });

            #endregion

            #region Local Realm web server
            temp.Add("LocalRealm.WebServer01.FQDN", new List<string>() { this.detectionInfo.localAP.FQDN });
            temp.Add("LocalRealm.WebServer01.NetBiosName", new List<string>() { this.detectionInfo.localAP.NetBIOS });
            temp.Add("LocalRealm.WebServer01.Password", new List<string>() { this.detectionInfo.localAP.Password });

            if (this.detectionInfo.ipVersion == IPVersion.IPv4)
            {
                temp.Add("LocalRealm.WebServer01.IPv4Address", new List<string>() { this.detectionInfo.localAP.IPv4 });
            }
            else
            {
                temp.Add("LocalRealm.WebServer01.IPv6Address", new List<string>() { this.detectionInfo.localAP.IPv6 });
            }
            temp.Add("LocalRealm.WebServer01.DefaultServiceName", new List<string>() { this.detectionInfo.localAP.DefaultServiceName });
            temp.Add("LocalRealm.WebServer01.ServiceSalt", new List<string>() { this.detectionInfo.localAP.ServiceSalt });

            temp.Add("LocalRealm.WebServer01.HttpServiceName", new List<string>() { this.detectionInfo.localAP.httpService.HttpServiceName});
            temp.Add("LocalRealm.WebServer01.HttpUri", new List<string>() { this.detectionInfo.localAP.httpService.Uri});


            #endregion

            #region Local Realm AuthNotRequired server
            temp.Add("LocalRealm.AuthNotRequired.FQDN", new List<string>() { this.detectionInfo.localAP.authNotReqService.FQDN });
            temp.Add("LocalRealm.AuthNotRequired.NetBiosName", new List<string>() { this.detectionInfo.localAP.authNotReqService.NetBios });
            temp.Add("LocalRealm.AuthNotRequired.Password", new List<string>() { this.detectionInfo.localAP.authNotReqService.Password });
            temp.Add("LocalRealm.AuthNotRequired.DefaultServiceName", new List<string>() { this.detectionInfo.localAP.authNotReqService.DefaultServiceName});
            temp.Add("LocalRealm.AuthNotRequired.ServiceSalt", new List<string>() { this.detectionInfo.localAP.authNotReqService.ServiceSalt });


            #endregion

            #region Local Realm localResource01 server
            temp.Add("LocalRealm.localResource01.FQDN", new List<string>() { this.detectionInfo.localAP.localResourceService1.FQDN });
            temp.Add("LocalRealm.localResource01.NetBiosName", new List<string>() { this.detectionInfo.localAP.localResourceService1.NetBios });
            temp.Add("LocalRealm.localResource01.Password", new List<string>() { this.detectionInfo.localAP.localResourceService1.Password });
            temp.Add("LocalRealm.localResource01.DefaultServiceName", new List<string>() { this.detectionInfo.localAP.localResourceService1.DefaultServiceName });
            temp.Add("LocalRealm.localResource01.ServiceSalt", new List<string>() { this.detectionInfo.localAP.localResourceService1.ServiceSalt });


            #endregion

            #region Local Realm localResource02 server
            temp.Add("LocalRealm.localResource02.FQDN", new List<string>() { this.detectionInfo.localAP.localResourceService2.FQDN });
            temp.Add("LocalRealm.localResource02.NetBiosName", new List<string>() { this.detectionInfo.localAP.localResourceService2.NetBios });
            temp.Add("LocalRealm.localResource02.Password", new List<string>() { this.detectionInfo.localAP.localResourceService2.Password });
            temp.Add("LocalRealm.localResource02.DefaultServiceName", new List<string>() { this.detectionInfo.localAP.localResourceService2.DefaultServiceName });
            temp.Add("LocalRealm.localResource02.ServiceSalt", new List<string>() { this.detectionInfo.localAP.localResourceService2.ServiceSalt });

            #endregion

            #region Local Realm resourceGroups server

            temp.Add("LocalRealm.resourceGroups.resourceGroupCount", new List<string>() { "2" });
            temp.Add("LocalRealm.resourceGroups.resourceGroup01.GroupName", new List<string>() { "resourceGroup" });
            temp.Add("LocalRealm.resourceGroups.resourceGroup02.GroupName", new List<string>() { "resourceGroup2" });

            #endregion

            #region Local Realm Users Information
            temp.Add("LocalRealm.Users.krbtgt.Username", new List<string>() { "krbtgt" });
            temp.Add("LocalRealm.Users.krbtgt.Password", new List<string>() { this.detectionInfo.localDomain.KrbtgtPassword});

            temp.Add("LocalRealm.Users.User01.Username", new List<string>() { this.detectionInfo.localUsers["User01"].Name });
            temp.Add("LocalRealm.Users.User01.Password", new List<string>() { this.detectionInfo.localUsers["User01"].Password });
            temp.Add("LocalRealm.Users.User01.Salt", new List<string>() { this.detectionInfo.localUsers["User01"].Salt });

            temp.Add("LocalRealm.Users.User02.Username", new List<string>() { this.detectionInfo.localUsers["User02"].Name });
            temp.Add("LocalRealm.Users.User02.Password", new List<string>() { this.detectionInfo.localUsers["User02"].Password });

            temp.Add("LocalRealm.Users.User03.Username", new List<string>() { this.detectionInfo.localUsers["User03"].Name });
            temp.Add("LocalRealm.Users.User03.Password", new List<string>() { this.detectionInfo.localUsers["User03"].Password });

            temp.Add("LocalRealm.Users.User04.ServiceName", new List<string>() { this.detectionInfo.localUsers["User04"].ServiceName });
            temp.Add("LocalRealm.Users.User04.Username", new List<string>() { this.detectionInfo.localUsers["User04"].Name });
            temp.Add("LocalRealm.Users.User04.Password", new List<string>() { this.detectionInfo.localUsers["User04"].Password });

            temp.Add("LocalRealm.Users.User05.Username", new List<string>() { this.detectionInfo.localUsers["User05"].Name });
            temp.Add("LocalRealm.Users.User05.Password", new List<string>() { this.detectionInfo.localUsers["User05"].Password });

            temp.Add("LocalRealm.Users.User06.Username", new List<string>() { this.detectionInfo.localUsers["User06"].Name });
            temp.Add("LocalRealm.Users.User06.Password", new List<string>() { this.detectionInfo.localUsers["User06"].Password });

            temp.Add("LocalRealm.Users.User07.Username", new List<string>() { this.detectionInfo.localUsers["User07"].Name });
            temp.Add("LocalRealm.Users.User07.Password", new List<string>() { this.detectionInfo.localUsers["User07"].Password });

            temp.Add("LocalRealm.Users.User08.Username", new List<string>() { this.detectionInfo.localUsers["User08"].Name });
            temp.Add("LocalRealm.Users.User08.Password", new List<string>() { this.detectionInfo.localUsers["User08"].Password });

            temp.Add("LocalRealm.Users.User09.Username", new List<string>() { this.detectionInfo.localUsers["User09"].Name });
            temp.Add("LocalRealm.Users.User09.Password", new List<string>() { this.detectionInfo.localUsers["User09"].Password });

            temp.Add("LocalRealm.Users.User10.Username", new List<string>() { this.detectionInfo.localUsers["User10"].Name });
            temp.Add("LocalRealm.Users.User10.Password", new List<string>() { this.detectionInfo.localUsers["User10"].Password });

            temp.Add("LocalRealm.Users.User11.Username", new List<string>() { this.detectionInfo.localUsers["User11"].Name });
            temp.Add("LocalRealm.Users.User11.Password", new List<string>() { this.detectionInfo.localUsers["User11"].Password });

            temp.Add("LocalRealm.Users.User12.Username", new List<string>() { this.detectionInfo.localUsers["User12"].Name });
            temp.Add("LocalRealm.Users.User12.Password", new List<string>() { this.detectionInfo.localUsers["User12"].Password });

            temp.Add("LocalRealm.Users.User13.Username", new List<string>() { this.detectionInfo.localUsers["User13"].Name });
            temp.Add("LocalRealm.Users.User13.Password", new List<string>() { this.detectionInfo.localUsers["User13"].Password });

            temp.Add("LocalRealm.Users.User14.Username", new List<string>() { this.detectionInfo.localUsers["User14"].Name });
            temp.Add("LocalRealm.Users.User14.Password", new List<string>() { this.detectionInfo.localUsers["User14"].Password });

            temp.Add("LocalRealm.Users.User15.Username", new List<string>() { this.detectionInfo.localUsers["User15"].Name });
            temp.Add("LocalRealm.Users.User15.Password", new List<string>() { this.detectionInfo.localUsers["User15"].Password });

            temp.Add("LocalRealm.Users.User16.Username", new List<string>() { this.detectionInfo.localUsers["User16"].Name });
            temp.Add("LocalRealm.Users.User16.Password", new List<string>() { this.detectionInfo.localUsers["User16"].Password });

            temp.Add("LocalRealm.Users.User17.Username", new List<string>() { this.detectionInfo.localUsers["User17"].Name });
            temp.Add("LocalRealm.Users.User17.Password", new List<string>() { this.detectionInfo.localUsers["User17"].Password });

            temp.Add("LocalRealm.Users.User18.Username", new List<string>() { this.detectionInfo.localUsers["User18"].Name });
            temp.Add("LocalRealm.Users.User18.Password", new List<string>() { this.detectionInfo.localUsers["User18"].Password });

            temp.Add("LocalRealm.Users.User19.Username", new List<string>() { this.detectionInfo.localUsers["User19"].Name });
            temp.Add("LocalRealm.Users.User19.Password", new List<string>() { this.detectionInfo.localUsers["User19"].Password });

            temp.Add("LocalRealm.Users.User22.Username", new List<string>() { this.detectionInfo.localUsers["User22"].Name });
            temp.Add("LocalRealm.Users.User22.Password", new List<string>() { this.detectionInfo.localUsers["User22"].Password });

            #endregion

            #region Trust Realm
            temp.Add("TrustedRealm.TrustPassword", new List<string>() { this.detectionInfo.TrustPwd });
            temp.Add("TrustedRealm.RealmName", new List<string>() { this.detectionInfo.trustDomain.Name });
            temp.Add("TrustedRealm.DomainControllerFunctionality", new List<string>() { this.detectionInfo.trustDomain.FunctionalLevel });
            #endregion

            #region Trust Realm KDC
            temp.Add("TrustedRealm.KDC01.IsWindows", new List<string>() { this.detectionInfo.trustDC.IsWindows.ToString()});
            temp.Add("TrustedRealm.KDC01.FQDN", new List<string>() { this.detectionInfo.trustDC.FQDN });
            temp.Add("TrustedRealm.KDC01.NetBiosName", new List<string>() { this.detectionInfo.trustDC.NetBIOS });
            temp.Add("TrustedRealm.KDC01.Password", new List<string>() { this.detectionInfo.trustDC.Password });
            temp.Add("TrustedRealm.KDC01.Port", new List<string>() { this.detectionInfo.trustDC.Port });

            if (this.detectionInfo.ipVersion == IPVersion.IPv4)
            {
                temp.Add("TrustedRealm.KDC01.IPv4Address", new List<string>() { this.detectionInfo.trustDC.IPv4 });
            }
            else
            {
                temp.Add("TrustedRealm.KDC01.IPv6Address", new List<string>() { this.detectionInfo.trustDC.IPv6 });
            }
            temp.Add("TrustedRealm.KDC01.DefaultServiceName", new List<string>() { this.detectionInfo.trustDC.DefaultServiceName });
            #endregion

            #region Trust Realm SMB Server

            temp.Add("TrustedRealm.FileServer01.FQDN", new List<string>() { this.detectionInfo.trustAP.FQDN });
            temp.Add("TrustedRealm.FileServer01.NetBiosName", new List<string>() {this.detectionInfo.trustAP.NetBIOS});
            temp.Add("TrustedRealm.FileServer01.Password", new List<string>() { this.detectionInfo.trustAP.Password });

            if (this.detectionInfo.ipVersion == IPVersion.IPv4)
            {
                temp.Add("TrustedRealm.FileServer01.IPv4Address", new List<string>() { this.detectionInfo.trustAP.IPv4 });
            }
            else
            {
                temp.Add("TrustedRealm.FileServer01.IPv6Address", new List<string>() { this.detectionInfo.trustAP.IPv6 });
            }

            temp.Add("TrustedRealm.FileServer01.DefaultServiceName", new List<string>() { this.detectionInfo.trustAP.DefaultServiceName });
            temp.Add("TrustedRealm.FileServer01.ServiceSalt", new List<string>() { this.detectionInfo.trustAP.ServiceSalt });

            temp.Add("TrustedRealm.FileServer01.Smb2ServiceName", new List<string>() { this.detectionInfo.trustAP.smb2Service.SMB2ServiceName });
            temp.Add("TrustedRealm.FileServer01.Smb2Dialect", new List<string>() { this.detectionInfo.trustAP.smb2Service.SMB2Dialect });
            temp.Add("TrustedRealm.FileServer01.CBACShareFolder", new List<string>() { this.detectionInfo.trustAP.smb2Service.CBACShare });
            #endregion

            #region Trust Realm Ldap server

            temp.Add("TrustedRealm.LdapServer01.FQDN", new List<string>() { this.detectionInfo.trustDC.FQDN });
            temp.Add("TrustedRealm.LdapServer01.NetBiosName", new List<string>() { this.detectionInfo.trustDC.NetBIOS });
            temp.Add("TrustedRealm.LdapServer01.Password", new List<string>() { this.detectionInfo.trustDC.Password });

            if (this.detectionInfo.ipVersion == IPVersion.IPv4)
            {
                temp.Add("TrustedRealm.LdapServer01.IPv4Address", new List<string>() { this.detectionInfo.trustDC.IPv4 });
            }
            else
            {
                temp.Add("TrustedRealm.LdapServer01.IPv6Address", new List<string>() { this.detectionInfo.trustDC.IPv6 });
            }
            temp.Add("TrustedRealm.LdapServer01.DefaultServiceName", new List<string>() { this.detectionInfo.trustDC.DefaultServiceName });
            temp.Add("TrustedRealm.LdapServer01.ServiceSalt", new List<string>() { this.detectionInfo.trustDC.ServiceSalt });

            temp.Add("TrustedRealm.LdapServer01.LdapServiceName", new List<string>() { this.detectionInfo.trustDC.ldapService.LdapServiceName });
            temp.Add("TrustedRealm.LdapServer01.LdapPort", new List<string>() { this.detectionInfo.trustDC.ldapService.Port });
            temp.Add("TrustedRealm.LdapServer01.GssToken", new List<string>() { this.detectionInfo.trustDC.ldapService.GssToken });

            #endregion

            #region Trust Realm web server
            temp.Add("TrustedRealm.WebServer01.FQDN", new List<string>() { this.detectionInfo.trustAP.FQDN });
            temp.Add("TrustedRealm.WebServer01.NetBiosName", new List<string>() { this.detectionInfo.trustAP.NetBIOS });
            temp.Add("TrustedRealm.WebServer01.Password", new List<string>() { this.detectionInfo.trustAP.Password });

            if (this.detectionInfo.ipVersion == IPVersion.IPv4)
            {
                temp.Add("TrustedRealm.WebServer01.IPv4Address", new List<string>() { this.detectionInfo.trustAP.IPv4 });
            }
            else
            {
                temp.Add("TrustedRealm.WebServer01.IPv6Address", new List<string>() { this.detectionInfo.trustAP.IPv6 });
            }
            temp.Add("TrustedRealm.WebServer01.DefaultServiceName", new List<string>() { this.detectionInfo.trustAP.DefaultServiceName });
            temp.Add("TrustedRealm.WebServer01.ServiceSalt", new List<string>() { this.detectionInfo.trustAP.ServiceSalt });

            temp.Add("TrustedRealm.WebServer01.HttpServiceName", new List<string>() { this.detectionInfo.trustAP.httpService.HttpServiceName });
            temp.Add("TrustedRealm.WebServer01.HttpUri", new List<string>() { this.detectionInfo.trustAP.httpService.Uri });


            #endregion

            #region Trust Realm Users
            temp.Add("TrustedRealm.Users.krbtgt.Username", new List<string>() { this.detectionInfo.trustUsers["krbtgt"].Name });
            temp.Add("TrustedRealm.Users.krbtgt.Password", new List<string>() { this.detectionInfo.trustUsers["krbtgt"].Password });

            temp.Add("TrustedRealm.Users.Admin.Username", new List<string>() { this.detectionInfo.trustUsers["Admin"].Name });
            temp.Add("TrustedRealm.Users.Admin.Password", new List<string>() { this.detectionInfo.trustUsers["Admin"].Password });

            temp.Add("TrustedRealm.Users.User01.Username", new List<string>() { this.detectionInfo.trustUsers["User01"].Name });
            temp.Add("TrustedRealm.Users.User01.Password", new List<string>() { this.detectionInfo.trustUsers["User01"].Password });

            temp.Add("TrustedRealm.Users.User02.Username", new List<string>() { this.detectionInfo.trustUsers["User02"].Name });
            temp.Add("TrustedRealm.Users.User02.Password", new List<string>() { this.detectionInfo.trustUsers["User02"].Password });

            #endregion
            propertiesDic = temp;
            return true;
        }

        //
        // Summary:
        //     Gets the list of properties that will be hidder in the configure page.
        //
        // Parameters:
        //   rules:
        //     Selected rules.
        //
        // Returns:
        //     The list of properties whick will not be shown in the configure page.
        public List<string> GetHiddenProperties(List<CaseSelectRule> rules)
        {
            //todo
            return new List<string>();
        }

        /// <summary>
        /// Get selected rules
        /// </summary>
        /// <returns>Selected rules</returns>
        public List<CaseSelectRule> GetSelectedRules()
        {
            List<CaseSelectRule> caselist = new List<CaseSelectRule>();
            RuleStatus status = RuleStatus.Default;

            caselist.Add(new CaseSelectRule()
            {
                Name = "NetworkTopology.Single Realm",
                Status = RuleStatus.Selected
            });

            caselist.Add(new CaseSelectRule()
            {
                Name = "SUT.KDC",
                Status = RuleStatus.Selected
            });

            if (this.detectionInfo.trustType != KerberosTrustType.NoTrust
                && this.detectionInfo.trustDomain != null
                && this.detectionInfo.trustDC != null
                && !string.IsNullOrEmpty(this.detectionInfo.trustDC.DefaultServiceName)
                && !string.IsNullOrEmpty(this.detectionInfo.trustDC.ComputerName)
                && !string.IsNullOrEmpty(this.detectionInfo.trustDC.FQDN)
                && !string.IsNullOrEmpty(this.detectionInfo.trustDC.NetBIOS)
                )
            {

                status = RuleStatus.Selected;
            }
            else
            {
                status = RuleStatus.NotSupported;
            }

            caselist.Add(new CaseSelectRule()
            {
                Name = "NetworkTopology.Cross Realm",
                Status = status
            });


            if (this.detectionInfo.localDomain == null || string.IsNullOrEmpty(this.detectionInfo.localDomain.FunctionalLevel))
            {
                caselist.Add(new CaseSelectRule()
                {
                    Name = "DomainFunctionalityLevel.2K8R2",
                    Status = RuleStatus.NotSupported
                });
                caselist.Add(new CaseSelectRule()
                {
                    Name = "DomainFunctionalityLevel.2K12",
                    Status = RuleStatus.NotSupported
                });
                caselist.Add(new CaseSelectRule()
                {
                    Name = "DomainFunctionalityLevel.2K12R2",
                    Status = RuleStatus.NotSupported
                });
            }
            else
            {
                if (int.Parse(this.detectionInfo.localDomain.FunctionalLevel) == 4)
                {
                    caselist.Add(new CaseSelectRule()
                    {
                        Name = "DomainFunctionalityLevel.2K8R2",
                        Status = RuleStatus.Selected
                    });
                    caselist.Add(new CaseSelectRule()
                    {
                        Name = "DomainFunctionalityLevel.2K12",
                        Status = RuleStatus.NotSupported
                    });
                    caselist.Add(new CaseSelectRule()
                    {
                        Name = "DomainFunctionalityLevel.2K12R2",
                        Status = RuleStatus.NotSupported
                    });
                }
                else if (int.Parse(this.detectionInfo.localDomain.FunctionalLevel) == 5)
                {
                    caselist.Add(new CaseSelectRule()
                    {
                        Name = "DomainFunctionalityLevel.2K8R2",
                        Status = RuleStatus.Selected
                    });
                    caselist.Add(new CaseSelectRule()
                    {
                        Name = "DomainFunctionalityLevel.2K12",
                        Status = RuleStatus.Selected
                    });
                    caselist.Add(new CaseSelectRule()
                    {
                        Name = "DomainFunctionalityLevel.2K12R2",
                        Status = RuleStatus.NotSupported
                    });
                }
                else if (int.Parse(this.detectionInfo.localDomain.FunctionalLevel) == 6)
                {
                    caselist.Add(new CaseSelectRule()
                    {
                        Name = "DomainFunctionalityLevel.2K8R2",
                        Status = RuleStatus.Selected
                    });
                    caselist.Add(new CaseSelectRule()
                    {
                        Name = "DomainFunctionalityLevel.2K12",
                        Status = RuleStatus.Selected
                    });
                    caselist.Add(new CaseSelectRule()
                    {
                        Name = "DomainFunctionalityLevel.2K12R2",
                        Status = RuleStatus.Selected
                    });
                }
                else if (int.Parse(this.detectionInfo.localDomain.FunctionalLevel) == 7)
                {
                    caselist.Add(new CaseSelectRule()
                    {
                        Name = "DomainFunctionalityLevel.2K8R2",
                        Status = RuleStatus.Selected
                    });
                    caselist.Add(new CaseSelectRule()
                    {
                        Name = "DomainFunctionalityLevel.2K12",
                        Status = RuleStatus.Selected
                    });
                    caselist.Add(new CaseSelectRule()
                    {
                        Name = "DomainFunctionalityLevel.2K12R2",
                        Status = RuleStatus.Selected
                    });
                }
                else
                {
                    caselist.Add(new CaseSelectRule()
                    {
                        Name = "DomainFunctionalityLevel.2K8R2",
                        Status = RuleStatus.NotSupported
                    });
                    caselist.Add(new CaseSelectRule()
                    {
                        Name = "DomainFunctionalityLevel.2K12",
                        Status = RuleStatus.NotSupported
                    });
                    caselist.Add(new CaseSelectRule()
                    {
                        Name = "DomainFunctionalityLevel.2K12R2",
                        Status = RuleStatus.NotSupported
                    });
                }

            }


            if(
                (this.detectionInfo.localAP != null
                && this.detectionInfo.localAP.smb2Service != null
                && !string.IsNullOrEmpty(this.detectionInfo.localAP.smb2Service.SMB2ServiceName)
                && !string.IsNullOrEmpty(this.detectionInfo.localAP.smb2Service.SMB2Dialect))
                ||
                 (this.detectionInfo.trustAP != null
                && this.detectionInfo.trustAP.smb2Service != null
                && !string.IsNullOrEmpty(this.detectionInfo.trustAP.smb2Service.SMB2ServiceName)
                && !string.IsNullOrEmpty(this.detectionInfo.trustAP.smb2Service.SMB2Dialect))
                )
            {
                caselist.Add(new CaseSelectRule()
                {
                    Name = "SUT.Smb2ApplicationServer",
                    Status = RuleStatus.Selected
                });
            }
            else
            {
                caselist.Add(new CaseSelectRule()
                {
                    Name = "SUT.Smb2ApplicationServer",
                    Status = RuleStatus.NotSupported
                });
            }

            if(
                (this.detectionInfo.localAP != null
               && this.detectionInfo.localAP.httpService != null
               && !string.IsNullOrEmpty(this.detectionInfo.localAP.httpService.HttpServiceName)
               && !string.IsNullOrEmpty(this.detectionInfo.localAP.httpService.Uri))
                ||
                (this.detectionInfo.trustAP != null
               && this.detectionInfo.trustAP.httpService != null
               && !string.IsNullOrEmpty(this.detectionInfo.trustAP.httpService.HttpServiceName)
               && !string.IsNullOrEmpty(this.detectionInfo.trustAP.httpService.Uri))
                )
            {

                caselist.Add(new CaseSelectRule()
                {
                    Name = "SUT.HttpApplicationServer",
                    Status = RuleStatus.Selected
                });
            }
            else
            {
                caselist.Add(new CaseSelectRule()
                {
                    Name = "SUT.HttpApplicationServer",
                    Status = RuleStatus.NotSupported
                });
            }

            if (
                (this.detectionInfo.localDC != null
               && this.detectionInfo.localDC.ldapService != null
               && !string.IsNullOrEmpty(this.detectionInfo.localDC.ldapService.LdapServiceName)
               && !string.IsNullOrEmpty(this.detectionInfo.localDC.ldapService.Port)
               && !string.IsNullOrEmpty(this.detectionInfo.localDC.ldapService.GssToken))
                ||
                (this.detectionInfo.trustDC != null
               && this.detectionInfo.trustDC.ldapService != null
               && !string.IsNullOrEmpty(this.detectionInfo.trustDC.ldapService.LdapServiceName)
               && !string.IsNullOrEmpty(this.detectionInfo.trustDC.ldapService.Port)
               && !string.IsNullOrEmpty(this.detectionInfo.trustDC.ldapService.GssToken))
                )
            {
                caselist.Add(new CaseSelectRule()
                {
                    Name = "SUT.LdapApplicationServer",
                    Status = RuleStatus.Selected
                });
            }
            else
            {
                caselist.Add(new CaseSelectRule()
                {
                    Name = "SUT.LdapApplicationServer",
                    Status = RuleStatus.NotSupported
                });
            }

            caselist.Add(new CaseSelectRule()
            {
                Name = "Feature.BVT",
                Status = RuleStatus.Selected
            });

            if (this.detectionInfo.IsKileImplemented)
            {
                caselist.Add(new CaseSelectRule()
                {
                    Name = "Feature.KilePac",
                    Status = RuleStatus.Selected
                });
            }
            else
            {
                caselist.Add(new CaseSelectRule()
                {
                    Name = "Feature.KilePac",
                    Status = RuleStatus.NotSupported
                });
            }

            //handle the dfl, claim supported
            int dfl = 4;
            try
            {

                int.TryParse(this.detectionInfo.localDomain.FunctionalLevel, out dfl);
            }
            catch
            {
                dfl = 4;
            }

            if (dfl > 5)
            {
                this.detectionInfo.IsClaimSupported = true;
            }

            if (this.detectionInfo.IsClaimSupported)
            {
                caselist.Add(new CaseSelectRule()
                {
                    Name = "Feature.Claim",
                    Status = RuleStatus.Selected
                });
            }
            else
            {
                caselist.Add(new CaseSelectRule()
                {
                    Name = "Feature.Claim",
                    Status = RuleStatus.NotSupported
                });
            }

            caselist.Add(new CaseSelectRule()
            {
                Name = "Feature.FAST",
                Status = RuleStatus.Selected
            });

            caselist.Add(new CaseSelectRule()
            {
                Name = "Feature.Kerberos Error",
                Status = RuleStatus.Selected

            });

            if (this.detectionInfo.localDomain != null
                && int.Parse(this.detectionInfo.localDomain.FunctionalLevel) >= 6
                )
            {
                caselist.Add(new CaseSelectRule()
                {
                    Name = "Feature.Silo",
                    Status = RuleStatus.Selected
                });
            }
            else
            {
                caselist.Add(new CaseSelectRule()
                {
                    Name = "Feature.Silo",
                    Status = RuleStatus.NotSupported
                });
            }

            if (this.detectionInfo.UseKKdcp)
            {
                caselist.Add(new CaseSelectRule()
                {
                    Name = "SUT.Kerberos Proxy Service",
                    Status = RuleStatus.Selected
                });
                caselist.Add(new CaseSelectRule()
                {
                    Name = "Feature.KKDCP",
                    Status = RuleStatus.Selected
                });
            }

            return caselist;
        }

        DetectionResultControl SUTSummaryControl;
        public object GetSUTSummary()
        {
            SUTSummaryControl = new DetectionResultControl();
            SUTSummaryControl.LoadDetectionInfo(detectionInfo);
            return SUTSummaryControl;
        }

        public void SelectEnvironment(string NetworkEnvironmet)
        {
            Enum.TryParse(NetworkEnvironmet, out env);
        }

        /// <summary>
        /// return false if check failed and set failed property in dictionary
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public bool CheckConfigrationSettings(Dictionary<string, string> properties)
        {
            return true;
        }
    }
}
