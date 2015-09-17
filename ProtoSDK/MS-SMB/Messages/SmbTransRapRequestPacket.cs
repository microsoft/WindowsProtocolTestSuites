// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.Protocols.TestTools.StackSdk.Messages;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// rap packet. 
    /// </summary>
    public class SmbTransRapRequestPacket : SmbTransactionRequestPacket
    {
        #region Fields

        private TRANSACTION_Rap_Request_Trans_Parameters transParameters;
        private TRANSACTION_Rap_Request_Trans_Data transData;

        #endregion

        #region Properties

        /// <summary>
        /// get or set the transParameters:TRANSACTION_Rap_Request_Trans_Parameters 
        /// </summary>
        public TRANSACTION_Rap_Request_Trans_Parameters TransParameters
        {
            get
            {
                return this.transParameters;
            }
            set
            {
                this.transParameters = value;
            }
        }


        /// <summary>
        /// get or set the transData: TRANSACTION_Rap_Request_Trans_Data 
        /// </summary>
        public TRANSACTION_Rap_Request_Trans_Data TransData
        {
            get
            {
                return this.transData;
            }
            set
            {
                this.transData = value;
            }
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbTransRapRequestPacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbTransRapRequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbTransRapRequestPacket(SmbTransRapRequestPacket packet)
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
            return new SmbTransRapRequestPacket(this);
        }


        /// <summary>
        /// Encode the struct of TransParameters into the byte array in SmbData.Trans_Parameters 
        /// </summary>
        protected override void EncodeTransParameters()
        {
            List<byte> bytes = new List<byte>();

            bytes.AddRange(TypeMarshal.ToBytes<ushort>(this.transParameters.RapOPCode));
            if (this.transParameters.ParamDesc != null)
            {
                bytes.AddRange(this.transParameters.ParamDesc);
            }
            if (this.transParameters.DataDesc != null)
            {
                bytes.AddRange(this.transParameters.DataDesc);
            }
            if (this.transParameters.RAPParamsAndAuxDesc != null)
            {
                bytes.AddRange(this.transParameters.RAPParamsAndAuxDesc);
            }

            this.smbData.Trans_Parameters = bytes.ToArray();
        }


        /// <summary>
        /// Encode the struct of TransData into the byte array in SmbData.Trans_Data 
        /// </summary>
        protected override void EncodeTransData()
        {
            this.smbData.Trans_Data = this.transData.RAPInData;
        }


        /// <summary>
        /// to decode the Trans parameters: from the general TransParameters to the concrete Trans Parameters. 
        /// </summary>
        protected override void DecodeTransParameters()
        {
            if (this.smbData.Trans_Parameters == null || this.smbData.Trans_Parameters.Length == 0)
            {
                return;
            }

            TRANSACTION_Rap_Request_Trans_Parameters param = new TRANSACTION_Rap_Request_Trans_Parameters();
            using (MemoryStream stream = new MemoryStream(this.smbData.Trans_Parameters))
            {
                using (Channel channel = new Channel(null, stream))
                {
                    param.RapOPCode = channel.Read<ushort>();
                    // the ParamDesc and DataDesc size
                    int descSize = this.smbData.Trans_Parameters.Length - SmbCapability.NUM_BYTES_OF_WORD;
                    if (descSize > 0)
                    {
                        byte[] descs = channel.ReadBytes(descSize);
                        int start = 0;
                        int index = 0;

                        // read the field ParamDesc.
                        if ((index = FindEndIndexOfAsciiString(descs, start)) != -1)
                        {
                            // move to the terminated descriptor.
                            index++;
                            // get the data
                            param.ParamDesc = ArrayUtility.SubArray<byte>(descs, start, index - start);
                            // update the start
                            start = index ;
                        }

                        // read the field DataDesc.
                        if ((index = FindEndIndexOfAsciiString(descs, start)) != -1)
                        {
                            // move to the terminated descriptor.
                            index++;
                            // get the data
                            param.DataDesc = ArrayUtility.SubArray<byte>(descs, start, index - start);
                            // update the start
                            start = index ;
                        }

                        // read the field RAPParamsAndAuxDesc.
                        if (descSize > start)
                        {
                            param.RAPParamsAndAuxDesc = ArrayUtility.SubArray<byte>(descs, start, descSize - start);
                        }
                    }
                }
            }
            this.transParameters = param;
        }


        /// <summary>
        /// find the end index of ASCII string which is end with terminated descriptor.
        /// </summary>
        /// <param name="data">the data contains the strings</param>
        /// <param name="start">the start search index</param>
        /// <returns>the end index of descriptor</returns>
        private int FindEndIndexOfAsciiString(byte[] data, int start)
        {
            int descSize = data.Length;
            for (int index = start; index < descSize; index++)
            {
                if (data[index] == 0)
                {
                    return index;
                }
            }

            return -1;
        }


        /// <summary>
        /// to decode the Trans data: from the general TransDada to the concrete Trans Data. 
        /// </summary>
        protected override void DecodeTransData()
        {
            this.transData.RAPInData = this.smbData.Trans_Data;
        }


        #endregion
    }
}
