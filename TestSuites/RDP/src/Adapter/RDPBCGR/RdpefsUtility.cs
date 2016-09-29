// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestSuites.Rdpbcgr.efs
{
    /// <summary>
    /// Encode/Decode MS-RDPEFS packets.
    /// </summary>
    public static class RdpefsUtility
    {
        /// <summary>
        /// Create Server Announce Request packet.
        /// </summary>
        /// <returns>Server Announce Request packet</returns>
        public static DR_CORE_SERVER_ANNOUNCE_REQ CreateServerAnnounceRequest()
        {
            DR_CORE_SERVER_ANNOUNCE_REQ servreAnounceRequest = new DR_CORE_SERVER_ANNOUNCE_REQ();
            servreAnounceRequest.Header = new RDPDR_HEADER();
            servreAnounceRequest.Header.Component = Component_Values.RDPDR_CTYP_CORE;
            servreAnounceRequest.Header.PacketId = PacketId_Values.PAKID_CORE_SERVER_ANNOUNCE;
            servreAnounceRequest.VersionMajor = VersionMajor_Values.V1;
            servreAnounceRequest.VersionMinor = VersionMinor_Values.V1;
            servreAnounceRequest.ClientId = 1;
            return servreAnounceRequest;
        }

        /// <summary>
        /// Encode Server Announce Request packet.
        /// </summary>
        /// <param name="packet">Server Announce Request packet</param>
        /// <returns>Encoded byte array.</returns>
        public static byte[] EncodeServerAnnounceRequest(DR_CORE_SERVER_ANNOUNCE_REQ packet)
        {
            List<byte> buffer = new List<byte>();
            EncodeStructure(buffer, (ushort)(packet.Header.Component));
            EncodeStructure(buffer, (ushort)(packet.Header.PacketId));
            EncodeStructure(buffer, (ushort)(packet.VersionMajor));
            EncodeStructure(buffer, (ushort)(packet.VersionMinor));
            EncodeStructure(buffer, (uint)packet.ClientId);
            return buffer.ToArray();
        }

        /// <summary>
        /// Create Server Core Capability Request packet.
        /// </summary>
        /// <returns>Server Core Capability Request packet.</returns>
        public static DR_CORE_CAPABILITY_REQ CreateServerCoreCapabilityRequest()
        {
            DR_CORE_CAPABILITY_REQ request = new DR_CORE_CAPABILITY_REQ();
            request.Header = new RDPDR_HEADER();
            request.Header.Component = Component_Values.RDPDR_CTYP_CORE;
            request.Header.PacketId = PacketId_Values.PAKID_CORE_SERVER_CAPABILITY;
            request.numCapabilities = 5;
            request.Padding = 0;

            List<CAPABILITY_SET> capabilitySet = new List<CAPABILITY_SET>();

            GENERAL_CAPS_SET generalCapability = new GENERAL_CAPS_SET();
            generalCapability.Header = new CAPABILITY_HEADER();
            generalCapability.Header.CapabilityType = CapabilityType_Values.CAP_GENERAL_TYPE;
            generalCapability.Header.CapabilityLength = 44;
            generalCapability.Header.Version = CAPABILITY_VERSION.V1;
            generalCapability.osType = osType_Values.OS_TYPE_UNKNOWN;
            generalCapability.osVersion = osVersion_Values.V1;
            generalCapability.protocolMajorVersion = protocolMajorVersion_Values.V1;
            generalCapability.protocolMinorVersion = 0x000C;
            generalCapability.ioCode1 = (ioCode1_Values)0x0000FFFF;
            generalCapability.ioCode2 = ioCode2_Values.V1;
            generalCapability.extendedPDU = extendedPDU_Values.RDPDR_DEVICE_REMOVE_PDUS | extendedPDU_Values.RDPDR_CLIENT_DISPLAY_NAME_PDU;
            generalCapability.extraFlags1 = extraFlags1_Values.ENABLE_ASYNCIO;
            generalCapability.extraFlags2 = extraFlags2_Values.V1;
            generalCapability.SpecialTypeDeviceCap = 2;
            capabilitySet.Add(generalCapability);

            PRINTER_CAPS_SET printerCapability = new PRINTER_CAPS_SET();
            printerCapability.Header = new CAPABILITY_HEADER();
            printerCapability.Header.CapabilityType = CapabilityType_Values.CAP_PRINTER_TYPE;
            printerCapability.Header.CapabilityLength = 8;
            printerCapability.Header.Version = CAPABILITY_VERSION.V1;
            capabilitySet.Add(printerCapability);


            PORT_CAPS_SET portCapability = new PORT_CAPS_SET();
            portCapability.Header = new CAPABILITY_HEADER();
            portCapability.Header.CapabilityType = CapabilityType_Values.CAP_PORT_TYPE;
            portCapability.Header.CapabilityLength = 8;
            portCapability.Header.Version = CAPABILITY_VERSION.V1;
            capabilitySet.Add(portCapability);

            DRIVE_CAPS_SET driveCapability = new DRIVE_CAPS_SET();
            driveCapability.Header = new CAPABILITY_HEADER();
            driveCapability.Header.CapabilityType = CapabilityType_Values.CAP_DRIVE_TYPE;
            driveCapability.Header.CapabilityLength = 8;
            driveCapability.Header.Version = CAPABILITY_VERSION.V2;
            capabilitySet.Add(driveCapability);

            SMARTCARD_CAPS_SET smartcardCapability = new SMARTCARD_CAPS_SET();
            smartcardCapability.Header = new CAPABILITY_HEADER();
            smartcardCapability.Header.CapabilityType = CapabilityType_Values.CAP_SMARTCARD_TYPE;
            smartcardCapability.Header.CapabilityLength = 8;
            smartcardCapability.Header.Version = CAPABILITY_VERSION.V1;
            capabilitySet.Add(smartcardCapability);

            request.CapabilityMessage = capabilitySet.ToArray();
            return request;
        }

        /// <summary>
        /// Encode Server Core Capability Request packet.
        /// </summary>
        /// <param name="packet">Server Core Capability Request packet.</param>
        /// <returns>Encoded byte array.</returns>
        public static byte[] EncodeServerCoreCapabilityRequest(DR_CORE_CAPABILITY_REQ packet)
        {
            List<byte> buffer = new List<byte>();
            EncodeStructure(buffer, (ushort)packet.Header.Component);
            EncodeStructure(buffer, (ushort)packet.Header.PacketId);
            EncodeStructure(buffer, packet.numCapabilities);
            EncodeStructure(buffer, packet.Padding);

            if (packet.CapabilityMessage != null)
            {
                foreach (CAPABILITY_SET capability in packet.CapabilityMessage)
                {
                    if (capability is GENERAL_CAPS_SET)
                    {
                        GENERAL_CAPS_SET generalCapability = capability as GENERAL_CAPS_SET;
                        EncodeStructure(buffer, (ushort)generalCapability.Header.CapabilityType);
                        EncodeStructure(buffer, (ushort)generalCapability.Header.CapabilityLength);
                        EncodeStructure(buffer, (uint)generalCapability.Header.Version);
                        EncodeStructure(buffer, (uint)generalCapability.osType);
                        EncodeStructure(buffer, (uint)generalCapability.osVersion);
                        EncodeStructure(buffer, (ushort)generalCapability.protocolMajorVersion);
                        EncodeStructure(buffer, (ushort)generalCapability.protocolMinorVersion);
                        EncodeStructure(buffer, (uint)generalCapability.ioCode1);
                        EncodeStructure(buffer, (uint)generalCapability.ioCode2);
                        EncodeStructure(buffer, (uint)generalCapability.extendedPDU);
                        EncodeStructure(buffer, (uint)generalCapability.extraFlags1);
                        EncodeStructure(buffer, (uint)generalCapability.extraFlags2);
                        EncodeStructure(buffer, (uint)generalCapability.SpecialTypeDeviceCap);
                    }
                    else
                    {
                        EncodeStructure(buffer, (ushort)capability.Header.CapabilityType);
                        EncodeStructure(buffer, (ushort)capability.Header.CapabilityLength);
                        EncodeStructure(buffer, (uint)capability.Header.Version);
                    }
                }
            }

            return buffer.ToArray();
        }

        /// <summary>
        /// Create Server Client ID Confirm packet.
        /// </summary>
        /// <param name="clientId">Client Id.</param>
        /// <returns>Server Client ID Confirm packet.</returns>
        public static DR_CORE_SERVER_CLIENTID_CONFIRM CreateServerClientIDConfirm(uint clientId)
        {
            DR_CORE_SERVER_CLIENTID_CONFIRM request = new DR_CORE_SERVER_CLIENTID_CONFIRM();
            request.Header = new RDPDR_HEADER();
            request.Header.Component = Component_Values.RDPDR_CTYP_CORE;
            request.Header.PacketId = PacketId_Values.PAKID_CORE_CLIENTID_CONFIRM;
            request.VersionMajor = DR_CORE_SERVER_CLIENTID_CONFIRM_VersionMajor_Values.V1;
            request.VersionMinor = DR_CORE_SERVER_CLIENTID_CONFIRM_VersionMinor_Values.V1;
            request.ClientId = clientId;
            return request;
        }

        /// <summary>
        /// Encode Server Client ID Confirm packet.
        /// </summary>
        /// <param name="packet">Server Client ID Confirm packet.</param>
        /// <returns>Encoded byte array.</returns>
        public static byte[] EncodeServerClientIDConfirm(DR_CORE_SERVER_CLIENTID_CONFIRM packet)
        {
            List<byte> buffer = new List<byte>();
            EncodeStructure(buffer, (ushort)(packet.Header.Component));
            EncodeStructure(buffer, (ushort)(packet.Header.PacketId));
            EncodeStructure(buffer, (ushort)(packet.VersionMajor));
            EncodeStructure(buffer, (ushort)(packet.VersionMinor));
            EncodeStructure(buffer, (uint)(packet.ClientId));
            return buffer.ToArray();
        }

        /// <summary>
        /// Create Server User Logged On packet
        /// </summary>
        /// <returns>DR_CORE_USER_LOGGEDON</returns>
        public static DR_CORE_USER_LOGGEDON CreateServerUserLoggedOn()
        {
            DR_CORE_USER_LOGGEDON logonPDU = new DR_CORE_USER_LOGGEDON();
            logonPDU.Header = new RDPDR_HEADER();
            logonPDU.Header.Component = Component_Values.RDPDR_CTYP_CORE;
            logonPDU.Header.PacketId = PacketId_Values.PAKID_CORE_USER_LOGGEDON;
            return logonPDU;
        }

        /// <summary>
        /// Encode a Server User Logged On packet
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        public static byte[] EncodeServerUserLoggedOn(DR_CORE_USER_LOGGEDON packet)
        {
            List<byte> buffer = new List<byte>();
            EncodeStructure(buffer, (ushort)(packet.Header.Component));
            EncodeStructure(buffer, (ushort)(packet.Header.PacketId));

            return buffer.ToArray();
        }

        /// <summary>
        /// Decode Client Announce Reply packet.
        /// </summary>
        /// <param name="data">Packet data.</param>
        /// <returns>Client Announce Reply packet.</returns>
        public static DR_CORE_SERVER_ANNOUNCE_RSP DecodeClientAnnounceReply(byte[] data)
        {
            int index = 0;
            DR_CORE_SERVER_ANNOUNCE_RSP packet = new DR_CORE_SERVER_ANNOUNCE_RSP();
            packet.Header = DecodeRdpdrHeader(data, ref index, false);
            packet.VersionMajor = (DR_CORE_SERVER_ANNOUNCE_RSP_VersionMajor_Values)ParseUInt16(data, ref index, false);
            packet.VersionMinor = (DR_CORE_SERVER_ANNOUNCE_RSP_VersionMinor_Values)ParseUInt16(data, ref index, false);
            packet.ClientId = ParseUInt32(data, ref index, false);
            return packet;
        }

        /// <summary>
        /// Decode Client Core Capability Response packet
        /// </summary>
        /// <param name="data">Packet data</param>
        /// <returns>Client Core Capability Response packet</returns>
        public static DR_CORE_CAPABILITY_RSP DecodeClientCoreCapabilityRSP(byte[] data)
        {
            int index = 0;
            DR_CORE_CAPABILITY_RSP packet = new DR_CORE_CAPABILITY_RSP();
            packet.Header = DecodeRdpdrHeader(data, ref index, false);
            packet.numCapabilities = ParseUInt16(data, ref index, false);
            packet.Padding = ParseUInt16(data, ref index, false);

            List<CAPABILITY_SET> capbilityList = new List<CAPABILITY_SET>();

            while (index + 8 <= data.Length)
            {
                CAPABILITY_HEADER header = DecodeCapabilityHeader(data, ref index, false);
                if (header.CapabilityType == CapabilityType_Values.CAP_GENERAL_TYPE)
                {
                    int originalIndex = index;
                    GENERAL_CAPS_SET capSet = new GENERAL_CAPS_SET();
                    capSet.Header = header;
                    capSet.osType = (osType_Values)ParseUInt32(data, ref index, false);
                    capSet.osVersion = (osVersion_Values)ParseUInt32(data, ref index, false);
                    capSet.protocolMajorVersion = (protocolMajorVersion_Values)ParseUInt16(data, ref index, false);
                    capSet.protocolMinorVersion = ParseUInt16(data, ref index, false);
                    capSet.ioCode1 = (ioCode1_Values)ParseUInt32(data, ref index, false);
                    capSet.ioCode2 = (ioCode2_Values)ParseUInt32(data, ref index, false);
                    capSet.extendedPDU = (extendedPDU_Values)ParseUInt32(data, ref index, false);
                    capSet.extraFlags1 = (extraFlags1_Values)ParseUInt32(data, ref index, false);
                    capSet.extraFlags2 = (extraFlags2_Values)ParseUInt32(data, ref index, false);
                    capSet.SpecialTypeDeviceCap = ParseUInt32(data, ref index, false);
                    index = originalIndex + header.CapabilityLength;
                    capbilityList.Add(capSet);
                }
                else if (header.CapabilityType == CapabilityType_Values.CAP_PRINTER_TYPE)
                {
                    PRINTER_CAPS_SET capSet = new PRINTER_CAPS_SET();
                    capSet.Header = header;
                    capbilityList.Add(capSet);
                }
                else if (header.CapabilityType == CapabilityType_Values.CAP_DRIVE_TYPE)
                {
                    DRIVE_CAPS_SET capSet = new DRIVE_CAPS_SET();
                    capSet.Header = header;
                    capbilityList.Add(capSet);
                }
                else if (header.CapabilityType == CapabilityType_Values.CAP_PORT_TYPE)
                {
                    PORT_CAPS_SET capSet = new PORT_CAPS_SET();
                    capSet.Header = header;
                    capbilityList.Add(capSet);
                }
                else if (header.CapabilityType == CapabilityType_Values.CAP_SMARTCARD_TYPE)
                {
                    SMARTCARD_CAPS_SET capSet = new SMARTCARD_CAPS_SET();
                    capSet.Header = header;
                    capbilityList.Add(capSet);
                }
                else
                {
                    return null;
                }
            }
            packet.CapabilityMessage = capbilityList.ToArray();

            return packet;
        }
        #region private methods
        /// <summary>
        /// Encode a structure to a byte list.
        /// </summary>
        /// <param name="buffer">The buffer list to contain the structure. 
        /// This argument cannot be null. It may throw ArgumentNullException if it is null.</param>
        /// <param name="structure">The structure to be added to buffer list.
        /// This argument cannot be null. It may throw ArgumentNullException if it is null.</param>
        private static void EncodeStructure(List<byte> buffer, object structure)
        {
            byte[] structBuffer = StructToBytes(structure);
            buffer.AddRange(structBuffer);
        }

        /// <summary>
        /// Method to covert struct to byte[]
        /// </summary>
        /// <param name="structp">The struct prepare to covert</param>
        /// <returns>The byte array converted from struct</returns>
        private static byte[] StructToBytes(object structp)
        {
            IntPtr ptr = IntPtr.Zero;
            byte[] buffer = null;

            try
            {
                int size = Marshal.SizeOf(structp.GetType());
                ptr = Marshal.AllocHGlobal(size);
                buffer = new byte[size];
                Marshal.StructureToPtr(structp, ptr, false);
                Marshal.Copy(ptr, buffer, 0, size);
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }

            return buffer;
        }

        private static RDPDR_HEADER DecodeRdpdrHeader(byte[] data, ref int index, bool isBigEndian)
        {
            RDPDR_HEADER header = new RDPDR_HEADER();
            header.Component = (Component_Values)ParseUInt16(data, ref index, isBigEndian);
            header.PacketId = (PacketId_Values)ParseUInt16(data, ref index, isBigEndian);

            return header;
        }

        private static CAPABILITY_HEADER DecodeCapabilityHeader(byte[] data, ref int index, bool isBigEndian)
        {
            CAPABILITY_HEADER header = new CAPABILITY_HEADER();
            header.CapabilityType = (CapabilityType_Values)ParseUInt16(data, ref index, isBigEndian);
            header.CapabilityLength = ParseUInt16(data, ref index, isBigEndian);
            header.Version = (CAPABILITY_VERSION)ParseUInt32(data, ref index, isBigEndian);

            return header;
        }
        /// <summary>
        /// Parse UInt16
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="index">parser index</param>
        /// <param name="isBigEndian">big endian format flag</param>
        /// <returns>parsed UInt16 number</returns>
        private static UInt16 ParseUInt16(byte[] data, ref int index, bool isBigEndian)
        {
            // Read 2 bytes
            byte[] bytes = GetBytes(data, ref index, sizeof(UInt16));

            // Big Endian format requires reversed byte order
            if (isBigEndian)
            {
                Array.Reverse(bytes, 0, sizeof(UInt16));
            }

            // Convert
            return BitConverter.ToUInt16(bytes, 0);
        }

        /// <summary>
        /// Parse UInt32
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="index">parser index</param>
        /// <param name="isBigEndian">big endian format flag</param>
        /// <returns>parsed UInt32 number</returns>
        private static UInt32 ParseUInt32(byte[] data, ref int index, bool isBigEndian)
        {
            // Read 4 bytes
            byte[] bytes = GetBytes(data, ref index, sizeof(UInt32));

            // Big Endian format requires reversed byte order
            if (isBigEndian)
            {
                Array.Reverse(bytes, 0, sizeof(UInt32));
            }

            // Convert
            return BitConverter.ToUInt32(bytes, 0);
        }

        /// <summary>
        /// Get specified length of bytes from a byte array
        /// (start index is updated according to the specified length)
        /// </summary>
        /// <param name="data">data in byte array</param>
        /// <param name="startIndex">start index</param>
        /// <param name="bytesToRead">specified length</param>
        /// <returns>bytes of specified length</returns>
        private static byte[] GetBytes(byte[] data, ref int startIndex, int bytesToRead)
        {
            // if input data is null
            if (null == data)
            {
                return null;
            }

            // if index is out of range
            if ((startIndex < 0) || (startIndex + bytesToRead > data.Length))
            {
                return null;
            }

            // read bytes of specific length
            byte[] dataRead = new byte[bytesToRead];
            Array.Copy(data, startIndex, dataRead, 0, bytesToRead);

            // update index
            startIndex += bytesToRead;
            return dataRead;
        }
        #endregion

    }
}
