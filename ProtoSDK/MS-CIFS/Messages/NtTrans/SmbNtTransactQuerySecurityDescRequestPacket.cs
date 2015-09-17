// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbNtTransactQuerySecurityDesc Request
    /// </summary>
    public class SmbNtTransactQuerySecurityDescRequestPacket : SmbNtTransactRequestPacket
    {
        #region Fields

        private NT_TRANSACT_QUERY_SECURITY_DESC_Request_NT_Trans_Parameters ntTransParameters;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the NT_Trans_Parameters:NT_TRANSACT_QUERY_SECURITY_DESC_Request_NT_Trans_Parameters
        /// </summary>
        public NT_TRANSACT_QUERY_SECURITY_DESC_Request_NT_Trans_Parameters NtTransParameters
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
        public SmbNtTransactQuerySecurityDescRequestPacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbNtTransactQuerySecurityDescRequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbNtTransactQuerySecurityDescRequestPacket(SmbNtTransactQuerySecurityDescRequestPacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

            this.ntTransParameters.FID = packet.ntTransParameters.FID;
            this.ntTransParameters.Reserved = packet.ntTransParameters.Reserved;
            this.ntTransParameters.SecurityInfoFields = packet.ntTransParameters.SecurityInfoFields;
        }

        #endregion


        #region override methods

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this.</returns>
        public override StackPacket Clone()
        {
            return new SmbNtTransactQuerySecurityDescRequestPacket(this);
        }


        /// <summary>
        /// Encode the struct of NtTransParameters into the byte array in SmbData.NT_Trans_Parameters
        /// </summary>
        protected override void EncodeNtTransParameters()
        {
            this.smbData.NT_Trans_Parameters =
                CifsMessageUtils.ToBytes<NT_TRANSACT_QUERY_SECURITY_DESC_Request_NT_Trans_Parameters>(
                this.ntTransParameters);
        }


        /// <summary>
        /// Encode the struct of NtTransData into the byte array in SmbData.NT_Trans_Data
        /// </summary>
        protected override void EncodeNtTransData()
        {
            this.smbData.NT_Trans_Data = new byte[0];
        }


        /// <summary>
        /// to decode the NtTrans parameters: from the general NtTransParameters to the concrete NtTrans Parameters.
        /// </summary>
        protected override void DecodeNtTransParameters()
        {
            if (this.smbData.NT_Trans_Parameters != null)
            {
                this.ntTransParameters =
                    TypeMarshal.ToStruct<NT_TRANSACT_QUERY_SECURITY_DESC_Request_NT_Trans_Parameters>(
                    this.smbData.NT_Trans_Parameters);
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