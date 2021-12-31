// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp.Adapter
{
    public class MessageBuilderParameter
    {
        public string[] PropertySet_One_DBProperties;

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

        public uint EType;

        public uint BufferSize;

        public uint LcidValue;

        public uint ClientBase;

        public uint RowsToTransfer;
    }

    /// <summary>
    /// Message Builder class provides methods to build MS-WSP request messages
    /// </summary>
    public class MessageBuilder
    {
        #region Fields

        /// <summary>
        /// Specifies a field is Used
        /// </summary>
        private const byte FIELD_USED = 0x01;

        /// <summary>
        /// Specifies a field is NOT used
        /// </summary>
        private const byte FIELD_NOT_USED = 0x00;

        /// <summary>
        /// Specifies alignment of 4 bytes
        /// </summary>
        private const byte OFFSET_4 = 4;

        /// <summary>
        /// Specifies alignment of 8 bytes
        /// </summary>
        private const byte OFFSET_8 = 8;

        /// <summary>
        /// Represent a search scope
        /// </summary>
        private string searchScope = string.Empty;

        /// <summary>
        /// Represent a query string
        /// </summary>
        private string queryString = string.Empty;

        /// <summary>
        /// Specifies the property type is ID
        /// </summary>
        private const int PROPERTY_ID = 0x1;

        /// <summary>
        /// Length of each message header
        /// </summary>
        private const int HEADER_LENGTH = 16;

        /// <summary>
        /// Specifies comma separated properties
        /// </summary>
        private static char[] delimiter = new char[] { ',' };

        /// <summary>
        /// Number of external property set count
        /// </summary>
        private const uint EXTERNAL_PROPSET_COUNT = 4;

        /// <summary>
        /// Value to XOR with for Header checksum
        /// </summary>
        private const uint CHECKSUM_XOR_VALUE = 0x59533959;

        /// <summary>
        /// Value to OR with for Safe Array Type
        /// </summary>
        private const ushort SAFE_ARRAY_TYPE = 0x2000;

        /// <summary>
        /// Value to OR with for Vector Type
        /// </summary>
        private const ushort VECTOR_TYPE = 0x1000;

        /// <summary>
        /// Command time out
        /// </summary>
        private const uint COMMAND_TIME_OUT = 0x1E;

        /// <summary>
        /// Content Restriction operation
        /// </summary>
        private const uint RELATION_OPERATION = 0x4;

        /// <summary>
        /// Id of Property Restrictin Node
        /// </summary>
        private const uint PROPERTY_RESTRICTION_NODE_ID = 5;

        /// <summary>
        /// Id of Content Restriction Node
        /// </summary>
        private const uint CONTENT_RESTRICTION_NODE_ID = 4;

        /// <summary>
        /// Wieghtage of the Node
        /// </summary>
        private const uint NODE_WEIGHTAGE = 1000;

        /// <summary>
        /// Means Logical AND operation between Node Restriction
        /// </summary>
        private const uint LOGICAL_AND = 0x1;

        /// <summary>
        /// Static value for chapter of CPMGetRowsIn message
        /// </summary>
        private static uint chapter;

        /// <summary>
        /// Static value for RowWidth of CPMGetRowsIn message
        /// </summary>
        public static uint RowWidth = 92;

        /// <summary>
        /// The parameter of the builder.
        /// </summary>
        public MessageBuilderParameter Parameter;

        /// <summary>
        /// Indicates if we use 64-bit or 32-bit when building requests.
        /// </summary>
        public bool Is64bit = true;

        #endregion

        /// <summary>
        /// Constructor takes the ITestSite as parameter
        /// </summary>
        /// <param name="testSite">Site from where it needs to read configurable data</param>
        public MessageBuilder(MessageBuilderParameter parameter)
        {
            this.Parameter = parameter;
        }

        #region MS-WSP Request Messages

        /// <summary>
        /// Builds CPMConnectIn message 
        /// </summary>
        /// <param name="clientVersion">Version of the protocol client</param>
        /// <param name="isRemote">If the query is remote, then 1 else 0</param>
        /// <param name="userName">User initiating the connection</param>
        /// <param name="machineName">Client Machine Name</param>
        /// <param name="serverMachineName">Server Machine Name</param>
        /// <param name="catalogName">Name of the Catalog under operation</param>
        /// <param name="languageLocale">Language Locale</param>
        /// <param name="cPropSets">The number of CDbPropSet structures in the following fields</param>
        /// <param name="cExtPropSet">The number of CDbPropSet structures in aPropertySet</param>
        /// <returns>CPMConnectIn message</returns>
        public CPMConnectIn GetCPMConnectIn(
            uint clientVersion,
            int isRemote,
            string userName,
            string machineName,
            string serverMachineName,
            string catalogName,
            string languageLocale,
            uint cPropSets = 2,
            uint cExtPropSet = 4)
        {
            var message = new CPMConnectIn();

            message._iClientVersion = clientVersion;

            message._fClientIsRemote = (uint)isRemote;

            message.MachineName = machineName;

            message.UserName = userName;

            message.cPropSets = cPropSets;

            message.PropertySet1 = GetPropertySet1(catalogName);
            // PropertySet1 specifying MachineName
            message.PropertySet2 = GetPropertySet2(serverMachineName);

            message.cExtPropSet = cExtPropSet;

            message.aPropertySets = new CDbPropSet[4];

            message.aPropertySets[0] = GetAPropertySet1(languageLocale); //Language locale
            message.aPropertySets[1] = GetAPropertySet2(); // FLAGs
            message.aPropertySets[2] = GetAPropertySet3(serverMachineName); // server
            message.aPropertySets[3] = GetAPropertySet4(catalogName); // Catalog

            message.Header = new WspMessageHeader
            {
                _msg = WspMessageHeader_msg_Values.CPMConnectIn,
            };

            return message;
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
            byte[] bytes = new byte[Constants.SIZE_OF_UINT/*Cursor Handle */
                + Constants.SIZE_OF_UINT/* BookMark Handle */];
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
        public CPMCiStateInOut GetCPMCiStateInOut()
        {
            var message = new CPMCiStateInOut()
            {
                Header = new WspMessageHeader()
                {
                    _msg = WspMessageHeader_msg_Values.CPMCiStateInOut,
                },

                State = new CPMCiState
                {
                    cbStruct = 0x0000003C,
                },
            };

            return message;
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
            int ratioFinishedInMessageLength = 2 * Constants.SIZE_OF_UINT;
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
        /// Gets CPMFetchValueIn Message
        /// </summary>
        /// <param name="workID">Document ID</param>
        /// <param name="cbSoFar">Number of bytes previously transferred</param>
        /// <param name="cbChunk">Maximum number of bytes that the sender can accept in a CPMFetchValueOut message</param>
        /// <param name="propSpec">The proptery to fetch</param>
        /// <returns>CPMFetchValueIn Message</returns>
        public CPMFetchValueIn GetCPMFetchValueIn(uint workID, uint cbSoFar, uint cbChunk, CFullPropSpec propSpec)
        {
            var message = new CPMFetchValueIn
            {
                Header = new WspMessageHeader
                {
                    _msg = WspMessageHeader_msg_Values.CPMFetchValueIn
                },
                _wid = workID,
                _cbSoFar = cbSoFar,
                _cbChunk = cbChunk,
                PropSpec = propSpec
            };

            return message;
        }

        /// <summary>
        /// Gets CPMCompareBmkIn Message Blob
        /// </summary>
        /// <param name="cursor">Handle from the CPMCreateQueryOut message</param>
        /// <param name="chapt">Handle of the chapter containing the bookmarks to compare</param>
        /// <param name="bmkFirst">Handle to the first bookmark to compare</param>
        /// <param name="bmkSecond">Handle to the second bookmark to compare</param>
        /// <returns>CPMCompareBmkIn BLOB</returns>
        public byte[] GetCPMCompareBmkIn(uint cursor, uint chapt, uint bmkFirst, uint bmkSecond)
        {
            int compareBmkInMessageLength = 4 * Constants.SIZE_OF_UINT;
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
        /// <param name="cursor">Handle obtained from a CPMCreateQueryOut message</param>
        /// <param name="chapt">Handle of a chapter from which to retrieve rows</param>
        /// <returns>CPMRestartPositionIn BLOB</returns>
        public byte[] GetCPMRestartPositionIn(uint cursor, uint chapt)
        {
            int restartPositionInMessageLength = 2 * Constants.SIZE_OF_UINT;
            byte[] mainBlob = new byte[restartPositionInMessageLength];
            //================ Converting values into Bytes ====================
            int index = 0;
            Helper.CopyBytes(mainBlob, ref index, BitConverter.GetBytes(cursor));
            Helper.CopyBytes(mainBlob, ref index, BitConverter.GetBytes(chapt));
            return AddMessageHeader(MessageType.CPMRestartPositionIn, mainBlob);
        }

        /// <summary>
        /// Gets CPMFreeCursorIn Message
        /// </summary>
        /// <param name="cursor">Handle from the CPMCreateQueryOut message</param>
        /// <returns>CPMFreeCursorIn Message</returns>
        public CPMFreeCursorIn GetCPMFreeCursorIn(uint cursor)
        {
            var message = new CPMFreeCursorIn
            {
                Header = new WspMessageHeader
                {
                    _msg = WspMessageHeader_msg_Values.CPMFreeCursorIn,
                },
                _hCursor = cursor
            };

            return message;
        }

        /// <summary>
        /// Gets CPMGetApproximatePositionIn Message Blob
        /// </summary>
        /// <param name="cursor">Handle from CPMCreateQueryOut message</param>
        /// <param name="chapt">Handle to the chapter containing the bookmark</param>
        /// <param name="bmk">Handle to the bookmark for which to retrieve the approximate position</param>
        /// <returns>CPMGetApproximatePosiionIn Mesage</returns>
        public byte[] GetCPMApproximatePositionIn(uint cursor, uint chapt, uint bmk)
        {
            int approximatePositionInLength = 3 * Constants.SIZE_OF_UINT;
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
        public byte[] GetCPMDisconnect()
        {
            byte[] messageHeader = null;
            int index = 0;
            //Total message size
            uint messageValue = (uint)MessageType.CPMDisconnect;
            uint messageStatus = 0;
            uint checksum = 0;
            uint reserveField = 0;
            messageHeader = new byte[4 * Constants.SIZE_OF_UINT];
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
        /// Build CPMFindIndicesIn message
        /// </summary>
        /// <param name="cWids"></param>
        /// <param name="cDepthPrev"></param>
        /// <returns>CPMFindIndices BLOB</returns>
        public byte[] GetCPMFindIndicesIn(uint cWids, uint cDepthPrev)
        {
            int index = 0;
            int messageOffset = 4 * Constants.SIZE_OF_UINT;
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
        public byte[] GetCPMGetRowsetNotifyIn()
        {
            byte[] mainBlob = new byte[Constants.SIZE_OF_UINT];

            return AddMessageHeader
                (MessageType.CPMGetRowsetNotifyIn, mainBlob);
        }

        /// <summary>
        /// Build CPMSetScopePrioritizationIn message
        /// </summary>
        /// <param name="priority"></param>
        /// <param name="eventFrequency"></param>
        /// <returns>CPMSetScopePrioritization BLOB</returns>
        public byte[] GetCPMSetScopePrioritizationIn(uint priority, uint eventFrequency)
        {
            byte[] mainBlob = new byte[2 * Constants.SIZE_OF_UINT];

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
            messageHeader = new byte[4 * Constants.SIZE_OF_UINT];
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
        /// <param name="catalogName">Name of the catalog</param>
        /// <returns>PropertySet BLOB</returns>
        private CDbPropSet GetPropertySet1(string catalogName)
        {
            var propSet = new CDbPropSet();

            uint cProperties = 4;

            propSet.guidPropertySet = WspConsts.DBPROPSET_FSCIFRMWRK_EXT;

            propSet.cProperties = cProperties;

            propSet.aProps = new CDbProp[0];

            // Get First PropSet with Guid Value DBPROPSET_FSCIFRMWRK_EXT
            foreach (string property in Parameter.PropertySet_One_DBProperties)
            {
                switch (property)
                {
                    case "2":
                        // Creating CDBProp with PropId 2 for GUID type FSCIFRMWRK
                        var value_FSCIFRMWRK_2 = GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_LPWSTR, new VT_LPWSTR(catalogName));
                        AppendDbProp(ref propSet, 2, value_FSCIFRMWRK_2);
                        break;

                    case "3":
                        // Creating CDBProp with PropId 3 for GUID type FCCIFRMWRK
                        var value_FCCIFRMWRK_3 = GetVector(CBaseStorageVariant_vType_Values.VT_LPWSTR, new VT_LPWSTR[] { new VT_LPWSTR(null) });
                        AppendDbProp(ref propSet, 3, value_FCCIFRMWRK_3);
                        break;

                    case "4":
                        // Creating CDBProp with PropId 4 for GUID type FSCIFRMWRK
                        var value_FSCIFRMWRK_4 = GetVector(CBaseStorageVariant_vType_Values.VT_I4, new int[] { 1 });
                        AppendDbProp(ref propSet, 4, value_FSCIFRMWRK_4);
                        break;

                    case "7":
                        // Creating CDBProp with PropId 7 for GUID type FSCIFRMWRK
                        var value_FSCIFRMWRK_7 = GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_I4, (int)0);
                        AppendDbProp(ref propSet, 7, value_FSCIFRMWRK_7);
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
        /// <param name="machineName">Name of the connecting client</param>
        /// <returns>PropertySet BLOB</returns>
        private CDbPropSet GetPropertySet2(string machineName)
        {
            var propSet = new CDbPropSet();

            uint cProperties = 1;

            propSet.guidPropertySet = WspConsts.DBPROPSET_CIFRMWRKCORE_EXT;

            propSet.cProperties = cProperties;

            propSet.aProps = new CDbProp[0];

            foreach (string property in Parameter.PropertySet_Two_DBProperties)
            {
                switch (property)
                {
                    case "2":
                        // Creating CDBProp with PropId 2 for GUID type CIFRMWRKCORE
                        var value_CIFRMWRKCORE_2 = GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_BSTR, new VT_BSTR(machineName));
                        AppendDbProp(ref propSet, 2, value_CIFRMWRKCORE_2);
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
        /// <param name="languageLocale">language locale of the client</param>
        /// <returns>PropertySet BLOB</returns>
        private CDbPropSet GetAPropertySet1(string languageLocale)
        {
            var propSet = new CDbPropSet();

            uint cProperties = 6;

            var guid = Parameter.Array_PropertySet_One_Guid;

            propSet.cProperties = cProperties;

            propSet.guidPropertySet = guid;

            propSet.aProps = new CDbProp[0];

            // Compile aPropertySet1 with Guid Value DBPROPSET_MSIDXS_ROWSETEXT
            foreach (string property in Parameter.Array_PropertySet_One_DBProperties)
            {
                switch (property)
                {
                    case "2":
                        // Creating CDBProp with PropId 2 for GUID type ROWSETEXT
                        var value_ROWSETEXT_2 = GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_I4, (int)0);
                        AppendDbProp(ref propSet, 2, value_ROWSETEXT_2);
                        break;

                    case "3":
                        // Creating CDBProp with PropId 3 for GUID type ROWSETEXT
                        var value_ROWSETEXT_3 = GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_BSTR, new VT_BSTR(languageLocale));
                        AppendDbProp(ref propSet, 3, value_ROWSETEXT_3);
                        break;

                    case "4":
                        // Creating CDBProp with PropId 4 for GUID type ROWSETEXT
                        var value_ROWSETEXT_4 = GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_BSTR, new VT_BSTR(""));
                        AppendDbProp(ref propSet, 4, value_ROWSETEXT_4);
                        break;

                    case "5":
                        // Creating CDBProp with PropId 5 for GUID type ROWSETEXT
                        var value_ROWSETEXT_5 = GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_BSTR, new VT_BSTR(""));
                        AppendDbProp(ref propSet, 5, value_ROWSETEXT_5);
                        break;

                    case "6":
                        // Creating CDBProp with PropId 6 for GUID type ROWSETEXT
                        var value_ROWSETEXT_6 = GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_I4, (int)0);
                        AppendDbProp(ref propSet, 6, value_ROWSETEXT_6);
                        break;

                    case "7":
                        // Creating CDBProp with PropId 7 for GUID type ROWSETEXT
                        var value_ROWSETEXT_7 = GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_I4, (int)0);
                        AppendDbProp(ref propSet, 7, value_ROWSETEXT_7);
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
        /// <returns>PropertySet BLOB</returns>
        private CDbPropSet GetAPropertySet2()
        {
            var propSet = new CDbPropSet();

            uint cProperties = 10;
            var guid = Parameter.Array_PropertySet_Two_Guid;

            propSet.cProperties = cProperties;
            propSet.guidPropertySet = guid;
            propSet.aProps = new CDbProp[0];

            // Compile aPropertySet2 with Guid Value DBPROPSET_QUERYEXT
            foreach (string property in Parameter.Array_PropertySet_Two_DBProperties)
            {
                switch (property)
                {
                    case "2":
                        // Creating CDBProp with PropId 2 for GUID type QUERYEXT
                        var value_QUERYEXT_2 = GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_BOOL, (ushort)0x0000);
                        AppendDbProp(ref propSet, 2, value_QUERYEXT_2);
                        break;

                    case "3":
                        // Creating CDBProp with PropId 3 for GUID type QUERYEXT
                        var value_QUERYEXT_3 = GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_BOOL, (ushort)0x0000);
                        AppendDbProp(ref propSet, 3, value_QUERYEXT_3);
                        break;

                    case "4":
                        // Creating CDBProp with PropId 4 for GUID type QUERYEXT
                        var value_QUERYEXT_4 = GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_BOOL, (ushort)0x0000);
                        AppendDbProp(ref propSet, 4, value_QUERYEXT_4);
                        break;

                    case "5":
                        // Creating CDBProp with PropId 5 for GUID type QUERYEXT
                        var value_QUERYEXT_5 = GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_BOOL, (ushort)0x0000);
                        AppendDbProp(ref propSet, 5, value_QUERYEXT_5);
                        break;

                    case "6":
                        // Creating CDBProp with PropId 6 for GUID type QUERYEXT
                        var value_QUERYEXT_6 = GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_BSTR, new VT_BSTR(""));
                        AppendDbProp(ref propSet, 6, value_QUERYEXT_6);
                        break;

                    case "8":
                        // Creating CDBProp with PropId 8 for GUID type QUERYEXT
                        var value_QUERYEXT_8 = GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_BOOL, (ushort)0x0000);
                        AppendDbProp(ref propSet, 8, value_QUERYEXT_8);
                        break;

                    case "10":
                        // Creating CDBProp with PropId 10 for GUID type QUERYEXT
                        var value_QUERYEXT_10 = GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_BOOL, (ushort)0x0000);
                        AppendDbProp(ref propSet, 10, value_QUERYEXT_10);
                        break;

                    case "12":
                        // Creating CDBProp with PropId 12 for GUID type QUERYEXT
                        var value_QUERYEXT_12 = GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_BOOL, (ushort)0x0000);
                        AppendDbProp(ref propSet, 12, value_QUERYEXT_12);
                        break;

                    case "13":
                        // Creating CDBProp with PropId 13 for GUID type QUERYEXT
                        var value_QUERYEXT_13 = GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_BOOL, (ushort)0x0000);
                        AppendDbProp(ref propSet, 13, value_QUERYEXT_13);
                        break;

                    case "14":
                        // Creating CDBProp with PropId 14 for GUID type QUERYEXT
                        var value_QUERYEXT_14 = GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_BOOL, (ushort)0x0000);
                        AppendDbProp(ref propSet, 14, value_QUERYEXT_14);
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
        /// <param name="serverName">Name of the Server to connect</param>
        /// <returns>BLOB</returns>
        private CDbPropSet GetAPropertySet3(string serverName)
        {
            var propSet = new CDbPropSet();

            uint cProperties = 1;
            var guid = Parameter.Array_PropertySet_Three_Guid;

            propSet.cProperties = cProperties;
            propSet.guidPropertySet = guid;
            propSet.aProps = new CDbProp[0];

            foreach (string property in Parameter.Array_PropertySet_Three_DBProperties)
            {
                switch (property)
                {
                    case "2":
                        // Creating CDBProp with PropId 2
                        // for GUID type FSCIFRMWRK_EXT
                        var value_FSCIFRMWRK_EXT_2 = GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_BSTR, new VT_BSTR(serverName));
                        AppendDbProp(ref propSet, 2, value_FSCIFRMWRK_EXT_2);
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
        /// <param name="catalogName">Name of the Catalog to connect</param>
        /// <returns>BLOB</returns>
        private CDbPropSet GetAPropertySet4(string catalogName)
        {
            var propSet = new CDbPropSet();

            uint cProperties = 3;

            var guid = Parameter.Array_PropertySet_Four_Guid;

            propSet.cProperties = cProperties;
            propSet.guidPropertySet = guid;
            propSet.aProps = new CDbProp[0];

            // Compile aPropertySet4 with Guid Value DBPROPSET_CIFRMWRKCORE_EXT
            foreach (string property in Parameter.Array_PropertySet_Four_DBProperties)
            {
                switch (property)
                {
                    case "2":
                        // Creating CDBProp with PropId 2 for GUID type CIFRMWRKCORE_EXT
                        var value_CIFRMWRKCORE_EXT_2 = GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_BSTR, new VT_BSTR(catalogName));
                        AppendDbProp(ref propSet, 2, value_CIFRMWRKCORE_EXT_2);
                        break;

                    case "3":
                        // Creating CDBProp with PropId 3 for GUID type CIFRMWRKCORE_EXT
                        var safeArrayForStr = GetSafeArray<VT_BSTR>(CBaseStorageVariant_vType_Values.VT_BSTR, new VT_BSTR[] { new VT_BSTR("") }, 0);
                        AppendDbProp(ref propSet, 3, safeArrayForStr);
                        break;

                    case "4":
                        // Creating CDBProp with PropId 4 for GUID type CIFRMWRKCORE_EXT
                        var safeArrayForInt = GetSafeArray<int>(CBaseStorageVariant_vType_Values.VT_I4, new int[] { 0 }, 0);
                        AppendDbProp(ref propSet, 4, safeArrayForInt);
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
        /// <param name="value">BLOB of the Value field</param>
        private void AppendDbProp(ref CDbPropSet propertySet, uint id, CBaseStorageVariant value)
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
        /// <returns>BLOB ColumnID</returns>
        private CDbColId GetColumnId()
        {
            var result = new CDbColId(WspConsts.EmptyGuid, 0);
            return result;
        }

        #endregion

        #region Structures of CPMGetRowsIn message

        /// <summary>
        /// Gets FullPropertySpec for a given GUID, eKIND and propSec Id
        /// </summary>
        /// <param name="guid">PropSpec GUID</param>
        /// <param name="kind">EKind</param>
        /// <param name="propSec">PropSec Id</param>
        /// <param name="messageOffset">offset from the beginning of the message</param>
        /// <returns>full property spec structure BLOB</returns>
        private byte[] GetFullPropSec(Guid guid, int kind, int propSec, ref int messageOffset)
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
            messageOffset += Constants.SIZE_OF_GUID;
            uint ulKind = (uint)kind;
            uint pRpSec = (uint)propSec;
            messageOffset += 2 * Constants.SIZE_OF_UINT;
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
        /// Gets the Seek Description type for the RowsIn message
        /// </summary>
        /// <param name="eType">eType</param>
        /// <returns>returns object form of SeekDescription variable</returns>
        private IWspSeekDescription GetSeekDescription(CPMGetRowsIn_eType_Values eType)
        {
            IWspSeekDescription result = null;

            switch (eType)
            {
                case CPMGetRowsIn_eType_Values.None:
                    break;

                case CPMGetRowsIn_eType_Values.eRowSeekNext:
                    {
                        result = new CRowSeekNext()
                        {
                            _cskip = 0x00000000,
                        };
                    }
                    break;

                case CPMGetRowsIn_eType_Values.eRowSeekAt:
                    {
                        result = new CRowSeekAt()
                        {
                            _bmkOffset = 2,
                            _cskip = 2,
                            _hRegion = 0,
                        };
                    }
                    break;

                case CPMGetRowsIn_eType_Values.eRowSeekAtRatio:
                    {
                        result = new CRowSeekAtRatio()
                        {
                            _ulNumerator = 1,
                            _ulDenominator = 2,
                            _hRegion = 0,
                        };
                    }
                    break;

                default:
                    throw new InvalidOperationException("Unsupported eType!");
            }

            return result;
        }

        /// <summary>
        /// Gets CRestrictionArray structure
        /// </summary>
        /// <param name="queryString">Search Query String</param>
        /// <param name="searchScope">Search Query Scope</param>
        /// <param name="queryStringProperty">The property used by queryString</param>
        /// <returns>CRestrictionArray structure BLOB</returns>
        public CRestrictionArray GetRestrictionArray(string queryString, string searchScope, CFullPropSpec queryStringProperty)
        {
            var result = new CRestrictionArray();

            result.count = 0x01;

            result.isPresent = 0x01;

            result.Restriction = GetQueryPathRestriction(queryString, searchScope, queryStringProperty);

            return result;
        }

        /// <summary>
        /// Gets CRestrictionArray structure
        /// </summary>
        /// <param name="restrictions">The restrictions to be inserted to the array</param>
        /// <returns>CRestrictionArray structure BLOB</returns>
        public CRestrictionArray GetRestrictionArray(params CRestriction[] restrictions)
        {
            var result = new CRestrictionArray();

            result.count = 0x01;
            result.isPresent = 0x01;
            result.Restriction = GetNodeRestriction(CRestriction_ulType_Values.RTAnd, restrictions);

            return result;
        }

        /// <summary>
        /// Gets node restriction
        /// </summary>
        /// <param name="ulType">Indicates the restriction type used for the command tree node</param>
        /// <param name="restrictions">The child restrictions to be inserted to the restriction</param>
        public CRestriction GetNodeRestriction(CRestriction_ulType_Values ulType, params CRestriction[] restrictions)
        {
            var restriction = new CRestriction();
            restriction._ulType = ulType;
            restriction.Weight = NODE_WEIGHTAGE;
            var node = new CNodeRestriction();
            node._cNode = (uint)restrictions.Length;
            node._paNode = new CRestriction[node._cNode];
            for (int i = 0; i < restrictions.Length; i++)
            {
                node._paNode[i] = restrictions[i];
            }
            restriction.Restriction = node;

            return restriction;
        }

        /// <summary>
        /// Gets Node restriction specific to the query 
        /// scope and the queryText
        /// </summary>
        /// <returns>CPropertyRestrictionNode structure BLOB</returns>
        public CRestriction GetQueryPathRestriction(string queryString, string searchScope, CFullPropSpec queryStringProperty)
        {
            var result = new CRestriction();

            result._ulType = CRestriction_ulType_Values.RTAnd;

            result.Weight = NODE_WEIGHTAGE;

            var node = new CNodeRestriction();

            node._cNode = 2;

            node._paNode = new CRestriction[2];

            node._paNode[0] = GetPropertyRestriction(CPropertyRestriction_relop_Values.PREQ, WspConsts.System_Search_Scope, GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_LPWSTR, new VT_LPWSTR(searchScope)));

            node._paNode[1] = GetContentRestriction(queryString, queryStringProperty);

            result.Restriction = node;

            return result;
        }

        /// <summary>
        /// Gets CPropertyRestriction specific to the query path
        /// </summary>
        /// <returns></returns>
        public CRestriction GetPropertyRestriction(CPropertyRestriction_relop_Values relopValue, CFullPropSpec propSpec, CBaseStorageVariant prVal)
        {
            var result = new CRestriction();

            result._ulType = CRestriction_ulType_Values.RTProperty;

            result.Weight = NODE_WEIGHTAGE;

            var node = new CPropertyRestriction();

            node._relop = relopValue;

            node._Property = propSpec;

            node._prval = prVal;

            node._lcid = Parameter.LcidValue;

            result.Restriction = node;

            return result;
        }

        /// <summary>
        /// Get rowset event structure
        /// </summary>
        /// <param name="ENABLEROWSETEVENTS"></param>
        /// <returns></returns>
        private CRowsetProperties GetRowSetProperties(bool ENABLEROWSETEVENTS)
        {
            var result = new CRowsetProperties();

            if (ENABLEROWSETEVENTS)
            {
                result._uBooleanOptions = CRowsetProperties_uBooleanOptions_Values.eScrollable | CRowsetProperties_uBooleanOptions_Values.eEnableRowsetEvents;
                Constants.DBPROP_ENABLEROWSETEVENTS = true;
            }
            else
            {
                result._uBooleanOptions = CRowsetProperties_uBooleanOptions_Values.eScrollable;
                Constants.DBPROP_ENABLEROWSETEVENTS = false;
            }

            result._ulMaxOpenRows = 0x00000000;

            result._ulMemoryUsage = 0x00000000;

            result._cMaxResults = 0x00000000;

            result._cCmdTimeout = COMMAND_TIME_OUT;

            return result;
        }

        /// <summary>
        /// Gets the ContentRestrictionNode structure specific 
        /// to the query Text
        /// </summary>
        /// <param name="queryString">Query String of the search</param>
        /// <param name="queryStringProperty">Property used by Query string</param>
        /// <param name="generateMethod">The value of _ulGenerateMethod field</param>
        /// <returns>ContentRestriction structure Node</returns>
        public CRestriction GetContentRestriction(string queryString, CFullPropSpec queryStringProperty, CContentRestriction_ulGenerateMethod_Values generateMethod = CContentRestriction_ulGenerateMethod_Values.GENERATE_METHOD_EXACT)
        {
            var result = new CRestriction();

            result._ulType = CRestriction_ulType_Values.RTContent;

            result.Weight = NODE_WEIGHTAGE;

            var node = new CContentRestriction();

            node._Property = queryStringProperty;

            node.Cc = (uint)queryString.Length;

            node._pwcsPhrase = queryString;

            node.Lcid = Parameter.LcidValue;

            node._ulGenerateMethod = generateMethod;

            result.Restriction = node;

            return result;

        }

        #endregion

        #region Helper methods for Base Storage Type

        /// <summary>
        /// Gets a Vector form of a Base Storage Type
        /// </summary>
        /// <param name="type">vType_Values (Vector Item type)</param>
        /// <param name="inputValues">vType_Values (Vector Item values)</param>
        /// <returns>Vector Base Storage Type BLOB</returns>
        private CBaseStorageVariant GetVector<T>(CBaseStorageVariant_vType_Values type, T[] inputValues) where T : struct
        {
            var result = new CBaseStorageVariant();
            result.vType = type | CBaseStorageVariant_vType_Values.VT_VECTOR;
            result.vData1 = 0;
            result.vData2 = 0;

            switch (type)
            {
                case CBaseStorageVariant_vType_Values.VT_I4:
                    {
                        var vector = new VT_VECTOR<int>();
                        vector.vVectorElements = (uint)inputValues.Length;
                        vector.vVectorData = inputValues.Cast<int>().ToArray();
                        result.vValue = vector;
                    }
                    break;

                case CBaseStorageVariant_vType_Values.VT_LPWSTR:
                    {
                        var vector = new VT_VECTOR<VT_LPWSTR>();
                        vector.vVectorElements = (uint)inputValues.Length;
                        vector.vVectorData = inputValues.Cast<VT_LPWSTR>().ToArray();
                        result.vValue = vector;
                    }
                    break;

                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// Gets Safe Array of given vType_Values
        /// </summary>
        /// <param name="type">Type of item(s)</param>
        /// <param name="arrayValue">Value of item(s)</param>
        /// <param name="features">Safe Array Features</param>
        /// <returns>Safe Array Storage Type</returns>
        private CBaseStorageVariant GetSafeArray<T>(CBaseStorageVariant_vType_Values type, T[] arrayValue, ushort features) where T : struct
        {
            var result = new CBaseStorageVariant();
            result.vType = type | CBaseStorageVariant_vType_Values.VT_ARRAY;
            result.vData1 = 0;
            result.vData2 = 0;

            var array = new SAFEARRAY<T>();
            array.cDims = 1;
            array.fFeatures = features;

            var tempBuffer = new WspBuffer();
            if (arrayValue[0] is IWspStructure)
            {
                (arrayValue[0] as IWspStructure).ToBytes(tempBuffer);
            }
            else
            {
                tempBuffer.Add(arrayValue[0]);
            }

            array.cbElements = (uint)tempBuffer.WriteOffset;

            array.Rgsabound = new SAFEARRAYBOUND[] { new SAFEARRAYBOUND() { cElements = (uint)arrayValue.Length, lLbound = 0 } };
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
            byte[] value = new byte[2 * Constants.SIZE_OF_UINT];
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
        /// <param name="inputValue">input value, if isArray is true, pass values as array of objects</param>
        /// <returns>CBaseStorageVariant BLOB</returns>
        public CBaseStorageVariant GetBaseStorageVariant(CBaseStorageVariant_vType_Values type, object inputValue)
        {
            var result = new CBaseStorageVariant();
            ushort vType = (ushort)type;
            byte vData1 = 0;
            byte vData2 = 0;

            result.vType = (CBaseStorageVariant_vType_Values)vType;

            result.vData1 = vData1;

            result.vData2 = vData2;

            result.vValue = inputValue;

            return result;
        }

        #endregion

        #region Query Trio Messages (CreateQueryIn, SetBindingsIn and GetRowsIn)

        /// <summary>
        /// Gets the CPMSetBindingsIn message
        /// </summary>
        /// <param name="queryCursor">Query associated Cursor</param>
        /// <param name="columns">Array of TableColumns to be Queried</param>
        /// <param name="isValidBinding">True if the binding is valid</param>
        /// <returns>CPMSetBindingsIn message.</returns>
        public CPMSetBindingsIn GetCPMSetBindingsIn(uint queryCursor, out TableColumn[] columns, bool isValidBinding)
        {
            uint cursor = queryCursor;
            uint rows = (uint)Parameter.EachRowSize;
            // SIZE of ColumnCount and Columns combined to be assigned later.
            uint dummy = 0;// Dummy value
            Random r = new Random();
            columns = GetDefaultTableColumns();

            if (!isValidBinding)
            {
                // Decreasing the number of bytes to fail the bindings
                rows -= (uint)r.Next((int)rows - 10, (int)rows);
            }

            uint columnCount = (uint)columns.Length;

            var message = new CPMSetBindingsIn()
            {
                Header = new WspMessageHeader
                {
                    _msg = WspMessageHeader_msg_Values.CPMSetBindingsIn,
                },

                _hCursor = cursor,

                _cbRow = rows,

                _dummy = dummy,

                cColumns = columnCount,

                aColumns = columns.Select(column => GetTableColumn(column)).ToArray(),
            };

            Helper.UpdateTableColumns(message.aColumns);

            return message;
        }

        /// <summary>
        /// Get the default table columns details.
        /// </summary>
        /// <returns>Array of Table Columns</returns>
        public TableColumn[] GetDefaultTableColumns()
        {
            var columns = new TableColumn[]
            {
                new TableColumn()
                {
                    Property = WspConsts.System_ItemName,
                    Type = CBaseStorageVariant_vType_Values.VT_VARIANT,
                },
                new TableColumn()
                {
                    Property = WspConsts.System_ItemFolderNameDisplay,
                    Type = CBaseStorageVariant_vType_Values.VT_VARIANT,
                },
            };

            return columns;
        }

        /// <summary>
        /// Gets ColumnSet structure
        /// </summary>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <returns>ColumnSet structure BLOB</returns>
        public CColumnSet GetColumnSet(int numberOfColumns = 2)
        {
            var result = new CColumnSet();

            // Index of Properties to be queried
            uint[] indexes = new uint[numberOfColumns];

            for (uint i = 0; i < numberOfColumns; i++)
            {
                indexes[i] = i;
            }
            // Links to the 'pidMapper' field

            result.count = (uint)indexes.Length;

            result.indexes = indexes;

            return result;
        }

        /// <summary>
        /// Gets the PidMapper Structure
        /// </summary>
        /// <returns>Pid Mapper structure BLOB</returns>
        private CPidMapper GetPidMapper()
        {
            var result = new CPidMapper();

            result.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_ItemFolderNameDisplay,
                WspConsts.System_Search_Scope,
                WspConsts.System_Search_Contents,
            };

            result.count = (uint)result.aPropSpec.Length;

            return result;
        }

        public CPMGetRowsIn GetCPMGetRowsIn(uint cursor, uint rowsToTransfer, uint rowWidth, uint cbReadBuffer, uint fBwdFetch, uint eType, uint? chapt, IWspSeekDescription seekDescription, out uint reserved)
        {
            reserved = 256;

            var message = new CPMGetRowsIn()
            {
                Header = new WspMessageHeader()
                {
                    _msg = WspMessageHeader_msg_Values.CPMGetRowsIn,
                },

                _hCursor = cursor,

                _cRowsToTransfer = rowsToTransfer,

                _cbRowWidth = rowWidth,

                _cbReserved = reserved,

                _cbReadBuffer = cbReadBuffer,

                _ulClientBase = 0,

                _fBwdFetch = fBwdFetch,

                eType = (CPMGetRowsIn_eType_Values)eType,

                _chapt = chapt ?? chapter,

                SeekDescription = seekDescription ?? GetSeekDescription((CPMGetRowsIn_eType_Values)eType),
            };

            return message;
        }

        /// <summary>
        /// Gets TableColumn structure from given values
        /// </summary>
        /// <param name="column">TableColumn information</param>
        /// <returns>CTableColumn structure.</returns>
        public CTableColumn GetTableColumn(TableColumn column)
        {
            var result = new CTableColumn()
            {
                PropSpec = column.Property,

                vType = (CBaseStorageVariant_vType_Values)column.Type,

                AggregateType = CAggregSpec_type_Values.DBAGGTTYPE_BYNONE,

                ValueSize = Helper.GetSize(column.Type, Is64bit),
            };

            return result;
        }

        /// <summary>
        /// Gets TableColumn structure from given values
        /// </summary>
        /// <param name="column">TableColumn information</param>
        /// <returns>CTableColumn structure.</returns>
        public CTableColumn GetTableColumn(CFullPropSpec property, CBaseStorageVariant_vType_Values type)
        {
            var result = new CTableColumn()
            {
                PropSpec = property,

                vType = type,

                AggregateType = CAggregSpec_type_Values.DBAGGTTYPE_BYNONE,

                ValueSize = Helper.GetSize(type, Is64bit),
            };

            return result;
        }

        /// <summary>
        /// Gets the CreateQueryIn message
        /// </summary>
        /// <param name="path">A null terminated unicode
        /// string representing the scope of search</param>
        /// <param name="queryString">A NON null terminated unicode string representing the query string</param>
        /// <param name="queryStringProperty">The property used by queryString</param>
        /// <param name="ENABLEROWSETEVENTS">flag for ENABLEROWSETEVENTS</param>
        public CPMCreateQueryIn GetCPMCreateQueryIn(string path, string queryString, CFullPropSpec queryStringProperty, bool ENABLEROWSETEVENTS)
        {
            searchScope = path;

            var message = new CPMCreateQueryIn();

            message.ColumnSet = GetColumnSet();

            message.RestrictionArray = GetRestrictionArray(queryString, searchScope, queryStringProperty);

            message.SortSet = null;

            message.CCategorizationSet = null;

            message.RowSetProperties = GetRowSetProperties(ENABLEROWSETEVENTS);

            message.PidMapper = GetPidMapper();

            message.GroupArray = new CColumnGroupArray()
            {
                count = 0,
                aGroupArray = new CColumnGroup[0]
            };

            message.Lcid = Parameter.LcidValue;

            message.Header = new WspMessageHeader
            {
                _msg = WspMessageHeader_msg_Values.CPMCreateQueryIn,
            };

            return message;
        }

        #endregion

        /// <summary>
        /// Gets CPMGetScopeStatisticsIn message BLOB 
        /// </summary>
        /// <returns>CPMGetScopeStatisticsIn Message BLOB</returns>
        public byte[] GetCPMGetScopeStatisticsIn()
        {
            // Add Message Header
            byte[] mainBlob = new byte[Constants.SIZE_OF_UINT];
            return AddMessageHeader(MessageType.CPMGetScopeStatisticsIn, mainBlob);
        }

        #region Utility Methods

        /// <summary>
        /// Adds Header to a Message and also the checksum if required
        /// </summary>
        /// <param name="msgType">Type of message</param>
        /// <param name="messageBlob">Message BLOB</param>
        /// <returns>Message BLOB with Message Header Added</returns>
        private byte[] AddMessageHeader(MessageType msgType, byte[] messageBlob)
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
                = new byte[4 * Constants.SIZE_OF_UINT + messageBlob.Length];
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

        #endregion
    }

    /// <summary>
    /// Represent the Table Column to be queried
    /// </summary>
    public struct TableColumn
    {
        /// <summary>
        /// Guid and property of the Column
        /// </summary>
        public CFullPropSpec Property;

        /// <summary>
        /// Base Storage Type of the Column
        /// </summary>
        public CBaseStorageVariant_vType_Values Type;
    }
}
