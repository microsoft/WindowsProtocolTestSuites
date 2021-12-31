// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestManager.SMBDPlugin.Detector;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.SMBDPlugin
{

    public class DetectionResultControl
    {
        public List<ResultItemMap> LoadDetectionInfo(DetectionInfo detectionInfo)
        {
            info = detectionInfo;

            AddDialects();
            AddSmb2TransportSupport();

            resultItemMapList.Add(dialectsItems);
            resultItemMapList.Add(smbdItems);

            return resultItemMapList;
        }

        #region Properties

        private DetectionInfo info = null;

        private const string dialectsDescription = @"""SMB Version Supported"" lists the SMB dialects supported by SUT SMB2 over SMBD implementation and at least SMB dialect 3.0 is required.";
        private const string smbdDescription = @"""SMB2 over SMBD Feature Supported"" lists the feature supported by SUT SMB2 over SMBD implementation";


        private ResultItemMap dialectsItems = new ResultItemMap() { Header = "SMB Version Supported", Description = dialectsDescription };
        private ResultItemMap smbdItems = new ResultItemMap() { Header = "SMB2 over SMBD Feature Supported", Description = smbdDescription };

        private List<ResultItemMap> resultItemMapList = new List<ResultItemMap>();

        #endregion

        #region Private functions
        private void AddDialects()
        {
            foreach (var dialect in info.SupportedSmbDialects)
            {
                string dialectName = null;
                switch (dialect)
                {
                    case DialectRevision.Smb30:
                        dialectName = "SMB 3.0";
                        break;
                    case DialectRevision.Smb302:
                        dialectName = "SMB 3.0.2";
                        break;
                    case DialectRevision.Smb311:
                        dialectName = "SMB 3.1.1";
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected dialect!");
                }
                AddResultItem(dialectsItems, dialectName, TestManager.Detector.DetectResult.Supported);
            }
        }

        private void AddSmb2TransportSupport()
        {
            if (info.DriverNonRdmaNICIPAddress == null || info.SUTNonRdmaNICIPAddress == null)
            {
                AddResultItem(smbdItems, "Multiple Channels", TestManager.Detector.DetectResult.DetectFail);
            }
            else
            {
                if (info.NonRDMATransportSupported && info.RDMATransportSupported)
                {
                    AddResultItem(smbdItems, "Multiple Channels", TestManager.Detector.DetectResult.Supported);
                }
                else
                {
                    AddResultItem(smbdItems, "Multiple Channels", TestManager.Detector.DetectResult.UnSupported);
                }
            }

            if (info.DriverRdmaNICIPAddress == null || info.SUTRdmaNICIPAddress == null)
            {
                AddResultItem(smbdItems, "RDMA Channel V1", TestManager.Detector.DetectResult.DetectFail);
                AddResultItem(smbdItems, "RDMA Channel V1 Remote Invalidate", TestManager.Detector.DetectResult.DetectFail);
            }
            else
            {
                if (info.RDMAChannelV1Supported)
                {
                    AddResultItem(smbdItems, "RDMA Channel V1", TestManager.Detector.DetectResult.Supported);
                }
                else
                {
                    AddResultItem(smbdItems, "RDMA Channel V1", TestManager.Detector.DetectResult.UnSupported);
                }

                if (info.RDMAChannelV1InvalidateSupported)
                {
                    AddResultItem(smbdItems, "RDMA Channel V1 Remote Invalidate", TestManager.Detector.DetectResult.Supported);
                }
                else
                {
                    AddResultItem(smbdItems, "RDMA Channel V1 Remote Invalidate", TestManager.Detector.DetectResult.UnSupported);
                }
            }

        }

        private void AddResultItem(ResultItemMap resultItemMap, string value, TestManager.Detector.DetectResult result)
        {
            string imagePath = string.Empty;
            switch (result)
            {
                case TestManager.Detector.DetectResult.Supported:
                    imagePath = "/SMBDPlugin;component/Icons/supported.png";
                    break;
                case TestManager.Detector.DetectResult.UnSupported:
                    imagePath = "/SMBDPlugin;component/Icons/unsupported.png";
                    break;
                case TestManager.Detector.DetectResult.DetectFail:
                    imagePath = "/SMBDPlugin;component/Icons/undetected.png";
                    break;
                default:
                    break;
            }

            ResultItem item = new ResultItem() { DetectedResult = result, ImageUrl = imagePath, Name = value };
            resultItemMap.ResultItemList.Add(item);
        }
        #endregion
    }
}
