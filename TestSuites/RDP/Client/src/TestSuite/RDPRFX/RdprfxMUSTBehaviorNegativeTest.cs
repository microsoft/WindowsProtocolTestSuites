// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;
using Microsoft.Protocols.TestSuites.Rdpbcgr;

namespace Microsoft.Protocols.TestSuites.Rdprfx
{
    public partial class RdprfxTestSuite
    {        
        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]
        [Description(@"Negative test for MUST statements in message of TS_RFX_CODEC_CHANNELT.")]
        public void Rdprfx_MUST_NegativeTest_TsRfxCodecChannelT()
        {
            //Test Type List
            RdprfxNegativeType[] negTestTypeArr = new RdprfxNegativeType[]{
                RdprfxNegativeType.TsRfxCodecChannelT_InvalidCodecId,
                RdprfxNegativeType.TsRfxCodecChannelT_InvalidChannelId};

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            Dictionary<RdprfxNegativeType, bool> testResultDic = new Dictionary<RdprfxNegativeType, bool>();

            foreach (RdprfxNegativeType negType in negTestTypeArr)
            {
                bool fDisconnected = rdprfxNegativeTest(negType);
                testResultDic.Add(negType, fDisconnected);
            }

            logNegativeTestResult(testResultDic);
        }
        
        [TestMethod]
        [Priority(2)]        
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]
        [Description(@"Negative test for MUST statements in message of TS_RFX_SYNC.")]
        public void Rdprfx_MUST_NegativeTest_TsRfxSync()
        {
            //Test Type List
            RdprfxNegativeType[] negTestTypeArr = new RdprfxNegativeType[]{
                RdprfxNegativeType.TsRfxSync_InvalidMagic,
                RdprfxNegativeType.TsRfxSync_InvalidVersion};

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            Dictionary<RdprfxNegativeType, bool> testResultDic = new Dictionary<RdprfxNegativeType, bool>();

            foreach (RdprfxNegativeType negType in negTestTypeArr)
            {
                bool fDisconnected = rdprfxNegativeTest(negType);
                testResultDic.Add(negType, fDisconnected);
            }

            logNegativeTestResult(testResultDic);
        }

        [TestMethod]
        [Priority(2)]        
        [TestCategory("RDP7.1")]
        [TestCategory("RDPRFX")]
        [Description(@"Negative test for MUST statements in message of TS_RFX_CHANNELS.")]
        public void Rdprfx_MUST_NegativeTest_TsRfxChannels()
        {
            //Test Type List
            RdprfxNegativeType[] negTestTypeArr = new RdprfxNegativeType[]{
                RdprfxNegativeType.TsRfxChannels_InvalidChannelId};

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            Dictionary<RdprfxNegativeType, bool> testResultDic = new Dictionary<RdprfxNegativeType, bool>();

            foreach (RdprfxNegativeType negType in negTestTypeArr)
            {
                bool fDisconnected = rdprfxNegativeTest(negType);
                testResultDic.Add(negType, fDisconnected);
            }

            logNegativeTestResult(testResultDic);
        }

        #region private methods

        private bool rdprfxNegativeTest(RdprfxNegativeType negType)
        {
            StartRDPConnection();

            this.rdprfxAdapter.SetTestType(negType);

            #region Fill parameters
            TS_RFX_ICAP[] clientSupportedCaps;
            rdprfxAdapter.ReceiveAndCheckClientCapabilities(maxRequestSize, out clientSupportedCaps);

            OperationalMode opMode = OperationalMode.ImageMode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR3;
            ushort destLeft = 0; //the left bound of the frame.
            ushort destTop = 0; //the top bound of the frame.

            //Set OperationalMode/EntropyAlgorithm to valid pair.
            if (clientSupportedCaps != null)
            {
                opMode = (OperationalMode)clientSupportedCaps[0].flags;
                enAlgorithm = (EntropyAlgorithm)clientSupportedCaps[0].entropyBits;
            }
            #endregion

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending one frame of encoded bitmap data to client. Operational Mode = {0}, Entropy Algorithm = {1}, destLeft = {2}, destTop = {3}.",
            opMode, enAlgorithm, destLeft, destTop);
            this.rdprfxAdapter.SendImageToClient(image_64X64, opMode, enAlgorithm, destLeft, destTop);

            bool fDisconnected = this.rdpbcgrAdapter.WaitForDisconnection(waitTime);

            StopRDPConnection();

            return fDisconnected;
        }

        private void logNegativeTestResult(Dictionary<RdprfxNegativeType, bool> testResultDic)
        {
            bool fPass = true;
            foreach (RdprfxNegativeType negType in testResultDic.Keys)
            {
                fPass = fPass && testResultDic[negType];
                TestSite.Log.Add(LogEntryKind.Comment, "The test result with negative type of {0}: {1}.", negType, (testResultDic[negType]) ? "Passed" : "Failed");
            }
            TestSite.Assert.IsTrue(fPass, "RDP client is expected to terminate the RDP connection when receiving an invalid message.");
        } 

        #endregion
    }
}
