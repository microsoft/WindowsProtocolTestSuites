// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter
{
    /// <summary>
    /// This class is a base of the Model adapter.
    /// </summary>
    public class ModelManagedAdapterBase: ManagedAdapterBase
    {
        protected SMB2ModelTestConfig testConfig;
        protected ISutProtocolControlAdapter sutProtocolController;

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
            base.Reset();
        }
        #endregion
    }
}
