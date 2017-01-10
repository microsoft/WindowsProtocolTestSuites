// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Protocols.TestTools;
using BKUPParser;
using Decompressor;

namespace FileStreamDataParser
{
    /// <summary>
    /// MS-FSCC Parser Class.
    /// It implements IFSCCParser interface.
    /// </summary>
    public class FSCCAdapter : ManagedAdapterBase, IFSCCAdapter
    {
        #region Variables

        /// <summary>
        /// Stores status of the Parser.
        /// </summary>
        bool isParserSuccessful = true;

        /// <summary>
        /// Index value points to the next byte to be read.
        /// </summary>
        static int index = 0;

        /// <summary>
        /// The maximum size of the Uncompressed buffer
        /// can not be more than 8192.
        /// </summary>
        const int maxUncompressedSize = 8192;

        /// <summary>
        /// Represents the number of streams in the file.
        /// </summary>
        static int streamCount = 0;

        /// <summary>
        /// Creates the object of Helper Class.
        /// </summary>
        Helper objHelper = new Helper();

        #endregion

        /// <summary>
        /// Method to validate the contents of dataBuffer
        /// </summary>
        /// <param name="dataBuffer">
        /// The byte array to be validated.
        /// </param>
        /// <param name="objStructure">
        /// The outparam represents the complete structure
        /// of File stream.
        /// </param>
        /// <returns>
        /// The return value indicates the status of the parser.
        /// </returns>
        public bool ValidateDataBuffer(byte[] dataBuffer,
                                       out ReplicatedFileStructure objStructure)
        {
            ReplicatedFileStructure obj = new ReplicatedFileStructure();
            index = 0;

            #region BlockSignature

            obj.signature.BlockSignature = objHelper.GetSubArray(dataBuffer, index, 8);
            index += 8;

            ASCIIEncoding encoding = new ASCIIEncoding();
            string signature = encoding.GetString(obj.signature.BlockSignature);

            //The string "FRSXXBLO" correspond to a standard signature
            //representing the compressed format.
            if (signature.ToUpper() != "FRSXXBLO")
            {
                isParserSuccessful = false;
                objStructure = obj;
                return isParserSuccessful;
            }

            obj.signature.BlockCompressedSize = BitConverter.ToUInt32(dataBuffer, index);
            index += 4;

            obj.signature.BlockUncompressedSize = BitConverter.ToUInt32(dataBuffer, index);
            index += 4;

            #endregion

            if (obj.signature.BlockUncompressedSize > maxUncompressedSize)
            {
                isParserSuccessful = false;
                objStructure = obj;
                return isParserSuccessful;
            }

            //Invalid Data
            if (obj.signature.BlockCompressedSize > obj.signature.BlockUncompressedSize)
            {
                isParserSuccessful = false;
                objStructure = obj;
                return isParserSuccessful;
            }

            //The data is in Compressed Format
            if (obj.signature.BlockCompressedSize < obj.signature.BlockUncompressedSize)
            {
                Decompressor.Decompressor objDecompressor = new Decompressor.Decompressor();

                byte[] input = objHelper.GetSubArray(dataBuffer, 16, (int)obj.signature.BlockCompressedSize);
                byte[] outputBuffer = new byte[obj.signature.BlockUncompressedSize];

                objDecompressor.Decompress(input, obj.signature.BlockUncompressedSize, out outputBuffer);

                isParserSuccessful = ReadData(outputBuffer, 0, ref obj);
            }

            //The data is in Uncompressed Format
            if (obj.signature.BlockCompressedSize == obj.signature.BlockUncompressedSize)
            {
                isParserSuccessful = ReadData(dataBuffer, 16, ref obj);
            }

            objStructure = obj;
            return isParserSuccessful;
        }

