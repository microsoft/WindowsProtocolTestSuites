// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace RDPSUTControlAgent
{
    public delegate void PostFunction();
    public class RDPClientControl
    {
        public static uint WaitSeconds = 5;
        public static PostFunction PostOperation = null;
        /// <summary>
        /// Process SUT Control Command
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public static SUT_Control_Response_Message ProcessCommand(SUT_Control_Request_Message requestMessage)
        {
            PostOperation = null;
            if (requestMessage == null)
            {
                throw new ArgumentNullException("SUT_Control_Request_Message inputed is null");
            }
            if (requestMessage.messageType != SUTControl_MessageType.SUT_CONTROL_REQUEST || requestMessage.testsuiteId != SUTControl_TestsuiteId.RDP_TESTSUITE)
            {
                throw new ArgumentException("Not available request message." +requestMessage.messageType+","+ requestMessage.testsuiteId);
            }

            RDPSUTControl_CommandId commandId =(RDPSUTControl_CommandId) requestMessage.commandId;
            byte[] payload = null;
            string errorMessage = null;
            uint resultCode = 1;
            try
            {
                switch (commandId)
                {
                    case RDPSUTControl_CommandId.START_RDP_CONNECTION:
                        RDP_Connection_Payload rdpPayload = new RDP_Connection_Payload();
                        int index = 0;
                        if (rdpPayload.Decode(requestMessage.payload, (int)requestMessage.payloadLength, ref index))
                        {

                            if (rdpPayload.type == RDP_Connect_Payload_Type.RDP_FILE)
                            {
                                if (Start_RDP_Connection(rdpPayload.rdpFileConfig) > 0)
                                {
                                    resultCode = (uint)SUTControl_ResultCode.SUCCESS;
                                }
                            }
                            else
                            {
                                if (Start_RDP_Connection(rdpPayload.configureParameters) > 0)
                                {
                                    resultCode = (uint)SUTControl_ResultCode.SUCCESS;
                                }
                            }
                        }
                        break;
                    case RDPSUTControl_CommandId.CLOSE_RDP_CONNECTION:
                        if (Close_RDP_Connection() > 0)
                        {
                            resultCode = (uint)SUTControl_ResultCode.SUCCESS;
                        }
                        break;
                    case RDPSUTControl_CommandId.AUTO_RECONNECT:                        
                        resultCode = (uint)SUTControl_ResultCode.SUCCESS;                            
                        PostOperation = AUTO_RECONNECT;                        
                        break;
                    case RDPSUTControl_CommandId.SCREEN_SHOT:
                        if (TAKE_SCREEN_SHOT(out payload) > 0)
                        {
                            resultCode = (uint)SUTControl_ResultCode.SUCCESS;
                        }
                        break;
                    default:
                        errorMessage = "SUT control agent doesn't support this command:"+commandId;
                        break;
                }
            }
            catch(Exception e)
            {
                errorMessage = "Exception found when process " + commandId + "," + e.Message;
            }

            SUT_Control_Response_Message responseMessage = new SUT_Control_Response_Message(SUTControl_TestsuiteId.RDP_TESTSUITE, (ushort)commandId, requestMessage.caseName, requestMessage.requestId, resultCode, errorMessage, payload);
            return responseMessage;
        }

        #region RDP Client Control methods
        public static string TmpRDPFile = @".\Connect.rdp";
        /// <summary>
        /// Start a RDP connection using a RDP file string
        /// </summary>
        /// <param name="rdpFileConfig">Content of a .rdp file</param>
        /// <returns></returns>
        public static int Start_RDP_Connection(string rdpFileConfig)
        {

                //Create RDP File
                FileStream rdpFile = new FileStream(TmpRDPFile, FileMode.Create);
                StreamWriter sw = new StreamWriter(rdpFile);
                sw.Write(rdpFileConfig);
                sw.Flush();
                sw.Close();
                rdpFile.Close();
                
                //Start RDP connection
                Process rdpProcess = new Process();
                rdpProcess.StartInfo.FileName = "mstsc.exe";
                rdpProcess.StartInfo.Arguments = TmpRDPFile;
                rdpProcess.Start();
                rdpProcess.Close();
                return 1;
            
        }

        /// <summary>
        /// Start RDP connection using a parameter structure
        /// </summary>
        /// <param name="configureParameters"></param>
        /// <returns></returns>
        public static int Start_RDP_Connection(RDP_Connection_Configure_Parameters configureParameters)
        {
            string arguments = "";

            if (configureParameters.port == 0)
            {
                arguments += string.Format(@" /v:{0}", configureParameters.address);
            }
            else
            {
                arguments += string.Format(@" /v:{0}:{1}", configureParameters.address, configureParameters.port);
            }

            if (configureParameters.screenType == RDP_Screen_Type.FULL_SCREEN)
            {
                arguments += @" /f";
            }
            else
            {
                arguments += string.Format(@" /w:{0} /h:{1}", configureParameters.desktopWidth, configureParameters.desktopHeight);
            }

            //Start RDP connection
            Process rdpProcess = new Process();
            rdpProcess.StartInfo.FileName = "mstsc.exe";
            rdpProcess.StartInfo.Arguments = arguments;
            rdpProcess.Start();
            rdpProcess.Close();
            return 1;


        }

        /// <summary>
        /// Close all RDP connection
        /// </summary>
        /// <returns></returns>
        public static int Close_RDP_Connection()
        {
                Process[] rdpProcesses = Process.GetProcessesByName("mstsc");
                foreach (Process process in rdpProcesses)
                {
                    process.Kill();
                }
            return 1;
        }


        /// <summary>
        /// Trigger auto reconnect event, make a short-term network failure
        /// </summary>
        public static void AUTO_RECONNECT()
        {
            // Restart network using another thread.
            Thread restartNetworkThread = new Thread(new ThreadStart(RestartNetWorkThread));
            restartNetworkThread.Start();
        }


        /// <summary>
        /// Type a screenshot
        /// </summary>
        /// <param name="screenImageBinary">out parameter for screen image data</param>
        /// <returns></returns>
        public static int TAKE_SCREEN_SHOT(out byte[] screenImageBinary)
        {
            Bitmap bmpmapScreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            Graphics graphicScreen = Graphics.FromImage(bmpmapScreen);
            graphicScreen.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);

            List<byte> imageBuffer = new List<byte>();
            byte[] width = BitConverter.GetBytes(Screen.PrimaryScreen.Bounds.Width);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(width);
            }
            imageBuffer.AddRange(width);

            byte[] height = BitConverter.GetBytes(Screen.PrimaryScreen.Bounds.Height);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(height);
            }
            imageBuffer.AddRange(height);

            for(int j=0; j< Screen.PrimaryScreen.Bounds.Height; j++)
            {
                for (int i = 0; i < Screen.PrimaryScreen.Bounds.Width; i++)
                {
                    Color c = bmpmapScreen.GetPixel(i, j);
                    imageBuffer.Add(c.R);
                    imageBuffer.Add(c.G);
                    imageBuffer.Add(c.B);
                }
            }

            screenImageBinary = imageBuffer.ToArray();
            return 1;
        }

        /// <summary>
        /// Restart all network on the system
        /// </summary>
        private static void RestartNetWorkThread()
        {
            string manage = "SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionId != NULL";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(manage);
            ManagementObjectCollection collection = searcher.Get();
            List<string> netWorkList = new List<string>();
            foreach (ManagementObject network in collection)
            {                
                network.InvokeMethod("Disable", null);                
            }
            System.Threading.Thread.Sleep(new TimeSpan(0,0, (int)WaitSeconds));
            foreach (ManagementObject network in collection)
            {
                network.InvokeMethod("Enable", null);                
            }

            return;
        }

        #endregion
    }
}
