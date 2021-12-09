// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestManager.Detector;
using System.Reflection;

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
                logWriter.AddLog(DetectLogLevel.Warning, "Skip for non-Windows", false, LogStyle.StepSkipped);
                logWriter.AddLog(DetectLogLevel.Information, "Skip for non-Windows");
                DetectionInfo.Platform = Platform.NonWindows;
                return true;
            }

            logWriter.AddLog(DetectLogLevel.Information, "Check the OS version...");

            string[] error;
            string path = Assembly.GetExecutingAssembly().Location + "/../../Plugin/script/GetRemoteOSVersion.ps1";
            var output = ExecutePowerShellCommand(path, out error);

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
                logWriter.AddLog(DetectLogLevel.Information, "Finished");
                return true;
            }
            else
            {
                logWriter.AddLog(DetectLogLevel.Warning, "Failed", false, LogStyle.StepFailed);
                logWriter.AddLog(DetectLogLevel.Information, "Failed");
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