        /// <summary>
        /// Validates the data buffer.
        /// </summary>
        /// <param name="dataBuffer">
        /// input data buffer to be validated.
        /// </param>
        /// <param name="index">
        /// Index from where the data is to be read.
        /// </param>
        /// <param name="obj">
        /// Returns Replicated File Structure
        /// </param>
        /// <returns></returns>
        public bool ReadData(byte[] dataBuffer, int index, ref ReplicatedFileStructure obj)
        {
            obj.streamdata = new Dictionary<int, StreamData>();

            StreamData objStreamData = new StreamData();

            while (index < dataBuffer.Length)
            {
                //Reads the Stream Header                
                ReadStreamHeader(dataBuffer,
                                 streamCount,
                                 ref index,
                                 ref objStreamData);

                if (!isParserSuccessful)
                {
                    return isParserSuccessful;
                }

                switch (objStreamData.Header.streamType)
                {
                    //Read Meata Data Stream
                    #region METADATA STREAM

                    case (uint)StreamType_Values.MS_TYPE_META_DATA:

                        int metaDataSize = (int)objStreamData.Header.blockSize;
                        byte[] metaData = new byte[metaDataSize];
                        metaData = objHelper.GetSubArray(dataBuffer, index, metaDataSize);

                        if (metaDataSize == 0x48)
                        {
                            ValidateMetaData(metaData, ref objStreamData);
                            obj.streamdata.Add(streamCount, objStreamData);
                        }
                        else
                        {
                            isParserSuccessful = false;
                        }

                        index += metaDataSize;
                        streamCount++;
                        break;

                    #endregion

                    //Read Compression Data Stream
                    #region COMPRESSION STREAM

                    case (uint)StreamType_Values.MS_TYPE_COMPRESSION_DATA:

                        int compressionDataSize = (int)objStreamData.Header.blockSize;
                        byte[] compressionData = new byte[compressionDataSize];
                        compressionData = objHelper.GetSubArray(dataBuffer, index, compressionDataSize);

                        if (compressionDataSize == 0x02)
                        {
                            ValidateCompressionData(compressionData, ref objStreamData);
                            obj.streamdata.Add(streamCount, objStreamData);
                        }
                        else
                        {
                            isParserSuccessful = false;
                        }

                        index += compressionDataSize;
                        streamCount++;
                        break;

                    #endregion

                    //Read Reparse Data Stream
                    #region REPARSE STREAM

                    case (uint)StreamType_Values.MS_TYPE_REPARSE_DATA:

                        int reparseDataSize = (int)objStreamData.Header.blockSize;
                        byte[] reparseData = new byte[reparseDataSize];
                        reparseData = objHelper.GetSubArray(dataBuffer, index, reparseDataSize);

                        ValidateReparseData(reparseData, ref objStreamData);
                        obj.streamdata.Add(streamCount, objStreamData);

                        index += reparseDataSize;
                        streamCount++;
                        break;

                    #endregion

                    //Read Flat Data Stream
                    #region FLAT STREAM

                    case (uint)StreamType_Values.MS_TYPE_FLAT_DATA:

                        //This is the last stream of the Marshalled Data Buffer
                        //Can contain multiple Backup Streams.
                        while (index < dataBuffer.Length)
                        {
                            UInt64 flatDataSize = BitConverter.ToUInt64(dataBuffer, index + 8);
                            UInt32 dwStreamNameSize = BitConverter.ToUInt32(dataBuffer, index + 16);

                            int totalSize = (int)(20 + flatDataSize + dwStreamNameSize);
                            byte[] flatData = new byte[totalSize];

                            flatData = objHelper.GetSubArray(dataBuffer,
                                                             index,
                                                             totalSize);

                            BKUPParser.BKUPParser objFlat = new BKUPParser.BKUPParser();
                            FlatDataStream flatStreamData = new FlatDataStream();
                            isParserSuccessful = objFlat.ValidateBKUPDataBuffer(flatData, out flatStreamData);

                            objStreamData.FlatData = flatStreamData;
                            obj.streamdata.Add(streamCount, objStreamData);

                            index += totalSize;
                        }
                        streamCount++;
                        break;

                    #endregion

                    //Read Security Data Stream
                    #region SECURITY STREAM

                    case (uint)StreamType_Values.MS_TYPE_SECURITY_DATA:

                        int securityDataSize = (int)objStreamData.Header.blockSize;
                        byte[] securityData = new byte[securityDataSize];
                        securityData = objHelper.GetSubArray(dataBuffer, index, securityDataSize);

                        ValidateSecurityData(securityData, ref objStreamData);
                        obj.streamdata.Add(streamCount, objStreamData);

                        index += securityDataSize;
                        streamCount++;
                        break;

                    #endregion

                    default:

                        isParserSuccessful = false;
                        return isParserSuccessful;
                }
            }

            return isParserSuccessful;
        }


