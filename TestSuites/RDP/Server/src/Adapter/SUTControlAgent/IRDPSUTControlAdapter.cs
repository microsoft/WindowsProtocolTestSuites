// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.Rdp
{
    /// <summary>
    /// The interface of IRDPSUTControlAdapter, which defines public methods
    /// for interacting with the RDP Server in test case running time.
    /// </summary>
    public interface IRDPSUTControlAdapter : IAdapter
    {
        /// <summary>
        /// This method is used to trigger an increase in the size of the pointer on the server.
        /// </summary>
        /// <param name="caseName">Name of test case</param>
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        [MethodHelp("Initiate the increase in the size of the pointer.\r\n\r\n" +
                    "Note: please finish the operation in 10 seconds otherwise the case will fail with timeout.")]
        [DefaultValue("1")]
        int PointerIncreaseSize(string caseName);

        /// <summary>
        /// This method is used to trigger movement of the pointer on the server.
        /// </summary>
        /// <param name="caseName">Name of test case</param>
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        [MethodHelp("Initiate the movement of the pointer.\r\n\r\n" +
                    "Note: please finish the operation in 10 seconds otherwise the case will fail with timeout.")]
        [DefaultValue("1")]
        int PointerTriggerMotion(string caseName);

        /// <summary>
        /// This method is used to reverse the pointer to its default size on the server.
        /// </summary>
        /// <param name="caseName">Name of test case</param>
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        [MethodHelp("Initiate the reversal of the size of the pointer back to default size.\r\n\r\n" +
                    "Note: please finish the operation in 10 seconds otherwise the case will fail with timeout.")]
        [DefaultValue("1")]
        int PointerReverseToDefaultSize(string caseName);
    }
}
