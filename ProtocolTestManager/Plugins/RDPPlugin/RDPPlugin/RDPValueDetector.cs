// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Protocols.TestManager.Detector;

namespace Microsoft.Protocols.TestManager.RDPPlugin
{
    public class RDPValueDetector : IValueDetector
    {
        #region Private Types

        private enum EnvironmentType
        {
            Domain,
            Workgroup,
        }

        #endregion Private Types

        #region Variables

        private EnvironmentType env = EnvironmentType.Workgroup;
        private DetectionInfo detectionInfo = new DetectionInfo();       

        #endregion Variables

        #region Constant
        private const string tcComputerNameTitle = "SUT Name";
        private const string isWindowsImplementationTitle = "IsWindowsImplementation";
        private const string triggerMethodTitle = "Trigger RDP Client By";
        private const string userNameInTCTitle =  "SUT User Name \n* Only for PowerShell Trigger";
        private const string userPwdInTCTitle = "SUT Password \n* Only for PowerShell Trigger";
        private const string agentPortTitle = "Agent Listen Port \n* Only for Managed Trigger";

        #endregion Constant

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
            Prerequisites prerequisites = new Prerequisites();
            prerequisites.Title = "Remote Desktop";
            prerequisites.Summary = "Please input the below info to detect SUT.";

            Dictionary<string, List<string>> propertiesDic = new Dictionary<string, List<string>>();

            //Retrieve values from *.ptfconfig file
            string sutName = DetectorUtil.GetPropertyValue("SUTName");
            string userNameInTC = DetectorUtil.GetPropertyValue("SUTUserName");
            string userPwdInTC = DetectorUtil.GetPropertyValue("SUTUserPassword");
            string isWindowsImplementation = DetectorUtil.GetPropertyValue("IsWindowsImplementation");

            List<string> sutNames = new List<string>();
            List<string> userNamesInTC = new List<string>();
            List<string> userPwdsInTC = new List<string>();
            List<string> isWindowsImplementationList = new List<string>();

            if (string.IsNullOrWhiteSpace(sutName)
                || string.IsNullOrWhiteSpace(userNameInTC)
                || string.IsNullOrWhiteSpace(userPwdInTC)
                || string.IsNullOrWhiteSpace(isWindowsImplementation))
            {
                sutNames.Add("SUT01");
                userNamesInTC.Add("administrator");
                userPwdsInTC.Add("Password01!");
                isWindowsImplementationList.Add("true");
                isWindowsImplementationList.Add("false");
            }
            else
            {
                sutNames.Add(sutName);
                userNamesInTC.Add(userNameInTC);
                userPwdsInTC.Add(userPwdInTC);
                isWindowsImplementationList.Add(isWindowsImplementation);
                if (isWindowsImplementation.ToUpper().Equals("TRUE"))
                {
                    isWindowsImplementationList.Add("false");
                }
                else
                {
                    isWindowsImplementationList.Add("true");
                }
            }

            propertiesDic.Add(tcComputerNameTitle, sutNames);            
            propertiesDic.Add(isWindowsImplementationTitle, isWindowsImplementationList);
            propertiesDic.Add(triggerMethodTitle, new List<string>() { "Powershell", "Managed", "Interactive"});
            propertiesDic.Add(userNameInTCTitle, userNamesInTC);
            propertiesDic.Add(userPwdInTCTitle, userPwdsInTC);
            propertiesDic.Add(agentPortTitle, new List<string>() {"4488"});

            prerequisites.Properties = propertiesDic;

            return prerequisites;
        }

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
            // Save the prerequisites set by user
            detectionInfo.SUTName = properties[tcComputerNameTitle];
            detectionInfo.UserNameInTC = properties[userNameInTCTitle];
            detectionInfo.UserPwdInTC = properties[userPwdInTCTitle];
            detectionInfo.IsWindowsImplementation = properties[isWindowsImplementationTitle];
            detectionInfo.TriggerMethod = TriggerMethod.Powershell;
            if (properties[triggerMethodTitle] != null && properties[triggerMethodTitle].Equals("Interactive"))
            {
                detectionInfo.TriggerMethod = TriggerMethod.Manual;
            }
            else if (properties[triggerMethodTitle] != null && properties[triggerMethodTitle].Equals("Managed"))
            {
                detectionInfo.TriggerMethod = TriggerMethod.Managed;
            }
            detectionInfo.AgentListenPort = 4488;
            if (properties[agentPortTitle] != null)
            {
                try
                {
                    detectionInfo.AgentListenPort = Int32.Parse(properties[agentPortTitle]);
                }
                catch (Exception) { };
            }
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
            propertiesDic = new Dictionary<string, List<string>>();

            propertiesDic.Add("SUTName", new List<string>() { detectionInfo.SUTName });
            propertiesDic.Add("SUTUserName", new List<string>() { detectionInfo.UserNameInTC });
            propertiesDic.Add("SUTUserPassword", new List<string>() { detectionInfo.UserPwdInTC });
            propertiesDic.Add("IsWindowsImplementation", new List<string>() { detectionInfo.IsWindowsImplementation });