        /// <summary>
        /// This function reads the stream header 
        /// and identifies the stream type.
        /// </summary>
        /// <param name="dataBuffer">
        /// Input data blob to be read.
        /// </param>
        /// <param name="streamCount">
        /// Represents the number of streams in the file.
        /// </param>
        /// <param name="index">
        /// Index value points to the next byte to be read.
        /// </param>
        /// <param name="objStream">
        /// Fills the stream header values in the 
        /// StreamData structure.
        /// </param>
        public void ReadStreamHeader(byte[] dataBuffer,
                                     int streamCount,
                                     ref int index,
                                     ref StreamData objStream)
        {
            objStream.Header.streamType = BitConverter.ToUInt32(dataBuffer, index);

            if (objStream.Header.streamType <= 0 ||
                objStream.Header.streamType == 5 ||
                objStream.Header.streamType > 7)
            {
                isParserSuccessful = false;
            }
            index += 4;

            objStream.Header.blockSize = BitConverter.ToUInt32(dataBuffer, index);
            index += 4;

            objStream.Header.flags = BitConverter.ToUInt32(dataBuffer, index);

            if (objStream.Header.flags < 0 ||
                objStream.Header.flags > 1)
            {
                isParserSuccessful = false;
            }
            index += 4;
        }

        /// <summary>
        /// Parses the Meta-data stream buffer
        /// </summary>
        /// <param name="metaDataBuffer">
        /// Byte array corresponding to the
        /// Meta-data stream buffer.
        /// </param>
        /// <param name="objStream">
        /// Fills the meta-data stream values in the 
        /// StreamData structure.
        /// </param>
        public void ValidateMetaData(byte[] metaDataBuffer,
                                     ref StreamData objStream)
        {
            int localIndex = 0;

            objStream.MetaData.Version = BitConverter.ToUInt32(metaDataBuffer, localIndex);
            localIndex += 4;

            if (objStream.MetaData.Version != 3)
            {
                isParserSuccessful = false;
            }

            objStream.MetaData.Reserved1 =
                objHelper.GetSubArray(metaDataBuffer, localIndex, 4);
            localIndex += 4;

            objStream.MetaData.FileBasicInfo.CreationTime.dwLowDateTime =
                BitConverter.ToUInt32(metaDataBuffer, localIndex);
            objStream.MetaData.FileBasicInfo.CreationTime.dwHighDateTime =
                BitConverter.ToUInt32(metaDataBuffer, localIndex + 4);
            localIndex += 8;

            objStream.MetaData.FileBasicInfo.LastAccessTime.dwLowDateTime =
                BitConverter.ToUInt32(metaDataBuffer, localIndex);
            objStream.MetaData.FileBasicInfo.LastAccessTime.dwHighDateTime =
                BitConverter.ToUInt32(metaDataBuffer, localIndex + 4);
            localIndex += 8;

            objStream.MetaData.FileBasicInfo.LastWriteTime.dwLowDateTime =
                BitConverter.ToUInt32(metaDataBuffer, localIndex);
            objStream.MetaData.FileBasicInfo.LastWriteTime.dwHighDateTime =
                BitConverter.ToUInt32(metaDataBuffer, localIndex + 4);
            localIndex += 8;

            objStream.MetaData.FileBasicInfo.ChangeTime.dwLowDateTime =
                BitConverter.ToUInt32(metaDataBuffer, localIndex);
            objStream.MetaData.FileBasicInfo.ChangeTime.dwHighDateTime =
                BitConverter.ToUInt32(metaDataBuffer, localIndex + 4);
            localIndex += 8;

            objStream.MetaData.FileBasicInfo.FileAttribute =
                BitConverter.ToUInt32(metaDataBuffer, localIndex);

            UInt32 fileAttributeValue = objStream.MetaData.FileBasicInfo.FileAttribute;
            isParserSuccessful = CheckIfValidFileAttribute(fileAttributeValue);
            localIndex += 4;

            objStream.MetaData.FileBasicInfo.Reserved =
                BitConverter.ToUInt32(metaDataBuffer, localIndex);
            localIndex += 4;

            objStream.MetaData.sdControl =
                BitConverter.ToUInt16(metaDataBuffer, localIndex);
            localIndex += 2;

            objStream.MetaData.Reserved2 =
                objHelper.GetSubArray(metaDataBuffer, localIndex, 6);
            localIndex += 6;

            objStream.MetaData.primaryDataStreamSize =
                BitConverter.ToUInt64(metaDataBuffer, localIndex);
            localIndex += 8;

            objStream.MetaData.Reserved3 =
                objHelper.GetSubArray(metaDataBuffer, localIndex, 8);
            localIndex += 8;
        }

