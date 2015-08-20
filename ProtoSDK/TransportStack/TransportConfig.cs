// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Net;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// TransportConfig stores the configurable parameters used by transport.<para/>
    /// user can construct its sub classes, such as SocketTransportConfig, NetbiosTransportConfig
    /// and StreamConfig.<para/>
    /// user can also use the Create APIs of TransportConfig to create some specified config.
    /// </summary>
    public abstract class TransportConfig
    {
        #region Fields

        /// <summary>
        /// a const int value that specifies the default buffer size.
        /// </summary>
        private const int DefaultBufferSize = 8 * 1024;

        /// <summary>
        /// the transport role.
        /// </summary>
        private Role role;

        /// <summary>
        /// the transport type.
        /// </summary>
        private StackTransportType type;

        /// <summary>
        /// the size of buffer used for receiving data.
        /// </summary>
        private int bufferSize;

        #endregion

        #region Properties

        /// <summary>
        /// get/set a Role enum that specifies the transport role.
        /// </summary>
        public Role Role
        {
            get
            {
                return this.role;
            }
            set
            {
                this.role = value;
            }
        }


        /// <summary>
        /// get/set a StackTransportType enum that specifies the transport type.
        /// </summary>
        public StackTransportType Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }


        /// <summary>
        /// get/set an int value that specifies the size of buffer used for receiving data.<para/>
        /// by default, it is 8192.
        /// </summary>
        public int BufferSize
        {
            get
            {
                return this.bufferSize;
            }
            set
            {
                this.bufferSize = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// constructor
        /// </summary>
        protected TransportConfig()
        {
            this.bufferSize = DefaultBufferSize;
            this.role = Role.None;
            this.type = StackTransportType.None;
        }

        #endregion
    }
}
