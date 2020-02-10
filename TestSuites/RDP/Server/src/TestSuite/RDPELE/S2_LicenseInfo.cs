// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpele;
using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.Protocols.TestSuites.Rdpbcgr;

namespace Microsoft.Protocols.TestSuites.Rdpele
{
    public partial class RdpeleTestSuite
    {
        [TestMethod]
        [Priority(0)]
        [TestCategory("RDPELE")]
        [TestCategory("BVT")]
        [TestCategory("RDP7.0")]
        [Description(@"This test case is used to verify SUT can handle Client License Information message correctly. ")]
        public void S2_ELE_LicenseInfo()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Set up a RDPBCGR connection with server.");
            StartRDPConnect();

            TS_LICENSE_PDU licensePdu = rdpbcgrAdapter.rdpeleClient.ExpectPdu(testConfig.timeout);
            Site.Log.Add(LogEntryKind.TestStep, "Start a RDPELE New license procedure.");
            Site.Assert.AreEqual(bMsgType_Values.LICENSE_REQUEST, licensePdu.preamble.bMsgType, $"A LICENSE_REQUEST message should be received from server, the real message type is {licensePdu.preamble.bMsgType}");

            Site.Log.Add(LogEntryKind.TestStep, "Send Client New License Request to the server.");
            rdpbcgrAdapter.rdpeleClient.SendClientNewLicenseRequest(
                KeyExchangeAlg.KEY_EXCHANGE_ALG_RSA, (uint)Client_OS_ID.CLIENT_OS_ID_WINNT_POST_52 | (uint)Client_Image_ID.CLIENT_IMAGE_ID_MICROSOFT, testConfig.userName, testConfig.localAddress);
            licensePdu = rdpbcgrAdapter.rdpeleClient.ExpectPdu(testConfig.timeout);
            Site.Assert.AreEqual(bMsgType_Values.PLATFORM_CHALLENGE, licensePdu.preamble.bMsgType, $"A PLATFORM_CHALLENGE message should be received from server, the real message type is {licensePdu.preamble.bMsgType}");

            Site.Log.Add(LogEntryKind.TestStep, "Send Client Platform Challenge Response to the server.");
            uint platformId = (uint)Client_OS_ID.CLIENT_OS_ID_WINNT_POST_52 | (uint)Client_Image_ID.CLIENT_IMAGE_ID_MICROSOFT;
            Random random = new Random();
            CLIENT_HARDWARE_ID clientHWID = new CLIENT_HARDWARE_ID {
                PlatformId = platformId,
                Data1 = (uint)random.Next(),
                Data2 = (uint)random.Next(),
                Data3 = (uint)random.Next(),
                Data4 = (uint)random.Next()
            };
            rdpbcgrAdapter.rdpeleClient.SendClientPlatformChallengeResponse(clientHWID);
            licensePdu = rdpbcgrAdapter.rdpeleClient.ExpectPdu(testConfig.timeout);
            Site.Assert.AreEqual(bMsgType_Values.NEW_LICENSE, licensePdu.preamble.bMsgType, $"A NEW_LICENSE message should be received from server, the real message type is {licensePdu.preamble.bMsgType}");
            byte[] licenseInfo = rdpbcgrAdapter.rdpeleClient.GetNewLicenseInfo().Value.pbLicenseInfo; // Get the license info for the next step
            rdpbcgrAdapter.Disconnect();

            Site.Log.Add(LogEntryKind.TestStep, "Set up a second RDPBCGR connection with server.");
            rdpbcgrAdapter = new RdpbcgrAdapter(testConfig);
            rdpbcgrAdapter.Initialize(Site);
            StartRDPConnect();

            Site.Log.Add(LogEntryKind.TestStep, "Start a RDPELE license info procedure.");
            licensePdu = rdpbcgrAdapter.rdpeleClient.ExpectPdu(testConfig.timeout);
            Site.Assert.AreEqual(bMsgType_Values.LICENSE_REQUEST, licensePdu.preamble.bMsgType, $"A LICENSE_REQUEST message should be received from server, the real message type is {licensePdu.preamble.bMsgType}");
            Site.Log.Add(LogEntryKind.TestStep, "Send Client License Information to the server.");
            rdpbcgrAdapter.rdpeleClient.SendClientLicenseInformation(
                KeyExchangeAlg.KEY_EXCHANGE_ALG_RSA,
                platformId, // same platformId as the first procedure (new license).
                licenseInfo, // the license info retreived from the server in the first procedure.
                clientHWID); // same clientHWID as the first procedure.
            licensePdu = rdpbcgrAdapter.rdpeleClient.ExpectPdu(testConfig.timeout);

            if (testConfig.issueTemporaryLicenseForTheFirstTime)
            {
                Site.Log.Add(LogEntryKind.Comment, "If the client presents a license that requires upgrading, that is a valid temporary license. " +
                    "The server MUST respond back with Server Platform Challenge message to the client.");
                Site.Assert.AreEqual(bMsgType_Values.PLATFORM_CHALLENGE, licensePdu.preamble.bMsgType, $"A PLATFORM_CHALLENGE message should be received from server, the real message type is {licensePdu.preamble.bMsgType}");
                Site.Log.Add(LogEntryKind.TestStep, "Send Client Platform Challenge Response to the server.");
                rdpbcgrAdapter.rdpeleClient.SendClientPlatformChallengeResponse(clientHWID);
                licensePdu = rdpbcgrAdapter.rdpeleClient.ExpectPdu(testConfig.timeout);
                Site.Assert.AreEqual(bMsgType_Values.UPGRADE_LICENSE, licensePdu.preamble.bMsgType, $"An UPGRADE_LICENSE message should be received from server, the real message type is {licensePdu.preamble.bMsgType}");
            }
            else
            {
                Site.Log.Add(LogEntryKind.Comment, "The client presents a valid permanent license that does not require an upgrade. " +
                    "The server MUST send a license error message with the error code STATUS_VALID_CLIENT and the state transition code ST_NO_TRANSITION.The licensing protocol is complete at this point.");
                Site.Assert.AreEqual(bMsgType_Values.ERROR_ALERT, licensePdu.preamble.bMsgType, $"An ERROR_ALERT message should be received from server, the real message type is {licensePdu.preamble.bMsgType}");
                Site.Assert.AreEqual(dwErrorCode_Values.STATUS_VALID_CLIENT,
                    licensePdu.LicensingMessage.LicenseError.Value.dwErrorCode,
                    $"The error code of the ERROR_ALERT message should be STATUS_VALID_CLIENT, the real error code is {licensePdu.LicensingMessage.LicenseError.Value.dwErrorCode}");
            }

        }           
       
    }
}
