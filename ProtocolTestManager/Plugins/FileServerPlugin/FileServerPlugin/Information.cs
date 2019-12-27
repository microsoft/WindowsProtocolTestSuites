// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rsvd;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Sqos;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestManager.FileServerPlugin
{
    /// <summary>
    /// Result enumeration.
    /// </summary>
    public enum DetectResult
    {
        DetectFail,
        Supported,
        UnSupported,
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

        /// <summary>
        /// Windows Server 2016
        /// </summary>
        WindowsServer2016 = 0x10000008,

        /// <summary>
        /// Windows Server v1709
        /// </summary>
        WindowsServerV1709 = 0x10000009,

        /// <summary>
        /// Windows Server v1803
        /// </summary>
        WindowsServerV1803 = 0x1000000A,

        /// <summary>
        /// Windows Server 2019 
        /// </summary>
        WindowsServer2019 = 0x1000000B,

        /// <summary>
        /// Windows Server v1903
        /// </summary>
        WindowsServerV1903 = 0x1000000C,


    }

    /// <summary>
    /// This class defines the structure of detection information.
    /// </summary>
    public class DetectionInfo
    {
        #region Properties

        public string targetShareFullPath;
        public string targetSUT;
        public string domainName;
        public string userName;
        public string password;
        public SecurityPackageType securityPackageType;

        public DetectResult[] F_CopyOffload;
        public DetectResult[] F_IntegrityInfo;
        public DetectResult F_FileLevelTrim;
        public DetectResult F_ResilientHandle;
        public DetectResult F_ValidateNegotiateInfo;
        public DetectResult F_AppInstanceId;
        public DetectResult F_HandleV1_BatchOplock;
        public DetectResult F_HandleV1_LeaseV1;
        public DetectResult F_HandleV2_BatchOplock;
        public DetectResult F_HandleV2_LeaseV1;
        public DetectResult F_HandleV2_LeaseV2;
        public DetectResult F_Leasing_V1;
        public DetectResult F_Leasing_V2;
        public DetectResult F_EnumerateSnapShots;

        public DetectResult SqosSupport;
        public DetectResult RsvdSupport;
        public SQOS_PROTOCOL_VERSION SqosVersion;
        public RSVD_PROTOCOL_VERSION RsvdVersion;

        // The dialect array for Negotiation.
        public DialectRevision[] requestDialect;

        // Network information.
        public NetworkInfo networkInfo;

        public Platform platform;

        public List<string> nonadminUserAccounts;

        // Guest user account, set to empty as default
        public string guestUserAccount = string.Empty;

        public Smb2Info smb2Info;

        public ShareInfo[] shareInfo;

        public List<string> shareListWithForceLevel2AndSOFS = new List<string>();

        public List<string> shareListWithForceLevel2WithoutSOFS = new List<string>();

        public List<string> shareListWithoutForceLevel2WithSOFS = new List<string>();

        public List<string> shareListWithoutForceLevel2OrSOFS = new List<string>();

        public string BasicShareName;

        public string SymbolicLink = string.Empty;

        public string SymboliclinkInSubFolder = string.Empty;

        public Dictionary<string, string> detectExceptions;

        public List<string> unsupportedIoctlCodes;

        public List<string> unsupportedCreateContexts;

        public string shareSupportingIntegrityInfo;

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public DetectionInfo()
        {
            ResetDetectResult();
            Array allDialects = Enum.GetValues(typeof(DialectRevision));
            requestDialect = new DialectRevision[allDialects.Length - 2];
            int index = 0;
            foreach (var dialect in allDialects)
            {
                if ((DialectRevision)dialect != DialectRevision.Smb2Unknown && (DialectRevision)dialect != DialectRevision.Smb2Wildcard)
                {
                    requestDialect[index++] = (DialectRevision)dialect;
                }
            }
        }

        /// <summary>
        /// To reset the detection information.
        /// </summary>
        public void ResetDetectResult()
        {
            F_CopyOffload = new DetectResult[] { DetectResult.UnSupported, DetectResult.UnSupported };
            F_IntegrityInfo = new DetectResult[] { DetectResult.UnSupported, DetectResult.UnSupported };
            F_FileLevelTrim = DetectResult.UnSupported;
            F_ResilientHandle = DetectResult.UnSupported;
            F_ValidateNegotiateInfo = DetectResult.UnSupported;
            F_EnumerateSnapShots = DetectResult.UnSupported;
            F_AppInstanceId = DetectResult.UnSupported;
            F_HandleV1_BatchOplock = DetectResult.UnSupported;
            F_HandleV1_LeaseV1 = DetectResult.UnSupported;
            F_HandleV2_BatchOplock = DetectResult.UnSupported;
            F_HandleV2_LeaseV1 = DetectResult.UnSupported;
            F_HandleV2_LeaseV2 = DetectResult.UnSupported;
            F_Leasing_V1 = DetectResult.UnSupported;
            F_Leasing_V2 = DetectResult.UnSupported;
            RsvdVersion = 0;
            SqosVersion = 0;            
    }

        public bool CheckHigherDialect(DialectRevision MaxSupported, DialectRevision ComTarget)
        {
            if (MaxSupported == DialectRevision.Smb2Wildcard || MaxSupported == DialectRevision.Smb2Unknown)
                return false;
            else
            {
                if (MaxSupported.GetHashCode() < ComTarget.GetHashCode())
                    return false;
                else
                    return true;
            }
        }
    }
}
