// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;
using System.Diagnostics.Eventing;
using System.IO;
using System.Collections.ObjectModel;
using Microsoft.Win32;

namespace Microsoft.Protocols.TestTools.MessageAnalyzer
{
    public class LiveTraceSession
    {
        #region variables

        private string startScript;
        private IList<string> providerList;
        private string filePath;
        private TimeSpan timeout = new TimeSpan(0, 10, 0);
        private Process process = null;

        // variables for message trigger to stop the session
        private int remotePort = 15000;
        private IPEndPoint remoteEndpoint;
        private UdpClient udpClient;
        private byte[] udpPayload = new byte[] {0x0A, 0x0A,0x0A,0x0A };
        private string udpPayloadStr = "0A0A0A0A";
        private string triggerFilter = null;

        //Version of Windows 8.0
        private string win8Version = "6.2";

        // const string of script commands
        const string InitScript = "Import-Module PEF\n"
            + "New-PefTraceSession\n"
            + "Exit\n";

        const string StartScriptBase = "Import-Module PEF\n"
            + "$TS = New-PefTraceSession -Mode Linear -Force -Path '@CaptureFilePath' -SaveOnStop\n"
            + "@SetPefTraceFilterScript" 
            + "@AddPefMessageSourceScript"
            + "$Trigger01 = New-PefMessageTrigger -PEFSession $TS -Filter '@MessageTriggerFilter'\n"
            + "Stop-PefTraceSession -PEFSession $TS -Trigger $Trigger01\n"
            + "Start-PefTraceSession -PEFSession $TS\n"
            + "Exit\n";

        #endregion variables

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="providerList">A list of Providers used in this capture</param>
        internal LiveTraceSession(IList<string> providerList)
        {

            if (providerList == null)
            {
                providerList = new List<string>();
                if (string.Compare(GetWindowsOSVersion(), win8Version) > 0)
                {
                    providerList.Add(ETWProvider.Windows_NDIS);
                }
                else
                {
                    providerList.Add(ETWProvider.PEF_NDIS);
                }
            }

            this.providerList = providerList;
            string addMessageSourceScripts = "";
            foreach (string provider in providerList)
            {
                if (ETWProvider.Windows_NDIS.ToLower().Equals(provider.ToLower()))
                {
                    // Workaround for Powershell issue, set truncation length to 0.
                    addMessageSourceScripts += "$targetHost = New-PefTargetHost -ComputerName 'localhost'\n"
                        + "$config = $targetHost | Add-PefProviderConfig -Provider 'Microsoft-Windows-NDIS-PacketCapture'\n"
                        + "$config.Configurations[0].TruncationLength = 0\n"
                        + "Add-PefMessageSource -PEFSession $TS -Source $targetHost\n";
                }
                else
                {
                    addMessageSourceScripts += "Add-PefMessageSource -PEFSession $TS –Source '" + provider + "'\n";
                }
            }
            startScript = StartScriptBase.Replace("@AddPefMessageSourceScript", addMessageSourceScripts);
            InitMessageTrigger();
        }

        #endregion Constructor

        #region Public method

        /// <summary>
        /// Start the capture
        /// </summary>
        /// <param name="filter">A filter string</param>
        /// <param name="selectedModels">A List of strings for top level models, if this parameter is not null, capture only give messages on the level of these models</param>
        /// <param name="parse">Whether parsing the message during capturing</param>
        /// <param name="clearExistingMessages">Whether clear existing messages in the capture lists of capture</param>
        public void Start(string filePath, string filter = null)
        {
            this.filePath = filePath;

            // Delete the file if it is exist.
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            //Start the capture
            string setFilterScript = "";
            if (filter != null && !filter.Equals(""))
            {
                if (triggerFilter != null)
                {
                    // Also add trigger message filter, otherwise the session will not stop.
                    filter = "(" + filter + ") or (" + triggerFilter + ")";
                }
                setFilterScript = "Set-PefTraceFilter -PEFSession $TS -Filter '" + filter + "'\n";
            }
            startScript = startScript.Replace("@SetPefTraceFilterScript", setFilterScript);
            string scriptText = startScript.Replace("@CaptureFilePath", filePath);
            
            // Use a new Process to run the script, avoid confict when use MMA PowerShell and MMA API simutaneously in one process.
            ExecutePowerShellCommandInNewProcess(scriptText);
            
            // Wait some time for trace started. 
            // This will make sure the trace session is started to capture when this function returned.
            WaitForLiveCaptureStarted();
            
        }

