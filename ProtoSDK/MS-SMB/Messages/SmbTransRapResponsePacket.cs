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
    /// rap packet. 
    /// </summary>
    public class SmbTransRapResponsePacket : SmbTransactionSuccessResponsePacket
    {
        #region Fields

        private TRANSACTION_Rap_Response_Trans_Parameters transParameters;
        private TRANSACTION_Rap_Response_Trans_Data transData;

        #endregion

        #region Properties

        /// <summary>
        /// get or set the transParameters:TRANSACTION_Rap_Response_Trans_Parameters 
        /// </summary>
        public TRANSACTION_Rap_Response_Trans_Parameters TransParameters
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
        /// get or set the transData: TRANSACTION_Rap_Response_Trans_Data 
        /// </summary>
        public TRANSACTION_Rap_Response_Trans_Data TransData
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
        public SmbTransRapResponsePacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbTransRapResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbTransRapResponsePacket(SmbTransRapResponsePacket packet)
            : base(packet)
        {
        }


        #endregion

        #region override methods

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this. </returns>
        public override StackPacket Clone()
        {
            return new SmbTransRapResponsePacket(this);
        }


        /// <summary>
        /// Encode the struct of TransParameters into the byte array in SmbData.Trans_Parameters 
        /// </summary>
        protected override void EncodeTransParameters()
        {
            List<byte> bytes = new List<byte>();

            bytes.AddRange(TypeMarshal.ToBytes<ushort>(this.transParameters.Win32ErrorCode));
            bytes.AddRange(TypeMarshal.ToBytes<ushort>(this.transParameters.Converter));
            if (this.transParameters.RAPOutParams != null)
            {
                bytes.AddRange(this.transParameters.RAPOutParams);
            }

            this.smbData.Trans_Parameters = bytes.ToArray();
        }


        /// <summary>
        /// Encode the struct of TransData into the byte array in SmbData.Trans_Data 
        /// </summary>
        protected override void EncodeTransData()
        {
            this.smbData.Trans_Data = this.transData.RAPOutData;
        }


        /// <summary>
        /// to decode the Trans parameters: from the general TransParameters to the concrete Trans Parameters. 
        /// </summary>
        protected override void DecodeTransParameters()
        {
            if (this.smbData.Trans_Parameters == null || this.smbData.Trans_Parameters.Length == 0)
            {
                return;
            }

            TRANSACTION_Rap_Response_Trans_Parameters param = new TRANSACTION_Rap_Response_Trans_Parameters();
            using (MemoryStream stream = new MemoryStream(this.smbData.Trans_Parameters))
            {
                using (Channel channel = new Channel(null, stream))
                {
                    param.Win32ErrorCode = channel.Read<ushort>();
                    param.Converter = channel.Read<ushort>();
                    // the ParamDesc and DataDesc size
                    int descSize = this.smbData.Trans_Parameters.Length - SmbCapability.NUM_BYTES_OF_WORD * 2;
                    if (descSize > 0)
                    {
                        param.RAPOutParams = channel.ReadBytes(descSize);
                    }
                }
            }
            this.transParameters = param;
        }


        /// <summary>
        /// to decode the Trans data: from the general TransDada to the concrete Trans Data. 
        /// </summary>
        protected override void DecodeTransData()
        {
            this.transData.RAPOutData = this.smbData.Trans_Data;
        }


        #endregion
    }
}
