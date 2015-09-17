// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbTransSetNmpipeState Request
    /// </summary>
    public class SmbTransSetNmpipeStateRequestPacket : SmbTransactionRequestPacket
    {
        #region Fields

        private TRANS_SET_NMPIPE_STATE_RequestTransParameters transParameters;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Trans_Parameters:TRANS_SET_NMPIPE_STATE_RequestTransParameters
        /// </summary>
        public TRANS_SET_NMPIPE_STATE_RequestTransParameters TransParameters
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
        public SmbTransSetNmpipeStateRequestPacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbTransSetNmpipeStateRequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbTransSetNmpipeStateRequestPacket(SmbTransSetNmpipeStateRequestPacket packet)
            : base(packet)
        {
            this.InitDefaultValue();
            this.transParameters.PipeState = packet.transParameters.PipeState;
        }

        #endregion


        #region override methods

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this.</returns>
        public override StackPacket Clone()
        {
            return new SmbTransSetNmpipeStateRequestPacket(this);
        }


        /// <summary>
        /// Encode the struct of TransParameters into the byte array in SmbData.Trans_Parameters
        /// </summary>
        protected override void EncodeTransParameters()
        {
            this.smbData.Trans_Parameters = CifsMessageUtils.ToBytes<TRANS_SET_NMPIPE_STATE_RequestTransParameters>(
                this.transParameters);
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
            if (this.smbData.Trans_Parameters != null)
            {
                this.transParameters = TypeMarshal.ToStruct<TRANS_SET_NMPIPE_STATE_RequestTransParameters>(
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