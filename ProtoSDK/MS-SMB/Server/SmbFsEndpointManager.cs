// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// Used to manage smb and fs endpoint
    /// </summary>
    internal class SmbFsEndpointManager : EndpointManager<SmbServerConnection, FsEndpoint>
    {
        /// <summary>
        /// Create file service endpoint from smb endpoint
        /// </summary>
        /// <param name="underProtocolEndpoint">The smb endpoint</param>
        /// <returns>The file service endpoint</returns>
        protected override FsEndpoint CreateCurrentProtocolEndpoint(SmbServerConnection underProtocolEndpoint)
        {
            FsEndpoint fsEndpoint = new FsEndpoint();

            try
            {
                fsEndpoint.IpEndpoint = underProtocolEndpoint.GetClientIPEndPoint();
                fsEndpoint.EndpointType = FsEndpointType.Tcp;
            }
            catch (InvalidCastException)
            {
                fsEndpoint.EndpointType = FsEndpointType.NetBios;
                fsEndpoint.NetBiosEndpoint = underProtocolEndpoint.GetClientNetbiosSessionId();
            }

            return fsEndpoint;
        }
    }
}
