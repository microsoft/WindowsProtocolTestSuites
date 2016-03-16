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
        /// and the client should use Direct Approach with TLS as the security protocol.
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        [MethodHelp("1.Initiate a remote desktop connection from the client using the Direct Approach connection sequence and the TLS security protocol.\r\n\r\n"+
                    "2.Enter a return value, using a positive value for a successful connection or a negative value for a failed connection.\r\n\r\n"+
                    "3.To pass the value to the test case, click the Succeed button. Or, to end the test case, enter a message into the Failure Message dialog box and then click Fail.")]
        [DefaultValue("1")]
        int RDPConnectWithDirectTLS();

        /// <summary>
        /// This method used to trigger client to initiate a full screen RDP connection from RDP client, 
        /// and the client should use Direct Approach with TLS as the security protocol.
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        [MethodHelp("1.Initiate a full screen remote desktop connection from the client using the Direct Approach connection sequence and the TLS security protocol.\r\n\r\n" +
                    "2.Enter a return value, using a positive value for a successful connection or a negative value for a failed connection.\r\n\r\n" +
                    "3.To pass the value to the test case, click the Succeed button. Or, to end the test case, enter a message into the Failure Message dialog box and then click Fail.")]
        [DefaultValue("1")]
        int RDPConnectWithDirectTLSFullScreen();

        /// <summary>
        /// This method used to trigger client to initiate a RDP connection from RDP client, 
        /// and the client should use Direct Approach with CredSSP as the security protocol.
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        [MethodHelp("1.Initiate a remote desktop connection from the client using the Direct Approach connection sequence and the CredSSP security protocol.\r\n\r\n"+
                    "2.Enter a return value, using a positive value for a successful connection or a negative value for a failed connection.\r\n\r\n" +
                    "3.To pass the value to the test case, click the Succeed button. Or, to end the test case, enter a message into the Failure Message dialog box and then click Fail.")]
        [DefaultValue("1")]
        int RDPConnectWithDirectCredSSP();

        /// <summary>
        /// This method used to trigger client to initiate a full screen RDP connection from RDP client, 
        /// and the client should use Direct Approach with CredSSP as the security protocol.
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        [MethodHelp("1.Initiate a full screen remote desktop connection from the client using the Direct Approach connection sequence and the CredSSP security protocol.\r\n\r\n" +
                    "2.Enter a return value, using a positive value for a successful connection or a negative value for a failed connection.\r\n\r\n" +
                    "3.To pass the value to the test case, click the Succeed button. Or, to end the test case, enter a message into the Failure Message dialog box and then click Fail.")]
        [DefaultValue("1")]
        int RDPConnectWithDirectCredSSPFullScreen();

        /// <summary>
        /// This method used to trigger client to initiate a RDP connection from RDP client, 
        /// and the client should use Negotiation-Based Approach to advertise the support for TLS, CredSSP or RDP standard security protocol.
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        [MethodHelp("1.Initiate a remote desktop connection from the client using the negotiation-based approach and the TLS, CredSSP, or RDP standard security protocol.\r\n\r\n"+
                    "2.Enter a return value, using a positive value for a successful connection or a negative value for a failed connection.\r\n\r\n"+
                    "3.To pass the value to the test case, click the Succeed button. Or, to end the test case, enter a message into the Failure Message dialog box and then click Fail.")]
        [DefaultValue("1")]
        int RDPConnectWithNegotiationApproach();

        /// <summary>
        /// This method used to trigger client to initiate a full screen RDP connection from RDP client, 
        /// and the client should use Negotiation-Based Approach to advertise the support for TLS, CredSSP or RDP standard security protocol.
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        [MethodHelp("1.Initiate a full screen remote desktop connection from the client using the negotiation-based approach and the TLS, CredSSP, or RDP standard security protocol.\r\n\r\n" +
                    "2.Enter a return value, using a positive value for a successful connection or a negative value for a failed connection.\r\n\r\n" +
                    "3.To pass the value to the test case, click the Succeed button. Or, to end the test case, enter a message into the Failure Message dialog box and then click Fail.")]
        [DefaultValue("1")]
        int RDPConnectWithNegotiationApproachFullScreen();

        /// <summary>
        /// This method is used to trigger RDP client initiate a disconnection of current session.
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        [MethodHelp("1.Disconnect a remote desktop session between the client and server.\r\n\r\n" +
                    "2.Enter a return value, using a positive value if the disconnect succeeds or a negative value if the disconnect fails.\r\n\r\n" +
                    "3.To pass the value to the test case, click the Succeed button. Or, to end the test case, enter a message into the Failure Message dialog box and then click Fail.")]
        [DefaultValue("1")]
        int TriggerClientDisconnect();

        /// <summary>
        /// This method is used to trigger RDP client to close all RDP connection to a server for clean up.
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        [MethodHelp("1.Close all remote desktop sessions between the client and server.\r\n\r\n" +
                    "2.Enter a return value, using a positive value if all connections close successfully, or a negative value if any connections fail to close.\r\n\r\n" +
                    "3.To pass the value to the test case, click the Succeed button. Or, to end the test case, enter a message into the Failure Message dialog box and then click Fail.")]
        [DefaultValue("1")]
        int TriggerClientDisconnectAll();

        /// <summary>
        /// This method is used to trigger RDP client to start an Auto-Reconnect sequence after a network interruption.
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        [MethodHelp("1.Trigger an automatic reconnection using a method such as creating a short-term network failure between the client and server.\r\n\r\n"+
                    "2.Enter a return value, using a positive value if the reconnection succeeds or a negative value if the reconnection fails.\r\n\r\n"+
                    "3.To pass the value to the test case, click the Succeed button. Or, to end the test case, enter a message into the Failure Message dialog box and then click Fail. ")]
        [DefaultValue("1")]
        int TriggerClientAutoReconnect();

        /// <summary>
        /// This method is used to trigger the client to server input events. 
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        [MethodHelp("1.Trigger the following events from the remote desktop session:\r\n"+
                    "    •Keyboard input or Unicode keyboard input\r\n"+
                    "    •Mouse input or extended mouse input\r\n"+
                    "    •Client Synchronize\r\n"+
                    "    •Client Refresh\r\n"+
                    "    •Client Suppress Output\r\n\r\n"+
                    "2.Enter a return value, using a positive value if all events succeed or a negative value if any events fail.\r\n\r\n" +
                    "3.To pass the value to the test case, click the Succeed button. Or, to end the test case, enter a message into the Failure Message dialog box and then click Fail.")]
        [DefaultValue("1")]
        int TriggerInputEvents();

        /// <summary>
        /// This method is used to trigger client do a screenshot and transfer the captured image to the server, this method will save the captured image on filepath
        /// </summary>
        /// <param name="filePath">Filepath to save captured image</param>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        [MethodHelp("This SUT control adapter interface is only available when using protocol-based SUT control adapter.\r\n\r\n" +
                    "If you have implemented an Agent for protocol based SUT control adapter, please use protocol-based SUT control adapter.\r\n\r\n" +
                    "Otherwise, please change the configuration in RDP_ClientTestSuite.deployment.ptfconfig, set VerifySUTDisplay.Enable entry to false to disable SUT display verification.\r\n\r\n" +
                    "For more details, please refer to the user guide.")]
        [DefaultValue("-1")]
        int CaptureScreenShot(string filePath);

    }
}
