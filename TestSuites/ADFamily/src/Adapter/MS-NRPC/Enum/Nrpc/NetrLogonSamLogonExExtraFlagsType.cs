// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    using System;

    /// <summary>
    /// Enums for NetrLogonSamLogonEx ExtraFlags:
    /// These items are defined in section 3.5.5.4.2 in the TD of MS-NRPC.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32",
        Justification = "EnumStorageShouldBeInt32"), Flags()]
    public enum NetrLogonSamLogonExExtraFlagsType : uint
    {
        /// <summary>
        /// Request MUST be passed to the domain controller at the root of the forest.
        /// </summary>
        A = 0x00000001,

        /// <summary>
        /// Request MUST be passed to the DC at the end of the first hop over a cross-forest trust.
        /// </summary>
        B = 0x00000002,

        /// <summary>
        /// Request was passed by an RODC to a DC in a different domain.
        /// </summary>
        C = 0x00000004,

        /// <summary>
        /// Request is an NTLM authentication package request passed by an RODC.
        /// </summary>
        D = 0x00000008,
    }
}
