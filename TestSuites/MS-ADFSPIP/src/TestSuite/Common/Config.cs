// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Reflection;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    internal static class EnvironmentConfig
    {
        public static int    SUTMaxDelayBetweenOperations;
        public static bool   IsWindows;
        public static bool   IsWin2016;
        public static bool   TestDeployment;
        public static string SUTIP;
        public static string SUTShare;
        public static string SUTLocalAdmin;
        public static string SUTLocalAdminPassword;
        public static string DomainAdminUser;
        public static string DomainAdminPassword;
        public static string TLSServerCertificatePath;
        public static string TLSServerCertificatePassword;
        public static string ADFSFarmName;
        public static string ADFSFamrDNSName;
        public static string ADFSServerUrl;
        public static string ADFSFarmIP;
        public static string DomainName;
        public static string SUTConfigEntryKey_2012R2;
        public static string SUTConfigEntryKey_2016;
        public static string App1Url;
        public static string App1Name;
        public static string App2Url;
        public static string App2Name;
        public static string AppUser;
        public static string AppUserUpn;
        public static string AppUserPassword;
        public static string SUTDNS;
        public static string DataStorePath;
        public static string ProxyTrustCertificatePath;
        public static string ProxyTrustCertificatePassword;
        public static string DriverShare;
        public static string ADFSCert;
        public static string SignCert;
        public static string EncryptCert;
        public static string WebAppCert;
        public static string ClientCert;
        public static string PfxPassword;
        public static string MetadataTemplate_2012R2;
        public static string MetadataTemplate_2016;

        public static void LoadParameters(ITestSite site)
        {                      
            SUTMaxDelayBetweenOperations  = int .Parse(site.Properties["SUT.MaxDelaySeconds"]);
            IsWindows                     = bool.Parse(site.Properties["SUT.IsWindows"]);
            IsWin2016                     = bool.Parse(site.Properties["ADFS.IsWin2016"]);
            DomainName                    = site.Properties["Domain.DomainName"];
            DomainAdminUser               = site.Properties["Domain.Username"];
            DomainAdminPassword           = site.Properties["Domain.Password"];           
            ADFSFarmName                  = site.Properties["ADFS.AdfsName"];
            ADFSFamrDNSName               = site.Properties["ADFS.AdfsDns"];
            ADFSServerUrl                 = site.Properties["ADFS.AdfsUrl"];
            ADFSCert                      = site.Properties["ADFS.AdfsCert"];
            SignCert                      = site.Properties["ADFS.SignCert"];
            EncryptCert                   = site.Properties["ADFS.EncryptCert"];
            SUTConfigEntryKey_2012R2      = site.Properties["ADFS.StoreEntryKey_2012R2"];
            SUTConfigEntryKey_2016        = site.Properties["ADFS.StoreEntryKey_2016"];
            MetadataTemplate_2012R2       = site.Properties["ADFS.MetadataTemplate_2012R2"];
            MetadataTemplate_2016         = site.Properties["ADFS.MetadataTemplate_2016"];
            App1Url                       = site.Properties["WebApp.App1Url"];
            App2Url                       = site.Properties["WebApp.App2Url"];
            App1Name                      = site.Properties["WebApp.App1Name"];
            App2Name                      = site.Properties["WebApp.App2Name"];
            AppUser                       = site.Properties["WebApp.AppUserName"];
            AppUserUpn                    = site.Properties["WebApp.AppUserUpn"];
            AppUserPassword               = site.Properties["WebApp.AppUserPassword"];
            WebAppCert                    = site.Properties["WebApp.WebAppCert"];
            ClientCert                    = site.Properties["WebApp.ClientCert"];
            SUTIP                         = site.Properties["SUT.SutIPAddress"];
            SUTDNS                        = site.Properties["SUT.SutDns"];
            SUTLocalAdmin                 = site.Properties["SUT.Username"];
            SUTLocalAdminPassword         = site.Properties["SUT.Password"];
            DataStorePath                 = site.Properties["Common.DataStorePath"];
            PfxPassword                   = site.Properties["Common.PfxPassword"];
            DriverShare                   = site.Properties["Common.DriverShareFolder"];
            SUTShare                      = site.Properties["Common.SutShareFolder"];
            ADFSFarmIP                    = site.Properties["Common.RealADFSIP"];
            ProxyTrustCertificatePath     = site.Properties["Common.ProxyCert"];
            ProxyTrustCertificatePassword = PfxPassword;
            TLSServerCertificatePath      = ADFSCert;
            TLSServerCertificatePassword  = PfxPassword;
            TestDeployment                = false;
        }

        public static void CheckParameters(ITestSite site)
        {
            // retrieve all fields in EnvironmentConfig
            var fields = typeof (EnvironmentConfig).GetFields(
                BindingFlags.Public | BindingFlags.Static);

            // if there are fields with null, throw an exception
            foreach (var field in fields.Where(field => null == field.GetValue(null))) {
                site.Assert.Fail("Parameter {0} cannot be null", field.Name);
            }
        }
    }
}
