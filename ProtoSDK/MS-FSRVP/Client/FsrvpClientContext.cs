// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;


namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fsrvp
{
    /// <summary>
    /// The context of client. 
    /// </summary>
    public class FsrvpClientContext
    {
        // This field indicates the level of authentication or 
        /// message protection that remote procedure call (RPC) 
        /// will apply to a specific message exchange. 
        private RpceAuthenticationLevel authenticationLevel;

        /// <summary>
        /// A enum setting that the level of authentication or 
        /// message protection that remote procedure call (RPC) 
        /// will apply to a specific message exchange. 
        /// </summary>
        public RpceAuthenticationLevel AuthenticationLevel
        {
            get
            {
                return authenticationLevel;
            }
            set
            {
                authenticationLevel = value;
            }
        }
    }
}
