using Microsoft.Protocols.TestSuites.Rdp.SUTControlAgent.Handler;
using Microsoft.Protocols.TestSuites.Rdp.SUTControlAgent.Message;
using Microsoft.Protocols.TestTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestSuites.Rdp.SUTControlAgent
{
    public class ProtocolBasedRDPSUTControlAdapter : ManagedAdapterBase, IRDPSUTControlAdapter
    {

        #region variables

        const string interfaceFullName = "Microsoft.Protocols.TestSuites.Rdp.IRdpSutControlAdapter";
        const string basicRDPFileString = "session bpp:i:32\nconnection type:i:6\nauthentication level:i:0\nuse redirection server name:i:1\n";

        private bool isClientSupportCompression = false;

        string rdpServerIP;
        ushort localPort;
        RDP_Connect_Payload_Type connectPayloadType;
        SUTControlProtocolHandler controlHandler;

        #endregion variables

        /// <summary>
        /// This method is used to trigger an increase in the size of the pointer on the server.
        /// </summary>
        /// <param name="caseName">Name of test case</param>
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        public int PointerIncreaseSize(string caseName)
        {
            return OpertateSUTControl(caseName, (ushort)RDPSUTControlCommand.ENLARGE_POINTER);
        }

        /// <summary>
        /// This method is used to reverse the pointer to its default size on the server.
        /// </summary>
        /// <param name="caseName">Name of test case</param>
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        public int PointerReverseToDefaultSize(string caseName)
        {
            return OpertateSUTControl(caseName, (ushort)RDPSUTControlCommand.SHRINK_POINTER);
        }

        /// <summary>
        /// Get Help message from MethodHelp defined in an interface.
        /// </summary>
        /// <param name="interfaceFullName">Fullname of interfaces implemented by current class</param>
        /// <returns>Help message of a SUT Control function</returns>
        private static string GetHelpMessage(string interfaceFullName)
        {
            string helpMessage = "";
            try
            {
                // Get method name
                StackTrace trace = new StackTrace();
                string methodName = trace.GetFrame(1).GetMethod().Name;

                // Get corresponding method defined in interface
                Assembly assembly = Assembly.GetCallingAssembly();
                Type adapterInterface = assembly.GetType(interfaceFullName);
                MethodBase method = adapterInterface.GetMethod(methodName);

                // Get and Return help message
                object[] attrArray = method.GetCustomAttributes(typeof(MethodHelpAttribute), true);

                foreach (object attr in attrArray)
                {
                    MethodHelpAttribute helpAttr = (MethodHelpAttribute)attr;
                    helpMessage += helpAttr.HelpMessage;
                }
            }
            catch { }

            return helpMessage;
        }

        /// <summary>
        /// This private method is used to trigger an the operate SUT control.
        /// </summary>
        /// <param name="caseName">Name of test case</param>
        /// <param name="RDPSUTControlCommand">RDP SUT Control Command to be executed</param>
        /// <returns>Return value 1 indicates the operation is succesful, otherwise, failed.</returns>
        private int OpertateSUTControl(string caseName, ushort RDPSUTControlCommand)
        {
            // Get help message
            string helpMessage = GetHelpMessage(interfaceFullName);
            // Create payload
            byte[] payload = null;

            // Create request message
            ushort reqId = controlHandler.GetNextRequestId();
            SUTControlRequestMessage requestMessage = new SUTControlRequestMessage(SUTControl_TestsuiteId.RDP_TESTSUITE, RDPSUTControlCommand, caseName, reqId, helpMessage, payload);

            //Send the request and get response if necessary
            byte[] resposePayload = null;
            return controlHandler.OperateSUTControl(requestMessage, false, out resposePayload);
        }
    }
}
