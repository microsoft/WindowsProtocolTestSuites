// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbNegotiate Response. 
    /// Do not support LM Negotiate response.
    /// </summary>
    public class SmbNegotiateResponsePacket : SmbSingleResponsePacket
    {
        #region Fields

        private SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Parameters smbParameters;
        private SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Data smbData;

        // If the server has selected the Core Protocol dialect, or if none of the offered protocols is supported 
        // by the server, then WordCount MUST be 0x01 and the dialect index (the selected dialect) MUST be returned
        // as the only parameter.
        private const byte WordCountOfCoreProtocol = 0x01;

        // If the server has selected any dialect from LAN Manager 1.0 through LAN Manager 2.1, then WordCount MUST
        // be 0x0D.
        private const byte WordCountOfLanManager = 0x0D;

        // If the server has selected the NT LAN Manager dialect, then WordCount MUST be 0x11.
        private const byte WordCountOfNtLanManager = 0x11;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Smb_Parameters:SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Parameters
        /// Core Protocol response share the DialectIndex member, others members will be ignore.
        /// </summary>
        public SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Parameters SmbParameters
        {
            get
            {
                return this.smbParameters;
            }
            set
            {
                this.smbParameters = value;
            }
        }


        /// <summary>
        /// get or set the Smb_Data:SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Data
        /// Core Protocol response share the same SmbData structure with NTLM.
        /// </summary>
        public SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Data SmbData
        {
            get
            {
                return this.smbData;
            }
            set
            {
                this.smbData = value;
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SmbNegotiateResponsePacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbNegotiateResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbNegotiateResponsePacket(SmbNegotiateResponsePacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

            this.smbParameters.WordCount = packet.SmbParameters.WordCount;
            this.smbParameters.DialectIndex = packet.SmbParameters.DialectIndex;
            this.smbParameters.SecurityMode = packet.SmbParameters.SecurityMode;
            this.smbParameters.MaxMpxCount = packet.SmbParameters.MaxMpxCount;
            this.smbParameters.MaxNumberVcs = packet.SmbParameters.MaxNumberVcs;
            this.smbParameters.MaxBufferSize = packet.SmbParameters.MaxBufferSize;
            this.smbParameters.MaxRawSize = packet.SmbParameters.MaxRawSize;
            this.smbParameters.SessionKey = packet.SmbParameters.SessionKey;
            this.smbParameters.Capabilities = packet.SmbParameters.Capabilities;
            this.smbParameters.SystemTime = new FileTime();
            this.smbParameters.SystemTime.Time = packet.SmbParameters.SystemTime.Time;
            this.smbParameters.ServerTimeZone = packet.SmbParameters.ServerTimeZone;
            this.smbParameters.ChallengeLength = packet.SmbParameters.ChallengeLength;
            this.smbData.ByteCount = packet.SmbData.ByteCount;

            if (packet.smbData.Challenge != null)
            {
                this.smbData.Challenge = new byte[packet.smbData.Challenge.Length];
                Array.Copy(packet.smbData.Challenge, this.smbData.Challenge, packet.smbData.Challenge.Length);
            }
            else
            {
                this.smbData.Challenge = new byte[0];
            }

            if (packet.smbData.DomainName != null)
            {
                this.smbData.DomainName = new byte[packet.smbData.DomainName.Length];
                Array.Copy(packet.smbData.DomainName, this.smbData.DomainName, packet.smbData.DomainName.Length);
            }
            else
            {
                this.smbData.DomainName = new byte[0];
            }
        }

        #endregion


        #region override methods

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this.</returns>
        public override StackPacket Clone()
        {
            return new SmbNegotiateResponsePacket(this);
        }


        /// <summary>
        /// Encode the struct of SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Parameters
        /// into the struct of SmbParameters
        /// </summary>
        protected override void EncodeParameters()
        {
            if (this.smbParameters.WordCount == WordCountOfCoreProtocol)
            {
                this.smbParametersBlock.WordCount = WordCountOfCoreProtocol;
                this.smbParametersBlock.Words = new ushort[WordCountOfCoreProtocol] { this.smbParameters.DialectIndex };
            }
            else
            {
                this.smbParametersBlock = TypeMarshal.ToStruct<SmbParameters>(
                    CifsMessageUtils.ToBytes<SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Parameters>(this.smbParameters));
            }
        }


        /// <summary>
        /// Encode the struct of SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Data into the struct of SmbData
        /// </summary>
        protected override void EncodeData()
        {
            this.smbDataBlock.ByteCount = this.SmbData.ByteCount;
            this.smbDataBlock.Bytes = new byte[this.smbDataBlock.ByteCount];
            using (MemoryStream memoryStream = new MemoryStream(this.smbDataBlock.Bytes))
            {
                using (Channel channel = new Channel(null, memoryStream))
                {
                    channel.BeginWriteGroup();
                    if (this.smbData.Challenge != null)
                    {
                        channel.WriteBytes(this.smbData.Challenge);
                    }
                    if (this.smbData.DomainName != null)
                    {
                        channel.WriteBytes(this.smbData.DomainName);
                    }
                    channel.EndWriteGroup();
                }
            }
        }


        /// <summary>
        /// to decode the smb parameters: from the general SmbParameters to the concrete Smb Parameters.
        /// </summary>
        protected override void DecodeParameters()
        {
            if (this.smbParametersBlock.WordCount == WordCountOfCoreProtocol)
            {
                this.smbParameters.WordCount = this.smbParametersBlock.WordCount;
                using (MemoryStream memoryStream = new MemoryStream(CifsMessageUtils.ToBytesArray(this.SmbParametersBlock.Words)))
                {
                    using (Channel channel = new Channel(null, memoryStream))
                    {
                        this.smbParameters.DialectIndex = channel.Read<ushort>();
                    }
                }
            }
            else if (this.smbParametersBlock.WordCount == WordCountOfNtLanManager)
            {
                this.smbParameters = TypeMarshal.ToStruct<SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Parameters>(
                    CifsMessageUtils.ToBytes<SmbParameters>(this.smbParametersBlock));
            }
            else if (this.smbParametersBlock.WordCount == WordCountOfLanManager)
            {
                throw new NotImplementedException("TD did not define the SmbParameters for Lan Manager Dialect.");
            }
            else
            {
                throw new NotSupportedException("Unknow SmbParameters structure.");
            }
        }


        /// <summary>
        /// to decode the smb data: from the general SmbDada to the concrete Smb Data.
        /// </summary>
        protected override void DecodeData()
        {
            this.smbData.ByteCount = this.smbDataBlock.ByteCount;

            if (this.smbParametersBlock.WordCount == WordCountOfNtLanManager
                || this.smbParametersBlock.WordCount == WordCountOfCoreProtocol)
            {
                using (MemoryStream memoryStream = new MemoryStream(this.smbDataBlock.Bytes))
                {
                    using (Channel channel = new Channel(null, memoryStream))
                    {
                        this.smbData.Challenge = channel.ReadBytes(this.SmbParameters.ChallengeLength);
                        this.smbData.DomainName = channel.ReadBytes(this.smbData.ByteCount
                            - this.SmbParameters.ChallengeLength);
                    }
                }
            }
            else if (this.smbParametersBlock.WordCount == WordCountOfLanManager)
            {
                throw new NotImplementedException("TD did not define the SmbParameters for Lan Manager Dialect.");
            }
        }

        #endregion


        #region initialize fields with default value

        /// <summary>
        /// init packet, set default field data
        /// </summary>
        private void InitDefaultValue()
        {
            this.smbData.Challenge = new byte[0];
            this.smbData.DomainName = new byte[0];
        }

        #endregion
    }
}
