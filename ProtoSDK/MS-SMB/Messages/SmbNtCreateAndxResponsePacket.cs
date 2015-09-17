// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.Protocols.TestTools.StackSdk.Messages;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// Packets for SmbNtCreateAndx Response
    /// </summary>
    public class SmbNtCreateAndxResponsePacket : Cifs.SmbNtCreateAndxResponsePacket
    {
        #region Const

        /// <summary>
        /// in TD: WordCount, This field SHOULD be 0x32.<para/>
        /// and windows feature: Windows-based servers set this field to 0x2A.<para/>
        /// so, if there is 16(0x32 - 0x2A) bytes more in channel, need to unmarshal the additional data.
        /// </summary>
        private const int WINDOWS_BEHAVIOR_ADDITIONAL_DATA_LENGTH = 16;

        /// <summary>
        /// The word count for this response MUST be 0x2A (42). WordCount in this case is not used as the count of 
        /// parameter words but is just a number.
        /// </summary>
        private const int CREATE_EXTENDED_INFORMATION_RESPONSE_LENGTH = 42;

        #endregion

        #region Fields

        private SMB_COM_NT_CREATE_ANDX_Response_SMB_Parameters smbParameters;

        #endregion

        #region Properties

        /// <summary>
        /// get or set the Smb_Parameters:SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters
        /// </summary>
        public new SMB_COM_NT_CREATE_ANDX_Response_SMB_Parameters SmbParameters
        {
            get
            {
                return this.smbParameters;
            }
            set
            {
                this.smbParameters = value;

                base.SmbParameters = SmbMessageUtils.ConvertSmbComCreatePacketPayload(this.smbParameters);
            }
        }


        #endregion

        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbNtCreateAndxResponsePacket(Cifs.SmbNtCreateAndxResponsePacket packet)
            : base(packet)
        {
            this.smbParameters = SmbMessageUtils.ConvertSmbComCreatePacketPayload(base.SmbParameters);
        }


        #endregion

        #region override methods

        /// <summary>
        /// to unmarshal the SmbParameters struct from a channel.
        /// </summary>
        /// <param name="channel">the channel started with SmbParameters.</param>
        /// <returns>the size in bytes of the SmbParameters.</returns>
        protected override int ReadParametersFromChannel(Channel channel)
        {
            this.smbParametersBlock = channel.Read<SmbParameters>();

            if (channel.Stream.Position <= channel.Stream.Length - WINDOWS_BEHAVIOR_ADDITIONAL_DATA_LENGTH)
            {
                byte[] data = CifsMessageUtils.ToBytesArray<ushort>(this.smbParametersBlock.Words);

                this.smbParametersBlock.Words = CifsMessageUtils.ToTypeArray<ushort>(
                    ArrayUtility.ConcatenateArrays<byte>(
                        data, channel.ReadBytes(WINDOWS_BEHAVIOR_ADDITIONAL_DATA_LENGTH)));
            }

            this.DecodeParameters();

            int sizeOfWordCount = sizeof(byte);
            int sizeOfWords = this.smbParametersBlock.WordCount * sizeof(ushort);

            return sizeOfWordCount + sizeOfWords;
        }


        /// <summary>
        /// Encode the struct of SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters into the struct of SmbParameters
        /// </summary>
        protected override void EncodeParameters()
        {
            List<byte> bytes = new List<byte>();

            bytes.AddRange(TypeMarshal.ToBytes<SmbCommand>(this.smbParameters.AndXCommand));
            bytes.AddRange(TypeMarshal.ToBytes<byte>(this.smbParameters.AndXReserved));
            bytes.AddRange(TypeMarshal.ToBytes<ushort>(this.smbParameters.AndXOffset));
            bytes.AddRange(TypeMarshal.ToBytes<OplockLevelValue>(this.smbParameters.OplockLevel));
            bytes.AddRange(TypeMarshal.ToBytes<ushort>(this.smbParameters.FID));
            bytes.AddRange(TypeMarshal.ToBytes<uint>(this.smbParameters.CreationAction));
            bytes.AddRange(TypeMarshal.ToBytes<Cifs.FileTime>(this.smbParameters.CreateTime));
            bytes.AddRange(TypeMarshal.ToBytes<Cifs.FileTime>(this.smbParameters.LastAccessTime));
            bytes.AddRange(TypeMarshal.ToBytes<Cifs.FileTime>(this.smbParameters.LastWriteTime));
            bytes.AddRange(TypeMarshal.ToBytes<Cifs.FileTime>(this.smbParameters.LastChangeTime));
            bytes.AddRange(TypeMarshal.ToBytes<uint>(this.smbParameters.ExtFileAttributes));
            bytes.AddRange(TypeMarshal.ToBytes<ulong>(this.smbParameters.AllocationSize));
            bytes.AddRange(TypeMarshal.ToBytes<ulong>(this.smbParameters.EndOfFile));
            bytes.AddRange(TypeMarshal.ToBytes<FileTypeValue>(this.smbParameters.ResourceType));
            bytes.AddRange(TypeMarshal.ToBytes<SMB_NMPIPE_STATUS>(this.smbParameters.NMPipeStatus_or_FileStatusFlags));
            bytes.AddRange(TypeMarshal.ToBytes<byte>(this.smbParameters.Directory));

            if (this.smbParameters.VolumeGUID != null)
            {
                bytes.AddRange(this.smbParameters.VolumeGUID);
            }
            if (this.smbParameters.FileId != null)
            {
                bytes.AddRange(this.smbParameters.FileId);
            }
            if (this.smbParameters.MaximalAccessRights != null)
            {
                bytes.AddRange(this.smbParameters.MaximalAccessRights);
            }
            if (this.smbParameters.GuestMaximalAccessRights != null)
            {
                bytes.AddRange(this.smbParameters.GuestMaximalAccessRights);
            }

            this.smbParametersBlock.WordCount = (byte)this.smbParameters.WordCount;
            this.smbParametersBlock.Words = CifsMessageUtils.ToTypeArray<ushort>(bytes.ToArray());
        }


        /// <summary>
        /// to decode the smb parameters: from the general SmbParameters to the concrete Smb Parameters.
        /// </summary>
        protected override void DecodeParameters()
        {
            // When a client requests extended information, the word count must be 42
            if (this.smbParametersBlock.WordCount == CREATE_EXTENDED_INFORMATION_RESPONSE_LENGTH)
            {
                SMB_COM_NT_CREATE_ANDX_Response_SMB_Parameters param =
                    new SMB_COM_NT_CREATE_ANDX_Response_SMB_Parameters();

                using (MemoryStream stream = new MemoryStream(
                    CifsMessageUtils.ToBytesArray<ushort>(this.smbParametersBlock.Words)))
                {
                    using (Channel channel = new Channel(null, stream))
                    {
                        param.WordCount = this.smbParametersBlock.WordCount;
                        param.AndXCommand = channel.Read<SmbCommand>();
                        param.AndXReserved = channel.Read<byte>();
                        param.AndXOffset = channel.Read<ushort>();
                        param.OplockLevel = channel.Read<OplockLevelValue>();
                        param.FID = channel.Read<ushort>();
                        param.CreationAction = channel.Read<uint>();
                        param.CreateTime = channel.Read<Cifs.FileTime>();
                        param.LastAccessTime = channel.Read<Cifs.FileTime>();
                        param.LastWriteTime = channel.Read<Cifs.FileTime>();
                        param.LastChangeTime = channel.Read<Cifs.FileTime>();
                        param.ExtFileAttributes = channel.Read<uint>();
                        param.AllocationSize = channel.Read<ulong>();
                        param.EndOfFile = channel.Read<ulong>();
                        param.ResourceType = channel.Read<FileTypeValue>();
                        param.NMPipeStatus_or_FileStatusFlags = channel.Read<SMB_NMPIPE_STATUS>();
                        param.Directory = channel.Read<byte>();
                        // VolumeGUID (16 bytes), td defines this length
                        param.VolumeGUID = channel.ReadBytes(CifsMessageUtils.GetSize<Guid>(new Guid()));
                        // if there is more 16 bytes in the channel.
                        if (channel.Stream.Position <= channel.Stream.Length - WINDOWS_BEHAVIOR_ADDITIONAL_DATA_LENGTH)
                        {
                            // FileId (8 bytes), td defines this length
                            param.FileId = channel.ReadBytes(sizeof(ulong));
                            // MaximalAccessRights (4 bytes), td defines this length
                            param.MaximalAccessRights = channel.ReadBytes(sizeof(uint));
                            // GuestMaximalAccessRights (4 bytes), td defines this length
                            param.GuestMaximalAccessRights = channel.ReadBytes(sizeof(uint));
                        }
                    }
                }

                this.SmbParameters = param;
            }
            else
            {
                base.DecodeParameters();

                this.smbParameters = SmbMessageUtils.ConvertSmbComCreatePacketPayload(base.SmbParameters);
            }
        }


        /// <summary>
        /// override the marshal method of smb parameter.<para/>
        /// in the create response, the WordCount is not equal to Words.Count.
        /// </summary>
        /// <param name="channel">
        /// a Channel to write data to.
        /// </param>
        protected override void WriteSmbParameter(Channel channel)
        {
            channel.Write<byte>((byte)CREATE_EXTENDED_INFORMATION_RESPONSE_LENGTH);
            channel.WriteBytes(Cifs.CifsMessageUtils.ToBytesArray<ushort>(this.smbParametersBlock.Words));
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbNtCreateAndxResponsePacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbNtCreateAndxResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbNtCreateAndxResponsePacket(SmbNtCreateAndxResponsePacket packet)
            : base(packet)
        {
        }


        #endregion
    }
}
