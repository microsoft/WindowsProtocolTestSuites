// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
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

        #endregion

        public TestConfig(ITestSite testSite)
        {
            this.Initialize(testSite);
        }

        public void Initialize(ITestSite testSite)
        {
            this.site = testSite;
            this.Platform = (Platform)Enum.Parse(typeof(Platform), site.Properties["Platform"]);
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
        WindowsServer2016 = 0x10000008
    }
}
