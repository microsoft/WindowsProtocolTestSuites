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
    /// Packets for SmbNegotiate Response.  Do not support LM Negotiate response. 
    /// </summary>
    public class SmbNegotiateResponsePacket : Cifs.SmbSingleResponsePacket
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
            this.smbParameters.EncryptionKeyLength = packet.SmbParameters.EncryptionKeyLength;
            this.smbData.ByteCount = packet.SmbData.ByteCount;
            this.smbData.ServerGuid = packet.smbData.ServerGuid;

            if (packet.smbData.SecurityBlob != null)
            {
                this.smbData.SecurityBlob = new byte[packet.smbData.SecurityBlob.Length];
                Array.Copy(packet.smbData.SecurityBlob, this.smbData.SecurityBlob, packet.smbData.SecurityBlob.Length);
            }
            else
            {
                this.smbData.SecurityBlob = new byte[0];
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
            return new SmbNegotiateResponsePacket(this);
        }


        /// <summary>
        /// Encode the struct of SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Parameters into the struct of 
        /// SmbParameters 
        /// </summary>
        protected override void EncodeParameters()
        {
            this.smbParametersBlock = CifsMessageUtils.ToStuct<SmbParameters>(
                CifsMessageUtils.ToBytes<SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Parameters>(this.smbParameters));
        }


        /// <summary>
        /// Encode the struct of SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Data into the struct of SmbData 
        /// </summary>
        protected override void EncodeData()
        {
            this.smbDataBlock.ByteCount = this.SmbData.ByteCount;

            List<byte> bytes = new List<byte>();
            bytes.AddRange(TypeMarshal.ToBytes<Guid>(this.smbData.ServerGuid));
            if (this.smbData.SecurityBlob != null)
            {
                bytes.AddRange(this.smbData.SecurityBlob);
            }

            this.smbDataBlock.Bytes = bytes.ToArray();
        }


        /// <summary>
        /// to decode the smb parameters: from the general SmbParameters to the concrete Smb Parameters. 
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// TD did not define the SmbParameters for Lan Manager Dialect.
        /// </exception>
        /// <exception cref="NotSupportedException">Unknow SmbParameters structure.</exception>
        protected override void DecodeParameters()
        {
            if (this.SmbParametersBlock.WordCount > 0)
            {
                if (this.smbParametersBlock.WordCount == WordCountOfCoreProtocol)
                {
                    this.smbParameters.WordCount = this.smbParametersBlock.WordCount;
                    this.smbParameters.DialectIndex = this.SmbParametersBlock.Words[0];
                }
                else if (this.smbParametersBlock.WordCount == WordCountOfNtLanManager)
                {
                    this.smbParameters = CifsMessageUtils.ToStuct<SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Parameters>(
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
            else
            {
                this.smbParameters.WordCount = this.SmbParametersBlock.WordCount;
            }
        }


        /// <summary>
        /// to decode the smb data: from the general SmbDada to the concrete Smb Data. 
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// TD did not define the SmbParameters for Lan Manager Dialect.
        /// </exception>
        /// <exception cref="NotSupportedException">Unknow SmbParameters structure.</exception>
        protected override void DecodeData()
        {
            if (this.SmbDataBlock.ByteCount > 0)
            {
                this.smbData.ByteCount = this.smbDataBlock.ByteCount;

                if (this.smbParametersBlock.WordCount == WordCountOfNtLanManager)
                {
                    using (MemoryStream stream = new MemoryStream(this.smbDataBlock.Bytes))
                    {
                        using (Channel channel = new Channel(null, stream))
                        {
                            this.smbData.ServerGuid = channel.Read<Guid>();
                            this.smbData.SecurityBlob = channel.ReadBytes(this.smbData.ByteCount
                                - CifsMessageUtils.GetSize<Guid>(this.smbData.ServerGuid));
                        }
                    }
                }
                else if (this.smbParametersBlock.WordCount == WordCountOfLanManager)
                {
                    throw new NotImplementedException("TD did not define the SmbParameters for Lan Manager Dialect.");
                }
                else if (this.smbParametersBlock.WordCount != WordCountOfCoreProtocol)
                {
                    throw new NotSupportedException("Unknow SmbParameters structure.");
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
            this.smbData.ServerGuid = new Guid();
            this.smbData.SecurityBlob = new byte[0];
        }


        #endregion
    }
}
