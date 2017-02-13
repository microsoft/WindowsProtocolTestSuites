// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocol.TestSuites.ADOD.Adapter.Util;
using System.Globalization;

namespace Microsoft.Protocol.TestSuites.ADOD.Adapter
{

    /// <summary>
    /// Test settings read from PTF configuration file.
    /// </summary>
    public class ADODTestConfig
    {
        #region Class Members

        #region PTF Protocol TestSite

        public ITestSite TestSite
        {
            get;
            private set;
        }
        
        #endregion

        #region Domain Information

        /// <summary>
        /// Gets the NetBIOS domain name from PTF configuration file.
        /// </summary>
        public string NetbiosDomainName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the full domain name from PTF configuration file.
        /// </summary>
        public string FullDomainName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the root domain naming context from PTF configuration file.
        /// </summary>
        public string RootDomainNC
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the domain administrator username from PTF configuration file.
        /// </summary>
        public string DomainAdminUsername
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the domain administrator password from PTF configuration file.
        /// </summary>
        public string DomainAdminPwd
        {
            get;
            private set;
        }
        
        #endregion

        #region Domain Controller Computer Information

        /// <summary>
        /// Gets the operating system of the primary domain controller from PTF configuration file.
        /// </summary>
        public string PDCOperatingSystem
        {
            get;
            private set;
        }
                
        /// <summary>
        /// Gets the NetBIOS name of the primary domain controller from PTF configuration file.
        /// </summary>
        public string NetbiosComputerName
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets the FQDN of the primary domain controller from PTF configuration file.
        /// </summary>
        public string PDCComputerName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the IP address of the primary domain controller from PTF configuration file.
        /// </summary>
        public string PDCIP
        {
            get;
            private set;
        }

        #endregion

        #region Client Computer Information

        /// <summary>
        /// Gets the operating system of the client computer from PTF configuration file.
        /// </summary>
        public string ClientOperatingSystem
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the operating system version of the client computer from PTF configuration file:
        /// 5 = Windows XP; 6 = Windows Vista; 7 = Windows 7; 8 = Windows 8; 9 = Windows 8.1; 10 = Windows 10; etc.
        /// </summary>
        public float ClientOSVersion
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the FQDN of the client computer from PTF configuration file.
        /// </summary>
        public string ClientComputerName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the IP address of the client computer from PTF configuration file.
        /// </summary>
        public string ClientIP
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the local administrator username of the client computer from PTF configuration file.
        /// </summary>
        public string ClientAdminUsername
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the local administrator password of the client computer from PTF configuration file.
        /// </summary>
        public string ClientAdminPwd
        {
            get;
            private set;
        }

        #endregion

        #region Scripts/Logs Path and Sleep Time for Scripts

        /// <summary>
        /// Gets the logging path for PowerShell adapters on the driver computer from PTF configuration file.
        /// </summary>
        public string DriverLogPath
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the scripts path on the client computer from PTF configuration file.
        /// Windows: PowerShell scripts
        /// NonWindows: Shell scripts, etc
        /// </summary>
        public string ClientScriptPath
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the scripts logging path on the client computer from PTF configuration file.
        /// Windows: PowerShell scripts logs
        /// Non-Windows: Shell scripts logs, etc.
        /// </summary>
        public string ClientLogPath
        {
            get;
            private set;
        }

        
        /// <summary>
        /// Gets the Sleep time for test cases from PTF configuration file.
        /// </summary>
        public int JoinDomainCreateAcctLDAPSleepTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Sleep time for test cases from PTF configuration file.
        /// </summary>
        public int JoinDomainCreateAcctSAMRSleepTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Sleep time for test cases from PTF configuration file.
        /// </summary>
        public int JoinDomainPredefAcctSleepTime
        {
            get;
            private set;
        }
               

        /// <summary>
        /// Gets the Sleep time for test cases from PTF configuration file.
        /// </summary>
        public int UnjoinDomainSleepTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Disables the trigger for SUT control adapters and Message Analyzer live captures.
        /// </summary>
        public bool TriggerDisabled
        {
            get;
            private set;
        }

        #endregion

        #region Telnet Configuration
        /// <summary>
        /// Gets the telnet port number from PTF configuration file.
        /// </summary>
        public UInt16 TelnetPort
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the non-windows script names on client computer from PTF configuration file.
        /// </summary>
        public string LocateDomainControllerScript
        {
            get;
            private set;
        }

        public string JoinDomainCreateAcctLDAPScript
        {
            get;
            private set;
        }

        public string JoinDomainCreateAcctSAMRScript
        {
            get;
            private set;
        }

        public string JoinDomainPredefAcctScript
        {
            get;
            private set;
        }

        public string UnjoinDomainScript
        {
            get;
            private set;
        }

        public string IsJoinDomainSuccessScript
        {
            get;
            private set;
        }

        public string IsUnjoinDomainSuccessScript
        {
            get;
            private set;
        }

        #endregion

