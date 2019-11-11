// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using Microsoft.Win32;
using Microsoft.Protocols.TestSuites.Rdp;
using System.Management;
using Microsoft.Protocols.TestSuites.Rdpbcgr;

namespace Microsoft.Protocols.TestSuites.Rdpedyc
{
    public partial class RdpedycTestSuite
    {
        [TestMethod]
        [Priority(0)]
        [TestCategory("RDP8.1")]
        [TestCategory("RDPEDYC")]
        [TestCategory("BVT")]
        [Description(@"This test case is used to verify SUT can create and close DVC channel. ")]
        public void S1_EDYC_CreateAndCloseChannel()
        {
            #region Test Code            

            this.rdpedycAdapter.Initialize(this.Site);          

            // Set up RDPBCGR connection with server
            this.rdpedycAdapter.ConnectToServer(testConfig.transportProtocol, testConfig.requestProtocol, new string[] { RdpConstValue.SVCNAME_RDPEDYC }, CompressionType.PACKET_COMPR_TYPE_NONE,false, true);
          
            // Capability exchange
            this.Site.Log.Add(LogEntryKind.Debug, "Expect the Exchange Capabilites PDU from SUT.");

            // Expect the geometry channel create request from server
            this.Site.Log.Add(LogEntryKind.Debug, "Expect the DVC channel create PDU from SUT.");

            DynamicVirtualChannel channel = this.rdpedycAdapter.ExpectChannel(testConfig.timeout, DynamicVC_TransportType.RDP_TCP);
          
            // Expect the geometry channel close request from server
            this.Site.Log.Add(LogEntryKind.Debug, "Expect the DVC channel close PDU from SUT.");

            this.rdpedycAdapter.CloseChannel(testConfig.timeout, (ushort)channel.ChannelId);

            this.Site.Log.Add(LogEntryKind.Debug, "The DVC channel is closed successfully.");

            #endregion Test Code
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP8.1")]
        [TestCategory("RDPEDYC")]
        [Description(@"This test case is used to verify SUT can process DYNVC_DATA data. ")]
        public void S1_EDYC_SendUncompressedData()
        {
            #region Test Code

            this.rdpedycAdapter.Initialize(this.Site);

            // Set up RDPBCGR connection with server            
            this.rdpedycAdapter.ConnectToServer(testConfig.transportProtocol, testConfig.requestProtocol, new string[] { RdpConstValue.SVCNAME_RDPEDYC }, CompressionType.PACKET_COMPR_TYPE_NONE, false, true);

            // Capability exchange
            this.Site.Log.Add(LogEntryKind.Debug, "Expect the Exchange Capabilites PDU from SUT.");

            this.rdpedycAdapter.ExchangeCapabilities(testConfig.timeout);

            // Expect the geometry channel create request from server
            this.Site.Log.Add(LogEntryKind.Debug, "Expect the DVC channel create PDU from SUT.");

            DynamicVirtualChannel channel = this.rdpedycAdapter.ExpectChannel(testConfig.timeout, DynamicVC_TransportType.RDP_TCP);

            // Send data to server
            this.Site.Log.Add(LogEntryKind.Debug, "Send the DYNVC_Data PDU to SUT.");
            this.rdpedycAdapter.SendUncompressedPdu(channel.ChannelId, DynamicVC_TransportType.RDP_TCP);
           
            // Expect the geometry channel close request from server
            this.Site.Log.Add(LogEntryKind.Debug, "Expect the DVC channel close PDU from SUT.");

            this.rdpedycAdapter.CloseChannel(testConfig.timeout, (ushort)channel.ChannelId);

            this.Site.Log.Add(LogEntryKind.Debug, "The DVC channel is closed successfully.");

            #endregion Test Code
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP8.1")]
        [TestCategory("RDPEDYC")]
        [Description(@"This test case is used to verify SUT can can process compreseed data sequence. ")]
        public void S1_EDYC_SendCompressedDataSequence()
        {
            #region Test Code

            this.rdpedycAdapter.Initialize(this.Site);

            // Set up RDPBCGR connection with server
            this.rdpedycAdapter.ConnectToServer(testConfig.transportProtocol, testConfig.requestProtocol, new string[] { RdpConstValue.SVCNAME_RDPEDYC }, CompressionType.PACKET_COMPR_TYPE_NONE, false, true);

            // Capability exchange
            this.Site.Log.Add(LogEntryKind.Debug, "Expect the Exchange Capabilites PDU from SUT.");

            // This PDU MUST NOT be used unless both DVC managers support version 3 of the Remote Desktop Protocol: dynamic Virtual Channel Extension, and a reliable transport is being used (UDP-Random or TCP).)
            DYNVC_CAPS_Version capVersion = this.rdpedycAdapter.ExchangeCapabilities(testConfig.timeout);

            if (capVersion != DYNVC_CAPS_Version.VERSION3)
            {
                this.Site.Assert.Inconclusive("Compressed data only be used when DVC managers support Version 3 of the EDYC channel.");
            }

            // Expect the geometry channel create request from server
            this.Site.Log.Add(LogEntryKind.Debug, "Expect the DVC channel create PDU from SUT.");

            DynamicVirtualChannel channel = this.rdpedycAdapter.ExpectChannel(testConfig.timeout, DynamicVC_TransportType.RDP_TCP);

            // Send compressed data sequence to server
            this.Site.Log.Add(LogEntryKind.Debug, "Expect the DYNVC_Data PDU from SUT.");
           
            this.rdpedycAdapter.SendCompressedSequencePdu( channel.ChannelId, DynamicVC_TransportType.RDP_TCP);            

            // Expect the geometry channel close request from server
            this.Site.Log.Add(LogEntryKind.Debug, "Expect the DVC channel close PDU from SUT.");

            this.rdpedycAdapter.CloseChannel(testConfig.timeout, (ushort)channel.ChannelId);

            this.Site.Log.Add(LogEntryKind.Debug, "The DVC channel is closed successfully.");

            #endregion Test Code
        }       
    }
}
