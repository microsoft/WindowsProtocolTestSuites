// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestManager.SMBDPlugin.Detector
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
    }

    public class DetectionInfo
    {
        /// <summary>
        /// SUT host name.
        /// </summary>
        public string SUTName { get; set; }

        /// <summary>
        /// Domain name.
        /// </summary>
        public string DomainName { get; set; }

        /// <summary>
        /// User Name on SUT.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Password on SUT.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Is Windows implementation.
        /// </summary>
        public bool IsWindowsImplementation { get; set; }
    }

    public class LocalNetworkInterfaceInformation
    {
        /// <summary>
        /// Name of network interface.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// IP Address of network interface.
        /// </summary>
        public string IpAddress { get; set; }
    }
}
