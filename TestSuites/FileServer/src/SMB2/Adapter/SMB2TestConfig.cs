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

        public SMB2TestConfig(ITestSite site):base(site)
        {
        }
    }
}
