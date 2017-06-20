// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.Protocols.TestManager.BranchCachePlugin;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestManager.Detector
{
    public class BranchCacheValueDetector : IValueDetector
    {
        private enum EnvironmentType
        {
            TestAgainstContentServer,
            TestAgainstHostedCacheServer,
        }

        private enum SelectCategory
        {
            SelectBySUT,
            SelectByVersion,
            SelectByTransport
        }

        #region Fields

        private DetectionInfo detectionInfo = new DetectionInfo();
        private EnvironmentType env = EnvironmentType.TestAgainstContentServer;

        private Logger logWriter = new Logger();

        private const string ContentServerTitle = "Content Server Name";
        private const string HostedCacheServerTitle = "HostedCache Server Name";

        private const string domainTitle = "Domain Name";
        private const string userTitle = "User Name";
        private const string passwordTitle = "Password";

        private const string contentTransportTitle = "Content Transport";

        private const string uncSharePathTitle = "UNC Share Path";

        #endregion

        void System.IDisposable.Dispose()
        {
        }

        #region Interface Required

        /// <summary>
        /// Select the test environment
        /// </summary>
        /// <param name="environment">The environment user selected</param>
        public void SelectEnvironment(string environment)
        {
            Enum.TryParse(environment, out env);
        }

        /// <summary>
        /// Get the prerequisites for auto-detection: Read the configuration values from .ptfconfig file into prerequisites.Properties
        /// </summary>
        /// <returns>A instance of Prerequisites class.</returns>
        public Prerequisites GetPrerequisites()
        {
            Prerequisites prerequisites = new Prerequisites();
            prerequisites.Title = "Pre-Detect Configure";
            prerequisites.Summary = "Please input below info to detect SUT.\r\nIf SUT is ContentServer, leave \"HostedCache Server Name\" as blank.";

            Dictionary<string, List<string>> propertiesDic = new Dictionary<string, List<string>>();

            #region Set Properties

            //Retrieve saved value from *.ptfconfig file
            string contentServerName = DetectorUtil.GetPropertyValue("ContentServerComputerName");
            string hostedCacheServerName = DetectorUtil.GetPropertyValue("HostedCacheServerComputerName");
            string domain = DetectorUtil.GetPropertyValue("DomainName");
            string user = DetectorUtil.GetPropertyValue("UserName");
            string password = DetectorUtil.GetPropertyValue("UserPassword");
            string contentTransport = DetectorUtil.GetPropertyValue("ContentTransport");
            string uncSharePath = DetectorUtil.GetPropertyValue("SharedFolderName");

            List<string> contentServerList = new List<string>();
            List<string> hostedCacheServerList = new List<string>();
            List<string> domainList = new List<string>();
            List<string> SUTList = new List<string>();
            List<string> userList = new List<string>();
            List<string> passwordList = new List<string>();
            List<string> transportList = new List<string>();
            List<string> uncSharePathList = new List<string>();

            if (string.IsNullOrWhiteSpace(contentServerName)
                || string.IsNullOrWhiteSpace(hostedCacheServerName)
                || string.IsNullOrWhiteSpace(domain)
                || string.IsNullOrWhiteSpace(user)
                || string.IsNullOrWhiteSpace(password)
                || string.IsNullOrWhiteSpace(contentTransport)
                || string.IsNullOrWhiteSpace(uncSharePath))
            {
                contentServerList.Add("ContentServer");
                hostedCacheServerList.Add("HostedCacheServer");
                domainList.Add("contoso.com");
                userList.Add("administrator");
                passwordList.Add("Password01!");
                transportList.Add("SMB2");
                uncSharePathList.Add("C:\\FileShare");
            }
            else
            {
                contentServerList.Add(contentServerName);
                hostedCacheServerList.Add(hostedCacheServerName);
                domainList.Add(domain);
                userList.Add(user);
                passwordList.Add(password);
                transportList.Add(contentTransport);
                uncSharePathList.Add(uncSharePath);
            }

            propertiesDic.Add(ContentServerTitle, contentServerList);
            propertiesDic.Add(HostedCacheServerTitle, hostedCacheServerList);
            propertiesDic.Add(domainTitle, domainList);
            propertiesDic.Add(userTitle, userList);
            propertiesDic.Add(passwordTitle, passwordList);
            propertiesDic.Add(contentTransportTitle, transportList);
            propertiesDic.Add(uncSharePathTitle, uncSharePathList);

            //Get the real environment DomainName
            string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;

            #endregion

            prerequisites.Properties = propertiesDic;

            return prerequisites;
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
            //Save the prerequisites set by user
            detectionInfo.ContentServerName = properties[ContentServerTitle];
            detectionInfo.HostedCacheServerName = properties[HostedCacheServerTitle];
            detectionInfo.Domain = properties[domainTitle];
            detectionInfo.UserName = properties[userTitle];
            detectionInfo.Password = properties[passwordTitle];
            detectionInfo.SelectedTransport = properties[contentTransportTitle];
            detectionInfo.FileShareLocation = properties[uncSharePathTitle];

            if (string.IsNullOrEmpty(detectionInfo.ContentServerName)
                || string.IsNullOrEmpty(detectionInfo.Domain)
                || string.IsNullOrEmpty(detectionInfo.UserName)
                || string.IsNullOrEmpty(detectionInfo.Password)
                || string.IsNullOrEmpty(detectionInfo.SelectedTransport)
                || string.IsNullOrEmpty(detectionInfo.FileShareLocation))
            {
                throw new Exception(string.Format(
                    "Following boxes should not be empty: {0} ContentServerName {1} Domain Name {2} User Name {3} Password {4} Content Transport {5} File Share Location",
                    Environment.NewLine, Environment.NewLine, Environment.NewLine, Environment.NewLine, Environment.NewLine));
            }

            //No other property needed
            return true;
        }

        /// <summary>
        /// Add Detection steps to the log shown when detecting
        /// </summary>
        public List<DetectingItem> GetDetectionSteps()
        {
            List<DetectingItem> DetectingItems = new List<DetectingItem>();
            DetectingItems.Add(new DetectingItem("Ping Target SUT", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Fetch Local Network Info", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Check the Credential", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Detect Platform Info", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Detect Hash Generation Info", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Detect BranchCache Version Info", DetectingStatus.Pending, LogStyle.Default));
            return DetectingItems;
        }

        /// <summary>
        /// Run property autodetection.
        /// </summary>
        /// <returns>Return true if the function is succeeded.</returns>
        public bool RunDetection()
        {
            logWriter.AddLog(LogLevel.Warning, "===== Start detecting =====", false);
            BranchCacheDetector detector = new BranchCacheDetector(
                logWriter, 
                detectionInfo.ContentServerName, 
                detectionInfo.HostedCacheServerName,
                new AccountCredential(detectionInfo.Domain, detectionInfo.UserName,detectionInfo.Password), 
                SecurityPackageType.Negotiate);

            try
            {
                //Terminate the whole detection if any exception happens in the following processes
                if (!PingSUT(detector))
                {
                    return false;
                }

                if (!DetectLocalNetworkInfo(detector))
                {
                    return false;
                }

                if (!CheckUsernamePassword(detector))
                {
                    return false;
                }

                //Do not interrupt auto-detection
                //Even if detect platform failed
                DetectPlatform(detector);

                if (!DetectShareInfo(detector))
                {
                    return false;
                }

                if (!DetectVersionInfo(detector))
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //PTM will catch this exception and display to the user with message box.
                throw new Exception(string.Format("Failed to do auto detection with exception {0}.", ex.Message), ex);
            }
            finally
            {
                CleanUpAfterDetection(detector.ClientList);
            }

            logWriter.AddLog(LogLevel.Warning, "===== End detecting =====");
            return true;
        }

        /// <summary>
        /// Get the detected result.
        /// </summary>
        /// <param name="propertiesDic">Properties</param>
        /// <returns>Return true if the property value is successfully got.</returns>
        public bool GetDetectedProperty(out Dictionary<string, List<string>> propertiesDic)
        {
            propertiesDic = new Dictionary<string, List<string>>();

            // SUT info
            //propertiesDic.Add("SutComputerName", new List<string>() { detectionInfo.targetSUT });
            propertiesDic.Add("ContentServerComputerName", new List<string>() { detectionInfo.ContentServerName });
            propertiesDic.Add("HostedCacheServerComputerName", new List<string>() { detectionInfo.HostedCacheServerName });

            //Credential info
            propertiesDic.Add("DomainName", new List<string>() { detectionInfo.Domain });
            propertiesDic.Add("UserName", new List<string>() { detectionInfo.UserName });
            propertiesDic.Add("UserPassword", new List<string>() { detectionInfo.Password });

            //Transport
            propertiesDic.Add("ContentTransport", new List<string>() { detectionInfo.SelectedTransport });

            //Force hash generation supported
            propertiesDic.Add("SupportWebsiteForcedHashGeneration", new List<string>() { detectionInfo.IsWebsiteForcedHashGenerationSupported.ToString() });
            propertiesDic.Add("SupportFileShareForcedHashGeneration", new List<string>() { detectionInfo.IsFileShareForcedHashGenerationSupported.ToString() });

            //Website local path info
            propertiesDic.Add("WebsiteLocalPath", new List<string>() { detectionInfo.WebsiteLocalPath });

            return true;
        }

        /// <summary>
        /// Get selected rules
        /// </summary>
        /// <returns>Selected rules</returns>
        public List<CaseSelectRule> GetSelectedRules()
        {
            List<CaseSelectRule> selectedRuleList = new List<CaseSelectRule>();

            selectedRuleList.Add(new CaseSelectRule() { Name = "SUT.ContentServer", Status = RuleStatus.Selected });
            selectedRuleList.Add(CreateRule("SUT.HostedCacheServer", !string.IsNullOrWhiteSpace(detectionInfo.HostedCacheServerName)));

            selectedRuleList.Add(CreateRule("BranchCacheVersion.BranchCacheV1", detectionInfo.VersionInformation.branchCacheVersion.HasFlag(BranchCacheVersion.BranchCacheVersion1)));
            selectedRuleList.Add(CreateRule("BranchCacheVersion.BranchCacheV2", detectionInfo.VersionInformation.branchCacheVersion.HasFlag(BranchCacheVersion.BranchCacheVersion2)));

            selectedRuleList.Add(CreateRule("Transport.PCCRTP", detectionInfo.SelectedTransport.Contains("PCCRTP")));
            selectedRuleList.Add(CreateRule("Transport.SMB2", detectionInfo.SelectedTransport.Contains("SMB2")));

            selectedRuleList.Add(new CaseSelectRule() { Name = "Priority.BVT", Status = RuleStatus.Selected });
            selectedRuleList.Add(new CaseSelectRule() { Name = "Priority.Non-BVT", Status = RuleStatus.Selected });

            return selectedRuleList;
        }

        /// <summary>
        /// Get SUT summary information
        /// </summary>
        /// <returns></returns>
        public object GetSUTSummary()
        {
            DetectionResultControl SUTSummaryControl = new DetectionResultControl();
            SUTSummaryControl.LoadDetectionInfo(detectionInfo);
            return SUTSummaryControl;
        }

        /// <summary>
        /// Get the list of properties that will be hidder in the configure page
        /// </summary>
        /// <param name="rules">The selected rules</param>
        /// <returns></returns>
        public List<string> GetHiddenProperties(List<CaseSelectRule> rules)
        {
            //TO be implemented if necessary
            return new List<string>();
        }

        /// <summary>
        /// Check configuration settings
        /// </summary>
        /// <param name="properties">The properties</param>
        /// <returns></returns>
        public bool CheckConfigrationSettings(Dictionary<string, string> properties)
        {
            //To be implemented if necessary
            return true;
        }

        #endregion

        #region Helper functions for Detecting SUT Info

        private bool PingSUT(BranchCacheDetector detector)
        {
            logWriter.AddLog(LogLevel.Warning, "===== Ping Target SUT =====");

            try
            {
                detectionInfo.ContentServerNetworkInformation = detector.PingTargetSUT(detector.ContentServerName);
                if (!string.IsNullOrEmpty(detector.HostedCacheServerName))
                {
                    detectionInfo.HostedCacheServerNetworkInfo = detector.PingTargetSUT(detector.HostedCacheServerName);
                }
            }
            catch (Exception ex)
            {
                logWriter.AddLog(LogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                throw new Exception(ex.Message);
            }

            if (detectionInfo.ContentServerNetworkInformation.SUTIpList.Count == 0
                ||!string.IsNullOrEmpty(detectionInfo.HostedCacheServerName)&&detectionInfo.HostedCacheServerNetworkInfo.SUTIpList.Count == 0)
            {
                logWriter.AddLog(LogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                return false;
            }

            logWriter.AddLog(LogLevel.Warning, "Success", false, LogStyle.StepPassed);
            logWriter.AddLineToLog(LogLevel.Information);
            return true;
        }

        private bool DetectLocalNetworkInfo(BranchCacheDetector detector)
        {
            logWriter.AddLog(LogLevel.Warning, "===== Fetch Network Info =====");

            try
            {
                detectionInfo.ContentServerNetworkInformation = detector.FetchLocalNetworkInfo(detectionInfo);
                if (!string.IsNullOrEmpty(detectionInfo.HostedCacheServerName))
                {
                    detectionInfo.HostedCacheServerNetworkInfo = detector.FetchLocalNetworkInfo(detectionInfo);
                }
            }
            catch (Exception ex)
            {
                logWriter.AddLog(LogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                logWriter.AddLineToLog(LogLevel.Information);
                logWriter.AddLog(LogLevel.Error, string.Format("Detect network info failed: {0} \r\nPlease check Target SUT", ex.Message));
            }

            if (detectionInfo.ContentServerNetworkInformation == null
                || (!string.IsNullOrEmpty(detectionInfo.HostedCacheServerName) && detectionInfo.HostedCacheServerNetworkInfo == null))
            {
                logWriter.AddLog(LogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                logWriter.AddLineToLog(LogLevel.Information);
                return false;
            }

            logWriter.AddLog(LogLevel.Warning, "Finished", false, LogStyle.StepPassed);
            logWriter.AddLineToLog(LogLevel.Information);

            logWriter.AddLog(LogLevel.Information, "ContentServer Network Info:");
            logWriter.AddLog(LogLevel.Information, "Available IP Address:");
            foreach (var item in detectionInfo.ContentServerNetworkInformation.SUTIpList)
            {
                logWriter.AddLog(LogLevel.Information, "\t" + item.ToString());
            }
            logWriter.AddLineToLog(LogLevel.Information);

            if (!string.IsNullOrEmpty(detectionInfo.HostedCacheServerName))
            {
                logWriter.AddLog(LogLevel.Information, "HostedCacheServer Network Info:");
                logWriter.AddLog(LogLevel.Information, "Available IP Address:");

                foreach (var item in detectionInfo.HostedCacheServerNetworkInfo.SUTIpList)
                {
                    logWriter.AddLog(LogLevel.Information, "\t" + item.ToString());
                }
                logWriter.AddLineToLog(LogLevel.Information);
            }

            logWriter.AddLog(LogLevel.Information, "Local Test Driver Network Info:");
            logWriter.AddLog(LogLevel.Information, "Available IP Address:");
            foreach (var item in detectionInfo.ContentServerNetworkInformation.LocalIpList)
            {
                logWriter.AddLog(LogLevel.Information, "\t" + item.ToString());
            }
            logWriter.AddLineToLog(LogLevel.Information);

            return true;
        }

        private bool CheckUsernamePassword(BranchCacheDetector detector)
        {
            logWriter.AddLog(LogLevel.Warning, "===== Check the Credential =====");

            try
            {
                detector.CheckUsernamePassword(detectionInfo);
            }
            catch (Exception ex)
            {
                logWriter.AddLog(LogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                logWriter.AddLineToLog(LogLevel.Information);
                logWriter.AddLog(LogLevel.Error, string.Format("The User cannot log on:{0} \r\nPlease check the credential", ex.Message));
            }

            logWriter.AddLog(LogLevel.Warning, "Finished", false, LogStyle.StepPassed);
            logWriter.AddLineToLog(LogLevel.Information);

            return true;
        }

        private void DetectPlatform(BranchCacheDetector detector)
        {
            logWriter.AddLog(LogLevel.Warning, "===== Detect SUT Platform and Useraccounts =====");
            detector.FetchPlatform(detectionInfo);
            logWriter.AddLog(LogLevel.Warning, "Finished", false, LogStyle.StepPassed);
            logWriter.AddLineToLog(LogLevel.Information);
        }

        private bool DetectShareInfo(BranchCacheDetector detector)
        {
            logWriter.AddLog(LogLevel.Warning, "===== Fetch Share Info =====");

            try
            {
                detectionInfo.ShareInformation = detector.FetchShareInfo(detectionInfo);
            }
            catch (Exception ex)
            {
                logWriter.AddLog(LogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                logWriter.AddLineToLog(LogLevel.Information);
                logWriter.AddLog(LogLevel.Information, string.Format("FetchShareInfo failed, reason: {0}", ex.Message));
                logWriter.AddLog(LogLevel.Error, string.Format("Detect share info failed. Cannot do further detection.", ex.Message));
            }

            logWriter.AddLog(LogLevel.Warning, "Finished", false, LogStyle.StepPassed);
            logWriter.AddLineToLog(LogLevel.Information);

            if (detectionInfo.ShareInformation != null)
            {
                logWriter.AddLog(LogLevel.Information, "Target SUT Share Info:");

                logWriter.AddLog(LogLevel.Information, string.Format("Share Name: {0}", detectionInfo.ShareInformation.ShareName));
                logWriter.AddLog(LogLevel.Information, string.Format("\tContent Information Version: {0}", detectionInfo.ShareInformation.shareHashGeneration));

                logWriter.AddLineToLog(LogLevel.Information);
            }

            return true;
        }

        private bool DetectVersionInfo(BranchCacheDetector detector)
        {
            logWriter.AddLog(LogLevel.Warning, "===== Fetch Version Info =====");

            try
            {
                detectionInfo.VersionInformation = detector.FetchVersionInfo(detectionInfo);
            }
            catch (Exception ex)
            {
                logWriter.AddLog(LogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                logWriter.AddLineToLog(LogLevel.Information);
                logWriter.AddLog(LogLevel.Information, string.Format("FetchVersionInfo failed, reason: {0}", ex.Message));
                logWriter.AddLog(LogLevel.Error, string.Format("Detect version info failed. Cannot do further detection.", ex.Message));
            }

            logWriter.AddLog(LogLevel.Warning, "Finished", false, LogStyle.StepPassed);
            logWriter.AddLineToLog(LogLevel.Information);

            if (detectionInfo.VersionInformation != null)
            {
                logWriter.AddLog(LogLevel.Information, "Target SUT Share Info:");
                logWriter.AddLog(LogLevel.Information, string.Format("Content Information SUT supported: {0}", detectionInfo.VersionInformation));
                logWriter.AddLineToLog(LogLevel.Information);
            }
            return true;
        }

        private CaseSelectRule CreateRule(string ruleCategoryName, bool isSelected)
        {
            CaseSelectRule rule = null;
            rule = new CaseSelectRule() { Name = ruleCategoryName, Status = isSelected ? RuleStatus.Selected : RuleStatus.NotSupported };

            return rule;
        }

        private void CleanUpAfterDetection(List<Smb2Client> clientList)
        {
            if (clientList == null || clientList.Count == 0)
                return;
            else
            {
                foreach (var client in clientList)
                {
                    if (client.IsConnected)
                    {
                        client.Disconnect();
                        client.Dispose();
                    }
                }
            }

            clientList = null;
        }

        #endregion
    }   
}
