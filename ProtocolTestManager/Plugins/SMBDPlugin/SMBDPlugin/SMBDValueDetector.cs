// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestManager.SMBDPlugin;
using Microsoft.Protocols.TestManager.SMBDPlugin.Detector;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.Detector
{
    static class PropertyDictionaryConstant
    {
        public const string SUTNAME = "SUT Name";
        public const string DOMAINNAME = "Domain Name";
        public const string SUTUSERNAME = "SUT User Name";
        public const string SUTPASSWORD = "SUT Password";
        public const string SHAREFOLDER = "SUT Shared Folder";
        public const string ISWINDOWSIMPLEMENTATION = "Is Windows Implementation";
        public const string AUTHENTICATION = "Authentication";
        public const string SMBDPORT = "SMBD Port";
        public const string CONNECTIONTIMEOUT = "Connection Timeout";
    }

    static class DeploymentPtfConfigConstant
    {
        public const string SUTCOMPUTERNAME = "SutComputerName";
        public const string DOMAINNAME = "DomainName";
        public const string SUTUSERNAME = "SutUserName";
        public const string SUTPASSWORD = "SutPassword";
        public const string SERVERNONRNICIP = "ServerNonRNicIp";
        public const string SERVERRNICIP = "ServerRNicIp";
        public const string CLIENTNONRNICIP = "ClientNonRNicIp";
        public const string CLIENTRNICIP = "ClientRNicIp";
        public const string PLATFORM = "Platform";
        public const string SHAREFOLDER = "ShareFolder";
    }

    static class PtfConfigConstant
    {
        public const string SMBDTCPPORT = "SmbdTcpPort";
        public const string RECEIVECREDITMAX = "ReceiveCreditMax";
        public const string SENDCREDITTARGET = "SendCreditTarget";
        public const string MAXSENDSIZE = "MaxSendSize";
        public const string MAXFRAGMENTEDSIZE = "MaxFragmentedSize";
        public const string MAXRECEIVESIZE = "MaxReceiveSize";
        public const string SMB2CONNECTIONTIMEOUTINSECONDS = "Smb2ConnectionTimeoutInSeconds";
        public const string ENDIANOFBUFFERDESCRIPTOR = "EndianOfBufferDescriptor";
    }

    public class SMBDValueDetector : IValueDetector
    {
        private DetectionInfo detectionInfo;

        public SMBDValueDetector()
        {
            detectionInfo = new DetectionInfo();
        }

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

            propertiesDic[DeploymentPtfConfigConstant.SUTCOMPUTERNAME] = new List<string> { detectionInfo.SUTName };

            if (detectionInfo.SUTNonRdmaNICIPAddress != null)
            {
                propertiesDic[DeploymentPtfConfigConstant.SERVERNONRNICIP] = new List<string> { detectionInfo.SUTNonRdmaNICIPAddress };
            }
            if (detectionInfo.SUTRdmaNICIPAddress != null)
            {
                propertiesDic[DeploymentPtfConfigConstant.SERVERRNICIP] = new List<string> { detectionInfo.SUTRdmaNICIPAddress };
            }
            if (detectionInfo.DriverNonRdmaNICIPAddress != null)
            {
                propertiesDic[DeploymentPtfConfigConstant.CLIENTNONRNICIP] = new List<string> { detectionInfo.DriverNonRdmaNICIPAddress };
            }
            if (detectionInfo.DriverRdmaNICIPAddress != null)
            {
                propertiesDic[DeploymentPtfConfigConstant.CLIENTRNICIP] = new List<string> { detectionInfo.DriverRdmaNICIPAddress };
            }

            if (detectionInfo.OSDetected)
            {
                propertiesDic[DeploymentPtfConfigConstant.PLATFORM] = new List<string> { detectionInfo.Platform.ToString() };
            }

            propertiesDic[DeploymentPtfConfigConstant.SHAREFOLDER] = new List<string> { detectionInfo.ShareFolder };
            propertiesDic[DeploymentPtfConfigConstant.SUTUSERNAME] = new List<string> { detectionInfo.UserName };
            propertiesDic[DeploymentPtfConfigConstant.SUTPASSWORD] = new List<string> { detectionInfo.Password };
            propertiesDic[DeploymentPtfConfigConstant.DOMAINNAME] = new List<string> { detectionInfo.DomainName };
            propertiesDic[PtfConfigConstant.SMBDTCPPORT] = new List<string> { detectionInfo.SMBDPort.ToString() };
            propertiesDic[PtfConfigConstant.SMB2CONNECTIONTIMEOUTINSECONDS] = new List<string> { detectionInfo.ConnectionTimeout.TotalSeconds.ToString() };

            return true;
        }

        /// <summary>
        /// Adds Detection steps to the log shown when detecting
        /// </summary>
        public List<DetectingItem> GetDetectionSteps()
        {
            List<DetectingItem> DetectingItems = new List<DetectingItem>();
            DetectingItems.Add(new DetectingItem("Ping target SUT", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Check OS version", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Check NICs of local computer", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Check SMB dialect", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Check NICs of target SUT", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Connect to share(Non-RDMA)", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Connect to share(RDMA)", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Check SMB2 over SMBD feature", DetectingStatus.Pending, LogStyle.Default));
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

            string sutName = DetectorUtil.GetPropertyValue(DeploymentPtfConfigConstant.SUTCOMPUTERNAME);
            prerequisites.Properties.Add(PropertyDictionaryConstant.SUTNAME, new List<string> { sutName });

            string domainName = DetectorUtil.GetPropertyValue(DeploymentPtfConfigConstant.DOMAINNAME);
            prerequisites.Properties.Add(PropertyDictionaryConstant.DOMAINNAME, new List<string> { domainName });

            string sutUserName = DetectorUtil.GetPropertyValue(DeploymentPtfConfigConstant.SUTUSERNAME);
            prerequisites.Properties.Add(PropertyDictionaryConstant.SUTUSERNAME, new List<string> { sutUserName });

            string sutPassword = DetectorUtil.GetPropertyValue(DeploymentPtfConfigConstant.SUTPASSWORD);
            prerequisites.Properties.Add(PropertyDictionaryConstant.SUTPASSWORD, new List<string> { sutPassword });

            prerequisites.Properties.Add(PropertyDictionaryConstant.AUTHENTICATION, new List<string>() {
                SecurityPackageType.Negotiate.ToString(),
                SecurityPackageType.Kerberos.ToString(),
                SecurityPackageType.Ntlm.ToString()
            });

            string shareFolder = DetectorUtil.GetPropertyValue(DeploymentPtfConfigConstant.SHAREFOLDER);
            prerequisites.Properties.Add(PropertyDictionaryConstant.SHAREFOLDER, new List<string> { shareFolder });

            prerequisites.Properties.Add(PropertyDictionaryConstant.ISWINDOWSIMPLEMENTATION, new List<string> { "True", "False" });

            string smbdPort = DetectorUtil.GetPropertyValue(PtfConfigConstant.SMBDTCPPORT);
            prerequisites.Properties.Add(PropertyDictionaryConstant.SMBDPORT, new List<string> { smbdPort });

            string connectionTimeout = DetectorUtil.GetPropertyValue(PtfConfigConstant.SMB2CONNECTIONTIMEOUTINSECONDS);
            prerequisites.Properties.Add(PropertyDictionaryConstant.CONNECTIONTIMEOUT, new List<string> { connectionTimeout });

            return prerequisites;
        }

        private CaseSelectRule GenerateRule(string ruleName, bool isSupported)
        {
            var rule = new CaseSelectRule();
            rule.Name = ruleName;
            if (isSupported)
            {
                rule.Status = RuleStatus.Selected;
            }
            else
            {
                rule.Status = RuleStatus.NotSupported;
            }
            return rule;
        }

        /// <summary>
        /// Gets selected rules
        /// </summary>
        /// <returns>Selected rules</returns>
        public List<CaseSelectRule> GetSelectedRules()
        {
            var selectedRules = new List<CaseSelectRule>();

            // select BVT and Non-BVT cases
            selectedRules.Add(GenerateRule("Priority.BVT", true));
            selectedRules.Add(GenerateRule("Priority.Non-BVT", true));

            // select SMBD related cases if RDMA cards of both nodes are detected
            bool bothRdmaNicsdetected = detectionInfo.DriverRdmaNICIPAddress != null && detectionInfo.SUTRdmaNICIPAddress != null;
            selectedRules.Add(GenerateRule("Feature.SMBD.SMBD Negotiate", bothRdmaNicsdetected));
            selectedRules.Add(GenerateRule("Feature.SMBD.SMBD Credits Management", bothRdmaNicsdetected));
            selectedRules.Add(GenerateRule("Feature.SMBD.SMBD Data Transfer", bothRdmaNicsdetected));

            if (bothRdmaNicsdetected)
            {
                // select RDMA Channel if RDMA transport is supported
                selectedRules.Add(GenerateRule("Feature.SMB2 over SMBD.SMB 30.RDMA Channel", detectionInfo.RDMATransportSupported));

                // select Multiple Channels if both RDMA and Non-RDMA transports are supported
                selectedRules.Add(GenerateRule("Feature.SMB2 over SMBD.SMB 30.Multiple Channels", detectionInfo.RDMATransportSupported && detectionInfo.NonRDMATransportSupported));

                // select RDMA Channel Remote Invalidation if RDMAChannelV1Invalidate is supported
                selectedRules.Add(GenerateRule("Feature.SMB2 over SMBD.SMB 302.RDMA Channel Remote Invalidation", detectionInfo.RDMAChannelV1InvalidateSupported));
            }

            return selectedRules;
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

            bool result = detector.Detect();

            return result;
        }

        public void SelectEnvironment(string NetworkEnvironment)
        {
            throw new NotImplementedException();
        }

        public bool SetPrerequisiteProperties(Dictionary<string, string> properties)
        {
            detectionInfo.Authentication = (SecurityPackageType)Enum.Parse(typeof(SecurityPackageType), properties[PropertyDictionaryConstant.AUTHENTICATION]);
            detectionInfo.SUTName = properties[PropertyDictionaryConstant.SUTNAME];
            detectionInfo.DomainName = properties[PropertyDictionaryConstant.DOMAINNAME];
            detectionInfo.UserName = properties[PropertyDictionaryConstant.SUTUSERNAME];
            detectionInfo.Password = properties[PropertyDictionaryConstant.SUTPASSWORD];
            detectionInfo.IsWindowsImplementation = Boolean.Parse(properties[PropertyDictionaryConstant.ISWINDOWSIMPLEMENTATION]);
            detectionInfo.ShareFolder = properties[PropertyDictionaryConstant.SHAREFOLDER];
            detectionInfo.SMBDPort = UInt16.Parse(properties[PropertyDictionaryConstant.SMBDPORT]);

            detectionInfo.ConnectionTimeout = TimeSpan.FromSeconds(UInt32.Parse(properties[PropertyDictionaryConstant.CONNECTIONTIMEOUT]));
            detectionInfo.Endian = (RDMAEndian)Enum.Parse(typeof(RDMAEndian), DetectorUtil.GetPropertyValue(PtfConfigConstant.ENDIANOFBUFFERDESCRIPTOR));
            detectionInfo.SMBDClientCapability = new SMBDClientCapability()
            {
                CreditsRequested = UInt16.Parse(DetectorUtil.GetPropertyValue(PtfConfigConstant.SENDCREDITTARGET)),
                ReceiveCreditMax = UInt16.Parse(DetectorUtil.GetPropertyValue(PtfConfigConstant.RECEIVECREDITMAX)),
                PreferredSendSize = UInt32.Parse(DetectorUtil.GetPropertyValue(PtfConfigConstant.MAXSENDSIZE)),
                MaxReceiveSize = UInt32.Parse(DetectorUtil.GetPropertyValue(PtfConfigConstant.MAXRECEIVESIZE)),
                MaxFragmentedSize = UInt32.Parse(DetectorUtil.GetPropertyValue(PtfConfigConstant.MAXFRAGMENTEDSIZE))
            };
            return true;
        }
    }
}
