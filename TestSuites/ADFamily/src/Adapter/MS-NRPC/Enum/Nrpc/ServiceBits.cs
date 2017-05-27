// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    /// <summary>
    /// Enum for ServiceBits
    /// These items are defined in section 3.5.1 in the TD of MS-NRPC.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags", 
        Justification = "MarkEnumsWithFlags"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", 
            "CA1028:EnumStorageShouldBeInt32", Justification = "EnumStorageShouldBeInt32"), 
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", 
        Justification = "EnumsShouldHaveZeroValue")]
    public enum ServiceBits : uint
    {
        /// <summary>
        /// Time service is running.
        /// </summary>
        A = 0x00000040,

        /// <summary>
        /// Time service with clock hardware is running.
        /// </summary>
        B = 0x00000200,

        /// <summary>
        /// Active Directory Web service is running.
        /// </summary>
        C = 0x00002000,
    }
}
