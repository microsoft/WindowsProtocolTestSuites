// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The request which is waiting for response, this is a field in Smb2ClientConnection
    /// </summary>
    public class Smb2OutStandingRequest
    {
        internal ulong messageId;
        internal DateTime timeStamp;
        internal bool isHandleAsync;
        internal ulong asyncId;
        internal Smb2Packet request;

        /// <summary>
        /// The messageId of the request
        /// </summary>
        [CLSCompliant(false)]
        public ulong MessageId
        {
            get
            {
                return messageId;
            }
        }

        /// <summary>
        /// A time stamp of when the request was sent
        /// </summary>
        public DateTime TimeStamp
        {
            get
            {
                return timeStamp;
            }
        }
        
        /// <summary>
        /// Indicate if this request is handled async
        /// </summary>
        public bool IsHandleAsync
        {
            get
            {
                return isHandleAsync;
            }
        }

        /// <summary>
        /// The asyncId of this request
        /// </summary>
        [CLSCompliant(false)]
        public ulong AsyncId
        {
            get
            {
                return asyncId;
            }
        }

        /// <summary>
        /// The request packet
        /// </summary>
        public Smb2Packet Request
        {
            get
            {
                return request;
            }
        }
    }
}
