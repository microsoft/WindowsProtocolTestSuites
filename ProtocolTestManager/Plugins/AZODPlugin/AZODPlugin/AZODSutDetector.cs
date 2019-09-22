// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Protocols.TestManager.Detector;
using System.Windows.Controls;
using System.Net;
using Microsoft.Protocols.TestManager.Detector.Common;

namespace Microsoft.Protocols.TestManager.AZODPlugin
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
    /// MS-AZOD Test Suite SUT detector
    /// </summary>
    public class AZODSutDetector : IValueDetector
    {
        PtfConfig ptfcfg;
        string SharedPassword;

        private const string KdcAdminPwd = "Local domain admin password";
        private const string KdcDomainName = "Local domain Name";
        private const string KdcName = "Local domain DC name";
        private const string CrossForestDCName = "Cross domain DC name";
        private const string ApplicationServerName = "Local domain AP name";
        private const string CrossForestApplicationServerName = "Cross domain AP name";
        private const string ClientComputerName = "Client Computer";

        private Logger logWriter = new Logger("AZODPlugin_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + ".log");

        public AZODSutDetector()
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
            steps.AddStep("Verify DC01");
            steps.AddStep("Verify DC02");
            steps.AddStep("Verify Application server 01");
            steps.AddStep("Verify Application server 02");
            steps.AddStep("Verify Client/Driver computer");
            return steps;
        }

        public List<string> GetHiddenProperties(List<CaseSelectRule> rules)
        {
            return new List<string>();
        }

        public Prerequisites GetPrerequisites()
        {
            ptfcfg.LoadDefaultValues();
            Prerequisites prerequisites = new Prerequisites();
            prerequisites.Title = "Properties";
            prerequisites.Summary = "Properties required for checking domain test environment.";
            prerequisites.Properties = new Dictionary<string, List<string>>();
            prerequisites.AddProperty(AZODSutDetector.KdcAdminPwd, ptfcfg.KdcAdminPwd);
            prerequisites.AddProperty(AZODSutDetector.KdcDomainName, ptfcfg.KdcDomainName);
            prerequisites.AddProperty(AZODSutDetector.KdcName, ptfcfg.KdcName);
            prerequisites.AddProperty(AZODSutDetector.CrossForestDCName, ptfcfg.CrossForestDCName);
            prerequisites.AddProperty(AZODSutDetector.ApplicationServerName, ptfcfg.ApplicationServerName);
            prerequisites.AddProperty(AZODSutDetector.CrossForestApplicationServerName, ptfcfg.CrossForestApplicationServerName);
            prerequisites.AddProperty(AZODSutDetector.ClientComputerName, ptfcfg.ClientComputerName);
            return prerequisites;
        }

        public object GetSUTSummary()
        {
            SummaryBuilder summary = new SummaryBuilder(ptfcfg);
            WebBrowser wb = new WebBrowser();
            wb.NavigateToString(summary.GetHtml());
            return wb;
        }

        public bool RunDetection()
        {
            logWriter.AddLog("===== Start detecting =====", LogLevel.Normal, false);

            DomainInfo domainInfo = new DomainInfo()
            {
                Admin = ptfcfg.KdcAdminUser,
                AdminPassword = ptfcfg.KdcAdminPwd,
                Name = ptfcfg.KdcDomainName
            };

            //local dc01
            Server dc01 = new Server()
            {
                ComputerName = ptfcfg.KdcName,
                IPv4 = ptfcfg.KDCIP
            };
            if (!ServerHelper.DetectDC(domainInfo, dc01, logWriter))
            {
                return false;
            }

            var dc02 = new Server()
            {
                ComputerName = ptfcfg.KdcName,
                IPv4 = ptfcfg.KDCIP
            };
            if (!ServerHelper.DetectDC(domainInfo, dc02, logWriter))
            {
                return false;
            }

            var ap01 = new Server()
            {
                ComputerName = ptfcfg.ApplicationServerName,
                IPv4 = ptfcfg.ApplicationServerIP,
                FQDN = ptfcfg.ApplicationServerName
            };
            if (!DetectAP(domainInfo, ap01))
            {
                return false;
            }

            var ap02 = new Server()
            {
                ComputerName = ptfcfg.CrossForestApplicationServerName,
                IPv4 = ptfcfg.CrossForestApplicationServerIP,
                FQDN = ptfcfg.CrossForestApplicationServerName
            };

            if (!DetectAP(domainInfo, ap02))
            {
                return false;
            }

            ComputerInfo client = new ComputerInfo()
            {
                ComputerName = ptfcfg.ClientComputerName,
                IPv4 = ptfcfg.ClientComputerIp,
                Password = ptfcfg.ClientAdminPwd
            };
            //local client
            if (!ServerHelper.DetectClient(domainInfo, client, logWriter))
            {
                return false;
            }

            return true;
        }

        public List<CaseSelectRule> GetSelectedRules()
        {
            List<CaseSelectRule> rules = new List<CaseSelectRule>();

            rules.Add(new CaseSelectRule() { Name = "Priority.BVT", Status = RuleStatus.Selected });
            rules.Add(new CaseSelectRule() { Name = "Priority.Non-BVT", Status = RuleStatus.NotSupported });

            return rules;
        }

        public void SelectEnvironment(string NetworkEnvironment)
        {
        }

        public bool SetPrerequisiteProperties(Dictionary<string, string> properties)
        {
            SharedPassword = properties[AZODSutDetector.KdcAdminPwd];
            ptfcfg.KdcName = properties[AZODSutDetector.KdcName];
            ptfcfg.CrossForestDCName = properties[AZODSutDetector.CrossForestDCName];
            ptfcfg.ApplicationServerName = properties[AZODSutDetector.ApplicationServerName];
            ptfcfg.CrossForestApplicationServerName = properties[AZODSutDetector.CrossForestApplicationServerName];
            ptfcfg.ClientComputerName = properties[AZODSutDetector.ClientComputerName];
            return true;
        }

        private bool DetectAP(DomainInfo domain, Server ap)
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
                ap.FQDN = string.Format("{0}.{1}", hostname, domain.Name);
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

            logWriter.AddLog("Success", LogLevel.Normal, false, LogStyle.StepPassed);
            logWriter.AddLineToLog(LogLevel.Advanced);
            return true;
        }

        public void Dispose()
        {
        }
    }
}
