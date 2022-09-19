// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


namespace Microsoft.Protocols.TestSuites.Rdp.SUTControlAgent.Message
{
    /// <summary>
    /// Command for commandId field of SUTControlRequestMessage
    /// </summary>
    public enum RDPSUTControlCommand : ushort
    {
        // For RDP SUT Control Command
        ENLARGE_POINTER_SIZE = 0x0101,
        REVERT_POINTER_SIZE = 0x0102,
        CHANGE_POINTER_POSITION = 0x0103,
    }
}
