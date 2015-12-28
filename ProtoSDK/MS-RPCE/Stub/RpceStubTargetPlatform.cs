// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// Target platform to build a RPC stub. 
    /// If target platform doesn't match actually running system, 
    /// program should throw an exception.
    /// </summary>
    [Flags]
    public enum RpceStubTargetPlatform
    {
        /// <summary>
        /// Target platform is x86.
        /// </summary>
        X86 = 1,

        /// <summary>
        /// Target platform is amd64.
        /// </summary>
        Amd64 = 2
    }
}
