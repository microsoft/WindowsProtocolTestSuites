// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Collections.Generic;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.ServerFailover.Adapter
{
    public class ServerFailoverTestConfig : TestConfigBase
    {
        /// <summary>
        /// Get property value from grouped ptf config.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="checkNullOrEmpty">Check if the property is null or the value is empty.</param>
        /// <returns>The value of the property.</returns>
        public string GetProperty(string propertyName, bool checkNullOrEmpty = true)
        {
            return GetProperty("Cluster", propertyName, checkNullOrEmpty);
        }

        public TimeSpan FailoverTimeout
        {
            get
            {
                return TimeSpan.FromSeconds(int.Parse(GetProperty("FailoverTimeout")));
            }
        }

        #region Cluster Configuration
        public string ClusterNode01
        {
            get
            {
                return GetProperty("ClusterNode01");
            }
        }
        public string ClusterNode02
        {
            get
            {
                return GetProperty("ClusterNode02");
            }
        }
        public string ClusteredFileServerName
        {
            get
            {
                return GetProperty("ClusteredFileServerName");
            }
        }
        public string ClusteredScaleOutFileServerName
        {
            get
            {
                return GetProperty("ClusteredScaleOutFileServerName");
            }
        }

        public string ClusteredFileShare
        {
            get
            {
                return CAShareName;
            }
        }
        public string ClusteredEncryptedFileShare
        {
            get
            {
                return GetProperty("CAShareWithDataEncryption");
            }
        }
        public string AsymmetricShare
        {
            get
            {
                return GetProperty("AsymmetricShare");
            }
        }
        public string OptimumNodeOfAsymmetricShare
        {
            get
            {
                return GetProperty("OptimumNodeOfAsymmetricShare");
            }
        }
        public string NonOptimumNodeOfAsymmetricShare
        {
            get
            {
                return GetProperty("NonOptimumNodeOfAsymmetricShare");
            }
        }

        #endregion

        #region SWN Configuration
        public TimeSpan swnWitnessSyncTimeout
        {
            get
            {
                return TimeSpan.FromSeconds(int.Parse(GetProperty("SwnWitnessSyncTimeout")));
            }
        }

        public string WitnessClientName
        {
            get
            {
                return GetProperty("WitnessClientName");
            }
        }
        #endregion

        public ServerFailoverTestConfig(ITestSite site):base(site)
        {
        }
    }
}