        #region Test Case Specific Configuration
        /// <summary>
        /// Gets the retry interval from PTF configuration file.
        /// When the remote server is rebooted, some methods need to wait for the services to be ready on that remote server.
        /// As soon as the service starts up, the method will return a result, either success or failure.
        /// Otherwise, the method will throw exception.
        /// </summary>
        public TimeSpan Timeout
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the single method retry interval in seconds from PTF configuration file.
        /// When the remote server is rebooted, some methods need to wait for the services to be ready on that remote server.
        /// This interval indicates the retry frequency when this method returns failure.
        /// As soon as the service starts up, the method will return a result, either success or failure.
        /// Otherwise, the method will throw exception.
        /// </summary>
        public TimeSpan RetryInterval
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the domain new user username from PTF configuration file.
        /// </summary>
        public string DomainNewUserUsername
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the domain new user password from PTF configuration file.
        /// </summary>
        public string DomainNewUserPwd
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the domain new user SAMAccountName from PTF configuration file.
        /// </summary>
        public string DomainNewUserSamAccountName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the domain new user new password from PTF configuration file.
        /// </summary>
        public string DomainNewUserNewPwd
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the domain new group from PTF configuration file.
        /// </summary>
        public string DomainNewGroup
        {
            get;
            private set;
        }
        #endregion

        #region Message Analyzer Trace Configuration

        /// <summary>
        /// Gets the capture file path on the local computer from PTF configuration file.
        /// Local computer can be PDCComputer, Client Computer and Driver Computer, as long as the OS is Windows.
        /// </summary>
        public string LocalCapFileName
        {
            get;
            private set;
        }

        #endregion

        #endregion

        #region Methods

        #region MA methods

        /// <summary>
        /// Gets the expected message sequence file by test case name.
        /// </summary>
        /// <param name="caseName">test case name</param>
        /// <returns></returns>
        public string GetMAExpectedSequenceFile(string caseName)
        {
            string expectFile = string.Format(CultureInfo.InvariantCulture, "{0}-ExpectedFrames-MA.xml", caseName);

            string capFileName = string.Format(CultureInfo.InvariantCulture, "{0}-{1}.matp", caseName, this.ClientOperatingSystem);
            string capFilePath = this.TestSite.Properties["LocalCapFilePath"];

            if (!Directory.Exists(capFilePath))
            {
                Directory.CreateDirectory(capFilePath);
            }

            this.LocalCapFileName = Path.Combine(capFilePath, capFileName);
            this.TestSite.Assert.IsNotNull(this.LocalCapFileName, "Capture file name: {0}", this.LocalCapFileName);

            return expectFile;
        }

        /// <summary>
        /// Updates the expected message files according to the PTF configuration file.
        /// Replaces the variables in the xml file.
        /// </summary>
        /// <param name="filename"></param>
        public void UpdateExpectedSequenceFile(string filename)
        {
            string content = string.Empty;
            using (StreamReader reader = new StreamReader(filename))
            {
                content = reader.ReadToEnd();
            }

            foreach (var property in this.GetType().GetProperties())
            {
                if (property.GetValue(this, null) is string[])
                {
                    string[] stringValues = (string[])property.GetValue(this, null);
                    for (int i = 0; i < stringValues.Length; i++)
                    {
                        // for the properties are defined with multiple values, split these values by ",".
                        string[] arrayValues = stringValues[i].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                        // replace the expect sequence file with splited values by the index
                        // for example: {CentralAccessPolicyNames{0}} will be replaced by the first value of the property CentralAccessPolicyNames in ptfconfig file
                        content = content.Replace("{" + property.Name + "[" + i + "]}", stringValues[i]);
                    }
                }
                else
                {
                    content = Regex.Replace(content, "{" + property.Name + "}", property.GetValue(this, null).ToString());
                }
            }

            using (StreamWriter writer = new StreamWriter(filename, false))
            {
                writer.Write(content);
            }
        }

        #endregion


