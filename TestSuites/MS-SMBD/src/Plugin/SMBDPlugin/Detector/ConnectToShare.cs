// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Detector;
#if WINDOWS
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rdma;
#endif
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using System;
using System.Linq;
using System.Net;

namespace Microsoft.Protocols.TestManager.SMBDPlugin.Detector
{
    partial class SmbdDetector
    {
        public bool ConnectToShareNonRDMA()
        {
            logWriter.AddLog(DetectLogLevel.Information, "Connect to share through non-RDMA transport...");
            if (DetectionInfo.SUTNonRdmaNICIPAddress == null || DetectionInfo.DriverNonRdmaNICIPAddress == null)
            {
                logWriter.AddLog(DetectLogLevel.Warning, "Failed", true, LogStyle.StepSkipped);
                logWriter.AddLog(DetectLogLevel.Information, "Connect to share through non-RDMA transport skipped since not available.");
                return false;
            }

            bool result = ConnectToShare(DetectionInfo.SUTNonRdmaNICIPAddress, DetectionInfo.DriverNonRdmaNICIPAddress);
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

        public bool ConnectToShareRDMA()
        {
            logWriter.AddLog(DetectLogLevel.Information, "Connect to share through RDMA transport...");
            if (DetectionInfo.SUTRdmaNICIPAddress == null || DetectionInfo.DriverRdmaNICIPAddress == null)
            {
                logWriter.AddLog(DetectLogLevel.Warning, "Failed", true, LogStyle.StepSkipped);
                logWriter.AddLog(DetectLogLevel.Information, "Connect to share through RDMA transport skipped since not available.");
                return false;
            }

            bool result = ConnectToShare(DetectionInfo.SUTRdmaNICIPAddress, DetectionInfo.DriverRdmaNICIPAddress);
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

        public bool ConnectToShare(string serverIp, string clientIp)
        {
            try
            {
                using (var client = new SmbdClient(DetectionInfo.ConnectionTimeout))
                {
                    // Determine if this is an RDMA connection based on IP addresses
                    bool isRdmaConnection = IsRdmaAddress(serverIp) && IsRdmaAddress(clientIp);
                    logWriter.AddLog(DetectLogLevel.Information, string.Format("The isRdmaConnection is {0}", isRdmaConnection));
                    if (isRdmaConnection)
                    {
                        // Connect using RDMA transport
                        RdmaAdapterData adapterInfo;
                        client.ConnectOverRDMA(clientIp, serverIp, 445, 8192, out adapterInfo);
                        logWriter.AddLog(DetectLogLevel.Information, "RDMA connection successfully.");
                    }
                    else
                    {
                        // Connect using TCP transport
                        client.Connect(IPAddress.Parse(serverIp), IPAddress.Parse(clientIp));
                    }

                    client.Smb2Negotiate(DetectionInfo.SupportedSmbDialects);
                    logWriter.AddLog(DetectLogLevel.Information, "SMB2 Negotiate successfully.");
                    client.Smb2SessionSetup(DetectionInfo.Authentication, DetectionInfo.DomainName, DetectionInfo.SUTName, DetectionInfo.UserName, DetectionInfo.Password);
                    logWriter.AddLog(DetectLogLevel.Information, "SMB2 Session setup successfully.");
                    string path = Smb2Utility.GetUncPath(DetectionInfo.SUTName, DetectionInfo.ShareFolder);
                    client.Smb2TreeConnect(path, out uint treeId);
                    logWriter.AddLog(DetectLogLevel.Information, "SMB2 Tree connection successfully.");
                    client.CreateRandomFile(treeId, out FILEID fileId);

                    uint fileLength = isRdmaConnection ? client.CalculateSMBDMaxReadWriteSize() : client.CalculateSmb2MaxReadWriteSize();
                    var buffer = Smb2Utility.CreateRandomByteArray((int)fileLength);
                    client.Smb2Write(treeId, fileId, 0, buffer);
                    logWriter.AddLog(DetectLogLevel.Information, "SMB2 Write successfully.");
                    client.Smb2Read(treeId, fileId, 0, fileLength, out byte[] output);
                    logWriter.AddLog(DetectLogLevel.Information, "SMB2 Read successfully.");
                    bool result = Enumerable.SequenceEqual(buffer, output);
                    if (!result)
                    {
                        logWriter.AddLog(DetectLogLevel.Information, "The content of read and write is inconsistent.");
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                logWriter.AddLog(DetectLogLevel.Information, string.Format("ConnectToShare threw exception: {0}", ex));
                return false;
            }
        }

        private bool IsRdmaAddress(string ipAddress)
        {
            // Check if the IP address matches the RDMA NIC IP addresses
            return DetectionInfo.SUTRdmaNICIPAddress == ipAddress || DetectionInfo.DriverRdmaNICIPAddress == ipAddress;
        }
    }
}