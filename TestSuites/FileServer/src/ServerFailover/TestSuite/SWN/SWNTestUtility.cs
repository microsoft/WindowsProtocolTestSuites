// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.Swn;
using System;
using System.Linq;
using System.Net;

namespace Microsoft.Protocols.TestSuites.FileSharing.ServerFailover.TestSuite
{
    public class SWNTestUtility
    {
        private static ITestSite baseTestSite;

        public static string swnWitnessName = "SmbWitness";

        /// <summary>
        /// The base test site
        /// </summary>
        public static ITestSite BaseTestSite
        {
            set
            {
                baseTestSite = value;
            }
            get
            {
                return baseTestSite;
            }
        }

        #region Print methods
        /// <summary>
        /// Print WITNESS_INTERFACE_LIST.
        /// </summary>
        /// <param name="interfaceList">Interface list.</param>
        [CLSCompliant(false)]
        public static void PrintInterfaceList(WITNESS_INTERFACE_LIST interfaceList)
        {
            BaseTestSite.Log.Add(LogEntryKind.Debug, "WITNESS_INTERFACE_LIST length: {0}", interfaceList.NumberOfInterfaces);
            for (int i = 0; i < interfaceList.NumberOfInterfaces; i++)
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "\tInterface {0}:", i);
                BaseTestSite.Log.Add(LogEntryKind.Debug, "\t\tInterfaceGroupName: {0}", interfaceList.InterfaceInfo[i].InterfaceGroupName);
                BaseTestSite.Log.Add(LogEntryKind.Debug, "\t\tVersion: {0:X08}", interfaceList.InterfaceInfo[i].Version);
                BaseTestSite.Log.Add(LogEntryKind.Debug, "\t\tNodeState: {0}", interfaceList.InterfaceInfo[i].NodeState);
                BaseTestSite.Log.Add(LogEntryKind.Debug, "\t\tIPV4: {0}", (new IPAddress(interfaceList.InterfaceInfo[i].IPV4)).ToString());
                BaseTestSite.Log.Add(LogEntryKind.Debug, "\t\tIPV6: {0}", ConvertIPV6(interfaceList.InterfaceInfo[i].IPV6).ToString());
                BaseTestSite.Log.Add(LogEntryKind.Debug, "\t\tFlags: {0:x8}", interfaceList.InterfaceInfo[i].Flags);
            }
        }

        /// <summary>
        /// Print RESP_ASYNC_NOTIFY
        /// </summary>
        /// <param name="respNotify">Asynchronous notification</param>
        public static void PrintNotification(RESP_ASYNC_NOTIFY respNotify)
        {
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Receive asynchronous notification");
            switch ((SwnMessageType)respNotify.MessageType)
            {
                case SwnMessageType.RESOURCE_CHANGE_NOTIFICATION:
                    {
                        RESOURCE_CHANGE[] resourceChangeList;
                        SwnUtility.ParseResourceChange(respNotify, out resourceChangeList);

                        BaseTestSite.Log.Add(LogEntryKind.Debug, "\tRESOURCE_CHANGE");
                        BaseTestSite.Log.Add(LogEntryKind.Debug, "\tcount: {0}", resourceChangeList.Length);

                        foreach (var res in resourceChangeList)
                        {
                            BaseTestSite.Log.Add(LogEntryKind.Debug, "\t\tResource name: {0}", res.ResourceName.Substring(0, res.ResourceName.Length - 1));
                            switch ((SwnResourceChangeType)res.ChangeType)
                            {
                                case SwnResourceChangeType.RESOURCE_STATE_UNKNOWN:
                                    BaseTestSite.Log.Add(LogEntryKind.Debug, "\t\tChange type: RESOURCE_STATE_UNKNOWN");
                                    break;
                                case SwnResourceChangeType.RESOURCE_STATE_AVAILABLE:
                                    BaseTestSite.Log.Add(LogEntryKind.Debug, "\t\tChange type: RESOURCE_STATE_AVAILABLE");
                                    break;
                                case SwnResourceChangeType.RESOURCE_STATE_UNAVAILABLE:
                                    BaseTestSite.Log.Add(LogEntryKind.Debug, "\t\tChange type: RESOURCE_STATE_UNAVAILABLE");
                                    break;
                                default:
                                    BaseTestSite.Log.Add(LogEntryKind.Debug, "\t\tChange type: Unknown type {0}", res.ChangeType);
                                    break;
                            }
                        }
                    }
                    break;
                case SwnMessageType.CLIENT_MOVE_NOTIFICATION:
                    {
                        IPADDR_INFO_LIST IPAddrInfoList;
                        SwnUtility.ParseIPAddrInfoList(respNotify, out IPAddrInfoList);
                        BaseTestSite.Log.Add(LogEntryKind.Debug, "\tCLIENT_MOVE");
                        BaseTestSite.Log.Add(LogEntryKind.Debug, "\tReserved: {0}", IPAddrInfoList.Reserved);
                        BaseTestSite.Log.Add(LogEntryKind.Debug, "\tIP address count: {0}", IPAddrInfoList.IPAddrInstances);

                        foreach (var ip in IPAddrInfoList.IPAddrList)
                        {
                            BaseTestSite.Log.Add(LogEntryKind.Debug, "\tFlags: {0}", ip.Flags);
                            if (((uint)SwnIPAddrInfoFlags.IPADDR_V4 & ip.Flags) != 0)
                            {
                                BaseTestSite.Log.Add(LogEntryKind.Debug, "\tIPAddr V4: {0}", (new IPAddress(ip.IPV4)).ToString());
                            }
                            if (((uint)SwnIPAddrInfoFlags.IPADDR_V6 & ip.Flags) != 0)
                            {
                                BaseTestSite.Log.Add(LogEntryKind.Debug, "\tIPAddr V6: {0}", ConvertIPV6(ip.IPV6).ToString());
                            }
                        }
                    }
                    break;
                case SwnMessageType.SHARE_MOVE_NOTIFICATION:
                    {
                        IPADDR_INFO_LIST IPAddrInfoList;
                        SwnUtility.ParseIPAddrInfoList(respNotify, out IPAddrInfoList);
                        BaseTestSite.Log.Add(LogEntryKind.Debug, "\tSHARE_MOVE");
                        BaseTestSite.Log.Add(LogEntryKind.Debug, "\tReserved: {0}", IPAddrInfoList.Reserved);
                        BaseTestSite.Log.Add(LogEntryKind.Debug, "\tIP address count: {0}", IPAddrInfoList.IPAddrInstances);

                        foreach (var ip in IPAddrInfoList.IPAddrList)
                        {
                            BaseTestSite.Log.Add(LogEntryKind.Debug, "\tFlags: {0}", ip.Flags);
                            if (((uint)SwnIPAddrInfoFlags.IPADDR_V4 & ip.Flags) != 0)
                            {
                                BaseTestSite.Log.Add(LogEntryKind.Debug, "\tIPAddr V4: {0}", (new IPAddress(ip.IPV4)).ToString());
                            }
                            if (((uint)SwnIPAddrInfoFlags.IPADDR_V6 & ip.Flags) != 0)
                            {
                                BaseTestSite.Log.Add(LogEntryKind.Debug, "\tIPAddr V6: {0}", ConvertIPV6(ip.IPV6).ToString());
                            }
                        }
                    }
                    break;
                case SwnMessageType.IP_CHANGE_NOTIFICATION:
                    {
                        IPADDR_INFO_LIST IPAddrInfoList;
                        SwnUtility.ParseIPAddrInfoList(respNotify, out IPAddrInfoList);
                        BaseTestSite.Log.Add(LogEntryKind.Debug, "\tIP_CHANGE");
                        BaseTestSite.Log.Add(LogEntryKind.Debug, "\tReserved: {0}", IPAddrInfoList.Reserved);
                        BaseTestSite.Log.Add(LogEntryKind.Debug, "\tIP address count: {0}", IPAddrInfoList.IPAddrInstances);

                        foreach (var ip in IPAddrInfoList.IPAddrList)
                        {
                            BaseTestSite.Log.Add(LogEntryKind.Debug, "\tFlags: {0}", ip.Flags);
                            if (((uint)SwnIPAddrInfoFlags.IPADDR_V4 & ip.Flags) != 0)
                            {
                                BaseTestSite.Log.Add(LogEntryKind.Debug, "\tIPAddr V4: {0}", (new IPAddress(ip.IPV4)).ToString());
                            }
                            if (((uint)SwnIPAddrInfoFlags.IPADDR_V6 & ip.Flags) != 0)
                            {
                                BaseTestSite.Log.Add(LogEntryKind.Debug, "\tIPAddr V6: {0}", ConvertIPV6(ip.IPV6).ToString());
                            }
                        }
                    }
                    break;
                default:
                    BaseTestSite.Assert.Fail("\t\tMessage type: Unknown type {0}", respNotify.MessageType);
                    break;
            }
        }
        #endregion

        /// <summary>
        /// Get principle name without domain name.
        /// </summary>
        /// <param name="domainName">Domain name</param>
        /// <param name="serverName">Server name</param>
        /// <returns>Return principle name</returns>
        public static string GetPrincipleName(string domainName, string serverName)
        {
            if (serverName.Contains(domainName))
            {
                return serverName.Substring(0, serverName.Length - domainName.Length - 1);
            }

            return serverName;
        }

        /// <summary>
        /// Bind RPC server.
        /// </summary>
        /// <param name="swnClient">SWN rpc client</param>
        /// <param name="networkAddress">RPC network address</param>
        /// <param name="domainName">Domain name</param>
        /// <param name="userName">User name</param>
        /// <param name="password">Password</param>
        /// <param name="securityPackage">Security package</param>
        /// <param name="authLevel">Authentication level</param>
        /// <param name="timeout">Timeout</param>
        /// <param name="serverComputerName">ServerComputerName</param>
        /// <returns>Return true if success, otherwise return false</returns>
        public static bool BindServer(SwnClient swnClient, IPAddress networkAddress, string domainName, string userName, string password,
            SecurityPackageType securityPackage, RpceAuthenticationLevel authLevel, TimeSpan timeout, string serverComputerName = null)
        {
            AccountCredential accountCredential = new AccountCredential(domainName, userName, password);
            string cifsServicePrincipalName = string.Empty;
            if (!string.IsNullOrEmpty(serverComputerName))
            {
                cifsServicePrincipalName = "cifs/" + serverComputerName;
            }
            else
            {
                IPHostEntry hostEntry = null;
                try
                {
                    hostEntry = Dns.GetHostEntry(networkAddress);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Failed to resolve network address {0} with exception: {1}", networkAddress.ToString(), ex.Message));
                }

                if (hostEntry != null && !string.IsNullOrEmpty(hostEntry.HostName))
                {
                    cifsServicePrincipalName = "cifs/" + hostEntry.HostName;
                }
                else
                {
                    throw new Exception("Failed to get HostName from network address " + networkAddress.ToString());
                }
            }
            ClientSecurityContext securityContext =
                new SspiClientSecurityContext(
                    securityPackage,
                    accountCredential,
                    cifsServicePrincipalName,
                    ClientSecurityContextAttribute.Connection
                        | ClientSecurityContextAttribute.DceStyle
                        | ClientSecurityContextAttribute.Integrity
                        | ClientSecurityContextAttribute.ReplayDetect
                        | ClientSecurityContextAttribute.SequenceDetect
                        | ClientSecurityContextAttribute.UseSessionKey,
                    SecurityTargetDataRepresentation.SecurityNativeDrep);

            try
            {
                //Bind
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Start to Bind RPC to {0}.", networkAddress.ToString());
                swnClient.SwnBind(networkAddress.ToString(), accountCredential, securityContext, authLevel, timeout);
            }
            catch (Exception ex)
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug,
                    "Bind server {0} failed. Exception: {1}",
                    networkAddress.ToString(),
                    ex.Message);
                swnClient.SwnUnbind(timeout);
                return false;
            }

            BaseTestSite.Log.Add(LogEntryKind.Debug, "Bind server {0} successfully.", networkAddress.ToString());
            return true;
        }

        /// <summary>
        /// Get the witness server to be used to register.
        /// </summary>
        /// <param name="interfaceList">The interface list from server.</param>
        /// <param name="registerInterface">Interface to register.</param>
        public static void GetRegisterInterface(WITNESS_INTERFACE_LIST interfaceList, out WITNESS_INTERFACE_INFO registerInterface)
        {
            registerInterface = new WITNESS_INTERFACE_INFO();

            foreach (var info in interfaceList.InterfaceInfo)
            {
                if (info.NodeState == SwnNodeState.AVAILABLE && (info.Flags & (uint)SwnNodeFlagsValue.INTERFACE_WITNESS) != 0)
                {
                    registerInterface = info;

                    BaseTestSite.Log.Add(LogEntryKind.Debug, "Register to interface:");
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "\tInterface group name: {0}", registerInterface.InterfaceGroupName);
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "\tInterface IPV4 address: {0}", new IPAddress(registerInterface.IPV4).ToString());
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "\tInterface IPV6 address: {0}", SWNTestUtility.ConvertIPV6(registerInterface.IPV6).ToString());
                    return;
                }
            }

            BaseTestSite.Assert.Fail("Cannot get register interface.");
        }

        /// <summary>
        /// Get the witness server to be used to access.
        /// </summary>
        /// <param name="interfaceList">The interface list from server.</param>
        /// <param name="accessInterface">Interface to access.</param>
        public static void GetAccessInterface(WITNESS_INTERFACE_LIST interfaceList, out WITNESS_INTERFACE_INFO accessInterface)
        {
            accessInterface = new WITNESS_INTERFACE_INFO();

            foreach (var info in interfaceList.InterfaceInfo)
            {
                if (info.NodeState == SwnNodeState.AVAILABLE && (info.Flags & (uint)SwnNodeFlagsValue.INTERFACE_WITNESS) == 0)
                {
                    accessInterface = info;

                    BaseTestSite.Log.Add(LogEntryKind.Debug, "Access interface:");
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "\tInterface group name: {0}", accessInterface.InterfaceGroupName);
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "\tInterface IPV4 address: {0}", new IPAddress(accessInterface.IPV4).ToString());
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "\tInterface IPV6 address: {0}", SWNTestUtility.ConvertIPV6(accessInterface.IPV6).ToString());
                    return;
                }
            }

            BaseTestSite.Assert.Fail("Cannot get register interface.");
        }

        /// <summary>
        /// Verify the interface list.
        /// </summary>
        /// <param name="interfaceList">The interface list from server.</param>
        /// <param name="platform">SUT platform.</param>
        /// <param name="failCaseIfProtocolVersionIsInvalid">Indicates if failing the case if the verification of protocol version fails</param>
        public static bool VerifyInterfaceList(WITNESS_INTERFACE_LIST interfaceList, Platform platform, bool failCaseIfProtocolVersionIsInvalid = false)
        {
            bool ret = false;

            PrintInterfaceList(interfaceList);
            BaseTestSite.Assert.AreNotEqual<uint>(0, interfaceList.NumberOfInterfaces, "WitnessrGetInterfaceList MUST return at least one available interface.");

            foreach (var info in interfaceList.InterfaceInfo)
            {
                if (info.InterfaceGroupName == "")
                {
                    BaseTestSite.Assert.Fail("The InterfaceGroupName is empty.");
                }

                if (info.NodeState == SwnNodeState.AVAILABLE && (info.Flags & (uint)SwnNodeFlagsValue.INTERFACE_WITNESS) != 0)
                {
                    ret = true;
                }

                #region Check Version
                switch ((SwnVersion)info.Version)
                {
                    case SwnVersion.SWN_VERSION_1:
                        BaseTestSite.Log.Add(LogEntryKind.Debug,
                            "The Version of SWN service on {0} is 0x{1:X08}",
                            info.InterfaceGroupName,
                            (uint)SwnVersion.SWN_VERSION_1);
                        break;
                    case SwnVersion.SWN_VERSION_2:
                        BaseTestSite.Log.Add(LogEntryKind.Debug,
                            "The Version of SWN service on {0} is 0x{1:X08}",
                            info.InterfaceGroupName,
                            (uint)SwnVersion.SWN_VERSION_2);
                        break;
                    case SwnVersion.SWN_VERSION_UNKNOWN:
                        if (platform >= Platform.WindowsServer2012R2)
                        {
                            // Windows Server 2012 R2, Windows Server 2016, Windows Server operating system, and Windows Server 2019 initialize the value of Version to 0xFFFFFFFF.
                            BaseTestSite.Log.Add(LogEntryKind.Debug,
                            "The Version of SWN service on {0} is 0x{1:X08}",
                            info.InterfaceGroupName,
                            (uint)SwnVersion.SWN_VERSION_UNKNOWN);
                        }
                        else
                        {
                            BaseTestSite.Assert.Fail(
                            "The Version of SWN service on {0} is unknown 0x{1:X08}",
                            info.InterfaceGroupName,
                            info.Version);
                        }
                        break;
                    default:
                        BaseTestSite.Assert.Fail(
                            "The Version of SWN service on {0} is unknown 0x{1:X08}",
                            info.InterfaceGroupName,
                            info.Version);
                        break;
                }
                #endregion

                if ((info.Flags & (uint)SwnNodeFlagsValue.IPv4) != 0 && info.IPV4 == 0)
                {
                    BaseTestSite.Assert.Fail("The IPV4 {0} of {1} is invalid.",
                        new IPAddress(info.IPV4).ToString(), info.InterfaceGroupName);
                }
                else if ((info.Flags & (uint)SwnNodeFlagsValue.IPv6) != 0 && info.IPV6.All(ip => ip == 0))
                {
                    BaseTestSite.Assert.Fail("The IPV6 {0} of {1} is invalid.",
                        ConvertIPV6(info.IPV6).ToString(), info.InterfaceGroupName);
                }
            }

            return ret;
        }

        /// <summary>
        /// Verify RESOURCE_CHANGE_NOTIFICATION in Asynchronous Notification
        /// </summary>
        /// <param name="respNotify">Asynchronous notification</param>
        /// <param name="changeType">State change of the resource</param>
        public static void VerifyResourceChange(RESP_ASYNC_NOTIFY respNotify, SwnResourceChangeType changeType)
        {
            BaseTestSite.Assert.AreEqual<uint>((uint)SwnMessageType.RESOURCE_CHANGE_NOTIFICATION,
                respNotify.MessageType, "Expect MessageType is set to RESOURCE_CHANGE_NOTIFICATION");

            RESOURCE_CHANGE[] resourceChangeList;
            SwnUtility.ParseResourceChange(respNotify, out resourceChangeList);
            BaseTestSite.Assert.AreEqual<uint>(0x00000001, respNotify.NumberOfMessages, "Expect NumberOfMessages is set to 1.");

            BaseTestSite.Assert.AreEqual<uint>((uint)changeType, resourceChangeList[0].ChangeType, "Expect ChangeType is set to {0}.", changeType);
        }
        /// <summary>
        /// Verify CLIENT_MOVE_NOTIFICATION/SHARE_MOVE_NOTIFICATION/IP_CHANGE_NOTIFICATION  in Asynchronous Notification
        /// </summary>
        /// <param name="respNotify">Asynchronous notification</param>
        /// <param name="expectedMessageType">Expected message type</param>
        /// <param name="expectedIPAddrInforFlag">Expected flag</param>
        /// <param name="platform">Platform of SUT</param>
        public static void VerifyClientMoveShareMoveAndIpChange(RESP_ASYNC_NOTIFY respNotify, SwnMessageType expectedMessageType, uint expectedIPAddrInforFlag, Platform platform)
        {
            BaseTestSite.Assert.AreEqual<uint>((uint)expectedMessageType,
                respNotify.MessageType, "Expect MessageType is set to " + expectedMessageType.ToString());
            BaseTestSite.Assert.AreEqual<uint>(1,
                respNotify.NumberOfMessages, "NumberOfMessages MUST be set to 1.");

            IPADDR_INFO_LIST IPAddrInfoList;
            SwnUtility.ParseIPAddrInfoList(respNotify, out IPAddrInfoList);

            BaseTestSite.Assert.AreEqual<uint>(respNotify.Length, IPAddrInfoList.Length,
                "Expect Length is the size of the IPADDR_INFO_LIST structure.");
            BaseTestSite.Assert.AreEqual<uint>(0, IPAddrInfoList.Reserved,
                "Expect Reserved is 0.");
            BaseTestSite.Assert.IsTrue(IPAddrInfoList.IPAddrInstances >= 1,
                "Expect that there is at least one available IPAddress in the IPADDR_INFO structures.");
            BaseTestSite.Assert.AreEqual<uint>(IPAddrInfoList.IPAddrInstances, (uint)IPAddrInfoList.IPAddrList.Length,
                "Expect that the length of IPAddrList equals IPAddrInstances .");

            for (int i = 0; i < IPAddrInfoList.IPAddrInstances; i++)
            {
                /// 2.2.2.1   IPADDR_INFO
                /// Flags (4 bytes):  The Flags field SHOULD<1> be set to a combination of one or more of the following values.
                /// <1> Section 2.2.2.1:  Windows Server 2012 and Windows Server 2012 R2 set the undefined Flags field bits to arbitrary values.
                if (platform == Platform.NonWindows)
                {
                    BaseTestSite.Assert.AreEqual<uint>(expectedIPAddrInforFlag, IPAddrInfoList.IPAddrList[i].Flags,
                        "Expect the Flags in IPADDR_INFO structures in the IPAddrList equals " + expectedIPAddrInforFlag.ToString());
                }

                if ((IPAddrInfoList.IPAddrList[i].Flags & (uint)SwnNodeFlagsValue.IPv4) != 0 && IPAddrInfoList.IPAddrList[i].IPV4 == 0)
                {
                    BaseTestSite.Assert.Fail("The IPV4 {0} in IPAddrInfoList.IPAddrList is invalid.",
                        new IPAddress(IPAddrInfoList.IPAddrList[i].IPV4).ToString());
                }
                else if ((IPAddrInfoList.IPAddrList[i].Flags & (uint)SwnNodeFlagsValue.IPv6) != 0 && IPAddrInfoList.IPAddrList[i].IPV6.All(ip => ip == 0))
                {
                    BaseTestSite.Assert.Fail("The IPV6 {0} in IPAddrInfoList.IPAddrList is invalid.",
                        ConvertIPV6(IPAddrInfoList.IPAddrList[i].IPV6).ToString());
                }
            }
        }

        /// <summary>
        /// Get the ip address of current accessed node.
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        public static IPAddress GetCurrentAccessIP(string server)
        {
            IPAddress[] accessIpList = Dns.GetHostEntry(server).AddressList;
            IPAddress currentAccessIp = accessIpList[0];
            BaseTestSite.Assume.AreNotEqual(null, currentAccessIp, "Access IP to the file server should NOT be empty.");
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Got IP {0} to access the file server", currentAccessIp.ToString());
            return currentAccessIp;
        }

        /// <summary>
        /// Get the list of InterfaceGroupName.
        /// </summary>
        /// <param name="interfaceList">The interface list from server.</param>
        /// <returns>InterfaceGroupName array. 
        /// Index 0: InterfaceGroupName of the registered node.
        /// Index 1: InterfaceGroupName of the witnessed node.</returns>
        public static string[] GetInterfaceGroupName(WITNESS_INTERFACE_LIST interfaceList)
        {
            // TODO:
            string[] interfaceGroupName = new string[2];

            foreach (var info in interfaceList.InterfaceInfo)
            {
                if ((info.Flags & (uint)SwnNodeFlagsValue.INTERFACE_WITNESS) != 0)
                {
                    // InterfaceGroupName of the registered node.
                    interfaceGroupName[0] = info.InterfaceGroupName;
                }
                if ((info.Flags & (uint)SwnNodeFlagsValue.INTERFACE_WITNESS) == 0)
                {
                    // InterfaceGroupName of the witnessed node.
                    interfaceGroupName[1] = info.InterfaceGroupName;
                }
            }

            return interfaceGroupName;
        }

        public static IPAddress ConvertIPV6(ushort[] ipv6)
        {
            byte[] buffer = new byte[16];
            Buffer.BlockCopy(ipv6, 0, buffer, 0, 16);
            return new IPAddress(buffer);
        }

        /// <summary>
        /// Check SWN version
        /// </summary>
        /// <param name="expectedVersion">The expected SWN Version.</param>
        /// <param name="actualVersion">The actual SWN Version.</param>
        public static void CheckVersion(SwnVersion expectedVersion, SwnVersion actualVersion)
        {
            if ((uint)actualVersion < (uint)expectedVersion)
            {
                // If server's actual SWN version is less than the expected version to run this test case, set it as inconclusive.
                BaseTestSite.Assert.Inconclusive("Test case is not applicable to servers that support SWN Version 0x{0:X08}.", (uint)actualVersion);
            }
        }
    }
}
