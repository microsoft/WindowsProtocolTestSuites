// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter
{
    public class FSATestConfig : TestConfigBase
    {
        private List<FsControlCommand> unSupportedFSCTL;
        public FSATestConfig(ITestSite site) : base(site)
        {
            this.unSupportedFSCTL = ParsePropertyToList<FsControlCommand>("FSCC_UnSupportedFSCTL", "FSA");
        }

        /// <summary>
        /// Get property value from grouped ptf config.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="checkNullOrEmpty">Check if the property is null or the value is empty.</param>
        /// <returns>The value of the property.</returns>
        public string GetProperty(string propertyName, bool checkNullOrEmpty = true)
        {
            return GetProperty("FSA", propertyName, checkNullOrEmpty);
        }

        public void CheckFSCTL(uint fsctl)
        {
            // If the fsctl is shown in ptfconfig "FSCC_UnSupportedFSCTL", which means the fsctl is not supported,
            // then fail the case inconclusive.
            var fsctlCommand = (FsControlCommand)fsctl;
            if (unSupportedFSCTL.Contains((FsControlCommand)fsctlCommand))
            {
                Site.Assert.Inconclusive($"{fsctlCommand} is not supported by SUT according to the ptfconfig \"FSCC_UnSupportedFSCTL\". " +
                    "Please modify the ptfconfig if the FSCTL is supported.");
            }
        }
    }
}
