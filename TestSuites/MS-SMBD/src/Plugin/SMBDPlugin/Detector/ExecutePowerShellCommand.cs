// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Text.Json;
using System.Runtime.InteropServices;

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
                string psExecutable;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    psExecutable = "powershell.exe";
                }
                else
                {
                    psExecutable = "pwsh";
                }

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = psExecutable;

                string fullScriptPath = System.IO.Path.GetFullPath(scriptPath);
                startInfo.Arguments = $"-File \"{fullScriptPath}\" {DetectionInfo.DomainName} {DetectionInfo.SUTName} {DetectionInfo.UserName} {DetectionInfo.Password}";

                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;

                using (Process process = new Process())
                {
                    process.StartInfo = startInfo;
                    process.Start();

                    string output = process.StandardOutput.ReadToEnd();
                    string errorOutput = process.StandardError.ReadToEnd();

                    process.WaitForExit(); 

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
            }
            catch (System.ComponentModel.Win32Exception w32ex)
            {
                if (w32ex.NativeErrorCode == 2)
                {
                    error = new string[] { $"Critical Error: Cannot find PowerShell executable. Please install PowerShell on Linux ('sudo apt-get install -y powershell'). Exception: {w32ex.Message}" };
                    return new T[0];
                }
                error = new string[] { w32ex.ToString() };
                return new T[0];
            }
            catch (Exception ex)
            {
                error = new string[] { ex.ToString() };
                return new T[0];
            }
        }
    }
}