// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestSuites;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestSuites.Rdpei;
using System.Drawing;

namespace Microsoft.Protocols.TestSuites.Rdp
{
    public class ProtocolBasedRdpeiSUTControlAdapter : ManagedAdapterBase, IRdpeiSUTControlAdapter
    {

        #region variables

        const string interfaceFullName = "Microsoft.Protocols.TestSuites.Rdpei.IRdpeiSUTControlAdapter";

        const uint singleTouchTimes = 5;
        const uint multiTouchCount = 5;
        SUTControlProtocolHandler controlHandler;

        #endregion variables


        /// <summary>
        /// Initialize this adapter
        /// </summary>
        /// <param name="testSite"></param>
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
            controlHandler = SUTControlProtocolHandler.GetInstance(testSite);            
        }

        /// <summary>
        /// This method is used to trigger one touch event on the client.
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        public int TriggerOneTouchEventOnClient(string caseName)
        {
            // Get help message
            string helpMessage = CommonUtility.GetHelpMessage(interfaceFullName);
            // Create payload
            uint inputTimes = 1;
            byte[] payload = BitConverter.GetBytes((uint)inputTimes);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(payload);
            }

            // Create request message
            ushort reqId = controlHandler.GetNextRequestId();
            SUT_Control_Request_Message requestMessage = new SUT_Control_Request_Message(SUTControl_TestsuiteId.RDP_TESTSUITE, (ushort)RDPSUTControl_CommandId.TOUCH_EVENT_SINGLE, caseName,
                reqId, helpMessage, payload);

            //Send the request and get response if necessary
            byte[] resposePayload = null;
            return controlHandler.OperateSUTControl(requestMessage, false, out resposePayload);
        }

        /// <summary>
        /// This method is used to trigger continuous touch events on the client.
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        public int TriggerContinuousTouchEventOnClient(string caseName)
        {
            // Get help message
            string helpMessage = CommonUtility.GetHelpMessage(interfaceFullName);
            // Create payload
            byte[] payload = BitConverter.GetBytes((uint)singleTouchTimes);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(payload);
            }

            // Create request message
            ushort reqId = controlHandler.GetNextRequestId();
            SUT_Control_Request_Message requestMessage = new SUT_Control_Request_Message(SUTControl_TestsuiteId.RDP_TESTSUITE, (ushort)RDPSUTControl_CommandId.TOUCH_EVENT_SINGLE, caseName,
                reqId, helpMessage, payload);

            //Send the request and get response if necessary
            byte[] resposePayload = null;
            return controlHandler.OperateSUTControl(requestMessage, false, out resposePayload);
        }

        /// <summary>
        /// This method is used to trigger multitouch events on the client.
        /// </summary>
        /// <param name="caseName">The name of the calling test case.</param>
        /// <param name="contactCount">The number of multitouch contacts.</param>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        public int TriggerMultiTouchEventOnClient(string caseName, ushort contactCount)
        {
            // Get help message
            string helpMessage = CommonUtility.GetHelpMessage(interfaceFullName);
            // Create payload
            byte[] payload = BitConverter.GetBytes((uint)multiTouchCount);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(payload);
            }

            // Create request message
            ushort reqId = controlHandler.GetNextRequestId();
            SUT_Control_Request_Message requestMessage = new SUT_Control_Request_Message(SUTControl_TestsuiteId.RDP_TESTSUITE, (ushort)RDPSUTControl_CommandId.TOUCH_EVENT_MULTIPLE, caseName,
                reqId, helpMessage, payload);

            //Send the request and get response if necessary
            byte[] resposePayload = null;
            return controlHandler.OperateSUTControl(requestMessage, false, out resposePayload);
        }

        /// <summary>
        /// This method is only used by managed adapter. This method is used to touch events at specified position. 
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        public int TriggerPositionSpecifiedTouchEventOnClient(string caseName)
        {
            // this interface need some updates, should contains an input 
            Point[] points = new Point[5];
            // Get help message
            string helpMessage = CommonUtility.GetHelpMessage(interfaceFullName);
            // Create payload
            List<byte> payloadList = new List<byte>();
            byte[] valueByte = BitConverter.GetBytes((uint)points.Length);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(valueByte);
            }
            payloadList.AddRange(valueByte);

            foreach (Point point in points)
            {
                byte[] xBytes = BitConverter.GetBytes(point.X);
                byte[] yBytes = BitConverter.GetBytes(point.Y);
                if (!BitConverter.IsLittleEndian)
                {
                    Array.Reverse(xBytes);
                    Array.Reverse(yBytes);
                }
                payloadList.AddRange(xBytes);
                payloadList.AddRange(yBytes);
            }
            byte[] payload = payloadList.ToArray();
            // Create request message
            ushort reqId = controlHandler.GetNextRequestId();
            SUT_Control_Request_Message requestMessage = new SUT_Control_Request_Message(SUTControl_TestsuiteId.RDP_TESTSUITE, (ushort)RDPSUTControl_CommandId.TOUCH_EVENT_SINGLE, caseName,
                reqId, helpMessage, payload);

            //Send the request and get response if necessary
            byte[] resposePayload = null;
            return controlHandler.OperateSUTControl(requestMessage, false, out resposePayload);
        }

        /// <summary>
        /// This method is used to trigger the RDPINPUT_DISMISS_HOVERING_CONTACT_PDU message.
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        public int TriggerDismissHoveringContactPduOnClient(string caseName)
        {
            // Get help message
            string helpMessage = CommonUtility.GetHelpMessage(interfaceFullName);
            // Create payload            
            byte[] payload = null;

            // Create request message
            ushort reqId = controlHandler.GetNextRequestId();
            SUT_Control_Request_Message requestMessage = new SUT_Control_Request_Message(SUTControl_TestsuiteId.RDP_TESTSUITE, (ushort)RDPSUTControl_CommandId.TOUCH_EVENT_DISMISS_HOVERING_CONTACT, caseName,
                reqId, helpMessage, payload);

            //Send the request and get response if necessary
            byte[] resposePayload = null;
            return controlHandler.OperateSUTControl(requestMessage, false, out resposePayload);
        }
    }
}
