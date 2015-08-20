// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// StreamConfig stores the configurable parameters used by stream transport.
    /// </summary>
    public class StreamConfig : TransportConfig
    {
        #region Fields

        /// <summary>
        /// the stream for StreamTransport.
        /// </summary>
        private Stream stream;

        #endregion

        #region Properties

        /// <summary>
        /// get/set a Stream object that specifies the stream for StreamTransport.
        /// </summary>
        public Stream Stream
        {
            get
            {
                return this.stream;
            }
            set
            {
                this.stream = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// constructor
        /// </summary>
        public StreamConfig()
            : base()
        {
            this.Type = StackTransportType.Stream;
        }


        /// <summary>
        /// construct a TransportConfig object that contains the config for Stream transport.
        /// </summary>
        /// <param name="stream">
        /// an Stream object that indicates the stream to use.
        /// </param>
        public StreamConfig(Stream stream)
            : this()
        {
            this.stream = stream;
        }

        #endregion
    }
}
