// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Detector;
using System.Reflection;

namespace Microsoft.Protocols.TestManager.SMBDPlugin.Detector
{
    partial class SmbdDetector
    {
        /// <summary>
        /// Check whether the credential of SUT is valid.
        /// </summary>
        /// <returns>true/false indicating valid/invalid.</returns>
        public bool GetOSVersion()
        {
            if (!DetectionInfo.IsWindowsImplementation)
            {
                logWriter.AddLog(DetectLogLevel.Warning, "Failed", false, LogStyle.StepSkipped);
                logWriter.AddLog(DetectLogLevel.Information, "Skip for non-Windows");
                DetectionInfo.Platform = Platform.NonWindows;
                return true;
            }

            logWriter.AddLog(DetectLogLevel.Information, "Check the OS version...");

            string path = Assembly.GetExecutingAssembly().Location + "/../../Plugin/script/GetRemoteOSVersion.ps1";
            OSVersion[] output = ExecutePowerShellCommand<OSVersion>(path, out string[] error);

            if (error != null)
            {
                foreach (var item in error)
                {
                    logWriter.AddLog(DetectLogLevel.Information, item.ToString());
                }
            }

            bool result = false;
            if (output.Length == 1)
            {
                try
                {
                    var ret = output[0];
                    if (ret != null)
                    {
                        if (MapOSVersion(ret))
                        {
                            result = true;
                        }
                    }
                }
                catch
                {
                    logWriter.AddLog(DetectLogLevel.Information, "The format of return value is invalid!");
                }
            }
            else
            {
                logWriter.AddLog(DetectLogLevel.Information, string.Format("Set platform to {0}.", DetectionInfo.Platform));
                logWriter.AddLog(DetectLogLevel.Information, "The format of return value is invalid!");
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

        private bool MapOSVersion(OSVersion os)
        {
            bool found = false;
            if (os.Caption.Contains("Server"))
            {
                if (os.Version.StartsWith("10.0."))
                {
                    DetectionInfo.Platform = Platform.WindowsServer2016;
                    found = true;
                }
                else if (os.Version.StartsWith("6.3."))
                {
                    DetectionInfo.Platform = Platform.WindowsServer2012R2;
                    found = true;
                }
                else if (os.Version.StartsWith("6.2."))
                {
                    DetectionInfo.Platform = Platform.WindowsServer2012;
                    found = true;
                }
                else if (os.Version.StartsWith("6.1."))
                {
                    DetectionInfo.Platform = Platform.WindowsServer2008R2;
                    found = true;
                }
                else if (os.Version.StartsWith("6.0."))
                {
                    DetectionInfo.Platform = Platform.WindowsServer2008;
                    found = true;
                }
            }

            return found;
        }
    }
}
