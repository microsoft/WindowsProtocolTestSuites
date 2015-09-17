// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbNtCreateAndx Response
    /// </summary>
    public class SmbNtCreateAndxResponsePacket : SmbBatchedResponsePacket
    {
        #region Fields

        private SMB_COM_NT_CREATE_ANDX_Response_SMB_Parameters smbParameters;
        private SMB_COM_NT_CREATE_ANDX_Response_SMB_Data smbData;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Smb_Parameters:SMB_COM_NT_CREATE_ANDX_Response_SMB_Parameters
        /// </summary>
        public SMB_COM_NT_CREATE_ANDX_Response_SMB_Parameters SmbParameters
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
        /// get or set the Smb_Data:SMB_COM_NT_CREATE_ANDX_Response_SMB_Data
        /// </summary>
        public SMB_COM_NT_CREATE_ANDX_Response_SMB_Data SmbData
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


        /// <summary>
        /// the SmbCommand of the andx packet.
        /// </summary>
        protected override SmbCommand AndxCommand
        {
            get
            {
                return this.SmbParameters.AndXCommand;
            }
        }


        /// <summary>
        /// Set the AndXOffset from batched request
        /// </summary>
        protected override ushort AndXOffset
        {
            get
            {
                return this.smbParameters.AndXOffset;
            }
            set
            {
                this.smbParameters.AndXOffset = value;
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SmbNtCreateAndxResponsePacket()
            : base()
        {
            this.InitDefaultValue();
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
            this.InitDefaultValue();

            this.smbParameters.WordCount = packet.SmbParameters.WordCount;
            this.smbParameters.AndXCommand = packet.SmbParameters.AndXCommand;
            this.smbParameters.AndXReserved = packet.SmbParameters.AndXReserved;
            this.smbParameters.AndXOffset = packet.SmbParameters.AndXOffset;
            this.smbParameters.OplockLevel = packet.SmbParameters.OplockLevel;
            this.smbParameters.FID = packet.SmbParameters.FID;
            this.smbParameters.CreateDisposition = packet.SmbParameters.CreateDisposition;
            this.smbParameters.CreateTime = packet.SmbParameters.CreateTime;
            this.smbParameters.LastAccessTime = packet.SmbParameters.LastAccessTime;
            this.smbParameters.LastChangeTime = packet.SmbParameters.LastChangeTime;
            this.smbParameters.ExtFileAttributes = packet.SmbParameters.ExtFileAttributes;
            this.smbParameters.AllocationSize = packet.SmbParameters.AllocationSize;
            this.smbParameters.EndOfFile = packet.SmbParameters.EndOfFile;
            this.smbParameters.ResourceType = packet.SmbParameters.ResourceType;
            this.smbParameters.NMPipeStatus = packet.SmbParameters.NMPipeStatus;
            this.smbParameters.Directory = packet.SmbParameters.Directory;
            this.smbData.ByteCount = packet.SmbData.ByteCount;
        }

        #endregion


        #region override methods

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this.</returns>
        public override StackPacket Clone()
        {
            return new SmbNtCreateAndxResponsePacket(this);
        }


        /// <summary>
        /// Encode the struct of SMB_COM_NT_CREATE_ANDX_Response_SMB_Parameters into the struct of SmbParameters
        /// </summary>
        protected override void EncodeParameters()
        {
            this.smbParametersBlock = TypeMarshal.ToStruct<SmbParameters>(
                CifsMessageUtils.ToBytes<SMB_COM_NT_CREATE_ANDX_Response_SMB_Parameters>(this.smbParameters));
        }


        /// <summary>
        /// Encode the struct of SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Data into the struct of SmbData
        /// </summary>
        protected override void EncodeData()
        {
            this.smbDataBlock = TypeMarshal.ToStruct<SmbData>(
                CifsMessageUtils.ToBytes<SMB_COM_NT_CREATE_ANDX_Response_SMB_Data>(this.SmbData));
        }


        /// <summary>
        /// to decode the smb parameters: from the general SmbParameters to the concrete Smb Parameters.
        /// </summary>
        protected override void DecodeParameters()
        {
            if (this.smbParametersBlock.WordCount > 0)
            {
                this.smbParameters = TypeMarshal.ToStruct<SMB_COM_NT_CREATE_ANDX_Response_SMB_Parameters>(
                     TypeMarshal.ToBytes(this.smbParametersBlock));
            }
        }


        /// <summary>
        /// to decode the smb data: from the general SmbDada to the concrete Smb Data.
        /// </summary>
        protected override void DecodeData()
        {
            if (this.smbData.ByteCount > 0)
            {
                this.smbData = TypeMarshal.ToStruct<SMB_COM_NT_CREATE_ANDX_Response_SMB_Data>(
                     TypeMarshal.ToBytes(this.smbDataBlock));
            }
        }

        #endregion


        #region initialize fields with default value

        /// <summary>
        /// init packet, set default field data
        /// </summary>
        private void InitDefaultValue()
        {
            this.smbParameters.AndXCommand = SmbCommand.SMB_COM_NO_ANDX_COMMAND;
        }

        #endregion
    }
}
