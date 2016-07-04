// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeusb
{
    /// <summary>
    /// This class is used to parse the TsUrb Types.
    /// </summary>
    public class RdpeusbUrbParser
    {
        protected URB_FUNCTIONID GetTsUrbType(byte[] data)
        {
            if (null == data) return URB_FUNCTIONID.UNKNOWN;

            TS_URB urb = new TS_URB();
            if (!PduMarshaler.Unmarshal(data, urb))
            {
                return URB_FUNCTIONID.UNKNOWN;
            }

            return urb.Header.URB_Function;
        }

        public TS_URB ParsePdu(byte[] data)
        {
            TS_URB urb;

            switch (GetTsUrbType(data))
            {
                // Indicates to the host controller driver that a configuration is to be selected
                case URB_FUNCTIONID.URB_FUNCTION_SELECT_CONFIGURATION:
                    urb = new TS_URB_SELECT_CONFIGURATION();
                    break;
                // Indicates to the host controller driver that an alternate interface setting is being selected for an interface.
                // If set, the URB is used with _URB_SELECT_INTERFACE as the data structure. 
                case URB_FUNCTIONID.URB_FUNCTION_SELECT_INTERFACE:
                    urb = new TS_URB_SELECT_INTERFACE();
                    break;
                // Indicates that all outstanding requests for a pipe should be canceled.
                case URB_FUNCTIONID.URB_FUNCTION_ABORT_PIPE:

                // Resets the indicated pipe.
                case URB_FUNCTIONID.URB_FUNCTION_SYNC_RESET_PIPE_AND_CLEAR_STALL:

                // Clears the halt condition on the host side of a pipe.
                // If set, this URB is used with _URB_PIPE_REQUEST as the data structure.
                case URB_FUNCTIONID.URB_FUNCTION_SYNC_RESET_PIPE:

                // Clears the stall condition on the endpoint. For all pipes except isochronous pipes.
                case URB_FUNCTIONID.URB_FUNCTION_SYNC_CLEAR_STALL:
                    urb = new TS_URB_PIPE_REQUEST();
                    break;

                // Requests the current frame number from the host controller driver.
                case URB_FUNCTIONID.URB_FUNCTION_GET_CURRENT_FRAME_NUMBER:
                    urb = new TS_URB_GET_CURRENT_FRAME_NUMBER();
                    break;

                // Transfers data to or from a control pipe.
                case URB_FUNCTIONID.URB_FUNCTION_CONTROL_TRANSFER:
                    urb = new TS_URB_CONTROL_TRANSFER();
                    break;

                // Transfers data from a bulk pipe or interrupt pipe or to an bulk pipe.
                case URB_FUNCTIONID.URB_FUNCTION_BULK_OR_INTERRUPT_TRANSFER:
                    urb = new TS_URB_BULK_OR_INTERRUPT_TRANSFER();
                    break;

                // Transfers data to or from an isochronous pipe.
                case URB_FUNCTIONID.URB_FUNCTION_ISOCH_TRANSFER:
                    urb = new TS_URB_ISOCH_TRANSFER();
                    break;

                // Retrieves the device descriptor from a specific USB device.
                case URB_FUNCTIONID.URB_FUNCTION_GET_DESCRIPTOR_FROM_DEVICE:

                // Retrieves the descriptor from an endpoint on an interface for a USB device.
                case URB_FUNCTIONID.URB_FUNCTION_GET_DESCRIPTOR_FROM_ENDPOINT:

                // Sets a device descriptor on a device.
                case URB_FUNCTIONID.URB_FUNCTION_SET_DESCRIPTOR_TO_DEVICE:

                // Sets an endpoint descriptor on an endpoint for an interface.
                case URB_FUNCTIONID.URB_FUNCTION_SET_DESCRIPTOR_TO_ENDPOINT:

                // Retrieves the descriptor from an interface for a USB device.
                case URB_FUNCTIONID.URB_FUNCTION_GET_DESCRIPTOR_FROM_INTERFACE:

                // Sets a descriptor for an interface on a USB device.
                case URB_FUNCTIONID.URB_FUNCTION_SET_DESCRIPTOR_TO_INTERFACE:
                    urb = new TS_URB_CONTROL_DESCRIPTOR_REQUEST();
                    break;

                // Sets a USB-defined feature on a device.
                case URB_FUNCTIONID.URB_FUNCTION_SET_FEATURE_TO_DEVICE:

                // Sets a USB-defined feature on an endpoint for an interface on a USB device.
                case URB_FUNCTIONID.URB_FUNCTION_SET_FEATURE_TO_ENDPOINT:

                // Sets a USB-defined feature on an interface for a device.
                case URB_FUNCTIONID.URB_FUNCTION_SET_FEATURE_TO_INTERFACE:

                // Sets a USB-defined feature on a device-defined target on a USB device.
                case URB_FUNCTIONID.URB_FUNCTION_SET_FEATURE_TO_OTHER:

                // Clears a USB-defined feature on a device.
                case URB_FUNCTIONID.URB_FUNCTION_CLEAR_FEATURE_TO_DEVICE:

                // Clears a USB-defined feature on an endpoint, for an interface, on a USB device.
                case URB_FUNCTIONID.URB_FUNCTION_CLEAR_FEATURE_TO_ENDPOINT:

                // Clears a USB-defined feature on an interface for a device.
                case URB_FUNCTIONID.URB_FUNCTION_CLEAR_FEATURE_TO_INTERFACE:

                // Clears a USB-defined feature on a device defined target on a USB device.
                case URB_FUNCTIONID.URB_FUNCTION_CLEAR_FEATURE_TO_OTHER:
                    urb = new TS_URB_CONTROL_FEATURE_REQUEST();
                    break;

                // Retrieves status from a USB device.
                case URB_FUNCTIONID.URB_FUNCTION_GET_STATUS_FROM_DEVICE:

                // Retrieves status from an endpoint for an interface on a USB device.
                case URB_FUNCTIONID.URB_FUNCTION_GET_STATUS_FROM_ENDPOINT:

                // Retrieves status from an interface on a USB device.
                case URB_FUNCTIONID.URB_FUNCTION_GET_STATUS_FROM_INTERFACE:

                // Retrieves status from a device-defined target on a USB device.
                case URB_FUNCTIONID.URB_FUNCTION_GET_STATUS_FROM_OTHER:
                    urb = new TS_URB_CONTROL_GET_STATUS_REQUEST();
                    break;

                // Sends a vendor-specific command to a USB device.
                case URB_FUNCTIONID.URB_FUNCTION_VENDOR_DEVICE:

                // Sends a vendor-specific command for an endpoint on an interface on a USB device. 
                case URB_FUNCTIONID.URB_FUNCTION_VENDOR_ENDPOINT:

                // Sends a vendor-specific command for an interface on a USB device.
                case URB_FUNCTIONID.URB_FUNCTION_VENDOR_INTERFACE:

                // Sends a vendor-specific command to a device-defined target on a USB device.
                case URB_FUNCTIONID.URB_FUNCTION_VENDOR_OTHER:

                // Sends a USB-defined class-specific command to a USB device.
                case URB_FUNCTIONID.URB_FUNCTION_CLASS_DEVICE:

                // Sends a USB-defined class-specific command to an endpoint, on an interface, on a USB device.
                case URB_FUNCTIONID.URB_FUNCTION_CLASS_ENDPOINT:

                // Sends a USB-defined class-specific command to an interface on a USB device.
                case URB_FUNCTIONID.URB_FUNCTION_CLASS_INTERFACE:

                // Sends a USB-defined class-specific command to a device defined target on a USB device.
                case URB_FUNCTIONID.URB_FUNCTION_CLASS_OTHER:
                    urb = new TS_URB_CONTROL_VENDOR_OR_CLASS_REQUEST();
                    break;

                // Retrieves the current configuration on a USB device.
                case URB_FUNCTIONID.URB_FUNCTION_GET_CONFIGURATION:
                    urb = new TS_URB_CONTROL_GET_CONFIGURATION_REQUEST();
                    break;

                // Retrieves the current settings for an interface on a USB device.
                case URB_FUNCTIONID.URB_FUNCTION_GET_INTERFACE:
                    urb = new TS_URB_CONTROL_GET_INTERFACE_REQUEST();
                    break;

                // Retrieves a Microsoft OS feature descriptor from a USB device or an interface on a USB device.
                case URB_FUNCTIONID.URB_FUNCTION_GET_MS_FEATURE_DESCRIPTOR:
                    urb = new TS_URB_OS_FEATURE_DESCRIPTOR_REQUEST();
                    break;

                // Transfers data to or from a control pipe without a time limit specified by a timeout value.
                case URB_FUNCTIONID.URB_FUNCTION_CONTROL_TRANSFER_EX:
                    urb = new TS_URB_CONTROL_TRANSFER_EX();
                    break;
                default:
                    urb = new TS_URB_UNKNOWN();
                    break;
            }

            if (!PduMarshaler.Unmarshal(data, urb))
            {
                urb = new TS_URB_UNKNOWN();
                PduMarshaler.Unmarshal(data, urb);
            }
            return urb;
        }
    }
}