        /// <summary>
        /// Gets the domain distinguished name (DN) from its full qualified domain name (FQDN).
        /// </summary>
        /// <param name="domainName">Specifies the full qualified domain name.</param>
        /// <returns>domain distinguished name</returns>
        private string GetDomainDnFromDomainName(string domainName)
        {
            string[] strs = domainName.Split('.');
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < strs.Length; i++)
            {
                sb.Append("DC=");
                sb.Append(strs[i]);
                if (i != strs.Length - 1)
                    sb.Append(",");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the computer roles defined in PTF Configuration file.
        /// The MA adapter can map the computer IP and computer name by the dictionary.
        /// </summary>
        /// <returns>Returns a dictionary that will become the input for the MA Adapter which maps the computer IP and computer name.</returns>
        public Dictionary<string, EndpointRole> GetEndpointRoles()
        {
            Dictionary<string, EndpointRole> endpointRoles = new Dictionary<string, EndpointRole>();
            EndpointRole clientRole = new EndpointRole();
            clientRole.Role = ClientComputerName;
            clientRole.ComputerName = ClientComputerName;
            clientRole.IPv4 = ClientIP;
            clientRole.IPv6 = null;
            clientRole.MAC = null;

            EndpointRole pdcRole = new EndpointRole();
            pdcRole.Role = PDCComputerName;
            pdcRole.ComputerName = PDCComputerName;
            pdcRole.IPv4 = PDCIP;
            pdcRole.IPv6 = null;
            pdcRole.MAC = null;

            endpointRoles.Add(clientRole.Role, clientRole);
            endpointRoles.Add(pdcRole.Role, pdcRole);

            return endpointRoles;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Test Configurations in MS-ADOD test suite.
        /// </summary>
        /// <param name="testSite">Test Site</param>
        public ADODTestConfig(ITestSite testSite)
        {
            this.TestSite = testSite;

            //Domain Information
            this.FullDomainName = testSite.Properties.Get("FullDomainName");
            if (this.FullDomainName.IndexOf(".", StringComparison.OrdinalIgnoreCase) == -1)
            {
                this.NetbiosDomainName = this.FullDomainName;
            }
            else
            {
                this.NetbiosDomainName = this.FullDomainName.Substring(0, this.FullDomainName.IndexOf(".", StringComparison.OrdinalIgnoreCase));
            }
            this.RootDomainNC = GetDomainDnFromDomainName(FullDomainName);
            this.DomainAdminUsername = testSite.Properties.Get("DomainAdminUsername");
            this.DomainAdminPwd = testSite.Properties.Get("DomainAdminPwd");

            //Primary Domain Controller Computer Information
            this.PDCOperatingSystem = testSite.Properties.Get("PDCOperatingSystem");
            this.PDCComputerName = testSite.Properties.Get("PDCComputerName");
            if (this.PDCComputerName.IndexOf(".", StringComparison.OrdinalIgnoreCase) == -1)
            {
                this.NetbiosComputerName = this.PDCComputerName;
            }
            else
            {
                this.NetbiosComputerName = this.PDCComputerName.Substring(0, this.PDCComputerName.IndexOf(".", StringComparison.OrdinalIgnoreCase));
            }
            this.PDCIP = testSite.Properties.Get("PDCIP");

            //Client Computer Information
            this.ClientOperatingSystem = testSite.Properties.Get("ClientOperatingSystem");
            this.ClientOSVersion = float.Parse(testSite.Properties.Get("ClientOSVersion"), CultureInfo.InvariantCulture);
            this.ClientComputerName = testSite.Properties.Get("ClientComputerName");
            this.ClientIP = testSite.Properties.Get("ClientIP");
            this.ClientAdminUsername = testSite.Properties.Get("ClientAdminUsername");
            this.ClientAdminPwd = testSite.Properties.Get("ClientAdminPwd");

            //Logs and Scrips Configuration
            this.DriverLogPath = testSite.Properties.Get("DriverLogPath");
            this.ClientScriptPath = testSite.Properties.Get("ClientScriptPath");
            this.ClientLogPath = testSite.Properties.Get("ClientLogPath");
            this.TriggerDisabled = Boolean.Parse(testSite.Properties.Get("TriggerDisabled"));

            //Telnet Configuration
            this.TelnetPort = UInt16.Parse(testSite.Properties.Get("TelnetPort"), CultureInfo.InvariantCulture);
            //these script names are only for Non-Windows environment
            this.LocateDomainControllerScript = testSite.Properties.Get("LocateDomainControllerScript");
            this.JoinDomainCreateAcctLDAPScript = testSite.Properties.Get("JoinDomainCreateAcctLDAPScript");
            this.JoinDomainCreateAcctSAMRScript = testSite.Properties.Get("JoinDomainCreateAcctSAMRScript");
            this.JoinDomainPredefAcctScript = testSite.Properties.Get("JoinDomainPredefAcctScript");
            this.UnjoinDomainScript = testSite.Properties.Get("UnjoinDomainScript");
            this.IsJoinDomainSuccessScript = testSite.Properties.Get("IsJoinDomainSuccessScript");
            this.IsUnjoinDomainSuccessScript = testSite.Properties.Get("IsUnjoinDomainSuccessScript");

            //Test Case Specific Configuration
            this.Timeout = TimeSpan.FromSeconds(int.Parse(testSite.Properties.Get("Timeout"), CultureInfo.InvariantCulture));
            this.RetryInterval = TimeSpan.FromSeconds(int.Parse(testSite.Properties.Get("RetryInterval"), CultureInfo.InvariantCulture));
            this.DomainNewUserUsername = testSite.Properties.Get("DomainNewUserUsername");
            this.DomainNewUserPwd = testSite.Properties.Get("DomainNewUserPwd");
            this.DomainNewUserSamAccountName = testSite.Properties.Get("DomainNewUserSamAccountName");
            this.DomainNewUserNewPwd = testSite.Properties.Get("DomainNewUserNewPwd");
            this.DomainNewGroup = testSite.Properties.Get("DomainNewGroup");
        }

        #endregion
    }
}