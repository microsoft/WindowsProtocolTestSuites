// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc
{
    /// <summary>
    /// the request packet of FileFsObjectIdInformation 
    /// </summary>
    public class FsccFileFsObjectIdInformationRequestPacket : FsccStandardPacket<FileFsObjectIdInformation>
    {
        #region Properties

        /// <summary>
        /// the command of fscc packet 
        /// </summary>
        public override uint Command
        {
            get
            {
                return (uint)FsInformationCommand.FileFsObjectIdInformation;
            }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// default constructor 
        /// </summary>
        public FsccFileFsObjectIdInformationRequestPacket()
            : base()
        {
        }


        #endregion
    }
}
