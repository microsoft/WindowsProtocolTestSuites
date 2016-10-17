// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrc
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Linq;
    using Microsoft.Protocols.TestTools.StackSdk;
    using Microsoft.Protocols.TestTools.StackSdk.CommonStack;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrc;

    /// <summary>
    /// The PCCRC utility.
    /// </summary>
    public static class PccrcUtility
    {
        #region Const Fields

        /// <summary>
        /// C2 string in PCCRC protocol.
        /// </summary>
        public const string HOHODKAPPENDSTRING = "MS_P2P_CACHING\0";

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

        #endregion

        #region Utility Methods

        /// <summary>
        /// Compute the segment HoD.
        /// </summary>
        /// <param name="kp">The Kp data</param>
        /// <param name="hod">The segment HoD</param>
        /// <param name="hashAlgo">The HoHoDk hash algorithm.</param>
        /// <returns>The HoHoDk data</returns>
        public static byte[] GetHoHoDkBytes(byte[] kp, byte[] hod, dwHashAlgo_Values hashAlgo)
        {
            HMAC hohodkAlgo = null;
            List<byte> tempList = new List<byte>();
            switch (hashAlgo)
            {
                case dwHashAlgo_Values.SHA256:
                    hohodkAlgo = HMAC.Create(ALGOHMACSHA256);
                    break;
                case dwHashAlgo_Values.SHA384:
                    hohodkAlgo = HMAC.Create(ALGOHMACSHA384);
                    break;
                case dwHashAlgo_Values.SHA512:
                    hohodkAlgo = HMAC.Create(ALGOHMACSHA512);
                    break;
                default:
                    throw new NotImplementedException();
            }

            hohodkAlgo.Key = kp;
            tempList.AddRange(hod);
            tempList.AddRange(Encoding.Unicode.GetBytes(HOHODKAPPENDSTRING));
            var hash = hohodkAlgo.ComputeHash(tempList.ToArray());

            return hash;
        }

        /// <summary>
        /// Compute the segment HoD.
        /// </summary>
        /// <param name="kp">The Kp data</param>
        /// <param name="hod">The segment HoD</param>
        /// <param name="hashAlgo">The HoHoDk hash algorithm.</param>
        /// <returns>The HoHoDk data</returns>
        public static byte[] GetHoHoDkBytes(byte[] kp, byte[] hod, dwHashAlgoV2_Values hashAlgo)
        {
            HMAC hohodkAlgo = null;
            List<byte> tempList = new List<byte>();
            switch (hashAlgo)
            {
                case dwHashAlgoV2_Values.TRUNCATED_SHA512:
                    hohodkAlgo = HMAC.Create(ALGOHMACSHA512);
                    break;
                default:
                    throw new NotImplementedException();
            }

            hohodkAlgo.Key = kp;
            tempList.AddRange(hod);
            tempList.AddRange(Encoding.Unicode.GetBytes(HOHODKAPPENDSTRING));
            var hash = hohodkAlgo.ComputeHash(tempList.ToArray());

            if (hashAlgo == dwHashAlgoV2_Values.TRUNCATED_SHA512)
                hash = hash.Take(32).ToArray();

            return hash;
        }

        /// <summary>
        /// Compute the segment HoD.
        /// </summary>
        /// <param name="kp">The Kp data</param>
        /// <param name="hod">The segment HoD</param>
        /// <param name="hashAlgo">The HoHoDk hash algorithm.</param>
        /// <returns>The HoHoDk data</returns>
        public static string GetHoHoDkString(byte[] kp, byte[] hod, dwHashAlgo_Values hashAlgo)
        {
            byte[] hohodk = GetHoHoDkBytes(kp, hod, hashAlgo);
            return ToHexString(hohodk);
        }

        /// <summary>
        /// Get the hash algorithm and hmac algorithm for V1.
        /// </summary>
        /// <param name="hashAlgo">The hash algorithm value</param>
        /// <returns>The hash algorithm and hmac algorithm</returns>
        public static void GetHashAlgorithm(dwHashAlgo_Values hashAlgo, out HashAlgorithm hashAlgorithm, out HMAC hmacAlgorithm, out int blockHashSize)
        {
            switch (hashAlgo)
            {
                case dwHashAlgo_Values.SHA256:
                    hashAlgorithm = HashAlgorithm.Create("SHA256");
                    hmacAlgorithm = HMAC.Create("HMACSHA256");
                    blockHashSize = 32;
                    break;
                case dwHashAlgo_Values.SHA384:
                    hashAlgorithm = HashAlgorithm.Create("SHA384");
                    hmacAlgorithm = HMAC.Create("HMACSHA384");
                    blockHashSize = 48;
                    break;
                case dwHashAlgo_Values.SHA512:
                    hashAlgorithm = HashAlgorithm.Create("SHA512");
                    hmacAlgorithm = HMAC.Create("HMACSHA512");
                    blockHashSize = 64;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Get the hash algorithm and hmac algorithm for V2.
        /// </summary>
        /// <param name="hashAlgo">The hash algorithm value</param>
        /// <returns>The hash algorithm and hmac algorithm</returns>
        public static void GetHashAlgorithm(dwHashAlgoV2_Values hashAlgo, out HashAlgorithm hashAlgorithm, out HMAC hmacAlgorithm)
        {
            switch (hashAlgo)
            {
                case dwHashAlgoV2_Values.TRUNCATED_SHA512:
                    hashAlgorithm = HashAlgorithm.Create("SHA512");
                    hmacAlgorithm = HMAC.Create("HMACSHA512");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Read files from assigned directory.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>The file data.</returns>
        public static byte[] ReadFile(string filePath)
        {
            try
            {
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

                byte[] data = new byte[fileStream.Length];
                while (true)
                {
                    int readCount = fileStream.Read(data, 0, data.Length);
                    if (readCount <= 0)
                    {
                        fileStream.Dispose();
                        fileStream.Close();
                        return data;
                    }
                }
            }
            catch (FileNotFoundException)
            {
                throw;
            }
        }

        /// <summary>
        /// Transfer a byte array to a hex string. 
        /// Example from a byte array { 0x0a, 0x0b, 0x0c }, 
        /// the return is a string 0A0B0C.
        /// </summary>
        /// <param name="data">The input byte array</param>
        /// <returns>Hex String from the byte array</returns>
        public static string ToHexString(byte[] data)
        {
            string hexStr = string.Empty;
            if (data != null)
            {
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    builder.Append(data[i].ToString("X2"));
                }

                hexStr = builder.ToString();
            }

            return hexStr;
        }

        #endregion

        #region Parse content information from server response data

        #region Public methods

        /// <summary>
        /// Parse the information of the content
        /// </summary>
        /// <param name="data">The byte data in little-endian order.</param>
        /// <returns>Returns the content information data structure.</returns>
        public static Content_Information_Data_Structure ParseContentInformation(byte[] data)
        {
            int offset = 0;
            return ParseContentInformation(data, ref offset);
        }

        /// <summary>
        /// Parse the information of the content
        /// </summary>
        /// <param name="data">The byte data in little-endian order.</param>
        /// <param name="index">The start index.</param>
        /// <returns>Returns the content information data structure.</returns>
        public static Content_Information_Data_Structure ParseContentInformation(byte[] data, ref int index)
        {
            Content_Information_Data_Structure contentInfo = new Content_Information_Data_Structure();
            byte[] informationData = GetBytes(data, ref index, data.Length - index);
            int tempIndex = 0;

            contentInfo.Version = GetUInt16(informationData, ref tempIndex);
            contentInfo.dwHashAlgo = (dwHashAlgo_Values)GetUInt32(informationData, ref tempIndex);
            contentInfo.dwOffsetInFirstSegment = GetUInt32(informationData, ref tempIndex);
            contentInfo.dwReadBytesInLastSegment = GetUInt32(informationData, ref tempIndex);
            contentInfo.cSegments = GetUInt32(informationData, ref tempIndex);
            contentInfo.segments = ParseSements(
                contentInfo.dwHashAlgo, 
                contentInfo.cSegments, 
                informationData, 
                ref tempIndex);
            contentInfo.blocks = ParseBlocks(
                contentInfo.dwHashAlgo, 
                contentInfo.cSegments, 
                informationData, 
                ref tempIndex);

            return contentInfo;
        }

        /// <summary>
        /// Parse the information of the content
        /// </summary>
        /// <param name="data">The byte data in little-endian order.</param>
        /// <returns>Returns the content information data structure.</returns>
        public static Content_Information_Data_Structure_V2 ParseContentInformationV2(byte[] data)
        {
            int offset = 0;
            return ParseContentInformationV2(data, ref offset);
        }

        /// <summary>
        /// Parse the information of the content
        /// </summary>
        /// <param name="data">The byte data in little-endian order.</param>
        /// <param name="index">The start index.</param>
        /// <returns>Returns the content information data structure.</returns>
        public static Content_Information_Data_Structure_V2 ParseContentInformationV2(byte[] data, ref int index)
        {
            var contentInfo = TypeMarshal.ToStruct<Content_Information_Data_Structure_V2>(data, ref index);
            List<ChunkDescription> chunks = new List<ChunkDescription>();
            while (index < data.Length - 1)
            {
                chunks.Add(TypeMarshal.ToStruct<ChunkDescription>(data, ref index));
            }
            contentInfo.chunks = chunks.ToArray();

            return contentInfo;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Parse the information of the block.
        /// </summary>
        /// <param name="dwHashAlgo">The hash algorithm to use.</param>
        /// <param name="cSegments">The number of segments which intersect the content range and hence are contained 
        /// in the Content Information structure.</param>
        /// <param name="data">The byte data in little-endian order.</param>
        /// <param name="index">The start index.</param>
        /// <returns>Returns the block infromation.</returns>
        private static SegmentContentBlocks[] ParseBlocks(
            dwHashAlgo_Values dwHashAlgo,
            uint cSegments,
            byte[] data,
            ref int index)
        {
            SegmentContentBlocks[] retBlocks = new SegmentContentBlocks[cSegments];
            int dataSize = GetDataSizeByHashAlgo(dwHashAlgo);
            for (int i = 0; i < retBlocks.Length; i++)
            {
                retBlocks[i].cBlocks = GetUInt32(data, ref index);
                retBlocks[i].BlockHashes = GetBytes(
                    data,
                    ref index,
                    (int)(dataSize * retBlocks[i].cBlocks));
            }

            return retBlocks;
        }

        /// <summary>
        /// Parse the information of segment.
        /// </summary>
        /// <param name="dwHashAlgo">The hash algorithm to use.</param>
        /// <param name="cSegments">The number of segments which intersect the content range and hence are contained 
        /// in the Content Information structure.</param>
        /// <param name="data">The byte data.</param>
        /// <param name="index">The start index.</param>
        /// <returns>Returns the segment infromation.</returns>
        private static SegmentDescription[] ParseSements(
            dwHashAlgo_Values dwHashAlgo,
            uint cSegments,
            byte[] data,
            ref int index)
        {
            SegmentDescription[] retSegments = new SegmentDescription[cSegments];
            int dataSize = GetDataSizeByHashAlgo(dwHashAlgo);
            for (int i = 0; i < retSegments.Length; i++)
            {
                retSegments[i].ullOffsetInContent = GetUInt64(data, ref index);
                retSegments[i].cbSegment = GetUInt32(data, ref index);
                retSegments[i].cbBlockSize = GetUInt32(data, ref index);
                byte[] tempSegmentHashOfData = GetBytes(data, ref index, dataSize);
                retSegments[i].SegmentHashOfData = tempSegmentHashOfData;
                byte[] tempSegmentSecret = GetBytes(data, ref index, dataSize);
                retSegments[i].SegmentSecret = tempSegmentSecret;
            }

            return retSegments;
        }

        /// <summary>
        /// Parse UInt16 from bytes.
        /// </summary>
        /// <param name="buffer">The buffer stores the value in little-endian order..</param>
        /// <param name="index">The index of start to parse</param>
        /// <returns>Parsed UInt16 value</returns>
        private static ushort GetUInt16(byte[] buffer, ref int index)
        {
            ushort ushortRet = 0;
            byte[] byteTemp = BitConverter.GetBytes(ushortRet);

            Array.Copy(buffer, index, byteTemp, 0, byteTemp.Length);
            ushortRet = BitConverter.ToUInt16(byteTemp, 0);
            index += byteTemp.Length;

            return ushortRet;
        }

        /// <summary>
        /// Parse UInt32 from bytes
        /// </summary>
        /// <param name="buffer">The buffer stores the value in little-endian order.</param>
        /// <param name="index">The index of start to parse</param>
        /// <returns>Parsed UInt32 value</returns>
        private static uint GetUInt32(byte[] buffer, ref int index)
        {
            uint uintRet = 0;
            byte[] byteTemp = BitConverter.GetBytes(uintRet);

            Array.Copy(buffer, index, byteTemp, 0, byteTemp.Length);
            uintRet = BitConverter.ToUInt32(byteTemp, 0);
            index += byteTemp.Length;

            return uintRet;
        }

        /// <summary>
        /// Parse UInt64 from bytes.
        /// </summary>
        /// <param name="buffer">The buffer stores the value in little-endian order.</param>
        /// <param name="index">The index of start to parse</param>
        /// <returns>Parsed UInt32 value</returns>
        private static ulong GetUInt64(byte[] buffer, ref int index)
        {
            ulong uintRet = 0;
            byte[] byteTemp = BitConverter.GetBytes(uintRet);

            Array.Copy(buffer, index, byteTemp, 0, byteTemp.Length);
            uintRet = BitConverter.ToUInt64(byteTemp, 0);
            index += byteTemp.Length;

            return uintRet;
        }

        /// <summary>
        /// Get bytes from bytes.
        /// </summary>
        /// <param name="buffer">The buffer stores the value in little-endian order.</param>
        /// <param name="index">The index of start to parse</param>
        /// <param name="count">Count of bytes</param>
        /// <returns>Parsed UInt16 value</returns>
        private static byte[] GetBytes(byte[] buffer, ref int index, int count)
        {
            byte[] byteRet = null;

            if (count > 0)
            {
                byteRet = new byte[count];

                Array.Copy(buffer, index, byteRet, 0, byteRet.Length);
                index += byteRet.Length;
            }

            return byteRet;
        }

        /// <summary>
        /// Get data size by hash algorithm.
        /// </summary>
        /// <param name="dwHashAlgo">The hash algorithm</param>
        /// <returns>The data size</returns>
        private static int GetDataSizeByHashAlgo(dwHashAlgo_Values dwHashAlgo)
        {
            int dataSize = 0;
            switch (dwHashAlgo)
            {
                case dwHashAlgo_Values.SHA256:
                    dataSize = 32;
                    break;
                case dwHashAlgo_Values.SHA384:
                    dataSize = 48;
                    break;
                case dwHashAlgo_Values.SHA512:
                    dataSize = 64;
                    break;
            }

            return dataSize;
        }

        #endregion

        #endregion
    }
}

