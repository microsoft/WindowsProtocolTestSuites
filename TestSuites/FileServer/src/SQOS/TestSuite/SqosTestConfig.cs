// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Sqos;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.FileSharing.SQOS.TestSuite
{
    public class SqosTestConfig: TestConfigBase
    {
        #region Fields
        private string fileServerNameContainingSharedVHD;
        private string shareContainingSharedVHD;
        #endregion

        /// <summary>
        /// Get property value from grouped ptf config.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="checkNullOrEmpty">Check if the property is null or the value is empty.</param>
        /// <returns>The value of the property.</returns>
        public string GetProperty(string propertyName, bool checkNullOrEmpty = true)
        {
            return GetProperty("SQOS", propertyName, checkNullOrEmpty);
        }

        public IPAddress FileServerIPContainingSharedVHD
        {
            get
            {
                IPAddress fileServerIPContainingSharedVHD;
                // If FileServerName in full path of SQOS vhd file is an IP address, use that one directly.
                if (IPAddress.TryParse(FileServerNameContainingSharedVHD, out fileServerIPContainingSharedVHD))
                {
                    return fileServerIPContainingSharedVHD;
                }
                else
                {
                    return Dns.GetHostEntry(FileServerNameContainingSharedVHD).AddressList[0];
                }
            }
        }

        public string FileServerNameContainingSharedVHD
        {
            get
            {
                if (fileServerNameContainingSharedVHD == null)
                {
                    ParseSqosFullPath();
                }
                return fileServerNameContainingSharedVHD;
            }
        }

        public string ShareContainingSharedVHD
        {
            get
            {
                if (shareContainingSharedVHD == null)
                {
                    ParseSqosFullPath();
                }
                return shareContainingSharedVHD;
            }
        }

        public string NameOfSharedVHD
        {
            get
            {   
                return "test.vhdx";
            }
        }

        public Guid SqosPolicyId
        {
            get
            {
                return Guid.Parse(GetProperty("SqosPolicyId"));
            }
        }

        public string SqosInitiatorNodeName
        {
            get
            {
                return System.Net.Dns.GetHostName();
            }
        }

        public string SqosInitiatorName
        {
            get
            {
                return GetProperty("SqosInitiatorName");
            }
        }

        public ulong SqosMaximumIoRate
        {
            get
            {
                return ulong.Parse(GetProperty("SqosMaximumIoRate"));
            }
        }

        public ulong SqosMinimumIoRate
        {
            get
            {
                return ulong.Parse(GetProperty("SqosMinimumIoRate"));
            }
        }

        public uint SqosBaseIoSize
        {
            get
            {
                return uint.Parse(GetProperty("SqosBaseIoSizeInBytes"));
            }
        }

        public ulong SqosMaximumBandwidth
        {
            get
            {
                return ulong.Parse(GetProperty("SqosMaximumBandwidth"));
            }
        }

        public SQOS_PROTOCOL_VERSION SqosClientDialect
        {
            get
            {
                return ParsePropertyToEnum<SQOS_PROTOCOL_VERSION>(GetProperty("SqosClientDialect"), "SqosClientDialect");
            }
        }

        public string FullPathShareContainingSharedVHD
        {
            get
            {
                return GetProperty("ShareContainingSharedVHD");
            }
        }

        private void ParseSqosFullPath()
        {
            // Parse full path to separate properties.
            string fullPath = GetProperty("ShareContainingSharedVHD");
            if (!fullPath.StartsWith(@"\\"))
            {
                Site.Assert.Inconclusive(@"ShareContainingSharedVHD should start with \\");
            }

            fullPath = fullPath.Substring(2);
            fileServerNameContainingSharedVHD = fullPath.Substring(0, fullPath.IndexOf(@"\"));
            shareContainingSharedVHD = fullPath.Substring(fileServerNameContainingSharedVHD.Length + 1);
        }

        public SqosTestConfig(ITestSite site):base(site)
        {
        }
    }
}
