// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;


namespace Microsoft.Protocols.TestTools.StackSdk.Swn
{
    public static class SwnUtility
    {
        #region Fields
        /// <summary>
        /// SWN interface UUID
        /// </summary>
        public static readonly Guid SWN_INTERFACE_UUID = new Guid("ccd8c074-d0e5-4a40-92b4-d074faa6ba28");

        /// <summary>
        /// SWN interface major version
        /// </summary>
        public const ushort SWN_INTERFACE_MAJOR_VERSION = 1;

        /// <summary>
        /// SWN interface minor version
        /// </summary>
        public const ushort SWN_INTERFACE_MINOR_VERSION = 1;
        #endregion

        #region utility method
        /// <summary>
        /// Retrieve SWN TCP endpoint of a server
        /// </summary>
        /// <param name="serverName">SWN server name</param>
        /// <returns>TCP endpoints</returns>
        /// <exception cref="ArgumentNullException">Thrown when serverName is null or empty</exception>
        public static ushort[] QueryEndpoints(string serverName)
        {
            if (string.IsNullOrEmpty(serverName))
            {
                throw new ArgumentNullException("serverName");
            }

            return RpceEndpointMapper.QueryDynamicTcpEndpointByInterface(
                serverName,
                SWN_INTERFACE_UUID,
                SWN_INTERFACE_MAJOR_VERSION,
                SWN_INTERFACE_MINOR_VERSION);
        }

        /// <summary>
        /// Parse IPADDR_INFO_LIST from RESP_ASYNC_NOTIFY.
        /// </summary>
        /// <param name="respNotify">A pointer to a PRESP_ASYNC_NOTIFY structure, as specified in section 2.2.1.7.</param>
        /// <param name="IPAddrInfoList">IPADDR_INFO_LIST list.</param>
        public static void ParseIPAddrInfoList(RESP_ASYNC_NOTIFY respNotify, out IPADDR_INFO_LIST IPAddrInfoList)
        {
            IPAddrInfoList = new IPADDR_INFO_LIST();
            IPAddrInfoList.Length = BitConverter.ToUInt32(respNotify.MessageBuffer, 0);
            IPAddrInfoList.Reserved = BitConverter.ToUInt32(respNotify.MessageBuffer, sizeof(int));
            IPAddrInfoList.IPAddrInstances = BitConverter.ToUInt32(respNotify.MessageBuffer, 2 * sizeof(int));
            IPAddrInfoList.IPAddrList = new IPADDR_INFO[IPAddrInfoList.IPAddrInstances];

            int len = 3 * sizeof(int);
            for (int i = 0; i < IPAddrInfoList.IPAddrInstances; i++)
            {
                IPAddrInfoList.IPAddrList[i].Flags = BitConverter.ToUInt32(respNotify.MessageBuffer, len);
                if ((IPAddrInfoList.IPAddrList[i].Flags & (uint)SwnIPAddrInfoFlags.IPADDR_V4) != 0)
                {
                    IPAddrInfoList.IPAddrList[i].IPV4 = BitConverter.ToUInt32(respNotify.MessageBuffer, len + sizeof(int));
                }
                IPAddrInfoList.IPAddrList[i].IPV6 = new ushort[8];
                if ((IPAddrInfoList.IPAddrList[i].Flags & (uint)SwnIPAddrInfoFlags.IPADDR_V6) != 0)
                {
                    Array.Copy(respNotify.MessageBuffer, len + 2 * sizeof(int), IPAddrInfoList.IPAddrList[i].IPV6, 0, 8);
                }
                len += 2 * sizeof(int) + 8 * sizeof(ushort);
            }
        }

        /// <summary>
        /// Parse RESOURCE_CHANGE from RESP_ASYNC_NOTIFY.
        /// </summary>
        /// <param name="respNotify">A pointer to a PRESP_ASYNC_NOTIFY structure, as specified in section 2.2.1.7.</param>
        /// <param name="resourceChangeList">RESOURCE_CHANGE list.</param>
        public static void ParseResourceChange(RESP_ASYNC_NOTIFY respNotify, out RESOURCE_CHANGE[] resourceChangeList)
        {
            resourceChangeList = new RESOURCE_CHANGE[respNotify.NumberOfMessages];
            int len = 0;
            for (int i = 0; i < respNotify.NumberOfMessages; i++)
            {
                resourceChangeList[i] = new RESOURCE_CHANGE();
                resourceChangeList[i].Length = BitConverter.ToUInt32(respNotify.MessageBuffer, len);
                resourceChangeList[i].ChangeType = BitConverter.ToUInt32(respNotify.MessageBuffer, len + sizeof(int));
                resourceChangeList[i].ResourceName =
                    Encoding.Unicode.GetString(respNotify.MessageBuffer,
                    len + 2 * sizeof(int),
                    (int)(resourceChangeList[i].Length - 2 * sizeof(int)));

                len += (int)resourceChangeList[i].Length;
            }
        }
        #endregion
    }
}
