// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp
{
    /// <summary>
    /// The Operation System (OS) version of the System Under Testing (SUT).
    /// </summary>
    public enum OSVersion
    {
        /// <summary>
        /// The OS version of the SUT is non Windows platform.
        /// </summary>
        NonWindows,

        /// <summary>
        /// The OS version of the SUT is Windows Vista platform.
        /// </summary>
        WinVista,

        /// <summary>
        /// The OS version of the SUT is Windows 7 platform.
        /// </summary>
        Win7,

        /// <summary>
        /// The OS version of the SUT is Windows 2008 platform.
        /// </summary>
        Win2K8,

        /// <summary>
        /// The OS version of the SUT is Windows 2008 R2 platform.
        /// </summary>
        Win2K8R2
    }
}
