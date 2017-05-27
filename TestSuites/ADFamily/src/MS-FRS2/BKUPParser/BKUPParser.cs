// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace BKUPParser
{
    /// <summary>
    /// BKUPParser Class which implements the IBKUPParser interface.
    /// </summary>
    public class BKUPParser : IBKUPParser
    {
        /// <summary>
        /// Creates an object of Helper Class.
        /// </summary>
        Helper objHelper = new Helper();

        /// <summary>
        /// bool variavle which stores the state of Parser.
        /// </summary>
        static bool isParserSuccessful = true;

        /// <summary>
        /// int variable which serves as index to iterate through the data buffer.
        /// </summary>
        static int index = 0;

        /// <summary>
        /// Validates the data buffer.
        /// </summary>
        /// <param name="dataBuffer">
        /// Input Parameter, Data Buffer to be parsed.
        /// </param>
        /// <param name="flatData">
        /// Output Parameter, the filled FlatDataStream
        /// structure which contains various backup streams.
        /// </param>
        /// <returns></returns>
        public bool ValidateBKUPDataBuffer(byte[] dataBuffer, out FlatDataStream flatData)
        {
            index = 0;
            
            //Creates an object of FlatStream Structure.
            FlatDataStream flatStreamData = new FlatDataStream();

            //Iterates through the data buffer for backup streams.
            for (int streamCount = 0; index < dataBuffer.Length; streamCount++)
            {
                UInt64 Size = BitConverter.ToUInt64(dataBuffer, index + 8);
                UInt32 dwStreamNameSize = BitConverter.ToUInt32(dataBuffer, index + 16);
                UInt64 streamSize = 20 + Size + dwStreamNameSize;

                //Reads a backup stream and calls the ValidateBKUPStream method on that stream.
                byte[] streamData = objHelper.GetSubArray(dataBuffer, index, (int)streamSize);
                
                //Validates one Backup stream.
                isParserSuccessful = ValidateBKUPStream(streamData, ref flatStreamData);

                index += (int)streamSize;
            }

            flatData = flatStreamData;
            return isParserSuccessful;
        }

        /// <summary>
        /// Validates a backup stream.
        /// </summary>
        /// <param name="dataBuffer">
        /// Input Parameter, Data Buffer to be parsed.
        /// </param>
        /// <param name="flatData">
        /// Returns the filled FlatDataStream
        /// structure which contains various backup streams.
        /// </param>
        /// <returns></returns>
        private bool ValidateBKUPStream(byte[] dataBuffer, ref FlatDataStream flatData)
        {
            int localIndex = 0;
            
            UInt32 dwStreamId = BitConverter.ToUInt32(dataBuffer, localIndex);
            
            //Checks the dwStreamId value.
            bool checkStreamID = checkIfValidStreamID(dwStreamId);

            if (!checkStreamID)
            {
                isParserSuccessful = false;
                return isParserSuccessful;
            }
            else
            {
                flatData.dwStreamId = (dwStreamId_Values)dwStreamId;
            }

            localIndex += 4;

            UInt32 dwStreamAttributes = BitConverter.ToUInt32(dataBuffer, localIndex);

            //Checks the dwStreamAttributes value.
            bool checkStreamAttr = checkIfValidStreamAttr(dwStreamAttributes);

            if (!checkStreamAttr)
            {
                isParserSuccessful = false;
                return isParserSuccessful;
            }
            else
            {
                flatData.dwStreamAttributes = (dwStreamAttributes_Values)dwStreamAttributes;
            }

            localIndex += 4;

            //Reads the size of backup stream.
            flatData.Size = BitConverter.ToUInt64(dataBuffer, localIndex);
            
            //Reads the size of stream name.
            flatData.dwStreamNameSize = BitConverter.ToUInt32(dataBuffer, localIndex + 8);
            localIndex += 12;

            if ((dwStreamId != (UInt32)dwStreamId_Values.ALTERNATE_DATA) && (flatData.dwStreamNameSize > 0) ||
                (dwStreamId == (UInt32)dwStreamId_Values.ALTERNATE_DATA) && (flatData.dwStreamNameSize == 0))
            {
                isParserSuccessful = false;
                return isParserSuccessful;
            }

            //reads stream name if it exists
            if (flatData.dwStreamNameSize > 0)
            {
                //retrieves a byte array equal to dwStreamNameSize
                //to read UnicodeEncoding cStreamName field value.
                flatData.cStreamName = objHelper.GetSubArray(dataBuffer,
                                                             localIndex,
                                                             (int)flatData.dwStreamNameSize);

                //Reads Stream Name in string format.
                UnicodeEncoding encoding = new UnicodeEncoding();
                flatData.strStreamName = encoding.GetString(flatData.cStreamName);

                //Checks whether the stream name starts with a colon character.
                if (!flatData.strStreamName.StartsWith(":"))
                {
                    isParserSuccessful = false;
                    return isParserSuccessful;
                }

                localIndex += (int)flatData.dwStreamNameSize;
            }

            if (flatData.Size > 0)
            {
                //Determines the stream ID and validates the corresponding stream data.
                byte[] data = objHelper.GetSubArray(dataBuffer, localIndex, (int)flatData.Size);
                isParserSuccessful = ValidateData(data, flatData.dwStreamId, ref flatData);

                localIndex += (int)flatData.Size;
            }

            return isParserSuccessful;
        }

        //Validates dwStreamId field.
        public bool checkIfValidStreamID(UInt32 dwStreamId)
        {
            bool check = false;
            try
            {
                check = Enum.IsDefined(typeof(dwStreamId_Values), dwStreamId);
            }
            catch
            {
                return false;
            }

            return check;
        }

        //Validates dwStreamId field.
        public bool checkIfValidStreamAttr(UInt32 dwStreamAttributes)
        {
            bool check = false;
            try
            {
                check = Enum.IsDefined(typeof(dwStreamAttributes_Values), dwStreamAttributes);
            }
            catch
            {
                return false;
            }

            return check;
        }

        /// <summary>
        /// Validates the backup stream based on the particular stream ID.
        /// </summary>
        /// <param name="data">
        /// Input Parameter, Data Buffer to be parsed.
        /// </param>
        /// <param name="dwStreamId">
        /// Stream ID of the backup stream.
        /// </param>
        /// <param name="flatData">
        /// Returns the filled FlatDataStream
        /// structure which contains various backup streams.
        /// </param>
        /// <returns></returns>
        public bool ValidateData(byte[] data, dwStreamId_Values dwStreamId, ref FlatDataStream flatData)
        {
            bool result = true;
            int dataIndex = 0;            
            ASCIIEncoding encoding = new ASCIIEncoding();

            switch (dwStreamId)
            {
                case dwStreamId_Values.ALTERNATE_DATA:

                    //Reads the Alternate Data Backup Stream
                    flatData.AlternateData.streamCount++;
                    flatData.AlternateData.data = data;                    
                    flatData.AlternateData.strData = encoding.GetString(data);
                    break;

                case dwStreamId_Values.DATA:

                    //Reads the Data Backup Stream                    
                    flatData.Data.data = data;
                    flatData.Data.strData = encoding.GetString(data);
                    break;

                case dwStreamId_Values.EA_DATA:
                    
                    //This stream is not found on wire.
                    break;

                case dwStreamId_Values.LINK:

                    //This stream is not found on wire.
                    break;

                case dwStreamId_Values.OBJECT_ID:

                    //Reads the Object ID Backup Stream
                    if (data.Length != 64)
                    {
                        result = false;
                    }
                    else
                    {
                        flatData.ObjectID.objectID = new Guid(objHelper.GetSubArray(data, dataIndex, 16));
                        dataIndex += 16;

                        flatData.ObjectID.birthVolumeID = new Guid(objHelper.GetSubArray(data, dataIndex, 16));
                        dataIndex += 16;

                        flatData.ObjectID.birthObjectID = new Guid(objHelper.GetSubArray(data, dataIndex, 16));
                        dataIndex += 16;

                        flatData.ObjectID.domainID = new Guid(objHelper.GetSubArray(data, dataIndex, 16));
                        dataIndex += 16;

                        Guid nullGuid = new Guid();
                        if (flatData.ObjectID.domainID != nullGuid)
                        {
                            result = false;
                        }
                    }
                    break;

                case dwStreamId_Values.REPARSE_DATA:
                    
                    //Reads the Reparse Data Backup Stream
                    ValidateReparseData(data, ref flatData.ReparseData);
                    break;

                case dwStreamId_Values.SECURITY_DATA:

                    //This stream is not found on wire.
                    break;

                case dwStreamId_Values.SPARSE_BLOCK:

                    //This stream is not found on wire.
                    break;

                case dwStreamId_Values.TXFS_DATA:

                    //This stream is not found on wire.
                    break;
            }

            return result;
        }

        /// <summary>
        /// Validates the reparse data backup stream.
        /// </summary>
        /// <param name="reparseDataBuffer">
        /// Reparse Data Buffer
        /// </param>
        /// <param name="ReparseData">
        /// Returns the FlatData_REPARSE_DATA structure.
        /// </param>
        private void ValidateReparseData(byte[] reparseDataBuffer,
                                           ref FlatData_REPARSE_DATA ReparseData)
        {
            int localIndex = 0;
            UnicodeEncoding encoding = new UnicodeEncoding();

            UInt32 reparseTag = BitConverter.ToUInt32(reparseDataBuffer, localIndex);
            ReparseData.ReparseTag = reparseTag;
            localIndex += 4;

            //REPARSE_DATA_BUFFER
            if ((reparseTag & 0x80000000) == 0x80000000)
            {
                ReparseData.DATA_BUFFER.ReparseDataLength =
                    BitConverter.ToUInt16(reparseDataBuffer, localIndex);

                ReparseData.DATA_BUFFER.Reserved =
                    objHelper.GetSubArray(reparseDataBuffer, localIndex + 2, 2);

                //IO_REPARSE_TAG_SYMLINK
                if (reparseTag == (UInt32)FlatData_ReparseTag_Values.IO_REPARSE_TAG_SYMLINK)
                {
                    ReparseData.DATA_BUFFER.symbolicLinkData.SubstituteNameOffset =
                    BitConverter.ToUInt16(reparseDataBuffer, localIndex + 4);

                    ReparseData.DATA_BUFFER.symbolicLinkData.SubstituteNameLength =
                        BitConverter.ToUInt16(reparseDataBuffer, localIndex + 6);

                    ReparseData.DATA_BUFFER.symbolicLinkData.PrintNameOffset =
                        BitConverter.ToUInt16(reparseDataBuffer, localIndex + 8);

                    ReparseData.DATA_BUFFER.symbolicLinkData.PrintNameLength =
                        BitConverter.ToUInt16(reparseDataBuffer, localIndex + 10);

                    ReparseData.DATA_BUFFER.symbolicLinkData.Flags =
                        BitConverter.ToUInt32(reparseDataBuffer, localIndex + 12);

                    ReparseData.DATA_BUFFER.symbolicLinkData.PathBuffer =
                        objHelper.GetSubArray(reparseDataBuffer,
                                              localIndex + 16,
                                              reparseDataBuffer.Length - (16 + 4));

                    ReparseData.DATA_BUFFER.symbolicLinkData.SubstituteName =
                        encoding.GetString(ReparseData.DATA_BUFFER.symbolicLinkData.PathBuffer,
                                           ReparseData.DATA_BUFFER.symbolicLinkData.SubstituteNameOffset,
                                           ReparseData.DATA_BUFFER.symbolicLinkData.SubstituteNameLength);

                    ReparseData.DATA_BUFFER.symbolicLinkData.PrintName =
                        encoding.GetString(ReparseData.DATA_BUFFER.symbolicLinkData.PathBuffer,
                                           ReparseData.DATA_BUFFER.symbolicLinkData.PrintNameOffset,
                                           ReparseData.DATA_BUFFER.symbolicLinkData.PrintNameLength);
                }

                //IO_REPARSE_TAG_MOUNT_POINT
                else if (reparseTag == (UInt32)FlatData_ReparseTag_Values.IO_REPARSE_TAG_MOUNT_POINT)
                {
                    ReparseData.DATA_BUFFER.mountPointData.SubstituteNameOffset =
                    BitConverter.ToUInt16(reparseDataBuffer, localIndex + 4);

                    ReparseData.DATA_BUFFER.mountPointData.SubstituteNameLength =
                        BitConverter.ToUInt16(reparseDataBuffer, localIndex + 6);

                    ReparseData.DATA_BUFFER.mountPointData.PrintNameOffset =
                        BitConverter.ToUInt16(reparseDataBuffer, localIndex + 8);

                    ReparseData.DATA_BUFFER.mountPointData.PrintNameLength =
                        BitConverter.ToUInt16(reparseDataBuffer, localIndex + 10);

                    ReparseData.DATA_BUFFER.mountPointData.PathBuffer =
                        objHelper.GetSubArray(reparseDataBuffer,
                                              localIndex + 12,
                                              reparseDataBuffer.Length - (12 + 4));

                    ReparseData.DATA_BUFFER.mountPointData.SubstituteName =
                        encoding.GetString(ReparseData.DATA_BUFFER.mountPointData.PathBuffer,
                                           ReparseData.DATA_BUFFER.mountPointData.SubstituteNameOffset,
                                           ReparseData.DATA_BUFFER.mountPointData.SubstituteNameLength);

                    ReparseData.DATA_BUFFER.mountPointData.PrintName =
                        encoding.GetString(ReparseData.DATA_BUFFER.mountPointData.PathBuffer,
                                           ReparseData.DATA_BUFFER.mountPointData.PrintNameOffset,
                                           ReparseData.DATA_BUFFER.mountPointData.PrintNameLength);
                }
                else
                {
                    isParserSuccessful = false;
                }
            }

            //REPARSE_GUID_DATA_BUFFER
            else
            {
                ReparseData.GUID_DATA_BUFFER.ReparseDataLength =
                    BitConverter.ToUInt16(reparseDataBuffer, localIndex);

                ReparseData.GUID_DATA_BUFFER.Reserved =
                    objHelper.GetSubArray(reparseDataBuffer, localIndex + 2, 2);

                ReparseData.GUID_DATA_BUFFER.ReparseGuid =
                    new Guid(objHelper.GetSubArray(reparseDataBuffer, localIndex + 4, 16));

                ReparseData.GUID_DATA_BUFFER.ReparseDataBuffer =
                    objHelper.GetSubArray(reparseDataBuffer, localIndex + 20, reparseDataBuffer.Length - (20 + 4));
            }
        }
    }
}
