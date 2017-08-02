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
        [Description(@"This test case is used to ensure SUT can complete the Connection Finalization phase successfully 
                       when the server doesn't advertised support for the Bitmap Host Cache Support Capability Set.")]
        public void S1_ConnectionTest_ConnectionFinalization_PositiveTest_BitmapHostCacheNotSupported()
        {
            #region Test Description
            /*
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, 
                Basic Setting Exchange phase, Channel Connection phase, RDP Security Commencement phase, 
                Secure Setting Exchange phase , Licensing phase and Capabilities Exchange phase.
            2.	SUT continue the connection with sending the following PDUs sequentially: Client Synchronize PDU
                >	Client Synchronize PDU
                >	Client Control PDU - Cooperate
                >	Client Control PDU - Request Control
                >	Client Font List PDU
            3.	Test Suite verifies the received PDUs one by one and responds the following PDUs sequentially:
                >	Server Synchronize PDU
                >	Server Control PDU - Cooperate
                >	Server Control PDU - Granted Control
                >	Server Font Map PDU
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
            //Set server capability and set supportServerBitmapCacheHost to false
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(false, true, true, true, true, true, true, true, true, true);
            //Send a Server Demand Active PDU to SUT and advertise the support for the Bitmap Host Cache Support Capability Set.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server Demand Active PDU to SUT and does not advertise the support for the Bitmap Host Cache Support Capability Set.");
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

            ////Expect a Client Persistent Key List PDU.
            //this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Persistent Key List PDU.");
            //this.rdpbcgrAdapter.WaitForPacket<Client_Persistent_Key_List_Pdu>(waitTime);

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
        [Description(@"This test case is used to ensure SUT can complete the Connection Finalization phase successfully with a valid sending and receiving PDU sequences.
                        The concrete sequences is: Server Synchronize PDU -> Client Synchronize PDU -> Client Control PDU ?Cooperate 
                        -> Server Control PDU ?Cooperate -> Client Control PDU - Request Control -> Client Persistent Key List PDU 
                        -> Server Control PDU ?Granted Control -> Client Font List PDU -> Server Font Map PDU.")]
        public void S1_ConnectionTest_ConnectionFinalization_PositiveTest_BitmapHostCacheSupported_sequenceTest_1()
        {
            #region Test Description
            /*
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, 
                Basic Setting Exchange phase, Channel Connection phase, RDP Security Commencement phase, 
                Secure Setting Exchange phase , Licensing phase and Capabilities Exchange phase.
            2.	SUT continue the connection with sending the following PDUs sequentially:
                    -> Server Synchronize PDU 
                    -> Client Synchronize PDU 
                    -> Client Control PDU - Cooperate 
                    -> Server Control PDU - Cooperate
                    -> Client Control PDU - Request Control
                    -> Client Persistent Key List PDU
                    -> Server Control PDU - Granted Control
                    -> Client Font List PDU
                    -> Server Font Map PDU 
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
            //Send a Server Synchronize PDU to SUT.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server Synchronize PDU to SUT.");
            this.rdpbcgrAdapter.ServerSynchronize();
            
            //Expect SUT send a Client Synchronize PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Synchronize PDU.");
            this.rdpbcgrAdapter.WaitForPacket<Client_Synchronize_Pdu>(waitTime);

            //Expect SUT send a Client Control PDU - Cooperate.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Control PDU - Cooperate.");
            this.rdpbcgrAdapter.WaitForPacket<Client_Control_Pdu_Cooperate>(waitTime);

            //Send a Server Control Cooperate PDU to SUT.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server Control Cooperate PDU to SUT.");
            this.rdpbcgrAdapter.ServerControlCooperate();

            //Expect SUT send a Client Control PDU - Request Control.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Control PDU - Request Control.");
            this.rdpbcgrAdapter.WaitForPacket<Client_Control_Pdu_Request_Control>(waitTime);

            if (this.isclientSupportPersistentBitmapCache)
            {
                //Expect a Client Persistent Key List PDU.
                this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Persistent Key List PDU.");
                this.rdpbcgrAdapter.WaitForPacket<Client_Persistent_Key_List_Pdu>(waitTime);
            }

            //Respond a Server Control Granted Control PDU to SUT.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server Control Granted Control PDU to SUT.");
            this.rdpbcgrAdapter.ServerControlGrantedControl();

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
        [Description(@"This test case is used to ensure SUT can complete the Connection Finalization phase successfully with a valid sending and receiving PDU sequences. 
                        The concrete sequences is: Client Synchronize PDU -> Client Control PDU ?Cooperate -> Server Synchronize PDU 
                        -> Client Control PDU - Request Control -> Server Control PDU ?Cooperate -> Client Persistent Key List PDU 
                        -> Client Font List PDU -> Server Control PDU ?Granted Control -> Server Font Map PDU.")]
        public void S1_ConnectionTest_ConnectionFinalization_PositiveTest_BitmapHostCacheSupported_sequenceTest_2()
        {
            #region Test Description
            /* 
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, 
                Basic Setting Exchange phase, Channel Connection phase, RDP Security Commencement phase, 
                Secure Setting Exchange phase , Licensing phase and Capabilities Exchange phase.
            2.	SUT continue the connection with sending the following PDUs sequentially:  
                    -> Client Synchronize PDU 
                    -> Client Control PDU - Cooperate
                    -> Server Synchronize PDU 
                    -> Client Control PDU - Request Control
                    -> Server Control PDU - Cooperate
                    -> Client Persistent Key List PDU
                    -> Client Font List PDU
                    -> Server Control PDU - Granted Control
                    -> Server Font Map PDU 
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

            //Expect SUT send a Client Control PDU - Cooperate.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Control PDU - Cooperate.");
            this.rdpbcgrAdapter.WaitForPacket<Client_Control_Pdu_Cooperate>(waitTime);

            //Send a Server Synchronize PDU to SUT.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server Synchronize PDU to SUT.");
            this.rdpbcgrAdapter.ServerSynchronize();

            //Expect SUT send a Client Control PDU - Request Control.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Control PDU - Request Control.");
            this.rdpbcgrAdapter.WaitForPacket<Client_Control_Pdu_Request_Control>(waitTime);

            //Send a Server Control Cooperate PDU to SUT.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server Control Cooperate PDU to SUT.");
            this.rdpbcgrAdapter.ServerControlCooperate();

            if (this.isclientSupportPersistentBitmapCache)
            {
                //Expect a Client Persistent Key List PDU.
                this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Persistent Key List PDU.");
                this.rdpbcgrAdapter.WaitForPacket<Client_Persistent_Key_List_Pdu>(waitTime);
            }

            //Expect a Client Font List PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Client Font List PDU.");
            this.rdpbcgrAdapter.WaitForPacket<Client_Font_List_Pdu>(waitTime);

            //Respond a Server Control Granted Control PDU to SUT.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server Control Granted Control PDU to SUT.");
            this.rdpbcgrAdapter.ServerControlGrantedControl();

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
        [Description(@"This test case is used to ensure SUT can complete the Connection Finalization phase successfully with a valid sending and receiving PDU sequences.  
                        The concrete sequences is: Server Synchronize PDU ->  Server Control PDU ?Cooperate -> Client Synchronize PDU 
                        -> Client Control PDU ?Cooperate -> Client Control PDU - Request Control -> Client Persistent Key List PDU 
                        -> Server Control PDU ?Granted Control -> Client Font List PDU -> Server Font Map PDU.")]
        public void S1_ConnectionTest_ConnectionFinalization_PositiveTest_BitmapHostCacheSupported_sequenceTest_3()
        {
            #region Test Description
            /*           
            Test Execution Steps:
            1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, 
                Basic Setting Exchange phase, Channel Connection phase, RDP Security Commencement phase, 
                Secure Setting Exchange phase , Licensing phase and Capabilities Exchange phase.
            2.	SUT continue the connection with sending the following PDUs sequentially:  
                    -> Server Synchronize PDU 
                    -> Server Control PDU - Cooperate
                    -> Client Synchronize PDU 
                    -> Client Control PDU - Cooperate
                    -> Client Control PDU - Request Control
                    -> Client Persistent Key List PDU
                    -> Server Control PDU - Granted Control
                    -> Client Font List PDU
                    -> Server Font Map PDU 
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
            //Send a Server Synchronize PDU to SUT.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server Synchronize PDU to SUT.");
            this.rdpbcgrAdapter.ServerSynchronize();

            //Send a Server Control Cooperate PDU to SUT.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server Control Cooperate PDU to SUT.");
            this.rdpbcgrAdapter.ServerControlCooperate();

            //Expect SUT send a Client Synchronize PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Synchronize PDU.");
            this.rdpbcgrAdapter.WaitForPacket<Client_Synchronize_Pdu>(waitTime);

            //Expect SUT send a Client Control PDU - Cooperate.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Control PDU - Cooperate.");
            this.rdpbcgrAdapter.WaitForPacket<Client_Control_Pdu_Cooperate>(waitTime);

            //Expect SUT send a Client Control PDU - Request Control.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Control PDU - Request Control.");
            this.rdpbcgrAdapter.WaitForPacket<Client_Control_Pdu_Request_Control>(waitTime);

            if (this.isclientSupportPersistentBitmapCache)
            {
                //Expect a Client Persistent Key List PDU.
                this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Persistent Key List PDU.");
                this.rdpbcgrAdapter.WaitForPacket<Client_Persistent_Key_List_Pdu>(waitTime);
            }

            //Respond a Server Control Granted Control PDU to SUT.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server Control Granted Control PDU to SUT.");
            this.rdpbcgrAdapter.ServerControlGrantedControl();

            //Expect a Client Font List PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Client Font List PDU.");
            this.rdpbcgrAdapter.WaitForPacket<Client_Font_List_Pdu>(waitTime);

            //Send a Server Font Map PDU to SUT.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server Font Map PDU to SUT.");
            this.rdpbcgrAdapter.ServerFontMap();
            #endregion

            #endregion
        }
        
    }
}
