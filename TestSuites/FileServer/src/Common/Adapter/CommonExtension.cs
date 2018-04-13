// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using System.Net;

namespace Microsoft.Protocols.TestSuites.FileSharing.Common
{
    public static class CommonExtension
    {
        /// <summary>
        /// Parse hostname or ip string to IPAddress
        /// </summary>
        /// <param name="hostNameOrIpAddress">HostName or Ip String</param>
        /// <returns>IPV4 Address</returns>
        public static IPAddress ParseIPAddress(this string hostNameOrIpAddress)
        {
            IPAddress ipAddress;
            bool isIp = IPAddress.TryParse(hostNameOrIpAddress, out ipAddress);
            if(!isIp)
            {
                IPAddress[] ipAddresses = Dns.GetHostAddresses(hostNameOrIpAddress);
                ipAddress = ipAddresses.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).First();
            }
            
            return ipAddress;
        }
    }
}
