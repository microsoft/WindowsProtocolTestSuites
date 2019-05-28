// Copyright(c) Microsoft.All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

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
                using (var runspace = RunspaceFactory.CreateRunspace())
                {

                    runspace.Open();

                    runspace.SessionStateProxy.SetVariable("PtfProp_SUTName", DetectionInfo.SUTName);
                    runspace.SessionStateProxy.SetVariable("PtfProp_DomainName", DetectionInfo.DomainName);
                    runspace.SessionStateProxy.SetVariable("PtfProp_SUTUserName", DetectionInfo.UserName);
                    runspace.SessionStateProxy.SetVariable("PtfProp_SUTUserPassword", DetectionInfo.Password);

                    using (var pipeline = runspace.CreatePipeline())
                    {
                        pipeline.Commands.Add(scriptPath);

                        var output = pipeline.Invoke();

                        var result = output.Select(element => ParsePSObject(element));


                        // output error
                        if (pipeline.HadErrors)
                        {
                            var errors = pipeline.Error.ReadToEnd();

                            error = errors.Select(element => element.ToString()).ToArray();
                        }
                        else
                        {
                            error = null;
                        }

                        return result.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                error = new string[] { ex.ToString() };
                return new object[0];
            }
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