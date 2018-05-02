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
                return true;
            }

            DetectorUtil.WriteLog("Check the OS version...");

            string[] error;

            var output = ExecutePowerShellCommand(@"..\etc\MS-SMBD\GetRemoteOSVersion.ps1", out error);

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
                    int ret = (int)output[0];
                    if (ret == 0)
                    {
                        result = true;
                    }
                }
                catch
                {
                    DetectorUtil.WriteLog("The format of return value is invalid!");
                }
            }
            else
            {
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




    }
}
