// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestManager.Detector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

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
        private const string ClientName = "Client Name";
        private const string RDPVersion = "RDP Version";

        #endregion Private Types

        #region Variables

        private EnvironmentType env = EnvironmentType.Workgroup;
        private DetectionInfo detectionInfo = new DetectionInfo();       

        #endregion Variables

        #region Implemented IValueDetector

        /// <summary>
        /// Sets selected test environment.
        /// </summary>
        /// <param name="Environment"></param>
        public void SelectEnvironment(string NetworkEnvironment)
        {
            Enum.TryParse<EnvironmentType>(NetworkEnvironment, out env);
        }

        /// <summary>
        /// Gets the prerequisites for auto-detection.
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
            prereq.AddProperty(RDPValueDetector.ClientName, config.ClientName);
            prereq.AddProperty(RDPValueDetector.RDPVersion, config.Version);

            return prereq;
        }

        private Dictionary<string, string> properties;
        /// <summary>
        /// Sets the values for the required properties.
        /// </summary>
        /// <param name="properties">Property name and values.</param>
        /// <returns>
        /// Return true if no other property needed. Return false means there are 
        /// other property required. PTF Tool will invoke GetPrerequisites and 
        /// pop up a dialog to set the value of the properties.
        /// </returns>
        public bool SetPrerequisiteProperties(Dictionary<string, string> properties)
        {
            this.properties = properties;
            return true;
        }

        /// <summary>
        /// Adds Detection steps to the log shown when detecting
        /// </summary>
        public List<DetectingItem> GetDetectionSteps()
        {
            List<DetectingItem> DetectingItems = new List<DetectingItem>();
            DetectingItems.Add(new DetectingItem("Ping Target SUT", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Establish RDP Connection with SUT", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Check Specified features Support", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Check Specified Protocols Support", DetectingStatus.Pending, LogStyle.Default));
            return DetectingItems;
        }

        /// <summary>
        /// Runs property autodetection.
        /// </summary>
        /// <returns>Return true if the function is succeeded.</returns>
        public bool RunDetection()
        {
            // set config if properties changed
            config.ServerDomain = properties[RDPValueDetector.ServerDomain];
            config.ServerName = properties[RDPValueDetector.ServerName];
            config.ServerPort = properties[RDPValueDetector.ServerPort];

            config.ServerUserName = properties[RDPValueDetector.ServerUserName];
            config.ServerUserPassword = properties[RDPValueDetector.ServerUserPassword];
            config.ClientName = properties[RDPValueDetector.ClientName];
            config.Version = properties[RDPValueDetector.RDPVersion];

            if (!PingSUT())
            {
                return false;
            }

            RDPDetector detector = new RDPDetector(detectionInfo);
            if (!detector.DetectRDPFeature())
            {
                detector.Dispose();
                return false;
            }
            detector.Dispose();
            return true;
        }

        /// <summary>
        /// Gets the detect result.
        /// </summary>
        /// <param name="name">Property name</param>
        /// <param name="value">Property value</param>
        /// <returns>Return true if the property value is successfully got.</returns>
        public bool GetDetectedProperty(out Dictionary<string, List<string>> propertiesDic)
        {
            propertiesDic = config.ToDictionary();
            return true;
        }

        /// <summary>
        /// Gets selected rules
        /// </summary>
        /// <returns>Selected rules</returns>
        public List<CaseSelectRule> GetSelectedRules()
        {
            List<CaseSelectRule> caseList = new List<CaseSelectRule>();

            caseList.Add(CreateRule("Priority.BVT", true));
            caseList.Add(CreateRule("Priority.NonBVT", true));

            #region Protocols

            caseList.Add(CreateRule("Protocol.RDPBCGR", true));

            #endregion Protocols

            caseList.Add(CreateRule("Specific Requirements.DeviceNeeded", false));
            caseList.Add(CreateRule("Specific Requirements.Interactive", false));

            return caseList;
        }

        /// <summary>
        /// Gets a summary of the detect result.
        /// </summary>
        /// <returns>Detect result.</returns>
        public object GetSUTSummary()
        {
            DetectionResultControl SUTSummaryControl = new DetectionResultControl();
            SUTSummaryControl.LoadDetectionInfo(detectionInfo);
            return SUTSummaryControl;
        }

        /// <summary>
        /// Gets the list of properties that will be hidder in the configure page.
        /// </summary>
        /// <param name="rules">Selected rules.</param>
        /// <returns>The list of properties whick will not be shown in the configure page.</returns>
        public List<string> GetHiddenProperties(List<CaseSelectRule> rules)
        {
            return new List<string>();
        }

        /// <summary>
        /// Returns false if check failed and set failed property in dictionary
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

        private bool PingSUT()
        {
            DetectorUtil.WriteLog("Ping Target SUT...");

            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            // Use the default TtL value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "0123456789ABCDEF0123456789ABCDEF";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 5000;
            bool result = false;
            List<PingReply> replys = new List<PingReply>();
            try
            {
                for (int i = 0; i < 4; i++)
                {
                    replys.Add(pingSender.Send(config.ServerName, timeout, buffer, options));
                }

            }
            catch
            {
                DetectorUtil.WriteLog("Error", false, LogStyle.Error);

                //return false;
                throw;
            }
            foreach (var reply in replys)
            {

                result |= (reply.Status == IPStatus.Success);
            }
            if (result)
            {
                DetectorUtil.WriteLog("Passed", false, LogStyle.StepPassed);
                return true;
            }
            else
            {
                DetectorUtil.WriteLog("Failed", false, LogStyle.StepFailed);
                DetectorUtil.WriteLog("Taget SUT don't respond.");
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
