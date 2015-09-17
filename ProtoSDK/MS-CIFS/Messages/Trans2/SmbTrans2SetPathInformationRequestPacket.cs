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
    ///  Packets for SmbTrans2SetPathInformation Request
    /// </summary>
    public class SmbTrans2SetPathInformationRequestPacket : SmbTransaction2RequestPacket
    {
        #region Fields

        private TRANS2_SET_PATH_INFORMATION_Request_Trans2_Parameters trans2Parameters;
        private TRANS2_SET_PATH_INFORMATION_Request_Trans2_Data trans2Data;
        /// <summary>
        /// The size of InformationLevel field in trans2Parameters
        /// </summary>
        private const ushort infoLevelLength = 2;
        /// <summary>
        /// The size of Reserved field in trans2Parameters
        /// </summary>
        private const ushort reservedLength = 4;
        /// <summary>
        /// The size of SizeOfListInBytes field in trans2Data
        /// </summary>
        private const ushort sizeOfListInBytesLength = 4;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Trans2_Parameters:TRANS2_SET_PATH_INFORMATION_Request_Trans2_Parameters
        /// </summary>
        public TRANS2_SET_PATH_INFORMATION_Request_Trans2_Parameters Trans2Parameters
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
        /// get or set the Trans2_Data:TRANS2_SET_PATH_INFORMATION_Request_Trans2_Data
        /// </summary>
        public TRANS2_SET_PATH_INFORMATION_Request_Trans2_Data Trans2Data
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
        public SmbTrans2SetPathInformationRequestPacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbTrans2SetPathInformationRequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbTrans2SetPathInformationRequestPacket(SmbTrans2SetPathInformationRequestPacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

            this.trans2Parameters.InformationLevel = packet.trans2Parameters.InformationLevel;
            this.trans2Parameters.Reserved = packet.trans2Parameters.Reserved;

            if (packet.trans2Parameters.FileName != null)
            {
                this.trans2Parameters.FileName = new byte[packet.trans2Parameters.FileName.Length];
                Array.Copy(packet.trans2Parameters.FileName,
                    this.trans2Parameters.FileName, packet.trans2Parameters.FileName.Length);
            }

            this.trans2Data.Data = packet.trans2Data.Data;
        }

        #endregion


        #region override methods

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this.</returns>
        public override StackPacket Clone()
        {
            return new SmbTrans2SetPathInformationRequestPacket(this);
        }


        /// <summary>
        /// Encode the struct of Trans2Parameters into the byte array in SmbData.Trans2_Parameters
        /// </summary>
        protected override void EncodeTrans2Parameters()
        {
            int trans2ParametersCount = infoLevelLength + reservedLength;

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
                    channel.Write<SetInformationLevel>(this.trans2Parameters.InformationLevel);
                    channel.Write<uint>(this.trans2Parameters.Reserved);

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
            if (this.trans2Data.Data != null)
            {
                Type type = this.trans2Data.Data.GetType();

                if (type == typeof(SMB_INFO_STANDARD_OF_TRANS2_SET_PATH_INFORMATION))
                {
                    this.smbData.Trans2_Data = CifsMessageUtils.ToBytes
                        <SMB_INFO_STANDARD_OF_TRANS2_SET_PATH_INFORMATION>(
                        (SMB_INFO_STANDARD_OF_TRANS2_SET_PATH_INFORMATION)this.trans2Data.Data);
                }
                else if (type == typeof(SMB_INFO_SET_EAS))
                {
                    SMB_INFO_SET_EAS data = (SMB_INFO_SET_EAS)this.trans2Data.Data;
                    this.smbData.Trans2_Data = new byte[sizeOfListInBytesLength + CifsMessageUtils.GetSmbEAListSize(
                        data.ExtendedAttributeList)];
                    using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans2_Data))
                    {
                        using (Channel channel = new Channel(null, memoryStream))
                        {
                            channel.BeginWriteGroup();
                            channel.Write<uint>(data.SizeOfListInBytes);

                            if (data.ExtendedAttributeList != null)
                            {
                                foreach (SMB_FEA smbEa in data.ExtendedAttributeList)
                                {
                                    channel.Write<byte>(smbEa.ExtendedAttributeFlag);
                                    channel.Write<byte>(smbEa.AttributeNameLengthInBytes);
                                    channel.Write<ushort>(smbEa.ValueNameLengthInBytes);

                                    if (smbEa.AttributeName != null)
                                    {
                                        channel.WriteBytes(smbEa.AttributeName);
                                    }
                                    if (smbEa.ValueName != null)
                                    {
                                        channel.WriteBytes(smbEa.ValueName);
                                    }
                                }
                            }
                            channel.EndWriteGroup();
                        }
                    }
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
            using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans2_Parameters))
            {
                using (Channel channel = new Channel(null, memoryStream))
                {
                    this.trans2Parameters.InformationLevel = channel.Read<SetInformationLevel>();
                    this.trans2Parameters.Reserved = channel.Read<uint>();
                    this.trans2Parameters.FileName = channel.ReadBytes(this.smbParameters.ParameterCount - infoLevelLength
                        - reservedLength);
                }
            }
        }


        /// <summary>
        /// to decode the Trans2 data: from the general Trans2Dada to the concrete Trans2 Data.
        /// </summary>
        protected override void DecodeTrans2Data()
        {
            if (this.smbData.Trans2_Data != null && this.smbData.Trans2_Data.Length > 0)
            {
                switch (this.trans2Parameters.InformationLevel)
                {
                    case SetInformationLevel.SMB_INFO_STANDARD:
                        this.trans2Data.Data = CifsMessageUtils.ToStuct
                               <SMB_INFO_STANDARD_OF_TRANS2_SET_PATH_INFORMATION>(this.smbData.Trans2_Data);
                        break;

                    case SetInformationLevel.SMB_INFO_SET_EAS:
                        using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans2_Data))
                        {
                            using (Channel channel = new Channel(null, memoryStream))
                            {
                                SMB_INFO_SET_EAS data = new SMB_INFO_SET_EAS();
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