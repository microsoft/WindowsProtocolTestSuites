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
        [TestCategory("BVT")]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [TestCategory("StaticVirtualChannel")]
        [Description("This test case is used to verify the uncompressed Static Virtual Channel PDU.")]
        public void BVT_StaticVirtualChannel_PositiveTest_CompressionNotSupported()
        {
            #region Test Description
            /*
            This test case is used to verify the uncompressed Static Virtual Channel PDU. 
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection. In Capability Exchange phase, Test Suite sets the flags field to VCCAPS_NO_COMPR (0x00000000) flag in Virtual Channel Capability Set.
            2.	After the connection sequence has been finished, Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on (how to determine the notification type is described in TD section 3.3.5.10) . 
            3.	Trigger SUT to send some Static Virtual Channel PDUs.
            4.	Test Suite verifies the received Virtual Channel PDUs and expects these PDUs are not compressed.
            */
            #endregion

            #region Test Sequence
            this.TestSite.Assert.IsTrue(isClientSupportRDPEFS, "To execute test cases of S7, RDP client should support [MS-RDPEFS]: Remote Desktop Protocol: File System Virtual Channel Extension.");
            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, false, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            //Check whether 'rdpdr' channel has been created
            if (this.rdpbcgrAdapter.GetStaticVirtualChannelId("RDPDR") == 0)
            {
                this.TestSite.Assume.Fail("The necessary channel RDPDR has not been created, so stop running this test case.");
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Generating Static Virtual Channel traffics.");
            SendStaticVirtualChannelTraffics(StaticVirtualChannel_InvalidType.None);
            #endregion
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [TestCategory("StaticVirtualChannel")]
        [Description("This test case is used to verify the Static Virtual Channel PDUs when VCChunkSizeField is not present in server-to-client Virtual Channel Capability Set.")]
        public void BVT_StaticVirtualChannel_PositiveTest_VCChunkSizeNotPresent()
        {
            #region Test Description
            /*
            This test case is used to verify the Static Virtual Channel PDUs when VCChunkSizeField is not present in server-to-client Virtual Channel Capability Set.
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection. In Capability Exchange phase, Test Suite sets the flags field to VCCAPS_NO_COMPR (0x00000000) flag 
                and not present the VCChunkSize field in Virtual Channel Capability Set.
            2.	After the connection sequence has been finished, Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), 
                INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on (how to determine the notification type is described in TD section 3.3.5.10) . 
            3.	Trigger SUT to send some Static Virtual Channel PDUs.
            4.	Test Suite verifies the received Virtual Channel PDUs and expects the size of PDUs do not exceed CHANNEL_CHUNK_LENGTH (1600).
            */
            #endregion

            #region Test Sequence
            this.TestSite.Assert.IsTrue(isClientSupportRDPEFS, "To execute test cases of S7, RDP client should support [MS-RDPEFS]: Remote Desktop Protocol: File System Virtual Channel Extension.");
            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported. VCChunkSize is not present.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, false, false);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            //Check whether 'rdpdr' channel has been created
            if (this.rdpbcgrAdapter.GetStaticVirtualChannelId("RDPDR") == 0)
            {
                this.TestSite.Assume.Fail("The necessary channel RDPDR has not been created, so stop running this test case.");
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Generating Static Virtual Channel traffics.");
            SendStaticVirtualChannelTraffics(StaticVirtualChannel_InvalidType.None);
            #endregion
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [TestCategory("StaticVirtualChannel")]
        [Description("This test case is used to verify the compressed Static Virtual Channel PDU.")]
        public void S7_StaticVirtualChannel_PositiveTest_CompressionSupported()
        {
            #region Test Description
            /*
            1.	Trigger SUT to initiate and complete a RDP connection. In Capability Exchange phase, Test Suite sets the flags field to VCCAPS_COMPR_CS_8K (0x00000002) flag in Virtual Channel Capability Set.
            2.	After the connection sequence has been finished, Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), 
                or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on (how to determine the notification type is described in TD section 3.3.5.10) . 
            3.	Trigger SUT to send some Static Virtual Channel PDUS.
            4.	Test Suite verifies the received Virtual Channel PDUs and expects these PDU are compressed.
            */
            #endregion

            #region Test Sequence
            this.TestSite.Assert.IsTrue(isClientSupportRDPEFS, "To execute test cases of S7, RDP client should support [MS-RDPEFS]: Remote Desktop Protocol: File System Virtual Channel Extension.");
            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            //Check whether 'rdpdr' channel has been created
            if (this.rdpbcgrAdapter.GetStaticVirtualChannelId("RDPDR") == 0)
            {
                this.TestSite.Assume.Fail("The necessary channel RDPDR has not been created, so stop running this test case.");
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Generating Static Virtual Channel traffics.");
            SendStaticVirtualChannelTraffics(StaticVirtualChannel_InvalidType.None);
            #endregion
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [TestCategory("StaticVirtualChannel")]
        [Description("This test case is used to verify the Static Virtual Channel PDUs when VCChunkSizeField is present in server-to-client Virtual Channel Capability Set.")]
        public void S7_StaticVirtualChannel_PositiveTest_VCChunkSizePresent()
        {
            #region Test Description
            /*
            1.	Trigger SUT to initiate and complete a RDP connection. In Capability Exchange phase, Test Suite sets the flags field to VCCAPS_NO_COMPR (0x00000000) flag and presents the VCChunkSize field in Virtual Channel Capability Set.
            2.	After the connection sequence has been finished, Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on (how to determine the notification type is described in TD section 3.3.5.10) . 
            3.	Trigger SUT to send some Static Virtual Channel PDUs.
            4.	Test Suite verifies the received Virtual Channel PDUs and expects the size of PDUs do not exceed the one set in VCChunkSize.
            */
            #endregion

            #region Test Sequence
            this.TestSite.Assert.IsTrue(isClientSupportRDPEFS, "To execute test cases of S7, RDP client should support [MS-RDPEFS]: Remote Desktop Protocol: File System Virtual Channel Extension.");
            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported. VCChunkSize is present.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, false, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            //Check whether 'rdpdr' channel has been created
            if (this.rdpbcgrAdapter.GetStaticVirtualChannelId("RDPDR") == 0)
            {
                this.TestSite.Assume.Fail("The necessary channel RDPDR has not been created, so stop running this test case.");
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Generating Static Virtual Channel traffics.");
            SendStaticVirtualChannelTraffics(StaticVirtualChannel_InvalidType.None);
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [TestCategory("StaticVirtualChannel")]
        [Description("This test case is used to ensure SUT drops the connection when the length field of tpktHeader is invalid.")]
        public void S7_StaticVirtualChannel_NegativeTest_InvalidTPKTLength()
        {
            #region Test Description
            /*
            1.  Trigger SUT to initiate and complete a RDP connection. 
            2.   After the connection sequence has been finished, Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), 
                 or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on (how to determine the notification type is described in TD section 3.3.5.10) . 
            3.   Test Suite sends SUT a Static Virtual Channel PDU and set the length field of tpktHeader to an invalid value (less than the actual value).
            4.   Test Suite expects SUT drops the connection.
            */
            #endregion

            #region Test Sequence
            this.TestSite.Assert.IsTrue(isClientSupportRDPEFS, "To execute test cases of S7, RDP client should support [MS-RDPEFS]: Remote Desktop Protocol: File System Virtual Channel Extension.");
            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, false, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            //Check whether 'rdpdr' channel has been created
            if (this.rdpbcgrAdapter.GetStaticVirtualChannelId("RDPDR") == 0)
            {
                this.TestSite.Assume.Fail("The necessary channel RDPDR has not been created, so stop running this test case.");
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending SUT a Static Virtual Channel PDU and filling the length field of tpktHeader to an invalid value (less than the actual value)");
            SendStaticVirtualChannelTraffics(StaticVirtualChannel_InvalidType.InvalidTPKTLength);

            RDPClientTryDropConnection("an invalid length field of tpktHeader in Slow-Path static Virtual Channel PDU");
            #endregion
        }
        
        /// <summary>
        /// SDK not support to modify the length of mcsData.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [TestCategory("StaticVirtualChannel")]
        [Description("This test case is used to ensure SUT drops the connection when the length field of mcsPdu is invalid.")]
        public void S7_StaticVirtualChannel_NegativeTest_InvalidMCSLength()
        {
            #region Test Description
            /*
            1.  Trigger SUT to initiate and complete a RDP connection. 
            2.  After the connection expectConnectionDisconnectionSequence has been finished, TestSuite sends a Server_Save_Session_Info_Pdu Session infoType_Values PDU Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request an ErrorNotificationType_Values type offscreenSupportLevel_Values the INFOTYPE_LOGON (0x00000000), 
                INFOTYPE_LOGON_LONG (0x00000001), orderFlags_Values INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the Client_MCS_Attach_User_Request has DR_CORE_USER_LOGGEDON on (how to determine the notification type is described in CAPABILITY_HEADER section 3.3.5.10) . 
            3.  TestSuite sends SUT a StaticVirtualChannel_InvalidType Virtual Channel_Options PDU and set the length Field of mcsPdu to and invalid ValueType (less than the actual ValueType).
            4.  TestSuite expects SUT drops the connection.
            */
            #endregion

            #region Test Sequence
            this.TestSite.Assert.IsTrue(isClientSupportRDPEFS, "To execute test cases of S7, RDP client should support [MS-RDPEFS]: Remote Desktop Protocol: File System Virtual Channel Extension.");
            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, false, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            //Check whether 'rdpdr' channel has been created
            if (this.rdpbcgrAdapter.GetStaticVirtualChannelId("RDPDR") == 0)
            {
                this.TestSite.Assume.Fail("The necessary channel RDPDR has not been created, so stop running this test case.");
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending SUT a Static Virtual Channel PDU and filling the length field of mcsPdu to an invalid value (less than the actual value)");
            SendStaticVirtualChannelTraffics(StaticVirtualChannel_InvalidType.InvalidMCSLength);

            RDPClientTryDropConnection("an invalid length field of mcsPdu Static Virtual Channel PDU");            
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [TestCategory("StaticVirtualChannel")]
        [Description("This test case is used to ensure SUT drops the connection when the length field of x224Data is invalid.")]
        public void S7_StaticVirtualChannel_NegativeTest_InvalidSignature()
        {
            #region Test Description
            /*
             1.  Trigger SUT to initiate and complete a RDP connection. 
             2.   After the connection sequence has been finished, Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), 
                  INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on (how to determine the notification type is described in TD section 3.3.5.10) . 
             3.   Test Suite sends SUT a Static Virtual Channel PDU and set the signature field of SecurityHeader to an incorrect value.
             4.   Test Suite expects SUT drops the connection.
            */
            #endregion

            #region Test Sequence
            if (this.selectedProtocol != selectedProtocols_Values.PROTOCOL_RDP_FLAG)
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "Not executed because this test case only applies when RDP Standard security is in effect.");
                return;
            }
            this.TestSite.Assert.IsTrue(isClientSupportRDPEFS, "To execute test cases of S7, RDP client should support [MS-RDPEFS]: Remote Desktop Protocol: File System Virtual Channel Extension.");
            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, false, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            //Check whether 'rdpdr' channel has been created
            if (this.rdpbcgrAdapter.GetStaticVirtualChannelId("RDPDR") == 0)
            {
                this.TestSite.Assume.Fail("The necessary channel RDPDR has not been created, so stop running this test case.");
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending SUT a Static Virtual Channel PDU and present an invalid signature.");
            SendStaticVirtualChannelTraffics(StaticVirtualChannel_InvalidType.InvalidSignature);

            RDPClientTryDropConnection("an invalid signature in Static Virtual Channel PDU");            
            #endregion
        }
    }
}
