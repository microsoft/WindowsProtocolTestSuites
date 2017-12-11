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
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case is used to verify 
                      SUT drops the connection if the length field within tpktHeader in License Error PDU is not consistent with the received data.")]
        public void S1_ConnectionTest_SecurityExchange_NegativeTest_LicenseError_InvalidTKPKLength()
        {
            #region Test Description
            //1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, Channel Connection phase.
            //2.	If Standard RDP Security mechanisms are being employed, Test Suite expects SUT continue the connection by sending a Client Security Exchange PDU.
            //3.	Test Suite expects SUT continues the connection by sending a Client Info PDU.
            //4.	Test Suite responds a Server License Error PDU – Valid Client with setting the length of tpktHeader to an invalid value (less than the actual value).
            //5.	Test Suite expects SUT drop the connection.
            #endregion

            #region Test Implementation
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

            //Respond a Server MCS Attach User Confirm PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Attach User Confirm PDU to SUT with invalid TPK header length");
            this.rdpbcgrAdapter.MCSAttachUserConfirm(NegativeType.None);

            //Expect SUT start a channel join sequence
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start the channel join sequence.");
            this.rdpbcgrAdapter.ChannelJoinRequestAndConfirm(NegativeType.None);
            #endregion

            //Expects SUT continue the connection by sending a Client Security Exchange PDU 
            //(if Standard RDP Security mechanisms are being employed) or a Client Info PDU.
            if (transportProtocol == EncryptedProtocol.Rdp)
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Security Exchange PDU.");
                this.rdpbcgrAdapter.ExpectPacket<Client_Security_Exchange_Pdu>(waitTime);
            }

            //Expect a Client Info PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Info PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_Info_Pdu>(waitTime);

            //Send SUT a Server License Error Pdu - Valid Client.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server License Error Pdu - Valid Client with setting the length of tpktHeader to an invalid value (less than the actual value).");
            this.rdpbcgrAdapter.Server_License_Error_Pdu_Valid_Client(NegativeType.InvalidTPKLength);

            RDPClientTryDropConnection("invalid TKPK length in License Error Pdu");
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case is used to verify 
                      SUT drops the connection if the length field within MCS Header in License Error PDU is not consistent with the received data.")]
        public void S1_ConnectionTest_SecurityExchange_NegativeTest_LicenseError_InvalidMCSLength()
        {
            #region Test Description
            //1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, Channel Connection phase.
            //2.	If Standard RDP Security mechanisms are being employed, Test Suite expects SUT continue the connection by sending a Client Security Exchange PDU.
            //3.	Test Suite expects SUT continues the connection by sending a Client Info PDU.
            //4.	Test Suite responds a Server License Error PDU – Valid Client with setting the length within MCS Header to an invalid value (less than the actual value).
            //5.	Test Suite expects SUT drop the connection.
            #endregion

            #region Test Implementation
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

            //Respond a Server MCS Attach User Confirm PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Attach User Confirm PDU to SUT");
            this.rdpbcgrAdapter.MCSAttachUserConfirm(NegativeType.None);

            //Expect SUT start a channel join sequence
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start the channel join sequence.");
            this.rdpbcgrAdapter.ChannelJoinRequestAndConfirm(NegativeType.None);
            #endregion

            //Expects SUT continue the connection by sending a Client Security Exchange PDU 
            //(if Standard RDP Security mechanisms are being employed) or a Client Info PDU.
            if (transportProtocol == EncryptedProtocol.Rdp)
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Security Exchange PDU.");
                this.rdpbcgrAdapter.ExpectPacket<Client_Security_Exchange_Pdu>(waitTime);
            }

            //Expect a Client Info PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Info PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_Info_Pdu>(waitTime);

            //Send SUT a Server License Error Pdu - Valid Client.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server License Error Pdu - Valid Client with setting the length within MCS Header to an invalid value (less than the actual value).");
            this.rdpbcgrAdapter.Server_License_Error_Pdu_Valid_Client(NegativeType.InvalidMCSLength);

            RDPClientTryDropConnection("invalid MCS length in License Error Pdu");
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case is used to verify 
                      SUT drops the connection if the SEC_LICENSE_PKT (0x0080) flag within securityHeader in License Error PDU is not present.")]
        public void S1_ConnectionTest_SecurityExchange_NegativeTest_LicenseError_InvalidFlag()
        {
            #region Test Description
            //1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, Channel Connection phase.
            //2.	If Standard RDP Security mechanisms are being employed, Test Suite expects SUT continue the connection by sending a Client Security Exchange PDU.
            //3.	Test Suite expects SUT continues the connection by sending a Client Info PDU.
            //4.	Test Suite responds a Server License Error PDU – Valid Client with not presenting the SEC_LICENSE_PKT (0x0080) flag within securityHeader.
            //5.	Test Suite expects SUT drop the connection.
            #endregion

            #region Test Implementation
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

            //Respond a Server MCS Attach User Confirm PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Attach User Confirm PDU to SUT");
            this.rdpbcgrAdapter.MCSAttachUserConfirm(NegativeType.None);

            //Expect SUT start a channel join sequence
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start the channel join sequence.");
            this.rdpbcgrAdapter.ChannelJoinRequestAndConfirm(NegativeType.None);
            #endregion

            //Expects SUT continue the connection by sending a Client Security Exchange PDU 
            //(if Standard RDP Security mechanisms are being employed) or a Client Info PDU.
            if (transportProtocol == EncryptedProtocol.Rdp)
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Security Exchange PDU.");
                this.rdpbcgrAdapter.ExpectPacket<Client_Security_Exchange_Pdu>(waitTime);
            }

            //Expect a Client Info PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Info PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_Info_Pdu>(waitTime);

            //Send SUT a Server License Error Pdu - Valid Client.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server License Error Pdu - Valid Client with not presenting the SEC_LICENSE_PKT (0x0080) flag within securityHeader.");
            this.rdpbcgrAdapter.Server_License_Error_Pdu_Valid_Client(NegativeType.InvalidFlagInSecurityHeader);

            RDPClientTryDropConnection("invalid Server License Error Pdu without SEC_LICENSE_PKT");
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case is used to verify 
                      SUT drops the connection if MAC signature within securityHeader in License Error PDU is not valid and SEC_ENCRYPT (0x0008) flag is present.")]
        public void S1_ConnectionTest_SecurityExchange_NegativeTest_LicenseError_InvalidSignature()
        {
            #region Test Description
            //1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, Channel Connection phase.
            //2.	If Standard RDP Security mechanisms are being employed, Test Suite expects SUT continue the connection by sending a Client Security Exchange PDU.
            //3.	Test Suite expects SUT continues the connection by sending a Client Info PDU.
            //4.	Test Suite responds a Server License Error PDU – Valid Client, the SEC_ENCRYPT flag is present and the signature within securityHeader is invalid.
            //5.	Test Suite expects SUT drop the connection.
            #endregion

            #region Test Implementation
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

            //Respond a Server MCS Attach User Confirm PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Attach User Confirm PDU to SUT");
            this.rdpbcgrAdapter.MCSAttachUserConfirm(NegativeType.None);

            //Expect SUT start a channel join sequence
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start the channel join sequence.");
            this.rdpbcgrAdapter.ChannelJoinRequestAndConfirm(NegativeType.None);
            #endregion

            //Expects SUT continue the connection by sending a Client Security Exchange PDU 
            //(if Standard RDP Security mechanisms are being employed) or a Client Info PDU.
            if (transportProtocol == EncryptedProtocol.Rdp)
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Security Exchange PDU.");
                this.rdpbcgrAdapter.ExpectPacket<Client_Security_Exchange_Pdu>(waitTime);
            }

            //Expect a Client Info PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Info PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_Info_Pdu>(waitTime);

            //Send SUT a Server License Error Pdu - Valid Client.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server License Error Pdu - Valid Client, the SEC_ENCRYPT flag is present and the signature within securityHeader is invalid.");
            this.rdpbcgrAdapter.Server_License_Error_Pdu_Valid_Client(NegativeType.InvalidSignatureInSecurityHeader);

            RDPClientTryDropConnection("Server License Error Pdu with invalid MAC signature");
            #endregion
        }

    }
}
