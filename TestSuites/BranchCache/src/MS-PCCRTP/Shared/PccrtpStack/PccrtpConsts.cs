// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp
{
    public static class PccrtpConsts
    {
        /// <summary>
        /// The Accept-Encoding header field in PCCRTP message.
        /// </summary>
        public const string AcceptEncodingHttpHeader = "Accept-Encoding";

        /// <summary>
        /// The X-P2P-PeerDist header field in PCCRTP message.
        /// </summary>
        public const string XP2PPeerDistHttpHeader = "X-P2P-PeerDist";

        /// <summary>
        /// The X-P2P-PeerDistEx header field in PCCRTP message.
        /// </summary>
        public const string XP2PPeerDistExHttpHeader = "X-P2P-PeerDistEx";
    }
}
