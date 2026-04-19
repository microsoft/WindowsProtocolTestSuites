// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib;
using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestSuites.Smbd.Adapter
{
    public class TestConfig
    {
        private const string LITTLE_ENDIAN = "littleendian";
        private const string BIG_ENDIAN = "bigendian";
        private ITestSite site;

        #region Properties
        public string TestFileName_LargeFile { get; set; }
        public uint SmallFileSizeInByte { get; set; }
        public uint ModerateFileSizeInByte { get; set; }
        public uint LargeFileSizeInByte { get; set; }
        public TimeSpan Smb2ConnectionTimeout { get; set; }
        public TimeSpan DisconnectionTimeout { get; set; }
        public SecurityPackageType SecurityPackageForSmb2UserAuthentication { get; set; }
        public string DomainName { get; set; }
        public string ServerName { get; set; }
        public string ShareFolder { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Platform Platform { get; private set; }
        public Platform DriverPlatform { get; private set; }
        public string ClientRNicIp { get; set; }
        public string ServerRNicIp { get; set; }
        public string ClientNonRNicIp { get; set; }
        public string ServerNonRNicIp { get; set; }
        public int SmbdTcpPort { get; set; }
        public uint InboundEntries { get; set; }
        public uint OutboundEntries { get; set; }
        public uint InboundReadLimit { get; set; }
        public bool ReversedBufferDescriptor { get; set; }
        public uint ReceiveCreditMax { get; set; }
        public uint SendCreditTarget { get; set; }
        public uint MaxSendSize { get; set; }
        public uint MaxFragmentedSize { get; set; }
        public uint MaxReceiveSize { get; set; }
        public int KeepAliveInterval { get; set; }
        public bool CheckDataLengthRemainingDataLength { get; set; }
        public bool RdmaLayerLoggingEnabled { get; set; }
        public List<string> ActiveTDI { get; private set; }
        public RDMATransport RDMATransport { get; private set; }
        #endregion

        public TestConfig(ITestSite testSite)
        {
            this.Initialize(testSite);
        }

        public void Initialize(ITestSite testSite)
        {
            this.site = testSite;
            this.Platform = (Platform)Enum.Parse(typeof(Platform), site.Properties["Platform"]);
            this.DriverPlatform = (Platform)Enum.Parse(typeof(Platform), site.Properties["DriverPlatform"]);
            this.ServerName = site.Properties["SutComputerName"];
            this.ClientRNicIp = site.Properties["ClientRNicIp"];
            this.ServerRNicIp = site.Properties["ServerRNicIp"];
            this.ClientNonRNicIp = site.Properties["ClientNonRNicIp"];
            this.ServerNonRNicIp = site.Properties["ServerNonRNicIp"];
            this.DomainName = site.Properties["DomainName"];
            this.UserName = site.Properties["SutUserName"];
            this.Password = site.Properties["SutPassword"];
            this.SmbdTcpPort = int.Parse(site.Properties["SmbdTcpPort"]);
            this.ShareFolder = site.Properties["ShareFolder"];

            this.TestFileName_LargeFile = site.Properties["TestFile_ReadLargeFile"];

            #region RDMA capablities
            InboundEntries = uint.Parse(site.Properties["InboundEntries"]);
            OutboundEntries = uint.Parse(site.Properties["OutboundEntries"]);
            InboundReadLimit = uint.Parse(site.Properties["InboundReadLimit"]);
            string endianness = site.Properties["EndianOfBufferDescriptor"];
            if (endianness.ToLower().Equals(LITTLE_ENDIAN))
            {
                ReversedBufferDescriptor = false;
            }
            else if (endianness.ToLower().Equals(BIG_ENDIAN))
            {
                ReversedBufferDescriptor = true;
            }
            else
            {
                throw new NotSupportedException(string.Format("PTF Configuration 'Endianness' with an unsupported value '{0}'.", endianness));
            }
            this.RDMATransport = (RDMATransport)Enum.Parse(typeof(RDMATransport), site.Properties["RDMATransport"]);
            #endregion

            #region SMBD
            ReceiveCreditMax = uint.Parse(site.Properties["ReceiveCreditMax"]);
            SendCreditTarget = uint.Parse(site.Properties["SendCreditTarget"]);
            MaxSendSize = uint.Parse(site.Properties["MaxSendSize"]);
            MaxFragmentedSize = uint.Parse(site.Properties["MaxFragmentedSize"]);
            MaxReceiveSize = uint.Parse(site.Properties["MaxReceiveSize"]);
            KeepAliveInterval = int.Parse(site.Properties["KeepAliveInterval"]);
            #endregion

            Smb2ConnectionTimeout = TimeSpan.FromSeconds(int.Parse(site.Properties["Smb2ConnectionTimeoutInSeconds"]));
            DisconnectionTimeout = TimeSpan.FromSeconds(double.Parse(site.Properties["DisconnectionTimeoutInSeconds"]));
            SecurityPackageForSmb2UserAuthentication = (SecurityPackageType)Enum.Parse(
                typeof(SecurityPackageType),
                site.Properties["SecurityPackageForSmb2UserAuthentication"]);

            SmallFileSizeInByte = uint.Parse(site.Properties["SmallFileSizeInByte"]);
            ModerateFileSizeInByte = uint.Parse(site.Properties["ModerateFileSizeInByte"]);
            LargeFileSizeInByte = uint.Parse(site.Properties["LargeFileSizeInKB"]) << 10;

            // switch 
            CheckDataLengthRemainingDataLength = bool.Parse(site.Properties["CheckDataLengthRemainingDataLength"]);

            RdmaLayerLoggingEnabled = bool.Parse(site.Properties["RdmaLayerLoggingEnabled"]);
            ActiveTDI = new List<string>(site.Properties["ActiveTDI"].Split(';'));
        }
    }

    public enum Platform
    {
        /// <summary>
        /// Non-Windows implementation
        /// </summary>
        NonWindows = 0x00000000,

        /// <summary>
        /// Windows Server 2012 operating system
        /// </summary>
        WindowsServer2012 = 0x10000006,

        /// <summary>
        /// Windows Server 2012 R2 operating system
        /// </summary>
        WindowsServer2012R2 = 0x10000007,

        /// <summary>
        /// Windows Server 2016 operating system
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

        /// <summary>
        /// Windows Server v1909
        /// </summary>
        WindowsServerV1909 = 0x1000000D,

        /// <summary>
        /// Windows Server v2004
        /// </summary>
        WindowsServerV2004 = 0x1000000E,

        /// <summary>
        /// Windows Server v2020
        /// </summary>
        WindowsServerV20H2 = 0x1000000F,

        /// <summary>
        /// Windows 10 v21H1
        /// </summary>
        Windows10V21H1 = 0x10000010,

        /// <summary>
        /// Windows Server v2022
        /// </summary>
        WindowsServer2022 = 0x10000011,

        /// <summary>
        /// Windows Server 2022 v22H2
        /// </summary>
        WindowsServerV22H2 = 0x10000012,

        /// <summary>
        /// Windows Server v2025
        /// </summary>
        WindowsServer2025 = 0x10000013,

        /// <summary>
        /// Windows 11
        /// </summary>
        Windows11 = 0x10000014,

        /// <summary>
        /// Windows 11 2021 v21H2
        /// </summary>
        Windows11V21H2 = 0x10000015,

        /// <summary>
        /// Windows 11 2022 v22H2
        /// </summary>
        Windows11V22H2 = 0x10000016,

        /// <summary>
        /// Windows 11 2023 v23H2
        /// </summary>
        Windows11V23H2 = 0x10000017,

        /// <summary>
        /// Windows 11 2024 v24H2
        /// </summary>
        Windows11V24H2 = 0x10000018
    }

    public enum RDMATransport
    {
        InfiniBand,
        RoCE,
        iWARP
    }
}
