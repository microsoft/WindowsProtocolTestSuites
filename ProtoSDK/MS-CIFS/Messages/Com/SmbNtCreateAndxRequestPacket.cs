// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbNtCreateAndx Request
    /// </summary>
    public class SmbNtCreateAndxRequestPacket : SmbBatchedRequestPacket
    {
        #region Fields

        private SMB_COM_NT_CREATE_ANDX_Request_SMB_Parameters smbParameters;
        private SMB_COM_NT_CREATE_ANDX_Request_SMB_Data smbData;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Smb_Parameters:SMB_COM_NT_CREATE_ANDX_Request_SMB_Parameters
        /// </summary>
        public SMB_COM_NT_CREATE_ANDX_Request_SMB_Parameters SmbParameters
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
        /// get or set the Smb_Data:SMB_COM_NT_CREATE_ANDX_Request_SMB_Data
        /// </summary>
        public SMB_COM_NT_CREATE_ANDX_Request_SMB_Data SmbData
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
        public SmbNtCreateAndxRequestPacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbNtCreateAndxRequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbNtCreateAndxRequestPacket(SmbNtCreateAndxRequestPacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

            this.smbParameters.WordCount = packet.SmbParameters.WordCount;
            this.smbParameters.AndXCommand = packet.SmbParameters.AndXCommand;
            this.smbParameters.AndXReserved = packet.SmbParameters.AndXReserved;
            this.smbParameters.AndXOffset = packet.SmbParameters.AndXOffset;
            this.smbParameters.Reserved = packet.SmbParameters.Reserved;
            this.smbParameters.NameLength = packet.SmbParameters.NameLength;
            this.smbParameters.Flags = packet.SmbParameters.Flags;
            this.smbParameters.RootDirectoryFID = packet.SmbParameters.RootDirectoryFID;
            this.smbParameters.DesiredAccess = packet.SmbParameters.DesiredAccess;
            this.smbParameters.AllocationSize = packet.SmbParameters.AllocationSize;
            this.smbParameters.ExtFileAttributes = packet.SmbParameters.ExtFileAttributes;
            this.smbParameters.ShareAccess = packet.SmbParameters.ShareAccess;
            this.smbParameters.CreateDisposition = packet.SmbParameters.CreateDisposition;
            this.smbParameters.CreateOptions = packet.SmbParameters.CreateOptions;
            this.smbParameters.ImpersonationLevel = packet.SmbParameters.ImpersonationLevel;
            this.smbParameters.SecurityFlags = packet.smbParameters.SecurityFlags;
            this.smbData.ByteCount = packet.SmbData.ByteCount;
            this.smbData.Pad = packet.smbData.Pad;

            if (packet.smbData.FileName != null)
            {
                this.smbData.FileName = new byte[packet.smbData.FileName.Length];
                Array.Copy(packet.smbData.FileName, this.smbData.FileName, packet.smbData.FileName.Length);
            }
            else
            {
                this.smbData.FileName = new byte[0];
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
            return new SmbNtCreateAndxRequestPacket(this);
        }


        /// <summary>
        /// Encode the struct of SMB_COM_NT_CREATE_ANDX_Request_SMB_Parameters into the struct of SmbParameters
        /// </summary>
        protected override void EncodeParameters()
        {
            this.smbParametersBlock = TypeMarshal.ToStruct<SmbParameters>(
                CifsMessageUtils.ToBytes<SMB_COM_NT_CREATE_ANDX_Request_SMB_Parameters>(this.smbParameters));
        }


        /// <summary>
        /// Encode the struct of SMB_COM_NT_CREATE_ANDX_Request_SMB_Data into the struct of SmbData
        /// </summary>
        protected override void EncodeData()
        {
            byte[] buf = ArrayUtility.ConcatenateArrays(
                BitConverter.GetBytes(this.SmbData.ByteCount),
                this.SmbData.Pad,
                this.SmbData.FileName);
            this.smbDataBlock = TypeMarshal.ToStruct<SmbData>(buf);
        }


        /// <summary>
        /// to decode the smb parameters: from the general SmbParameters to the concrete Smb Parameters.
        /// </summary>
        protected override void DecodeParameters()
        {
            this.smbParameters = TypeMarshal.ToStruct<SMB_COM_NT_CREATE_ANDX_Request_SMB_Parameters>(
                TypeMarshal.ToBytes(this.smbParametersBlock));
        }


        /// <summary>
        /// to decode the smb data: from the general SmbDada to the concrete Smb Data.
        /// </summary>
        protected override void DecodeData()
        {
            this.smbData.ByteCount = this.smbDataBlock.ByteCount;
            if ((this.smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == 0)
            {
                this.smbData.Pad = new byte[0];
                this.smbData.FileName = this.smbDataBlock.Bytes;
            }
            else
            {
                this.smbData.Pad = new byte[] { this.smbDataBlock.Bytes[0] };
                this.smbData.FileName = ArrayUtility.SubArray(this.smbDataBlock.Bytes, 1);
            }
        }

        #endregion


        #region initialize fields with default value

        /// <summary>
        /// init packet, set default field data
        /// </summary>
        private void InitDefaultValue()
        {
            if ((this.smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == 0)
            {
                this.smbData.Pad = new byte[0];
            }
            else
            {
                this.smbData.Pad = new byte[1];
            }
            this.smbData.FileName = new byte[0];
        }

        #endregion
    }
}
