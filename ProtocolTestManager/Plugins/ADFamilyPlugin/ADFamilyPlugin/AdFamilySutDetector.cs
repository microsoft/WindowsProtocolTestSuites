// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Protocols.TestManager.Detector;
using System.DirectoryServices.ActiveDirectory;
using System.Windows.Controls;

namespace Microsoft.Protocols.TestManager.ADFamilyPlugin
{
    public static class PluginExtention
    {
        public static void AddProperty(this Prerequisites prerequisite, string name, string value)
        {
            prerequisite.Properties.Add(name, new List<string>() { value });
        }

        public static void AddStep(this List<DetectingItem> step, string name)
        {
            step.Add(new DetectingItem(name, DetectingStatus.Pending, LogStyle.Default));
        }
    }
    /// <summary>
    /// AdFamily Test Suite SUT detector
    /// </summary>
    public class AdFamilySutDetector : IValueDetector
    {
        PtfConfig ptfcfg;
        bool SdcExists;
        bool RodcExists;
        bool DmExists;
        bool CdcExists;
        bool TdcExists;
        string SharedPassword;

        public AdFamilySutDetector()
        {
            ptfcfg = new PtfConfig();
        }

        public bool CheckConfigrationSettings(Dictionary<string, string> properties)
        {
            return true;
        }

        public bool GetDetectedProperty(out Dictionary<string, List<string>> propertiesDic)
        {
            propertiesDic = ptfcfg.ToDictionary();
            return true;
        }

        public List<DetectingItem> GetDetectionSteps()
        {
            List<DetectingItem> steps = new List<DetectingItem>();
            steps.AddStep("Verify PDC");
            steps.AddStep("Verify SDC");
            steps.AddStep("Verify RODC");
            steps.AddStep("Verify DM");
            steps.AddStep("Verify Child Domain");
            steps.AddStep("Verify Trust Domain");
            steps.AddStep("Verify User Accounts");
            steps.AddStep("Verify Machine Accounts");
            return steps;
        }

        public List<string> GetHiddenProperties(List<CaseSelectRule> rules)
        {
            return Utility.GetHiddenProperties(rules);
        }


        public Prerequisites GetPrerequisites()
        {
            ptfcfg.LoadDefaultValues();
            Prerequisites prerequisites = new Prerequisites();
            prerequisites.Title = "Properties";
            prerequisites.Summary = "Properties required for checking Active Directory test environment.";
            prerequisites.Properties = new Dictionary<string, List<string>>();
            prerequisites.AddProperty("Shared Password", ptfcfg.DomainUserPassword);
            prerequisites.AddProperty("Primary Domain", ptfcfg.PrimaryDomainDnsName);
            prerequisites.AddProperty("Primary Domain DC01", ptfcfg.Dc1NetBiosName);
            prerequisites.AddProperty("Primary Domain DC02", ptfcfg.Dc2NetbiosName);
            prerequisites.AddProperty("Primary Domain RODC", ptfcfg.RodcNetbiosName);
            prerequisites.AddProperty("Primary Domain Member", ptfcfg.DmNetbiosName);
            prerequisites.AddProperty("Child Domain", ptfcfg.ChildDomainDnsName);
            prerequisites.AddProperty("Trust Domain", ptfcfg.TrustDomainDnsName);
            return prerequisites;
        }

        public object GetSUTSummary()
        {
            SummaryBuilder summary = new SummaryBuilder(ptfcfg);
            WebBrowser wb = new WebBrowser();
            wb.NavigateToString(summary.GetHtml());
            return wb;
        }

        public List<CaseSelectRule> GetSelectedRules()
        {
            return Utility.GetRules(
                true,
                SdcExists,
                RodcExists,
                CdcExists,
                TdcExists,
                DmExists,
                !string.IsNullOrEmpty(ptfcfg.AdldsPort),
                int.Parse(ptfcfg.DomainFunctionLevel));
        }

