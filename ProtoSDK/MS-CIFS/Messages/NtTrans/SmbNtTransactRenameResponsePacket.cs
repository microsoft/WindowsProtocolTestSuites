// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbNtTransactRename Response
    /// </summary>
    public class SmbNtTransactRenameResponsePacket : SmbNtTransactSuccessResponsePacket
    {
        #region Fields

        // none

        #endregion


        #region Properties

        // none

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SmbNtTransactRenameResponsePacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbNtTransactRenameResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbNtTransactRenameResponsePacket(SmbNtTransactRenameResponsePacket packet)
            : base(packet)
        {
            this.InitDefaultValue();
        }

        #endregion


        #region override methods

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this.</returns>
        public override StackPacket Clone()
        {
            return new SmbNtTransactRenameResponsePacket(this);
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