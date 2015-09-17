// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Microsoft.Protocols.TestTools.StackSdk.Messages;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// enumerate snap shots. 
    /// </summary>
    public class SmbNtTransFsctlSrvEnumerateSnapshotsResponsePacket : Cifs.SmbNtTransactIoctlResponsePacket
    {
        #region Fields

        private NT_TRANSACT_ENUMERATE_SNAPSHOTS_Response_NT_Trans_Data ntTransData;
        private Collection<string> snapShots;

        #endregion

        #region Properties

        /// <summary>
        /// get or set the NT_Trans_Data:NT_TRANSACT_ENUMERATE_SNAPSHOTS_Response_NT_Trans_Data 
        /// </summary>
        public new NT_TRANSACT_ENUMERATE_SNAPSHOTS_Response_NT_Trans_Data NtTransData
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

                List<byte> dataInBytes = new List<byte>();
                dataInBytes.AddRange(TypeMarshal.ToBytes<uint>(this.ntTransData.NumberOfSnapShots));
                dataInBytes.AddRange(TypeMarshal.ToBytes<uint>(this.ntTransData.NumberOfSnapShotsReturned));
                dataInBytes.AddRange(TypeMarshal.ToBytes<uint>(this.ntTransData.SnapShotArraySize));
                if (this.ntTransData.snapShotMultiSZ != null)
                {
                    dataInBytes.AddRange(this.ntTransData.snapShotMultiSZ);
                }

                data.Data = dataInBytes.ToArray();

                base.NtTransData = data;
            }
        }


        /// <summary>
        /// the returned snap shots 
        /// </summary>
        public Collection<string> SnapShots
        {
            get
            {
                if (this.snapShots == null)
                {
                    this.snapShots = new Collection<string>();
                }

                return this.snapShots;
            }
            set
            {
                this.snapShots = value;
            }
        }


        #endregion

        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbNtTransFsctlSrvEnumerateSnapshotsResponsePacket(Cifs.SmbNtTransactIoctlResponsePacket packet)
            : base(packet)
        {
            this.ntTransData =
                CifsMessageUtils.ToStuct<NT_TRANSACT_ENUMERATE_SNAPSHOTS_Response_NT_Trans_Data>(
                packet.NtTransData.Data);
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbNtTransFsctlSrvEnumerateSnapshotsResponsePacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbNtTransFsctlSrvEnumerateSnapshotsResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbNtTransFsctlSrvEnumerateSnapshotsResponsePacket(
            SmbNtTransFsctlSrvEnumerateSnapshotsResponsePacket packet)
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
            List<byte> data = new List<byte>();
            foreach (string snapshot in SnapShots)
            {
                byte[] bytes = Encoding.Unicode.GetBytes(snapshot + "\0");
                data.AddRange(bytes);
            }
            data.AddRange(new byte[2]);

            NT_TRANSACT_ENUMERATE_SNAPSHOTS_Response_NT_Trans_Data snapshotsNtTransData =
                new NT_TRANSACT_ENUMERATE_SNAPSHOTS_Response_NT_Trans_Data();
            snapshotsNtTransData.NumberOfSnapShots = this.ntTransData.NumberOfSnapShots;
            snapshotsNtTransData.NumberOfSnapShotsReturned = this.ntTransData.NumberOfSnapShotsReturned;
            snapshotsNtTransData.SnapShotArraySize = (uint)data.Count;
            snapshotsNtTransData.snapShotMultiSZ = data.ToArray();

            this.NtTransData = snapshotsNtTransData;

            this.smbData.Data =
                CifsMessageUtils.ToBytes<NT_TRANSACT_ENUMERATE_SNAPSHOTS_Response_NT_Trans_Data>(this.NtTransData);
        }


        /// <summary>
        /// to decode the NtTrans data: from the general NtTransDada to the concrete NtTrans Data. 
        /// </summary>
        protected override void DecodeNtTransData()
        {
            NT_TRANSACT_ENUMERATE_SNAPSHOTS_Response_NT_Trans_Data data
                = new NT_TRANSACT_ENUMERATE_SNAPSHOTS_Response_NT_Trans_Data();

            using (MemoryStream stream = new MemoryStream(this.smbData.Data))
            {
                using (Channel channel = new Channel(null, stream))
                {
                    data.NumberOfSnapShots = channel.Read<uint>();
                    data.NumberOfSnapShotsReturned = channel.Read<uint>();
                    data.SnapShotArraySize = channel.Read<uint>();
                    if (data.NumberOfSnapShotsReturned > 0)
                    {
                        data.snapShotMultiSZ = channel.ReadBytes((int)data.SnapShotArraySize);
                    }
                }
            }

            this.NtTransData = data;

            if (this.ntTransData.snapShotMultiSZ == null || this.ntTransData.SnapShotArraySize <= 2)
            {
                return;
            }

            string snapshot = Encoding.Unicode.GetString(this.ntTransData.snapShotMultiSZ);
            this.SnapShots = new Collection<string>(snapshot.Trim('\0').Split('\0'));
        }


        #endregion
    }
}
