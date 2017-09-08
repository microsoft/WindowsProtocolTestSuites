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
        [Description("This test case is used to ensure SUT can process the Deactivation-Reactivation successfully.")]
        public void BVT_ReactivationTest_PositiveTest_BitmapHostCacheSupported()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process the Deactivation-Reactivation successfully.
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection.
            2.	Test Suite sends a Server Demand Active PDU to client Deactivate All PDU.
            3.	Test Suite starts the Deactivation-Reactivation sequence by sending a Server Demand Active PDU to SUT and presents the Bitmap Host Cache Support Capability Set.
            4.	Test Suite expects SUT continue the  Deactivation-Reactivation sequence by sending the following PDUs sequentially:
                >	Client Confirm Active PDU
                >	Client Synchronize PDU
                >	Client Control PDU - Cooperate
                >	Client Control PDU - Request Control
                >	Client Font List PDU
            5.	Test suite then sends the following PDUs to SUT sequentially to finish the  Deactivation-Reactivation sequence:
                >	Server Synchronize PDU
                >	Server Control PDU - Cooperate
                >	Server Control PDU - Granted Control
                >	Server Font Map PDU
            6.	Test Suite Expects SUT continues the RDP session with sending at least one input PDU.
            */
            #endregion

            #region Test Sequence

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

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, @"Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, @"Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            //Start a Deactivate-Reactivate sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Initiating a Deactivate-Reactivate sequence.");
            this.rdpbcgrAdapter.DeactivateReactivate();

            //Server initiates a diconnection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Initiating a disconnection.");
            this.rdpbcgrAdapter.ServerInitiatedDisconnect(true, true, NegativeType.None);

            //Stop RDP listening.
            //this.rdpbcgrAdapter.StopRDPListening();
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]        
        [Description("This test case is used to ensure SUT can process the Deactivation-Reactivation successfully.")]
        public void S2_ReactivationTest_PositiveTest_BitmapHostCacheNotSupported()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process the Deactivation-Reactivation successfully.
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection.
            2.	Test Suite sends a Server Demand Active PDU to client Deactivate All PDU.
            3.	Test Suite starts the Deactivation-Reactivation sequence by sending a Server Demand Active PDU to SUT and does not present the Bitmap Host Cache Support Capability Set.
            4.	Test Suite expects SUT continue the  Deactivation-Reactivation sequence by sending the following PDUs sequentially:
                >	Client Confirm Active PDU
                >	Client Synchronize PDU
                >	Client Control PDU - Cooperate
                >	Client Control PDU - Request Control
                >	Client Font List PDU
            5.	Test suite then sends the following PDUs to SUT sequentially to finish the  Deactivation-Reactivation sequence:
                >	Server Synchronize PDU
                >	Server Control PDU - Cooperate
                >	Server Control PDU - Granted Control
                >	Server Font Map PDU
            6.	Test Suite Expects SUT continues the RDP session with sending at least one input PDU.
            */
            #endregion

            #region Test Sequence

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

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            //Set Server Capability with Bitmap Host Cache not supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(false, true, true, true, true, true, true, true, true, true);

            //Start a Deactivate-Reactivate sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Initiating a Deactivate-Reactivate sequence.");
            this.rdpbcgrAdapter.DeactivateReactivate();

            //Server initiates a diconnection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Initiating a disconnection.");
            this.rdpbcgrAdapter.ServerInitiatedDisconnect(true, true, NegativeType.None);

            //Stop RDP listening.
            //this.rdpbcgrAdapter.StopRDPListening();
            #endregion
        }
    }
}
