// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeusb
{
    /// <summary>
    /// Describe an isochronous transfer packet
    /// </summary>
    public class USBD_ISO_PACKET_DESCRIPTOR : BasePDU
    {
        /// <summary>
        /// The beginning of the entire isochronous transfer buffer
        /// </summary>
        private uint offset;

        /// <summary>
        /// The beginning of the entire isochronous transfer buffer
        /// </summary>
        public uint Offset
        {
            set { offset = value; }
            get { return offset; }
        }

        /// <summary>
        /// Indicate the actual number of bytes received 
        /// </summary>
        private uint length;

        /// <summary>
        /// Indicate the actual number of bytes received 
        /// </summary>
        public uint Length
        {
            set { length = value; }
            get { return length; }
        }

        /// <summary>
        /// Contains the status, on return from the host controller driver
        /// </summary>
        private USBD_STATUS status;

        /// <summary>
        /// Contains the status, on return from the host controller driver
        /// </summary>
        public USBD_STATUS Status
        {
            set { status = value; }
            get { return status; }
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteUInt32(offset);
            marshaler.WriteUInt32(length);
            marshaler.WriteUInt32((uint)status);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                offset = marshaler.ReadUInt32();
                length = marshaler.ReadUInt32();
                status = (USBD_STATUS)marshaler.ReadUInt32();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Convert the fields of this PDU to a string.
        /// </summary>
        /// <returns>A string contains the fields of this PDU.</returns>
        public override string ToString()
        {
            string s = string.Format("CbSize: 0x{0:x8}\r\n", offset);
            s += string.Format("UsbBusInterfaceVersion: 0x{0:x8}\r\n", length);
            s += string.Format("Supported_USB_Version: 0x{0:x8}\r\n", (uint)status);

            return s;
        }
    }
}
