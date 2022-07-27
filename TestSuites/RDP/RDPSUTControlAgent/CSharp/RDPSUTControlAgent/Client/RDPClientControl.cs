// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;

namespace RDPSUTControlAgent
{
    public class RDPClientControl : IRDPControl
    {
        public static uint WaitSeconds = 5;
        public static PostFunction PostOperation = null;
        /// <summary>
        /// Process SUT Control Command
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public SUT_Control_Response_Message ProcessCommand(SUT_Control_Request_Message requestMessage)
        {
            PostOperation = null;
            if (requestMessage == null)
            {
                throw new ArgumentNullException("SUT_Control_Request_Message inputed is null");
            }
            if (requestMessage.messageType != SUTControl_MessageType.SUT_CONTROL_REQUEST || requestMessage.testsuiteId != SUTControl_TestsuiteId.RDP_TESTSUITE)
            {
                throw new ArgumentException("Not available request message." + requestMessage.messageType + "," + requestMessage.testsuiteId);
            }

            RDPSUTControl_CommandId commandId = (RDPSUTControl_CommandId)requestMessage.commandId;
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
                                else
                                {
                                    errorMessage = $"SUT control agent in '{GetCurrentOSType()}' doesn't support this command:" + commandId + " when it is .rdp file";
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
                        else {
                            errorMessage = $"SUT control agent in '{GetCurrentOSType()}' doesn't support this command: " + commandId;
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

                    case RDPSUTControl_CommandId.CREDENTIAL_MANAGER_ADD_INVALID_ACCOUNT:
                        try
                        {
                            Run_TaskScheduler_Task("CredentialManager_Invalid");
                            resultCode = (uint)SUTControl_ResultCode.SUCCESS;
                        }
                        catch (Exception e)
                        {
                            errorMessage = $"SUT control agent encountered an error when executing this command: {commandId} ({e.Message})";
                        }
                        break;

                    case RDPSUTControl_CommandId.CREDENTIAL_MANAGER_REVERSE_INVALID_ACCOUNT:
                        try
                        {
                            Run_TaskScheduler_Task("CredentialManager_InvalidAccount_Reverse");
                            resultCode = (uint)SUTControl_ResultCode.SUCCESS;
                        }
                        catch (Exception e)
                        {
                            errorMessage = $"SUT control agent encountered an error when executing this command: {commandId} ({e.Message})";
                        }
                        break;

                    default:
                        errorMessage = "SUT control agent doesn't support this command: " + commandId;
                        break;
                }
            }
            catch (Exception e)
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
            string remoteClientName = GetRemoteClientName();
            if (remoteClientName == "XFREERDP") {
                return 0;
            }

            //Create RDP File
            FileStream rdpFile = new FileStream(TmpRDPFile, FileMode.Create);
            StreamWriter sw = new StreamWriter(rdpFile);
            sw.Write(rdpFileConfig);
            sw.Flush();
            sw.Close();
            rdpFile.Close();

            //Start RDP connection
            return InvokeRemoteClientProcess(TmpRDPFile);
        }

        /// <summary>
        /// Start RDP connection using a parameter structure
        /// </summary>
        /// <param name="configureParameters"></param>
        /// <returns></returns>
        public static int Start_RDP_Connection(RDP_Connection_Configure_Parameters configureParameters)
        {
            string arguments;
            if (configureParameters.connectApproach == RDP_Connect_Approach.Negotiate && configureParameters.screenType == RDP_Screen_Type.NORMAL)
            {
                arguments = GetConfiguredValue("Negotiate");
            }
            else if (configureParameters.connectApproach == RDP_Connect_Approach.Negotiate && configureParameters.screenType == RDP_Screen_Type.FULL_SCREEN)
            {
                arguments = GetConfiguredValue("NegotiateFullScreen");
            }
            else if (configureParameters.connectApproach == RDP_Connect_Approach.Direct && configureParameters.screenType == RDP_Screen_Type.NORMAL)
            {
                arguments = GetConfiguredValue("DirectCredSSP");
            }
            else if (configureParameters.connectApproach == RDP_Connect_Approach.Direct && configureParameters.screenType == RDP_Screen_Type.FULL_SCREEN)
            {
                arguments = GetConfiguredValue("DirectCredSSPFullScreen");
            }
            else {
                arguments = GetConfiguredValue("Negotiate");
            }

            try {
                arguments = arguments.Replace("{{address}}", configureParameters.address);
                if (configureParameters.port == 0) {
                    arguments = arguments.Replace(":{{port}}", "");
                }
                else {
                    arguments = arguments.Replace("{{port}}", configureParameters.port.ToString());
                }
            }
            catch (Exception e)
            {
                return -1;
            }

            //Start RDP connection
            InvokeRemoteClientProcess(arguments);

            return 1;
        }

