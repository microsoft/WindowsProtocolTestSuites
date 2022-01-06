// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.Protocols.TestSuites.Rdpbcgr;
using Microsoft.Protocols.TestSuites.Rdpegt;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpevor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Protocols.TestSuites.Rdpevor
{
    [TestClass]
    public partial class RdpevorTestSuite : RdpTestClassBase
    {

        #region Adapter Instances

        private IRdpevorAdapter  rdpevorAdapter;
        private IRdpegtAdapter rdpegtAdapter;
        private RdpedycServer rdpedycServer;
        private RdpevorTestData testData;

        #endregion

        #region Class Initialization and Cleanup
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            RdpTestClassBase.BaseInitialize(context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            RdpTestClassBase.BaseCleanup();
        }
        #endregion

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            base.TestInitialize();

            this.rdpevorAdapter = this.TestSite.GetAdapter<IRdpevorAdapter>();
            this.rdpegtAdapter = this.TestSite.GetAdapter<IRdpegtAdapter>();

            this.rdpevorAdapter.Reset();
            this.rdpegtAdapter.Reset();
           
            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            GetTestData();
        }

        protected override void TestCleanup()
        {
            // TestCleanup() may be not main thread
            DynamicVCException.SetCleanUp(true);

            base.TestCleanup();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Dispose virtual channel manager.");
            if(rdpedycServer!=null)
                rdpedycServer.Dispose();
            
            this.TestSite.Log.Add(LogEntryKind.Comment, "Trigger client to close all RDP connections for clean up.");

            TriggerClientDisconnectAll();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Stop RDP listening.");
            this.rdpbcgrAdapter?.StopRDPListening();

            DynamicVCException.SetCleanUp(false);
        }
        #endregion

        #region Private Methods

        //Set default server capabilities
        private void setServerCapabilitiesWithRemoteFxSupported()
        {
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true,
               true, maxRequestSize,
               true,
               true, 1,
               true, CmdFlags_Values.SURFCMDS_FRAMEMARKER | CmdFlags_Values.SURFCMDS_SETSURFACEBITS | CmdFlags_Values.SURFCMDS_STREAMSURFACEBITS,
               true, true, true);
        }

        //Start RDP connection.
        private void StartRDPConnection(bool createReliableUDPtransport = false,
            bool createLossyUDPtransport = false)
        {

            #region Trigger client to connect
            //Trigger client to connect. 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            TriggerClientRDPConnect(transportProtocol);
            #endregion

            #region RDPBCGR Connection

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with RomoteFX codec supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            setServerCapabilitiesWithRemoteFxSupported();

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn);

            #endregion

            this.rdpedycServer = new RdpedycServer(this.rdpbcgrAdapter.ServerStack, this.rdpbcgrAdapter.SessionContext);
            rdpedycServer.ExchangeCapabilities(waitTime);
            bool initRdpegtChannel = this.rdpegtAdapter.ProtocolInitialize(this.rdpedycServer);
            Assert.IsTrue(initRdpegtChannel, "Creation of RDPEGT channel failed!");

        }

        //Stop RDP connection.
        private void StopRDPConnection()
        {
            TriggerClientDisconnectAll();

            this.rdpbcgrAdapter.Reset();
            this.rdpevorAdapter.Reset();
            this.rdpegtAdapter.Reset();
        }

        //private get Test Data
        private void GetTestData()
        {
            testData = new RdpevorTestData();
            try
            {
                String RdpevorTestDataPath;
                PtfPropUtility.GetPtfPropertyValue(Site, "RdpevorTestDataPath", out RdpevorTestDataPath);
                testData.LoadXMLFile(RdpevorTestDataPath);
            }
            catch (System.Xml.XmlException ex)
            {
                this.TestSite.Assert.Fail(ex.Message);
            }            
        }
        #endregion
    }
}

