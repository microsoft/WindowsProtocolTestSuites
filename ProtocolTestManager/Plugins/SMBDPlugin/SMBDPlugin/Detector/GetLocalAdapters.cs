// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestManager.SMBDPlugin.Detector;
using System.Linq;
using System.Windows;

namespace Microsoft.Protocols.TestManager.SMBDPlugin.Detector
{
    partial class SMBDDetector
    {

        public bool GetLocalAdapters()
        {
            DetectorUtil.WriteLog("Check the local adapters...");

            string[] error;

            var output = ExecutePowerShellCommand(@"..\etc\MS-SMBD\Scripts\GetLocalNetworkAdapters.ps1", out error);


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
                DetectionInfo.DriverNonRdmaNICIPAddress = nonRdmaNetworkInterfaces.First().IpAddress;
                DetectorUtil.WriteLog(string.Format("Choose {0} as non-RDMA IP address of driver computer.", DetectionInfo.DriverNonRdmaNICIPAddress));
            }
            else
            {
                var selected = Application.Current.Dispatcher.Invoke(() =>
                {
                    var dialog = new LocalNetworkInterfaceSelector(nonRdmaNetworkInterfaces.ToArray());
                    return dialog.ShowDialog("Please select the non-RDMA network interface of driver computer");

                });
                if (selected != null)
                {
                    DetectionInfo.DriverNonRdmaNICIPAddress = selected.IpAddress;
                    DetectorUtil.WriteLog(string.Format("User chose {0} as non-RDMA IP address of driver computer.", DetectionInfo.DriverNonRdmaNICIPAddress));
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
                DetectionInfo.DriverRdmaNICIPAddress = rdmaNetworkInterfaces.First().IpAddress;
                DetectorUtil.WriteLog(string.Format("Choose {0} as RDMA IP address of driver computer.", DetectionInfo.DriverRdmaNICIPAddress));
            }
            else
            {
                var selected = Application.Current.Dispatcher.Invoke(() =>
                {
                    var dialog = new LocalNetworkInterfaceSelector(rdmaNetworkInterfaces.ToArray());
                    return dialog.ShowDialog("Please select the RDMA network interface of driver computer");
                });
                if (selected != null)
                {
                    DetectionInfo.DriverRdmaNICIPAddress = selected.IpAddress;
                    DetectorUtil.WriteLog(string.Format("User chose {0} as RDMA IP address of driver computer.", DetectionInfo.DriverRdmaNICIPAddress));
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