        /// <summary>
        /// Stop the capture journal
        /// </summary>
        public void Stop()
        {
            // Send a message to trigger stop
            RaiseMessageTrigger();
            // Wait until the capture session stoped and capture file saved.
            // This will make sure the capture file ready when this function returned.
            DateTime endTime = DateTime.Now + timeout;
            while (!WaitForNewProcessExit(5000) && DateTime.Now < endTime)
            {
                // If the capture is not stoped, tried again until timeout.
                RaiseMessageTrigger();
            }

        }
        #endregion

        #region internal/Private methods              
                
        /// <summary>
        /// Ensure MMA PowerShell has been initialized for first use.
        /// If MMA PowrShell is not initialized for first use, initialize it.
        /// </summary>
        internal static void EnsurePowerShellInitiation()
        {
            string OPNAndConfigurationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft", "MessageAnalyzer", "OPNAndConfiguration");
            if (!Directory.Exists(OPNAndConfigurationPath))
            {
                // Initialize the powershell in another process.
                // This is a work around, this will make sure the compilation cache be generaged for first use. There is a conflict if MMA API generate it.
                ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe");
                startInfo.Arguments = InitScript;
                startInfo.WindowStyle = ProcessWindowStyle.Minimized;
                Process process = Process.Start(startInfo);
                // Wait until MMA PowerShell init finished
                process.WaitForExit();
            }
        }

        /// <summary>
        /// Wait until the live capture trace started.
        /// </summary>
        private void WaitForLiveCaptureStarted()
        {
            bool started = false;
            DateTime endTime = DateTime.Now + timeout;
            while (!started && DateTime.Now < endTime)
            {
                try
                {
                    Process logmanProcess = new Process();
                    logmanProcess.StartInfo.FileName = "logman.exe";
                    logmanProcess.StartInfo.Arguments = "query -ets";
                    logmanProcess.StartInfo.CreateNoWindow = true;
                    logmanProcess.StartInfo.UseShellExecute = false;
                    logmanProcess.StartInfo.RedirectStandardOutput = true;
                    logmanProcess.Start();
                    string traceList = logmanProcess.StandardOutput.ReadToEnd();
                    started = traceList.Contains("MMA-ETW-Livecapture");                    
                }
                catch
                {
                }

                if (!started)
                {
                    if (process.WaitForExit(1000))
                    {
                        throw new IOException("Start live capture failed, Livecapture trace is not created until the PowerShell finished.");
                    }
                }
            }

            if (!started)
            {
                throw new TimeoutException("Timeout when waiting for Live capture started.");
            }
        }

        #region PowerShell Execution
                
        /// <summary>
        /// Execute a Powershell command in a new Process
        /// </summary>
        /// <param name="scriptText">script block</param>
        private void ExecutePowerShellCommandInNewProcess(string scriptText)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe");
            startInfo.Arguments = scriptText;
            startInfo.WindowStyle = ProcessWindowStyle.Minimized;
            process = Process.Start(startInfo);
        }

        /// <summary>
        /// Wait for the new process (runing PowerShell command) exit
        /// </summary>
        /// <param name="milliseconds">wait milli-seconds</param>
        /// <returns>true if the associated process has exited; otherwise, false.</returns>
        private bool WaitForNewProcessExit(int milliseconds = 0)
        {
            if(process != null)
            {
                if (milliseconds == 0)
                {
                    process.WaitForExit();
                    return true;
                }
                else
                {
                    return process.WaitForExit(milliseconds);
                }
            }
            throw new InvalidOperationException("New process is not exist, Call ExecutePowerShellCommandInNewProcess to start a new process.");
                
        }
                
        #endregion PowerShell Execution

        #region Message Trigger

        /// <summary>
        /// Init Message Trigger for stop pef event session
        /// </summary>
        private void InitMessageTrigger()
        {
            remoteEndpoint = new IPEndPoint(IPAddress.Broadcast, remotePort);
            udpClient = new UdpClient();
            triggerFilter = "(UDP.DestinationPort == " + remotePort + ") and (UDP.Payload == $[" + udpPayloadStr + "])";
            startScript = startScript.Replace("@MessageTriggerFilter", triggerFilter);
        }

        /// <summary>
        /// Raise Message Trigger.
        /// Send a UDP packet, whose destination is 255.255.255.255, destination port is $remotePort and payload is $udpPayloadStr
        /// </summary>
        private void RaiseMessageTrigger()
        {
            udpClient.Send(udpPayload, 4, remoteEndpoint);
        }

        #endregion Message Trigger

        /// <summary>
        /// Get windows version
        /// </summary>
        /// <returns></returns>
        private string GetWindowsOSVersion()
        {
            // Using registry to get version, because System.Environment.OSVersion still return 9200 in winblue(8.1)
            String subKey = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";
            RegistryKey key = Registry.LocalMachine;
            RegistryKey skey = key.OpenSubKey(subKey);
            return (string)skey.GetValue("CurrentVersion");
        }

        #endregion Private methods
    }
}
