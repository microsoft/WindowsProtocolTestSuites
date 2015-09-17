// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbTransWriteNmpipe SuccessResponse
    /// </summary>
    public class SmbTransWriteNmpipeSuccessResponsePacket : SmbTransactionSuccessResponsePacket
    {
        #region Fields

        private TRANS_WRITE_NMPIPE_Response_Trans_Parameters transParameters;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Trans_Parameters:TRANS_WRITE_NMPIPE_Response_Trans_Parameters
        /// </summary>
        public TRANS_WRITE_NMPIPE_Response_Trans_Parameters TransParameters
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

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SmbTransWriteNmpipeSuccessResponsePacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbTransWriteNmpipeSuccessResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbTransWriteNmpipeSuccessResponsePacket(SmbTransWriteNmpipeSuccessResponsePacket packet)
            : base(packet)
        {
            this.InitDefaultValue();
            this.transParameters.BytesWritten = packet.transParameters.BytesWritten;
        }

        #endregion


        #region override methods

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this.</returns>
        public override StackPacket Clone()
        {
            return new SmbTransWriteNmpipeSuccessResponsePacket(this);
        }


        /// <summary>
        /// Encode the struct of TransParameters into the byte array in SmbData.Trans_Parameters
        /// </summary>
        protected override void EncodeTransParameters()
        {
            this.smbData.Trans_Parameters =
                CifsMessageUtils.ToBytes<TRANS_WRITE_NMPIPE_Response_Trans_Parameters>(this.transParameters);
        }


        /// <summary>
        /// Encode the struct of TransData into the byte array in SmbData.Trans_Data
        /// </summary>
        protected override void EncodeTransData()
        {
        }


        /// <summary>
        /// to decode the Trans parameters: from the general TransParameters to the concrete Trans Parameters.
        /// </summary>
        protected override void DecodeTransParameters()
        {
            if (this.smbData.Trans_Parameters != null && this.smbData.Trans_Parameters.Length > 0)
            {
                this.transParameters = TypeMarshal.ToStruct<TRANS_WRITE_NMPIPE_Response_Trans_Parameters>(
                    this.smbData.Trans_Parameters);
            }
        }


        /// <summary>
        /// to decode the Trans data: from the general TransDada to the concrete Trans Data.
        /// </summary>
        protected override void DecodeTransData()
        {
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