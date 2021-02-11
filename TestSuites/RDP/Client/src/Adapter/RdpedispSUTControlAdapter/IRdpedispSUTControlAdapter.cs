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
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        [MethodHelp("Please change screen resolution of the client and maximize the windows of RDP client.\r\n\r\n" +
                    "Note: please finish the operation in 10 seconds otherwise the case will fail with timeout." )]
        [DefaultValue("1")]
        int TriggerResolutionChangeOnClient(string caseName, ushort width, ushort height);

        /// <summary>
        /// This method is used to trigger screen orientation change on the client.
        /// </summary>
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        [MethodHelp("Please change screen orientation of the client and maximize the window of RDP client.\r\n\r\n" +
                    "Orientaion Value: Landscape(0) Portrait(1) Landscape Flipped(2) Portrait Flipped(3).\r\n\r\n" +
                    "Note: please finish the operation in 10 seconds otherwise the case will fail with timeout." )]
        [DefaultValue("1")]
        int TriggerOrientationChangeOnClient(string caseName, int orientation);

        /// <summary>
        /// This method is used to trigger client to initialize display settings.
        /// </summary>
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        [MethodHelp("Please set screen resolution and orientation to default.\r\n\r\n" +
                    "Orientaion Value: Landscape(0) Portrait(1) Landscape Flipped(2) Portrait Flipped(3).\r\n\r\n" +
                    "Note: please finish the operation in 10 seconds otherwise the case will fail with timeout." )]
        [DefaultValue("1")]
        int TriggerInitializeDisplaySettings(string caseName, ushort width, ushort height, int orientation);

        /// <summary>
        /// This method is used to trigger screen addition or removal on the client.
        /// </summary>
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        [MethodHelp("Please add or remove monitors of the client and maximize the windows of RDP client.\r\n\r\n" +
                    "Note: please finish the operation in 10 seconds otherwise the case will fail with timeout." )]
        [DefaultValue("1")]
        int TriggerMonitorAdditionRemovalOnClient(string caseName, String Action);

        /// <summary>
        /// This method is used to trigger repositioning of monitors on the client.
        /// </summary>
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        [MethodHelp("Please move positons of monitors and maximize the windows of RDP client. Make sure there are more than two monitors in client.\r\n\r\n" +
                    "Note: please finish the operation in 10 seconds otherwise the case will fail with timeout." )]
        [DefaultValue("1")]
        int TriggerMonitorReposition(string caseName, String Action);

        /// <summary>
        /// This method is used to trigger client to maximize RDP client window.
        /// </summary>
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        [MethodHelp("Please maximize the RDP Client window.\r\n\r\n" +
                    "Note: please finish the operation in 10 seconds otherwise the case will fail with timeout." )]
        [DefaultValue("1")]
        int TriggerMaximizeRDPClientWindow(string caseName);
    }
}
