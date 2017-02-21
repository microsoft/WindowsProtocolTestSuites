// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    /// <summary>
    /// The configuration class to specify the test environment.
    /// </summary>
    public static class EnvironmentConfig
    {
        /// <summary>
        /// Enumeration to specify the machines in test environment.
        /// </summary>
        public enum Machine
        {
            /// <summary>
            /// None
            /// </summary>
            None,

            /// <summary>
            /// A non-DC client running test suite.
            /// </summary>
            Endpoint,

            /// <summary>
            /// The Global Catalog Server. By default, it's WritableDC1.
            /// </summary>
            GCServer,

            /// <summary>
            /// main AD DS DC that always exists and will hold AD LDS instances
            /// </summary>
            MainDC,

            /// <summary>
            /// The first writable DC.
            /// </summary>
            WritableDC1,

            /// <summary>
            /// The second writable DC.
            /// </summary>
            WritableDC2,

            /// <summary>
            /// A readonly DC in parent domain.
            /// </summary>
            RODC,

            /// <summary>
            /// A DC with PDC role in child domain.
            /// </summary>
            CDC,

            /// <summary>
            /// A cross-forest DC with PDC role.
            /// </summary>
            ExternalDC,

            /// <summary>
            /// A DC that owns PDC Master Role. By default, it's WritableDC1.
            /// </summary>
            PDC,

            /// <summary>
            /// A DC that owns Domain Naming Master Role. By default, it's WritableDC1.
            /// </summary>
            DC_With_DomainNamingMasterRole,

            /// <summary>
            /// A DC that owns Rid Allocation Master Role. By default, it's WritableDC1.
            /// </summary>
            DC_With_RidAllocationMasterRole,

            /// <summary>
            /// An invalid DC for AD DS
            /// </summary>
            InvalidDSDC,

            /// <summary>
            /// An invalid DC for AD LDS
            /// </summary>
            InvalidLDSDC
        }

        /// <summary>
        /// The user enumeration to specify user account
        /// </summary>
        public enum User
        {
            /// <summary>
            /// default value
            /// </summary>
            None,

            /// <summary>
            /// An invalid account
            /// </summary>
            InvalidAccount,

            /// <summary>
            /// normal user in parent domain
            /// </summary>
            ParentDomainUser,

            /// <summary>
            /// admin user in parent domain
            /// </summary>
            ParentDomainAdmin,

            /// <summary>
            /// normal user in child domain
            /// </summary>
            ChildDomainUser,

            /// <summary>
            /// admin user in child domain
            /// </summary>
            ChildDomainAdmin,

            /// <summary>
            /// normal user in external forest
            /// </summary>
            ExternalDomainUser,

            /// <summary>
            /// admin user in external forest
            /// </summary>
            ExternalDomainAdmin,

            /// <summary>
            /// admin user for LDS instances
            /// </summary>
            LDSDomainAdmin,

            /// <summary>
            /// machine account of the writableDC1
            /// </summary>
            MainDCAccount,

            /// <summary>
            /// machine account of the writableDC2
            /// </summary>
            WritableDC2Account,

            RODCMachineAccount
        }

        /// <summary>
        /// domain function level
        /// </summary>
        static Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr.DrsrDomainFunctionLevel funcLv;

        /// <summary>
        /// has initialized before
        /// </summary>
        static bool dataLoaded = false;

        /// <summary>
        /// PTF test site
        /// </summary>
        static ITestSite testSite = null;

        /// <summary>
        /// Set to true for test AD DS, false for AD LDS
        /// </summary>
        static bool testDS = true;

        /// <summary>
        /// ldap adapter
        /// </summary>
        static ILdapAdapter ldapAd = null;

        /// <summary>
        /// ip transportation object
        /// </summary>
        static DSNAME transport_obj;

        static bool init_succeed = true;
        
        /// <summary>
        /// Username of a non-admin user account, the account is created on the primary domain.
        /// </summary>
        public static string DomainUserName = "drsrnormaluser";

        /// <summary>
        /// DNS suffix of a mock domain.
        /// </summary>
        public static string InvalidDomainDSDNSName = "abc.com";

        /// <summary>
        /// Name of a mock LDS service. Not used.
        /// </summary>
        public static string InvalidDomainLDSDNSName = "abc1";       

        /// <summary>
        /// AppNC's DN. Not set.
        /// </summary>
        public static string AppNCDistinguishedName = "";

        /// <summary>
        /// new cloned DC's NetBIOS name.
        /// </summary>
        public static string ClonedDCNetbiosName = "cloned";

        /// <summary>
        /// ip transportation object
        /// </summary>
        public static DSNAME TransportObject
        {
            get
            {
                return transport_obj;
            }
        }

        /// <summary>
        /// Set to true for test AD DS, false for AD LDS
        /// </summary>
        public static bool TestDS
        {
            get
            {
                return testDS;
            }
        }

        /// <summary>
        /// get PTF test site
        /// </summary>
        public static ITestSite TestSite
        {
            get
            {
                return testSite;
            }
        }

        public static bool ExpectSuccess { get; set; }

        /// <summary>
        /// main DC object, same as writableDC1, it holds LDS instances
        /// </summary>
        public static AddsServer MainDC { get; set; }

        /// <summary>
        /// domain function level
        /// </summary>
        public static Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr.DrsrDomainFunctionLevel DomainLevel
        {
            get
            {
                return funcLv;
            }
        }

        /// <summary>
        /// whether environment is broken
        /// </summary>
        public static bool EnvBroken
        {
            get;
            set;
        }

        /// <summary>
        /// a store that save all outgoing DRS session context
        /// </summary>
        public static Dictionary<EnvironmentConfig.Machine, DrsrClientSessionContext> DrsContextStore = new Dictionary<EnvironmentConfig.Machine, DrsrClientSessionContext>();

        /// <summary>
        /// a store to save stable information of all machines/roles
        /// </summary>
        public static Dictionary<EnvironmentConfig.Machine, DsMachine> MachineStore = new Dictionary<Machine, DsMachine>();

        /// <summary>
        /// a store to save stable information of all test related user accounts
        /// </summary>
        public static Dictionary<EnvironmentConfig.User, DsUser> UserStore = new Dictionary<EnvironmentConfig.User, DsUser>();

        /// <summary>
        /// a store to save all Domain basic information
        /// </summary>
        public static Dictionary<DomainEnum, DsDomain> DomainStore = new Dictionary<DomainEnum, DsDomain>();

        /// <summary>
        /// to store physical machines only
        /// </summary>
        public static Dictionary<EnvironmentConfig.Machine, DsMachine> PhysicalMachineStore = new Dictionary<Machine, DsMachine>();

        /// <summary>
        /// load all infos
        /// </summary>
        /// <param name="site">PTD test site</param>
        public static void Initialize(ITestSite site)
        {
            if (site == null)
                throw new ApplicationException("TestSite could not be NULL.");

            //only initialize once
            if (dataLoaded)
            {
                if (!init_succeed)
                    site.Assert.Fail("EnvironmentConfig init failed in previous cases");
                return;
            }

            //try
            //{
            testSite = site;
            ldapAd = site.GetAdapter<ILdapAdapter>();
            ldapAd.Site = site;
            testDS = bool.Parse(site.Properties["MS_DRSR.TestDS"]);
            funcLv = (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr.DrsrDomainFunctionLevel)(ADCommonServerAdapter.Instance(testSite).DomainFunctionLevel);
            UseKerberos = bool.Parse(site.Properties["MS_DRSR.UseKerberos"]);
            UseNativeRpcLib = bool.Parse(site.Properties["MS_DRSR.UseNativeRpcLib"]);

            //create normal user account if it does not exist yet
            string serverName = ADCommonServerAdapter.Instance(testSite).PDCNetbiosName;
            string serverPort = ADCommonServerAdapter.Instance(testSite).ADDSPortNum;
            string domainNC = "DC=" + ADCommonServerAdapter.Instance(testSite).PrimaryDomainDnsName.Replace(".", ",DC=");
            string parentDN = string.Format("CN=Users,{0}", domainNC);
            string userDN = string.Format("CN={0},CN=Users,{1}", DomainUserName, domainNC);
            string userPassword = ADCommonServerAdapter.Instance(testSite).DomainUserPassword;
            if (!Utilities.IsObjectExist(userDN, serverName, serverPort))
            {
                Utilities.NewUser(serverName, serverPort, parentDN, DomainUserName, userPassword);
            }
            
            //please do all object intialization in this call following example
            InitUserObjects();
            InitDomainObjects();
            InitMachineObjects();
            FinishUserObjects();

            //load DS AppNC late due to rootDSE search issue
            if (testDS && !string.IsNullOrEmpty(AppNCDistinguishedName))
            {
                ((AddsDomain)DomainStore[DomainEnum.PrimaryDomain]).OtherNCs = new DSNAME[1];
                ((AddsDomain)DomainStore[DomainEnum.PrimaryDomain]).OtherNCs[0] = ldapAd.GetDsName((DsServer)MachineStore[Machine.WritableDC1], AppNCDistinguishedName).Value;
            }

            MainDC = (AddsServer)EnvironmentConfig.MachineStore[Machine.MainDC];
            DsServer s1 = (DsServer)EnvironmentConfig.MachineStore[Machine.WritableDC1];
            transport_obj = ldapAd.GetDsName(
                s1,
                "cn=ip,cn=inter-site transports,cn=sites,"
                + LdapUtility.ConvertUshortArrayToString(s1.Domain.ConfigNC.StringName)
                ).Value;

            DsUser dcAccount = new DsUser();
            dcAccount.Domain = s1.Domain;
            dcAccount.Username = s1.NetbiosName + "$";
            dcAccount.Password = ADCommonServerAdapter.Instance(testSite).PDCPassword;
            UserStore.Add(User.MainDCAccount, dcAccount);

            //not always need dc2
            if (EnvironmentConfig.MachineStore.ContainsKey(Machine.WritableDC2))
            {
                DsServer s2 = (DsServer)EnvironmentConfig.MachineStore[Machine.WritableDC2];

                DsUser dcAccount2 = new DsUser();
                dcAccount2.Domain = s2.Domain;
                dcAccount2.Username = s2.NetbiosName + "$";
                dcAccount2.Password = ADCommonServerAdapter.Instance(testSite).SDCPassword;
                UserStore.Add(User.WritableDC2Account, dcAccount2);
            }

            if (EnvironmentConfig.MachineStore.ContainsKey(Machine.RODC))
            {
                DsServer rodc = (DsServer)EnvironmentConfig.MachineStore[Machine.RODC];

                DsUser rodcAccount = new DsUser();
                rodcAccount.Domain = rodc.Domain;
                rodcAccount.Username = rodc.NetbiosName + "$";
                rodcAccount.Password = ADCommonServerAdapter.Instance(testSite).RODCPassword;
                UserStore.Add(User.RODCMachineAccount, rodcAccount);
            }
            //}
            //catch (Exception e)
            //{
            //    init_succeed = false;
            //    site.Assert.Fail("data initialization failed due to exception:" + e.InnerException == null ? e.Message : e.InnerException.Message);
            //}

            //alwasy only init once
            dataLoaded = true;

        }

        /// <summary>
        /// init objects for DomainStore
        /// </summary>
        static void InitDomainObjects()
        {
            #region domain objects

            DsDomain primaryDomain = ldapAd.GetDomainInfo(
                ADCommonServerAdapter.Instance(testSite).PDCNetbiosName + "." + ADCommonServerAdapter.Instance(testSite).PrimaryDomainDnsName + (testDS == true ? "" : ":" + testSite.Properties[Machine.WritableDC1.ToString() + ".LDSPort"]),
                UserStore[User.ParentDomainAdmin]
                );
            DomainStore.Add(DomainEnum.PrimaryDomain, primaryDomain);

            UserStore[User.ParentDomainAdmin].Domain = primaryDomain;

            //Should initialize child domain and trust domain object here
            if (testDS)
            {
                DsDomain InvalidDomain = new AddsDomain();
                InvalidDomain.FsmoRoleOwners = new Dictionary<FSMORoles, string>();
                InvalidDomain.DNSName = testSite.Properties[DomainEnum.InvalidDomain.ToString() + ".DS.DNSName"];
                DomainStore.Add(DomainEnum.InvalidDomain, InvalidDomain);
            }
            else
            {
                DsDomain InvalidDomain = new AdldsDomain();
                InvalidDomain.FsmoRoleOwners = new Dictionary<FSMORoles, string>();
                InvalidDomain.DNSName = testSite.Properties[DomainEnum.InvalidDomain.ToString() + ".LDS.DNSName"];
                DomainStore.Add(DomainEnum.InvalidDomain, InvalidDomain);
            }

            #endregion
        }

        /// <summary>
        /// init objects for MachineStore
        /// </summary>
        static void InitMachineObjects()
        {
            #region machine objects
            string[] enum_vals = Enum.GetNames(typeof(EnvironmentConfig.Machine));
            //first loop only initialize physical machines
            foreach (string s in enum_vals)
            {
                DsMachine machine = null;
                EnvironmentConfig.Machine val = (EnvironmentConfig.Machine)Enum.Parse(typeof(EnvironmentConfig.Machine), s);

                switch (val)
                {
                    case Machine.None:
                        continue;
                    case Machine.Endpoint:
                        //initialize client
                        machine = new ClientMachine();
                        machine.Domain = DomainStore[DomainEnum.PrimaryDomain];
                        machine.NetbiosName = ADCommonServerAdapter.Instance(testSite).ENDPOINTNetbiosName;
                        machine.DnsHostName = machine.NetbiosName + "." + machine.Domain.DNSName;
                        break;
                    case Machine.MainDC:
                        if (MachineStore.ContainsKey(Machine.WritableDC1))
                            machine = MachineStore[Machine.WritableDC1];
                        else
                        {
                            machine = ldapAd.GetDCInfo(ADCommonServerAdapter.Instance(testSite).PDCNetbiosName + "." + ADCommonServerAdapter.Instance(testSite).PrimaryDomainDnsName, UserStore[User.ParentDomainAdmin]);
                            if (testDS)
                                //do this in AD LDS will assign an AD LDS domain
                                machine.Domain = DomainStore[DomainEnum.PrimaryDomain];
                        }
                        break;
                    case Machine.RODC:
                        if (string.IsNullOrWhiteSpace(ADCommonServerAdapter.Instance(testSite).RODCNetbiosName))
                            continue;
                        try
                        {
                            machine = ldapAd.GetDCInfo(ADCommonServerAdapter.Instance(testSite).RODCNetbiosName + "." + ADCommonServerAdapter.Instance(testSite).PrimaryDomainDnsName, UserStore[User.ParentDomainAdmin]);
                            if (testDS)
                                //do this in AD LDS will assign an AD LDS domain
                                machine.Domain = DomainStore[DomainEnum.PrimaryDomain];
                            ((AddsServer)machine).IsRODC = true;
                        }
                        catch (System.DirectoryServices.Protocols.LdapException)
                        {
                            continue;                            
                        }
                        break;
                    case Machine.WritableDC1:
                        if (MachineStore.ContainsKey(Machine.MainDC))
                            machine = MachineStore[Machine.MainDC];
                        else
                        {
                            machine = ldapAd.GetDCInfo(ADCommonServerAdapter.Instance(testSite).PDCNetbiosName + "." + ADCommonServerAdapter.Instance(testSite).PrimaryDomainDnsName, UserStore[User.ParentDomainAdmin]);
                            if (testDS)
                                //do this in AD LDS will assign an AD LDS domain
                                machine.Domain = DomainStore[DomainEnum.PrimaryDomain];
                        }
                        break;
                    case Machine.WritableDC2:
                        if (string.IsNullOrWhiteSpace(ADCommonServerAdapter.Instance(testSite).SDCNetbiosName))
                            continue;
                        try
                        {
                            machine = ldapAd.GetDCInfo(ADCommonServerAdapter.Instance(testSite).SDCNetbiosName + "." + ADCommonServerAdapter.Instance(testSite).PrimaryDomainDnsName, UserStore[User.ParentDomainAdmin]);
                            if (testDS)
                                //do this in AD LDS will assign an AD LDS domain
                                machine.Domain = DomainStore[DomainEnum.PrimaryDomain];
                        }
                        catch (System.DirectoryServices.Protocols.LdapException)
                        {
                            continue;
                        }
                        break;
                    default:
                        //by default, it should be a DC

                        if (testDS)
                        {
                            if (string.IsNullOrEmpty(testSite.Properties[s + ".NetbiosName"]))
                                continue;

                            machine = ldapAd.GetDCInfo(testSite.Properties[s + ".NetbiosName"] + "." + GetDomainFromMachineType(val).DNSName, GetDomainFromMachineType(val).Admin);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(testSite.Properties[s + ".LDSPort"]))
                                continue;
                            machine = ldapAd.GetDCInfo(
                                ADCommonServerAdapter.Instance(testSite).PDCNetbiosName
                                + "."
                                + ADCommonServerAdapter.Instance(testSite).PrimaryDomainDnsName
                                + ":"
                                + testSite.Properties[s + ".LDSPort"],
                                UserStore[User.ParentDomainAdmin]);
                            ((AdldsServer)machine).DsaNetworkAddress =
                                ADCommonServerAdapter.Instance(testSite).PDCNetbiosName
                                + "."
                                + ADCommonServerAdapter.Instance(testSite).PrimaryDomainDnsName
                                + ":"
                                + ((AdldsServer)machine).DsaNetworkAddress;
                        }
                        ((DsServer)machine).IsWindows = bool.Parse(testSite.Properties[s + ".IsWindows"]);

                        if (machine != null)
                        {
                            switch (val)
                            {
                                case Machine.CDC:
                                    machine.Domain = DomainStore[DomainEnum.ChildDomain];
                                    break;
                                case Machine.ExternalDC:
                                    machine.Domain = DomainStore[DomainEnum.ExternalDomain];
                                    break;
                                default:
                                    machine.Domain = DomainStore[DomainEnum.PrimaryDomain];
                                    break;
                            }
                        }
                        break;
                }
                if (machine != null)
                {
                    MachineStore.Add(val, machine);

                    if (val != Machine.MainDC)
                        PhysicalMachineStore.Add(val, machine);
                }
            }

            //second loop initialize roles
            foreach (string s in enum_vals)
            {
                EnvironmentConfig.Machine val = (EnvironmentConfig.Machine)Enum.Parse(typeof(EnvironmentConfig.Machine), s);

                switch (val)
                {
                    case Machine.DC_With_DomainNamingMasterRole:
                        break;
                    case Machine.DC_With_RidAllocationMasterRole:
                        break;
                    case Machine.GCServer:
                        break;
                    case Machine.PDC:
                        break;
                    default:
                        continue;
                }
                MachineStore.Add(val, MachineStore[Machine.WritableDC1]);
            }

            #endregion
        }


        static void InitSiteObjects()
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1304:SpecifyCultureInfo", MessageId = "System.String.ToLower")]
        public static DsMachine FoundMatchedServerByDNSName(string dns)
        {
            Dictionary<EnvironmentConfig.Machine, DsMachine>.ValueCollection.Enumerator enumer = EnvironmentConfig.PhysicalMachineStore.Values.GetEnumerator();
            while (enumer.MoveNext())
            {
                if (enumer.Current.DnsHostName.ToLower() == dns.ToLower())
                    return enumer.Current;
            }

            return null;
        }

        /// <summary>
        /// Get domain from machine type. Used  for machine store initialization
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        static DsDomain GetDomainFromMachineType(Machine src)
        {
            switch (src)
            {
                case Machine.MainDC:
                    return DomainStore[DomainEnum.PrimaryDomain];
                case Machine.WritableDC1:
                    return DomainStore[DomainEnum.PrimaryDomain];
                case Machine.WritableDC2:
                    return DomainStore[DomainEnum.PrimaryDomain];
                case Machine.PDC:
                    return DomainStore[DomainEnum.PrimaryDomain];
                case Machine.RODC:
                    return DomainStore[DomainEnum.PrimaryDomain];
                case Machine.GCServer:
                    return DomainStore[DomainEnum.PrimaryDomain];
                case Machine.DC_With_DomainNamingMasterRole:
                    return DomainStore[DomainEnum.PrimaryDomain];
                case Machine.DC_With_RidAllocationMasterRole:
                    return DomainStore[DomainEnum.PrimaryDomain];
                case Machine.CDC:
                    return DomainStore[DomainEnum.ChildDomain];
                case Machine.ExternalDC:
                    return DomainStore[DomainEnum.ExternalDomain];
                case Machine.Endpoint:
                    return DomainStore[DomainEnum.PrimaryDomain];
                case Machine.InvalidDSDC:
                    return DomainStore[DomainEnum.InvalidDomain];
                case Machine.InvalidLDSDC:
                    return DomainStore[DomainEnum.InvalidDomain];
                default:
                    return null;
            }
        }

        static void FinishUserObjects()
        {
            string[] enum_vals = Enum.GetNames(typeof(EnvironmentConfig.User));
            foreach (string s in enum_vals)
            {
                User tmpU = (EnvironmentConfig.User)Enum.Parse(typeof(EnvironmentConfig.User), s);

                if (tmpU == User.None)
                    continue;

                switch (tmpU)
                {
                    case User.ExternalDomainAdmin:
                        if (DomainStore.ContainsKey(DomainEnum.ExternalDomain))
                            UserStore[tmpU].Domain = DomainStore[DomainEnum.ExternalDomain];
                        break;
                    case User.ExternalDomainUser:
                        if (DomainStore.ContainsKey(DomainEnum.ExternalDomain))
                            UserStore[tmpU].Domain = DomainStore[DomainEnum.ExternalDomain];
                        break;
                    case User.ChildDomainAdmin:
                        if (DomainStore.ContainsKey(DomainEnum.ChildDomain))
                            UserStore[tmpU].Domain = DomainStore[DomainEnum.ChildDomain];
                        break;
                    case User.ChildDomainUser:
                        if (DomainStore.ContainsKey(DomainEnum.ChildDomain))
                            UserStore[tmpU].Domain = DomainStore[DomainEnum.ChildDomain];
                        break;
                    case User.LDSDomainAdmin:
                        if (DomainStore.ContainsKey(DomainEnum.LDSDomain))
                            UserStore[tmpU].Domain = DomainStore[DomainEnum.LDSDomain];
                        break;
                    case User.ParentDomainUser:
                        UserStore[tmpU].Domain = DomainStore[DomainEnum.PrimaryDomain];
                        break;
                    case User.MainDCAccount:
                        continue;
                    case User.WritableDC2Account:
                        continue;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// finish objects for UserStore
        /// </summary>
        static void InitUserObjects()
        {
            #region user objects
            string[] enum_vals = Enum.GetNames(typeof(EnvironmentConfig.User));
            foreach (string s in enum_vals)
            {
                User tmpU = (EnvironmentConfig.User)Enum.Parse(typeof(EnvironmentConfig.User), s);

                if (tmpU == User.None)
                    continue;

                DsUser user = new DsUser();
                if (s.Contains("Admin"))
                    user.Username = ADCommonServerAdapter.Instance(testSite).DomainAdministratorName;
                else
                    user.Username = DomainUserName;
                user.Password = ADCommonServerAdapter.Instance(testSite).DomainUserPassword;

                //force load domain dns name for loading domain info via ldap adapter
                switch (tmpU)
                {
                    case User.ExternalDomainAdmin:
                        user.Domain = new AddsDomain();
                        user.Domain.NetbiosName = testSite.Properties[DomainEnum.ExternalDomain.ToString() + ".NetbiosName"];
                        user.Domain.DNSName = testSite.Properties[DomainEnum.ExternalDomain.ToString() + ".DNSName"];
                        break;
                    case User.LDSDomainAdmin:
                        user.Domain = new AdldsDomain();
                        user.Domain.DNSName = testSite.Properties[DomainEnum.LDSDomain.ToString() + ".DNSName"];
                        break;
                    case User.MainDCAccount:
                        //machine account not initialized yet
                        continue;
                    case User.WritableDC2Account:
                        continue;
                    case User.RODCMachineAccount:
                        continue;
                    default:
                        user.Domain = new AddsDomain();
                        user.Domain.NetbiosName = testSite.Properties[DomainEnum.PrimaryDomain.ToString() + ".NetbiosName"];
                        user.Domain.DNSName = ADCommonServerAdapter.Instance(testSite).PrimaryDomainDnsName;
                        break;
                }
                UserStore.Add((EnvironmentConfig.User)Enum.Parse(typeof(EnvironmentConfig.User), s), user);
            }
            #endregion
        }

        /// <summary>
        /// How much time should single DRS session last, in seconds
        /// </summary>
        public static int TestTimeout = 60000;

        /// <summary>
        /// use Kerberos in RPC, true by default,otherwise, use Negotiate
        /// </summary>
        public static bool UseKerberos = true;

        public static bool UseNativeRpcLib = false;

        public static void ResetMachineLdapConnections()
        {
            foreach (DsMachine machine in MachineStore.Values)
            {
                if (machine is DsServer)
                {
                    DsServer svr = machine as DsServer;
                    if (svr.LdapConn != null)
                    {
                        try
                        {
                            svr.LdapConn.Dispose();
                        }
                        catch
                        {
                        }
                    }
                    LdapUtility.CreateConnection(svr.DnsHostName, svr.Domain.Admin, ref svr, true);
                }
            }
        }

    }
}
