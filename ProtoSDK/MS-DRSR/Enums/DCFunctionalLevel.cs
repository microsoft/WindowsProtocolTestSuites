// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr
{

    /// <summary>
    /// Enumeration specifies the function level
    /// </summary>
    public enum DrsrDomainFunctionLevel : int
    {
        /// <summary>
        /// Windows Server 2000
        /// </summary>
        DS_BEHAVIOR_WIN2000 = 0,

        /// <summary>
        /// Windows Server 2003
        /// </summary>
        DS_BEHAVIOR_WIN2003 = 2,

        /// <summary>
        /// Windows Server 2008
        /// </summary>
        DS_BEHAVIOR_WIN2008 = 3,

        /// <summary>
        /// Windows Server 2008 R2
        /// </summary>
        DS_BEHAVIOR_WIN2008R2 = 4,

        /// <summary>
        /// Windows Server 2012
        /// </summary>
        DS_BEHAVIOR_WIN2012 = 5,

        /// <summary>
        ///  Windows Server 2012 R2
        /// </summary>
        DS_BEHAVIOR_WIN2012R2 = 6,

        /// <summary>
        /// Windows Server Threshold
        /// </summary>
        DS_BEHAVIOR_WINTHRESHOLD = 7,
    }
}
