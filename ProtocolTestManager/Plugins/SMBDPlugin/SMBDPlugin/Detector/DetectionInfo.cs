// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
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

    public enum RDMAEndian
    {
        LittleEndian,
        BigEndian
    }

    public class DetectionInfo
    {
        public DetectionInfo()
        {
            OSDetected = false;
            NonRDMATransportSupported = false;
            RDMATransportSupported = false;
            RDMAChannelV1Supported = false;
            RDMAChannelV1InvalidateSupported = false;
        }

        /// <summary>
        /// Platform of SUT.
        /// </summary>
        public Platform Platform { get; set; }

        /// <summary>
        /// Authentication type for SUT credential.
        /// </summary>
        public SecurityPackageType Authentication { get; set; }

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

        /// <summary>
        /// The IP address of RDMA network interface on driver computer.
        /// </summary>
        public string DriverRdmaNICIPAddress { get; set; }

        /// <summary>
        /// The IP address of non-RDMA network interface on driver computer.
        /// </summary>
        public string DriverNonRdmaNICIPAddress { get; set; }

        /// <summary>
        /// The IP address of RDMA network interface on SUT.
        /// </summary>
        public string SUTRdmaNICIPAddress { get; set; }

        /// <summary>
        /// The IP address of non-RDMA network interface on SUT.
        /// </summary>
        public string SUTNonRdmaNICIPAddress { get; set; }

        /// <summary>
        /// Share folder on SUT.
        /// </summary>
        public string ShareFolder { get; set; }

        /// <summary>
        /// SMB dialects supported by SUT.
        /// </summary>
        public DialectRevision[] SupportedSmbDialects { get; set; }

        /// <summary>
        /// SMBD port.
        /// </summary>
        public ushort SMBDPort { get; set; }

        /// <summary>
        /// SMBD capability of client.
        /// </summary>
        public SMBDClientCapability SMBDClientCapability { get; set; }

        /// <summary>
        /// Connection timeout.
        /// </summary>
        public TimeSpan ConnectionTimeout { get; set; }

        /// <summary>
        /// Endian of RDMA transport.
        /// </summary>
        public RDMAEndian Endian { get; set; }

        #region detection result related properties
        public bool OSDetected { get; set; }
        public bool NonRDMATransportSupported { get; set; }
        public bool RDMATransportSupported { get; set; }
        public bool RDMAChannelV1Supported { get; set; }
        public bool RDMAChannelV1InvalidateSupported { get; set; }
        #endregion
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

        /// <summary>
        /// Description of network interface.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Indicating whether this network interface is RDMA-capable.
        /// </summary>
        public bool RDMACapable { get; set; }
    }

    public class RemoteNetworkInterfaceInformation
    {
        /// <summary>
        /// Index of network interface.
        /// </summary>
        public uint IfIndex { get; set; }

        /// <summary>
        /// IP Address of network interface.
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// The speed of network interface.
        /// </summary>
        public string LinkSpeed { get; set; }

        /// <summary>
        /// Indicating whether this network interface is RDMA-capable.
        /// </summary>
        public bool RDMACapable { get; set; }
    }

    public class OSVersion
    {
        /// <summary>
        /// OS caption.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// OS version.
        /// </summary>
        public string Version { get; set; }
    }

    public class SMBDClientCapability
    {
        public ushort CreditsRequested { get; set; }
        public ushort ReceiveCreditMax { get; set; }
        public uint PreferredSendSize { get; set; }
        public uint MaxReceiveSize { get; set; }
        public uint MaxFragmentedSize { get; set; }
    }
}
