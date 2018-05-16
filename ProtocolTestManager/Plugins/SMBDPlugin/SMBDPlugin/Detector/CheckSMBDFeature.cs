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

        public bool CheckSMBDCapability(out bool rdmaChannelV1Supported, out bool rdmaChannelV1InvalidateSupported)
        {
            rdmaChannelV1Supported = false;
            rdmaChannelV1InvalidateSupported = false;

            if (DetectionInfo.SUTRdmaNICIPAddress == null || DetectionInfo.DriverRdmaNICIPAddress == null)
            {
                DetectorUtil.WriteLog("Check the supported SMBD capabilities of SUT skipped since not available.", true, LogStyle.StepSkipped);
                return false;
            }

            DetectorUtil.WriteLog("Check the supported SMBD capabilities of SUT...");

            bool result = false;

            if (CheckSMBDNegotiate())
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

        private bool CheckSMBDNegotiate()
        {
            try
            {
                using (var client = new SMBDClient(DetectionInfo.ConnectionTimeout))
                {
                    var config = DetectionInfo.SMBDClientCapability;

                    client.ConnectOverRDMA(DetectionInfo.DriverRdmaNICIPAddress, DetectionInfo.SUTRdmaNICIPAddress, DetectionInfo.SMBDPort, config.MaxReceiveSize);


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
                DetectorUtil.WriteLog(String.Format("CheckSMBDNegotiate threw exception: {0}", ex));
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
                using (var client = new SMBDClient(DetectionInfo.ConnectionTimeout))
                {
                    var config = DetectionInfo.SMBDClientCapability;

                    client.ConnectOverRDMA(DetectionInfo.DriverRdmaNICIPAddress, DetectionInfo.SUTRdmaNICIPAddress, DetectionInfo.SMBDPort, config.MaxReceiveSize);


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

                    uint treeId;

                    client.Smb2TreeConnect(path, out treeId);

                    FILEID fileId;

                    client.CreateRandomFile(treeId, out fileId);

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
                DetectorUtil.WriteLog(String.Format("CheckSMBDReadWrite threw exception: {0}", ex));
                return false;
            }
        }

    }

}