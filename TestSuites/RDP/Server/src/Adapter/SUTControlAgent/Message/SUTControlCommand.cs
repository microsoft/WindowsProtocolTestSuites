using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestSuites.Rdp.SUTControlAgent.Message
{
    /// <summary>
    /// Command for commandId field of SUTControlRequestMessage
    /// </summary>
    public enum RDPSUTControlCommand : ushort
    {
        // For RDP SUT Control Command
        ENLARGE_POINTER = 0x0101,
        SHRINK_POINTER = 0x0102,
    }
}
