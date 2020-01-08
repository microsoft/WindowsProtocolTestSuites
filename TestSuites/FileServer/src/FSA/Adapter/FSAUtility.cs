// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter
{
    /// <summary>
    /// FsaUtility
    /// </summary>
    public static class FsaUtility
    {
        /// <summary>
        /// Transfer the result to the expected value when needed.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="reqId">The requirement ID which need to be disabled.</param>
        /// <param name="expectedValue">The expected result value.</param>
        /// <param name="actualValue">The actual result value.</param>
        /// <param name="site">The default test site.</param>
        /// <returns>The value after transferring.</returns>
        public static T TransferExpectedResult<T>(int reqId, T expectedValue, T actualValue, ITestSite site)
        {
            T retResult;
            bool bIsDisabledRequirementCheck = IsRequirementVerificationDisabledInTestcase(site, reqId);
            if (bIsDisabledRequirementCheck)
            {
                retResult = expectedValue;
            }
            else
            {
                retResult = actualValue;
            }
            return retResult;
        }

        /// <summary>
        /// This method checks if a requirement verification needs to be disabled.
        /// </summary>
        /// <param name="site">The related ITestSite instance.</param>
        /// <param name="requirementId">The requirement ID to be checked.</param>
        /// <returns>True indicates the requirement is disabled.</returns>
        public static bool IsRequirementVerificationDisabledInTestcase(ITestSite site, int requirementId)
        {
            List<int> disabledRequirements = new List<int>();

            // Check if there is a ReqSwitch in the config.
            string strReqIds = site.Properties[ConfigPropNames.ReqSwitchTestcase];
            if (String.IsNullOrEmpty(strReqIds.Trim()))
            {
                return false;
            }
            string[] ids = strReqIds.Split(new char[] { ',' });
            foreach (string id in ids)
            {
                string sid = id.Trim();
                int nid;
                if (!int.TryParse(sid, out nid))
                {
                    site.Assume.Fail("The value's format of disabled requirements property '{0}' is incorrect. For example, '1000, 1002'", ConfigPropNames.ReqSwitchTestcase);
                }
                disabledRequirements.Add(nid);
            }

            return disabledRequirements.Contains(requirementId);
        }

        /// <summary>
        /// Convert Sid string to binary.
        /// </summary>
        /// <param name="sid">The sid string.</param>
        /// <returns>The sid binary.</returns>
        public static byte[] ConvertSID2Bytes(string sid)
        {
 
            sid = sid.ToLower().Replace("s", "");
 
            string[] list = sid.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("{0:X2}", Convert.ToInt32(list[0])));
            sb.Append(String.Format("{0:X2}", Convert.ToInt32(list.Length - 2)));
            sb.Append(String.Format("{0:X12}", Convert.ToInt32(list[1])));
 
            Console.WriteLine(sb.ToString());
 
            for (int i = 2; i < list.Length; i++)
            {
                string tmp = String.Format("{0:X8}", Convert.ToUInt32(list[i]));
                for (int j = 6; j >= 0; j -= 2)
                {
                    sb.Append(tmp.Substring(j, 2));
                }
            }
 
            string obj = sb.ToString();
            byte[] ret = new byte[obj.Length / 2];
 
            for (int i = 0; i < obj.Length; i += 2)
            {
                ret[i / 2] = Convert.ToByte(obj.Substring(i, 2), 16);
            }
 
            return ret;
        }

        /// <summary>
        /// To check if a positive number is a power of two
        /// </summary>
        /// <param name="x">Positive number.</param>
        /// <returns>Return true if it is a power of two, else return false.</returns>
        public static bool IsPowerOfTwo(ulong x)
        {
            return (x & (x - 1)) == 0;
        }

        /// <summary>
        /// Get IPAddress from server name.
        /// </summary>
        /// <param name="serverName">Computer Name of the server.</param>
        /// <param name="isIpv4">Set to TRUE for IPv4, and set to FALSE for IPv6.</param>
        /// <returns>IPv4 or IPv6 address according to isIPv4 value.</returns>
        public static IPAddress GetIpAddress(string serverName, bool isIpv4)
        {
            IPAddress[] addresses = Dns.GetHostAddresses(serverName);

            foreach (IPAddress address in addresses)
            {
                if (isIpv4)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return address;
                    }
                }
                else
                {
                    if (address.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        return address;
                    }
                }
            }
            return null; //Return null if not suitable IP Address found.
        }

        /// <summary>
        /// To check if the checksumAlgorithm if one of defined value.
        /// </summary>
        /// <param name="checksumAlgorithm">ChecksumAlgorithm to verify.</param>
        /// <returns>A boolean value indicate if the result is correct.</returns>
        public static bool IsOneOfExpectedChecksumAlgorithm(FSCTL_GET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM checksumAlgorithm)
        {
            switch (checksumAlgorithm)
            {
                case FSCTL_GET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM.CHECKSUM_TYPE_NONE:
                case FSCTL_GET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM.CHECKSUM_TYPE_CRC64:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Unmarshal File information class from a byte array to a structure array, by using field NextEntryOffset
        /// To use this function, make sure the first 32-bit of the File information class is NextEntryOffset.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static T[] UnmarshalFileInformationArray<T>(byte[] buffer) where T: struct
        {
            if (buffer == null || buffer.Length <= 0)
            {
                throw new Exception("The input buffer could not be null or empty.");
            }
            List<T> listFileInformation = new List<T>();
            int offset = 0;
            while (offset < buffer.Length)
            {
                T fileQuotaInformationStruct = TypeMarshal.ToStruct<T>(buffer.Skip((int)offset).ToArray());
                listFileInformation.Add(fileQuotaInformationStruct);

                // The first 32-bit of the structure is NextEntryOffset
                int nextEntryOffset = BitConverter.ToInt32(buffer, offset);
                if (nextEntryOffset == 0)
                {
                    break; //If there are no subsequent structures, the NextEntryOffset field MUST be 0.
                }
                else
                {
                    offset += nextEntryOffset;
                }
            }
            return listFileInformation.ToArray();
        }

    }
}
