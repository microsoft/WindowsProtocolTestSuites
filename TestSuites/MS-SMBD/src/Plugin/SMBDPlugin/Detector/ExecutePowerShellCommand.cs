// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reflection;

namespace Microsoft.Protocols.TestManager.SMBDPlugin.Detector
{
    partial class SMBDDetector
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
                //using (var runspace = RunspaceFactory.CreateOutOfProcessRunspace(null))
                //{
                //    var pathb = Environment.GetEnvironmentVariable("KEY_VAULT_NAME").Split(";");
                //    var patha = Environment.GetEnvironmentVariable("PSModulePath").Split(";");

                //    runspace.Open();
                //    //runspace.SessionStateProxy.SetVariable("PtfProp_SUTName", DetectionInfo.SUTName);
                //    //runspace.SessionStateProxy.SetVariable("PtfProp_DomainName", DetectionInfo.DomainName);
                //    //runspace.SessionStateProxy.SetVariable("PtfProp_SUTUserName", DetectionInfo.UserName);
                //    //runspace.SessionStateProxy.SetVariable("PtfProp_SUTUserPassword", DetectionInfo.Password);
                //    using (var pipeline = runspace.CreatePipeline())
                //    {
                //        //pipeline.Commands.AddScript("install-Module WindowsCompatibility");
                //        //pipeline.Commands.AddScript("Install-Module NetAdapter");

                //        //pipeline.Commands.AddScript("Get-Module");
                //        //pipeline.Commands.AddScript("Get-NetAdapter");
                //        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                //        {
                //            //set execution policy for Windows in order to load and run local script files
                //            pipeline.Commands.AddScript("Set-ExecutionPolicy -Scope Process RemoteSigned");
                //        }
                //        string path = string.Format(". \"{0}\"", System.IO.Path.GetFullPath(scriptPath));
                //        pipeline.Commands.AddScript(path);

                //        var result = pipeline.Invoke();
                //        error = null;
                //        return null;
                //        //var result = output.Select(element => ParsePSObject(element));


                //        // output error

                //    }
                //}
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = @"powershell.exe";
                startInfo.Arguments=@"" + System.IO.Path.GetFullPath(scriptPath) + " "+DetectionInfo.DomainName+" "+ DetectionInfo.SUTName+" "+ DetectionInfo.UserName+" "+DetectionInfo.Password+"";
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;
                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();

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
                //var test = System.Text.Json.JsonSerializer.Deserialize<List<object>>(output);
                //var test = (JArray)JsonConvert.DeserializeObject(output);
                //var result = test.Children<JObject>().Select(element => ParsePSObject(element));
                //var result = test.Select(element => ParsePSObject(element));
                error = new string[] { process.StandardError.ReadToEnd() };
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

        private object ParsePSObject(PSObject psObject)
        {
            if (psObject.BaseObject is PSCustomObject)
            {
                var result = new ExpandoObject();
                foreach (var property in psObject.Properties)
                {
                    (result as IDictionary<string, Object>).Add(property.Name, property.Value);
                }
                return result;
            }
            else
            {
                return psObject.BaseObject;
            }
        }
    }
}