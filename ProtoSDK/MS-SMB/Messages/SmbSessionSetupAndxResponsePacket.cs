// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// Packets for SmbSessionSetupAndx Response 
    /// </summary>
    public class SmbSessionSetupAndxResponsePacket : Cifs.SmbBatchedResponsePacket
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
            this.smbParameters.SecurityBlobLength = packet.smbParameters.SecurityBlobLength;
            this.smbData.ByteCount = packet.SmbData.ByteCount;

            if (packet.smbData.SecurityBlob != null)
            {
                this.smbData.SecurityBlob = new byte[packet.smbData.SecurityBlob.Length];
                Array.Copy(packet.smbData.SecurityBlob, this.smbData.SecurityBlob, packet.smbData.SecurityBlob.Length);
            }
            else
            {
                this.smbData.SecurityBlob = new byte[0];
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
        /// <returns>a new Packet cloned from this. </returns>
        public override StackPacket Clone()
        {
            return new SmbSessionSetupAndxResponsePacket(this);
        }


        /// <summary>
        /// Encode the struct of SMB_COM_SESSION_SETUP_ANDX_Response_SMB_Parameters into the struct of SmbParameters 
        /// </summary>
        protected override void EncodeParameters()
        {
            this.smbParametersBlock = CifsMessageUtils.ToStuct<SmbParameters>(
                CifsMessageUtils.ToBytes<SMB_COM_SESSION_SETUP_ANDX_Response_SMB_Parameters>(this.smbParameters));
        }


        /// <summary>
        /// Encode the struct of SMB_COM_SESSION_SETUP_ANDX_Response_SMB_Data into the struct of SmbData 
        /// </summary>
        protected override void EncodeData()
        {
            this.smbDataBlock.ByteCount = this.SmbData.ByteCount;

            List<byte> bytes = new List<byte>();

            if (this.smbData.SecurityBlob != null)
            {
                bytes.AddRange(this.smbData.SecurityBlob);
            }
            if (this.smbData.Pad != null)
            {
                bytes.AddRange(this.smbData.Pad);
            }
            if (this.smbData.NativeOS != null)
            {
                bytes.AddRange(this.smbData.NativeOS);
            }
            if (this.smbData.NativeLanMan != null)
            {
                bytes.AddRange(this.smbData.NativeLanMan);
            }
            if (this.smbData.PrimaryDomain != null)
            {
                bytes.AddRange(this.smbData.PrimaryDomain);
            }

            this.smbDataBlock.Bytes = bytes.ToArray();
        }


        /// <summary>
        /// to decode the smb parameters: from the general SmbParameters to the concrete Smb Parameters. 
        /// </summary>
        protected override void DecodeParameters()
        {
            if (this.SmbParametersBlock.WordCount > 0)
            {
                this.smbParameters = CifsMessageUtils.ToStuct<SMB_COM_SESSION_SETUP_ANDX_Response_SMB_Parameters>(
                    CifsMessageUtils.ToBytes<SmbParameters>(this.smbParametersBlock));
            }
            else
            {
                this.smbParameters.WordCount = this.SmbParametersBlock.WordCount;
            }
        }


        /// <summary>
        /// to decode the smb data: from the general SmbDada to the concrete Smb Data. 
        /// </summary>
        protected override void DecodeData()
        {
            if (this.SmbDataBlock.ByteCount > 0)
            {
                //security blob
                this.smbData.SecurityBlob = new byte[this.smbParameters.SecurityBlobLength];
                Array.Copy(this.smbDataBlock.Bytes, this.smbData.SecurityBlob, this.smbData.SecurityBlob.Length);
                
                this.smbData.ByteCount = this.smbDataBlock.ByteCount;
                int padLen = 0;

                for (int i = this.smbParameters.SecurityBlobLength; i < this.smbDataBlock.Bytes.Length; i++)
                {
                    if (this.smbDataBlock.Bytes[i] != 0)
                    {
                        break;
                    }
                    padLen++;
                }
                this.smbData.Pad = new byte[padLen];
                int nulIndex = padLen + this.smbParameters.SecurityBlobLength;

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
            else
            {
                this.smbData.ByteCount = this.SmbDataBlock.ByteCount;
            }
        }


        #endregion


        #region initialize fields with default value

        /// <summary>
        /// init packet, set default field data 
        /// </summary>
        private void InitDefaultValue()
        {
            this.smbParameters.WordCount = 0x04;
            this.smbParameters.AndXCommand = SmbCommand.SMB_COM_NO_ANDX_COMMAND;

            this.smbData.NativeLanMan = new byte[0];
            this.smbData.NativeOS = new byte[0];
            this.smbData.Pad = new byte[0];
            this.smbData.PrimaryDomain = new byte[0];
        }


        #endregion


        #region methods on packet

        /// <summary>
        /// access the security blob of smb data. 
        /// </summary>
        internal byte[] SecurityBlob
        {
            get
            {
                return this.smbData.SecurityBlob;
            }
            set
            {
                this.smbData.SecurityBlob = value;
            }
        }


        #endregion
    }
}
