// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbReadRaw Response
    /// </summary>
    public class SmbReadRawResponsePacket : SmbSingleResponsePacket 
    {
        #region Fields

        private byte[] rawData;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the raw data
        /// </summary>
        public byte[] RawData
        {
            get
            {
                return this.rawData;
            }
            set
            {
                this.rawData = value;
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SmbReadRawResponsePacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbReadRawResponsePacket(byte[] data)
            : base(data)
        {
            this.rawData = data;
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbReadRawResponsePacket(SmbReadRawResponsePacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

            if (packet.RawData != null)
            {
                this.rawData = new byte[packet.rawData.Length];
                Array.Copy(packet.rawData, this.rawData, packet.rawData.Length);
            }
        }

        #endregion


        #region override methods

        /// <summary>
        /// to marshal the packet struct to bytes array.
        /// </summary>
        /// <returns>the bytes array of the packet.</returns>
        public override byte[] ToBytes()
        {
            return this.rawData;
        }


        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this.</returns>
        public override StackPacket Clone()
        {
            return new SmbReadRawResponsePacket(this);
        }


        /// <summary>
        /// Encode the struct of SMB_COM_READ_MPX_Response_SMB_Parameters into the struct of SmbParameters
        /// </summary>
        protected override void EncodeParameters()
        {

        }


        /// <summary>
        /// Encode the struct of SMB_COM_READ_MPX_Response_SMB_Data into the struct of SmbData
        /// </summary>
        protected override void EncodeData()
        {

        }


        /// <summary>
        /// to decode the smb parameters: from the general SmbParameters to the concrete Smb Parameters.
        /// </summary>
        protected override void DecodeParameters()
        {

        }


        /// <summary>
        /// to decode the smb data: from the general SmbDada to the concrete Smb Data.
        /// </summary>
        protected override void DecodeData()
        {

        }

        #endregion


        #region initialize fields with default value

        /// <summary>
        /// init packet, set default field data
        /// </summary>
        private void InitDefaultValue()
        {
            this.rawData = new byte[0];
        }

        #endregion
    }
}