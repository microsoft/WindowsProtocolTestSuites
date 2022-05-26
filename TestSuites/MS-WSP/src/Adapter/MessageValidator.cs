// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp.Adapter
{

    /// <summary>
    /// MessageValidator class validates syntax of MS-WSP response messages
    /// </summary>
    public class MessageValidator
    {
        /// <summary>
        /// ITestSite for logging prupose
        /// </summary>
        private ITestSite site;

        /// <summary>
        /// Indicates if we use 64-bit or 32-bit when validating responses.
        /// </summary>
        public bool Is64bit;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="site">ITestSite</param>
        public MessageValidator(ITestSite site)
        {
            this.site = site;
        }

        /// <summary>
        /// Validate CPMConnectOut message
        /// </summary>
        /// <param name="connectOutResponse">ConnectOut</param>
        public void ValidateCPMConnectOutResponse(CPMConnectOut connectOutResponse)
        {
            ValidateHeader(connectOutResponse.Header, WspMessageHeader_msg_Values.CPMConnectOut);

            uint messageStatus = connectOutResponse.Header._status;

            // For any arbitary value of padding fields 
            //of CDbPropSet structure the server response is success
            site.CaptureRequirementIfAreEqual<uint>((uint)WspErrorCode.SUCCESS,
                messageStatus, 264,
                "For any arbitrary value of '_padding' field of the" +
                "  CDbPropSet structure, server is returning success.");
            // For  arbitary value in padding fields between CDbProp 
            // structure the server response is success.Hence validated
            site.CaptureRequirementIfAreEqual<uint>((uint)WspErrorCode.SUCCESS,
                messageStatus, 269,
                "For any arbitrary value of padding bytes in the array" +
                " containing CDbProp structures,server response is success.");
            // For arbitary value in padding fields of '_paddingcbdBlob2' 
            // the server response is success.Hence validated
            site.CaptureRequirementIfAreEqual<uint>((uint)WspErrorCode.SUCCESS,
                messageStatus, 413, "For any arbitrary value of the" +
                " '_paddingcbdBlob2' bytes, server response is success.");
            // For arbitary value in padding fields of '_padding' 
            // the server response is success.Hence validated
            site.CaptureRequirementIfAreEqual<uint>((uint)WspErrorCode.SUCCESS,
                messageStatus, 416,
                "For any arbitrary value  '_padding' field of the" +
                "  CPMConnectIn message, server response is success.");
            // For arbitary value in padding fields of '_paddingcPropSets' 
            // the server response is success.Hence validated
            site.CaptureRequirementIfAreEqual<uint>((uint)WspErrorCode.SUCCESS,
                messageStatus, 422,
                "For any value of '_paddingcPropSets' field of" +
                " the  CPMConnectIn message, server response is success.");
            // For arbitary value in padding fields of 'PaddingExtPropset' 
            // the server response is success.Hence validated
            site.CaptureRequirementIfAreEqual<uint>((uint)WspErrorCode.SUCCESS,
                messageStatus, 430,
                "For any arbitrary value of 'PaddingExtPropset' field" +
                " of the  CPMConnectIn message, server response is success.");

            //For any value of _paddingcbBlob2 field,
            // message status is success
            site.CaptureRequirementIfAreEqual<uint>
                ((uint)WspErrorCode.SUCCESS, messageStatus, 413,
                "For any arbitrary value of the '_paddingcbdBlob2'" +
                " bytes, server response is success.");

            //For any value of _padding field, message status is success
            site.CaptureRequirementIfAreEqual<uint>((uint)WspErrorCode.SUCCESS,
                messageStatus, 416,
                "For any arbitrary value  '_padding' field of the " +
                " CPMConnectIn message, server response is success.");
            //
            //For any value of '_paddingcPropSets' 
            //field, message status is success
            site.CaptureRequirementIfAreEqual<uint>
                ((uint)WspErrorCode.SUCCESS, messageStatus, 422,
                "For any value of '_paddingcPropSets' field of the " +
                " CPMConnectIn message, server response is success.");

            //-----------------------------  Server Version   
            uint obtainedServerVersion = connectOutResponse._serverVersion;
            uint clientVersion
                    = Convert.ToUInt32
                    (site.Properties["ClientVersion"]);

            // Values greater than or equal to 0x00010000 indicate 64-bit support. Values less than 0x00010000 indicate 32-bit support.
            if (obtainedServerVersion >= WspConsts.Is64bitVersion && clientVersion >= WspConsts.Is64bitVersion)
            {
                this.Is64bit = true;
            }
            else
            {
                this.Is64bit = false;
            }

            uint serverOffset = Convert.ToUInt32(site.Properties.Get("ServerOffset"));
            #region Windows Behaviour Validation

            if (serverOffset == Constants.OFFSET_32)
            {
                //Updated by:v-zhil
                //Delta testing
                site.CaptureRequirementIfIsTrue
                    (clientVersion <= 0x00010000, 527,
                    " [Servers MUST format offsets to variable length data types in the row field as]" +
                    "The client indicated that it was a 32-bit system (_iClientVersion less than 0x00010000 " +
                    "in the _iClientVersion field of CPMConnectIn), and the server indicated " +
                    "that it was a 32-bit system (_serverVersion less than 0x00010000 in CPMConnectOut)." +
                    "Offsets are 32-bit integers.");

                //Updated by:v-zhil
                //Delta testing
                site.CaptureRequirementIfIsTrue(obtainedServerVersion < 0x00010000 || clientVersion < 0x00010000,
                    1016,
                    " [Servers MUST format offsets to variable length data types in the row field as]" +
                    "The client indicated that it was a 32-bit system (_iClientVersion less than 0x00010000 " +
                    "in the _iClientVersion field of CPMConnectIn), and the server indicated " +
                    "that it was a 64-bit system (_serverVersion greater than 0x00010000 in CPMConnectOut).Offsets are 32-bit integers.");

                if (site.Properties["IsWDSInstalled"].ToUpper() == "TRUE")
                {
                    site.CaptureRequirementIfIsTrue
                                ((clientVersion >= 0x00010000)
                                || (obtainedServerVersion < 0x00010000), 528,
                            " [Servers MUST format offsets to variable length data types in the row field as]" +
                            "The client indicated that it was a 64-bit system (_iClientVersion greater than 0x00010000 in CPMConnectIn), " +
                            "and the server indicated that it was a 32-bit system (_serverVersion less than 0x00010000 in CPMConnectOut). " +
                            "Offsets are 32-bit integers.");
                }
            }

            if (site.Properties["IsServerWindows"].ToUpper() == "TRUE")
            {
                // if server is handling 32 bit offset

                if (serverOffset == Constants.OFFSET_32)
                {
                    uint actualValue
                        = Convert.ToUInt32
                        (site.Properties["ServerVersion"]);
                    site.CaptureRequirementIfIsTrue
                        (actualValue < 0x00010000, 435,
                        "Values less than 0x00010000, of 4 bytes " +
                        "_serverVersion field of CPMConnectOut message ," +
                        "indicates 32-bit support.");

                    //Windows Behavior
                    // For 32-bit Windows Systems in 
                    //which WDS 4.0 is installed
                    if (site.Properties["IsWDSInstalled"].ToUpper() == "TRUE")
                    {
                        site.CaptureRequirementIfAreEqual<uint>
                            (actualValue, obtainedServerVersion, 939,
                            "<10>OS is either 32-bit Windows XP, 32-bit " +
                            "Windows Server 2003, 32-bit Windows Vista " +
                            "with Windows Desktop Search 4, 32-bit Windows" +
                            " Server 2003 with Windows Desktop Search 4.");

                    }
                    // For 32-bit Windows Systems in 
                    //which WDS 4.0 is not installed
                    if (site.Properties["IsWDSInstalled"].ToUpper() == "TRUE")
                    {
                        site.CaptureRequirementIfAreEqual<uint>
                            (actualValue, obtainedServerVersion,
                            938, "<10>OS is either 32-bit Windows Server" +
                            " 2008, or 32-bit Windows Vista.");
                    }
                }
                // if server is handling 64 bit offset
                if (serverOffset == Constants.OFFSET_64)
                {
                    uint actualValue
                        = Convert.ToUInt32
                        (site.Properties["ServerVersion"]);

                    site.CaptureRequirementIfIsTrue(
                        actualValue >= 0x00010000, 434,
                        " Values  greater than or equal to 0x00010000 ," +
                        "of 4 bytes _serverVersion field of " +
                        "CPMConnectOut message ," +
                        "indicates 64-bit support.");

                    // For 64-bit Windows Systems in
                    //which WDS 4.0 is installed
                    if (site.Properties["IsWDSInstalled"].ToUpper()
                        == "TRUE")
                    {
                        site.CaptureRequirementIfAreEqual<uint>
                            (actualValue,
                            obtainedServerVersion, 941,
                            "<10>64-bit version of Windows Vista or " +
                            "Windows Server 2008 with Windows Desktop " +
                            "Search 4 installed.");

                        //**********************************************************************************
                        // Edited by v-zhil
                        // This requriement should verify under both ServerOffset and ClientOffset equals 64
                        //**********************************************************************************
                        if (Convert.ToUInt32(site.Properties.Get("ClientOffset")) == Constants.OFFSET_64)
                        {
                            site.CaptureRequirementIfIsTrue
                                (clientVersion > 0x00010000, 529,
                                " If the iclientVersion is greater than" +
                                " 0x00010000 and the serverVersion is " +
                                "greater than 0x00010000, Servers MUST format" +
                                " offsets to variable length data types " +
                                "in the row field as 64-bit integers.");
                        }
                    }
                    // For 64-bit Windows Systems in
                    //which WDS 4.0 is not installed
                    if (site.Properties["IsWDSInstalled"].ToUpper() == "FALSE")
                    {
                        site.CaptureRequirementIfAreEqual<uint>
                            (actualValue,
                            obtainedServerVersion,
                            940, "<10>64-bit version of " +
                            "Windows Vista or Windows Server 2008.");

                    }
                }
            }
            #endregion

            //Update by v-aliche
            //Delta testing
            #region For Delta testing.
            //serverVersion is 0x00000102 when OS is either 32-bit Windows Server 2008, or 32-bit Windows Vista.
            if (obtainedServerVersion == 0x00000102 || obtainedServerVersion == 0x00010102)
            {
                var reservered = connectOutResponse._reserved;
            }
            else
            {
                //serverVersion is 0x00000700 when 32-bit Windows 7.
                if (obtainedServerVersion == 0x00000700 || obtainedServerVersion == 0x00010700)
                {
                    // Verify reservered
                    var reservered = connectOutResponse._reserved;
                    site.CaptureRequirementIfAreEqual<int>(4, reservered.Length, 1006,
                        "[CPMConnectOut]If server supports version reporting then this field" +
                        "[_reserved]  the size MUST be 4 bytes.");

                    // Verify WINDOWS_MAJOR_VERSION
                    uint dwWinVerMajor = connectOutResponse.dwWinVerMajor.Value;

                    // Verify WINDOWS_MINOR_VERSION
                    uint dwWinVerMinor = connectOutResponse.dwWinVerMinor.Value;

                    // Verify NLS_VERSION
                    uint dwNLSVerMajor = connectOutResponse.dwNLSVerMajor.Value;
                    uint dwNLSVerMinor = connectOutResponse.dwNLSVerMinor.Value;
                }
            }
            #endregion 
        }

        /// <summary>
        /// Validates CPMCiStateInOut response from the server.
        /// </summary>
        /// <param name="stateInOutResponse">CPMCiStateInOut response.</param>
        public void ValidateCPMCiStateInOutResponse(CPMCiStateInOut stateInOutResponse)
        {
            ValidateHeader(stateInOutResponse.Header, WspMessageHeader_msg_Values.CPMCiStateInOut);

            //--------------------       cbStruct    -------------------
            uint cbStruct = stateInOutResponse.State.cbStruct;
            site.CaptureRequirementIfAreEqual<uint>(60, cbStruct, 372,
                "For 0x00000000 value of the" +
                " '_ulReserved2' field of the message header," +
                "server response is success.");

            //--------------------      cWordList     ---------------
            uint wordList = stateInOutResponse.State.cWordList;
            site.CaptureRequirement(373,
                "The 4 bytes 'cWordList' field  of" +
                " the CPMCiStateInOut message specifies " +
                "the number of in-memory indexes " +
                "created for recently indexed documents.");

            //--------------------    cPersistentIndex   ----------------
            uint persistentIndex = stateInOutResponse.State.cPersistentIndex;
            site.CaptureRequirement(374,
                "The 4 bytes 'cPersistentIndex' field " +
                " of the CPMCiStateInOut message specifies" +
                "the number of persisted indexes.");

            //----------------------      cQueries   ----------------------
            uint queries = stateInOutResponse.State.cQueries;
            site.CaptureRequirement(375,
                "The 4 bytes 'cQueries' field  of the " +
                "CPMCiStateInOut message specifies the " +
                "number of actively running queries.");

            //----------------------     cDocuments   ----------------
            uint documents = stateInOutResponse.State.cDocuments;
            site.CaptureRequirement(376,
                "The 4 bytes 'cDocuments' field  of " +
                "the CPMCiStateInOut message specifies the" +
                "total number of documents waiting to be indexed.");

            //---------------------      cFreshTest   --------------
            uint freshTest = stateInOutResponse.State.cFreshTest;
            site.CaptureRequirement(377,
                "The 4 bytes 'cFreshTest' field  of " +
                "the CPMCiStateInOut message specifies the" +
                "number of unique documents with information " +
                "in indexes that are not fully optimized for performance.");

            //---------------------    dwMergeProgress  ----------------
            uint mergeProcess = stateInOutResponse.State.dwMergeProgress;
            site.CaptureRequirement(378,
                "The 4 bytes 'dwMergeProgress' field  " +
                "of the CPMCiStateInOut message specifies the" +
                "completion percentage of current full " +
                "optimization of indexes while " +
                "optimization is in progress.");
            site.CaptureRequirementIfAreEqual<bool>
                (true, mergeProcess <= 100, 379,
                "The 4 bytes 'dwMergeProgress' field  of the" +
                " CPMCiStateInOut message MUST be less " +
                "than or equal to 100.");

            //--------------------         eState    --------------------
            var eState = stateInOutResponse.State.eState;
            bool eStateValid = IsValidCPMCiState_eState(eState);

            //Updated by:v-zhil
            //For Delta tesing
            site.CaptureRequirementIfAreEqual<bool>(true, eStateValid, 380,
                "The 4 bytes 'eState' field  of the CPMCiStateInOut message MUST be zero " +
                "or one or more  constants defined in the following table:  " +
                "0x00000000,CI_STATE_SHADOW_MERGE(0x00000001),  CI_STATE_MASTER_MERGE(0x00000002),  " +
                "CI_STATE_CONTENT_SCAN_REQUIRED(0x00000004),  CI_STATE_ANNEALING_MERGE(0x00000008),  " +
                "CI_STATE_SCANNING(0x00000010),  CI_STATE_LOW_MEMORY(0x00000080), CI_STATE_HIGH_IO(0x00000100),  " +
                "CI_STATE_MASTER_MERGE_PAUSED(0x00000200), CI_STATE_READ_ONLY(0x00000400), " +
                "CI_STATE_BATTERY_POWER(0x00000800),  CI_STATE_USER_ACTIVE(0x00001000),  " +
                "CI_STATE_LOW_DISK(0x00010000), CI_STATE_HIGH_CPU(0x00020000).");

            //--------------------    cFilteredDocuments  
            uint filteredDocuments = stateInOutResponse.State.cFilteredDocuments;
            site.CaptureRequirement(381,
                "The 4 bytes 'cFilteredDocuments' field" +
                "  of the CPMCiStateInOut message specifies the" +
                "number of documents indexed " +
                "since content indexing was begun.");

            //-------------------      cTotalDocuments    
            uint totalDocuments = stateInOutResponse.State.cTotalDocuments;
            site.CaptureRequirement(382,
                "The 4 bytes 'cTotalDocuments' field  " +
                "of the CPMCiStateInOut message specifies the " +
                "total number of documents in the system.");

            //-------------------      cPendingScans     
            uint pendingScans = stateInOutResponse.State.cPendingScans;
            site.CaptureRequirement(383,
                "The 4 bytes 'cPendingScans' field  of the CPMCiStateInOut" +
                " message specifies the" +
                "number of pending high-level indexing operations.");
            //Updated by:v-zhil
            //For delta testing
            site.CaptureRequirementIfIsTrue(0 == pendingScans, 384,
                "<6> Section 2.2.3.1: This value [cPendingScans] is usually zero, " +
                "except immediately after indexing has been started or after a notification queue overflows.");
            //-------------------        dwIndexSize    
            uint indexSize = stateInOutResponse.State.dwIndexSize;
            site.CaptureRequirement(385,
                "The 4 bytes 'dwIndexSize' field  of the CPMCiStateInOut " +
                "message specifies the size," +
                "in megabytes, of the index (excluding the property cache).");

            //-------------------       cUniqueKeys    
            uint uniqueKeys = stateInOutResponse.State.cUniqueKeys;
            site.CaptureRequirement(386,
                "The 4 bytes 'cUniqueKeys' field  of the " +
                "CPMCiStateInOut message specifies the" +
                "approximate number of unique keys in the catalog.");

            //--------------------     cSecQDocuments   
            uint secQDocuments = stateInOutResponse.State.cSecQDocuments;
            site.CaptureRequirement(387,
                "The 4 bytes 'cSecQDocuments' field  of " +
                "the CPMCiStateInOut message specifies the" +
                "number of documents that the Windows Search" +
                " service will attempt to index again" +
                "because of a failure during the initial indexing attempt.");

            //--------------------      dwPropCacheSize  
            uint propCacheSize = stateInOutResponse.State.dwPropCacheSize;
            site.CaptureRequirement(388,
                "The 4 bytes 'dwPropCacheSize' field  of the " +
                "CPMCiStateInOut message specifies the size," +
                "in megabytes, of the property cache.");
        }

        /// <summary>
        /// Validates the CPMCreateQueryOut Message response
        /// </summary>
        /// <param name="queryOutResponse">CreateQueryOut</param>
        /// <param name="cursor">cursor handle that comes from the server</param>
        public void ValidateCPMCreateQueryOutResponse(CPMCreateQueryOut queryOutResponse, out uint[] cursor)
        {
            ValidateHeader(queryOutResponse.Header, WspMessageHeader_msg_Values.CPMCreateQueryOut);

            uint messageStatus = queryOutResponse.Header._status;

            //----------------------------   _fTrueSequential  
            uint trueSequential = queryOutResponse._fTrueSequential;
            site.CaptureRequirementIfIsTrue
                (((0x00000000 == trueSequential)
                || (0x00000001 == trueSequential)), 476,
                "The 4 bytes '_fTrueSequential' field of the " +
                " CPMCreateQueryOut message MUST be set to one " +
                "of the following values: (0x00000000) or  (0x00000001).");

            //----------------------------   _fWorkIdUnique    
            uint workIdUnique = queryOutResponse._fWorkIdUnique;
            site.CaptureRequirementIfIsTrue
                (((0x00000000 == workIdUnique)
                || (0x000000001 == workIdUnique)), 477,
                "The 4 bytes '_fWorkIdUnique' field of the  " +
                "CPMCreateQueryOut message MUST be set to one " +
                "of the following values: (0x00000000) or (0x00000001).");


            //Code has to be written to pass the length of the array.
            //It can be written once createqueryIn is done.
            //Current implementation do not have categorizationSet
            //So, it cannot be calculated at this point of time.
            cursor = queryOutResponse.aCursors;
            //If numberOfCatSet + 1 Cursors are read from the response.
            // The following requirement gets validated
            site.CaptureRequirement(478, "The 4 bytes 'aCursors '" +
                " field of the  CPMCreateQueryOut message is an" +
                " array of 32-bit unsigned integers  with the number" +
                " of elements equal to the number of categories in the" +
                " CategorizationSet field of CPMCreateQueryIn message" +
                " plus one element.");
        }

        /// <summary>
        /// Validates the CPMGetQueryStatusOut Message response
        /// </summary>
        /// <param name="queryStatusOutResponse">QueryStatusOut BLOB</param>
        /// <param name="queryStatusCheckSum">checksum</param>
        public void ValidateCPMGetQueryStatusOutResponse(byte[] queryStatusOutResponse, uint queryStatusCheckSum)
        {
            int startingIndex = 0;
            ValidateHeader
                (queryStatusOutResponse,
                MessageType.CPMGetQueryStatusOut,
                queryStatusCheckSum, ref startingIndex);
            //----------------------------      _Status     
            uint qstatus
                = Helper.GetUInt(queryStatusOutResponse, ref startingIndex);
            // If the field qStatus is obtained from the Server
            // the following requirements gets validated
            site.CaptureRequirement(677,
                "When the server receives a CPMGetQueryStatusIn message" +
                " request from a client, the server MUST " +
                "retrieve the current" +
                " query status and set it in the _QStatus field.");
            bool tempStatus
                = (qstatus == 0x00000000)
                || (qstatus == 0x00000001)
                || (qstatus == 0x00000002) || (qstatus == 0x00000003);
            site.CaptureRequirementIfIsTrue(tempStatus, 480,
                "The value of the 4 bytes '_QStatus' field of the" +
                " CPMGetQueryStatusOut message which is " +
                "obtained by performing a bitwise AND operation " +
                "on 4 bytes '_Status' field  with 0x00000007  " +
                "MUST be one of the following:" +
                "STAT_NOISE_WORDS(0x00000000),  " +
                "STAT_ERROR(0x00000001),  " +
                "STAT_DONE(0x00000002), STAT_REFRESH(0x00000003).");
        }

        /// <summary>
        /// Validates the CPMGetQueryStatusExOut Message response
        /// </summary>
        /// <param name="queryStatusExOutResponse">QueryStautsExOut
        /// BLOB</param>
        /// <param name="queryStatusExCheckSum">checksum of
        /// QueryStatusExIn message</param>
        public void ValidateCPMGetQueryStatusExOutResponse(byte[] queryStatusExOutResponse, uint queryStatusExCheckSum)
        {
            int startingIndex = 0;
            byte[] tempByteArray;
            ValidateHeader(queryStatusExOutResponse,
                MessageType.CPMGetQueryStatusExOut,
                queryStatusExCheckSum, ref startingIndex);
            //----------------------------      _Status       
            uint qstatus
                = Helper.GetUInt(queryStatusExOutResponse, ref startingIndex);
            bool tempStatus = (qstatus == 0x0000000)
                || (qstatus == 0x00000001)
                || (qstatus == 0x00000002)
                || (qstatus == 0x00000003);
            site.CaptureRequirementIfIsTrue(tempStatus, 484,
                "The value of the 4 bytes '_QStatus' field " +
                "of the CPMGetQueryStatusExOut message" +
                "which is obtained by performing a bitwise" +
                " AND operation on 4 bytes '_Status' field " +
                "with 0x00000007  MUST be one of the" +
                " following:  (0x00000000),  (0x00000001),  (0x00000002)," +
                "(0x00000003).");

            //--------------------------  _cFilteredDocuments  
            uint filteredDocuments
                = Helper.GetUInt(queryStatusExOutResponse, ref startingIndex);
            tempByteArray = BitConverter.GetBytes(filteredDocuments);
            site.CaptureRequirementIfIsTrue
                ((4 == tempByteArray.Length), 486,
                "The 4 bytes '_cFilteredDocuments' field " +
                "of the  CPMGetQueryStatusExOut message specifies" +
                "the number of documents that have been indexed.");

            //--------------------------  _cDocumentsToFilter  
            uint documentsTofilter
                = Helper.GetUInt(queryStatusExOutResponse, ref startingIndex);
            tempByteArray
                = BitConverter.GetBytes(documentsTofilter);
            site.CaptureRequirementIfIsTrue((4 == tempByteArray.Length), 487,
                "The 4 bytes '_cDocumentsToFilter' field of" +
                " the  CPMGetQueryStatusExOut message specifies" +
                "the number of documents that remain to be indexed.");
            // If fields filteredDocuments and documentstoFilter are obtained
            // the following requirements are validated
            site.CaptureRequirement(688, "When the server receives a" +
                " CPMGetQueryStatusExIn message request from a " +
                "client, the server MUST retrieve information " +
                "about the client's catalog and fill in " +
                "_cFilteredDocuments and _cDocumentsToFilter.");
            //--------------------------_dwRatioFinishedDenominator 
            uint ratioFinishedDenominator
                = Helper.GetUInt(queryStatusExOutResponse, ref startingIndex);
            tempByteArray
                = BitConverter.GetBytes(ratioFinishedDenominator);
            site.CaptureRequirementIfIsTrue((4 == tempByteArray.Length), 488,
                "The 4 bytes '_dwRatioFinishedDenominator' " +
                "field of the  CPMGetQueryStatusExOut message specifies" +
                "the denominator of the ratio of documents" +
                " the query has finished processing.");

            //--------------------------  _dwRatioFinishedNumerator 
            uint ratioFinishedNumerator
                = Helper.GetUInt(queryStatusExOutResponse, ref startingIndex);
            tempByteArray = BitConverter.GetBytes(ratioFinishedNumerator);
            site.CaptureRequirementIfIsTrue((4 == tempByteArray.Length), 489,
                "The 4 bytes '_dwRatioFinishedNumerator'" +
                " field of the  CPMGetQueryStatusExOut message specifies" +
                "the numerator of the ratio of documents " +
                "the query has finished processing.");
            // If ratioFinishedDenominator and ratioFinishedNumerator are
            // obtained the following requirements gets validated
            site.CaptureRequirement(685, "When the server receives " +
                "a CPMGetQueryStatusExIn message request from a client, " +
                "the server MUST retrieve the current query status " +
                "and query progress and set QStatus, " +
                "_dwRatioFinishedDenominator and " +
                "_dwRatioFinishedNumerator respectively.");
            //----------------------------     _iRowBmk       
            uint rowBmk
                = Helper.GetUInt(queryStatusExOutResponse, ref startingIndex);
            tempByteArray
                = BitConverter.GetBytes(rowBmk);
            site.CaptureRequirementIfIsTrue((4 == tempByteArray.Length), 490,
                "The 4 bytes '_iRowBmk' field of the  " +
                "CPMGetQueryStatusExOut message specifies" +
                "the approximate position of the " +
                "bookmark in the rowset in terms of rows.");
            // If rowBmk field is obtained from the server. 
            // The following requirement 690 gets validated
            site.CaptureRequirement(690, "When the server " +
                "receives a CPMGetQueryStatusExIn message" +
                " request from a client, the server MUST " +
                "retrieve the position of the bookmark " +
                "indicated by the handle in the _bmk field, " +
                "and fill in the remaining _iRowBmk field" +
                " in the CPMGetQueryStatusExOut message.");
            //----------------------------    _cRowsTotal    
            uint rowsTotal
                = Helper.GetUInt
                (queryStatusExOutResponse, ref startingIndex);
            tempByteArray = BitConverter.GetBytes(rowsTotal);
            site.CaptureRequirementIfIsTrue
                ((4 == tempByteArray.Length), 491,
                "The 4 bytes '_cRowsTotal' " +
                "field of the  CPMGetQueryStatusExOut" +
                " message specifies the" +
                "total number of rows in the rowset.");
            // If the rowsTotal is obtained from the server.
            // This requirement gets validated.
            site.CaptureRequirement(686, "When the " +
                "server receives a CPMGetQueryStatusExIn" +
                " message request from a client, the server" +
                " MUST set the number of rows in " +
                "the query results to _cRowsTotal.");
            //----------------------------     _maxRank      
            uint maxRank
                = Helper.GetUInt(queryStatusExOutResponse, ref startingIndex);
            tempByteArray = BitConverter.GetBytes(maxRank);
            site.CaptureRequirementIfIsTrue((4 == tempByteArray.Length), 492,
                "The 4 bytes '_maxRank' field " +
                "of the  CPMGetQueryStatusExOut" +
                " message specifies the" +
                "maximum rank found in the rowset.");

            //----------------------------   _cResultsFound  
            uint resultsFound
                = Helper.GetUInt(queryStatusExOutResponse, ref startingIndex);
            tempByteArray = BitConverter.GetBytes(resultsFound);
            site.CaptureRequirementIfIsTrue((4 == tempByteArray.Length), 493,
                "The 4 bytes '_cResultsFound' field " +
                "of the  CPMGetQueryStatusExOut " +
                "message specifies the" +
                "number of unique results " +
                "returned in the rowset.");

            //----------------------------      _whereID     
            uint whereID
                = Helper.GetUInt(queryStatusExOutResponse, ref startingIndex);
            tempByteArray = BitConverter.GetBytes(whereID);
            //If whereId field is obtained from the Server.
            // Requirement 942 is validated.
            site.CaptureRequirement(942, "The '_whereID' field " +
                "of the CPMGetQueryStatusExOut " +
                "message is a 4 bytes unsigned integer");
        }

        /// <summary>
        /// Validates SetBingingsInResponse message
        /// </summary>
        /// <param name="response">CPMSetBindingsOut</param>
        public void ValidateCPMSetBindingsInResponse(CPMSetBindingsOut response)
        {
            ValidateHeader(response.Header, WspMessageHeader_msg_Values.CPMSetBindingsIn);

            uint messageStatus = response.Header._status;

            // For any arbitrary value of Padding bytes  
            //in the array of CFullPropSpec structures, 
            //server response is success.
            site.CaptureRequirement(278, "For any arbitrary " +
                "value of Padding bytes  in the array of" +
                " CFullPropSpec structures, server response is success.");
            // For any arbitrary value of Padding bytes  
            //in the array of CFullPropSpec structures, 
            //server response is success.
            site.CaptureRequirement(344, "For any value of " +
                "'_padding1' field of the  CTableColumn " +
                "structure, server response is success.");
            // For any arbitrary value of Padding bytes  
            //in the array of CFullPropSpec structures, 
            //server response is success.
            site.CaptureRequirement(353, "For any value " +
                "of '_padding2' field of the  CTableColumn" +
                " structure, server response is success.");

            // The Response message contains only the message
            //Header with Success Status
            //(Status is validated in ValidateHeader method)
            site.CaptureRequirement(750, "When the server " +
            "receives a CPMSetBindingsIn message request " +
            "from a client, the server MUST respond to" +
                " the client with a message header (only)" +
            " with _msg set to CPMSetBindingsIn, and _status " +
            "set to the results of the specified binding.");

            site.CaptureRequirement(496, "The 4 bytes '_hCursor'" +
            " field of the CPMSetBindingsIn message MUST be absent" +
                "when the message is sent by the server.");

            // The Response message contains only the 
            //message Header with Success Status
            //(Status is validated in ValidateHeader method)
            site.CaptureRequirement(498, "The 4 bytes '_cbRow'" +
            " field of the CPMSetBindingsIn message  MUST be absent" +
                "when the message is sent by the server.");

            // The Response message contains only the message 
            //Header with Success Status
            //(Status is validated in ValidateHeader method)
            site.CaptureRequirement(501, "The 4 bytes '_cbBindingDesc'" +
            " field of the CPMSetBindingsIn message  MUST be absent" +
                "when the message is sent by the server.");

            //For any value of _dummy field, server response is success
            site.CaptureRequirementIfAreEqual<uint>
                (0x00000000, messageStatus, 503,
                "For any value of '_dummy' field of the " +
            "CPMSetBindingsIn message, server response is success.");

            // The Response message contains only the
            //message Header with Success Status
            //(Status is validated in ValidateHeader method)
            site.CaptureRequirement(505, "The 4 bytes '_dummy' " +
            "field of the CPMSetBindingsIn message MUST be absent" +
                "when the message is sent by the server. ");

            // The Response message contains only the message 
            //Header with Success Status
            //(Status is validated in ValidateHeader method)
            site.CaptureRequirement(508, "The 4 bytes 'cColumns'" +
            " field of the CPMSetBindingsIn message MUST be absent" +
                "when the message is sent by the server.");

            // The Response message contains only the message
            //Header with Success Status
            //(Status is validated in ValidateHeader method)
            site.CaptureRequirement(511, "The  'aColumns' field " +
            "of the CPMSetBindingsIn message MUST be absent when the" +
                "message is sent by the server.");
            //For any value of padding in CTableColumn field, server
            //response is success 
            site.CaptureRequirementIfAreEqual<uint>((uint)WspErrorCode.SUCCESS,
                messageStatus, 514, "For any arbitrary" +
            " value of Padding bytes in the array of " +
            "CTableColumn structures, server response is success.");
        }

        /// <summary>
        /// Validates CPMRatioFinishedOut Message response
        /// </summary>
        /// <param name="ratioFinishedResponse">RatioFinishedOut
        /// BLOB</param>
        /// <param name="ratioFinishedCheckSum">checksum of 
        /// RatioFinishedIn message</param>
        /// <param name="messageStatus">status of the 
        /// RatioFinishedOut message</param>
        public void ValidateCPMRatioFinishedOutResponse
            (byte[] ratioFinishedResponse,
            uint ratioFinishedCheckSum, uint messageStatus)
        {
            int startingIndex = 0;

            ValidateHeader
                (ratioFinishedResponse,
                MessageType.CPMRatioFinishedOut,
                ratioFinishedCheckSum, ref startingIndex);

            //For any value of _fQuick field, server response is success
            site.CaptureRequirementIfAreEqual<uint>
                (0x00000000, messageStatus, 545,
                "For 0x00000001 value of '_fQuick' " +
                "field of the CPMRatioFinishedIn" +
                " message, server response is success.");

            //-----------------------   _ulNumerator  ----
            uint ulNumerator
                = Helper.GetUInt(ratioFinishedResponse, ref startingIndex);
            // If GetUInt returns successfully Requirement 547 is validated
            site.CaptureRequirement(547, "The 4 bytes " +
                "'_ulNumerator' field of the " +
                "CPMRatioFinishedOut message specifies" +
                "the numerator of the " +
                "completion ratio in terms of rows.");

            //-----------------------  _ulDenominator ----
            uint ulDenominator
                = Helper.GetUInt(ratioFinishedResponse, ref startingIndex);
            // If GetUInt returns successfully Requirement 548 is validated
            site.CaptureRequirement
                (548, "The 4 bytes '_ulDenominator'" +
                " field of the CPMRatioFinishedOut message specifies" +
                "the denominator of the " +
                "completion ratio in terms of row.");

            //Updated by:v-zhil
            //Delta testing            
            //Windows XP,Windows Vista is client SKU, not in our test scope
            if (site.Properties.Get("ServerOSVersion").ToUpper() == "WIN7" || site.Properties.Get("ServerOSVersion").ToUpper() == "W2K8"
                || site.Properties.Get("ServerOSVersion").ToUpper() == "W2K3")
            {
                //this requirements was added to TD as the fix of TDI 36589, this is a Win7 product bug(514865),
                //this requirement was added to TD as work round.
                //MS-WSP_R10549
                site.CaptureRequirementIfIsTrue(ulDenominator > 0, 10549,
                    "<17>Windows XP, Windows Server 2003, Windows Vista, Windows Server 2008, Windows 7, " +
                    "and Windows Server 2008 R2 can return zero for this field [_ulDenominator] depending, in part, " +
                    "on when the CPMRatioFinishedIn and CPMRatioFinishedOut messages are exchanged.");
            }

            bool actualValue = ulDenominator > 0;
            //MS-WSP_R549
            site.CaptureRequirementIfIsTrue(actualValue, 549,
                string.Format(@"The 4 bytes '_ulDenominator' field of the CPMRatioFinishedOut message SHOULD be greater than zero.<17>", "this requirement is implemented"));

            //-----------------------       _cRows     ----
            uint cRows
                = Helper.GetUInt(ratioFinishedResponse, ref startingIndex);
            // If GetUInt returns successfully Requirement 550 is validated
            site.CaptureRequirement(550, "The 4 bytes '_cRows' field " +
                "of the CPMRatioFinishedOut message specifies" +
                "the total number of rows for the query.");
            // If ulDenominator,ulNumerator and cRows fields are present
            // the following requirement is validated
            site.CaptureRequirement(698, "When the server " +
                "receives a CPMRatioFinishedIn message " +
                "request from a client, the server MUST " +
                "retrieve the client's query status and " +
                "fill in the _ulNumerator, _ulDenominator," +
                " and _cRows fields.");
            //when cRows is obtained, Req 701 is validated
            site.CaptureRequirement(701, "When the server " +
                "receives a CPMRatioFinishedIn message " +
                "request from a client, the server MUST " +
                "update the last reported number of rows " +
                "for this query to the value of _cRows.");

            //-----------------------      _fNewRows   ----
            uint newRows
                = Helper.GetUInt(ratioFinishedResponse, ref startingIndex);
            bool isValidNewRows =
                (newRows == 0x00000000 || newRows == 0x00000001);
            site.CaptureRequirementIfIsTrue(isValidNewRows, 552,
                "The 4 bytes '_fNewRows' field of the " +
                "CPMRatioFinishedOut message MUST NOT be set to" +
                "any values other than " +
                "the following: (0x00000000) or (0x00000001).");
            site.CaptureRequirementIfIsTrue(isValidNewRows, 700,
                "When the server receives a CPMRatioFinishedIn message " +
                "request from a client, the server MUST set " +
                "_fNewRows to 0x00000000 if _cRows is equal" +
                " to the last reported number of rows for" +
                " this query; otherwise set it to 0x00000001.");

            // If GetUInt returns successfully Requirement 551 is validated
            site.CaptureRequirement(551, "The 4 bytes '_fNewRows'" +
                " field of the CPMRatioFinishedOut message" +
            "is a Boolean value indicating " +
                "if there are new rows available.");
        }

        /// <summary>
        /// Validates ValidateRestartPositionIn Response message
        /// </summary>
        /// <param name="restartPositionResponse">ValidateRestartPositionIn
        /// Response BLOB</param>
        /// <param name="restartPositionCheckSum">checksum</param>
        public void ValidateCPMRestartPositionInResponse
            (byte[] restartPositionResponse,
            uint restartPositionCheckSum)
        {
            int startingIndex = 0;
            ValidateHeader(restartPositionResponse,
                MessageType.CPMRestartPositionIn,
                restartPositionCheckSum, ref startingIndex);
        }

        /// <summary>
        /// Validates CPMGetApproximatePositionOut response from the server.
        /// </summary>
        /// <param name="approximatePositionResponse">ApproximatePositionOut 
        /// BLOB</param>
        /// <param name="approximatePositionCheckSum">checksum of
        /// ApproximatePositionIn message</param>
        public void ValidateCPMGetApproximatePositionOutReponse
            (byte[] approximatePositionResponse,
            uint approximatePositionCheckSum)
        {
            int startingIndex = 0;
            ValidateHeader
                (approximatePositionResponse,
                MessageType.CPMGetApproximatePositionOut,
                approximatePositionCheckSum, ref startingIndex);

            //----------------------     _numerator ---
            uint numerator
                = Helper.GetUInt
                (approximatePositionResponse, ref startingIndex);
            // If GetUInt returns successfully Requirement 574 is validated
            site.CaptureRequirement(574, "The 4 bytes '_numerator'" +
                " field of the " +
                "CPMGetApproximatePositionOut message specifies" +
                "the row number of the bookmark in the rowset.");
            site.CaptureRequirementIfIsTrue
                ((0x00000000 == numerator), 575,
                "If there are no rows, '_numerator' field of the " +
                "CPMGetApproximatePositionOut" +
                " message  MUST be set to 0x00000000.");
            //----------------------     _denominator -
            uint denominator
                = Helper.GetUInt
                (approximatePositionResponse, ref startingIndex);
            // If GetUInt returns successfully Requirement 576 is validated
            site.CaptureRequirement(576, "The 4 bytes '_denominator'" +
                " field of the CPMGetApproximatePositionOut " +
                "message specifies" +
                "the number of rows in the rowset.");

        }

        /// <summary>
        /// Validates CPMSendNotifyOut response from the server.
        /// </summary>
        /// <param name="bytes">byte array</param>
        /// <param name="sendNotifyOutCheckSum">checksum
        /// of CPMSendNotify message</param>
        public void ValidateCPMSendNotifyOutResponse(byte[] bytes,
            uint sendNotifyOutCheckSum)
        {
            int startingIndex = 0;
            ValidateHeader(bytes, MessageType.CPMSendNotifyOut,
           sendNotifyOutCheckSum, ref startingIndex);
            //----------------------    _watchNotify --
            uint watchNotify = Helper.GetUInt(bytes, ref startingIndex);

            // If the Server Sends WatchNotify field.Requirement 753 
            // is validated
            site.CaptureRequirement(753, "When the server receives a" +
                "CPMGetNotify message from a client, if there were no changes" +
                "in the query result set since the last CPMSendNotifyOut " +
                "message for this client, or if the query is not currently" +
                "monitored for changes in the results set, the server MUST" +
                "respond with a CPMGetNotify message and start to monitor" +
                "the query for changes in the results set.");
        }

        /// <summary>
        /// Validates CPMCompareBmkOut response from the server.
        /// </summary>
        /// <param name="compareBmkResponse">CompareBmkOut BLOB</param>
        /// <param name="compareBmkCheckSum">checksum of CompareBmkIn 
        /// message</param>
        public void ValidateCPMCompareBmkOutResponse(byte[] compareBmkResponse,
            uint compareBmkCheckSum)
        {
            int startingIndex = 0;
            ValidateHeader(compareBmkResponse, MessageType.CPMCompareBmkOut,
                compareBmkCheckSum, ref startingIndex);
            //---------------------      dwComparison    ----------------------
            uint dwComparison = Helper.GetUInt(compareBmkResponse,
                ref startingIndex);
            bool validDWComparison = (dwComparison == 0x00000000)
                || (dwComparison == 0x00000000)
                || (dwComparison == 0x00000000)
                || (dwComparison == 0x00000000)
                || (dwComparison == 0x00000000);
            site.CaptureRequirementIfIsTrue(validDWComparison, 581,
                "The 4 bytes 'dwComparison' field of the CPMCompareBmkOut" +
                "message MUST be one of the following values:" +
                "DBCOMPARE_LT(0x00000000), DBCOMPARE_EQ(0x00000001), " +
                "DBCOMPARE_GT(0x00000002), DBCOMPARE_NE(0x00000003), " +
                "DBCOMPARE_NOTCOMPARABLE(0x00000004).");
        }

        /// <summary>
        ///  Validates CPMFreeCursorOut response from the server.
        /// </summary>
        /// <param name="freeCursorResponse">FreeCursorOut BLOB</param>
        /// <param name="freeCursorCheckSum">checksum of FreeCursorIn 
        /// message</param>
        public void ValidateCPMFreeCursorOutResponse(byte[] freeCursorResponse,
            uint freeCursorCheckSum)
        {
            int startingIndex = 0;

            ValidateHeader(freeCursorResponse, MessageType.CPMFreeCursorOut,
                freeCursorCheckSum, ref startingIndex);

            var header = new WspMessageHeader();
            Helper.FromBytes(ref header, freeCursorResponse);

            if (header._status != 0)
            {
                // failed message
                return;
            }

            // If the Server Responds to the CPMFreeCursorIn message
            // and ValidateHeader method returns successfully.
            // Requirement 797 is validated.
            site.CaptureRequirement(797,
                "When the server receives a CPMFreeCursorIn message request" +
                "from the client, the server MUST respond with a " +
                "CPMFreeCursorOut message.");
            //--------------------    _cCursorsRemaining    -------------------
            uint cursorsRemaining
                = Helper.GetUInt(freeCursorResponse, ref startingIndex);
            //If GetUInt returns successfully, requirement 590 is validated
            site.CaptureRequirement(590,
                "The 4 bytes '_cCursorsRemaining' field of the " +
                "CPMFreeCursorOut message" +
                "is the number of cursors still in use for the query.");
        }

        /// <summary>
        ///  Validates CPMGetRowsOut message
        /// </summary>
        /// <param name="rowsOutResponse">Rowsout response blob obtained</param>
        public void ValidateCPMGetRowsOutResponse(CPMGetRowsOut rowsOutResponse)
        {
            ValidateHeader(rowsOutResponse.Header, WspMessageHeader_msg_Values.CPMGetRowsOut);
        }

        /// <summary>
        /// Verify the FindIndicesOut message,
        /// this message requests requests the rowset position of the next occurrence of a document identifier.
        /// </summary>
        /// <param name="bytes">byte array</param>
        /// <param name="checkSum">Checksum of the message</param>
        /// <param name="cDepthPrev">value of cDepthPrev</param>
        public void ValidateCPMFindIndicesOutResponse(byte[] bytes, uint checkSum, uint cDepthPrev)
        {
            int startingIndex = 0;
            ValidateHeader(bytes, MessageType.CPMFindIndicesOut, checkSum, ref startingIndex);

            startingIndex = 16;

            uint _cDepthNext = Helper.GetUInt(bytes, ref startingIndex);
            if (cDepthPrev == 0)
            {
                //MS-WSP_R1023
                site.CaptureRequirementIfAreEqual<int>(0, Convert.ToInt32(_cDepthNext), 1023,
                    "[CPMFindIndicesOut]This value [_cDepthNext] MUST be set to zero if no occurrence of document identifier " +
                    "has been found following the position of hierarchical group indices specified by preceding CPMFindIndicesIn message.");

                //MS-WSP_R1109
                site.CaptureRequirementIfAreEqual<int>(0, Convert.ToInt32(_cDepthNext), 1109,
                    "[When the server receives a CPMFindIndicesIn message request from a client]if no such occurrence was found, " +
                    "the server MUST indicate this by setting _cDepthNext to zero.");
            }

            //all message contain message header which contain four field, so it's length is 16
            int messageHeaderLength = 16;
            //get field number
            int valueNumber = (bytes.Length - messageHeaderLength) / 4;

            //when field number is less than 1, it means no row next exist
            if (valueNumber <= 1)
            {
                int lengthOfprgiRowNext = 0;

                //MS-WSP_R1024
                site.CaptureRequirementIfAreEqual<int>(Convert.ToInt32(_cDepthNext), lengthOfprgiRowNext, 1024,
                    "[CPMFindIndicesOut]The size of the array [_prgiRowNext] MUST be equal to _cDepthNext.");
            }
        }

        /// <summary>
        /// Verify the GetRowsetNotifyOut message,
        /// this message requests the next rowset event from the server if available
        /// </summary>
        /// <param name="bytes">byte array</param>
        /// <param name="checkSum">Checksum of the message</param>
        /// <param name="eventType">event type on server</param>
        /// <param name="additionalRowsetEvent">additional rowset event on server</param>
        public void ValidateCPMGetRowsetNotifyOutResponse(byte[] bytes, uint checkSum, int eventType, bool additionalRowsetEvent)
        {
            int startingIndex = 0;
            ValidateHeader(bytes, MessageType.CPMGetRowsetNotifyOut, checkSum, ref startingIndex);

            startingIndex = 16;
            uint wid = Helper.GetUInt(bytes, ref startingIndex);

            byte[] eventValue = Helper.GetData(bytes, ref startingIndex, 1);
            byte moreEvents;
            byte eventTypeValue;
            byte bEventValue = eventValue[0];
            string sEventValue = Convert.ToString(Convert.ToInt32(bEventValue), 2);
            if (sEventValue.Substring(sEventValue.Length - 1) == "0")
            {
                moreEvents = 0;
                eventTypeValue = bEventValue;
            }
            else
            {
                moreEvents = 1;
                eventTypeValue = (byte)(bEventValue >> 1);
            }

            if (eventType == (int)EventType.PROPAGATE_NONE || eventType == (int)EventType.PROPAGATE_ROWSET)
            {
                site.CaptureRequirementIfAreEqual<uint>(0, wid, 1027,
                    "[CPMGetRowsetNotifyOut]This value [wid] MUST be zero if eventType is PROPAGATE_NONE or PROPAGATE_ROWSETEVENT.");

                if (eventType == (int)EventType.PROPAGATE_NONE)
                {
                    //when server responses with eventType is PROPAGATE_NONE(0), 
                    //it indicates that there were no available rowset events waiting on the server.
                    site.CaptureRequirementIfAreEqual<uint>(0, eventTypeValue, 1030,
                        "[CPMGetRowsetNotifyOut]This response [PROPAGATE_NONE 0] indicates that there were no available rowset events waiting on the server.");
                }
            }

            byte[] rowsetItemState = Helper.GetData(bytes, ref startingIndex, 1);
            bool verifyRowsetItemState = false;
            if (eventType == (int)EventType.PROPAGATE_ADD || eventType == (int)EventType.PROPAGATE_DELETE || eventType == (int)EventType.PROPAGATE_MODIFY)
            {
                if (Constants.DBPROP_ENABLEROWSETEVENTS)
                {
                    //if DBPROP_ENABLEROWSETEVENTS is set to true, the server should generate rowset events that are relevant to the associated query,
                    //and when eventType is PROPAGATE_ADD, PROPAGATE_DELETE or PROPAGATE_MODIFY, this requirement should be covered
                    bool generateRowsetEvent = eventType != (int)EventType.PROPAGATE_NONE && eventType != (int)EventType.PROPAGATE_ROWSET && Constants.DBPROP_ENABLEROWSETEVENTS;
                    site.CaptureRequirementIfIsTrue(generateRowsetEvent, 1002,
                        "[Database Properties]If TRUE [property DBPROP_ENABLEROWSETEVENTS is set], this indicates that the server should generate rowset events that are relevant to the associated query.");
                }


                switch (Convert.ToInt32(rowsetItemState[0]))
                {
                    case (int)RowsetItemState.ROWSETEVENT_ITEMSTATE_NOTINROWSET:
                    case (int)RowsetItemState.ROWSETEVENT_ITEMSTATE_INROWSET:
                    case (int)RowsetItemState.ROWSETEVENT_ITEMSTATE_UNKNOWN:
                        verifyRowsetItemState = true;
                        break;
                    default:
                        break;
                }

                if (eventType == (int)EventType.PROPAGATE_ADD)
                {
                    //when server responses with eventType is PROPAGATE_ADD(1), 
                    //it indicates that an item was added to the index that may be of interest to the query originating the rowset.
                    site.CaptureRequirementIfAreEqual<uint>((uint)EventType.PROPAGATE_ADD, eventTypeValue, 1031,
                        "[CPMGetRowsetNotifyOut]This response [PROPAGATE_ADD 1] indicates that an item was added to the index that may be of interest to the query originating the rowset.");

                    site.CaptureRequirementIfIsTrue(verifyRowsetItemState, 201035,
                        "[CPMGetRowsetNotifyOut][rowsetItemState]An 8 bit unsigned integer that MUST be one of the following values " +
                        "[ROWSETEVENT_ITEMSTATE_NOTINROWSET(0),ROWSETEVENT_ITEMSTATE_INROWSET(1),ROWSETEVENT_ITEMSTATE_UNKNOWN(2)] " +
                        "if eventType is PROPAGATE_ADD.");
                }

                if (eventType == (int)EventType.PROPAGATE_DELETE)
                {
                    site.CaptureRequirementIfIsTrue(verifyRowsetItemState, 301035,
                        "[CPMGetRowsetNotifyOut][rowsetItemState]An 8 bit unsigned integer that MUST be one of the following values " +
                        "[ROWSETEVENT_ITEMSTATE_NOTINROWSET(0),ROWSETEVENT_ITEMSTATE_INROWSET(1),ROWSETEVENT_ITEMSTATE_UNKNOWN(2)] if eventType is PROPAGATE_DELETE.");


                    //when server responses with eventType is PROPAGATE_DELETE(2), 
                    //it indicates that an item was deleted from the index that may be of interest to the query originating the rowset.
                    site.CaptureRequirementIfAreEqual<uint>((uint)EventType.PROPAGATE_DELETE, eventTypeValue, 1032,
                        "[CPMGetRowsetNotifyOut]This response [PROPAGATE_DELETE 2] indicates that an item was deleted from the index that may be of interest to the query originating the rowset.");

                }

                if (eventType == (int)EventType.PROPAGATE_MODIFY)
                {
                    site.CaptureRequirementIfIsTrue(verifyRowsetItemState, 401035,
                        "[CPMGetRowsetNotifyOut][rowsetItemState]An 8 bit unsigned integer that MUST be one of the following values [ROWSETEVENT_ITEMSTATE_NOTINROWSET(0)," +
                        "ROWSETEVENT_ITEMSTATE_INROWSET(1),ROWSETEVENT_ITEMSTATE_UNKNOWN(2)] if eventType is  PROPAGATE_MODIFY.");
                }
            }

            if (eventType == (int)EventType.PROPAGATE_ADD)
            {
                if (Constants.DBPROP_ENABLEROWSETEVENTS)
                {
                    //if eventType is PROPAGATE_ADD, the document identifier specified by wid, should be the new rowset,
                    //while not in the original rowset, so RowsetItemState should be set to ROWSETEVENT_ITEMSTATE_NOTINROWSET.
                    site.CaptureRequirementIfAreEqual<int>((int)RowsetItemState.ROWSETEVENT_ITEMSTATE_NOTINROWSET, Convert.ToInt32(rowsetItemState[0]), 1036,
                    "[CPMGetRowsetNotifyOut]The document identifier [ROWSETEVENT_ITEMSTATE_NOTINROWSET] specified by wid " +
                    "MUST not have been contained within the originating rowset.");

                    //if eventType is PROPAGATE_ADD and DBPROP_ENABLEROWSETEVENTS value set as true, wid should be set as larger than 0
                    site.CaptureRequirementIfIsTrue(wid > 0, 1120,
                        "[When the server receives a CPMGetRowsetNotifyIn message request from a client, " +
                        "the eventType is set to ROPAGATE_ADD]wid MUST be set to the document identifier for the newly added document.");

                }
            }

            byte[] changedItemState = Helper.GetData(bytes, ref startingIndex, 1);
            bool verifyChangedItemState = false;

            if (eventType == (int)EventType.PROPAGATE_MODIFY)
            {

                switch (Convert.ToInt32(changedItemState[0]))
                {
                    case (int)RowsetItemState.ROWSETEVENT_ITEMSTATE_NOTINROWSET:
                    case (int)RowsetItemState.ROWSETEVENT_ITEMSTATE_INROWSET:
                    case (int)RowsetItemState.ROWSETEVENT_ITEMSTATE_UNKNOWN:
                        verifyChangedItemState = true;
                        break;
                    default:
                        break;
                }
                site.CaptureRequirementIfIsTrue(verifyChangedItemState, 1038,
                        "[CPMGetRowsetNotifyOut][changedItemState]An 8 bit unsigned integer that " +
                        "MUST be one of the following values [ROWSETEVENT_ITEMSTATE_NOTINROWSET(0),ROWSETEVENT_ITEMSTATE_INROWSET(1)," +
                        "ROWSETEVENT_ITEMSTATE_UNKNOWN(2)] if eventType is PROPAGATE_MODIFY.");
            }
            else
            {
                site.CaptureRequirementIfAreEqual<int>((int)RowsetItemState.ROWSETEVENT_ITEMSTATE_NOTINROWSET, Convert.ToInt32(changedItemState[0]), 1039,
                        "[CPMGetRowsetNotifyOut]For other eventTypes [eventType is not PROPAGATE_MODIFY] this value [changedItemState] must be set to zero.");
            }

            if (Convert.ToInt32(changedItemState[0]) == (int)RowsetItemState.ROWSETEVENT_ITEMSTATE_NOTINROWSET)
            {
                //when changedItemState is ROWSETEVENT_ITEMSTATE_NOTINROWSET, 
                //it should not contain within the originating rowset
                site.CaptureRequirement(1040,
                    "[CPMGetRowsetNotifyOut]The document identifier [ROWSETEVENT_ITEMSTATE_NOTINROWSET] specified by wid would NOT be contained within a subsequent query.");
            }

            byte[] rowsetEvent = Helper.GetData(bytes, ref startingIndex, 1);
            byte[] rowsetEventData1 = Helper.GetData(bytes, ref startingIndex, 8);
            byte[] rowsetEventData2 = Helper.GetData(bytes, ref startingIndex, 8);

            bool verifyRowsetEvent = false;
            if (eventType == (int)EventType.PROPAGATE_ROWSET)
            {
                switch (Convert.ToInt32(rowsetEvent[0]))
                {
                    case (int)RowsetEventType.ROWSETEVENT_TYPE_DATAEXPIRED:
                    case (int)RowsetEventType.ROWSETEVENT_TYPE_FOREGROUNDLOST:
                    case (int)RowsetEventType.ROWSETEVENT_TYPE_SCOPESTATISTICS:
                        verifyRowsetEvent = true;
                        break;
                    default:
                        break;
                }
                site.CaptureRequirementIfIsTrue(verifyRowsetEvent, 1042,
                    "[CPMGetRowsetNotifyOut]An 8 bit unsigned integer that MUST be one of the following values " +
                    "[ROWSETEVENT_TYPE_DATAEXPIRED(0),ROWSETEVENT_TYPE_FOREGROUNDLOST(1),ROWSETEVENT_TYPE_SCOPESTATISTICS(2)] " +
                    "if eventType is PROPAGATE_ROWSET.");

            }
            else
            {
                site.CaptureRequirementIfAreEqual<int>((int)RowsetEventType.ROWSETEVENT_TYPE_DATAEXPIRED, Convert.ToInt32(rowsetEvent[0]), 1043,
                    "[CPMGetRowsetNotifyOut][ROWSETEVENT_TYPE_DATAEXPIRED 0]For other eventTypes [eventType is not PROPAGATE_ROWSET] this value [rowsetEvent]  must be set to zero.");
            }

            if (Constants.EventFrequency == 0)
            {
                site.CaptureRequirementIfAreNotEqual<int>((int)RowsetEventType.ROWSETEVENT_TYPE_SCOPESTATISTICS, Convert.ToInt32(rowsetEvent[0]), 1047,
                    "[CPMSetScopePrioritizationIn]When eventFrequency is set to zero the server MUST NOT issue the ROWSETEVENT_TYPE_SCOPESTATISTICS events.");
            }

            if (wid == 0)
            {
                site.CaptureRequirementIfIsTrue((Convert.ToInt32(moreEvents) == 0) && (Convert.ToInt32(eventTypeValue) == (int)EventType.PROPAGATE_NONE)
                    && (Convert.ToInt32(rowsetItemState[0]) == (int)RowsetItemState.ROWSETEVENT_ITEMSTATE_NOTINROWSET) && (Convert.ToInt32(changedItemState[0]) == (int)RowsetItemState.ROWSETEVENT_ITEMSTATE_NOTINROWSET)
                    && (Convert.ToInt32(rowsetEvent[0]) == (int)RowsetEventType.ROWSETEVENT_TYPE_DATAEXPIRED) && (Convert.ToInt32(rowsetEventData1[0]) == 0)
                    && (Convert.ToInt32(rowsetEventData2[0]) == 0), 1116,
                    "[When the server receives a CPMGetRowsetNotifyIn message request from a client]All fields MUST be zero when not contributing to the value specified by eventType.");

                site.CaptureRequirementIfIsTrue(moreEvents == 0 && eventTypeValue == (int)EventType.PROPAGATE_NONE, 1117,
                        "[When the server receives a CPMGetRowsetNotifyIn message request from a client, " +
                        "the following fields MUST be set based upon the following event types]PROPAGATE_NONEeventType " +
                        "MUST be set to this value to indicate that there are no events available.");

            }

            if (site.Properties["IsServerWindows"].ToUpper() == "TRUE")
            {
                if (eventType != (int)EventType.PROPAGATE_NONE && eventType != (int)EventType.PROPAGATE_ROWSET && additionalRowsetEvent && Constants.DBPROP_ENABLEROWSETEVENTS == true)
                {
                    //MS-WSP_R201122
                    site.CaptureRequirementIfIsTrue(Convert.ToInt32(moreEvents) == 1, 201122,
                            "[When the server receives a CPMGetRowsetNotifyIn message request from a client, the eventType is set to ROPAGATE_ADD]moreEvents is set to 0 to indicate " +
                            "that there is not another event of any non-PROPAGATE_NONE type immediatly available in windows.");
                }
                else
                {
                    //MS-WSP_R101122
                    site.CaptureRequirementIfIsTrue(Convert.ToInt32(moreEvents) == 0, 101122,
                        "[When the server receives a CPMGetRowsetNotifyIn message request from a client, the eventType is set to ROPAGATE_ADD]moreEvents is set to 1 to indicate " +
                        "that there is another event of any non-PROPAGATE_NONE type immediatly available in windows.");
                }
            }

            bool actualValue = Convert.ToInt32(moreEvents) == 1 || Convert.ToInt32(moreEvents) == 0;

            //MS-WSP_R1122
            site.CaptureRequirementIfIsTrue(actualValue, 1122,
                    string.Format(@"[When the server receives a CPMGetRowsetNotifyIn message request from a client, " +
                    "the eventType is set to ROPAGATE_ADD]moreEvents SHOULD be set to indicate " +
                    "if there is another event of any non-PROPAGATE_NONE type immediatly available.", "this requirement is implemented"));

            bool rowsetItemStateChecked = Convert.ToInt32(rowsetItemState[0]) == (int)RowsetItemState.ROWSETEVENT_ITEMSTATE_NOTINROWSET || Convert.ToInt32(rowsetItemState[0]) == (int)RowsetItemState.ROWSETEVENT_ITEMSTATE_INROWSET
                    || Convert.ToInt32(rowsetItemState[0]) == (int)RowsetItemState.ROWSETEVENT_ITEMSTATE_UNKNOWN;

            if (eventType == (int)EventType.PROPAGATE_DELETE)
            {
                if (Constants.DBPROP_ENABLEROWSETEVENTS == true)
                {
                    if (site.Properties["IsServerWindows"].ToUpper() == "TRUE")
                    {
                        if (additionalRowsetEvent)
                        {
                            //MS-WSP_R101127
                            site.CaptureRequirementIfAreEqual<int>(1, Convert.ToInt32(moreEvents), 101127,
                                "[When the server receives a CPMGetRowsetNotifyIn message request from a client, " +
                                "the eventType is set to PROPAGATE_DELETE]moreEvents is set to 1 to indicate " +
                                "that there is another event of any non_PROPAGATE_NONE type immediately available in windows.");
                        }
                        else
                        {
                            site.CaptureRequirementIfAreEqual<int>(0, Convert.ToInt32(moreEvents), 201127,
                                "[When the server receives a CPMGetRowsetNotifyIn message request from a client, " +
                                "the eventType is set to PROPAGATE_DELETE]moreEvents is set to 0 to indicate " +
                                "that there is not any event of any non_PROPAGATE_NONE type immediately available in windows.");
                        }
                    }

                    actualValue = Convert.ToInt32(moreEvents) == 1 || Convert.ToInt32(moreEvents) == 0;

                    //MS-WSP_R1127
                    site.CaptureRequirementIfIsTrue(actualValue, 1127,
                        string.Format(@"[When the server receives a CPMGetRowsetNotifyIn message request from a client, " +
                        "the eventType is set to PROPAGATE_DELETE]moreEvents SHOULD be set to indicate " +
                        "if there is another event of any non_PROPAGATE_NONE type immediately available.", "this requirement is implemented"));

                }

                //rowsetItemState must be ROWSETEVENT_ITEMSTATE_NOTINROWSET, ROWSETEVENT_ITEMSTATE_INROWSET or ROWSETEVENT_ITEMSTATE_UNKNOWN
                site.CaptureRequirementIfIsTrue(rowsetItemStateChecked, 1125, "[When the server receives a CPMGetRowsetNotifyIn message request from a client, the eventType is set to PROPAGATE_DELETE]" +
                    "rowsetItemState MUST be set to indicate if the document identifier is contained within the rowset");

                //rowsetItemState must be ROWSETEVENT_ITEMSTATE_NOTINROWSET, ROWSETEVENT_ITEMSTATE_INROWSET or ROWSETEVENT_ITEMSTATE_UNKNOWN
                site.CaptureRequirementIfIsTrue(rowsetItemStateChecked, 1126, "[When the server receives a CPMGetRowsetNotifyIn message request from a client, the eventType is set to PROPAGATE_DELETE]rowsetItemState  MUST be set to indicate that this is unknown");


            }
            if (eventType == (int)EventType.PROPAGATE_MODIFY)
            {
                if (Constants.DBPROP_ENABLEROWSETEVENTS)
                {
                    //when server responses with eventType is PROPAGATE_MODIFY(3), 
                    //it indicates that an item was re-indexed that may be of interest to the query originating the rowset.
                    site.CaptureRequirementIfAreEqual<uint>((uint)EventType.PROPAGATE_MODIFY, eventTypeValue, 1033,
                        "[CPMGetRowsetNotifyOut]This response [PROPAGATE_MODIFY 3] indicates that an item was re-indexed that may be of interest to the query originating the rowset.");

                    if (site.Properties["IsServerWindows"].ToUpper() == "TRUE")
                    {
                        if (additionalRowsetEvent)
                        {
                            site.CaptureRequirementIfAreEqual<int>(1, Convert.ToInt32(moreEvents), 101134,
                                "[When the server receives a CPMGetRowsetNotifyIn message request from a client, " +
                                "the eventType is set to PROPAGATE_MODIFY]moreEvents is set to 1 to indicate " +
                                "that there is another event of any non PROPAGATE_NONE type immediately available in windows.");
                        }
                        else
                        {
                            site.CaptureRequirementIfAreEqual<int>(0, Convert.ToInt32(moreEvents), 201134,
                                "[When the server receives a CPMGetRowsetNotifyIn message request from a client, " +
                                "the eventType is set to PROPAGATE_MODIFY]moreEvents is set to 0 to indicate " +
                                "that there is not any event of any non PROPAGATE_NONE type immediately available in windows.");
                        }
                    }

                    actualValue = Convert.ToInt32(moreEvents) == 1 || Convert.ToInt32(moreEvents) == 0;

                    //MS-WSP_R1134
                    site.CaptureRequirementIfIsTrue(actualValue, 1134,
                        string.Format(@"[When the server receives a CPMGetRowsetNotifyIn message request from a client, " +
                        "the eventType is set to PROPAGATE_MODIFY]moreEvents SHOULD be set to indicate " +
                        "if there is another event of any non PROPAGATE_NONE type immediately available.", "this requirement is implemented"));

                    //MS-WSP_R1129
                    site.CaptureRequirementIfIsTrue(wid > 0, 1129,
                        "[When the server receives a CPMGetRowsetNotifyIn message request from a client, " +
                        "the eventType is set to PROPAGATE_MODIFY]wid MUST be set to the document identifier for the re-indexed document.");


                    //rowsetItemState must be ROWSETEVENT_ITEMSTATE_NOTINROWSET, ROWSETEVENT_ITEMSTATE_INROWSET or ROWSETEVENT_ITEMSTATE_UNKNOWN
                    site.CaptureRequirementIfIsTrue(rowsetItemStateChecked, 1130, "[When the server receives a CPMGetRowsetNotifyIn message request from a client, the eventType is set to PROPAGATE_MODIFY]" +
                        "rowsetItemState MUST be set to indicate if the document identifier is contained within the rowset");

                    //rowsetItemState must be ROWSETEVENT_ITEMSTATE_NOTINROWSET, ROWSETEVENT_ITEMSTATE_INROWSET or ROWSETEVENT_ITEMSTATE_UNKNOWN
                    site.CaptureRequirementIfIsTrue(rowsetItemStateChecked, 1131, "[When the server receives a CPMGetRowsetNotifyIn message request from a client, the eventType is set to PROPAGATE_MODIFY]" +
                        "rowsetItemState must be set to indicate that this is unknown.");

                    bool changedItemStateChecked = Convert.ToInt32(changedItemState[0]) == (int)RowsetItemState.ROWSETEVENT_ITEMSTATE_NOTINROWSET
                        || Convert.ToInt32(changedItemState[0]) == (int)RowsetItemState.ROWSETEVENT_ITEMSTATE_INROWSET
                        || Convert.ToInt32(changedItemState[0]) == (int)RowsetItemState.ROWSETEVENT_ITEMSTATE_UNKNOWN;

                    //changedItemState must be set to ROWSETEVENT_ITEMSTATE_NOTINROWSET or ROWSETEVENT_ITEMSTATE_INROWSET, or ROWSETEVENT_ITEMSTATE_UNKNOWN
                    site.CaptureRequirementIfIsTrue(changedItemStateChecked, 1132,
                            "[When the server receives a CPMGetRowsetNotifyIn message request from a client, " +
                            "the eventType is set to PROPAGATE_MODIFY]changedItemState must be set to indicate if the item would be included in the associated query if the query were executed again.");

                    //changedItemState must be set to ROWSETEVENT_ITEMSTATE_NOTINROWSET or ROWSETEVENT_ITEMSTATE_INROWSET, or ROWSETEVENT_ITEMSTATE_UNKNOWN
                    site.CaptureRequirementIfIsTrue(changedItemStateChecked, 1133,
                            "[When the server receives a CPMGetRowsetNotifyIn message request from a client, " +
                            "the eventType is set to PROPAGATE_MODIFY]changedItemState must be set to indicate that this is unknown.");

                }
            }

            if (eventType == (int)EventType.PROPAGATE_ROWSET)
            {
                if (Constants.DBPROP_ENABLEROWSETEVENTS == true)
                {
                    if (site.Properties["IsServerWindows"].ToUpper() == "TRUE")
                    {
                        if (Convert.ToInt32(rowsetEvent[0]) == (int)RowsetEventType.ROWSETEVENT_TYPE_DATAEXPIRED)
                        {
                            //if rowsetEvent is ROWSETEVENT_TYPE_DATAEXPIRED,it indicates the type [ROWSETEVENT_TYPE_DATAEXPIRED] of rowset event.
                            site.CaptureRequirement(101137,
                                "[When the server receives a CPMGetRowsetNotifyIn message request from a client, the eventType is set to PROPAGATE_ROWSET]" +
                                "rowsetEvent is set to 0  value indicating the type [ROWSETEVENT_TYPE_DATAEXPIRED] of rowset event in windows.");

                        }
                        else if (Convert.ToInt32(rowsetEvent[0]) == (int)RowsetEventType.ROWSETEVENT_TYPE_FOREGROUNDLOST)
                        {
                            //if rowsetEvent is ROWSETEVENT_TYPE_FOREGROUNDLOST,it indicates the type [ROWSETEVENT_TYPE_FOREGROUNDLOST] of rowset event.
                            site.CaptureRequirement(201137,
                                "[When the server receives a CPMGetRowsetNotifyIn message request from a client, " +
                                "the eventType is set to PROPAGATE_ROWSET]rowsetEvent is set to 1  value indicating the type [ROWSETEVENT_TYPE_FOREGROUNDLOST] of rowset event in windows.");
                        }
                        else
                        {
                            //if rowsetEvent is ROWSETEVENT_TYPE_SCOPESTATISTICS,it indicates the type [ROWSETEVENT_TYPE_SCOPESTATISTICS] of rowset event.
                            site.CaptureRequirement(301137,
                                "[When the server receives a CPMGetRowsetNotifyIn message request from a client, " +
                                "the eventType is set to PROPAGATE_ROWSET]rowsetEvent is set to 2  value indicating the type [ROWSETEVENT_TYPE_SCOPESTATISTICS] of rowset event in windows.");
                        }
                    }

                    actualValue = Convert.ToInt32(rowsetEvent[0]) == (int)RowsetEventType.ROWSETEVENT_TYPE_DATAEXPIRED
                        || Convert.ToInt32(rowsetEvent[0]) == (int)RowsetEventType.ROWSETEVENT_TYPE_FOREGROUNDLOST
                        || Convert.ToInt32(rowsetEvent[0]) == (int)RowsetEventType.ROWSETEVENT_TYPE_SCOPESTATISTICS;

                    //MS-WSP_R1137
                    site.CaptureRequirementIfIsTrue(actualValue,
                        1137,
                        string.Format(@"[When the server receives a CPMGetRowsetNotifyIn message request from a client, the eventType is set to PROPAGATE_ROWSET]" +
                        "rowsetEvent SHOULD be set to a value indicating the type of rowset event.", "this requirement is implemented"));
                }
            }
        }

        /// <summary>
        /// Method for verify the SetScopePrioritizationOut massage
        /// this message requests that the server prioritize indexing of items that may be relevant to the originating query at a rate specified in the message.
        /// </summary>
        /// <param name="bytes">byte array</param>
        /// <param name="checkSum">Checksum of the message</param>
        /// <param name="eventFrequency"></param>
        public void ValidateCPMSetScopePrioritizationOutResponse(byte[] bytes, uint checkSum, uint eventFrequency)
        {
            int startingIndex = 0;

            ValidateHeader(bytes, MessageType.CPMSetScopePrioritizationOut, checkSum, ref startingIndex);

            //MS-WSP_R1049
            //message header's length equals 16,so it must be 16 if the message not include a body
            site.CaptureRequirementIfAreEqual<int>(16, bytes.Length, 1049,
                "[CPMSetScopePrioritizationOut]The message MUST NOT include a body; only the message header.");
        }

        /// <summary>
        /// Method for verify the GetScopeStatisticsOut massage,
        /// this message request statistics regarding the number of indexed items, the number of items needing to be indexed 
        /// and the number of items needing to be re-indexed that are relevant to the originating query.
        /// </summary>
        /// <param name="bytes">response of GetScopeStatustucsOut</param>
        /// <param name="checkSum">Checksum of the message</param>
        /// <param name="msgStatus">return value for GetScopeStatisticsOut</param>
        public void ValidateCPMGetScopeStatisticsOutResponse(byte[] bytes,
            uint checkSum, uint msgStatus)
        {
            if (site.Properties["IsServerWindows"].ToUpper() == "TRUE")
            {
                if (Constants.DBPROP_ENABLEROWSETEVENTS && msgStatus == 0x00000000)
                {
                    //MS-WSP_R101157
                    site.CaptureRequirementIfAreEqual<uint>(0x00000000, msgStatus,
                            101157,
                            "[When the server receives a CPMGetScopeStatisticsIn message request from a client, the server MUST]" +
                            "Note that the server returns zero for all statistics if the client does not have explicitly requested " +
                            "their availability by setting the DBPROP_ENABLEROWSETEVENTS property to TRUE in windows.");
                }
            }

            bool actualValue = msgStatus == 0x00000000 || msgStatus != 0x00000000;
            //MS-WSP_R1157
            site.CaptureRequirementIfIsTrue(actualValue, 1157,
                string.Format(@"[When the server receives a CPMGetScopeStatisticsIn message request from a client, the server MUST]" +
                "Note that the server SHOULD return zero for all statistics unless the client has explicitly requested " +
                "their availability by setting the DBPROP_ENABLEROWSETEVENTS property to TRUE.", "this requirement is implemented"));

            byte[] tempByteArray;
            int startingIndex = 0;
            //Verify the massage header.
            ValidateHeader(bytes,
                MessageType.CPMGetScopeStatisticsOut, checkSum,
                ref startingIndex);

            //Get the 4 bytes data for the dwIndexedItems
            uint dwIndexedItems
                = Helper.GetUInt(bytes, ref startingIndex);
            tempByteArray
                = BitConverter.GetBytes(dwIndexedItems);
            site.CaptureRequirementIfAreEqual<int>(4, tempByteArray.Length, 10050, "[CPMGetScopeStatisticsOut]dwOutstandingAdds (4 bytes):" +
                 "A 32-bit unsigned integer containing the number of items" +
                 "that have yet to be indexed that may be relevant to the originating query.");

            site.CaptureRequirementIfIsTrue(tempByteArray.Length > 0, 1154, "[When the server receives a CPMGetScopeStatisticsIn message" +
                "request from a client, The CPMGetScopeStatisticsOut message " +
                "MUST be populated with the appropriate statistics. ]" +
                "dwIndexedItems indicates the number of indexed items" +
                "that are relevant for the associated query.");

            //Get the 4 bytes data for the dwOutstandingAdds
            uint dwOutstandingAdds
                = Helper.GetUInt(bytes, ref startingIndex);
            tempByteArray
                = BitConverter.GetBytes(dwOutstandingAdds);

            site.CaptureRequirementIfAreEqual<int>(4, tempByteArray.Length, 10051, "[CPMGetScopeStatisticsOut]dwOutstandingAdds (4 bytes):" +
                " AccessViolationException 32-BitConverter unsigned integer " +
                "containing the number of items that have yet ToString be indexed" +
                "that may be relevant to the originating query.");

            site.CaptureRequirementIfAreEqual<int>(4, tempByteArray.Length, 1155,
                "[When the server receives a CPMGetScopeStatisticsIn message request from a client, " +
                "The CPMGetScopeStatisticsOut message MUST be populated with the appropriate statistics]" +
                "dwOutstandingAdds indicates the number of non-indexed items ready for indexing that are relevant for the associated query.");

            //Get the 4 bytes data for the dwOutstandingAdds
            uint dwOutstandingModifies
                = Helper.GetUInt(bytes, ref startingIndex);
            tempByteArray
                = BitConverter.GetBytes(dwOutstandingModifies);

            site.CaptureRequirementIfAreEqual<int>(4, tempByteArray.Length, 10052,
                "[CPMGetScopeStatisticsOut]dwOustandingModifies (4 bytes):" +
                " A 32-bit unsigned integer containing the number of items" +
                " that need to be re-indexed that are relevant to the originating query.");

            site.CaptureRequirementIfAreEqual<int>(4, tempByteArray.Length, 1156,
                "[When the server receives a CPMGetScopeStatisticsIn message request from a client," +
                " The CPMGetScopeStatisticsOut message MUST be populated with the appropriate statistics.]" +
                "dwOutstandingModifies indicates the number of indexed items that need to be" +
                "re-indexed that are relevant for the associated query.");

        }

        /// <summary>
        /// Validates CPMFetchValueOut response from the server
        /// </summary>
        /// <param name="bytes">byte array</param>
        /// <param name="cbChunk">cbchunk obtained </param>
        /// <param name="cbSoFar">cbsofar parameter obtained</param>
        /// <param name="checkSum">Checksum of the message</param>
        public void ValidateCPMFetchValueOutResponse(byte[] bytes,
            uint checkSum, uint cbSoFar, uint cbChunk)
        {

            int startingIndex = 0;
            ValidateHeader(bytes, MessageType.CPMFetchValueOut,
                checkSum, ref startingIndex);
            //For any arbitrary value of the 'paddingPropSet' field,
            //server is returning success.
            site.CaptureRequirement(741,
                "When the server receives a CPMFetchValueIn message request" +
                "from a client, the server MUST respond to the client with" +
                "the CPMFetchValueOut message.");

            site.CaptureRequirement(67, "For any arbitrary value of the" +
                " 'paddingPropSet' field, server is returning success.");
            // For any arbitrary value of Padding bytes  in the array of
            //CFullPropSpec structures, server response is success.
            site.CaptureRequirement(278, "For any arbitrary " +
                "value of Padding bytes  in the array of" +
                " CFullPropSpec structures, server response is success.");

            // For any arbitrary value _padding field, server response 
            //is success.
            site.CaptureRequirement(562, "For any value of '_padding' field" +
                "of the CPMFetchValueIn message, server response is " +
                "successful.");

            //---------------------    _cbValue    ----
            uint cbValue = Helper.GetUInt(bytes, ref startingIndex);
            // If GetUInt returns successfully Requirement 563 is validated
            site.CaptureRequirement(563, "The 4 bytes '_cbValue' field of the" +
                "CPMFetchValueOut message is the total size, in " +
                "bytes in vValue.");

            //---------------------    _fMoreExists ---
            uint moreExists = Helper.GetUInt(bytes, ref startingIndex);
            // As Boolean as value 0 or 1
            site.CaptureRequirementIfIsTrue(moreExists == 1 || moreExists == 0, 564,
                "The 4 bytes '_fMoreExists' field of the CPMFetchValueOut" +
                "message is a Boolean value.");
            site.CaptureRequirementIfIsTrue(((moreExists == 0x00000000)
                                || (moreExists == 0x00000001)), 565,
                "The 4 bytes '_fMoreExists' field of the CPMFetchValueOut" +
                "message  is one of the following: (0x00000000) " +
                "or (0x00000001).");

            //--------------------     _fValueExists --
            uint valueExists = Helper.GetUInt(bytes, ref startingIndex);
            site.CaptureRequirementIfIsTrue(((valueExists == 0x00000000)
                || (valueExists == 0x00000001)), 944,
                "The 4 bytes '_fValeExists' field of the CPMFetchValueOut" +
                "message  is one of the following: (0x00000000) " +
                "or (0x00000001).");

            //If GetUInt returns successfully Requirement xxx is validated
            site.CaptureRequirement(943, "The 4 bytes '_fValueExists' field" +
                "of the CPMFetchValueOut message is a Boolean value. ");

            if (valueExists > 0)
            {
                site.CaptureRequirementIfAreEqual<uint>(1, valueExists, 733,
                    "When the server receives a CPMFetchValueIn message " +
                    "request from a client, the server MUST  if the property" +
                    "ID value is available, the server MUST set " +
                    "_fValueExists to 0x00000001.");
                // If ReadVariableDataFromBuffer is able to read 
                // data of length cbValue
                // then requirement 737 and 735 are validated
                site.CaptureRequirement(737,
                    "When the server receives a CPMFetchValueIn message" +
                    "request from a client, if _fValueExists is equal to" +
                    "0x00000001, the server MUST set _cbValue to the " +
                    "number of bytes copied.");
                // If ReadVariableDataFromBuffer is able to read 
                // data of length cbValue
                // then requirement 735 is validated
                site.CaptureRequirement(735,
                    "When the server receives a CPMFetchValueIn message" +
                    "request from a client, if _fValueExists is equal to" +
                    "0x00000001, the server MUST serialize the property" +
                    "value to a SERIALIZEDPROPERTYVALUE structure and copy," +
                    "starting from the _cbSoFar offset, at most _cbChunk " +
                    "bytes (but not past the end of the serialized " +
                    "property) to the vValue field.");

                int constantFields
                    = Constants.SIZE_OF_HEADER + 3 * Constants.SIZE_OF_UINT;
                uint variableLength = (uint)(cbChunk - constantFields);

                if (moreExists == 0)
                {
                    site.CaptureRequirementIfIsTrue(cbValue < cbChunk, 568,
                        "If _fMoreExists is set to 0x00000000, the length of" +
                        "the 'vValue' field of the CPMFetchValueOut message," +
                        "MUST be less than the value of _cbChunk in " +
                        "CPMFetchValueIn.");

                }
                else
                {
                    site.CaptureRequirementIfAreEqual<uint>(cbValue,
                        variableLength, 567, "If _fMoreExists is set to" +
                        "0x00000001, the length of the 'vValue' field of " +
                        "the CPMFetchValueOut message, indicated by the " +
                        "_cbValue field, MUST be equal to the value of " +
                        "_cbChunk in CPMFetchValueIn.");
                }
            }
            else
            {
                site.CaptureRequirementIfAreEqual<uint>(0, valueExists, 732,
                    "When the server receives a CPMFetchValueIn message" +
                    "request from a client, if the property ID value is" +
                    "not available, the server MUST set _fValueExists" +
                    "to 0x00000000.");
            }
        }

        /// <summary>
        /// Returns the Storage Width of a given BaseStorage Type
        /// </summary>
        /// <param name="bytes">BLOB to obtain the data  from</param>
        /// <param name="offset">Offset to read the data from</param>
        /// <param name="vType">Data type</param>
        /// <returns>length of the data type</returns>
        private int GetStorageWidth(byte[] bytes, uint offset, uint vType)
        {
            int length = 0;
            if (IsValidVariableLengthDataCBaseStorageVariant_vType((ushort)vType))
            {
                switch ((CBaseStorageVariant_vType_Values)vType)
                {
                    case CBaseStorageVariant_vType_Values.VT_BLOB:
                    case CBaseStorageVariant_vType_Values.VT_BSTR:
                    case CBaseStorageVariant_vType_Values.VT_LPSTR:
                    case CBaseStorageVariant_vType_Values.VT_LPWSTR:
                    case CBaseStorageVariant_vType_Values.VT_COMPRESSED_LPWSTR:
                        // First 4 byte should specify the length
                        length = BitConverter.ToInt32(bytes, (int)offset);

                        // If length is greater than zero for the above mentioned type
                        // this requirement is validated because the Client may obtain
                        // total length data in more than one Fetch Operation
                        site.CaptureRequirement(33,
                            " 'blobData' field of CBaseStorageVariant " +
                            "structure MUST be of length 'cbSize' in bytes.");

                        break;

                    default:
                        break;
                }
            }
            else
            {
                length = Helper.GetSize((CBaseStorageVariant_vType_Values)vType, Is64bit);
            }
            return length;
        }

        #region Private Helper Methods

        /// <summary>
        /// Validates RowSeekAt Type
        /// </summary>
        /// <param name="bytes">RowSeekAt type BLOB</param>
        private void ValidateRowSeekAt(byte[] bytes)
        {
            int startingIndex = 0;

            //---------------------        _bmkOffset    ----------------------------
            uint bmkOffset = Helper.GetUInt(bytes, ref startingIndex);
            // If GetUInt returns successfully Requirement 289 is validated
            site.CaptureRequirement(289,
                "The 4 bytes '_bmkOffset' field of the  CRowSeekAt structure" +
                "represents the handle of the bookmark indicating the " +
                "starting position from which to skip the number of rows" +
                "specified in _cskip, before beginning retrieval.");

            //----------------------        _cskip     
            uint skip = Helper.GetUInt(bytes, ref startingIndex);
            // If GetUInt returns successfully Requirement 290 is validated
            site.CaptureRequirement(290,
                "The 4 bytes '_cskip' field of the  CRowSeekAt structure" +
                "specifies the number of rows to skip in the rowset.");

            //-----------------------       _hRegion     -----------------------------
            uint region = Helper.GetUInt(bytes, ref startingIndex);
            site.CaptureRequirementIfIsTrue((0x00000000 == region), 1212,
                "[CRowSeekAt]This field [_hRegion] MUST be set to 0x00000000.");
            // If GetUInt returns successfully Requirement 293 is validated
            site.CaptureRequirement(293,
                "For  0x00000000 value of '_hRegion' field of the  " +
                "CRowSeekAt structure, server response is success.");
        }

        /// <summary>
        /// Validates RowSeekAtRatio Type
        /// </summary>
        /// <param name="bytes">RowSeekAtRatio BLOB</param>
        private void ValidateRowSeekAtRatio(byte[] bytes)
        {
            int startingIndex = 0;

            //----------------------     _ulNumerator    ----------------------
            uint numerator = Helper.GetUInt(bytes, ref startingIndex);
            // If GetUInt returns successfully Requirement 294 is validated
            site.CaptureRequirement(294,
                "The 4 bytes '_ulNumerator' field of the CRowSeekAtRatio" +
                "structure specifies the numerator of the ratio of rows" +
                "in the chapter at which to begin retrieval.");
            //----------------------     _ulDenominator   ---------------------
            uint denominator = Helper.GetUInt(bytes, ref startingIndex);
            // If GetUInt returns successfully Requirement 295 is validated
            site.CaptureRequirement(295,
                "The 4 bytes '_ulDenominator' field of the CRowSeekAtRatio" +
                "structure specifies the denominator of the ratio of rows" +
                "in the chapter at which to begin retrieval.");
            site.CaptureRequirementIfIsTrue((0x00000000 < denominator), 1215,
                "[CRowSeekAtRatio]_ulDenominator:MUST be greater than zero.");

            //----------------------        _hRegion      ---------------------
            uint region = Helper.GetUInt(bytes, ref startingIndex);
            site.CaptureRequirementIfIsTrue((0x00000000 == region), 1216,
                "[CRowSeekAtRatio]This field [_hRegion] MUST be set to 0x00000000.");
            // If GetUInt returns successfully Requirement 299 is validated
            site.CaptureRequirement(299, "For  0x00000000 value of " +
                "'_hRegion' field of the  CRowSeekAt structure, " +
                "server response is success.");
        }

        /// <summary>
        /// Validates RowSeekByBookmark Type
        /// </summary>
        /// <param name="bytes">RowSeekByBookmark BLOB</param>
        private void ValidateRowSeekByBookmark(byte[] bytes)
        {
            int startingIndex = 0;

            uint bookmarks = Helper.GetUInt(bytes, ref startingIndex);
            // If GetUInt returns successfully Requirement 300 is validated
            site.CaptureRequirement(300,
                "The 4 bytes '_cBookmarks' field of the CRowSeekByBookmark" +
                "structure specifies the number of elements in _aBookmarks" +
                "array of the same structure.");
        }

        /// <summary>
        /// Validates MS-WSP Message Header
        /// </summary>
        /// <param name="responseBytes">response message BLOB</param>
        /// <param name="requestType">type of WSP message</param>
        /// <param name="requestMessageChecksum">checksum of 
        /// request message</param>
        /// <param name="index">index of the BLOB</param>
        public void ValidateHeader(byte[] responseBytes,
            MessageType requestType, uint requestMessageChecksum,
            ref int index)
        {
            var buffer = new WspBuffer(responseBytes);

            var header = new WspMessageHeader();

            header.FromBytes(buffer);

            index = buffer.ReadOffset;

            uint messageType = (uint)header._msg;
            //type and messageType are Equal
            uint logicalOrredValue
                = Constants.AllPossibleMessageTypeValues;
            //Updated by:v-zhil
            //Delta testing
            bool isValidMessage
                = (messageType & logicalOrredValue) == messageType;
            site.CaptureRequirementIfIsTrue(isValidMessage, 363,
                "The value of the 4 bytes 'msg' field of the message header is one of the following: " +
                "0x000000C8, 0x000000C9, 0x000000CA, 0x000000CB, 0x000000CC, 0x000000CD, 0x000000CE, " +
                "0x000000CF, 0x000000D0, 0x000000D1, 0x000000D2, 0x000000D7, 0x000000D9, 0x000000E1, " +
                "0x000000E4, 0x000000E6, 0x000000E7, 0x000000E8, 0x000000E9, 0x000000EC, 0x000000F1, 0x000000F2, 0x000000F3, 0x000000F4.");

            site.CaptureRequirementIfIsTrue(isValidMessage, 621,
                "When a message arrives, the server MUST check the" +
                "_msg field value to see if it is a known type.");
            LogIfValidMessage(requestType, messageType);

            uint status = header._status;

            #region Windows Behaviour Validation
            if (site.Properties["IsServerWindows"].ToUpper() == "TRUE")
            {
                bool isValidNtStatus = IsValidNtStatus(status);
                site.CaptureRequirementIfIsTrue(isValidNtStatus, 1,
                    "This protocol also uses NTSTATUS values taken from" +
                    "the NTSTATUS number space.");
                site.CaptureRequirementIfIsTrue(isValidNtStatus, 2,
                    "<1>Windows only uses the values specified" +
                    "in [MS-ERREF].");
            }
            #endregion

            //Updated by:v-zhil
            //Delta testing
            // If 32 bit Status field is read as Response Status,
            // Requirement 593 is validated
            site.CaptureRequirementIfIsTrue(IsValidNtStatus(status), 593,
                "Otherwise [WSP return failed], WSP messages return a 32-bit error code " +
                "that can either be an HRESULT or an NTSTATUS value (see section 1.8).");
        }

        /// <summary>
        /// Validates MS-WSP Message Header
        /// </summary>
        /// <param name="header">response header</param>
        /// <param name="requestType">type of WSP message</param>
        public void ValidateHeader(WspMessageHeader header, WspMessageHeader_msg_Values requestType)
        {
            uint messageType = (uint)header._msg;
            //type and messageType are Equal
            uint logicalOrredValue
                = Constants.AllPossibleMessageTypeValues;
            //Updated by:v-zhil
            //Delta testing
            bool isValidMessage
                = (messageType & logicalOrredValue) == messageType;
            site.CaptureRequirementIfIsTrue(isValidMessage, 363,
                "The value of the 4 bytes 'msg' field of the message header is one of the following: " +
                "0x000000C8, 0x000000C9, 0x000000CA, 0x000000CB, 0x000000CC, 0x000000CD, 0x000000CE, " +
                "0x000000CF, 0x000000D0, 0x000000D1, 0x000000D2, 0x000000D7, 0x000000D9, 0x000000E1, " +
                "0x000000E4, 0x000000E6, 0x000000E7, 0x000000E8, 0x000000E9, 0x000000EC, 0x000000F1, 0x000000F2, 0x000000F3, 0x000000F4.");

            site.CaptureRequirementIfIsTrue(isValidMessage, 621,
                "When a message arrives, the server MUST check the" +
                "_msg field value to see if it is a known type.");
            LogIfValidMessage((MessageType)(uint)requestType, messageType);

            uint status = header._status;

            #region Windows Behaviour Validation
            if (site.Properties["IsServerWindows"].ToUpper() == "TRUE")
            {
                bool isValidNtStatus = IsValidNtStatus(status);
                site.CaptureRequirementIfIsTrue(isValidNtStatus, 1,
                    "This protocol also uses NTSTATUS values taken from" +
                    "the NTSTATUS number space.");
                site.CaptureRequirementIfIsTrue(isValidNtStatus, 2,
                    "<1>Windows only uses the values specified" +
                    "in [MS-ERREF].");
            }
            #endregion

            //Updated by:v-zhil
            //Delta testing
            // If 32 bit Status field is read as Response Status,
            // Requirement 593 is validated
            site.CaptureRequirementIfIsTrue(IsValidNtStatus(status), 593,
                "Otherwise [WSP return failed], WSP messages return a 32-bit error code " +
                "that can either be an HRESULT or an NTSTATUS value (see section 1.8).");
        }

        /// <summary>
        /// Validates the eState field of CPMCiStateInOut message
        /// </summary>
        /// <param name="eState">eState</param>
        /// <returns>bool value</returns>
        private bool IsValidCPMCiState_eState(CPMCiState_eState_Values eState)
        {
            uint mask = Enum.GetValues(typeof(CPMCiState_eState_Values)).Cast<uint>().Aggregate((uint)0, (x, y) => x | y);
            if ((((uint)eState) & (~mask)) != 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns Unsigned Short field from a BLOB data
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="startingIndex"></param>
        /// <returns></returns>
        public ushort GetUShort(byte[] bytes, ref int startingIndex)
        {
            byte[] tempArray = new byte[Constants.SIZE_OF_USHORT];
            for (int i = 0; i < Constants.SIZE_OF_USHORT; i++)
            {
                tempArray[i] = bytes[i + startingIndex];
            }
            startingIndex += Constants.SIZE_OF_USHORT;
            return BitConverter.ToUInt16(tempArray, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vType"></param>
        /// <returns></returns>
        private bool IsValidCBaseStorageVariant_vType(uint vType)
        {
            bool result = false;
            bool IsValidVector
                = (vType != (uint)(0x1000 | (int)CBaseStorageVariant_vType_Values.VT_DECIMAL)
                && vType != (uint)(0x1000 | (int)CBaseStorageVariant_vType_Values.VT_INT)
                && vType != (uint)(0x1000 | (int)CBaseStorageVariant_vType_Values.VT_UINT)
                && vType != (uint)(0x1000 | (int)CBaseStorageVariant_vType_Values.VT_BLOB));
            site.CaptureRequirementIfIsTrue(IsValidVector, 7,
                "VT_VECTOR type modifier MUST NOT be combined(binary ORed)" +
                "with the following types: VT_INT, VT_UINT," +
                "VT_DECIMAL, VT_BLOB.");

            bool isValidArray
                = 0x2000 != (uint)(0x2000 | (int)CBaseStorageVariant_vType_Values.VT_I8)
                && 0x2000 != (uint)(0x2000 | (int)CBaseStorageVariant_vType_Values.VT_UI8)
                && 0x2000 != (uint)(0x2000 | (int)CBaseStorageVariant_vType_Values.VT_FILETIME)
                && 0x2000 != (uint)(0x2000 | (int)CBaseStorageVariant_vType_Values.VT_CLSID)
                && 0x2000 != (uint)(0x2000 | (int)CBaseStorageVariant_vType_Values.VT_BLOB)
                && 0x2000 != (uint)(0x2000 | (int)CBaseStorageVariant_vType_Values.VT_LPSTR)
                && 0x2000 != (uint)(0x2000 | (int)CBaseStorageVariant_vType_Values.VT_LPWSTR);
            site.CaptureRequirementIfIsTrue(isValidArray, 8,
                "VT_ARRAY type modifier MUST NOT be combined(binary ORed)" +
                "with the following types: VT_I8, VT_UI8, VT_FILETIME," +
                "VT_CLSID, VT_BLOB,  VT_LPSTR, and VT_LPWSTR.");
            vType = vType & 0x2000; // If VT_SAFEARRAY
            vType = vType & 0x1000; // If VT_VECTOR

            switch ((uint)vType)
            {
                case (uint)CBaseStorageVariant_vType_Values.VT_BLOB:
                case (uint)CBaseStorageVariant_vType_Values.VT_BOOL:
                case (uint)CBaseStorageVariant_vType_Values.VT_BSTR:
                case (uint)CBaseStorageVariant_vType_Values.VT_CLSID:
                case (uint)CBaseStorageVariant_vType_Values.VT_COMPRESSED_LPWSTR:
                case (uint)CBaseStorageVariant_vType_Values.VT_CY:
                case (uint)CBaseStorageVariant_vType_Values.VT_DATE:
                case (uint)CBaseStorageVariant_vType_Values.VT_DECIMAL:
                case (uint)CBaseStorageVariant_vType_Values.VT_EMPTY:
                case (uint)CBaseStorageVariant_vType_Values.VT_ERROR:
                case (uint)CBaseStorageVariant_vType_Values.VT_FILETIME:
                case (uint)CBaseStorageVariant_vType_Values.VT_I1:
                case (uint)CBaseStorageVariant_vType_Values.VT_I2:
                case (uint)CBaseStorageVariant_vType_Values.VT_I4:
                case (uint)CBaseStorageVariant_vType_Values.VT_I8:
                case (uint)CBaseStorageVariant_vType_Values.VT_INT:
                case (uint)CBaseStorageVariant_vType_Values.VT_LPSTR:
                case (uint)CBaseStorageVariant_vType_Values.VT_LPWSTR:
                case (uint)CBaseStorageVariant_vType_Values.VT_NULL:
                case (uint)CBaseStorageVariant_vType_Values.VT_R4:
                case (uint)CBaseStorageVariant_vType_Values.VT_R8:
                case (uint)CBaseStorageVariant_vType_Values.VT_UI1:
                case (uint)CBaseStorageVariant_vType_Values.VT_UI2:
                case (uint)CBaseStorageVariant_vType_Values.VT_UI4:
                case (uint)CBaseStorageVariant_vType_Values.VT_UI8:
                case (uint)CBaseStorageVariant_vType_Values.VT_UINT:
                case (uint)CBaseStorageVariant_vType_Values.VT_VARIANT:
                    result = true;
                    break;
                default:
                    break;
            }
            return result;
        }
        /// <summary>
        /// Fetches Fixed length data from a Row
        /// </summary>
        /// <param name="row">Row Array</param>
        /// <param name="rowIndex">Current Index of the row array</param>
        /// <param name="basevType_Values">Type of data storrage type</param>
        /// <returns>Fixed length data blob</returns>
        private byte[] ReadFixedLengthData(byte[] row,
            ref int rowIndex, CBaseStorageVariant_vType_Values basevType_Values)
        {
            byte[] fixedSizedData = null;
            switch (basevType_Values)
            {
                case CBaseStorageVariant_vType_Values.VT_I1:
                    fixedSizedData
                        = Helper.GetData(row, ref rowIndex,
                        Constants.SIZE_OF_BYTE);
                    // Successful Retrieval of 1 byte validates requirement 12
                    site.CaptureRequirement(12,
                        "The size of VT_I1  value of the vValue field of" +
                        "CBaseStorageVariant structure is 1 byte.");
                    break;

                case CBaseStorageVariant_vType_Values.VT_UI1:
                    // Read One Bytes
                    fixedSizedData
                        = Helper.GetData(row, ref rowIndex,
                        Constants.SIZE_OF_BYTE);
                    // Successful Retrieval of 1 byte validates requirement 13
                    site.CaptureRequirement(13,
                        "The size of VT_UI1  value of the vValue field" +
                        "of CBaseStorageVariant structure is 1 byte.");
                    break;

                case CBaseStorageVariant_vType_Values.VT_I2:
                    fixedSizedData
                        = Helper.GetData(row, ref rowIndex,
                        Constants.SIZE_OF_USHORT);
                    // Successful Retrieval of 2 bytes validates requirement 14
                    site.CaptureRequirement(14,
                    "The size of VT_I2  value of the vValue field of" +
                    "CBaseStorageVariant structure is 2 bytes.");
                    break;

                case CBaseStorageVariant_vType_Values.VT_UI2:
                    fixedSizedData
                        = Helper.GetData(row, ref rowIndex,
                        Constants.SIZE_OF_USHORT);
                    // Successful Retrieval of 2 bytes validates requirement 15
                    site.CaptureRequirement(15,
                        "The size of VT_UI2  value of the vValue field " +
                        "of CBaseStorageVariant structure is 2 bytes.");
                    break;

                case CBaseStorageVariant_vType_Values.VT_BOOL:
                    fixedSizedData
                        = Helper.GetData(row, ref rowIndex,
                        Constants.SIZE_OF_USHORT);
                    // Successful Retrieval of 2 bytes validates requirement 16
                    site.CaptureRequirement(16,
                        "The size of VT_BOOL  value of the vValue field" +
                        "of CBaseStorageVariant structure is 2 bytes.");
                    break;

                case CBaseStorageVariant_vType_Values.VT_I4:
                    fixedSizedData
                        = Helper.GetData(row, ref rowIndex,
                        Constants.SIZE_OF_UINT);
                    // Successful Retrieval of 4 bytes validates requirement 17
                    site.CaptureRequirement(17,
                        "The size of VT_I4  value of the vValue field of" +
                        "CBaseStorageVariant structure is 4 bytes.");
                    break;

                case CBaseStorageVariant_vType_Values.VT_UI4:
                    fixedSizedData
                        = Helper.GetData(row, ref rowIndex,
                        Constants.SIZE_OF_UINT);
                    // Successful Retrieval of 4 bytes validates requirement 18
                    site.CaptureRequirement(18,
                        "The size of VT_UI4  value of the vValue field of" +
                        "CBaseStorageVariant structure is 4 bytes.");
                    break;

                case CBaseStorageVariant_vType_Values.VT_R4:
                    fixedSizedData
                        = Helper.GetData(row, ref rowIndex,
                        Constants.SIZE_OF_UINT);
                    // Successful Retrieval of 4 bytes validates requirement 19
                    site.CaptureRequirement(19,
                        "The size of VT_R4  value of the vValue field of" +
                        "CBaseStorageVariant structure is 4 bytes.");
                    break;

                case CBaseStorageVariant_vType_Values.VT_INT:
                    fixedSizedData
                        = Helper.GetData(row, ref rowIndex,
                        Constants.SIZE_OF_UINT);
                    // Successful Retrieval of 4 bytes validates requirement 20
                    site.CaptureRequirement(20,
                        "The size of VT_INT  value of the vValue field" +
                        "of CBaseStorageVariant structure is 4 bytes.");
                    break;

                case CBaseStorageVariant_vType_Values.VT_UINT:
                    fixedSizedData
                        = Helper.GetData(row, ref rowIndex,
                        Constants.SIZE_OF_UINT);
                    // Successful Retrieval of 4 bytes validates requirement 21
                    site.CaptureRequirement(21,
                        "The size of VT_UINT  value of the vValue field " +
                        "of CBaseStorageVariant structure is 4 bytes.");
                    break;

                case CBaseStorageVariant_vType_Values.VT_ERROR:
                    fixedSizedData
                        = Helper.GetData(row, ref rowIndex,
                        Constants.SIZE_OF_UINT);
                    // Successful Retrieval of 4 bytes validates requirement 22
                    site.CaptureRequirement(22,
                        "The size of VT_ERROR  value of the vValue field" +
                        "of CBaseStorageVariant structure is 4 bytes.");
                    break;

                case CBaseStorageVariant_vType_Values.VT_I8:
                    fixedSizedData
                        = Helper.GetData(row, ref rowIndex,
                        2 * Constants.SIZE_OF_UINT);
                    // Successful Retrieval of 8 bytes validates requirement 23
                    site.CaptureRequirement(23,
                    "The size of VT_I8 value of the vValue field of" +
                    "CBaseStorageVariant structure is 8 bytes.");
                    break;

                case CBaseStorageVariant_vType_Values.VT_UI8:
                    fixedSizedData = Helper.GetData(row, ref rowIndex,
                        2 * Constants.SIZE_OF_UINT);
                    // Successful Retrieval of 8 bytes validates requirement 24
                    site.CaptureRequirement(24,
                        "The size of VT_UI8 value of the vValue field of" +
                        "CBaseStorageVariant structure is 8 bytes.");
                    break;

                case CBaseStorageVariant_vType_Values.VT_R8:
                    fixedSizedData = Helper.GetData(row, ref rowIndex,
                        2 * Constants.SIZE_OF_UINT);
                    // Successful Retrieval of 8 bytes validates requirement 25
                    site.CaptureRequirement(25, "The size of VT_R8 value" +
                        "of the vValue field of CBaseStorageVariant " +
                        "structure is 8 bytes.");
                    break;

                case CBaseStorageVariant_vType_Values.VT_CY:
                    fixedSizedData = Helper.GetData(row, ref rowIndex,
                        2 * Constants.SIZE_OF_UINT);
                    // Successful Retrieval of 8 bytes validates requirement 26
                    site.CaptureRequirement(26,
                        "The size of VT_CY value of the vValue field of" +
                        "CBaseStorageVariant structure is 8 bytes.");
                    break;

                case CBaseStorageVariant_vType_Values.VT_DATE:
                    fixedSizedData = Helper.GetData(row, ref rowIndex,
                        2 * Constants.SIZE_OF_UINT);
                    // Successful Retrieval of 8 bytes validates requirement 27
                    site.CaptureRequirement(27,
                        "The size of VT_DATE value of the vValue field" +
                        "of CBaseStorageVariant structure is 8 bytes.");
                    break;

                case CBaseStorageVariant_vType_Values.VT_FILETIME:
                    fixedSizedData = Helper.GetData(row, ref rowIndex,
                        2 * Constants.SIZE_OF_UINT);
                    // Successful Retrieval of 8 bytes validates requirement 28
                    site.CaptureRequirement(28,
                        "The size of VT_FILETIME value of the vValue" +
                        "field of CBaseStorageVariant structure is 8 bytes.");
                    break;

                case CBaseStorageVariant_vType_Values.VT_DECIMAL:
                    fixedSizedData
                        = Helper.GetData(row, ref rowIndex,
                        Constants.SIZE_OF_GUID);
                    // Successful Retrieval of 16 bytes validates requirement 29
                    site.CaptureRequirement(29,
                        "The size of VT_DECIMAL value of the vValue field" +
                        "of CBaseStorageVariant structure is 8 bytes.");
                    break;

                case CBaseStorageVariant_vType_Values.VT_CLSID:
                    fixedSizedData = Helper.GetData(row, ref rowIndex,
                        Constants.SIZE_OF_GUID);
                    // Successful Retrieval of 16 bytes validates requirement 30
                    site.CaptureRequirement(30,
                        "The size of VT_FILETIME value of the vValue field" +
                        "of CBaseStorageVariant structure is 8 bytes.");
                    break;
            }

            return fixedSizedData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowsInResponse"></param>
        /// <param name="offset2"></param>
        /// <param name="actualLength"></param>
        /// <returns></returns>
        private byte[] ReadVariableLengthDataFromBuffer(byte[] rowsInResponse,
            uint offset2, int actualLength)
        {
            byte[] value = new byte[actualLength];
            Array.Copy(rowsInResponse, offset2, value, 0, actualLength);
            return value;
        }

        private bool IsValidVariableLengthDataCBaseStorageVariant_vType(ushort actualType)
        {
            bool isValid = false;
            switch ((CBaseStorageVariant_vType_Values)actualType)
            {
                case CBaseStorageVariant_vType_Values.VT_BLOB:
                case CBaseStorageVariant_vType_Values.VT_BSTR:
                case CBaseStorageVariant_vType_Values.VT_LPSTR:
                case CBaseStorageVariant_vType_Values.VT_LPWSTR:
                case CBaseStorageVariant_vType_Values.VT_COMPRESSED_LPWSTR:
                case CBaseStorageVariant_vType_Values.VT_VARIANT:
                    isValid = true;
                    break;

                default:
                    break;
            }

            return isValid;
        }

        /// <summary>
        /// Returns whether a Server returned code is a Valid NT Status
        /// </summary>
        /// <param name="status">4 byte unsigned status</param>
        /// <returns>true if it is a valid NT_STATUS, else false</returns>
        private bool IsValidNtStatus(uint status)
        {
            bool isValidStatus = false;
            // 0x00000000 to 0x3FFFFFFF represents SUCCESS Status
            // 0x40000000 to 0x7FFFFFFF represents INFORMATION Status
            // 0x80000000 to 0xBFFFFFFF represents WARNING Status
            // 0xC0000000 to 0xFFFFFFFF represents ERROR Status
            if ((status >= 0x00000000 && status <= 0x3FFFFFFF) ||
                (status >= 0x40000000 && status <= 0x7FFFFFFF) ||
                (status >= 0x80000000 && status <= 0xBFFFFFFF) ||
                (status >= 0xC0000000 && status <= 0xFFFFFFFF))
            {
                isValidStatus = true;
            }
            return isValidStatus;
        }

        /// <summary>
        /// Logs the requirement if the messageType is 
        /// valid for the request
        /// </summary>
        /// <param name="requestType">Input Request Type</param>
        /// <param name="messageType">Output Response value</param>
        private void LogIfValidMessage(MessageType requestType,
            uint messageType)
        {
            switch (requestType)
            {
                case MessageType.CPMConnectIn:
                    site.CaptureRequirementIfAreEqual<uint>(0x000000C8,
                        messageType, 904,
                        "The value of 4 bytes 'msg' field for CPMConnectOut" +
                        "message is 0x000000C8.");
                    break;
                case MessageType.CPMCreateQueryIn:
                    site.CaptureRequirementIfAreEqual<uint>(0x000000CA,
                        messageType, 907,
                        "The value of 4 bytes 'msg' field for " +
                        "CPMCreateQueryOut message is 0x000000CA.");
                    //If as a Response to CPMCreateQueryIn message
                    // CPMCreateQueryOut message is obtained, following
                    // requirement is validated.
                    site.CaptureRequirement(671,
                        "When the server receives a CPMCreateQueryIn " +
                        "message request from a client, the server MUST" +
                        "respond to the client with a " +
                        "CPMCreateQueryOut message.");
                    break;

                case MessageType.CPMFreeCursorIn:
                    site.CaptureRequirementIfAreEqual<uint>(0x000000CB,
                        messageType, 909,
                        "The value of 4 bytes 'msg' field for " +
                        "CPMFreeCursorOut message is 0x000000CB.");
                    break;

                case MessageType.CPMGetRowsIn:
                    site.CaptureRequirementIfAreEqual<uint>(0x000000CC,
                        messageType, 911,
                        "The value of 4 bytes 'msg' field for CPMGetRowsOut" +
                        "message is 0x000000CC.");
                    //If as a Response to CPMGetRowsIn message
                    // CPMGetRowsOut message is obtained, following
                    // requirement is validated.
                    site.CaptureRequirement(709,
                        "When the server receives a CPMGetRowsIn message" +
                        "request from a client, the server MUST prepare " +
                        "a CPMGetRowsOut message.");

                    break;

                case MessageType.CPMRatioFinishedIn:
                    site.CaptureRequirementIfAreEqual<uint>(0x000000CD,
                        messageType, 913,
                        "The value of 4 bytes 'msg' field for " +
                        "CPMRatioFinishedOut message is 0x000000CD.");
                    //If as a Response to CPMRatioFinishedIn message
                    // CPMRatioFinishedOut message is obtained, following
                    // requirement is validated.
                    site.CaptureRequirement(697,
                        "When the server receives a CPMRatioFinishedIn" +
                        "message request from a client, the server MUST" +
                        "prepare a CPMRatioFinishedOut message.");
                    break;

                case MessageType.CPMCompareBmkIn:
                    break;

                case MessageType.CPMGetApproximatePositionIn:
                    break;

                case MessageType.CPMSetBindingsIn:
                    site.CaptureRequirementIfAreEqual<uint>(0x000000D0,
                        messageType, 919,
                        "The value of 4 bytes 'msg' field for the response" +
                        "of CPMSetBindingsIn message is 0x000000D0.");
                    break;
                case MessageType.CPMGetNotify:
                    site.CaptureRequirementIfAreEqual<uint>(0x000000D2,
                        messageType, 921,
                        "The value of 4 bytes 'msg' field for " +
                        "CPMSendNotifyOut message is 0x000000D2.");
                    break;

                case MessageType.CPMGetQueryStatusIn:
                    site.CaptureRequirementIfAreEqual<uint>(0x000000D7,
                        messageType, 923,
                        "The value of 4 bytes 'msg' field for " +
                        "CPMGetQueryStatusOut message is 0x000000D7.");
                    //If as a Response to CPMGetQueryStatusIn message
                    // CPMGetQueryStatusOut message is obtained, following
                    // requirement is validated.
                    site.CaptureRequirement(676,
                        "When the server receives a CPMGetQueryStatusIn" +
                        "message request from a client, the server MUST" +
                        "prepare a CPMGetQueryStatusOut message.");
                    break;

                case MessageType.CPMCiStateInOut:
                    site.CaptureRequirementIfAreEqual<uint>(0x000000D9,
                        messageType, 925,
                        "The value of 4 bytes 'msg' field for the response" +
                        "of CPMCiStateInOut message is 0x000000D9.");
                    break;
                case MessageType.CPMFetchValueIn:
                    site.CaptureRequirementIfAreEqual<uint>(0x000000E4,
                        messageType, 929,
                        "The value of 4 bytes 'msg' field for CPMFetchValueOut" +
                        "message is 0x000000E4.");
                    //If as a Response to CPMFetchValueIn message
                    // CPMFetchValueOut message is obtained, following
                    // requirement is validated.
                    site.CaptureRequirement(729, "When the server receives a" +
                        "CPMFetchValueIn message request from a client, the " +
                        "server MUST  prepare a CPMFetchValueOut message.");
                    break;

                case MessageType.CPMGetQueryStatusExIn:
                    site.CaptureRequirementIfAreEqual<uint>(0x000000E7,
                        messageType, 933,
                        "The value of 4 bytes 'msg' field for " +
                        "CPMGetQueryStatusExOut message is 0x000000E7.");
                    //If as a Response to CPMGetQueryStatusExIn message
                    // CPMGetQueryStatusExOut message is obtained, following
                    // requirement is validated.
                    site.CaptureRequirement(684, "When the server receives a" +
                        "CPMGetQueryStatusExIn message request from a client," +
                        "the server MUST Prepare a " +
                        "CPMGetQueryStatusExOut message.");
                    break;

                case MessageType.CPMRestartPositionIn:
                    break;

                default:
                    break;
            }
        }

        #endregion
    }
}
