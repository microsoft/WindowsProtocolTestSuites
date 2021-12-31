// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Text.Json;

namespace Microsoft.Protocols.TestManager.SMBDPlugin.Detector
{
    partial class SmbdDetector
    {
        /// <summary>
        /// Execute a PowerShell script file.
        /// </summary>
        /// <param name="scriptPath">The script file path to be executed.</param>
        /// <param name="error">A string array to receive error.</param>
        /// <returns>An object array to receive return value of script.</returns>
        private T[] ExecutePowerShellCommand<T>(string scriptPath, out string[] error)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = @"powershell.exe";
                startInfo.Arguments = $"{System.IO.Path.GetFullPath(scriptPath)} { DetectionInfo.DomainName} {DetectionInfo.SUTName} {DetectionInfo.UserName} {DetectionInfo.Password}";
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;
                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string errorOutput = process.StandardError.ReadToEnd();

                if (!string.IsNullOrEmpty(errorOutput))
                {
                    error = new string[] { errorOutput };
                }
                else
                {
                    error = null;
                }

                if (string.IsNullOrEmpty(output))
                {
                    return new T[0];
                }

                return JsonSerializer.Deserialize<T[]>(output);
            }
            catch (Exception ex)
            {
                error = new string[] { ex.ToString() };
                return new T[0];
            }
        }
    }
}