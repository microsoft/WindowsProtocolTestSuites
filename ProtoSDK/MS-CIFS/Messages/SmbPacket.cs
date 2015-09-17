// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// the basic packet of SMB
    /// defined common method of all SMB packets
    /// </summary>
    public abstract class SmbPacket : SmbFamilyPacket
    {
        #region fields

        /// <summary>
        /// Direct TCP Transport header
        /// </summary>
        protected TransportHeader transportHeader;


        /// <summary>
        /// the smb header
        /// </summary>
        protected SmbHeader smbHeader;


        /// <summary>
        /// the smb parameter
        /// </summary>
        protected SmbParameters smbParametersBlock;


        /// <summary>
        /// the smb data
        /// </summary>
        protected SmbData smbDataBlock;


        /// <summary>
        /// The result of signature verification
        /// </summary>
        private bool isSignatureCorrect;

        #endregion


        #region properties

        /// <summary>
        /// Get the type of the packet: Request or Response, and Single or Compounded.
        /// </summary>
        /// <returns>the type of the packet.</returns>
        public abstract SmbPacketType PacketType
        {
            get;
        }


        /// <summary>
        /// check if the packet is required to sign. 
        /// </summary>
        public bool IsSignRequired
        {
            get
            {
                return (this.smbHeader.SecurityFeatures != 0);
            }
        }


        /// <summary>
        /// Direct TCP Transport Header
        /// Auto filled when ToBytes(), if not explicitly set.
        /// </summary>
        public TransportHeader TransportHeader
        {
            get
            {
                return this.transportHeader;
            }
            set
            {
                this.transportHeader = value;
            }
        }


        /// <summary>
        /// The SMB Header.
        /// </summary>
        public SmbHeader SmbHeader
        {
            get
            {
                return this.smbHeader;
            }
            set
            {
                this.smbHeader = value;
            }
        }


        /// <summary>
        /// The result of signature verification
        /// </summary>
        public bool IsSignatureCorrect
        {
            get
            {
                return this.isSignatureCorrect;
            }
            set
            {
                this.isSignatureCorrect = value;
            }
        }


        /// <summary>
        /// The SMB Parameters.
        /// </summary>
        protected internal SmbParameters SmbParametersBlock
        {
            get
            {
                return this.smbParametersBlock;
            }
            set
            {
                this.smbParametersBlock = value;
            }
        }


        /// <summary>
        /// The SMB Data.
        /// </summary>
        protected internal SmbData SmbDataBlock
        {
            get
            {
                return this.smbDataBlock;
            }
            set
            {
                this.smbDataBlock = value;
            }
        }


        /// <summary>
        /// get the size of Header. it is always 32.
        /// </summary>
        internal int HeaderSize
        {
            get
            {
                return 32;
            }
        }


        /// <summary>
        /// get the size of Parameters
        /// </summary>
        internal int ParametersSize
        {
            get
            {
                this.EncodeParameters();
                return 1 + this.smbParametersBlock.WordCount * 2;
            }
        }


        /// <summary>
        /// get the size of Data
        /// </summary>
        internal int DataSize
        {
            get
            {
                this.EncodeData();
                return 2 + this.smbDataBlock.ByteCount;
            }
        }

        #endregion


        #region constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        protected SmbPacket()
            : base()
        {
            this.smbParametersBlock.Words = new ushort[0];
            this.smbDataBlock.Bytes = new byte[0];
            this.isSignatureCorrect = true;
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        protected SmbPacket(byte[] data)
            : base(data)
        {
            this.smbParametersBlock.Words = new ushort[0];
            this.smbDataBlock.Bytes = new byte[0];
            this.isSignatureCorrect = true;
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">the smbPacket is null.</exception>
        protected SmbPacket(SmbPacket smbPacket)
            : base()
        {
            if (smbPacket == null)
            {
                throw new ArgumentNullException("smbPacket",
                    "the copy constructor of smbpacket can not accept null smbPacket.");
            }

            lock (smbPacket)
            {
                this.isSignatureCorrect = smbPacket.isSignatureCorrect;
                this.transportHeader = new TransportHeader();
                this.transportHeader.Zero = smbPacket.transportHeader.Zero;
                this.transportHeader.StreamProtocolLength = new byte[3];
                this.transportHeader.StreamProtocolLength = smbPacket.transportHeader.StreamProtocolLength;

                this.SmbHeader = new SmbHeader();
                this.smbHeader.Protocol = smbPacket.smbHeader.Protocol;
                this.smbHeader.Command = smbPacket.smbHeader.Command;
                this.smbHeader.Status = smbPacket.smbHeader.Status;
                this.smbHeader.Flags = smbPacket.smbHeader.Flags;
                this.smbHeader.Flags2 = smbPacket.smbHeader.Flags2;
                this.smbHeader.PidHigh = smbPacket.smbHeader.PidHigh;
                this.smbHeader.SecurityFeatures = smbPacket.smbHeader.SecurityFeatures;
                this.smbHeader.Reserved = smbPacket.smbHeader.Reserved;
                this.smbHeader.Tid = smbPacket.smbHeader.Tid;
                this.smbHeader.PidLow = smbPacket.smbHeader.PidLow;
                this.smbHeader.Uid = smbPacket.smbHeader.Uid;
                this.smbHeader.Mid = smbPacket.smbHeader.Mid;

                this.SmbParametersBlock = new SmbParameters();
                this.smbParametersBlock.WordCount = smbPacket.smbParametersBlock.WordCount;
                if (smbPacket.smbParametersBlock.Words != null)
                {
                    this.smbParametersBlock.Words = new ushort[smbPacket.smbParametersBlock.Words.Length];
                    Array.Copy(smbPacket.smbParametersBlock.Words,
                        this.smbParametersBlock.Words,
                        smbPacket.smbParametersBlock.Words.Length);
                }
                else
                {
                    this.smbParametersBlock.Words = new ushort[0];
                }

                this.SmbDataBlock = new SmbData();
                this.smbDataBlock.ByteCount = smbPacket.smbDataBlock.ByteCount;
                if (smbPacket.smbDataBlock.Bytes != null)
                {
                    this.smbDataBlock.Bytes = new byte[smbPacket.smbDataBlock.Bytes.Length];
                    Array.Copy(smbPacket.smbDataBlock.Bytes,
                        this.smbDataBlock.Bytes,
                        smbPacket.smbDataBlock.Bytes.Length);
                }
                else
                {
                    this.smbDataBlock.Bytes = new byte[0];
                }
            }
        }

        #endregion


        #region encoding

        /// <summary>
        /// to marshal the packet struct to bytes array.
        /// auto filled when ToBytes(), if not explicitly set.
        /// </summary>
        /// <returns>the bytes array of the packet.</returns>
        public override byte[] ToBytes()
        {
            // Convert the CIFS packet with out TransportHeader to byte array
            return ToBytes(true);
        }


        /// <summary>
        /// Convert the CIFS packet to byte array
        /// </summary>
        /// <param name="withoutTransportHeader">Indicates if CIFS packet have  TransportHeader.</param>
        /// <returns>byte array of CIFS packet</returns>
        protected byte[] ToBytes(bool withoutTransportHeader)
        {
            byte[] bytesWithoutHeader = this.GetBytesWithoutHeader();

            // get the total CIFS packet size:
            int size = this.HeaderSize + bytesWithoutHeader.Length;

            if (this.transportHeader.StreamProtocolLength == null)
            {
                // set transportHeader:
                this.transportHeader.StreamProtocolLength = new byte[3];
                this.transportHeader.StreamProtocolLength[0] = (byte)(size >> 16);
                this.transportHeader.StreamProtocolLength[1] = (byte)(size >> 8);
                this.transportHeader.StreamProtocolLength[2] = (byte)size;
            }

            if (!withoutTransportHeader)
            {
                // init packet bytes:
                byte[] packetBytes = new byte[size];
                using (MemoryStream memoryStream = new MemoryStream(packetBytes))
                {
                    using (Channel channel = new Channel(null, memoryStream))
                    {
                        channel.BeginWriteGroup();
                        channel.Write<TransportHeader>(this.transportHeader);
                        channel.Write<SmbHeader>(this.smbHeader);
                        channel.WriteBytes(bytesWithoutHeader);
                        channel.EndWriteGroup();
                    }
                }
                return packetBytes;
            }
            else
            {
                // init packet bytes:
                byte[] packetBytes = new byte[size];
                using (MemoryStream memoryStream = new MemoryStream(packetBytes))
                {
                    using (Channel channel = new Channel(null, memoryStream))
                    {
                        channel.BeginWriteGroup();
                        channel.Write<SmbHeader>(this.smbHeader);
                        channel.WriteBytes(bytesWithoutHeader);
                        channel.EndWriteGroup();
                    }
                }
                return packetBytes;
            }
        }


        /// <summary>
        /// encode the smb parameters: from the concrete Smb Parameters to the general SmbParameters.
        /// </summary>
        protected abstract void EncodeParameters();


        /// <summary>
        /// encode the smb data:: from the concrete Smb Dada to the general SmbDada.
        /// </summary>
        protected abstract void EncodeData();


        /// <summary>
        /// to encode the SmbParameters and SmbDada into bytes.
        /// </summary>
        /// <returns>the bytes array of SmbParameters, SmbDada, and AndX if existed.</returns>
        protected internal abstract byte[] GetBytesWithoutHeader();

        #endregion


        #region decoding

        /// <summary>
        /// to unmarshal the SmbParameters struct from a channel.
        /// </summary>
        /// <param name="channel">the channel started with SmbParameters.</param>
        /// <returns>the size in bytes of the SmbParameters.</returns>
        protected internal virtual int ReadParametersFromChannel(Channel channel)
        {
            this.smbParametersBlock = channel.Read<SmbParameters>();
            this.DecodeParameters();
            return 1 + this.smbParametersBlock.WordCount * 2;
        }


        /// <summary>
        /// to unmarshal the SmbDada struct from a channel.
        /// </summary>
        /// <param name="channel">the channel started with SmbDada.</param>
        /// <returns>the size in bytes of the SmbDada.</returns>
        protected internal virtual int ReadDataFromChannel(Channel channel)
        {
            this.smbDataBlock = channel.Read<SmbData>();
            this.DecodeData();
            return 2 + this.smbDataBlock.ByteCount;
        }


        /// <summary>
        /// to decode the smb parameters: from the general SmbParameters to the concrete Smb Parameters.
        /// </summary>
        protected abstract void DecodeParameters();


        /// <summary>
        /// to decode the smb data: from the general SmbDada to the concrete Smb Data.
        /// </summary>
        protected abstract void DecodeData();

        #endregion


        #region message signature mechenism
        /// <summary>
        /// Sign the packet: Compute the signature and put it in the Signature field of header.
        /// </summary>
        /// <param name="clientNextSendSequenceNumber">sequence number for the next signed
        /// request being sent.</param>
        /// <param name="sessionKey">the session key.</param>
        /// <exception cref="System.ArgumentException">
        /// the ClientNextSendSequenceNumber of Connection must be initialized to 2.</exception>
        public void Sign(ulong clientNextSendSequenceNumber, byte[] sessionKey)
        {
            this.Sign(clientNextSendSequenceNumber, sessionKey, new byte[0]);
        }


        /// <summary>
        /// Sign the packet: Compute the signature and put it in the Signature field of header.
        /// </summary>
        /// <param name="clientNextSendSequenceNumber">sequence number for the next signed
        /// request being sent.</param>
        /// <param name="sessionKey">the session key.</param>
        /// <param name="challengeResponse">the challenge response.</param>
        /// <exception cref="System.ArgumentException">
        /// the ClientNextSendSequenceNumber of Connection must be initialized to 2.</exception>
        public void Sign(ulong clientNextSendSequenceNumber, byte[] sessionKey, byte[] challengeResponse)
        {
            if (clientNextSendSequenceNumber < 2)
            {
                throw new ArgumentException(
                    "the ClientNextSendSequenceNumber of Connection must be initialized to 2.",
                    "clientNextSendSequenceNumber");
            }

            // update the signature flag
            this.smbHeader.Flags2 |= SmbFlags2.SMB_FLAGS2_SMB_SECURITY_SIGNATURE;

            // if the session key is null, 
            // just set the security signature in flag2,
            // then, return.
            if (sessionKey == null)
            {
                return;
            }

            // the 32-bit sequence number is copied in little-endian order (least-significant byte first)
            // into the first 4 bytes of the SecuritySignature field.
            this.smbHeader.SecurityFeatures = clientNextSendSequenceNumber;

            // get md5 hascode of session key and message body
            byte[] securitySignature = CifsMessageUtils.CreateSignature(this.ToBytes(true), sessionKey, challengeResponse);

            // update to security signature.
            this.smbHeader.SecurityFeatures = BitConverter.ToUInt64(securitySignature, 0);
        }

        #endregion


        /// <summary>
        /// the version of SMB packet, always returns SMB.
        /// </summary>
        public override SmbVersion Version
        {
            get 
            { 
                return SmbVersion.SMB; 
            }
        }


        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>The copy of this instance</returns>
        public override StackPacket Clone()
        {
            return ObjectUtility.DeepClone(this) as SmbPacket;
        }
    }
}