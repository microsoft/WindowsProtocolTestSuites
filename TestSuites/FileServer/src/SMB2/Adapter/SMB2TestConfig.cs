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
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rsvd;
using System.Globalization;

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
                string ipaddress = GetProperty("SutAlternativeIPAddress", false);
                if (string.IsNullOrEmpty(ipaddress))
                {
                    return IPAddress.None;
                }
                return IPAddress.Parse(ipaddress);
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
        private string nameOfSqosVHD;
        private string initiatorHostName;
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
                    if (IPAddress.TryParse(ShareServerName, out shareServerIP))
                    {
                        return shareServerIP;
                    }
                    else
                    {
                        try
                        {
                            shareServerIP = Dns.GetHostEntry(ShareServerName).AddressList[0];
                        }
                        catch
                        {
                            throw new Exception(string.Format("Cannot resolve IP address of CAShareServerName ({0}) from DNS.", ShareServerName));
                        }

                    }
                }
                return shareServerIP;
            }
        }

        /// <summary>
        /// Specifies the computer name on which the initiator resides
        /// </summary>
        public string InitiatorHostName
        {
            get
            {
                if (initiatorHostName == null)
                {
                    initiatorHostName = System.Net.Dns.GetHostName();
                }
                return initiatorHostName;
            }
        }

        /// <summary>
        /// Specifies the highest protocol version supported by the server
        /// </summary>
        public uint ServerServiceVersion
        {
            get
            {
                return ParseUint("RsvdServerServiceVersion", "RSVD");
            }
        }

        public string NameOfSqosVHD
        {
            get
            {
                if (nameOfSqosVHD == null)
                {
                    nameOfSqosVHD = GetProperty("SQOS", "SqosVHDName");
                }
                return nameOfSqosVHD;
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
        /// <summary>
        /// Get property value from grouped ptf config.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="checkNullOrEmpty">Check if the property is null or the value is empty.</param>
        /// <returns>The value of the property.</returns>
        //public string GetProperty(string propertyName, string groupName = "HVRS", bool checkNullOrEmpty = true)
        //{
        //    return GetProperty(groupName, propertyName, checkNullOrEmpty);
        //}

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

        private ulong ParseUlong(string propertyName, string groupName = "RSVD")
        {
            if (GetProperty(groupName, propertyName).StartsWith("0x"))
                return ulong.Parse(GetProperty(groupName, propertyName).Substring(2), NumberStyles.HexNumber);
            else
                return ulong.Parse(GetProperty(groupName, propertyName));
        }

        private uint ParseUint(string propertyName, string groupName = "RSVD")
        {
            if (GetProperty(groupName, propertyName).StartsWith("0x"))
                return uint.Parse(GetProperty(groupName, propertyName).Substring(2), NumberStyles.HexNumber);
            else
                return uint.Parse(GetProperty(groupName, propertyName));
        }

        #endregion 

        #endregion
        public SMB2TestConfig(ITestSite site):base(site)
        {
        }
    }
}
