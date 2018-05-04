// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestManager.FileServerPlugin.Detector;
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

        public bool ConnectToShareNonRDMA()
        {

            DetectorUtil.WriteLog("Connect to share through non-RDMA transport...");

            bool result = ConnectToShare(DetectionInfo.SUTNonRdmaNICIPAddress, DetectionInfo.DriverNonRdmaNICIPAddress);


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

        public bool ConnectToShareRDMA()
        {

            DetectorUtil.WriteLog("Connect to share through RDMA transport...");

            bool result = ConnectToShare(DetectionInfo.SUTRdmaNICIPAddress, DetectionInfo.DriverRdmaNICIPAddress);


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



        public bool ConnectToShare(string serverIp, string clientIp)
        {
            try
            {
                using (var client = new SMB3Client())
                {
                    client.Connect(IPAddress.Parse(serverIp), IPAddress.Parse(clientIp));

                    client.Negotiate(DetectionInfo.SupportedSmbDialects);

                    client.SessionSetup(DetectionInfo.Authentication, DetectionInfo.DomainName, DetectionInfo.SUTName, DetectionInfo.UserName, DetectionInfo.Password);

                    string path = Smb2Utility.GetUncPath(DetectionInfo.SUTName, DetectionInfo.ShareFolder);

                    uint treeId;

                    client.TreeConnect(path, out treeId);

                    FILEID fileId;

                    client.CreateRandomFile(treeId, out fileId);

                    uint fileLength = 64;

                    var buffer = Smb2Utility.CreateRandomByteArray((int)fileLength);

                    client.Write(treeId, fileId, 0, buffer);

                    byte[] output;

                    client.Read(treeId, fileId, 0, fileLength, out output);

                    bool result = Enumerable.SequenceEqual(buffer, output);

                    if (!result)
                    {
                        DetectorUtil.WriteLog("The content of read and write is inconsistent.");
                    }

                    return result;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}