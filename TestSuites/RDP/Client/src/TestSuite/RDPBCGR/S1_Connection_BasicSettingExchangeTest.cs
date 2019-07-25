// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;

namespace Microsoft.Protocols.TestSuites.Rdpbcgr
{
    public partial class RdpbcgrTestSuite
    {
        [TestMethod]
        [Priority(0)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]        
        [Description(@"This test case verifies that 
                        1. The correctness of Client MCS Connect Initial PDU with GCC Conference Create Request when the server did not advertise support for Extended Client Data Blocks.
                        2. SUT can process the valid Server MCS Connect Response PDU with GCC Conference Create Response correctly.")]
        public void S1_ConnectionTest_BasicSettingExchange_PositiveTest_ExtendedClientDataNotSupported()
        {
            #region Test Steps
            //1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase. Server should not set the EXTENDED_CLIENT_DATA_SUPPORTED flag in RDP Negotiation Response. 
            //2.	Test Suite expects SUT continue the connection sequence with sending a Client MCS Connect Initial PDU with GCC Conference Create Request. 
            //3.	Verify the received Client MCS Connect Initial PDU with GCC Conference Create Request and respond Server MCS Connect Response PDU with GCC Conference Create Response.
            //4.	Test Suite expects a Client MCS Erect Domain Request PDU to indicate the successful process of Server MCS Connect Response PDU with GCC Conference Create Response.
            #endregion

            #region
            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to initiate a RDP connection
            //Trigger client to initiate a RDP connection.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Expect the transport layer connection request
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and does not set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}; and set EXTENDED_CLIENT_DATA_SUPPORTED to false", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, false, true, NegativeType.None);

            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond A Server MCS Connect Response PDU with GCC Conference Create Response.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}.", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString());
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, rdpServerVersion, NegativeType.None);

