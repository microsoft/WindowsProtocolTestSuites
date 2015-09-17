// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc
{
    /// <summary>
    /// the response packet of FSCTL_GET_REPARSE_POINT 
    /// </summary>
    public class FsccFsctlGetReparsePointResponsePacket : FsccStandardBytesPacket
    {
        #region Properties

        /// <summary>
        /// the command of fscc packet 
        /// </summary>
        public override uint Command
        {
            get
            {
                return (uint)FsControlCommand.FSCTL_GET_REPARSE_POINT;
            }
        }


        /// <summary>
        /// get the guid data buffer format payload
        /// </summary>
        public REPARSE_GUID_DATA_BUFFER ReparseGuidDataBuffer
        {
            get
            {
                return TypeMarshal.ToStruct<REPARSE_GUID_DATA_BUFFER>(this.Payload);
            }
            set
            {
                this.Payload = TypeMarshal.ToBytes<REPARSE_GUID_DATA_BUFFER>(value);
            }
        }


        /// <summary>
        /// get the reparse data buffer format payload
        /// </summary>
        public REPARSE_DATA_BUFFER ReparseDataBuffer
        {
            get
            {
                return TypeMarshal.ToStruct<REPARSE_DATA_BUFFER>(this.Payload);
            }
            set
            {
                this.Payload = TypeMarshal.ToBytes<REPARSE_DATA_BUFFER>(value);
            }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// default constructor 
        /// </summary>
        public FsccFsctlGetReparsePointResponsePacket()
            : base()
        {
        }


        #endregion
    }
}
