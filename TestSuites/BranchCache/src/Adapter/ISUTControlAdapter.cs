// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.BranchCache.Adapter
{
    public interface ISUTControlAdapter : IAdapter
    {
        /// <summary>
        /// Generates hash for HTTP on specified computer.
        /// </summary>
        /// <param name="computerName">The computer on which the hash should be generated.</param>
        /// <param name="path">The physical directory path which contains the test files.</param>
        [MethodHelp("On the specified computer, for files under the specified path, please generate hash for PCCRTP retrieval.")]
        void GenerateHTTPHash(string computerName, string path);

        /// <summary>
        /// Generates hash for SMB2 on specified computer.
        /// </summary>
        /// <param name="computerName">The computer on which the hash should be generated.</param>
        /// <param name="path">The physical directory path which contains the test files.</param>
        [MethodHelp("On the specified computer, for files under the specified path, please generate hash for SMB2 retrieval.")]
        void GenerateSMB2Hash(string computerName, string path);

        /// <summary>
        /// Clears hash for HTTP on specified computer.
        /// </summary>
        /// <param name="computerName">The computer on which the hash should be generated.</param>
        /// <param name="path">The physical directory path which contains the test files.</param>
        [MethodHelp("On the specified computer, under the specified path, please clean up the hash generated for PCCRTP retrieval.")]
        void ClearHTTPHash(string computerName, string path);

        /// <summary>
        /// Clears hash for SMB2 on specified computer.
        /// </summary>
        /// <param name="computerName">The computer on which the hash should be generated.</param>
        /// <param name="path">The physical directory path which contains the test files.</param>
        [MethodHelp("On the specified computer, under the specified path, please clean up the hash generated for SMB2 retrieval.")]
        void ClearSMB2Hash(string computerName, string path);

        /// <summary>
        /// Determines whether the hash is generated for HTTP on specified computer.
        /// </summary>
        /// <param name="computerName">The computer to check.</param>
        /// <returns>True if the publication cache exists, false otherwise.</returns>
        [MethodHelp("On the specified computer, please check whether the hash is generated for PCCRTP retrieval exists. Return true if the local cache exists, false otherwise.")]
        bool IsHTTPHashExisted(string computerName);

        /// <summary>
        /// Clears all cached data and hashes on specified computer.
        /// </summary>
        /// <param name="computerName">The computer on which the cache should be cleared.</param>
        [MethodHelp("On the specified computer, please clean up all cached data and hashes.")]
        void ClearCache(string computerName);

        /// <summary>
        /// Determines whether the local cache is generated on specified computer.
        /// </summary>
        /// <param name="computerName">The computer to check.</param>
        /// <returns>True if the local cache exists, false otherwise.</returns>
        [MethodHelp("On the specified computer, please check whether the local cache exists. Return true if the local cache exists, false otherwise.")]
        bool IsLocalCacheExisted(string computerName);

        /// <summary>
        /// Instructs the specified computer to restart the branch cache service.
        /// </summary>
        /// <param name="computerName">The target computer on which the service will be restarted.</param>
        [MethodHelp("On the specified computer, please restart the branch cache service.")]
        void RestartBranchCacheService(string computerName);
    }
}
