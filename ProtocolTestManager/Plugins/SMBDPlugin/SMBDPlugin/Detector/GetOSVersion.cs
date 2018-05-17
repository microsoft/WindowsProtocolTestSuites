// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestManager.Detector;

namespace Microsoft.Protocols.TestManager.SMBDPlugin.Detector
{
    partial class SMBDDetector
    {
        /// <summary>
        /// Check whether the credential of SUT is valid.
        /// </summary>
        /// <returns>true/false indicating valid/invalid.</returns>
        public bool GetOSVersion()
        {
            if (!DetectionInfo.IsWindowsImplementation)
            {
                DetectorUtil.WriteLog("Skip for non-Windows", false, LogStyle.StepSkipped);
                DetectionInfo.Platform = Platform.NonWindows;
                return true;
            }

            DetectorUtil.WriteLog("Check the OS version...");

            string[] error;

            var output = ExecutePowerShellCommand(@"..\etc\MS-SMBD\Scripts\GetRemoteOSVersion.ps1", out error);

            if (error != null)
            {
                foreach (var item in error)
                {
                    DetectorUtil.WriteLog(item.ToString());
                }
            }

            bool result = false;

            if (output.Length == 1)
            {
                try
                {
                    var ret = ParseOSVersion(output[0]);
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
                    DetectorUtil.WriteLog("The format of return value is invalid!");
                }
            }
            else
            {
                DetectorUtil.WriteLog(string.Format("Set platform to {0}.", DetectionInfo.Platform));
                DetectorUtil.WriteLog("The format of return value is invalid!");
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

        public OSVersion ParseOSVersion(dynamic inputObject)
        {
            try
            {
                var result = new OSVersion
                {
                    Caption = inputObject.Caption,
                    Version = inputObject.Version
                };
                return result;
            }
            catch
            {
                return null;
            }
        }


    }
}
