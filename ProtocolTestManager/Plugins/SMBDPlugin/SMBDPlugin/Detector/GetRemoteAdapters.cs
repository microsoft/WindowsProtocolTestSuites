// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestManager.SMBDPlugin.Detector;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows;

namespace Microsoft.Protocols.TestManager.SMBDPlugin.Detector
{
    partial class SMBDDetector
    {

        public bool GetRemoteAdapters()
        {
            DetectorUtil.WriteLog("Get the remote adapters...");

            bool result = false;

            var ipList = GetIPAdressOfSut();

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
                DetectorUtil.WriteLog("Finished", false, LogStyle.StepPassed);
                return true;
            }
            else
            {
                DetectorUtil.WriteLog("Failed", false, LogStyle.StepFailed);
                return false;
            }
        }

        private bool GetRemoteNetworkInterfaceInformation(IPAddress ip)
        {
            try
            {
                using (var client = new SMBDClient(DetectionInfo.ConnectionTimeout))
                {

                    client.Connect(ip, IPAddress.Parse(DetectionInfo.DriverNonRdmaNICIPAddress));

                    client.Smb2Negotiate(new DialectRevision[] { DialectRevision.Smb30, DialectRevision.Smb302, DialectRevision.Smb311 });

                    client.Smb2SessionSetup(DetectionInfo.Authentication, DetectionInfo.DomainName, DetectionInfo.SUTName, DetectionInfo.UserName, DetectionInfo.Password);

                    uint treeId;

                    string ipcPath = Smb2Utility.GetIPCPath(DetectionInfo.SUTName);

                    client.Smb2TreeConnect(ipcPath, out treeId);

                    byte[] input;
                    byte[] output;

                    client.IoCtl(treeId, CtlCode_Values.FSCTL_QUERY_NETWORK_INTERFACE_INFO, FILEID.Invalid, IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL, out input, out output);

                    var networkInterfaces = Smb2Utility.UnmarshalNetworkInterfaceInfoResponse(output);

                    var remoteInterfaces = ParseRemoteNetworkInterfaceInformation(networkInterfaces);

                    FilterNetworkInterfaces(remoteInterfaces);

                    return true;
                }
            }
            catch (Exception ex)
            {
                DetectorUtil.WriteLog(String.Format("GetRemoteNetworkInterfaceInformation failed for {0}: {1}.", ip.ToString(), ex.ToString()));
                return false;
            }
        }

        private void FilterNetworkInterfaces(RemoteNetworkInterfaceInformation[] networkInterfaces)
        {
            if (DetectionInfo.DriverNonRdmaNICIPAddress == null)
            {
                DetectorUtil.WriteLog("Skip detecting any non-RDMA network interface of SUT since no corresponding selected for driver computer!");
                DetectionInfo.SUTNonRdmaNICIPAddress = null;
            }
            else
            {
                var nonRdmaNetworkInterfaces = networkInterfaces.Where(networkInterface => !networkInterface.RDMACapable);

                int nonRdmaNetworkInterfaceCount = nonRdmaNetworkInterfaces.Count();
                if (nonRdmaNetworkInterfaceCount == 0)
                {
                    DetectorUtil.WriteLog("Failed to detect any non-RDMA network interface of SUT!");
                }
                else if (nonRdmaNetworkInterfaceCount == 1)
                {
                    DetectionInfo.SUTNonRdmaNICIPAddress = nonRdmaNetworkInterfaces.First().IpAddress;
                    DetectorUtil.WriteLog(string.Format("Choose {0} as non-RDMA IP address.", DetectionInfo.SUTNonRdmaNICIPAddress));
                }
                else
                {
                    var selected = Application.Current.Dispatcher.Invoke(() =>
                    {
                        var dialog = new RemoteNetworkInterfaceSelector(nonRdmaNetworkInterfaces.ToArray());
                        string prompt = String.Format("The IP address selected as non-RDMA network interface of driver computer is {0}", DetectionInfo.DriverNonRdmaNICIPAddress);
                        return dialog.ShowDialog("Please select the non-RDMA network interface of SUT", prompt);
                    });
                    if (selected != null)
                    {
                        DetectionInfo.SUTNonRdmaNICIPAddress = selected.IpAddress;
                        DetectorUtil.WriteLog(string.Format("User chose {0} as non-RDMA IP address of SUT.", DetectionInfo.SUTNonRdmaNICIPAddress));
                    }
                    else
                    {
                        DetectorUtil.WriteLog("User skipped choosing non-RDMA network interface of SUT.");
                    }
                }
            }

            if (DetectionInfo.DriverRdmaNICIPAddress == null)
            {
                DetectorUtil.WriteLog("Skip detecting any RDMA network interface of SUT since no corresponding selected for driver computer!");
                DetectionInfo.SUTRdmaNICIPAddress = null;
            }
            else
            {
                var rdmaNetworkInterfaces = networkInterfaces.Where(networkInterface => networkInterface.RDMACapable);

                int rdmaNetworkInterfaceCount = rdmaNetworkInterfaces.Count();
                if (rdmaNetworkInterfaceCount == 0)
                {
                    DetectorUtil.WriteLog("Failed to detect any RDMA network interface of SUT!");
                }
                else if (rdmaNetworkInterfaceCount == 1)
                {
                    DetectionInfo.SUTRdmaNICIPAddress = rdmaNetworkInterfaces.First().IpAddress;
                    DetectorUtil.WriteLog(string.Format("Choose {0} as RDMA IP address of SUT.", DetectionInfo.SUTRdmaNICIPAddress));
                }
                else
                {
                    var selected = Application.Current.Dispatcher.Invoke(() =>
                    {
                        var dialog = new RemoteNetworkInterfaceSelector(rdmaNetworkInterfaces.ToArray());
                        string prompt = String.Format("The IP address selected as RDMA network interface of driver computer is {0}", DetectionInfo.DriverRdmaNICIPAddress);
                        return dialog.ShowDialog("Please select the RDMA network interface of SUT", prompt);
                    });
                    if (selected != null)
                    {
                        DetectionInfo.SUTRdmaNICIPAddress = selected.IpAddress;
                        DetectorUtil.WriteLog(string.Format("User chose {0} as RDMA IP address of SUT.", DetectionInfo.SUTRdmaNICIPAddress));
                    }
                    else
                    {
                        DetectorUtil.WriteLog("User skipped choosing RDMA network interface of SUT.");
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