        /// <summary>
        /// Parses the Security-data stream buffer
        /// </summary>
        /// <param name="securityDataBuffer">
        /// Byte array corresponding to the
        /// Security-data stream buffer.
        /// </param>
        /// <param name="objStream">
        /// Fills the security-data stream values in the 
        /// StreamData structure.
        /// </param>
        public void ValidateSecurityData(byte[] securityDataBuffer,
                                         ref StreamData objStream)
        {
            int localIndex = 0;

            objStream.SecurityData.securityData = objHelper.GetSubArray(securityDataBuffer,
                                                                        localIndex,
                                                                        securityDataBuffer.Length);
        }

        /// <summary>
        /// Parses the Compression-data stream buffer
        /// </summary>
        /// <param name="compressionDataBuffer">
        /// Byte array corresponding to the
        /// Compression-data stream buffer.
        /// </param>
        /// <param name="objStream">
        /// Fills the Compression-data stream values in the 
        /// StreamData structure.
        /// </param>
        public void ValidateCompressionData(byte[] compressionDataBuffer,
                                            ref StreamData objStream)
        {
            int localIndex = 0;

            UInt16 compressionFormat = BitConverter.ToUInt16(compressionDataBuffer, localIndex);

            if (Enum.IsDefined(typeof(CompressionFormat_Values), compressionFormat))
            {
                objStream.CompressionData.CompressionFormat = (CompressionFormat_Values)compressionFormat;
            }
            else
            {
                isParserSuccessful = false;
            }
        }

