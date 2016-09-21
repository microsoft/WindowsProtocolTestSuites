// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Security;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.CommonStack.Enum;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.BranchCache.TestSuite
{
    public class TestConfig
    {
        public ContentInformationTransport ContentTransport;

        public bool SupportBranchCacheV1;
        public bool SupportBranchCacheV2;

        public string ContentServerComputerName;
        public string HostedCacheServerComputerName;
        public string ClientPeerComputerName;

        public string DomainName;
        public string UserName;
        public string UserPassword;

        public ServerHashLevel HashLevelType;

        public SecurityPackageType SecurityPackageType;

        public byte[] ServerSecret;

        public string WebsiteLocalPath;
        public string FileShareLocalPath;

        public bool SupportWebsiteForcedHashGeneration;
        public bool SupportFileShareForcedHashGeneration;

        public TimeSpan Timeout;
        public TimeSpan RetryInterval;
        public TimeSpan NegativeTestTimeout;

        public string SharedFolderName;

        public string NameOfFileWithMultipleSegments;
        public string NameOfFileWithMultipleBlocks;
        public string NameOfFileWithSingleBlock;

        public int ContentServerHTTPListenPort;
        public int HostedCacheServerHTTPListenPort;
        public int HostedCacheServerHTTPSListenPort;
        public int ClientContentRetrievalListenPort;

        public string ContentServerComputerFQDNOrNetBiosName
        {
            get
            {
                return GetFQDNOrNetBiosName(ContentServerComputerName);
            }
        }

        public string HostedCacheServerComputerFQDNOrNetBiosName
        {
            get
            {
                return GetFQDNOrNetBiosName(HostedCacheServerComputerName);
            }
        }

        public string ClientPeerComputerFQDNOrNetBiosName
        {
            get
            {
                return GetFQDNOrNetBiosName(ClientPeerComputerName);
            }
        }

        public TestConfig(ITestSite site)
        {
            ContentTransport = (ContentInformationTransport)Enum.Parse(typeof(ContentInformationTransport), site.Properties["ContentTransport"]);

            SupportBranchCacheV1 = bool.Parse(site.Properties["SupportBranchCacheV1"]);
            SupportBranchCacheV2 = bool.Parse(site.Properties["SupportBranchCacheV2"]);

            ContentServerComputerName = site.Properties["ContentServerComputerName"];
            HostedCacheServerComputerName = site.Properties["HostedCacheServerComputerName"];
            ClientPeerComputerName = site.Properties["ClientPeerComputerName"];

            DomainName = site.Properties["DomainName"];
            UserName = site.Properties["UserName"];
            UserPassword = site.Properties["UserPassword"];

            HashLevelType = (ServerHashLevel)Enum.Parse(typeof(ServerHashLevel), site.Properties["SupportedHashLevel"]);

            SecurityPackageType = (SecurityPackageType)Enum.Parse(typeof(SecurityPackageType), site.Properties["SecurityPackageType"]);

            ServerSecret = Encoding.Unicode.GetBytes(site.Properties["ServerSecret"]);

            Timeout = TimeSpan.FromSeconds(int.Parse(site.Properties["Timeout"]));
            RetryInterval = TimeSpan.FromSeconds(int.Parse(site.Properties["RetryInterval"]));
            NegativeTestTimeout = TimeSpan.FromSeconds(int.Parse(site.Properties["NegativeTestTimeout"]));

            SharedFolderName = site.Properties["SharedFolderName"];

            NameOfFileWithMultipleSegments = site.Properties["NameOfFileWithMultipleSegments"];
            NameOfFileWithMultipleBlocks = site.Properties["NameOfFileWithMultipleBlocks"];
            NameOfFileWithSingleBlock = site.Properties["NameOfFileWithSingleBlock"];

            WebsiteLocalPath = site.Properties["WebsiteLocalPath"];
            FileShareLocalPath = site.Properties["FileShareLocalPath"];

            SupportWebsiteForcedHashGeneration = bool.Parse(site.Properties["SupportWebsiteForcedHashGeneration"]);
            SupportFileShareForcedHashGeneration = bool.Parse(site.Properties["SupportFileShareForcedHashGeneration"]);

            ContentServerHTTPListenPort = int.Parse(site.Properties["ContentServerHTTPListenPort"]);
            HostedCacheServerHTTPListenPort = int.Parse(site.Properties["HostedCacheServerHTTPListenPort"]);
            HostedCacheServerHTTPSListenPort = int.Parse(site.Properties["HostedCacheServerHTTPSListenPort"]);
            ClientContentRetrievalListenPort = int.Parse(site.Properties["ClientContentRetrievalListenPort"]);
        }

        private static string GetFQDNOrNetBiosName(string computerName)
        {
            // Return directly if the computer name is FQDN
            if (computerName.Contains("."))
                return computerName;

            // Return directly if the computer name is valid netbios name
            if (computerName.Length <= 15)
                return computerName;

            // Otherwise trim to 15 chars to get a valid netbios name
            return computerName.Substring(0, 15);
        }
    }
}
