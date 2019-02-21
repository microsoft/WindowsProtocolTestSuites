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
        public void S9_EDYC_CreateAndCloseChannel()
        {            
            #region Test Code
           
            this.rdpedycAdapter.Initialize(this.Site);

            // Set up RDPBCGR connection with server
            this.rdpedycAdapter.ConnectToServer(transportProtocol, requestProtocol, new string[] { RdpConstValue.SVCNAME_RDPEDYC }, CompressionType.PACKET_COMPR_TYPE_NONE,false, true);
          
            // Capability exchange
            this.Site.Log.Add(LogEntryKind.Debug, "Expect the Exchange Capabilites PDU from SUT.");

            this.rdpedycAdapter.ExchangeCapabilities(timeout);

            // Expect the geometry channel create request from server
            this.Site.Log.Add(LogEntryKind.Debug, "Expect the DVC channel create PDU from SUT.");

            DynamicVirtualChannel channel = this.rdpedycAdapter.ExpectChannel(timeout, "Microsoft::Windows::RDS::Geometry::v08.01", DynamicVC_TransportType.RDP_TCP);
          
            // Expect the geometry channel close request from server
            this.Site.Log.Add(LogEntryKind.Debug, "Expect the DVC channel close PDU from SUT.");

            this.rdpedycAdapter.CloseChannel(timeout, (ushort)channel.ChannelId);

            this.Site.Log.Add(LogEntryKind.Debug, "The DVC channel is closed successfully.");

            #endregion Test Code

        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP8.1")]
        [TestCategory("RDPEDYC")]
        [TestCategory("NonBVT")]
        [Description(@"This test case is used to verify SUT can process DYNVC_DATA data. ")]
        public void S9_EDYC_SendUncompressedData()
        {
            #region Test Code

            this.rdpedycAdapter.Initialize(this.Site);

            // Set up RDPBCGR connection with server            
            this.rdpedycAdapter.ConnectToServer(transportProtocol, requestProtocol, new string[] { RdpConstValue.SVCNAME_RDPEDYC }, CompressionType.PACKET_COMPR_TYPE_NONE, false, true);

            // Capability exchange
            this.Site.Log.Add(LogEntryKind.Debug, "Expect the Exchange Capabilites PDU from SUT.");

            this.rdpedycAdapter.ExchangeCapabilities(timeout);

            // Expect the geometry channel create request from server
            this.Site.Log.Add(LogEntryKind.Debug, "Expect the DVC channel create PDU from SUT.");

            DynamicVirtualChannel channel = this.rdpedycAdapter.ExpectChannel(timeout, "Microsoft::Windows::RDS::Geometry::v08.01", DynamicVC_TransportType.RDP_TCP);

            // Send data to server
            this.Site.Log.Add(LogEntryKind.Debug, "Send the DYNVC_Data PDU to SUT.");
            this.rdpedycAdapter.SendUncompressedPdu(channel.ChannelId, DynamicVC_TransportType.RDP_TCP);
           
            // Expect the geometry channel close request from server
            this.Site.Log.Add(LogEntryKind.Debug, "Expect the DVC channel close PDU from SUT.");

            this.rdpedycAdapter.CloseChannel(timeout, (ushort)channel.ChannelId);

            this.Site.Log.Add(LogEntryKind.Debug, "The DVC channel is closed successfully.");

            #endregion Test Code


        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP8.1")]
        [TestCategory("RDPEDYC")]
        [TestCategory("NonBVT")]
        [Description(@"This test case is used to verify SUT can can process compreseed data sequence. ")]
        public void S9_EDYC_SendCompressedDataSequence()
        {
            #region Test Code

            this.rdpedycAdapter.Initialize(this.Site);

            // Set up RDPBCGR connection with server
            this.rdpedycAdapter.ConnectToServer(transportProtocol, requestProtocol, new string[] { RdpConstValue.SVCNAME_RDPEDYC }, CompressionType.PACKET_COMPR_TYPE_NONE, false, true);

            // Capability exchange
            this.Site.Log.Add(LogEntryKind.Debug, "Expect the Exchange Capabilites PDU from SUT.");

            this.rdpedycAdapter.ExchangeCapabilities(timeout);

            // Expect the geometry channel create request from server
            this.Site.Log.Add(LogEntryKind.Debug, "Expect the DVC channel create PDU from SUT.");

            DynamicVirtualChannel channel = this.rdpedycAdapter.ExpectChannel(timeout, "Microsoft::Windows::RDS::Geometry::v08.01", DynamicVC_TransportType.RDP_TCP);

            // Send compressed data sequence to server
            this.Site.Log.Add(LogEntryKind.Debug, "Expect the DYNVC_Data PDU from SUT.");
           
            this.rdpedycAdapter.SendCompressedSequencePdu( channel.ChannelId, DynamicVC_TransportType.RDP_TCP);            

            // Expect the geometry channel close request from server
            this.Site.Log.Add(LogEntryKind.Debug, "Expect the DVC channel close PDU from SUT.");

            this.rdpedycAdapter.CloseChannel(timeout, (ushort)channel.ChannelId);

            this.Site.Log.Add(LogEntryKind.Debug, "The DVC channel is closed successfully.");

            #endregion Test Code

        }       
    }
}
