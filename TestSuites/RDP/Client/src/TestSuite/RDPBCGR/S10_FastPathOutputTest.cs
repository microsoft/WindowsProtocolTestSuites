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
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to verify RDP client can hide the pointer correctly when receiving a Fast-Path System Pointer Hidden Update message.")]
        public void S10_FastPathOutput_PositiveTest_PointerHidden()
        {
            #region Test Description
            /*
            This test case is used to verify RDP client can hide the pointer correctly when receiving a Fast-Path System Pointer Hidden Update message. 
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection. 
            2.	After the connection sequence has been finished, Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on. 
            3.	Test Suite sends a Fast-Path Color Pointer Update (TS_FP_COLORPOINTERATTRIBUTE) to the client to set pointer shape.
            4.	Test Suite sends a Fast-Path Pointer Position Update (TS_FP_POINTERPOSATTRIBUTE) to the client to set pointer position.
            5.  Test Suite sends a Fast-Path System Pointer Hidden Update (TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE) to the client to hide the pointer.
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

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            this.Site.Assume.IsTrue(this.rdpbcgrAdapter.SessionContext.IsFastPathOutputSupported, "Capability check failed, this test case need RDP client to support fast-path output.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_COLORPOINTERATTRIBUTE to the client.");
            this.rdpbcgrAdapter.FPColorPointer(0, PointerWidth / 2, PointerHeight / 2, PointerWidth, PointerHeight);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERPOSATTRIBUTE to the client to set pointer to ({0}, {1}).", PointerPos.xPos, PointerPos.yPos);
            this.rdpbcgrAdapter.FPPointerPosition(PointerPos.xPos, PointerPos.yPos);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE to the client to hide the pointer.");
            this.rdpbcgrAdapter.FPSystemPointerHidden();

            #endregion
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to verify RDP client can move the pointer correctly when receiving Fast-Path Pointer Position Update messages.")]
        public void S10_FastPathOutput_PositiveTest_PointerPosUpdate_PointerMove()
        {
            #region Test Description
            /* 
            This test case is used to verify RDP client can move the pointer correctly when receiving Fast-Path Pointer Position Update messages.
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection. 
            2.	After the connection sequence has been finished, Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on. 
            3.	Test Suite sends a Fast-Path Color Pointer Update (TS_FP_COLORPOINTERATTRIBUTE) to the client to set pointer shape.
            4.	Test Suite sends several Fast-Path Pointer Position Update (TS_FP_POINTERPOSATTRIBUTE) to the client to move the pointer.
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

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            this.Site.Assume.IsTrue(this.rdpbcgrAdapter.SessionContext.IsFastPathOutputSupported, "Capability check failed, this test case need RDP client to support fast-path output.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_COLORPOINTERATTRIBUTE to the client.");
            this.rdpbcgrAdapter.FPColorPointer(0, PointerWidth / 2, PointerHeight / 2, PointerWidth, PointerHeight);

            int desktopWidth = this.rdpbcgrAdapter.SessionContext.DesktopWidth;
            for (int x = PointerPos.xPos; x < desktopWidth; x += PointerMoveStep)
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERPOSATTRIBUTE to the client to set pointer to ({0}, {1}).", x, PointerPos.yPos);
                this.rdpbcgrAdapter.FPPointerPosition(x, PointerPos.yPos);
            }            

            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to verify RDP client can locate the pointer correctly when receiving Fast-Path Pointer Position Update messages with pointer positions on the border of screen.")]
        public void S10_FastPathOutput_PositiveTest_PointerPosUpdate_PointerOnTheBorderOfScreen()
        {
            #region Test Description
            /*
            This test case is used to verify RDP client can locate the pointer correctly when receiving Fast-Path Pointer Position Update messages with pointer positions on the border of screen. 
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection. 
            2.	After the connection sequence has been finished, Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on. 
            3.	Test Suite sends a Fast-Path Color Pointer Update (TS_FP_COLORPOINTERATTRIBUTE) to the client to set pointer shape.
            4.	Test Suite sends a Fast-Path Pointer Position Update (TS_FP_POINTERPOSATTRIBUTE) to the client to set pointer position to (0, 0).
            5.  Test Suite sends a Fast-Path Pointer Position Update (TS_FP_POINTERPOSATTRIBUTE) to the client to set pointer position to (desktopWidth - 1, 0).
            6.  Test Suite sends a Fast-Path Pointer Position Update (TS_FP_POINTERPOSATTRIBUTE) to the client to set pointer position to (0, desktopHeight - 1).
            7.  Test Suite sends a Fast-Path Pointer Position Update (TS_FP_POINTERPOSATTRIBUTE) to the client to set pointer position to (desktopWidth - 1, desktopHeight - 1).
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

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            this.Site.Assume.IsTrue(this.rdpbcgrAdapter.SessionContext.IsFastPathOutputSupported, "Capability check failed, this test case need RDP client to support fast-path output.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_COLORPOINTERATTRIBUTE to the client.");
            this.rdpbcgrAdapter.FPColorPointer(0, PointerWidth / 2, PointerHeight / 2, PointerWidth, PointerHeight);


            int desktopWidth = this.rdpbcgrAdapter.SessionContext.DesktopWidth;
            int desktopHeight = this.rdpbcgrAdapter.SessionContext.DesktopHeight;

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERPOSATTRIBUTE to the client to set pointer to the border of screen ({0}, {1}).", 0, 0);
            this.rdpbcgrAdapter.FPPointerPosition(0, 0);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERPOSATTRIBUTE to the client to set pointer to the border of screen ({0}, {1}).", desktopWidth - 1, 0);
            this.rdpbcgrAdapter.FPPointerPosition(desktopWidth - 1, 0);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERPOSATTRIBUTE to the client to set pointer to the border of screen ({0}, {1}).", 0, desktopHeight - 1);
            this.rdpbcgrAdapter.FPPointerPosition(0, desktopHeight - 1);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERPOSATTRIBUTE to the client to set pointer to the border of screen ({0}, {1}).", desktopWidth - 1, desktopHeight - 1);
            this.rdpbcgrAdapter.FPPointerPosition(desktopWidth - 1, desktopHeight - 1);

            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to verify RDP client can show the pointer correctly when receiving a Fast-Path Color Pointer Update message.")]
        public void S10_FastPathOutput_PositiveTest_ColorPointer_InitPointer()
        {
            #region Test Description
            /*
            This test case is used to verify RDP client can show the pointer correctly when receiving a Fast-Path Color Pointer Update message.
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection. 
            2.	After the connection sequence has been finished, Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on. 
            3.	Test Suite sends a Fast-Path Color Pointer Update (TS_FP_COLORPOINTERATTRIBUTE) to the client to set pointer shape.
            4.	Test Suite sends a Fast-Path Pointer Position Update (TS_FP_POINTERPOSATTRIBUTE) to the client to show the pointer on a specific position.
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

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            this.Site.Assume.IsTrue(this.rdpbcgrAdapter.SessionContext.IsFastPathOutputSupported, "Capability check failed, this test case need RDP client to support fast-path output.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_COLORPOINTERATTRIBUTE to the client.");
            this.rdpbcgrAdapter.FPColorPointer(0, PointerWidth / 2, PointerHeight / 2, PointerWidth, PointerHeight);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERPOSATTRIBUTE to the client to set pointer to ({0}, {1}).", PointerPos.xPos, PointerPos.yPos);
            this.rdpbcgrAdapter.FPPointerPosition(PointerPos.xPos, PointerPos.yPos);

            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to verify RDP client can update the pointer correctly when receiving multiple Fast-Path Color Pointer Update message.")]
        public void S10_FastPathOutput_PositiveTest_ColorPointer_UpdatePointer()
        {
            #region Test Description
            /*
            This test case is used to verify RDP client can update the pointer correctly when receiving multiple Fast-Path Color Pointer Update message. 
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection. 
            2.	After the connection sequence has been finished, Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on. 
            3.	Test Suite sends a Fast-Path Color Pointer Update (TS_FP_COLORPOINTERATTRIBUTE) to the client to set pointer shape.
            4.	Test Suite sends a Fast-Path Pointer Position Update (TS_FP_POINTERPOSATTRIBUTE) to the client to show the pointer on a specific position.
            5.  Test Suite sends another Fast-Path Color Pointer Update (TS_FP_COLORPOINTERATTRIBUTE) to the client to update pointer shape.
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

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            this.Site.Assume.IsTrue(this.rdpbcgrAdapter.SessionContext.IsFastPathOutputSupported, "Capability check failed, this test case need RDP client to support fast-path output.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_COLORPOINTERATTRIBUTE to the client.");
            this.rdpbcgrAdapter.FPColorPointer(0, PointerWidth / 2, PointerHeight / 2, PointerWidth, PointerHeight);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERPOSATTRIBUTE to the client to set pointer to ({0}, {1}).", PointerPos.xPos, PointerPos.yPos);
            this.rdpbcgrAdapter.FPPointerPosition(PointerPos.xPos, PointerPos.yPos);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_COLORPOINTERATTRIBUTE to the client.");
            this.rdpbcgrAdapter.FPColorPointer(1, PointerWidthForUpdate / 2, PointerHeightForUpdate / 2, PointerWidthForUpdate, PointerHeightForUpdate);

            #endregion
        }
        
        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to verify RDP client can show the pointer correctly when receiving a Fast-Path Color Pointer Update message, the pointer's width and height are set to maximum allowed value when large pointer is not supported.")]
        public void S10_FastPathOutput_PositiveTest_ColorPointer_MaxAllowedWidthHeightLargePointerNotSupported()
        {
            #region Test Description
            /*
            This test case is used to verify RDP client can show the pointer correctly when receiving a Fast-Path Color Pointer Update message, the pointer's width and height are set to maximum allowed value when large pointer is not supported. 
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection. In Capability Exchange phase, Test Suite doesn't send Large Pointer Capability Set.
            2.	After the connection sequence has been finished, Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on. 
            3.	Test Suite sends a Fast-Path Color Pointer Update (TS_FP_COLORPOINTERATTRIBUTE) to the client to set pointer shape, set width and height to maximum allowed value 32x32
            4.	Test Suite sends a Fast-Path Pointer Position Update (TS_FP_POINTERPOSATTRIBUTE) to the client to show the pointer on a specific position.
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

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true,
               true, maxRequestSize,
               false, // set to not support large pointer
               true, 1,
               true, CmdFlags_Values.SURFCMDS_FRAMEMARKER | CmdFlags_Values.SURFCMDS_SETSURFACEBITS | CmdFlags_Values.SURFCMDS_STREAMSURFACEBITS,
               true, true, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            this.Site.Assume.IsTrue(this.rdpbcgrAdapter.SessionContext.IsFastPathOutputSupported, "Capability check failed, this test case need RDP client to support fast-path output.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_COLORPOINTERATTRIBUTE to the client.");
            this.rdpbcgrAdapter.FPColorPointer(0, MaxPointerWidth / 2, MaxPointerHeight / 2, MaxPointerWidth, MaxPointerHeight);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERPOSATTRIBUTE to the client to set pointer to ({0}, {1}).", PointerPos.xPos, PointerPos.yPos);
            this.rdpbcgrAdapter.FPPointerPosition(PointerPos.xPos, PointerPos.yPos);

            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to verify RDP client can show the pointer correctly when receiving a Fast-Path Color Pointer Update message, the cacheIndex is set to the maximum allowed value according to capability change.")]
        public void S10_FastPathOutput_PositiveTest_ColorPointer_MaxAllowedCacheIndex()
        {
            #region Test Description
            /*
            This test case is used to verify RDP client can show the pointer correctly when receiving a Fast-Path Color Pointer Update message, the cacheIndex is set to the maximum allowed value according to capability change. 
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection.
            2.	After the connection sequence has been finished, Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on. 
            3.	Test Suite sends a Fast-Path Color Pointer Update (TS_FP_COLORPOINTERATTRIBUTE) to the client to set pointer shape, the cacheIndex is set to the maximum allowed value according to capability change
            4.	Test Suite sends a Fast-Path Pointer Position Update (TS_FP_POINTERPOSATTRIBUTE) to the client to show the pointer on a specific position.
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

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            this.Site.Assume.IsTrue(this.rdpbcgrAdapter.SessionContext.IsFastPathOutputSupported, "Capability check failed, this test case need RDP client to support fast-path output.");

            this.Site.Assert.IsNotNull(this.rdpbcgrAdapter.SessionContext.ColorPointerCacheSize, "Not find capability information about color pointer cache size, this information should be in Pointer Capability Set (TS_POINTER_CAPABILITYSET).");
            int maxCacheIndex = this.rdpbcgrAdapter.SessionContext.ColorPointerCacheSize.Value - 1;

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_COLORPOINTERATTRIBUTE to the client, set cache index in max allowed value: " + maxCacheIndex);
            this.rdpbcgrAdapter.FPColorPointer((ushort)maxCacheIndex, PointerWidth / 2, PointerHeight / 2, PointerWidth, PointerHeight);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERPOSATTRIBUTE to the client to set pointer to ({0}, {1}).", PointerPos.xPos, PointerPos.yPos);
            this.rdpbcgrAdapter.FPPointerPosition(PointerPos.xPos, PointerPos.yPos);

            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to verify RDP client can show the pointer correctly when receiving a Fast-Path Color Pointer Update message with hotspot on the border of pointer.")]
        public void S10_FastPathOutput_PositiveTest_ColorPointer_HotSpotOnTheBorderOfPointer()
        {
            #region Test Description
            /*
            This test case is used to verify RDP client can show the pointer correctly when receiving a Fast-Path Color Pointer Update message with hotspot on the border of point.
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection.
            2.	After the connection sequence has been finished, Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on. 
            3.	Test Suite sends a Fast-Path Color Pointer Update (TS_FP_COLORPOINTERATTRIBUTE) to the client to set pointer shape, set the hotspot on the right-bottom border of the pointer (PointerWidth-1, PointerHeight-1).
            4.	Test Suite sends a Fast-Path Pointer Position Update (TS_FP_POINTERPOSATTRIBUTE) to the client to show the pointer on a specific position.
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

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            this.Site.Assume.IsTrue(this.rdpbcgrAdapter.SessionContext.IsFastPathOutputSupported, "Capability check failed, this test case need RDP client to support fast-path output.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_COLORPOINTERATTRIBUTE to the client, set hotspot to ({0}, {1})", PointerWidth - 1, PointerHeight - 1);
            this.rdpbcgrAdapter.FPColorPointer(0, PointerWidth - 1, PointerHeight - 1, PointerWidth, PointerHeight);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERPOSATTRIBUTE to the client to set pointer to ({0}, {1}).", PointerPos.xPos, PointerPos.yPos);
            this.rdpbcgrAdapter.FPPointerPosition(PointerPos.xPos, PointerPos.yPos);
            
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to verify RDP client can show the pointer correctly when receiving a Fast-Path Color Pointer Update message with xorMaskData with padded data (set pointer's width to an odd value).")]
        public void S10_FastPathOutput_PositiveTest_ColorPointer_OddWidth()
        {
            #region Test Description
            /*
            This test case is used to verify RDP client can show the pointer correctly when receiving a Fast-Path Color Pointer Update message with xorMaskData with padded data (set pointer's width to an odd value). 
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection.
            2.	After the connection sequence has been finished, Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on. 
            3.	Test Suite sends a Fast-Path Color Pointer Update (TS_FP_COLORPOINTERATTRIBUTE) to the client to set pointer shape, the xorMaskData contains padded data(set pointer's width to an odd value).
            4.	Test Suite sends a Fast-Path Pointer Position Update (TS_FP_POINTERPOSATTRIBUTE) to the client to show the pointer on a specific position.
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

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            this.Site.Assume.IsTrue(this.rdpbcgrAdapter.SessionContext.IsFastPathOutputSupported, "Capability check failed, this test case need RDP client to support fast-path output.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_COLORPOINTERATTRIBUTE to the client, pointer width is set to an odd value.");
            this.rdpbcgrAdapter.FPColorPointer(0, PointerOddWidth / 2, PointerHeight / 2, PointerOddWidth, PointerHeight);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERPOSATTRIBUTE to the client to set pointer to ({0}, {1}).", PointerPos.xPos, PointerPos.yPos);
            this.rdpbcgrAdapter.FPPointerPosition(PointerPos.xPos, PointerPos.yPos);

            #endregion
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to verify RDP client can show the pointer correctly when receiving a Fast-Path New Pointer Update message.")]
        public void S10_FastPathOutput_PositiveTest_NewPointer_InitPointer()
        {
            #region Test Description
            /*
            This test case is used to verify RDP client can show the pointer correctly when receiving a Fast-Path New Pointer Update message.
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection. 
            2.	After the connection sequence has been finished, Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on. 
            3.	Test Suite sends a Fast-Path New Pointer Update (TS_FP_POINTERATTRIBUTE) to the client to set pointer shape.
            4.	Test Suite sends a Fast-Path Pointer Position Update (TS_FP_POINTERPOSATTRIBUTE) to the client to show the pointer on a specific position.
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

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            this.Site.Assume.IsTrue(this.rdpbcgrAdapter.SessionContext.IsFastPathOutputSupported, "Capability check failed, this test case need RDP client to support fast-path output.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERATTRIBUTE to the client, set xorBpp to 24.");
            this.rdpbcgrAdapter.FPNewPointer(24, 0, PointerWidth / 2, PointerHeight / 2, PointerWidth, PointerHeight);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERPOSATTRIBUTE to the client to set pointer to ({0}, {1}).", PointerPos.xPos, PointerPos.yPos);
            this.rdpbcgrAdapter.FPPointerPosition(PointerPos.xPos, PointerPos.yPos);

            #endregion
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to verify RDP client can update the pointer correctly when receiving multiple Fast-Path New Pointer Update messages.")]
        public void S10_FastPathOutput_PositiveTest_NewPointer_UpdatePointer()
        {
            #region Test Description
            /*
            This test case is used to verify RDP client can update the pointer correctly when receiving multiple Fast-Path New Pointer Update messages. 
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection. 
            2.	After the connection sequence has been finished, Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on. 
            3.	Test Suite sends a Fast-Path New Pointer Update (TS_FP_POINTERATTRIBUTE) to the client to set pointer shape.
            4.	Test Suite sends a Fast-Path Pointer Position Update (TS_FP_POINTERPOSATTRIBUTE) to the client to show the pointer on a specific position.
            5.  Test Suite sends another Fast-Path New Pointer Update (TS_FP_POINTERATTRIBUTE) to the client to update pointer shape.
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

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            this.Site.Assume.IsTrue(this.rdpbcgrAdapter.SessionContext.IsFastPathOutputSupported, "Capability check failed, this test case need RDP client to support fast-path output.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERATTRIBUTE to the client, set xorBpp to 24.");
            this.rdpbcgrAdapter.FPNewPointer(24, 0, PointerWidth / 2, PointerHeight / 2, PointerWidth, PointerHeight);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERPOSATTRIBUTE to the client to set pointer to ({0}, {1}).", PointerPos.xPos, PointerPos.yPos);
            this.rdpbcgrAdapter.FPPointerPosition(PointerPos.xPos, PointerPos.yPos);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERATTRIBUTE to the client, set xorBpp to 24.");
            this.rdpbcgrAdapter.FPNewPointer(24, 1, PointerWidthForUpdate / 2, PointerHeightForUpdate / 2, PointerWidthForUpdate, PointerHeightForUpdate);

            #endregion
        }
           
        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to verify RDP client can show the pointer correctly when receiving a Fast-Path New Pointer Update message, the pointer's width and height are set to maximum allowed value when large pointer is not supported.")]
        public void S10_FastPathOutput_PositiveTest_NewPointer_MaxAllowedWidthHeightLargePointerNotSupported()
        {
            #region Test Description
            /*
            This test case is used to verify RDP client can show the pointer correctly when receiving a Fast-Path New Pointer Update message, the pointer's width and height are set to maximum allowed value when large pointer is not supported. 
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection. In Capability Exchange phase, Test Suite doesn't send Large Pointer Capability Set.
            2.	After the connection sequence has been finished, Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on. 
            3.	Test Suite sends a Fast-Path New Pointer Update (TS_FP_POINTERATTRIBUTE) to the client to set pointer shape, set width and height to maximum allowed value 32x32
            4.	Test Suite sends a Fast-Path Pointer Position Update (TS_FP_POINTERPOSATTRIBUTE) to the client to show the pointer on a specific position.
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

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true,
               true, maxRequestSize,
               false, // set to not support large pointer
               true, 1,
               true, CmdFlags_Values.SURFCMDS_FRAMEMARKER | CmdFlags_Values.SURFCMDS_SETSURFACEBITS | CmdFlags_Values.SURFCMDS_STREAMSURFACEBITS,
               true, true, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            this.Site.Assume.IsTrue(this.rdpbcgrAdapter.SessionContext.IsFastPathOutputSupported, "Capability check failed, this test case need RDP client to support fast-path output.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERATTRIBUTE to the client, set xorBpp to 24, set width and height to max allowed value when large pointer is not supported: {0}, {1}.", MaxPointerWidth, MaxPointerHeight);
            this.rdpbcgrAdapter.FPNewPointer(24, 0, MaxPointerWidth / 2, MaxPointerHeight / 2, MaxPointerWidth, MaxPointerHeight);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERPOSATTRIBUTE to the client to set pointer to ({0}, {1}).", PointerPos.xPos, PointerPos.yPos);
            this.rdpbcgrAdapter.FPPointerPosition(PointerPos.xPos, PointerPos.yPos);

            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to verify RDP client can show the pointer correctly when receiving a Fast-Path New Pointer Update message, the cacheIndex is set to the maximum allowed value according to capability change.")]
        public void S10_FastPathOutput_PositiveTest_NewPointer_MaxAllowedCacheIndex()
        {
            #region Test Description
            /*
            This test case is used to verify RDP client can show the pointer correctly when receiving a Fast-Path New Pointer Update message, the cacheIndex is set to the maximum allowed value according to capability change. 
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection.
            2.	After the connection sequence has been finished, Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on. 
            3.	Test Suite sends a Fast-Path New Pointer Update (TS_FP_POINTERATTRIBUTE) to the client to set pointer shape, the cacheIndex is set to the maximum allowed value according to capability change
            4.	Test Suite sends a Fast-Path Pointer Position Update (TS_FP_POINTERPOSATTRIBUTE) to the client to show the pointer on a specific position.
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

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            this.Site.Assume.IsTrue(this.rdpbcgrAdapter.SessionContext.IsFastPathOutputSupported, "Capability check failed, this test case need RDP client to support fast-path output.");

            this.Site.Assert.IsNotNull(this.rdpbcgrAdapter.SessionContext.PointerCacheSize, "Not find capability information about pointer cache size, this information should be in Pointer Capability Set (TS_POINTER_CAPABILITYSET).");
            int maxCacheIndex = this.rdpbcgrAdapter.SessionContext.PointerCacheSize.Value - 1;

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_COLORPOINTERATTRIBUTE to the client, set xorBpp to 24, set cache index in max allowed value: " + maxCacheIndex);
            this.rdpbcgrAdapter.FPNewPointer(24, (ushort)maxCacheIndex, PointerWidth / 2, PointerHeight / 2, PointerWidth, PointerHeight);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERPOSATTRIBUTE to the client to set pointer to ({0}, {1}).", PointerPos.xPos, PointerPos.yPos);
            this.rdpbcgrAdapter.FPPointerPosition(PointerPos.xPos, PointerPos.yPos);

            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to verify RDP client can show the pointer correctly when receiving a Fast-Path New Pointer Update message with hotspot on the border of pointer.")]
        public void S10_FastPathOutput_PositiveTest_NewPointer_HotSpotOnTheBorderOfPointer()
        {
            #region Test Description
            /*
            This test case is used to verify RDP client can show the pointer correctly when receiving a Fast-Path New Pointer Update message with hotspot on the border of point.
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection.
            2.	After the connection sequence has been finished, Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on. 
            3.	Test Suite sends a Fast-Path New Pointer Update (TS_FP_POINTERATTRIBUTE) to the client to set pointer shape, set the hotspot on the right-bottom border of the pointer (PointerWidth-1, PointerHeight-1).
            4.	Test Suite sends a Fast-Path Pointer Position Update (TS_FP_POINTERPOSATTRIBUTE) to the client to show the pointer on a specific position.
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

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            this.Site.Assume.IsTrue(this.rdpbcgrAdapter.SessionContext.IsFastPathOutputSupported, "Capability check failed, this test case need RDP client to support fast-path output.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERATTRIBUTE to the client, set xorBpp to 24, set hotspot to ({0}, {1})", PointerWidth - 1, PointerHeight - 1);
            this.rdpbcgrAdapter.FPNewPointer(24, 0, PointerWidth - 1, PointerHeight - 1, PointerWidth, PointerHeight);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERPOSATTRIBUTE to the client to set pointer to ({0}, {1}).", PointerPos.xPos, PointerPos.yPos);
            this.rdpbcgrAdapter.FPPointerPosition(PointerPos.xPos, PointerPos.yPos);

            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to verify RDP client can show the pointer correctly when receiving a Fast-Path New Pointer Update message with xorMaskData with padded data (set pointer's width to an odd value).")]
        public void S10_FastPathOutput_PositiveTest_NewPointer_OddWidth()
        {
            #region Test Description
            /*
            This test case is used to verify RDP client can show the pointer correctly when receiving a Fast-Path New Pointer Update message with xorMaskData with padded data (set pointer's width to an odd value). 
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection.
            2.	After the connection sequence has been finished, Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on. 
            3.	Test Suite sends a Fast-Path New Pointer Update (TS_FP_POINTERATTRIBUTE) to the client to set pointer shape, the xorMaskData contains padded data(set pointer's width to an odd value).
            4.	Test Suite sends a Fast-Path Pointer Position Update (TS_FP_POINTERPOSATTRIBUTE) to the client to show the pointer on a specific position.
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

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            this.Site.Assume.IsTrue(this.rdpbcgrAdapter.SessionContext.IsFastPathOutputSupported, "Capability check failed, this test case need RDP client to support fast-path output.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERATTRIBUTE to the client, set xorBpp to 24, pointer width is set to an odd value.");
            this.rdpbcgrAdapter.FPNewPointer(24, 0, PointerOddWidth / 2, PointerHeight / 2, PointerOddWidth, PointerHeight);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERPOSATTRIBUTE to the client to set pointer to ({0}, {1}).", PointerPos.xPos, PointerPos.yPos);
            this.rdpbcgrAdapter.FPPointerPosition(PointerPos.xPos, PointerPos.yPos);

            #endregion
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to verify RDP client can show the pointer correctly when receiving a Fast-Path New Pointer Update message with XorBpp be set to 32.")]
        public void S10_FastPathOutput_PositiveTest_NewPointer_XorBpp_32()
        {
            #region Test Description
            /*
            This test case is used to verify RDP client can show the pointer correctly when receiving a Fast-Path New Pointer Update message with XorBpp be set to 32.
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection.
            2.	After the connection sequence has been finished, Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on. 
            3.	Test Suite sends a Fast-Path New Pointer Update (TS_FP_POINTERATTRIBUTE) to the client to set pointer shape, set XorBpp to 32.
            4.	Test Suite sends a Fast-Path Pointer Position Update (TS_FP_POINTERPOSATTRIBUTE) to the client to show the pointer on a specific position.
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

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            this.Site.Assume.IsTrue(this.rdpbcgrAdapter.SessionContext.IsFastPathOutputSupported, "Capability check failed, this test case need RDP client to support fast-path output.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERATTRIBUTE to the client, set xorBpp to 32.");
            this.rdpbcgrAdapter.FPNewPointer(32, 0, PointerWidth / 2, PointerHeight / 2, PointerWidth, PointerHeight);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERPOSATTRIBUTE to the client to set pointer to ({0}, {1}).", PointerPos.xPos, PointerPos.yPos);
            this.rdpbcgrAdapter.FPPointerPosition(PointerPos.xPos, PointerPos.yPos);

            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to verify RDP client can show the pointer correctly when receiving a Fast-Path Cached Pointer Update message to update the pointer to the shape cached from previous Fast-Path Color Pointer Update message.")]
        public void S10_FastPathOutput_PositiveTest_CachedPointer_WithColorPointer()
        {
            #region Test Description
            /*
            This test case is used to verify RDP client can show the pointer correctly when receiving a Fast-Path Cached Pointer Update message to update the pointer to the shape cached from prevous Fast-Path Color Pointer Update message.
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection. 
            2.	After the connection sequence has been finished, Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on. 
            3.	Test Suite sends a Fast-Path Color Pointer Update (TS_FP_COLORPOINTERATTRIBUTE) to the client to set pointer shape.
            4.	Test Suite sends a Fast-Path Pointer Position Update (TS_FP_POINTERPOSATTRIBUTE) to the client to set pointer position.
            5.  Test Suite sends a Fast-Path System Pointer Hidden Update (TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE) to the client to hide the pointer.
            6.  Test Suite sends a Fast-Path Cached Pointer Update (TS_FP_CACHEDPOINTERATTRIBUTE) to the client to show the pointer again.
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

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            this.Site.Assume.IsTrue(this.rdpbcgrAdapter.SessionContext.IsFastPathOutputSupported, "Capability check failed, this test case need RDP client to support fast-path output.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_COLORPOINTERATTRIBUTE to the client, set cache index to 0.");
            this.rdpbcgrAdapter.FPColorPointer(0, PointerWidth / 2, PointerHeight / 2, PointerWidth, PointerHeight);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERPOSATTRIBUTE to the client to set pointer to ({0}, {1}).", PointerPos.xPos, PointerPos.yPos);
            this.rdpbcgrAdapter.FPPointerPosition(PointerPos.xPos, PointerPos.yPos);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE to the client to hide the pointer.");
            this.rdpbcgrAdapter.FPSystemPointerHidden();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_CACHEDPOINTERATTRIBUTE to the client to show the pointer from cache index 0.");
            this.rdpbcgrAdapter.FPCachedPointer(0);

            #endregion
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to verify RDP client can show the pointer correctly when receiving a Fast-Path Cached Pointer Update message to update the pointer to the shape cached from previous Fast-Path New Pointer Update message.")]
        public void S10_FastPathOutput_PositiveTest_CachedPointer_WithNewPointer()
        {
            #region Test Description
            /*
            This test case is used to verify RDP client can show the pointer correctly when receiving a Fast-Path Cached Pointer Update message to update the pointer to the shape cached from prevous Fast-Path New Pointer Update message.
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection. 
            2.	After the connection sequence has been finished, Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on. 
            3.	Test Suite sends a Fast-Path New Pointer Update (TS_FP_POINTERATTRIBUTE) to the client to set pointer shape.
            4.	Test Suite sends a Fast-Path Pointer Position Update (TS_FP_POINTERPOSATTRIBUTE) to the client to set pointer position.
            5.  Test Suite sends a Fast-Path System Pointer Hidden Update (TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE) to the client to hide the pointer.
            6.  Test Suite sends a Fast-Path Cached Pointer Update (TS_FP_CACHEDPOINTERATTRIBUTE) to the client to show the pointer again.
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

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            this.Site.Assume.IsTrue(this.rdpbcgrAdapter.SessionContext.IsFastPathOutputSupported, "Capability check failed, this test case need RDP client to support fast-path output.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERATTRIBUTE to the client, set cache index to 0.");
            this.rdpbcgrAdapter.FPNewPointer(24, 0, PointerWidth / 2, PointerHeight / 2, PointerWidth, PointerHeight);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERPOSATTRIBUTE to the client to set pointer to ({0}, {1}).", PointerPos.xPos, PointerPos.yPos);
            this.rdpbcgrAdapter.FPPointerPosition(PointerPos.xPos, PointerPos.yPos);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE to the client to hide the pointer.");
            this.rdpbcgrAdapter.FPSystemPointerHidden();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_CACHEDPOINTERATTRIBUTE to the client to show the pointer from cache index 0.");
            this.rdpbcgrAdapter.FPCachedPointer(0);

            #endregion
        }


        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to verify RDP client can process correctly when receiving two Fast-Path New Pointer Update messages with the same cacheIndex.")]
        public void S10_FastPathOutput_PositiveTest_CachedPointer_WithNewPointer_ReuseCacheIndex()
        {
            #region Test Description
            /*
            This test case is used to verify RDP client can process correctly when receiving two Fast-Path New Pointer Update messages with the same cacheIndex.
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection. 
            2.	After the connection sequence has been finished, Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on. 
            3.	Test Suite sends a Fast-Path New Pointer Update (TS_FP_POINTERATTRIBUTE) to the client to set pointer shape.
            4.	Test Suite sends a Fast-Path Pointer Position Update (TS_FP_POINTERPOSATTRIBUTE) to the client to set pointer position.
            5.  Test Suite sends another Fast-Path New Pointer Update (TS_FP_POINTERATTRIBUTE) to the client to set pointer shape, using the same cacheIndex as the first one.
            5.  Test Suite sends a Fast-Path System Pointer Hidden Update (TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE) to the client to hide the pointer.
            6.  Test Suite sends a Fast-Path Cached Pointer Update (TS_FP_CACHEDPOINTERATTRIBUTE) to the client to show the pointer, expect display the second pointer shape.
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

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            this.Site.Assume.IsTrue(this.rdpbcgrAdapter.SessionContext.IsFastPathOutputSupported, "Capability check failed, this test case need RDP client to support fast-path output.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERATTRIBUTE to the client, set cache index to 0.");
            this.rdpbcgrAdapter.FPNewPointer(24, 0, PointerWidth / 2, PointerHeight / 2, PointerWidth, PointerHeight);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERPOSATTRIBUTE to the client to set pointer to ({0}, {1}).", PointerPos.xPos, PointerPos.yPos);
            this.rdpbcgrAdapter.FPPointerPosition(PointerPos.xPos, PointerPos.yPos);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERATTRIBUTE to the client to update pointer, set cache index to 0 too.");
            this.rdpbcgrAdapter.FPNewPointer(24, 0, PointerWidthForUpdate / 2, PointerHeightForUpdate / 2, PointerWidthForUpdate, PointerHeightForUpdate);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE to the client to hide the pointer.");
            this.rdpbcgrAdapter.FPSystemPointerHidden();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_CACHEDPOINTERATTRIBUTE to the client to show the pointer from cache index 0, the pointer should be the updated pointer in the second TS_FP_POINTERATTRIBUTE.");
            this.rdpbcgrAdapter.FPCachedPointer(0);

            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description("This test case is used to verify RDP client can process correctly when cached multiple pointer shapes from Fast-Path New Pointer Update messages.")]
        public void S10_FastPathOutput_PositiveTest_CachedPointer_WithNewPointer_MultiCachedPointers()
        {
            #region Test Description
            /*
            This test case is used to verify RDP client can process correctly when cached multiple pointer shapes from Fast-Path New Pointer Update messages. 
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection. 
            2.	After the connection sequence has been finished, Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on. 
            3.	Test Suite sends a Fast-Path New Pointer Update (TS_FP_POINTERATTRIBUTE) to the client to set pointer shape, set cacheIndex to 0.
            4.	Test Suite sends a Fast-Path Pointer Position Update (TS_FP_POINTERPOSATTRIBUTE) to the client to set pointer position.
            5.  Test Suite sends another Fast-Path New Pointer Update (TS_FP_POINTERATTRIBUTE) to the client to set pointer shape, set cacheIndex to 1.
            5.  Test Suite sends a Fast-Path Cached Pointer Update (TS_FP_CACHEDPOINTERATTRIBUTE) to the client to show the first pointer shape (cacheIndex = 0).
            6.  Test Suite sends a Fast-Path Cached Pointer Update (TS_FP_CACHEDPOINTERATTRIBUTE) to the client to show the second pointer shape (cacheIndex = 1).
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

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            this.Site.Assume.IsTrue(this.rdpbcgrAdapter.SessionContext.IsFastPathOutputSupported, "Capability check failed, this test case need RDP client to support fast-path output.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERATTRIBUTE to the client, set cache index to 0.");
            this.rdpbcgrAdapter.FPNewPointer(24, 0, PointerWidth / 2, PointerHeight / 2, PointerWidth, PointerHeight);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERPOSATTRIBUTE to the client to set pointer to ({0}, {1}).", PointerPos.xPos, PointerPos.yPos);
            this.rdpbcgrAdapter.FPPointerPosition(PointerPos.xPos, PointerPos.yPos);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_POINTERATTRIBUTE to the client to update pointer, set cache index to 1.");
            this.rdpbcgrAdapter.FPNewPointer(24, 1, PointerWidthForUpdate / 2, PointerHeightForUpdate / 2, PointerWidthForUpdate, PointerHeightForUpdate);


            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_CACHEDPOINTERATTRIBUTE to the client to show the pointer from cache index 0, the pointer should be the first pointer.");
            this.rdpbcgrAdapter.FPCachedPointer(0);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending TS_FP_CACHEDPOINTERATTRIBUTE to the client to show the pointer from cache index 1, the pointer should be the second pointer.");
            this.rdpbcgrAdapter.FPCachedPointer(1);
            #endregion
        }
    }
}