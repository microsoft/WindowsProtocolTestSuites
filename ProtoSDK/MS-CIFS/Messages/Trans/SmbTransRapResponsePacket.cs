// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Rap response packet. 
    /// Cifs is not responsible for defining detailed TransParameters and TransData.
    /// </summary>
    public class SmbTransRapResponsePacket : SmbTransactionSuccessResponsePacket
    {
        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbTransRapResponsePacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbTransRapResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbTransRapResponsePacket(SmbTransRapResponsePacket packet)
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
            return new SmbTransRapResponsePacket(this);
        }


        /// <summary>
        /// Encode the struct of TransParameters into the byte array in SmbData.Trans_Parameters 
        /// </summary>
        protected override void EncodeTransParameters()
        {
        }


        /// <summary>
        /// Encode the struct of TransData into the byte array in SmbData.Trans_Data 
        /// </summary>
        protected override void EncodeTransData()
        {
        }


        /// <summary>
        /// to decode the Trans parameters: from the general TransParameters to the concrete Trans Parameters. 
        /// </summary>
        protected override void DecodeTransParameters()
        {
        }


        /// <summary>
        /// to decode the Trans data: from the general TransDada to the concrete Trans Data. 
        /// </summary>
        protected override void DecodeTransData()
        {
        }

        #endregion
    }
}
