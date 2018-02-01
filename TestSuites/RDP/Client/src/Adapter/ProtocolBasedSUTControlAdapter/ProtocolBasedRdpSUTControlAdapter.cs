// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestSuites;
using Microsoft.Protocols.TestTools;
using System.Net;
using System.Collections;
using System.Reflection;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace Microsoft.Protocols.TestSuites.Rdp
{
    public class ProtocolBasedRdpSUTControlAdapter : ManagedAdapterBase, IRdpSutControlAdapter
    {

        #region variables

        const string interfaceFullName = "Microsoft.Protocols.TestSuites.Rdp.IRdpSutControlAdapter";
        const string basicRDPFileString = "session bpp:i:32\nconnection type:i:6\ncompression:i:0\nauthentication level:i:0\n";

        string localIP;
        ushort localPort;
        RDP_Connect_Payload_Type connectPayloadType;
        SUTControlProtocolHandler controlHandler;

        #endregion variables


        #region IRdpSutControlAdapter Implementation
        
        /// <summary>
        /// This method used to trigger client to initiate a RDP connection from RDP client, 
        /// and the client should use Direct Approach with CredSSP as the security protocol.
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        public int RDPConnectWithDirectCredSSP(string caseName)
        {
            // Get help message
            string helpMessage = CommonUtility.GetHelpMessage(interfaceFullName);
            // Create payload
            byte[] payload = CreateRDPConncectPayload(connectPayloadType, localIP, localPort, true, false);

            return Start_RDP_Connection(caseName, payload, helpMessage);
        }

        /// <summary>
        /// This method used to trigger client to initiate a full screen RDP connection from RDP client, 
        /// and the client should use Direct Approach with CredSSP as the security protocol.
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        public int RDPConnectWithDirectCredSSPFullScreen(string caseName)
        {
            // Get help message
            string helpMessage = CommonUtility.GetHelpMessage(interfaceFullName);
            // Create payload
            byte[] payload = CreateRDPConncectPayload(connectPayloadType, localIP, localPort, true, true);

            return Start_RDP_Connection(caseName, payload, helpMessage);
        }

        /// <summary>
        /// This method used to trigger client to initiate a RDP connection from RDP client, 
        /// and the client should use Negotiation-Based Approach to advertise the support for TLS, CredSSP or RDP standard security protocol.
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        public int RDPConnectWithNegotiationApproach(string caseName)
        {
            // Get help message
            string helpMessage = CommonUtility.GetHelpMessage(interfaceFullName);
            // Create payload
            byte[] payload = CreateRDPConncectPayload(connectPayloadType, localIP, localPort, false, false);

            return Start_RDP_Connection(caseName, payload, helpMessage);
        }

        /// <summary>
        /// This method used to trigger client to initiate a full screen RDP connection from RDP client, 
        /// and the client should use Negotiation-Based Approach to advertise the support for TLS, CredSSP or RDP standard security protocol.
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        public int RDPConnectWithNegotiationApproachFullScreen(string caseName)
        {
            // Get help message
            string helpMessage = CommonUtility.GetHelpMessage(interfaceFullName);
            // Create payload
            byte[] payload = CreateRDPConncectPayload(connectPayloadType, localIP, localPort, false, true);

            return Start_RDP_Connection(caseName, payload, helpMessage);
        }

        /// <summary>
        /// This method is used to trigger RDP client initiate a disconnection of current session.
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        public int TriggerClientDisconnect(string caseName)
        {      
            // Get help message
            string helpMessage = CommonUtility.GetHelpMessage(interfaceFullName);
            // Create payload
            byte[] payload = null;
            
            // Create request message
            ushort reqId = controlHandler.GetNextRequestId();
            SUT_Control_Request_Message requestMessage = new SUT_Control_Request_Message(SUTControl_TestsuiteId.RDP_TESTSUITE, (ushort)RDPSUTControl_CommandId.CLOSE_RDP_CONNECTION, caseName,
                reqId, helpMessage, payload);
            
            //Send the request and get response if necessary
            byte[] resposePayload = null;
            return controlHandler.OperateSUTControl(requestMessage, false, out resposePayload);
                        
        }

        /// <summary>
        /// This method is used to trigger RDP client to close all RDP connection to a server for clean up.
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        public int TriggerClientDisconnectAll(string caseName)
        {
            // Get help message
            string helpMessage = CommonUtility.GetHelpMessage(interfaceFullName);
            // Create payload
            byte[] payload = null;

            // Create request message
            ushort reqId = controlHandler.GetNextRequestId();
            SUT_Control_Request_Message requestMessage = new SUT_Control_Request_Message(SUTControl_TestsuiteId.RDP_TESTSUITE, (ushort)RDPSUTControl_CommandId.CLOSE_RDP_CONNECTION, caseName,
                reqId, helpMessage, payload);

            //Send the request and get response if necessary
            byte[] resposePayload = null;
            return controlHandler.OperateSUTControl(requestMessage, false, out resposePayload);
        }

        /// <summary>
        /// This method is used to trigger RDP client to start an Auto-Reconnect sequence after a network interruption.
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        public int TriggerClientAutoReconnect(string caseName)
        {
            // Get help message
            string helpMessage = CommonUtility.GetHelpMessage(interfaceFullName);
            // Create payload
            byte[] payload = null;

            // Create request message
            ushort reqId = controlHandler.GetNextRequestId();
            SUT_Control_Request_Message requestMessage = new SUT_Control_Request_Message(SUTControl_TestsuiteId.RDP_TESTSUITE, (ushort)RDPSUTControl_CommandId.AUTO_RECONNECT, caseName,
                        reqId, helpMessage, payload);

            //Send the request and get response if necessary
            byte[] resposePayload = null;
            return controlHandler.OperateSUTControl(requestMessage, false, out resposePayload);            
        }
        
        /// <summary>
        /// This method is used to trigger the client to server input events. 
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        public int TriggerInputEvents(string caseName)
        {
            // Get help message
            string helpMessage = CommonUtility.GetHelpMessage(interfaceFullName);
            // Create payload
            RDPSUTControl_BasicInputFlag inputFlag = RDPSUTControl_BasicInputFlag.Keyboard_Event | RDPSUTControl_BasicInputFlag.Unicode_Keyboard_Event
                | RDPSUTControl_BasicInputFlag.Mouse_Event | RDPSUTControl_BasicInputFlag.Extended_Mouse_Event
                | RDPSUTControl_BasicInputFlag.Client_Synchronize_Event | RDPSUTControl_BasicInputFlag.Client_Refresh_Rect
                | RDPSUTControl_BasicInputFlag.Client_Suppress_Output;

            byte[] payload = BitConverter.GetBytes((uint)inputFlag);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(payload);
            }
            
            // Create request message
            ushort reqId = controlHandler.GetNextRequestId();
            SUT_Control_Request_Message requestMessage = new SUT_Control_Request_Message(SUTControl_TestsuiteId.RDP_TESTSUITE, (ushort)RDPSUTControl_CommandId.BASIC_INPUT, caseName,
                reqId, helpMessage, payload);

            //Send the request and get response if necessary
            byte[] resposePayload = null;
            return controlHandler.OperateSUTControl(requestMessage, false, out resposePayload);  
        }

        /// <summary>
        /// This method is used to trigger client do a screenshot and transfer the captured image to the server, this method will save the captured image on filepath
        /// </summary>
        /// <param name="filePath">Filepath to save captured image</param>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        public int CaptureScreenShot(string caseName, string filePath)
        {
            if (!controlHandler.IsUsingTCP)
            {
                this.Site.Assume.Inconclusive("ScreenShot control method needs to transfer big data, only available when using TCP transport");
            }
            // Get help message
            string helpMessage = CommonUtility.GetHelpMessage(interfaceFullName);
            // Create payload
            byte[] payload = null;

            // Create request message
            ushort reqId = controlHandler.GetNextRequestId();
            SUT_Control_Request_Message requestMessage = new SUT_Control_Request_Message(SUTControl_TestsuiteId.RDP_TESTSUITE, (ushort)RDPSUTControl_CommandId.SCREEN_SHOT, caseName,
                        reqId, helpMessage, payload);

            //Send the request and get response if necessary
            byte[] resposePayload = null;
            if (controlHandler.OperateSUTControl(requestMessage, false, out resposePayload) > 0)
            {
                DecodeBitmapBinary(resposePayload, filePath);
                return 1;
            }
            else
            {
                return -1;
            }
            
        }

        /// <summary>
        /// Initialize this adapter
        /// </summary>
        /// <param name="testSite"></param>
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);

            controlHandler = SUTControlProtocolHandler.GetInstance(testSite);         
            // Initiate local IP
            localIP = GetLocalIP();

            // Initiate local port
            try
            {
                localPort = ushort.Parse(testSite.Properties["RDP.ServerPort"]);
            }
            catch
            {
                localPort = 0;
            }
                        
            // Initiate Connect payload type
            bool clientSupportRDPFile = false;
            try
            {
                clientSupportRDPFile = bool.Parse(testSite.Properties["SUTControl.ClientSupportRDPFile"]);
            }
            catch
            {                
            }
            if (clientSupportRDPFile)
            {
                connectPayloadType = RDP_Connect_Payload_Type.RDP_FILE;
            }
            else
            {
                connectPayloadType = RDP_Connect_Payload_Type.PARAMETERS_STRUCT;
            }
                        
        }


        #endregion IRdpSutControlAdapter Implementation


        #region Private methods

        /// <summary>
        /// Trigger SUT to start a RDP Connection
        /// </summary>
        /// <param name="payload">Payload of SUT control request message</param>
        /// <param name="helpMessage">helpMessage of SUT control request message</param>
        /// <returns></returns>
        private int Start_RDP_Connection(string caseName, byte[] payload, string helpMessage)
        {
            ushort reqId = controlHandler.GetNextRequestId();
            SUT_Control_Request_Message requestMessage = new SUT_Control_Request_Message(SUTControl_TestsuiteId.RDP_TESTSUITE, (ushort)RDPSUTControl_CommandId.START_RDP_CONNECTION, caseName,
                        reqId, helpMessage, payload);

            byte[] resposePayload = null;
            return controlHandler.OperateSUTControl(requestMessage, false, out resposePayload);
            
        }
        
        /// <summary>
        /// Create a payload for start RDP connection command
        /// </summary>
        /// <param name="payloadType">Type of payload: Content of .rdp file or Parameter structure</param>
        /// <param name="localAddress">Local address</param>
        /// <param name="RDPPort">Port test suite listening</param>
        /// <param name="DirectApproach">true for 'Direct', false for 'Negotiate'</param>
        /// <param name="fullScreen">true for full screen, otherwise false</param>
        /// <returns>Return encoded binary of the payload</returns>
        private byte[] CreateRDPConncectPayload(RDP_Connect_Payload_Type payloadType, string localAddress, uint RDPPort, bool DirectApproach, bool fullScreen)
        {
            RDP_Connection_Payload payLoad = new RDP_Connection_Payload();
            payLoad.type = payloadType;

            if (payloadType == RDP_Connect_Payload_Type.RDP_FILE)
            {
                payLoad.rdpFileConfig = GenerateRDPFileString(localAddress, RDPPort, DirectApproach, fullScreen);
            }
            else
            {
                payLoad.configureParameters = GenerateRDPConnectionConfigParameters(localAddress, RDPPort, DirectApproach, fullScreen);
            }

            return payLoad.Encode();
        }

        /// <summary>
        /// Generate content of .RDP file
        /// </summary>
        /// <param name="localAddress">Local address</param>
        /// <param name="RDPPort">Port for RDP test suite listening</param>
        /// <param name="DirectApproach">true for 'Direct', false for 'Negotiate'</param>
        /// <param name="fullScreen">true for full screen, otherwise false</param>
        /// <returns>string contains content of .RDP file</returns>
        private string GenerateRDPFileString(string localAddress, uint RDPPort, bool DirectApproach, bool fullScreen)
        {
            string rdpConfigString = basicRDPFileString;
            // Add RDP server address
            if (localAddress != null)
            {
                if (RDPPort > 0)
                {
                    rdpConfigString += "full address:s:" + localAddress + ":" + RDPPort + "\n";
                }
                else
                {
                    rdpConfigString += "full address:s:" + localAddress + "\n";
                }
            }

            if (DirectApproach)
            {
                rdpConfigString += "negotiate security layer:i:0\nenablecredsspsupport:i:1\npromptcredentialonce:i:1\n";
            }
            else
            {
                rdpConfigString += "negotiate security layer:i:1\npromptcredentialonce:i:0\n";
            }

            if (fullScreen)
            {
                rdpConfigString += "screen mode id:i:2\n";
            }
            else
            {
                rdpConfigString += "screen mode id:i:1\ndesktopwidth:i:1024\ndesktopheight:i:768\n";
            }
            rdpConfigString += "pinconnectionbar:i:0\n";
            return rdpConfigString;
        }

        /// <summary>
        /// Construct a Parameter structure for Start_RDP_Connection command
        /// </summary>
        /// <param name="localAddress">Local address</param>
        /// <param name="RDPPort">Port for RDP test suite listening</param>
        /// <param name="DirectApproach">true for 'Direct', false for 'Negotiate'</param>
        /// <param name="fullScreen">true for full screen, otherwise false</param>
        /// <returns>RDP_Connection_Configure_Parameters structure</returns>
        private RDP_Connection_Configure_Parameters GenerateRDPConnectionConfigParameters(string localAddress, uint RDPPort, bool DirectApproach, bool fullScreen)
        {
            RDP_Connection_Configure_Parameters config = new RDP_Connection_Configure_Parameters();
            config.port = (ushort)RDPPort;
            config.address = localAddress;

            config.screenType = RDP_Screen_Type.NORMAL;
            if (fullScreen)
            {
                config.screenType = RDP_Screen_Type.FULL_SCREEN;
            }

            config.connectApproach = RDP_Connect_Approach.Negotiate;
            if (DirectApproach)
            {
                config.connectApproach = RDP_Connect_Approach.Direct;
            }

            config.desktopWidth = 1024;
            config.desktopHeight = 768;

            return config;

        }
        
        /// <summary>
        /// Get IP address of local host
        /// </summary>
        /// <returns>string for local IP address</returns>
        private string GetLocalIP()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return null;
        }

        /// <summary>
        /// Decode byte array to a bitmap picture and save it to bitmap file
        /// </summary>
        /// <param name="bitmapBinary">byte array contains bitmap data</param>
        /// <param name="bitmapFile">full path to save the graphic picture</param>
        private void DecodeBitmapBinary(byte[] bitmapBinary, string bitmapFile)
        {
            int index = 0;
            byte[] widthArray = new byte[4];
            Array.Copy(bitmapBinary, index, widthArray, 0, 4);
            index += 4;
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(widthArray);
            }
            int width = BitConverter.ToInt32(widthArray, 0);

            byte[] heightArray = new byte[4];
            Array.Copy(bitmapBinary, index, heightArray, 0, 4);
            index += 4;
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(heightArray);
            }
            int height = BitConverter.ToInt32(heightArray, 0);

            Bitmap bitmapScreen = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    byte R = bitmapBinary[index++];
                    byte G = bitmapBinary[index++];
                    byte B = bitmapBinary[index++];
                    bitmapScreen.SetPixel(i, j, Color.FromArgb(R, G, B));
                }
            }
            bitmapScreen.Save(bitmapFile, ImageFormat.Bmp);
        }

        #endregion Private methods
    }
}
