// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbTrans2GetDfsReferalFinal Response
    /// </summary>
    public class SmbTrans2GetDfsReferalFinalResponsePacket : SmbTransaction2FinalResponsePacket
    {
        #region Fields

        private TRANS2_GET_DFS_REFERRAL_Response_Trans2_Data trans2Data;
        private const ushort referralHeaderSize = 8;
        private const ushort referralV1FixedSize = 8;
        private const ushort referralV2FixedSize = 22;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Trans2_Data:TRANS2_GET_DFS_REFERRAL_Response_Trans2_Data
        /// </summary>
        public TRANS2_GET_DFS_REFERRAL_Response_Trans2_Data Trans2Data
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
        public SmbTrans2GetDfsReferalFinalResponsePacket()
            : base()
        {
            this.InitDefaultValue();
        }

        
        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbTrans2GetDfsReferalFinalResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbTrans2GetDfsReferalFinalResponsePacket(SmbTrans2GetDfsReferalFinalResponsePacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

            this.trans2Data.ReferralResponse = packet.trans2Data.ReferralResponse;
        }

        #endregion


        #region Methods

        /// <summary>
        /// Update the offset of paths
        /// </summary>
        public void UpdatePathOffset()
        {
            DFS_REFERRAL_V2[] referral2List = this.trans2Data.ReferralResponse.ReferralEntries as DFS_REFERRAL_V2[];

            if (referral2List != null)
            {
                int pathOffset = 0;
                ushort wordLength = 2;
                ushort nullLength = 1;

                for (int i = 0; i < referral2List.Length; i++)
                {
                    int currentFerralFixedOffset = (referral2List.Length - i) * referralV2FixedSize;
                    referral2List[i].DFSPathOffset = (ushort)(currentFerralFixedOffset + pathOffset);
                    pathOffset += (referral2List[i].DFSPath.Length + nullLength) * wordLength;
                    referral2List[i].DFSAlternatePathOffset = (ushort)(currentFerralFixedOffset + pathOffset);
                    pathOffset += (referral2List[i].DFSAlternatePath.Length + nullLength) * wordLength;
                    referral2List[i].NetworkAddressOffset = (ushort)(currentFerralFixedOffset + pathOffset);
                    pathOffset += (referral2List[i].DFSTargetPath.Length + nullLength) * wordLength;
                }
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
            return new SmbTrans2GetDfsReferalFinalResponsePacket(this);
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
            int totalSize = referralHeaderSize;
            DFS_REFERRAL_V1[] referral1List = this.trans2Data.ReferralResponse.ReferralEntries as DFS_REFERRAL_V1[];
            DFS_REFERRAL_V2[] referral2List = this.trans2Data.ReferralResponse.ReferralEntries as DFS_REFERRAL_V2[];

            if (referral1List != null)
            {
                for (int i = 0; i < referral1List.Length; i++)
                {
                    totalSize += referralV1FixedSize + referral1List[i].ShareName.Length;
                }
            }
            else if (referral2List != null)
            {
                ushort wordLength = 2;
                ushort nullLength = 3;

                for (int i = 0; i < referral2List.Length; i++)
                {
                    totalSize += referralV2FixedSize + wordLength * (referral2List[i].DFSPath.Length
                         + referral2List[i].DFSAlternatePath.Length + referral2List[i].DFSTargetPath.Length + nullLength);
                }
            }
            else
            {
                // Branch for negative testing.
            }

            this.smbData.Trans2_Data = new byte[totalSize];
            using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans2_Data))
            {
                using (Channel channel = new Channel(null, memoryStream))
                {
                    channel.BeginWriteGroup();
                    channel.Write<ushort>(this.trans2Data.ReferralResponse.PathConsumed);
                    channel.Write<ushort>(this.trans2Data.ReferralResponse.NumberOfReferrals);
                    channel.Write<ReferralHeaderFlags>(this.trans2Data.ReferralResponse.ReferralHeaderFlags);

                    if (referral1List != null)
                    {
                        for (int i = 0; i < referral1List.Length; i++)
                        {
                            channel.Write<DFS_REFERRAL_V1>(referral1List[i]);
                        }
                    }
                    else if (referral2List != null)
                    {
                        for (int i = 0; i < referral2List.Length; i++)
                        {
                            channel.Write<ushort>(referral2List[i].VersionNumber);
                            channel.Write<ushort>(referral2List[i].Size);
                            channel.Write<ushort>(referral2List[i].ServerType);
                            channel.Write<ushort>(referral2List[i].ReferralEntryFlags);
                            channel.Write<uint>(referral2List[i].Proximity);
                            channel.Write<uint>(referral2List[i].TimeToLive);
                            channel.Write<ushort>(referral2List[i].DFSPathOffset);
                            channel.Write<ushort>(referral2List[i].DFSAlternatePathOffset);
                            channel.Write<ushort>(referral2List[i].NetworkAddressOffset);
                        }
                        for (int i = 0; i < referral2List.Length; i++)
                        {
                            channel.WriteBytes(Encoding.Unicode.GetBytes(referral2List[i].DFSPath + "\0"));
                            channel.WriteBytes(Encoding.Unicode.GetBytes(referral2List[i].DFSAlternatePath + "\0"));
                            channel.WriteBytes(Encoding.Unicode.GetBytes(referral2List[i].DFSTargetPath + "\0"));
                        }
                    }
                    else
                    {
                        // Branch for negative testing.
                    }
                    channel.EndWriteGroup();
                }
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
                using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans2_Data))
                {
                    using (Channel channel = new Channel(null, memoryStream))
                    {
                        this.trans2Data.ReferralResponse.PathConsumed = channel.Read<ushort>();
                        this.trans2Data.ReferralResponse.NumberOfReferrals = channel.Read<ushort>();
                        this.trans2Data.ReferralResponse.ReferralHeaderFlags = channel.Read<ReferralHeaderFlags>();

                        if (this.trans2Data.ReferralResponse.NumberOfReferrals == 0)
                        {
                            return;
                        }

                        ushort versionNumber = channel.Peek<ushort>(0);

                        switch (versionNumber)
                        {
                            case 0x0001:
                                List<DFS_REFERRAL_V1> referral1List = new List<DFS_REFERRAL_V1>();

                                for (int i = 0; i < this.trans2Data.ReferralResponse.NumberOfReferrals; i++)
                                {
                                    referral1List.Add(channel.Read<DFS_REFERRAL_V1>());
                                }
                                this.trans2Data.ReferralResponse.ReferralEntries = referral1List.ToArray();
                                break;

                            case 0x0002:
                                List<DFS_REFERRAL_V2> referral2List = new List<DFS_REFERRAL_V2>();

                                for (int i = 0; i < this.trans2Data.ReferralResponse.NumberOfReferrals; i++)
                                {
                                    DFS_REFERRAL_V2 referral2 = new DFS_REFERRAL_V2();
                                    referral2.VersionNumber = channel.Read<ushort>();
                                    referral2.Size = channel.Read<ushort>();
                                    referral2.ServerType = channel.Read<ushort>();
                                    referral2.ReferralEntryFlags = channel.Read<ushort>();
                                    referral2.Proximity = channel.Read<uint>();
                                    referral2.TimeToLive = channel.Read<uint>();
                                    referral2.DFSPathOffset = channel.Read<ushort>();
                                    referral2.DFSAlternatePathOffset = channel.Read<ushort>();
                                    referral2.NetworkAddressOffset = channel.Read<ushort>();
                                    referral2List.Add(referral2);
                                }
                                int fixedListSize = this.trans2Data.ReferralResponse.NumberOfReferrals * referralV2FixedSize;
                                byte[] pathData = channel.ReadBytes(this.smbData.Trans2_Data.Length - fixedListSize
                                    - referralHeaderSize);

                                for (int i = 0; i < referral2List.Count; i++)
                                {
                                    int leftCount = referral2List.Count - i;
                                    DFS_REFERRAL_V2 referral2 = referral2List[i];
                                    int dfsPathOffset = referral2.DFSPathOffset - leftCount * referralV2FixedSize;
                                    int dfsAlternatePathOffset = referral2.DFSAlternatePathOffset -
                                        leftCount * referralV2FixedSize;
                                    int targetPathOffset = referral2.NetworkAddressOffset - leftCount * referralV2FixedSize;
                                    int pathLength = 0;
                                    byte wordLength = 2;

                                    for (int j = dfsPathOffset; ; j += wordLength)
                                    {
                                        if (pathData[j] == 0 && pathData[j + 1] == 0)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            pathLength += wordLength;
                                        }
                                    }
                                    referral2.DFSPath = Encoding.Unicode.GetString(pathData, dfsPathOffset, pathLength);
                                    pathLength = 0;

                                    for (int j = dfsAlternatePathOffset; ; j += wordLength)
                                    {
                                        if (pathData[j] == 0 && pathData[j + 1] == 0)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            pathLength += wordLength;
                                        }
                                    }
                                    referral2.DFSAlternatePath = Encoding.Unicode.GetString(pathData, dfsAlternatePathOffset,
                                        pathLength);
                                    pathLength = 0;

                                    for (int j = targetPathOffset; ; j += wordLength)
                                    {
                                        if (pathData[j] == 0 && pathData[j + 1] == 0)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            pathLength += wordLength;
                                        }
                                    }
                                    referral2.DFSTargetPath = Encoding.Unicode.GetString(pathData, targetPathOffset, pathLength);
                                    referral2List[i] = referral2;
                                }
                                this.trans2Data.ReferralResponse.ReferralEntries = referral2List.ToArray();
                                break;
                        }
                    }
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