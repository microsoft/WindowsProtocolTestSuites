// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// resume key 
    /// </summary>
    public class SmbNtTransFsctlSrvRequestResumeKeyResponsePacket : Cifs.SmbNtTransactIoctlResponsePacket
    {
        #region Fields

        private NT_TRANSACT_RESUME_KEY_Response_NT_Trans_Data ntTransData;

        #endregion

        #region Properties

        /// <summary>
        /// get or set the NT_Trans_Data:NT_TRANSACT_RESUME_KEY_Response_NT_Trans_Data 
        /// </summary>
        public new NT_TRANSACT_RESUME_KEY_Response_NT_Trans_Data NtTransData
        {
            get
            {
                return this.ntTransData;
            }
            set
            {
                this.ntTransData = value;

                // update to base data.
                NT_TRANSACT_IOCTL_Response_NT_Trans_Data data = base.NtTransData;

                data.Data = CifsMessageUtils.ToBytes<NT_TRANSACT_RESUME_KEY_Response_NT_Trans_Data>(value);

                base.NtTransData = data;
            }
        }


        #endregion

        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbNtTransFsctlSrvRequestResumeKeyResponsePacket(Cifs.SmbNtTransactIoctlResponsePacket packet)
            : base(packet)
        {
            this.ntTransData =
                CifsMessageUtils.ToStuct<NT_TRANSACT_RESUME_KEY_Response_NT_Trans_Data>(
                packet.NtTransData.Data);
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbNtTransFsctlSrvRequestResumeKeyResponsePacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbNtTransFsctlSrvRequestResumeKeyResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbNtTransFsctlSrvRequestResumeKeyResponsePacket(
            SmbNtTransFsctlSrvRequestResumeKeyResponsePacket packet)
            : base(packet)
        {
        }


        #endregion

        #region override methods

        /// <summary>
        /// Encode the struct of NtTransData into the byte array in SmbData.NT_Trans_Data 
        /// </summary>
        protected override void EncodeNtTransData()
        {
            this.smbData.Data =
                CifsMessageUtils.ToBytes<NT_TRANSACT_RESUME_KEY_Response_NT_Trans_Data>(this.NtTransData);
        }


        /// <summary>
        /// to decode the NtTrans data: from the general NtTransDada to the concrete NtTrans Data. 
        /// </summary>
        protected override void DecodeNtTransData()
        {
            this.NtTransData =
                CifsMessageUtils.ToStuct<NT_TRANSACT_RESUME_KEY_Response_NT_Trans_Data>(this.smbData.Data);
        }


        #endregion
    }
}