        public bool RunDetection()
        {
            // Get Driver Information
            ptfcfg.EndpointNetbiosName = System.Environment.MachineName.ToUpper();
            var epHostEntry = Utility.GetHost(ptfcfg.EndpointNetbiosName);
            ptfcfg.EndpointIPAddress = epHostEntry.GetIpAddress();

            ptfcfg.NrpcSutSubnet = Utility.GetSubnetAddress(epHostEntry.GetAddress());

            // Check PDC
            DetectorUtil.WriteLog("Verify PDC - Begin", true, LogStyle.Default);
            var pdcHostEntry = Utility.GetHost(ptfcfg.Dc1NetBiosName);
            if (pdcHostEntry == null) throw new AutoDetectionException(string.Format("Cannot find {0}.", ptfcfg.Dc1NetBiosName));
            ptfcfg.Dc1IpAddress = pdcHostEntry.GetIpAddress();
            ptfcfg.Dc1NetBiosName = pdcHostEntry.GetNetbiosName().ToUpper();
            ptfcfg.LsatSutName = ptfcfg.Dc1NetBiosName;
            // Check ADLDS Port
            int ldsport;
            if (int.TryParse(ptfcfg.AdldsPort, out ldsport) && Utility.LdapPingHost(pdcHostEntry, ldsport))
            {
                DetectorUtil.WriteLog(
                    string.Format("Verify ADLDS Port:{0} - Done", ptfcfg.AdldsPort), true, LogStyle.Default);
            }
            else
            {
                DetectorUtil.WriteLog(
                    string.Format("Verify ADLDS Port:{0} - Failed", ptfcfg.AdldsPort), true, LogStyle.Default);
                ptfcfg.AdldsPort = "";
                ptfcfg.ClientUserName = "Administrator";
            }

            ptfcfg.LdsAppNc = "CN=ApplicationNamingContext," + String.Join(",", ptfcfg.PrimaryDomainDnsName.Split('.').Select(s => "DC=" + s));

            LdapSearchResponse pdcPingRep = null;
            try
            {
                pdcPingRep = Utility.LdapPing(ptfcfg.Dc1NetBiosName);
            }
            catch (Exception e)
            {
                DetectorUtil.WriteLog("Error: " + e.Message);
                DetectorUtil.WriteLog("Verify PDC - Failed", true, LogStyle.StepFailed);
                goto CheckSDC;
            }

            var d = Utility.GetDomainInfo(ptfcfg.Dc1NetBiosName, ptfcfg.PrimaryDomainDnsName, new System.Net.NetworkCredential(
                ptfcfg.DomainAdminName, SharedPassword, ptfcfg.PrimaryDomainDnsName));
            ptfcfg.PrimaryDomainServerGuid = d.DomainGuid;
            ptfcfg.PrimaryDomainSid = d.DomainSid;
            ptfcfg.PrimaryDomainNetBiosName = Utility.GetDomainNetbiosName(ptfcfg.PrimaryDomainDnsName);

            ptfcfg.ReplicaDirName = String.Format(@"\\{0}\SYSVOL\{0}\scripts", ptfcfg.PrimaryDomainDnsName);

            ptfcfg.DomainFunctionLevel = pdcPingRep.DomainFunctionality;
            string mechanisms = "";
            foreach (var s in pdcPingRep.SupportedSaslMechanisms) mechanisms += string.Format("{0}; ", s);
            DetectorUtil.WriteLog("Check PDC Supported SASL Mechanisms: " + mechanisms, true, LogStyle.Default);
            ptfcfg.PdcSupportedSaslMechanisms = mechanisms;
            if (!pdcPingRep.IsSupported("GSSAPI"))
            {
                throw new AutoDetectionException("ERROR: GSSAPI is not supported.");
            }
            if (!pdcPingRep.IsSupported("GSS-SPNEGO"))
            {
                throw new AutoDetectionException("ERROR: GSS-SPNEGO is not supported.");
            }
            DetectorUtil.WriteLog("Verify PDC - Done", true, LogStyle.StepPassed);

        CheckSDC:
            // Check SDC
            DetectorUtil.WriteLog("Verify SDC - Begin", true, LogStyle.Default);
            var sdcHostEntry = Utility.GetHost(ptfcfg.Dc2NetbiosName);
            if (sdcHostEntry == null || !Utility.LdapPingHost(sdcHostEntry))
            {
                DetectorUtil.WriteLog(string.Format("Secondary DC {0} is not found.", ptfcfg.Dc2NetbiosName), true, LogStyle.Default);
                SdcExists = false;
                ptfcfg.Dc2IpAddress = "";
                ptfcfg.Dc2NetbiosName = "";
                DetectorUtil.WriteLog("Verify SDC - Failed", true, LogStyle.StepNotFound);
            }
            else
            {
                SdcExists = true;
                ptfcfg.Dc2IpAddress = sdcHostEntry.GetIpAddress();
                ptfcfg.Dc2NetbiosName = sdcHostEntry.GetNetbiosName().ToUpper();
                DetectorUtil.WriteLog("Verify SDC - Done", true, LogStyle.StepPassed);
            }
            // Check RODC
            DetectorUtil.WriteLog("Verify RODC - Begin", true, LogStyle.Default);
            var rodcHostEntry = Utility.GetHost(ptfcfg.RodcNetbiosName);
            if (rodcHostEntry == null || !Utility.LdapPingHost(rodcHostEntry))
            {
                DetectorUtil.WriteLog(string.Format("Read Only DC {0} is not found.", ptfcfg.RodcNetbiosName), true, LogStyle.Default);
                RodcExists = false;
                ptfcfg.RodcIpAddress = "";
                ptfcfg.RodcNetbiosName = "";
                DetectorUtil.WriteLog("Verify RODC - Failed", true, LogStyle.StepNotFound);
            }
            else
            {
                RodcExists = true;
                ptfcfg.RodcIpAddress = rodcHostEntry.GetIpAddress();
                ptfcfg.RodcNetbiosName = rodcHostEntry.GetNetbiosName().ToUpper();
                DetectorUtil.WriteLog("Verify RODC - Done", true, LogStyle.StepPassed);
            }
            // Check DM
            DetectorUtil.WriteLog("Verify DM - Begin", true, LogStyle.Default);
            var dmHostEntry = Utility.GetHost(ptfcfg.DmNetbiosName);
            if (dmHostEntry == null)
            {
                DetectorUtil.WriteLog(string.Format("Domain Member {0} is not found.", ptfcfg.DmNetbiosName), true, LogStyle.Default);
                DmExists = false;
                ptfcfg.DmIpAddress = "";
                ptfcfg.DmNetbiosName = "";
                DetectorUtil.WriteLog("Verify DM - Failed", true, LogStyle.StepNotFound);
            }
            else
            {
                DmExists = true;
                ptfcfg.DmIpAddress = dmHostEntry.GetIpAddress();
                ptfcfg.DmNetbiosName = dmHostEntry.GetNetbiosName().ToUpper();
                DetectorUtil.WriteLog("Verify DM - Done", true, LogStyle.StepPassed);
            }
            // Check ChildDomain
            DetectorUtil.WriteLog("Verify Child Domain - Begin", true, LogStyle.Default);
            CdcExists = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(ptfcfg.ChildDomainDnsName))
                {
                    DirectoryContext domainContext = new DirectoryContext(DirectoryContextType.Domain, ptfcfg.ChildDomainDnsName);
                    var domain = System.DirectoryServices.ActiveDirectory.Domain.GetDomain(domainContext);
                    var cdc = domain.FindDomainController();
                    var cdcHostEntry = Utility.GetHost(cdc.Name);
                    ptfcfg.CdcIpAddress = cdcHostEntry.GetIpAddress();
                    ptfcfg.CdcNetbiosName = cdcHostEntry.GetNetbiosName().ToUpper();
                    ptfcfg.ChildDomainNetBiosName = Utility.GetDomainNetbiosName(ptfcfg.ChildDomainDnsName);
                    CdcExists = true;
                }
            }
            catch (Exception)
            {
                CdcExists = false;
            }
            if (CdcExists)
            {
                DetectorUtil.WriteLog("Verify Child Domain - Done", true, LogStyle.StepPassed);
            }
            else
            {
                ptfcfg.ChildDomainDnsName = "";
                ptfcfg.CdcIpAddress = "";
                ptfcfg.CdcNetbiosName = "";
                DetectorUtil.WriteLog("Verify Child Domain - Failed", true, LogStyle.StepNotFound);
            }
            // Check Trust
            DetectorUtil.WriteLog("Verify Trust Domain - Begin", true, LogStyle.Default);
            TdcExists = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(ptfcfg.TrustDomainDnsName))
                {
                    DirectoryContext domainContext = new DirectoryContext(DirectoryContextType.Domain, ptfcfg.TrustDomainDnsName);
                    var domain = System.DirectoryServices.ActiveDirectory.Domain.GetDomain(domainContext);
                    var tdc = domain.FindDomainController();
                    var tdcHostEntry = Utility.GetHost(tdc.Name);
                    ptfcfg.TdcIpAddress = tdcHostEntry.GetIpAddress();
                    ptfcfg.TdcNetbiosName = tdcHostEntry.GetNetbiosName().ToUpper();
                    ptfcfg.TrustDomainNetBiosName = Utility.GetDomainNetbiosName(ptfcfg.TrustDomainDnsName);
                    TdcExists = true;
                }
            }
            catch (Exception)
            {
                TdcExists = false;
            }
            if (TdcExists)
            {
                DetectorUtil.WriteLog("Verify Trust Domain - Done", true, LogStyle.StepPassed);
            }
            else
            {
                ptfcfg.TrustDomainDnsName = "";
                ptfcfg.TdcNetbiosName = "";
                ptfcfg.TdcIpAddress = "";
                DetectorUtil.WriteLog("Verify Trust Domain - Not Found", true, LogStyle.StepNotFound);
            }

            // Check User Accounts
            DetectorUtil.WriteLog("Verify User Accounts - Begin", true, LogStyle.Default);
            DetectorUtil.WriteLog(
                string.Format("Verify {0}\\{1} Password: {2} on {3}", ptfcfg.PrimaryDomainDnsName, ptfcfg.DomainAdminName, ptfcfg.DomainUserPassword, ptfcfg.Dc1NetBiosName),
                true, LogStyle.Default);
            Utility.LdapBind(ptfcfg.Dc1NetBiosName, new System.Net.NetworkCredential(ptfcfg.DomainAdminName, SharedPassword, ptfcfg.PrimaryDomainDnsName));
            ptfcfg.DomainUserPassword = SharedPassword;

            DetectorUtil.WriteLog(
                string.Format("Verify {0}\\{1} Password: {2} on {3}", ptfcfg.PrimaryDomainDnsName, ptfcfg.ClientUserName, ptfcfg.DomainUserPassword, ptfcfg.Dc1NetBiosName),
                true, LogStyle.Default);
            Utility.LdapBind(ptfcfg.Dc1NetBiosName, new System.Net.NetworkCredential(ptfcfg.ClientUserName, SharedPassword, ptfcfg.PrimaryDomainDnsName));
            ptfcfg.ClientUserPassword = SharedPassword;

            if (CdcExists)
            {
                DetectorUtil.WriteLog(
                string.Format("Verify {0}\\{1} Password: {2} on {3}", ptfcfg.ChildDomainDnsName, ptfcfg.DomainAdminName, ptfcfg.DomainUserPassword, ptfcfg.CdcNetbiosName),
                true, LogStyle.Default); 
                Utility.LdapBind(ptfcfg.CdcNetbiosName, new System.Net.NetworkCredential(ptfcfg.DomainAdminName, ptfcfg.DomainUserPassword, ptfcfg.ChildDomainDnsName));
            
            }
            if (TdcExists)
            {
                DetectorUtil.WriteLog(
                string.Format("Verify {0}\\{1} Password: {2} on {3}", ptfcfg.TrustDomainDnsName, ptfcfg.DomainAdminName, ptfcfg.DomainUserPassword, ptfcfg.TdcNetbiosName),
                true, LogStyle.Default);
                Utility.LdapBind(ptfcfg.TdcNetbiosName, new System.Net.NetworkCredential(ptfcfg.DomainAdminName, ptfcfg.DomainUserPassword, ptfcfg.TrustDomainDnsName));

            }
            DetectorUtil.WriteLog("Verify User Accounts - Done", true, LogStyle.StepPassed);
            // Check Machine Accounts

            DetectorUtil.WriteLog("Verify machine accounts - Begin.", true, LogStyle.Default);
            // PDC
            if (VerifyMachineAccount(ptfcfg.PrimaryDomainDnsName, ptfcfg.Dc1NetBiosName, ptfcfg.Dc1Password))
            {
                ptfcfg.Dc1PasswordVerified = true;
            }
            else if (ptfcfg.Dc1Password != SharedPassword && VerifyMachineAccount(ptfcfg.PrimaryDomainDnsName, ptfcfg.Dc1NetBiosName, SharedPassword))
            {
                ptfcfg.Dc1PasswordVerified = true;
                ptfcfg.Dc1Password = SharedPassword;
            }
            else
            {
                ptfcfg.Dc1PasswordVerified = false;
            }

            //SDC
            ptfcfg.Dc2PasswordVerified = false;
            if (SdcExists)
            {
                if (VerifyMachineAccount(ptfcfg.PrimaryDomainDnsName, ptfcfg.Dc2NetbiosName, ptfcfg.Dc2Password))
                {
                    ptfcfg.Dc2PasswordVerified = true;
                }
                else if (ptfcfg.Dc2Password != SharedPassword && VerifyMachineAccount(ptfcfg.PrimaryDomainDnsName, ptfcfg.Dc2NetbiosName, SharedPassword))
                {
                    ptfcfg.Dc1PasswordVerified = true;
                    ptfcfg.Dc2Password = SharedPassword;
                }
            }
            //RODC
            ptfcfg.RodcPasswordVerified = false;
            if (RodcExists)
            {
                if (VerifyMachineAccount(ptfcfg.PrimaryDomainDnsName, ptfcfg.RodcNetbiosName, ptfcfg.RodcPassword))
                {
                    ptfcfg.RodcPasswordVerified = true;
                }
                else if (ptfcfg.RodcPassword != SharedPassword && VerifyMachineAccount(ptfcfg.PrimaryDomainDnsName, ptfcfg.RodcNetbiosName, SharedPassword))
                {
                    ptfcfg.RodcPasswordVerified = true;
                    ptfcfg.RodcPassword = SharedPassword;
                }
            }

            //DM
            ptfcfg.DmPasswordVerified = false;
            if (DmExists)
            {
                if (VerifyMachineAccount(ptfcfg.PrimaryDomainDnsName, ptfcfg.DmNetbiosName, ptfcfg.DmPassword))
                {
                    ptfcfg.DmPasswordVerified = true;
                }
                else if (ptfcfg.DmPassword != SharedPassword && VerifyMachineAccount(ptfcfg.PrimaryDomainDnsName, ptfcfg.DmNetbiosName, SharedPassword))
                {
                    ptfcfg.DmPasswordVerified = true;
                    ptfcfg.DmPassword = SharedPassword;
                }
            }
            //Endpoint
            if (VerifyMachineAccount(ptfcfg.PrimaryDomainDnsName, ptfcfg.EndpointNetbiosName, ptfcfg.EndpointPassword))
            {
                ptfcfg.EndpointPasswordVerified = true;
            }
            else if (ptfcfg.EndpointPassword != SharedPassword && VerifyMachineAccount(ptfcfg.PrimaryDomainDnsName, ptfcfg.EndpointNetbiosName, SharedPassword))
            {
                ptfcfg.EndpointPasswordVerified = true;
                ptfcfg.EndpointPassword = SharedPassword;
            }
            else
            {
                ptfcfg.EndpointPasswordVerified = false;
            }

            DetectorUtil.WriteLog("Verify machine accounts - Done.", true, LogStyle.StepPassed);

            int functionLevel;
            int.TryParse(ptfcfg.DomainFunctionLevel, out functionLevel);

            // Get TD Path
            string installDir = @"C:\MicrosoftProtocolTests\ADFamily\Server-Endpoint\" + Utility.GetInstalledTestSuiteVersion();
            StringBuilder tddocs = new StringBuilder();
            tddocs.AppendFormat(@"{0}\Data\Common-TD-XML\MS-ADA1\*,", installDir);
            tddocs.AppendFormat(@"{0}\Data\Common-TD-XML\MS-ADA2\*,", installDir);
            tddocs.AppendFormat(@"{0}\Data\Common-TD-XML\MS-ADA3\*,", installDir);
            tddocs.AppendFormat(@"{0}\Data\Common-TD-XML\MS-ADSC\*,", installDir);
            if (functionLevel <= 5)
            {
                tddocs.AppendFormat(@"{0}\Data\Win8-TD-XML\MS-ADSC\*,", installDir);
                tddocs.AppendFormat(@"{0}\Data\Win8-TD-XML\MS-ADA2\*", installDir);
            }
            else
            {
                tddocs.AppendFormat(@"{0}\Data\WinBlue-TD-XML\MS-ADSC\*,", installDir);
                tddocs.AppendFormat(@"{0}\Data\WinBlue-TD-XML\MS-ADA2\*", installDir);
            }
            ptfcfg.SchemaTdPath = tddocs.ToString();

            StringBuilder ldstddocs = new StringBuilder();
            ldstddocs.AppendFormat(@"{0}\Data\Common-TD-XML\MS-ADLS\*,", installDir);
            if (functionLevel <= 5)
            {
                ldstddocs.AppendFormat(@"{0}\Data\Win8-TD-XML\MS-ADLS\*", installDir);
            }
            else
            {
                ldstddocs.AppendFormat(@"{0}\Data\WinBlue-TD-XML\MS-ADLS\*", installDir);
            }
            ptfcfg.SchemaLdsTdPath = ldstddocs.ToString();

            ptfcfg.SchemaOpenXmlPath = String.Format(@"{0}\Data\Win2016-TD-XML\DS\*", installDir);
            ptfcfg.SchemaLdsOpenXmlPath = String.Format(@"{0}\Data\Win2016-TD-XML\LDS\*", installDir);

            return true;
        }

        public bool VerifyMachineAccount(string domain, string account, string password)
        {
            try
            {
                DetectorUtil.WriteLog(
                     string.Format("Verify machine account: {0}\\{1}$ Password: {2}", domain, account, password),
                     true, LogStyle.Default);
                Utility.LdapBind(domain, new System.Net.NetworkCredential(account + "$", password, domain));
                DetectorUtil.WriteLog(
                     string.Format("Machine account: {0}\\{1} is verified.", domain, account),
                     true, LogStyle.Default);
            }
            catch (Exception e)
            {
                DetectorUtil.WriteLog(
                        string.Format("Error. Message: {0}", e.Message),
                        true, LogStyle.Default);
                return false;
            }
            return true;
        }

        public void SelectEnvironment(string NetworkEnvironment)
        {
        }

        public bool SetPrerequisiteProperties(Dictionary<string, string> properties)
        {
            SharedPassword = properties["Shared Password"];
            ptfcfg.PrimaryDomainDnsName = properties["Primary Domain"];
            ptfcfg.Dc1NetBiosName = properties["Primary Domain DC01"];
            ptfcfg.Dc2NetbiosName = properties["Primary Domain DC02"];
            ptfcfg.RodcNetbiosName = properties["Primary Domain RODC"];
            ptfcfg.DmNetbiosName = properties["Primary Domain Member"];
            ptfcfg.ChildDomainDnsName = properties["Child Domain"];
            ptfcfg.TrustDomainDnsName = properties["Trust Domain"];
            return true;
        }

        public void Dispose()
        {
        }
    }
}
