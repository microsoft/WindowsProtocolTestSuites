// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Detector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.DirectoryServices.ActiveDirectory;
using System.Collections;
using System.Net;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;
using System.Net.Sockets;

using DSSearchScope = System.DirectoryServices.SearchScope;
using SearchScope = System.DirectoryServices.Protocols.SearchScope;

namespace Microsoft.Protocols.TestManager.ADFamilyPlugin
{
    static class Utility
    {
        static string RegistryPath = @"SOFTWARE\Microsoft\ProtocolTestSuites";
        static string RegistryPath64 = @"SOFTWARE\Wow6432Node\Microsoft\ProtocolTestSuites";

        public static System.Net.IPHostEntry GetHost(string hostName)
        {
            try
            {
                if (string.IsNullOrEmpty(hostName)) return null;
                return System.Net.Dns.GetHostEntry(hostName);
            }
            catch (System.Net.Sockets.SocketException)
            {
                return null;
            }
        }

        public static IPAddress GetAddress(this System.Net.IPHostEntry host)
        {
            return host.AddressList.FirstOrDefault(i => i.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
        }

        public static string GetIpAddress(this System.Net.IPHostEntry host)
        {
            var ip = host.AddressList.FirstOrDefault(i => i.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            if (ip == null) return string.Empty;
            return ip.ToString();
        }

        public static string GetSubnetAddress(IPAddress address)
        {
            IPAddress netmask = IPAddress.Any;
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (adapter.OperationalStatus != OperationalStatus.Up)
                {
                    continue;
                }

                foreach (UnicastIPAddressInformation unicastIPAddressInformation in adapter.GetIPProperties().UnicastAddresses)
                {
                    if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        if (address.Equals(unicastIPAddressInformation.Address))
                        {
                            netmask = unicastIPAddressInformation.IPv4Mask;

                            byte[] addr = address.GetAddressBytes();
                            byte[] mask = netmask.GetAddressBytes();
                            int n = 0;
                            for (int i = 0; i < addr.Length; i++)
                            {
                                addr[i] &= mask[i];
                                for (int j = 0; j < 8; j++)
                                {
                                    n += (mask[i] >> j) & 0x1;
                                }
                            }
                            return new IPAddress(addr).ToString() + "/" + n.ToString();
                        }
                    }
                }
            }
            throw new ArgumentException(string.Format("Can't find subnetmask for IP address '{0}'", address));
        }


        public static string GetNetbiosName(this System.Net.IPHostEntry host)
        {
            return host.HostName.Split('.')[0];
        }

        public static string GetDomainNetbiosName(string dnsName)
        {
            if (String.IsNullOrEmpty(dnsName))
            {
                return string.Empty;
            }

            string netbiosName = string.Empty;

            DirectoryEntry rootDSE = new DirectoryEntry(string.Format("LDAP://{0}/RootDSE", dnsName));

            string configurationNamingContext = rootDSE.Properties["configurationNamingContext"][0].ToString();

            DirectoryEntry searchRoot = new DirectoryEntry("LDAP://cn=Partitions," + configurationNamingContext);

            DirectorySearcher searcher = new DirectorySearcher(searchRoot);
            searcher.SearchScope = DSSearchScope.OneLevel;
            searcher.PropertiesToLoad.Add("netbiosname");
            searcher.Filter = string.Format("(&(objectcategory=Crossref)(dnsRoot={0})(netBIOSName=*))", dnsName);

            SearchResult result = searcher.FindOne();

            if (result != null)
            {
                netbiosName = result.Properties["netbiosname"][0].ToString();
            }

            return netbiosName;
        }

