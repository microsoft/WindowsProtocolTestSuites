﻿// Copyright (c) Microsoft. All rights reserved.
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
using System.Net;
using System.Reflection;
using System.Net.NetworkInformation;
using Microsoft.Protocols.TestManager.FileServerPlugin;

namespace Microsoft.Protocols.TestManager.Detector
{
    public class FileServerValueDetector : IValueDetector
    {
        // Environment  enumeration
        private enum EnvironmentType
        {
            Domain,
            Workgroup,
        }

        #region Fields

        private EnvironmentType env = EnvironmentType.Domain;
        private DetectionInfo detectionInfo = new DetectionInfo();

        private Logger logWriter = new Logger();

        private const string SUTTitle = "Target SUT";
        private const string domainTitle = "Domain Name";
        private const string userTitle = "User Name";
        private const string passwordTitle = "Password";
        private const string securityPackageTitle = "Authentication";

        #endregion

        #region Interface required
        /// <summary>
        /// Set selected test environment.
        /// </summary>
        /// <param name="Environment"></param>
        public void SelectEnvironment(string environment)
        {
            Enum.TryParse(environment, out env);
        }

        /// <summary>
        /// Get the prerequisites for auto-detection.
        /// </summary>
        /// <returns>A instance of Prerequisites class.</returns>
        public Prerequisites GetPrerequisites()
        {
            Prerequisites prerequisites = new Prerequisites();

            prerequisites.Title = "FileServer";
            prerequisites.Summary = "Please input the below info to detect SUT.\r\nIf SUT is in WORKGROUP, set Domain Name to the same value as the Target SUT field.";

            Dictionary<string, List<string>> propertiesDic = new Dictionary<string, List<string>>();

            #region Set Properties

            // Retrieve saved value from *.ptfconfig file
            string SUT = DetectorUtil.GetPropertyValue("Common.SutComputerName");
            string domain = DetectorUtil.GetPropertyValue("Common.DomainName");
            string user = DetectorUtil.GetPropertyValue("Common.AdminUserName");
            string password = DetectorUtil.GetPropertyValue("Common.PasswordForAllUsers");

            List<string> domainList = new List<string>();
            List<string> SUTList = new List<string>();
            List<string> userList = new List<string>();
            List<string> passwordList = new List<string>();

            if (string.IsNullOrWhiteSpace(SUT)
                || string.IsNullOrWhiteSpace(domain)
                || string.IsNullOrWhiteSpace(user)
                || string.IsNullOrWhiteSpace(password))
            {
                SUTList.Add("node01");
                domainList.Add("contoso.com");
                userList.Add("administrator");
                passwordList.Add("Password01!");
            }
            else
            {
                SUTList.Add(SUT);
                domainList.Add(domain);
                userList.Add(user);
                passwordList.Add(password);
            }

            propertiesDic.Add(SUTTitle, SUTList);
            propertiesDic.Add(domainTitle, domainList);
            propertiesDic.Add(userTitle, userList);
            propertiesDic.Add(passwordTitle, passwordList);
            propertiesDic.Add(securityPackageTitle, new List<string>() { "Negotiate", "Kerberos", "Ntlm" });

            // Get the real environment DomainName
            string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
            if (string.IsNullOrWhiteSpace(domainName))
                domainName = SUT;

            List<string> tempDomainList = propertiesDic[domainTitle];
            tempDomainList.Clear();
            tempDomainList.Add(domainName);

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
            // Save the prerequisites set by user
            detectionInfo.targetSUT = properties[SUTTitle];
            detectionInfo.domainName = properties[domainTitle];
            detectionInfo.userName = properties[userTitle];
            detectionInfo.password = properties[passwordTitle];
            detectionInfo.securityPackageType = (SecurityPackageType)Enum.Parse(typeof(SecurityPackageType), properties["Authentication"]);

            if (detectionInfo.targetSUT == detectionInfo.domainName)
                env = EnvironmentType.Workgroup;

            // Check the validity of the inputs
            if (string.IsNullOrEmpty(detectionInfo.targetSUT)
                || string.IsNullOrEmpty(detectionInfo.userName)
                || string.IsNullOrEmpty(detectionInfo.password))
            {
                throw new Exception(string.Format(
                    "Following boxes should not be empty: {0} Target SUT {1} User Name {2} Password",
                    Environment.NewLine, Environment.NewLine, Environment.NewLine));
            }

            if (env == EnvironmentType.Domain)
            {
                if (string.IsNullOrEmpty(detectionInfo.domainName))
                    throw new Exception(string.Format("Following box should not be empty: {0} Domain Name", Environment.NewLine));
            }
            // No other property needed
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
            DetectingItems.Add(new DetectingItem("Fetch Smb2 Info", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Check the Credential", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Detect Platform and User Account", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Fetch Share Info", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Detect Basic Share", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Detect Ioctl Codes", DetectingStatus.Pending, LogStyle.Default));
            DetectingItems.Add(new DetectingItem("Detect Create Contexts", DetectingStatus.Pending, LogStyle.Default));
            return DetectingItems;
        }

        /// <summary>
        /// Run property autodetection.
        /// </summary>
        /// <returns>Return true if the function is succeeded.</returns>
        public bool RunDetection()
        {
            logWriter.AddLog(LogLevel.Information, "===== Start detecting =====");

            FSDetector detector = new FSDetector
                (logWriter,
                detectionInfo.targetSUT,
                new AccountCredential(detectionInfo.domainName, detectionInfo.userName, detectionInfo.password),
                detectionInfo.securityPackageType,
                int.Parse(DetectorUtil.GetPropertyValue("Common.Timeout"))); // Retrieve timeout from ptfconfig

            // Terminate the whole detection if any exception happens in the following processes
            if (!PingSUT(detector))
                return false;

            if (!DetectLocalNetworkInfo(detector))
                return false;

            if (!DetectSMB2Info(detector))
                return false;

            if (!CheckUsernamePassword(detector))
                return false;

            // Do not interrupt auto-detection 
            // Even if detecting platform and useraccount failed
            DetectPlatformAndUserAccount(detector);

            if (!DetectShareInfo(detector))
                return false;

            if (!DetermineBasicShare())
                return false;

            DetermineSymboliclink();

            detectionInfo.detectExceptions = new Dictionary<string, string>();

            // Detect IoctlCodes and Create Contexts
            // If any exceptions, just ignore
            DetectIoctlCodes(detector);
            DetectCreateContexts(detector);

            logWriter.AddLog(LogLevel.Information, "===== End detecting =====");
            return true;
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

            #region Common

            propertiesDic.Add("Common.SutComputerName", new List<string>() { detectionInfo.targetSUT });
            propertiesDic.Add("Common.SutIPAddress", new List<string>() { detectionInfo.networkInfo.SUTIpList[0].ToString() });
            propertiesDic.Add("Common.DomainName", new List<string>() { detectionInfo.domainName });
            propertiesDic.Add("Common.AdminUserName", new List<string>() { detectionInfo.userName });
            propertiesDic.Add("Common.NonAdminUserName", detectionInfo.nonadminUserAccounts);
            propertiesDic.Add("Common.GuestUserName", new List<string>() { detectionInfo.guestUserAccount });
            propertiesDic.Add("Common.PasswordForAllUsers", new List<string>() { detectionInfo.password });
            propertiesDic.Add("Common.BasicFileShare", CreateShareList());
            propertiesDic.Add("Common.EncryptedFileShare", new List<string>() { GetShare(ShareFlags_Values.SHAREFLAG_ENCRYPT_DATA) });
            propertiesDic.Add("Common.MaxSmbVersionSupported", new List<string>() { detectionInfo.smb2Info.MaxSupportedDialectRevision.ToString() });
            propertiesDic.Add("Common.IsLeasingSupported", new List<string>() { detectionInfo.smb2Info.SupportedCapabilities.HasFlag(Capabilities_Values.GLOBAL_CAP_LEASING).ToString().ToLower() });
            propertiesDic.Add("Common.IsMultiChannelCapable", new List<string>() { detectionInfo.smb2Info.SupportedCapabilities.HasFlag(Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL).ToString().ToLower() });
            propertiesDic.Add("Common.IsDirectoryLeasingSupported", new List<string>() { detectionInfo.smb2Info.SupportedCapabilities.HasFlag(Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING).ToString().ToLower() });
            propertiesDic.Add("Common.IsPersistentHandlesSupported", new List<string>() { detectionInfo.smb2Info.SupportedCapabilities.HasFlag(Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES).ToString().ToLower() });
            if (detectionInfo.CheckHigherDialect(detectionInfo.smb2Info.MaxSupportedDialectRevision, DialectRevision.Smb311))
            {
                propertiesDic.Add("Common.IsEncryptionSupported", new List<string>() { detectionInfo.smb2Info.SelectedCipherID > EncryptionAlgorithm.ENCRYPTION_NONE ? "true" : "false" });
            }
            else
            {
                propertiesDic.Add("Common.IsEncryptionSupported", new List<string>() { detectionInfo.smb2Info.SupportedCapabilities.HasFlag(Capabilities_Values.GLOBAL_CAP_ENCRYPTION).ToString().ToLower() });
            }
            propertiesDic.Add("Common.IsRequireMessageSigning", new List<string>() { detectionInfo.smb2Info.IsRequireMessageSigning.ToString().ToLower() });
            propertiesDic.Add("Common.IsMultiCreditSupported", new List<string>() { detectionInfo.smb2Info.SupportedCapabilities.HasFlag(Capabilities_Values.GLOBAL_CAP_LARGE_MTU).ToString().ToLower() });
            propertiesDic.Add("Common.UnsupportedIoCtlCodes", new List<string>() { GetUnsupportedItems(detectionInfo.unsupportedIoctlCodes) });
            propertiesDic.Add("Common.UnsupportedCreateContexts", new List<string>() { GetUnsupportedItems(detectionInfo.unsupportedCreateContexts) });
            string caShare = GetShare(ShareFlags_Values.SHAREFLAG_ACCESS_BASED_DIRECTORY_ENUM, Share_Capabilities_Values.SHARE_CAP_CONTINUOUS_AVAILABILITY);
            if (string.IsNullOrEmpty(caShare))
            {
                if (detectionInfo.platform != Platform.NonWindows)
                {
                    // Keep the default value for Windows platform
                }
                else
                {
                    propertiesDic.Add("Common.CAShareName", new List<string>() { string.Empty });
                    propertiesDic.Add("Common.CAShareServerName", new List<string>() { string.Empty });
                }
            }
            else
            {
                propertiesDic.Add("Common.CAShareName", CreateShareList());
                propertiesDic.Add("Common.CAShareServerName", new List<string>() { detectionInfo.targetSUT });
            }

            if (detectionInfo.networkInfo.LocalIpList.Count == 0)
            {
                propertiesDic.Add("Common.ClientNic1IPAddress", new List<string>() { string.Empty });
            }
            else
            {
                propertiesDic.Add("Common.ClientNic1IPAddress", new List<string>() { detectionInfo.networkInfo.LocalIpList[0].ToString() });
            }

            if (detectionInfo.networkInfo.LocalIpList.Count > 1)
            {
                propertiesDic.Add("Common.ClientNic2IPAddress", new List<string>() { detectionInfo.networkInfo.LocalIpList[1].ToString() });
            }
            else if (detectionInfo.networkInfo.LocalIpList.Count == 1)
            {
                propertiesDic.Add("Common.ClientNic2IPAddress", new List<string>() { detectionInfo.networkInfo.LocalIpList[0].ToString() });
            }
            else
            {
                propertiesDic.Add("Common.ClientNic2IPAddress", new List<string>() { string.Empty });
            }

            propertiesDic.Add("Common.Platform", new List<string>() { detectionInfo.platform.ToString() });
            propertiesDic.Add("Common.SupportedSecurityPackage", new List<string>() { detectionInfo.securityPackageType.ToString() });

            #endregion

            #region DFSC

            if (env == EnvironmentType.Workgroup)
            {
                propertiesDic.Add("DFSC.DomainNetBIOSName", new List<string>() { string.Empty });
                propertiesDic.Add("DFSC.DomainFQDNName", new List<string>() { string.Empty });
                propertiesDic.Add("DFSC.SiteName", new List<string>() { string.Empty });
                propertiesDic.Add("DFSC.DomainNamespace", new List<string>() { string.Empty });
                propertiesDic.Add("DFSC.DCServerComputerName", new List<string>() { string.Empty });
            }

            #endregion

            #region File Server Failover

            // Only Windows Server 2012 R2 supports asymmetric share
            if (detectionInfo.platform != Platform.WindowsServer2012R2)
            {
                // For other Windows platform, the feature is not supported.
                // For NonWindows platform, the items are suggested to be configured manually.
                propertiesDic.Add("Cluster.AsymmetricShare", GetAsymmetricShare());
                propertiesDic.Add("Cluster.OptimumNodeOfAsymmetricShare", new List<string>() { string.Empty });
                propertiesDic.Add("Cluster.NonOptimumNodeOfAsymmetricShare", new List<string>() { string.Empty });
            }

            #endregion

            #region SMB2 Traditional

            if (detectionInfo.networkInfo.SUTIpList.Count > 1)
                propertiesDic.Add("SMB2.SutAlternativeIPAddress", new List<string>() { detectionInfo.networkInfo.SUTIpList[1].ToString() });
            else
                propertiesDic.Add("SMB2.SutAlternativeIPAddress", new List<string>() { detectionInfo.networkInfo.SUTIpList[0].ToString() });

            propertiesDic.Add("SMB2.FileShareSupportingIntegrityInfo", CreateShareList(detectionInfo.shareSupportingIntegrityInfo));
            #endregion

            #region SMB2 Model


            propertiesDic.Add("SMB2.SpecialShare", CreateShareList(GetSpecialShare()));
            propertiesDic.Add("SMB2.ShareWithoutForceLevel2OrSOFS", CreateShareList());

            string scaleoutShare = GetShare(ShareFlags_Values.SHAREFLAG_ACCESS_BASED_DIRECTORY_ENUM, Share_Capabilities_Values.SHARE_CAP_SCALEOUT);
            if (!string.IsNullOrEmpty(scaleoutShare) || detectionInfo.platform == Platform.NonWindows)  // Keep default value for windows
            {
                propertiesDic.Add("SMB2.ShareWithoutForceLevel2WithSOFS", CreateShareList(scaleoutShare));
                propertiesDic.Add("SMB2.ShareWithForceLevel2AndSOFS", CreateShareList(scaleoutShare));
                propertiesDic.Add("SMB2.ScaleOutFileServerName", new List<string>() { string.Empty });
            }

            propertiesDic.Add("SMB2.ShareWithForceLevel2WithoutSOFS", CreateShareList(GetShare(ShareFlags_Values.SHAREFLAG_FORCE_LEVELII_OPLOCK)));
            propertiesDic.Add("SMB2.SameShareWithSMBBasic", HasShare("SameWithSMBBasic") ? CreateShareList("SameWithSMBBasic") : CreateShareList(string.Empty));
            propertiesDic.Add("SMB2.DifferentShareFromSMBBasic", HasShare("DifferentFromSMBBasic") ? CreateShareList("DifferentFromSMBBasic") : CreateShareList(string.Empty));

            // If there's no directory which has the same name with the default symboliclink, leave it blank to let user input the proper name
            propertiesDic.Add("SMB2.Symboliclink", new List<string>() { detectionInfo.SymbolicLink });
            propertiesDic.Add("SMB2.SymboliclinkInSubFolder", new List<string>() { detectionInfo.SymboliclinkInSubFolder });
            #endregion

            // Add every property whose name contains "share" and does not contain "server"
            string[] cfgFiles = {"CommonTestSuite.deployment.ptfconfig",
                                 "MS-SMB2_ServerTestSuite.deployment.ptfconfig",
                                 "MS-SMB2Model_ServerTestSuite.deployment.ptfconfig",
                                 "MS-FSRVP_ServerTestSuite.deployment.ptfconfig",
                                 "ServerFailoverTestSuite.deployment.ptfconfig",
                                 "MS-DFSC_ServerTestSuite.deployment.ptfconfig",
                                 "MS-RSVD_ServerTestSuite.deployment.ptfconfig",
                                 "MS-FSA_ServerTestSuite.deployment.ptfconfig"};
            foreach (string cfgFile in cfgFiles)
            {
                foreach (string property in DetectorUtil.GetPropertiesByFile(cfgFile))
                {
                    // Exclude the irrelevant properties
                    if (property.Equals("SMB2.ShareTypeInclude_STYPE_CLUSTER_SOFS")
                        || property.Equals("Cluster.OptimumNodeOfAsymmetricShare")
                        || property.Equals("Cluster.NonOptimumNodeOfAsymmetricShare"))
                        continue;

                    if (propertiesDic.ContainsKey(property))
                        continue;

                    if (detectionInfo.shareInfo != null
                        && property.ToLower().Contains("share")
                        && !property.ToLower().Contains("server")
                        && detectionInfo.platform == Platform.NonWindows)  // For windows, keep the default value
                    {
                        propertiesDic.Add(property, CreateShareList(string.Empty));
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Get rules status
        /// </summary>
        /// <returns>All rules with status: Selected, NotSupported, Unknown or Default</returns>
        public List<CaseSelectRule> GetSelectedRules()
        {
            List<CaseSelectRule> selectedRuleList = new List<CaseSelectRule>();
            // By default, check "Select All" in "Priority" rule
            selectedRuleList.Add(new CaseSelectRule() { Name = "Priority.BVT", Status = RuleStatus.Selected });
            selectedRuleList.Add(new CaseSelectRule() { Name = "Priority.Non-BVT", Status = RuleStatus.Selected });

            selectedRuleList.Add(new CaseSelectRule() { Name = "Feature.File Server Failover", Status = RuleStatus.Unknown });
            selectedRuleList.Add(new CaseSelectRule() { Name = "Feature.DFSC (Distributed File System Referral)", Status = RuleStatus.Unknown });
            selectedRuleList.Add(new CaseSelectRule() { Name = "Feature.FSRVP (File Server Remote VSS)", Status = RuleStatus.Unknown });
            selectedRuleList.Add(new CaseSelectRule() { Name = "Feature.SWN (Service Witness)", Status = RuleStatus.Unknown });
            selectedRuleList.Add(new CaseSelectRule() { Name = "Feature.RSVD (Remote Shared Virtual Disk)", Status = RuleStatus.Unknown });
            selectedRuleList.Add(new CaseSelectRule() { Name = "Feature.Auth (Authentication and Authorization)", Status = RuleStatus.Unknown });

            selectedRuleList.Add(CreateRule("Feature.SMB2&3.Negotiate"));
            selectedRuleList.Add(CreateRule("Feature.SMB2&3.Credit", detectionInfo.smb2Info.SupportedCapabilities.HasFlag(Capabilities_Values.GLOBAL_CAP_LARGE_MTU)));
            selectedRuleList.Add(CreateRule("Feature.SMB2&3.Signing"));
            if (detectionInfo.CheckHigherDialect(detectionInfo.smb2Info.MaxSupportedDialectRevision, DialectRevision.Smb311))
            {
                selectedRuleList.Add(CreateRule("Feature.SMB2&3.Encryption", detectionInfo.smb2Info.SelectedCipherID > EncryptionAlgorithm.ENCRYPTION_NONE));
            }
            else
            {
                selectedRuleList.Add(CreateRule("Feature.SMB2&3.Encryption", detectionInfo.smb2Info.SupportedCapabilities.HasFlag(Capabilities_Values.GLOBAL_CAP_ENCRYPTION)));
            }
            selectedRuleList.Add(CreateRule("Feature.SMB2&3.DirectoryLeasing", detectionInfo.smb2Info.SupportedCapabilities.HasFlag(Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING)));
            selectedRuleList.Add(CreateRule("Feature.SMB2&3.LeaseV1", null, detectionInfo.F_Leasing_V1));
            selectedRuleList.Add(CreateRule("Feature.SMB2&3.MixedOplockLease", null, detectionInfo.F_Leasing_V1));
            selectedRuleList.Add(CreateRule("Feature.SMB2&3.LeaseV2", null, detectionInfo.F_Leasing_V2));
            selectedRuleList.Add(CreateRule("Feature.SMB2&3.MultipleChannel", detectionInfo.smb2Info.SupportedCapabilities.HasFlag(Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL)));
            selectedRuleList.Add(CreateRule("Feature.SMB2&3.Session"));
            selectedRuleList.Add(CreateRule("Feature.SMB2&3.Tree"));
            selectedRuleList.Add(CreateRule("Feature.SMB2&3.CreateClose"));
            selectedRuleList.Add(CreateRule("Feature.SMB2&3.Oplock"));
            selectedRuleList.Add(CreateRule("Feature.SMB2&3.OperateOneFileFromTwoNodes"));
            selectedRuleList.Add(CreateRule("Feature.SMB2&3.Compound"));
            selectedRuleList.Add(CreateRule("Feature.SMB2&3.ChangeNotify"));
            selectedRuleList.Add(CreateRule("Feature.SMB2&3.LockUnlock"));
            selectedRuleList.Add(CreateRule("Feature.SMB2&3.QueryAndSetFileInfo"));
            selectedRuleList.Add(CreateRule("Feature.SMB2&3.DurableHandle.PersistentHandle", detectionInfo.smb2Info.SupportedCapabilities.HasFlag(Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES)));

            selectedRuleList.Add(CreateRule("Feature.SMB2&3.DurableHandle.DurableHandleV1BatchOplock", null, detectionInfo.F_HandleV1_BatchOplock));
            selectedRuleList.Add(CreateRule("Feature.SMB2&3.DurableHandle.DurableHandleV1LeaseV1", null, detectionInfo.F_HandleV1_LeaseV1));
            selectedRuleList.Add(CreateRule("Feature.SMB2&3.DurableHandle.DurableHandleV2BatchOplock", null, detectionInfo.F_HandleV2_BatchOplock));
            selectedRuleList.Add(CreateRule("Feature.SMB2&3.DurableHandle.DurableHandleV2LeaseV1", null, detectionInfo.F_HandleV2_LeaseV1));
            selectedRuleList.Add(CreateRule("Feature.SMB2&3.DurableHandle.DurableHandleV2LeaseV2", null, detectionInfo.F_HandleV2_LeaseV2));

            selectedRuleList.Add(CreateRule("Feature.SMB2&3.AppInstanceId", null, detectionInfo.F_AppInstanceId));
            if (detectionInfo.CheckHigherDialect(detectionInfo.smb2Info.MaxSupportedDialectRevision, DialectRevision.Smb311))
            {
                // If dialect is later than 3.11 and it supports AppInstanceId, then it should support AppInstanceVersion as well.
                selectedRuleList.Add(CreateRule("Feature.SMB2&3.AppInstanceVersion", null, detectionInfo.F_AppInstanceId));
            }
            selectedRuleList.Add(CreateRule("Feature.SMB2&3.Replay", detectionInfo.smb2Info.SupportedCapabilities.HasFlag(Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL)));

            selectedRuleList.Add(CreateRule("Feature.SMB2&3.FSCTL/IOCTL.FsctlLmrRequestResiliency", null, detectionInfo.F_ResilientHandle));
            selectedRuleList.Add(CreateRule("Feature.SMB2&3.FSCTL/IOCTL.FsctlFileLevelTrim", null, detectionInfo.F_FileLevelTrim));
            selectedRuleList.Add(CreateRule("Feature.SMB2&3.FSCTL/IOCTL.FsctlValidateNegotiateInfo", null, detectionInfo.F_ValidateNegotiateInfo));
            selectedRuleList.Add(CreateRule("Feature.SMB2&3.FSCTL/IOCTL.FsctlSetGetIntegrityInformation",
                detectionInfo.F_IntegrityInfo[0] == DetectResult.Supported || detectionInfo.F_IntegrityInfo[1] == DetectResult.Supported));
            selectedRuleList.Add(CreateRule("Feature.SMB2&3.FSCTL/IOCTL.FsctlOffloadReadWrite",
                detectionInfo.F_CopyOffload[0] == DetectResult.Supported || detectionInfo.F_CopyOffload[1] == DetectResult.Supported));

            return selectedRuleList;
        }

        /// <summary>
        /// Get a summary of the detect result.
        /// </summary>
        /// <returns>Detect result.</returns>
        public object GetSUTSummary()
        {
            DetectionResultControl SUTSummaryControl = new DetectionResultControl();
            SUTSummaryControl.LoadDetectionInfo(detectionInfo);
            return SUTSummaryControl;
        }

        /// <summary>
        /// Get the list of properties that will be hidden in the configure page.
        /// </summary>
        /// <param name="rules">All rules with status: Selected, NotSupported, Unknown or Default</param>
        /// <returns>The list of properties which will not be shown in the configure page.</returns>
        public List<string> GetHiddenProperties(List<CaseSelectRule> rules)
        {
            List<string> hiddenPropertiesList = new List<string>();

            // Save all the dependant properties to a dictionary.
            // Key is the property name, value is if the property should be shown to user.
            Dictionary<string, bool> dependantProperties = new Dictionary<string, bool>();

            bool isSMB2Selected = false;
            List<CaseSelectRule> smb2Rules = new List<CaseSelectRule>();
            bool isFsaSelected = false;
            bool isClusterSwnFsrvpSelected = false;
            bool isDfscSelected = false;
            bool isRsvdSelected = false;
            bool isSqosSelected = false;
            bool isAuthSelected = false;
            foreach (CaseSelectRule rule in rules)
            {
                string ruleName = rule.Name;

                // SMB2 and FSA have many rules, put them in the beginning to reduce IF statement checker.
                if (ruleName.StartsWith("Feature.SMB2") && rule.Status == RuleStatus.Selected)
                {
                    smb2Rules.Add(rule);
                    if (rule.Status == RuleStatus.Selected)
                    {
                        isSMB2Selected = true;
                    }

                }
                else if (ruleName.StartsWith("Feature.FSA (File System Algorithms)") && rule.Status == RuleStatus.Selected)
                {
                    isFsaSelected = true;
                }
                else if ((ruleName == "Feature.File Server Failover"
                    || ruleName == "Feature.SWN (Service Witness)"
                    || ruleName == "Feature.FSRVP (File Server Remote VSS)")
                    && rule.Status == RuleStatus.Selected)
                {
                    isClusterSwnFsrvpSelected = true;
                }
                else if (ruleName == "Feature.DFSC (Distributed File System Referral)")
                {
                    if (rule.Status == RuleStatus.Selected)
                    {
                        isDfscSelected = true;
                    }
                    else
                    {
                        hiddenPropertiesList.AddRange(DetectorUtil.GetPropertiesByFile("MS-DFSC_ServerTestSuite.deployment.ptfconfig"));
                    }
                }
                else if (ruleName.StartsWith("Feature.RSVD"))
                {
                    if (rule.Status == RuleStatus.Selected)
                    {
                        isRsvdSelected = true;
                    }
                }
                else if (ruleName == "Feature.SQOS (Storage Quality of Service)")
                {
                    if (rule.Status == RuleStatus.Selected)
                    {
                        isSqosSelected = true;
                    }
                    else
                    {
                        hiddenPropertiesList.AddRange(DetectorUtil.GetPropertiesByFile("MS-SQOS_ServerTestSuite.deployment.ptfconfig"));
                    }
                }
                else if (ruleName.StartsWith("Feature.Auth") && rule.Status == RuleStatus.Selected)
                {
                    isAuthSelected = true;
                }
            }

            if (!isSMB2Selected)
            {
                hiddenPropertiesList.AddRange(DetectorUtil.GetPropertiesByFile("MS-SMB2_ServerTestSuite.deployment.ptfconfig"));
                hiddenPropertiesList.AddRange(DetectorUtil.GetPropertiesByFile("MS-SMB2Model_ServerTestSuite.deployment.ptfconfig"));
            }
            else
            {
                Type type = typeof(ConfigItemDependency);
                MethodInfo[] methodInfo = type.GetMethods();

                foreach (CaseSelectRule smb2Rule in smb2Rules)
                {
                    // Find the category which is defined in ConfigItemDependancy.cs
                    string childRuleName = GetBottomLayerRuleName(smb2Rule.Name);

                    int index = -1;
                    for (int i = 0; i < methodInfo.Count(); i++)
                    {
                        if (childRuleName == methodInfo[i].Name)
                        {
                            index = i;
                            break;
                        }
                    }

                    if (index >= 0)
                    {
                        DependencyAttribute[] attributes = methodInfo[index].GetCustomAttributes(typeof(DependencyAttribute), false) as DependencyAttribute[];
                        for (int i = 0; i < attributes.Count(); i++)
                        {
                            if (!dependantProperties.ContainsKey(attributes[i].TestCategory))
                            {
                                // Initialize to false
                                dependantProperties.Add(attributes[i].TestCategory, false);
                            }

                            // If selected, set the value to true
                            // Otherwise, does not change the value.
                            // Once it is set to true, then it should not be set back to false.
                            if (smb2Rule.Status == RuleStatus.Selected)
                            {
                                dependantProperties[attributes[i].TestCategory] = true;
                            }
                        }
                    }
                }

                if (dependantProperties.Count > 1)
                {
                    // Add the property whose value is false (false means the property should be hidden)
                    hiddenPropertiesList.AddRange(dependantProperties.Where(q => !q.Value).Select(q => q.Key));
                }
            }

            if (!isFsaSelected)
            {
                hiddenPropertiesList.AddRange(DetectorUtil.GetPropertiesByFile("MS-FSA_ServerTestSuite.deployment.ptfconfig"));
            }

            if (!isAuthSelected)
            {
                hiddenPropertiesList.AddRange(DetectorUtil.GetPropertiesByFile("Auth_ServerTestSuite.deployment.ptfconfig"));
            }

            if (!isRsvdSelected)
            {
                hiddenPropertiesList.AddRange(DetectorUtil.GetPropertiesByFile("MS-RSVD_ServerTestSuite.deployment.ptfconfig"));
            }

            // Both Cluster, SWN and FSRVP cases depend on ServerFailoverTestSuite.deployment.ptfconfig
            // Only when none of them are selected, then hide the properties.
            if (!isClusterSwnFsrvpSelected)
            {
                hiddenPropertiesList.AddRange(DetectorUtil.GetPropertiesByFile("ServerFailoverTestSuite.deployment.ptfconfig"));
            }

            // The two ptfconfig files is only used for configuring sut control adapter.
            // No need to show to the user.
            hiddenPropertiesList.AddRange(DetectorUtil.GetPropertiesByFile("CommonTestSuite.ptfconfig"));
            hiddenPropertiesList.AddRange(DetectorUtil.GetPropertiesByFile("ServerFailoverTestSuite.ptfconfig"));

            // Hide common configure
            // None are selected
            if (!isSMB2Selected && !isFsaSelected && !isClusterSwnFsrvpSelected && !isDfscSelected && !isRsvdSelected && !isSqosSelected && !isAuthSelected)
            {
                hiddenPropertiesList.AddRange(DetectorUtil.GetPropertiesByFile("CommonTestSuite.deployment.ptfconfig"));
            }
            // Only FSA is selected, hide properties that are not used by FSA
            else if (isFsaSelected && !isSMB2Selected && !isClusterSwnFsrvpSelected && !isDfscSelected && !isRsvdSelected && !isSqosSelected && !isAuthSelected)
            {
                hiddenPropertiesList.AddRange(DetectorUtil.GetPropertiesByFile("CommonTestSuite.deployment.ptfconfig"));
                hiddenPropertiesList.Remove("PTF.NetworkCapture.Enabled");
                hiddenPropertiesList.Remove("PTF.NetworkCapture.CaptureFileFolder");
                hiddenPropertiesList.Remove("PTF.NetworkCapture.StopRunningOnError");
                hiddenPropertiesList.Remove("Common.SutComputerName");
                hiddenPropertiesList.Remove("Common.SutIPAddress");
                hiddenPropertiesList.Remove("Common.DomainName");
                hiddenPropertiesList.Remove("Common.AdminUserName");
                hiddenPropertiesList.Remove("Common.PasswordForAllUsers");
                hiddenPropertiesList.Remove("Common.Platform");
                hiddenPropertiesList.Remove("Common.Timeout");
                hiddenPropertiesList.Remove("Common.SendSignedRequest");
            }
            return hiddenPropertiesList;
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

        #endregion

        #region Helper functions for Detecting SUT Info

        private bool PingSUT(FSDetector detector)
        {
            logWriter.AddLog(LogLevel.Information, "===== Ping Target SUT =====");

            try
            {
                detectionInfo.networkInfo = detector.PingTargetSUT();
            }
            catch (Exception ex)
            {
                logWriter.AddLog(LogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                logWriter.AddLog(LogLevel.Error, ex.Message);
            }

            if (detectionInfo.networkInfo.SUTIpList.Count == 0)
            {
                logWriter.AddLog(LogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                return false;
            }

            logWriter.AddLog(LogLevel.Warning, "Success", false, LogStyle.StepPassed);
            logWriter.AddLineToLog(LogLevel.Information);
            return true;
        }

        private bool DetectLocalNetworkInfo(FSDetector detector)
        {
            logWriter.AddLog(LogLevel.Information, "===== Fetch Network Info =====");

            try
            {
                detectionInfo.networkInfo = detector.FetchLocalNetworkInfo(detectionInfo);
            }
            catch (Exception ex)
            {
                logWriter.AddLog(LogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                logWriter.AddLineToLog(LogLevel.Information);
                logWriter.AddLog(LogLevel.Error, string.Format("Detect network info failed: {0} \r\nPlease check Target SUT", ex.Message));
            }

            if (detectionInfo.networkInfo == null)
            {
                logWriter.AddLog(LogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                logWriter.AddLineToLog(LogLevel.Information);
                return false;
            }

            logWriter.AddLog(LogLevel.Warning, "Finished", false, LogStyle.StepPassed);
            logWriter.AddLineToLog(LogLevel.Information);

            logWriter.AddLog(LogLevel.Information, "Target SUT Network Info:");
            logWriter.AddLog(LogLevel.Information, "Available IP Address:");
            foreach (var item in detectionInfo.networkInfo.SUTIpList)
            {
                logWriter.AddLog(LogLevel.Information, "\t" + item.ToString());
            }
            logWriter.AddLineToLog(LogLevel.Information);

            logWriter.AddLog(LogLevel.Information, "Local Test Driver Network Info:");
            logWriter.AddLog(LogLevel.Information, "Available IP Address:");
            foreach (var item in detectionInfo.networkInfo.LocalIpList)
            {
                logWriter.AddLog(LogLevel.Information, "\t" + item.ToString());
            }
            logWriter.AddLineToLog(LogLevel.Information);

            return true;
        }

        private bool CheckUsernamePassword(FSDetector detector)
        {
            logWriter.AddLog(LogLevel.Information, "===== Check the Credential =====");

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

        private void DetectPlatformAndUserAccount(FSDetector detector)
        {
            logWriter.AddLog(LogLevel.Information, "===== Detect SUT Platform and Useraccounts =====");
            detector.FetchPlatformAndUseraccounts(ref detectionInfo);
            logWriter.AddLog(LogLevel.Warning, "Finished", false, LogStyle.StepPassed);
            logWriter.AddLineToLog(LogLevel.Information);
        }

        private bool DetectSMB2Info(FSDetector detector)
        {
            logWriter.AddLog(LogLevel.Information, "===== Fetch Smb2 Info =====");

            try
            {
                detectionInfo.smb2Info = detector.FetchSmb2Info(detectionInfo);
            }
            catch (Exception ex)
            {
                logWriter.AddLog(LogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                logWriter.AddLineToLog(LogLevel.Information);
                logWriter.AddLog(LogLevel.Error, string.Format("Detect SUT SMB2 info failed: {0} \r\n", ex.Message));
            }

            if (detectionInfo.smb2Info.MaxSupportedDialectRevision == DialectRevision.Smb2Unknown || detectionInfo.smb2Info.MaxSupportedDialectRevision == DialectRevision.Smb2Wildcard)
            {
                logWriter.AddLog(LogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                logWriter.AddLineToLog(LogLevel.Information);
                logWriter.AddLog(LogLevel.Error, string.Format("The DialectRevision in Negotiate response should not be {0}", detectionInfo.smb2Info.MaxSupportedDialectRevision));
            }

            logWriter.AddLog(LogLevel.Warning, "Finished", false, LogStyle.StepPassed);
            logWriter.AddLineToLog(LogLevel.Information);

            logWriter.AddLog(LogLevel.Information, "Target SUT SMB2 Info:");
            logWriter.AddLog(LogLevel.Information, string.Format("MaxSupportedDialectRevision: {0}", detectionInfo.smb2Info.MaxSupportedDialectRevision));
            logWriter.AddLog(LogLevel.Information, string.Format("SupportedCapabilities: {0}", detectionInfo.smb2Info.SupportedCapabilities));
            logWriter.AddLineToLog(LogLevel.Information);

            return true;
        }

        private bool DetectShareInfo(FSDetector detector)
        {
            logWriter.AddLog(LogLevel.Information, "===== Fetch Share Info =====");

            try
            {
                detectionInfo.shareInfo = detector.FetchShareInfo(detectionInfo);
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

            if (detectionInfo.shareInfo != null)
            {
                logWriter.AddLog(LogLevel.Information, "Target SUT Share Info:");

                foreach (var item in detectionInfo.shareInfo)
                {
                    logWriter.AddLog(LogLevel.Information, string.Format("Share Name: {0}", item.ShareName));
                    logWriter.AddLog(LogLevel.Information, string.Format("\tShare Type: {0}", item.ShareType));
                    logWriter.AddLog(LogLevel.Information, string.Format("\tShare Flags: {0}", item.ShareFlags));
                    logWriter.AddLog(LogLevel.Information, string.Format("\tShare Capabilities: {0}", item.ShareCapabilities));
                }
                logWriter.AddLineToLog(LogLevel.Information);
            }
            return true;
        }

        private bool DetermineBasicShare()
        {
            logWriter.AddLog(LogLevel.Information, "===== Detect Basic Share =====");

            string smbBasicShare = null;
            string containBasicShare = null;
            string nonAdminShare = null;
            foreach (ShareInfo item in detectionInfo.shareInfo)
            {
                if (item.ShareName.ToLower() == "smbbasic")
                {
                    smbBasicShare = item.ShareName;
                    break;
                }

                if (item.ShareName.ToLower().Contains("basic") && ((item.ShareFlags & ShareFlags_Values.SHAREFLAG_ENCRYPT_DATA) != ShareFlags_Values.SHAREFLAG_ENCRYPT_DATA))
                {
                    containBasicShare = item.ShareName;
                }

                if ((!item.ShareName.Contains("$")) && ((item.ShareFlags & ShareFlags_Values.SHAREFLAG_ENCRYPT_DATA) != ShareFlags_Values.SHAREFLAG_ENCRYPT_DATA))
                {
                    nonAdminShare = item.ShareName;
                }
            }

            if (null == smbBasicShare && null != containBasicShare)
            {
                smbBasicShare = containBasicShare;
            }

            if (null == smbBasicShare && null != nonAdminShare)
            {
                smbBasicShare = nonAdminShare;
            }

            if (null == smbBasicShare)
            {
                logWriter.AddLog(LogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                logWriter.AddLineToLog(LogLevel.Information);
                logWriter.AddLog(LogLevel.Error, "Did not find shares as a basic share folder");
            }

            logWriter.AddLog(LogLevel.Warning, "Finished", false, LogStyle.StepPassed);
            logWriter.AddLineToLog(LogLevel.Information);
            logWriter.AddLog(LogLevel.Information, string.Format("Basic Share is: {0}", smbBasicShare));
            logWriter.AddLineToLog(LogLevel.Information);

            detectionInfo.BasicShareName = smbBasicShare;
            return true;
        }

        private void DetermineSymboliclink()
        {
            string symboliclink = DetectorUtil.GetPropertyValue("SMB2.Symboliclink");
            if (string.IsNullOrEmpty(symboliclink))
            {
                logWriter.AddLog(LogLevel.Information, "The property value of Symboliclink is empty.");
            }
            else
            {
                if (IsSymboliclink(string.Format(@"\\{0}\{1}\{2}", detectionInfo.targetSUT, detectionInfo.BasicShareName, symboliclink)))
                {
                    detectionInfo.SymbolicLink = symboliclink;
                }
            }

            string symboliclinkInSubFolder = DetectorUtil.GetPropertyValue("SMB2.SymboliclinkInSubFolder");
            if (string.IsNullOrEmpty(symboliclinkInSubFolder))
            {
                logWriter.AddLog(LogLevel.Information, "The property value of SymboliclinkInSubFolder is empty.");
            }
            else
            {
                if (IsSymboliclink(string.Format(@"\\{0}\{1}\{2}", detectionInfo.targetSUT, detectionInfo.BasicShareName, symboliclinkInSubFolder)))
                {
                    detectionInfo.SymboliclinkInSubFolder = symboliclinkInSubFolder;
                }
            }
        }

        private bool DetectIoctlCodes(FSDetector detector)
        {
            logWriter.AddLog(LogLevel.Information, "===== Detect Ioctl Codes =====");
            detectionInfo.unsupportedIoctlCodes = new List<string>();
            detectionInfo.ResetDetectResult();

            if (detectionInfo.CheckHigherDialect(detectionInfo.smb2Info.MaxSupportedDialectRevision, DialectRevision.Smb30))
            {
                try
                {
                    detectionInfo.F_CopyOffload = detector.CheckIOCTL_CopyOffload(detectionInfo.BasicShareName, ref detectionInfo);
                }
                catch (Exception ex)
                {
                    detectionInfo.F_CopyOffload = new DetectResult[] { DetectResult.DetectFail, DetectResult.DetectFail };
                    detectionInfo.detectExceptions.Add(CtlCode_Values.FSCTL_OFFLOAD_READ.ToString(), string.Format("Detect FSCTL_OFFLOAD failed: {0}", ex.Message));
                    detectionInfo.detectExceptions.Add(CtlCode_Values.FSCTL_OFFLOAD_WRITE.ToString(), string.Format("Detect FSCTL_OFFLOAD failed: {0}", ex.Message));
                }

                //Add the unsupported IoctlCodes to the list
                if (detectionInfo.F_CopyOffload[0] != DetectResult.Supported)
                    detectionInfo.unsupportedIoctlCodes.Add(CtlCode_Values.FSCTL_OFFLOAD_READ.ToString());
                if (detectionInfo.F_CopyOffload[1] != DetectResult.Supported)
                    detectionInfo.unsupportedIoctlCodes.Add(CtlCode_Values.FSCTL_OFFLOAD_WRITE.ToString());

                try
                {
                    detectionInfo.F_FileLevelTrim = detector.CheckIOCTL_FileLevelTrim(detectionInfo.BasicShareName, ref detectionInfo);
                }
                catch (Exception ex)
                {
                    detectionInfo.F_FileLevelTrim = DetectResult.DetectFail;
                    detectionInfo.detectExceptions.Add(CtlCode_Values.FSCTL_FILE_LEVEL_TRIM.ToString(), string.Format("Detect FSCTL_FILE_LEVEL_TRIM failed: {0}", ex.Message));
                }

                //Add the unsupported IoctlCodes to the list
                if (detectionInfo.F_FileLevelTrim != DetectResult.Supported)
                    detectionInfo.unsupportedIoctlCodes.Add(CtlCode_Values.FSCTL_FILE_LEVEL_TRIM.ToString());

                try
                {
                    detectionInfo.F_ValidateNegotiateInfo = detector.CheckIOCTL_ValidateNegotiateInfo("IPC$", ref detectionInfo);
                }
                catch (Exception ex)
                {
                    detectionInfo.F_ValidateNegotiateInfo = DetectResult.DetectFail;
                    if (detectionInfo.CheckHigherDialect(detectionInfo.smb2Info.MaxSupportedDialectRevision, DialectRevision.Smb311))
                    {
                        detectionInfo.detectExceptions.Add(
                            CtlCode_Values.FSCTL_VALIDATE_NEGOTIATE_INFO.ToString(),
                            "FSCTL_VALIDATE_NEGOTIATE_INFO is not applicable when the Connection.Dialect is 3.1.1.");
                    }
                    else
                    {
                        detectionInfo.detectExceptions.Add(CtlCode_Values.FSCTL_VALIDATE_NEGOTIATE_INFO.ToString(), string.Format("Detect FSCTL_VALIDATE_NEGOTIATE_INFO failed: {0}", ex.Message));
                    }
                }

                //Add the unsupported IoctlCodes to the list
                if (detectionInfo.F_ValidateNegotiateInfo != DetectResult.Supported)
                    detectionInfo.unsupportedIoctlCodes.Add(CtlCode_Values.FSCTL_VALIDATE_NEGOTIATE_INFO.ToString());
            }
            if (detectionInfo.CheckHigherDialect(detectionInfo.smb2Info.MaxSupportedDialectRevision, DialectRevision.Smb21))
            {
                try
                {
                    detectionInfo.F_ResilientHandle = detector.CheckIOCTL_ResilientHandle(detectionInfo.BasicShareName, ref detectionInfo);
                }
                catch (Exception ex)
                {
                    detectionInfo.F_ResilientHandle = DetectResult.DetectFail;
                    detectionInfo.detectExceptions.Add(CtlCode_Values.FSCTL_LMR_REQUEST_RESILIENCY.ToString(), string.Format("Detect FSCTL_LMR_REQUEST_RESILIENCY failed: {0}", ex.Message));
                }

                //Add the unsupported IoctlCodes to the list
                if (detectionInfo.F_ResilientHandle != DetectResult.Supported)
                    detectionInfo.unsupportedIoctlCodes.Add(CtlCode_Values.FSCTL_LMR_REQUEST_RESILIENCY.ToString());

                bool supported = false;
                string failedReason = null;

                // Try all shares to detect if integrity info is supported.
                foreach (var share in detectionInfo.shareInfo)
                {
                    if (!share.ShareName.Contains("$"))
                    {
                        try
                        {
                            detectionInfo.F_IntegrityInfo = detector.CheckIOCTL_IntegrityInfo(share.ShareName, ref detectionInfo);
                        }
                        catch (Exception ex)
                        {
                            // Do not need to record all failedReasons.
                            // So just pick one, the last failedReason.
                            failedReason = ex.Message;
                        }

                        if (detectionInfo.F_IntegrityInfo[0] == DetectResult.Supported)
                        {
                            supported = true;
                            detectionInfo.shareSupportingIntegrityInfo = share.ShareName;

                            // Clean up last detect exception record when detected previous shares
                            if (detectionInfo.detectExceptions.ContainsKey(CtlCode_Values.FSCTL_GET_INTEGRITY_INFORMATION.ToString()))
                                detectionInfo.detectExceptions.Remove(CtlCode_Values.FSCTL_GET_INTEGRITY_INFORMATION.ToString());
                            if (detectionInfo.detectExceptions.ContainsKey(CtlCode_Values.FSCTL_SET_INTEGRITY_INFORMATION.ToString()))
                                detectionInfo.detectExceptions.Remove(CtlCode_Values.FSCTL_SET_INTEGRITY_INFORMATION.ToString());

                            break;
                        }
                    }
                }

                if (!supported && failedReason != null && !detectionInfo.detectExceptions.ContainsKey(CtlCode_Values.FSCTL_GET_INTEGRITY_INFORMATION.ToString()))
                {
                    detectionInfo.F_IntegrityInfo = new DetectResult[] { DetectResult.DetectFail, DetectResult.DetectFail };
                    detectionInfo.detectExceptions.Add(CtlCode_Values.FSCTL_GET_INTEGRITY_INFORMATION.ToString(), string.Format("Detect FSCTL_INTEGRITY_INFORMATION failed: {0}", failedReason));
                    detectionInfo.detectExceptions.Add(CtlCode_Values.FSCTL_SET_INTEGRITY_INFORMATION.ToString(), string.Format("Detect FSCTL_INTEGRITY_INFORMATION failed: {0}", failedReason));
                }

                //Add the unsupported IoctlCodes to the list
                if (detectionInfo.F_IntegrityInfo[0] != DetectResult.Supported)
                    detectionInfo.unsupportedIoctlCodes.Add(CtlCode_Values.FSCTL_GET_INTEGRITY_INFORMATION.ToString());
                if (detectionInfo.F_IntegrityInfo[1] != DetectResult.Supported)
                    detectionInfo.unsupportedIoctlCodes.Add(CtlCode_Values.FSCTL_SET_INTEGRITY_INFORMATION.ToString());
            }

            logWriter.AddLog(LogLevel.Warning, "Finished", false, LogStyle.StepPassed);
            logWriter.AddLineToLog(LogLevel.Information);

            return true;
        }

        private void DetectCreateContexts(FSDetector detector)
        {
            logWriter.AddLog(LogLevel.Information, "===== Detect Create Contexts =====");

            detectionInfo.unsupportedCreateContexts = new List<string>();

            if (detectionInfo.CheckHigherDialect(detectionInfo.smb2Info.MaxSupportedDialectRevision, DialectRevision.Smb30))
            {
                try
                {
                    detectionInfo.F_AppInstanceId = detector.CheckCreateContexts_AppInstanceId(detectionInfo.BasicShareName, ref detectionInfo);
                }
                catch (Exception ex)
                {
                    detectionInfo.detectExceptions.Add(
                        CreateContextTypeValue.SMB2_CREATE_APP_INSTANCE_ID.ToString(),
                        string.Format("Detect SMB2_CREATE_APP_INSTANCE_ID failed: {0}",
                        ex.Message));
                }

                // Add the unsupported CreateContexts to the list
                if (detectionInfo.F_AppInstanceId != DetectResult.Supported)
                {
                    detectionInfo.unsupportedCreateContexts.Add(CreateContextTypeValue.SMB2_CREATE_APP_INSTANCE_ID.ToString());
                }

                try
                {
                    detectionInfo.F_Leasing_V2 = detector.CheckCreateContexts_LeaseV2(detectionInfo.BasicShareName, ref detectionInfo);
                }
                catch (Exception ex)
                {
                    detectionInfo.detectExceptions.Add(
                        CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE_V2.ToString(),
                        string.Format("Detect SMB2_CREATE_REQUEST_LEASE_V2 failed: {0}",
                        ex.Message));
                }

                // Add the unsupported CreateContexts to the list
                if (detectionInfo.F_Leasing_V2 != DetectResult.Supported)
                {
                    detectionInfo.unsupportedCreateContexts.Add(CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE_V2.ToString());
                }

                string failedReason = null;
                try
                {
                    detectionInfo.F_HandleV2_BatchOplock = detector.CheckCreateContexts_HandleV2BatchOplock(detectionInfo.BasicShareName, ref detectionInfo);
                }
                catch (Exception ex)
                {
                    failedReason = ex.Message;
                }

                try
                {
                    detectionInfo.F_HandleV2_LeaseV1 = detector.CheckCreateContexts_HandleV2LeaseV1(detectionInfo.BasicShareName, ref detectionInfo);
                }
                catch (Exception ex)
                {
                    failedReason = ex.Message;
                }

                try
                {
                    detectionInfo.F_HandleV2_LeaseV2 = detector.CheckCreateContexts_HandleV2LeaseV2(detectionInfo.BasicShareName, ref detectionInfo);
                }
                catch (Exception ex)
                {
                    failedReason = ex.Message;
                }

                if (detectionInfo.F_HandleV2_BatchOplock != DetectResult.Supported
                    && detectionInfo.F_HandleV2_LeaseV1 != DetectResult.Supported
                    && detectionInfo.F_HandleV2_LeaseV2 != DetectResult.Supported)
                {
                    detectionInfo.unsupportedCreateContexts.Add(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2.ToString());
                    detectionInfo.unsupportedCreateContexts.Add(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2.ToString());
                    if (failedReason != null)
                    {
                        detectionInfo.detectExceptions.Add(
                            CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2.ToString(),
                            string.Format("Detect Durable Handle V2 failed: {0}",
                            failedReason));
                        detectionInfo.detectExceptions.Add(
                            CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2.ToString(),
                            string.Format("Detect Durable Handle V2 failed: {0}",
                            failedReason));
                    }
                }
            }

            if (detectionInfo.CheckHigherDialect(detectionInfo.smb2Info.MaxSupportedDialectRevision, DialectRevision.Smb21))
            {
                try
                {
                    detectionInfo.F_Leasing_V1 = detector.CheckCreateContexts_LeaseV1(detectionInfo.BasicShareName, ref detectionInfo);
                }
                catch (Exception ex)
                {
                    detectionInfo.detectExceptions.Add(
                        CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE.ToString(),
                        string.Format("Detect SMB2_CREATE_REQUEST_LEASE failed: {0}",
                        ex.Message));
                }

                // Add the unsupported CreateContexts to the list
                if (detectionInfo.F_Leasing_V1 != DetectResult.Supported)
                {
                    detectionInfo.unsupportedCreateContexts.Add(CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE.ToString());
                }

                string failedReason = null;
                try
                {
                    detectionInfo.F_HandleV1_BatchOplock = detector.CheckCreateContexts_HandleV1BatchOplock(
                        detectionInfo.BasicShareName,
                        detectionInfo.smb2Info.MaxSupportedDialectRevision,
                        ref detectionInfo);
                }
                catch (Exception ex)
                {
                    failedReason = ex.Message;
                }

                try
                {
                    detectionInfo.F_HandleV1_LeaseV1 = detector.CheckCreateContexts_HandleV1LeaseV1(detectionInfo.BasicShareName, ref detectionInfo);
                }
                catch (Exception ex)
                {
                    failedReason = ex.Message;
                }

                // Add the unsupported CreateContexts to the list
                if (detectionInfo.F_HandleV1_BatchOplock != DetectResult.Supported
                    && detectionInfo.F_HandleV1_LeaseV1 != DetectResult.Supported)
                {
                    detectionInfo.unsupportedCreateContexts.Add(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST.ToString());
                    detectionInfo.unsupportedCreateContexts.Add(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT.ToString());
                    if (failedReason != null)
                    {
                        detectionInfo.detectExceptions.Add(
                            CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST.ToString(),
                            string.Format("Detect Durable Handle V1 failed: {0}",
                            failedReason));
                        detectionInfo.detectExceptions.Add(
                            CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT.ToString(),
                            string.Format("Detect Durable Handle V1 failed: {0}",
                            failedReason));
                    }
                }
            }

            logWriter.AddLog(LogLevel.Warning, "Finished", false, LogStyle.StepPassed);
            logWriter.AddLineToLog(LogLevel.Information);
        }

        #endregion

        #region Helper functions for Getting Detected Results

        private string GetShare(ShareFlags_Values shareFlags, Share_Capabilities_Values shareCap = Share_Capabilities_Values.NONE)
        {
            if (detectionInfo.shareInfo != null)
            {
                foreach (var item in detectionInfo.shareInfo)
                {
                    ShareFlags_Values itemshareFlags = item.ShareFlags & shareFlags;
                    Share_Capabilities_Values itemshareCap = item.ShareCapabilities & shareCap;
                    if (item.ShareType == ShareType_Values.SHARE_TYPE_DISK && itemshareFlags == shareFlags && itemshareCap == shareCap)
                        return item.ShareName;
                }
            }
            return string.Empty;
        }

        private string GetSpecialShare()
        {
            if (detectionInfo.shareInfo != null)
            {
                foreach (var item in detectionInfo.shareInfo)
                {
                    if (item.ShareName.Equals("ADMIN$"))
                        return "ADMIN$";
                }
            }
            return string.Empty;
        }

        private List<string> GetAsymmetricShare()
        {
            List<string> asymmetricShareList = new List<string>();
            if (detectionInfo.shareInfo == null)
                return asymmetricShareList;
            foreach (ShareInfo share in detectionInfo.shareInfo)
            {
                if (share.ShareCapabilities.HasFlag(Share_Capabilities_Values.SHARE_CAP_ASYMMETRIC))
                    asymmetricShareList.Add(share.ShareName);
            }
            return asymmetricShareList;
        }

        private List<string> CreateShareList(params string[] shareNames)
        {
            List<string> shareList = new List<string>();

            if (shareNames.Length != 0)
                shareList.AddRange(shareNames);

            //Put the the basic share name prior to others
            if (!string.IsNullOrEmpty(detectionInfo.BasicShareName))
                shareList.Add(detectionInfo.BasicShareName);

            foreach (ShareInfo share in detectionInfo.shareInfo)
            {
                if (!shareList.Contains(share.ShareName))
                    shareList.Add(share.ShareName);
            }

            return shareList;
        }

        private bool HasShare(string shareName)
        {
            if (shareName == null)
                return false;

            if (detectionInfo.shareInfo != null)
            {
                foreach (var item in detectionInfo.shareInfo)
                {
                    if (item.ShareName.ToLower() == shareName.ToLower())
                        return true;
                }
            }
            return false;
        }

        private string GetUnsupportedItems(List<string> itemList)
        {
            StringBuilder itemsSb = new StringBuilder();

            foreach (var item in itemList)
            {
                if (itemsSb.Length == 0)
                    itemsSb.Append(item);
                else
                    itemsSb.Append(";" + item);
            }

            return itemsSb.ToString();
        }

        private bool IsSymboliclink(string path)
        {
            logWriter.AddLog(LogLevel.Information, "Start checking: " + path);
            FileInfo fileInfo = new FileInfo(path);
            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            bool isFile = fileInfo.Exists;
            bool isDirectory = directoryInfo.Exists;

            if (isFile)
            {
                logWriter.AddLog(LogLevel.Information, path + " is a file.");
                if (fileInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
                {
                    logWriter.AddLog(LogLevel.Information, path + " is a symboliclink and its target is a file.");
                    return true;
                }
            }
            else if (isDirectory)
            {
                logWriter.AddLog(LogLevel.Information, path + " is a directory.");
                if (directoryInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
                {
                    logWriter.AddLog(LogLevel.Information, path + " is a symboliclink and its target is a directory.");
                    return true;
                }
            }
            else
            {
                logWriter.AddLog(LogLevel.Information, path + " is neither a file nor a directory.");
            }

            logWriter.AddLog(LogLevel.Information, path + " is NOT a symboliclink.");
            return false;
        }

        private CaseSelectRule CreateRule(string ruleCategoryName, bool? hasFlag = true, DetectResult detectResult = DetectResult.Supported)
        {
            CaseSelectRule rule = null;

            if (hasFlag == null)
            {
                switch (detectResult)
                {
                    case DetectResult.Supported:
                        rule = new CaseSelectRule() { Name = ruleCategoryName, Status = RuleStatus.Selected };
                        break;
                    case DetectResult.DetectFail:
                        rule = new CaseSelectRule() { Name = ruleCategoryName, Status = RuleStatus.NotSupported };
                        break;
                    case DetectResult.UnSupported:
                        rule = new CaseSelectRule() { Name = ruleCategoryName, Status = RuleStatus.NotSupported };
                        break;
                    default:
                        break;
                }
            }
            else if (hasFlag == true)
                rule = new CaseSelectRule() { Name = ruleCategoryName, Status = RuleStatus.Selected };
            else
                rule = new CaseSelectRule() { Name = ruleCategoryName, Status = RuleStatus.NotSupported };

            return rule;
        }

        private string GetBottomLayerRuleName(string fullName)
        {
            string[] rules = fullName.Split('.');
            return rules[rules.Length - 1];
        }

        #endregion
    }
}