        /// <summary>
        /// Close all RDP connection
        /// </summary>
        /// <returns></returns>
        public static int Close_RDP_Connection()
        {
            string stopRDPArguments = GetConfiguredValue("StopRDP");
            int result = InvokeRemoteClientProcess(stopRDPArguments);

            return result;
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
        /// Trigger scheduled task
        /// </summary>
        public static void Run_TaskScheduler_Task( string taskName)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                using (TaskService ts = new TaskService())
                {
                    var runCreatedTask = ts.FindTask(taskName).Run();
                }
            }
            else
            {
                throw new NotImplementedException($"Not Implement in OS {GetCurrentOSType()}");
            }
        }

        /// <summary>
        /// Type a screenshot
        /// </summary>
        /// <param name="screenImageBinary">out parameter for screen image data</param>
        /// <returns></returns>
        public static int TAKE_SCREEN_SHOT(out byte[] screenImageBinary)
        {
            Bitmap testBitmap = new Bitmap(400, 300);
            Graphics graphicScreen = Graphics.FromImage(testBitmap);
            graphicScreen.CopyFromScreen(0, 0, 0, 0, testBitmap.Size, CopyPixelOperation.SourceCopy);

            testBitmap.Save("testBitmap.bmp", ImageFormat.Bmp);

            List<byte> imageBuffer = new List<byte>();
            byte[] width = BitConverter.GetBytes(testBitmap.Width);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(width);
            }
            imageBuffer.AddRange(width);

            byte[] height = BitConverter.GetBytes(testBitmap.Height);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(height);
            }
            imageBuffer.AddRange(height);

            for (int j = 0; j < testBitmap.Height; j++)
            {
                for (int i = 0; i < testBitmap.Width; i++)
                {
                    Color c = testBitmap.GetPixel(i, j);
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
            System.Threading.Thread.Sleep(new TimeSpan(0, 0, (int)WaitSeconds));
            foreach (ManagementObject network in collection)
            {
                network.InvokeMethod("Enable", null);
            }

            return;
        }

        private static int InvokeRemoteClientProcess(string arguments) {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return InvokeWindowsCmdProcess(arguments);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return InvokeLinuxShellProcess(arguments);
            }
            else {
                throw new NotImplementedException($"Not Implement in OS {GetCurrentOSType()}");
            }
        }

        private static int InvokeWindowsCmdProcess(string arguments) {
            Process rdpProcess = new Process();
            rdpProcess.StartInfo.FileName = "cmd.exe";
            rdpProcess.StartInfo.Arguments = $"/c {arguments}";
            rdpProcess.Start();
            rdpProcess.Close();

            return 1;
        }

        private static int InvokeLinuxShellProcess(string arguments) {

            Process rdpProcess = new Process();
            rdpProcess.StartInfo.FileName = "/bin/bash";
            rdpProcess.StartInfo.Arguments = $"-c \"{arguments}\"";
            rdpProcess.Start();
            rdpProcess.Close();

            return 1;
        }

        private static string GetRemoteClientName()
        {
            string remoteClientName = ConfigurationManager.AppSettings["REMOTE_CLIENT"];

            if (string.IsNullOrWhiteSpace(remoteClientName))
            {
                throw new ArgumentNullException("The REMOTE_CLIENT parameter is NOT configured well!");
            }

            return remoteClientName.ToUpper();
        }

        private static string GetConfiguredValue(string key) {

            string currentRemoteClient = GetRemoteClientName();
            string platformKey = $"{key.ToUpper()}_{currentRemoteClient}";

            string value = ConfigurationManager.AppSettings[platformKey];

            return value;
        }

        private static string GetCurrentOSType() {
            return RuntimeInformation.OSDescription;
        }

        #endregion
    }
}
