// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.BranchCache
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Receive Pccrr request message handle.
    /// </summary>
    public class ReceivedPccrrRequestEventArg : EventArgs
    {
        #region fields

        /// <summary>
        /// The pccrr port number.
        /// </summary>
        private int pccrrPort;

        /// <summary>
        /// The segment Id list in pccrr protocol request
        /// </summary>
        private byte[] segmentID;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ReceivedPccrrRequestEventArg"/> class
        /// Receive Pccrr request message handle.
        /// </summary>
        /// <param name="pccrrListenPort">The pccrr protocol used port</param>
        /// <param name="firstSegmentID">The segmentID inclued in pccrr request</param>
        public ReceivedPccrrRequestEventArg(int pccrrListenPort, byte[] firstSegmentID)
        {
            this.pccrrPort = pccrrListenPort;

            this.segmentID = firstSegmentID;
        }

        #region Property

        /// <summary>
        /// Gets the pccrr protocol used port number
        /// </summary>
        public int PccrrPort
        {
            get
            {
                return this.pccrrPort;
            }
        }

        /// <summary>
        /// Gets the pccrr request segmentID
        /// </summary>
        public byte[] SegmentID
        {
            get
            {
                return this.segmentID;
            }
        }

        #endregion
    }
}
