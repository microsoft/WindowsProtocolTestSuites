// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;


namespace Microsoft.Protocols.TestSuites.Rdpbcgr
{
    public partial class RdpbcgrTestSuite
    {
        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]        
        [Description("This test case is used to ensure SUT drop the connection if the length field within tpktHeader in Server Demand Active PDU is not consistent with the received data.")]
        public void S1_ConnectionTest_CapabilityExchange_NegativeTest_DemandActive_InvalidTKPKLength()
        {
            #region Test Description
            //1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange Phase and Licensing phase.
            //2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT and set the length field within tpktHeader to an invalid value (less than the actual value).
            //3.	Test Suite expects SUT drop the connection.
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

            //Expect the transport layer connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            #region Connection Initiation phase
            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}; Extended Client Data supported: true", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, true, true, NegativeType.None);
            #endregion

            #region Basic Setting Exchange phase
            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond a Server MCS Connect Response PDU with GCC Conference Create Response.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}.", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString());
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, TS_UD_SC_CORE_version_Values.V2, NegativeType.None);
            #endregion

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

            //Expect SUT start a channel join sequence
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
            //Send a Server Demand Active PDU to SUT.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server Demand Active PDU to SUT.");
            this.rdpbcgrAdapter.Server_Demand_Active(NegativeType.InvalidTPKLength);

            RDPClientTryDropConnection("invalid TPKLength in Server Demand Active PDU");
            #endregion
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]        
        [Description("This test case is used to ensure SUT drop the connection if the length field within the MCS Header in Server Demand Active PDU is not consistent with the received data.")]
        public void S1_ConnectionTest_CapabilityExchange_NegativeTest_DemandActive_InvalidMCSLength()
        {
            #region Test Description
            //1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange Phase and Licensing phase.
            //2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT and set the length field within MCS Header to an invalid value (less than the actual value).
            //3.	Test Suite expects SUT drop the connection.
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

            //Expect the transport layer connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            #region Connection Initiation phase
            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}; Extended Client Data supported: true", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, true, true, NegativeType.None);
            #endregion

            #region Basic Setting Exchange phase
            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond a Server MCS Connect Response PDU with GCC Conference Create Response.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}.", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString());
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, TS_UD_SC_CORE_version_Values.V2, NegativeType.None);
            #endregion

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

            //Expect SUT start a channel join sequence
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
            //Send a Server Demand Active PDU to SUT.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server Demand Active PDU to SUT.");
            this.rdpbcgrAdapter.Server_Demand_Active(NegativeType.InvalidMCSLength);

            RDPClientTryDropConnection("invalid MCS length in Server Demand Active PDU");            
            #endregion
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]        
        [Description("This test case is used to ensure SUT drop the connection if the MAC signature field within securityHeader in Server Demand Active PDU is incorrect data.")]
        public void S1_ConnectionTest_CapabilityExchange_NegativeTest_DemandActive_IncorrectSignature()
        {
            #region Test Description
            //1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange Phase and Licensing phase.
            //2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT and set the MAC signature to an incorrect value.
            //3.	Test Suite expects SUT drop the connection.
            #endregion

            #region Test Sequence
            if (enLevel != EncryptionLevel.ENCRYPTION_LEVEL_CLIENT_COMPATIBLE && enLevel != EncryptionLevel.ENCRYPTION_LEVEL_HIGH)
            {
                this.TestSite.Log.Add(LogEntryKind.Debug, "This case requires the encryption level set to Client or High for RDP standard connection");
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

            //Expect the transport layer connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            #region Connection Initiation phase
            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}; Extended Client Data supported: true", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, true, true, NegativeType.None);
            #endregion

            #region Basic Setting Exchange phase
            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond a Server MCS Connect Response PDU with GCC Conference Create Response.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}.", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString());
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, TS_UD_SC_CORE_version_Values.V2, NegativeType.None);
            #endregion

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

            //Expect SUT start a channel join sequence
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
            //Send a Server Demand Active PDU to SUT.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server Demand Active PDU to SUT.");
            this.rdpbcgrAdapter.Server_Demand_Active(NegativeType.InvalidSignatureInSecurityHeader);

            RDPClientTryDropConnection("invalid MAC Signature in Server Demand Active pdu");            
            #endregion
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]        
        [Description("This test case is used to ensure SUT drop the connection if the total Length field within shareControlHeader in Server Demand Active PDU is invalid.")]
        public void S1_ConnectionTest_CapabilityExchange_NegativeTest_DemandActive_InvalidPDULength()
        {
            #region Test Description
            //1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange Phase and Licensing phase.
            //2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT and set the totalLength field within shareControlHeader to an invalue value (less than the actual value).
            //3.	Test Suite expects SUT drop the connection.
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

            //Expect the transport layer connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            #region Connection Initiation phase
            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}; Extended Client Data supported: true", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, true, true, NegativeType.None);
            #endregion

            #region Basic Setting Exchange phase
            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond a Server MCS Connect Response PDU with GCC Conference Create Response.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}.", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString());
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, TS_UD_SC_CORE_version_Values.V2, NegativeType.None);
            #endregion

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

            //Expect SUT start a channel join sequence
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
            //Send a Server Demand Active PDU to SUT.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server Demand Active PDU to SUT.");
            this.rdpbcgrAdapter.Server_Demand_Active(NegativeType.InvalidPduLength);
            #endregion

            #region Wait for Client to drop the connection
            RDPClientTryDropConnection("invalid pdu length in Server Demand Active pdu");            
            #endregion
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process TS_GENERAL_CAPABILITYSET in Server Demand Active PDU correctly when OS type is set to Windows RT. ")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_GeneralCapSet_OSTypeWindowsRT()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process TS_GENERAL_CAPABILITYSET in Server Demand Active PDU collectly when OS type is set to Windows RT. 
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, 
                osMajorType of TS_GENERAL_CAPABILITYSET is set to OSMAJORTYPE_WINDOWS (0x0001), osMinorType is set to OSMINORTYPE_WINDOWS RT (0x0009).
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, set osMajorType of TS_GENERAL_CAPABILITYSET to OSMAJORTYPE_WINDOWS, and set osMinorType to OSMINORTYPE_WINDOWS RT.");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSTYPE_GENERAL);

            TS_GENERAL_CAPABILITYSET generalCapset = RdpbcgrCapSet.CreateGeneralCapSet(true, true, true, true, true, true, true, osMajorType_Values.OSMAJORTYPE_WINDOWS, osMinorType_Values.OSMINORTYPE_WINDOWS_RT);
            capSetCollection.Add(generalCapset);

            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");            
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process TS_GENERAL_CAPABILITYSET in Server Demand Active PDU correctly when OS type is set to unspecified. ")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_GeneralCapSet_OSTypeUnspecified()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process TS_GENERAL_CAPABILITYSET in Server Demand Active PDU collectly when OS type is set to unspecified. 
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, 
                osMajorType of TS_GENERAL_CAPABILITYSET is set to OSMAJORTYPE_UNSPECIFIED (0x0000), osMinorType is set to OSMINORTYPE_UNSPECIFIED (0x0000).
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, set osMajorType of TS_GENERAL_CAPABILITYSET to OSMAJORTYPE_UNSPECIFIED, and set osMinorType to OSMINORTYPE_UNSPECIFIED.");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSTYPE_GENERAL);

            TS_GENERAL_CAPABILITYSET generalCapset = RdpbcgrCapSet.CreateGeneralCapSet(true, true, true, true, true, true, true, osMajorType_Values.OSMAJORTYPE_UNSPECIFIED, osMinorType_Values.OSMINORTYPE_UNSPECIFIED);
            capSetCollection.Add(generalCapset);

            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process TS_GENERAL_CAPABILITYSET in Server Demand Active PDU correctly when fast-path output is not supported. ")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_GeneralCapSet_FPOutputNotSupported()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process TS_GENERAL_CAPABILITYSET in Server Demand Active PDU collectly when fast-path output is not supported. 
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, not set FASTPATH_OUTPUT_SUPPORTED or LONG_CREDENTIALS_SUPPORTED in extraFlags of TS_GENERAL_CAPABILITYSET.
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, not set FASTPATH_OUTPUT_SUPPORTED or LONG_CREDENTIALS_SUPPORTED in extraFlags of TS_GENERAL_CAPABILITYSET.");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSTYPE_GENERAL);

            TS_GENERAL_CAPABILITYSET generalCapset = RdpbcgrCapSet.CreateGeneralCapSet(false, true, false, true, true, true, true, osMajorType_Values.OSMAJORTYPE_WINDOWS, osMinorType_Values.OSMINORTYPE_WINDOWS_NT);
            capSetCollection.Add(generalCapset);

            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process TS_GENERAL_CAPABILITYSET in Server Demand Active PDU correctly when the Refresh Rect PDU is not supported.")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_GeneralCapSet_RefreshRectNotSupported()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process TS_GENERAL_CAPABILITYSET in Server Demand Active PDU collectly when the Refresh Rect PDU is not supported. 
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, refreshRectSupport of TS_GENERAL_CAPABILITYSET is false (0x00).
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, refreshRectSupport of TS_GENERAL_CAPABILITYSET is false.");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSTYPE_GENERAL);

            TS_GENERAL_CAPABILITYSET generalCapset = RdpbcgrCapSet.CreateGeneralCapSet(true, true, true, true, true, false, true, osMajorType_Values.OSMAJORTYPE_WINDOWS, osMinorType_Values.OSMINORTYPE_WINDOWS_NT);
            capSetCollection.Add(generalCapset);            

            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process TS_GENERAL_CAPABILITYSET in Server Demand Active PDU correctly when Suppress output PDU is not supported.")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_GeneralCapSet_SuppressOutputNotSupported()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process TS_GENERAL_CAPABILITYSET in Server Demand Active PDU collectly when Suppress output PDU is not supported. 
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, suppressOutputSupport of TS_GENERAL_CAPABILITYSET is false (0x00).
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, suppressOutputSupport of TS_GENERAL_CAPABILITYSET is false.");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSTYPE_GENERAL);

            TS_GENERAL_CAPABILITYSET generalCapset = RdpbcgrCapSet.CreateGeneralCapSet(true, true, true, true, true, true, false, osMajorType_Values.OSMAJORTYPE_WINDOWS, osMinorType_Values.OSMINORTYPE_WINDOWS_NT);
            capSetCollection.Add(generalCapset);

            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process TS_BITMAP_CAPABILITYSET in Server Demand Active PDU correctly when preferredBitsPerPixel is set to 24. ")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_BitmapCapSet_24ColorDepth()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process TS_BITMAP_CAPABILITYSET in Server Demand Active PDU correctly when preferredBitsPerPixel is set to 24.
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, set preferredBitsPerPixel of TS_BITMAP_CAPABILITYSET to 24.
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, set preferredBitsPerPixel of TS_BITMAP_CAPABILITYSET to 24.");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSTYPE_BITMAP);

            TS_BITMAP_CAPABILITYSET bitmapCapset = RdpbcgrCapSet.CreateBitmapCapSet(DesktopWidthForCapTest, DesktopHeightForCapTest, desktopResizeFlag_Values.TRUE, true, true, true);
            bitmapCapset.preferredBitsPerPixel = 24;
            capSetCollection.Add(bitmapCapset);

            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process TS_BITMAP_CAPABILITYSET in Server Demand Active PDU correctly when desktopWidth is set to the maximum allowed value on the server (8192). ")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_BitmapCapSet_MaxDesktopWidth()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process TS_BITMAP_CAPABILITYSET in Server Demand Active PDU correctly when desktopWidth is set to the maximum allowed value on the server (8192). 
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, set desktopWidth of TS_BITMAP_CAPABILITYSET to the maximum allowed value on the server (8192).
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, set desktopWidth of TS_BITMAP_CAPABILITYSET to maximum allowed value on server (8192).");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSTYPE_BITMAP);

            TS_BITMAP_CAPABILITYSET bitmapCapset = RdpbcgrCapSet.CreateBitmapCapSet(MaxDesktopWidth, DesktopHeightForCapTest, desktopResizeFlag_Values.TRUE, true, true, true);
            capSetCollection.Add(bitmapCapset);
           
            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process TS_BITMAP_CAPABILITYSET in Server Demand Active PDU correctly when desktopHeight is set to the maximum allowed value on the server (8192).")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_BitmapCapSet_MaxDesktopHeight()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process TS_BITMAP_CAPABILITYSET in Server Demand Active PDU correctly when desktopHeight is set to the maximum allowed value on the server (8192).
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, set desktopHeight of TS_BITMAP_CAPABILITYSET to the maximum allowed value on the server (8192).
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, set desktopHeight of TS_BITMAP_CAPABILITYSET to maximum allowed value on server (8192).");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSTYPE_BITMAP);

            TS_BITMAP_CAPABILITYSET bitmapCapset = RdpbcgrCapSet.CreateBitmapCapSet(DesktopWidthForCapTest, MaxDesktopHeight, desktopResizeFlag_Values.TRUE, true, true, true);
            capSetCollection.Add(bitmapCapset);
            
            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process TS_BITMAP_CAPABILITYSET in Server Demand Active PDU correctly when desktopResizeFlag is set to False (0x0000). ")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_BitmapCapSet_DesktopResizeNotSupported()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process TS_BITMAP_CAPABILITYSET in Server Demand Active PDU correctly when desktopResizeFlag is set to False (0x0000). 
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, set desktopResizeFlag of TS_BITMAP_CAPABILITYSET to False (0x0000).
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, set desktopResizeFlag of TS_BITMAP_CAPABILITYSET to False (0x0000).");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSTYPE_BITMAP);

            TS_BITMAP_CAPABILITYSET bitmapCapset = RdpbcgrCapSet.CreateBitmapCapSet(DesktopWidthForCapTest, DesktopHeightForCapTest, desktopResizeFlag_Values.FALSE, true, true, true);
            capSetCollection.Add(bitmapCapset);
            
            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process TS_BITMAP_CAPABILITYSET in Server Demand Active PDU correctly when drawingFlags is set to 0 (All not supported). ")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_BitmapCapSet_DrawingFlagsNotSet()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process TS_BITMAP_CAPABILITYSET in Server Demand Active PDU correctly when drawingFlags is set to 0 (All not supported). 
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, set drawingFlags of TS_BITMAP_CAPABILITYSET to 0.
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, set drawingFlags of TS_BITMAP_CAPABILITYSET to 0.");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSTYPE_BITMAP);

            TS_BITMAP_CAPABILITYSET bitmapCapset = RdpbcgrCapSet.CreateBitmapCapSet(DesktopWidthForCapTest, DesktopHeightForCapTest, desktopResizeFlag_Values.TRUE, false, false, false);
            bitmapCapset.drawingFlags = drawingFlags_Values.None; 
            capSetCollection.Add(bitmapCapset);
            

            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process TS_ORDER_CAPABILITYSET in Server Demand Active PDU correctly when SOLIDPATTERNBRUSHONLY flag is set on orderFlags. ")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_OrderCapSet_SolidPatternBrushOnly()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process TS_ORDER_CAPABILITYSET in Server Demand Active PDU correctly when SOLIDPATTERNBRUSHONLY flag is set on orderFlags.
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, set SOLIDPATTERNBRUSHONLY flag on orderFlags of TS_ORDER_CAPABILITYSET.
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, set SOLIDPATTERNBRUSHONLY flag on orderFlags of TS_ORDER_CAPABILITYSET.");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSTYPE_ORDER);

            TS_ORDER_CAPABILITYSET orderCapset = RdpbcgrCapSet.CreateOrderCapSet(new byte[] { 0x01, 0x01, 0x01, 0x01, 0x01, 0x00, 0x00, 0x01, 
                         0x01, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 
                         0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x00, 
                         0x01, 0x01, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00}, true, true, true, true, true, true, true);
            capSetCollection.Add(orderCapset);
            
            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process TS_ORDER_CAPABILITYSET in Server Demand Active PDU correctly when ORDERFLAGS_EXTRA_FLAGS flag is not set on orderFlags. ")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_OrderCapSet_ExtraFlagNotValid()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process TS_ORDER_CAPABILITYSET in Server Demand Active PDU correctly when ORDERFLAGS_EXTRA_FLAGS flag is not set on orderFlags.
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, ORDERFLAGS_EXTRA_FLAGS flag on orderFlags of TS_ORDER_CAPABILITYSET is not set.
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, ORDERFLAGS_EXTRA_FLAGS flag on orderFlags of TS_ORDER_CAPABILITYSET is not set.");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSTYPE_ORDER);

            TS_ORDER_CAPABILITYSET orderCapset = RdpbcgrCapSet.CreateOrderCapSet(new byte[] { 0x01, 0x01, 0x01, 0x01, 0x01, 0x00, 0x00, 0x01, 
                         0x01, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 
                         0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x00, 
                         0x01, 0x01, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00}, true, true, true, false, false, true, true);
            capSetCollection.Add(orderCapset);

            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process TS_POINTER_CAPABILITYSET in Server Demand Active PDU correctly when colorPointerCacheSize is set to 0. ")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_PointerCapSet_ColorPointerCacheSize_0()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process TS_POINTER_CAPABILITYSET in Server Demand Active PDU correctly when colorPointerCacheSize is set to 0. 
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, set colorPointerCacheSize of TS_POINTER_CAPABILITYSET to 0.
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, set colorPointerCacheSize of TS_POINTER_CAPABILITYSET to 0.");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSTYPE_POINTER);

            TS_POINTER_CAPABILITYSET pointerCapset = RdpbcgrCapSet.CreatePointerCapSet(colorPointerFlag_Values.TRUE, 0, 25);
            capSetCollection.Add(pointerCapset);

            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process TS_POINTER_CAPABILITYSET in Server Demand Active PDU correctly when pointerCacheSize is set to 0. ")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_PointerCapSet_PointerCacheSize_0()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process TS_POINTER_CAPABILITYSET in Server Demand Active PDU correctly when pointerCacheSize is set to 0. 
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, set pointerCacheSize of TS_POINTER_CAPABILITYSET to 0.
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, set pointerCacheSize of TS_POINTER_CAPABILITYSET to 0.");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSTYPE_POINTER);

            TS_POINTER_CAPABILITYSET pointerCapset = RdpbcgrCapSet.CreatePointerCapSet(colorPointerFlag_Values.TRUE, 25, 0);
            capSetCollection.Add(pointerCapset);

            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process TS_INPUT_CAPABILITYSET in Server Demand Active PDU correctly when inputFlags only set INPUT_FLAG_SCANCODES flag. ")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_InputCapSet_OnlySupportScanCodes()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process TS_INPUT_CAPABILITYSET in Server Demand Active PDU correctly when inputFlags only set INPUT_FLAG_SCANCODES flag. 
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, only set INPUT_FLAG_SCANCODES flag on inputFlags of TS_INPUT_CAPABILITYSET.
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, only set INPUT_FLAG_SCANCODES flag on inputFlags of TS_INPUT_CAPABILITYSET.");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSTYPE_INPUT);

            TS_INPUT_CAPABILITYSET inputCapset = RdpbcgrCapSet.CreateInputCapSet(true, false, false, false, false);
            capSetCollection.Add(inputCapset);

            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process TS_VIRTUALCHANNEL_CAPABILITYSET in Server Demand Active PDU correctly when flags is set to VCCAPS_NO_COMPR flag. ")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_VirtualChannelCapSet_CompressionNotSupport()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process TS_VIRTUALCHANNEL_CAPABILITYSET in Server Demand Active PDU correctly when flags is set to VCCAPS_NO_COMPR flag.
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, flags of TS_VIRTUALCHANNEL_CAPABILITYSET is set to VCCAPS_NO_COMPR.
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, false, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, flags of TS_VIRTUALCHANNEL_CAPABILITYSET is set to VCCAPS_NO_COMPR.");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSTYPE_VIRTUALCHANNEL);

            TS_VIRTUALCHANNEL_CAPABILITYSET virtualChannelCapset = RdpbcgrCapSet.CreateVirtualChannelCapSet(false, true);
            capSetCollection.Add(virtualChannelCapset);

            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process TS_VIRTUALCHANNEL_CAPABILITYSET in Server Demand Active PDU correctly when VCChunkSize is not present. ")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_VirtualChannelCapSet_VCChunkSizeNotPresent()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process TS_VIRTUALCHANNEL_CAPABILITYSET in Server Demand Active PDU correctly when VCChunkSize is not present.
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, VCChunkSize is not present in TS_VIRTUALCHANNEL_CAPABILITYSET.
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, false);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, VCChunkSize is not present in TS_VIRTUALCHANNEL_CAPABILITYSET.");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSTYPE_VIRTUALCHANNEL);

            TS_VIRTUALCHANNEL_CAPABILITYSET virtualChannelCapset = RdpbcgrCapSet.CreateVirtualChannelCapSet(true, false);
            capSetCollection.Add(virtualChannelCapset);

            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process TS_VIRTUALCHANNEL_CAPABILITYSET in Server Demand Active PDU correctly when VCChunkSize is set to max value. ")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_VirtualChannelCapSet_MaxVCChunkSize()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process TS_VIRTUALCHANNEL_CAPABILITYSET in Server Demand Active PDU correctly when VCChunkSize is set to max value.
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, VCChunkSize of TS_VIRTUALCHANNEL_CAPABILITYSET is set to max value.
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, VCChunkSize of TS_VIRTUALCHANNEL_CAPABILITYSET is set to max value.");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSTYPE_VIRTUALCHANNEL);

            TS_VIRTUALCHANNEL_CAPABILITYSET virtualChannelCapset = RdpbcgrCapSet.CreateVirtualChannelCapSet(true, true);
            virtualChannelCapset.VCChunkSize = MaxVCChunkSize;
            capSetCollection.Add(virtualChannelCapset);

            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process Server Demand Active PDU correctly when TS_BITMAPCACHE_HOSTSUPPORT_CAPABILITYSET is not present. ")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_BitmapCacheHostSupportCapSet_NotPresent()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process Server Demand Active PDU correctly when TS_BITMAPCACHE_HOSTSUPPORT_CAPABILITYSET is not present.
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, TS_BITMAPCACHE_HOSTSUPPORT_CAPABILITYSET is not present.
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(false, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, TS_BITMAPCACHE_HOSTSUPPORT_CAPABILITYSET is not present.");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSTYPE_BITMAPCACHE_HOSTSUPPORT);
            
            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process Server Demand Active PDU correctly when TS_FONT_CAPABILITYSET is not present. ")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_FontCapSet_NotPresent()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process Server Demand Active PDU correctly when TS_FONT_CAPABILITYSET is not present.
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, TS_FONT_CAPABILITYSET is not present.
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, TS_FONT_CAPABILITYSET is not present.");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSTYPE_FONT);

            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process Server Demand Active PDU correctly when TS_MULTIFRAGMENTUPDATE_CAPABILITYSET is not present. ")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_MultifragmentUpdateCapSet_NotPresent()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process Server Demand Active PDU correctly when TS_MULTIFRAGMENTUPDATE_CAPABILITYSET is not present.
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, TS_MULTIFRAGMENTUPDATE_CAPABILITYSET is not present.
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, TS_MULTIFRAGMENTUPDATE_CAPABILITYSET is not present.");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSETTYPE_MULTIFRAGMENTUPDATE);

            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process Server Demand Active PDU correctly when MaxRequestSize of TS_MULTIFRAGMENTUPDATE_CAPABILITYSET is set to a large value. ")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_MultifragmentUpdateCapSet_MaxRequestSizeLargeValue()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process Server Demand Active PDU correctly when MaxRequestSize of TS_MULTIFRAGMENTUPDATE_CAPABILITYSET is set to a large value.
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, MaxRequestSize of TS_MULTIFRAGMENTUPDATE_CAPABILITYSET is set to a large value.
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, MaxRequestSize of TS_MULTIFRAGMENTUPDATE_CAPABILITYSET is set to a large value.");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSETTYPE_MULTIFRAGMENTUPDATE);

            TS_MULTIFRAGMENTUPDATE_CAPABILITYSET multifragmentUpdatecapset = RdpbcgrCapSet.CreateMultiFragmentUpdateCapSet(LargeMaxRequestSize);
            capSetCollection.Add(multifragmentUpdatecapset);

            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process Server Demand Active PDU correctly when TS_LARGE_POINTER_CAPABILITYSET is not present. ")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_LargePointerCapSet_NotPresent()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process Server Demand Active PDU correctly when TS_LARGE_POINTER_CAPABILITYSET is not present.
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, TS_LARGE_POINTER_CAPABILITYSET is not present.
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, TS_LARGE_POINTER_CAPABILITYSET is not present.");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSETTYPE_LARGE_POINTER);
            
            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process Server Demand Active PDU correctly when largePointerSupportFlags of TS_LARGE_POINTER_CAPABILITYSET is set to 0. ")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_LargePointerCapSet_LargePointerNotSupport()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process Server Demand Active PDU correctly when largePointerSupportFlags of TS_LARGE_POINTER_CAPABILITYSET is set to 0.
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, largePointerSupportFlags of TS_LARGE_POINTER_CAPABILITYSET is set to 0.
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, largePointerSupportFlags of TS_LARGE_POINTER_CAPABILITYSET is set to 0.");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSETTYPE_LARGE_POINTER);

            TS_LARGE_POINTER_CAPABILITYSET largePointerCap = RdpbcgrCapSet.CreateLargePointerCapSet(largePointerSupportFlags_Values.None);
            capSetCollection.Add(largePointerCap);

            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process Server Demand Active PDU correctly when TS_COMPDESK_CAPABILITYSET is not present. ")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_DesktopCompositionCapSet_NotPresent()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process Server Demand Active PDU correctly when TS_COMPDESK_CAPABILITYSET is not present.
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, TS_COMPDESK_CAPABILITYSET is not present.
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, TS_COMPDESK_CAPABILITYSET is not present.");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSETTYPE_COMPDESK);

            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process Server Demand Active PDU correctly when CompDeskSupportLevel of TS_COMPDESK_CAPABILITYSET is set to COMPDESK_NOT_SUPPORTED (0x0000). ")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_DesktopCompositionCapSet_CompositionNotSupport()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process Server Demand Active PDU correctly when CompDeskSupportLevel of TS_COMPDESK_CAPABILITYSET is set to COMPDESK_NOT_SUPPORTED (0x0000).
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, CompDeskSupportLevel of TS_COMPDESK_CAPABILITYSET is set to COMPDESK_NOT_SUPPORTED (0x0000).
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets,CompDeskSupportLevel of TS_COMPDESK_CAPABILITYSET is set to COMPDESK_NOT_SUPPORTED (0x0000)..");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSETTYPE_COMPDESK);

            TS_COMPDESK_CAPABILITYSET compDeskCapset = RdpbcgrCapSet.CreateDesktopCompositionCapSet(CompDeskSupportLevel_Values.COMPDESK_NOT_SUPPORTED);
            capSetCollection.Add(compDeskCapset);

            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process Server Demand Active PDU correctly when TS_SURFCMDS_CAPABILITYSET is not present. ")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_SurfaceCommandsCapSet_NotPresent()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process Server Demand Active PDU correctly when TS_SURFCMDS_CAPABILITYSET is not present.
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, TS_SURFCMDS_CAPABILITYSET is not present.
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, TS_SURFCMDS_CAPABILITYSET is not present.");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSETTYPE_SURFACE_COMMANDS);
                        
            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process Server Demand Active PDU correctly when cmdFlags of TS_SURFCMDS_CAPABILITYSET is set not support Stream Surface Bits Command. ")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_SurfaceCommandsCapSet_StreamSurfaceBitsNotSupported()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process Server Demand Active PDU correctly when cmdFlags of TS_SURFCMDS_CAPABILITYSET is set not support Stream Surface Bits Command.
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, cmdFlags of TS_SURFCMDS_CAPABILITYSET is set not support Stream Surface Bits Command.
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, cmdFlags of TS_SURFCMDS_CAPABILITYSET is set not support Stream Surface Bits Command.");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSETTYPE_SURFACE_COMMANDS);

            TS_SURFCMDS_CAPABILITYSET surfCMDsCap = RdpbcgrCapSet.CreateSurfaceCmdCapSet(CmdFlags_Values.SURFCMDS_FRAMEMARKER | CmdFlags_Values.SURFCMDS_SETSURFACEBITS);
            capSetCollection.Add(surfCMDsCap);

            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process Server Demand Active PDU correctly when TS_BITMAPCODECS_CAPABILITYSET is not present. ")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_BitmapCodecsCapSet_NotPresent()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process Server Demand Active PDU correctly when TS_BITMAPCODECS_CAPABILITYSET is not present.
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, TS_BITMAPCODECS_CAPABILITYSET is not present.
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, TS_BITMAPCODECS_CAPABILITYSET is not present.");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSETTYPE_BITMAP_CODECS);

            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process Server Demand Active PDU correctly when TS_BITMAPCODECS_CAPABILITYSET contains no codec (bitmapCodecCount of TS_BITMAPCODECS is 0). ")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_BitmapCodecsCapSet_NoBitmapCodec()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process Server Demand Active PDU correctly when TS_BITMAPCODECS_CAPABILITYSET contains no codec (bitmapCodecCount of TS_BITMAPCODECS is 0).
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, TS_BITMAPCODECS_CAPABILITYSET contains no codec (bitmapCodecCount of TS_BITMAPCODECS is 0).
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, TS_BITMAPCODECS_CAPABILITYSET contains no codec (bitmapCodecCount of TS_BITMAPCODECS is 0).");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSETTYPE_BITMAP_CODECS);

            TS_BITMAPCODECS_CAPABILITYSET codecCapset = RdpbcgrCapSet.CreateBitmapCodecsCapSet(false, false);
            codecCapset.supportedBitmapCodecs.bitmapCodecCount = 0;
            codecCapset.lengthCapability = 5;
            capSetCollection.Add(codecCapset);
            
            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process Server Demand Active PDU correctly when TS_BITMAPCODECS_CAPABILITYSET contains many bitmap codecs (some are unknown codecs). ")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_BitmapCodecsCapSet_BitmapCodecCount_LargeValue()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process Server Demand Active PDU correctly when TS_BITMAPCODECS_CAPABILITYSET contains many bitmap codecs (some are unknown codecs).
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, TS_BITMAPCODECS_CAPABILITYSET contains many bitmap codecs (some are unknown codecs).
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, TS_BITMAPCODECS_CAPABILITYSET contains many bitmap codecs (some are unknown codecs).");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSETTYPE_BITMAP_CODECS);

            TS_BITMAPCODECS_CAPABILITYSET codecCapset = RdpbcgrCapSet.CreateBitmapCodecsCapSet(true, true);
            codecCapset.supportedBitmapCodecs.bitmapCodecCount = LargeBitmapCodecCount;
            List<TS_BITMAPCODEC> codecList = new List<TS_BITMAPCODEC>();
            codecList.AddRange(codecCapset.supportedBitmapCodecs.bitmapCodecArray);
            while (codecList.Count < codecCapset.supportedBitmapCodecs.bitmapCodecCount)
            {
                TS_BITMAPCODEC codec = GenerateBitmapCodedcForTest();
                codecCapset.lengthCapability += (ushort)(19 + codec.codecPropertiesLength);
                codecList.Add(codec);
            }
            codecCapset.supportedBitmapCodecs.bitmapCodecArray = codecList.ToArray();
            capSetCollection.Add(codecCapset);

            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to ensure SUT can process Server Demand Active PDU correctly when TS_BITMAPCODECS_CAPABILITYSET contains the max number of bitmap codecs (some are unknown codecs). ")]
        public void S1_ConnectionTest_CapabilityExchange_PositiveTest_BitmapCodecsCapSet_BitmapCodecCount_MaxValue()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process Server Demand Active PDU correctly when TS_BITMAPCODECS_CAPABILITYSET contains the max number of bitmap codecs (some are unknown codecs).
            
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, 
                Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.
            2.	Test Suite continues the connection with sending a Server Demand Active PDU to SUT, TS_BITMAPCODECS_CAPABILITYSET contains the max number of bitmap codecs (some are unknown codecs).
            3.	Test Suite expects SUT respond a Client confirm Active PDU. When received, Test Suite verifies this PDU.
            4.	Test Suite expects SUT continue the connection with sending a Synchronize PDU or input PDU.
            */
            #endregion

            #region Test Sequence

            // Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            // Generate the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Generate Capability sets, TS_BITMAPCODECS_CAPABILITYSET contains the max number of bitmap codecs (some are unknown codecs).");
            Collection<ITsCapsSet> capSetCollection = this.rdpbcgrAdapter.GenerateServerCapSet();

            // Update TS_GENERAL_CAPABILITYSET
            this.DeleteCapset(capSetCollection, capabilitySetType_Values.CAPSETTYPE_BITMAP_CODECS);

            TS_BITMAPCODECS_CAPABILITYSET codecCapset = RdpbcgrCapSet.CreateBitmapCodecsCapSet(true, true);
            codecCapset.supportedBitmapCodecs.bitmapCodecCount = byte.MaxValue;
            List<TS_BITMAPCODEC> codecList = new List<TS_BITMAPCODEC>();
            codecList.AddRange(codecCapset.supportedBitmapCodecs.bitmapCodecArray);
            while (codecList.Count < codecCapset.supportedBitmapCodecs.bitmapCodecCount)
            {
                TS_BITMAPCODEC codec = GenerateBitmapCodedcForTest();
                codecCapset.lengthCapability += (ushort)(19 + codec.codecPropertiesLength);
                codecList.Add(codec);
            }
            codecCapset.supportedBitmapCodecs.bitmapCodecArray = codecList.ToArray();
            capSetCollection.Add(codecCapset);

            // Establish RDP connection to test the capability sets
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish RDP connection to test capability sets.");
            TestCapabilitySets(capSetCollection);
            #endregion
        }

        #region Private Methods

        /// <summary>
        /// Test a specified capability sets.
        /// </summary>
        /// <param name="capSetCollection">Capability sets</param>
        private void TestCapabilitySets(Collection<ITsCapsSet> capSetCollection)
        {
            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to initiate a RDP connection
            //Trigger client to initiate a RDP connection.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Expect the transport layer connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            #region Connection Initiation phase
            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}; Extended Client Data supported: true", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, true, true, NegativeType.None);
            #endregion

            #region Basic Setting Exchange phase
            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond a Server MCS Connect Response PDU with GCC Conference Create Response.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}.", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString());
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, TS_UD_SC_CORE_version_Values.V2, NegativeType.None);
            #endregion

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

            //Expect SUT start a channel join sequence
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
            //Send a Server Demand Active PDU to SUT.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server Demand Active PDU to SUT.");
            this.rdpbcgrAdapter.Server_Demand_Active(capSetCollection);

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

        }

        /// <summary>
        /// Generate a random bitmap codec structure used for test
        /// </summary>
        /// <returns></returns>
        private TS_BITMAPCODEC GenerateBitmapCodedcForTest()
        {
            TS_BITMAPCODEC bitmapCodec = new TS_BITMAPCODEC();
            byte[] guidData = new byte[16];
            random.NextBytes(guidData);
            bitmapCodec.codecGUID.codecGUID1 = BitConverter.ToUInt32(guidData, 0);
            bitmapCodec.codecGUID.codecGUID2 = BitConverter.ToUInt16(guidData, 4);
            bitmapCodec.codecGUID.codecGUID3 = BitConverter.ToUInt16(guidData, 6);
            bitmapCodec.codecGUID.codecGUID4 = guidData[8];
            bitmapCodec.codecGUID.codecGUID5 = guidData[9];
            bitmapCodec.codecGUID.codecGUID6 = guidData[10];
            bitmapCodec.codecGUID.codecGUID7 = guidData[11];
            bitmapCodec.codecGUID.codecGUID8 = guidData[12];
            bitmapCodec.codecGUID.codecGUID9 = guidData[13];
            bitmapCodec.codecGUID.codecGUID10 = 0xFF; // Specify this field to a constant value, prevent the random GUID coincidentally same as a real codec GUID.
            bitmapCodec.codecGUID.codecGUID11 = guidData[15];
            bitmapCodec.codecPropertiesLength = (ushort)random.Next(2, 10);
            if (bitmapCodec.codecPropertiesLength != 0)
            {
                bitmapCodec.codecProperties = new byte[bitmapCodec.codecPropertiesLength];
                random.NextBytes(bitmapCodec.codecProperties);
            }
            return bitmapCodec;
        }

        #endregion Private Methods
    }
}
