// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestManager.FileServerPlugin.Detector;
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

            var output = ExecutePowerShellCommand(@"..\etc\MS-SMBD\GetLocalNetworkAdapters.ps1", out error);


            bool result = true;

            if (output.Length != 0)
            {
                result = FilterNetworkInterfaces(output);
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

        private bool FilterNetworkInterfaces(object[] output)
        {
            var networkInterfaces = output.Select(item => ParseLocalNetworkInterfaceInformation(item));

            var nonRdmaNetworkInterfaces = networkInterfaces.Where(networkInterface => !networkInterface.RDMACapable);

            var rdmaNetworkInterfaces = networkInterfaces.Where(networkInterface => networkInterface.RDMACapable);

            int nonRdmaNetworkInterfaceCount = nonRdmaNetworkInterfaces.Count();
            if (nonRdmaNetworkInterfaceCount == 0)
            {
                DetectorUtil.WriteLog("Failed to detect any non-RDMA network interface!");
                return false;
            }
            else if (nonRdmaNetworkInterfaceCount == 1)
            {
                DetectionInfo.DriverNonRdmaNICIPAddress = nonRdmaNetworkInterfaces.First().IpAddress;
            }
            else
            {
                var selected = Application.Current.Dispatcher.Invoke(() =>
                {
                    var dialog = new NetworkInterfaceSelector(nonRdmaNetworkInterfaces.ToArray());
                    return dialog.ShowDialog("Please select the non-RDMA network interface");

                });
                if (selected != null)
                {
                    DetectionInfo.DriverNonRdmaNICIPAddress = selected.IpAddress;
                }
            }

            int rdmaNetworkInterfaceCount = rdmaNetworkInterfaces.Count();
            if (rdmaNetworkInterfaceCount == 0)
            {
                DetectorUtil.WriteLog("Failed to detect any RDMA network interface!");
                return false;
            }
            else if (rdmaNetworkInterfaceCount == 1)
            {
                DetectionInfo.DriverRdmaNICIPAddress = rdmaNetworkInterfaces.First().IpAddress;
            }
            else
            {
                var selected = Application.Current.Dispatcher.Invoke(() =>
                {
                    var dialog = new NetworkInterfaceSelector(rdmaNetworkInterfaces.ToArray());
                    return dialog.ShowDialog("Please select the RDMA network interface");

                });
                if (selected != null)
                {
                    DetectionInfo.DriverRdmaNICIPAddress = selected.IpAddress;
                }
            }

            if (DetectionInfo.DriverNonRdmaNICIPAddress == null || DetectionInfo.DriverRdmaNICIPAddress == null)
            {
                // if user do not select any network interface by closing dialog
                return false;
            }


            return true;

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