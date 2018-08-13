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
    public class ModelManagedAdapterBase: ManagedAdapterBase
    {
        protected SMB2ModelTestConfig testConfig;
        protected ISutProtocolControlAdapter sutProtocolController;
        protected List<string> testFiles = new List<string>();
        protected List<string> testDirectories = new List<string>();

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
                try
                {
                    sutProtocolController.DeleteDirectory(Smb2Utility.GetShareName(directory), Smb2Utility.GetFileName(directory));
                }
                catch
                {
                }
            }

            testDirectories.Clear();

            foreach (var fileName in testFiles)
            {
                try
                {
                    sutProtocolController.DeleteFile(Smb2Utility.GetShareName(fileName), Smb2Utility.GetFileName(fileName));
                }
                catch
                {
                }
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
            string fileName = CurrentTestCaseName + "_" + Guid.NewGuid().ToString();
            testFiles.Add(Path.Combine(share, fileName));
            return fileName;
        }

        protected void AddTestFileName(string share, string fileName)
        {
            testFiles.Add(Path.Combine(share, fileName));
        }

        protected string GetTestDirectoryName(string share)
        {
            string directoryName = CurrentTestCaseName + "_" + Guid.NewGuid().ToString();
            testDirectories.Add(Path.Combine(share, directoryName));
            return directoryName;
        }

        protected void AddTestDirectoryName(string share, string directoryName)
        {
            testDirectories.Add(Path.Combine(share, directoryName));
        }
    }
}
