// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedisp
{
    #region Common Structure

    /// <summary>
    /// The DISPLAYCONTROL_HEADER structure is included in all display control PDUs and specifies the PDU type and the length of the PDU.
    /// </summary>
    public struct DISPLAYCONTROL_HEADER
    {
        /// <summary>
        /// A 32-bit unsigned integer that specifies the display control PDU type.
        /// </summary>
        public PDUTypeValues Type;

        /// <summary>
        /// A 32-bit unsigned integer that specifies the display control PDU type.
        /// </summary>
        public uint Length;
    }

    /// <summary>
    /// The unknown type.
    /// </summary>
    public class RdpedispUnkownPdu : RdpedispPdu
    {
        public byte[] Data;

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            if (Data != null)
            {
                marshaler.WriteBytes(Data);
            };
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            Data = marshaler.ReadToEnd();
            return Data != null;
        }

    }

    #endregion

    #region Base PDUs
    /// <summary>
    /// The base pdu of all MS-RDPEGFX messages.
    /// </summary>
    public class RdpedispPdu : BasePDU
    {
        #region Message Fields

        /// <summary>
        /// A DISPLAYCONTROL_HEADER (section 2.2.1.1) structure.
        /// </summary>
        public DISPLAYCONTROL_HEADER Header;
        
        #endregion

        #region Methods
        
        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteUInt32((uint)Header.Type);
            marshaler.WriteUInt32(Header.Length);
        }

        /// <summary>
        /// Decode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to Decode the fields of this PDU.</param>
        /// <returns></returns>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                Header.Type = (PDUTypeValues) marshaler.ReadUInt32();
                Header.Length = marshaler.ReadUInt32();
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }

        /// <summary>
        /// Convert the fields of this PDU to a string.
        /// </summary>
        /// <returns>A string contains the fields of this PDU.</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("Header.Typer:{0}\n", Header.Type);
            stringBuilder.AppendFormat("header.Length:{0}\n", Header.Length);
            return stringBuilder.ToString();
        }

        #endregion
    }

    /// <summary>
    /// The DISPLAYCONTROL_MONITOR_LAYOUT class is used to specify the characteristics of a monitor. 
    /// </summary>
    public class DISPLAYCONTROL_MONITOR_LAYOUT
    {
        #region Message Fields
        /// <summary>
        /// A 32-bit unsigned integer that specifies monitor configuration flags.
        /// </summary>
        public MonitorLayout_FlagValues Flags;

        /// <summary>
        /// A 32-bit signed integer that specifies the x-coordinate of the upper-left corner of the display monitor.
        /// </summary>
        public int Left;

        /// <summary>
        /// A 32-bit signed integer that specifies the y-coordinate of the upper-left corner of the display monitor.
        /// </summary>
        public int Top;

        /// <summary>
        /// A 32-bit unsigned integer that specifies the width of the monitor in pixels. 
        /// </summary>
        public uint Width;

        /// <summary>
        /// A 32-bit unsigned integer that specifies the height of the monitor in pixels. 
        /// </summary>
        public uint Height;

        /// <summary>
        /// A 32-bit unsigned integer that specifies the physical width of the monitor, in millimeters (mm). 
        /// </summary>
        public uint PhysicalWidth;

        /// <summary>
        /// A 32-bit unsigned integer that specifies the physical height of the monitor, in millimeters. 
        /// </summary>
        public uint PhysicalHeight;

        /// <summary>
        /// A 32-bit unsigned integer that specifies the orientation of the monitor in degrees.
        /// </summary>
        public MonitorLayout_OrientationValues Orientation;

        /// <summary>
        /// A 32-bit, unsigned integer that specifies the desktop scale factor of the monitor. 
        /// </summary>
        public uint DesktopScaleFactor;

        /// <summary>
        /// A 32-bit, unsigned integer that specifies the device scale factor of the monitor.
        /// </summary>
        public uint DeviceScaleFactor;

        #endregion

        #region Methods
        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteUInt32((uint)Flags);
            marshaler.WriteInt32(Left);
            marshaler.WriteInt32(Top);
            marshaler.WriteUInt32(Width);
            marshaler.WriteUInt32(Height);
            marshaler.WriteUInt32(PhysicalWidth);
            marshaler.WriteUInt32(PhysicalHeight);
            marshaler.WriteUInt32((uint)Orientation);
            marshaler.WriteUInt32(DesktopScaleFactor);
            marshaler.WriteUInt32(DeviceScaleFactor);
        }

        /// <summary>
        /// Decode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to Decode the fields of this PDU.</param>
        /// <returns></returns>
        public bool Decode(PduMarshaler marshaler)
        {
            try
            {
                Flags = (MonitorLayout_FlagValues)marshaler.ReadUInt32();
                Left = marshaler.ReadInt32();
                Top = marshaler.ReadInt32();
                Width = marshaler.ReadUInt32();
                Height = marshaler.ReadUInt32();
                PhysicalWidth = marshaler.ReadUInt32();
                PhysicalHeight = marshaler.ReadUInt32();
                Orientation = (MonitorLayout_OrientationValues)marshaler.ReadUInt32();
                DesktopScaleFactor = marshaler.ReadUInt32();
                DeviceScaleFactor = marshaler.ReadUInt32();
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }

        /// <summary>
        /// Convert the fields of this PDU to a string.
        /// </summary>
        /// <returns>A string contains the fields of this PDU.</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("DISPLAYCONTROL_MONITOR_LAYOUT");
            stringBuilder.AppendFormat("Flags:{0}\n", Flags);
            stringBuilder.AppendFormat("Left:{0}\n", Left);
            stringBuilder.AppendFormat("Top:{0}\n", Top);
            stringBuilder.AppendFormat("Width:{0}\n", Width);
            stringBuilder.AppendFormat("Height:{0}\n", Height);
            stringBuilder.AppendFormat("PhysicalWidth:{0}\n", PhysicalWidth);
            stringBuilder.AppendFormat("PhysicalHeight:{0}\n", PhysicalHeight);
            stringBuilder.AppendFormat("Orientation:{0}\n", Orientation);
            stringBuilder.AppendFormat("DesktopScaleFactor:{0}\n", DesktopScaleFactor);
            stringBuilder.AppendFormat("DeviceScaleFactor:{0}\n", DeviceScaleFactor);
            return stringBuilder.ToString();
        }

        #endregion
    }

    #endregion

    #region Server PDUs
    /// <summary>
    /// The DISPLAYCONTROL_CAPS_PDU message is a server-to-client PDU that is used to specify a set of parameters
    /// which the client must adhere to when sending the DISPLAYCONTROL_MONITOR_LAYOUT_PDU (section 2.2.2.2) message.
    /// </summary>
    public class DISPLAYCONTROL_CAPS_PDU : RdpedispPdu
    {
        #region Message Fields
        /// <summary>
        /// A 32-bit unsigned integer that specifies the maximum number of monitors supported by the server.
        /// </summary>
        public uint MaxNumMonitors;

        /// <summary>
        /// A 32-bit unsigned integer that is used to specify the maximum monitor area supported by the server. 
        /// </summary>
        public uint MaxMonitorAreaFactorA;

        /// <summary>
        /// A 32-bit unsigned integer that is used to specify the maximum monitor area supported by the server. 
        /// </summary>
        public uint MaxMonitorAreaFactorB;
        #endregion

        #region Methods
        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(MaxNumMonitors);
            marshaler.WriteUInt32(MaxMonitorAreaFactorA);
            marshaler.WriteUInt32(MaxMonitorAreaFactorB);
        }

        /// <summary>
        /// Decode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to Decode the fields of this PDU.</param>
        /// <returns></returns>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                MaxNumMonitors = marshaler.ReadUInt32();
                MaxMonitorAreaFactorA = marshaler.ReadUInt32();
                MaxMonitorAreaFactorB = marshaler.ReadUInt32();
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }

        /// <summary>
        /// Convert the fields of this PDU to a string.
        /// </summary>
        /// <returns>A string contains the fields of this PDU.</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(base.ToString());
            stringBuilder.AppendFormat("MaxNumMonitors:{0}\n", MaxNumMonitors);
            stringBuilder.AppendFormat("MaxMonitorAreaFactorA:{0}\n", MaxMonitorAreaFactorA);
            stringBuilder.AppendFormat("MaxMonitorAreaFactorB:{0}\n", MaxMonitorAreaFactorB);
            return stringBuilder.ToString();
        }

        #endregion
    }
    #endregion

    #region Client PDUs
    /// <summary>
    /// The DISPLAYCONTROL_MONITOR_LAYOUT structure is used to specify the characteristics of a monitor. 
    /// </summary>
    public class DISPLAYCONTROL_MONITOR_LAYOUT_PDU : RdpedispPdu
    {
        #region Message Fields
        /// <summary>
        /// A 32-bit unsigned integer that specifies the size, in bytes, of a single element in the Monitors field. 
        /// </summary>
        public uint MonitorLayoutSize;

        /// <summary>
        /// A 32-bit unsigned integer that specifies the number of display monitor definitions in the Monitors field.
        /// </summary>
        public uint NumMonitors;

        /// <summary>
        /// A variable-length array containing a series of DISPLAYCONTROL_MONITOR_LAYOUT structures that specify the display monitor layout of the client.
        /// </summary>
        public DISPLAYCONTROL_MONITOR_LAYOUT[] Monitors;
        #endregion

        #region Methods
        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(MonitorLayoutSize);
            marshaler.WriteUInt32(NumMonitors);
            foreach(var monitor in Monitors)
            {
                monitor.Encode(marshaler);
            }
        }

        /// <summary>
        /// Decode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to Decode the fields of this PDU.</param>
        /// <returns></returns>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                MonitorLayoutSize = marshaler.ReadUInt32();
                NumMonitors = marshaler.ReadUInt32();
                Monitors = new DISPLAYCONTROL_MONITOR_LAYOUT[NumMonitors];
                for (int i = 0; i < Monitors.Length;i++)
                {
                    Monitors[i] = new DISPLAYCONTROL_MONITOR_LAYOUT();
                    Monitors[i].Decode(marshaler);
                }
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }

        /// <summary>
        /// Convert the fields of this PDU to a string.
        /// </summary>
        /// <returns>A string contains the fields of this PDU.</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(base.ToString());
            stringBuilder.AppendFormat("MonitorLayoutSize:{0}\n", MonitorLayoutSize);
            stringBuilder.AppendFormat("NumMonitors:{0}\n", NumMonitors);
            foreach (var monitor in Monitors)
            {
                stringBuilder.Append(monitor.ToString());
            }
            return stringBuilder.ToString();
        }
        #endregion

    }
    #endregion
}
