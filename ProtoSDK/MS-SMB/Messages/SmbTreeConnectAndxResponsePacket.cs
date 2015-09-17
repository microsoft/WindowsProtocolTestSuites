// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// Packets for SmbTreeConnectAndx Response
    /// </summary>
    public class SmbTreeConnectAndxResponsePacket : Cifs.SmbTreeConnectAndxResponsePacket
    {
        #region Fields

        private SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters smbParameters;

        #endregion

        #region Properties

        /// <summary>
        /// get or set the Smb_Parameters:SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters
        /// </summary>
        public new SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters SmbParameters
        {
            get
            {
                return this.smbParameters;
            }
            set
            {
                this.smbParameters = value;

                base.SmbParameters = SmbMessageUtils.ConvertSmbComTreeConnectPacketPayload(this.smbParameters);
            }
        }


        #endregion

        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbTreeConnectAndxResponsePacket(Cifs.SmbTreeConnectAndxResponsePacket packet)
            : base(packet)
        {
            this.smbParameters = SmbMessageUtils.ConvertSmbComTreeConnectPacketPayload(base.SmbParameters);
        }


        #endregion

        #region override methods

        /// <summary>
        /// Encode the struct of SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters into the struct of SmbParameters
        /// </summary>
        protected override void EncodeParameters()
        {
            this.smbParametersBlock = CifsMessageUtils.ToStuct<SmbParameters>(
                CifsMessageUtils.ToBytes<SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters>(this.smbParameters));
        }


        /// <summary>
        /// to decode the smb parameters: from the general SmbParameters to the concrete Smb Parameters.
        /// </summary>
        protected override void DecodeParameters()
        {
            // if smb packet, the word count must be 7.
            // end with more 2-int fields: MaximalShareAccessRights and GuestMaximalShareAccessRights.
            if (this.smbParametersBlock.WordCount == 7)
            {
                this.SmbParameters = CifsMessageUtils.ToStuct<SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters>(
                    CifsMessageUtils.ToBytes<SmbParameters>(this.smbParametersBlock));
            }
            // if cifs packet, the word count must be 3.
            else if (this.smbParametersBlock.WordCount == 3)
            {
                Cifs.SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters param =
                    CifsMessageUtils.ToStuct<Cifs.SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters>(
                    CifsMessageUtils.ToBytes<SmbParameters>(this.smbParametersBlock));

                this.SmbParameters = SmbMessageUtils.ConvertSmbComTreeConnectPacketPayload(param);
            }
            else
            {
                this.smbParameters.WordCount = this.smbParametersBlock.WordCount;
            }
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbTreeConnectAndxResponsePacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbTreeConnectAndxResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbTreeConnectAndxResponsePacket(SmbTreeConnectAndxResponsePacket packet)
            : base(packet)
        {
        }


        #endregion
    }
}
