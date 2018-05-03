// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Microsoft.Protocols.TestManager.SMBDPlugin.Detector
{
    partial class SMBDDetector
    {

        public bool GetRemoteAdapters()
        {
            DetectorUtil.WriteLog("Get the remote adapters...");

            bool result = false;

            var ipList = Dns.GetHostAddresses(DetectionInfo.SUTName).Where(ipAddress => ipAddress.AddressFamily == AddressFamily.InterNetwork);


            foreach (var ip in ipList)
            {
                result = GetRemoteNetworkInterfaceInformation(ip);
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

        private IPAddress[] GetIPAdressOfSut()
        {
            try
            {
                return Dns.GetHostAddresses(DetectionInfo.SUTName);
            }
            catch (Exception ex)
            {
                DetectorUtil.WriteLog(String.Format("Cannot get SUT IP addresses: {0}.", ex));
                return new IPAddress[0];
            }
        }

        private bool GetRemoteNetworkInterfaceInformation(IPAddress ip)
        {
            try
            {
                using (var client = new Smb3Client())
                {

                    client.Connect(ip);

                    client.Negotiate(new DialectRevision[] { DialectRevision.Smb30, DialectRevision.Smb302, DialectRevision.Smb311 });

                    client.SessionSetup(DetectionInfo.Authentication, DetectionInfo.DomainName, DetectionInfo.SUTName, DetectionInfo.UserName, DetectionInfo.Password);

                    uint treeId;

                    string ipcPath = Smb2Utility.GetIPCPath(DetectionInfo.SUTName);

                    client.TreeConnect(ipcPath, out treeId);

                    return true;
                }
            }
            catch (Exception ex)
            {
                DetectorUtil.WriteLog(String.Format("GetRemoteNetworkInterfaceInformation failed for {0}: {1}.", ip.ToString(), ex.ToString()));
                return false;
            }
        }

    }
}