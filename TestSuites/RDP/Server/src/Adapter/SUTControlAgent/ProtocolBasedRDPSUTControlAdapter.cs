// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.Rdp.SUTControlAgent.Handler;
using Microsoft.Protocols.TestSuites.Rdp.SUTControlAgent.Message;
using Microsoft.Protocols.TestTools;
using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;

namespace Microsoft.Protocols.TestSuites.Rdp
{
    public class ProtocolBasedRDPSUTControlAdapter : ManagedAdapterBase, IRDPSUTControlAdapter
    {

        #region variables

        const string interfaceFullName = "Microsoft.Protocols.TestSuites.Rdp.IRDPSUTControlAdapter";
        const string basicRDPFileString = "session bpp:i:32\nconnection type:i:6\nauthentication level:i:0\nuse redirection server name:i:1\n";

        private bool isClientSupportCompression = false;

        string rdpServerIP;
        ushort localPort;
        RDP_Connect_Payload_Type connectPayloadType;
        SUTControlProtocolHandler controlHandler;

        #endregion variables

        /// <summary>
        /// Get IP address of local host
        /// </summary>
        /// <returns>string for local IP address</returns>
        private string GetLocalIP()
        {
            foreach (IPAddress ip in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return null;
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
            string proxyIP;
            PtfPropUtility.GetPtfPropertyValue(testSite, "ProxyIP", out proxyIP);
            if (string.IsNullOrEmpty(proxyIP))
            {
                rdpServerIP = GetLocalIP();
            }
            else
            {
                rdpServerIP = proxyIP;
            }

            // Initiate local port
            try
            {
                string localPortString;
                PtfPropUtility.GetPtfPropertyValue(testSite, "ServerPort", out localPortString);
                localPort = ushort.Parse(localPortString);
            }
            catch
            {
                localPort = 0;
            }

            // Initiate Connect payload type
            bool clientSupportRDPFile;
            PtfPropUtility.GetPtfPropertyValue(testSite, "ClientSupportRDPFile", out clientSupportRDPFile, new string[] { RdpPtfGroupNames.SUTControl });

            if (clientSupportRDPFile)
            {
                connectPayloadType = RDP_Connect_Payload_Type.RDP_FILE;
            }
            else
            {
                connectPayloadType = RDP_Connect_Payload_Type.PARAMETERS_STRUCT;
            }

            PtfPropUtility.GetPtfPropertyValue(testSite, "SupportCompression", out isClientSupportCompression);
        }


        /// <summary>
        /// This method is used to trigger an increase in the size of the pointer on the server.
        /// </summary>
        /// <param name="caseName">Name of test case</param>
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        public int PointerIncreaseSize(string caseName)
        {
            return OperateSUTControl(caseName, (ushort)RDPSUTControlCommand.ENLARGE_POINTER_SIZE);
        }

        /// <summary>
        /// This method is used to trigger movement of the pointer on the server.
        /// </summary>
        /// <param name="caseName">Name of test case</param>
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        public int PointerTriggerMotion(string caseName)
        {
            return OperateSUTControl(caseName, (ushort)RDPSUTControlCommand.CHANGE_POINTER_POSITION);
        }

        /// <summary>
        /// This method is used to reverse the pointer to its default size on the server.
        /// </summary>
        /// <param name="caseName">Name of test case</param>
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        public int PointerReverseToDefaultSize(string caseName)
        {
            return OperateSUTControl(caseName, (ushort)RDPSUTControlCommand.REVERT_POINTER_SIZE);
        }

        /// <summary>
        /// Get Help message from MethodHelp defined in an interface.
        /// </summary>
        /// <param name="interfaceFullName">Fullname of interfaces implemented by current class</param>
        /// <returns>Help message of a SUT Control function</returns>
        private static string GetHelpMessage(string interfaceFullName)
        {
            string helpMessage = "";
            try
            {
                // Get method name
                StackTrace trace = new StackTrace();
                string methodName = trace.GetFrame(1).GetMethod().Name;

                // Get corresponding method defined in interface
                Assembly assembly = Assembly.GetCallingAssembly();
                Type adapterInterface = assembly.GetType(interfaceFullName);
                MethodBase method = adapterInterface.GetMethod(methodName);

                // Get and Return help message
                object[] attrArray = method.GetCustomAttributes(typeof(MethodHelpAttribute), true);

                foreach (object attr in attrArray)
                {
                    MethodHelpAttribute helpAttr = (MethodHelpAttribute)attr;
                    helpMessage += helpAttr.HelpMessage;
                }
            }
            catch { }

            return helpMessage;
        }

        /// <summary>
        /// This private method is used to trigger an the operate SUT control.
        /// </summary>
        /// <param name="caseName">Name of test case</param>
        /// <param name="RDPSUTControlCommand">RDP SUT Control Command to be executed</param>
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        private int OperateSUTControl(string caseName, ushort RDPSUTControlCommand)
        {
            // Get help message
            string helpMessage = GetHelpMessage(interfaceFullName);
            // Create payload
            byte[] payload = null;

            // Create request message
            ushort reqId = controlHandler.GetNextRequestId();
            SUTControlRequestMessage requestMessage = new SUTControlRequestMessage(SUTControl_TestsuiteId.RDP_TESTSUITE, RDPSUTControlCommand, caseName, reqId, helpMessage, payload);

            //Send the request and get response if necessary
            byte[] responsePayload = null;
            return controlHandler.OperateSUTControl(requestMessage, false, out responsePayload);
        }
    }
}
