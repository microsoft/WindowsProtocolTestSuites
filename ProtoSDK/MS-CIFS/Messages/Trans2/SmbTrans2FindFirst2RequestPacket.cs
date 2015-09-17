// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbTrans2FindFirst2 Request
    /// </summary>
    public class SmbTrans2FindFirst2RequestPacket : SmbTransaction2RequestPacket
    {
        #region Fields

        private TRANS2_FIND_FIRST2_Request_Trans2_Parameters trans2Parameters;
        private TRANS2_FIND_FIRST2_Request_Trans2_Data trans2Data;
        /// <summary>
        /// The size of SizeOfListInBytes field in trans2Data
        /// </summary>
        private const ushort sizeOfListInBytesLength = 4;

        /// <summary>
        /// This is a fixed length including length of SearchAttributes, SearchCount, Flags, InformationLevel,
        /// and SearchStorageType.
        /// </summary>
        private const ushort trans2ParametersLength = 12;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Trans2_Parameters:TRANS2_FIND_FIRST2_Request_Trans2_Parameters
        /// </summary>
        public TRANS2_FIND_FIRST2_Request_Trans2_Parameters Trans2Parameters
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
        /// get or set the Trans2_Data:TRANS2_FIND_FIRST2_Request_Trans2_Data
        /// </summary>
        public TRANS2_FIND_FIRST2_Request_Trans2_Data Trans2Data
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


        /// <summary>
        /// get the FID of Trans2_Parameters
        /// </summary>
        internal override ushort FID
        {
            get
            {
                return INVALID_FID;
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SmbTrans2FindFirst2RequestPacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbTrans2FindFirst2RequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbTrans2FindFirst2RequestPacket(SmbTrans2FindFirst2RequestPacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

            this.trans2Parameters.SearchAttributes = packet.trans2Parameters.SearchAttributes;
            this.trans2Parameters.SearchCount = packet.trans2Parameters.SearchCount;
            this.trans2Parameters.Flags = packet.trans2Parameters.Flags;
            this.trans2Parameters.InformationLevel = packet.trans2Parameters.InformationLevel;
            this.trans2Parameters.SearchStorageType = packet.trans2Parameters.SearchStorageType;

            if (packet.trans2Parameters.FileName != null)
            {
                this.trans2Parameters.FileName = new byte[packet.trans2Parameters.FileName.Length];
                Array.Copy(packet.trans2Parameters.FileName,
                    this.trans2Parameters.FileName, packet.trans2Parameters.FileName.Length);
            }
            else
            {
                this.trans2Parameters.FileName = new byte[0];
            }
            this.trans2Data.GetExtendedAttributeList.SizeOfListInBytes = 
                packet.trans2Data.GetExtendedAttributeList.SizeOfListInBytes;

            if (packet.trans2Data.GetExtendedAttributeList.GEAList != null)
            {
                this.trans2Data.GetExtendedAttributeList.GEAList = 
                    new SMB_GEA[packet.trans2Data.GetExtendedAttributeList.GEAList.Length];
                Array.Copy(packet.trans2Data.GetExtendedAttributeList.GEAList,
                    this.trans2Data.GetExtendedAttributeList.GEAList,
                    packet.trans2Data.GetExtendedAttributeList.GEAList.Length);
            }
            else
            {
                this.trans2Data.GetExtendedAttributeList.GEAList = new SMB_GEA[0];
            }
        }

        #endregion


        #region override methods

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this.</returns>
        public override StackPacket Clone()
        {
            return new SmbTrans2FindFirst2RequestPacket(this);
        }


        /// <summary>
        /// Encode the struct of Trans2Parameters into the byte array in SmbData.Trans2_Parameters
        /// </summary>
        protected override void EncodeTrans2Parameters()
        {
            int trans2ParametersCount = trans2ParametersLength;

            if (this.trans2Parameters.FileName != null)
            {
                trans2ParametersCount += this.trans2Parameters.FileName.Length;
            }
            this.smbData.Trans2_Parameters = new byte[trans2ParametersCount];
            using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans2_Parameters))
            {
                using (Channel channel = new Channel(null, memoryStream))
                {
                    channel.BeginWriteGroup();
                    channel.Write<SmbFileAttributes>(this.trans2Parameters.SearchAttributes);
                    channel.Write<ushort>(this.trans2Parameters.SearchCount);
                    channel.Write<Trans2FindFlags>(this.trans2Parameters.Flags);
                    channel.Write<FindInformationLevel>(this.trans2Parameters.InformationLevel);
                    channel.Write<Trans2FindFirst2SearchStorageType>(this.trans2Parameters.SearchStorageType);

                    if (this.trans2Parameters.FileName != null)
                    {
                        channel.WriteBytes(this.trans2Parameters.FileName);
                    }
                    channel.EndWriteGroup();
                }
            }
        }


        /// <summary>
        /// Encode the struct of Trans2Data into the byte array in SmbData.Trans2_Data
        /// </summary>
        protected override void EncodeTrans2Data()
        {
            if (this.trans2Parameters.InformationLevel == FindInformationLevel.SMB_INFO_QUERY_EAS_FROM_LIST)
            {
                this.smbData.Trans2_Data = new byte[sizeOfListInBytesLength + CifsMessageUtils.GetSmbQueryEAListSize(
                    this.trans2Data.GetExtendedAttributeList.GEAList)];
                using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans2_Data))
                {
                    using (Channel channel = new Channel(null, memoryStream))
                    {
                        channel.BeginWriteGroup();
                        channel.Write<uint>(this.trans2Data.GetExtendedAttributeList.SizeOfListInBytes);

                        if (this.trans2Data.GetExtendedAttributeList.GEAList != null)
                        {
                            foreach (SMB_GEA smbQueryEa in this.trans2Data.GetExtendedAttributeList.GEAList)
                            {
                                channel.Write<byte>(smbQueryEa.AttributeNameLengthInBytes);

                                if (smbQueryEa.AttributeName != null)
                                {
                                    channel.WriteBytes(smbQueryEa.AttributeName);
                                }
                            }
                        }
                        channel.EndWriteGroup();
                    }
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
            using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans2_Parameters))
            {
                using (Channel channel = new Channel(null, memoryStream))
                {
                    this.trans2Parameters.SearchAttributes = channel.Read<SmbFileAttributes>();
                    this.trans2Parameters.SearchCount = channel.Read<ushort>();
                    this.trans2Parameters.Flags = channel.Read<Trans2FindFlags>();
                    this.trans2Parameters.InformationLevel = channel.Read<FindInformationLevel>();
                    this.trans2Parameters.SearchStorageType = channel.Read<Trans2FindFirst2SearchStorageType>();
                    this.trans2Parameters.FileName = channel.ReadBytes(this.SmbParameters.ParameterCount
                        - trans2ParametersLength);
                }
            }
        }


        /// <summary>
        /// to decode the Trans2 data: from the general Trans2Dada to the concrete Trans2 Data.
        /// </summary>
        protected override void DecodeTrans2Data()
        {
            if (this.trans2Parameters.InformationLevel == FindInformationLevel.SMB_INFO_QUERY_EAS_FROM_LIST)
            {
                using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans2_Data))
                {
                    using (Channel channel = new Channel(null, memoryStream))
                    {
                        this.trans2Data.GetExtendedAttributeList.SizeOfListInBytes = channel.Read<uint>();
                        uint sizeOfListInBytes = 
                            this.trans2Data.GetExtendedAttributeList.SizeOfListInBytes - sizeOfListInBytesLength;
                        List<SMB_GEA> attributeList = new List<SMB_GEA>();

                        while (sizeOfListInBytes > 0)
                        {
                            SMB_GEA smbQueryEa = channel.Read<SMB_GEA>();
                            attributeList.Add(smbQueryEa);
                            sizeOfListInBytes -= (uint)(EA.SMB_QUERY_EA_FIXED_SIZE + smbQueryEa.AttributeName.Length);
                        }
                        this.trans2Data.GetExtendedAttributeList.GEAList = attributeList.ToArray();
                    }
                }
            }
            else
            {
                this.trans2Data.GetExtendedAttributeList.GEAList = new SMB_GEA[0];
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