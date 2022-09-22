// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestSuites.Rdp.SUTControlAgent.Message
{
    public enum SUTControl_MessageType : ushort
    {
        SUT_CONTROL_REQUEST = 0x0000,
        SUT_CONTROL_RESPONSE = 0x0001
    }

    public enum SUTControl_TestsuiteId : ushort
    {
        RDP_TESTSUITE = 0x0001
    }

    public enum SUTControl_ResultCode : uint
    {
        SUCCESS = 0x00000000,
    }

    /// <summary>
    /// Payload type of SUT_Control_Request_Message with commandId: START_RDP_CONNECTION
    /// </summary>
    public enum RDP_Connect_Payload_Type : uint
    {
        RDP_FILE = 0x0000,
        PARAMETERS_STRUCT = 0x0001
    }
}
