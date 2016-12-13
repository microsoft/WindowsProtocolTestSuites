// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocol.TestSuites.Azod.Adapter.Util;
using Microsoft.Protocols.TestTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Microsoft.Protocol.TestSuites.Azod.Adapter
{
    /// <summary>
    /// Test settings read from PTF config file
    /// </summary>
    public class AzodTestConfig
    {
        #region Ptf TestSite
        public ITestSite TestSite
        {
            get;
            private set;
        }
        #endregion

        #region Global Configuration       

        /// <summary>
        ///Get the KDC domain name from PTF config file
        /// </summary>
        public string KdcDomainName
        {
            get;
            private set;
        }

        /// <summary>
        ///Get the KDC domain netbios from KDC domain name
        /// </summary>
        public string KdcDomainNetbios
        {
            get;
            private set;
        }

        /// <summary>
        ///Get KDC domain controller name from PTF config file
        /// </summary>
        public string KdcName
        {
            get;
            private set;
        }
        /// <summary>
        /// Get KDC domain controller IP from PTF config file
        /// </summary>
        public string KDCIP
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the KDC domain administrator username from PTF config file
        /// </summary>
        public string KdcAdminUser
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the KDC domain administrator password from PTF config file
        /// </summary>
        public string KdcAdminPwd
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the KDC domain claim user username from PTF config file
        /// </summary>
        public string KdcClaimUser
        {
            get;
            private set;
        }

        /// <summary>
        ///Get the KDC domain claim user password from PTF config file
        /// </summary>
        public string KdcClaimUserPwd
        {
            get;
            private set;
        }


        /// <summary>
        ///Get the application server name from PTF config file
        /// </summary>
        public string ApplicationServerName
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the application server name from PTF config file
        /// </summary>
        public string ApplicationServerIP
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the client computer name from PTF config file
        /// </summary>
        public string ClientComputerName
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the client local administrator username from PTF config file
        /// </summary>
        public string ClientAdminUser
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the client local administrator password from PTF config file
        /// </summary>
        public string ClientAdminPwd
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the client computer IP
        /// </summary>
        public string ClientComputerIp
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the cross forest name from PTF config file
        /// </summary>
        public string CrossForestName
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the cross forest DC name from PTF config
        /// </summary>
        public string CrossForestDCName
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the cross forest DC IP from PTF config
        /// </summary>
        public string CrossForestDCIP
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the cross forest administrator username from PTF config
        /// </summary>
        public string CrossForestAdminUser
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the cross forest administrator password from PTF config
        /// </summary>
        public string CrossForestAdminPwd
        {
            get;
            private set;
        }       

        /// <summary>
        /// Get the cross forest application server name from PTF config
        /// </summary>
        public string CrossForestApplicationServerName
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the cross forest application server IP from PTF config
        /// </summary>
        public string CrossForestApplicationServerIP
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the cross forest application server share folder from PTF config
        /// </summary>
        public string CrossForestApplicationServerShareFolder
        {
            get;
            private set;
        }
       
        /// <summary>
        /// Get Max SMB2 Dialect version supported
        /// </summary>
        public string MaxSMB2DialectSupported
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the ScriptPath from PTF config file
        /// </summary>
        public string ScriptPath
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the SiteName from PTF config file
        /// </summary>
        public string SiteName
        {
            get;
            private set;
        }
      
        /// <summary>
        /// Get the UNC path
        /// </summary>
        public string UncPath
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the FQDN UNC path
        /// </summary>
        public string FQDNUncPath
        {
            get;
            private set;
        }    
    
        #endregion

        #region Case Level MA Capture Configuration
        
        /// <summary>
        /// Get the local expected frames path
        /// </summary>
        public string ExpectedSequenceFilePath
        {
            get;
            set;
        }

        /// <summary>
        /// Get the local netmon capture file path
        /// </summary>
        public string LocalCapFilePath
        {
            get;
            set;
        }

        /// <summary>
        /// Get the log files folder
        /// </summary>
        public string DriverLogPath
        {
            get;
            set;
        }

        /// <summary>
        /// Get Central Access Policies to be tested
        /// </summary>
        public string[] CentralAccessPolicyNames
        {
            get;
            set;
        }

        /// <summary>
        /// Get Central Access Rules to be tested
        /// </summary>
        public string[] CentralAccessRuleNames
        {
            get;
            set;
        }

        /// <summary>
        /// Get Resource property to be tested
        /// </summary>
        public string[] ResourcepropertyNames
        {
            get;
            set;
        }
        #endregion

        public AzodTestConfig(ITestSite testSite)
        {
            this.TestSite = testSite;          
            
            this.ScriptPath = testSite.Properties["ScriptPath"];
            this.KdcDomainName = testSite.Properties["kdcDomainName"];
            this.KdcDomainNetbios = KdcDomainName.Split('.')[0];
            this.KdcName = testSite.Properties["kdcName"];
            this.KDCIP = testSite.Properties["KDCIP"];
            this.KdcAdminUser = testSite.Properties["KdcAdminUser"];
            this.KdcAdminPwd = testSite.Properties["KdcAdminPwd"];
            this.KdcClaimUser = testSite.Properties["KdcClaimUser"];
            this.KdcClaimUserPwd = testSite.Properties["KdcClaimUserPwd"];

            this.ApplicationServerName = testSite.Properties["ApplicationServerName"];
            this.ApplicationServerIP = testSite.Properties["ApplicationServerIP"];
           
            this.ClientComputerName = testSite.Properties["ClientComputerName"];
            this.ClientAdminUser = testSite.Properties["ClientAdminUser"];
            this.ClientAdminPwd = testSite.Properties["ClientAdminPwd"];
            this.ClientComputerIp = testSite.Properties["ClientComputerIp"];

            this.CrossForestName = testSite.Properties["CrossForestName"];
            this.CrossForestAdminUser = testSite.Properties["CrossForestAdminUser"];
            this.CrossForestAdminPwd = testSite.Properties["CrossForestAdminPwd"];
            this.CrossForestDCName = testSite.Properties["CrossForestDCName"];
            this.CrossForestDCIP = testSite.Properties["CrossForestDCIP"];
            this.CrossForestApplicationServerName = testSite.Properties["CrossForestApplicationServerName"];
            this.CrossForestApplicationServerIP = testSite.Properties["CrossForestApplicationServerIP"];
            this.CrossForestApplicationServerShareFolder = testSite.Properties["CrossForestApplicationServerShareFolder"];
            
            this.MaxSMB2DialectSupported = testSite.Properties["MaxSMB2DialectSupported"];
            this.SiteName = testSite.Properties["SiteName"];

            this.UncPath = testSite.Properties["UncPath"];
            this.FQDNUncPath = testSite.Properties["FQDNUncPath"];
          
            this.LocalCapFilePath = testSite.Properties["LocalCapFilePath"];
            this.ExpectedSequenceFilePath = testSite.Properties["ExpectedSequenceFilePath"];
            this.DriverLogPath = testSite.Properties["DriverLogPath"];

            this.CentralAccessPolicyNames = TestSite.Properties["CentralAccessPolicyNames"].Replace(" ", "").Split(',');
            this.CentralAccessRuleNames = TestSite.Properties["CentralAccessRuleNames"].Replace(" ", "").Split(',');
            this.ResourcepropertyNames = TestSite.Properties["ResourcepropertyNames"].Replace(" ", "").Split(',');
           
        }

        /// <summary>
        /// Get the expected message sequence file by test case name
        /// </summary>
        /// <param name="caseName">Test case name</param>
        public string GetMAExpectedSequenceFile(string caseName)
        {
            string expectFile = string.Format("{0}-ExpectedFrames.xml", caseName);
            return expectFile;
        }

        /// <summary>
        ///update the expected message files according to the PTF config file
        ///replace the variables in the xml file
        /// </summary>
        /// <param name="filename"></param>
        public void UpdateExpectedSequenceFile(string filename)
        {
            StreamReader reader = new StreamReader(filename);
            string content = reader.ReadToEnd();
            reader.Close();

            foreach (var property in this.GetType().GetProperties())
            {
                if (property.GetValue(this, null) is string[])
                {
                    string[] stringValues = (string[])property.GetValue(this, null);

                    for (int i = 0; i < stringValues.Length; i++ )
                    {                       
                        //for the properties are denfined with multiple values, split these values by ",".
                        string[] arrayValues = stringValues[i].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                        //replace the expect sequence file with splicted values by the index
                        //for example: {CentralAccessPolicyNames[0]} will be replaced by the first value of the property CentralAccessPolicyNames in ptfconfig file
                        content = content.Replace("{" + property.Name + "[" + i + "]}", stringValues[i]);                        
                    }
                }
                else
                {
                    content = Regex.Replace(content, "{" + property.Name + "}", property.GetValue(this, null).ToString());
                }
            }
            StreamWriter writer = new StreamWriter(filename, false);
            writer.Write(content);
            writer.Close();
        }
        
        /// <summary>
        /// Get computer roles defined in PTF config file 
        /// The returned dictionary will be the input for MA Adapter.
        /// The MA adapter can map the computer IP and computer name by the dictionary.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, EndpointRole>  GetEndpointRoles()
        {
            Dictionary<string, EndpointRole> endpointRoles =new Dictionary<string, EndpointRole>();
            EndpointRole clientRole = new EndpointRole();
            clientRole.Role = ClientComputerName;
            clientRole.ComputerName = ClientComputerName;
            clientRole.Ipv4 = ClientComputerIp;
            clientRole.Ipv6 = null;
            clientRole.MAC = null;

            EndpointRole APRole = new EndpointRole();
            APRole.Role = ApplicationServerName;
            APRole.ComputerName = ApplicationServerName;
            APRole.Ipv4 = ApplicationServerIP;
            APRole.Ipv6 = null;
            APRole.MAC = null;

            EndpointRole KDCRole = new EndpointRole();
            KDCRole.Role = KdcName;
            KDCRole.ComputerName = KdcName;
            KDCRole.Ipv4 = KDCIP;
            KDCRole.Ipv6 = null;
            KDCRole.MAC = null;

            EndpointRole CrossForestDCRole = new EndpointRole();
            CrossForestDCRole.Role = CrossForestDCName;
            CrossForestDCRole.ComputerName = CrossForestDCName;
            CrossForestDCRole.Ipv4 = CrossForestDCIP;
            CrossForestDCRole.Ipv6 = null;
            CrossForestDCRole.MAC = null;

            EndpointRole CrossForestAPRole = new EndpointRole();
            CrossForestAPRole.Role = CrossForestApplicationServerName;
            CrossForestAPRole.ComputerName = CrossForestApplicationServerName;
            CrossForestAPRole.Ipv4 = CrossForestApplicationServerIP;
            CrossForestAPRole.Ipv6 = null;
            CrossForestAPRole.MAC = null;

            endpointRoles.Add(clientRole.Role, clientRole);
            endpointRoles.Add(APRole.Role, APRole);
            endpointRoles.Add(KDCRole.Role, KDCRole);
            endpointRoles.Add(CrossForestDCRole.Role, CrossForestDCRole);
            endpointRoles.Add(CrossForestAPRole.Role, CrossForestAPRole);

            return endpointRoles;
        }
    }
}
