// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc
{
    /// <summary>
    /// the request packet of FileRenameInformation 
    /// </summary>
    public class FsccFileRenameInformationRequestPacket : FsccStandardBytesPacket
    {
        #region Properties

        /// <summary>
        /// the command of fscc packet 
        /// </summary>
        public override uint Command
        {
            get
            {
                return (uint)FileInformationCommand.FileRenameInformation;
            }
        }


        /// <summary>
        /// get the smb format payload
        /// </summary>
        public FileRenameInformation_SMB FileRenameInformationSmb
        {
            get
            {
                return TypeMarshal.ToStruct<FileRenameInformation_SMB>(this.Payload);
            }
            set
            {
                this.Payload = TypeMarshal.ToBytes<FileRenameInformation_SMB>(value);
            }
        }


        /// <summary>
        /// get the smb2 format payload
        /// </summary>
        public FileRenameInformation_SMB2 FileRenameInformationSmb2
        {
            get
            {
                return TypeMarshal.ToStruct<FileRenameInformation_SMB2>(this.Payload);
            }
            set
            {
                this.Payload = TypeMarshal.ToBytes<FileRenameInformation_SMB2>(value);
            }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// default constructor 
        /// </summary>
        public FsccFileRenameInformationRequestPacket()
            : base()
        {
        }


        #endregion
    }
}
