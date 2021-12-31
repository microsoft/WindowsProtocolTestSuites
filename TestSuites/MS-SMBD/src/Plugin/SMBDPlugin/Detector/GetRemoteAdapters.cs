// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using System;
using System.Linq;
using System.Net;

namespace Microsoft.Protocols.TestManager.SMBDPlugin.Detector
{
    partial class SmbdDetector
    {
        public bool GetRemoteAdapters()
        {
            logWriter.AddLog(DetectLogLevel.Information, "Get the remote adapters...");

            bool result = false;
            var ipList = GetIpAddressListOfSut();
            // try all reachable SUT IP address
            foreach (var ip in ipList)
            {
                result = GetRemoteNetworkInterfaceInformation(ip);
                if (result)
                {
                    break;
                }
            }

            if (result)
            {
                logWriter.AddLog(DetectLogLevel.Warning, "Finished", false, LogStyle.StepPassed);
                return true;
            }
            else
            {
                logWriter.AddLog(DetectLogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                logWriter.AddLineToLog(DetectLogLevel.Information);
                return false;
            }
        }

        private bool GetRemoteNetworkInterfaceInformation(IPAddress ip)
        {
            try
            {
                using (var client = new SmbdClient(DetectionInfo.ConnectionTimeout))
                {
                    client.Connect(ip, IPAddress.Parse(DetectionInfo.DriverNonRdmaNICIPAddress));
                    client.Smb2Negotiate(new DialectRevision[] { DialectRevision.Smb30, DialectRevision.Smb302, DialectRevision.Smb311 });
                    client.Smb2SessionSetup(DetectionInfo.Authentication, DetectionInfo.DomainName, DetectionInfo.SUTName, DetectionInfo.UserName, DetectionInfo.Password);

                    string ipcPath = Smb2Utility.GetIPCPath(DetectionInfo.SUTName);
                    client.Smb2TreeConnect(ipcPath, out uint treeId);

                    client.IoCtl(treeId, CtlCode_Values.FSCTL_QUERY_NETWORK_INTERFACE_INFO, FILEID.Invalid, IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL, out byte[] input, out byte[] output);

                    var networkInterfaces = Smb2Utility.UnmarshalNetworkInterfaceInfoResponse(output);
                    var remoteInterfaces = ParseRemoteNetworkInterfaceInformation(networkInterfaces);
                    FilterNetworkInterfaces(remoteInterfaces);

                    return true;
                }
            }
            catch (Exception ex)
            {
                logWriter.AddLog(DetectLogLevel.Information, string.Format("GetRemoteNetworkInterfaceInformation failed for {0}: {1}.", ip.ToString(), ex.ToString()));
                return false;
            }
        }

        private void FilterNetworkInterfaces(RemoteNetworkInterfaceInformation[] networkInterfaces)
        {
            if (DetectionInfo.DriverNonRdmaNICIPAddress == null)
            {
                logWriter.AddLog(DetectLogLevel.Information, "Skip detecting any non-RDMA network interface of SUT since no corresponding selected for driver computer!");
                DetectionInfo.SUTNonRdmaNICIPAddress = null;
            }
            else
            {
                var nonRdmaNetworkInterfaces = networkInterfaces.Where(networkInterface => !networkInterface.RDMACapable);
                int nonRdmaNetworkInterfaceCount = nonRdmaNetworkInterfaces.Count();
                if (nonRdmaNetworkInterfaceCount == 0)
                {
                    logWriter.AddLog(DetectLogLevel.Information, "Failed to detect any non-RDMA network interface of SUT!");
                }
                else if (nonRdmaNetworkInterfaceCount == 1)
                {
                    DetectionInfo.SUTNonRdmaNICIPAddress = nonRdmaNetworkInterfaces.First().IpAddress;
                    logWriter.AddLog(DetectLogLevel.Information, string.Format("Choose {0} as non-RDMA IP address of SUT.", DetectionInfo.SUTNonRdmaNICIPAddress));
                }
                else
                {
                    foreach (var nonRdmaInterface in nonRdmaNetworkInterfaces)
                    {
                        try
                        {
                            using (var client = new SmbdClient(DetectionInfo.ConnectionTimeout))
                            {
                                client.Connect(IPAddress.Parse(nonRdmaInterface.IpAddress), IPAddress.Parse(DetectionInfo.DriverNonRdmaNICIPAddress));
                                DetectionInfo.SUTNonRdmaNICIPAddress = nonRdmaInterface.IpAddress;
                                logWriter.AddLog(DetectLogLevel.Information, string.Format("Choose {0} as non-RDMA IP address of SUT.", DetectionInfo.SUTNonRdmaNICIPAddress));
                                break;
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }

            if (DetectionInfo.DriverRdmaNICIPAddress == null)
            {
                logWriter.AddLog(DetectLogLevel.Information, "Skip detecting any RDMA network interface of SUT since no corresponding selected for driver computer!");
                DetectionInfo.SUTRdmaNICIPAddress = null;
            }
            else
            {
                var rdmaNetworkInterfaces = networkInterfaces.Where(networkInterface => networkInterface.RDMACapable);
                int rdmaNetworkInterfaceCount = rdmaNetworkInterfaces.Count();
                if (rdmaNetworkInterfaceCount == 0)
                {
                    logWriter.AddLog(DetectLogLevel.Information, "Failed to detect any RDMA network interface of SUT!");
                }
                else if (rdmaNetworkInterfaceCount == 1)
                {
                    DetectionInfo.SUTRdmaNICIPAddress = rdmaNetworkInterfaces.First().IpAddress;
                    logWriter.AddLog(DetectLogLevel.Information, string.Format("Choose {0} as RDMA IP address of SUT.", DetectionInfo.SUTRdmaNICIPAddress));
                }
                else
                {
                    foreach (var rdmaInterface in rdmaNetworkInterfaces)
                    {
                        try
                        {
                            using (var client = new SmbdClient(DetectionInfo.ConnectionTimeout))
                            {
                                client.Connect(IPAddress.Parse(rdmaInterface.IpAddress), IPAddress.Parse(DetectionInfo.DriverRdmaNICIPAddress));
                                DetectionInfo.SUTRdmaNICIPAddress = rdmaInterface.IpAddress;
                                logWriter.AddLog(DetectLogLevel.Information, string.Format("Choose {0} as RDMA IP address of SUT.", DetectionInfo.SUTRdmaNICIPAddress));
                                break;
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }

        private RemoteNetworkInterfaceInformation[] ParseRemoteNetworkInterfaceInformation(NETWORK_INTERFACE_INFO_Response[] networkInterfaceInfo)
        {
            var result = networkInterfaceInfo
                            .Where(info => info.AddressStorage.Family == 2)
                            .Select(info => new RemoteNetworkInterfaceInformation
                            {
                                IfIndex = info.IfIndex,
                                IpAddress = info.AddressStorage.Address,
                                LinkSpeed = ParseLinkSpeed(info.LinkSpeed),
                                RDMACapable = info.Capability.HasFlag(NETWORK_INTERFACE_INFO_Response_Capabilities.RDMA_CAPABLE)
                            });

            return result.ToArray();
        }

        private string ParseLinkSpeed(ulong linkSpeed)
        {
            string[] postfix = { "bps", "Kbps", "Mbps", "Gbps" };
            int level = 0;

            while (linkSpeed > 1000 && level + 1 < postfix.Length)
            {
                linkSpeed /= 1000;
                level++;
            }

            string result = String.Format("{0}{1}", linkSpeed, postfix[level]);

            return result;
        }
    }
}