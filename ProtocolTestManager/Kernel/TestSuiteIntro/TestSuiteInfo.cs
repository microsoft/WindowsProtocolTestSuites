// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// This class defines the structure of a test suite information.
    /// </summary>
    public class TestSuiteInfo
    {

        // Name of test suite
        public string TestSuiteName { get; set; }
        // Endpoint of test suite
        public string TestSuiteEndPoint { get; set; }
        // Description of test suite
        public string DetailDescription { get; set; }


        // Description of test suite
        public string ShortDescription { get; set; }

        // Version of test suite
        public string TestSuiteVersion { get; set; }

        // The format of the test suite folder. Environment variables are not supported.
        // C:\MicrosoftProtocolTests\$(TestSuiteName)\$(TestSuiteEndpoint)\$(TestSuiteVersion)
        public string TestSuiteFolderFormat { get; set; }

        // Folder of test suite
        public string TestSuiteFolder { get; set; }

        //The MSI to install the test suite.
        public string Installer { get; set; }

        // Existence of test suite
        public bool IsInstalled { get; set; }

        // The test suite is configured
        public bool IsConfiged { get; set; }

        // The profile of last run.
        public string LastProfile { get; set; }

        // Indicates a test suite is installed or not on this machine.
        public InstallStatus InstallStatus
        {
            get
            {
                if (IsInstalled) return InstallStatus.Installed;
                if(!string.IsNullOrEmpty(Installer)) return Kernel.InstallStatus.MSIAvailable;
                return Kernel.InstallStatus.None;
            }
        }
    }

    /// <summary>
    /// Test suite installation status.
    /// </summary>
    public enum InstallStatus { Installed, MSIAvailable, None }
}
