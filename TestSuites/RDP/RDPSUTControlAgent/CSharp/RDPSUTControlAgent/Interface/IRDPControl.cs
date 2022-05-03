using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