        /// <summary>
        /// Parses the Reparse-data stream buffer.
        /// </summary>
        /// <param name="reparseDataBuffer">
        /// Byte array corresponding to the
        /// Reparse-data stream buffer.
        /// </param>
        /// <param name="objStream">
        /// Fills the Reparse-data stream values in the 
        /// StreamData structure.
        /// </param>
        public void ValidateReparseData(byte[] reparseDataBuffer,
                                            ref StreamData objStream)
        {
            int localIndex = 0;
            UnicodeEncoding encoding = new UnicodeEncoding();

            UInt32 reparseTag = BitConverter.ToUInt32(reparseDataBuffer, localIndex);
            objStream.ReparseData.ReparseTag = reparseTag;
            localIndex += 4;

            //REPARSE_DATA_BUFFER
            if ((reparseTag & 0x80000000) == 0x80000000)
            {
                objStream.ReparseData.DATA_BUFFER.ReparseDataLength =
                    BitConverter.ToUInt16(reparseDataBuffer, localIndex);

                objStream.ReparseData.DATA_BUFFER.Reserved =
                    objHelper.GetSubArray(reparseDataBuffer, localIndex + 2, 2);

                //IO_REPARSE_TAG_SYMLINK
                if (reparseTag == (UInt32)ReparseTag_Values.IO_REPARSE_TAG_SYMLINK)
                {
                    objStream.ReparseData.DATA_BUFFER.symbolicLinkData.SubstituteNameOffset =
                    BitConverter.ToUInt16(reparseDataBuffer, localIndex + 4);

                    objStream.ReparseData.DATA_BUFFER.symbolicLinkData.SubstituteNameLength =
                        BitConverter.ToUInt16(reparseDataBuffer, localIndex + 6);

                    objStream.ReparseData.DATA_BUFFER.symbolicLinkData.PrintNameOffset =
                        BitConverter.ToUInt16(reparseDataBuffer, localIndex + 8);

                    objStream.ReparseData.DATA_BUFFER.symbolicLinkData.PrintNameLength =
                        BitConverter.ToUInt16(reparseDataBuffer, localIndex + 10);

                    objStream.ReparseData.DATA_BUFFER.symbolicLinkData.Flags =
                        BitConverter.ToUInt32(reparseDataBuffer, localIndex + 12);

                    objStream.ReparseData.DATA_BUFFER.symbolicLinkData.PathBuffer =
                        objHelper.GetSubArray(reparseDataBuffer,
                                              localIndex + 16,
                                              reparseDataBuffer.Length - (16 + 4));

                    objStream.ReparseData.DATA_BUFFER.symbolicLinkData.SubstituteName =
                        encoding.GetString(objStream.ReparseData.DATA_BUFFER.symbolicLinkData.PathBuffer,
                                           objStream.ReparseData.DATA_BUFFER.symbolicLinkData.SubstituteNameOffset,
                                           objStream.ReparseData.DATA_BUFFER.symbolicLinkData.SubstituteNameLength);

                    objStream.ReparseData.DATA_BUFFER.symbolicLinkData.PrintName =
                        encoding.GetString(objStream.ReparseData.DATA_BUFFER.symbolicLinkData.PathBuffer,
                                           objStream.ReparseData.DATA_BUFFER.symbolicLinkData.PrintNameOffset,
                                           objStream.ReparseData.DATA_BUFFER.symbolicLinkData.PrintNameLength);
                }

                //IO_REPARSE_TAG_MOUNT_POINT
                else if (reparseTag == (UInt32)ReparseTag_Values.IO_REPARSE_TAG_MOUNT_POINT)
                {
                    objStream.ReparseData.DATA_BUFFER.mountPointData.SubstituteNameOffset =
                    BitConverter.ToUInt16(reparseDataBuffer, localIndex + 4);

                    objStream.ReparseData.DATA_BUFFER.mountPointData.SubstituteNameLength =
                        BitConverter.ToUInt16(reparseDataBuffer, localIndex + 6);

                    objStream.ReparseData.DATA_BUFFER.mountPointData.PrintNameOffset =
                        BitConverter.ToUInt16(reparseDataBuffer, localIndex + 8);

                    objStream.ReparseData.DATA_BUFFER.mountPointData.PrintNameLength =
                        BitConverter.ToUInt16(reparseDataBuffer, localIndex + 10);

                    objStream.ReparseData.DATA_BUFFER.mountPointData.PathBuffer =
                        objHelper.GetSubArray(reparseDataBuffer,
                                              localIndex + 12,
                                              reparseDataBuffer.Length - (12 + 4));

                    objStream.ReparseData.DATA_BUFFER.mountPointData.SubstituteName =
                        encoding.GetString(objStream.ReparseData.DATA_BUFFER.mountPointData.PathBuffer,
                                           objStream.ReparseData.DATA_BUFFER.mountPointData.SubstituteNameOffset,
                                           objStream.ReparseData.DATA_BUFFER.mountPointData.SubstituteNameLength);

                    objStream.ReparseData.DATA_BUFFER.mountPointData.PrintName =
                        encoding.GetString(objStream.ReparseData.DATA_BUFFER.mountPointData.PathBuffer,
                                           objStream.ReparseData.DATA_BUFFER.mountPointData.PrintNameOffset,
                                           objStream.ReparseData.DATA_BUFFER.mountPointData.PrintNameLength);
                }
                else
                {
                    isParserSuccessful = false;
                }
            }

            //REPARSE_GUID_DATA_BUFFER
            else
            {
                objStream.ReparseData.GUID_DATA_BUFFER.ReparseDataLength =
                    BitConverter.ToUInt16(reparseDataBuffer, localIndex);

                objStream.ReparseData.GUID_DATA_BUFFER.Reserved =
                    objHelper.GetSubArray(reparseDataBuffer, localIndex + 2, 2);

                objStream.ReparseData.GUID_DATA_BUFFER.ReparseGuid =
                    new Guid(objHelper.GetSubArray(reparseDataBuffer, localIndex + 4, 16));

                objStream.ReparseData.GUID_DATA_BUFFER.ReparseDataBuffer =
                    objHelper.GetSubArray(reparseDataBuffer, localIndex + 20, reparseDataBuffer.Length - (20 + 4));
            }
        }

