// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp
{
    /// <summary>
    /// the base packet of nlmp
    /// </summary>
    public abstract class NlmpPacket : StackPacket
    {
        #region Attributes

        /// <summary>
        /// the common header of all nlmp packet
        /// </summary>
        protected NLMP_HEADER header;

        /// <summary>
        /// the common header of all nlmp packet
        /// </summary>
        public NLMP_HEADER Header
        {
            get
            {
                return this.header;
            }
            set
            {
                this.header = value;
            }
        }


        /// <summary>
        /// the length of nlmp header
        /// </summary>
        protected int HeaderLength
        {
            get
            {
                return NlmpUtility.NLMP_HEADER_SIZE;
            }
        }


        /// <summary>
        /// get the payload length.sub class should override this property.
        /// </summary>
        protected abstract int PayLoadLength
        {
            get;
        }


        /// <summary>
        /// get the packet length
        /// </summary>
        public int Length
        {
            get
            {
                return HeaderLength + PayLoadLength;
            }
        }


        #endregion

        #region Constructor and Copy Constructor.

        /// <summary>
        /// default empty constructor
        /// </summary>
        protected NlmpPacket()
        {
            // 'N', 'T', 'L', 'M', 'S', 'S', 'P', '\0'
            // in the little order
            this.header.Signature = new byte[] { 0x4E, 0x54, 0x4C, 0x4D, 0x53, 0x53, 0x50, 0x00 };
        }


        /// <summary>
        /// copy constructor.
        /// </summary>
        /// <param name="nlmpPacket">the source packet</param>
        /// <exception cref="ArgumentNullException">the source packet must not be null</exception>
        protected NlmpPacket(
            NlmpPacket nlmpPacket
            )
        {
            if (nlmpPacket == null)
            {
                throw new ArgumentNullException("nlmpPacket");
            }

            this.header = nlmpPacket.header;
        }


        /// <summary>
        /// decode packet from bytes
        /// </summary>
        /// <param name="packetBytes">the bytes contain packet</param>
        /// <exception cref="ArgumentNullException">packetBytes can not be null</exception>
        [SuppressMessage(
            "Microsoft.Usage",
            "CA2214:DoNotCallOverridableMethodsInConstructors"
            )]
        protected NlmpPacket(
            byte[] packetBytes
            )
        {
            if (packetBytes == null)
            {
                throw new ArgumentNullException("packetBytes");
            }

            if (packetBytes.Length < HeaderLength)
            {
                throw new InvalidOperationException("the bytes just contains NlmpHeader data!");
            }

            int start = 0;

            this.header.Signature = NlmpUtility.ReadBytes(
                packetBytes, ref start, NlmpUtility.NLMP_HEADER_SIGNATURE_SIZE);

            this.header.MessageType = (MessageType_Values)NlmpUtility.BytesToStruct<uint>(packetBytes, ref start);

            ReadStructFromBytes(packetBytes, start);
        }


        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>return the clone of this packet</returns>
        public override abstract StackPacket Clone();


        #endregion

        #region Marshal UnMarshal and Members

        /// <summary>
        /// set the version
        /// </summary>
        /// <param name="version">the new version</param>
        public abstract void SetVersion(
            VERSION version
            );


        /// <summary>
        /// set the negotiate flags
        /// </summary>
        /// <param name="negotiateFlags">the new flags</param>
        public abstract void SetNegotiateFlags(
            NegotiateTypes negotiateFlags
            );


        /// <summary>
        /// write the payload to bytes. For all sub class to marshal itself.
        /// </summary>
        /// <returns>the bytes of struct</returns>
        protected abstract byte[] WriteStructToBytes();


        /// <summary>
        /// marshal the packet struct to bytes array.
        /// </summary>
        /// <returns>get the bytes of packet</returns>
        public override byte[] ToBytes()
        {
            byte[] bytes = new byte[Length];

            Array.Copy(NlmpUtility.StructGetBytes(this.header), bytes, HeaderLength);
            Array.Copy(WriteStructToBytes(), 0, bytes, HeaderLength, PayLoadLength);

            return bytes;
        }


        /// <summary>
        /// read struct from bytes. All sub class override this to unmarshal itself.
        /// </summary>
        /// <param name="start">the start to read bytes</param>
        /// <param name="packetBytes">the bytes of struct</param>
        /// <returns>the read result, if success, return true.</returns>
        protected abstract bool ReadStructFromBytes(
            byte[] packetBytes, 
            int start
            );


        #endregion
    }
}
