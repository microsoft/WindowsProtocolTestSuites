// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter
{
    /// <summary>
    /// This class is a base of the Model adapter.
    /// </summary>
    public class ModelManagedAdapterBase : ManagedAdapterBase
    {
        protected SMB2ModelTestConfig testConfig;
        protected ISutProtocolControlAdapter sutProtocolController;
        protected List<string> testFiles = new List<string>();
        protected List<string> testDirectories = new List<string>();

        public static HashSet<string> AllTestFiles { get; } = new HashSet<string>();
        public static HashSet<string> AllTestDirectories { get; } = new HashSet<string>();

        #region Initialization

        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);

            Site.DefaultProtocolDocShortName = "MS-SMB2";

            testConfig = new SMB2ModelTestConfig(Site);

            sutProtocolController = Site.GetAdapter<ISutProtocolControlAdapter>();
        }

        public override void Reset()
        {
            foreach (var directory in testDirectories)
            {
                AllTestDirectories.Add(directory);
            }

            testDirectories.Clear();

            foreach (var fileName in testFiles)
            {
                AllTestFiles.Add(fileName);
            }

            testFiles.Clear();

            base.Reset();
        }
        #endregion

        protected string CurrentTestCaseName
        {
            get
            {
                string fullName = (string)Site.TestProperties["CurrentTestCaseName"];
                return fullName.Split('.').LastOrDefault();
            }
        }

        protected string GetTestFileName(string share)
        {
            if (string.IsNullOrEmpty(share))
            {
                return null;
            }

            string fileName = CurrentTestCaseName + "_" + Guid.NewGuid().ToString();
            testFiles.Add(Path.Combine(share, fileName));
            return fileName;
        }

        protected void AddTestFileName(string share, string fileName)
        {
            if (string.IsNullOrEmpty(share))
            {
                return;
            }

            testFiles.Add(Path.Combine(share, fileName));
        }

        protected string GetTestDirectoryName(string share)
        {
            if (string.IsNullOrEmpty(share))
            {
                return null;
            }

            string directoryName = CurrentTestCaseName + "_" + Guid.NewGuid().ToString();
            testDirectories.Add(Path.Combine(share, directoryName));
            return directoryName;
        }

        protected void AddTestDirectoryName(string share, string directoryName)
        {
            if (string.IsNullOrEmpty(share))
            {
                return;
            }

            testDirectories.Add(Path.Combine(share, directoryName));
        }
    }
}
