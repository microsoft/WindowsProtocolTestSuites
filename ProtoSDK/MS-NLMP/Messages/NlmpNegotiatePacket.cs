// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp
{
    /// <summary>
    /// the negotiate packet of nlmp
    /// </summary>
    public class NlmpNegotiatePacket : NlmpPacket
    {
        /// <summary>
        /// 2.2.1.1 NEGOTIATE_MESSAGE The NEGOTIATE_MESSAGE defines an NTLM Negotiate message that is sent from the 
        /// client to the server. This message allows the client to specify its supported NTLM options to the server.
        /// </summary>
        private NEGOTIATE_MESSAGE payload;

        /// <summary>
        /// 2.2.1.1 NEGOTIATE_MESSAGE The NEGOTIATE_MESSAGE defines an NTLM Negotiate message that is sent from the 
        /// client to the server. This message allows the client to specify its supported NTLM options to the server.
        /// </summary>
        public NEGOTIATE_MESSAGE Payload
        {
            get
            {
                return this.payload;
            }
            set
            {
                this.payload = value;
            }
        }


        /// <summary>
        /// the length of NEGOTIATE_MESSAGE
        /// </summary>
        protected override int PayLoadLength
        {
            get
            {
                return NlmpUtility.NEGOTIATE_MESSAGE_CONST_SIZE + payload.DomainNameFields.Len 
                    + payload.WorkstationFields.Len;
            }
        }


        /// <summary>
        /// default empty constructor
        /// </summary>
        public NlmpNegotiatePacket()
        {
            this.header.MessageType = MessageType_Values.NEGOTIATE;
        }


        /// <summary>
        /// copy constructor.
        /// </summary>
        public NlmpNegotiatePacket(
            NlmpNegotiatePacket stackPacket
            )
            : base(stackPacket)
        {
            this.payload = stackPacket.payload;

            if (stackPacket.payload.DomainName != null)
            {
                this.payload.DomainName = new byte[stackPacket.payload.DomainName.Length];
                Array.Copy(
                    stackPacket.payload.DomainName, this.payload.DomainName, stackPacket.payload.DomainName.Length
                    );
            }

            if (stackPacket.payload.WorkstationName != null)
            {
                this.payload.WorkstationName = new byte[stackPacket.payload.WorkstationName.Length];
                Array.Copy(
                    stackPacket.payload.WorkstationName, this.payload.WorkstationName,
                    stackPacket.payload.WorkstationName.Length);
            }
        }


        /// <summary>
        /// decode packet from bytes
        /// </summary>
        /// <param name="packetBytes">the bytes contain packet</param>
        public NlmpNegotiatePacket(
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
            return new NlmpNegotiatePacket(this);
        }


        /// <summary>
        /// set the domain name
        /// </summary>
        /// <param name="domainName">the domain name</param>
        public void SetDomainName(
            string domainName
            )
        {
            payload.DomainName = NlmpUtility.StringGetBytes(domainName, false);
            payload.DomainNameFields.Len = (ushort)payload.DomainName.Length;
            payload.DomainNameFields.MaxLen = (ushort)payload.DomainName.Length;

            UpdateOffset();
        }


        /// <summary>
        /// set the workstation name
        /// </summary>
        /// <param name="workstationName">the workstation name</param>
        public void SetWorkstationName(
            string workstationName
            )
        {
            payload.WorkstationName = NlmpUtility.StringGetBytes(workstationName, false);
            payload.WorkstationFields.Len = (ushort)payload.WorkstationName.Length;
            payload.WorkstationFields.MaxLen = (ushort)payload.WorkstationName.Length;

            UpdateOffset();
        }


        /// <summary>
        /// update the offset of payload
        /// </summary>
        private void UpdateOffset()
        {
            payload.DomainNameFields.BufferOffset = (uint)(HeaderLength + NlmpUtility.NEGOTIATE_MESSAGE_CONST_SIZE);

            payload.WorkstationFields.BufferOffset = (uint)(
                HeaderLength + NlmpUtility.NEGOTIATE_MESSAGE_CONST_SIZE + payload.DomainNameFields.Len);
        }


        /// <summary>
        /// set the version
        /// </summary>
        /// <param name="version">the new version</param>
        public override void SetVersion(
            VERSION version
            )
        {
            payload.Version = version;
        }


        /// <summary>
        /// set the negotiate flags
        /// </summary>
        /// <param name="negotiateFlags">the new flags</param>
        public override void SetNegotiateFlags(
            NegotiateTypes negotiateFlags
            )
        {
            payload.NegotiateFlags = negotiateFlags;
        }


        /// <summary>
        /// write the payload to bytes. For all sub class to marshal itself.
        /// </summary>
        /// <returns>the bytes of struct</returns>
        protected override byte[] WriteStructToBytes()
        {
            byte[] bytes = NlmpUtility.StructGetBytes(payload);

            if (PayLoadLength != bytes.Length)
            {
                throw new InvalidOperationException("the payload length is not equal to the marshal size!");
            }

            return bytes;
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
            NEGOTIATE_MESSAGE negotiate = new NEGOTIATE_MESSAGE();

            negotiate.NegotiateFlags = (NegotiateTypes)NlmpUtility.BytesToStruct<uint>(packetBytes, ref start);
            negotiate.DomainNameFields = NlmpUtility.BytesToStruct<MESSAGE_FIELDS>(packetBytes, ref start);
            negotiate.WorkstationFields = NlmpUtility.BytesToStruct<MESSAGE_FIELDS>(packetBytes, ref start);
            negotiate.Version = NlmpUtility.BytesToStruct<VERSION>(packetBytes, ref start);

            int currentIndex = 0;
            while (currentIndex != start)
            {
                currentIndex = start;
                if (negotiate.DomainNameFields.Len != 0 && negotiate.DomainNameFields.BufferOffset == start)
                {
                    negotiate.DomainName = NlmpUtility.ReadBytes(
                        packetBytes, ref start, negotiate.DomainNameFields.Len);
                    continue;
                }
                else if (negotiate.WorkstationFields.Len != 0 && negotiate.WorkstationFields.BufferOffset == start)
                {
                    negotiate.WorkstationName = NlmpUtility.ReadBytes(
                        packetBytes, ref start, negotiate.WorkstationFields.Len);
                    continue;
                }
                else
                {
                    break;
                }
            }

            this.payload = negotiate;

            return true;
        }
    }
}
