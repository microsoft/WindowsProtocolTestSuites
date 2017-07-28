// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestSuites.Rdpedisp;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.Rdp
{
    public class ProtocolBasedRdpedispSUTControlAdapter : ManagedAdapterBase, IRdpedispSUTControlAdapter
    {

        #region variables

        const string interfaceFullName = "Microsoft.Protocols.TestSuites.Rdpedisp.IRdpedispSUTControlAdapter";

        SUTControlProtocolHandler controlHandler;

        #endregion variables

        /// <summary>
        /// Initialize this adapter
        /// </summary>
        /// <param name="testSite">testSite instance</param>
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
            controlHandler = SUTControlProtocolHandler.GetInstance(testSite);
        }

        /// <summary>
        /// This method is used to trigger screen resolution change on the client.
        /// </summary>
        /// <param name="width">width of new desktop resolution</param>
        /// <param name="height">height of new desktop resolution</param>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        public int TriggerResolutionChangeOnClient(string caseName, ushort width, ushort height)
        {
            // Get help message
            string helpMessage = CommonUtility.GetHelpMessage(interfaceFullName);
            // Create payload
            List<byte> payloadList = new List<byte>();

            // Add operation 
            byte[] operationBytes = BitConverter.GetBytes((uint)RDPEDISP_Update_Resolution_Operation.UPDATE_RESOLUTION);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(operationBytes);
            }
            payloadList.AddRange(operationBytes);
            // Add width and height
            byte[] widthBytes = BitConverter.GetBytes(width);
            byte[] heightBytes = BitConverter.GetBytes(height);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(widthBytes);
                Array.Reverse(heightBytes);
            }
            payloadList.AddRange(widthBytes);
            payloadList.AddRange(heightBytes);

            // Create request message
            ushort reqId = controlHandler.GetNextRequestId();
            SUT_Control_Request_Message requestMessage = new SUT_Control_Request_Message(SUTControl_TestsuiteId.RDP_TESTSUITE, (ushort)RDPSUTControl_CommandId.DISPLAY_UPDATE_RESOLUTION, caseName,
                reqId, helpMessage, payloadList.ToArray());

            //Send the request and get response if necessary
            byte[] resposePayload = null;
            return controlHandler.OperateSUTControl(requestMessage, false, out resposePayload);
        }

        /// <summary>
        /// This method is used to trigger screen orientation change on the client.
        /// </summary>
        /// <param name="orientation">new orientation</param>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        public int TriggerOrientationChangeOnClient(string caseName,int orientation)
        {
            // Get help message
            string helpMessage = CommonUtility.GetHelpMessage(interfaceFullName);
            // Create payload
            List<byte> payloadList = new List<byte>();
            // add operation
            byte[] operationBytes = BitConverter.GetBytes((uint)RDPEDISP_Update_Resolution_Operation.UPDATE_ORIENTATION);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(operationBytes);
            }
            payloadList.AddRange(operationBytes);
            // add orientation
            byte[] orientationBytes = BitConverter.GetBytes((uint)orientation);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(orientationBytes);
            }
            payloadList.AddRange(orientationBytes);

            // Create request message
            ushort reqId = controlHandler.GetNextRequestId();
            SUT_Control_Request_Message requestMessage = new SUT_Control_Request_Message(SUTControl_TestsuiteId.RDP_TESTSUITE, (ushort)RDPSUTControl_CommandId.DISPLAY_UPDATE_RESOLUTION, caseName,
                reqId, helpMessage, payloadList.ToArray());

            //Send the request and get response if necessary
            byte[] resposePayload = null;
            return controlHandler.OperateSUTControl(requestMessage, false, out resposePayload);
        }

        /// <summary>
        /// This method is used to trigger client to initialize display settings.
        /// </summary>
        /// <param name="width">width of new desktop resolution</param>
        /// <param name="height">height of new desktop resolution</param>
        /// <param name="orientation">new orientation</param>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        public int TriggerInitializeDisplaySettings(string caseName, ushort width, ushort height, int orientation)
        {
            // Get help message
            string helpMessage = CommonUtility.GetHelpMessage(interfaceFullName);
            // Create payload
            List<byte> payloadList = new List<byte>();
            // add operation
            byte[] operationBytes = BitConverter.GetBytes((uint)(RDPEDISP_Update_Resolution_Operation.UPDATE_RESOLUTION | RDPEDISP_Update_Resolution_Operation.UPDATE_ORIENTATION));
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(operationBytes);
            }
            payloadList.AddRange(operationBytes);
            // Add width and height
            byte[] widthBytes = BitConverter.GetBytes(width);
            byte[] heightBytes = BitConverter.GetBytes(height);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(widthBytes);
                Array.Reverse(heightBytes);
            }
            payloadList.AddRange(widthBytes);
            payloadList.AddRange(heightBytes);
            // Add orientation
            byte[] orientationBytes = BitConverter.GetBytes((uint)orientation);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(orientationBytes);
            }
            payloadList.AddRange(orientationBytes);

            // Create request message
            ushort reqId = controlHandler.GetNextRequestId();
            SUT_Control_Request_Message requestMessage = new SUT_Control_Request_Message(SUTControl_TestsuiteId.RDP_TESTSUITE, (ushort)RDPSUTControl_CommandId.DISPLAY_UPDATE_RESOLUTION, caseName,
                reqId, helpMessage, payloadList.ToArray());

            //Send the request and get response if necessary
            byte[] resposePayload = null;
            return controlHandler.OperateSUTControl(requestMessage, false, out resposePayload);
        }

        /// <summary>
        /// This method is used to trigger screen addition or removal on the client.
        /// </summary>
        /// <param name="Action">Action of monitor updates: "Add a monitor" or "Remove a monitor"</param>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        public int TriggerMonitorAdditionRemovalOnClient(string caseName, String Action)
        {
            // Get help message
            string helpMessage = CommonUtility.GetHelpMessage(interfaceFullName);
            // Create payload
            RDPEDISP_Update_Monitors_Operation operation = RDPEDISP_Update_Monitors_Operation.ADD_MONITOR;
            if (Action.ToLower().Equals("remove a monitor"))
            {
                operation = RDPEDISP_Update_Monitors_Operation.REMOVE_MONITOR;
            }
            byte[] payload = BitConverter.GetBytes((uint)operation);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(payload);
            }
            
            // Create request message
            ushort reqId = controlHandler.GetNextRequestId();
            SUT_Control_Request_Message requestMessage = new SUT_Control_Request_Message(SUTControl_TestsuiteId.RDP_TESTSUITE, (ushort)RDPSUTControl_CommandId.DISPLAY_UPDATE_MONITORS, caseName,
                reqId, helpMessage, payload);

            //Send the request and get response if necessary
            byte[] resposePayload = null;
            return controlHandler.OperateSUTControl(requestMessage, false, out resposePayload);
        }

        /// <summary>
        /// This method is used to trigger repositioning of monitors on the client.
        /// </summary>
        /// <param name="Action">Action of monitor position update: "Move position of monitors"</param>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        public int TriggerMonitorReposition(string caseName, String Action)
        {
            // Get help message
            string helpMessage = CommonUtility.GetHelpMessage(interfaceFullName);
            // Create payload
            RDPEDISP_Update_Monitors_Operation operation = RDPEDISP_Update_Monitors_Operation.MOVE_MONITOR_POSITION;
            byte[] payload = BitConverter.GetBytes((uint)operation);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(payload);
            }

            // Create request message
            ushort reqId = controlHandler.GetNextRequestId();
            SUT_Control_Request_Message requestMessage = new SUT_Control_Request_Message(SUTControl_TestsuiteId.RDP_TESTSUITE, (ushort)RDPSUTControl_CommandId.DISPLAY_UPDATE_MONITORS, caseName,
                reqId, helpMessage, payload);

            //Send the request and get response if necessary
            byte[] resposePayload = null;
            return controlHandler.OperateSUTControl(requestMessage, false, out resposePayload);
        }

        /// <summary>
        /// This method is used to trigger client to maximize RDP client window.
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        public int TriggerMaximizeRDPClientWindow(string caseName)
        {
            // Get help message
            string helpMessage = CommonUtility.GetHelpMessage(interfaceFullName);
            // Create payload
            byte[] payload = null;

            // Create request message
            ushort reqId = controlHandler.GetNextRequestId();
            SUT_Control_Request_Message requestMessage = new SUT_Control_Request_Message(SUTControl_TestsuiteId.RDP_TESTSUITE, (ushort)RDPSUTControl_CommandId.DISPLAY_FULLSCREEN, caseName,
                reqId, helpMessage, payload);

            //Send the request and get response if necessary
            byte[] resposePayload = null;
            return controlHandler.OperateSUTControl(requestMessage, false, out resposePayload);
        }
    }
}
