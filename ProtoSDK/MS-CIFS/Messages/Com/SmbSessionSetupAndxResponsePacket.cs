// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    ///  Packets for SmbSessionSetupAndx Response
    /// </summary>
    public class SmbSessionSetupAndxResponsePacket : SmbBatchedResponsePacket
    {
        #region Fields

        private SMB_COM_SESSION_SETUP_ANDX_Response_SMB_Parameters smbParameters;
        private SMB_COM_SESSION_SETUP_ANDX_Response_SMB_Data smbData;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Smb_Parameters:SMB_COM_SESSION_SETUP_ANDX_Response_SMB_Parameters
        /// </summary>
        public SMB_COM_SESSION_SETUP_ANDX_Response_SMB_Parameters SmbParameters
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
        /// get or set the Smb_Data:SMB_COM_SESSION_SETUP_ANDX_Response_SMB_Data
        /// </summary>
        public SMB_COM_SESSION_SETUP_ANDX_Response_SMB_Data SmbData
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
        public SmbSessionSetupAndxResponsePacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbSessionSetupAndxResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbSessionSetupAndxResponsePacket(SmbSessionSetupAndxResponsePacket packet)
            : base(packet)
        {
            this.InitDefaultValue();
            this.smbParameters.WordCount = packet.SmbParameters.WordCount;
            this.smbParameters.AndXCommand = packet.SmbParameters.AndXCommand;
            this.smbParameters.AndXReserved = packet.SmbParameters.AndXReserved;
            this.smbParameters.AndXOffset = packet.SmbParameters.AndXOffset;
            this.smbParameters.Action = packet.SmbParameters.Action;
            this.smbData.ByteCount = packet.SmbData.ByteCount;

            if (packet.smbData.Pad != null)
            {
                this.smbData.Pad = new byte[packet.smbData.Pad.Length];
                Array.Copy(packet.smbData.Pad, this.smbData.Pad, packet.smbData.Pad.Length);
            }
            else
            {
                this.smbData.Pad = new byte[0];
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
        }

        #endregion


        #region override methods

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this.</returns>
        public override StackPacket Clone()
        {
            return new SmbSessionSetupAndxResponsePacket(this);
        }


        /// <summary>
        /// Encode the struct of SMB_COM_SESSION_SETUP_ANDX_Response_SMB_Parameters into the struct of SmbParameters
        /// </summary>
        protected override void EncodeParameters()
        {
            this.smbParametersBlock = TypeMarshal.ToStruct<SmbParameters>(
                CifsMessageUtils.ToBytes<SMB_COM_SESSION_SETUP_ANDX_Response_SMB_Parameters>(this.smbParameters));
        }


        /// <summary>
        /// Encode the struct of SMB_COM_SESSION_SETUP_ANDX_Response_SMB_Data into the struct of SmbData
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
                    if (this.smbData.Pad != null)
                    {
                        channel.WriteBytes(this.smbData.Pad);
                    }
                    if (this.smbData.NativeOS != null)
                    {
                        channel.WriteBytes(this.smbData.NativeOS);
                    }
                    if (this.smbData.NativeLanMan != null)
                    {
                        channel.WriteBytes(this.smbData.NativeLanMan);
                    }
                    if (this.smbData.PrimaryDomain != null)
                    {
                        channel.WriteBytes(this.smbData.PrimaryDomain);
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
            if (this.smbParametersBlock.WordCount > 0)
            {
                this.smbParameters = TypeMarshal.ToStruct<SMB_COM_SESSION_SETUP_ANDX_Response_SMB_Parameters>(
                    TypeMarshal.ToBytes(this.smbParametersBlock));
            }
        }


        /// <summary>
        /// to decode the smb data: from the general SmbDada to the concrete Smb Data.
        /// </summary>
        protected override void DecodeData()
        {
            this.smbData.ByteCount = this.smbDataBlock.ByteCount;
            int padLen = 0;

            for (int i = 0; i < this.smbDataBlock.Bytes.Length; i++)
            {
                if (this.smbDataBlock.Bytes[i] != 0)
                {
                    break;
                }
                padLen++;
            }
            this.smbData.Pad = new byte[padLen];
            int nulIndex = padLen;

            for (int i = 0; i < 3; i++)
            {
                int beginIndex = nulIndex;

                if ((this.SmbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE)
                {
                    for (int j = nulIndex; j < this.smbDataBlock.Bytes.Length - 1; j += 2)
                    {
                        nulIndex += 2;

                        if ((this.smbDataBlock.Bytes[j] == 0
                            && this.smbDataBlock.Bytes[j + 1] == 0)
                            || j == this.smbDataBlock.Bytes.Length - 2)
                        {
                            if (i == 0)
                            {
                                this.smbData.NativeOS = new byte[nulIndex - beginIndex];
                                Array.Copy(this.smbDataBlock.Bytes, beginIndex,
                                    this.smbData.NativeOS, 0, nulIndex - beginIndex);
                            }
                            else if (i == 1)
                            {
                                this.smbData.NativeLanMan = new byte[nulIndex - beginIndex];
                                Array.Copy(this.smbDataBlock.Bytes, beginIndex,
                                    this.smbData.NativeLanMan, 0, nulIndex - beginIndex);
                            }
                            else if (i == 2)
                            {
                                this.smbData.PrimaryDomain = new byte[nulIndex - beginIndex];
                                Array.Copy(this.smbDataBlock.Bytes, beginIndex,
                                    this.smbData.PrimaryDomain, 0, nulIndex - beginIndex);
                            }
                            break;
                        }
                    }
                }
                else
                {
                    for (int j = nulIndex; j < this.smbDataBlock.Bytes.Length; j++)
                    {
                        nulIndex++;

                        if (this.smbDataBlock.Bytes[j] == 0)
                        {
                            if (i == 0)
                            {
                                this.smbData.NativeOS = new byte[nulIndex - beginIndex];
                                Array.Copy(this.smbDataBlock.Bytes, beginIndex,
                                    this.smbData.NativeOS, 0, nulIndex - beginIndex);
                            }
                            else if (i == 1)
                            {
                                this.smbData.NativeLanMan = new byte[nulIndex - beginIndex];
                                Array.Copy(this.smbDataBlock.Bytes, beginIndex,
                                    this.smbData.NativeLanMan, 0, nulIndex - beginIndex);
                            }
                            else if (i == 2)
                            {
                                this.smbData.PrimaryDomain = new byte[nulIndex - beginIndex];
                                Array.Copy(this.smbDataBlock.Bytes, beginIndex,
                                    this.smbData.PrimaryDomain, 0, nulIndex - beginIndex);
                            }
                            break;
                        }
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
            this.smbParameters.AndXCommand = SmbCommand.SMB_COM_NO_ANDX_COMMAND;
            this.smbData.NativeLanMan = new byte[0];
            this.smbData.NativeOS = new byte[0];
            this.smbData.Pad = new byte[0];
            this.smbData.PrimaryDomain = new byte[0];
        }

        #endregion
    }
}
