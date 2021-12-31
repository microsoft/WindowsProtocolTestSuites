// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rdma;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using System;
using System.Linq;

namespace Microsoft.Protocols.TestManager.SMBDPlugin.Detector
{
    partial class SmbdDetector
    {
        public bool CheckSMBDCapability(out RdmaAdapterInfo rdmaAdapterInfo, out bool rdmaChannelV1Supported, out bool rdmaChannelV1InvalidateSupported)
        {
            rdmaAdapterInfo = null;
            rdmaChannelV1Supported = false;
            rdmaChannelV1InvalidateSupported = false;

            logWriter.AddLog(DetectLogLevel.Information, "Check the supported SMBD capabilities of SUT...");
            if (DetectionInfo.SUTRdmaNICIPAddress == null || DetectionInfo.DriverRdmaNICIPAddress == null)
            {
                logWriter.AddLog(DetectLogLevel.Warning, "Failed", true, LogStyle.StepSkipped);
                logWriter.AddLog(DetectLogLevel.Information, "Check the supported SMBD capabilities of SUT skipped since not available.");
                return false;
            }

            bool result = CheckSMBDNegotiate(out rdmaAdapterInfo);
            if (result)
            {
                if (CheckSMBDReadWriteRDMAV1())
                {
                    result = true;
                    rdmaChannelV1Supported = true;
                    if (CheckSMBDReadWriteRDMAV1Invalidate())
                    {
                        rdmaChannelV1InvalidateSupported = true;
                    }
                }
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

        private bool CheckSMBDNegotiate(out RdmaAdapterInfo rdmaAdapterInfo)
        {
            try
            {
                using (var client = new SmbdClient(DetectionInfo.ConnectionTimeout))
                {
                    var config = DetectionInfo.SMBDClientCapability;
                    client.ConnectOverRDMA(DetectionInfo.DriverRdmaNICIPAddress, DetectionInfo.SUTRdmaNICIPAddress, DetectionInfo.SMBDPort, config.MaxReceiveSize, out rdmaAdapterInfo);
                    client.SMBDNegotiate(
                            config.CreditsRequested,
                            config.ReceiveCreditMax,
                            config.PreferredSendSize,
                            config.MaxReceiveSize,
                            config.MaxFragmentedSize
                            );

                    return true;
                }
            }
            catch (Exception ex)
            {
                logWriter.AddLog(DetectLogLevel.Information, string.Format("CheckSMBDNegotiate threw exception: {0}", ex));
                rdmaAdapterInfo = null;
                return false;
            }
        }

        private bool CheckSMBDReadWriteRDMAV1()
        {
            bool result = CheckSMBDReadWrite(DetectionInfo.SupportedSmbDialects, Channel_Values.CHANNEL_RDMA_V1);
            return result;
        }

        private bool CheckSMBDReadWriteRDMAV1Invalidate()
        {
            var dialectSupportingInvalidate = new DialectRevision[] { DialectRevision.Smb302, DialectRevision.Smb311 };
            var dialects = dialectSupportingInvalidate.Intersect(DetectionInfo.SupportedSmbDialects);
            if (dialects.Count() == 0)
            {
                return false;
            }
            bool result = CheckSMBDReadWrite(DetectionInfo.SupportedSmbDialects, Channel_Values.CHANNEL_RDMA_V1_INVALIDATE);
            return result;
        }

        private bool CheckSMBDReadWrite(DialectRevision[] dialects, Channel_Values channel)
        {
            try
            {
                using (var client = new SmbdClient(DetectionInfo.ConnectionTimeout))
                {
                    var config = DetectionInfo.SMBDClientCapability;
                    RdmaAdapterInfo rdmaAdapterInfo;
                    client.ConnectOverRDMA(DetectionInfo.DriverRdmaNICIPAddress, DetectionInfo.SUTRdmaNICIPAddress, DetectionInfo.SMBDPort, config.MaxReceiveSize, out rdmaAdapterInfo);
                    client.SMBDNegotiate(
                            config.CreditsRequested,
                            config.ReceiveCreditMax,
                            config.PreferredSendSize,
                            config.MaxReceiveSize,
                            config.MaxFragmentedSize
                            );

                    client.Smb2Negotiate(dialects);
                    client.Smb2SessionSetup(DetectionInfo.Authentication, DetectionInfo.DomainName, DetectionInfo.SUTName, DetectionInfo.UserName, DetectionInfo.Password);

                    string path = Smb2Utility.GetUncPath(DetectionInfo.SUTName, DetectionInfo.ShareFolder);
                    client.Smb2TreeConnect(path, out uint treeId);

                    client.CreateRandomFile(treeId, out FILEID fileId);

                    uint length = client.CalculateSMBDMaxReadWriteSize();
                    var buffer = Smb2Utility.CreateRandomByteArray((int)length);
                    client.SMBDWrite(treeId, fileId, channel, buffer, 0, DetectionInfo.Endian);

                    var readBuffer = new byte[length];
                    client.SMBDRead(treeId, fileId, channel, out readBuffer, 0, length, DetectionInfo.Endian);
                    if (!Enumerable.SequenceEqual(buffer, readBuffer))
                    {
                        throw new InvalidOperationException("The data is inconsistent for write and read!");
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                logWriter.AddLog(DetectLogLevel.Information, string.Format("CheckSMBDReadWrite threw exception: {0}", ex));
                return false;
            }
        }
    }
}