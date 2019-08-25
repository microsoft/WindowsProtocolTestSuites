/******************************************************************************
* File: Model.cs
* 
* Summary: Model Class provides a message flow validation for all the possible 
           scenarios of the WSP behavior.
*       
* Classes: Model
* 
* Author:  (v-shgoel),{v-abdand}      
*******************************************************************************
*/

using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools;
using System.Text;

using Microsoft.Modeling;

namespace Microsoft.Protocols.TestSuites.WspTS
{
    /// <summary>
    /// Model Class contains all the actions and state variable to generate the
    /// model of MS-WSP protocol
    /// </summary>
    static class Model
    {
        /// <summary>
        /// Represent the server's info including server platform,etc
        /// </summary>
        public static ServerInfo serverInfo = new ServerInfo();

        /// <summary>
        /// 'protocolState' variable is defined to update the protocol state 
        /// after the successful request sent to the server.
        /// 'protocolState' holds the present state of the message flow.
        /// </summary>
        static WspMessage protocolState = WspMessage.None;

        /// <summary>
        /// 'pendingRequest' is a bool variable defined to updated the pending
        /// request information.It is updated after each action is performed.
        /// 'pendingRequest' indicates whether there is any pending request for
        /// server or not.
        /// </summary>
        static bool pendingRequest = false;

        /// <summary>
        /// 'adminPrivilege' is a bool variable to indicate whether the client
        /// have/does not have admin priviledges.For the admin messages, server
        /// will resond successfully only if this variable is set to 'true'.
        /// </summary>
        static bool adminPrivilege = false;

        /// <summary>
        /// 'rowsReceived' is a bool variable that indicates whether there are
        /// more rows to be fetched or not.Server will respond successfully for
        /// CPMFetchValueIn request, only if this variable is set to 'true'.
        /// </summary>
        static bool rowsReceived = false;

        /// <summary>
        /// IsValidBindingInfo is the bool variable to indicate whether the 
        /// client has/does not have valid current set of bindings. 
        /// </summary>
        static bool isValidBinding = false;

        /// <summary>
        /// isConnected is a bool variable to indicate whether the client is 
        /// successfully connected or not.
        /// </summary>
        static bool isConnected = false;

        /// <summary>
        /// isQueryAssociated is a bool variable to indicate whether the query 
        /// has successfully associated with the client or not.
        /// </summary>
        static bool isQueryAssociated = false;

        /// <summary>
        /// isHandleAssociated is a bool variable to indicate whether the 
        /// handle is associated with the client or not.
        /// </summary>
        static bool isHandleAssociated = false;

        #region Connect In and Connect Out

        /// <summary>
        /// CPMConnectInRequest() is used to sent a request to establish a 
        /// connection with the server and starts WSP query processing or WSP
        /// catalog administration.
        /// </summary>
        [Action("CPMConnectInRequest()")]
        static void CPMConnectInRequest()
        {
            //Updating the state variables
            protocolState = WspMessage.CPMConnectIn;
            pendingRequest = true;
        }

        /// <summary>
        /// CPMConnectOutResponse is the response of the CPMConnectIn request.
        /// </summary>
        /// <param name="returnCode">Error code that comes from the server 
        /// response.</param>
        [Action("CPMConnectOutResponse(returnCode)")]
        static void ConnectOutResponse(uint returnCode)
        {

            Contracts.Requires(protocolState == WspMessage.CPMConnectIn
                               && returnCode == 0x00000000); //|| returnCode < 0x80000000));//

            Helper.RequiresCapture(592,
                "[Windows Search Protocol (WSP) messages indicate success two ways,first way]A zero value (0x00000000).");

            Helper.Requires(isConnected == false, 647,
                "When the server receives a CPMConnectIn request from a" +
                 "client,the server MUST check if the client is in the " +
                 "list of connected clients.");

            isConnected = true;

            //============================   TDI 19907  =======================
            //Helper.Requires(!connectedClients.Contains(client.cName),649,
            //"When the server receives a CPMConnectIn request from a client,"+
            //"the server MUST check if the specified catalog exists and is "+
            //"not in the 'stopped' state.");

            //============================   TDI 19907  =======================
            //Helper.Requires(connectedClients.Contains(client.cName), 652,
            //    "When the server receives a CPMConnectIn request from a"+ 
            //     "client, the server MUST associate" +
            //    "the catalog with the client.");

            Helper.Requires(isConnected, 651,
                "When the server receives a CPMConnectIn request from a " +
                 "client, the server MUST add the client to the list of" +
                 "connected clients.");
            Requirements.Capture(RequirementId.Make("MS-WSP", 653, "When the server receives a " +
                 "CPMConnectIn request from a client,the server MUST" +
                 "store the information passed in the CPMConnectIn message " +
                 "(such as catalog name or client version) in the client" +
                 "state."));
            Requirements.Capture(RequirementId.Make("MS-WSP", 654, "When the server receives a " +
                 "CPMConnectIn request from a client,the server MUST" +
                 "respond to the client with a CPMConnectOut message."));

            protocolState = WspMessage.CPMConnectOut;
            pendingRequest = false;

        }

        /// <summary>
        /// This rule validates the possible error cases for the 
        /// CPMConnectIn request. 
        /// </summary>
        /// <param name="returnCode">Error code that comes from the 
        /// server response.</param>
        [Action("CPMConnectOutResponse(returnCode)")]
        static void ConnectOutResponseError(uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMConnectIn 
                 && returnCode != 0x00000000 //returnCode >= 0x80000000 //
                               );
            //&& returnCode == (uint)WspErrorCode.STATUS_INVALID_PARAMETER
            Helper.RequiresCapture(648,"When the server receives a CPMConnectIn request from a" +
                    "client, the server MUST report a " +
                    "STATUS_INVALID_PARAMETER (0xC000000D) error if the" +
                    "client is in the list of connected clients.");

            isConnected = false;
            //else if (catalogNameValue == CatalogName.inValid)
            //{
            //    //====================     TDI   42511     ==================
            //Helper.Requires(returnCode
            //     == (uint)WspErrorCode.CI_E_NO_CATALOG, 650,
            //    "When the server receives a CPMConnectIn request from" +
            //    "a client, the server MUST reporta CI_E_NO_CATALOG " +
            //    "(0x8004181D) error if the specified catalog doesn't" +
            //    "exist or is in stopped' state.");
            //}
            protocolState = WspMessage.ErrorState;
            pendingRequest = false;
        }


        #endregion

        #region Admin Messages

        # region CPMCiStateInOut and CPMCiStateInOutResponse


        /// <summary>
        /// CPMCiStateInOut() requests the information about the state of 
        /// the Windows Search Service.
        /// </summary>
        [Action("CPMCiStateInOut()")]
        static void CPMCiStateInOut()
        {
            protocolState = WspMessage.CPMCiStateInOut;
            pendingRequest = true;
        }


        /// <summary>
        /// CPMCiStateInOutResponse is the response for the CPMCiStateInOut 
        /// request.
        /// </summary>
        /// <param name="returnCode">Error code that comes from the server
        /// response.</param>
        [Action("CPMCiStateInOutResponse(returnCode)")]
        static void CPMCiStateInOutResponse(uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMCiStateInOut
                           && returnCode == 0x00000000 //returnCode < 0x80000000//
                           && isConnected == true);

            Helper.RequiresCapture(592,
                "[Windows Search Protocol (WSP) messages indicate success two ways,first way]A zero value (0x00000000).");

            Requirements.Capture(RequirementId.Make("MS-WSP", 599,
                "All messages MUST be preceded by an initial CPMConnectIn " +
                "message."));
            Requirements.Capture(RequirementId.Make("MS-WSP", 630,
                "When the server receives a CPMCIStateInOut message request " +
                "from the client, the server MUST first check if the client " +
                "is in a list of connected clients."));
            Requirements.Capture(RequirementId.Make("MS-WSP", 632, "When the server receives a " +
                "CPMCIStateInOut message request from the client, and if the" +
                "client is in the list of connected clients,the server  MUST" +
                "respond to the client with a CPMCIStateInOut message."));

