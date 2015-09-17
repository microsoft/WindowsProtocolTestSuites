// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbTransPeekNmpipe SuccessResponse
    /// </summary>
    public class SmbTransPeekNmpipeSuccessResponsePacket : SmbTransactionSuccessResponsePacket
    {
        #region Fields

        private TRANS_PEEK_NMPIPE_Response_Trans_Parameters transParameters;
        private TRANS_PEEK_NMPIPE_Response_Trans_Data transData;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Trans_Parameters:TRANS_PEEK_NMPIPE_Response_Trans_Parameters
        /// </summary>
        public TRANS_PEEK_NMPIPE_Response_Trans_Parameters TransParameters
        {
            get
            {
                return this.transParameters;
            }
            set
            {
                this.transParameters = value;
            }
        }


        /// <summary>
        /// get or set the Trans_Data:TRANS_PEEK_NMPIPE_Response_Trans_Data
        /// </summary>
        public TRANS_PEEK_NMPIPE_Response_Trans_Data TransData
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
        public SmbTransPeekNmpipeSuccessResponsePacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbTransPeekNmpipeSuccessResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbTransPeekNmpipeSuccessResponsePacket(SmbTransPeekNmpipeSuccessResponsePacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

            this.transParameters.ReadDataAvailable = packet.transParameters.ReadDataAvailable;
            this.transParameters.MessageBytesLength = packet.transParameters.MessageBytesLength;
            this.transParameters.NamedPipeState = packet.transParameters.NamedPipeState;

            if (packet.transData.ReadData != null)
            {
                this.transData.ReadData = new byte[packet.transData.ReadData.Length];
                Array.Copy(packet.transData.ReadData, this.transData.ReadData, packet.transData.ReadData.Length);
            }
            else
            {
                this.transData.ReadData = new byte[0];
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
            return new SmbTransPeekNmpipeSuccessResponsePacket(this);
        }


        /// <summary>
        /// Encode the struct of TransParameters into the byte array in SmbData.Trans_Parameters
        /// </summary>
        protected override void EncodeTransParameters()
        {
            this.smbData.Trans_Parameters =
                CifsMessageUtils.ToBytes<TRANS_PEEK_NMPIPE_Response_Trans_Parameters>(this.transParameters);
        }


        /// <summary>
        /// Encode the struct of TransData into the byte array in SmbData.Trans_Data
        /// </summary>
        protected override void EncodeTransData()
        {
            if (this.transData.ReadData != null)
            {
                this.smbData.Trans_Data = new byte[this.transData.ReadData.Length];
                Array.Copy(this.transData.ReadData, this.smbData.Trans_Data, this.transData.ReadData.Length);
            }
            else
            {
                this.transData.ReadData = new byte[0];
            }
        }


        /// <summary>
        /// to decode the Trans parameters: from the general TransParameters to the concrete Trans Parameters.
        /// </summary>
        protected override void DecodeTransParameters()
        {
            if (this.smbData.Trans_Parameters != null && this.smbData.Trans_Parameters.Length > 0)
            {
                this.transParameters = TypeMarshal.ToStruct<TRANS_PEEK_NMPIPE_Response_Trans_Parameters>(
                    this.smbData.Trans_Parameters);
            }
        }


        /// <summary>
        /// to decode the Trans data: from the general TransDada to the concrete Trans Data.
        /// </summary>
        protected override void DecodeTransData()
        {
            if (this.smbData.Trans_Data != null)
            {
                using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans_Data))
                {
                    using (Channel channel = new Channel(null, memoryStream))
                    {
                        this.transData.ReadData = channel.ReadBytes(this.smbData.Trans_Data.Length);
                    }
                }
            }
            else
            {
                this.transData.ReadData = new byte[0];
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