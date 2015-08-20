// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocol.TestSuites.Smb
{
    /// <summary>
    /// the interface for the interactive adapter
    /// </summary>
    public interface ISutInteractiveAdapter : IConfigSignStateInteractiveAdapter, IDeleteFilesInteractiveAdapter, IAdapter
    {
        /// <summary>
        /// Delete specific files on the SUT.
        /// </summary>
        new void DeleteFiles();

        /// <summary>
        /// Config the SUT signing policy.
        /// </summary>
        /// <param name="serverName">The SUT name.</param>
        /// <param name="signState">A state that determines whether this node signs messages.</param>
        new void ConfigSignState(string serverName, string signState);
    }
}
