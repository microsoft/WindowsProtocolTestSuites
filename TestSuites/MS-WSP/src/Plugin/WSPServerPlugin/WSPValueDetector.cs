// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Detector;
using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.WSPServerPlugin
{
    public class WSPValueDetector : IValueDetector
    {
        #region Private fields

        DetectLogger logWriter = new DetectLogger();
        private DetectionInfo detectionInfo = new DetectionInfo();
        private Configs configs = new Configs();

        #endregion   

        #region Implement IValueDetector

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
            Prerequisites prereq = new Prerequisites();

            prereq.Title = "MS-WSP";
            prereq.Summary = "Please input the below info to detect the server.";

            var properties = new Dictionary<string, List<string>>();
            AddProperty(properties, nameof(configs.ServerComputerName), configs.ServerComputerName);
            AddProperty(properties, nameof(configs.DomainName), configs.DomainName);
            AddProperty(properties, nameof(configs.UserName), configs.UserName);
            AddProperty(properties, nameof(configs.Password), configs.Password);
            properties.Add(nameof(configs.SupportedSecurityPackage), new List<string>() { "Negotiate", "Kerberos", "Ntlm" });
            AddProperty(properties, nameof(configs.ShareName), configs.ShareName);
            AddProperty(properties, nameof(configs.QueryPath), configs.QueryPath);
            AddProperty(properties, nameof(configs.CatalogName), configs.CatalogName);

            properties.Add(nameof(configs.IsServerWindows), new List<string> { "true", "false" });
            properties.Add(nameof(configs.IsWDSInstalled), new List<string> { "true", "false" });

            prereq.Properties = properties;

            return prereq;

            void AddProperty(Dictionary<string, List<string>> dic, string propertyName, string propertyValue)
            {
                dic.Add(propertyName, new List<string>() { propertyValue });
            }
        }

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
            detectionInfo.ServerComputerName = properties[nameof(detectionInfo.ServerComputerName)];
            detectionInfo.DomainName = properties[nameof(detectionInfo.DomainName)];
            detectionInfo.UserName = properties[nameof(detectionInfo.UserName)];
            detectionInfo.Password = properties[nameof(detectionInfo.Password)];
            detectionInfo.SupportedSecurityPackage = properties[nameof(detectionInfo.SupportedSecurityPackage)];

            detectionInfo.CatalogName = properties[nameof(detectionInfo.CatalogName)];
            detectionInfo.ShareName = properties[nameof(detectionInfo.ShareName)];
            detectionInfo.QueryPath = properties[nameof(detectionInfo.QueryPath)];

            detectionInfo.IsServerWindows = bool.Parse(properties[nameof(detectionInfo.IsServerWindows)]);
            detectionInfo.IsWDSInstalled = bool.Parse(properties[nameof(detectionInfo.IsWDSInstalled)]);

            // Check the validity of the inputs
            if (string.IsNullOrEmpty(detectionInfo.ServerComputerName)
                || string.IsNullOrEmpty(detectionInfo.UserName)
                || string.IsNullOrEmpty(detectionInfo.Password))
            {
                throw new InvalidOperationException($"Following prerequisite properties should not be empty: ServerComputerName, UserName or Password");
            }

            detectionInfo.QueryText = configs.QueryText;

            detectionInfo.ClientName = configs.ClientName;
            detectionInfo.ClientVersion = configs.ClientVersion;
            detectionInfo.ClientOffset = configs.ClientOffset;

            detectionInfo.UseServerGssToken = bool.Parse(configs.UseServerGssToken);
            detectionInfo.SMB2ClientTimeout = TimeSpan.FromSeconds(int.Parse(configs.SMB2ClientTimeout));

            return true;
        }

        /// <summary>
        /// Add Detection steps to the log when detecting
        /// </summary>
        public List<DetectingItem> GetDetectionSteps()
        {
            List<DetectingItem> detectingItems = new List<DetectingItem>();
            detectingItems.Add(new DetectingItem("Detect Target Server Connection", DetectingStatus.Pending, LogStyle.Default));
            detectingItems.Add(new DetectingItem("Check the Credential to Target Server", DetectingStatus.Pending, LogStyle.Default));
            detectingItems.Add(new DetectingItem("Detect Platform Info", DetectingStatus.Pending, LogStyle.Default));
            return detectingItems;
        }

        /// <summary>
        /// Run auto detection properly.
        /// </summary>
        /// <param name="context">Detection Context.</param>
        /// <returns>Return true if the function succeeded.</returns>
        public bool RunDetection(DetectContext context)
        {
            logWriter.ApplyDetectContext(context);

            logWriter.AddLog(DetectLogLevel.Information, "===== Start detecting =====");

            using var detector = new WspDetector(logWriter, detectionInfo);

            var detectionActions = new List<Func<WspDetector, bool>>();
            detectionActions.Add(DetectServerConnection);
            detectionActions.Add(CheckUserLogon);
            detectionActions.Add(DetectPlatformInfo);

            foreach (var func in detectionActions)
            {
                if (context.Token.IsCancellationRequested)
                {
                    logWriter.AddLog(DetectLogLevel.Information, "===== Cancel detecting =====");
                    return false;
                }

                if (!func(detector))
                {
                    return false;
                }
            }
            detectionActions.Clear();

            logWriter.AddLog(DetectLogLevel.Information, "===== End detecting =====");
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
            propertiesDic.Add(nameof(detectionInfo.ServerComputerName), new List<string>() { detectionInfo.ServerComputerName });
            propertiesDic.Add(nameof(detectionInfo.DomainName), new List<string>() { detectionInfo.DomainName });
            propertiesDic.Add(nameof(detectionInfo.UserName), new List<string>() { detectionInfo.UserName });
            propertiesDic.Add(nameof(detectionInfo.Password), new List<string>() { detectionInfo.Password });
            propertiesDic.Add(nameof(detectionInfo.SupportedSecurityPackage), new List<string>() { detectionInfo.SupportedSecurityPackage });

            propertiesDic.Add(nameof(detectionInfo.CatalogName), new List<string>() { detectionInfo.CatalogName });
            propertiesDic.Add(nameof(detectionInfo.ShareName), new List<string>() { detectionInfo.ShareName });
            propertiesDic.Add(nameof(detectionInfo.QueryPath), new List<string>() { detectionInfo.QueryPath });

            propertiesDic.Add(nameof(detectionInfo.ClientName), new List<string>() { detectionInfo.ClientName });
            propertiesDic.Add(nameof(detectionInfo.ClientVersion), new List<string>() { detectionInfo.ClientVersion });
            propertiesDic.Add(nameof(detectionInfo.ClientOffset), new List<string>() { detectionInfo.ClientOffset });

            propertiesDic.Add(nameof(detectionInfo.ServerVersion), new List<string>() { detectionInfo.ServerVersion });
            propertiesDic.Add(nameof(detectionInfo.ServerOffset), new List<string>() { detectionInfo.ServerOffset });

            propertiesDic.Add(nameof(detectionInfo.IsServerWindows), new List<string>() { detectionInfo.IsServerWindows.ToString() });
            propertiesDic.Add(nameof(detectionInfo.IsWDSInstalled), new List<string>() { detectionInfo.IsWDSInstalled.ToString() });
            propertiesDic.Add(nameof(detectionInfo.LanguageLocale), new List<string>() { detectionInfo.LanguageLocale });
            propertiesDic.Add(nameof(detectionInfo.LcidValue), new List<string>() { detectionInfo.LcidValue });

            propertiesDic.Add(nameof(detectionInfo.UseServerGssToken), new List<string>() { detectionInfo.UseServerGssToken.ToString() });
            propertiesDic.Add(nameof(detectionInfo.SMB2ClientTimeout), new List<string>() { ((int)detectionInfo.SMB2ClientTimeout.TotalSeconds).ToString() });

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

            #region Scenarios

            caseList.Add(CreateRule("Scenario.CPMCiState", true));
            caseList.Add(CreateRule("Scenario.CPMConnect", true));
            caseList.Add(CreateRule("Scenario.CPMCreateQuery", true));
            caseList.Add(CreateRule("Scenario.CPMDisconnect", true));
            caseList.Add(CreateRule("Scenario.CPMFetchValue", true));
            caseList.Add(CreateRule("Scenario.CPMFindIndices", true));
            caseList.Add(CreateRule("Scenario.CPMFreeCursor", true));
            caseList.Add(CreateRule("Scenario.CPMGetQueryStatus", true));
            caseList.Add(CreateRule("Scenario.CPMGetQueryStatusEx", true));
            caseList.Add(CreateRule("Scenario.CPMGetRows", true));
            caseList.Add(CreateRule("Scenario.CPMGetRowsetNotify", true));
            caseList.Add(CreateRule("Scenario.CPMGetScopeStatistics", true));
            caseList.Add(CreateRule("Scenario.CPMSetBindings", true));
            caseList.Add(CreateRule("Scenario.CPMSetScopePrioritization", true));
            caseList.Add(CreateRule("Scenario.WspMessageHeader", true));

            #endregion

            return caseList;
        }

        /// <summary>
        /// Get a summary of the detection result.
        /// </summary>
        /// <returns>Detection result.</returns>
        public List<ResultItemMap> GetSUTSummary()
        {
            var summary = new List<ResultItemMap>();

            var userInfo = new ResultItemMap() { Header = "Domain and User Info", Description = "Information of the doamin and the user used to access the share.", ResultItemList = new List<ResultItem>() };
            if (!string.IsNullOrEmpty(detectionInfo.DomainName))
            {
                AddResultItem(userInfo, $"{nameof(detectionInfo.DomainName)}: {detectionInfo.DomainName}", DetectResult.Supported);
            }
            AddResultItem(userInfo, $"{nameof(detectionInfo.UserName)}: {detectionInfo.UserName}", DetectResult.Supported);
            AddResultItem(userInfo, $"User{nameof(detectionInfo.Password)}: {detectionInfo.Password}", DetectResult.Supported);
            summary.Add(userInfo);

            var clientInfo = new ResultItemMap() { Header = "Client Info", Description = "Information of the WSP client.", ResultItemList = new List<ResultItem>() };
            AddResultItem(clientInfo, $"{nameof(detectionInfo.ClientName)}: {detectionInfo.ClientName}", DetectResult.Supported);
            AddResultItem(clientInfo, $"{nameof(detectionInfo.ClientVersion)}: {detectionInfo.ClientVersion}", DetectResult.Supported);
            AddResultItem(clientInfo, $"{nameof(detectionInfo.ClientOffset)}: {detectionInfo.ClientOffset}", DetectResult.Supported);
            summary.Add(clientInfo);

            var serverInfo = new ResultItemMap() { Header = "Server Info", Description = "Information of the WSP server.", ResultItemList = new List<ResultItem>() };
            AddResultItem(serverInfo, $"{nameof(detectionInfo.ServerComputerName)}: {detectionInfo.ServerComputerName}", DetectResult.Supported);
            AddResultItem(serverInfo, $"{nameof(detectionInfo.ServerVersion)}: {detectionInfo.ServerVersion}", DetectResult.Supported);
            AddResultItem(serverInfo, $"{nameof(detectionInfo.ServerOffset)}: {detectionInfo.ServerOffset}", DetectResult.Supported);
            summary.Add(serverInfo);

            var serviceDetail = new ResultItemMap() { Header = "Detail of WSP Service", Description = "Detail of the WSP service during the test case execution.", ResultItemList = new List<ResultItem>() };
            AddResultItem(serviceDetail, $"{nameof(detectionInfo.IsWDSInstalled)}: {detectionInfo.IsWDSInstalled}", DetectResult.Supported);
            AddResultItem(serviceDetail, $"{nameof(detectionInfo.IsServerWindows)}: {detectionInfo.IsServerWindows}", DetectResult.Supported);
            AddResultItem(serviceDetail, $"{nameof(detectionInfo.LanguageLocale)}: {detectionInfo.LanguageLocale}", DetectResult.Supported);
            AddResultItem(serviceDetail, $"{nameof(detectionInfo.LcidValue)}: {detectionInfo.LcidValue}", DetectResult.Supported);
            summary.Add(serviceDetail);

            return summary;
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
            // 2. FeatureName
            var properties = DetectorUtil.GetPropertiesByFile("MS-WSP_ServerTestSuite.ptfconfig");
            hiddenPropertiesList.AddRange(properties);
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

        #endregion

        #region Private methods

        private bool DetectServerConnection(WspDetector detector)
        {
            logWriter.AddLog(DetectLogLevel.Information, "===== Detect Target Server IP Address =====", true);

            bool result = false;
            try
            {
                result = detector.DetectServerConnection(ref detectionInfo);
                if (result)
                {
                    logWriter.AddLog(DetectLogLevel.Information, "Finished", false, LogStyle.StepPassed);
                }
                else
                {
                    logWriter.AddLog(DetectLogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                }
            }
            catch (Exception ex)
            {
                logWriter.AddLog(DetectLogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                logWriter.AddLog(DetectLogLevel.Error, ex.ToString());
            }

            return result;
        }

        private bool CheckUserLogon(WspDetector detector)
        {
            logWriter.AddLog(DetectLogLevel.Information, "===== Check the Credential =====");

            bool result = false;
            try
            {
                result = detector.CheckUserLogon(detectionInfo);
                if (result)
                {
                    logWriter.AddLog(DetectLogLevel.Information, "Finished", false, LogStyle.StepPassed);
                }
                else
                {
                    logWriter.AddLog(DetectLogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                }
            }
            catch (Exception ex)
            {
                logWriter.AddLog(DetectLogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                logWriter.AddLog(DetectLogLevel.Error, $"The User cannot log on: {ex} {Environment.NewLine}Please check the credential");
            }
            return result;
        }

        private bool DetectPlatformInfo(WspDetector detector)
        {
            logWriter.AddLog(DetectLogLevel.Information, "===== Detect Server Platform and User Accounts =====");

            bool result = false;
            try
            {
                result = detector.FetchPlatformInfo(ref detectionInfo);
                if (result)
                {
                    logWriter.AddLog(DetectLogLevel.Information, "Finished", false, LogStyle.StepPassed);
                }
                else
                {
                    logWriter.AddLog(DetectLogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                }
            }
            catch (Exception ex)
            {
                logWriter.AddLog(DetectLogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                logWriter.AddLog(DetectLogLevel.Error, $"The platfrom info cannot be fetched since there are exceptions thrown: {ex}");
            }
            return result;
        }

        private CaseSelectRule CreateRule(string ruleCategoryName, bool? isSupported)
        {
            CaseSelectRule rule;
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

        private void AddResultItem(ResultItemMap resultItemMap, string description, DetectResult result)
        {
            string imagePath = string.Empty;
            switch (result)
            {
                case DetectResult.Supported:
                    imagePath = "/WSPServerPlugin;component/Icons/supported.png";
                    break;
                case DetectResult.UnSupported:
                    imagePath = "/WSPServerPlugin;component/Icons/unsupported.png";
                    break;
                case DetectResult.DetectFail:
                    imagePath = "/WSPServerPlugin;component/Icons/undetected.png";
                    break;
                default:
                    break;
            }

            ResultItem item = new ResultItem() { DetectedResult = result, ImageUrl = imagePath, Name = description };
            resultItemMap.ResultItemList.Add(item);
        }

        #endregion Private Methods
    }
}
