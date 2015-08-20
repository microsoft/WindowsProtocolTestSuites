// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// TransportConfig stores the configurable parameters used by transport over Netbios.
    /// </summary>
    public class NetbiosServerConfig : NetbiosTransportConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor a NetbiosTransportConfig object that contains the config for Netbios client.
        /// </summary>
        /// <param name="adapterIndex">
        /// a byte value that indicates which network adapter will be used.
        /// </param>
        /// <param name="maxSessions">
        /// an int value that specifies the maximum count of sessions.
        /// </param>
        /// <param name="maxNames">
        /// an int value that specifies the maximum count of names.
        /// </param>
        public NetbiosServerConfig(byte adapterIndex, int maxSessions, int maxNames)
            : base()
        {
            this.Role = Role.Server;

            this.MaxSessions = maxSessions;
            this.MaxNames = maxNames;
            this.AdapterIndex = adapterIndex;
        }


        /// <summary>
        /// Constructor a NetbiosTransportConfig object that contains the config for Netbios client.
        /// </summary>
        /// <param name="serverNetbiosName">
        /// a string that represents the netbios name of server.
        /// </param>
        /// <param name="adapterIndex">
        /// a byte value that indicates which network adapter will be used.
        /// </param>
        /// <param name="maxSessions">
        /// an int value that specifies the maximum count of sessions.
        /// </param>
        /// <param name="maxNames">
        /// an int value that specifies the maximum count of names.
        /// </param>
        public NetbiosServerConfig(string serverNetbiosName, byte adapterIndex, int maxSessions, int maxNames)
            : this(adapterIndex, maxSessions, maxNames)
        {
            this.LocalNetbiosName = serverNetbiosName;
        }

        #endregion
    }
}
