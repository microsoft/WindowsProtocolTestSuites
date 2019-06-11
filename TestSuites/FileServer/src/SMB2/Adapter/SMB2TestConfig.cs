// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using System.Globalization;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.Adapter
{
    public class SMB2TestConfig : TestConfigBase
    {
        /// <summary>
        /// Get property value from grouped ptf config.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="checkNullOrEmpty">Check if the property is null or the value is empty.</param>
        /// <returns>The value of the property.</returns>
        public string GetProperty(string propertyName, bool checkNullOrEmpty = true)
        {
            return GetProperty("SMB2", propertyName, checkNullOrEmpty);
        }

        public int WaitTimeoutInMilliseconds
        {
            get
            {
                return Int32.Parse(GetProperty("WaitTimeoutInMilliseconds"));
            }
        }

        public IPAddress SutAlternativeIPAddress
        {
            get
            {
                var result = GetProperty("SutAlternativeIPAddress").ParseSecondaryIPAddress();
                Site.Assume.IsTrue(result != IPAddress.None, "SutAlternativeIPAddress should be a valid IP address or a resolvable host name with at least two IP addresses!");
                return result;
            }
        }

        public string FileShareSupportingIntegrityInfo
        {
            get
            {
                return GetProperty("FileShareSupportingIntegrityInfo");
            }
        }

        public uint NumberOfPreviousVersions
        {
            get
            {
                return uint.Parse(GetProperty("NumberOfPreviousVersions"));
            }
        }

        public string DriverComputerName
        {
            get
            {
                return GetProperty("DriverComputerName");
            }
        }

        public string ClusteredInfrastructureFileServerName
        {
            get
            {
                return GetProperty("ClusteredInfrastructureFileServerName");
            }
        }

        public string InfrastructureRootShare
        {
            get
            {
                return GetProperty("InfrastructureRootShare");
            }
        }

        #region Symbolic link configuration
        public string Symboliclink
        {
            get
            {
                return GetProperty("Symboliclink");
            }
        }

        public string SymboliclinkInSubFolder
        {
            get
            {
                return GetProperty("SymboliclinkInSubFolder");
            }
        }
        #endregion

        #region HRVS

        #region Private Fields
        private string sharePath;
        private string shareServerName;
        private string shareName;
        private IPAddress shareServerIP;
        private bool isOffLoadImplemented;
        #endregion

        #region Variable
        public string SharePath
        {
            get
            {
                if (sharePath == null)
                {
                    sharePath = GetProperty("HVRS", "SharePath");
                }
                return sharePath;
            }
        }

        public string ShareServerName
        {
            get
            {
                if (shareServerName == null)
                {
                    ParseSharePath();
                }
                return shareServerName;
            }
        }

        /// <summary>
        /// Name of the share containing the shared virtual disk file
        /// </summary>
        public string ShareName
        {
            get
            {
                if (shareName == null)
                {
                    ParseSharePath();
                }
                return shareName;
            }
        }

        public IPAddress ShareServerIP
        {
            get
            {
                if (shareServerIP == null)
                {
                    shareServerIP = ShareServerName.ParseIPAddress();
                    Site.Assume.IsTrue(shareServerIP != IPAddress.None, "ShareServerName should be a valid IP address or a resolvable host name!");
                }
                return shareServerIP;
            }
        }

        public bool IsOffLoadImplemented
        {
            get
            {
                if (isOffLoadImplemented == false)
                {
                    isOffLoadImplemented = bool.Parse(GetProperty("HVRS", "IsOffLoadImplemented"));
                }
                return isOffLoadImplemented;
            }
        }

        public bool IsSetZeroDataImplemented
        {
            get
            {
                return bool.Parse(GetProperty("HVRS", "IsSetZeroDataImplemented"));
            }
        }

        public int VolumnClusterSize
        {
            get
            {
                return int.Parse(GetProperty("HVRS", "VolumnClusterSize"));
            }
        }
        #endregion

        #region Methods
        private void ParseSharePath()
        {
            string sharePath = GetProperty("HVRS", "SharePath");
            if (!sharePath.StartsWith(@"\\"))
            {
                Site.Assert.Inconclusive(@"ShareContainingSharedVHD should start with \\");
            }
            sharePath = sharePath.Trim('\\');
            shareServerName = sharePath.Substring(0, sharePath.IndexOf(@"\"));
            shareName = sharePath.Substring(shareServerName.Length + 1);
        }
        #endregion 

        #endregion
        public SMB2TestConfig(ITestSite site) : base(site)
        {
        }
    }
}
