// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Protocols.TestTools.StackSdk.Messages;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbTrans2QueryPathInformationFinal Response
    /// </summary>
    public class SmbTrans2QueryPathInformationFinalResponsePacket : SmbTransaction2FinalResponsePacket
    {
        #region Fields

        private TRANS2_QUERY_PATH_INFORMATION_Response_Trans2_Parameters trans2Parameters;
        private TRANS2_QUERY_PATH_INFORMATION_Response_Trans2_Data trans2Data;

        /// <summary>
        /// Indicates that client specifies the information it is requesting. Server return different data based on 
        /// the client's request. 
        /// </summary>
        protected QueryInformationLevel informationLevel;

        /// <summary>
        /// The size of SizeOfListInBytes field in trans2Data
        /// </summary>
        private const ushort sizeOfListInBytesLength = 4;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Trans2_Parameters:TRANS2_QUERY_PATH_INFORMATION_Response_Trans2_Parameters
        /// </summary>
        public TRANS2_QUERY_PATH_INFORMATION_Response_Trans2_Parameters Trans2Parameters
        {
            get
            {
                return this.trans2Parameters;
            }
            set
            {
                this.trans2Parameters = value;
            }
        }


        /// <summary>
        /// get or set the Trans2_Data:TRANS2_QUERY_PATH_INFORMATION_Response_Trans2_Data
        /// </summary>
        public TRANS2_QUERY_PATH_INFORMATION_Response_Trans2_Data Trans2Data
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
        public SmbTrans2QueryPathInformationFinalResponsePacket()
            : base()
        {
            this.InitDefaultValue();
        }

        
        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbTrans2QueryPathInformationFinalResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        /// <param name="informationLevel">The response format is controlled by the value of the InformationLevel
        /// field provided in the request.</param>
        public SmbTrans2QueryPathInformationFinalResponsePacket(QueryInformationLevel informationLevel)
        {
            this.InitDefaultValue();
            this.informationLevel = informationLevel;
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbTrans2QueryPathInformationFinalResponsePacket(
                SmbTrans2QueryPathInformationFinalResponsePacket packet)
            : base(packet)
        {
            this.InitDefaultValue();
            this.trans2Parameters.EaErrorOffset = packet.trans2Parameters.EaErrorOffset;
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
            return new SmbTrans2QueryPathInformationFinalResponsePacket(this);
        }


        /// <summary>
        /// Encode the struct of Trans2Parameters into the byte array in SmbData.Trans2_Parameters
        /// </summary>
        protected override void EncodeTrans2Parameters()
        {
            this.smbData.Trans2_Parameters =
                CifsMessageUtils.ToBytes<TRANS2_QUERY_PATH_INFORMATION_Response_Trans2_Parameters>(
                this.trans2Parameters);
        }


        /// <summary>
        /// Encode the struct of Trans2Data into the byte array in SmbData.Trans2_Data
        /// </summary>
        protected override void EncodeTrans2Data()
        {
            if (this.trans2Data.Data != null)
            {
                Type type = this.trans2Data.Data.GetType();

                if (type == typeof(SMB_INFO_STANDARD_OF_TRANS2_QUERY_PATH_INFORMATION))
                {
                    this.smbData.Trans2_Data = CifsMessageUtils.ToBytes<
                        SMB_INFO_STANDARD_OF_TRANS2_QUERY_PATH_INFORMATION>(
                        (SMB_INFO_STANDARD_OF_TRANS2_QUERY_PATH_INFORMATION)this.trans2Data.Data);
                }

                else if (type == typeof(SMB_INFO_QUERY_EA_SIZE_OF_TRANS2_QUERY_PATH_INFORMATION))
                {
                    this.smbData.Trans2_Data = CifsMessageUtils.ToBytes<
                        SMB_INFO_QUERY_EA_SIZE_OF_TRANS2_QUERY_PATH_INFORMATION>(
                        (SMB_INFO_QUERY_EA_SIZE_OF_TRANS2_QUERY_PATH_INFORMATION)this.trans2Data.Data);
                }

                else if (type == typeof(SMB_INFO_QUERY_EAS_FROM_LIST_OF_TRANS2_QUERY_PATH_INFORMATION))
                {
                    SMB_INFO_QUERY_EAS_FROM_LIST_OF_TRANS2_QUERY_PATH_INFORMATION data =
                        (SMB_INFO_QUERY_EAS_FROM_LIST_OF_TRANS2_QUERY_PATH_INFORMATION)this.trans2Data.Data;

                    if (data.ExtendedAttributeList != null)
                    {
                        this.smbData.Trans2_Data = new byte[data.SizeOfListInBytes];
                        using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans2_Data))
                        {
                            using (Channel channel = new Channel(null, memoryStream))
                            {
                                channel.BeginWriteGroup();
                                channel.Write<uint>(data.SizeOfListInBytes);

                                foreach (SMB_FEA smbEa in data.ExtendedAttributeList)
                                {
                                    channel.Write<SMB_FEA>(smbEa);
                                }
                                channel.EndWriteGroup();
                            }
                        }
                    }
                }

                else if (type == typeof(SMB_INFO_QUERY_ALL_EAS_OF_TRANS2_QUERY_PATH_INFORMATION))
                {
                    SMB_INFO_QUERY_ALL_EAS_OF_TRANS2_QUERY_PATH_INFORMATION easData =
                        (SMB_INFO_QUERY_ALL_EAS_OF_TRANS2_QUERY_PATH_INFORMATION)this.trans2Data.Data;

                    if (easData.ExtendedAttributeList != null)
                    {
                        this.smbData.Trans2_Data = new byte[easData.SizeOfListInBytes];
                        using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans2_Data))
                        {
                            using (Channel channel = new Channel(null, memoryStream))
                            {
                                channel.BeginWriteGroup();
                                channel.Write<uint>(easData.SizeOfListInBytes);

                                foreach (SMB_FEA smbEa in easData.ExtendedAttributeList)
                                {
                                    channel.Write<SMB_FEA>(smbEa);
                                }
                                channel.EndWriteGroup();
                            }
                        }
                    }
                }

                else if (type == typeof(SMB_QUERY_FILE_BASIC_INFO_OF_TRANS2_QUERY_PATH_INFORMATION))
                {
                    this.smbData.Trans2_Data = CifsMessageUtils.ToBytes<
                        SMB_QUERY_FILE_BASIC_INFO_OF_TRANS2_QUERY_PATH_INFORMATION>(
                        (SMB_QUERY_FILE_BASIC_INFO_OF_TRANS2_QUERY_PATH_INFORMATION)this.trans2Data.Data);
                }

                else if (type == typeof(SMB_QUERY_FILE_STANDARD_INFO_OF_TRANS2_QUERY_PATH_INFORMATION))
                {
                    this.smbData.Trans2_Data = CifsMessageUtils.ToBytes<
                        SMB_QUERY_FILE_STANDARD_INFO_OF_TRANS2_QUERY_PATH_INFORMATION>(
                        (SMB_QUERY_FILE_STANDARD_INFO_OF_TRANS2_QUERY_PATH_INFORMATION)this.trans2Data.Data);
                }

                else if (type == typeof(SMB_QUERY_FILE_EA_INFO_OF_TRANS2_QUERY_PATH_INFORMATION))
                {
                    this.smbData.Trans2_Data = CifsMessageUtils.ToBytes<
                        SMB_QUERY_FILE_EA_INFO_OF_TRANS2_QUERY_PATH_INFORMATION>(
                        (SMB_QUERY_FILE_EA_INFO_OF_TRANS2_QUERY_PATH_INFORMATION)this.trans2Data.Data);
                }

                else if (type == typeof(SMB_QUERY_FILE_NAME_INFO_OF_TRANS2_QUERY_PATH_INFORMATION))
                {
                    this.smbData.Trans2_Data = CifsMessageUtils.ToBytes<
                        SMB_QUERY_FILE_NAME_INFO_OF_TRANS2_QUERY_PATH_INFORMATION>(
                        (SMB_QUERY_FILE_NAME_INFO_OF_TRANS2_QUERY_PATH_INFORMATION)this.trans2Data.Data);
                }

                else if (type == typeof(SMB_QUERY_FILE_ALL_INFO_OF_TRANS2_QUERY_PATH_INFORMATION))
                {
                    this.smbData.Trans2_Data = CifsMessageUtils.ToBytes<
                        SMB_QUERY_FILE_ALL_INFO_OF_TRANS2_QUERY_PATH_INFORMATION>(
                        (SMB_QUERY_FILE_ALL_INFO_OF_TRANS2_QUERY_PATH_INFORMATION)this.trans2Data.Data);
                }

                else if (type == typeof(SMB_QUERY_FILE_ALT_NAME_INFO_OF_TRANS2_QUERY_PATH_INFORMATION))
                {
                    this.smbData.Trans2_Data = CifsMessageUtils.ToBytes<
                        SMB_QUERY_FILE_ALT_NAME_INFO_OF_TRANS2_QUERY_PATH_INFORMATION>(
                        (SMB_QUERY_FILE_ALT_NAME_INFO_OF_TRANS2_QUERY_PATH_INFORMATION)this.trans2Data.Data);
                }

                else if (type == typeof(SMB_QUERY_FILE_STREAM_INFO_OF_TRANS2_QUERY_PATH_INFORMATION))
                {
                    this.smbData.Trans2_Data = CifsMessageUtils.ToBytes<
                        SMB_QUERY_FILE_STREAM_INFO_OF_TRANS2_QUERY_PATH_INFORMATION>(
                        (SMB_QUERY_FILE_STREAM_INFO_OF_TRANS2_QUERY_PATH_INFORMATION)this.trans2Data.Data);
                }

                else if (type == typeof(SMB_QUERY_FILE_COMPRESSION_INFO_OF_TRANS2_QUERY_PATH_INFORMATION))
                {
                    this.smbData.Trans2_Data = CifsMessageUtils.ToBytes<
                        SMB_QUERY_FILE_COMPRESSION_INFO_OF_TRANS2_QUERY_PATH_INFORMATION>(
                        (SMB_QUERY_FILE_COMPRESSION_INFO_OF_TRANS2_QUERY_PATH_INFORMATION)this.trans2Data.Data);
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
            if (this.informationLevel != QueryInformationLevel.SMB_INFO_IS_NAME_VALID
                && this.smbData.Trans2_Parameters != null && this.smbData.Trans2_Parameters.Length > 0)
            {
                this.trans2Parameters = CifsMessageUtils.ToStuct<
                    TRANS2_QUERY_PATH_INFORMATION_Response_Trans2_Parameters>(this.smbData.Trans2_Parameters);
            }
        }


        /// <summary>
        /// to decode the Trans2 data: from the general Trans2Dada to the concrete Trans2 Data.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        protected override void DecodeTrans2Data()
        {
            if (this.smbData.Trans2_Data != null && this.smbData.Trans2_Data.Length > 0)
            {
                switch (this.informationLevel)
                {
                    case QueryInformationLevel.SMB_INFO_STANDARD:
                        this.trans2Data.Data = CifsMessageUtils.ToStuct<
                    SMB_INFO_STANDARD_OF_TRANS2_QUERY_PATH_INFORMATION>(this.smbData.Trans2_Data);
                        break;

                    case QueryInformationLevel.SMB_INFO_QUERY_EA_SIZE:
                        this.trans2Data.Data = CifsMessageUtils.ToStuct<
                    SMB_INFO_QUERY_EA_SIZE_OF_TRANS2_QUERY_PATH_INFORMATION>(this.smbData.Trans2_Data);
                        break;

                    case QueryInformationLevel.SMB_INFO_QUERY_EAS_FROM_LIST:
                        if (this.smbData.Trans2_Data != null && this.smbData.Trans2_Data.Length > 0)
                        {
                            using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans2_Data))
                            {
                                using (Channel channel = new Channel(null, memoryStream))
                                {
                                    SMB_INFO_QUERY_EAS_FROM_LIST_OF_TRANS2_QUERY_PATH_INFORMATION data = new
                                        SMB_INFO_QUERY_EAS_FROM_LIST_OF_TRANS2_QUERY_PATH_INFORMATION();
                                    data.SizeOfListInBytes = channel.Read<uint>();
                                    uint sizeOfListInBytes = data.SizeOfListInBytes - sizeOfListInBytesLength;
                                    List<SMB_FEA> attributeList = new List<SMB_FEA>();

                                    while (sizeOfListInBytes > 0)
                                    {
                                        SMB_FEA smbEa = channel.Read<SMB_FEA>();
                                        attributeList.Add(smbEa);
                                        sizeOfListInBytes -= (uint)(EA.SMB_EA_FIXED_SIZE + smbEa.AttributeName.Length +
                                            smbEa.ValueName.Length);
                                    }
                                    data.ExtendedAttributeList = attributeList.ToArray();
                                    this.trans2Data.Data = data;
                                }
                            }
                        }
                        break;

                    case QueryInformationLevel.SMB_INFO_QUERY_ALL_EAS:
                        if (this.smbData.Trans2_Data != null && this.smbData.Trans2_Data.Length > 0)
                        {
                            using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans2_Data))
                            {
                                using (Channel channel = new Channel(null, memoryStream))
                                {
                                    SMB_INFO_QUERY_ALL_EAS_OF_TRANS2_QUERY_PATH_INFORMATION easData = new
                                        SMB_INFO_QUERY_ALL_EAS_OF_TRANS2_QUERY_PATH_INFORMATION();
                                    easData.SizeOfListInBytes = channel.Read<uint>();
                                    uint sizeOfList = easData.SizeOfListInBytes - sizeOfListInBytesLength;
                                    List<SMB_FEA> eaList = new List<SMB_FEA>();

                                    while (sizeOfList > 0)
                                    {
                                        SMB_FEA smbEa = channel.Read<SMB_FEA>();
                                        eaList.Add(smbEa);
                                        sizeOfList -= (uint)(EA.SMB_EA_FIXED_SIZE + smbEa.AttributeName.Length +
                                            smbEa.ValueName.Length);
                                    }
                                    easData.ExtendedAttributeList = eaList.ToArray();
                                    this.trans2Data.Data = easData;
                                }
                            }
                        }
                        break;

                    case QueryInformationLevel.SMB_QUERY_FILE_BASIC_INFO:
                        this.trans2Data.Data = CifsMessageUtils.ToStuct<
                    SMB_QUERY_FILE_BASIC_INFO_OF_TRANS2_QUERY_PATH_INFORMATION>(this.smbData.Trans2_Data);
                        break;

                    case QueryInformationLevel.SMB_QUERY_FILE_STANDARD_INFO:
                        this.trans2Data.Data = CifsMessageUtils.ToStuct<
                    SMB_QUERY_FILE_STANDARD_INFO_OF_TRANS2_QUERY_PATH_INFORMATION>(this.smbData.Trans2_Data);
                        break;

                    case QueryInformationLevel.SMB_QUERY_FILE_EA_INFO:
                        this.trans2Data.Data = CifsMessageUtils.ToStuct<
                    SMB_QUERY_FILE_EA_INFO_OF_TRANS2_QUERY_PATH_INFORMATION>(this.smbData.Trans2_Data);
                        break;

                    case QueryInformationLevel.SMB_QUERY_FILE_NAME_INFO:
                        this.trans2Data.Data = CifsMessageUtils.ToStuct<
                    SMB_QUERY_FILE_NAME_INFO_OF_TRANS2_QUERY_PATH_INFORMATION>(this.smbData.Trans2_Data);
                        break;

                    case QueryInformationLevel.SMB_QUERY_FILE_ALL_INFO:
                        this.trans2Data.Data = CifsMessageUtils.ToStuct<
                    SMB_QUERY_FILE_ALL_INFO_OF_TRANS2_QUERY_PATH_INFORMATION>(this.smbData.Trans2_Data);
                        break;

                    case QueryInformationLevel.SMB_QUERY_FILE_ALT_NAME_INFO:
                        this.trans2Data.Data = CifsMessageUtils.ToStuct<
                    SMB_QUERY_FILE_ALT_NAME_INFO_OF_TRANS2_QUERY_PATH_INFORMATION>(this.smbData.Trans2_Data);
                        break;

                    case QueryInformationLevel.SMB_QUERY_FILE_STREAM_INFO:
                        this.trans2Data.Data = CifsMessageUtils.ToStuct<
                    SMB_QUERY_FILE_STREAM_INFO_OF_TRANS2_QUERY_PATH_INFORMATION>(this.smbData.Trans2_Data);
                        break;

                    case QueryInformationLevel.SMB_QUERY_FILE_COMPRESSION_INFO:
                        this.trans2Data.Data = CifsMessageUtils.ToStuct<
                    SMB_QUERY_FILE_COMPRESSION_INFO_OF_TRANS2_QUERY_PATH_INFORMATION>(this.smbData.Trans2_Data);
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