// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// Smb2Packet is the class which every single smb2 packet will inherit
    /// </summary>
    public abstract class Smb2Packet : StackPacket
    {
        public object AssociatedObject
        {
            get;
            set;
        }

        /// <summary>
        /// NOT IMPLEMENTED. To create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>The copy of this instance</returns>
        public override StackPacket Clone()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Build a Smb2Packet from a byte array
        /// </summary>
        /// <param name="data">The byte array</param>
        /// <param name="consumedLen">The consumed data length</param>
        /// <param name="expectedLen">The expected data length</param>
        internal abstract void FromBytes(byte[] data, out int consumedLen, out int expectedLen);
    }
}
