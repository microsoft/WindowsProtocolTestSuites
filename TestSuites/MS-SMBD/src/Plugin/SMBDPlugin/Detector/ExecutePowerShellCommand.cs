// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;

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
        private object[] ExecutePowerShellCommand(string scriptPath, out string[] error)
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
                    return new object[0];
                }

                using var doc = System.Text.Json.JsonDocument.Parse(output);
                System.Text.Json.JsonElement root = doc.RootElement;
                var result = new List<ExpandoObject>();
                if (root.ValueKind == System.Text.Json.JsonValueKind.Array)
                {
                    var elements = root.EnumerateArray();
                    while (elements.MoveNext())
                    {
                        var element = elements.Current;
                        result.Add(ParsePSObject(element));
                    }
                }
                else
                {
                    result.Add(ParsePSObject(root));
                }

                return result.ToArray();
            }
            catch (Exception ex)
            {
                error = new string[] { ex.ToString() };
                return new object[0];
            }
        }

        private ExpandoObject ParsePSObject(System.Text.Json.JsonElement element)
        {
            var props = element.EnumerateObject();
            var ExpandoObject = new ExpandoObject();
            while (props.MoveNext())
            {
                var prop = props.Current;
                (ExpandoObject as IDictionary<string, Object>).Add(prop.Name, (prop.Value.ValueKind == System.Text.Json.JsonValueKind.True || prop.Value.ValueKind == System.Text.Json.JsonValueKind.False) ? prop.Value.GetBoolean() : prop.Value.GetString());
            }
            return ExpandoObject;
        }
    }
}