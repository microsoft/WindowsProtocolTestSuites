// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rsvd;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestSuites.FileSharing.RSVD.TestSuite
{
    public class RSVDTestConfig : TestConfigBase
    {
        #region Field
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
            return GetProperty("RSVD", propertyName, checkNullOrEmpty);
        }

        /// <summary>
        /// Name of the file server containing the shared virtual disk file
        /// </summary>
        public string FileServerNameContainingSharedVHD
        {
            get
            {
                return fileServerNameContainingSharedVHD;
            }
        }

        /// <summary>
        /// IP address of the file server containing the shared virtual disk file
        /// </summary>
        public IPAddress FileServerIPContainingSharedVHD
        {
            get
            {
                var result = GetProperty("FileServerIPContainingSharedVHD").ParseIPAddress();
                Site.Assume.IsTrue(result != IPAddress.None, "FileServerIPContainingSharedVHD should be a valid IP address or a resolvable host name!");
                return result;
            }
        }

        /// <summary>
        /// Name of the share containing the shared virtual disk file
        /// </summary>
        public string ShareContainingSharedVHD
        {
            get
            {
                return shareContainingSharedVHD;
            }
        }

        /// <summary>
        /// Name of the shared virtual disk file
        /// </summary>
        public string NameOfSharedVHDX
        {
            get
            {
                return "test.vhdx";
            }
        }

        /// <summary>
        /// Name of the shared virtual disk set file
        /// </summary>
        public string NameOfSharedVHDS
        {
            get
            {
                return "rsvd.vhds";
            }
        }

        /// <summary>
        /// Name of the shared virtual disk set file which is converted from a virtual disk file
        /// </summary>
        public string NameOfConvertedSharedVHDS
        {
            get
            {
                return "ConvertedSharedVHDSName.vhds";
            }
        }

        /// <summary>
        /// Specifies the computer name on which the initiator resides
        /// </summary>
        public string InitiatorHostName
        {
            get
            {
                return "Client01";
            }
        }

        /// <summary>
        /// Specifies the highest protocol version supported by the server
        /// </summary>
        public uint ServerServiceVersion
        {
            get
            {
                return ParseUint("ServerServiceVersion");
            }
        }

        /// <summary>
        /// A 32-bit unsigned integer which indicates the sector size, in bytes, of the shared virtual disk
        /// </summary>
        public uint SectorSize
        {
            get
            {
                return 512;
            }
        }

        /// <summary>
        /// A 32-bit unsigned integer which indicates the physical sector size, in bytes, of the shared virtual disk
        /// </summary>
        public uint PhysicalSectorSize
        {
            get
            {
                return 4096;
            }
        }

        /// <summary>
        /// A 64-bit unsigned integer which indicates the virtual size, in Gigabytes, of the shared virtual disk.
        /// 1 GB
        /// </summary>
        public ulong VirtualSize
        {
            get
            {
                return 1 * 0x40000000;
            }
        }

        /// <summary>
        /// Indicates the type of the virtual disk.
        /// </summary>
        public DISK_TYPE DiskType
        {
            get
            {
                return DISK_TYPE.VHD_TYPE_DYNAMIC;
            }
        }

        /// <summary>
        /// A Boolean value. 
        ///"false" indicates that the disk is not ready for read or write operations. 
        ///"true" indicates that the disk is mounted and ready for read or write operations.
        /// </summary>
        public bool IsMounted
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// A Boolean value. 
        ///"false" indicates disk sectors are not aligned to 4 kilobytes. 
        /// "true" indicates disk sectors are aligned to 4 kilobytes.
        /// </summary>
        public bool Is4kAligned
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// The size of shared virtual disk, in Megabytes.
        /// </summary>
        public ulong FileSizeInMB
        {
            get
            {
                return 4 * 0x100000;  // 4 MB
            }
        }

        public string FullPathShareContainingSharedVHD
        {
            get
            {
                return GetProperty("ShareContainingSharedVHD");
            }
        }


        public RSVDTestConfig(ITestSite site)
            : base(site)
        {
            ParseFullPath();
        }

        private void ParseFullPath()
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

        private ulong ParseUlong(string propertyName)
        {
            if (GetProperty(propertyName).StartsWith("0x"))
                return ulong.Parse(GetProperty(propertyName).Substring(2), NumberStyles.HexNumber);
            else
                return ulong.Parse(GetProperty(propertyName));
        }

        private uint ParseUint(string propertyName)
        {
            if (GetProperty(propertyName).StartsWith("0x"))
                return uint.Parse(GetProperty(propertyName).Substring(2), NumberStyles.HexNumber);
            else
                return uint.Parse(GetProperty(propertyName));
        }
    }
}