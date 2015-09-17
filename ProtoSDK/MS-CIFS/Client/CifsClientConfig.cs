// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestTools.StackSdk.Transport;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// the config for cifs client
    /// </summary>
    public class CifsClientConfig
    {
        #region fields

        private ushort ncbBufferSize;
        private byte ncbMaxSessions;
        private byte ncbMaxNames;
        private bool isWriteAndCloseResponseExtraPadding;

        /// <summary>
        /// default value of Netbios buffer size
        /// </summary>
        private const ushort defaultNcbBufferSize = 4096;

        /// <summary>
        /// default number of Netbios max sessions
        /// </summary>
        private const byte defaultNcbMaxSessions = 16;

        /// <summary>
        /// default number of Netbios max names
        /// </summary>
        private const byte defaultNcbMaxNames = 16;

        /// <summary>
        /// default value of whether the WriteAndCloseResponse has extra padding bytes
        /// </summary>
        private const bool defaultIsWriteAndCloseResponseExtraPadding = true;

        #endregion


        #region properties

        /// <summary>
        /// the buffer size in NCB to send or receive data.
        /// </summary>
        public ushort NcbBufferSize
        {
            get
            {
                return this.ncbBufferSize;
            }
            set
            {
                this.ncbBufferSize = value;
            }
        }


        /// <summary>
        /// the max sessions(ncb_callname[0]) used in NCBRESET.
        /// </summary>
        public byte NcbMaxSessions
        {
            get
            {
                return this.ncbMaxSessions;
            }
            set
            {
                this.ncbMaxSessions = value;
            }
        }


        /// <summary>
        /// the max names(ncb_callname[2]) used in NCBRESET.
        /// </summary>
        public byte NcbMaxNames
        {
            get
            {
                return this.ncbMaxNames;
            }
            set
            {
                this.ncbMaxNames = value;
            }
        }


        /// <summary>
        /// If true, the WriteAndClose Response will be parsed as Windows implementation.
        /// [WBN: Windows NT Server appends three NULL bytes to WriteAndCloseResponse message, 
        /// following the ByteCount field. These three bytes are not message data and can safely be 
        /// discarded.]. Otherwise, the WriteAndClose Response will be parsed as protocol behavior
        /// in which no extra data is padded. 
        /// </summary>
        public bool IsWriteAndCloseResponseExtraPadding
        {
            get
            {
                return this.isWriteAndCloseResponseExtraPadding;
            }
            set
            {
                this.isWriteAndCloseResponseExtraPadding = value;
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// default constructor
        /// </summary>
        public CifsClientConfig()
        {
            // set the default values:
            this.NcbBufferSize = defaultNcbBufferSize;
            this.NcbMaxSessions = defaultNcbMaxSessions;
            this.NcbMaxNames = defaultNcbMaxNames;
            this.isWriteAndCloseResponseExtraPadding = defaultIsWriteAndCloseResponseExtraPadding;
        }


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="buffSize">the buffer size in NCB to send or receive data</param>
        /// <param name="maxSessions">the max sessions(ncb_callname[0]) used in NCBRESET.</param>
        /// <param name="maxNames">the max names(ncb_callname[2]) used in NCBRESET.</param>
        public CifsClientConfig(
            ushort buffSize,
            byte maxSessions,
            byte maxNames)
        {
            this.NcbBufferSize = buffSize;
            this.NcbMaxSessions = maxSessions;
            this.NcbMaxNames = maxNames;
            this.IsWriteAndCloseResponseExtraPadding = defaultIsWriteAndCloseResponseExtraPadding;
        }

        #endregion
    }
}