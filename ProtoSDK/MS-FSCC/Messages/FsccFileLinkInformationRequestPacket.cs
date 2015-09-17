// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc
{
    /// <summary>
    /// the request packet of FileLinkInformation 
    /// </summary>
    public class FsccFileLinkInformationRequestPacket : FsccStandardBytesPacket
    {
        #region Properties

        /// <summary>
        /// the command of fscc packet 
        /// </summary>
        public override uint Command
        {
            get
            {
                return (uint)FileInformationCommand.FileLinkInformation;
            }
        }

        /// <summary>
        /// get the smb format payload
        /// </summary>
        public FILE_LINK_INFORMATION_TYPE_SMB FileLinkInformationSMB
        {
            get
            {
                return TypeMarshal.ToStruct<FILE_LINK_INFORMATION_TYPE_SMB>(this.Payload);
            }
            set
            {
                this.Payload = TypeMarshal.ToBytes<FILE_LINK_INFORMATION_TYPE_SMB>(value);
            }
        }


        /// <summary>
        /// get the smb2 format payload
        /// </summary>
        public FILE_LINK_INFORMATION_TYPE_SMB2 FileLinkInformationSMB2
        {
            get
            {
                return TypeMarshal.ToStruct<FILE_LINK_INFORMATION_TYPE_SMB2>(this.Payload);
            }
            set
            {
                this.Payload = TypeMarshal.ToBytes<FILE_LINK_INFORMATION_TYPE_SMB2>(value);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// default constructor 
        /// </summary>
        public FsccFileLinkInformationRequestPacket()
            : base()
        {
        }


        #endregion
    }
}