        /// <summary>
        /// Checks if a file attribute is valid or not.
        /// </summary>
        /// <param name="fileAttributeValue">
        /// File attribute value to be checked.
        /// </param>
        /// <returns>
        /// Returns true is valid file attribute value, else false.
        /// </returns>
        public bool CheckIfValidFileAttribute(UInt32 fileAttributeValue)
        {
            if ((fileAttributeValue & 0x00008000) == 0x00008000)
            {
                isParserSuccessful = false;
                return isParserSuccessful;
            }

            if ((fileAttributeValue & 0x00000040) == 0x00000040)
            {
                isParserSuccessful = false;
                return isParserSuccessful;
            }

            if ((fileAttributeValue & 0x00000008) == 0x00000008)
            {
                isParserSuccessful = false;
                return isParserSuccessful;
            }

            //Greater than 2^14
            if (fileAttributeValue > 16384)
            {
                isParserSuccessful = false;
                return isParserSuccessful;
            }

            return isParserSuccessful;
        }

        /// <summary>
        /// Stores all the file format values that applies to a file.
        /// </summary>
        /// <param name="isValidFileAttribute">
        /// this method should be called only when
        /// CheckIfValidFileAttribute method returns true.
        /// </param>
        /// <param name="fileAttribute">
        /// FileAttribute to add.
        /// </param>
        /// <param name="countOfAttributes">
        /// returns the count of count of attributes in the Dictionary.
        /// </param>
        /// <returns>
        /// Returns dictionary of File Attributes
        /// </returns>
        public Dictionary<int, FileAttribute> FileAttributeList(bool isValidFileAttribute,
                                                         UInt32 fileAttribute,
                                                         out int countOfAttributes)
        {
            int attributeCount = 0;
            Dictionary<int, FileAttribute> AttributeList = new Dictionary<int, FileAttribute>();

            if (isValidFileAttribute)
            {
                //FILE_ATTRIBUTE_READONLY: 0x00000001
                if ((fileAttribute & (UInt32)FileAttribute.FILE_ATTRIBUTE_READONLY)
                    == (UInt32)FileAttribute.FILE_ATTRIBUTE_READONLY)
                {
                    AttributeList.Add(attributeCount, FileAttribute.FILE_ATTRIBUTE_READONLY);
                    attributeCount++;
                }

                //FILE_ATTRIBUTE_HIDDEN: 0x00000002
                if ((fileAttribute & (UInt32)FileAttribute.FILE_ATTRIBUTE_HIDDEN)
                    == (UInt32)FileAttribute.FILE_ATTRIBUTE_HIDDEN)
                {
                    AttributeList.Add(attributeCount, FileAttribute.FILE_ATTRIBUTE_HIDDEN);
                    attributeCount++;
                }

                //FILE_ATTRIBUTE_SYSTEM: 0x00000004
                if ((fileAttribute & (UInt32)FileAttribute.FILE_ATTRIBUTE_SYSTEM)
                    == (UInt32)FileAttribute.FILE_ATTRIBUTE_SYSTEM)
                {
                    AttributeList.Add(attributeCount, FileAttribute.FILE_ATTRIBUTE_SYSTEM);
                    attributeCount++;
                }

                //FILE_ATTRIBUTE_DIRECTORY: 0x00000010
                if ((fileAttribute & (UInt32)FileAttribute.FILE_ATTRIBUTE_DIRECTORY)
                    == (UInt32)FileAttribute.FILE_ATTRIBUTE_DIRECTORY)
                {
                    AttributeList.Add(attributeCount, FileAttribute.FILE_ATTRIBUTE_DIRECTORY);
                    attributeCount++;
                }

                //FILE_ATTRIBUTE_ARCHIVE: 0x00000020
                if ((fileAttribute & (UInt32)FileAttribute.FILE_ATTRIBUTE_ARCHIVE)
                    == (UInt32)FileAttribute.FILE_ATTRIBUTE_ARCHIVE)
                {
                    AttributeList.Add(attributeCount, FileAttribute.FILE_ATTRIBUTE_ARCHIVE);
                    attributeCount++;
                }

                //FILE_ATTRIBUTE_NORMAL: 0x00000080
                if ((fileAttribute & (UInt32)FileAttribute.FILE_ATTRIBUTE_NORMAL)
                    == (UInt32)FileAttribute.FILE_ATTRIBUTE_NORMAL)
                {
                    AttributeList.Add(attributeCount, FileAttribute.FILE_ATTRIBUTE_NORMAL);
                    attributeCount++;
                }

                //FILE_ATTRIBUTE_TEMPORARY: 0x00000100
                if ((fileAttribute & (UInt32)FileAttribute.FILE_ATTRIBUTE_TEMPORARY)
                    == (UInt32)FileAttribute.FILE_ATTRIBUTE_TEMPORARY)
                {
                    AttributeList.Add(attributeCount, FileAttribute.FILE_ATTRIBUTE_TEMPORARY);
                    attributeCount++;
                }

                //FILE_ATTRIBUTE_SPARSE_FILE: 0x00000200
                if ((fileAttribute & (UInt32)FileAttribute.FILE_ATTRIBUTE_SPARSE_FILE)
                    == (UInt32)FileAttribute.FILE_ATTRIBUTE_SPARSE_FILE)
                {
                    AttributeList.Add(attributeCount, FileAttribute.FILE_ATTRIBUTE_SPARSE_FILE);
                    attributeCount++;
                }

                //FILE_ATTRIBUTE_REPARSE_POINT: 0x00000400
                if ((fileAttribute & (UInt32)FileAttribute.FILE_ATTRIBUTE_REPARSE_POINT)
                    == (UInt32)FileAttribute.FILE_ATTRIBUTE_REPARSE_POINT)
                {
                    AttributeList.Add(attributeCount, FileAttribute.FILE_ATTRIBUTE_REPARSE_POINT);
                    attributeCount++;
                }

                //FILE_ATTRIBUTE_COMPRESSED: 0x00000800
                if ((fileAttribute & (UInt32)FileAttribute.FILE_ATTRIBUTE_COMPRESSED)
                    == (UInt32)FileAttribute.FILE_ATTRIBUTE_COMPRESSED)
                {
                    AttributeList.Add(attributeCount, FileAttribute.FILE_ATTRIBUTE_COMPRESSED);
                    attributeCount++;
                }

                //FILE_ATTRIBUTE_OFFLINE: 0x00001000
                if ((fileAttribute & (UInt32)FileAttribute.FILE_ATTRIBUTE_OFFLINE)
                    == (UInt32)FileAttribute.FILE_ATTRIBUTE_OFFLINE)
                {
                    AttributeList.Add(attributeCount, FileAttribute.FILE_ATTRIBUTE_OFFLINE);
                    attributeCount++;
                }

                //FILE_ATTRIBUTE_NOT_CONTENT_INDEXED: 0x00002000
                if ((fileAttribute & (UInt32)FileAttribute.FILE_ATTRIBUTE_NOT_CONTENT_INDEXED)
                    == (UInt32)FileAttribute.FILE_ATTRIBUTE_NOT_CONTENT_INDEXED)
                {
                    AttributeList.Add(attributeCount, FileAttribute.FILE_ATTRIBUTE_NOT_CONTENT_INDEXED);
                    attributeCount++;
                }

                //FILE_ATTRIBUTE_ENCRYPTED: 0x00004000
                if ((fileAttribute & (UInt32)FileAttribute.FILE_ATTRIBUTE_ENCRYPTED)
                    == (UInt32)FileAttribute.FILE_ATTRIBUTE_ENCRYPTED)
                {
                    AttributeList.Add(attributeCount, FileAttribute.FILE_ATTRIBUTE_ENCRYPTED);
                    attributeCount++;
                }

                //FILE_ATTRIBUTE_INVALID: 0x00000000
                if (fileAttribute == (UInt32)FileAttribute.FILE_ATTRIBUTE_INVALID)
                {
                    AttributeList.Add(attributeCount, FileAttribute.FILE_ATTRIBUTE_INVALID);
                    attributeCount = 1;
                }
            }

            countOfAttributes = attributeCount;
            return AttributeList;
        }

    }
}
