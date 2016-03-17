// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestSuites.Rdp
{
    /// <summary>
    /// Command ID for commandId field of SUT_Control_Request_Message
    /// </summary>
    public enum RDPSUTControl_CommandId : ushort
    {
        // For RDP SUT control adapter
        START_RDP_CONNECTION = 0x0001,
        CLOSE_RDP_CONNECTION = 0x0002,
        AUTO_RECONNECT = 0x0003,
        BASIC_INPUT = 0x0004,
        SCREEN_SHOT = 0x0005,

        // For RDPEI SUT Control Adapter
        TOUCH_EVENT_SINGLE = 0x0101,
        TOUCH_EVENT_MULTIPLE = 0x0102,
        TOUCH_EVENT_DISMISS_HOVERING_CONTACT = 0x0103,

        // For RDPEDISP SUT Control Adapter
        DISPLAY_UPDATE_RESOLUTION = 0x0201,
        DISPLAY_UPDATE_MONITORS = 0x0202,
        DISPLAY_FULLSCREEN = 0x0203
    }

    /// <summary>
    /// Flags for BASIC_INPUT command
    /// </summary>
    [Flags]
    public enum RDPSUTControl_BasicInputFlag : uint
    {
        Keyboard_Event = 0x00000001,
        Unicode_Keyboard_Event = 0x00000002,
        Mouse_Event = 0x00000004,
        Extended_Mouse_Event = 0x00000008,
        Client_Synchronize_Event = 0x00000010,
        Client_Refresh_Rect = 0x00000020,
        Client_Suppress_Output = 0x00000040

    }

    /// <summary>
    /// Flag for operation of command DISPLAY_UPDATE_RESOLUTIO
    /// </summary>
    [Flags]
    public enum RDPEDISP_Update_Resolution_Operation : uint
    {
        UPDATE_RESOLUTION = 0x00000001,
        UPDATE_ORIENTATION = 0x00000002
    }

    /// <summary>
    /// Flag for operation of command DISPLAY_UPDATE_MONITORS 
    /// </summary>
    [Flags]
    public enum RDPEDISP_Update_Monitors_Operation : uint
    {
        ADD_MONITOR = 0x00000001,
        REMOVE_MONITOR = 0x00000002,
        MOVE_MONITOR_POSITION = 0x00000004
    }
    
    /// <summary>
    /// Payload type of SUT_Control_Request_Message with commandId: START_RDP_CONNECTIO
    /// </summary>
    public enum RDP_Connect_Payload_Type: uint
    {
        RDP_FILE = 0x0000,
        PARAMETERS_STRUCT = 0x0001
    }

    /// <summary>
    /// Screen type, including full and normal, used for screenType field of RDP_Connection_Configure_Parameters
    /// </summary>
    public enum RDP_Screen_Type : ushort
    {
        NORMAL = 0x0000,
        FULL_SCREEN = 0x0001
    }

    /// <summary>
    /// Connect approach, including Negotiate and Direct, used for the connectApproach field of RDP_Connection_Configure_Parameters
    /// </summary>
    public enum RDP_Connect_Approach:ushort 
    {
        Negotiate = 0x0000,
        Direct = 0x0001
    }

    /// <summary>
    /// This structure is used as part of payload of Start_RDP_Connection when payload type is PARAMETERS_STRUCT
    /// </summary>
    public struct RDP_Connection_Configure_Parameters
    {
        public ushort port;
        public RDP_Screen_Type screenType;
        public ushort desktopWidth;
        public ushort desktopHeight;
        public RDP_Connect_Approach connectApproach;
        public ushort AddressLength
        {
            get
            {
                return (ushort)Encoding.UTF8.GetByteCount(address);
            }
        }
        public string address;

        public byte[] Encode()
        {
            List<byte> bufferList = new List<byte>();
            bufferList.AddRange(Utility.ChangeBytesOrderForNumeric(BitConverter.GetBytes(this.port)));
            bufferList.AddRange(Utility.ChangeBytesOrderForNumeric(BitConverter.GetBytes((ushort)this.screenType)));
            bufferList.AddRange(Utility.ChangeBytesOrderForNumeric(BitConverter.GetBytes((ushort)this.desktopWidth)));
            bufferList.AddRange(Utility.ChangeBytesOrderForNumeric(BitConverter.GetBytes((ushort)this.desktopHeight)));

            bufferList.AddRange(Utility.ChangeBytesOrderForNumeric(BitConverter.GetBytes((ushort)this.connectApproach)));
            bufferList.AddRange(Utility.ChangeBytesOrderForNumeric(BitConverter.GetBytes((ushort)this.AddressLength)));
            if (address != null && address.Length > 0)
            {
                bufferList.AddRange(Encoding.UTF8.GetBytes(address));
            }
            return bufferList.ToArray();
        }
    }

    /// <summary>
    /// Structure of Payload field of SUT_Control_Request_Message with commandId: START_RDP_CONNECTIO
    /// </summary>
    public struct RDP_Connection_Payload
    {
        public RDP_Connect_Payload_Type type;
        public string rdpFileConfig;
        public RDP_Connection_Configure_Parameters configureParameters;

        public byte[] Encode()
        {
            List<byte> bufferList = new List<byte>();
            bufferList.AddRange(Utility.ChangeBytesOrderForNumeric(BitConverter.GetBytes((uint)this.type)));
            if (type == RDP_Connect_Payload_Type.RDP_FILE)
            {
                bufferList.AddRange(Encoding.UTF8.GetBytes(rdpFileConfig));
            }
            else
            {
                bufferList.AddRange(configureParameters.Encode());
            }
            return bufferList.ToArray();
        }

    }
}

