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
                            foreach(var registryValue in ExtraLargeDefaultPointerRegistrySet())
                            {
                                key.SetValue(registryValue.Key, registryValue.Value);
                            }
                            
                            key.Close();
                        }

                        PointerManager.EffectChange();

                        resultCode = (uint)SUTControl_ResultCode.SUCCESS;

                        break;

                    case RDPSUTControl_CommandId.SHRINK_POINTER:
                        key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Cursors", true);
                        if (key != null)
                        {
                            foreach (var registryValue in DefaultPointerRegistrySet())
                            {
                                key.SetValue(registryValue.Key, registryValue.Value);
                            }

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

        private Dictionary<string, string> DefaultPointerRegistrySet()
        {
            Dictionary<string, string> pointerRegistrySet = new Dictionary<string, string>();

            pointerRegistrySet[""] = "Windows Default";
            pointerRegistrySet["CursorBaseSize"] = "20";
            pointerRegistrySet["AppStarting"] = "%SystemRoot%\\cursors\\aero_working.ani";
            pointerRegistrySet["Arrow"] = "%SystemRoot%\\cursors\\aero_arrow.cur";
            pointerRegistrySet["Hand"] = "%SystemRoot%\\cursors\\aero_link.cur";
            pointerRegistrySet["Help"] = "%SystemRoot%\\cursors\\aero_helpsel.cur";
            pointerRegistrySet["No"] = "%SystemRoot%\\cursors\\aero_unavail.cur";
            pointerRegistrySet["NWPen"] = "%SystemRoot%\\cursors\\aero_pen.cur";
            pointerRegistrySet["Person"] = "%SystemRoot%\\cursors\\aero_person.cur";
            pointerRegistrySet["Pin"] = "%SystemRoot%\\cursors\\aero_pin.cur";
            pointerRegistrySet["SizeAll"] = "%SystemRoot%\\cursors\\aero_move.cur";
            pointerRegistrySet["SizeNESW"] = "%SystemRoot%\\cursors\\aero_nesw.cur";
            pointerRegistrySet["SizeNS"] = "%SystemRoot%\\cursors\\aero_ns.cur";
            pointerRegistrySet["SizeNWSE"] = "%SystemRoot%\\cursors\\aero_nwse.cur";
            pointerRegistrySet["SizeWE"] = "%SystemRoot%\\cursors\\aero_ew.cur";
            pointerRegistrySet["UpArrow"] = "%SystemRoot%\\cursors\\aero_up.cur";
            pointerRegistrySet["Wait"] = "%SystemRoot%\\cursors\\aero_busy.ani";

            return pointerRegistrySet;
        }

        private Dictionary<string, string> ExtraLargeDefaultPointerRegistrySet()
        {
            Dictionary<string, string> pointerRegistrySet = new Dictionary<string, string>();

            pointerRegistrySet[""] = "Windows Default (extra large)"; //
            pointerRegistrySet["CursorBaseSize"] = "100";
            pointerRegistrySet["AppStarting"] = "%SystemRoot%\\cursors\\aero_working_xl.ani";
            pointerRegistrySet["Arrow"] = "%SystemRoot%\\cursors\\aero_arrow_xl.cur";
            pointerRegistrySet["Hand"] = "%SystemRoot%\\cursors\\aero_link_xl.cur";
            pointerRegistrySet["Help"] = "%SystemRoot%\\cursors\\aero_helpsel_xl.cur";
            pointerRegistrySet["No"] = "%SystemRoot%\\cursors\\aero_unavail_xl.cur";
            pointerRegistrySet["NWPen"] = "%SystemRoot%\\cursors\\aero_pen_xl.cur";
            pointerRegistrySet["Person"] = "%SystemRoot%\\cursors\\aero_person_xl.cur";
            pointerRegistrySet["Pin"] = "%SystemRoot%\\cursors\\aero_pin_xl.cur";
            pointerRegistrySet["SizeAll"] = "%SystemRoot%\\cursors\\aero_move_xl.cur";
            pointerRegistrySet["SizeNESW"] = "%SystemRoot%\\cursors\\aero_nesw_xl.cur";
            pointerRegistrySet["SizeNS"] = "%SystemRoot%\\cursors\\aero_ns_xl.cur";
            pointerRegistrySet["SizeNWSE"] = "%SystemRoot%\\cursors\\aero_nwse_xl.cur";
            pointerRegistrySet["SizeWE"] = "%SystemRoot%\\cursors\\aero_ew_xl.cur";
            pointerRegistrySet["UpArrow"] = "%SystemRoot%\\cursors\\aero_up_xl.cur";
            pointerRegistrySet["Wait"] = "%SystemRoot%\\cursors\\aero_busy_xl.ani";

            return pointerRegistrySet;
        }
    }
}
