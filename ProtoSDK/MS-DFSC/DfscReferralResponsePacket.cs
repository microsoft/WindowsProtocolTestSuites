// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Dfsc
{
    /// <summary>
    /// RESP_GET_DFS_REFERRAL packet
    /// </summary>
    public class DfscReferralResponsePacket
    {
        #region Fields

        private uint ntStatus;
        private RESP_GET_DFS_REFERRAL referralResponse;
        private byte[] payload;
        private ushort versionNumber;
        private bool isNameListReferral;

        private const byte sizeofReferralHeader = 8;
        private const byte sizeofWord = 2;

        #endregion


        #region Properties

        /// <summary>
        /// Specify the error code
        /// </summary>
        [CLSCompliant(false)]
        public uint NTStatus
        {
            get
            {
                return this.ntStatus;
            }
        }


        /// <summary>
        /// RESP_GET_DFS_REFERRAL structure
        /// </summary>
        public RESP_GET_DFS_REFERRAL ReferralResponse
        {
            get
            {
                return this.referralResponse;
            }
        }

        /// <summary>
        /// Version number of referral entries
        /// </summary>
        public ushort VersionNumber
        {
            get
            {
                return this.versionNumber;
            }
        }

        /// <summary>
        /// Bool value indicates whether V3 or V4 referral entry is NameListReferral
        /// </summary>
        public bool IsNameListReferral
        {
            get
            {
                return this.isNameListReferral;
            }
        }

        /// <summary>
        /// RESP_GET_DFS_REFERRAL structure in byte array
        /// </summary>
        public byte[] Payload
        {
            get
            {
                return this.payload;
            }
        }
        #endregion


        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public DfscReferralResponsePacket()
        {
        }


        /// <summary>
        /// Constructor
        /// The payload parameter is used to decoded to RESP_GET_DFS_REFERRAL structure.
        /// </summary>
        /// <param name="ntStatus">Specify the error code</param>
        /// <param name="payload">RESP_GET_DFS_REFERRAL structure in byte array</param>
        /// <exception cref="System.InvalidOperationException">
        /// Fail to decode buffer data to RESP_GET_DFS_REFERRAL structure
        /// </exception>
        /// <exception cref=" System.FormatException">payload is null or empty</exception>
        [CLSCompliant(false)]
        public DfscReferralResponsePacket(uint ntStatus, byte[] payload)
        {
            this.ntStatus = ntStatus;
            this.payload = payload;

            if (ntStatus == 0)
            {
                this.FromBytes(payload);
            }
        }

        #endregion


        #region Methods

        /// <summary>
        /// Decode the payload data from byte array to RESP_GET_DFS_REFERRAL structure.
        /// And assign the result to referralResponse field.
        /// </summary>
        /// <param name="payloadData">RESP_GET_DFS_REFERRAL in byte array</param>
        /// <exception cref=" System.InvalidOperationException">
        /// Fail to decode payload data to RESP_GET_DFS_REFERRAL structure
        /// </exception>
        /// <exception cref=" System.FormatException">payloadData is null or empty</exception>
        private void FromBytes(byte[] payloadData)
        {
            if (payloadData != null && payloadData.Length > 0)
            {
                using (MemoryStream memoryStream = new MemoryStream(payloadData))
                {
                    Channel channel = new Channel(null, memoryStream);

                    // Read the referral header
                    this.referralResponse.PathConsumed = channel.Read<ushort>();
                    this.referralResponse.NumberOfReferrals = channel.Read<ushort>();
                    this.referralResponse.ReferralHeaderFlags = channel.Read<ReferralHeaderFlags>();

                    // Read Referral Entries
                    if (this.referralResponse.NumberOfReferrals > 0)
                    {
                        this.versionNumber = channel.Peek<ushort>(0);

                        switch (this.versionNumber)
                        {
                            case 0x0001:
                                List<DFS_REFERRAL_V1> referral1List = new List<DFS_REFERRAL_V1>();
                                // The total size of VersionNumber, Size, ServerType and ReferralEntryFlags in DFS_REFERRAL_V1
                                const ushort referralV1fixedSize = 8;

                                for (int i = 0; i < this.referralResponse.NumberOfReferrals; i++)
                                {
                                    DFS_REFERRAL_V1 referral1 = new DFS_REFERRAL_V1();
                                    referral1.VersionNumber = channel.Read<ushort>();
                                    referral1.Size = channel.Read<ushort>();
                                    referral1.ServerType = channel.Read<ushort>();
                                    referral1.ReferralEntryFlags = channel.Read<ushort>();
                                    referral1.ShareName = Encoding.Unicode.GetString(channel.ReadBytes(referral1.Size
                                        - referralV1fixedSize - sizeofWord));
                                    channel.ReadBytes(sizeofWord);
                                    referral1List.Add(referral1);
                                }
                                this.referralResponse.ReferralEntries = referral1List.ToArray();
                                break;

                            case 0x0002:
                                DecodeReferralV2(channel);
                                break;

                            case 0x0003:
                            case 0x0004:
                                // The offset from beginning of DFS_REFERRAL_V3V4 to ReferralEntryFlags field
                                ushort flagOffset = 6;
                                ReferralEntryFlags_Values referralEntryFlags = channel.Peek<ReferralEntryFlags_Values>(flagOffset);

                                if ((referralEntryFlags & ReferralEntryFlags_Values.NameListReferral)
                                    == ReferralEntryFlags_Values.NameListReferral)
                                {
                                    this.isNameListReferral = true;
                                    DecodeReferralV3V4_NameListReferral(channel);
                                }
                                else
                                {
                                    this.isNameListReferral = false;
                                    DecodeReferralV3V4_NonNameListReferral(channel);
                                }
                                break;

                            default:
                                throw new InvalidOperationException("The version number of Referral Entry is not correct.");
                        }
                    }
                }
            }
            else
            {
                throw new FormatException("payload byte array is null or empty for decoding.");
            }
        }


        /// <summary>
        /// Decode the payload for type of Referral Entry 2
        /// </summary>
        /// <param name="channel">channel with payload data</param>
        /// <exception cref=" System.InvalidOperationException">
        /// Fail to decode payload data to ReferralV2 structure
        /// </exception>
        private void DecodeReferralV2(Channel channel)
        {
            bool isDfsPathFollowed = false;
            List<DFS_REFERRAL_V2> referral2List = new List<DFS_REFERRAL_V2>();

            for (int i = 0; i < this.referralResponse.NumberOfReferrals; i++)
            {
                DFS_REFERRAL_V2 referral2 = new DFS_REFERRAL_V2();
                // Read the fixed portion of DFS_REFERRAL_V2
                referral2.VersionNumber = channel.Read<ushort>();
                referral2.Size = channel.Read<ushort>();
                referral2.ServerType = channel.Read<ushort>();
                referral2.ReferralEntryFlags = channel.Read<ushort>();
                referral2.Proximity = channel.Read<uint>();
                referral2.TimeToLive = channel.Read<uint>();
                referral2.DFSPathOffset = channel.Read<ushort>();
                referral2.DFSAlternatePathOffset = channel.Read<ushort>();
                referral2.NetworkAddressOffset = channel.Read<ushort>();

                if (i == 0 && referral2.DFSPathOffset == referral2.Size)
                {
                    isDfsPathFollowed = true;
                }

                // The Dfs paths of immediately follows each referral entry. Read the paths orderly.
                if (isDfsPathFollowed)
                {
                    // Get the Dfs paths from channel
                    // TD does not mention whether padding data exists between Dfs paths.
                    // Drop the possibly exists padding data
                    referral2.DFSPath = ReadDfsPath(channel);
                    ReadPadding(channel, referral2.DFSAlternatePathOffset - referral2.DFSPathOffset
                        - (referral2.DFSPath.Length + 1) * sizeofWord);

                    referral2.DFSAlternatePath = ReadDfsPath(channel);
                    ReadPadding(channel, referral2.NetworkAddressOffset - referral2.DFSAlternatePathOffset
                        - (referral2.DFSAlternatePath.Length + 1) * sizeofWord);

                    referral2.DFSTargetPath = ReadDfsPath(channel);
                }
                referral2List.Add(referral2);
            }

            // All Dfs paths follow the last referral entry.Read the paths after all entries have been read.
            if (!isDfsPathFollowed)
            {
                ushort referral2FixedSize = referral2List[0].Size;
                byte[] pathData = channel.ReadBytes((int)(channel.Stream.Length - channel.Stream.Position));

                for (int i = 0; i < referral2List.Count; i++)
                {
                    // Calculate the offsets of Dfs paths
                    int leftCount = referral2List.Count - i;
                    DFS_REFERRAL_V2 referral2 = referral2List[i];
                    int dfsPathOffset = referral2.DFSPathOffset - leftCount * referral2FixedSize;
                    int dfsAlternatePathOffset = referral2.DFSAlternatePathOffset -
                        leftCount * referral2FixedSize;
                    int targetPathOffset = referral2.NetworkAddressOffset - leftCount * referral2FixedSize;

                    // Get the Dfs paths
                    referral2.DFSPath = ReadDfsPath(pathData, dfsPathOffset);
                    referral2.DFSAlternatePath = ReadDfsPath(pathData, dfsAlternatePathOffset);
                    referral2.DFSTargetPath = ReadDfsPath(pathData, targetPathOffset);

                    referral2List[i] = referral2;
                }
            }
            this.referralResponse.ReferralEntries = referral2List.ToArray();
        }


        /// <summary>
        /// Decode the payload for type of Referral Entry V3V4_NameListReferral
        /// </summary>
        /// <param name="channel">channel with payload data</param>
        /// <exception cref=" System.InvalidOperationException">
        /// Fail to decode payload data to ReferralV3V4_NameListReferral structure
        /// </exception>
        private void DecodeReferralV3V4_NameListReferral(Channel channel)
        {
            bool isDfsPathFollowed = false;
            List<DFS_REFERRAL_V3V4_NameListReferral> tempList = new List<DFS_REFERRAL_V3V4_NameListReferral>();
            // The total size of all fields in front of Padding field in DFS_REFERRAL_V3V4_NameListReferral
            const ushort referral3FixedSize = 18;

            for (int i = 0; i < this.referralResponse.NumberOfReferrals; i++)
            {
                DFS_REFERRAL_V3V4_NameListReferral referral3 = new DFS_REFERRAL_V3V4_NameListReferral();

                // Read the fixed portion of DFS_REFERRAL_V3V4_NameListReferral
                referral3.VersionNumber = channel.Read<ushort>();
                referral3.Size = channel.Read<ushort>();
                referral3.ServerType = channel.Read<ushort>();
                referral3.ReferralEntryFlags = channel.Read<ReferralEntryFlags_Values>();
                referral3.TimeToLive = channel.Read<uint>();
                referral3.SpecialNameOffset = channel.Read<ushort>();
                referral3.NumberOfExpandedNames = channel.Read<ushort>();
                referral3.ExpandedNameOffset = channel.Read<ushort>();
                referral3.Padding = channel.ReadBytes(referral3.Size - referral3FixedSize);

                if (i == 0 && referral3.SpecialNameOffset == referral3.Size)
                {
                    isDfsPathFollowed = true;
                }

                // The Dfs paths of immediately follows each referral entry. Read the paths orderly.
                if (isDfsPathFollowed)
                {
                    // Get the Dfs paths from channel
                    // TD does not mention whether padding data exists between Dfs paths.
                    // Drop the possibly exists padding data
                    referral3.SpecialName = ReadDfsPath(channel);
                    ReadPadding(channel, referral3.ExpandedNameOffset - referral3.SpecialNameOffset
                        - (referral3.SpecialName.Length + 1) * sizeofWord);
                    referral3.DCNameArray = new string[referral3.NumberOfExpandedNames];

                    for (int j = 0; j < referral3.NumberOfExpandedNames; j++)
                    {
                        referral3.DCNameArray[j] = ReadDfsPath(channel);
                    }
                }
                tempList.Add(referral3);
            }

            // All Dfs paths follow the last referral entry.Read the paths after all entries have been read.
            if (!isDfsPathFollowed)
            {
                ushort referralFixedSize = tempList[0].Size;
                byte[] pathData = channel.ReadBytes((int)(channel.Stream.Length - channel.Stream.Position));

                for (int i = 0; i < tempList.Count; i++)
                {
                    // Calculate the offsets of Dfs paths
                    int leftCount = tempList.Count - i;
                    DFS_REFERRAL_V3V4_NameListReferral referral3 = tempList[i];
                    int specialNameOffset = referral3.SpecialNameOffset - leftCount * referralFixedSize;
                    int dcNameOffset = referral3.ExpandedNameOffset - leftCount * referralFixedSize;

                    // Get the Dfs paths
                    referral3.SpecialName = ReadDfsPath(pathData, specialNameOffset);
                    referral3.DCNameArray = new string[referral3.NumberOfExpandedNames];

                    for (int j = 0; j < referral3.NumberOfExpandedNames; j++)
                    {
                        referral3.DCNameArray[j] = ReadDfsPath(pathData, dcNameOffset);
                        dcNameOffset += referral3.DCNameArray[j].Length * sizeofWord;
                    }
                    tempList[i] = referral3;
                }
            }
            this.referralResponse.ReferralEntries = tempList.ToArray();
        }


        /// <summary>
        /// Decode the payload for type of Referral Entry V3V4_NonNameListReferral
        /// </summary>
        /// <param name="channel">channel with payload data</param>
        /// <exception cref=" System.InvalidOperationException">
        /// Fail to decode payload data to ReferralV3V4_NonNameListReferral structure
        /// </exception>
        private void DecodeReferralV3V4_NonNameListReferral(Channel channel)
        {
            bool isDfsPathFollowed = false;
            List<DFS_REFERRAL_V3V4_NonNameListReferral> tempList = new List<DFS_REFERRAL_V3V4_NonNameListReferral>();

            for (int i = 0; i < this.referralResponse.NumberOfReferrals; i++)
            {
                DFS_REFERRAL_V3V4_NonNameListReferral referral3 = new DFS_REFERRAL_V3V4_NonNameListReferral();

                // Read the fixed portion of DFS_REFERRAL_V3V4_NonNameListReferral
                referral3.VersionNumber = channel.Read<ushort>();
                referral3.Size = channel.Read<ushort>();
                referral3.ServerType = channel.Read<ushort>();
                referral3.ReferralEntryFlags = channel.Read<ReferralEntryFlags_Values>();
                referral3.TimeToLive = channel.Read<uint>();
                referral3.DFSPathOffset = channel.Read<ushort>();
                referral3.DFSAlternatePathOffset = channel.Read<ushort>();
                referral3.NetworkAddressOffset = channel.Read<ushort>();
                referral3.ServiceSiteGuid = new Guid(channel.ReadBytes(16));

                if (i == 0 && referral3.DFSPathOffset == referral3.Size)
                {
                    isDfsPathFollowed = true;
                }

                // The Dfs paths of immediately follows each referral entry. Read the paths orderly.
                if (isDfsPathFollowed)
                {
                    // Get the Dfs paths from channel
                    // TD does not mention whether padding data exists between Dfs paths.
                    // Drop the possibly exists padding data
                    referral3.DFSPath = ReadDfsPath(channel);
                    ReadPadding(channel, referral3.DFSAlternatePathOffset - referral3.DFSPathOffset
                        - (referral3.DFSPath.Length + 1) * sizeofWord);

                    referral3.DFSAlternatePath = ReadDfsPath(channel);
                    ReadPadding(channel, referral3.NetworkAddressOffset - referral3.DFSAlternatePathOffset
                        - (referral3.DFSAlternatePath.Length + 1) * sizeofWord);

                    referral3.DFSTargetPath = ReadDfsPath(channel);
                }
                tempList.Add(referral3);
            }

            // All Dfs paths follow the last referral entry.Read the paths after all entries have been read.
            if (!isDfsPathFollowed)
            {
                ushort referralFixedSize = tempList[0].Size;
                byte[] pathData = channel.ReadBytes((int)(channel.Stream.Length - channel.Stream.Position));

                for (int i = 0; i < tempList.Count; i++)
                {
                    // Calculate the offsets of Dfs paths
                    int leftCount = tempList.Count - i;
                    DFS_REFERRAL_V3V4_NonNameListReferral referral3 = tempList[i];
                    int dfsPathOffset = referral3.DFSPathOffset - leftCount * referralFixedSize;
                    int dfsAlternatePathOffset = referral3.DFSAlternatePathOffset -
                        leftCount * referralFixedSize;
                    int targetPathOffset = referral3.NetworkAddressOffset - leftCount * referralFixedSize;

                    // Get the Dfs paths
                    referral3.DFSPath = ReadDfsPath(pathData, dfsPathOffset);
                    referral3.DFSAlternatePath = ReadDfsPath(pathData, dfsAlternatePathOffset);
                    referral3.DFSTargetPath = ReadDfsPath(pathData, targetPathOffset);

                    tempList[i] = referral3;
                }
            }
            this.referralResponse.ReferralEntries = tempList.ToArray();
        }


        /// <summary>
        /// Get Dfs path from channel.
        /// </summary>
        /// <param name="channel">channel with Dfs path data</param>
        /// <returns>Dfs path in string</returns>
        /// <exception cref=" System.InvalidOperationException">Fail to get dfs path from channel.</exception>
        private string ReadDfsPath(Channel channel)
        {
            int pathLength = 0;

            while (channel.Peek<ushort>(pathLength) != 0)
            {
                pathLength += sizeofWord;
            }

            string dfsPAth = Encoding.Unicode.GetString(channel.ReadBytes(pathLength));
            channel.ReadBytes(sizeofWord);
            return dfsPAth;
        }


        /// <summary>
        /// Get Dfs path from path data buffer with the specified offset
        /// </summary>
        /// <param name="pathData">path data buffer</param>
        /// <param name="pathOffset">offset of current expected Dfs path</param>
        /// <returns>Dfs path in string</returns>
        /// <exception cref=" System.InvalidOperationException">Fail to get dfs from pathData.</exception>
        private string ReadDfsPath(byte[] pathData, int pathOffset)
        {
            int pathLength = 0;

            for (int j = pathOffset; ; j += sizeofWord)
            {
                if (j + 1 >= pathData.Length)
                {
                    throw new InvalidOperationException(
                        "Fail to get null-terminator from pathOffset to the end of pathData.");
                }

                if (pathData[j] == 0 && pathData[j + 1] == 0)
                {
                    break;
                }
                pathLength += sizeofWord;
            }

            return Encoding.Unicode.GetString(pathData, pathOffset, pathLength);
        }


        /// <summary>
        /// Read the pad data from channel.
        /// </summary>
        /// <param name="channel">channel with payload data</param>
        /// <param name="padLength">the length of pad</param>
        /// <exception cref=" System.InvalidOperationException">pad length is negative</exception>
        private void ReadPadding(Channel channel, int padLength)
        {
            if (padLength < 0)
            {
                throw new InvalidOperationException("The length of pad data is negative.");
            }
            channel.ReadBytes(padLength);
        }

        #endregion
    }
}