// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp.Adapter
{
    /// <summary>
    /// WspAdapter class implements the methods of the IWspAdapter interface.
    /// </summary>
    public class WspAdapter : ManagedAdapterBase, IWspAdapter
    {
        #region Fields

        //Platform version
        private SkuOsVersion platform;

        /// <summary>
        /// startingIndex define the offset of the massage
        /// </summary>
        private int startingIndexconnect;

        /// <summary>
        /// ITestSite to access the configurable data and requirement logging 
        /// </summary>
        private ITestSite wspTestSite = null;

        /// <summary>
        /// Sends WSP-Messages to the Server for negative scenarios
        /// (To validate messages with out sending the pre-requisite
        /// ConnectIn message)
        /// </summary>
        private WspClient defaultClient = null;

        /// <summary>
        /// Sends WSP-Messages to the Server for negative scenarios
        /// (To validate messages with out sending the pre-requisite
        /// ConnectIn message)
        /// </summary>
        private RequestSender defaultSender = null;

        /// <summary>
        /// MessageBuilder builds client requests
        /// </summary>
        public MessageBuilder Builder = null;

        /// <summary>
        /// Validates Server responses
        /// </summary>
        private MessageValidator validator = null;

        /// <summary>
        /// Maps list of connected clients
        /// </summary>
        private Dictionary<string, WspClient> connectedClients = null;

        /// <summary>
        /// List of disconnected clients for cleaning up underlying SMB2Client instances
        /// </summary>
        private List<WspClient> disconnectedClients = new List<WspClient>();

        /// <summary>
        /// Maps a cursor corresponding to a client
        /// </summary>
        private Dictionary<string, uint> cursorMap = new Dictionary<string, uint>();

        /// <summary>
        /// Name of the Server hosting Windows Search Service
        /// </summary>
        private string serverMachineName = null;

        /// <summary>
        /// Timeout of the underlying SMB2Client used by the RequestSender.
        /// </summary>
        private TimeSpan smb2ClientTimeout = default(TimeSpan);

        /// <summary>
        /// Number of bytes read from the server for a 
        /// given request message
        /// </summary>
        private int bytesRead = -1;

        /// <summary>
        /// Name of the connected Client
        /// </summary>
        public string ClientMachineName = null;

        /// <summary>
        /// Catalog Name to Query
        /// </summary>
        public string CatalogName = null;

        /// <summary>
        /// Name of the connected user
        /// </summary>
        private string userName = null;

        /// <summary>
        /// Domain of the connected user
        /// </summary>
        private string domainName = null;

        /// <summary>
        /// Password of the connected user
        /// </summary>
        private string password = null;

        /// <summary>
        /// Security package used for authentication.
        /// </summary>
        private string securityPackage = null;

        /// <summary>
        /// Whether the client will use server-initiated SPNEGO authentication.
        /// </summary>
        private bool useServerGssToken = false;

        /// <summary>
        /// Determines whether the client is connected
        /// </summary>
        private bool isClientConnected = false;

        /// <summary>
        /// CbReserve Field of RowsIn message
        /// </summary>
        private uint rowsInReserve = 0;

        /// <summary>
        /// Client Base Field of RowsIn message
        /// </summary>
        private uint rowsInClientBase = 0;

        /// <summary>
        /// TableColumns to be queried
        /// </summary>
        private TableColumn[] tableColumns = null;

        /// <summary>
        /// Language locale
        /// </summary>
        private string locale = null;

        /// <summary>
        /// Client version
        /// </summary>
        public uint ClientVersion = 0;

        /// <summary>
        /// Send an invalid _msg value in the message header
        /// </summary>
        public bool SendInvalidMsg = false;

        /// <summary>
        /// Send an invalid _status value in the message header.
        /// </summary>
        public bool SendInvalidStatus = false;

        /// <summary>
        /// Send an invalid _ulChecksum value in the message header. 
        /// </summary>
        public bool SendInvalidUlChecksum = false;

        /// <summary>
        /// Calculate the corret checksum of the message.
        /// </summary>
        private bool calculateChecksum = true;

        /// <summary>
        /// The current CPMSetBindingsIn message sent to service indicates the columns bindings of the current query.
        /// </summary>
        private CPMSetBindingsIn currentCPMSetBindingsIn;
        #endregion

        #region Initialize & Cleanup
        /// <summary>
        /// Initialises the fields of WspAdapter
        /// </summary>
        /// <param name="testSite">testSite</param>
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
            wspTestSite = testSite;
            wspTestSite = TestClassBase.BaseTestSite;
            connectedClients = new Dictionary<string, WspClient>();
            wspTestSite.DefaultProtocolDocShortName = "MS-WSP";
            validator = new MessageValidator(wspTestSite);
            serverMachineName = wspTestSite.Properties.Get("ServerComputerName");
            ClientMachineName = wspTestSite.Properties.Get("ClientName");
            CatalogName = wspTestSite.Properties.Get("CatalogName");
            userName = wspTestSite.Properties.Get("UserName");
            domainName = wspTestSite.Properties.Get("DomainName");
            password = wspTestSite.Properties.Get("Password");
            ClientVersion = Convert.ToUInt32(wspTestSite.Properties["ClientVersion"]);

            securityPackage = wspTestSite.Properties.Get("SupportedSecurityPackage");
            useServerGssToken = bool.Parse(wspTestSite.Properties.Get("UseServerGssToken"));
            smb2ClientTimeout = TimeSpan.FromSeconds(int.Parse(wspTestSite.Properties.Get("SMB2ClientTimeout")));
            defaultSender = new RequestSender(
                serverMachineName,
                userName,
                domainName,
                password,
                securityPackage,
                useServerGssToken,
                smb2ClientTimeout);

            defaultClient = new WspClient();

            defaultClient.Sender = defaultSender;

            var parameter = InitializeParameter();
            Builder = new MessageBuilder(parameter);

            isClientConnected = false;
        }

        private MessageBuilderParameter InitializeParameter()
        {
            char[] delimiter = new char[] { ',' };

            var parameter = new MessageBuilderParameter();

            parameter.PropertySet_One_DBProperties = wspTestSite.Properties.Get("PropertySet_One_DBProperties").Split(delimiter);

            parameter.PropertySet_Two_DBProperties = wspTestSite.Properties.Get("PropertySet_Two_DBProperties").Split(delimiter);

            parameter.Array_PropertySet_One_Guid = new Guid(wspTestSite.Properties.Get("Array_PropertySet_One_Guid"));

            parameter.Array_PropertySet_One_DBProperties = wspTestSite.Properties.Get("Array_PropertySet_One_DBProperties").Split(delimiter);

            parameter.Array_PropertySet_Two_Guid = new Guid(wspTestSite.Properties.Get("Array_PropertySet_Two_Guid"));

            parameter.Array_PropertySet_Two_DBProperties = wspTestSite.Properties.Get("Array_PropertySet_Two_DBProperties").Split(delimiter);

            parameter.Array_PropertySet_Three_Guid = new Guid(wspTestSite.Properties.Get("Array_PropertySet_Three_Guid"));

            parameter.Array_PropertySet_Three_DBProperties = wspTestSite.Properties.Get("Array_PropertySet_Three_DBProperties").Split(delimiter);

            parameter.Array_PropertySet_Four_Guid = new Guid(wspTestSite.Properties.Get("Array_PropertySet_Four_Guid"));

            parameter.Array_PropertySet_Four_DBProperties = wspTestSite.Properties.Get("Array_PropertySet_Four_DBProperties").Split(delimiter);

            parameter.EachRowSize = MessageBuilder.RowWidth;

            parameter.EType = uint.Parse(wspTestSite.Properties.Get("EType"));

            parameter.BufferSize = uint.Parse(wspTestSite.Properties.Get("BufferSize"));

            parameter.LcidValue = uint.Parse(wspTestSite.Properties.Get("LcidValue"));

            parameter.ClientBase = uint.Parse(wspTestSite.Properties.Get("ClientBase"));

            parameter.RowsToTransfer = uint.Parse(wspTestSite.Properties.Get("RowsToTransfer"));

            return parameter;
        }

        /// <summary>
        /// Closes all the communication Pipe and deallocate the resources used.
        /// </summary>
        public override void Reset()
        {
            defaultClient.Sender.Disconnect();

            foreach (var client in connectedClients.Values)
            {
                client.Sender.Disconnect();
            }

            foreach (var client in disconnectedClients)
            {
                client.Sender.Disconnect();
            }

            connectedClients.Clear();
            disconnectedClients.Clear();
            cursorMap.Clear();

            isClientConnected = false;

            base.Reset();
        }

        #endregion

        #region IWspAdapter Members

        #region CPMConnectIn and CPMConnectOut

        /// <summary>
        /// CPMConnectIn is used to send a request to establish
        /// a connection with the server and starts WSP query processing
        /// or WSP catalog administration.
        /// </summary>
        public void CPMConnectIn()
        {
            int remoteClient = 1;
            CPMConnectIn(ClientVersion, remoteClient, CatalogName);
        }

        /// <summary>
        /// Used to send a request to establish a connection with the server
        /// and start WSP query processing or WSP catalog administration
        /// </summary>
        /// <param name="clientVersion">Indicate whether the server is to validate the checksum</param>
        /// <param name="isClientRemote">Indicate if the client is running on a different machine than the server</param>
        /// <param name="catalogName">The name of the catalog</param>
        /// <param name="cPropSets">The number of CDbPropSet structures in the following fields</param>
        /// <param name="cExtPropSet">The number of CDbPropSet structures in aPropertySet</param>
        public void CPMConnectIn(uint clientVersion, int isClientRemote, string catalogName, uint cPropSets = 2, uint cExtPropSet = 4)
        {
            WspClient client;

            string locale = wspTestSite.Properties.Get("LanguageLocale");

            var connectInMessage = Builder.GetCPMConnectIn(clientVersion, isClientRemote, userName, ClientMachineName, serverMachineName, catalogName, locale, cPropSets, cExtPropSet);

            WspMessageHeader? header = GetWspMessageHeader(connectInMessage.Header._msg, connectInMessage.Header._status, connectInMessage.Header._ulChecksum);

            //Send the connectIn message to Server.
            if (!connectedClients.ContainsKey(ClientMachineName))
            {
                client = new WspClient();
                client.Sender = new RequestSender(
                    serverMachineName,
                    userName,
                    domainName,
                    password,
                    securityPackage,
                    useServerGssToken,
                    smb2ClientTimeout);
                connectedClients.Add(ClientMachineName, client);
            }
            else
            {
                client = connectedClients[ClientMachineName];
            }

            // Send CPMConnectIn Message
            // Write the message in the Pipe and Get the response in the outputBuffer
            var connectInMessageBytes = Helper.ToBytes(connectInMessage);
            uint checkSum = GetChecksumField(connectInMessageBytes);
            client.SendCPMConnectIn(connectInMessage._iClientVersion, connectInMessage._fClientIsRemote, connectInMessage.MachineName, connectInMessage.UserName, connectInMessage.PropertySet1, connectInMessage.PropertySet2, connectInMessage.aPropertySets, cPropSets: connectInMessage.cPropSets, cExtPropSet: connectInMessage.cExtPropSet, calculateChecksum: calculateChecksum, header: header);

            // RequestSender objects uses path '\\pipe\\MSFTEWDS' for the protocol transport
            wspTestSite.CaptureRequirement(3, @"All messages MUST be " +
                 "transported using a named pipe: \\pipe\\MSFTEWDS");

            // Get the errorCode & invoke event
            if (CPMConnectOutResponse != null)
            {
                CPMConnectOut connectOutMessage;

                client.ExpectMessage(out connectOutMessage);

                uint msgId = (uint)connectOutMessage.Header._msg;
                uint msgStatus = connectOutMessage.Header._status;
                if (msgStatus != 0)
                {
                    wspTestSite.CaptureRequirement(620,
                        "Whenever an error occurs during processing of a " +
                        "message sent by a client, the server MUST set the " +
                        "_status field to the error code value.");
                }
                if ((msgId == (uint)MessageType.CPMConnectOut)
                    && (msgStatus == 0x00000000))
                {
                    validator.ValidateCPMConnectOutResponse(connectOutMessage);
                    Builder.Is64bit = validator.Is64bit;
                    isClientConnected = true;
                }

                //Updated by v-aliche
                //Delta Testing
                startingIndexconnect = 0;
                validator.ValidateHeader
                    (connectInMessageBytes,
                    MessageType.CPMConnectOut,
                    checkSum, ref startingIndexconnect);
                uint obtainedServerVersion
                 = Helper.GetUInt(connectInMessageBytes, ref startingIndexconnect);
                //if serverVersion equals 0x00000102, ir means Windows server 2008 with OS 32-bit
                //if serverVersion equals 0x00010102, ir means Windows server 2008 with OS 64-bit
                if (obtainedServerVersion == 0x00000102 || obtainedServerVersion == 0x00010102)
                {
                    byte[] dWordIn = new byte[4];
                    byte[] dWordOut = new byte[4];
                    Helper.CopyBytes(connectInMessageBytes, ref startingIndexconnect, dWordIn);
                    Helper.CopyBytes(connectInMessageBytes, ref startingIndexconnect, dWordOut);
                    bool isEqual = dWordIn.ToString() == dWordOut.ToString();

                    Site.CaptureRequirementIfIsTrue(isEqual, 1055, "When the server receives a CPMConnectIn request from a client," +
                        "Otherwise[the server not support reporting versioning information]" +
                        "server MUST copy 4 DWORDs at the offset starting after _serverVersion" +
                        "from the CPMConnectIn message to indicate that versioning is not supported.");
                }

                CPMConnectOutResponse(msgStatus);
            }
        }

        /// <summary>
        /// This event is used to get the response from CPMConnectIn request.
        /// </summary>
        public event CPMConnectOutResponseHandler CPMConnectOutResponse;

        #endregion

        #region CPMCreateQueryIn and CPMCreateQueryOut

        /// <summary>
        /// CPMCreateQueryIn() emulates WSP CPMCreateQueryIn message and 
        /// associates a query with the client if successful
        /// </summary>
        public void CPMCreateQueryIn(bool ENABLEROWSETEVENTS)
        {
            string queryScope = wspTestSite.Properties.Get("QueryPath");
            string queryText = wspTestSite.Properties.Get("QueryText");

            var queryInMessage = Builder.GetCPMCreateQueryIn(queryScope, queryText, WspConsts.System_Search_Contents, ENABLEROWSETEVENTS);

            CPMCreateQueryIn(
                queryInMessage.ColumnSet,
                queryInMessage.RestrictionArray,
                queryInMessage.SortSet,
                queryInMessage.CCategorizationSet,
                queryInMessage.RowSetProperties,
                queryInMessage.PidMapper,
                queryInMessage.GroupArray,
                queryInMessage.Lcid);
        }

        /// <summary>
        /// Create and send CPMCreateQueryIn and expect response.
        /// </summary>
        public void CPMCreateQueryIn(
            CColumnSet? columnSet,
            CRestrictionArray? restrictionArray,
            CInGroupSortAggregSets? sortSet,
            CCategorizationSet? categorizationSet,
            CRowsetProperties rowsetProperties,
            CPidMapper pidMapper,
            CColumnGroupArray groupArray,
            uint lcid)
        {
            CPMCreateQueryIn(
                columnSet,
                restrictionArray,
                sortSet,
                categorizationSet,
                rowsetProperties,
                pidMapper,
                groupArray,
                lcid,
                out _);
        }

        /// <summary>
        /// Create and send CPMCreateQueryIn and expect response.
        /// </summary>
        public void CPMCreateQueryIn(
            CColumnSet? columnSet,
            CRestrictionArray? restrictionArray,
            CInGroupSortAggregSets? sortSet,
            CCategorizationSet? categorizationSet,
            CRowsetProperties rowsetProperties,
            CPidMapper pidMapper,
            CColumnGroupArray groupArray,
            uint lcid,
            out CPMCreateQueryOut createQueryOut)
        {
            var client = GetClient(isClientConnected);

            client.SendCPMCreateQueryIn(columnSet, restrictionArray, sortSet, categorizationSet, rowsetProperties, pidMapper, groupArray, lcid);

            client.ExpectMessage<CPMCreateQueryOut>(out createQueryOut);

            if (createQueryOut.Header._status == 0)
            {
                uint[] cursor = null;// to be obtained from the server
                validator.ValidateCPMCreateQueryOutResponse(createQueryOut, out cursor);
                cursorMap[ClientMachineName] = cursor[0];
            }

            CPMCreateQueryOutResponse(createQueryOut.Header._status);
        }

        /// <summary>
        /// This event is used to get the response for CPMCreateQueryIn request.
        /// </summary>
        public event CPMCreateQueryOutResponseHandler CPMCreateQueryOutResponse;

        #endregion

        #region CPMGetQueryStatusIn and CPMGetQueryStatusOut

        /// <summary>
        /// CPMGetQueryStatusIn() requests the status of the query.
        /// </summary>
        /// <param name="isCursorValid">Indicates the client requesting
        /// status of the query.</param>
        public void CPMGetQueryStatusIn(bool isCursorValid)
        {
            int MAX_RANGE = 500;
            int MIN_RANGE = 100;
            int startingIndex = 0;
            uint cursorAssociated = 0;
            if (isCursorValid)
            {
                cursorAssociated = GetCursor(ClientMachineName);
            }
            else
            {
                // Generating a PseudoRandom Number
                Random r = new Random();
                cursorAssociated
                    = uint.MaxValue - (uint)r.Next(MIN_RANGE, MAX_RANGE);
            }
            byte[] statusInMessage
                = Builder.GetCPMQueryStatusIn(cursorAssociated);
            byte[] statusOutMessage;
            RequestSender sender = GetRequestSender(isClientConnected);
            bytesRead
                = sender.SendMessage(statusInMessage, out statusOutMessage);
            if (sender == defaultSender)
            {
                //This means that disconnect message has been sent
                // through the pipe which does not have a connect In
                // sent across it.
                // If the sender.SendMessage() method is successful
                // Requirement 598 is validated
                wspTestSite.CaptureRequirement(598,
                   "In Windows the same pipe connection is used for the " +
                   "following messages, except when the error is returned" +
                   "in a CPMConnectOut message.");

            }
            // RequestSender objects uses path '\\pipe\\MSFTEWDS'
            // for the protocol transport
            wspTestSite.CaptureRequirement(3, @"All messages MUST be " +
                "transported using a named pipe: \\pipe\\MSFTEWDS");

            uint checkSum = 0;
            if (CPMGetQueryStatusOutResponse != null)
            {
                startingIndex = 0;
                uint msgId = Helper.GetUInt(statusOutMessage,
                    ref startingIndex);
                uint msgStatus = Helper.GetUInt(statusOutMessage,
                    ref startingIndex);
                if (msgStatus != 0)
                {
                    wspTestSite.CaptureRequirementIfAreEqual<int>(bytesRead,
                    Constants.SIZE_OF_HEADER, 619,
                    "Whenever an error occurs during processing of a " +
                    "message sent by a client, the server MUST respond " +
                    "with the message header (only) of the message sent " +
                    "by the client, keeping the _msg field intact.");
                    //If 4 byte Non Zero field is read as status
                    // The requirement 620 gets validated
                    wspTestSite.CaptureRequirement(620,
                        "Whenever an error occurs during processing of a " +
                        "message sent by a client, the server MUST set the " +
                        "_status field to the error code value.");
                }
                if ((msgId == (uint)MessageType.CPMGetQueryStatusOut)
                    && (msgStatus == 0x00000000))
                {
                    validator.ValidateCPMGetQueryStatusOutResponse(statusOutMessage,
                        checkSum);
                }
                CPMGetQueryStatusOutResponse(msgStatus);
            }
        }

        /// <summary>
        /// This event is used to get the response from 
        /// CPMGetQueryStatusIn request.
        /// </summary>
        public event CPMGetQueryStatusOutResponseHandler CPMGetQueryStatusOutResponse;

        #endregion

        #region CPMGetQueryStatusExIn and CPMGetQueryStatusExOut

        /// <summary>
        /// CPMGetQueryStatusExIn() requests for the status of the query and the additional information from the server.
        /// </summary>
        /// <param name="isCursorValid">Indicates the client requesting status has a valid cursor.</param>
        public void CPMGetQueryStatusExIn(bool isCursorValid)
        {
            uint cursorAssociated = 0;
            if (isCursorValid)
            {
                cursorAssociated = GetCursor(ClientMachineName);
            }
            else
            {
                cursorAssociated = 2;
            }

            CPMGetQueryStatusExIn(cursorAssociated, 1, out var _);
        }

        /// <summary>
        /// CPMGetQueryStatusExIn() requests for the status of the query and the additional information from the server.
        /// </summary>
        /// <param name="response">The CPMGetQueryStatusExOut response.</param>
        public void CPMGetQueryStatusExIn(out CPMGetQueryStatusExOut response)
        {
            uint cursorAssociated = GetCursor(ClientMachineName);
            CPMGetQueryStatusExIn(cursorAssociated, 1, out response);
        }

        /// <summary>
        /// CPMGetQueryStatusExIn() requests for the status of the query and the additional information from the server.
        /// </summary>
        /// <param name="cursor">The query handle.</param>
        /// <param name="bookMarkHandle">The bookmark handle.</param>
        /// <param name="response">The CPMGetQueryStatusExOut response.</param>
        public void CPMGetQueryStatusExIn(uint cursor, uint bookMarkHandle, out CPMGetQueryStatusExOut response)
        {
            response = null;
            byte[] statusExInMessage = Builder.GetCPMQueryStatusExIn(cursor, bookMarkHandle);
            byte[] statusExOutMessage;
            uint checkSum = 0;
            RequestSender sender = GetRequestSender(isClientConnected); //Get the Sender
            bytesRead = sender.SendMessage(statusExInMessage, out statusExOutMessage);

            if (bytesRead == -1)
            {
                int error = Marshal.GetLastWin32Error();
            }

            if (sender == defaultSender)
            {
                // This means that disconnect message has been sent
                // through the pipe which does not have a connect In
                // sent across it.
                // If the sender.SendMessage() method is successful
                // Requirement 598 is validated
                wspTestSite.CaptureRequirement(598,
                   "In Windows the same pipe connection is used for the " +
                   "following messages, except when the error is returned" +
                   "in a CPMConnectOut message.");

            }

            wspTestSite.CaptureRequirement(3, @"All messages MUST be " +
                            "transported using a named pipe: \\pipe\\MSFTEWDS");

            if (CPMGetQueryStatusExOutResponse != null)
            {
                int startingIndex = 0;
                uint msgId = Helper.GetUInt(statusExOutMessage, ref startingIndex);
                uint msgStatus = Helper.GetUInt(statusExOutMessage, ref startingIndex);

                if (msgStatus != 0)
                {
                    wspTestSite.CaptureRequirementIfAreEqual<int>(bytesRead,
                        Constants.SIZE_OF_HEADER, 619,
                        "Whenever an error occurs during processing of a " +
                        "message sent by a client, the server MUST respond " +
                        "with the message header (only) of the message sent " +
                        "by the client, keeping the _msg field intact.");

                    //If 4 byte Non Zero field is read as status
                    // The requirement 620 gets validated
                    wspTestSite.CaptureRequirement(620,
                        "Whenever an error occurs during processing of a " +
                        "message sent by a client, the server MUST set the " +
                        "_status field to the error code value.");
                }

                if ((msgId == (uint)MessageType.CPMGetQueryStatusExOut)
                    && (msgStatus == 0x00000000))
                {
                    validator.ValidateCPMGetQueryStatusExOutResponse(statusExOutMessage,
                        checkSum);

                    var getQueryStatusExOut = new CPMGetQueryStatusExOut();
                    Helper.FromBytes<CPMGetQueryStatusExOut>(ref getQueryStatusExOut, statusExOutMessage);
                    response = getQueryStatusExOut;
                }

                // Fire Response Event
                CPMGetQueryStatusExOutResponse(msgStatus);
            }
        }

        /// <summary>
        /// This event is used to get the response from 
        /// CPMGetQueryStatusExIn request.
        /// </summary>
        public event CPMGetQueryStatusExOutResponseHandler CPMGetQueryStatusExOutResponse;

        #endregion

        #region CPMRatioFinishedIn and CPMRatioFinishedOut

        /// <summary>
        /// CPMRatioFinishedIn() requests for the completion percentage of the 
        /// query.
        /// </summary>
        /// <param name="isCursorValid">Indicates the client requesting 
        /// has a valid cursor</param>
        public void CPMRatioFinishedIn(bool isCursorValid)
        {
            uint cursorAssociated = 0;
            if (isCursorValid)
            {
                cursorAssociated = GetCursor(ClientMachineName);
            }
            else
            {
                cursorAssociated = uint.MaxValue - 10;
            }

            //Build ratioFinishedIn request
            byte[] ratioFinishedInrequest
                = Builder.GetCPMRatioFinishedIn(cursorAssociated, 1);
            byte[] ratioFinishedInResponse;
            //Send ratioFinishedIn message to server
            RequestSender sender = GetRequestSender(isClientConnected);
            bytesRead
                = sender.SendMessage(ratioFinishedInrequest,
                out ratioFinishedInResponse);
            if (sender == defaultSender)
            {
                // This means that disconnect message has been sent
                // through the pipe which does not have a connect In
                // sent across it.
                // If the sender.SendMessage() method is successful
                // Requirement 598 is validated
                wspTestSite.CaptureRequirement(598,
                   "In Windows the same pipe connection is used for the " +
                   "following messages, except when the error is returned" +
                   "in a CPMConnectOut message.");

            }
            // RequestSender objects uses path '\\pipe\\MSFTEWDS'
            // for the protocol transport
            wspTestSite.CaptureRequirement(3, @"All messages MUST be " +
                "transported using a named pipe: \\pipe\\MSFTEWDS");

            uint checkSum = 0;
            if (CPMRatioFinishedOutResponse != null)
            {
                int startingIndex = 0;
                uint msgId
                   = Helper.GetUInt(ratioFinishedInResponse,
                   ref startingIndex);
                uint msgStatus
                    = Helper.GetUInt(ratioFinishedInResponse,
                    ref startingIndex);
                if (msgStatus != 0)
                {
                    wspTestSite.CaptureRequirementIfAreEqual<int>(bytesRead,
                    Constants.SIZE_OF_HEADER, 619,
                    "Whenever an error occurs during processing of a " +
                    "message sent by a client, the server MUST respond " +
                    "with the message header (only) of the message sent " +
                    "by the client, keeping the _msg field intact.");
                    //If 4 byte Non Zero field is read as status
                    // The requirement 620 gets validated
                    wspTestSite.CaptureRequirement(620,
                        "Whenever an error occurs during processing of a " +
                        "message sent by a client, the server MUST set the " +
                        "_status field to the error code value.");
                }
                if ((msgId == (uint)MessageType.CPMRatioFinishedOut)
                    && (msgStatus == 0x00000000))
                {
                    validator.ValidateCPMRatioFinishedOutResponse(ratioFinishedInResponse,
                        checkSum, msgStatus);
                }

                if (wspTestSite.Properties.Get("ServerOSVersion").ToUpper() == "WIN7")
                {
                    if (!isCursorValid && msgStatus != 0x00000000)
                    {
                        //MS-WSP_R696
                        wspTestSite.CaptureRequirementIfAreEqual<uint>((uint)WspErrorCode.E_FAIL, msgStatus, 696,
                            "When the server receives a CPMRatioFinishedIn message request from a client, " +
                            "the server MUST report an E_FAIL (0x80004005) error if the cursor handle passed is not in the list of the clients cursor handles.");
                    }
                }

                CPMRatioFinishedOutResponse(msgStatus); // Fire response event
            }
        }
        /// <summary>
        /// This event is used to get the response from 
        /// CPMRatioFinishedIn request.
        /// </summary>
        public event CPMRatioFinishedOutResponseHandler CPMRatioFinishedOutResponse;

        #endregion

        #region CPMSetBindingsIn and CPMSetBindingsInResponse

        /// <summary>
        /// CPMSetBindingsIn() requests the bindings of columns to a rowset.
        /// </summary>
        /// <param name="isValidBinding">Indicates the client requesting 
        /// bindings of columns to a rowset.</param>
        /// <param name="isCursorValid">Indicates whether the cursor used
        /// is valid</param>
        public void CPMSetBindingsIn(bool isValidBinding, bool isCursorValid)
        {
            uint cursorAssociated = isCursorValid ? GetCursor(ClientMachineName) : 0;

            var setBindingsInMessage = Builder.GetCPMSetBindingsIn(cursorAssociated, out tableColumns, isValidBinding);
            this.currentCPMSetBindingsIn = setBindingsInMessage;

            CPMSetBindingsIn(
                setBindingsInMessage._hCursor,
                setBindingsInMessage._cbRow,
                setBindingsInMessage.cColumns,
                setBindingsInMessage.aColumns);
        }

        /// <summary>
        /// CPMSetBindingsIn() requests the bindings of columns to a rowset.
        /// </summary>
        public void CPMSetBindingsIn(CTableColumn[] aColumns)
        {
            CPMSetBindingsIn(
                GetCursor(ClientMachineName),
                MessageBuilder.RowWidth,
                (uint)aColumns.Length,
                aColumns);
        }

        /// <summary>
        /// Create and send CPMSetBindingsIn and expect response.
        /// </summary>
        public void CPMSetBindingsIn(uint _hCursor, uint _cbRow, uint cColumns, CTableColumn[] aColumns)
        {
            var client = GetClient(isClientConnected);

            Helper.UpdateTableColumns(aColumns);
            client.SendCPMSetBindingsIn(_hCursor, _cbRow, cColumns, aColumns);

            CPMSetBindingsOut response;
            client.ExpectMessage<CPMSetBindingsOut>(out response);

            if (client == defaultClient)
            {
                // This means that disconnect message has been sent
                // through the pipe which does not have a connect In
                // sent across it.
                // If the sender.SendMessage() method is successful
                // Requirement 598 is validated
                wspTestSite.CaptureRequirement(598,
                   "In Windows the same pipe connection is used for the " +
                   "following messages, except when the error is returned" +
                   "in a CPMConnectOut message.");

            }
            // RequestSender objects uses path '\\pipe\\MSFTEWDS'
            // for the protocol transport
            wspTestSite.CaptureRequirement(3, @"All messages MUST be " +
                         "transported using a named pipe: \\pipe\\MSFTEWDS");

            uint msgId = (uint)response.Header._msg;
            uint msgStatus = response.Header._status;
            if (msgStatus != 0)
            {
                //If 4 byte Non Zero field is read as status
                // The requirement 620 gets validated
                wspTestSite.CaptureRequirement(620,
                    "Whenever an error occurs during processing of a " +
                    "message sent by a client, the server MUST set the " +
                    "_status field to the error code value.");

            }
            if ((msgId == (uint)MessageType.CPMSetBindingsIn)
                && (msgStatus == 0x00000000))
            {
                validator.ValidateCPMSetBindingsInResponse(response);
            }
            CPMSetBindingsInResponse(msgStatus); // Fire Response Event
        }

        /// <summary>
        /// This event is used to get the response from 
        /// CPMSetBindingsIn request.
        /// </summary>
        public event CPMSetBindingsInResponseHandler CPMSetBindingsInResponse;

        #endregion

        #region CPMGetRowsIn and CPMGetRowsOut

        /// <summary>
        /// CPMGetRowsIn() message requests rows from a query.
        /// </summary>
        /// <param name="isCursorValid">Indicates the client requesting row has a valid cursor.</param>
        public void CPMGetRowsIn(bool isCursorValid)
        {
            uint cursorAssociated;
            if (isCursorValid)
            {
                cursorAssociated = GetCursor(ClientMachineName);
            }
            else
            {
                Random r = new Random();
                cursorAssociated = (uint)r.Next(50, 60);
            }

            CPMGetRowsIn(cursorAssociated, Builder.Parameter.RowsToTransfer, Builder.Parameter.EachRowSize, Builder.Parameter.BufferSize, 0, Builder.Parameter.EType, out CPMGetRowsOut _);
        }

        /// <summary>
        /// CPMGetRowsIn() message requests rows from a query.
        /// </summary>
        /// <param name="getRowsOut">The CPMGetRowsOut response from the server.</param>
        public void CPMGetRowsIn(out CPMGetRowsOut getRowsOut)
        {
            uint cursorAssociated = GetCursor(ClientMachineName);
            CPMGetRowsIn(cursorAssociated, Builder.Parameter.RowsToTransfer, Builder.Parameter.EachRowSize, Builder.Parameter.BufferSize, 0, Builder.Parameter.EType, out getRowsOut);
        }

        /// <summary>
        /// CPMGetRowsIn() message requests rows from a query.
        /// </summary>
        /// <param name="cursor">Representing the handle from the CPMCreateQueryOut message identifying the query for which to retrieve rows. </param>
        /// <param name="rowsToTransfer">Indicating the maximum number of rows that the client will receive in response to this message</param>
        /// <param name="rowWidth">Indicating the length of a row, in bytes</param>
        /// <param name="cbReadBuffer">This field MUST be set to the maximum of the value of _cbRowWidth or 1000 times the value of _cRowsToTransfer, rounded up to the nearest 512 byte multiple. The value MUST NOT exceed 0x00004000</param>
        /// <param name="fBwdFetch">Indicating the order in which to fetch the rows</param>
        /// <param name="eType">Type of SeekDescription</param>
        /// <param name="response">The CPMGetRowsOut response from the server.</param>
        public void CPMGetRowsIn(uint cursor, uint rowsToTransfer, uint rowWidth, uint cbReadBuffer, uint fBwdFetch, uint eType, out CPMGetRowsOut response)
        {
            CPMGetRowsIn(cursor, rowsToTransfer, rowWidth, cbReadBuffer, fBwdFetch, eType, null, null, out response);
        }

        /// <summary>
        /// CPMGetRowsIn() message requests rows from a query.
        /// </summary>
        /// <param name="cursor">Representing the handle from the CPMCreateQueryOut message identifying the query for which to retrieve rows. </param>
        /// <param name="rowsToTransfer">Indicating the maximum number of rows that the client will receive in response to this message</param>
        /// <param name="rowWidth">Indicating the length of a row, in bytes</param>
        /// <param name="cbReadBuffer">This field MUST be set to the maximum of the value of _cbRowWidth or 1000 times the value of _cRowsToTransfer, rounded up to the nearest 512 byte multiple. The value MUST NOT exceed 0x00004000</param>
        /// <param name="fBwdFetch">Indicating the order in which to fetch the rows</param>
        /// <param name="eType">Type of SeekDescription</param>
        /// <param name="seekDescription">The SeekDescription structure.</param>
        /// <param name="response">The CPMGetRowsOut response from the server.</param>
        public void CPMGetRowsIn(uint cursor, uint rowsToTransfer, uint rowWidth, uint cbReadBuffer, uint fBwdFetch, uint eType, IWspSeekDescription seekDescription, out CPMGetRowsOut response)
        {
            CPMGetRowsIn(cursor, rowsToTransfer, rowWidth, cbReadBuffer, fBwdFetch, eType, null, seekDescription, out response);
        }

        /// <summary>
        /// CPMGetRowsIn() message requests rows from a query.
        /// </summary>
        /// <param name="cursor">Representing the handle from the CPMCreateQueryOut message identifying the query for which to retrieve rows. </param>
        /// <param name="rowsToTransfer">Indicating the maximum number of rows that the client will receive in response to this message</param>
        /// <param name="rowWidth">Indicating the length of a row, in bytes</param>
        /// <param name="cbReadBuffer">This field MUST be set to the maximum of the value of _cbRowWidth or 1000 times the value of _cRowsToTransfer, rounded up to the nearest 512 byte multiple. The value MUST NOT exceed 0x00004000</param>
        /// <param name="fBwdFetch">Indicating the order in which to fetch the rows</param>
        /// <param name="eType">Type of SeekDescription</param>
        /// <param name="chapt">The handle to a chapter of hierarchical results.</param>
        /// <param name="seekDescription">The SeekDescription structure.</param>
        /// <param name="response">The CPMGetRowsOut response from the server.</param>
        public void CPMGetRowsIn(uint cursor, uint rowsToTransfer, uint rowWidth, uint cbReadBuffer, uint fBwdFetch, uint eType, uint? chapt, IWspSeekDescription seekDescription, out CPMGetRowsOut response)
        {
            response = null;

            // Get hold of appropriate Sender (Pipe with/without connection)
            var client = GetClient(isClientConnected);
            var getRowsInMessage = Builder.GetCPMGetRowsIn(cursor, rowsToTransfer, rowWidth, cbReadBuffer, fBwdFetch, eType, chapt, seekDescription, out rowsInReserve);
            var getRowsInMessageBytes = Helper.ToBytes(getRowsInMessage);

            client.SendCPMGetRowsIn(
                getRowsInMessage._hCursor,
                getRowsInMessage._cRowsToTransfer,
                getRowsInMessage._cbRowWidth,
                getRowsInMessage._cbSeek,
                getRowsInMessage._cbReserved,
                getRowsInMessage._cbReadBuffer,
                getRowsInMessage._ulClientBase,
                getRowsInMessage._fBwdFetch,
                getRowsInMessage.eType,
                getRowsInMessage._chapt,
                getRowsInMessage.SeekDescription);

            if (client == defaultClient)
            {
                // This means that disconnect message has been sent
                // through the pipe which does not have a connect In
                // sent across it.
                // If the sender.SendMessage() method is successful
                // Requirement 598 is validated
                wspTestSite.CaptureRequirement(598,
                   "In Windows the same pipe connection is used for the " +
                   "following messages, except when the error is returned" +
                   "in a CPMConnectOut message.");
            }
            // RequestSender objects uses path '\\pipe\\MSFTEWDS'
            // for the protocol transport
            wspTestSite.CaptureRequirement(3, @"All messages MUST be " +
                "transported using a named pipe: \\pipe\\MSFTEWDS");

            if (CPMGetRowsOutResponse != null)
            {
                client.ExpectMessage<CPMGetRowsOut>(out response);

                uint offsetUsed = GetOffsetUsed();
                validator.ValidateCPMGetRowsOutResponse(response);

                // Fire Response Event
                CPMGetRowsOutResponse(response.Header._status);
            }
        }

        /// <summary>
        /// This event is used to get the response from CPMGetRowsIn request.
        /// </summary>
        public event CPMGetRowsOutResponseHandler CPMGetRowsOutResponse;

        #endregion

        #region CPMFetchValueIn and CPMFetchValueOut

        /// <summary>
        /// CPMFetchValueIn() requests for the property value that was too large to return in a rowset.
        /// </summary>
        /// <param name="isCursorValid">Indicates whether the client has a valid cursor associated</param>
        public void CPMFetchValueIn(bool isCursorValid)
        {
            uint workId = 0xfffffff0;
            uint cbSoFar = 0;
            uint cbChunk = Convert.ToUInt32(wspTestSite.Properties["CbChunk"]);

            CPMFetchValueIn(workId, cbSoFar, cbChunk, WspConsts.System_ItemName, out _);
        }

        /// <summary>
        /// CPMFetchValueIn() requests for the property value that was too large to return in a rowset.
        /// </summary>
        /// <param name="workId">The document work Id.</param>
        /// <param name="propSpec">The property to fetch.</param>
        /// <param name="serializedPropertyValue">The serialized property value</param>
        public void CPMFetchValueIn(uint workId, CFullPropSpec propSpec, out SERIALIZEDPROPERTYVALUE? serializedPropertyValue)
        {
            uint cbChunk = 0x4000;
            CPMFetchValueIn(workId, cbChunk, propSpec, out serializedPropertyValue);
        }

        /// <summary>
        /// CPMFetchValueIn() requests for the property value that was too large to return in a rowset.
        /// Multiple CPMFetchValueIn messages may be sent if cbChunk indicates a buffer that is not enough to be filled by the whole property value. 
        /// </summary>
        /// <param name="workId">The document work Id.</param>
        /// <param name="cbChunk">The buffer length for a single CPMFetchValueIn message.</param>
        /// <param name="propSpec">The property to fetch.</param>
        /// <param name="serializedPropertyValue">The serialized property value</param>
        public void CPMFetchValueIn(uint workId, uint cbChunk, CFullPropSpec propSpec, out SERIALIZEDPROPERTYVALUE? serializedPropertyValue)
        {
            serializedPropertyValue = null;

            var consequentByteArrays = new List<byte[]>();
            uint cbSofar = 0;
            while (true)
            {
                CPMFetchValueIn(workId, cbSofar, cbChunk, propSpec, out var fetchValueOut);

                if (fetchValueOut._fValueExists == 1)
                {
                    consequentByteArrays.Add(fetchValueOut.vValue);
                }

                if (fetchValueOut._fMoreExists == 1)
                {
                    cbSofar += fetchValueOut._cbValue;
                }
                else
                {
                    break;
                }
            }

            if (consequentByteArrays.Any())
            {
                serializedPropertyValue = SERIALIZEDPROPERTYVALUE.GetSerializedPropertyValue(consequentByteArrays);
            }
        }

        /// <summary>
        /// CPMFetchValueIn() requests for the property value that was too large to return in a rowset.
        /// </summary>
        /// <param name="workId">The document work Id.</param>
        /// <param name="cbSoFar">The current offset of the property value.</param>
        /// <param name="cbChunk">The buffer length for a single CPMFetchValueIn message.</param>
        /// <param name="propSpec">The property to fetch.</param>
        /// <param name="fetchValueOut">The CPMFetchValueOut response to the request.</param>
        public void CPMFetchValueIn(uint workId, uint cbSoFar, uint cbChunk, CFullPropSpec propSpec, out CPMFetchValueOut fetchValueOut)
        {
            fetchValueOut = null;
            byte[] value = null;

            CPMFetchValueIn fetchValueIn = Builder.GetCPMFetchValueIn(workId, cbSoFar, cbChunk, propSpec);
            var tempBuffer = new WspBuffer();
            fetchValueIn.ToBytes(tempBuffer);
            var fetchValueInBytes = tempBuffer.GetBytes();

            uint checkSum = GetChecksumField(fetchValueInBytes);
            RequestSender sender = GetRequestSender(isClientConnected);
            bytesRead = sender.SendMessage(fetchValueInBytes, out var fetchValueOutBytes);
            value = new byte[bytesRead];
            Array.Copy(fetchValueOutBytes, 0, value, 0, bytesRead);
            int index = 4;
            // Read the message Response 
            uint msgStatus = Helper.GetUInt(fetchValueOutBytes, ref index);
            if (msgStatus == 0)
            {
                validator.ValidateCPMFetchValueOutResponse(value, checkSum, cbSoFar, cbChunk);

                fetchValueOut = new CPMFetchValueOut();
                fetchValueOut.FromBytes(new WspBuffer(fetchValueOutBytes));
            }

            CPMFetchValueOutResponse(msgStatus);
        }

        /// <summary>
        /// this event is used to get the response from 
        /// cpmfetchvaluein request.
        /// </summary>
        public event CPMFetchValueOutResponseHandler CPMFetchValueOutResponse;

        #endregion

        #region CPMCiStateInOut and CPMCiStateInOutResponse

        /// <summary>
        /// CPMCiStateInOut() requests the information about the 
        /// state of the Windows Search Service.
        /// </summary>
        public void CPMCiStateInOut()
        {
            var ciStateInOutMessage = Builder.GetCPMCiStateInOut();
            var ciStateInOutMessageBytes = Helper.ToBytes(ciStateInOutMessage);
            byte[] ciStateInOutResponseMessageBytes;
            RequestSender sender
                = GetRequestSender(isClientConnected); //Get the Sender
            bytesRead = sender.SendMessage(ciStateInOutMessageBytes, out ciStateInOutResponseMessageBytes);
            if (sender == defaultSender)
            {
                // This means that disconnect message has been sent
                // through the pipe which does not have a connect In
                // sent across it.
                // If the sender.SendMessage() method is successful
                // Requirement 598 is validated
                wspTestSite.CaptureRequirement(598,
                   "In Windows the same pipe connection is used for the " +
                   "following messages, except when the error is returned" +
                   "in a CPMConnectOut message.");

            }
            // RequestSender objects uses path '\\pipe\\MSFTEWDS'
            // for the protocol transport
            wspTestSite.CaptureRequirement(3, @"All messages MUST be " +
                  "transported using a named pipe: \\pipe\\MSFTEWDS");

            if (ciStateInOutResponseMessageBytes != null)
            {
                var header = new WspMessageHeader();
                Helper.FromBytes(ref header, ciStateInOutResponseMessageBytes);

                uint msgId = (uint)header._msg;
                uint msgStatus = header._status;
                if (msgStatus != 0)
                {
                    wspTestSite.CaptureRequirementIfAreEqual<int>(bytesRead,
                        Constants.SIZE_OF_HEADER, 619,
                        "Whenever an error occurs during processing of a " +
                        "message sent by a client, the server MUST respond " +
                        "with the message header (only) of the message sent " +
                        "by the client, keeping the _msg field intact.");
                    //If 4 byte Non Zero field is read as status
                    // The requirement 620 gets validated
                    wspTestSite.CaptureRequirement(620,
                        "Whenever an error occurs during processing of a " +
                        "message sent by a client, the server MUST set the " +
                        "_status field to the error code value.");
                }
                if (msgStatus == 0)
                {
                    var response = new CPMCiStateInOut();
                    Helper.FromBytes(ref response, ciStateInOutResponseMessageBytes);

                    validator.ValidateCPMCiStateInOutResponse(response);
                }
                CPMCiStateInOutResponse(msgStatus); // Fire Response Event
            }
        }
        /// <summary>
        /// This event is used to get the response from 
        /// CPMCiStateInOut request.
        /// </summary>
        public event CPMCiStateInOutResponseHandler CPMCiStateInOutResponse;

        #endregion

        #region CPMGetNotify and CPMSendNotifyOut

        /// <summary>
        /// CPMGetNotify() requests that the client wants to be notified
        /// of rowset changes.
        /// </summary>
        /// <param name="isCursorValid">Indicates whether the 
        /// client has valid query cursor</param>
        public void CPMGetNotify(bool isCursorValid)
        {
            uint cursorAssociated = 0;
            if (isCursorValid)
            {
                cursorAssociated = GetCursor(ClientMachineName);
            }
            else
            {
                cursorAssociated = uint.MaxValue - 10;
            }

            byte[] getNotifyMessage = Builder.GetCPMGetNotify();
            byte[] sendNotifyOut;
            uint checkSum = 0;
            RequestSender sender
                = GetRequestSender(isClientConnected); //Get the Sender
            sender.SendMessage(getNotifyMessage, out sendNotifyOut);
            if (sender == defaultSender)
            {
                // This means that disconnect message has been sent
                // through the pipe which does not have a connect In
                // sent across it.
                // If the sender.SendMessage() method is successful
                // Requirement 598 is validated
                wspTestSite.CaptureRequirement(598,
                   "In Windows the same pipe connection is used for the " +
                   "following messages, except when the error is returned" +
                   "in a CPMConnectOut message.");

            }
            // RequestSender objects uses path '\\pipe\\MSFTEWDS'
            // for the protocol transport
            wspTestSite.CaptureRequirement(3, @"All messages MUST be " +
                "transported using a named pipe: \\pipe\\MSFTEWDS");

            if (CPMSendNotifyOutResponse != null)
            {
                int startingIndex = 0;
                uint msgId
                    = Helper.GetUInt(sendNotifyOut, ref startingIndex);
                uint msgStatus
                    = Helper.GetUInt(sendNotifyOut, ref startingIndex);
                if (msgStatus != 0)
                {
                    if (wspTestSite.Properties.Get("ServerOSVersion").ToUpper() != "WIN7")
                    {
                        wspTestSite.CaptureRequirementIfAreEqual<int>(bytesRead,
                        Constants.SIZE_OF_HEADER, 619,
                        "Whenever an error occurs during processing of a " +
                        "message sent by a client, the server MUST respond " +
                        "with the message header (only) of the message sent " +
                        "by the client, keeping the _msg field intact.");
                    }

                    //If 4 byte Non Zero field is read as status
                    // The requirement 620 gets validated
                    wspTestSite.CaptureRequirement(620,
                        "Whenever an error occurs during processing of a " +
                        "message sent by a client, the server MUST set the " +
                        "_status field to the error code value.");
                }
                if ((msgId == (uint)MessageType.CPMSendNotifyOut)
                    && (msgStatus == 0x00000000))
                {
                    validator.ValidateCPMSendNotifyOutResponse(sendNotifyOut, checkSum);
                }
                CPMSendNotifyOutResponse(msgStatus); // Fire Response Event
            }
        }

        /// <summary>
        /// This event is used to get the response from CPMGetNotify request.
        /// </summary>
        public event CPMSendNotifyOutResponseHandler CPMSendNotifyOutResponse;

        #endregion

        #region CPMFreeCursorIn and CPMFreeCursorOut

        /// <summary>
        /// CPMFreeCursorIn() requests to release a cursor.
        /// </summary>
        /// <param name="isCursorValid">Indicates the client requesting to release a cursor.</param>
        public void CPMFreeCursorIn(bool isCursorValid)
        {
            CPMFreeCursorIn(isCursorValid, out _);
        }

        /// <summary>
        /// CPMFreeCursorIn() requests to release a cursor.
        /// </summary>
        /// <param name="isCursorValid">Indicates the client requesting to release a cursor.</param>
        /// <param name="freeCursorOut">The CPMFreeCursorOut response from the server.</param> 
        public void CPMFreeCursorIn(bool isCursorValid, out CPMFreeCursorOut freeCursorOut)
        {
            uint cursorAssociated = 0;
            if (isCursorValid)
            {
                cursorAssociated = GetCursor(ClientMachineName);
            }
            else
            {
                cursorAssociated = uint.MaxValue - 10;
            }

            CPMFreeCursorIn(cursorAssociated, out freeCursorOut);
        }

        /// <summary>
        /// CPMFreeCursorIn() requests to release a cursor.
        /// </summary>
        /// <param name="cursor">The handle to the query cursor.</param>
        public void CPMFreeCursorIn(uint cursor)
        {
            CPMFreeCursorIn(cursor, out _);
        }

        /// <summary>
        /// CPMFreeCursorIn() requests to release a cursor.
        /// </summary>
        /// <param name="cursor">The handle to the query cursor.</param>
        /// <param name="freeCursorOut">The CPMFreeCursorOut response from the server.</param>
        public void CPMFreeCursorIn(uint cursor, out CPMFreeCursorOut freeCursorOut)
        {
            freeCursorOut = null;
            CPMFreeCursorIn freeCursorIn = Builder.GetCPMFreeCursorIn(cursor);
            RequestSender sender = GetRequestSender(isClientConnected);
            var tempBuffer = new WspBuffer();
            freeCursorIn.ToBytes(tempBuffer);
            sender.SendMessage(tempBuffer.GetBytes(), out byte[] freeCursorOutBytes);
            if (sender == defaultSender)
            {
                // This means that disconnect message has been sent
                // through the pipe which does not have a connect In
                // sent across it.
                // If the sender.SendMessage() method is successful
                // Requirement 598 is validated
                wspTestSite.CaptureRequirement(598,
                   "In Windows the same pipe connection is used for the " +
                   "following messages, except when the error is returned" +
                   "in a CPMConnectOut message.");

            }
            // RequestSender objects uses path '\\pipe\\MSFTEWDS'
            // for the protocol transport
            wspTestSite.CaptureRequirement(3, @"All messages MUST be " +
                "transported using a named pipe: \\pipe\\MSFTEWDS");
            int startingIndex = 0;
            uint msgId = Helper.GetUInt(freeCursorOutBytes, ref startingIndex);
            uint errorCode = Helper.GetUInt(freeCursorOutBytes, ref startingIndex);
            if (CPMFreeCursorOutResponse != null)
            {
                uint checkSumZero = 0x00000000;
                if ((msgId == (uint)MessageType.CPMFreeCursorOut) && (errorCode == 0x00000000))
                {
                    freeCursorOut = new CPMFreeCursorOut();
                    freeCursorOut.FromBytes(new WspBuffer(freeCursorOutBytes));
                    validator.ValidateCPMFreeCursorOutResponse(freeCursorOutBytes, checkSumZero);
                }

                CPMFreeCursorOutResponse(errorCode);
            }
        }

        /// <summary>
        /// This event is used to get the response from 
        /// CPMFreeCursorIn request.
        /// </summary>
        public event CPMFreeCursorOutResponseHandler CPMFreeCursorOutResponse;

        #endregion

        #region CPMDisconnect

        /// <summary>
        /// CPMDisconnect() request is sent to end the connection with the server.
        /// </summary>
        public void CPMDisconnect()
        {
            CPMDisconnect(true);
        }

        /// <summary>
        /// CPMDisconnect() request is sent to end the connection with the server.
        /// </summary>
        /// <param name="removeClient">Remove the client associated to the current connection.</param>
        public void CPMDisconnect(bool removeClient)
        {
            byte[] disconnectResponse = null;
            RequestSender sender = null;
            byte[] disconnectMessage = Builder.GetCPMDisconnect();
            if (connectedClients.ContainsKey(ClientMachineName))
            {
                sender = connectedClients[ClientMachineName].Sender;
            }
            else
            {
                sender = defaultSender;
            }
            int bytesRead = sender.SendMessage(disconnectMessage,
                out disconnectResponse);
            if (sender == defaultSender)
            {
                // This means that disconnect message has been sent
                // through the pipe which does not have a connect In
                // sent across it.
                // If the sender.SendMessage() method is successful
                // Requirement 598 is validated
                wspTestSite.CaptureRequirement(598,
                   "In Windows the same pipe connection is used for the " +
                   "following messages, except when the error is returned" +
                   "in a CPMConnectOut message.");

            }
            // RequestSender objects uses path '\\pipe\\MSFTEWDS'
            // for the protocol transport
            wspTestSite.CaptureRequirement(3, @"All messages MUST be " +
                "transported using a named pipe: \\pipe\\MSFTEWDS");

            if (connectedClients.ContainsKey(ClientMachineName) && removeClient)
            {
                // Remove the Request Sender associated with the client
                disconnectedClients.Add(connectedClients[ClientMachineName]);
                connectedClients.Remove(ClientMachineName);
            }
        }

        #endregion

        #region CPMFindIndicesIn and CPMFindIndicesOut

        /// <summary>
        /// CPMFindIndicesIn() request the rowset position of the next occurrence of a document identifier
        /// </summary>
        /// <param name="isCursorValid"></param>
        public void CPMFindIndicesIn(bool isCursorValid)
        {
            uint cWids = 1;
            uint cDepthPrev = 0;
            byte[] findIndicesOut = null;
            byte[] value = null;
            byte[] findIndicesIn
                = Builder.GetCPMFindIndicesIn(cWids, cDepthPrev);
            uint checkSum = GetChecksumField(findIndicesIn);
            RequestSender sender = GetRequestSender(isClientConnected);
            bytesRead = sender.SendMessage(findIndicesIn, out findIndicesOut);
            value = new byte[bytesRead];
            Array.Copy(findIndicesOut, 0, value, 0, bytesRead);
            int startingIndex = 0;
            // Read the message Response
            uint msgId = Helper.GetUInt(findIndicesOut, ref startingIndex);
            uint errorCode = Helper.GetUInt(findIndicesOut, ref startingIndex);

            //If HRESULT is DB_S_ENDOFROWSET and the thirty-first bit is not set, it should be success
            if (errorCode != 0)
            {
                startingIndex = 4;
                if (VerifyHResult(findIndicesOut, ref startingIndex))
                {
                    errorCode = 0;
                    //if return code is DB_S_ENDOFROWSET, it is not equal 0, but it still shows successfully.
                    Site.CaptureRequirement(1201,
                        "[Windows Search Protocol (WSP) messages indicate success two ways,second way]An HRESULT success value, " +
                        "[such as DB_S_ENDOFROWSET]in whichAn HRESULT [An HRESULT ] the thirty-first bit is not set.");
                }
            }

            if (errorCode == 0)
            {
                validator.ValidateCPMFindIndicesOutResponse(value, checkSum, cDepthPrev);
            }

            CPMFindIndicesOutResponse(errorCode);
        }

        /// <summary>
        /// This event is used to get the response from 
        /// CPMFindIndicesIn request.
        /// </summary>
        public event CPMFindIndicesOutResponseHandler CPMFindIndicesOutResponse;

        #endregion

        #region CPMGetRowsetNotidyIn and CPMGetRowsetNotidyOut

        /// <summary>
        /// CPMGetRowsetNotifyIn message requests the next rowset event from the server if available
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="additionalRowsetEvent"></param>
        public void CPMGetRowsetNotifyIn(int eventType, bool additionalRowsetEvent)
        {
            IWspSutAdapter sutAdapter = Site.GetAdapter<IWspSutAdapter>();
            string fileName;
            int sutStatus;
            switch (eventType)
            {
                case (int)EventType.PROPAGATE_ADD:
                    fileName = wspTestSite.Properties["NewFile1"];
                    //Create a file on remote server
                    sutStatus = sutAdapter.CreateFile(fileName);
                    if (sutStatus != 0)
                    {
                        wspTestSite.Log.Add(LogEntryKind.Comment, "File created failed on server");
                        return;
                    }
                    if (additionalRowsetEvent)
                    {
                        fileName = wspTestSite.Properties["NewFile2"];
                        //Create a file on remote server
                        sutStatus = sutAdapter.CreateFile(fileName);

                        if (sutStatus != 0)
                        {
                            wspTestSite.Log.Add(LogEntryKind.Comment, "File created failed on server");
                            return;
                        }
                    }
                    break;
                case (int)EventType.PROPAGATE_DELETE:
                    fileName = wspTestSite.Properties["ExistFile1"];
                    //Delete a file on remote server
                    sutStatus = sutAdapter.DeleteFile(fileName);
                    if (sutStatus != 0)
                    {
                        wspTestSite.Log.Add(LogEntryKind.Comment, "File deleted failed on server");
                        return;
                    }
                    if (additionalRowsetEvent)
                    {
                        fileName = wspTestSite.Properties["ExistFile2"];
                        //Delete a file on remote server
                        sutStatus = sutAdapter.DeleteFile(fileName);
                        if (sutStatus != 0)
                        {
                            wspTestSite.Log.Add(LogEntryKind.Comment, "File deleted failed on server");
                            return;
                        }
                    }
                    break;
                case (int)EventType.PROPAGATE_MODIFY:
                    fileName = wspTestSite.Properties["ExistFile3"];
                    //Modify a file on remote server
                    sutStatus = sutAdapter.ModifyFile(fileName);
                    if (sutStatus != 0)
                    {
                        wspTestSite.Log.Add(LogEntryKind.Comment, "File operation failed on server while modifying");
                        return;
                    }
                    if (additionalRowsetEvent)
                    {
                        fileName = wspTestSite.Properties["ExistFile4"];
                        //Modify a file on remote server
                        sutStatus = sutAdapter.ModifyFile(fileName);
                        if (sutStatus != 0)
                        {
                            wspTestSite.Log.Add(LogEntryKind.Comment, "File operation failed on server while modifying");
                            return;
                        }
                    }
                    break;
                case (int)EventType.PROPAGATE_NONE:
                default:
                    break;
            }

            byte[] getRowsetNotifyOutMessage = null;
            RequestSender sender = GetRequestSender(isClientConnected);
            byte[] getRowsetNotifyInMessage = Builder.GetCPMGetRowsetNotifyIn();
            int bytesRead = sender.SendMessage(getRowsetNotifyInMessage,
                out getRowsetNotifyOutMessage);
            byte[] value = null;
            value = new byte[bytesRead];
            Array.Copy(getRowsetNotifyOutMessage, 0, value, 0, bytesRead);

            uint checkSum = GetChecksumField(getRowsetNotifyInMessage);

            int startingIndex = 0;
            uint msgId = Helper.GetUInt(getRowsetNotifyOutMessage, ref startingIndex);
            uint errorCode = Helper.GetUInt(getRowsetNotifyOutMessage, ref startingIndex);

            if (errorCode == 0)
            {
                validator.ValidateCPMGetRowsetNotifyOutResponse(value, checkSum, eventType, additionalRowsetEvent);
            }
            CPMGetRowsetNotifyOutResponse(errorCode);

            //Clean the environment for the next CPMGetRowsetNotifyIn call
            CleanUpSettings(eventType, additionalRowsetEvent);
        }

        /// <summary>
        /// Reseting the files for CPMGetRowsetNotifyIn method
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="additionalRowsetEvent"></param>
        public void CleanUpSettings(int eventType, bool additionalRowsetEvent)
        {
            string fileName;
            IWspSutAdapter sutAdapter = Site.GetAdapter<IWspSutAdapter>();
            int sutStatus;
            switch (eventType)
            {
                case (int)EventType.PROPAGATE_ADD:
                    if (additionalRowsetEvent)
                    {
                        fileName = wspTestSite.Properties["NewFile2"];
                        //Delete file on the remote server
                        sutStatus = sutAdapter.DeleteFile(fileName);

                        if (sutStatus != 0)
                        {
                            wspTestSite.Log.Add(LogEntryKind.Comment, "File deleted failed on server");
                            return;
                        }
                    }
                    fileName = wspTestSite.Properties["NewFile1"];
                    //Delete file on the remote server
                    sutStatus = sutAdapter.DeleteFile(fileName);

                    if (sutStatus != 0)
                    {
                        wspTestSite.Log.Add(LogEntryKind.Comment, "File deleted failed on server");
                        return;
                    }
                    break;
                case (int)EventType.PROPAGATE_DELETE:
                    fileName = wspTestSite.Properties["ExistFile1"];
                    //Create file on the remote server
                    sutStatus = sutAdapter.CreateFile(fileName);
                    if (sutStatus != 0)
                    {
                        wspTestSite.Log.Add(LogEntryKind.Comment, "File created failed on server");
                        return;
                    }
                    if (additionalRowsetEvent)
                    {
                        fileName = wspTestSite.Properties["ExistFile2"];
                        //Create file on the remote server
                        sutStatus = sutAdapter.CreateFile(fileName);
                        if (sutStatus != 0)
                        {
                            wspTestSite.Log.Add(LogEntryKind.Comment, "File created failed on server");
                            return;
                        }
                    }
                    break;
                case (int)EventType.PROPAGATE_MODIFY:
                    if (additionalRowsetEvent)
                    {
                        fileName = wspTestSite.Properties["ExistFile4"];
                        //Delete file on the remote server
                        sutStatus = sutAdapter.DeleteFile(fileName);
                        if (sutStatus != 0)
                        {
                            wspTestSite.Log.Add(LogEntryKind.Comment, "File deleted failed on server");
                            return;
                        }
                    }
                    fileName = wspTestSite.Properties["ExistFile3"];
                    //Delete file on the remote server
                    sutStatus = sutAdapter.DeleteFile(fileName);
                    if (sutStatus != 0)
                    {
                        wspTestSite.Log.Add(LogEntryKind.Comment, "File deleted failed on server");
                        return;
                    }
                    //Create file on the remote server
                    sutStatus = sutAdapter.CreateFile(fileName);
                    if (sutStatus != 0)
                    {
                        wspTestSite.Log.Add(LogEntryKind.Comment, "File created failed on server");
                        return;
                    }
                    if (additionalRowsetEvent)
                    {
                        fileName = wspTestSite.Properties["ExistFile4"];
                        //Create file on the remote server
                        sutStatus = sutAdapter.CreateFile(fileName);
                        if (sutStatus != 0)
                        {
                            wspTestSite.Log.Add(LogEntryKind.Comment, "File created failed on server");
                            return;
                        }
                    }
                    break;
                case (int)EventType.PROPAGATE_NONE:
                default:
                    break;
            }
        }

        /// <summary>
        /// Verify the error code if it is the HResult success value
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="startingIndex"></param>
        /// <returns></returns>
        public bool VerifyHResult(byte[] bytes, ref int startingIndex)
        {
            bool success = false;
            byte[] temp = new byte[Constants.SIZE_OF_UINT];
            temp = Helper.GetData(bytes, ref startingIndex, Constants.SIZE_OF_UINT);
            if (Convert.ToInt32(temp[3]) == 0)
            {
                //if the last bit is not set, it should be shown as success
                success = true;
            }

            return success;
        }

        /// <summary>
        /// This event is used to get the response from 
        /// CPMGetRowsetNotifyIn request.
        /// </summary>
        public event CPMGetRowsetNotifyOutResponseHandler CPMGetRowsetNotifyOutResponse;

        #endregion

        #region CPMGetScopeStatisticsIn and CMPGetScopeStatisticsOut

        /// <summary>
        /// CPMGetScopeStatisticsIn() request that statistics regarding the number of indexed items
        /// </summary>
        public void CPMGetScopeStatisticsIn()
        {
            byte[] statusInMessage
                = Builder.GetCPMGetScopeStatisticsIn();
            byte[] statusOutMessage;
            RequestSender sender = GetRequestSender(isClientConnected);
            bytesRead
                = sender.SendMessage(statusInMessage, out statusOutMessage);
            if (sender == defaultSender)
            {
                //This means that disconnect message has been sent
                // through the pipe which does not have a connect In
                // sent across it.
                // If the sender.SendMessage() method is successful
                // Requirement 598 is validated
                wspTestSite.CaptureRequirement(598,
                   "In Windows the same pipe connection is used for the " +
                   "following messages, except when the error is returned" +
                   "in a CPMConnectOut message.");

            }
            // RequestSender objects uses path '\\pipe\\MSFTEWDS'
            // for the protocol transport
            wspTestSite.CaptureRequirement(3, @"All messages MUST be " +
                "transported using a named pipe: \\pipe\\MSFTEWDS");

            uint checkSum = 0;
            if (CPMGetQueryStatusOutResponse != null)
            {
                int startingIndex = 0;
                uint msgId = Helper.GetUInt(statusOutMessage,
                    ref startingIndex);
                uint msgStatus = Helper.GetUInt(statusOutMessage,
                    ref startingIndex);


                if ((msgId == (uint)MessageType.CPMGetScopeStatisticsOut)
                    && (msgStatus == 0x00000000))
                {
                    validator.ValidateCPMGetScopeStatisticsOutResponse(statusOutMessage, checkSum, msgStatus);

                    //if message response successfully, it must populated with the appropriate statistics
                    wspTestSite.CaptureRequirement(1153,
                        "[When the server receives a CPMGetScopeStatisticsIn message request from a client, " +
                        "the server MUST]The CPMGetScopeStatisticsOut message MUST be populated with the appropriate statistics.");
                }

                CPMGetScopeStatisticsOutResponse(msgStatus);
            }
        }
        /// <summary>
        /// This is to get the response for CMPGetScopeStatistics
        /// </summary>
        public event CPMGetScopeStatisticsOutResponseHandler CPMGetScopeStatisticsOutResponse;

        #endregion

        #region CPMSetScopePrioritizationIn and CPMSetScopePrioritizationOut

        /// <summary>
        /// CPMSetScopePrioritizationIn() request that server prioritize indexing of items that may be relevant to the originating query
        /// at a rate specified in the message
        /// </summary>
        /// <param name="priority"></param>
        public void CPMSetScopePrioritizationIn(uint priority)
        {
            byte[] setScopePrioritizationOutMessage = null;
            RequestSender sender = GetRequestSender(isClientConnected);
            uint eventFrequency = 0;
            if (priority == (uint)Priority.PRIORITY_LEVEL_DEFAULT)
            {
                eventFrequency = 0;
            }
            else
            {
                eventFrequency = 0x00001000;
            }
            byte[] setScopePrioritizationInMessage = Builder.GetCPMSetScopePrioritizationIn(priority, eventFrequency);

            int bytesRead = sender.SendMessage(setScopePrioritizationInMessage,
                out setScopePrioritizationOutMessage);
            byte[] value = null;
            value = new byte[bytesRead];
            Array.Copy(setScopePrioritizationOutMessage, 0, value, 0, bytesRead);

            uint checkSum = GetChecksumField(setScopePrioritizationInMessage);
            // Read the message Response           

            int startingIndex = 0;
            uint msgId = Helper.GetUInt(setScopePrioritizationOutMessage, ref startingIndex);
            uint errorCode = Helper.GetUInt(setScopePrioritizationOutMessage, ref startingIndex);

            if (errorCode == 0)
            {
                validator.ValidateCPMSetScopePrioritizationOutResponse(value, checkSum, eventFrequency);
            }
            CPMSetScopePrioritizationOutResponse(errorCode);
        }

        /// <summary>
        /// This event is used to get the response from 
        /// CPMSetScopePrioritizationIn request.
        /// </summary>
        public event CPMSetScopePrioritizationOutResponseHandler CPMSetScopePrioritizationOutResponse;

        #endregion

        #region CPMRestartPositionIn request and response

        /// <summary>
        /// CPMRestartPositionIn() request directs the server to move the fetch position for a cursor to the beginning of the chapter. 
        /// </summary>
        /// <param name="_hCursor">The query handle.</param>
        /// <param name="_chapt">The chapter handle.</param>
        public void CPMRestartPositionIn(uint _hCursor, uint _chapt)
        {
            RequestSender sender = GetRequestSender(isClientConnected);

            byte[] restartPositionInRequset = Builder.GetCPMRestartPositionIn(_hCursor, _chapt);

            int bytesRead = sender.SendMessage(restartPositionInRequset, out byte[] restartPositionInResponse);
            byte[] value = null;
            value = new byte[bytesRead];
            Array.Copy(restartPositionInResponse, 0, value, 0, bytesRead);
            uint checkSum = GetChecksumField(restartPositionInResponse);

            // Read the message Response           
            int startingIndex = 0;
            uint msgId = Helper.GetUInt(restartPositionInResponse, ref startingIndex);
            uint errorCode = Helper.GetUInt(restartPositionInResponse, ref startingIndex);

            if (errorCode == 0)
            {
                validator.ValidateCPMRestartPositionInResponse(value, checkSum);
            }

            CPMRestartPositionInResponse(errorCode);
        }

        /// <summary>
        /// This event is used to get the response from CPMRestartPositionIn request.
        /// </summary>
        public event CPMRestartPositionInResponseHadler CPMRestartPositionInResponse;

        #endregion

        #endregion

        #region Helper Methods

        /// <summary>
        /// Get server's OS version
        /// </summary>
        /// <param name="platform">Represent server's platform type</param>
        /// <returns>The call must return true which indicates success</returns>
        public bool GetServerPlatform(out SkuOsVersion platform)
        {
            platform = CheckSkuOsVersion();
            this.platform = platform;

            return true;
        }

        #region Check SKU OS Version

        /// <summary>
        /// Check SKU version.
        /// </summary>
        /// <returns>SKU version</returns>
        private SkuOsVersion CheckSkuOsVersion()
        {
            string skuOsVersion = wspTestSite.Properties.Get("ServerOSVersion");

            if (skuOsVersion.ToLower() != "NonWin".ToLower() &&
                skuOsVersion.ToLower() != "W2K3".ToLower() &&
                skuOsVersion.ToLower() != "W2K8".ToLower() &&
                skuOsVersion.ToLower() != "Win7".ToLower())
            {
                wspTestSite.Assert.Fail("Unknown SKU OS version: {0}", skuOsVersion);
            }

            switch (skuOsVersion.ToLower())
            {
                case "nonwin":
                    Properties.SkuOsVersion = SkuOsVersion.NonWindows;
                    break;
                case "w2k3":
                    Properties.SkuOsVersion = SkuOsVersion.Win2K3;
                    break;
                case "w2k8":
                    Properties.SkuOsVersion = SkuOsVersion.Win2K8;
                    break;
                case "win7":
                    Properties.SkuOsVersion = SkuOsVersion.Win2K8R2;
                    break;
                default:
                    break;
            }

            return Properties.SkuOsVersion;
        }

        #endregion

        /// <summary>
        /// Fetches Checksum field from a WSP message
        /// </summary>
        /// <param name="wspMessage">WSP message BLOB</param>
        /// <returns>Checksum</returns>
        private uint GetChecksumField(byte[] wspMessage)
        {
            int index = 8;
            uint checksumField = Helper.GetUInt(wspMessage, ref index);
            return checksumField;
        }

        /// <summary>
        /// Retrieves the cursor associated with the client
        /// </summary>
        /// <param name="machineName">client machine name</param>
        /// <returns>cursor associated with the query</returns>
        public uint GetCursor(string machineName)
        {
            uint cursor = 0;
            if (cursorMap.ContainsKey(machineName))
            {
                cursor = cursorMap[machineName];
            }
            return cursor;
        }

        /// <summary>
        /// Returns a WspClient object to send message to Server 
        /// </summary>
        /// <param name="isClientConnected">Whether the client is already Connected through CPMConnectIn request</param>
        /// <returns>WspClient</returns>
        private WspClient GetClient(bool isClientConnected)
        {
            if (isClientConnected)
            {
                return connectedClients[ClientMachineName];
            }
            else
            {
                return defaultClient;
            }
        }

        /// <summary>
        /// Returns a RequestSender object to send message to Server 
        /// </summary>
        /// <param name="isclientConnected">true if the client is already
        /// Connected through CPMConnectIn request</param>
        /// <returns>RequestSender</returns>
        private RequestSender GetRequestSender(bool isclientConnected)
        {
            if (isclientConnected)
            {
                return connectedClients[ClientMachineName].Sender;
            }
            else
            {
                return defaultSender;
            }
        }

        /// <summary>
        /// Returns the Offset to be used by protocol client and server
        /// </summary>
        /// <returns>Offset to be used (either 64 or 32)</returns>
        private uint GetOffsetUsed()
        {
            uint returnValue = 0;
            uint clientOffset
                = Convert.ToUInt32(wspTestSite.Properties["ClientOffset"]);
            uint serverOffset
                = Convert.ToUInt32(wspTestSite.Properties["ServerOffset"]);
            if (serverOffset == Constants.OFFSET_64
                && clientOffset == Constants.OFFSET_64)
            {
                returnValue = Constants.OFFSET_64;
            }
            else
            {
                returnValue = Constants.OFFSET_32;
            }
            return returnValue;
        }

        /// <summary>
        /// Returns the WspMessageHeader by the adapter context.
        /// </summary>
        /// <param name="_msg">The original _msg value in header.</param>
        /// <param name="_status">The original _status value in header.</param>
        /// <param name="_ulChecksum">The orignial _ulChecksum value in header.</param>
        /// <returns>WspMessageHeader to be used.</returns>
        private WspMessageHeader? GetWspMessageHeader(WspMessageHeader_msg_Values _msg, uint _status, uint _ulChecksum)
        {
            WspMessageHeader? header = null;

            if (SendInvalidMsg)
            {
                header = new WspMessageHeader
                {
                    _msg = WspMessageHeader_msg_Values.Invalid,
                    _status = _status,
                    _ulChecksum = _ulChecksum
                };
            }

            if (SendInvalidStatus)
            {
                header = new WspMessageHeader
                {
                    _msg = _msg,
                    _status = 0xFFFFFFFFU,
                    _ulChecksum = _ulChecksum
                };
            }

            if (SendInvalidUlChecksum)
            {
                header = new WspMessageHeader
                {
                    _msg = _msg,
                    _status = _status,
                    _ulChecksum = 1
                };
                calculateChecksum = false;
            }

            return header;
        }

        #endregion
    }
}

