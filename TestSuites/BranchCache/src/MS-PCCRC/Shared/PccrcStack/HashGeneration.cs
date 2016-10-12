// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrc
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Generate the content information data according to the data read from file.
    /// </summary>
    public class HashGeneration
    {
        #region Fields

        /// <summary>
        /// The byte count of a block.
        /// </summary>
        private const int BLOCKBYTECOUNT = 0x10000;

        /// <summary>
        /// The block count of a segment.
        /// </summary>
        private const int SEGMENTBLOCKCOUNT = 512;

        /// <summary>
        /// The string used for create SHA-256 hash algorithm.
        /// </summary>
        private const string ALGOSHA256 = "SHA256";

        /// <summary>
        /// The string used for create SHA-384 hash algorithm.
        /// </summary>
        private const string ALGOSHA384 = "SHA384";

        /// <summary>
        /// The string used for create SHA-512 hash algorithm.
        /// </summary>
        private const string ALGOSHA512 = "SHA512";

        /// <summary>
        /// The string used for create SHA-256 HMAC algorithm.
        /// </summary>
        private const string ALGOHMACSHA256 = "HMACSHA256";

        /// <summary>
        /// The string used for create SHA-384 HMAC algorithm.
        /// </summary>
        private const string ALGOHMACSHA384 = "HMACSHA384";

        /// <summary>
        /// The string used for create SHA-512 HMAC algorithm.
        /// </summary>
        private const string ALGOHMACSHA512 = "HMACSHA512";

        /// <summary>
        /// C2 string in PCCRC protocol.
        /// </summary>
        private const string HOHODKAPPENDSTRING = "MS_P2P_CACHING\0";

        /// <summary>
        /// Hash algorithm.
        /// </summary>
        private HashAlgorithm hashAlgo;

        /// <summary>
        /// Hash algorithm.
        /// </summary>
        private HMAC hmacAlgo;

        /// <summary>
        /// Hash algorithm to use.
        /// </summary>
        private dwHashAlgo_Values dwHashAlgo;

        /// <summary>
        /// Hash algorithm to use.
        /// </summary>
        private dwHashAlgoV2_Values dwHashAlgoV2;

        /// <summary>
        /// BlockHash size to use.
        /// </summary>
        private int blockHashSize;

        /// <summary>
        /// Server secret.
        /// A SHA-256 hash of an arbitrary length binary string.
        /// </summary>
        private byte[] serverSecret;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the HashGeneration class.
        /// </summary>
        public HashGeneration()
        {
        }

        /// <summary>
        /// Initializes a new instance of the HashGeneration class with server secret and hash algorithm.
        /// </summary>
        /// <param name="secret">The binary array of the server secret.</param>
        /// <param name="algo">The hash algorithm.</param>
        public HashGeneration(byte[] secret, dwHashAlgo_Values algo)
        {
            this.ServerSecret = secret;
            this.DwHashAlgo = algo;
        }

        /// <summary>
        /// Initializes a new instance of the HashGeneration class with server secret and hash algorithm.
        /// </summary>
        /// <param name="secret">The unicode string of the server secret.</param>
        /// <param name="algo">The hash algorithm.</param>
        public HashGeneration(string secret, dwHashAlgo_Values algo)
        {
            this.ServerSecret = Encoding.Unicode.GetBytes(secret);
            this.DwHashAlgo = algo;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the dwHashAlgo.
        /// </summary>
        public dwHashAlgo_Values DwHashAlgo
        {
            get
            {
                return this.dwHashAlgo; 
            }

            set
            {
                this.dwHashAlgo = value;
                PccrcUtility.GetHashAlgorithm(this.dwHashAlgo, out this.hashAlgo, out this.hmacAlgo, out this.blockHashSize);
            }
        }

        /// <summary>
        /// Gets or sets the dwHashAlgoV2.
        /// </summary>
        public dwHashAlgoV2_Values DwHashAlgoV2
        {
            get
            {
                return this.dwHashAlgoV2;
            }

            set
            {
                this.dwHashAlgoV2 = value;
                PccrcUtility.GetHashAlgorithm(this.dwHashAlgoV2, out this.hashAlgo, out this.hmacAlgo);
            }
        }

        /// <summary>
        /// Gets or sets the serverSecret.
        /// </summary>
        public byte[] ServerSecret
        {
            get 
            {
                return this.serverSecret; 
            }

            set
            {
                HashAlgorithm secretAlgo = HashAlgorithm.Create(ALGOSHA256);
                this.serverSecret = secretAlgo.ComputeHash(value);
                secretAlgo.Dispose();
            }
        }

        #endregion 

        #region Methods

        /// <summary>
        /// Compute the segment HoD.
        /// </summary>
        /// <param name="kp">The Kp data</param>
        /// <param name="hod">The segment HoD</param>
        /// <returns>The HoHoDk data</returns>
        public byte[] ComputeHoHoDk(byte[] kp, byte[] hod)
        {
            List<byte> tempList = new List<byte>();
            this.hmacAlgo.Key = kp;
            tempList.AddRange(hod);
            tempList.AddRange(Encoding.Unicode.GetBytes(PccrcUtility.HOHODKAPPENDSTRING));
            return this.hmacAlgo.ComputeHash(tempList.ToArray());
        }

        /// <summary>
        /// Generate the content information structure use the file data.
        /// </summary>
        /// <param name="data">The file data.</param>
        /// <returns>The content information structure</returns>
        public Content_Information_Data_Structure GenerateContentInformation(byte[] data)
        {
            int segmentIndex = 0;
            int blockIndex = 0;
            List<byte> blockHashList = new List<byte>();
            Content_Information_Data_Structure contentInformation = new Content_Information_Data_Structure();

            int blockCount = data.Length / BLOCKBYTECOUNT;
            if (data.Length > BLOCKBYTECOUNT * blockCount)
            {
                blockCount = blockCount + 1;
            }

            int segmentCount = blockCount / SEGMENTBLOCKCOUNT;
            if (blockCount > SEGMENTBLOCKCOUNT * segmentCount)
            {
                segmentCount = segmentCount + 1;
            }

            contentInformation.Version = 0x0100;
            contentInformation.dwHashAlgo = this.dwHashAlgo;
            contentInformation.dwOffsetInFirstSegment = 0;
            contentInformation.dwReadBytesInLastSegment = 0;
            contentInformation.cSegments = (uint)segmentCount;
            contentInformation.segments = new SegmentDescription[segmentCount];
            contentInformation.blocks = new SegmentContentBlocks[segmentCount];

            for (int i = 0; i < blockCount; i++)
            {
                int copyLength = BLOCKBYTECOUNT;
                if (data.Length < BLOCKBYTECOUNT * (i + 1))
                {
                    copyLength = data.Length - (BLOCKBYTECOUNT * i);
                }

                byte[] temp = new byte[copyLength];
                Array.Copy(data, i * BLOCKBYTECOUNT, temp, 0, copyLength);
                byte[] blockHash = this.ComputeBlockHash(temp);
                blockHashList.AddRange(blockHash);
                blockIndex = i - (segmentIndex * SEGMENTBLOCKCOUNT);

                if (blockIndex + 1 == SEGMENTBLOCKCOUNT
                    || i + 1 == blockCount)
                {
                    contentInformation.segments[segmentIndex].ullOffsetInContent =
                        (ulong)(segmentIndex * BLOCKBYTECOUNT * SEGMENTBLOCKCOUNT);
                    contentInformation.segments[segmentIndex].cbSegment =
                        (uint)((blockIndex * BLOCKBYTECOUNT) + copyLength);
                    contentInformation.segments[segmentIndex].cbBlockSize =
                        (uint)BLOCKBYTECOUNT;
                    contentInformation.segments[segmentIndex].SegmentHashOfData = this.ComputeHod(blockHashList);
                    contentInformation.segments[segmentIndex].SegmentSecret = this.ComputeKp(this.ComputeHod(blockHashList));
                    contentInformation.blocks[segmentIndex].cBlocks = (uint)blockIndex + 1;
                    contentInformation.blocks[segmentIndex].BlockHashes = blockHash;

                    blockHashList.Clear();
                    segmentIndex++;
                }
            }

            return contentInformation;
        }

        /// <summary>
        /// Generate the content information structure use the file data.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>The content information structure</returns>
        public Content_Information_Data_Structure GenerateContentInformation(string filePath)
        {
            return this.GenerateContentInformation(PccrcUtility.ReadFile(filePath));
        }

        /// <summary>
        /// Compute the block data to block hash data.
        /// </summary>
        /// <param name="blockData">The block data</param>
        /// <returns>The block hash data</returns>
        private byte[] ComputeBlockHash(byte[] blockData)
        {
            return this.hashAlgo.ComputeHash(blockData);
        }

        /// <summary>
        /// Compute the segment HoD.
        /// </summary>
        /// <param name="blockDataList">The block data list</param>
        /// <returns>The hod data</returns>
        private byte[] ComputeHod(List<byte> blockDataList)
        {
            return this.hashAlgo.ComputeHash(blockDataList.ToArray());
        }

        /// <summary>
        /// Compute the segment secret.
        /// </summary>
        /// <param name="hod">The segment HoD</param>
        /// <returns>The Kp data</returns>
        private byte[] ComputeKp(byte[] hod)
        {
            this.hmacAlgo.Key = this.serverSecret;
            return this.hmacAlgo.ComputeHash(hod);
        }

        #endregion
    }
}

