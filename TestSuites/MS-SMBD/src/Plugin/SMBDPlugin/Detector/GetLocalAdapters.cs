// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestManager.SMBDPlugin.Detector;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Microsoft.Protocols.TestManager.SMBDPlugin.Detector
{
    partial class SMBDDetector
    {

        public bool GetLocalAdapters()
        {
            DetectorUtil.WriteLog("Check the local adapters...");

            string[] error;

            string path = Assembly.GetExecutingAssembly().Location + "/../../Plugin/script/GetLocalNetworkAdapters.ps1";
            var output = ExecutePowerShellCommand(path, out error);


            bool result = true;

            if (output.Length != 0)
            {
                FilterNetworkInterfaces(output);
                result = true;
            }
            else
            {
                DetectorUtil.WriteLog("Failed to detect any network interface!");
                result = false;
            }



            if (result)
            {
                DetectorUtil.WriteLog("Finished", false, LogStyle.StepPassed);
                return true;
            }
            else
            {
                if (error != null)
                {
                    foreach (var item in error)
                    {
                        DetectorUtil.WriteLog(item.ToString());
                    }
                }
                DetectorUtil.WriteLog("Failed", false, LogStyle.StepFailed);
                return false;
            }
        }
        public List<string> GetLocalInterfaceIps(bool isNon)
        {
            string[] error;

            string path = Assembly.GetExecutingAssembly().Location+ "/../../Plugin/script/GetLocalNetworkAdapters.ps1";
            var output = ExecutePowerShellCommand(path, out error);
            if (error != null)
            {
                foreach (var item in error)
                {
                    DetectorUtil.WriteLog(item.ToString());
                }
            }
            DetectorUtil.WriteLog("Failed", false, LogStyle.StepFailed);
            if (output.Length != 0)
            {
                var networkInterfaces = output
                                        .Select(item => ParseLocalNetworkInterfaceInformation(item))
                                        .Where(item => item != null);
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
            return new List<string>();
        }

        private void FilterNetworkInterfaces(object[] output)
        {

            var networkInterfaces = output
                                        .Select(item => ParseLocalNetworkInterfaceInformation(item))
                                        .Where(item => item != null);

            var nonRdmaNetworkInterfaces = networkInterfaces.Where(networkInterface => !networkInterface.RDMACapable);

            var rdmaNetworkInterfaces = networkInterfaces.Where(networkInterface => networkInterface.RDMACapable);

            int nonRdmaNetworkInterfaceCount = nonRdmaNetworkInterfaces.Count();
            if (nonRdmaNetworkInterfaceCount == 0)
            {
                DetectorUtil.WriteLog("Failed to detect any non-RDMA network interface of driver computer!");
            }
            else if (nonRdmaNetworkInterfaceCount == 1)
            {
                if (nonRdmaNetworkInterfaces.First().IpAddress != DetectionInfo.DriverNonRdmaNICIPAddress)
                {
                    DetectorUtil.WriteLog(string.Format("Can't Choose {0} as non-RDMA IP address of driver computer.Please use the default non-RDMA IP address", DetectionInfo.DriverNonRdmaNICIPAddress));
                }
                else
                {
                    DetectorUtil.WriteLog(string.Format("Choose {0} as non-RDMA IP address of driver computer.", DetectionInfo.DriverNonRdmaNICIPAddress));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(DetectionInfo.DriverNonRdmaNICIPAddress) && nonRdmaNetworkInterfaces.Select(x => x.IpAddress).Contains(DetectionInfo.DriverNonRdmaNICIPAddress))
                {
                    DetectorUtil.WriteLog(string.Format("User choose {0} as non-RDMA IP address of driver computer.", DetectionInfo.DriverNonRdmaNICIPAddress));
                }
                else
                {
                    DetectorUtil.WriteLog("User skipped choosing non-RDMA network interface of driver computer.");
                }
            }

            int rdmaNetworkInterfaceCount = rdmaNetworkInterfaces.Count();
            if (rdmaNetworkInterfaceCount == 0)
            {
                DetectorUtil.WriteLog("Failed to detect any RDMA network interface of driver computer!");
            }
            else if (rdmaNetworkInterfaceCount == 1)
            {
                if (rdmaNetworkInterfaces.First().IpAddress != DetectionInfo.DriverRdmaNICIPAddress)
                {
                    DetectorUtil.WriteLog(string.Format("Can't Choose {0} as RDMA IP address of driver computer.Please use the default RDMA IP address", DetectionInfo.DriverRdmaNICIPAddress));
                }
                else
                {
                    DetectorUtil.WriteLog(string.Format("Choose {0} as RDMA IP address of driver computer.", DetectionInfo.DriverRdmaNICIPAddress));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(DetectionInfo.DriverRdmaNICIPAddress) && rdmaNetworkInterfaces.Select(x => x.IpAddress).Contains(DetectionInfo.DriverRdmaNICIPAddress))
                {
                    DetectorUtil.WriteLog(string.Format("User choose {0} as RDMA IP address of driver computer.", DetectionInfo.DriverRdmaNICIPAddress));
                }
                else
                {
                    DetectorUtil.WriteLog("User skipped choosing RDMA network interface of driver computer.");
                }
            }
        }


        private LocalNetworkInterfaceInformation ParseLocalNetworkInterfaceInformation(dynamic inputObject)
        {
            try
            {
                return new LocalNetworkInterfaceInformation
                {
                    Name = inputObject.Name,
                    IpAddress = inputObject.IpAddress,
                    Description = inputObject.Description,
                    RDMACapable = inputObject.RDMACapable
                };
            }
            catch
            {
                return null;
            }
        }
    }
}