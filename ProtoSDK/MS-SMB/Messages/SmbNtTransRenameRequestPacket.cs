// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// query quota. 
    /// </summary>
    public class SmbNtTransRenameRequestPacket : Cifs.SmbNtTransactRenameRequestPacket
    {
        #region Fields

        private NT_TRANSACT_RENAME_Request_NT_Trans_Parameters ntTransParameters;
        
        #endregion

        #region Properties

        /// <summary>
        /// get or set the ntTransParameters:NT_TRANSACT_RENAME_Request_NT_Trans_Parameters 
        /// </summary>
        public NT_TRANSACT_RENAME_Request_NT_Trans_Parameters NtTransParameters
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

        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbNtTransRenameRequestPacket(Cifs.SmbNtTransactRenameRequestPacket packet)
            : base(packet)
        {
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbNtTransRenameRequestPacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbNtTransRenameRequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbNtTransRenameRequestPacket(SmbNtTransRenameRequestPacket packet)
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
            return new SmbNtTransRenameRequestPacket(this);
        }


        /// <summary>
        /// Encode the struct of NtTransParameters into the byte array in SmbData.NT_Trans_Parameters 
        /// </summary>
        protected override void EncodeNtTransParameters()
        {
            List<byte> bytes = new List<byte>();

            bytes.AddRange(TypeMarshal.ToBytes<ushort>(this.ntTransParameters.Fid));
            bytes.AddRange(TypeMarshal.ToBytes<ushort>(this.ntTransParameters.RenameFlags));
            if (this.ntTransParameters.Pad1 != null)
            {
                bytes.AddRange(this.ntTransParameters.Pad1);
            }
            if (this.ntTransParameters.NewName != null)
            {
                bytes.AddRange(this.ntTransParameters.NewName);
            }

            this.smbData.NT_Trans_Parameters = bytes.ToArray();
        }


        /// <summary>
        /// to decode the NtTrans parameters: from the general NtTransParameters to the concrete NtTrans Parameters. 
        /// </summary>
        protected override void DecodeNtTransParameters()
        {
            this.ntTransParameters = 
                CifsMessageUtils.ToStuct<NT_TRANSACT_RENAME_Request_NT_Trans_Parameters>(
                this.smbData.NT_Trans_Parameters);
        }


        #endregion
    }
}
