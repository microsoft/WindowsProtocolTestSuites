// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2.Common
{
    /// <summary>
    /// SMB2 compression info.
    /// </summary>
    public class Smb2CompressionInfo
    {
        public Smb2CompressionInfo()
        {
            CompressAllPackets = false;
            CompressionIds = new CompressionAlgorithm[0];
            PreferredCompressionAlgorithm = CompressionAlgorithm.NONE;
        }

        /// <summary>
        /// Indicating whether to compress all requests at sending.
        /// </summary>
        public bool CompressAllPackets { get; set; }

        /// <summary>
        /// Compression algorithms supported by both endpoints.
        /// </summary>
        public CompressionAlgorithm[] CompressionIds { get; set; }

        /// <summary>
        /// Indicating the preferred compression algorithm to be used.
        /// </summary>
        public CompressionAlgorithm PreferredCompressionAlgorithm { get; set; }
    }
}
