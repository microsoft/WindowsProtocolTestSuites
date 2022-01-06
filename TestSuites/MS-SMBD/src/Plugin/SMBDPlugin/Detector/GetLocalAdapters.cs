// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Detector;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Protocols.TestManager.SMBDPlugin.Detector
{
    partial class SmbdDetector
    {
        public bool GetLocalAdapters()
        {
            logWriter.AddLog(DetectLogLevel.Information, "Check the local adapters...");

            string path = Assembly.GetExecutingAssembly().Location + "/../../Plugin/script/GetLocalNetworkAdapters.ps1";
            LocalNetworkInterfaceInformation[] output = ExecutePowerShellCommand<LocalNetworkInterfaceInformation>(path, out string[] error);

            bool result = true;
            if (output.Length != 0)
            {
                FilterNetworkInterfaces(output);
                result = true;
            }
            else
            {
                logWriter.AddLog(DetectLogLevel.Information, "Failed to detect any network interface!");
                result = false;
            }

            if (result)
            {
                logWriter.AddLog(DetectLogLevel.Warning, "Finished", false, LogStyle.StepPassed);
                return true;
            }
            else
            {
                if (error != null)
                {
                    foreach (var item in error)
                    {
                        logWriter.AddLog(DetectLogLevel.Information, item.ToString());
                    }
                }
                logWriter.AddLog(DetectLogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                logWriter.AddLineToLog(DetectLogLevel.Information);
                return false;
            }
        }

        public List<string> GetLocalInterfaceIps(bool isNon)
        {
            string path = Assembly.GetExecutingAssembly().Location + "/../../Plugin/script/GetLocalNetworkAdapters.ps1";
            LocalNetworkInterfaceInformation[] output = ExecutePowerShellCommand<LocalNetworkInterfaceInformation>(path, out string[] error);

            if (output.Length != 0)
            {
                var networkInterfaces = output;
                if (isNon)
                {
                    var nonRdmaNetworkInterfaceIps = networkInterfaces.Where(networkInterface => !networkInterface.RDMACapable).Select(result => result.IpAddress);
                    return nonRdmaNetworkInterfaceIps.ToList();
                }
                else
                {
                    var rdmaNetworkInterfaceIps = networkInterfaces.Where(networkInterface => networkInterface.RDMACapable).Select(result => result.IpAddress);
                    return rdmaNetworkInterfaceIps.ToList();
                }
            }
            else
            {
                if (error != null)
                {
                    foreach (var item in error)
                    {
                        logWriter.AddLog(DetectLogLevel.Information, item.ToString());
                    }
                }
                logWriter.AddLog(DetectLogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                logWriter.AddLog(DetectLogLevel.Information, "Failed to detect any network interface!");
            }
            return new List<string>();
        }

        private void FilterNetworkInterfaces(LocalNetworkInterfaceInformation[] output)
        {
            var networkInterfaces = output;
            var nonRdmaNetworkInterfaces = networkInterfaces.Where(networkInterface => !networkInterface.RDMACapable);
            var rdmaNetworkInterfaces = networkInterfaces.Where(networkInterface => networkInterface.RDMACapable);

            int nonRdmaNetworkInterfaceCount = nonRdmaNetworkInterfaces.Count();
            if (nonRdmaNetworkInterfaceCount == 0)
            {
                logWriter.AddLog(DetectLogLevel.Information, "Failed to detect any non-RDMA network interface of driver computer!");
            }
            else if (nonRdmaNetworkInterfaceCount == 1)
            {
                if (nonRdmaNetworkInterfaces.First().IpAddress != DetectionInfo.DriverNonRdmaNICIPAddress)
                {
                    logWriter.AddLog(DetectLogLevel.Information, string.Format("Can't Choose {0} as non-RDMA IP address of driver computer.Please Choose {1} as non-RDMA IP address of driver computer.", DetectionInfo.DriverNonRdmaNICIPAddress, nonRdmaNetworkInterfaces.First().IpAddress));
                }
                else
                {
                    logWriter.AddLog(DetectLogLevel.Information, string.Format("Choose {0} as non-RDMA IP address of driver computer.", DetectionInfo.DriverNonRdmaNICIPAddress));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(DetectionInfo.DriverNonRdmaNICIPAddress) && nonRdmaNetworkInterfaces.Select(x => x.IpAddress).Contains(DetectionInfo.DriverNonRdmaNICIPAddress))
                {
                    logWriter.AddLog(DetectLogLevel.Information, string.Format("User choose {0} as non-RDMA IP address of driver computer.", DetectionInfo.DriverNonRdmaNICIPAddress));
                }
                else
                {
                    logWriter.AddLog(DetectLogLevel.Information, "User skipped choosing non-RDMA network interface of driver computer.");
                }
            }

            int rdmaNetworkInterfaceCount = rdmaNetworkInterfaces.Count();
            if (rdmaNetworkInterfaceCount == 0)
            {
                logWriter.AddLog(DetectLogLevel.Information, "Failed to detect any RDMA network interface of driver computer!");
            }
            else if (rdmaNetworkInterfaceCount == 1)
            {
                if (rdmaNetworkInterfaces.First().IpAddress != DetectionInfo.DriverRdmaNICIPAddress)
                {
                    logWriter.AddLog(DetectLogLevel.Information, string.Format("Can't Choose {0} as RDMA IP address of driver computer.Please Choose {1} as RDMA IP address of driver computer.", DetectionInfo.DriverRdmaNICIPAddress, rdmaNetworkInterfaces.First().IpAddress));
                }
                else
                {
                    logWriter.AddLog(DetectLogLevel.Information, string.Format("Choose {0} as RDMA IP address of driver computer.", DetectionInfo.DriverRdmaNICIPAddress));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(DetectionInfo.DriverRdmaNICIPAddress) && rdmaNetworkInterfaces.Select(x => x.IpAddress).Contains(DetectionInfo.DriverRdmaNICIPAddress))
                {
                    logWriter.AddLog(DetectLogLevel.Information, string.Format("User choose {0} as RDMA IP address of driver computer.", DetectionInfo.DriverRdmaNICIPAddress));
                }
                else
                {
                    logWriter.AddLog(DetectLogLevel.Information, "User skipped choosing RDMA network interface of driver computer.");
                }
            }
        }
    }
}