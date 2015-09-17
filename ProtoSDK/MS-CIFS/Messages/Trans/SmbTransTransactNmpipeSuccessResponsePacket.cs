// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    ///  Packets for SmbTransTransactNmpipe SuccessResponse
    /// </summary>
    public class SmbTransTransactNmpipeSuccessResponsePacket : SmbTransactionSuccessResponsePacket
    {
        #region Fields

        private TRANS_TRANSACT_NMPIPE_Response_Trans_Data transData;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Trans_Data:TRANS_TRANSACT_NMPIPE_Response_Trans_Data
        /// </summary>
        public TRANS_TRANSACT_NMPIPE_Response_Trans_Data TransData
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
        public SmbTransTransactNmpipeSuccessResponsePacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbTransTransactNmpipeSuccessResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbTransTransactNmpipeSuccessResponsePacket(SmbTransTransactNmpipeSuccessResponsePacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

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
            return new SmbTransTransactNmpipeSuccessResponsePacket(this);
        }


        /// <summary>
        /// Encode the struct of TransParameters into the byte array in SmbData.Trans_Parameters
        /// </summary>
        protected override void EncodeTransParameters()
        {
        }


        /// <summary>
        /// Encode the struct of TRANS_TRANSACT_NMPIPE_Response_Trans_Data into the struct of SmbData
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