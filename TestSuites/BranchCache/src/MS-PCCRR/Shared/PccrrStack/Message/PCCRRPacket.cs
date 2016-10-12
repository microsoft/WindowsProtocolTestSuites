// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrr
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This is the abstract class.
    /// </summary>
    public abstract class PccrrPacket
    {
        /// <summary>
        /// The X_P2P_PeerDist
        /// </summary>
        public const string XP2PPeerDist = "Version=1.0";

        /// <summary>
        /// The Accept_Encoding
        /// </summary>
        public const string AcceptEncoding = "gzip, deflate, peerdist";

        /// <summary>
        /// The UserAgent
        /// </summary>
        public const string UserAgent = "Peernet HTTP Transport/1.0";

        /// <summary>
        /// Initializes a new instance of the <see cref="PccrrPacket"/> class.
        /// </summary>
        protected PccrrPacket()
            : base()
        {
        }

        /// <summary>
        /// Gets the type of the packet.
        /// </summary>
        /// <returns>The type of the packet.</returns>
        public abstract MsgType_Values PacketType
        {
            get;
        }

        /// <summary>
        /// Encode pack.
        /// </summary>
        /// <returns>Encode bytes.</returns>
        public abstract byte[] Encode();

        /// <summary>
        /// Decode pack.
        /// </summary>
        /// <param name="rawdata">The rawdata.</param>
        /// <returns>The PccrrPacket.</returns>
        public abstract PccrrPacket Decode(byte[] rawdata);
    }
}
