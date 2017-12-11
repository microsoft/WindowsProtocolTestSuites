// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDPSUTControlAgent
{
    public enum RDPSUTControl_CommandId : ushort
    {
        START_RDP_CONNECTION = 0x0001,
        CLOSE_RDP_CONNECTION = 0x0002,
        AUTO_RECONNECT = 0x0003,
        BASIC_INPUT = 0x0004,
        SCREEN_SHOT = 0x0005
    }

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
    
    public enum RDP_Connect_Payload_Type : uint
    {
        RDP_FILE = 0x0000,
        PARAMETERS_STRUCT = 0x0001
    }

    public enum RDP_Screen_Type : ushort
    {
        NORMAL = 0x0000,
        FULL_SCREEN = 0x0001
    }

    public enum RDP_Connect_Approach : ushort
    {
        Negotiate = 0x0000,
        Direct = 0x0001
    }

    public struct RDP_Connection_Configure_Parameters
    {
        public ushort port;
        public RDP_Screen_Type screenType;
        public ushort desktopWidth;
        public ushort desktopHeight;
        public RDP_Connect_Approach connectApproach;
        public ushort addressLength;
        
        public string address;

        public byte[] Encode()
        {
            List<byte> bufferList = new List<byte>();
            bufferList.AddRange(Utility.ChangeBytesOrderForNumeric(BitConverter.GetBytes(this.port)));
            bufferList.AddRange(Utility.ChangeBytesOrderForNumeric(BitConverter.GetBytes((ushort)this.screenType)));
            bufferList.AddRange(Utility.ChangeBytesOrderForNumeric(BitConverter.GetBytes((ushort)this.desktopWidth)));
            bufferList.AddRange(Utility.ChangeBytesOrderForNumeric(BitConverter.GetBytes((ushort)this.desktopHeight)));

            bufferList.AddRange(Utility.ChangeBytesOrderForNumeric(BitConverter.GetBytes((ushort)this.connectApproach)));
            bufferList.AddRange(Utility.ChangeBytesOrderForNumeric(BitConverter.GetBytes((ushort)this.addressLength)));
            if (address != null && address.Length > 0)
            {
                bufferList.AddRange(Encoding.UTF8.GetBytes(address));
            }
            return bufferList.ToArray();
        }

        public bool Decode(byte[] rawData, ref int index)
        {
            try
            {
                this.port = BitConverter.ToUInt16(Utility.GetNumericBytes(rawData, index, 2), 0);
                index += 2;

                this.screenType = (RDP_Screen_Type)BitConverter.ToUInt16(Utility.GetNumericBytes(rawData, index, 2), 0);
                index += 2;

                this.desktopWidth = BitConverter.ToUInt16(Utility.GetNumericBytes(rawData, index, 2), 0);
                index += 2;

                this.desktopHeight = BitConverter.ToUInt16(Utility.GetNumericBytes(rawData, index, 2), 0);
                index += 2;

                this.connectApproach = (RDP_Connect_Approach)BitConverter.ToUInt16(Utility.GetNumericBytes(rawData, index, 2), 0);
                index += 2;

                this.addressLength = BitConverter.ToUInt16(Utility.GetNumericBytes(rawData, index, 2), 0);
                index += 2;

                this.address = Encoding.UTF8.GetString(rawData, index, this.addressLength);
                index += this.addressLength;


            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }

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

        public bool Decode(byte[] rawData, int payloadLength, ref int index)
        {
            try
            {
                this.type = (RDP_Connect_Payload_Type)BitConverter.ToUInt32(Utility.GetNumericBytes(rawData, index, 4), 0);
                index += 4;

                if (type == RDP_Connect_Payload_Type.RDP_FILE)
                {
                    this.rdpFileConfig = Encoding.UTF8.GetString(rawData, index, payloadLength - 4);
                    index += (payloadLength - 4);
                }
                else
                {
                    this.configureParameters = new RDP_Connection_Configure_Parameters();
                    if (!this.configureParameters.Decode(rawData, ref index))
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

    }
}

