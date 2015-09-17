// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    ///  Packets for SmbTransQueryNmpipeInfo SuccessResponse
    /// </summary>
    public class SmbTransQueryNmpipeInfoSuccessResponsePacket : SmbTransactionSuccessResponsePacket
    {
        #region Fields

        private TRANS_QUERY_NMPIPE_INFO_Response_Trans_Data transData;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Trans_Data:TRANS_QUERY_NMPIPE_INFO_Response_Trans_Data
        /// </summary>
        public TRANS_QUERY_NMPIPE_INFO_Response_Trans_Data TransData
        {
            get
            {
                return this.transData;
            }
            set
            {
                this.transData = value;
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SmbTransQueryNmpipeInfoSuccessResponsePacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbTransQueryNmpipeInfoSuccessResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbTransQueryNmpipeInfoSuccessResponsePacket(SmbTransQueryNmpipeInfoSuccessResponsePacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

            this.transData.OutputBufferSize = packet.transData.OutputBufferSize;
            this.transData.InputBufferSize = packet.transData.InputBufferSize;
            this.transData.MaximumInstances = packet.transData.MaximumInstances;
            this.transData.CurrentInstances = packet.transData.CurrentInstances;
            this.transData.PipeNameLength = packet.transData.PipeNameLength;
            if (packet.transData.PipeName != null)
            {
                this.transData.PipeName = new byte[packet.transData.PipeName.Length];
                Array.Copy(packet.transData.PipeName, this.transData.PipeName, packet.transData.PipeName.Length);
            }
            if (packet.transData.Pad != null)
            {
                this.transData.Pad = new byte[packet.transData.Pad.Length];
                Array.Copy(packet.transData.Pad, this.transData.Pad, packet.transData.Pad.Length);
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
            return new SmbTransQueryNmpipeInfoSuccessResponsePacket(this);
        }


        /// <summary>
        /// Encode the struct of TransParameters into the byte array in SmbData.Trans_Parameters
        /// </summary>
        protected override void EncodeTransParameters()
        {
        }


        /// <summary>
        /// Encode the struct of TransData into the byte array in SmbData.Trans_Data
        /// </summary>
        protected override void EncodeTransData()
        {
            this.smbData.Trans_Data = ArrayUtility.ConcatenateArrays(
                TypeMarshal.ToBytes(this.transData.OutputBufferSize),
                TypeMarshal.ToBytes(this.transData.InputBufferSize),
                TypeMarshal.ToBytes(this.transData.MaximumInstances),
                TypeMarshal.ToBytes(this.transData.CurrentInstances),
                TypeMarshal.ToBytes(this.transData.PipeNameLength),
                this.transData.Pad,
                this.transData.PipeName);
        }


        /// <summary>
        /// to decode the Trans parameters: from the general TransParameters to the concrete Trans Parameters.
        /// </summary>
        protected override void DecodeTransParameters()
        {
        }


        /// <summary>
        /// to decode the Trans data: from the general TransDada to the concrete Trans Data.
        /// </summary>
        protected override void DecodeTransData()
        {
            if (this.smbData.Trans_Data != null && this.smbData.Trans_Data.Length > 0)
            {
                using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans_Data))
                {
                    using (Channel channel = new Channel(null, memoryStream))
                    {
                        this.transData.OutputBufferSize = channel.Read<ushort>();
                        this.transData.InputBufferSize = channel.Read<ushort>();
                        this.transData.MaximumInstances = channel.Read<byte>();
                        this.transData.CurrentInstances = channel.Read<byte>();
                        this.transData.PipeNameLength = channel.Read<byte>();

                        if ((this.smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE)
                        {
                            this.transData.Pad = new byte[1]{ channel.Read<byte>()};
                        }
                        this.transData.PipeName = channel.ReadBytes(this.transData.PipeNameLength);
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
        }

        #endregion
    }
}