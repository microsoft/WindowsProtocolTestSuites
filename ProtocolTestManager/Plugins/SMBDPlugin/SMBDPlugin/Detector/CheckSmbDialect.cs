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

        public bool CheckSmbDialect()
        {
            DetectorUtil.WriteLog("Check the supported SMB dialects of SUT...");

            bool result = false;

            var dialects = new DialectRevision[] { DialectRevision.Smb30, DialectRevision.Smb302, DialectRevision.Smb311 };

            var ipList = GetIPAdressOfSut();

            // try all reachable SUT IP address
            foreach (var ip in ipList)
            {
                var supportedDialects = TryNegotiateDialects(ip, dialects);
                if (supportedDialects.Length > 0)
                {
                    DetectionInfo.SupportedSmbDialects = supportedDialects;
                    DetectorUtil.WriteLog(String.Format("SMB dialects supported by SUT: {0}", String.Join(",", supportedDialects)));
                    result = true;
                    break;
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

        private DialectRevision[] TryNegotiateDialects(IPAddress ip, DialectRevision[] dialects)
        {
            var result = dialects.Where(dialect => TryNegotiateDialect(ip, dialect));

            return result.ToArray();
        }

        private bool TryNegotiateDialect(IPAddress ip, DialectRevision dialect)
        {
            try
            {
                using (var client = new SMBDClient(DetectionInfo.ConnectionTimeout))
                {
                    client.Connect(ip, IPAddress.Parse(DetectionInfo.DriverNonRdmaNICIPAddress));

                    client.Smb2Negotiate(new DialectRevision[] { dialect });

                    return true;
                }
            }
            catch (Exception ex)
            {
                DetectorUtil.WriteLog(String.Format("TryNegotiateDialect threw exception: {0}", ex));
                return false;
            }
        }
    }
}