// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pchc
{
    /// <summary>
    /// The platform of the SUT(server role).
    /// </summary>
    public enum ServerRoleOSVersion
    {
        /// <summary>
        /// The platform of the SUT(server role) is non windows platform.
        /// </summary>
        NonWindows,

        /// <summary>
        /// The platform of the SUT(server role) is Windows Vista platform.
        /// </summary>
        WinVista,

        /// <summary>
        /// The platform of the SUT(server role) is Windows 7 platform.
        /// </summary>
        Win7,

        /// <summary>
        /// The platform of the SUT(server role) is Windows 2008 platform.
        /// </summary>
        Win2K8,

        /// <summary>
        /// The platform of the SUT(server role) is Windows 2008 R2 platform.
        /// </summary>
        Win2K8R2
    }
}
