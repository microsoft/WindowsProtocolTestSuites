// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Net;
using System.Collections.Generic;
using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestManager.Detector.Common;

namespace Microsoft.Protocols.TestManager.RDPClientPlugin
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
        private DetectLogger logWriter = new DetectLogger();

        #endregion Variables

        #region Constant
        private const string tcComputerNameTitle = "SUT IP or proxy IP in Dirver LAN ";
        private const string proxyIPTitle = "The Proxy IP in SUT LAN \n *Leave it blank if no proxy";
        private const string isWindowsImplementationTitle = "IsWindowsImplementation";
        private const string dropConnectionForInvalidRequestTitle = "DropConnectionForInvalidRequest";
        private const string triggerMethodTitle = "Trigger RDP Client By";
        private const string userNameInTCTitle = "SUT User Name \n* Only for PowerShell Trigger";
        private const string userPwdInTCTitle = "SUT Password \n* Only for PowerShell Trigger";
        private const string agentPortTitle = "Agent Listening Port \n* Only for Managed Trigger";
        private const string rdpServerPortTitle = "RDP Server Listening Port";
        private const string propSutName = "SUTName";
        private const string propSUTUserName = "SUTUserName";
        private const string propSUTUserPassword = "SUTUserPassword";
        private const string propProxyIP = "ProxyIP";
        private const string propIsWindowsImplementation = "IsWindowsImplementation";
        private const string propDropConnectionForInvalidRequest = "DropConnectionForInvalidRequest";
        private const string propSupportAutoReconnect = "SupportAutoReconnect";
        private const string propSupportServerRedirection = "SupportServerRedirection";
        private const string propSupportRDPEFS = "SupportRDPEFS";
        private const string propRDPVersion = "Version";
        private const string propRDPServerPort = "ServerPort";
        private const string propAgentAddress = "SUTControl.AgentAddress";
        private const string propIsClientSupportRDPFile = "SUTControl.ClientSupportRDPFile";

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
            string sutName = DetectorUtil.GetPropertyValue(propSutName);
            string userNameInTC = DetectorUtil.GetPropertyValue(propSUTUserName);
            string userPwdInTC = DetectorUtil.GetPropertyValue(propSUTUserPassword);
            string rdpServerPort = DetectorUtil.GetPropertyValue(propRDPServerPort);

            //The RDP proxy IP, which should be set when a proxy is used.
            //Leave it blank if there's no proxy in the env
            string proxyIP = DetectorUtil.GetPropertyValue(propProxyIP);
            string isWindowsImplementation = DetectorUtil.GetPropertyValue(propIsWindowsImplementation);
            string DropConnectionForInvalidRequest = isWindowsImplementation.ToUpper().Equals("TRUE") ? "true" : DetectorUtil.GetPropertyValue(propDropConnectionForInvalidRequest); // The value is always true for Windows implementation.

            List<string> sutNames = new List<string>();
            List<string> userNamesInTC = new List<string>();
            List<string> userPwdsInTC = new List<string>();
            List<string> proxyIPs = new List<string>();
            List<string> isWindowsImplementationList = new List<string>();
            List<string> dropConnectionForInvalidRequestList = new List<string>();            

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
            proxyIPs.Add(proxyIP);

            dropConnectionForInvalidRequestList.Add("true");
            dropConnectionForInvalidRequestList.Add("false");
            propertiesDic.Add(tcComputerNameTitle, sutNames);
            propertiesDic.Add(proxyIPTitle, proxyIPs);
            propertiesDic.Add(isWindowsImplementationTitle, isWindowsImplementationList);
            propertiesDic.Add(dropConnectionForInvalidRequestTitle, dropConnectionForInvalidRequestList);
            propertiesDic.Add(triggerMethodTitle, new List<string>() { "Powershell", "Managed", "Interactive" });
            propertiesDic.Add(userNameInTCTitle, userNamesInTC);
            propertiesDic.Add(userPwdInTCTitle, userPwdsInTC);
            propertiesDic.Add(agentPortTitle, new List<string>() { "4488" });
            propertiesDic.Add(rdpServerPortTitle, new List<string>() { string.IsNullOrWhiteSpace(rdpServerPort) ? "3389" : rdpServerPort });

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
            detectionInfo.ProxyIP = properties[proxyIPTitle];
            detectionInfo.IsWindowsImplementation = properties[isWindowsImplementationTitle];
            detectionInfo.DropConnectionForInvalidRequest = properties[dropConnectionForInvalidRequestTitle];
            detectionInfo.TriggerMethod = TriggerMethod.Powershell;
            if (properties[triggerMethodTitle] != null && properties[triggerMethodTitle].Equals("Interactive"))
            {
                detectionInfo.TriggerMethod = TriggerMethod.Manual;
            }
            else if (properties[triggerMethodTitle] != null && properties[triggerMethodTitle].Equals("Managed"))
            {
                detectionInfo.TriggerMethod = TriggerMethod.Managed;
            }
            else if (properties[triggerMethodTitle] != null && properties[triggerMethodTitle].Equals("Shell"))
            {
                detectionInfo.TriggerMethod = TriggerMethod.Shell;
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
            detectionInfo.RDPServerPort = 3389;
            if (properties[rdpServerPortTitle] != null)
            {
                try
                {
                    detectionInfo.RDPServerPort = Int32.Parse(properties[rdpServerPortTitle]);
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
            DetectingItems.Add(new DetectingItem("Detect Target SUT or Proxy IP Address", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Detect RDP connection with SUT/Proxy", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Check Specified features Support", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Check Specified Protocols Support", DetectingStatus.Pending, LogStyle.Default));
            return DetectingItems;
        }

        /// <summary>
        /// Runs property autodetection.
        /// </summary>
        /// <returns>Return true if the function is succeeded.</returns>
        public bool RunDetection(DetectContext context)
        {
            logWriter.ApplyDetectContext(context);
            try
            {
                if (context.Token.IsCancellationRequested)
                    return false;

                if (!DetectSUTIPAddress())
                {
                    return false;
                }

                if (context.Token.IsCancellationRequested)
                    return false;

                using (var detector = new RDPDetector(detectionInfo, logWriter, context.TestSuitePath))
                {
                    if (!detector.DetectRDPFeature())
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                logWriter.AddLog(DetectLogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                logWriter.AddLog(DetectLogLevel.Error, ex.Message);
                return false;
            }
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

            propertiesDic.Add(propSutName, new List<string>() { detectionInfo.SUTName });
            propertiesDic.Add(propSUTUserName, new List<string>() { detectionInfo.UserNameInTC });
            propertiesDic.Add(propSUTUserPassword, new List<string>() { detectionInfo.UserPwdInTC });
            propertiesDic.Add(propProxyIP, new List<string>() { detectionInfo.ProxyIP});

            propertiesDic.Add(propIsWindowsImplementation, new List<string>() { detectionInfo.IsWindowsImplementation });
            propertiesDic.Add(propDropConnectionForInvalidRequest, new List<string>() { detectionInfo.DropConnectionForInvalidRequest });

            propertiesDic.Add(propSupportAutoReconnect, new List<string>() { NullableBoolToString(detectionInfo.IsSupportAutoReconnect) });
            propertiesDic.Add(propSupportServerRedirection, new List<string>() { NullableBoolToString(detectionInfo.IsSupportServerRedirection) });
            propertiesDic.Add(propSupportRDPEFS, new List<string>() { NullableBoolToString(detectionInfo.IsSupportRDPEFS) });

            propertiesDic.Add(propRDPVersion, new List<string>() { detectionInfo.RdpVersion });
            propertiesDic.Add(propRDPServerPort, new List<string>() { detectionInfo.RDPServerPort.ToString() });

            propertiesDic.Add(propAgentAddress, new List<string>() { detectionInfo.SUTName + ":" + detectionInfo.AgentListenPort });
            propertiesDic.Add(propIsClientSupportRDPFile, new List<string>() { detectionInfo.IsWindowsImplementation });

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
                || (detectionInfo.IsSupportTransportTypeUdpFECL != null && detectionInfo.IsSupportTransportTypeUdpFECL.Value)))
            {
                isSupportMultitransport = true;
            }
            caseList.Add(CreateRule("Protocol.RDPEMT", isSupportMultitransport));
            caseList.Add(CreateRule("Protocol.RDPEUDP", isSupportMultitransport));
            caseList.Add(CreateRule("Protocol.RDPEUDP2", detectionInfo.IsSupportRDPEUDP2));
            caseList.Add(CreateRule("Protocol.RDPEUSB", detectionInfo.IsSupportRDPEUSB));
            caseList.Add(CreateRule("Protocol.RDPEVOR", detectionInfo.IsSupportRDPEVOR));

            #endregion Protocols

            caseList.Add(CreateRule("Specific Requirements.DeviceNeeded", false));
            caseList.Add(CreateRule("Specific Requirements.Interactive", false));
            caseList.Add(CreateRule("Specific Requirements.NoDeviceNeeded", true));
            caseList.Add(CreateRule("Specific Requirements.NonInteractive", true));            

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

            caseList.Add(CreateRule("Enable Supported Feature.BasicRequirement", true));
            caseList.Add(CreateRule("Enable Supported Feature.BasicFeature", true));

            return caseList;
        }

        public List<ResultItemMap> GetSUTSummary()
        {
            SUTSummary sutSummary = new SUTSummary();
            var detectionSummary = sutSummary.LoadDetectionInfo(detectionInfo);
            return detectionSummary;
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
            hiddenProp.Add("RDPConnectWithNegotiationApproachInvalidAccount_Task");
            hiddenProp.Add("RDPConnectWithDirectTLS_Task");
            hiddenProp.Add("RDPConnectWithDirectCredSSP_Task");
            hiddenProp.Add("RDPConnectWithDirectCredSSPInvalidAccount_Task");
            hiddenProp.Add("RDPConnectWithNegotiationApproachFullScreen_Task");
            hiddenProp.Add("RDPConnectWithDirectTLSFullScreen_Task");
            hiddenProp.Add("RDPConnectWithDirectCredSSPFullScreen_Task");
            hiddenProp.Add("TriggerClientAutoReconnect_Task");
            hiddenProp.Add("TriggerInputEvents_Task");
            hiddenProp.Add("TriggerOneTouchEvent_Task");
            hiddenProp.Add("TriggerMultiTouchEvent_Task");
            hiddenProp.Add("TriggerSingleTouchPositionEvent_Task");
            hiddenProp.Add("TriggerContinuousTouchEvent_Task");
            hiddenProp.Add("TriggerTouchHoverEvent_Task");
            hiddenProp.Add("TriggerCloseRDPWindow_Task");
            hiddenProp.Add("CredentialManagerAddInvalidAccount_Task");
            hiddenProp.Add("CredentialManagerReverseInvalidAccount_Task");
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
            hiddenProp.Add("TestName");
            hiddenProp.Add("ProtocolName");
            hiddenProp.Add("Version");

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

        private bool DetectSUTIPAddress()
        {
            logWriter.AddLog(DetectLogLevel.Information, "===== Detect Target SUT IP Address=====");

            try
            {
                IPAddress address;
                //Detect SUT IP address by SUT name
                //If SUT name is an ip address, skip to resolve, use the ip address directly
                if (IPAddress.TryParse(detectionInfo.SUTName, out address))
                {
                    logWriter.AddLog(DetectLogLevel.Warning, "Finished", true, LogStyle.StepPassed);
                    return true;
                }
                else //DNS resolve the SUT IP address by SUT name
                {
                    IPAddress[] addList = Dns.GetHostAddresses(detectionInfo.SUTName);

                    if (null == addList)
                    {
                        logWriter.AddLog(DetectLogLevel.Warning, "The SUT name {0} cannot be resolved.", true, LogStyle.StepFailed);
                        return false;
                    }
                    else
                    {
                        logWriter.AddLog(DetectLogLevel.Warning, "SUT detection finished", true, LogStyle.StepPassed);

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                logWriter.AddLog(DetectLogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                logWriter.AddLog(DetectLogLevel.Error, ex.Message);
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
