// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbSearch Request
    /// </summary>
    public class SmbSearchRequestPacket : SmbSingleRequestPacket
    {
        #region Fields

        private SMB_COM_SEARCH_Request_SMB_Parameters smbParameters;
        private SMB_COM_SEARCH_Request_SMB_Data smbData;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Smb_Parameters:SMB_COM_SEARCH_Request_SMB_Parameters
        /// </summary>
        public SMB_COM_SEARCH_Request_SMB_Parameters SmbParameters
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
        /// get or set the Smb_Data:SMB_COM_SEARCH_Request_SMB_Data
        /// </summary>
        public SMB_COM_SEARCH_Request_SMB_Data SmbData
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
        public SmbSearchRequestPacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbSearchRequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbSearchRequestPacket(SmbSearchRequestPacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

            this.smbParameters.WordCount = packet.SmbParameters.WordCount;
            this.smbParameters.MaxCount = packet.SmbParameters.MaxCount;
            this.smbParameters.SearchAttributes = packet.SmbParameters.SearchAttributes;

            this.smbData.ByteCount = packet.SmbData.ByteCount;
            this.smbData.BufferFormat1 = packet.SmbData.BufferFormat1;
            this.smbData.FileName = new byte[packet.smbData.FileName.Length];
            Array.Copy(packet.smbData.FileName, this.smbData.FileName, packet.smbData.FileName.Length);
            this.smbData.BufferFormat2 = packet.SmbData.BufferFormat2;
            this.smbData.ResumeKeyLength = packet.SmbData.ResumeKeyLength;

            if (packet.smbData.ResumeKey != null)
            {
                this.smbData.ResumeKey = new SMB_Resume_Key[packet.smbData.ResumeKey.Length];

                for (int i = 0; i < packet.smbData.ResumeKey.Length; i++)
                {
                    this.smbData.ResumeKey[i] = new SMB_Resume_Key();
                    this.smbData.ResumeKey[i].Reserved = packet.SmbData.ResumeKey[i].Reserved;
                    this.smbData.ResumeKey[i].ServerState = new byte[packet.smbData.ResumeKey[i].ServerState.Length];
                    this.smbData.ResumeKey[i].ClientState = new byte[packet.smbData.ResumeKey[i].ClientState.Length];
                    Array.Copy(packet.smbData.ResumeKey[i].ServerState,
                        this.smbData.ResumeKey[i].ServerState, packet.smbData.ResumeKey[i].ServerState.Length);
                    Array.Copy(packet.smbData.ResumeKey[i].ClientState,
                        this.smbData.ResumeKey[i].ClientState, packet.smbData.ResumeKey[i].ClientState.Length);
                }
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
            return new SmbSearchRequestPacket(this);
        }


        /// <summary>
        /// Encode the struct of SMB_COM_SEARCH_Request_SMB_Parameters into the struct of SmbParameters
        /// </summary>
        protected override void EncodeParameters()
        {
            this.smbParametersBlock = TypeMarshal.ToStruct<SmbParameters>(
                CifsMessageUtils.ToBytes<SMB_COM_SEARCH_Request_SMB_Parameters>(this.smbParameters));
        }


        /// <summary>
        /// Encode the struct of SMB_COM_SEARCH_Request_SMB_Data into the struct of SmbData
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
                    channel.Write<byte>(this.smbData.BufferFormat1);

                    if (this.smbData.FileName != null)
                    {
                        channel.WriteBytes(this.smbData.FileName);
                    }
                    channel.Write<byte>(this.smbData.BufferFormat2);
                    channel.Write<ushort>(this.smbData.ResumeKeyLength);

                    if (this.smbData.ResumeKey != null)
                    {
                        foreach (SMB_Resume_Key resumeKey in this.smbData.ResumeKey)
                        {
                            channel.Write<SMB_Resume_Key>(resumeKey);
                        }
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
            this.smbParameters = TypeMarshal.ToStruct<SMB_COM_SEARCH_Request_SMB_Parameters>(
                TypeMarshal.ToBytes(this.smbParametersBlock));
        }


        /// <summary>
        /// to decode the smb data: from the general SmbDada to the concrete Smb Data.
        /// </summary>
        protected override void DecodeData()
        {
            using (MemoryStream memoryStream = new MemoryStream(CifsMessageUtils.ToBytes<SmbData>(this.smbDataBlock)))
            {
                using (Channel channel = new Channel(null, memoryStream))
                {
                    this.smbData.ByteCount = channel.Read<ushort>();
                    this.smbData.BufferFormat1 = channel.Read<byte>();
                    this.smbData.FileName = CifsMessageUtils.ReadNullTerminatedString(channel,
                        (this.smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);
                    this.smbData.BufferFormat2 = channel.Read<byte>();
                    this.smbData.ResumeKeyLength = channel.Read<ushort>();

                    if (this.smbData.ResumeKeyLength != 0)
                    {
                        this.smbData.ResumeKey = new SMB_Resume_Key[] { channel.Read<SMB_Resume_Key>() };
                    }
                    else
                    {
                        this.smbData.ResumeKey = new SMB_Resume_Key[0];
                    }
                }
            }
        }

        #endregion


        #region initialize fields with default value

        /// <summary>
        /// init packet, set default field data
        /// </summary>
        private void InitDefaultValue()
        {
            this.smbData.FileName = new byte[0];
            this.smbData.ResumeKey = new SMB_Resume_Key[0];
        }

        #endregion
    }
}
