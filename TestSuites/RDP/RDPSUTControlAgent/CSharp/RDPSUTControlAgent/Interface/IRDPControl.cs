// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


namespace RDPSUTControlAgent
{
    public interface IRDPControl
    {
        /// <summary>
        /// This interface is used to trigger an SUT Control Command.
        /// </summary>
        /// <param name="requestMessage">Command Message Sent By Driver Computer</param>
        /// <returns>Return value SUT_Control_Response_Message.</returns>
        SUT_Control_Response_Message ProcessCommand(SUT_Control_Request_Message requestMessage);
    }
}
