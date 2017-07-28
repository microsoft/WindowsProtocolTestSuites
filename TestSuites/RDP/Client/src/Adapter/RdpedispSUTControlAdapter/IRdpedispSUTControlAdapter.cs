// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.Rdpedisp
{
    public interface IRdpedispSUTControlAdapter : IAdapter
    {
        /// <summary>
        /// This method is used to trigger screen resolution change on the client.
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        [MethodHelp("1.Change screen resolution of the client and maximize the windows of RDP client.\r\n\r\n" +
                    "2.Enter a return value, using a nonnegative value for a successful operation or a negative value for a failed operation.\r\n\r\n" +
                    "3.To pass the value to the test case, click the Succeed button. Or, to end the test case, enter a message into the Failure Message dialog box and then click Fail.")]
        [DefaultValue("1")]
        int TriggerResolutionChangeOnClient(string caseName, ushort width, ushort height);

        /// <summary>
        /// This method is used to trigger screen orientation change on the client.
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        [MethodHelp("1.Change screen orientation of the client and maximize the windows of RDP client.\r\n\r\n" +
                    "2.Orientaion Value: Landscape(0) Portrait(1) Landscape Flipped(2) Portrait Flipped(3).\r\n\r\n" +
                    "3.Enter a return value, using a nonnegative value for a successful operation or a negative value for a failed operation.\r\n\r\n" +
                    "4.To pass the value to the test case, click the Succeed button. Or, to end the test case, enter a message into the Failure Message dialog box and then click Fail.")]
        [DefaultValue("1")]
        int TriggerOrientationChangeOnClient(string caseName, int orientation);

        /// <summary>
        /// This method is used to trigger client to initialize display settings.
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        [MethodHelp("1.Set screen resolution and orientation to default.\r\n\r\n" +
                    "2.Orientaion Value: Landscape(0) Portrait(1) Landscape Flipped(2) Portrait Flipped(3).\r\n\r\n" +
                    "3.Enter a return value, using a nonnegative value for a successful operation or a negative value for a failed operation.\r\n\r\n" +
                    "4.To pass the value to the test case, click the Succeed button. Or, to end the test case, enter a message into the Failure Message dialog box and then click Fail.")]
        [DefaultValue("1")]
        int TriggerInitializeDisplaySettings(string caseName, ushort width, ushort height, int orientation);

        /// <summary>
        /// This method is used to trigger screen addition or removal on the client.
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        [MethodHelp("1.Add or remove monitors of the client and maximize the windows of RDP client.\r\n\r\n" +
                    "2.Enter a return value, using a nonnegative value for a successful operation or a negative value for a failed operation.\r\n\r\n" +
                    "3.To pass the value to the test case, click the Succeed button. Or, to end the test case, enter a message into the Failure Message dialog box and then click Fail.")]
        [DefaultValue("1")]
        int TriggerMonitorAdditionRemovalOnClient(string caseName, String Action);

        /// <summary>
        /// This method is used to trigger repositioning of monitors on the client.
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        [MethodHelp("1.Move positons of monitors and maximize the windows of RDP client. Make sure there are more than two monitors in client.\r\n\r\n" +
                    "2.Enter a return value, using a nonnegative value for a successful operation or a negative value for a failed operation.\r\n\r\n" +
                    "3.To pass the value to the test case, click the Succeed button. Or, to end the test case, enter a message into the Failure Message dialog box and then click Fail.")]
        [DefaultValue("1")]
        int TriggerMonitorReposition(string caseName, String Action);

        /// <summary>
        /// This method is used to trigger client to maximize RDP client window.
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        [MethodHelp("1.Maximize the RDP Client window.\r\n\r\n" +
                    "2.Enter a return value, using a nonnegative value for a successful operation or a negative value for a failed operation.\r\n\r\n" +
                    "3.To pass the value to the test case, click the Succeed button. Or, to end the test case, enter a message into the Failure Message dialog box and then click Fail.")]
        [DefaultValue("1")]
        int TriggerMaximizeRDPClientWindow(string caseName);
    }
}
