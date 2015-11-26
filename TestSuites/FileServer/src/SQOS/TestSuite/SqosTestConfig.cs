// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.FileSharing.SQOS.TestSuite
{
    public class SqosTestConfig: TestConfigBase
    {
        #region Fields
        private string fileServerNameContainingSqosVHD;
        private string shareContainingSqosVHD;
        private string nameOfSqosVHD;
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

        public IPAddress FileServerIPContainingSqosVHD
        {
            get
            {
                IPAddress fileServerIPContainingSqosVHD;
                // If FileServerName in full path of SQOS vhd file is an IP address, use that one directly.
                if (IPAddress.TryParse(FileServerNameContainingSqosVHD, out fileServerIPContainingSqosVHD))
                {
                    return fileServerIPContainingSqosVHD;
                }
                else
                {
                    return Dns.GetHostEntry(FileServerNameContainingSqosVHD).AddressList[0];
                }
            }
        }

        public string FileServerNameContainingSqosVHD
        {
            get
            {
                if (fileServerNameContainingSqosVHD == null)
                {
                    ParseSqosFullPath();
                }
                return fileServerNameContainingSqosVHD;
            }
        }

        public string ShareContainingSqosVHD
        {
            get
            {
                if (shareContainingSqosVHD == null)
                {
                    ParseSqosFullPath();
                }
                return shareContainingSqosVHD;
            }
        }

        public string NameOfSqosVHD
        {
            get
            {
                if (nameOfSqosVHD == null)
                {
                    ParseSqosFullPath();
                }
                return nameOfSqosVHD;
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

        private void ParseSqosFullPath()
        {
            // Parse full path to separate properties.
            string fullPath = GetProperty("SqosVHDFullPath");
            if (!fullPath.StartsWith(@"\\"))
            {
                Site.Assert.Inconclusive(@"SqosVHDFullPath should start with \\");
            }

            fullPath = fullPath.Substring(2);
            fileServerNameContainingSqosVHD = fullPath.Substring(0, fullPath.IndexOf(@"\"));
            fullPath = fullPath.Substring(fileServerNameContainingSqosVHD.Length + 1);
            shareContainingSqosVHD = fullPath.Substring(0, fullPath.IndexOf(@"\"));
            nameOfSqosVHD = fullPath.Substring(shareContainingSqosVHD.Length + 1);
        }

        public SqosTestConfig(ITestSite site):base(site)
        {
        }
    }
}
