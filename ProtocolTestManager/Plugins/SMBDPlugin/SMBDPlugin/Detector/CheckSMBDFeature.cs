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

        public bool CheckSMBDCapability()
        {
            DetectorUtil.WriteLog("Check the supported SMBD capabilities of SUT...");

            bool result = false;

            if (CheckSMBDNegotiate())
            {
                if (CheckSMBDReadWrite())
                {
                    result = true;
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
                using (var client = new SMBDClient(new TimeSpan(0, 0, 20)))
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
            catch
            {
                return false;
            }
        }

        private bool CheckSMBDReadWrite()
        {
            try
            {
                using (var client = new SMBDClient(new TimeSpan(0, 0, 20)))
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

                    client.Smb2Negotiate(DetectionInfo.SupportedSmbDialects);

                    client.Smb2SessionSetup(DetectionInfo.Authentication, DetectionInfo.DomainName, DetectionInfo.SUTName, DetectionInfo.UserName, DetectionInfo.Password);

                    string path = Smb2Utility.GetUncPath(DetectionInfo.SUTName, DetectionInfo.ShareFolder);

                    uint treeId;

                    client.Smb2TreeConnect(path, out treeId);

                    FILEID fileId;

                    client.CreateRandomFile(treeId, out fileId);

                    uint length = client.CalculateSMBDMaxReadWriteSize();

                    var buffer = Smb2Utility.CreateRandomByteArray((int)length);

                    client.SMBDWrite(treeId, fileId, Channel_Values.CHANNEL_RDMA_V1, buffer);

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

    }

}