// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestSuites.Rdpbcgr;
using Microsoft.Protocols.TestSuites.Rdprfx;


namespace Microsoft.Protocols.TestSuites.Rdpei
{
    /// <summary>
    /// The SUT Control Adapter for Remote Touch Testing.
    /// </summary>
    public interface IRdpeiSUTControlAdapter : IAdapter
    {

        /// <summary>
        /// This method is used to trigger one touch event on the client.
        /// </summary>
        /// <param name="caseName">Name of test case</param>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        [MethodHelp("1.Touch the screen of the client.\r\n\r\n" +
                    "2.Enter a return value, using a positive value for a successful operation or a negative value for a failed operation.\r\n\r\n" +
                    "3.To pass the value to the test case, click the Succeed button. Or, to end the test case, enter a message into the Failure Message dialog box and then click Fail.")]
        [DefaultValue("1")]
        int TriggerOneTouchEventOnClient(string caseName);

        /// <summary>
        /// This method is used to trigger continuous touch events on the client.
        /// </summary>
        /// <param name="caseName">Name of test case</param>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        [MethodHelp("1.Touch the screen several times to trigger touch events (at least touch 5 times).\r\n\r\n" +
                    "2.Enter a return value, using a positive value for a successful operation or a negative value for a failed operation.\r\n\r\n" +
                    "3.To pass the value to the test case, click the Succeed button. Or, to end the test case, enter a message into the Failure Message dialog box and then click Fail.")]
        [DefaultValue("1")]
        int TriggerContinuousTouchEventOnClient(string caseName);

        /// <summary>
        /// This method is used to trigger multitouch events on the client.
        /// </summary>
        /// <param name="caseName">Name of test case</param>
        /// <param name="contactCount">The number of multitouch contacts.</param>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        [MethodHelp("1.Touch the screen of the client with multiple touch points, the number of touch points is specified in the parameter contactCout.\r\n\r\n" +
                    "2.Enter a return value, using a positive value for a successful operation or a negative value for a failed operation.\r\n\r\n" +
                    "3.To pass the value to the test case, click the Succeed button. Or, to end the test case, enter a message into the Failure Message dialog box and then click Fail.")]
        [DefaultValue("1")]
        int TriggerMultiTouchEventOnClient(string caseName, ushort contactCount);

        /// <summary>
        /// This method is only used by managed adapter. This method is used to touch events at specified position. 
        /// </summary>
        /// <param name="caseName">Name of test case</param>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        int TriggerPositionSpecifiedTouchEventOnClient(string caseName);

        /// <summary>
        /// This method is used to trigger the RDPINPUT_DISMISS_HOVERING_CONTACT_PDU message.
        /// </summary>
        /// <param name="caseName">Name of test case</param>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        [MethodHelp("1.If your device supports proximity, trigger the RDPINPUT_DISMISS_HOVERING_CONTACT_PDU message on client, and enter a positive return value. \r\n\r\n" +
                    "2.If your device does not support proximity, enter a negative return value.\r\n\r\n" +
                    "3.To pass the value to the test case, click the Succeed button. Or, to end the test case, enter a message into the Failure Message dialog box and then click Fail.")]
        [DefaultValue("1")]
        int TriggerDismissHoveringContactPduOnClient(string caseName);
    }
}
