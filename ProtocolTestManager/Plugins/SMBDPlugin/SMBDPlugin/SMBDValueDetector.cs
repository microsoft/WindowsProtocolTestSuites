// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rsvd;
using System.Net;
using System.Reflection;
using System.Net.NetworkInformation;
using Microsoft.Protocols.TestManager.SMBDPlugin;
using System.Windows;
using System.ComponentModel;

namespace Microsoft.Protocols.TestManager.Detector
{
    public class SMBDValueDetector : IValueDetector
    {
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

        public bool GetDetectedProperty(out Dictionary<string, List<string>> propertiesDic)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds Detection steps to the log shown when detecting
        /// </summary>
        public List<DetectingItem> GetDetectionSteps()
        {
            List<DetectingItem> DetectingItems = new List<DetectingItem>();
            DetectingItems.Add(new DetectingItem("Ping Target SUT", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Check Credential", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Get OS Version", DetectingStatus.Pending, LogStyle.Default));
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
            prerequisites.Properties.Add("SUT Name", new List<string> { sutName });

            string serverNonRNicIp = DetectorUtil.GetPropertyValue("ServerNonRNicIp");
            prerequisites.Properties.Add("Server Non-RDMA NIC IP Address", new List<string> { serverNonRNicIp });

            string serverRNicIp = DetectorUtil.GetPropertyValue("ServerRNicIp");
            prerequisites.Properties.Add("Server RDMA NIC IP Address", new List<string> { serverRNicIp });

            string sutUserName = DetectorUtil.GetPropertyValue("SutUserName");
            prerequisites.Properties.Add("SUT User Name", new List<string> { sutUserName });

            string sutPassword = DetectorUtil.GetPropertyValue("SutPassword");
            prerequisites.Properties.Add("SUT Password", new List<string> { sutPassword });

            string shareFolder = DetectorUtil.GetPropertyValue("ShareFolder");
            prerequisites.Properties.Add("SUT Shared Folder", new List<string> { shareFolder });

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
            throw new NotImplementedException();
        }

        public void SelectEnvironment(string NetworkEnvironment)
        {
            throw new NotImplementedException();
        }

        public bool SetPrerequisiteProperties(Dictionary<string, string> properties)
        {
            throw new NotImplementedException();
        }
    }
}
