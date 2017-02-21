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

namespace Microsoft.Protocols.TestManager.ADFSPIPPlugin
{
    public static class PluginExtention
    {
        public static void AddProperty(this Prerequisites prereq, string name, string value)
        {
            prereq.Properties.Add(name, new List<string>() { value });
        }

        public static void AddProperty(this Prerequisites prereq, string name, List<string> value)
        {
            prereq.Properties.Add(name, value);
        }

        public static void AddStep(this List<DetectingItem> step, string name)
        {
            step.Add(new DetectingItem(name, DetectingStatus.Pending, LogStyle.Default));
        }

        public static void AddRule(this List<CaseSelectRule> rules, string name, RuleStatus status)
        {
            rules.Add(new CaseSelectRule()
            {
                Name = name,
                Status = status
            });
        }
    }
    /// <summary>
    /// ADFSPIP Test Suite SUT detector
    /// </summary>
    public class ADFSPIPDetector : IValueDetector
    {
        PtfConfig ptfcfg;
        private Dictionary<string, string> properties;

        public ADFSPIPDetector()
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
            steps.AddStep("Verify Domain Account");
            steps.AddStep("Verify SUT Account");
            steps.AddStep("Ping ADFS DNS");
            steps.AddStep("Find ADFS Certs");
            steps.AddStep("Find Web App Certs");
            steps.AddStep("Ping Driver Machine");
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
            prerequisites.Summary = "Properties required for checking ADFSPIP test environment.";
            prerequisites.Properties = new Dictionary<string, List<string>>();
            prerequisites.AddProperty("Domain Name", ptfcfg.DomainName);
            prerequisites.AddProperty("Domain Username", ptfcfg.DomainUsername);
            prerequisites.AddProperty("Domain Password", ptfcfg.DomainPassword);

            prerequisites.AddProperty("SUT IP", ptfcfg.SutIPAddress);
            prerequisites.AddProperty("SUT Username", ptfcfg.SutUsername);
            prerequisites.AddProperty("SUT User Password", ptfcfg.SutPassword);

            prerequisites.AddProperty("ADFS DNS", ptfcfg.AdfsDns);
            prerequisites.AddProperty("Adfs Cert", ptfcfg.AdfsCert);
            prerequisites.AddProperty("Sign Cert", ptfcfg.AdfsSignCert);
            prerequisites.AddProperty("Encrypt Cert", ptfcfg.AdfsEncryptCert);
            prerequisites.AddProperty("ADFS Windows Version", new List<string>() { "2016", "2012R2" });

            prerequisites.AddProperty("Web App Cert", ptfcfg.WebAppCert);
            prerequisites.AddProperty("CARoot Cert", ptfcfg.WebAppClientCert);

            string driverIP = Utility.ExtractIP(ptfcfg.CommonDriverShareFolder);
            prerequisites.AddProperty("Driver Machine IP", driverIP);

            prerequisites.AddProperty("PFX Password", ptfcfg.CommonPfxPassword);
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
            List<CaseSelectRule> rules = new List<CaseSelectRule>();
            rules.AddRule("Test Scope.BVT", RuleStatus.Selected);
            rules.AddRule("Test Scope.Non BVT", RuleStatus.Selected);

            rules.AddRule("Scenario.Deployment", RuleStatus.Selected);
            rules.AddRule("Scenario.Management", RuleStatus.Selected);
            rules.AddRule("Scenario.Runtime", RuleStatus.Selected);

            rules.AddRule("SUT Function Level.Windows 2016", RuleStatus.Selected);
            rules.AddRule("SUT Function Level.Windows 2012R2", RuleStatus.Selected);
            return rules;
        }

        public bool RunDetection()
        {
            // Verify Domain Account
            string accountName = properties["Domain Username"].Split('\\')[1];
            Utility.VerifyDomainAccount(properties["Domain Name"], accountName, properties["Domain Password"], "Verify Domain Account");
            ptfcfg.DomainName = properties["Domain Name"];
            ptfcfg.DomainUsername = string.Format(@"contoso\{0}", accountName);
            ptfcfg.DomainPassword = properties["Domain Password"];

            // Verify SUT Account
            Utility.VerifyLocalAccount(properties["SUT IP"], properties["SUT Username"], properties["SUT User Password"], "Verify SUT Account");
            ptfcfg.SutIPAddress = properties["SUT IP"];
            ptfcfg.SutUsername = properties["SUT Username"];
            ptfcfg.SutPassword = properties["SUT User Password"];

            // Ping ADFS
            Utility.PingHost(ptfcfg.AdfsDns, "Ping ADFS DNS");
            ptfcfg.AdfsDns = properties["ADFS DNS"];

            // Find ADFS Certs
            Utility.VerifyCertExist(properties["Adfs Cert"], "Find ADFS Certs");
            Utility.VerifyCertExist(properties["Sign Cert"], "Find ADFS Certs");
            Utility.VerifyCertExist(properties["Encrypt Cert"], "Find ADFS Certs");
            ptfcfg.AdfsCert = properties["Adfs Cert"];
            ptfcfg.AdfsSignCert = properties["Sign Cert"];
            ptfcfg.AdfsEncryptCert = properties["Encrypt Cert"];
            ptfcfg.AdfsIsWin2016 = (properties["ADFS Windows Version"] == "2016") ? "true" : "false";

            // Find Web App Certs
            Utility.VerifyCertExist(properties["Web App Cert"], "Find ADFS Certs");
            Utility.VerifyCertExist(properties["CARoot Cert"], "Find ADFS Certs");
            ptfcfg.WebAppCert = properties["Web App Cert"];
            ptfcfg.WebAppClientCert = properties["CARoot Cert"];

            // Ping Driver Machine
            Utility.PingHost(properties["Driver Machine IP"], "Ping ADFS DNS");
            ptfcfg.CommonDriverShareFolder = string.Format(@"\\{0}\temp\", properties["Driver Machine IP"]);

            return true;
        }

        public void SelectEnvironment(string NetworkEnvironment)
        {
        }

        public bool SetPrerequisiteProperties(Dictionary<string, string> properties)
        {
            this.properties = properties;
            return true;
        }

        public void Dispose()
        {
        }
    }
}
