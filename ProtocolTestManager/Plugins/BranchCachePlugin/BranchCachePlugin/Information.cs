// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestManager.BranchCachePlugin
{
    /// <summary>
    /// The detect result supported
    /// </summary>
    public enum DetectResult
    {
        /// <summary>
        /// The detected result is supported by SUT
        /// </summary>
        Supported,

        /// <summary>
        /// The detected result is not supported by SUT
        /// </summary>
        UnSupported,

        /// <summary>
        /// The detect process failed
        /// </summary>
        DetectFail,
    }

    /// <summary>
    /// Platform type of the SUT
    /// </summary>
    public enum Platform
    {
        /// <summary>
        /// Non Windows implementation
        /// </summary>
        NonWindows = 0x00000000,

        /// <summary>
        /// Windows Server 2008
        /// </summary>
        WindowsServer2008 = 0x10000002,

        /// <summary>
        /// Windows Server 2008 R2
        /// </summary>
        WindowsServer2008R2 = 0x10000004,

        /// <summary>
        /// Windows Server 2012
        /// </summary>
        WindowsServer2012 = 0x10000006,

        /// <summary>
        /// Windows Server 2012 R2
        /// </summary>
        WindowsServer2012R2 = 0x10000007,
    }

    /// <summary>
    /// The branch cahce version SUT support
    /// </summary>
    [Flags]
    public enum BranchCacheVersion
    {
        /// <summary>
        /// Branch cache not supports
        /// </summary>
        NotSupported,

        /// <summary>
        /// Only branch cache version1 supported
        /// </summary>
        BranchCacheVersion1,

        /// <summary>
        /// Only branch cache version2 supported
        /// </summary>
        BranchCacheVersion2
    }

    /// <summary>
    /// The hash generation SUT support
    /// </summary>
    [Flags]
    public enum ShareHashGeneration
    {
        /// <summary>
        /// SUT does not support hash generation
        /// </summary>
        NotEnabled,

        /// <summary>
        /// SUT only supports hash generation version1
        /// </summary>
        V1Enabled,

        /// <summary>
        /// SUT only supports SUT hash generation version2
        /// </summary>
        V2Enabled
    }
    
    /// <summary>
    /// The detection information
    /// </summary>
    public class DetectionInfo
    {
        //SUT info
        public string TargetSUT;
        public string ContentServerName;
        public string HostedCacheServerName;

        //Credential info
        public string Domain;
        public string UserName;
        public string Password;

        public string FileShareLocation;

        //Transport info. Possible values: SMB2, PCCRTP
        public string SelectedTransport;

        //Platform and Network info
        public Platform SUTPlatform;
        public NetworkInfo ContentServerNetworkInformation;
        public NetworkInfo HostedCacheServerNetworkInfo;

        //Share info
        public ShareInfo ShareInformation;

        //BranchCache Version info
        public VersionInfo VersionInformation;

        //Force hash generation supported
        public bool IsWebsiteForcedHashGenerationSupported;
        public bool IsFileShareForcedHashGenerationSupported;

        //Website local path info
        public string WebsiteLocalPath;        

        public Dictionary<string, string> DetectExceptions;
        public List<string> UnsupportedItems;

        /// <summary>
        /// Construct
        /// </summary>
        public DetectionInfo()
        {
            ResetDetectResult();
        }

        /// <summary>
        /// Reset the detected result
        /// </summary>
        public void ResetDetectResult()
        {
            DetectExceptions = new Dictionary<string, string>();

            Domain = string.Empty;
            UserName = string.Empty;
            Password = string.Empty;

            ContentServerName = string.Empty;
            HostedCacheServerName = string.Empty;

            SelectedTransport = string.Empty;
            FileShareLocation = string.Empty;

            ShareInformation = new ShareInfo();
            ShareInformation.ShareName = string.Empty;
            ShareInformation.shareHashGeneration = ShareHashGeneration.NotEnabled;

            VersionInformation = new VersionInfo();
            VersionInformation.branchCacheVersion = BranchCacheVersion.NotSupported;

            IsWebsiteForcedHashGenerationSupported = false;
            IsFileShareForcedHashGenerationSupported = true;

            WebsiteLocalPath=string.Empty;
        }       
    }
}