            //Expect a Client MCS Erect Domain Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Erect Domain Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Erect_Domain_Request>(waitTime);
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]        
        [Description(@"This test case verifies that SUT drops the connection when the result field of the MCS Connect Response PDU is not set to rt-successful (non 0).")]
        public void S1_ConnectionTest_BasicSettingExchange_NegativeTest_MCSConnectResponseFailure()
        {
            #region Test Steps
            //1. Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase.
            //2. Test Suite expects SUT continue the connection sequence with sending a Client MCS Connect Initial PDU with GCC Conference Create Request.
            //3. Test Suite responds an invalid Server MCS Connect Response PDU with GCC Conference Create Response by setting the result field to a non rt-successful (non 0).
            //4. Test Suite expects SUT drop the connection.
            #endregion

            #region
            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to initiate a RDP connection
            //Trigger client to initiate a RDP connection.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Expect the transport layer connection request
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and does not set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, false, true, NegativeType.None);

            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond A Server MCS Connect Response PDU with GCC Conference Create Response having invalid result.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}; Invalid Result.", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString());
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, TS_UD_SC_CORE_version_Values.V2, NegativeType.InvalidResult);

            RDPClientTryDropConnection("MCS connection response");
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]        
        [Description(@"This test case verifies that SUT drops the connection when the H.221 nonstandard key embedded at the start of x224Ddata field is ANSI character string “McDn”.")]
        public void S1_ConnectionTest_BasicSettingExchange_NegativeTest_InvalidH221NonStandardkey()
        {
            #region Test Steps
            //1. Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase.
            //2. Test Suite expects SUT continue the connection sequence with sending a Client MCS Connect Initial PDU with GCC Conference Create Request.
            //3. Test Suite responds an invalid Server MCS Connect Response PDU with GCC Conference Create Response by setting the H.221 nonstandard key embedded at the start of x224Ddata field to an invalid value (not ANSI character string “McDn”).
            //4. Test Suite expects SUT drop the connection.
            #endregion

            #region
            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to initiate a RDP connection
            //Trigger client to initiate a RDP connection.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Expect the transport layer connection request
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and does not set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, false, true, NegativeType.None);

            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond A Server MCS Connect Response PDU with GCC Conference Create Response having invalid H221 key.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}; Invalid H221.", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString());
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, TS_UD_SC_CORE_version_Values.V2, NegativeType.InvalidH221);

            RDPClientTryDropConnection("MCS connection response with invalid H221 key");            
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]        
        [Description(@"This test case verifies that SUT drops the connection when the length field of User Data Header to an invalid value (less than the actual value).
            Comment: SDK does not expose method to get the length of user data as of 2011-11-17 ")]        
        public void S1_ConnectionTest_BasicSettingExchange_NegativeTest_InvalidEncodedLength()
        {
            #region Test Steps
            //1. Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase.
            //2. Test Suite expects SUT continue the connection sequence with sending a Client MCS Connect Initial PDU with GCC Conference Create Request.
            //3. Test Suite responds an invalid Server MCS Connect Response PDU with GCC Conference Create Response by setting the length field of User Data Header to an invalid value (less than the actual value).
            //4. Test Suite expects SUT drop the connection.
            #endregion

            #region
            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to initiate a RDP connection
            //Trigger client to initiate a RDP connection.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Expect the transport layer connection request
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and does not set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, false, true, NegativeType.None);

            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond A Server MCS Connect Response PDU with GCC Conference Create Response having invalid encoded length.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}; Invalid encoded length.", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString());
            if (this.selectedProtocol == selectedProtocols_Values.PROTOCOL_RDP_FLAG)
                this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, TS_UD_SC_CORE_version_Values.V2, NegativeType.InvalidEncodedLength);
            else
                this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, TS_UD_SC_CORE_version_Values.V2, NegativeType.InvalidEncodedLengthExternalSecurityProtocols);

            RDPClientTryDropConnection("MCS connection response with invalid User Data Header");            
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]        
        [Description(@"This test case verifies that SUT drops the connection when the clientReaquestedProtocols field in the Server Core Data is not same as that received in RDP Negotiation Response.")]
        public void S1_ConnectionTest_BasicSettingExchange_NegativeTest_InvalidClientReaquestedProtocols()
        {
            #region Test Steps
            //1. Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase.
            //2. Test Suite expects SUT continue the connection sequence with sending a Client MCS Connect Initial PDU with GCC Conference Create Request.
            //3. Test Suite responds an invalid Server MCS Connect Response PDU with GCC Conference Create Response and set clientReaquestedProtocols field in the Server Core Data to a value different with that sent in RDP Negotiation Response.
            //4. Test Suite expects SUT drop the connection.
            #endregion

            #region
            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to initiate a RDP connection
            //Trigger client to initiate a RDP connection.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Expect the transport layer connection request
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and does not set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, false, true, NegativeType.None);

            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond A Server MCS Connect Response PDU with GCC Conference Create Response having invalid Client Requested protocol
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}; Invalid Client Requested Protocol.", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString());
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, TS_UD_SC_CORE_version_Values.V2, NegativeType.InvalidClientRequestedProtocols);

            RDPClientTryDropConnection("MCS connection response with invalid Client Requested protocol");            
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"Potential TDI. This test case verifies that SUT drops the connection when the encryptionMethod field in the Server Security Data is not valid or not supported.")]
        public void S1_ConnectionTest_BasicSettingExchange_NegativeTest_InvalidEncryptionMethod()
        {
            #region Test Steps
            //1. Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase.
            //2. Test Suite expects SUT continue the connection sequence with sending a Client MCS Connect Initial PDU with GCC Conference Create Request.
            //3. Test Suite responds an invalid Server MCS Connect Response PDU with GCC Conference Create Response and set the encryptionMethod field in the Server Security Data to an invalid Encryption Method identifier.
            //4. Test Suite expects SUT drop the connection.
            #endregion

            #region
            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to initiate a RDP connection
            //Trigger client to initiate a RDP connection.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Expect the transport layer connection request
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and does not set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, false, true, NegativeType.None);

            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond A Server MCS Connect Response PDU with GCC Conference Create Response and set the encryptionMethod field in the Server Security Data to an invalid Encryption Method identifier.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}; Invalid encryptionMethod field.", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString());
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, TS_UD_SC_CORE_version_Values.V2, NegativeType.InvalidEncryptionMethod);

            if (isWindowsImplementation)
            {
                //According to TDI#66010, the Server Security Data is only evaluated when the client security layer is connected, which is after the channels have been joined.
                #region Channel Connection phase
                //Expect a Client MCS Erect Domain Request PDU.
                this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Erect Domain Request PDU.");
                this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Erect_Domain_Request>(waitTime);

                //Expect a Client MCS Attach User Request PDU
                this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Attach User Request PDU.");
                this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Attach_User_Request>(waitTime);

                //Respond a Server MCS Channel Join Confirm PDU.
                this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Channel Join Confirm PDU to SUT.");
                this.rdpbcgrAdapter.MCSAttachUserConfirm(NegativeType.None);

                //Expect SUT start a channel join sequence.
                this.TestSite.Log.Add(LogEntryKind.Comment, "Expect SUT to start the  channel join sequence.");
                this.rdpbcgrAdapter.ChannelJoinRequestAndConfirm(NegativeType.None);
                #endregion
            }

            RDPClientTryDropConnection("MCS connection response with invalid/unspported encryptionMethod field in the Server Security Data");            
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]        
        [Description(@"This test case verifies that SUT drops the connection when the serverRandomLen field in the Server Security Data is not valid.")]
        public void S1_ConnectionTest_BasicSettingExchange_NegativeTest_InvalidServerRandomLen()
        {
            #region Test Steps
            //1. Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase.
            //2. Test Suite expects SUT continue the connection sequence with sending a Client MCS Connect Initial PDU with GCC Conference Create Request.
            //3. Test Suite responds an invalid Server MCS Connect Response PDU with GCC Conference Create Response and set the serverRandomLen field in the Server Security Data to an invalid value (non 32).
            //4. Test Suite expects SUT drop the connection.
            #endregion

            #region

            if (this.selectedProtocol != selectedProtocols_Values.PROTOCOL_RDP_FLAG)
            {
                this.TestSite.Log.Add(LogEntryKind.Debug, "This case requires using RDP encrypted protocol");
                return;
            }

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to initiate a RDP connection
            //Trigger client to initiate a RDP connection.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Expect the transport layer connection request
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and does not set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, false, true, NegativeType.None);

            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond A Server MCS Connect Response PDU with GCC Conference Create Response and set the serverRandomLen field in the Server Security Data to an invalid value.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}; Invalid serverRandomLen value.", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString());
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, TS_UD_SC_CORE_version_Values.V2, NegativeType.InvalidServerRandomLen);

            if (isWindowsImplementation)
            {
                //According to TDI#66010, the Server Security Data is only evaluated when the client security layer is connected, which is after the channels have been joined.
                #region Channel Connection phase
                //Expect a Client MCS Erect Domain Request PDU.
                this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Erect Domain Request PDU.");
                this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Erect_Domain_Request>(waitTime);

                //Expect a Client MCS Attach User Request PDU
                this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Attach User Request PDU.");
                this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Attach_User_Request>(waitTime);

                //Respond a Server MCS Channel Join Confirm PDU.
                this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Channel Join Confirm PDU to SUT.");
                this.rdpbcgrAdapter.MCSAttachUserConfirm(NegativeType.None);

                //Expect SUT start a channel join sequence.
                this.TestSite.Log.Add(LogEntryKind.Comment, "Expect SUT to start the  channel join sequence.");
                this.rdpbcgrAdapter.ChannelJoinRequestAndConfirm(NegativeType.None);
                #endregion
            }

            RDPClientTryDropConnection("MCS connection response with invalid serverRandomLen field in the Server Security Data");            
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]        
        [Description(@"This test case verifies that SUT drops the connection when the serverCertificate field in the Server Security Data is not valid.")]
        public void S1_ConnectionTest_BasicSettingExchange_NegativeTest_InvalidServerCertificate()
        {
            #region Test Steps
            //1. Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase.
            //2. Test Suite expects SUT continue the connection sequence with sending a Client MCS Connect Initial PDU with GCC Conference Create Request.
            //3. Test Suite responds an invalid Server MCS Connect Response PDU with GCC Conference Create Response and set the serverCertificate field in the Server Security Data to an invalid value.
            //4. Test Suite expects SUT drop the connection.
            #endregion

            #region

            if (this.selectedProtocol != selectedProtocols_Values.PROTOCOL_RDP_FLAG)
            {
                this.TestSite.Log.Add(LogEntryKind.Debug, "This case requires using RDP encrypted protocol");
                return;
            }

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to initiate a RDP connection
            //Trigger client to initiate a RDP connection.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Expect the transport layer connection request
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and does not set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, false, true, NegativeType.None);

            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond A Server MCS Connect Response PDU with GCC Conference Create Response and set the serverCertificate field in the Server Security Data to an invalid value.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}; Invalid serverCertificate value.", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString());
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, TS_UD_SC_CORE_version_Values.V2, NegativeType.InvalidServerCertificate);

            if (isWindowsImplementation)
            {
                //According to TDI#66010, the Server Security Data is only evaluated when the client security layer is connected, which is after the channels have been joined.
                #region Channel Connection phase
                //Expect a Client MCS Erect Domain Request PDU.
                this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Erect Domain Request PDU.");
                this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Erect_Domain_Request>(waitTime);

                //Expect a Client MCS Attach User Request PDU
                this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Attach User Request PDU.");
                this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Attach_User_Request>(waitTime);

                //Respond a Server MCS Channel Join Confirm PDU.
                this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Channel Join Confirm PDU to SUT.");
                this.rdpbcgrAdapter.MCSAttachUserConfirm(NegativeType.None);

                //Expect SUT start a channel join sequence.
                this.TestSite.Log.Add(LogEntryKind.Comment, "Expect SUT to start the  channel join sequence.");
                this.rdpbcgrAdapter.ChannelJoinRequestAndConfirm(NegativeType.None);
                #endregion
            }

            RDPClientTryDropConnection("MCS connection response with invalid serverCertificate field in the Server Security Data");            
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case verifies that SUT can connect to a RDP 10.0 Server.")]
        public void S1_ConnectionTest_BasicSettingExchange_PositiveTest_V10Server()
        {
            #region Test Description
            /*
            This test case tests:
            1) The correctness of Client MCS Connect Initial PDU with GCC Conference Create Request 
               when the server advertised support for Extended Client Data Blocks.
            2) SUT can process the valid Server MCS Connect Response PDU with GCC Conference Create Response from RDP 10.0 Server correctly.

            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase. 
                Server should set the EXTENDED_CLIENT_DATA_SUPPORTED flag in RDP Negotiation Response. 
            2.	Test Suite expects SUT continue the connection sequence with sending a 
                Client MCS Connect Initial PDU with GCC Conference Create Request. 
            3.	Verify the received Client MCS Connect Initial PDU with GCC Conference Create Request 
                and respond Server MCS Connect Response PDU with GCC Conference Create Response.
                Server should set the version as 0x00080005 in TS_UD_SC_CORE.
            4.	Test Suite expects a Client MCS Erect Domain Request PDU to indicate the successful process 
                of Server MCS Connect Response PDU with GCC Conference Create Response.
            */
            #endregion

            #region Test Sequence

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to initiate a RDP connection
            //Trigger client to initiate a RDP connection.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Expect the transport layer connection request
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}; Extended Client Data supported: true", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, true, true, NegativeType.None);

            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond A Server MCS Connect Response PDU with GCC Conference Create Response.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}.", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V3.ToString());
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, TS_UD_SC_CORE_version_Values.V3, NegativeType.None);

            ////Expect a Client MCS Erect Domain Request PDU.
            //this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Erect Domain Request PDU.");
            //this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Erect_Domain_Request>(waitTime);

            #region Channel Connection phase
            //Expect a Client MCS Erect Domain Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Erect Domain Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Erect_Domain_Request>(waitTime);

            //Expect a Client MCS Attach User Request PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Attach User Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Attach_User_Request>(waitTime);

            //Respond a Server MCS Channel Join Confirm PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Channel Join Confirm PDU to SUT.");
            this.rdpbcgrAdapter.MCSAttachUserConfirm(NegativeType.None);

            //Expect SUT start a channel join sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expect SUT to start the  channel join sequence.");
            this.rdpbcgrAdapter.ChannelJoinRequestAndConfirm(NegativeType.None);
            #endregion

            #region RDP Security Commencement phase
            //Expects SUT continue the connection by sending a Client Security Exchange PDU 
            //(if Standard RDP Security mechanisms are being employed) or a Client Info PDU.
            if (transportProtocol == EncryptedProtocol.Rdp)
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send Client a Security Exchange PDU.");
                this.rdpbcgrAdapter.ExpectPacket<Client_Security_Exchange_Pdu>(waitTime);
            }
            #endregion

            #region Secure Setting Exchange phase
            //Expect a Client Info PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send Client a Client Info PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_Info_Pdu>(waitTime);
            #endregion

            #region Licensing phase
            //Send SUT a Server License Error Pdu - Valid Client.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server License Error Pdu - Valid Client to SUT.");
            this.rdpbcgrAdapter.Server_License_Error_Pdu_Valid_Client(NegativeType.None);
            #endregion

            #region Capabilities Exchange phase
            //Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);
            //Send a Server Demand Active PDU to SUT and advertise the support for the Bitmap Host Cache Support Capability Set.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server Demand Active PDU to SUT and advertise the support for the Bitmap Host Cache Support Capability Set.");
            this.rdpbcgrAdapter.Server_Demand_Active(NegativeType.None);

            //Expect SUT respond a Client Confirm Active PDU.
            //Once the Confirm Active PDU has been sent, the client can start sending input PDUs (see section 2.2.8) to the server.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Confirm Active PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_Confirm_Active_Pdu>(waitTime);
            #endregion

            #region Connection Finalization phase
            //Expect SUT send a Client Synchronize PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Synchronize PDU.");
            this.rdpbcgrAdapter.WaitForPacket<Client_Synchronize_Pdu>(waitTime);

            //Send a Server Synchronize PDU to SUT.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server Synchronize PDU to SUT.");
            this.rdpbcgrAdapter.ServerSynchronize();

            //Send a Server Control Cooperate PDU to SUT.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server Control Cooperate PDU to SUT.");
            this.rdpbcgrAdapter.ServerControlCooperate();

            //Expect SUT send a Client Control PDU - Cooperate.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Control PDU - Cooperate.");
            this.rdpbcgrAdapter.WaitForPacket<Client_Control_Pdu_Cooperate>(waitTime);

            //Expect SUT send a Client Control PDU - Request Control.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Control PDU - Request Control.");
            this.rdpbcgrAdapter.WaitForPacket<Client_Control_Pdu_Request_Control>(waitTime);

            //Respond a Server Control Granted Control PDU to SUT.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server Control Granted Control PDU to SUT.");
            this.rdpbcgrAdapter.ServerControlGrantedControl();

            if (this.isclientSupportPersistentBitmapCache)
            {
                //Expect a Client Persistent Key List PDU.
                this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Persistent Key List PDU.");
                this.rdpbcgrAdapter.WaitForPacket<Client_Persistent_Key_List_Pdu>(waitTime);
            }

            //Expect a Client Font List PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Client Font List PDU.");
            this.rdpbcgrAdapter.WaitForPacket<Client_Font_List_Pdu>(waitTime);

            //Send a Server Font Map PDU to SUT.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server Font Map PDU to SUT.");
            this.rdpbcgrAdapter.ServerFontMap();
            #endregion


            #endregion
        }


        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case verifies that SUT can process the earlyCapabilityFlags of the TS_US_SC_CORE correctly with supporting version 1 RNS_UD_SC_EDGE_ACTIONS.")]
        public void S1_ConnectionTest_BasicSettingExchange_PositiveTest_V1RnsUdScEdgeActionsSupported()
        {
            #region Test Description
            /*
            This test case tests:
            1) The correctness of Client MCS Connect Initial PDU with GCC Conference Create Request 
               when the server advertised support for Extended Client Data Blocks.
            2) SUT can process the valid Server MCS Connect Response PDU with GCC Conference Create Response with earlyCapabilityFlags is RNS_UD_SC_EDGE_ACTIONS_SUPPORTED_V1 correctly.

            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase. 
                Server should set the EXTENDED_CLIENT_DATA_SUPPORTED flag in RDP Negotiation Response. 
            2.	Test Suite expects SUT continue the connection sequence with sending a 
                Client MCS Connect Initial PDU with GCC Conference Create Request. 
            3.	Verify the received Client MCS Connect Initial PDU with GCC Conference Create Request 
                and respond Server MCS Connect Response PDU with GCC Conference Create Response.
                Server should set earlyCapabiliyFlags as RNS_UD_SC_EDGE_ACTIONS_SUPPORTED_V1 in TS_UD_SC_SEC1.
            4.	Test Suite expects a Client MCS Erect Domain Request PDU to indicate the successful process 
                of Server MCS Connect Response PDU with GCC Conference Create Response.
            */
            #endregion

            #region Test Sequence

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to initiate a RDP connection
            //Trigger client to initiate a RDP connection.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Expect the transport layer connection request
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}; Extended Client Data supported: true", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, true, true, NegativeType.None);

            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond A Server MCS Connect Response PDU with GCC Conference Create Response.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}; EarlyCapabilityFlags: {3}.", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString(), SC_earlyCapabilityFlags_Values.RNS_UD_SC_EDGE_ACTIONS_SUPPORTED.ToString());
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, TS_UD_SC_CORE_version_Values.V2, NegativeType.None, MULTITRANSPORT_TYPE_FLAGS.None, true, SC_earlyCapabilityFlags_Values.RNS_UD_SC_EDGE_ACTIONS_SUPPORTED);

            //Expect a Client MCS Erect Domain Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Erect Domain Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Erect_Domain_Request>(waitTime);

            #endregion
        }


        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case verifies that SUT can process the earlyCapabilityFlags of the TS_US_SC_CORE correctly with supporting version 2 RNS_UD_EDGE_ACTIONS.")]
        public void S1_ConnectionTest_BasicSettingExchange_PositiveTest_V2RnsUdScEdgeActionsSupported()
        {
            #region Test Steps
            /*
                1).	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase. Server should not set the EXTENDED_CLIENT_DATA_SUPPORTED flag in RDP Negotiation Response. 
                2).	Test Suite expects SUT continue the connection sequence with sending a Client MCS Connect Initial PDU with GCC Conference Create Request. 
                3).	Verify the received Client MCS Connect Initial PDU with GCC Conference Create Request and respond Server MCS Connect Response PDU with GCC Conference Create Response. 
                    Server should set earlyCapabiliyFlags = RNS_UD_SC_EDGE_ACTIONS_SUPPORTED_V2 in TS_UD_SC_SEC1.
                4).	Test Suite expects a Client MCS Erect Domain Request PDU to indicate the successful process of Server MCS Connect Response PDU with GCC Conference Create Response.
            */
            #endregion

            #region
            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to initiate a RDP connection
            //Trigger client to initiate a RDP connection.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Expect the transport layer connection request
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and does not set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}; and set EXTENDED_CLIENT_DATA_SUPPORTED to false", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, false, true, NegativeType.None);

            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond A Server MCS Connect Response PDU with GCC Conference Create Response.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}; EarlyCapabilityFlags: {3}.", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString(), SC_earlyCapabilityFlags_Values.RNS_UD_SC_EDGE_ACTIONS_SUPPORTED_V2.ToString());
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, rdpServerVersion, NegativeType.None, MULTITRANSPORT_TYPE_FLAGS.None, true, SC_earlyCapabilityFlags_Values.RNS_UD_SC_EDGE_ACTIONS_SUPPORTED_V2);

            //Expect a Client MCS Erect Domain Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Erect Domain Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Erect_Domain_Request>(waitTime);
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case verifies that SUT can process the earlyCapabilityFlags of the TS_US_SC_CORE correctly with supporting version 1 and version 2 RNS_UD_SC_EDGE_ACTIONS.")]
        public void S1_ConnectionTest_BasicSettingExchange_PositiveTest_V1andV2RnsUdScEdgeActionsSupported()
        {
            #region Test Description
            /*
            This test case tests:
            1) The correctness of Client MCS Connect Initial PDU with GCC Conference Create Request 
               when the server advertised support for Extended Client Data Blocks.
            2) SUT can process the valid Server MCS Connect Response PDU with GCC Conference Create Response 
               with supporting RNS_UD_SC_EDGE_ACTIONS_SUPPORTED_V1 and RNS_UD_SC_EDGE_ACTIONS_SUPPORTED_V2 correctly.

            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase. 
                Server should set the EXTENDED_CLIENT_DATA_SUPPORTED flag in RDP Negotiation Response. 
            2.	Test Suite expects SUT continue the connection sequence with sending a 
                Client MCS Connect Initial PDU with GCC Conference Create Request. 
            3.	Verify the received Client MCS Connect Initial PDU with GCC Conference Create Request 
                and respond Server MCS Connect Response PDU with GCC Conference Create Response.
                Server should set earlyCapabiliyFlags as RNS_UD_SC_EDGE_ACTIONS_SUPPORTED_V1 | RNS_UD_SC_EDGE_ACTIONS_SUPPORTED_V2 in TS_UD_SC_SEC1.
            4.	Test Suite expects a Client MCS Erect Domain Request PDU to indicate the successful process 
                of Server MCS Connect Response PDU with GCC Conference Create Response.
            */
            #endregion

            #region Test Sequence

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to initiate a RDP connection
            //Trigger client to initiate a RDP connection.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Expect the transport layer connection request
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}; Extended Client Data supported: true", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, true, true, NegativeType.None);

            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //if (this.rdpbcgrAdapter.SessionContext.ClientRequestedProtocol == null)
            //{

            //}

            /*
             * A 32-bit, unsigned integer that contains the flags sent by the client in the requestedProtocols field of the RDP Negotiation Request (section 2.2.1.1.1). In the event that an RDP Negotiation Request was not received from the client, this field MUST be initialized to PROTOCOL_RDP (0). If this field is not present, all of the subsequent fields MUST NOT be present.
              not sure when these cases for earlyCapabilityFlags are not needed!
             */

            //Respond A Server MCS Connect Response PDU with GCC Conference Create Response.
            SC_earlyCapabilityFlags_Values earlyCapabilityFlagsValue = SC_earlyCapabilityFlags_Values.RNS_UD_SC_EDGE_ACTIONS_SUPPORTED | SC_earlyCapabilityFlags_Values.RNS_UD_SC_EDGE_ACTIONS_SUPPORTED_V2;
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}; EarlyCapabilityFlags: {3}.", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString(), SC_earlyCapabilityFlags_Values.RNS_UD_SC_DYNAMIC_DST_SUPPORTED.ToString() + " | " + SC_earlyCapabilityFlags_Values.RNS_UD_SC_EDGE_ACTIONS_SUPPORTED_V2.ToString());
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, TS_UD_SC_CORE_version_Values.V2, NegativeType.None, MULTITRANSPORT_TYPE_FLAGS.None, true, earlyCapabilityFlagsValue);

            //Expect a Client MCS Erect Domain Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Erect Domain Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Erect_Domain_Request>(waitTime);

            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case verifies that SUT can process the earlyCapabilityFlags of the TS_US_SC_CORE correctly with supporting RNS_UD_SC_DYNAMIC_DST.")]
        public void S1_ConnectionTest_BasicSettingExchange_PositiveTest_RnsUdScDynamicDstSupported()
        {
            #region Test Steps
            /*
                1).	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase. Server should not set the EXTENDED_CLIENT_DATA_SUPPORTED flag in RDP Negotiation Response. 
                2).	Test Suite expects SUT continue the connection sequence with sending a Client MCS Connect Initial PDU with GCC Conference Create Request. 
                3).	Verify the received Client MCS Connect Initial PDU with GCC Conference Create Request and respond Server MCS Connect Response PDU with GCC Conference Create Response. 
                    Server should set earlyCapabiliyFlags = RNS_UD_SC_DYNAMIC_DST_SUPPORTED in TS_UD_SC_SEC1.
                4).	Test Suite expects a Client MCS Erect Domain Request PDU to indicate the successful process of Server MCS Connect Response PDU with GCC Conference Create Response.
            */
            #endregion

            #region
            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to initiate a RDP connection
            //Trigger client to initiate a RDP connection.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Expect the transport layer connection request
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and does not set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}; and set EXTENDED_CLIENT_DATA_SUPPORTED to false", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, false, true, NegativeType.None);

            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond A Server MCS Connect Response PDU with GCC Conference Create Response.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}; EarlyCapabilityFlags: {3}.", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString(), SC_earlyCapabilityFlags_Values.RNS_UD_SC_DYNAMIC_DST_SUPPORTED.ToString());
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, rdpServerVersion, NegativeType.None, MULTITRANSPORT_TYPE_FLAGS.None, true, SC_earlyCapabilityFlags_Values.RNS_UD_SC_DYNAMIC_DST_SUPPORTED);

            //Expect a Client MCS Erect Domain Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Erect Domain Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Erect_Domain_Request>(waitTime);
            #endregion
        }


        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case verifies that SUT can process the encryptionMethod and encryptionLevel of the TS_UD_SC_SEC1 correctly.")]
        public void S1_ConnectionTest_BasicSettingExchange_PositiveTest_EncryptionMethodandLevel()
        {
            #region Test Steps
            /*
                1). Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase.
                2). Test Suite expects SUT continue the connection sequence with sending a Client MCS Connect Initial PDU with GCC Conference Create Request.
                3). Test Suite responds an valid Server MCS Connect Response PDU with GCC Conference Create Response. 
                    In this Response, Server set EncryptionMethod = ENCRYPTION_METHOD_56BIT, EncryptionLevel = ENCRYPTION_LEVEL_HIGH.
                4). Test Suite expects SUT drop the connection.
            */
            #endregion

            #region

            // If not use Standard RDP Security, return.
            if (this.selectedProtocol != selectedProtocols_Values.PROTOCOL_RDP_FLAG)
            {
                this.TestSite.Log.Add(LogEntryKind.Debug, "This case requires using RDP encrypted protocol");
                return;
            }

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to initiate a RDP connection
            //Trigger client to initiate a RDP connection.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Expect the transport layer connection request
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and does not set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, false, true, NegativeType.None);

            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond A Server MCS Connect Response PDU with GCC Conference Create Response and set the serverRandomLen field in the Server Security Data to an invalid value.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}; Invalid serverRandomLen value.", EncryptionMethods.ENCRYPTION_METHOD_56BIT.ToString(), EncryptionLevel.ENCRYPTION_LEVEL_HIGH.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString());
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(EncryptionMethods.ENCRYPTION_METHOD_56BIT, EncryptionLevel.ENCRYPTION_LEVEL_HIGH, TS_UD_SC_CORE_version_Values.V2, NegativeType.None);

            //Expect a Client MCS Erect Domain Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Erect Domain Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Erect_Domain_Request>(waitTime);
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case verifies that SUT can process the boundary value (65534) of MCSChannelId field in Server Network Data.")]
        public void S1_ConnectionTest_BasicSettingExchange_PositiveTest_MCSChannelIdOfServerNetworkData_LessMaxValue()
        {
            #region Test Description
            /*
            This test case tests:
            1) The correctness of Client MCS Connect Initial PDU with GCC Conference Create Request 
               when the server advertised support for Extended Client Data Blocks.
            2) SUT can process the valid Server MCS Connect Response PDU with GCC Conference Create Response with MCSChannelId = 65534 in Server Netword Data correctly.

            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase. 
                Server should set the EXTENDED_CLIENT_DATA_SUPPORTED flag in RDP Negotiation Response. 
            2.	Test Suite expects SUT continue the connection sequence with sending a 
                Client MCS Connect Initial PDU with GCC Conference Create Request. 
            3.	Verify the received Client MCS Connect Initial PDU with GCC Conference Create Request 
                and respond Server MCS Connect Response PDU with GCC Conference Create Response.
                Server set the MCSChannelId = 65534 in Server Network Data.
            4.	Test Suite expects a Client MCS Erect Domain Request PDU to indicate the successful process 
                of Server MCS Connect Response PDU with GCC Conference Create Response.
            */
            #endregion

            #region Test Sequence

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to initiate a RDP connection
            //Trigger client to initiate a RDP connection.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Expect the transport layer connection request
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}; Extended Client Data supported: true", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, true, true, NegativeType.None);

            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond A Server MCS Connect Response PDU with GCC Conference Create Response.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}; MCSChannelId: {3}", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString(), ConstValue.ID_LessMax);
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, TS_UD_SC_CORE_version_Values.V2, NegativeType.None, MULTITRANSPORT_TYPE_FLAGS.None, false, 0, ConstValue.ID_LessMax);
            // TODO: give 65535 would failed, need to recheck the boundary of MCSChannelID.

            //Expect a Client MCS Erect Domain Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Erect Domain Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Erect_Domain_Request>(waitTime);

            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case verifies that SUT can process the boundary value (1) of MCSChannelId field in Server Network Data.")]
        public void S1_ConnectionTest_BasicSettingExchange_PositiveTest_MCSChannelIdOfServerNetworkData_ONE()
        {
            #region Test Description
            /*
            This test case tests:
            1) The correctness of Client MCS Connect Initial PDU with GCC Conference Create Request 
               when the server advertised support for Extended Client Data Blocks.
            2) SUT can process the valid Server MCS Connect Response PDU with GCC Conference Create Response with MCSChannelId = 1 in Server Netword Data correctly.

            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase. 
                Server should set the EXTENDED_CLIENT_DATA_SUPPORTED flag in RDP Negotiation Response. 
            2.	Test Suite expects SUT continue the connection sequence with sending a 
                Client MCS Connect Initial PDU with GCC Conference Create Request. 
            3.	Verify the received Client MCS Connect Initial PDU with GCC Conference Create Request 
                and respond Server MCS Connect Response PDU with GCC Conference Create Response.
                Server set the MCSChannelId as 1 in Server Network Data.
            4.	Test Suite expects a Client MCS Erect Domain Request PDU to indicate the successful process 
                of Server MCS Connect Response PDU with GCC Conference Create Response.
            */
            #endregion

            #region Test Sequence

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to initiate a RDP connection
            //Trigger client to initiate a RDP connection.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Expect the transport layer connection request
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}; Extended Client Data supported: true", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, true, true, NegativeType.None);

            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond A Server MCS Connect Response PDU with GCC Conference Create Response.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}; MCSChannelId: {3}", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString(), ConstValue.ID_ONE);
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, TS_UD_SC_CORE_version_Values.V2, NegativeType.None, MULTITRANSPORT_TYPE_FLAGS.None, false, 0, ConstValue.ID_ONE);
            // TODO: give 65535 would failed, need to recheck the boundary of MCSChannelID.

            //Expect a Client MCS Erect Domain Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Erect Domain Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Erect_Domain_Request>(waitTime);

            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case verifies that SUT can process the boundary value (0) of MCSChannelId field in Server Network Data.")]
        public void S1_ConnectionTest_BasicSettingExchange_PositiveTest_MCSChannelIdOfServerNetworkData_ZERO()
        {
            #region Test Description
            /*
            This test case tests:
            1) The correctness of Client MCS Connect Initial PDU with GCC Conference Create Request 
               when the server advertised support for Extended Client Data Blocks.
            2) SUT can process the valid Server MCS Connect Response PDU with GCC Conference Create Response with MCSChannelId = 0 in Server Netword Data correctly.

            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase. 
                Server should set the EXTENDED_CLIENT_DATA_SUPPORTED flag in RDP Negotiation Response. 
            2.	Test Suite expects SUT continue the connection sequence with sending a 
                Client MCS Connect Initial PDU with GCC Conference Create Request. 
            3.	Verify the received Client MCS Connect Initial PDU with GCC Conference Create Request 
                and respond Server MCS Connect Response PDU with GCC Conference Create Response.
                Server set the MCSChannelId as 0 in Server Network Data.
            4.	Test Suite expects a Client MCS Erect Domain Request PDU to indicate the successful process 
                of Server MCS Connect Response PDU with GCC Conference Create Response.
            */
            #endregion

            #region Test Sequence

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to initiate a RDP connection
            //Trigger client to initiate a RDP connection.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Expect the transport layer connection request
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}; Extended Client Data supported: true", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, true, true, NegativeType.None);

            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond A Server MCS Connect Response PDU with GCC Conference Create Response.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}; MCSChannelId: {3}", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString(), ConstValue.ID_ZERO);
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, TS_UD_SC_CORE_version_Values.V2, NegativeType.None, MULTITRANSPORT_TYPE_FLAGS.None, false, 0, ConstValue.ID_ZERO);
            // TODO: give 65535 would failed, need to recheck the boundary of MCSChannelID.

            //Expect a Client MCS Erect Domain Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Erect Domain Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Erect_Domain_Request>(waitTime);

            #endregion
        }



        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case verifies that SUT can process the boundary value (65535) of MCSChannelId field in Server Message Channel Data.")]
        public void S1_ConnectionTest_BasicSettingExchange_PositiveTest_MCSChannelIdOfServerMessageChannelData_MaxValue()
        {
            #region Test Description
            /*
            This test case tests:
            1) The correctness of Client MCS Connect Initial PDU with GCC Conference Create Request 
               when the server advertised support for Extended Client Data Blocks.
            2) SUT can process the valid Server MCS Connect Response PDU with GCC Conference Create Response with MCSChannelId = 65535 in Server Message Channel Data correctly.

            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase. 
                Server should set the EXTENDED_CLIENT_DATA_SUPPORTED flag in RDP Negotiation Response. 
            2.	Test Suite expects SUT continue the connection sequence with sending a 
                Client MCS Connect Initial PDU with GCC Conference Create Request. 
            3.	Verify the received Client MCS Connect Initial PDU with GCC Conference Create Request 
                and respond Server MCS Connect Response PDU with GCC Conference Create Response.
                Server set the MCSChannelId = 65535 in Server Message Channel Data.
            4.	Test Suite expects a Client MCS Erect Domain Request PDU to indicate the successful process 
                of Server MCS Connect Response PDU with GCC Conference Create Response.
            */
            #endregion

            #region Test Sequence

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to initiate a RDP connection
            //Trigger client to initiate a RDP connection.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Expect the transport layer connection request
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}; Extended Client Data supported: true", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, true, true, NegativeType.None);

            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond A Server MCS Connect Response PDU with GCC Conference Create Response.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}; MCSChannelId: {3}", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString(), ConstValue.ID_MAX);
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, TS_UD_SC_CORE_version_Values.V2, NegativeType.None, MULTITRANSPORT_TYPE_FLAGS.None, false, 0, ConstValue.IO_CHANNEL_ID, ConstValue.ID_MAX);

            //Expect a Client MCS Erect Domain Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Erect Domain Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Erect_Domain_Request>(waitTime);

            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case verifies that SUT can process the boundary value (65534) of MCSChannelId field in Server Message Channel Data.")]
        public void S1_ConnectionTest_BasicSettingExchange_PositiveTest_MCSChannelIdOfServerMessageChannelData_LessMaxValue()
        {
            #region Test Description
            /*
            This test case tests:
            1) The correctness of Client MCS Connect Initial PDU with GCC Conference Create Request 
               when the server advertised support for Extended Client Data Blocks.
            2) SUT can process the valid Server MCS Connect Response PDU with GCC Conference Create Response with MCSChannelId = 65534 in Server Message Channel Data correctly.

            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase. 
                Server should set the EXTENDED_CLIENT_DATA_SUPPORTED flag in RDP Negotiation Response. 
            2.	Test Suite expects SUT continue the connection sequence with sending a 
                Client MCS Connect Initial PDU with GCC Conference Create Request. 
            3.	Verify the received Client MCS Connect Initial PDU with GCC Conference Create Request 
                and respond Server MCS Connect Response PDU with GCC Conference Create Response.
                Server set the MCSChannelId = 65534 in Server Message Channel Data.
            4.	Test Suite expects a Client MCS Erect Domain Request PDU to indicate the successful process 
                of Server MCS Connect Response PDU with GCC Conference Create Response.
            */
            #endregion

            #region Test Sequence

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to initiate a RDP connection
            //Trigger client to initiate a RDP connection.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Expect the transport layer connection request
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}; Extended Client Data supported: true", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, true, true, NegativeType.None);

            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond A Server MCS Connect Response PDU with GCC Conference Create Response.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}; MCSChannelId: {3}", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString(), ConstValue.ID_LessMax);
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, TS_UD_SC_CORE_version_Values.V2, NegativeType.None, MULTITRANSPORT_TYPE_FLAGS.None, false, 0, ConstValue.IO_CHANNEL_ID, ConstValue.ID_LessMax);

            //Expect a Client MCS Erect Domain Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Erect Domain Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Erect_Domain_Request>(waitTime);

            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case verifies that SUT can process the boundary value (1) of MCSChannelId field in Server Message Channel Data.")]
        public void S1_ConnectionTest_BasicSettingExchange_PositiveTest_MCSChannelIdOfServerMessageChannelData_ONE()
        {
            #region Test Description
            /*
            This test case tests:
            1) The correctness of Client MCS Connect Initial PDU with GCC Conference Create Request 
               when the server advertised support for Extended Client Data Blocks.
            2) SUT can process the valid Server MCS Connect Response PDU with GCC Conference Create Response with MCSChannelId = 1 in Server Message Channel Data correctly.

            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase. 
                Server should set the EXTENDED_CLIENT_DATA_SUPPORTED flag in RDP Negotiation Response. 
            2.	Test Suite expects SUT continue the connection sequence with sending a 
                Client MCS Connect Initial PDU with GCC Conference Create Request. 
            3.	Verify the received Client MCS Connect Initial PDU with GCC Conference Create Request 
                and respond Server MCS Connect Response PDU with GCC Conference Create Response.
                Server set the MCSChannelId as 1 in Server Message Channel Data.
            4.	Test Suite expects a Client MCS Erect Domain Request PDU to indicate the successful process 
                of Server MCS Connect Response PDU with GCC Conference Create Response.
            */
            #endregion

            #region Test Sequence

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to initiate a RDP connection
            //Trigger client to initiate a RDP connection.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Expect the transport layer connection request
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}; Extended Client Data supported: true", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, true, true, NegativeType.None);

            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond A Server MCS Connect Response PDU with GCC Conference Create Response.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}; MCSChannelId: {3}", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString(), ConstValue.ID_ONE);
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, TS_UD_SC_CORE_version_Values.V2, NegativeType.None, MULTITRANSPORT_TYPE_FLAGS.None, false, 0, ConstValue.IO_CHANNEL_ID, ConstValue.ID_ONE);

            //Expect a Client MCS Erect Domain Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Erect Domain Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Erect_Domain_Request>(waitTime);

            #endregion
        }


        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case verifies that SUT can process the boundary value (0) of MCSChannelId field in Server Message Channel Data.")]
        public void S1_ConnectionTest_BasicSettingExchange_PositiveTest_MCSChannelIdOfServerMessageChannelData_ZERO()
        {
            #region Test Description
            /*
            This test case tests:
            1) The correctness of Client MCS Connect Initial PDU with GCC Conference Create Request 
               when the server advertised support for Extended Client Data Blocks.
            2) SUT can process the valid Server MCS Connect Response PDU with GCC Conference Create Response with MCSChannelId = 0 in Server Message Channel Data correctly.

            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase. 
                Server should set the EXTENDED_CLIENT_DATA_SUPPORTED flag in RDP Negotiation Response. 
            2.	Test Suite expects SUT continue the connection sequence with sending a 
                Client MCS Connect Initial PDU with GCC Conference Create Request. 
            3.	Verify the received Client MCS Connect Initial PDU with GCC Conference Create Request 
                and respond Server MCS Connect Response PDU with GCC Conference Create Response.
                Server set the MCSChannelId = 0 in Server Message Channel Data.
            4.	Test Suite expects a Client MCS Erect Domain Request PDU to indicate the successful process 
                of Server MCS Connect Response PDU with GCC Conference Create Response.
            */
            #endregion

            #region Test Sequence

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to initiate a RDP connection
            //Trigger client to initiate a RDP connection.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Expect the transport layer connection request
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}; Extended Client Data supported: true", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, true, true, NegativeType.None);

            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond A Server MCS Connect Response PDU with GCC Conference Create Response.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}; MCSChannelId: {3}", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString(), ConstValue.ID_ZERO);
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, TS_UD_SC_CORE_version_Values.V2, NegativeType.None, MULTITRANSPORT_TYPE_FLAGS.None, false, 0, ConstValue.IO_CHANNEL_ID, ConstValue.ID_ZERO);

            //Expect a Client MCS Erect Domain Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Erect Domain Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Erect_Domain_Request>(waitTime);

            #endregion
        }

    
    }
}
