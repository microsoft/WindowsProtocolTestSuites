// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;
using Microsoft.Protocols.TestTools;
using System.Text.RegularExpressions;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestSuites.Rdpbcgr;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using System.Runtime.CompilerServices;

namespace Microsoft.Protocols.TestSuites.Rdp
{
    public abstract class RdpTestClassBase : TestClassBase
    {
        const int TileSize = 0x40; //The remoteFX tile size.
        const int VideoMode_TileRowNum = 5; //The row number of tiles to be sent to client.
        const int VideoMode_TileColNum = 5; //The column number of tiles to be sent to client.
        const string RDPVersionPattern = "RDP\\d+\\.\\d+"; // Used to match the RDP version in TestCategory. It MUST follow this format.

        #region Adapter Instances
        protected IRdpbcgrAdapter rdpbcgrAdapter;
        protected IRdpSutControlAdapter sutControlAdapter;
        #endregion

        #region Variables

        protected EncryptedProtocol transportProtocol;
        protected selectedProtocols_Values selectedProtocol;
        protected EncryptionMethods enMethod;
        protected EncryptionLevel enLevel;
        protected TS_UD_SC_CORE_version_Values rdpServerVersion;
        protected TimeSpan waitTime = new TimeSpan(0, 0, 40);
        protected TimeSpan shortWaitTime = new TimeSpan(0, 0, 5);
        protected bool isClientSupportFastPathInput = true;
        protected bool isClientSuportAutoReconnect = true;
        protected bool isClientSupportRDPEFS = true;
        protected bool isClientSupportServerRedirection = true;
        protected bool isClientSupportSoftSync = true;
        protected bool isClientSupportTunnelingStaticVCTraffic = true;
        protected bool isClientSupportEmptyRdpNegData;
        protected static Image image_64X64; //Defined to static for reuse across test cases
        protected static Image imageForVideoMode;
        protected uint maxRequestSize = 0x50002A; //The MaxReqestSize field of  Multifragment Update Capability Set. Just for test.
        protected bool isWindowsImplementation = true;
        protected bool DropConnectionForInvalidRequest = true;
        protected bool bVerifyRdpbcgrMessage;
        protected bool isclientSupportPersistentBitmapCache = false;
        protected ushort payloadLength = 15992; //payload length for RDP_BW_PAYLOAD and RDP_BW_STOP
        protected int payloadNum = 10;//How many RDP_BW_PAYLOAD will be sent

        // Variable for SUT display verification
        protected bool verifySUTDisplay = false;
        protected Point sutDisplayShift;
        protected string bitmapSavePath;
        protected int imageId = 0;
        protected string tmpFilePath = @".\tmpBitmap.bmp";
        #endregion

        #region Class Initialization and Cleanup
        public static void BaseInitialize(TestContext context)
        {
            TestClassBase.Initialize(context);
            BaseTestSite.DefaultProtocolDocShortName = BaseTestSite.Properties["ProtocolName"];
        }

        public static void BaseCleanup()
        {
            TestClassBase.Cleanup();
            if (image_64X64 != null)
            {
                image_64X64.Dispose();
            }
            if (imageForVideoMode != null)
            {
                imageForVideoMode.Dispose();
            }
        }
        #endregion

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            // Verify RDP version
            string testName = this.TestContext.TestName;
            MethodInfo method = GetType().GetMethod(testName);
            Attribute[] attrs = Attribute.GetCustomAttributes(method);
            string rdpVersion = GetSupportedRDPVersion(attrs);
            VerifyRDPVersion(testName, rdpVersion);

            this.rdpbcgrAdapter = (IRdpbcgrAdapter)this.TestSite.GetAdapter(typeof(IRdpbcgrAdapter));
            this.sutControlAdapter = (IRdpSutControlAdapter)this.TestSite.GetAdapter(typeof(IRdpSutControlAdapter));
            this.rdpbcgrAdapter.Reset();
            LoadConfig();

