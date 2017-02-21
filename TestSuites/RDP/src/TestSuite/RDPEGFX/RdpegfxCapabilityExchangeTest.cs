// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx;
using Microsoft.Protocols.TestSuites.Rdpbcgr;
using Microsoft.Protocols.TestSuites.Rdp;

namespace Microsoft.Protocols.TestSuites.Rdpegfx
{
    public partial class RdpegfxTestSuite : RdpTestClassBase
    {
        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test capability advertise from client and server sends capability confirm.")]
        public void RDPEGFX_CapabilityExchange_PositiveTest()
        {
            this.TestSite.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            this.TestSite.Log.Add(LogEntryKind.Debug, "Creating dynamic virtual channels for MS-RDPEGFX ...");
            bool bProtocolSupported = this.rdpegfxAdapter.ProtocolInitialize(this.rdpedycServer);
            this.TestSite.Assert.IsTrue(bProtocolSupported, "Client should support this protocol.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting capability advertise from client.");
            RDPGFX_CAPS_ADVERTISE capsAdv = this.rdpegfxAdapter.ExpectCapabilityAdvertise();
            this.TestSite.Assert.IsNotNull(capsAdv, "RDPGFX_CAPS_ADVERTISE is received.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending capability confirm to client.");
            // Set first capset in capability advertise request, if no capdata in request, use default flag.
            CapsFlags capFlag = CapsFlags.RDPGFX_CAPS_FLAG_DEFAULT;
            if (capsAdv.capsSetCount > 0)
                capFlag = (CapsFlags) BitConverter.ToUInt32(capsAdv.capsSets[0].capsData, 0);
            this.rdpegfxAdapter.SendCapabilityConfirm(capFlag);

        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("Server set an incorrect capability version in CapabilityConfirm response.")]
        public void RDPEGFX_CapabilityExchange_Negative_IncorrectVersion()
        {
            this.TestSite.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            this.TestSite.Log.Add(LogEntryKind.Debug, "Creating dynamic virtual channels for MS-RDPEGFX ...");
            bool bProtocolSupported = this.rdpegfxAdapter.ProtocolInitialize(this.rdpedycServer);
            this.TestSite.Assert.IsTrue(bProtocolSupported, "Client should support this protocol.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting capability advertise from client.");
            RDPGFX_CAPS_ADVERTISE capsAdv = this.rdpegfxAdapter.ExpectCapabilityAdvertise();
            this.TestSite.Assert.IsNotNull(capsAdv, "RDPGFX_CAPS_ADVERTISE is received.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdpegfxNegativeTypes.Capability_Incorrect_Version);
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.Capability_Incorrect_Version);

            // Set first capset in capability advertise request, if no capdata in request, use default flag.
            CapsFlags capFlag = CapsFlags.RDPGFX_CAPS_FLAG_DEFAULT;
            if (capsAdv.capsSetCount > 0)
                capFlag = (CapsFlags)BitConverter.ToUInt32(capsAdv.capsSets[0].capsData, 0);
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending capability confirm message with an incorrect capability version to client.");
            this.rdpegfxAdapter.SendCapabilityConfirm(capFlag);

            RDPClientTryDropConnection("capability confirm message with an incorrect capability version");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("Server set an incorrect CapsDataLength in CapabilityConfirm response")]
        public void RDPEGFX_CapabilityExchange_Negative_IncorrectCapsDataLength()
        {

            this.TestSite.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            this.TestSite.Log.Add(LogEntryKind.Debug, "Creating dynamic virtual channels for MS-RDPEGFX ...");
            bool bProtocolSupported = this.rdpegfxAdapter.ProtocolInitialize(this.rdpedycServer);
            this.TestSite.Assert.IsTrue(bProtocolSupported, "Client should support this protocol.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting capability advertise from client.");
            RDPGFX_CAPS_ADVERTISE capsAdv = this.rdpegfxAdapter.ExpectCapabilityAdvertise();
            this.TestSite.Assert.IsNotNull(capsAdv, "RDPGFX_CAPS_ADVERTISE is received.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdpegfxNegativeTypes.Capability_Incorrect_CapsDatalength);
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.Capability_Incorrect_CapsDatalength);

            // Set first capset in capability advertise request, if no capdata in request, use default flag.
            CapsFlags capFlag = CapsFlags.RDPGFX_CAPS_FLAG_DEFAULT;
            if (capsAdv.capsSetCount > 0)
                capFlag = (CapsFlags)BitConverter.ToUInt32(capsAdv.capsSets[0].capsData, 0);
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending capability confirm message with incorrect CapsDataLength to client.");
            this.rdpegfxAdapter.SendCapabilityConfirm(capFlag);

            RDPClientTryDropConnection("capability confirm message with incorrect CapsDataLength");            
        }
                
    }
}