        public static bool LdapPingHost(System.Net.IPHostEntry host, int port = 389)
        {
            try
            {
                LdapConnection ldapconn = new LdapConnection(host.HostName + ":" + port.ToString());
                SearchRequest sr = new SearchRequest("", "(objectClass=*)", SearchScope.Base);
                SearchResponse sp = ldapconn.SendRequest(sr) as SearchResponse;
                ldapconn.Dispose();
                if (sp.ResultCode != ResultCode.Success) return false;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static void AddRule(this List<CaseSelectRule> rules, string name, RuleStatus status = RuleStatus.Selected)
        {
            rules.Add(new CaseSelectRule() { Name = name, Status = status });
        }

        public static bool IsSelected(this List<CaseSelectRule> rules, string name)
        {
            var r = rules.FirstOrDefault(i => i.Name == name);
            if (r == null) return false;
            if (r.Status != RuleStatus.Selected) return false;
            return true;
        }

        public static List<CaseSelectRule> GetRules(
                bool pdc,
                bool sdc,
                bool rodc,
                bool cdc,
                bool tdc,
                bool dm,
                bool adlds,
                int domainFunctionLevel)
        {
            List<CaseSelectRule> rules = new List<CaseSelectRule>();
            rules.AddRule("Machine.PDC", pdc ? RuleStatus.Selected : RuleStatus.NotSupported);
            rules.AddRule("Machine.SDC", sdc ? RuleStatus.Selected : RuleStatus.NotSupported);
            rules.AddRule("Machine.RODC", rodc ? RuleStatus.Selected : RuleStatus.NotSupported);
            rules.AddRule("Machine.CDC", cdc ? RuleStatus.Selected : RuleStatus.NotSupported);
            rules.AddRule("Machine.TDC", tdc ? RuleStatus.Selected : RuleStatus.NotSupported);
            rules.AddRule("Machine.DM", dm ? RuleStatus.Selected : RuleStatus.NotSupported);

            rules.AddRule("Protocol.MS-ADTS-LDAP");
            rules.AddRule("Protocol.MS-ADTS-PublishDC");
            rules.AddRule("Protocol.MS-ADTS-Schema");
            rules.AddRule("Protocol.MS-ADTS-Security.ADDS");
            if (adlds) rules.AddRule("Protocol.MS-ADTS-Security.ADLDS");
            rules.AddRule("Protocol.MS-APDS");
            rules.AddRule("Protocol.MS-DRSR");
            rules.AddRule("Protocol.MS-FRS2");
            rules.AddRule("Protocol.MS-LSAD");
            rules.AddRule("Protocol.MS-LSAT");
            rules.AddRule("Protocol.MS-NRPC");
            rules.AddRule("Protocol.MS-SAMR");

            rules.AddRule("Domain Function Level.Windows Server 2008 R2 and below");
            if (domainFunctionLevel >= 5)
            {
                rules.AddRule("Domain Function Level.Windows Server 2012");
                rules.AddRule("Domain Function Level.Windows Server 2012 R2");
                rules.AddRule("Domain Function Level.Windows Threshold");
            }
            return rules;
        }

        public static List<string> GetHiddenProperties(List<CaseSelectRule> rules)
        {
            List<string> properties = new List<string>();
            foreach (var p in DetectorUtil.GetPropertiesByFile("AD_ServerTestSuite.deployment.ptfconfig"))
            {
                // Machines
                if (!rules.IsSelected("Machine.SDC") && p.IndexOf("Common.WritableDC2") >= 0)
                {
                    properties.Add(p);
                    continue;
                }
                if (!rules.IsSelected("Machine.RODC") && p.IndexOf("Common.RODC") >= 0)
                {
                    properties.Add(p);
                    continue;
                }
                if (!rules.IsSelected("Machine.CDC") &&
                    (p.IndexOf("Common.CDC") >= 0 || p.IndexOf("Common.ChildDomain") >= 0))
                {
                    properties.Add(p);
                    continue;
                }
                if (!rules.IsSelected("Machine.TDC") &&
                    (p.IndexOf("Common.TDC") >= 0 || p.IndexOf("Common.TrustDomain") >= 0))
                {
                    properties.Add(p);
                    continue;
                }
                if (!rules.IsSelected("Machine.DM") && p.IndexOf("Common.DM") >= 0)
                {
                    properties.Add(p);
                    continue;
                }
                if (!rules.IsSelected("Protocol.MS-ADTS-LDAP") && p.IndexOf("MS_ADTS_LDAP") >= 0)
                {
                    properties.Add(p);
                    continue;
                }
                if (!rules.IsSelected("Protocol.MS-ADTS-PublichDC") && p.IndexOf("MS_ADTS_PublishDC") >= 0)
                {
                    properties.Add(p);
                    continue;
                }
                if (!rules.IsSelected("Protocol.MS-ADTS-Schema") && p.IndexOf("MS_ADTS_Schema") >= 0)
                {
                    properties.Add(p);
                    continue;
                }
                if (!rules.IsSelected("Protocol.MS-ADTS-Security") && p.IndexOf("MS_ADTS_SECURITY") >= 0)
                {
                    properties.Add(p);
                    continue;
                }
                if (!rules.IsSelected("Protocol.MS-APDS") && p.IndexOf("MS_APDS") >= 0)
                {
                    properties.Add(p);
                    continue;
                }
                if (!rules.IsSelected("Protocol.MS-DRSR") && p.IndexOf("MS_DRSR") >= 0)
                {
                    properties.Add(p);
                    continue;
                }
                if (!rules.IsSelected("Protocol.MS-FRS2") && p.IndexOf("MS_FRS2") >= 0)
                {
                    properties.Add(p);
                    continue;
                }
                if (!rules.IsSelected("Protocol.MS-LSAD") && p.IndexOf("MS_LSAD") >= 0)
                {
                    properties.Add(p);
                    continue;
                }
                if (!rules.IsSelected("Protocol.MS-LSAT") && p.IndexOf("MS_LSAT") >= 0)
                {
                    properties.Add(p);
                    continue;
                }
                if (!rules.IsSelected("Protocol.MS-NRPC") && p.IndexOf("MS_NRPC") >= 0)
                {
                    properties.Add(p);
                    continue;
                }
                if (!rules.IsSelected("Protocol.MS-SAMR") && p.IndexOf("MS_SAMR") >= 0)
                {
                    properties.Add(p);
                    continue;
                }
                if (p.IndexOf("MS_DRSR.TDI") >= 0)
                {
                    properties.Add(p);
                    continue;
                }
                if (p.IndexOf("MS_ADTS_LDAP.IsTDI") >= 0)
                {
                    properties.Add(p);
                    continue;
                }
                if (p.IndexOf("MS_NRPC.SHOULDMAY") >= 0)
                {
                    properties.Add(p);
                    continue;
                }
                if (p.IndexOf("MS_ADTS_Schema.TDI") >= 0)
                {
                    properties.Add(p);
                    continue;
                }
                if (p.IndexOf("MS_FRS2.ReqImplemented") >= 0)
                {
                    properties.Add(p);
                    continue;
                }
                if (p.IndexOf("MS_FRS2.MAYRequirements") >= 0)
                {
                    properties.Add(p);
                    continue;
                }
                if (p.IndexOf("MS_FRS2.TestSuiteIssueFixed") >= 0)
                {
                    properties.Add(p);
                    continue;
                }
            }
            return properties;
        }

        public static string GetRootDseValue(this SearchResultAttributeCollection attributes, string name)
        {
            var attribute = attributes[name];
            if (attribute == null) throw new AutoDetectionException("Ldap Ping failed, cannot find attribute {0} in RootDSE.", name);
            return attribute.GetValues(typeof(string))[0] as string;
        }

        public static string[] GetRootDseValues(this SearchResultAttributeCollection attributes, string name)
        {
            var attribute = attributes[name];
            if (attribute == null) throw new AutoDetectionException("Ldap Ping failed, cannot find attribute {0} in RootDSE.", name);
            return attribute.GetValues(typeof(string)) as string[];
        }

        public static string GetInstalledTestSuiteVersion()
        {

            RegistryKey HKLM = Registry.LocalMachine;
            RegistryKey TestSuitesRegPath = Environment.Is64BitProcess ?
                HKLM.OpenSubKey(RegistryPath64) : HKLM.OpenSubKey(RegistryPath);

            string registryKeyName = TestSuitesRegPath.GetSubKeyNames()
                                                 .Where((s) => s.Contains("ADFamily"))
                                                 .FirstOrDefault();

            Match versionMatch = Regex.Match(registryKeyName, @"\d+\.\d+\.\d+\.\d+");
            return versionMatch.Value;
        }

        public static LdapSearchResponse LdapPing(string server)
        {
            LdapSearchResponse pingrep = new LdapSearchResponse();
            LdapConnection ldapconn = new LdapConnection(server);
            SearchRequest sr = new SearchRequest("", "(objectClass=*)", SearchScope.Base);
            SearchResponse sp = ldapconn.SendRequest(sr) as SearchResponse;
            if (sp.ResultCode != ResultCode.Success) throw new AutoDetectionException("Ldap Ping failed: {0}, Result Code: {1}", server, sp.ResultCode);
            var attributes = sp.Entries[0].Attributes;
            pingrep.DomainFunctionality = attributes.GetRootDseValue("domainfunctionality");
            pingrep.SupportedSaslMechanisms = attributes.GetRootDseValues("supportedSASLMechanisms");
            ldapconn.Dispose();
            return pingrep;
        }

        public static void LdapBind(string server, NetworkCredential credential)
        {
            LdapConnection ldapconn = new LdapConnection(server);
            ldapconn.Bind(credential);
            ldapconn.Dispose();
        }

        public static LdapSearchResponse GetDomainInfo(string server, string domain, NetworkCredential credential)
        {
            string rootNc = "DC=" + domain.Replace(".", ",DC=");
            LdapSearchResponse pingrep = new LdapSearchResponse();
            LdapConnection ldapconn = new LdapConnection(server);
            ldapconn.Bind(credential);
            SearchRequest sr = new SearchRequest(rootNc, "(objectClass=*)", SearchScope.Base);
            SearchResponse sp = ldapconn.SendRequest(sr) as SearchResponse;
            if (sp.ResultCode != ResultCode.Success) throw new AutoDetectionException(string.Format("Cannot get information of domain: {0} from server {1}.", domain, server));
            var attributes = sp.Entries[0].Attributes;
            // Domain GUID
            byte[] rawguid = attributes["objectGuid"][0] as byte[];
            pingrep.DomainGuid = (new Guid(rawguid)).ToString().ToUpper();

            // Domain SID
            byte[] rawsid = attributes["objectSid"][0] as byte[];
            pingrep.DomainSid = (new System.Security.Principal.SecurityIdentifier(rawsid, 0)).ToString().ToUpper();
            ldapconn.Dispose();
            return pingrep;
        }
    }

    class LdapSearchResponse
    {
        public string DomainFunctionality;
        public string[] SupportedSaslMechanisms;
        public bool IsSupported(string saslMechanisms)
        {
            return SupportedSaslMechanisms.Contains(saslMechanisms);
        }

        public string DomainGuid;
        public string DomainSid;

    }

    public class AutoDetectionException : Exception
    {
        public AutoDetectionException(string message) : base(message)
        {

        }
        public AutoDetectionException(string format, params Object[] args) :
            base(string.Format(format, args))
        {
        }
    }
}
