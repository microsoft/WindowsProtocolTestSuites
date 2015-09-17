// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp
{
    /// <summary>
    /// the empty packet of nlmp
    /// </summary>
    public class NlmpEmptyPacket : NlmpPacket
    {
        /// <summary>
        /// the length of payload
        /// </summary>
        protected override int PayLoadLength
        {
            get
            {
                return 0;
            }
        }


        /// <summary>
        /// default empty constructor
        /// </summary>
        public NlmpEmptyPacket()
        {
        }


        /// <summary>
        /// copy constructor.
        /// </summary>
        public NlmpEmptyPacket(
            NlmpEmptyPacket stackPacket
            )
            : base(stackPacket)
        {
        }


        /// <summary>
        /// decode packet from bytes
        /// </summary>
        /// <param name="packetBytes">the bytes contain packet</param>
        public NlmpEmptyPacket(
            byte[] packetBytes
            )
            : base(packetBytes)
        {
        }


        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>return the clone of this packet</returns>
        public override StackPacket Clone()
        {
            return new NlmpEmptyPacket(this);
        }


        /// <summary>
        /// write the payload to bytes. For all sub class to marshal itself.
        /// </summary>
        /// <returns>the bytes of struct</returns>
        protected override byte[] WriteStructToBytes()
        {
            return new byte[0];
        }


        /// <summary>
        /// read struct from bytes. All sub class override this to unmarshal itself.
        /// </summary>
        /// <param name="start">the start to read bytes</param>
        /// <param name="packetBytes">the bytes of struct</param>
        /// <returns>the read result, if success, return true.</returns>
        protected override bool ReadStructFromBytes(
            byte[] packetBytes, 
            int start
            )
        {
            return true;
        }


        /// <summary>
        /// set the version
        /// </summary>
        /// <param name="version">the new version</param>
        /// <exception cref="ArgumentException">Reserved of VERSION must be 0!</exception>
        public override void SetVersion(
            VERSION version
            )
        {
            if (version.Reserved.Reserved1 != 0 || version.Reserved.Reserved2 != 0 || version.Reserved.Reserved3 != 0)
            {
                throw new ArgumentException("Reserved of VERSION must be 0!", "version");
            }
        }


        /// <summary>
        /// set the negotiate flags
        /// </summary>
        /// <param name="negotiateFlags">the new flags</param>
        public override void SetNegotiateFlags(NegotiateTypes negotiateFlags)
        {
        }
    }
}
