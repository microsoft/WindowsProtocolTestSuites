// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.Rdp
{
    /// <summary>
    /// The interface of IRdpSutControlAdapter, which defines public methods
    /// for interacting with the RDP client in test case running time.
    /// </summary>
    public interface IRdpSutControlAdapter : IAdapter
    {
        /// <summary>
        /// This method used to trigger client to initiate a RDP connection from RDP client, 
        /// and the client should use Direct Approach with CredSSP as the security protocol.
        /// </summary>
        /// <param name="caseName">Name of test case</param>
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        [MethodHelp("Please initiate a remote desktop connection from the client using the Direct Approach connection sequence and the CredSSP security protocol.\r\n\r\n"+
                    "Note: please finish the operation in 10 seconds otherwise the case will fail with timeout.")]
        [DefaultValue("1")]
        int RDPConnectWithDirectCredSSP(string caseName);

        /// <summary>
        /// This method used to trigger client to initiate a RDP connection from RDP client, 
        /// and the client should use Direct Approach with CredSSP as the security protocol
        /// and should also attempt connecting with an invalid username.
        /// </summary>
        /// <param name="caseName">Name of test case</param>
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        [MethodHelp("Please initiate a remote desktop connection from the client, with invalid credentials, using the Direct Approach connection sequence and the CredSSP security protocol.\r\n\r\n" +
                    "Note: please finish the operation in 10 seconds otherwise the case will fail with timeout.")]
        [DefaultValue("1")]
        int RDPConnectWithDirectCredSSPInvalidAccount(string caseName);

        /// <summary>
        /// This method used to trigger client to initiate a full screen RDP connection from RDP client, 
        /// and the client should use Direct Approach with CredSSP as the security protocol.
        /// </summary>
        /// <param name="caseName">Name of test case</param>
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        [MethodHelp("Please initiate a full screen remote desktop connection from the client using the Direct Approach connection sequence and the CredSSP security protocol.\r\n\r\n" +
                    "Note: please finish the operation in 10 seconds otherwise the case will fail with timeout.")]
        [DefaultValue("1")]
        int RDPConnectWithDirectCredSSPFullScreen(string caseName);

        /// <summary>
        /// This method used to trigger client to initiate a RDP connection from RDP client, 
        /// and the client should use Negotiation-Based Approach to advertise the support for TLS, CredSSP or RDP standard security protocol.
        /// </summary>
        /// <param name="caseName">Name of test case</param>
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        [MethodHelp("Please initiate a remote desktop connection from the client using the negotiation-based approach and the TLS, CredSSP, or RDP standard security protocol.\r\n\r\n"+
                    "Note: please finish the operation in 10 seconds otherwise the case will fail with timeout.")]
        [DefaultValue("1")]
        int RDPConnectWithNegotiationApproach(string caseName);

        /// <summary>
        /// This method used to trigger client to initiate a RDP connection from RDP client, 
        /// and the client should use Negotiation-Based Approach to advertise the support for TLS, CredSSP or RDP standard security protocol using an Invalid Username.
        /// </summary>
        /// <param name="caseName">Name of test case</param>
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        [MethodHelp("Please initiate a remote desktop connection from the client using the negotiation-based approach and the TLS, CredSSP, or RDP standard security protocol.\r\n\r\n" +
                    "Note: please finish the operation in 10 seconds otherwise the case will fail with timeout.")]
        [DefaultValue("1")]
        int RDPConnectWithNegotiationApproachInvalidAccount(string caseName);

        /// <summary>
        /// This method used to trigger client to initiate a full screen RDP connection from RDP client, 
        /// and the client should use Negotiation-Based Approach to advertise the support for TLS, CredSSP or RDP standard security protocol.
        /// </summary>
        /// <param name="caseName">Name of test case</param>
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        [MethodHelp("Please initiate a full screen remote desktop connection from the client using the negotiation-based approach and the TLS, CredSSP, or RDP standard security protocol.\r\n\r\n" +
                    "Note: please finish the operation in 10 seconds otherwise the case will fail with timeout.")]
        [DefaultValue("1")]
        int RDPConnectWithNegotiationApproachFullScreen(string caseName);

        /// <summary>
        /// This method is used to trigger RDP client initiate a disconnection of current session.
        /// </summary>
        /// <param name="caseName">Name of test case</param>
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        [MethodHelp("Please disconnect a remote desktop session between the client and server.\r\n\r\n" +
                    "Note: please finish the operation in 10 seconds otherwise the case will fail with timeout.")]
        [DefaultValue("1")]
        int TriggerCloseRDPWindow(string caseName);

        /// <summary>
        /// This method is used to trigger RDP client to close all RDP connection to a server for clean up.
        /// </summary>
        /// <param name="caseName">Name of test case</param>
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        [MethodHelp("Please close all remote desktop sessions between the client and server.\r\n\r\n" +
                    "Note: please finish the operation in 10 seconds otherwise the case will fail with timeout.")]
        [DefaultValue("1")]
        int TriggerClientDisconnectAll(string caseName);

        /// <summary>
        /// This method is used to trigger change of stored credentials to invalid user account.
        /// </summary>
        /// <param name="caseName">Name of test case</param>
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        [MethodHelp("Please initiate the addition of invalid credentials into the credential manager.\r\n\r\n" +
                    "Note: please finish the operation in 10 seconds otherwise the case will fail with timeout.")]
        [DefaultValue("1")]
        int CredentialManagerAddInvalidAccount(string caseName);

        /// <summary>
        /// This method is used to trigger change of stored credentials to valid user account.
        /// </summary>
        /// <param name="caseName">Name of test case</param>
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        [MethodHelp("Please initiate the addition of valid credentials into the credential manager.\r\n\r\n" +
                    "Note: please finish the operation in 10 seconds otherwise the case will fail with timeout.")]
        [DefaultValue("1")]
        int CredentialManagerReverseInvalidAccount(string caseName);

        /// <summary>
        /// This method is used to trigger RDP client to start an Auto-Reconnect sequence after a network interruption.
        /// </summary>
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        [MethodHelp("Please trigger an automatic reconnection using a method such as creating a short-term network failure between the client and server.\r\n\r\n"+
                    "Note: please finish the operation in 10 seconds otherwise the case will fail with timeout.")]
        [DefaultValue("1")]
        int TriggerClientAutoReconnect(string caseName);

        /// <summary>
        /// This method is used to trigger the client to server input events. 
        /// </summary>
        /// <param name="caseName">Name of test case</param>
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        [MethodHelp("Please trigger the following events from the remote desktop session:\r\n"+
                    "    •Keyboard input or Unicode keyboard input\r\n"+
                    "    •Mouse input or extended mouse input\r\n"+
                    "    •Client Synchronize\r\n"+
                    "    •Client Refresh\r\n"+
                    "    •Client Suppress Output\r\n\r\n"+
                    "Note: please finish the operation in 10 seconds otherwise the case will fail with timeout.")]
        [DefaultValue("1")]
        int TriggerInputEvents(string caseName);

        /// <summary>
        /// This method is used to trigger client do a screenshot and transfer the captured image to the server, this method will save the captured image on filepath
        /// </summary>
        /// <param name="caseName">Name of test case</param>
        /// <param name="filePath">Filepath to save captured image</param>
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        [MethodHelp("This SUT control adapter interface is only available when using protocol-based SUT control adapter.\r\n\r\n" +
                    "If you have implemented an Agent for protocol based SUT control adapter, please use protocol-based SUT control adapter.\r\n\r\n" +
                    "Otherwise, please change the configuration in RDP_ClientTestSuite.deployment.ptfconfig, set Enable entry in VerifySUTDisplay group to false to disable SUT display verification.\r\n\r\n" +
                    "For more details, please refer to the user guide.")]
        [DefaultValue("0")]
        int CaptureScreenShot(string caseName, string filePath);

        /// <summary>
        /// This method is used to enable compression in PS adapter, CS managed adapter would generated RDP file directly with the compression flag
        /// </summary>
        /// <param name="isCompressionEnable">If enable compression</param>
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        [MethodHelp("This method is used to enable compression in PS adapter, protocol-based SUT control adapter would generated RDP file directly with the compression flag.\r\n\r\n")]
        [DefaultValue("0")]
        int SetCompressionValue(bool isCompressionEnable);
    }
}
