// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestManager.PTMService.Common.Types
{
    /// <summary>
    /// Test suite install method.
    /// </summary>
    public enum TestSuiteInstallMethod
    {
        /// <summary>
        /// Install by uploading package.
        /// </summary>
        UploadPackage,

        /// <summary>
        /// Install from repo.
        /// </summary>
        InstallFromRepo,
    }
}
