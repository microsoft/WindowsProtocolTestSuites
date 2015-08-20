// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// TransportConfig stores the configurable parameters used by transport over Netbios.
    /// </summary>
    public class NetbiosTransportConfig : TransportConfig
    {
        #region Fields

        /// <summary>
        /// the max sessions supported by the transport.
        /// </summary>
        private int maxSessions;

        /// <summary>
        /// the max Netbios names used to initialize the NCB. It is only used in NetBios tranport.
        /// </summary>
        private int maxNames;

        /// <summary>
        /// the local Netbios name. It is only used in NetBios tranport.
        /// </summary>
        private string localNetbiosName;

        /// <summary>
        /// the remote Betbios name. It is only used in NetBios tranport.
        /// </summary>
        private string remoteNetbiosName;

        /// <summary>
        /// Indicate which network adapter will be used
        /// </summary>
        private byte adapterIndex;

        #endregion

        #region Properties

        /// <summary>
        /// get/set an int value that specifies the max sessions supported by the transport.
        /// </summary>
        public int MaxSessions
        {
            get
            {
                return this.maxSessions;
            }
            set
            {
                this.maxSessions = value;
            }
        }


        /// <summary>
        /// get/set an int value that specifies the max Netbios names used to initialize the NCB.
        /// </summary>
        public int MaxNames
        {
            get
            {
                return this.maxNames;
            }
            set
            {
                this.maxNames = value;
            }
        }


        /// <summary>
        /// get/set a string that specifies the remote Betbios name.
        /// </summary>
        public string RemoteNetbiosName
        {
            get
            {
                return this.remoteNetbiosName;
            }
            set
            {
                this.remoteNetbiosName = value;
            }
        }


        /// <summary>
        /// get/set a string that specifies the local Netbios name.
        /// </summary>
        public string LocalNetbiosName
        {
            get
            {
                return this.localNetbiosName;
            }
            set
            {
                this.localNetbiosName = value;
            }
        }


        /// <summary>
        /// get/set a byte value that indicate which network adapter will be used
        /// </summary>
        public byte AdapterIndex
        {
            get
            {
                return adapterIndex;
            }
            set
            {
                adapterIndex = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public NetbiosTransportConfig()
        {
            this.Type = StackTransportType.Netbios;
        }

        #endregion
    }
}
