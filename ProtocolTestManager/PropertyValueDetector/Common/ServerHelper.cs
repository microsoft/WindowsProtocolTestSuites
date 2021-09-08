// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.Protocols.TestManager.Detector.Common
{
    /// <summary>
    /// All common method will be add in this class
    /// it can be used for all Plugins to do common check.
    /// </summary>
    public static class ServerHelper
    {
        public static string GetDomainFunctionalLevel(string domainName, string domainAdminName, string domainAdminPwd)
        {
            LdapConnection connection = new LdapConnection(domainName);
            NetworkCredential cred = new NetworkCredential(domainAdminName, domainAdminPwd, domainName);
            connection.Credential = cred;
            string attributeName = "domainControllerFunctionality";

            string attributeValue = null;
            object[] attribute = null;
            try
            {
                SearchRequest searchRequest = new SearchRequest(null, "(objectClass=*)", SearchScope.Base, attributeName);
                SearchResponse searchResponse = (SearchResponse)connection.SendRequest(searchRequest);
                SearchResultAttributeCollection attributes = searchResponse.Entries[0].Attributes;
                attribute = attributes[attributeName].GetValues(typeof(string));
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to query Domain Controller Functionality.", ex);
            }

            if (attribute.Length > 1)
            {
                throw new Exception("Attribute has more than one entry.");
            }
            attributeValue = Convert.ToString(attribute[0]);

            return attributeValue;
        }

        /// <summary>
        /// get a bunch of accounts with same type, such as computers, or users at once
        /// </summary>
        /// <param name="accountType"></param>
        /// <param name="domainName"></param>
        /// <param name="domainAdminName"></param>
        /// <param name="domainAdminPwd"></param>
        /// <returns></returns>
        public static string GetAccounts(string accountType, string domainName, string domainAdminName, string domainAdminPwd)
        {
            LdapConnection connection = new LdapConnection(domainName);
            NetworkCredential cred = new NetworkCredential(domainAdminName, domainAdminPwd, domainName);
            connection.Credential = cred;

            //get domain DN name
            string[] domainDn = domainName.Split('.');
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < domainDn.Length; i++)
            {
                sb.Append("dc=");
                sb.Append(domainDn[i]);
                if (i != domainDn.Length - 1)
                {
                    sb.Append(",");
                }
            }
            string dn = sb.ToString();
            string targetOu = "CN=" + accountType + "," + dn;
            //string[] accounts = new string[] { };

            SearchRequest searchRequest = null;
            SearchResponse searchResponse = null;
            string attributeValue = null;
            string filter = "CN=*";
            string attributeName = "users";
            string[] accounts = new string[] { attributeName };

            try
            {
                searchRequest = new SearchRequest(targetOu, filter, SearchScope.Subtree, accounts);

                searchResponse = (SearchResponse)connection.SendRequest(searchRequest);
                SearchResultAttributeCollection attributes = searchResponse.Entries[0].Attributes;


            }
            catch (Exception ex)
            {
                throw new Exception("Requst attribute failed with targetOU: " + targetOu + ", filter: " + filter + ", attribute: msDS-Behavior-Version. " + ex.Message);
            }

            return attributeValue;
        }

        public static string GetDCAttribute(string computerName, string attributeName, string domainName, string domainAdminName, string domainAdminPwd)
        {
            LdapConnection connection = new LdapConnection(domainName);
            NetworkCredential cred = new NetworkCredential(domainAdminName, domainAdminPwd, domainName);
            connection.Credential = cred;

            //get domain DN name
            string[] domainDn = domainName.Split('.');
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < domainDn.Length; i++)
            {
                sb.Append("dc=");
                sb.Append(domainDn[i]);
                if (i != domainDn.Length - 1)
                {
                    sb.Append(",");
                }
            }
            string dn = sb.ToString();
            string targetOu = "OU=Domain Controllers," + dn;
            string filter = "CN=" + computerName;
            string[] attributesToReturn = new string[] { attributeName };

            SearchRequest searchRequest = null;
            SearchResponse searchResponse = null;
            string attributeValue = null;

            try
            {
                searchRequest = new SearchRequest(targetOu, filter, SearchScope.Subtree, attributesToReturn);

                searchResponse = (SearchResponse)connection.SendRequest(searchRequest);
                SearchResultAttributeCollection attributes = searchResponse.Entries[0].Attributes;
                object[] attribute = null;

                try
                {
                    attribute = attributes[attributeName].GetValues(Type.GetType("System.String"));
                }
                catch
                {
                    return null;
                }
                if (attribute.Length > 1)
                {
                    throw new Exception("Attribute has more than one entry.");
                }

                attributeValue = Convert.ToString(attribute[0]);

            }
            catch (Exception ex)
            {
                throw new Exception("Requst attribute failed with targetOU: " + targetOu + ", filter: " + filter + ", attribute: " + attributeName + ". " + ex.Message);
            }

            return attributeValue;
        }

        public static string GetComputerIP(string computerName)
        {
            string localIP = string.Empty;
            foreach (IPAddress ip in Dns.GetHostAddresses(computerName))
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }

        public static Platform ConvertPlatform(string platform)
        {
            if (platform.Contains("Windows Server 2016"))
                return Platform.WindowsServer2016;
            if (platform.Contains("Windows Server 2012 R2"))
                return Platform.WindowsServer2012R2;
            else if (platform.Contains("Windows Server 2012"))
                return Platform.WindowsServer2012;
            else if (platform.Contains("Windows Server 2008 R2"))
                return Platform.WindowsServer2008R2;
            else if (platform.Contains("Windows Serer 2008"))
                return Platform.WindowsServer2008;
            else
                return Platform.NonWindows;
        }

        public static bool DetectClient(DomainInfo domain, ComputerInfo client, DetectLogger logWriter)
        {
            logWriter.AddLog(DetectLogLevel.Warning, string.Format("===== Detect Client/Driver in Domain {0} =====", domain.Name));
            string ip = client.IPv4 != null ? client.IPv4 : client.IPv6;

            try
            {
                string computerName = Dns.GetHostEntry(ip).HostName;
                computerName = computerName.Split('.')[0];
            }
            catch
            {
                logWriter.AddLog(DetectLogLevel.Error, "Failed", false, LogStyle.StepFailed);
                logWriter.AddLineToLog(DetectLogLevel.Information);
                return false;
            }

            logWriter.AddLog(DetectLogLevel.Warning, "Success", false, LogStyle.StepPassed);
            logWriter.AddLineToLog(DetectLogLevel.Information);
            return true;
        }

        public static bool DetectDC(DomainInfo domain, Server dc, DetectLogger logWriter)
        {
            logWriter.AddLog(DetectLogLevel.Warning, string.Format("===== Detect DC in Domain {0} =====", domain.Name));
            DirectoryContext context = new DirectoryContext(DirectoryContextType.Domain, domain.Name, domain.Admin, domain.AdminPassword);

            string hostName = DomainController.FindOne(context).Name;
            var hostEntry = Dns.GetHostEntry(hostName);
            try
            {

                string computerName = hostEntry.HostName;
                computerName = computerName.Split('.')[0];
                dc.ComputerName = computerName;
                dc.FQDN = ServerHelper.GetDCAttribute(computerName, "dNSHostName", domain.Name, domain.Admin, domain.AdminPassword);
                dc.IsWindows = ServerHelper.FetchPlatformInfo(computerName, logWriter);
            }
            catch
            {
                logWriter.AddLog(DetectLogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                logWriter.AddLineToLog(DetectLogLevel.Information);
                return false;
            }

            if (dc.FQDN == null)
            {
                logWriter.AddLog(DetectLogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                logWriter.AddLineToLog(DetectLogLevel.Information);
                return false;
            }

            try
            {
                dc.NetBIOS = ServerHelper.GetDCAttribute(dc.ComputerName, "sAMAccountName", domain.Name, domain.Admin, domain.AdminPassword);//DC01$: NetBIOS name                                       
                dc.DefaultServiceName = "krbtgt/" + domain.Name.ToUpper();
                dc.ServiceSalt = domain.Name.ToUpper() + "host" + dc.FQDN.ToLower();
            }
            catch
            {
                logWriter.AddLog(DetectLogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                logWriter.AddLineToLog(DetectLogLevel.Information);
                return false;
            }
            try
            {
                domain.FunctionalLevel = ServerHelper.GetDomainFunctionalLevel(domain.Name, domain.Admin, domain.AdminPassword);
            }
            catch
            {
                logWriter.AddLog(DetectLogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                logWriter.AddLineToLog(DetectLogLevel.Information);
                return false;
            }

            logWriter.AddLog(DetectLogLevel.Warning, "Success", false, LogStyle.StepPassed);
            logWriter.AddLineToLog(DetectLogLevel.Information);
            return true;
        }

        public static bool FetchPlatformInfo(string computerName, DetectLogger logWriter)
        {
            bool isWindows = true;
            try
            {
                ManagementObjectCollection resultCollection = QueryWmiObject(computerName, "SELECT * FROM Win32_OperatingSystem");
                foreach (ManagementObject result in resultCollection)
                {
                    isWindows = (ConvertPlatform(result["Caption"].ToString()) != Platform.NonWindows);
                    logWriter.AddLog(DetectLogLevel.Information, "Platform: " + ConvertPlatform(result["Caption"].ToString()));
                    break;
                }
            }
            catch
            {

            }
            return isWindows;
        }

        private static ManagementObjectCollection QueryWmiObject(string machineName, string queryString)
        {
            ConnectionOptions options = new ConnectionOptions() { Timeout = new TimeSpan(0, 0, 5) };
            ManagementScope ms = new ManagementScope("\\\\" + machineName + "\\root\\cimv2", options);
            ms.Connect();
            ObjectQuery query = new ObjectQuery(queryString);
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(ms, query);
            return searcher.Get();
        }
    }
}
