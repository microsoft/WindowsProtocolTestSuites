// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Ldap;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Common
{
    /// <summary>
    /// Base class for server adapter of AD test suites.
    /// </summary>
    public class ADCommonServerAdapter : ManagedAdapterBase
    {
        #region PTF variables
        // Common configuration
        public bool AllowBreakEnvironment;
        public DomainFunctionLevel DomainFunctionLevel;
        public string DomainAdminGroup;
        public string DomainAdministratorName;
        public string DomainUserPassword;
        // Primary DC
        public string PrimaryDomainNetBiosName;
        public string PrimaryDomainDnsName;
        public string PrimaryDomainSrvGUID;
        public string PrimaryDomainSID;
        public string PDCNetbiosName;
        public string PDCPassword;
        public string PDCIPAddress;
        public ServerVersion PDCOSVersion;
        public bool PDCIsWindows;
        // Secondary DC
        public string SDCNetbiosName;
        public string SDCPassword;
        public string SDCIPAddress;
        public ServerVersion SDCOSVersion;
        public bool SDCIsWindows;
        // Read-only DC
        public string RODCNetbiosName;
        public string RODCPassword;
        public string RODCIPAddress;
        public ServerVersion RODCOSVersion;
        public bool RODCIsWindows;
        // Child DC
        public string ChildDomainNetBiosName;
        public string ChildDomainDnsName;
        public string CDCNetbiosName;
        public string CDCIPAddress;
        public ServerVersion CDCOSVersion;
        public bool CDCIsWindows;
        // Trust DC
        public string TrustDomainNetBiosName;
        public string TrustDomainDnsName;
        public string TDCNetbiosName;
        public string TDCIPAddress;
        public ServerVersion TDCOSVersion;
        public bool TDCIsWindows;
        // Domain Member
        public string DMNetbiosName;
        public string DMPassword;
        public string DMOldPassword;
        public string DMIPAddress;
        // Endpoint
        public string ENDPOINTNetbiosName;
        public string ENDPOINTPassword;
        public string ENDPOINTOldPassword;
        public string ENDPOINTIPAddress;
        // AD LDS
        public string ADLDSInstanceName;
        public string ADDSPortNum;
        public string ADLDSPortNum;
        public string ADLDSSSLPortNum;
        public string LDSApplicationNC;
        public string ClientUserName;
        public string ClientUserPassword;
                
        #endregion PTF variables

        string propertyGroup = "Common.";
        static ADCommonServerAdapter _adapter = null;

        /// <summary>
        /// MA capture path for case PublishDC_TestCaseSamLogonResponseEx
        /// </summary>
        public string capturePath_SamLogonResponseEx = @"C:\Capture\SamLogonResponseEx.matu";

        /// <summary>
        /// MA capture path for case PublishDC_TestCaseSamLogonResponse
        /// </summary>
        public string capturePath_SamLogonResponse = @"C:\Capture\SamLogonResponse.matu";

        /// <summary>
        /// MA capture path for case Security_TCSicilyAuth_2K8R2S8
        /// </summary>
        public string capturePath_SecuritySicily = @"C:\Capture\SecuritySicilyCapture.matu";

        /// <summary>
        /// Static instance for ADCommonServerAdapter
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public static ADCommonServerAdapter Instance(ITestSite site)
        {
            if (_adapter == null)
            {
                _adapter = new ADCommonServerAdapter();
                _adapter.Initialize(site);
            }
            return _adapter;
        }

        public static DomainCollection domains;
        public static DomainControllerCollection domainControllers;
        public static ComputerCollection domainMembers;

        /// <summary>
        /// Initialize information from PTF config
        /// </summary>
        /// <param name="testSite"></param>
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);

            Site.Log.Add(LogEntryKind.Debug, "Read common properties from PTF configure file.");

            AllowBreakEnvironment   = GetBoolProperty(propertyGroup + "AllowBreakEnvironment");
            DomainFunctionLevel     = GetEnumProperty<DomainFunctionLevel>(propertyGroup + "DomainFunctionLevel");
            DomainAdminGroup        = GetProperty(propertyGroup + "DomainAdminGroup");
            DomainAdministratorName = GetProperty(propertyGroup + "DomainAdministratorName", true);
            DomainUserPassword      = GetProperty(propertyGroup + "DomainUserPassword", true);
            PrimaryDomainDnsName    = GetProperty(propertyGroup + "PrimaryDomain.DNSName", true);
            PrimaryDomainNetBiosName = GetProperty(propertyGroup + "PrimaryDomain.NetBiosName" ?? (PrimaryDomainDnsName.Split('.'))[0].ToString());
            PrimaryDomainSrvGUID    = GetProperty(propertyGroup + "PrimaryDomain.ServerGUID", true);
            PrimaryDomainSID        = GetProperty(propertyGroup + "PrimaryDomain.SID", true);
            PDCNetbiosName          = GetProperty(propertyGroup + "WritableDC1.NetbiosName", true);
            PDCPassword             = GetProperty(propertyGroup + "WritableDC1.Password", true);
            PDCIPAddress            = GetProperty(propertyGroup + "WritableDC1.IPAddress");
            PDCOSVersion            = GetEnumProperty<ServerVersion>(propertyGroup + "WritableDC1.OSVersion");
            PDCIsWindows            = (PDCOSVersion == ServerVersion.NonWin ? false : true);
            SDCNetbiosName          = GetProperty(propertyGroup + "WritableDC2.NetbiosName");
            SDCPassword             = GetProperty(propertyGroup + "WritableDC2.Password");
            SDCIPAddress            = GetProperty(propertyGroup + "WritableDC2.IPAddress");
            SDCOSVersion            = GetEnumProperty<ServerVersion>(propertyGroup + "WritableDC2.OSVersion");
            SDCIsWindows            = (SDCOSVersion == ServerVersion.NonWin ? false : true);
            RODCNetbiosName         = GetProperty(propertyGroup + "RODC.NetbiosName");
            RODCPassword            = GetProperty(propertyGroup + "RODC.Password");
            RODCIPAddress           = GetProperty(propertyGroup + "RODC.IPAddress");
            RODCOSVersion           = GetEnumProperty<ServerVersion>(propertyGroup + "RODC.OSVersion");
            RODCIsWindows           = (RODCOSVersion == ServerVersion.NonWin ? false : true);
            ChildDomainDnsName = GetProperty(propertyGroup + "ChildDomain.DNSName");
            ChildDomainNetBiosName = GetProperty(propertyGroup + "ChildDomain.NetBiosName") ?? (ChildDomainDnsName.Split('.'))[0].ToString();
            CDCNetbiosName          = GetProperty(propertyGroup + "CDC.NetbiosName");
            CDCIPAddress            = GetProperty(propertyGroup + "CDC.IPAddress");
            CDCOSVersion            = GetEnumProperty<ServerVersion>(propertyGroup + "CDC.OSVersion");
            CDCIsWindows            = (CDCOSVersion == ServerVersion.NonWin ? false : true);
            TrustDomainDnsName = GetProperty(propertyGroup + "TrustDomain.DNSName");
            TrustDomainNetBiosName = GetProperty(propertyGroup + "TrustDomain.NetBiosName") ?? (TrustDomainDnsName.Split('.'))[0].ToString();
            TDCNetbiosName          = GetProperty(propertyGroup + "TDC.NetbiosName");
            TDCIPAddress            = GetProperty(propertyGroup + "TDC.IPAddress");
            TDCOSVersion            = GetEnumProperty<ServerVersion>(propertyGroup + "TDC.OSVersion");
            TDCIsWindows            = (TDCOSVersion == ServerVersion.NonWin ? false : true);
            DMNetbiosName           = GetProperty(propertyGroup + "DM.NetbiosName");
            DMPassword              = GetProperty(propertyGroup + "DM.Password");
            DMOldPassword           = GetProperty(propertyGroup + "DM.OldPassword");
            DMIPAddress             = GetProperty(propertyGroup + "DM.IPAddress");
            ENDPOINTNetbiosName     = GetProperty(propertyGroup + "ENDPOINT.NetbiosName", true);
            ENDPOINTPassword        = GetProperty(propertyGroup + "ENDPOINT.Password", true);
            ENDPOINTOldPassword     = GetProperty(propertyGroup + "ENDPOINT.OldPassword");
            ENDPOINTIPAddress       = GetProperty(propertyGroup + "ENDPOINT.IPAddress");
            ADLDSInstanceName       = GetProperty(propertyGroup + "ADLDSInstanceName");
            ADDSPortNum             = GetProperty(propertyGroup + "ADDSPortNum");
            ADLDSPortNum            = GetProperty(propertyGroup + "ADLDSPortNum");
            ADLDSSSLPortNum         = GetProperty(propertyGroup + "ADLDSSSLPortNum");
            LDSApplicationNC        = GetProperty(propertyGroup + "LDSApplicationNC");
            ClientUserName          = GetProperty(propertyGroup + "ClientUserName");
            ClientUserPassword      = GetProperty(propertyGroup + "ClientUserPassword");

            Site.Log.Add(LogEntryKind.Debug, "Read common properties from PTF configure file completed.");
            Site.Log.Add(LogEntryKind.Debug, "Construct common classes for domain, domain controller and endpoint.");

            domains = new DomainCollection();
            domainControllers = new DomainControllerCollection();

            Domain primaryDomain = new Domain(PrimaryDomainDnsName, PrimaryDomainNetBiosName);
            domains.Add(primaryDomain);
            DomainController pdc = new DomainController(primaryDomain, PDCNetbiosName, PDCIPAddress, (ServerVersion)PDCOSVersion);
            DomainController sdc = new DomainController(primaryDomain, SDCNetbiosName, SDCIPAddress, (ServerVersion)SDCOSVersion);
            DomainController rodc = new DomainController(primaryDomain, RODCNetbiosName, RODCIPAddress, (ServerVersion)RODCOSVersion);
            domainControllers.Add(pdc);
            domainControllers.Add(sdc);
            domainControllers.Add(rodc);
            
            if (string.IsNullOrEmpty(ChildDomainDnsName))
            {
                Site.Log.Add(LogEntryKind.Warning, "ChildDomainDnsName is not configured in PTF, indicating the environment has no child domain.");
            }
            else
            {
                Domain childDomain = new Domain(ChildDomainDnsName);
                DomainController cdc = new DomainController(childDomain, CDCNetbiosName, CDCIPAddress, (ServerVersion)CDCOSVersion);
                domains.Add(childDomain);
                domainControllers.Add(cdc);
            }

            if (string.IsNullOrEmpty(TrustDomainDnsName))
            {
                Site.Log.Add(LogEntryKind.Warning, "TrustDomainDnsName is not configured in PTF, indicating the environment has no trusted domain.");
            }
            else
            {
                Domain trustDomain = new Domain(TrustDomainDnsName);
                DomainController tdc = new DomainController(trustDomain, TDCNetbiosName, TDCIPAddress, (ServerVersion)TDCOSVersion);
                domains.Add(trustDomain);
                domainControllers.Add(tdc);
            }

            Computer endpoint = new Computer(primaryDomain, ENDPOINTNetbiosName, ENDPOINTIPAddress);
            domainMembers = new ComputerCollection();
            domainMembers.Add(endpoint);

            Site.Log.Add(LogEntryKind.Debug, "Construct common classes for domain, domain controller and endpoint completed.");
        }

        protected string GetProperty(string property, bool isMandatory = false)
        {
            string value = Site.Properties.Get(property);
            if(isMandatory && string.IsNullOrEmpty(value))
            {
                Site.Assume.Fail("Property {0} cannot be null.", property);
            }
            return value; 
        }

        protected bool GetBoolProperty(string property, bool isMandatory = false)
        {
            bool value;
            string temp = GetProperty(property, isMandatory);
            if (!bool.TryParse(temp, out value))
            {
                Site.Assume.Fail("Value {0} of property {1} cannot be parsed.", temp, property); 
            }
            return value;
        }

        protected T GetEnumProperty<T>(string property, bool IsMandatory = false) where T: struct
        {
            T value;
            string temp = GetProperty(property, IsMandatory);
            if(!Enum.TryParse<T>(temp, out value))
            {
                Site.Assume.Fail("Value {0} of property {1} cannot be parsed.", temp, property); 
            }
            return value;
        }

        protected int GetIntProperty(string property, bool isMandatory = false)
        {
            int value;
            string temp = GetProperty(property, isMandatory);
            if (!int.TryParse(temp, out value))
            {
                Site.Assume.Fail("Value {0} of property {1} cannot be parsed.", temp, property); 
            }
            return value;
        }

        protected double GetDoubleProperty(string property, bool isMandatory = false)
        {
            double value;
            string temp = GetProperty(property, isMandatory);
            if (!double.TryParse(temp, out value))
            {
                Site.Assume.Fail("Value {0} of property {1} cannot be parsed.", temp, property);
            }
            return value;
        }
		
		/// <summary>
        /// This method is to find the domain controller with its FQDN/NetBiosName provided
        /// </summary>
        /// <param name="fullDomainName">This parameter represents the FQDN/NetBiosName of the domain controller</param>
        /// <returns>The Domain Controller found</returns>
        public DomainController GetDomainController(string fullDomainName)
        {
            // only search the netbios name, if the input is fqdn, transfer to netbios
            string name = fullDomainName.Split('.')[0];
            return domainControllers.SingleOrDefault(x => String.Equals(x.NetbiosName, name, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// This method is to find the domain controller with its ip address provided
        /// </summary>
        /// <param name="ipaddress">This parameter represents the ip address of the domain controller</param>
        /// <returns>The Domain Controller found</returns>
        public DomainController GetDomainController(IPAddress ipaddress)
        {
            // only search the netbios name, if the input is fqdn, transfer to netbios
            return domainControllers.SingleOrDefault(x => x.IPAddress == ipaddress);
        }

        /// <summary>
        /// This method is to find the domain member with its FQDN/NetBiosName provided
        /// </summary>
        /// <param name="fullDomainName">This parameter represents the FQDN/NetBiosName of the domain member</param>
        /// <returns>The Domain Member found</returns>
        protected Computer GetDomainMember(string fullDomainName)
        {
            // only search the netbios name, if the input is fqdn, transfer to netbios
            string name = fullDomainName.Split('.')[0];
            return domainMembers.SingleOrDefault(x => String.Equals(x.NetbiosName, name, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Get current ADFamily version
        /// </summary>
        /// <returns>version number string</returns>
        public string GetInstalledTestSuiteVersion()
        {
            string RegistryPath = @"SOFTWARE\Microsoft\ProtocolTestSuites";
            string RegistryPath64 = @"SOFTWARE\Wow6432Node\Microsoft\ProtocolTestSuites";
            RegistryKey HKLM = Registry.LocalMachine;
            RegistryKey TestSuitesRegPath = Environment.Is64BitProcess ?
                HKLM.OpenSubKey(RegistryPath64) : HKLM.OpenSubKey(RegistryPath);

            string registryKeyName = TestSuitesRegPath.GetSubKeyNames()
                                                 .Where((s) => s.Contains("ADFamily"))
                                                 .FirstOrDefault();

            Match versionMatch = Regex.Match(registryKeyName, @"\d\.\d\.\d{4}\.\d");
            return versionMatch.Value;
        }
    }
}
