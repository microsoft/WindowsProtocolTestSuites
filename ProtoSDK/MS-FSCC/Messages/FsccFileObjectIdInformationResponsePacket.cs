// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc
{
    /// <summary>
    /// the response packet of FileObjectIdInformation 
    /// </summary>
    public class FsccFileObjectIdInformationResponsePacket : FsccStandardBytesPacket
    {
        #region Properties

        /// <summary>
        /// the command of fscc packet 
        /// </summary>
        public override uint Command
        {
            get
            {
                return (uint)FileInformationCommand.FileObjectIdInformation;
            }
        }


        /// <summary>
        /// get the type1 format payload
        /// </summary>
        public FileObjectIdInformation_Type_1 FileObjectIdInformationType1
        {
            get
            {
                return TypeMarshal.ToStruct<FileObjectIdInformation_Type_1>(this.Payload);
            }
            set
            {
                this.Payload = TypeMarshal.ToBytes<FileObjectIdInformation_Type_1>(value);
            }
        }


        /// <summary>
        /// get the type2 format payload
        /// </summary>
        public FileObjectIdInformation_Type_2 FileObjectIdInformationType2
        {
            get
            {
                return TypeMarshal.ToStruct<FileObjectIdInformation_Type_2>(this.Payload);
            }
            set
            {
                this.Payload = TypeMarshal.ToBytes<FileObjectIdInformation_Type_2>(value);
            }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// default constructor 
        /// </summary>
        public FsccFileObjectIdInformationResponsePacket()
            : base()
        {
        }


        #endregion
    }
}
