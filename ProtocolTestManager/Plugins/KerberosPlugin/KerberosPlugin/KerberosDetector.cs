// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestManager.KerberosPlugin
{
    public class KerberosDetector
    {
        #region Properties

        public Logger logWriter = null;

        #endregion
        public KerberosDetector(Logger logger)
        {
           
            logWriter = logger;            
            logWriter.AddLineToLog(LogLevel.Advanced);
        }
              
        public bool FetchPlatformInfo(string computerName)
        {
            bool isWindows  = true;
            try
            {
                ManagementObjectCollection resultCollection = QueryWmiObject(computerName, "SELECT * FROM Win32_OperatingSystem");
                foreach (ManagementObject result in resultCollection)
                {
                    isWindows = (ConvertPlatform(result["Caption"].ToString()) != Platform.NonWindows);
                    logWriter.AddLog("Platform: " + ConvertPlatform(result["Caption"].ToString()), LogLevel.Advanced);
                    break;
                }                
            }
            catch
            {
                
            }
            return isWindows;
        }

        private Platform ConvertPlatform(string platform)
        {
            if (platform.Contains("Windows Server 2016"))
                return Platform.WindowsServer2016;
            if (platform.Contains("Windows Server 2012 R2"))
                return Platform.WindowsServer2012R2;
            else if (platform.Contains("Windows Server 2012"))
                return Platform.WindowsServer2012;
            else if (platform.Contains("Windows Server 2008 R2"))
                return Platform.WindowsServer2008R2;
            else if (platform.Contains("Windows Serer 2008"))
                return Platform.WindowsServer2008;
            else
                return Platform.NonWindows;
        }
              

        private ManagementObjectCollection QueryWmiObject(string machineName, string queryString)
        {
            ConnectionOptions options = new ConnectionOptions() { Timeout = new TimeSpan(0, 0, 5) };
            ManagementScope ms = new ManagementScope("\\\\" + machineName + "\\root\\cimv2", options);
            ms.Connect();
            ObjectQuery query = new ObjectQuery(queryString);
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(ms, query);
            return searcher.Get();
        }

   
        private void LogFailedStatus(string operation, uint status)
        {
            logWriter.AddLog(string.Format(operation + " failed, status: {0}", Smb2Status.GetStatusCode(status)), LogLevel.Advanced);
        }
    }

    public class NetworkInfo
    {
        public List<IPAddress> SUTIpList { get; set; }
        public List<IPAddress> LocalIpList { get; set; }
    }

    public class Smb2Info
    {
        public DialectRevision MaxSupportedDialectRevision { get; set; }
        public Capabilities_Values SupportedCapabilities { get; set; }
    }

    public class ShareInfo
    {
        public string ShareName { get; set; }
        public ShareType_Values ShareType { get; set; }
        public Share_Capabilities_Values ShareCapabilities { get; set; }
        public ShareFlags_Values ShareFlags { get; set; }
    }   
}
