// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocol.TestSuites.Smbd.Adapter
{
    public interface ISutProtocolControlAdapter : IAdapter
    {
        /// <summary>
        /// Delete specific file on test share
        /// </summary>
        /// <param name="fileName">Name of file to be deleted</param>
        void DeleteFile(string fileName);
    }
}
