// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// Convert HostName or Ip to IpAddress
    /// </summary>
    public static class IpUtility
    {
        /// <summary>
        /// Parse hostname or ip string to IPAddress
        /// </summary>
        /// <param name="hostNameOrIpAddress">HostName or Ip String</param>
        /// <returns>IPV4 Address</returns>
        public static IPAddress ParseIPAddress(this string hostNameOrIpAddress)
        {
            IPAddress ipAddress;

            if (string.IsNullOrEmpty(hostNameOrIpAddress))
            {
                return IPAddress.None;
            }

            bool isIp = IPAddress.TryParse(hostNameOrIpAddress, out ipAddress);
            if (!isIp)
            {
                try
                {
                    var entry = Dns.Resolve(hostNameOrIpAddress);
                    ipAddress = entry.AddressList.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).First();
                }
                catch
                {
                    throw new System.Exception(string.Format("Cannot resolve IP address of ({0}) from DNS.", hostNameOrIpAddress));
                }
            }

            return ipAddress;
        }

        /// <summary>
        /// Parse host name or IP string to optional secondary IPAddress
        /// </summary>
        /// <param name="hostNameOrIpAddress">HostName or IP String</param>
        /// <returns>Secondary IPV4 Address</returns>
        public static IPAddress ParseSecondaryIPAddress(this string hostNameOrIpAddress)
        {
            IPAddress ipAddress;

            if (string.IsNullOrEmpty(hostNameOrIpAddress))
            {
                return IPAddress.None;
            }

            bool isIp = IPAddress.TryParse(hostNameOrIpAddress, out ipAddress);
            if (!isIp)
            {
                try
                {
                    var entry = Dns.Resolve(hostNameOrIpAddress);
                    var ipAddresses = entry.AddressList.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                    if (ipAddresses.Count() > 1)
                    {
                        ipAddress = ipAddresses.ElementAt(1);
                    }
                    else
                    {
                        ipAddress = IPAddress.None;
                    }
                }
                catch
                {
                    ipAddress = IPAddress.None;
                }
            }

            return ipAddress;
        }

        /// <summary>
        /// Parse hostname or ip string to IPAddress Arrary
        /// </summary>
        /// <param name="hostNameOrIpAddress">HostName or Ip Address, "," or ";" between each hostname or Ip</param>
        /// <returns>IpAddress Arrary</returns>
        public static IPAddress[] ParseArraryIPAddress(this string hostNameOrIpAddress)
        {
            if (string.IsNullOrEmpty(hostNameOrIpAddress))
            {
                return new IPAddress[0];
            }

            List<IPAddress> ipList = new List<IPAddress>();
            foreach (string nameOrIp in hostNameOrIpAddress.Split(new char[] { ',', ';' }))
            {
                ipList.Add(nameOrIp.ParseIPAddress());
            }
            return ipList.ToArray();
        }
    }
}
