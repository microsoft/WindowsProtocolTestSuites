// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Protocols.TestManager.Detector;
using System.DirectoryServices.ActiveDirectory;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using Microsoft.Win32;
using System.IO;

namespace Microsoft.Protocols.TestManager.ADODPlugin
{
    static class PluginHelper
    {
        public static void AddProperty(this Prerequisites prereq, string property, params string[] value)
        {
            prereq.Properties.Add(property, value.ToList());
        }

        public static void AddProperty(this Prerequisites prereq, string property, List<string> value)
        {
            prereq.Properties.Add(property, value);
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
    public class ADODDetector : IValueDetector
    {
        Configs config;

        public bool CheckConfigrationSettings(Dictionary<string, string> properties)
        {
            throw new NotImplementedException();
        }

        public bool GetDetectedProperty(out Dictionary<string, List<string>> propertiesDic)
        {
            propertiesDic = config.ToDictionary();
            return true;
        }

        List<DetectingItem> steps;

        public List<DetectingItem> GetDetectionSteps()
        {
            steps = new List<DetectingItem>();
            steps.Add(new DetectingItem("Check MMA Installation", DetectingStatus.Pending, LogStyle.Default));
            steps.Add(new DetectingItem("Ping Domain", DetectingStatus.Pending, LogStyle.Default));
            steps.Add(new DetectingItem("Ping Primary Domain Controller", DetectingStatus.Pending, LogStyle.Default));
            steps.Add(new DetectingItem("TCP connection", DetectingStatus.Pending, LogStyle.Default));
            steps.Add(new DetectingItem("Ping Client Computer", DetectingStatus.Pending, LogStyle.Default));
            steps.Add(new DetectingItem("Verify Offline Capture Path", DetectingStatus.Pending, LogStyle.Default));
            return steps;
        }

        public List<string> GetHiddenProperties(List<CaseSelectRule> rules)
        {
            return new List<string>();
        }

        public Prerequisites GetPrerequisites()
        {
            config = new Configs();
            config.LoadDefaultValues();
            Prerequisites prereq = new Prerequisites()
            {
                Title = "Check Environment",
                Summary = "Please configure the environment",
                Properties = new Dictionary<string, List<string>>()
            };

            prereq.AddProperty("Full Qualified Domain Name", config.FullDomainName);
            prereq.AddProperty("Domain Administrator Username", config.DomainAdminUsername);
            prereq.AddProperty("Domain Administrator Password", config.DomainAdminPwd);

            prereq.AddProperty("Primary DC Operating System", config.PDCOperatingSystem);
            prereq.AddProperty("Primary DC Computer Name", config.PDCComputerName);
            prereq.AddProperty("Primary DC IP Address", config.PDCIP);

            prereq.AddProperty("Client Computer Operating System", config.ClientOperatingSystem);
            prereq.AddProperty("Client Operating System Version", config.ClientOSVersion);
            prereq.AddProperty("Client Computer Name", config.ClientComputerName);
            prereq.AddProperty("Client IP Address", config.ClientIP);
            prereq.AddProperty("Client Administrator Username", config.ClientAdminUsername);
            prereq.AddProperty("Client Administrator Password", config.ClientAdminPwd);

            prereq.AddProperty("Enable Offline Capture Testing", bool.Parse(config.TriggerDisabled).ToString(), (!bool.Parse(config.TriggerDisabled)).ToString());
            prereq.AddProperty("Capture File Path", config.LocalCapFilePath);

            return prereq;
        }

        public object GetSUTSummary()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Please confirm the test environment. ");
            sb.AppendLine("If they are not correct, please correct them on the Configure Test Cases page.");
            sb.AppendLine();
            sb.AppendLine(string.Format("Full Qualified Domain Name: {0}", config.FullDomainName));
            sb.AppendLine(string.Format("Domain Administrator Username: {0}", config.DomainAdminUsername));
            sb.AppendLine(string.Format("Domain Administrator Password: {0}", config.DomainAdminPwd));
            sb.AppendLine();
            sb.AppendLine(string.Format("Primary DC Operating System: {0}", config.PDCOperatingSystem));
            sb.AppendLine(string.Format("Primary DC Computer Name: {0}", config.PDCComputerName));
            sb.AppendLine(string.Format("Primary DC IP Address: {0}", config.PDCIP));
            sb.AppendLine();
            sb.AppendLine(string.Format("Client Computer Operating System: {0}", config.ClientOperatingSystem));
            sb.AppendLine(string.Format("Client Operating System Version: {0}", config.ClientOSVersion));
            sb.AppendLine(string.Format("Client Computer Name: {0}", config.ClientComputerName));
            sb.AppendLine(string.Format("Client IP Address: {0}", config.ClientIP));
            sb.AppendLine(string.Format("Client Administrator Username: {0}", config.ClientAdminUsername));
            sb.AppendLine(string.Format("Client Administrator Password: {0}", config.ClientAdminPwd));
            sb.AppendLine();
            sb.AppendLine(string.Format("Enable Offline Capture Testing: {0}", config.TriggerDisabled));
            sb.AppendLine(string.Format("Capture File Path: {0}", config.LocalCapFilePath));
            sb.AppendLine();
            return sb.ToString();
        }

        public List<CaseSelectRule> GetSelectedRules()
        {
            List<CaseSelectRule> rules = new List<CaseSelectRule>();
            rules.AddRule("Select Category.BVT", RuleStatus.Selected);
            rules.AddRule("Select Category.non BVT", RuleStatus.Selected);
            return rules;
        }

        private string Pinghost(string hostname, string stepname)
        {
            Ping ping = new Ping();
            IPAddress[] ipAddr = null;
            uint actualIpIndex = 0;
            try
            {
                ipAddr = Dns.GetHostAddresses(hostname);
                foreach (var ip in ipAddr)
                {
                    // Break if current IP is not loopback addr (127.0.0.1 or ::1)
                    // Otherwise increment actualIpIndex
                    if (!IPAddress.IsLoopback(ip))
                    {
                        break;
                    }
                    actualIpIndex += 1;
                }
            }
            catch (SocketException e)
            {
                DetectorUtil.WriteLog(stepname + " failed!", false, LogStyle.StepFailed);
                throw new Exception(string.Format("Cannot resolve host name {0}.\r\n{1}", hostname, e.Message));
            }
            try
            {
                ping.Send(ipAddr[actualIpIndex]);
            }
            catch (PingException e)
            {
                DetectorUtil.WriteLog(stepname + " failed!", false, LogStyle.StepFailed);
                throw new Exception(string.Format("Ping {0} error.\r\n{1}", hostname, e.Message));
            }

            DetectorUtil.WriteLog(stepname + " successful.", false, LogStyle.StepPassed);
            return ipAddr[actualIpIndex].ToString();
        }

        private void IsMessageAnalyzerInstalled()
        {
            try
            {
                RegistryKey HKLM = Registry.LocalMachine;
                RegistryKey key = HKLM.OpenSubKey(@"SOFTWARE\Microsoft\MessageAnalyzer\Capabilities");

                string appDesc = key.GetValue("ApplicationDescription").ToString();
                string appName = key.GetValue("ApplicationName").ToString();

                if (appDesc.Equals("Microsoft Message Analyzer")
                    && appName.Equals("Microsoft Message Analyzer"))
                {
                    DetectorUtil.WriteLog("Check MMA Installation successful.", false, LogStyle.StepPassed);
                }
                else
                {
                    DetectorUtil.WriteLog(
                        string.Format("Check MMA Installation failed: ApplicationDescription={0}, ApplicationName={1}.", appDesc, appName),
                        false,
                        LogStyle.StepFailed);
                }
            }
            catch (Exception e)
            {
                DetectorUtil.WriteLog("Check MMA Installation failed: Message Analyzer not properly installed!", false, LogStyle.StepFailed);
                throw new Exception(string.Format("Message Analyzer not properly installed: {0}", e.Message));
            }
        }

        private void TcpTcpConnection(string ipAddr, int port)
        {
            try
            {
                using (TcpClient c = new TcpClient())
                {
                    c.Connect(ipAddr, port);
                    DetectorUtil.WriteLog(string.Format("TCP connection to {0}:{1} successful.", ipAddr, port), false, LogStyle.StepPassed);
                }
            }
            catch (Exception e)
            {
                DetectorUtil.WriteLog(string.Format("TCP connection to {0}:{1} failed!", ipAddr, port), false, LogStyle.StepFailed);
                throw new Exception(string.Format("TCP connection to {0}:{1} failed: {2}", ipAddr, port, e.Message));
            }
        }

        private void VerifyCapturePath(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    DetectorUtil.WriteLog(string.Format("Verify Offline Capture Path: {0} successful! The path exists.", path), false, LogStyle.StepPassed);
                }
                else
                {
                    DetectorUtil.WriteLog(string.Format("Verify Offline Capture Path: {0} failed! The Path doesn't exist.", path), false, LogStyle.StepFailed);
                }
            }
            catch (Exception e)
            {
                DetectorUtil.WriteLog(string.Format("Verify Offline Capture Path: {0} failed!", path), false, LogStyle.StepFailed);
                throw new Exception(string.Format("Verify Offline Capture Path: {0} failed: {1}", path, e.Message));
            }
        }

