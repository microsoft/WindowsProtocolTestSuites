// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// copy chunk. 
    /// </summary>
    public class SmbNtTransFsctlSrvCopyChunkRequestPacket : Cifs.SmbNtTransactIoctlRequestPacket
    {
        #region Fields

        private NT_TRANSACT_COPY_CHUNK_Request_NT_Trans_Data ntTransData;

        #endregion

        #region Properties

        /// <summary>
        /// get or set the NT_Trans_Data:NT_TRANSACT_COPY_CHUNK_Request_NT_Trans_Data 
        /// </summary>
        public new NT_TRANSACT_COPY_CHUNK_Request_NT_Trans_Data NtTransData
        {
            get
            {
                return this.ntTransData;
            }
            set
            {
                this.ntTransData = value;

                // update to base data.
                NT_TRANSACT_IOCTL_Request_NT_Trans_Data data = base.NtTransData;

                data.Data = CifsMessageUtils.ToBytes<NT_TRANSACT_COPY_CHUNK_Request_NT_Trans_Data>(value);

                base.NtTransData = data;
            }
        }


        #endregion

        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbNtTransFsctlSrvCopyChunkRequestPacket(Cifs.SmbNtTransactIoctlRequestPacket packet)
            : base(packet)
        {
            if (packet.NtTransData.Data != null && packet.NtTransData.Data.Length > 0)
            {
                this.ntTransData =
                  CifsMessageUtils.ToStuct<NT_TRANSACT_COPY_CHUNK_Request_NT_Trans_Data>(
                    packet.NtTransData.Data);
            }
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbNtTransFsctlSrvCopyChunkRequestPacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbNtTransFsctlSrvCopyChunkRequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbNtTransFsctlSrvCopyChunkRequestPacket(
            SmbNtTransFsctlSrvCopyChunkRequestPacket packet)
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
            this.smbData.NT_Trans_Data =
                CifsMessageUtils.ToBytes<NT_TRANSACT_COPY_CHUNK_Request_NT_Trans_Data>(this.NtTransData);
        }


        /// <summary>
        /// to decode the NtTrans data: from the general NtTransDada to the concrete NtTrans Data. 
        /// </summary>
        protected override void DecodeNtTransData()
        {
            this.NtTransData =
                CifsMessageUtils.ToStuct<NT_TRANSACT_COPY_CHUNK_Request_NT_Trans_Data>(this.smbData.NT_Trans_Data);
        }


        #endregion
    }
}
