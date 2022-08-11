// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using Sma = System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace Microsoft.Protocols.Tools
{
    /// <summary>
    /// Driver class for hosting a PSRemoting session.
    /// </summary>
    public class PSRemotingSession
    {
        private readonly string hostName;
        private readonly string userName;
        private readonly string keyFilePath;

        /// <summary>
        /// Creates an instance of <see cref="PSRemotingSession" />
        /// </summary>
        /// <param name="hostName">The computer name or IP address of the remote host.</param>
        /// <param name="userName">The userName for running the session on the remote machine.</param>
        /// <param name="keyFilePath">Path to the private key file to use for authenticating. If this parameter is not specified, the sshd service will attempt to use one in the default location.</param>
        public PSRemotingSession(string hostName, string userName, string keyFilePath = null)
        {
            this.hostName = hostName;
            this.userName = userName;
            this.keyFilePath = keyFilePath;
        }

        /// <summary>
        /// Runs a PowerShell script remotely and returns the result as a JSON array.
        /// </summary>
        /// <param name="script">The PowerShell script to run.</param>
        /// <returns>The result of the script execution.</returns>
        /// <exception cref="AggregateException">Thrown if there are any errors returned by the script execution.</exception>
        /// <exception cref="InvalidOperationException">Thrown if there is an error connecting to the remote host.</exception>
        public JArray Invoke(string script)
        {
            using (Runspace runspace = RunspaceFactory.CreateRunspace())
            {
                using (Sma.PowerShell powershell = Sma.PowerShell.Create())
                {

                    var command = new PSCommand();
                    command.AddCommand("New-PSSession");
                    command.AddParameter("HostName", this.hostName);
                    command.AddParameter("Username", this.userName);

                    if(!String.IsNullOrWhiteSpace(this.keyFilePath))
                    {
                        command.AddParameter("KeyFilePath", this.keyFilePath);
                    }

                    powershell.Commands = command;
                    runspace.Open();
                    powershell.Runspace = runspace;
                    Collection<PSSession> result = powershell.Invoke<PSSession>();

                    var exceptions = powershell.Streams.Error.Select(e => e.Exception).ToArray();
                    if(exceptions.Any())
                    {
                        throw new AggregateException(exceptions);
                    }

                    if (result.Count != 1)
                    {
                        throw new InvalidOperationException("Unexpected number of Remote Runspace connections returned.");
                    }

                    command = new PSCommand();
                    command.AddCommand("Set-Variable");
                    command.AddParameter("Name", "session");
                    command.AddParameter("Value", result[0]);
                    powershell.Commands = command;
                    powershell.Runspace = runspace;
                    powershell.Invoke();

                    command = new PSCommand();
                    command.AddScript("Enter-PSSession $session");
                    powershell.Commands = command;
                    powershell.Runspace = runspace;
                    powershell.Invoke();

                    command = new PSCommand();
                    var scriptBlock = @$"Invoke-Command $session -ScriptBlock {{
                        {script}
                    }}";
                    command.AddScript(scriptBlock);
                    powershell.Commands = command;
                    powershell.Runspace = runspace;

                    Collection<PSObject> results = new Collection<PSObject>();
                    results = powershell.Invoke();

                    var resultArray = new JArray();
                    foreach (PSObject psObject in results)
                    {
                        var item = new JObject();
                        foreach (PSPropertyInfo prop in psObject.Properties)
                        {
                            var name = prop.Name;
                            var value = prop.Value;

                            if (prop.Value != default(object))
                            {
                                item[prop.Name] = JToken.FromObject(prop.Value);
                            }
                        }

                        resultArray.Add(item);
                    }

                    return resultArray;
                }
            }
        }
    }
}