            this.rdpbcgrAdapter.ConfirmActiveRequest += new ConfirmActiveRequestHandler(this.testClassBase_getConfirmActivePduInfo);

            if (isWindowsImplementation)
            {
                string RDPClientVersion = this.Site.Properties["RDP.Version"].ToString();
                if (string.CompareOrdinal(RDPClientVersion, "10.3") == 0) // Windows client will not interrupt the connection for RDPClient 10.3.
                {
                    DropConnectionForInvalidRequest = true; //A switch to avoid waiting till timeout. 
                }
                else
                {
                    DropConnectionForInvalidRequest = false; //A switch to avoid waiting till timeout. 
                }
            }
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }
        #endregion

        #region Private Methods

        protected ITestSite TestSite
        {
            get
            {
                return BaseTestSite;
            }
        }

        /// <summary>
        /// Verify the version of operation system or MSTSC
        /// </summary>
        /// <param name="testCaseName">Test case name</param>
        /// <param name="version">RDP version</param>
        private void VerifyRDPVersion(String testCaseName, String version)
        {
            try
            {
                string currentRDPVersion = this.Site.Properties["RDP.Version"];

                //Validate the format of RDP.version in ptfconfig. The format of the version is required to be x.x
                Version currentRDPVer = null;
                if (!Version.TryParse(currentRDPVersion, out currentRDPVer))
                {
                    this.Site.Assert.Fail("Invalid format of RDP.Version {0} in config file. The valid format is required to be x.x. Please check the ptf config file.", currentRDPVersion);
                }

                //Validate the RDP version required by the test case against the RDP.version in ptfconfig
                Version testcaseRdpVer = null;
                if (Version.TryParse(version, out testcaseRdpVer))
                {
                    if (testcaseRdpVer > currentRDPVer)
                    {
                        this.Site.Assert.Inconclusive("Test case {0} is only supported by RDP {1} or above. But current RDP version is set to {2}", testCaseName, version, currentRDPVersion);
                    }
                }
                else
                {
                    this.Site.Assert.Fail("Invalid format of test case category RDP version {0}. The valid format is required to be RDPx.x.", version, testCaseName);
                }
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException || ex is FormatException || ex is OverflowException)
                {
                    this.Site.Log.Add(LogEntryKind.Comment, ex.Message);
                    this.Site.Assert.Fail("RDP.Version in PTF config, or the RDP version in TestCategory attribute is not configured correctly.");
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Find the RDP version from the attributes of test case
        /// </summary>
        /// <param name="attributes">Attributes of test case</param>
        /// <returns>RDP version</returns>
        private string GetSupportedRDPVersion(Attribute[] attributes)
        {
            foreach (var attr in attributes)
            {
                if (attr is TestCategoryAttribute)
                {
                    string value = ((TestCategoryAttribute)attr).TestCategories[0];
                    Regex pattern = new Regex(RDPVersionPattern);
                    if (pattern.IsMatch(value))
                    {
                        return value.Replace("RDP", "");
                    }
                }
            }
            return null;
        }

        protected void triggerClientRDPConnect(EncryptedProtocol enProtocol, bool fullScreen = false)
        {
            int iResult = 0;
            string strMethod = null;
            switch (enProtocol)
            {
                // negotiation based approach
                case EncryptedProtocol.Rdp:
                case EncryptedProtocol.NegotiationCredSsp:
                case EncryptedProtocol.NegotiationTls:
                    {
                        if (fullScreen)
                        {
                            iResult = this.sutControlAdapter.RDPConnectWithNegotiationApproachFullScreen(this.TestContext.TestName);
                        }
                        else
                        {
                            iResult = this.sutControlAdapter.RDPConnectWithNegotiationApproach(this.TestContext.TestName);
                        }
                        strMethod = "RDPConnectWithNegotiationApproach";
                    }
                    break;

                // direct approach
                case EncryptedProtocol.DirectCredSsp:
                    {
                        if (fullScreen)
                            iResult = this.sutControlAdapter.RDPConnectWithDirectCredSSPFullScreen(this.TestContext.TestName);
                        else
                            iResult = this.sutControlAdapter.RDPConnectWithDirectCredSSP(this.TestContext.TestName);
                        strMethod = "RDPConnectWithDirectCredSSP";
                    }
                    break;
            }

            TestSite.Assert.IsTrue(strMethod != null, "Unknown encryption protocol: {0}!", enProtocol);
            TestSite.Assert.IsTrue(iResult >= 0, "SUT Control Adapter: {0} should be successful: {1}.", strMethod, iResult);
        }

        protected void LoadConfig()
        {
            #region Read and convert properties from PTFCONFIG file

            #region Security Approach and Protocol
            string strRDPSecurityProtocol;
            bool isNegotiationBased = true;
            if (!PtfPropUtility.GetBoolPtfProperty(TestSite, RdpPtfPropNames.RdpSecurityNegotiation, out isNegotiationBased))
            {
                assumeFailForInvalidPtfProp(RdpPtfPropNames.RdpSecurityNegotiation);
            }

            selectedProtocol = selectedProtocols_Values.PROTOCOL_RDP_FLAG;
            if (!PtfPropUtility.GetStringPtfProperty(TestSite, RdpPtfPropNames.RdpSecurityProtocol, out strRDPSecurityProtocol))
            {
                assumeFailForInvalidPtfProp(RdpPtfPropNames.RdpSecurityProtocol);
            }

            // Check the combination of RdpSecurityNegotiation and RdpSecurityProtocol
            if (strRDPSecurityProtocol.Equals("TLS", StringComparison.CurrentCultureIgnoreCase))
            {
                selectedProtocol = selectedProtocols_Values.PROTOCOL_SSL_FLAG;
                this.TestSite.Assume.IsTrue(
                        isNegotiationBased,
                        "When TLS is used as the security protocol, {0} is set to 'TLS', {1} must be true.",
                        RdpPtfPropNames.RdpSecurityProtocol,
                        RdpPtfPropNames.RdpSecurityNegotiation);
                transportProtocol = EncryptedProtocol.NegotiationTls;
            }
            else if (strRDPSecurityProtocol.Equals("CredSSP", StringComparison.CurrentCultureIgnoreCase))
            {
                selectedProtocol = selectedProtocols_Values.PROTOCOL_HYBRID_FLAG;
                if (isNegotiationBased)
                {
                    transportProtocol = EncryptedProtocol.NegotiationCredSsp;
                }
                else
                {
                    transportProtocol = EncryptedProtocol.DirectCredSsp;
                }
            }
            else if (strRDPSecurityProtocol.Equals("RDP", StringComparison.CurrentCultureIgnoreCase))
            {
                selectedProtocol = selectedProtocols_Values.PROTOCOL_RDP_FLAG;
                transportProtocol = EncryptedProtocol.Rdp;
            }
            else
            {
                assumeFailForInvalidPtfProp(RdpPtfPropNames.RdpSecurityProtocol);
            }
            #endregion

            #region Encryption Level
            string strRDPSecurityEncryptionLevel;
            enLevel = EncryptionLevel.ENCRYPTION_LEVEL_LOW;
            if (!PtfPropUtility.GetStringPtfProperty(TestSite, RdpPtfPropNames.RdpSecurityEncryptionLevel, out strRDPSecurityEncryptionLevel))
            {
                assumeFailForInvalidPtfProp(RdpPtfPropNames.RdpSecurityEncryptionLevel);
            }
            else
            {//None, Low, Client, High, FIPS
                if (strRDPSecurityEncryptionLevel.Equals("None", StringComparison.CurrentCultureIgnoreCase))
                {
                    enLevel = EncryptionLevel.ENCRYPTION_LEVEL_NONE;
                }
                else if (strRDPSecurityEncryptionLevel.Equals("Low", StringComparison.CurrentCultureIgnoreCase))
                {
                    enLevel = EncryptionLevel.ENCRYPTION_LEVEL_LOW;
                }
                else if (strRDPSecurityEncryptionLevel.Equals("Client", StringComparison.CurrentCultureIgnoreCase))
                {
                    enLevel = EncryptionLevel.ENCRYPTION_LEVEL_CLIENT_COMPATIBLE;
                }
                else if (strRDPSecurityEncryptionLevel.Equals("High", StringComparison.CurrentCultureIgnoreCase))
                {
                    enLevel = EncryptionLevel.ENCRYPTION_LEVEL_HIGH;
                }
                else if (strRDPSecurityEncryptionLevel.Equals("FIPS", StringComparison.CurrentCultureIgnoreCase))
                {
                    enLevel = EncryptionLevel.ENCRYPTION_LEVEL_FIPS;
                }
                else
                {
                    assumeFailForInvalidPtfProp(RdpPtfPropNames.RdpSecurityEncryptionLevel);
                }

            }

            if (transportProtocol == EncryptedProtocol.Rdp && enLevel == EncryptionLevel.ENCRYPTION_LEVEL_NONE)
            {
                this.TestSite.Assert.Fail("When use Standard RDP Security, the encryption level must be greater than None.");
            }

            if (transportProtocol != EncryptedProtocol.Rdp && enLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE)
            {
                this.TestSite.Assert.Fail("When use enhanced security protocls (TLS or CredSSP), the encryption level MUST be None.");
            }

            #endregion

            #region Encryption Method
            string strRDPSecurityEncryptionMethod;
            enMethod = EncryptionMethods.ENCRYPTION_METHOD_128BIT;
            if (!PtfPropUtility.GetStringPtfProperty(TestSite, RdpPtfPropNames.RdpSecurityEncryptionMethod, out strRDPSecurityEncryptionMethod))
            {
                assumeFailForInvalidPtfProp(RdpPtfPropNames.RdpSecurityEncryptionMethod);
            }
            else
            {//None, 40bit, 56bit, 128bit, FIPS
                if (strRDPSecurityEncryptionMethod.Equals("None", StringComparison.CurrentCultureIgnoreCase))
                {
                    enMethod = EncryptionMethods.ENCRYPTION_METHOD_NONE;
                }
                else if (strRDPSecurityEncryptionMethod.Equals("40bit", StringComparison.CurrentCultureIgnoreCase))
                {
                    enMethod = EncryptionMethods.ENCRYPTION_METHOD_40BIT;
                }
                else if (strRDPSecurityEncryptionMethod.Equals("56bit", StringComparison.CurrentCultureIgnoreCase))
                {
                    enMethod = EncryptionMethods.ENCRYPTION_METHOD_56BIT;
                }
                else if (strRDPSecurityEncryptionMethod.Equals("128bit", StringComparison.CurrentCultureIgnoreCase))
                {
                    enMethod = EncryptionMethods.ENCRYPTION_METHOD_128BIT;
                }
                else if (strRDPSecurityEncryptionMethod.Equals("FIPS", StringComparison.CurrentCultureIgnoreCase))
                {
                    enMethod = EncryptionMethods.ENCRYPTION_METHOD_FIPS;
                }
                else
                {
                    assumeFailForInvalidPtfProp(RdpPtfPropNames.RdpSecurityEncryptionMethod);
                }
            }

            if (enLevel == EncryptionLevel.ENCRYPTION_LEVEL_NONE && enMethod != EncryptionMethods.ENCRYPTION_METHOD_NONE)
            {
                this.TestSite.Assume.Fail("When Encryption Level is set to None, the Encryption Method should also set to None.");
            }
            if (enLevel == EncryptionLevel.ENCRYPTION_LEVEL_FIPS && enMethod != EncryptionMethods.ENCRYPTION_METHOD_FIPS)
            {
                this.TestSite.Assume.Fail("When Encryption Level is set to FIPS, the Encryption Method should also set to FIPS.");
            }
            #endregion

            #region RDP Version
            rdpServerVersion = TS_UD_SC_CORE_version_Values.V2;

            #endregion

            #region WaitTime
            int waitSeconds;
            if (!PtfPropUtility.GetIntPtfProperty(TestSite, RdpPtfPropNames.Timeout, out waitSeconds))
            {
                assumeFailForInvalidPtfProp(RdpPtfPropNames.Timeout);
            }
            else
            {
                waitTime = new TimeSpan(0, 0, waitSeconds);
            }

            #endregion

            #region SUT Display Verification

            if (!PtfPropUtility.GetBoolPtfProperty(TestSite, "VerifySUTDisplay.Enable", out verifySUTDisplay))
            {
                verifySUTDisplay = false;
            }

            int shiftX, shiftY;
            if (!PtfPropUtility.GetIntPtfProperty(TestSite, "VerifySUTDisplay.Shift.X", out shiftX))
            {
                shiftX = 0;
            }

            if (!PtfPropUtility.GetIntPtfProperty(TestSite, "VerifySUTDisplay.Shift.Y", out shiftY))
            {
                shiftY = 0;
            }

            sutDisplayShift = new Point(shiftX, shiftY);

            if (!PtfPropUtility.GetStringPtfProperty(TestSite, "VerifySUTDisplay.BitmapSavePath", out bitmapSavePath))
            {
                bitmapSavePath = @".\";
            }

            // If the bitmap save path is not existed, create it.
            if (!Directory.Exists(bitmapSavePath))
            {
                Directory.CreateDirectory(bitmapSavePath);
            }

            #endregion SUT Display Verification

            #region Other configrations

            if (!PtfPropUtility.GetBoolPtfProperty(TestSite, RdpPtfPropNames.RDPClientSupportFastPathInput, out isClientSupportFastPathInput))
            {
                isClientSupportFastPathInput = false; //if property not found, set to false as default value
            }

            if (!PtfPropUtility.GetBoolPtfProperty(TestSite, RdpPtfPropNames.RDPClientSupportAutoReconnect, out isClientSuportAutoReconnect))
            {
                isClientSuportAutoReconnect = false; //if property not found, set to false as default value
            }

            if (!PtfPropUtility.GetBoolPtfProperty(TestSite, RdpPtfPropNames.RDPClientSupportRDPEFS, out isClientSupportRDPEFS))
            {
                isClientSupportRDPEFS = false; //if property not found, set to false as default value
            }

            if (!PtfPropUtility.GetBoolPtfProperty(TestSite, RdpPtfPropNames.RDPClientSupportServerRedirection, out isClientSupportServerRedirection))
            {
                isClientSupportServerRedirection = false; //if property not found, set to false as default value
            }

            if (!PtfPropUtility.GetBoolPtfProperty(TestSite, RdpPtfPropNames.RDPClientSupportSoftSync, out isClientSupportSoftSync))
            {
                isClientSupportSoftSync = false; //if property not found, set to false as default value
            }

            if (!PtfPropUtility.GetBoolPtfProperty(TestSite, RdpPtfPropNames.RDPClientSupportTunnelingStaticVCTraffic, out isClientSupportTunnelingStaticVCTraffic))
            {
                isClientSupportTunnelingStaticVCTraffic = false; //if property not found, set to false as default value
            }

            if (!PtfPropUtility.GetBoolPtfProperty(TestSite, RdpPtfPropNames.RDPClientSupportRdpNegDataEmpty, out isClientSupportEmptyRdpNegData))
            {
                isClientSupportEmptyRdpNegData = false; //if property not found, set to false as default value
            }

            if (!PtfPropUtility.GetBoolPtfProperty(TestSite, RdpPtfPropNames.IsWindowsImplementation, out isWindowsImplementation))
            {
                isWindowsImplementation = true; //if property not found, set to true as default value
            }

            if (!PtfPropUtility.GetBoolPtfProperty(TestSite, RdpPtfPropNames.DropConnectionForInvalidRequest, out DropConnectionForInvalidRequest))
            {
                DropConnectionForInvalidRequest = true; //if property not found, set to true as default value
            }

            if (!PtfPropUtility.GetBoolPtfProperty(TestSite, "VerifyRdpbcgrMessage", out bVerifyRdpbcgrMessage))
            {
                bVerifyRdpbcgrMessage = true; //if property not found, set to true as default value
            }

            String rdprfxImageFile;
            if (!PtfPropUtility.GetStringPtfProperty(TestSite, "RDPRFX.Image", out rdprfxImageFile))
            {
                rdprfxImageFile = ""; //if property not found, set to true as default value
            }

            String rdprfxVideoModeImageFile;
            if (!PtfPropUtility.GetStringPtfProperty(TestSite, "RDPRFXVideoMode.Image", out rdprfxVideoModeImageFile))
            {
                rdprfxVideoModeImageFile = ""; //if property not found, set to true as default value
            }

            try
            {
                //Get image from file
                image_64X64 = Image.FromFile(rdprfxImageFile);
                imageForVideoMode = Image.FromFile(rdprfxVideoModeImageFile);
            }
            catch (System.IO.FileNotFoundException)
            {
                //capture screen if failed to get image from file
                //Capture 64*64 bitmap for Image Mode
                image_64X64 = captureScreenImage(0, 0, TileSize, TileSize);

                //Capture screen bitmap for Vedio Mode
                imageForVideoMode = captureScreenImage(0, 0, TileSize * VideoMode_TileRowNum, TileSize * VideoMode_TileColNum);
            }
            #endregion

            #endregion

            #region Logging
            this.TestSite.Log.Add(LogEntryKind.Debug,
                @"isClientSupportFastPathInput = {0};
                isClientSuportAutoReconnect = {1};
                isClientSupportRDPEFS = {2};
                isClientSupportServerRedirection = {3};
                isClientSupportEmptyRdpNegData = {4};
                isClientSupportSoftSync = {5}
                isClientSupportTunnelingStaticVCTraffic = {6}",
                isClientSupportFastPathInput,
                isClientSuportAutoReconnect,
                isClientSupportRDPEFS,
                isClientSupportServerRedirection,
                isClientSupportEmptyRdpNegData,
                isClientSupportSoftSync,
                isClientSupportTunnelingStaticVCTraffic);
            #endregion
        }

        /// <summary>
        /// Verify SUT Dispaly
        /// </summary>
        /// <param name="usingRemoteFX">Whether the output image is using RemoteFX codec</param>
        /// <param name="compareRect">The Rectangle on the image to be compared</param>
        /// <param name="callStackIndex">Call stack index from the test method</param>
        protected void VerifySUTDisplay(bool usingRemoteFX, Rectangle compareRect, int callStackIndex = 1)
        {
            if (!verifySUTDisplay || this.rdpbcgrAdapter.SimulatedScreen == null)
            {
                return;
            }

            imageId++;

            // Get test method name and construct file name for screenshot bitmap
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(callStackIndex);
            string pathForSUTScreenShot = bitmapSavePath + @"\" + sf.GetMethod().Name + "_" + imageId + "_ScreenShot.bmp";
            // Not save the bitmap to the path directly since the interface has limit on the length of path.
            // Save the bitmap into a temprory file and copy it to the right file path.
            int result = this.sutControlAdapter.CaptureScreenShot(this.TestContext.TestName, tmpFilePath);
            File.Copy(tmpFilePath, pathForSUTScreenShot, true);
            this.TestSite.Assume.IsTrue(result >= 0, "To verify output of RDP client, the protocol-based SUT control adapter should be used and the Agent on SUT should support screenshot control command.");

            // print the absolute path of screen shot file
            var fileInfoScreenShot = new FileInfo(pathForSUTScreenShot);
            this.TestSite.Log.Add(LogEntryKind.Debug, "The absolute path of screen shot file is {0}.", fileInfoScreenShot.FullName);

            // Compare the screenshot with base image in simulated screen
            Bitmap image = new Bitmap(pathForSUTScreenShot);
            string pathForBaseImage = bitmapSavePath + @"\" + sf.GetMethod().Name + "_" + imageId + "_BaseImage.bmp";
            this.rdpbcgrAdapter.SimulatedScreen.BaseImage.Save(tmpFilePath);
            File.Copy(tmpFilePath, pathForBaseImage, true);

            // print the absolute path of base image file
            var fileInfoBaseImage = new FileInfo(pathForBaseImage);
            this.TestSite.Log.Add(LogEntryKind.Debug, "The absolute path of base image file is {0}.", fileInfoBaseImage.FullName);

            bool compareRes = this.rdpbcgrAdapter.SimulatedScreen.Compare(image, sutDisplayShift, compareRect, usingRemoteFX);
            this.TestSite.Assert.IsTrue(compareRes, "SUT display verification should success, the output on RDP client should be equal (or similar enough if using RemoteFX codec) as expected.");
        }
        
        /// <summary>
        /// End the test case with inconclusive if it's Windows implementation and using RDP security protocol
        /// Used in test cases which need to establish TLS/DTLS on RDP-UDP connection.
        /// Included in base class since it will be used in test cases of RDPEUDP, RDPEMT and RDPEVOR
        /// </summary>
        protected void CheckSecurityProtocolForMultitransport()
        {
            if (transportProtocol == EncryptedProtocol.Rdp && isWindowsImplementation)
            {
                Site.Assert.Inconclusive("Not Applicable, Microsoft RDP clients fail the TLS or DTLS handshake for a multitransport connection if Enhanced RDP Security is not in effect for the main RDP connection.");
            }
        }
        //override, assume fail for an invalid PTF property.
        private void assumeFailForInvalidPtfProp(string propName)
        {
            this.TestSite.Assume.Fail("The property \"{0}\" is invalid or not present in PTFConfig file!", propName);
        }

        //Capture image from screen
        private Bitmap captureScreenImage(int left, int top, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(left, top, 0, 0, new Size(width, height));
            g.Dispose();
            return bitmap;
        }

        //Get capability information from Client_Confirm_Active_Pdu, 
        //will be added to ConfirmActiveRequest event
        private void testClassBase_getConfirmActivePduInfo(Client_Confirm_Active_Pdu confirmActivePdu)
        {
            foreach (ITsCapsSet cap in confirmActivePdu.confirmActivePduData.capabilitySets)
            {
                if (cap.GetCapabilityType() == capabilitySetType_Values.CAPSTYPE_BITMAPCACHE_REV2)
                {
                    TS_BITMAPCACHE_CAPABILITYSET_REV2 bitmapCacheV2 = (TS_BITMAPCACHE_CAPABILITYSET_REV2)cap;
                    if ((bitmapCacheV2.CacheFlags & CacheFlags_Values.PERSISTENT_KEYS_EXPECTED_FLAG) != 0)
                    {
                        isclientSupportPersistentBitmapCache = true;
                    }

                }
            }
        }
                
        #endregion
        /// <summary>
        /// Provide a generic method to handle the invalid request from RDP server
        /// For Windows, it drops the rdp connection directly.
        /// For non-Windows, it may ignore the invalid request or deny the request.
        /// </summary>
        /// <param name="requestDesc">The description about the invalid request for logging output</param>
        public void RDPClientTryDropConnection(string requestDesc)
        {
            if (DropConnectionForInvalidRequest)
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "Expect RDP client to drop the connection");
                bool bDisconnected = this.rdpbcgrAdapter.WaitForDisconnection(waitTime);
                if (!bDisconnected)
                {
                    this.TestSite.Assert.IsTrue(bDisconnected, "RDP client should terminate the connection when invalid " + requestDesc + " received.");
                }
            }
            else
            {
                this.TestSite.Log.Add(LogEntryKind.Warning, "Non-Windows RDP client did not terminate the connection when invalid " + requestDesc + " received.");
                this.TestSite.Log.Add(LogEntryKind.Comment, "Please double check the RDP client behavior is as expected.");
            }
        }
    }
}
