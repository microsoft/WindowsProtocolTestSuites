// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestManager.Detector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Net;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using System.ComponentModel;
using System.Management;

namespace Microsoft.Protocols.TestManager.WSPServerPlugin
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

    public class WSPValueDetector : IValueDetector
    {
        #region Private Types

        Logger logWriter = new Logger();
        private DetectionInfo detectionInfo = new DetectionInfo();
        
        private const string DomainName = "DomainName";
        private const string ServerComputerName = "ServerComputerName";
        private const string UserName = "UserName";
        private const string Password = "Password";
           
        private const string SharedPath = "SharedPath";
        private const string CatalogName = "CatalogName";
                   
        private const string IsWDSInstalled = "IsWDSInstalled";
        private const string IsServerWindows = "IsServerWindows";

        private const string LanguageLocale = "LanguageLocale";
        private const string LCID_VALUE = "LCID_VALUE";
        #endregion Private Types      

        #region Implemented IValueDetector

        /// <summary>
        /// Set selected test environment.
        /// </summary>
        /// <param name="Environment"></param>
        public void SelectEnvironment(string NetworkEnvironment)
        {
            return;
        }

        /// <summary>
        /// Get the prerequisites for auto-detection.
        /// </summary>
        /// <returns>A instance of Prerequisites class.</returns>
        public Prerequisites GetPrerequisites()
        {
            Configs config = new Configs();
            config.LoadDefaultValues();
            Prerequisites prereq = new Prerequisites()
            {
                Title = "MS-WSP",
                Summary = "Please input the below info to detect SUT.",
                Properties = new Dictionary<string, List<string>>()
            };

            prereq.AddProperty(DomainName, config.DomainName);
            prereq.AddProperty(ServerComputerName, config.ServerComputerName);
            prereq.AddProperty(UserName, config.UserName);
            prereq.AddProperty(Password, config.Password);
            prereq.AddProperty(SharedPath, config.SharedPath);
            prereq.AddProperty(CatalogName, config.CatalogName);

            prereq.AddProperty(IsServerWindows, config.IsServerWindows);
            prereq.AddProperty(IsWDSInstalled, config.IsWDSInstalled);
            prereq.AddProperty(LanguageLocale, config.LanguageLocale);
            prereq.AddProperty(LCID_VALUE, config.LCID_VALUE);
            
            return prereq;
        }

        private Dictionary<string, string> properties;
        /// <summary>
        /// Set the values for the required properties.
        /// </summary>
        /// <param name="properties">Property name and values.</param>
        /// <returns>
        /// Return true if no property is needed. Return false means there are
        /// other property required. PTF Tool will invoke GetPrerequisites and
        /// pop up a dialog to set the value of the properties.
        /// </returns>
        public bool SetPrerequisiteProperties(Dictionary<string, string> properties)
        {
            // Save the prerequisites set by user
            detectionInfo.DomainName = properties[DomainName];
            detectionInfo.ServerComputerName = properties[ServerComputerName];
            detectionInfo.UserName = properties[UserName];
            detectionInfo.Password = properties[Password];
            detectionInfo.CatalogName = properties[CatalogName];
            detectionInfo.SharedPath = properties[SharedPath];    

            detectionInfo.IsServerWindows = bool.Parse(properties[IsServerWindows]);
            detectionInfo.IsWDSInstalled = bool.Parse(properties[IsWDSInstalled]);

            detectionInfo.LanguageLocale = properties[LanguageLocale];
            detectionInfo.LCID_VALUE = properties[LCID_VALUE];
            this.properties = properties;

            // Check the validity of the inputs
            if (string.IsNullOrEmpty(detectionInfo.DomainName)
                || string.IsNullOrEmpty(detectionInfo.ServerComputerName)
                || string.IsNullOrEmpty(detectionInfo.UserName)
                || string.IsNullOrEmpty(detectionInfo.Password))
            {
                throw new Exception(string.Format(
                    "Following boxes should not be empty: {0} Domain Name, {1} Server Computer Name, {2} Server User Name or {3} Server User Password",
                    Environment.NewLine, Environment.NewLine, Environment.NewLine, Environment.NewLine));
            }          

            return true;
        }

        /// <summary>
        /// Add Detection steps to the log when detecting
        /// </summary>
        public List<DetectingItem> GetDetectionSteps()
        {
            List<DetectingItem> DetectingItems = new List<DetectingItem>();
            DetectingItems.Add(new DetectingItem("Detect Target SUT Connection", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Detect Targer SUT OS Version", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Check the Credential to Shared Path", DetectingStatus.Pending, LogStyle.Default));
            return DetectingItems;
        }

        /// <summary>
        /// Run auto detection properly.
        /// </summary>
        /// <returns>Return true if the function succeeded.</returns>
        public bool RunDetection()
        {
            WSPDetector detector = new WSPDetector(logWriter, detectionInfo);

            logWriter.AddLog(LogLevel.Information, "===== Start detecting =====");                   

            // Terminate the whole detection if any exception happens in the following processes
            if (!DetectSUTConnection(detector))
            {
                logWriter.AddLog(LogLevel.Error, "===== Detecting SUT Connection failed.=====");
                return false;
            }           

            if (!DetectPlatformInfo(detector))
            {
                logWriter.AddLog(LogLevel.Error, "===== Detecting SUT Platform Info failed.=====");
                return false;
            }

            if (!DetectShareInfo(detector))
            {
                logWriter.AddLog(LogLevel.Error, "===== Detecting SUT Share Info failed.=====");
                return false;
            }

            logWriter.AddLog(LogLevel.Information, "===== End detecting =====");
            return true;
        }

        /// <summary>
        /// Get the detection result.
        /// </summary>
        /// <param name="propertiesDic">Dictionary which contains property information</param>
        /// <returns>Return true if the property information is successfully obtained.</returns>
        public bool GetDetectedProperty(out Dictionary<string, List<string>> propertiesDic)
        {           
            propertiesDic = new Dictionary<string, List<string>>();
            propertiesDic.Add("ServerComputerName", new List<string>() { detectionInfo.ServerComputerName });
            propertiesDic.Add("DomainName", new List<string>() { detectionInfo.DomainName });
            propertiesDic.Add("UserName", new List<string>() { detectionInfo.UserName });
            propertiesDic.Add("Password", new List<string>() { detectionInfo.Password });
            propertiesDic.Add("ServerVersion", new List<string>() { detectionInfo.ServerVersion });
            propertiesDic.Add("SharedPath", new List<string>() { detectionInfo.SharedPath });
            propertiesDic.Add("ServerOffset", new List<string>() { detectionInfo.ServerOffset });
            propertiesDic.Add("ClientComputerName", new List<string>() { detectionInfo.ClientName });
            propertiesDic.Add("CatalogName", new List<string>() { detectionInfo.CatalogName });
            propertiesDic.Add("IsWDSInstalled", new List<string>() { detectionInfo.IsWDSInstalled.ToString() });
            propertiesDic.Add("ClientOffset", new List<string>() { detectionInfo.ClientOffset });
            propertiesDic.Add("ClientVersion", new List<string>() { detectionInfo.ClientVersion });
            propertiesDic.Add("IsServerWindows", new List<string>() { detectionInfo.IsServerWindows.ToString() });
            propertiesDic.Add("LanguageLocale", new List<string>() { detectionInfo.LanguageLocale});
            return true;
        }

        /// <summary>
        /// Get selected rules
        /// </summary>
        /// <returns>Selected rules</returns>
        public List<CaseSelectRule> GetSelectedRules()
        {
            List<CaseSelectRule> caseList = new List<CaseSelectRule>();

            caseList.Add(CreateRule("Priority.BVT", true));
            caseList.Add(CreateRule("Priority.NonBVT", true));

            #region Features

            #endregion Features

            return caseList;
        }

        /// <summary>
        /// Get a summary of the detection result.
        /// </summary>
        /// <returns>Detection result.</returns>
        public object GetSUTSummary()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Please confirm the test environment. ");
            sb.AppendLine("If they are not correct, please correct them on the Configure Test Cases page.");
            sb.AppendLine();
            sb.AppendLine(string.Format("Domain Name: {0}", detectionInfo.DomainName));
            sb.AppendLine(string.Format("Domain Administrator Username: {0}", detectionInfo.UserName));
            sb.AppendLine(string.Format("Domain Administrator Password: {0}", detectionInfo.Password));
            sb.AppendLine();
            sb.AppendLine(string.Format("Client Computer Name : {0}", detectionInfo.ClientName));
            sb.AppendLine(string.Format("Client Computer Operating System Version: {0}", detectionInfo.ClientVersion));
            sb.AppendLine(string.Format("Client Offset: {0}", detectionInfo.ClientOffset));
            sb.AppendLine();
            sb.AppendLine(string.Format("Server Computer Name : {0}", detectionInfo.ServerComputerName));
            sb.AppendLine(string.Format("Server Computer Operating System Version: {0}", detectionInfo.ServerVersion));
            sb.AppendLine(string.Format("Server Offset: {0}", detectionInfo.ServerOffset));
            sb.AppendLine();
            sb.AppendLine(string.Format("Is Search Service Installed: {0}", detectionInfo.IsWDSInstalled));
            sb.AppendLine(string.Format("Is SUT Windows Server: {0}", detectionInfo.IsServerWindows));
            sb.AppendLine(string.Format("Language Locale: {0}", detectionInfo.LanguageLocale));
            sb.AppendLine();
            return sb.ToString();
        }

        /// <summary>
        /// Get the list of properties that will be hidden in the configure page.
        /// </summary>
        /// <param name="rules">Selected rules.</param>
        /// <returns>The list of properties which will not be shown in the configure page.</returns>
        public List<string> GetHiddenProperties(List<CaseSelectRule> rules)
        {
            List<string> hiddenPropertiesList = new List<string>();

            // Hidden the following properties in MS-WSP_ServerTestSuite.ptfconfig:
            // 1. TestName
            // 2. ProtocolName
            // 3. Version
            hiddenPropertiesList.AddRange(DetectorUtil.GetPropertiesByFile("MS-WSP_ServerTestSuite.ptfconfig"));
            return hiddenPropertiesList;
        }

        /// <summary>
        /// Return false if check failed and set failed property in dictionary
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public bool CheckConfigrationSettings(Dictionary<string, string> properties)
        {
            return true;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
        }

        #endregion Implemented IValueDetector

        #region Private Methods
        private bool DetectSUTConnection(WSPDetector detector)
        {
            DetectorUtil.WriteLog("===== Detect Target SUT IP Address=====", true, LogStyle.Default);

            bool result = false;
            try
            {
                result= detector.DetectSUTConnection(ref detectionInfo);
                logWriter.AddLog(LogLevel.Information, "Finished", false, LogStyle.StepPassed);
            }
            catch (Exception ex)
            {
                logWriter.AddLog(LogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                logWriter.AddLog(LogLevel.Error, ex.ToString());
            }
            
            return result;
        }
        private bool CheckUsernamePassword(WSPDetector detector)
        {
            bool result = true;

            logWriter.AddLog(LogLevel.Information, "===== Check the Credential =====");

            try
            {
                result = detector.CheckUsernamePassword(detectionInfo);
                if (result)
                {
                    logWriter.AddLog(LogLevel.Information, "Finished", false, LogStyle.StepPassed);
                }
                else
                {
                    logWriter.AddLog(LogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                }
            }
            catch (Exception ex)
            {
                result = false;
                logWriter.AddLineToLog(LogLevel.Information);
                logWriter.AddLog(LogLevel.Error, string.Format("The User cannot log on:{0} \r\nPlease check the credential", ex.ToString()));
                logWriter.AddLog(LogLevel.Warning, "Failed", false, LogStyle.StepFailed);
            }
            return result;
        }

        private bool DetectPlatformInfo(WSPDetector detector)
        {
            bool result = false;
            logWriter.AddLog(LogLevel.Information, "===== Detect SUT Platform and Useraccounts =====");
            result = detector.FetchPlatformInfo(ref detectionInfo);
            if (result)
            {
                logWriter.AddLog(LogLevel.Information, "Finished", false, LogStyle.StepPassed);
            }
            else
            {
                logWriter.AddLog(LogLevel.Information, "Failed", false, LogStyle.StepFailed);
            }
            return result;
        }            
       
        private bool DetectShareInfo(WSPDetector detector)
        {
            logWriter.AddLog(LogLevel.Information, "===== Fetch Share Info =====");
            bool result = false;
            try
            {
                result = detector.FetchShareInfo(ref detectionInfo);
                if (result)
                {
                    logWriter.AddLog(LogLevel.Warning, "Finished", false, LogStyle.StepPassed);
                }
                else
                {
                    logWriter.AddLog(LogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                }
            }
            catch (Exception ex)
            {
                logWriter.AddLog(LogLevel.Error, string.Format("Detect share info failed. Cannot do further detection.", ex.ToString()));
                logWriter.AddLog(LogLevel.Warning, "Failed", false, LogStyle.StepFailed);
            }
                       
            return result;
        }

        private CaseSelectRule CreateRule(string ruleCategoryName, bool? isSupported)
        {
            CaseSelectRule rule = null;
            if (isSupported == null)
            {
                rule = new CaseSelectRule() { Name = ruleCategoryName, Status = RuleStatus.Unknown };
            }
            else
            {
                if (isSupported.Value)
                {
                    rule = new CaseSelectRule() { Name = ruleCategoryName, Status = RuleStatus.Selected };
                }
                else
                {
                    rule = new CaseSelectRule() { Name = ruleCategoryName, Status = RuleStatus.NotSupported };
                }
            }

            return rule;
        }

        private string NullableBoolToString(bool? value)
        {
            if (value == null)
            {
                return "false";
            }
            return value.Value.ToString().ToLower();
        }
        #endregion Private Methods
    } 
}
