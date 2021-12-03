// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using System;
using System.Linq;
using System.Net;

namespace Microsoft.Protocols.TestManager.SMBDPlugin.Detector
{
    partial class SMBDDetector
    {

        public bool ConnectToShareNonRDMA()
        {
            if (DetectionInfo.SUTNonRdmaNICIPAddress == null || DetectionInfo.DriverNonRdmaNICIPAddress == null)
            {
                DetectorUtil.WriteLog("Connect to share through non-RDMA transport skipped since not available.", true, LogStyle.StepSkipped);
                return false;
            }

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
            if (DetectionInfo.SUTRdmaNICIPAddress == null || DetectionInfo.DriverRdmaNICIPAddress == null)
            {
                DetectorUtil.WriteLog("Connect to share through RDMA transport skipped since not available.", true, LogStyle.StepSkipped);
                return false;
            }

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
                using (var client = new SMBDClient(DetectionInfo.ConnectionTimeout))
                {
                    client.Connect(IPAddress.Parse(serverIp), IPAddress.Parse(clientIp));

                    client.Smb2Negotiate(DetectionInfo.SupportedSmbDialects);

                    client.Smb2SessionSetup(DetectionInfo.Authentication, DetectionInfo.DomainName, DetectionInfo.SUTName, DetectionInfo.UserName, DetectionInfo.Password);

                    string path = Smb2Utility.GetUncPath(DetectionInfo.SUTName, DetectionInfo.ShareFolder);

                    uint treeId;

                    client.Smb2TreeConnect(path, out treeId);

                    FILEID fileId;

                    client.CreateRandomFile(treeId, out fileId);

                    uint fileLength = client.CalculateSmb2MaxReadWriteSize();

                    var buffer = Smb2Utility.CreateRandomByteArray((int)fileLength);

                    client.Smb2Write(treeId, fileId, 0, buffer);

                    byte[] output;

                    client.Smb2Read(treeId, fileId, 0, fileLength, out output);

                    bool result = Enumerable.SequenceEqual(buffer, output);

                    if (!result)
                    {
                        DetectorUtil.WriteLog("The content of read and write is inconsistent.");
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                DetectorUtil.WriteLog(String.Format("ConnectToShare threw exception: {0}", ex));
                return false;
            }
        }
    }
}