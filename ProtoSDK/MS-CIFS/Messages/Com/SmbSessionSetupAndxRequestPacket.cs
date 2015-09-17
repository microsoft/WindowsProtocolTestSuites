// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbSessionSetupAndx Request
    /// </summary>
    public class SmbSessionSetupAndxRequestPacket : SmbBatchedRequestPacket
    {
        #region Fields

        private SMB_COM_SESSION_SETUP_ANDX_Request_SMB_Parameters smbParameters;
        private SMB_COM_SESSION_SETUP_ANDX_Request_SMB_Data smbData;
        private byte[] implicitNtlmSessionKey;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Smb_Parameters:SMB_COM_SESSION_SETUP_ANDX_Request_SMB_Parameters
        /// </summary>
        public SMB_COM_SESSION_SETUP_ANDX_Request_SMB_Parameters SmbParameters
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
        /// get or set the Smb_Data:SMB_COM_SESSION_SETUP_ANDX_Request_SMB_Data
        /// </summary>
        public SMB_COM_SESSION_SETUP_ANDX_Request_SMB_Data SmbData
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
        /// the SessionKey for the implicit NTLM.
        /// </summary>
        public byte[] ImplicitNtlmSessionKey
        {
            get
            {
                return this.implicitNtlmSessionKey;
            }
            set
            {
                this.implicitNtlmSessionKey = value;
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
        public SmbSessionSetupAndxRequestPacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbSessionSetupAndxRequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbSessionSetupAndxRequestPacket(SmbSessionSetupAndxRequestPacket packet)
            : base(packet)
        {
            this.InitDefaultValue();
            if (packet.ImplicitNtlmSessionKey != null)
            {
                this.implicitNtlmSessionKey = new byte[packet.ImplicitNtlmSessionKey.Length];
                Array.Copy(packet.ImplicitNtlmSessionKey, this.implicitNtlmSessionKey,
                    this.implicitNtlmSessionKey.Length);
            }
            this.smbParameters.WordCount = packet.SmbParameters.WordCount;
            this.smbParameters.AndXCommand = packet.SmbParameters.AndXCommand;
            this.smbParameters.AndXReserved = packet.SmbParameters.AndXReserved;
            this.smbParameters.AndXOffset = packet.SmbParameters.AndXOffset;
            this.smbParameters.MaxBufferSize = packet.SmbParameters.MaxBufferSize;
            this.smbParameters.MaxMpxCount = packet.SmbParameters.MaxMpxCount;
            this.smbParameters.VcNumber = packet.SmbParameters.VcNumber;
            this.smbParameters.SessionKey = packet.SmbParameters.SessionKey;
            this.smbParameters.OEMPasswordLen = packet.SmbParameters.OEMPasswordLen;
            this.smbParameters.UnicodePasswordLen = packet.SmbParameters.UnicodePasswordLen;
            this.smbParameters.Reserved = packet.SmbParameters.Reserved;
            this.smbParameters.Capabilities = packet.SmbParameters.Capabilities;

            this.smbData.ByteCount = packet.SmbData.ByteCount;

            if (packet.smbData.OEMPassword != null)
            {
                this.smbData.OEMPassword = new byte[packet.smbData.OEMPassword.Length];
                Array.Copy(packet.smbData.OEMPassword, this.smbData.OEMPassword, packet.smbData.OEMPassword.Length);
            }
            else
            {
                this.smbData.OEMPassword = new byte[0];
            }

            if (packet.smbData.UnicodePassword != null)
            {
                this.smbData.UnicodePassword = new byte[packet.smbData.UnicodePassword.Length];
                Array.Copy(packet.smbData.UnicodePassword,
                    this.smbData.UnicodePassword, packet.smbData.UnicodePassword.Length);
            }
            else
            {
                this.smbData.UnicodePassword = new byte[0];
            }

            if (packet.smbData.Pad != null)
            {
                this.smbData.Pad = new byte[packet.smbData.Pad.Length];
                Array.Copy(packet.smbData.Pad, this.smbData.Pad, packet.smbData.Pad.Length);
            }
            else
            {
                this.smbData.Pad = new byte[0];
            }

            if (packet.smbData.AccountName != null)
            {
                this.smbData.AccountName = new byte[packet.smbData.AccountName.Length];
                Array.Copy(packet.smbData.AccountName, this.smbData.AccountName, packet.smbData.AccountName.Length);
            }
            else
            {
                this.smbData.AccountName = new byte[0];
            }

            if (packet.smbData.PrimaryDomain != null)
            {
                this.smbData.PrimaryDomain = new byte[packet.smbData.PrimaryDomain.Length];
                Array.Copy(packet.smbData.PrimaryDomain,
                    this.smbData.PrimaryDomain, packet.smbData.PrimaryDomain.Length);
            }
            else
            {
                this.smbData.PrimaryDomain = new byte[0];
            }

            if (packet.smbData.NativeOS != null)
            {
                this.smbData.NativeOS = new byte[packet.smbData.NativeOS.Length];
                Array.Copy(packet.smbData.NativeOS, this.smbData.NativeOS, packet.smbData.NativeOS.Length);
            }
            else
            {
                this.smbData.NativeOS = new byte[0];
            }

            if (packet.smbData.NativeLanMan != null)
            {
                this.smbData.NativeLanMan = new byte[packet.smbData.NativeLanMan.Length];
                Array.Copy(packet.smbData.NativeLanMan, this.smbData.NativeLanMan, packet.smbData.NativeLanMan.Length);
            }
            else
            {
                this.smbData.NativeLanMan = new byte[0];
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
            return new SmbSessionSetupAndxRequestPacket(this);
        }


        /// <summary>
        /// Encode the struct of SMB_COM_SESSION_SETUP_ANDX_Request_SMB_Parameters into the struct of SmbParameters
        /// </summary>
        protected override void EncodeParameters()
        {
            this.smbParametersBlock = CifsMessageUtils.ToStuct<SmbParameters>(
                CifsMessageUtils.ToBytes<SMB_COM_SESSION_SETUP_ANDX_Request_SMB_Parameters>(this.smbParameters));
        }


        /// <summary>
        /// Encode the struct of SMB_COM_SESSION_SETUP_ANDX_Request_SMB_Data into the struct of SmbData
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
                    if (this.SmbData.OEMPassword != null)
                    {
                        channel.WriteBytes(this.SmbData.OEMPassword);
                    }
                    if (this.SmbData.UnicodePassword != null)
                    {
                        channel.WriteBytes(this.SmbData.UnicodePassword);
                    }
                    if (this.SmbData.Pad != null)
                    {
                        channel.WriteBytes(this.SmbData.Pad);
                    }
                    if (this.SmbData.AccountName != null)
                    {
                        channel.WriteBytes(this.SmbData.AccountName);
                    }
                    if (this.SmbData.PrimaryDomain != null)
                    {
                        channel.WriteBytes(this.SmbData.PrimaryDomain);
                    }
                    if (this.SmbData.NativeOS != null)
                    {
                        channel.WriteBytes(this.SmbData.NativeOS);
                    }
                    if (this.SmbData.NativeLanMan != null)
                    {
                        channel.WriteBytes(this.SmbData.NativeLanMan);
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
            this.smbParameters = TypeMarshal.ToStruct<SMB_COM_SESSION_SETUP_ANDX_Request_SMB_Parameters>(
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
                    this.smbData.OEMPassword = channel.ReadBytes(this.SmbParameters.OEMPasswordLen);
                    this.smbData.UnicodePassword = channel.ReadBytes(this.SmbParameters.UnicodePasswordLen);
                    
                    bool isUnicode = (this.smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE;
                    // pad:
                    if (isUnicode == true
                        && (Marshal.SizeOf(this.SmbHeader) + Marshal.SizeOf(this.SmbParameters)
                            + Marshal.SizeOf(this.smbData.ByteCount) + this.SmbParameters.OEMPasswordLen
                            + this.SmbParameters.UnicodePasswordLen) % 2 != 0)
                    {
                        this.smbData.Pad = channel.ReadBytes(1);
                    }
                    else
                    {
                        this.smbData.Pad = new byte[0];
                    }
                    
                    this.smbData.AccountName = CifsMessageUtils.ReadNullTerminatedString(channel, isUnicode);
                    this.smbData.PrimaryDomain = CifsMessageUtils.ReadNullTerminatedString(channel, isUnicode);
                    this.smbData.NativeOS = CifsMessageUtils.ReadNullTerminatedString(channel, isUnicode);
                    this.smbData.NativeLanMan = CifsMessageUtils.ReadNullTerminatedString(channel, isUnicode);
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
            this.smbData.AccountName = new byte[0];
            this.smbData.NativeLanMan = new byte[0];
            this.smbData.NativeOS = new byte[0];
            this.smbData.OEMPassword = new byte[0];
            this.smbData.Pad = new byte[0];
            this.smbData.PrimaryDomain = new byte[0];
            this.smbData.UnicodePassword = new byte[0];
        }

        #endregion
    }
}
