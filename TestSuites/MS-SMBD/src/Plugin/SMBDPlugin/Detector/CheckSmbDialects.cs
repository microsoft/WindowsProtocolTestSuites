// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using System;
using System.Linq;
using System.Net;

namespace Microsoft.Protocols.TestManager.SMBDPlugin.Detector
{
    partial class SmbdDetector
    {
        public bool CheckSmbDialects()
        {
            logWriter.AddLog(DetectLogLevel.Information, "Check the supported SMB dialects of SUT...");
            bool result = false;
            var dialects = new DialectRevision[] { DialectRevision.Smb30, DialectRevision.Smb302, DialectRevision.Smb311 };
            var ipList = GetIpAddressListOfSut();
            // try all reachable SUT IP address
            foreach (var ip in ipList)
            {
                var supportedDialects = TryNegotiateDialects(ip, dialects);
                if (supportedDialects.Length > 0)
                {
                    DetectionInfo.SupportedSmbDialects = supportedDialects;
                    logWriter.AddLog(DetectLogLevel.Information, string.Format("SMB dialects supported by SUT: {0}", string.Join(",", supportedDialects)));
                    result = true;
                    break;
                }
            }
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

        private DialectRevision[] TryNegotiateDialects(IPAddress ip, DialectRevision[] dialects)
        {
            var result = dialects.Where(dialect => TryNegotiateDialect(ip, dialect));
            return result.ToArray();
        }

        private bool TryNegotiateDialect(IPAddress ip, DialectRevision dialect)
        {
            try
            {
                using (var client = new SmbdClient(DetectionInfo.ConnectionTimeout))
                {
                    client.Connect(ip, IPAddress.Parse(DetectionInfo.DriverNonRdmaNICIPAddress));
                    client.Smb2Negotiate(new DialectRevision[] { dialect });
                    return true;
                }
            }
            catch (Exception ex)
            {
                logWriter.AddLog(DetectLogLevel.Information, string.Format("TryNegotiateDialect threw exception: {0}", ex));
                return false;
            }
        }
    }
}