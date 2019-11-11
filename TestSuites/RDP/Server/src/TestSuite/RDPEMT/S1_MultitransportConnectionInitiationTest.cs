// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpemt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestSuites.Rdpemt
{
    public partial class RdpemtTestSuite
    {
        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEMT")]
        [Description(@"This test case is used to ensure multistranport connection can initiated successfully with SUT.")]
        public void S1_MultitransportConnectionInitiation_PositiveTest()
        {
            #region Test Steps
            // 1. Initiate an RDP connection to RDP server (SUT) and complete the Connection Initiation phase, Basic Setting Exchange phase, Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange Phase and Licensing phase. Indicate support for both reliable and lossy multitransport in basic setting exchange phase.
            // 2. Test Suite expects the Server Initiate Multitransport Request PDUs with requestedProtocol supported by SUT. When received, Test Suite verifies this PDUs.
            // 3. Start the underlying multitransport connect with SUT.
            // 4. Send the Tunnel Create Request PDU to SUT.
            // 5. Expect the Tunnel Create Response from SUT and verify.
            #endregion Test Steps

            #region Test Code

            ConnectionInitiationTest(NegativeType.None);

            #endregion Test Code
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEMT")]
        [Description(@"This test case is used to ensure SUT will fail as expect if RequestID is invalid during multitransport connection initiation.")]
        public void S1_MultitransportConnectionInitiation_NegativeTest_InvalidRequestID()
        {
            #region Test Steps
            // 1. Initiate an RDP connection to RDP server (SUT) and complete the Connection Initiation phase, Basic Setting Exchange phase, Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange Phase and Licensing phase. Indicate support for both reliable and lossy multitransport in basic setting exchange phase.
            // 2. Test Suite expects the Server Initiate Multitransport Request PDUs with requestedProtocol supported by SUT. When received, Test Suite verifies this PDUs.
            // 3. Start the underlying multitransport connect with SUT.
            // 4. Send the Tunnel Create Request PDU to SUT with invalid SecurityCookie.
            // 5. Expect SUT will either close the connection or respond Tunnel Create Response with HRESULT code indicating failure.
            #endregion Test Steps

            #region Test Code

            ConnectionInitiationTest(NegativeType.TunnelCreateRequest_InvalidRequestID);
            #endregion Test Code
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEMT")]
        [Description(@"This test case is used to ensure SUT will fail as expect if SecurityCookie is invalid during multitransport connection initiation.")]
        public void S1_MultitransportConnectionInitiation_NegativeTest_InvalidSecurityCookie()
        {
            #region Test Steps
            // 1. Initiate an RDP connection to RDP server (SUT) and complete the Connection Initiation phase, Basic Setting Exchange phase, Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange Phase and Licensing phase. Indicate support for both reliable and lossy multitransport in basic setting exchange phase.
            // 2. Test Suite expects the Server Initiate Multitransport Request PDUs with requestedProtocol supported by SUT. When received, Test Suite verifies this PDUs.
            // 3. Start the underlying multitransport connect with SUT.
            // 4. Send the Tunnel Create Request PDU to SUT with invalid RequestID.
            // 5. Expect SUT will either close the connection or respond Tunnel Create Response with HRESULT code indicating failure.
            #endregion Test Steps

            #region Test Code

            ConnectionInitiationTest(NegativeType.TunnelCreateRequest_InvalidSecurityCookie);
            #endregion Test Code
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEMT")]
        [Description(@"This test case is used to ensure SUT will fail as expect if RequestID and SecurityCookie is invalid during multitransport connection initiation.")]
        public void S1_MultitransportConnectionInitiation_NegativeTest_InvalidRequestIDAndSecurityCookie()
        {
            #region Test Steps
            // 1. Initiate an RDP connection to RDP server (SUT) and complete the Connection Initiation phase, Basic Setting Exchange phase, Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange Phase and Licensing phase. Indicate support for both reliable and lossy multitransport in basic setting exchange phase.
            // 2. Test Suite expects the Server Initiate Multitransport Request PDUs with requestedProtocol supported by SUT. When received, Test Suite verifies this PDUs.
            // 3. Start the underlying multitransport connect with SUT.
            // 4. Send the Tunnel Create Request PDU to SUT with invalid RequestID and SecurityCookie.
            // 5. Expect SUT will either close the connection or respond Tunnel Create Response with HRESULT code indicating failure.
            #endregion Test Steps

            #region Test Code

            ConnectionInitiationTest(NegativeType.TunnelCreateRequest_InvalidRequestIDAndSecurityCookie);
            #endregion Test Code
        }

        private void ConnectionInitiationTest(NegativeType negativeType)
        {
            Site.Log.Add(LogEntryKind.TestStep, "1. Initiate an RDP connection to RDP server (SUT) and complete the Connection Initiation phase, Basic Setting Exchange phase, Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange Phase and Licensing phase. Indicate support for both reliable and lossy multitransport in basic setting exchange phase.");

            StartRDPConnect();

            Site.Log.Add(LogEntryKind.TestStep, "2. Test Suite expects the Server Initiate Multitransport Request PDUs with requestedProtocol supported by SUT. When received, Test Suite verifies this PDUs.");

            var serverInitiateMultitransportRequestPDUs = ExpectServerInitiateMultitransportRequestPDUs();

            Site.Log.Add(LogEntryKind.TestStep, "3. Start the underlying multitransport connect with SUT.");

            rdpemtAdapter.StartMultitransportConnect(serverInitiateMultitransportRequestPDUs.ToArray());

            Site.Log.Add(LogEntryKind.TestStep, "4. Send the Tunnel Create Request PDU to SUT with invalid RequestID.");

            foreach (var pdu in serverInitiateMultitransportRequestPDUs)
            {
                rdpemtAdapter.SendTunnelCreateRequest(negativeType, pdu.requestedProtocol);
            }

            var responses = new List<RDP_TUNNEL_CREATERESPONSE>();

            foreach (var pdu in serverInitiateMultitransportRequestPDUs)
            {
                var response = rdpemtAdapter.ExpectTunnelCreateResponse(pdu.requestedProtocol);

                responses.Add(response);
            }

            if (negativeType == NegativeType.None)
            {
                Site.Log.Add(LogEntryKind.TestStep, "5. Expect the Tunnel Create Response from SUT and verify.");

                foreach (var response in responses)
                {
                    Site.Log.Add(LogEntryKind.Comment, "If a match for the RequestID and SecurityCookie pair is found on the server for a pending multitransport request, the server associates the incoming multitransport connection with the existing session and MUST send the client an RDP_TUNNEL_CREATERESPONSE PDU with a successful HRESULT code.");

                    if (testConfig.isWindowsImplementation)
                    {
                        Site.Log.Add(LogEntryKind.Comment, "Windows always sends an HrResponse code of S_OK (0x0) if the connection is accepted");

                        Site.Assert.AreEqual((uint)HrResponse_Value.S_OK, response.HrResponse, "SUT return HRESULT code S_OK.");
                    }
                    else
                    {
                        bool isSuccess = (response.HrResponse & (uint)HRESULT.S) == 0;
                        Site.Assert.IsTrue(isSuccess, "SUT returned a successful HRESULT code.");
                    }
                }
            }
            else
            {
                Site.Log.Add(LogEntryKind.TestStep, "5. Expect SUT will either close the connection or respond Tunnel Create Response with HRESULT code indicating failure.");

                foreach (var response in responses)
                {
                    Site.Log.Add(LogEntryKind.Comment, "If a match is not found, the server can either close the connection to the client or send an RDP_TUNNEL_CREATERESPONSE PDU with an unsuccessful HRESULT code.");

                    if (testConfig.isWindowsImplementation)
                    {
                        Site.Log.Add(LogEntryKind.Comment, "Windows closes the connection to the client if a successful match is not found.");

                        Site.Assert.IsNull(response, "SUT closed the underlying connection.");
                    }
                    else
                    {
                        if (response == null)
                        {
                            Site.Assert.Pass("SUT closed the underlying connection.");
                        }
                        else
                        {
                            bool isSuccess = (response.HrResponse & (uint)HRESULT.S) == 0;
                            Site.Assert.IsFalse(isSuccess, "SUT returned an unsuccessful HRESULT code.");
                        }
                    }
                }
            }
        }

    }
}