// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa
{
    /// <summary>
    /// LSA client context
    /// </summary>
    public class LsaClientContext
    {
        // RPC transport context
        internal RpceClientContext rpceTransportContext;


        /// <summary>
        /// Initialize a LSA client context class
        /// </summary>
        internal LsaClientContext()
        {
        }


        /// <summary>
        /// Gets RPC transport context.
        /// </summary>
        [CLSCompliant(false)]
        public RpceClientContext RpceTransportContext
        {
            get
            {
                return rpceTransportContext;
            }
        }
    }
}
