// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    ///  Packets for SmbTrans2QueryFsInformationFinal Response
    /// </summary>
    public class SmbTrans2QueryFsInformationFinalResponsePacket : SmbTransaction2FinalResponsePacket
    {
        #region Fields

        private TRANS2_QUERY_FS_INFORMATION_Response_Trans2_Data trans2Data;

        /// <summary>
        /// Indicates that client specifies the information it is requesting. Server return different data based on 
        /// the client's request. 
        /// </summary>
        protected QueryFSInformationLevel informationLevel;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Trans2_Data:TRANS2_QUERY_FS_INFORMATION_Response_Trans2_Data
        /// </summary>
        public TRANS2_QUERY_FS_INFORMATION_Response_Trans2_Data Trans2Data
        {
            get
            {
                return this.trans2Data;
            }
            set
            {
                this.trans2Data = value;
            }
        }
        #endregion


        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SmbTrans2QueryFsInformationFinalResponsePacket()
            : base()
        {
            this.InitDefaultValue();
        }

        
        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbTrans2QueryFsInformationFinalResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbTrans2QueryFsInformationFinalResponsePacket(QueryFSInformationLevel informationLevel)
        {
            this.InitDefaultValue();
            this.informationLevel = informationLevel;
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbTrans2QueryFsInformationFinalResponsePacket(SmbTrans2QueryFsInformationFinalResponsePacket packet)
            : base(packet)
        {
            this.InitDefaultValue();
            this.trans2Data.Data = packet.trans2Data.Data;
            this.informationLevel = packet.informationLevel;
        }

        #endregion


        #region override methods

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this.</returns>
        public override StackPacket Clone()
        {
            return new SmbTrans2QueryFsInformationFinalResponsePacket(this);
        }


        /// <summary>
        /// Encode the struct of Trans2Parameters into the byte array in SmbData.Trans2_Parameters
        /// </summary>
        protected override void EncodeTrans2Parameters()
        {
        }


        /// <summary>
        /// Encode the struct of Trans2Data into the byte array in SmbData.Trans2_Data
        /// </summary>
        protected override void EncodeTrans2Data()
        {
            if (this.trans2Data.Data != null)
            {
                Type type = this.trans2Data.Data.GetType();

                if (type == typeof(SMB_INFO_ALLOCATION))
                {
                    this.smbData.Trans2_Data = CifsMessageUtils.ToBytes<SMB_INFO_ALLOCATION>(
                        (SMB_INFO_ALLOCATION)this.trans2Data.Data);
                }

                else if (type == typeof(SMB_INFO_VOLUME))
                {
                    this.smbData.Trans2_Data = CifsMessageUtils.ToBytes<SMB_INFO_VOLUME>(
                        (SMB_INFO_VOLUME)this.trans2Data.Data);
                }

                else if (type == typeof(SMB_QUERY_FS_VOLUME_INFO))
                {
                    this.smbData.Trans2_Data = CifsMessageUtils.ToBytes<SMB_QUERY_FS_VOLUME_INFO>(
                        (SMB_QUERY_FS_VOLUME_INFO)this.trans2Data.Data);
                }

                else if (type == typeof(SMB_QUERY_FS_SIZE_INFO))
                {
                    this.smbData.Trans2_Data = CifsMessageUtils.ToBytes<SMB_QUERY_FS_SIZE_INFO>(
                        (SMB_QUERY_FS_SIZE_INFO)this.trans2Data.Data);
                }

                else if (type == typeof(SMB_QUERY_FS_DEVICE_INFO))
                {
                    this.smbData.Trans2_Data = CifsMessageUtils.ToBytes<SMB_QUERY_FS_DEVICE_INFO>(
                        (SMB_QUERY_FS_DEVICE_INFO)this.trans2Data.Data);
                }

                else if (type == typeof(SMB_QUERY_FS_ATTRIBUTE_INFO))
                {
                    this.smbData.Trans2_Data = CifsMessageUtils.ToBytes<SMB_QUERY_FS_ATTRIBUTE_INFO>(
                        (SMB_QUERY_FS_ATTRIBUTE_INFO)this.trans2Data.Data
                              );
                }

                else
                {
                    // Branch for negative testing.
                }
            }
            else
            {
                this.smbData.Trans2_Data = new byte[0];
            }
        }


        /// <summary>
        /// to decode the Trans2 parameters: from the general Trans2Parameters to the concrete Trans2 Parameters.
        /// </summary>
        protected override void DecodeTrans2Parameters()
        {
        }


        /// <summary>
        /// to decode the Trans2 data: from the general Trans2Dada to the concrete Trans2 Data.
        /// </summary>
        protected override void DecodeTrans2Data()
        {
            if (this.smbData.Trans2_Data != null && this.smbData.Trans2_Data.Length > 0)
            {
                switch (this.informationLevel)
                {
                    case QueryFSInformationLevel.SMB_INFO_ALLOCATION:
                        this.trans2Data.Data = TypeMarshal.ToStruct<SMB_INFO_ALLOCATION>(this.smbData.Trans2_Data);
                        break;

                    case QueryFSInformationLevel.SMB_INFO_VOLUME:
                        this.trans2Data.Data = TypeMarshal.ToStruct<SMB_INFO_VOLUME>(this.smbData.Trans2_Data);
                        break;

                    case QueryFSInformationLevel.SMB_QUERY_FS_VOLUME_INFO:
                        this.trans2Data.Data = TypeMarshal.ToStruct<SMB_QUERY_FS_VOLUME_INFO>(this.smbData.Trans2_Data);
                        break;

                    case QueryFSInformationLevel.SMB_QUERY_FS_SIZE_INFO:
                        this.trans2Data.Data = TypeMarshal.ToStruct<SMB_QUERY_FS_SIZE_INFO>(this.smbData.Trans2_Data);
                        break;

                    case QueryFSInformationLevel.SMB_QUERY_FS_DEVICE_INFO:
                        this.trans2Data.Data = TypeMarshal.ToStruct<SMB_QUERY_FS_DEVICE_INFO>(this.smbData.Trans2_Data);
                        break;

                    case QueryFSInformationLevel.SMB_QUERY_FS_ATTRIBUTE_INFO:
                        this.trans2Data.Data = TypeMarshal.ToStruct<SMB_QUERY_FS_ATTRIBUTE_INFO>(
                            this.smbData.Trans2_Data);
                        break;
                }
            }
        }

        #endregion


        #region initialize fields with default value

        /// <summary>
        /// init packet, set default field data
        /// </summary>
        private void InitDefaultValue()
        {
        }

        #endregion
    }
}