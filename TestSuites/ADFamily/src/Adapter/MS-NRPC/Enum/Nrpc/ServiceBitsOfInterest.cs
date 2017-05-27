// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    /// <summary>
    /// Enum for ServiceBitsOfInterest
    /// These items are defined in section 3.5.5.7.5 in the TD of MS-NRPC.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags", 
        Justification = "MarkEnumsWithFlags"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", 
            "CA1028:EnumStorageShouldBeInt32", Justification = "EnumStorageShouldBeInt32"), 
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", 
        Justification = "EnumsShouldHaveZeroValue")]
    public enum ServiceBitsOfInterest : uint
    {
        /// <summary>
        /// The state of the time service is being set.
        /// </summary>
        A = 0x00000040,

        /// <summary>
        /// The state of the time service with clock hardware is being set.
        /// </summary>
        B = 0x00000200,

        /// <summary>
        /// The state of the Active Directory Web service is being set.
        /// </summary>
        C = 0x00002000,
    }
}
