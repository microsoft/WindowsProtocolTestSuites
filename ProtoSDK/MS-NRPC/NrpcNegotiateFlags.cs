// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc
{
    /// <summary>
    /// A 32-bit set of bit flags that identify the negotiated capabilities 
    /// between the client and the server.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum NrpcNegotiateFlags : uint
    {
        /// <summary>
        /// No flag is set.
        /// </summary>
        None = 0,

        /// <summary>
        /// C:<para/>
        /// Supports RC4 encryption.
        /// </summary>
        SupportsRC4 = 0x4,

        /// <summary>
        /// G:<para/>
        /// Does not require ValidationLevel 2 for nongeneric pass-through.
        /// </summary>
        DoesNotRequireValidationLevel2 = 0x40,

        /// <summary>
        /// I:<para/>
        /// Supports RefusePasswordChange.
        /// </summary>
        SupportsRefusePasswordChange = 0x100,

        /// <summary>
        /// J:<para/>
        /// Supports the NetrLogonSendToSam functionality.
        /// </summary>
        SupportsNetrLogonSendToSam = 0x200,

        /// <summary>
        /// K:<para/>
        /// Supports generic pass-through authentication.
        /// </summary>
        SupportsGenericPassThroughAuthentication = 0x400,

        /// <summary>
        /// L:<para/>
        /// Supports concurrent RPC calls.
        /// </summary>
        SupportsConcurrentRpcCalls = 0x800,

        /// <summary>
        /// O:<para/>
        /// Supports strong keys.
        /// </summary>
        SupportsStrongKeys = 0x4000,

        /// <summary>
        /// P:<para/>
        /// Supports transitive trusts.
        /// </summary>
        SupportsTransitiveTrusts = 0x8000,

        /// <summary>
        /// R:<para/>
        /// Supports the NetrServerPasswordSet2 functionality.
        /// </summary>
        SupportsNetrServerPasswordSet2 = 0x20000,

        /// <summary>
        /// S:<para/>
        /// Supports the NetrLogonGetDomainInfo functionality.
        /// </summary>
        SupportsNetrLogonGetDomainInfo = 0x40000,

        /// <summary>
        /// T:<para/>
        /// Supports cross-forest trusts.
        /// </summary>
        SupportsCrossForestTrusts = 0x80000,

        /// <summary>
        /// U:<para/>
        /// Supports neutralizing Microsoft Windows NT® 4.0 operating system emulation. 
        /// Note that when this flag is negotiated between a client and a server, 
        /// it indicates that the server SHOULD ignore the NT4Emulator ADM element.
        /// </summary>
        SupportsWinNT4Emulation = 0x100000,

        /// <summary>
        /// V:<para/>
        /// Supports RODC pass-through to different domains.
        /// </summary>
        SupportsRodcPassThroughToDifferentDomains = 0x200000,

        /// <summary>
        /// W:<para/>
        /// Supports AES encryption and SHA2 hashing.
        /// </summary>
        SupportsAESAndSHA2 = 0x1000000,

        /// <summary>
        /// Y:<para/>
        /// Supports Secure RPC.
        /// </summary>
        SupportsSecureRpc = 0x40000000
    }
}
