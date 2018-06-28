// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using Microsoft.Protocols.TestManager.Detector;
using System.DirectoryServices.Protocols;
using System.Management;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.IO;

namespace Microsoft.Protocols.TestManager.ADFSPIPPlugin
{
    public static class Utility
    {
        static string RegistryPath = @"SOFTWARE\Microsoft\ProtocolTestSuites";
        static string RegistryPath64 = @"SOFTWARE\Wow6432Node\Microsoft\ProtocolTestSuites";

        private static string GetInstalledTestSuiteVersion()
        {

            RegistryKey HKLM = Registry.LocalMachine;
            RegistryKey TestSuitesRegPath = Environment.Is64BitProcess ?
                HKLM.OpenSubKey(RegistryPath64) : HKLM.OpenSubKey(RegistryPath);

            string registryKeyName = TestSuitesRegPath.GetSubKeyNames()
                                                 .Where((s) => s.Contains("ADFSPIP"))
                                                 .FirstOrDefault();

            Match versionMatch = Regex.Match(registryKeyName, @"\d+\.\d+\.\d+\.\d+");
            return versionMatch.Value;
        }

        public static string FilePath(string filename)
        {
            return string.Format(@"C:\MicrosoftProtocolTests\MS-ADFSPIP\Client-Endpoint\{0}\Bin\Certificates\{1}", GetInstalledTestSuiteVersion(), filename);
        }

        public static bool VerifyCertExist(string filename, string stepname)
        {
            string path = FilePath(filename);
            if (File.Exists(path))
            {
                DetectorUtil.WriteLog(stepname + " success", true, LogStyle.StepPassed);
                return true;
            }
            else
            {
                DetectorUtil.WriteLog(stepname + " failed", false, LogStyle.StepFailed);
                return false;
            }
        }

        public static string ExtractIP(string str)
        {
            Regex pattern = new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
            Match match = pattern.Match(str);
            return match.Value;
        }

        public static void PingHost(string hostname, string stepname)
        {
            Ping ping = new Ping();
            IPAddress[] ipAddr = null;
            try
            {
                ipAddr = Dns.GetHostAddresses(hostname);
            }
            catch (SocketException e)
            {
                DetectorUtil.WriteLog(string.Format("{0} failed: {1}", stepname, e.Message), false, LogStyle.StepFailed);
            }
            try
            {
                ping.Send(ipAddr[0]);
            }
            catch (PingException e)
            {
                DetectorUtil.WriteLog(string.Format("{0} failed: {1}", stepname, e.Message), false, LogStyle.StepFailed);
            }

            DetectorUtil.WriteLog(stepname + " success.", true, LogStyle.StepPassed);
        }

        public static void LdapBind(string server, NetworkCredential credential)
        {
            LdapConnection ldapconn = new LdapConnection(server);
            ldapconn.Bind(credential);
            ldapconn.Dispose();
        }

        public static bool VerifyDomainAccount(string domain, string account, string password, string stepname)
        {
            try
            {
                DetectorUtil.WriteLog(
                     string.Format("{0}: {1}\n{2} Password: {3}", stepname, domain, account, password),
                     true, LogStyle.Default);
                Utility.LdapBind(domain, new System.Net.NetworkCredential(account, password, domain));
                DetectorUtil.WriteLog(
                     string.Format("{0}: {1}\n{2} is verified.", stepname, domain, account),
                     true, LogStyle.StepPassed);
            }
            catch (Exception e)
            {
                DetectorUtil.WriteLog(
                        string.Format("{0} failed: {1}", stepname, e.Message),
                        false, LogStyle.StepFailed);
                return false;
            }
            return true;
        }

        public static bool VerifyLocalAccount(string machine, string username, string password, string stepname)
        {
            DetectorUtil.WriteLog(
                string.Format("{0}: {1}\n{2}$ Password: {3}", stepname, machine, username, password),
                true, LogStyle.Default);

            ConnectionOptions options = new ConnectionOptions();
            options.Impersonation = System.Management.ImpersonationLevel.Impersonate;
            options.Username = username;
            options.Password = password;

            ManagementScope scope = new ManagementScope(@"\\" + machine + @"\root\cimv2", options);
            try
            {
                scope.Connect();

                DetectorUtil.WriteLog(string.Format("{0} passed", stepname), true, LogStyle.StepPassed);

                //Query system for Operating System information
                ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
            }
            catch (Exception e)
            {
                DetectorUtil.WriteLog(
                    string.Format("{0} failed: {1}", stepname, e.Message),
                    true, LogStyle.StepFailed);
                return false;
            }

            return true;
        }
    }
}
