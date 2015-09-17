// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbNtTransactIoctl Response
    /// </summary>
    public class SmbNtTransactIoctlResponsePacket : SmbNtTransactSuccessResponsePacket
    {
        #region Fields

        private NT_TRANSACT_IOCTL_Response_NT_Trans_Data ntTransData;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the NT_Trans_Data:NT_TRANSACT_IOCTL_Response_NT_Trans_Data
        /// </summary>
        public NT_TRANSACT_IOCTL_Response_NT_Trans_Data NtTransData
        {
            get
            {
                return this.ntTransData;
            }
            set
            {
                this.ntTransData = value;
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SmbNtTransactIoctlResponsePacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbNtTransactIoctlResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbNtTransactIoctlResponsePacket(SmbNtTransactIoctlResponsePacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

            if (packet.ntTransData.Data != null)
            {
                this.ntTransData.Data = new byte[packet.ntTransData.Data.Length];
                Array.Copy(packet.ntTransData.Data, this.ntTransData.Data, packet.ntTransData.Data.Length);
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
            return new SmbNtTransactIoctlResponsePacket(this);
        }


        /// <summary>
        /// Encode the struct of NtTransParameters into the byte array in SmbData.NT_Trans_Parameters
        /// </summary>
        protected override void EncodeNtTransParameters()
        {
        }


        /// <summary>
        /// Encode the struct of NtTransData into the byte array in SmbData.NT_Trans_Data
        /// </summary>
        protected override void EncodeNtTransData()
        {
            if (this.ntTransData.Data != null)
            {
                this.smbData.Data = new byte[this.ntTransData.Data.Length];
                Array.Copy(this.ntTransData.Data, this.smbData.Data, this.ntTransData.Data.Length);
            }
        }


        /// <summary>
        /// to decode the NtTrans parameters: from the general NtTransParameters to the concrete NtTrans Parameters.
        /// </summary>
        protected override void DecodeNtTransParameters()
        {
        }


        /// <summary>
        /// to decode the NtTrans data: from the general NtTransDada to the concrete NtTrans Data.
        /// </summary>
        protected override void DecodeNtTransData()
        {
            if (this.smbData.Data != null)
            {
                this.ntTransData.Data = new byte[this.smbData.Data.Length];
                Array.Copy(this.smbData.Data, this.ntTransData.Data, this.smbData.Data.Length);
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