// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// TransportConfig stores the configurable parameters used by transport over Netbios.
    /// </summary>
    public class NetbiosClientConfig : NetbiosTransportConfig
    {
        #region Constructor

        /// <summary>
        /// Construct a NetbiosTransportConfig object that contains the config for Netbios client.
        /// </summary>
        /// <param name="serverNetbiosName">
        /// a string that represents the netbios name of server.
        /// </param>
        /// <param name="clientNetbiosName">
        /// a string that represents the netbios name of client.
        /// </param>
        /// <param name="adapterIndex">
        /// a byte value that specifies the index of local network adapter to use.
        /// </param>
        /// <param name="maxSessions">
        /// an int value that specifies the maximum count of sessions.
        /// </param>
        /// <param name="maxNames">
        /// an int value that specifies the maximum count of names.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when the serverNetbiosName is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when clientNetbiosName is null.
        /// </exception>
        public NetbiosClientConfig(
            string serverNetbiosName, string clientNetbiosName, byte adapterIndex, int maxSessions, int maxNames)
            : base()
        {
            if (serverNetbiosName == null)
            {
                throw new ArgumentNullException("serverNetbiosName");
            }

            if (clientNetbiosName == null)
            {
                throw new ArgumentNullException("clientNetbiosName");
            }

            this.Role = Role.Client;

            this.AdapterIndex = adapterIndex;
            this.MaxSessions = maxSessions;
            this.MaxNames = maxNames;
            this.LocalNetbiosName = clientNetbiosName;
            this.RemoteNetbiosName = serverNetbiosName;
        }

        #endregion
    }
}