        public bool RunDetection()
        {
            // set config if properties changed
            config.FullDomainName = properties["Full Qualified Domain Name"];
            config.DomainAdminUsername = properties["Domain Administrator Username"];
            config.DomainAdminPwd = properties["Domain Administrator Password"];

            config.DomainNC = "DC=" + config.FullDomainName.Replace(".", ",DC=");
            config.PartitionsNC = "CN=Partitions,CN=Configuration,DC=" + config.FullDomainName.Replace(".", ",DC=");

            config.PDCOperatingSystem = properties["Primary DC Operating System"];
            config.PDCComputerName = properties["Primary DC Computer Name"];
            config.PDCIP = properties["Primary DC IP Address"];

            config.ClientOperatingSystem = properties["Client Computer Operating System"];
            config.ClientOSVersion = properties["Client Operating System Version"];
            config.ClientComputerName = properties["Client Computer Name"];
            config.ClientIP = properties["Client IP Address"];
            config.ClientAdminUsername = properties["Client Administrator Username"];
            config.ClientAdminPwd = properties["Client Administrator Password"];

            config.TriggerDisabled = properties["Enable Offline Capture Testing"];
            config.LocalCapFilePath = properties["Capture File Path"];

            // run detection and set config if any mis-settings on IP according to the computer name resolve result
            IsMessageAnalyzerInstalled();

            if (bool.Parse(config.TriggerDisabled))
            {
                DetectorUtil.WriteLog("Offline Capture Testing Enabled, Ping Domain Skipped.", false, LogStyle.StepSkipped);
                DetectorUtil.WriteLog("Offline Capture Testing Enabled, Ping Primary Domain Controller Skipped.", false, LogStyle.StepSkipped);
                DetectorUtil.WriteLog("Offline Capture Testing Enabled, Tcp Connection Check Skipped.", false, LogStyle.StepSkipped);
                DetectorUtil.WriteLog("Offline Capture Testing Enabled, Ping Client Computer Skipped.", false, LogStyle.StepSkipped);
                VerifyCapturePath(config.LocalCapFilePath);
            }
            else
            {
                Pinghost(config.FullDomainName, "Ping Domain");
                config.PDCIP = Pinghost(config.PDCComputerName, "Ping Primary Domain Controller");
                TcpTcpConnection(config.PDCIP, 389);
                config.ClientIP = Pinghost(config.ClientComputerName, "Ping Client Computer");
                DetectorUtil.WriteLog(string.Format("Offline Capture Testing Disabled, Verify Capture Path Skipped."), false, LogStyle.StepSkipped);
            }

            return true;
        }

        public void SelectEnvironment(string NetworkEnvironment)
        {

        }

        private Dictionary<string, string> properties;

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
