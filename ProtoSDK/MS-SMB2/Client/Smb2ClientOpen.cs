// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// OperationBucket
    /// </summary>
    public class OperationBucket
    {
        /// <summary>
        /// A value of FALSE indicates that there is an outstanding lock or unlock request using this BucketNumber and SequenceNumber combination.
        /// </summary>
        public bool Free
        {
            get;
            set;
        }

        /// <summary>
        /// A rolling 4-bit sequence number which counts from 0 through 15.
        /// </summary>
        public byte SequenceNumber
        {
            get;
            set;
        }
    }

    /// <summary>
    /// A structure contains information about Open
    /// </summary>
    public class Smb2ClientOpen
    {
        /// <summary>
        /// The SMB2_FILEID, as specified in section 2.2.14.1, returned by the server for this open.
        /// </summary>
        public FILEID FileId
        {
            get;
            set;
        }

        /// <summary>
        /// A reference to the tree connect on which this Open was established
        /// </summary>
        public Smb2ClientTreeConnect TreeConnect
        {
            get;
            set;
        }

        /// <summary>
        /// A reference to the SMB2 transport connection on which this open was established.
        /// </summary>
        public Smb2ClientConnection Connection
        {
            get;
            set;
        }

        /// <summary>
        /// The current oplock level for this open.
        /// </summary>
        public OplockLevel_Values OplockLevel
        {
            get;
            set;
        }

        /// <summary>
        /// A Boolean that indicates whether this open is setup for reestablishment after a disconnect.
        /// </summary>
        public bool Durable
        {
            get;
            set;
        }

        /// <summary>
        /// A Unicode string with the name of the file.
        /// </summary>
        public string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// A Boolean that indicates whether resiliency was granted for this open by the server.
        /// </summary>
        public bool ResilientHandle
        {
            get;
            set;
        }

        /// <summary>
        /// The time at which the last network disconnect occurred on the connection used by this open.
        /// </summary>
        public DateTime LastDisconnectTime
        {
            get;
            set;
        }

        /// <summary>
        /// The minimum time (in milliseconds) for which the server will hold this open, 
        /// while waiting for the client to reestablish the open after a network disconnect.
        /// </summary>
        public TimeSpan ResilientTimeout
        {
            get;
            set;
        }

        /// <summary>
        /// An array of 64 elements used to maintain information about outstanding lock and unlock operations performed on resilient Opens. 
        /// </summary>
        public OperationBucket[] OperationBuckets
        {
            get;
            set;
        }
    }
}
