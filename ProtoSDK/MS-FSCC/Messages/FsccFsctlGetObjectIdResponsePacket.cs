// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc
{
    /// <summary>
    /// the response packet of FSCTL_GET_OBJECT_ID 
    /// </summary>
    public class FsccFsctlGetObjectIdResponsePacket : FsccStandardBytesPacket
    {
        #region Properties

        /// <summary>
        /// the command of fscc packet 
        /// </summary>
        public override uint Command
        {
            get
            {
                return (uint)FsControlCommand.FSCTL_GET_OBJECT_ID;
            }
        }


        /// <summary>
        /// get the buffer type1 format payload
        /// </summary>
        public FILE_OBJECTID_BUFFER_Type_1 FileObjectidBufferType1
        {
            get
            {
                return TypeMarshal.ToStruct<FILE_OBJECTID_BUFFER_Type_1>(this.Payload);
            }
            set
            {
                this.Payload = TypeMarshal.ToBytes<FILE_OBJECTID_BUFFER_Type_1>(value);
            }
        }


        /// <summary>
        /// get the buffer type2 format payload
        /// </summary>
        public FILE_OBJECTID_BUFFER_Type_2 FileObjectidBufferType2
        {
            get
            {
                return TypeMarshal.ToStruct<FILE_OBJECTID_BUFFER_Type_2>(this.Payload);
            }
            set
            {
                this.Payload = TypeMarshal.ToBytes<FILE_OBJECTID_BUFFER_Type_2>(value);
            }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// default constructor 
        /// </summary>
        public FsccFsctlGetObjectIdResponsePacket()
            : base()
        {
        }


        #endregion
    }
}