            propertiesDic.Add("RDP.Client.SupportAutoReconnect", new List<string>() { NullableBoolToString(detectionInfo.IsSupportAutoReconnect) });
            propertiesDic.Add("RDP.Client.SupportServerRedirection", new List<string>() { NullableBoolToString(detectionInfo.IsSupportServerRedirection) });
            propertiesDic.Add("RDP.Client.SupportRDPEFS", new List<string>() { NullableBoolToString(detectionInfo.IsSupportRDPEFS) });

            propertiesDic.Add("SUTControl.AgentAddress", new List<string>() { detectionInfo.SUTName+":"+detectionInfo.AgentListenPort });
            propertiesDic.Add("SUTControl.ClientSupportRDPFile", new List<string>() { detectionInfo.IsWindowsImplementation });

            

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
            caseList.Add(CreateRule("Protocol.RDPRFX", detectionInfo.IsSupportRDPRFX));
            
            caseList.Add(CreateRule("Protocol.RDPEDISP", detectionInfo.IsSupportRDPEDISP));
            caseList.Add(CreateRule("Protocol.RDPEGFX", detectionInfo.IsSupportRDPEGFX));
            caseList.Add(CreateRule("Protocol.RDPEI", detectionInfo.IsSupportRDPEI));
            bool isSupportMultitransport = false;
            if (detectionInfo.IsSupportStaticVirtualChannel != null && detectionInfo.IsSupportStaticVirtualChannel.Value
                && ((detectionInfo.IsSupportTransportTypeUdpFECR != null && detectionInfo.IsSupportTransportTypeUdpFECR.Value) 
                || (detectionInfo.IsSupportTransportTypeUdpFECL != null&& detectionInfo.IsSupportTransportTypeUdpFECL.Value)))
            {
                isSupportMultitransport = true;                
            }
            caseList.Add(CreateRule("Protocol.RDPEMT", isSupportMultitransport));
            caseList.Add(CreateRule("Protocol.RDPEUDP", isSupportMultitransport));
            caseList.Add(CreateRule("Protocol.RDPEUSB", detectionInfo.IsSupportRDPEUSB));
            caseList.Add(CreateRule("Protocol.RDPEVOR", detectionInfo.IsSupportRDPEVOR));

            #endregion Protocols

            caseList.Add(CreateRule("Specific Requirements.DeviceNeeded", false));
            caseList.Add(CreateRule("Specific Requirements.Interactive", false));

            caseList.Add(CreateRule("Enable Supported Feature.AutoReconnect", detectionInfo.IsSupportAutoReconnect));
            caseList.Add(CreateRule("Enable Supported Feature.FastPathInput", true));
            caseList.Add(CreateRule("Enable Supported Feature.HeartBeat", detectionInfo.IsSupportHeartbeatPdu));
            caseList.Add(CreateRule("Enable Supported Feature.NetcharAutoDetect", detectionInfo.IsSupportNetcharAutoDetect));

            caseList.Add(CreateRule("Enable Supported Feature.ServerRedirection", detectionInfo.IsSupportServerRedirection));
            bool bothSupportSVCAndEFS = false;
            if (detectionInfo.IsSupportRDPEFS != null && detectionInfo.IsSupportRDPEFS.Value
                && detectionInfo.IsSupportStaticVirtualChannel != null && detectionInfo.IsSupportStaticVirtualChannel.Value)
            {
                bothSupportSVCAndEFS = true;
            }
            caseList.Add(CreateRule("Enable Supported Feature.StaticVirtualChannel", bothSupportSVCAndEFS));
            
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
            List<string> hiddenProp = new List<string>();
            // Task name for SUT control adapter
            hiddenProp.Add("RDPConnectWithNegotiationApproach_Task");
            hiddenProp.Add("RDPConnectWithDirectTLS_Task");
            hiddenProp.Add("RDPConnectWithDirectCredSSP_Task");
            hiddenProp.Add("RDPConnectWithNegotiationApproachFullScreen_Task");
            hiddenProp.Add("RDPConnectWithDirectTLSFullScreen_Task");
            hiddenProp.Add("RDPConnectWithDirectCredSSPFullScreen_Task");
            hiddenProp.Add("TriggerClientAutoReconnect_Task");
            hiddenProp.Add("TriggerInputEvents_Task");
            hiddenProp.Add("TriggerClientDisconnectAll_Task");
            hiddenProp.Add("TriggerOneTouchEvent_Task");
            hiddenProp.Add("TriggerMultiTouchEvent_Task");
            hiddenProp.Add("TriggerSingleTouchPositionEvent_Task");
            hiddenProp.Add("TriggerContinuousTouchEvent_Task");
            hiddenProp.Add("TriggerTouchHoverEvent_Task");
            hiddenProp.Add("MaximizeRDPClientWindow_Task");

            // Image configure for RDPEDISP
            hiddenProp.Add("RdpedispEndImage");
            hiddenProp.Add("RdpedispOrientationChange1Image");
            hiddenProp.Add("RdpedispOrientationChange2Image");
            hiddenProp.Add("RdpedispOrientationChange3Image");
            hiddenProp.Add("RdpedispOrientationChange4Image");
            hiddenProp.Add("RdpedispMonitorAddImage");
            hiddenProp.Add("RdpedispMonitorRemoveImage");

            // Other properties should be hidden
            hiddenProp.Add("IsTDI67289Fixed");
            return hiddenProp;
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
                    replys.Add(pingSender.Send(detectionInfo.SUTName, timeout, buffer, options));
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
