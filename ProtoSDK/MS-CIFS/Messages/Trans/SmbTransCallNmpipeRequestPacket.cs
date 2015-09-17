// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbTransCallNmpipe Request
    /// </summary>
    public class SmbTransCallNmpipeRequestPacket : SmbTransactionRequestPacket
    {
        #region Fields

        private TRANS_CALL_NMPIPE_Request_Trans_Data transData;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Trans_Data:TRANS_CALL_NMPIPE_Request_Trans_Data
        /// </summary>
        public TRANS_CALL_NMPIPE_Request_Trans_Data TransData
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
        public SmbTransCallNmpipeRequestPacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbTransCallNmpipeRequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbTransCallNmpipeRequestPacket(SmbTransCallNmpipeRequestPacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

            this.transData.WriteData = new byte[packet.transData.WriteData.Length];
            Array.Copy(packet.transData.WriteData, this.transData.WriteData, packet.transData.WriteData.Length);
        }

        #endregion


        #region override methods

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this.</returns>
        public override StackPacket Clone()
        {
            return new SmbTransCallNmpipeRequestPacket(this);
        }


        /// <summary>
        /// Encode the struct of TransParameters into the byte array in SmbData.Trans_Parameters
        /// </summary>
        protected override void EncodeTransParameters()
        {
            this.smbData.Trans_Parameters = new byte[0];
        }


        /// <summary>
        /// Encode the struct of TransData into the byte array in SmbData.Trans_Data
        /// </summary>
        protected override void EncodeTransData()
        {
            if (this.transData.WriteData != null)
            {
                this.smbData.Trans_Data = new byte[this.transData.WriteData.Length];
                using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans_Data))
                {
                    using (Channel channel = new Channel(null, memoryStream))
                    {
                        channel.BeginWriteGroup();
                        channel.WriteBytes(this.transData.WriteData);
                        channel.EndWriteGroup();
                    }
                }
            }
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
            if (this.smbData.Trans_Data != null)
            {
                using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans_Data))
                {
                    using (Channel channel = new Channel(null, memoryStream))
                    {
                        this.transData.WriteData = channel.ReadBytes(this.SmbParameters.DataCount);
                    }
                }
            }
            else
            {
                this.transData.WriteData = new byte[0];
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