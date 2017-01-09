// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    /// <summary>
    /// The type of computer.
    /// </summary>
    public enum ComputerType
    {
        /// <summary>
        /// It is NULL.
        /// </summary>
        Null,

        /// <summary>
        /// A name of a non-exist computer.
        /// </summary>
        NonExistComputer,

        /// <summary>
        /// The computer with the role of the server in the primary domain which is not a DC and serves as a server.
        /// </summary>
        NonDcServer,

        /// <summary>
        /// The computer with the role of the primary DC is also the root DC.
        /// </summary>
        PrimaryDc,

        /// <summary>
        /// The computer with the role of the client.
        /// </summary>
        Client,

        /// <summary>
        /// The computer with the role of the trust DC.
        /// </summary>
        TrustDc,
    }
}
