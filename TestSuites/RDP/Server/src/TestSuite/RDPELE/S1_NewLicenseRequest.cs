// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpele;
using Microsoft.Protocols.TestSuites.Rdp;

namespace Microsoft.Protocols.TestSuites.Rdpele
{
    public partial class RdpeleTestSuite
    {
        [TestMethod]
        [Priority(0)]
        [TestCategory("RDPELE")]
        [TestCategory("BVT")]
        [TestCategory("RDP7.0")]
        [Description("This test case is used to verify SUT can handle Client New License Request correctly. ")]
        public void S1_ELE_NewLicenseRequest()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Set up a RDPBCGR connection with server.");
            StartRDPConnect();

            TS_LICENSE_PDU licensePdu = rdpbcgrAdapter.rdpeleClient.ExpectPdu(testConfig.timeout);
            Site.Log.Add(LogEntryKind.TestStep, "Start a RDPELE New license procedure");
            Site.Assert.AreEqual(bMsgType_Values.LICENSE_REQUEST, licensePdu.preamble.bMsgType, $"A LICENSE_REQUEST message should be received from server, the real message type is {licensePdu.preamble.bMsgType}");

            Site.Log.Add(LogEntryKind.TestStep, "Send Client New License Request to the server.");
            rdpbcgrAdapter.rdpeleClient.SendClientNewLicenseRequest(
                KeyExchangeAlg.KEY_EXCHANGE_ALG_RSA, (uint)Client_OS_ID.CLIENT_OS_ID_WINNT_POST_52 | (uint)Client_Image_ID.CLIENT_IMAGE_ID_MICROSOFT, testConfig.userName, testConfig.localAddress);
            licensePdu = rdpbcgrAdapter.rdpeleClient.ExpectPdu(testConfig.timeout);
            Site.Assert.AreEqual(bMsgType_Values.PLATFORM_CHALLENGE, licensePdu.preamble.bMsgType, $"A PLATFORM_CHALLENGE message should be received from server, the real message type is {licensePdu.preamble.bMsgType}");

            Site.Log.Add(LogEntryKind.TestStep, "Send Client Platform Challenge Response to the server.");
            Random random = new Random();
            rdpbcgrAdapter.rdpeleClient.SendClientPlatformChallengeResponse(
                new CLIENT_HARDWARE_ID {
                    PlatformId = (uint)Client_OS_ID.CLIENT_OS_ID_WINNT_POST_52 | (uint)Client_Image_ID.CLIENT_IMAGE_ID_MICROSOFT,
                    Data1 = (uint)random.Next(),
                    Data2 = (uint)random.Next(),
                    Data3 = (uint)random.Next(),
                    Data4 = (uint)random.Next()
                });
            licensePdu = rdpbcgrAdapter.rdpeleClient.ExpectPdu(testConfig.timeout);
            Site.Assert.AreEqual(bMsgType_Values.NEW_LICENSE, licensePdu.preamble.bMsgType, $"A NEW_LICENSE message should be received from server, the real message type is {licensePdu.preamble.bMsgType}");
        }
       
    }
}
