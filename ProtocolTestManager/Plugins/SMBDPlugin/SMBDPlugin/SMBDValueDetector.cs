// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestManager.SMBDPlugin;
using Microsoft.Protocols.TestManager.SMBDPlugin.Detector;
using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.Detector
{
    public class SMBDValueDetector : IValueDetector
    {
        private const string SUTNAME = "SUT Name";
        private const string DOMAINNAME = "Domain Name";
        private const string SUTUSERNAME = "SUT User Name";
        private const string SUTPASSWORD = "SUT Password";
        private const string ISWINDOWSIMPLEMENTATION = "Is Windows Implementation";
        private DetectionInfo detectionInfo = new DetectionInfo();

        /// <summary>
        /// return false if check failed and set failed property in dictionary
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public bool CheckConfigrationSettings(Dictionary<string, string> properties)
        {
            return true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Get the detect result.
        /// </summary>
        /// <param name="name">Property name</param>
        /// <param name="value">Property value</param>
        /// <returns>Return true if the property value is successfully got.</returns>
        public bool GetDetectedProperty(out Dictionary<string, List<string>> propertiesDic)
        {
            propertiesDic = new Dictionary<string, List<string>>();
            propertiesDic["ServerNonRNicIp"] = new List<string> { detectionInfo.ServerNonRdmaNICIPAddress };
            propertiesDic["ServerRNicIp"] = new List<string> { detectionInfo.ServerRdmaNICIPAddress };
            return true;
        }

        /// <summary>
        /// Adds Detection steps to the log shown when detecting
        /// </summary>
        public List<DetectingItem> GetDetectionSteps()
        {
            List<DetectingItem> DetectingItems = new List<DetectingItem>();
            DetectingItems.Add(new DetectingItem("Ping target SUT", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Check credential", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Check NICs of local computer", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Check NICs of target SUT", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Get OS version", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Connect to share(Non-RDMA)", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Connect to share(RDMA)", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Check RDMA capability", DetectingStatus.Pending, LogStyle.Default));
            return DetectingItems;
        }

        /// <summary>
        /// Gets the list of properties that will be hidden in the configure page.
        /// </summary>
        /// <param name="rules">Selected rules.</param>
        /// <returns>The list of properties which will not be shown in the configure page.</returns>
        public List<string> GetHiddenProperties(List<CaseSelectRule> rules)
        {
            var hiddenProperties = new List<string>();
            return hiddenProperties;
        }

        /// <summary>
        /// Get the prerequisites for auto-detection.
        /// </summary>
        /// <returns>A instance of Prerequisites class.</returns>
        public Prerequisites GetPrerequisites()
        {
            var prerequisites = new Prerequisites();
            prerequisites.Title = "MS-SMBD";
            prerequisites.Summary = "Please input the below info to detect SUT.\r\nIf SUT is in WORKGROUP, leave Domain Name blank.";

            // Properties
            prerequisites.Properties = new Dictionary<string, List<string>>();

            string sutName = DetectorUtil.GetPropertyValue("SutComputerName");
            prerequisites.Properties.Add(SUTNAME, new List<string> { sutName });

            string domainName = DetectorUtil.GetPropertyValue("DomainName");
            prerequisites.Properties.Add(DOMAINNAME, new List<string> { domainName });

            string sutUserName = DetectorUtil.GetPropertyValue("SutUserName");
            prerequisites.Properties.Add(SUTUSERNAME, new List<string> { sutUserName });

            string sutPassword = DetectorUtil.GetPropertyValue("SutPassword");
            prerequisites.Properties.Add(SUTPASSWORD, new List<string> { sutPassword });

            string shareFolder = DetectorUtil.GetPropertyValue("ShareFolder");
            prerequisites.Properties.Add("SUT Shared Folder", new List<string> { shareFolder });

            prerequisites.Properties.Add(ISWINDOWSIMPLEMENTATION, new List<string> { "True", "False" });

            return prerequisites;
        }

        public List<CaseSelectRule> GetSelectedRules()
        {
            throw new NotImplementedException();
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
        /// Runs property auto detection.
        /// </summary>
        /// <returns>Return true if the function is succeeded.</returns>
        public bool RunDetection()
        {
            var detector = new SMBDDetector(detectionInfo);

            detector.PingSUT();

            detector.CheckUsernamePassword();

            return true;
        }

        public void SelectEnvironment(string NetworkEnvironment)
        {
            throw new NotImplementedException();
        }

        public bool SetPrerequisiteProperties(Dictionary<string, string> properties)
        {
            detectionInfo.SUTName = properties[SUTNAME];
            detectionInfo.DomainName = properties[DOMAINNAME];
            detectionInfo.UserName = properties[SUTUSERNAME];
            detectionInfo.Password = properties[SUTPASSWORD];
            detectionInfo.IsWindowsImplementation = Boolean.Parse(properties[ISWINDOWSIMPLEMENTATION]);
            return true;
        }
    }
}
