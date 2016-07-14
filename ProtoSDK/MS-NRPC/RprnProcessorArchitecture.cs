// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.PrintService.Rprn
{
    /// <summary>
    /// The value of this member is the implementation-specific identifier 
    /// for the client system's processor architecture.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum RprnProcessorArchitecture : uint
    {
        /// <summary>
        /// X86 architecture
        /// </summary>
        PROCESSOR_ARCHITECTURE_INTEL = 0x0000,

        /// <summary>
        /// Itanium architecture
        /// </summary>
        PROCESSOR_ARCHITECTURE_IA64 = 0x0006,

        /// <summary>
        /// Amd64 architecture
        /// </summary>
        PROCESSOR_ARCHITECTURE_AMD64 = 0x0009,
    }
}