            pendingRequest = false;
        }

        /// <summary>
        /// This rule validates the possible error cases for the 
        /// CPMCiStateInOut request. 
        /// </summary>
        /// <param name="returnCode">Error code that comes from the 
        /// server response.</param>
        [Action("CPMCiStateInOutResponse(returnCode)")]
        static void CPMCiStateInOutResponseError(uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMCiStateInOut
                               && isConnected == false);

            Helper.Requires(returnCode
                == (uint)WspErrorCode.STATUS_INVALID_PARAMETER, 631,
                  "When the server receives a CPMCIStateInOut message" +
                  "request from the client, and if the client is not in" +
                  "the list of connected clients,the server MUST report" +
                  "a STATUS_INVALID_PARAMETER (0xC000000D) error.");

            protocolState = WspMessage.ErrorState;
            pendingRequest = false;
        }

        # endregion

        # region CPMForceMergeIn and CPMForceMergeInResponse

        /// <summary>
        /// CPMForceMergeIn()request is send to improve query performance.
        /// </summary>
        [Action("CPMForceMergeIn(isAdminClient)")]
        static void CPMForceMergeIn(bool isAdminClient)
        {
            protocolState = WspMessage.CPMForceMergeIn;
            pendingRequest = true;
            adminPrivilege = isAdminClient;
        }


        /// <summary>
        /// CPMForceMergeInResponse is the response of the CPMForceMergeIn
        /// request.
        /// </summary>
        /// <param name="returnCode">Error code that comes from the server
        /// response.</param>
        [Action("CPMForceMergeInResponse(returnCode)")]
        static void CPMForceMergeInResponse(uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMForceMergeIn
                              && returnCode == 0x00000000 // returnCode < 0x80000000//
                              && isConnected == true
                              && adminPrivilege == true);
            Helper.RequiresCapture(592,
                "[Windows Search Protocol (WSP) messages indicate success two ways,first way]A zero value (0x00000000).");

            Requirements.Capture(RequirementId.Make("MS-WSP", 599, "All messages MUST be preceded by an" +
                " initial CPMConnectIn message."));
            Requirements.Capture(RequirementId.Make("MS-WSP", 641, "When the server receives a " +
                "CPMForceMergeIn message request,the server MUST Check" +
                "if the client is in a list of connected clients " +
                "(which have a catalog associated). "));
            Requirements.Capture(RequirementId.Make("MS-WSP", 643, "When the server receives a " +
                "CPMForceMergeIn message request, the server MUST check" +
                "that client has administrative access."));
            pendingRequest = false;
        }

        /// <summary>
        /// This rule validates the Status Invalid Parameter error case 
        /// for the CPMForceMergeIn request. 
        /// </summary>
        /// <param name="returnCode">Error code that comes from the 
        /// server response.</param>
        [Action("CPMForceMergeInResponse(returnCode)")]
        static void CPMForceMergeInResponse_StatusInvalidParameterError
            (uint returnCode)
        {
            Contracts.Requires((protocolState == WspMessage.CPMForceMergeIn
                             && isConnected == false
                             && returnCode == (uint)WspErrorCode.STATUS_INVALID_PARAMETER));


            Helper.RequiresCapture(642, "When the server receives a CPMForceMergeIn message request, " +
                    "if the client is not in the list of connected clients, " +
                    "the server MUST report a STATUS_INVALID_PARAMETER (0xC000000D) error.");
            protocolState = WspMessage.ErrorState;
            pendingRequest = false;
        }

        /// <summary>
        /// This rule validates the Status Access Denied error case for 
        /// the CPMForceMergeIn request.
        /// </summary>
        /// <param name="returnCode">Error code that comes from the 
        /// server response.</param>
        [Action("CPMForceMergeInResponse(returnCode)")]
        static void CPMForceMergeInResponse_StatusAccessDeniedError
            (uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMForceMergeIn
                              && isConnected == true
                              && adminPrivilege == false
                              && returnCode  == (uint)WspErrorCode.E_ACCESSDENIED);
            
            Helper.RequiresCapture(644, "When the server receives a CPMForceMergeIn message " +
                    "request, if the client does not have administrative" +
                    "access,the server MUST report a STATUS_ACCESS_DENIED" +
                    "(0xC0000022) error.");
            protocolState = WspMessage.ErrorState;
            pendingRequest = false;
        }

        # endregion

        # endregion

        #region Query In and Query Out

        /// <summary>
        /// CPMCreateQueryIn() emulates WSP CPMCreateQueryIn message and 
        /// associates a query with the client
        /// if successful
        /// </summary>
        [Action("CPMCreateQueryIn(ENABLEROWSETEVENTS)")]
        static void CPMCreateQueryIn(bool ENABLEROWSETEVENTS)
        {
            //Updating the state variables.
            protocolState = WspMessage.CPMCreateQueryIn;
            pendingRequest = true;
        }


        /// <summary>
        /// CPMCreateQueryOutResponse is the response of the 
        /// CPMCreateQueryIn request.
        /// </summary>
        /// <param name="returnCode">Error code that comes from the 
        /// server response</param>
        [Action("CPMCreateQueryOutResponse(returnCode)")]
        static void CPMCreateQueryOut(uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMCreateQueryIn
                               && returnCode == 0x00000000 //returnCode < 0x80000000//
                               && isQueryAssociated == false
                               && isConnected == true);

            //Updating the client's query details.
            isHandleAssociated = true;
            isQueryAssociated = true;

            Helper.RequiresCapture(592,
                "[Windows Search Protocol (WSP) messages indicate success two ways,first way]A zero value (0x00000000).");

            Helper.Requires(isConnected, 599, "All messages MUST be preceded" +
                " by an initial CPMConnectIn message.");
            Requirements.Capture(RequirementId.Make("MS-WSP", 655, "When the server receives a " +
                "CPMCreateQueryIn message request from a client," +
                "the server MUST check if the client is in the list" +
                "of connected clients."));
            Requirements.Capture(RequirementId.Make("MS-WSP", 657, "When the server receives a " +
                "CPMCreateQueryIn message request from a client," +
                "the server MUST check if the client already has a " +
                "query associated with it."));

            pendingRequest = false;
        }

        /// <summary>
        /// This rule validates the STATUS_INVALID_PARAMETER error case
        /// for the CPMCreateQueryIn request. 
        /// </summary>
        /// <param name="returnCode">Error code that comes from the 
        /// server response.</param>
        [Action("CPMCreateQueryOutResponse(returnCode)")]
        static void CPMCreateQueryOut_StatusInvalidParameterError
            (uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMCreateQueryIn
                               && false == isConnected
                               && returnCode != 0x00000000);//returnCode >= 0x80000000);//

            Helper.Requires(returnCode
                == (uint)WspErrorCode.STATUS_INVALID_PARAMETER, 656,
                    "When the server receives a CPMCreateQueryIn message" +
                    "request from a client,the server MUST report a " +
                    "STATUS_INVALID_PARAMETER (0xC000000D) error if the " +
                    "client is not in the list of connected clients.");
            Requirements.Capture(RequirementId.Make("MS-WSP", 662,
                "When the server receives a CPMCreateQueryIn message request" +
                "from a client, the server MUST report an appropriate error " +
                "if the server encounters an error while parsing the " +
                "restriction set, sort orders, and groupings that are" +
                "specified in the query."));
            Requirements.Capture(RequirementId.Make("MS-WSP", 663,
                "When the server receives a CPMCreateQueryIn message request" +
                "from a client, the server MUST report the error encountered" +
                "if parsing the restriction set, sort orders, and groupings" +
                "that are specified in the query fails for any other reason."));
            protocolState = WspMessage.ErrorState;
            pendingRequest = false;
        }

        /// <summary>
        /// This rule validates the error case when the 
        /// existing query is again created for the CPMCreateQueryIn request.
        /// </summary>
        /// <param name="returnCode">Error code that comes from the 
        /// server response.</param>
        [Action("CPMCreateQueryOutResponse(returnCode)")]
        static void CPMCreateQueryOut_CreatingExistingQueryError
            (uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMCreateQueryIn
                               && isQueryAssociated == true
                               && returnCode != 0x00000000); //returnCode >= 0x80000000);//

            Helper.Requires(returnCode
                == (uint)WspErrorCode.STATUS_INVALID_PARAMETER, 658,
                    "When the server receives a CPMCreateQueryIn" +
                    " message request from a client," +
                    "the server MUST report a STATUS_INVALID_PARAMETER" +
                    " (0xC000000D) error if the client already has a " +
                    "query associated with it.");
            Requirements.Capture(RequirementId.Make("MS-WSP", 662,
                 "When the server receives a " +
                 "CPMCreateQueryIn message request from a client, the " +
                 "server MUST report an appropriate error if the server" +
                 " encounters an error while parsing the restriction set," +
                 " sort orders, and groupings that are specified in the" +
                 "query."));
            Requirements.Capture(RequirementId.Make("MS-WSP", 663,
                "When the server receives a CPMCreateQueryIn message request" +
                "from a client, the server MUST report the error encountered" +
                "if parsing the restriction set, sort orders, and groupings" +
                "that are specified in the query fails for any other reason."));

            protocolState = WspMessage.ErrorState;
            pendingRequest = false;
        }

        #endregion

        # region QueryStatusIn and QueryStatusOut

        /// <summary>
        /// CPMGetQueryStatusIn() requests the status of the query.
        /// </summary>
        /// <param name="isCursorValid">indicates valid/invalid cursor</param>
        [Action("CPMGetQueryStatusIn(isCursorValid)")]
        static void CPMGetQueryStatusIn(bool isCursorValid)
        {
            isHandleAssociated = isCursorValid;
            protocolState = WspMessage.CPMGetQueryStatusIn;
            pendingRequest = true;
        }

        /// <summary>
        /// CPMGetQueryStatusOutResponse is the response of the 
        /// CPMGetQueryStatusIn request.
        /// </summary>
        /// <param name="returnCode">Error code that comes from the 
        /// server response.</param>
        [Action("CPMGetQueryStatusOutResponse(returnCode)")]
        static void CPMGetQueryStatusOut(uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMGetQueryStatusIn
                              && returnCode == 0x00000000 //returnCode < 0x80000000//
                              && isQueryAssociated == true
                              && isHandleAssociated == true
                              && isConnected == true);
            Helper.RequiresCapture(592,
                "[Windows Search Protocol (WSP) messages indicate success two ways,first way]A zero value (0x00000000).");

            Helper.Requires(isConnected, 599, "All messages MUST be preceded" +
                " by an initial CPMConnectIn message.");
            Requirements.Capture(RequirementId.Make("MS-WSP", 679, "When the server receives a " +
                "CPMGetQueryStatusIn message request from a client, the" +
                "server MUST respond to the client with the " +
                "CPMGetQueryStatusOut message."));
            Requirements.Capture(RequirementId.Make("MS-WSP", 672, "When the server receives a " +
                "CPMGetQueryStatusIn message request from a client," +
                "the server MUST check whether the client has a query" +
                "associated with it."));
            Requirements.Capture(RequirementId.Make("MS-WSP", 674, "When the server receives a " +
                "CPMGetQueryStatusIn message request from a client," +
                "the server MUST check whether the cursor handle is in" +
                "a list of the client's cursor handles."));
            pendingRequest = false;
        }

        /// <summary>
        /// This rule validates the STATUS_INVALID_PARAMETER error case
        /// for the CPMGetQueryStatusIn request. 
        /// </summary>
        /// <param name="returnCode">Error code that comes from
        /// the server response.</param>
        [Action("CPMGetQueryStatusOutResponse(returnCode)")]
        static void CPMGetQueryStatusOut_StatusInvalidParameterError
            (uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMGetQueryStatusIn
                             && returnCode != 0x00000000 //returnCode >= 0x80000000//
                             && isQueryAssociated == false);
            Requirements.Capture(RequirementId.Make("MS-WSP", 678,
                "When the server receives a CPMGetQueryStatusIn message " +
                "request from a client, the server MUST report an error" +
                "if it fails to retrieve current query status and set it" +
                "in _Qstatus field for any reason."));
            Helper.Requires(returnCode
                == (uint)WspErrorCode.STATUS_INVALID_PARAMETER, 673,
                    "When the server receives a CPMGetQueryStatusIn message" +
                    "request from a client,the server MUST report a " +
                    "STATUS_INVALID_PARAMETER (0xC000000D) error if the" +
                    "client do not have a query associated with it.");

            protocolState = WspMessage.ErrorState;
            pendingRequest = false;
        }

        /// <summary>
        /// This rule validates the E_FAIL error case for the 
        /// CPMGetQueryStatusIn request. 
        /// </summary>
        /// <param name="returnCode">Error code that comes 
        /// from the server response.</param>
        [Action("CPMGetQueryStatusOutResponse(returnCode)")]
        static void CPMGetQueryStatusOut_E_FailError(uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMGetQueryStatusIn
                              && returnCode != 0x00000000 //returnCode >= 0x80000000//
                              && isQueryAssociated == true
                              && isHandleAssociated == false);

            Requirements.Capture(RequirementId.Make("MS-WSP", 678,
                "When the server receives a CPMGetQueryStatusIn message " +
                "request from a client, the server MUST report an error if" +
                "it fails to retrieve current query status and set it" +
                "in _Qstatus field for any reason."));
            //============================  TDI 19903     =====================
            Helper.Requires(returnCode == (uint)WspErrorCode.E_FAIL, 675,
                "When the server receives a CPMGetQueryStatusIn message" +
                "request from a client,the server MUST report an E_FAIL" +
                "(0x80004005) error if the cursor handle  is not in the" +
                "list of the client's cursor handles.");


            protocolState = WspMessage.ErrorState;
            pendingRequest = false;
        }

        # endregion

        # region QueryStatusExIn and QueryStatusExOut

        /// <summary>
        /// CPMGetQueryStatusExIn()requests for the status of the query and the
        /// additional information from the server.
        /// </summary>
        /// <param name="isCursorValid">indicates valid/invalid cursor</param>
        [Action("CPMGetQueryStatusExIn(isCursorValid)")]
        static void CPMGetQueryStatusExIn(bool isCursorValid)
        {
            isHandleAssociated = isCursorValid;
            protocolState = WspMessage.CPMGetQueryStatusExIn;
            pendingRequest = true;
        }

        /// <summary>
        /// CPMGetQueryStatusExOutResponse is the response for the 
        /// CPMGetQueryStatusExOutIn request.
        /// </summary>
        /// <param name="returnCode">Error code that comes from 
        /// the server response.</param>
        [Action("CPMGetQueryStatusExOutResponse(returnCode)")]
        static void CPMGetQueryStatusExOut(uint returnCode)
        {
            Contracts.Requires(protocolState
                              == WspMessage.CPMGetQueryStatusExIn
                              && returnCode == 0x00000000 //returnCode < 0x80000000//
                              && isQueryAssociated == true
                              && isHandleAssociated == true
                              && isConnected == true);
            Helper.RequiresCapture(592,
                "[Windows Search Protocol (WSP) messages indicate success two ways,first way]A zero value (0x00000000).");

            Helper.Requires(isConnected, 599, "All messages MUST be preceded" +
                " by an initial CPMConnectIn message.");
            Requirements.Capture(RequirementId.Make("MS-WSP", 680, "When the server receives a " +
                "CPMGetQueryStatusExIn message request from a client," +
                "the server MUST check whether the client has a query" +
                "associated with it."));
            Requirements.Capture(RequirementId.Make("MS-WSP", 682, "When the server receives a " +
                "CPMGetQueryStatusExIn message request from a client," +
                "the server MUST check whether the cursor handle passed" +
                "is in a list of the client's cursor handles"));
            Requirements.Capture(RequirementId.Make("MS-WSP", 692, "When the server receives a " +
                "CPMGetQueryStatusExIn message request from a client, " +
                "the server MUST respond to the client with the " +
                "CPMGetQueryStatusExOut message."));
            pendingRequest = false;
        }

        /// <summary>
        /// This rule validates the STATUS_INVALID_PARAMETER error case for
        /// the CPMGetQueryStatusExIn request. 
        /// </summary>
        /// <param name="returnCode">Error code that comes from the 
        /// server response.</param>
        [Action("CPMGetQueryStatusExOutResponse(returnCode)")]
        static void CPMGetQueryStatusExOut_StatusInvalidParameterError
            (uint returnCode)
        {
            Contracts.Requires(protocolState
                             == WspMessage.CPMGetQueryStatusExIn
                             && returnCode != 0x00000000 //returnCode >= 0x80000000//
                             && isQueryAssociated == false);

            Helper.Requires(returnCode
                == (uint)WspErrorCode.STATUS_INVALID_PARAMETER, 681,
                   "When the server receives a CPMGetQueryStatusExIn " +
                   "message request from a client," +
                   "the server MUST report a STATUS_INVALID_PARAMETER " +
                   "(0xC000000D) error if the client do not have a query" +
                   "associated with it.");
            Requirements.Capture(RequirementId.Make("MS-WSP", 687, "When the server receives a " +
             "CPMGetQueryStatusExIn message request from a client, the" +
             "server MUST report that an error was encountered if it fails" +
             "to set values in the CPMGetQueryStatusExOut message."));
            Requirements.Capture(RequirementId.Make("MS-WSP", 689, "When the server receives a" +
             "CPMGetQueryStatusExIn message request from a client, " +
             "the server MUST report an error if it fails to retrieve" +
             "information about the client's catalog and fill in" +
             "_cFilteredDocuments and _cDocumentsToFilter."));
            Requirements.Capture(RequirementId.Make("MS-WSP", 691, "When the server receives a" +
                "CPMGetQueryStatusExIn message request from a client," +
                "the server MUST report that an error was encountered if" +
                "it fails to fill in the remaining _iRowBmk field."));
            protocolState = WspMessage.ErrorState;
            pendingRequest = false;
        }

        /// <summary>
        /// This rule validates the E_FAIL error case for the 
        /// CPMGetQueryStatusExIn request.
        /// </summary>
        /// <param name="returnCode">Error code that comes from 
        /// the server response.</param>
        [Action("CPMGetQueryStatusExOutResponse(returnCode)")]
        static void CPMGetQueryStatusExOutE_FailError(uint returnCode)
        {
            Contracts.Requires(protocolState
                               == WspMessage.CPMGetQueryStatusExIn
                               && returnCode != 0x00000000 //returnCode >= 0x80000000//
                               && isHandleAssociated == false
                               );
            
            Requirements.Capture(RequirementId.Make("MS-WSP", 687, "When the server receives a" +
                   "CPMGetQueryStatusExIn message request from a client," +
                   "the server MUST report that an error was encountered" +
                   "if it fails to set values in the CPMGetQueryStatusExOut" +
                   "message."));
            Requirements.Capture(RequirementId.Make("MS-WSP", 689, "When the server receives a" +
                   "CPMGetQueryStatusExIn message request from a client," +
                   "the server MUST report an error if it fails to retrieve" +
                   "information about the client's catalog and fill in " +
                   "_cFilteredDocuments and _cDocumentsToFilter."));
            Requirements.Capture(RequirementId.Make("MS-WSP", 691, "When the server receives a " +
                   "CPMGetQueryStatusExIn message request from a client," +
                   "the server MUST report that an error was encountered " +
                   "if it fails to fill in the remaining _iRowBmk field."));
            protocolState = WspMessage.ErrorState;
            pendingRequest = false;
        }


        # endregion

        # region CPMRatioFinishedIn and CPMRatioFinishedOut

        /// <summary>
        /// CPMRatioFinishedIn() requests for the completion percentage of the 
        /// query.
        /// </summary>
        /// <param name="isCursorValid">indicates valid/invalid cursor</param>
        [Action("CPMRatioFinishedIn(isCursorValid)")]
        static void CPMRatioFinishedIn(bool isCursorValid)
        {
            isHandleAssociated = isCursorValid;
            protocolState = WspMessage.CPMRatioFinishedIn;
            pendingRequest = true;
        }

        /// <summary>
        /// CPMRatioFinishedOutResponse is the response for the 
        /// CPMRatioFinishedIn request.
        /// </summary>
        /// <param name="returnCode">Error code that comes from 
        /// the server response.</param>
        [Action("CPMRatioFinishedOutResponse(returnCode)")]
        static void CPMRatioFinishedOut(uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMRatioFinishedIn
                              && returnCode == 0x00000000 //returnCode < 0x80000000//
                              && isQueryAssociated == true
                              && isHandleAssociated == true
                              && isConnected == true);
            Helper.RequiresCapture(592,
                "[Windows Search Protocol (WSP) messages indicate success two ways,first way]A zero value (0x00000000).");

            Helper.Requires(isConnected, 599, "All messages MUST be preceded " +
                "by an initial CPMConnectIn message.");
            Requirements.Capture(RequirementId.Make("MS-WSP", 693, "When the server receives a " +
                "CPMRatioFinishedIn message request from a client," +
                "the server MUST check whether the client has a query" +
                "associated with it."));
            Requirements.Capture(RequirementId.Make("MS-WSP", 695, "When the server receives a " +
                "CPMRatioFinishedIn message request from a client," +
                "the server MUST check whether the cursor handle passed" +
                "is in the list of the clients cursor handles."));
            Requirements.Capture(RequirementId.Make("MS-WSP", 702, "When the server receives a " +
                "CPMRatioFinishedIn message request from a client, the" +
                "server MUST respond to the client with the " +
                "CPMRatioFinishedOut message"));
            pendingRequest = false;
        }

        /// <summary>
        /// This rule validates the STATUS_INVALID_PARAMETER error 
        /// case for the CPMRatioFinishedIn request. 
        /// </summary>
        /// <param name="returnCode">Error code that comes from 
        /// the server response.</param>
        [Action("CPMRatioFinishedOutResponse(returnCode)")]
        static void CPMRatioFinishedOut_StatusInvalidParameterError
            (uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMRatioFinishedIn
                            && returnCode != 0x00000000 //returnCode >= 0x80000000//
                            && isQueryAssociated == false);

            Helper.Requires(returnCode
                == (uint)WspErrorCode.STATUS_INVALID_PARAMETER, 694,
                    "When the server receives a CPMRatioFinishedIn message " +
                    "request from a client," +
                    "the server MUST report a STATUS_INVALID_PARAMETER " +
                    "(0xC000000D) error if the client doesn’t have a query" +
                    "associated with it.");

            Requirements.Capture(RequirementId.Make("MS-WSP", 699, "When the server receives a " +
                    "CPMRatioFinishedIn message request from a client," +
                    "the server MUST report that an error was encountered" +
                    "it it fails to retrieve the client's query status and" +
                    "fill in the _ulNumerator, _ulDenominator, " +
                    "and _cRows fields."));
            protocolState = WspMessage.ErrorState;
            pendingRequest = false;
        }

        /// <summary>
        /// This rule validates the E_FAIL error case for the 
        /// CPMRatioFinishedIn request.
        /// </summary>
        /// <param name="returnCode"></param>
        [Action("CPMRatioFinishedOutResponse(returnCode)")]
        static void CPMRatioFinishedOut_E_FailError(uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMRatioFinishedIn
                            && returnCode != 0x00000000 //returnCode >= 0x80000000//
                            && isHandleAssociated == false);

            Requirements.Capture(RequirementId.Make("MS-WSP", 699, "When the server receives a " +
                    "CPMRatioFinishedIn message request from a client," +
                    "the server MUST report that an error was encountered" +
                    "it it fails to retrieve the client's query status and" +
                    "fill in the _ulNumerator, _ulDenominator, " +
                    "and _cRows fields."));

            protocolState = WspMessage.ErrorState;
            pendingRequest = false;
        }

        # endregion

        # region CPMGetNotify and CPMSendNotifyOut

        /// <summary>
        /// CPMGetNotifyIn() requests that the client wants to be notified
        /// of rowset changes.
        /// </summary>
        /// <param name="isCursorValid">indicates valid/invalid 
        /// cursor</param>
        [Action("CPMGetNotify(isCursorValid)")]
        static void CPMGetNotify(bool isCursorValid)
        {
            isHandleAssociated = isCursorValid;
            protocolState = WspMessage.CPMGetNotify;
            pendingRequest = true;
        }

        /// <summary>
        /// CPMSendNotifyOutResponse is the response for CPMGetNotify request.
        /// </summary>
        /// <param name="returnCode">Error code that comes from the 
        /// server response.</param>
        [Action("CPMSendNotifyOutResponse(returnCode)")]
        static void CPMSendNotifyOutResponse(uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMGetNotify
                              //&& returnCode == 0x00000000 //returnCode < 0x80000000//
                              && isQueryAssociated == true
                              && isHandleAssociated == true
                              && isConnected == true);

            if (serverInfo.ServerPlatForm == SkuOsVersion.Win2K8R2)
            {
                Contracts.Requires(returnCode != 0x00000000);
            }
            else
            {
                Helper.RequiresCapture(592,
                    "[Windows Search Protocol (WSP) messages indicate success two ways,first way]A zero value (0x00000000).");

                Helper.Requires(isConnected, 599, "All messages MUST be preceded" +
                    " by an initial CPMConnectIn message.");
                Requirements.Capture(RequirementId.Make("MS-WSP", 751, "When the server receives a " +
                    "CPMGetNotify message from a client, " +
                    "the server MUST check whether the client has a " +
                    "query associated with it."));
            }
            
            pendingRequest = false;
        }

        /// <summary>
        /// This rule validates the possible error cases for the
        /// CPMGetNotify request. 
        /// </summary>
        /// <param name="returnCode">Error code that comes from 
        /// the server response.</param>
        [Action("CPMSendNotifyOutResponse(returnCode)")]
        static void CPMSendNotifyOutResponseError(uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMGetNotify
                            && returnCode != 0x00000000 //returnCode >= 0x80000000//
                            && isQueryAssociated == false);

            Helper.Requires(returnCode
                == (uint)WspErrorCode.STATUS_INVALID_PARAMETER, 752,
                    "When the server receives a CPMGetNotify message from" +
                    "a client,the server MUST  report" +
                    "a STATUS_INVALID_PARAMETER (0xC000000D) error if the" +
                    "client doesn't have a query associated with it.");

            protocolState = WspMessage.ErrorState;
            pendingRequest = false;
        }

        # endregion

        # region CPMSetBindingsIn and CPMSetBindingsInResponse

        /// <summary>
        /// CPMSetBindingsIn() requests the bindings of columns 
        /// to a rowset.
        /// </summary>
        /// <param name="isValidBindingInfo">indicates 
        /// valid/invalid binding info</param>
        /// <param name="isCursorValid">indicates 
        /// valid/invalid cursor</param>
        [Action("CPMSetBindingsIn(isValidBindingInfo,isCursorValid)")]
        static void CPMSetBindingsIn(bool isValidBindingInfo,
            bool isCursorValid)
        {
            Model.isValidBinding = isValidBindingInfo;
            isHandleAssociated = isCursorValid;
            protocolState = WspMessage.CPMSetBindingsIn;
            pendingRequest = true;
        }

        /// <summary>
        /// CPMSetBindingsInResponse is the response for the 
        /// CPMSetBindingsIn request.
        /// </summary>
        /// <param name="returnCode">Error code that comes 
        /// from the server response.</param>
        [Action("CPMSetBindingsInResponse(returnCode)")]
        static void CPMSetBindingsInResponse(uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMSetBindingsIn
                              && returnCode == 0x00000000 //returnCode < 0x80000000//
                              && isQueryAssociated == true
                              && isHandleAssociated == true
                              && isConnected == true
                              && Model.isValidBinding == true);

            Helper.RequiresCapture(592,
                "[Windows Search Protocol (WSP) messages indicate success two ways,first way]A zero value (0x00000000).");

            Requirements.Capture(RequirementId.Make("MS-WSP", 742, "When the server receives a " +
                "CPMSetBindingsIn message request from a client, " +
                "the server MUST check whether the client has a " +
                "query associated with it."));

            Requirements.Capture(RequirementId.Make("MS-WSP", 744, "When the server receives a " +
                "CPMSetBindingsIn message request from a client, " +
                "the server MUST check whether the cursor handle passed" +
                "is in the list of the client's cursor handles."));
            Requirements.Capture(RequirementId.Make("MS-WSP", 746,
                "When the server receives a CPMSetBindingsIn message" +
                "request from a client,the server MUST verify that bindings" +
                "information is valid."));

            Helper.Requires(isConnected, 599,
                "All messages MUST be preceded by an initial CPMConnectIn " +
                "message.");
            pendingRequest = false;
        }

        /// <summary>
        /// This rule validates the STATUS_INVALID_PARAMETER error 
        /// case for the CPMSetBindingsIn request. 
        /// </summary>
        /// <param name="returnCode">Error code that comes from 
        /// the server response.</param>
        [Action("CPMSetBindingsInResponse(returnCode)")]
        static void CPMSetBindingsInResponse_StatusInvalidParameterError
            (uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMSetBindingsIn
                               && returnCode != 0x00000000 //returnCode >= 0x80000000//
                               && isQueryAssociated == false);

            Helper.Requires(returnCode
                == (uint)WspErrorCode.STATUS_INVALID_PARAMETER, 743,
                "When the server receives a CPMSetBindingsIn message request" +
                "from a client, the server MUST report a " +
                "STATUS_INVALID_PARAMETER (0xC000000D) error if the client " +
                "does not have a query associated with it.");

            Model.isValidBinding = false;

            protocolState = WspMessage.ErrorState;
            pendingRequest = false;
        }

        /// <summary>
        /// This rule validates the E_FAIL error case for the 
        /// CPMSetBindingsIn request.
        /// </summary>
        /// <param name="returnCode">Error code that comes from 
        /// the server response.</param>
        [Action("CPMSetBindingsInResponse(returnCode)")]
        static void CPMSetBindingsInResponse_E_FailError
            (uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMSetBindingsIn
                               && returnCode != 0x00000000 //returnCode>= 0x80000000//
                               && isHandleAssociated == false
                               );

            protocolState = WspMessage.ErrorState;
            pendingRequest = false;
        }

        /// <summary>
        /// This rule validates the DB_E_BADBINDINFO error case for the 
        /// CPMSetBindingsIn request.
        /// </summary>
        /// <param name="returnCode">Error code that comes from the 
        /// server response.</param>
        [Action("CPMSetBindingsInResponse(returnCode)")]
        static void CPMSetBindingsInResponse_DB_E_BADBINDINFOError
            (uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMSetBindingsIn
                               && returnCode != 0x00000000 //returnCode >= 0x80000000//
                               && false == Model.isValidBinding);

            Helper.Requires(returnCode == (uint)WspErrorCode.DB_E_BADBINDINFO,
                747, "When the server receives a CPMSetBindingsIn message" +
                "request from a client, the server MUST report a " +
                "DB_E_BADBINDINFO (0x80040E08) error if the bindings" +
                "information is not valid.");

            protocolState = WspMessage.ErrorState;
            pendingRequest = false;
        }

        # endregion

        # region CPMGetRowsIn and CPMGetRowsOut

        /// <summary>
        /// CPMGetRowsIn() message requests rows from a query.
        /// </summary>
        /// <param name="isCursorValid">indicates valid/invalid cursor</param>
        [Action("CPMGetRowsIn(isCursorValid)")]
        static void CPMGetRowsIn(bool isCursorValid)
        {
            isHandleAssociated = isCursorValid;
            protocolState = WspMessage.CPMGetRowsIn;
            pendingRequest = true;
        }

        /// <summary>
        /// CPMGetRowsOutResponse is the response for CPMGetRowsIn request.
        /// </summary>
        /// <param name="returnCode">Error code that comes from the 
        /// server response.</param>
        [Action("CPMGetRowsOut(returnCode)")]
        static void CPMGetRowsOut(uint returnCode)
        {

            Contracts.Requires(protocolState == WspMessage.CPMGetRowsIn
                               && Model.isValidBinding == true
                               && isQueryAssociated == true
                               && isHandleAssociated == true
                               && isConnected == true);

            Helper.RequiresCapture(592,
                "[Windows Search Protocol (WSP) messages indicate success two ways,first way]A zero value (0x00000000).");

            Requirements.Capture(RequirementId.Make("MS-WSP", 703, "When the server receives a " +
                "CPMGetRowsIn message request from a client," +
                "the server MUST check whether the client has a query" +
                "associated with it."));
            Requirements.Capture(RequirementId.Make("MS-WSP", 705, "When the server receives a " +
                "CPMGetRowsIn message request from a client," +
                "the server MUST Check if the cursor handle passed is" +
                "in the list of the clients cursor handles."));
            Requirements.Capture(RequirementId.Make("MS-WSP", 707, "When the server receives a " +
                "CPMGetRowsIn message request from a client, " +
                "the server MUST check whether the client has a current" +
                "set of bindings."));
            Requirements.Capture(RequirementId.Make("MS-WSP", 723, "When the server receives a " +
                "CPMGetRowsIn message request from a client, the server" +
                "MUST respond to the client with the CPMGetRowsOut message."));

            Helper.Requires(isConnected, 599,
                "All messages MUST be preceded by an initial " +
                "CPMConnectIn message.");
            rowsReceived = true;
            pendingRequest = false;
        }

        /// <summary>
        /// This rule validates the Status Invalid Parameter error 
        /// case for the CPMGetRowsIn request. 
        /// </summary>
        /// <param name="returnCode">Error code that comes from 
        /// the server response.</param>
        [Action("CPMGetRowsOut(returnCode)")]
        static void CPMGetRowsOut_StatusInvalidParameterError(uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMGetRowsIn
                //&& returnCode != 0x00000000
                               && isQueryAssociated == false
                               && isHandleAssociated == true
                               //&& Model.isValidBinding == true
                               );

            Helper.Requires(returnCode
                == (uint)WspErrorCode.STATUS_INVALID_PARAMETER, 704,
                    " When the server receives a CPMGetRowsIn message " +
                    "request from a client, the server MUST " +
                    "report a STATUS_INVALID_PARAMETER (0xC000000D) error " +
                    "if the client do not have a query associated with it.");

            protocolState = WspMessage.ErrorState;
            pendingRequest = false;
        }

        /// <summary>
        /// This rule validates the error case when the handle associated 
        /// is not valid for the CPMGetRowsIn request. 
        /// </summary>
        /// <param name="returnCode">Error code that comes from the 
        /// server response.</param>
        [Action("CPMGetRowsOut(returnCode)")]
        static void CPMGetRowsOut_HandleNotAssociatedError(uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMGetRowsIn
                //&& returnCode != 0x00000000
                               && isQueryAssociated == true
                               && isHandleAssociated == false
                               && Model.isValidBinding == true);
            //newC
            Helper.Requires(returnCode == (uint)WspErrorCode.E_FAIL, 706,
                   "When the server receives a CPMGetRowsIn message request"
                    + "from a client, the server MUST report an " +
                   "E_FAIL (0x80004005) error if the cursor handle passed" +
                    "is not in the list of the clients cursor handles.");

            protocolState = WspMessage.ErrorState;
            pendingRequest = false;
        }

        /// <summary>
        /// This rule validates the error case when the binding info 
        /// are not valid for the CPMGetRowsIn request.
        /// </summary>
        /// <param name="returnCode">Error code that comes from 
        /// the server response.</param>
        [Action("CPMGetRowsOut(returnCode)")]
        static void CPMGetRowsOut_InvalidBindingError(uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMGetRowsIn
                               && Model.isValidBinding == false
                               && isQueryAssociated == true
                               && isConnected == true
                               //&& returnCode == (uint)WspErrorCode.E_UNEXPECTED
                               );
            if (serverInfo.ServerPlatForm == SkuOsVersion.Win2K8R2)
            {
                //this requirement is blocked by TDI 42414,this TDI may be fixed to add a endnote
                //to describe the behavior in Windows Server 2008 R2(return 0xC000000D error).
                Contracts.Requires(returnCode == (uint)WspErrorCode.STATUS_INVALID_PARAMETER);
            }
            else
            {
                Contracts.Requires(returnCode == (uint)WspErrorCode.E_UNEXPECTED);
                Helper.RequiresCapture(708,
                        "When the server receives a CPMGetRowsIn message request from a client, " +
                        "If this is not the case[client dose not  has a current set of bindings], " +
                        "the server MUST report an E_UNEXPECTED (0x8000FFFF) error.");
            }
            
            protocolState = WspMessage.ErrorState;
            pendingRequest = false;
        }
        # endregion

        # region CPMFetchValueIn and CPMFetchValueOut

        /// <summary>
        /// CPMFetchValueIn() requests for the property value that was too 
        /// large to return in a rowset.
        /// </summary>
        /// <param name="isCursorValid">Indicates whether the client has
        /// a valid cursor associated with the query</param>
        [Action("CPMFetchValueIn(isCursorValid)")]
        static void CPMFetchValueIn(bool isCursorValid)
        {
            isHandleAssociated = isCursorValid;
            protocolState = WspMessage.CPMFetchValueIn;
            pendingRequest = true;
        }

        /// <summary>
        /// CPMFetchValueOutResponse is the response for 
        /// CPMFetchValueIn request.
        /// </summary>
        /// <param name="returnCode">Error code that comes from 
        /// the server response.</param>
        [Action("CPMFetchValueOutResponse(returnCode)")]
        static void CPMFetchValueOut(uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMFetchValueIn
                //&& Model.isValidBindingInfo == true
                               && isQueryAssociated == true
                               && isHandleAssociated == true
                               && rowsReceived == true
                               && returnCode == 0x00000000//returnCode < 0x80000000//
                               && isConnected == true);

            Helper.RequiresCapture(592,
                "[Windows Search Protocol (WSP) messages indicate success two ways,first way]A zero value (0x00000000).");

            Requirements.Capture(RequirementId.Make("MS-WSP", 727, "When the server receives a " +
                "CPMFetchValueIn message request from a client, " +
                "the server MUST check whether the client has a query" +
                "associated with it."));
            Helper.Requires(isConnected, 599, "All messages MUST be " +
                "preceded by an initial CPMConnectIn message.");
            pendingRequest = false;
        }

        /// <summary>
        /// This rule validates the possible error cases for the 
        /// CPMFetchValueIn request. 
        /// </summary>
        /// <param name="returnCode">Error code that comes from 
        /// the server response.</param>
        [Action("CPMFetchValueOutResponse(returnCode)")]
        static void CPMFetchValueOutError(uint returnCode)
        {

            Contracts.Requires(protocolState == WspMessage.CPMFetchValueIn
                               && returnCode != 0x00000000 //returnCode >= 0x80000000//
                               && (isQueryAssociated == false
                               || isHandleAssociated == false
                               || rowsReceived == false));

            if (isQueryAssociated == false)
            {

                Helper.Requires(returnCode
                    == (uint)WspErrorCode.STATUS_INVALID_PARAMETER, 728,
                    "When the server receives a CPMFetchValueIn message " +
                    "request from a client, the server MUST report a " +
                    "STATUS_INVALID_PARAMETER (0xC000000D) error if the " +
                    "client doesn't have a query associated with it.");
            }

            protocolState = WspMessage.ErrorState;
            pendingRequest = false;

            Requirements.Capture(RequirementId.Make("MS-WSP", 734, "When the server receives a " +
                "CPMFetchValueIn message request from a client, the server" +
                "MUST report an error if it fails to check the " +
                "property ID value."));
            Requirements.Capture(RequirementId.Make("MS-WSP", 736, "When the server receives a " +
                "CPMFetchValueIn message request from a client, the server " +
                "MUST report an error if it fails to serialize the property" +
                "value or copy the value to vValue field."));
            Requirements.Capture(RequirementId.Make("MS-WSP", 730, "When the server receives a " +
                "CPMFetchValueIn message request from a client, the server" +
                "MUST report an error if it fails to prepare " +
                "CPMFetchValueOut message."));
        }
        # endregion

        # region CPMFreeCursorIn and CPMFreeCursorOut

        /// <summary>
        /// CPMFreeCursorIn() requests to release a cursor.
        /// </summary>
        /// <param name="isCursorValid">indicated valid/invalid cursor</param>
        [Action("CPMFreeCursorIn(isCursorValid)")]
        static void CPMFreeCursorIn(bool isCursorValid)
        {
            isHandleAssociated = isCursorValid;
            protocolState = WspMessage.CPMFreeCursorIn;
            pendingRequest = true;
        }

        /// <summary>
        /// CPMFreeCursorOutResponse is the response for 
        /// CPMFreeCursorIn request.
        /// </summary>
        /// <param name="returnCode">Error code that comes from 
        /// the server response.</param>
        [Action("CPMFreeCursorOutResponse(returnCode)")]
        static void CPMFreeCursorOutResponse(uint returnCode)
        {

            Contracts.Requires(protocolState == WspMessage.CPMFreeCursorIn
                               && isQueryAssociated == true
                               && isHandleAssociated == true
                               && returnCode == 0x00000000 //returnCode < 0x80000000//
                               && isConnected == true);

            Helper.RequiresCapture(592,
                "[Windows Search Protocol (WSP) messages indicate success two ways,first way]A zero value (0x00000000).");

            Helper.Requires(isConnected, 599, "All messages MUST be preceded " +
                "by an initial CPMConnectIn message.");
            Requirements.Capture(RequirementId.Make("MS-WSP", 791, "When the server receives a " +
                "CPMFreeCursorIn message request from the client, " +
                "the server MUST check whether the client has a query" +
                "associated with it."));
            Requirements.Capture(RequirementId.Make("MS-WSP", 793, "When the server receives a " +
                "CPMFreeCursorIn message request from the client, " +
                "the server MUST check whether the cursor handle passed" +
                "is in the list of the client's cursor handles."));
            isQueryAssociated = false;
            isHandleAssociated = false;
            pendingRequest = false;

        }

        /// <summary>
        /// This rule validates the possible error cases for the 
        /// CPMFreeCursorIn request. 
        /// </summary>
        /// <param name="returnCode">Error code that comes from the 
        /// server response.</param>
        [Action("CPMFreeCursorOutResponse(returnCode)")]
        static void CPMFreeCursorOutResponseError(uint returnCode)
        {

            Contracts.Requires(protocolState == WspMessage.CPMFreeCursorIn
                               && returnCode != 0x00000000 //returnCode >= 0x80000000 //
                               && (isQueryAssociated == false
                               || isHandleAssociated == false));

            if (isQueryAssociated == false)
            {
                Helper.Requires(returnCode
                    == (uint)WspErrorCode.STATUS_INVALID_PARAMETER, 792,
                    "When the server receives a CPMFreeCursorIn message" +
                    "request from the client, the server MUST report a " +
                    "STATUS_INVALID_PARAMETER " +
                    "(0xC000000D) error if the client doesnot have a query" +
                    "associated with it.");
            }
            //================================  TDI 19903  ==================
            //else if (isHandleAssociated == false)
            //{
            //    Helper.Requires(returnCode == (uint)WspErrorCode.E_FAIL, 794,
            //        "When the server receives a CPMFreeCursorIn message " +
            //        "request from the client, the server MUST report an " +
            //        "E_FAIL (0x80004005) error if the cursor handle passed" +
            //        "is not in the list of the client's cursor handles.");
            //}
            protocolState = WspMessage.ErrorState;
            pendingRequest = false;
        }

        # endregion

        # region CPMDisconnect

        /// <summary>
        /// CPMDisconnect() request is sent to end the connection 
        /// with the server.
        /// </summary>
        [Action("CPMDisconnect()")]
        static void Disconnect()
        {
            isConnected = false;
            protocolState = WspMessage.None;
        }

        # endregion

        #region CPMFindIndicesIn and CPMFindIndicesOut
        /// <summary>
        /// CPMFindIndicesIn() message requests requests the rowset position of the next occurrence of a document identifier.
        /// </summary>
        /// <param name="isCursorValid">indicates valid/invalid cursor</param>
        [Action("CPMFindIndicesIn(isCursorValid)")]
        static void CPMFindIndicesIn(bool isCursorValid)
        {
            isHandleAssociated = isCursorValid;
            protocolState = WspMessage.CPMFindIndicesIn;
            pendingRequest = true;
        }

        /// <summary>
        /// CPMFindIndicesOut is the response for CPMFindIndicesIn
        /// </summary>
        /// <param name="returnCode"></param>
        [Action("CPMFindIndicesOutResponse(returnCode)")]
        static void CPMFindIndicesOut(uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMFindIndicesIn
                //&& Model.isValidBindingInfo == true
                                && returnCode == 0x00000000
                               && isQueryAssociated == true
                               && isHandleAssociated == true
                               && isConnected == true);

            Helper.RequiresCapture(592,
                "[Windows Search Protocol (WSP) messages indicate success two ways,first way]A zero value (0x00000000).");

            Helper.Requires(isConnected, 599, "All messages MUST be " +
                "preceded by an initial CPMConnectIn message.");
            pendingRequest = false;
        }

        /// <summary>
        /// This rule validates the possible error cases for the 
        /// CPMFindIndicesIn request. 
        /// </summary>
        /// <param name="returnCode"></param>
        [Action("CPMFindIndicesOutResponse(returnCode)")]
        static void CPMFindIndicesOut_StatusInvalidParameterError(uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMFindIndicesIn
                               && returnCode != 0x00000000 //returnCode >= 0x80000000//
                               && isQueryAssociated == false
                               );
            //&& isQueryAssociated == false
            //this requirement is blocked by TDI 42166, so it will be covered after TDI 42166 fixed
            //Helper.Requires(returnCode
            //    == (uint)WspErrorCode.STATUS_INVALID_PARAMETER, 1105,
            //    "[When the server receives a CPMFindIndicesIn message request from a client," +
            //    "If this is not the case[client dose not has a query associated with it]" +
            //    "The server MUST report a STATUS_INVALID_PARAMETER (0xC000000D) error.");

            Requirements.Capture(RequirementId.Make("MS-WSP", 1107,
                "[When the server receives a CPMFindIndicesIn message request from a client]" +
                "If this step fails for any reason, the server MUST report that an error was encountered."));
            pendingRequest = false;
        }

        #endregion

        #region CPMGetRowsetNotifyIn and CPMGetRowsetNotifyOut
        /// <summary>
        /// CPMGetRowsetNotifyIn message requests the next rowset event from the server if available
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="moreEvent"></param>
        [Action("CPMGetRowsetNotifyIn(eventType,moreEvent)")]
        static void CPMGetRowsetNotifyIn(int eventType, bool moreEvent)
        {
            protocolState = WspMessage.CPMGetRowsetNotifyIn;
            pendingRequest = true;
        }

        /// <summary>
        /// CPMGetRowsetNotidyOutResponse is the response for CPMGetRowsetNotifyIn
        /// </summary>
        /// <param name="returnCode"></param>
        [Action("CPMGetRowsetNotifyOutResponse(returnCode)")]
        static void CPMGetRowsetNotidyOutResponse(uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMGetRowsetNotifyIn
                //&& Model.isValidBindingInfo == true
                               && isQueryAssociated == true
                               && isHandleAssociated == true
                               && returnCode == 0x00000000
                               && isConnected == true);
            Helper.RequiresCapture(592,
                "[Windows Search Protocol (WSP) messages indicate success two ways,first way]A zero value (0x00000000).");

            Helper.Requires(isConnected, 599, "All messages MUST be " +
                "preceded by an initial CPMConnectIn message.");

            pendingRequest = false;
        }

        /// <summary>
        /// This rule validates the possible error cases for the 
        /// CPMGetRowsetNotifyIn request. 
        /// </summary>
        /// <param name="returnCode"></param>
        [Action("CPMGetRowsetNotifyOutResponse(returnCode)")]
        static void CPMGetRowsetNotidyOutResponse_StatusInvalidParameterError(uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMGetRowsetNotifyIn
                               && returnCode != 0x00000000 //returnCode >= 0x80000000//
                               && isQueryAssociated == false
                               );
            //&& isQueryAssociated == false

            Helper.Requires(returnCode
                == (uint)WspErrorCode.STATUS_INVALID_PARAMETER, 1112,
                "[When the server receives a CPMGetRowsetNotifyIn message request from a " +
                "client]If this is not the case[ the client dose not has a query associated with it], " +
                "the server MUST report a STATUS_INVALID PARAMETER (0xC000000D) error.");

            pendingRequest = false;
        }



        #endregion

        #region CPMSetScopePrioritizationIn and CPMSetScopePrioritizationOut

        /// <summary>
        /// CPMSetScopePrioritizationIn() request that server prioritize indexing of items that may be relevant to the originating query
        /// at a rate specified in the message
        /// </summary>
        /// <param name="priority"></param>
        [Action("CPMSetScopePrioritizationIn(priority)")]
        static void CPMSetScopePrioritizationIn(uint priority)
        {
            protocolState = WspMessage.CPMSetScopePrioritizationIn;
            pendingRequest = true;
        }

        /// <summary>
        /// CPMSetScopePrioritizationOutResponse() is the response of CPMSetScopePrioritizationIn()
        /// </summary>
        /// <param name="returnCode"></param>
        [Action("CPMSetScopePrioritizationOutResponse(returnCode)")]
        static void CPMSetScopePrioritizationOutResponse(uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMSetScopePrioritizationIn
                //&& Model.isValidBindingInfo == true
                               && isQueryAssociated == true
                               && isHandleAssociated == true
                               && returnCode == 0x00000000
                               && isConnected == true);
            Helper.RequiresCapture(592,
                "[Windows Search Protocol (WSP) messages indicate success two ways,first way]A zero value (0x00000000).");

            Helper.Requires(isConnected, 599, "All messages MUST be " +
                "preceded by an initial CPMConnectIn message.");

            pendingRequest = false;
        }

        /// <summary>
        /// This rule validates the possible error cases for the 
        /// CPMSetScopePrioritizationIn request. 
        /// </summary>
        /// <param name="returnCode"></param>
        [Action("CPMSetScopePrioritizationOutResponse(returnCode)")]
        static void CPMSetScopePrioritizationOutResponse_StatusInvalidParameterError(uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMSetScopePrioritizationIn
                               && returnCode != 0x00000000 //returnCode >= 0x80000000//
                               && isQueryAssociated == false
                               );

            Helper.Requires(returnCode
                == (uint)WspErrorCode.STATUS_INVALID_PARAMETER, 1159,
                "[When the server receives a CPMSetScopePrioritizationIn message request from a client, " +
                "the server MUST]If this is not the case[the client does not has a query associated with this call], " +
                "the server MUST report a STATUS_INVALID PARAMETER (0xC000000D) error.");

            pendingRequest = false;
        }



        #endregion

        # region CPMGetScopeStatisticsIn and CPMGetScopeStatisticsOut

        /// <summary>
        /// CPMGetScopeStatisticsIn() requests requests statistics regarding the number of indexed items.
        /// </summary>
        /// a valid cursor associated with the query</param>
        [Action("CPMGetScopeStatisticsIn()")]
        static void CPMGetScopeStatisticsIn()
        {
            protocolState = WspMessage.CPMGetScopeStatisticsIn;
            isConnected = true;
            pendingRequest = true;
        }

        /// <summary>
        /// CPMGetScopeStatisticsOutResponse is the response for 
        /// CPMGetScopeStatisticsIn request.
        /// </summary>
        /// <param name="returnCode">Error code that comes from 
        /// the server response.</param>
        [Action("CPMGetScopeStatisticsOutResponse(returnCode)")]
        static void CPMGetScopeStatisticsOutResponse(uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMGetScopeStatisticsIn
                               && returnCode == 0x00000000//returnCode < 0x80000000//
                               && isQueryAssociated == true
                               && isHandleAssociated == true
                               && isConnected == true);
            Helper.RequiresCapture(592,
                "[Windows Search Protocol (WSP) messages indicate success two ways,first way]A zero value (0x00000000).");

            Requirements.Capture(RequirementId.Make("MS-WSP", 599,
                "All messages MUST be preceded by an initial CPMConnectIn " +
                "message."));
            pendingRequest = false;
        }

        /// <summary>
        /// This rule validates the possible error cases for the 
        /// CPMGetScopeStatisticsIn request. 
        /// </summary>
        /// <param name="returnCode">Error code that comes from 
        /// the server response.</param>
        [Action("CPMGetScopeStatisticsOutResponse(returnCode)")]
        static void CPMGetScopeStatisticsOutResponse_failed(uint returnCode)
        {

            Contracts.Requires(protocolState == WspMessage.CPMGetScopeStatisticsIn
                               && returnCode != 0x00000000//returnCode >= 0x80000000//
                               && (isQueryAssociated == false
                               || isHandleAssociated == false
                               || isConnected == false));

            Helper.Requires(returnCode
                == (uint)WspErrorCode.STATUS_INVALID_PARAMETER, 1150,
                "[When the server receives a CPMGetScopeStatisticsIn " +
                "message request from a client, the server MUST]" +
                "If this is not the case[client does not has a " +
                "query associated with this method], " +
                "the server MUST report a STATUS_INVALID PARAMETER (0xC000000D) error.");
            protocolState = WspMessage.ErrorState;
            pendingRequest = false;
        }
        # endregion

        #region CPMUpdateDocumentsIn

        /// <summary>
        /// CPMUpdateDocumentsIn() directs the server to index the specified path
        /// </summary>
        /// <param name="_flag"></param>
        /// <param name="_fRootPath"></param>
        [Action("CPMUpdateDocumentsIn(_flag,_fRootPath)")]
        static void CPMUpdateDocumentsIn(uint _flag, uint _fRootPath)
        {
            protocolState = WspMessage.CPMUpdateDocumentsIn;
            isConnected = true;
            pendingRequest = true;
        }

        /// <summary>
        /// This rule validates the possible error cases for the 
        /// CPMUpdateDocumentsIn request. 
        /// </summary>
        /// <param name="returnCode"></param>
        [Action("CPMUpdateDocumentsOutResponse(returnCode)")]
        static void CPMUpdateDocumentsOutResponse_failed(uint returnCode)
        {
            Contracts.Requires(protocolState == WspMessage.CPMUpdateDocumentsIn
                               && returnCode != 0x00000000);

            Helper.Requires(returnCode
                == (uint)WspErrorCode.E_NOINTERFACE, 1052,
                "<28> Section 3.1.5.1.2: This message [CPMUpdateDocumentsIn] is not supported on Windows and returns E_NOINTERFACE in response.");

            pendingRequest = false;
        }


        #endregion

        #region GetServerPlatform
        /// <summary>
        /// Call of Get server's OS version
        /// </summary>
        [Action("call GetServerPlatform(_)")]
        public static void CallGetOS()
        {
        }
        /// <summary>
        /// Return of Get server's OS version
        /// </summary>
        /// <param name="platform">Represent server's platform type</param>
        /// <returns>The call must return true which indicates success</returns>
        [Action("return GetServerPlatform(out platform)/result")]
        public static bool ReturnGetOS(SkuOsVersion platform)
        {
            Contracts.Requires(
                platform == SkuOsVersion.NonWindows || platform == SkuOsVersion.Win2K3 ||
                platform == SkuOsVersion.Win2K8 ||
                platform == SkuOsVersion.Win2K8R2);

            serverInfo.ServerPlatForm = platform;
            return true;
        }
        #endregion

        [AcceptingStateConditionAttribute()]
        static bool IsAcceptingState()
        {
            return pendingRequest == false;
        }

    }
}
