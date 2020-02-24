// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestManager.Detector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Net;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;

namespace Microsoft.Protocols.TestManager.RDPServerPlugin
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

    public class RDPValueDetector : IValueDetector
    {
        #region Private Types

        Configs config;

        private enum EnvironmentType
        {
            Domain,
            Workgroup,
        }

        private const string ServerDomain = "Server Domain";
        private const string ServerName = "Server Name";
        private const string ServerPort = "Server Port";
        private const string ServerUserName = "Server User Name";
        private const string ServerUserPassword = "Server User Password";
      
        #endregion Private Types

        #region Variables

        private EnvironmentType env = EnvironmentType.Workgroup;
        private DetectionInfo detectionInfo = new DetectionInfo();

        #endregion Variables

        #region Implemented IValueDetector

        /// <summary>
        /// Set selected test environment.
        /// </summary>
        /// <param name="Environment"></param>
        public void SelectEnvironment(string NetworkEnvironment)
        {
            Enum.TryParse<EnvironmentType>(NetworkEnvironment, out env);
        }

        /// <summary>
        /// Get the prerequisites for auto-detection.
        /// </summary>
        /// <returns>A instance of Prerequisites class.</returns>
        public Prerequisites GetPrerequisites()
        {
            config = new Configs();
            config.LoadDefaultValues();
            Prerequisites prereq = new Prerequisites()
            {
                Title = "Remote Desktop",
                Summary = "Please input the below info to detect SUT.",
                Properties = new Dictionary<string, List<string>>()
            };

            prereq.AddProperty(RDPValueDetector.ServerDomain, config.ServerDomain);
            prereq.AddProperty(RDPValueDetector.ServerName, config.ServerName);
            prereq.AddProperty(RDPValueDetector.ServerPort, config.ServerPort);
            prereq.AddProperty(RDPValueDetector.ServerUserName, config.ServerUserName);
            prereq.AddProperty(RDPValueDetector.ServerUserPassword, config.ServerUserPassword);            
        
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
            this.properties = properties;
            // Save the prerequisites set by user
            detectionInfo.SUTName = properties[ServerName];
            detectionInfo.DomainName = properties[ServerDomain];
            detectionInfo.UserName = properties[ServerUserName];
            detectionInfo.Port = properties[ServerPort];     
            return true;
        }

        /// <summary>
        /// Add Detection steps to the log when detecting
        /// </summary>
        public List<DetectingItem> GetDetectionSteps()
        {
            List<DetectingItem> DetectingItems = new List<DetectingItem>();
            DetectingItems.Add(new DetectingItem("Detect Client HostName", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Detect Target SUT IP Address", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Establish RDP Connection with SUT", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Check Specified features Support", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Check Specified Protocols Support", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Detect RDP Version", DetectingStatus.Pending, LogStyle.Default));
            return DetectingItems;
        }

        /// <summary>
        /// Run auto detection properly.
        /// </summary>
        /// <returns>Return true if the function succeeded.</returns>
        public bool RunDetection()
        {
            try
            {
                DetectorUtil.WriteLog("Detect Client HostName...");

                // set config if properties changed
                config.ServerName = properties[RDPValueDetector.ServerName];
                config.ServerDomain = properties[RDPValueDetector.ServerDomain];
                if (config.ServerDomain != null && config.ServerDomain.Length == 0)
                {
                    config.ServerDomain = config.ServerName;
                }
                config.ServerPort = properties[RDPValueDetector.ServerPort];
                config.ServerUserName = properties[RDPValueDetector.ServerUserName];
                config.ServerUserPassword = properties[RDPValueDetector.ServerUserPassword];
                config.ClientName = Dns.GetHostName();               

                DetectorUtil.WriteLog("Finished!", false, LogStyle.StepPassed);

                if (!DetectSUTIPAddress())
                {
                    return false;
                }

                using (var detector = new RDPDetector(detectionInfo))
                {
                    if (!detector.DetectRDPFeature(config))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                DetectorUtil.WriteLog(String.Format("RunDetection() threw exception: {0}", ex));
                return false;
            }
        }

        /// <summary>
        /// Get the detection result.
        /// </summary>
        /// <param name="propertiesDic">Dictionary which contains property information</param>
        /// <returns>Return true if the property information is successfully obtained.</returns>
        public bool GetDetectedProperty(out Dictionary<string, List<string>> propertiesDic)
        {
            propertiesDic = config.ToDictionary();
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

            #region RDP Version
            var rdpVersion = detectionInfo.Version;

            if (rdpVersion >= TS_UD_SC_CORE_version_Values.V2)
            {
                caseList.Add(CreateRule("RDP Version.RDP 70", true));
                caseList.Add(CreateRule("RDP Version.RDP 80", true));
                caseList.Add(CreateRule("RDP Version.RDP 81", true));
            }            

            #endregion RDP Version

            #region Protocols
            caseList.Add(CreateRule("Protocol.RDPBCGR", true));
            caseList.Add(CreateRule("Protocol.RDPEDYC", detectionInfo.IsSupportRDPEDYC));
            caseList.Add(CreateRule("Protocol.RDPEMT", detectionInfo.IsSupportRDPEMT));
            caseList.Add(CreateRule("Protocol.RDPELE", detectionInfo.IsSupportRDPELE));
            #endregion Protocols

            return caseList;
        }

        /// <summary>
        /// Get a summary of the detection result.
        /// </summary>
        /// <returns>Detection result.</returns>
        public object GetSUTSummary()
        {
            DetectionResultControl SUTSummaryControl = new DetectionResultControl();
            SUTSummaryControl.LoadDetectionInfo(detectionInfo);
            return SUTSummaryControl;
        }

        /// <summary>
        /// Get the list of properties that will be hidden in the configure page.
        /// </summary>
        /// <param name="rules">Selected rules.</param>
        /// <returns>The list of properties which will not be shown in the configure page.</returns>
        public List<string> GetHiddenProperties(List<CaseSelectRule> rules)
        {
            List<string> hiddenPropertiesList = new List<string>();

            // Hidden the following properties in RDP_ServerTestSuite.ptfconfig:
            // 1. TestName
            // 2. ProtocolName
            // 3. Version
            hiddenPropertiesList.AddRange(DetectorUtil.GetPropertiesByFile("RDP_ServerTestSuite.ptfconfig"));
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

        private bool DetectSUTIPAddress()
        {
            DetectorUtil.WriteLog("===== Detect Target SUT IP Address=====", true, LogStyle.Default);

            try
            {
                IPAddress address;
                //Detect SUT IP address by SUT name
                //If SUT name is an ip address, skip to resolve, use the ip address directly
                if (IPAddress.TryParse(detectionInfo.SUTName, out address))
                {
                    DetectorUtil.WriteLog("Finished", true, LogStyle.StepPassed);
                    return true;
                }
                else //DNS resolve the SUT IP address by SUT name
                {
                    IPAddress[] addList = Dns.GetHostAddresses(detectionInfo.SUTName);

                    if (null == addList)
                    {
                        DetectorUtil.WriteLog(string.Format("The SUT name {0} cannot be resolved.", detectionInfo.SUTName), true, LogStyle.Error);
                        DetectorUtil.WriteLog("Failed", true, LogStyle.StepFailed);
                        return false;
                    }
                    else
                    {
                        DetectorUtil.WriteLog(string.Format("The SUT name {0} can be resolved as :", addList.ToString()), true, LogStyle.Default);
                        DetectorUtil.WriteLog("Finished", true, LogStyle.StepPassed);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                DetectorUtil.WriteLog(ex.Message, true, LogStyle.StepFailed);
                DetectorUtil.WriteLog("Failed", true, LogStyle.StepFailed);
                return false;
            }
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
