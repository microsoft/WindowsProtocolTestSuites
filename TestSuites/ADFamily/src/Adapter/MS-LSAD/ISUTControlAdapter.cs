// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.Protocols.TestTools;

    /// <summary>
    /// Define all actions of the interface ISUTControlAdapter.
    /// </summary>
    public interface ILsadSutControlAdapter : IAdapter
    {
        /// <summary>
        /// start the directory service
        /// </summary>
        /// <returns> the call must return true which indicates success</returns>
        [MethodHelp("start directory service on the machine specified by remoteServerName parameter")]
        bool StartDirectoryService();

        /// <summary>
        /// stop the directory service
        /// </summary>
        /// <returns> the call must return true which indicates success</returns>
        [MethodHelp("stop directory service on the machine specified by remoteServerName parameter")]
        bool StopDirectoryService();

        /// <summary>
        /// get the directory service
        /// </summary>
        /// <returns> the call must return true if the service is stopped</returns>
        [MethodHelp("get directory service on the machine specified by remoteServerName parameter")]
        bool IsDirectoryServiceStopped();
    }
}
