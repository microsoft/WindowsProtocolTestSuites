using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDPSUTControlAgent
{
    public class RDPServerControl : IRDPControl
    {
        public static uint WaitSeconds = 5;
        public static PostFunction PostOperation = null;
        /// <summary>
        /// Process SUT Control Command
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public SUT_Control_Response_Message ProcessCommand(SUT_Control_Request_Message requestMessage)
        {
            PostOperation = null;
            if (requestMessage == null)
            {
                throw new ArgumentNullException("SUT_Control_Request_Message inputed is null");
            }
            if (requestMessage.messageType != SUTControl_MessageType.SUT_CONTROL_REQUEST || requestMessage.testsuiteId != SUTControl_TestsuiteId.RDP_TESTSUITE)
            {
                throw new ArgumentException("Not available request message." + requestMessage.messageType + "," + requestMessage.testsuiteId);
            }

            RDPSUTControl_CommandId commandId = (RDPSUTControl_CommandId)requestMessage.commandId;
            byte[] payload = null;
            string errorMessage = null;
            uint resultCode = 1;
            RegistryKey key;

            try
            {
                switch (commandId)
                {
                    case RDPSUTControl_CommandId.ENLARGE_POINTER:
                        key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Cursors",true);
                        if (key != null)
                        {
                            key.SetValue("", "Windows Standard (extra large)");
                            key.Close();
                        }

                        PointerManager.EffectChange();

                        resultCode = (uint)SUTControl_ResultCode.SUCCESS;

                        break;

                    case RDPSUTControl_CommandId.SHRINK_POINTER:
                        key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Cursors", true);
                        if (key != null)
                        {
                            key.SetValue("", "Windows Standard");
                            key.Close();
                        }

                        PointerManager.EffectChange();

                        resultCode = (uint)SUTControl_ResultCode.SUCCESS;
                        break;

                    
                    default:
                        errorMessage = "SUT control agent doesn't support this command: " + commandId;
                        break;
                }
            }
            catch (Exception e)
            {
                errorMessage = "Exception found when process " + commandId + "," + e.Message;
            }

            SUT_Control_Response_Message responseMessage = new SUT_Control_Response_Message(SUTControl_TestsuiteId.RDP_TESTSUITE, (ushort)commandId, requestMessage.caseName, requestMessage.requestId, resultCode, errorMessage, payload);
            return responseMessage;
        }

    }
}
