// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    /// <summary>
    /// The flags that indicates how to set the Md5Digest field of IDL_DRSGetObjectExistence.
    /// </summary>
    public enum  Md5Digest_Flags
    {
        /// <summary>
        /// Set the Md5Digest to be consistent with that computed by server.
        /// </summary>
        ConsistentDigest,

        /// <summary>
        /// Set the Md5Digest to be inconsistent with that computed by server.
        /// </summary>
        InconsistentDigest
    }
}
