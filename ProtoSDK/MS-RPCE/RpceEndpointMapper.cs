// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

using Microsoft.Protocols.TestTools.StackSdk.Transport;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// EPM utility class
    /// </summary>
    public static class RpceEndpointMapper
    {
        // timeout for EPT query
        private static readonly TimeSpan queryEptTimeout = TimeSpan.FromMinutes(1);

        // followings are endpoint, protoseq, if_uuid, if_vers_major, if_vers_minor for EPT.
        private const int eptEndpoint = 135;
        private const string eptProtocolSequence = RpceUtility.RPC_OVER_TCPIP_PROTOCOL_SEQUENCE;
        private static readonly Guid eptInterfaceUuid = new Guid("E1AF8308-5D1F-11C9-91A4-08002B14A0FA");
        private const ushort eptInterfaceMajorVersion = 3;
        private const ushort eptInterfaceMinorVersion = 0;

        private static Dictionary<p_syntax_id_t, ushort> endpointMap;
        private static Thread epmThread;
        private static readonly TimeSpan EPM_RECEIVE_LOOP_TIMEOUT = new TimeSpan(0, 0, 1);
        private static RpceServerTransport epmServer;
        private static IPAddress hostIp;

        //This is request stub for EPT Map method.
        private static readonly byte[] eptMapRequestStub = new byte[] { 
                        0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, // object
                        0x02, 0x00, 0x00, 0x00, 0x4B, 0x00, 0x00, 0x00, 
                        0x4B, 0x00, 0x00, 0x00, 0x05, 0x00, 0x13, 0x00, 
                        0x0D, 
                        0x78, 0x56, 0x34, 0x12, 0x34, 0x12, 0xCD, 0xAB, // interface uuid to query, offset == 37
                        0xEF, 0x00, 0x01, 0x23, 0x45, 0x67, 0xCF, 0xFB,
                        0x01, 0x00, // interface ver_major to query, offset = 53
                        0x02, 0x00, 
                        0x00, 0x00, // interface ver_minor to query, offset = 57
                        0x13, 0x00, 0x0D, 
                        0x04, 0x5D, 0x88, 0x8A, 0xEB, 0x1C, 0xC9, 0x11, // NDR uuid to query
                        0x9F, 0xE8, 0x08, 0x00, 0x2B, 0x10, 0x48, 0x60, 
                        0x02, 0x00, // NDR ver_major to query
                        0x02, 0x00, 
                        0x00, 0x00, // NDR ver_minor to query
                        0x01, 0x00, 0x0B, 0x02, 0x00, 0x00, 0x00, 0x01, 
                        0x00, 0x07, 0x02, 0x00, 0x00, 0x87, 0x01, 0x00, 
                        0x09, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, // map_tower + 1 byte pad
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, // entry_handle
                        0x04, 0x00, 0x00, 0x00 // max_towers
                        };

        //This is response stub for EPT Map method.
        private static readonly byte[] eptMapResponseStub = new byte[] { 
                        0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, // entry handle
                        0x01, 0x00, 0x00, 0x00, // tower number, 
                        0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x01, 0x00, 0x00, 0x00, //ArrayInfo
                        0x03, 0x00, 0x00, 0x00, //Tower ptr
                        0x4B, 0x00, 0x00, 0x00, //Length
                        0x4B, 0x00, 0x00, 0x00, //Tower Length
                        0x05, 0x00, //Floor Count
                        0x13, 0x00, 0x0d,
                        0x78, 0x56, 0x34, 0x12, 0x34, 0x12, 0xCD, 0xAB, // interface uuid to query, offset == 52
                        0xEF, 0x00, 0x01, 0x23, 0x45, 0x67, 0xCF, 0xFB,
                        0x01, 0x00, // interface ver_major to query, offset = 68
                        0x02, 0x00, 
                        0x00, 0x00, // interface ver_minor to query, offset = 72
                        0x13, 0x00, 0x0D, 
                        0x04, 0x5D, 0x88, 0x8A, 0xEB, 0x1C, 0xC9, 0x11, // NDR uuid to query
                        0x9F, 0xE8, 0x08, 0x00, 0x2B, 0x10, 0x48, 0x60, 
                        0x02, 0x00, // NDR ver_major to query
                        0x02, 0x00, 
                        0x00, 0x00, // NDR ver_minor to query
                        0x01, 0x00, 0x0B, 0x02, 0x00, 0x00, 0x00, 0x01, 
                        0x00, 0x07, 0x02, 0x00, 0x00, 0x87, 0x01, 0x00, 
                        0x09, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, // map_tower + 1 byte pad
                        0x00, 0x00, 0x00, 0x00 // Status
                        };

        // opnum of EPT map method.
        private const ushort eptMapOpnum = 3;


        /// <summary>
        /// Retrieve dynamic TCP endpoint by interface uuid, ver_major and ver_minor.
        /// </summary>
        /// <param name="serverName">Remote host name to query the interface.</param>
        /// <param name="interfaceId">Interface uuid to query.</param>
        /// <param name="interfaceMajorVersion">Interface ver_major to query.</param>
        /// <param name="interfaceMinorVersion">Interface ver_minor to query.</param>
        /// <returns>TCP endpoints which is querying.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when serverName is null.
        /// </exception>
        public static ushort[] QueryDynamicTcpEndpointByInterface(
            string serverName, 
            Guid interfaceId,
            ushort interfaceMajorVersion,
            ushort interfaceMinorVersion)
        {
            if (serverName == null)
            {
                throw new ArgumentNullException("serverName");
            }

            byte[] requestStub = new byte[eptMapRequestStub.Length];
            Buffer.BlockCopy(eptMapRequestStub, 0, requestStub, 0, eptMapRequestStub.Length);

            // 37 == offset of interface uuid to query
            byte[] buffer = interfaceId.ToByteArray();
            Buffer.BlockCopy(buffer, 0, requestStub, 37, buffer.Length);

            //53 == offset of interface vers_major to query
            buffer = BitConverter.GetBytes(interfaceMajorVersion);
            Buffer.BlockCopy(buffer, 0, requestStub, 53, buffer.Length);

            //57 == offset of interface vers_minor to query
            buffer = BitConverter.GetBytes(interfaceMinorVersion);
            Buffer.BlockCopy(buffer, 0, requestStub, 57, buffer.Length);

            using (RpceClientTransport rpce = new RpceClientTransport())
            {
                rpce.Bind(
                    eptProtocolSequence,
                    serverName,
                    eptEndpoint.ToString(),
                    null,
                    eptInterfaceUuid,
                    eptInterfaceMajorVersion,
                    eptInterfaceMinorVersion,
                    null,
                    RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_NONE,
                    false,
                    queryEptTimeout);

                byte[] responseStub;

                rpce.Call(
                    eptMapOpnum,
                    requestStub,
                    queryEptTimeout,
                    out responseStub);

                if (responseStub == null)
                {
                    throw new InvalidOperationException("Query NRPC interface failed.");
                }
                if (responseStub.Length < sizeof(uint)) // status
                {
                    throw new InvalidOperationException("Query NRPC interface read status code failed.");
                }

                uint status = BitConverter.ToUInt32(responseStub, responseStub.Length - sizeof(uint));
                if (status != 0)
                {
                    throw new InvalidOperationException(
                        string.Format("Query interface failed with status code 0x{0:x}.", status));
                }

                int offset = 20; // skip entry_handle
                uint numTowers = BitConverter.ToUInt32(responseStub, offset);
                offset += 4;
                if (numTowers == 0)
                {
                    throw new InvalidOperationException("Query interface failed, no tower returned.");
                }

                ushort[] endpoints = new ushort[numTowers];

                offset += 12; //skip ITowers array size
                offset += 4 * (int)numTowers; //skip ITowers pointer
                for (int i = 0; i < numTowers; i++)
                {
                    offset += 8; //skip length and tower_length
                    offset += 2; //skip floor count
                    offset += 25; //skip if_id
                    offset += 25; //skip ndr rep
                    offset += 7; //skip proto_id
                    offset += 5; //skip port_addr lhs, id, rhs

                    //Port address is 16-bit unsigned integer, big-endian order.
                    endpoints[i] = (ushort)((responseStub[offset] << 8) + responseStub[offset + 1]);

                    offset += 2; //skip port address
                    offset += 9; //skip host_addr
                    offset += 1; //skip pad
                }

                return endpoints;
            }
        }


        /// <summary>
        /// Start EPM server
        /// Make sure 135 port is shut down, Or other trying to start EPM will fail.
        /// </summary>
        /// <param name="endpointMapper">A mapper from interface to endpoint, ipv4 only for now</param>
        /// <param name="host">The host IP address</param>
        public static void Start(IDictionary<p_syntax_id_t, ushort> endpointMapper, IPAddress host)
        {
            endpointMap = new Dictionary<p_syntax_id_t, ushort>(endpointMapper);
            epmServer = new RpceServerTransport();
            epmThread = new Thread(epmReceiveLoop);
            hostIp = host;
            epmServer.RegisterInterface(eptInterfaceUuid, eptInterfaceMajorVersion, eptInterfaceMinorVersion);
            epmServer.StartTcp(eptEndpoint);
            epmThread.Start();
        }


        /// <summary>
        /// Loop epm receive process
        /// </summary>
        private static void epmReceiveLoop()
        {
            byte[] responseStub = new byte[eptMapResponseStub.Length];
            Buffer.BlockCopy(eptMapResponseStub, 0, responseStub, 0, eptMapResponseStub.Length);

            while (true)
            {
                RpceServerSessionContext context;
                ushort opnum;
                try
                {
                    byte[] requestStub = epmServer.ExpectCall(EPM_RECEIVE_LOOP_TIMEOUT, out context, out opnum);
                
                    if (opnum == eptMapOpnum)
                    {
                        p_syntax_id_t ifSpec = new p_syntax_id_t();
                        // 37 == offset of interface uuid, 16 == the size of uuid
                        ifSpec.if_uuid = new Guid(ArrayUtility.SubArray(requestStub, 37, 16));
                        //53 == offset of interface version
                        ifSpec.if_version = BitConverter.ToUInt32(requestStub, 53);
                        if (endpointMap.ContainsKey(ifSpec))
                        {
                            //copy tower from request stub to response stub
                            //37 is the offset to interface uuid in request
                            //53 is the offset to interface uuid in response
                            Buffer.BlockCopy(requestStub, 37, responseStub, 53, 70);
                            //modify the endpoint in big enddian oder, offset is 112
                            responseStub[112] = (byte)(endpointMap[ifSpec] >> 8);
                            responseStub[113] = (byte)endpointMap[ifSpec];
                            //modify the address, offset is 119
                            Buffer.BlockCopy(hostIp.GetAddressBytes(), 0, responseStub, 119, 4);
                            epmServer.SendResponse(context, responseStub);
                        }
                    }
                }
                catch (TimeoutException)
                {
                    continue;
                }
            }
        }


        /// <summary>
        /// Stop EPM server
        /// </summary>
        public static void Stop()
        {
            epmServer.StopTcp(eptEndpoint);
            epmThread.Abort();
        }
    }
}
