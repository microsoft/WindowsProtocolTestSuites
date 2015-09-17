// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Microsoft.Protocols.TestTools.StackSdk.Messages;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// Packets for SmbSessionSetupAndx Request 
    /// </summary>
    public class SmbSessionSetupAndxRequestPacket : Cifs.SmbBatchedRequestPacket
    {
        #region Fields

        private SMB_COM_SESSION_SETUP_ANDX_Request_SMB_Parameters smbParameters;
        private SMB_COM_SESSION_SETUP_ANDX_Request_SMB_Data smbData;
        private byte[] sessionKey;

        #endregion


        #region Properties

        /// <summary>
        /// 
        /// </summary>
        protected override SmbCommand AndxCommand
        {
            get
            {
                return this.SmbParameters.AndXCommand;
            }
        }


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
        /// the SessionKey for the implicit NTLM. 
        /// </summary>
        public byte[] SessionKey
        {
            get
            {
                return this.sessionKey;
            }
            set
            {
                this.sessionKey = value;
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
            if (packet.SessionKey != null)
            {
                this.sessionKey = new byte[packet.SessionKey.Length];
                Array.Copy(packet.SessionKey, this.sessionKey,
                    this.sessionKey.Length);
            }
            this.smbParameters.WordCount = packet.SmbParameters.WordCount;
            this.smbParameters.AndXCommand = packet.SmbParameters.AndXCommand;
            this.smbParameters.AndXReserved = packet.SmbParameters.AndXReserved;
            this.smbParameters.AndXOffset = packet.SmbParameters.AndXOffset;
            this.smbParameters.MaxBufferSize = packet.SmbParameters.MaxBufferSize;
            this.smbParameters.MaxMpxCount = packet.SmbParameters.MaxMpxCount;
            this.smbParameters.VcNumber = packet.SmbParameters.VcNumber;
            this.smbParameters.SessionKey = packet.SmbParameters.SessionKey;
            this.smbParameters.SecurityBlobLength = packet.SmbParameters.SecurityBlobLength;
            this.smbParameters.Reserved = packet.SmbParameters.Reserved;
            this.smbParameters.Capabilities = packet.SmbParameters.Capabilities;

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
        }


        #endregion


        #region override methods

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this. </returns>
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

            List<byte> bytes = new List<byte>();

            if (this.SmbData.SecurityBlob != null)
            {
                bytes.AddRange(this.SmbData.SecurityBlob);
            }
            if (this.SmbData.Pad != null)
            {
                bytes.AddRange(this.SmbData.Pad);
            }
            if (this.SmbData.NativeOS != null)
            {
                bytes.AddRange(this.SmbData.NativeOS);
            }
            if (this.SmbData.NativeLanMan != null)
            {
                bytes.AddRange(this.SmbData.NativeLanMan);
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
                this.smbParameters = CifsMessageUtils.ToStuct<SMB_COM_SESSION_SETUP_ANDX_Request_SMB_Parameters>(
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
                using (MemoryStream stream = new MemoryStream(CifsMessageUtils.ToBytes<SmbData>(this.smbDataBlock)))
                {
                    using (Channel channel = new Channel(null, stream))
                    {

                        this.smbData.ByteCount = channel.Read<ushort>();
                        this.smbData.SecurityBlob = channel.ReadBytes(this.SmbParameters.SecurityBlobLength);

                        // pad:
                        if ((Marshal.SizeOf(this.SmbHeader) + Marshal.SizeOf(this.SmbParameters)
                            + Marshal.SizeOf(this.smbData.ByteCount) + this.SmbParameters.SecurityBlobLength) % 2 != 0)
                        {
                            this.smbData.Pad = channel.ReadBytes(1);
                        }
                        else
                        {
                            this.smbData.Pad = new byte[0];
                        }

                        // NativeOS:
                        int nativeOSLen = 0;

                        while (true)
                        {
                            if ((this.SmbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE)
                            {
                                byte data0 = channel.Peek<byte>(nativeOSLen++);
                                byte data1 = channel.Peek<byte>(nativeOSLen++);

                                if (data0 == 0 && data1 == 0)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                byte data = channel.Peek<byte>(nativeOSLen++);

                                if (data == 0)
                                {
                                    break;
                                }
                            }
                        }
                        this.smbData.NativeOS = channel.ReadBytes(nativeOSLen);

                        // NativeLanMan
                        int nativeLanManLen = 0;
                        while (true)
                        {
                            if ((this.SmbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE)
                            {
                                byte data0 = channel.Peek<byte>(nativeLanManLen++);
                                byte data1 = channel.Peek<byte>(nativeLanManLen++);

                                if (data0 == 0 && data1 == 0)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                byte data = channel.Peek<byte>(nativeLanManLen++);

                                if (data == 0)
                                {
                                    break;
                                }
                            }
                        }
                        this.smbData.NativeLanMan = channel.ReadBytes(nativeLanManLen);
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
            this.smbParameters.WordCount = 0xC;
            this.smbParameters.AndXCommand = SmbCommand.SMB_COM_NO_ANDX_COMMAND;

            this.smbData.SecurityBlob = new byte[0];
            this.smbData.NativeLanMan = new byte[0];
            this.smbData.Pad = new byte[0];
            this.smbData.NativeOS = new byte[0];
        }


        #endregion
    }
}
