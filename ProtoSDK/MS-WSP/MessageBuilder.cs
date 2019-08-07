// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP
{
    public class MessageBuilderColumnParameter
    {
        public Guid Guid;

        public uint PropertyId;

        public ushort ValueOffset;

        public ushort StatusOffset;

        public ushort LengthOffset;

        public StorageType StorageType;
    }

    public class CreateQueryColumnParameter
    {
        public Guid Guid;
        public uint PropertyId;
    }

    public class MessageBuilderParameter
    {
        public Guid EmptyGuid;

        public Guid PropertySet_One_Guid;

        public string[] PropertySet_One_DBProperties;

        public Guid PropertySet_Two_Guid;

        public string[] PropertySet_Two_DBProperties;

        public Guid Array_PropertySet_One_Guid;

        public string[] Array_PropertySet_One_DBProperties;

        public Guid Array_PropertySet_Two_Guid;

        public string[] Array_PropertySet_Two_DBProperties;

        public Guid Array_PropertySet_Three_Guid;

        public string[] Array_PropertySet_Three_DBProperties;

        public Guid Array_PropertySet_Four_Guid;

        public string[] Array_PropertySet_Four_DBProperties;

        public uint EachRowSize;

        public Guid PropertyRestrictionGuid;

        public int PropertyRestrictionProperty;

        public Guid ContentRestrictionGuid;

        public int ContentRestrictionProperty;

        public uint EType;

        public uint BufferSize;

        public uint LCID_VALUE;

        public uint ClientBase;

        public uint RowsToTransfer;

        public int NumberOfSetBindingsColumns;

        public int NumberOfCreateQueryColumns;

        public MessageBuilderColumnParameter[] ColumnParameters;

        public CreateQueryColumnParameter[] CreateQueryColumnParameters;


        public Guid PropertyGuidToFetch;

        public int PropertyIdToFetch;
    }

    /// <summary>
    /// Message Builder class provides methods to 
    /// build MS-WSP request messages
    /// </summary>
    public class MessageBuilder
    {
        #region Fields

        /// <summary>
        /// Specifies a field is Used
        /// </summary>
        byte FIELD_USED = 0x01;
        /// <summary>
        /// Specifies a field is NOT used
        /// </summary>
        byte FIELD_NOT_USED = 0x00;
        /// <summary>
        /// Specifies alignment of 4 bytes
        /// </summary>
        byte OFFSET_4 = 4;
        /// <summary>
        /// Specifies alignment of 8 bytes
        /// </summary>
        byte OFFSET_8 = 8;
        /// <summary>
        /// Represent a search scope
        /// </summary>
        string searchScope = string.Empty;
        /// <summary>
        /// Represent a query string
        /// </summary>
        string queryString = string.Empty;

        /// <summary>
        /// Specifies the property type is ID
        /// </summary>
        int PROPERTY_ID = 0x1;

        /// <summary>
        /// Length of each message header
        /// </summary>
        int HEADER_LENGTH = 16;

        /// <summary>
        /// Specifies comma separated properties
        /// </summary>
        char[] delimiter = new char[] { ',' };
        /// <summary>
        /// Number of external property set count
        /// </summary>
        uint EXTERNAL_PROPSET_COUNT = 4;

        /// <summary>
        /// Value to XOR with for Header checksum
        /// </summary>
        uint CHECKSUM_XOR_VALUE = 0x59533959;

        /// <summary>
        /// Value to OR with for Safe Array Type
        /// </summary>
        ushort SAFE_ARRAY_TYPE = 0x2000;

        /// <summary>
        /// Value to OR with for Vector Type
        /// </summary>
        ushort VECTOR_TYPE = 0x1000;

        /// <summary>
        /// Command time out
        /// </summary>
        uint COMMAND_TIME_OUT = 0x1E;
        /// <summary>
        /// Content Restriction operation
        /// </summary>
        uint RELATION_OPERATION = 0x4;

        /// <summary>
        /// Id of Property Restrictin Node
        /// </summary>
        uint PROPERTY_RESTRICTION_NODE_ID = 5;

        /// <summary>
        /// Id of Content Restriction Node
        /// </summary>
        uint CONTENT_RESTRICTION_NODE_ID = 4;

        /// <summary>
        /// Wieghtage of the Node
        /// </summary>
        uint NODE_WEIGHTAGE = 1000;

        /// <summary>
        /// Means Logical AND operation between Node Restriction
        /// </summary>
        uint LOGICAL_AND = 0x1;

        //newC
        /// <summary>
        /// static value for chapter of CPMGetRowsIn message
        /// </summary>
        public static uint chapter;

        public static uint rowWidth = 40;


        public MessageBuilderParameter parameter;

        #endregion

        /// <summary>
        /// constructor takes the ITestSite as parameter
        /// </summary>
        /// <param name="testSite">Site from where it needs 
        /// to read configurable data</param>
        public MessageBuilder(MessageBuilderParameter parameter)
        {
            this.parameter = parameter;
        }

        #region MS-WSP Request Messages
        /// <summary>
        /// Builds CPMConnectIn message 
        /// </summary>
        /// <param name="clientVersion">Version of the
        /// protocol client</param>
        /// <param name="isRemote">If the query is remote, 
        /// then 1 else 0</param>
        /// <param name="userName">User initiating the connection</param>
        /// <param name="machineName">Client Machine Name</param>
        /// <param name="serverMachineName">Server Machine Name</param>
        /// <param name="catalogName">Name of the Catalog 
        /// under operation</param>
        /// <param name="languageLocale">Language Locale</param>
        /// <returns>Connect In message BLOB</returns>
        public byte[] GetConnectInMessage(uint clientVersion, int isRemote,
            string userName, string machineName, string serverMachineName,
            string catalogName, string languageLocale)
        {
            var message = new CPMConnectIn();

            message._iClientVersion = clientVersion;

            message._fClientIsRemote = (UInt32)isRemote;

            message.MachineName = machineName;

            message.UserName = userName;

            message.cPropSets = 2;

            message.PropertySet1 = GetPropertySet1(catalogName);
            // PropertySet1 specifying MachineName
            message.PropertySet2 = GetPropertySet2(serverMachineName);

            message.cExtPropSet = 4;

            message.aPropertySets = new CDbPropSet[4];

            message.aPropertySets[0] = GetAPropertySet1(languageLocale); //Language locale
            message.aPropertySets[1] = GetAPropertySet2(); // FLAGs
            message.aPropertySets[2] = GetAPropertySet3(serverMachineName); // server
            message.aPropertySets[3] = GetAPropertySet4(catalogName); // Catalog

            var mainBlob = ToBytes(message);

            return AddMessageHeader(MessageType.CPMConnectIn, mainBlob);
            //Adding Message Header
        }



        /// <summary>
        /// Gets QueryStatusIn message BLOB for a given cursor
        /// </summary>
        /// <param name="cursor">Cursor of CPMQueryOut Message</param>
        /// <returns>QueryStatusIn Message BLOB</returns>
        public byte[] GetCPMQueryStatusIn(uint cursor)
        {
            // this message has just one field _cursor
            byte[] bytes = BitConverter.GetBytes(cursor);
            // Add Message Header
            return AddMessageHeader(MessageType.CPMGetQueryStatusIn, bytes);
        }

        /// <summary>
        /// Gets QueryStatusExIn Message BLOB for a given cursor and bookmark
        /// </summary>
        /// <param name="cursor">Cursor of CPMQueryOut Message</param>
        /// <param name="bookMarkHandle">Handle of the Bookmark</param>
        /// <returns>QueryStatusExIn BLOB</returns>
        public byte[] GetCPMQueryStatusExIn(uint cursor, uint bookMarkHandle)
        {
            int index = 0;
            byte[] bytes = new byte[Constant.SIZE_OF_UINT/*Cursor Handle */
                + Constant.SIZE_OF_UINT/* BookMark Handle */];
            Helper.CopyBytes(bytes, ref index,
                BitConverter.GetBytes(cursor));
            Helper.CopyBytes(bytes, ref index,
                BitConverter.GetBytes(bookMarkHandle));
            return AddMessageHeader(MessageType.CPMGetQueryStatusExIn, bytes);
        }

        /// <summary>
        /// Gets the CPMCiStateInOut Message BLOB
        /// </summary>
        /// <returns>CPMCiStateInOut BLOB</returns>
        public byte[] GetCPMCiStateInOut()
        {
            var message = new CPMCiStateInOut();

            message.cbStruct = 0x0000003C;

            var mainBlob = ToBytes(message);

            return AddMessageHeader(MessageType.CPMCiStateInOut, mainBlob);
        }

        /// <summary>
        /// Gets the CPMUpdateDocumentsIn message BOLB.
        /// </summary>
        /// <param name="flag">type of update</param>
        /// <param name="flagRootPath">Boolean value indicating
        /// if the RootPath field specifies a path on which 
        /// to perform the update.</param>
        /// <param name="rootPath">Name of the path to be updated</param>
        /// <returns>CPMUpdateDocumentsIn BLOB</returns>
        public byte[] GetCPMUpdateDocumentsIn(uint flag,
            uint flagRootPath, string rootPath)
        {
            int documentsInMessageLength = 2 * Constant.SIZE_OF_UINT
                + 2 * rootPath.Length/* unicode character */;
            byte[] mainBlob = new byte[documentsInMessageLength];
            //================ Converting values into Bytes ============
            int index = 0;

            Helper.CopyBytes(mainBlob, ref index,
                BitConverter.GetBytes(flag));
            Helper.CopyBytes(mainBlob, ref index,
                BitConverter.GetBytes(flagRootPath));
            Helper.CopyBytes(mainBlob, ref index,
                Encoding.Unicode.GetBytes(rootPath));
            return AddMessageHeader(MessageType.CPMUpdateDocumentsIn,
                mainBlob);
        }

        /// <summary>
        /// Gets the ForceMergeIn message BLOB
        /// </summary>
        /// <param name="partId"></param>
        /// <returns>CPMForceMergeIn BLOB</returns>
        public byte[] GetCPMForceMergeIn(uint partId)
        {
            byte[] mainBlob = new byte[Constant.SIZE_OF_UINT];
            //================ Converting value into Bytes ==============
            int index = 0;
            Helper.CopyBytes(mainBlob, ref index,
                BitConverter.GetBytes(partId));
            return AddMessageHeader(MessageType.CPMForceMergeIn, mainBlob);
        }

        /// <summary>
        /// Gets RatioFinishedIn Message BLOB
        /// </summary>
        /// <param name="cursor">Handle identifying the query for
        /// which to request completion information</param>
        /// <param name="quick">Unused field</param>
        /// <returns>CPMRatioFinishedIn BLOB</returns>
        public byte[] GetCPMRatioFinishedIn(uint cursor, uint quick)
        {
            int ratioFinishedInMessageLength = 2 * Constant.SIZE_OF_UINT;
            byte[] mainBlob = new byte[ratioFinishedInMessageLength];
            //================ Converting value into Bytes ==================
            int index = 0;
            Helper.CopyBytes(mainBlob, ref index,
                BitConverter.GetBytes(cursor));
            Helper.CopyBytes(mainBlob, ref index,
                BitConverter.GetBytes(quick));
            return AddMessageHeader(MessageType.CPMRatioFinishedIn, mainBlob);
        }

        /// <summary>
        /// Gets CPMFetchValueIn Message BLOB
        /// </summary>
        /// <param name="workID">Document ID</param>
        /// <param name="cbSoFar">Number of bytes 
        /// previously transferred</param>
        /// <param name="cbChunk">Maximum number of bytes that the sender 
        /// can accept in a CPMFetchValueOut message</param>
        /// <returns>CPMFetchValueIn BLOB</returns>
        public byte[] GetCPMFetchValueIn(uint workID,
            uint cbSoFar, uint cbChunk)
        {
            byte[] padding = null;
            int messageOffset = 0;
            messageOffset += 4 * Constant.SIZE_OF_UINT;
            TableColumn[] col = GetTableColumnFromConfig();
            byte[] propSpec = GetFullPropSec(parameter.PropertyGuidToFetch,
                PROPERTY_ID, parameter.PropertyIdToFetch, ref messageOffset);
            if (messageOffset % OFFSET_4 != 0)
            {
                padding = new byte[OFFSET_4 - messageOffset % OFFSET_4];
                for (int i = 0; i < padding.Length; i++)
                {
                    padding[i] = 0;
                }
                messageOffset += padding.Length;
            }
            byte[] mainBlob = new byte[messageOffset];
            uint cbPropSpec = (uint)propSpec.Length;
            //================ Converting values into Bytes ================
            int index = 0;
            Helper.CopyBytes(mainBlob, ref index,
                BitConverter.GetBytes(workID));
            Helper.CopyBytes(mainBlob, ref index,
                BitConverter.GetBytes(cbSoFar));
            Helper.CopyBytes(mainBlob, ref index,
                BitConverter.GetBytes(cbPropSpec));
            Helper.CopyBytes(mainBlob, ref index,
                BitConverter.GetBytes(cbChunk));
            Helper.CopyBytes(mainBlob, ref index, propSpec);
            if (padding != null)
            {
                //byte[] paddingValue = new byte[padding];
                Helper.CopyBytes(mainBlob, ref index, padding);
            }
            return AddMessageHeader(MessageType.CPMFetchValueIn, mainBlob);
        }

        /// <summary>
        /// Gets CPMCompareBmkIn Message Blob
        /// </summary>
        /// <param name="cursor">Handle from the 
        /// CPMCreateQueryOut message</param>
        /// <param name="chapt">Handle of the 
        /// chapter containing the bookmarks to compare</param>
        /// <param name="bmkFirst">Handle to the 
        /// first bookmark to compare</param>
        /// <param name="bmkSecond">Handle to the 
        /// second bookmark to compare</param>
        /// <returns>CPMCompareBmkIn BLOB</returns>
        public byte[] GetCPMCompareBmkIn(uint cursor,
            uint chapt, uint bmkFirst, uint bmkSecond)
        {
            int compareBmkInMessageLength = 4 * Constant.SIZE_OF_UINT;
            byte[] mainBlob = new byte[compareBmkInMessageLength];
            //================ Converting values into Bytes ====================
            int index = 0;
            Helper.CopyBytes(mainBlob, ref index,
                BitConverter.GetBytes(cursor));
            Helper.CopyBytes(mainBlob, ref index,
                BitConverter.GetBytes(chapt));
            Helper.CopyBytes(mainBlob, ref index,
                BitConverter.GetBytes(bmkFirst));
            Helper.CopyBytes(mainBlob, ref index,
                BitConverter.GetBytes(bmkSecond));
            return AddMessageHeader
                (MessageType.CPMCompareBmkIn, mainBlob);
        }

        /// <summary>
        /// Gets CPMRestartPositionIn Message BLOB
        /// </summary>
        /// <param name="cursor">Handle obtained from 
        /// a CPMCreateQueryOut message</param>
        /// <param name="chapt">Handle of a chapter from
        /// which to retrieve rows</param>
        /// <returns>CPMRestartPositionIn BLOB</returns>
        public byte[] GetRestartPositionIn(uint cursor, uint chapt)
        {
            int restartPositionInMessageLength = 2 * Constant.SIZE_OF_UINT;
            byte[] mainBlob = new byte[restartPositionInMessageLength];
            //================ Converting values into Bytes ====================
            int index = 0;
            Helper.CopyBytes(mainBlob, ref index, BitConverter.GetBytes(cursor));
            Helper.CopyBytes(mainBlob, ref index, BitConverter.GetBytes(chapt));
            return AddMessageHeader(MessageType.CPMRestartPositionIn, mainBlob);
        }

        //Update by v-aliche for delta testing
        ///// <summary>
        ///// Gets CPMStopAsyncIn Message Blob
        ///// </summary>
        ///// <param name="cursor">Handle from the
        ///// CPMCreateQueryOut message</param>
        ///// <returns>CPMStopAsyncIn BLOB</returns>
        //public byte[] GetStopAsyncIn(uint cursor)
        //{
        //    byte[] mainBlob = new byte[Constant.SIZE_OF_UINT];
        //    //================ Converting values into Bytes ===================
        //    int index = 0;
        //    Helper.CopyBytes(mainBlob, ref index,
        //        BitConverter.GetBytes(cursor));
        //    return AddMessageHeader
        //        (MessageType.CPMStopAsynchIn, mainBlob);
        //}

        /// <summary>
        /// Gets CPMFreeCursorIn Message Blob
        /// </summary>
        /// <param name="cursor">Handle from the 
        /// CPMCreateQueryOut message</param>
        /// <returns>CPMFreeCursorIn BLOB</returns>
        public byte[] GetFreeCursorIn(uint cursor)
        {
            byte[] mainBlob = new byte[Constant.SIZE_OF_UINT];
            //================ Converting values into Bytes ==============
            int index = 0;
            Helper.CopyBytes(mainBlob, ref index,
                BitConverter.GetBytes(cursor));
            return AddMessageHeader
                (MessageType.CPMFreeCursorIn, mainBlob);
        }

        /// <summary>
        /// Gets CPMGetApproximatePositionIn Message Blob
        /// </summary>
        /// <param name="cursor">Handle from 
        /// CPMCreateQueryOut message</param>
        /// <param name="chapt">Handle to the chapter
        /// containing the bookmark</param>
        /// <param name="bmk">Handle to the bookmark for
        /// which to retrieve the approximate position</param>
        /// <returns></returns>
        public byte[] GetApproximatePositionIn
            (uint cursor, uint chapt, uint bmk)
        {
            int approximatePositionInLength = 3 * Constant.SIZE_OF_UINT;
            byte[] mainBlob = new byte[approximatePositionInLength];
            //========== Converting values into Bytes ============
            int index = 0;
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(cursor));
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(chapt));
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(bmk));
            return AddMessageHeader
                (MessageType.CPMGetApproximatePositionIn, mainBlob);
        }

        /// <summary>
        /// Builds CPMDisconnect message
        /// </summary>
        /// <returns>CPMDisconnect BLOB</returns>
        public byte[] GetDisconnectMessage()
        {
            byte[] messageHeader = null;
            int index = 0;
            //Total message size
            uint messageValue = (uint)MessageType.CPMDisconnect;
            uint messageStatus = 0;
            uint checksum = 0;
            uint reserveField = 0;
            messageHeader = new byte[4 * Constant.SIZE_OF_UINT];
            Helper.CopyBytes
                (messageHeader, ref index, BitConverter.GetBytes(messageValue));
            Helper.CopyBytes
                (messageHeader, ref index, BitConverter.GetBytes(messageStatus));
            Helper.CopyBytes
                (messageHeader, ref index, BitConverter.GetBytes(checksum));
            Helper.CopyBytes
                (messageHeader, ref index, BitConverter.GetBytes(reserveField));

            return messageHeader;
        }

        /// <summary>
        /// Build PMFindIndicesIn message
        /// </summary>
        /// <param name="cWids"></param>
        /// <param name="cDepthPrev"></param>
        /// <returns>CPMFindIndices BLOB</returns>
        public byte[] GetCPMFindIndices(uint cWids, uint cDepthPrev)
        {
            int index = 0;
            int messageOffset = 4 * Constant.SIZE_OF_UINT;
            byte[] pwids = new byte[cWids];

            byte[] prgiRowPrev = new byte[cDepthPrev];

            byte[] mainBlob = new byte[messageOffset];
            Helper.CopyBytes(mainBlob, ref index,
                BitConverter.GetBytes(cWids));
            Helper.CopyBytes(mainBlob, ref index,
                BitConverter.GetBytes(cDepthPrev));
            Helper.CopyBytes(mainBlob, ref index, pwids);
            Helper.CopyBytes(mainBlob, ref index, prgiRowPrev);

            return AddMessageHeader(MessageType.CPMFindIndicesIn, mainBlob);
        }

        /// <summary>
        /// Build CPMGetRowsetNotifyIn message
        /// </summary>
        /// <returns>CPMGetRowsetNotify BLOB</returns>
        public byte[] GetCPMGetRowsetNotify()
        {
            byte[] mainBlob = new byte[Constant.SIZE_OF_UINT];

            return AddMessageHeader
                (MessageType.CPMGetRowsetNotifyIn, mainBlob);
        }

        /// <summary>
        /// Build CPMSetScopePrioritizationIn message
        /// </summary>
        /// <param name="priority"></param>
        /// <param name="eventFrequency"></param>
        /// <returns>CPMSetScopePrioritization BLOB</returns>
        public byte[] GetCPMSetScopePrioritization(uint priority, uint eventFrequency)
        {
            byte[] mainBlob = new byte[2 * Constant.SIZE_OF_UINT];

            int index = 0;
            Helper.CopyBytes(mainBlob, ref index,
                BitConverter.GetBytes(priority));
            Helper.CopyBytes(mainBlob, ref index,
                BitConverter.GetBytes(eventFrequency));
            return AddMessageHeader
                (MessageType.CPMSetScopePrioritizationIn, mainBlob);
        }

        /// <summary>
        /// Builds the CPMGetNotify request message
        /// </summary>
        /// <returns>CPMGetNotify BLOB</returns>
        public byte[] GetCPMGetNotify()
        {
            byte[] messageHeader = null;
            int index = 0;
            //Total message size
            uint messageValue = (uint)MessageType.CPMGetNotify;
            uint messageStatus = 0;
            uint checksum = 0;
            uint reserveField = 0;
            messageHeader = new byte[4 * Constant.SIZE_OF_UINT];
            Helper.CopyBytes
                (messageHeader, ref index, BitConverter.GetBytes(messageValue));
            Helper.CopyBytes
                (messageHeader, ref index, BitConverter.GetBytes(messageStatus));
            Helper.CopyBytes
                (messageHeader, ref index, BitConverter.GetBytes(checksum));
            Helper.CopyBytes
                (messageHeader, ref index, BitConverter.GetBytes(reserveField));

            return messageHeader;
        }
        #endregion

        #region Structures for CPMConnectIn message
        /// <summary>
        /// Gets the first PropertySet1 ConnectIn message
        /// </summary>
        /// <param name="messageOffset">offset from
        /// the starting of the message</param>
        /// <param name="catalogName">Name of the catalog</param>
        /// <returns>PropertySet BLOB</returns>
        private CDbPropSet GetPropertySet1(string catalogName)
        {
            var propSet = new CDbPropSet();

            uint cProperties = 4;

            propSet.guidPropertySet = parameter.PropertySet_One_Guid;

            propSet.cProperties = cProperties;

            propSet.aProps = new CDbProp[0];

            // --Get First PropSet with Guid Value DBPROPSET_FSCIFRMWRK_EXT -

            foreach (string property in parameter.PropertySet_One_DBProperties)
            {
                switch (property)
                {
                    case "2":
                        // Creating CDBProp with PropId 2 
                        // for GUID type FSCIFRMWRK
                        var value_FSCIFRMWRK_2 = GetBaseStorageVariant(vType_Values.VT_LPWSTR, false, new VT_LPWSTR(catalogName));
                        AppendDBProp(ref propSet, 2, value_FSCIFRMWRK_2);
                        break;
                    case "3":
                        // Creating CDBProp with PropId 3 
                        //for GUID type FCCIFRMWRK
                        var value_FCCIFRMWRK_3 = GetVector(vType_Values.VT_LPWSTR, new VT_LPWSTR[] { new VT_LPWSTR(null) });
                        AppendDBProp(ref propSet, 3, value_FCCIFRMWRK_3);
                        break;
                    case "4":
                        // Creating CDBProp with PropId 4 
                        // for GUID type FSCIFRMWRK
                        var value_FSCIFRMWRK_4 = GetVector(vType_Values.VT_I4, new Int32[] { 1 });
                        AppendDBProp(ref propSet, 4, value_FSCIFRMWRK_4);
                        break;

                    case "7":
                        // Creating CDBProp with PropId 7 
                        //for GUID type FSCIFRMWRK
                        var value_FSCIFRMWRK_7 = GetBaseStorageVariant(vType_Values.VT_I4, false, (Int32)0);
                        AppendDBProp(ref propSet, 7, value_FSCIFRMWRK_7);
                        break;
                    default:
                        break;
                }
            }

            return propSet;
        }
        /// <summary>
        /// Gets the first PropertySet2 ConnectIn message
        /// </summary>
        /// <param name="messageOffset">offset from the 
        /// starting of the message</param>
        /// <param name="machineName">Name of the 
        /// connecting client</param>
        /// <returns>PropertySet BLOB</returns>
        private CDbPropSet GetPropertySet2(string machineName)
        {
            var propSet = new CDbPropSet();

            uint cProperties = 1;

            propSet.guidPropertySet = parameter.PropertySet_Two_Guid;

            propSet.cProperties = cProperties;

            propSet.aProps = new CDbProp[0];

            foreach (string property in parameter.PropertySet_Two_DBProperties)
            {
                switch (property)
                {
                    case "2":
                        // Creating CDBProp with PropId 2 
                        //for GUID type CIFRMWRKCORE
                        var value_CIFRMWRKCORE_2 = GetBaseStorageVariant(vType_Values.VT_BSTR, false, new VT_BSTR(machineName));
                        AppendDBProp(ref propSet, 2, value_CIFRMWRKCORE_2);
                        break;
                    default:
                        break;
                }
            }

            return propSet;
        }
        /// <summary>
        /// PropertySet1 for extPropSets array
        /// </summary>
        /// <param name="messageOffset">messageOffset</param>
        /// <param name="languageLocale">language locale 
        /// of the client</param>
        /// <returns>PropertySet BLOB</returns>
        private CDbPropSet GetAPropertySet1(string languageLocale)
        {
            var propSet = new CDbPropSet();

            uint cProperties = 6;

            var guid = parameter.Array_PropertySet_One_Guid;

            propSet.cProperties = cProperties;

            propSet.guidPropertySet = guid;

            propSet.aProps = new CDbProp[0];

            // Compile aPropertySet1 with Guid Value DBPROPSET_MSIDXS_ROWSETEXT
            foreach (string property in parameter.Array_PropertySet_One_DBProperties)
            {
                switch (property)
                {
                    case "2":
                        // Creating CDBProp with PropId 2 
                        //for GUID type ROWSETEXT
                        var value_ROWSETEXT_2 = GetBaseStorageVariant(vType_Values.VT_I4, false, (Int32)0);
                        AppendDBProp(ref propSet, 2, value_ROWSETEXT_2);
                        break;
                    case "3":
                        // Creating CDBProp with PropId 3 
                        //for GUID type ROWSETEXT
                        var value_ROWSETEXT_3 = GetBaseStorageVariant(vType_Values.VT_BSTR, false, new VT_BSTR(languageLocale));
                        AppendDBProp(ref propSet, 3, value_ROWSETEXT_3);
                        break;
                    case "4":

                        // Creating CDBProp with PropId 4 
                        //for GUID type ROWSETEXT
                        var value_ROWSETEXT_4 = GetBaseStorageVariant(vType_Values.VT_BSTR, false, new VT_BSTR(""));
                        AppendDBProp(ref propSet, 4, value_ROWSETEXT_4);
                        break;

                    case "5":
                        // Creating CDBProp with PropId 5 
                        //for GUID type ROWSETEXT
                        var value_ROWSETEXT_5 = GetBaseStorageVariant(vType_Values.VT_BSTR, false, new VT_BSTR(""));
                        AppendDBProp(ref propSet, 5, value_ROWSETEXT_5);
                        break;

                    case "6":

                        // Creating CDBProp with PropId 6 
                        // for GUID type ROWSETEXT
                        var value_ROWSETEXT_6 = GetBaseStorageVariant(vType_Values.VT_I4, false, (Int32)0);
                        AppendDBProp(ref propSet, 6, value_ROWSETEXT_6);
                        break;

                    case "7":

                        // Creating CDBProp with PropId 7 
                        //for GUID type ROWSETEXT
                        var value_ROWSETEXT_7 = GetBaseStorageVariant(vType_Values.VT_I4, false, (Int32)0);
                        AppendDBProp(ref propSet, 7, value_ROWSETEXT_7);
                        break;
                    default:
                        break;
                }
            }
            return propSet;
        }
        /// <summary>
        /// PropertySet2 for extPropSets array
        /// </summary>
        /// <param name="messageOffset">messageOffset</param>
        /// <returns>PropertySet BLOB</returns>
        private CDbPropSet GetAPropertySet2()
        {
            var propSet = new CDbPropSet();

            uint cProperties = 10;
            var guid = parameter.Array_PropertySet_Two_Guid;

            propSet.cProperties = cProperties;
            propSet.guidPropertySet = guid;
            propSet.aProps = new CDbProp[0];

            //---- Compile aPropertySet2 with Guid Value DBPROPSET_QUERYEXT
            foreach (string property in parameter.Array_PropertySet_Two_DBProperties)
            {
                switch (property)
                {
                    case "2":
                        // Creating CDBProp with PropId 2
                        // for GUID type QUERYEXT
                        var value_QUERYEXT_2 = GetBaseStorageVariant(vType_Values.VT_BOOL, false, (UInt16)0x0000);
                        AppendDBProp(ref propSet, 2, value_QUERYEXT_2);
                        break;
                    case "3":
                        // Creating CDBProp with PropId 3 
                        // for GUID type QUERYEXT
                        var value_QUERYEXT_3 = GetBaseStorageVariant(vType_Values.VT_BOOL, false, (UInt16)0x0000);
                        AppendDBProp(ref propSet, 3, value_QUERYEXT_3);
                        break;
                    case "4":

                        // Creating CDBProp with PropId 4 
                        //for GUID type QUERYEXT
                        var value_QUERYEXT_4 = GetBaseStorageVariant(vType_Values.VT_BOOL, false, (UInt16)0x0000);
                        AppendDBProp(ref propSet, 4, value_QUERYEXT_4);
                        break;
                    case "5":
                        // Creating CDBProp with PropId 5 
                        //for GUID type QUERYEXT
                        var value_QUERYEXT_5 = GetBaseStorageVariant(vType_Values.VT_BOOL, false, (UInt16)0x0000);
                        AppendDBProp(ref propSet, 5, value_QUERYEXT_5);
                        break;
                    case "6":
                        // Creating CDBProp with PropId 6
                        //for GUID type QUERYEXT
                        var value_QUERYEXT_6 = GetBaseStorageVariant(vType_Values.VT_BSTR, false, new VT_BSTR(""));
                        AppendDBProp(ref propSet, 6, value_QUERYEXT_6);
                        break;
                    case "8":
                        // Creating CDBProp with PropId 8 
                        //for GUID type QUERYEXT
                        var value_QUERYEXT_8 = GetBaseStorageVariant(vType_Values.VT_BOOL, false, (UInt16)0x0000);
                        AppendDBProp(ref propSet, 8, value_QUERYEXT_8);
                        break;
                    case "10":

                        // Creating CDBProp with PropId 10 
                        //for GUID type QUERYEXT
                        var value_QUERYEXT_10 = GetBaseStorageVariant(vType_Values.VT_BOOL, false, (UInt16)0x0000);
                        AppendDBProp(ref propSet, 10, value_QUERYEXT_10);
                        break;
                    case "12":

                        // Creating CDBProp with PropId 12 
                        //for GUID type QUERYEXT
                        var value_QUERYEXT_12 = GetBaseStorageVariant(vType_Values.VT_BOOL, false, (UInt16)0x0000);
                        AppendDBProp(ref propSet, 12, value_QUERYEXT_12);
                        break;
                    case "13":
                        // Creating CDBProp with PropId 13 
                        //for GUID type QUERYEXT
                        var value_QUERYEXT_13 = GetBaseStorageVariant(vType_Values.VT_BOOL, false, (UInt16)0x0000);
                        AppendDBProp(ref propSet, 13, value_QUERYEXT_13);
                        break;

                    case "14":

                        // Creating CDBProp with PropId 14 
                        //for GUID type QUERYEXT
                        var value_QUERYEXT_14 = GetBaseStorageVariant(vType_Values.VT_BOOL, false, (UInt16)0x0000);
                        AppendDBProp(ref propSet, 14, value_QUERYEXT_14);
                        break;
                    default:
                        break;
                }
            }
            return propSet;
        }
        /// <summary>
        /// Gets PropertySet3 of external PropertySets array
        /// </summary>
        /// <param name="messageOffset">message Offset</param>
        /// <param name="serverName">Name of the Server to connect</param>
        /// <returns>BLOB</returns>
        private CDbPropSet GetAPropertySet3(string serverName)
        {
            var propSet = new CDbPropSet();

            uint cProperties = 1;
            var guid = parameter.Array_PropertySet_Three_Guid;

            propSet.cProperties = cProperties;
            propSet.guidPropertySet = guid;
            propSet.aProps = new CDbProp[0];


            foreach (string property in parameter.Array_PropertySet_Three_DBProperties)
            {
                switch (property)
                {
                    case "2":
                        // Creating CDBProp with PropId 2
                        // for GUID type FSCIFRMWRK_EXT
                        var value_FSCIFRMWRK_EXT_2 = GetBaseStorageVariant(vType_Values.VT_BSTR, false, new VT_BSTR(serverName));
                        AppendDBProp(ref propSet, 2, value_FSCIFRMWRK_EXT_2);
                        break;
                    default:
                        break;
                }
            }

            return propSet;
        }
        /// <summary>
        /// Gets PropertySet4 of external PropertySets array
        /// </summary>
        /// <param name="messageOffset">message Offset</param>
        /// <param name="catalogName">Name of the Catalog to connect</param>
        /// <returns>BLOB</returns>
        private CDbPropSet GetAPropertySet4(string catalogName)
        {
            var propSet = new CDbPropSet();

            uint cProperties = 3;



            var guid = parameter.Array_PropertySet_Four_Guid;

            propSet.cProperties = cProperties;
            propSet.guidPropertySet = guid;
            propSet.aProps = new CDbProp[0];


            //Compile aPropertySet4 with Guid Value DBPROPSET_CIFRMWRKCORE_EXT
            foreach (string property in parameter.Array_PropertySet_Four_DBProperties)
            {
                switch (property)
                {
                    case "2":
                        // Creating CDBProp with PropId 2 
                        //for GUID type CIFRMWRKCORE_EXT
                        var value_CIFRMWRKCORE_EXT_2 = GetBaseStorageVariant(vType_Values.VT_BSTR, false, new VT_BSTR(catalogName));
                        AppendDBProp(ref propSet, 2, value_CIFRMWRKCORE_EXT_2);
                        break;
                    case "3":
                        // Creating CDBProp with PropId 3 
                        //for GUID type CIFRMWRKCORE_EXT
                        var safeArrayForStr = GetSafeArray<VT_BSTR>(vType_Values.VT_BSTR, new VT_BSTR[] { new VT_BSTR("") }, 0);
                        AppendDBProp(ref propSet, 3, safeArrayForStr);
                        break;

                    case "4":
                        // Creating CDBProp with PropId 4
                        // for GUID type 
                        var safeArrayForInt = GetSafeArray<Int32>(vType_Values.VT_I4, new Int32[] { 0 }, 0);
                        AppendDBProp(ref propSet, 4, safeArrayForInt);
                        break;
                    default:
                        break;
                }
            }

            return propSet;
        }

        /// <summary>
        /// Adds DB PropertySet
        /// </summary>
        /// <param name="propertySet">PropertySet BLOB</param>
        /// <param name="id">DbPropId</param>
        /// <param name="columnValue">BLOB of the Value field</param>
        /// <param name="messageOffSet">messageOffSet</param>
        private void AppendDBProp(ref CDbPropSet propertySet, uint id, CBaseStorageVariant value)
        {
            uint dbPropId = id;
            uint dbPropOption = 0;
            uint dbPropStatus = 0;

            var prop = new CDbProp();

            prop.DBPROPID = dbPropId;

            prop.DBPROPOPTIONS = dbPropOption;

            prop.DBPROPSTATUS = dbPropStatus;

            prop.colid = GetColumnId();

            prop.vValue = value;

            propertySet.aProps = propertySet.aProps.Append(prop).ToArray();
        }

        /// <summary>
        /// Get ColumnId for PropertySet
        /// </summary>
        /// <param name="messageOffset">messageOffset</param>
        /// <returns>BLOB ColumnID</returns>
        private CDbColId GetColumnId()
        {
            var result = new CDbColId(parameter.EmptyGuid, 0);
            return result;
        }
        #endregion

        #region Structures of CPMgetRowsIn message

        /// <summary>
        /// Gets FullPropertySpec for a given GUID, eKIND and propSec Id
        /// </summary>
        /// <param name="guid">PropSpec GUID</param>
        /// <param name="kind">EKind</param>
        /// <param name="propSec">PropSec Id</param>
        /// <param name="messageOffset">offset from the 
        /// beginning of the message</param>
        /// <returns>full property spec structure BLOB</returns>
        private byte[] GetFullPropSec
            (Guid guid, int kind, int propSec, ref int messageOffset)
        {
            int startingIndex = messageOffset;
            int index = 0;
            byte[] padding = null;
            if (messageOffset % OFFSET_8 != 0)
            {
                padding = new byte[OFFSET_8 - messageOffset % OFFSET_8];
                for (int i = 0; i < padding.Length; i++)
                {
                    padding[i] = 0;
                }
                messageOffset += padding.Length;
            }
            messageOffset += Constant.SIZE_OF_GUID;
            uint ulKind = (uint)kind;
            uint pRpSec = (uint)propSec;
            messageOffset += 2 * Constant.SIZE_OF_UINT;
            byte[] mainBlob = new byte[messageOffset - startingIndex];
            if (padding != null)
            {
                Helper.CopyBytes(mainBlob, ref index, padding);
            }
            Helper.CopyBytes
                (mainBlob, ref index, guid.ToByteArray());
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(ulKind));
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(pRpSec));
            return mainBlob;
        }
        /// <summary>
        /// Gets the Seek Desciption type for the RowsIn message
        /// </summary>
        /// <param name="eType">eType</param>
        /// <returns>returns byte[] form of SeekDescription variable</returns>
        private byte[] GetSeekDescription(uint eType)
        {
            byte[] returnBlob = null;
            uint skip = 0;
            uint bmkOffset = 0;
            uint region = 0;
            uint numerator = 0;
            uint denominator = 5;
            int index = 0;
            switch (eType)
            {
                case (uint)RowSeekType.eRowSeekNext:
                    skip = 0x00000000;
                    returnBlob = new byte[Constant.SIZE_OF_UINT];
                    Helper.CopyBytes
                        (returnBlob, ref index,
                        BitConverter.GetBytes(skip));
                    break;
                case (uint)RowSeekType.eRowSeekAt:
                    bmkOffset = 2;
                    skip = 2;
                    region = 0;
                    returnBlob = new byte[3 * Constant.SIZE_OF_UINT];
                    Helper.CopyBytes
                        (returnBlob, ref index,
                        BitConverter.GetBytes(bmkOffset));
                    Helper.CopyBytes
                        (returnBlob, ref index,
                        BitConverter.GetBytes(skip));
                    Helper.CopyBytes
                        (returnBlob, ref index,
                        BitConverter.GetBytes(region));
                    break;
                case (uint)RowSeekType.eRowSeekAtRatio:
                    numerator = 1;
                    denominator = 2; // Must be greater than ZER0
                    region = 0;
                    returnBlob = new byte[3 * Constant.SIZE_OF_UINT];
                    Helper.CopyBytes
                        (returnBlob, ref index,
                        BitConverter.GetBytes(numerator));
                    Helper.CopyBytes
                        (returnBlob, ref index,
                        BitConverter.GetBytes(denominator));
                    Helper.CopyBytes
                        (returnBlob, ref index,
                        BitConverter.GetBytes(region));
                    break;
                default:
                    break;
            }
            return returnBlob;
        }

        /// <summary>
        /// Gets CRestrictionArray structure
        /// </summary>
        /// <param name="messageOffset">Offset from the 
        /// beginning of the message</param>
        /// <param name="queryString">Search Query String</param>
        /// <param name="searchScope">Search Query Scope</param>
        /// <returns>CRestrictionArray structure BLOB</returns>
        private byte[] GetRestrictionArray
            (ref int messageOffset, string queryString, string searchScope)
        {
            int startIndex = messageOffset;
            int index = 0;
            byte[] padding = null;
            byte count = 1; //to be assigned later
            byte isRestrictionPresent = 0x01; // means Restriction is present
            messageOffset += 2 * Constant.SIZE_OF_BYTE;
            if (messageOffset % OFFSET_4 != 0)
            {
                padding = new byte[OFFSET_4 - messageOffset % OFFSET_4];
                for (int i = 0; i < padding.Length; i++)
                {
                    padding[i] = 0;
                }
                messageOffset += padding.Length;
            }
            byte[] totalRestriction = GetQueryPathRestriction
                (ref messageOffset, queryString, searchScope);
            byte[] mainBlob = new byte[messageOffset - startIndex];

            Helper.CopyBytes(mainBlob, ref index, new byte[] { count });
            Helper.CopyBytes(mainBlob, ref index, new byte[]
            { isRestrictionPresent });
            Helper.CopyBytes(mainBlob, ref index, padding);
            Helper.CopyBytes(mainBlob, ref index, totalRestriction);
            return mainBlob;

        }

        /// <summary>
        /// Gets Node restriction specific to the query 
        /// scope and the queryText
        /// </summary>
        /// <param name="messageOffset">Offset from the
        /// beginning from the message</param>
        /// <param name="queryString"></param>
        /// <param name="searchScope"></param>
        /// <returns>CPropertyRestrictionNode structure BLOB</returns>
        private byte[] GetQueryPathRestriction
            (ref int messageOffset, string queryString, string searchScope)
        {
            int startIndex = messageOffset;
            int index = 0;
            uint restrictionType = LOGICAL_AND; // AND type. == 1
            uint weight = NODE_WEIGHTAGE;// Weightage of restriction. 1000
            messageOffset += 2 * Constant.SIZE_OF_UINT;
            //------ Logic for first Node of Restriction -----
            //------ This refers to the query path ----------
            uint node = 0x00000002; // two nodes are AND-ed together
            //------ Get first node ----
            uint firstNodeType = PROPERTY_RESTRICTION_NODE_ID; // Property 5
            uint firstNodeWeight = NODE_WEIGHTAGE;// weightage
            // node + firstNodeType + firstNodeWeight
            messageOffset += 3 * Constant.SIZE_OF_UINT;
            byte[] propertyRestriction
                = GetPropertyRestriction(ref messageOffset, searchScope);

            //------ Logic for second Node of Restriction -----
            //------ This refers to the search string path ----

            uint secondNodeType = CONTENT_RESTRICTION_NODE_ID; // Content 4
            uint secondNodeWeight = NODE_WEIGHTAGE;// weightage
            messageOffset += 2 * Constant.SIZE_OF_UINT;
            //Get Content Restriction
            byte[] contentRestriction
                = GetContentRestriction(ref messageOffset, queryString);

            byte[] blob = new byte[messageOffset - startIndex];
            Helper.CopyBytes
                (blob, ref index, BitConverter.GetBytes(restrictionType));
            Helper.CopyBytes
                (blob, ref index, BitConverter.GetBytes(weight));
            Helper.CopyBytes
                (blob, ref index, BitConverter.GetBytes(node));
            Helper.CopyBytes
                (blob, ref index, BitConverter.GetBytes(firstNodeType));
            Helper.CopyBytes
                (blob, ref index, BitConverter.GetBytes(firstNodeWeight));
            Helper.CopyBytes
                (blob, ref index, propertyRestriction);
            Helper.CopyBytes
                (blob, ref index, BitConverter.GetBytes(secondNodeType));
            Helper.CopyBytes
                (blob, ref index, BitConverter.GetBytes(secondNodeWeight));
            Helper.CopyBytes
                (blob, ref index, contentRestriction);
            return blob;

        }

        /// <summary>
        /// Gets CBaseStorgeVariant structure of a given type/value
        /// </summary>
        /// <param name="type">Type of the storage value</param>
        /// <param name="isArray">true if it is an array of items</param>
        /// <param name="inputValue">input value, if isArray
        /// is true, pass values as array of objects</param>
        /// <returns>CBaseStorageVariant BLOB</returns>
        private byte[] GetBaseStorageVariant
            (StorageType type, bool isArray, object inputValue)
        {
            ushort vType = (ushort)type;
            byte vData1 = 0;
            byte vData2 = 0;
            byte[] value = new byte[1];
            if (!isArray)
            {
                switch (type)
                {
                    case StorageType.VT_BOOL:
                        bool tempBool = (bool)inputValue;
                        value = new byte[] { 0x00, 0x00 };
                        if (tempBool)
                        {
                            value = new byte[] { 0xFF, 0xFF };
                        }
                        break;
                    case StorageType.VT_I4:
                        int tempI4 = (int)inputValue;
                        value = BitConverter.GetBytes(tempI4);
                        break;
                    case StorageType.VT_UI4:
                        int tempVal = (int)inputValue;
                        value = BitConverter.GetBytes((uint)tempVal);
                        break;
                    //VT_BSTR and VT_LPWSTR is same
                    case StorageType.VT_BSTR:
                        int bStrIndex = 0;
                        //=====  Building Unicode String Type ==============
                        string bStr = (string)inputValue;
                        uint bStrLength = (uint)(2 * bStr.Length);
                        value = new byte[Constant.SIZE_OF_UINT + bStrLength];
                        Helper.CopyBytes
                            (value, ref bStrIndex,
                            BitConverter.GetBytes(bStrLength));
                        Helper.CopyBytes
                            (value, ref bStrIndex,
                            Encoding.Unicode.GetBytes(bStr));

                        //==================================================
                        break;
                    case StorageType.VT_LPWSTR:
                        int strIndex = 0;
                        //=====  Building Unicode String Type ==============
                        string str = (string)inputValue;
                        uint unicodeStringLength = (uint)str.Length;
                        value
                            = new byte[Constant.SIZE_OF_UINT
                                + 2 * unicodeStringLength];
                        Helper.CopyBytes
                            (value, ref strIndex,
                            BitConverter.GetBytes(unicodeStringLength));
                        Helper.CopyBytes
                            (value, ref strIndex,
                            Encoding.Unicode.GetBytes(str));
                        //==================================================
                        break;
                    case StorageType.VT_COMPRESSED_LPWSTR:
                        break;
                    default:
                        break;
                }
                byte[] storageVariantBytes
                    = new byte[sizeof(ushort) +
                        2 * sizeof(byte) + value.Length];
                int index = 0;
                Helper.CopyBytes
                    (storageVariantBytes, ref index,
                    BitConverter.GetBytes(vType));
                Helper.CopyBytes
                    (storageVariantBytes, ref index,
                    new byte[] { vData1 });
                Helper.CopyBytes
                    (storageVariantBytes, ref index,
                    new byte[] { vData2 });
                Helper.CopyBytes
                    (storageVariantBytes, ref index, value);
                return storageVariantBytes;
            }
            return null;
        }

        /// <summary>
        /// Gets CPropertyRestriction specific to the query path
        /// </summary>
        /// <param name="messageOffset">offset from 
        /// the beginning of the message</param>
        /// <param name="searchScope">Scope of the current search</param>
        /// <returns></returns>
        private byte[] GetPropertyRestriction
            (ref int messageOffset, string searchScope)
        {
            int startingIndex = messageOffset;
            int index = 0;
            byte[] lcidPadding = null; // padding for LCID field
            uint relop = RELATION_OPERATION; // Relation operation = 4 
            messageOffset += Constant.SIZE_OF_UINT;
            // Get FullPropSec for Prop Id 20
            byte[] fullPropSec
                = GetFullPropSec
                (parameter.PropertyRestrictionGuid, PROPERTY_ID, parameter.PropertyRestrictionProperty, ref messageOffset);
            byte[] propValue
            = GetBaseStorageVariant
            (StorageType.VT_LPWSTR, false, searchScope);
            messageOffset += propValue.Length;
            if (messageOffset % OFFSET_4 != 0)
            {
                lcidPadding = new byte[OFFSET_4 - messageOffset % OFFSET_4];
                for (int i = 0; i < lcidPadding.Length; i++)
                {
                    lcidPadding[i] = 0;
                }
                messageOffset += lcidPadding.Length;
            }
            uint lcid = 0x0000409;
            messageOffset += Constant.SIZE_OF_UINT;

            byte[] mainBlob = new byte[messageOffset - startingIndex];
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(relop));
            Helper.CopyBytes
                (mainBlob, ref index, fullPropSec);
            Helper.CopyBytes
                (mainBlob, ref index, propValue);
            if (lcidPadding != null)
            {
                Helper.CopyBytes(mainBlob, ref index, lcidPadding);
            }
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(lcid));
            return mainBlob;
        }

        /// <summary>
        /// Get rowset event structure
        /// </summary>
        /// <param name="ENABLEROWSETEVENTS"></param>
        /// <param name="messageOffset"></param>
        /// <returns></returns>
        private byte[] GetRowSetProperties(bool ENABLEROWSETEVENTS, ref int messageOffset)
        {
            int startIndex = messageOffset;
            int index = 0;
            uint booleanOptions;
            if (ENABLEROWSETEVENTS)
            {
                booleanOptions = 0x00800000;
                Constant.DBPROP_ENABLEROWSETEVENTS = true;
            }
            else
            {
                booleanOptions = FIELD_USED;
                Constant.DBPROP_ENABLEROWSETEVENTS = false;
            }
            uint maxOpenRows = 0x00000000;
            uint memoryUsage = 0x00000000;
            uint maxResults = 0x00000000;
            uint cmdTimeOut = COMMAND_TIME_OUT;
            //CRowsetProperties contain 5 field(contain head), and all are 4 bytes,
            //so the messageSize should be 5 * Constant.SIZE_OF_UINT
            int messageSize = 5;
            messageOffset += messageSize * Constant.SIZE_OF_UINT;
            // Copy all the fields in a BLOB
            byte[] mainBlob = new byte[messageOffset - startIndex];
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(booleanOptions));
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(maxOpenRows));
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(memoryUsage));
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(maxResults));
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(cmdTimeOut));
            return mainBlob;
        }

        /// <summary>
        /// Get RowSet Properties structure
        /// </summary>
        /// <param name="messageOffset">Offset from the
        /// beginning of the message</param>
        /// <returns>RowSet structure BLOB</returns>
        private byte[] GetRowSetProperties(ref int messageOffset)
        {
            int startIndex = messageOffset;
            int index = 0;

            //Updated by:v-zhil
            //For Delta Testing
            //uint booleanOptions = FIELD_USED;
            uint booleanOptions = 0x00800000;
            uint maxOpenRows = 0x00000000;
            uint memoryUsage = 0x00000000;
            uint maxResults = 0x00000000;
            uint cmdTimeOut = COMMAND_TIME_OUT;

            messageOffset += 5 * Constant.SIZE_OF_UINT;
            // Copy all the fields in a BLOB
            byte[] mainBlob = new byte[messageOffset - startIndex];
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(booleanOptions));
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(maxOpenRows));
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(memoryUsage));
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(maxResults));
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(cmdTimeOut));
            return mainBlob;
        }

        /// <summary>
        /// Gets the ContentRestrictionNode structure specific 
        /// to the query Text
        /// </summary>
        /// <param name="messageOffset">offset from the 
        /// begining of the message</param>
        /// <param name="queryString">Query String of the search</param>
        /// <returns>ContentRestriction structure Node</returns>
        private byte[] GetContentRestriction
            (ref int messageOffset, string queryString)
        {
            int startingIndex = messageOffset;
            int index = 0;
            byte[] padding1 = null;
            byte[] padding2 = null;
            byte[] fullPropSec
                = GetFullPropSec
                (parameter.ContentRestrictionGuid, PROPERTY_ID, parameter.ContentRestrictionProperty, ref messageOffset);

            if (messageOffset % OFFSET_4 != 0)
            {
                padding1 = new byte[OFFSET_4 - messageOffset % OFFSET_4];
                for (int i = 0; i < padding1.Length; i++)
                {
                    padding1[i] = 0;
                }
                messageOffset += padding1.Length;
            }
            // Length of the Query string
            uint cc = (uint)queryString.Length;
            byte[] phrase = Encoding.Unicode.GetBytes(queryString);
            messageOffset += Constant.SIZE_OF_UINT + phrase.Length;

            // Second Padding
            if (messageOffset % OFFSET_4 != 0)
            {
                padding2 = new byte[OFFSET_4 - messageOffset % OFFSET_4];
                for (int i = 0; i < padding2.Length; i++)
                {
                    padding2[i] = 0;
                }
                messageOffset += padding2.Length;
            }
            uint lcid = 0x409; //en-US
            uint generatedMethod = 0x00000000;// means exact match
            messageOffset += 2 * Constant.SIZE_OF_UINT;
            byte[] mainBlob = new byte[messageOffset - startingIndex];
            Helper.CopyBytes(mainBlob, ref index, fullPropSec);
            if (padding1 != null)
            {
                Helper.CopyBytes(mainBlob, ref index, padding1);
            }
            Helper.CopyBytes(mainBlob, ref index, BitConverter.GetBytes(cc));
            Helper.CopyBytes(mainBlob, ref index, phrase);

            if (padding2 != null)
            {
                Helper.CopyBytes(mainBlob, ref index, padding2);
            }
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(lcid));
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(generatedMethod));
            return mainBlob;

        }
        #endregion

        #region Helper methods for Base Storage Type
        /// <summary>
        /// Gets a Vector form of a Base Storage Type
        /// </summary>
        /// <param name="type">StorageType (Vector Item type)</param>
        /// <param name="inputvalues">StorageType
        /// (Vector Item values)</param>
        /// <returns>Vector Base Storage Type BLOB</returns>
        private CBaseStorageVariant GetVector<T>(vType_Values type, T[] inputvalues) where T : struct
        {
            var result = new CBaseStorageVariant();
            result.vType = type | vType_Values.VT_VECTOR;
            result.vData1 = 0;
            result.vData2 = 0;
            switch (type)
            {
                case vType_Values.VT_I4:
                    {
                        var vector = new VT_VECTOR<Int32>();
                        vector.vVectorElements = (UInt32)inputvalues.Length;
                        vector.vVectorData = inputvalues.Cast<Int32>().ToArray();
                        result.vValue = vector;
                    }
                    break;
                case vType_Values.VT_LPWSTR:
                    {
                        var vector = new VT_VECTOR<VT_LPWSTR>();
                        vector.vVectorElements = (UInt32)inputvalues.Length;
                        vector.vVectorData = inputvalues.Cast<VT_LPWSTR>().ToArray();
                        result.vValue = vector;
                    }
                    break;

                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// Gets Safe Array of given StorageType
        /// </summary>
        /// <param name="type">Type of item(s)</param>
        /// <param name="arrayValue">Value of item(s)</param>
        /// <param name="features">Safe Array Features</param>
        /// <returns>Safe Array Storage Type</returns>
        private CBaseStorageVariant GetSafeArray<T>(vType_Values type, T[] arrayValue, ushort features) where T : struct
        {
            var result = new CBaseStorageVariant();
            result.vType = type | vType_Values.VT_ARRAY;
            result.vData1 = 0;
            result.vData2 = 0;

            var array = new SAFEARRAY<T>();
            array.cDims = 1;
            array.fFeatures = features;

            var tempBuffer = new WSPBuffer();
            if (arrayValue[0] is IWSPObject)
            {

                (arrayValue[0] as IWSPObject).ToBytes(tempBuffer);

            }
            else
            {
                tempBuffer.Add(arrayValue[0]);
            }

            array.cbElements = (UInt32)tempBuffer.Offset;

            array.Rgsabound = new SAFEARRAYBOUND[] { new SAFEARRAYBOUND() { cElements = (UInt32)arrayValue.Length, lLbound = 0 } };
            array.vData = arrayValue;

            result.vValue = array;

            return result;
        }

        /// <summary>
        /// Gets the bound for a Safe Array Type
        /// </summary>
        /// <param name="numberOfElements">Number of 
        /// items in safe array</param>
        /// <param name="lowerLimit">lower index of the safe array</param>
        /// <returns>Bound BLOB</returns>
        private byte[] GetBound(uint numberOfElements, uint lowerLimit)
        {
            byte[] value = new byte[2 * Constant.SIZE_OF_UINT];
            int index = 0;
            Helper.CopyBytes
                (value, ref index, BitConverter.GetBytes(numberOfElements));
            Helper.CopyBytes
                (value, ref index, BitConverter.GetBytes(lowerLimit));
            return value;
        }

        /// <summary>
        /// Gets CBaseStorgeVariant structure of a given type/value
        /// </summary>
        /// <param name="type">Type of the storage value</param>
        /// <param name="isArray">true if it is an array of items</param>
        /// <param name="inputValue">input value, if isArray
        /// is true, pass values as array of objects</param>
        /// <returns>CBaseStorageVariant BLOB</returns>
        private CBaseStorageVariant GetBaseStorageVariant(vType_Values type, bool isArray, object inputValue)
        {
            var result = new CBaseStorageVariant();
            ushort vType = (ushort)type;
            byte vData1 = 0;
            byte vData2 = 0;
            if (!isArray)
            {
                result.vType = (vType_Values)vType;

                result.vData1 = vData1;

                result.vData2 = vData2;

                result.vValue = inputValue;

                return result;
            }
            else
            {
                throw new InvalidOperationException("Array is not supported yet!");
            }
        }
        #endregion

        #region Query Trio Messages (QueryIn, SetBindingsIn and RowsIn)

        /// <summary>
        /// Gets the CPMSetBindingsIn message
        /// </summary>
        /// <param name="queryCursor">Query associated Cursor</param>
        /// <param name="columns">Array of TableColumns to be Queried</param>
        /// <param name="isValidBinding">True if the binding is valid</param>
        /// <returns></returns>
        public byte[] GetCPMSetBindingsIn
            (uint queryCursor, out TableColumn[] columns, bool isValidBinding)
        {
            int index = 0;
            uint cursor = queryCursor;
            uint rows = (uint)parameter.EachRowSize;
            uint bindingDesc = 0;
            // SIZE of ColumnCount and Columns combined to be assigned later.
            uint dummy = 0;// Dummy value
            uint columnCount = 0;
            int messageOffset = 5 * Constant.SIZE_OF_UINT;
            int bookMark = messageOffset;
            Random r = new Random();
            columns = GetTableColumnFromConfig();
            TableColumn[] newTableCol = new TableColumn[1];
            if (!isValidBinding)
            {
                // decreasing the number of bytes to fail the Bindings
                rows -= (uint)r.Next((int)rows - 10, (int)rows);

            }
            byte[][] paddingColumn = new byte[columns.Length][];
            byte[][] tableColumn = new byte[columns.Length][];

            for (int i = 0; i < columns.Length; i++)
            {
                tableColumn[i]
                    = GetTableColumn(columns[i], ref messageOffset);
                if (messageOffset % OFFSET_4 != 0)
                {
                    paddingColumn[i]
                        = new byte[OFFSET_4 - messageOffset % 4];
                    for (int j = 0; j < paddingColumn[i].Length; j++)
                    {
                        paddingColumn[i][j] = 0;
                    }
                    messageOffset += paddingColumn[i].Length;
                }
            }
            columnCount = (uint)tableColumn.Length;
            //==========  Build the message BLOB ===========
            byte[] blob = new byte[messageOffset];
            //Add Cursor
            Helper.CopyBytes
                (blob, ref index, BitConverter.GetBytes(cursor));
            // Add Rows fields
            Helper.CopyBytes
                (blob, ref index, BitConverter.GetBytes(rows));

            // update the bindingDesc value before inserting in the BLOB
            bindingDesc
                = (uint)(messageOffset - 4 * Constant.SIZE_OF_UINT);
            if (!isValidBinding)
            {
                bindingDesc -= 50;
            }
            Helper.CopyBytes
                (blob, ref index, BitConverter.GetBytes(bindingDesc));
            Helper.CopyBytes
                (blob, ref index, BitConverter.GetBytes(dummy));
            //Add Column count
            Helper.CopyBytes
                (blob, ref index, BitConverter.GetBytes(columnCount));
            if (tableColumn.Length > 0)
            {
                Helper.CopyBytes(blob, ref index, tableColumn[0]);
            }
            for (int i = 1; i < tableColumn.Length; i++)
            {
                if (paddingColumn[i - 1] != null)
                    Helper.CopyBytes(blob, ref index, paddingColumn[i - 1]);
                Helper.CopyBytes(blob, ref index, tableColumn[i]);
            }

            // Add message Header with MessageType CPMSetBindingsIn
            return AddMessageHeader(MessageType.CPMSetBindingsIn, blob);
        }

        /// <summary>
        /// Reads the table Columns details from the configuration file
        /// </summary>
        /// <returns>array of Table Column</returns>
        private TableColumn[] GetTableColumnFromConfig()
        {
            int numberofTableColumns = parameter.NumberOfSetBindingsColumns;
            TableColumn[] columns = new TableColumn[numberofTableColumns];
            for (int i = 0; i < numberofTableColumns; i++)
            {
                columns[i] = new TableColumn();
                columns[i].Guid = parameter.ColumnParameters[i].Guid;
                columns[i].PropertyId = parameter.ColumnParameters[i].PropertyId;
                columns[i].ValueOffset = parameter.ColumnParameters[i].ValueOffset;
                columns[i].StatusOffset = parameter.ColumnParameters[i].StatusOffset;
                columns[i].LengthOffset = parameter.ColumnParameters[i].LengthOffset;
                columns[i].Type = parameter.ColumnParameters[i].StorageType;
            }
            return columns;
        }
        #region Get Table Column for CPMSetBindingsIn message



        #endregion

        /// <summary>
        /// Gets ColumnSet structure
        /// </summary>
        /// <param name="messageOffset">offset from the
        /// beginning of the message</param>
        /// <returns>ColumnSet structure BLOB</returns>
        private byte[] GetColumnSet(ref int messageOffset)
        {
            int startIndex = messageOffset;
            int index = 0;
            int innerIndex = 0;
            // Index of Properties to be queried
            uint[] indexes = new uint[] { 0};
            // Links to the 'pidMapper' field
            messageOffset += (1 + indexes.Length) * Constant.SIZE_OF_UINT;
            uint count = (uint)indexes.Length;

            byte[] indexesBytes
                = new byte[indexes.Length * Constant.SIZE_OF_UINT];
            for (int i = 0; i < indexes.Length; i++)
            {
                Helper.CopyBytes
                    (indexesBytes, ref innerIndex,
                    BitConverter.GetBytes(indexes[i]));
            }

            byte[] mainBlob = new byte[messageOffset - startIndex];
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(count));
            Helper.CopyBytes
                (mainBlob, ref index, indexesBytes);

            return mainBlob;
        }
        /// <summary>
        /// Gets the PIDMapper Structure
        /// </summary>
        /// <param name="messageOffset">Offset from 
        /// the beginning of the message</param>
        /// <returns>Pid Mapper structure BLOB</returns>
        private byte[] GetPidMapper(ref int messageOffset)
        {
            int startIndex = messageOffset;
            int index = 0;
            uint count = (uint)parameter.NumberOfCreateQueryColumns;
            messageOffset += Constant.SIZE_OF_UINT;
            byte[][] propSecs = new byte[count][];

            for (int i = 0; i < propSecs.Length; i++)
            {
                propSecs[i]
                    = GetFullPropSec(parameter.CreateQueryColumnParameters[i].Guid,
                    PROPERTY_ID,
                    (int)parameter.CreateQueryColumnParameters[i].PropertyId,
                    ref messageOffset);
            }
            uint columnGroupArray = 0x00000000;

            messageOffset += 2 * Constant.SIZE_OF_UINT;
            // Copy the content in a BLOB
            byte[] blob = new byte[messageOffset - startIndex];
            Helper.CopyBytes(blob, ref index, BitConverter.GetBytes(count));
            for (int i = 0; i < propSecs.Length; i++)
            {
                Helper.CopyBytes(blob, ref index, propSecs[i]);
            }
            Helper.CopyBytes
                (blob, ref index, BitConverter.GetBytes(columnGroupArray));
            Helper.CopyBytes
                (blob, ref index, BitConverter.GetBytes(parameter.LCID_VALUE));
            return blob;
        }

        public byte[] GetCPMRowsInMessage(uint cursor, uint rowsToTransfer, uint rowWidth, uint cbReadBuffer, uint fBwdFetch, uint eType, out uint reserved)
        {
            int index = 0;

            uint sizeOfSeek = 0; // To be assigned with the size of seek.
            reserved = 0; // To be assigned later.


            // In RowsOut Message the columnoffset with be clientBase + X

            int messageOffset = 10 * Constant.SIZE_OF_UINT;

            //Assing variable values
            byte[] seekDescription = GetSeekDescription(eType);
            int seekDescriptionLength = seekDescription == null ? 0 : seekDescription.Length;
            sizeOfSeek
                = (uint)(2 * Constant.SIZE_OF_UINT + seekDescriptionLength);
            reserved = (uint)(28 + seekDescriptionLength);

            messageOffset += seekDescriptionLength;
            byte[] mainBlob = new byte[messageOffset];
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(cursor));
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(rowsToTransfer));
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(rowWidth));
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(sizeOfSeek));
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(reserved));
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(cbReadBuffer));
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(0));
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(fBwdFetch));
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(eType));
            Helper.CopyBytes
                (mainBlob, ref index, BitConverter.GetBytes(0)); // chapt

            if (seekDescription != null)
            {
                Helper.CopyBytes
                    (mainBlob, ref index, seekDescription);
            }
            return AddMessageHeader(MessageType.CPMGetRowsIn, mainBlob);
        }

        /// <summary>
        /// Gets TableColumn structure from given values
        /// </summary>
        /// <param name="column">TableColumn information</param>
        /// <param name="messageOffset">Offset from
        /// the begining of the message</param>
        /// <returns></returns>
        private byte[] GetTableColumn
            (TableColumn column, ref int messageOffset)
        {
            int startingIndex = messageOffset;
            byte[] padding = null;
            byte[] padding2 = null;
            byte[] padding3 = null;
            int index = 0;
            byte[] propSpec
                = GetFullPropSec
                (column.Guid, PROPERTY_ID,
                (int)column.PropertyId, ref messageOffset);

            uint vType = (uint)column.Type; // VT_VARIANT
            byte aggregateStored = FIELD_USED;
            byte aggregateType = (byte)AggregateType.DBAGGTTYPE_BYNONE;
            byte valueUsed = FIELD_NOT_USED;
            byte lengthUsed = FIELD_NOT_USED;
            byte statusUsed = FIELD_NOT_USED; // Using status

            ushort valueOffset = 0;//Optional field
            ushort valueSize = 0; //Optional field
            ushort statusOffset = 0;//optional field
            ushort lengthOffset = 0;//optional field

            if (column.ValueOffset > 0)
            {
                valueUsed = FIELD_USED;
                valueSize = GetSize(column.Type);
                valueOffset = column.ValueOffset;
            }

            if (column.LengthOffset > 0)
            {
                lengthUsed = FIELD_USED;
                lengthOffset = column.LengthOffset;
            }
            if (column.StatusOffset > 0)
            {
                statusUsed = FIELD_USED;
                statusOffset = column.StatusOffset;
            }

            messageOffset
                += Constant.SIZE_OF_UINT + 3 * Constant.SIZE_OF_BYTE;
            if (valueUsed == FIELD_USED)
            {
                if (messageOffset % 2 != 0)
                {
                    padding = new byte[1];
                    padding[0] = 0x00;
                    messageOffset += Constant.SIZE_OF_BYTE;
                }

                messageOffset += 2 * Constant.SIZE_OF_USHORT;
                // value offset and size
            }

            messageOffset += Constant.SIZE_OF_BYTE;
            if (statusUsed == FIELD_USED)
            {
                if (messageOffset % 2 != 0)
                {
                    padding2 = new byte[1];
                    padding2[0] = 0x00;
                    messageOffset += padding2.Length;
                }
                messageOffset += Constant.SIZE_OF_USHORT;
                // size of status offset
            }

            messageOffset += Constant.SIZE_OF_BYTE;
            if (lengthUsed == FIELD_USED)
            {
                if (messageOffset % 2 != 0)
                {
                    padding3 = new byte[1];
                    padding3[0] = 0x00;
                    messageOffset += padding3.Length;
                }
                messageOffset += Constant.SIZE_OF_USHORT;
                //size of length offset
            }
            byte[] blob = new byte[messageOffset - startingIndex];
            Helper.CopyBytes(blob, ref index, propSpec);
            Helper.CopyBytes(blob, ref index, BitConverter.GetBytes(vType));
            Helper.CopyBytes(blob, ref index, new byte[] { aggregateStored });
            Helper.CopyBytes(blob, ref index, new byte[] { aggregateType });
            Helper.CopyBytes(blob, ref index, new byte[] { valueUsed });
            if (padding != null)
                Helper.CopyBytes(blob, ref index, padding);
            if (valueUsed >= FIELD_USED)
            {
                Helper.CopyBytes
                    (blob, ref index, BitConverter.GetBytes(valueOffset));
                Helper.CopyBytes
                    (blob, ref index, BitConverter.GetBytes(valueSize));
            }
            Helper.CopyBytes(blob, ref index, new byte[] { statusUsed });
            if (padding2 != null)
                Helper.CopyBytes(blob, ref index, padding2);
            if (statusUsed >= FIELD_USED)
            {
                Helper.CopyBytes
                    (blob, ref index, BitConverter.GetBytes(statusOffset));
            }
            Helper.CopyBytes(blob, ref index, new byte[] { lengthUsed });
            if (padding3 != null)
                Helper.CopyBytes(blob, ref index, padding3);
            if (lengthUsed >= FIELD_USED)
            {
                Helper.CopyBytes
                    (blob, ref index, BitConverter.GetBytes(lengthOffset));
            }
            return blob;
        }

        /// <summary>
        /// Gets the CreateQueryIn message
        /// </summary>
        /// <param name="path">A null terminated unicode
        /// string representing the scope of search</param>
        /// <param name="queryText">A NON null terminated 
        /// unicode string representing the query string</param>
        /// <param name="numberOfCategorization">Number of 
        /// Categorization Set</param>
        public byte[] GetCPMCreateQueryIn
            (string path, string queryText, out uint numberOfCategorization)
        {

            byte[] externalPadding = null;
            byte[] externalPadding1 = null;

            //-----------
            int messageOffset = 0;
            uint size = 0;
            // to be assigned after fixating on the total size of the message.
            int index = 0;
            messageOffset += Constant.SIZE_OF_UINT;
            // Size of Size :)
            searchScope = path;
            queryString = queryText;
            byte IsColumnSetPresent = 0x01;
            messageOffset += Constant.SIZE_OF_BYTE;
            byte[] paddingColumnSetPresent = null;
            // To be assigned as per ColumnSet Presence.
            byte[] restrictionArray = null;
            byte[] columnSet = null;
            if (IsColumnSetPresent != 0)
            {
                paddingColumnSetPresent
                    = new byte[OFFSET_4 - messageOffset % 4];
                for (int i = 0; i < paddingColumnSetPresent.Length; i++)
                {
                    paddingColumnSetPresent[i] = 0;
                }
                messageOffset += paddingColumnSetPresent.Length;
                columnSet = GetColumnSet(ref messageOffset);
            }

            byte IsRestrictionPresent = 0x01;
            messageOffset += Constant.SIZE_OF_BYTE;
            if (IsRestrictionPresent != 0)
            {

                restrictionArray
                    = GetRestrictionArray
                    (ref messageOffset, queryString, searchScope);
            }

            byte IsSortPresent = 0x00;
            byte IsCategorizationSetPresent = 0x00;
            numberOfCategorization = 0;
            messageOffset += 2 * Constant.SIZE_OF_BYTE;

            if (messageOffset % 4 != 0)
            {
                externalPadding = new byte[OFFSET_4 - messageOffset % 4];
                messageOffset += externalPadding.Length;
            }
            //------------

            byte[] rowSetProperties = GetRowSetProperties(ref messageOffset);

            if (messageOffset % 4 != 0)
            {
                externalPadding1 = new byte[OFFSET_4 - messageOffset % 4];
                messageOffset += externalPadding1.Length;
            }
            //-------------
            byte[] pidMapper = GetPidMapper(ref messageOffset);

            byte[] messageBlob = new byte[messageOffset];
            size = (uint)messageOffset;
            Helper.CopyBytes
                (messageBlob, ref index, BitConverter.GetBytes(size));
            Helper.CopyBytes
                (messageBlob, ref index, new byte[] { IsColumnSetPresent });

            if (IsColumnSetPresent != 0)
            {
                Helper.CopyBytes
                    (messageBlob, ref index, paddingColumnSetPresent);
                Helper.CopyBytes
                    (messageBlob, ref index, columnSet);
            }
            Helper.CopyBytes
                (messageBlob, ref index, new byte[] { IsRestrictionPresent });
            if (IsRestrictionPresent != 0)
            {

                Helper.CopyBytes(messageBlob, ref index, restrictionArray);
            }
            Helper.CopyBytes
                (messageBlob, ref index, new byte[] { IsSortPresent });
            Helper.CopyBytes
                (messageBlob, ref index,
                new byte[] { IsCategorizationSetPresent });

            if (externalPadding != null)
                Helper.CopyBytes
                    (messageBlob, ref index, externalPadding);

            Helper.CopyBytes(messageBlob, ref index, rowSetProperties);

            if (externalPadding1 != null)
                Helper.CopyBytes(messageBlob, ref index, externalPadding1);
            //--------
            Helper.CopyBytes(messageBlob, ref index, pidMapper);
            return AddMessageHeader(MessageType.CPMCreateQueryIn, messageBlob);
        }

        /// <summary>
        /// Gets the CreateQueryIn message
        /// </summary>
        /// <param name="path">A null terminated unicode
        /// string representing the scope of search</param>
        /// <param name="queryText">A NON null terminated 
        /// unicode string representing the query string</param>
        /// <param name="numberOfCategorization">Number of 
        /// Categorization Set</param>
        /// <param name="ENABLEROWSETEVENTS">flag for ENABLEROWSETEVENTS</param>
        public byte[] GetCPMCreateQueryIn
            (string path, string queryText, out uint numberOfCategorization, bool ENABLEROWSETEVENTS)
        {

            byte[] externalPadding = null;
            byte[] externalPadding1 = null;

            //-----------
            int messageOffset = 0;
            uint size = 0;
            // to be assigned after fixating on the total size of the message.
            int index = 0;
            messageOffset += Constant.SIZE_OF_UINT;
            // Size of Size :)
            searchScope = path;
            queryString = queryText;
            byte IsColumnSetPresent = 0x01;
            messageOffset += Constant.SIZE_OF_BYTE;
            byte[] paddingColumnSetPresent = null;
            // To be assigned as per ColumnSet Presence.
            byte[] restrictionArray = null;
            byte[] columnSet = null;
            if (IsColumnSetPresent != 0)
            {
                //The length of this field MUST be such that the following field begins at an offset 
                //that is a multiple of 4 bytes from the beginning of the message that contains this structure.
                int messageSize = 4;
                paddingColumnSetPresent
                    = new byte[OFFSET_4 - messageOffset % messageSize];
                for (int i = 0; i < paddingColumnSetPresent.Length; i++)
                {
                    paddingColumnSetPresent[i] = 0;
                }
                messageOffset += paddingColumnSetPresent.Length;
                columnSet = GetColumnSet(ref messageOffset);
            }

            byte IsRestrictionPresent = 0x01;
            messageOffset += Constant.SIZE_OF_BYTE;
            if (IsRestrictionPresent != 0)
            {

                restrictionArray
                    = GetRestrictionArray
                    (ref messageOffset, queryString, searchScope);
            }

            byte IsSortPresent = 0x00;
            byte IsCategorizationSetPresent = 0x00;
            numberOfCategorization = 0;
            messageOffset += 2 * Constant.SIZE_OF_BYTE;

            //The length of this field MUST be such that the following field begins at an offset that is a multiple of 4 bytes 
            //from the beginning of the message that contains this structure.
            int length = 4;
            if (messageOffset % length != 0)
            {
                externalPadding = new byte[OFFSET_4 - messageOffset % length];
                messageOffset += externalPadding.Length;
            }
            //------------

            byte[] rowSetProperties = GetRowSetProperties(ENABLEROWSETEVENTS, ref messageOffset);

            if (messageOffset % length != 0)
            {
                externalPadding1 = new byte[OFFSET_4 - messageOffset % length];
                messageOffset += externalPadding1.Length;
            }
            //-------------
            byte[] pidMapper = GetPidMapper(ref messageOffset);

            byte[] messageBlob = new byte[messageOffset];
            size = (uint)messageOffset;
            Helper.CopyBytes
                (messageBlob, ref index, BitConverter.GetBytes(size));
            Helper.CopyBytes
                (messageBlob, ref index, new byte[] { IsColumnSetPresent });

            if (IsColumnSetPresent != 0)
            {
                Helper.CopyBytes
                    (messageBlob, ref index, paddingColumnSetPresent);
                Helper.CopyBytes
                    (messageBlob, ref index, columnSet);
            }
            Helper.CopyBytes
                (messageBlob, ref index, new byte[] { IsRestrictionPresent });
            if (IsRestrictionPresent != 0)
            {

                Helper.CopyBytes(messageBlob, ref index, restrictionArray);
            }
            Helper.CopyBytes
                (messageBlob, ref index, new byte[] { IsSortPresent });
            Helper.CopyBytes
                (messageBlob, ref index,
                new byte[] { IsCategorizationSetPresent });

            if (externalPadding != null)
                Helper.CopyBytes
                    (messageBlob, ref index, externalPadding);

            Helper.CopyBytes(messageBlob, ref index, rowSetProperties);

            if (externalPadding1 != null)
                Helper.CopyBytes(messageBlob, ref index, externalPadding1);
            //--------
            Helper.CopyBytes(messageBlob, ref index, pidMapper);
            return AddMessageHeader(MessageType.CPMCreateQueryIn, messageBlob);
        }
        #endregion

        /// <summary>
        /// Gets CPMGetScopeStatisticsIn message BLOB 
        /// </summary>
        /// <returns>CPMGetScopeStatisticsIn Message BLOB</returns>
        public byte[] GetCPMGetScopeStatisticsIn()
        {
            // Add Message Header
            byte[] mainBlob = new byte[Constant.SIZE_OF_UINT];
            return AddMessageHeader(MessageType.CPMGetScopeStatisticsIn, mainBlob);
        }

        #region Utility Methods
        /// <summary>
        /// Adds Header to a Message and also the checksum if required
        /// </summary>
        /// <param name="msgType">Type of message</param>
        /// <param name="messageBlob">Message BLOB</param>
        /// <returns>Message BLOB with Message Header Added</returns>
        private byte[] AddMessageHeader
            (MessageType msgType, byte[] messageBlob)
        {
            uint messageValue = 0;
            uint messageStatus = 0;
            uint reserveField = 0;
            bool requiresCheckSum = false;
            messageValue = (uint)msgType;
            switch (msgType)
            {
                case MessageType.CPMConnectIn:
                case MessageType.CPMCreateQueryIn:
                case MessageType.CPMGetRowsIn:
                case MessageType.CPMSetBindingsIn:
                case MessageType.CPMFetchValueIn:
                    requiresCheckSum = true;
                    break;
                default:
                    break;
            }
            int index = 0;
            uint checksum = 0;
            byte[] messagewithHeader = null;

            //If checksum is required
            //Calculate the checksum and assign the value
            if (requiresCheckSum)
            {
                if (messageBlob.Length % OFFSET_4 != 0)
                {
                    Array.Resize
                        (ref messageBlob,
                        messageBlob.Length
                        + (4 - messageBlob.Length % OFFSET_4));
                }

                while (index != messageBlob.Length)
                {
                    checksum += Helper.GetUInt(messageBlob, ref index);
                }

                checksum = checksum ^ CHECKSUM_XOR_VALUE;
                checksum -= messageValue;

            }
            index = 0;
            //Total message size
            messagewithHeader
                = new byte[4 * Constant.SIZE_OF_UINT + messageBlob.Length];
            Helper.CopyBytes
                (messagewithHeader, ref index,
                BitConverter.GetBytes(messageValue));
            Helper.CopyBytes
                (messagewithHeader, ref index,
                BitConverter.GetBytes(messageStatus));
            Helper.CopyBytes
                (messagewithHeader, ref index,
                BitConverter.GetBytes(checksum));
            Helper.CopyBytes
                (messagewithHeader, ref index,
                BitConverter.GetBytes(reserveField));
            Helper.CopyBytes(messagewithHeader, ref index, messageBlob);

            return messagewithHeader;
        }

        /// <summary>
        /// Returns the Storage SIZE of a given BaseStorageVariant type
        /// </summary>
        /// <param name="type">StorageType</param>
        /// <returns>size in bytes</returns>
        private ushort GetSize(StorageType type)
        {
            ushort size = 0;
            switch (type)
            {
                case StorageType.VT_EMPTY:
                    break;
                case StorageType.VT_NULL:
                    break;
                case StorageType.VT_I1:
                case StorageType.VT_UI1:
                    size = 1; // Take 1 Byte
                    break;
                case StorageType.VT_I2:
                case StorageType.VT_UI2:
                case StorageType.VT_BOOL:
                    size = 2; // Take 2 Bytes
                    break;
                case StorageType.VT_I4:
                case StorageType.VT_UI4:
                case StorageType.VT_INT:
                case StorageType.VT_UINT:
                case StorageType.VT_ERROR:
                case StorageType.VT_R4:
                    size = 4; // Take 4 byte
                    break;
                case StorageType.VT_I8:
                case StorageType.VT_UI8:
                case StorageType.VT_CY:
                case StorageType.VT_R8:
                case StorageType.VT_DATE:
                case StorageType.VT_CLSID:
                case StorageType.VT_FILETIME:
                    size = 8;
                    break;

                case StorageType.VT_DECIMAL:
                    size = 12;
                    break;
                case StorageType.VT_VARIANT:
                    size = 24;
                    break;
                default:
                    break;
            }
            return size;
        }

        private byte[] ToBytes(IWSPObject obj)
        {
            var buffer = new WSPBuffer();

            obj.ToBytes(buffer);

            return buffer.GetBytes();
        }
        #endregion
    }

    /// <summary>
    /// Represent the Table Column to be queried
    /// </summary>
    public struct TableColumn
    {
        /// <summary>
        /// Guid of the Column
        /// </summary>
        public Guid Guid;
        /// <summary>
        /// Property Id of the column
        /// </summary>
        public uint PropertyId;
        /// <summary>
        /// Base Storage Type of the Column
        /// </summary>
        public StorageType Type;
        /// <summary>
        /// Length Offset of the Column
        /// </summary>
        public ushort LengthOffset;
        /// <summary>
        /// Value offset of the column
        /// </summary>
        public ushort ValueOffset;
        /// <summary>
        /// Status Offset of the Column
        /// </summary>
        public ushort StatusOffset;
    }

    /// <summary>
    /// ColumnId Class representing DBColumnId structure
    /// </summary>
    internal class ColumnId
    {
        /// <summary>
        /// Seek Kind for ConnectIn message
        /// </summary>
        public Ekind eKind;
        /// <summary>
        /// Table Column Guid
        /// </summary>
        public Guid guid;
        /// <summary>
        /// Specifies Property type (Name or ID )
        /// </summary>
        public uint UlId;
        /// <summary>
        /// Name of the Property
        /// </summary>
        public string propertyName = string.Empty;
        /// <summary>
        /// Property Id
        /// </summary>
        public uint propertyId = 0;
    }
}
