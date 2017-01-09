// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    /// <summary>
    /// A 32-bit set of bit flags that identify the negotiated capabilities between the client and the server.
    /// These items are defined in section 3.1.4.2 in the TD of MS-NRPC.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags", 
        Justification = "MarkEnumsWithFlags"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", 
            "CA1028:EnumStorageShouldBeInt32", Justification = "EnumStorageShouldBeInt32"), 
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue",
        Justification = "EnumsShouldHaveZeroValue")]
    public enum NegotiateFlagsType : uint
    {
        /// <summary>
        /// Not used. MUST be ignored on receipt.
        /// </summary>
        A = 0x00000001,

        /// <summary>
        /// Microsoft Windows NT 3.5 operating systemBDCs persistently try to update their database to the PDC's 
        /// version once they get a notification, which indicates that their database is out-of-date. 
        /// The presence of this flag indicates that this behavior is supported.
        /// </summary>
        B = 0x00000002,

        /// <summary>
        /// Support RC4 encryption.
        /// </summary>
        C = 0x00000004,

        /// <summary>
        /// Not used. It MUST be ignored on receipt.
        /// </summary>
        D = 0x00000008,

        /// <summary>
        /// Support BDCs handling CHANGELOGs.
        /// </summary>
        E = 0x00000010,

        /// <summary>
        /// Support restarting of full synchronization between DCs.
        /// </summary>
        F = 0x00000020,

        /// <summary>
        /// Not require ValidationLevel 2 for no generic passthrough.
        /// </summary>
        G = 0x00000040,

        /// <summary>
        /// Support the NetrDatabaseRedo (Opnum 17) functionality.
        /// </summary>
        H = 0x00000080,

        /// <summary>
        /// Support the refusal of password changes.
        /// </summary>
        I = 0x00000100,

        /// <summary>
        /// Support the NetrLogonSendToSam functionality.
        /// </summary>
        J = 0x00000200,

        /// <summary>
        /// Support the generic pass-through authentication.
        /// </summary>
        K = 0x00000400,

        /// <summary>
        /// Support the concurrent RPC calls.
        /// </summary>
        L = 0x00000800,

        /// <summary>
        /// Support avoiding of user account database replication.
        /// </summary>
        M = 0x00001000,

        /// <summary>
        /// Support avoiding of Security Authority database replication.
        /// </summary>
        N = 0x00002000,

        /// <summary>
        /// Support strong keys.
        /// </summary>
        O = 0x00004000,

        /// <summary>
        /// Support transitive trusts.
        /// </summary>
        P = 0x00008000,

        /// <summary>
        /// Support DNS domain trusts. This flag has not been used and SHOULD be ignored.
        /// </summary>
        Q = 0x00010000,

        /// <summary>
        /// Support the NetrServerPasswordSet2 functionality.
        /// </summary>
        R = 0x00020000,

        /// <summary>
        /// Support the NetrLogonGetDomainInfo functionality.
        /// </summary>
        S = 0x00040000,

        /// <summary>
        /// Support cross-forest trusts.
        /// </summary>
        T = 0x00080000,

        /// <summary>
        /// Support neutralizing Microsoft Windows NT 4.0 operating system emulation. 
        /// Note that when this flag is negotiated between the client and the server, 
        /// it indicates that the server SHOULD ignore the NT4Emulator ADM element.
        /// </summary>
        U = 0x00100000,

        /// <summary>
        /// Support RODC pass-through to different domains.
        /// </summary>
        V = 0x00200000,

        /// <summary>
        /// Support AES encryption and SHA2 hashing.
        /// </summary>
        W = 0x01000000,

        /// <summary>
        /// Not used. It MUST be ignored on receipt.
        /// </summary>
        X = 0x20000000,

        /// <summary>
        /// Support Secure RPC.
        /// </summary>
        Y = 0x40000000
    }
}
