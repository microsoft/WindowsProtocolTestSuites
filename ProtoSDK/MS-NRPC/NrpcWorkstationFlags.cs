// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc
{
    /// <summary>
    /// A set of bit flags specifying workstation behavior.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum NrpcWorkstationFlags : uint
    {
        /// <summary>
        /// No flag is set.
        /// </summary>
        None = 0,

        /// <summary>
        /// A:<para/>
        /// Client will receive inbound trusts as specified in [MS-LSAD] section 2.2.7.9. 
        /// The client sets this bit in order to receive the inbound trusts.
        /// </summary>
        ClientReceivesInboundTrusts = 1,

        /// <summary>
        /// B:<para/>
        /// Client handles the update of the service principal name (SPN).
        /// </summary>
        ClientHandlesUpdateOfSpn = 2
    }
}
