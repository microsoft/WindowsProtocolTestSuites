// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbNtTransactCreate Response
    /// </summary>
    public class SmbNtTransactCreateResponsePacket : SmbNtTransactSuccessResponsePacket
    {
        #region Fields

        private NT_TRANSACT_CREATE_Response_NT_Trans_Parameters ntTransParameters;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the NT_Trans_Parameters:NT_TRANSACT_CREATE_Response_NT_Trans_Parameters
        /// </summary>
        public NT_TRANSACT_CREATE_Response_NT_Trans_Parameters NtTransParameters
        {
            get
            {
                return this.ntTransParameters;
            }
            set
            {
                this.ntTransParameters = value;
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SmbNtTransactCreateResponsePacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbNtTransactCreateResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbNtTransactCreateResponsePacket(SmbNtTransactCreateResponsePacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

            this.ntTransParameters = packet.ntTransParameters;
        }

        #endregion


        #region override methods

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this.</returns>
        public override StackPacket Clone()
        {
            return new SmbNtTransactCreateResponsePacket(this);
        }


        /// <summary>
        /// Encode the struct of NtTransParameters into the byte array in SmbData.NT_Trans_Parameters
        /// </summary>
        protected override void EncodeNtTransParameters()
        {
            this.smbData.Parameters =
                CifsMessageUtils.ToBytes<NT_TRANSACT_CREATE_Response_NT_Trans_Parameters>(this.ntTransParameters);
        }


        /// <summary>
        /// Encode the struct of NtTransData into the byte array in SmbData.NT_Trans_Data
        /// </summary>
        protected override void EncodeNtTransData()
        {
        }


        /// <summary>
        /// to decode the NtTrans parameters: from the general NtTransParameters to the concrete NtTrans Parameters.
        /// </summary>
        protected override void DecodeNtTransParameters()
        {
            if (this.smbData.Parameters != null && this.smbData.Parameters.Length > 0)
            {
                this.ntTransParameters = TypeMarshal.ToStruct<NT_TRANSACT_CREATE_Response_NT_Trans_Parameters>(
                    this.smbData.Parameters);
            }
        }


        /// <summary>
        /// to decode the NtTrans data: from the general NtTransDada to the concrete NtTrans Data.
        /// </summary>
        protected override void DecodeNtTransData()
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